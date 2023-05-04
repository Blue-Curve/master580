Imports BlueCurve.Core.CS
Imports System.io
Imports System.Runtime.Serialization
Imports System.Net.NetworkInformation
REM ===============================================
REM Bluecurve Limited 2005
REM Module:       Create User Object Model
REM Type:         Object Model
REM Description:  Users
REM               User
REM Version:      1
REM Change history
REM ===============================================
<Serializable()> Public Class bc_om_get_prefs_for_users
    Inherits bc_cs_soap_base_class
    Public users As New List(Of Long)
    Public prefs As New List(Of bc_om_user_pref)
    Public class_id As Long

    Public Overrides Sub process_object()
        Select Case tmode
            Case tREAD
                db_read()
        End Select

    End Sub
    Public Sub db_read()
        Try
            Dim gdb As New bc_cs_db_services
            Dim sql As String
            Dim vres As Object
            Dim opref As bc_om_user_pref
            prefs.Clear()

            For i = 0 To users.Count - 1
                vres = gdb.executesql("exec dbo.bc_core_prefs_for_user_and_class " + CStr(users(i)) + "," + CStr(class_id), certificate)
                If IsArray(vres) Then
                    For j = 0 To UBound(vres, 2)
                        opref = New bc_om_user_pref
                        opref.entity_id = vres(0, j)
                        prefs.Add(opref)
                    Next
                End If
            Next
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_get_prefs_for_users", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        End Try
    End Sub
End Class


<Serializable()> Public Class bc_om_get_pref_rating
    Inherits bc_cs_soap_base_class
    Public entity_id As Long
    Public pref_type_id As Integer
    Public users As New List(Of bc_om_user)
    Public Overrides Sub process_object()
        Select Case tmode
            Case tREAD
                db_read()
        End Select

    End Sub
    Public Sub db_read()
        Dim db As New bc_cs_db_services
        Dim res As Object
        Dim user As bc_om_user
        res = db.executesql("exec dbo.bc_core_get_pref_rating  " + CStr(entity_id) + "," + CStr(pref_type_id), certificate)
        If IsArray(res) Then
            For i = 0 To UBound(res, 2)
                user = New bc_om_user
                user.id = res(0, i)
                user.first_name = res(1, i)
                user.surname = res(2, i)
                user.rating = i + 1
                user.inactive = res(4, i)
                users.Add(user)
            Next
        End If

    End Sub

End Class

<Serializable()> Public Class bc_om_submit_preferanes_for_type
    Public pref_type_id As Long
    Public entities As New List(Of bc_om_submit_preferanes_for_entity)

    <Serializable()> Public Class bc_om_submit_preferanes_for_entity
        Public entity_id As Long
        Public users As New List(Of Long)
    End Class
End Class


<Serializable()> Public Class bc_om_set_partial_sync
    Inherits bc_cs_soap_base_class
    Public type As Integer

    Public Overrides Sub process_object()
        Select Case MyBase.tmode
            Case tWRITE
                db_write()
        End Select
    End Sub
    Public Sub db_write()
        Dim gdb As New bc_om_user_db
        gdb.set_partial_sync(type, certificate)
    End Sub
End Class
<Serializable()> Public Class bc_om_set_sync
    Inherits bc_cs_soap_base_class

    Public Overrides Sub process_object()
        Select Case MyBase.tmode
            Case tWRITE
                db_write()
        End Select
    End Sub
    Public Sub db_write()
        Dim gdb As New bc_om_user_db
        gdb.set_sync(certificate)
    End Sub
End Class
<Serializable()> Public Class bc_om_create_settings
    Inherits bc_cs_soap_base_class
    Public size As System.Drawing.Size
    Public alerter_on As Boolean
    Public col_1_width As System.Drawing.Size
    Public col_2_width As System.Drawing.Size
    Public col_3_width As System.Drawing.Size
    Public col_4_width As System.Drawing.Size
    Public col_5_width As System.Drawing.Size
    Public col_6_width As System.Drawing.Size
    Public col_7_width As System.Drawing.Size
    Public col_8_width As System.Drawing.Size
End Class
<Serializable()> Public Class bc_om_insight_settings
    Inherits bc_cs_soap_base_class
    Public size As System.Drawing.Size
    Public col_1_width As System.Drawing.Size
    Public col_2_width As System.Drawing.Size
    Public col_3_width As System.Drawing.Size
    Public col_4_width As System.Drawing.Size
    Public col_5_width As System.Drawing.Size
    Public col_6_width As System.Drawing.Size
    Public col_7_width As System.Drawing.Size
    Public col_8_width As System.Drawing.Size
    Public col_9_width As System.Drawing.Size
