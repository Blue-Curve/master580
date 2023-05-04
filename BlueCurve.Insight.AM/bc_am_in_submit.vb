Imports BlueCurve.Core.OM
Imports BlueCurve.Core.CS
Imports BlueCurve.Core.AS
Imports System.Windows.Forms
Imports System.Threading
Imports System.Diagnostics
Imports System.Collections
Imports System.Runtime.InteropServices
Imports System.Text
Imports BlueCurve.Create.AM
Imports System.IO
REM ==========================================
REM Blue Curve Limited 2005
REM Module:       Insight Link
REM Type:         Application Module
REM Description:  Links Excel worksheet
REM Version:      1
REM Change history
REM ==========================================
REM constructor determines whether to show the
REM submit form or not

REM AUTO AUG 2013
Public Class bc_am_in_submit_model
    Public Sub New()

    End Sub
    Public Sub submit_model(ByVal show_form As Boolean, ByVal ao_object As Object, ByVal ao_type As String, ByVal pub_type_id As Long, ByVal title As String)
        Dim slog = New bc_cs_activity_log("bc_am_in_submit_model", "new", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim oexcel As bc_ao_in_excel = Nothing
            Dim dfilename As String
             Dim omessage As New bc_cs_message
            Dim excelexstension As String

            If bc_cs_central_settings.userOfficeStatus = 2 Then
                excelexstension = ".xlsx"
            Else
                excelexstension = ".xls"
            End If

            If ao_type <> bc_ao_in_object.EXCEL_DOC Then
                omessage = New bc_cs_message("Blue Curve Create", "Application Object Type: " + ao_type + " not supported", bc_cs_message.MESSAGE)
            Else
                REM instantiate correct AO object
                If ao_type = bc_ao_in_object.EXCEL_DOC Then
                    If IsNothing(ao_object) Then
                        omessage = New bc_cs_message("Blue Curve insight", "Workbook must be Open", bc_cs_message.MESSAGE)
                        Exit Sub
                    End If
                    oexcel = New bc_ao_in_excel(ao_object)
                    oexcel.disable_application_alerts()
                    bc_am_load_objects.obc_in_ao_object = oexcel
                End If
                REM AUTO SEP 2013
                REM maybe a form later
                REM load data for the contribution object based on selected criteria
                REM get parts from spreadsheet to get filename
                Dim filename As String
                Dim keys As String

                keys = oexcel.get_sheet_keys()

                If keys <> "No Keys Found" Then
                    If keys = "Read Only" Then
                        omessage = New bc_cs_message("Blue Curve insight", "Workbook is read only copy cannot Upload to system.", bc_cs_message.MESSAGE)
                        Exit Sub
                    End If
                    dfilename = keys + excelexstension
                    filename = keys + ".xml"
                Else
                    omessage = New bc_cs_message("Blue Curve insight", "This workbook has never been linked to Blue Curve please Build first.", bc_cs_message.MESSAGE)
                    Exit Sub
                End If
                If keys = "No Keys Found" Then
                    omessage = New bc_cs_message("Blue Curve insight", "This workbook has never been linked to Blue Curve please Build first.", bc_cs_message.MESSAGE)
                    Exit Sub
                Else
                    Dim orecreate As New bc_am_recreate_workbook_metadata(keys, oexcel)
                    If orecreate.recreate = False Then
                        omessage = New bc_cs_message("Blue Curve", "Metadata file not found and cant be recreated Upload to system failed!", bc_cs_message.MESSAGE)
                        Exit Sub
                    End If
                End If

                REM check workbook is first saved
                Dim ext As String
                Dim fn As String
                fn = oexcel.filename

                ext = Right(fn, Len(fn) - InStrRev(fn, "."))

                If ext <> "xls" And ext <> "xlsx" And ext <> "xlsm" Then
                    omessage = New bc_cs_message("Blue Curve", "Please save the workbook first!", bc_cs_message.MESSAGE)
                    Exit Sub
                End If

                oexcel.hide_worksheets()

                Dim osubmit As New bc_am_import_insight_model

                If osubmit.show_dialogue(bc_am_load_objects.obc_om_insight_contribution_for_entity.lead_entity_id, pub_type_id, fn, title, ext) = False Then
                    Exit Sub
                End If

                REM MAR JULY 2016
                REM put data in excel.
                REM register document to get system doc_Id
                Dim ocommentary As New bc_cs_activity_log("bc_am_at_submit", "new", bc_cs_activity_codes.COMMENTARY, "Registring Document as actions required.")
                osubmit.onewdoc.bimport_support_only = False
                osubmit.onewdoc.register_only = True
                osubmit.onewdoc.byteDoc = Nothing
                Dim no_register As Boolean = False
                bc_cs_central_settings.progress_bar.increment("Registering Document ...")
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    ocommentary = New bc_cs_activity_log("bc_am_at_submit", "new", bc_cs_activity_codes.COMMENTARY, "Registering Document via ADO.")
                    osubmit.onewdoc.db_write(Nothing)


                ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then

                    ocommentary = New bc_cs_activity_log("bc_am_at_submit", "new", bc_cs_activity_codes.COMMENTARY, "Registering Document via SOAP.")
                    osubmit.onewdoc.tmode = bc_cs_soap_base_class.tWRITE
                    If osubmit.onewdoc.transmit_to_server_and_receive(osubmit.onewdoc, True) = False Then
                        no_register = True
                    End If
                End If
                If osubmit.onewdoc.doc_write_error_text <> "" Or no_register = True Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Document registration failed please try again ir contact system administation", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Dim ocomm As New bc_cs_activity_log("bc_am_at_submit", "New", bc_cs_activity_codes.COMMENTARY, "Document Registration Failed: " + osubmit.onewdoc.doc_write_error_text)
                    Exit Sub
                End If

                osubmit.onewdoc.register_only = False

                REM getdata and put it in excel
                Dim osql As New bc_om_sql("exec dbo.bcc_core_get_data_for_excel " + CStr(osubmit.onewdoc.id))

                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                    osql.tmode = bc_cs_soap_base_class.tREAD
                    osql.transmit_to_server_and_receive(osql, True)
                ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    osql.db_read()
                End If
                If osql.success = False Then
                    ocommentary = New bc_cs_activity_log("bc_am_publish_document", "run_archive", bc_cs_activity_codes.COMMENTARY, "SQL Error: exec dbo.bcc_core_wf_publish_document " + CStr(osubmit.onewdoc.id))
                    Exit Sub
                End If
                REM insert into excel

                If oexcel.insert_data_into_workbook(osql.results) = False Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Failed to write data into Excel. Please check relevant worksheets exist.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub

                End If

                REM end MAR

                REM now save and close the workbook
                If oexcel.save(fn, False) = False Then
                    omessage = New bc_cs_message("Blue Curve", "Failed to save workbook cannot submit model!", bc_cs_message.MESSAGE)
                    Exit Sub
                End If

                oexcel.close()

                If osubmit.upload_model() = False Then
                    omessage = New bc_cs_message("Blue Curve", "Failed to upload model to server!", bc_cs_message.MESSAGE)
                    Exit Sub
                End If

            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_in_submit_model", "new", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_in_submit_model", "new", bc_cs_activity_codes.TRACE_EXIT, "")

        End Try

    End Sub
End Class

