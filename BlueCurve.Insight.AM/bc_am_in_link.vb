Imports BlueCurve.Core.OM
Imports BlueCurve.Core.CS
Imports BlueCurve.Core.AS
Imports System.Collections
Imports System.Windows.Forms
REM ==========================================
REM paul coments Bluecurve Limited 2005
REM Module:       Insight Link
REM Type:         Application Module
REM Description:  Links Excel worksheet
REM Version:      1
REM Change history
REM ==========================================
REM constructor determines whether to show the
REM submit form or not
REM FIL JUly 2012
Public Class bc_am_in_clone
    Dim ao_type As Integer
    Dim ao_object As Object
    Dim load_failed As Boolean = False
    Dim odoc As bc_ao_in_object
    Dim odocmetadata As New bc_om_document
    Dim obc_objects As New bc_am_load_objects
    REM constructor determines whether to show the
    REM submit form or not the active object and the type
    Public Sub New(ByVal ao_object As Object, ByVal ao_type As String)
        Dim slog = New bc_cs_activity_log("bc_am_in_clone", "new", bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim oexcel As bc_ao_in_object = Nothing
        Dim ocommentary As bc_cs_activity_log
        Dim omessage As bc_cs_message
        'Dim ofrmlink As bc_am_in_main_frm = Nothing
        Try
            If ao_type <> bc_ao_in_object.EXCEL_DOC Then
                omessage = New bc_cs_message("Blue Curve insight", "Application Object Type: " + ao_type + " not supported", bc_cs_message.MESSAGE)
            Else
                REM load system into memory
                bc_cs_central_settings.version = Application.ProductVersion
                REM instantiate correct AO object
                If ao_type = bc_ao_in_object.EXCEL_DOC Then
                    If IsNothing(ao_object) Then
                        omessage = New bc_cs_message("Blue Curve insight", "Workbook must be Open", bc_cs_message.MESSAGE)
                        load_failed = True
                        Exit Sub
                    End If
                    oexcel = New bc_ao_in_excel(ao_object)
                    'If oexcel.issaved = False Then
                    '    omessage = New bc_cs_message("Blue Curve insight", "Workbook must be saved prior to clone", bc_cs_message.MESSAGE)
                    '    Exit Sub
                    'End If
                    oexcel.screen_updating(True)
                    oexcel.disable_application_alerts()
                Else
                    omessage = New bc_cs_message("Blue Curve insight", "mime type: " + ao_type + " not supported.", bc_cs_message.MESSAGE)
                    Exit Sub
                End If
                If oexcel.get_sheet_keys = "Read Only" Then
                    omessage = New bc_cs_message("Blue Curve insight", "Workbook is read only copy cant build.", bc_cs_message.MESSAGE)
                    Exit Sub
                End If

                If oexcel.get_sheet_keys = "No Keys Found" Then
                    omessage = New bc_cs_message("Blue Curve insight", "Workbook is not linked to Blue Curve so cannot clone", bc_cs_message.MESSAGE)
                    Exit Sub
                End If



                bc_am_load_objects.obc_om_insight_submission_entity_links.bc_om_insight_submission_entity_links.Clear()
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    bc_am_load_objects.obc_om_insight_submission_entity_links.db_read()
                Else
                    bc_am_load_objects.obc_om_insight_submission_entity_links.tmode = bc_cs_soap_base_class.tREAD
                    bc_am_load_objects.obc_om_insight_submission_entity_links.transmit_to_server_and_receive(bc_am_load_objects.obc_om_insight_submission_entity_links, True)
                End If
                REM get key metdata of original workbook
                Dim oschema As Long = oexcel.get_key_value(2, "BCSHEET1")
                Dim oentity As Long = oexcel.get_key_value(3, "BCSHEET1")
                Dim oclass As Long = oexcel.get_key_value(4, "BCSHEET1")
                REM get primary template id
                Dim optemplate As Long = oexcel.get_key_value(5, "BCSHEET1")
                REM child template id
                Dim sost As String = oexcel.get_key_value(5, "BCSHEET2")
                Dim ostemplate As Integer
                If IsNumeric(sost) Then
                    ostemplate = CInt(sost)
                End If
                REM get number of child templates

                Dim onumchild As Integer = 0
                Dim sname As String
                Dim t As String
                Dim i As Integer = 2

                sname = "BCSHEET" + CStr(i)
                t = ""
                t = oexcel.get_key_value(5, sname)
                While IsNumeric(t) = True
                    onumchild = onumchild + 1
                    i = i + 1
                    sname = "BCSHEET" + CStr(i)
                    t = ""
                    t = oexcel.get_key_value(5, sname)
                End While

                Dim ffs As New bc_dx_insight_build

                Dim cfs As New Cbc_dx_insight_build
                If cfs.load_data(ffs, "Insight - Clone", "Clone") = False Then
                    Exit Sub
                Else
                    ffs.ShowDialog()
                End If
                If cfs.lead_entity_id = 0 Then
                    ocommentary = New bc_cs_activity_log("bc_am_in_link", "new", bc_cs_activity_codes.COMMENTARY, "Cancel Selected on Link form.")
                    Exit Sub
                End If


                'ofrmlink = New bc_am_in_main_frm
                'REM read meta data object to form
                'ofrmlink.ShowDialog()
                'REM write form data to metadata object
                'If Not ofrmlink.ok_selected Then
                '    REM if cancel selected do nothing
                '    ocommentary = New bc_cs_activity_log("bc_am_in_link", "new", bc_cs_activity_codes.COMMENTARY, "Cancel Selected on Link form.")
                '    Exit Sub
                'End If
                REM check schema id and class id are the same
                'If oschema <> ofrmlink.contributor_id Or oclass <> ofrmlink.class_id Then
                '    omessage = New bc_cs_message("Blue Curve insight", "Selection is not in same Class and Contributor Cannot create a Clone.", bc_cs_message.MESSAGE)
                '    Exit Sub
                'End If
                'REM check not the same entity
                'If oentity = ofrmlink.entity_id Then
                '    omessage = New bc_cs_message("Blue Curve insight", "Cannot create a clone of the same entity.", bc_cs_message.MESSAGE)
                '    Exit Sub
                'End If

                If oschema <> cfs.schema_id Or oclass <> cfs.class_id Then
                    omessage = New bc_cs_message("Blue Curve insight", "Selection is not in same Class and Contributor Cannot create a Clone.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub
                End If
                REM check not the same entity
                If oentity = cfs.lead_entity_id Then
                    omessage = New bc_cs_message("Blue Curve insight", "Cannot create a clone of the same entity.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub
                End If
              
                REM load data for the contribution object based on selected criteria
                'bc_am_load_objects.obc_om_insight_contribution_for_entity = New bc_om_insight_contribution_for_entity(ofrmlink.class_id, ofrmlink.class_name, ofrmlink.entity_id, ofrmlink.entity_name, ofrmlink.contributor_id, ofrmlink.contributor_name)
                bc_am_load_objects.obc_om_insight_contribution_for_entity = New bc_om_insight_contribution_for_entity(cfs.class_id, cfs.class_name, cfs.lead_entity_id, cfs.entity_name, cfs.schema_id, cfs.schema_name)

                REM rem now check same financial template

                bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve insight", "Retrieving Configuration Data for Workbook", 10, False, True)


                bc_am_load_objects.obc_om_insight_contribution_for_entity.certificate.user_id = bc_cs_central_settings.logged_on_user_id
                bc_am_load_objects.obc_om_insight_contribution_for_entity.certificate.name = bc_cs_central_settings.logged_on_user_name
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    Dim gbc_db As New bc_cs_db_services(False)
                    bc_am_load_objects.obc_om_insight_contribution_for_entity.db_read()
                ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                    bc_am_load_objects.obc_om_insight_contribution_for_entity.tmode = bc_cs_soap_base_class.tREAD
                    bc_am_load_objects.obc_om_insight_contribution_for_entity.transmit_to_server_and_receive(bc_am_load_objects.obc_om_insight_contribution_for_entity, True)
                End If
                REM check primary template ids match
                If bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets.Count = 0 Then
                    omessage = New bc_cs_message("Blue Curve insight", "Clone Doesnt have a financial template assigned", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub

                End If
                If bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets(0).logical_template_Id <> optemplate Then
                    omessage = New bc_cs_message("Blue Curve insight", "Primary financial templates are not the same, cloning is not allowed", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub
                End If
                REM check child sheets are the same number
                If bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets.Count - 1 <> onumchild Then
                    omessage = New bc_cs_message("Blue Curve insight", "Number of Child sheets doesnt match cannot clone.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                End If

                REM check child template ids macth
                If onumchild > 0 Then
                    If bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets(1).logical_template_Id <> ostemplate Then
                        omessage = New bc_cs_message("Blue Curve insight", "Child financial templates are not the same cannot clone.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                        Exit Sub
                    End If
                End If


                REM copy activeworkbook
                Dim ofn As String = oexcel.filename
                Dim nfn As String = ""
                Dim fs As New bc_cs_file_transfer_services

                REM write down metadata file
                REM get temp filename
                Dim ext As String
                ext = Right(ofn, ofn.Length - InStrRev(ofn, "."))
                ext = "." + ext

                i = 1
                Dim found As Boolean
                While found = False
                    nfn = bc_cs_central_settings.local_repos_path + "clone" + CStr(i) + ext
                    If fs.check_document_exists(nfn) = True Then
                        If fs.delete_file(nfn) = True Then
                            If oexcel.is_workbook_open("clone" + CStr(i) + ext) = False Then
                                found = True
                            End If
                        End If
                    ElseIf fs.check_document_exists(nfn) = False Then
                        found = True
                    End If
                    i = i + 1
                End While
                REM now check workbook with this name isnt already opened



                bc_cs_central_settings.progress_bar.increment("Copying file")
                fs.file_copy(ofn, nfn)


                bc_cs_central_settings.progress_bar.increment("Saving metadate")
                Dim filename As String
                filename = "insight_" + CStr(bc_am_load_objects.obc_om_insight_contribution_for_entity.lead_entity_id) + "_" + CStr(bc_am_load_objects.obc_om_insight_contribution_for_entity.contributor_id) + ".xml"
                bc_am_load_objects.obc_om_insight_contribution_for_entity.write_data_to_file(bc_cs_central_settings.local_repos_path + filename)

                REM open clones workbook
                bc_cs_central_settings.progress_bar.increment("Opening Cloned Workbook")
                Dim new_wb As Object
                REM set the new entity ids and names
                Dim nexcel As bc_ao_in_object
                nexcel = New bc_ao_in_excel(ao_object)

                new_wb = oexcel.open_clone(nfn, "Clone: " + bc_am_load_objects.obc_om_insight_contribution_for_entity.lead_entity_name)
                nexcel = New bc_ao_in_excel(new_wb)
                nexcel.set_key_value(3, 1, "BCSHEET1", bc_am_load_objects.obc_om_insight_contribution_for_entity.lead_entity_id)
                nexcel.unprotect_sheet()


                nexcel.set_key_value(3, 6, "BCSHEET1", bc_am_load_objects.obc_om_insight_contribution_for_entity.lead_entity_name)
                Dim sheet_name, alt_sheet_name As String
                sheet_name = bc_am_load_objects.obc_om_insight_contribution_for_entity.lead_class_name + " " + bc_am_load_objects.obc_om_insight_contribution_for_entity.lead_entity_name
                alt_sheet_name = bc_am_load_objects.obc_om_insight_contribution_for_entity.lead_class_name + "_BCSHEET1_1"

                nexcel.set_sheet_name("BCSHEET1", sheet_name, alt_sheet_name)


                Dim sn As String
                For i = 1 To bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets.Count - 1
                    sn = "BCSHEET" + CStr(i + 1)
                    nexcel.set_key_value(3, 1, sn, bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets(i).entity_id)
                    nexcel.set_key_value(3, 6, sn, bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets(i).entity_name)
                    sheet_name = bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets(i).class_name + " " + bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets(i).entity_name
                    alt_sheet_name = bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets(i).class_name + "_" + sn + "_1"
                    nexcel.set_sheet_name(sn, sheet_name, alt_sheet_name)
                Next

            End If


        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_clone", "new", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            If load_failed = False Then
                oexcel.enable_application_alerts()
                oexcel.screen_updating(False)
            End If
            bc_cs_central_settings.progress_bar.unload()
            slog = New bc_cs_activity_log("bc_am_in_clone", "new", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
End Class


Public Class bc_am_in_build
    Dim ao_type As Integer
    Dim ao_object As Object
    Dim load_failed As Boolean = False
    Dim odoc As bc_ao_in_object
    Dim odocmetadata As New bc_om_document
    Dim obc_objects As New bc_am_load_objects
    REM constructor determines whether to show the
    REM submit form or not the active object and the type
    Public Sub New(ByVal show_form As Boolean, ByVal ao_object As Object, ByVal ao_type As String, ByVal link_now As Boolean)
        Dim slog = New bc_cs_activity_log("bc_am_in_link", "new", bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim oexcel As bc_ao_in_object = Nothing
        Dim ocommentary As bc_cs_activity_log
        Dim omessage As bc_cs_message
        'Dim ofrmlink As bc_am_in_main_frm = Nothing
        Try
            If ao_type <> bc_ao_in_object.EXCEL_DOC Then
                omessage = New bc_cs_message("Blue Curve insight", "Application Object Type: " + ao_type + " not supported", bc_cs_message.MESSAGE)
            Else
                REM load system into memory
                bc_cs_central_settings.version = Application.ProductVersion
                REM instantiate correct AO object
                If ao_type = bc_ao_in_object.EXCEL_DOC Then
                    If IsNothing(ao_object) Then
                        omessage = New bc_cs_message("Blue Curve insight", "Workbook must be Open", bc_cs_message.MESSAGE)
                        load_failed = True
                        Exit Sub
                    End If
                    oexcel = New bc_ao_in_excel(ao_object)
                    oexcel.screen_updating(True)
                    oexcel.disable_application_alerts()
                Else
                    omessage = New bc_cs_message("Blue Curve insight", "mime type: " + ao_type + " not supported.", bc_cs_message.MESSAGE)
                    Exit Sub
                End If
                If oexcel.get_sheet_keys = "Read Only" Then
                    omessage = New bc_cs_message("Blue Curve insight", "Workbook is read only copy cant build.", bc_cs_message.MESSAGE)
                    Exit Sub
                End If

                If oexcel.get_sheet_keys <> "No Keys Found" Then
                    omessage = New bc_cs_message("Blue Curve insight", "Workbook is already linked to Research Net please Re-build", bc_cs_message.MESSAGE)
                    REM put re-link call here
                    Exit Sub
                End If
                 bc_am_load_objects.obc_om_insight_submission_entity_links.bc_om_insight_submission_entity_links.Clear()
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    bc_am_load_objects.obc_om_insight_submission_entity_links.db_read()
                Else
                    bc_am_load_objects.obc_om_insight_submission_entity_links.tmode = bc_cs_soap_base_class.tREAD
                    bc_am_load_objects.obc_om_insight_submission_entity_links.transmit_to_server_and_receive(bc_am_load_objects.obc_om_insight_submission_entity_links, True)
                End If

                If show_form = True Then
                    REM if can identify entity and schema by properties then dont show form
                    Dim lent As String
                    Dim lsch As String
                    Dim lclassid As String
                    Dim lclassname As String
                    Dim lentname As String
                    Dim lschname As String
                    Dim auto_assign As Boolean = False
                    lent = ""
                    lsch = ""
                    lclassid = ""
                    lent = oexcel.get_property("rnet_entity_id")
                    lsch = oexcel.get_property("rnet_schema_id")
                    lclassid = oexcel.get_property("rnet_class_id")
                    lentname = oexcel.get_property("rnet_entity_name")
                    lschname = oexcel.get_property("rnet_schema_name")
                    lclassname = oexcel.get_property("rnet_class_name")
                    If lent <> "" And lsch <> "" And lclassid <> "" And lentname <> "" And lclassname <> "" And lschname <> "" Then
                        If IsNumeric(lent) = True And IsNumeric(lsch) = True And IsNumeric(lclassid) = True Then
                            auto_assign = True
                        End If
                    End If
                    Dim cfl As New Cbc_dx_insight_build
                    If auto_assign = False Then
                        'ofrmlink = New bc_am_in_main_frm
                        REM read meta data object to form
                        'ofrmlink.ShowDialog()
                        REM 5.8

                        Dim ffl As New bc_dx_insight_build

                        If cfl.load_data(ffl, "Insight - Build", "Build") = False Then
                            Exit Sub
                        Else
                            ffl.ShowDialog()

                        End If
                        If cfl.lead_entity_id = 0 Then
                            ocommentary = New bc_cs_activity_log("bc_am_in_link", "new", bc_cs_activity_codes.COMMENTARY, "Cancel Selected on Link form.")
                            Exit Sub
                            Exit Sub
                        End If


                        REM write form data to metadata object
                        'If Not ofrmlink.ok_selected Then
                        '    REM if cancel selected do nothing
                        '    ocommentary = New bc_cs_activity_log("bc_am_in_link", "new", bc_cs_activity_codes.COMMENTARY, "Cancel Selected on Link form.")
                        '    Exit Sub
                        'End If
                        REM load data for the contribution object based on selected criteria
                        bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve insight", "Retrieving Configuration Data for Workbook", 10, False, True)
                        'bc_am_load_objects.obc_om_insight_contribution_for_entity = New bc_om_insight_contribution_for_entity(ofrmlink.class_id, ofrmlink.class_name, ofrmlink.entity_id, ofrmlink.entity_name, ofrmlink.contributor_id, ofrmlink.contributor_name)
                        bc_am_load_objects.obc_om_insight_contribution_for_entity = New bc_om_insight_contribution_for_entity(cfl.class_id, cfl.class_name, cfl.lead_entity_id, cfl.entity_name, cfl.schema_id, cfl.schema_name)
                    Else
                        bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve insight", "Retrieving Configuration Data for Workbook", 10, False, True)
                        bc_am_load_objects.obc_om_insight_contribution_for_entity = New bc_om_insight_contribution_for_entity(lclassid, lclassname, lent, lentname, lsch, lschname)
                    End If
                    bc_am_load_objects.obc_om_insight_contribution_for_entity.certificate.user_id = bc_cs_central_settings.logged_on_user_id
                    bc_am_load_objects.obc_om_insight_contribution_for_entity.certificate.name = bc_cs_central_settings.logged_on_user_name
                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        Dim gbc_db As New bc_cs_db_services(False)
                        bc_am_load_objects.obc_om_insight_contribution_for_entity.db_read()
                    ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                        bc_am_load_objects.obc_om_insight_contribution_for_entity.tmode = bc_cs_soap_base_class.tREAD
                        bc_am_load_objects.obc_om_insight_contribution_for_entity.transmit_to_server_and_receive(bc_am_load_objects.obc_om_insight_contribution_for_entity, True)
                    End If

                    If bc_am_load_objects.obc_om_insight_contribution_for_entity.no_template = True Then
                        Dim omsg As New bc_cs_message("Blue Curve", "Template Not Assinged for all parent and child entities. Please check all entity templates are assigned in Insight Toolkit.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                        Exit Sub
                    End If
                    REM save the Excel sheet to file system
                    REM build the workbook
                    bc_cs_central_settings.progress_bar.increment("Building Workbook...")
                    If auto_assign = True Then
                        oexcel.build_workbook(lclassid, lent, lsch)
                    Else
                        'oexcel.build_workbook(ofrmlink.class_id, ofrmlink.entity_id, ofrmlink.contributor_id)
                        oexcel.build_workbook(cfl.class_id, cfl.lead_entity_id, cfl.schema_id)
                    End If
                    REM filename is insight_ + master entity_id
                    If bc_am_insight_formats.store_dat_file = 1 Then
                        bc_cs_central_settings.progress_bar.increment("Saving Metadata...")
                        Dim filename As String
                        filename = "insight_" + CStr(bc_am_load_objects.obc_om_insight_contribution_for_entity.lead_entity_id) + "_" + CStr(bc_am_load_objects.obc_om_insight_contribution_for_entity.contributor_id) + ".xml"
                        bc_am_load_objects.obc_om_insight_contribution_for_entity.write_data_to_file(bc_cs_central_settings.local_repos_path + filename)
                    End If

                    REM link
                    If link_now = True Then
                        bc_cs_central_settings.progress_bar.increment("Linking...")
                        oexcel.link_workbook(True)
                    End If
                    oexcel.hide_worksheets()
                    bc_cs_central_settings.progress_bar.unload()
                End If
                If bc_am_insight_formats.controlled_Submission = 1 Then
                    Dim caption = "Blue Curve Insight: " + CStr(bc_am_load_objects.obc_om_insight_contribution_for_entity.lead_entity_name) + " " + bc_am_load_objects.obc_om_insight_contribution_for_entity.contributor_name
                    oexcel.set_caption(caption)
                End If

                omessage = New bc_cs_message("Blue Curve - insight", "Build Successful", bc_cs_message.MESSAGE)

            End If


        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_link", "new", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            If load_failed = False Then
                oexcel.enable_application_alerts()
                oexcel.screen_updating(False)
            End If
            bc_cs_central_settings.progress_bar.unload()
            slog = New bc_cs_activity_log("bc_am_in_link", "new", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
End Class
Public Class bc_am_in_link_only
    Dim ao_type As Integer
    Dim ao_object As Object
    Dim load_failed As Boolean = False
    Dim odoc As bc_ao_in_object
    Dim odocmetadata As New bc_om_document
    Dim obc_objects As New bc_am_load_objects
    REM constructor determines whether to show the
    REM submit form or not the active object and the type
    Public Sub New(ByVal show_form As Boolean, ByVal ao_object As Object, ByVal ao_type As String)
        Dim slog = New bc_cs_activity_log("bc_am_in_link_only", "new", bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim oexcel As bc_ao_in_object = Nothing

        Dim omessage As bc_cs_message

        Try
            If ao_type <> bc_ao_in_object.EXCEL_DOC Then
                omessage = New bc_cs_message("Blue Curve insight", "Application Object Type: " + ao_type + " not supported", bc_cs_message.MESSAGE)
            Else
                REM load system into memory
                bc_cs_central_settings.version = Application.ProductVersion
                REM instantiate correct AO object
                If ao_type = bc_ao_in_object.EXCEL_DOC Then
                    If IsNothing(ao_object) Then
                        omessage = New bc_cs_message("Blue Curve insight", "Workbook must be Open", bc_cs_message.MESSAGE)
                        load_failed = True
                        Exit Sub
                    End If
                    oexcel = New bc_ao_in_excel(ao_object)
                    oexcel.screen_updating(True)
                    oexcel.disable_application_alerts()
                Else
                    omessage = New bc_cs_message("Blue Curve insight", "mime type: " + ao_type + " not supported.", bc_cs_message.MESSAGE)
                    Exit Sub
                End If


                If oexcel.get_sheet_keys = "No Keys Found" Then
                    omessage = New bc_cs_message("Blue Curve insight", "Workbook is not linked to Research Net please build first", bc_cs_message.MESSAGE)
                    REM put re-link call here
                    Exit Sub
                End If
                If oexcel.get_sheet_keys = "Read Only" Then
                    omessage = New bc_cs_message("Blue Curve insight", "Workbook is Read only cant link.", bc_cs_message.MESSAGE)
                    Exit Sub
                End If
                REM extract parameters for relink
                bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve insight", "Retrieving Configuration Data for Workbook from local filestore", 10, False, True)
                bc_am_load_objects.obc_om_insight_contribution_for_entity = New bc_om_insight_contribution_for_entity
                bc_am_load_objects.obc_om_insight_contribution_for_entity = bc_am_load_objects.obc_om_insight_contribution_for_entity.read_data_from_file(bc_cs_central_settings.local_repos_path + oexcel.get_sheet_keys + ".xml")
                oexcel.unprotect_sheet()
                bc_cs_central_settings.progress_bar.increment("Linking...")
                oexcel.link_workbook(True)
                oexcel.protect_sheet()
                oexcel.hide_worksheets()
                bc_cs_central_settings.progress_bar.unload()
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_link_only", "new", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            If load_failed = False Then
                oexcel.enable_application_alerts()
                oexcel.screen_updating(False)
            End If
            bc_cs_central_settings.progress_bar.unload()
            slog = New bc_cs_activity_log("bc_am_in_link_only", "new", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
End Class
Public Class bc_am_in_rebuild
    Dim ao_type As Integer
    Dim ao_object As Object
    Dim odoc As bc_ao_in_object
    Dim odocmetadata As New bc_om_document
    REM constructor determines whether to show the
    REM submit form or not the active object and the type
    Public Sub New(ByVal show_form As Boolean, ByVal ao_object As Object, ByVal ao_type As String, ByVal link_now As Boolean)
        Dim slog = New bc_cs_activity_log("bc_am_in_rebuild", "new", bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim oexcel As bc_ao_in_object = Nothing

        Dim omessage As bc_cs_message
        Dim load_failed As Boolean = False

        Try
            If ao_type <> bc_ao_in_object.EXCEL_DOC Then
                omessage = New bc_cs_message("Blue Curve insight", "Application Object Type: " + ao_type + " not supported", bc_cs_message.MESSAGE)
            Else
                bc_cs_central_settings.version = Application.ProductVersion
                REM instantiate correct AO object
                If ao_type = bc_ao_in_object.EXCEL_DOC Then
                    If IsNothing(ao_object) Then
                        omessage = New bc_cs_message("Blue Curve insight", "Workbook must be Open", bc_cs_message.MESSAGE)
                        load_failed = True
                        Exit Sub
                    End If
                    oexcel = New bc_ao_in_excel(ao_object)
                    oexcel.disable_application_alerts()
                    oexcel.screen_updating(True)
                Else
                    omessage = New bc_cs_message("Blue Curve insight", "mime type: " + ao_type + " not supported.", bc_cs_message.MESSAGE)
                    Exit Sub
                End If
                Dim keys = oexcel.get_sheet_keys
                If keys = "Read Only" Then
                    omessage = New bc_cs_message("Blue Curve insight", "Workbook is read only cant rebuild.", bc_cs_message.MESSAGE)
                    Exit Sub
                End If
                If keys = "No Keys Found" Then
                    omessage = New bc_cs_message("Blue Curve insight", "This workbook has never been linked to Blue Curve please Build first.", bc_cs_message.MESSAGE)
                    Exit Sub
                Else
                    Dim orecreate As New bc_am_recreate_workbook_metadata(keys, oexcel)
                    If orecreate.recreate(False) = False Then
                        omessage = New bc_cs_message("Blue Curve", "Metadata file not found and cant be recreated!", bc_cs_message.MESSAGE)
                        Exit Sub
                    End If
                End If

                If Trim(bc_am_insight_formats.rebuild_warning) <> "" Then
                    Dim omsg As New bc_cs_message("Blue Curve", bc_am_insight_formats.rebuild_warning, bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
                    If omsg.cancel_selected = True Then
                        Exit Sub
                    End If
                End If


                REM reset workbook
                bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve insight", "Resetting Workbook", 10, False, True)
                oexcel.delete_system_sheets()

                REM extract parameters for relink
                bc_cs_central_settings.progress_bar.increment("Retrieving Configuration Data for Workbook")
                If bc_am_insight_formats.store_dat_file = 1 Then
                    bc_am_load_objects.obc_om_insight_contribution_for_entity = New bc_om_insight_contribution_for_entity
                    bc_am_load_objects.obc_om_insight_contribution_for_entity = bc_am_load_objects.obc_om_insight_contribution_for_entity.read_data_from_file(bc_cs_central_settings.local_repos_path + keys + ".xml")
                End If
                REM read in current file
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    Dim gbc_db As New bc_cs_db_services(False)
                    bc_am_load_objects.obc_om_insight_contribution_for_entity.db_read()
                ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                    Dim tmpobj As New bc_om_insight_contribution_for_entity
                    'tmpobj.tmode = tmpobj.tREAD
                    'tmpobj.transmit_to_server_and_receive(tmpobj, True)
                    'bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets.Clear()
                    'For i = 0 To tmpobj.insight_sheets.bc_om_insight_sheets.Count - 1
                    'bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets.Add(tmpobj.insight_sheets.bc_om_insight_sheets(i))
                    'Next
                    bc_am_load_objects.obc_om_insight_contribution_for_entity.tmode = bc_cs_soap_base_class.tREAD
                    bc_am_load_objects.obc_om_insight_contribution_for_entity.transmit_to_server_and_receive(bc_am_load_objects.obc_om_insight_contribution_for_entity, True)
                End If
                If bc_am_load_objects.obc_om_insight_contribution_for_entity.no_template = True Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Template Not Assinged for all parent and child entities. Please check all entity templates are assigned in Insight Toolkit.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub
                End If
                bc_cs_central_settings.progress_bar.increment("Building Workbook...")
                oexcel.build_workbook(bc_am_load_objects.obc_om_insight_contribution_for_entity.lead_class_id, bc_am_load_objects.obc_om_insight_contribution_for_entity.lead_entity_id, bc_am_load_objects.obc_om_insight_contribution_for_entity.contributor_id)
                REM save the Excel sheet to file system

                If bc_am_insight_formats.store_dat_file = 1 Then
                    bc_cs_central_settings.progress_bar.increment("Saving Metadata...")
                    Dim filename As String
                    filename = "insight_" + CStr(bc_am_load_objects.obc_om_insight_contribution_for_entity.lead_entity_id) + "_" + CStr(bc_am_load_objects.obc_om_insight_contribution_for_entity.contributor_id) + ".xml"
                    bc_am_load_objects.obc_om_insight_contribution_for_entity.write_data_to_file(bc_cs_central_settings.local_repos_path + filename)
                End If
                REM build the workbook

                If link_now = True Then
                    oexcel.unprotect_sheet()
                    bc_cs_central_settings.progress_bar.increment("Linking...")
                    oexcel.link_workbook(True)
                    oexcel.protect_sheet()
                End If
                oexcel.hide_worksheets()
                bc_cs_central_settings.progress_bar.unload()
                If bc_am_insight_formats.controlled_Submission = 1 Then
                    Dim caption = "Blue Curve Insight: " + CStr(bc_am_load_objects.obc_om_insight_contribution_for_entity.lead_entity_name) + " " + bc_am_load_objects.obc_om_insight_contribution_for_entity.contributor_name
                    oexcel.set_caption(caption)
                End If

                omessage = New bc_cs_message("Blue Curve - insight", "Rebuild Successful", bc_cs_message.MESSAGE)

            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_rebuild", "new", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            If load_failed = False Then
                oexcel.enable_application_alerts()
                oexcel.screen_updating(False)
            End If
            bc_cs_central_settings.progress_bar.unload()
            slog = New bc_cs_activity_log("bc_am_in_rebuild", "new", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class
Public Class bc_am_recreate_workbook_metadata
    Private keys As String
    Private ao_excel As bc_ao_in_excel

    Public Sub New(ByVal keys As String, ByVal ao_excel As bc_ao_in_excel)
        Me.keys = keys
        Me.ao_excel = ao_excel
    End Sub
    Public Function recreate(Optional ByVal from_db As Boolean = True) As Boolean
        Dim slog As New bc_cs_activity_log("bc_am_recreate_workbook_metadata", "recreate", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            REM check if file exists if so dont do anything
            Dim ocommentary As bc_cs_activity_log

            Dim fn As String
            Dim params() As String
            Dim fs As New bc_cs_file_transfer_services
            recreate = False

            fn = bc_cs_central_settings.local_repos_path + keys + ".xml"
            If fs.check_document_exists(fn) Then
                ocommentary = New bc_cs_activity_log("bc_am_recreate_workbook_metadata", "recreate", bc_cs_activity_codes.COMMENTARY, "Excel Metadata file exists", Nothing)
                Try
                    params = Me.ao_excel.get_sheet_parameters()
                    bc_am_load_objects.obc_om_insight_contribution_for_entity = New bc_om_insight_contribution_for_entity
                    bc_am_load_objects.obc_om_insight_contribution_for_entity = bc_am_load_objects.obc_om_insight_contribution_for_entity.read_data_from_file(fn, Nothing, False)
                    Dim ts As String
                    ts = bc_am_load_objects.obc_om_insight_contribution_for_entity.lead_entity_name
                    recreate = True
                Catch ex As Exception
                    recreate = False
                End Try
                REM if xml if corrupt then also recreate
            End If
            If recreate = False Then
                REM recreate from server
                REM get parameters
                params = Me.ao_excel.get_sheet_parameters()
                bc_am_load_objects.obc_om_insight_contribution_for_entity = New bc_om_insight_contribution_for_entity(params(0), params(1), params(2), params(3), params(4), params(5))
                bc_am_load_objects.obc_om_insight_contribution_for_entity.certificate.user_id = bc_cs_central_settings.logged_on_user_id
                bc_am_load_objects.obc_om_insight_contribution_for_entity.certificate.name = bc_cs_central_settings.logged_on_user_name

                If from_db = True Then
                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        Dim gbc_db As New bc_cs_db_services(False)
                        bc_am_load_objects.obc_om_insight_contribution_for_entity.db_read()
                        recreate = True
                    ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                        bc_am_load_objects.obc_om_insight_contribution_for_entity.tmode = bc_cs_soap_base_class.tREAD
                        bc_am_load_objects.obc_om_insight_contribution_for_entity.transmit_to_server_and_receive(bc_am_load_objects.obc_om_insight_contribution_for_entity, True)
                        recreate = True
                    Else
                        ocommentary = New bc_cs_activity_log("bc_am_recreate_workbook_metadata", "recreate", bc_cs_activity_codes.COMMENTARY, "Excel Metadata file doesnt exist and working locally cant proceed!", Nothing)
                    End If
                Else
                    recreate = True
                End If
            End If
            REM now read me
            If bc_am_insight_formats.store_dat_file = 1 Then
                bc_am_load_objects.obc_om_insight_contribution_for_entity.write_data_to_file(fn)
                bc_am_load_objects.obc_om_insight_contribution_for_entity = bc_am_load_objects.obc_om_insight_contribution_for_entity.read_data_from_file(bc_cs_central_settings.local_repos_path + keys + ".xml")
            End If
            REM if not recreate from server
        Catch ex As Exception
            recreate = False
            Dim db_err As New bc_cs_error_log("bc_am_recreate_workbook_metadata", "recreate", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_recreate_workbook_metadata", "recreate", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
End Class
Public Class bc_am_in_retrieve_data
    Dim ao_type As Integer
    Dim ao_object As Object
    Dim load_failed As Boolean = False
    Dim odoc As bc_ao_in_object
    Dim odocmetadata As New bc_om_document
    Dim obc_objects As New bc_am_load_objects
    REM constructor determines whether to show the
    REM submit form or not the active object and the type
    Public Sub New(ByVal show_form As Boolean, ByVal ao_object As Object, ByVal ao_type As String)
        Dim slog = New bc_cs_activity_log("bc_am_in_retrieve_data", "new", bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim oexcel As bc_ao_in_object = Nothing

        Dim omessage As bc_cs_message
        Dim i As Integer

        Try
            Dim oload As New bc_am_load_objects
            If ao_type <> bc_ao_in_object.EXCEL_DOC Then
                omessage = New bc_cs_message("Blue Curve insight", "Application Object Type: " + ao_type + " not supported", bc_cs_message.MESSAGE)
            Else
                REM load system into memory
                bc_cs_central_settings.version = Application.ProductVersion
                REM instantiate correct AO object
                If ao_type = bc_ao_in_object.EXCEL_DOC Then
                    If IsNothing(ao_object) Then
                        omessage = New bc_cs_message("Blue Curve insight", "Workbook must be Open", bc_cs_message.MESSAGE)
                        load_failed = True
                        Exit Sub
                    End If
                    oexcel = New bc_ao_in_excel(ao_object)
                    oexcel.screen_updating(True)
                    oexcel.disable_application_alerts()
                Else
                    omessage = New bc_cs_message("Blue Curve insight", "mime type: " + ao_type + " not supported.", bc_cs_message.MESSAGE)
                    Exit Sub
                End If
                Dim keys As String
                keys = oexcel.get_sheet_keys
                If keys = "Read Only" Then
                    omessage = New bc_cs_message("Blue Curve insight", "Workbook is read only cant retrieve data.", bc_cs_message.MESSAGE)
                    Exit Sub
                End If

                If keys = "No Keys Found" Then
                    omessage = New bc_cs_message("Blue Curve insight", "This workbook has never been linked to Blue Curve please Build first.", bc_cs_message.MESSAGE)
                    Exit Sub
                Else
                    Dim orecreate As New bc_am_recreate_workbook_metadata(keys, oexcel)
                    If orecreate.recreate = False Then
                        omessage = New bc_cs_message("Blue Curve", "Metadata file not found and cant be recreated!", bc_cs_message.MESSAGE)
                        Exit Sub
                    End If
                End If


                REM create obejct for retreiveing data
                Dim oretrieve_data_values As New bc_om_insight_retrieve_values
                oretrieve_data_values.stage_id = bc_am_load_objects.obc_om_insight_contribution_for_entity.workflow_stage

                If oretrieve_data_values.stage_id = 0 Then
                    oretrieve_data_values.stage_id = 1
                End If

                oretrieve_data_values.contributor_id = bc_am_load_objects.obc_om_insight_contribution_for_entity.contributor_id
                oretrieve_data_values.accounting_id = bc_am_load_objects.obc_om_insight_contribution_for_entity.accounting_standard

                For i = 0 To bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets.Count - 1
                    With bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets(i)
                        oretrieve_data_values.entity_id.Add(.entity_id)
                    End With
                Next

                REM assign entity ids
                oretrieve_data_values.values.Clear()

                Dim oretrieve_data_value As bc_om_insight_retrieve_value

                For i = 0 To bc_am_insight_retrieve_values.retrieve_values.Count - 1
                    oretrieve_data_value = New bc_om_insight_retrieve_value
                    oretrieve_data_value.attribute_code = bc_am_insight_retrieve_values.retrieve_values(i).attribute_code
                    oretrieve_data_value.submission_code = bc_am_insight_retrieve_values.retrieve_values(i).submission_code
                    oretrieve_data_value.sheet = bc_am_insight_retrieve_values.retrieve_values(i).sheet
                    oretrieve_data_value.row = bc_am_insight_retrieve_values.retrieve_values(i).row
                    oretrieve_data_value.col = bc_am_insight_retrieve_values.retrieve_values(i).col
                    oretrieve_data_value.dimension = bc_am_insight_retrieve_values.retrieve_values(i).dimension
                    oretrieve_data_value.contributor_id = bc_am_insight_retrieve_values.retrieve_values(i).contributor_id
                    oretrieve_data_value.scale_factor = bc_am_insight_retrieve_values.retrieve_values(i).scale_factor
                    oretrieve_data_value.order = bc_am_insight_retrieve_values.retrieve_values(i).order
                    oretrieve_data_values.values.Add(oretrieve_data_value)
                Next

                bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve insight", "Retrieving Upload Data", 10, False, True)
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    oretrieve_data_values.db_read()
                ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                    oretrieve_data_values.tmode = bc_cs_soap_base_class.tREAD
                    oretrieve_data_values.transmit_to_server_and_receive(oretrieve_data_values, True)
                Else
                    omessage = New bc_cs_message("Blue Curve - insight", "Netwotk Not Connected Can't Retreive Items", bc_cs_message.MESSAGE)
                    Exit Sub
                End If
                bc_cs_central_settings.progress_bar.increment("Writing Uploaded Values...")
                oexcel.write_upload_values(oretrieve_data_values)
            End If


        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_retrieve_data", "new", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            oexcel.enable_application_alerts()
            oexcel.screen_updating(False)
            bc_cs_central_settings.progress_bar.unload()
            slog = New bc_cs_activity_log("bc_am_in_retrieve_data", "new", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
End Class
Public Class bc_am_insight_parameters
    Dim slog = New bc_cs_activity_log("bc_am_insight_parameters", "new", bc_cs_activity_codes.TRACE_ENTRY, "")
    Dim name As String
    Dim oexcel As bc_ao_in_object
    Dim ocommentary As bc_cs_activity_log
    Dim omessage As bc_cs_message
    Dim xmlload As New Xml.XmlDocument
    Public load_failed As Boolean
    Public Shared bc_am_insight_link_names As bc_am_insight_link_names

    Public Shared binsight_config_loaded As Boolean
    Const INSIGHT_CONFIG_FILE = "bc_am_insight_config.xml"
    Public Sub New(Optional ByVal show_msg As Boolean = True)

        Try
            load_failed = False
            Dim myXmlNodeDBTypeList As Xml.XmlNodeList
            REM read in link codes
            xmlload.Load(bc_cs_central_settings.local_template_path + INSIGHT_CONFIG_FILE)
            bc_am_insight_link_names.year_head = xmlload.SelectSingleNode("/insight_settings/period_headers/year").InnerXml()
            bc_am_insight_link_names.period_head = xmlload.SelectSingleNode("/insight_settings/period_headers/period").InnerXml()
            bc_am_insight_link_names.e_a_head = xmlload.SelectSingleNode("/insight_settings/period_headers/e_a").InnerXml()
            bc_am_insight_link_names.start_column = xmlload.SelectSingleNode("/insight_settings/period_headers/start_column").InnerXml()
            bc_am_insight_link_names.concurrent_blanks = CInt(xmlload.SelectSingleNode("/insight_settings/period_headers/concurrent_blanks").InnerXml())
            bc_am_insight_link_names.actual_prefix = xmlload.SelectSingleNode("/insight_settings/period_headers/actual_prefix").InnerXml()
            bc_am_insight_link_names.estimate_prefix = xmlload.SelectSingleNode("/insight_settings/period_headers/estimate_prefix").InnerXml()
            bc_am_insight_link_names.accounting_head = xmlload.SelectSingleNode("/insight_settings/period_headers/accounting").InnerXml()
            Try
                bc_am_insight_link_names.enable_disable = xmlload.SelectSingleNode("/insight_settings/period_headers/enable_disable").InnerXml()
            Catch

            End Try

            myXmlNodeDBTypeList = xmlload.SelectNodes("descendant::hide_cells")
            If myXmlNodeDBTypeList.Count > 0 Then
                bc_am_insight_link_names.hide_cells = xmlload.SelectSingleNode("/insight_settings/hide_cells").InnerXml()
            Else
                'if there is no configuration information use normal connection type.
                bc_am_insight_link_names.hide_cells = "1"
            End If

            REM read in period codes
            Dim myXMLNodelist As Xml.XmlNodeList
            Dim myxmlnode As Xml.XmlNode
            bc_am_insight_link_names.period_values.Clear()
            myXMLNodelist = xmlload.SelectNodes("/insight_settings/period_headers/period_codes/bc_code")
            For Each myxmlnode In myXMLNodelist
                Dim obc_am_period_values As New bc_am_period_values(myxmlnode.InnerXml)
                bc_am_insight_link_names.period_values.Add(obc_am_period_values)
            Next
            myXMLNodelist = xmlload.SelectNodes("/insight_settings/period_headers/period_codes/ext_code")
            Dim i As Integer = 0
            For Each myxmlnode In myXMLNodelist
                bc_am_insight_link_names.period_values(i).external_code = myxmlnode.InnerXml
                i = i + 1
            Next
            myXMLNodelist = xmlload.SelectNodes("/insight_settings/cell_format")
            If myXMLNodelist.Count > 0 Then
                bc_am_insight_link_names.cell_format = xmlload.SelectSingleNode("/insight_settings/cell_format").InnerXml()
            End If
            bc_am_insight_link_names.accounting_values.Clear()
            myXMLNodelist = xmlload.SelectNodes("/insight_settings/period_headers/accounting_codes/bc_code")
            For Each myxmlnode In myXMLNodelist
                Dim obc_am_accounting_codes As New bc_am_accounting_codes(myxmlnode.InnerXml)
                bc_am_insight_link_names.accounting_values.Add(obc_am_accounting_codes)
            Next
            myXMLNodelist = xmlload.SelectNodes("/insight_settings/period_headers/accounting_codes/ext_code")
            i = 0
            For Each myxmlnode In myXMLNodelist
                bc_am_insight_link_names.accounting_values(i).external_code = myxmlnode.InnerXml
                i = i + 1
            Next
            bc_am_insight_link_names.static_head = xmlload.SelectSingleNode("/insight_settings/static_headers/header").InnerXml()
            bc_am_insight_link_names.label_head = xmlload.SelectSingleNode("/insight_settings/label_headers/header").InnerXml()
            REM read in format codes  

            myXMLNodelist = xmlload.SelectNodes("/insight_settings/show_all_entities")
            If myXMLNodelist.Count > 0 Then
                bc_am_insight_link_names.show_all_entities = xmlload.SelectSingleNode("/insight_settings/show_all_entities").InnerXml()
            End If
            myXMLNodelist = xmlload.SelectNodes("/insight_settings/my_entities_default")
            If myXMLNodelist.Count > 0 Then
                bc_am_insight_link_names.my_entities_default = xmlload.SelectSingleNode("/insight_settings/my_entities_default").InnerXml()
            End If

            Dim oretrieve_value As bc_am_insight_retrieve_value
            bc_am_insight_retrieve_values.retrieve_values.Clear()

            myXMLNodelist = xmlload.SelectNodes("/insight_settings/retrieve_values/retrieve_value/attribute_code")
            i = 0
            For Each myxmlnode In myXMLNodelist
                oretrieve_value = New bc_am_insight_retrieve_value
                oretrieve_value.attribute_code = myxmlnode.InnerXml
                bc_am_insight_retrieve_values.retrieve_values.Add(oretrieve_value)
                i = i + 1
            Next

            myXMLNodelist = xmlload.SelectNodes("/insight_settings/retrieve_values/retrieve_value/sheet")
            i = 0
            For Each myxmlnode In myXMLNodelist
                oretrieve_value = New bc_am_insight_retrieve_value
                bc_am_insight_retrieve_values.retrieve_values(i).sheet = myxmlnode.InnerXml
                i = i + 1
            Next
            myXMLNodelist = xmlload.SelectNodes("/insight_settings/retrieve_values/retrieve_value/row")
            i = 0
            For Each myxmlnode In myXMLNodelist
                oretrieve_value = New bc_am_insight_retrieve_value
                bc_am_insight_retrieve_values.retrieve_values(i).row = myxmlnode.InnerXml
                i = i + 1
            Next
            myXMLNodelist = xmlload.SelectNodes("/insight_settings/retrieve_values/retrieve_value/col")
            i = 0
            For Each myxmlnode In myXMLNodelist
                oretrieve_value = New bc_am_insight_retrieve_value
                bc_am_insight_retrieve_values.retrieve_values(i).col = myxmlnode.InnerXml
                i = i + 1
            Next
            myXMLNodelist = xmlload.SelectNodes("/insight_settings/retrieve_values/retrieve_value/submission_code")
            i = 0
            For Each myxmlnode In myXMLNodelist
                oretrieve_value = New bc_am_insight_retrieve_value
                bc_am_insight_retrieve_values.retrieve_values(i).submission_code = myxmlnode.InnerXml
                i = i + 1
            Next
            myXMLNodelist = xmlload.SelectNodes("/insight_settings/retrieve_values/retrieve_value/dimension")
            i = 0
            For Each myxmlnode In myXMLNodelist
                oretrieve_value = New bc_am_insight_retrieve_value
                bc_am_insight_retrieve_values.retrieve_values(i).dimension = myxmlnode.InnerXml
                i = i + 1
            Next
            myXMLNodelist = xmlload.SelectNodes("/insight_settings/retrieve_values/retrieve_value/scale_factor")
            i = 0
            For Each myxmlnode In myXMLNodelist
                oretrieve_value = New bc_am_insight_retrieve_value
                bc_am_insight_retrieve_values.retrieve_values(i).scale_factor = myxmlnode.InnerXml
                i = i + 1
            Next
            myXMLNodelist = xmlload.SelectNodes("/insight_settings/retrieve_values/retrieve_value/use_entity_id")
            i = 0
            For Each myxmlnode In myXMLNodelist
                oretrieve_value = New bc_am_insight_retrieve_value
                bc_am_insight_retrieve_values.retrieve_values(i).use_entity_id = myxmlnode.InnerXml
                i = i + 1
            Next
            myXMLNodelist = xmlload.SelectNodes("/insight_settings/retrieve_values/retrieve_value/contributor_id")

            i = 0
            For Each myxmlnode In myXMLNodelist
                oretrieve_value = New bc_am_insight_retrieve_value
                bc_am_insight_retrieve_values.retrieve_values(i).contributor_id = myxmlnode.InnerXml
                i = i + 1
            Next
            myXMLNodelist = xmlload.SelectNodes("/insight_settings/retrieve_values/retrieve_value/order")
            i = 0
            For Each myxmlnode In myXMLNodelist
                oretrieve_value = New bc_am_insight_retrieve_value
                bc_am_insight_retrieve_values.retrieve_values(i).order = myxmlnode.InnerXml
                i = i + 1
            Next
            REM Exclude Sheets
            bc_am_insight_link_names.exclude_sheets.Clear()
            myXMLNodelist = xmlload.SelectNodes("/insight_settings/exclude_sheets/sheet")
            For Each myxmlnode In myXMLNodelist
                bc_am_insight_link_names.exclude_sheets.Add(myxmlnode.InnerXml)
            Next
            REM toggle
            myXMLNodelist = xmlload.SelectNodes("/insight_settings/use_include_sheets")
            If myXMLNodelist.Count > 0 Then
                bc_am_insight_link_names.use_include_sheets = xmlload.SelectSingleNode("/insight_settings/use_include_sheets").InnerXml()
            End If
            REM include Sheets
            bc_am_insight_link_names.include_sheets.Clear()
            myXMLNodelist = xmlload.SelectNodes("/insight_settings/include_sheets/sheet")
            For Each myxmlnode In myXMLNodelist
                bc_am_insight_link_names.include_sheets.Add(myxmlnode.InnerXml)
            Next
            REM header errors in cell
            myXMLNodelist = xmlload.SelectNodes("/insight_settings/show_header_errors_in_cell")
            If myXMLNodelist.Count > 0 Then
                bc_am_insight_link_names.show_header_errors_in_cell = xmlload.SelectSingleNode("/insight_settings/show_header_errors_in_cell").InnerXml()
            End If
            REM insight changes
            REM class id
            myXMLNodelist = xmlload.SelectNodes("/insight_settings/insight_changes/class_id")
            If myXMLNodelist.Count > 0 Then
                bc_am_insight_changes_params.class_id = xmlload.SelectSingleNode("/insight_settings/insight_changes/class_id").InnerXml()
            End If
            REM row id
            myXMLNodelist = xmlload.SelectNodes("/insight_settings/insight_changes/row_id")
            If myXMLNodelist.Count > 0 Then
                bc_am_insight_changes_params.row_id = xmlload.SelectSingleNode("/insight_settings/insight_changes/row_id").InnerXml()
            End If
            Dim iformats As New bc_am_insight_formats
            If iformats.load_failed = True Then
                If show_msg = True Then
                    Dim omessage As New bc_cs_message("Blue Curve - insight", "Cannot find or read insight config file: " + bc_cs_central_settings.local_template_path + INSIGHT_CONFIG_FILE + " insight may not function as expected!", bc_cs_message.MESSAGE)
                End If
            End If
        Catch ex As Exception
            Dim ocommentary As New bc_cs_activity_log("bc_am_insight_parameters", "new", bc_cs_activity_codes.COMMENTARY, "Cannot find or read insight config file: " + bc_cs_central_settings.local_template_path + INSIGHT_CONFIG_FILE + ex.Message)
            load_failed = True
            If show_msg = True Then
                Dim omessage As New bc_cs_message("Blue Curve - insight", "Cannot find or read insight config file: " + bc_cs_central_settings.local_template_path + INSIGHT_CONFIG_FILE + " insight may not function as expected!" + ex.Message, bc_cs_message.MESSAGE)
            End If
        Finally
            slog = New bc_cs_activity_log("bc_am_insight_parameters", "new", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
End Class

'* 
'* Singleton to manage global settings read from bc_am_insight_config.xml file
'* Craig Berry 03-11-08
'*
Public Class bc_am_insight_formats
    Public Shared format_filename As String
    Public Shared headerName As String
    Public Shared row_Header_Style As String
    Public Shared row_Style As String
    Public Shared section_style As String
    Public Shared header_Name_Style As String
    Public Shared header_Value_Style As String
    Public Shared flexible_Label_Style As String
    Public Shared hide_Sheets As String
    Public Shared controlled_Submission As String = 0
    Public Shared linked_Header As String
    Public Shared local_Save_Enabled As Integer = 0
    Public Shared local_Save_Dir As String
    Public Shared number_per_batch As Integer
    Public Shared update_lists As Integer
    Public Shared rebuild_warning As String
    Public Shared db_batch_mode As Integer = 0
    Public Shared client_side_validation As Integer = 0
    Public Shared store_dat_file As Integer = 1
    Public load_failed As Boolean = False
    Public Shared excel_rtd_rest_url As String = ""
    Public Shared excel_rtd_thread_sleep As Integer = 10
    Public Shared excel_rtd_enable_real_time_update As Boolean = False
    Public Shared excel_rtd_real_time_update_interval As Integer = 10000
    'Private Shared myInstance As bc_am_insight_formats
    Const INSIGHT_CONFIG_FILE = "bc_am_insight_config.xml"
    Public Sub New()
        Try

            REM read in link codes
            Dim xmlload As New Xml.XmlDocument
            Dim myXMLNodelist As Xml.XmlNodeList

            xmlload.Load(bc_cs_central_settings.local_template_path + INSIGHT_CONFIG_FILE)

            hide_Sheets = xmlload.SelectSingleNode("/insight_settings/hide_sheets").InnerXml()
            controlled_Submission = xmlload.SelectSingleNode("/insight_settings/controlled_submission").InnerXml()
            linked_Header = xmlload.SelectSingleNode("/insight_settings/linked_header").InnerXml()

            Try
                format_filename = xmlload.SelectSingleNode("/insight_settings/format/template").InnerXml()
                section_style = xmlload.SelectSingleNode("/insight_settings/format/section/style").InnerXml()
                flexible_Label_Style = xmlload.SelectSingleNode("/insight_settings/format/flexible_label/style").InnerXml()
                header_Value_Style = xmlload.SelectSingleNode("/insight_settings/format/header_value/style").InnerXml()
                header_Name_Style = xmlload.SelectSingleNode("/insight_settings/format/header_name/style").InnerXml()
                row_Style = xmlload.SelectSingleNode("/insight_settings/format/value/style").InnerXml()
                row_Style = xmlload.SelectSingleNode("/insight_settings/format/row/style").InnerXml()
                row_Header_Style = xmlload.SelectSingleNode("/insight_settings/format/row_header/style").InnerXml()
            Catch

            End Try

            REM local copy
            myXMLNodelist = xmlload.SelectNodes("/insight_settings/local_copy/enabled")
            If myXMLNodelist.Count > 0 Then
                local_Save_Enabled = xmlload.SelectSingleNode("/insight_settings/local_copy/enabled").InnerXml()
            End If
            REM local copy dir
            myXMLNodelist = xmlload.SelectNodes("/insight_settings/local_copy/dir")
            If myXMLNodelist.Count > 0 Then
                local_Save_Dir = xmlload.SelectSingleNode("/insight_settings/local_copy/dir").InnerXml()
            End If
            number_per_batch = 1000
            myXMLNodelist = xmlload.SelectNodes("/insight_settings/excel_functions/number_per_batch")
            If myXMLNodelist.Count > 0 Then
                Try
                    number_per_batch = xmlload.SelectSingleNode("/insight_settings/excel_functions/number_per_batch").InnerXml()
                Catch
                    Dim ocomm As New bc_cs_activity_log("bc_am_insight_formats", "new", bc_cs_activity_codes.COMMENTARY, "number_per_batch not numeric")
                End Try
            End If

            update_lists = 0
            myXMLNodelist = xmlload.SelectNodes("/insight_settings/excel_functions/update_lists")
            If myXMLNodelist.Count > 0 Then
                Try
                    update_lists = xmlload.SelectSingleNode("/insight_settings/excel_functions/update_lists").InnerXml()
                Catch
                    Dim ocomm As New bc_cs_activity_log("bc_am_insight_formats", "new", bc_cs_activity_codes.COMMENTARY, "update_lists not numeric")
                End Try
            End If
            REM FIL 5.5
            db_batch_mode = 0
            myXMLNodelist = xmlload.SelectNodes("/insight_settings/excel_functions/db_batch_mode")
            If myXMLNodelist.Count > 0 Then
                Try
                    db_batch_mode = xmlload.SelectSingleNode("/insight_settings/excel_functions/db_batch_mode").InnerXml()
                Catch
                    Dim ocomm As New bc_cs_activity_log("bc_am_insight_formats", "new", bc_cs_activity_codes.COMMENTARY, "db_batch_mode not numeric")
                End Try
            End If


            rebuild_warning = ""
            myXMLNodelist = xmlload.SelectNodes("/insight_settings/rebuild_warning")
            If myXMLNodelist.Count > 0 Then
                Try
                    rebuild_warning = xmlload.SelectSingleNode("/insight_settings/rebuild_warning").InnerXml()
                Catch ex As Exception
                    Dim ocomm As New bc_cs_activity_log("bc_am_insight_formats", "new", bc_cs_activity_codes.COMMENTARY, "rebuild_warning error: " + ex.Message)
                End Try
            End If

            REM 5.5
            client_side_validation = 0
            myXMLNodelist = xmlload.SelectNodes("/insight_settings/client_side_validation")
            If myXMLNodelist.Count > 0 Then
                Try
                    client_side_validation = xmlload.SelectSingleNode("/insight_settings/client_side_validation").InnerXml()
                Catch ex As Exception
                    Dim ocomm As New bc_cs_activity_log("bc_am_insight_formats", "new", bc_cs_activity_codes.COMMENTARY, "client_side_validation error: " + ex.Message)
                End Try
            End If

            store_dat_file = 1
            myXMLNodelist = xmlload.SelectNodes("/insight_settings/store_dat_file")
            If myXMLNodelist.Count > 0 Then
                Try
                    store_dat_file = xmlload.SelectSingleNode("/insight_settings/store_dat_file").InnerXml()
                Catch ex As Exception
                    Dim ocomm As New bc_cs_activity_log("bc_am_insight_formats", "new", bc_cs_activity_codes.COMMENTARY, "store_dat_file error: " + ex.Message)
                End Try
            End If
            myXMLNodelist = xmlload.SelectNodes("/insight_settings/excel_functions/excel_rtd_rest_url")
            If myXMLNodelist.Count > 0 Then
                Try
                    excel_rtd_rest_url = xmlload.SelectSingleNode("/insight_settings/excel_functions/excel_rtd_rest_url").InnerXml()
                Catch ex As Exception
                    excel_rtd_rest_url = ""
                End Try
            End If

            myXMLNodelist = xmlload.SelectNodes("/insight_settings/excel_functions/excel_rtd_thread_sleep")
            If myXMLNodelist.Count > 0 Then
                Try

                    excel_rtd_thread_sleep = xmlload.SelectSingleNode("/insight_settings/excel_functions/excel_rtd_thread_sleep").InnerXml()

                Catch ex As Exception
                    excel_rtd_thread_sleep = 10
                End Try
            End If

            myXMLNodelist = xmlload.SelectNodes("/insight_settings/excel_functions/excel_rtd_enable_real_time_update")
            If myXMLNodelist.Count > 0 Then
                Try

                    excel_rtd_enable_real_time_update = xmlload.SelectSingleNode("/insight_settings/excel_functions/excel_rtd_enable_real_time_update").InnerXml()

                Catch ex As Exception
                    excel_rtd_enable_real_time_update = False
                End Try
            End If

            myXMLNodelist = xmlload.SelectNodes("/insight_settings/excel_functions/excel_rtd_real_time_update_interval")
            If myXMLNodelist.Count > 0 Then
                Try

                    excel_rtd_real_time_update_interval = xmlload.SelectSingleNode("/insight_settings/excel_functions/excel_rtd_real_time_update_interval").InnerXml()

                Catch ex As Exception
                    excel_rtd_real_time_update_interval = False
                End Try
            End If


        Catch ex As Exception
            load_failed = True
            Dim db_err As New bc_cs_error_log("bc_am_insight_formats", "New", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try


    End Sub

End Class


'Friend Class bc_am_insight_formats
'    Private formatFilename As String
'    Private headerName As String
'    Private rowHeaderStyle As String
'    Private rowStyle As String
'    Private sectionstyle As String
'    Private headerNameStyle As String
'    Private headerValueStyle As String
'    Private flexibleLabelStyle As String
'    Private hideSheets As String
'    Private controlledSubmission As String = 0
'    Private linkedHeader As String
'    Private localSaveEnabled As Integer = 0
'    Private localSaveDir As String
'    Private Shared myInstance As bc_am_insight_formats
'    Const INSIGHT_CONFIG_FILE = "bc_am_insight_config.xml"

'    'Shared access function 
'    Public Shared Function Instance() As bc_am_insight_formats
'        'If myInstance Is Nothing Then
'        myInstance = New bc_am_insight_formats
'        'End If
'        ' return the initialized instance of the Singleton Class
'        Return myInstance
'    End Function

'    'Private constructor that loads member variable with values from config file

'    Private Sub New()
'        Dim slog = New bc_cs_activity_log("bc_am_insight_formats", "New", bc_cs_activity_codes.TRACE_ENTRY, "")

'        Try
'            Dim xmlload As New Xml.XmlDocument
'            Dim myXMLNodelist As Xml.XmlNodeList

'            xmlload.Load(bc_cs_central_settings.local_template_path + INSIGHT_CONFIG_FILE)

'            hideSheets = xmlload.SelectSingleNode("/insight_settings/hide_sheets").InnerXml()
'            controlledSubmission = xmlload.SelectSingleNode("/insight_settings/controlled_submission").InnerXml()
'            linkedHeader = xmlload.SelectSingleNode("/insight_settings/linked_header").InnerXml()

'            Try
'                formatFilename = xmlload.SelectSingleNode("/insight_settings/format/template").InnerXml()
'                sectionstyle = xmlload.SelectSingleNode("/insight_settings/format/section/style").InnerXml()
'                flexibleLabelStyle = xmlload.SelectSingleNode("/insight_settings/format/flexible_label/style").InnerXml()
'                headerValueStyle = xmlload.SelectSingleNode("/insight_settings/format/header_value/style").InnerXml()
'                headerNameStyle = xmlload.SelectSingleNode("/insight_settings/format/header_name/style").InnerXml()
'                rowStyle = xmlload.SelectSingleNode("/insight_settings/format/value/style").InnerXml()
'                rowStyle = xmlload.SelectSingleNode("/insight_settings/format/row/style").InnerXml()
'                rowHeaderStyle = xmlload.SelectSingleNode("/insight_settings/format/row_header/style").InnerXml()
'            Catch

'            End Try


'            REM local copy
'            myXMLNodelist = xmlload.SelectNodes("/insight_settings/local_copy/enabled")
'            If myXMLNodelist.Count > 0 Then
'                localSaveEnabled = xmlload.SelectSingleNode("/insight_settings/local_copy/enabled").InnerXml()
'            End If
'            REM local copy dir
'            myXMLNodelist = xmlload.SelectNodes("/insight_settings/local_copy/dir")
'            If myXMLNodelist.Count > 0 Then
'                localSaveDir = xmlload.SelectSingleNode("/insight_settings/local_copy/dir").InnerXml()
'            End If
'        Catch ex As Exception
'            Dim db_err As New bc_cs_error_log("bc_am_insight_formats", "New", bc_cs_error_codes.USER_DEFINED, ex.Message)
'        End Try


'        slog = New bc_cs_activity_log("bc_am_insight_formats", "New", bc_cs_activity_codes.TRACE_EXIT, "")
'    End Sub

'    Public ReadOnly Property format_filename() As String
'        Get
'            format_filename = formatFilename
'        End Get
'    End Property

'    Public ReadOnly Property header_name() As String
'        Get
'            header_name = headerName
'        End Get
'    End Property

'    Public ReadOnly Property row_header_style() As String
'        Get
'            row_header_style = rowHeaderStyle
'        End Get
'    End Property

'    Public ReadOnly Property row_style() As String
'        Get
'            row_style = rowStyle
'        End Get
'    End Property

'    Public ReadOnly Property section_style() As String
'        Get
'            section_style = sectionstyle
'        End Get
'    End Property

'    Public ReadOnly Property header_name_style() As String
'        Get
'            header_name_style = headerNameStyle
'        End Get
'    End Property

'    Public ReadOnly Property header_value_style() As String
'        Get
'            header_value_style = headerValueStyle
'        End Get
'    End Property

'    Public ReadOnly Property flexible_label_style() As String
'        Get
'            flexible_label_style = flexibleLabelStyle
'        End Get
'    End Property

'    Public ReadOnly Property hide_sheets() As String
'        Get
'            hide_sheets = hideSheets
'        End Get
'    End Property

'    Public ReadOnly Property controlled_submission() As String
'        Get
'            controlled_submission = controlledSubmission
'        End Get
'    End Property

'    Public ReadOnly Property linked_header() As String
'        Get
'            linked_header = linkedHeader
'        End Get
'    End Property

'    Public ReadOnly Property local_save_enabled() As String
'        Get
'            local_save_enabled = localSaveEnabled
'        End Get
'    End Property

'    Public ReadOnly Property local_save_dir() As String
'        Get
'            local_save_dir = localSaveDir
'        End Get
'    End Property
'End Class
Friend Class bc_am_insight_changes_params
    Public Shared class_id As String
    Public Shared row_id As String

    Public Sub New()

    End Sub
End Class

Public Class bc_am_insight_link_names
    Public Shared e_a_head As String
    Public Shared period_head As String
    Public Shared period_values As New ArrayList
    Public Shared year_head As String
    Public Shared static_head As String
    Public Shared label_head As String
    Public Shared accounting_head As String
    Public Shared start_column As Integer
    Public Shared actual_prefix As String
    Public Shared estimate_prefix As String
    Public Shared concurrent_blanks As Integer
    Public Shared exclude_sheets As New ArrayList
    Public Shared include_sheets As New ArrayList
    Public Shared use_include_sheets As Boolean
    Public Shared show_header_errors_in_cell As Boolean
    Public Shared cell_format As String
    Public Shared hide_cells As String
    Public Shared enable_disable As String
    REM accounting standard codes
    Public Shared accounting_values As New ArrayList

    Public Shared show_all_entities As Boolean = True
    Public Shared my_entities_default As Boolean = False


    Public Sub New()

    End Sub
End Class
Friend Class bc_am_insight_retrieve_values
    Public Shared retrieve_values As New ArrayList
    Public Sub New()

    End Sub
End Class
Friend Class bc_am_insight_retrieve_value
    Public attribute_code As String
    Public sheet As String
    Public entity_id As Long
    Public row As String
    Public col As String
    Public submission_code As String
    Public dimension As String
    Public contributor_id As String
    Public use_entity_id As Integer
    Public scale_factor As Double
    Public order As Integer
    Public Sub New()

    End Sub
End Class
Friend Class bc_am_period_values
    Public bc_code As String
    Public external_code As String

    Public Sub New(ByVal bc_code As String)
        Me.bc_code = bc_code
    End Sub

End Class
Friend Class bc_am_accounting_codes
    Public bc_code As String
    Public external_code As String

    Public Sub New(ByVal bc_code As String)
        Me.bc_code = bc_code
    End Sub

End Class
REM ==========================================
REM paul coments Bluecurve Limited 2006
REM Module:       Inisght Custom Items
REM Type:         Application Module
REM Description:  Links Excel worksheet
REM Version:      1
REM Change history
REM ==========================================
Public Class bc_am_in_custom_items
    Dim ao_type As Integer
    Dim ao_object As Object
    Dim odoc As bc_ao_in_object
    Dim odocmetadata As New bc_om_document
    REM constructor determines whether to show the
    REM submit form or not the active object and the type
    Public Sub New(ByVal show_form As Boolean, ByVal ao_object As Object, ByVal ao_type As String)
        Dim slog = New bc_cs_activity_log("bc_am_in_custom_items", "new", bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim oexcel As bc_ao_in_object
        Dim ocommentary As bc_cs_activity_log
        Dim omessage As bc_cs_message
        Dim sheet_name As String
        Dim i As Integer

        Try
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.LOCAL Then
                omessage = New bc_cs_message("Blue Curve insight", "Cannot update custom rows as system is working locally.", bc_cs_message.MESSAGE)
                Exit Sub
            End If

            If ao_type <> bc_ao_in_object.EXCEL_DOC Then
                omessage = New bc_cs_message("Blue Curve insight", "Application Object Type: " + ao_type + " not supported", bc_cs_message.MESSAGE)
            Else

                REM ok selected so attach objects to master metadata object
                oexcel = New bc_ao_in_excel(ao_object)
                sheet_name = oexcel.get_sheet_name()
                If Left(sheet_name, 7) <> "BCSHEET" Then
                    omessage = New bc_cs_message("Blue Curve insight", "Sheet is Not Rnet!", bc_cs_message.MESSAGE)
                    Exit Sub
                End If
                REM get parts from spreadsheet to get filename
                Dim filename As String
                filename = oexcel.get_sheet_keys()
                If filename = "Read Only" Then
                    omessage = New bc_cs_message("Blue Curve insight", "Workbook is read only copy cant Submit.", bc_cs_message.MESSAGE)
                    Exit Sub
                End If

                If filename <> "" Then
                    filename = filename + ".xml"
                Else
                    Exit Sub
                End If
                REM attempt to read metadata into memory
                bc_am_load_objects.obc_om_insight_contribution_for_entity = New bc_om_insight_contribution_for_entity
                ocommentary = New bc_cs_activity_log("bc_am_in_submit", "New", bc_cs_activity_codes.COMMENTARY, "Attempting to Load Metadata for Spreadsheet")
                bc_am_load_objects.obc_om_insight_contribution_for_entity = bc_am_load_objects.obc_om_insight_contribution_for_entity.read_data_from_file(bc_cs_central_settings.local_repos_path + filename)

                For i = 0 To bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets.Count
                    With bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets(i)
                        If .sheet_name = sheet_name Then
                            bc_am_load_objects.obc_om_custom_period_section = .bc_om_period_section_custom
                            bc_am_load_objects.obc_om_custom_static_section = .bc_om_static_section_custom

                            Dim oitemform As New bc_am_in_insert_custom_rows
                            oitemform.ShowDialog()
                            If oitemform.ok_selected = False Then
                                Exit Sub
                            End If
                            .bc_om_period_section_custom = bc_am_load_objects.obc_om_custom_period_section
                            .bc_om_static_section_custom = bc_am_load_objects.obc_om_custom_static_section
                            REM metasave it down
                            bc_am_load_objects.obc_om_insight_contribution_for_entity.write_data_to_file(bc_cs_central_settings.local_repos_path + filename)
                            REM create server metadata for this
                            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                                ocommentary = New bc_cs_activity_log("bc_am_in_submit", "New", bc_cs_activity_codes.COMMENTARY, "Uploading Custom Section Definitions for Sheet via ADO")
                                bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets(i).db_write_custom_sections()
                            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                                ocommentary = New bc_cs_activity_log("bc_am_in_submit", "New", bc_cs_activity_codes.COMMENTARY, "Uploading Custom Section Definitions for Sheet via SOAP")
                                bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets(i).write_xml_via_soap_client_request()
                            End If
                            REM what about local working flag cant update
                            Exit For
                        End If
                    End With
                Next
                REM now rebuild workbook and link
                REM could consider a partial rebuild later
                Dim bc_am_rebuild As New bc_am_in_rebuild(False, ao_object, ao_type, True)
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_custom_items", "new", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_in_custom_items", "new", bc_cs_activity_codes.TRACE_ENTRY, "")
        End Try
    End Sub

End Class
REM configuration tool
Public Class bc_am_insight_configuration
    Private ao_object As Object
    Private ao_type As Object
    Private oexcel As bc_ao_in_excel
    Public Sub New(ByVal ao_object As Object, ByVal ao_type As String)
        Me.ao_object = ao_object
        Me.ao_type = ao_type
    End Sub
    REM uploads entire insight configuration into excel
    Public Function download_all_to_excel(ByVal context_id As Long) As Boolean
        Dim slog As New bc_cs_activity_log("bc_am_insight_configuration", "download_all_to_excel", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim omessage As bc_cs_message
            REM uploads into excel a particular configuration for a template_id and context
            If ao_type <> bc_ao_in_object.EXCEL_DOC Then
                omessage = New bc_cs_message("Blue Curve insight", "Application Object Type: " + ao_type + " not supported", bc_cs_message.MESSAGE)
            Else
                REM load system into memory
                bc_cs_central_settings.version = Application.ProductVersion
                REM instantiate correct AO object
                If ao_type = bc_ao_in_object.EXCEL_DOC Then
                    If IsNothing(ao_object) Then
                        omessage = New bc_cs_message("Blue Curve", "Workbook must be Open", bc_cs_message.MESSAGE)
                        download_all_to_excel = False
                        Exit Function
                    End If
                    oexcel = New bc_ao_in_excel(ao_object)
                    REM get all template ids
                    bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve", "Loading Data...", 1, False, True)
                    bc_cs_central_settings.progress_bar.increment("Evaluating Logical Templates...")

                    Dim asheet As New bc_om_insight_sheet(-99)
                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        asheet.db_read()
                    Else
                        asheet.tmode = bc_cs_soap_base_class.tREAD
                        download_all_to_excel = asheet.transmit_to_server_and_receive(asheet, True)
                    End If
                    oexcel.screen_updating(True)
                    oexcel.disable_application_alerts()
                    oexcel.remove_config_sheets()
                    oexcel.enable_application_alerts()
                    Dim i As Integer
                    Dim osheet As bc_om_insight_sheet
                    For i = 0 To asheet.logical_templates_ids.Count - 1
                        bc_cs_central_settings.progress_bar.change_caption("Loading logical template: " + CStr(i + 1) + " of " + CStr(asheet.logical_templates_names.Count))
                        osheet = New bc_om_insight_sheet(asheet.logical_templates_ids(i))
                        oexcel.status_bar_text("Downloading configuration for: " + asheet.logical_templates_names(i))
                        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                            osheet.db_read()
                        Else
                            osheet.tmode = bc_cs_soap_base_class.tREAD
                            download_all_to_excel = osheet.transmit_to_server_and_receive(osheet, True)
                        End If
                        oexcel.add_config_sheet()
                        oexcel.config_populate_sheet(osheet, oexcel.worksheet_count)

                    Next
                    omessage = New bc_cs_message("Blue Curve", "Download Complete!", bc_cs_message.MESSAGE)
                    oexcel.status_bar_text("")
                End If
            End If


        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_insight_configuration", "download_all_to_excel", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            oexcel.screen_updating(False)
            bc_cs_central_settings.progress_bar.unload()
            slog = New bc_cs_activity_log("bc_am_insight_configuration", "download_all_to_excel", bc_cs_activity_codes.TRACE_ENTRY, "")
        End Try
    End Function
    Public Function download_to_excel(ByVal sheet_id As Integer, ByVal logical_template_id As Long, ByVal context_id As Long) As Boolean
        Dim slog As New bc_cs_activity_log("bc_am_insight_configuration", "download_to_excel", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim omessage As bc_cs_message
            REM uploads into excel a particular configuration for a template_id and context
            If ao_type <> bc_ao_in_object.EXCEL_DOC Then
                omessage = New bc_cs_message("Blue Curve insight", "Application Object Type: " + ao_type + " not supported", bc_cs_message.MESSAGE)
            Else
                REM load system into memory
                bc_cs_central_settings.version = Application.ProductVersion
                REM instantiate correct AO object
                If ao_type = bc_ao_in_object.EXCEL_DOC Then
                    If IsNothing(ao_object) Then
                        omessage = New bc_cs_message("Blue Curve", "Workbook must be Open", bc_cs_message.MESSAGE)
                        download_to_excel = False
                        Exit Function
                    End If
                    oexcel = New bc_ao_in_excel(ao_object)
                    If oexcel.test_for_config_sheet(sheet_id) = False Then
                        omessage = New bc_cs_message("Blue Curve", "Worksheet is not an Insight Config sheet", bc_cs_message.MESSAGE)
                        download_to_excel = False
                        Exit Function
                    End If
                    bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve", "Downloading data...", 1, False, True)
                    oexcel.status_bar_text("Downloading data...")
                    bc_cs_central_settings.progress_bar.increment("Downloading Data...")

                    Dim osheet As New bc_om_insight_sheet(logical_template_id)
                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        osheet.db_read()
                    Else
                        osheet.tmode = bc_cs_soap_base_class.tREAD
                        download_to_excel = osheet.transmit_to_server_and_receive(osheet, True)
                    End If
                    oexcel.status_bar_text("Populating sheet...")
                    bc_cs_central_settings.progress_bar.increment("Populating sheet...")
                    oexcel.config_populate_sheet(osheet, sheet_id)
                    oexcel.status_bar_text("")
                    bc_cs_central_settings.progress_bar.unload()
                    omessage = New bc_cs_message("Blue Curve", "Download Complete!", bc_cs_message.MESSAGE)
                End If
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_insight_configuration", "download_to_excel", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_insight_configuration", "download_to_excel", bc_cs_activity_codes.TRACE_ENTRY, "")
        End Try
    End Function
    Public Function add_new_template(ByVal from_template As Integer) As Boolean
        Dim slog As New bc_cs_activity_log("bc_am_insight_configuration", "add_new_template", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim omessage As bc_cs_message

            If ao_type <> bc_ao_in_object.EXCEL_DOC Then
                omessage = New bc_cs_message("Blue Curve insight", "Application Object Type: " + ao_type + " not supported", bc_cs_message.MESSAGE)
            Else
                REM load system into memory
                bc_cs_central_settings.version = Application.ProductVersion
                REM instantiate correct AO object
                If ao_type = bc_ao_in_object.EXCEL_DOC Then
                    If IsNothing(ao_object) Then
                        omessage = New bc_cs_message("Blue Curve", "Workbook must be Open", bc_cs_message.MESSAGE)
                        add_new_template = False
                        Exit Function
                    End If
                    bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve", "Setting up sheet...", 1, False, True)
                    bc_cs_central_settings.progress_bar.increment("Setting up sheet...")
                    oexcel = New bc_ao_in_excel(ao_object)
                    oexcel.add_config_sheet()
                    bc_cs_central_settings.progress_bar.unload()
                End If
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_insight_configuration", "add_new_template", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_insight_configuration", "add_new_template", bc_cs_activity_codes.TRACE_ENTRY, "")
        End Try
    End Function
    Public Function upload_from_excel(ByVal sheet_id As Integer) As Boolean
        Dim slog As New bc_cs_activity_log("bc_am_insight_configuration", "download_from_excel", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim omessage As bc_cs_message
            REM uploads into excel a particular configuration for a template_id and context
            If ao_type <> bc_ao_in_object.EXCEL_DOC Then
                omessage = New bc_cs_message("Blue Curve insight", "Application Object Type: " + ao_type + " not supported", bc_cs_message.MESSAGE)
            Else
                REM load system into memory
                bc_cs_central_settings.version = Application.ProductVersion
                REM instantiate correct AO object
                If ao_type = bc_ao_in_object.EXCEL_DOC Then
                    If IsNothing(ao_object) Then
                        omessage = New bc_cs_message("Blue Curve", "Workbook must be Open", bc_cs_message.MESSAGE)
                        upload_from_excel = False
                        Exit Function
                    End If
                    oexcel = New bc_ao_in_excel(ao_object)
                    If oexcel.test_for_config_sheet(sheet_id) = False Then
                        omessage = New bc_cs_message("Blue Curve", "Worksheet is not an Insight Config sheet", bc_cs_message.MESSAGE)
                        upload_from_excel = False
                        Exit Function
                    Else
                        bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve", "Reading in data", 1, False, True)
                        bc_cs_central_settings.progress_bar.increment("Reading in data")
                        REM read in data from excel into config object
                        Dim osheetconfig As New bc_om_insight_template_config
                        Dim rcode As Integer
                        rcode = oexcel.read_config_data(sheet_id, osheetconfig)

                        If rcode = 1 Then
                            bc_cs_central_settings.progress_bar.unload()
                            omessage = New bc_cs_message("Blue Curve", "Validation failed please look at cell comments!", bc_cs_message.MESSAGE)
                            Exit Function
                        End If
                        If osheetconfig.logical_template_id = 0 Then
                            REM new template
                            REM check template name has been entered and uniqiue
                            If osheetconfig.logical_template_name = "ENTER NAME HERE..." Then
                                bc_cs_central_settings.progress_bar.unload()
                                omessage = New bc_cs_message("Blue Curve", "Template Name must be entered!", bc_cs_message.MESSAGE)
                                Exit Function
                            End If
                        End If
                        oexcel.status_bar_text("Uploading to system...")
                        bc_cs_central_settings.progress_bar.increment("Uploading data to system")
                        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                            osheetconfig.db_read()
                            upload_from_excel = True
                        Else
                            osheetconfig.tmode = bc_cs_soap_base_class.tREAD
                            upload_from_excel = osheetconfig.transmit_to_server_and_receive(osheetconfig, True)
                        End If
                        REM check for duplicate
                        If osheetconfig.logical_template_id = -1 Then
                            bc_cs_central_settings.progress_bar.unload()
                            omessage = New bc_cs_message("Blue Curve", "Template Name: " + osheetconfig.logical_template_name + " is already in use please change!", bc_cs_message.MESSAGE)
                            Exit Function
                        Else
                            REM set template id
                            oexcel.set_logical_template_id(osheetconfig.logical_template_id)
                        End If

                        bc_cs_central_settings.progress_bar.unload()
                        oexcel.status_bar_text("")
                        omessage = New bc_cs_message("Blue Curve", "Upload Complete!", bc_cs_message.MESSAGE)
                        oexcel.set_config_status("Last Uploaded: " + CStr(Date.Now.Day) + "-" + CStr(Date.Now.Month) + "-" + CStr(Date.Now.Year) + " " + CStr(Date.Now.Hour) + ":" + CStr(Date.Now.Minute) + ":" + CStr(Date.Now.Second), sheet_id)
                    End If
                End If
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_insight_configuration", " upload_from_excel", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_insight_configuration", " upload_from_excel", bc_cs_activity_codes.TRACE_ENTRY, "")
        End Try
    End Function
    Public Function delete_logical_template(ByVal sheet_id As Integer) As Boolean
        Dim slog As New bc_cs_activity_log("bc_am_insight_configuration", "delete_logical_template", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim omessage As bc_cs_message
            If ao_type <> bc_ao_in_object.EXCEL_DOC Then
                omessage = New bc_cs_message("Blue Curve insight", "Application Object Type: " + ao_type + " not supported", bc_cs_message.MESSAGE)
            Else
                REM load system into memory
                bc_cs_central_settings.version = Application.ProductVersion
                REM instantiate correct AO object
                If ao_type = bc_ao_in_object.EXCEL_DOC Then
                    If IsNothing(ao_object) Then
                        omessage = New bc_cs_message("Blue Curve", "Workbook must be Open", bc_cs_message.MESSAGE)
                        delete_logical_template = False
                        Exit Function
                    End If
                    oexcel = New bc_ao_in_excel(ao_object)
                    If oexcel.test_for_config_sheet(sheet_id) = False Then
                        omessage = New bc_cs_message("Blue Curve", "Worksheet is not an Insight Config sheet", bc_cs_message.MESSAGE)
                        delete_logical_template = False
                        Exit Function
                    Else
                        REM read in data from excel into config object
                        Dim osheetconfig As New bc_om_insight_template_config
                        osheetconfig.logical_template_id = oexcel.get_logical_template_id(sheet_id)
                        osheetconfig.logical_template_name = "DELETE"
                        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                            osheetconfig.db_read()
                            delete_logical_template = True
                        Else
                            osheetconfig.tmode = bc_cs_soap_base_class.tREAD
                            delete_logical_template = osheetconfig.transmit_to_server_and_receive(osheetconfig, True)
                        End If
                        oexcel.disable_application_alerts()
                        oexcel.delete_sheet(sheet_id)
                        oexcel.enable_application_alerts()
                        omessage = New bc_cs_message("Blue Curve", "Logical Template Deleted.", bc_cs_message.MESSAGE)
                    End If
                End If
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_insight_configuration", "delete_logical_template", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_insight_configuration", "delete_logical_template", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
End Class
Public Class bc_am_in_changes
    Public Sub New()
        Dim obc_load As New bc_am_load("Insight")
        Dim oset As New bc_am_insight_parameters(True)
        Dim ofrm As New bc_am_insight_changes
        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.LOCAL Then
            Dim omessage As New bc_cs_message("Blue Curve", "System is working locally cant change comments", bc_cs_message.MESSAGE)
        Else
            ofrm.ShowDialog()
        End If
    End Sub
    Public Shared Function retrieve_changes(ByVal row_id As Long, ByVal class_id As Long, ByVal entity_id As Long, ByVal stage As String, ByVal date_from As Date) As Object
        Dim slog As New bc_cs_activity_log("bc_am_in_changes", "retrieve_changes", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim sql As String
            sql = "exec dbo.bcc_core_in_changes " + CStr(row_id) + "," + CStr(entity_id) + ",'" + CStr(stage) + "','" + CStr(date_from) + "'"
            Dim ocommentary As New bc_cs_activity_log("bc_am_in_changes", "retrieve_changes", bc_cs_activity_codes.COMMENTARY, sql, Nothing)
            Dim osql As New bc_om_sql(sql)
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                osql.db_read()
            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                osql.tmode = bc_cs_soap_base_class.tREAD
                osql.transmit_to_server_and_receive(osql, True)
            Else
                retrieve_changes = Nothing
                Exit Function
            End If
            If osql.success = True Then
                retrieve_changes = osql.results
            Else
                retrieve_changes = Nothing
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_changes", "retrieve_changes", bc_cs_error_codes.USER_DEFINED, ex.Message)
            retrieve_changes = Nothing
        Finally
            slog = New bc_cs_activity_log("bc_am_in_changes", "retrieve_changes", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function
    Public Shared Sub update_comment(ByVal row_id As Long, ByVal class_id As Long, ByVal entity_id As Long, ByVal stage As String, ByVal date_from As Date, ByVal comment As String)
        Dim slog As New bc_cs_activity_log("bc_am_in_changes", "update_comment", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim sql As String
            Dim fs As New bc_cs_string_services(comment)
            comment = fs.delimit_apostrophies
            sql = "exec dbo.bcc_core_in_update_comment  " + CStr(row_id) + "," + CStr(entity_id) + ",'" + CStr(stage) + "','" + CStr(date_from) + "','" + comment + "'"
            Dim ocommentary As New bc_cs_activity_log("bc_am_in_changes", "retrieve_changes", bc_cs_activity_codes.COMMENTARY, sql, Nothing)
            Dim osql As New bc_om_sql(sql)
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                osql.db_read()
            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                osql.tmode = bc_cs_soap_base_class.tREAD
                osql.transmit_to_server_and_receive(osql, True)
            Else
                Exit Sub
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_changes", "update_comment", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_in_changes", "update_comment", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
End Class
Public Class bc_am_in_insert_intermediate_sheets
    Dim ao_type As Integer
    Dim ao_object As Object
    Dim load_failed As Boolean = False
    Dim odoc As bc_ao_in_object
    Dim odocmetadata As New bc_om_document
    Dim obc_objects As New bc_am_load_objects
    REM constructor determines whether to show the
    REM submit form or not the active object and the type
    Public Sub New(ByVal show_form As Boolean, ByVal ao_object As Object, ByVal ao_type As String)
        Dim slog = New bc_cs_activity_log("bc_am_in_insert_intermediate_sheets", "new", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim oexcel As bc_ao_in_object = Nothing
        Dim ocommentary As bc_cs_activity_log
        Dim omessage As bc_cs_message

        Try
            If ao_type <> bc_ao_in_object.EXCEL_DOC Then
                omessage = New bc_cs_message("Blue Curve insight", "Application Object Type: " + ao_type + " not supported", bc_cs_message.MESSAGE)
            Else
                REM load system into memory
                bc_cs_central_settings.version = Application.ProductVersion
                REM instantiate correct AO object
                If ao_type = bc_ao_in_object.EXCEL_DOC Then
                    If IsNothing(ao_object) Then
                        omessage = New bc_cs_message("Blue Curve insight", "Workbook must be Open", bc_cs_message.MESSAGE)
                        load_failed = True
                        Exit Sub
                    End If
                    oexcel = New bc_ao_in_excel(ao_object)
                    oexcel.screen_updating(True)
                    oexcel.disable_application_alerts()
                Else
                    omessage = New bc_cs_message("Blue Curve insight", "mime type: " + ao_type + " not supported.", bc_cs_message.MESSAGE)
                    Exit Sub
                End If
                If oexcel.get_sheet_keys = "Read Only" Then
                    omessage = New bc_cs_message("Blue Curve insight", "Workbook is read only copy cant build.", bc_cs_message.MESSAGE)
                    Exit Sub
                End If

                REM see if workbook already has format sheets
                Dim sexists As String = ""
                sexists = oexcel.get_property("rnet_entity_id")
                If sexists <> "" Then
                    omessage = New bc_cs_message("Blue Curve insight", "Intermediate sheets already exists in workbook. If you wish to reinsert  please select yes and manually delete the exisiting sheets then try operation again!", bc_cs_message.MESSAGE, True, False, "Yes", "No")
                    If omessage.cancel_selected = False Then
                        oexcel.set_property("rnet_entity_id", "")
                    End If
                    Exit Sub
                End If
                bc_am_load_objects.obc_om_insight_submission_entity_links.bc_om_insight_submission_entity_links.Clear()
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    bc_am_load_objects.obc_om_insight_submission_entity_links.db_read()
                Else
                    bc_am_load_objects.obc_om_insight_submission_entity_links.tmode = bc_cs_soap_base_class.tREAD
                    bc_am_load_objects.obc_om_insight_submission_entity_links.transmit_to_server_and_receive(bc_am_load_objects.obc_om_insight_submission_entity_links, True)
                End If

                If show_form = True Then
                    'Dim ofrmlink As New bc_am_in_main_frm
                    REM read meta data object to form
                    'ofrmlink.ShowDialog()


                    Dim ffl As New bc_dx_insight_build
                    Dim cfl As New Cbc_dx_insight_build
                    If cfl.load_data(ffl, "Insight - Insert Sheets", "Insert") = True Then
                        ffl.ShowDialog()
                        If cfl.lead_entity_id = 0 Then
                            ocommentary = New bc_cs_activity_log("bc_am_in_link", "new", bc_cs_activity_codes.COMMENTARY, "Cancel Selected on insert intermediate sheet form.")
                            Exit Sub
                        End If
                    Else
                        Exit Sub
                    End If
                    REM write form data to metadata object
                    'If Not ofrmlink.ok_selected Then
                    '    REM if cancel selected do nothing
                    '    ocommentary = New bc_cs_activity_log("bc_am_in_link", "new", bc_cs_activity_codes.COMMENTARY, "Cancel Selected on insert intermediate sheet form.")
                    '    Exit Sub
                    'End If
                    REM load data for the contribution object based on selected criteria
                    bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve insight", "Retrieving Intermediate Sheets", 10, False, True)
                    Dim oi_sheet As New bc_om_intermediate_sheets
                    'oi_sheet.lead_entity_id = ofrmlink.entity_id
                    'oi_sheet.schema_id = ofrmlink.contributor_id
                    oi_sheet.lead_entity_id = cfl.lead_entity_id
                    oi_sheet.schema_id = cfl.schema_id



                    oi_sheet.certificate.user_id = bc_cs_central_settings.logged_on_user_id
                    oi_sheet.certificate.name = bc_cs_central_settings.logged_on_user_name

                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        oi_sheet.db_read()
                    ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                        oi_sheet.tmode = bc_cs_soap_base_class.tREAD
                        If oi_sheet.transmit_to_server_and_receive(oi_sheet, False) = False Then
                            omessage = New bc_cs_message("Blue Curve", "Intermediate sheets cannot be inserted general Server Error.", bc_cs_message.MESSAGE)
                            Exit Sub
                        End If
                    End If
                    If oi_sheet.err_str <> "" Then
                        omessage = New bc_cs_message("Blue Curve", "Intermediate sheets cannot be inserted " + CStr(oi_sheet.err_str), bc_cs_message.MESSAGE)
                        Exit Sub
                    Else
                        If oi_sheet.insert_sheets.Count = 0 Then
                            omessage = New bc_cs_message("Blue Curve", "No Intermediate sheets configured for template.", bc_cs_message.MESSAGE)
                            Exit Sub
                        End If
                    End If
                    REM save the Excel sheet to file system
                    REM build the workbook
                    bc_cs_central_settings.progress_bar.increment("inserting intermediate sheets...")
                    Dim i As Integer
                    Dim fs As New bc_cs_file_transfer_services
                    oexcel.disable_application_alerts()

                    For i = 0 To oi_sheet.insert_sheets.Count - 1
                        fs.write_bytestream_to_document(bc_cs_central_settings.local_template_path + oi_sheet.insert_sheets(i).file_name, oi_sheet.insert_sheets(i).sheet, Nothing)
                        REM first check if sheet exists
                        If oexcel.check_sheet_exists(oi_sheet.insert_sheets(i).name) = True Then
                            omessage = New bc_cs_message("Blue Curve", "Format Sheet: " + oi_sheet.insert_sheets(i).name + " already exists in workbook would you like to replace?", bc_cs_message.MESSAGE, True, False, "Yes", "No")
                            If omessage.cancel_selected = False Then
                                oexcel.delete_int_sheet(oi_sheet.insert_sheets(i).name)
                                If oexcel.insert_sheet(oi_sheet.insert_sheets(i).name, bc_cs_central_settings.local_template_path + oi_sheet.insert_sheets(i).file_name) = False Then
                                    omessage = New bc_cs_message("Blue Curve", "Failed to insert intermediate sheet: " + oi_sheet.insert_sheets(i).name, bc_cs_message.MESSAGE)
                                    Exit Sub
                                End If
                            End If
                        Else
                            If oexcel.insert_sheet(oi_sheet.insert_sheets(i).name, bc_cs_central_settings.local_template_path + oi_sheet.insert_sheets(i).file_name) = False Then
                                omessage = New bc_cs_message("Blue Curve", "Failed to insert intermediate sheet: " + oi_sheet.insert_sheets(i).name, bc_cs_message.MESSAGE)
                                Exit Sub
                            End If
                        End If
                    Next
                    REM set properties
                    oexcel.set_property("rnet_entity_id", oi_sheet.lead_entity_id)
                    oexcel.set_property("rnet_schema_id", oi_sheet.schema_id)
                    'oexcel.set_property("rnet_class_id", ofrmlink.class_id)
                    'oexcel.set_property("rnet_entity_name", ofrmlink.entity_name)
                    'oexcel.set_property("rnet_class_name", ofrmlink.class_name)
                    'oexcel.set_property("rnet_schema_name", ofrmlink.contributor_name)
                    oexcel.set_property("rnet_class_id", cfl.class_id)
                    oexcel.set_property("rnet_entity_name", cfl.entity_name)
                    oexcel.set_property("rnet_class_name", cfl.class_name)
                    oexcel.set_property("rnet_schema_name", cfl.schema_name)
                End If
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_insert_intermediate_sheets", "new", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            bc_cs_central_settings.progress_bar.unload()
            If load_failed = False Then
                oexcel.enable_application_alerts()
                oexcel.screen_updating(False)
            End If
            bc_cs_central_settings.progress_bar.unload()
            slog = New bc_cs_activity_log("bc_am_in_insert_intermediate_sheets", "new", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
End Class

