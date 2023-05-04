Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports BlueCurve.Core.AS
Imports System.Windows.Forms
Imports System.Collections

REM FIL AUG 2013
REM AUTO AUG 2013
Public Class bc_am_import_insight_model
    Public onewdoc As bc_om_document

    Public Function show_dialogue(ByVal entity_id As Long, ByVal pub_type_id As Long, ByVal fn As String, ByVal title As String, ByVal extension As String) As Boolean
        Dim slog As New bc_cs_activity_log("bc_am_workflow", "import_insight_model", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            Dim i As Integer

            Dim pub_type_found As Boolean = False

            Dim fs As New bc_cs_file_transfer_services

            'Dim osubmit As bc_am_at_submit_frm
            Dim osubmit As bc_dx_as_categorize
            Dim ocommentary As bc_cs_activity_log
            show_dialogue = False
            For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                If bc_am_load_objects.obc_pub_types.pubtype(i).id = pub_type_id Then
                    pub_type_found = True
                    With bc_am_load_objects.obc_pub_types.pubtype(i)
                        onewdoc = New bc_om_document(0, entity_id, CStr(bc_cs_central_settings.logged_on_user_id), pub_type_id, "", "", Now, 0, .language, bc_cs_central_settings.selected_conn_method, "", .name, 0, False, "1", "", .bus_area_id, True, 0, "Draft", "9-9-9999", "", 0)
                        'If sub_entity_id <> 0 Then
                        '    Dim oent As New bc_om_taxonomy
                        '    oent.entity_id = sub_entity_id
                        '    onewdoc.taxonomy.Add(oent)
                        'End If
                        onewdoc.refresh_components.workflow_state = bc_am_load_objects.obc_pub_types.pubtype(i).default_financial_workflow_stage
                        onewdoc.refresh_components.accounting_standard = 1
                        onewdoc.originating_author = bc_cs_central_settings.logged_on_user_id

                        If bc_am_load_objects.obc_pub_types.pubtype(i).workflow.stages.count > 0 Then
                            onewdoc.stage = bc_am_load_objects.obc_pub_types.pubtype(i).workflow.stages(0).stage_id
                            onewdoc.stage_name = bc_am_load_objects.obc_pub_types.pubtype(i).workflow.stages(0).stage_name
                        End If
                        onewdoc.workflow_stages = bc_am_load_objects.obc_pub_types.pubtype(i).workflow

                        REM show submit disalogue
                        REM PR Feb 2016 move to dev express and UTC

                      

                        REM DEV EXPRESS
                        Dim fdxsubmit As New bc_dx_as_categorize
                        fdxsubmit.document = onewdoc
                        fdxsubmit.create_mode = True
                        fdxsubmit.show_stage_change = True
                        fdxsubmit.show_local_submit = True

                        fdxsubmit.enable_pub_types = False
                        fdxsubmit.enable_lead_entity = False

                        Dim pts As New List(Of String)
                        pts.Add(onewdoc.pub_type_name)
                        fdxsubmit.set_pub_types = pts
                        onewdoc.title = title
                        fdxsubmit.ShowDialog()

                        If Not fdxsubmit.ok_selected Then
                            REM if cancel selected do nothing
                            ocommentary = New bc_cs_activity_log("bc_am_workflow", "import_insight_model", bc_cs_activity_codes.COMMENTARY, "Cancel Selected on Submit form.")
                            Exit Function
                        End If
                        show_dialogue = True
                        onewdoc.filename = fn


                       

                        REM mark document as imported 
                        onewdoc.extension = "[imp]." + extension

                        show_dialogue = True
                    End With
                    Exit For
                End If
            Next

            If pub_type_found = False Then
                Dim omessage As New bc_cs_message("Blue Curve", "Invalid pub type id supplied: " + CStr(pub_type_id), bc_cs_message.MESSAGE)
                Exit Function
            End If


        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_workflow", "import_insight_model", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_workflow", "import_insight_model", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function
    Public Function upload_model() As Boolean
        Dim slog As New bc_cs_activity_log("bc_am_workflow", "upload_model", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            upload_model = False
            REM suck file up into document object
            Dim fs As New bc_cs_file_transfer_services

            If fs.check_document_exists(onewdoc.filename) = False Then
                Dim omessage As New bc_cs_message("Blue Curve", "Document cant be accessed import aborted", bc_cs_message.MESSAGE)
                Exit Function
            End If
            fs.write_document_to_bytestream(onewdoc.filename, onewdoc.byteDoc, Nothing)

            onewdoc.bwith_document = True

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                onewdoc.db_write(Nothing)
            Else
                onewdoc.tmode = bc_cs_soap_base_class.tWRITE
                If onewdoc.transmit_to_server_and_receive(onewdoc, True) = False Then
                    Exit Function
                End If
            End If

            If onewdoc.doc_write_error_text <> "" Then
                Dim omsg As New bc_cs_message("Blue Curve", "Import Insight Model Failed: " + onewdoc.doc_write_error_text, bc_cs_message.MESSAGE, False, False, "yes", "no", True)
            Else
                upload_model = True
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_workflow", "upload_model", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_workflow", "upload_model", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
End Class


REM EFG JUne 2012
Public Class bc_am_copy_document
    REM metadata document
    Private mdoc_id As Long
    REM physical document
    Private sdoc_id As Long
    Private ao_obj As bc_ao_at_object

    Public Sub New(ByVal mdoc_id As Long, ByVal sdoc_id As Long)
        Me.mdoc_id = mdoc_id
        Me.sdoc_id = sdoc_id
    End Sub

    Public Function copy_doc() As Boolean
        Dim otrace As New bc_cs_activity_log("bc_am_copy_doc", "copy_doc", bc_cs_activity_codes.TRACE_ENTRY, "")
        bc_cs_central_settings.progress_bar.unload()
        bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve", "Creating a clone of the document...", 0, False, True)

        Try
            REM get metadata from master document
            Dim mdoc As New bc_om_document
            mdoc.id = Me.mdoc_id
            If mdoc_id <> sdoc_id Then
                mdoc.bwith_document = False
            Else
                mdoc.bwith_document = True
            End If
            mdoc.btake_revision = False
            mdoc.bcheck_out = False

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                mdoc.db_read_for_create()
            Else
                mdoc.tmode = bc_cs_soap_base_class.tREAD
                mdoc.read_mode = bc_om_document.READ_FOR_CREATE
                If mdoc.transmit_to_server_and_receive(mdoc, True) = False Then
                    Return False
                End If
            End If

            bc_cs_central_settings.progress_bar.increment("Retrieving metadata...")
            bc_cs_central_settings.progress_bar.increment("Retrieving metadata...")
            bc_cs_central_settings.progress_bar.refresh()
            REM generate new id
            REM reset stage id and originator in metadate
            mdoc.id = 0
            mdoc.stage = 1
            mdoc.stage_name = "Draft"
            mdoc.originating_author = bc_cs_central_settings.logged_on_user_id
            mdoc.doc_date = Now
            mdoc.bwith_document = True

            REM now get physical document

            Dim sdoc As New bc_om_document
            sdoc.id = Me.sdoc_id
            sdoc.bwith_document = True
            sdoc.btake_revision = False
            sdoc.bcheck_out = False


            If mdoc_id <> sdoc_id Then
                bc_cs_central_settings.progress_bar.increment("Retrieving document...")
                bc_cs_central_settings.progress_bar.increment("Retrieving document...")
                bc_cs_central_settings.progress_bar.refresh()
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    sdoc.db_read_for_create()
                Else
                    sdoc.tmode = bc_cs_soap_base_class.tREAD
                    sdoc.read_mode = bc_om_document.READ_FOR_CREATE
                    If sdoc.transmit_to_server_and_receive(sdoc, True) = False Then
                        Return False
                    End If
                End If
            Else
                sdoc = mdoc
            End If

            REM make sure its a supported mime type
            If sdoc.extension = ".doc" Or sdoc.extension = ".docx" Then
                ao_obj = New bc_ao_word
            ElseIf sdoc.extension = ".ppt" Or sdoc.extension = ".pptx" Then
                ao_obj = New bc_ao_powerpoint

            Else
                Dim omsg As New bc_cs_message("Blue Curve", "Mime type: " + sdoc.extension + " not supported in copy doc", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Return False
            End If

            mdoc.filename = ao_obj.generate_local_filename_prefix

            REM now change extension
            mdoc.extension = sdoc.extension
            REM reset workflow
            For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                If bc_am_load_objects.obc_pub_types.pubtype(i).id = mdoc.pub_type_id Then
                    mdoc.workflow_stages = bc_am_load_objects.obc_pub_types.pubtype(i).workflow
                    Exit For
                End If
            Next


            If IsNothing(sdoc.byteDoc) Then
                Dim db_err As New bc_cs_error_log("bc_am_copy_doc", "copy_doc", bc_cs_error_codes.USER_DEFINED, "Document not found on server")
                copy_doc = False
                Exit Function
            End If

            REM write file down
            Dim fs As New bc_cs_file_transfer_services
            If fs.write_bytestream_to_document(bc_cs_central_settings.local_repos_path + mdoc.filename + mdoc.extension, sdoc.byteDoc, Nothing) = False Then
                Return False
            End If
            mdoc.connection_method = bc_cs_central_settings.selected_conn_method


            REM clear up some data
            REM support docs
            mdoc.support_documents.Clear()
            mdoc.urgent_flag = False
            mdoc.urgent_text = ""
            mdoc.behalf_of_author_id = 0
            mdoc.oec.Lists.Clear()





            mdoc.write_data_to_file(bc_cs_central_settings.local_repos_path + mdoc.filename + ".dat")
            bc_cs_central_settings.progress_bar.increment("Opening new document...")
            bc_cs_central_settings.progress_bar.increment("Opening new document...")
            bc_cs_central_settings.progress_bar.refresh()


            REM open document
            ao_obj.open(bc_cs_central_settings.local_repos_path + mdoc.filename + mdoc.extension, True, False)
            REM reset properies
            ao_obj.set_doc_id(mdoc.filename)
            ao_obj.set_parameter("rnet_analyst_id", CStr(bc_cs_central_settings.logged_on_user_id))
            copy_doc = True
        Catch ex As Exception
            copy_doc = False
            Dim db_err As New bc_cs_error_log("bc_am_copy_doc", "copy_doc", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            bc_cs_central_settings.progress_bar.hide()
            bc_cs_central_settings.progress_bar.unload()
            otrace = New bc_cs_activity_log("bc_am_copy_doc", "copy_doc", bc_cs_activity_codes.TRACE_EXIT, "")

        End Try


    End Function

End Class

REM ==========================================
REM Blue Curve Limited 2005
REM Module:       Build
REM Type:         Application Module
REM Description:  Builds a Document irrespective of output format
REM Version:      1
REM Change history
REM ==========================================
Public Class bc_am_at_build_document
    Public Const WORD_EXT = ".dot"
    Public Const POWERPOINT_EXT = ".pot"
    Private Const WORD2007_EXT = ".dotm"
    Private Const POWERPOINT2007_EXT = ".potm"

    Public odoc As bc_ao_at_object

    Public entity_id As Long
    Public pub_type_id As Long
    Public language_id As Long
    Public sub_entity_id As Long
    Public default_financial_workflow_stage As Long


   
    Public Sub New()

    End Sub
    REM constructor assumes templates have already been loaded by calling AP
    REM passs in name of template to build, pub type id of product and entity it is for
    Public Sub New(ByVal name As String, ByVal lpub_type_id As Long, ByVal lentity_id As Long, ByVal params As bc_om_parameters, ByVal lsub_entity_id As Long)
        Dim otrace As New bc_cs_activity_log("bc_am_at_build_document", "new", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim i As Integer

        Try
           
          
            

            REM fill out document metadata information
            entity_id = lentity_id
            pub_type_id = lpub_type_id
            sub_entity_id = lsub_entity_id
            set_doc_metadata()



            bc_cs_central_settings.progress_bar.increment("Setting Document Metadata...")
            REM get filename for name and then 
            For i = 0 To bc_am_load_objects.obc_templates.template.Count - 1
                If bc_am_load_objects.obc_templates.template(i).name = name Then
                    build_from_template(bc_am_load_objects.obc_templates.template(i), bc_am_load_objects.obc_doc_properties, params)
                End If
            Next
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_at_build_document", "n ", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_am_at_build_document", "new", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    REM sets uo some of the document metadata in a document object
    Public Sub set_doc_metadata()
        Dim otrace As New bc_cs_activity_log("bc_am_at_build_document", "set_doc_metadata", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim i As Integer
        Dim pub_type_name As String = ""
        Dim bus_area_id As String = ""
        Dim lclass_id As Long
        Dim logged_on_user As String

        logged_on_user = bc_cs_central_settings.GetLoginName
        Try
            If bc_cs_central_settings.logged_on_user_id = 0 Then
                REM local working but still evaluate who si connected
                For i = 0 To bc_am_load_objects.obc_users.user.Count - 1
                    With bc_am_load_objects.obc_users.user(i)
                        If logged_on_user = .os_user_name Then
                            bc_cs_central_settings.logged_on_user_id = .id
                            Exit For
                        End If
                    End With
                Next
            End If
            REM get language id from pub type
            For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                If bc_am_load_objects.obc_pub_types.pubtype(i).id = pub_type_id Then
                    language_id = bc_am_load_objects.obc_pub_types.pubtype(i).language
                    pub_type_name = bc_am_load_objects.obc_pub_types.pubtype(i).name
                    bus_area_id = bc_am_load_objects.obc_pub_types.pubtype(i).bus_area_Id
                    default_financial_workflow_stage = bc_am_load_objects.obc_pub_types.pubtype(i).default_financial_workflow_stage
                    Exit For
                End If
            Next
            REM disclosures only if product entity can be propogated to a company

            Dim disclosure_id As Long
            Dim company_class_id As Long
            Dim j As Integer
            For j = 0 To bc_am_load_objects.obc_entities.entity.Count - 1
                If bc_am_load_objects.obc_entities.entity(j).class_name = "Company" Or Right(bc_am_load_objects.obc_entities.entity(j).class_name, 7) = "Company" Then
                    company_class_id = bc_am_load_objects.obc_entities.entity(j).class_id
                End If

                If bc_am_load_objects.obc_entities.entity(j).id = entity_id Then
                    lclass_id = bc_am_load_objects.obc_entities.entity(j).class_id
                End If
            Next
            disclosure_id = entity_id
            REM if class of discloures entities is different to doc class then progate to company
            If bc_am_load_objects.obc_entities.disclosure_entities.Count > 0 Then
                If bc_am_load_objects.obc_entities.disclosure_entities(0).class_id <> lclass_id Then
                    disclosure_id = bc_am_load_objects.obc_entity_links.get_entity_id_for_class_hierarchy(entity_id, company_class_id, bc_am_load_objects.obc_entities)
                End If
            End If

            REM BMO Sep 2012
            REM sub entity id
            Dim sdisclosure_Id As Long = 0
            If sub_entity_id <> 0 Then
                sdisclosure_Id = sub_entity_id
                If bc_am_load_objects.obc_entities.disclosure_entities.Count > 0 Then
                    If bc_am_load_objects.obc_entities.disclosure_entities(0).class_id <> lclass_id Then
                        sdisclosure_Id = bc_am_load_objects.obc_entity_links.get_entity_id_for_class_hierarchy(sub_entity_id, company_class_id, bc_am_load_objects.obc_entities)
                    End If
                End If
            End If
            REM ========================


            REM PR took this out as doesnt make any sense MAY 2016
            REM FIL DROP 5 changed this as it swicthes the user back if a valid os logon is found but authentication prompt has been set
            'If bc_cs_central_settings.authentication_method = 0 Then

            '    Dim k As Integer
            '    For k = 0 To bc_am_load_objects.obc_users.user.Count - 2
            '        With bc_am_load_objects.obc_users.user(k)
            '            If UCase(Trim(.os_user_name)) = UCase(Trim(logged_on_user)) Then
            '                bc_cs_central_settings.logged_on_user_id = .id
            '                Exit For
            '            End If
            '        End With
            '    Next
            'End If

            REM END FIL DROP 5


            bc_am_load_objects.obc_current_document = New bc_om_document(0, entity_id, CStr(bc_cs_central_settings.logged_on_user_id), pub_type_id, "", "", Now, 0, 1, bc_cs_central_settings.selected_conn_method, "", pub_type_name, 0, False, "1", "", bus_area_id, True, 0, "Draft", "9-9-9999", "", disclosure_id)
            REM BMO Sep 2012
            REM sub entity id
            If sdisclosure_Id <> 0 Then
                Dim odisc = New bc_om_taxonomy(sdisclosure_Id)
                bc_am_load_objects.obc_current_document.disclosures.Add(odisc)
            End If
            REM =======================

            REM get default disclosues tabe setting
            bc_am_load_objects.obc_current_document.allow_disclosures = False

            For j = 0 To bc_am_load_objects.obc_prefs.stage_role_access_codes.Count - 1

                If bc_am_load_objects.obc_prefs.stage_role_access_codes(j).stage_id = 1 AndAlso bc_am_load_objects.obc_prefs.stage_role_access_codes(j).access_id = "D" Then
                    bc_am_load_objects.obc_current_document.allow_disclosures = True
                    Exit For
                End If
            Next

            bc_am_load_objects.obc_current_document.refresh_components.workflow_state = default_financial_workflow_stage
            bc_am_load_objects.obc_current_document.refresh_components.accounting_standard = 1
            If bc_am_load_objects.obc_pub_types.pubtype(i).workflow.stages.count > 0 Then
                bc_am_load_objects.obc_current_document.stage = bc_am_load_objects.obc_pub_types.pubtype(i).workflow.stages(0).stage_id
                bc_am_load_objects.obc_current_document.stage_name = bc_am_load_objects.obc_pub_types.pubtype(i).workflow.stages(0).stage_name
            End If
            bc_am_load_objects.obc_current_document.workflow_stages = bc_am_load_objects.obc_pub_types.pubtype(i).workflow
            bc_am_load_objects.obc_current_document.language_id = bc_am_load_objects.obc_pub_types.pubtype(i).language
            For i = 0 To bc_am_load_objects.obc_users.user.Count - 1
                With bc_am_load_objects.obc_users.user(i)
                    If UCase(Trim(.os_user_name)) = UCase(Trim(bc_cs_central_settings.logged_on_user_name)) Then
                        bc_am_load_objects.obc_current_document.user_role = Trim(.role)
                        REM display other taxonomy or not
                        bc_am_load_objects.obc_current_document.do_not_show_taxonomy = .no_display_taxonomy
                        Exit For
                    End If
                End With
            Next
            REM display other taxonomy or not
            Dim default_entity As New bc_om_taxonomy
            default_entity.entity_id = sub_entity_id
            bc_am_load_objects.obc_current_document.taxonomy.Add(default_entity)

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_at_build_document", "set_doc_metadata", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_am_at_build_document", "set_doc_metadata", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    REM pass in actual template object to build
    Private Sub build_from_template(ByRef otemplate As bc_om_template, ByRef oproperties As bc_om_doc_properties, ByVal params As bc_om_parameters)
        Dim otrace As New bc_cs_activity_log("bc_am_at_build_document", "build_from_template", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim cancel As Boolean = False
        Try
            Dim strmimetype As String
            Dim extensionsize As Integer


            REM evaluate MIME type and bind to correct AO library


            REM SW cope with office versions
            extensionsize = 0
            extensionsize = (Len(otemplate.filename) - (InStrRev(otemplate.filename, ".") - 1))

            strmimetype = Right(otemplate.filename, extensionsize)
            Dim i As Integer
            REM if user has access locked then lock document
            bc_am_load_objects.obc_current_document.locked = False
            For i = 0 To bc_am_load_objects.obc_prefs.stage_role_access_codes.Count - 1
                If bc_am_load_objects.obc_prefs.stage_role_access_codes(i).stage_id = 1 And bc_am_load_objects.obc_prefs.stage_role_access_codes(i).access_id = "L" Then
                    bc_am_load_objects.obc_current_document.locked = True
                    Exit For
                End If
            Next


            REM AO parts
            If strmimetype = WORD_EXT Or strmimetype = WORD2007_EXT Then

                REM SW cope with office versions
                If bc_cs_central_settings.userOfficeStatus = 2 Then
                    bc_am_load_objects.obc_current_document.extension = ".docx"
                Else
                    bc_am_load_objects.obc_current_document.extension = ".doc"
                End If
                'If bc_cs_central_settings.office_version < 12 And bc_cs_central_settings.user_office_version >= 12 Then
                '    bc_am_load_objects.obc_current_document.extension = ".doc"
                'ElseIf bc_cs_central_settings.office_version < 12 Then
                '    bc_am_load_objects.obc_current_document.extension = ".doc"
                'ElseIf bc_cs_central_settings.office_version >= 12 Then
                '    bc_am_load_objects.obc_current_document.extension = ".docx"
                'End If


                REM open up instance of template as document in word
                'odoc = New bc_ao_word(otemplate, oproperties, False, False)

                REM FIL 5.3
                Dim vbehalf As New bc_dx_am_behalf_author
                Dim obehalf As New Cbc_am_behalf_author(bc_am_load_objects.obc_current_document.filename, bc_am_load_objects.obc_current_document.pub_type_id, bc_am_load_objects.obc_current_document.entity_id, vbehalf)
                If obehalf.check_behalf_selection_required = True Then
                    Dim oapi As New API
                    API.SetWindowPos(vbehalf.Handle.ToInt32, API.HWND_TOPMOST, 10, 10, 1, 1, 1)
                    vbehalf.ShowDialog()
                    If obehalf.cancel = True Then
                        bc_cs_central_settings.progress_bar.unload()
                        cancel = True
                        Exit Sub
                    End If
                End If
                REM open up instance of template as document in word
                odoc = New bc_ao_word(otemplate, oproperties, False, False)
                REM set behalf of user id
                If obehalf.originator <> 0 Then
                    obehalf.save_originator(bc_am_load_objects.obc_current_document.filename)
                    If obehalf.originator <> bc_cs_central_settings.logged_on_user_id Then
                        bc_am_load_objects.obc_current_document.originating_author = obehalf.originator
                        bc_am_load_objects.obc_current_document.behalf_of_author_id = bc_cs_central_settings.logged_on_user_id
                        Dim oa As New bc_om_user
                        oa.id = obehalf.originator
                        bc_am_load_objects.obc_current_document.authors.Add(oa)
                    End If
                End If


                REM build the document
                If odoc.bsuccess = True Then
                    bc_cs_central_settings.progress_bar.increment("Building Document...")


                    odoc.build(otemplate.id)
                    Dim na As String = odoc.get_name
                    REM SW cope with office versions
                    If Left(Right(na, 4), 1) = "." Then
                        na = Left(na, Len(na) - 4)
                        bc_am_load_objects.obc_current_document.extension = ".doc"
                    End If
                    If Left(Right(na, 5), 1) = "." Then
                        na = Left(na, Len(na) - 5)
                        bc_am_load_objects.obc_current_document.extension = ".docx"
                    End If

                    odoc.set_parameters(params)
                    bc_cs_central_settings.progress_bar.increment("Initializing Document...")

                    odoc.initialize()
                End If
            ElseIf strmimetype = POWERPOINT_EXT Or strmimetype = POWERPOINT2007_EXT Then

                'bc_am_load_objects.obc_current_document.extension = ".ppt"
                REM SW cope with office versions
                If bc_cs_central_settings.userOfficeStatus = 2 Then
                    bc_am_load_objects.obc_current_document.extension = ".pptx"
                Else
                    bc_am_load_objects.obc_current_document.extension = ".ppt"
                End If
                REM open up instance of template as document in word
                odoc = New bc_ao_powerpoint(otemplate, oproperties, True, True)
                REM build the document
                If odoc.bsuccess = True Then
                    bc_cs_central_settings.progress_bar.increment("Building Document...")
                    odoc.build(otemplate.id)
                    REM doc_id
                    Dim na As String = odoc.get_name

                    REM SW cope with office versions
                    If Left(Right(na, 4), 1) = "." Then
                        na = Left(na, Len(na) - 4)
                        bc_am_load_objects.obc_current_document.extension = ".ppt"
                    End If
                    If Left(Right(na, 5), 1) = "." Then
                        na = Left(na, Len(na) - 5)
                        bc_am_load_objects.obc_current_document.extension = ".pptx"
                    End If
                    'If InStr(na, ".", CompareMethod.Text) > 1 Then
                    '    na = Left(na, Len(na) - 4)
                    'End If

                    'odoc.set_doc_id(na)
                    odoc.set_parameters(params)

                    bc_cs_central_settings.progress_bar.increment("Initializing Document...")
                    odoc.initialize()
                End If
            Else
                REM raise error file type not supported
                Err.Raise(bc_cs_error_codes.RETURN_ERROR, , "File Type: " + strmimetype + " not supported.")
            End If
        Catch ex As Exception
            Dim err As New bc_cs_error_log("bc_am_at_build_document", "build_from_template", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            If cancel = False Then
                If Not odoc Is Nothing Then
                    odoc.show()
                    odoc.activate()
                End If

                REM if user has access locked then lock document
                If bc_am_load_objects.obc_current_document.locked = True Then
                    odoc.lock_document()
                Else
                    odoc.unlock_document()
                End If
            End If
            otrace = New bc_cs_activity_log("bc_am_at_build_document", "build_from_template", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub


    Public Function launch_product(ByVal pub_type_id As Long, ByVal entity_id As Long, ByVal params As bc_om_parameters, ByVal sub_entity_id As Long) As Boolean

        REM Allow new documents to be created by word.
        REM Steve Wooderson 26/06/2013

        Dim oproductselection As New bc_am_product_selection
        bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve create", "Initializing Build...", 20, True, True)
        If oproductselection.launch_product(pub_type_id, entity_id, params, sub_entity_id) = bc_cs_error_codes.RETURN_ERROR Then
            launch_product = False
        End If

        launch_product = True
        bc_cs_central_settings.progress_bar.unload()

    End Function

    Public Sub invoke_composite(ByVal entity_id As Long, ByVal pub_type As Object, Optional ByVal customParam As String = "")

        REM Invoke to allow new documents to be created by word.
        REM Steve Wooderson 26/06/2013
        bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve create", "Retrieving Composite Document List...", 1, False, True)
        bc_cs_central_settings.progress_bar.show()
        bc_cs_central_settings.progress_bar.refresh()
        bc_am_load_objects.obc_om_composite_pub_type = pub_type

        Dim bc_am_composite_doc = New bc_am_composite_docs(entity_id, customParam)
        bc_cs_central_settings.progress_bar.unload()
        Dim bcs As New bc_cs_central_settings(True)
        If bc_am_composite_doc.get_component_documents = True Then
            If bc_cs_central_settings.use_advanced_composite_build = True Then
                Dim oadv As New bc_am_adv_composite_build
                oadv.ShowDialog()
                If oadv.proceed = True Then
                    bc_am_composite_doc.merge()
                End If
            Else
                bc_am_composite_doc.merge()
            End If
        End If
    End Sub

    'Public Sub invoke_clone(ByVal pt_id As Long, ByVal entity_id As Long, ByVal sub_entity_id As Long)
    '    Dim otrace As New bc_cs_activity_log("bc_am_at_wizard_main", "invoke_clone", bc_cs_activity_codes.TRACE_ENTRY, "")
    '    Dim bcs As New bc_cs_progress("Blue Curve", "Retrieving Document List", 0, False, True)

    '    Try

    '        Dim oclone As New bc_om_clone_document_list(pt_id, entity_id, sub_entity_id)
    '        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
    '            oclone.db_read()
    '        Else
    '            oclone.tmode = bc_cs_soap_base_class.tREAD
    '            If oclone.transmit_to_server_and_receive(oclone, True) = False Then
    '                Exit Sub
    '            End If
    '        End If
    '        bcs.unload()

    '        If oclone.documents.document.Count = 0 Then
    '            Dim omsg As New bc_cs_message("Blue Curve", "There are no previous documents for this combination. Please create a new document.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
    '            Exit Sub
    '        End If

    '        Dim fclone As New bc_am_clone_document
    '        Dim pt_name As String = ""
    '        Dim ent_name As String = ""
    '        Dim sub_ent_name As String = ""
    '        For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
    '            If bc_am_load_objects.obc_pub_types.pubtype(i).id = pt_id Then
    '                pt_name = bc_am_load_objects.obc_pub_types.pubtype(i).name
    '                Exit For
    '            End If
    '        Next
    '        If entity_id <> 0 Then
    '            For i = 0 To bc_am_load_objects.obc_entities.entity.Count - 1
    '                If bc_am_load_objects.obc_entities.entity(i).id = entity_id Then
    '                    ent_name = bc_am_load_objects.obc_entities.entity(i).name
    '                    If sub_entity_id = 0 Then
    '                        Exit For
    '                    End If
    '                End If
    '                If sub_entity_id <> 0 Then
    '                    If bc_am_load_objects.obc_entities.entity(i).id = sub_entity_id Then
    '                        sub_ent_name = bc_am_load_objects.obc_entities.entity(i).name
    '                    End If
    '                End If
    '            Next
    '        End If
    '        If entity_id = 0 Then
    '            fclone.ltitle.Text = "Please select a report of type: " + pt_name + " to clone from"
    '        ElseIf sub_entity_id = 0 Then
    '            fclone.ltitle.Text = "Please select a report of type: " + pt_name + " for: " + ent_name + " to clone from"
    '        Else
    '            fclone.ltitle.Text = "Please select a report of type: " + pt_name + " for: " + ent_name + " and " + sub_ent_name + " to clone from"
    '        End If
    '        fclone.load_list(oclone.documents.document)

    '        Dim oapi As New API
    '        API.SetWindowPos(fclone.Handle.ToInt32, API.HWND_TOPMOST, 10, 10, 1, 1, 1)
    '        fclone.ShowDialog()
    '        If fclone.proceed = True Then

    '            Dim ocopydoc As New bc_am_copy_document(fclone.mdoc_id, fclone.sdoc_id)
    '            If ocopydoc.copy_doc = False Then
    '                Exit Sub
    '            End If
    '        End If

    '    Catch ex As Exception
    '        Dim db_err As New bc_cs_error_log("bc_am_at_build_document", "invoke_clone", bc_cs_error_codes.USER_DEFINED, ex.Message)
    '    Finally
    '        bcs.unload()
    '        otrace = New bc_cs_activity_log("bc_am_at_build_document", "invoke_clone", bc_cs_activity_codes.TRACE_EXIT, "")
    '    End Try

    'End Sub



End Class

REM ==========================================
REM Blue Curve Limited 2005
REM Module:       Product Selection
REM Type:         Application Module
REM Description:  Evaluates Which Product to Build
REM Version:      1
REM Change history
REM ==========================================
Public Class bc_am_product_selection

    REM attempts to launch a product
    Public Function launch_product(ByVal pub_type_id As Long, ByVal entity_id As Long, ByVal params As bc_om_parameters, ByVal sub_entity_id As Long) As Integer
        Dim otrace As New bc_cs_activity_log("bc_am_product_selection", "launch_product", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim ocommentary As bc_cs_activity_log

        Dim k As Integer

        Try

            Dim i As Integer
            Dim product_id, template_id As Long
            Dim templateFound As Boolean = False

            product_id = choose_product(pub_type_id, entity_id)

            If product_id = 0 Then
                Dim omessage As New bc_cs_message("Blue Curve create", "No Product/Template Defined for this selection!", bc_cs_message.MESSAGE)
                ocommentary = New bc_cs_activity_log("bc_am_product_selection", "launch_product", bc_cs_activity_codes.COMMENTARY, "No Product/template Found for Pub Type Id:" + CStr(pub_type_id))
                launch_product = bc_cs_error_codes.RETURN_ERROR
            Else
                REM attempt build document from template for this product
                REM get template id for product
                For k = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                    If bc_am_load_objects.obc_pub_types.pubtype(k).id = pub_type_id Then
                        For i = 0 To bc_am_load_objects.obc_pub_types.pubtype(k).products.product.count - 1
                            If bc_am_load_objects.obc_pub_types.pubtype(k).products.product(i).id = product_id Then
                                REM call AM build document
                                template_id = bc_am_load_objects.obc_pub_types.pubtype(k).products.product(i).template_id
                                ocommentary = New bc_cs_activity_log("bc_am_product_selection", "launch_product", bc_cs_activity_codes.COMMENTARY, "Template Selected:" + CStr(template_id))
                                Exit For
                            End If
                        Next
                        REM get template name
                        'bc_cs_central_settings.progress_bar = New bc_cs_progress
                        bc_cs_central_settings.progress_bar.increment("Evaluating template...")
                        For i = 0 To bc_am_load_objects.obc_templates.template.Count - 1
                            If bc_am_load_objects.obc_templates.template(i).id = template_id Then
                                REM call AM build document

                                If Len(bc_am_load_objects.obc_templates.template(i).filename) > 4 AndAlso UCase(Right(bc_am_load_objects.obc_templates.template(i).filename, 4)) = "HTML" Then
                                    Dim hfn As String

                                    Dim oguid As New bc_om_get_guid_for_html_product
                                    oguid.entity_id = entity_id
                                    oguid.pub_type_id = pub_type_id
                                    oguid.doc_id = 0
                                    oguid.user_id = bc_cs_central_settings.logged_on_user_id
                                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                                        oguid.db_read()
                                    Else
                                        oguid.tmode = bc_cs_soap_base_class.tREAD
                                        If oguid.transmit_to_server_and_receive(oguid, True) = False Then
                                            Exit Function
                                        End If
                                    End If

                                    hfn = bc_cs_central_settings.web_process_url + bc_am_load_objects.obc_templates.template(i).filename + "?guid=" + oguid.sguid
                                    System.Diagnostics.Process.Start(hfn)
                                    bc_cs_central_settings.progress_bar.unload()
                                Else
                                    Dim obuild_doc As New bc_am_at_build_document(bc_am_load_objects.obc_templates.template(i).name, pub_type_id, entity_id, params, sub_entity_id)
                                End If
                                templateFound = True
                                Exit For
                            End If
                        Next
                    End If
                Next
                If Not templateFound Then
                    Dim omessage As New bc_cs_message("Blue Curve create", "No Product/Template Defined for this selection!", bc_cs_message.MESSAGE)
                    ocommentary = New bc_cs_activity_log("bc_am_product_selection", "launch_product", bc_cs_activity_codes.COMMENTARY, "No Product/template Found for Pub Type Id:" + CStr(pub_type_id))
                    launch_product = bc_cs_error_codes.RETURN_ERROR
                End If
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_product_selection", "launch_product", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_am_product_selection", "launch_product", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
    REM chooses which product to select based upon pub type and entity
    Private Function choose_product(ByVal pt As String, ByVal entity_id As Long) As Integer
        Dim otrace As New bc_cs_activity_log("bc_am_product_selection", "choose_product", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim matching_entity_id As Long
            Dim matching_entity_id2 As Long

            Dim i, j As Integer

            choose_product = 0
            For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                REM get pub type of interest
                If bc_am_load_objects.obc_pub_types.pubtype(i).id = pt Then
                    With bc_am_load_objects.obc_pub_types.pubtype(i)
                        REM check super value to see if a parent category exists
                        If IsNumeric(.parent_category) And IsNumeric(.parent_category2) Then
                            If CInt(.parent_category) > 0 And CInt(.parent_category2) = 0 Then
                                Dim ocomm As New bc_cs_activity_log("bc_am_product_selection", "choose_product", bc_cs_activity_codes.TRACE_ENTRY, "Super value only  set")
                                REM if child category is same as parent category
                                REM find product that matches entity
                                If .child_category = .parent_category Then
                                    matching_entity_id = entity_id
                                Else

                                    matching_entity_id = bc_am_load_objects.obc_entity_links.get_entity_id_for_class_hierarchy(entity_id, .parent_category, bc_am_load_objects.obc_entities, 0, 1)
                                End If
                                ocomm = New bc_cs_activity_log("bc_am_product_selection", "choose_product", bc_cs_activity_codes.TRACE_ENTRY, "Super value matched on entity_id: " + CStr(matching_entity_id))
                                If matching_entity_id > 0 Then
                                    For j = 0 To .products.product.count - 1
                                        If CInt(.products.product(j).super_value) = matching_entity_id Then
                                            choose_product = .products.product(j).id
                                            ocomm = New bc_cs_activity_log("bc_am_product_selection", "choose_product", bc_cs_activity_codes.TRACE_ENTRY, "Product selected: " + CStr(choose_product))
                                        End If
                                    Next
                                End If
                            ElseIf CInt(.parent_category2) > 0 Then
                                Dim ocomm As New bc_cs_activity_log("bc_am_product_selection", "choose_product", bc_cs_activity_codes.TRACE_ENTRY, "Super value and Super Value 2 set")
                                REM primary category
                                If .child_category = .parent_category Then
                                    matching_entity_id = entity_id
                                Else
                                    matching_entity_id = bc_am_load_objects.obc_entity_links.get_entity_id_for_class_hierarchy(entity_id, .parent_category, bc_am_load_objects.obc_entities)
                                End If
                                ocomm = New bc_cs_activity_log("bc_am_product_selection", "choose_product", bc_cs_activity_codes.TRACE_ENTRY, "Super value matched on entity_id: " + CStr(matching_entity_id))
                                REM secondary category
                                If .child_category = .parent_category2 Then
                                    matching_entity_id2 = entity_id
                                Else
                                    matching_entity_id2 = bc_am_load_objects.obc_entity_links.get_entity_id_for_class_hierarchy(entity_id, .parent_category2, bc_am_load_objects.obc_entities)
                                End If
                                ocomm = New bc_cs_activity_log("bc_am_product_selection", "choose_product", bc_cs_activity_codes.TRACE_ENTRY, "Super value2 matched on entity_id: " + CStr(matching_entity_id2))
                                If matching_entity_id > 0 And matching_entity_id2 > 0 Then
                                    For j = 0 To .products.product.count - 1
                                        If CInt(.products.product(j).super_value) = matching_entity_id And CInt(.products.product(j).super_value2) = matching_entity_id2 Then
                                            choose_product = .products.product(j).id
                                            ocomm = New bc_cs_activity_log("bc_am_product_selection", "choose_product", bc_cs_activity_codes.TRACE_ENTRY, "Product selected: " + CStr(choose_product))
                                            Exit For
                                        End If
                                    Next
                                End If

                            Else
                                For j = 0 To .products.product.count - 1
                                    choose_product = .products.product(j).id
                                Next
                            End If
                        Else
                            Dim ocomm As New bc_cs_activity_log("bc_am_product_selection", "choose_product", bc_cs_activity_codes.TRACE_ENTRY, "No super values")
                            REM return product id for pub type
                            For j = 0 To .products.product.count - 1
                                choose_product = .product.product(j).id
                                ocomm = New bc_cs_activity_log("bc_am_product_selection", "choose_product", bc_cs_activity_codes.TRACE_ENTRY, "Product selected: " + CStr(choose_product))
                            Next
                        End If
                    End With
                End If
            Next
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_product_selection", "choose_product", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_am_product_selection", "choose_product", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function
End Class

Public Class bc_am_composite_docs
    Private entity_id As Long
    Public Sub New(ByVal entity_id As Long, ByVal customParam As String)
        Me.entity_id = entity_id
        bc_am_load_objects.obc_om_composite_pub_type.composite_composition.documents.document.Clear()
        bc_am_load_objects.obc_om_composite_pub_type.composite_composition.customParam = customParam

    End Sub
    Public Function get_component_documents() As Boolean
        Dim otrace As New bc_cs_activity_log("bc_am_composite_docs", "get_component_documents", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            bc_cs_central_settings.progress_bar.unload()
            bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve", "Retrieving document list...", 0, False, True)


            bc_cs_central_settings.progress_bar.increment("Retrieving document list...")
            bc_cs_central_settings.progress_bar.refresh()
            get_component_documents = True
            REM get document list and place in box
            bc_am_load_objects.obc_om_composite_pub_type.composite_composition.pub_type_id = bc_am_load_objects.obc_om_composite_pub_type.id
            REM if composite is no merge flag this so only front page is returned
            If bc_cs_central_settings.custom_library = "" Or bc_cs_central_settings.custom_library = "Core" Then
                REM see if pub type id has specified core merge
                For i = 0 To bc_cs_central_settings.merges.Count - 1
                    If IsNumeric(bc_cs_central_settings.merges(i).pub_type_id) Then
                        If bc_cs_central_settings.merges(i).pub_type_id = bc_am_load_objects.obc_om_composite_pub_type.composite_composition.pub_type_id Then
                            If bc_cs_central_settings.merges(i).merge_name = "NoMerge" Then
                                bc_am_load_objects.obc_om_composite_pub_type.composite_composition.no_merge = True
                                Dim ocommentary As New bc_cs_activity_log("bc_am_composite_docs", "get_component_documents", bc_cs_activity_codes.COMMENTARY, "No Mereg so only front page retrived from server")
                                Exit For
                            End If
                        End If
                    End If
                Next
            End If

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                bc_am_load_objects.obc_om_composite_pub_type.composite_composition.db_read()
            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                bc_am_load_objects.obc_om_composite_pub_type.composite_composition.tmode = bc_cs_soap_base_class.tREAD
                bc_am_load_objects.obc_om_composite_pub_type.composite_composition.transmit_to_server_and_receive(bc_am_load_objects.obc_om_composite_pub_type.composite_composition, True)
            Else
                get_component_documents = False
                Dim omessage As New bc_cs_message("Blue Curve - create", "Cannot create composite as offline", bc_cs_message.MESSAGE)
                Exit Function
            End If
            bc_cs_central_settings.progress_bar.increment("Retrieving document list...")

            REM now filte list if entity_id is not 0
            If Me.entity_id > 0 Then
                filter_composite_list()
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_composite_docs", "get_component_documents", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            bc_cs_central_settings.progress_bar.unload()

            otrace = New bc_cs_activity_log("bc_am_composite_docs", "get_component_documents", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function
    Private Sub filter_composite_list()
        Dim otrace As New bc_cs_activity_log("bc_am_composite_docs", "filter_composite_list", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim i, j As Integer

            Dim include As Boolean = False
            Dim final_documents As New bc_om_documents

            REM for each document in compoistion need to check if it is 
            REM either classifed against entity_id or propogation of entity_id
            For i = 0 To bc_am_load_objects.obc_om_composite_pub_type.composite_composition.documents.document.Count - 1
                include = False
                With bc_am_load_objects.obc_om_composite_pub_type.composite_composition.documents.document(i)
                    For j = 0 To .taxonomy.count - 1
                        REM include if directly classfied against entity
                        If .taxonomy(j).entity_id = Me.entity_id Then
                            include = True
                            Exit For
                            REM include if a child of parent propogarion
                        Else
                            include = propogate_links(.taxonomy(j).entity_id, Me.entity_id)
                            If include = True Then
                                Exit For
                            End If
                        End If
                    Next
                    REM include anthging with no classifcation as well
                    If .taxonomy.count = 0 Then
                        include = True
                    End If
                End With
                If include = True Then
                    final_documents.document.Add(bc_am_load_objects.obc_om_composite_pub_type.composite_composition.documents.document(i))
                End If
            Next
            bc_am_load_objects.obc_om_composite_pub_type.composite_composition.documents.document.Clear()
            bc_am_load_objects.obc_om_composite_pub_type.composite_composition.documents.document = final_documents.document
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_composite_docs", "filter_composite_list", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_am_composite_docs", "filter_composite_list", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Private Function propogate_links(ByVal child_entity_id As Long, ByVal parent_entity_id As Long) As Boolean
        Dim otrace As New bc_cs_activity_log("bc_am_composite_docs", "propogate_links", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim i As Integer
            Dim ocommentray As New bc_cs_activity_log("bc_am_composite_docs", "propogate_links", bc_cs_activity_codes.COMMENTARY, "Propogating: " + CStr(child_entity_id) + " to: " + CStr(parent_entity_id))
            propogate_links = False
            For i = 0 To bc_am_load_objects.obc_entity_links.entity_link.Count - 1
                With bc_am_load_objects.obc_entity_links.entity_link(i)
                    If .child_entity_id = child_entity_id Then
                        If .parent_entity_id = parent_entity_id Then
                            propogate_links = True
                            Exit Function
                        Else
                            propogate_links = propogate_links(.parent_entity_id, parent_entity_id)
                            If propogate_links = True Then
                                Exit Function
                            End If
                        End If
                    End If
                End With

            Next
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_composite_docs", "propogate_links", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_am_composite_docs", "propogate_links", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
    Public Sub merge()
        Dim otrace As New bc_cs_activity_log("bc_am_composite_docs", "merge", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim merge_name As String
            Dim ocommentary As bc_cs_activity_log
            Dim bookmark_name As String

            If bc_am_load_objects.obc_om_composite_pub_type.composite_composition.documents.document.Count = 0 Then
                Dim omessage As New bc_cs_message("Blue Curve create", "Nothing to merge!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Sub
            End If

            bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve create", "Initializing Composite Build", 1, False, True)
            bc_cs_central_settings.progress_bar.show()

            Dim bc_ao_word As New bc_ao_word

            Dim i, j, k As Integer
            Dim name As String
            Dim found As Boolean
            Dim fs As New bc_cs_file_transfer_services

            REM create new document object
            Dim bc_am_build_document = New bc_am_at_build_document("", bc_am_load_objects.obc_om_composite_pub_type.id, 0, Nothing, 0)
            bc_am_build_document.set_doc_metadata()
            name = bc_ao_word.generate_local_filename_prefix
            If bc_cs_central_settings.userOfficeStatus = 2 Then
                bc_am_load_objects.obc_current_document.filename = name + ".docx"
                bc_am_load_objects.obc_current_document.extension = ".docx"
            Else
                bc_am_load_objects.obc_current_document.filename = name + ".doc"
                bc_am_load_objects.obc_current_document.extension = ".doc"
            End If
            bc_am_load_objects.obc_current_document.entity_id = Me.entity_id
            bc_cs_central_settings.progress_bar.increment("Assigning metadata...")

            REM delete previous composite pieces
            If bc_cs_central_settings.userOfficeStatus = 2 Then
                For i = 0 To 900
                    If fs.check_document_exists(bc_cs_central_settings.local_repos_path + CStr(i + 2) + ".docx") Then
                        fs.delete_file(bc_cs_central_settings.local_repos_path + CStr(i + 2) + ".docx")
                    Else
                        ocommentary = New bc_cs_activity_log("bc_am_composite_docs", "merge", bc_cs_activity_codes.COMMENTARY, "Deleting " + CStr(i) + " Previous Composite Units")
                        Exit For
                    End If

                Next
            Else
                For i = 0 To 900
                    If fs.check_document_exists(bc_cs_central_settings.local_repos_path + CStr(i + 2) + ".doc") Then
                        fs.delete_file(bc_cs_central_settings.local_repos_path + CStr(i + 2) + ".doc")
                    Else
                        ocommentary = New bc_cs_activity_log("bc_am_composite_docs", "merge", bc_cs_activity_codes.COMMENTARY, "Deleting " + CStr(i) + " Previous Composite Units")
                        Exit For
                    End If

                Next
            End If

            REM copy pieces down to local area
            Dim constituent_docs As New ArrayList

            Dim no_merge As Boolean
            no_merge = False
            If bc_cs_central_settings.custom_library = "" Or bc_cs_central_settings.custom_library = "Core" Then
                REM see if pub type id has specified core merge
                For i = 0 To bc_cs_central_settings.merges.Count - 1
                    If bc_cs_central_settings.merges(i).pub_type_id = bc_am_load_objects.obc_om_composite_pub_type.composite_composition.pub_type_id Then
                        If bc_cs_central_settings.merges(i).merge_name = "NoMerge" Then
                            no_merge = True
                            Exit For
                        End If
                    End If
                Next
            End If

            With bc_am_load_objects.obc_om_composite_pub_type.composite_composition.documents
                REM write documents to local repos
                For i = 0 To .document.Count - 1
                    constituent_docs.Add(.document(i).id)
                    If i = 0 Then
                        fs.write_bytestream_to_document(bc_cs_central_settings.local_repos_path + bc_am_load_objects.obc_current_document.filename, .document(i).bytedoc, Nothing)
                    Else
                        If no_merge = False Then
                            If bc_cs_central_settings.userOfficeStatus = 2 Then
                                fs.write_bytestream_to_document(bc_cs_central_settings.local_repos_path + CStr(i + 1) + ".docx", .document(i).bytedoc, Nothing)
                            Else
                                fs.write_bytestream_to_document(bc_cs_central_settings.local_repos_path + CStr(i + 1) + ".doc", .document(i).bytedoc, Nothing)
                            End If
                        End If
                    End If
                    REM copy over taxonomy
                    For j = 0 To .document(i).taxonomy.count - 1
                        REM only add if doesnt exist
                        found = False
                        For k = 0 To bc_am_load_objects.obc_current_document.taxonomy.Count - 1
                            If bc_am_load_objects.obc_current_document.taxonomy(k).entity_id = .document(i).taxonomy(j).entity_id Then
                                found = True
                                Exit For
                            End If
                        Next
                        If found = False Then
                            bc_am_load_objects.obc_current_document.taxonomy.Add(.document(i).taxonomy(j))
                        End If
                    Next
                    bc_am_load_objects.obc_current_document.composite_constiuents = constituent_docs
                    REM copy over disclosures
                    For j = 0 To .document(i).disclosures.count - 1
                        REM only add if doesnt exist
                        found = False
                        For k = 0 To bc_am_load_objects.obc_current_document.disclosures.Count - 1
                            If bc_am_load_objects.obc_current_document.disclosures(k).entity_id = .document(i).disclosures(j).entity_id Then
                                found = True
                                Exit For
                            End If
                        Next
                        If found = False Then
                            bc_am_load_objects.obc_current_document.disclosures.Add(.document(i).disclosures(j))
                        End If
                    Next
                    REM copy over author
                    For j = 0 To .document(i).authors.count - 1
                        For k = 0 To bc_am_load_objects.obc_current_document.authors.Count - 1
                            If bc_am_load_objects.obc_current_document.authors(k).id = .document(i).authors(j).id Then
                                REM only add if doesnt exists
                                found = True
                                Exit For
                            End If
                        Next
                        If found = False Then
                            bc_am_load_objects.obc_current_document.authors.Add(.document(i).authors(j))
                        End If
                    Next
                    REM copy over display parameters
                    For j = 0 To .document(i).refresh_components.refresh_components.count - 1
                        REM get union classification
                        bc_am_load_objects.obc_current_document.refresh_components.refresh_components.Add(.document(i).refresh_components.refresh_components(j))
                        REM get display parameters
                    Next
                    REM conduct physical merge and leave open
                Next
                bc_cs_central_settings.progress_bar.increment("Opening up front page...")
                REM open up front page

                bc_ao_word.open(bc_cs_central_settings.local_repos_path + bc_am_load_objects.obc_current_document.filename, False, False)
                bc_ao_word.set_parameter("rnet_pub_type_id", bc_am_load_objects.obc_current_document.pub_type_id)
                bc_cs_central_settings.progress_bar.increment("Setting Properties...")
                bc_ao_word.set_doc_id(name)
                bc_cs_central_settings.progress_bar.increment("Conducting Merge...")
                REM if custom merge hook in otherwise use default
                REM custom merge TBD
                bc_ao_word.visible(False)
                If bc_cs_central_settings.custom_library = "" Or bc_cs_central_settings.custom_library = "Core" Then
                    merge_name = ""
                    bookmark_name = ""
                    REM see if pub type id has specified core merge
                    For i = 0 To bc_cs_central_settings.merges.Count - 1
                        If bc_cs_central_settings.merges(i).pub_type_id = bc_am_load_objects.obc_current_document.pub_type_id Then
                            merge_name = bc_cs_central_settings.merges(i).merge_name
                            bookmark_name = bc_cs_central_settings.merges(i).bookmark_name
                            ocommentary = New bc_cs_activity_log("bc_am_composite_docs", "merge", bc_cs_activity_codes.COMMENTARY, "Core merge: " + merge_name + " selected for pub type id:" + CStr(bc_am_load_objects.obc_current_document.pub_type_id))
                            Exit For
                        End If
                    Next
                    ocommentary = New bc_cs_activity_log("bc_am_composite_docs", "merge", bc_cs_activity_codes.COMMENTARY, "Core merge: " + merge_name + " selected for pub type id:" + CStr(bc_am_load_objects.obc_current_document.pub_type_id) + " insert bookmark: " + bookmark_name)
                    If merge_name = "Continuous" Then
                        ocommentary = New bc_cs_activity_log("bc_am_composite_docs", "merge", bc_cs_activity_codes.COMMENTARY, "Continuous Chosen")
                        For i = 1 To .document.Count - 1
                            If bc_cs_central_settings.userOfficeStatus = 2 Then
                                bc_ao_word.insert_file(bc_cs_central_settings.local_repos_path + CStr(i + 1) + ".docx", False, True, bookmark_name)
                            Else
                                bc_ao_word.insert_file(bc_cs_central_settings.local_repos_path + CStr(i + 1) + ".doc", False, True, bookmark_name)
                            End If
                        Next

                        REM default is insert file new page 
                    ElseIf merge_name = "NoLink" Then
                        ocommentary = New bc_cs_activity_log("bc_am_composite_docs", "merge", bc_cs_activity_codes.COMMENTARY, "NoLink Chosen")
                        For i = 1 To .document.Count - 1
                            If bc_cs_central_settings.userOfficeStatus = 2 Then
                                bc_ao_word.insert_file(bc_cs_central_settings.local_repos_path + CStr(i + 1) + ".docx", True, False, bookmark_name)
                            Else
                                bc_ao_word.insert_file(bc_cs_central_settings.local_repos_path + CStr(i + 1) + ".doc", True, False, bookmark_name)
                            End If
                        Next
                    ElseIf merge_name = "NoMerge" Or merge_name = "NoMergeWithDocs" Then
                        ocommentary = New bc_cs_activity_log("bc_am_composite_docs", "merge", bc_cs_activity_codes.COMMENTARY, "NoMerge Chosen")
                    Else
                        ocommentary = New bc_cs_activity_log("bc_am_composite_docs", "merge", bc_cs_activity_codes.COMMENTARY, "Default Merge Chosen")
                        For i = 1 To .document.Count - 1
                            If bc_cs_central_settings.userOfficeStatus = 2 Then
                                bc_ao_word.insert_file(bc_cs_central_settings.local_repos_path + CStr(i + 1) + ".docx", True, True, bookmark_name)
                            Else
                                bc_ao_word.insert_file(bc_cs_central_settings.local_repos_path + CStr(i + 1) + ".doc", True, True, bookmark_name)
                            End If
                        Next
                    End If
                Else
                    ocommentary = New bc_cs_activity_log("bc_am_composite_docs", "merge", bc_cs_activity_codes.COMMENTARY, "Custom Merge Selected:" + bc_cs_central_settings.custom_library)
                    Dim custommerge As New bc_am_custom_merge(bc_am_load_objects.obc_current_document.pub_type_id, .document.Count - 1, bc_ao_word.worddocument, bc_cs_central_settings.local_repos_path)
                    If custommerge.merge = False Then
                        Dim omessage = New bc_cs_message("Blue Curve - create", "Custom Merge Failed", bc_cs_message.MESSAGE)
                    End If
                End If
                bc_am_load_objects.obc_current_document.write_data_to_file(bc_cs_central_settings.local_repos_path + name + ".dat")
            End With
            REM attempt to call DOCINIT
            bc_ao_word.initialize()
            bc_ao_word.show()
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_composite_docs", "merge", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Try
                bc_cs_central_settings.progress_bar.hide()
            Catch
            End Try
            otrace = New bc_cs_activity_log("bc_am_composite_docs", "merge", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class

Public Class bc_am_custom_merge
    Private pub_type_id As Long
    Private activedocument As Object
    Private num_docs As Integer
    Private local_repos As String
    Private corpoClientList As String

    REM custom DLL later on
    Public Sub New(ByVal pub_type_id As Long, ByVal num_docs As Integer, ByVal activedocument As Object, ByVal local_repos As String)
        Me.pub_type_id = pub_type_id
        Me.activedocument = activedocument
        Me.num_docs = num_docs
        Me.local_repos = local_repos
        Me.corpoClientList = ""
    End Sub
    Public Function merge() As Boolean
        Dim otrace As New bc_cs_activity_log("bc_am_composite_docs", "custom_merge", bc_cs_activity_codes.TRACE_ENTRY, "")
        merge = False
        Try

            Select Case Me.pub_type_id
                Case 12
                    'merge = Me.morning_meeting_merge()
                    merge = Me.MergeMorningCall()
                Case 4
                    'merge = Me.handbook_merge()
                    merge = Me.MergeHandbook()
            End Select

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_composite_docs", "custom_merge", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_am_composite_docs", "custom_merge", bc_cs_activity_codes.TRACE_EXIT, "")

        End Try
    End Function


    '   CS Merge V1 - Deprecated
    '    Private Function handbook_merge() As Boolean
    '        Dim ocommentary As bc_cs_activity_log
    '        Dim unitPath As String
    '        Dim docId As Integer
    '        handbook_merge = True

    '        'Debug: Me.activedocument.application.visible = True
    '        For docId = 2 To (num_docs + 1)
    '            unitPath = Me.local_repos + docId.ToString + ".doc"
    '            If docId > 2 Then
    '                If Not duplicateChartHolder(Me.activedocument, Me.pub_type_id) Then
    '                    ocommentary = New bc_cs_activity_log("bc_am_custom_merge", "handbook_merge", bc_cs_activity_codes.COMMENTARY, "Unable to duplicate chart holder")
    '                    handbook_merge = False
    '                End If

    '                If Not placeChartKey(Me.activedocument, Me.pub_type_id) Then
    '                    ocommentary = New bc_cs_activity_log("bc_am_custom_merge", "handbook_merge", bc_cs_activity_codes.COMMENTARY, "Unable to insert chart Key")
    '                    handbook_merge = False
    '                End If
    '            End If

    '            If Not insertNewDoc(Me.activedocument, unitPath, docId - 1, Me.pub_type_id) Then
    '                ocommentary = New bc_cs_activity_log("bc_am_custom_merge", "handbook_merge", bc_cs_activity_codes.COMMENTARY, "Unable to insert document [" + docId.ToString + "]")
    '                handbook_merge = False
    '            End If

    '            If Not renameChartKey(Me.activedocument, docId - 1, Me.pub_type_id) Then
    '                ocommentary = New bc_cs_activity_log("bc_am_custom_merge", "handbook_merge", bc_cs_activity_codes.COMMENTARY, "Unable to rename chart key [" + docId.ToString + "]")
    '                handbook_merge = False
    '            End If
    '        Next docId

    '    End Function

    '    Private Function morning_meeting_merge() As Boolean
    '        Dim ocommentary As bc_cs_activity_log
    '        Dim unitPath As String
    '        Dim docId As Integer
    '        morning_meeting_merge = True

    '        'Debug :Me.activedocument.application.visible = True
    '        For docId = 2 To (num_docs + 1)
    '            unitPath = Me.local_repos + docId.ToString + ".doc"
    '            If docId > 2 Then
    '                If Not duplicateChartHolder(Me.activedocument, Me.pub_type_id) Then
    '                    ocommentary = New bc_cs_activity_log("bc_am_custom_merge", "morning_meeting_merge", bc_cs_activity_codes.COMMENTARY, "Unable to duplicate chart holder")
    '                    morning_meeting_merge = False
    '                End If

    '                If Not placeChartKey(Me.activedocument, Me.pub_type_id) Then
    '                    ocommentary = New bc_cs_activity_log("bc_am_custom_merge", "morning_meeting_merge", bc_cs_activity_codes.COMMENTARY, "Unable to insert chart key")
    '                    morning_meeting_merge = False
    '                End If
    '            End If

    '            If Not insertNewDoc(Me.activedocument, unitPath, docId - 1, Me.pub_type_id) Then
    '                ocommentary = New bc_cs_activity_log("bc_am_custom_merge", "morning_meeting_merge", bc_cs_activity_codes.COMMENTARY, "Unable to insert document [" + docId.ToString + "]")
    '                morning_meeting_merge = False
    '            End If


    '            If Not ungroupAll(Me.activedocument, Me.pub_type_id) Then
    '                ocommentary = New bc_cs_activity_log("bc_am_custom_merge", "morning_meeting_merge", bc_cs_activity_codes.COMMENTARY, "Unable to ungroup boxes")
    '                morning_meeting_merge = False
    '            End If

    '            If Not renameChartKey(Me.activedocument, docId - 1, Me.pub_type_id) Then
    '                ocommentary = New bc_cs_activity_log("bc_am_custom_merge", "morning_meeting_merge", bc_cs_activity_codes.COMMENTARY, "Unable to rename chart key [" + docId.ToString + "]")
    '                morning_meeting_merge = False
    '            End If

    '            If Not createOverviewBookmarks(Me.activedocument, docId - 1) Then
    '                ocommentary = New bc_cs_activity_log("bc_am_custom_merge", "morning_meeting_merge", bc_cs_activity_codes.COMMENTARY, "Unable to create overview bookmarks [" + docId.ToString + "]")
    '                morning_meeting_merge = False
    '            End If

    '            If Not populateValuesToOverview(Me.activedocument, docId - 1, Me.pub_type_id) Then
    '                ocommentary = New bc_cs_activity_log("bc_am_custom_merge", "morning_meeting_merge", bc_cs_activity_codes.COMMENTARY, "Unable to populate overview [" + docId.ToString + "]")
    '                morning_meeting_merge = False
    '            End If


    '        Next docId

    '    End Function
    '    Private Function duplicateChartHolder(ByVal compositeDoc As Object, ByVal pub_type As Integer) As Boolean
    '        Dim T As Object     'word.table
    '        Dim S As Object     'word.shape
    '        Dim keyS As Object  'word.shape
    '        Dim D As Object     ' word.shape
    '        Dim nbShapes As Integer
    '        Dim pageHeight As Double
    '        Dim NewPos As Double
    '        Dim chartLocation As Integer
    '        Dim cNameBk As String

    '        Select Case pub_type
    '            Case 12
    '                chartLocation = 3
    '            Case 4
    '                chartLocation = 2
    '        End Select

    '        duplicateChartHolder = True
    '        On Error GoTo errorHandler

    '        '***************
    '        'Debug: comment on release
    '        'Set composite = New Word.Document
    '        'compositeDoc = Word.ActiveDocument

    '        '***************

    '        nbShapes = ((compositeDoc.Sections(chartLocation).Range.ShapeRange.Count - 1) / 2)

    '        compositeDoc.Sections(chartLocation).Range.Select()
    '        compositeDoc.Application.Selection.MoveLeft(unit:=1, Count:=1)
    '        S = compositeDoc.Sections(chartLocation).Range.ShapeRange("PriceChart Chart " & nbShapes)
    '        keyS = compositeDoc.Sections(chartLocation).Range.ShapeRange("PriceChart Header " & nbShapes)
    '        S.Select()
    '        S.RelativeHorizontalPosition = 1   'wdRelativeHorizontalPositionPage
    '        S.RelativeVerticalPosition = 1   'wdRelativeVerticalPositionPage
    '        S.WrapFormat.AllowOverlap = True

    '        keyS.RelativeHorizontalPosition = 1   'wdRelativeHorizontalPositionPage
    '        keyS.RelativeVerticalPosition = 1   'wdRelativeVerticalPositionPage
    '        keyS.WrapFormat.AllowOverlap = True


    '        pageHeight = compositeDoc.Sections(chartLocation).PageSetup.pageHeight

    '        NewPos = S.Top + S.Height + 38


    '        If ((NewPos + S.Height) > pageHeight) Then
    '            'insert new page

    '            compositeDoc.Sections(chartLocation).Range.Select()
    '            compositeDoc.Application.Selection.MoveRight(unit:=1, Count:=1)
    '            compositeDoc.Application.Selection.MoveLeft(unit:=1, Count:=1)
    '            compositeDoc.Application.Selection.MoveLeft(unit:=1, Count:=1)
    '            compositeDoc.Application.Selection.MoveLeft(unit:=1, Count:=1)

    '            NewPos = 122.9

    '        End If

    '        D = keyS.Duplicate

    '        D.Name = "PriceChart Header " & (nbShapes + 1)
    '        D.Left = S.Left
    '        D.RelativeHorizontalPosition = 1   'wdRelativeHorizontalPositionPage
    '        D.RelativeVerticalPosition = 1   'wdRelativeVerticalPositionPage
    '        D.Top = NewPos - 18
    '        D.Select()

    '        D = S.Duplicate

    '        D.Name = "PriceChart Chart " & (nbShapes + 1)
    '        D.Left = S.Left
    '        D.RelativeHorizontalPosition = 1   'wdRelativeHorizontalPositionPage
    '        D.RelativeVerticalPosition = 1   'wdRelativeVerticalPositionPage
    '        D.Top = NewPos
    '        D.Select()
    '        D.TextFrame.ContainingRange.Tables(1).Cell(1, 1).Range.Delete()
    '        While D.TextFrame.ContainingRange.Tables(1).Columns.Count <> 1
    '            D.TextFrame.ContainingRange.Tables(1).Columns.Last.Delete()
    '        End While
    '        While D.TextFrame.ContainingRange.Tables(1).Rows.Count <> 1
    '            D.TextFrame.ContainingRange.Tables(1).Rows.Last.Delete()
    '        End While



    '        GoTo EndOfSub
    'errorHandler:
    '        duplicateChartHolder = False
    'EndOfSub:
    '    End Function


    '    '*********************************************
    '    ' place the ChartKey below the last chart.
    '    '*********************************************

    '    Private Function placeChartKey(ByVal compositeDoc As Object, ByVal pub_type As Integer) As Boolean
    '        Dim Sh As Object        'word.shape
    '        Dim xPos As Double
    '        Dim pageHeight As Double
    '        Dim pKeySh As Object    'word.shape
    '        Dim nbShapes As Integer
    '        Dim chartLocation As Integer

    '        Select Case pub_type
    '            Case 12
    '                chartLocation = 3
    '            Case 4
    '                chartLocation = 2
    '        End Select


    '        placeChartKey = True

    '        nbShapes = ((compositeDoc.Sections(chartLocation).Range.ShapeRange.Count - 1) / 2)

    '        On Error GoTo errorHandler
    '        Sh = compositeDoc.Sections(chartLocation).Range.ShapeRange("PriceChart Chart " & nbShapes)
    '        pKeySh = compositeDoc.Sections(chartLocation).Range.ShapeRange("PriceChart Key")

    '        Sh.RelativeHorizontalPosition = 1   'wdRelativeHorizontalPositionPage
    '        Sh.RelativeVerticalPosition = 1   'wdRelativeVerticalPositionPage
    '        Sh.WrapFormat.AllowOverlap = True
    '        Sh.Select()
    '        pKeySh.RelativeHorizontalPosition = 1   'wdRelativeHorizontalPositionPage
    '        pKeySh.RelativeVerticalPosition = 1   'wdRelativeVerticalPositionPage
    '        pKeySh.WrapFormat.AllowOverlap = True

    '        xPos = Sh.Top + Sh.Height
    '        pageHeight = compositeDoc.Sections(3).PageSetup.pageHeight


    '        If ((xPos + pKeySh.Height) > pageHeight) Then
    '            compositeDoc.Sections(chartLocation).Range.Select()
    '            compositeDoc.Application.Selection.MoveRight(unit:=1, Count:=1)
    '            compositeDoc.Application.Selection.MoveLeft(unit:=1, Count:=1)
    '            compositeDoc.Application.Selection.MoveLeft(unit:=1, Count:=1)
    '            compositeDoc.Application.Selection.InsertBreak(Type:=7)  '7: wdpagebreak
    '            compositeDoc.Application.Selection.TypeParagraph()
    '            compositeDoc.Application.Selection.TypeParagraph()

    '            xPos = 104.9
    '        Else
    '            xPos = Sh.Top + Sh.Height
    '            'see if we can add a gap between the 2 box
    '            If ((xPos + pKeySh.Height + 20) < pageHeight) Then
    '                xPos = xPos + 20
    '            End If
    '        End If

    '        pKeySh.Left = Sh.Left
    '        pKeySh.Top = xPos


    '        GoTo EndOfSub
    'errorHandler:
    '        placeChartKey = False
    'EndOfSub:
    '    End Function


    '    '**********************************************
    '    'Insert the specified document in the section 2
    '    'of the current document
    '    '**********************************************

    '    Private Function insertNewDoc(ByVal compositeDoc As Object, ByVal unitDoc As String, ByVal id As Integer, ByVal pub_type As Integer) As Boolean
    '        Dim section As Object       'word.section
    '        Dim cGroup As Object        'word.shape
    '        Dim chartHolder As Object   'word.shape
    '        Dim pageHeight As Single
    '        Dim chartLocation As Integer
    '        Dim mnSpecific As Boolean

    '        Select Case pub_type
    '            Case 12
    '                chartLocation = 2
    '                mnSpecific = True
    '            Case 4
    '                chartLocation = 1
    '                mnSpecific = False
    '        End Select

    '        insertNewDoc = True

    '        On Error GoTo errorHandler
    '        section = compositeDoc.Sections(chartLocation)
    '        section.Range.Select()


    '        If id <> 1 Then


    '            compositeDoc.Application.Selection.MoveRight(unit:=1, Count:=1)
    '            compositeDoc.Application.Selection.MoveLeft(unit:=1, Count:=1)
    '            compositeDoc.Application.Selection.InsertBreak(Type:=7) '7: wdpagebreak
    '            compositeDoc.Application.Selection.TypeParagraph()
    '            compositeDoc.Application.Selection.TypeParagraph()
    '            compositeDoc.Application.Selection.TypeParagraph()
    '            compositeDoc.Application.Selection.MoveLeft(unit:=1, Count:=3)
    '        Else
    '            compositeDoc.Application.Selection.MoveLeft(unit:=1, Count:=1)
    '        End If


    '        compositeDoc.Application.Selection.InsertFile(unitDoc, Range:="unitRangeToInsert", _
    '                            ConfirmConversions:=False, Link:=False, Attachment:=False)


    '        If mnSpecific Then
    '            cGroup = compositeDoc.Sections(chartLocation).Range.ShapeRange("BC_Group_page1")
    '            cGroup.RelativeHorizontalPosition = 1   'wdRelativeHorizontalPositionPage
    '            cGroup.RelativeVerticalPosition = 1   'wdRelativeVerticalPositionPage
    '            cGroup.WrapFormat.AllowOverlap = True
    '            cGroup.Top = 104.9
    '            cGroup.Left = 42.55

    '            cGroup = compositeDoc.Sections(chartLocation).Range.ShapeRange("tStampBox")
    '            cGroup.Delete()
    '        End If


    '        cGroup = compositeDoc.Sections(chartLocation + 1).Range.ShapeRange("PriceChart")

    '        chartHolder = compositeDoc.Sections(chartLocation + 2).Range.ShapeRange("PriceChart Chart " & id)

    '        cGroup.TextFrame.ContainingRange.Tables(1).Cell(1, 1).Range.Copy()
    '        chartHolder.TextFrame.ContainingRange.Tables(1).Cell(1, 1).Range.Paste()
    '        While chartHolder.TextFrame.ContainingRange.Tables(1).Columns.Count <> 1
    '            chartHolder.TextFrame.ContainingRange.Tables(1).Columns.Last.Delete()
    '        End While

    '        compositeDoc.Sections(chartLocation + 1).Range.Select()
    '        compositeDoc.Sections(chartLocation + 1).Range.Delete()

    '        If (id <> 1) And (mnSpecific) Then
    '            If Not duplicateOverview(compositeDoc) Then
    '                Err.Raise(8, "duplicateOverview")
    '            End If
    '        End If




    '        GoTo EndOfSub
    'errorHandler:
    '        'error handler
    '        insertNewDoc = False
    'EndOfSub:
    '    End Function



    '    '**********************************************************
    '    'Duplicates an overview table.
    '    '1st table's name has to be "overview 1"
    '    'subsequent tables will be named "overview X"
    '    '   where X is the table occurence number
    '    '**********************************************************

    '    Private Function duplicateOverview(ByVal doc As Object) As Boolean

    '        Dim T As Object    'word.table
    '        Dim S As Object    'word.shape
    '        Dim D As Object    'word.shape
    '        Dim nbShapes As Integer
    '        Dim pageHeight As Double
    '        Dim NewPos As Double

    '        duplicateOverview = True
    '        On Error GoTo errorHandler


    '        nbShapes = doc.Sections(1).Range.ShapeRange.Count - 1
    '        doc.Sections(1).Range.Select()
    '        doc.Application.Selection.MoveLeft(unit:=1, Count:=1)
    '        S = doc.Sections(1).Range.ShapeRange("overview " & nbShapes)

    '        S.RelativeHorizontalPosition = 1   'wdRelativeHorizontalPositionPage
    '        S.RelativeVerticalPosition = 1   'wdRelativeVerticalPositionPage
    '        S.WrapFormat.AllowOverlap = True

    '        S.Select()

    '        pageHeight = doc.Sections(1).PageSetup.pageHeight
    '        NewPos = S.Top + S.Height + 20

    '        If ((NewPos + S.Height) > pageHeight) Then
    '            'insert new page
    '            doc.Sections(1).Range.Select()
    '            doc.Application.Selection.MoveRight(unit:=1, Count:=1)
    '            doc.Application.Selection.MoveLeft(unit:=1, Count:=2)
    '            doc.Application.Selection.InsertBreak(Type:=7)  '7: wdpagebreak
    '            doc.Application.Selection.TypeParagraph()
    '            doc.Application.Selection.TypeParagraph()
    '            doc.Application.Selection.TypeParagraph()
    '            doc.Application.Selection.MoveLeft(unit:=1, Count:=2)
    '            NewPos = 104.9

    '        End If

    '        D = S.Duplicate


    '        D.Name = "overview " & (nbShapes + 1)
    '        D.Left = S.Left

    '        D.RelativeHorizontalPosition = 1   'wdRelativeHorizontalPositionPage
    '        D.RelativeVerticalPosition = 1   'wdRelativeVerticalPositionPage

    '        D.Top = NewPos

    '        D.Select()


    '        D = doc.Sections(1).Range.ShapeRange("BxCorporateClient")
    '        D.RelativeHorizontalPosition = 1   'wdRelativeHorizontalPositionPage
    '        D.RelativeVerticalPosition = 1   'wdRelativeVerticalPositionPage
    '        D.Top = NewPos + S.Height + 10
    '        D.Left = S.Left


    '        GoTo EndOfSub
    'errorHandler:
    '        duplicateOverview = False
    'EndOfSub:
    '    End Function

    '    '*********************************************
    '    ' Renaming bookmarks after insertions
    '    ' Can't have 2 bookmarks with the same name
    '    ' And as we are inserting multiple copies of the
    '    ' same document,....
    '    '*********************************************
    '    Private Function renameDocBookmarks(ByVal doc As Object, ByVal id As Integer, ByVal pub_type As Integer) As Boolean
    '        Dim Bk As Object            'word.bookmark
    '        Dim bkName As String
    '        Dim newBk As Object         'word.bookmark
    '        Dim section As Object       'word.section
    '        Dim Sh As Object            'word.shape
    '        Dim chartLocation As Integer
    '        Dim Shs As Object           'word.shapes
    '        Dim groupedB As Boolean

    '        Select Case pub_type
    '            Case 12
    '                chartLocation = 2
    '                groupedB = True
    '            Case 84
    '                chartLocation = 1
    '                groupedB = False
    '        End Select

    '        renameDocBookmarks = True

    '        On Error GoTo errorHandler

    '        section = doc.Sections(chartLocation)
    '        If groupedB Then
    '            For Each Sh In section.Range.ShapeRange.Item("BC_Group_page1").GroupItems
    '                For Each Bk In Sh.TextFrame.ContainingRange.Bookmarks
    '                    bkName = Bk.Name
    '                    'searching for rnet bookmark that does not have a cdocid string
    '                    If ((Left(bkName, 4) = "rnet") And (InStr(bkName, "_cdocid_") = 0)) Then
    '                        newBk = Bk.Copy(bkName & "_cdocid_" & id)
    '                        Bk.Delete()
    '                    End If
    '                    If bkName = "mainContentBox" Then
    '                        newBk = Bk.Copy(bkName & "_cdocid_" & id)
    '                        Bk.Delete()
    '                    End If
    '                Next
    '            Next
    '        Else
    '            For Each Sh In section.Range.ShapeRange
    '                If Sh.AutoShapeType <> -2 Then
    '                    For Each Bk In Sh.TextFrame.ContainingRange.Bookmarks
    '                        bkName = Bk.Name
    '                        'searching for rnet bookmark that does not have a cdocid string
    '                        If ((Left(bkName, 4) = "rnet") And (InStr(bkName, "_cdocid_") = 0)) Then
    '                            newBk = Bk.Copy(bkName & "_cdocid_" & id)
    '                            Bk.Delete()
    '                        End If
    '                        If bkName = "mainContentBox" Then
    '                            newBk = Bk.Copy(bkName & "_cdocid_" & id)
    '                            Bk.Delete()
    '                        End If
    '                    Next
    '                End If
    '            Next
    '        End If



    '        GoTo EndOfSub
    'errorHandler:
    '        renameDocBookmarks = False
    'EndOfSub:

    '    End Function


    '    '******************************************************
    '    ' Ungroup grouped boxes from the inserted document
    '    '******************************************************
    '    Private Function ungroupAll(ByVal doc As Object, ByVal pub_type As Integer) As Boolean
    '        Dim gSh As Object       'word.shape
    '        Dim chartLocation As Integer

    '        Select Case pub_type
    '            Case 12
    '                chartLocation = 2
    '            Case 4
    '                chartLocation = 1
    '        End Select

    '        ungroupAll = True

    '        On Error GoTo errorHandler

    '        gSh = doc.Sections(chartLocation).Range.ShapeRange("BC_Group_page1")
    '        gSh.Select()
    '        gSh.Ungroup()

    '        GoTo EndOfSub
    'errorHandler:
    '        ungroupAll = False
    'EndOfSub:
    '    End Function



    '    '*************************************************************
    '    ' Create bookmarks for the overview table
    '    '*************************************************************

    '    Private Function createOverviewBookmarks(ByVal doc As Object, ByVal id As Integer) As Boolean
    '        Dim Sh As Object        'word.shape
    '        Dim Bk As Object        'word.bookmark
    '        Dim T As Object         'word.table 
    '        Dim rng As Object       'word.range

    '        createOverviewBookmarks = True

    '        On Error GoTo errorHandler

    '        Sh = doc.Sections(1).Range.ShapeRange("overview " & id)
    '        T = Sh.TextFrame.ContainingRange.Tables(1)

    '        rng = T.Cell(1, 2).Range
    '        rng.Bookmarks.Add("MC_CompanyType_cdocid_" & id)

    '        rng = T.Cell(1, 3).Range
    '        rng.Bookmarks.Add("MC_CompanyName_cdocid_" & id)

    '        rng = T.Cell(1, 4).Range
    '        rng.Bookmarks.Add("MC_CompanySector_cdocid_" & id)

    '        rng = T.Cell(1, 5).Range
    '        rng.Bookmarks.Add("MC_Recommendations_cdocid_" & id)

    '        rng = T.Cell(1, 6).Range
    '        rng.Bookmarks.Add("MC_PreviousRecommendations_cdocid_" & id)

    '        rng = T.Cell(3, 5).Range
    '        rng.Bookmarks.Add("MC_ContentTitle_cdocid_" & id)

    '        rng = T.Cell(4, 5).Range
    '        rng.Bookmarks.Add("MC_Content_cdocid_" & id)

    '        rng = T.Cell(3, 3).Range
    '        rng.Bookmarks.Add("MC_Price_cdocid_" & id)

    '        rng = T.Cell(4, 3).Range
    '        rng.Bookmarks.Add("MC_PriceTarget_cdocid_" & id)

    '        rng = T.Cell(6, 2).Range
    '        rng.Bookmarks.Add("MC_AnalystName_cdocid_" & id)

    '        rng = T.Cell(7, 2).Range
    '        rng.Bookmarks.Add("MC_AnalystPhone_cdocid_" & id)

    '        rng = T.Cell(8, 2).Range
    '        rng.Bookmarks.Add("MC_AnalystMail_cdocid_" & id)


    '        GoTo EndOfSub
    'errorHandler:
    '        createOverviewBookmarks = False
    'EndOfSub:
    '    End Function


    '    '******************************************************
    '    ' Retrieve bookmark values from inserted documents
    '    ' and copy them in the relevant overview table
    '    '******************************************************
    '    Private Function populateValuesToOverview(ByVal doc As Object, ByVal docId As Integer, ByVal pub_type As Integer) As Boolean
    '        Dim section As Object           'word.section
    '        Dim Sh As Object                'word.shape
    '        Dim cRng As Object              'word.range
    '        Dim Bk As Object                'word.bookmark
    '        Dim targetBk As Object          'word.bookmark
    '        Dim pageNb As Integer
    '        Dim nbSectionPages As Integer
    '        Dim overview As Object          'word.shape
    '        Dim sh1 As Object               'word.shape
    '        Dim nbShapes As Integer
    '        Dim tAdviser As String
    '        Dim tBroker As String
    '        Dim chartLocation As Integer
    '        Dim tNomad As String

    '        Select Case pub_type
    '            Case 12
    '                chartLocation = 3
    '            Case 4
    '                chartLocation = 2
    '        End Select
    '        nbShapes = ((doc.Sections(chartLocation).Range.ShapeRange.Count - 1) / 2)

    '        populateValuesToOverview = True
    '        On Error GoTo errorHandler

    '        section = doc.Sections(1)
    '        nbSectionPages = section.Range.Information(3) ' - 1  '3: wdActiveEndPageNumber

    '        ' nbSectionPages = docId
    '        section = doc.Sections(2)

    '        overview = doc.Sections(1).Range.ShapeRange("overview " & docId)

    '        For Each Sh In section.Range.ShapeRange
    '            Sh.Select()
    '            pageNb = doc.application.Selection.Information(3)  'wdActiveEndPageNumber
    '            'If ((pageNb - nbSectionPages) = docId) Then
    '            If ((pageNb - nbSectionPages) = docId) Then
    '                If InStr(Sh.Name, "Page1ContentBox") <> 0 Then

    '                    targetBk = overview.TextFrame.ContainingRange.Bookmarks("MC_ContentTitle_cdocid_" & docId)
    '                    targetBk.Range.Text = reformatString(Sh.TextFrame.ContainingRange.Paragraphs(1).Range.Text)

    '                    targetBk = overview.TextFrame.ContainingRange.Bookmarks("MC_Content_cdocid_" & docId)
    '                    targetBk.Range.Text = Sh.TextFrame.ContainingRange.Paragraphs(3).Range.Text

    '                Else

    '                    For Each Bk In Sh.TextFrame.ContainingRange.Bookmarks
    '                        If (Right(Bk.Name, 3) <> "en0") Then
    '                            If (InStr(Bk.Name, "_at179") <> 0) Then 'Price
    '                                targetBk = overview.TextFrame.ContainingRange.Bookmarks("MC_Price_cdocid_" & docId)
    '                                targetBk.Range.Text = reformatString(Bk.Range.Text)
    '                            End If
    '                            If InStr(Bk.Name, "_at180") <> 0 Then  'Price Target
    '                                targetBk = overview.TextFrame.ContainingRange.Bookmarks("MC_PriceTarget_cdocid_" & docId)
    '                                targetBk.Range.Text = reformatString(Bk.Range.Text)
    '                            End If
    '                            If InStr(Bk.Name, "_at200") <> 0 Then  'Company Name
    '                                targetBk = overview.TextFrame.ContainingRange.Bookmarks("MC_CompanyName_cdocid_" & docId)
    '                                targetBk.Range.Text = reformatString(Bk.Range.Text)
    '                            End If
    '                            If InStr(Bk.Name, "_at203") <> 0 Then  'Sector
    '                                targetBk = overview.TextFrame.ContainingRange.Bookmarks("MC_CompanySector_cdocid_" & docId)
    '                                targetBk.Range.Text = reformatString(Bk.Range.Text)
    '                            End If
    '                            If InStr(Bk.Name, "_at201") <> 0 Then  'company type
    '                                targetBk = overview.TextFrame.ContainingRange.Bookmarks("MC_CompanyType_cdocid_" & docId)
    '                                targetBk.Range.Text = Replace(reformatString(Bk.Range.Text), "Sector: ", "")
    '                            End If
    '                            If InStr(Bk.Name, "_at202") <> 0 Then  'Recommendations
    '                                targetBk = overview.TextFrame.ContainingRange.Bookmarks("MC_Recommendations_cdocid_" & docId)
    '                                targetBk.Range.Text = reformatString(Bk.Range.Text)
    '                            End If
    '                            If InStr(Bk.Name, "_at204") <> 0 Then  'Previous Recommendations
    '                                targetBk = overview.TextFrame.ContainingRange.Bookmarks("MC_PreviousRecommendations_cdocid_" & docId)
    '                                targetBk.Range.Text = reformatString(Bk.Range.Text)
    '                            End If
    '                            If InStr(Bk.Name, "_at197") <> 0 Then  'Analyst Name
    '                                targetBk = overview.TextFrame.ContainingRange.Bookmarks("MC_AnalystName_cdocid_" & docId)
    '                                targetBk.Range.Text = reformatString(Bk.Range.Text)
    '                            End If
    '                            If InStr(Bk.Name, "_at198") <> 0 Then  'Phone
    '                                targetBk = overview.TextFrame.ContainingRange.Bookmarks("MC_AnalystPhone_cdocid_" & docId)
    '                                targetBk.Range.Text = reformatString(Bk.Range.Text)
    '                            End If
    '                            If InStr(Bk.Name, "_at199") <> 0 Then  'Mail
    '                                targetBk = overview.TextFrame.ContainingRange.Bookmarks("MC_AnalystMail_cdocid_" & docId)
    '                                targetBk.Range.Text = reformatString(Bk.Range.Text)
    '                            End If

    '                            If InStr(Bk.Name, "_at194") <> 0 Then  'Adviser
    '                                tAdviser = reformatString(Bk.Range.Text)
    '                            End If
    '                            If InStr(Bk.Name, "_at195") <> 0 Then  'Broker
    '                                tBroker = reformatString(Bk.Range.Text)
    '                            End If
    '                            If InStr(Bk.Name, "_at196") <> 0 Then  'Nomad
    '                                tNomad = reformatString(Bk.Range.Text)
    '                            End If


    '                        End If
    '                    Next 'bookmark
    '                End If
    '            End If
    '        Next 'shape

    '        If ((InStr(tAdviser, "Yes") <> 0) Or (InStr(tBroker, "Yes") <> 0) Or (InStr(tNomad, "Yes") <> 0)) Then
    '            targetBk = overview.TextFrame.ContainingRange.Bookmarks("MC_CompanySector_cdocid_" & docId)
    '            targetBk.Range.Text = reformatString(targetBk.Range.Text & "*")
    '        End If

    '        GoTo EndOfSub
    'errorHandler:
    '        populateValuesToOverview = False
    'EndOfSub:
    '    End Function


    '    '*********************************************
    '    ' Remane ChartKey to include company name
    '    '*********************************************
    '    Private Function renameChartKey(ByVal doc As Object, ByVal id As Integer, ByVal pub_type As Integer) As Boolean
    '        Dim cGroup As Object       'word.shape
    '        Dim chartHolder As Object   'Word.Shape
    '        Dim chartLocation As Integer
    '        renameChartKey = True
    '        On Error GoTo errorHandler

    '        Select Case pub_type
    '            Case 12
    '                chartLocation = 2
    '            Case 4
    '                chartLocation = 1
    '        End Select

    '        cGroup = doc.Sections(chartLocation).Range.ShapeRange("Bx_CompanyDetails")
    '        cGroup.Name = "Bx_CompanyDetails_" & id
    '        chartHolder = doc.Sections(chartLocation + 1).Range.ShapeRange("PriceChart Header " & id)
    '        chartHolder.TextFrame.ContainingRange.Text = "Price Chart for " & reformatString(cGroup.TextFrame.ContainingRange.Tables(1).Cell(1, 1).Range.Text)

    '        GoTo EndOfSub
    'errorHandler:
    '        renameChartKey = False
    'EndOfSub:
    '    End Function



    '    '*********************************************
    '    ' Strips out unwanted characters
    '    '*********************************************
    '    Private Function reformatString(ByVal txt As String) As String
    '        Dim val As String

    '        val = Replace(txt, Chr(13), "")
    '        val = Replace(val, Chr(10), "")
    '        val = Replace(val, vbCrLf, "")
    '        val = Trim(val)
    '        reformatString = val
    '    End Function



    '*************************************************************************************
    ' Charles Stanley.
    ' Merge Version 2.
    ' Pierre [20/03/2007]
    '*************************************************************************************


    Private Function MergeHandbook() As Boolean
        Dim ocommentary As bc_cs_activity_log
        Dim unitpath As String

        Dim docId As Integer

        MergeHandbook = True

        'Me.activedocument.application.visible = True

        For docId = 2 To (Me.num_docs + 1)
            unitpath = Me.local_repos + docId.ToString + ".doc"
            If Not insertDocumentHB(Me.activedocument, docId, unitpath) Then
                ocommentary = New bc_cs_activity_log("bc_am_custom_merge", "MergeHandbook", bc_cs_activity_codes.COMMENTARY, "Unable to insert unit")
                MergeHandbook = False
            End If

        Next docId

    End Function

    'Handbook insertion function
    '***************************

    Private Function insertDocumentHB(ByVal cDoc As Object, ByVal docId As Integer, ByVal unitPath As String) As Boolean
        Dim Bk As Object 'Word.Bookmark
        Dim FolderPath As String
        Dim hasError As Boolean
        Dim ocommentary As bc_cs_activity_log

        On Error GoTo errorHandler
        insertDocumentHB = True

        If cDoc.Bookmarks.Exists("Bmk_Placeholder_Warning") Then
            Bk = cDoc.Bookmarks("Bmk_Placeholder_Warning")
            Bk.Range.Delete()
        End If

        'use the disclosure bookmark location
        Bk = cDoc.Bookmarks("Bmk_Disclosure")
        Bk.Select()
        cDoc.Application.Selection.MoveLeft(unit:=1, Count:=2) 'wdcharacter

        If docId > 2 Then
            cDoc.Application.Selection.InsertBreak(Type:=2) 'wdsectionbreaknextpage
        End If

        cDoc.Application.Selection.InsertFile(unitPath)

        If Not renameUnitBoxes(cDoc, docId) Then
            ocommentary = New bc_cs_activity_log("bc_am_custom_merge", "insertDocumentHB", bc_cs_activity_codes.COMMENTARY, "Unable to rename unit boxes")
            hasError = True
        End If


        GoTo EndOfSub
errorHandler:
        insertDocumentHB = False
        Err.Clear()
EndOfSub:
    End Function


    'Morning Call insertion function
    '***************************
    Private Function MergeMorningCall() As Boolean
        Dim ocommentary As bc_cs_activity_log
        Dim unitPath As String
        Dim docId As Integer
        MergeMorningCall = True

        'Me.activedocument.application.visible = True

        For docId = 2 To (num_docs + 1)
            unitPath = Me.local_repos + docId.ToString + ".doc"
            If Not addSummaryBox(Me.activedocument, docId) Then
                ocommentary = New bc_cs_activity_log("bc_am_custom_merge", "MergeMorningCall", bc_cs_activity_codes.COMMENTARY, "Unable to add a summary box")
                MergeMorningCall = False
            End If
            If Not insertDocument(Me.activedocument, docId, unitPath) Then
                ocommentary = New bc_cs_activity_log("bc_am_custom_merge", "MergeMorningCall", bc_cs_activity_codes.COMMENTARY, "Unable to insert unit")
                MergeMorningCall = False
            End If
        Next docId

        If Not updateCorpoParagraph(Me.activedocument) Then
            ocommentary = New bc_cs_activity_log("bc_am_custom_merge", "MergeMorningCall", bc_cs_activity_codes.COMMENTARY, "Unable to update corporate paragraph")
            MergeMorningCall = False
        End If
        updatePagenumbers(Me.activedocument)
    End Function

    ' Rename boxes to avoid duplicates issues
    '***************************
    Private Function renameUnitBoxes(ByVal cDoc As Object, ByVal id As Integer) As Boolean
        Dim Sh As Object 'Word.Shape

        renameUnitBoxes = True
        On Error Resume Next

        For Each Sh In cDoc.Shapes
            If InStr(Sh.name, "_BC_") = 0 Then
                Sh.name = Sh.name & "_BC_" & id
            End If
        Next

        Err.Clear()

    End Function



    ' Adds a summary box
    '********************************
    Private Function addSummaryBox(ByVal cDoc As Object, ByVal Nb As Integer) As Boolean
        Dim gDoc As Object  'word.document
        Dim Sh As Object 'Word.Shape
        Dim Bk As Object 'Word.Bookmark
        Dim Pos As Integer
        Dim topPos As Integer
        Dim tGalleryPath As String


        addSummaryBox = True
        On Error GoTo errorHandler
        Nb = Nb - 1
        'Do not need to add the 1st summary box
        If Nb = 1 Then
            Exit Function
        End If

        'Opening gallery file and copying summary box
        tGalleryPath = bc_cs_central_settings.local_template_path
        gDoc = cDoc.Application.Documents.Open(FileName:=tGalleryPath & "BC Table Gallery.dot", _
                    Visible:=False)

        Sh = gDoc.Shapes("Morning_Comment_Summary_Box")
        Sh.Select()
        gDoc.Application.Selection.Copy()

        'Bmk_MN_P1_END_Marker
        'Pasting Summary box

        Pos = ((Nb - 1) Mod 4) + 1

        If Pos = 1 Then
            Bk = cDoc.Bookmarks("Bmk_MN_P1_END_Marker")
            Bk.Select()
            cDoc.Application.Selection.MoveLeft(unit:=1, Count:=3) 'unit character
            cDoc.Application.Selection.InsertBreak(Type:=7) 'wdpagebreak

            cDoc.Application.Selection.TypeParagraph()
            cDoc.Application.Selection.TypeParagraph()
            cDoc.Application.Selection.TypeParagraph()
            cDoc.Application.Selection.TypeParagraph()
            cDoc.Application.Selection.TypeParagraph()
        End If

        Bk = cDoc.Bookmarks("Bmk_MN_P1_END_Marker")
        Bk.Select()

        cDoc.Application.Selection.MoveLeft(unit:=1, Count:=5) 'wdcharacter
        cDoc.Application.Selection.Paste()

        'Closing gallery document
        gDoc.Close(SaveChanges:=False)
        gDoc = Nothing
        Sh = Nothing

        Sh = cDoc.Shapes("Morning_Comment_Summary_Box")
        Sh.name = "Bx_Summary_BC_" & Nb

        Sh.RelativeHorizontalPosition = 1 'wdRelativeHorizontalPositionPage
        Sh.RelativeVerticalPosition = 1 'wdRelativeVerticalPositionPage
        Sh.Left = 0

        topPos = 153 + ((Pos - 1) * 140)
        Sh.Top = topPos


        GoTo EndOfSub
errorHandler:
        addSummaryBox = False
        Err.Clear()
EndOfSub:
    End Function


    ' Insert a unit
    '********************************
    Private Function insertDocument(ByVal cDoc As Object, ByVal id As Integer, ByVal unitPath As String) As Boolean
        Dim Bk As Object 'Word.Bookmark
        Dim FolderPath As String
        Dim hasError As Boolean
        Dim ocommentary As bc_cs_activity_log

        hasError = False
        insertDocument = True
        cDoc.application.visible = True

        On Error GoTo errorHandler


        If cDoc.Bookmarks.Exists("Bmk_Placeholder_Warning") Then
            Bk = cDoc.Bookmarks("Bmk_Placeholder_Warning")
            Bk.Range.Delete()
        End If


        Bk = cDoc.Bookmarks("Bmk_Disclosure")
        Bk.Select()
        cDoc.Application.Selection.MoveLeft(unit:=1, Count:=2) 'wdcharacter

        If id > 2 Then
            cDoc.Application.Selection.InsertBreak(Type:=2) 'wdsectionbreaknextpage
        End If

        cDoc.Application.Selection.InsertFile(unitPath)

        If Not renameUnitBoxes(cDoc, id) Then
            ocommentary = New bc_cs_activity_log("bc_am_custom_merge", "insertDocumet", bc_cs_activity_codes.COMMENTARY, "Unable to rename unit boxes")
            hasError = True
        End If
        If Not populateSummary(cDoc, id) Then
            ocommentary = New bc_cs_activity_log("bc_am_custom_merge", "insertDocumet", bc_cs_activity_codes.COMMENTARY, "Unable to populate summary")
            hasError = True
        End If
        If Not insertSharePerfGraph(cDoc, id) Then
            ocommentary = New bc_cs_activity_log("bc_am_custom_merge", "insertDocumet", bc_cs_activity_codes.COMMENTARY, "Unable to insert share perf graph")
            hasError = True
        End If
        If Not deleteInsertedDisclaimer(cDoc, id) Then
            ocommentary = New bc_cs_activity_log("bc_am_custom_merge", "insertDocumet", bc_cs_activity_codes.COMMENTARY, "Unable to delete unit disclaimer")
            hasError = True
        End If
        If Not updateDisclaimerNotification(cDoc) Then
            ocommentary = New bc_cs_activity_log("bc_am_custom_merge", "insertDocumet", bc_cs_activity_codes.COMMENTARY, "Unable to update disclaimer notification")
            hasError = True
        End If

        insertDocument = Not hasError

        GoTo EndOfSub
errorHandler:
        insertDocument = False
        Err.Clear()
EndOfSub:
    End Function

    ' Delete Unit disclaimer
    '***************************
    Private Function deleteInsertedDisclaimer(ByVal cDoc As Object, ByVal id As Integer) As Boolean
        Dim Rng As Object 'Word.Range

        'Dim cDoc As Word.Document
        'Set cDoc = ActiveDocument

        deleteInsertedDisclaimer = True
        On Error GoTo errorHandler

        Rng = cDoc.Sections(id + 1).Range

        Rng.Select()
        Rng.Delete()

        GoTo EndOfSub
errorHandler:
        deleteInsertedDisclaimer = False
        Err.Clear()
EndOfSub:
    End Function

    ' populate summary box with unit content
    '***************************
    Private Function populateSummary(ByVal cDoc As Object, ByVal Id As Integer) As Boolean
        Dim Sh1 As Object 'Word.Shape
        Dim Sh2 As Object 'Word.Shape
        Dim T1 As Object 'Word.Table
        Dim T2 As Object 'Word.Table
        Dim Txt As String
        Dim corpo As Boolean

        Dim CmpName As String

        populateSummary = True
        On Error GoTo errorHandler


        Sh1 = cDoc.Shapes("Bx_Summary_BC_" & Id - 1)
        T1 = Sh1.TextFrame.ContainingRange.Tables(1)



        Sh2 = cDoc.Shapes("Bx_handbook_Recommendation_BC_" & Id)
        T2 = Sh2.TextFrame.ContainingRange.Tables(1)
        'Recommendation-------------------------------------------
        Txt = T2.Cell(1, 1).Range.Text
        T1.Cell(1, 6).Range.Text = Left(Txt, Len(Txt) - 2)


        Sh2 = cDoc.Shapes("Bx_Sector_BC_" & Id)
        T2 = Sh2.TextFrame.ContainingRange.Tables(1)
        'Sector-------------------------------------------
        Txt = T2.Cell(1, 3).Range.Text
        T1.Cell(1, 3).Range.Text = Left(Txt, Len(Txt) - 2)

        Sh2 = cDoc.Shapes("CompanyName_BC_" & Id)
        T2 = Sh2.TextFrame.ContainingRange.Tables(1)
        'Company Name-------------------------------------------
        Txt = T2.Cell(1, 1).Range.Text
        CmpName = Left(Txt, Len(Txt) - 2)
        T1.Cell(1, 5).Range.Text = CmpName

        Sh2 = cDoc.Shapes("CompanyInfo_BC_" & Id)
        T2 = Sh2.TextFrame.ContainingRange.Tables(1)
        'Price-------------------------------------------
        Txt = T2.Cell(1, 4).Range.Text
        T1.Cell(2, 4).Range.Text = Left(Txt, Len(Txt) - 2)
        'Target price-------------------------------------------
        Txt = T2.Cell(2, 4).Range.Text
        T1.Cell(3, 4).Range.Text = Left(Txt, Len(Txt) - 2)

        'Analyst name-------------------------------------------
        Txt = T2.Cell(20, 3).Range.Text
        T1.Cell(5, 3).Range.Text = Left(Txt, Len(Txt) - 2)

        'Analyst phone-------------------------------------------
        Txt = T2.Cell(21, 3).Range.Text
        T1.Cell(6, 3).Range.Text = Left(Txt, Len(Txt) - 2)

        'Analyst mail-------------------------------------------
        Txt = T2.Cell(22, 3).Range.Text
        T1.Cell(7, 3).Range.Text = Left(Txt, Len(Txt) - 2)

        'RIC-------------------------------------------
        Txt = T2.Cell(10, 4).Range.Text
        T1.Cell(1, 5).Range.Text = Left(T1.Cell(1, 5).Range.Text, Len(T1.Cell(1, 5).Range.Text) - 2) & " (" & Left(Txt, Len(Txt) - 2) & ")"

        'Corporate Client-------------------------------------------
        If ((InStr(T2.Cell(17, 4).Range.Text, "Yes") <> 0) Or (InStr(T2.Cell(18, 4).Range.Text, "Yes") <> 0)) Then
            corpo = True
        Else
            corpo = False
        End If

        Sh2 = cDoc.Shapes("FrontPageContent_BC_" & Id)
        'Document Title-------------------------------------------
        Txt = Sh2.TextFrame.ContainingRange.Paragraphs(1).Range.Text
        T1.Cell(3, 6).Range.Text = Left(Txt, Len(Txt) - 1)


        Txt = Sh2.TextFrame.ContainingRange.Paragraphs(3).Range.Text
        T1.Cell(4, 6).Range.Text = Left(Txt, Len(Txt) - 1)


        If corpo Then
            T1.Cell(1, 5).Range.Text = Left(T1.Cell(1, 5).Range.Text, Len(T1.Cell(1, 5).Range.Text) - 2) & "*"
            If Me.corpoClientList = "" Then
                Me.corpoClientList = CmpName
            Else
                Me.corpoClientList = Me.corpoClientList & ", " & CmpName
            End If
        End If

        GoTo EndOfSub
errorHandler:
        populateSummary = False
        Err.Clear()
EndOfSub:
    End Function


    Private Function insertSharePerfGraph(ByVal cDoc As Object, ByVal id As Integer) As Boolean
        Dim Sh As Object 'Word.Shape
        Dim T As Object 'Word.Table
        Dim T0 As Object 'Word.Table
        Dim data As DataObject

        insertSharePerfGraph = True
        On Error GoTo errorHandler

        T0 = addNewSharePerf(cDoc, id)
        If T0 Is Nothing Then
            insertSharePerfGraph = False
            Exit Function
        End If

        Sh = cDoc.Shapes("Bx_share_price_graph_BC_" & id)
        T = Sh.TextFrame.ContainingRange.Tables(1)

        T.Cell(2, 1).Range.Select()
        cDoc.Application.Selection.Copy()
        data = Clipboard.GetDataObject()
        If data.GetFormats.Length <> 0 Then
            T0.Cell(2, 1).Range.Paste()
        End If

        GoTo EndOfSub
errorHandler:
        insertSharePerfGraph = False
        Err.Clear()
EndOfSub:
    End Function

    Private Function addNewSharePerf(ByVal cDoc As Object, ByVal Id As Integer) As Object  'Word.Table
        Dim T As Object 'Word.Table
        Dim Bk As Object 'Word.Bookmark
        Dim NbR As Integer
        Dim Sh As Object 'Word.Shape
        Dim Rng As Object ' Word.range
        Dim Txt As String


        On Error GoTo errorHandler

        If Id = 2 Then
            'Adding the rating distribution table.
            Sh = cDoc.Shapes("Bx_rating_dist_BC_" & Id)
            T = Sh.TextFrame.ContainingRange.Tables(1)
            T.Range.Copy()
            Bk = cDoc.Bookmarks("Bmk_Rating_Dist")
            Bk.Select()
            Bk.Range.Paste()

            'remove line that appears after the paste
            Bk.Select()
            cDoc.Application.Selection.Borders(-1).LineStyle = 0 'bordertop & wdLineStyleNone

            cDoc.Application.Selection.Tables(1).Rows.WrapAroundText = True

            Bk = cDoc.Bookmarks("Bmk_Share_perf_tbl")
            T = Bk.Range.Tables(1)
            addNewSharePerf = T

            Sh = cDoc.Shapes("Bx_Summary_BC_1")
            Txt = Sh.TextFrame.ContainingRange.Tables(1).Cell(1, 5).Range.Text
            Txt = Left(Txt, Len(Txt) - 1)

            T.Cell(1, 1).Range.Text = "Share performance - " & Txt
            T.Cell(1, 1).Range.Style = "CS Side note Bold"

            Exit Function
        End If

        Bk = cDoc.Bookmarks("Bmk_Share_perf_tbl")

        Bk.Select()
        cDoc.Application.Selection.MoveRight(unit:=1, Count:=2) 'wdcharacter
        cDoc.Application.Selection.TypeParagraph()
        Rng = cDoc.application.selection.range
        T = cDoc.Application.Selection.Tables.Add(Rng, 3, 3)
        T.Borders(-2).LineStyle = 0  'wdborderleft  / wdlinestylenone
        T.Borders(-4).LineStyle = 0  'wdBorderRight / wdlinestylenone
        T.Borders(-1).LineStyle = 0 'wdBorderTop / wdlinestylenone
        T.Borders(-3).LineStyle = 0   'wdBorderBottom / wdlinestylenone
        T.Borders(-5).LineStyle = 0  'wdBorderHorizontal / wdlinestylenone
        T.Borders(-6).LineStyle = 0  'wdBorderVertical / wdlinestylenone

        NbR = T.Rows.Count

        T.Cell(3, 1).Range.Bookmarks.Add("Bmk_Share_perf_tbl")


        T.Rows.SetLeftIndent(LeftIndent:=5.4, RulerStyle:=0)  'wdAdjustNone
        T.Columns(3).SetWidth(ColumnWidth:=149.6, RulerStyle:=0) 'wdAdjustNone


        SelectCells(T, T.Rows.Count, 1, 1, 3)

        cDoc.Application.Selection.Cells.Merge()

        formatCell(T, 1, 1)
        formatCell(T, 1, 2)
        formatCell(T, 1, 3)
        formatCell(T, 2, 1)
        formatCell(T, 2, 2)
        formatCell(T, 2, 3)
        T.Rows(2).height = 140
        T.Rows(2).HeightRule = 2 'wdRowHeightExactly

        T.Rows(1).height = 12
        T.Rows(1).HeightRule = 2 'wdRowHeightExactly

        SelectCells(T, 1, 1, 1, 3)

        With cDoc.Application.Selection.Cells
            .Borders(-2).LineStyle = 0 'borderleft / none
            .Borders(-4).LineStyle = 0  'borderright / none
            With .Borders(-1) 'bordertop
                .LineStyle = 1 'wdLineStyleSingle
                .LineWidth = 4 'wdLineWidth050pt
                .Color = 2196197
            End With
        End With

        T.Cell(1, 3).Range.Style = "CS Bar"

        SelectCells(T, 2, 1, 1, 1)

        With cDoc.Application.Selection.Cells
            With .Borders(-3)  'borderbottom
                .LineStyle = 1 ' wdLineStyleSingle
                .LineWidth = 2 'wdLineWidth025pt
                .Color = 2196197
            End With
        End With

        Sh = cDoc.Shapes("Bx_Summary_BC_" & Id - 1)
        Txt = Sh.TextFrame.ContainingRange.Tables(1).Cell(1, 5).Range.Text
        Txt = Left(Txt, Len(Txt) - 1)

        T.Cell(1, 1).Range.Text = "Share performance - " & Txt
        T.Cell(1, 1).Range.Style = "CS Side note Bold"

        T.Rows.WrapAroundText = True
        addNewSharePerf = T


        GoTo EndOfSub
errorHandler:
        addNewSharePerf = Nothing
        MsgBox(Err.Description)
        Err.Clear()
EndOfSub:
    End Function


    Private Function formatCell(ByVal T As Object, ByVal R As Integer, ByVal C As Integer) 'word.table


        On Error GoTo errorHandler

        T.Cell(R, C).TopPadding = 0
        T.Cell(R, C).BottomPadding = 0
        T.Cell(R, C).LeftPadding = 0
        T.Cell(R, C).RightPadding = 0
        T.Cell(R, C).WordWrap = True
        T.Cell(R, C).FitText = False

        Select Case C
            Case 1
                T.Cell(R, 1).width = 339.2
            Case 2
                T.Cell(R, 2).width = 11.8
            Case 3
                T.Cell(R, 3).width = 47.2
        End Select

        GoTo EndOfSub
errorHandler:
        Err.Clear()
EndOfSub:
    End Function


    ' Generic function
    ' Selects several cells from a given table
    '********************************
    Function SelectCells(ByRef tbl As Object, _
    ByVal rowStart As Integer, ByVal colStart As Integer, _
            ByVal rowEnd As Integer, ByVal colEnd As Integer) _
                As Object 'word.selection

        Dim sel As Object 'Word.Selection

        Dim nrRows As Long
        Dim nrCols As Long

        sel = Nothing
        nrRows = tbl.Rows.Count
        nrCols = tbl.Columns.Count

        'Make sure the start cell exists in the table
        If rowStart > nrRows Then Return Nothing
        If colStart > nrCols Then Return Nothing

        'Make sure the end point exists in the table
        'If it does not, set the last row/column as end points
        If rowEnd >= nrRows Then rowEnd = nrRows - rowStart + 1
        If colEnd >= nrCols Then colEnd = nrCols - colStart + 1

        'Select the start cell
        tbl.Cell(rowStart, colStart).Select()
        sel = tbl.application.Selection

        'Make sure the selection will extend
        sel.ExtendMode = True

        'First select the start cell
        sel.Expand(unit:=12) 'wdCell

        'Now extend across the columns
        '-1 because first row and col are already selected
        sel.MoveRight(unit:=1, Count:=colEnd - 1, Extend:=True) ' wdCharacter

        'And now extend down the rows
        sel.MoveDown(unit:=5, Count:=rowEnd - 1, Extend:=True)  'wdLine
        SelectCells = sel
    End Function

    'update disclaimer text

    '***************************
    Private Function updateDisclaimerNotification(ByVal cDoc As Object) As Boolean
        Dim Sh As Object 'Word.Shape
        Dim Pg As Integer
        Dim Bk As Object 'Word.Bookmark

        'Dim cDoc As Word.Document
        'Set cDoc = ActiveDocument
        On Error GoTo errorHandler
        updateDisclaimerNotification = True

        Bk = cDoc.Bookmarks("Bmk_Disclosure")

        Bk.Select()
        Pg = cDoc.Application.Selection.Information(3)  'wdActiveEndPageNumber

        Sh = cDoc.Shapes("Bx_Notification_BC_DISCLOSURE")
        Sh.TextFrame.ContainingRange.Text = "Please see important disclaimers and disclosures on page " & Pg

        GoTo EndOfSub
errorHandler:
        updateDisclaimerNotification = False
        Err.Clear()
EndOfSub:
    End Function


    Private Function updateCorpoParagraph(ByVal cDoc As Object) As Boolean
        Dim Bk As Object 'Word.Bookmark
        Dim Txt As String
        Dim origTxt As String

        'Dim cDoc As Word.Document
        'Set cDoc = ActiveDocument

        updateCorpoParagraph = True
        On Error GoTo errorHandler
        Bk = cDoc.Bookmarks("Bmk_Corporate_Client")
        If corpoClientList <> "" Then

            Txt = "{COMPNAME} is a corporate client of Charles Stanley Securities. Charles Stanley Securities is party to an agreement with {COMPNAME} for the provision of investment banking services and the production of research.  Charles Stanley Securities may have a principal position in the shares of {COMPNAME_OR}."
            Txt = Replace(Txt, "{COMPNAME}", corpoClientList)
            Txt = Replace(Txt, "{COMPNAME_OR}", Replace(corpoClientList, ", ", " or "))

        Else
            Txt = ""
        End If

        Bk.Range.Text = Txt

        GoTo EndOfSub
errorHandler:
        updateCorpoParagraph = False
        Err.Clear()
EndOfSub:
    End Function


    Private Sub updatePagenumbers(ByVal cDoc As Object) 'Word.Document)
        Dim HF As Object 'Word.HeaderFooter
        Dim S As Object 'Word.Section

        'Dim cDoc As Word.Document
        'Set cDoc = ActiveDocument

        On Error Resume Next

        For Each S In cDoc.Sections
            For Each HF In S.Footers
                HF.PageNumbers.RestartNumberingAtSection = False
                HF.PageNumbers.StartingNumber = 0
            Next
        Next


    End Sub

End Class
Public Class bc_am_landscape_page
    Private ao_object As Object
    Private ao_type As String
    Public Sub New(ByVal ao_object As Object, ByVal ao_type As String)
        Me.ao_object = ao_object
        Me.ao_type = ao_type
    End Sub
    Public Function insert()
        insert = False
        Try
            If UCase(ao_type) <> "WORD" Then
                Exit Function
            End If
            Dim oword As New bc_ao_word(ao_object)
            If oword.insert_landscape_page = False Then
                Exit Function
            End If
            insert = False

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_landscape_page", "insert", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Function
End Class