End Class
<Serializable()> Public Class bc_om_users

    Inherits bc_cs_soap_base_class

    Public user As New ArrayList
    Public pref_types As New ArrayList
    Public inactive As Boolean = False
    Public resetsync As Boolean = True

    Public read_partial_sync As Boolean = False
    Public user_display_attributes As New bc_om_user_display_attributes
    Public user_system_attributes As New List(Of bc_om_user_system_attribute)
    REM FIL June 2013
    <Serializable()> Public Class bc_om_user_display_attributes
        Inherits bc_cs_soap_base_class
        Public user_display_attributes As New List(Of bc_om_user_display_attribute)
        Public Overrides Sub process_object()
            Select Case tmode
                Case bc_cs_soap_base_class.tREAD
                    db_read()
                Case bc_cs_soap_base_class.tWRITE
                    db_write()

            End Select
        End Sub
        Public Sub db_write()
            Dim gdb As New bc_om_user_db
            gdb.clear_user_display_attributes(MyBase.certificate)
            For i = 0 To user_display_attributes.Count - 1
                gdb.add_user_display_attribute(user_display_attributes(i).name, i + 1, MyBase.certificate)
            Next
        End Sub
        Public Sub db_read()
            Dim gdb As New bc_om_user_db
            Dim vres As Object
            vres = gdb.read_display_attributes_for_user(MyBase.certificate)
            Dim ouda As bc_om_user_display_attribute
            Me.user_display_attributes.Clear()

            If IsArray(vres) Then
                For i = 0 To UBound(vres, 2)
                    ouda = New bc_om_user_display_attribute
                    ouda.name = vres(0, i)
                    ouda.attribute_id = vres(1, i)
                    Me.user_display_attributes.Add(ouda)
                Next
            End If

        End Sub
    End Class
    <Serializable()> Public Class bc_om_user_display_attribute
        Public name As String
        Public attribute_id As Long

        Public Sub New()

        End Sub

    End Class
    <Serializable()> Public Class bc_om_user_system_attribute
        Public field_name As String
        Public display_name As String
        Public mandatory As Boolean
        Public add_new As String
        Public lookup_sql As String
        Public lookup_vals As New List(Of String)
        Public lookup_vals_key As New List(Of String)
        Public lookup_vals_inactive As New List(Of Boolean)
        Public type_id As Integer
        Public length As Integer
        Public popup As Boolean
    End Class
    <Serializable()> Public Class bc_om_preference_type
        Inherits bc_cs_soap_base_class
        Public id As Long
        Public name As String
        Public Const ADD = 0
        Public Const DELETE = 1
        Public Const RENAME = 2
        Public write_mode As Integer
        Public delete_err As String

        Public Overrides Sub process_object()
            Dim otrace As New bc_cs_activity_log("bc_om_preference_type", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
            Try
                REM this is always specific to object
                If Me.tmode = bc_cs_soap_base_class.tWRITE Then
                    db_write()
                End If

            Catch ex As Exception
                Dim db_err As New bc_cs_error_log("bc_om_preference_type", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            Finally
                otrace = New bc_cs_activity_log("bc_om_preference_type", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
            End Try
        End Sub
        Public Sub db_write()
            Dim otrace As New bc_cs_activity_log("bc_om_preference_type", "db_write", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

            Try
                Dim gdb As New bc_om_user_db
                Dim vres As Object
                If Me.write_mode = bc_om_preference_type.ADD Then
                    gdb.add_pref_type(Me.name, MyBase.certificate)
                End If

                If Me.write_mode = bc_om_preference_type.RENAME Then
                    gdb.rename_pref_type(Me.id, Me.name, MyBase.certificate)
                End If
                If Me.write_mode = bc_om_preference_type.DELETE Then
                    vres = gdb.delete_pref_type(Me.id, MyBase.certificate)
                    Try
                        If IsArray(vres) Then
                            Me.delete_err = vres(0, 0)
                        End If
                    Catch ex As Exception
                        Me.delete_err = "general error " + ex.Message

                    End Try
                End If


            Catch ex As Exception
                Dim db_err As New bc_cs_error_log("bc_om_preference_type", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            Finally
                otrace = New bc_cs_activity_log("bc_om_preference_type", "db_write", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
            End Try
        End Sub

    End Class
    REM ===============

    Public Overrides Sub process_object()
        Dim otrace As New bc_cs_activity_log("bc_om_user_prefs", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            REM this is always specific to object
            If Me.tmode = bc_cs_soap_base_class.tREAD Then
                db_read()
            End If
            If Me.tmode = bc_cs_soap_base_class.tHASHPASSWORD Then
                Me.HashPassword()
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om__user_prefs", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om__user_prefs", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub
    Public Function test_connection() As Boolean
        Dim db_user As New bc_om_user_db
        test_connection = db_user.test_connection
    End Function

    Public Sub db_read()

        Dim otrace As New bc_cs_activity_log("bc_om_users", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "", MyBase.certificate)
        Try
            Dim i As Integer
            Dim ouser As bc_om_user
            Dim opref_type As bc_om_preference_type

            Dim db_user As New bc_om_user_db
            Dim vusers As Object
            Dim vpref_types As Object
            Me.user.Clear()
            Me.pref_types.Clear()

            MyBase.certificate = certificate
            vusers = db_user.read_all_users(inactive, resetsync, MyBase.certificate)
            'If resetsync = True Then
            '    reset_user_sync()
            'End If
            If IsArray(vusers) Then
                For i = 0 To UBound(vusers, 2)
                    ouser = New bc_om_user(vusers(0, i), vusers(1, i), vusers(2, i), vusers(3, i), vusers(4, i), vusers(5, i), vusers(6, i), vusers(7, i), Me.certificate)
                    ouser.inactive = vusers(8, i)
                    ouser.office_id = vusers(9, i)
                    ouser.role_level = vusers(10, i)
                    ouser.language_id = vusers(11, i)
                    ouser.role_id = vusers(12, i)
                    ouser.password = vusers(13, i)
                    ouser.mobile = vusers(14, i)
                    ouser.job_title = vusers(15, i)
                    REM add taxonomy display flag
                    ouser.no_display_taxonomy = db_user.do_not_display_taxonomy(vusers(0, i), certificate)

                    REM FIL JUNE 2013
                    REM get partial sync settings
                    If read_partial_sync = True Then
                        ouser.read_partial_sync_settings_only = True
                        ouser.db_read()
                        ouser.read_partial_sync_settings_only = False
                    End If
                    user.Add(ouser)
                Next
            End If
            REM FIL July 2012
            vpref_types = db_user.get_pref_types(certificate)
            Me.pref_types.Clear()
            If IsArray(vpref_types) Then
                For i = 0 To UBound(vpref_types, 2)
                    opref_type = New bc_om_preference_type
                    opref_type.id = vpref_types(0, i)
                    opref_type.name = vpref_types(1, i)
                    Me.pref_types.Add(opref_type)
                Next
            End If
            If Me.pref_types.Count = 0 Then
                opref_type = New bc_om_preference_type
                opref_type.id = 1
                opref_type.name = "Analyst Preference"
                Me.pref_types.Add(opref_type)
            End If

            REM FIL JUne 2013 custom user attributes
            Dim vres As Object
            vres = db_user.read_system_attributes_for_user(MyBase.certificate)
            Dim ousa As bc_om_user_system_attribute


            If IsArray(vres) Then
                For i = 0 To UBound(vres, 2)
                    ousa = New bc_om_user_system_attribute
                    ousa.field_name = vres(0, i)
                    ousa.display_name = vres(1, i)
                    ousa.mandatory = vres(2, i)
                    ousa.lookup_sql = vres(3, i)
                    ousa.add_new = vres(4, i)
                    ousa.type_id = vres(5, i)
                    ousa.length = vres(6, i)
                    ousa.popup = vres(7, i)

                    If ousa.lookup_sql <> "" Then
                        Dim lres As Object
                        Dim sql As New bc_om_sql(ousa.lookup_sql)
                        sql.db_read()
                        lres = sql.results
                        For j = 0 To UBound(lres, 2)
                            ousa.lookup_vals.Add(lres(0, j))
                            ousa.lookup_vals_key.Add(lres(1, j))
                            ousa.lookup_vals_inactive.Add(lres(2, j))
                        Next
                    End If
                    Me.user_system_attributes.Add(ousa)
                Next
            End If

            Me.user_display_attributes.db_read()

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_users", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, MyBase.certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_users", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", MyBase.certificate)
        End Try
    End Sub


    'Public Sub reset_user_sync()
    '    Dim otrace As New bc_cs_activity_log("bc_om_users", "reset_user_sync", bc_cs_activity_codes.TRACE_ENTRY, "", MyBase.certificate)

    '    Try
    '        Dim db_user As New bc_om_user_db
    '        Dim ocommentary As New bc_cs_activity_log("bc_om_users", "reser_user_sync", bc_cs_activity_codes.COMMENTARY, "Resetting User Sync: via connection method: " + bc_cs_central_settings.selected_conn_method, Me.certificate)
    '        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
    '            db_user.reset_user_sync(MyBase.certificate)
    '        End If

    '    Catch ex As Exception
    '        Dim db_err As New bc_cs_error_log("bc_om_users", "reset_user_synv", bc_cs_error_codes.USER_DEFINED, ex.Message, MyBase.certificate)
    '    Finally
    '        otrace = New bc_cs_activity_log("bc_om_users", "reset_user_sync", bc_cs_activity_codes.TRACE_EXIT, "", MyBase.certificate)
    '    End Try
    'End Sub
    REM specific web service for object model
    'Public Overloads Overrides Function call_web_service() As String
    '    Dim otrace As New bc_cs_activity_log("bc_om_users", "call_web_service", bc_cs_activity_codes.TRACE_ENTRY, "", MyBase.certificate)
    '    Try
    '        Dim ocommentary As New bc_cs_activity_log("bc_om_users", "LoadUsers", bc_cs_activity_codes.COMMENTARY, "Attempting to call SOAP Method LoadUsers.")
    '        If IsNumeric(bc_cs_central_settings.timeout) Then
    '            webservice.Timeout = CLng(bc_cs_central_settings.timeout)
    '            ocommentary = New bc_cs_activity_log("bc_om_web services", "call_web_service", bc_cs_activity_codes.COMMENTARY, "Setting Web Service Timeout to: " + bc_cs_central_settings.timeout, MyBase.certificate)
    '        End If
    '        Dim s As String
    '        s = webservice.LoadUsers()
    '        call_web_service = s
    '    Catch ex As Exception
    '        MsgBox(ex.Message)
    '    Finally
    '        otrace = New bc_cs_activity_log("bc_om_users", "call_web_service", bc_cs_activity_codes.TRACE_EXIT, "", MyBase.certificate)
    '    End Try
    'End Function
    Public Function check_logon(ByRef ocurrent_user As bc_om_user) As Integer
        Dim slog As New bc_cs_activity_log("bc_om_users", "check_logon", bc_cs_activity_codes.TRACE_ENTRY, "", MyBase.certificate)

        Try
            Dim i As Integer
            Dim ocommentary As bc_cs_activity_log
            check_logon = 0
            REM see if current user has valid logon credentials
            REM get current local logon for user
            For i = 0 To Me.user.Count - 1
                If bc_cs_central_settings.show_authentication_form = 0 Then
                    If UCase(Trim(Me.user(i).os_user_name)) = UCase(bc_cs_central_settings.logged_on_user_name) Then
                        check_logon = 1
                    End If
                ElseIf bc_cs_central_settings.show_authentication_form = 1 Then
                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                        If UCase(Trim(Me.user(i).user_name)) = UCase(Trim(bc_cs_central_settings.user_name)) Then 'already authenticated on the server using password
                            check_logon = 1
                        End If
                    Else
                        If (UCase(Trim(Me.user(i).user_name)) = UCase(Trim(bc_cs_central_settings.user_name))) And (UCase(Trim(Me.user(i).password)) = UCase(Trim(bc_cs_central_settings.user_password))) Then
                            check_logon = 1
                        End If
                    End If
                ElseIf bc_cs_central_settings.show_authentication_form = 2 And bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                    If UCase(Trim(Me.user(i).os_user_name)) = UCase(bc_cs_central_settings.logged_on_user_name) Then
                        check_logon = 1
                    End If
                ElseIf bc_cs_central_settings.selected_autenticated_method = 3 And bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                    If InStr(UCase(Trim(bc_cs_central_settings.user_name)), UCase(Trim(Me.user(i).os_user_name))) > 0 Then
                        check_logon = 1
                    End If
                Else
                    Dim oerr As New bc_cs_error_log("bc_om_users", "check_logon", bc_cs_error_codes.USER_DEFINED, "Invalid connection method for selected authetication method")
                    check_logon = 0
                End If
                If check_logon = 1 Then
                    ocurrent_user = Me.user(i)
                    Me.certificate = MyBase.certificate

                    bc_cs_central_settings.logged_on_user_id = Me.user(i).id
                    ocommentary = New bc_cs_activity_log("bc_om_users", "check_logon", bc_cs_activity_codes.COMMENTARY, "Logon Sucessful for: " + Me.user(i).first_name + " " + Me.user(i).surname, MyBase.certificate)
                    'If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    '    reset_user_sync()
                    'End If
                    Exit For
                End If
            Next

            If check_logon = 0 Then
                If (bc_cs_central_settings.show_authentication_form = 0 Or bc_cs_central_settings.show_authentication_form = 2) Then
                    ocommentary = New bc_cs_activity_log("bc_om_users", "check_logon", bc_cs_activity_codes.COMMENTARY, "Logon Failed for OS logon: " + bc_cs_central_settings.logged_on_user_name, MyBase.certificate)
                Else
                    ocommentary = New bc_cs_activity_log("bc_om_users", "check_logon", bc_cs_activity_codes.COMMENTARY, "Logon Failed for OS logon: " + bc_cs_central_settings.user_name, MyBase.certificate)
                End If
            Else

                REM FIL JUNE 2013
                If ocurrent_user.sync_level = 0 Then

                    ocurrent_user.reset_partial_sync = True
                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then

                        ocurrent_user.db_read()

                    Else
                        ocurrent_user.tmode = bc_cs_soap_base_class.tREAD
                        ocurrent_user.transmit_to_server_and_receive(ocurrent_user, True)
                    End If

                End If
                ocurrent_user.reset_partial_sync = False
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_users", "check_logon", bc_cs_error_codes.USER_DEFINED, ex.Message, MyBase.certificate)
        Finally


            slog = New bc_cs_activity_log("bc_om_users", "check_logon", bc_cs_activity_codes.TRACE_EXIT, "", MyBase.certificate)
        End Try
    End Function

    Public Sub HashPassword(Optional ByRef userPassword As String = "")

        '==========================================
        ' Blue Curve Limited 2010
        ' Desciption:    Hash user entered password
        '                 
        ' Version:        1.0
        ' Change history:
        '
        '==========================================

        Dim slog As New bc_cs_activity_log("bc_om_users", "HashPassword", bc_cs_activity_codes.TRACE_ENTRY, "", MyBase.certificate)
        Dim salt As String
        Dim enteredPassword As String
        Dim hashedPassword As String

        Try
            Dim i As Integer
            Dim ocommentary As New bc_cs_activity_log("bc_om_users", "HashPassword", bc_cs_activity_codes.COMMENTARY, "Role based financial workflow not installed", Me.certificate)
            Dim hashProvider As New bc_cs_encyption_hash(bc_cs_encyption_hash.Provider.SHA1)

            If userPassword <> "" Then
                enteredPassword = userPassword
            Else

                If bc_cs_central_settings.server_flag = 0 Then
                    enteredPassword = bc_cs_central_settings.user_password
                Else
                    enteredPassword = MyBase.certificate.password
                End If
            End If

            salt = "0"
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                Me.db_read()
            Else
                Me.tmode = bc_cs_soap_base_class.tREAD
                Me.transmit_to_server_and_receive(Me, True)
            End If
            For i = 0 To Me.user.Count - 1
                If bc_cs_central_settings.server_flag = 0 Then
                    If (UCase(Trim(Me.user(i).user_name)) = UCase(Trim(bc_cs_central_settings.user_name))) Then
                        salt = CStr(Me.user(i).id)
                    End If
                Else
                    If (UCase(Trim(Me.user(i).user_name)) = UCase(Trim(MyBase.certificate.name))) Then
                        salt = CStr(Me.user(i).id)
                    End If

                End If
            Next

            Dim hashData As New bc_cs_encyption_data(enteredPassword)
            Dim hashsalt As New bc_cs_encyption_data(salt)
            hashProvider.Calculate(hashData, hashsalt)
            hashedPassword = hashProvider.Value.ToHex

            If userPassword <> "" Then
                userPassword = hashedPassword
            Else
                If bc_cs_central_settings.server_flag = 0 Then
                    bc_cs_central_settings.user_password = hashedPassword
                Else
                    MyBase.certificate.password = hashedPassword
                End If
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_users", "HashPassword", bc_cs_error_codes.USER_DEFINED, ex.Message, MyBase.certificate)
        Finally
            slog = New bc_cs_activity_log("bc_om_users", "HashPassword", bc_cs_activity_codes.TRACE_EXIT, "", MyBase.certificate)
        End Try

    End Sub



    Public Sub New()

    End Sub
End Class
<Serializable()> Public Class bc_om_user
    Inherits bc_cs_soap_base_class

    Public id As Long
    Public os_user_name As String
    Public surname As String
    Public first_name As String
    Public middle_name As String
    Public email As String
    Public telephone As String
    Public fax As String
    Public sync_level As Integer
    Public role As String
    Public role_id As Long
    Public user_name As String
    Public password As String
    Public no_display_taxonomy As Boolean
    Public office_id As Long
    Public role_level As Integer
    Public language_id As Long
    Public bus_areas As New ArrayList
    Public prefs As New List(Of bc_om_user_pref)
    Public inactive As Boolean = False
    Public write_mode As Integer
    Public picture As Byte()
    Public picture_extension As String
    Public comment As String
    Public change_user As String
    Public delete_error As String = ""
    Public set_user_prefs As New bc_om_set_user_prefs
    Public mobile As String
    Public job_title As String
    Public biography As String
    Public associated_users As New ArrayList
    Public other_roles As New ArrayList
    REM FIL JUN 2013
    Public sync_settings As New List(Of Long)
    Public reset_partial_sync As Boolean = False
    Public read_partial_sync_settings_only As Boolean = False
    Public attribute_values As New List(Of bc_om_attribute_value)
    Public rating As Integer
    REM FIL 5.5
    Public sort As Boolean = False
    Public rating_changed As Boolean = False
    Public Const INSERT = 0
    Public Const UPDATE = 1
    Public Const DELETE = 2
    Public Const SET_ACTIVE = 3
    Public Const SET_INACTIVE = 4
    Public Const SET_SYNC = 5
    Public Const CLEAR_SYNC = 6
    Public Const WRITE_PARTIAL_SYNC = 7
    Public Const tGET_SYNC_LEVEL = 10
    Public Const INSERT_AND_SET_DEFAULT_ATTRIBUTE = 11
    Public submit_prefs As New List(Of bc_om_submit_preferanes_for_type)
    Public picture_change As Boolean = False

    Public Overrides Sub process_object()
        Select Case tmode
            Case tREAD
                db_read()
            Case tWRITE
                db_write()
            Case tGET_SYNC_LEVEL
                get_sync_level()
        End Select
    End Sub
    Public Sub get_sync_level()
        Dim gdb As New bc_om_user_db
        Dim vres As Object
        Me.sync_level = 0
        Me.sync_settings.Clear()

        If bc_cs_central_settings.server_flag = False Then
            Try
                certificate = New bc_cs_security.certificate
                certificate.user_id = bc_cs_central_settings.logged_on_user_id
                Dim nics() As NetworkInterface = NetworkInterface.GetAllNetworkInterfaces()

                For Each adapter In nics
                    Select Case adapter.NetworkInterfaceType
                        'Exclude Tunnels, Loopbacks and PPP
                        Case NetworkInterfaceType.Tunnel, NetworkInterfaceType.Loopback, NetworkInterfaceType.Ppp
                        Case Else
                            If Not adapter.GetPhysicalAddress.ToString = String.Empty And Not adapter.GetPhysicalAddress.ToString = "00000000000000E0" Then
                                certificate.client_mac_address = adapter.GetPhysicalAddress.ToString
                                Exit For ' Got a mac so exit for
                            End If

                    End Select
                Next adapter
            Catch
                certificate.client_mac_address = "cant find mac address"
            End Try


        End If

        vres = gdb.get_sync_level(Me.id, MyBase.certificate)


        If IsArray(vres) Then
            For i = 0 To UBound(vres, 2)
                If vres(0, 0) = 1 Then
                    Me.sync_level = 1
                    Exit For
                Else
                    Me.sync_level = 0
                    Me.sync_settings.Add(vres(1, i))
                End If
            Next
        End If

    End Sub
    Public Sub db_read()
        Try
            Dim vres As Object
            Dim opref As bc_om_user_pref
            Dim gdb As New bc_om_user_db

            If reset_partial_sync = True Then

                REM FIL JUN 2013
                sync_settings.Clear()
                vres = gdb.get_user_sync_settings(Me.id, MyBase.certificate)
                If IsArray(vres) Then
                    For i = 0 To UBound(vres, 2)
                        Me.sync_settings.Add(vres(0, i))
                    Next
                End If
                REM PR MAy 2016 mac address
                'gdb.reset_partial_sync(MyBase.certificate)
                Exit Sub
            End If

            REM FIL JUN 2013

            sync_settings.Clear()
            vres = gdb.get_user_sync_settings(Me.id, MyBase.certificate)
            If IsArray(vres) Then
                For i = 0 To UBound(vres, 2)
                    Me.sync_settings.Add(vres(0, i))
                Next
            End If

            REM ================================================
            If read_partial_sync_settings_only = False Then
                bus_areas.Clear()
                prefs.Clear()

                vres = gdb.read_user(Me.id, MyBase.certificate)
                If IsArray(vres) Then
                    If UBound(vres, 2) > -1 Then
                        Me.first_name = vres(0, 0)
                        Me.surname = vres(1, 0)
                        Me.middle_name = vres(2, 0)
                        Me.role = vres(3, 0)
                        Me.os_user_name = vres(4, 0)
                        Me.language_id = vres(5, 0)
                        Me.email = vres(6, 0)
                        Me.telephone = vres(7, 0)
                        Me.fax = vres(8, 0)
                        Me.office_id = vres(9, 0)
                        Me.comment = vres(10, 0)
                        Me.role_id = vres(11, 0)
                        Me.change_user = CStr(vres(12, 0))
                        Me.sync_level = vres(13, 0)
                        Me.password = vres(14, 0)
                        Me.mobile = vres(15, 0)
                        Me.job_title = vres(16, 0)
                        Me.biography = vres(17, 0)
                        REM ING August 2012
                        Me.user_name = vres(18, 0)
                        Me.picture_extension = vres(19, 0)

                    End If
                End If

                vres = gdb.read_bus_areas_for_user(Me.id, MyBase.certificate)
                If IsArray(vres) Then
                    For i = 0 To UBound(vres, 2)
                        bus_areas.Add(vres(0, i))
                    Next

                End If
                REM FIL 5.5
                Dim pentity_id As Long = 0
                Dim ptype As Integer = 0
                Dim puser As bc_om_user = Nothing
                vres = gdb.read_prefs_for_user(Me.id, MyBase.certificate)
                Me.sort = False
                Dim rating As Integer
                If IsArray(vres) Then
                    For i = 0 To UBound(vres, 2)
                        If pentity_id = 0 Or pentity_id <> vres(0, i) Or ptype = 0 Or ptype <> vres(3, i) Then
                            rating = 1
                            opref = New bc_om_user_pref
                            opref.entity_id = vres(0, i)
                            opref.class_id = vres(1, i)
                            opref.rating = vres(2, i)
                            opref.pref_type = vres(3, i)
                            opref.pref_name = vres(4, i)
                            opref.entity_name = vres(5, i)
                            opref.inactive = vres(6, i)
                            opref.class_name = vres(7, i)
                            prefs.Add(opref)
                        End If
                        puser = New bc_om_user
                        puser.id = vres(10, i)
                        puser.first_name = vres(8, i)
                        puser.surname = vres(9, i)
                        puser.inactive = vres(11, i)
                        puser.rating = rating
                        rating = rating + 1
                        prefs(prefs.Count - 1).users.Add(puser)
                        ptype = vres(3, i)
                        pentity_id = vres(0, i)
                    Next
                End If

                REM ===============
                REM read picture
                Dim fs As New bc_cs_file_transfer_services
                Dim picname As String
                picname = bc_cs_central_settings.central_repos_path + "user images\" + CStr(Me.id) + Me.picture_extension


                If fs.check_document_exists(picname) Then
                    fs.write_document_to_bytestream(picname, Me.picture, MyBase.certificate)
                End If

                vres = gdb.read_associated_users(Me.id, MyBase.certificate)
                Me.associated_users.Clear()

                If IsArray(vres) Then
                    For i = 0 To UBound(vres, 2)
                        Me.associated_users.Add(vres(0, i))
                    Next
                End If

                other_roles.Clear()
                vres = gdb.read_other_roles(Me.id, MyBase.certificate)
                If IsArray(vres) Then
                    For i = 0 To UBound(vres, 2)
                        Me.other_roles.Add(vres(0, i))
                    Next
                End If
                REM common platform attributes
                For i = 0 To attribute_values.Count - 1
                    attribute_values(i).user_id = Me.id

                    attribute_values(i).certificate = MyBase.certificate
                    attribute_values(i).value_changed = False
                    attribute_values(i).db_read()
                Next

            End If


        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_user", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, MyBase.certificate)
        End Try
    End Sub

    Public Sub db_write()
        Dim gdb As New bc_om_user_db
        Dim res As Object


        If bc_cs_central_settings.server_flag = 0 Then
            certificate.user_id = bc_cs_central_settings.logged_on_user_id
        End If
        Select Case Me.write_mode

            Case INSERT
                res = gdb.create_new_user(first_name, surname, MyBase.certificate)
                If IsArray(res) Then
                    Me.id = res(0, 0)
                End If

                For i = 0 To Me.attribute_values.Count - 1
                    Me.attribute_values(i).user_id = Me.id
                    Me.attribute_values(i).certificate = MyBase.certificate
                    Me.attribute_values(i).db_write()
                Next

                write_bus_area()
                write_prefs(Me.id)

                save_picture()

                update_user_associations()
                update_other_roles()
                set_partial_sync()
                gdb.audit_entity_action(Me.id, "Created", MyBase.certificate)

            Case INSERT_AND_SET_DEFAULT_ATTRIBUTE
                res = gdb.create_new_user(first_name, surname, MyBase.certificate)
                If IsArray(res) Then
                    Me.id = res(0, 0)
                End If

                For i = 0 To Me.attribute_values.Count - 1
                    Me.attribute_values(i).user_id = Me.id
                    Me.attribute_values(i).certificate = MyBase.certificate
                    Me.attribute_values(i).db_write()
                Next

                write_bus_area()
                write_prefs(Me.id)

                save_picture()

                update_user_associations()
                update_other_roles()
                set_partial_sync()
                gdb.audit_entity_action(Me.id, "Created", MyBase.certificate)
                gdb.set_default_attribute_values(Me.id, MyBase.certificate)
            Case UPDATE
                For i = 0 To Me.attribute_values.Count - 1
                    Me.attribute_values(i).user_id = Me.id
                    Me.attribute_values(i).certificate = MyBase.certificate
                    Me.attribute_values(i).db_write()
                Next
                write_bus_area()
                write_prefs(Me.id)
                save_picture()
                update_user_associations()
                update_other_roles()
                set_partial_sync()
            Case SET_ACTIVE
                gdb.set_user_active(0, Me.id, MyBase.certificate)
                gdb.audit_entity_action(Me.id, "Set Active", MyBase.certificate)
            Case SET_INACTIVE
                gdb.set_user_active(1, Me.id, MyBase.certificate)
                gdb.audit_entity_action(Me.id, "Set Inactive", MyBase.certificate)
                REM ING August 2012
            Case SET_SYNC
                gdb.set_sync_flag(1, Me.id, MyBase.certificate)
            Case CLEAR_SYNC
                gdb.set_sync_flag(0, Me.id, MyBase.certificate)
            Case WRITE_PARTIAL_SYNC
                set_partial_sync()

            Case DELETE
                Me.delete_error = gdb.delete_user(Me.id, MyBase.certificate)
                'If Me.delete_error = "" Then
                '    REM remove picture
                '    Dim ffs As New bc_cs_file_transfer_services
                '    If ffs.check_document_exists(bc_cs_central_settings.central_repos_path + CStr(Me.id) + ".bmp") Then
                '        ffs.delete_file(bc_cs_central_settings.central_repos_path + CStr(Me.id) + ".bmp")
                '    End If

                'End If
                gdb.audit_entity_action(Me.id, "Deleted", MyBase.certificate)
        End Select

    End Sub
    Private Sub update_other_roles()
        Dim gdb As New bc_om_user_db
        gdb.delete_other_roles(Me.id, certificate)
        For i = 0 To Me.other_roles.Count - 1
            gdb.write_other_role(Me.id, Me.other_roles(i), certificate)
        Next
    End Sub
    Private Sub set_partial_sync()
        Dim gdb As New bc_om_user_db
        gdb.delete_user_sync_settings(Me.id, certificate)
        For i = 0 To Me.sync_settings.Count - 1
            gdb.set_user_sync_settings(Me.id, Me.sync_settings(i), certificate)
        Next
    End Sub
    Private Sub update_user_associations()
        Dim gdb As New bc_om_user_db
        gdb.delete_associated_users(Me.id, certificate)
        For i = 0 To Me.associated_users.Count - 1
            gdb.write_associated_user(Me.id, Me.associated_users(i), certificate)
        Next
    End Sub
    Private Sub write_prefs(ByVal id As Long)
        Dim otrace As New bc_cs_activity_log("bc_om_user", "write_prefs", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Dim gdb As New bc_om_user_db
            Dim audit_id As Long
            Dim vres As Object
            vres = gdb.get_audit_id(certificate)
            If IsArray(vres) Then
                If UBound(vres, 2) = 0 Then
                    audit_id = vres(0, 0)
                End If
            End If


            For i = 0 To submit_prefs.Count - 1
                For j = 0 To submit_prefs(i).entities.Count - 1
                    REM delete prefs for entity
                    gdb.delete_prefs_for_entity(audit_id, submit_prefs(i).entities(j).entity_id, submit_prefs(i).pref_type_id, Me.id, certificate)

                Next
            Next
            REM remove prefs for user
            gdb.delete_prefs_for_user(audit_id, Me.id, MyBase.certificate)

            For i = 0 To submit_prefs.Count - 1
                For j = 0 To submit_prefs(i).entities.Count - 1
                    For k = 0 To submit_prefs(i).entities(j).users.Count - 1
                        If submit_prefs(i).entities(j).users(k) = 0 Then
                            gdb.insert_prefs_for_entity(audit_id, submit_prefs(i).entities(j).entity_id, Me.id, k + 1, submit_prefs(i).pref_type_id, certificate)
                        Else
                            gdb.insert_prefs_for_entity(audit_id, submit_prefs(i).entities(j).entity_id, submit_prefs(i).entities(j).users(k), k + 1, submit_prefs(i).pref_type_id, certificate)
                            'gdb.insert_prefs_for_entity(audit_id, submit_prefs(i).entities(j).entity_id, Me.id, k + 1, submit_prefs(i).pref_type_id, certificate)
                        End If
                    Next
                Next
            Next

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_user", "write_prefs", bc_cs_error_codes.USER_DEFINED, ex.Message, MyBase.certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_user", "write_prefs", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub
    Private Sub write_bus_area()
        Dim otrace As New bc_cs_activity_log("bc_om_user", "write_bus_area", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Dim gdb As New bc_om_user_db
            gdb.delete_bus_area_for_user(Me.id, MyBase.certificate)
            For i = 0 To Me.bus_areas.Count - 1
                gdb.add_bus_area_for_user(Me.id, Me.bus_areas(i), MyBase.certificate)
            Next

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_user", "write_bus_area", bc_cs_error_codes.USER_DEFINED, ex.Message, MyBase.certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_user", "write_bus_area", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub
    Private Sub save_picture()
        Dim otrace As New bc_cs_activity_log("bc_om_user", "save_picture", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try

            If picture_change = False Then
                Exit Sub
            End If
            Dim gdb As New bc_om_user_db
            Dim fs As New bc_cs_file_transfer_services
            If Not (Me.picture Is Nothing) Then
                fs.write_bytestream_to_document(bc_cs_central_settings.central_repos_path + "user images\" + CStr(Me.id) + Me.picture_extension, Me.picture, MyBase.certificate)
                gdb.add_picture_extension(Me.id, Me.picture_extension, True, certificate)

            Else
                gdb.add_picture_extension(Me.id, "", False, certificate)
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_user", "save_picture", bc_cs_error_codes.USER_DEFINED, ex.Message, MyBase.certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_user", "save_picture", bc_cs_activity_codes.TRACE_EXIT, "", certificate)

        End Try

    End Sub
    Public Sub New()
        no_display_taxonomy = False
    End Sub
    Public Sub New(ByVal iid As Long, ByVal stros_user_name As String, ByVal strsurname As String, ByVal strfirst_name As String, ByVal isync_level As Integer, ByVal role As String, ByVal password As String, ByVal user_name As String, ByVal certificate As bc_cs_security.certificate)
        Dim otrace As New bc_cs_activity_log("bc_om_user", "new", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            id = iid
            os_user_name = stros_user_name
            surname = strsurname
            first_name = strfirst_name
            sync_level = isync_level
            Me.role = role
            Me.password = password
            Me.user_name = user_name
            no_display_taxonomy = False
            MyBase.certificate = certificate

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_users", "new", bc_cs_error_codes.USER_DEFINED, ex.Message, MyBase.certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_user", "new", bc_cs_activity_codes.TRACE_EXIT, "", MyBase.certificate)
        End Try

    End Sub
    Public Function check_authentication() As Long
        Dim gdb_db As New bc_om_user_db
        Dim vres As Object
        check_authentication = 0
        Me.os_user_name = bc_cs_central_settings.GetLoginName
        Dim str As New bc_cs_string_services(Me.os_user_name)
        Me.os_user_name = str.delimit_apostrophies
        vres = gdb_db.check_authentication(Me.os_user_name, MyBase.certificate)
        If IsArray(vres) Then
            If UBound(vres, 2) > -1 Then
                check_authentication = CLng(vres(0, 0))
            End If
        End If

    End Function

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class

REM ===============================================
REM Bluecurve Limited 2005
REM Module:       Create User Preference Model
REM Type:         Object Model
REM Description:  Users
REM               User
REM Version:      1
REM Change history
REM ===============================================
<Serializable()> Public Class bc_om_user_bus_area
    Public user_id As Long
    Public bus_area_id As Long

    Public Sub New()

    End Sub
End Class
REM FIL HUn 2013
<Serializable()> Public Class bc_om_sync_types
    Inherits bc_cs_soap_base_class


    Public sync_types As New List(Of bc_om_sync_type)

    Public Overrides Sub process_object()
        Select Case tmode
            Case bc_cs_soap_base_class.tWRITE
                db_write()
        End Select
    End Sub

    Public Sub db_write()
        Dim db As New bc_om_user_db
        For i = 0 To Me.sync_types.Count - 1
            If Me.sync_types(i).sync_set = True Then
                db.set_user_sync_type_for_users(Me.sync_types(i).id, MyBase.certificate)
            End If

        Next
    End Sub

End Class
<Serializable()> Public Class bc_om_sync_type
    Public id As Long
    Public name As String
    Public sync_set As Boolean

    Public Sub New()

    End Sub
End Class
<Serializable()> Public Class bc_om_user_admin
    Inherits bc_cs_soap_base_class
    Public roles As New ArrayList
    Public business_areas As New ArrayList
    Public offices As New ArrayList
    Public languages As New ArrayList
    Public user_bus_areas As New ArrayList
    Public installed_apps As New ArrayList
    Public sync_types As New bc_om_sync_types


    Public Overrides Sub process_object()
        Select Case Me.tmode
            Case tREAD
                db_read()
        End Select
    End Sub
    Public Sub db_read()
        Dim gbb As New bc_om_user_db
        Dim vres As Object
        Dim orole As bc_om_user_role
        Dim ooffice As bc_om_user_office
        Dim obus_area As bc_om_bus_area
        Dim olang As bc_om_user_language
        Dim opbus = New bc_om_user_bus_area

        REM read roles
        roles.Clear()
        offices.Clear()
        business_areas.Clear()
        languages.Clear()
        user_bus_areas.Clear()
        installed_apps.Clear()

        Dim gdb As New bc_om_user_db
        vres = gdb.read_installed_apps(MyBase.certificate)
        If IsArray(vres) Then
            For i = 0 To UBound(vres, 2)
                Me.installed_apps.Add(vres(0, i))
            Next
        End If

        vres = gdb.read_roles(MyBase.certificate)
        If IsArray(vres) Then
            For i = 0 To UBound(vres, 2)
                orole = New bc_om_user_role
                orole.id = vres(0, i)
                orole.description = vres(1, i)
                orole.level = vres(2, i)
                orole.inactive = vres(3, i)
                orole.certificate = MyBase.certificate
                orole.db_read()
                Me.roles.Add(orole)
            Next
        End If
        REM read offices
        vres = gdb.read_offices(MyBase.certificate)
        If IsArray(vres) Then
            For i = 0 To UBound(vres, 2)
                ooffice = New bc_om_user_office
                ooffice.id = vres(0, i)
                ooffice.description = vres(1, i)
                ooffice.inactive = vres(2, i)
                Me.offices.Add(ooffice)
            Next
        End If
        REM read bus areas
        vres = gdb.read_bus_areas(MyBase.certificate)
        If IsArray(vres) Then
            For i = 0 To UBound(vres, 2)
                obus_area = New bc_om_bus_area
                obus_area.id = vres(0, i)
                obus_area.description = vres(1, i)
                obus_area.inactive = vres(2, i)
                Me.business_areas.Add(obus_area)
            Next
        End If
        vres = gdb.read_languages(MyBase.certificate)
        If IsArray(vres) Then
            For i = 0 To UBound(vres, 2)
                olang = New bc_om_user_language
                olang.language_id = vres(0, i)
                olang.language_name = vres(1, i)
                olang.inactive = vres(2, i)
                Me.languages.Add(olang)
            Next
        End If
        vres = gdb.read_person_bus_areas(MyBase.certificate)
        If IsArray(vres) Then
            For i = 0 To UBound(vres, 2)
                opbus = New bc_om_user_bus_area
                opbus.user_id = vres(0, i)
                opbus.bus_area_id = vres(1, i)
                Me.user_bus_areas.Add(opbus)
            Next
        End If


        Me.sync_types.sync_types.Clear()
        Dim ost As bc_om_sync_type

        vres = gdb.read_all_sync_types(MyBase.certificate)
        If IsArray(vres) Then
            For i = 0 To UBound(vres, 2)
                ost = New bc_om_sync_type
                ost.id = vres(0, i)
                ost.name = vres(1, i)
                Me.sync_types.sync_types.Add(ost)
            Next
        End If

        'Dim opref As bc_om_user_pref
        'vres = gdb.read_all_prefs(MyBase.certificate)
        'If IsArray(vres) Then
        '    For i = 0 To UBound(vres, 2)
        '        opref = New bc_om_user_pref
        '        opref.entity_id = vres(0, i)
        '        opref.class_id = vres(1, i)
        '        opref.rating = vres(2, i)
        '        opref.user_id = vres(3, i)
        '        Me.user_prefs.Add(opref)
        '    Next
        'End If

    End Sub

    Public Sub New()

    End Sub
End Class
<Serializable()> Public Class bc_om_set_user_prefs
    Public delete_entity_pref_list As New ArrayList
    Public prefs As New ArrayList
End Class
<Serializable()> Public Class bc_om_all_prefs
    Inherits bc_cs_soap_base_class
    Public user_prefs As New ArrayList
    Public Overrides Sub process_object()
        Select Case MyBase.tmode
            Case bc_cs_soap_base_class.tREAD
                db_read()
            Case bc_cs_soap_base_class.tREAD

        End Select
    End Sub
    REM FIL July 2012
    Public Sub db_read()
        Dim gdb As New bc_om_user_db
        Dim vres As Object
        Me.user_prefs.Clear()
        Dim opref As bc_om_user_pref
        vres = gdb.read_all_prefs(MyBase.certificate)
        If IsArray(vres) Then
            For i = 0 To UBound(vres, 2)
                opref = New bc_om_user_pref
                opref.entity_id = vres(0, i)
                opref.class_id = vres(1, i)
                opref.rating = vres(2, i)
                opref.user_id = vres(3, i)
                opref.entity_name = vres(4, i)
                opref.class_name = vres(5, i)
                opref.user_name = vres(6, i)
                opref.inactive = vres(7, i)
                opref.pref_type = vres(8, i)
                opref.pref_name = vres(9, i)
                Me.user_prefs.Add(opref)
            Next
        End If
    End Sub

End Class
<Serializable()> Public Class bc_om_user_language
    Inherits bc_cs_soap_base_class
    Public language_id As Long
    Public language_name As String
    Public write_mode As Integer = 0
    Public delete_error As String = ""
    Public inactive As Boolean = False

    Public Const INSERT = 0
    Public Const UPDATE = 1
    Public Const DELETE = 2
    Public Const SET_ACTIVE = 3
    Public Const SET_INACTIVE = 4
    Public Sub New()

    End Sub
    Public Overrides Sub process_object()
        Select Case Me.tmode
            Case tWRITE
                db_write()
        End Select
    End Sub
    Public Sub db_write()
        Dim gdb As New bc_om_user_db
        Dim vres As Object
        Dim tname As String
        tname = Me.language_name
        If bc_cs_central_settings.server_flag = 0 Then
            certificate.user_id = bc_cs_central_settings.logged_on_user_id
        End If
        Dim fs As New bc_cs_string_services(Me.language_name)
        Me.language_name = fs.delimit_apostrophies
        Select Case write_mode
            Case INSERT
                vres = gdb.add_language(language_name, MyBase.certificate)
                If IsArray(vres) Then
                    Me.language_id = vres(0, 0)
                End If
            Case UPDATE
                gdb.update_language(language_id, language_name, MyBase.certificate)
            Case DELETE
                delete_error = gdb.delete_language(language_id, MyBase.certificate)
            Case SET_ACTIVE
                gdb.set_language_active(False, language_id, MyBase.certificate)
            Case SET_INACTIVE
                gdb.set_language_active(True, language_id, MyBase.certificate)
        End Select
        Me.language_name = tname

    End Sub
End Class
<Serializable()> Public Class bc_om_stage_role_access
    Inherits bc_cs_soap_base_class
    Public stage_id As Long
    Public stage_name As String
    Public access_id As String
    Public role_id As Long
    Public del_flag As Boolean = False
    Public Sub New()

    End Sub
    Public Overrides Sub process_object()
        Select Case tmode
            Case bc_cs_soap_base_class.tWRITE
                db_write()
        End Select
    End Sub
    Public Sub db_write()
        Dim gdb As New bc_om_user_db
        If del_flag = False Then
            gdb.add_stage_role_access(stage_name, access_id, role_id, MyBase.certificate)
        Else
            gdb.del_stage_role_access(stage_name, access_id, role_id, MyBase.certificate)
        End If
    End Sub
End Class

<Serializable()> Public Class bc_om_user_role
    Inherits bc_cs_soap_base_class
    Public id As Long
    Public description As String
    Public level As Integer
    Public write_mode As Integer = 0
    Public delete_error As String = ""
    Public apps As New ArrayList
    Public inactive As Boolean
    Public stage_role_access As New ArrayList
    Public Const INSERT = 0
    Public Const UPDATE = 1
    Public Const DELETE = 2
    Public Const SET_ACTIVE = 3
    Public Const SET_INACTIVE = 4

    Public Sub New()

    End Sub
    Public Overrides Sub process_object()
        Select Case Me.tmode
            Case tWRITE
                db_write()
        End Select
    End Sub
    Public Sub db_read()
        Dim gdb As New bc_om_user_db
        Dim vres As Object
        Me.apps.Clear()
        vres = gdb.read_apps_for_role(Me.id, MyBase.certificate)
        If IsArray(vres) Then
            For i = 0 To UBound(vres, 2)
                Me.apps.Add(vres(0, i))
            Next
        End If
        vres = gdb.read_stage_access_for_role(Me.id, MyBase.certificate)
        If IsArray(vres) Then
            For i = 0 To UBound(vres, 2)
                Dim osr As New bc_om_stage_role_access
                osr.stage_id = vres(0, i)
                osr.stage_name = vres(1, i)
                osr.access_id = vres(2, i)
                Me.stage_role_access.Add(osr)
            Next
        End If

    End Sub
    Public Sub db_write()
        Dim gdb As New bc_om_user_db
        Dim vres As Object
        Dim tname As String
        tname = Me.description
        If bc_cs_central_settings.server_flag = 0 Then
            certificate.user_id = bc_cs_central_settings.logged_on_user_id
        End If
        Dim fs As New bc_cs_string_services(Me.description)
        Me.description = fs.delimit_apostrophies
        Select Case write_mode
            Case INSERT
                vres = gdb.add_role(description, Me.level, MyBase.certificate)
                If IsArray(vres) Then
                    Me.id = vres(0, 0)
                End If
                REM now set up all applications as default
                gdb.assign_all_apps_to_role(id, MyBase.certificate)
                REM assign to object
                vres = gdb.read_installed_apps(MyBase.certificate)
                If IsArray(vres) Then
                    For i = 0 To UBound(vres, 2)
                        Me.apps.Add(vres(0, i))
                    Next
                End If

            Case UPDATE
                gdb.update_role(id, description, level, MyBase.certificate)
                gdb.delete_apps_for_role(id, MyBase.certificate)
                For i = 0 To Me.apps.Count - 1
                    gdb.add_app_for_role(id, Me.apps(i), MyBase.certificate)
                Next
            Case DELETE
                delete_error = gdb.delete_role(id, MyBase.certificate)
            Case SET_ACTIVE
                gdb.set_role_active(False, id, MyBase.certificate)

            Case SET_INACTIVE
                gdb.set_role_active(True, id, MyBase.certificate)

        End Select
        Me.description = tname
    End Sub
End Class
<Serializable()> Public Class bc_om_user_office
    Inherits bc_cs_soap_base_class
    Public id As Long
    Public description As String
    Public write_mode As Integer = 0
    Public inactive As Boolean = False
    Public delete_error As String = ""
    Public Const INSERT = 0
    Public Const UPDATE = 1
    Public Const DELETE = 2
    Public Const SET_ACTIVE = 3
    Public Const SET_INACTIVE = 4
    Public Sub New()

    End Sub
    Public Overrides Sub process_object()
        Select Case Me.tmode
            Case tWRITE
                db_write()
        End Select
    End Sub
    Public Sub db_write()
        Dim gdb As New bc_om_user_db
        Dim vres As Object
        Dim tname As String
        tname = Me.description
        If bc_cs_central_settings.server_flag = 0 Then
            certificate.user_id = bc_cs_central_settings.logged_on_user_id
        End If
        Dim fs As New bc_cs_string_services(Me.description)
        Me.description = fs.delimit_apostrophies
        Select Case write_mode
            Case INSERT
                vres = gdb.add_office(description, MyBase.certificate)
                If IsArray(vres) Then
                    Me.id = vres(0, 0)
                End If
            Case UPDATE
                gdb.update_office(id, description, MyBase.certificate)
            Case DELETE
                delete_error = gdb.delete_office(id, MyBase.certificate)
            Case SET_ACTIVE
                gdb.set_office_active(False, id, MyBase.certificate)

            Case SET_INACTIVE
                gdb.set_office_active(True, id, MyBase.certificate)
        End Select
        Me.description = tname
    End Sub
End Class
<Serializable()> Public Class bc_om_bus_area
    Inherits bc_cs_soap_base_class
    Public id As Long
    Public description As String
    Public write_mode As Integer = 0
    Public inactive As Boolean = False

    Public delete_error As String = ""
    Public Const INSERT = 0
    Public Const UPDATE = 1
    Public Const DELETE = 2
    Public Const SET_ACTIVE = 3
    Public Const SET_INACTIVE = 4

    Public Sub New()

    End Sub
    Public Overrides Sub process_object()
        Select Case Me.tmode
            Case tWRITE
                db_write()
        End Select
    End Sub
    Public Sub db_write()
        Dim gdb As New bc_om_user_db
        Dim vres As Object
        Dim tname As String
        tname = Me.description
        Dim fs As New bc_cs_string_services(Me.description)
        Me.description = fs.delimit_apostrophies
        If bc_cs_central_settings.server_flag = 0 Then
            certificate.user_id = bc_cs_central_settings.logged_on_user_id
        End If
        Select Case write_mode
            Case INSERT
                vres = gdb.add_bus_area(description, MyBase.certificate)
                If IsArray(vres) Then
                    Me.id = vres(0, 0)
                End If
            Case UPDATE
                gdb.update_bus_area(id, description, MyBase.certificate)
            Case DELETE
                delete_error = gdb.delete_bus_area(id, MyBase.certificate)
            Case SET_ACTIVE
                gdb.set_bus_area_active(False, Me.id, MyBase.certificate)
            Case SET_INACTIVE
                gdb.set_bus_area_active(True, Me.id, MyBase.certificate)
        End Select
        Me.description = tname
    End Sub
End Class
Public Enum process_client_events
    SYNC = 1
    ASYNC_CURRENT_USER = 2
    ASYNC_SERVER_USER = 3
    ASYNC_SERVER = 4
End Enum
REM FIL JUN 2013
<Serializable()> Public Class bc_om_user_prefs
    Inherits bc_cs_soap_base_class

    REM Public user_id As Long
    Public pref As New ArrayList
    Public language_strings As New bc_om_user_language_strings
    Public bus_areas As New ArrayList
    Public stage_role_access_codes As New ArrayList
    Public picture As Byte()
    Public financial_workflow_stages As New ArrayList
    Public default_financial_workflow_stage As Long
    Public document_protection_password As String
    Public secondary_roles As New ArrayList

    Public process_events_mode As process_client_events
    Public create_settings As bc_om_create_settings

    REM FIL JUNE 2013
    <Serializable()> Public Class bc_om_create_settings

        Public run_time_pref_list As Boolean = False
        Public show_all_entities As Boolean = False
        Public my_entities_default As Boolean = True

        REM Steve Wooderson Drexel 27/01/2014 Show prefs only
        Public show_all_entities_submit As Boolean = False
        Public my_entities_default_submit As Boolean = True

        Public alt_entity_for_build As Boolean = False
        Public alt_entity_for_apref As Boolean = False
        Public alt_entity_for_submit As Boolean = False
        Public merges As New ArrayList
        Public custom_library As String
        Public use_advanced_composite_build As Boolean = False
        Public search_pub_type_list As New ArrayList
        Public document_scan_namespace As String
        Public document_scan_node As String

        Public show_summary_text As Boolean = True
        Public show_teaser_text As Boolean = True
        Public summary_text_display_name As String = ""
        Public teaser_text_display_name As String = ""
        Public allow_support_import As Boolean = False
        Public deny_backdated_submission As Boolean = False
        Public show_keep_open_check_out As Boolean = False
        Public Const CREATE_CONFIG_FILE = "bc_am_create_config.xml"


        Public Sub db_read()

            readcreateConfigFile()

        End Sub
        Private Sub readcreateConfigFile()

            Dim xmlload As New Xml.XmlDocument
            Dim myXmlNodeList As Xml.XmlNodeList
            Dim myxmlnode As Xml.XmlNode
            Dim opubtypemerge As bc_cs_central_settings.pub_type_merges


            Dim i As Integer
            Try

                REM FIL NOV 2013
                xmlload.Load(bc_cs_central_settings.central_template_path + CREATE_CONFIG_FILE)

                myXmlNodeList = xmlload.SelectNodes("/create_settings/show_all_entities")
                If myXmlNodeList.Count > 0 Then
                    show_all_entities = xmlload.SelectSingleNode("/create_settings/show_all_entities").InnerXml()
                End If
                myXmlNodeList = xmlload.SelectNodes("/create_settings/my_entities_default")
                If myXmlNodeList.Count > 0 Then
                    my_entities_default = xmlload.SelectSingleNode("/create_settings/my_entities_default").InnerXml()
                End If

                REM Steve Wooderson Drexel 27/01/2014 Show prefs only
                myXmlNodeList = xmlload.SelectNodes("/create_settings/show_all_entities_submit")
                If myXmlNodeList.Count > 0 Then
                    show_all_entities_submit = xmlload.SelectSingleNode("/create_settings/show_all_entities_submit").InnerXml()
                End If
                myXmlNodeList = xmlload.SelectNodes("/create_settings/my_entities_default_submit")
                If myXmlNodeList.Count > 0 Then
                    my_entities_default_submit = xmlload.SelectSingleNode("/create_settings/my_entities_default_submit").InnerXml()
                End If

                myXmlNodeList = xmlload.SelectNodes("/create_settings/alt_entity_for_build")
                If myXmlNodeList.Count > 0 Then
                    alt_entity_for_build = xmlload.SelectSingleNode("/create_settings/alt_entity_for_build").InnerXml()
                End If
                myXmlNodeList = xmlload.SelectNodes("/create_settings/alt_entity_for_apref")
                If myXmlNodeList.Count > 0 Then
                    alt_entity_for_apref = xmlload.SelectSingleNode("/create_settings/alt_entity_for_apref").InnerXml()
                End If
                myXmlNodeList = xmlload.SelectNodes("/create_settings/alt_entity_for_submit")
                If myXmlNodeList.Count > 0 Then
                    alt_entity_for_submit = xmlload.SelectSingleNode("/create_settings/alt_entity_for_submit").InnerXml()
                End If
                merges.Clear()
                myXmlNodeList = xmlload.SelectNodes("/create_settings/custom_library/pub_types/id")
                i = 0
                For Each myxmlnode In myXmlNodeList
                    opubtypemerge = New bc_cs_central_settings.pub_type_merges
                    opubtypemerge.pub_type_id = myxmlnode.InnerXml
                    merges.Add(opubtypemerge)
                    i = i + 1
                Next
                myXmlNodeList = xmlload.SelectNodes("/create_settings/custom_library/pub_types/merge_name")
                i = 0
                For Each myxmlnode In myXmlNodeList
                    merges(i).merge_name = myxmlnode.InnerXml
                    i = i + 1
                Next
                myXmlNodeList = xmlload.SelectNodes("/create_settings/custom_library/pub_types/bookmark_name")
                i = 0
                For Each myxmlnode In myXmlNodeList
                    merges(i).bookmark_name = myxmlnode.InnerXml
                    i = i + 1
                Next

                myXmlNodeList = xmlload.SelectNodes("/create_settings/custom_library/name")
                If myXmlNodeList.Count > 0 Then
                    custom_library = xmlload.SelectSingleNode("/create_settings/custom_library/name").InnerXml()
                End If

                myXmlNodeList = xmlload.SelectNodes("/create_settings/adv_composite_build")
                If myXmlNodeList.Count > 0 Then
                    use_advanced_composite_build = xmlload.SelectSingleNode("/create_settings/adv_composite_build").InnerXml()
                End If

                myXmlNodeList = xmlload.SelectNodes("/create_settings/adv_composite_search/pub_type_name")
                search_pub_type_list.Clear()
                For Each myxmlnode In myXmlNodeList
                    search_pub_type_list.Add(myxmlnode.InnerXml())
                Next

                myXmlNodeList = xmlload.SelectNodes("/create_settings/document_scan_namespace")
                If myXmlNodeList.Count > 0 Then
                    document_scan_namespace = xmlload.SelectSingleNode("/create_settings/document_scan_namespace").InnerXml()
                End If

                myXmlNodeList = xmlload.SelectNodes("/create_settings/document_scan_node")
                If myXmlNodeList.Count > 0 Then
                    document_scan_node = xmlload.SelectSingleNode("/create_settings/document_scan_node").InnerXml()
                End If

                myXmlNodeList = xmlload.SelectNodes("/create_settings/document_scan_node")
                If myXmlNodeList.Count > 0 Then
                    document_scan_node = xmlload.SelectSingleNode("/create_settings/document_scan_node").InnerXml()
                End If

                myXmlNodeList = xmlload.SelectNodes("/create_settings/deny_backdated_submission")
                If myXmlNodeList.Count > 0 Then
                    deny_backdated_submission = xmlload.SelectSingleNode("/create_settings/deny_backdated_submission").InnerXml()
                End If

                myXmlNodeList = xmlload.SelectNodes("/create_settings/show_summary_text")
                If myXmlNodeList.Count > 0 Then
                    show_summary_text = xmlload.SelectSingleNode("/create_settings/show_summary_text").InnerXml()
                End If
                myXmlNodeList = xmlload.SelectNodes("/create_settings/show_teaser_text")
                If myXmlNodeList.Count > 0 Then
                    show_teaser_text = xmlload.SelectSingleNode("/create_settings/show_teaser_text").InnerXml()
                End If
                myXmlNodeList = xmlload.SelectNodes("/create_settings/summary_text_display_name")
                If myXmlNodeList.Count > 0 Then
                    summary_text_display_name = xmlload.SelectSingleNode("/create_settings/summary_text_display_name").InnerXml()
                End If
                myXmlNodeList = xmlload.SelectNodes("/create_settings/teaser_text_display_name")
                If myXmlNodeList.Count > 0 Then
                    teaser_text_display_name = xmlload.SelectSingleNode("/create_settings/teaser_text_display_name").InnerXml()
                End If

                REM FIL JIRA 6835
                myXmlNodeList = xmlload.SelectNodes("/create_settings/allow_support_import")

                If myXmlNodeList.Count > 0 Then
                    allow_support_import = xmlload.SelectSingleNode("/create_settings/allow_support_import").InnerXml()
                End If

                REM FIL JIRA 4913
                myXmlNodeList = xmlload.SelectNodes("/create_settings/show_keep_open_check_out")

                If myXmlNodeList.Count > 0 Then
                    show_keep_open_check_out = xmlload.SelectSingleNode("/create_settings/show_keep_open_check_out").InnerXml()
                End If
                myXmlNodeList = xmlload.SelectNodes("/create_settings/run_time_pref_list")
                If myXmlNodeList.Count > 0 Then
                    run_time_pref_list = xmlload.SelectSingleNode("/create_settings/run_time_pref_list").InnerXml()
                End If

            Catch ex As Exception
                Dim slog As New bc_cs_activity_log("Error Reading Create Config File", "readConfigFile", CStr(bc_cs_activity_codes.COMMENTARY), ex.Message)
            End Try
        End Sub
    End Class

    Public Overrides Sub process_object()
        Dim otrace As New bc_cs_activity_log("bc_om_user_prefs", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            REM this is always specific to object
            If Me.tmode = bc_cs_soap_base_class.tREAD Then
                db_read()

            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_user_prefs", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_user_prefs", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub
    Public Sub db_read()
        Dim otrace As New bc_cs_activity_log("bc_om_user_prefs", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "", MyBase.certificate)
        Try
            Dim i As Integer
            Dim opref As bc_om_user_pref
            Dim db_pref As New bc_om_pref_db
            Dim vprefs As Object
            Dim vbusareas As Object

            create_settings = New bc_om_create_settings
            create_settings.db_read()

            pref.Clear()
            bus_areas.Clear()
            stage_role_access_codes.Clear()

            Dim luserid As String
            If bc_cs_central_settings.server_flag = 0 Then
                luserid = bc_cs_central_settings.logged_on_user_id
            Else
                luserid = certificate.user_id
            End If

            vprefs = db_pref.read_pref_for_user(luserid, certificate)
            If IsArray(vprefs) Then
                For i = 0 To UBound(vprefs, 2)
                    opref = New bc_om_user_pref(vprefs(0, i))
                    opref.entity_name = vprefs(1, i)
                    opref.inactive = vprefs(2, i)
                    opref.class_id = vprefs(3, i)
                    opref.class_name = vprefs(4, i)

                    opref.certificate = MyBase.certificate
                    pref.Add(opref)
                Next
            End If
            REM language dependnet strings 
            language_strings.certificate = MyBase.certificate
            language_strings.db_read()
            REM business areas
            vbusareas = db_pref.get_bus_areas(luserid, MyBase.certificate)
            If IsArray(vbusareas) Then
                For i = 0 To UBound(vbusareas, 2)
                    bus_areas.Add(vbusareas(0, i))
                Next
            End If
            REM load stage role access
            Dim vaccess As Object
            Dim oaccess As stage_role_access
            vaccess = db_pref.get_stage_role_access(luserid, MyBase.certificate)
            If IsArray(vprefs) Then
                For i = 0 To UBound(vaccess, 2)
                    oaccess = New stage_role_access
                    oaccess.stage_id = vaccess(0, i)
                    oaccess.stage_name = vaccess(1, i)
                    oaccess.access_id = vaccess(2, i)
                    Me.stage_role_access_codes.Add(oaccess)
                Next
            End If
            REM financial workflow stages
            Dim vfroles As Object
            financial_workflow_stages.Clear()
            vfroles = db_pref.get_financial_workflow_stage(luserid, MyBase.certificate)
            If IsArray(vfroles) Then
                For i = 0 To UBound(vfroles, 2)
                    If IsNumeric(vfroles(0, 0)) = 0 Then
                        If CStr(vfroles(0, 0) = "Error") Then
                            Dim ocommentary As New bc_cs_activity_log("bc_om_user_prefs", "db_read", bc_cs_activity_codes.COMMENTARY, "Role based financial workflow not installed", Me.certificate)
                            Exit For
                        End If
                    End If
                    financial_workflow_stages.Add(vfroles(0, i))
                    If vfroles(1, i) = True Then
                        Me.default_financial_workflow_stage = vfroles(0, i)
                    End If
                Next
            End If
            Dim vpw As Object
            Me.document_protection_password = ""
            vpw = db_pref.get_document_protection_password(MyBase.certificate)
            If IsArray(vpw) Then
                For i = 0 To UBound(vpw, 2)
                    Me.document_protection_password = vpw(0, 0)
                Next
            End If



            REM SIM MAY 2013
            Dim vroles As Object
            Me.secondary_roles.Clear()
            vroles = db_pref.get_secondary_user_roles(certificate)
            If IsArray(vroles) Then
                For i = 0 To UBound(vroles, 2)
                    Me.secondary_roles.Add(vroles(0, i))
                Next
            End If

            REM =============
            REM async process APR 2012
            Me.process_events_mode = process_client_events.SYNC
            'Dim vres As Object
            'Me.process_events_mode = process_client_events.SYNC
            'vres = db_pref.get_events_mode(luserid, MyBase.certificate)
            'If IsArray(vres) Then
            '    Me.process_events_mode = vres(0, 0)
            'End If
            REM =============

            REM attempt to load analyst picture
            Dim fs As New bc_cs_file_transfer_services
            'If fs.check_document_exists(bc_cs_central_settings.central_repos_path + "user_" + CStr(luserid) + ".bmp", Me.certificate) Then
            'fs.write_document_to_bytestream(bc_cs_central_settings.central_repos_path + "user_" + CStr(luserid) + ".bmp", Me.picture, MyBase.certificate, False)
            'Else
            '   Dim ocommentary As New bc_cs_activity_log("bc_om_user_prefs", "db_read", bc_cs_activity_codes.COMMENTARY, "Cant find users picture: " + bc_cs_central_settings.central_repos_path + "user_" + CStr(luserid) + ".bmp", Me.certificate)
            'End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_user_prefs", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, MyBase.certificate)

        Finally
            otrace = New bc_cs_activity_log("bc_om_user_prefs", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", MyBase.certificate)
        End Try
    End Sub
    'Public Overloads Overrides Function call_web_service() As String
    '    Dim otrace As New bc_cs_activity_log("bc_om_user_prefs", "call_web_service", bc_cs_activity_codes.TRACE_ENTRY, "", MyBase.certificate)
    '    If IsNumeric(bc_cs_central_settings.timeout) Then
    '        webservice.Timeout = CLng(bc_cs_central_settings.timeout)
    '        Dim ocommentary As New bc_cs_activity_log("bc_om_web services", "call_web_service", bc_cs_activity_codes.COMMENTARY, "Setting Web Service Timeout to: " + bc_cs_central_settings.timeout, MyBase.certificate)
    '    End If
    '
    '        call_web_service = webservice.LoadUserPrefs
    '        otrace = New bc_cs_activity_log("bc_om_user_prefs", "call_web_service", bc_cs_activity_codes.TRACE_EXIT, "", MyBase.certificate)
    '    End Function

    Public Sub New()

    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
    <Serializable()> Private Class stage_role_access
        Public stage_id As Long
        Public stage_name As String
        Public access_id As String

        Public Sub New()

        End Sub
    End Class
End Class
<Serializable()> Public Class bc_om_user_pref
    Inherits bc_cs_soap_base_class
    Public entity_id As Long
    Public class_id As Long
    Public user_id As Long
    Public rating As Integer
    Public class_name As String
    Public entity_name As String
    Public user_name As String
    Public inactive As Integer
    REM FIL JULY 2012
    Public pref_type As Long
    Public pref_name As String
    Public users As New List(Of bc_om_user)


    Public Sub New()

    End Sub
    Public Sub New(ByVal ientity_id As Long)
        entity_id = ientity_id
    End Sub

End Class
REM ===============================================
REM Bluecurve Limited 2005
REM Module:       Create User lanaguage
REM Type:         Object Model
REM Description:  Users
REM               User_laguage_strings
REM               holds system strings for users preferred language
REM Version:      1
REM Change history
REM ===============================================
<Serializable()> Public Class bc_om_user_language_strings
    Inherits bc_cs_soap_base_class
    Public forms As New ArrayList


    Public Sub New()

    End Sub
    Public Sub db_read()
        Dim otrace As New bc_cs_activity_log("bc_om_user_language_strings", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "", MyBase.certificate)
        Try
            Dim i As Integer
            Dim oform As bc_om_user_form
            Dim db_pref As New bc_om_pref_db
            Dim vforms As Object
            vforms = db_pref.get_application_forms(MyBase.certificate)
            If IsArray(vforms) Then
                For i = 0 To UBound(vforms, 2)
                    oform = New bc_om_user_form(vforms(0, i))
                    oform.certificate = MyBase.certificate
                    oform.db_read(vforms(0, i))
                    forms.Add(oform)
                Next
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_user_language_strings", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, MyBase.certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_user_language_strings", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", MyBase.certificate)
        End Try
    End Sub
End Class
<Serializable()> Public Class bc_om_user_form
    Inherits bc_cs_soap_base_class
    Public form_name As String
    Public form_labels As New ArrayList
    Public Sub New()

    End Sub
    Public Sub New(ByVal form_name As String)
        Me.form_name = form_name
    End Sub
    Public Shadows Sub db_read(ByVal form_name As String)
        Dim otrace As New bc_cs_activity_log("bc_om_user_form", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "", MyBase.certificate)
        Try
            Dim i As Integer
            Dim oform_label As bc_om_user_form_label
            Dim db_pref As New bc_om_pref_db
            Dim vform_labels As Object
            vform_labels = db_pref.get_application_form_labels(form_name, MyBase.certificate)

            If IsArray(vform_labels) Then
                For i = 0 To UBound(vform_labels, 2)
                    oform_label = New bc_om_user_form_label(vform_labels(0, i), vform_labels(1, i))
                    oform_label.certificate = MyBase.certificate
                    form_labels.Add(oform_label)
                Next
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_user_form", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, MyBase.certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_user_form", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", MyBase.certificate)
        End Try
    End Sub
End Class
<Serializable()> Public Class bc_om_user_form_label
    Inherits bc_cs_soap_base_class
    Public label_code As String
    Public text As String
    Public Sub New(ByVal label_code As String, ByVal text As String)
        Me.label_code = label_code
        Me.text = text
    End Sub
End Class
REM =========================================================================
REM Database interaction layer
REM =========================================================================
Public Class bc_om_user_db
    Private gbc_db As New bc_cs_db_services(False)

    REM FIL JUNE 2013
    Public Function get_audit_id(ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "exec dbo.bc_core_cp_get_audit_id"
        get_audit_id = gbc_db.executesql(sql, certificate)
    End Function
    Public Sub clear_user_display_attributes(ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "exec dbo.bc_core_cp_delete_user_display_attributes"
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Sub add_user_display_attribute(ByVal name As String, ByVal ord As Integer, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "exec dbo.bc_core_cp_add_user_display_attribute '" + name + "'," + CStr(ord)

        gbc_db.executesql(sql, certificate)
    End Sub

    Public Function create_new_user(ByVal first_name As String, ByVal surname As String, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        Dim fs As New bc_cs_string_services(first_name)
        first_name = fs.delimit_apostrophies
        fs = New bc_cs_string_services(surname)
        surname = fs.delimit_apostrophies



        sql = "exec dbo.bc_core_create_new_user '" + first_name + "','" + surname + "'"
        create_new_user = gbc_db.executesql(sql, certificate)
    End Function
    Public Function read_display_attributes_for_user(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "exec dbo.bc_core_cp_get_user_display_attributes"
        read_display_attributes_for_user = gbc_db.executesql(sql, certificate)
    End Function
    Public Function read_system_attributes_for_user(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "exec dbo.bc_core_cp_get_user_system_attributes"
        read_system_attributes_for_user = gbc_db.executesql(sql, certificate)
    End Function

    REM reads all pub type in database
    Public Function test_connection() As Boolean
        test_connection = False
        Dim sql As String
        sql = "select count(*) from user_table"
        If gbc_db.test_sql(sql, Nothing) = "" Then
            test_connection = True
        End If
    End Function
    Public Sub audit_entity_action(entity_id As Long, comment As String, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        Dim sr As New bc_cs_string_services(comment)
        comment = sr.delimit_apostrophies
        Dim user_id As Long
        If bc_cs_central_settings.server_flag = 0 Then
            user_id = bc_cs_central_settings.logged_on_user_id
        Else
            user_id = certificate.user_id
        End If
        sql = "exec dbo.bc_core_add_entity_user_audit " + CStr(entity_id) + ",0,'" + comment + "'," + CStr(user_id)
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Sub set_default_attribute_values(user_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        Dim change_user_id As Long
        If bc_cs_central_settings.server_flag = 0 Then
            change_user_id = bc_cs_central_settings.logged_on_user_id
        Else
            change_user_id = certificate.user_id
        End If
        sql = "exec dbo.bc_core_cp_set_def_att_values " + CStr(user_id) + "," + CStr(change_user_id) + ",0"
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Function read_bus_areas_for_user(ByVal user_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select p.bus_area_id from person_bus_area_tbl p, bus_area_table b where b.bus_area_id=p.bus_area_id and p.user_id=" + CStr(user_id) + " and coalesce(deleted,0) =0"
        read_bus_areas_for_user = gbc_db.executesql(sql, certificate)
    End Function
    Public Sub delete_prefs_for_user(audit_id As Long, ByVal user_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        Dim change_user_id As Long
        If bc_cs_central_settings.server_flag = 0 Then
            change_user_id = bc_cs_central_settings.logged_on_user_id
        Else
            change_user_id = certificate.user_id
        End If
        sql = "exec dbo.bc_core_delete_user_prefs_from_user " + CStr(audit_id) + "," + CStr(user_id)
        'sql = "delete from apref_entity_tbl where user_id=" + CStr(user_id)
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Sub delete_prefs_for_entity(audit_id As Long, ByVal entity_id As Long, ByVal pref_type_id As Integer, user_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        Dim change_user_id As Long
        If bc_cs_central_settings.server_flag = 0 Then
            change_user_id = bc_cs_central_settings.logged_on_user_id
        Else
            change_user_id = certificate.user_id
        End If
        sql = "exec dbo.bc_core_delete_entity_prefs_from_user " + CStr(audit_id) + "," + CStr(entity_id) + "," + CStr(pref_type_id) + "," + CStr(user_id) + "," + CStr(change_user_id)
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Sub insert_prefs_for_entity(audit_id As Long, ByVal entity_id As Long, user_id As Long, rating As Integer, ByVal pref_type_id As Integer, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        Dim change_user_id As Long
        If bc_cs_central_settings.server_flag = 0 Then
            change_user_id = bc_cs_central_settings.logged_on_user_id
        Else
            change_user_id = certificate.user_id
        End If
        sql = "exec dbo.bc_core_add_user_pref " + CStr(audit_id) + "," + CStr(user_id) + "," + CStr(entity_id) + "," + CStr(rating) + "," + CStr(pref_type_id) + "," + CStr(change_user_id)
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Sub delete_prefs_for_entity(audit_id As Long, ByVal entity_id As Long, ByVal pref_type As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "delete from apref_entity_tbl where entity_id=" + CStr(audit_id) + "," + CStr(entity_id) + " and preference_type_id=" + CStr(pref_type)
        gbc_db.executesql(sql, certificate)
    End Sub
    'Public Sub add_user_pref(ByVal user_id As Long, ByVal entity_id As Long, ByVal rating As Integer, ByVal pref_type As Long, ByRef certificate As bc_cs_security.certificate)
    '    Dim sql As String
    '    Dim change_user_id As Long
    '    If bc_cs_central_settings.server_flag = 0 Then
    '        change_user_id = bc_cs_central_settings.logged_on_user_id
    '    Else
    '        change_user_id = certificate.user_id
    '    End If
    '    sql = "exec dbo.bc_core_add_user_pref_from_user " + CStr(user_id) + "," + CStr(entity_id) + "," + CStr(rating) + "," + CStr(pref_type) + "," + CStr(change_user_id)
    '    'sql = "insert into apref_entity_tbl(user_id,entity_id,rating,date_from,preference_type_id) values(" + CStr(user_id) + "," + CStr(entity_id) + "," + CStr(rating) + ", getdate()," + CStr(pref_type) + ")"
    '    gbc_db.executesql(sql, certificate)
    'End Sub
    REM FIL 5.5
    'Public Sub update_user_pref(ByVal user_id As Long, ByVal entity_id As Long, ByVal rating As Integer, ByVal pref_type As Long, ByRef certificate As bc_cs_security.certificate)
    '    Dim sql As String
    '    Dim change_user_id As Long
    '    If bc_cs_central_settings.server_flag = 0 Then
    '        change_user_id = bc_cs_central_settings.logged_on_user_id
    '    Else
    '        change_user_id = certificate.user_id
    '    End If
    '    sql = "update apref_entity_tbl set rating=" + CStr(rating) + " where entity_id=" + CStr(entity_id) + " and user_id=" + CStr(user_id) + " and preference_type_id=" + CStr(pref_type)
    '    gbc_db.executesql(sql, certificate)
    'End Sub
    Public Sub add_stage_role_access(ByVal stage_name As String, ByVal access_id As String, ByVal role_id As String, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "exec dbo.bc_core_cp_upd_st_role_access '" + stage_name + "','" + access_id + "','" + CStr(role_id) + "',0"
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Sub del_stage_role_access(ByVal stage_name As String, ByVal access_id As String, ByVal role_id As String, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "exec dbo.bc_core_cp_upd_st_role_access '" + stage_name + "','" + access_id + "','" + CStr(role_id) + "',1"
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Function read_prefs_for_user(ByVal user_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        REM FIL 5.4
        sql = "exec dbo.bc_core_prefs_for_user " + CStr(user_id)
        'sql = "select e.entity_id, e.class_id, a.rating,a.preference_type_id,p.preference_type_name,e.name, coalesce(e.inactive,0)  from apref_entity_tbl a " + _
        '      " inner join entity_tbl e on a.entity_id=e.entity_id " + _
        '      " inner join bc_core_cp_preference_types p on p.preference_type_id =a.preference_type_id" + _
        '      " where a.user_id =" + CStr(user_id) + " and coalesce(e.deleted,0) = 0 order by class_id,e.name asc"
        read_prefs_for_user = gbc_db.executesql(sql, certificate)
    End Function
    Public Function read_installed_apps(ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "select app_id, app_name from bcc_core_cp_installed_apps order by app_name asc"
        read_installed_apps = gbc_db.executesql(sql, certificate)
    End Function
    Public Function read_apps_for_role(ByVal id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "select app_id from bcc_core_cp_apps_for_role where role_id=" + CStr(id) + " order by app_id asc"
        read_apps_for_role = gbc_db.executesql(sql, certificate)
    End Function
    REM FIL JULY 2012
    Public Function read_all_prefs(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select e.entity_id, e.class_id, a.rating,a.user_id,e.name,c.class_name, u.first_name + ' ' + u.surname, coalesce(u.inactive,0)," + _
          " a.preference_type_id,p.preference_type_name from apref_entity_tbl a " + _
          " inner join entity_tbl e on a.entity_id=e.entity_id  and coalesce(e.deleted,0)= 0" + _
          " inner join entity_class_tbl c on c.class_id=e.class_id" + _
          " inner join user_table u on u.user_id = a.user_id and coalesce(u.deleted,0) = 0" + _
          " inner join bc_core_cp_preference_types p on p.preference_type_id=a.preference_type_id " + _
          "order by e.entity_id, rating asc"

        read_all_prefs = gbc_db.executesql(sql, certificate)
    End Function
    Public Function read_user(ByVal id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select u.first_name,u.surname,coalesce(u.middle_name,''),r.role_name,coalesce(u.os_user_name,''),u.language_id,coalesce(u.email,''),coalesce(u.telephone,''),coalesce(u.fax,''), u.office_id,coalesce(u.comment,''),u.role_id,coalesce(change_user_id,0),coalesce(u.user_at_closedown_enable_flag,0), coalesce(u.password,''), coalesce(u.mobile_telephone,''), coalesce(u.job_title,''),coalesce(u.biography,'') ,coalesce(u.web_user_name,''), coalesce(shortcode,'') from user_table u inner join role_tbl r on r.role_id =u.role_id where u.user_id = " + CStr(id)
        read_user = gbc_db.executesql(sql, certificate)
    End Function
    REM FIL 5.5
    Public Sub set_partial_sync(ByVal type As Integer, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        If bc_cs_central_settings.server_flag = 0 Then
            sql = "delete from bc_core_cp_user_sync_items  where sync_id=" + CStr(type) + " and user_id=" + CStr(bc_cs_central_settings.logged_on_user_id)
            gbc_db.executesql(sql, certificate)
            sql = "insert into bc_core_cp_user_sync_items select " + CStr(type) + "," + CStr(bc_cs_central_settings.logged_on_user_id)
            gbc_db.executesql(sql, certificate)
        Else
            sql = "delete from bc_core_cp_user_sync_items  where sync_id=" + CStr(type) + " and user_id=" + CStr(certificate.user_id)
            gbc_db.executesql(sql, certificate)
            sql = "insert into bc_core_cp_user_sync_items select " + CStr(type) + "," + CStr(certificate.user_id)
            gbc_db.executesql(sql, certificate)
        End If
    End Sub

    Public Sub set_sync(ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "update user_table set user_at_closedown_enable_flag=1"
        gbc_db.executesql(sql, certificate)
    End Sub
    REM update sync flag for logged on user
    Public Sub treset_user_sync(ByRef certificate As bc_cs_security.certificate)

        Dim sql As String
        Dim luserid As String
        Dim lusername As String
        If bc_cs_central_settings.server_flag = 0 Then
            luserid = bc_cs_central_settings.logged_on_user_id
            lusername = bc_cs_central_settings.logged_on_user_name
        Else
            luserid = certificate.user_id
            lusername = certificate.name
        End If

        sql = "update user_table set user_at_closedown_enable_flag=0 where user_id=" + CStr(luserid)
        gbc_db.executesql(sql, certificate)

    End Sub
    Public Function evaluate_upgrade_required(ByVal user_id As Long, ByRef release As Integer, ByRef certificate As bc_cs_security.certificate) As Boolean
        Try
            Dim sql As String
            Dim res As Object
            evaluate_upgrade_required = False
            sql = "select a.release_id from user_release u, bc_core_assemblies a where a.server_flag=0 and u.release_id = a.release_id and a.release_date=(select max(release_date) from bc_core_assemblies where server_flag=0) and user_id=" + CStr(user_id)
            res = gbc_db.executesql_show_no_error(sql)
            If Not IsArray(res) Then
                REM get newest release
                sql = "select max(release_id) from bc_core_assemblies"
                res = gbc_db.executesql_show_no_error(sql)
                If IsArray(res) Then
                    release = CInt(res(0, 0))
                    evaluate_upgrade_required = True
                End If
            End If
        Catch

        End Try
    End Function
    Public Function read_assemblies_for_release(ByVal release_id, ByVal certificate) As Object
        Dim sql As String
        sql = "select description,assembly_name, full_storage_path,type, source_safe_label,version from bc_core_assembly where release_id=" + CStr(release_id)
        read_assemblies_for_release = gbc_db.executesql_show_no_error(sql)
    End Function
    Public Function add_role(ByVal desc As String, ByVal level As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "insert into role_tbl (role_id, role_description, role_name, time_out_after_mins, role_level, admin_flag, deleted, comment, inactive, user_id, cp_access_level) select coalesce(max(role_id),0)+1,'" + desc + "','" + desc + "',480," + CStr(level) + ",0,0,convert(varchar(20),getdate()),0,0," + CStr(level) + " from role_tbl"
        gbc_db.executesql(sql, certificate)
        sql = "select max(role_id) from role_tbl"
        add_role = gbc_db.executesql(sql, certificate)
    End Function
    Public Sub update_role(ByVal role_id As Long, ByVal desc As String, ByVal level As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "update role_tbl set role_description='" + desc + "', role_name='" + desc + "', cp_access_level=" + CStr(level) + " where role_id=" + CStr(role_id)
        gbc_db.executesql(sql, certificate)
    End Sub

    Public Function delete_role(ByVal id As Long, ByRef certificate As bc_cs_security.certificate) As String
        Dim sql As String
        sql = "exec dbo.bcc_core_cp_generic_delete 'role'," + CStr(id) + "," + CStr(certificate.user_id)
        delete_role = gbc_db.test_sql(sql, certificate)
    End Function
    Public Sub assign_all_apps_to_role(ByVal id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "insert into bcc_core_cp_apps_for_role (role_id, app_id) select " + CStr(id) + ", app_id from  bcc_core_cp_installed_apps "
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Function read_all_releases(ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "select release_id,server_flag,create_date,release_date,status,description from  bc_core_assemblies order by release_date desc"
        read_all_releases = gbc_db.executesql(sql, certificate)
    End Function
    Public Function read_stage_access_for_role(ByVal id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "exec dbo.bc_core_cp_get_stage_role_access " + CStr(id)
        read_stage_access_for_role = gbc_db.executesql(sql, certificate)
    End Function
    Public Sub rename_pref_type(ByVal id As String, ByVal name As String, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        Dim str As New bc_cs_string_services(name)
        name = str.delimit_apostrophies
        sql = "update bc_core_cp_preference_types set preference_type_name ='" + name + "' where preference_type_id=" + CStr(id)
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Function delete_pref_type(ByVal id As String, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "exec dbo.bc_core_del_pref_type " + CStr(id)
        delete_pref_type = gbc_db.executesql(sql, certificate)
    End Function
    Public Sub add_pref_type(ByVal name As String, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        Dim str As New bc_cs_string_services(name)
        name = str.delimit_apostrophies
        sql = "exec dbo.bc_core_add_pref_type '" + CStr(name) + "'"
        gbc_db.executesql(sql, certificate)
    End Sub
    REM FIL JUNE 2013
    Public Sub set_user_sync_type_for_users(ByVal sync_id As Long, ByRef certificate As bc_cs_security.certificate)

        Dim sql As String
        sql = "bc_core_cp_set_user_sync_type_for_users " + CStr(sync_id)
        gbc_db.executesql(sql, certificate)

    End Sub

    Public Function add_language(ByVal language_name As String, ByRef certificate As bc_cs_security.certificate)
        REM language code is first 2 letter of name
        Dim sql As String

        REM FIL JULY 2013
        sql = "exec dbo.bc_core_cp_add_language'" + language_name + "'"
        add_language = gbc_db.executesql(sql, certificate)

    End Function
    REM FIL JUN 2013
    Public Function get_user_sync_settings(ByVal user_id As Long, ByRef certificate As bc_cs_security.certificate) As Object

        Dim sql As String

        If bc_cs_central_settings.server_flag = 0 Then
            certificate.user_id = bc_cs_central_settings.logged_on_user_id
        End If

        sql = "exec dbo.bc_core_cp_get_user_sync_settings " + CStr(user_id)
        get_user_sync_settings = gbc_db.executesql(sql, certificate)

    End Function
    Public Function get_sync_level(ByVal user_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "exec dbo.bc_core_get_sync_level " + CStr(user_id) + ",'" + CStr(certificate.client_mac_address) + "'"
        get_sync_level = gbc_db.executesql(sql, certificate)
    End Function
    Public Sub set_user_sync_settings(ByVal user_id As Long, ByVal setting_id As Long, ByRef certificate As bc_cs_security.certificate)

        Dim sql As String

        sql = "exec dbo.bc_core_cp_set_user_sync_settings " + CStr(user_id) + "," + CStr(setting_id)
        gbc_db.executesql(sql, certificate)

    End Sub
    Public Sub delete_user_sync_settings(ByVal user_id As Long, ByRef certificate As bc_cs_security.certificate)

        Dim sql As String

        sql = "exec dbo.bc_core_cp_delete_user_sync_settings " + CStr(user_id)
        gbc_db.executesql(sql, certificate)

    End Sub
    Public Sub update_language(ByVal id As Long, ByVal name As String, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        REM update label code
        sql = "update label_value_tbl set label_value='" + name + "' where language_code='en' and label_code=(select label_code from language_table where language_id=" + CStr(id) + ")"
        gbc_db.executesql(sql, certificate)
    End Sub
    REM FIL July 2012
    Public Function get_pref_types(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select preference_type_id,preference_type_name from bc_core_cp_preference_types order by default_type desc, preference_type_name asc"
        get_pref_types = gbc_db.executesql(sql, certificate)
    End Function
    REM ====================
    Public Function add_office(ByVal desc As String, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "insert into office_tbl (OFFICE_ID, OFFICE_NAME, FILESTORE, ARCHIVE_FILESTORE, deleted, comment, inactive, user_id) select coalesce(max(office_id),1)+1,'" + desc + "',null,null,0,convert(varchar(20),getdate()),0,0 from office_tbl"
        gbc_db.executesql(sql, certificate)
        sql = "select max(office_id) from office_tbl"
        add_office = gbc_db.executesql(sql, certificate)
    End Function
    Public Sub update_office(ByVal id As Long, ByVal desc As String, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "update office_tbl set office_name='" + desc + "' where office_id=" + CStr(id)
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Function delete_language(ByVal id As Long, ByRef certificate As bc_cs_security.certificate) As String
        Dim sql As String
        sql = "exec dbo.bcc_core_cp_generic_delete 'language'," + CStr(id) + "," + CStr(certificate.user_id)
        delete_language = gbc_db.test_sql(sql, certificate)
    End Function
    Public Sub delete_apps_for_role(ByVal id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "delete from bcc_core_cp_apps_for_role where role_id=" + CStr(id)
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Sub add_app_for_role(ByVal id As Long, ByVal app_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "insert into bcc_core_cp_apps_for_role (role_id, app_id) values(" + CStr(id) + "," + CStr(app_id) + ")"
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Function delete_office(ByVal id As Long, ByRef certificate As bc_cs_security.certificate) As String
        Dim sql As String
        sql = "exec dbo.bcc_core_cp_generic_delete 'office'," + CStr(id) + "," + CStr(certificate.user_id)
        delete_office = gbc_db.test_sql(sql, certificate)
    End Function
    Public Function read_associated_users(ByVal id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select associated_user_id from bc_core_cp_user_associations where user_id=" + CStr(id)
        read_associated_users = gbc_db.executesql(sql, certificate)
    End Function
    Public Function read_other_roles(ByVal id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        If bc_cs_central_settings.server_flag = 0 Then
            certificate.user_id = bc_cs_central_settings.logged_on_user_id
        End If
        sql = "dbo.bc_core_read_other_roles " + CStr(id)
        read_other_roles = gbc_db.executesql(sql, certificate)
    End Function
    Public Sub delete_other_roles(ByVal user_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "exec dbo.bc_core_delete_other_roles " + CStr(user_id)
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Sub write_other_role(ByVal user_id As Long, ByVal role_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        If bc_cs_central_settings.server_flag = 0 Then
            sql = "exec dbo.bc_core_write_other_role " + CStr(user_id) + "," + CStr(role_id) + "," + CStr(bc_cs_central_settings.logged_on_user_id)
        Else
            sql = "exec dbo.bc_core_write_other_role " + CStr(user_id) + "," + CStr(role_id) + "," + CStr(certificate.user_id)
        End If

        'sql = "exec dbo.bc_core_write_other_role " + CStr(user_id) + "," + CStr(role_id)
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Sub delete_associated_users(ByVal id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "exec dbo.bc_core_delete_user_associations " + CStr(id)
        'sql = "delete from  bc_core_cp_user_associations where user_id=" + CStr(id)
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Sub write_associated_user(ByVal id As Long, ByVal ass_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        If bc_cs_central_settings.server_flag = 0 Then
            sql = "exec dbo.bc_core_write_user_association " + CStr(id) + "," + CStr(ass_id) + "," + CStr(bc_cs_central_settings.logged_on_user_id)
        Else
            sql = "exec dbo.bc_core_write_user_association " + CStr(id) + "," + CStr(ass_id) + "," + CStr(certificate.user_id)
        End If

        'sql = "insert into bc_core_cp_user_associations (user_id,associated_user_id) values(" + CStr(id) + "," + CStr(ass_id) + ")"
        gbc_db.executesql(sql, certificate)
    End Sub

    Public Function add_bus_area(ByVal desc As String, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "insert into bus_area_table (bus_area_id, bus_area_name, deleted, comment, inactive, user_id) select coalesce(max(bus_area_id),1)+1,'" + desc + "',0,convert(varchar(20),getdate()),0," + CStr(certificate.user_id) + " from bus_area_table"
        gbc_db.executesql(sql, certificate)
        sql = "select max(bus_area_id) from bus_area_table"
        add_bus_area = gbc_db.executesql(sql, certificate)
    End Function
    Public Sub update_bus_area(ByVal id As Long, ByVal desc As String, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "update bus_area_table set bus_area_name='" + desc + "' where bus_area_id=" + CStr(id)
        gbc_db.executesql(sql, certificate)
    End Sub

    Public Function delete_bus_area(ByVal id As Long, ByRef certificate As bc_cs_security.certificate) As String
        Dim sql As String
        sql = "exec dbo.bcc_core_cp_generic_delete 'bus area'," + CStr(id) + "," + CStr(certificate.user_id)
        delete_bus_area = gbc_db.test_sql(sql, certificate)
    End Function


    Public Function do_not_display_taxonomy(ByVal id As Long, ByRef certificate As bc_cs_security.certificate) As Boolean
        do_not_display_taxonomy = False
        Dim sql As String
        Dim res As Object
        sql = "exec dbo.bcc_core_disp_taxonomy " + CStr(id)
        res = gbc_db.executesql_show_no_error(sql)
        If IsArray(res) Then
            If UBound(res, 2) > -1 Then
                If res(0, 0) = "1" Then
                    do_not_display_taxonomy = True
                End If
            End If
        End If
    End Function
    Public Function read_roles(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select role_id, role_name, coalesce(cp_access_level,0),coalesce(inactive,0) from role_tbl where coalesce(deleted,0)=0 order by role_description asc"
        read_roles = gbc_db.executesql(sql, certificate)
    End Function
    Public Function delete_user(ByVal user_id As Long, ByRef certificate As bc_cs_security.certificate) As String
        Dim sql As String
        sql = "exec dbo.bcc_core_cp_generic_delete 'user'," + CStr(user_id) + "," + CStr(certificate.user_id)
        delete_user = gbc_db.test_sql(sql, certificate)
    End Function
    Public Function read_languages(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select l.language_id, v.label_value, coalesce(l.inactive,0) from language_table l inner join label_value_tbl v on v.label_code=l.label_code and v.language_code='en' and coalesce(deleted,0)=0 order by v.label_value asc"
        read_languages = gbc_db.executesql(sql, certificate)
    End Function

    Public Function read_offices(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select office_id, rtrim(office_name), coalesce(inactive,0) from office_tbl where coalesce(deleted,0)=0 order by office_name asc"
        read_offices = gbc_db.executesql(sql, certificate)
    End Function
    Public Function read_bus_areas(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select bus_area_id, bus_area_name, coalesce(inactive,0) from bus_area_table where coalesce(deleted,0)=0 order by bus_area_name asc"
        read_bus_areas = gbc_db.executesql(sql, certificate)
    End Function
    Public Function add_user(ByVal first_name As String, ByVal middle_name As String, ByVal surname As String, ByVal role_level As Integer, ByVal os_user_name As String, ByVal language_id As Integer, ByVal email As String, ByVal telephone As String, ByVal fax As String, ByVal office_id As Integer, ByVal sync_level As Integer, ByVal web_user_name As String, ByVal password As String, ByVal mobile As String, ByVal job_title As String, ByVal biography As String, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        Dim vres As Object
        Dim na As String
        sql = "select coalesce(max(user_id) ,0)  + 1 from user_table"
        vres = gbc_db.executesql(sql, certificate)
        na = web_user_name
        sql = "insert into user_table(access_flag,user_id,first_name, middle_name,surname,web_user_name,role_id,os_user_name,language_id,email,telephone,fax,office_id,user_at_closedown_enable_flag,bus_area_id,login_failure_count,comment,deleted,change_user_id,password,mobile_telephone,job_title, biography) values(0," + CStr(vres(0, 0)) + ", '" + first_name + "','" + middle_name + "','" + surname + "','" + na + "'," + CStr(role_level) + ",'" + os_user_name + "'," + CStr(language_id) + ",'" + email + "','" + telephone + "','" + fax + "'," + CStr(office_id) + "," + CStr(sync_level) + ",null,0, convert(varchar(20),getdate()),0," + CStr(certificate.user_id) + ",'" + password + "','" + mobile + "','" + job_title + "','" + biography + "')"
        gbc_db.executesql(sql, certificate)
        sql = "select max(coalesce(user_id,0)) + 1 from user_table"
        add_user = vres
    End Function
    Public Sub delete_bus_area_for_user(ByVal id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "exec dbo.bc_core_delete_bus_area_for_user " + CStr(id)
        'sql = "delete from person_bus_area_tbl where user_id=" + CStr(id)
        gbc_db.executesql(sql, certificate)
    End Sub

    Public Sub add_picture_extension(id As Long, picture_extension As String, dist_files As Boolean, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "update user_table set shortcode='" + picture_extension + "' where user_id=" + CStr(id)
        gbc_db.executesql(sql, certificate)

        REM now distrunte the imaeg
        sql = "exec dbo.bc_core_dist_user_image " + CStr(id)
        Dim res As Object
        res = gbc_db.executesql(sql, certificate)
        If dist_files = False Then
            Exit Sub
        End If

        Dim control_file As String
        Dim source_file As String
        Dim control_filename As String
        Dim target_location As String

        If IsArray(res) Then
            If UBound(res, 2) = 0 Then
                Try
                    source_file = CStr(id) + picture_extension
                    control_file = res(0, 0)
                    control_filename = res(1, 0)
                    target_location = res(2, 0)

                    Dim fs As New bc_cs_file_transfer_services
                    If fs.check_document_exists(bc_cs_central_settings.central_repos_path + "user images\" + source_file, certificate) = True Then
                        fs.file_copy(bc_cs_central_settings.central_repos_path + "user images\" + source_file, target_location + "\" + source_file)
                        Dim sw As New StreamWriter(target_location + "\" + control_filename, False)
                        sw.WriteLine(control_file)
                        sw.Close()

                    End If
                Catch ex As Exception
                    Dim oerr As New bc_cs_error_log("bc_om_user_db", "add_picture_extension", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)



                End Try
            End If
        End If








        If IsArray(res) Then

        End If

    End Sub

    Public Function get_picture_extension(id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "select shortcode from user_table where user_id=" + CStr(id)
        get_picture_extension = gbc_db.executesql(sql, certificate)
    End Function
    Public Sub add_bus_area_for_user(ByVal id As Long, ByVal bus_area_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        If bc_cs_central_settings.server_flag = 0 Then
            sql = "exec dbo.bc_core_insert_person_bus_area_tbl " + CStr(id) + "," + CStr(bus_area_id) + "," + CStr(bc_cs_central_settings.logged_on_user_id)
        Else
            sql = "exec dbo.bc_core_insert_person_bus_area_tbl " + CStr(id) + "," + CStr(bus_area_id) + "," + CStr(certificate.user_id)
        End If
        'sql = "insert into person_bus_area_tbl (user_id, bus_area_id, core_flag) values (" + CStr(id) + "," + CStr(bus_area_id) + ",1)"
        gbc_db.executesql(sql, certificate)
    End Sub


    'Public Sub update_user(ByVal id As Long, ByVal first_name As String, ByVal middle_name As String, ByVal surname As String, ByVal role_level As Integer, ByVal os_user_name As String, ByVal language_id As Integer, ByVal email As String, ByVal telephone As String, ByVal fax As String, ByVal office_id As Integer, ByVal sync_level As Integer, ByVal web_user_name As String, ByVal password As String, ByVal mobile As String, ByVal job_title As String, ByVal biography As String, ByRef certificate As bc_cs_security.certificate)
    '    Dim sql As String
    '    Dim fs As New bc_cs_string_services(web_user_name)
    '    web_user_name = fs.delimit_apostrophies
    '    sql = "update user_table set web_user_name='" + web_user_name + "', first_name='" + first_name + "', surname='" + surname + "',middle_name='" + middle_name + "', role_id=" + CStr(role_level) + ", os_user_name='" + os_user_name + "',language_id=" + CStr(language_id) + ",email='" + email + "', telephone='" + telephone + "', fax='" + fax + "', office_id=" + CStr(office_id) + ", user_at_closedown_enable_flag=" + CStr(sync_level) + ", comment =  convert(varchar(20),getdate()), change_user_id=" + CStr(certificate.user_id) + ", password='" + password + "', mobile_telephone='" + mobile + "', job_title='" + job_title + "', biography='" + biography + "' where user_id=" + CStr(id)
    '    gbc_db.executesql(sql, certificate)
    'End Sub
    REM FIL JUne 2013
    Public Function read_all_sync_types(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "exec dbo.bc_core_cp_get_sync_settings"
        read_all_sync_types = gbc_db.executesql(sql, certificate)
    End Function

    Public Function read_person_bus_areas(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select b.user_id, b.bus_area_id from person_bus_area_tbl b inner join user_table u on u.user_id=b.user_id and coalesce(u.deleted,0)=0 inner join bus_area_table c on c.bus_area_id=b.bus_area_id and coalesce(c.deleted,0) = 0"
        read_person_bus_areas = gbc_db.executesql(sql, certificate)
    End Function
    REM FIL JUN 2013
    Public Sub reset_partial_sync(ByRef certificate As bc_cs_security.certificate)

        Dim sql As String
        If bc_cs_central_settings.server_flag = 0 Then
            certificate.user_id = bc_cs_central_settings.logged_on_user_id
        End If
        sql = "exec dbo.bc_core_reset_partial_user_sync " + CStr(certificate.user_id)
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Function check_authentication(ByVal os_user_name As String, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select user_id from user_table where os_user_name='" + CStr(os_user_name) + "'"
        check_authentication = gbc_db.executesql(sql, certificate)
    End Function
    Public Sub deploy_success(ByVal release_number, ByVal user_id, ByVal certificate)
        Dim sql As String
        REM SR - "user_release" is a redundant table
        sql = "insert into user_release values (" + CStr(release_number) + ",getdate()," + CStr(user_id) + ")"
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Sub set_language_active(ByVal inactive_flag As Integer, ByVal id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        Dim comment As String
        If inactive_flag = 0 Then
            comment = " set active"
        Else
            comment = " set inactive"
        End If
        sql = "update language_table set inactive=" + CStr(inactive_flag) + ", comment = '" + comment + " ' +  convert(varchar(20),getdate()), user_id=" + CStr(certificate.user_id) + " where language_id=" + CStr(id)
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Sub set_role_active(ByVal inactive_flag As Integer, ByVal id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        Dim comment As String
        If inactive_flag = 0 Then
            comment = " set active"
        Else
            comment = " set inactive"
        End If
        sql = "update role_tbl set inactive=" + CStr(inactive_flag) + ", comment = '" + comment + " ' +  convert(varchar(20),getdate()), user_id=" + CStr(certificate.user_id) + " where role_id=" + CStr(id)
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Sub set_bus_area_active(ByVal inactive_flag As Integer, ByVal id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        Dim comment As String
        If inactive_flag = 0 Then
            comment = " set active"
        Else
            comment = " set inactive"
        End If
        sql = "update bus_area_table set inactive=" + CStr(inactive_flag) + ", comment = '" + comment + " ' +  convert(varchar(20),getdate()), user_id=" + CStr(certificate.user_id) + " where bus_area_id=" + CStr(id)
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Sub set_office_active(ByVal inactive_flag As Integer, ByVal id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        Dim comment As String
        If inactive_flag = 0 Then
            comment = " set active"
        Else
            comment = " set inactive"
        End If
        sql = "update office_tbl set inactive=" + CStr(inactive_flag) + ", comment = '" + comment + " ' +  convert(varchar(20),getdate()), user_id=" + CStr(certificate.user_id) + " where office_id=" + CStr(id)
        gbc_db.executesql(sql, certificate)
    End Sub

    Public Sub set_sync_flag(ByVal flag As Integer, ByVal user_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "update user_table set user_at_closedown_enable_flag=" + CStr(flag) + " where user_id=" + CStr(user_id)
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Sub set_user_active(ByVal inactive_flag As Integer, ByVal user_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        Dim comment As String
        If inactive_flag = 0 Then
            comment = " set active"
        Else
            comment = " set inactive"
        End If
        sql = "update user_table set inactive=" + CStr(inactive_flag) + ", comment = '" + comment + " ' +  convert(varchar(20),getdate()), change_user_id=" + CStr(certificate.user_id) + " where user_id=" + CStr(user_id)
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Function read_all_users(ByVal inactive As Boolean, ByVal set_sync As Boolean, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        If inactive = True Then
            sql = "select user_table.user_id, coalesce(os_user_name,''), rtrim(surname), rtrim(first_name), coalesce(user_at_closedown_enable_flag,0), role_name,coalesce(password,''),coalesce(web_user_name,''), coalesce(user_table.inactive,'0'), coalesce(office_id,0),coalesce(cp_access_level,0), coalesce(language_id,0),coalesce(user_table.role_id,0), coalesce(password,''), coalesce(mobile_telephone,''), coalesce(job_title,'') from " + _
                  " user_table, role_tbl where user_table.role_id=role_tbl.role_id and coalesce(user_table.deleted,0)=0 and coalesce(role_tbl.deleted,0)=0 order by first_name asc"
        Else
            sql = "select user_table.user_id, coalesce(os_user_name,''), rtrim(surname), rtrim(first_name), coalesce(user_at_closedown_enable_flag,0), role_name,coalesce(password,''),coalesce(web_user_name,''), coalesce(user_table.inactive,'0'), coalesce(office_id,0),coalesce(cp_access_level,0), coalesce(language_id,0),coalesce(user_table.role_id,0), coalesce(password,''), coalesce(mobile_telephone,''), coalesce(job_title,'') from " + _
                  " user_table, role_tbl where user_table.role_id=role_tbl.role_id and coalesce(user_table.deleted,0)=0 and coalesce(user_table.inactive,0)=0 and coalesce(role_tbl.deleted,0)=0 order by first_name asc"

        End If
        read_all_users = gbc_db.executesql(sql, certificate)
        'If set_sync = True Then
        '    reset_user_sync(certificate)
        'End If
    End Function
End Class
Public Class bc_om_pref_db
    Private gbc_db As New bc_cs_db_services
    Public Function get_events_mode(ByVal user_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "exec dbo.bc_core_get_events_mode " + CStr(user_id)
        get_events_mode = gbc_db.executesql(sql, certificate)
    End Function

    REM read all prefes for logged on user from database
    Public Function read_pref_for_user(ByVal logged_on_user_id As String, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "exec dbo.bc_core_user_prefs " + CStr(logged_on_user_id)
        read_pref_for_user = gbc_db.executesql(sql, certificate)
        'sql = "select entity_id from apref_entity_tbl where user_id=" + CStr(logged_on_user_id)
        'read_pref_for_user = gbc_db.executesql(sql, certificate)
    End Function
    Public Function get_application_forms(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select label_list_code from label_list_tbl where label_list_code like 'create%' or label_list_code like 'insight%' order by label_list_code asc"
        get_application_forms = gbc_db.executesql(sql, certificate)
    End Function
    Public Function get_application_form_labels(ByVal form_name As String, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        Dim luser As String
        Dim lusername As String
        If bc_cs_central_settings.server_flag = 0 Then
            luser = bc_cs_central_settings.logged_on_user_id
            lusername = bc_cs_central_settings.logged_on_user_name
        Else
            luser = certificate.user_id
            lusername = certificate.name
        End If

        sql = "select  m.label_code, label_value from label_value_tbl l, label_list_map_tbl m, language_table u, user_table t " + _
              "where  u.language_id=t.language_id and t.user_id=" + CStr(luser) + " and " + _
              "u.language_code = l.language_code and " + _
              "l.label_code = m.label_code and " + _
              "m.label_list_code='" + form_name + "' order by m.label_code asc"

        get_application_form_labels = gbc_db.executesql(sql, certificate)
    End Function
    Public Function get_financial_workflow_stage(ByVal logged_on_user_id As String, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "select stage_id, default_flag from bcc_core_financial_wf_role f, user_table u where  u.user_id = " + CStr(logged_on_user_id) + " And u.role_id = f.role_id order by stage_id asc"
        get_financial_workflow_stage = gbc_db.executesql_show_no_error(sql)
    End Function
    Public Function get_bus_areas(ByVal logged_on_user_id As String, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "select p.bus_area_id from person_bus_area_tbl p, user_table u, bus_area_table b where p.user_id = u.user_id And u.user_id = " + CStr(logged_on_user_id) + " And coalesce(b.deleted, 0) = 0 And coalesce(u.deleted, 0) = 0 and p.bus_area_id=b.bus_area_id"
        get_bus_areas = gbc_db.executesql(sql, certificate)

    End Function
    REM SIM MAY 2012
    Public Function get_secondary_user_roles(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        If bc_cs_central_settings.server_flag = 0 Then
            certificate.user_id = bc_cs_central_settings.logged_on_user_id
        End If
        sql = "bc_core_get_secondary_user_roles " + CStr(certificate.user_id)
        get_secondary_user_roles = gbc_db.executesql(sql, certificate)
    End Function
    REM ==========================================

    Public Function get_document_protection_password(ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "select password from bc_core_document_protection"
        get_document_protection_password = gbc_db.executesql(sql, certificate)
    End Function

    Public Function get_stage_role_access(ByVal logged_on_user_id As String, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "select s.stage_id, stage_name, upper(access_id) from stage_tbl s, stage_role_access_tbl l, user_table u where u.role_Id = l.role_id and s.stage_id= l.stage_id and u.user_id=" + CStr(logged_on_user_id) + " order by stage_name asc"
        get_stage_role_access = gbc_db.executesql(sql, certificate)
    End Function

    Public Sub New()

    End Sub
End Class
<Serializable()> Public Class bc_om_releases
    Inherits bc_cs_soap_base_class
    Public releases As New ArrayList

    Public Sub New()

    End Sub
    Public Overrides Sub process_object()
        Dim otrace As New bc_cs_activity_log("bc_om_releases", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            REM this is always specific to object
            If Me.tmode = bc_cs_soap_base_class.tREAD Then
                db_read()
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_releases", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_releases", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub
    Public Sub db_read()
        Dim otrace As New bc_cs_activity_log("bc_om_releases", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Dim gdb As New bc_om_user_db
            Dim orelease As bc_om_release
            Dim vres As Object
            Me.releases.Clear()
            vres = gdb.read_all_releases(certificate)
            If IsArray(vres) Then
                For i = 0 To UBound(vres, 2)
                    orelease = New bc_om_release
                    orelease.release_number = vres(0, i)
                    orelease.server_flag = vres(1, i)
                    orelease.create_date = vres(2, i)
                    orelease.release_date = vres(3, i)
                    orelease.status = vres(4, i)
                    orelease.desc = vres(5, i)
                    Me.releases.Add(orelease)
                Next
            End If


        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_releases", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_releases", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Sub
End Class


<Serializable()> Public Class bc_om_release
    Inherits bc_cs_soap_base_class
    Public release_number As Integer
    Public create_date As Date
    Public release_date As Date
    Public server_flag As Boolean
    Public status As Integer
    Public desc As String
    Public user_id As Long
    Public client_deployment_date As Date
    Public assemblies As New ArrayList
    Public load_error As String
    Public Const WORKING = 0
    Public Const ACTIVE = 1
    Public Const PREVIOUS = 2
    Public Overrides Sub process_object()
        Dim otrace As New bc_cs_activity_log("bc_om_assemblies", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            REM this is always specific to object
            If Me.tmode = bc_cs_soap_base_class.tREAD Then
                db_read()
            End If
            If Me.tmode = bc_cs_soap_base_class.tWRITE Then
                db_write()
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_assemblies", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_assemblies", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub
    Public Sub New()
        status = WORKING
    End Sub
    Public Sub db_write()
        REM identify deployment was successful
        Dim db_users As New bc_om_user_db
        db_users.deploy_success(release_number, user_id, certificate)
    End Sub
    Public Sub db_read()
        Dim db_users As New bc_om_user_db
        Dim vass As Object
        Dim i As Integer
        Dim oassembly As bc_om_assembly
        Dim fs As New bc_cs_file_transfer_services
        load_error = ""
        If db_users.evaluate_upgrade_required(user_id, release_number, certificate) = True Then
            REM read all assemblies for release
            vass = db_users.read_assemblies_for_release(release_number, certificate)
            If IsArray(vass) Then
                For i = 0 To UBound(vass, 2)
                    oassembly = New bc_om_assembly
                    oassembly.name = vass(1, i)
                    oassembly.type = vass(3, i)
                    oassembly.version = vass(5, i)
                    If fs.write_document_to_bytestream(vass(2, i) + vass(1, i), oassembly.assembly_file, certificate, False) = False Then
                        Dim ocommentary As New bc_cs_activity_log("bc_om_assemblies", "db_read", bc_cs_activity_codes.COMMENTARY, "File not found so Assembly upgrade aborted!: " + vass(2, i) + vass(1, i), certificate)
                        release_number = 0
                        load_error = "Deployment Server Error File not found so Assembly upgrade aborted!: " + vass(2, i) + vass(1, i)
                        Exit For
                    Else
                        Me.assemblies.Add(oassembly)
                    End If
                Next
            End If
        Else
            release_number = 0
        End If
    End Sub
End Class
<Serializable()> Public Class bc_om_assembly
    Public name As String
    Public version As String
    Public assembly_file As Byte()
    Public type As Integer
    REM 1 GAC dll
    REM 2 tlb
    REM 3 exe
    REM 4 config file
End Class
<Serializable()> Public Class bc_om_alert
    Inherits bc_cs_soap_base_class
    Public doc_id As Long
    Public alert_tx As String
    Public application As String
    Public datetime As DateTime
    Public process_mode As Integer = 0
End Class
<Serializable()> Public Class bc_om_user_messages
    Inherits bc_cs_soap_base_class
    Public Const tREAD_NEW = 4
    Public current_user_id As Long
    Public from_service_alert As Boolean = False
    Public message_threads As New ArrayList
    Public Overrides Sub process_object()
        Select Case Me.tmode
            Case bc_om_user_messages.tREAD_NEW
                db_read_new_threads()
        End Select

    End Sub
    Public Sub db_read_new_threads()
        Dim vres As Object
        Dim i As Integer
        Dim omt As bc_om_user_msg_thread
        Dim gdb As New bc_on_user_messages_db

        Me.message_threads.Clear()
        vres = gdb.read_new_threads(Me.current_user_id, Me.from_service_alert, MyBase.certificate)
        If IsArray(vres) Then
            For i = 0 To UBound(vres, 2)
                omt = New bc_om_user_msg_thread
                omt.message = vres(0, i)
                omt.date_sent = vres(1, i)
                omt.from_user_id = vres(2, i)
                Me.message_threads.Add(omt)
            Next
        End If
    End Sub

End Class
<Serializable()> Public Class bc_om_user_msg_thread
    Inherits bc_cs_soap_base_class
    Public from_user_id As Long
    Public to_user_id As Long
    Public date_sent As Date
    Public date_received As Date
    Public message As String


    Public Overrides Sub process_object()
        Select Case Me.tmode

            Case bc_cs_soap_base_class.tWRITE
                db_write()
        End Select

    End Sub
    Public Sub db_write()
        Dim gdb As New bc_on_user_messages_db
        Dim str As New bc_cs_string_services(Me.message)
        Me.message = str.delimit_apostrophies

        gdb.write_thread(Me.from_user_id, Me.to_user_id, message, certificate)
    End Sub

End Class
Public Class bc_on_user_messages_db
    Private gbc_db As New bc_cs_db_services

    Public Sub write_thread(ByVal from_user_id As Long, ByVal to_user_id As Long, ByVal message As String, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        REM SR - "bcc_core_al_msg_threads" is a redundant table
        sql = "insert into bcc_core_al_msg_threads values(" + CStr(to_user_id) + "," + CStr(from_user_id) + ",getdate(),'9-9-9999','" + message + "',0)"
        gbc_db.executesql(sql, certificate)
        REM read all prefes for logged on user from database
    End Sub
    Public Function read_new_threads(ByVal from_user_id As Long, ByVal from_service_alert As Boolean, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        If from_service_alert = False Then
            sql = "select message,date_submitted, from_user_id from bcc_core_al_msg_threads where to_user_id=" + CStr(from_user_id) + " and date_read = '9-9-9999' order by date_submitted asc"
            read_new_threads = gbc_db.executesql(sql, certificate)
            REM mark as received
            sql = "update bcc_core_al_msg_threads set date_read=getdate() where to_user_id=" + CStr(from_user_id) + " and date_read='9-9-9999'"
            gbc_db.executesql(sql, certificate)
        Else
            sql = "select message,date_submitted, from_user_id from bcc_core_al_msg_threads where to_user_id=" + CStr(from_user_id) + " and date_read = '9-9-9999' and server_alerted = 0 order by date_submitted asc"
            read_new_threads = gbc_db.executesql(sql, certificate)
            REM mark as picked up by service alert
            sql = "update bcc_core_al_msg_threads set server_alerted=1 where to_user_id=" + CStr(from_user_id) + " and date_read='9-9-9999'"
            gbc_db.executesql(sql, certificate)
            REM mark as picked up by service alert
        End If
    End Function


End Class









