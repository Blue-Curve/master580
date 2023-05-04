Imports System.Data.SqlClient
Imports System.Data
Public Class bc_cs_db_repos
    Public Function write_file_to_db(fullfilename As String, filename As String, doc_id As String, certificate As bc_cs_security.certificate) As Boolean
        Try
            REM read file in
            Dim docbyte As Byte()
            Dim fs As New bc_cs_file_transfer_services
            fs.write_document_to_bytestream(fullfilename, docbyte, certificate)
            Dim db As New bc_cs_db_services

            Dim sqlcn As New SqlConnection
            sqlcn.ConnectionString = db.GetConnectionString(certificate)
            sqlcn.Open()
            Dim cmd As New SqlCommand("dbo.bc_core_write_file_to_db", sqlcn)
            cmd.CommandTimeout = bc_cs_central_settings.ado_timeout
            cmd.CommandType = Data.CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@filename", filename)
            cmd.Parameters.Add("@file", SqlDbType.VarBinary, docbyte.Length).Value = docbyte


            cmd.ExecuteNonQuery()
            sqlcn.Close()

            Return True

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_cs_db_repos", "write_file_to_db", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            Return False
        End Try

    End Function
    Public Function read_file_from_db(filename As String, certificate As bc_cs_security.certificate) As Byte()
        Try


            Dim db As New bc_cs_db_services
            Dim sqlcn As New SqlConnection
            sqlcn.ConnectionString = db.GetConnectionString(certificate)
            sqlcn.Open()





            Dim cmd As New SqlCommand("dbo.bc_core_read_file_from_db", sqlcn)
            cmd.CommandTimeout = bc_cs_central_settings.ado_timeout
            cmd.CommandType = Data.CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@filename", filename)
            Dim ocomm As New bc_cs_activity_log("bc_cs_db_repos", "read_file_from_db", bc_cs_activity_codes.COMMENTARY, "Filename: " + filename, certificate)

            Dim dr As SqlDataReader = Nothing
            dr = cmd.ExecuteReader(CommandBehavior.CloseConnection)


            Dim row_Count As Integer = 0
            Dim obj As Byte()
            Dim oresults As New List(Of Object)
            If dr.HasRows = True Then
                Do While dr.Read
                    obj = dr.GetValue(0)
                Loop
            End If
            dr.Close()
            If (obj Is Nothing) Then
                ocomm = New bc_cs_activity_log("bc_cs_db_repos", "read_file_from_db", bc_cs_activity_codes.COMMENTARY, "Couldnt Find File in Db:" + filename, certificate)
            End If
            Return obj
            sqlcn.Close()


        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_cs_db_repos", "read_file_from_db", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            Return Nothing
        End Try

    End Function

    Public Function read_html_file_from_db(filename As String, certificate As bc_cs_security.certificate) As Byte()
        Try


            Dim db As New bc_cs_db_services
            Dim sqlcn As New SqlConnection
            sqlcn.ConnectionString = db.GetConnectionString(certificate)
            sqlcn.Open()





            Dim cmd As New SqlCommand("dbo.bc_core_get_html_from_repos_fn", sqlcn)
            cmd.CommandTimeout = bc_cs_central_settings.ado_timeout
            cmd.CommandType = Data.CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@filename", filename)
            Dim ocomm As New bc_cs_activity_log("bc_cs_db_repos", "read_html_file_from_db", bc_cs_activity_codes.COMMENTARY, "doc_id: " + filename, certificate)

            Dim dr As SqlDataReader = Nothing
            dr = cmd.ExecuteReader(CommandBehavior.CloseConnection)


            Dim row_Count As Integer = 0
            Dim obj As Byte()
            Dim oresults As New List(Of Object)
            If dr.HasRows = True Then
                Do While dr.Read
                    obj = dr.GetValue(0)
                Loop
            End If
            dr.Close()
            If (obj Is Nothing) Then
                ocomm = New bc_cs_activity_log("bc_cs_db_repos", "read_html_file_from_db", bc_cs_activity_codes.COMMENTARY, "Couldnt Find html File in Db:" + filename, certificate)
            End If
            Return obj
            sqlcn.Close()


        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_cs_db_repos", "read_html_file_from_db", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            Return Nothing
        End Try

    End Function


    Public Function take_revision_in_db(filename As String, stage_id As Integer, certificate As bc_cs_security.certificate) As Integer
        take_revision_in_db = 0
        Try
            Dim db As New bc_cs_db_services
            Dim sql As String
            sql = "exec dbo.bc_core_write_revision_file_to_db '" + filename + "'," + CStr(stage_id)
            Dim res As Object
            res = db.executesql(sql, certificate)
            If IsArray(res) Then
                take_revision_in_db = res(0, 0)
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_cs_db_repos", "take_revision_in_db", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            Return Nothing
        End Try
    End Function

    Public Function write_revision_file_to_db(fullfilename As String, filename As String, doc_id As String, certificate As bc_cs_security.certificate) As Boolean
        Try
            REM write revision file in
            Dim docbyte As Byte()
            Dim fs As New bc_cs_file_transfer_services
            fs.write_document_to_bytestream(fullfilename, docbyte, certificate)
            Dim db As New bc_cs_db_services

            Dim sqlcn As New SqlConnection
            sqlcn.ConnectionString = db.GetConnectionString(certificate)
            sqlcn.Open()
            Dim cmd As New SqlCommand("dbo.bc_core_write_docrevision_file_to_db", sqlcn)
            cmd.CommandTimeout = bc_cs_central_settings.ado_timeout
            cmd.CommandType = Data.CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@filename", filename)
            cmd.Parameters.Add("@file", SqlDbType.VarBinary, docbyte.Length).Value = docbyte

            cmd.ExecuteNonQuery()
            sqlcn.Close()

            Return True

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_cs_db_repos", "write_revision_file_to_db", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            Return False
        End Try

    End Function

    Public Function read_revision_file_from_db(filename As String, certificate As bc_cs_security.certificate) As Byte()
        Try


            Dim db As New bc_cs_db_services
            Dim sqlcn As New SqlConnection
            sqlcn.ConnectionString = db.GetConnectionString(certificate)
            sqlcn.Open()

            Dim cmd As New SqlCommand("dbo.bc_core_read_revision_file_from_db", sqlcn)
            cmd.CommandTimeout = bc_cs_central_settings.ado_timeout
            cmd.CommandType = Data.CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@filename", filename)
            Dim ocomm As New bc_cs_activity_log("bc_cs_db_repos", "read_revision_file_from_db", bc_cs_activity_codes.COMMENTARY, "Filename: " + filename, certificate)

            Dim dr As SqlDataReader = Nothing
            dr = cmd.ExecuteReader(CommandBehavior.CloseConnection)


            Dim row_Count As Integer = 0
            Dim obj As Byte()
            Dim oresults As New List(Of Object)
            If dr.HasRows = True Then
                ocomm = New bc_cs_activity_log("bc_cs_db_repos", "read_revision_file_from_db", bc_cs_activity_codes.COMMENTARY, "hi", certificate)
                Do While dr.Read
                    ocomm = New bc_cs_activity_log("bc_cs_db_repos", "read_revision_file_from_db", bc_cs_activity_codes.COMMENTARY, "hi1", certificate)
                    obj = dr.GetValue(0)
                Loop
            End If
            dr.Close()
            If (obj Is Nothing) Then
                ocomm = New bc_cs_activity_log("bc_cs_db_repos", "read_revision_file_from_db", bc_cs_activity_codes.COMMENTARY, "Couldnt Find File in Db:" + filename, certificate)
            End If
            Return obj
            sqlcn.Close()


        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_cs_db_repos", "read_revision_file_from_db", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            Return Nothing
        End Try

    End Function
    Public Function revert_file(filename As String, doc_id As Long, certificate As bc_cs_security.certificate) As Boolean
        Try
            revert_file = False
            Dim db As New bc_cs_db_services
            Dim sql As String
            sql = "exec dbo.bc_core_revert_from_db '" + filename + "'," + CStr(doc_id)
            Dim res As Object
            res = db.executesql(sql, certificate)
            If IsArray(res) Then
                If res(0, 0) = 0 Then
                    revert_file = True
                End If
            End If


        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_cs_db_repos", " revert_file", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)

        End Try

    End Function

    Public Function write_html_preview_to_db(fullfilename As String, filename As String, doc_id As String, certificate As bc_cs_security.certificate) As Boolean
        Try
            REM read file in
            Dim docbyte As Byte()
            Dim fs As New bc_cs_file_transfer_services
            fs.write_document_to_bytestream(fullfilename, docbyte, certificate)
            Dim db As New bc_cs_db_services

            Dim sqlcn As New SqlConnection
            sqlcn.ConnectionString = db.GetConnectionString(certificate)
            sqlcn.Open()
            Dim cmd As New SqlCommand("dbo.bc_core_write_html_preview_to_db", sqlcn)
            cmd.CommandTimeout = bc_cs_central_settings.ado_timeout
            cmd.CommandType = Data.CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@filename", filename)
            cmd.Parameters.Add("@file", SqlDbType.VarBinary, docbyte.Length).Value = docbyte


            cmd.ExecuteNonQuery()
            sqlcn.Close()

            Return True

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_cs_db_repos", "write_html_preview_to_db", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            Return False
        End Try
    End Function

    Public Function write_component_to_db(filename As String, doc_id As String, docbyte As Byte(), certificate As bc_cs_security.certificate) As Boolean
        Try
            
            Dim db As New bc_cs_db_services

            Dim sqlcn As New SqlConnection
            sqlcn.ConnectionString = db.GetConnectionString(certificate)
            sqlcn.Open()
            Dim cmd As New SqlCommand("dbo.bc_core_write_component_to_db", sqlcn)
            cmd.CommandTimeout = bc_cs_central_settings.ado_timeout
            cmd.CommandType = Data.CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@doc_id", doc_id)
            cmd.Parameters.AddWithValue("@filename", filename)
            cmd.Parameters.Add("@file", SqlDbType.VarBinary, docbyte.Length).Value = docbyte


            cmd.ExecuteNonQuery()
            sqlcn.Close()

            Return True

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_cs_db_repos", "write_component_to_db", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            Return False
        End Try
    End Function


    Public Function write_file_to_reach_db(fullfilename As String, filename As String, doc_id As String, channel_id As Integer, certificate As bc_cs_security.certificate) As Boolean
        Try
            REM read file in
            Dim docbyte As Byte()
            Dim fs As New bc_cs_file_transfer_services
            fs.write_document_to_bytestream(fullfilename, docbyte, certificate)
            Dim db As New bc_cs_db_services

            Dim sqlcn As New SqlConnection
            sqlcn.ConnectionString = db.GetConnectionString(certificate)
            sqlcn.Open()



            Dim cmd As New SqlCommand("dbo.bc_core_db_write_reach_files", sqlcn)
            cmd.CommandTimeout = bc_cs_central_settings.ado_timeout
            cmd.CommandType = Data.CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@doc_id", doc_id)

            cmd.Parameters.Add("@file", SqlDbType.VarBinary, docbyte.Length).Value = docbyte
            cmd.Parameters.AddWithValue("@filename", filename)
            cmd.Parameters.AddWithValue("@channel_id", channel_id)

            cmd.ExecuteNonQuery()
            sqlcn.Close()



            Return True

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_cs_db_repos", "write_file_to_reach_db", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            Return False
        End Try

    End Function


