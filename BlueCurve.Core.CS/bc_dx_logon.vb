
Imports System.Xml.Serialization
Imports System.Web.Script.Serialization
Imports System.IO
Public Class bc_dx_logon
    Implements Ibc_dx_logon

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()
        DevExpress.Skins.SkinManager.EnableFormSkins()
        DevExpress.UserSkins.BonusSkins.Register()
        DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle("Blue")
        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Public Event logon(user_name As String, password As String) Implements Ibc_dx_logon.logon
    Public Event passwordreset(user_name As String) Implements Ibc_dx_logon.passwordreset
    Public Event changepassword(code As String, user_name As String, password As String) Implements Ibc_dx_logon.changepassword
    Public Event test() Implements Ibc_dx_logon.test

    Dim _bhide As Boolean = True
   
    Public Function load_view(Optional bhide As Boolean = True) Implements Ibc_dx_logon.load_view

        _bhide = bhide
        load_view = True
    End Function
    Sub vhide() Implements Ibc_dx_logon.vhide
        Me.Hide()
    End Sub
    Public Function clear_request() Implements Ibc_dx_logon.clear_request
        uxpasswordresetemail.Text = ""
        XtraTabControl1.SelectedTabPageIndex = 2

    End Function
    Public Function clear_reset() Implements Ibc_dx_logon.clear_reset
        uxcode.Text = ""
        uxchemail.Text = ""
        uxp1.Text = ""
        uxp2.Text = ""

        XtraTabControl1.SelectedTabPageIndex = 0

    End Function

    Private Sub bok_Click(sender As Object, e As EventArgs) Handles bok.Click
        Cursor = Windows.Forms.Cursors.WaitCursor
        RaiseEvent logon(Me.uxusername.Text, Me.uxpassword.Text)
        Cursor = Windows.Forms.Cursors.Default

    End Sub

    Private Sub uxusername_EditValueChanged(sender As Object, e As EventArgs) Handles uxusername.EditValueChanged
        check_logon()
    End Sub
    Private Sub uxpassword_EditValueChanged(sender As Object, e As EventArgs)
        check_logon()
    End Sub
    Sub check_logon()
        Me.bok.Enabled = False
        If Trim(Me.uxusername.Text) <> "" And Trim(Me.uxpassword.Text) <> "" Then
            Me.bok.Enabled = True
        End If
    End Sub

    Private Sub bcancel_Click(sender As Object, e As EventArgs) Handles bcancel.Click
        Me.Hide()
    End Sub

    Private Sub uxpassword_EditValueChanged_1(sender As Object, e As EventArgs) Handles uxpassword.EditValueChanged
        check_logon()
    End Sub



    Private Sub uxchangepassword_Click(sender As Object, e As EventArgs)
        RaiseEvent changepassword("a", "a", "a")

    End Sub

    Private Sub uxpasswordreset_Click(sender As Object, e As EventArgs)
        RaiseEvent passwordreset(uxpasswordresetemail.Text)
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        Cursor = Windows.Forms.Cursors.WaitCursor


        RaiseEvent passwordreset(uxpasswordresetemail.Text)
        Cursor = Windows.Forms.Cursors.Default

    End Sub

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        Try
            Cursor = Windows.Forms.Cursors.WaitCursor
            If uxp1.Text.Length < 8 Then
                Dim omsg As New bc_cs_message("Blue Curve", "Password must be a least 8 characters", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Sub
            End If
            If uxp1.Text <> uxp2.Text Then
                Dim omsg As New bc_cs_message("Blue Curve", "Passwords do not match", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Sub
            End If
            REM at least one upper case
            Dim bupper As Boolean = False
            Dim bnumeric As Boolean = False
            For i = 0 To uxp1.Text.Length - 1
                If uxp1.Text(i) = UCase(uxp1.Text(i)) Then
                    bupper = True
                    Exit For
                End If
            Next
            For i = 0 To uxp1.Text.Length - 1
                If IsNumeric(uxp1.Text(i)) = True Then
                    bnumeric = True
                    Exit For
                End If
            Next
            If bupper = False Then
                Dim omsg As New bc_cs_message("Blue Curve", "Password must contain at least 1 upper case character", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Sub
            End If
            If bnumeric = False Then
                Dim omsg As New bc_cs_message("Blue Curve", "Passwords must contain at least 1 number", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Sub
            End If
            RaiseEvent changepassword(uxcode.Text, uxchemail.Text, uxp1.Text)
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_dx_full_logon", "Reset Password", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            Cursor = Windows.Forms.Cursors.Default
        End Try


    End Sub

    Private Sub uxpasswordresetemail_EditValueChanged(sender As Object, e As EventArgs) Handles uxpasswordresetemail.EditValueChanged
        SimpleButton1.Enabled = False
        If InStr(uxpasswordresetemail.Text, "@") = 0 Then
            Exit Sub
        End If
        If Trim(uxpasswordresetemail.Text) <> "" Then
            SimpleButton1.Enabled = True
        End If


    End Sub

    Private Sub uxchemail_EditValueChanged(sender As Object, e As EventArgs) Handles uxchemail.EditValueChanged
        check_correct()
    End Sub

    Private Sub uxcode_EditValueChanged(sender As Object, e As EventArgs) Handles uxcode.EditValueChanged
        check_correct()
    End Sub

    Private Sub uxp1_EditValueChanged(sender As Object, e As EventArgs) Handles uxp1.EditValueChanged
        check_correct()
    End Sub

    Private Sub uxp2_EditValueChanged(sender As Object, e As EventArgs) Handles uxp2.EditValueChanged
        check_correct()
    End Sub
    Sub check_correct()
        SimpleButton2.Enabled = False
        If InStr(uxchemail.Text, "@") = 0 Then
            Exit Sub
        End If
        If uxcode.Text <> "" And uxchemail.Text <> "" And uxp1.Text <> "" And uxp2.Text <> "" Then

            SimpleButton2.Enabled = True

        End If
    End Sub
End Class
Public Class Cbc_dx_logon
    WithEvents _view As Ibc_dx_logon
   

    Public Sub New(view As Ibc_dx_logon)
        _view = view
    End Sub


    Public _username As String
    Public _password As String
    Public _ok As Boolean = False

    Public Sub logon(username As String, password As String) Handles _view.logon
        _username = username
        _password = password
        _ok = True

    End Sub
End Class
Public Class Cbc_dx_full_logon
    WithEvents _view As Ibc_dx_logon
    Dim lo As New bc_cs_logon
    Public success As Boolean = False
    Public user_id As Long
    Public user_name As String
    Public user_email As String
    Public role_id As Long
    Public role_name As String
    Public bshow_form As Boolean = False
    Dim credentials_file As String
    Public Sub New()
        Dim bcs As New bc_cs_central_settings(True)
        credentials_file = bc_cs_central_settings.local_repos_path + "_x.bcx"
    End Sub
    Public Sub remove_credentials_file()
        Dim fs As New bc_cs_file_transfer_services
        If fs.check_document_exists(credentials_file) Then
            fs.delete_file(credentials_file)
        End If
    End Sub
    Function username_password_passthrough() As Boolean
        Try
            username_password_passthrough = False
            Dim fs As New bc_cs_file_transfer_services
            If fs.check_document_exists(credentials_file) Then
                Dim sr As New StreamReader(credentials_file, False)
                Try
                    lo = lo.read_data_from_string(sr.ReadLine(), Nothing)
                    sr.Close()
                    If Not IsNothing(lo) Then
                        If lo.certificate.os_name = bc_cs_central_settings.GetLoginName Then
                            bc_cs_central_settings.override_username_password = True
                            bc_cs_central_settings.user_name = lo.certificate.name
                            bc_cs_central_settings.user_password = lo.certificate.password
                            success = Attempt_Logon(False)
                            If success = True Then
                                username_password_passthrough = True
                                Exit Function
                            End If
                        End If
                    End If
                Catch
                    Try
                        sr.Close()
                    Catch

                    End Try
                End Try
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Cbc_dx_full_logon", "username_password_passthrough", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Function
    Public Function connect(view As Ibc_dx_logon) As Boolean
        Try

            Dim bcs As New bc_cs_central_settings(True)
            If bc_cs_central_settings.bloaderror = True Then
                Dim omsg As New bc_cs_message("Blue Curve", "Configuratuon file not found or cant be read (bc_config.xml). Application cannot be started.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Function
            End If
            If bc_cs_central_settings.show_authentication_form = 2 And bc_cs_central_settings.connection_method = bc_cs_central_settings.ADO Then
                Dim omsg As New bc_cs_message("Blue Curve", "Active Directory Authentication Not Valid in ADO Mode", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Function
            End If
            If bc_cs_central_settings.show_authentication_form > 2 Then
                Dim omsg As New bc_cs_message("Blue Curve", "Invalid Authentication Method: " + CStr(bc_cs_central_settings.show_authentication_form) + " valid is 0=oslogon ,1= username/password 2 = AD", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Function
            End If


            _view = view
            lo.certificate = New bc_cs_security.certificate

            REM if credential file exists always use this irrespective of show_authentication_form
            If username_password_passthrough() = True Then
                Exit Function
            Else
                REM if it doesnt exist or fails then reset
                bc_cs_central_settings.override_username_password = False
                Dim fs As New bc_cs_file_transfer_services
                If fs.check_document_exists(credentials_file) Then
                    remove_credentials_file()
                    bshow_form = False
                End If
            End If

            If bc_cs_central_settings.show_authentication_form = 1 Then
                bshow_form = True
                Exit Function
            End If

           

            success = Attempt_Logon()

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Cbc_dx_full_logon", "Connect", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Function
    'Function show_logon_form()
    '    Try
    '        Dim fs As New bc_cs_file_transfer_services
    '        If fs.check_document_exists(credentials_file) Then
    '            fs.delete_file(credentials_file)
    '        End If
    '        show_logon_form = True
    '        bshow_form = True

    '    Catch ex As Exception
    '        Dim oerr As New bc_cs_error_log("Cbc_dx_full_logon", "show_logon_form", bc_cs_error_codes.USER_DEFINED, ex.Message)
    '    End Try
    'End Function

    Function Attempt_Logon(Optional bshow_msg As Boolean = True)
        Try
            Dim err_tx As String
            Dim ocomm As bc_cs_activity_log
            Attempt_Logon = False
            If bc_cs_central_settings.connection_method = bc_cs_central_settings.ADO Then
                lo.db_read()
                If lo.transmission_state = 2 Then
                    If lo.transmission_state = 1 Then
                        err_tx = "Server Error"
                    ElseIf lo.transmission_state = 2 Then
                        err_tx = "Authentication Denied"
                        If bc_cs_central_settings.show_authentication_form <> 1 And bc_cs_central_settings.override_username_password = False Then
                            If bc_cs_central_settings.show_authentication_form = 0 Then
                                err_tx = "Authentication Denied for os logon please use Email and Password."
                                bc_cs_central_settings.override_username_password = True
                            End If
                        Else
                            err_tx = "Authentication Denied"
                        End If
                        bshow_form = True
                    Else
                        err_tx = "Network Error Cannot Connect To Server."
                    End If

                    ocomm = New bc_cs_activity_log("Cbc_dx_full_logon", "Connect", bc_cs_activity_codes.COMMENTARY, err_tx)

                    If bshow_msg = True Then
                        Dim omsg As New bc_cs_message("Blue Curve", err_tx, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    End If
                Else
                    Attempt_Logon = True
                    bc_cs_central_settings.logged_on_user_id = lo.user_id
                    user_name = lo.user_name
                    user_email = lo.user_name
                    role_id = lo.role_id
                    role_name = lo.role_name

                    _view.vhide()
                    Exit Function
                End If

            Else
                lo.tmode = bc_cs_soap_base_class.tREAD
                If lo.transmit_to_server_and_receive(lo, False) = True Then
                    Attempt_Logon = True
                    bc_cs_central_settings.logged_on_user_id = lo.certificate.user_id
                    user_name = lo.user_name
                    user_email = lo.user_name
                    role_id = lo.role_id
                    role_name = lo.role_name
                    _view.vhide()
                    Exit Function
                Else
                    If lo.transmission_state = 1 Then
                        err_tx = "Server Error"
                    ElseIf lo.transmission_state = 2 Then
                        err_tx = "Authentication Denied"
                        If bc_cs_central_settings.show_authentication_form <> 1 And bc_cs_central_settings.override_username_password = False Then
                            bc_cs_central_settings.override_username_password = True
                            If bc_cs_central_settings.show_authentication_form = 0 Then
                                err_tx = "Authentication Denied for os logon please use Email and Password."
                            ElseIf bc_cs_central_settings.show_authentication_form = 2 Then
                                err_tx = "Authentication Denied for Active Directory please use Email and Password."
                            End If
                        Else
                            err_tx = "Authentication Denied"
                        End If
                        bshow_form = True

                    Else
                        err_tx = "Network Error Cannot Connect To Server."
                    End If
                    If lo.certificate.server_errors.Count > 0 Then
                        err_tx = err_tx + ": " + lo.certificate.server_errors(0)
                    End If
                    ocomm = New bc_cs_activity_log("Cbc_dx_full_logon", "Connect", bc_cs_activity_codes.COMMENTARY, err_tx)

                    For i = 0 To lo.certificate.server_errors.Count - 1
                        ocomm = New bc_cs_activity_log("Cbc_dx_full_logon", "Connect", bc_cs_activity_codes.COMMENTARY, lo.certificate.server_errors(i))
                    Next
                    If bshow_msg = True Then
                        Dim omsg As New bc_cs_message("Blue Curve", err_tx, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    End If
                End If
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Cbc_dx_full_logon", "Attempt_Logon", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Function

    Public Sub logon(username As String, password As String) Handles _view.logon
        Try
            bc_cs_central_settings.user_name = username
            Dim sec As New bc_cs_security

            bc_cs_central_settings.user_password = sec.HashPassword(0, password, Nothing)

            If IsNothing(lo) Then
                lo = New bc_cs_logon
            End If
            If Attempt_Logon() = True Then
                REM save logon details to disk
                lo.certificate.name = bc_cs_central_settings.user_name
                lo.certificate.password = bc_cs_central_settings.user_password
                lo.certificate.os_name = bc_cs_central_settings.GetLoginName

                Dim sw As New StreamWriter(credentials_file, False)
                sw.WriteLine(lo.write_data_to_string(Nothing))
                sw.Close()
                _view.vhide()
                success = True
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Cbc_dx_full_logon", "logon", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Public Sub test() Handles _view.test

    End Sub


    Public Sub change_password(code As String, email As String, password As String) Handles _view.changepassword
        Try
            Dim sec As New bc_cs_security
            Dim omsg As bc_cs_message
            Dim err_tx As String = ""
            password = sec.HashPassword(0, password, Nothing)

            If bc_cs_central_settings.connection_method = bc_cs_central_settings.ADO Then
                lo.certificate.name = email
                lo.certificate.authentication_token = code
                lo.certificate.password = password
                lo.change_password(lo.certificate)


                If lo.certificate.server_errors.Count > 0 Then
                    err_tx = lo.certificate.server_errors(0)
                Else
                    err_tx = "0"
                End If
            Else
                If bc_cs_central_settings.use_rest_post = True Then
                    lo.mode = 2
                    lo.certificate = New bc_cs_security.certificate
                    lo.certificate.name = email
                    lo.certificate.authentication_token = code
                    lo.certificate.password = password
                    err_tx = send_via_rest()
                Else

                    If Right(bc_cs_central_settings.soap_server, 3) = "svc" Then
                        If (IsNothing(bc_cs_soap_base_class.wcfclient)) Then
                            If bc_cs_soap_base_class.load_wcf_proxy() = False Then
                                omsg = New bc_cs_message("Blue Curve", "Cannot Connect to Server", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                                Exit Sub
                            End If
                        End If
                    End If


                    Dim lo = New ServiceReference1.bc_cs_logon
                    lo.mode = 2
                    lo.certificate = New ServiceReference1.bc_cs_securitycertificate
                    lo.certificate.name = email
                    lo.certificate.authentication_token = code
                    lo.certificate.password = password

                    err_tx = bc_cs_soap_base_class.wcfclient.password_management(lo)
                End If
            End If
            If err_tx = "0" Or err_tx = """0""" Then
                omsg = New bc_cs_message("Blue Curve", "Password Changes Successfully Please Logon.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                _view.clear_reset()
            Else
                omsg = New bc_cs_message("Blue Curve", err_tx, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Cbc_dx_full_logon", "change_password", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try


    End Sub
    Sub password_reset_request(user_name As String) Handles _view.passwordreset
        Try
            Dim omsg As bc_cs_message
            Dim err_tx As String = ""
            If bc_cs_central_settings.connection_method = bc_cs_central_settings.ADO Then
                lo.certificate.name = user_name
                lo.reset_password_request(lo.certificate)


                If lo.certificate.server_errors.Count > 0 Then
                    err_tx = lo.certificate.server_errors(0)
                Else
                    err_tx = "0"
                End If
            Else
                If bc_cs_central_settings.use_rest_post = True Then
                    lo.mode = 1
                    lo.certificate = New bc_cs_security.certificate
                    lo.certificate.name = user_name
                    err_tx = send_via_rest()
                Else

                    If Right(bc_cs_central_settings.soap_server, 3) = "svc" Then
                        If (IsNothing(bc_cs_soap_base_class.wcfclient)) Then
                            If bc_cs_soap_base_class.load_wcf_proxy() = False Then
                                omsg = New bc_cs_message("Blue Curve", "Cannot Connect to Server", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                                Exit Sub
                            End If
                        End If
                    End If


                    Dim lo = New ServiceReference1.bc_cs_logon
                    lo.mode = 1
                    lo.certificate = New ServiceReference1.bc_cs_securitycertificate
                    lo.certificate.name = user_name
                    err_tx = bc_cs_soap_base_class.wcfclient.password_management(lo)
                End If
            End If

            If err_tx = "0" Or err_tx = """0""" Then
                omsg = New bc_cs_message("Blue Curve", "You will shortly receive an email with a code." + vbCrLf + "Please use change password with this code to reset your password.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                _view.clear_request()

            Else
                omsg = New bc_cs_message("Blue Curve", err_tx, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Cbc_dx_full_logon", "password_reset_request", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Function send_via_rest() As String
        Try
            Dim certificate As New bc_cs_security.certificate
            Dim json As String

            Dim JsonSerializer = New JavaScriptSerializer
            JsonSerializer.MaxJsonLength = Int32.MaxValue

            json = JsonSerializer.Serialize(lo)

            json = "{""ingot"":" + json + "}"
            Dim rp As New bc_cs_ns_json_post(bc_cs_central_settings.soap_server + "Rest_cs_password_management", json)
            rp.send(certificate)
            If rp.err_text = "" Then
                rp.response_text = Left(rp.response_text, Len(rp.response_text) - 1)
                rp.response_text = Right(rp.response_text, Len(rp.response_text) - 5)
                send_via_rest = rp.response_text
            Else
                send_via_rest = rp.err_text
            End If

        Catch ex As Exception
            send_via_rest = ex.Message
        End Try
    End Function
  
End Class
Public Interface Ibc_dx_logon
    Function load_view(Optional bhide As Boolean = True)
    Function clear_request()
    Function clear_reset()
    Sub vhide()
    Event logon(user_name As String, password As String)
    Event passwordreset(user_name As String)
    Event changepassword(code As String, user_name As String, password As String)
    Event test()
End Interface

<Serializable> Public Class bc_cs_logon
    Inherits bc_cs_soap_base_class
    Public user_id As Long
    Public user_name As String
    Public user_email As String
    Public role_id As Long
    Public role_name As String
    Public mode As Integer
  

    Public Overrides Sub process_object()
        Select Case tmode
            Case bc_cs_soap_base_class.tREAD
                db_read()
        End Select
    End Sub
    Public Sub db_read()
        Try
            REM if ADO mode check logon here
            REM soap has already been autheticated
            Dim db As New bc_cs_db_services
            Dim res As Object
            If bc_cs_central_settings.server_flag = 0 Then
                Dim sqlparam As bc_cs_db_services.bc_cs_sql_parameter
                Dim sqlparams As New List(Of bc_cs_db_services.bc_cs_sql_parameter)
                sqlparam = New bc_cs_db_services.bc_cs_sql_parameter
                sqlparam.name = "method"
                If bc_cs_central_settings.override_username_password = True Then
                    sqlparam.value = 1
                Else
                    sqlparam.value = bc_cs_central_settings.show_authentication_form
                End If
                sqlparams.Add(sqlparam)
                sqlparam = New bc_cs_db_services.bc_cs_sql_parameter
                sqlparam.name = "name"
                If bc_cs_central_settings.show_authentication_form = 0 And bc_cs_central_settings.override_username_password = False Then
                    sqlparam.value = bc_cs_central_settings.GetLoginName
                Else
                    sqlparam.value = bc_cs_central_settings.user_name
                End If
                sqlparams.Add(sqlparam)

                sqlparam = New bc_cs_db_services.bc_cs_sql_parameter
                sqlparam.name = "password"
                sqlparam.value = bc_cs_central_settings.password
                If bc_cs_central_settings.show_authentication_form = 0 And bc_cs_central_settings.override_username_password = False Then
                    sqlparam.value = ""
                Else
                    sqlparam.value = bc_cs_central_settings.user_password
                End If
                sqlparams.Add(sqlparam)
                Dim success As Boolean = False
                res = db.executesql_with_parameters("dbo.bc_core_dx_ado_logon", sqlparams, Nothing)

                If IsArray(res) Then
                    If UBound(res, 2) = 0 Then
                        If IsNumeric(res(0, 0)) Then
                            user_id = res(0, 0)
                            success = True
                            transmission_state = 0
                        End If
                    End If
                End If
                If success = False Then
                    transmission_state = 2
                    Exit Sub
                End If
            Else
                user_id = certificate.user_id
            End If
                REM get user_info

                res = db.executesql("exec dbo.bc_core_dx_logon_info " + CStr(user_id), certificate)
                If (IsArray(res)) Then
                    If UBound(res, 2) = 0 Then
                        user_name = res(0, 0)
                        user_email = res(1, 0)
                        role_id = res(2, 0)
                        role_name = res(3, 0)
                    End If
                End If




        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_cs_logon", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub



    Public Function reset_password_request(ByRef certificate As bc_cs_security.certificate) As String
        Try
            certificate.server_errors = New ArrayList
            certificate.user_id = "reset password request"


            Dim db As New bc_cs_db_services
            Dim sqlparam As New bc_cs_db_services.bc_cs_sql_parameter
            Dim sqlparams As New List(Of bc_cs_db_services.bc_cs_sql_parameter)
            sqlparam = New bc_cs_db_services.bc_cs_sql_parameter
            sqlparam.name = "email"
            sqlparam.value = certificate.name
            sqlparams.Add(sqlparam)


            Dim res As Object
            res = db.executesql_with_parameters("dbo.bc_core_cs_reset_password_request", sqlparams, certificate)


            If IsArray(res) Then
                If res(0, 0) <> "0" Then
                    certificate.error_state = True
                    certificate.server_errors.Add("Password Reset Request Failed: " + res(0, 0))
                End If
            End If

        Catch ex As Exception
            certificate.error_state = True
            Dim oerr As New bc_cs_error_log("bc_cs_cs_authenticate", "reset_password_request", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Function
    Public Sub change_password(ByRef certificate As bc_cs_security.certificate)
        Try
            certificate.server_errors = New ArrayList
            certificate.user_id = "change password"
            Dim db As New bc_cs_db_services
            Dim sqlparam As New bc_cs_db_services.bc_cs_sql_parameter
            Dim sqlparams As New List(Of bc_cs_db_services.bc_cs_sql_parameter)
            sqlparam = New bc_cs_db_services.bc_cs_sql_parameter
            sqlparam.name = "email"
            sqlparam.value = certificate.name
            sqlparams.Add(sqlparam)
            sqlparam = New bc_cs_db_services.bc_cs_sql_parameter
            sqlparam.name = "guid"
            sqlparam.value = certificate.authentication_token
            sqlparams.Add(sqlparam)
            sqlparam = New bc_cs_db_services.bc_cs_sql_parameter
            sqlparam.name = "password"
            sqlparam.value = certificate.password
            sqlparams.Add(sqlparam)
            Dim res As Object
            res = db.executesql_with_parameters("dbo.bc_core_cs_change_password", sqlparams, certificate)
            If IsArray(res) Then
                If res(0, 0) <> "0" Then
                    certificate.error_state = True
                    certificate.server_errors.Add("Password Reset Request Failed: " + res(0, 0))
                End If
            End If

        Catch ex As Exception
            certificate.error_state = True
            Dim oerr As New bc_cs_error_log("bc_cs_cs_authenticate", "change_password", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub

End Class


'End Namespace