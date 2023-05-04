Imports BlueCurve.Core.CS
Imports System.io
REM ===============================================
REM Bluecurve Limited 2007
REM Module:       SQL
REM Type:         Object Model
REM Description:  used to transmit and execute SQL 
REM               via web service
REM Version:      1
REM Change history
REM ===============================================
REM Object for ad-hoc stored procedure execution
<Serializable()> Public Class bc_om_rest_ef
    Public ef As String
    Public user_id As Long
    Public index As Integer = 0
End Class

<Serializable()> Public Class bc_om_sql
    Inherits bc_cs_soap_base_class
    Dim sql As String
    Public success As Boolean
    Public results As Object
    Private boolIsXml As Boolean

    Public Property IsXml() As Boolean
        Get
            Return boolIsXml
        End Get
        Set(ByVal boolIsXml As Boolean)
            Me.boolIsXml = boolIsXml
        End Set
    End Property

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
    'Public Overloads Overrides Function call_web_service() As String
    '    Dim otrace As New bc_cs_activity_log("bc_om_sql", "call_web_service", bc_cs_activity_codes.TRACE_ENTRY, "", MyBase.certificate)
    '    If IsNumeric(bc_cs_central_settings.timeout) Then
    '        webservice.Timeout = CLng(bc_cs_central_settings.timeout)
    '        Dim ocommentary As New bc_cs_activity_log("bc_om_web services", "call_web_service", bc_cs_activity_codes.COMMENTARY, "Setting Web Service Timeout to: " + bc_cs_central_settings.timeout, MyBase.certificate)
    '    End If

    '    call_web_service = webservice.Executesql(MyBase.write_xml_to_string)
    '    otrace = New bc_cs_activity_log("bc_om_sql", "call_web_service", bc_cs_activity_codes.TRACE_EXIT, "", MyBase.certificate)
    'End Function
    Public Sub New(ByVal sql As String)
        Me.sql = sql
    End Sub
    Public Sub New()

    End Sub
    'allred
    Public Sub db_read()
        Dim otrace As New bc_cs_activity_log("bc_om_sql", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "", MyBase.certificate)
        Try
            success = True
            Dim bc_om_sql_db As New bc_om_sql_db
            Dim ocommentary As New bc_cs_activity_log("bc_om_sql", "db_read", bc_cs_activity_codes.COMMENTARY, "SQL: " + Me.sql, MyBase.certificate)
            'allred
            If boolIsXml Then
                results = bc_om_sql_db.execute_for_xml(sql)
            Else
                results = bc_om_sql_db.execute(sql)
            End If
            If IsArray(results) Then
                If UBound(results, 2) = 0 Then
                    If CStr(results(0, 0)) = "Error" Then
                        results(0, 0) = "DB Err"
                        success = False
                        ocommentary = New bc_cs_activity_log("bc_om_sql", "db_read", bc_cs_activity_codes.COMMENTARY, "SQL error: " + Me.sql + "; " + CStr(results(1, 0)), MyBase.certificate)
                    End If
                End If
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_sql", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, MyBase.certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_sql", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", MyBase.certificate)
        End Try
    End Sub

    'Public Overloads Overrides Sub db_read(ByVal id As Long, ByRef certificate As cs.bc_cs_security.certificate)

    'End Sub
End Class
Public Class bc_om_sql_db
    Private gbc_db As New bc_cs_db_services(False)
    Public Function execute(ByVal sql As String) As Object
        execute = gbc_db.executesql_show_no_error(sql)
    End Function
    'allred
    Public Function execute_for_xml(ByVal sql As String) As Object
        execute_for_xml = gbc_db.executexmlsql_show_no_error(sql)
    End Function