End Class

'Public Class bc_cs_web_repos

'    Public err_text As String
'    Public fileout As Byte()
'    Public Function write_base64_file_to_web_repos(filename As String, certificate As bc_cs_security.certificate) As Boolean
'        Try

'            bc_cs_central_settings.web_repos_url = "http://10.145.119.75/bc_core_components_svc/Ajax.svc/bc_web_repos_upload"
'            Dim docbyte As Byte()
'            Dim fs As New bc_cs_file_transfer_services
'            fs.write_document_to_bytestream(filename, docbyte, certificate)
'            Dim sfile As String = Convert.ToBase64String(docbyte)
'            Dim cfilename As String
'            cfilename = filename.Replace(bc_cs_central_settings.central_repos_path, "[repos]")


'            Dim json = "{ ""filename"": """ + cfilename + """, ""file"": """ + sfile + """}"

'            Dim uri As String
'            uri = bc_cs_central_settings.web_repos_url

'            Dim ocomm As New bc_cs_activity_log("bc_cs_web_repos", "write_base64_file_via_web_repos", bc_cs_activity_codes.COMMENTARY, "Rest post file: " + filename + " to " + uri, certificate)
'            Dim sjson As New bc_cs_ns_json_post(uri, json)
'            If sjson.send(certificate) = False Then
'                err_text = sjson.err_text
'                ocomm = New bc_cs_activity_log("bc_cs_web_repos", "write_base64_file_via_web_repos", bc_cs_activity_codes.COMMENTARY, "Rest post error: " + err_text, certificate)

