'Public Class bc_cs_cs_authentication


'    Public Function authenticate(ByRef certificate As bc_cs_security.certificate)
'        Try


'            authenticate = False
'            Dim db As New bc_cs_db_services
'            Dim sqlparam As New bc_cs_db_services.bc_cs_sql_parameter
'            Dim sqlparams As New List(Of bc_cs_db_services.bc_cs_sql_parameter)
'            Dim err_tx As String
'            err_tx = check_authentication_methods(certificate)
'            If err_tx <> "" Then
'                certificate.error_state = True
'                certificate.server_errors.Add("Authentication Failed: " + err_tx)
'                Return False
'            End If

'            sqlparam = New bc_cs_db_services.bc_cs_sql_parameter
'            sqlparam.name = "mode"
'            sqlparam.value = certificate.authentication_mode
'            sqlparams.Add(sqlparam)
'            sqlparam = New bc_cs_db_services.bc_cs_sql_parameter
'            sqlparam.name = "os_user_name"
'            sqlparam.value = certificate.os_name
'            sqlparams.Add(sqlparam)
'            sqlparam = New bc_cs_db_services.bc_cs_sql_parameter
'            sqlparam.name = "user_name"
'            sqlparam.value = certificate.name
'            sqlparams.Add(sqlparam)
'            sqlparam = New bc_cs_db_services.bc_cs_sql_parameter
'            sqlparam.name = "password"
'            sqlparam.value = certificate.password
'            sqlparams.Add(sqlparam)
'            sqlparam = New bc_cs_db_services.bc_cs_sql_parameter
'            sqlparam.name = "token"
'            sqlparam.value = certificate.authentication_token
'            sqlparams.Add(sqlparam)

'            Dim res As Object
'            Dim user_id As Long
'            res = db.executesql_with_parameters("dbo.bc_core_cs_authenticate", sqlparams, certificate)
'            If IsArray(res) Then
'                If UBound(res, 2) = 0 Then
'                    If IsNumeric(res(0, 0)) Then
'                        user_id = res(0, 0)
'                        If user_id > 0 Then
'                            certificate.user_id = user_id
'                            Return True
'                        End If
'                    Else
'                        certificate.error_state = True
'                        certificate.server_errors.Add("Authentication Failed: " + res(0, 0))
'                    End If
'                End If
'            End If


'        Catch ex As Exception
'            Dim oerr As New bc_cs_error_log("bc_cs_cs_authenticate", "authenticate", bc_cs_error_codes.USER_DEFINED, ex.Message)
'        End Try


'    End Function
'    Public Sub reset_password_request(ByRef certificate As bc_cs_security.certificate)
'        Try
'            Dim db As New bc_cs_db_services
'            Dim sqlparam As New bc_cs_db_services.bc_cs_sql_parameter
'            Dim sqlparams As New List(Of bc_cs_db_services.bc_cs_sql_parameter)
'            sqlparam = New bc_cs_db_services.bc_cs_sql_parameter
'            sqlparam.name = "email"
'            sqlparam.value = certificate.name
'            sqlparams.Add(sqlparam)

'            Dim res As Object
'            res = db.executesql_with_parameters("dbo.bc_core_cs_reset_password_request", sqlparams, certificate)
'            If IsArray(res) Then
'                If res(0, 0) <> "0" Then
'                    certificate.error_state = True
'                    certificate.server_errors.Add("Password Reset Request Failed: " + res(0, 0))
'                End If
'            End If

'        Catch ex As Exception
'            Dim oerr As New bc_cs_error_log("bc_cs_cs_authenticate", "reset_password_request", bc_cs_error_codes.USER_DEFINED, ex.Message)
'        End Try
'    End Sub
'    Public Sub change_password(ByRef certificate As bc_cs_security.certificate)
'        Try
'            Dim db As New bc_cs_db_services
'            Dim sqlparam As New bc_cs_db_services.bc_cs_sql_parameter
'            Dim sqlparams As New List(Of bc_cs_db_services.bc_cs_sql_parameter)
'            sqlparam = New bc_cs_db_services.bc_cs_sql_parameter
'            sqlparam.name = "email"
'            sqlparam.value = certificate.name
'            sqlparams.Add(sqlparam)
'            sqlparam = New bc_cs_db_services.bc_cs_sql_parameter
'            sqlparam.name = "guid"
'            sqlparam.value = certificate.authentication_token
'            sqlparams.Add(sqlparam)
'            sqlparam = New bc_cs_db_services.bc_cs_sql_parameter
'            sqlparam.name = "password"
'            sqlparam.value = certificate.password
'            sqlparams.Add(sqlparam)
'            Dim res As Object
'            res = db.executesql_with_parameters("dbo.bc_core_cs_change_password", sqlparams, certificate)
'            If IsArray(res) Then
'                If res(0, 0) <> "0" Then
'                    certificate.error_state = True
'                    certificate.server_errors.Add("Password Reset Request Failed: " + res(0, 0))
'                End If
'            End If

'        Catch ex As Exception
'            Dim oerr As New bc_cs_error_log("bc_cs_cs_authenticate", "change_password", bc_cs_error_codes.USER_DEFINED, ex.Message)
'        End Try
'    End Sub

'    Function check_authentication_methods(certificate As bc_cs_security.certificate) As String
'        If certificate.authentication_mode = 0 And bc_cs_central_settings.authentication_method = 0 Then
'            Return ""
'        ElseIf certificate.authentication_mode = 2 And bc_cs_central_settings.authentication_method = 2 Then
'            Return ""
'        ElseIf certificate.authentication_mode = 1 Then
'            Return ""
'        ElseIf certificate.authentication_mode = 2 And certificate.authentication_token = "" Then
'            Return "No token supplied for AD authentication." + bc_cs_central_settings.authentication_method
'        Else
'            Return "Client Server Authentication Mismatch Client Method " + CStr(certificate.authentication_mode) + " Sserver Expecting " + bc_cs_central_settings.authentication_method
'        End If

'    End Function
'End Class