End Class
<Serializable()> Public Class bc_om_excel_functions
    Inherits bc_cs_soap_base_class

    <NonSerialized()> Public function_call As String
    <NonSerialized()> Public result As String
    Public batch_efs As New ArrayList
    Public batch_results As New ArrayList
    Public batch_results_list As New ArrayList
    Public number_per_batch As Integer
    Public batch_count As Integer
    Public token As String = ""
    Public load_efs_flag As Boolean = False
    Public get_results As Boolean = False
    Public db_batch_mode As Integer = 1

    Public Sub New()
        Me.batch_count = 0
        Me.number_per_batch = 0
    End Sub
    Public Function execute_function(ByVal function_call As String) As Boolean
        Try

            'Me.function_call = function_call
            'execute_function = web_service_excel_function()

            Dim osql As New bc_om_sql("exec dbo.bcc_core_v5_excel_function '" + function_call + "','" + CStr(bc_cs_central_settings.logged_on_user_id) + "'")
            osql.tmode = bc_cs_soap_base_class.tREAD

            If osql.transmit_to_server_and_receive(osql, True) = True Then
                Try
                    result = osql.results(0, 0)
                Catch
                    result = ""
                End Try
                execute_function = True
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_excel_functions", "execute_function", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)

        End Try

    End Function
    Public Overrides Sub process_object()
        Try
            Select Case tmode
                Case tREAD
                    If db_batch_mode = 0 Then
                        db_read()
                    Else
                        db_read_batch()
                    End If

            End Select
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_excel_functions", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)

        End Try

    End Sub
    Public Sub db_read()
        Dim otrace As New bc_cs_activity_log("bc_om_excel_functions", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try
            Dim sql As String
            Dim gbc As New bc_cs_db_services
            Dim ores As Object
            Dim start_ef As Integer = 0
            Dim end_ef As Integer = 0
            Dim exec_count As Integer

            exec_count = 0

            If number_per_batch = 0 Then
                Dim ocomm As New bc_cs_activity_log("bc_om_excel_functions", "db_read", bc_cs_activity_codes.COMMENTARY, "Executing " + CStr(batch_efs.Count) + ": functions in 1 batch", certificate)
                start_ef = 0
                end_ef = Me.batch_efs.Count - 1
            Else
                start_ef = (batch_count - 1) * number_per_batch
                end_ef = start_ef + number_per_batch - 1
                If end_ef > batch_efs.Count Then
                    end_ef = batch_efs.Count - 1
                End If
                Dim ocomm As New bc_cs_activity_log("bc_om_excel_functions", "db_read", bc_cs_activity_codes.COMMENTARY, "Executing functions " + CStr(start_ef + 1) + " to " + CStr(end_ef + 1) + " total: " + CStr(batch_efs.Count), certificate)
            End If
            Dim st As bc_cs_string_services

            For i = start_ef To end_ef
                REM this needs to go into FIL
                st = New bc_cs_string_services(batch_efs(i))
                batch_efs(i) = st.delimit_apostrophies()
                REM -------------------------
                sql = "exec dbo.bcc_core_v5_excel_function '" + batch_efs(i) + "','" + certificate.user_id + "'"
                ores = gbc.executesql(sql, certificate)
                If IsArray(ores) Then
                    Try
                        Me.batch_results(i) = CStr(ores(0, 0))
                        Me.batch_results_list(i) = CStr(ores(0, 0))
                        If InStr(CStr(ores(0, 0)), ";") > 0 Then
                            Me.batch_results_list(i) = CStr(ores(0, 0)).Substring(0, InStr(CStr(ores(0, 0)), ";") - 1)
                        End If

                    Catch
                        Me.batch_results(i) = ""
                        Me.batch_results_list(i) = ""
                    End Try
                End If

                For j = i + 1 To batch_efs.Count - 1
                    If InStr(Me.batch_efs(j), "eval[" + CStr(i) + "]") > 0 Then
                        Me.batch_efs(j) = Replace(Me.batch_efs(j), "eval[" + CStr(i) + "]", Me.batch_results_list(i))
                    End If
                Next
                Me.batch_efs(i) = ""

            Next

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_excel_functions", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_om_excel_functions", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", certificate)


        End Try
    End Sub
    REM FIL 55
    REM run the SQL batches all on the database
    Public Sub db_read_batch()
        Dim otrace As New bc_cs_activity_log("bc_om_excel_functions", "db_read55", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try
            Dim sql As String
            Dim gbc As New bc_cs_db_services
            Dim ores As Object
            Dim start_ef As Integer = 0
            Dim end_ef As Integer = 0
            'Dim exec_count As Integer
            Dim st As bc_cs_string_services

            Dim all_efs As String

            If number_per_batch = 0 Then
                Dim ocomm As New bc_cs_activity_log("bc_om_excel_functions", "db_read", bc_cs_activity_codes.COMMENTARY, "Executing " + CStr(batch_efs.Count) + ": functions in 1 batch", certificate)
                start_ef = 0
                end_ef = Me.batch_efs.Count - 1
            Else
                start_ef = (batch_count - 1) * number_per_batch
                end_ef = start_ef + number_per_batch - 1
                If end_ef > batch_efs.Count Then
                    end_ef = batch_efs.Count - 1
                End If
                'Dim ocomm As New bc_cs_activity_log("bc_om_excel_functions", "db_read", bc_cs_activity_codes.COMMENTARY, "Executing functions " + CStr(start_ef + 1) + " to " + CStr(end_ef + 1) + " total: " + CStr(batch_efs.Count), certificate)
            End If
            Dim user_id As String
            If bc_cs_central_settings.server_flag = 0 Then
                user_id = bc_cs_central_settings.logged_on_user_id
            Else
                user_id = certificate.user_id
            End If

            If load_efs_flag = True Then
                If start_ef = 0 Then
                    token = user_id + Format(Now, "ddMMyyyyhhmmss")
                End If

                all_efs = ""
                For i = start_ef To end_ef
                    all_efs = all_efs + batch_efs(i)
                Next
                st = New bc_cs_string_services(all_efs)
                all_efs = st.delimit_apostrophies()
                sql = "exec dbo.bc_core_load_batch_efs " + CStr(start_ef) + "," + CStr(end_ef) + ",'" + all_efs + "','" + token + "','" + CStr(user_id) + "'"
                ores = gbc.executesql(sql, certificate)

                If UBound(ores, 2) > -1 Then
                    If ores(0, 0) = "Error" Then
                        Dim oerr As New bc_cs_error_log("bc_om_excel_function", "db_read_55", bc_cs_error_codes.USER_DEFINED, "Error executing bc_core_load_batch_efs", certificate)
                        Dim occmm As New bc_cs_activity_log("bc_om_excel_function", "db_read_55", bc_cs_activity_codes.COMMENTARY, "Error executing bc_core_load_batch_efs: " + sql, certificate)
                    End If
                End If

            ElseIf get_results = True Then

                REM === now get results from database
                sql = "exec dbo.bc_core_retrieve_batch_efs_results " + CStr(start_ef) + "," + CStr(end_ef) + ",'" + token + "','" + user_id + "'"
                ores = gbc.executesql(sql, certificate)

                If IsArray(ores) Then
                    For i = 0 To UBound(ores, 2)
                        Try
                            Me.batch_results(start_ef + i) = CStr(ores(0, i))

                        Catch
                            Me.batch_results(start_ef + i) = ""
                            Me.batch_results_list(start_ef + i) = ""
                        End Try
                    Next
                End If
                REM  ======================== 
            Else

                REM --------- excute the functions of the batch

                sql = "exec dbo.bc_core_execute_batch_efs " + CStr(start_ef) + "," + CStr(end_ef) + ",'" + token + "','" + CStr(user_id) + "'"
                gbc.executesql(sql, certificate)
                REM ----------

            End If



        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_excel_functions", "db_read55", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_om_excel_functions", "db_read55", bc_cs_activity_codes.TRACE_EXIT, "", certificate)


        End Try
    End Sub
    REM PR April 2015 put in wcg version of this as well
    'Private Function web_service_excel_function() As Boolean
    '    If Right(bc_cs_central_settings.soap_server, 3) = "svc" Then
    '        If IsNothing(bc_cs_soap_base_class.wcfclient) Then
    '            bc_cs_soap_base_class.load_wcf_proxy()
    '        End If
    '        Try
    '            result = bc_cs_soap_base_class.wcfclient.excel_function_transmission_wcf(Me.function_call, CStr(bc_cs_central_settings.logged_on_user_id), CStr(bc_cs_central_settings.ad_excel_token))
    '            web_service_excel_function = True
    '        Catch ex As Exception
    '            Dim ocommentary As New bc_cs_activity_log("bc_om_excel_functions", "web_service_excel_function_wcf", bc_cs_activity_codes.COMMENTARY, "Network Error: " + ex.Message)
    '        End Try

    '    Else
    '        Dim webservice As New BlueCurveWebService.BlueCurveWS
    '        webservice.Url = bc_cs_central_settings.soap_server

    '        web_service_excel_function = False
    '        REM call object model specific web service
    '        If IsNumeric(bc_cs_central_settings.timeout) Then
    '            webservice.Timeout = CLng(bc_cs_central_settings.timeout)
    '        End If
    '        Try
    '            result = webservice.excel_function_transmission(Me.function_call, CStr(bc_cs_central_settings.logged_on_user_id), CStr(bc_cs_central_settings.ad_excel_token))
    '            web_service_excel_function = True
    '        Catch ex As Exception
    '            Dim ocommentary As New bc_cs_activity_log("bc_om_excel_functions", "web_service_excel_function", bc_cs_activity_codes.COMMENTARY, "Network Error: " + ex.Message)
    '        End Try
    '    End If

    'End Function
    'Public Function process_web_request(ByVal s As String, ByVal logged_on_user_id As String, ByVal ad_token As String) As String
    '    Dim sql As String = ""
    '    Dim ores As Object
    '    Dim certificate As New bc_cs_security.certificate
    '    certificate.user_id = logged_on_user_id

    '    process_web_request = ""

    '    Try

    '        If bc_cs_central_settings.show_authentication_form = 2 Then

    '            REM if active dirctory firstly authticate user
    '            Dim gdb As New bc_cs_db_services
    '            Dim res As Object
    '            Dim dtoken As String
    '            Dim ad_passed As Boolean = False
    '            Try
    '                res = gdb.executesql("select token from bc_core_ef_active_directory where user_id='" + CStr(logged_on_user_id) + "'", certificate)
    '                If IsArray(res) Then
    '                    If UBound(res, 2) = 0 Then
    '                        dtoken = res(0, 0)
    '                        If ad_token = dtoken And dtoken <> "" And ad_token <> "" Then
    '                            ad_passed = True
    '                        Else
    '                            Dim ocomm As New bc_cs_activity_log("bc_om_excel_functions", "process_web_request", bc_cs_activity_codes.COMMENTARY, "AD ef token match failed for user" + CStr(logged_on_user_id), certificate)
    '                            process_web_request = "AD failed authentication"
    '                            Exit Function
    '                        End If
    '                    End If
    '                End If
    '            Catch ex As Exception
    '                Dim ocomm As New bc_cs_activity_log("bc_om_excel_functions", "process_web_request", bc_cs_activity_codes.COMMENTARY, "AD ef token failed:" + ex.Message, certificate)
    '                process_web_request = "AD failed authentication"
    '                Exit Function
    '            End Try
    '            If ad_passed = False Then
    '                process_web_request = "AD failed authentication"
    '                Exit Function
    '            End If
    '        End If

    '        'sql = "exec dbo.bcc_core_excel_function '" + s + "','" + logged_on_user_id + "'"
    '        sql = "exec dbo.bcc_core_v5_excel_function '" + s + "','" + logged_on_user_id + "'"
    '        Dim gbc_db As New bc_cs_db_services
    '        ores = gbc_db.executesql_show_no_error(sql)
    '        If IsArray(ores) Then
    '            process_web_request = ores(0, 0)
    '        End If
    '        If process_web_request = "Error" Then
    '            process_web_request = "DB Error: " + sql
    '            certificate.user_id = logged_on_user_id
    '            Dim oerr As New bc_cs_error_log("bc_om_excel_functions", "process_web_request", bc_cs_error_codes.DB_ERR, "Error executing sql: " + sql, certificate)
    '        End If
    '    Catch ex As Exception
    '        Dim oerr As New bc_cs_error_log("bc_om_excel_functions", "process_web_request", bc_cs_error_codes.DB_ERR, "Error executing sql: " + sql + ": " + ex.Message, certificate)
    '        process_web_request = "DB Error (null):  " + sql + ": " + ex.Message
    '    End Try
    'End Function

    REM Standard Banl sept 2015 SQLi fix
    'Public Function process_web_request(ByVal s As String, ByVal logged_on_user_id As String, ByVal ad_token As String) As String
    '    Dim sql As String = ""
    '    Dim ores As Object
    '    Dim certificate As New bc_cs_security.certificate
    '    certificate.user_id = logged_on_user_id

    '    process_web_request = ""

    '    Try
    '        Dim gbc_db As New bc_cs_db_services
    '        Dim sp_params As New List(Of bc_cs_db_services.bc_cs_sql_parameter)
    '        Dim sp_param As bc_cs_db_services.bc_cs_sql_parameter

    '        If bc_cs_central_settings.show_authentication_form = 2 Then
    '            Dim ocomm As New bc_cs_activity_log("bc_om_excel_functions", "process_web_request", bc_cs_activity_codes.COMMENTARY, "Active Directory", certificate)

    '            REM if active dirctory firstly authticate user
    '            Dim gdb As New bc_cs_db_services
    '            Dim res As Object
    '            Dim dtoken As String
    '            Dim ad_passed As Boolean = False
    '            Try
    '                sp_params = New List(Of bc_cs_db_services.bc_cs_sql_parameter)
    '                sp_param = New bc_cs_db_services.bc_cs_sql_parameter
    '                sp_param.name = "@user_id"
    '                sp_param.value = CLng(logged_on_user_id)
    '                sp_params.Add(sp_param)

    '                res = gbc_db.executesql_with_parameters("bc_core_get_ad_excel_token", sp_params, certificate)

    '                'res = gdb.executesql("select token from bc_core_ef_active_directory where user_id='" + CStr(logged_on_user_id) + "'", certificate)
    '                If IsArray(res) Then
    '                    If UBound(res, 2) = 0 Then
    '                        dtoken = res(0, 0)
    '                        If ad_token = dtoken And dtoken <> "" And ad_token <> "" Then
    '                            ad_passed = True
    '                        Else
    '                            ocomm = New bc_cs_activity_log("bc_om_excel_functions", "process_web_request", bc_cs_activity_codes.COMMENTARY, "AD ef token match failed for user" + CStr(logged_on_user_id), certificate)
    '                            process_web_request = "AD failed authentication"
    '                            Exit Function
    '                        End If
    '                    End If
    '                End If
    '            Catch ex As Exception
    '                ocomm = New bc_cs_activity_log("bc_om_excel_functions", "process_web_request", bc_cs_activity_codes.COMMENTARY, "AD ef token failed:" + ex.Message, certificate)
    '                process_web_request = "AD failed authentication"
    '                Exit Function
    '            End Try
    '            If ad_passed = False Then
    '                process_web_request = "AD failed authentication"
    '                Exit Function
    '            End If
    '        End If

    '        sp_params = New List(Of bc_cs_db_services.bc_cs_sql_parameter)
    '        sp_param = New bc_cs_db_services.bc_cs_sql_parameter
    '        sp_param.name = "@xml_text"
    '        sp_param.value = s
    '        sp_params.Add(sp_param)
    '        sp_param = New bc_cs_db_services.bc_cs_sql_parameter
    '        sp_param.name = "@user_id"
    '        sp_param.value = logged_on_user_id
    '        sp_params.Add(sp_param)

    '        ores = gbc_db.executesql_with_parameters("bcc_core_v5_excel_function", sp_params, certificate)

    '        If IsArray(ores) Then
    '            process_web_request = ores(0, 0)
    '        End If
    '        If process_web_request = "Error" Then
    '            process_web_request = "DB Error: " + sql
    '            certificate.user_id = logged_on_user_id
    '            Dim oerr As New bc_cs_error_log("bc_om_excel_functions", "process_web_request", bc_cs_error_codes.DB_ERR, "Error executing sql: " + sql, certificate)
    '        End If
    '    Catch ex As Exception
    '        Dim oerr As New bc_cs_error_log("bc_om_excel_functions", "process_web_request", bc_cs_error_codes.DB_ERR, "Error executing sql: " + sql + ": " + ex.Message, certificate)
    '        process_web_request = "DB Error (null):  " + sql + ": " + ex.Message
    '    End Try
    'End Function

End Class