'                write_base64_file_to_web_repos = False
'                fs.delete_file(filename)
'                Exit Function
'            Else
'                REM check for errors returned in JSON
'                ocomm = New bc_cs_activity_log("bc_cs_web_repos", "write_base64_file_via_web_repos", bc_cs_activity_codes.COMMENTARY, "Rest post response: " + sjson.response_text, certificate)

'                fs.delete_file(filename)
'                write_base64_file_to_web_repos = True

'            End If
'        Catch ex As Exception
'            Dim oerr As New bc_cs_error_log("bc_cs_web_repos", "write_base64_file_to_web_repos", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
'            Return False
'        End Try

'    End Function
'    Public Function read_base64_file_from_web_repos(filename As String, certificate As bc_cs_security.certificate) As Boolean
'        Try

'            bc_cs_central_settings.web_repos_url = "http://10.145.119.75/bc_core_components_svc/Ajax.svc/bc_web_repos_download"

'            Dim cfilename As String
'            cfilename = filename.Replace(bc_cs_central_settings.central_repos_path, "[repos]")


'            Dim json = "{ ""filename"": """ + cfilename + """}"

'            Dim uri As String
'            uri = bc_cs_central_settings.web_repos_url

'            Dim ocomm As New bc_cs_activity_log("bc_cs_web_repos", "read_base64_file_from_web_repos", bc_cs_activity_codes.COMMENTARY, "Rest post file: " + filename + " to " + uri, certificate)
'            Dim sjson As New bc_cs_ns_json_post(uri, json)
'            If sjson.send(certificate) = False Then
'                err_text = sjson.err_text
'                ocomm = New bc_cs_activity_log("bc_cs_web_repos", "read_base64_file_from_web_repos", bc_cs_activity_codes.COMMENTARY, "Rest post error: " + err_text, certificate)
'                read_base64_file_from_web_repos = False
'                Exit Function
'            Else
'                REM check for errors returned in JSON
'                ocomm = New bc_cs_activity_log("bc_cs_web_repos", "read_base64_file_from_web_repos", bc_cs_activity_codes.COMMENTARY, "Rest post response: " + sjson.response_text, certificate)
'                REM parse file out of JSON
'                Dim sfile As String
'                REM may need to remove  wrapped node.
'                sfile = sjson.response_text
'                fileout = Convert.FromBase64String(sfile)
'            End If
'        Catch ex As Exception
'            Dim oerr As New bc_cs_error_log("bc_cs_web_repos", "read_base64_file_from_web_repos", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
'            Return False
'        End Try

'    End Function
'End Class
