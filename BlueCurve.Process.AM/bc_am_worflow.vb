Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports BlueCurve.Core.AS
Imports BlueCurve.Create.AM
Imports System.Collections
Imports System.Threading
Imports System.Windows.Forms.Screen
Imports System.io
Imports Microsoft.Win32
Imports System.Windows.forms
Imports System.Diagnostics


Public Class bc_am_workflow
    Public Const WORKFLOW_SETTINGS As String = "bc_am_workflow_settings.dat"
    Public Shared polling_interval As Long
    Public Shared polling_enabled As Boolean
    Public Shared alerter_enabled As Boolean
    Public Shared screen_update_enabled As Boolean
    Public Shared screen_poll As Boolean

    REM if user mode is async server then start the server
    Public Shared ae As bc_am_asyncronous_events
    Public Shared fa As bc_am_aync_doc_failure_alert


    Public Shared user_inactive_interval As Long
    Public Shared user_inactive_last As DateTime

    Public Shared docs As New bc_om_documents
    Public Shared pdocs As New bc_om_documents
    Public Shared ndocs As New ArrayList
    Public Shared edocs As New ArrayList
    Public Shared udocs As New ArrayList
    Public Shared stdocs As New ArrayList
    Public Shared snew As Boolean
    Public Shared processing As Boolean
    Public Shared pre_expire_alert_notify As Integer
    Public Shared ofrm As bc_am_workflow_frm
    Public Shared cfrm As bc_am_workflow_container_frm
    Public Shared filterpanelheight As Integer = 138
    Public Shared snapsummary As Boolean = True
    Private wstate As Integer
    Private olocation As Object
    Public Shared terminate_polling As Boolean
    Public Shared mode As Integer
    Public Shared new_form As Boolean
    Public Shared tpoll As Thread
    Public Shared tscreen As Thread
    Public Shared sdocs As New ArrayList
    Public Shared unread_mode As Boolean
    Public Shared beep_enabled As Boolean
    Public Shared fade_interval As Integer
    Public Shared selected_doc_id As String
    Public Shared auto_refresh As Boolean
    Public Shared days_back As Integer
    REM filters
    Public Shared lpub_types As New ArrayList
    Public Shared lstages As New ArrayList
    Public Shared lentity As New ArrayList
    Public Shared lauthor As New ArrayList
    Public Shared lbusarea As New ArrayList
    REM filter settings
    Public Shared otaxonomies As New ArrayList
    Public Shared spubtype_id As New ArrayList
    Public Shared sentity_id As New ArrayList
    Public Shared sauthor_id As New ArrayList
    Public Shared sbusarea_id As New ArrayList
    Public Shared sstage As New ArrayList
    Public Shared curr_doc As New bc_om_document
    REM selected filter settings 
    REM soon to be removed
    REM =================================
    Public Shared fdatefrom As New Date
    Public Shared fdateto As New Date
    Public Shared last_screen_refresh As New Date
    Public Shared from_screen As Boolean
    Public Shared screen_refresh_interval As Integer
    Public Shared screen_inactive_interval As Integer
    Public Shared loading As Boolean
    Public Shared alert_doc_id
    Public Shared alert_mode
    Public Shared polling_process As Boolean
    Public Shared matches_only As Boolean
    Public odoc As New bc_am_at_open_doc
    Public new_stage
    Public new_stage_name
    Public oalertfrm As bc_am_workflow_alert
    Public msg As String
    Public Shared from_service As Boolean = False
    Public Shared colors As New ProcessColors
    Public Shared summary_pinned As Boolean
    Public Shared summary_displayed As Boolean
    Public Shared filters_displayed As Boolean
    Public Shared oopen_doc As bc_am_at_open_doc



    Public Sub New()

    End Sub
    Public Shared Sub terminate_threads()

        If bc_am_load_objects.obc_prefs.process_events_mode = process_client_events.ASYNC_SERVER Then
            ae.abort_thread()
        End If
        REM if async failures set up failure alerter

        If bc_am_load_objects.obc_prefs.process_events_mode <> process_client_events.SYNC Then
            fa.abort_thread()
        End If
    End Sub
    Public Sub load()
        Dim slog As New bc_cs_activity_log("bc_am_workflow", "load", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim i As Integer
            set_defaults()
            Dim ows As New bc_om_workflow_settings

            REM load user settings if set else use defaults
            Dim fs As New bc_cs_file_transfer_services
            If fs.check_document_exists_and_can_be_read(bc_cs_central_settings.get_user_dir + "\" + CStr(bc_cs_central_settings.logged_on_user_id) + "_" + WORKFLOW_SETTINGS) = True Then
                ows = New bc_om_workflow_settings
                ows = ows.read_data_from_file(bc_cs_central_settings.get_user_dir + "\" + CStr(bc_cs_central_settings.logged_on_user_id) + "_" + WORKFLOW_SETTINGS)
                ows.turned_on = True
                ows.write_data_to_file(bc_cs_central_settings.get_user_dir + "\" + CStr(bc_cs_central_settings.logged_on_user_id) + "_" + WORKFLOW_SETTINGS)
                bc_am_workflow.polling_interval = ows.polling_interval
                bc_am_workflow.filterpanelheight = ows.filterpanelheight
                bc_am_workflow.snapsummary = ows.snapmode
                bc_am_workflow.user_inactive_interval = ows.user_inactive_interval

                bc_am_workflow.polling_enabled = UCase(ows.polling_enabled)
                bc_am_workflow.alerter_enabled = UCase(ows.alerter_enabled)
                bc_am_workflow.pre_expire_alert_notify = ows.pre_expire_alert_notify
                bc_am_workflow.screen_poll = UCase(ows.screen_update)
                If bc_am_workflow.from_service = False Then
                    bc_am_workflow.mode = ows.mode
                End If
                bc_am_workflow.unread_mode = ows.unread_mode
                bc_am_workflow.beep_enabled = ows.beep_enabled
                bc_am_workflow.fade_interval = ows.fade_interval
                bc_am_workflow.auto_refresh = CBool(ows.auto_refresh)
                bc_am_workflow.sentity_id = ows.sentity
                bc_am_workflow.sauthor_id = ows.sauthor
                bc_am_workflow.sstage = ows.sstage
                bc_am_workflow.spubtype_id = ows.spubtype
                bc_am_workflow.sbusarea_id = ows.sbus
                bc_am_workflow.days_back = ows.days_back

                Dim t As New TimeSpan(bc_am_workflow.days_back, 0, 0, 0, 0)
                bc_am_workflow.fdatefrom = Now.Subtract(t)
                'Me.fdatefrom = ows.fdatefrom
                'Me.fdateto = ows.fdateto

                bc_am_workflow.screen_refresh_interval = ows.screen_refresh_interval
                bc_am_workflow.screen_inactive_interval = ows.screen_inactive_interval
                bc_am_workflow.colors.loadFromConfig(ows.colors)
                bc_am_workflow.pdocs.document.Clear()
                For i = 0 To ows.ndocs.document.Count - 1
                    bc_am_workflow.pdocs.document.Add(ows.ndocs.document(i))
                Next

            End If
            If bc_am_workflow.from_service = True Then
                REM take polling settings from bc config
                bc_am_workflow.fade_interval = bc_cs_central_settings.alert_interval / 1000
                bc_am_workflow.polling_enabled = True
                bc_am_workflow.alerter_enabled = True
                bc_am_workflow.polling_interval = bc_cs_central_settings.poll_interval
                Dim da As New DateTime
                da = Now
                Dim tp As TimeSpan
                tp = New TimeSpan(bc_cs_central_settings.service_poll_date_range, 0, 0, 0, 0)
                bc_am_workflow.fdatefrom = da.Subtract(tp)
            End If
            REM if service poll running then disable client poller settings

            If System.Diagnostics.Process.GetProcessesByName("BlueCurveService").GetLength(0) > 0 Then
                If bc_cs_central_settings.service_poll_enabled = 0 Then
                    bc_am_workflow.polling_enabled = False
                    bc_am_workflow.alerter_enabled = False
                Else
                    bc_am_workflow.polling_enabled = True
                    bc_am_workflow.alerter_enabled = True
                End If
                bc_am_workflow.polling_interval = bc_cs_central_settings.poll_interval
                bc_am_workflow.fade_interval = bc_cs_central_settings.alert_interval / 1000
                bc_am_workflow.user_inactive_interval = bc_cs_central_settings.user_inactive_interval
            End If


            bc_am_workflow.polling_process = False
            tpoll = New Thread(AddressOf doclistpoll)
            tpoll.Priority = ThreadPriority.AboveNormal
            tpoll.Start()

            bc_am_workflow.snew = True
            cfrm = New bc_am_workflow_container_frm
            cfrm.columnorder = ows.columnorder
            cfrm.columnwidth = ows.columnwidth
            cfrm.WindowState = FormWindowState.Maximized
            set_polling_status()

            REM if user mode is async server then start the server
            If bc_am_load_objects.obc_prefs.process_events_mode = process_client_events.ASYNC_SERVER Then
                ae = New bc_am_asyncronous_events(2)
                ae.run_events()
                Dim ocomm As New bc_cs_activity_log("bc_am_workflow", "load", bc_cs_activity_codes.COMMENTARY, "Process working in Aync Server Mode", Nothing)
            End If
            REM if async failures set up failure alerter

            If bc_am_load_objects.obc_prefs.process_events_mode <> process_client_events.SYNC Then
                fa = New bc_am_aync_doc_failure_alert()
                fa.run()
            End If


            cfrm.ShowDialog()
            Try
                tpoll.Abort()
            Catch

            End Try

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_workflow", "load", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_workflow", "load", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Private Sub set_defaults()
        terminate_polling = False
        polling_interval = 10000
        polling_enabled = "FALSE"
        alerter_enabled = "FALSE"
        screen_poll = "FALSE"
        screen_update_enabled = True
        pre_expire_alert_notify = 4
        beep_enabled = False
        days_back = 1
        fade_interval = 5
        If bc_am_workflow.from_service = False Then
            mode = 0
        End If
        unread_mode = False
        bc_am_workflow.auto_refresh = True
        last_screen_refresh = "1-1-1900"
        Try
            Dim t As New TimeSpan(1, 0, 0, 0, 0)
            Try
                bc_am_workflow.cfrm.FilterDateTo.Value = Now.Add(t)
                bc_am_workflow.cfrm.FilterDateTo.Visible = False
            Catch

            End Try
            bc_am_workflow.fdateto = "9-9-9999"
            Try
                bc_am_workflow.cfrm.pdatefrom.Image = bc_am_workflow.cfrm.CheckBoxImages.Images(3)
                bc_am_workflow.cfrm.pdateto.Image = bc_am_workflow.cfrm.CheckBoxImages.Images(2)
            Catch

            End Try
            t = New TimeSpan(1, 0, 0, 0, 0)
            bc_am_workflow.fdatefrom = Now.Subtract(t)
            bc_am_workflow.cfrm.FilterDateFrom.Value = Now.Subtract(t)
            bc_am_workflow.cfrm.FilterDateFrom.Visible = True
            deselect()
            bc_am_workflow.filterpanelheight = 138
            bc_am_workflow.cfrm.FilterPanel.Height = 138
            bc_am_workflow.snapsummary = True
            set_polling_status()
        Catch

        End Try

        from_screen = False
        screen_refresh_interval = 5
        screen_inactive_interval = 60
        user_inactive_interval = 10
    End Sub
    Public Shared Sub write_user_settings_fo_file(Optional ByVal turned_on As Boolean = False)
        Dim slog As New bc_cs_activity_log("bc_am_workflow", "close", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            terminate_threads()
            REM write down system settings prior to closing app
            Dim ows As New bc_om_workflow_settings
            ows.turned_on = turned_on
            ows.polling_enabled = polling_enabled
            ows.polling_interval = polling_interval
            ows.alerter_enabled = alerter_enabled
            ows.screen_update = screen_poll
            ows.user_inactive_interval = user_inactive_interval

            ows.filterpanelheight = bc_am_workflow.filterpanelheight
            ows.snapmode = bc_am_workflow.snapsummary
            ows.pre_expire_alert_notify = pre_expire_alert_notify
            ows.ndocs.document.Clear()
            ows.ndocs = docs
            ows.mode = mode
            ows.unread_mode = unread_mode
            ows.beep_enabled = beep_enabled
            ows.fade_interval = fade_interval
            ows.auto_refresh = auto_refresh
            ows.sentity = sentity_id
            ows.sauthor = sauthor_id
            ows.sstage = sstage
            ows.spubtype = spubtype_id
            ows.sbus = sbusarea_id
            ows.days_back = days_back
            ows.fdatefrom = fdatefrom
            ows.fdateto = fdateto
            ows.screen_refresh_interval = screen_refresh_interval
            ows.screen_inactive_interval = screen_inactive_interval
            ows.colors = colors.convertForConfig
            For i = 0 To ofrm.DocumentList.Columns.Count - 1
                ows.columnorder.Add(ofrm.DocumentList.Columns(i).DisplayIndex)
            Next
            For i = 0 To ofrm.DocumentList.Columns.Count - 1
                ows.columnwidth.Add(ofrm.DocumentList.Columns(i).Width)
            Next
            ows.write_data_to_file(bc_cs_central_settings.get_user_dir + "\" + CStr(bc_cs_central_settings.logged_on_user_id) + "_" + WORKFLOW_SETTINGS)
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_workflow", "load", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_workflow", "close", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Public Sub approve_doc(ByVal adoc As bc_om_document)
        Dim slog As New bc_cs_activity_log("bc_am_workflow", "approve_doc", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            adoc.certificate.user_id = bc_cs_central_settings.logged_on_user_id
            adoc.certificate.user_id = bc_cs_central_settings.logged_on_user_name
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                adoc.db_read()
            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                adoc.tmode = bc_cs_soap_base_class.tREAD
                adoc.transmit_to_server_and_receive(adoc, True)
            End If
            retrieve_docs(False)

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_workflow", "approve_doc", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_workflow", "approve_doc", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Public Function getDocForId(ByVal id As String) As bc_om_document
        Dim i As Integer

        getDocForId = Nothing

        For i = 0 To bc_am_workflow.docs.document.Count - 1
            If bc_am_workflow.docs.document(i).id = id Then
                getDocForId = bc_am_workflow.docs.document(i)
                Exit For
            End If
        Next
    End Function

    Public Sub change_stage(ByVal with_document As Boolean, ByVal stage As Long, ByVal stage_name As String, ByVal ldoc As bc_om_document)
        Dim slog As New bc_cs_activity_log("bc_am_workflow", "change_stage", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim tpoll As Boolean
        Try

            tpoll = bc_am_workflow.polling_enabled
            bc_am_workflow.polling_enabled = False
            bc_am_workflow.cfrm.cursor_wait()


            Dim doc_open As Boolean = False
            Dim weh As bc_am_workflow_events_handler = Nothing
            REM before stage change check out doc and
            REM make sure it hasnt recently befcome checked out elsewhere or
            REM changed stage
            REM TBD

            ldoc.bcheck_out = False
            ldoc.bwith_document = False
            ldoc.btake_revision = False

            Dim tstage_id As Long
            Dim tstage_name As String
            tstage_id = ldoc.stage
            tstage_name = ldoc.stage_name

            ldoc.stage = ldoc.original_stage
            Dim lcomment As String
            Dim lexpire As DateTime
            lcomment = ldoc.main_note
            lexpire = ldoc.stage_expire_date
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                ldoc.db_read()
            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                ldoc.tmode = bc_cs_soap_base_class.tREAD
                ldoc.transmit_to_server_and_receive(ldoc, True)
            End If
            If ldoc.id = -1 Then
                Dim omessage As New bc_cs_message("Blue Curve - process", "Another user has just changed the stage of this document you cant change stage and may no longer be able to view", bc_cs_message.MESSAGE)
                Me.retrieve_docs(False, False)
                Exit Sub
            End If
            If ldoc.id = 0 Then
                Dim omessage As New bc_cs_message("Blue Curve - process", "Another user has just checked out  this document you cant change stage", bc_cs_message.MESSAGE)
                Me.retrieve_docs(False, False)
                Exit Sub
            End If
            ldoc.stage = tstage_id
            ldoc.stage_name = tstage_name
            ldoc.main_note = lcomment
            ldoc.stage_expire_date = lexpire

            REM need to add actions to this soon
            ldoc.history.Clear()
            ldoc.support_documents.document.Clear()
            If ldoc.main_note <> "" And ldoc.main_note <> "No Transition" Then
                ldoc.history.Add("Stage Change Comment:" + ldoc.main_note)
                Dim oc As New bc_om_comment
                oc.user_id = bc_cs_central_settings.logged_on_user_id
                oc.stage_id = ldoc.stage
                oc.comment = ldoc.main_note
                ldoc.comments.comments.Clear()
                ldoc.comments.comments.Add(oc)

            End If
            'PR temp async fix
            ldoc.pending_mode = 0
            If ldoc.action_Ids.Count > 0 And bc_am_load_objects.obc_prefs.process_events_mode <> process_client_events.SYNC Then
                Dim omsg As New bc_cs_message("Blue Curve", "Client Side Event Handling is asyncronous so the stage change will be pending", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                ldoc.pending_mode = bc_om_document.PS
                ldoc.action_Ids.Clear()
            End If

            If ldoc.action_Ids.Count > 0 Then
                REM actions required prior to stage change
                weh = New bc_am_workflow_events_handler(Nothing, ldoc, True)
                weh.doc_open = False
                weh.stage_id_to = ldoc.stage
                weh.stage_name_to = ldoc.stage_name
                REM set these so open doenst detect a stage change
                ldoc.stage = ldoc.original_stage
                ldoc.stage_name = ldoc.original_stage_name
                weh.run_events(ldoc, False)
                If weh.success = True Then
                    ldoc.stage = weh.stage_id_to
                    ldoc.stage_name = weh.stage_name_to
                    REM if document was opened read save it
                    REM resave xml and doc
                    If weh.doc_open = True Then
                        REM remove this
                        'ldoc = ldoc.read_xml_from_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ".dat")
                        ldoc.stage = weh.stage_id_to
                        ldoc.stage_name = weh.stage_name_to
                        ldoc.write_data_to_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ".dat")
                        ldoc.docobject = weh.obj
                        ldoc.docobject.save()
                    End If
                Else
                    REM remove server side events
                    ldoc.server_side_events.events.Clear()
                    ldoc.main_note = "Stage Change Comment: change supressed due to event failure"
                    ldoc.stage = ldoc.original_stage
                    ldoc.stage_name = ldoc.original_stage_name
                    If weh.suppress_no_trans = False Then
                        Dim omessage As New bc_cs_message("Blue Curve - Workflow", "Transitional Events Failed so document will not change Stage. Contact System Administrator", bc_cs_message.MESSAGE)
                    End If
                    ldoc.main_note = "No Transition"
                    doc_open = False
                End If
                Try
                    bc_cs_central_settings.progress_bar.unload()
                Catch

                End Try

            End If
            If ldoc.action_Ids.Count > 0 Then
                If weh.doc_open = False Or weh.success = False Then
                    ldoc.register_only = True
                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        ldoc.db_write(Nothing)
                    ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                        ldoc.tmode = bc_cs_soap_base_class.tWRITE
                        ldoc.transmit_to_server_and_receive(ldoc, True)
                    End If
                    REM delete xml if exists
                    Dim fs As New bc_cs_file_transfer_services
                    If weh.doc_open = True Then
                        weh.close()
                        If fs.check_document_exists(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + "xml") = True Then
                            fs.delete_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + "xml")
                        End If
                        If fs.check_document_exists(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ldoc.extension) = True Then
                            fs.delete_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ldoc.extension)
                        End If
                    End If

                Else
                    ldoc.main_note = lcomment
                    ldoc.stage_expire_date = lexpire
                    ldoc.write_data_to_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ".dat")
                    REM events opened document so full submission is required
                    REM only call full submit on BC document
                    'Dim osubmit As New bc_am_at_submit(False, ldoc.docobject, "word", False, True, True, False, ldoc.id)
                    Dim osubmit As New bc_am_at_submit
                    osubmit.submit_after_stage_change(ldoc, ldoc.docobject, "word", False, True, False)


                End If
            Else
                ldoc.register_only = True
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    ldoc.db_write(Nothing)
                ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                    ldoc.tmode = bc_cs_soap_base_class.tWRITE
                    ldoc.transmit_to_server_and_receive(ldoc, True)
                End If
            End If
            If ldoc.server_side_events_failed <> "" Then
                Dim omsg As New bc_cs_message("Blue Curve", ldoc.server_side_events_failed, bc_cs_message.MESSAGE)
            End If

            REM reload
            Me.retrieve_docs(False, False)
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_workflow", "change_stage", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Try
                bc_cs_central_settings.progress_bar.hide()
            Catch

            End Try
            bc_am_workflow.polling_enabled = tpoll
            bc_am_workflow.cfrm.cursor_default()
            slog = New bc_cs_activity_log("bc_am_workflow", "change_stage", bc_cs_activity_codes.TRACE_EXIT, "auto: " + CStr(bc_am_workflow.auto_refresh))
        End Try
    End Sub
   

    REM EFG PATHC JULY 2012
    Public Sub import_master_document(ByVal ldoc As bc_om_document, Optional ByVal register_only As Boolean = False, Optional ByVal regular_report As Boolean = False)
        Dim slog As New bc_cs_activity_log("bc_am_workflow", "import_master_document", bc_cs_activity_codes.TRACE_ENTRY, "auto: " + CStr(bc_am_workflow.auto_refresh))
        Dim tpoll As Boolean
        Try
            Dim i As Integer
            Dim extensionsize As Integer

            Dim fs As New bc_cs_file_transfer_services
            Dim pub_type_id As Long
            Dim entity_id As Long
            Dim sub_entity_id As Long
            Dim onewdoc As bc_om_document
            Dim ocommentary As bc_cs_activity_log
            tpoll = bc_am_workflow.polling_enabled
            bc_am_workflow.polling_enabled = False

            REM show pub type dialogue
            Dim fwizard_main As New bc_am_at_wizard_main
            fwizard_main.from_workflow = True
            fwizard_main.attach_doc = True
            fwizard_main.bfrom_import = True
            fwizard_main.Text = "Blue Curve - import document"
            If regular_report = True Then
                fwizard_main.regular_report = True
                fwizard_main.Text = "Blue Curve - regular reports"
            End If
            If register_only = True Then
                fwizard_main.Text = "Blue Curve - register document"
            End If

            fwizard_main.ShowDialog()
            If fwizard_main.attach_selected = False Then
                Exit Sub
            End If

            If regular_report = False Then
                pub_type_id = fwizard_main.gpub_type_id
                entity_id = fwizard_main.entity_id
                sub_entity_id = fwizard_main.sub_entity_id
                fwizard_main = Nothing
                For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                    If bc_am_load_objects.obc_pub_types.pubtype(i).id = pub_type_id Then
                        With bc_am_load_objects.obc_pub_types.pubtype(i)
                            onewdoc = New bc_om_document(0, entity_id, CStr(bc_cs_central_settings.logged_on_user_id), pub_type_id, "", "", Now, 0, .language, bc_cs_central_settings.selected_conn_method, "", .name, 0, False, "1", "", .bus_area_id, True, 0, "Draft", "9-9-9999", "", 0)
                            If sub_entity_id <> 0 Then
                                Dim oent As New bc_om_taxonomy
                                oent.entity_id = sub_entity_id
                                onewdoc.taxonomy.Add(oent)



                            End If
                            onewdoc.refresh_components.workflow_state = bc_am_load_objects.obc_pub_types.pubtype(i).default_financial_workflow_stage
                            onewdoc.refresh_components.accounting_standard = 1
                            If bc_am_load_objects.obc_pub_types.pubtype(i).workflow.stages.count > 0 Then
                                onewdoc.stage = bc_am_load_objects.obc_pub_types.pubtype(i).workflow.stages(0).stage_id
                                onewdoc.stage_name = bc_am_load_objects.obc_pub_types.pubtype(i).workflow.stages(0).stage_name
                            End If
                            onewdoc.workflow_stages = bc_am_load_objects.obc_pub_types.pubtype(i).workflow
                            ldoc = onewdoc

                            REM show submit disalogue
                            Dim osubmit As New bc_am_at_submit_frm
                            osubmit.register_only = register_only
                            If register_only = False Then
                                osubmit.Text = "Blue Curve - import document: " + .name
                                osubmit.attach_mode = True
                            Else
                                osubmit.attach_mode = False
                                osubmit.Text = "Blue Curve - register document: " + .name
                            End If

                            osubmit.ldoc = ldoc
                            osubmit.ActiveControl = osubmit.txttitle
                            osubmit.Focus()
                            osubmit.ShowDialog()
                            If Not osubmit.ok_selected Then
                                REM if cancel selected do nothing
                                ocommentary = New bc_cs_activity_log("bc_am_workflow", "import_master_document", bc_cs_activity_codes.COMMENTARY, "Cancel Selected on Submit form.")
                                Exit Sub
                            End If
                            If register_only = False Then
                                ldoc.filename = osubmit.attach_file
                                REM SW cope with office versions
                                extensionsize = (Len(osubmit.attach_file) - (InStrRev(osubmit.attach_file, ".") - 1))
                                ldoc.extension = Right(osubmit.attach_file, extensionsize)

                                REM mark document as imported 
                                ldoc.extension = "[imp]" + ldoc.extension
                            End If
                        End With
                        Exit For
                    End If
                Next
                If register_only = False Then
                    REM suck file up into document object
                    If fs.check_document_exists(ldoc.filename) = False Then
                        Dim omessage As New bc_cs_message("Blue Curve", "Document cant be accessed import aborted", bc_cs_message.MESSAGE)
                        Exit Sub
                    End If
                    fs.write_document_to_bytestream(ldoc.filename, ldoc.byteDoc, Nothing)
                Else
                    ldoc.register_template = True
                End If
            Else
                REM regular report

                ldoc = bc_am_load_objects.obc_pub_types.regular_reports.regular_reports(fwizard_main.report_idx).doc

                Dim osubmit As New bc_am_at_submit_frm
                ldoc.doc_date = Now
                ldoc.id = 0
                REM set workflow

                For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                    If bc_am_load_objects.obc_pub_types.pubtype(i).id = ldoc.pub_type_id Then
                        ldoc.stage = bc_am_load_objects.obc_pub_types.pubtype(i).workflow.stages(0).stage_id
                        ldoc.stage_name = bc_am_load_objects.obc_pub_types.pubtype(i).workflow.stages(0).stage_name
                        ldoc.workflow_stages = bc_am_load_objects.obc_pub_types.pubtype(i).workflow
                        ldoc.bus_area = bc_am_load_objects.obc_pub_types.pubtype(i).bus_area_id
                        ldoc.originating_author = bc_cs_central_settings.logged_on_user_id
                        osubmit.Text = "Blue Curve  - Import Regular Report: " + bc_am_load_objects.obc_pub_types.pubtype(i).name
                        Exit For
                    End If
                Next
                ldoc.master_flag = True
                ldoc.auto_generate_taxonomy = False
                osubmit.ldoc = ldoc
                osubmit.regular_report = True
                osubmit.register_only = register_only
                osubmit.Focus()
                osubmit.attach_mode = True
                osubmit.ShowDialog()
                If Not osubmit.ok_selected Then
                    REM if cancel selected do nothing
                    ocommentary = New bc_cs_activity_log("bc_am_workflow", "import_master_document", bc_cs_activity_codes.COMMENTARY, "Cancel Selected on Submit form.")
                    Exit Sub
                End If
                ldoc.filename = osubmit.attach_file

                REM SW cope with office versions
                extensionsize = (Len(osubmit.attach_file) - (InStrRev(osubmit.attach_file, ".") - 1))

                ldoc.extension = Right(osubmit.attach_file, extensionsize)
                REM mark document as imported 
                ldoc.extension = "[imp]" + ldoc.extension
                fs.write_document_to_bytestream(ldoc.filename, ldoc.byteDoc, Nothing)
            End If
            REM now blank filename
            ldoc.filename = ""
            bc_am_workflow.cfrm.cursor_wait()
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                ldoc.db_write(Nothing)
            Else
                ldoc.tmode = bc_cs_soap_base_class.tWRITE
                ldoc.transmit_to_server_and_receive(ldoc, True)
            End If

            If ldoc.doc_write_error_text <> "" Then
                Dim omsg As New bc_cs_message("Blue Curve", "Import Document Failed: " + ldoc.doc_write_error_text, bc_cs_message.MESSAGE, False, False, "yes", "no", True)
            End If
            Me.retrieve_docs(False, False)

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_workflow", "import_master_document", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            bc_am_workflow.cfrm.cursor_default()
            bc_am_workflow.polling_enabled = tpoll
            slog = New bc_cs_activity_log("bc_am_workflow", "import_master_document", bc_cs_activity_codes.TRACE_EXIT, "auto: " + CStr(bc_am_workflow.auto_refresh))
        End Try

    End Sub
    Public Sub import_support_document(ByVal ldoc As bc_om_document)
        REM FIL JUN 2013
        Dim slog As New bc_cs_activity_log("bc_am_workflow", "import_support_document", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim tpoll As Boolean
        Try
            tpoll = bc_am_workflow.polling_enabled
            bc_am_workflow.polling_enabled = False


            Dim extensionsize As Integer

            Dim fs As New bc_cs_file_transfer_services
           
            Dim ocommentary As bc_cs_activity_log
            tpoll = bc_am_workflow.polling_enabled
            bc_am_workflow.polling_enabled = False
            Dim sdoc As New bc_om_document

            sdoc.id = 0
            sdoc.master_flag = False
            sdoc.pub_type_id = ldoc.pub_type_id
            sdoc.pub_type_name = ldoc.pub_type_name
            sdoc.originating_author = ldoc.originating_author
            sdoc.bus_area = ldoc.bus_area
            sdoc.checked_out_user = 0
            'sdoc.doc_date = ldoc.doc_date
            sdoc.entity_id = ldoc.entity_id
            sdoc.originating_author = ldoc.originating_author
            sdoc.authors = ldoc.authors
            sdoc.taxonomy = ldoc.taxonomy
            sdoc.disclosures = ldoc.disclosures
            sdoc.workflow_stages = ldoc.workflow_stages

            sdoc.doc_date = Now

            REM show pub type dialogue
            Dim osubmit As New bc_am_at_submit_frm
            osubmit.import_support_doc = True
            osubmit.register_only = False

            osubmit.Text = "Blue Curve - import support document"
            osubmit.attach_mode = True

            osubmit.Cpub.Enabled = True

            For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                osubmit.Cpub.Items.Add(bc_am_load_objects.obc_pub_types.pubtype(i).name)
            Next
            osubmit.Cpub.Text = ldoc.pub_type_name

            osubmit.ldoc = sdoc
            osubmit.ActiveControl = osubmit.txttitle
            osubmit.Focus()
            osubmit.ShowDialog()
            If Not osubmit.ok_selected Then
                REM if cancel selected do nothing
                ocommentary = New bc_cs_activity_log("bc_am_workflow", "import_master_document", bc_cs_activity_codes.COMMENTARY, "Cancel Selected on Submit form.")
                Exit Sub
            End If
            Dim filename As String
            filename = osubmit.attach_file
            REM SW cope with office versions
            extensionsize = (Len(filename) - (InStrRev(filename, ".") - 1))
            sdoc.extension = Right(filename, extensionsize)

            'sdoc.title = Left(filename, Len(filename) - extensionsize)
            'sdoc.title = Right(sdoc.title, Len(sdoc.title) - InStrRev(sdoc.title, "\", Len(sdoc.title)))

            sdoc.register_only = False
            sdoc.filename = filename
            sdoc.bwith_document = True
            If fs.write_document_to_bytestream(filename, sdoc.byteDoc, Nothing, False) = False Then
                Dim omessage As New bc_cs_message("Blue Curve", "File: " + filename + " can't be accessed", bc_cs_message.MESSAGE)
                Exit Sub
            End If


            For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                If osubmit.Cpub.Text = bc_am_load_objects.obc_pub_types.pubtype(i).name Then
                    sdoc.pub_type_id = bc_am_load_objects.obc_pub_types.pubtype(i).id()
                    sdoc.pub_type_name = bc_am_load_objects.obc_pub_types.pubtype(i).name
                    Exit For
                End If
            Next

            ldoc.support_documents.document.Clear()
            ldoc.support_documents.document.Add(sdoc)
            ldoc.register_only = False
            ldoc.bcheck_out = False
            ldoc.bwith_document = False
            ldoc.history.Clear()
            ldoc.bimport_support_only = True
            bc_am_workflow.cfrm.cursor_wait()
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                ldoc.db_write(Nothing)
            Else
                ldoc.tmode = bc_cs_soap_base_class.tWRITE
                ldoc.transmit_to_server_and_receive(ldoc, True)
            End If
            If ldoc.doc_write_error_text <> "" Then
                Dim omsg As New bc_cs_message("Blue Curve", "Import Support Document Failed: " + ldoc.doc_write_error_text, bc_cs_message.MESSAGE, False, False, "yes", "no", True)
            End If
            Me.retrieve_docs(False, False)

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_workflow", "import_support_document", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            bc_am_workflow.polling_enabled = tpoll
            bc_am_workflow.cfrm.cursor_default()
            slog = New bc_cs_activity_log("bc_am_workflow", "import_support_document", bc_cs_activity_codes.TRACE_EXIT, "auto: " + CStr(bc_am_workflow.auto_refresh))
        End Try

    End Sub
    Public Sub import_registered_document(ByVal ldoc As bc_om_document)
        Dim slog As New bc_cs_activity_log("bc_am_workflow", "import_registered_document", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim tpoll As Boolean
        Try
            tpoll = bc_am_workflow.polling_enabled
            bc_am_workflow.polling_enabled = False

            Dim fs As New bc_cs_file_transfer_services

            Dim odialog As New OpenFileDialog
            Dim extensionsize As Integer

            odialog.Title = "Import Registered Document for " + ldoc.title
            odialog.ShowDialog()

            REM SW cope with office versions
            extensionsize = (Len(odialog.FileName) - (InStrRev(odialog.FileName, ".") - 1))

            If odialog.FileName <> "" Then
                ldoc.register_template = False
                ldoc.extension = "[imp]" + Right(odialog.FileName, extensionsize)
                ldoc.register_only = False
                ldoc.filename = odialog.FileName
                ldoc.bwith_document = True
                If fs.write_document_to_bytestream(odialog.FileName, ldoc.byteDoc, Nothing, False) = False Then
                    Dim omessage As New bc_cs_message("Blue Curve", "File: " + odialog.FileName + " can't be accessed", bc_cs_message.MESSAGE)
                    Exit Sub
                End If
                ldoc.support_documents.document.Clear()
                ldoc.register_only = False
                ldoc.register_template = True
                ldoc.bcheck_out = False
                ldoc.history.Clear()
                ldoc.history.Add("Registered Document Attached")
                bc_am_workflow.cfrm.cursor_wait()
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    ldoc.db_write(Nothing)
                Else
                    ldoc.tmode = bc_cs_soap_base_class.tWRITE
                    ldoc.transmit_to_server_and_receive(ldoc, True)
                End If
                If ldoc.doc_write_error_text <> "" Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Import Registered Failed: " + ldoc.doc_write_error_text, bc_cs_message.MESSAGE, False, False, "yes", "no", True)
                End If
                Me.retrieve_docs(False, False)
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_workflow", "import_registered_document", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            bc_am_workflow.polling_enabled = tpoll
            bc_am_workflow.cfrm.cursor_default()
            slog = New bc_cs_activity_log("bc_am_workflow", "import_registered_document", bc_cs_activity_codes.TRACE_EXIT, "auto: " + CStr(bc_am_workflow.auto_refresh))
        End Try

    End Sub
    Public Sub attach_new_document(ByVal ldoc As bc_om_document)
        Dim slog As New bc_cs_activity_log("bc_am_workflow", "attach_new_document", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim tpoll As Boolean
        Try
            tpoll = bc_am_workflow.polling_enabled
            bc_am_workflow.polling_enabled = False

            Dim fs As New bc_cs_file_transfer_services
            Dim extensionsize As Integer

            Dim odialog As New OpenFileDialog
            odialog.Title = "Attach New Document for " + ldoc.title
            odialog.ShowDialog()
            ldoc.original_extension = ldoc.extension
            If odialog.FileName <> "" Then

                REM SW cope with office versions
                extensionsize = (Len(odialog.FileName) - (InStrRev(odialog.FileName, ".") - 1))

                ldoc.extension = "[imp]" + Right(odialog.FileName, extensionsize)
                ldoc.filename = odialog.FileName
                ldoc.bwith_document = True
                If fs.write_document_to_bytestream(odialog.FileName, ldoc.byteDoc, Nothing, False) = False Then
                    Dim omessage As New bc_cs_message("Blue Curve", "File: " + odialog.FileName + " can't be accessed", bc_cs_message.MESSAGE)
                    Exit Sub
                End If
                ldoc.support_documents.document.Clear()
                ldoc.register_only = False
                ldoc.register_template = True
                ldoc.bcheck_out = False
                ldoc.history.Clear()
                ldoc.btake_revision = True
                ldoc.history.Add("New Document Attached of type: " + ldoc.extension)
                bc_am_workflow.cfrm.cursor_wait()
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    ldoc.db_write(Nothing)
                Else
                    ldoc.tmode = bc_cs_soap_base_class.tWRITE
                    ldoc.transmit_to_server_and_receive(ldoc, True)
                End If
                If ldoc.doc_write_error_text <> "" Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Attach Document Failed: " + ldoc.doc_write_error_text, bc_cs_message.MESSAGE, False, False, "yes", "no", True)
                End If
                Me.retrieve_docs(False, False)
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_workflow", "attach_new_document", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            bc_am_workflow.polling_enabled = tpoll
            bc_am_workflow.cfrm.cursor_default()
            slog = New bc_cs_activity_log("bc_am_workflow", "attach_new_document", bc_cs_activity_codes.TRACE_EXIT, "auto: " + CStr(bc_am_workflow.auto_refresh))
        End Try

    End Sub
    Public Sub retrieve_docs(ByVal first As Boolean, Optional ByVal from_poll As Boolean = False, Optional ByVal update_form As Boolean = True, Optional ByVal autorefresh As Boolean = False)
        If bc_am_workflow.polling_process = False Then
            retrieve_docs_after_poll_check(first, from_poll, update_form, autorefresh)
        End If
    End Sub

    Public Sub force_check_in(ByVal ldoc As bc_om_document)
        Dim omsg As New bc_cs_message("Blue Curve", "Are you sure you wish to force check in: " + ldoc.title + ". This operation will remove checked out users edits", bc_cs_message.MESSAGE, True, False, "Yes", "No")
        If omsg.cancel_selected = True Then
            Exit Sub
        End If
        ldoc.do_force_check_in = True
        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
            ldoc.db_write(Nothing)
        Else
            ldoc.tmode = bc_cs_soap_base_class.tWRITE
            ldoc.transmit_to_server_and_receive(ldoc, True)
        End If
        Me.retrieve_docs(False, False)
    End Sub


    Public Sub retrieve_docs_after_poll_check(ByVal first As Boolean, Optional ByVal from_poll As Boolean = False, Optional ByVal update_form As Boolean = True, Optional ByVal autorefresh As Boolean = False)
        Dim slog As New bc_cs_activity_log("bc_am_workflow", "retrieve_docs", bc_cs_activity_codes.TRACE_ENTRY, "auto: " + CStr(bc_am_workflow.auto_refresh))
        Try

            bc_am_workflow.cfrm.cursor_wait()
            If from_poll = False Then
                'If update_form = True Then
                'Me.ofrm.DocumentList.Visible = False
                'End If
            End If
            If first = False Then
                If processing = True Then
                    Exit Sub
                End If
                If bc_am_workflow.auto_refresh = False And autorefresh = False Then
                    Exit Sub
                End If
                Dim tdate As New TimeSpan

                tdate = Now.Subtract(bc_am_workflow.last_screen_refresh)
                If tdate.Seconds < screen_refresh_interval And from_screen = True Then
                    from_screen = False
                    bc_am_workflow.cfrm.set_server_status("Auto Refresh " + CStr(bc_am_workflow.auto_refresh) + "s")
                    Exit Sub
                Else
                    bc_am_workflow.last_screen_refresh = Now
                End If
            Else
                bc_am_workflow.last_screen_refresh = Now
            End If
            If from_poll = False Then
                'set_server_status("Requesting...")
            End If
            processing = True
            Dim i As Integer
            If first = False Then
                pdocs.document.Clear()
                For i = 0 To docs.document.Count - 1
                    pdocs.document.Add(docs.document(i))
                Next
            End If
            docs.document.Clear()
            docs.workflow_mode = True
            docs.date_from = bc_am_workflow.fdatefrom
            Try
                If bc_am_workflow.cfrm.FilterDateTo.Visible = True Then
                    docs.date_to = bc_am_workflow.fdateto
                Else
                    docs.date_to = "9-9-9999"
                End If
            Catch
                docs.date_to = "9-9-9999"
            End Try
            If from_poll = False Then
                docs.packet_code = "process"
            Else
                docs.packet_code = "poll"
            End If

            REM steve wooderson 08/07/2013 local docs
            If bc_am_workflow.cfrm.uxLocal.Checked Then
                 local_refresh()
            End If
            If bc_am_workflow.cfrm.uxCentral.Checked Then
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    docs.db_read()
                Else
                    docs.tmode = bc_cs_soap_base_class.tREAD
                    docs.transmit_to_server_and_receive(docs, True)
                End If
            End If

            eval_new_doc()
            pdocs.document.Clear()
            If update_form = True Then
                If from_poll = True Then
                    If screen_update_enabled = True And screen_poll = "TRUE" Then
                        ofrm.load_data(bc_am_workflow.mode)
                        ofrm.apply_sort()

                        Dim tx As String
                        Dim n As Integer
                        tx = bc_am_workflow.cfrm.lpoll.Text
                        n = InStr(tx, "Updated:")
                        If n > 0 Then
                            tx = Left(tx, n - 2)
                        End If
                        bc_am_workflow.cfrm.lpoll.Text = tx + " Updated: " + Format(Now, "HH:mm:ss")

                    End If
                Else
                    ofrm.load_data(bc_am_workflow.mode, autorefresh)
                    ofrm.apply_sort()

                    Dim tx As String
                    Dim n As Integer
                    tx = bc_am_workflow.cfrm.lpoll.Text
                    n = InStr(tx, "Updated:")
                    If n > 0 Then
                        tx = Left(tx, n - 2)
                    End If
                    bc_am_workflow.cfrm.lpoll.Text = tx + " Updated: " + Format(Now, "HH:mm:ss")

                End If
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_workflow", "retrieve_doc", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            bc_am_workflow.cfrm.cursor_default()
            'bc_am_workflow.ofrm.DocumentList.Visible = True
            clear_server_status()
            processing = False
            slog = New bc_cs_activity_log("bc_am_workflow", "retrieve_docs", bc_cs_activity_codes.TRACE_EXIT, "auto: " + CStr(bc_am_workflow.auto_refresh))
        End Try
    End Sub
    Public Sub deselect()
        bc_am_workflow.selected_doc_id = 0

    End Sub
    Public Sub set_server_status(ByVal status As String)
        Try
            bc_am_workflow.cfrm.set_server_status(status)
        Catch
        End Try
    End Sub
    Public Sub clear_server_status()
        bc_am_workflow.cfrm.clear_server_status()
    End Sub
    Public Sub set_polling_status()
        bc_am_workflow.cfrm.set_polling_status(txtpoll)
    End Sub
    Public Sub clear_polling_status()
        bc_am_workflow.cfrm.clear_polling_status()
    End Sub
    Public Sub doclistpoll()
        Dim slog As New bc_cs_activity_log("bc_am_workflow", "doclistpoll", bc_cs_activity_codes.TRACE_ENTRY, "auto: " + CStr(bc_am_workflow.auto_refresh))
        Try
            Dim k As Integer
            Dim lscreen_enable As Boolean
            Dim ocommentary As New bc_cs_activity_log("bc_am_workflow", "doclistpoll", bc_cs_activity_codes.COMMENTARY, "New")
            Thread.Sleep(1000)
            lscreen_enable = bc_am_workflow.auto_refresh
            bc_am_workflow.auto_refresh = True
            retrieve_docs(True)
            bc_am_workflow.auto_refresh = lscreen_enable
            bc_am_workflow.snew = False
            ocommentary = New bc_cs_activity_log("bc_am_workflow", "doclistpoll", bc_cs_activity_codes.COMMENTARY, "Not new:" + CStr(bc_am_workflow.auto_refresh))
            While k = 0
                bc_am_workflow.polling_process = False
                Thread.Sleep(bc_am_workflow.polling_interval)
                bc_am_workflow.polling_process = True
                ocommentary = New bc_cs_activity_log("bc_am_workflow", "doclistpoll", bc_cs_activity_codes.COMMENTARY, "CHECK POLL")
                REM only update id user isnt in Process form

                REM Check inactive timer
                If polling_enabled = True And screen_update_enabled = False Then
                    If Now() > DateAdd(DateInterval.Minute, user_inactive_interval, user_inactive_last) Then
                        screen_update_enabled = True
                        set_polling_status()
                    End If
                End If
                If polling_enabled = True And screen_update_enabled = True Then
                    slog = New bc_cs_activity_log("bc_am_workflow", "doclistpoll", bc_cs_activity_codes.TRACE_EXIT, "POLL")
                    lscreen_enable = bc_am_workflow.auto_refresh
                    bc_am_workflow.auto_refresh = True
                    bc_am_workflow.loading = True
                    retrieve_docs_after_poll_check(False, True)
                    bc_am_workflow.auto_refresh = lscreen_enable
                    bc_am_workflow.loading = False
                    Dim tx As String
                    Dim n As Integer
                    tx = bc_am_workflow.cfrm.lpoll.Text
                    n = InStr(tx, "Updated:")
                    If n > 0 Then
                        tx = Left(tx, n - 2)
                    End If
                    bc_am_workflow.cfrm.lpoll.Text = tx + " Updated: " + Format(Now, "HH:mm:ss")
                End If

            End While
        Catch ex As Exception
            'Dim db_err As New bc_cs_error_log("bc_am_workflow", "doclistpoll", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            bc_am_workflow.polling_process = False
            slog = New bc_cs_activity_log("bc_am_workflow", "doclistpoll", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub

    Public Sub setColors(ByVal newColors As ProcessColors)
        bc_am_workflow.cfrm.setColors(newColors)
    End Sub

    Private Function get_doc_title(ByVal doc_id As Long) As String
        Dim i As Integer

        get_doc_title = ""

        For i = 0 To docs.document.Count - 1
            If doc_id = docs.document(i).id Then
                get_doc_title = docs.document(i).title
                Exit For
            End If
        Next
    End Function
    Private Sub alert()
        Dim slog As New bc_cs_activity_log("bc_am_workflow", "alert", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            bc_am_workflow.alert_doc_id = 0
            bc_am_workflow.alert_mode = 0
            msg = ""

            If ndocs.Count = 1 Then
                msg = "New Document has Arrived: " + get_doc_title(ndocs(0)) + " in Process"
                bc_am_workflow.alert_doc_id = ndocs(0)
                bc_am_workflow.alert_mode = 1
            End If
            If ndocs.Count > 1 Then
                If msg = "" Then
                    msg = CStr(ndocs.Count) + " Documents have Arrived in Process"
                    bc_am_workflow.alert_mode = 1
                Else
                    bc_am_workflow.alert_mode = 0
                    msg = msg + "; " + CStr(ndocs.Count) + " Documents have Arrived in Process"
                End If
            End If
            If edocs.Count = 1 Then
                If msg = "" Then
                    msg = msg + "Document has Expired in Process: " + get_doc_title(edocs(0))
                    bc_am_workflow.alert_doc_id = edocs(0)
                    bc_am_workflow.alert_mode = 3
                Else
                    bc_am_workflow.alert_mode = 0
                    msg = msg + "; Document has Expired in Process"
                End If
            End If
            If edocs.Count > 1 Then
                If msg = "" Then
                    msg = CStr(edocs.Count) + " Documents have Expired in Process"
                    bc_am_workflow.alert_mode = 3
                Else
                    bc_am_workflow.alert_mode = 0
                    msg = msg + "; " + CStr(edocs.Count) + " Documents have Expired in Process"
                End If
            End If
            If udocs.Count = 1 Then
                If msg = "" Then
                    msg = msg + "Document has become urgent in Process: " + get_doc_title(udocs(0))
                    bc_am_workflow.alert_doc_id = udocs(0)
                    bc_am_workflow.alert_mode = 2
                Else
                    bc_am_workflow.alert_mode = 0
                    msg = msg + "; Document has become urgent in Process"
                End If
            End If
            If udocs.Count > 1 Then
                If msg = "" Then
                    msg = CStr(udocs.Count) + " Document have become urgent in Process"
                    bc_am_workflow.alert_mode = 2
                Else
                    msg = msg + "; " + CStr(udocs.Count) + " Documents have become urgent in Process"
                    bc_am_workflow.alert_mode = 0
                End If
            End If
            If stdocs.Count = 1 Then
                If msg = "" Then
                    msg = msg + "Document has changed stage: " + get_doc_title(stdocs(0))
                    bc_am_workflow.alert_doc_id = stdocs(0)
                    bc_am_workflow.alert_mode = 4
                Else
                    msg = msg + "; " + "Document has changed stage: " + get_doc_title(stdocs(0))
                    bc_am_workflow.alert_mode = 0
                End If
            End If
            If stdocs.Count > 1 Then
                If msg = "" Then
                    msg = CStr(stdocs.Count) + " Documents have changed stage"
                    bc_am_workflow.alert_mode = 4
                Else
                    msg = msg + "; " + CStr(stdocs.Count) + " Documents have changed stage"
                    bc_am_workflow.alert_mode = 0
                End If
            End If
            Dim talertform As New Thread(AddressOf alert_form)
            If bc_am_workflow.beep_enabled = True Then
                Beep()
            End If
            talertform.Start()
            Thread.Sleep(bc_am_workflow.fade_interval * 1000)
            oalertfrm.Close()


        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_workflow", "alert", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_workflow", "alert", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Private Sub alert_form()
        oalertfrm = New bc_am_workflow_alert
        oalertfrm.lalert.Text = msg
        Dim oapi As New API
        API.SetWindowPos(oalertfrm.Handle.ToInt32, API.HWND_TOPMOST, Nothing, 1, 1, 1, 1)
        oalertfrm.ShowDialog()
    End Sub
    Private Sub eval_new_doc()
        Dim slog As New bc_cs_activity_log("bc_am_workflow", "eval_new_doc", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim i, j As Integer
            Dim found As Boolean
            ndocs.Clear()
            edocs.Clear()
            udocs.Clear()
            stdocs.Clear()
            REM see if new docs have arrived           
            For i = 0 To docs.document.Count - 1
                docs.document(i).new_flag = False
                found = False
                For j = 0 To pdocs.document.Count - 1
                    If docs.document(i).id = pdocs.document(j).id Then
                        docs.document(i).new_flag = pdocs.document(j).new_flag
                        docs.document(i).expire_flag = pdocs.document(j).expire_flag
                        docs.document(i).urgent_flag = pdocs.document(j).urgent_flag
                        docs.document(i).unread = pdocs.document(j).unread
                        docs.document(i).search_flag = pdocs.document(j).search_flag
                        docs.document(i).acknowledged = pdocs.document(j).acknowledged
                        docs.document(i).arrive = pdocs.document(j).arrive
                        docs.document(i).stage_change_flag = pdocs.document(j).stage_change_flag
                        REM check here if existing document has now become urgent or expired
                        If docs.document(i).stage_expire_date < Now And docs.document(i).expire_flag = False Then
                            docs.document(i).expire_flag = True
                            docs.document(i).urgent_flag = False
                            docs.document(i).unread = True
                            docs.document(i).arrive = Now
                            edocs.Add(docs.document(i).id)
                        End If
                        If Now.AddDays(bc_am_workflow.pre_expire_alert_notify) > docs.document(i).stage_expire_date And Now < docs.document(i).stage_expire_date And docs.document(i).urgent_flag = False Then
                            docs.document(i).urgent_flag = True
                            docs.document(i).expire_flag = False
                            docs.document(i).unread = True
                            docs.document(i).arrive = Now
                            udocs.Add(docs.document(i).id)
                        End If
                        If docs.document(i).stage_expire_date > Now Then
                            docs.document(i).expire_flag = False
                        End If
                        If docs.document(i).stage_expire_date > Now.AddDays(bc_am_workflow.pre_expire_alert_notify) Then
                            docs.document(i).urgent_flag = False
                        End If
                        REM check if document has changed stage
                        If docs.document(i).stage <> pdocs.document(j).stage Then
                            docs.document(i).stage_change_flag = True
                            docs.document(i).arrive = Now
                            stdocs.Add(docs.document(i).id)
                        End If
                        found = True
                        Exit For
                    End If
                Next
                If found = False Then
                    docs.document(i).new_flag = True
                    docs.document(i).unread = True
                    docs.document(i).arrive = Now
                    ndocs.Add(docs.document(i).id)
                End If
            Next
            If (ndocs.Count > 0 Or edocs.Count > 0 Or udocs.Count > 0 Or stdocs.Count > 0) And alerter_enabled = "TRUE" Then
                Dim talert As New Thread(AddressOf alert)
                Beep()
                talert.Start()
            Else
                bc_am_workflow.mode = bc_am_workflow.mode
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_workflow", "eval_new_doc", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            processing = False
            slog = New bc_cs_activity_log("bc_am_workflow", "eval_new_doc", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Public Sub load_doc(ByVal ldoc As bc_om_document)
        Dim slog As New bc_cs_activity_log("bc_am_workflow", "load_doc", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            REM assign selected document
            Dim tid As Long
            tid = ldoc.id
            'Me.cfrm.ofrmsummary.clear_doc()
            'set_server_status("Requesting...")
            ldoc.bcheck_out = False
            ldoc.bwith_document = False
            ldoc.btake_revision = False
            If Not IsNothing(ldoc.support_documents) Then
                ldoc.support_documents.document.Clear()
            End If
            If Not IsNothing(ldoc.history) Then
                ldoc.history.Clear()
            End If

            REM steve wooderson 08/07/2013 local docs
            If bc_am_workflow.cfrm.uxLocal.Checked Then

                REM find the local document id
                For l = 0 To oopen_doc.onetwork_docs.document.Count - 1
                    If ldoc.filename = oopen_doc.onetwork_docs.document(l).filename Then
                        ldoc = oopen_doc.onetwork_docs.document(l)
                        ldoc.read = True
                        ldoc.write = True
                        ldoc.local_filename = ldoc.filename
                        ldoc.local_index = ldoc.id
                        Exit For
                    End If
                Next l

            End If
            If bc_am_workflow.cfrm.uxCentral.Checked Then
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    ldoc.db_read()
                ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                    ldoc.tmode = bc_cs_soap_base_class.tREAD
                    ldoc.transmit_to_server_and_receive(ldoc, True)
                End If
            End If

            clear_server_status()
            If ldoc.id = -1 Then
                Dim omessage As New bc_cs_message("Blue Curve - process", "Another user has just changed the stage of this document you may no longer be able to view", bc_cs_message.MESSAGE)
                Me.retrieve_docs(False, False)
            Else
                ldoc.id = tid
                bc_am_workflow.cfrm.ofrmsummary.ldoc = ldoc
                bc_am_workflow.cfrm.ofrmsummary.load_doc()
            End If
            ldoc.id = tid
            bc_am_workflow.curr_doc = ldoc
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_workflow", "load_doc", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            processing = False
            slog = New bc_cs_activity_log("bc_am_workflow", "load_doc", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Public Sub view_revision(ByVal fn As String, ByVal ldoc As bc_om_document)

        Dim slog As New bc_cs_activity_log("bc_am_workflow", "view_revision", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim rdoc As New bc_om_document
            Dim vfn As String
            Dim extensionsize As Integer

            rdoc.brevision_mode = 1
            rdoc.revision_filename = fn

            REM SW cope with office versions
            extensionsize = (Len(fn) - (InStrRev(fn, ".") - 1))

            rdoc.extension = Right(fn, extensionsize)
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                rdoc.db_read()
            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                rdoc.tmode = bc_cs_soap_base_class.tREAD
                rdoc.transmit_to_server_and_receive(rdoc, True)
            End If
            If rdoc.doc_not_found = True Then
                Dim omessage As New bc_cs_message("Blue Curve", "Document: " + fn + " not found on server.", bc_cs_message.MESSAGE)
                Exit Sub
            End If
            REM view it
            rdoc.extension = Right(fn, extensionsize)
            vfn = save_view_only_file(rdoc)
            Dim omime As New bc_am_mime_types
            omime.view(bc_cs_central_settings.local_repos_path + vfn + rdoc.extension, rdoc.extension)
            bc_am_workflow.cfrm.WindowState = FormWindowState.Minimized

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_workflow", "view_revision", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_workflow", "view_revision", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Public Sub revert(ByVal fn As String, ByVal ldoc As bc_om_document)

        Dim slog As New bc_cs_activity_log("bc_am_workflow", "revert", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim rdoc As New bc_om_document
            rdoc.btake_revision = False
            rdoc.bwith_document = False
            rdoc.bcheck_out = False
            rdoc.stage = ldoc.stage
            rdoc.brevision_mode = 2
            rdoc.revision_filename = fn
            rdoc.id = ldoc.id
            rdoc.extension = ldoc.extension
            rdoc.checked_out_user = -1
            rdoc.certificate.name = bc_cs_central_settings.logged_on_user_name
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                rdoc.db_read()
            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                rdoc.tmode = bc_cs_soap_base_class.tREAD
                rdoc.transmit_to_server_and_receive(rdoc, True)
            End If
            If rdoc.id = -1 Then
                Dim omessage As New bc_cs_message("Blue Curve - process", "Another user has just changed the stage of this document you cant revert stage and may no longer be able to view", bc_cs_message.MESSAGE)
                Exit Sub
            End If
            If rdoc.id = 0 Then
                Dim omessage As New bc_cs_message("Blue Curve - process", "Another user has just checked out  this document you cant revert", bc_cs_message.MESSAGE)
                Exit Sub
            End If
            If rdoc.id = -2 Then
                Dim omessage As New bc_cs_message("Blue Curve - process", "Revert failed reverted file does not exist on server.", bc_cs_message.MESSAGE)
                Exit Sub
            End If
            If rdoc.id = -3 Then
                Dim omessage As New bc_cs_message("Blue Curve - process", "Revert failed master file does not exist on server.", bc_cs_message.MESSAGE)
                Exit Sub
            End If


        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_workflow", "revert", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.retrieve_docs(False, False)
            slog = New bc_cs_activity_log("bc_am_workflow", "revert", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Public Sub checkout_master_doc(ByVal ldoc As bc_om_document)

        Dim omime As bc_am_mime_types
        Dim fn As String
        Dim tpoll As Boolean
        Dim slog As New bc_cs_activity_log("bc_am_workflow", "checkout_master_doc", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim extensionsize As Integer

        Try
            set_server_status("Requesting...")
            Dim omessage As bc_cs_message
            tpoll = bc_am_workflow.polling_enabled
            bc_am_workflow.polling_enabled = False

            REM if document is checkedout to you open it from local area
            If ldoc.checked_out_user = bc_cs_central_settings.logged_on_user_id Then

                REM SW cope with office versions
                extensionsize = (Len(ldoc.filename) - (InStrRev(ldoc.filename, ".") - 1))

                fn = bc_cs_central_settings.local_repos_path + Left(ldoc.filename, Len(ldoc.filename) - extensionsize) + ldoc.extension
                REM check file exists if it doesnt get it of server
                Dim fs As New bc_cs_file_transfer_services
                If fs.check_document_exists(fn) = True Then
                    omime = New bc_am_mime_types
                    omime.view(fn, ldoc.extension)
                    Exit Sub
                Else
                    omessage = New bc_cs_message("Blue Curve - process", "Document is checked out to you but local copy doesnt exist. Server Copy will be extracted.", bc_cs_message.MESSAGE)
                End If
            End If
            ldoc.btake_revision = True
            ldoc.bcheck_out = True
            ldoc.bwith_document = True
            Dim tdid As Long
            tdid = ldoc.id
            REM if edit is requested and document is checked out to current user then attempt to use local
            REM copy if it is already open or doesnt exist flag this


            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                ldoc.db_read()
            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                ldoc.certificate.name = bc_cs_central_settings.logged_on_user_name
                ldoc.tmode = bc_cs_soap_base_class.tREAD
                ldoc.transmit_to_server_and_receive(ldoc, True)
            End If

            If ldoc.doc_not_found Then
                omessage = New bc_cs_message("Blue Curve", "Document not found on server contact system administrator!", bc_cs_message.MESSAGE)
                Exit Sub
            End If
            If ldoc.id = 0 Then
                omessage = New bc_cs_message("Blue Curve", "Document has just become checked out to another user cannot open", bc_cs_message.MESSAGE)
                Exit Sub
            End If
            If ldoc.id = -1 Then
                omessage = New bc_cs_message("Blue Curve", "Document has just had stage changed  cannot open", bc_cs_message.MESSAGE)
                Exit Sub
            End If

            REM can only quick open a word document
            REM save as view only file
            set_server_status("Saving Document for Edit Locally")
            fn = save_edit_file(ldoc)
            omime = New bc_am_mime_types
            omime.view(bc_cs_central_settings.local_repos_path + fn, ldoc.extension)
            bc_am_workflow.cfrm.WindowState = FormWindowState.Minimized

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_workflow", "checkout_master_doc", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.retrieve_docs(False, False)
            bc_am_workflow.polling_enabled = tpoll
            slog = New bc_cs_activity_log("bc_am_workflow", "checkout_master_doc", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    Public Sub edit_metadata(ByVal ldoc As bc_om_document)
        Dim slog As New bc_cs_activity_log("bc_am_workflow", "edit_metadata", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim tpoll As Boolean
        Try
            tpoll = bc_am_workflow.polling_enabled
            bc_am_workflow.polling_enabled = False

            REM invoke submit form
            REM show submit disalogue
            REM can only edit if write access on document

            If ldoc.write = True Then
                Dim osubmit As New bc_am_at_submit_frm

                osubmit.Text = "Blue Curve - Edit metadata"
                osubmit.edit_mode = True
                osubmit.ldoc = ldoc
                osubmit.ActiveControl = osubmit.txttitle
                osubmit.ShowDialog()
                bc_am_workflow.cfrm.cursor_wait()

                If osubmit.ok_selected Then
                    REM write metadata to system
                    ldoc.history.Clear()
                    ldoc.support_documents.document.Clear()
                    ldoc.bimport_support_only = True
                    ldoc.bwith_document = False
                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        ldoc.db_write(Nothing)
                    Else
                        ldoc.tmode = bc_cs_soap_base_class.tWRITE
                        ldoc.no_send_back = True
                        ldoc.transmit_to_server_and_receive(ldoc, True)
                    End If
                    If ldoc.doc_write_error_text <> "" Then
                        Dim omsg As New bc_cs_message("Blue Curve", "Change Metadata Failed: " + ldoc.doc_write_error_text, bc_cs_message.MESSAGE, False, False, "yes", "no", True)
                    End If
                End If
                ldoc.history.Clear()
                retrieve_docs(False, False)
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_workflow", "edit_metadata", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            bc_am_workflow.cfrm.cursor_default()
            bc_am_workflow.polling_enabled = tpoll
            slog = New bc_cs_activity_log("bc_am_workflow", "edit_metadata", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    REM FIL JUNE 2013
    Public Sub change_support_doc_categorisation(ByVal id As Long, ByVal ldoc As bc_om_document)

       
        Dim tpoll As Boolean
        Dim slog As New bc_cs_activity_log("bc_am_workflow", "change_support_doc_categorisation", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            tpoll = bc_am_workflow.polling_enabled
            bc_am_workflow.polling_enabled = False
            Dim sdoc As bc_om_document
            'Try
            '    set_server_status("Requesting...")
            'Catch

            'End Try
            sdoc = New bc_om_document
            sdoc = ldoc
            REM get support doc
            Dim i As Integer
            For i = 0 To ldoc.support_documents.document.Count - 1
                If ldoc.support_documents.document(i).id = id Then
                    sdoc = New bc_om_document
                    sdoc = ldoc.support_documents.document(i)
                    Exit For
                End If

            Next
            Dim tdoc_id As Long
            tdoc_id = sdoc.id
            REM get actually document
            sdoc.bwith_document = False

            sdoc.btake_revision = False
            sdoc.bcheck_out = False
          

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                sdoc.db_read()
            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                sdoc.certificate.name = bc_cs_central_settings.logged_on_user_name
                sdoc.certificate.user_id = bc_cs_central_settings.logged_on_user_id
                sdoc.tmode = bc_cs_soap_base_class.tREAD
                If sdoc.transmit_to_server_and_receive(sdoc, True) = False Then
                    Exit Sub
                End If
            End If
            Dim osubmit As New bc_am_at_submit_frm
            osubmit.import_support_doc = True
            osubmit.register_only = False
            osubmit.edit_mode = True
            osubmit.Text = "Blue Curve - Support Document Categorisation"
            osubmit.attach_mode = False

            osubmit.Cpub.Enabled = True

     

            osubmit.ldoc = sdoc
            osubmit.ActiveControl = osubmit.txttitle
            osubmit.Focus()
            osubmit.ShowDialog()
            If Not osubmit.ok_selected Then
                REM if cancel selected do nothing
                Dim ocommentary As New bc_cs_activity_log("bc_am_workflow", "import_master_document", bc_cs_activity_codes.COMMENTARY, "Cancel Selected on Submit form.")
                Exit Sub
            End If

            sdoc.bwith_document = False


            For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                If osubmit.Cpub.Text = bc_am_load_objects.obc_pub_types.pubtype(i).name Then
                    sdoc.pub_type_id = bc_am_load_objects.obc_pub_types.pubtype(i).id()
                    sdoc.pub_type_name = bc_am_load_objects.obc_pub_types.pubtype(i).name
                    Exit For
                End If
            Next

            sdoc.support_documents.document.Clear()

            sdoc.register_only = False
            sdoc.bcheck_out = False
            sdoc.bwith_document = False
            sdoc.history.Clear()
            sdoc.bimport_support_only = True
            bc_am_workflow.cfrm.cursor_wait()
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                sdoc.db_write(Nothing)
            Else
                sdoc.tmode = bc_cs_soap_base_class.tWRITE
                sdoc.transmit_to_server_and_receive(ldoc, True)
            End If
            If sdoc.doc_write_error_text <> "" Then
                Dim omsg As New bc_cs_message("Blue Curve", "Change Support Document Categorisation Failed: " + ldoc.doc_write_error_text, bc_cs_message.MESSAGE, False, False, "yes", "no", True)
            End If
            Me.retrieve_docs(False, False)

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_workflow", "change_support_doc_categorisation", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            processing = False
            bc_am_workflow.polling_enabled = tpoll
            slog = New bc_cs_activity_log("bc_am_workflow", "change_support_doc_categorisation", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    Public Sub look_at_support_doc(ByVal id As Long, ByVal ldoc As bc_om_document, Optional ByVal view_only As Boolean = True)

        Dim omime As bc_am_mime_types
        Dim fn As String
        Dim tpoll As Boolean
        Dim slog As New bc_cs_activity_log("bc_am_workflow", "look_at_support_doc", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            tpoll = bc_am_workflow.polling_enabled
            bc_am_workflow.polling_enabled = False
            Dim sdoc As bc_om_document
            Try
                set_server_status("Requesting...")
            Catch

            End Try
            sdoc = New bc_om_document
            sdoc = ldoc
            REM get support doc
            Dim i As Integer
            For i = 0 To ldoc.support_documents.document.Count - 1
                If ldoc.support_documents.document(i).id = id Then
                    sdoc = New bc_om_document
                    sdoc = ldoc.support_documents.document(i)
                    Exit For
                End If

            Next
            Dim tdoc_id As Long
            tdoc_id = sdoc.id
            REM get actually document
            sdoc.bwith_document = True
            If view_only = True Then
                sdoc.btake_revision = False
                sdoc.bcheck_out = False
            Else
                REM if document is checkedout to you open it from local area
                If sdoc.checked_out_user <> "0" Then
                    fn = bc_cs_central_settings.local_repos_path + CStr(sdoc.id) + sdoc.extension
                    REM check file exists if it doesnt get it of server
                    Dim fs As New bc_cs_file_transfer_services
                    If fs.check_document_exists(fn) = True Then
                        omime = New bc_am_mime_types
                        omime.view(fn, sdoc.extension)
                        Exit Sub
                    Else
                        Dim omessage As New bc_cs_message("Blue Curve - process", "Document is checked out to you but local copy doesnt exist. Server Copy will be extracted.", bc_cs_message.MESSAGE)
                    End If
                End If
                sdoc.btake_revision = True
                sdoc.bcheck_out = True
            End If
            sdoc.bwith_document = True
            REM if edit is requested and document is checked out to current user then attempt to use local
            REM copy if it is already open or doesnt exist flag this


            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                sdoc.db_read()
            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                sdoc.certificate.name = bc_cs_central_settings.logged_on_user_name
                sdoc.certificate.user_id = bc_cs_central_settings.logged_on_user_id
                sdoc.tmode = bc_cs_soap_base_class.tREAD
                If sdoc.transmit_to_server_and_receive(sdoc, True) = False Then
                    Exit Sub
                End If
            End If
            If sdoc.doc_not_found Then
                Dim omessage As New bc_cs_message("Blue Curve", "Document not found on server contact system administrator!", bc_cs_message.MESSAGE)
                Exit Sub
            End If
            If view_only = False Then
                REM if document has become checked out flag this and stop
                If sdoc.doc_not_found Then
                    Dim omessage As New bc_cs_message("Blue Curve", "Document not found on server contact system administrator!", bc_cs_message.MESSAGE)
                    Exit Sub
                End If
                If sdoc.id = 0 Then
                    Dim omessage As New bc_cs_message("Blue Curve", "Document has just become checked out to another user cannot open", bc_cs_message.MESSAGE)
                    Exit Sub
                End If

                If sdoc.id = 0 And ldoc.checked_out_user <> CStr(bc_cs_central_settings.logged_on_user_id) Then
                    Dim omessage As New bc_cs_message("Blue Curve", "Document has just become checked out to another user cant edit!", bc_cs_message.MESSAGE)
                    retrieve_docs(False, False)
                    Exit Sub
                Else
                    REM reset support doc
                    'If sdoc.support_documents.document.Count > 0 Then
                    'sdoc.support_documents.document(i).id = tdid
                    'End If
                End If
            End If
            sdoc.id = tdoc_id
            REM can only quick open a word document
            REM save as view only file
            If view_only = True Then
                set_server_status("Saving View Only Document Locally")

                fn = save_view_only_file(sdoc)
                If fn <> "" Then
                    omime = New bc_am_mime_types
                    set_server_status("Opening Document in Viewer")
                    omime.view(bc_cs_central_settings.local_repos_path + fn + sdoc.extension, sdoc.extension)
                    REM if AT document then mark as read only else it may get submitted or refreshed
                End If

                set_server_status("Document Open for View")
            Else
                set_server_status("Saving Document for Edit Locally")
                fn = save_edit_file(sdoc)
                omime = New bc_am_mime_types
                omime.view(bc_cs_central_settings.local_repos_path + fn, sdoc.extension)
                retrieve_docs(False, False)
            End If
            bc_am_workflow.cfrm.WindowState = FormWindowState.Minimized

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_workflow", "look_at_support_doc", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            processing = False
            bc_am_workflow.polling_enabled = tpoll
            slog = New bc_cs_activity_log("bc_am_workflow", "look_at_support_doc", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    Public Function save_view_only_file(ByVal ldoc As bc_om_document) As String
        Dim slog As New bc_cs_activity_log("bc_am_workflow", "save_view_only_file", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim fs As New bc_cs_file_transfer_services
            Dim fn As String
            Dim i As Integer = 0
            Dim found As Boolean
            save_view_only_file = ""
            fn = "view" + CStr(i) + ldoc.extension
            found = True
            While found = True
                i = i + 1
                fn = "view" + CStr(i) + ldoc.extension
                REM tidy up other files
                fs.delete_file(bc_cs_central_settings.local_repos_path + fn, Nothing, False)
                If fs.check_document_exists(bc_cs_central_settings.local_repos_path + fn) = False Then
                    fs.write_bytestream_to_document(bc_cs_central_settings.local_repos_path + fn, ldoc.byteDoc, Nothing)
                    found = False
                End If
            End While
            save_view_only_file = "view" + CStr(i)
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_workflow", "save_view_only_file", bc_cs_error_codes.USER_DEFINED, ex.Message)
            save_view_only_file = ""
        Finally
            slog = New bc_cs_activity_log("bc_am_workflow", "save_view_only_file", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function
    Private Function save_edit_file(ByVal ldoc As bc_om_document) As String
        Dim slog As New bc_cs_activity_log("bc_am_workflow", "save_edit_file", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim fs As New bc_cs_file_transfer_services
            Dim fn As String
            Dim i As Integer = 0

            'fn = CStr(ldoc.id) + ldoc.extension
            fn = ldoc.filename
            fs.write_bytestream_to_document(bc_cs_central_settings.local_repos_path + fn, ldoc.byteDoc, Nothing)
            save_edit_file = fn
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_workflow", "save_edit_file", bc_cs_error_codes.USER_DEFINED, ex.Message)
            save_edit_file = ""
        Finally
            slog = New bc_cs_activity_log("bc_am_workflow", "save_edit_file", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function
    Public Function check_in(ByVal id As String, ByVal master_doc As Boolean, ByVal ldoc As bc_om_document) As Boolean
        Dim slog As New bc_cs_activity_log("bc_am_workflow", "check_in", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim tpoll As Boolean
        Try
            Dim i As Integer
            Dim omessage As bc_cs_message
            tpoll = bc_am_workflow.polling_enabled
            bc_am_workflow.polling_enabled = False

            check_in = False
            Dim fs As New bc_cs_file_transfer_services
            If master_doc = False Then
                REM match id to correct support doc
                For i = 0 To ldoc.support_documents.document.Count - 1
                    If ldoc.support_documents.document(i).id = id Then
                        ldoc = ldoc.support_documents.document(i)
                        Exit For
                    End If
                Next

            Else
                ldoc.support_documents.document.Clear()
                ldoc = ldoc

            End If
            Dim fn As String
            fn = bc_cs_central_settings.local_repos_path + ldoc.filename
            'fn = bc_cs_central_settings.local_repos_path + CStr(id) + ldoc.extension
            'If fs.delete_file(bc_cs_central_settings.local_repos_path + CStr(id) + ldoc.extension, Nothing, False) = "in use" Then
            'omessage = New bc_cs_message("Blue Curve - process", "Document is Open please save and close prior to check in.", bc_cs_message.MESSAGE)
            'Exit Function
            'End If
            If fs.check_document_exists(fn, Nothing) = False Then
                omessage = New bc_cs_message("Blue Curve - process", "Document: " + fn + " could not be found please locate  or request check out to retrieve last saved server copy.", bc_cs_message.MESSAGE)
                Exit Function
            End If
            If fs.in_use_by_another_process(fn, Nothing) = True Then
                omessage = New bc_cs_message("Blue Curve - process", "Document: " + fn + " is open please save and close prior to check in.", bc_cs_message.MESSAGE)
                Exit Function
            End If
            fs.write_document_to_bytestream(fn, ldoc.byteDoc, Nothing, True)

            If fs.delete_file(fn, Nothing, False) = "in use" Then
                omessage = New bc_cs_message("Blue Curve - process", "Document is Open please save and close prior to check in.", bc_cs_message.MESSAGE)
                Exit Function
            End If

            ldoc.history.Clear()
            ldoc.bwith_document = True
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                ldoc.db_write(Nothing)
            Else
                ldoc.tmode = bc_cs_soap_base_class.tWRITE
                ldoc.no_send_back = True
                ldoc.transmit_to_server_and_receive(ldoc, True)
            End If

      
            REM delete local metadata file if exists
            fs.delete_file(bc_cs_central_settings.local_repos_path + CStr(id) + ".dat", Nothing, False)
            check_in = True
            REM remove record from local documents
            REM delete local record if exists
            Dim local_docs As New bc_om_documents
            REM read in local documents list
            If fs.check_document_exists(bc_cs_central_settings.local_repos_path + "bc_local_documents.dat") Then
                local_docs = local_docs.read_data_from_file(bc_cs_central_settings.local_repos_path + "bc_local_documents.dat")
                REM delete this document
                For i = 0 To local_docs.document.Count - 1
                    REM steve wooderson 08/07/2013 local docs
                    If (local_docs.document(i).id = ldoc.id And ldoc.id <> 0) _
                        Or (local_docs.document(i).filename = ldoc.filename) Then
                        local_docs.document.RemoveAt(i)
                        Exit For
                    End If
                Next
                REM save back down
                local_docs.write_data_to_file(bc_cs_central_settings.local_repos_path + "bc_local_documents.dat")
            End If


        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_workflow", "check_in", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            retrieve_docs(False, False)
            bc_am_workflow.polling_enabled = tpoll
            slog = New bc_cs_activity_log("bc_am_workflow", "check_in", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function
    Private Function txtpoll() As String
        'If bc_am_workflow.polling_enabled = "FALSE" Then
        '    txtpoll = "Polling Disabled"
        'Else
        '    If bc_am_workflow.alerter_enabled = "TRUE" And bc_am_workflow.screen_poll = "TRUE" And (bc_am_workflow.screen_update_enabled = True) Then
        '        txtpoll = "All Polling Enabled " + CStr(bc_am_workflow.polling_interval / 1000) + "s"

        '    ElseIf (bc_am_workflow.alerter_enabled = "TRUE") Then
        '        txtpoll = "Alerter Enabled " + CStr(bc_am_workflow.polling_interval / 1000) + "s"
        '    ElseIf (bc_am_workflow.screen_poll = "TRUE") And (bc_am_workflow.screen_update_enabled = True) Then
        '        txtpoll = "Screen Polling Enabled " + CStr(bc_am_workflow.polling_interval / 1000) + "s"
        '    Else
        '        txtpoll = "Screen Polling Disabled"
        '    End If
        'End If
        'If bc_am_workflow.auto_refresh = True Then
        '    txtpoll = txtpoll + "; Auto Refresh Enabled"
        '    REM txtpoll = txtpoll + "; Auto Refresh On: " + CStr(bc_am_workflow.screen_refresh_interval) + "s  "
        '    REM Inactive " + CStr(bc_am_workflow.screen_inactive_interval) + "s"
        'Else
        '    txtpoll = txtpoll + "; Auto Refresh Off"
        'End If

        Dim LastText As String = ""
        Dim n As Integer
        n = InStr(bc_am_workflow.cfrm.lpoll.Text, " Updated:")
        If n > 0 Then
            LastText = Mid(bc_am_workflow.cfrm.lpoll.Text, n)
        End If

        txtpoll = ""
        If bc_am_workflow.polling_enabled = False Then
            txtpoll = "Polling Disabled"
        Else
            If bc_am_workflow.alerter_enabled = True And (bc_am_workflow.screen_poll = True) Then
                txtpoll = "All Polling Enabled"
            ElseIf (bc_am_workflow.alerter_enabled = True) Then
                txtpoll = "Alerter Enabled"
            Else
                txtpoll = "Polling Enabled"
            End If
        End If

        If bc_am_workflow.auto_refresh = True Then
            txtpoll = txtpoll + "; Auto Refresh Enabled."
        Else
            txtpoll = txtpoll + "; Auto Refresh Off."
        End If

        txtpoll = txtpoll + LastText


    End Function
    Public Sub view_doc(ByVal ldoc As bc_om_document)
        Dim slog As New bc_cs_activity_log("bc_am_workflow", "view_doc", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            REM steve wooderson 08/07/2013 local docs
            If bc_am_workflow.cfrm.uxLocal.Checked Then
                REM oopen_doc.open_read_only(ldoc)
                oopen_doc.open(True, ldoc)
            Else
                Dim odoc As New bc_am_at_open_doc
                odoc.open_read_only(ldoc)
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_workflow", "view_doc", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            slog = New bc_cs_activity_log("bc_am_workflow", "view_doc", bc_cs_activity_codes.TRACE_ENTRY, "")

        End Try

    End Sub

    Public Sub open_doc(ByVal ldoc As bc_om_document)
        Dim slog As New bc_cs_activity_log("bc_am_workflow", "open_doc", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim tpoll As Boolean

        Try
            Dim odoc As New bc_am_at_open_doc
            Dim fn As String
            tpoll = bc_am_workflow.polling_enabled
            bc_am_workflow.polling_enabled = False

            ldoc.history.Clear()
            If ldoc.checked_out_user <> 0 Then

                If ldoc.checked_out_user = bc_cs_central_settings.logged_on_user_id Then
                    fn = bc_cs_central_settings.local_repos_path + ldoc.filename
                    REM check file exists if it doesnt get it of server
                    Dim fs As New bc_cs_file_transfer_services
                    If fs.check_document_exists(fn) = True Then
                        set_server_status("opening document for edit from local")
                        odoc.open(True, ldoc, True)
                        set_server_status("")
                        Exit Sub
                    Else
                        Dim omessage As New bc_cs_message("Blue Curve - process", "Document is checked out to you but local copy doesnt exist. Server Copy will be extracted.", bc_cs_message.MESSAGE)
                    End If
                End If
            End If
            set_server_status("Checking out document from server")

            REM steve wooderson 08/07/2013 local docs
            If bc_am_workflow.cfrm.uxLocal.Checked Then
                oopen_doc.open(True, ldoc)
            Else
                odoc.open(False, ldoc, True)
            End If

            ldoc.history.Clear()
            ldoc.support_documents.document.Clear()
            set_server_status("")

            retrieve_docs(False, False)
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_workflow", "open_doc", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            bc_am_workflow.polling_enabled = tpoll
            slog = New bc_cs_activity_log("bc_am_workflow", "open_doc", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    Public Sub restore_defaults()
        Dim slog As New bc_cs_activity_log("bc_am_workflow", "restore_defaults", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim fs As New bc_cs_file_transfer_services
            Dim fn As String
            fn = bc_cs_central_settings.get_user_dir + "\" + CStr(bc_cs_central_settings.logged_on_user_id) + "_" + WORKFLOW_SETTINGS
            If fs.check_document_exists(fn) = True Then
                fs.delete_file(fn)
            End If
            set_defaults()

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_workflow", "restore_defaults", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_workflow", "restore_defaults", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Public Sub local_refresh()
        REM steve wooderson 08/07/2013 local docs
        REM load local documents and refresh form.

        Dim localDocs As New bc_om_documents
        Dim from_date As Date
        Dim to_date As Date
        Dim localdoc_date As Date

        from_date = bc_am_workflow.cfrm.FilterDateFrom.Value.ToString("d")
        to_date = bc_am_workflow.cfrm.FilterDateTo.Value.ToString("d")

        REM set up with local docs
        oopen_doc = New bc_am_at_open_doc(True, True, bc_am_workflow.cfrm.FilterDateFrom.Value, bc_am_workflow.cfrm.FilterDateTo.Value, False, True)
        localDocs.date_from = oopen_doc.onetwork_docs.date_from
        localDocs.date_to = oopen_doc.onetwork_docs.date_to

        For l = 0 To oopen_doc.onetwork_docs.document.Count - 1
            REM filter on dates and local documents only.
            localdoc_date = oopen_doc.onetwork_docs.document(l).doc_date
            localdoc_date = localdoc_date.ToString("d")
            If localdoc_date >= from_date And localdoc_date <= to_date _
            And oopen_doc.onetwork_docs.document(l).id = 0 Then
                localDocs.document.Add(oopen_doc.onetwork_docs.document(l))
            End If
        Next
        bc_am_workflow.docs = localDocs

        ofrm.load_data(bc_am_workflow.mode)
        ofrm.apply_sort()

        Dim tx As String
        Dim n As Integer
        tx = bc_am_workflow.cfrm.lpoll.Text
        n = InStr(tx, "Updated:")
        If n > 0 Then
            tx = Left(tx, n - 2)
        End If
        bc_am_workflow.cfrm.lpoll.Text = tx + " Updated: " + Format(Now, "HH:mm:ss")

    End Sub


    Public Sub loadDefaultSettings()
        set_defaults()
    End Sub
End Class
Public Class bc_am_show_tip_text
    Public Shared tshow_tip_text As Thread
    Public Shared ott As bc_am_tip_text
    Dim tx As String
    Public Sub New(ByVal tx As String)
        Try
            Me.tx = tx
            tshow_tip_text = New Thread(AddressOf show_tip_text)
            tshow_tip_text.Start()
        Catch

        End Try
    End Sub
    Private Sub show_tip_text()
        Try
            Dim i As Integer = 2
            Thread.Sleep(1000)
            ott = New bc_am_tip_text
            ott.RichTextBox1.Text = tx
            ott.ShowDialog()
        Catch

        End Try
    End Sub
    Public Shared Sub abort()
        Try
            ott.Hide()
            tshow_tip_text.Abort()
        Catch

        End Try
    End Sub
End Class

Friend Class taxonomy
    Public name As String
    Public all As Boolean
    Public taxonomy_items As New ArrayList

    Public Sub New()

    End Sub
End Class
Friend Class taxonomy_Item
    Public name As String
    Public id As String
    Public selected As Boolean
End Class

REM Workflow Process Colors
Public Class ProcessColors
    Public workflow_current_stage_backcolor As System.Drawing.Color
    Public workflow_current_stage_forecolor As System.Drawing.Color
    Public workflow_next_stage_backcolor As System.Drawing.Color
    Public workflow_next_stage_forecolor As System.Drawing.Color
    Public doc_list_read_backcolor As System.Drawing.Color
    Public doc_list_read_forecolor As System.Drawing.Color
    Public doc_list_unread_backcolor As System.Drawing.Color
    Public doc_list_unread_forecolor As System.Drawing.Color
    Public doc_list_search_backcolor As System.Drawing.Color
    Public doc_list_search_forecolor As System.Drawing.Color
    Public doc_list_urgent_backcolor As System.Drawing.Color
    Public doc_list_urgent_forecolor As System.Drawing.Color
    Public doc_list_expired_backcolor As System.Drawing.Color
    Public doc_list_expired_forecolor As System.Drawing.Color
    Public doc_list_stage_changed_backcolor As System.Drawing.Color
    Public doc_list_stage_changed_forecolor As System.Drawing.Color

    'Set colors to default in the constructor
    Public Sub New()
        resetDefaultColors()
    End Sub

    'Set the default color scheme    
    Public Sub resetDefaultColors()
        Me.doc_list_expired_backcolor = System.Drawing.Color.LightCoral
        Me.doc_list_expired_forecolor = System.Drawing.Color.Black
        Me.doc_list_read_backcolor = System.Drawing.Color.LightGray
        Me.doc_list_read_forecolor = System.Drawing.Color.Black
        Me.doc_list_search_backcolor = System.Drawing.Color.LightBlue
        Me.doc_list_search_forecolor = System.Drawing.Color.Black
        Me.doc_list_stage_changed_backcolor = System.Drawing.Color.LightYellow
        Me.doc_list_stage_changed_forecolor = System.Drawing.Color.Black
        Me.doc_list_unread_backcolor = System.Drawing.Color.LightGreen
        Me.doc_list_unread_forecolor = System.Drawing.Color.Black
        Me.doc_list_urgent_backcolor = System.Drawing.Color.LightPink
        Me.doc_list_urgent_forecolor = System.Drawing.Color.Black
        Me.workflow_current_stage_backcolor = System.Drawing.Color.LightGreen
        Me.workflow_current_stage_forecolor = System.Drawing.Color.Black
        Me.workflow_next_stage_backcolor = System.Drawing.Color.LightCoral
        Me.workflow_next_stage_forecolor = System.Drawing.Color.Black
       
    End Sub

    Public Function convertForConfig() As Hashtable
        Dim output As New Hashtable
        output.Add("doc_list_expired_backcolor", Me.doc_list_expired_backcolor.ToArgb())
        output.Add("doc_list_expired_forecolor", Me.doc_list_expired_forecolor.ToArgb())
        output.Add("doc_list_read_backcolor", Me.doc_list_read_backcolor.ToArgb())
        output.Add("doc_list_read_forecolor", Me.doc_list_read_forecolor.ToArgb())
        output.Add("doc_list_search_backcolor", Me.doc_list_search_backcolor.ToArgb())
        output.Add("doc_list_search_forecolor", Me.doc_list_search_forecolor.ToArgb())
        output.Add("doc_list_stage_changed_backcolor", Me.doc_list_stage_changed_backcolor.ToArgb())
        output.Add("doc_list_stage_changed_forecolor", Me.doc_list_stage_changed_forecolor.ToArgb())
        output.Add("doc_list_unread_backcolor", Me.doc_list_unread_backcolor.ToArgb())
        output.Add("doc_list_unread_forecolor", Me.doc_list_unread_forecolor.ToArgb())
        output.Add("doc_list_urgent_backcolor", Me.doc_list_urgent_backcolor.ToArgb())
        output.Add("doc_list_urgent_forecolor", Me.doc_list_urgent_forecolor.ToArgb())
        output.Add("workflow_current_stage_backcolor", Me.workflow_current_stage_backcolor.ToArgb())
        output.Add("workflow_current_stage_forecolor", Me.workflow_current_stage_forecolor.ToArgb())
        output.Add("workflow_next_stage_backcolor", Me.workflow_next_stage_backcolor.ToArgb())
        output.Add("workflow_next_stage_forecolor", Me.workflow_next_stage_forecolor.ToArgb())

        convertForConfig = output
    End Function

    Public Sub loadFromConfig(ByVal savedValues As Hashtable)
        Me.doc_list_expired_backcolor = System.Drawing.Color.FromArgb(savedValues("doc_list_expired_backcolor"))
        Me.doc_list_expired_forecolor = System.Drawing.Color.FromArgb(savedValues("doc_list_expired_forecolor"))
        Me.doc_list_read_backcolor = System.Drawing.Color.FromArgb(savedValues("doc_list_read_backcolor"))
        Me.doc_list_read_forecolor = System.Drawing.Color.FromArgb(savedValues("doc_list_read_forecolor"))
        Me.doc_list_search_backcolor = System.Drawing.Color.FromArgb(savedValues("doc_list_search_backcolor"))
        Me.doc_list_search_forecolor = System.Drawing.Color.FromArgb(savedValues("doc_list_search_forecolor"))
        Me.doc_list_stage_changed_backcolor = System.Drawing.Color.FromArgb(savedValues("doc_list_stage_changed_backcolor"))
        Me.doc_list_stage_changed_forecolor = System.Drawing.Color.FromArgb(savedValues("doc_list_stage_changed_forecolor"))
        Me.doc_list_unread_backcolor = System.Drawing.Color.FromArgb(savedValues("doc_list_unread_backcolor"))
        Me.doc_list_unread_forecolor = System.Drawing.Color.FromArgb(savedValues("doc_list_unread_forecolor"))
        Me.doc_list_urgent_backcolor = System.Drawing.Color.FromArgb(savedValues("doc_list_urgent_backcolor"))
        Me.doc_list_urgent_forecolor = System.Drawing.Color.FromArgb(savedValues("doc_list_urgent_forecolor"))
        Me.workflow_current_stage_backcolor = System.Drawing.Color.FromArgb(savedValues("workflow_current_stage_backcolor"))
        Me.workflow_current_stage_backcolor = System.Drawing.Color.FromArgb(savedValues("workflow_current_stage_backcolor"))
        Me.workflow_next_stage_backcolor = System.Drawing.Color.FromArgb(savedValues("workflow_next_stage_backcolor"))
        Me.workflow_next_stage_forecolor = System.Drawing.Color.FromArgb(savedValues("workflow_next_stage_forecolor"))

    End Sub
End Class
REM class called from wondows service to poll server for documents
REM and detect alerts
Public Class bc_am_sv_poll_for_alerts
    Dim oalertfrm As bc_am_workflow_alert
    Dim t As Thread
    Dim oalert As New bc_om_alert
    Public ondoc As New bc_om_documents
    Public pdoc As bc_am_at_open_doc
    Public new_checked_out_docs As New ArrayList
    Public Sub New()
        Dim bcs As New bc_cs_central_settings(True)
        bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.connection_method
        REM if ado check user is an authenticated system user
    End Sub
    Public Sub start_poll()
        t = New Thread(AddressOf check_documents)
        t.Start()
    End Sub
    Public Sub stop_poll()
        t.Abort()
    End Sub
    Private Sub logged_on_user()

    End Sub
    Private Sub check_documents()
        Try
            Dim ondoc As New bc_om_documents
            Dim ouser As New bc_om_user
            Dim atx As String
            Dim doc_id As Long
            Dim first_poll As Boolean
            Dim suspend_poll As Boolean = False
            Dim new_checked_out_docs As New ArrayList
            Dim date_from As DateTime
            Dim blogging As String
            Dim fs As New bc_cs_file_transfer_services

            Dim ado_authentication_failed As Boolean

            first_poll = True
            While True
                atx = ""
                doc_id = 0
                If suspend_poll = False Then
                    ouser.os_user_name = "not set"
                    suspend_poll = True
                    Dim bcs As New bc_cs_central_settings(True)
                    blogging = bc_cs_central_settings.activity_file
                    bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.connection_method
                    If bc_cs_central_settings.service_poll_enabled = 1 And check_process() = False Then
                        Dim ocomm As New bc_cs_activity_log("bc_am_sv_poll_for_alerts", "new_documents", bc_cs_activity_codes.COMMENTARY, "Polling interval set to:" + CStr(bc_cs_central_settings.poll_interval) + " type: " + bc_cs_central_settings.service_poll_type, Nothing)
                        ado_authentication_failed = False
                        bc_cs_central_settings.activity_file = "off"
                        If fs.check_document_exists_and_can_be_read(bc_cs_central_settings.get_user_dir + "\bc_current_user.dat") = False Then
                            bc_cs_central_settings.activity_file = blogging
                            ocomm = New bc_cs_activity_log("bc_am_sv_poll_for_alerts", "new_documents", bc_cs_activity_codes.COMMENTARY, "bc_current_user.xml doesnt exists so polling disabled.", Nothing)
                            bc_cs_central_settings.activity_file = "off"
                        Else
                            REM blue curve communicator comment this out for now
                            'check_messages()
                            REM get date from 
                            Dim da As New DateTime
                            da = Now
                            Dim tp As TimeSpan
                            tp = New TimeSpan(bc_cs_central_settings.service_poll_date_range, 0, 0, 0, 0)
                            date_from = da.Subtract(tp)
                            ouser.os_user_name = bc_cs_central_settings.GetLoginName
                            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                                REM ADO
                                bc_cs_central_settings.logged_on_user_id = ouser.check_authentication
                                If bc_cs_central_settings.logged_on_user_id = 0 Then
                                    bc_cs_central_settings.activity_file = blogging
                                    ocomm = New bc_cs_activity_log("bc_am_sv_poll_for_alerts", "new_documents", bc_cs_activity_codes.COMMENTARY, "authentication failed for: " + CStr(ouser.os_user_name), Nothing)
                                Else
                                    check_documents(date_from, first_poll, ouser.os_user_name)
                                End If
                            Else
                                REM SOAP
                                If check_documents(date_from, first_poll, ouser.os_user_name) = False Then
                                    bc_cs_central_settings.activity_file = blogging
                                    ocomm = New bc_cs_activity_log("bc_am_sv_poll_for_alerts", "new_documents", bc_cs_activity_codes.COMMENTARY, "soap authentication failed for: " + CStr(ouser.os_user_name), Nothing)
                                End If
                            End If
                        End If
                        first_poll = False
                        suspend_poll = False
                        bc_cs_central_settings.activity_file = blogging
                    Else
                        Dim ocomm As New bc_cs_activity_log("bc_am_sv_poll_for_alerts", "new_document", bc_cs_activity_codes.COMMENTARY, "Service Polling disabled", Nothing)
                        suspend_poll = False
                        first_poll = True
                    End If
                End If
                Thread.Sleep(bc_cs_central_settings.poll_interval)
            End While
        Catch ex As Exception
            Dim ocommentary As New bc_cs_activity_log("bc_am_sv_poll_fr_alerts", "check_documents", bc_cs_activity_codes.COMMENTARY, "POLL ERROR:" + ex.Message)

        End Try
    End Sub
    Private Sub check_messages()
        Dim onew_messages As New bc_om_user_messages

        onew_messages.current_user_id = 1
        onew_messages.from_service_alert = True
        If bc_cs_central_settings.connection_method = bc_cs_central_settings.ADO Then
            onew_messages.db_read_new_threads()
        Else
            onew_messages.tmode = bc_om_user_messages.tREAD_NEW
            onew_messages.transmit_to_server_and_receive(onew_messages, True)
        End If

        If onew_messages.message_threads.Count > 1 Then
            write_out_alert("Communicator", "Messages have arrived", 0, 0)
        Else
            write_out_alert("Communicator", onew_messages.message_threads(0).message, 0, 0)
        End If
    End Sub
    Private Function check_documents(ByVal date_from As DateTime, ByVal first_poll As Boolean, ByVal os_user_name As String) As Boolean
        Dim i, j, k As Integer
        Dim found As Boolean
        Dim alert_flag As Boolean
        Dim atx As String = ""
        Dim doc_id As Long
        Dim wdocs As New bc_om_documents
        Dim process_mode As Integer = 1
        Try
            check_documents = True
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                If bc_cs_central_settings.service_poll_type = "Process" Then
                    wdocs.workflow_mode = True
                    wdocs.date_from = date_from
                    wdocs.date_to = "9-9-9999"
                    wdocs.db_read()
                Else
                    pdoc = New bc_am_at_open_doc(False, True, date_from, "9-9-9999", False, False, False)
                    wdocs = pdoc.onetwork_docs
                End If
            Else
                If bc_cs_central_settings.service_poll_type = "Process" Then
                    wdocs.workflow_mode = True
                    wdocs.date_from = date_from
                    wdocs.date_to = "9-9-9999"
                    wdocs.tmode = bc_cs_soap_base_class.tREAD
                    wdocs.packet_code = "spoll"

                    If wdocs.transmit_to_server_and_receive(wdocs, False) = False Then
                        check_documents = False
                    End If

                Else
                    pdoc = New bc_am_at_open_doc(False, True, date_from, "9-9-9999", False, False, False)
                    wdocs = pdoc.onetwork_docs
                    If pdoc.authentication_failed = True Then
                        check_documents = False
                    End If
                End If
            End If

            found = False
            alert_flag = False
            If ondoc.document.Count < wdocs.document.Count And first_poll = False Then
                If ondoc.document.Count - wdocs.document.Count = -1 Then
                    For i = 0 To wdocs.document.Count - 1
                        found = False
                        For j = 0 To ondoc.document.Count - 1
                            If ondoc.document(j).id = wdocs.document(i).id Then
                                found = True
                                Exit For
                            End If
                        Next
                        If found = False Then
                            If wdocs.document(i).checked_out_user = 0 Then
                                alert_flag = True
                                If bc_cs_central_settings.service_poll_type = "Process" Then
                                    atx = "New Document has arrived in Process: " + wdocs.document(i).title + " in stage: " + wdocs.document(i).stage_name + ". "
                                Else
                                    atx = "New Document has arrived in Create: " + wdocs.document(i).title + " for my teams in stage: " + wdocs.document(i).stage_name + ". "
                                End If
                                doc_id = wdocs.document(i).id
                                Exit For
                            Else
                                REM do not alert yet has checed out
                                alert_flag = False
                                new_checked_out_docs.Add(wdocs.document(i).id)
                            End If
                            Exit For
                        End If
                    Next
                Else
                    If wdocs.document.Count > ondoc.document.Count Then
                        alert_flag = True
                        If bc_cs_central_settings.service_poll_type = "Process" Then
                            atx = CStr(wdocs.document.Count - ondoc.document.Count) + " New Documents have arrived in Process. "
                        Else
                            atx = CStr(wdocs.document.Count - ondoc.document.Count) + " New Documents have arrived in Create for my teams. "
                        End If
                    End If
                End If
            End If
            Dim patx As String
            patx = atx
            REM checked if new checked out document is now checked in
            k = new_checked_out_docs.Count
            i = 0

            While i < k
                For j = 0 To wdocs.document.Count - 1
                    If new_checked_out_docs(i) = wdocs.document(j).id And wdocs.document(j).checked_out_user = "0" Then
                        If alert_flag = False Then
                            atx = "New Document has arrived in Create: " + wdocs.document(j).title + " for my teams in stage: " + wdocs.document(i).stage_name + ". "
                            doc_id = wdocs.document(i).id
                        Else
                            If bc_cs_central_settings.service_poll_type = "Process" Then
                                atx = "New Documents have arrived in Process."
                            Else
                                atx = "New Documents have arrived in Create."
                            End If
                        End If
                        new_checked_out_docs.RemoveAt(i)
                        i = i - 1
                        k = k - 1
                        alert_flag = True
                        Exit For
                    End If
                Next
                i = i + 1
            End While
            REM check for stage change
            Dim more_than_one As Boolean = False
            For i = 0 To ondoc.document.Count - 1
                For j = 0 To wdocs.document.Count - 1
                    If wdocs.document(j).id = ondoc.document(i).id Then
                        If ondoc.document(i).stage_name <> wdocs.document(j).stage_name Then
                            alert_flag = True
                            If more_than_one = True Then
                                process_mode = 4
                                If bc_cs_central_settings.service_poll_type = "Process" Then
                                    atx = patx + "Documents have changed stage in Process."

                                Else
                                    atx = patx + "Documents have changed stage in Create."
                                End If
                            Else
                                process_mode = 4
                                If bc_cs_central_settings.service_poll_type = "Process" Then
                                    atx = patx + "Document has changed stage: " + ondoc.document(i).title + " from " + ondoc.document(i).stage_name + " to  " + wdocs.document(j).stage_name + " in Process"

                                Else
                                    atx = patx + "Document has changed stage: " + ondoc.document(i).title + " from " + ondoc.document(i).stage_name + " to  " + wdocs.document(j).stage_name + " in Create"
                                End If
                                doc_id = ondoc.document(i).id
                                more_than_one = True
                            End If
                        End If
                        Exit For
                    End If
                Next
            Next
            If alert_flag = True Then
                Dim blogging As String
                blogging = bc_cs_central_settings.activity_file
                bc_cs_central_settings.activity_file = "on"
                write_out_alert(bc_cs_central_settings.service_poll_type, atx, doc_id, process_mode)
                bc_cs_central_settings.activity_file = blogging
            End If

            REM check my documents
            ondoc.document.Clear()
            For i = 0 To wdocs.document.Count - 1
                ondoc.document.Add(wdocs.document(i))
            Next
        Catch ex As Exception
            Dim ocommentary As New bc_cs_activity_log("bc_am_sv_poll_fr_alerts", "check_create_documents", bc_cs_activity_codes.COMMENTARY, "POLL ERROR:" + ex.Message)
        End Try
    End Function
    Private Sub write_out_alert(ByVal application As String, ByVal atx As String, ByVal doc_id As Long, ByVal process_mode As Integer)
        Dim str_shell As String = ""

        Try
            oalert.doc_id = doc_id
            oalert.alert_tx = atx
            oalert.application = application
            oalert.datetime = Now
            oalert.process_mode = process_mode
            'bc_cs_central_settings.activity_file = "on"
            'Dim ocommentary As New bc_cs_activity_log("bc_am_sv_poll_for_alerts", "invoke_alert_form", bc_cs_activity_codes.COMMENTARY, "writing alert file to : " + bc_cs_central_settings.get_user_dir + "\bc_alert.dat", Nothing)
            'oalert.write_xml_to_file(bc_cs_central_settings.get_user_dir + "\bc_alert.dat")
            REM now run shell to invoke alert
            invoke_alert_form()
            'str_shell = bc_cs_central_settings.get_user_dir + "\bluecurve.alert.ap.exe"
            'Shell(str_shell)
        Catch ex As Exception
            Dim ocommentary As New bc_cs_activity_log("bc_am_sv_poll_for_alerts", "invoke_alert_form", bc_cs_activity_codes.COMMENTARY, "Error Launching: " + str_shell + " : " + ex.Message, Nothing)
        End Try
    End Sub
    Public Sub invoke_alert_form()
        Try
            Dim bcs As New bc_cs_central_settings(True)
            REM read alert file
            'Dim fs As New bc_cs_file_transfer_services
            'If fs.check_document_exists_and_can_be_read(bc_cs_central_settings.get_user_dir + "\bc_alert.dat") Then
            'oalert = oalert.read_xml_from_file(bc_cs_central_settings.get_user_dir + "\bc_alert.dat")
            Dim talertform As New Thread(AddressOf alert_form)
            talertform.Start()
            Thread.Sleep(bc_cs_central_settings.alert_interval)
            oalertfrm.Close()
            'Else
            bc_cs_central_settings.activity_file = "on"
            Dim ocommentary As New bc_cs_activity_log("wot", "invoke_alert_form", bc_cs_activity_codes.COMMENTARY, "Unable to read alert file: " + bc_cs_central_settings.get_user_dir + "bc_alert.dat", Nothing)
            'End If
        Catch ex As Exception
            Dim ocommentary As New bc_cs_activity_log("bc_am_sv_poll_for_alerts", "invoke_alert_form", bc_cs_activity_codes.COMMENTARY, "Error Launching alerter: " + ex.Message, Nothing)
        End Try

    End Sub
    Private Sub alert_form()
        oalertfrm = New bc_am_workflow_alert
        oalertfrm.from_service = True
        oalertfrm.lalert.Text = oalert.alert_tx
        If oalert.application = "Communicator" Then
            oalertfrm.Text = "Blue Curve - Communicator"
        End If
        oalertfrm.application = oalert.application
        oalertfrm.doc_id = oalert.doc_id
        oalertfrm.process_mode = oalert.process_mode
        Dim oapi As New API
        API.SetWindowPos(oalertfrm.Handle.ToInt32, API.HWND_TOPMOST, Nothing, 1, 1, 1, 1)
        oalertfrm.ShowDialog()
    End Sub
    Private Function check_process() As Boolean
        check_process = False
        REM if create or process already running then disable alerter
        If System.Diagnostics.Process.GetProcessesByName("bluecurve.create.ap").GetLength(0) > 0 Then
            check_process = True
        End If
        If System.Diagnostics.Process.GetProcessesByName("bluecurve.process.ap").GetLength(0) > 0 Then
            check_process = True
        End If
        If bc_cs_central_settings.create_client_poll_enabled = True Or bc_cs_central_settings.process_client_poll_enabled Then
            check_process = True
        End If
    End Function

End Class




