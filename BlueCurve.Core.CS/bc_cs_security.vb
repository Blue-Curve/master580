Imports System.IO
Imports System.Net
Imports System.Web.Services.Protocols
Imports ICSharpCode.SharpZipLib.Zip
Imports ICSharpCode.SharpZipLib.GZip
Imports ICSharpCode.SharpZipLib.Core
Imports ICSharpCode.SharpZipLib.Checksums
Imports System.Security.Cryptography
Imports System.DirectoryServices
Imports System.Security.Principal
Imports System.Text
Imports System.ServiceModel
Imports System.ServiceModel.Channels
Imports System.Net.NetworkInformation
Imports System.Security.Cryptography.X509Certificates



REM ==========================================
REM Bluecurve Limited 2005
REM Module:       Internet Secuity Services
REM Description:  New
REM Type:         CS
REM Version:      1
REM Change history
REM ==========================================


<Serializable()> Public Class bc_cs_security
    <Serializable()> Public Class certificate
        Public user_id As String
        Public authentication_mode As Integer
        Public os_name As String
        Public name As String
        Public password As String
        Public error_state As Boolean
        Public server_errors As New ArrayList
        Public authentication_token As String
        'Public ad_excel_token As String
        Public client_mac_address As String
        Public override_username_password As Boolean = False
        Public Sub New()
            error_state = False
        End Sub
    End Class
    Public Sub New()

    End Sub
    Public Sub log_user_session(ip_address As String, ByRef certificate As certificate)
        Dim db As New bc_cs_security_db
        db.log_user_session(ip_address, certificate)

    End Sub
    Public Sub log_failed_user_session(ip_address As String, ByRef certificate As certificate)
        Dim db As New bc_cs_security_db
        db.log_failed_user_session(ip_address, certificate)
    End Sub
    Function get_mac_address()
        Try
            Dim nics() As NetworkInterface = NetworkInterface.GetAllNetworkInterfaces()

            For Each adapter In nics
                Select Case adapter.NetworkInterfaceType
                    'Exclude Tunnels, Loopbacks and PPP
                    Case NetworkInterfaceType.Tunnel, NetworkInterfaceType.Loopback, NetworkInterfaceType.Ppp
                    Case Else
                        If Not adapter.GetPhysicalAddress.ToString = String.Empty And Not adapter.GetPhysicalAddress.ToString = "00000000000000E0" Then
                            get_mac_address = adapter.GetPhysicalAddress.ToString
                            Exit For ' Got a mac so exit for
                        End If

                End Select
            Next adapter
        Catch
            get_mac_address = "cant find mac address"
        End Try
    End Function
    Public Function connect_to_active_directory(ByRef certificate As bc_cs_security.certificate) As Boolean
        Dim ocomm As bc_cs_activity_log
        Try
            ocomm = New bc_cs_activity_log("bc_cs_secuirty", "connect_to_active_directory_wcf", bc_cs_activity_codes.COMMENTARY, "Attempting Active Drectory Authentication")
            connect_to_active_directory = False
            REM active directory so first see if can windows authtenticate
            Dim oauws As New wcfadauthenticate.bluecurve_core_ad_wcf
            oauws.Credentials = System.Net.CredentialCache.DefaultCredentials
            Dim url As String
            Dim ourl As String
            Dim stem As String
            url = oauws.Url
            ourl = bc_cs_central_settings.soap_server
            stem = Left(ourl, InStr(9, ourl, "/") - 1)
            url = Replace(url, "http://localhost", stem)
            url = Replace(url, "http://prose-pc.bc.local", stem)

            ocomm = New bc_cs_activity_log("bc_cs_secuirty", "connect_to_active_directory_wcf", bc_cs_activity_codes.COMMENTARY, "URL: " + CStr(url))
            oauws.Url = url
            Dim token As String
            Try
                token = oauws.TestAdConnect(bc_cs_central_settings.GetLoginName, get_mac_address())
            Catch ex As Exception
                ocomm = New bc_cs_activity_log("bc_cs_secuirty", "connect_to_active_directory_wcf", bc_cs_activity_codes.COMMENTARY, "Active Directory Test Service Error: " + ex.Message)
                Exit Function
            End Try

            If Len(token) > 6 Then
                If Left(token, 6) = "Error:" Then
                    ocomm = New bc_cs_activity_log("bc_cs_secuirty", "connect_to_active_directory", bc_cs_activity_codes.COMMENTARY, "Active Directory Authentication failed: " + token)
                    Exit Function
                End If
            End If
            connect_to_active_directory = True
            certificate.authentication_token = token
            ocomm = New bc_cs_activity_log("bc_cs_secuirty", "connect_to_active_directory_wcf", bc_cs_activity_codes.COMMENTARY, "Active Directory Authentication successful")
        Catch ex As Exception
            ocomm = New bc_cs_activity_log("bc_cs_secuirty", "connect_to_active_directory_wcf", bc_cs_activity_codes.COMMENTARY, "Active Directory Authentication failed: " + ex.Message)
        End Try
    End Function


    'Public Function old_connect_to_active_directory(ByRef certificate As bc_cs_security.certificate) As Boolean
    '    Dim ocomm As bc_cs_activity_log
    '    Try
    '        ocomm = New bc_cs_activity_log("bc_cs_secuirty", "connect_to_active_directory", bc_cs_activity_codes.COMMENTARY, "Attempting Active Drectory Authentication")
    '        connect_to_active_directory = False
    '        REM active directory so first see if can windows authtenticate
    '        Dim oauws As New BlueCurveAuthenticationService.Service



    '        Dim token As String
    '        oauws.Credentials = System.Net.CredentialCache.DefaultCredentials
    '        REM set URL TBD
    '        Dim url As String
    '        Dim ourl As String
    '        Dim stem As String

    '        url = oauws.Url
    '        REM now replace localhost with actual server
    '        ourl = bc_cs_central_settings.soap_server
    '        stem = Left(ourl, InStr(9, ourl, "/") - 1)


    '        url = Replace(url, "http://localhost", stem)

    '        ocomm = New bc_cs_activity_log("bc_cs_secuirty", "connect_to_active_directory", bc_cs_activity_codes.COMMENTARY, "URL: " + CStr(url))


    '        oauws.Url = url
    '        token = oauws.GetCustomToken()

    '        If Len(token) > 6 Then
    '            If Left(token, 7) = "failed:" Then
    '                ocomm = New bc_cs_activity_log("bc_cs_secuirty", "connect_to_active_directory", bc_cs_activity_codes.COMMENTARY, "Active Directory Authentication failed: " + token)
    '                Exit Function
    '            End If
    '        End If
    '        connect_to_active_directory = True
    '        certificate.authentication_token = token
    '        ocomm = New bc_cs_activity_log("bc_cs_secuirty", "connect_to_active_directory", bc_cs_activity_codes.COMMENTARY, "Active Directory Authentication successful")
    '    Catch ex As Exception
    '        ocomm = New bc_cs_activity_log("bc_cs_secuirty", "connect_to_active_directory", bc_cs_activity_codes.COMMENTARY, "Active Directory Authentication failed: " + ex.Message)
    '    End Try
    'End Function
    Public Function decompress_xml_winzip(ByVal compressedByte As Byte(), ByVal certificate As bc_cs_security.certificate) As String
        Dim otrace As New bc_cs_activity_log("bc_cs_security", "decompress_xml_winzip", bc_cs_activity_codes.TRACE_ENTRY, CStr(certificate.user_id), certificate)
        Try


            Using ms As New MemoryStream(compressedByte)
                Using strmZipInputStream As ZipInputStream = New ZipInputStream(ms)
                    Dim objEntry As ZipEntry


                    objEntry = strmZipInputStream.GetNextEntry()
                    Dim uncompressedByte(objEntry.Size) As Byte


                    Dim i As Integer
                    i = strmZipInputStream.Read(uncompressedByte, 0, uncompressedByte.Length)


                    strmZipInputStream.Close()
                    Dim fn As String
                    Dim kk As System.DateTime
                    kk = System.DateTime.Now

                    If bc_cs_central_settings.server_flag = 0 Then
                        fn = bc_cs_central_settings.get_user_dir + "\" + "dw_" + CStr(kk.Minute) + CStr(kk.Second) + CStr(kk.Millisecond) + CStr(bc_cs_central_settings.logged_on_user_id)
                    Else
                        fn = bc_cs_central_settings.server_log_file_path + "\" + "dw_" + CStr(kk.Minute) + CStr(kk.Second) + CStr(kk.Millisecond) + CStr(certificate.user_id)
                    End If
                    fn = fn + ".txt"
                    Dim fs As New FileStream(fn, FileMode.Create)


                    fs.Write(uncompressedByte, 0, uncompressedByte.Length)
                    fs.Close()
                    Dim ofs As New StreamReader(fn)


                    decompress_xml_winzip = ofs.ReadToEnd

                    ofs.Close()
                    Dim fser As New bc_cs_file_transfer_services
                    fser.delete_file(fn)


                End Using

                ms.Close()

            End Using

        Catch ex As Exception
            Dim db_err As New bc_cs_activity_log("bc_cs_security", "decompress_xml_winzip", bc_cs_activity_codes.COMMENTARY, ex.Message, certificate)
            decompress_xml_winzip = ""
        Finally
            otrace = New bc_cs_activity_log("bc_cs_security", "decompress_xml_winzip", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Function



    Public Function compress_xml_winzip(ByVal cst As String, ByRef certificate As bc_cs_security.certificate) As Byte()
        Dim otrace As New bc_cs_activity_log("bc_cs_security", "compress_xml_winzip", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Dim fn As String
            Dim kk As System.DateTime
            kk = System.DateTime.Now
            Dim ocommentary As New bc_cs_activity_log("bc_cs_security", "compress_xml_winzip", bc_cs_activity_codes.COMMENTARY, "Object size before compression: " + CStr(cst.Length), certificate)
            If bc_cs_central_settings.server_flag = 0 Then
                fn = bc_cs_central_settings.get_user_dir + "\" + "c_" + CStr(kk.Minute) + CStr(kk.Second) + CStr(kk.Millisecond) + CStr(bc_cs_central_settings.logged_on_user_id)
            Else
                fn = bc_cs_central_settings.server_log_file_path + "\" + "c_" + CStr(kk.Minute) + CStr(kk.Second) + CStr(kk.Millisecond) + CStr(certificate.user_id)
            End If
            REM compress
            Dim fs As New StreamWriter(fn + ".txt", FileMode.Create)
            fs.WriteLine(cst)
            fs.Close()
            REM compress
            Dim objCrc32 As New Crc32
            Dim zos As ZipOutputStream

            Dim k As New StreamWriter(fn + ".zip")
            zos = New ZipOutputStream(k.BaseStream) 'your

            Dim strFile As String = fn + ".txt"

            Dim strmFile As FileStream = File.OpenRead(strFile)
            Dim abyBuffer(CInt(strmFile.Length - 1)) As Byte

            strmFile.Read(abyBuffer, 0, abyBuffer.Length)
            Dim objZipEntry As ZipEntry = New ZipEntry(strFile)

            objZipEntry.DateTime = DateTime.Now
            objZipEntry.Size = strmFile.Length
            strmFile.Close()
            objCrc32.Reset()
            objCrc32.Update(abyBuffer)
            objZipEntry.Crc = objCrc32.Value
            zos.PutNextEntry(objZipEntry)
            zos.Write(abyBuffer, 0, abyBuffer.Length)


            zos.Finish()
            zos.Close()


            Dim ss As New bc_cs_file_transfer_services
            Dim obyte As Byte() = Nothing
            ss.write_document_to_bytestream(fn + ".zip", obyte, certificate)

            compress_xml_winzip = obyte
            'Dim ns As New FileStream(fn + ".zip", FileMode.Open)
            'Dim xbyBuffer(ns.Length - 1) As Byte
            'ns.Read(xbyBuffer, 0, ns.Length - 1)
            's = UTF8Encoding.UTF8.GetString(xbyBuffer)

            REM s = Convert.ToBase64String(xbyBuffer)
            'ns.Close()
            'ocommentary = New bc_cs_activity_log("bc_cs_security", "compress_xml", bc_cs_activity_codes.COMMENTARY, "Object size after compression: " + CStr(s.Length), certificate)
            'compress_xml_winzip = s
            ocommentary = New bc_cs_activity_log("bc_cs_security", "compress_xml_winzip", bc_cs_activity_codes.COMMENTARY, "Object size after compression: " + CStr(obyte.Length), certificate)


            Dim fser As New bc_cs_file_transfer_services
            fser.delete_file(fn + ".zip")
            REM PR remove this when done
            fser.delete_file(fn + ".txt")


        Catch ex As Exception
            compress_xml_winzip = Nothing
            'Dim db_err As New bc_cs_activity_log("bc_cs_security", "compress_xml_winzip", bc_cs_activity_codes.COMMENTARY, ex.Message, certificate)

        Finally
            otrace = New bc_cs_activity_log("bc_cs_security", "compress_xml_winzip", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Function
    Public Function compress_xml(ByVal cst As String, ByRef certificate As bc_cs_security.certificate) As String
        Dim otrace As New bc_cs_activity_log("bc_cs_security", "compress_xml", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try

            Dim uncompressedByte() As Byte
            Dim crc As New Crc32

            uncompressedByte = Convert.FromBase64String(cst)

            Using ms As New MemoryStream()

                Using strmZipOutputStream As New ZipOutputStream(ms)

                    strmZipOutputStream.SetLevel(9)

                    Dim objZipEntry As New ZipEntry("filename") 'dummy file name as using byte array

                    objZipEntry.DateTime = DateTime.Now

                    crc.Reset()

                    crc.Update(uncompressedByte)

                    objZipEntry.Size = uncompressedByte.Length

                    objZipEntry.Crc = crc.Value

                    strmZipOutputStream.PutNextEntry(objZipEntry)

                    strmZipOutputStream.Write(uncompressedByte, 0, uncompressedByte.Length)

                    strmZipOutputStream.Finish()

                    Dim compressedByte() As Byte

                    compressedByte = ms.ToArray

                    Dim s As String

                    s = Convert.ToBase64String(compressedByte)

                    Dim ocommentary = New bc_cs_activity_log("bc_cs_security", "compress_xml", bc_cs_activity_codes.COMMENTARY, "Object size after compression: " + CStr(s.Length), certificate)

                    compress_xml = s

                    strmZipOutputStream.Close()

                End Using

                ms.Close()
            End Using

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_cs_security", "compress_xml", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            compress_xml = ""
        Finally
            otrace = New bc_cs_activity_log("bc_cs_security", "compress_xml", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Function
    Public Function decompress_xml_from_ms(ByVal ms As MemoryStream, ByVal certificate As bc_cs_security.certificate) As String
        Dim otrace As New bc_cs_activity_log("bc_cs_security", "decompress_xml_from_ms", bc_cs_activity_codes.TRACE_ENTRY, CStr(certificate.user_id), certificate)
        Try




            Using ms
                Using strmZipInputStream As ZipInputStream = New ZipInputStream(ms)
                    Dim objEntry As ZipEntry

                    objEntry = strmZipInputStream.GetNextEntry()
                    Dim uncompressedByte(objEntry.Size) As Byte
                    Dim i As Integer
                    i = strmZipInputStream.Read(uncompressedByte, 0, uncompressedByte.Length)

                    Dim s As String
                    s = Convert.ToBase64String(uncompressedByte)

                    decompress_xml_from_ms = s 'System.Text.Encoding.UTF8.GetString(by)
                    strmZipInputStream.Close()
                End Using

                ms.Close()
                ms.Dispose()

            End Using

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_cs_security", "decompress_xml_from_ms", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            decompress_xml_from_ms = ""
        Finally
            otrace = New bc_cs_activity_log("bc_cs_security", "decompress_xml_from_ms", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Function


    Public Function decompress_xml(ByVal cst As String, ByVal certificate As bc_cs_security.certificate) As String
        Dim otrace As New bc_cs_activity_log("bc_cs_security", "decompress_xml", bc_cs_activity_codes.TRACE_ENTRY, CStr(certificate.user_id), certificate)
        Try

            Dim compressedByte() As Byte

            compressedByte = Convert.FromBase64String(cst)

            Using ms As New MemoryStream(compressedByte)
                Using strmZipInputStream As ZipInputStream = New ZipInputStream(ms)
                    Dim objEntry As ZipEntry

                    objEntry = strmZipInputStream.GetNextEntry()
                    Dim uncompressedByte(objEntry.Size) As Byte
                    Dim i As Integer
                    i = strmZipInputStream.Read(uncompressedByte, 0, uncompressedByte.Length)

                    Dim s As String
                    s = Convert.ToBase64String(uncompressedByte)

                    decompress_xml = s 'System.Text.Encoding.UTF8.GetString(by)
                    strmZipInputStream.Close()
                End Using

                ms.Close()
                ms.Dispose()

            End Using

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_cs_security", "decompress_xml", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            decompress_xml = ""
        Finally
            otrace = New bc_cs_activity_log("bc_cs_security", "decompress_xml", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Function
    Public Function authenticate_user(ByVal authentication_method As Integer, ByVal os_login_name As String, ByVal user_name As String, ByVal password As String, ByRef certificate As bc_cs_security.certificate) As String
        REM authenticates user_id directly from database
        Dim otrace As New bc_cs_activity_log("bc_cs_security", "authenticate_user", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Dim valid As Boolean
        Try
            Dim user As Object
            'Dim user_id As Long
            Dim bc_cs_security_db As New bc_cs_security_db
            Dim ad_authentication_failed As Boolean = False
            valid = False

            REM Handel encryption
            'If authentication_method = 1 And bc_cs_central_settings.user_authentication_method = "encrypted" Then
            '    REM Get user id  from user name
            '    user = bc_cs_security_db.get_user_id_for_name(user_name, certificate)
            '    If IsArray(user) Then
            '        If UBound(user, 2) > -1 Then
            '            REM hash password
            '            user_id = user(0, 0)
            '            password = Me.HashPassword(1, password, certificate)
            '        End If
            '    End If
            'End If

            ' active directory authentication
            'If authentication_method = 2 Then
            '    otrace = New bc_cs_activity_log("bc_cs_security", "authenticate_user", bc_cs_activity_codes.COMMENTARY, "Active directory token: " + certificate.authentication_token.ToString, certificate)
            'End If
            'active director user name  password

            'If authentication_method = 1 And bc_cs_central_settings.user_authentication_method = "AD" Then
            '    Dim ad As New bc_cs_active_directory
            '    If ad.Authenticate(certificate.authentication_token, certificate.name, certificate.password, certificate) Then
            '        otrace = New bc_cs_activity_log("bc_cs_security", "authenticate_user", bc_cs_activity_codes.COMMENTARY, "AD Authenticated", certificate)
            '        authentication_method = 0
            '    Else
            '        otrace = New bc_cs_activity_log("bc_cs_security", "authenticate_user", bc_cs_activity_codes.COMMENTARY, "AD Not Authenticated", certificate)
            '        ad_authentication_failed = True
            '    End If
            'End If

            REM os logon authentication
            'If Not ad_authentication_failed Then

            'If certificate.authentication_mode = 3 Then
            '    REM need to compare username and password to an active directory user
            '    If ad_authenticate(user_name, password, certificate) = False Then
            '        authenticate_user = "0"
            '        Exit Function
            '    End If
            '    os_login_name = user_name
            'End If
            user = bc_cs_security_db.os_user_name(authentication_method, os_login_name, user_name, password, certificate)
            bc_cs_central_settings.authenticated_server_user = user(0, 0)
            otrace = New bc_cs_activity_log("bc_cs_security", "authenticate_user", bc_cs_activity_codes.COMMENTARY, bc_cs_central_settings.authenticated_server_user, certificate)
            'End If

            bc_cs_central_settings.authentication_method = authentication_method
            authenticate_user = "0"

            If IsArray(user) Then
                If UBound(user, 2) > -1 Then
                    If authentication_method = 0 Then
                        If UCase(user(0, 0)) = UCase(os_login_name) Then
                            authenticate_user = True
                            REM set central settings logged on user
                            bc_cs_central_settings.logged_on_user_name = user(0, 0)
                            bc_cs_central_settings.logged_on_user_id = user(1, 0)
                            authenticate_user = CStr(user(1, 0))
                            certificate.name = user(2, 0)
                            certificate.user_id = user(1, 0)
                            valid = True
                            Dim ocommentary = New bc_cs_activity_log("bc_cs_security", "authenticate_user_from_os_logon", bc_cs_activity_codes.COMMENTARY, "OS Authentication valid for: " + os_login_name, certificate)
                        End If
                    ElseIf authentication_method = 1 Then
                        If UCase(user(2, 0)) = UCase(user_name) Then
                            authenticate_user = True
                            REM set central settings logged on user
                            bc_cs_central_settings.authenticated_server_user = user_name
                            bc_cs_central_settings.logged_on_user_id = user(1, 0)
                            bc_cs_central_settings.logged_on_user_name = user(2, 0)
                            certificate.name = user(3, 0)
                            certificate.user_id = user(1, 0)
                            authenticate_user = CStr(user(1, 0))
                            valid = True
                            Dim ocommentary = New bc_cs_activity_log("bc_cs_security", "authenticate_user", bc_cs_activity_codes.COMMENTARY, "User Name Authentication valid for: " + user_name, certificate)
                        End If
                    ElseIf authentication_method = 2 Then
                        REM active directory
                        authenticate_user = True
                        REM set central settings logged on user
                        bc_cs_central_settings.logged_on_user_name = user(0, 0)
                        bc_cs_central_settings.logged_on_user_id = user(1, 0)
                        authenticate_user = CStr(user(1, 0))
                        certificate.name = user(2, 0)
                        certificate.user_id = user(1, 0)
                        valid = True
                        Dim ocommentary = New bc_cs_activity_log("bc_cs_security", "authenticate_user_from_ad", bc_cs_activity_codes.COMMENTARY, "OS Authentication valid for: " + os_login_name, certificate)
                    ElseIf authentication_method = 2 Then
                        REM TBD
                    End If
                End If
            End If
        Catch ex As Exception

            authenticate_user = "0"
        Finally
            If valid = False Then
                If authentication_method = 0 Then
                    Dim ocommentary = New bc_cs_activity_log("bc_cs_security", "authenticate_user", bc_cs_activity_codes.COMMENTARY, "OS Authentication failed for: " + os_login_name, certificate)
                ElseIf authentication_method = 1 Then
                    Dim ocommentary = New bc_cs_activity_log("bc_cs_security", "authenticate_user", bc_cs_activity_codes.COMMENTARY, "User Name Authentication failed for: " + user_name, certificate)
                ElseIf authentication_method = 2 Then
                    Dim ocommentary = New bc_cs_activity_log("bc_cs_security", "authenticate_user", bc_cs_activity_codes.COMMENTARY, "AD Authentication failed for: " + os_login_name, certificate)
                End If
            End If
            otrace = New bc_cs_activity_log("bc_cs_security", "authenticate_user", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Function


    'Public Function ad_authenticate(ByVal username As String, ByVal password As String, ByRef certificate As bc_cs_security.certificate) As Boolean
    '    Try
    '        Dim obj As Object
    '        Dim user As DirectoryEntry
    '        Dim deSearch As DirectorySearcher
    '        Dim results As SearchResult
    '        Dim ocomm As New bc_cs_activity_log("bc_cs_security", "ttad_authenticate", bc_cs_activity_codes.COMMENTARY, "LDAP:" + bc_cs_central_settings.ldap_path, certificate)

    '        Dim entry As System.DirectoryServices.DirectoryEntry
    '        entry = New DirectoryEntry(bc_cs_central_settings.ldap_path, _
    '                                                           username, _
    '                                                           password, _
    '                                                           AuthenticationTypes.Secure _
    '                                                           )
    '        Try
    '            obj = entry.NativeObject

    '        Catch ex As Exception
    '            ocomm = New bc_cs_activity_log("bc_cs_security", "ad_authenticate", bc_cs_activity_codes.COMMENTARY, "Couldnt find native user: " + ex.Message, certificate)
    '            ad_authenticate = False
    '            Exit Function
    '        End Try
    '        Dim otrace As New bc_cs_activity_log("bc_cs_active_directory", "yeah: " + CStr(bc_cs_central_settings.ad_group), bc_cs_activity_codes.COMMENTARY, "now attempting to authenticate against group", certificate)

    '        REM now try against group
    '        If bc_cs_central_settings.ad_group.Trim <> "" Then
    '            Try
    '                ' get the user 
    '                deSearch = New DirectorySearcher
    '                deSearch.SearchRoot = entry
    '                If InStr(username, "\") > 1 Then
    '                    username = Right(username, Len(username) - InStr(username, "\"))
    '                End If

    '                deSearch.Filter = "(&(objectClass=user)(SAMAccountName=" + username + "))"
    '                ocomm = New bc_cs_activity_log("bc_cs_active_directory", "Autenticate", bc_cs_activity_codes.COMMENTARY, "find user:" + username, certificate)
    '                deSearch.SearchScope = SearchScope.Subtree


    '                results = deSearch.FindOne()

    '                If Not results.Path Is Nothing Then
    '                    ocomm = New bc_cs_activity_log("bc_cs_active_directory", "Autenticate", bc_cs_activity_codes.COMMENTARY, "path is not  nothing", certificate)

    '                    'get user directory entry for checking groups
    '                    ' create directory entry
    '                    user = New DirectoryEntry( _
    '                                            results.Path, _
    '                                            username, _
    '                                            password, _
    '                                            AuthenticationTypes.Secure _
    '                                            )
    '                    Dim ad As New bc_cs_active_directory

    '                    If ad.isUserInGroup(entry, user, certificate) = True Then
    '                        ocomm = New bc_cs_activity_log("bc_cs_active_directory", "Autenticate", bc_cs_activity_codes.COMMENTARY, "user in group", certificate)
    '                        ad_authenticate = True
    '                    Else
    '                        ocomm = New bc_cs_activity_log("bc_cs_active_directory", "Autenticate", bc_cs_activity_codes.COMMENTARY, "user not in group:" + bc_cs_central_settings.ad_group, certificate)
    '                        ad_authenticate = False
    '                    End If
    '                End If
    '            Catch ex As Exception
    '                ocomm = New bc_cs_activity_log("bc_cs_active_directory", "Autenticate", bc_cs_activity_codes.COMMENTARY, "failed to validate against group: " + bc_cs_central_settings.ad_group + ": " + ex.Message, certificate)
    '                Dim oerr As New bc_cs_error_log("bc_cs_active_directory", "Authenticate", bc_cs_error_codes.USER_DEFINED, "failed to validate against group: " + bc_cs_central_settings.ad_group + ": " + ex.Message, certificate)
    '                ad_authenticate = False
    '            End Try
    '        Else
    '            ad_authenticate = True
    '        End If

    '    Catch ex As Exception
    '        ad_authenticate = False
    '        Dim ocomm As New bc_cs_activity_log("bc_cs_security", "ad_authenticate", bc_cs_activity_codes.COMMENTARY, ex.Message, certificate)
    '    End Try

    'End Function

    Public Function HashPassword(ByVal user_id As Long, ByVal user_password As String, ByRef certificate As bc_cs_security.certificate) As String

        '==========================================
        ' Blue Curve Limited 2010
        ' Desciption:    Hash user entered password
        '                 
        ' Version:        1.0
        ' Change history:
        '
        '==========================================

        Dim slog As New bc_cs_activity_log("bc_om_users", "HashPassword", bc_cs_activity_codes.TRACE_ENTRY, "Hash password", certificate)
        Dim salt As String
        Dim enteredPassword As String

        HashPassword = Nothing

        Try
            Dim ocommentary As New bc_cs_activity_log("bc_om_users", "HashPassword", bc_cs_activity_codes.COMMENTARY, "Hash password", certificate)
            Dim hashProvider As New bc_cs_encyption_hash(bc_cs_encyption_hash.Provider.SHA1)

            enteredPassword = user_password
            REM salt = Trim(user_id.ToString)
            salt = 1
            Dim hashData As New bc_cs_encyption_data(enteredPassword)
            Dim hashsalt As New bc_cs_encyption_data(salt)
            hashProvider.Calculate(hashData, hashsalt)

            HashPassword = hashProvider.Value.ToHex

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_users", "HashPassword", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            slog = New bc_cs_activity_log("bc_om_users", "HashPassword", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Function

    Public Class bc_cs_encrytion

        Public Function generate_key_file(ByVal filename As String, ByVal compress As Boolean, ByRef key As Byte(), ByRef IV As Byte(), ByRef certificate As bc_cs_security.certificate) As Boolean
            generate_key_file = False
            Dim RijndaelAlg As Rijndael = Rijndael.Create
            Dim keyfile As New bc_cs_encrytion_keys
            keyfile.key = RijndaelAlg.Key
            keyfile.IV = RijndaelAlg.IV
            key = RijndaelAlg.Key
            IV = RijndaelAlg.IV
            If compress = False Then
                keyfile.write_data_to_file(filename)
            Else
                Dim bcs As New bc_cs_security
                Dim fkey As New StreamWriter(filename)
                If bc_cs_central_settings.server_flag = 0 Then
                    fkey.Write(bcs.compress_xml(keyfile.write_data_to_string(Nothing), Nothing))
                Else
                    fkey.Write(bcs.compress_xml(keyfile.write_data_to_string(certificate), certificate))
                End If
                fkey.Close()
            End If
            generate_key_file = True
        End Function
        REM encrypted input string to file using key file
        Public Function encrypt(ByVal str As String, ByVal filename As String, ByVal key As Byte(), ByVal IV As Byte()) As Boolean

            Try
                REM in key file
                encrypt = False
                ' Create a new Rijndael object.
                Dim RijndaelAlg As Rijndael = Rijndael.Create
                RijndaelAlg.Key = key
                RijndaelAlg.IV = IV
                ' Create or open the specified file.
                Dim fStream As FileStream = File.Open(filename, FileMode.Create)
                ' Create a CryptoStream using the FileStream 
                ' and the passed key and initialization vector (IV).
                Dim cStream As New CryptoStream(fStream, _
                                               RijndaelAlg.CreateEncryptor(RijndaelAlg.Key, RijndaelAlg.IV), _
                                               CryptoStreamMode.Write)

                ' Create a StreamWriter using the CryptoStream.
                Dim sWriter As New StreamWriter(cStream)

                Try

                    ' Write the data to the stream 
                    ' to encrypt it.
                    sWriter.WriteLine(str)
                Catch e As Exception

                    Console.WriteLine("An error occurred: {0}", e.Message)

                Finally

                    ' Close the streams and
                    ' close the file.
                    sWriter.Close()
                    cStream.Close()
                    fStream.Close()
                    encrypt = True
                End Try

            Catch ex As Exception
                Dim oerr As New bc_cs_error_log("bc_cs_encryption", "Encrypt", bc_cs_error_codes.USER_DEFINED, CStr(ex.Message))
            End Try

        End Function
        REM takes a filename with an encryted string in it and decrypts it using keyfile
        Public Function decrypt(ByVal filename As String, ByVal key As Byte(), ByVal IV As Byte(), ByRef output As String) As Boolean
            Try
                decrypt = False
                ' Create a new Rijndael object.
                Dim RijndaelAlg As Rijndael = Rijndael.Create
                RijndaelAlg.Key = key
                RijndaelAlg.IV = IV
                ' Create or open the specified file.
                Dim fStream As FileStream = File.Open(filename, FileMode.OpenOrCreate)


                ' Create a CryptoStream using the FileStream 
                ' and the passed key and initialization vector (IV).
                Dim cStream As New CryptoStream(fStream, _
                                                RijndaelAlg.CreateDecryptor(RijndaelAlg.Key, RijndaelAlg.IV), _
                                                CryptoStreamMode.Read)

                ' Create a StreamReader using the CryptoStream.
                Dim sReader As New StreamReader(cStream)

                ' Read the data from the stream 
                ' to decrypt it.
                Dim val As String = Nothing

                Try

                    val = sReader.ReadLine()

                Catch ex As Exception
                    Dim oerr As New bc_cs_error_log("bc_cs_encryption", "Decrypt", bc_cs_error_codes.USER_DEFINED, CStr(ex.Message))
                Finally
                    ' Close the streams and
                    ' close the file.
                    sReader.Close()
                    cStream.Close()
                    fStream.Close()

                End Try
                decrypt = True
                ' Return the string. 
                output = val
            Catch ex As Exception
                Dim oerr As New bc_cs_error_log("bc_cs_encryption", "Decrypt", bc_cs_error_codes.USER_DEFINED, CStr(ex.Message))
            End Try

        End Function
    End Class
    <Serializable()> Public Class bc_cs_encrytion_keys
        Inherits bc_cs_soap_base_class
        Public key As Byte()
        Public IV As Byte()
        Public Sub New()

        End Sub
    End Class
    <Serializable()> Public Class bc_cs_db_settings
        Inherits bc_cs_soap_base_class
        Public user_name As Byte()
        Public password As Byte()
        Public Sub New()

        End Sub
    End Class
End Class



Public Class bc_cs_accountinfoheader
    Inherits SoapHeader

    Public os_logon_name As String
    Public user_name As String
    Public password As String
    Public authentication_method As Integer

End Class
'<Serializable()> Public Class bc_cs_om_compressed_object
'    Inherits bc_cs_om_base_class
'    Public compressed_body As String
'    <NonSerialized()> Private packets As bc_cs_om_objects_packets
'    <NonSerialized()> Private received_packets As bc_cs_om_objects_packets
'    Public Sub New()

'    End Sub
'    Public Function transmit() As Boolean
'        Dim otrace As New bc_cs_activity_log("bc_cs_om_compressed", "transmit", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
'        Try
'            transmit = False
'            write_object_to_packets()
'            transmit = Me.transmit_packets

'        Catch ex As Exception
'            Dim db_err As New bc_cs_error_log("bc_cs_om_compressed", "transmit", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)

'        Finally
'            otrace = New bc_cs_activity_log("bc_cs_om_compressed", "transmit", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
'        End Try
'    End Function
'    REM packet functionality to split an object up for transmission
'    Private Sub write_object_to_packets()
'        Dim otrace As New bc_cs_activity_log("bc_cs_om_base_class", "write_object_to_packets", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
'        Try
'            REM serialize to string max 2M limit per packet
'            Const packetsize As Long = 2000000
'            Dim oxml As New bc_cs_xml_services
'            Dim s As String
'            Dim bend As Boolean = False
'            Dim start As Integer = 0
'            Dim packet_count As Long = 0
'            Dim str As String

'            Dim packet As bc_cs_om_object_packet
'            Dim bytedoc As Byte()
'            Dim ocommentary As bc_cs_activity_log


'            Dim i As Integer
'            Dim fstream As StreamWriter

'            packets = New bc_cs_om_objects_packets(1)

'            s = write_xml_to_string()
'            ocommentary = New bc_cs_activity_log("bc_cs_om_compressed_object", "write_object_to_packets", bc_cs_activity_codes.COMMENTARY, "Compressed object total size: " + CStr(s.Length), certificate)
'            REM chop up
'            While bend = False
'                str = ""
'                Try
'                    str = s.Substring(start, packetsize)
'                Catch
'                    str = s.Substring(start)
'                End Try
'                'REM write string to file
'                fstream = New StreamWriter(bc_cs_central_settings.get_user_dir + "\packet.txt", FileMode.Create)
'                fstream.Write(str)
'                fstream.Close()
'                Dim fs As New bc_cs_file_transfer_services
'                Dim obyte As Byte()
'                fs.write_document_to_bytestream(bc_cs_central_settings.get_user_dir + "\packet.txt", obyte, Me.certificate)
'                packet = New bc_cs_om_object_packet(packet_count, obyte, 0)
'                If Len(str) < packetsize Then
'                    bend = True
'                End If
'                fs.delete_file(bc_cs_central_settings.get_user_dir + "\packet.txt")


'                packets.opackets.Add(packet)
'                packet_count = packet_count + 1
'                start = start + packetsize
'                If str.Length < packetsize Then
'                    bend = True
'                    Exit While
'                End If
'            End While
'            packets.number_of_packets = packet_count
'            ocommentary = New bc_cs_activity_log("bc_om_base_class", "read_xml_from_file", bc_cs_activity_codes.COMMENTARY, "Creating: " + CStr(packet_count) + " Packets of size: " + CStr(packetsize) + " each.")


'        Catch ex As Exception
'            Dim db_err As New bc_cs_error_log("bc_cs_om_base_class", "write_object_to_packets", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)

'        Finally
'            otrace = New bc_cs_activity_log("bc_cs_om_base_class", "write_object_to_packets", bc_cs_activity_codes.TRACE_EXIT, "", certificate)

'        End Try
'    End Sub
'    Private Function web_service_transmit_packet(ByVal s As String) As Boolean
'        Dim otrace As New bc_cs_activity_log("bc_cs_om_compressed_object", "web_service_transmit_packet", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
'        Try
'            Dim bc_cs_accountinfoheader As New localhost.bc_cs_accountinfoheader
'            webservice = New localhost.Service1
'            webservice.Url = bc_cs_central_settings.soap_server
'            bc_cs_accountinfoheader.authentication_method = bc_cs_central_settings.show_authentication_form
'            If bc_cs_central_settings.show_authentication_form = 0 Then
'                bc_cs_accountinfoheader.os_logon_name = bc_cs_central_settings.logged_on_user_name
'            Else
'                bc_cs_accountinfoheader.user_name = bc_cs_central_settings.user_name
'                bc_cs_accountinfoheader.password = bc_cs_central_settings.user_password
'            End If
'            webservice.bc_cs_accountinfoheaderValue = bc_cs_accountinfoheader
'            If IsNumeric(bc_cs_central_settings.timeout) Then
'                webservice.Timeout = CLng(bc_cs_central_settings.timeout)
'                Dim ocommentary As New bc_cs_activity_log("bc_om_web services", "call_web_service", bc_cs_activity_codes.COMMENTARY, "Setting Web Service Timeout to: " + bc_cs_central_settings.timeout)
'            End If

'            set_proxy_settings()
'            Dim es As String
'            es = webservice.LoadObjectViaPackets(s)
'            If Left(es, 25) = "Webservice Authentication" Then
'                web_service_transmit_packet = False
'                Dim ocommentary As New bc_cs_activity_log("bc_om_base_class", "read_xml_via_soap_client_request", bc_cs_activity_codes.COMMENTARY, "Authentication Failed for Web Service! User:" + bc_cs_accountinfoheader.os_logon_name)
'                Dim msgtxt As String
'                If bc_cs_central_settings.show_authentication_form = 0 Then
'                    msgtxt = "Authentication Failed for Web Service! User:" + bc_cs_accountinfoheader.os_logon_name
'                Else
'                    msgtxt = "Authentication Failed for Web Service! User:" + bc_cs_accountinfoheader.user_name
'                End If
'                Dim omessage As New bc_cs_message("Blue Curve Create", msgtxt, bc_cs_message.MESSAGE)
'            Else
'                web_service_transmit_packet = es
'            End If


'        Catch ex As Exception
'            REM if server has thrown client of then this is not a serious error
'            REM so trap this and return success. Not ideal as it will suppress real errors.
'            REM then return true
'            If InStr(ex.Message, "The connection was closed unexpectedly.", CompareMethod.Text) > 0 Then
'                Dim ocommentary As New bc_cs_activity_log("bc_cs_om_base_class", "web_service_transmit_packet", bc_cs_activity_codes.COMMENTARY, "Error Supressed Web Server disconnected from client:" + ex.Message)
'                web_service_transmit_packet = True
'            Else
'                Dim db_err As New bc_cs_error_log("bc_cs_om_compressed_object", "web_service_transmit_packet", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
'            End If
'        Finally
'            otrace = New bc_cs_activity_log("bc_cs_om_compressed_object", "web_service_transmit_packet", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
'        End Try

'    End Function
'    Private Function transmit_packets() As Boolean
'        Dim otrace As New bc_cs_activity_log("bc_cs_om_base_class", "transmit_packets", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
'        Try
'            Dim i As Integer
'            Dim ocommentary As bc_cs_activity_log
'            For i = 0 To packets.number_of_packets - 1
'                transmit_packets = transmit_packet(i)
'                If transmit_packets = False Then
'                    ocommentary = New bc_cs_activity_log("bc_om_base_class", "transmit_packets", bc_cs_activity_codes.COMMENTARY, "Packet: " + CStr(i + 1) + " failed to transmit or general server error")
'                    If bc_cs_central_settings.suppress_server_error = 0 Then
'                        Dim omessage As New bc_cs_message("Blue Curve", "Server Error Contact System Administrator", bc_cs_message.MESSAGE)
'                    End If
'                    Exit For
'                End If
'            Next

'        Catch ex As Exception
'            Dim db_err As New bc_cs_error_log("bc_cs_om_base_class", "transmit_packets", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
'        Finally
'            otrace = New bc_cs_activity_log("bc_cs_om_base_class", "transmit_packets", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
'        End Try

'    End Function
'    Private Function transmit_packet(ByVal packet_number As Long) As Boolean
'        Dim otrace As New bc_cs_activity_log("bc_cs_om_compressed_object", "transmit_packet", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
'        Try
'            transmit_packet = False
'            Dim cbc_data_services As bc_cs_xml_services
'            Dim ocommentary As bc_cs_activity_log
'            Dim s As String
'            REM transmits single packet
'            Dim tpacket As New bc_cs_om_object_packet(packet_number, Me.packets.opackets(packet_number).packet, Me.packets.number_of_packets)
'            tpacket.packet_code = Me.packet_code
'            ocommentary = New bc_cs_activity_log("bc_om_base_class", "read_xml_from_file", bc_cs_activity_codes.COMMENTARY, "Transmitting packet: " + CStr(packet_number + 1) + " of: " + CStr(Me.packets.number_of_packets) + " ;packet code: " + Me.packet_code)
'            REM serialize to string
'            cbc_data_services = New bc_cs_xml_services
'            cbc_data_services.o = tpacket
'            s = cbc_data_services.soap_serialize_object_to_string(Me.certificate)
'            REM call web service to transmit
'            s = web_service_transmit_packet(s)
'            REM deserialoze string back and read error code
'            cbc_data_services = New bc_cs_xml_services
'            'tpacket = cbc_data_services.soap_deserialize_string_to_object(s)
'            'If tpacket.transmission_state = 1 Then


'            If s = False Then
'                transmit_packet = False
'            Else
'                transmit_packet = True
'            End If
'            'End If
'        Catch ex As Exception
'            Dim db_err As New bc_cs_error_log("bc_cs_om_compressed_object", "transmit_packet", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
'        Finally
'            otrace = New bc_cs_activity_log("bc_cs_om_compressed_object", "transmit_packet", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
'        End Try

'    End Function
'    Public Function receive_packet(ByVal s As String) As String
'        Dim otrace As New bc_cs_activity_log("bc_cs_om_compressed_object", "receive_packet", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
'        Try
'            REM deserialize packet
'            Dim tpacket As New bc_cs_om_object_packet
'            Dim ocommentary As bc_cs_activity_log
'            Dim pfs As StreamReader
'            Dim i As Integer
'            Dim complete_string As String
'            Dim o As Object
'            Dim id As String
'            Dim cbc_data_services As New bc_cs_xml_services

'            tpacket = cbc_data_services.soap_deserialize_string_to_object(s, certificate)
'            id = tpacket.packet_code + "_" + CStr(certificate.user_id)
'            Dim fs As New bc_cs_file_transfer_services
'            fs.write_bytestream_to_document("c:\bluecurve\packet" + id + "_" + CStr(tpacket.packet_number + 1) + ".txt", tpacket.packet, Me.certificate)
'            ocommentary = New bc_cs_activity_log("bc_cs_om_base_class", "receive_packet", bc_cs_activity_codes.COMMENTARY, "Receiving packet: " + CStr(tpacket.packet_number + 1) + " of: " + CStr(tpacket.number_of_packets) + " Logged on user_id:" + certificate.user_id + "filename: packet" + id, certificate)
'            REM check if last packet if so  rebuild object from packets
'            If tpacket.packet_number + 1 = tpacket.number_of_packets Then
'                ocommentary = New bc_cs_activity_log("bc_cs_om_base_class", "receive_packet", bc_cs_activity_codes.COMMENTARY, "All Packets received attempting to reconstruct object filename: packet" + id, certificate)
'                REM reconstitute

'                For i = 1 To tpacket.number_of_packets
'                    Dim sfs As New StreamReader("c:\bluecurve\packet" + id + "_" + CStr(i) + ".txt")
'                    complete_string = complete_string + sfs.ReadToEnd()
'                    sfs.Close()
'                Next
'                REM house keep files

'                For i = 1 To tpacket.number_of_packets
'                    fs.delete_file("c:\bluecurve\packet" + id + "_" + CStr(i) + ".txt")
'                Next

'                If certificate.error_state = False Then
'                    REM uncompress input
'                    Dim ocomp As New bc_cs_security
'                    read_xml_from_soap_server_reponse(ocomp.decrypt_xml(complete_string, Me.certificate), certificate, True)
'                End If

'            End If
'            tpacket.transmission_state = 1
'            cbc_data_services.o = tpacket
'            ocommentary = New bc_cs_activity_log("bc_cs_om_base_class", "receive_packet", bc_cs_activity_codes.COMMENTARY, "Server Error State: " + CStr(certificate.error_state), certificate)
'            If certificate.error_state = True Then
'                receive_packet = "false"
'            Else
'                receive_packet = "True"
'            End If


'        Catch ex As Exception
'            Dim db_err As New bc_cs_error_log("bc_cs_om_compressed_object", "receive_packet", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
'        Finally
'            otrace = New bc_cs_activity_log("bc_cs_om_compressed_object", "receive_packet", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
'        End Try

'    End Function
'    <Serializable()> Private Class bc_cs_om_objects_packets
'        Inherits bc_cs_om_base_class
'        Public number_of_packets As Integer
'        Public opackets As New ArrayList
'        Public Sub New(ByVal number_of_packets As Integer)
'            Me.number_of_packets = number_of_packets
'        End Sub
'    End Class
'    <Serializable()> Private Class bc_cs_om_object_packet
'        Inherits bc_cs_om_base_class
'        Public packet_number As Integer
'        Public number_of_packets As Integer
'        Public packet As Byte()
'        Public transmission_state As Integer = 0
'        Public Sub New(ByVal packet_number As Integer, ByVal packet As Byte(), ByVal number_of_packets As Integer)
'            Me.packet_number = packet_number
'            Me.packet = packet
'            Me.number_of_packets = number_of_packets
'        End Sub
'        Public Sub New()

'        End Sub
'    End Class

'    Protected Overrides Sub Finalize()
'        MyBase.Finalize()
'    End Sub
'End Class
REM =========================================================================
REM Database interaction layer
REM =========================================================================
Public Class bc_cs_active_directory
    Const SecurityImpersonation As Integer = 2


    Public Declare Auto Function DuplicateToken Lib "advapi32.dll" (ByVal ExistingTokenHandle As IntPtr, _
                ByVal SECURITY_IMPERSONATION_LEVEL As Integer, _
                ByRef DuplicateTokenHandle As IntPtr) As Boolean

    Public Sub New()
    End Sub

    Public Function Authenticate(ByVal WinId As IIdentity, ByVal token As IntPtr, ByVal username As String, ByVal password As String, ByVal certificate As bc_cs_security.certificate) As String

        Dim otrace As New bc_cs_activity_log("bc_cs_active_directory", "Authenticate", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Dim windowsIdent As WindowsIdentity
        Dim impersonationContext As WindowsImpersonationContext

        Dim entry As DirectoryEntry
        Dim user As DirectoryEntry
        Dim deSearch As DirectorySearcher
        Dim results As SearchResult

        Dim dupeTokenHandle As New IntPtr(0)

        Authenticate = ""

        impersonationContext = Nothing
        entry = Nothing
        user = Nothing
        deSearch = Nothing
        otrace = New bc_cs_activity_log("bc_cs_active_directory", "rrrdddddmpersonate", bc_cs_activity_codes.COMMENTARY, username, certificate)

        results = Nothing

        Dim obj As Object
        obj = Nothing
        Try

            If IntPtr.Zero.Equals(token) Then ' token not set so use the user name and password
                otrace = New bc_cs_activity_log("bc_cs_active_directory", "aaaImpersonate", bc_cs_activity_codes.COMMENTARY, username, certificate)

                ' create directory entry
                entry = New DirectoryEntry( _
                                        bc_cs_central_settings.ldap_path, _
                                        username, _
                                        password, _
                                        AuthenticationTypes.Secure _
                                        )
                obj = entry.NativeObject
            Else

                ' get the windows identity using the token provided
                'windowsIdent = New WindowsIdentity(token)
                'otrace = New bc_cs_activity_log("bc_cs_active_directory", "ydddImpersonate", bc_cs_activity_codes.COMMENTARY, windowsIdent.Name + ": " + WindowsIdentity.GetCurrent().Name, certificate)





                ' impersonate the token user
                windowsIdent = WinId
                otrace = New bc_cs_activity_log("bc_cs_active_directory", "Windows identity name", bc_cs_activity_codes.COMMENTARY, windowsIdent.Name + ": " + WindowsIdentity.GetCurrent().Name, certificate)
                otrace = New bc_cs_activity_log("bc_cs_active_directory", "windows identity token", bc_cs_activity_codes.COMMENTARY, windowsIdent.Token.ToString, certificate)
                otrace = New bc_cs_activity_log("bc_cs_active_directory", "ybefore impersonate", bc_cs_activity_codes.COMMENTARY, WindowsIdentity.GetCurrent().Name, certificate)



                'impersonationContext = windowsIdent.Impersonate()

                impersonationContext = WindowsIdentity.Impersonate(windowsIdent.Token)

                otrace = New bc_cs_activity_log("bc_cs_active_directory", "after impersonate", bc_cs_activity_codes.COMMENTARY, WindowsIdentity.GetCurrent().Name, certificate)
                otrace = New bc_cs_activity_log("bc_cs_active_directory", "LDAP path ", bc_cs_activity_codes.COMMENTARY, bc_cs_central_settings.ldap_path, certificate)

                entry = New DirectoryEntry( _
                                        bc_cs_central_settings.ldap_path, _
                                        Nothing, _
                                        Nothing, _
                                        AuthenticationTypes.Secure _
                                        )

                otrace = New bc_cs_activity_log("bc_cs_active_directory", "Impersonate", bc_cs_activity_codes.COMMENTARY, "directory entry succssful: " + CStr(entry.Name), certificate)
                obj = entry.NativeObject
                otrace = New bc_cs_activity_log("bc_cs_active_directory", "attempting authenticate", bc_cs_activity_codes.COMMENTARY, "authenticate successful", certificate)


            End If

            If bc_cs_central_settings.ad_group.Trim <> "" Then
                Try
                    otrace = New bc_cs_activity_log("bc_cs_active_directory", "Group", bc_cs_activity_codes.COMMENTARY, "now attempting to authenticate against group", certificate)
                    ' get the user 
                    deSearch = New DirectorySearcher
                    deSearch.SearchRoot = entry
                    username = WindowsIdentity.GetCurrent().Name
                    If InStr(username, "\") > 1 Then
                        username = Right(username, Len(username) - InStr(username, "\"))
                    End If

                    deSearch.Filter = "(&(objectClass=user)(SAMAccountName=" + username + "))"
                    Dim ocomm As New bc_cs_activity_log("bc_cs_active_directory", "Autenticate", bc_cs_activity_codes.COMMENTARY, "find user:" + username, certificate)
                    deSearch.SearchScope = SearchScope.Subtree


                    results = deSearch.FindOne()
                    ocomm = New bc_cs_activity_log("bc_cs_active_directory", "Autenticate", bc_cs_activity_codes.COMMENTARY, "path is nothing", certificate)

                    If Not results.Path Is Nothing Then
                        ocomm = New bc_cs_activity_log("bc_cs_active_directory", "Autenticate", bc_cs_activity_codes.COMMENTARY, "path is not nothing", certificate)

                        'get user directory entry for checking groups
                        If IntPtr.Zero.Equals(token) Then ' token not set so use the user name and password
                            ' create directory entry
                            user = New DirectoryEntry( _
                                                    results.Path, _
                                                    username, _
                                                    password, _
                                                    AuthenticationTypes.Secure _
                                                    )
                        Else
                            ' create directory entry
                            user = New DirectoryEntry( _
                                                    results.Path, _
                                                    Nothing, _
                                                    Nothing, _
                                                    AuthenticationTypes.Secure _
                                                    )
                        End If
                        If isUserInGroup(entry, user, certificate) = True Then
                            ocomm = New bc_cs_activity_log("bc_cs_active_directory", "Autenticate", bc_cs_activity_codes.COMMENTARY, "user in group", certificate)
                            Authenticate = ""
                        Else
                            ocomm = New bc_cs_activity_log("bc_cs_active_directory", "Autenticate", bc_cs_activity_codes.COMMENTARY, "user not in group", certificate)
                            Authenticate = "Failed to AD authenticate user not in group: " + bc_cs_central_settings.ad_group
                        End If
                    End If
                Catch ex As Exception
                    Dim ocomm As New bc_cs_activity_log("bc_cs_active_directory", "Autenticate", bc_cs_activity_codes.COMMENTARY, "failed to validate against group: " + bc_cs_central_settings.ad_group + ": " + ex.Message, certificate)
                    Dim oerr As New bc_cs_error_log("bc_cs_active_directory", "Authenticate", bc_cs_error_codes.USER_DEFINED, "failed to validate against group: " + bc_cs_central_settings.ad_group + ": " + ex.Message, certificate)
                    Authenticate = "Failed to AD authenticate user not in group: " + bc_cs_central_settings.ad_group + ": " + ex.Message
                End Try
            Else
                Authenticate = ""
            End If


        Catch ex As Exception
            otrace = New bc_cs_activity_log("bc_cs_active_directory", "Impersonate", bc_cs_activity_codes.COMMENTARY, "error: " + ex.Message, certificate)
            Dim oerr As New bc_cs_error_log("bc_cs_active_directory", "Authenticate", bc_cs_error_codes.USER_DEFINED, CStr(ex.Message), certificate)

            Authenticate = "Failed to AD authenticate general error: " + ex.Message

        Finally
            If Not impersonationContext Is Nothing Then
                ' stop impersonating user
                impersonationContext.Undo()
            End If
            If Not entry Is Nothing Then
                ' dispose of ldap directory object
                entry.Dispose()
            End If
            If Not user Is Nothing Then
                ' dispose of ldap directory object
                user.Dispose()
            End If
            If Not obj Is Nothing Then
                ' clear authenticated object
                obj = Nothing
            End If
            If Not results Is Nothing Then
                ' clear results object
                results = Nothing
            End If
            If Not deSearch Is Nothing Then
                ' dispose of search object
                deSearch.Dispose()
            End If
            otrace = New bc_cs_activity_log("bc_cs_active_directory", "Authenticate", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Function

    Friend Function isUserInGroup(ByVal searchRoot As DirectoryEntry, ByVal user As DirectoryEntry, _
                                    ByVal certificate As bc_cs_security.certificate) As Boolean

        Dim otrace As New bc_cs_activity_log("bc_cs_active_directory", "isUserInGroup", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Dim sb As New StringBuilder
        Try

            'we are building an '|' clause
            sb.Append("(|")

            user.RefreshCache(New String() {"tokenGroups"})

            For Each sid As Byte() In user.Properties("tokenGroups")
                'append each member into the filter
                sb.AppendFormat("(objectSid={0})", BuildFilterOctetString(sid))
            Next

            'end our initial filter
            sb.Append(")")

            'we now have our filter, we can just search for the groups
            Dim ds As New DirectorySearcher(searchRoot, sb.ToString())

            Dim src As SearchResultCollection = ds.FindAll()
            For Each sr As SearchResult In src
                'Here is each group now...
                If sr.Properties("samAccountName")(0) = bc_cs_central_settings.ad_group Then
                    Return True
                End If
            Next

        Catch ex As Exception
            Return False
        Finally
            otrace = New bc_cs_activity_log("bc_cs_active_directory", "isUserInGroup", bc_cs_activity_codes.TRACE_EXIT, "", certificate)

        End Try

    End Function

    Function BuildFilterOctetString(ByVal bytes() As Byte) As String
        Dim sb As StringBuilder = New StringBuilder
        Dim i As Integer = 0
        Do While (i < bytes.Length)
            sb.AppendFormat("\{0}", bytes(i).ToString("X2"))
            i = (i + 1)
        Loop
        Return sb.ToString
    End Function

End Class
Public Class bc_cs_security_db
    Private gbc_db As New bc_cs_db_services(False)
    REM reads all pub type in database
    Public Sub log_user_session(ip_address As String, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        If IsNumeric(certificate.user_id) = False Then
            certificate.user_id = -1

        End If

        sql = "exec dbo.bc_core_write_session " + CStr(certificate.user_id) + ",'" + ip_address + " (" + certificate.client_mac_address + ")'"
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Sub log_failed_user_session(ip_address As String, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "exec dbo.bc_core_write_failed_session " + CStr(certificate.authentication_mode) + ",'" + certificate.os_name + "','" + certificate.name + "','" + ip_address + "','" + certificate.client_mac_address + "'"
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Function os_user_name(ByVal authentication_method As Integer, ByVal os_logon_id As String, ByVal user_name As String, ByVal password As String, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String = Nothing
        Try
            If authentication_method = 0 Then
                sql = "select os_user_name, user_id, first_name + ' ' + surname from user_table where os_user_name='" + os_logon_id + "' and coalesce(inactive,0)=0 and coalesce(deleted,0)=0"
                os_user_name = gbc_db.executesql(sql, certificate)
            ElseIf authentication_method = 1 Then
                REM SQL injection fix May 2016

                Dim ps As New List(Of bc_cs_db_services.bc_cs_sql_parameter)
                Dim p As New bc_cs_db_services.bc_cs_sql_parameter
                p.name = "username"
                p.value = user_name
                ps.Add(p)
                p = New bc_cs_db_services.bc_cs_sql_parameter
                p.name = "password"
                p.value = password
                ps.Add(p)
                p = New bc_cs_db_services.bc_cs_sql_parameter
                os_user_name = gbc_db.executesql_with_parameters("bc_core_username_password_logon", ps, certificate)
                Exit Function

            ElseIf authentication_method = 2 Then

                Dim ps As New List(Of bc_cs_db_services.bc_cs_sql_parameter)
                Dim p As New bc_cs_db_services.bc_cs_sql_parameter
                p.name = "os_logon"
                p.value = os_logon_id
                ps.Add(p)
                p = New bc_cs_db_services.bc_cs_sql_parameter
                p.name = "token"
                p.value = certificate.authentication_token
                ps.Add(p)
                p = New bc_cs_db_services.bc_cs_sql_parameter
                p.name = "mac_address"
                p.value = certificate.client_mac_address
                ps.Add(p)

                p = New bc_cs_db_services.bc_cs_sql_parameter
                os_user_name = gbc_db.executesql_with_parameters("bc_core_validate_ad", ps, certificate)
                Exit Function

            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_cs_security_db", "os_user_name", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)

        End Try
    End Function
    Public Function get_user_id_for_name(ByVal user_name As String, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select  user_id, web_user_name,first_name + ' ' + surname from user_table where upper(web_user_name)='" + UCase(user_name) + "'"
        get_user_id_for_name = gbc_db.executesql(sql, certificate)
    End Function
End Class
