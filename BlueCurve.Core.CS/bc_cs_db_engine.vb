REM ==========================================
REM Bluecurve Limited 2005
REM Module:       Central Database utilities
REM Description:  Connections
REM Type:         Common Services
REM               Execute SQL
REM Version:      1
REM Change history
'22/6/06 modified the complete design by Rama Boya to use local connections and to improve performance
REM ==========================================
REM -------------------------------------------------------------------------------------------------------------------
REM Changes:
REM Tracker                 Initials                   Date                      Synopsis
REM FIL JIRA 7301           PR                         2/01/2014                 added central routine for paramter count in SP


Imports Microsoft.Win32
Imports System.IO
Imports System.Data.sqlclient
Imports System.Data

<Serializable()> Public Class bc_cs_sql
    Inherits bc_cs_soap_base_class
    Public sql As String
    Public type As SQL_TYPE
    Public results As Object
    Public success As Boolean


    Public Enum SQL_TYPE
        DEF = 1
        NO_TIMEOUT = 2
        PARAMS = 3

    End Enum
    Public Overrides Sub process_object()
        Dim otrace As New bc_cs_activity_log("bc_om_sql", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            REM this is always specific to object
            If Me.tmode = bc_cs_soap_base_class.tREAD Then
                db_read()
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_sql", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_sql", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub
    

    'allred
    Public Sub db_read()
        'Dim otrace As New bc_cs_activity_log("bc_cs_sql", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "", MyBase.certificate)
        Try
            Dim gdb As New bc_cs_db_services

            Select Case type
                Case SQL_TYPE.NO_TIMEOUT
                    results = gdb.executesql_no_timeout(sql, certificate)
                Case SQL_TYPE.PARAMS

                Case Else
                    results = gdb.executesql(sql, certificate)

            End Select
            success = gdb.success


        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_cs_sql", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, MyBase.certificate)
        Finally
            'otrace = New bc_cs_activity_log("bc_cs_sql", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", MyBase.certificate)
        End Try
    End Sub
    Public Function exec_sql(_sql As String, _type As SQL_TYPE, ByRef _results As Object) As Boolean
        Try
            exec_sql = False
            sql = _sql
            type = _type
            If (bc_cs_central_settings.connection_method = bc_cs_central_settings.ADO) Then
                db_read()
                _results = results
                exec_sql = True
            Else

                Me.tmode = bc_cs_soap_base_class.tREAD
                Dim os As New bc_cs_sql
                os.sql = _sql
                os.type = type
                os.tmode = bc_cs_soap_base_class.tREAD
                If os.transmit_to_server_and_receive(os, True) = False Then
                    Exit Function
                End If
                _results = os.results
                exec_sql = True
            End If
           
        Catch ex As Exception

            Dim db_err As New bc_cs_error_log("bc_cs_sql", "exec_sql", bc_cs_error_codes.USER_DEFINED, ex.Message, MyBase.certificate)
        Finally
            'otrace = New bc_cs_activity_log("bc_cs_sql", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", MyBase.certificate)
        End Try
    End Function

End Class

Public Class bc_cs_db_services
    'Public Const MAX_ROWS = 2000000
    Dim gsqlcn As SqlConnection
    Public success As Boolean = False
    Public rp_dl As Boolean = False

    REM constructor if connection is open
    Public Sub New()

    End Sub
    REM FIL JIRA 7301  
    Public Function get_sp_number_of_parameters(ByVal sp_name As String, ByVal sql As String, ByRef certificate As bc_cs_security.certificate)
        Try
            Dim user_id As Long
            Dim sqlcn As New SqlConnection

            If bc_cs_central_settings.server_flag = 0 Then
                user_id = bc_cs_central_settings.logged_on_user_id
            Else
                user_id = certificate.user_id
            End If

            REM Get the number of parameters in the target stored proc.
            REM Steve wooderson 17/06/2013
            sqlcn.ConnectionString = Me.GetConnectionString(certificate)
            Dim pcmd As New SqlCommand(sql, sqlcn)
            pcmd.CommandTimeout = bc_cs_central_settings.ado_timeout
            pcmd.Connection = sqlcn
            pcmd.CommandText = sp_name
            pcmd.CommandType = Data.CommandType.StoredProcedure
            sqlcn.Open()
            SqlCommandBuilder.DeriveParameters(pcmd)
            get_sp_number_of_parameters = pcmd.Parameters.Count - 1

        Catch ex As Exception
            get_sp_number_of_parameters = 0
            Dim oerr As New bc_cs_error_log("bc_cs_db_services", "Get_sp_number_of_parameters", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            'Dim oerr As New bc_cs_error_log("bc_cs_db_services", "Get_sp_number_of_parameters", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Function
    REM END JIRA

    Public Sub New(ByVal test_connection As Boolean)
    End Sub
    Public Function GetConnectionString(ByRef certificate As bc_cs_security.certificate) As String
        Dim strConnection As String
        'check whether we are using trusted connection or normal connection type.
        '"Data Source=Aron1;Initial Catalog=pubs;Integrated Security=SSPI;" 

        GetConnectionString = ""
        REM for 4.6 and higher ammend Database ib bc_config with ;TransparentNetworkIPResolution=false
        If bc_cs_central_settings.connection_type.ToLower.Trim = "trusted" Then
            strConnection = "Server=" & bc_cs_central_settings.servername + ";Database=" & bc_cs_central_settings.dbname + ";Trusted_Connection=True;Pooling=true"
        Else
            strConnection = "Data Source=SQLOLEDB;Server=" & bc_cs_central_settings.servername + ";Database=" & bc_cs_central_settings.dbname + ";UID=" & bc_cs_central_settings.username + ";PWD=" & bc_cs_central_settings.password & ";Pooling=true"
        End If
        GetConnectionString = strConnection
        Exit Function


        'If bc_cs_central_settings.connection_type.ToLower.Trim = "trusted" Then
        '    strConnection = "Server=" & bc_cs_central_settings.servername + ";Database=" & bc_cs_central_settings.dbname + ";Trusted_Connection=True;Pooling=true;TransparentNetworkIPResolution=false"
        'ElseIf bc_cs_central_settings.connection_type.ToLower.Trim = "encrypted" Then
        '    Dim encrypt_settings As String
        '    Dim settings_to_encrypt As String
        '    encrypt_settings = bc_cs_central_settings.central_repos_path + "encrypt\bc_db_settings_encrypt.xml"
        '    settings_to_encrypt = bc_cs_central_settings.central_repos_path + "encrypt\bc_config.xml"
        '    REM settings can only be encryted for client mode ADO connectivity
        '    REM if pw file exists encrytion has yet to take place so generate from settings
        '    Dim fs As New bc_cs_file_transfer_services
        '    If fs.check_document_exists(encrypt_settings) = False Then
        '        REM settings have yet to be encryted so read them in then encrypt
        '        If read_db_settings(settings_to_encrypt, certificate) = False Then
        '            strConnection = ""
        '            GetConnectionString = ""
        '            Exit Function
        '        Else
        '            REM now create encrypted settings
        '            Dim oencrypt As New bc_cs_security.bc_cs_encrytion

        '            Dim keyfile As String
        '            Dim tmpfile As String
        '            Dim key As Byte() = Nothing
        '            Dim IV As Byte() = Nothing
        '            tmpfile = bc_cs_central_settings.central_repos_path + "\encrypt\bc_tmp.txt"
        '            keyfile = bc_cs_central_settings.central_repos_path + "\encrypt\bc_key.txt"
        '            If bc_cs_central_settings.server_flag = 0 Then
        '                If oencrypt.generate_key_file(keyfile, True, key, IV, certificate) = False Then
        '                    GetConnectionString = ""
        '                    Exit Function
        '                End If
        '            Else
        '                If oencrypt.generate_key_file(keyfile, False, key, IV, certificate) = False Then
        '                    GetConnectionString = ""
        '                    Exit Function
        '                End If
        '            End If
        '            Dim oencrypt_settings As New bc_cs_db_encrypt_settings
        '            REM now encrypt parameters
        '            If oencrypt.encrypt(bc_cs_central_settings.dbname, tmpfile, key, IV) = False Then
        '                GetConnectionString = ""
        '                Exit Function
        '            End If
        '            If fs.write_document_to_bytestream(tmpfile, oencrypt_settings.dbname, certificate) = False Then
        '                Exit Function
        '            End If
        '            If oencrypt.encrypt(bc_cs_central_settings.servername, tmpfile, key, IV) = False Then
        '                Exit Function
        '            End If
        '            If fs.write_document_to_bytestream(tmpfile, oencrypt_settings.servername, certificate) = False Then
        '                Exit Function
        '            End If
        '            If oencrypt.encrypt(bc_cs_central_settings.username, tmpfile, key, IV) = False Then
        '                Exit Function
        '            End If
        '            If fs.write_document_to_bytestream(tmpfile, oencrypt_settings.username, certificate) = False Then
        '                Exit Function
        '            End If
        '            If oencrypt.encrypt(bc_cs_central_settings.password, tmpfile, key, IV) = False Then
        '                Exit Function
        '            End If
        '            If fs.write_document_to_bytestream(tmpfile, oencrypt_settings.password, certificate) = False Then
        '                Exit Function
        '            End If
        '            REM delete tmp file
        '            fs.delete_file(tmpfile, certificate)
        '            oencrypt_settings.write_data_to_file(encrypt_settings)
        '            REM now delete seed file
        '            fs.delete_file(settings_to_encrypt)
        '            strConnection = "Data Source=SQLOLEDB;Server=" & bc_cs_central_settings.servername + ";Database=" & bc_cs_central_settings.dbname + ";UID=" & bc_cs_central_settings.username + ";PWD=" & bc_cs_central_settings.password & ";Pooling=true"
        '            bc_cs_central_settings.connection_type = ""
        '        End If
        '    Else
        '        strConnection = read_encryped_settings(certificate)
        '        bc_cs_central_settings.connection_type = ""
        '    End If
        '    REM otherwise decrypt user name and password
        'Else
        '    strConnection = "Data Source=SQLOLEDB;Server=" & bc_cs_central_settings.servername + ";Database=" & bc_cs_central_settings.dbname + ";UID=" & bc_cs_central_settings.username + ";PWD=" & bc_cs_central_settings.password & ";Pooling=true;TransparentNetworkIPResolution=false"
        'End If
        'Dim sqlcn As New SqlConnection
        'Try
        '    sqlcn.ConnectionString = strConnection
        'Catch ex As Exception
        '    'Dim sw As New StreamWriter(bc_cs_central_settings.server_log_file_path + "connection_string_failed.txt", True)
        '    'sw.WriteLine(ex.Message)
        '    'sw.WriteLine(strConnection)
        '    'sw.Close()
        '    If bc_cs_central_settings.connection_type.ToLower.Trim = "trusted" Then
        '        strConnection = "Server=" & bc_cs_central_settings.servername + ";Database=" & bc_cs_central_settings.dbname + ";Trusted_Connection=True;Pooling=true"
        '    Else
        '        strConnection = "Data Source=SQLOLEDB;Server=" & bc_cs_central_settings.servername + ";Database=" & bc_cs_central_settings.dbname + ";UID=" & bc_cs_central_settings.username + ";PWD=" & bc_cs_central_settings.password & ";Pooling=true"
        '    End If
        'End Try



        'Return strConnection
    End Function
    Public Function read_encryped_settings(ByRef certificate As bc_cs_security.certificate) As String
        Try
            Dim encrypt_settings As String
            Dim okey As New bc_cs_security.bc_cs_encrytion_keys
            Dim keyfile As String
            Dim tmpfile As String
            Dim key As Byte()
            Dim IV As Byte()
            Dim fs As New bc_cs_file_transfer_services
            keyfile = bc_cs_central_settings.central_repos_path + "\encrypt\bc_key.txt"
            encrypt_settings = bc_cs_central_settings.central_repos_path + "encrypt\bc_db_settings_encrypt.xml"

            read_encryped_settings = ""

            REM first check settings and key files exist
            If fs.check_document_exists(keyfile, Nothing) = False Then
                Dim oerr As New bc_cs_error_log("bc_cs_db_services", "read_encryped_settings", bc_cs_error_codes.USER_DEFINED, "Key file doesnt exist cant read database settings.", certificate)
                Exit Function
            End If
            If fs.check_document_exists(encrypt_settings, Nothing) = False Then
                Dim oerr As New bc_cs_error_log("bc_cs_db_services", "read_encryped_settings", bc_cs_error_codes.USER_DEFINED, "Encrypted settings file doesnt exist cant read database settings.", certificate)
                Exit Function
            End If
            read_encryped_settings = False
            tmpfile = bc_cs_central_settings.central_repos_path + "\encrypt\bc_tmp.txt"
            REM read settings file
            Dim oencrypt_settings As New bc_cs_db_encrypt_settings
            oencrypt_settings = oencrypt_settings.read_data_from_file(encrypt_settings, Nothing)

            If bc_cs_central_settings.server_flag = 0 Then
                REM read key file and uncompress
                Dim bcs As New bc_cs_security
                Dim fkey As New StreamReader(keyfile)
                Dim s As String
                s = fkey.ReadToEnd
                s = bcs.decompress_xml(s, certificate)
                okey = okey.read_data_from_string(s, Nothing)
            Else
                okey = okey.read_data_from_file(keyfile, certificate)
            End If
            key = okey.key
            IV = okey.IV
            Dim oencrypt As New bc_cs_security.bc_cs_encrytion
            fs.write_bytestream_to_document(tmpfile, oencrypt_settings.dbname, certificate)
            oencrypt.decrypt(tmpfile, key, IV, bc_cs_central_settings.dbname)
            fs.write_bytestream_to_document(tmpfile, oencrypt_settings.servername, certificate)
            oencrypt.decrypt(tmpfile, key, IV, bc_cs_central_settings.servername)
            fs.write_bytestream_to_document(tmpfile, oencrypt_settings.username, certificate)
            oencrypt.decrypt(tmpfile, key, IV, bc_cs_central_settings.username)
            fs.write_bytestream_to_document(tmpfile, oencrypt_settings.password, certificate)
            oencrypt.decrypt(tmpfile, key, IV, bc_cs_central_settings.password)
            fs.delete_file(tmpfile)
            read_encryped_settings = "Data Source=SQLOLEDB;Server=" & bc_cs_central_settings.servername + ";Database=" & bc_cs_central_settings.dbname + ";UID=" & bc_cs_central_settings.username + ";PWD=" & bc_cs_central_settings.password & ";Pooling=true"

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_cs_db_services", "read_encryped_settings", bc_cs_error_codes.USER_DEFINED, "Encrypt mode cannot read encryted settings." + ex.Message, certificate)
            read_encryped_settings = ""
        End Try

    End Function
    <Serializable()> Private Class bc_cs_db_encrypt_settings
        Inherits bc_cs_soap_base_class

        Public dbname As Byte()
        Public servername As Byte()
        Public username As Byte()
        Public password As Byte()
        Public Sub New()

        End Sub
    End Class

    Private Function read_db_settings(ByVal filename As String, ByRef certificate As bc_cs_security.certificate) As Boolean
        Dim xmlload As New Xml.XmlDocument
        Dim myXmlNodeList As Xml.XmlNodeList

        'Export registry keys into current user key if there are no settinsg existing
        Try
            read_db_settings = False
            xmlload.Load(filename)
            myXmlNodeList = xmlload.SelectNodes("/settings/connectivity/ado/server")
            If myXmlNodeList.Count > 0 Then
                bc_cs_central_settings.servername = xmlload.SelectSingleNode("/settings/connectivity/ado/server").InnerXml()
            End If
            myXmlNodeList = xmlload.SelectNodes("/settings/connectivity/ado/name")
            If myXmlNodeList.Count > 0 Then
                bc_cs_central_settings.dbname = xmlload.SelectSingleNode("/settings/connectivity/ado/name").InnerXml()
            End If
            myXmlNodeList = xmlload.SelectNodes("/settings/connectivity/ado/user")
            If myXmlNodeList.Count > 0 Then
                bc_cs_central_settings.username = xmlload.SelectSingleNode("/settings/connectivity/ado/user").InnerXml()
            End If
            myXmlNodeList = xmlload.SelectNodes("/settings/connectivity/ado/password ")
            If myXmlNodeList.Count > 0 Then
                bc_cs_central_settings.password = xmlload.SelectSingleNode("/settings/connectivity/ado/password").InnerXml()
            End If
            read_db_settings = True

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_cs_db_services", "read_db_settings", bc_cs_error_codes.USER_DEFINED, "Encrypt mode cannot read seed file: " + filename + ". Cannot connect to database", certificate)
        End Try

    End Function
    Public Function open_conn(ByRef certificate As bc_cs_security.certificate) As Boolean
        Dim otrace As New bc_cs_activity_log("bc_db_services", "open_conn", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            open_conn = False
            gsqlcn = New SqlConnection
            gsqlcn.ConnectionString = Me.GetConnectionString(certificate)
            gsqlcn.Open()
            Dim cmd As New SqlCommand("begin tran", gsqlcn)
            cmd.CommandTimeout = bc_cs_central_settings.ado_timeout
            Dim odbtrace = New bc_cs_activity_log("bc_db_services", "executesql", bc_cs_activity_codes.SQL, "begin tran", certificate)
            odbtrace = New bc_cs_activity_log("bc_db_services", "executesql", bc_cs_activity_codes.COMMENTARY, "ADO timeout:" + bc_cs_central_settings.ado_timeout, certificate)
            cmd.ExecuteNonQuery()
            open_conn = True
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_db_services", "open_conn", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_db_services", "open_conn", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Function
    Public Function close_conn(ByVal commit As Boolean, ByRef certificate As bc_cs_security.certificate) As Boolean
        Dim otrace As New bc_cs_activity_log("bc_db_services", "close_conn", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try
            If IsNothing(gsqlcn) Then
                close_conn = True
                Exit Function
            End If
            If gsqlcn.State = ConnectionState.Closed Then
                close_conn = True
                Exit Function
            End If
            close_conn = False
            Dim cmd As SqlCommand
            If commit = True Then
                cmd = New SqlCommand("commit tran", gsqlcn)
            Else
                cmd = New SqlCommand("rollback tran", gsqlcn)
            End If
            cmd.CommandTimeout = bc_cs_central_settings.ado_timeout
            If commit = True Then
                Dim odbtrace = New bc_cs_activity_log("bc_db_services", "executesql", bc_cs_activity_codes.SQL, "commit tran", certificate)
            Else
                Dim odbtrace = New bc_cs_activity_log("bc_db_services", "executesql", bc_cs_activity_codes.SQL, "rollback tran", certificate)
            End If
            cmd.ExecuteNonQuery()
            gsqlcn.Close()
            close_conn = True
        Catch ex As Exception
            'Dim oerr As New bc_cs_error_log("bc_db_services", "close_conn", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            Dim ocomm As New bc_cs_activity_log("bc_db_services", "close_conn", bc_cs_activity_codes.COMMENTARY, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_db_services", "close_conn", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try


    End Function
    Public Function executesql_trans(ByVal strsql As String, ByRef certificate As bc_cs_security.certificate, Optional ByVal show_no_error As Boolean = False, Optional ByVal show_no_trace As Boolean = False) As Object
        Dim otrace As New bc_cs_activity_log("bc_db_services", "executesql_trans", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        ' Dim ocommentary As bc_cs_activity_log
        Dim i, j As Integer
        Dim row_Count As Integer

        Dim dr As SqlDataReader = Nothing

        Try
            Dim cmd As New SqlCommand(strsql, gsqlcn)
            cmd.CommandTimeout = bc_cs_central_settings.ado_timeout
            If show_no_trace = False Then
                Dim odbtrace = New bc_cs_activity_log("bc_db_services", "executesql_trans", bc_cs_activity_codes.SQL, strsql, certificate)
                odbtrace = New bc_cs_activity_log("bc_db_services", "executesql_trans", bc_cs_activity_codes.COMMENTARY, "ADO timeout:" + bc_cs_central_settings.ado_timeout, certificate)
            End If
            dr = cmd.ExecuteReader(CommandBehavior.Default)
            row_Count = 0
            Dim oresults As New List(Of Object)
            If dr.HasRows = True Then
                Do While dr.Read
                    For i = 0 To dr.FieldCount - 1

                        oresults.Add(dr.GetValue(i))
                    Next
                    row_Count = row_Count + 1
                Loop
            End If
            Dim tresults(dr.FieldCount, row_Count - 1) As Object
            For i = 0 To row_Count - 1
                For j = 0 To dr.FieldCount - 1
                    tresults(j, i) = oresults(i + (dr.FieldCount - 1) * i + j)
                Next
            Next
            dr.Close()
            executesql_trans = tresults
        Catch ex As Exception
            If show_no_error = False Then
                If bc_cs_central_settings.server_flag = 1 Then
                    Dim db_err As New bc_cs_error_log("bc_db_services", "executesql_trans", bc_cs_error_codes.DB_ERR, ex.Message + "::" + strsql, certificate)
                Else
                    Dim db_err As New bc_cs_error_log("bc_db_services", "executesql_trans", bc_cs_error_codes.DB_ERR, ex.Message, certificate)
                End If
                executesql_trans = Nothing
            Else
                If bc_cs_central_settings.server_flag = 1 Then
                    Dim ocomm As New bc_cs_activity_log("bc_db_services", "executesql_trans", bc_cs_activity_codes.COMMENTARY, ex.Message + "::" + strsql, certificate)
                Else
                    Dim ocomm As New bc_cs_activity_log("bc_db_services", "executesql_trans", bc_cs_activity_codes.COMMENTARY, ex.Message, certificate)
                End If

                Dim ret_tx(0, 0) As String
                ret_tx(0, 0) = "error: " + ex.Message + "::" + strsql
                executesql_trans = ret_tx
            End If
            Me.success = False
        Finally

            otrace = New bc_cs_activity_log("bc_db_services", "executesql_trans", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
            If Not (dr Is Nothing) Then
                dr.Close()
            End If
        End Try
    End Function

    Public Function executesql(ByVal strsql As String, ByRef certificate As bc_cs_security.certificate) As Object
        Dim otrace As New bc_cs_activity_log("bc_db_services", "executesql", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        ' Dim ocommentary As bc_cs_activity_log

        Dim i, j As Integer
        Dim row_Count As Integer
        Dim sqlcn As New SqlConnection

        sqlcn.ConnectionString = Me.GetConnectionString(certificate)


        Dim dr As SqlDataReader = Nothing

        Try
            '  ocommentary = New bc_cs_activity_log("bc_db_services", "open_connection", bc_cs_activity_codes.COMMENTARY, "Attempting to open Database for SQL Connection: " + CStr(sqlcn.ConnectionString))
            sqlcn.Open()

            '  ocommentary = New bc_cs_activity_log("bc_db_services", "open_connection", bc_cs_activity_codes.COMMENTARY, "Open Database Sucessful for SQL Connection: " + CStr(sqlcn.ConnectionString))
            Dim cmd As New SqlCommand(strsql, sqlcn)
            cmd.CommandTimeout = bc_cs_central_settings.ado_timeout

            Dim odbtrace = New bc_cs_activity_log("bc_db_services", "executesql", bc_cs_activity_codes.SQL, strsql, certificate)
            odbtrace = New bc_cs_activity_log("bc_db_services", "executesql", bc_cs_activity_codes.COMMENTARY, "ADO timeout:" + bc_cs_central_settings.ado_timeout, certificate)
            dr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            row_Count = 0


            Dim oresults As New List(Of Object)
            If dr.HasRows = True Then
                Do While dr.Read
                    For i = 0 To dr.FieldCount - 1

                        oresults.Add(dr.GetValue(i))
                    Next
                    row_Count = row_Count + 1
                Loop
            End If
            Dim tresults(dr.FieldCount, row_Count - 1) As Object
            For i = 0 To row_Count - 1
                For j = 0 To dr.FieldCount - 1
                    tresults(j, i) = oresults(i + (dr.FieldCount - 1) * i + j)
                Next
            Next
            dr.Close()
            executesql = tresults
        Catch ex As Exception
            If bc_cs_central_settings.server_flag = 1 Then
                Dim db_err As New bc_cs_error_log("bc_db_services", "executesql", bc_cs_error_codes.DB_ERR, ex.Message + "::" + strsql, certificate)
            Else
                Dim db_err As New bc_cs_error_log("bc_db_services", "executesql", bc_cs_error_codes.DB_ERR, ex.Message, certificate)
            End If
            executesql = Nothing
        Finally
            otrace = New bc_cs_activity_log("bc_db_services", "executesql", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
            If Not (dr Is Nothing) Then
                dr.Close()
            End If
            If Not (sqlcn Is Nothing) Then
                sqlcn.Close()
            End If

        End Try
    End Function
    Public Function executesql_return_error(ByVal strsql As String, ByRef certificate As bc_cs_security.certificate, ByRef err_tx As String, ByRef lgdb As bc_cs_db_services) As Object
        Dim otrace As New bc_cs_activity_log("bc_db_services", "executesql_return_error", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        ' Dim ocommentary As bc_cs_activity_log

        Dim i, j As Integer
        Dim row_Count As Integer
        Dim sqlcn As SqlConnection
        If IsNothing(lgdb) Then
            sqlcn = New SqlConnection
            sqlcn.ConnectionString = Me.GetConnectionString(certificate)
        End If

        Dim dr As SqlDataReader = Nothing
        err_tx = ""

        Try
            Dim cmd As SqlCommand
            If IsNothing(lgdb) Then
                sqlcn.Open()

                cmd = New SqlCommand(strsql, sqlcn)
            Else
                cmd = New SqlCommand(strsql, gsqlcn)
            End If
            cmd.CommandTimeout = bc_cs_central_settings.ado_timeout
            Dim odbtrace = New bc_cs_activity_log("bc_db_services", "executesql", bc_cs_activity_codes.SQL, strsql, certificate)
            odbtrace = New bc_cs_activity_log("bc_db_services", "executesql", bc_cs_activity_codes.COMMENTARY, "ADO timeout:" + bc_cs_central_settings.ado_timeout, certificate)
            If IsNothing(lgdb) Then
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            Else
                dr = cmd.ExecuteReader()
            End If
            row_Count = 0


            Dim oresults As New List(Of Object)
            If dr.HasRows = True Then
                Do While dr.Read
                    For i = 0 To dr.FieldCount - 1

                        oresults.Add(dr.GetValue(i))
                    Next
                    row_Count = row_Count + 1
                Loop
            End If
            Dim tresults(dr.FieldCount, row_Count - 1) As Object
            For i = 0 To row_Count - 1
                For j = 0 To dr.FieldCount - 1
                    tresults(j, i) = oresults(i + (dr.FieldCount - 1) * i + j)
                Next
            Next
            dr.Close()
            executesql_return_error = tresults
        Catch ex As Exception
            'If bc_cs_central_settings.server_flag = 1 Then
            '    Dim db_err As New bc_cs_error_log("bc_db_services", "executesql_return_error", bc_cs_error_codes.DB_ERR, ex.Message + "::" + strsql, certificate)
            'Else
            '    Dim db_err As New bc_cs_error_log("bc_db_services", "executesql_return_error", bc_cs_error_codes.DB_ERR, ex.Message, certificate)
            'End If
            err_tx = ex.Message + "::" + strsql
            executesql_return_error = Nothing
        Finally
            otrace = New bc_cs_activity_log("bc_db_services", "executesql_return_error", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
            If Not (dr Is Nothing) Then
                dr.Close()
            End If
            If Not (sqlcn Is Nothing) AndAlso IsNothing(lgdb) Then
                sqlcn.Close()
            End If

        End Try
    End Function
    Public Function executesql_return_error_no_tran(ByVal strsql As String, ByRef certificate As bc_cs_security.certificate, ByRef err_tx As String) As Object
        Dim otrace As New bc_cs_activity_log("bc_db_services", "executesql_return_error", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        ' Dim ocommentary As bc_cs_activity_log

        Dim i, j As Integer
        Dim row_Count As Integer
        Dim sqlcn As SqlConnection
        sqlcn = New SqlConnection
        sqlcn.ConnectionString = Me.GetConnectionString(certificate)

        Dim dr As SqlDataReader = Nothing
        err_tx = ""

        Try
            Dim cmd As SqlCommand
            sqlcn.Open()

            cmd = New SqlCommand(strsql, sqlcn)

            cmd.CommandTimeout = bc_cs_central_settings.ado_timeout
            Dim odbtrace = New bc_cs_activity_log("bc_db_services", "executesql", bc_cs_activity_codes.SQL, strsql, certificate)
            odbtrace = New bc_cs_activity_log("bc_db_services", "executesql", bc_cs_activity_codes.COMMENTARY, "ADO timeout:" + bc_cs_central_settings.ado_timeout, certificate)
            dr = cmd.ExecuteReader(CommandBehavior.CloseConnection)

            row_Count = 0


            Dim oresults As New List(Of Object)
            If dr.HasRows = True Then
                Do While dr.Read
                    For i = 0 To dr.FieldCount - 1

                        oresults.Add(dr.GetValue(i))
                    Next
                    row_Count = row_Count + 1
                Loop
            End If
            Dim tresults(dr.FieldCount, row_Count - 1) As Object
            For i = 0 To row_Count - 1
                For j = 0 To dr.FieldCount - 1
                    tresults(j, i) = oresults(i + (dr.FieldCount - 1) * i + j)
                Next
            Next
            dr.Close()
            executesql_return_error_no_tran = tresults
        Catch ex As Exception
            If bc_cs_central_settings.server_flag = 1 Then
                Dim db_err As New bc_cs_error_log("bc_db_services", "executesql_return_error", bc_cs_error_codes.DB_ERR, ex.Message + "::" + strsql, certificate)
            Else
                Dim db_err As New bc_cs_error_log("bc_db_services", "executesql_return_error", bc_cs_error_codes.DB_ERR, ex.Message, certificate)
            End If
            err_tx = ex.Message + "::" + strsql
            executesql_return_error_no_tran = Nothing
        Finally
            otrace = New bc_cs_activity_log("bc_db_services", "executesql_return_error", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
            If Not (dr Is Nothing) Then
                dr.Close()
            End If
            If Not (sqlcn Is Nothing) Then
                sqlcn.Close()
            End If

        End Try
    End Function
    Public Function executesql_extended_timeout(ByVal strsql As String, ByRef certificate As bc_cs_security.certificate) As Object
        Dim otrace As New bc_cs_activity_log("bc_db_services", "executesql_from_async_web_service", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        ' Dim ocommentary As bc_cs_activity_log
        Dim i, j As Integer
        Dim row_Count As Integer
        Dim sqlcn As New SqlConnection
        sqlcn.ConnectionString = Me.GetConnectionString(certificate)

        Dim dr As SqlDataReader = Nothing

        Try
            '  ocommentary = New bc_cs_activity_log("bc_db_services", "open_connection", bc_cs_activity_codes.COMMENTARY, "Attempting to open Database for SQL Connection: " + CStr(sqlcn.ConnectionString))
            sqlcn.Open()

            '  ocommentary = New bc_cs_activity_log("bc_db_services", "open_connection", bc_cs_activity_codes.COMMENTARY, "Open Database Sucessful for SQL Connection: " + CStr(sqlcn.ConnectionString))
            Dim cmd As New SqlCommand(strsql, sqlcn)
            cmd.CommandTimeout = bc_cs_central_settings.extended_timeout

            Dim odbtrace = New bc_cs_activity_log("bc_db_services", "executesql_extended_timeout", bc_cs_activity_codes.SQL, strsql, certificate)
            odbtrace = New bc_cs_activity_log("bc_db_services", "executesql_extended_timeout", bc_cs_activity_codes.COMMENTARY, "ADO extended timeout:" + bc_cs_central_settings.extended_timeout, certificate)
            dr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            row_Count = 0

            Dim oresults As New List(Of Object)
            If dr.HasRows = True Then
                Do While dr.Read
                    For i = 0 To dr.FieldCount - 1

                        oresults.Add(dr.GetValue(i))
                    Next
                    row_Count = row_Count + 1
                Loop
            End If
            Dim tresults(dr.FieldCount, row_Count - 1) As Object
            For i = 0 To row_Count - 1
                For j = 0 To dr.FieldCount - 1
                    tresults(j, i) = oresults(i + (dr.FieldCount - 1) * i + j)
                Next
            Next
            dr.Close()
            executesql_extended_timeout = tresults
        Catch ex As Exception
            If bc_cs_central_settings.server_flag = 1 Then
                Dim db_err As New bc_cs_error_log("bc_db_services", "executesql_extended_timeout", bc_cs_error_codes.DB_ERR, ex.Message + "::" + strsql, certificate)
            Else
                Dim db_err As New bc_cs_error_log("bc_db_services", "executesql_extended_timeout", bc_cs_error_codes.DB_ERR, ex.Message, certificate)
            End If
            executesql_extended_timeout = Nothing
        Finally
            otrace = New bc_cs_activity_log("bc_db_services", "executesql_extended_timeout", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
            If Not (dr Is Nothing) Then
                dr.Close()
            End If
            If Not (sqlcn Is Nothing) Then
                sqlcn.Close()
            End If
        End Try
    End Function
    'retries if connection limit or deadlock
    Public Function executesql_no_timeout_retry_cp_dl(ByVal strsql As String, ByRef certificate As bc_cs_security.certificate) As Object
        Dim otrace As New bc_cs_activity_log("bc_db_services", "executesql_no_timeout_cp_dl", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        ' Dim ocommentary As bc_cs_activity_log
        Dim i, j As Integer
        Dim row_Count As Integer
        Dim sqlcn As New SqlConnection
        sqlcn.ConnectionString = Me.GetConnectionString(certificate)

        Dim dr As SqlDataReader = Nothing

        Try
            sqlcn.Open()

            Dim cmd As New SqlCommand(strsql, sqlcn)
            cmd.CommandTimeout = 3600

            Dim odbtrace = New bc_cs_activity_log("bc_db_services", "executesql_no_timeout_cp_dl", bc_cs_activity_codes.SQL, strsql, certificate)
            dr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            row_Count = 0

            Dim oresults As New List(Of Object)
            If dr.HasRows = True Then
                Do While dr.Read
                    For i = 0 To dr.FieldCount - 1

                        oresults.Add(dr.GetValue(i))
                    Next
                    row_Count = row_Count + 1
                Loop
            End If
            Dim tresults(dr.FieldCount, row_Count - 1) As Object
            For i = 0 To row_Count - 1
                For j = 0 To dr.FieldCount - 1
                    tresults(j, i) = oresults(i + (dr.FieldCount - 1) * i + j)
                Next
            Next
            dr.Close()
            executesql_no_timeout_retry_cp_dl = tresults
            success = True

        Catch ex As Exception
            success = False
            If InStr(ex.Message, "deadlock") > 1 Or InStr(ex.Message, "timeout") > 1 Then
                rp_dl = True
                executesql_no_timeout_retry_cp_dl = Nothing
                otrace = New bc_cs_activity_log("bc_db_services", "executesql_no_timeout_cp_dl", bc_cs_activity_codes.TRACE_EXIT, "pool or deadlock:" + ex.Message, certificate)
                Exit Function
            End If
            If bc_cs_central_settings.server_flag = 1 Then
                Dim db_err As New bc_cs_error_log("bc_db_services", "executesql_no_timeout_cp_dl ", bc_cs_error_codes.DB_ERR, ex.Message + "::" + strsql, certificate)
            Else
                Dim db_err As New bc_cs_error_log("bc_db_services", "executesql_no_timeout_cp_dl ", bc_cs_error_codes.DB_ERR, ex.Message, certificate)
            End If
            rp_dl = False
            executesql_no_timeout_retry_cp_dl = Nothing

        Finally
            otrace = New bc_cs_activity_log("bc_db_services", "executesql_no_timeout_cp_dl", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
            If Not (dr Is Nothing) Then
                dr.Close()
            End If
            If Not (sqlcn Is Nothing) Then
                sqlcn.Close()
            End If
        End Try
    End Function



    Public Function executesql_no_timeout(ByVal strsql As String, ByRef certificate As bc_cs_security.certificate) As Object
        Dim otrace As New bc_cs_activity_log("bc_db_services", "executesql_no_timeout", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        ' Dim ocommentary As bc_cs_activity_log
        Dim i, j As Integer
        Dim row_Count As Integer
        Dim sqlcn As New SqlConnection
        sqlcn.ConnectionString = Me.GetConnectionString(certificate)

        Dim dr As SqlDataReader = Nothing

        Try
            '  ocommentary = New bc_cs_activity_log("bc_db_services", "open_connection", bc_cs_activity_codes.COMMENTARY, "Attempting to open Database for SQL Connection: " + CStr(sqlcn.ConnectionString))
            sqlcn.Open()

            '  ocommentary = New bc_cs_activity_log("bc_db_services", "open_connection", bc_cs_activity_codes.COMMENTARY, "Open Database Sucessful for SQL Connection: " + CStr(sqlcn.ConnectionString))
            Dim cmd As New SqlCommand(strsql, sqlcn)
            cmd.CommandTimeout = 3600

            Dim odbtrace = New bc_cs_activity_log("bc_db_services", "executesql_no_timeout", bc_cs_activity_codes.SQL, strsql, certificate)
            'odbtrace = New bc_cs_activity_log("bc_db_services", "executesql_extended_timeout", bc_cs_activity_codes.COMMENTARY, "ADO extended timeout:" + bc_cs_central_settings.extended_timeout, certificate)
            dr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            row_Count = 0

            Dim oresults As New List(Of Object)
            If dr.HasRows = True Then
                Do While dr.Read
                    For i = 0 To dr.FieldCount - 1

                        oresults.Add(dr.GetValue(i))
                    Next
                    row_Count = row_Count + 1
                Loop
            End If
            Dim tresults(dr.FieldCount, row_Count - 1) As Object
            For i = 0 To row_Count - 1
                For j = 0 To dr.FieldCount - 1
                    tresults(j, i) = oresults(i + (dr.FieldCount - 1) * i + j)
                Next
            Next
            dr.Close()
            executesql_no_timeout = tresults
            success = True

        Catch ex As Exception
            If bc_cs_central_settings.server_flag = 1 Then
                Dim db_err As New bc_cs_error_log("bc_db_services", "executesql_no_timeout", bc_cs_error_codes.DB_ERR, ex.Message + "::" + strsql, certificate)
            Else
                Dim db_err As New bc_cs_error_log("bc_db_services", "executesql_no_timeout", bc_cs_error_codes.DB_ERR, ex.Message, certificate)
            End If
            executesql_no_timeout = Nothing
        Finally
            otrace = New bc_cs_activity_log("bc_db_services", "executesql_no_timeout", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
            If Not (dr Is Nothing) Then
                dr.Close()
            End If
            If Not (sqlcn Is Nothing) Then
                sqlcn.Close()
            End If
        End Try
    End Function
    REM routine tests sql
    REM returns error message of fails
    Public Function test_sql(ByVal strsql As String, ByRef certificate As bc_cs_security.certificate) As String
        Dim otrace As New bc_cs_activity_log("bc_db_services", "test_sql", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        ' Dim ocommentary As bc_cs_activity_log
        Dim row_Count As Integer
        Dim sqlcn As New SqlConnection
        sqlcn.ConnectionString = Me.GetConnectionString(certificate)

        Dim dr As SqlDataReader = Nothing


        Try
            '  ocommentary = New bc_cs_activity_log("bc_db_services", "open_connection", bc_cs_activity_codes.COMMENTARY, "Attempting to open Database for SQL Connection: " + CStr(sqlcn.ConnectionString))
            sqlcn.Open()

            '  ocommentary = New bc_cs_activity_log("bc_db_services", "open_connection", bc_cs_activity_codes.COMMENTARY, "Open Database Sucessful for SQL Connection: " + CStr(sqlcn.ConnectionString))
            Dim cmd As New SqlCommand(strsql, sqlcn)
            cmd.CommandTimeout = bc_cs_central_settings.ado_timeout
            Dim odbtrace = New bc_cs_activity_log("bc_db_services", "executesql", bc_cs_activity_codes.SQL, strsql, certificate)
            odbtrace = New bc_cs_activity_log("bc_db_services", "executesql", bc_cs_activity_codes.COMMENTARY, "ADO timeout:" + bc_cs_central_settings.ado_timeout, certificate)
            dr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            row_Count = 0


            dr.Close()
            Return ""
        Catch ex As Exception
            test_sql = CStr(ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_db_services", "test_sql", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
            If Not (dr Is Nothing) Then
                dr.Close()
            End If
            If Not (sqlcn Is Nothing) Then
                sqlcn.Close()
            End If
        End Try
    End Function
    'allred
    Public Function executexmlsql_show_no_error(ByVal strsql As String, Optional ByRef certificate As bc_cs_security.certificate = Nothing) As Object
        ' Dim otrace As New bc_cs_activity_log("bc_db_services", "executesql", bc_cs_activity_codes.TRACE_ENTRY, "")
        ' Dim ocommentary As bc_cs_activity_log

        Dim row_Count As Integer
        Dim sqlcn As New SqlConnection
        sqlcn.ConnectionString = Me.GetConnectionString(certificate)
        Dim xr As Xml.XmlReader = Nothing

        Try
            '  ocommentary = New bc_cs_activity_log("bc_db_services", "open_connection", bc_cs_activity_codes.COMMENTARY, "Attempting to open Database for SQL Connection: " + CStr(sqlcn.ConnectionString))
            sqlcn.Open()

            '  ocommentary = New bc_cs_activity_log("bc_db_services", "open_connection", bc_cs_activity_codes.COMMENTARY, "Open Database Sucessful for SQL Connection: " + CStr(sqlcn.ConnectionString))
            Dim cmd As New SqlCommand(strsql, sqlcn)
            cmd.CommandTimeout = bc_cs_central_settings.ado_timeout

            'Dim odbtrace = New bc_cs_activity_log("bc_db_services", "executesql_show_no_error", bc_cs_activity_codes.SQL, strsql, certificate)
            xr = cmd.ExecuteXmlReader()
            'CommandBehavior.CloseConnection
            row_Count = 0
            Dim oresults As Object

            If Not xr Is Nothing Then

                Dim sb = New System.Text.StringBuilder()

                While (xr.Read())
                    sb.AppendLine(xr.ReadOuterXml())
                End While

                ReDim oresults(1, 1)

                oresults(0, 0) = sb.ToString()

                executexmlsql_show_no_error = oresults

            Else
                executexmlsql_show_no_error = Nothing
            End If
            'If dr.HasRows = True Then
            '    ReDim oresults(dr.FieldCount - 1, MAX_ROWS)
            '    Do While dr.Read
            '        For i = 0 To dr.FieldCount - 1
            '            dr.GetValue(i)
            '            oresults(i, row_Count) = dr.GetValue(i)
            '        Next
            '        row_Count = row_Count + 1
            '    Loop
            '    Dim tresults(dr.FieldCount, row_Count - 1) As Object
            '    For i = 0 To row_Count - 1
            '        For j = 0 To dr.FieldCount - 1
            '            tresults(j, i) = oresults(j, i)
            '        Next
            '    Next
            '    executexmlsql_show_no_error = tresults
            'Else
            '    executexmlsql_show_no_error = Nothing
            'End If
            xr.Close()

        Catch ex As Exception
            Dim tresults(1, 0) As Object
            tresults(0, 0) = "Error"
            tresults(1, 0) = ex.Message
            executexmlsql_show_no_error = tresults
        Finally
            'Dim otrace As New bc_cs_activity_log("bc_db_services", "executesql", bc_cs_activity_codes.TRACE_EXIT, "")
            If Not (xr Is Nothing) Then
                xr.Close()
            End If
            If Not (sqlcn Is Nothing) Then
                sqlcn.Close()
            End If
        End Try
    End Function
    Public Function executesql_show_no_error(ByVal strsql As String) As Object
        ' Dim otrace As New bc_cs_activity_log("bc_db_services", "executesql", bc_cs_activity_codes.TRACE_ENTRY, "")
        ' Dim ocommentary As bc_cs_activity_log
        Dim i, j As Integer
        Dim row_Count As Integer
        Dim sqlcn As New SqlConnection
        sqlcn.ConnectionString = Me.GetConnectionString(Nothing)
        Dim dr As SqlDataReader = Nothing

        Try
            '  ocommentary = New bc_cs_activity_log("bc_db_services", "open_connection", bc_cs_activity_codes.COMMENTARY, "Attempting to open Database for SQL Connection: " + CStr(sqlcn.ConnectionString))
            sqlcn.Open()

            '  ocommentary = New bc_cs_activity_log("bc_db_services", "open_connection", bc_cs_activity_codes.COMMENTARY, "Open Database Sucessful for SQL Connection: " + CStr(sqlcn.ConnectionString))
            Dim cmd As New SqlCommand(strsql, sqlcn)
            cmd.CommandTimeout = bc_cs_central_settings.ado_timeout

            'Dim odbtrace = New bc_cs_activity_log("bc_db_services", "executesql_show_no_error", bc_cs_activity_codes.SQL, strsql, certificate)
            dr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            row_Count = 0

            If dr.HasRows = True Then
                Dim oresults As New List(Of Object)
                If dr.HasRows = True Then
                    Do While dr.Read
                        For i = 0 To dr.FieldCount - 1

                            oresults.Add(dr.GetValue(i))
                        Next
                        row_Count = row_Count + 1
                    Loop
                End If
                Dim tresults(dr.FieldCount, row_Count - 1) As Object
                For i = 0 To row_Count - 1
                    For j = 0 To dr.FieldCount - 1
                        tresults(j, i) = oresults(i + (dr.FieldCount - 1) * i + j)
                    Next
                Next

                executesql_show_no_error = tresults
            Else
                executesql_show_no_error = Nothing
            End If
            dr.Close()

        Catch ex As Exception
            Dim tresults(1, 0) As Object
            tresults(0, 0) = "Error"
            tresults(1, 0) = ex.Message
            executesql_show_no_error = tresults
        Finally
            'Dim otrace As New bc_cs_activity_log("bc_db_services", "executesql", bc_cs_activity_codes.TRACE_EXIT, "")
            If Not (dr Is Nothing) Then
                dr.Close()
            End If
            If Not (sqlcn Is Nothing) Then
                sqlcn.Close()
            End If
        End Try
    End Function

    Public Function executesp_for_component_from_web(ByVal sp_name As String, ByVal entity_id As String, ByVal pub_type_id As Long, ByVal lang_id As Integer, ByVal contributor_id As Integer, ByVal stage_id As Long, ByVal acc_standard As Long, ByVal ddata_at_date As Date, ByVal doc_id As String, ByVal parameters As Object, ByVal row As Integer, ByVal col As Integer, ByVal parent_entity_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim otrace As New bc_cs_activity_log("bc_db_services", "executesp_for_component_from_web", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim rval As Object = Nothing
        Dim sqlcn As New SqlConnection
        Dim sql As String = ""
        Dim i, j As Integer
        Dim res As Object = Nothing

        Dim ocommentary As bc_cs_activity_log
        Dim procParameterCount As Integer
        Dim data_at_date As String
        Dim user_id As Long
        Try
            If ddata_at_date = "9-9-9999" Then
                data_at_date = "9-9-9999"
            Else
                data_at_date = ddata_at_date
                data_at_date = Format(ddata_at_date, "yyyyMMdd")
            End If

            REM get the user id
            REM Steve wooderson 17/06/2013
            If bc_cs_central_settings.server_flag = 0 Then
                user_id = bc_cs_central_settings.logged_on_user_id
            Else
                user_id = certificate.user_id
            End If

            REM Get the number of parameters in the target stored proc.
            REM Steve wooderson 17/06/2013
            sqlcn.ConnectionString = Me.GetConnectionString(certificate)
            Dim pcmd As New SqlCommand(sql, sqlcn)
            pcmd.CommandTimeout = bc_cs_central_settings.ado_timeout
            pcmd.Connection = sqlcn
            pcmd.CommandText = sp_name
            pcmd.CommandType = Data.CommandType.StoredProcedure
            sqlcn.Open()
            SqlCommandBuilder.DeriveParameters(pcmd)
            procParameterCount = pcmd.Parameters.Count - 1

            Dim param As bc_cs_sql_parameter
            Dim params As New List(Of bc_cs_sql_parameter)

            For i = 1 To pcmd.Parameters.Count - 1
                Dim ocomm As New bc_cs_activity_log("aa", "nn:" + CStr(parameters.component_template_parameters.count), bc_cs_activity_codes.COMMENTARY, pcmd.Parameters(i).ParameterName, certificate)

                param = New bc_cs_sql_parameter
                param.name = pcmd.Parameters(i).ParameterName
                param.name = Right(param.name, Len(param.name) - 1)
                If doc_id = "" Then
                    doc_id = " "
                End If
                If i < 10 Then
                    If (i = 1) Then
                        param.value = entity_id
                    ElseIf (i = 2) Then
                        param.value = pub_type_id
                    ElseIf (i = 3) Then
                        param.value = lang_id
                    ElseIf (i = 4) Then
                        param.value = stage_id
                    ElseIf (i = 5) Then
                        param.value = contributor_id
                    ElseIf (i = 6) Then
                        param.value = 1
                    ElseIf (i = 7) Then
                        param.value = doc_id
                    ElseIf (i = 8) Then
                        param.value = data_at_date
                    ElseIf (i = 9) Then
                        param.value = certificate.user_id

                    End If
                Else
                    REM custom parameters#
                    With parameters.component_template_parameters(i - 10)
                        If .datatype = 9 Then
                            Try
                                param.value = .context_items.write_data_to_xml(certificate)
                            Catch
                            End Try
                        Else


                            If .system_defined = True Then
                                param.value = "null"
                            Else
                                If .default_value_id <> "" Then
                                    param.value = .default_value_id
                                Else
                                    param.value = .default_value
                                End If
                            End If
                        End If
                    End With



                End If
                params.Add(param)
            Next
            executesp_for_component_from_web = executesql_with_parameters(sp_name, params, certificate)

        Catch ex As Exception

            Dim db_err As New bc_cs_error_log("bc_db_services", "executesp_from_web", bc_cs_error_codes.DB_ERR, ex.Message, certificate)

        Finally
            ocommentary = New bc_cs_activity_log("bc_db_services", "executesp_from_web", bc_cs_activity_codes.COMMENTARY, "Stored Procedure returned value: " + CStr(rval), certificate)
            If Not (sqlcn Is Nothing) Then
                sqlcn.Close()
            End If
        End Try
    End Function


    REM executes a component stored procedure
    Public Function executesp_for_component(ByVal sp_name As String, ByVal entity_id As String, ByVal pub_type_id As Long, ByVal lang_id As Integer, ByVal contributor_id As Integer, ByVal stage_id As Long, ByVal acc_standard As Long, ByVal ddata_at_date As Date, ByVal doc_id As String, ByVal parameters As Object, ByVal row As Integer, ByVal col As Integer, ByVal parent_entity_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        ' Dim otrace As New bc_cs_activity_log("bc_db_services", "executesp", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim rval As Object = Nothing
        Dim sqlcn As New SqlConnection
        Dim sql As String = ""
        Dim i, j As Integer
        Dim res As Object = Nothing
        Dim tsql As String
        Dim psql As String
        Dim ocommentary As bc_cs_activity_log
        Dim procParameterCount As Integer
        Dim data_at_date As String
        Dim user_id As Long
        If ddata_at_date = "9-9-9999" Then
            data_at_date = "9-9-9999"
        Else
            data_at_date = ddata_at_date
            data_at_date = Format(ddata_at_date, "yyyyMMdd")
        End If

        REM get the user id
        REM Steve wooderson 17/06/2013
        If bc_cs_central_settings.server_flag = 0 Then
            user_id = bc_cs_central_settings.logged_on_user_id
        Else
            user_id = certificate.user_id
        End If

        REM Get the number of parameters in the target stored proc.
        REM Steve wooderson 17/06/2013
        sqlcn.ConnectionString = Me.GetConnectionString(certificate)
        Dim pcmd As New SqlCommand(sql, sqlcn)
        pcmd.CommandTimeout = bc_cs_central_settings.ado_timeout
        pcmd.Connection = sqlcn
        pcmd.CommandText = sp_name
        pcmd.CommandType = Data.CommandType.StoredProcedure
        sqlcn.Open()
        SqlCommandBuilder.DeriveParameters(pcmd)
        procParameterCount = pcmd.Parameters.Count - 1




        REM see if one of the parameters is type 9 context list
        For j = 0 To parameters.component_template_parameters.count - 1
            If parameters.component_template_parameters(j).datatype = 9 Then

                ocommentary = New bc_cs_activity_log("bc_db_services", "executesp", bc_cs_activity_codes.COMMENTARY, "Using Context Items Component Refresh Stored Procedure interface for: " + sp_name, certificate)
                If pcmd.Parameters.Item(9).ToString = "@user_id" Then
                    sql = "exec " + sp_name + " " + CStr(entity_id) + "," + CStr(pub_type_id) + "," + CStr(lang_id) + "," + CStr(stage_id) + "," + CStr(contributor_id) + "," + CStr(acc_standard) + ",'" + CStr(doc_id) + "','" + data_at_date + "','" + CStr(user_id) + "'"
                Else
                    sql = "exec " + sp_name + " " + CStr(entity_id) + "," + CStr(pub_type_id) + "," + CStr(lang_id) + "," + CStr(stage_id) + "," + CStr(contributor_id) + "," + CStr(acc_standard) + ",'" + CStr(doc_id) + "','" + data_at_date + "'"
                End If

                REM now add parameters
                psql = ""
                For i = 0 To parameters.component_template_parameters.count - 1
                    With parameters.component_template_parameters(i)
                        If .datatype = 9 Then

                            REM serialize context items
                            Dim scontext As String = ""
                            Try
                                scontext = .context_items.write_data_to_xml(certificate)
                            Catch
                            End Try
                            psql = psql + ",'" + scontext + "'"
                        Else
                            If .system_defined = True Then
                                psql = psql + "," + "null"
                            Else
                                If .default_value_id <> "" Then
                                    psql = psql + ",'" + .default_value_id + "'"
                                Else
                                    psql = psql + ",'" + .default_value + "'"
                                End If
                            End If
                        End If
                    End With
                Next
                tsql = sql + psql
                res = executesql(tsql, certificate)
                executesp_for_component = res
                REM Arden July 2019
                If Not (sqlcn Is Nothing) Then
                    sqlcn.Close()
                End If
                Exit Function
            End If
        Next

        Try

            REM Decide which interface to use.
            REM case 1 Parameter Count 3
            If procParameterCount = 3 Then
                ocommentary = New bc_cs_activity_log("bc_db_services", "executesp", bc_cs_activity_codes.COMMENTARY, "Using Component Refresh case 1 interface for: " + sp_name, certificate)
                res = execute_component_sql(sqlcn, sp_name, entity_id, lang_id, certificate)
            End If

            REM case 2 ) Parameter Count 4 (third parameter @user_id)
            If procParameterCount = 4 Then
                If pcmd.Parameters.Item(3).ToString = "@user_id" Then
                    ocommentary = New bc_cs_activity_log("bc_db_services", "executesp", bc_cs_activity_codes.COMMENTARY, "Using Component Refresh case 2 interface for: " + sp_name, certificate)
                    res = execute_component_sql(sqlcn, sp_name, entity_id, lang_id, user_id, certificate)
                End If
            End If

            REM case 3 Parameter Count 4 (third parameter @parent_entity_id)
            If procParameterCount = 4 Then
                If pcmd.Parameters.Item(3).ToString = "@parent_entity_id" Then
                    ocommentary = New bc_cs_activity_log("bc_db_services", "executesp", bc_cs_activity_codes.COMMENTARY, "Using Component Refresh case 3 interface for: " + sp_name, certificate)
                    res = execute_component_sql(sqlcn, sp_name, entity_id, lang_id, parent_entity_id, "A", certificate)
                End If
            End If

            REM case 4 Parameter Count 5 
            If procParameterCount = 5 Then
                ocommentary = New bc_cs_activity_log("bc_db_services", "executesp", bc_cs_activity_codes.COMMENTARY, "Using Component Refresh case 4 interface for: " + sp_name, certificate)
                res = execute_component_sql(sqlcn, sp_name, entity_id, lang_id, parent_entity_id, user_id, certificate)
            End If

            REM case 5 Parameter Count =>8 (if > 8 check 9th parameter not equal to @parent_entity_id or @user_id)
            If procParameterCount >= 8 Then
                If procParameterCount = 8 Then
                    ocommentary = New bc_cs_activity_log("bc_db_services", "executesp", bc_cs_activity_codes.COMMENTARY, "Using Component Refresh case 5 interface for: " + sp_name, certificate)
                    res = execute_component_sql(sqlcn, sp_name, parameters, entity_id, pub_type_id, lang_id, stage_id, contributor_id, acc_standard, doc_id, data_at_date, certificate)
                End If
                If procParameterCount > 8 Then
                    If (pcmd.Parameters.Item(9).ToString <> "@parent_entity_id" And pcmd.Parameters.Item(9).ToString <> "@user_id") Then
                        ocommentary = New bc_cs_activity_log("bc_db_services", "executesp", bc_cs_activity_codes.COMMENTARY, "Using Component Refresh case 5 interface for: " + sp_name, certificate)
                        res = execute_component_sql(sqlcn, sp_name, parameters, entity_id, pub_type_id, lang_id, stage_id, contributor_id, acc_standard, doc_id, data_at_date, certificate)
                    End If
                End If
            End If

            REM case 6 Parameter Count =>9 (Check 9th parameter equal to @parent_entity_id)
            If procParameterCount >= 9 Then
                If pcmd.Parameters.Item(9).ToString = "@parent_entity_id" Then
                    ocommentary = New bc_cs_activity_log("bc_db_services", "executesp", bc_cs_activity_codes.COMMENTARY, "Using Component Refresh case 6 interface for: " + sp_name, certificate)
                    res = execute_component_sql(sqlcn, sp_name, parameters, entity_id, pub_type_id, lang_id, stage_id, contributor_id, acc_standard, doc_id, data_at_date, parent_entity_id, "A", certificate)
                End If
            End If

            REM case 7 Parameter Count =>9 (Check 9th parameter equal to @user_id & 10th parameter not equal to @parent_entity_id)
            If procParameterCount >= 9 Then
                If procParameterCount = 9 And pcmd.Parameters.Item(9).ToString = "@user_id" Then
                    ocommentary = New bc_cs_activity_log("bc_db_services", "executesp", bc_cs_activity_codes.COMMENTARY, "Using Component Refresh case 7 interface for: " + sp_name, certificate)
                    res = execute_component_sql(sqlcn, sp_name, parameters, entity_id, pub_type_id, lang_id, stage_id, contributor_id, acc_standard, doc_id, data_at_date, user_id, certificate)
                End If
                If procParameterCount > 9 Then
                    If pcmd.Parameters.Item(9).ToString = "@user_id" And pcmd.Parameters.Item(10).ToString <> "@parent_entity_id" Then
                        ocommentary = New bc_cs_activity_log("bc_db_services", "executesp", bc_cs_activity_codes.COMMENTARY, "Using Component Refresh case 7 interface for: " + sp_name, certificate)
                        res = execute_component_sql(sqlcn, sp_name, parameters, entity_id, pub_type_id, lang_id, stage_id, contributor_id, acc_standard, doc_id, data_at_date, user_id, certificate)
                    End If
                End If
            End If

            REM case 8 Parameter Count =>10 (Check 9th parameter equal to @user_id & 10th parameter equal to @parent_entity_id)
            If procParameterCount >= 10 Then
                If pcmd.Parameters.Item(9).ToString = "@user_id" And pcmd.Parameters.Item(10).ToString = "@parent_entity_id" Then
                    ocommentary = New bc_cs_activity_log("bc_db_services", "executesp", bc_cs_activity_codes.COMMENTARY, "Using Component Refresh case 8 interface for: " + sp_name, certificate)
                    res = execute_component_sql(sqlcn, sp_name, parameters, entity_id, pub_type_id, lang_id, stage_id, contributor_id, acc_standard, doc_id, data_at_date, user_id, parent_entity_id, certificate)
                End If
            End If

        Catch ex As Exception

            Dim db_err As New bc_cs_error_log("bc_db_services", "executesp", bc_cs_error_codes.DB_ERR, ex.Message, certificate)

        Finally
            ocommentary = New bc_cs_activity_log("bc_db_services", "executesp", bc_cs_activity_codes.COMMENTARY, "Stored Procedure returned value: " + CStr(rval), certificate)
            If Not (sqlcn Is Nothing) Then
                sqlcn.Close()
            End If
            executesp_for_component = res

        End Try
    End Function

    Public Overloads Function execute_component_sql(ByVal sqlcn As SqlConnection, ByVal sp_name As String, ByVal entity_id As String, ByVal lang_id As Integer, ByRef certificate As bc_cs_security.certificate) As Object

        REM 2 parameter + output

        Dim tresults(0, 0) As Object
        Dim cmd As New SqlCommand(sp_name, sqlcn)
        cmd.CommandTimeout = bc_cs_central_settings.ado_timeout

        cmd.CommandType = Data.CommandType.StoredProcedure
        cmd.Parameters.AddWithValue("@entity_id", entity_id)
        cmd.Parameters.AddWithValue("@lang_id", lang_id)
        With cmd.Parameters.Add("@output_text", SqlDbType.NVarChar, 255)
            .Direction = ParameterDirection.Output
        End With

        cmd.ExecuteNonQuery()

        tresults(0, 0) = cmd.Parameters("@output_text").Value.ToString
        execute_component_sql = tresults

    End Function


    Public Overloads Function execute_component_sql(ByVal sqlcn As SqlConnection, ByVal sp_name As String, ByVal entity_id As String, ByVal lang_id As Integer, ByVal user_id As Long, ByRef certificate As bc_cs_security.certificate) As Object

        REM 3 parameter(user_id) + output

        Dim tresults(0, 0) As Object

        Dim cmd As New SqlCommand(sp_name, sqlcn)
        cmd.CommandTimeout = bc_cs_central_settings.ado_timeout

        cmd.CommandType = Data.CommandType.StoredProcedure
        cmd.Parameters.AddWithValue("@entity_id", entity_id)
        cmd.Parameters.AddWithValue("@lang_id", lang_id)
        cmd.Parameters.AddWithValue("@user_id", user_id)
        With cmd.Parameters.Add("@output_text", SqlDbType.NVarChar, 255)
            .Direction = ParameterDirection.Output
        End With

        cmd.ExecuteNonQuery()

        tresults(0, 0) = cmd.Parameters("@output_text").Value.ToString
        execute_component_sql = tresults

    End Function


    Public Overloads Function execute_component_sql(ByVal sqlcn As SqlConnection, ByVal sp_name As String, ByVal entity_id As String, ByVal lang_id As Integer, ByVal parent_entity_id As Long, ByVal user_id As Long, ByRef certificate As bc_cs_security.certificate) As Object

        REM 4 parameter + output

        Dim tresults(0, 0) As Object

        Dim cmd As New SqlCommand(sp_name, sqlcn)
        cmd.CommandTimeout = bc_cs_central_settings.ado_timeout

        cmd.CommandType = Data.CommandType.StoredProcedure
        cmd.Parameters.AddWithValue("@entity_id", entity_id)
        cmd.Parameters.AddWithValue("@lang_id", lang_id)
        cmd.Parameters.AddWithValue("@parent_entity_id", parent_entity_id)
        cmd.Parameters.AddWithValue("@user_id", user_id)
        With cmd.Parameters.Add("@output_text", SqlDbType.NVarChar, 255)
            .Direction = ParameterDirection.Output
        End With

        cmd.ExecuteNonQuery()

        tresults(0, 0) = cmd.Parameters("@output_text").Value.ToString
        execute_component_sql = tresults

    End Function


    Public Overloads Function execute_component_sql(ByVal sqlcn As SqlConnection, ByVal sp_name As String, ByVal entity_id As String, ByVal lang_id As Integer, ByVal parent_entity_id As Long, ByVal dummy As String, ByRef certificate As bc_cs_security.certificate) As Object

        REM 3 parameter(parent_entity_id) + output

        Dim tresults(0, 0) As Object

        Dim cmd As New SqlCommand(sp_name, sqlcn)
        cmd.CommandTimeout = bc_cs_central_settings.ado_timeout

        cmd.CommandType = Data.CommandType.StoredProcedure
        cmd.Parameters.AddWithValue("@entity_id", entity_id)
        cmd.Parameters.AddWithValue("@lang_id", lang_id)
        cmd.Parameters.AddWithValue("@parent_entity_id", parent_entity_id)
        With cmd.Parameters.Add("@output_text", SqlDbType.NVarChar, 255)
            .Direction = ParameterDirection.Output
        End With

        cmd.ExecuteNonQuery()

        tresults(0, 0) = cmd.Parameters("@output_text").Value.ToString
        execute_component_sql = tresults

    End Function

    Public Overloads Function execute_component_sql(ByVal sqlcn As SqlConnection, ByVal sp_name As String, ByVal parameters As Object, ByVal entity_id As String, ByVal pub_type_id As Long, ByVal lang_id As Integer, ByVal stage_id As Long, ByVal contributor_id As Integer, ByVal acc_standard As Long, ByVal doc_id As String, ByVal data_at_date As String, ByRef certificate As bc_cs_security.certificate) As Object

        REM 8+ parameters

        Dim res As Object
        Dim sql As String = ""
        Dim tsql As String
        Dim psql As String

        Dim cmd As New SqlCommand(sp_name, sqlcn)
        cmd.CommandTimeout = bc_cs_central_settings.ado_timeout
        cmd.CommandType = Data.CommandType.StoredProcedure

        sql = "exec " + sp_name + " " + CStr(entity_id) + "," + CStr(pub_type_id) + "," + CStr(lang_id) + "," + CStr(stage_id) + "," + CStr(contributor_id) + "," + CStr(acc_standard) + ",'" + CStr(doc_id) + "','" + data_at_date + "'"
        REM now add parameters
        psql = ""
        For i = 0 To parameters.component_template_parameters.count - 1
            With parameters.component_template_parameters(i)
                If .system_defined = True Then
                    psql = psql + "," + "null"
                Else
                    If .default_value_id <> "" Then
                        psql = psql + ",'" + .default_value_id + "'"
                    Else
                        psql = psql + ",'" + .default_value + "'"
                    End If
                End If
            End With
        Next
        tsql = sql + psql
        Dim ocomm As New bc_cs_activity_log("bc_cs_db_services", "execute_component_sql", bc_cs_activity_codes.COMMENTARY, tsql, certificate)

        res = executesql(tsql, certificate)

        execute_component_sql = res

    End Function

    Public Overloads Function execute_component_sql(ByVal sqlcn As SqlConnection, ByVal sp_name As String, ByVal parameters As Object, ByVal entity_id As String, ByVal pub_type_id As Long, ByVal lang_id As Integer, ByVal stage_id As Long, ByVal contributor_id As Integer, ByVal acc_standard As Long, ByVal doc_id As String, ByVal data_at_date As String, ByVal parent_entity_id As Long, ByVal dummy As String, ByRef certificate As bc_cs_security.certificate) As Object

        REM 9+ parameters + parent_entity_id

        Dim res As Object
        Dim sql As String = ""
        Dim tsql As String
        Dim psql As String

        Dim cmd As New SqlCommand(sp_name, sqlcn)
        cmd.CommandTimeout = bc_cs_central_settings.ado_timeout
        cmd.CommandType = Data.CommandType.StoredProcedure

        sql = "exec " + sp_name + " " + CStr(entity_id) + "," + CStr(pub_type_id) + "," + CStr(lang_id) + "," + CStr(stage_id) + "," + CStr(contributor_id) + "," + CStr(acc_standard) + ",'" + CStr(doc_id) + "','" + data_at_date + "','" + CStr(parent_entity_id) + "'"
        REM now add parameters
        psql = ""
        For i = 0 To parameters.component_template_parameters.count - 1
            With parameters.component_template_parameters(i)
                If .system_defined = True Then
                    psql = psql + "," + "null"
                Else
                    If .default_value_id <> "" Then
                        psql = psql + ",'" + .default_value_id + "'"
                    Else
                        psql = psql + ",'" + .default_value + "'"
                    End If
                End If
            End With
        Next
        tsql = sql + psql
        Dim ocomm As New bc_cs_activity_log("bc_cs_db_services", "execute_component_sql", bc_cs_activity_codes.COMMENTARY, tsql, certificate)

        res = executesql(tsql, certificate)

        execute_component_sql = res

    End Function

    Public Overloads Function execute_component_sql(ByVal sqlcn As SqlConnection, ByVal sp_name As String, ByVal parameters As Object, ByVal entity_id As String, ByVal pub_type_id As Long, ByVal lang_id As Integer, ByVal stage_id As Long, ByVal contributor_id As Integer, ByVal acc_standard As Long, ByVal doc_id As String, ByVal data_at_date As String, ByVal user_id As Long, ByRef certificate As bc_cs_security.certificate) As Object

        REM 9+ parameters + user_id
        Dim res As Object
        Dim sql As String = ""
        Dim tsql As String
        Dim psql As String

        Dim cmd As New SqlCommand(sp_name, sqlcn)
        cmd.CommandTimeout = bc_cs_central_settings.ado_timeout
        cmd.CommandType = Data.CommandType.StoredProcedure

        sql = "exec " + sp_name + " " + CStr(entity_id) + "," + CStr(pub_type_id) + "," + CStr(lang_id) + "," + CStr(stage_id) + "," + CStr(contributor_id) + "," + CStr(acc_standard) + ",'" + CStr(doc_id) + "','" + data_at_date + "','" + CStr(user_id) + "'"
        REM now add parameters
        psql = ""
        For i = 0 To parameters.component_template_parameters.count - 1
            With parameters.component_template_parameters(i)
                If .datatype = 15 Or .datatype = 16 Then
                    If .list_key_ids.count = 0 Then
                        psql = psql + "," + "' '"
                    Else
                        psql = psql + ",'"
                        For m = 0 To .list_key_ids.count - 1
                            psql = psql + CStr(.list_key_ids(m)) + ";"
                        Next
                        psql = psql + "'"
                    End If

                ElseIf .system_defined = True Then
                    psql = psql + "," + "null"
                Else
                    If .default_value_id <> "" Then
                        psql = psql + ",'" + .default_value_id + "'"
                    Else
                        psql = psql + ",'" + .default_value + "'"
                    End If
                End If
            End With
        Next
        tsql = sql + psql
        Dim ocomm As New bc_cs_activity_log("bc_cs_db_services", "execute_component_sql", bc_cs_activity_codes.COMMENTARY, tsql, certificate)

        res = executesql(tsql, certificate)

        execute_component_sql = res

    End Function
    REM FIL AUG 2013

    Public Overloads Function execute_component_sql(ByVal sqlcn As SqlConnection, ByVal sp_name As String, ByVal parameters As Object, ByVal entity_id As String, ByVal pub_type_id As Long, ByVal lang_id As Integer, ByVal stage_id As Long, ByVal contributor_id As Integer, ByVal acc_standard As Long, ByVal doc_id As String, ByVal data_at_date As String, ByVal user_id As Long, ByVal parent_entity_id As Long, ByRef certificate As bc_cs_security.certificate) As Object

        REM 10+ parameters + user_id/parent_entity_id

        Dim res As Object
        Dim sql As String = ""
        Dim tsql As String
        Dim psql As String

        Dim cmd As New SqlCommand(sp_name, sqlcn)
        cmd.CommandTimeout = bc_cs_central_settings.ado_timeout
        cmd.CommandType = Data.CommandType.StoredProcedure

        sql = "exec " + sp_name + " " + CStr(entity_id) + "," + CStr(pub_type_id) + "," + CStr(lang_id) + "," + CStr(stage_id) + "," + CStr(contributor_id) + "," + CStr(acc_standard) + ",'" + CStr(doc_id) + "','" + data_at_date + "','" + CStr(user_id) + "','" + CStr(parent_entity_id) + "'"
        REM now add parameters
        psql = ""
        For i = 0 To parameters.component_template_parameters.count - 1
            With parameters.component_template_parameters(i)
                If .system_defined = True Then
                    psql = psql + "," + "null"
                Else
                    If .default_value_id <> "" Then
                        psql = psql + ",'" + .default_value_id + "'"
                    Else
                        psql = psql + ",'" + .default_value + "'"
                    End If
                End If
            End With
        Next
        tsql = sql + psql
        Dim ocomm As New bc_cs_activity_log("bc_cs_db_services", "execute_component_sql", bc_cs_activity_codes.COMMENTARY, tsql, certificate)
        res = executesql(tsql, certificate)

        execute_component_sql = res

    End Function

    Public Function executesp_for_logical_template(ByVal entity_id As Long, ByVal schema_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        '  Dim otrace As New bc_cs_activity_log("bc_db_services", "executesp_for_logical_template", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim rval As Object = Nothing
        Dim sqlcn As New SqlConnection
        Dim ocommentary As bc_cs_activity_log

        sqlcn.ConnectionString = Me.GetConnectionString(certificate)
        '    Dim ocommentary As bc_cs_activity_log
        Try
            REM initally do this for entity id and language
            REM but need to later extend to other dimensions or even multi dimensions
            ' ocommentary = New bc_cs_activity_log("bc_db_services", "open_connection", bc_cs_activity_codes.COMMENTARY, "Attempting to open Database for SQL Connection: " + CStr(sqlcn.ConnectionString))
            sqlcn.Open()
            '  ocommentary = New bc_cs_activity_log("bc_db_services", "open_connection", bc_cs_activity_codes.COMMENTARY, "Open Database Sucessful for SQL Connection: " + CStr(sqlcn.ConnectionString))
            '  ocommentary = New bc_cs_activity_log("bc_db_services", "executesp", bc_cs_activity_codes.COMMENTARY, "Attempting to execute stored procedure: insight_get_template_id for entity_id:" + CStr(entity_id))
            Dim cmd As New SqlCommand("insight_get_template_id", sqlcn)
            cmd.CommandTimeout = bc_cs_central_settings.ado_timeout
            cmd.CommandType = Data.CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@entity_id", entity_id)
            cmd.Parameters.AddWithValue("@schema_id", schema_id)
            With cmd.Parameters.Add("@template_id", SqlDbType.VarChar, 255)
                .Direction = ParameterDirection.Output
            End With
            cmd.ExecuteNonQuery()
            Dim tresults(0, 0) As Object
            tresults(0, 0) = cmd.Parameters("@template_id").Value.ToString
            executesp_for_logical_template = tresults
        Finally
            ocommentary = New bc_cs_activity_log("bc_db_services", "executesp_for_logical_template", bc_cs_activity_codes.COMMENTARY, "Stored Procedure returned value: " + CStr(rval), certificate)

            '   otrace = New bc_cs_activity_log("bc_db_services", "executesp_for_logical_template", bc_cs_activity_codes.TRACE_EXIT, "")
            If Not (sqlcn Is Nothing) Then
                sqlcn.Close()
            End If
        End Try
    End Function
    Public Sub executesp_for_workbook_submission(ByVal sp_name As String, ByVal entity_id As String, ByVal contributor_id As Integer, ByRef docbyte() As Byte, ByVal mode As Integer, ByRef certificate As bc_cs_security.certificate)
        ' Dim otrace As New bc_cs_activity_log("bc_db_services", "executesp_for_workbook_submission", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim sqlcn As New SqlConnection
        sqlcn.ConnectionString = Me.GetConnectionString(certificate)
        ' Dim ocommentary As bc_cs_activity_log
        Try
            REM initally do this for entity id and language
            REM but need to later extend to other dimensions or even multi dimensions
            '  ocommentary = New bc_cs_activity_log("bc_db_services", "open_connection", bc_cs_activity_codes.COMMENTARY, "Attempting to open Database for SQL Connection: " + CStr(sqlcn.ConnectionString))
            sqlcn.Open()
            ' ocommentary = New bc_cs_activity_log("bc_db_services", "open_connection", bc_cs_activity_codes.COMMENTARY, "Open Database Sucessful for SQL Connection: " + CStr(sqlcn.ConnectionString))
            ' ocommentary = New bc_cs_activity_log("bc_db_services", "executesp_for_workbook_submission", bc_cs_activity_codes.COMMENTARY, "Attempting to execute stored procedure: " + CStr(sp_name) + " for entity_id:" + CStr(entity_id))
            Dim cmd As New SqlCommand(sp_name, sqlcn)
            cmd.CommandTimeout = bc_cs_central_settings.ado_timeout
            cmd.CommandType = Data.CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@entity_id", entity_id)
            cmd.Parameters.AddWithValue("@contributor_id", contributor_id)
            cmd.Parameters.AddWithValue("@mode", mode)
            cmd.Parameters.AddWithValue("@docbyte", docbyte)
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_db_services", "executesp_for_workbook_submission", bc_cs_error_codes.DB_ERR, ex.Message)
        Finally
            ' otrace = New bc_cs_activity_log("bc_db_services", "executesp_for_workbook_submission", bc_cs_activity_codes.TRACE_EXIT, "")
            If Not (sqlcn Is Nothing) Then
                sqlcn.Close()
            End If
        End Try
    End Sub
    Public Sub executesp_for_document_submission(ByVal sp_name As String, ByVal filename As String, ByRef docbyte() As Byte, ByRef certificate As bc_cs_security.certificate)
        ' Dim otrace As New bc_cs_activity_log("bc_db_services", "executesp_for_workbook_submission", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim sqlcn As New SqlConnection
        sqlcn.ConnectionString = Me.GetConnectionString(certificate)
        ' Dim ocommentary As bc_cs_activity_log
        Try
            REM initally do this for entity id and language
            REM but need to later extend to other dimensions or even multi dimensions
            '  ocommentary = New bc_cs_activity_log("bc_db_services", "open_connection", bc_cs_activity_codes.COMMENTARY, "Attempting to open Database for SQL Connection: " + CStr(sqlcn.ConnectionString))
            sqlcn.Open()
            ' ocommentary = New bc_cs_activity_log("bc_db_services", "open_connection", bc_cs_activity_codes.COMMENTARY, "Open Database Sucessful for SQL Connection: " + CStr(sqlcn.ConnectionString))
            ' ocommentary = New bc_cs_activity_log("bc_db_services", "executesp_for_workbook_submission", bc_cs_activity_codes.COMMENTARY, "Attempting to execute stored procedure: " + CStr(sp_name) + " for entity_id:" + CStr(entity_id))
            Dim cmd As New SqlCommand(sp_name, sqlcn)
            cmd.CommandTimeout = bc_cs_central_settings.ado_timeout
            cmd.CommandType = Data.CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@filename", filename)
            cmd.Parameters.AddWithValue("@docbyte", docbyte)
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_db_services", "executesp_for_workbook_submission", bc_cs_error_codes.DB_ERR, ex.Message)
        Finally
            ' otrace = New bc_cs_activity_log("bc_db_services", "executesp_for_workbook_submission", bc_cs_activity_codes.TRACE_EXIT, "")
            If Not (sqlcn Is Nothing) Then
                sqlcn.Close()
            End If
        End Try
    End Sub
    Public Function executesql_no_log(ByVal strsql As String) As Object
        Dim sqlcn As New SqlConnection

        Dim i, j As Integer
        Dim row_Count As Integer
        Try
            sqlcn.ConnectionString = Me.GetConnectionString(Nothing)
            sqlcn.Open()
            Dim cmd As New SqlCommand(strsql, sqlcn)
            cmd.CommandTimeout = bc_cs_central_settings.ado_timeout
            Dim dr As SqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            row_Count = 0
            Dim oresults As New List(Of Object)
            If dr.HasRows = True Then
                Do While dr.Read
                    For i = 0 To dr.FieldCount - 1

                        oresults.Add(dr.GetValue(i))
                    Next
                    row_Count = row_Count + 1
                Loop
            End If
            Dim tresults(dr.FieldCount, row_Count - 1) As Object
            For i = 0 To row_Count - 1
                For j = 0 To dr.FieldCount - 1
                    tresults(j, i) = oresults(i + (dr.FieldCount - 1) * i + j)
                Next
            Next
            dr.Close()
            executesql_no_log = tresults
        Catch ex As Exception
            Dim tresults(0, 0) As Object
            tresults(0, 0) = "Error"
            executesql_no_log = tresults
        Finally
            If Not (sqlcn Is Nothing) Then
                sqlcn.Close()
            End If
        End Try
    End Function
    Public Function executesp_for_generate_unique_key(ByRef certificate As bc_cs_security.certificate) As Object

        Dim sqlcn As New SqlConnection
        Dim ocommentary As bc_cs_activity_log
        Dim returnValue As Int64

        sqlcn.ConnectionString = Me.GetConnectionString(certificate)

        Try
            sqlcn.Open()
            Dim cmd As New SqlCommand("generate_unique_key", sqlcn)
            cmd.CommandTimeout = bc_cs_central_settings.ado_timeout
            cmd.CommandType = Data.CommandType.StoredProcedure
            With cmd.Parameters.Add("@key_id", SqlDbType.BigInt)
                .Direction = ParameterDirection.Output
            End With
            cmd.ExecuteNonQuery()
            Dim tresults(0, 0) As Object
            tresults(0, 0) = cmd.Parameters("@key_id").Value
            returnValue = tresults(0, 0)
            executesp_for_generate_unique_key = tresults

        Finally
            ocommentary = New bc_cs_activity_log("bc_db_services", "executesp_for_logical_template", bc_cs_activity_codes.COMMENTARY, "Stored Procedure returned value: " + Str(returnValue), certificate)
            If Not (sqlcn Is Nothing) Then
                sqlcn.Close()
            End If
        End Try
    End Function
    Public Sub async_query_finished(ByVal result As IAsyncResult)
        Try
            Dim command As SqlCommand = CType(result.AsyncState, SqlCommand)
            Dim rowCount As Integer = command.EndExecuteNonQuery(result)
            command.Connection.Close()
        Catch ex As Exception
            Dim omsg As New bc_cs_error_log("bc_cs_db_services", "async_query_finished", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub


    Public Sub executesql_async(ByVal strsql As String, ByRef certificate As bc_cs_security.certificate)
        Dim otrace As New bc_cs_activity_log("bc_db_services", "executesql_async", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        ' Dim ocommentary As bc_cs_activity_log

        Dim sqlcn As New SqlConnection
        sqlcn.ConnectionString = Me.GetConnectionString(certificate)


        Dim dr As SqlDataReader = Nothing

        Try

            sqlcn.ConnectionString = sqlcn.ConnectionString + ";Asynchronous Processing=true"
            sqlcn.Open()

            Dim cmd As New SqlCommand(strsql, sqlcn)
            cmd.CommandTimeout = bc_cs_central_settings.extended_timeout
            Dim odbtrace = New bc_cs_activity_log("bc_db_services", "executesql_async", bc_cs_activity_codes.SQL, strsql, certificate)

            Dim callback As New AsyncCallback(AddressOf async_query_finished)

            cmd.BeginExecuteNonQuery(callback, cmd)


        Catch ex As Exception
            If bc_cs_central_settings.server_flag = 1 Then
                Dim db_err As New bc_cs_error_log("bc_db_services", "executesql_async", bc_cs_error_codes.DB_ERR, ex.Message + "::" + strsql, certificate)
            Else
                Dim db_err As New bc_cs_error_log("bc_db_services", "executesql_async", bc_cs_error_codes.DB_ERR, ex.Message, certificate)
            End If

        Finally
            otrace = New bc_cs_activity_log("bc_db_services", "executesql_async", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
            'If Not (dr Is Nothing) Then
            '    dr.Close()
            'End If
            'If Not (sqlcn Is Nothing) Then
            '    sqlcn.Close()
            'End If
        End Try
    End Sub
    REM this doesnt appear to work it stops very quickly
    'Public Sub executesql_async_no_callback(ByVal strsql As String, ByRef certificate As bc_cs_security.certificate)
    '    Dim otrace As New bc_cs_activity_log("bc_db_services", "executesql_async_no_callback", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

    '    Dim sqlcn As New SqlConnection
    '    sqlcn.ConnectionString = Me.GetConnectionString(certificate)


    '    Dim dr As SqlDataReader = Nothing

    '    Try
    '        sqlcn.ConnectionString = sqlcn.ConnectionString + ";Asynchronous Processing=true"
    '        sqlcn.Open()

    '        Dim cmd As New SqlCommand(strsql, sqlcn)
    '        REM cmd.CommandTimeout = bc_cs_central_settings.extended_timeout
    '        cmd.CommandTimeout = 10000000
    '        Dim odbtrace = New bc_cs_activity_log("bc_db_services", "executesql_async", bc_cs_activity_codes.SQL, strsql, certificate)

    '        cmd.BeginExecuteNonQuery(Nothing, cmd)

    '    Catch ex As Exception
    '        If bc_cs_central_settings.server_flag = 1 Then
    '            Dim db_err As New bc_cs_error_log("bc_db_services", "executesql_async_no_callback", bc_cs_error_codes.DB_ERR, ex.Message + "::" + strsql, certificate)
    '        Else
    '            Dim db_err As New bc_cs_error_log("bc_db_services", "executesql_async_no_callback", bc_cs_error_codes.DB_ERR, ex.Message, certificate)
    '        End If

    '    Finally
    '        otrace = New bc_cs_activity_log("bc_db_services", "executesql_async_no_callback", bc_cs_activity_codes.TRACE_EXIT, "", certificate)

    '    End Try
    'End Sub


    Public Class bc_cs_sql_parameter
        Public name As String
        Public value As Object
    End Class
    Public Function executesql_with_parameters_no_timeout_cp_dl(ByVal sp_name As String, parameters As List(Of bc_cs_sql_parameter), ByRef certificate As bc_cs_security.certificate) As Object
        Dim otrace As New bc_cs_activity_log("bc_db_services", "executesql_with_parameters_no_timeout_cp_dl", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        ' Dim ocommentary As bc_cs_activity_log

        Dim i, j As Integer
        Dim row_Count As Integer
        Dim sqlcn As New SqlConnection
        sqlcn.ConnectionString = Me.GetConnectionString(certificate)

        Dim dr As SqlDataReader = Nothing

        Try
            sqlcn.Open()
            Dim odbtrace = New bc_cs_activity_log("bc_db_services", "executesql_with_parameters_no_timeout_cp_dl", bc_cs_activity_codes.SQL, sp_name + "with parameters no_timeout", certificate)

            Dim cmd As New SqlCommand(sp_name, sqlcn)
            cmd.CommandTimeout = 3600

            cmd.CommandType = Data.CommandType.StoredProcedure
            Dim ocomm As New bc_cs_activity_log("bc_db_services", "executesql_with_parameters_no_timeout_cp_dl", bc_cs_activity_codes.COMMENTARY, "start params no_timeout", certificate)

            For i = 0 To parameters.Count - 1
                ocomm = New bc_cs_activity_log("bc_db_services", "executesql_with_parameters_no_timeout_cp_dl", bc_cs_activity_codes.COMMENTARY, parameters(i).name + CStr(parameters(i).value), certificate)
                cmd.Parameters.AddWithValue(parameters(i).name, parameters(i).value)
            Next

            dr = cmd.ExecuteReader(CommandBehavior.CloseConnection)

            row_Count = 0

            Dim oresults As New List(Of Object)
            If dr.HasRows = True Then
                Do While dr.Read
                    For i = 0 To dr.FieldCount - 1

                        oresults.Add(dr.GetValue(i))
                    Next
                    row_Count = row_Count + 1
                Loop
            End If
            Dim tresults(dr.FieldCount, row_Count - 1) As Object
            For i = 0 To row_Count - 1
                For j = 0 To dr.FieldCount - 1
                    tresults(j, i) = oresults(i + (dr.FieldCount - 1) * i + j)
                Next
            Next
            dr.Close()
            executesql_with_parameters_no_timeout_cp_dl = tresults
        Catch ex As Exception
            success = False
            If InStr(ex.Message, "deadlock") > 1 Or InStr(ex.Message, "timeout") > 1 Then
                rp_dl = True
                executesql_with_parameters_no_timeout_cp_dl = Nothing
                otrace = New bc_cs_activity_log("bc_db_services", "executesql_with_parameters_no_timeout_cp_dl", bc_cs_activity_codes.TRACE_EXIT, "pool or deadlock:" + ex.Message, certificate)
                Exit Function
            End If


            If bc_cs_central_settings.server_flag = 1 Then
                Dim db_err As New bc_cs_error_log("bc_db_services", "executesql_with_parameters_no_timeout_cp_dl", bc_cs_error_codes.DB_ERR, ex.Message + "::" + sp_name, certificate)
            Else
                Dim db_err As New bc_cs_error_log("bc_db_services", "executesql_with_parameters_no_timeout_cp_dl", bc_cs_error_codes.DB_ERR, ex.Message, certificate)
            End If
            executesql_with_parameters_no_timeout_cp_dl = Nothing
        Finally
            otrace = New bc_cs_activity_log("bc_db_services", "executesql_with_parameters_no_timeout_cp_dl", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
            If Not (dr Is Nothing) Then
                dr.Close()
            End If
            If Not (sqlcn Is Nothing) Then
                sqlcn.Close()
            End If

        End Try
    End Function


    Public Function executesql_with_parameters_no_timeout(ByVal sp_name As String, parameters As List(Of bc_cs_sql_parameter), ByRef certificate As bc_cs_security.certificate) As Object
        Dim otrace As New bc_cs_activity_log("bc_db_services", "executesql", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        ' Dim ocommentary As bc_cs_activity_log

        Dim i, j As Integer
        Dim row_Count As Integer
        Dim sqlcn As New SqlConnection
        sqlcn.ConnectionString = Me.GetConnectionString(certificate)

        Dim dr As SqlDataReader = Nothing

        Try
            sqlcn.Open()
            Dim odbtrace = New bc_cs_activity_log("bc_db_services", "executesql", bc_cs_activity_codes.SQL, sp_name + "with parameters no_timeout", certificate)

            Dim cmd As New SqlCommand(sp_name, sqlcn)
            cmd.CommandTimeout = 3600

            cmd.CommandType = Data.CommandType.StoredProcedure
            Dim ocomm As New bc_cs_activity_log("bc_db_services", "executesql", bc_cs_activity_codes.COMMENTARY, "start params no_timeout", certificate)

            For i = 0 To parameters.Count - 1
                ocomm = New bc_cs_activity_log("bc_db_services", "executesql", bc_cs_activity_codes.COMMENTARY, parameters(i).name + CStr(parameters(i).value), certificate)
                cmd.Parameters.AddWithValue(parameters(i).name, parameters(i).value)
            Next

            dr = cmd.ExecuteReader(CommandBehavior.CloseConnection)

            row_Count = 0

            Dim oresults As New List(Of Object)
            If dr.HasRows = True Then
                Do While dr.Read
                    For i = 0 To dr.FieldCount - 1

                        oresults.Add(dr.GetValue(i))
                    Next
                    row_Count = row_Count + 1
                Loop
            End If
            Dim tresults(dr.FieldCount, row_Count - 1) As Object
            For i = 0 To row_Count - 1
                For j = 0 To dr.FieldCount - 1
                    tresults(j, i) = oresults(i + (dr.FieldCount - 1) * i + j)
                Next
            Next
            dr.Close()
            executesql_with_parameters_no_timeout = tresults
        Catch ex As Exception
            If bc_cs_central_settings.server_flag = 1 Then
                Dim db_err As New bc_cs_error_log("bc_db_services", "executesql", bc_cs_error_codes.DB_ERR, ex.Message + "::" + sp_name, certificate)
            Else
                Dim db_err As New bc_cs_error_log("bc_db_services", "executesql", bc_cs_error_codes.DB_ERR, ex.Message, certificate)
            End If
            executesql_with_parameters_no_timeout = Nothing
        Finally
            otrace = New bc_cs_activity_log("bc_db_services", "executesql with parameters no_timeout", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
            If Not (dr Is Nothing) Then
                dr.Close()
            End If
            If Not (sqlcn Is Nothing) Then
                sqlcn.Close()
            End If

        End Try
    End Function
    Public Function executesql_with_parameters(ByVal sp_name As String, parameters As List(Of bc_cs_sql_parameter), ByRef certificate As bc_cs_security.certificate) As Object
        Dim otrace As New bc_cs_activity_log("bc_db_services", "executesql", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        ' Dim ocommentary As bc_cs_activity_log

        Dim i, j As Integer
        Dim row_Count As Integer
        Dim sqlcn As New SqlConnection
        sqlcn.ConnectionString = Me.GetConnectionString(certificate)

        Dim dr As SqlDataReader = Nothing

        Try
            sqlcn.Open()
            Dim odbtrace = New bc_cs_activity_log("bc_db_services", "executesql", bc_cs_activity_codes.SQL, sp_name + "with parameters", certificate)

            Dim cmd As New SqlCommand(sp_name, sqlcn)
            cmd.CommandTimeout = bc_cs_central_settings.ado_timeout

            cmd.CommandType = Data.CommandType.StoredProcedure
            Dim ocomm As New bc_cs_activity_log("bc_db_services", "executesql", bc_cs_activity_codes.COMMENTARY, "start params", certificate)

            For i = 0 To parameters.Count - 1
                ocomm = New bc_cs_activity_log("bc_db_services", "executesql", bc_cs_activity_codes.COMMENTARY, parameters(i).name + CStr(parameters(i).value), certificate)
                cmd.Parameters.AddWithValue(parameters(i).name, parameters(i).value)
            Next

            dr = cmd.ExecuteReader(CommandBehavior.CloseConnection)

            row_Count = 0

            Dim oresults As New List(Of Object)
            If dr.HasRows = True Then
                Do While dr.Read
                    For i = 0 To dr.FieldCount - 1

                        oresults.Add(dr.GetValue(i))
                    Next
                    row_Count = row_Count + 1
                Loop
            End If
            Dim tresults(dr.FieldCount, row_Count - 1) As Object
            For i = 0 To row_Count - 1
                For j = 0 To dr.FieldCount - 1
                    tresults(j, i) = oresults(i + (dr.FieldCount - 1) * i + j)
                Next
            Next
            dr.Close()
            executesql_with_parameters = tresults
        Catch ex As Exception
            If bc_cs_central_settings.server_flag = 1 Then
                Dim db_err As New bc_cs_error_log("bc_db_services", "executesql", bc_cs_error_codes.DB_ERR, ex.Message + "::" + sp_name, certificate)
            Else
                Dim db_err As New bc_cs_error_log("bc_db_services", "executesql", bc_cs_error_codes.DB_ERR, ex.Message, certificate)
            End If
            executesql_with_parameters = Nothing
        Finally
            otrace = New bc_cs_activity_log("bc_db_services", "executesql", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
            If Not (dr Is Nothing) Then
                dr.Close()
            End If
            If Not (sqlcn Is Nothing) Then
                sqlcn.Close()
            End If

        End Try
    End Function
End Class