REM FIL JULY 2012
Public Class bc_am_in_submit
    Public close_flag As Boolean = False
    Dim t As Thread
    Dim load_failed = False
    Public Sub New(ByVal show_form As Boolean, ByVal ao_object As Object, ByVal ao_type As String, ByVal validate_only As Boolean)
        Dim slog = New bc_cs_activity_log("bc_am_in_submit", "new", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim warnings As Boolean = False
        Dim oexcel As bc_ao_in_object = Nothing
        Dim dfilename As String
        Dim ocommentary As bc_cs_activity_log
        Dim omessage As New bc_cs_message
        'Dim frminsightsubmit As New bc_am_in_main_frm
        Dim olocal_workbooks As New bc_om_insight_contribution_for_entities
        Dim checked_out_user As String
        Dim i As Integer
        Dim fs As New bc_cs_file_transfer_services
        Dim server_success As Boolean
        Dim warningstx As New ArrayList
        Dim warningstype As New ArrayList
        Dim warningsrow As New ArrayList
        Dim warningscol As New ArrayList
        Dim warningssheet As New ArrayList
        Dim warningsaddress As New ArrayList
        Dim excelexstension As String

        server_success = False

        Try

            REM SW cope with office versions
            If bc_cs_central_settings.userOfficeStatus = 2 Then
                excelexstension = ".xlsx"
            Else
                excelexstension = ".xls"
            End If

            If ao_type <> bc_ao_in_object.EXCEL_DOC Then
                omessage = New bc_cs_message("Blue Curve Create", "Application Object Type: " + ao_type + " not supported", bc_cs_message.MESSAGE)
            Else
                REM instantiate correct AO object
                If ao_type = bc_ao_in_object.EXCEL_DOC Then
                    If IsNothing(ao_object) Then
                        load_failed = True
                        omessage = New bc_cs_message("Blue Curve insight", "Workbook must be Open", bc_cs_message.MESSAGE)
                        Exit Sub
                    End If
                    oexcel = New bc_ao_in_excel(ao_object)
                    oexcel.disable_application_alerts()
                    bc_am_load_objects.obc_in_ao_object = oexcel
                End If
                oexcel.show_worksheets()
                REM maybe a form later
                REM load data for the contribution object based on selected criteria
                REM get parts from spreadsheet to get filename
                Dim filename As String
                Dim keys As String
                keys = oexcel.get_sheet_keys()
                If keys <> "No Keys Found" Then
                    If keys = "Read Only" Then
                        omessage = New bc_cs_message("Blue Curve insight", "Workbook is read only copy cannot Validate/Submit.", bc_cs_message.MESSAGE)
                        Exit Sub
                    End If
                    dfilename = keys + excelexstension
                    filename = keys + ".xml"
                Else
                    omessage = New bc_cs_message("Blue Curve insight", "This workbook has never been linked to Blue Curve please Build first.", bc_cs_message.MESSAGE)
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

                With bc_am_load_objects.obc_om_insight_contribution_for_entity
                    If .workbook_name = "" Then
                        .workbook_name = .contributor_name + "(" + .lead_class_name + "): " + .lead_entity_name
                    End If
                    'If .author_name = "" Then
                    For i = 0 To bc_am_load_objects.obc_users.user.Count - 1
                        With bc_am_load_objects.obc_users.user(i)
                            If UCase(Trim(.os_user_name)) = UCase(Trim(bc_cs_central_settings.logged_on_user_name)) Then
                                bc_am_load_objects.obc_om_insight_contribution_for_entity.author_name = .first_name + " " + .surname
                            End If
                        End With
                    Next
                    'End If
                End With
                bc_am_load_objects.obc_om_insight_contribution_for_entity.workflow_stage = 1
                REM firstly set default value for role if exists
                If bc_am_load_objects.obc_prefs.financial_workflow_stages.Count > 0 Then
                    If bc_am_load_objects.obc_prefs.default_financial_workflow_stage = 1 Or bc_am_load_objects.obc_prefs.default_financial_workflow_stage = 8 Then
                        ocommentary = New bc_cs_activity_log("bc_am_in_submit", "New", bc_cs_activity_codes.COMMENTARY, "Setting default financial workflow Stage via role to :" + CStr(bc_am_load_objects.obc_prefs.default_financial_workflow_stage))
                        bc_am_load_objects.obc_om_insight_contribution_for_entity.workflow_stage = bc_am_load_objects.obc_prefs.default_financial_workflow_stage
                    End If
                End If
                REM FIL JULY 2012 security fix
                clear_cell_values()
                Dim osec As bc_om_in_submission_security = Nothing
                If validate_only = False And bc_cs_central_settings.selected_conn_method <> bc_cs_central_settings.LOCAL Then

                    osec = New bc_om_in_submission_security
                    osec.schema_id = bc_am_load_objects.obc_om_insight_contribution_for_entity.contributor_id
                    osec.class_id = bc_am_load_objects.obc_om_insight_contribution_for_entity.lead_class_id
                    osec.entity_id = bc_am_load_objects.obc_om_insight_contribution_for_entity.lead_entity_id
                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        osec.db_read()
                    Else
                        osec.tmode = bc_cs_soap_base_class.tREAD
                        If osec.transmit_to_server_and_receive(osec, True) = False Then
                            Exit Sub
                        End If
                    End If
                    If osec.approval_type = 0 Then
                        omessage = New bc_cs_message("Blue Curve", "You do not have security rights to submit this model." + vbCrLf + "Please contact your Administrator to grant submission rights.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                        Exit Sub
                    End If
                End If

                bc_am_load_objects.obc_om_insight_contribution_for_entity.surrogate_author_id = bc_cs_central_settings.logged_on_user_id

                If show_form = True Then
                    REM 5.8 comment in when ready
                    Dim fis As New bc_dx_insight_submit
                    Dim cis As New Cbc_dx_insight_submit
                    validate_only = False
                    If cis.load_data(fis, osec) = True Then
                        fis.ShowDialog()
                    Else
                        Exit Sub
                    End If

                    If cis.submit = False AndAlso cis.validate_only = False Then
                        Exit Sub
                    End If
                    If cis.validate_only = True Then
                        validate_only = True
                    End If


                    REM 

                    'frminsightsubmit.slitanal.Visible = False
                    'frminsightsubmit = New bc_am_in_main_frm

                    'frminsightsubmit.stxtanal.Text = bc_am_load_objects.obc_om_insight_contribution_for_entity.author_name
                    'bc_am_load_objects.obc_om_insight_contribution_for_entity.author_id = bc_cs_central_settings.logged_on_user_id
                    'frminsightsubmit.Btnnext.Enabled = True
                    'If bc_cs_central_settings.selected_conn_method <> bc_cs_central_settings.LOCAL Then
                    '    If osec.approval_type = 2 Then
                    '        If osec.proxy_user_ids.Count > 1 Then
                    '            frminsightsubmit.sanalyst.Text = "Choose Analyst"
                    '            frminsightsubmit.stxtanal.Visible = False
                    '            frminsightsubmit.slitanal.Visible = True
                    '            For i = 0 To osec.proxy_user_names.Count - 1
                    '                frminsightsubmit.slitanal.Items.Add(osec.proxy_user_names(i))
                    '            Next
                    '            frminsightsubmit.Btnnext.Enabled = False

                    '        Else
                    '            frminsightsubmit.slitanal.Visible = False
                    '            frminsightsubmit.stxtanal.Visible = True
                    '            frminsightsubmit.stxtanal.Text = osec.proxy_user_names(0)
                    '            bc_am_load_objects.obc_om_insight_contribution_for_entity.author_id = osec.proxy_user_ids(0)
                    '            bc_am_load_objects.obc_om_insight_contribution_for_entity.author_name = osec.proxy_user_names(0)
                    '        End If
                    '    End If
                    'End If
                    'frminsightsubmit.CheckBox3.Visible = True
                    'frminsightsubmit.CheckBox4.Visible = True
                    'frminsightsubmit.CheckBox5.Visible = True
                    'frminsightsubmit.CheckBox6.Visible = True
                    'frminsightsubmit.submit_mode = True
                    'frminsightsubmit.stxtname.Text = bc_am_load_objects.obc_om_insight_contribution_for_entity.workbook_name
                    'bc_am_load_objects.obc_om_insight_contribution_for_entity.workflow_stage = 1

                    'REM set workflow stage later.
                    'If validate_only = True Then
                    '    frminsightsubmit.chkvalidate.Checked = True
                    'End If
                    'frminsightsubmit.load_submit_controls()
                    'frminsightsubmit.ShowDialog()
                    'If frminsightsubmit.ok_selected = True Then
                    '    validate_only = False
                    '    If frminsightsubmit.chkvalidate.Checked = True Then
                    '        validate_only = True
                    '    End If
                    '    If bc_cs_central_settings.selected_conn_method <> bc_cs_central_settings.LOCAL Then
                    '        If osec.approval_type = 2 And osec.proxy_user_ids.Count > 1 Then
                    '            bc_am_load_objects.obc_om_insight_contribution_for_entity.author_id = osec.proxy_user_ids(frminsightsubmit.slitanal.SelectedIndex)
                    '            bc_am_load_objects.obc_om_insight_contribution_for_entity.author_name = osec.proxy_user_names(frminsightsubmit.slitanal.SelectedIndex)
                    '        End If
                    '    End If
                    '    bc_am_load_objects.obc_om_insight_contribution_for_entity.workbook_name = frminsightsubmit.stxtname.Text
                    '    'bc_am_load_objects.obc_om_insight_contribution_for_entity.author_name = frminsightsubmit.stxtanal.Text
                    '    'bc_am_load_objects.obc_om_insight_contribution_for_entity.author_id = bc_cs_central_settings.logged_on_user_id
                    '    bc_am_load_objects.obc_om_insight_contribution_for_entity.ignore_checkin = frminsightsubmit.CheckBox6.Checked
                    '    close_flag = False
                    '    If frminsightsubmit.Chkclose.Checked = True Then
                    '        close_flag = True
                    '    End If
                    'Else
                    '    Exit Sub
                    'End If
                End If
                bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve insight", "Validating headers", 10, False, True)
                REM populate object model with valid data
                If oexcel.populate_object_with_data(False) = True Then
                    If validate_only = True And bc_am_insight_formats.client_side_validation = 1 Then
                        omessage = New bc_cs_message("Blue Curve insight", "Validation Passed", bc_cs_message.MESSAGE)
                        Exit Sub
                    End If
                    REM if successful submit data 
                    REM if local save down to sheet
                    REM put workbook in Object
                    REM save Excel sheet
                    REM if close requested copy file close it and write it to bytestream

                    If bc_am_insight_formats.controlled_Submission = 1 And validate_only = False Then
                        If oexcel.save(bc_cs_central_settings.local_repos_path + "tmp" + excelexstension, False) = True Then
                        End If
                        REM if local copy requested then save here
                        If bc_am_insight_formats.local_Save_Enabled = 1 Then
                            REM mark it as read only
                            oexcel.read_only(True)
                            If oexcel.save(bc_am_insight_formats.local_Save_Dir + bc_am_load_objects.obc_om_insight_contribution_for_entity.lead_entity_name + excelexstension, True) = False Then
                                ocommentary = New bc_cs_activity_log("bc_am_in_submit", "New", bc_cs_activity_codes.COMMENTARY, "Attempting to save local copy: " + bc_am_insight_formats.local_Save_Dir + bc_am_load_objects.obc_om_insight_contribution_for_entity.lead_entity_name + excelexstension)
                            End If
                            REM reset now for system model
                            oexcel.read_only(False)
                        End If
                        REM also if set save last excel copy
                        REM save to temporay file in case of failure
                        If bc_cs_central_settings.temp_save = True Then
                            ocommentary = New bc_cs_activity_log("bc_am_in_submit", "new", bc_cs_activity_codes.COMMENTARY, "Saving temp copy:" + bc_cs_central_settings.local_repos_path + "bc_tmp")
                            oexcel.tmp_saveas("bc_tmp")
                        End If
                        If oexcel.save(bc_cs_central_settings.local_repos_path + dfilename, False) = False Then
                            omessage = New bc_cs_message("lue Curve - insight", "Workbook cannot be saved so submission aborted!", bc_cs_message.MESSAGE)
                            oexcel.enable_application_alerts()
                            oexcel.screen_updating(False)
                            Exit Sub
                        End If
                        fs.write_document_to_bytestream(bc_cs_central_settings.local_repos_path + "tmp" + excelexstension, bc_am_load_objects.obc_om_insight_contribution_for_entity.doc_byte, Nothing)
                        fs.delete_file(bc_cs_central_settings.local_repos_path + "tmp" + excelexstension)
                    End If
                    bc_am_load_objects.obc_om_insight_contribution_for_entity.controlled_submission = bc_am_insight_formats.controlled_Submission
                    'If frminsightsubmit.close_mode = bc_am_in_main_frm.LOCAL Then
                    '    ocommentary = New bc_cs_activity_log("bc_am_in_submit", "New", bc_cs_activity_codes.COMMENTARY, "No Network Connected So Submitting to XML file:")
                    '    REM re-read metadata from disk as data has not hit database
                    'ElseIf frminsightsubmit.close_mode = bc_am_in_main_frm.CHECKED_OUT Then

                    '    REM if leave document checked out blank storage of Excel
                    '    bc_am_load_objects.obc_om_insight_contribution_for_entity.checked_out_user_id = bc_cs_central_settings.logged_on_user_id
                    '    ocommentary = New bc_cs_activity_log("bc_am_in_submit", "New", bc_cs_activity_codes.COMMENTARY, "Network submission checked out selected.")
                    'Else
                    '    REM if checked in write excel to doc byte
                    bc_am_load_objects.obc_om_insight_contribution_for_entity.checked_out_user_id = "0"
                    '    ocommentary = New bc_cs_activity_log("bc_am_in_submit", "New", bc_cs_activity_codes.COMMENTARY, "Network submission checked in selected.")
                    'End If
                    bc_am_load_objects.obc_om_insight_contribution_for_entity.date_from = Format(Now, "dd-MM-yyyy HH:mm:ss")
                    ocommentary = New bc_cs_activity_log("bc_am_in_submit", "New", bc_cs_activity_codes.COMMENTARY, "Submitting to financial workflow Stage:" + CStr(bc_am_load_objects.obc_om_insight_contribution_for_entity.workflow_stage))

                    bc_am_load_objects.obc_om_insight_contribution_for_entity.validate_only = False
                    If validate_only = True Then
                        If bc_am_insight_formats.client_side_validation = 0 Then
                            bc_am_load_objects.obc_om_insight_contribution_for_entity.validate_only = True
                        End If
                    End If

                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        'And frminsightsubmit.close_mode <> bc_am_in_main_frm.LOCAL Then
                        Dim gbc As New bc_cs_db_services(False)
                        oexcel.status_bar_text("Saving Data to System via ADO...")
                        bc_cs_central_settings.progress_bar.increment("Saving Data to System")
                        REM PR July 2010
                        bc_am_load_objects.obc_om_insight_contribution_for_entity.certificate.server_errors.Clear()
                        checked_out_user = bc_am_load_objects.obc_om_insight_contribution_for_entity.db_write()

                        If checked_out_user <> "" Then
                            checked_out_user = checked_out_user + " To override this check out please check box and resubmit"
                            omessage = New bc_cs_message("Blue Curve Insight", checked_out_user, bc_cs_message.MESSAGE)
                            Exit Sub
                        End If
                        server_success = True
                        'clear_cell_values()
                        oexcel.status_bar_text("")
                    End If
                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                        'And frminsightsubmit.close_mode <> bc_am_in_main_frm.LOCAL Then
                        Dim gbc As New bc_cs_db_services(False)
                        oexcel.status_bar_text("Saving Data to System via SOAP...")
                        bc_cs_central_settings.progress_bar.increment("Saving Data to System")
                        'checked_out_user = bc_am_load_objects.obc_om_insight_contribution_for_entity.db_write
                        If bc_am_insight_formats.controlled_Submission = 0 Then
                            bc_am_load_objects.obc_om_insight_contribution_for_entity.doc_byte = Nothing
                        End If

                        'PR fix april
                        bc_am_load_objects.obc_om_insight_contribution_for_entity.packet_code = "insight"
                        bc_am_load_objects.obc_om_insight_contribution_for_entity.tmode = bc_cs_soap_base_class.tWRITE
                        server_success = bc_am_load_objects.obc_om_insight_contribution_for_entity.transmit_to_server_and_receive(bc_am_load_objects.obc_om_insight_contribution_for_entity, True)
                        'checked_out_user = bc_am_load_objects.obc_om_insight_contribution_for_entity.write_xml_via_soap_client_request
                        'If checked_out_user <> "" Then
                        'omessage = New bc_cs_message("Blue Curve Insight", checked_out_user, bc_cs_message.MESSAGE)
                        'Exit Sub
                        'End If
                        'clear_cell_values()
                        oexcel.status_bar_text("")
                    End If
                    Dim errors As Boolean = False

                    warningssheet.Clear()
                    warningstype.Clear()
                    warningstx.Clear()
                    warningsrow.Clear()
                    warningscol.Clear()
                    warningsaddress.Clear()
                    Dim err_count As Integer = 0
                    For i = 0 To bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets.Count - 1
                        For j = 0 To bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets(i).warnings.count - 1
                            errors = True
                            If bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets(i).is_warning = True Then
                                warnings = True
                            Else
                                err_count = err_count + 1
                            End If
                            warningssheet.Add(bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets(i).sheet_name)
                            warningstype.Add(bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets(i).warnings(j).type)
                            warningstx.Add(bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets(i).warnings(j).tx)
                            REM March 2-16 use excel address instead
                            Dim ef As New bc_am_ef_functions
                            warningsrow.Add(bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets(i).warnings(j).row)
                            warningscol.Add(bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets(i).warnings(j).column)
                            warningsaddress.Add(ef.excel_address(bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets(i).warnings(j).row, bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets(i).warnings(j).column))
                            Dim ocomm As New bc_cs_activity_log("bc_am_in_submit", "new", bc_cs_activity_codes.COMMENTARY, "Insight submission warnings:" + warningstype(warningstype.Count - 1) + " desc: " + warningstx(warningstx.Count - 1) + "(" + CStr(warningsrow(warningsrow.Count - 1)) + "," + CStr(warningscol(warningscol.Count - 1)) + ")")
                        Next
                        bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets(i).warnings.clear()
                    Next
                    If err_count > 0 Then
                        warnings = False
                    End If

                    If bc_am_insight_formats.controlled_Submission = 0 And errors = False Then
                        REM just save metadata file
                        If bc_am_insight_formats.store_dat_file = 1 Then
                            bc_am_load_objects.obc_om_insight_contribution_for_entity.write_data_to_file(bc_cs_central_settings.local_repos_path + filename)
                        End If
                    End If
                    'If bc_am_insight_formats.controlled_Submission = 1 And errors = False Then
                    '    ocommentary = New bc_cs_activity_log("bc_am_in_submit", "New", bc_cs_activity_codes.COMMENTARY, "Controlled Submission.")
                    '    If frminsightsubmit.close_mode = bc_am_in_main_frm.LOCAL Or frminsightsubmit.close_mode = bc_am_in_main_frm.CHECKED_OUT Then
                    '        REM if local reset metadata from disk as we need last newtrok saved data
                    '        If frminsightsubmit.close_mode = bc_am_in_main_frm.LOCAL Then
                    '            REM local file must recent dataset must be last network submitted set.
                    '            bc_am_load_objects.obc_om_insight_contribution_for_entity = bc_am_load_objects.obc_om_insight_contribution_for_entity.read_data_from_file(bc_cs_central_settings.local_repos_path + filename)
                    '            REM put workbook in this object
                    '            'AAAA()
                    '        ElseIf frminsightsubmit.close_mode = bc_am_in_main_frm.CHECKED_OUT Then
                    '            REM if checkdout save new dataset to disk
                    '            REM write metadata to disk
                    '            bc_am_load_objects.obc_om_insight_contribution_for_entity.write_data_to_file(bc_cs_central_settings.local_repos_path + filename)
                    '        End If
                    '        olocal_workbooks = New bc_om_insight_contribution_for_entities
                    '        REM add to local store
                    '        bc_cs_central_settings.progress_bar.increment("Storing Local Copy...")
                    '        bc_cs_central_settings.progress_bar.hide()
                    '        bc_cs_central_settings.progress_bar = New bc_cs_progress("Insight", "Checking for local store", 1, False, True)
                    '        If fs.check_document_exists(bc_cs_central_settings.local_repos_path + "bc_local_workbooks.dat") Then
                    '            REM read in
                    '            bc_cs_central_settings.progress_bar.increment("Reading in local store")
                    '            olocal_workbooks = olocal_workbooks.read_data_from_file(bc_cs_central_settings.local_repos_path + "bc_local_workbooks.dat")
                    '        End If
                    '        bc_cs_central_settings.progress_bar.increment("Saving Workbook and Metadata to local store")
                    '        bc_am_load_objects.obc_om_insight_contribution_for_entity.last_submission_date = Now
                    '        REM reattach workbook
                    '        oexcel.hide_worksheets()
                    '        oexcel.save(bc_cs_central_settings.local_repos_path + "tmp" + excelexstension, False)
                    '        oexcel.save(bc_cs_central_settings.local_repos_path + dfilename, False)
                    '        fs.write_document_to_bytestream(bc_cs_central_settings.local_repos_path + "tmp" + excelexstension, bc_am_load_objects.obc_om_insight_contribution_for_entity.doc_byte, Nothing)
                    '        fs.delete_file(bc_cs_central_settings.local_repos_path + "tmp" + excelexstension)
                    '        Dim found As Boolean = False
                    '        For i = 0 To olocal_workbooks.bc_om_insight_contribution_for_entity.Count - 1
                    '            If olocal_workbooks.bc_om_insight_contribution_for_entity(i).lead_entity_id = bc_am_load_objects.obc_om_insight_contribution_for_entity.lead_entity_id And olocal_workbooks.bc_om_insight_contribution_for_entity(i).contributor_id = bc_am_load_objects.obc_om_insight_contribution_for_entity.contributor_id Then
                    '                olocal_workbooks.bc_om_insight_contribution_for_entity(i) = bc_am_load_objects.obc_om_insight_contribution_for_entity
                    '                found = True
                    '            End If
                    '        Next
                    '        If found = False Then
                    '            olocal_workbooks.bc_om_insight_contribution_for_entity.Add(bc_am_load_objects.obc_om_insight_contribution_for_entity)
                    '        End If
                    '        REM save back down
                    '        olocal_workbooks.write_data_to_file(bc_cs_central_settings.local_repos_path + "bc_local_workbooks.dat")
                    '    ElseIf frminsightsubmit.close_mode = bc_am_in_main_frm.CHECKED_IN Then
                    '        REM delete local xml file and workbook
                    '        Try
                    '            oexcel.screen_updating(False)
                    '            oexcel.close()
                    '        Catch

                    '        End Try
                    '        If fs.check_document_exists(bc_cs_central_settings.local_repos_path + dfilename) Then
                    '            fs.delete_file(bc_cs_central_settings.local_repos_path + dfilename)
                    '        End If
                    '        If fs.check_document_exists(bc_cs_central_settings.local_repos_path + filename) Then
                    '            fs.delete_file(bc_cs_central_settings.local_repos_path + filename)
                    '        End If
                    '        REM delete any local store records
                    '        REM add to local store 
                    '        If fs.check_document_exists(bc_cs_central_settings.local_repos_path + "bc_local_workbooks.dat") Then
                    '            REM read in
                    '            olocal_workbooks = New bc_om_insight_contribution_for_entities
                    '            bc_cs_central_settings.progress_bar.increment("Consolidating Local Store")
                    '            olocal_workbooks = olocal_workbooks.read_data_from_file(bc_cs_central_settings.local_repos_path + "bc_local_workbooks.dat")
                    '            REM remove record for checked in workbook
                    '            For i = 0 To olocal_workbooks.bc_om_insight_contribution_for_entity.Count - 1
                    '                If olocal_workbooks.bc_om_insight_contribution_for_entity(i).lead_entity_id = bc_am_load_objects.obc_om_insight_contribution_for_entity.lead_entity_id And olocal_workbooks.bc_om_insight_contribution_for_entity(i).contributor_id = bc_am_load_objects.obc_om_insight_contribution_for_entity.contributor_id Then
                    '                    olocal_workbooks.bc_om_insight_contribution_for_entity.RemoveAt(i)
                    '                    olocal_workbooks.write_data_to_file(bc_cs_central_settings.local_repos_path + "bc_local_workbooks.dat")
                    '                    Exit For
                    '                End If
                    '            Next
                    '        End If
                    '    End If
                    'Else
                    oexcel.enable_application_alerts()
                    oexcel.screen_updating(False)
                    ocommentary = New bc_cs_activity_log("bc_am_in_submit", "New", bc_cs_activity_codes.COMMENTARY, "Uncontrolled Submission.")
                    'End If
                    If Me.close_flag = True Then
                        Try
                            oexcel.enable_application_alerts()
                            oexcel.screen_updating(False)
                            oexcel.close()
                        Catch

                        End Try
                    Else
                        ocommentary = New bc_cs_activity_log("bc_am_in_submit", "New", bc_cs_activity_codes.COMMENTARY, "Uncontrolled Submission.")
                    End If
                Else
                    oexcel.screen_updating(False)
                    Me.close_flag = False
                End If
            End If



        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_submit", "new", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            'clear_cell_values()
            bc_cs_central_settings.progress_bar.unload()
            If bc_am_insight_formats.hide_Sheets = 1 And Me.close_flag = False Then
                oexcel.hide_worksheets()
            End If

            If Me.close_flag = False And load_failed = False Then
                oexcel.enable_application_alerts()
            End If

            If server_success = True And warningstype.Count = 0 Then
                If validate_only = False Then
                    omessage = New bc_cs_message("Blue Curve - insight", "Submission Successful", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Else
                    omessage = New bc_cs_message("Blue Curve - insight", "Validation Passed", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                End If
            ElseIf server_success = True And warningstype.Count > 0 Then
                Dim show_warnings As Boolean = True
                If validate_only = False Then
                    If warnings = False Then
                        omessage = New bc_cs_message("Blue Curve - insight", "Submission Failed!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Else
                        Dim wtx As String = ""
                        For i = 0 To warningstype.Count - 1
                            If i > 0 Then
                                wtx = wtx + "," + CStr(warningstx(i))
                            Else
                                wtx = warningstx(i)
                            End If
                        Next
                        omessage = New bc_cs_message("Blue Curve - insight", "Validation Warnings: " + vbCrLf + vbCrLf + wtx + vbCrLf + vbCrLf + "Select yes to continue the submission or no to check the warnings first!", bc_cs_message.MESSAGE, True, False, "Submit", "View", True)
                        If omessage.cancel_selected = False Then
                            show_warnings = False
                            bc_am_load_objects.obc_om_insight_contribution_for_entity.no_validate = True
                            bc_am_load_objects.obc_om_insight_contribution_for_entity.certificate.server_errors.Clear()
                            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                                bc_am_load_objects.obc_om_insight_contribution_for_entity.db_write()
                            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                                oexcel.status_bar_text("Saving Data to System via SOAP...")
                                bc_cs_central_settings.progress_bar.increment("Saving Data to System")
                                bc_am_load_objects.obc_om_insight_contribution_for_entity.packet_code = "insight"
                                bc_am_load_objects.obc_om_insight_contribution_for_entity.tmode = bc_cs_soap_base_class.tWRITE
                                If bc_am_load_objects.obc_om_insight_contribution_for_entity.transmit_to_server_and_receive(bc_am_load_objects.obc_om_insight_contribution_for_entity, True) = True Then
                                    omessage = New bc_cs_message("Blue Curve - insight", "Submission Successful", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                                End If

                            End If
                            bc_am_load_objects.obc_om_insight_contribution_for_entity.no_validate = False
                            oexcel.status_bar_text("")
                        End If

                    End If
                Else
                    If warnings = False Then
                        omessage = New bc_cs_message("Blue Curve - insight", "Validation Failed !", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Else
                        omessage = New bc_cs_message("Blue Curve - insight", "Validation Passed With Warnings !", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    End If
                End If

                If show_warnings = True Then
                    Dim fis As New bc_dx_cell_validation
                    Dim cis As New cbc_dx_cell_validation
                    If cis.load_data(fis, oexcel, warningstype, warningssheet, warningstx, warningsrow, warningscol, warningsaddress) = True Then
                        Dim oapi As New API
                        API.SetWindowPos(fis.Handle.ToInt32, API.HWND_TOPMOST, 10, 10, 1, 1, 1)
                        fis.Show()
                    End If
                End If


            End If
            clear_cell_values()
            slog = New bc_cs_activity_log("bc_am_in_submit", "new", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    Public Sub dotask()
        Dim slog = New bc_cs_activity_log("bc_am_in_submit", "dotask-submit", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            bc_am_load_objects.obc_om_insight_contribution_for_entity.db_write()
            clear_cell_values()
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_submit", "dotask-submit", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_in_submit", "dotask-submit", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Public Sub clear_cell_values()
        Dim slog = New bc_cs_activity_log("bc_am_in_submit", "clear_cell_values", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            bc_am_load_objects.obc_om_insight_contribution_for_entity.clear_cell_values()
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_submit", "clear_cell_values", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_in_submit", "clear_cell_values", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class
REM ==========================================
REM Blue Curve Limited 2005
REM Module:       Insight Cell Validation
REM Type:         Application Module
REM Description:  Links Excel worksheet
REM Version:      1
REM Change history
REM ==========================================
Public Class insight_cell_validation
    Dim value As Long
    Dim validation_id As Long
    Public Sub New(ByVal value As Long, ByVal validation_id As Integer)
        Me.value = value
        Me.validation_id = validation_id
    End Sub
End Class
Public Class bc_am_in_open
    Public Sub New(ByVal show_form As Boolean, ByVal ao_object As Object, ByVal ao_type As String)
        Dim slog = New bc_cs_activity_log("bc_am_in_open", "new", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim omessage As bc_cs_message
        Dim ocommentary As bc_cs_activity_log
        Dim i, j As Integer
        Dim fn, mfn As String
        Dim user As String = ""
        Dim caption As String = ""
        Dim oexcel As Object
        Dim wopen As Boolean
        Dim from_server As Boolean = False
        Dim local As Boolean = False
        Dim fs As New bc_cs_file_transfer_services
        Dim excelexstension As String

        Try
            REM SW cope with office versions
            If bc_cs_central_settings.userOfficeStatus = 2 Then
                excelexstension = ".xlsx"
            Else
                excelexstension = ".xls"
            End If

            wopen = True
            If bc_am_insight_formats.controlled_Submission <> 1 Then
                omessage = New bc_cs_message("Blue Curve - insight", "System configured for uncontrolled submission please find workbook manually!", bc_cs_message.MESSAGE)
                Exit Sub
            End If
            If ao_type <> bc_ao_in_object.EXCEL_DOC Then
                omessage = New bc_cs_message("Blue Curve Create", "Application Object Type: " + ao_type + " not supported", bc_cs_message.MESSAGE)
            Else
                REM instantiate correct AO object
                If ao_type = bc_ao_in_object.EXCEL_DOC Then
                    REM load system
                    bc_am_load_objects.obc_om_insight_contribution_for_entity = Nothing
                    Dim open_frm As New bc_am_in_open_frm
                    open_frm.ShowDialog()

                    If open_frm.ok_selected = True Then
                        REM if document checked out or local get from either file
                        REM system or store
                        With bc_am_load_objects.obc_om_insight_contribution_for_entity
                            REM find selected workbook
                            For i = 0 To bc_am_load_objects.obc_workbooks.bc_om_insight_contribution_for_entity.Count
                                If bc_am_load_objects.obc_workbooks.bc_om_insight_contribution_for_entity(i).lead_entity_id = .lead_entity_id And bc_am_load_objects.obc_workbooks.bc_om_insight_contribution_for_entity(i).contributor_id = .contributor_id Then
                                    caption = "Blue Curve Insight: " + CStr(bc_am_load_objects.obc_workbooks.bc_om_insight_contribution_for_entity(i).lead_entity_name + " " + bc_am_load_objects.obc_workbooks.bc_om_insight_contribution_for_entity(i).contributor_name)
                                    Exit For
                                End If
                            Next
                            fn = "insight_" + CStr(.lead_entity_id) + "_" + CStr(.contributor_id) + excelexstension
                            mfn = "insight_" + CStr(.lead_entity_id) + "_" + CStr(.contributor_id) + ".xml"
                            If bc_am_load_objects.obc_workbooks.bc_om_insight_contribution_for_entity(i).checked_out_user_id = bc_cs_central_settings.logged_on_user_id Or bc_am_load_objects.obc_workbooks.bc_om_insight_contribution_for_entity(i).checked_out_user_id = -1 Or open_frm.PictureBox6.BorderStyle = Windows.Forms.BorderStyle.Fixed3D Then
                                local = True
                                from_server = False
                                REM check if workbook exists on file system
                                If Not fs.check_document_exists(bc_cs_central_settings.local_repos_path + fn) Then
                                    REM if not extract from XML local store
                                    REM if document is from local list then extract
                                    If open_frm.PictureBox6.BorderStyle = Windows.Forms.BorderStyle.Fixed3D Then
                                        ocommentary = New bc_cs_activity_log("bc_am_in_open", "new", bc_cs_activity_codes.COMMENTARY, "Workbook: " + fn + " being opened locally and extracted from local store.")
                                        fs.write_bytestream_to_document(bc_cs_central_settings.local_repos_path + fn, bc_am_load_objects.obc_workbooks.bc_om_insight_contribution_for_entity(i).doc_byte, Nothing)
                                        REM check if metadata file exists on file system
                                        If Not fs.check_document_exists(bc_cs_central_settings.local_repos_path + mfn) Then
                                            REM if not extract from XML local store
                                            ocommentary = New bc_cs_activity_log("bc_am_in_open", "new", bc_cs_activity_codes.COMMENTARY, "Meta Data: " + mfn + " being extracted from local store.")
                                            bc_am_load_objects.obc_workbooks.bc_om_insight_contribution_for_entity(i).write_xml_to_file(bc_cs_central_settings.local_repos_path + mfn)
                                        End If
                                    Else
                                        REM local copy has gone so flag this and get from server
                                        Dim omsg As New bc_cs_message("Blue Curve", "Workbook is checked out to you but local copy has been deleted network do you want to retrieve last submitted copy?", bc_cs_message.MESSAGE, True)
                                        If omsg.cancel_selected = True Then
                                            Exit Sub
                                        Else
                                            from_server = True
                                        End If
                                    End If
                                End If

                            End If
                            If local = False Or from_server = True Then
                                REM if document checked in request from server
                                Dim role As String = ""
                                If bc_am_load_objects.obc_workbooks.bc_om_insight_contribution_for_entity(i).checked_out_user_id <> 0 Then
                                    For j = 0 To bc_am_load_objects.obc_users.user.Count - 1
                                        If bc_am_load_objects.obc_users.user(j).id = bc_am_load_objects.obc_workbooks.bc_om_insight_contribution_for_entity(i).checked_out_user_id Then
                                            user = bc_am_load_objects.obc_users.user(j).first_name + " " + bc_am_load_objects.obc_users.user(j).surname
                                        End If
                                        REM current user
                                        If bc_am_load_objects.obc_users.user(j).id = bc_cs_central_settings.logged_on_user_id Then
                                            role = bc_am_load_objects.obc_users.user(j).role
                                        End If
                                    Next
                                    If from_server = False Then
                                        If Trim(role) = "Manager" Or Trim(role) = "Administrator" Then
                                            omessage = New bc_cs_message("Blue Curve Create", "Workbook is Checked out to: " + user + " but can be overriden as you are role: " + role, bc_cs_message.MESSAGE, True)
                                            If omessage.cancel_selected = True Then
                                                wopen = False
                                            Else
                                                wopen = True
                                            End If
                                        Else
                                            omessage = New bc_cs_message("Blue Curve Create", "Workbook is Checked out to: " + user, bc_cs_message.MESSAGE)
                                            wopen = False
                                            Exit Sub
                                        End If
                                    End If
                                End If
                                If wopen = True Then
                                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                                        bc_am_load_objects.obc_workbooks.bc_om_insight_contribution_for_entity(i) = bc_am_load_objects.obc_workbooks.bc_om_insight_contribution_for_entity(i).populate_from_files(Nothing)
                                    ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                                        bc_am_load_objects.obc_workbooks.bc_om_insight_contribution_for_entity(i).certificate.user_id = bc_cs_central_settings.logged_on_user_id
                                        bc_am_load_objects.obc_workbooks.bc_om_insight_contribution_for_entity(i).load_from_files = True
                                        bc_am_load_objects.obc_workbooks.bc_om_insight_contribution_for_entity(i).tmode = bc_am_load_objects.obc_workbooks.bc_om_insight_contribution_for_entity(i).tREAD
                                        bc_am_load_objects.obc_workbooks.bc_om_insight_contribution_for_entity(i).transmit_to_server_and_receive(bc_am_load_objects.obc_workbooks.bc_om_insight_contribution_for_entity(i), True)
                                        bc_am_load_objects.obc_workbooks.bc_om_insight_contribution_for_entity(i).load_from_files = False
                                    End If
                                    REM write down document
                                    Try
                                        fs.write_bytestream_to_document(bc_cs_central_settings.local_repos_path + fn, bc_am_load_objects.obc_workbooks.bc_om_insight_contribution_for_entity(i).doc_byte, Nothing)
                                    Catch
                                        omessage = New bc_cs_message("Blue Curve Create", "Workbook not found on server.", bc_cs_message.MESSAGE)
                                        Exit Sub
                                    End Try

                                    REM if document doe not exist then flag this
                                    If Not fs.check_document_exists(bc_cs_central_settings.local_repos_path + fn) Then
                                        Exit Sub
                                    End If
                                    REM write down XML file

                                    bc_am_load_objects.obc_workbooks.bc_om_insight_contribution_for_entity(i).write_data_to_file(bc_cs_central_settings.local_repos_path + mfn)
                                    REM attach to local store
                                    Dim bc_local_workbooks As New bc_om_insight_contribution_for_entities
                                    If fs.check_document_exists(bc_cs_central_settings.local_repos_path + "bc_local_workbooks.dat") Then
                                        bc_local_workbooks = bc_local_workbooks.read_data_from_file(bc_cs_central_settings.local_repos_path + "bc_local_workbooks.dat")
                                    End If
                                    bc_am_load_objects.obc_workbooks.bc_om_insight_contribution_for_entity(i).last_submission_date = Now
                                    bc_local_workbooks.bc_om_insight_contribution_for_entity.Add(bc_am_load_objects.obc_workbooks.bc_om_insight_contribution_for_entity(i))
                                    bc_local_workbooks.write_data_to_file(bc_cs_central_settings.local_repos_path + "bc_local_workbooks.dat")
                                End If
                            End If
                        End With
                        REM open workbook
                        If wopen = True Then
                            oexcel = New bc_ao_in_excel(ao_object)
                            oexcel.open(bc_cs_central_settings.local_repos_path + fn)
                            oexcel.set_caption(caption)
                            oexcel.enable_application_alerts()
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_open", "new", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

            slog = New bc_cs_activity_log("bc_am_in_open", "new", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
End Class
Public Class bc_am_in_open_workbook_list

    Public Sub New(ByVal network_flag As Boolean, ByVal mine As Boolean)
        Dim slog As New bc_cs_activity_log("bc_am_in_open_workbook_list", "new", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            bc_am_load_objects.obc_workbooks = New bc_om_insight_contribution_for_entities
            If network_flag = True Then
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                    bc_am_load_objects.obc_workbooks.mine = mine
                    bc_am_load_objects.obc_workbooks.tmode = bc_cs_soap_base_class.tREAD
                    bc_am_load_objects.obc_workbooks.transmit_to_server_and_receive(bc_am_load_objects.obc_workbooks, True)
                ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    bc_am_load_objects.obc_workbooks.mine = mine
                    bc_am_load_objects.obc_workbooks.db_read()
                End If
            Else
                Dim fs As New bc_cs_file_transfer_services
                If fs.check_document_exists(bc_cs_central_settings.local_repos_path + "bc_local_workbooks.dat") Then
                    bc_am_load_objects.obc_workbooks = bc_am_load_objects.obc_workbooks.read_data_from_file(bc_cs_central_settings.local_repos_path + "bc_local_workbooks.dat")
                End If
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_open_workbook_list", "new", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_in_open_workbook_list", "new", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
End Class
Public Class bc_am_in_download_values
    Dim load_failed = False
    Public Sub New(ByVal show_form As Boolean, ByVal ao_object As Object, ByVal ao_type As String, ByVal validate_only As Boolean)
        Dim slog = New bc_cs_activity_log("bc_am_in_download_values", "new", bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim oexcel As bc_ao_in_object = Nothing


        Dim omessage As New bc_cs_message
        'Dim frminsightsubmit As New bc_am_in_main_frm
        Dim olocal_workbooks As New bc_om_insight_contribution_for_entities

        Dim i As Integer
        Dim fs As New bc_cs_file_transfer_services
        Try
            If ao_type <> bc_ao_in_object.EXCEL_DOC Then
                omessage = New bc_cs_message("Blue Curve Create", "Application Object Type: " + ao_type + " not supported", bc_cs_message.MESSAGE)
            Else
                REM instantiate correct AO object
                If ao_type = bc_ao_in_object.EXCEL_DOC Then
                    If IsNothing(ao_object) Then
                        load_failed = True
                        omessage = New bc_cs_message("Blue Curve insight", "Workbook must be Open", bc_cs_message.MESSAGE)
                        Exit Sub
                    End If
                    oexcel = New bc_ao_in_excel(ao_object)
                    oexcel.disable_application_alerts()
                    bc_am_load_objects.obc_in_ao_object = oexcel
                Else
                    Exit Sub
                End If
                oexcel.show_worksheets()
                REM maybe a form later
                REM load data for the contribution object based on selected criteria
                REM get parts from spreadsheet to get filename
                bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve insight", "Loading Metadata...", 10, False, True)
                Dim keys As String
                keys = oexcel.get_sheet_keys
                If keys = "No Keys Found" Then
                    If keys = "Read Only" Then
                        omessage = New bc_cs_message("Blue Curve insight", "Workbook is read only copy cannot Validate/Submit.", bc_cs_message.MESSAGE)
                        Exit Sub
                    End If

                    omessage = New bc_cs_message("Blue Curve insight", "This workbook has never been linked to Blue Curve please Build first.", bc_cs_message.MESSAGE)
                    Exit Sub
                Else
                    Dim orecreate As New bc_am_recreate_workbook_metadata(keys, oexcel)
                    If orecreate.recreate = False Then
                        omessage = New bc_cs_message("Blue Curve", "Metadata file not found and cant be recreated!", bc_cs_message.MESSAGE)
                        Exit Sub
                    End If
                End If
                REM attempt to read metadata into memory
                If bc_am_load_objects.obc_om_insight_contribution_for_entity.workflow_stage = 0 Then
                    bc_am_load_objects.obc_om_insight_contribution_for_entity.workflow_stage = 1
                End If
                REM make sure actually workboo is deleted for size issues
                bc_am_load_objects.obc_om_insight_contribution_for_entity.doc_byte = Nothing

                REM populate object model with valid data
                REM just populate headers
                If oexcel.populate_object_with_data(True) = False Then
                    Exit Sub
                Else
                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        bc_am_load_objects.obc_om_insight_contribution_for_entity.db_read_values(Nothing)
                    Else
                        bc_am_load_objects.obc_om_insight_contribution_for_entity.tmode = bc_om_insight_contribution_for_entity.tREAD_VALUES
                        If bc_am_load_objects.obc_om_insight_contribution_for_entity.transmit_to_server_and_receive(bc_am_load_objects.obc_om_insight_contribution_for_entity, True) = False Then
                            Exit Sub
                        End If
                    End If
                    REM put values into excel
                    For i = 0 To bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets.Count - 1
                        oexcel.populate_output_data(bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets(i))
                    Next
                End If
            End If
            bc_cs_central_settings.progress_bar.hide()
            oexcel.hide_worksheets()

            REM EVR JUNE 2013 refresh lists

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                bc_am_load_objects.obc_om_insight_contribution_for_entity.db_read()
            Else
                bc_am_load_objects.obc_om_insight_contribution_for_entity.tmode = bc_om_insight_contribution_for_entity.tREAD
                If bc_am_load_objects.obc_om_insight_contribution_for_entity.transmit_to_server_and_receive(bc_am_load_objects.obc_om_insight_contribution_for_entity, True) = False Then
                    Exit Sub
                End If
            End If
            oexcel.evaluate_list_boxes(bc_am_load_objects.obc_om_insight_contribution_for_entity.lead_class_id, bc_am_load_objects.obc_om_insight_contribution_for_entity.lead_entity_id, bc_am_load_objects.obc_om_insight_contribution_for_entity.contributor_id)


        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_download_values", "new", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

            slog = New bc_cs_activity_log("bc_am_in_download_values", "new", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
End Class
'Public Class bc_am_force_check_in
'    Public close_flag As Boolean = False
'    Dim load_failed = False
'    Public Sub New(ByVal show_form As Boolean, ByVal ao_object As Object, ByVal ao_type As String)
'        Dim slog = New bc_cs_activity_log("bc_am_force_check_in", "new", bc_cs_activity_codes.TRACE_ENTRY, "")

'        Dim dfilename As String
'        Dim oexcel As bc_ao_in_object = Nothing

'        Dim omessage As New bc_cs_message
'        Dim frminsightsubmit As New bc_am_in_main_frm

'        Dim server_success As Boolean
'        Dim i As Integer
'        server_success = False

'        Try
'            If ao_type <> bc_ao_in_object.EXCEL_DOC Then
'                omessage = New bc_cs_message("Blue Curve Create", "Application Object Type: " + ao_type + " not supported", bc_cs_message.MESSAGE)
'            Else
'                REM instantiate correct AO object
'                If ao_type = bc_ao_in_object.EXCEL_DOC Then
'                    If IsNothing(ao_object) Then
'                        load_failed = True
'                        omessage = New bc_cs_message("Blue Curve insight", "Workbook must be Open", bc_cs_message.MESSAGE)
'                        Exit Sub
'                    End If
'                    oexcel = New bc_ao_in_excel(ao_object)
'                    oexcel.disable_application_alerts()
'                    bc_am_load_objects.obc_in_ao_object = oexcel
'                End If
'                oexcel.show_worksheets()
'                REM maybe a form later
'                REM load data for the contribution object based on selected criteria
'                REM get parts from spreadsheet to get filename
'                Dim filename As String
'                Dim keys As String
'                If bc_am_insight_formats.controlled_Submission = 0 Then
'                    omessage = New bc_cs_message("Blue Curve Insight", "Cannot undo checkout in as running in uncontrolled submission mode.", bc_cs_message.MESSAGE)
'                    Exit Sub
'                End If

'                keys = oexcel.get_sheet_keys()
'                If keys <> "No Keys Found" Then
'                    If keys = "Read Only" Then
'                        omessage = New bc_cs_message("Blue Curve insight", "Workbook is read only copy cannot Validate/Submit.", bc_cs_message.MESSAGE)
'                        Exit Sub
'                    End If

'                    REM SW cope with office versions
'                    If bc_cs_central_settings.userOfficeStatus = 2 Then
'                        dfilename = keys + ".xlsx"
'                    Else
'                        dfilename = keys + ".xls"
'                    End If
'                    'dfilename = keys + ".xls"

'                    filename = keys + ".xml"
'                Else
'                    omessage = New bc_cs_message("Blue Curve insight", "This workbook has never been linked to Blue Curve please Build first.", bc_cs_message.MESSAGE)
'                    Exit Sub
'                End If
'                If keys = "No Keys Found" Then
'                    omessage = New bc_cs_message("Blue Curve insight", "This workbook has never been linked to Blue Curve please Build first.", bc_cs_message.MESSAGE)
'                    Exit Sub
'                Else
'                    Dim orecreate As New bc_am_recreate_workbook_metadata(keys, oexcel)
'                    If orecreate.recreate = False Then
'                        omessage = New bc_cs_message("Blue Curve", "Metadata file not found and cant be recreated!", bc_cs_message.MESSAGE)
'                        Exit Sub
'                    End If
'                End If

'                With bc_am_load_objects.obc_om_insight_contribution_for_entity
'                    If .workbook_name = "" Then
'                        .workbook_name = .contributor_name + "(" + .lead_class_name + "): " + .lead_entity_name
'                    End If
'                    If .author_name = "" Then
'                        For i = 0 To bc_am_load_objects.obc_users.user.Count - 1
'                            With bc_am_load_objects.obc_users.user(i)
'                                If UCase(Trim(.os_user_name)) = UCase(Trim(bc_cs_central_settings.logged_on_user_name)) Then
'                                    bc_am_load_objects.obc_om_insight_contribution_for_entity.author_name = .first_name + " " + .surname
'                                End If
'                            End With
'                        Next
'                    End If
'                End With
'                bc_am_load_objects.obc_om_insight_contribution_for_entity.workflow_stage = 1
'                bc_am_load_objects.obc_om_insight_contribution_for_entity.author_id = "Force Check In"

'                If show_form = True Then
'                    omessage = New bc_cs_message("Blue Curve Insight", "Are you sure you want to discard this workbook?", bc_cs_message.MESSAGE, True, False, "Yes", "No")
'                    If omessage.cancel_selected = True Then
'                        Exit Sub
'                    End If
'                End If
'                REM close workbook without saving
'                bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve insight", "Undoing Check Out...", 10, False, True)
'                oexcel.close()
'                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
'                    bc_am_load_objects.obc_om_insight_contribution_for_entity.db_write()
'                    close_flag = True
'                ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
'                    bc_am_load_objects.obc_om_insight_contribution_for_entity.tmode = bc_cs_soap_base_class.tWRITE
'                    bc_am_load_objects.obc_om_insight_contribution_for_entity.no_send_back = True
'                    server_success = bc_am_load_objects.obc_om_insight_contribution_for_entity.transmit_to_server_and_receive(bc_am_load_objects.obc_om_insight_contribution_for_entity, True)
'                    If server_success = True Then
'                        close_flag = True
'                    End If
'                End If
'                REM delete local copies
'                If close_flag = True Then
'                    Dim olocal_workbooks As bc_om_insight_contribution_for_entities
'                    Dim fs As New bc_cs_file_transfer_services
'                    If fs.check_document_exists(bc_cs_central_settings.local_repos_path + dfilename) Then
'                        fs.delete_file(bc_cs_central_settings.local_repos_path + dfilename)
'                    End If
'                    If fs.check_document_exists(bc_cs_central_settings.local_repos_path + filename) Then
'                        fs.delete_file(bc_cs_central_settings.local_repos_path + filename)
'                    End If
'                    REM delete any local store records
'                    REM add to local store 
'                    If fs.check_document_exists(bc_cs_central_settings.local_repos_path + "bc_local_workbooks.dat") Then
'                        REM read in
'                        olocal_workbooks = New bc_om_insight_contribution_for_entities
'                        bc_cs_central_settings.progress_bar.increment("Consolidating Local Store")
'                        olocal_workbooks = olocal_workbooks.read_data_from_file(bc_cs_central_settings.local_repos_path + "bc_local_workbooks.dat")
'                        REM remove record for checked in workbook
'                        For i = 0 To olocal_workbooks.bc_om_insight_contribution_for_entity.Count - 1
'                            If olocal_workbooks.bc_om_insight_contribution_for_entity(i).lead_entity_id = bc_am_load_objects.obc_om_insight_contribution_for_entity.lead_entity_id And olocal_workbooks.bc_om_insight_contribution_for_entity(i).contributor_id = bc_am_load_objects.obc_om_insight_contribution_for_entity.contributor_id Then
'                                olocal_workbooks.bc_om_insight_contribution_for_entity.RemoveAt(i)
'                                olocal_workbooks.write_data_to_file(bc_cs_central_settings.local_repos_path + "bc_local_workbooks.dat")
'                                Exit For
'                            End If
'                        Next
'                    End If
'                End If
'            End If

'        Catch ex As Exception
'            Dim db_err As New bc_cs_error_log("bc_am_force_check_in", "new", bc_cs_error_codes.USER_DEFINED, ex.Message)
'        Finally
'            bc_cs_central_settings.progress_bar.unload()
'            slog = New bc_cs_activity_log("bc_am_force_check_in", "new", bc_cs_activity_codes.TRACE_EXIT, "")
'        End Try

'    End Sub
'End Class