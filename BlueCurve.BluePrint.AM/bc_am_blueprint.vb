Imports BlueCurve.Core.AS
Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports System.Windows.Forms
'Imports System.Text

'==========================================
' Bluecurve Limited 2008
' Module:         BluePrint
' Type:           AM
' Desciption:     main form controller
' Public Methods: Show
'                 NavigationBarSelection  
'                 EnableToolBar
'                 ActionToolBar
' Version:        1.0
' Change history:
'
'==========================================
Public Class bc_am_blueprint

    ' icon constants
    Private Const create_new_icon As Integer = 0
    Private Const register_icon As Integer = 1
    Private Const open_icon As Integer = 2
    Private Const delete_icon As Integer = 3
    Private Const clone_icon As Integer = 4
    Private Const export_icon As Integer = 5
    Private Const import_icon As Integer = 6
    Private Const amend_icon As Integer = 7
    Private Const parameters_icon As Integer = 8
    Private Const copy_icon As Integer = 9
    Private Const templates_icon As Integer = 0
    Private Const components_icon As Integer = 1
    Private Const publicationTypes_icon As Integer = 2
    Private Const composites_icon As Integer = 3
    Private Const data_def_icon As Integer = 4
    Private Const table_icon As Integer = 4

    ' main view
    Private view As bc_am_bp_main
    'template components view
    Private viewTempComp As bc_am_bp_template_components
    'register new table view
    Private viewRegTable As bc_am_bp_register_table
    'registered tables view
    Private viewRegTables As bc_am_bp_registered_tables
    'update registered tables view
    Private viewUpdRegTables As bc_am_bp_update_registered_table
    'template advanced view
    Private viewTempAdvanced As bc_am_bp_advanced

    'contexts Collection
    Private contexts As ComboBoxHelper

    Public Sub New(ByVal view As bc_am_bp_main)

        view.Controller = Me
        Me.view = view

    End Sub

    Public Sub New(ByVal view As bc_am_bp_template_components)

        view.Controller = Me
        Me.viewTempComp = view

    End Sub

    Public Sub New(ByVal view As bc_am_bp_register_table)

        view.Controller = Me
        Me.viewRegTable = view

    End Sub

    Public Sub New(ByVal view As bc_am_bp_update_registered_table)

        view.Controller = Me
        Me.viewUpdRegTables = view

    End Sub

    Public Sub New(ByVal view As bc_am_bp_registered_tables)

        view.Controller = Me
        Me.viewRegTables = view

    End Sub

    Public Sub New(ByVal view As bc_am_bp_advanced)

        view.Controller = Me
        Me.viewTempAdvanced = view

    End Sub

    Public Sub Show()

        Dim log = New bc_cs_activity_log("bc_am_BluePrint", "Show", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            ' load default view (templates)
            view.uxNavBar.Items(0).Selected = True

            SetRoleUserDB(view)

            Application.DoEvents()

            view.MenuItem1.Visible = False
            If bc_cs_central_settings.show_authentication_form = 1 Or bc_cs_central_settings.override_username_password = True Then
                view.MenuItem1.Visible = True
            End If

            ' show the main form
            view.ShowDialog()

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_BluePrint", "Show", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_BluePrint", "Show", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Friend Sub NavigationBarSelection()

        Dim log = New bc_cs_activity_log("bc_am_BluePrint", "NavigationBarSelection", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            view.uxcreate.Visible = False
            view.uxemail.Visible = False

            ' load the relevant list
            If view.uxNavBar.SelectedIndices.Count = 1 Then
                view.uxGenericList.Enabled = True
                Select Case view.uxNavBar.SelectedIndices(0)
                    Case templates_icon
                        components_selected = False
                        If view.uxcreate.Checked = True Then
                            loadTemplates()
                        Else
                            loadEmailTemplates()
                        End If
                        view.uxcreate.Visible = True
                        view.uxemail.Visible = True

                    Case components_icon
                        components_selected = True
                        view.uxcreate.Visible = True
                        view.uxemail.Visible = True
                        If view.uxcreate.Checked = True Then
                            LoadComponents(view)
                        Else
                            LoadEmailComponents(view)
                        End If
                    Case publicationTypes_icon
                        loadPubTypes()
                    Case composites_icon
                        loadPubTypes(True)
                    Case data_def_icon
                        loadContexts(False)
                End Select

                ' dynamically build the toolbar
                BuildToolBar()

                ' select the first item in the list
                setSelectedItem()
            Else
                EnableToolBar(False)
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_BluePrint", "NavigationBarSelection", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_BluePrint", "NavigationBarSelection", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    Public components_selected As Boolean = False
    Friend Sub ActionToolbar(ByVal btn As ToolBarButton)

        Dim log = New bc_cs_activity_log("bc_am_BluePrint", "ActionToolbar", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Select Case view.uxNavBar.SelectedIndices(0)
                Case templates_icon

                    If view.uxcreate.Checked = True Then
                        Select Case btn.ImageIndex
                            Case create_new_icon
                                ' set title and default path
                                view.uxSaveFileDlg.Title = "Create New"
                                view.uxSaveFileDlg.InitialDirectory = bc_cs_central_settings.local_template_path
                                view.uxSaveFileDlg.FileName = ""
                                view.uxSaveFileDlg.Filter = "Word Templates (*.dot)|*.dot|Excel Files (*.xls)|*.xls|Powerpoint Templates (*.pot)|*.pot"

                                REM SW cope with office versions
                                If bc_cs_central_settings.userOfficeStatus = 2 Then
                                    view.uxSaveFileDlg.Filter = view.uxSaveFileDlg.Filter + "|Word 2007 Macro-Enabled Templates (*.dotm)|*.dotm|Excel 2007 Macro-Enabled Files (*.xlsm)|*.xlsm|Powerpoint 2007 Macro-Enabled Templates (*.potm)|*.potm"
                                    view.uxSaveFileDlg.FilterIndex = 4
                                End If

                                If view.uxSaveFileDlg.ShowDialog() = DialogResult.OK Then
                                    Dim templateManagement As New bc_am_template(view.uxSaveFileDlg.FileName)
                                    Dim name As String = ""
                                    If templateManagement.Register(True, name, bc_cs_central_settings.local_template_path) Then
                                        loadTemplates()
                                        setSelectedItem(name)
                                    End If
                                End If
                            Case register_icon
                                ' set title and default path
                                view.uxOpenFileDlg.Title = "Register"
                                view.uxOpenFileDlg.InitialDirectory = bc_cs_central_settings.local_template_path
                                view.uxOpenFileDlg.FileName = ""
                                view.uxOpenFileDlg.Filter = "Word Templates (*.dot)|*.dot|Excel Files (*.xls)|*.xls|Powerpoint Templates (*.pot)|*.pot"
                                view.uxOpenFileDlg.FilterIndex = 0

                                REM SW cope with office versions
                                If bc_cs_central_settings.userOfficeStatus = 2 Then
                                    view.uxOpenFileDlg.Filter = view.uxOpenFileDlg.Filter + "|Word 2007 Macro-Enabled Templates (*.dotm)|*.dotm|Excel 2007 Macro-Enabled Files (*.xlsm)|*.xlsm|Powerpoint 2007 Macro-Enabled Templates (*.potm)|*.potm"
                                    view.uxOpenFileDlg.FilterIndex = 4
                                End If

                                If view.uxOpenFileDlg.ShowDialog() = DialogResult.OK Then
                                    Dim templateManagement As New bc_am_template(view.uxOpenFileDlg.FileName)
                                    Dim name As String = ""
                                    If templateManagement.Register(False, name, String.Concat(System.IO.Path.GetDirectoryName(view.uxOpenFileDlg.FileName), "\")) Then
                                        loadTemplates()
                                        setSelectedItem(name)
                                    End If
                                End If
                            Case open_icon
                                Dim templateManagement As New bc_am_template(bc_am_load_objects.obc_templates.template(view.uxGenericList.SelectedItems(0).Tag).filename)
                                templateManagement.Open(bc_am_load_objects.obc_templates.template(view.uxGenericList.SelectedItems(0).Tag).id)
                            Case amend_icon
                                Dim viewEditTemplateDets As New bc_am_bp_edit_template_details
                                Dim templateManagement As New bc_am_template(viewEditTemplateDets, bc_am_load_objects.obc_templates.template(view.uxGenericList.SelectedItems(0).Tag).filename)
                                Dim name As String = ""
                                If templateManagement.Edit(view.uxGenericList.SelectedItems(0).Tag, name) Then
                                    loadTemplates()
                                    setSelectedItem(name)
                                End If
                            Case delete_icon
                                If MessageBox.Show("Are you sure you wish to un-register this template?", "Un-Register", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                                    Dim templateManagement As New bc_am_template(bc_am_load_objects.obc_templates.template(view.uxGenericList.SelectedItems(0).Tag).filename)
                                    templateManagement.Delete(view.uxGenericList.SelectedItems(0).Tag)
                                    loadTemplates()
                                    setSelectedItem()
                                End If
                            Case clone_icon
                                Dim viewClone As New bc_am_bp_clone
                                Dim templateManagement As New bc_am_template(viewClone, bc_am_load_objects.obc_templates.template(view.uxGenericList.SelectedItems(0).Tag).filename)
                                viewClone.Text = "Clone from: " + bc_am_load_objects.obc_templates.template(view.uxGenericList.SelectedItems(0).Tag).name
                                Dim name As String = ""
                                If templateManagement.Clone(bc_am_load_objects.obc_templates.template(view.uxGenericList.SelectedItems(0).Tag).id, name) Then
                                    loadTemplates()
                                    setSelectedItem(name)
                                End If
                            Case export_icon
                                ' set title and default path
                                view.uxSaveFileDlg.Title = "Export"
                                view.uxSaveFileDlg.InitialDirectory = bc_cs_central_settings.local_template_path
                                view.uxSaveFileDlg.FileName = ""
                                view.uxSaveFileDlg.Filter = "XML Files (*.xml)|*.xml"
                                If view.uxSaveFileDlg.ShowDialog() = DialogResult.OK Then
                                    Dim templateManagement As New bc_am_template(view.uxSaveFileDlg.FileName)
                                    'If templateManagement.Export(view.uxGenericList.SelectedItems(0).Tag, String.Concat(Environment.CurrentDirectory, "\")) Then
                                    If templateManagement.Export(view.uxGenericList.SelectedItems(0).Tag, String.Concat(view.uxSaveFileDlg.InitialDirectory, "\")) Then
                                        If MessageBox.Show("Do you wish to copy the physical template to the new location?", "Copy Template", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                                            Dim copyBtn As New ToolBarButton
                                            copyBtn.ImageIndex = copy_icon
                                            ActionToolbar(copyBtn)
                                        End If
                                        Dim omessage As New bc_cs_message("BluePrint - Blue Curve", "Export Completed.", bc_cs_message.MESSAGE)
                                    End If
                                End If
                            Case copy_icon
                                If MessageBox.Show("You are about to copy the template to the server.  Are you sure?", "Copy Template", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                                        Dim templateManagement As New bc_am_template(bc_am_load_objects.obc_templates.template(view.uxGenericList.SelectedItems(0).Tag).filename)
                                        If templateManagement.Copy("") Then
                                            Dim omessage As New bc_cs_message("BluePrint - Blue Curve", "Copy Completed.", bc_cs_message.MESSAGE)
                                        End If
                                    Else
                                        Dim templateManagement As New bc_am_template(bc_am_load_objects.obc_templates.template(view.uxGenericList.SelectedItems(0).Tag).filename)
                                        If templateManagement.Copy(bc_cs_central_settings.central_template_path) Then
                                            Dim omessage As New bc_cs_message("BluePrint - Blue Curve", "Copy Completed.", bc_cs_message.MESSAGE)
                                        End If
                                    End If
                                End If
                            Case import_icon
                                ' set title and default path
                                view.uxOpenFileDlg.Title = "Import"
                                view.uxOpenFileDlg.InitialDirectory = bc_cs_central_settings.local_template_path
                                view.uxOpenFileDlg.FileName = ""
                                view.uxOpenFileDlg.Filter = "XML Files (*.xml)|*.xml"
                                If view.uxOpenFileDlg.ShowDialog() = DialogResult.OK Then
                                    Dim templateManagement As New bc_am_template(view.uxOpenFileDlg.FileName)
                                    Dim templateName As String = ""
                                    'If templateManagement.Import(templateName, String.Concat(Environment.CurrentDirectory, "\")) Then
                                    If templateManagement.Import(templateName, String.Concat(view.uxOpenFileDlg.InitialDirectory, "\")) Then
                                        loadTemplates()
                                        setSelectedItem(templateName)
                                        Dim omessage As New bc_cs_message("BluePrint - Blue Curve", "Import Completed.", bc_cs_message.MESSAGE)
                                    End If
                                End If
                        End Select
                    Else
                        Select Case btn.ImageIndex
                            Case register_icon
                                ' set title and default path
                                view.uxOpenFileDlg.Title = "Register"
                                view.uxOpenFileDlg.InitialDirectory = bc_cs_central_settings.local_template_path
                                view.uxOpenFileDlg.FileName = ""
                                view.uxOpenFileDlg.Filter = "HTML Templates(*.html)|*.html"
                                view.uxOpenFileDlg.FilterIndex = 0
                                If view.uxOpenFileDlg.ShowDialog() = DialogResult.OK Then
                                    Dim et As New bc_om_email_template
                                    Dim fullpath As String
                                    fullpath = view.uxOpenFileDlg.FileName

                                    et.write_mode = bc_om_email_template.EWRITE_MODE.INSERT
                                    et.html_filename = fullpath.Substring(InStrRev(fullpath, "\"), fullpath.Length - InStrRev(fullpath, "\"))
                                    For i = 0 To email_templates.email_templates.Count - 1
                                        If LCase(email_templates.email_templates(i).html_filename) = LCase(et.html_filename) Then
                                            MessageBox.Show("Template Already Exists!", "Register", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                            Exit Sub
                                        End If
                                    Next


                                    Dim fs As New bc_cs_file_transfer_services
                                    If fs.write_document_to_bytestream(view.uxOpenFileDlg.FileName, et.file, Nothing, False) = False Then
                                        MessageBox.Show("Cannot access file " + view.uxOpenFileDlg.FileName + ". Please check it isnt  open.", "Register", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                        Exit Sub
                                    End If

                                    REM upload

                                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                                        et.db_write()
                                    Else
                                        et.tmode = bc_cs_soap_base_class.tWRITE
                                        If et.transmit_to_server_and_receive(et, True) = False Then
                                            Exit Sub
                                        End If
                                    End If
                                    If et.err_text <> "" Then
                                        MessageBox.Show("Error Registering Template: " + et.err_text, "Register", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                    Else
                                        MessageBox.Show("Template Registered and Uploaded to Server.", "Upload", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                        loadEmailTemplates()
                                    End If
                                End If

                            Case delete_icon
                                If MessageBox.Show("Are you sure you wish to remove this email template?", "Remove", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                                    Dim et As New bc_om_email_template
                                    et = email_templates.email_templates(view.uxGenericList.SelectedItems(0).Index)
                                    et.write_mode = bc_om_email_template.EWRITE_MODE.DELETE
                                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                                        et.db_write()
                                    Else
                                        et.tmode = bc_cs_soap_base_class.tWRITE
                                        If et.transmit_to_server_and_receive(et, True) = False Then
                                            Exit Sub
                                        End If
                                    End If
                                    If et.err_text <> "" Then
                                        MessageBox.Show("Error Deleting Template: " + et.err_text, "Delete", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                    Else
                                        loadEmailTemplates()
                                    End If
                                End If
                            Case clone_icon
                                Dim viewClone As New bc_am_bp_clone
                                viewClone.uxOpenNow.Visible = False
                                viewClone.uxCreatePhysicalFile.Visible = False
                                Dim templateManagement As New bc_am_template(viewClone, email_templates.email_templates(view.uxGenericList.SelectedItems(0).Index).html_filename)


                                viewClone.Text = "Clone from: " + email_templates.email_templates(view.uxGenericList.SelectedItems(0).Index).html_filename

                                Dim name As String = ""
                                If viewClone.ShowDialog() = DialogResult.OK Then

                                    'check it ends in html

                                    'If Len(viewClone.uxTemplateName.Text) < 6 Then
                                    '    MessageBox.Show("Template Name Must end in .html", "Clone", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                    '    Exit Sub
                                    'End If
                                    'If LCase(Right(viewClone.uxTemplateName.Text, 5)) <> ".html" Then
                                    '    MessageBox.Show("Template Name Must end in .html", "Clone", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                    '    Exit Sub
                                    'End If

                                    'check template does not already exist
                                    For i = 0 To email_templates.email_templates.Count - 1
                                        If LCase(email_templates.email_templates(i).html_filename) = LCase(viewClone.uxTemplateName.Text) + ".html" Then
                                            MessageBox.Show("Clone Template Name Already Exists!", "Clone", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                            Exit Sub
                                        End If
                                    Next

                                    Dim et As New bc_om_email_template
                                    et = email_templates.email_templates(view.uxGenericList.SelectedItems(0).Index)
                                    et.clone_html_filename = viewClone.uxTemplateName.Text + ".html"
                                    et.write_mode = bc_om_email_template.EWRITE_MODE.CLONE
                                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                                        et.db_write()
                                    Else
                                        et.tmode = bc_cs_soap_base_class.tWRITE
                                        If et.transmit_to_server_and_receive(et, True) = False Then
                                            Exit Sub
                                        End If
                                    End If
                                    If et.err_text <> "" Then
                                        MessageBox.Show("Error Cloning Template: " + et.err_text, "Clone", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                    End If
                                    loadEmailTemplates()
                                End If
                            Case open_icon
                                REM download
                                Dim et As New bc_om_email_template
                                et = email_templates.email_templates(view.uxGenericList.SelectedItems(0).Index)
                                Dim fs As New bc_cs_file_transfer_services
                                If fs.check_document_exists(bc_cs_central_settings.local_repos_path + et.html_filename, Nothing) = True Then
                                    If MessageBox.Show("Template is already downloaded do you want to overwrite wit server copy?" + et.err_text, "Download", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = DialogResult.Cancel Then
                                        Exit Sub
                                    End If
                                End If

                                et.write_mode = bc_om_email_template.EWRITE_MODE.DOWNLOAD
                                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                                    et.db_write()
                                Else
                                    et.tmode = bc_cs_soap_base_class.tWRITE
                                    If et.transmit_to_server_and_receive(et, True) = False Then
                                        Exit Sub
                                    End If
                                End If
                                If et.err_text <> "" Then
                                    MessageBox.Show("Error Downloading Template: " + et.err_text, "Download", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                Else

                                    If fs.write_bytestream_to_document(bc_cs_central_settings.local_repos_path + et.html_filename, et.file, Nothing, False) = True Then
                                        MessageBox.Show("Html Template Downloaded To " + bc_cs_central_settings.local_repos_path + et.html_filename + ". Please open and edit from here", "Download", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                        loadEmailTemplates()
                                    Else
                                        MessageBox.Show("Error Downloading Template: Cannot Write file " + bc_cs_central_settings.local_repos_path + et.html_filename, "Download", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                    End If
                                End If
                            Case copy_icon
                                Dim et As New bc_om_email_template
                                et = email_templates.email_templates(view.uxGenericList.SelectedItems(0).Index)
                                et.write_mode = bc_om_email_template.EWRITE_MODE.UPLOAD
                                Dim fs As New bc_cs_file_transfer_services
                                If fs.write_document_to_bytestream(bc_cs_central_settings.local_repos_path + et.html_filename, et.file, Nothing, False) = False Then
                                    MessageBox.Show("Cannot access file " + bc_cs_central_settings.local_repos_path + et.html_filename + ". Please check it isnt already uploaded or open.", "Upload", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                    Exit Sub
                                End If

                                REM upload

                                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                                    et.db_write()
                                Else
                                    et.tmode = bc_cs_soap_base_class.tWRITE
                                    If et.transmit_to_server_and_receive(et, True) = False Then
                                        Exit Sub
                                    End If
                                End If
                                If et.err_text <> "" Then
                                    MessageBox.Show("Error Uploading Template: " + et.err_text, "Download", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                Else
                                    fs.delete_file(bc_cs_central_settings.local_repos_path + et.html_filename)
                                    MessageBox.Show("Template Uploaded to Server. Local Copy has been deleted.", "Upload", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                    loadEmailTemplates()
                                End If
                        End Select
                    End If
                Case components_icon
                    If view.uxcreate.Checked = True Then
                        Select Case btn.ImageIndex
                            Case create_new_icon
                                Dim componentView As New bc_am_bp_component
                                Dim componentMgmt As New bc_am_component(componentView)
                                Dim name As String = ""
                                If componentMgmt.Show(name) Then
                                    LoadComponents(view)
                                    setSelectedItem(name)
                                End If
                            Case amend_icon
                                Dim componentView As New bc_am_bp_component
                                Dim componentMgmt As New bc_am_component(componentView)
                                Dim name As String = ""
                                If componentMgmt.Show(view.uxGenericList.SelectedItems(0).Tag, name) Then
                                    LoadComponents(view)
                                    setSelectedItem(name)
                                End If
                            Case delete_icon
                                Dim componentMgmt As New bc_am_component
                                If MessageBox.Show("Are you sure you wish to delete this component?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                                    If componentMgmt.Delete(view.uxGenericList.SelectedItems(0).Tag) Then
                                        LoadComponents(view)
                                        setSelectedItem()
                                    End If
                                End If
                            Case parameters_icon
                                Dim parameterView As New bc_am_bp_parameters
                                Dim parameterMgmt As New bc_am_parameters(parameterView)
                                Dim name As String
                                name = view.uxGenericList.SelectedItems(0).Text
                                If parameterMgmt.Show(view.uxGenericList.SelectedItems(0).Tag) Then
                                    LoadComponents(view)
                                    setSelectedItem(name)
                                End If
                                'Sam 2.1.1 Data Model Cloning
                            Case clone_icon
                                Dim componentView As New bc_am_bp_component
                                Dim componentMgmt As New bc_am_component(componentView)
                                Dim name As String = ""
                                'If componentMgmt.CloneComponent(bc_am_load_objects.obc_templates.component_types.component_types(view.uxGenericList.SelectedItems(0).Tag).component_id) Then
                                If componentMgmt.Clone(view.uxGenericList.SelectedItems(0).Tag, name) Then
                                    LoadComponents(view)
                                    setSelectedItem(name)
                                End If
                        End Select
                    Else
                          Select btn.ImageIndex
                            Case create_new_icon
                                Dim componentView As New bc_am_bp_email_component
                                componentView.ShowDialog()
                                If componentView.ok_selected = False Then
                                    Exit Sub
                                End If
                                Dim ec As New bc_om_email_component
                                ec.desc = componentView.uxDescription.Text
                                ec.sp_name = componentView.uxStoredProcName.Text
                                ec.write_mode = bc_om_email_component.EWRITE_MODE.INSERT
                                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                                    ec.db_write()
                                Else
                                    ec.tmode = bc_cs_soap_base_class.tWRITE
                                    If ec.transmit_to_server_and_receive(ec, True) = False Then
                                        Exit Sub
                                    End If
                                End If

                                LoadEmailComponents(view)
                            Case amend_icon
                                Dim componentView As New bc_am_bp_email_component
                                Dim ec As New bc_om_email_component
                                ec = gec.components(view.uxGenericList.SelectedItems(0).Index)
                                componentView.uxDescription.Text = ec.desc
                                componentView.uxStoredProcName.Text = ec.sp_name

                                componentView.ShowDialog()
                                If componentView.ok_selected = False Then
                                    Exit Sub
                                End If

                                ec.desc = componentView.uxDescription.Text
                                ec.sp_name = componentView.uxStoredProcName.Text
                                ec.write_mode = bc_om_email_component.EWRITE_MODE.UPDATE
                                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                                    ec.db_write()
                                Else
                                    ec.tmode = bc_cs_soap_base_class.tWRITE
                                    If ec.transmit_to_server_and_receive(ec, True) = False Then
                                        Exit Sub
                                    End If
                                End If

                                LoadEmailComponents(view)
                            Case delete_icon
                                If MessageBox.Show("Are you sure you wish to delete this component?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then

                                    Dim ec As New bc_om_email_component
                                    ec.id = view.uxGenericList.SelectedItems(0).Tag
                                    ec.write_mode = bc_om_email_component.EWRITE_MODE.DELETE
                                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                                        ec.db_write()
                                    Else
                                        ec.tmode = bc_cs_soap_base_class.tWRITE
                                        If ec.transmit_to_server_and_receive(ec, True) = False Then
                                            Exit Sub
                                        End If
                                    End If

                                    LoadEmailComponents(view)

                                End If

                        End Select


                    End If
                Case publicationTypes_icon
                    Select Case btn.ImageIndex
                        Case create_new_icon
                            Dim publicationView As New bc_am_bp_publication_type
                            Dim publicationMgmt As New bc_am_publication(publicationView)
                            Dim name As String = ""
                            If publicationMgmt.Show(name) Then
                                loadPubTypes()
                                setSelectedItem(name)
                            End If
                        Case amend_icon
                            Dim publicationView As New bc_am_bp_publication_type
                            Dim publicationMgmt As New bc_am_publication(publicationView)
                            Dim name As String = ""
                            If publicationMgmt.Update(view.uxGenericList.SelectedItems(0).Tag, name) Then
                                loadPubTypes()
                                setSelectedItem(name)
                            Else
                                publicationMgmt.Remove(bc_am_load_objects.obc_pub_types.pubtype(view.uxGenericList.SelectedItems(0).Tag).id, view.uxGenericList.SelectedItems(0).Tag)
                                loadPubTypes()
                                setSelectedItem()
                            End If
                        Case delete_icon
                            If MessageBox.Show("Are you sure you wish to remove this publication?", "Remove", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                                Dim publicationMgmt As New bc_am_publication
                                publicationMgmt.Remove(bc_am_load_objects.obc_pub_types.pubtype(view.uxGenericList.SelectedItems(0).Tag).id, view.uxGenericList.SelectedItems(0).Tag)
                                loadPubTypes()
                                setSelectedItem()
                            End If
                        Case parameters_icon

                            Dim pubParamView As New bc_am_bp_pub_type_params
                            Dim pubParamMgmt As New bc_am_publication_params(pubParamView)
                            If pubParamMgmt.Show(view.uxGenericList.SelectedItems(0).Tag) Then

                            End If
                    End Select
                Case composites_icon
                    Select Case btn.ImageIndex
                        Case create_new_icon
                            Dim compositeView As New bc_am_bp_composite
                            Dim compositeMgmt As New bc_am_composite(compositeView)
                            Dim pubTypeID As Integer
                            Dim i As Integer
                            Dim name As String = ""
                            If compositeMgmt.Show(pubTypeID, name) Then
                                For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                                    If bc_am_load_objects.obc_pub_types.pubtype(i).id = pubTypeID Then
                                        bc_am_load_objects.obc_pub_types.pubtype(i).composite = True
                                    End If
                                Next
                                loadPubTypes(True)
                                setSelectedItem(name)
                            End If
                        Case amend_icon
                            Dim i As Integer
                            Dim compositeView As New bc_am_bp_composite
                            Dim compositeMgmt As New bc_am_composite(compositeView)
                            Dim name As String
                            name = view.uxGenericList.SelectedItems(0).Text
                            If Not compositeMgmt.Update(bc_am_load_objects.obc_pub_types.pubtype(view.uxGenericList.SelectedItems(0).Tag).id, view.uxGenericList.SelectedItems(0).Text) Then
                                For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                                    If bc_am_load_objects.obc_pub_types.pubtype(i).id = bc_am_load_objects.obc_pub_types.pubtype(view.uxGenericList.SelectedItems(0).Tag).id Then
                                        bc_am_load_objects.obc_pub_types.pubtype(i).composite = False
                                        Exit For
                                    End If
                                Next
                                loadPubTypes(True)
                                setSelectedItem(name)
                            End If
                        Case delete_icon
                            Dim i As Integer
                            Dim compositeMgmt As New bc_am_composite
                            If compositeMgmt.DeleteAllUnits(bc_am_load_objects.obc_pub_types.pubtype(view.uxGenericList.SelectedItems(0).Tag).id) Then
                                For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                                    If bc_am_load_objects.obc_pub_types.pubtype(i).id = bc_am_load_objects.obc_pub_types.pubtype(view.uxGenericList.SelectedItems(0).Tag).id Then
                                        bc_am_load_objects.obc_pub_types.pubtype(i).composite = False
                                        Exit For
                                    End If
                                Next
                                loadPubTypes(True)
                                setSelectedItem()
                            End If
                    End Select
                Case data_def_icon
                    Select Case btn.ImageIndex
                        Case create_new_icon
                            Dim contextView As New bc_am_bp_context
                            Dim contextMgmt As New bc_am_context(contextView)
                            Dim name As String = ""
                            contextMgmt.Show(name)
                            loadContexts(True)
                            setSelectedItem(name)
                        Case amend_icon
                            Dim contextView As New bc_am_bp_context
                            Dim contextMgmt As New bc_am_context(contextView)
                            Dim name As String
                            name = view.uxGenericList.SelectedItems(0).Text
                            contextMgmt.Update(name, view.uxGenericList.SelectedItems(0).Tag)
                            loadContexts(True)
                            setSelectedItem(name)
                        Case delete_icon
                            If MessageBox.Show("Are you sure you wish to delete this data definition?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                                Dim contextView As New bc_am_bp_context
                                Dim contextMgmt As New bc_am_context(contextView)
                                contextMgmt.DeleteContext(view.uxGenericList.SelectedItems(0).Tag, True, True)
                                loadContexts(True)
                                setSelectedItem()
                            End If
                        Case clone_icon
                            Dim contextView As New bc_am_bp_context
                            Dim contextMgmt As New bc_am_context(contextView)
                            If contextMgmt.CloneContext(view.uxGenericList.SelectedItems(0).Tag) Then
                                loadContexts(True)
                                setSelectedItem()
                            End If
                        Case export_icon
                            Dim contextView As New bc_am_bp_context
                            Dim contextMgmt As New bc_am_context(contextView)
                            contextMgmt.Export(view.uxGenericList.SelectedItems(0).Tag)
                        Case import_icon
                            Dim contextView As New bc_am_bp_context
                            Dim contextMgmt As New bc_am_context(contextView)
                            contextMgmt.Import()
                            loadContexts(True)
                            setSelectedItem()
                    End Select
            End Select

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_BluePrint", "ActionToolbar", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            view.Activate()
            view.BringToFront()
            log = New bc_cs_activity_log("bc_am_BluePrint", "ActionToolbar", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Private Sub BuildToolBar()

        Dim log = New bc_cs_activity_log("bc_am_BluePrint", "BuildToolBar", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim newIndex As Integer

            view.uxToolBarMain.Buttons.Clear()

            If view.uxNavBar.SelectedIndices.Count = 1 Then

                ' dynamically create toolbar depending on nav bar selection
                Select Case view.uxNavBar.SelectedIndices(0)

                    Case templates_icon
                        With view.uxToolBarMain.Buttons
                            newIndex = .Add("&New")
                            .Item(newIndex).ImageIndex = create_new_icon
                            .Item(newIndex).ToolTipText = "Create new template and register"
                            newIndex = .Add("&Register")
                            .Item(newIndex).ImageIndex = register_icon
                            .Item(newIndex).ToolTipText = "Register an existing template"
                            newIndex = .Add("")
                            .Item(newIndex).Style = ToolBarButtonStyle.Separator
                            newIndex = .Add("&Open")
                            .Item(newIndex).ImageIndex = open_icon
                            .Item(newIndex).ToolTipText = "Open an existing template"
                            newIndex = .Add("&Edit")
                            .Item(newIndex).ImageIndex = amend_icon
                            .Item(newIndex).ToolTipText = "Edit an existing template"
                            newIndex = .Add("&Remove")
                            .Item(newIndex).ImageIndex = delete_icon
                            .Item(newIndex).ToolTipText = "Un-register an existing template"
                            newIndex = .Add("")
                            .Item(newIndex).Style = ToolBarButtonStyle.Separator
                            newIndex = .Add("&Clone")
                            .Item(newIndex).ImageIndex = clone_icon
                            .Item(newIndex).ToolTipText = "Clone an existing template"
                            newIndex = .Add("")
                            .Item(newIndex).Style = ToolBarButtonStyle.Separator
                            newIndex = .Add("&Export")
                            .Item(newIndex).ImageIndex = export_icon
                            .Item(newIndex).ToolTipText = "Export an existing template's database configuration"
                            newIndex = .Add("&Copy")
                            .Item(newIndex).ImageIndex = copy_icon
                            .Item(newIndex).ToolTipText = "Copy a physical template file to a new location"
                            newIndex = .Add("&Import")
                            .Item(newIndex).ImageIndex = import_icon
                            .Item(newIndex).ToolTipText = "Import an existing template's database configuration"
                        End With
                    Case components_icon
                        With view.uxToolBarMain.Buttons
                            newIndex = .Add("&New")
                            .Item(newIndex).ImageIndex = create_new_icon
                            .Item(newIndex).ToolTipText = "Create a new component"
                            newIndex = .Add("&Amend")
                            .Item(newIndex).ImageIndex = amend_icon
                            .Item(newIndex).ToolTipText = "Amend an existing component"
                            newIndex = .Add("&Delete")
                            .Item(newIndex).ImageIndex = delete_icon
                            .Item(newIndex).ToolTipText = "Delete a component"
                            newIndex = .Add("")
                            .Item(newIndex).Style = ToolBarButtonStyle.Separator
                            newIndex = .Add("&Parameters")
                            .Item(newIndex).ImageIndex = parameters_icon
                            .Item(newIndex).ToolTipText = "Set parameters for an existing component"
                            newIndex = .Add("&Clone")
                            .Item(newIndex).ImageIndex = clone_icon
                            .Item(newIndex).ToolTipText = "Clone an existing component"
                        End With
                    Case publicationTypes_icon
                        With view.uxToolBarMain.Buttons
                            newIndex = .Add("&Add")
                            .Item(newIndex).ImageIndex = create_new_icon
                            .Item(newIndex).ToolTipText = "Add publication"
                            newIndex = .Add("&Configure")
                            .Item(newIndex).ImageIndex = amend_icon
                            .Item(newIndex).ToolTipText = "Configure publication"
                            newIndex = .Add("&Remove")
                            .Item(newIndex).ImageIndex = delete_icon
                            .Item(newIndex).ToolTipText = "Un - configure publication"
                            newIndex = .Add("")
                            .Item(newIndex).Style = ToolBarButtonStyle.Separator
                            newIndex = .Add("&Parameters")
                            .Item(newIndex).ImageIndex = parameters_icon
                            .Item(newIndex).ToolTipText = "Set parameters for an existing publication"
                        End With
                    Case composites_icon
                        With view.uxToolBarMain.Buttons
                            newIndex = .Add("&New")
                            .Item(newIndex).ImageIndex = create_new_icon
                            .Item(newIndex).ToolTipText = "Register a new publication as a composite"
                            newIndex = .Add("&Configure")
                            .Item(newIndex).ImageIndex = amend_icon
                            .Item(newIndex).ToolTipText = "Configure an existing composite"
                            newIndex = .Add("&Delete")
                            .Item(newIndex).ImageIndex = delete_icon
                            .Item(newIndex).ToolTipText = "Un-register an existing composite"
                        End With
                    Case data_def_icon
                        With view.uxToolBarMain.Buttons
                            newIndex = .Add("&New")
                            .Item(newIndex).ImageIndex = create_new_icon
                            .Item(newIndex).ToolTipText = "Create a new data definition"
                            newIndex = .Add("&Amend")
                            .Item(newIndex).ImageIndex = amend_icon
                            .Item(newIndex).ToolTipText = "Amend an existing data defintion"
                            newIndex = .Add("&Delete")
                            .Item(newIndex).ImageIndex = delete_icon
                            .Item(newIndex).ToolTipText = "Delete a data defintion"
                            newIndex = .Add("")
                            .Item(newIndex).Style = ToolBarButtonStyle.Separator
                            newIndex = .Add("&Clone")
                            .Item(newIndex).ImageIndex = clone_icon
                            .Item(newIndex).ToolTipText = "Clone a data defintion"
                            newIndex = .Add("")
                            .Item(newIndex).Style = ToolBarButtonStyle.Separator
                            newIndex = .Add("&Export")
                            .Item(newIndex).ImageIndex = export_icon
                            .Item(newIndex).ToolTipText = "Export an existing data definition"
                            newIndex = .Add("&Import")
                            .Item(newIndex).ImageIndex = import_icon
                            .Item(newIndex).ToolTipText = "Import an existing data defintion"
                        End With

                End Select
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_BluePrint", "BuildToolBar", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_BluePrint", "BuildToolBar", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Public Sub loadTemplates()

        Dim log = New bc_cs_activity_log("bc_am_BluePrint", "loadTemplates", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim i As Integer
            Dim lvi As ListViewItem

            view.uxGenericList.Items.Clear()

            For i = 0 To bc_am_load_objects.obc_templates.template.Count - 1
                lvi = view.uxGenericList.Items.Add(bc_am_load_objects.obc_templates.template(i).name, templates_icon)
                ' store the index for object retrieval later on
                lvi.Tag = i
            Next

            If view.uxGenericList.Items.Count = 0 Then
                Exit Try
            End If

            'sort the list
            view.lvwColumnSorter.SortColumn = 0
            view.lvwColumnSorter.Order = SortOrder.Ascending
            view.uxGenericList.Sort()
            view.uxGenericList.SelectedItems.Clear()
            Try
                If view.uxGenericList.SelectedItems.Count = 0 Then
                    EnableToolBar(False)
                Else
                    EnableToolBar(True, view.uxGenericList.SelectedItems(0))
                End If
            Catch ex As Exception


            End Try
        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_BluePrint", "loadTemplates", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_BluePrint", "loadTemplates", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    Public email_templates As New bc_om_email_templates

    Public Sub loadEmailTemplates()

        Dim log = New bc_cs_activity_log("bc_am_BluePrint", "loadEmailTemplates", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim i As Integer
            Dim lvi As ListViewItem

            view.uxGenericList.Items.Clear()
            email_templates.email_templates.Clear()

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                email_templates.db_read()
            Else
                email_templates.tmode = bc_cs_soap_base_class.tREAD
                If email_templates.transmit_to_server_and_receive(email_templates, True) = False Then
                    Exit Sub
                End If
            End If

            For i = 0 To email_templates.email_templates.Count - 1
                lvi = view.uxGenericList.Items.Add(email_templates.email_templates(i).html_filename, templates_icon)
                ' store the index for object retrieval later on
                lvi.Tag = email_templates.email_templates(i).email_template_id

            Next

            If view.uxGenericList.Items.Count = 0 Then
                Exit Try
            End If

            'sort the list
            view.lvwColumnSorter.SortColumn = 0
            view.lvwColumnSorter.Order = SortOrder.Ascending
            view.uxGenericList.Sort()
            view.uxGenericList.SelectedItems.Clear()
            Try
                If view.uxGenericList.SelectedItems.Count = 0 Then
                    EnableToolBar(False)
                Else
                    EnableToolBar(True, view.uxGenericList.SelectedItems(0))
                End If
            Catch ex As Exception


            End Try

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_BluePrint", "loadEmailTemplates", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_BluePrint", "loadEmailTemplates", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    Private Sub loadPubTypes(Optional ByVal compositesOnly As Boolean = False)

        Dim log = New bc_cs_activity_log("bc_am_BluePrint", "loadPubTypes", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim i As Integer
            Dim lvi As ListViewItem

            view.uxGenericList.Items.Clear()

            For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                If bc_am_load_objects.obc_pub_types.pubtype(i).support_doc_only = False Then
                    If (Not compositesOnly) Then
                        lvi = view.uxGenericList.Items.Add(bc_am_load_objects.obc_pub_types.pubtype(i).description, publicationTypes_icon)
                        ' store the index for object retrieval later on
                        lvi.Tag = i
                    End If
                    If (compositesOnly And bc_am_load_objects.obc_pub_types.pubtype(i).composite) Then
                        lvi = view.uxGenericList.Items.Add(bc_am_load_objects.obc_pub_types.pubtype(i).name, composites_icon)
                        ' store the index for object retrieval later on
                        lvi.Tag = i
                    End If
                End If
            Next

            If view.uxGenericList.Items.Count = 0 Then
                Exit Try
            End If

            'sort the list
            view.lvwColumnSorter.SortColumn = 0
            view.lvwColumnSorter.Order = SortOrder.Ascending
            view.uxGenericList.Sort()

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_BluePrint", "loadPubTypes", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_BluePrint", "loadPubTypes", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Private Sub loadContexts(ByVal refreshData As Boolean)

        Dim log = New bc_cs_activity_log("bc_am_BluePrint", "loadContext", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim i As Integer
            Dim lvi As ListViewItem

            view.uxGenericList.Items.Clear()

            ' populate the collection if not set or refresh required
            If contexts Is Nothing Or refreshData Then

                Dim osql As New bc_om_sql("exec bcc_core_bp_get_contexts")

                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                    osql.tmode = bc_cs_soap_base_class.tREAD
                    osql.transmit_to_server_and_receive(osql, True)
                ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    osql.db_read()
                End If

                If osql.success And Not osql.results Is Nothing Then
                    contexts = New ComboBoxHelper

                    contexts.RemoveAt(0)

                    For i = 0 To UBound(osql.results, 2)
                        contexts.Add(CInt(osql.results(0, i)), osql.results(1, i))
                    Next
                Else
                    If Not osql.success Then
                        Dim omessage As New bc_cs_message("BluePrint - Blue Curve", "Loading data definitions failed!", bc_cs_message.MESSAGE)
                    End If
                    Exit Try
                End If

            End If

            For i = 0 To contexts.Count - 1
                lvi = view.uxGenericList.Items.Add(contexts(i).Name, data_def_icon)
                lvi.Tag = contexts(i).ID
            Next

            If view.uxGenericList.Items.Count = 0 Then
                Exit Try
            End If

            'sort the list
            view.lvwColumnSorter.SortColumn = 0
            view.lvwColumnSorter.Order = SortOrder.Ascending
            view.uxGenericList.Sort()

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_BluePrint", "loadContext", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_BluePrint", "loadContext", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Friend Sub LoadComponents(ByRef currentView As Object)

        Dim log = New bc_cs_activity_log("bc_am_BluePrint", "loadComponents", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim i As Integer
            Dim lvi As ListViewItem

            currentView.uxGenericList.Items.Clear()

            For i = 0 To bc_am_load_objects.obc_templates.component_types.component_types.Count - 1
                lvi = currentView.uxGenericList.Items.Add(bc_am_load_objects.obc_templates.component_types.component_types(i).component_name, components_icon)
                ' store the index for object retrieval later on
                lvi.Tag = i
            Next

            If currentView.uxGenericList.Items.Count = 0 Then
                Exit Try
            End If

            ' sort the list
            currentView.lvwColumnSorter.SortColumn = 0
            currentView.lvwColumnSorter.Order = SortOrder.Ascending
            currentView.uxGenericList.Sort()

            Try
                If view.uxGenericList.SelectedItems.Count = 0 Then
                    EnableToolBar(False)
                Else
                    EnableToolBar(True, view.uxGenericList.SelectedItems(0))
                End If
            Catch ex As Exception


            End Try

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_BluePrint", "loadComponents", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_BluePrint", "loadComponents", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    Public gec As New bc_om_email_components
    Friend Sub LoadEmailComponents(ByRef currentView As Object)

        Dim log = New bc_cs_activity_log("bc_am_BluePrint", "loadEmailComponents", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim i As Integer
            Dim lvi As ListViewItem

            currentView.uxGenericList.Items.Clear()
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                gec.db_read()
            Else
                gec.tmode = bc_cs_soap_base_class.tREAD
                If gec.transmit_to_server_and_receive(gec, True) = False Then
                    Exit Sub
                End If
            End If
            For i = 0 To gec.components.Count - 1
                lvi = currentView.uxGenericList.Items.Add(gec.components(i).desc + " " + gec.components(i).markup_name, components_icon)
                ' store the index for object retrieval later on
                lvi.Tag = gec.components(i).id

            Next

            Try
                If view.uxGenericList.SelectedItems.Count = 0 Then
                    EnableToolBar(False)
                Else
                    EnableToolBar(True, view.uxGenericList.SelectedItems(0))
                End If
            Catch ex As Exception


            End Try
        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_BluePrint", "loadEmailComponents", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_BluePrint", "loadEMailComponents", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub


    Private Sub setSelectedItem(Optional ByVal name As String = "")

        Dim log = New bc_cs_activity_log("bc_am_BluePrint", "setSelectedItem", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim lvi As ListViewItem

            ' ensure item is selected and visible
            For Each lvi In view.uxGenericList.Items
                If lvi.Text = name Then
                    lvi.Selected = True
                    lvi.EnsureVisible()
                    Exit For
                End If
            Next

            ' if no match then select the first item in the list
            If view.uxGenericList.SelectedItems.Count = 0 And view.uxGenericList.Items.Count > 0 Then
                view.uxGenericList.Items(0).Selected = True
                view.uxGenericList.Items(0).EnsureVisible()
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_BluePrint", "setSelectedItem", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_BluePrint", "setSelectedItem", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Friend Sub EnableToolBar(ByVal enable As Boolean, Optional ByVal selectedItem As ListViewItem = Nothing)

        Dim log = New bc_cs_activity_log("bc_am_BluePrint", "EnableToolBar", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            If view.uxNavBar.SelectedIndices.Count = 1 Then
              
               
                Select Case view.uxNavBar.SelectedIndices(0)

                    Case templates_icon
                        With view.uxToolBarMain
                            .Buttons(0).Visible = True
                            .Buttons(3).Text = "Open"
                            .Buttons(4).Visible = True
                            .Buttons(9).Visible = True
                            .Buttons(10).Text = "Copy"
                            .Buttons(11).Visible = True

                        End With
                        If view.uxcreate.Checked = True Then
                            With view.uxToolBarMain

                                .Buttons(3).Enabled = enable ' open
                                .Buttons(4).Enabled = enable ' edit
                                .Buttons(5).Enabled = enable ' delete
                                .Buttons(7).Enabled = enable ' clone
                                .Buttons(9).Enabled = enable ' export
                                .Buttons(10).Enabled = enable ' copy
                            End With
                        Else
                            With view.uxToolBarMain
                                .Buttons(0).Visible = False
                                '.Buttons(3).Enabled = enable ' open
                                .Buttons(3).Enabled = enable
                                .Buttons(3).Text = "Download"
                                .Buttons(4).Visible = False

                                .Buttons(5).Enabled = enable ' delete
                                .Buttons(7).Enabled = enable ' clone
                                '.Buttons(9).Enabled = enable ' export
                                .Buttons(9).Visible = False
                                .Buttons(10).Enabled = enable ' copy
                                .Buttons(10).Text = "Upload"
                                .Buttons(11).Visible = False
                                Dim hfn As String
                                hfn = email_templates.email_templates(view.uxGenericList.SelectedItems(0).Index).html_filename
                                Dim fs As New bc_cs_file_transfer_services
                                If fs.check_document_exists(bc_cs_central_settings.local_repos_path + hfn, Nothing) = False Then
                                    .Buttons(10).Enabled = False
                                End If

                            End With
                        End If
                    Case components_icon
                        With view.uxToolBarMain
                            .Buttons(4).Visible = True
                            .Buttons(5).Visible = True

                        End With
                        If view.uxcreate.Checked = True Then

                            With view.uxToolBarMain

                                .Buttons(1).Enabled = enable ' amend
                                .Buttons(2).Enabled = enable ' delete                            
                                .Buttons(4).Enabled = enable ' parameters
                            End With
                        Else

                            With view.uxToolBarMain
                                .Buttons(1).Enabled = enable ' amend
                                .Buttons(2).Enabled = enable ' delete                            
                                .Buttons(4).Enabled = enable ' parameters
                                .Buttons(4).Visible = False
                                .Buttons(5).Visible = False
                            End With

                        End If
                    Case publicationTypes_icon
                        With view.uxToolBarMain
                            .Buttons(1).Enabled = enable ' configure
                            .Buttons(2).Enabled = enable ' remove
                            .Buttons(4).Enabled = enable ' parameters
                        End With
                    Case composites_icon
                        With view.uxToolBarMain
                            .Buttons(1).Enabled = enable ' configure
                            .Buttons(2).Enabled = enable ' delete
                        End With
                    Case data_def_icon
                        With view.uxToolBarMain
                            .Buttons(1).Enabled = enable ' amend
                            .Buttons(2).Enabled = enable ' delete
                            .Buttons(4).Enabled = enable ' clone
                            .Buttons(6).Enabled = enable ' export
                        End With

                End Select
            Else
                Dim btn As ToolBarButton
                For Each btn In view.uxToolBarMain.Buttons
                    btn.Enabled = enable
                Next
                view.uxGenericList.Enabled = enable
            End If

        Catch ex As Exception
            'Dim errLog As New bc_cs_error_log("bc_am_BluePrint", "EnableToolBar", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_BluePrint", "EnableToolBar", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Friend Sub SetRoleUserDB(ByRef currentView As Object)

        Dim log = New bc_cs_activity_log("bc_am_BluePrint", "SetRoleUserDB", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            ' set user, role, database & server
            Dim i As Integer
            For i = 0 To bc_am_load_objects.obc_users.user.Count - 1
                With bc_am_load_objects.obc_users.user(i)
                    If (bc_cs_central_settings.show_authentication_form = 0 Or bc_cs_central_settings.show_authentication_form = 2) Then
                        If UCase(Trim(.os_user_name)) = UCase(Trim(bc_cs_central_settings.logged_on_user_name)) Then
                            currentView.uxUser.Text = "User: " + .first_name + " " + .surname
                            currentView.uxRole.Text = "Role: " + .role
                            Exit For
                        End If
                    Else
                        If UCase(Trim(.user_name)) = UCase(Trim(bc_cs_central_settings.user_name)) Then
                            currentView.uxUser.Text = "User: " + .user_name
                            currentView.uxRole.Text = "Role: " + .role
                            Exit For
                        End If
                    End If
                End With
            Next

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                currentView.uxServer.Text = "Connected via Ado: " + bc_cs_central_settings.servername
            Else
                currentView.uxServer.Text = "Connected via Soap: " + bc_cs_central_settings.soap_server
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_BluePrint", "SetRoleUserDB", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_BluePrint", "SetRoleUserDB", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try


    End Sub

    Friend Sub ValidateSubComponents()

        Dim log = New bc_cs_activity_log("bc_am_BluePrint", "ValidateSubComponents", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            If viewTempComp.uxGenericList.SelectedItems.Count = 1 Then
                viewTempComp.uxAdd.Enabled = True
            Else
                viewTempComp.uxAdd.Enabled = False
            End If

            If viewTempComp.uxGenericList.SelectedItems.Count = 1 Then
                If bc_am_load_objects.obc_templates.component_types.component_types(viewTempComp.uxGenericList.SelectedItems(0).Tag).component_id = 2 Then ' text component
                    viewTempComp.uxTextEntry.Enabled = True
                Else
                    viewTempComp.uxTextEntry.Enabled = False
                End If
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_BluePrint", "ValidateSubComponents", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_BluePrint", "ValidateSubComponents", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Friend Sub ValidateRegisteredTables()

        Dim log = New bc_cs_activity_log("bc_am_BluePrint", "ValidateRegisteredTables", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            If viewRegTables.uxGenericList.SelectedItems.Count = 1 Then
                viewRegTables.uxAdd.Enabled = True
            Else
                viewRegTables.uxAdd.Enabled = False
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_BluePrint", "ValidateRegisteredTables", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_BluePrint", "ValidateRegisteredTables", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Friend Sub ToggleRegisteredTablesView(ByVal distinctList As Boolean)
        Dim log = New bc_cs_activity_log("bc_am_BluePrint", "ToggleRegisteredTablesView", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim i As Integer
            Dim lvi As ListViewItem

            viewRegTables.uxGenericList.Items.Clear()

            If distinctList Then
                If IsArray(viewRegTables.distinctRegisteredTables) Then
                    For i = 0 To UBound(viewRegTables.distinctRegisteredTables, 2)
                        lvi = viewRegTables.uxGenericList.Items.Add(viewRegTables.distinctRegisteredTables(0, i), table_icon)
                        lvi.Tag = i ' store the index for use later
                    Next
                End If
            Else
                If IsArray(viewRegTables.allRegisteredTables) Then
                    For i = 0 To UBound(viewRegTables.allRegisteredTables, 2)
                        lvi = viewRegTables.uxGenericList.Items.Add(String.Concat(viewRegTables.allRegisteredTables(0, i), _
                                                                                    " in template: ", _
                                                                                    viewRegTables.allRegisteredTables(1, i)), _
                                                                                    table_icon)
                        lvi.Tag = i ' store the index for use later
                    Next
                End If
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_BluePrint", "ToggleRegisteredTablesView", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            bc_cs_central_settings.progress_bar.unload()
            log = New bc_cs_activity_log("bc_am_BluePrint", "ToggleRegisteredTablesView", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Friend Sub Sync()

        Dim log = New bc_cs_activity_log("bc_am_BluePrint", "Sync", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim osql As New bc_om_sql(String.Concat("bcc_core_bp_set_user_to_sync ", bc_cs_central_settings.logged_on_user_id, ", ", 2))
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                osql.tmode = bc_cs_soap_base_class.tREAD
                osql.transmit_to_server_and_receive(osql, True)
            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                osql.db_read()
            End If

            If osql.success = True Then
                bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve", "synchronizing...", 10, False, True)
                Dim osync As New bc_am_synchronize
                REM FIL JULY 2013
                REM now have partial sync so just sync publications and files
                Dim otemplates As New bc_om_templates
                osync.sync_files(otemplates)
                osync.sync_publications(otemplates)


                osql = New bc_om_sql(String.Concat("bcc_core_bp_set_user_to_sync ", bc_cs_central_settings.logged_on_user_id, ", ", 0))
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                    osql.tmode = bc_cs_soap_base_class.tREAD
                    osql.transmit_to_server_and_receive(osql, True)
                ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    osql.db_read()
                End If
                If view.uxNavBar.SelectedIndices(0) = templates_icon Then
                    loadTemplates()
                Else
                    view.uxNavBar.Items(0).Selected = True
                End If
                setSelectedItem()
            Else
                Dim omessage As New bc_cs_message("BluePrint - Blue Curve", "Synchronize failed.", bc_cs_message.MESSAGE)
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_BluePrint", "Sync", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            bc_cs_central_settings.progress_bar.unload()
            log = New bc_cs_activity_log("bc_am_BluePrint", "Sync", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Friend Sub ValidateDefinitionList(ByVal lv As ListView)

        Dim log = New bc_cs_activity_log("bc_am_BluePrint", "ValidateDefinitionList", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try

            If lv.Name = "uxDbDefinitionList" Then
                If viewTempAdvanced.uxDbDefinitionList.SelectedItems.Count = 1 Then
                    viewTempAdvanced.uxTempDefinitionList.SelectedItems.Clear()
                    viewTempAdvanced.uxAdd.Enabled = True
                    viewTempAdvanced.uxDelete.Enabled = True
                Else
                    viewTempAdvanced.uxAdd.Enabled = False
                    viewTempAdvanced.uxDelete.Enabled = False
                End If

            ElseIf lv.Name = "uxTempDefinitionList" Then
                If viewTempAdvanced.uxTempDefinitionList.SelectedItems.Count = 1 Then
                    viewTempAdvanced.uxDbDefinitionList.SelectedItems.Clear()
                    viewTempAdvanced.uxAdd.Enabled = True
                    viewTempAdvanced.uxDelete.Enabled = True
                Else
                    viewTempAdvanced.uxAdd.Enabled = False
                    viewTempAdvanced.uxDelete.Enabled = False
                End If
            Else
                viewTempAdvanced.uxAdd.Enabled = False
                viewTempAdvanced.uxDelete.Enabled = False
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_BluePrint", "ValidateDefinitionList", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_BluePrint", "ValidateDefinitionList", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Friend Sub AddTempOrDbBookmark()

        Dim log = New bc_cs_activity_log("bc_am_BluePrint", "AddTempOrDbBookmark", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            If viewTempAdvanced.uxDbDefinitionList.SelectedItems.Count = 1 Then
                If viewTempAdvanced.uxDbDefinitionList.SelectedItems.Item(0).BackColor = Drawing.Color.Red Then
                    MsgBox("Add bookmark for " + viewTempAdvanced.uxDbDefinitionList.Name + " red item is selected")
                Else
                    MsgBox("Add bookmark for " + viewTempAdvanced.uxDbDefinitionList.Name + " there is already an item on the other list, you cannot add!")
                End If

            ElseIf viewTempAdvanced.uxTempDefinitionList.SelectedItems.Count = 1 Then
                If viewTempAdvanced.uxTempDefinitionList.SelectedItems.Item(0).BackColor = Drawing.Color.Red Then
                    MsgBox("Add bookmark for " + viewTempAdvanced.uxTempDefinitionList.Name + " red item is selected")

                    Dim sql As String
                    sql = "exec bcc_core_bp_advanced_add_db_bookmark " + viewTempAdvanced.uxTempDefinitionList.Tag + ", '" + _ 
                    viewTempAdvanced.uxTempDefinitionList.SelectedItems.Item(0).Text(+"', '" + _
                    viewTempAdvanced.uxTempDefinitionList.SelectedItems.Item(0).SubItems(1).Text + "', " + _
                    viewTempAdvanced.uxTempDefinitionList.SelectedItems.Item(0).SubItems(2).Text + "," + _
                    viewTempAdvanced.uxTempDefinitionList.SelectedItems.Item(0).SubItems(3).Text + ", '" + _
                    viewTempAdvanced.uxTempDefinitionList.SelectedItems.Item(0).SubItems(4).Text + "'")

                    Dim osql As New bc_om_sql(sql.ToString)

                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                        osql.tmode = bc_cs_soap_base_class.tREAD
                        osql.transmit_to_server_and_receive(osql, True)
                    ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        osql.db_read()
                    End If

                    If osql.success Then

                        MsgBox("Success!!!")
                        'If osql.results Is Nothing Then
                        '    Dim omessage As New bc_cs_message("AddTempOrDbBookmark", "There is no registered table located at the current cursor location", bc_cs_message.MESSAGE)
                        '    Exit Try
                        'End If

                        'Dim lvi As ListViewItem

                        'For Each lvi In templateComponents.uxGenericList.Items
                        '    If bc_am_load_objects.obc_templates.component_types.component_types(lvi.Tag).component_id = osql.results(2, 0) Then
                        '        lvi.Selected = True
                        '        Exit For
                        '    End If
                        'Next

                        '' if no item in the list is selected then select the first item
                        'If templateComponents.uxGenericList.Items.Count > 0 And templateComponents.uxGenericList.SelectedItems.Count = 0 Then
                        '    templateComponents.uxGenericList.Items(0).Selected = True
                        'End If

                        '' if the component type is text then enable the text entry
                        'If osql.results(2, 0) = 2 Then
                        '    templateComponents.uxTextEntry.Enabled = True
                        '    templateComponents.uxTextEntry.Text = osql.results(1, 0)
                        'Else
                        '    templateComponents.uxTextEntry.Enabled = False
                        '    templateComponents.uxTextEntry.Text = ""
                        'End If
                    Else
                        Dim omessage As New bc_cs_message("Advanced Add Bookmark", "The bookmark insert was not successful.", bc_cs_message.MESSAGE)
                        Exit Try
                    End If

                Else
                    MsgBox("Add bookmark for " + viewTempAdvanced.uxTempDefinitionList.Name + " there is already an item on the other list, you cannot add!")
                End If

            Else
                MsgBox("No bookmark selected.")
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_BluePrint", "AddTempOrDbBookmark", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_BluePrint", "AddTempOrDbBookmark", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Friend Sub DeleteTempOrDbBookmark()

        Dim log = New bc_cs_activity_log("bc_am_BluePrint", "DeleteTempOrDbBookmark", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            If viewTempAdvanced.uxDbDefinitionList.SelectedItems.Count = 1 Then
                If viewTempAdvanced.uxDbDefinitionList.SelectedItems.Item(0).BackColor = Drawing.Color.Red Then
                    MsgBox("Delete bookmark for " + viewTempAdvanced.uxDbDefinitionList.Name + " red item is selected.  subcomponentid: " + viewTempAdvanced.uxDbDefinitionList.SelectedItems.Item(0).Tag.ToString)

                    Dim sql As String 'Builder
                    'With sql
                    '.Append("exec bcc_core_bp_advanced_delete_db_bookmark ")
                    '.Append(viewTempAdvanced.uxDbDefinitionList.SelectedItems.Item(0).Tag)
                    sql = "exec bcc_core_bp_advanced_delete_db_bookmark '" + viewTempAdvanced.uxDbDefinitionList.SelectedItems.Item(0).Tag.ToString + "'"
                    'End With

                    Dim osql As New bc_om_sql(sql.ToString)

                    If MessageBox.Show("Are you sure you wish to delete the bookmark from database?  This process is irreversable.", "Delete Bookmark", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then

                        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                            osql.tmode = bc_cs_soap_base_class.tREAD
                            osql.transmit_to_server_and_receive(osql, True)
                        ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                            osql.db_read()
                        End If

                        'If osql.success Then
                        '    MsgBox("Success!!!")
                        'Else
                        '    Dim omessage As New bc_cs_message("Advanced Delete DB Bookmark", "Deleting DB bookmark was not successful.", bc_cs_message.MESSAGE)
                        '    Exit Try
                        'End If

                    End If

                Else
                    MsgBox("Delete bookmark for " + viewTempAdvanced.uxDbDefinitionList.Name + " there is a correspeonding item on the other list, deleting this bookmark will create a mismatch.  Are you sure you want to continue?")
                End If

            ElseIf viewTempAdvanced.uxTempDefinitionList.SelectedItems.Count = 1 Then
                If viewTempAdvanced.uxTempDefinitionList.SelectedItems.Item(0).BackColor = Drawing.Color.Red Then
                    MsgBox("Delete bookmark for " + viewTempAdvanced.uxTempDefinitionList.Name + " red item is selected")
                Else
                    MsgBox("Delete bookmark for " + viewTempAdvanced.uxTempDefinitionList.Name + " there is a correspeonding item on the other list, deleting this bookmark will create a mismatch.  Are you sure you want to continue?")
                End If
            Else
                MsgBox("No bookmark selected.")
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_BluePrint", "DeleteTempOrDbBookmark", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_BluePrint", "DeleteTempOrDbBookmark", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class


