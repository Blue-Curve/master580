Imports BlueCurve.Core.AS
Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports System.Windows.Forms
Imports System.Text
'==========================================
' Bluecurve Limited 2008
' Module:         BluePrint
' Type:           AM
' Desciption:     publication config
'                 management
' Public Methods: Show
'                 Update
'                 Apply
'                 Remove
'                 Validate
'                 AssignTemplateToVariation
'                 CheckConfigurationComplete
'                 LoadVariations
'                 SaveTemplatesAgainstVariation
'                 ValidateVariations
'  
' Version:        1.0
' Change history:
'
'==========================================
Friend Class bc_am_publication

    Private view As bc_am_bp_publication_type
    Private selectorView As bc_am_bp_pub_type_selector

    Private entityClasses As Object
    Private entityVariations As New ArrayList
    Private changesApplied As Boolean
    Private selectedIndex As Integer

    Private Const template_icon As Integer = 2

    Private saveType As eSaveType
    Private Enum eSaveType As Integer
        eAdd = 1
        eEdit = 2
    End Enum

    Friend Sub New(Optional ByVal view As bc_am_bp_publication_type = Nothing)

        If Not view Is Nothing Then
            view.Controller = Me
            Me.view = view
        End If

    End Sub

    Friend Function Show(ByRef name As String) As Boolean

        Dim log = New bc_cs_activity_log("bc_am_publication", "Show", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim selectorView As New bc_am_bp_pub_type_selector

            changesApplied = False

            view.uxVariations.Enabled = False
            view.uxApply.Enabled = False

            ' retrieve un-configured publication types for selection
            Dim osql As New bc_om_sql("exec bcc_core_bp_get_publication_Types")

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                osql.tmode = bc_cs_soap_base_class.tREAD
                osql.transmit_to_server_and_receive(osql, True)
            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                osql.db_read()
            End If

            If osql.success = True Then
                Dim i As Integer
                Dim j As Integer
                Dim cbi As New ComboBoxHelper

                ' check pub type is not already configured
                For i = 0 To UBound(osql.results, 2)
                    If bc_am_load_objects.obc_pub_types.pubtype.Count = 0 Then
                        cbi.Add(CType(osql.results(0, i), Integer), osql.results(1, i))
                    Else
                        For j = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                            If bc_am_load_objects.obc_pub_types.pubtype(j).name = osql.results(1, i) Then
                                Exit For
                            End If

                            If j = bc_am_load_objects.obc_pub_types.pubtype.Count - 1 Then
                                cbi.Add(CType(osql.results(0, i), Integer), osql.results(1, i))
                            End If
                        Next
                    End If

                Next

                selectorView.uxPubType.DisplayMember = "Name"
                selectorView.uxPubType.ValueMember = "ID"
                selectorView.uxPubType.DataSource = cbi

                selectorView.uxPubType.SelectedIndex = 0

                selectorView.uxOK.Enabled = False

                saveType = eSaveType.eAdd

                If selectorView.ShowDialog() = DialogResult.OK Then

                    view.Text = selectorView.uxPubType.Text ' pub type name
                    view.uxPublicationType.Text = selectorView.uxPubType.Text
                    view.Tag = selectorView.uxPubType.SelectedValue ' pub type id                    

                    loadLanguages()
                    loadWorkflowStages()
                    loadEntityClassesAndParents()

                    view.ShowDialog()

                    name = view.uxDescription.Text
                    Show = changesApplied
                End If

            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_publication", "Show", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_publication", "Show", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

    Friend Function Update(ByVal index As Integer, ByRef name As String) As Boolean

        Dim log = New bc_cs_activity_log("bc_am_publication", "Update", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim i As Integer
            Dim lvi As ListViewItem

            view.Text = bc_am_load_objects.obc_pub_types.pubtype(index).name ' pub type name    
            view.uxPublicationType.Text = bc_am_load_objects.obc_pub_types.pubtype(index).name
            view.Tag = bc_am_load_objects.obc_pub_types.pubtype(index).id ' pub type id

            loadLanguages()
            loadWorkflowStages()
            loadEntityClassesAndParents()

            ' update form values
            view.uxDescription.Text = bc_am_load_objects.obc_pub_types.pubtype(index).description
            view.uxLanguage.SelectedValue = bc_am_load_objects.obc_pub_types.pubtype(index).language
            view.uxWorkflowStage.SelectedValue = CType(bc_am_load_objects.obc_pub_types.pubtype(index).default_financial_workflow_stage, Integer)
            view.uxCategory.SelectedValue = CType(bc_am_load_objects.obc_pub_types.pubtype(index).child_category, Integer)
            view.uxVariation.SelectedValue = CType(bc_am_load_objects.obc_pub_types.pubtype(index).parent_category, Integer)
            view.uxSecondaryVariation.SelectedValue = CType(bc_am_load_objects.obc_pub_types.pubtype(index).parent_category2, Integer)

            loadTemplates()

            ' load variations
            For i = 0 To bc_am_load_objects.obc_pub_types.pubtype(index).products.product.count - 1
                lvi = view.uxProducts.Items.Add(bc_am_load_objects.obc_pub_types.pubtype(index).products.product(i).name, template_icon)
                lvi.SubItems.Add(getTemplateNameForID(bc_am_load_objects.obc_pub_types.pubtype(index).products.product(i).template_id))
                lvi.Tag = bc_am_load_objects.obc_pub_types.pubtype(index).products.product(i).id
            Next

            saveType = eSaveType.eEdit
            selectedIndex = index

            view.uxApply.Enabled = False
            loadEmailTemplates()

            view.ShowDialog()

            name = view.uxDescription.Text

            Update = changesApplied

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_publication", "Update", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_publication", "Update", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

    Friend Sub Remove(ByVal pubTypeID As Integer, Optional ByVal index As Integer = -1)

        Dim log = New bc_cs_activity_log("bc_am_publication", "Remove", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim osql As New bc_om_sql(String.Concat("exec bcc_core_bp_remove_publication_config ", pubTypeID))

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                osql.tmode = bc_cs_soap_base_class.tREAD
                osql.transmit_to_server_and_receive(osql, True)
            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                osql.db_read()
            End If

            If osql.success = True Then
                If index <> -1 Then
                    bc_am_load_objects.obc_pub_types.Remove(index)
                End If
            Else
                Dim errLog As New bc_cs_error_log("bc_am_publication", "Remove", bc_cs_error_codes.USER_DEFINED, "Failed to delete publication configuration")
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_publication", "Remove", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_publication", "Remove", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Private Sub loadLanguages()

        Dim log = New bc_cs_activity_log("bc_am_publication", "loadLanguages", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim i As Integer

            Dim cbi As New ComboBoxHelper

            For i = 0 To bc_am_load_objects.obc_pub_types.languages.Count - 1
                cbi.Add(CType(bc_am_load_objects.obc_pub_types.languages(i).id, Integer), bc_am_load_objects.obc_pub_types.languages(i).name)
            Next

            view.uxLanguage.DisplayMember = "Name"
            view.uxLanguage.ValueMember = "ID"
            view.uxLanguage.DataSource = cbi

            view.uxLanguage.SelectedIndex = 0

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_publication", "loadLanguages", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_publication", "loadLanguages", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub


    Private Sub loadWorkflowStages()

        Dim log = New bc_cs_activity_log("bc_am_publication", "loadWorkflowStages", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim cbi As New ComboBoxHelper

            'For i = 0 To bc_am_load_objects.obc_pub_types.stages.Count - 1
            cbi.Add(1, "Draft")
            cbi.Add(8, "Publish")
            'cbi.Add(CType(bc_am_load_objects.obc_pub_types.stages(i).id, Integer), bc_am_load_objects.obc_pub_types.stages(i).name)
            'Next

            view.uxWorkflowStage.DisplayMember = "Name"
            view.uxWorkflowStage.ValueMember = "ID"
            view.uxWorkflowStage.DataSource = cbi

            'view.uxWorkflowStage.SelectedIndex = 0

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_publication", "loadWorkflowStages", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_publication", "loadWorkflowStages", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub


    Private Sub loadEntityClassesAndParents()

        Dim log = New bc_cs_activity_log("bc_am_publication", "loadEntityClassesAndParents", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim i As Integer

            bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve", "Loading Publication Details...", 10, False, True)
            Cursor.Current = Cursors.WaitCursor

            Dim osql As New bc_om_sql("exec bcc_core_bp_get_entity_classes")

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                osql.tmode = bc_cs_soap_base_class.tREAD
                osql.transmit_to_server_and_receive(osql, True)
            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                osql.db_read()
            End If

            bc_cs_central_settings.progress_bar.increment("Loading Publication Details...")

            If osql.success = True Then

                Dim cbi As New ComboBoxHelper

                entityClasses = osql.results
                cbi.Add(0, "None")

                Dim halfWay As Integer
                halfWay = Math.Round(UBound(entityClasses, 2) / 2, 0)

                For i = 0 To UBound(entityClasses, 2)
                    cbi.Add(CType(entityClasses(0, i), Integer), entityClasses(1, i))

                    osql = New bc_om_sql(String.Concat("bcc_core_bp_get_entity_class_parent_classes ", entityClasses(0, i)))

                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                        osql.tmode = bc_cs_soap_base_class.tREAD
                        osql.transmit_to_server_and_receive(osql, True)
                    ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        osql.db_read()
                    End If

                    If i = halfWay Or i = Math.Round(halfWay / 2) Or i = Math.Round((halfWay / 2) + halfWay) Then
                        bc_cs_central_settings.progress_bar.increment("Loading Publication Details...")
                    End If

                    If osql.success = True Then
                        'store variations for later use
                        entityVariations.Add(osql.results)
                    Else
                        Dim omessage As New bc_cs_message("BluePrint - Blue Curve", "Loading categories and variations failed!", bc_cs_message.MESSAGE)
                        Exit For
                    End If

                Next

                bc_cs_central_settings.progress_bar.increment("Loading Publication Details...")

                view.uxCategory.DisplayMember = "Name"
                view.uxCategory.ValueMember = "ID"
                view.uxCategory.DataSource = cbi

                view.uxCategory.SelectedIndex = 0
            Else
                Dim omessage As New bc_cs_message("BluePrint - Blue Curve", "Loading categories and variations failed!", bc_cs_message.MESSAGE)
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_publication", "loadEntityClassesAndParents", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Cursor.Current = Cursors.Default
            bc_cs_central_settings.progress_bar.unload()
            log = New bc_cs_activity_log("bc_am_publication", "loadEntityClassesAndParents", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Friend Sub LoadVariations()

        Dim log = New bc_cs_activity_log("bc_am_publication", "LoadVariations", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim results As Object
            Dim i As Integer

            If view.uxCategory.SelectedIndex = 0 Then
                Exit Sub
            End If

            Dim cbi As New ComboBoxHelper

            If view.uxCategory.SelectedIndex = 1 Then ' none
                cbi.Add(0, "None")
            Else
                results = entityVariations(view.uxCategory.SelectedIndex - 2)
                cbi.Add(0, "None")
                cbi.Add(CType(view.uxCategory.SelectedValue, Integer), view.uxCategory.Text)

                If Not results Is Nothing Then
                    For i = 0 To UBound(results, 2)
                        cbi.Add(CType(results(0, i), Integer), results(1, i))
                    Next
                End If

            End If

            view.uxVariation.BindingContext = New BindingContext
            view.uxVariation.DisplayMember = "Name"
            view.uxVariation.ValueMember = "ID"
            view.uxVariation.DataSource = cbi

            view.uxVariation.SelectedIndex = 0

            view.uxSecondaryVariation.BindingContext = New BindingContext
            view.uxSecondaryVariation.DisplayMember = "Name"
            view.uxSecondaryVariation.ValueMember = "ID"
            view.uxSecondaryVariation.DataSource = cbi

            view.uxSecondaryVariation.SelectedIndex = 0

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_publication", "LoadVariations", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            view.Cursor = Cursors.Default
            log = New bc_cs_activity_log("bc_am_publication", "LoadVariations", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub

    Friend Function Validate(ByVal redefineProducts As Boolean) As Boolean

        Dim log = New bc_cs_activity_log("bc_am_publication", "validate", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Validate = True

            If redefineProducts Then
                saveType = eSaveType.eAdd
            End If

            If Trim(view.uxDescription.Text) = "" Then
                Validate = False
            End If

            If view.uxLanguage.SelectedIndex = 0 Then
                Validate = False
            End If

            If view.uxWorkflowStage.SelectedIndex = 0 Then
                Validate = False
            End If

            If view.uxCategory.SelectedIndex = 0 Then
                Validate = False
            End If

            If view.uxVariation.SelectedIndex = 0 Then
                Validate = False
            End If

            If view.uxSecondaryVariation.SelectedIndex = 0 Then
                Validate = False
            End If

            If view.uxVariation.SelectedIndex < 2 And view.uxSecondaryVariation.SelectedIndex > 1 Then
                Validate = False
            End If

            If Validate Then
                view.uxApply.Enabled = True
            Else
                view.uxApply.Enabled = False
            End If

        Catch ex As Exception
            Validate = False
            Dim errLog As New bc_cs_error_log("bc_am_publication", "validate", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_publication", "validate", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

    Friend Function ValidateVariations(ByVal updateGUI As Boolean) As Boolean

        Dim log = New bc_cs_activity_log("bc_am_publication", "ValidateVariations", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim lvi As ListViewItem

            ValidateVariations = True

            If Trim(view.uxTemplate.Text) <> "" And view.uxProducts.SelectedItems.Count > 0 Then
                view.uxAssign.Enabled = True
            Else
                view.uxAssign.Enabled = False
            End If

            For Each lvi In view.uxProducts.Items
                If lvi.SubItems.Count = 1 Then
                    ValidateVariations = False
                    Exit For
                End If
            Next

            If updateGUI Then
                If ValidateVariations Then
                    view.uxSaveVariations.Enabled = True
                    view.uxUndoVariations.Enabled = True
                Else
                    view.uxSaveVariations.Enabled = False
                    view.uxUndoVariations.Enabled = False
                End If
            End If

        Catch ex As Exception
            ValidateVariations = False
            Dim errLog As New bc_cs_error_log("bc_am_publication", "ValidateVariations", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_publication", "ValidateVariations", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try


    End Function


    Friend Sub Apply()

        Dim log = New bc_cs_activity_log("bc_am_publication", "Apply", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim sql As New StringBuilder

            If saveType = eSaveType.eAdd And view.uxProducts.Items.Count > 0 Then
                If MessageBox.Show("The variations will be redefined and the templates will need to be re-associated. Do you wish to proceed?", "Variations", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
                    Exit Sub
                End If
            End If

            'set the flag to indicate changes have been made
            changesApplied = True

            Cursor.Current = Cursors.WaitCursor

            With sql
                .Append("exec bcc_core_bp_configure_publication ")
                .Append(view.Tag) ' pub type id
                .Append(",'")
                .Append(view.Text.Replace("'", "''")) ' pub type name
                .Append("','")
                .Append(view.uxDescription.Text.Replace("'", "''")) ' pub type description
                .Append("',")
                .Append(view.uxLanguage.SelectedValue) ' language
                .Append(",")
                .Append(view.uxWorkflowStage.SelectedValue) ' workflow stage
                .Append(",")
                .Append(view.uxCategory.SelectedValue) ' category
                .Append(",")
                .Append(view.uxVariation.SelectedValue) ' variation
                .Append(",")
                .Append(view.uxSecondaryVariation.SelectedValue) ' 2nd variation
            End With

            Dim osql As New bc_om_sql(sql.ToString)

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                osql.tmode = bc_cs_soap_base_class.tREAD
                osql.transmit_to_server_and_receive(osql, True)
            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                osql.db_read()
            End If

            If osql.success Then
                If saveType = eSaveType.eAdd Then
                    Dim i As Integer
                    Dim lvi As ListViewItem

                    view.uxProducts.Items.Clear()

                    ' load variations
                    For i = 0 To UBound(osql.results, 2)
                        lvi = view.uxProducts.Items.Add(osql.results(1, i), template_icon)
                        lvi.Tag = osql.results(0, i)
                    Next

                    ' load templates
                    loadTemplates()

                    view.uxVariations.Enabled = True
                    view.uxApply.Enabled = False
                    view.uxProducts.Items(0).Selected = True


                Else
                    'update the object model with the changes
                    bc_am_load_objects.obc_pub_types.pubtype(selectedIndex).description = view.uxDescription.Text
                    bc_am_load_objects.obc_pub_types.pubtype(selectedIndex).language = view.uxLanguage.SelectedValue 'set the flag to indicate changes have been made                   
                    view.uxApply.Enabled = False
                End If
            Else
                Dim errLog As New bc_cs_error_log("bc_am_publication", "Apply", bc_cs_error_codes.USER_DEFINED, "Failed to save publication.")
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_publication", "Apply", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Cursor.Current = Cursors.Default
            log = New bc_cs_activity_log("bc_am_publication", "Apply", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Friend Sub AssignTemplateToVariation()

        Dim log = New bc_cs_activity_log("bc_am_publication", "AssignTemplateToVariation", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim lvi As ListViewItem

            For Each lvi In view.uxProducts.SelectedItems
                If lvi.SubItems.Count > 1 Then
                    lvi.SubItems.Remove(lvi.SubItems(1))
                End If
                lvi.SubItems.Add(view.uxTemplate.Text)
            Next

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_publication", "AssignTemplateToVariation", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_publication", "AssignTemplateToVariation", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Friend Function SaveTemplatesAgainstVariations() As Boolean

        Dim log = New bc_cs_activity_log("bc_am_publication", "SaveTemplatesAgainstVariations", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim lvi As ListViewItem
            Dim sql As New StringBuilder
            Dim valuePair As String
            Dim ids As String = ""

            Cursor.Current = Cursors.WaitCursor

            changesApplied = True

            With sql
                .Append("exec bcc_core_bp_assign_templates_products ")
                .Append(view.Tag)
                .Append(",'")
                ' build pipe delimited string of template IDs and Names
                For Each lvi In view.uxProducts.Items
                    valuePair = String.Concat(lvi.Tag, ",", getIDForTemplateName(lvi.SubItems(1).Text), "|")
                    ids = String.Concat(ids, valuePair)
                Next
                .Append(Left(ids, Len(ids) - 1))
                .Append("'")
            End With

            Dim osql As New bc_om_sql(sql.ToString)

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                osql.tmode = bc_cs_soap_base_class.tREAD
                osql.transmit_to_server_and_receive(osql, True)
            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                osql.db_read()
            End If

            If osql.success Then
                ' refresh pub types object
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                    Dim ocommentary = New bc_cs_activity_log("bc_am_publication", "SaveTemplatesAgainstVariations", bc_cs_activity_codes.COMMENTARY, "Loading Pub Types via SOAP")
                    bc_am_load_objects.obc_pub_types.tmode = bc_cs_soap_base_class.tREAD
                    bc_am_load_objects.obc_pub_types.transmit_to_server_and_receive(bc_am_load_objects.obc_pub_types, True)
                Else
                    Dim ocommentary = New bc_cs_activity_log("bc_am_publication", "SaveTemplatesAgainstVariations", bc_cs_activity_codes.COMMENTARY, "Loading Pub Types from Database")
                    bc_am_load_objects.obc_pub_types.db_read()
                End If
                view.uxSaveVariations.Enabled = False
                view.uxUndoVariations.Enabled = False
                SaveTemplatesAgainstVariations = True
            Else
                Dim omessage As New bc_cs_message("BluePrint - Blue Curve", "Updating variations failed!", bc_cs_message.MESSAGE)
                Exit Try
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_publication", "SaveTemplatesAgainstVariations", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Cursor.Current = Cursors.Default
            log = New bc_cs_activity_log("bc_am_publication", "SaveTemplatesAgainstVariations", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

    Private Function getIDForTemplateName(ByVal templateName As String) As Integer

        Dim log = New bc_cs_activity_log("bc_am_publication", "getIDForTemplateName", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim i As Integer

            For i = 0 To bc_am_load_objects.obc_templates.template.Count - 1
                If bc_am_load_objects.obc_templates.template(i).name = templateName Then
                    Return bc_am_load_objects.obc_templates.template(i).id
                End If
            Next

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_publication", "getIDForTemplateName", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_publication", "getIDForTemplateName", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function
    Public email_templates As New bc_om_email_templates
    Private Sub loadEmailTemplates()
        Try
            email_templates.email_templates.Clear()
            REM get all email templates
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                email_templates.db_read()
            Else
                email_templates.tmode = bc_cs_soap_base_class.tREAD
                If email_templates.transmit_to_server_and_receive(email_templates, True) = False Then
                    Exit Sub
                End If

            End If
            Dim et As New bc_om_email_template
            et.email_template_id = 0
            et.html_filename = "None"
            email_templates.email_templates.Insert(0, et)

            view.uxemailtemplate.Items.Clear()
            For i = 0 To email_templates.email_templates.Count - 1
                view.uxemailtemplate.Items.Add(email_templates.email_templates(i).html_filename)
            Next
            Dim ept As New bc_om_email_template_for_pub_type
            ept.pub_type_id = bc_am_load_objects.obc_pub_types.pubtype(selectedIndex).id
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                ept.db_read()
            Else
                ept.tmode = bc_cs_soap_base_class.tREAD
                If ept.transmit_to_server_and_receive(ept, True) = False Then
                    Exit Sub
                End If
            End If
           

            For i = 0 To email_templates.email_templates.Count - 1
                If email_templates.email_templates(i).email_template_id = ept.email_template_id Then
                    view.uxemailtemplate.SelectedIndex = i
                    Exit For
                End If
            Next
            



            view.uxsaveemailtemplate.Enabled = False
        Catch ex As Exception

            Dim errLog As New bc_cs_error_log("bc_am_publication", "loadEmailTemplates", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

        End Try
    End Sub
    Public Sub saveemailtemplate()

        Try
            Dim ept As New bc_om_email_template_for_pub_type
            ept.pub_type_id = bc_am_load_objects.obc_pub_types.pubtype(selectedIndex).id
            ept.email_template_id = email_templates.email_templates(view.uxemailtemplate.SelectedIndex).email_template_id
            If ept.email_template_id = 0 Then
                ept.write_mode = bc_om_email_template_for_pub_type.EWRITE_MODE.DELETE
            Else
                ept.write_mode = bc_om_email_template_for_pub_type.EWRITE_MODE.UPDATE
            End If

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                ept.db_write()
            Else
                ept.tmode = bc_cs_soap_base_class.tWRITE
                If ept.transmit_to_server_and_receive(ept, True) = False Then
                    Exit Sub
                End If
            End If

            view.uxsaveemailtemplate.Enabled = False
        Catch ex As Exception

            Dim errLog As New bc_cs_error_log("bc_am_publication", "saveemailtemplate", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

        End Try
    End Sub
    Private Sub loadTemplates()

        Dim log = New bc_cs_activity_log("bc_am_publication", "loadTemplates", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim i As Integer

            Dim cbi As New ComboBoxHelper

            For i = 0 To bc_am_load_objects.obc_templates.template.Count - 1
                cbi.Add(CType(bc_am_load_objects.obc_templates.template(i).id, Integer), bc_am_load_objects.obc_templates.template(i).name)
            Next

            cbi.Sort()

            view.uxTemplate.DisplayMember = "Name"
            view.uxTemplate.ValueMember = "ID"
            view.uxTemplate.DataSource = cbi

            view.uxTemplate.SelectedIndex = 0

            view.uxAssign.Enabled = False
            view.uxSaveVariations.Enabled = False
            view.uxUndoVariations.Enabled = False

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_publication", "loadTemplates", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_publication", "loadTemplates", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Private Function getTemplateNameForID(ByVal templateID As Integer) As String

        Dim log = New bc_cs_activity_log("bc_am_publication", "getIDForTemplateName", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim i As Integer

            getTemplateNameForID = ""

            For i = 0 To bc_am_load_objects.obc_templates.template.Count - 1
                If bc_am_load_objects.obc_templates.template(i).id = templateID Then
                    Return bc_am_load_objects.obc_templates.template(i).name
                End If
            Next

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_publication", "getIDForTemplateName", bc_cs_error_codes.USER_DEFINED, ex.Message)
            Return ""
        Finally
            log = New bc_cs_activity_log("bc_am_publication", "getIDForTemplateName", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

    Friend Function CheckConfigurationComplete() As Boolean

        CheckConfigurationComplete = True
        changesApplied = True

        If view.uxApply.Enabled Or Validate(False) = False Then
            If MessageBox.Show("Unsaved changes will be lost are you sure you wish to exit?", "Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
                CheckConfigurationComplete = False
                Exit Function
            End If
        End If

        ' need to check whether everything has been committed or not
        If (Not ValidateVariations(False)) Or view.uxSaveVariations.Enabled = True Then
            If MessageBox.Show("Configuration is not complete.  If not completed it will be removed.  Are you sure you wish to close?", _
                             "Incomplete Configuration", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
                changesApplied = False
            Else
                CheckConfigurationComplete = False
            End If
        End If

    End Function

    Friend Sub Undo()

        Dim lvi As ListViewItem
        Dim i As Integer

        loadTemplates()

        view.uxProducts.Items.Clear()

        ' load variations
        For i = 0 To bc_am_load_objects.obc_pub_types.pubtype(selectedIndex).products.product.count - 1
            lvi = view.uxProducts.Items.Add(bc_am_load_objects.obc_pub_types.pubtype(selectedIndex).products.product(i).name, template_icon)
            lvi.SubItems.Add(getTemplateNameForID(bc_am_load_objects.obc_pub_types.pubtype(selectedIndex).products.product(i).template_id))
            lvi.Tag = bc_am_load_objects.obc_pub_types.pubtype(selectedIndex).products.product(i).id
        Next

        view.uxSaveVariations.Enabled = False
        view.uxUndoVariations.Enabled = False
        view.uxAssign.Enabled = False

    End Sub

End Class

'==========================================
' Bluecurve Limited 2008
' Module:         BluePrint
' Type:           AM
' Desciption:     publication config
'                 management
' Public Methods: Show
'                 AssignClassification
'                 RemoveClassification
'                 SaveClassifications
'                 AssignReportOption
'                 RemoveReportOption
'                 SaveReportOptions
'  
' Version:        1.0
' Change history:
'
'==========================================
Friend Class bc_am_publication_params

    Private view As bc_am_bp_pub_type_params
    Private changesApplied As Boolean

    Private Const parameters_icon As Integer = 3

    Friend Sub New(Optional ByVal view As bc_am_bp_pub_type_params = Nothing)

        If Not view Is Nothing Then
            view.Controller = Me
            Me.view = view
        End If

    End Sub

    Friend Function Show(ByVal index As Integer) As Boolean

        Dim log = New bc_cs_activity_log("bc_am_bp_pub_type_params", "Show", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            changesApplied = False

            view.Text = bc_am_load_objects.obc_pub_types.pubtype(index).name
            view.Tag = bc_am_load_objects.obc_pub_types.pubtype(index).id

            loadAllCategories()
            loadClassifications(index)
            loadAllReportOptions()
            loadAssignedReportOptions(index)

            view.uxAssignCategory.Enabled = False
            view.uxAssignReportOption.Enabled = False
            view.uxRemoveCategory.Enabled = False
            view.uxRemoveReportOption.Enabled = False
            view.uxSaveCategories.Enabled = False
            view.uxSaveReportOptions.Enabled = False

            view.ShowDialog()

            If changesApplied Then
                ' refresh pub types object
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                    Dim ocommentary = New bc_cs_activity_log("bc_am_publication", "Show", bc_cs_activity_codes.COMMENTARY, "Loading Pub Types via SOAP")
                    bc_am_load_objects.obc_pub_types.tmode = bc_cs_soap_base_class.tREAD
                    bc_am_load_objects.obc_pub_types.transmit_to_server_and_receive(bc_am_load_objects.obc_pub_types, True)
                Else
                    Dim ocommentary = New bc_cs_activity_log("bc_am_publication", "Show", bc_cs_activity_codes.COMMENTARY, "Loading Pub Types from Database")
                    bc_am_load_objects.obc_pub_types.db_read()
                End If
            End If

            Show = changesApplied

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_bp_pub_type_params", "Show", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_bp_pub_type_params", "Show", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

    Private Sub loadAllCategories()

        Dim log = New bc_cs_activity_log("bc_am_bp_pub_type_params", "loadAllCategories", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim osql As New bc_om_sql("exec bcc_core_bp_get_entity_classes")

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                osql.tmode = bc_cs_soap_base_class.tREAD
                osql.transmit_to_server_and_receive(osql, True)
            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                osql.db_read()
            End If

            If osql.success = True Then
                Dim i As Integer
                Dim cbi As New ComboBoxHelper

                For i = 0 To UBound(osql.results, 2)
                    cbi.Add(CType(osql.results(0, i), Integer), osql.results(1, i))
                Next

                view.uxCategory.DataSource = cbi
                view.uxCategory.DisplayMember = "Name"
                view.uxCategory.ValueMember = "ID"

                view.uxCategory.SelectedIndex = 0
            Else
                Dim omessage As New bc_cs_message("BluePrint - Blue Curve", "Loading categories failed!", bc_cs_message.MESSAGE)
                Exit Try
            End If


        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_bp_pub_type_params", "loadAllCategories", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_bp_pub_type_params", "loadAllCategories", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Private Sub loadClassifications(ByVal index As Integer)

        Dim log = New bc_cs_activity_log("bc_am_bp_pub_type_params", "loadClassifications", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim i As Integer
            
            For i = 0 To bc_am_load_objects.obc_pub_types.pubtype(index).taxonomy.count - 1
                view.uxCategories.Items.Add(bc_am_load_objects.obc_pub_types.pubtype(index).taxonomy(i).class_name, parameters_icon)
            Next

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_bp_pub_type_params", "loadClassifications", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_bp_pub_type_params", "loadClassifications", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Friend Sub AssignClassification()

        Dim log = New bc_cs_activity_log("bc_am_bp_pub_type_params", "assignClassification", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim lvi As ListViewItem

            For Each lvi In view.uxCategories.Items
                If lvi.Text = view.uxCategory.Text Then
                    Dim omessage As New bc_cs_message("BluePrint - Blue Curve", "This category is already assigned.", bc_cs_message.MESSAGE)
                    Exit Try
                End If
            Next

            lvi = view.uxCategories.Items.Add(view.uxCategory.Text, parameters_icon)
            lvi.Tag = view.uxCategory.SelectedValue


            view.uxSaveCategories.Enabled = True

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_bp_pub_type_params", "assignClassification", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_bp_pub_type_params", "assignClassification", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Friend Sub RemoveClassification()

        Dim log = New bc_cs_activity_log("bc_am_bp_pub_type_params", "removeClassification", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim lvi As ListViewItem

            For Each lvi In view.uxCategories.SelectedItems
                lvi.Remove()
            Next

            view.uxSaveCategories.Enabled = True

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_bp_pub_type_params", "removeClassification", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_bp_pub_type_params", "removeClassification", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Friend Function SaveClassifications() As Boolean

        Dim log = New bc_cs_activity_log("bc_am_bp_pub_type_params", "SaveClassifications", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim lvi As ListViewItem
            Dim sql As New StringBuilder
            Dim ids As String = ""

            With sql
                .Append("exec bcc_core_bp_assign_pub_type_classifications ")
                .Append(view.Tag)
                .Append(",'")
                For Each lvi In view.uxCategories.Items

                    ids = String.Concat(ids, lvi.Tag, "|")
                Next
                If Len(ids) > 0 Then
                    .Append(Left(ids, Len(ids) - 1))
                End If
                .Append("'")
            End With

            Dim osql As New bc_om_sql(sql.ToString)

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                osql.tmode = bc_cs_soap_base_class.tREAD
                osql.transmit_to_server_and_receive(osql, True)
            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                osql.db_read()
            End If

            If osql.success Then
                changesApplied = True
                SaveClassifications = True
                view.uxSaveCategories.Enabled = False
            Else
                Dim omessage As New bc_cs_message("BluePrint - Blue Curve", "Updating classifications failed!", bc_cs_message.MESSAGE)
                Exit Try
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_bp_pub_type_params", "SaveClassifications", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_bp_pub_type_params", "SaveClassifications", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try


    End Function

    Private Sub loadAllReportOptions()

        Dim log = New bc_cs_activity_log("bc_am_bp_pub_type_params", "loadAllReportOptions", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim osql As New bc_om_sql("exec bcc_core_bp_get_pub_type_parameters")

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                osql.tmode = bc_cs_soap_base_class.tREAD
                osql.transmit_to_server_and_receive(osql, True)
            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                osql.db_read()
            End If

            If osql.success = True Then
                If Not osql.results Is Nothing Then
                    Dim i As Integer
                    Dim cbi As New ComboBoxHelper

                    For i = 0 To UBound(osql.results, 2)
                        cbi.Add(CType(osql.results(0, i), Integer), osql.results(1, i))
                    Next

                    view.uxReportOption.DataSource = cbi
                    view.uxReportOption.DisplayMember = "Name"
                    view.uxReportOption.ValueMember = "ID"

                    view.uxReportOption.SelectedIndex = 0
                End If
            Else
                Dim omessage As New bc_cs_message("BluePrint - Blue Curve", "Loading all report options failed!", bc_cs_message.MESSAGE)
                Exit Try
            End If


        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_bp_pub_type_params", "loadAllReportOptions", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_bp_pub_type_params", "loadAllReportOptions", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Private Sub loadAssignedReportOptions(ByVal index As Integer)

        Dim log = New bc_cs_activity_log("bc_am_bp_pub_type_params", "loadAssignedReportOptions", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim i As Integer

            For i = 0 To bc_am_load_objects.obc_pub_types.pubtype(index).parameters.parameters.count - 1
                view.uxReportOptions.Items.Add(bc_am_load_objects.obc_pub_types.pubtype(index).parameters.parameters(i).caption, parameters_icon)
            Next

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_bp_pub_type_params", "loadAssignedReportOptions", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_bp_pub_type_params", "loadAssignedReportOptions", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Friend Sub AssignReportOption()

        Dim log = New bc_cs_activity_log("bc_am_bp_pub_type_params", "AssignReportOption", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim lvi As ListViewItem

            For Each lvi In view.uxReportOptions.Items
                If lvi.Text = view.uxReportOption.Text Then
                    Dim omessage As New bc_cs_message("BluePrint - Blue Curve", "This report option is already assigned.", bc_cs_message.MESSAGE)
                    Exit Try
                End If
            Next

            lvi = view.uxReportOptions.Items.Add(view.uxReportOption.Text, parameters_icon)
            lvi.Tag = view.uxReportOption.SelectedValue

            view.uxSaveReportOptions.Enabled = True

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_bp_pub_type_params", "AssignReportOption", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_bp_pub_type_params", "AssignReportOption", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Friend Sub RemoveReportOption()

        Dim log = New bc_cs_activity_log("bc_am_bp_pub_type_params", "RemoveReportOption", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim lvi As ListViewItem

            For Each lvi In view.uxReportOptions.SelectedItems
                lvi.Remove()
            Next

            view.uxSaveReportOptions.Enabled = True

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_bp_pub_type_params", "RemoveReportOption", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_bp_pub_type_params", "RemoveReportOption", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Friend Function SaveReportOptions() As Boolean

        Dim log = New bc_cs_activity_log("bc_am_bp_pub_type_params", "SaveReportOptions", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim lvi As ListViewItem
            Dim sql As New StringBuilder
            Dim ids As String = ""

            With sql
                .Append("exec bcc_core_bp_assign_pub_type_parameters ")
                .Append(view.Tag)
                .Append(",'")
                For Each lvi In view.uxReportOptions.Items
                    ids = String.Concat(ids, lvi.Tag, "|")
                Next
                If Len(ids) > 0 Then
                    .Append(Left(ids, Len(ids) - 1))
                End If
                .Append("'")
            End With

            Dim osql As New bc_om_sql(sql.ToString)

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                osql.tmode = bc_cs_soap_base_class.tREAD
                osql.transmit_to_server_and_receive(osql, True)
            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                osql.db_read()
            End If

            If osql.success Then
                changesApplied = True
                SaveReportOptions = True
                view.uxSaveReportOptions.Enabled = False
            Else
                Dim omessage As New bc_cs_message("BluePrint - Blue Curve", "Updating report options failed!", bc_cs_message.MESSAGE)
                Exit Try
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_bp_pub_type_params", "SaveReportOptions", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_bp_pub_type_params", "SaveReportOptions", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try


    End Function

    Friend Function Validate() As Boolean

        Dim log = New bc_cs_activity_log("bc_am_bp_pub_type_params", "Validate", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            If Trim(view.uxReportOption.Text) = "" Then
                view.uxAssignReportOption.Enabled = False
            Else
                view.uxAssignReportOption.Enabled = True
            End If

            If view.uxReportOptions.SelectedItems.Count = 0 Then
                view.uxRemoveReportOption.Enabled = False
            Else
                view.uxRemoveReportOption.Enabled = True
            End If

            If Trim(view.uxCategory.Text) = "" Then
                view.uxAssignCategory.Enabled = False
            Else
                view.uxAssignCategory.Enabled = True
            End If

            If view.uxCategories.SelectedItems.Count = 0 Then
                view.uxRemoveCategory.Enabled = False
            Else
                view.uxRemoveCategory.Enabled = True
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_bp_pub_type_params", "Validate", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_bp_pub_type_params", "Validate", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

End Class
