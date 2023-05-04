Imports System.Collections
Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports BlueCurve.Core.AS
Imports BlueCurve.Create.AM
Imports System.threading
Imports System.IO
Imports System.Windows.Forms
Imports System.Runtime.InteropServices
REM ING JUNE 2012
#Region "Changes"
REM -------------------------------------------------------------------------------------------------------------------
REM Changes:
REM Tracker                 Initials                   Date                      Synopsis
REM FIL JIRA 8035           PR                         2/01/2014                 client side event to save document locally
#End Region
REM Workflow Events Handler
Public Class bc_am_workflow_events_handler
    Public success As Boolean
    Public action_ids As New ArrayList
    Public doc_id As Long
    Public obj As Object
    Public client_flag As Boolean
    'Public ldoc As bc_om_document
    Public doc_open As Boolean
    Public doc_title As String
    Public stage_id_to As Long
    Public stage_name_to As String
    Public odoc As New bc_am_at_open_doc
    Public override_stage As Long
    Public suppress_no_trans As Boolean = False
    Public new_instance As Boolean = False
    Public last_revision_filename As String
    REM FIL 5.6.3
    Public keep_open_on_failure As Boolean = False
    Public suppress_failure_message As Boolean = False
    Public refresh As Boolean = False
    Public from_create As Boolean = False
    REM END
    Public Sub New(ByVal obj As Object, ByRef current_document As bc_om_document, ByVal client_flag As Boolean, Optional ByVal new_instance As Boolean = False)
        Me.action_ids = current_document.action_Ids
        Me.doc_id = current_document.id
        Me.client_flag = client_flag
        Me.doc_title = current_document.title
        'Me.ldoc = current_document
        Me.new_instance = new_instance

        REM PR check this
        Me.obj = obj
        success = False
    End Sub
    Public Sub run_events(ByRef ldoc As bc_om_document, Optional ByVal doc_open As Boolean = True, Optional ByVal show_messages As Boolean = True)
        REM run each event until an event fails
        REM if all succeed then set success to true
        Dim slog = New bc_cs_activity_log("bc_am_workflow_events_handler", "run_events", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim ocommentary As bc_cs_activity_log
        Dim history_list As New List(Of bc_om_history)
        Try
            Dim i, j As Integer
            Dim history As String = ""


            ldoc.server_side_events.events.Clear()
            'Me.ldoc = current_document
            REM clear out server side events
            If show_messages = True Then
                bc_cs_central_settings.progress_bar.unload()
                bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve - create", "Workflow Events", 12, False, True)
                bc_cs_central_settings.progress_bar.show()
            End If
            Dim suppress As Boolean = False
            Dim server_side_event As Boolean
            For i = 0 To action_ids.Count - 1
                REM match event to action list then execute
                For j = 0 To bc_am_load_objects.obc_pub_types.actions.actions.Count - 1
                    If action_ids(i) = bc_am_load_objects.obc_pub_types.actions.actions(j).id Then
                        If show_messages = True Then
                            bc_cs_central_settings.progress_bar.increment("Running Event " + bc_am_load_objects.obc_pub_types.actions.actions(j).name + " on Document: " + doc_title)
                        End If
                        REM system defined events
                        If bc_am_load_objects.obc_pub_types.actions.actions(j).calling_object = "system event" Or bc_am_load_objects.obc_pub_types.actions.actions(j).calling_object = "workflow_events.asp" Then
                            ocommentary = New bc_cs_activity_log("bc_am_workflow_events_handler", "run_events", bc_cs_activity_codes.COMMENTARY, "Running System Event " + bc_am_load_objects.obc_pub_types.actions.actions(j).name + " on Document: " + doc_title)
                            history = ""
                            server_side_event = False
                            success = run_system_event(bc_am_load_objects.obc_pub_types.actions.actions(j).name, ldoc, doc_open, history, bc_am_load_objects.obc_pub_types.actions.actions(j).calling_method)
                            If ldoc.doc_not_found = True Or bc_cs_central_settings.async_events_central_error_text.Count > 0 Then
                                success = False
                            End If
                        Else
                            ocommentary = New bc_cs_activity_log("bc_am_workflow_events_handler", "run_events", bc_cs_activity_codes.COMMENTARY, "Event " + bc_am_load_objects.obc_pub_types.actions.actions(j).name + " not implemented")
                        End If
                        Dim oh As bc_om_history
                        If history <> "" Then
                            oh = New bc_om_history
                            oh.comment = history
                            history_list.Add(oh)
                        End If
                        'Else
                        If Me.success = False Then
                            ocommentary = New bc_cs_activity_log("bc_am_workflow_events_handler", "run_events", bc_cs_activity_codes.COMMENTARY, "Event " + bc_am_load_objects.obc_pub_types.actions.actions(j).name + " failed on Document:" + doc_title)
                            history = "Client Side Event:  " + bc_am_load_objects.obc_pub_types.actions.actions(j).name + " failed"
                            oh = New bc_om_history
                            oh.comment = history
                            history_list.Add(oh)
                            REM if any event attached a support doc remove it
                            Dim m As Integer = 0
                            While m < ldoc.support_documents.Count
                                If ldoc.support_documents(m).from_event = True Then
                                    ldoc.support_documents.RemoveAt(m)
                                    m = m - 1
                                End If
                                m = m + 1
                            End While
                            Exit Sub
                        Else
                            history = "Client Side Event:  " + bc_am_load_objects.obc_pub_types.actions.actions(j).name + " succeeded"
                            ocommentary = New bc_cs_activity_log("bc_am_workflow_events_handler", "run_events", bc_cs_activity_codes.COMMENTARY, "Event " + bc_am_load_objects.obc_pub_types.actions.actions(j).name + " succeeded on Document:" + doc_title)
                            oh = New bc_om_history
                            oh.comment = history
                            history_list.Add(oh)
                        End If
                        'End If
                    End If
                Next
            Next
            Me.success = True


        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_workflow_events_handler", "run_events", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            ldoc.history = history_list
            If Me.success = False Then
                ldoc.component_componetize = False
            End If

            If show_messages = True Then
                bc_cs_central_settings.progress_bar.unload()
                bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve - create", "Submit", 12, False, True)
            End If
            slog = New bc_cs_activity_log("bc_am_workflow_events_handler", "run_events", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Dim copy_disclosures_class_id As Long
    Public Function run_system_event(ByVal action_name As String, ByRef ldoc As bc_om_document, Optional ByVal open_doc As Boolean = True, Optional ByRef history As String = "", Optional ByVal calling_method As String = "")
        Dim slog = New bc_cs_activity_log("bc_am_workflow_events_handler", "run_system_event", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim ocommentary As bc_cs_activity_log
        Dim i As Integer
        Try

            Dim obc_refresh As bc_am_at_component_refresh
            override_stage = 0
            ocommentary = New bc_cs_activity_log("bc_am_workflow_events_handler", "run_system_event", bc_cs_activity_codes.COMMENTARY, "Attempting to run action:" + action_name)
            run_system_event = False
            Dim cont As Boolean
            Dim render_only As Boolean = False
            'If action_name = "Componetiser" Then
            '    If IsNothing(obj) Then
            '        cont = check_for_open_doc(ldoc, open_doc, obj, render_only)
            '        If cont = False Then
            '            history = "Event: " + action_name + " surpressed as cant operate on mime type: " + ldoc.extension
            '            run_system_event = True
            '            Exit Function
            '        ElseIf render_only = True Then
            '            If action_name <> "Render" And action_name <> "Render and Publish" Then
            '                REM if action is not a render suppress
            '                history = "Event: " + action_name + " surpressed as cant operate on imported mime type: " + ldoc.extension
            '                run_system_event = True
            '                Exit Function
            '            End If
            '        End If


            '    Else
            '        ldoc.docobject = obj
            '    End If
            '    REM instanstatiate componitozer
            '    Dim obc_componetizer As New bc_am_at_componentize
            '    run_system_event = obc_componetizer.Componentise(False, obj, "word", ldoc.id)
            If action_name = "Style Componetiser" Then
                If IsNothing(obj) Then
                    cont = check_for_open_doc(ldoc, open_doc, obj, render_only)
                    If cont = False Then
                        history = "Event: " + action_name + " surpressed as cant operate on mime type: " + ldoc.extension
                        run_system_event = True
                        Exit Function
                    ElseIf render_only = True Then
                        If action_name <> "Render" And action_name <> "Render and Publish" Then
                            REM if action is not a render suppress
                            history = "Event: " + action_name + " surpressed as cant operate on imported mime type: " + ldoc.extension
                            run_system_event = True
                            Exit Function
                        End If
                    End If


                Else
                    ldoc.docobject = obj
                End If
                REM instanstatiate componitozer
                Dim style_componetiser As New bc_am_style_componetize
                run_system_event = style_componetiser.componetize(obj, "word", ldoc.id)
                If run_system_event = True Then
                    ldoc.style_components = style_componetiser.ocomponents
                Else
                    If style_componetiser.err_txt <> "" Then
                        history = "Client Side Event: " + action_name + " error: " + style_componetiser.err_txt
                    End If
                End If
            ElseIf action_name = "HTML Componetiser" Then
                If IsNothing(obj) Then
                    'cont = check_for_open_doc(ldoc, open_doc, obj, render_only)
                    cont = check_for_open_doc(ldoc, open_doc, obj, render_only, True, True)

                    If cont = False Then

                        history = "Event: " + action_name + " surpressed as cant operate on mime type: " + ldoc.extension
                        run_system_event = True
                        Exit Function

                    End If


                Else
                    ldoc.docobject = obj
                End If
                REM instanstatiate componitozer
                Dim style_componetiser As New bc_am_html_componetize
                run_system_event = style_componetiser.componetize(obj, "word", ldoc.id)
                If run_system_event = True Then
                    ldoc.style_components = style_componetiser.ocomponents
                Else
                    If style_componetiser.err_txt <> "" Then
                        history = "Client Side Event: " + action_name + " error: " + style_componetiser.err_txt
                    End If
                End If
            ElseIf action_name = "Render to mht" Then
                If IsNothing(obj) Then
                    cont = check_for_open_doc(ldoc, open_doc, obj, render_only)
                    If cont = False Then
                        history = "Event: " + action_name + " surpressed as cant operate on mime type: " + ldoc.extension
                        run_system_event = True
                        Exit Function
                    ElseIf render_only = True Then
                        If action_name <> "Render" And action_name <> "Render and Publish" Then
                            REM if action is not a render suppress
                            history = "Event: " + action_name + " surpressed as cant operate on imported mime type: " + ldoc.extension
                            run_system_event = True
                            Exit Function
                        End If
                    End If
                Else
                    ldoc.docobject = obj
                End If
                REM get filename and location
                Dim orender As New bc_am_render_to_mht(ldoc, obj)
                run_system_event = orender.render

                REM FIL JIRA 8035
            ElseIf action_name = "Make Local Copy" Then
                If IsNothing(obj) Then
                    cont = check_for_open_doc(ldoc, open_doc, obj, render_only)
                    If cont = False Then
                        history = "Event: " + action_name + " surpressed as cant operate on mime type: " + ldoc.extension
                        run_system_event = True
                        Exit Function
                    ElseIf render_only = True Then
                        If action_name <> "Render" And action_name <> "Render and Publish" Then
                            REM if action is not a render suppress
                            history = "Event: " + action_name + " surpressed as cant operate on imported mime type: " + ldoc.extension
                            run_system_event = True
                            Exit Function
                        End If
                    End If
                Else
                    ldoc.docobject = obj
                End If
                REM get filename and location
                Dim omakelocalcopy As New bc_am_make_local_copy(ldoc, obj)
                run_system_event = omakelocalcopy.copy


            ElseIf action_name = "Componetise Component" Then
                If IsNothing(obj) Then
                    cont = check_for_open_doc(ldoc, open_doc, obj, render_only)
                    If cont = False Then
                        history = "Event: " + action_name + " surpressed as cant operate on mime type: " + ldoc.extension
                        run_system_event = True
                        Exit Function
                    ElseIf render_only = True Then
                        If action_name <> "Render" And action_name <> "Render and Publish" Then
                            REM if action is not a render suppress
                            history = "Event: " + action_name + " surpressed as cant operate on imported mime type: " + ldoc.extension
                            run_system_event = True
                            Exit Function
                        End If
                    End If
                Else
                    ldoc.docobject = obj
                End If
                REM instanstatiate componitozer
                Dim obc_componetizer As New bc_am_componetize_component
                run_system_event = obc_componetizer.componetize(obj, ldoc)
                If run_system_event = True Then
                    ldoc.component_componetize = True
                End If



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
                    cont = check_for_open_doc(ldoc, open_doc, obj, render_only)
                    If cont = False Then
                        history = "Event: " + action_name + " surpressed as cant operate on mime type: " + ldoc.extension
                        run_system_event = True
                        Exit Function
                    ElseIf render_only = True Then
                        If action_name <> "Render" And action_name <> "Render and Publish" Then
                            REM if action is not a render suppress
                            history = "Event: " + action_name + " surpressed as cant operate on imported mime type: " + ldoc.extension
                            run_system_event = True
                            Exit Function
                        End If
                    End If
                Else
                    ldoc.docobject = obj
                End If
                If Me.stage_id_to <> 0 Then
                    ldoc.stage = Me.stage_id_to
                    ldoc.stage_name = Me.stage_name_to
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
                    cont = check_for_open_doc(ldoc, open_doc, obj, render_only)
                    If cont = False Then
                        history = "Event: " + action_name + " surpressed as cant operate on mime type: " + ldoc.extension
                        run_system_event = True
                        Exit Function
                    ElseIf render_only = True Then
                        If action_name <> "Render" And action_name <> "Render and Publish" Then
                            REM if action is not a render suppress
                            history = "Event: " + action_name + " surpressed as cant operate on imported mime type: " + ldoc.extension
                            run_system_event = True
                            Exit Function
                        End If
                    End If
                Else
                    ldoc.docobject = obj
                End If
                Dim ocopy As New bc_am_copy_doc(ldoc.id, obj, True, False)
                ocopy.copy(ldoc)
                run_system_event = ocopy.success

            ElseIf action_name = "Insert Disclosures" Then
                If IsNothing(obj) Then
                    cont = check_for_open_doc(ldoc, open_doc, obj, render_only)
                    If cont = False Then
                        history = "Event: " + action_name + " surpressed as cant operate on mime type: " + ldoc.extension
                        run_system_event = True
                        Exit Function
                    ElseIf render_only = True Then
                        If action_name <> "Render" And action_name <> "Render and Publish" Then
                            REM if action is not a render suppress
                            history = "Event: " + action_name + " surpressed as cant operate on imported mime type: " + ldoc.extension
                            run_system_event = True
                            Exit Function
                        End If
                    End If
                Else
                    ldoc.docobject = obj
                End If
                ldoc.write_data_to_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ".dat")
                obc_refresh = New bc_am_at_component_refresh(False, obj, "word", 2, False)
                REM after refresh formatter
                Try
                    obj.bc_refresh_post(obj)
                Catch ex As Exception
                    ocommentary = New bc_cs_activity_log("bc_am_workflow_events_handler", "run_system_event", bc_cs_activity_codes.COMMENTARY, "Failed to find vba bc_refresh_post: " + ex.Message)
                End Try
                ldoc = ldoc.read_data_from_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ".dat")
                run_system_event = obc_refresh.success

            ElseIf action_name = "Insert Disclosures PowerPoint" Then

                If ldoc.extension <> ".ppt" And ldoc.extension <> ".pptx" And ldoc.extension <> ".pptm" Then
                    history = "Event: " + action_name + " surpressed as cant operate on mime type: " + ldoc.extension
                    run_system_event = True
                    Exit Function
                End If
                If IsNothing(obj) Then
                    obj = New bc_ao_powerpoint

                    cont = check_for_open_doc_powerpoint(ldoc, open_doc, obj, render_only)
                    If cont = False Then
                        history = "Event: " + action_name + " failed powerpoint document couldnt be opened " + ldoc.filename
                        run_system_event = False
                    End If
                Else
                    ldoc.docobject = obj
                End If

                ldoc.write_data_to_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ".dat")

                obc_refresh = New bc_am_at_component_refresh(False, obj, "powerpoint", 2, False)
                REM after refresh formatter
                'Try
                '    obj.bc_refresh_post(obj)
                'Catch ex As Exception
                '    ocommentary = New bc_cs_activity_log("bc_am_workflow_events_handler", "run_system_event", bc_cs_activity_codes.COMMENTARY, "Failed to find vba bc_refresh_post: " + ex.Message)
                'End Try
                ldoc = ldoc.read_data_from_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ".dat")
                run_system_event = obc_refresh.success

            ElseIf action_name = "Insert Disclosures Excel" Then

               
               
    
                If ldoc.extension <> "[imp].xls" And ldoc.extension <> "[imp].xlsx" And ldoc.extension <> "[imp].xlsm" And ldoc.extension <> ".xlsx" And ldoc.extension <> ".xlsm" Then
                    history = "Event: " + action_name + " surpressed as cant operate on mime type: " + ldoc.extension
                    run_system_event = True
                    Exit Function
                End If

              


                Dim oexcel As New bc_ao_excel()
                If IsNothing(obj) Then
                    cont = check_for_open_doc_excel(ldoc, open_doc, obj, oexcel)
                    If cont = False Then
                        history = "Event: " + action_name + " surpressed as cant operate on mime type: " + ldoc.extension
                        run_system_event = True
                        Exit Function
                    End If
                Else
                    ldoc.docobject = obj
                    oexcel.docobject = obj
                End If
                
                oexcel.excelworkbook = oexcel.docobject

                ldoc.bwith_document = True
                odoc.odoc = obj

              
                Dim oeid As New insert_excel_disclosures

                run_system_event = oeid.run(ldoc, oexcel)



            ElseIf action_name = "Refresh Document" Then
                    If IsNothing(obj) Then
                        cont = check_for_open_doc(ldoc, open_doc, obj, render_only)
                        If cont = False Then
                            history = "Event: " + action_name + " surpressed as cant operate on mime type: " + ldoc.extension
                            run_system_event = True
                            Exit Function
                        ElseIf render_only = True Then
                            If action_name <> "Render" And action_name <> "Render and Publish" Then
                                REM if action is not a render suppress
                                history = "Event: " + action_name + " surpressed as cant operate on imported mime type: " + ldoc.extension
                                run_system_event = True
                                Exit Function
                            End If
                        End If
                    Else
                        ldoc.docobject = obj
                    End If
                    ldoc.write_data_to_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ".dat")
                    obc_refresh = New bc_am_at_component_refresh(False, obj, "word", 1, False)
                    ldoc = ldoc.read_data_from_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ".dat")
                    run_system_event = obc_refresh.success
                    REM FIL JUNE 2013
            ElseIf Len(action_name) >= 18 AndAlso Left(action_name, 18) = "Refresh Components" Then
                    If IsNothing(obj) Then
                        cont = check_for_open_doc(ldoc, open_doc, obj, render_only)
                        If cont = False Then
                            history = "Event: " + action_name + " surpressed as cant operate on mime type: " + ldoc.extension
                            run_system_event = True
                            Exit Function
                        ElseIf render_only = True Then
                            If action_name <> "Render" And action_name <> "Render and Publish" Then
                                REM if action is not a render suppress
                                history = "Event: " + action_name + " surpressed as cant operate on imported mime type: " + ldoc.extension
                                run_system_event = True
                                Exit Function
                            End If
                        End If
                    Else
                        ldoc.docobject = obj
                    End If
                    ldoc.write_data_to_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ".dat")
                    REM match event to action list then execute


                    If calling_method <> "" Then
                        obc_refresh = New bc_am_at_component_refresh(False, obj, "word", 1, False, False, False, False, calling_method)
                        ldoc = ldoc.read_data_from_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ".dat")
                        run_system_event = obc_refresh.success
                    Else
                        history = "Event: " + action_name + " failed as no component list stored procedure "
                        run_system_event = False
                    End If


                    'ElseIf action_name = "Render" Or action_name = "Render and Publish" Then
                    '    If IsNothing(obj) Then
                    '        cont = check_for_open_doc(ldoc, open_doc, obj, render_only, True)
                    '        If cont = False Then
                    '            history = "Event: " + action_name + " surpressed as cant operate on mime type: " + ldoc.extension
                    '            run_system_event = True
                    '            Exit Function
                    '        ElseIf render_only = True Then
                    '            If action_name <> "Render" And action_name <> "Render and Publish" Then
                    '                REM if action is not a render suppress
                    '                history = "Event: " + action_name + " surpressed as cant operate on imported mime type: " + ldoc.extension
                    '                run_system_event = True
                    '                Exit Function
                    '            End If
                    '        End If
                    '    Else
                    '        ldoc.docobject = obj
                    '    End If
                    '    Dim obc_render As New bc_am_render_to_pdf(False, obj, "word", "Adobe PDF", ldoc.extension, 30, CStr(ldoc.id))
                    '    run_system_event = obc_render.render(ldoc)
                    '    run_system_event = obc_render.success
                    '    If action_name = "Render and Publish" Then
                    '        ldoc.publish_flag = True
                    '    End If
                    'ElseIf action_name = "Render with PDFMaker" Then
                    '    If IsNothing(obj) Then
                    '        cont = check_for_open_doc(ldoc, open_doc, obj, render_only, True)
                    '        If cont = False Then
                    '            history = "Event: " + action_name + " surpressed as cant operate on mime type: " + ldoc.extension
                    '            run_system_event = True
                    '            Exit Function
                    '        ElseIf render_only = True Then
                    '            If action_name <> "Render with PDFMaker" Then
                    '                REM if action is not a render suppress
                    '                history = "Event: " + action_name + " surpressed as cant operate on imported mime type: " + ldoc.extension
                    '                run_system_event = True
                    '                Exit Function
                    '            End If
                    '        End If
                    '    Else
                    '        ldoc.docobject = obj
                    '    End If
                    '    Dim obc_render As New bc_am_render_to_pdf(False, obj, "word", "Adobe PDF", ldoc.extension, 30, CStr(ldoc.id))
                    '    run_system_event = obc_render.render_with_pdfmaker(ldoc)
                    '    run_system_event = obc_render.success
                    '    'If action_name = "Render and Publish" Then
                    '    '    ldoc.publish_flag = True
                '    'End If

           

            ElseIf action_name = "Render with office" Then
                    Dim nbcr As bc_am_render
                    Dim allow_pdf_types As New List(Of String)
                Dim allow_pdf As Boolean = False
                allow_pdf_types.Add("[imp].docx")
                    allow_pdf_types.Add("[imp].doc")
                    allow_pdf_types.Add("[imp].docx")
                    allow_pdf_types.Add("[imp].docm")
                    allow_pdf_types.Add("[imp].xlsx")
                    allow_pdf_types.Add("[imp].xlsm")
                    allow_pdf_types.Add("[imp].pptx")
                    allow_pdf_types.Add("[imp].pptm")
                    allow_pdf_types.Add("[imp].xls")

                    allow_pdf_types.Add("[imp].ppt")
                    allow_pdf_types.Add(".pptx")
                    allow_pdf_types.Add(".ppt")
                allow_pdf_types.Add(".doc")
                allow_pdf_types.Add(".docx")
                allow_pdf_types.Add(".docm")
                    For i = 0 To allow_pdf_types.Count - 1
                        If allow_pdf_types(i) = ldoc.extension Then
                            allow_pdf = True
                            Exit For
                        End If
                    Next

                    REM see if we can allow a non BC document to be rendered
                    If allow_pdf = False Then
                        history = "Event: " + action_name + " surpressed as cant operate on mime type: " + ldoc.extension
                        run_system_event = True
                        Exit Function
                    End If

                    If IsNothing(obj) Then
                        cont = check_for_open_doc(ldoc, open_doc, obj, render_only, True)

                        If cont = False Then

                            REM non BC document

                            nbcr = New bc_am_render
                            If nbcr.render_non_bc(ldoc) = True Then
                                ldoc.support_doc_non_open_doc_event = True
                                run_system_event = True
                                Exit Function
                            Else
                                Dim ocomm As New bc_cs_activity_log("bc_am_workflow_event_handler", "render_to_pdf_non_bc", bc_cs_activity_codes.COMMENTARY, nbcr.err_text)
                                Exit Function
                            End If
                        End If
                    Else
                        ldoc.docobject = obj
                    End If

                    nbcr = New bc_am_render
                    

                    If nbcr.render_bc(ldoc, ldoc.docobject) = True Then
                        run_system_event = True
                        Exit Function
                    Else
                        Dim ocomm As New bc_cs_activity_log("bc_am_workflow_event_handler", "render_to_pdf_bc", bc_cs_activity_codes.COMMENTARY, nbcr.err_text)
                        Exit Function
                    End If

                    'Dim obc_render As New bc_am_render_to_pdf(False, obj, "word", "Adobe PDF", ldoc.extension, 30, CStr(ldoc.id))
                    'run_system_event = obc_render.rendertoPDF(ldoc)
                    'run_system_event = obc_render.success

                    If action_name = "Render with office" Then
                        ldoc.publish_flag = True
                    End If


            ElseIf action_name = "Set Doc Date" Then
                    If IsNothing(obj) Then
                        cont = check_for_open_doc(ldoc, open_doc, obj, render_only, True)
                        If cont = False Then
                            history = "Event: " + action_name + " surpressed as cant operate on mime type: " + ldoc.extension
                            run_system_event = True
                            Exit Function
                        ElseIf render_only = True Then
                            If action_name <> "Render" And action_name <> "Render and Publish" And action_name <> "Render with office" Then
                                REM if action is not a render suppress 
                                history = "Event: " + action_name + " surpressed as cant operate on imported mime type: " + ldoc.extension
                                run_system_event = True
                                Exit Function
                            End If
                        End If

                    Else
                        ldoc.docobject = obj
                    End If

                    REM pass doc date through to document
                    Try
                        obj.update_doc_date(ldoc.doc_date.ToLocalTime)
                        'obj.update_doc_date(ldoc.doc_date)
                        run_system_event = True
                    Catch ex As Exception
                        Dim ocomm As New bc_cs_activity_log("bc_am_workflow_events_handler", "run_system_event", bc_cs_activity_codes.COMMENTARY, ex.Message)
                        run_system_event = False
                    End Try
            ElseIf action_name = "Check Refresh" Then
                    run_system_event = False

                    Dim osql As bc_om_sql
                    osql = New bc_om_sql("exec dbo.bc_core_check_refresh " + CStr(ldoc.id) + "," + CStr(bc_cs_central_settings.logged_on_user_id))

                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        osql.db_read()
                    Else
                        osql.tmode = bc_cs_soap_base_class.tREAD
                        If osql.transmit_to_server_and_receive(osql, False) = False Then
                            Exit Function
                        End If
                    End If
                    If osql.success = True Then
                        Try
                            If osql.results(0, 0) = 0 Then
                                run_system_event = True
                            Else
                                Dim omsg As bc_cs_message
                                If from_create = True Then
                                    omsg = New bc_cs_message("Blue Curve", CStr(osql.results(1, 0)), bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
                                    If omsg.cancel_selected = False Then
                                        run_system_event = False
                                        keep_open_on_failure = True
                                        suppress_failure_message = True
                                        omsg = New bc_cs_message("Blue Curve", CStr(osql.results(3, 0)), bc_cs_message.MESSAGE, False, False, "Ok", "No", True)
                                        refresh = False
                                    Else
                                        run_system_event = True
                                    End If
                                Else
                                    omsg = New bc_cs_message("Blue Curve", CStr(osql.results(2, 0)), bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
                                    If omsg.cancel_selected = False Then
                                        run_system_event = False
                                        suppress_failure_message = True
                                        omsg = New bc_cs_message("Blue Curve", CStr(osql.results(4, 0)), bc_cs_message.MESSAGE, False, False, "Ok", "No", True)

                                    Else
                                        run_system_event = True
                                    End If

                                End If
                            End If
                        Catch ex As Exception
                            Dim ocomm As New bc_cs_activity_log("bc_am_workflow_events_handler", "run_system_event", bc_cs_activity_codes.COMMENTARY, "failed to execute bc_core_check_refresh " + CStr(ldoc.id) + ": " + ex.Message)
                            Exit Function
                        End Try
                    Else
                        Dim ocomm As New bc_cs_activity_log("bc_am_workflow_events_handler", "run_system_event", bc_cs_activity_codes.COMMENTARY, "failed to execute bc_core_check_refresh " + CStr(ldoc.id))
                        Exit Function
                    End If
            ElseIf Len(action_name) > 22 AndAlso Left(action_name, 22) = "Check Data Consistancy" Then
                    If IsNothing(obj) Then
                        cont = check_for_open_doc(ldoc, open_doc, obj, render_only, True)
                        If cont = False Then
                            history = "Event: " + action_name + " surpressed as cant operate on mime type: " + ldoc.extension
                            run_system_event = True
                            Exit Function
                        ElseIf render_only = True Then
                            If action_name <> "Render" And action_name <> "Render and Publish" And action_name <> "Render with office" Then
                                REM if action is not a render suppress 
                                history = "Event: " + action_name + " surpressed as cant operate on imported mime type: " + ldoc.extension
                                run_system_event = True
                                Exit Function
                            End If
                        End If

                    Else
                        ldoc.docobject = obj
                    End If

                    run_system_event = False
                    REM loop through each item to check object model against document version
                    Dim cid As String

                    cid = calling_method

                    REM get value from object model
                    Dim k As bc_om_refresh_component
                    Dim val1, val2 As String
                    For j = 0 To ldoc.refresh_components.refresh_components.Count - 1
                        If (InStr(ldoc.refresh_components.refresh_components(j).locator, "at" + cid) > 0) Then
                            val1 = ldoc.refresh_components.refresh_components(j).parameters.component_template_parameters(0).default_value_id
                            val2 = obj.bookmarks(ldoc.refresh_components.refresh_components(j).locator).range.text
                            REM these into an SP for a compare
                            Dim st As New bc_cs_string_services(val1)
                            val1 = st.delimit_apostrophies
                            st = New bc_cs_string_services(val2)
                            val2 = st.delimit_apostrophies
                            Dim osql As bc_om_sql
                            osql = New bc_om_sql("exec dbo.bc_core_data_consistancy " + CStr(cid) + ",'" + CStr(val1) + "','" + CStr(val2) + "'")
                            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                                osql.db_read()
                            Else
                                osql.tmode = bc_cs_soap_base_class.tREAD
                                If osql.transmit_to_server_and_receive(osql, False) = False Then
                                    Exit Function
                                End If
                            End If
                            If osql.success = True Then
                                Try
                                    If osql.results(0, 0) = 0 Then
                                        run_system_event = True
                                    Else
                                        Dim omsg As bc_cs_message
                                        omsg = New bc_cs_message("Blue Curve", CStr(osql.results(1, 0)), bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                                        run_system_event = False
                                        keep_open_on_failure = True
                                        suppress_failure_message = True
                                    End If
                                Catch ex As Exception
                                    Dim ocomm As New bc_cs_activity_log("bc_am_workflow_events_handler", "run_system_event", bc_cs_activity_codes.COMMENTARY, "failed to execute bc_core_check_refresh " + CStr(ldoc.id) + ": " + ex.Message)
                                    Exit Function
                                End Try
                            Else
                                Dim ocomm As New bc_cs_activity_log("bc_am_workflow_events_handler", "run_system_event", bc_cs_activity_codes.COMMENTARY, action_name + ": failed to execute bc_core_data_consistanc " + CStr(ldoc.id))
                                Exit Function
                            End If

                            Exit For
                        End If
                    Next


            ElseIf action_name = "Confirm Stage Change" Then

                    run_system_event = False
                    Dim osql As bc_om_sql
                    osql = New bc_om_sql("exec dbo.bc_core_confirm_stage_change " + CStr(ldoc.id) + "," + CStr(Me.stage_id_to) + "," + CStr(bc_cs_central_settings.logged_on_user_id))

                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        osql.db_read()
                    Else
                        osql.tmode = bc_cs_soap_base_class.tREAD
                        If osql.transmit_to_server_and_receive(osql, False) = False Then
                            Exit Function
                        End If
                    End If
                    If osql.success = True Then
                        Try
                            If osql.results(0, 0) = 0 Then
                                run_system_event = True
                            Else
                                Dim omsg As bc_cs_message
                                omsg = New bc_cs_message("Blue Curve", CStr(osql.results(1, 0)), bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
                                If omsg.cancel_selected = False Then
                                    run_system_event = True
                                    history = osql.results(2, 0)
                                Else
                                    run_system_event = False
                                    suppress_failure_message = True
                                    'history = "Event: " + action_name + " user aborted stage change as rejected confirmation."
                                End If
                            End If
                        Catch ex As Exception
                            Dim ocomm As New bc_cs_activity_log("bc_am_workflow_events_handler", "run_system_event", bc_cs_activity_codes.COMMENTARY, "failed to execute bc_core_confirm_stage_change " + CStr(ldoc.id) + ": " + ex.Message)
                            Exit Function
                        End Try
                    Else
                        Dim ocomm As New bc_cs_activity_log("bc_am_workflow_events_handler", "run_system_event", bc_cs_activity_codes.COMMENTARY, "failed to execute bc_core_confirm_stage_change " + CStr(ldoc.id))
                        Exit Function
                    End If

            ElseIf action_name = "Copy Disclosures" Then
                    run_system_event = False
                    Dim cp As New bc_om_copy_disclosures

                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    cp.db_read()
                    run_system_event = True
                    Else
                        cp.tmode = bc_cs_soap_base_class.tREAD
                        If cp.transmit_to_server_and_receive(cp, False) = True Then
                            run_system_event = True
                        End If

                    End If
                    copy_disclosures_class_id = cp.class_id

            ElseIf Len(action_name) >= 23 AndAlso Left(action_name, 23) = "Create Support Document" Then

                    run_system_event = False

                    Dim bd As New bc_am_build_automated_support_document
                    bd.template_name = calling_method

                    bd.master_doc = ldoc
                    bd.title = Right(action_name, Len(action_name) - 24)
                    ldoc.support_doc_non_open_doc_event = True

                    run_system_event = bd.create_doc(copy_disclosures_class_id)
                    If bd.err_text <> "" Then
                        history = bd.err_text
                    End If

            ElseIf action_name = "Show Attestation" Then

                    run_system_event = False


                    Dim fass As New bc_am_attestation

                    Dim ass As New Cbc_am_attestation
                    If ass.load_data(Me.doc_id, bc_cs_central_settings.logged_on_user_id, "", fass) = True Then
                        fass.ShowDialog()
                    Else
                        If ass.err_text <> "" Then
                            history = "Client Side Event: " + action_name + " error: " + ass.err_text
                        End If
                        Dim ocomm As New bc_cs_activity_log("bc_am_workflow_events_handler", "run_system_event", bc_cs_activity_codes.COMMENTARY, "failed to load data for Show Assertation")
                        Exit Function
                    End If
                    If ass._cancel = False Then
                        history = "Attestation Answers: "
                        For i = 0 To ass._config.questions.Count - 1
                            history = history + " " + CStr(i + 1) + ")" + ass._config.questions(i).option_selected_text
                        Next
                        run_system_event = True
                    Else
                        If from_create = True Then
                            Dim omsg = New bc_cs_message("Blue Curve", "Assertation Cancelled Document will remain open.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                            run_system_event = False
                            keep_open_on_failure = True
                            suppress_failure_message = True
                        Else
                            Dim omsg = New bc_cs_message("Blue Curve", "Assertation Cancelled Document will remain in Draft", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                            run_system_event = False
                            suppress_failure_message = True
                        End If
                    End If
            ElseIf action_name = "Set Excel Data" Then
                    Dim oexcel As New bc_ao_excel()
                    If IsNothing(obj) Then
                        cont = check_for_open_doc_excel(ldoc, open_doc, obj, oexcel)
                        If cont = False Then
                            history = "Event: " + action_name + " surpressed as cant operate on mime type: " + ldoc.extension
                            run_system_event = True
                            Exit Function
                        End If
                    Else
                        ldoc.docobject = obj
                        oexcel.docobject = obj
                    End If
                    Dim iexcel As New bc_am_insert_data_into_excel(oexcel, ldoc.id)

                    ldoc.bwith_document = True

                    run_system_event = iexcel.run


            ElseIf action_name = "Document Scanning" Or action_name = "Remove Highlighting" Then
                    If IsNothing(obj) Then
                        cont = check_for_open_doc(ldoc, open_doc, obj, render_only)
                        If cont = False Then
                            history = "Event: " + action_name + " surpressed as cant operate on mime type: " + ldoc.extension
                            run_system_event = True
                            Exit Function
                        ElseIf render_only = True Then
                            If action_name <> "Render" And action_name <> "Render and Publish" Then
                                REM if action is not a render suppress
                                history = "Event: " + action_name + " surpressed as cant operate on imported mime type: " + ldoc.extension
                                run_system_event = True
                                Exit Function
                            End If
                        End If
                    Else
                        ldoc.docobject = obj
                    End If
                    ldoc.write_data_to_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ".dat")
                    Dim scanDoc As New bc_am_scan_document
                    Dim scanResults As ArrayList = Nothing
                    Dim results(2, 0) As String
                    scanDoc.scan_document(calling_method, obj, "word", scanResults)
                    If scanDoc.success = False Then
                        run_system_event = False
                        Exit Function
                    End If
                    ldoc = ldoc.read_data_from_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ".dat")
                    If Not scanResults Is Nothing AndAlso scanResults.Count > 0 Then
                        For i = 0 To scanResults.Count - 1
                            ReDim Preserve results(2, i)
                            results(0, i) = CType(scanResults(i), String(,))(0, 0)
                            results(1, i) = CType(scanResults(i), String(,))(1, 0)
                            results(2, i) = CType(scanResults(i), String(,))(2, 0)
                        Next

                        REM highlight scan results
                        Try
                            If action_name = "Document Scanning" Then
                                obj.DocumentHighlighting(results, obj)
                            Else
                                obj.RemoveDocumentHighlighting(results, obj)
                            End If
                        Catch ex As Exception
                            ocommentary = New bc_cs_activity_log("bc_am_workflow_events_handler", "run_system_event", bc_cs_activity_codes.COMMENTARY, "Failed to find vba DocumentHighlighting: " + ex.Message)

                        End Try

                        'move to specified stage
                        Dim oh As New bc_om_history

                        If action_name = "Document Scanning" Then
                            If scanDoc.GetStage = False Then
                                run_system_event = False
                                Exit Function
                            End If
                            If scanDoc.stage_out > 0 Or Me.stage_id_to <> scanDoc.stage_out Then
                                ldoc.stage = scanDoc.stage_out
                                ldoc.stage_name = scanDoc.stage_out_name
                                Me.stage_id_to = scanDoc.stage_out
                                Me.stage_name_to = scanDoc.stage_out_name

                                oh = New bc_om_history
                                history = "Document has restricted words / phrases and will be moved to stage: " + scanDoc.stage_out_name

                            Else
                                oh = New bc_om_history
                                history = "Document has restricted words / phrases but stage not overriden"

                            End If

                        End If
                    End If
                    run_system_event = scanDoc.success
                    REM BOI Feb 2014
            ElseIf action_name = "Email Preview" Then
                    Dim obc_preview As New bc_am_email_preview(ldoc.id)
                    run_system_event = obc_preview.preview



                    REM no longer supported Me.suppress_trans_fail = True


                    'ElseIf action_name = "Check Duplicate" Then
                    '    Dim obc_check_duplicate As New bc_am_check_duplicate(CStr(ldoc.pub_type_id), CStr(ldoc.entity_id), CStr(ldoc.id))
                    '    If obc_check_duplicate.check_duplicate = True Then
                    '        Me.success = True
                    '        suppress = True
                    '        Dim omessage As New bc_cs_message("Blue Curve", obc_check_duplicate.out_text + " Document wont be allowed to be release from current stage", bc_cs_message.MESSAGE)
                    '        ldoc.history.Add(obc_check_duplicate.out_text + " Document wont be allowed to be released from current stage")
                    '        ldoc.stage = ldoc.original_stage
                    '        ldoc.stage_name = ldoc.original_stage_name

                    '        Me.stage_id_to = ldoc.original_stage
                    '        Me.stage_name_to = ldoc.original_stage_name
                    '    End If
                    '    run_system_event = True
                    'ElseIf action_name = "Style Componetiser" Then
                    '    If IsNothing(obj) Then
                    '        cont = check_for_open_doc(open_doc, obj, render_only)
                    '        If cont = False Then
                    '            history = "Event: " + action_name + " surpressed as cant operate on mime type: " + ldoc.extension
                    '            run_system_event = True
                    '            Exit Function
                    '        ElseIf render_only = True Then
                    '            If action_name <> "Render" And action_name <> "Render and Publish" Then
                    '                REM if action is not a render suppress
                    '                history = "Event: " + action_name + " surpressed as cant operate on imported mime type: " + ldoc.extension
                    '                run_system_event = True
                    '                Exit Function
                    '            End If
                    '        End If
                    '    Else
                    '        ldoc.docobject = obj
                    '    End If
                    '    REM instanstatiate componitozer
                    '    Dim obc_componetizer As New bc_am_style_componetize
                    '    run_system_event = obc_componetizer.componetize(obj, "word", ldoc.id)
                    'ElseIf action_name = "Upload Translations" Then
                    '    REM this event only run on a copy document
                    '    If ldoc.translated_from_doc = 0 Then
                    '        history = "Upload Translations surpressed not a copy document"
                    '        run_system_event = True
                    '    Else
                    '        Dim tdoc As New bc_om_document
                    '        tdoc.translated_from_doc = ldoc.translated_from_doc
                    '        tdoc.id = ldoc.id
                    '        tdoc.language_id = ldoc.language_id
                    '        tdoc.entity_id = ldoc.entity_id
                    '        tdoc.receive_translation = True
                    '        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    '            tdoc.db_read()
                    '        Else
                    '            tdoc.tmode = bc_cs_soap_base_class.tREAD
                    '            tdoc.transmit_to_server_and_receive(tdoc, True)
                    '        End If

                    '        If tdoc.uploaded_translated_components = True Then
                    '            REM now open document and refresh
                    '            If IsNothing(obj) Then
                    '                cont = check_for_open_doc(open_doc, obj, render_only)
                    '                If cont = False Then
                    '                    history = "Event: " + action_name + " surpressed as cant operate on mime type: " + ldoc.extension
                    '                    run_system_event = True
                    '                    Exit Function
                    '                ElseIf render_only = True Then
                    '                    If action_name <> "Render" And action_name <> "Render and Publish" Then
                    '                        REM if action is not a render suppress
                    '                        history = "Event: " + action_name + " surpressed as cant operate on imported mime type: " + ldoc.extension
                    '                        run_system_event = True
                    '                        Exit Function
                    '                    End If
                    '                End If
                    '            Else
                    '                ldoc.docobject = obj
                    '            End If
                    '            ldoc.write_data_to_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ".dat")
                    '            obc_refresh = New bc_am_at_component_refresh(False, obj, "word", 1, False)
                    '            ldoc = ldoc.read_data_from_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ".dat")
                    '            run_system_event = True
                    '        Else
                    '            history = "Translation file not yet received Document will be held at current stage"
                    '            Me.stage_id_to = ldoc.original_stage
                    '            Me.stage_name_to = ldoc.original_stage_name
                    '            ldoc.stage = ldoc.original_stage
                    '            ldoc.stage_name = ldoc.original_stage_name
                    '            suppress = True
                    '            run_system_event = True
                    '            Dim omessage As New bc_cs_message("Blue Curve", history, bc_cs_message.MESSAGE)
                    '        End If
                    '    End If
                    'ElseIf action_name = "Archive Units" Then
                    '    Dim obc_archive As New bc_am_archive_units(CStr(ldoc.id))
                    '    run_system_event = obc_archive.run_archive
                    'ElseIf action_name = "Distribution List" Then
                    '    Dim obc_distribution As New bc_am_distribution(CStr(ldoc.id), 1)
                    '    run_system_event = obc_distribution.run
                    'ElseIf action_name = "Distribute" Then
                    '    Dim obc_distribution As New bc_am_distribution(CStr(ldoc.id), 2)
                    '    run_system_event = obc_distribution.run
                    'ElseIf action_name = "Revert from PDF" Then
                    '    Dim obc_distribution As New bc_am_pdf_revert(CStr(ldoc.id))
                    '    run_system_event = obc_distribution.run
                    'ElseIf action_name = "Check Distribution List" Then
                    '    Dim obc_dist_check As New bc_am_check_dist_list(CStr(ldoc.id))
                    '    Me.suppress_no_trans = True
                    '    run_system_event = obc_dist_check.check
                    'ElseIf action_name = "Publish Document" Then
                    '    Dim obc_publish_document As New bc_am_publish_document(CStr(ldoc.id), True)
                    '    run_system_event = obc_publish_document.run
            ElseIf action_name = "Unpublish Document" Then
                    Dim obc_publish_document As New bc_am_publish_document(CStr(ldoc.id), False)
                    run_system_event = obc_publish_document.run
                    'ElseIf action_name = "Publish Data" Then
                    '    Dim obc_publish_data As New bc_am_publish_data(CStr(ldoc.id), ldoc.entity_id)
                    '    run_system_event = obc_publish_data.run
                    'ElseIf action_name = "Initial Publish Data" Then
                    '    Dim obc_init_publish_data As New bc_am_initial_publish_data(CStr(ldoc.id), ldoc.entity_id)
                    '    run_system_event = obc_init_publish_data.run
                    '    suppress_no_trans = True
                    'ElseIf action_name = "Email Notify" Then
                    '    Dim obc_email_notify As New bc_am_email_notify(CStr(ldoc.id), ldoc.stage, False, False)
                    '    run_system_event = obc_email_notify.run
                    'ElseIf action_name = "Email Notify Author Only" Then
                    '    Dim obc_email_notify As New bc_am_email_notify(CStr(ldoc.id), ldoc.stage, True, False)
                    '    run_system_event = obc_email_notify.run
                    REM deprecated please us custom server side
                    'ElseIf action_name = "Email Notify Server Side" Then
                    '    Dim obc_email_notify As New bc_am_email_notify(CStr(ldoc.id), ldoc.stage, False, True)
                    '    run_system_event = obc_email_notify.run
                    '    REM server side event now
                    '    Dim osse As New bc_om_document.bc_om_server_side_event
                    '    osse.sql = obc_email_notify.sql
                    '    osse.name = action_name
                    '    ldoc.server_side_events.events.Add(osse)
                    '    server_side_event = True
                    'ElseIf action_name = "Check Disclosures Assigned" Then
                    '    REM server side event now
                    '    Dim osse As New bc_om_document.bc_om_server_side_event
                    '    osse.sql = "exec dbo.bcc_core_wf_check_disc_ass " + CStr(ldoc.id)
                    '    osse.name = action_name
                    '    ldoc.server_side_events.events.Add(osse)
                    '    server_side_event = True
                    '    run_system_event = True
                    '    REM deprecated please us custom server side
                    'ElseIf action_name = "Email Notify Author Only Server Side" Then
                    '    Dim obc_email_notify As New bc_am_email_notify(CStr(ldoc.id), ldoc.stage, True, True)
                    '    run_system_event = obc_email_notify.run
                    '    Dim osse As New bc_om_document.bc_om_server_side_event
                    '    osse.sql = obc_email_notify.sql
                    '    osse.name = action_name

                    '    ldoc.server_side_events.events.Add(osse)
                    '    server_side_event = True
                    'ElseIf action_name = "First Call Note" Then
                    '    Dim obc_first_call_note As New bc_am_first_call_note(CStr(ldoc.id), ldoc.stage)
                    '    run_system_event = obc_first_call_note.run
                    'ElseIf action_name = "QA" Then
                    '    ldoc.qa = True
                    '    run_system_event = True
                    'replaced with server side event now so use this
                    'ElseIf action_name = "Conflict Check" Or action_name = "Conflict Check With Email" Then
                    '   Dim oconflict As bc_am_conflict_check


                    '   If action_name = "Conflict Check With Email" Then
                    '       oconflict = New bc_am_conflict_check(ldoc.id, True)
                    '   Else
                    '       oconflict = New bc_am_conflict_check(ldoc.id, False)
                    '   End If
                    '   If oconflict.check_for_conflicts = False Then
                    '       REM event itself has failed so indicate not to change stage
                    '       run_system_event = False
                    '   Else
                    '       run_system_event = True
                    '       REM copy history over
                    '       If oconflict.has_conflicts = True Then
                    '           REM if conflict override stage move to conflict stage
                    '           ldoc.stage = oconflict.conflict_stage_id
                    '           Me.stage_id_to = oconflict.conflict_stage_id
                    '           Me.stage_name_to = oconflict.conflict_stage_name
                    '           ldoc.history.Add("Document has Conflicts so will move to stage:" + oconflict.conflict_stage_name)
                    '           suppress = True
                    '           REM email notity bit
                    '           Dim omessage As New bc_cs_message("Blue Curve", "Document has conflicts with be in stage: " + oconflict.conflict_stage_name, bc_cs_message.MESSAGE)
                    '       Else
                    '           ldoc.history.Add("Document has no Conflicts")
                    '       End If
                    '   End If

                    'ElseIf action_name = "Check Under Embargo" Then
                    '    Dim obc_embargo As New bc_am_embargo_check(ldoc.id, ldoc.stage)
                    '    run_system_event = obc_embargo.check
                    '    If run_system_event = True Then
                    '        If obc_embargo.stage_out = 0 Then
                    '            run_system_event = False
                    '        Else
                    '            Dim res As String
                    '            res = obc_embargo.result
                    '            ldoc.history.Add(res)
                    '            If obc_embargo.stage_out <> 0 Then
                    '                REM set stage of document depening on embargo or not
                    '                ldoc.stage = obc_embargo.stage_out
                    '                override_stage = obc_embargo.stage_out
                    '                ldoc.stage_name = obc_embargo.stage_out_name
                    '                Me.stage_id_to = obc_embargo.stage_out
                    '                Me.stage_name_to = obc_embargo.stage_out_name
                    '                If obc_embargo.res = "Fail" Then
                    '                    Dim omessage As New bc_cs_message("Blue Curve", "Document has companies under embargo will be in stage: " + obc_embargo.stage_out_name, bc_cs_message.MESSAGE)
                    '                End If
                    '            End If
                    'End If
                    '    End If
                    'ElseIf action_name = "Check Restrictions" Then
                    '    Dim obc_res As New bc_am_check_restrictions(ldoc.id, ldoc.stage, ldoc.entity_id)
                    '    run_system_event = obc_res.check
                    '    If run_system_event = True Then
                    '        If obc_res.mode = 0 Then
                    '            REM do nothing
                    '            history = "No Restrictions"
                    '            run_system_event = True
                    '        Else
                    '            Dim res As String
                    '            res = obc_res.result
                    '            If obc_res.mode = 1 Then
                    '                REM set stage of document depening on embargo or not

                    '                Dim k As Boolean
                    '                k = False
                    '                If k = True Then
                    '                    REM now refresh document
                    '                    If IsNothing(obj) Then
                    '                        cont = check_for_open_doc(open_doc, obj, render_only)
                    '                        If cont = False Then
                    '                            history = "Event: " + action_name + " surpressed as cant operate on mime type: " + ldoc.extension
                    '                            run_system_event = True
                    '                            Exit Function
                    '                        ElseIf render_only = True Then
                    '                            If action_name <> "Render" And action_name <> "Render and Publish" Then
                    '                                REM if action is not a render suppress
                    '                                history = "Event: " + action_name + " surpressed as cant operate on imported mime type: " + ldoc.extension
                    '                                run_system_event = True
                    '                                Exit Function
                    '                            End If
                    '                        End If
                    '                    Else
                    '                        ldoc.docobject = obj
                    '                    End If
                    '                End If
                    '                REM if stage not zero change route
                    '                If obc_res.stage_out > 0 Then
                    '                    Dim omessage As New bc_cs_message("Blue Curve", "Document has Flag(s) will be moved to  stage: " + obc_res.stage_out_name, bc_cs_message.MESSAGE)
                    '                    suppress = True
                    '                    ldoc.stage = obc_res.stage_out
                    '                    ldoc.stage_name = obc_res.stage_out_name
                    '                    Me.stage_id_to = obc_res.stage_out
                    '                    Me.stage_name_to = obc_res.stage_out_name
                    '                    ldoc.history.Add("Restriction(s): " + obc_res.res_tx)
                    '                    ldoc.history.Add("Document has Restrictions will be moved to  stage: " + obc_res.stage_out_name)
                    '                End If
                    '                ldoc.write_data_to_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ".dat")
                    '                If k = True Then
                    '                    obc_refresh = New bc_am_at_component_refresh(False, obj, "word", 1, False)
                    '                    Dim j As Integer
                    '                    For j = 0 To ldoc.refresh_components.refresh_components.Count - 1
                    '                        If ldoc.refresh_components.refresh_components(j).refresh_type = 2 Then
                    '                            ldoc.refresh_components.refresh_components(j).disabled = False
                    '                            ldoc.refresh_components.refresh_components(j).no_refresh = False
                    '                        End If
                    '                    Next
                    '                    ldoc.write_data_to_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ".dat")
                    '                    For i = 0 To ldoc.refresh_components.refresh_components.Count - 1
                    '                        If ldoc.refresh_components.refresh_components(i).refresh_type = 2 Then
                    '                            If CDate(ldoc.refresh_components.refresh_components(i).last_refresh_date) <= Now And ldoc.refresh_components.refresh_components(i).last_refresh_date <> "9-9-9999" Then
                    '                                ocommentary = New bc_cs_activity_log("bc_am_copy_doc", "create_new_doc", bc_cs_activity_codes.COMMENTARY, "Refreshing Disclosures in translated document")
                    '                                bc_cs_central_settings.progress_bar.increment("Refreshing Disclosures")
                    '                                obc_refresh = New bc_am_at_component_refresh(ldoc.docobject, "word")
                    '                                obc_refresh.refresh(False, 2, False, ldoc, True)
                    '                            Else
                    '                                ocommentary = New bc_cs_activity_log("bc_am_copy_doc", "create_new_doc", bc_cs_activity_codes.COMMENTARY, "Disclosures not refreshed in original")
                    '                            End If
                    '                            Exit For
                    '                        End If
                    '                    Next
                    '                    ldoc = ldoc.read_data_from_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ".dat")
                    '                End If
                    '                ldoc.write_data_to_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ".dat")
                    '                run_system_event = True
                    '            End If
                    '        End If
                    '    End If
                    'ElseIf action_name = "US Flag" Then
                    '    Dim obc_us As New bc_am_us_flag(ldoc.entity_id)
                    '    run_system_event = obc_us.check
                    '    If run_system_event = True Then
                    '        If obc_us.flag = 0 Then
                    '            REM do nothing
                    '            history = "No US flag"
                    '            run_system_event = True
                    '        Else
                    '            suppress = True
                    '            history = "US Flag US copy created"
                    '            Dim omessage As New bc_cs_message("Blue Curve", "Document has US Flag will be moved to  stage: " + obc_us.stage_name_to + " and US copy created", bc_cs_message.MESSAGE)
                    '            suppress = True

                    '            ldoc.history.Add("Document has US Flag will be moved to  stage: " + obc_us.stage_name_to)
                    '            REM open document
                    '            If IsNothing(obj) Then
                    '                cont = check_for_open_doc(open_doc, obj, render_only)
                    '                If cont = False Then
                    '                    history = "Event: " + action_name + " surpressed as cant operate on mime type: " + ldoc.extension
                    '                    run_system_event = True
                    '                    Exit Function
                    '                ElseIf render_only = True Then
                    '                    If action_name <> "Render" And action_name <> "Render and Publish" Then
                    '                        REM if action is not a render suppress
                    '                        history = "Event: " + action_name + " surpressed as cant operate on imported mime type: " + ldoc.extension
                    '                        run_system_event = True
                    '                        Exit Function
                    '                    End If
                    '                End If
                    '            Else
                    '                ldoc.docobject = obj
                    '            End If
                    '            Dim ocopy As New bc_am_copy_doc(ldoc.id, obj, True, True)
                    '            ocopy.create_new_doc(obc_us.pub_type_new, ldoc.language_id, obj, ldoc, True, obc_us.stage_id_to, False, 0)
                    '            ldoc.stage = obc_us.stage_id_to
                    '            ldoc.stage_name = obc_us.stage_name_to
                    '            Me.stage_id_to = obc_us.stage_id_to
                    '            Me.stage_name_to = obc_us.stage_name_to
                    '            If ocopy.success = True Then
                    '                run_system_event = True
                    '            End If
                    '            suppress = True
                    '        End If

                    '    End If
            ElseIf action_name = "Componetize Powerpoint" Then
                    If IsNothing(obj) Then
                        cont = check_for_open_doc(ldoc, open_doc, obj, render_only, True)

                        If cont = False Then
                            Dim nbcr = New bc_am_pp_componetize

                            If nbcr.componetize_non_bc(ldoc) = True Then
                                run_system_event = True
                                Exit Function
                            Else
                                Dim ocomm As New bc_cs_activity_log("bc_am_workflow_event_handler", "Componetize Powerpoint", bc_cs_activity_codes.COMMENTARY, nbcr.err_text)
                                Exit Function
                            End If
                        End If
                    Else
                        ldoc.docobject = obj
                    End If


            Else
                    run_system_event = False
                    ocommentary = New bc_cs_activity_log("bc_am_workflow_events_handler", "run_event", bc_cs_activity_codes.COMMENTARY, "System Event " + action_name + " not supported.")
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_workflow_events_handler", "run_system_event", bc_cs_error_codes.USER_DEFINED, ex.Message)
            run_system_event = False
        Finally
            ldoc.last_revision_filename = last_revision_filename


            slog = New bc_cs_activity_log("bc_am_workflow_events_handler", "run_system_event", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
    Public Sub close()
        Try
            odoc.close()
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_workflow_events_handler", "close", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Private Function check_for_open_doc(ByRef ldoc As bc_om_document, ByVal open_doc As Boolean, ByRef outdoc As Object, ByRef render_only As Boolean, Optional ByVal render_event As Boolean = False, Optional html_comp As Boolean = False) As Boolean
        Dim slog = New bc_cs_activity_log("check_for_open_doc", "open_doc", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            Dim ocommentary As bc_cs_activity_log
            render_only = False
            check_for_open_doc = True
            REM check mime type can support open doc
            check_for_open_doc = True
            Dim omime As New bc_am_mime_types
            Dim imp As Boolean = False

            If omime.can_edit(ldoc.extension, render_only) = False Then
                If html_comp = False Then
                    check_for_open_doc = False
                    Exit Function

                ElseIf ldoc.extension <> "[imp].docx" And ldoc.extension <> "[imp].doc" And ldoc.extension <> "[imp].docm" Then
                    check_for_open_doc = False
                    Exit Function
                ElseIf ldoc.extension = "[imp].docx" Or ldoc.extension = "[imp].doc" Or ldoc.extension = "[imp].docm" Then
                    imp = True
                End If
            End If
            If render_only = True And render_event = False And html_comp = False Then
                Exit Function
            End If
            REM if document is not open  
            REM open it as events requires it
            If Me.doc_open = False And open_doc = False Then

                ocommentary = New bc_cs_activity_log("bc_am_workflow_events_handler", "run_system_event", bc_cs_activity_codes.COMMENTARY, "Event requires an Open docuent")
                'Dim obc_workflow As New bc_am_workflow

                Dim phist As New ArrayList
                For i = 0 To ldoc.history.Count - 1
                    phist.Add(ldoc.history(i))
                Next
                ldoc.history.Clear()
                'obc_workflow.set_server_status("Checking out document from server")
                REM update metadata
                ldoc.connection_method = bc_cs_central_settings.selected_conn_method
                REM ldoc.write_xml_to_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ldoc.extension)
                REM XXXXXX

                If odoc.open(False, ldoc, False, new_instance, imp) = False Then
                    Exit Function
                End If
                ldoc.history.Clear()
                ldoc.support_documents.Clear()
                For i = 0 To phist.Count - 1
                    ldoc.history.Add(phist(i))
                Next


                Me.obj = ldoc.docobject
                Me.doc_open = True
                REM reset stage transition
                ldoc.stage = Me.stage_id_to
                ldoc.stage_name = Me.stage_name_to
                ldoc.write_data_to_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ".dat")
                outdoc = ldoc.docobject
                last_revision_filename = ldoc.last_revision_filename


            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("check_for_open_doc", "open_doc", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("check_for_open_doc", "open_doc", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try


    End Function
    Private Function check_for_open_doc_excel(ByRef ldoc As bc_om_document, ByVal open_doc As Boolean, ByRef outdoc As Object, ByRef oexcel As bc_ao_excel) As Boolean
        Dim slog = New bc_cs_activity_log("check_for_open_doc_excel", "open_doc", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            Dim ocommentary As bc_cs_activity_log
            check_for_open_doc_excel = True
            REM check mime type can support open doc
            check_for_open_doc_excel = True
            Dim omime As New bc_am_mime_types
            If ldoc.extension <> "[imp].xls" And ldoc.extension <> "[imp].xlsx" And ldoc.extension <> "[imp].xlsm" And ldoc.extension <> ".xlsx" And ldoc.extension <> ".xlsm" Then
                Exit Function
            End If

            REM if document is not open  
            REM open it as events requires it
            If Me.doc_open = False And open_doc = False Then

                ocommentary = New bc_cs_activity_log("bc_am_workflow_events_handler", "run_system_event", bc_cs_activity_codes.COMMENTARY, "Event requires an Open exceldocuent")
                'Dim obc_workflow As New bc_am_workflow

                Dim phist As New ArrayList
                For i = 0 To ldoc.history.Count - 1
                    phist.Add(ldoc.history(i))
                Next
                ldoc.history.Clear()
                'obc_workflow.set_server_status("Checking out document from server")
                REM update metadata
                ldoc.connection_method = bc_cs_central_settings.selected_conn_method
                REM ldoc.write_xml_to_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ldoc.extension)
                REM XXXXXX
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    ldoc.db_check_out_non_create_doc()
                Else
                    ldoc.tmode = bc_cs_soap_base_class.tREAD
                    ldoc.read_mode = bc_om_document.CHECK_OUT_NON_CREATE_DOC
                    If ldoc.transmit_to_server_and_receive(ldoc, True) = False Then
                        Exit Function
                    End If
                End If
                Dim fs As New bc_cs_file_transfer_services
                If fs.write_bytestream_to_document(bc_cs_central_settings.local_repos_path + ldoc.filename, ldoc.byteDoc, Nothing, True) = False Then
                    Exit Function
                End If

                ldoc.history.Clear()
                ldoc.support_documents.Clear()
                For i = 0 To phist.Count - 1
                    ldoc.history.Add(phist(i))
                Next


                oexcel.open(bc_cs_central_settings.local_repos_path + ldoc.filename, True, False)

                ldoc.docobject = oexcel.docobject

                'ldoc.docobject = oexcel.docobject

                Me.doc_open = True
                REM reset stage transition
                ldoc.stage = Me.stage_id_to
                ldoc.stage_name = Me.stage_name_to
                ldoc.write_data_to_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ".dat")
                outdoc = ldoc.docobject
                last_revision_filename = ldoc.last_revision_filename


            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("check_for_open_doc_excel", "open_doc", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("check_for_open_doc_excel", "open_doc", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try


    End Function
    Private Function check_for_open_doc_powerpoint(ByRef ldoc As bc_om_document, ByVal open_doc As Boolean, ByRef outdoc As Object, ByRef render_only As Boolean, Optional ByVal render_event As Boolean = False, Optional html_comp As Boolean = False) As Boolean
        Dim slog = New bc_cs_activity_log("check_for_open_doc_powerpoint", "open_doc", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            Dim ocommentary As bc_cs_activity_log
            render_only = False
            check_for_open_doc_powerpoint = True
            REM check mime type can support open doc

            Dim imp As Boolean = False

          
            REM if document is not open  
            REM open it as events requires it
            If Me.doc_open = False And open_doc = False Then

                ocommentary = New bc_cs_activity_log("bc_am_workflow_events_handler", "run_system_event", bc_cs_activity_codes.COMMENTARY, "Event requires an Open docuent")
                'Dim obc_workflow As New bc_am_workflow

                Dim phist As New ArrayList
                For i = 0 To ldoc.history.Count - 1
                    phist.Add(ldoc.history(i))
                Next
                ldoc.history.Clear()
                'obc_workflow.set_server_status("Checking out document from server")
                REM update metadata
                ldoc.connection_method = bc_cs_central_settings.selected_conn_method
                REM ldoc.write_xml_to_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ldoc.extension)
                REM XXXXXX

                If odoc.open(False, ldoc, False, new_instance, imp) = False Then
                    check_for_open_doc_powerpoint = False
                    Exit Function
                End If
                ldoc.history.Clear()
                ldoc.support_documents.Clear()
                For i = 0 To phist.Count - 1
                    ldoc.history.Add(phist(i))
                Next


                Me.obj = ldoc.docobject
                Me.doc_open = True
                REM reset stage transition
                ldoc.stage = Me.stage_id_to
                ldoc.stage_name = Me.stage_name_to
                ldoc.write_data_to_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ".dat")
                outdoc = ldoc.docobject
                last_revision_filename = ldoc.last_revision_filename


            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("check_for_open_doc_powerpoint", "open_doc", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("check_for_open_doc_powerpoint", "open_doc", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try


    End Function

    Private Sub check_for_doc_close(ByVal open_doc As Boolean)
        REM if document is open and was not original open
        REM close it
        If Me.doc_open = True And open_doc = False Then

        End If


    End Sub

End Class
'Public Class bc_am_render_to_pdf
'    Public doc As Object
'    Private doc_id As String
'    Private timeout As Integer
'    Private printer As String
'    Private extension As String
'    Public success As Boolean
'    Private fn As String
'    Private ofn As String
'    Private wait As Boolean = True
'    Private finish As Boolean = False
'    Private open As Boolean = True
'    Private odoc As bc_om_document
'    Dim WithEvents fsw As New FileSystemWatcher
'    REM constructor if document is not yet open
'    Public Sub New(ByVal show_form As Boolean, ByVal doc As Object, ByVal application As String, ByVal printer As String, ByVal extension As String, ByVal timeout As Integer, ByVal doc_id As String)
'        Me.doc = doc
'        Me.doc_id = doc_id
'        Me.printer = printer
'        Me.extension = extension
'        Me.timeout = timeout
'        REM flag to determine whether to register pdf in system out of submission process
'        Me.open = True
'        success = False
'    End Sub
'    REM constructor if document is already open
'    Public Sub New(ByVal doc_id As String, ByVal printer As String, ByVal timeout As Integer, ByVal ldoc As bc_om_document)
'        Me.printer = printer
'        Me.extension = extension
'        Me.timeout = timeout
'        Me.doc_id = doc_id
'        REM flag to determine whether to register pdf in system out of submission process
'        Me.open = False
'        success = False
'    End Sub
'    Public Function rendertoPDF(ByVal ldoc As bc_om_document) As Boolean
'        Dim slog = New bc_cs_activity_log("bc_am_render_to_pdf", "rendertoPDF", bc_cs_activity_codes.TRACE_ENTRY, "")
'        Dim ocommentary As bc_cs_activity_log

'        Dim wordDocument As Object
'        wordDocument = Nothing

'        Dim paramExportFormat As Integer
'        paramExportFormat = 17 'WdExportFormat.wdExportFormatPDF
'        Dim paramOpenAfterExport As Boolean = False
'        Dim paramExportOptimizeFor As Integer
'        paramExportOptimizeFor = 0 'WdExportOptimizeFor.wdExportOptimizeForPrint
'        Dim paramExportRange As Integer
'        paramExportRange = 0 'WdExportRange.wdExportAllDocument
'        Dim paramStartPage As Int32 = 0
'        Dim paramEndPage As Int32 = 0
'        Dim paramExportItem As Integer
'        paramExportItem = 0 'WdExportItem.wdExportDocumentContent
'        Dim paramIncludeDocProps As Boolean = True
'        Dim paramKeepIRM As Boolean = True
'        Dim paramCreateBookmarks As Integer
'        paramCreateBookmarks = 1 'WdExportCreateBookmarks.wdExportCreateWordBookmarks

'        Dim paramDocStructureTags As Boolean = False
'        Dim paramBitmapMissingFonts As Boolean = True
'        Dim paramUseISO19005_1 As Boolean = False

'        Dim i As Integer
'        Dim extensionsize As Integer

'        Try

'            If IsNothing(Me.doc) Then
'                Exit Function
'            End If

'            Me.success = False
'            If open = False Then
'                bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO
'                ocommentary = New bc_cs_activity_log("bc_am_render_to_pdf", "rendertoPDF", bc_cs_activity_codes.COMMENTARY, "Attempting to Open Document prior to Render")
'                REM recreate document object
'                ldoc.id = Me.doc_id
'                Dim oopendoc As New bc_am_at_open_doc
'                oopendoc.open(False, ldoc, False)
'                REM assign active document across
'                Me.doc = ldoc.docobject
'            End If

'            Dim lfn As String
'            Dim dfn As String = ""

'            REM SW cope with office versions
'            extensionsize = 0
'            extensionsize = (Len(ldoc.filename) - (InStrRev(ldoc.filename, ".") - 1))

'            If Right(ldoc.filename, extensionsize) = Right(ldoc.extension, extensionsize) Then
'                fn = Left(ldoc.filename, Len(ldoc.filename) - extensionsize) + ".pdf"
'            Else
'                fn = ldoc.filename + ".pdf"
'            End If
'            lfn = Left(fn, Len(fn) - 4) + ldoc.extension
'            Dim origfn As String
'            Dim use_temp_file As String
'            use_temp_file = False
'            origfn = fn
'            fn = Left(fn, Len(fn) - 4)
'            If InStr(fn, ".") > 0 Then
'                use_temp_file = True
'                fn = Replace(fn, ".", "", 1)
'                ldoc.docobject.saveas(bc_cs_central_settings.local_repos_path + fn + ldoc.extension)
'                dfn = bc_cs_central_settings.local_repos_path + fn + ldoc.extension
'            End If

'            fn = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + "\" + fn + ".pdf"

'            wordDocument = Me.doc

'            Try
'                wordDocument.ExportAsFixedFormat(fn, paramExportFormat, paramOpenAfterExport, _
'                    paramExportOptimizeFor, paramExportRange, paramStartPage, paramEndPage, paramExportItem, paramIncludeDocProps, paramKeepIRM, _
'                    paramCreateBookmarks, paramDocStructureTags, paramBitmapMissingFonts, paramUseISO19005_1)
'            Catch
'                success = False
'                Exit Function
'            End Try

'            REM file will end up in aplication data area so wait for it to arrive then copy
'            REM to local repos

'            ofn = bc_cs_central_settings.local_repos_path + origfn

'            i = 0
'            REM see if file exists

'            While i < 30 And Me.success = False
'                Thread.Sleep(1000)
'                REM check every second for up 30 seconds
'                REM for file
'                Dim fs As New bc_cs_file_transfer_services

'                If fs.check_document_exists(fn) = True Then
'                    REM copy over
'                    fs.file_copy(fn, ofn)
'                    REM remove
'                    fs.delete_file(fn)
'                    Me.success = True
'                End If
'                i = i + 1
'            End While
'            REM save original back
'            If use_temp_file = True Then
'                ldoc.docobject.saveas(bc_cs_central_settings.local_repos_path + lfn)
'                Dim fs As New bc_cs_file_transfer_services
'                fs.delete_file(dfn)
'            End If
'            If Me.success = False Then
'                ocommentary = New bc_cs_activity_log("bc_am_render_to_pdf", "render", bc_cs_activity_codes.COMMENTARY, "Render Timed out.")
'                Dim omessage As New bc_cs_message("Blue Curve - create", "Render timed out.", bc_cs_message.MESSAGE)
'            End If
'            'If open = False Then
'            '    ocommentary = New bc_cs_activity_log("bc_am_render_to_pdf", "render", bc_cs_activity_codes.COMMENTARY, "Attempting to Submit document post Render")
'            '    Dim osubmit As New bc_am_at_submit(False, doc, "word", False)
'            'End If

'        Catch ex As Exception
'            ocommentary = New bc_cs_activity_log("bc_am_render_to_pdf", "rendertoPDF", bc_cs_activity_codes.COMMENTARY, ex.Message)
'            Dim db_err As New bc_cs_error_log("bc_am_render_to_pdf", "rendertoPDF", "run_system_event", bc_cs_error_codes.USER_DEFINED, ex.Message)
'        Finally

'            GC.Collect()
'            GC.WaitForPendingFinalizers()
'            GC.Collect()
'            GC.WaitForPendingFinalizers()

'            slog = New bc_cs_activity_log("bc_am_render_to_pdf", "rendertoPDF", bc_cs_activity_codes.TRACE_EXIT, "")
'        End Try

'    End Function

'    Public Function render(ByVal ldoc As bc_om_document) As Boolean
'        Dim slog = New bc_cs_activity_log("bc_am_render_to_pdf", "render", bc_cs_activity_codes.TRACE_ENTRY, "")
'        Dim ocommentary As bc_cs_activity_log
'        Dim default_printer As String = ""

'        Dim i As Integer
'        Dim extensionsize As Integer

'        Try

'            If IsNothing(Me.doc) Then
'                Exit Function
'            End If

'            Me.success = False
'            If open = False Then
'                bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO
'                ocommentary = New bc_cs_activity_log("bc_am_render_to_pdf", "render", bc_cs_activity_codes.COMMENTARY, "Attempting to Open Document prior to Render")
'                REM recreate document object
'                ldoc.id = Me.doc_id
'                Dim oopendoc As New bc_am_at_open_doc
'                oopendoc.open(False, ldoc, False)
'                REM assign active document across
'                Me.doc = ldoc.docobject
'            End If

'            Dim lfn As String
'            Dim dfn As String = ""

'            Try
'                default_printer = doc.application.activeprinter
'                doc.application.activeprinter = Me.printer
'            Catch
'                success = False
'                Exit Function
'            End Try

'            REM SW cope with office versions
'            extensionsize = 0
'            extensionsize = (Len(ldoc.filename) - (InStrRev(ldoc.filename, ".") - 1))

'            If Right(ldoc.filename, extensionsize) = Right(ldoc.extension, extensionsize) Then
'                fn = Left(ldoc.filename, Len(ldoc.filename) - extensionsize) + ".pdf"
'            Else
'                fn = ldoc.filename + ".pdf"
'            End If
'            lfn = Left(fn, Len(fn) - 4) + ldoc.extension
'            Dim origfn As String
'            Dim use_temp_file As String
'            use_temp_file = False
'            origfn = fn
'            fn = Left(fn, Len(fn) - 4)
'            If InStr(fn, ".") > 0 Then
'                use_temp_file = True
'                fn = Replace(fn, ".", "", 1)
'                ldoc.docobject.saveas(bc_cs_central_settings.local_repos_path + fn + ldoc.extension)
'                dfn = bc_cs_central_settings.local_repos_path + fn + ldoc.extension
'            End If

'            REM file will end up in aplication data area so wait for it to arrive then copy
'            REM to local repos

'            dothetask()

'            ofn = bc_cs_central_settings.local_repos_path + origfn
'            REM if filename has . in it remove temporaily


'            i = 0
'            REM see if file exists
'            fn = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + "\" + fn + ".pdf"

'            While i < 30 And Me.success = False
'                Thread.Sleep(1000)
'                REM check every second for up 30 seconds
'                REM for file
'                Dim fs As New bc_cs_file_transfer_services

'                If fs.check_document_exists(fn) = True Then
'                    REM copy over
'                    fs.file_copy(fn, ofn)
'                    REM remove
'                    fs.delete_file(fn)
'                    Me.success = True
'                End If
'                i = i + 1
'            End While
'            REM save original back
'            If use_temp_file = True Then
'                ldoc.docobject.saveas(bc_cs_central_settings.local_repos_path + lfn)
'                Dim fs As New bc_cs_file_transfer_services
'                fs.delete_file(dfn)
'            End If
'            If Me.success = False Then
'                ocommentary = New bc_cs_activity_log("bc_am_render_to_pdf", "render", bc_cs_activity_codes.COMMENTARY, "Render Timed out.")
'                Dim omessage As New bc_cs_message("Blue Curve - create", "Render timed out.", bc_cs_message.MESSAGE)
'            End If
'            'If open = False Then
'            '    doc.application.activeprinter = default_printer
'            '    ocommentary = New bc_cs_activity_log("bc_am_render_to_pdf", "render", bc_cs_activity_codes.COMMENTARY, "Attempting to Submit document post Render")
'            '    Dim osubmit As New bc_am_at_submit(False, doc, "word", False)
'            'End If
'        Catch ex As Exception
'            ocommentary = New bc_cs_activity_log("bc_am_render_to_pdf", "render", bc_cs_activity_codes.COMMENTARY, ex.Message)
'            Dim db_err As New bc_cs_error_log("bc_am_render_to_pdf", "render", "run_system_event", bc_cs_error_codes.USER_DEFINED, ex.Message)
'        Finally
'            If open = True Then
'                doc.application.activeprinter = default_printer
'            End If
'            slog = New bc_cs_activity_log("bc_am_render_to_pdf", "render", bc_cs_activity_codes.TRACE_EXIT, "")
'        End Try

'    End Function
'    Public Sub dothetask()
'        Dim slog = New bc_cs_activity_log("bc_am_render_to_pdf", "dothetask", bc_cs_activity_codes.TRACE_ENTRY, "")
'        Try
'            doc.Application.PrintOut(FileName:="", Range:=0, Item:=0, _
'            Copies:=1, Pages:="", PageType:=0, _
'            ManualDuplexPrint:=False, Collate:=True, Background:=False, PrintToFile:= _
'            False, PrintZoomColumn:=0, PrintZoomRow:=0, PrintZoomPaperWidth:=0, _
'            PrintZoomPaperHeight:=0)

'            REM if not
'        Catch ex As Exception
'            Dim db_err As New bc_cs_error_log("bc_am_render_to_pdf", "dothetask", "run_system_event", bc_cs_error_codes.USER_DEFINED, ex.Message)
'        Finally
'            slog = New bc_cs_activity_log("bc_am_render_to_pdf", "dothetask", bc_cs_activity_codes.TRACE_EXIT, "")
'        End Try


'    End Sub
'    Public Sub dothetaskpdfmaker()

'        Dim slog = New bc_cs_activity_log("bc_am_render_to_pdf", "dothetaskpdfmaker", bc_cs_activity_codes.TRACE_ENTRY, "")
'        Dim found As Boolean = False
'        Try
'            For i = 1 To doc.Application.CommandBars.Count

'                If InStr(1, doc.Application.CommandBars(i).Name, "Acrobat PDFMaker") > 0 Then
'                    doc.Application.CommandBars(i).Controls(1).Execute()
'                    found = True
'                    Exit For
'                End If
'            Next
'            If found = False Then
'                Dim ocomm As New bc_cs_activity_log("bc_am_render_to_pdf", "dothetaskpdfmaker", bc_cs_activity_codes.COMMENTARY, "PDFMaker not installed")
'            End If
'            REM if not
'        Catch ex As Exception
'            Dim db_err As New bc_cs_error_log("bc_am_render_to_pdf", "dothetaskpdfmaker", "run_system_event", bc_cs_error_codes.USER_DEFINED, ex.Message)
'        Finally

'            slog = New bc_cs_activity_log("bc_am_render_to_pdf", "dothetaskpdfmaker", bc_cs_activity_codes.TRACE_EXIT, "")
'        End Try


'    End Sub
'Public Function render_with_pdfmaker(ByVal ldoc As bc_om_document) As Boolean
'    Dim slog = New bc_cs_activity_log("bc_am_render_to_pdf", "render", bc_cs_activity_codes.TRACE_ENTRY, "")
'    Dim ocommentary As bc_cs_activity_log
'    Dim default_printer As String = ""

'    Dim i As Integer
'    Dim extensionsize As Integer

'    Try

'        If IsNothing(Me.doc) Then
'            Exit Function
'        End If

'        Me.success = False
'        If open = False Then
'            bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO
'            ocommentary = New bc_cs_activity_log("bc_am_render_to_pdf", "render", bc_cs_activity_codes.COMMENTARY, "Attempting to Open Document prior to Render")
'            REM recreate document object
'            ldoc.id = Me.doc_id
'            Dim oopendoc As New bc_am_at_open_doc
'            oopendoc.open(False, ldoc, False)
'            REM assign active document across
'            Me.doc = ldoc.docobject
'        End If

'        Dim lfn As String
'        Dim dfn As String = ""


'        REM SW cope with office versions
'        extensionsize = 0
'        extensionsize = (Len(ldoc.filename) - (InStrRev(ldoc.filename, ".") - 1))

'        If Right(ldoc.filename, extensionsize) = Right(ldoc.extension, extensionsize) Then
'            fn = Left(ldoc.filename, Len(ldoc.filename) - extensionsize) + ".pdf"
'        Else
'            fn = ldoc.filename + ".pdf"
'        End If
'        lfn = Left(fn, Len(fn) - 4) + ldoc.extension
'        Dim origfn As String
'        Dim use_temp_file As String
'        use_temp_file = False
'        origfn = fn
'        fn = Left(fn, Len(fn) - 4)
'        If InStr(fn, ".") > 0 Then
'            use_temp_file = True
'            fn = Replace(fn, ".", "", 1)
'            ldoc.docobject.saveas(bc_cs_central_settings.local_repos_path + fn + ldoc.extension)
'            dfn = bc_cs_central_settings.local_repos_path + fn + ldoc.extension
'        End If

'        REM file will end up in aplication data area so wait for it to arrive then copy
'        REM to local repos

'        Dim found As Boolean
'        For i = 1 To doc.Application.CommandBars.Count
'            If InStr(1, doc.Application.CommandBars(i).Name, "Acrobat PDFMaker") > 0 Then
'                Application.DoEvents()
'                doc.Application.CommandBars(i).Controls(1).Execute()
'                found = True
'                Exit For
'            End If
'        Next

'        Application.DoEvents()

'        'dothetaskpdfmaker()
'        If found = False Then
'            Dim ocomm As New bc_cs_activity_log("bc_am_render_to_pdf", "dothetaskpdfmaker", bc_cs_activity_codes.COMMENTARY, "PDFMaker not installed")
'            Exit Function
'        End If

'        ofn = bc_cs_central_settings.local_repos_path + origfn
'        REM if filename has . in it remove temporaily


'        i = 0

'        While i < 30 And Me.success = False
'            Thread.Sleep(1000)
'            REM check every second for up 30 seconds
'            REM for file
'            Dim fs As New bc_cs_file_transfer_services

'            If fs.check_document_exists(ofn) = True Then
'                Me.success = True
'            End If
'            i = i + 1
'        End While
'        REM save original back
'        If use_temp_file = True Then
'            ldoc.docobject.saveas(bc_cs_central_settings.local_repos_path + lfn)
'            Dim fs As New bc_cs_file_transfer_services
'            fs.delete_file(dfn)
'        End If
'        If Me.success = False Then
'            ocommentary = New bc_cs_activity_log("bc_am_render_to_pdf", "render", bc_cs_activity_codes.COMMENTARY, "Render Timed out.")
'            Dim omessage As New bc_cs_message("Blue Curve - create", "Render timed out.", bc_cs_message.MESSAGE)
'        End If
'    Catch ex As Exception
'        ocommentary = New bc_cs_activity_log("bc_am_render_to_pdf", "render", bc_cs_activity_codes.COMMENTARY, ex.Message)
'        Dim db_err As New bc_cs_error_log("bc_am_render_to_pdf", "render", "run_system_event", bc_cs_error_codes.USER_DEFINED, ex.Message)
'    Finally

'        slog = New bc_cs_activity_log("bc_am_render_to_pdf", "render", bc_cs_activity_codes.TRACE_EXIT, "")
'    End Try

'End Function
'End Class
REM archive units event
'Public Class bc_am_archive_units
'    Private doc_id As String
'    Public Sub New(ByVal doc_id As String)
'        Me.doc_id = doc_id
'    End Sub
'    Public Function run_archive() As Boolean
'        Dim slog = New bc_cs_activity_log("bc_am_archive_units", "run_archive", bc_cs_activity_codes.TRACE_ENTRY, "")
'        Try
'            Dim osql As New bc_om_sql("exec dbo.bcc_core_wf_archive_composite '" + CStr(doc_id) + "'")
'            run_archive = False
'            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
'                osql.tmode = bc_cs_soap_base_class.tREAD
'                osql.transmit_to_server_and_receive(osql, True)
'            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
'                osql.db_read()
'            End If
'            If osql.success = True Then
'                run_archive = True
'            Else
'                Dim ocommentary As New bc_cs_activity_log("bc_am_archive_units", "run_archive", bc_cs_activity_codes.COMMENTARY, "SQL Error: exec dbo.bcc_core_wf_archive_composite '" + CStr(doc_id) + "'")
'            End If

'        Catch ex As Exception
'            Dim db_err As New bc_cs_error_log("bc_am_archive_units", "run_archive", bc_cs_error_codes.USER_DEFINED, ex.Message)
'        Finally
'            slog = New bc_cs_activity_log("bc_am_archive_units", "run_archive", bc_cs_activity_codes.TRACE_EXIT, "")
'        End Try
'    End Function

'End Class
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
    Private refresh_level As Integer
    Private embargo_level As Integer
    Private check_duplicate As Boolean


    Public Sub New(ByVal doc_id As String, ByVal ao_object As Object, Optional ByVal close_doc As Boolean = False, Optional ByVal refresh As Boolean = True, Optional ByVal show_message As Boolean = False, Optional ByVal refresh_level As Integer = 1, Optional ByVal embargo_level As Integer = 0, Optional ByVal check_duplicate As Boolean = True)
        Me.doc_id = doc_id
        Me.ao_object = ao_object
        Me.close_doc = close_doc
        Me.refresh = refresh
        Me.refresh_level = refresh_level
        Me.embargo_level = embargo_level
        Me.show_message = show_message
        Me.check_duplicate = check_duplicate

    End Sub
    Public Function copy(ByVal ldoc As bc_om_document) As Boolean
        Dim slog = New bc_cs_activity_log("bc_am_copy_doc", "copy", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim fs As New bc_cs_file_transfer_services
            Dim oh As bc_om_history
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
                        oh = New bc_om_history
                        oh.comment = "No copy pub types for this pub type:" + CStr(ldoc.pub_type_id)
                        ldoc.history.Add(oh)
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
                oh = New bc_om_history
                oh.comment = "Document is already copied from document: " + ldoc.master_translated_doc + " cannot translate but will just refresh"
                ldoc.history.Add(oh)
                bc_cs_central_settings.progress_bar.increment("Refreshing Copied Document")
                Dim orefresh As New bc_am_at_component_refresh(ao_object, "word", False)
                orefresh.refresh(False, 1, False, ldoc, True)
                Exit Function
            End If
            If ldoc.sub_translated_docs <> "" Then
                If Me.show_message = True Then
                    omessage = New bc_cs_message("Blue Curve - create", "Document already has  copy(s): " + ldoc.sub_translated_docs + " can't copy again", bc_cs_message.MESSAGE)
                End If
                ocommentary = New bc_cs_activity_log("bc_am_copy_doc", "copy", bc_cs_activity_codes.TRACE_ENTRY, "Document already has  copy(s): " + ldoc.sub_translated_docs + " can't copy again")
                oh = New bc_om_history
                oh.comment = "Document already has  copy(s): " + ldoc.sub_translated_docs + " can't copy again"
                ldoc.history.Add(oh)
                Exit Function
            End If
            If ldoc.translated_from_doc <> "0" Then
                If Me.show_message = True Then
                    omessage = New bc_cs_message("Blue Curve - create", "Document is already  copy  cant copy", bc_cs_message.MESSAGE)
                End If
                ocommentary = New bc_cs_activity_log("bc_am_copy_doc", "copy", bc_cs_activity_codes.TRACE_ENTRY, "Document is already  copy  can't copy")
                ldoc.translate_flag = False
                oh = New bc_om_history
                oh.comment = "Document is already  copy  can't copy"
                ldoc.history.Add(oh)

                Exit Function
            End If

            Dim found As Boolean = False
            copy = True
            Dim co As Integer
            co = 0
            For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                If bc_am_load_objects.obc_pub_types.pubtype(i).id = ldoc.pub_type_id Then

                    REM else loop for each tranlation variant
                    For j = 0 To bc_am_load_objects.obc_pub_types.pubtype(i).translated_pub_types.count - 1
                        For k = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                            If bc_am_load_objects.obc_pub_types.pubtype(k).id = bc_am_load_objects.obc_pub_types.pubtype(i).translated_pub_types(j) Then
                                language = bc_am_load_objects.obc_pub_types.pubtype(k).language
                                ocommentary = New bc_cs_activity_log("bc_am_copy_doc", "copy", bc_cs_activity_codes.COMMENTARY, "Translating doc: " + ldoc.title + " to pub type id: " + CStr(bc_am_load_objects.obc_pub_types.pubtype(i).translated_pub_types(j)) + " language: " + CStr(language))
                                create_new_doc(CStr(bc_am_load_objects.obc_pub_types.pubtype(i).translated_pub_types(j)), language, ao_object, ldoc, False, ldoc.stage, True, co)
                                co = co + 1
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
    Public Function create_new_doc(ByVal pub_type_id As String, ByVal language As Long, ByVal ao_object As Object, ByVal ldoc As bc_om_document, ByVal no_workflow As Boolean, ByVal stage_id_to As Long, ByVal link_doc As Boolean, ByVal ord As Integer) As Boolean
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
            If Right(ldoc.filename, 5) = ldoc.extension Then
                original_filename = ldoc.filename
                'original_metadata = Left(original_filename, Len(original_filename) - 4) + ".dat"
            Else
                original_filename = ldoc.filename + ldoc.extension
                'original_metadata = ldoc.filename + ".dat"
            End If
            REM change current doc to reflect new doc
            Dim ndoc As New bc_om_document
            ndoc.id = 0
            ndoc.pub_type_id = pub_type_id
            ndoc.language_id = language
            'ndoc.action_Ids = ldoc.action_Ids
            ndoc.authors = ldoc.authors
            ndoc.bus_area = ldoc.bus_area
            ndoc.connection_method = ldoc.connection_method
            ndoc.disclosures = ldoc.disclosures
            ndoc.doc_date = ldoc.doc_date
            ndoc.entity_id = ldoc.entity_id
            ndoc.extension = ldoc.extension
            'ndoc.logged_on_user_name = ldoc.logged_on_user_name
            'ndoc.logged_on_user_password = ldoc.logged_on_user_password
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
            If ldoc.stage = 0 Then
                ndoc.stage = 1
                ndoc.stage_name = "Draft"
            Else
                ndoc.stage = ldoc.stage
                ndoc.stage_name = ldoc.stage_name
            End If
            ndoc.stage_name = ldoc.stage_name
            ndoc.sub_title = ldoc.sub_title
            ndoc.summary = ldoc.summary
            ndoc.taxonomy = ldoc.taxonomy
            ndoc.teaser_text = ldoc.teaser_text
            ndoc.workflow_stages = ldoc.workflow_stages

            Dim language_name As String = ""
            For n = 0 To bc_am_load_objects.obc_pub_types.languages.Count - 1
                If bc_am_load_objects.obc_pub_types.languages(n).id = ndoc.language_id Then
                    language_name = bc_am_load_objects.obc_pub_types.languages(n).name
                    Exit For
                End If
            Next

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
                ndoc.tmode = bc_cs_soap_base_class.tWRITE
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
            ndoc.write_data_to_file(bc_cs_central_settings.local_repos_path + CStr(ndoc.id) + ".dat")
            Dim oopen As New bc_am_at_open_doc
            If Me.close_doc = False Then
                oopen.open(True, ndoc, True)
            Else
                oopen.open(True, ndoc, False)
            End If
            If Me.refresh = True Then
                REM treat as local and make visible
                Try
                    bc_cs_central_settings.progress_bar.increment("Refreshing Copied Document: " + CStr(ord + 1) + " into " + language_name)
                Catch
                    bc_cs_central_settings.progress_bar = New bc_cs_progress("", "Refreshing Copied Document: " + CStr(ord + 1), 0, False, True)
                    bc_cs_central_settings.progress_bar.increment("Refreshing Copied Document: " + CStr(ord + 1) + " into " + language_name)

                End Try

                Try
                    ndoc.docobject.copydoc_refresh(ndoc.docobject, 1, bc_cs_central_settings.user_name, bc_cs_central_settings.user_password)
                Catch ex As Exception
                    ocommentary = New bc_cs_activity_log("bc_am_copy_doc", "create_new_doc", bc_cs_activity_codes.COMMENTARY, "copydoc_refresh not found in template")
                    Dim orefresh As New bc_am_at_component_refresh(ndoc.docobject, "word", False)
                    ndoc.refresh_components.embargo_level = Me.embargo_level
                    orefresh.refresh(False, Me.refresh_level, False, ndoc, True)
                End Try


                Dim j As Integer

                For i = 0 To ndoc.refresh_components.refresh_components.Count - 1
                    If ndoc.refresh_components.refresh_components(i).refresh_type = 2 Then
                        If ndoc.refresh_components.refresh_components(i).last_refresh_date <> "9-9-9999" Then
                            'If CDate(ndoc.refresh_components.refresh_components(i).last_refresh_date) <= Now And ndoc.refresh_components.refresh_components(i).last_refresh_date <> "9-9-9999" Then
                            ocommentary = New bc_cs_activity_log("bc_am_copy_doc", "create_new_doc", bc_cs_activity_codes.COMMENTARY, "Refreshing Disclosures in translated document")
                            bc_cs_central_settings.progress_bar.increment("Refreshing Disclosures into Copied Document: " + CStr(ord + 1) + " into " + language_name)
                            ndoc.refresh_components.language_id = language


                            For j = 0 To ndoc.refresh_components.refresh_components.Count - 1
                                If ndoc.refresh_components.refresh_components(j).refresh_type = 2 Then
                                    ndoc.refresh_components.refresh_components(j).disabled = False
                                    ndoc.refresh_components.refresh_components(j).no_refresh = False
                                End If
                            Next
                            ndoc.write_data_to_file(bc_cs_central_settings.local_repos_path + CStr(ndoc.id) + ".dat")
                            Try
                                ndoc.docobject.copydoc_refresh(ndoc.docobject, 2, bc_cs_central_settings.user_name, bc_cs_central_settings.user_password)
                            Catch ex As Exception
                                ocommentary = New bc_cs_activity_log("bc_am_copy_doc", "create_new_doc", bc_cs_activity_codes.COMMENTARY, "copydoc_refresh not found in template")
                                Dim orefresh As New bc_am_at_component_refresh(ndoc.docobject, "word", False)
                                orefresh.refresh(False, 2, False, ndoc, True)
                            End Try

                        Else
                            ocommentary = New bc_cs_activity_log("bc_am_copy_doc", "create_new_doc", bc_cs_activity_codes.COMMENTARY, "Disclosures not refreshed in original")
                        End If
                        Exit For
                    End If

                Next
                REM after refresh formatter
                Try
                    ndoc.docobject.at_after_translate(ndoc.docobject, CStr(ndoc.language_id))
                Catch ex As Exception
                    ocommentary = New bc_cs_activity_log("bc_am_copy_doc", "create_new_doc", bc_cs_activity_codes.COMMENTARY, "Failed to find vba at_after_translate: " + ex.Message)
                End Try
            End If
            REM if close requested submit copy doc
            If Me.close_doc = True Then
                ndoc.history.Clear()
                Dim oh As New bc_om_history
                oh.comment = "Document Copied from: " + CStr(ldoc.id)

                ndoc.history.Add(oh)
                ndoc.stage = stage_id_to
                ndoc.write_data_to_file(bc_cs_central_settings.local_repos_path + CStr(ndoc.id) + ".dat")
                ndoc.docobject.save()
                REM save active document 
                'Dim osubmit As New bc_am_at_submit(False, ndoc.docobject, "word", False, no_workflow, False, True)
                Dim osubmit As New bc_am_at_submit
                osubmit.submit_after_stage_change(ndoc, ndoc.docobject, "word")
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
'Public Class bc_am_distribution
'    Private doc_id As String
'    Private mode As Integer
'    Public Sub New(ByVal doc_id As String, ByVal mode As Integer)
'        Me.doc_id = doc_id
'        Me.mode = mode
'    End Sub
'    Public Function run() As Boolean
'        Dim slog = New bc_cs_activity_log("bc_am_distribution", "run", bc_cs_activity_codes.TRACE_ENTRY, "")
'        Try
'            Dim osql As New bc_om_sql("exec dbo.bcc_core_wf_distribute '" + CStr(doc_id) + "','" + CStr(mode) + "'")

'            run = False
'            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
'                osql.tmode = bc_cs_soap_base_class.tREAD
'                osql.transmit_to_server_and_receive(osql, True)

'            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
'                osql.db_read()
'            End If
'            If osql.success = True Then
'                run = True
'            Else
'                Dim ocommentary As New bc_cs_activity_log("bc_am_distribution", "run", bc_cs_activity_codes.COMMENTARY, "SQL Error: exec dbo.bcc_core_wf_distribute '" + CStr(doc_id) + "','" + CStr(mode) + "'")
'            End If

'        Catch ex As Exception
'            Dim db_err As New bc_cs_error_log("bc_am_distribution", "run", bc_cs_error_codes.USER_DEFINED, ex.Message)
'        Finally
'            slog = New bc_cs_activity_log("bc_am_distribution", "run", bc_cs_activity_codes.TRACE_EXIT, "")
'        End Try
'    End Function

'End Class
'Public Class bc_am_pdf_revert
'    Private doc_id As String
'    Public Sub New(ByVal doc_id As String)
'        Me.doc_id = doc_id
'    End Sub
'    Public Function run() As Boolean
'        Dim slog = New bc_cs_activity_log("bc_am_pdf_revert", "run", bc_cs_activity_codes.TRACE_ENTRY, "")
'        Try
'            Dim osql As New bc_om_sql("exec dbo.bcc_core_wf_switch_to_master '" + CStr(doc_id) + "', 0")
'            run = False
'            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
'                osql.tmode = bc_cs_soap_base_class.tREAD
'                osql.transmit_to_server_and_receive(osql, True)
'            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
'                osql.db_read()
'            End If
'            If osql.success = True Then
'                run = True
'            Else
'                Dim ocommentary As New bc_cs_activity_log("bc_am_distribution", "run", bc_cs_activity_codes.COMMENTARY, "SQL Error: exec dbo.bcc_core_wf_switch_to_master '" + CStr(doc_id) + "', 0" + "'")
'            End If

'        Catch ex As Exception
'            Dim db_err As New bc_cs_error_log("bc_am_pdf_revert", "run", bc_cs_error_codes.USER_DEFINED, ex.Message)
'        Finally
'            slog = New bc_cs_activity_log("bc_am_pdf_revert", "run", bc_cs_activity_codes.TRACE_EXIT, "")
'        End Try
'    End Function

'End Class

REM checks if document is an unpublished duplicate if so wont release it to next stage
'Public Class bc_am_check_duplicate
'    Public pub_type_id As String
'    Public entity_id As String
'    Public doc_id As String
'    Public out_text As String
'    Public Sub New(ByVal pub_type_id As String, ByVal entity_id As String, ByVal doc_id As String)
'        Me.pub_type_id = pub_type_id
'        Me.entity_id = entity_id
'        Me.doc_id = doc_id
'    End Sub
'    Public Function check_duplicate() As Boolean
'        Dim sql As String
'        sql = "exec dbo.bcc_core_check_duplicate " + CStr(pub_type_id) + "," + CStr(entity_id) + "," + CStr(doc_id)
'        Dim osql As New bc_om_sql(sql)

'        check_duplicate = False
'        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
'            osql.tmode = bc_cs_soap_base_class.tREAD
'            osql.transmit_to_server_and_receive(osql, True)

'        ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
'            osql.db_read()
'        End If
'        If osql.success = True Then
'            If IsArray(osql.results) Then
'                If osql.results(0, 0) = 1 Then
'                    Me.out_text = osql.results(1, 0)
'                    check_duplicate = True
'                End If
'            End If
'        End If
'    End Function

'End Class
REM conflict check
'Public Class bc_am_conflict_check
'    Public has_conflicts As Boolean = False
'    Public conflict_stage_id As Long
'    Public conflict_stage_name As String
'    Public oconflicts As bc_om_conflict_check
'    Public with_email As Boolean
'    Private doc_id As String
'    Public Sub New(ByVal doc_id As String, Optional ByVal with_email As Boolean = False)
'        Me.doc_id = doc_id
'        Me.with_email = with_email
'    End Sub
'    Public Function check_for_conflicts() As Boolean
'        Dim slog = New bc_cs_activity_log("bc_am_conflict_check", "check_for_conflicts", bc_cs_activity_codes.TRACE_ENTRY, "")

'        Try
'            oconflicts = New bc_om_conflict_check(Me.doc_id, Me.with_email)
'            check_for_conflicts = True
'            has_conflicts = False
'            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
'                oconflicts.tmode = bc_cs_soap_base_class.tREAD
'                oconflicts.transmit_to_server_and_receive(oconflicts, True)
'            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
'                oconflicts.db_read()
'            End If
'            Me.has_conflicts = oconflicts.has_conflicts
'            Me.conflict_stage_id = oconflicts.conflict_stage_id
'            Me.conflict_stage_name = oconflicts.conflict_stage_name
'            If oconflicts.err = True Then
'                check_for_conflicts = False
'            End If

'        Catch ex As Exception
'            REM conflict system has failed so event has failed
'            check_for_conflicts = False
'            Dim db_err As New bc_cs_error_log("bc_am_conflict_check", "check_for_conflicts", bc_cs_error_codes.USER_DEFINED, ex.Message)
'        Finally
'            slog = New bc_cs_activity_log("bc_am_conflict_check", "check_for_conflicts", bc_cs_activity_codes.TRACE_EXIT, "")
'        End Try
'    End Function


'End Class
REM archive units event



'Public Class bc_am_initial_publish_data
'    Private doc_id As String
'    Private entity_id As Long
'    Public Sub New(ByVal doc_id As String, ByVal entity_id As Long)
'        Me.doc_id = doc_id
'        Me.entity_id = entity_id
'    End Sub
'    Public Function run() As Boolean
'        Dim slog = New bc_cs_activity_log("bc_am_publish_data", "run", bc_cs_activity_codes.TRACE_ENTRY, "")
'        Dim omsg As bc_cs_message
'        Try
'            Dim osql As New bc_om_sql("exec dbo.bcc_core_wf_init_publish_data '" + CStr(doc_id) + "','" + CStr(entity_id) + "'")
'            run = False
'            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
'                osql.tmode = bc_cs_soap_base_class.tREAD
'                osql.transmit_to_server_and_receive(osql, True)

'            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
'                osql.db_read()
'            End If
'            If osql.success = True Then
'                REM check here result set
'                'run = True
'                If IsArray(osql.results) Then
'                    If osql.results(1, 0) = 1 Then
'                        REM data published sucessfully
'                        omsg = New bc_cs_message("Blue Curve", CStr(osql.results(0, 0)), bc_cs_message.MESSAGE)
'                        run = True
'                    ElseIf osql.results(1, 0) = 2 Then
'                        REM data not published as no draft
'                        omsg = New bc_cs_message("Blue Curve", CStr(osql.results(0, 0)), bc_cs_message.MESSAGE)
'                        run = False
'                    Else
'                        REM draft and publish data exists so do nothing
'                        run = True
'                    End If
'                End If
'            Else
'                Dim ocommentary As New bc_cs_activity_log("bc_am_publish_data", "run_archive", bc_cs_activity_codes.COMMENTARY, "SQL Error: exec dbo.bcc_core_wf_publish_data '" + CStr(doc_id) + "','" + CStr(entity_id) + "'")
'            End If

'        Catch ex As Exception
'            Dim db_err As New bc_cs_error_log("bc_am_publish_data", "run", bc_cs_error_codes.USER_DEFINED, ex.Message)
'        Finally
'            slog = New bc_cs_activity_log("bc_am_publish_data", "run", bc_cs_activity_codes.TRACE_EXIT, "")
'        End Try
'    End Function

'End Class
Public Class bc_am_componetize_component
    Public Sub New()

    End Sub
    Public Function componetize(ByVal obj As Object, ByRef ldoc As bc_om_document) As Boolean
        componetize = True
        Try
            Dim ocomm = New bc_cs_activity_log("bc_am_componetize_component", "componetize", bc_cs_activity_codes.TRACE_ENTRY, CStr(ldoc.refresh_components.refresh_components.Count))


            Dim odoc As New bc_ao_word(obj)
            Dim success As Boolean = True
            For i = 0 To ldoc.refresh_components.refresh_components.Count - 1
                With ldoc.refresh_components.refresh_components(i)
                    ocomm = New bc_cs_activity_log("bc_am_componetize_component", "componetize", bc_cs_activity_codes.COMMENTARY, "AA Start " + ldoc.refresh_components.refresh_components(i).locator)
                    .value = Nothing
                    .tx = Nothing
                    .objbyte = Nothing
                    .compressed_html = Nothing
                    If .mode = 9 Or .mode = 10 Then

                        .value = CStr(odoc.get_value_for_locator(ldoc.refresh_components.refresh_components(i).locator))
                    End If
                    If .mode = 8 Or .mode = 100 Or .mode = 11 Or .mode = 88 Or .mode = 888 Then
                        .objbyte = odoc.get_selection_in_rtf_compressed(success, ldoc.refresh_components.refresh_components(i).locator, False, ldoc.refresh_components.refresh_components(i).compressed_html, 0, .mode)
                        .tx = CStr(odoc.get_value_for_locator(ldoc.refresh_components.refresh_components(i).locator))
                        .images = odoc.get_images_in_selection(.locator)
                    End If
                    If .mode = 111 Then
                        REM changed feb 2010 compressed rtf here so that object to serialize is smaller
                        .objbyte = odoc.get_selection_in_rtf_compressed(success, ldoc.refresh_components.refresh_components(i).locator, False, ldoc.refresh_components.refresh_components(i).compressed_html, .mode)
                        .tx = CStr(odoc.get_value_for_locator(ldoc.refresh_components.refresh_components(i).locator))
                    End If
                    If .mode = 21 Or .mode = 26 Then
                        REM rtf entire table
                        .objbyte = odoc.get_selection_in_rtf_compressed(success, ldoc.refresh_components.refresh_components(i).locator, True, ldoc.refresh_components.refresh_components(i).compressed_html)
                        .tx = CStr(odoc.get_value_for_locator(ldoc.refresh_components.refresh_components(i).locator))
                        .images = odoc.get_images_in_selection(.locator, True)
                    End If
                    If .mode = 31 Then
                        REM chart component
                        .objbyte = odoc.get_selection_in_rtf_compressed(success, ldoc.refresh_components.refresh_components(i).locator, True, ldoc.refresh_components.refresh_components(i).compressed_html)
                        .tx = CStr(odoc.get_value_for_locator(ldoc.refresh_components.refresh_components(i).locator))
                        .images = odoc.get_images_in_selection(.locator)
                    End If
                    REM 5.8
                    Select Case .mode
                        Case 80, 880, 8880, 110, 210, 260

                            success = odoc.get_selection_from_dev_expresss_converter(ldoc.refresh_components.refresh_components(i))
                            .images = odoc.get_images_in_selection(.locator)

                    End Select

                    
                    ocomm = New bc_cs_activity_log("bc_am_componetize_component", "componetize", bc_cs_activity_codes.COMMENTARY, "AA END " + ldoc.refresh_components.refresh_components(i).locator)
                End With
                If success = False Then
                    REM clear out any components that succeeded
                    For j = 0 To i
                        With ldoc.refresh_components.refresh_components(j)
                            .value = Nothing
                            .tx = Nothing
                            .objbyte = Nothing
                            .compressed_html = Nothing
                        End With
                    Next
                    componetize = False
                    Exit Function

                End If
            Next
        Catch ex As Exception
            componetize = False
            Dim oerr As New bc_cs_activity_log("bc_am_componetize_component", "componetize", bc_cs_activity_codes.COMMENTARY, "Error with componetize component: " + ex.Message)
        Finally
            Dim ocomm = New bc_cs_activity_log("bc_am_componetize_component", "componetize", bc_cs_activity_codes.TRACE_EXIT, "")


        End Try
    End Function

End Class
'Public Class bc_am_embargo_check
'    Private doc_id As String
'    Private stage_to As Long
'    Public stage_out As Long
'    Public stage_out_name As String
'    Public res As String
'    Public Sub New(ByVal doc_id As String, ByVal stage_to As Long)
'        Me.doc_id = doc_id
'        Me.stage_to = stage_to
'    End Sub
'    Public Function check() As Boolean
'        REM calls sp which will return  stage
'        REM if embargo will return stage that matches stage to
'        REM if not will return stage to promote to
'        Dim slog = New bc_cs_activity_log("bc_am_embargo_check", "check", bc_cs_activity_codes.TRACE_ENTRY, "")
'        Try
'            Dim osql As New bc_om_sql("exec dbo.bcc_core_check_embargo '" + CStr(doc_id) + "'")

'            check = True
'            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
'                osql.tmode = bc_cs_soap_base_class.tREAD
'                osql.transmit_to_server_and_receive(osql, True)

'            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
'                osql.db_read()
'            End If
'            If osql.success = True Then
'                If IsArray(osql.results) Then
'                    If UBound(osql.results, 2) >= 0 Then
'                        stage_out = CLng(osql.results(0, 0))
'                        res = CStr(osql.results(1, 0))
'                        stage_out_name = CStr(osql.results(2, 0))
'                    End If
'                End If
'                check = True
'            Else
'                Dim ocommentary As New bc_cs_activity_log("bc_am_publish_data", "run_archive", bc_cs_activity_codes.COMMENTARY, "SQL Error: exec dbo.bcc_core_check_embargo '" + CStr(doc_id) + "'")
'            End If

'        Catch ex As Exception
'            Dim db_err As New bc_cs_error_log("bc_am_embargo_check", "check", bc_cs_error_codes.USER_DEFINED, ex.Message)
'        Finally
'            slog = New bc_cs_activity_log("bc_am_embargo_check", "check", bc_cs_activity_codes.TRACE_EXIT, "")
'        End Try
'    End Function
'    Public Function result() As String
'        If res = "Fail" Then
'            result = "Document has companies under Embargo so will be in stage: " + stage_out_name
'        Else
'            result = "Document does not have embargo companies so auto promote to stage:" + stage_out_name
'        End If
'    End Function
'End Class
'Public Class bc_am_check_restrictions
'    Private doc_id As String
'    Private stage_to As Long
'    Public stage_out As Long
'    Public entity_id As Long
'    Public mode As Long
'    Public res_tx As String
'    Public stage_out_name As String
'    Public res As String
'    Public Sub New(ByVal doc_id As String, ByVal stage_to As Long, ByVal entity_id As Long)
'        Me.doc_id = doc_id
'        Me.stage_to = stage_to
'        Me.entity_id = entity_id
'    End Sub
'    Public Function check() As Boolean
'        REM calls sp which will return  stage
'        REM if embargo will return stage that matches stage to
'        REM if not will return stage to promote to
'        Dim slog = New bc_cs_activity_log("bc_am_check_restrictions", "check", bc_cs_activity_codes.TRACE_ENTRY, "")
'        Try
'            Dim osql As New bc_om_sql("exec bcc_core_res_workflow  '" + CStr(entity_id) + "','" + CStr(doc_id) + "'")

'            check = True
'            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
'                osql.tmode = bc_cs_soap_base_class.tREAD
'                osql.transmit_to_server_and_receive(osql, True)

'            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
'                osql.db_read()
'            End If
'            If osql.success = True Then
'                If IsArray(osql.results) Then
'                    If UBound(osql.results, 2) >= 0 Then
'                        Me.mode = CLng(osql.results(0, 0))
'                        Me.stage_out = CLng(osql.results(1, 0))
'                        Me.stage_out_name = CStr(osql.results(2, 0))
'                        Me.res_tx = CStr(osql.results(3, 0))
'                    End If
'                End If
'                check = True
'            Else
'                Dim ocommentary As New bc_cs_activity_log("bc_am_publish_data", "run_archive", bc_cs_activity_codes.COMMENTARY, "SQL Error: exec bcc_core_res_workflow")
'            End If

'        Catch ex As Exception
'            Dim db_err As New bc_cs_error_log("bc_am_check_restrictions", "check", bc_cs_error_codes.USER_DEFINED, ex.Message)
'        Finally
'            slog = New bc_cs_activity_log("bc_am_check_restrictions", "check", bc_cs_activity_codes.TRACE_EXIT, "")
'        End Try
'    End Function
'    Public Function result() As String
'        If res = "Fail" Then
'            result = "Document has companies under Embargo so will be in stage: " + stage_out_name
'        Else
'            result = "Document does not have embargo companies so auto promote to stage:" + stage_out_name
'        End If
'    End Function
'End Class
'Public Class bc_am_us_flag
'    Public entity_id As Long
'    Public flag As Long
'    Public stage_id_to As Long
'    Public stage_name_to As String
'    Public pub_type_new As Long
'    Public Sub New(ByVal entity_id As Long)
'        Me.entity_id = entity_id
'    End Sub
'    Public Function check() As Boolean
'        REM calls sp which will return  stage
'        REM if embargo will return stage that matches stage to
'        REM if not will return stage to promote to
'        Dim slog = New bc_cs_activity_log("bc_am_us_flag", "check", bc_cs_activity_codes.TRACE_ENTRY, "")
'        Try
'            Dim osql As New bc_om_sql("exec bcc_core_us_flag  '" + CStr(entity_id) + "'")

'            check = False
'            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
'                osql.tmode = bc_cs_soap_base_class.tREAD
'                osql.transmit_to_server_and_receive(osql, True)

'            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
'                osql.db_read()
'            End If
'            If osql.success = True Then
'                If IsArray(osql.results) Then
'                    If UBound(osql.results, 2) >= 0 Then
'                        Me.flag = CLng(osql.results(0, 0))
'                        Me.stage_id_to = CLng(osql.results(1, 0))
'                        Me.stage_name_to = CStr(osql.results(2, 0))
'                        Me.pub_type_new = CStr(osql.results(3, 0))
'                    End If
'                End If
'                check = True
'            Else
'                Dim ocommentary As New bc_cs_activity_log("bc_am_us_flag", "check", bc_cs_activity_codes.COMMENTARY, "SQL Error: exec bcc_core_us_flag  '" + CStr(entity_id) + "'")
'            End If

'        Catch ex As Exception
'            Dim db_err As New bc_cs_error_log("bc_am_check_restrictions", "us_flag", bc_cs_error_codes.USER_DEFINED, ex.Message)
'        Finally
'            slog = New bc_cs_activity_log("bc_am_check_restrictions", "check", bc_cs_activity_codes.TRACE_EXIT, "")
'        End Try
'    End Function

'End Class


'Public Class bc_am_check_dist_list
'    Private doc_id As String

'    Public Sub New(ByVal doc_id As String)
'        Me.doc_id = doc_id
'    End Sub
'    Public Function check() As Boolean
'        Dim slog = New bc_cs_activity_log("bc_am_check_dist_list", "check", bc_cs_activity_codes.TRACE_ENTRY, "")
'        Try
'            Dim min_num As Integer
'            Dim email_num As Integer
'            Dim paper_num As Integer
'            Dim msg As String = ""
'            Dim osql As New bc_om_sql("exec dbo.bcc_core_wf_check_dist_list '" + CStr(doc_id) + "'")
'            check = False
'            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
'                osql.tmode = bc_cs_soap_base_class.tREAD
'                osql.transmit_to_server_and_receive(osql, True)

'            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
'                osql.db_read()
'            End If
'            If osql.success = True Then
'                If IsArray(osql.results) Then
'                    If UBound(osql.results, 2) >= 0 Then
'                        min_num = CLng(osql.results(0, 0))
'                        email_num = CStr(osql.results(1, 0))
'                        paper_num = CStr(osql.results(2, 0))
'                        If paper_num >= min_num And email_num >= min_num Then
'                            check = True
'                            Exit Function
'                        End If
'                        If paper_num < min_num And email_num < min_num Then
'                            msg = "Both Paper (" + CStr(paper_num) + "/" + CStr(min_num) + ") and Email distribution (" + CStr(email_num) + "/" + CStr(min_num) + ") lists are below minimum"
'                        ElseIf paper_num < min_num Then
'                            msg = "Paper distribution list (" + CStr(paper_num) + "/" + CStr(min_num) + ") is below minimum"
'                        ElseIf email_num < min_num Then
'                            msg = "Email distribution list (" + CStr(email_num) + "/" + CStr(min_num) + ") is below minimum"
'                        End If
'                        Dim omsg As New bc_cs_message("Distribution Lists", msg + " do you wish to continue with stage change? If not please check distribution lists via Reach Links.", bc_cs_message.MESSAGE, True)
'                        If omsg.cancel_selected = False Then
'                            check = True
'                            Exit Function
'                        End If
'                    End If
'                End If
'            Else
'                Dim ocommentary As New bc_cs_activity_log("bc_am_check_dist_list", "bc_am_check_dist_list", bc_cs_activity_codes.COMMENTARY, "SQL Error: exec dbo.bcc_core_wf_check_dist_list '" + CStr(doc_id) + "'")
'            End If

'        Catch ex As Exception
'            Dim db_err As New bc_cs_error_log("bc_am_check_dist_list", "check", bc_cs_error_codes.USER_DEFINED, ex.Message)
'        Finally
'            slog = New bc_cs_activity_log("bc_am_check_dist_list", "check", bc_cs_activity_codes.TRACE_EXIT, "")
'        End Try
'    End Function

'End Class
'Public Class bc_am_first_call_note
'    Private doc_id As String
'    Public Sub New(ByVal doc_id As String, ByVal entity_id As Long)
'        Me.doc_id = doc_id
'    End Sub
'    Public Function run() As Boolean
'        Dim slog = New bc_cs_activity_log("bc_am_first_call_note", "run", bc_cs_activity_codes.TRACE_ENTRY, "")
'        Try
'            Dim osql As New bc_om_sql("exec dbo.bcc_core_wf_first_call_note '" + CStr(doc_id) + "'")
'            run = False
'            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
'                osql.tmode = bc_cs_soap_base_class.tREAD
'                osql.transmit_to_server_and_receive(osql, True)

'            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
'                osql.db_read()
'            End If
'            If osql.success = True Then
'                run = True
'            Else
'                Dim ocommentary As New bc_cs_activity_log("bc_am_first_call_note", "run", bc_cs_activity_codes.COMMENTARY, "SQL Error: exec dbo.bcc_core_wf_first_call_note '" + CStr(doc_id) + "'")
'            End If

'        Catch ex As Exception
'            Dim db_err As New bc_cs_error_log("bc_am_first_call_note", "run", bc_cs_error_codes.USER_DEFINED, ex.Message)
'        Finally
'            slog = New bc_cs_activity_log("bc_am_first_call_note", "run", bc_cs_activity_codes.TRACE_EXIT, "")
'        End Try
'    End Function

'End Class
REM used 

REM event create local copy and sends attches to email
'Public Class bc_am_send_to_corp
'    Public dest_pub_type_id As Long
'    Public success As Boolean
'    Public show_message As Boolean
'    'Private ldoc As bc_om_document
'    Private ldoc As bc_om_document
'    Private odoc As bc_ao_word
'    Private close_doc As Boolean
'    Private refresh As Boolean
'    Private ao_object As Object
'    Private embargo_level As Integer

'    Public Sub New(ByVal ldoc As bc_om_document, ByVal odoc As bc_ao_word, ByVal ao_object As Object, Optional ByVal close_doc As Boolean = False, Optional ByVal refresh As Boolean = True, Optional ByVal show_message As Boolean = False, Optional ByVal embargo_level As Integer = 0)
'        Me.ldoc = ldoc
'        Me.odoc = odoc
'        Me.close_doc = close_doc
'        Me.ao_object = ao_object
'        Me.embargo_level = embargo_level
'        Me.refresh = refresh
'        Me.show_message = show_message
'    End Sub
'    Public Function copy() As Boolean
'        Dim slog = New bc_cs_activity_log("bc_am_send_to_corp", "create_new_doc", bc_cs_activity_codes.TRACE_ENTRY, "")
'        'Dim original_doc_id As Long

'        Try
'            Dim ffs As New bc_cs_file_transfer_services
'            REM save original document
'            Dim sfn As String
'            Dim ps As New bc_cs_progress
'            bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve", "Sending Document for Review", 5, False, True)
'            bc_cs_central_settings.progress_bar.increment("Refreshing Embargo Copy...")

'            REM refresh
'            If ldoc.id = 0 Then
'                sfn = ldoc.filename
'            Else
'                sfn = CStr(ldoc.id)
'            End If
'            odoc.visible(False)

'            Dim orefresh As New bc_am_at_component_refresh(ao_object, "word")

'            ldoc.refresh_components.embargo_level = Me.embargo_level
'            orefresh.refresh(False, 1, False, ldoc, True)
'            odoc.save()
'            odoc.saveas(sfn)
'            REM if adobe installed then render
'            Dim orender As New bc_am_render_to_pdf(False, ao_object, "word", "Adobe PDF", ldoc.extension, 30, CStr(ldoc.id))
'            bc_cs_central_settings.progress_bar.increment("Attempting to render to pdf...")
'            ldoc.docobject = ao_object
'            orender.render(ldoc)
'            If orender.success = True Then
'                bc_cs_central_settings.progress_bar.increment("Preparing Email with PDF ...")
'                send_email(bc_cs_central_settings.local_repos_path + sfn + ".pdf", ldoc.title)
'                If ffs.check_document_exists(bc_cs_central_settings.local_repos_path + sfn + ".pdf") Then
'                    ffs.delete_file(bc_cs_central_settings.local_repos_path + sfn + ".pdf")
'                Else
'                    bc_cs_central_settings.progress_bar.increment("Preparing Email with DOC...")
'                    REM send word copy
'                    send_email(bc_cs_central_settings.local_repos_path + sfn + ".doc", ldoc.title)
'                End If
'            Else
'                bc_cs_central_settings.progress_bar.increment("Preparing Email with DOC...")
'                REM send word copy
'                send_email(bc_cs_central_settings.local_repos_path + sfn + ".doc", ldoc.title)
'            End If
'            REM refresh original
'            bc_cs_central_settings.progress_bar.increment("Refreshing Original...")
'            ldoc.refresh_components.embargo_level = 0
'            orefresh.refresh(False, 1, False, ldoc, True)

'        Catch ex As Exception
'            success = False
'            copy = False
'            Dim db_err As New bc_cs_error_log("bc_am_send_to_corp", "create_new_doc", bc_cs_error_codes.USER_DEFINED, ex.Message)
'        Finally
'            REM switch back original
'            bc_cs_central_settings.progress_bar.unload()
'            odoc.visible(True)
'            slog = New bc_cs_activity_log("bc_am_send_to_corp", "create_new_doc", bc_cs_activity_codes.TRACE_EXIT, "")
'        End Try
'    End Function
'    Private Function send_email(ByVal filename As String, ByVal title As String) As Boolean
'        Dim slog As New bc_cs_activity_log("bc_am_send_to_corp", "send_email", bc_cs_activity_codes.TRACE_ENTRY, "")
'        Try
'            REM hoo into outlook
'            Dim ooutlook As Object
'            Dim omail As Object
'            Try
'                ooutlook = GetObject(, "Outlook.application")
'            Catch
'                Try
'                    ooutlook = CreateObject("Outlook.application")
'                Catch ex As Exception
'                    Dim db_err As New bc_cs_error_log("bc_ao_word", "invoke_word", bc_cs_error_codes.USER_DEFINED, ex.Message)
'                    Exit Function
'                End Try
'            End Try
'            With ooutlook
'                omail = ooutlook.createitem(0)
'                With omail
'                    .subject = title + ": for review"
'                    .attachments.add(filename, 1)
'                    .Display()
'                End With
'            End With
'        Catch ex As Exception
'            Dim db_err As New bc_cs_error_log("bc_am_send_to_corp", "send_email", bc_cs_error_codes.USER_DEFINED, ex.Message)
'        Finally
'            REM switch back original
'            slog = New bc_cs_activity_log("bc_am_send_to_corp", "send_email", bc_cs_activity_codes.TRACE_EXIT, "")
'        End Try


'    End Function
'End Class

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
            bc_am_load_objects.obc_users = bc_am_load_objects.obc_users.read_data_from_file(bc_cs_central_settings.local_template_path + bc_am_load_objects.USERS_FILENAME)
            REM set logged on user
            For i = 0 To bc_am_load_objects.obc_users.user.Count - 1
                If LCase(bc_am_load_objects.obc_users.user(i).os_user_name) = LCase(bc_cs_central_settings.GetLoginName) Then
                    bc_cs_central_settings.logged_on_user_id = bc_am_load_objects.obc_users.user(i).id
                    Exit For
                End If
            Next
            REM entities
            bc_am_load_objects.obc_entities = bc_am_load_objects.obc_entities.read_data_from_file(bc_cs_central_settings.local_template_path + bc_am_load_objects.ENTITIES_FILENAME)
            REM pub types
            bc_am_load_objects.obc_pub_types = bc_am_load_objects.obc_pub_types.read_data_from_file(bc_cs_central_settings.local_template_path + bc_am_load_objects.PUB_TYPES_FILENAME)
            REM preferances
            bc_am_load_objects.obc_prefs = bc_am_load_objects.obc_pub_types.read_data_from_file(bc_cs_central_settings.local_template_path + bc_am_load_objects.PREFS_FILENAME)

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
                    If (bc_cs_central_settings.show_authentication_form = 0 Or bc_cs_central_settings.show_authentication_form = 2) Then
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

            REM SW cope with office versions
            If Left(Right(name, 4), 1) = "." Then
                name = Left(name, Len(name) - 4)
            End If
            If Left(Right(name, 5), 1) = "." Then
                name = Left(name, Len(name) - 5)
            End If


            Dim fs As New bc_cs_file_transfer_services
            If fs.check_document_exists_and_can_be_read(bc_cs_central_settings.local_repos_path + name + ".dat") = False Then
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.LOCAL Then
                    omessage = New bc_cs_message("Blue Curve create", "Cannot Submit Document as no network and document was not opened via Create.", bc_cs_message.MESSAGE)
                    Exit Sub
                End If
                REM xml metadata file doesnt exists so attempt to recreate
                Dim ordm As New bc_am_recreate_doc_metadata
                If ordm.recreate_doc_metadata(name, odoc) = False Then
                    Exit Sub
                Else
                    bc_am_load_objects.obc_current_document = bc_am_load_objects.obc_current_document.read_data_from_file(bc_cs_central_settings.local_repos_path + name + ".dat")
                End If
            Else
                REM load metadata
                bc_am_load_objects.obc_current_document = bc_am_load_objects.obc_current_document.read_data_from_file(bc_cs_central_settings.local_repos_path + name + ".dat")
            End If
            bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve = create", "Translating Document...", 1, False, True)
            bc_cs_central_settings.progress_bar.increment("Translating Document...")
            REM if form authentication assign values
            'If bc_cs_central_settings.show_authentication_form = 1 Then
            '    bc_cs_central_settings.user_name = bc_am_load_objects.obc_current_document.logged_on_user_name
            '    bc_cs_central_settings.user_password = bc_am_load_objects.obc_current_document.logged_on_user_password
            'End If
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
            Dim ocopy As New bc_am_copy_doc(bc_am_load_objects.obc_current_document.id, ao_object, False, True, False)
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

'Public Class bc_am_email_copy
'    Public Sub New()

'    End Sub
'    REM loads relevant data for submit into memory
'    Private Function load_data() As Boolean
'        Dim slog = New bc_cs_activity_log("bc_am_email_copy", "load_data", bc_cs_activity_codes.TRACE_ENTRY, "")

'        Try

'            load_data = False
'            Dim i As Integer
'            'Dim obc_objects As New bc_am_load_objects
'            REM check connection
'            REM should be getting this from document
'            bc_cs_central_settings.version = Application.ProductVersion
'            'bc_cs_central_settings.selected_conn_method = bc_am_load_objects.obc_current_document.connection_method
'            REM users
'            bc_am_load_objects.obc_users = bc_am_load_objects.obc_users.read_data_from_file(bc_cs_central_settings.local_template_path + bc_am_load_objects.USERS_FILENAME)
'            REM set logged on user
'            For i = 0 To bc_am_load_objects.obc_users.user.Count - 1
'                If LCase(bc_am_load_objects.obc_users.user(i).os_user_name) = LCase(bc_cs_central_settings.GetLoginName) Then
'                    bc_cs_central_settings.logged_on_user_id = bc_am_load_objects.obc_users.user(i).id
'                    Exit For
'                End If
'            Next
'            load_data = True
'        Catch ex As Exception
'            Dim db_err As New bc_cs_error_log("bc_am_email_copy", "load_data", bc_cs_error_codes.USER_DEFINED, ex.Message)
'        Finally
'            slog = New bc_cs_activity_log("bc_am_email_copy", "load_data", bc_cs_activity_codes.TRACE_EXIT, "")
'        End Try

'    End Function
'    Public Sub email_copy(ByVal show_form As Boolean, ByVal ao_object As Object, ByVal ao_type As String, ByVal embargo_level As Integer)
'        Dim otrace As New bc_cs_activity_log("bc_am_email_copy", "email_copy", bc_cs_activity_codes.TRACE_ENTRY, "")

'        Try
'            Dim bc_cs_central_settings As New bc_cs_central_settings

'            REM read in config file of current doc
'            Dim odoc As Object
'            Dim omessage As bc_cs_message
'            Dim ldoc As New bc_om_document
'            Dim recreate_flag As Boolean = False
'            If load_data() = False Then
'                omessage = New bc_cs_message("Blue Curve Create", "Meta Data Failed to Load!", bc_cs_message.MESSAGE)
'                Exit Sub
'            End If

'            REM instantiate correct AO object
'            If ao_type = bc_ao_at_object.WORD_DOC Then
'                odoc = New bc_ao_word(ao_object)
'            Else
'                omessage = New bc_cs_message("Blue Curve Create", "Objtec Not Yet Implemented: " + CStr(ao_type), bc_cs_message.MESSAGE)
'                Exit Sub
'            End If

'            Dim iname As String
'            iname = odoc.get_doc_id
'            Dim fs As New bc_cs_file_transfer_services
'            If fs.check_document_exists_and_can_be_read(bc_cs_central_settings.local_repos_path + iname + ".dat") = False Then

'                REM xml metadata file doesnt exists so attempt to recreate
'                Dim ordm As New bc_am_recreate_doc_metadata
'                If ordm.recreate_doc_metadata(iname, odoc) = False Then
'                    Exit Sub
'                Else
'                    ldoc = ldoc.read_data_from_file(bc_cs_central_settings.local_repos_path + iname + ".dat")
'                    recreate_flag = True
'                End If
'            Else
'                REM load metadata
'                ldoc = ldoc.read_data_from_file(bc_cs_central_settings.local_repos_path + iname + ".dat")
'            End If
'            Dim osend_to_corp As bc_am_send_to_corp
'            osend_to_corp = New bc_am_send_to_corp(ldoc, odoc, ao_object, False, True, True, embargo_level)
'            osend_to_corp.copy()

'        Catch ex As Exception
'            Dim db_err As New bc_cs_error_log("bc_am_email_copy", "email_copy", bc_cs_error_codes.USER_DEFINED, ex.Message)

'        Finally
'            otrace = New bc_cs_activity_log("bc_am_email_copy", "email_copy", bc_cs_activity_codes.TRACE_EXIT, "")
'        End Try
'    End Sub
'End Class




Public Class pr_custom_event
    Inherits bc_am_custom_client_side_event
    Implements bc_am_custom_client_side_event_interface
    Public Function run() As Boolean Implements bc_am_custom_client_side_event_interface.run
        '    REM e.g. disclosures
        '    refresh(1, 0)
        '    write_history("hello Paul")
        '    Return Me.success
    End Function
End Class
Public Interface bc_am_custom_client_side_event_interface
    Function run() As Boolean
End Interface

Public MustInherit Class bc_am_custom_client_side_event
    REM bc document object
    Public bc_document As bc_om_document
    REM word document object
    Public ms_document As Object
    Public err_text As String
    Public success As Boolean
    REM history list
    Public history_list As New ArrayList
    Public action_name As String
    Public event_complete As Boolean
    Public odoc As New bc_am_at_open_doc

    Public Function initialise(ByRef obj As Object) As Boolean
        success = True
        event_complete = False
        open_document(obj)
        If event_complete = True Then
            Return True
        End If
        ms_document = bc_document.docobject
    End Function
    Private Sub open_document(ByRef obj As Object)
        REM get handle to active document
        Dim slog = New bc_cs_activity_log("bc_am_custom_client_side_event", "open_document", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim render_only As Boolean = False
            Dim cont As Boolean = True
            If IsNothing(obj) Then
                cont = check_for_open_doc(obj, render_only)
                If cont = False Then
                    history_list.Add("Event: " + action_name + " surpressed as cant operate on mime type: " + bc_document.extension)
                    bc_document.docobject = obj
                    event_complete = True
                    Exit Sub
                ElseIf render_only = True Then
                    If action_name <> "Render" And action_name <> "Render and Publish" Then
                        REM if action is not a render suppress
                        history_list.Add("Event: " + action_name + " surpressed as cant operate on imported mime type: " + bc_document.extension)
                        bc_document.docobject = obj
                        event_complete = True
                        Exit Sub
                    End If
                End If
            Else
                bc_document.docobject = obj
            End If
        Catch ex As Exception
            Me.success = False
            Dim db_err As New bc_cs_error_log("bc_am_custom_client_side_event", "open_document", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_custom_client_side_event", "open_document", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    Protected Sub bc_write_process_history(ByVal text As String)
        Me.history_list.Add(text)
        bc_write_system_log(text)
    End Sub
    Protected Sub bc_write_system_log(ByVal text As String)
        Dim ocommentray As New bc_cs_activity_log(Me.action_name, "run", bc_cs_activity_codes.COMMENTARY, text)
    End Sub

    Private Function check_for_open_doc(ByRef outdoc As Object, ByRef render_only As Boolean) As Boolean
        Dim slog = New bc_cs_activity_log("bc_am_custom_client_side_event", "check_for_open_doc", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            Dim ocommentary As bc_cs_activity_log
            render_only = False
            check_for_open_doc = True
            REM check mime type can support open doc
            check_for_open_doc = True
            Dim omime As New bc_am_mime_types
            If omime.can_edit(bc_document.extension, render_only) = False Then
                check_for_open_doc = False
                Exit Function
            End If

            REM if document is not open  
            REM open it as events requires it

            ocommentary = New bc_cs_activity_log("bc_am_custom_client_side_event", "check_for_open_doc", bc_cs_activity_codes.COMMENTARY, "Event requires an Open docuent")
            'Dim obc_workflow As New bc_am_workflow
            bc_document.history.Clear()
            'obc_workflow.set_server_status("Checking out document from server")
            REM update metadata
            bc_document.connection_method = bc_cs_central_settings.selected_conn_method
            REM ldoc.write_xml_to_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ldoc.extension)
            odoc.open(False, bc_document, False)
            bc_document.history.Clear()
            bc_document.support_documents.Clear()
            REM reset stage transition
            'ldoc.stage = Me.stage_id_to
            'ldoc.stage_name = Me.stage_name_to
            bc_document.write_data_to_file(bc_cs_central_settings.local_repos_path + CStr(bc_document.id) + ".dat")
            outdoc = bc_document
            outdoc = bc_document.docobject

        Catch ex As Exception
            success = False
            Dim db_err As New bc_cs_error_log("bc_am_custom_client_side_event", "check_for_open_doc", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_custom_client_side_event", "check_for_open_doc", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function


    REM standard functions on a document object supported by BC core

    Public Sub bc_show_message(ByVal text As String)
        Dim omsg As New bc_cs_message(Me.action_name, text, bc_cs_message.MESSAGE)
    End Sub
    Public Function bc_refresh_doc(Optional ByVal refresh_type As Integer = 1, Optional ByVal embargo_level As Integer = 0) As Boolean
        Dim slog As New bc_cs_activity_log("bc_am_custom_client_side_event", "refresh", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim orefresh As New bc_am_at_component_refresh(ms_document, "word")

            bc_document.refresh_components.embargo_level = embargo_level
            orefresh.refresh(False, refresh_type, False, bc_document, True)
            bc_refresh_doc = orefresh.success
        Catch ex As Exception
            bc_refresh_doc = False
            Dim db_err As New bc_cs_error_log("bc_am_custom_client_side_event", "refresh", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            bc_document.refresh_components.embargo_level = 0
            slog = New bc_cs_activity_log("bc_am_custom_client_side_event", "refresh", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
    'Public Function bc_render_doc_to_pdf() As Boolean
    '    Dim slog As New bc_cs_activity_log("bc_am_custom_client_side_event", "render_to_pdf", bc_cs_activity_codes.TRACE_ENTRY, "")
    '    Try
    '        Dim obc_render As New bc_am_render_to_pdf(False, Me.ms_document, "word", "Adobe PDF", bc_document.extension, 30, CStr(bc_document.id))
    '        If obc_render.render(bc_document) = True Then
    '            bc_render_doc_to_pdf = True
    '            bc_render_doc_to_pdf = obc_render.success
    '        Else
    '            bc_render_doc_to_pdf = False
    '            bc_render_doc_to_pdf = obc_render.success
    '        End If
    '    Catch ex As Exception
    '        bc_render_doc_to_pdf = False
    '        Dim db_err As New bc_cs_error_log("bc_am_custom_client_side_event", "render_to_pdf", bc_cs_error_codes.USER_DEFINED, ex.Message)
    '    Finally
    '        slog = New bc_cs_activity_log("bc_am_custom_client_side_event", "render_to_pdf", bc_cs_activity_codes.TRACE_EXIT, "")

    '    End Try

    'End Function
    Public Function bc_componetize_doc() As Boolean
        Dim slog As New bc_cs_activity_log("bc_am_custom_client_side_event", "bc_componetize_doc", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

        Catch ex As Exception
            bc_componetize_doc = False
            Dim db_err As New bc_cs_error_log("bc_am_custom_client_side_event", "bc_componetize_doc", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_custom_client_side_event", "bc_componetize_doc", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function
    Public Function bc_copy_doc(Optional ByVal check_duplicate As Boolean = True, Optional ByVal close_doc As Boolean = True, Optional ByVal refresh As Boolean = False, Optional ByVal refresh_level As Integer = 1, Optional ByVal embargo_level As Integer = 0) As Boolean
        Dim slog As New bc_cs_activity_log("bc_am_custom_client_side_event", "bc_copy_doc", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            'If check_duplicate = True Then
            '    If bc_document.sub_translated_docs <> "" Then
            '        Me.bc_write_process_history("Document already copied.")
            '        bc_copy_doc = True
            '        Exit Function
            '    End If
            '    If bc_document.translated_from_doc <> "0" Then
            '        Me.bc_write_process_history("Document is already  copy  cant copy")
            '        bc_copy_doc = True
            '        Exit Function
            '    End If
            'End If

            Dim ocopy As New bc_am_copy_doc(bc_document.id, ms_document, close_doc, refresh, False, refresh_level, embargo_level, check_duplicate)
            ocopy.copy(bc_document)
            REM if translate event then flag this
            If ocopy.success = True Then
                bc_copy_doc = True
                If check_duplicate = True Then
                    bc_document.translate_flag = True
                End If
            Else
                bc_copy_doc = False
            End If
        Catch ex As Exception
            bc_copy_doc = False
            Dim db_err As New bc_cs_error_log("bc_am_custom_client_side_event", "bc_copy_doc", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_custom_client_side_event", "bc_copy_doc", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
End Class

Public Class bc_am_scan_document

    Public success As Boolean
    Public stage_out As Long
    Public stage_out_name As String

    Public Sub New()

    End Sub

    Public Sub scan_document(ByVal listType As String, ByVal ao_object As Object, ByVal ao_type As String, Optional ByRef scanResults As ArrayList = Nothing)
        Dim otrace As New bc_cs_activity_log("bc_am_scan_document", "scan_document", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            Dim bc_cs_central_settings As New bc_cs_central_settings
            Dim odoc As bc_ao_at_object
            Dim omessage As bc_cs_message

            If listType = "" Then
                bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve - Create", "Scanning Document...", 10, False, True)
                bc_cs_central_settings.progress_bar.show()
            End If

            If listType = "" Then
                bc_cs_central_settings.progress_bar.increment("Scanning Document...")
            End If

            REM instantiate correct AO object
            If ao_type = bc_ao_at_object.WORD_DOC Then
                odoc = New bc_ao_word(ao_object)
            ElseIf ao_type = bc_ao_at_object.POWERPOINT_DOC Then
                omessage = New bc_cs_message("Blue Curve Create", "Not Yet Implemented!", bc_cs_message.MESSAGE)
                Exit Sub
            Else
                omessage = New bc_cs_message("Blue Curve Create", "Not Yet Implemented!", bc_cs_message.MESSAGE)
                Exit Sub
            End If

            If listType = "" Then
                bc_cs_central_settings.progress_bar.increment("Scanning Document...")
            End If

            scanResults = odoc.scan_document(listType, bc_cs_central_settings.document_scan_namespace, bc_cs_central_settings.document_scan_node)
            If scanResults Is Nothing Then
                success = False
                Exit Sub

            End If

            'If listType = "" Then
            '    bc_cs_central_settings.progress_bar.increment("Scanning Document...")
            'End If

            success = True

        Catch ex As Exception
            success = False
            If listType = "" Then
                bc_cs_central_settings.progress_bar.unload()
            End If
            Dim ocomm As New bc_cs_activity_log("bc_am_scan_document", "scan_document", bc_cs_activity_codes.COMMENTARY, "Document Scanning Failed:" + ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_am_scan_document", "scan_document", bc_cs_activity_codes.TRACE_EXIT, "")
            If listType = "" Then
                bc_cs_central_settings.progress_bar.unload()
            End If
        End Try
    End Sub

    Friend Function GetStage() As Boolean

        ' get the stage to move the scanned document to if results found

        Dim slog = New bc_cs_activity_log("bc_am_scan_document", "GetStage", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim osql As New bc_om_sql("exec dbo.bc_core_wf_get_scanned_doc_stage")
            GetStage = False
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                osql.tmode = bc_cs_soap_base_class.tREAD
                If osql.transmit_to_server_and_receive(osql, True) = False Then
                    Return False
                End If
            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                osql.db_read()
            End If
            If osql.success = True Then
                If IsArray(osql.results) Then
                    If UBound(osql.results, 2) >= 0 Then
                        stage_out = CLng(osql.results(0, 0))
                        stage_out_name = CStr(osql.results(1, 0))
                    End If
                End If
                GetStage = True
            Else

                Dim ocommentary As New bc_cs_activity_log("bc_am_scan_document", "GetStage", bc_cs_activity_codes.COMMENTARY, "SQL Error: exec dbo.bc_core_wf_get_compliance_stage")
                Return False
            End If

        Catch ex As Exception
            Dim ocommentary = New bc_cs_activity_log("bc_am_scan_document", "GetStage", bc_cs_activity_codes.COMMENTARY, "Error getting scan doc override stage " + ex.Message)
            Return False
        Finally
            slog = New bc_cs_activity_log("bc_am_scan_document", "GetStage", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function

End Class
Public Class bc_am_asyncronous_events
    Private mode As Integer
    Private Const CURRENT_DOCUMENT = 1
    Private Const PENDING_DOCUMENTS = 2
    Private ldoc As bc_om_document
    Private stage_id_from
    Private stage_id_to
    Private stage_id_to_name
    Private t As Thread

    Public Sub New(ByVal mode As Integer, Optional ByVal doc As bc_om_document = Nothing, Optional ByVal stage_id_from As Long = 0, Optional ByVal stage_id_to As Long = 0, Optional ByVal stage_id_to_name As String = "")
        Me.mode = mode
        If Me.mode = CURRENT_DOCUMENT Then
            Me.ldoc = doc
            Me.ldoc.packet_code = "pdd"
            Me.stage_id_from = stage_id_from
            Me.stage_id_to = stage_id_to
            Me.stage_id_to_name = stage_id_to_name
        End If
    End Sub
    Public Function run_events() As Boolean
        Try

            If Me.mode = CURRENT_DOCUMENT Then
                t = New Thread(AddressOf process_single_document)

            ElseIf Me.mode = PENDING_DOCUMENTS Then
                t = New Thread(AddressOf process_pending_documents)
            Else
                Exit Function
            End If
            t.Name = "BC_ASYNC_PROCESS_EVENTS"
            t.SetApartmentState(ApartmentState.STA)
            t.Start()

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_asyncronous_events", "run_events", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Function
    Public Sub abort_thread()
        Try
            t.Abort()
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_asyncronous_events", "abort_thread", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try
    End Sub
    Private Function load_data() As Boolean
        load_data = False
        Try
            bc_am_load_objects.obc_pub_types = bc_am_load_objects.obc_pub_types.read_data_from_file(bc_cs_central_settings.local_template_path + bc_am_load_objects.PUB_TYPES_FILENAME)
            load_data = True
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_asyncronous_events", "load_data", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Function
    REM called when a word event server is set up
    Private Sub process_pending_documents()

        Try
            Dim ols As String
            ols = bc_cs_central_settings.activity_file

            Thread.Sleep(1000)
            If load_data() = False Then
                Exit Sub
            End If
            While True
                If bc_cs_central_settings.ap_log_poll = 0 Then
                    bc_cs_central_settings.activity_file = "off"
                End If

                REM get earliest pending document
                Dim pd As New bc_om_pending_document
                pd.packet_code = "pd"
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    pd.db_read()
                Else
                    pd.tmode = bc_cs_soap_base_class.tREAD
                    If pd.transmit_to_server_and_receive(pd, False) = False Then
                        Dim ocomm As New bc_cs_activity_log("bc_am_asyncronous_events", "process_pending_documents", bc_cs_message.MESSAGE, "Failed to retrieve pending documents")
                    End If
                End If
                If pd.pending_document = True Then
                    bc_cs_central_settings.async_events_central_error_text.Clear()

                    Me.ldoc = pd.document
                    Me.stage_id_from = pd.stage_id_from
                    Me.stage_id_to = pd.stage_id_to
                    Me.stage_id_to_name = pd.stage_id_to_name
                    pd.failed = run_document()

                    If bc_cs_central_settings.async_events_central_error_text.Count > 0 Then
                        pd.failed = True
                        pd.system_errors = bc_cs_central_settings.async_events_central_error_text
                    End If

                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        pd.db_write()
                    Else
                        pd.tmode = bc_cs_soap_base_class.tWRITE
                        If pd.transmit_to_server_and_receive(pd, False) = False Then
                            Dim ocomm As New bc_cs_activity_log("bc_am_asyncronous_events", "process_pending_documents", bc_cs_message.MESSAGE, "Failed to clear pending documents")
                        End If
                    End If
                End If
                bc_cs_central_settings.activity_file = ols
                Thread.Sleep(bc_cs_central_settings.ap_pending_interval)
            End While
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_asyncronous_events", "process_pending_documents", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Sub
    REM called for asyncronous event firing from create or process
    REM on the same machine
    Private Sub process_single_document()
        Try
            Thread.Sleep(1000)
            If load_data() = False Then
                Exit Sub
            End If
            If run_document() = False Then
                Dim omsg As New bc_cs_message("Blue Curve", "Document " + ldoc.title + " stage change failed.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_asyncronous_events", "process_single_document", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Private Function run_document() As Boolean
        Try
            Dim weh As New bc_am_workflow_events_handler(Nothing, ldoc, True)
            Dim doc_open As Boolean = False
            run_document = False


            ldoc.action_Ids.Clear()
            ldoc.master_flag = True

            For i = 0 To ldoc.workflow_stages.stages.Count - 1
                If ldoc.workflow_stages.stages(i).stage_id = stage_id_to Then
                    For j = 0 To ldoc.workflow_stages.stages(i).action_ids.count - 1
                        ldoc.action_Ids.Add(ldoc.workflow_stages.stages(i).action_ids(j))
                    Next
                    Exit For
                End If
            Next

            If ldoc.action_Ids.Count > 0 Then
                REM actions required prior to stage change
                weh = New bc_am_workflow_events_handler(Nothing, ldoc, True, True)
                weh.doc_open = False
                weh.stage_id_to = stage_id_to
                weh.stage_name_to = stage_id_to_name
                REM set these so open doenst detect a stage change
                ldoc.stage = stage_id_from
                ldoc.stage_name = ldoc.original_stage_name
                ldoc.original_stage = stage_id_from


                weh.run_events(ldoc, False, False)
                If weh.success = True And bc_cs_central_settings.async_events_central_error_text.Count = 0 Then
                    ldoc.stage = weh.stage_id_to
                    ldoc.stage_name = weh.stage_name_to
                    If weh.doc_open = True Then
                        ldoc.stage = weh.stage_id_to
                        ldoc.stage_name = weh.stage_name_to
                        ldoc.write_data_to_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ".dat")
                        ldoc.docobject = weh.obj
                        ldoc.docobject.save()
                    End If
                Else
                    run_document = True
                    REM remove server side events
                    ldoc.server_side_events.events.Clear()
                    ldoc.main_note = "Stage Change Comment: change supressed due to event failure"
                    ldoc.stage = ldoc.original_stage
                    ldoc.stage_name = ldoc.original_stage_name
                    ' If weh.suppress_no_trans = False Then
                    'Dim omessage As New bc_cs_message("Blue Curve - Workflow", "Transitional Events Failed so document will not change Stage. Contact System Administrator", bc_cs_message.MESSAGE)
                    'End If
                    ldoc.main_note = "No Transition"
                    doc_open = False
                End If
                Try
                    bc_cs_central_settings.progress_bar.unload()
                Catch

                End Try

                If weh.doc_open = False Or weh.success = False Then
                    ldoc.register_only = True
                    ldoc.write_history = True

                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        ldoc.db_write(Nothing)
                    ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                        ldoc.tmode = bc_cs_soap_base_class.tWRITE
                        ldoc.transmit_to_server_and_receive(ldoc, True)
                    End If
                    REM delete xml if exists
                    Dim fs As New bc_cs_file_transfer_services
                    If weh.doc_open = True Then
                        Try
                            weh.close()
                        Catch

                        End Try
                        If fs.check_document_exists(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + "xml") = True Then
                            fs.delete_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + "xml")
                        End If
                        If fs.check_document_exists(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ldoc.extension) = True Then
                            fs.delete_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ldoc.extension)
                        End If
                    End If

                Else
                    ldoc.write_data_to_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ".dat")
                    Dim osubmit As New bc_am_at_submit
                    osubmit.submit_after_stage_change(ldoc, ldoc.docobject, "word", True, False, True)
                End If
                If ldoc.server_side_events_failed <> "" Then
                    If ldoc.server_side_events_failed.Length >= 24 Then
                        If Left(ldoc.server_side_events_failed, 24) = "Server Side Event failed" Then
                            run_document = True
                        End If
                    End If
                End If
                If ldoc.doc_write_error_text <> "" Then
                    run_document = True
                End If

            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_asyncronous_events", "run_document", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Function
End Class
Public Class bc_am_aync_doc_failure_alert
    Private Const POLLING_INTERVAL = 5000
    Private t As Thread
    Public Sub New()

    End Sub
    Public Sub run()
        t = New Thread(AddressOf check_for_failures)
        t.Start()
    End Sub
    Public Sub abort_thread()
        Try
            t.Abort()
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_aync_doc_failure_alert", "abort_thread", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try
    End Sub
    Private Sub check_for_failures()
        Try
            Dim os As New bc_om_check_async_failures
            Dim tx As String = ""
            Dim ols As String
            ols = bc_cs_central_settings.activity_file

            os.certificate.user_id = 1
            While True
                Thread.Sleep(bc_cs_central_settings.ap_failure_interval)
                If bc_cs_central_settings.ap_log_poll = 0 Then
                    bc_cs_central_settings.activity_file = "off"
                End If

                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    os.db_read()
                Else
                    os.tmode = bc_cs_soap_base_class.tREAD
                    If os.transmit_to_server_and_receive(os, False) = False Then
                        Dim ocomm As New bc_cs_activity_log(" bc_am_aync_doc_failure_alert", "check_for_failures", bc_cs_message.MESSAGE, "Failed to poll for async failures")
                    End If
                End If

                bc_cs_central_settings.activity_file = ols
                If os.failed_docs.Count > 0 Then
                    For i = 0 To os.failed_docs.Count - 1
                        If i = 0 And os.failed_docs.Count = 1 Then
                            tx = "The following document stage change failed please check history: " + vbCr + os.failed_docs(i)
                        ElseIf i = 0 And os.failed_docs.Count > 0 Then
                            tx = "The following documents stage change failed please check history: " + vbCr
                        Else
                            tx = vbCr + os.failed_docs(i)
                        End If
                    Next
                    Dim omsg As New bc_cs_message("Blue Curve Process", tx, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                End If
            End While
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_aync_doc_failure_alert", "check_for_failurest", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
End Class
REM FIL JIRA 8035
Public Class bc_am_make_local_copy
    Private _ldoc As bc_om_document
    Private _ao_object As Object
    Public Sub New(ByVal ldoc As bc_om_document, ByVal ao_object As Object)
        Me._ldoc = ldoc
        Me._ao_object = ao_object
    End Sub
    Public Function copy()
        copy = False
        Dim officeFormat As Integer = 0
        Dim fn As String = ""
        Dim title As String = ""
        Try
            Dim sql As New bc_om_sql("exec dbo.bc_core_make_local_copy " + CStr(_ldoc.id))
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                sql.db_read()
            Else
                sql.tmode = bc_cs_soap_base_class.tREAD
                If sql.transmit_to_server_and_receive(sql, True) = False Then
                    Exit Function
                End If
            End If
            If IsArray(sql.results) Then
                If UBound(sql.results, 2) = 0 AndAlso sql.results(0, 0) = "DB Err" Then
                    Dim ocomm As New bc_cs_activity_log("bc_am_make_local_copy", "copy", bc_cs_activity_codes.COMMENTARY, CStr(sql.results(1, 0)))
                    Exit Function
                End If
                Dim fs As New bc_cs_file_transfer_services

                For i = 0 To UBound(sql.results, 2)


                    title = sql.results(0, i)
                    fn = sql.results(1, i)
                    title = title.Replace("<date>", Format(Now, "dd-MMM-yyyy HH-mm-ss"))
                    fn = fn.Replace("<local_repos>", bc_cs_central_settings.local_repos_path)
                    Dim source_file As String

                    If sql.results(2, i) = 0 Then
                        REM first document is file
                        fn = fn + "\" + title + _ldoc.extension
                        If InStr(_ldoc.filename, _ldoc.extension) = 0 Then
                            source_file = bc_cs_central_settings.local_repos_path + _ldoc.filename + _ldoc.extension
                        Else
                            source_file = bc_cs_central_settings.local_repos_path + _ldoc.filename
                        End If

                        Dim ocomm As New bc_cs_activity_log("bc_am_make_local_copy", "copy", bc_cs_activity_codes.COMMENTARY, "Attempting to copy file: " + bc_cs_central_settings.local_repos_path + _ldoc.filename + _ldoc.extension)
                        If fs.check_document_exists(source_file) = True Then
                            ocomm = New bc_cs_activity_log("bc_am_make_local_copy", "copy", bc_cs_activity_codes.COMMENTARY, "Attempting to write file: " + fn)
                            FileCopy(source_file, fn)
                        End If
                    Else
                        REM second is pdf is it exists
                        fn = fn + "\" + title + ".pdf"
                        Dim pdf_filename As String = _ldoc.filename
                        pdf_filename = pdf_filename.Replace(_ldoc.extension, "") + ".pdf"
                        Dim ocomm As New bc_cs_activity_log("bc_am_make_local_copy", "copy", bc_cs_activity_codes.COMMENTARY, "Attempting to copy file: " + bc_cs_central_settings.local_repos_path + pdf_filename)
                        If fs.check_document_exists(bc_cs_central_settings.local_repos_path + pdf_filename) = True Then
                            ocomm = New bc_cs_activity_log("bc_am_make_local_copy", "copy", bc_cs_activity_codes.COMMENTARY, "Attempting to write file: " + fn)
                            FileCopy(bc_cs_central_settings.local_repos_path + pdf_filename, fn)
                        End If
                    End If
                Next
            End If
            copy = True
        Catch ex As Exception
            Dim ocomm As New bc_cs_activity_log("bc_am_make_local_copy", "copy", bc_cs_activity_codes.COMMENTARY, ex.Message)
        End Try
    End Function
End Class
REM END JIRA 8035
REM JIRA 8726 
Public Class bc_am_render_to_mht
    Private _ldoc As bc_om_document
    Private _ao_object As Object
    Public Sub New(ByVal ldoc As bc_om_document, ByVal ao_object As Object)
        Me._ldoc = ldoc
        Me._ao_object = ao_object
    End Sub
    Public Function render()
        render = False
        'Dim officeFormat As Integer = 0
        'Dim fn As String = ""
        'Dim title As String = ""
        Dim nword As bc_ao_word
        Try
            Dim source_file As String
            Dim ifile As String
            If InStr(_ldoc.filename, _ldoc.extension) = 0 Then
                source_file = bc_cs_central_settings.local_repos_path + _ldoc.filename + _ldoc.extension
            Else
                source_file = bc_cs_central_settings.local_repos_path + _ldoc.filename
            End If
            ifile = bc_cs_central_settings.local_repos_path + "mtmp" + _ldoc.extension

            FileCopy(source_file, ifile)

            nword = New bc_ao_word
            nword.open(ifile, False, False)
            Dim fn As String
            fn = _ldoc.filename.Replace(_ldoc.extension, "") + ".mht"
            If nword.save_to_mht(bc_cs_central_settings.local_repos_path + fn) = False Then
                Exit Function
            End If

            nword.close()
            Dim fs As New bc_cs_file_transfer_services
            If fs.check_document_exists(ifile) Then
                fs.delete_file(ifile)
            End If

            render = True

        Catch ex As Exception
            Dim ocomm As New bc_cs_activity_log("bc_am_render_to_mht", "render", bc_cs_activity_codes.COMMENTARY, ex.Message)
        Finally

        End Try
    End Function
End Class
Public Class bc_am_email_preview
    Private doc_id As Long
    Public Sub New(ByVal doc_id As Long)
        Me.doc_id = doc_id
    End Sub
    Public Function preview() As Boolean
        Dim slog = New bc_cs_activity_log("bc_am_email_preview", "preview", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim url As String
            Dim sql As String
            Dim reject As Boolean
            Dim ocom As bc_cs_activity_log


            url = bc_cs_central_settings.common_platform
            url = url + "document/documentemailpreview.do?osUser="
            url = url + bc_cs_central_settings.GetLoginName
            url = url + "&docid=" + CStr(Me.doc_id)
            ocom = New bc_cs_activity_log("bc_am_email_preview", "preview", bc_cs_activity_codes.COMMENTARY, "URL for email preview:" + url)
            REM invoke thread to launch browser
            System.Diagnostics.Process.Start(url)
            System.Threading.Thread.Sleep(10000)


            Dim omessage As New bc_cs_message("Blue Curve - HTML Email Preview", "Please Accept or Reject preview displayed in Brower. If rejected document will stay in Draft and system administrator will be notified.", True, False, "Accept", "Reject", False)

            reject = False

            If omessage.cancel_selected = True Then
                reject = True
            End If
            If reject = True Then
                sql = "exec dbo.core_wf_email_preview " + CStr(Me.doc_id) + "," + CStr(bc_cs_central_settings.logged_on_user_id) + ",1"
                preview = False
            Else
                sql = "exec dbo.core_wf_email_preview " + CStr(Me.doc_id) + "," + CStr(bc_cs_central_settings.logged_on_user_id) + ",0"
                preview = True
            End If
            Dim osql As New bc_om_sql(sql)
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                osql.db_read()
            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                osql.tmode = bc_cs_soap_base_class.tREAD
                If osql.transmit_to_server_and_receive(osql, True) = False Then
                    preview = False
                End If
            Else
                ocom = New bc_cs_activity_log("bc_am_email_preview", "preview", bc_cs_activity_codes.COMMENTARY, "Invalid connection method")
                Exit Function
            End If

            REM invokde accept reject form
        Catch ex As Exception
            preview = False
            Dim db_err As New bc_cs_error_log("bc_am_email_preview", "preview", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_email_preview", "preview", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function
End Class
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
                osql.tmode = bc_cs_soap_base_class.tREAD
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
Public Class bc_am_insert_data_into_excel
    Private _doc_id As String
    Private _excel As bc_ao_excel
    Public Sub New(excel As bc_ao_excel, doc_id As String)
        _excel = excel
        _doc_id = doc_id


    End Sub
    Public Function run() As Boolean
        Dim slog = New bc_cs_activity_log("bc_am_insert_data_into_excel", "run", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim osql As New bc_om_sql("exec dbo.bcc_core_get_data_for_excel '" + CStr(_doc_id) + "'")
            run = False
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                osql.tmode = bc_cs_soap_base_class.tREAD
                osql.transmit_to_server_and_receive(osql, True)
            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                osql.db_read()
            End If
            If osql.success = True Then
                run = True
            Else
                Dim ocommentary As New bc_cs_activity_log("bc_am_publish_document", "run_archive", bc_cs_activity_codes.COMMENTARY, "SQL Error: exec dbo.bcc_core_wf_publish_document '" + CStr(_doc_id) + "','")
                Exit Function
            End If
            REM insert into excel
            run = _excel.insert_data_into_workbook(osql.results)

        Catch ex As Exception
            slog = New bc_cs_activity_log("bc_am_insert_data_into_excel", "run", bc_cs_activity_codes.COMMENTARY, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_insert_data_into_excel", "run", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function

End Class
Public Class bc_am_build_automated_support_document
    Public template_name As String
    Public master_doc As bc_om_document
    Public err_text As String
    Public title As String
    Public odoc As bc_ao_word

    Public Function create_doc_from_process_toolbar(master_doc As bc_om_document) As Boolean
        Try
            Dim filename As String
            create_doc_from_process_toolbar = False
            REM create document`

            Dim bd As New bc_am_at_build_document(template_name, master_doc.pub_type_id, master_doc.entity_id, Nothing, 0)
            REM attach to it
            If bc_am_load_objects.obc_current_document.extension <> ".docx" Then
                Dim ocomm = New bc_cs_activity_log("bc_am_build_automated_support_document", "create_doc_from_process_toolbar", bc_cs_activity_codes.COMMENTARY, "Mime type:" + bc_am_load_objects.obc_current_document.extension + " not suppotred for action")
                Exit Function
            End If
            odoc = bd.odoc
            REM configure the metadata for it
            Dim dat_file As String
            dat_file = bc_cs_central_settings.local_repos_path + bc_am_load_objects.obc_current_document.filename + ".dat"
            Dim sdoc As New bc_om_document
            sdoc = sdoc.read_data_from_file(dat_file)
            Dim fs As New bc_cs_file_transfer_services

            If fs.check_document_exists(bc_cs_central_settings.local_repos_path + bc_am_load_objects.obc_current_document.filename + ".dat") Then
                fs.delete_file(dat_file)
            End If
            sdoc.title = title
            sdoc.id = master_doc.id
            sdoc.taxonomy = master_doc.taxonomy

            sdoc.disclosures = master_doc.disclosures
            sdoc.authors = master_doc.authors
            'sdoc.write_data_to_file(dat_file)
            REM refresh it
            odoc.visible(False)
            'Dim obc_refresh As New bc_am_at_component_refresh(False, odoc.worddocument, "word", 1, True)
            Dim obc_refresh As New bc_am_at_component_refresh(odoc)
            sdoc.refresh_components.from_stage_change = True
            obc_refresh.refresh(False, 2, False, sdoc, False, "", True)

            Clipboard.Clear()
            REM save and close
            odoc.save()
            odoc.close()
            odoc.quit_if_no_docs()

            bd.odoc = Nothing

            filename = bc_cs_central_settings.local_repos_path + bc_am_load_objects.obc_current_document.filename + bc_am_load_objects.obc_current_document.extension

            REM see if refresh sequence errored

            If sdoc.refresh_components.stage_change_error_text <> "" Then
                err_text = sdoc.refresh_components.stage_change_error_text
                Dim oocomm As New bc_cs_activity_log("bc_am_build_automated_support_doc", "create", bc_cs_activity_codes.COMMENTARY, "Error refreshing document: " + err_text, Nothing)
                Exit Function
            Else
                sdoc.id = 0
                sdoc.title = "Disclosures"
                sdoc.master_flag = False
                sdoc.pub_type_id = master_doc.pub_type_id
                sdoc.pub_type_name = master_doc.pub_type_name

                sdoc.originating_author = bc_cs_central_settings.logged_on_user_id
                sdoc.bus_area = master_doc.bus_area
                sdoc.checked_out_user = 0
                sdoc.stage = master_doc.stage
                sdoc.doc_date = Now.ToUniversalTime

                fs.write_document_to_bytestream(filename, sdoc.byteDoc, Nothing)

                master_doc.support_documents.Clear()
                master_doc.support_documents.Add(sdoc)
                master_doc.register_only = False
                master_doc.bcheck_out = False
                master_doc.bwith_document = False
                master_doc.history.Clear()
                master_doc.bimport_support_only = True
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    master_doc.db_write(Nothing)
                Else
                    master_doc.tmode = bc_cs_soap_base_class.tWRITE
                    If master_doc.transmit_to_server_and_receive(master_doc, True) = False Then
                        Exit Function
                    End If
                End If
                If master_doc.doc_write_success = False Then
                    err_text = "Failed to upload disclosure file: " + master_doc.doc_write_error_text
                End If
            End If


            REM remove it
            If fs.check_document_exists(filename) = True Then
                fs.delete_file(filename)
            End If

            create_doc_from_process_toolbar = True
        Catch ex As Exception
            err_text = ex.Message
            Dim ocomm = New bc_cs_activity_log("bc_am_build_automated_support_document", "create_doc_from_process_toolbar", bc_cs_activity_codes.COMMENTARY, ex.Message)
        End Try

    End Function



    Public Function create_doc(copy_disclosures_class_id) As Boolean
        Try
            Dim filename As String
            create_doc = False
            REM create document
            Dim bd As New bc_am_at_build_document(template_name, master_doc.pub_type_id, master_doc.entity_id, Nothing, 0)

            REM attach to it
            If bc_am_load_objects.obc_current_document.extension <> ".docx" Then
                Dim ocomm = New bc_cs_activity_log("bc_am_build_automated_support_document", "create", bc_cs_activity_codes.COMMENTARY, "Mime type:" + bc_am_load_objects.obc_current_document.extension + " not supported for action")
                Exit Function
            End If

            odoc = bd.odoc

            REM configure the metadata for it
            Dim dat_file As String
            dat_file = bc_cs_central_settings.local_repos_path + bc_am_load_objects.obc_current_document.filename + ".dat"
            Dim sdoc As New bc_om_document
            sdoc = sdoc.read_data_from_file(dat_file)
            Dim fs As New bc_cs_file_transfer_services

            If fs.check_document_exists(bc_cs_central_settings.local_repos_path + bc_am_load_objects.obc_current_document.filename + ".dat") Then
                fs.delete_file(dat_file)
            End If
            sdoc.title = title
            sdoc.id = master_doc.id
            sdoc.taxonomy = master_doc.taxonomy
            If copy_disclosures_class_id > 0 Then

                master_doc.disclosures.Clear()

                For i = 0 To sdoc.taxonomy.Count - 1
                    If sdoc.taxonomy(i).class_id = copy_disclosures_class_id Then
                        master_doc.disclosures.Add(sdoc.taxonomy(i))
                    End If
                Next
            End If


            sdoc.disclosures = master_doc.disclosures


            sdoc.authors = master_doc.authors
            'sdoc.write_data_to_file(dat_file)
            REM refresh it
            odoc.visible(False)
            'Dim obc_refresh As New bc_am_at_component_refresh(False, odoc.worddocument, "word", 1, True)
            Dim obc_refresh As New bc_am_at_component_refresh(odoc)
            sdoc.refresh_components.from_stage_change = True

            obc_refresh.refresh(False, 2, False, sdoc, False, "", True)



            Clipboard.Clear()
            REM save and close
            odoc.save()
            odoc.close()
            odoc.quit_if_no_docs()

            bd.odoc = Nothing

            filename = bc_cs_central_settings.local_repos_path + bc_am_load_objects.obc_current_document.filename + bc_am_load_objects.obc_current_document.extension

            REM see if refresh sequence errored

            If sdoc.refresh_components.stage_change_error_text <> "" Then
                err_text = sdoc.refresh_components.stage_change_error_text
                Dim oocomm As New bc_cs_activity_log("bc_am_build_automated_support_doc", "create", bc_cs_activity_codes.COMMENTARY, "Error refreshing document: " + err_text, Nothing)

            Else
                REM import it as a support document
                sdoc.id = 0
                sdoc.master_flag = False
                sdoc.doc_date = Now.ToUniversalTime
                sdoc.from_event = True
                sdoc.new_doc = True
                fs.write_document_to_bytestream(filename, sdoc.byteDoc, Nothing)
                master_doc.support_documents.Add(sdoc)
                create_doc = True
            End If


            REM remove it
            If fs.check_document_exists(filename) = True Then
                fs.delete_file(filename)
            End If


        Catch ex As Exception
            err_text = ex.Message
            Dim ocomm = New bc_cs_activity_log("bc_am_build_automated_support_document", "create", bc_cs_activity_codes.COMMENTARY, ex.Message)
        End Try

    End Function


End Class
Public Class bc_am_pp_componetize
    Public err_text As String
    Dim bc_ao_obj As bc_ao_at_object
    Dim _ldoc As bc_om_document
    Dim _obj As Object


    Public Function componetize_non_bc(ByRef ldoc As bc_om_document) As Boolean
        Try
            Dim cdoc As New bc_om_document
            cdoc.id = ldoc.id
            cdoc.filename = ldoc.filename
            cdoc.stage = ldoc.stage
            cdoc.extension = ldoc.extension

            _ldoc = ldoc
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                cdoc.db_check_out_non_create_doc()
            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                cdoc.tmode = bc_cs_soap_base_class.tREAD
                cdoc.read_mode = bc_om_document.CHECK_OUT_NON_CREATE_DOC
                cdoc.transmit_to_server_and_receive(cdoc, True)
            End If


            If ldoc.doc_not_found Then
                err_text = "Document not found on server"
                Exit Function
            End If


            REM can only quick open a word document
            REM save as view only file
            If saveEditFile(cdoc) <> "" Then
                If set_ao_obj() = True Then
                    bc_ao_obj.open(bc_cs_central_settings.local_repos_path + ldoc.filename, False, False)
                    componetize_non_bc = bc_ao_obj.componetize_powerpoint(ldoc)
                    '
                    bc_ao_obj.close()
                    bc_ao_obj.quit_if_no_docs()

                    Dim fs As New bc_cs_file_transfer_services
                    fs.delete_file(bc_cs_central_settings.local_repos_path + ldoc.filename)

                    '


                End If
            End If
        Catch ex As Exception
            err_text = ex.Message
            componetize_non_bc = False
            Dim ocomm = New bc_cs_activity_log("bc_am_pp_componetize", "componetize_non_bc", bc_cs_activity_codes.COMMENTARY, ex.Message)
        End Try
    End Function
    Private Function set_ao_obj() As Boolean
        Try

            Select Case _ldoc.extension
                Case ".docx", "[imp].docx", "[imp].docm"
                    If IsNothing(_obj) Then
                        bc_ao_obj = New bc_ao_word()
                    Else
                        bc_ao_obj = New bc_ao_word(_obj)
                    End If
                Case ".pptx", "[imp].pptx", "[imp].pptm"
                    If IsNothing(_obj) Then
                        bc_ao_obj = New bc_ao_powerpoint
                    Else
                        bc_ao_obj = New bc_ao_powerpoint(_obj)
                    End If

                Case "[imp].xlsx", "[imp].xlsm", "[imp].xls"
                    bc_ao_obj = New bc_ao_excel
                Case Else
                    set_ao_obj = False
                    Exit Function
            End Select

            set_ao_obj = True
        Catch ex As Exception
            err_text = ex.Message
            Dim ocomm = New bc_cs_activity_log("bc_am_ render_non_bc_doc", "render", bc_cs_activity_codes.COMMENTARY, ex.Message)
        End Try
    End Function
    Private Function saveEditFile(ByVal ldoc As bc_om_document) As String
        Dim slog As New bc_cs_activity_log("bc_am_process", "save_edit_file", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim fs As New bc_cs_file_transfer_services
            Dim fn As String
            Dim i As Integer = 0

            'fn = CStr(ldoc.id) + ldoc.extension
            fn = ldoc.filename
            fs.write_bytestream_to_document(bc_cs_central_settings.local_repos_path + fn, ldoc.byteDoc, Nothing)
            saveEditFile = fn
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_process", "save_edit_file", bc_cs_error_codes.USER_DEFINED, ex.Message)
            saveEditFile = ""
        Finally
            slog = New bc_cs_activity_log("bc_am_process", "save_edit_file", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function
End Class

Public Class bc_am_render
    Public err_text As String
    Dim bc_ao_obj As bc_ao_at_object
    Dim _ldoc As bc_om_document
    Dim _obj As Object
    Public Function render_bc(ByRef ldoc As bc_om_document, obj As Object) As Boolean
        Try
            _ldoc = ldoc
            _obj = obj
            render_bc = False
            If set_ao_obj() = False Then
                Dim ocomm = New bc_cs_activity_log("bc_am_ render", "render_bc", bc_cs_activity_codes.COMMENTARY, "cant get ao object")
                Exit Function
            End If

            Dim pdf_fn As String
            REM can only quick open a word document
            REM save as view only file
            pdf_fn = bc_cs_central_settings.local_repos_path + CStr(_ldoc.id) + ".pdf"
            err_text = bc_ao_obj.rendertoPDF_With_office(pdf_fn)
            If err_text <> "" Then
                Exit Function
            End If
            REM import it as a support document
            Dim sdoc As New bc_om_document
            sdoc.id = 0
            sdoc.title = _ldoc.title + ".pdf"
            sdoc.pub_type_id = ldoc.pub_type_id
            sdoc.originating_author = bc_cs_central_settings.logged_on_user_id
            sdoc.extension = ".pdf"
            sdoc.master_flag = False
            sdoc.doc_date = Now.ToUniversalTime
            sdoc.from_event = True
            sdoc.new_doc = True
            Dim fs As New bc_cs_file_transfer_services

            fs.write_document_to_bytestream(pdf_fn, sdoc.byteDoc, Nothing)
            _ldoc.support_documents.Add(sdoc)

            REM remove it
            If fs.check_document_exists(pdf_fn) = True Then
                fs.delete_file(pdf_fn)
            End If

            render_bc = True

        Catch ex As Exception
            err_text = ex.Message
            Dim ocomm = New bc_cs_activity_log("bc_am_ render", "render_bc", bc_cs_activity_codes.COMMENTARY, ex.Message)
        End Try
    End Function


    Public Function render_non_bc(ByRef ldoc As bc_om_document) As Boolean
        Try
            Dim cdoc As New bc_om_document
            cdoc.id = ldoc.id
            cdoc.filename = ldoc.filename
            cdoc.stage = ldoc.stage
            cdoc.extension = ldoc.extension

            _ldoc = ldoc
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                cdoc.db_check_out_non_create_doc()
            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                cdoc.tmode = bc_cs_soap_base_class.tREAD
                cdoc.read_mode = bc_om_document.CHECK_OUT_NON_CREATE_DOC
                cdoc.transmit_to_server_and_receive(cdoc, True)
            End If


            If ldoc.doc_not_found Then
                err_text = "Document not found on server"
                Exit Function
            End If


            REM can only quick open a word document
            REM save as view only file
            If saveEditFile(cdoc) <> "" Then
                If set_ao_obj() = True Then
                    bc_ao_obj.open(bc_cs_central_settings.local_repos_path + _ldoc.filename, False, False)
                    Dim pdf_fn As String
                    pdf_fn = bc_cs_central_settings.local_repos_path + CStr(_ldoc.id) + ".pdf"
                    err_text = bc_ao_obj.rendertoPDF_With_office(pdf_fn)

                    If err_text <> "" Then
                        Exit Function
                    End If
                    bc_ao_obj.close()
                    bc_ao_obj.quit_if_no_docs()

                    Dim fs As New bc_cs_file_transfer_services
                    fs.delete_file(bc_cs_central_settings.local_repos_path + ldoc.filename)

                    REM import it as a support document
                    Dim sdoc As New bc_om_document
                    sdoc.id = 0
                    sdoc.title = _ldoc.title + ".pdf"
                    sdoc.pub_type_id = ldoc.pub_type_id
                    sdoc.originating_author = bc_cs_central_settings.logged_on_user_id
                    sdoc.extension = ".pdf"
                    sdoc.master_flag = False
                    sdoc.doc_date = Now.ToUniversalTime
                    sdoc.from_event = True
                    sdoc.new_doc = True
                    fs.write_document_to_bytestream(pdf_fn, sdoc.byteDoc, Nothing)
                    _ldoc.support_documents.Add(sdoc)
                    REM remove it
                    If fs.check_document_exists(pdf_fn) = True Then
                        fs.delete_file(pdf_fn)
                    End If

                    render_non_bc = True
                End If
            End If
        Catch ex As Exception
            err_text = ex.Message
            Dim ocomm = New bc_cs_activity_log("bc_am_ render", "render_non_bc", bc_cs_activity_codes.COMMENTARY, ex.Message)
        End Try
    End Function
    Private Function set_ao_obj() As Boolean
        Try

            Select Case _ldoc.extension
                Case ".docx", "[imp].docx", "[imp].docm", "[imp].doc"
                    If IsNothing(_obj) Then
                        bc_ao_obj = New bc_ao_word()
                    Else
                        bc_ao_obj = New bc_ao_word(_obj)
                    End If
                Case ".pptx", "[imp].pptx", "[imp].pptm"
                    If IsNothing(_obj) Then
                        bc_ao_obj = New bc_ao_powerpoint
                    Else
                        bc_ao_obj = New bc_ao_powerpoint(_obj)
                    End If

                Case "[imp].xlsx", "[imp].xlsm", "[imp].xls"
                    bc_ao_obj = New bc_ao_excel
                    If IsNothing(_obj) Then
                        bc_ao_obj = New bc_ao_excel
                    Else
                        bc_ao_obj = New bc_ao_excel(_obj)
                    End If

                Case Else
                    set_ao_obj = False
                    Exit Function
            End Select

            set_ao_obj = True
        Catch ex As Exception
            err_text = ex.Message
            Dim ocomm = New bc_cs_activity_log("bc_am_ render_non_bc_doc", "render", bc_cs_activity_codes.COMMENTARY, ex.Message)
        End Try
    End Function
    Private Function saveEditFile(ByVal ldoc As bc_om_document) As String
        Dim slog As New bc_cs_activity_log("bc_am_process", "save_edit_file", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim fs As New bc_cs_file_transfer_services
            Dim fn As String
            Dim i As Integer = 0

            'fn = CStr(ldoc.id) + ldoc.extension
            fn = ldoc.filename
            fs.write_bytestream_to_document(bc_cs_central_settings.local_repos_path + fn, ldoc.byteDoc, Nothing)
            saveEditFile = fn
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_process", "save_edit_file", bc_cs_error_codes.USER_DEFINED, ex.Message)
            saveEditFile = ""
        Finally
            slog = New bc_cs_activity_log("bc_am_process", "save_edit_file", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function
End Class
Public Class insert_excel_disclosures
    Dim err_text As String

    Public Function run(ldoc As bc_om_document, oexcel As bc_ao_excel) As Boolean
        run = False
        Try
            ldoc.refresh_components.refresh_components.Clear()
            Dim rc As bc_om_refresh_component
            Dim worksheet As String
            Dim start_row As Integer
            Dim start_col As Integer
            Dim chart_row_gap As Integer
            Dim final_worksheet_selected As String

            REM get disclosure components
            Dim osql As New bc_om_sql("exec dbo.bc_custom_get_excel_disclosure_components")
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                osql.db_read()
            Else
                osql.tmode = bc_cs_soap_base_class.tREAD
                If osql.transmit_to_server_and_receive(osql, True) = False Then
                    err_text = "server error retrieving disclosure components: exec dbo.bc_custom_get_excel_disclosure_components"
                    Exit Function
                End If
            End If
            If osql.success = False Then
                err_text = "failed to retrieve disclosure components: exec dbo.bc_custom_get_excel_disclosure_components"
                Exit Function
            End If
            ldoc.refresh_components.refresh_type = 2
            ldoc.refresh_components.doc_id = ldoc.id
            ldoc.refresh_components.data_at_date = "9-9-9999"
            ldoc.refresh_components.workflow_state = 8
            ldoc.refresh_components.assoc_entities = ldoc.disclosures


            For i = 0 To UBound(osql.results, 2)

                REM first row is positional information
                If i = 0 Then
                    worksheet = osql.results(0, 0)
                    start_row = osql.results(1, 0)
                    start_col = osql.results(2, 0)
                    chart_row_gap = osql.results(4, 0)
                    final_worksheet_selected = osql.results(6, 0)

                Else
                    REM refresh components metadata
                    rc = New bc_om_refresh_component
                    rc.mode = osql.results(0, i)
                    rc.type = osql.results(1, i)
                    rc.mode_param1 = osql.results(2, i)
                    rc.mode_param2 = osql.results(3, i)
                    rc.entity_id = ldoc.entity_id
                    rc.refresh_type = 2
                    ldoc.refresh_components.refresh_components.Add(rc)
                End If

            Next
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                ldoc.refresh_components.db_read()
            Else
                ldoc.refresh_components.tmode = bc_cs_soap_base_class.tREAD
                If ldoc.refresh_components.transmit_to_server_and_receive(ldoc.refresh_components, False) = False Then
                    err_text = "server error refreshing disclosure components"
                    Exit Function
                End If
            End If

            REM populate excel
            run = oexcel.set_component_markup_disclosures(ldoc.refresh_components, worksheet, start_row, start_col, chart_row_gap, final_worksheet_selected)
           
        Catch ex As Exception
            err_text = ex.Message
        Finally
            If err_text <> "" Then
                Dim ocomm As New bc_cs_activity_log("insert_excel_disclosures", "run", bc_cs_activity_codes.COMMENTARY, "Failed: " + err_text)
            End If

        End Try

    End Function

End Class
