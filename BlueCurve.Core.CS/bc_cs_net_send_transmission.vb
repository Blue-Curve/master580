Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Threading
Imports System.Net
Imports System.Text
Imports System.IO
Imports ICSharpCode.SharpZipLib.Zip
Imports ICSharpCode.SharpZipLib.GZip
Imports ICSharpCode.SharpZipLib.Core
Imports ICSharpCode.SharpZipLib.Checksums
REM sftp
Imports WinSCP
Public Class bc_cs_net_send_channel

    Public Class bc_cs_net_send_file
        Public source_file As String
        Public control_file_sp As String
        Public dest_file As String
        Public embed As Boolean
        Public base64_file As String
        Public control_file As Boolean = False
        Public copy_dir As String
        Public control_file_content As String
    End Class

    Public Enum eTRANSFER_METHOD
        REST_POST = 1
        FTP = 2
        SFTP = 3
        FILE_COPY = 4
        SOAP_POST = 5
    End Enum


    Dim channel_url As String
    'Dim control_file_sp As String
    Dim files_sp As String
    Dim channel_user_name As String
    Dim channel_password As String
    Dim channel_fingerprint As String
    Dim channel_port As String
    Dim channel_dir As String
    Dim channel_certificate_file As String
    Dim soap_action As String
    Dim compress_output As Boolean
    Dim transfer_method As eTRANSFER_METHOD
    Dim _channel_id As Long
    Dim _doc_id As Long
    Dim files As New List(Of bc_cs_net_send_file)
    Dim proxymethod As String
    Dim proxyhost As String
    Dim proxyport As String
    Dim proxyuser As String
    Dim proxypassword As String
    Dim save_files_to_repos As Boolean = True
    Dim save_files_to_db As Boolean = False
    Friend success As String
    Const WAIT_RETRY = 3
    Const WAIT_DELAY = 10000
    Sub New(doc_id As Long, channel_id As Long)
        _channel_id = channel_id
        _doc_id = doc_id
    End Sub
    Friend Function send_net_send_channel(ByRef certificate As bc_cs_security.certificate) As String
        Dim oerr_count As Integer
        Try
            success = ""
            Dim db As New bc_cs_db_services
            Dim res As Object

            Dim files As New List(Of bc_cs_net_send_file)
            Dim rest_control_file_content As String
            REM get channel properities

            oerr_count = certificate.server_errors.Count

            res = db.executesql("exec dbo.bc_core_ns_get_channel_details " + CStr(_channel_id), certificate)
            If IsArray(res) Then
                If UBound(res, 2) = 0 Then
                    transfer_method = res(0, 0)
                    files_sp = res(1, 0)
                    channel_url = res(2, 0)
                    channel_user_name = res(3, 0)
                    channel_password = res(4, 0)
                    channel_fingerprint = res(5, 0)
                    channel_port = res(6, 0)
                    channel_dir = res(7, 0)
                    compress_output = res(8, 0)
                    Try

                        channel_certificate_file = res(9, 0)
                        soap_action = res(10, 0)

                        proxymethod = res(11, 0)
                        proxyhost = res(12, 0)
                        proxyport = res(13, 0)
                        proxyuser = res(14, 0)
                        proxypassword = res(15, 0)
                        save_files_to_repos = res(16, 0)
                        save_files_to_db = res(17, 0)
                    Catch ex As Exception

                    End Try

                    REM get files
                    res = db.executesql("exec " + files_sp + " " + CStr(_doc_id), certificate)
                    Dim cres As Object
                    If IsArray(res) Then
                        Dim fs As New bc_cs_file_transfer_services

                        Dim file As bc_cs_net_send_file

                        For i = 0 To UBound(res, 2)
                            file = New bc_cs_net_send_file
                            file.source_file = res(0, i)
                            file.source_file = Replace(file.source_file, "<repos>", bc_cs_central_settings.central_repos_path)
                            file.control_file_sp = res(1, i)
                            file.embed = res(2, i)
                            'If file.embed = False Then
                            file.dest_file = res(3, i)
                            file.dest_file = Replace(file.dest_file, "<repos>", bc_cs_central_settings.central_repos_path)
                            'End If
                            file.control_file = res(4, i)
                            file.copy_dir = res(5, i)
                            file.copy_dir = Replace(file.copy_dir, "<repos>", bc_cs_central_settings.central_repos_path)
                            If file.control_file = True Then
                                cres = db.executesql("exec " + file.control_file_sp + " " + CStr(_doc_id) + "," + CStr(_channel_id), certificate)
                                If IsArray(cres) Then
                                    If UBound(cres, 2) >= 0 Then
                                        For l = 0 To UBound(cres, 2)
                                            file.control_file_content = file.control_file_content + cres(0, l)
                                        Next
                                        If Len(file.control_file_content) >= 5 Then
                                            If Left(file.control_file_content, 5) = "Error" Then
                                                success = "control file generation failed: " + file.control_file_content + " (SP:" + file.control_file_sp + ")"
                                                Exit Function
                                            End If
                                        End If
                                        If file.embed = True Then
                                            If fs.check_document_exists(file.source_file, certificate) = True Then
                                                Dim byteDoc As Byte()

                                                If fs.write_document_to_bytestream(file.source_file, byteDoc, certificate, False) = False Then
                                                    success = "failed to read in file for channel " + CStr(_channel_id) + "file: " + file.source_file
                                                    Exit Function
                                                End If
                                                file.base64_file = Convert.ToBase64String(byteDoc)

                                                REM embded file in control file
                                                If InStr(file.control_file_content, "[embed" + file.source_file + "]") > 0 Then
                                                    file.control_file_content = file.control_file_content.Replace("[embed" + file.source_file + "]", file.base64_file)
                                                Else
                                                    success = "failed to find embedded file markup in control file " + CStr(_channel_id) + "file: " + bc_cs_central_settings.central_repos_path + file.source_file
                                                    Exit Function
                                                End If
                                            Else
                                                success = "failed to get file for channel " + CStr(_channel_id) + "file: " + file.source_file
                                                Exit Function
                                            End If

                                        End If
                                    Else
                                        success = "bc_cs_reach_distribute:send_net_send_channel:failed to generate control file 1: " + CStr(_channel_id) + "sp: " + file.source_file
                                        Exit Function
                                    End If
                                Else
                                    success = "bc_cs_reach_distribute:send_net_send_channel:failed to generate control file 2: " + CStr(_channel_id) + "sp: " + file.source_file
                                    Exit Function
                                End If

                            End If
                            If (bc_cs_central_settings.repos_db = True AndAlso bc_cs_central_settings.repos_file_system = False) Then


                            Else
                                If file.control_file = False AndAlso fs.check_document_exists(file.source_file, certificate) = False Then
                                    success = "failed to get file for channel " + CStr(_channel_id) + "file: " + file.source_file
                                    Exit Function
                                End If
                            End If
                            files.Add(file)
                        Next
                    Else
                        success = "failed to get files from sp channel: " + CStr(_channel_id) + "sp: " + files_sp
                    End If

                    If compress_output = True Then
                        If files_copy(files, certificate) = False Then
                            Exit Function
                        End If


                        If compress_dir_winzip(files(0).copy_dir + "\" + CStr(_doc_id), files, certificate) = False Then
                            Exit Function
                        End If

                        Dim zfile As New bc_cs_net_send_file
                        zfile.source_file = files(0).copy_dir + "\" + CStr(_doc_id) + ".zip"
                        zfile.dest_file = CStr(_doc_id) + ".zip"
                        REM compress this directory and set this as new file
                        files.Clear()
                        files.Add(zfile)
                    End If


                    Select Case transfer_method
                        Case eTRANSFER_METHOD.REST_POST

                            db.executesql("exec dbo.bc_core_ns_set_transmitted_info_for_doc " + CStr(_doc_id) + "," + CStr(_channel_id) + "," + CStr(transfer_method) + ",'" + CStr(channel_url) + "',''", certificate)

                            If json_post_send_config_data(certificate) = False Then
                                Dim fss As New bc_cs_string_services(success)
                                fss.delimit_apostrophies()
                                db.executesql("exec dbo.bc_core_ns_update_transmitted_info_for_doc " + CStr(_doc_id) + "," + CStr(_channel_id) + ",1,'" + fss.delimit_apostrophies + "'", certificate)

                                Exit Function

                            End If
                            Dim fs As New bc_cs_string_services(files(0).control_file_content)
                            db.executesql("exec dbo.bc_core_ns_set_transmitted_info_for_doc " + CStr(_doc_id) + "," + CStr(_channel_id) + "," + CStr(transfer_method) + ",'" + CStr(channel_url) + "','" + fs.delimit_apostrophies + "'", certificate)
                            'Dim ocomm As New bc_cs_activity_log("bc_cs_net_send_channel", "rest_post", bc_cs_activity_codes.COMMENTARY, "Doc_id: " + CStr(_doc_id) + ", Channel: " + CStr(_channel_id) + " uri: " + channel_url + ", content: " + control_file_content, certificate)
                            Dim rp As New bc_cs_ns_json_post(channel_url, files(0).control_file_content)
                            If rp.send(certificate) = False Then
                                success = rp.err_text
                                fs = New bc_cs_string_services(success)
                                db.executesql(" exec dbo.bc_core_ns_update_transmitted_info_for_doc " + CStr(_doc_id) + "," + CStr(_channel_id) + ",1,'" + fs.delimit_apostrophies + "'", certificate)

                                Exit Function
                            Else
                                fs = New bc_cs_string_services(rp.response_text)
                                res = db.executesql(" exec dbo.bc_core_ns_update_transmitted_info_for_doc " + CStr(_doc_id) + "," + CStr(_channel_id) + ",0,'" + fs.delimit_apostrophies + "'", certificate)
                                REM see if response is an explicit error
                                If IsArray(res) Then
                                    If UBound(res, 2) = 0 Then
                                        If res(0, 0) = 1 Then
                                            success = rp.response_text
                                        End If
                                    End If
                                End If
                            End If

                        Case eTRANSFER_METHOD.FTP
                            Dim fs As bc_cs_string_services
                            db.executesql("exec dbo.bc_core_ns_set_transmitted_info_for_doc " + CStr(_doc_id) + "," + CStr(_channel_id) + "," + CStr(transfer_method) + ",'" + CStr(channel_url) + "',''", certificate)
                            REM copy files to area if required
                            If files_copy(files, certificate) = False Then
                                Exit Function
                            End If


                            Dim rp As New bc_cs_ns_ftp(channel_url, channel_user_name, channel_password, channel_dir, channel_port)
                            If rp.send(files, certificate) = False Then
                                success = rp.err_text
                                fs = New bc_cs_string_services(success)
                                db.executesql(" exec dbo.bc_core_ns_update_transmitted_info_for_doc " + CStr(_doc_id) + "," + CStr(_channel_id) + ",1,'" + fs.delimit_apostrophies + "'", certificate)
                                Exit Function
                            Else
                                fs = New bc_cs_string_services(rp.response_text)
                                res = db.executesql(" exec dbo.bc_core_ns_update_transmitted_info_for_doc " + CStr(_doc_id) + "," + CStr(_channel_id) + ",0,'" + fs.delimit_apostrophies + "'", certificate)
                                REM see if response is an explicit error
                                If IsArray(res) Then
                                    If UBound(res, 2) = 0 Then
                                        If res(0, 0) = 1 Then
                                            success = rp.response_text
                                        End If
                                    End If
                                End If
                            End If

                        Case eTRANSFER_METHOD.SFTP
                            Dim fs As bc_cs_string_services
                            db.executesql("exec dbo.bc_core_ns_set_transmitted_info_for_doc " + CStr(_doc_id) + "," + CStr(_channel_id) + "," + CStr(transfer_method) + ",'" + CStr(channel_url) + "',''", certificate)
                            REM copy files to area if required
                            If files_copy(files, certificate) = False Then
                                Exit Function
                            End If


                            Dim rp As New bc_cs_ns_sftp(channel_url, channel_user_name, channel_password, channel_fingerprint, channel_port, channel_dir, proxymethod, proxyhost, proxyport, proxyuser, proxypassword, channel_certificate_file)
                            If rp.send(files, certificate) = False Then
                                success = rp.err_text
                                fs = New bc_cs_string_services(success)
                                db.executesql(" exec dbo.bc_core_ns_update_transmitted_info_for_doc " + CStr(_doc_id) + "," + CStr(_channel_id) + ",1,'" + fs.delimit_apostrophies + "'", certificate)
                                Exit Function
                            Else
                                fs = New bc_cs_string_services(rp.response_text)
                                res = db.executesql(" exec dbo.bc_core_ns_update_transmitted_info_for_doc " + CStr(_doc_id) + "," + CStr(_channel_id) + ",0,'" + fs.delimit_apostrophies + "'", certificate)
                                REM see if response is an explicit error
                                If IsArray(res) Then
                                    If UBound(res, 2) = 0 Then
                                        If res(0, 0) = 1 Then
                                            success = rp.response_text
                                        End If
                                    End If
                                End If
                            End If

                        Case eTRANSFER_METHOD.FILE_COPY
                            REM copy files to area if required
                            If files_copy(files, certificate) Then

                                If save_files_to_db = True Then
                                    Dim rdb As New bc_cs_db_repos
                                    For i = 0 To files.Count - 1
                                        rdb.write_file_to_reach_db(files(i).copy_dir + "\" + files(i).dest_file, files(i).dest_file, _doc_id, _channel_id, certificate)
                                        If save_files_to_repos = False Then
                                            Dim fs As New bc_cs_file_transfer_services
                                            fs.delete_file(files(i).copy_dir + "\" + files(i).dest_file)
                                        End If
                                    Next

                                End If

                                'If bc_cs_central_settings.repos_db = True Then
                                '    Dim rdb As New bc_cs_db_repos
                                '    For i = 0 To files.Count - 1
                                '        rdb.write_file_to_reach_db(files(i).copy_dir + "\" + files(i).dest_file, files(i).dest_file, _doc_id, _channel_id, certificate)
                                '        If bc_cs_central_settings.repos_file_system = False Then
                                '            Dim fs As New bc_cs_file_transfer_services
                                '            fs.delete_file(files(i).copy_dir + "\" + files(i).dest_file)
                                '        End If
                                '    Next
                                'End If
                                Exit Function
                            End If

                        Case eTRANSFER_METHOD.SOAP_POST

                            db.executesql("exec dbo.bc_core_ns_set_transmitted_info_for_doc " + CStr(_doc_id) + "," + CStr(_channel_id) + "," + CStr(transfer_method) + ",'" + CStr(channel_url) + "',''", certificate)

                            REM copy files to area if required
                            If files_copy(files, certificate) = False Then

                                Exit Function
                            End If

                            Dim fs As New bc_cs_string_services(files(0).control_file_content)

                            Dim sp As New bc_cs_ns_soap_post(channel_url, files(0).control_file_content, channel_certificate_file, channel_password, soap_action)
                            If sp.send(certificate) = False Then
                                success = sp.err_text
                                fs = New bc_cs_string_services(success)
                                db.executesql("exec dbo.bc_core_ns_update_transmitted_info_for_doc " + CStr(_doc_id) + "," + CStr(_channel_id) + ",0,'" + fs.delimit_apostrophies + "'", certificate)

                                Exit Function
                            Else
                                fs = New bc_cs_string_services(sp.response_text)
                                res = db.executesql(" exec dbo.bc_core_ns_update_transmitted_info_for_doc " + CStr(_doc_id) + "," + CStr(_channel_id) + ",0,'" + fs.delimit_apostrophies + "'", certificate)
                                REM see if response is an explicit error
                                If IsArray(res) Then
                                    If UBound(res, 2) = 0 Then
                                        If res(0, 0) = 1 Then
                                            success = sp.response_text
                                        End If
                                    End If
                                End If
                            End If

                        Case Else
                            success = "invalid transfer method for channel: " + CStr(_channel_id) + " method: " + transfer_method
                            Exit Function
                    End Select

                Else
                    success = "1 No channel info for channel: " + CStr(_channel_id)
                    Exit Function
                End If
            Else
                success = "2 No channel info for channel: " + CStr(_channel_id)
                Exit Function
            End If

            If success <> "" Then
                success = "bc_cs_reach_distribute:send_net_send_channel: " + success
            End If
            REM file system, or db option per channel
            If save_files_to_db = True Then
                Dim rdb As New bc_cs_db_repos
                For i = 0 To files.Count - 1
                    rdb.write_file_to_reach_db(files(i).copy_dir + "\" + files(i).dest_file, files(i).dest_file, _doc_id, _channel_id, certificate)
                    If save_files_to_repos = False Then
                        Dim fs As New bc_cs_file_transfer_services
                        fs.delete_file(files(i).copy_dir + files(i).dest_file)
                    End If
                Next

            End If

            'If bc_cs_central_settings.repos_db = True Then
            '    Dim rdb As New bc_cs_db_repos
            '    For i = 0 To files.Count - 1
            '        rdb.write_file_to_reach_db(files(i).copy_dir + files(i).dest_file, files(i).dest_file, _doc_id, _channel_id, certificate)
            '        If bc_cs_central_settings.repos_file_system = False Then
            '            Dim fs As New bc_cs_file_transfer_services
            '            fs.delete_file(files(i).copy_dir + files(i).dest_file)
            '        End If
            '    Next
            'End If
            'check_write_to_repos(certificate)

        Catch ex As Exception
            success = "bc_cs_reach_distribute:send_net_send_channel: " + ex.Message
            Dim err As New bc_cs_error_log("bc_cs_net_send_channel", "send_net_send_channel", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            If certificate.server_errors.Count > oerr_count Then
                send_net_send_channel = certificate.server_errors(oerr_count)
            Else
                send_net_send_channel = success
            End If

        End Try
    End Function
 
    Private Function files_copy(ByRef files As List(Of bc_cs_net_send_file), ByRef certificate As bc_cs_security.certificate)
        Try
            files_copy = False
          
            If bc_cs_central_settings.repos_db = True And bc_cs_central_settings.repos_file_system = False Then
                For i = 0 To files.Count - 1
                    If files(i).copy_dir <> "" Then
                        REM create dirctory
                        Try
                            System.IO.Directory.CreateDirectory(files(i).copy_dir)
                        Catch ex As Exception
                            success = "Failed to create directory: " + files(i).copy_dir
                            Exit Function
                        End Try

                        REM if control file create now on filesystem
                        If files(i).control_file = True Then
                            Try
                                Dim sw As New StreamWriter(files(i).copy_dir + "\" + files(i).dest_file)
                                sw.WriteLine(files(i).control_file_content)
                                sw.Close()
                            Catch ex As Exception
                                success = "Failed to create write control file : " + files(i).dest_file + ": " + ex.Message
                                Exit Function
                            End Try
                        Else
                            Try
                                REM get file from repos
                                Dim rdb As New bc_cs_db_repos
                                Dim obyte As Byte()
                                Dim dfn As String
                                dfn = Replace(files(i).source_file, bc_cs_central_settings.central_repos_path, "")

                                obyte = rdb.read_file_from_db(dfn, certificate)
                                If IsNothing(obyte) Then
                                    If bc_cs_central_settings.repos_html_db = True Then
                                        dfn = Replace(dfn, "email_previews\", "")
                                        obyte = rdb.read_html_file_from_db(dfn, certificate)
                                        If IsNothing(obyte) Then
                                            success = "could not find file in db: " + dfn
                                            Exit Function
                                        Else
                                            Dim fs As New bc_cs_file_transfer_services
                                            fs.write_bytestream_to_document(files(i).copy_dir + "\" + files(i).dest_file, obyte, certificate)
                                        End If
                                    Else
                                        FileCopy(files(i).source_file, files(i).copy_dir + "\" + files(i).dest_file)

                                    End If
                                Else
                                    Dim fs As New bc_cs_file_transfer_services
                                    fs.write_bytestream_to_document(files(i).copy_dir + "\" + files(i).dest_file, obyte, certificate)
                                End If


                                'FileCopy(files(i).source_file, files(i).copy_dir + "\" + files(i).dest_file)
                            Catch ex As Exception
                                success = "Failed to copy file : " + files(i).source_file + " to: " + files(i).dest_file + ": " + ex.Message
                                Exit Function
                            End Try
                        End If


                        REM change source file location to this area
                        files(i).source_file = files(i).copy_dir + "\" + files(i).dest_file
                    End If
                Next

                files_copy = True



            Else
                For i = 0 To files.Count - 1
                    If files(i).copy_dir <> "" Then
                        REM create dirctory
                        Try
                            System.IO.Directory.CreateDirectory(files(i).copy_dir)
                        Catch ex As Exception
                            success = "Failed to create directory: " + files(i).copy_dir
                            Exit Function
                        End Try

                        REM if control file create now on filesystem
                        If files(i).control_file = True Then
                            Try
                                Dim sw As New StreamWriter(files(i).copy_dir + "\" + files(i).dest_file)
                                sw.WriteLine(files(i).control_file_content)
                                sw.Close()
                            Catch ex As Exception
                                success = "Failed to create write control file : " + files(i).dest_file + ": " + ex.Message
                                Exit Function
                            End Try
                        Else
                            Try
                                FileCopy(files(i).source_file, files(i).copy_dir + "\" + files(i).dest_file)
                            Catch ex As Exception
                                success = "Failed to copy file : " + files(i).source_file + " to: " + files(i).dest_file + ": " + ex.Message
                                Exit Function
                            End Try
                        End If


                        REM change source file location to this area
                        files(i).source_file = files(i).copy_dir + "\" + files(i).dest_file
                    End If
                Next

                files_copy = True
            End If
        Catch ex As Exception
            success = "bc_cs_reach_distribute:send_net_send_channel:file_copy " + ex.Message
            Dim err As New bc_cs_error_log("bc_cs_net_send_channel", "send_net_send_channel", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        End Try
    End Function
    REM different net send tranmission types
    REM json via rest post
    Private Function json_post_send_config_data(ByRef certificate As bc_cs_security.certificate) As Boolean

        Try
            Dim db As New bc_cs_db_services
            Dim res, ares As Object

            json_post_send_config_data = False

            res = db.executesql("exec dbo.bc_core_ns_get_rp_config_data " + CStr(_doc_id), certificate)
            REM see if response is an explicit error
            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    'Dim ocomm As New bc_cs_activity_log("aaa", "bbb", bc_cs_activity_codes.COMMENTARY, CStr(i) + ":" + CStr(UBound(res, 2)), certificate)



                    Dim json As String
                    json = CStr(res(1, i))
                    'ocomm = New bc_cs_activity_log("xxx", "bbb", bc_cs_activity_codes.COMMENTARY, CStr(i) + ":" + CStr(UBound(res, 2)), certificate)

                    Dim rp As New bc_cs_ns_json_post(CStr(res(0, i)), json)
                    'ocomm = New bc_cs_activity_log("zz", "bbb", bc_cs_activity_codes.COMMENTARY, CStr(i) + ":" + CStr(UBound(res, 2)), certificate)

                    If rp.send(certificate) = False Then
                        success = rp.err_text
                        Exit Function
                    Else
                        REM write repsonse and check valid
                        Dim fs As New bc_cs_string_services(rp.response_text)
                        rp.response_text = fs.delimit_apostrophies
                        fs = New bc_cs_string_services(json)

                        json = fs.delimit_apostrophies()
                        ares = db.executesql("exec dbo.bc_core_ns_write_rp_config_response " + CStr(_doc_id) + ",'" + rp.response_text + "','" + json + "','" + CStr(res(2, i)) + "'", certificate)
                        If IsArray(ares) Then
                            If UBound(ares, 2) = 0 Then
                                If ares(0, 0) = 1 Then
                                    success = ares(1, 0)
                                    Exit Function
                                End If
                            End If
                        End If
                    End If
                Next
            End If

            json_post_send_config_data = True

        Catch ex As Exception
            success = "error: json_post_send_config_data: " + ex.Message
            json_post_send_config_data = False
        End Try
    End Function


    Private Function compress_dir_winzip(filename As String, ByRef files As List(Of bc_cs_net_send_file), ByRef certificate As bc_cs_security.certificate) As Boolean
        Try
            compress_dir_winzip = False
            REM compress
            Dim objCrc32 As New Crc32
            Dim zos As ZipOutputStream

            Dim k As New StreamWriter(filename + ".zip")
            zos = New ZipOutputStream(k.BaseStream) 'your


            For i = 0 To files.Count - 1
                Dim strmFile As FileStream = File.OpenRead(files(i).copy_dir + "\" + files(i).dest_file)

                Dim abyBuffer(CInt(strmFile.Length - 1)) As Byte

                strmFile.Read(abyBuffer, 0, abyBuffer.Length)
                Dim objZipEntry As ZipEntry = New ZipEntry(files(i).dest_file)
                objZipEntry.DateTime = DateTime.Now
                objZipEntry.Size = strmFile.Length
                strmFile.Close()
                objCrc32.Reset()
                objCrc32.Update(abyBuffer)
                objZipEntry.Crc = objCrc32.Value
                zos.PutNextEntry(objZipEntry)
                zos.Write(abyBuffer, 0, abyBuffer.Length)
            Next

            zos.Finish()
            zos.Close()
            compress_dir_winzip = True
        Catch ex As Exception
            success = "error: compress_dir_winzip: " + ex.Message
        End Try
    End Function

    Public Class bc_cs_ns_ftp
        Dim _uri As String
        Dim _username As String
        Dim _password As String
        Dim _dir As String
        Dim _port As Integer
        Public err_text As String
        Public response_text As String

        Public Sub New(uri As String, username As String, password As String, dir As String, port As Integer)
            _uri = uri
            _username = username
            _password = password
            _dir = dir
            _port = port
        End Sub
        Public Function send(files As List(Of bc_cs_net_send_file), certificate As bc_cs_security.certificate) As Boolean
            Try
                send = False
                For i = 0 To files.Count - 1
                    If send_file(files(i).source_file, files(i).dest_file, _dir, certificate) = False Then
                        Dim ocomm As New bc_cs_activity_log("bc_cs_ns_ftp", "send", bc_cs_activity_codes.COMMENTARY, "attemping to ftp source: " + files(i).source_file + " to dest: " + files(i).dest_file, certificate)
                        Exit Function
                    End If
                Next
                send = True
            Catch ex As Exception
                err_text = "ftp send failed end: " + ex.Message
            End Try
        End Function

        Public Function send_file(source_file As String, destination_file As String, dir As String, certiciate As bc_cs_security.certificate) As Boolean
            Try



                send_file = False
                Dim _UploadPath As String
                Dim _FileInfo As New System.IO.FileInfo(source_file)
                If dir <> "" Then
                    _UploadPath = _uri + "\" + dir + "\" + destination_file
                Else
                    _UploadPath = _uri + "\" + destination_file
                End If

                Dim clsRequest As System.Net.FtpWebRequest = CType(System.Net.FtpWebRequest.Create(New Uri(_UploadPath)), System.Net.FtpWebRequest)




                clsRequest.Credentials = New System.Net.NetworkCredential(_username, _password)
                clsRequest.Method = System.Net.WebRequestMethods.Ftp.UploadFile

                clsRequest.KeepAlive = False
                clsRequest.Timeout = 60000
                clsRequest.Method = System.Net.WebRequestMethods.Ftp.UploadFile
                clsRequest.UseBinary = True
                clsRequest.UsePassive = False
                ' Notify the server about the size of the uploaded file
                clsRequest.ContentLength = _FileInfo.Length


                Dim buffLength As Long = 100000000
                Dim buff(buffLength - 1) As Byte

                ' Opens a file stream (System.IO.FileStream) to read the file to be uploaded
                Dim _FileStream As System.IO.FileStream = _FileInfo.OpenRead()


                ' Stream to which the file to be upload is written
                Dim _Stream As System.IO.Stream = clsRequest.GetRequestStream()

                ' Read from the file stream 2kb at a time
                Dim contentLen As Integer = _FileStream.Read(buff, 0, buffLength)

                ' Till Stream content ends
                Do While contentLen <> 0
                    ' Write Content from the file stream to the FTP Upload Stream
                    _Stream.Write(buff, 0, contentLen)
                    contentLen = _FileStream.Read(buff, 0, buffLength)
                Loop

                ' Close the file stream and the Request Stream
                _Stream.Close()
                _Stream.Dispose()
                _FileStream.Close()
                _FileStream.Dispose()
                send_file = True
            Catch ex As Exception
                err_text = "ftp failed to send file " + source_file + ": " + destination_file + ": " + ex.Message
            End Try
        End Function

    End Class


    Public Class bc_cs_ns_sftp
        Dim _uri As String
        Dim _username As String
        Dim _password As String
        Dim _fingerprint As String
        Dim _port As Integer
        Dim _dir As String

        Dim _proxymethod As String
        Dim _proxyhost As String
        Dim _proxyport As String
        Dim _proxyuser As String
        Dim _proxypassword As String

        Dim _cert_file As String

        Public err_text As String
        Public response_text As String

        Public Sub New(uri As String, username As String, password As String, fingerprint As String, port As Integer, dir As String, proxymethod As String, proxyhost As String, proxyport As String, proxyuser As String, proxypassword As String, cert_location As String)
            _uri = uri
            _username = username
            _password = password
            _fingerprint = fingerprint
            _port = port
            _dir = dir

            _proxymethod = proxymethod
            _proxyhost = proxyhost
            _proxyport = proxyport
            _proxyuser = proxyuser
            _proxypassword = proxypassword

            _cert_file = cert_location

        End Sub
        Public Function send(files As List(Of bc_cs_net_send_file), certificate As bc_cs_security.certificate) As Boolean
            Try
                send = False
                For i = 0 To files.Count - 1
                    Dim ocomm As New bc_cs_activity_log("bc_cs_ns_sftp", "send", bc_cs_activity_codes.COMMENTARY, "attempting to sftp source: " + files(i).source_file + " to dest: " + files(i).dest_file, certificate)
                    'If send_file(files(i).source_file, files(i).dest_file, certificate) = False Then
                    '    Exit Function
                    'End If
                Next
                send = True
            Catch ex As Exception
                err_text = "sftp send failed end: " + ex.Message
            End Try
        End Function

        'Public Function send_file(source_file As String, destination_file As String, certiciate As bc_cs_security.certificate) As Boolean
        '    send_file = False
        '    Try

        '        ' Setup session options
        '        Dim sessionOptions As New SessionOptions

        '        If _cert_file <> "" Then
        '            With sessionOptions
        '                'SFTP with private key
        '                .Protocol = Protocol.Sftp
        '                .HostName = _uri
        '                .UserName = _username
        '                .PortNumber = _port
        '                .SshHostKeyFingerprint = _fingerprint
        '                .SshPrivateKeyPath = _cert_file
        '                .PrivateKeyPassphrase = _password
        '            End With
        '        Else
        '            With sessionOptions
        '                'SFTP with user password
        '                .Protocol = Protocol.Sftp
        '                .HostName = _uri
        '                .UserName = _username
        '                .Password = _password
        '                .SshHostKeyFingerprint = _fingerprint
        '                .PortNumber = _port
        '            End With
        '        End If

        '        'Proxy settings
        '        If _proxymethod <> "" Then
        '            If _proxymethod <> "" Then sessionOptions.AddRawSettings("ProxyMethod", _proxymethod)
        '            If _proxyhost <> "" Then sessionOptions.AddRawSettings("ProxyHost", _proxyhost)
        '            If _proxyport <> "" Then sessionOptions.AddRawSettings("ProxyPort", _proxyport)
        '            If _proxyuser <> "" Then sessionOptions.AddRawSettings("ProxyUsername", _proxyuser)
        '            If _proxypassword <> "" Then sessionOptions.AddRawSettings("ProxyPassword", _proxypassword)
        '        End If

        '        Using session As New Session
        '            ' Connect
        '            session.Open(sessionOptions)

        '            ' Upload files
        '            Dim transferOptions As New TransferOptions
        '            transferOptions.TransferMode = TransferMode.Binary
        '            transferOptions.ResumeSupport.State = TransferResumeSupportState.Off
        '            transferOptions.PreserveTimestamp = False

        '            Dim transferResult As TransferOperationResult

        '            If _dir = "" Then
        '                transferResult = session.PutFiles(source_file, "\" + destination_file, False, transferOptions)
        '            Else
        '                transferResult = session.PutFiles(source_file, _dir + destination_file, False, transferOptions)
        '            End If

        '            'Log Any errors
        '            For Each failure In transferResult.Failures
        '                Dim ocomm As New bc_cs_activity_log("bc_cs_ns_sftp", "send", bc_cs_activity_codes.COMMENTARY, "sftp failure: " + failure.message, certiciate)
        '            Next

        '            ' Throw on any error
        '            transferResult.Check()
        '            session.Close()

        '            Dim ocomms As New bc_cs_activity_log("bc_cs_ns_sftp", "send", bc_cs_activity_codes.COMMENTARY, "sftp file sent: " + _dir + destination_file, certiciate)

        '        End Using

        '        send_file = True


        '    Catch ex As Exception
        '        err_text = "sftp failed to send file " + source_file + ": " + destination_file + ": " + ex.Message
        '    End Try
        'End Function

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
        End Sub
    End Class

End Class

