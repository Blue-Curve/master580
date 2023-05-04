Imports BlueCurve.Core.OM
Imports BlueCurve.Core.CS
Imports System.Threading
Imports System.Security.Principal
Imports System.Runtime.InteropServices
REM ==========================================
REM Blue Curve Limited 2005
REM Module:       Load
REM Type:         AM
REM Desciption:   Class to load System Data into memory
REM Methods:  New
REM               check connection
REM               check logon
REM               check sync
REM Version:      1
REM Change history
REM ==========================================
Public Class bc_am_load
    Public bdataloaded As Boolean
    Public abortload As Boolean
    Public bdeploy As Boolean
    Public bcancelselected As Boolean
    Private Shared already_loaded As Boolean = False
    Private Shared selected_authentication_method As Integer = -1
    Private current_application As String
    Declare Auto Function CloseHandle Lib "kernel32.dll" (ByVal handle As IntPtr) As Long

    REM constructor for other Load
    Public Sub New(ByVal external As Boolean)

    End Sub

    REM new dx load
    Sub dx_load_data_into_memory(application As String)
        Try
            Dim oall = New bc_am_load_objects(bc_cs_central_settings.local_template_path, application)
            If bc_am_load_objects.bloadsuccess = False Then
                Dim ocomm As New bc_cs_activity_log("bc_am_load", "dx_load_data_into_memory", bc_cs_activity_codes.COMMENTARY, "load objects failed to load")
                Dim omsg As New bc_cs_message("Blue Curve", "System is offline and never been synchronized. Cannot work offline", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Sub
            End If
            copy_create_settings_into_memory()
            bdataloaded = True
        Catch ex As Exception
            Dim ocomm As New bc_cs_activity_log("bc_am_load", "dx_load_data_into_memory", bc_cs_activity_codes.COMMENTARY, ex.Message)
        End Try
    End Sub
    REM 5.9 when we release new CP
    Public Sub New(ByVal application As String, Optional dummy1 As Boolean = True, Optional authenticate_only As Boolean = False)
        Dim slog = New bc_cs_activity_log("bc_am_load_at", "dx_new", bc_cs_activity_codes.TRACE_ENTRY, "Application Start Load")

        Dim ocomm As bc_cs_activity_log

        Try

            If bc_cs_central_settings.logged_on_user_id <> 0 Then
                bdataloaded = True
                Exit Sub
            End If




            Dim fl As New bc_dx_logon()
            Dim flo As New Cbc_dx_full_logon()
            Dim fs As bc_cs_data_services
            bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve", "Checking Connection...", 10, False, True)
            flo.connect(fl)
            If flo.bshow_form = True Then
                bc_cs_central_settings.progress_bar.hide()
                fl.ShowDialog()
                bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve", "Checking Connection...", 10, False, True)
            End If

            If (flo.success = False) Then
                If application = "Create with Offline Working" Then
                    REM offline TBD
                    If bc_cs_central_settings.bloaderror = True Then
                        Exit Sub
                    End If
                    REM see if local system installed
                    bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.LOCAL
                    fs = New bc_cs_data_services
                    If fs.soap_check_deserialize_file_to_object(bc_cs_central_settings.local_template_path + "bc_pub_types.dat", Nothing) = False Then
                        ocomm = New bc_cs_activity_log("bc_am_load", "dx_load", bc_cs_activity_codes.COMMENTARY, "System never been synced cant work offline.")
                        Dim omsg As New bc_cs_message("Blue Curve", "System is offline and never been synchronized. Cannot work offline", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                        Exit Sub
                    Else
                        REM load system into memory
                        ocomm = New bc_cs_activity_log("bc_am_load", "dx_load", bc_cs_activity_codes.COMMENTARY, "System working offline")
                        Dim omsg As New bc_cs_message("Blue Curve", "System will work offline.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                        dx_load_data_into_memory(application)
                        Exit Sub
                    End If
                Else
                    Exit Sub
                End If
            End If
            bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.connection_method



            REM check sync level
            Dim ouser As New bc_om_user
            ouser.id = bc_cs_central_settings.logged_on_user_id
            Dim sync_level As Integer

            sync_level = check_sync(ouser)
            If authenticate_only = True Then
                sync_level = 0
            End If

            If sync_level > 0 AndAlso application = "BluePrint API" Then
                abortload = True
                Dim omsg As New bc_cs_message("BluePrint - Blue Curve", "Please synchronize first by running BluePrint.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                sync_level = 0
            End If
            REM always load users
            bc_am_load_objects.obc_users = New bc_om_users
            bc_am_load_objects.obc_users.read_partial_sync = False
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                bc_am_load_objects.obc_users.tmode = bc_cs_soap_base_class.tREAD
                bc_am_load_objects.obc_users.transmit_to_server_and_receive(bc_am_load_objects.obc_users, True)
            Else
                bc_am_load_objects.obc_users.db_read()
            End If
            bc_am_load_objects.obc_users.write_data_to_file(bc_cs_central_settings.local_template_path + bc_am_load_objects.USERS_FILENAME)

            Dim sync_failed As Boolean = False
            Dim osync As bc_am_synchronize
            If sync_level = 1 Then

                osync = New bc_am_synchronize
                osync.full_sync()
                If osync.synchronize <> 1 Then
                    sync_failed = True
                End If
            ElseIf sync_level = 2 Then
                'partial sync
                osync = New bc_am_synchronize
                osync.partial_sync(ouser)
                If osync.synchronize <> 1 Then
                    sync_failed = True
                End If
            Else
                REM if no sync load prefs
                bc_cs_central_settings.progress_bar.increment("Loading Preference Data...")

                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                    bc_am_load_objects.obc_prefs.tmode = bc_cs_soap_base_class.tREAD
                    bc_am_load_objects.obc_prefs.transmit_to_server_and_receive(bc_am_load_objects.obc_prefs, True)
                Else
                    bc_am_load_objects.obc_prefs.db_read()
                End If
                bc_am_load_objects.obc_prefs.write_data_to_file(bc_cs_central_settings.local_template_path + bc_am_load_objects.PREFS_FILENAME)
            End If

            REM load data into memoey
            dx_load_data_into_memory(application)

        Finally

            bc_cs_central_settings.progress_bar.hide()
            bc_cs_central_settings.progress_bar.unload()
            slog = New bc_cs_activity_log("bc_am_load_at", "dx_new", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub


    REM consturctor for full system load
    'Public Sub New(ByVal application As String, Optional ByVal not_from_service As Boolean = True, Optional ByVal authenticate_only As Boolean = False)
    '    Dim slog = New bc_cs_activity_log("bc_am_load_at", "new", bc_cs_activity_codes.TRACE_ENTRY, "Application Start Load")
    '    'Dim splash As New bc_as_splash
    '    'splash.Show()

    '    Try
    '        'Dim fl As New bc_dx_logon()
    '        'Dim flo As New Cbc_dx_full_logon()
    '        'If flo.connect(fl) = True Then
    '        '    fl.ShowDialog()
    '        'End If
    '        'If (flo.success = False) Then
    '        '    Exit Sub
    '        'End If
    '        'bdataloaded = True
    '        'Exit Sub



    '        Dim omessage As bc_cs_message
    '        Dim ocommentary As bc_cs_activity_log
    '        Dim entry_level As Integer = -2
    '        REM read .ini file
    '        bdeploy = False
    '        bdataloaded = False
    '        bcancelselected = False
    '        abortload = False
    '        REM 5.6b make this held in work session for performance
    '        Dim bc_cs_central_settings As New bc_cs_central_settings(True)
    '        If application = "Create" Or application = "Create with Offline Working" Then
    '            If bc_cs_central_settings.logged_on_user_id <> 0 Then
    '                bdataloaded = True
    '                Exit Sub
    '            End If
    '        End If
    '        REM end

    '        If already_loaded = True Then
    '            bdataloaded = True
    '            If selected_authentication_method = 3 Then
    '                bc_cs_central_settings.selected_autenticated_method = 3
    '            End If
    '            Exit Sub
    '        End If
    '        current_application = application

    '        bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve", "initializing...", 10, False, True)


    '        REM if not successful abort load system does not function
    '        REM without config file
    '        If Not bc_cs_central_settings.bloaderror Then
    '            REM reset log files 
    '            Dim elog As New bc_cs_error_log_file
    '            Dim alog As New bc_cs_activity_log_file
    '            Dim ocurrent_user As New bc_om_user
    '            REM initialise to local use
    '            bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.LOCAL
    '            REM check if connected method requested is visible
    '            bc_cs_central_settings.progress_bar.increment("Checking Connection...")
    '            REM if SOAP then test here
    '            Dim soap_connection As Boolean = True
    '            Dim ad_failed As Boolean = False
    '            bc_cs_central_settings.selected_autenticated_method = -1
    '            If bc_cs_central_settings.connection_method = bc_cs_central_settings.SOAP Then



    '                REM if AD mode check this first
    '                If bc_cs_central_settings.show_authentication_form = 2 Then
    '                    Dim bcs As New bc_cs_security
    '                    Dim certificate As New bc_cs_security.certificate
    '                    REM steve temp
    '                    If bcs.connect_to_active_directory(certificate) = False Then
    '                        'If bc_cs_central_settings.allow_ad_prompt = 1 Then
    '                        '    REM active directory failed
    '                        '    Dim ocomm As New bc_cs_activity_log("bc_am_load", "new", bc_cs_activity_codes.COMMENTARY, "AD authetication failed so tryin username password")
    '                        '    Dim j As Integer
    '                        '    Dim auth_ret As Integer = -1
    '                        '    bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP
    '                        '    REM change mode to prompt active directory
    '                        '    bc_cs_central_settings.show_authentication_form = 3
    '                        '    bc_cs_central_settings.selected_autenticated_method = 3

    '                        '    REM i username/ password passed in bypass thisbit
    '                        '    For j = 1 To bc_cs_central_settings.authentication_retries

    '                        '        bc_cs_central_settings.user_name = ""
    '                        '        bc_cs_central_settings.user_password = ""
    '                        '        auth_ret = authentication_form(ocurrent_user)
    '                        '        If auth_ret <> 0 Then
    '                        '            Exit For
    '                        '        End If


    '                        '        If j = 3 Then
    '                        '            auth_ret = -1
    '                        '        End If
    '                        '    Next
    '                        '    entry_level = auth_ret
    '                        '    If entry_level <> 1 Then
    '                        '        bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.LOCAL
    '                        '        soap_connection = False
    '                        '    Else
    '                        '        soap_connection = True
    '                        '    End If
    '                        '    ad_failed = True
    '                        'Else
    '                        Dim ocomm As New bc_cs_activity_log("bc_am_load", "new", bc_cs_activity_codes.COMMENTARY, "AD authetication failed")
    '                        soap_connection = False
    '                        ad_failed = True
    '                        bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.LOCAL
    '                        'End If
    '                    Else
    '                        Dim ocomm As New bc_cs_activity_log("bc_am_load", "new", bc_cs_activity_codes.COMMENTARY, "AD authetication passed")
    '                        soap_connection = True
    '                    End If
    '                End If
    '                If entry_level = 3 Then
    '                    Dim fs As New bc_cs_data_services
    '                    If fs.soap_check_deserialize_file_to_object(bc_cs_central_settings.local_template_path + "bc_pub_types.xml", Nothing) = False Then
    '                        ocommentary = New bc_cs_activity_log("bc_am_load", "new", bc_cs_activity_codes.COMMENTARY, "Local System not installed. Contact System Administrator!")
    '                        'omessage = New bc_cs_message("Blue Curve", "Local System not installed. Contact System Administrator!", bc_cs_message.MESSAGE)
    '                        abortload = True
    '                    Else
    '                        bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.LOCAL
    '                        Dim obcobjects As New bc_am_load_objects(bc_cs_central_settings.local_template_path, application)
    '                        bdataloaded = bc_am_load_objects.bloadsuccess
    '                    End If
    '                End If
    '                If ad_failed = False Then
    '                    Dim lsoapconn As New bc_cs_test_soap_server_connection
    '                    If lsoapconn.transmit_to_server_and_receive(lsoapconn, False) = False Then
    '                        If lsoapconn.transmission_state = 3 Then
    '                            soap_connection = False
    '                        End If

    '                    End If
    '                End If
    '            End If

    '            If bc_cs_central_settings.check_connection(bc_cs_central_settings.connection_method, soap_connection) = True Then
    '                REM attempt logon

    '                bc_cs_central_settings.progress_bar.increment("Attempting Logon...")
    '                entry_level = check_logon(ocurrent_user)


    '                If entry_level = 2 Then
    '                    ocommentary = New bc_cs_activity_log("bc_am_load", "new", bc_cs_activity_codes.COMMENTARY, "User Selected Application Cancel from Logon!")
    '                    bcancelselected = True
    '                    abortload = True
    '                    Exit Sub
    '                End If
    '                If entry_level = 1 And authenticate_only = False Then
    '                    Dim xcheck_sync As Integer
    '                    xcheck_sync = check_sync(ocurrent_user)
    '                    bc_cs_central_settings.logged_on_user_id = ocurrent_user.id
    '                    If xcheck_sync = 1 Then
    '                        ' do not sync when using the BluePrint API
    '                        If application = "BluePrint API" Then
    '                            abortload = True
    '                            omessage = New bc_cs_message("BluePrint - Blue Curve", "Please synchronize first by running BluePrint.", bc_cs_message.MESSAGE)
    '                            Exit Try
    '                        End If
    '                        Dim osync As New bc_am_synchronize
    '                        osync.full_sync()

    '                        If osync.synchronize = 1 Then
    '                            REM synchronize also loads all data into memory
    '                            bdataloaded = True
    '                        Else
    '                            ocommentary = New bc_cs_activity_log("bc_am_load", "new", bc_cs_activity_codes.COMMENTARY, "Synchronize failed so attempting to load from local files")
    '                        End If
    '                        REM FIL JUN 2013
    '                    ElseIf xcheck_sync = 2 Then

    '                        ' do not sync when using the BluePrint API
    '                        If application = "BluePrint API" Then
    '                            abortload = True
    '                            omessage = New bc_cs_message("BluePrint - Blue Curve", "Please synchronize first by running BluePrint.", bc_cs_message.MESSAGE)
    '                            Exit Try
    '                        End If
    '                        Dim osync As New bc_am_synchronize
    '                        bc_cs_central_settings.progress_bar.unload()

    '                        bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve", "Synchronizing...", 0, False, True)


    '                        REM read in current files in case not all got synced
    '                        Dim obcobjects As New bc_am_load_objects(bc_cs_central_settings.local_template_path, application)
    '                        osync.partial_sync(ocurrent_user)
    '                        If osync.synchronize = 1 Then
    '                            REM synchronize also loads all data into memory
    '                            bdataloaded = True
    '                        Else
    '                            ocommentary = New bc_cs_activity_log("bc_am_load", "new", bc_cs_activity_codes.COMMENTARY, "Synchronize failed so attempting to load from local files")
    '                        End If
    '                    Else
    '                        REM sync user preferance data only
    '                        bc_cs_central_settings.progress_bar.increment("Loading Preference Data...")

    '                        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then

    '                            bc_am_load_objects.obc_prefs.tmode = bc_cs_soap_base_class.tREAD
    '                            bc_am_load_objects.obc_prefs.transmit_to_server_and_receive(bc_am_load_objects.obc_prefs, True)

    '                            ocommentary = New bc_cs_activity_log("bc_am_load", "new", bc_cs_activity_codes.COMMENTARY, "Loading User Preference Data via SOAP")
    '                        Else
    '                            bc_am_load_objects.obc_prefs.db_read()
    '                        End If
    '                        bc_am_load_objects.obc_prefs.write_data_to_file(bc_cs_central_settings.local_template_path + bc_am_load_objects.PREFS_FILENAME)
    '                    End If
    '                    already_loaded = True
    '                End If
    '                If entry_level = 3 Then
    '                    Dim fs As New bc_cs_data_services
    '                    If fs.soap_check_deserialize_file_to_object(bc_cs_central_settings.local_template_path + "bc_pub_types.dat", Nothing) = False Then
    '                        ocommentary = New bc_cs_activity_log("bc_am_load", "new", bc_cs_activity_codes.COMMENTARY, "Local System not installed. Contact System Administrator!")
    '                        'omessage = New bc_cs_message("Blue Curve", "Logon Failed for User: " + bc_cs_central_settings.logged_on_user_name + " and Local System not installed. Contact System Administrator!", bc_cs_message.MESSAGE)
    '                        abortload = True
    '                    Else
    '                        bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.LOCAL
    '                        Dim obcobjects As New bc_am_load_objects(bc_cs_central_settings.local_template_path, application)
    '                        bdataloaded = bc_am_load_objects.bloadsuccess
    '                    End If
    '                End If

    '                If entry_level = 0 Or entry_level = -1 Then

    '                    If (bc_cs_central_settings.show_authentication_form = 0 Or bc_cs_central_settings.show_authentication_form = 2) Then
    '                        omessage = New bc_cs_message("Blue Curve", "Authentication Failed for: " + bc_cs_central_settings.logged_on_user_name, bc_cs_message.MESSAGE)
    '                    Else
    '                        'If bc_cs_central_settings.selected_conn_method <> bc_cs_central_settings.SOAP Then
    '                        '    omessage = New bc_cs_message("Blue Curve", "Authentication Failed for: " + bc_cs_central_settings.user_name, bc_cs_message.MESSAGE)
    '                        'End If
    '                    End If
    '                    abortload = True
    '                End If
    '                'allred

    '                Dim strLockMessage As String = ""
    '                If abortload = False Then

    '                    strLockMessage = GetLockMessage()
    '                End If
    '                If Not application = "Deployment" AndAlso abortload = False AndAlso GetDatabaseStatus() Then
    '                    'need to get message
    '                    omessage = New bc_cs_message("Blue Curve", "System has been locked.", bc_cs_message.MESSAGE)
    '                    abortload = True
    '                ElseIf Not application = "Deployment" AndAlso Not strLockMessage = "" Then
    '                    omessage = New bc_cs_message("Blue Curve", strLockMessage, bc_cs_message.MESSAGE)
    '                    SetLockMessageDisplayed()
    '                End If
    '                slog = New bc_cs_activity_log("bc_am_load", "new", bc_cs_activity_codes.COMMENTARY, "Connection Method Selected: " + bc_cs_central_settings.selected_conn_method)
    '            End If


    '            REM if data has not been loaded then load now from local xml
    '            If bdataloaded = False And abortload = False And (entry_level = 1 Or entry_level = -2) Then
    '                Dim fs As New bc_cs_data_services
    '                If fs.soap_check_deserialize_file_to_object(bc_cs_central_settings.local_template_path + "bc_pub_types.dat", Nothing) = True Then
    '                    Dim obcobjects As New bc_am_load_objects(bc_cs_central_settings.local_template_path, application)
    '                    bdataloaded = bc_am_load_objects.bloadsuccess
    '                Else
    '                    'omessage = New bc_cs_message("Blue Curve create", "Logon Failed for User: " + bc_cs_central_settings.logged_on_user_name + ". Invalid User. and system has never been synchronized contact system administrator!", bc_cs_message.MESSAGE)
    '                    ocommentary = New bc_cs_activity_log("bc_am_load", "new", bc_cs_activity_codes.COMMENTARY, "System Failed to Load as Local Configuration XML files not Found. System has never been Synchronized")
    '                End If
    '            End If
    '        End If
    '        REM FIL June 2013
    '        REM copy create settings into global memeory
    '        copy_create_settings_into_memory()
    '    Catch ex As Exception

    '        If Err.Number = 57 Then
    '            'process cannot access file
    '            Dim sMsg As New bc_cs_message("Blue Curve", "The Blue Curve server is busy, please try again.", bc_cs_message.MESSAGE)
    '        Else
    '            Dim db_err As New bc_cs_error_log("bc_am_load_at", "new", bc_cs_error_codes.USER_DEFINED, Err.Number, ex.Message)
    '        End If
    '    Finally

    '        If bc_cs_central_settings.selected_autenticated_method = 3 Then
    '            selected_authentication_method = bc_cs_central_settings.selected_autenticated_method
    '        End If
    '        bc_cs_central_settings.progress_bar.unload()
    '        slog = New bc_cs_activity_log("bc_am_load_at", "new2", bc_cs_activity_codes.TRACE_EXIT, "")
    '    End Try
    '    'splash.Hide()
    'End Sub
    ''REM FIL JULY 2013
    Private Sub copy_create_settings_into_memory()

        Try
            bc_cs_central_settings.show_all_entities = bc_am_load_objects.obc_prefs.create_settings.show_all_entities
            bc_cs_central_settings.my_entities_default = bc_am_load_objects.obc_prefs.create_settings.my_entities_default

            REM Steve Wooderson Drexel 27/01/2014 Show prefs only
            bc_cs_central_settings.show_all_entities_submit = bc_am_load_objects.obc_prefs.create_settings.show_all_entities_submit
            bc_cs_central_settings.my_entities_default_submit = bc_am_load_objects.obc_prefs.create_settings.my_entities_default_submit

            bc_cs_central_settings.alt_entity_for_build = bc_am_load_objects.obc_prefs.create_settings.alt_entity_for_build
            bc_cs_central_settings.alt_entity_for_apref = bc_am_load_objects.obc_prefs.create_settings.alt_entity_for_apref
            bc_cs_central_settings.alt_entity_for_submit = bc_am_load_objects.obc_prefs.create_settings.alt_entity_for_submit

            bc_cs_central_settings.merges = bc_am_load_objects.obc_prefs.create_settings.merges
            bc_cs_central_settings.custom_library = bc_am_load_objects.obc_prefs.create_settings.custom_library

            bc_cs_central_settings.use_advanced_composite_build = bc_am_load_objects.obc_prefs.create_settings.use_advanced_composite_build

            bc_cs_central_settings.search_pub_type_list = bc_am_load_objects.obc_prefs.create_settings.search_pub_type_list
            bc_cs_central_settings.document_scan_namespace = bc_am_load_objects.obc_prefs.create_settings.document_scan_namespace
            bc_cs_central_settings.document_scan_node = bc_am_load_objects.obc_prefs.create_settings.document_scan_node
            bc_cs_central_settings.deny_backdated_submission = bc_am_load_objects.obc_prefs.create_settings.deny_backdated_submission
            bc_cs_central_settings.show_summary_text = bc_am_load_objects.obc_prefs.create_settings.show_summary_text
            bc_cs_central_settings.show_teaser_text = bc_am_load_objects.obc_prefs.create_settings.show_teaser_text
            bc_cs_central_settings.summary_text_display_name = bc_am_load_objects.obc_prefs.create_settings.summary_text_display_name
            bc_cs_central_settings.teaser_text_display_name = bc_am_load_objects.obc_prefs.create_settings.teaser_text_display_name
            REM FIL Nov 2013 JIRA 4913
            bc_cs_central_settings.allow_support_import = bc_am_load_objects.obc_prefs.create_settings.allow_support_import
            bc_cs_central_settings.show_keep_open_check_out = bc_am_load_objects.obc_prefs.create_settings.show_keep_open_check_out

            bc_cs_central_settings.run_time_pref_list = bc_am_load_objects.obc_prefs.create_settings.run_time_pref_list
        Catch
            REM create settings could not be read
            bc_cs_central_settings.show_all_entities = False
            bc_cs_central_settings.my_entities_default = True
            bc_cs_central_settings.alt_entity_for_build = False
            bc_cs_central_settings.alt_entity_for_apref = False
            bc_cs_central_settings.alt_entity_for_apref = False
            bc_cs_central_settings.show_summary_text = False
            bc_cs_central_settings.show_teaser_text = False
            bc_cs_central_settings.show_keep_open_check_out = False
        End Try
    End Sub

    REM checks if a synchronize is required
    Private Function check_sync(ByRef ocurrent_user As bc_om_user) As Integer

        Dim slog As New bc_cs_activity_log("bc_am_load", "check_sync", bc_cs_activity_codes.TRACE_ENTRY, "")


        check_sync = False
        REM if system has never been syncronized check now
        REM this can be done 
        REM if system has never been synchronized check now
        REM this can be done by checkig for existance of 
        Dim fs As New bc_cs_data_services
        If fs.soap_check_deserialize_file_to_object(bc_cs_central_settings.local_template_path + "bc_pub_types.dat", Nothing) = False Then
            check_sync = 1
            Exit Function
        End If

        REM Mac address check fix May 2016
        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
            ocurrent_user.tmode = bc_om_user.tGET_SYNC_LEVEL
            If ocurrent_user.transmit_to_server_and_receive(ocurrent_user, True) = False Then
                Exit Function
            End If
            ocurrent_user.tmode = bc_cs_soap_base_class.tREAD
        Else
            ocurrent_user.get_sync_level()
        End If

        If ocurrent_user.sync_level > 0 Then
            check_sync = 1
            REM FIL JUNE 2013
        ElseIf ocurrent_user.sync_settings.Count > 0 Then
            check_sync = 2
        End If


        slog = New bc_cs_activity_log("bc_am_load", "check_sync", bc_cs_activity_codes.TRACE_EXIT, "")

    End Function
    REM checks if user has valid logon to server
    Public Function check_logon(ByRef ocurrent_user As bc_om_user) As Integer
        Dim slog As New bc_cs_activity_log("bc_am_load", "check_logon", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            Dim ousers As New bc_om_users
            Dim i As Integer
            Dim j As Integer
            Dim ocommentary As bc_cs_activity_log

            check_logon = 0
            REM check method
            If bc_cs_central_settings.show_authentication_form = 1 Then
                bc_cs_central_settings.progress_bar.increment("Authenticating User...")
                'If bc_cs_central_settings.user_authentication_method = "AD" Then
                '    ' retrieve the current windows logon token
                '    bc_cs_central_settings.user_token = LogonUser()
                '    bc_cs_central_settings.user_name = bc_cs_central_settings.logged_on_user_name
                '    check_logon = authentication_form(ocurrent_user)
                '    If Not bc_cs_central_settings.user_token.Equals(IntPtr.Zero) Then
                '        CloseHandle(bc_cs_central_settings.user_token)
                '    End If
                '    bc_cs_central_settings.user_token = IntPtr.Zero
                'End If
                If check_logon <> 1 Then
                    For j = 1 To bc_cs_central_settings.authentication_retries

                        check_logon = authentication_form(ocurrent_user)
                        If check_logon <> 0 Then
                            Exit For
                        End If
                        bc_cs_central_settings.user_name = ""
                        bc_cs_central_settings.user_password = ""


                        If j = 3 Then
                            check_logon = -1
                        End If
                    Next
                End If

                Exit Function
            Else
                'bc_cs_central_settings.progress_bar.increment("Authenticating User...")
                REM load users
                If current_application = "Common Platform" Then
                    ousers.read_partial_sync = True
                End If


                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    ousers.db_read()
                ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                    REM get users via soap request
                    ousers.tmode = bc_cs_soap_base_class.tREAD
                    If ousers.transmit_to_server_and_receive(ousers, False) = False Then
                        check_logon = -1
                    End If
                Else
                    REM invalid connection method
                    Exit Function
                End If

                If Not ousers Is Nothing And check_logon <> -1 Then
                    REM store in global memory 
                    Dim obcobjects As New bc_am_load_objects
                    bc_am_load_objects.obc_users = ousers
                    REM write out to file
                    For i = 0 To ousers.user.Count - 1
                        ousers.user(i).password = ""
                    Next

                    REM black password
                    ousers.write_data_to_file(bc_cs_central_settings.local_template_path + bc_am_load_objects.USERS_FILENAME)
                    check_logon = ousers.check_logon(ocurrent_user)



                End If
                If check_logon = 0 Or check_logon = -1 Then
                    If (bc_cs_central_settings.show_authentication_form = 0 Or bc_cs_central_settings.show_authentication_form = 2) Then
                        ocommentary = New bc_cs_activity_log("bc_am_load", "check_logon", bc_cs_activity_codes.COMMENTARY, "Authentication Failed for: " + bc_cs_central_settings.logged_on_user_name)
                    Else
                        ocommentary = New bc_cs_activity_log("bc_am_load", "check_logon", bc_cs_activity_codes.COMMENTARY, "Authentication Failed for: " + bc_cs_central_settings.user_name)
                    End If
                End If
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_load", "check_logon", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_load", "check_logon", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
    Private Function authentication_form(ByRef ocurrent_user As bc_om_user) As Integer
        Dim slog As New bc_cs_activity_log("bc_am_load", "authentication_form", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try

            'Dim logon_form As New bc_cs_logon_form
            Dim logon_form As New bc_dx_logon
            Dim clogon_form As New Cbc_dx_logon(logon_form)

            Dim authentication_success As Boolean = False
            Dim ocommentary As bc_cs_activity_log
            Dim omessage As bc_cs_message
            Dim lastUser As New bc_om_user

            Dim ousers As New bc_om_users



            bc_cs_central_settings.progress_bar.hide()
            If bc_cs_central_settings.user_password = "" And bc_cs_central_settings.user_name = "" Then
                logon_form.ShowDialog()
                bc_cs_central_settings.progress_bar.show()
                bc_cs_central_settings.progress_bar.refresh()
                If clogon_form._ok = False Then
                    REM cancel
                    authentication_form = 2
                    ocommentary = New bc_cs_activity_log("bc_am_load", "check_logon", bc_cs_activity_codes.COMMENTARY, "Cancel Selected on User Authentication form.")
                    Exit Function
                End If
                bc_cs_central_settings.user_name = clogon_form._username

                bc_cs_central_settings.user_password = clogon_form._password
            End If

            If bc_cs_central_settings.user_authentication_method = "encrypted" Then
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    ousers.HashPassword()
                End If
            End If
            'End If

            REM load users
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                ousers.db_read()
            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                REM get users via soap request
                ousers.tmode = bc_cs_soap_base_class.tREAD
                If ousers.transmit_to_server_and_receive(ousers, False) = False Then
                    authentication_success = False
                    Dim omsg As New bc_cs_message("Blue Curve", "Authentication Failed for User", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Function
                End If
            Else
                REM invalid connection method
                Exit Function
            End If

            If Not IsNothing(ousers) Then
                REM store in global memory 


                authentication_success = ousers.check_logon(ocurrent_user)

                REM blank out passwords
                Dim i As Integer
                For i = 0 To ousers.user.Count - 1
                    ousers.user(i).password = ""
                Next
                REM write out to file
                ousers.write_data_to_file(bc_cs_central_settings.local_template_path + bc_am_load_objects.USERS_FILENAME)
                bc_am_load_objects.obc_users = ousers

            End If

            If authentication_success = False Then
                ocommentary = New bc_cs_activity_log("bc_am_load", "check_logon", bc_cs_activity_codes.COMMENTARY, "Authentication Failed for: " + bc_cs_central_settings.user_name)
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    omessage = New bc_cs_message("Blue Curve", "Authentication failed for:" + bc_cs_central_settings.user_name, bc_cs_message.MESSAGE)
                End If
            Else
                authentication_form = 1
                authentication_success = True
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_load", "authentication_form", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_load", "authentication_form", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function

    ' Retrieve the account token from the current WindowsIdentity object
    ' instead of calling the unmanaged LogonUser method in the advapi32.dll.
    Private Function LogonUser() As IntPtr
        Dim accountToken As IntPtr = WindowsIdentity.GetCurrent().Token

        Return accountToken
    End Function

End Class
REM ==========================================
REM Blue Curve Limited 2005
REM Module:       Load Objects
REM Type:         AM
REM Desciption:   Class to Hold System data in Memory
REM               on instantiation gives access to all system 
REM               held in global memory or can load all system 
REM               data into memory
REM Methods:      New
REM Version:      1
REM Change history
REM ==========================================
Public Class bc_am_load_objects
    REM successful load flag
    Public Shared bloadsuccess As Boolean

    REM users
    Public Shared obc_users As New bc_om_users
    Public Const USERS_FILENAME = "bc_users.dat"

    Public Shared obc_prefs As New bc_om_user_prefs
    Public Const PREFS_FILENAME = "bc_prefs.dat"

    REM pub types
    Public Shared obc_pub_types As New bc_om_pub_types
    Public Const PUB_TYPES_FILENAME = "bc_pub_types.dat"

    REM templates
    Public Shared obc_templates As New bc_om_templates
    Public Const TEMPLATES_FILENAME = "bc_templates.dat"

    REM properties
    Public Shared obc_doc_properties As New bc_om_doc_properties
    Public Const PROPERTIES_FILENAME = "bc_properties.dat"

    REM entities
    Public Shared obc_entities As New bc_om_entities
    Public Const ENTITIES_FILENAME = "bc_entities.dat"

    REM entity inks
    Public Shared obc_entity_links As New bc_om_entity_links
    Public Const ENTITY_LINKS_FILENAME = "bc_entity_links.dat"

    REM current document
    Public Shared obc_current_document As New bc_om_document
    REM documents
    Public Shared obc_documents As New bc_om_documents
    REM local document flag
    Public Shared local_document As Boolean
    REM refresh object
    Public Shared obc_refresh_components As New bc_om_refresh_components
    REM isnight workbooks
    Public Shared obc_workbooks As New bc_om_insight_contribution_for_entities

    REM Insight Specific Objects
    Public Shared obc_om_insight_submission_entity_links As New bc_om_insight_submission_entity_links
    Public Const INSIGHT_LINKS_FILENAME = "bc_insight_links.dat"

    REM insight contribution for entity
    Public Shared obc_om_insight_contribution_for_entity As bc_om_insight_contribution_for_entity

    Public Shared obc_in_ao_object As Object

    REM custom period section
    Public Shared obc_om_custom_period_section As New bc_om_insight_section

    REM custom static section
    Public Shared obc_om_custom_static_section As New bc_om_insight_section

    REM current compoiste
    Public Shared obc_om_composite_pub_type As New bc_om_pub_type

    Public Sub New()

    End Sub
    REM constructor when assigning when loaded previously from another method 
    REM on when only partial load is required
    Public Sub New(ByVal dummybool As Boolean)

    End Sub
    REM constructor when loading from XML on file system into global memory
    Public Sub New(ByVal strpathname As String, ByVal application As String)
        Dim slog = New bc_cs_activity_log("bc_am_load_objects", "new", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim ocommentary As bc_cs_activity_log

        Try
            If application = "Authenticate Only" Then
                Exit Sub
            End If
            Dim bc_cs_central_settings As New bc_cs_central_settings
            bloadsuccess = False
            REM load pub types
            If application = "Create" Or application = "Create with Offline Working" Then

                ocommentary = New bc_cs_activity_log("bc_am_load_objects", "New", bc_cs_activity_codes.COMMENTARY, "Attempting to Load Pub Types")
                obc_pub_types = obc_pub_types.read_data_from_file(bc_cs_central_settings.local_template_path + PUB_TYPES_FILENAME)
                If Not IsNothing(obc_pub_types) Then
                    REM load templates
                    ocommentary = New bc_cs_activity_log("bc_am_load_objects", "New", bc_cs_activity_codes.COMMENTARY, "Attempting to Load Templates")
                    obc_templates = obc_templates.read_data_from_file(bc_cs_central_settings.local_template_path + TEMPLATES_FILENAME)
                    If Not IsNothing(obc_templates) Then
                        REM properties
                        ocommentary = New bc_cs_activity_log("bc_am_load_objects", "New", bc_cs_activity_codes.COMMENTARY, "Attempting to Load Properties")
                        obc_doc_properties = obc_doc_properties.read_data_from_file(bc_cs_central_settings.local_template_path + PROPERTIES_FILENAME)
                        If Not IsNothing(obc_doc_properties) Then
                            REM users 
                            ocommentary = New bc_cs_activity_log("bc_am_load_objects", "New", bc_cs_activity_codes.COMMENTARY, "Attempting to Load Users")
                            obc_users = obc_users.read_data_from_file(bc_cs_central_settings.local_template_path + USERS_FILENAME)
                            If Not IsNothing(obc_users) Then
                                REM entities
                                ocommentary = New bc_cs_activity_log("bc_am_load_objects", "New", bc_cs_activity_codes.COMMENTARY, "Attempting to Load Entities")
                                obc_entities = obc_entities.read_data_from_file(bc_cs_central_settings.local_template_path + ENTITIES_FILENAME)
                                If Not IsNothing(obc_entities) Then
                                    REM prefs
                                    ocommentary = New bc_cs_activity_log("bc_am_load_objects", "New", bc_cs_activity_codes.COMMENTARY, "Attempting to Load Prefs")
                                    obc_prefs = obc_prefs.read_data_from_file(bc_cs_central_settings.local_template_path + PREFS_FILENAME)
                                    If Not IsNothing(obc_prefs) Then
                                        REM entity links
                                        ocommentary = New bc_cs_activity_log("bc_am_load_objects", "New", bc_cs_activity_codes.COMMENTARY, "Attempting to Load Entity Links")
                                        obc_entity_links = obc_entity_links.read_data_from_file(bc_cs_central_settings.local_template_path + ENTITY_LINKS_FILENAME)
                                        REM all has loaded successfully
                                        bloadsuccess = True
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            End If
            If application = "Insight" Or application = "Excel Functions" Then
                REM entities
                ocommentary = New bc_cs_activity_log("bc_am_load_objects", "New", bc_cs_activity_codes.COMMENTARY, "Attempting to Load Entities...")
                obc_entities = obc_entities.read_data_from_file(bc_cs_central_settings.local_template_path + ENTITIES_FILENAME)
                REM insight links
                If Not IsNothing(obc_entities) Then
                    'ocommentary = New bc_cs_activity_log("bc_am_load_objects", "New", bc_cs_activity_codes.COMMENTARY, "Attempting to Load Insight Submission Entity Links...")
                    'obc_om_insight_submission_entity_links = obc_om_insight_submission_entity_links.read_xml_from_file(bc_cs_central_settings.local_template_path + INSIGHT_LINKS_FILENAME)
                    'If Not IsNothing(obc_om_insight_submission_entity_links) Then
                    ocommentary = New bc_cs_activity_log("bc_am_load_objects", "New", bc_cs_activity_codes.COMMENTARY, "Attempting to Load Preferences...")
                    obc_prefs = obc_prefs.read_data_from_file(bc_cs_central_settings.local_template_path + PREFS_FILENAME)
                    If Not IsNothing(obc_prefs) Then
                        REM AUTO AUG 2013
                        ocommentary = New bc_cs_activity_log("bc_am_load_objects", "New", bc_cs_activity_codes.COMMENTARY, "Attempting to Load Pub Types")
                        obc_pub_types = obc_pub_types.read_data_from_file(bc_cs_central_settings.local_template_path + PUB_TYPES_FILENAME)
                        bloadsuccess = True
                    End If
                    'End If
                End If
                REM load config file
            End If
            If application = "Workflow" Then


            End If
            If application = "Insight Config" Or application = "Common Platform" Then
                bloadsuccess = True
            End If
            If application = "BluePrint" Then
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                    REM rem read in via SOAP
                    ocommentary = New bc_cs_activity_log("bc_am_load_objects", "new", bc_cs_activity_codes.COMMENTARY, "Loading Publication Types from via SOAP")
                    obc_pub_types.tmode = bc_cs_soap_base_class.tREAD
                    obc_pub_types.transmit_to_server_and_receive(obc_pub_types, True)
                    ocommentary = New bc_cs_activity_log("bc_am_load_objects", "new", bc_cs_activity_codes.COMMENTARY, "Loading Properties via SOAP")
                    obc_doc_properties.tmode = bc_cs_soap_base_class.tREAD
                    obc_doc_properties.transmit_to_server_and_receive(obc_doc_properties, True)
                    ocommentary = New bc_cs_activity_log("bc_am_load_objects", "new", bc_cs_activity_codes.COMMENTARY, "Loading Templates via SOAP")
                    obc_templates.tmode = bc_cs_soap_base_class.tREAD
                    obc_templates.transmit_to_server_and_receive(obc_templates, True)
                Else
                    REM read in directly from database
                    REM pub types
                    ocommentary = New bc_cs_activity_log("bc_am_load_objects", "new", bc_cs_activity_codes.COMMENTARY, "Loading Publication Types from Database")
                    obc_pub_types.db_read()
                    REM properties
                    ocommentary = New bc_cs_activity_log("bc_am_load_objects", "new", bc_cs_activity_codes.COMMENTARY, "Loading Properties from Database")
                    obc_doc_properties.db_read()
                    REM templates
                    ocommentary = New bc_cs_activity_log("bc_am_load_objects", "new", bc_cs_activity_codes.COMMENTARY, "Loading Templates from Database")
                    obc_templates.db_read()
                End If

                Dim strfn As String
                strfn = bc_cs_central_settings.local_template_path + PUB_TYPES_FILENAME
                ocommentary = New bc_cs_activity_log("bc_am_load_objects", "new", bc_cs_activity_codes.COMMENTARY, "Writing Publication Types")
                Dim ierr As Integer
                ierr = obc_pub_types.write_data_to_file(strfn)

                strfn = bc_cs_central_settings.local_template_path + TEMPLATES_FILENAME
                ocommentary = New bc_cs_activity_log("bc_am_load_objects", "new", bc_cs_activity_codes.COMMENTARY, "Writing Templates")
                ierr = obc_templates.write_data_to_file(strfn)

                strfn = bc_cs_central_settings.local_template_path + PROPERTIES_FILENAME
                ocommentary = New bc_cs_activity_log("bc_am_load_objects", "new", bc_cs_activity_codes.COMMENTARY, "Writing Properties")
                ierr = obc_doc_properties.write_data_to_file(strfn)

                bloadsuccess = True
            End If
            If application = "BluePrint API" Then
                bloadsuccess = True
            End If
            'If application = "Excel Functions" Then
            '    bloadsuccess = True
            'End If
            'allred
            If application = "Deployment" Then
                bloadsuccess = True
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_load_objects", "new", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_load_objects", "new", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
End Class
Public Class bc_am_deploy
    Public user_id As Long
    Public update As Boolean
    Public Sub deploy()
        Dim slog As New bc_cs_activity_log("bc_am_deploy", "deploy", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            update = False
            Dim oassemblies As New bc_om_release
            Dim i As Integer
            Dim pa As String
            Dim fs As New bc_cs_file_transfer_services

            oassemblies.user_id = user_id
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                oassemblies.db_read()
            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                oassemblies.tmode = bc_cs_soap_base_class.tREAD
                If oassemblies.transmit_to_server_and_receive(oassemblies, True) = False Then
                    Exit Sub
                End If
            Else
                Exit Sub
            End If
            If oassemblies.load_error <> "" Then
                Dim ocommentary As New bc_cs_activity_log("bc_am_deploy", "deploy", bc_cs_activity_codes.COMMENTARY, oassemblies.load_error)
                Exit Sub
            End If
            If oassemblies.release_number > 0 Then
                bc_cs_central_settings.progress_bar.increment("Upgrading System...")
                For i = 0 To oassemblies.assemblies.Count - 1

                    Dim ocommentary As New bc_cs_activity_log("bc_am_deploy", "deploy", bc_cs_activity_codes.COMMENTARY, "Deploying: " + oassemblies.assemblies(i).name + "; type: " + CStr(oassemblies.assemblies(i).type))
                    pa = bc_cs_central_settings.get_user_dir
                    If Right(pa, 1) <> "\" Then
                        pa = pa + "\"
                    End If
                    pa = pa + oassemblies.assemblies(i).name
                    Select Case oassemblies.assemblies(i).type
                        Case 1
                            REM GAC
                            Dim opa As String
                            opa = pa
                            fs.write_bytestream_to_document(pa, oassemblies.assemblies(i).assembly_file, Nothing)
                            REM move it to GAC
                            Dim cmd As String
                            If fs.in_use_by_another_process(pa, Nothing) = False Then
                                opa = """" + pa + """"
                                cmd = "c:\windows\Microsoft.NET\Framework\v1.1.4322\gacutil /i " + opa
                                ocommentary = New bc_cs_activity_log("bc_am_deploy", "deploy", bc_cs_activity_codes.COMMENTARY, cmd)
                                Shell(cmd, AppWinStyle.Hide)
                            Else
                                ocommentary = New bc_cs_activity_log("bc_am_deploy", "deploy", bc_cs_activity_codes.COMMENTARY, "Didnt deploy " + opa + " as in use")
                            End If
                            REM now remove this copy
                            Thread.Sleep(1200)
                            fs.delete_file(pa)
                        Case 2
                            REM tlb file
                            fs.write_bytestream_to_document(pa, oassemblies.assemblies(i).assembly_file, Nothing)
                        Case 3
                            REM file to application area
                            REM file could be in use so check ths before update
                            If fs.write_bytestream_to_document(pa, oassemblies.assemblies(i).assembly_file, Nothing) = False Then
                                ocommentary = New bc_cs_activity_log("bc_am_deploy", "deploy", bc_cs_activity_codes.COMMENTARY, "Didnt deploy " + pa + " as in use")
                            End If

                        Case 4
                            REM file to application area
                            fs.write_bytestream_to_document(pa, oassemblies.assemblies(i).assembly_file, Nothing)
                    End Select
                Next
                REM save down configuration file
                Dim da As Date
                da = Now
                oassemblies.client_deployment_date = da
                Dim user_dir As String
                user_dir = bc_cs_central_settings.get_user_dir
                If Right(user_dir, 1) <> "\" Then
                    user_dir = user_dir + "\"
                End If
                'oassemblies.write_xml_to_file(user_dir + CStr(Me.user_id) + "_bc_version_" + CStr(da.Year) + CStr(da.Month) + CStr(da.Day) + CStr(da.Hour) + CStr(da.Minute) + CStr(da.Second) + ".dat")
                oassemblies.write_data_to_file(user_dir + CStr(Me.user_id) + "_bc_version.dat")
                REM flag upgrade has been successful
                For i = 0 To oassemblies.assemblies.Count - 1
                    oassemblies.assemblies(i).assembly_file = Nothing
                Next
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    oassemblies.db_write()
                ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                    oassemblies.tmode = bc_cs_soap_base_class.tWRITE
                    oassemblies.no_send_back = True
                    If oassemblies.transmit_to_server_and_receive(oassemblies, True) = False Then
                        Exit Sub
                    End If
                Else
                    Exit Sub
                End If
                REM tell user system will need a restart as update has occured
                update = True
            Else
                Dim ocommentary As New bc_cs_activity_log("bc_am_deploy", "deploy", bc_cs_activity_codes.COMMENTARY, "No Deployment Requested")
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_deploy", "deploy", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_deploy", "deploy", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Public Sub New(ByVal user_id As Long)
        Me.user_id = user_id
    End Sub
End Class

