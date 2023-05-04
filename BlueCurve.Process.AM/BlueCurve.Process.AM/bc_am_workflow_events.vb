Imports System.Collections
Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports BlueCurve.Core.AS
Imports BlueCurve.Create.AM
Imports System.threading
Imports System.IO
Imports System.Windows.Forms

REM Workflow Events Handler
Public Class bc_am_workflow_events_handler
    Public success As Boolean
    Public action_ids As New ArrayList
    Public doc_id As Long
    Public obj As Object
    Public client_flag As Boolean
    Public ldoc As bc_om_document
    Public doc_open As Boolean
    Public doc_title As String
    Public stage_id_to As Long
    Public stage_name_to As String
    Public odoc As New bc_am_at_open_doc
    Public override_stage As Long
    Public suppress_no_trans As Boolean = False
    Public Sub New(ByVal obj As Object, ByRef current_document As bc_om_document, ByVal client_flag As Boolean)
        Me.action_ids = current_document.action_Ids
        Me.doc_id = current_document.id
        Me.client_flag = client_flag
        Me.doc_title = current_document.title
        Me.ldoc = current_document
        REM PR check this
        Me.obj = obj
        success = False
    End Sub
    Public Sub run_events(ByRef current_document As bc_om_document, Optional ByVal doc_open As Boolean = True)
        REM run each event until an event fails
        REM if all succeed then set success to true
        Dim slog = New bc_cs_activity_log("bc_am_workflow_events_handler", "run_events", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim ocommentary As bc_cs_activity_log
        Try
            Dim i, j As Integer
            Dim history As String = ""
            current_document.server_side_events.events.Clear()
            Me.ldoc = current_document
            REM clear out server side events
            bc_cs_central_settings.progress_bar.unload()
            bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve - create", "Workflow Events", 12, False, True)
            bc_cs_central_settings.progress_bar.show()
            Dim suppress As Boolean = False
            Dim server_side_event As Boolean
            For i = 0 To action_ids.Count - 1
                REM match event to action list then execute
                For j = 0 To bc_am_load_objects.obc_pub_types.actions.actions.Count - 1
                    If action_ids(i) = bc_am_load_objects.obc_pub_types.actions.actions(j).id Then
                        bc_cs_central_settings.progress_bar.increment("Running Event " + bc_am_load_objects.obc_pub_types.actions.actions(j).name + " on Document: " + doc_title)
                        REM system defined events
                        If bc_am_load_objects.obc_pub_types.actions.actions(j).calling_object = "system event" Or bc_am_load_objects.obc_pub_types.actions.actions(j).calling_object = "workflow_events.asp" Then
                            ocommentary = New bc_cs_activity_log("bc_am_workflow_events_handler", "run_events", bc_cs_activity_codes.COMMENTARY, "Running System Event " + bc_am_load_objects.obc_pub_types.actions.actions(j).name + " on Document: " + doc_title)
                            history = ""
                            server_side_event = False
                            success = run_system_event(bc_am_load_objects.obc_pub_types.actions.actions(j).name, current_document, server_side_event, suppress, doc_open, history)
                        Else
                            ocommentary = New bc_cs_activity_log("bc_am_workflow_events_handler", "run_events", bc_cs_activity_codes.COMMENTARY, "Event " + bc_am_load_objects.obc_pub_types.actions.actions(j).name + " not implemented")
                        End If

                        'current_document = current_document.read_xml_from_file(bc_cs_central_settings.local_repos_path + CStr(current_document.id) + ".xml")
                        REM if conflicts stop other events
                        If history <> "" Then
                            current_document.history.Add(history)
                            If Me.success = True And suppress = True Then
                                ocommentary = New bc_cs_activity_log("bc_am_workflow_events_handler", "run_events", bc_cs_activity_codes.COMMENTARY, "Restrictions so furthur events supressed")
                                history = "Client Side Event:  " + bc_am_load_objects.obc_pub_types.actions.actions(j).name + " ran sucessfully with restrictions so suppressing other events."
                                current_document.history.Add(history)
                                'current_document.write_xml_to_file(bc_cs_central_settings.local_repos_path + CStr(current_document.id) + ".xml")

                                Exit Sub
                            End If
                        Else
                            If Me.success = True And suppress = True Then

                                ocommentary = New bc_cs_activity_log("bc_am_workflow_events_handler", "run_events", bc_cs_activity_codes.COMMENTARY, "Restrictions so furthur events supressed")
                                history = "Client Side Event:  " + bc_am_load_objects.obc_pub_types.actions.actions(j).name + " ran sucessfully with restrictions so suppressing other events."
                                current_document.history.Add(history)
                                'current_document.write_xml_to_file(bc_cs_central_settings.local_repos_path + CStr(current_document.id) + ".xml")

                                Exit Sub
                            End If
                            If Me.success = False Then
                                ocommentary = New bc_cs_activity_log("bc_am_workflow_events_handler", "run_events", bc_cs_activity_codes.COMMENTARY, "Event " + bc_am_load_objects.obc_pub_types.actions.actions(j).name + " failed on Document:" + doc_title)
                                history = "Client Side Event:  " + bc_am_load_objects.obc_pub_types.actions.actions(j).name + " failed run by:  " + bc_cs_central_settings.logged_on_user_name
                                current_document.history.Add(history)
                                'current_document.write_xml_to_file(bc_cs_central_settings.local_repos_path + CStr(current_document.id) + ".xml")
                                Exit Sub
                            Else
                                If server_side_event = False Then
                                    history = "Client Side Event:  " + bc_am_load_objects.obc_pub_types.actions.actions(j).name + " succeeded run by:  " + bc_cs_central_settings.logged_on_user_name
                                    ocommentary = New bc_cs_activity_log("bc_am_workflow_events_handler", "run_events", bc_cs_activity_codes.COMMENTARY, "Event " + bc_am_load_objects.obc_pub_types.actions.actions(j).name + " succeeded on Document:" + doc_title)
                                Else
                                    history = "Client Side Event:  " + bc_am_load_objects.obc_pub_types.actions.actions(j).name + " set for server execution run by:  " + bc_cs_central_settings.logged_on_user_name
                                    ocommentary = New bc_cs_activity_log("bc_am_workflow_events_handler", "run_events", bc_cs_activity_codes.COMMENTARY, "Event " + bc_am_load_objects.obc_pub_types.actions.actions(j).name + " set for server execution on Document:" + doc_title)
                                End If
                                current_document.history.Add(history)
                                'current_document.write_xml_to_file(bc_cs_central_settings.local_repos_path + CStr(current_document.id) + ".xml")

                            End If
                        End If
                    End If
                Next
            Next
            Me.success = True

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_workflow_events_handler", "run_events", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            bc_cs_central_settings.progress_bar.unload()
            bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve - create", "Submit", 12, False, True)
            slog = New bc_cs_activity_log("bc_am_workflow_events_handler", "run_events", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Public Function run_system_event(ByVal action_name As String, ByRef ldoc As bc_om_document, ByRef server_side_event As Boolean, Optional ByRef suppress As Boolean = False, Optional ByVal open_doc As Boolean = True, Optional ByRef history As String = "")
        Dim slog = New bc_cs_activity_log("bc_am_workflow_events_handler", "run_system_event", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim ocommentary As bc_cs_activity_log
        Dim i As Integer
        Try
            REM default to client side event
            server_side_event = False
            Dim obc_refresh As bc_am_at_component_refresh
            override_stage = 0
            ocommentary = New bc_cs_activity_log("bc_am_workflow_events_handler", "run_system_event", bc_cs_activity_codes.COMMENTARY, "Attempting to run action:" + action_name)
            run_system_event = False
            If action_name = "Componetiser" Then
                If IsNothing(obj) Then
                    If check_for_open_doc(open_doc, obj) = False Then
                        history = "Event: " + action_name + " surpressed as cant operate on mime type: " + ldoc.extension
                        run_system_event = True
                        Exit Function
                    End If
                Else
                    ldoc.docobject = obj
                End If
                REM instanstatiate componitozer
                Dim obc_componetizer As New bc_am_at_componentize
                run_system_event = obc_componetizer.Componentise(False, obj, "word", ldoc.id)
            ElseIf action_name = "Copy Doc With Refresh" Or action_name = "Translate" Then
                If action_name = "Translate" Then
                    If ldoc.sub_translated_docs <> "" Then
                        history = "Document already translated"
                        run_system_event = True
                        Exit Function
                    End If
                    If ldoc.translated_from_doc <> "0" Then
                        history = "Document is already  copy  cant copy"
                        run_system_event = True
                        Exit Function
                    End If
                End If
                If IsNothing(obj) Then
                    If check_for_open_doc(open_doc, obj) = False Then
                        history = "Event: " + action_name + " surpressed as cant operate on mime type: " + ldoc.extension
                        run_system_event = True
                        Exit Function
                    End If
                Else
                    ldoc.docobject = obj
                End If
                Dim ocopy As New bc_am_copy_doc(ldoc.id, obj, True, True)
                ocopy.copy(ldoc)
                REM if translate event then flag this
                If action_name = "Translate" Then
                    ldoc.translate_flag = True
                End If
                run_system_event = ocopy.success
            ElseIf action_name = "Copy Doc Without Refresh" Then
                If IsNothing(obj) Then
                    If check_for_open_doc(open_doc, obj) = False Then
                        history = "Event: " + action_name + " surpressed as cant operate on mime type: " + ldoc.extension
                        run_system_event = True
                        Exit Function
                    End If
                Else
                    ldoc.docobject = obj
                End If
                Dim ocopy As New bc_am_copy_doc(ldoc.id, obj, True, False)
                ocopy.copy(ldoc)
                run_system_event = ocopy.success
            ElseIf action_name = "Upload Translations" Then
                REM this event only run on a copy document
                If ldoc.translated_from_doc = 0 Then
                    history = "Upload Translations surpressed not a copy document"
                    run_system_event = True
                Else
                    Dim tdoc As New bc_om_document
                    tdoc.translated_from_doc = ldoc.translated_from_doc
                    tdoc.id = ldoc.id
                    tdoc.language_id = ldoc.language_id
                    tdoc.entity_id = ldoc.entity_id
                    tdoc.receive_translation = True
                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        tdoc.db_read()
                    Else
                        tdoc.tmode = bc_om_soap_base_class.tREAD
                        tdoc.transmit_to_server_and_receive(tdoc, True)
                    End If

                    If tdoc.uploaded_translated_components = True Then
                        REM now open document and refresh
                        If IsNothing(obj) Then
                            If check_for_open_doc(open_doc, obj) = False Then
                                history = "Event: " + action_name + " surpressed as cant operate on mime type: " + ldoc.extension
                                ldoc.docobject = obj
                                run_system_event = True
                                Exit Function
                            End If
                        Else
                            ldoc.docobject = obj
                        End If
                        ldoc.write_xml_to_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ".xml")
                        obc_refresh = New bc_am_at_component_refresh(False, obj, "word", 1, False)
                        ldoc = ldoc.read_xml_from_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ".xml")
                        run_system_event = True
                    Else
                        history = "Translation file not yet received Document will be held at current stage"
                        Me.stage_id_to = ldoc.original_stage
                        Me.stage_name_to = ldoc.original_stage_name
                        ldoc.stage = ldoc.original_stage
                        ldoc.stage_name = ldoc.original_stage_name
                        suppress = True
                        run_system_event = True
                        Dim omessage As New bc_cs_message("Blue Curve", history, bc_cs_message.MESSAGE)
                    End If
                End If
            ElseIf action_name = "Insert Disclosures" Then
                If IsNothing(obj) Then
                    If check_for_open_doc(open_doc, obj) = False Then
                        history = "Event: " + action_name + " surpressed as cant operate on mime type: " + ldoc.extension
                        ldoc.docobject = obj
                        run_system_event = True
                        Exit Function
                    End If
                Else
                    ldoc.docobject = obj
                End If
                ldoc.write_xml_to_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ".xml")
                obc_refresh = New bc_am_at_component_refresh(False, obj, "word", 2, False)
                ldoc = ldoc.read_xml_from_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ".xml")
                run_system_event = obc_refresh.success
            ElseIf action_name = "Refresh Document" Then
                If IsNothing(obj) Then
                    If check_for_open_doc(open_doc, obj) = False Then
                        history = "Event: " + action_name + " surpressed as cant operate on mime type: " + ldoc.extension
                        ldoc.docobject = obj
                        run_system_event = True
                        Exit Function
                    End If
                Else
                    ldoc.docobject = obj
                End If
                ldoc.write_xml_to_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ".xml")
                obc_refresh = New bc_am_at_component_refresh(False, obj, "word", 1, False)
                ldoc = ldoc.read_xml_from_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ".xml")
                run_system_event = True
            ElseIf action_name = "Render" Or action_name = "Render and Publish" Then
                If IsNothing(obj) Then
                    If check_for_open_doc(open_doc, obj) = False Then
                        history = "Event: " + action_name + " surpressed as cant operate on mime type: " + ldoc.extension
                        run_system_event = True
                        Exit Function
                    End If
                Else
                    ldoc.docobject = obj
                End If
                Dim obc_render As New bc_am_render_to_pdf(False, obj, "word", "Adobe PDF", ldoc.extension, 30, CStr(ldoc.id))
                run_system_event = obc_render.render(ldoc)
                run_system_event = obc_render.success
                If action_name = "Render and Publish" Then
                    ldoc.publish_flag = True
                End If
            ElseIf action_name = "Archive Units" Then
                Dim obc_archive As New bc_am_archive_units(CStr(ldoc.id))
                run_system_event = obc_archive.run_archive
            ElseIf action_name = "Distribution List" Then
                Dim obc_distribution As New bc_am_distribution(CStr(ldoc.id), 1)
                run_system_event = obc_distribution.run
            ElseIf action_name = "Distribute" Then
                Dim obc_distribution As New bc_am_distribution(CStr(ldoc.id), 2)
                run_system_event = obc_distribution.run
            ElseIf action_name = "Revert from PDF" Then
                Dim obc_distribution As New bc_am_pdf_revert(CStr(ldoc.id))
                run_system_event = obc_distribution.run
            ElseIf action_name = "Check Distribution List" Then
                Dim obc_dist_check As New bc_am_check_dist_list(CStr(ldoc.id))
                Me.suppress_no_trans = True
                run_system_event = obc_dist_check.check
            ElseIf action_name = "Publish Document" Then
                Dim obc_publish_document As New bc_am_publish_document(CStr(ldoc.id), True)
                run_system_event = obc_publish_document.run
            ElseIf action_name = "Check Duplicate" Then
                Dim obc_check_duplicate As New bc_am_check_duplicate(CStr(ldoc.pub_type_id), CStr(ldoc.entity_id), CStr(ldoc.id))
                If obc_check_duplicate.check_duplicate = True Then
                    Me.success = True
                    suppress = True
                    Dim omessage As New bc_cs_message("Blue Curve", obc_check_duplicate.out_text + " Document wont be allowed to be release from current stage", bc_cs_message.MESSAGE)
                    ldoc.history.Add(obc_check_duplicate.out_text + " Document wont be allowed to be released from current stage")
                    ldoc.stage = ldoc.original_stage
                    ldoc.stage_name = ldoc.original_stage_name

                    Me.stage_id_to = ldoc.original_stage
                    Me.stage_name_to = ldoc.original_stage_name
                End If
                run_system_event = True
            ElseIf action_name = "Unpublish Document" Then
                Dim obc_publish_document As New bc_am_publish_document(CStr(ldoc.id), False)
                run_system_event = obc_publish_document.run
            ElseIf action_name = "Publish Data" Then
                Dim obc_publish_data As New bc_am_publish_data(CStr(ldoc.id), ldoc.entity_id)
                run_system_event = obc_publish_data.run
            ElseIf action_name = "Email Notify" Then
                Dim obc_email_notify As New bc_am_email_notify(CStr(ldoc.id), ldoc.stage, False, False)
                run_system_event = obc_email_notify.run
            ElseIf action_name = "Email Notify Author Only" Then
                Dim obc_email_notify As New bc_am_email_notify(CStr(ldoc.id), ldoc.stage, True, False)
                run_system_event = obc_email_notify.run
            ElseIf action_name = "Email Notify Server Side" Then
                Dim obc_email_notify As New bc_am_email_notify(CStr(ldoc.id), ldoc.stage, False, True)
                run_system_event = obc_email_notify.run
                REM server side event now
                Dim osse As New bc_om_document.bc_om_server_side_event
                osse.sql = obc_email_notify.sql
                osse.name = action_name
                ldoc.server_side_events.events.Add(osse)
                server_side_event = True
            ElseIf action_name = "Email Notify Author Only Server Side" Then
                Dim obc_email_notify As New bc_am_email_notify(CStr(ldoc.id), ldoc.stage, True, True)
                run_system_event = obc_email_notify.run
                Dim osse As New bc_om_document.bc_om_server_side_event
                osse.sql = obc_email_notify.sql
                osse.name = action_name

                ldoc.server_side_events.events.Add(osse)
                server_side_event = True
            ElseIf action_name = "First Call Note" Then
                Dim obc_first_call_note As New bc_am_first_call_note(CStr(ldoc.id), ldoc.stage)
                run_system_event = obc_first_call_note.run
            ElseIf action_name = "QA" Then
                ldoc.qa = True
                run_system_event = True
            ElseIf action_name = "Refresh Document" Then
                If IsNothing(obj) Then
                    If check_for_open_doc(open_doc, obj) = False Then
                        history = "Event: " + action_name + " surpressed as cant operate on mime type: " + ldoc.extension
                        run_system_event = True
                        Exit Function
                    End If
                Else
                    ldoc.docobject = obj
                End If
                obc_refresh = New bc_am_at_component_refresh(False, obj, "word", 1, False)
                run_system_event = True
                bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve - create", "Workflow Events", 12, False, True)
            ElseIf action_name = "Conflict Check" Or action_name = "Conflict Check With Email" Then
                Dim oconflict As bc_am_conflict_check

                If action_name = "Conflict Check With Email" Then
                    oconflict = New bc_am_conflict_check(ldoc.id, True)
                Else
                    oconflict = New bc_am_conflict_check(ldoc.id, False)
                End If
                If oconflict.check_for_conflicts = False Then
                    REM event itself has failed so indicate not to change stage
                    run_system_event = False
                Else
                    run_system_event = True
                    REM copy history over
                    If oconflict.has_conflicts = True Then
                        REM if conflict override stage move to conflict stage
                        ldoc.stage = oconflict.conflict_stage_id
                        Me.stage_id_to = oconflict.conflict_stage_id
                        Me.stage_name_to = oconflict.conflict_stage_name
                        ldoc.history.Add("Document has Conflicts so will move to stage:" + oconflict.conflict_stage_name)
                        suppress = True
                        REM email notity bit
                        Dim omessage As New bc_cs_message("Blue Curve", "Document has conflicts with be in stage: " + oconflict.conflict_stage_name, bc_cs_message.MESSAGE)
                    Else
                        ldoc.history.Add("Document has no Conflicts")
                    End If
                End If

            ElseIf action_name = "Check Under Embargo" Then
                Dim obc_embargo As New bc_am_embargo_check(ldoc.id, ldoc.stage)
                run_system_event = obc_embargo.check
                If run_system_event = True Then
                    If obc_embargo.stage_out = 0 Then
                        run_system_event = False
                    Else
                        Dim res As String
                        res = obc_embargo.result
                        ldoc.history.Add(res)
                        If obc_embargo.stage_out <> 0 Then
                            REM set stage of document depening on embargo or not
                            ldoc.stage = obc_embargo.stage_out
                            override_stage = obc_embargo.stage_out
                            ldoc.stage_name = obc_embargo.stage_out_name
                            Me.stage_id_to = obc_embargo.stage_out
                            Me.stage_name_to = obc_embargo.stage_out_name
                            If obc_embargo.res = "Fail" Then
                                Dim omessage As New bc_cs_message("Blue Curve", "Document has companies under embargo will be in stage: " + obc_embargo.stage_out_name, bc_cs_message.MESSAGE)
                            End If
                        End If
                    End If
                End If
            ElseIf action_name = "Check Restrictions" Then
                Dim obc_res As New bc_am_check_restrictions(ldoc.id, ldoc.stage, ldoc.entity_id)
                run_system_event = obc_res.check
                If run_system_event = True Then
                    If obc_res.mode = 0 Then
                        REM do nothing
                        history = "No Restrictions"
                        run_system_event = True
                    Else
                        Dim res As String
                        res = obc_res.result
                        If obc_res.mode = 1 Then
                            REM set stage of document depening on embargo or not

                            Dim k As Boolean
                            k = False
                            If k = True Then
                                REM now refresh document
                                If IsNothing(obj) Then
                                    If check_for_open_doc(open_doc, obj) = False Then
                                        history = "Event: " + action_name + " surpressed as cant operate on mime type: " + ldoc.extension
                                        ldoc.docobject = obj
                                        run_system_event = True
                                        Exit Function
                                    End If
                                Else
                                    ldoc.docobject = obj
                                End If
                            End If
                            REM if stage not zero change route
                            If obc_res.stage_out > 0 Then
                                Dim omessage As New bc_cs_message("Blue Curve", "Document has Flag(s) will be moved to  stage: " + obc_res.stage_out_name, bc_cs_message.MESSAGE)
                                suppress = True
                                ldoc.stage = obc_res.stage_out
                                ldoc.stage_name = obc_res.stage_out_name
                                Me.stage_id_to = obc_res.stage_out
                                Me.stage_name_to = obc_res.stage_out_name
                                ldoc.history.Add("Restriction(s): " + obc_res.res_tx)
                                ldoc.history.Add("Document has Restrictions will be moved to  stage: " + obc_res.stage_out_name)
                            End If
                            ldoc.write_xml_to_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ".xml")
                            If k = True Then
                                obc_refresh = New bc_am_at_component_refresh(False, obj, "word", 1, False)
                                Dim j As Integer
                                For j = 0 To ldoc.refresh_components.refresh_components.Count - 1
                                    If ldoc.refresh_components.refresh_components(j).refresh_type = 2 Then
                                        ldoc.refresh_components.refresh_components(j).disabled = False
                                        ldoc.refresh_components.refresh_components(j).no_refresh = False
                                    End If
                                Next
                                ldoc.write_xml_to_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ".xml")
                                For i = 0 To ldoc.refresh_components.refresh_components.Count - 1
                                    If ldoc.refresh_components.refresh_components(i).refresh_type = 2 Then
                                        If CDate(ldoc.refresh_components.refresh_components(i).last_refresh_date) <= Now And ldoc.refresh_components.refresh_components(i).last_refresh_date <> "9-9-9999" Then
                                            ocommentary = New bc_cs_activity_log("bc_am_copy_doc", "create_new_doc", bc_cs_activity_codes.COMMENTARY, "Refreshing Disclosures in translated document")
                                            bc_cs_central_settings.progress_bar.increment("Refreshing Disclosures")
                                            obc_refresh = New bc_am_at_component_refresh(ldoc.docobject, "word")
                                            obc_refresh.refresh(False, 2, False, ldoc)
                                        Else
                                            ocommentary = New bc_cs_activity_log("bc_am_copy_doc", "create_new_doc", bc_cs_activity_codes.COMMENTARY, "Disclosures not refreshed in original")
                                        End If
                                        Exit For
                                    End If
                                Next
                                ldoc = ldoc.read_xml_from_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ".xml")
                            End If
                            ldoc.write_xml_to_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ".xml")
                            run_system_event = True
                        End If
                    End If
                End If
            ElseIf action_name = "US Flag" Then
                Dim obc_us As New bc_am_us_flag(ldoc.entity_id)
                run_system_event = obc_us.check
                If run_system_event = True Then
                    If obc_us.flag = 0 Then
                        REM do nothing
                        history = "No US flag"
                        run_system_event = True
                    Else
                        suppress = True
                        history = "US Flag US copy created"
                        Dim omessage As New bc_cs_message("Blue Curve", "Document has US Flag will be moved to  stage: " + obc_us.stage_name_to + " and US copy created", bc_cs_message.MESSAGE)
                        suppress = True

                        ldoc.history.Add("Document has US Flag will be moved to  stage: " + obc_us.stage_name_to)
                        REM open document
                        If IsNothing(obj) Then
                            If check_for_open_doc(open_doc, obj) = False Then
                                history = "Event: " + action_name + " surpressed as cant operate on mime type: " + ldoc.extension
                                ldoc.docobject = obj
                                run_system_event = True
                                Exit Function
                            End If
                        Else
                            ldoc.docobject = obj
                        End If
                        Dim ocopy As New bc_am_copy_doc(ldoc.id, obj, True, True)
                        ocopy.create_new_doc(obc_us.pub_type_new, ldoc.language_id, obj, ldoc, True, obc_us.stage_id_to, False)
                        ldoc.stage = obc_us.stage_id_to
                        ldoc.stage_name = obc_us.stage_name_to
                        Me.stage_id_to = obc_us.stage_id_to
                        Me.stage_name_to = obc_us.stage_name_to
                        If ocopy.success = True Then
                            run_system_event = True
                        End If
                        suppress = True
                    End If

                End If

            Else
                ocommentary = New bc_cs_activity_log("bc_am_workflow_events_handler", "run_event", bc_cs_activity_codes.COMMENTARY, "System Event " + action_name + " not supported.")
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_workflow_events_handler", "run_system_event", bc_cs_error_codes.USER_DEFINED, ex.Message)
            run_system_event = False
        Finally
            slog = New bc_cs_activity_log("bc_am_workflow_events_handler", "run_system_event", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
    Public Sub close()
        odoc.close()
    End Sub
    Private Function check_for_open_doc(ByVal open_doc As Boolean, ByRef outdoc As Object) As Boolean
        Dim slog = New bc_cs_activity_log("check_for_open_doc", "open_doc", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            Dim ocommentary As bc_cs_activity_log

            check_for_open_doc = True
            REM check mime type can support open doc
            check_for_open_doc = True
            Dim omime As New bc_am_mime_types
            If omime.can_edit(ldoc.extension) = False Then
                check_for_open_doc = False
                Exit Function
            End If

            REM if document is not open  
            REM open it as events requires it
            If Me.doc_open = False And open_doc = False Then

                ocommentary = New bc_cs_activity_log("bc_am_workflow_events_handler", "run_system_event", bc_cs_activity_codes.COMMENTARY, "Event requires an Open docuent")
                Dim obc_workflow As New bc_am_workflow
                ldoc.history.Clear()
                obc_workflow.set_server_status("Checking out document from server")
                REM update metadata
                ldoc.connection_method = bc_cs_central_settings.selected_conn_method
                REM ldoc.write_xml_to_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ldoc.extension)
                odoc.open(False, ldoc, False)
                ldoc.history.Clear()
                ldoc.support_documents.document.Clear()
                Me.obj = ldoc.docobject
                Me.doc_open = True
                REM reset stage transition
                ldoc.stage = Me.stage_id_to
                ldoc.stage_name = Me.stage_name_to
                ldoc.write_xml_to_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ".xml")
                outdoc = ldoc
                outdoc = ldoc.docobject

            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("check_for_open_doc", "open_doc", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("check_for_open_doc", "open_doc", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try


    End Function
    Private Sub check_for_doc_close(ByVal open_doc As Boolean)
        REM if document is open and was not original open
        REM close it
        If Me.doc_open = True And open_doc = False Then

        End If


    End Sub

End Class
Public Class bc_am_render_to_pdf
    Public doc As Object
    Private doc_id As String
    Private timeout As Integer
    Private printer As String
    Private extension As String
    Public success As Boolean
    Private fn As String
    Private ofn As String
    Private wait As Boolean = True
    Private finish As Boolean = False
    Private open As Boolean = True
    Private odoc As bc_om_document
    Dim WithEvents fsw As New FileSystemWatcher
    REM constructor if document is not yet open
    Public Sub New(ByVal show_form As Boolean, ByVal doc As Object, ByVal application As String, ByVal printer As String, ByVal extension As String, ByVal timeout As Integer, ByVal doc_id As String)
        Me.doc = doc
        Me.doc_id = doc_id
        Me.printer = printer
        Me.extension = extension
        Me.timeout = timeout
        REM flag to determine whether to register pdf in system out of submission process
        Me.open = True
        success = False
    End Sub
    REM constructor if document is already open
    Public Sub New(ByVal doc_id As String, ByVal printer As String, ByVal timeout As Integer, ByVal ldoc As bc_om_document)
        Me.printer = printer
        Me.extension = extension
        Me.timeout = timeout
        Me.doc_id = doc_id
        REM flag to determine whether to register pdf in system out of submission process
        Me.open = False
        success = False
    End Sub
    Public Function render(ByVal ldoc As bc_om_document) As Boolean
        Dim slog = New bc_cs_activity_log("bc_am_render_to_pdf", "render", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim ocommentary As bc_cs_activity_log
        Dim default_printer As String = ""

        Dim i As Integer

        Try
            If IsNothing(Me.doc) Then
                Exit Function
            End If

            Me.success = False
            If open = False Then
                bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO
                ocommentary = New bc_cs_activity_log("bc_am_render_to_pdf", "render", bc_cs_activity_codes.COMMENTARY, "Attempting to Open Document prior to Render")
                REM recreate document object
                ldoc.id = Me.doc_id
                Dim oopendoc As New bc_am_at_open_doc
                oopendoc.open(False, ldoc, False)
                REM assign active document across
                Me.doc = ldoc.docobject
            End If

            Dim lfn As String
            Dim dfn As String = ""

            Try
                default_printer = doc.application.activeprinter
                doc.application.activeprinter = Me.printer
            Catch
                success = False
                Exit Function
            End Try

            If Right(ldoc.filename, 4) = Right(ldoc.extension, 4) Then
                fn = Left(ldoc.filename, Len(ldoc.filename) - 4) + ".pdf"
            Else
                fn = ldoc.filename + ".pdf"
            End If
            lfn = Left(fn, Len(fn) - 4) + ldoc.extension
            Dim origfn As String
            Dim use_temp_file As String
            use_temp_file = False
            origfn = fn
            fn = Left(fn, Len(fn) - 4)
            If InStr(fn, ".") > 0 Then
                use_temp_file = True
                fn = Replace(fn, ".", "", 1)
                ldoc.docobject.saveas(bc_cs_central_settings.local_repos_path + fn + ldoc.extension)
                dfn = bc_cs_central_settings.local_repos_path + fn + ldoc.extension
            End If

            REM file will end up in aplication data area so wait for it to arrive then copy
            REM to local repos
            dothetask()

            ofn = bc_cs_central_settings.local_repos_path + origfn
            REM if filename has . in it remove temporaily


            i = 0
            REM see if file exists
            fn = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + "\" + fn + ".pdf"

            While i < 30 And Me.success = False
                Thread.Sleep(1000)
                REM check every second for up 30 seconds
                REM for file
                Dim fs As New bc_cs_file_transfer_services

                If fs.check_document_exists(fn) = True Then
                    REM copy over
                    fs.file_copy(fn, ofn)
                    REM remove
                    fs.delete_file(fn)
                    Me.success = True
                End If
                i = i + 1
            End While
            REM save original back
            If use_temp_file = True Then
                ldoc.docobject.saveas(bc_cs_central_settings.local_repos_path + lfn)
                Dim fs As New bc_cs_file_transfer_services
                fs.delete_file(dfn)
            End If
            If Me.success = False Then
                ocommentary = New bc_cs_activity_log("bc_am_render_to_pdf", "render", bc_cs_activity_codes.COMMENTARY, "Render Timed out.")
                Dim omessage As New bc_cs_message("Blue Curve - create", "Render timed out.", bc_cs_message.MESSAGE)
            End If
            If open = False Then
                doc.application.activeprinter = default_printer
                ocommentary = New bc_cs_activity_log("bc_am_render_to_pdf", "render", bc_cs_activity_codes.COMMENTARY, "Attempting to Submit document post Render")
                Dim osubmit As New bc_am_at_submit(False, doc, "word", False)
            End If
        Catch ex As Exception
                ocommentary = New bc_cs_activity_log("bc_am_render_to_pdf", "render", bc_cs_activity_codes.COMMENTARY, ex.Message)
                Dim db_err As New bc_cs_error_log("bc_am_render_to_pdf", "render", "run_system_event", bc_cs_error_codes.USER_DEFINED, ex.Message)
            Finally
                If open = True Then
                    doc.application.activeprinter = default_printer
                End If
                slog = New bc_cs_activity_log("bc_am_render_to_pdf", "render", bc_cs_activity_codes.TRACE_EXIT, "")
            End Try

    End Function
    Public Sub dothetask()
        Dim slog = New bc_cs_activity_log("bc_am_render_to_pdf", "dothetask", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            doc.Application.PrintOut(FileName:="", Range:=0, Item:=0, _
            Copies:=1, Pages:="", PageType:=0, _
            ManualDuplexPrint:=False, Collate:=True, Background:=False, PrintToFile:= _
            False, PrintZoomColumn:=0, PrintZoomRow:=0, PrintZoomPaperWidth:=0, _
            PrintZoomPaperHeight:=0)

            REM if not
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_render_to_pdf", "dothetask", "run_system_event", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_render_to_pdf", "dothetask", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try


    End Sub
End Class
REM archive units event
Public Class bc_am_archive_units
    Private doc_id As String
    Public Sub New(ByVal doc_id As String)
        Me.doc_id = doc_id
    End Sub
    Public Function run_archive() As Boolean
        Dim slog = New bc_cs_activity_log("bc_am_archive_units", "run_archive", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim osql As New bc_om_sql("exec dbo.bcc_core_wf_archive_composite '" + CStr(doc_id) + "'")
            run_archive = False
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                osql.tmode = bc_om_soap_base_class.tREAD
                osql.transmit_to_server_and_receive(osql, True)
            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                osql.db_read()
            End If
            If osql.success = True Then
                run_archive = True
            Else
                Dim ocommentary As New bc_cs_activity_log("bc_am_archive_units", "run_archive", bc_cs_activity_codes.COMMENTARY, "SQL Error: exec dbo.bcc_core_wf_archive_composite '" + CStr(doc_id) + "'")
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_archive_units", "run_archive", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_archive_units", "run_archive", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function

End Class
REM copy doc for translation
Public Class bc_am_copy_doc
    Public dest_pub_type_id As Long
    Public success As Boolean
    Public show_message As Boolean
    'Private ldoc As bc_om_document
    Private doc_id As String
    Private ao_object As Object
    Private close_doc As Boolean
    Private refresh As Boolean

    Public Sub New(ByVal doc_id As String, ByVal ao_object As Object, Optional ByVal close_doc As Boolean = False, Optional ByVal refresh As Boolean = True, Optional ByVal show_message As Boolean = False)
        Me.doc_id = doc_id
        Me.ao_object = ao_object
        Me.close_doc = close_doc
        Me.refresh = refresh
        Me.show_message = show_message
    End Sub
    Public Function copy(ByVal ldoc As bc_om_document) As Boolean
        Dim slog = New bc_cs_activity_log("bc_am_copy_doc", "copy", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim fs As New bc_cs_file_transfer_services

            Dim i, j, k As Integer
            Dim language As String
            Dim newdoc As New bc_om_document
            Dim omessage As bc_cs_message
            Dim ocommentary As bc_cs_activity_log
            success = True

            For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                If bc_am_load_objects.obc_pub_types.pubtype(i).id = ldoc.pub_type_id Then
                    If bc_am_load_objects.obc_pub_types.pubtype(i).translated_pub_types.count = 0 Then
                        ocommentary = New bc_cs_activity_log("bc_am_copy_doc", "copy", bc_cs_activity_codes.TRACE_ENTRY, "No copy pub types for this pub type:" + CStr(ldoc.pub_type_id))
                        ldoc.history.Add("No copy pub types for this pub type:" + CStr(ldoc.pub_type_id))
                        If Me.show_message = True Then
                            omessage = New bc_cs_message("Blue curve - Create", "No copy pub types for this pub type", bc_cs_message.MESSAGE)
                        End If
                        Exit Function
                    End If
                End If
            Next
            If ldoc.master_translated_doc <> "" Then
                If Me.show_message = True Then
                    omessage = New bc_cs_message("Blue Curve - create", "Document is already copied from document: " + ldoc.master_translated_doc + " can't translate", bc_cs_message.MESSAGE)
                End If
                ocommentary = New bc_cs_activity_log("bc_am_copy_doc", "copy", bc_cs_activity_codes.TRACE_ENTRY, "Document is already copied from document: " + ldoc.master_translated_doc + " can't translate")
                ldoc.history.Add("Document is already copied from document: " + ldoc.master_translated_doc + " cannot translate but will just refresh")
                bc_cs_central_settings.progress_bar.increment("Refreshing Copied Document")
                Dim orefresh As New bc_am_at_component_refresh(ao_object, "word")
                orefresh.refresh(False, 1, False, ldoc)
                Exit Function
            End If
            If ldoc.sub_translated_docs <> "" Then
                If Me.show_message = True Then
                    omessage = New bc_cs_message("Blue Curve - create", "Document already has  copy(s): " + ldoc.sub_translated_docs + " can't copy again", bc_cs_message.MESSAGE)
                End If
                ocommentary = New bc_cs_activity_log("bc_am_copy_doc", "copy", bc_cs_activity_codes.TRACE_ENTRY, "Document already has  copy(s): " + ldoc.sub_translated_docs + " can't copy again")
                ldoc.history.Add("Document already has  copy(s): " + ldoc.sub_translated_docs + " can't copy again")
                Exit Function
            End If
            If ldoc.translated_from_doc <> "0" Then
                If Me.show_message = True Then
                    omessage = New bc_cs_message("Blue Curve - create", "Document is already  copy  cant copy", bc_cs_message.MESSAGE)
                End If
                ocommentary = New bc_cs_activity_log("bc_am_copy_doc", "copy", bc_cs_activity_codes.TRACE_ENTRY, "Document is already  copy  can't copy")
                ldoc.translate_flag = False
                ldoc.history.Add("Document is already  copy  can't copy")

                Exit Function
            End If

            Dim found As Boolean = False
            copy = True
            For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                If bc_am_load_objects.obc_pub_types.pubtype(i).id = ldoc.pub_type_id Then

                    REM else loop for each tranlation variant
                    For j = 0 To bc_am_load_objects.obc_pub_types.pubtype(i).translated_pub_types.count - 1
                        For k = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                            If bc_am_load_objects.obc_pub_types.pubtype(k).id = bc_am_load_objects.obc_pub_types.pubtype(i).translated_pub_types(j) Then
                                language = bc_am_load_objects.obc_pub_types.pubtype(k).language
                                ocommentary = New bc_cs_activity_log("bc_am_copy_doc", "copy", bc_cs_activity_codes.COMMENTARY, "Translating doc: " + ldoc.title + " to pub type id: " + CStr(bc_am_load_objects.obc_pub_types.pubtype(i).translated_pub_types(j)) + " language: " + CStr(language))
                                create_new_doc(CStr(bc_am_load_objects.obc_pub_types.pubtype(i).translated_pub_types(j)), language, ao_object, ldoc, False, ldoc.stage, True)
                                found = True
                                Exit For
                            End If
                        Next
                    Next
                    Exit For
                End If
            Next
            If found = False Then
                ocommentary = New bc_cs_activity_log("bc_am_copy_doc", "copy", bc_cs_activity_codes.COMMENTARY, "Could not find translated pub type id." + ldoc.title + " to pub type id.")
            End If
        Catch ex As Exception
            success = False
            copy = False
            Dim db_err As New bc_cs_error_log("bc_am_copy_doc", "copy", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_copy_doc", "copy", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
    Public Function create_new_doc(ByVal pub_type_id As String, ByVal language As Long, ByVal ao_object As Object, ByVal ldoc As bc_om_document, ByVal no_workflow As Boolean, ByVal stage_id_to As Long, ByVal link_doc As Boolean) As Boolean
        Dim slog = New bc_cs_activity_log("bc_am_copy_doc", "create_new_doc", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim oword As bc_ao_word
        Dim original_filename As String

        'Dim original_doc_id As Long

        Try
            Dim i As Integer
            Dim ocommentary As bc_cs_activity_log
            Dim ptypename As String = ""
            
            REM take original copies
            'original_doc_id = ldoc.id
            If Right(ldoc.filename, 4) = ldoc.extension Then
                original_filename = ldoc.filename
                'original_metadata = Left(original_filename, Len(original_filename) - 4) + ".xml"
            Else
                original_filename = ldoc.filename + ldoc.extension
                'original_metadata = ldoc.filename + ".xml"
            End If
            REM change current doc to reflect new doc
            Dim ndoc As New bc_om_document
            ndoc.id = 0
            ndoc.pub_type_id = pub_type_id
            ndoc.language_id = language
            ndoc.action_Ids = ldoc.action_Ids
            ndoc.authors = ldoc.authors
            ndoc.bus_area = ldoc.bus_area
            ndoc.connection_method = ldoc.connection_method
            ndoc.disclosures = ldoc.disclosures
            ndoc.doc_date = ldoc.doc_date
            ndoc.entity_id = ldoc.entity_id
            ndoc.extension = ldoc.extension
            ndoc.logged_on_user_name = ldoc.logged_on_user_name
            ndoc.logged_on_user_password = ldoc.logged_on_user_password
            ndoc.master_flag = True
            ndoc.original_stage = ldoc.original_stage
            ndoc.original_stage_name = ldoc.original_stage_name
            ndoc.originating_author = ldoc.originating_author
            ndoc.pages = ldoc.pages
            ndoc.pub_type_name = ldoc.pub_type_name
            ndoc.refresh_components = ldoc.refresh_components
            ndoc.translate_flag = False
            For i = 0 To ndoc.refresh_components.refresh_components.Count - 1
                ndoc.refresh_components.refresh_components(i).disabled = 0
            Next
            ndoc.stage = ldoc.stage
            ndoc.stage_name = ldoc.stage_name
            ndoc.sub_title = ldoc.sub_title
            ndoc.summary = ldoc.summary
            ndoc.taxonomy = ldoc.taxonomy
            ndoc.teaser_text = ldoc.teaser_text
            ndoc.workflow_stages = ldoc.workflow_stages



            For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                If bc_am_load_objects.obc_pub_types.pubtype(i).id = pub_type_id Then
                    ptypename = bc_am_load_objects.obc_pub_types.pubtype(i).name
                    Exit For
                End If
            Next
            For i = 0 To bc_am_load_objects.obc_pub_types.languages.Count - 1
                If bc_am_load_objects.obc_pub_types.languages(i).id = language Then
                    ndoc.title = ldoc.title + "-(copy):" + ptypename
                    Exit For
                End If
            Next
            REM register this as a new document
            ndoc.register_only = True
            ndoc.byteDoc = Nothing
            ndoc.translate_flag = False

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                ocommentary = New bc_cs_activity_log("bc_am_at_submit", "new", bc_cs_activity_codes.COMMENTARY, "Registering Document via ADO.")
                ndoc.db_write(Nothing)

            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                ocommentary = New bc_cs_activity_log("bc_am_at_submit", "new", bc_cs_activity_codes.COMMENTARY, "Registering Document via SOAP.")
                ndoc.tmode = bc_om_soap_base_class.tWRITE
                ndoc.no_send_back = False
                ndoc.transmit_to_server_and_receive(ndoc, True)

                REM now get document id back it will be max doc id for originator
                Dim osql As New bc_om_sql("select max(doc_id)from document_table where originator_id=" + CStr(ldoc.originating_author))

            End If
            ndoc.filename = CStr(ndoc.id) + ndoc.extension
            REM reset register flag
            ndoc.register_only = False

            REM save original document
            oword = New bc_ao_word(ao_object)
            oword.save()
            REM copy it over
            Dim fs As New bc_cs_file_transfer_services
            fs.file_copy(bc_cs_central_settings.local_repos_path + CStr(original_filename), bc_cs_central_settings.local_repos_path + CStr(ndoc.id) + ndoc.extension)
            REM attach this to master
            If link_doc = True Then
                ndoc.translated_from_doc = ldoc.id
            End If
            REM save down metadata file
            ndoc.write_xml_to_file(bc_cs_central_settings.local_repos_path + CStr(ndoc.id) + ".xml")
            Dim oopen As New bc_am_at_open_doc
            If Me.close_doc = False Then
                oopen.open(True, ndoc, True)
            Else
                oopen.open(True, ndoc, False)
            End If
            If Me.refresh = True Then
                REM treat as local and make visible
                bc_cs_central_settings.progress_bar.increment("Refreshing Copied Document")
                Dim orefresh As New bc_am_at_component_refresh(ndoc.docobject, "word")
                orefresh.refresh(False, 1, False, ndoc)
                Dim j As Integer

                For i = 0 To ndoc.refresh_components.refresh_components.Count - 1
                    If ndoc.refresh_components.refresh_components(i).refresh_type = 2 Then
                        If CDate(ndoc.refresh_components.refresh_components(i).last_refresh_date) <= Now And ndoc.refresh_components.refresh_components(i).last_refresh_date <> "9-9-9999" Then
                            ocommentary = New bc_cs_activity_log("bc_am_copy_doc", "create_new_doc", bc_cs_activity_codes.COMMENTARY, "Refreshing Disclosures in translated document")
                            bc_cs_central_settings.progress_bar.increment("Refreshing Disclosures")
                            ndoc.refresh_components.language_id = language


                            For j = 0 To ndoc.refresh_components.refresh_components.Count - 1
                                If ndoc.refresh_components.refresh_components(j).refresh_type = 2 Then
                                    ndoc.refresh_components.refresh_components(j).disabled = False
                                    ndoc.refresh_components.refresh_components(j).no_refresh = False
                                End If
                            Next
                            ndoc.write_xml_to_file(bc_cs_central_settings.local_repos_path + CStr(ndoc.id) + ".xml")
                            orefresh = New bc_am_at_component_refresh(ndoc.docobject, "word")
                            orefresh.refresh(False, 2, False, ndoc)
                        Else
                            ocommentary = New bc_cs_activity_log("bc_am_copy_doc", "create_new_doc", bc_cs_activity_codes.COMMENTARY, "Disclosures not refreshed in original")
                        End If
                        Exit For
                    End If

                Next
                REM after refresh formatter
                Try
                    ndoc.docobject.at_after_translate()
                Catch ex As Exception
                    ocommentary = New bc_cs_activity_log("bc_am_copy_doc", "create_new_doc", bc_cs_activity_codes.COMMENTARY, "Failed to find vba at_after_translate")
                End Try
            End If
            REM if close requested submit copy doc
            If Me.close_doc = True Then
                ndoc.history.Clear()
                ndoc.history.Add("Document Copied from: " + CStr(ldoc.id))
                ndoc.stage = stage_id_to
                ndoc.write_xml_to_file(bc_cs_central_settings.local_repos_path + CStr(ndoc.id) + ".xml")
                ndoc.docobject.save()
                REM save active document 
                Dim osubmit As New bc_am_at_submit(False, ndoc.docobject, "word", False, no_workflow, False, True)
            End If

        Catch ex As Exception
            success = False
            create_new_doc = False
            Dim db_err As New bc_cs_error_log("bc_am_copy_doc", "create_new_doc", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            REM switch back original
            slog = New bc_cs_activity_log("bc_am_copy_doc", "create_new_doc", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class
REM archive units event
Public Class bc_am_distribution
    Private doc_id As String
    Private mode As Integer
    Public Sub New(ByVal doc_id As String, ByVal mode As Integer)
        Me.doc_id = doc_id
        Me.mode = mode
    End Sub
    Public Function run() As Boolean
        Dim slog = New bc_cs_activity_log("bc_am_distribution", "run", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim osql As New bc_om_sql("exec dbo.bcc_core_wf_distribute '" + CStr(doc_id) + "','" + CStr(mode) + "'")

            run = False
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                osql.tmode = bc_om_soap_base_class.tREAD
                osql.transmit_to_server_and_receive(osql, True)

            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                osql.db_read()
            End If
            If osql.success = True Then
                run = True
            Else
                Dim ocommentary As New bc_cs_activity_log("bc_am_distribution", "run", bc_cs_activity_codes.COMMENTARY, "SQL Error: exec dbo.bcc_core_wf_distribute '" + CStr(doc_id) + "','" + CStr(mode) + "'")
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_distribution", "run", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_distribution", "run", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function

End Class
Public Class bc_am_pdf_revert
    Private doc_id As String
    Public Sub New(ByVal doc_id As String)
        Me.doc_id = doc_id
    End Sub
    Public Function run() As Boolean
        Dim slog = New bc_cs_activity_log("bc_am_pdf_revert", "run", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim osql As New bc_om_sql("exec dbo.bcc_core_wf_switch_to_master '" + CStr(doc_id) + "', 0")
            run = False
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                osql.tmode = bc_om_soap_base_class.tREAD
                osql.transmit_to_server_and_receive(osql, True)
            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                osql.db_read()
            End If
            If osql.success = True Then
                run = True
            Else
                Dim ocommentary As New bc_cs_activity_log("bc_am_distribution", "run", bc_cs_activity_codes.COMMENTARY, "SQL Error: exec dbo.bcc_core_wf_switch_to_master '" + CStr(doc_id) + "', 0" + "'")
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_pdf_revert", "run", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_pdf_revert", "run", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function

End Class

REM checks if document is an unpublished duplicate if so wont release it to next stage
Public Class bc_am_check_duplicate
    Public pub_type_id As String
    Public entity_id As String
    Public doc_id As String
    Public out_text As String
    Public Sub New(ByVal pub_type_id As String, ByVal entity_id As String, ByVal doc_id As String)
        Me.pub_type_id = pub_type_id
        Me.entity_id = entity_id
        Me.doc_id = doc_id
    End Sub
    Public Function check_duplicate() As Boolean
        Dim sql As String
        sql = "exec dbo.bcc_core_check_duplicate " + CStr(pub_type_id) + "," + CStr(entity_id) + "," + CStr(doc_id)
        Dim osql As New bc_om_sql(sql)

        check_duplicate = False
        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
            osql.tmode = bc_om_soap_base_class.tREAD
            osql.transmit_to_server_and_receive(osql, True)

        ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
            osql.db_read()
        End If
        If osql.success = True Then
            If IsArray(osql.results) Then
                If osql.results(0, 0) = 1 Then
                    Me.out_text = osql.results(1, 0)
                    check_duplicate = True
                End If
            End If
        End If
    End Function

End Class
REM conflict check
Public Class bc_am_conflict_check
    Public has_conflicts As Boolean = False
    Public conflict_stage_id As Long
    Public conflict_stage_name As String
    Public oconflicts As bc_om_conflict_check
    Public with_email As Boolean
    Private doc_id As String
    Public Sub New(ByVal doc_id As String, Optional ByVal with_email As Boolean = False)
        Me.doc_id = doc_id
        Me.with_email = with_email
    End Sub
    Public Function check_for_conflicts() As Boolean
        Dim slog = New bc_cs_activity_log("bc_am_conflict_check", "check_for_conflicts", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            oconflicts = New bc_om_conflict_check(Me.doc_id, Me.with_email)
            check_for_conflicts = True
            has_conflicts = False
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                oconflicts.tmode = bc_om_soap_base_class.tREAD
                oconflicts.transmit_to_server_and_receive(oconflicts, True)
            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                oconflicts.db_read()
            End If
            Me.has_conflicts = oconflicts.has_conflicts
            Me.conflict_stage_id = oconflicts.conflict_stage_id
            Me.conflict_stage_name = oconflicts.conflict_stage_name
            If oconflicts.err = True Then
                check_for_conflicts = False
            End If

        Catch ex As Exception
            REM conflict system has failed so event has failed
            check_for_conflicts = False
            Dim db_err As New bc_cs_error_log("bc_am_conflict_check", "check_for_conflicts", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_conflict_check", "check_for_conflicts", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function


End Class
REM archive units event
Public Class bc_am_publish_document
    Private doc_id As String
    Private publish As Boolean
    Public Sub New(ByVal doc_id As String, ByVal publish As Boolean)
        Me.doc_id = doc_id
        Me.publish = publish
    End Sub
    Public Function run() As Boolean
        Dim slog = New bc_cs_activity_log("bc_am_publish_document", "run_archive", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim osql As New bc_om_sql("exec dbo.bcc_core_wf_Publish_document '" + CStr(doc_id) + "','" + CStr(publish) + "'")
            run = False
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                osql.tmode = bc_om_soap_base_class.tREAD
                osql.transmit_to_server_and_receive(osql, True)

            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                osql.db_read()
            End If
            If osql.success = True Then
                run = True
            Else
                Dim ocommentary As New bc_cs_activity_log("bc_am_publish_document", "run_archive", bc_cs_activity_codes.COMMENTARY, "SQL Error: exec dbo.bcc_core_wf_publish_document '" + CStr(doc_id) + "','" + CStr(publish) + "'")
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_publish_document", "run", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_publish_document", "run", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function

End Class
Public Class bc_am_publish_data
    Private doc_id As String
    Private entity_id As Long
    Public Sub New(ByVal doc_id As String, ByVal entity_id As Long)
        Me.doc_id = doc_id
        Me.entity_id = entity_id
    End Sub
    Public Function run() As Boolean
        Dim slog = New bc_cs_activity_log("bc_am_publish_data", "run", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim osql As New bc_om_sql("exec dbo.bcc_core_wf_publish_data '" + CStr(doc_id) + "','" + CStr(entity_id) + "'")
            run = False
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                osql.tmode = bc_om_soap_base_class.tREAD
                osql.transmit_to_server_and_receive(osql, True)

            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                osql.db_read()
            End If
            If osql.success = True Then
                run = True
            Else
                Dim ocommentary As New bc_cs_activity_log("bc_am_publish_data", "run_archive", bc_cs_activity_codes.COMMENTARY, "SQL Error: exec dbo.bcc_core_wf_publish_data '" + CStr(doc_id) + "','" + CStr(entity_id) + "'")
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_publish_data", "run", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_publish_data", "run", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function

End Class
Public Class bc_am_embargo_check
    Private doc_id As String
    Private stage_to As Long
    Public stage_out As Long
    Public stage_out_name As String
    Public res As String
    Public Sub New(ByVal doc_id As String, ByVal stage_to As Long)
        Me.doc_id = doc_id
        Me.stage_to = stage_to
    End Sub
    Public Function check() As Boolean
        REM calls sp which will return  stage
        REM if embargo will return stage that matches stage to
        REM if not will return stage to promote to
        Dim slog = New bc_cs_activity_log("bc_am_embargo_check", "check", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim osql As New bc_om_sql("exec dbo.bcc_core_check_embargo '" + CStr(doc_id) + "'")

            check = True
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                osql.tmode = bc_om_soap_base_class.tREAD
                osql.transmit_to_server_and_receive(osql, True)

            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                osql.db_read()
            End If
            If osql.success = True Then
                If IsArray(osql.results) Then
                    If UBound(osql.results, 2) >= 0 Then
                        stage_out = CLng(osql.results(0, 0))
                        res = CStr(osql.results(1, 0))
                        stage_out_name = CStr(osql.results(2, 0))
                    End If
                End If
                check = True
            Else
                Dim ocommentary As New bc_cs_activity_log("bc_am_publish_data", "run_archive", bc_cs_activity_codes.COMMENTARY, "SQL Error: exec dbo.bcc_core_check_embargo '" + CStr(doc_id) + "'")
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_embargo_check", "check", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_embargo_check", "check", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
    Public Function result() As String
        If res = "Fail" Then
            result = "Document has companies under Embargo so will be in stage: " + stage_out_name
        Else
            result = "Document does not have embargo companies so auto promote to stage:" + stage_out_name
        End If
    End Function
End Class
Public Class bc_am_check_restrictions
    Private doc_id As String
    Private stage_to As Long
    Public stage_out As Long
    Public entity_id As Long
    Public mode As Long
    Public res_tx As String
    Public stage_out_name As String
    Public res As String
    Public Sub New(ByVal doc_id As String, ByVal stage_to As Long, ByVal entity_id As Long)
        Me.doc_id = doc_id
        Me.stage_to = stage_to
        Me.entity_id = entity_id
    End Sub
    Public Function check() As Boolean
        REM calls sp which will return  stage
        REM if embargo will return stage that matches stage to
        REM if not will return stage to promote to
        Dim slog = New bc_cs_activity_log("bc_am_check_restrictions", "check", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim osql As New bc_om_sql("exec bcc_core_res_workflow  '" + CStr(entity_id) + "','" + CStr(doc_id) + "'")

            check = True
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                osql.tmode = bc_om_soap_base_class.tREAD
                osql.transmit_to_server_and_receive(osql, True)

            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                osql.db_read()
            End If
            If osql.success = True Then
                If IsArray(osql.results) Then
                    If UBound(osql.results, 2) >= 0 Then
                        Me.mode = CLng(osql.results(0, 0))
                        Me.stage_out = CLng(osql.results(1, 0))
                        Me.stage_out_name = CStr(osql.results(2, 0))
                        Me.res_tx = CStr(osql.results(3, 0))
                    End If
                End If
                check = True
            Else
                Dim ocommentary As New bc_cs_activity_log("bc_am_publish_data", "run_archive", bc_cs_activity_codes.COMMENTARY, "SQL Error: exec bcc_core_res_workflow")
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_check_restrictions", "check", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_check_restrictions", "check", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
    Public Function result() As String
        If res = "Fail" Then
            result = "Document has companies under Embargo so will be in stage: " + stage_out_name
        Else
            result = "Document does not have embargo companies so auto promote to stage:" + stage_out_name
        End If
    End Function
End Class
Public Class bc_am_us_flag
    Public entity_id As Long
    Public flag As Long
    Public stage_id_to As Long
    Public stage_name_to As String
    Public pub_type_new As Long
    Public Sub New(ByVal entity_id As Long)
        Me.entity_id = entity_id
    End Sub
    Public Function check() As Boolean
        REM calls sp which will return  stage
        REM if embargo will return stage that matches stage to
        REM if not will return stage to promote to
        Dim slog = New bc_cs_activity_log("bc_am_us_flag", "check", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim osql As New bc_om_sql("exec bcc_core_us_flag  '" + CStr(entity_id) + "'")

            check = False
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                osql.tmode = bc_om_soap_base_class.tREAD
                osql.transmit_to_server_and_receive(osql, True)

            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                osql.db_read()
            End If
            If osql.success = True Then
                If IsArray(osql.results) Then
                    If UBound(osql.results, 2) >= 0 Then
                        Me.flag = CLng(osql.results(0, 0))
                        Me.stage_id_to = CLng(osql.results(1, 0))
                        Me.stage_name_to = CStr(osql.results(2, 0))
                        Me.pub_type_new = CStr(osql.results(3, 0))
                    End If
                End If
                check = True
            Else
                Dim ocommentary As New bc_cs_activity_log("bc_am_us_flag", "check", bc_cs_activity_codes.COMMENTARY, "SQL Error: exec bcc_core_us_flag  '" + CStr(entity_id) + "'")
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_check_restrictions", "us_flag", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_check_restrictions", "check", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function

End Class

Public Class bc_am_email_notify
    Private doc_id As String
    Private stage_to As Long
    Private mode As Integer = 0
    Private run_server_side As Boolean
    Public sql As String
    Public Sub New(ByVal doc_id As String, ByVal entity_id As Long, ByVal author_only As Boolean, ByVal run_server_side As Boolean)
        Me.doc_id = doc_id
        Me.stage_to = entity_id
        Me.run_server_side = run_server_side
        If author_only = True Then
            mode = 1
        End If
    End Sub
    Public Function run() As Boolean
        Dim slog = New bc_cs_activity_log("bc_am_email_notify", "run", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            REM now make this server side
            If run_server_side = True Then
                Dim ocommentary As New bc_cs_activity_log("bc_am_email_notify", "run", bc_cs_activity_codes.COMMENTARY, "Setting Email notify as sever side event")
                Me.sql = "exec dbo.bcc_core_wf_email_notify '" + CStr(doc_id) + "','" + CStr(stage_to) + "','" + CStr(mode) + "'"
                
                run = True
            Else

                    Dim osql As New bc_om_sql("exec dbo.bcc_core_wf_email_notify '" + CStr(doc_id) + "','" + CStr(stage_to) + "','" + CStr(mode) + "'")
                    run = False
                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                    osql.tmode = bc_om_soap_base_class.tREAD
                        osql.transmit_to_server_and_receive(osql, True)

                    ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        osql.db_read()
                    End If
                    If osql.success = True Then
                        run = True
                    Else
                        Dim ocommentary As New bc_cs_activity_log("bc_am_publish_data", "run_archive", bc_cs_activity_codes.COMMENTARY, "SQL Error: exec dbo.bcc_core_wf_email_notify '" + CStr(doc_id) + "','" + CStr(stage_to) + "'")
                    End If
                End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_email_notify", "run", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_email_notify", "run", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function

End Class
Public Class bc_am_check_dist_list
    Private doc_id As String
    
    Public Sub New(ByVal doc_id As String)
        Me.doc_id = doc_id
    End Sub
    Public Function check() As Boolean
        Dim slog = New bc_cs_activity_log("bc_am_check_dist_list", "check", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim min_num As Integer
            Dim email_num As Integer
            Dim paper_num As Integer
            Dim msg As String = ""
            Dim osql As New bc_om_sql("exec dbo.bcc_core_wf_check_dist_list '" + CStr(doc_id) + "'")
            check = False
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                osql.tmode = bc_om_soap_base_class.tREAD
                osql.transmit_to_server_and_receive(osql, True)

            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                osql.db_read()
            End If
            If osql.success = True Then
                If IsArray(osql.results) Then
                    If UBound(osql.results, 2) >= 0 Then
                        min_num = CLng(osql.results(0, 0))
                        email_num = CStr(osql.results(1, 0))
                        paper_num = CStr(osql.results(2, 0))
                        If paper_num >= min_num And email_num >= min_num Then
                            check = True
                            Exit Function
                        End If
                        If paper_num < min_num And email_num < min_num Then
                            msg = "Both Paper (" + CStr(paper_num) + "/" + CStr(min_num) + ") and Email distribution (" + CStr(email_num) + "/" + CStr(min_num) + ") lists are below minimum"
                        ElseIf paper_num < min_num Then
                            msg = "Paper distribution list (" + CStr(paper_num) + "/" + CStr(min_num) + ") is below minimum"
                        ElseIf email_num < min_num Then
                            msg = "Email distribution list (" + CStr(email_num) + "/" + CStr(min_num) + ") is below minimum"
                        End If
                        Dim omsg As New bc_cs_message("Distribution Lists", msg + " do you wish to continue with stage change? If not please check distribution lists via Reach Links.", bc_cs_message.MESSAGE, True)
                        If omsg.cancel_selected = False Then
                            check = True
                            Exit Function
                        End If
                    End If
                End If
            Else
                Dim ocommentary As New bc_cs_activity_log("bc_am_check_dist_list", "bc_am_check_dist_list", bc_cs_activity_codes.COMMENTARY, "SQL Error: exec dbo.bcc_core_wf_check_dist_list '" + CStr(doc_id) + "'")
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_check_dist_list", "check", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_check_dist_list", "check", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function

End Class
Public Class bc_am_first_call_note
    Private doc_id As String
    Public Sub New(ByVal doc_id As String, ByVal entity_id As Long)
        Me.doc_id = doc_id
    End Sub
    Public Function run() As Boolean
        Dim slog = New bc_cs_activity_log("bc_am_first_call_note", "run", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim osql As New bc_om_sql("exec dbo.bcc_core_wf_first_call_note '" + CStr(doc_id) + "'")
            run = False
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                osql.tmode = bc_om_soap_base_class.tREAD
                osql.transmit_to_server_and_receive(osql, True)

            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                osql.db_read()
            End If
            If osql.success = True Then
                run = True
            Else
                Dim ocommentary As New bc_cs_activity_log("bc_am_first_call_note", "run", bc_cs_activity_codes.COMMENTARY, "SQL Error: exec dbo.bcc_core_wf_first_call_note '" + CStr(doc_id) + "'")
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_first_call_note", "run", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_first_call_note", "run", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function

End Class
REM used 
Public Class bc_am_remote_workflow_event
    Private doc_id As String
    REM active document
    Private doc As Object
    Private ldoc As New bc_om_document
    Public override_stage As Long
    Public Sub New(ByVal doc_id As String, ByVal ldoc As bc_om_document)
        Me.doc_id = doc_id
        Me.ldoc = ldoc
    End Sub
    Public Function run(ByVal action_name As String) As Boolean
        REM call into workflow event handler
        Dim slog = New bc_cs_activity_log("bc_am_remote_workflow_event", "run", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            run = False
            Dim oworkflow_event As New bc_am_workflow_events_handler(doc, ldoc, False)
            run = oworkflow_event.run_system_event(action_name, ldoc, False, False, False, True)
            Me.override_stage = oworkflow_event.override_stage
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_remote_workflow_event", "run", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_remote_workflow_event", "run", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
    REM for actions that dont work on document object
    Public Sub get_doc_metadata()
        Dim slog = New bc_cs_activity_log("bc_am_remote_workflow_event", "get_doc_metadata", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            ldoc = New bc_om_document
            ldoc.id = Me.doc_id
            ldoc.bcheck_out = False
            ldoc.btake_revision = False
            ldoc.bwith_document = False
            ldoc.db_read()
            ldoc.id = Me.doc_id
            ldoc.history.Clear()
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_remote_workflow_event", "get_doc_metadata", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_remote_workflow_event", "get_doc_metadata", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    REM for actions that work on a document object
    Public Sub open_doc()
        Dim slog = New bc_cs_activity_log("bc_am_remote_workflow_event", "open_doc", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim ocommentary As bc_cs_activity_log
            bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO
            ocommentary = New bc_cs_activity_log("bc_am_remote_workflow_event", "open_doc", bc_cs_activity_codes.COMMENTARY, "Attempting to Open Document")
            ldoc = New bc_om_document
            REM recreate document object
            ldoc.id = Me.doc_id
            ldoc.bcheck_out = False
            ldoc.btake_revision = False
            ldoc.bwith_document = False
            ldoc.db_read()
            ldoc.id = Me.doc_id
            ldoc.history.Clear()
            Dim oopendoc As New bc_am_at_open_doc
            oopendoc.open(False, ldoc, False)
            REM assign active document across
            Me.doc = ldoc.docobject
            REM clear down history
            ldoc.history.Clear()


        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_remote_workflow_event", "open_doc", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_remote_workflow_event", "open_doc", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Public Sub close_doc()
        Dim slog = New bc_cs_activity_log("bc_am_remote_workflow_event", "open_doc", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim ocommentary As bc_cs_activity_log
            ocommentary = New bc_cs_activity_log("bc_am_remote_workflow_event", "open_doc", bc_cs_activity_codes.COMMENTARY, "Attempting to close Document after event")
            Dim osubmit As New bc_am_at_submit(False, doc, "word", False)
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_remote_workflow_event", "open_doc", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_remote_workflow_event", "open_doc", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
End Class
REM event create local copy and sends attches to email
Public Class bc_am_send_to_corp
    Public dest_pub_type_id As Long
    Public success As Boolean
    Public show_message As Boolean
    'Private ldoc As bc_om_document
    Private ldoc As bc_om_document
    Private odoc As bc_ao_word
    Private close_doc As Boolean
    Private refresh As Boolean
    Private ao_object As Object
    Private embargo_level As Integer

    Public Sub New(ByVal ldoc As bc_om_document, ByVal odoc As bc_ao_word, ByVal ao_object As Object, Optional ByVal close_doc As Boolean = False, Optional ByVal refresh As Boolean = True, Optional ByVal show_message As Boolean = False, Optional ByVal embargo_level As Integer = 0)
        Me.ldoc = ldoc
        Me.odoc = odoc
        Me.close_doc = close_doc
        Me.ao_object = ao_object
        Me.embargo_level = embargo_level
        Me.refresh = refresh
        Me.show_message = show_message
    End Sub
    Public Function copy() As Boolean
        Dim slog = New bc_cs_activity_log("bc_am_send_to_corp", "create_new_doc", bc_cs_activity_codes.TRACE_ENTRY, "")
        'Dim original_doc_id As Long

        Try
            Dim ffs As New bc_cs_file_transfer_services
            REM save original document
            Dim sfn As String
            Dim ps As New bc_cs_progress
            bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve", "Sending Document for Review", 5, False, True)
            bc_cs_central_settings.progress_bar.increment("Refreshing Embargo Copy...")

            REM refresh
            If ldoc.id = 0 Then
                sfn = ldoc.filename
            Else
                sfn = CStr(ldoc.id)
            End If
            odoc.visible(False)

            Dim orefresh As New bc_am_at_component_refresh(ao_object, "word")

            ldoc.refresh_components.embargo_level = Me.embargo_level
            orefresh.refresh(False, 1, False, ldoc)
            odoc.save()
            odoc.saveas(sfn)
            REM if adobe installed then render
            Dim orender As New bc_am_render_to_pdf(False, ao_object, "word", "Adobe PDF", ldoc.extension, 30, CStr(ldoc.id))
            bc_cs_central_settings.progress_bar.increment("Attempting to render to pdf...")
            ldoc.docobject = ao_object
            orender.render(ldoc)
            If orender.success = True Then
                bc_cs_central_settings.progress_bar.increment("Preparing Email with PDF ...")
                send_email(bc_cs_central_settings.local_repos_path + sfn + ".pdf", ldoc.title)
                If ffs.check_document_exists(bc_cs_central_settings.local_repos_path + sfn + ".pdf") Then
                    ffs.delete_file(bc_cs_central_settings.local_repos_path + sfn + ".pdf")
                Else
                    bc_cs_central_settings.progress_bar.increment("Preparing Email with DOC...")
                    REM send word copy
                    send_email(bc_cs_central_settings.local_repos_path + sfn + ".doc", ldoc.title)
                End If
            Else
                bc_cs_central_settings.progress_bar.increment("Preparing Email with DOC...")
                REM send word copy
                send_email(bc_cs_central_settings.local_repos_path + sfn + ".doc", ldoc.title)
            End If
            REM refresh original
            bc_cs_central_settings.progress_bar.increment("Refreshing Original...")
            ldoc.refresh_components.embargo_level = 0
            orefresh.refresh(False, 1, False, ldoc)

        Catch ex As Exception
            success = False
            copy = False
            Dim db_err As New bc_cs_error_log("bc_am_send_to_corp", "create_new_doc", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            REM switch back original
            bc_cs_central_settings.progress_bar.unload()
            odoc.visible(True)
            slog = New bc_cs_activity_log("bc_am_send_to_corp", "create_new_doc", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
    Private Function send_email(ByVal filename As String, ByVal title As String) As Boolean
        Dim slog As New bc_cs_activity_log("bc_am_send_to_corp", "send_email", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            REM hoo into outlook
            Dim ooutlook As Object
            Dim omail As Object
            Try
                ooutlook = GetObject(, "Outlook.application")
            Catch
                Try
                    ooutlook = CreateObject("Outlook.application")
                Catch ex As Exception
                    Dim db_err As New bc_cs_error_log("bc_ao_word", "invoke_word", bc_cs_error_codes.USER_DEFINED, ex.Message)
                    Exit Function
                End Try
            End Try
            With ooutlook
                omail = ooutlook.createitem(0)
                With omail
                    .subject = title + ": for review"
                    .attachments.add(filename, 1)
                    .Display()
                End With
            End With
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_send_to_corp", "send_email", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            REM switch back original
            slog = New bc_cs_activity_log("bc_am_send_to_corp", "send_email", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try


    End Function
End Class

Public Class bc_am_translate_document
    Public Sub New()

    End Sub
    REM loads relevant data for submit into memory
    Private Function load_data() As Boolean
        Dim slog = New bc_cs_activity_log("bc_am_translate_document", "load_data", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try

            load_data = False
            Dim i As Integer
            'Dim obc_objects As New bc_am_load_objects
            REM check connection
            REM should be getting this from document
            bc_cs_central_settings.version = Application.ProductVersion
            'bc_cs_central_settings.selected_conn_method = bc_am_load_objects.obc_current_document.connection_method
            REM users
            bc_am_load_objects.obc_users = bc_am_load_objects.obc_users.read_xml_from_file(bc_cs_central_settings.local_template_path + bc_am_load_objects.USERS_FILENAME)
            REM set logged on user
            For i = 0 To bc_am_load_objects.obc_users.user.Count - 1
                If LCase(bc_am_load_objects.obc_users.user(i).os_user_name) = LCase(bc_cs_central_settings.GetLoginName) Then
                    bc_cs_central_settings.logged_on_user_id = bc_am_load_objects.obc_users.user(i).id
                    Exit For
                End If
            Next
            REM entities
            bc_am_load_objects.obc_entities = bc_am_load_objects.obc_entities.read_xml_from_file(bc_cs_central_settings.local_template_path + bc_am_load_objects.ENTITIES_FILENAME)
            REM pub types
            bc_am_load_objects.obc_pub_types = bc_am_load_objects.obc_pub_types.read_xml_from_file(bc_cs_central_settings.local_template_path + bc_am_load_objects.PUB_TYPES_FILENAME)
            REM preferances
            bc_am_load_objects.obc_prefs = bc_am_load_objects.obc_pub_types.read_xml_from_file(bc_cs_central_settings.local_template_path + bc_am_load_objects.PREFS_FILENAME)

            load_data = True
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_translate_document", "load_data", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_translate_document", "load_data", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function
    Public Sub translate(ByVal show_form As Boolean, ByVal ao_object As Object, ByVal ao_type As String)
        Dim otrace As New bc_cs_activity_log("bc_am_translate_document", "translate", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            Dim bc_cs_central_settings As New bc_cs_central_settings


            REM read in config file of current doc
            Dim odoc As Object = Nothing
            Dim omessage As bc_cs_message
            Dim name As String
            If load_data() = False Then
                omessage = New bc_cs_message("Blue Curve Create", "Meta Data Failed to Load!", bc_cs_message.MESSAGE)
                Exit Sub
            End If
            REM only allow for role DTP or Manager
            Dim i As Integer
            Dim role As String = ""
            For i = 0 To bc_am_load_objects.obc_users.user.Count - 1
                With bc_am_load_objects.obc_users.user(i)
                    If bc_cs_central_settings.show_authentication_form = 0 Then
                        If UCase(Trim(.os_user_name)) = UCase(Trim(bc_cs_central_settings.logged_on_user_name)) Then
                            role = .role
                            Exit For
                        End If
                    Else
                        If UCase(Trim(.user_name)) = UCase(Trim(bc_cs_central_settings.user_name)) Then
                            role = .role
                            Exit For
                        End If
                    End If
                End With
            Next

            If Trim(UCase(role)) <> "DTP" And Trim(UCase(role)) <> "MANAGER" Then
                omessage = New bc_cs_message("Blue Curve Create", "You do not have correct role to translate document!", bc_cs_message.MESSAGE)
                Exit Sub
            End If
            REM instantiate correct AO object
            If ao_type = bc_ao_at_object.WORD_DOC Then
                odoc = New bc_ao_word(ao_object)
            Else
                omessage = New bc_cs_message("Blue Curve Create", "Objtec Not Yet Implemented: " + CStr(ao_type), bc_cs_message.MESSAGE)
            End If
            REM get filename
            name = odoc.get_name
            If Left(Right(name, 4), 1) = "." Then
                name = Left(name, Len(name) - 4)
            End If
            Dim fs As New bc_cs_file_transfer_services
            If fs.check_document_exists_and_can_be_read(bc_cs_central_settings.local_repos_path + name + ".xml") = False Then
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.LOCAL Then
                    omessage = New bc_cs_message("Blue Curve create", "Cannot Submit Document as no network and document was not opened via Create.", bc_cs_message.MESSAGE)
                    Exit Sub
                End If
                REM xml metadata file doesnt exists so attempt to recreate
                Dim ordm As New bc_am_recreate_doc_metadata
                If ordm.recreate_doc_metadata(name, odoc) = False Then
                    Exit Sub
                Else
                    bc_am_load_objects.obc_current_document = bc_am_load_objects.obc_current_document.read_xml_from_file(bc_cs_central_settings.local_repos_path + name + ".xml")
                End If
            Else
                REM load metadata
                bc_am_load_objects.obc_current_document = bc_am_load_objects.obc_current_document.read_xml_from_file(bc_cs_central_settings.local_repos_path + name + ".xml")
            End If
            bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve = create", "Translating Document...", 1, False, True)
            bc_cs_central_settings.progress_bar.increment("Translating Document...")
            REM if form authentication assign values
            If bc_cs_central_settings.show_authentication_form = 1 Then
                bc_cs_central_settings.user_name = bc_am_load_objects.obc_current_document.logged_on_user_name
                bc_cs_central_settings.user_password = bc_am_load_objects.obc_current_document.logged_on_user_password
            End If
            bc_cs_central_settings.selected_conn_method = bc_am_load_objects.obc_current_document.connection_method
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.LOCAL Then
                omessage = New bc_cs_message("Blue Curve Create", "Cannot translate as system working Locally!", bc_cs_message.MESSAGE)
                Exit Sub
            End If
            REM if document not yet network registered dont translate
            If bc_am_load_objects.obc_current_document.id = 0 Then
                omessage = New bc_cs_message("Blue Curve Create", "Cannot translate as document has never been submitted!", bc_cs_message.MESSAGE)
                Exit Sub
            End If
            REM call copy doc workflow event 
            Dim ocopy As New bc_am_copy_doc(bc_am_load_objects.obc_current_document.id, ao_object, False, True, True)
            ocopy.copy(bc_am_load_objects.obc_current_document)

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_translate_document", "translate", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Try
                bc_cs_central_settings.progress_bar.unload()
            Catch ex As Exception

            End Try

            otrace = New bc_cs_activity_log("bc_am_translate_document", "translate", bc_cs_activity_codes.TRACE_EXIT, "")

        End Try
    End Sub
End Class
