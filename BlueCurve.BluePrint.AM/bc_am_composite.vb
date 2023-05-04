Imports BlueCurve.Core.CS
Imports BlueCurve.Core.as
Imports BlueCurve.Core.om
Imports System.Text
Imports System.Windows.Forms
'==========================================
' Bluecurve Limited 2008
' Module:         BluePrint
' Type:           AM
' Desciption:     composite management
' Public Methods: Show
'                 Save
'                 Update
'                 Reset
'                 ActionToolbar
'                 PopulateDetails
'                 ChangeOrder
'                 DeleteAllUnits
'                 Validate
'  
' Version:        1.0
' Change history:
'
'==========================================
Friend Class bc_am_composite

    Private view As bc_am_bp_composite
    Private selectorView As bc_am_bp_pub_type_selector

    Private results As Object

    Private saveType As eSaveType
    Private Enum eSaveType As Integer
        eAdd = 1
        eEdit = 2
        eOrder = 3
    End Enum

    ' icon constants
    Private Const create_new_icon As Integer = 0
    Private Const amend_icon As Integer = 1
    Private Const delete_icon As Integer = 2
    Private Const publicationTypes_icon As Integer = 3
    Private Const save_order_icon As Integer = 4
    Private Const referesh_icon As Integer = 7

    Friend Sub New(Optional ByVal view As bc_am_bp_composite = Nothing)

        If Not view Is Nothing Then
            view.Controller = Me
            Me.view = view
        End If

    End Sub

    Friend Function Show(ByRef pubTypeID As Integer, ByRef pubTypeName As String) As Boolean

        Dim log = New bc_cs_activity_log("bc_am_composite", "Show", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim i As Integer
            Dim selectorView As New bc_am_bp_pub_type_selector

            selectorView.uxPubType.Items.Clear()
            For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                If Not bc_am_load_objects.obc_pub_types.pubtype(i).composite Then
                    selectorView.uxPubType.Items.Add(bc_am_load_objects.obc_pub_types.pubtype(i))
                End If
            Next
            selectorView.uxPubType.SelectedIndex = -1

            loadWorkflowStages()

            If selectorView.ShowDialog() = DialogResult.OK Then
                ' set the form title and store the pub type id
                view.Text = selectorView.uxPubType.SelectedItem.name
                view.Tag = selectorView.uxPubType.SelectedItem.id

                view.uxPublicationType.Items.Clear()
                For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                    view.uxPublicationType.Items.Add(bc_am_load_objects.obc_pub_types.pubtype(i))
                Next
                view.uxPublicationType.SelectedIndex = -1

                view.uxPubTypes.Items.Clear()

                Reset()

                view.ShowDialog()

                If view.uxPubTypes.Items.Count > 0 Then
                    Show = True
                    pubTypeName = view.Text
                    pubTypeID = view.Tag
                End If
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_composite", "Show", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_composite", "Show", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

    Friend Function Update(ByVal pubTypeID As Integer, ByVal pubTypeName As String) As Boolean

        Dim log = New bc_cs_activity_log("bc_am_composite", "Update", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim i As Integer

            ' set the form title and store the pub type id
            view.Text = pubTypeName
            view.Tag = pubTypeID

            loadWorkflowStages()

            populateUnits()

            ' load publication types
            view.uxPublicationType.Items.Clear()
            For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                view.uxPublicationType.Items.Add(bc_am_load_objects.obc_pub_types.pubtype(i))
            Next
            view.uxPublicationType.SelectedIndex = -1

            view.ShowDialog()

            If Not view.uxPubTypes.Items.Count = 0 Then
                Update = True
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_composite", "Update", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_composite", "Update", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

    Friend Sub PopulateDetails()

        Dim log = New bc_cs_activity_log("bc_am_composite", "PopulateDetails", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim i As Integer

            For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                If bc_am_load_objects.obc_pub_types.pubtype(i).id = results(2, view.uxPubTypes.SelectedItems(0).Tag) Then
                    view.uxPublicationType.SelectedItem = bc_am_load_objects.obc_pub_types.pubtype(i)
                End If
            Next

            view.uxDescription.Text = results(1, view.uxPubTypes.SelectedItems(0).Tag)

            view.uxWorkflowStage.SelectedValue = results(5, view.uxPubTypes.SelectedItems(0).Tag)

            view.uxEdit.Enabled = True
            view.uxDelete.Enabled = True
            view.uxUp.Enabled = True
            view.uxDown.Enabled = True

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_composite", "PopulateDetails", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_composite", "PopulateDetails", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Friend Sub ActionToolbar(ByVal btn As ToolBarButton)

        Dim log = New bc_cs_activity_log("bc_am_composite", "ActionToolbar", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Select Case btn.ImageIndex
                Case create_new_icon
                    view.uxDetails.Enabled = True
                    view.uxPublicationType.Enabled = True
                    view.uxPublicationType.SelectedIndex = -1
                    view.uxWorkflowStage.SelectedIndex = 0
                    view.uxDescription.Text = ""
                    view.uxUnits.Enabled = False
                    view.uxSave.Enabled = False
                    saveType = eSaveType.eAdd
                Case amend_icon
                    view.uxDetails.Enabled = True
                    view.uxUnits.Enabled = False
                    view.uxPublicationType.Enabled = False
                    saveType = eSaveType.eEdit
                Case delete_icon
                    deleteUnit()
                Case save_order_icon
                    saveType = eSaveType.eOrder
                    Save()
                Case referesh_icon
                    populateUnits()
            End Select

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_composite", "ActionToolbar", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_composite", "ActionToolbar", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Friend Sub Reset()

        Dim log = New bc_cs_activity_log("bc_am_composite", "Reset", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            'reset form state
            view.uxAdd.Enabled = True
            view.uxUnits.Enabled = True
            view.uxDetails.Enabled = False
            view.uxPublicationType.SelectedIndex = -1
            view.uxWorkflowStage.SelectedIndex = 0
            view.uxDescription.Text = ""
            view.uxDelete.Enabled = False
            view.uxEdit.Enabled = False
            view.uxSaveOrder.Enabled = False
            view.uxRefresh.Enabled = False
            view.uxUp.Enabled = False
            view.uxDown.Enabled = False
            If view.uxPubTypes.SelectedItems.Count = 1 Then
                view.uxPubTypes.SelectedItems(0).Selected = False
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_composite", "Reset", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_composite", "Reset", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Friend Function Save() As Boolean

        Dim log = New bc_cs_activity_log("bc_am_composite", "Save", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim sql As New StringBuilder

            If validate(True) Then

                view.Cursor = Cursors.WaitCursor

                Select Case saveType
                    Case eSaveType.eAdd
                        With sql
                            .Append("exec dbo.bcc_core_bp_configure_composite ")
                            .Append(view.Tag) ' pub type id
                            .Append(",")
                            .Append("0") ' add
                            .Append(",'")
                            .Append(view.uxDescription.Text.Replace("'", "''")) ' description
                            .Append("',")
                            .Append(view.uxPublicationType.SelectedItem.id) ' sub pub type id
                            .Append(",'")
                            .Append(buildSQL(view.uxPublicationType.SelectedItem.id)) ' query string
                            .Append("',")
                            .Append(view.uxWorkflowStage.SelectedItem.id)
                        End With

                        Dim osql As New bc_om_sql(sql.ToString)
                        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                            osql.tmode = bc_cs_soap_base_class.tREAD
                            osql.transmit_to_server_and_receive(osql, True)
                        ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                            osql.db_read()
                        End If

                        If osql.success = True Then
                            Save = True
                            ' not good but ok for ado.  Will change when all offline
                            populateUnits()
                        Else
                            Save = False
                            Dim errLog As New bc_cs_error_log("bc_am_composite", "Save", bc_cs_error_codes.USER_DEFINED, "Failed to save composite")
                            Exit Try
                        End If

                    Case eSaveType.eEdit
                        With sql
                            .Append("exec dbo.bcc_core_bp_configure_composite ")
                            .Append(view.Tag) ' pub type id
                            .Append(",")
                            .Append("1") ' edit
                            .Append(",'")
                            .Append(view.uxDescription.Text.Replace("'", "''")) ' description
                            .Append("',")
                            .Append(view.uxPublicationType.SelectedItem.id) ' sub pub type id
                            .Append(",'")
                            .Append(buildSQL(view.uxPublicationType.SelectedItem.id)) ' query string
                            .Append("',")
                            .Append(view.uxWorkflowStage.SelectedItem.id)
                        End With

                        Dim osql As New bc_om_sql(sql.ToString)
                        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                            osql.tmode = bc_cs_soap_base_class.tREAD
                            osql.transmit_to_server_and_receive(osql, True)
                        ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                            osql.db_read()
                        End If

                        If osql.success = True Then
                            Save = True
                            ' not good but ok for ado.  Will change when all offline
                            populateUnits()
                        Else
                            Save = False
                            Dim errLog As New bc_cs_error_log("bc_am_composite", "Save", bc_cs_error_codes.USER_DEFINED, "Failed to save composite")
                            Exit Try
                        End If
                    Case eSaveType.eOrder
                        Dim lvi As ListViewItem
                        Dim params As String = ""
                        With sql
                            .Append("exec dbo.bcc_core_bp_update_composite_order ")
                            .Append(view.Tag) ' pub type id
                            .Append(",'")
                            For Each lvi In view.uxPubTypes.Items
                                params = String.Concat(params, results(2, lvi.Tag), "|") ' concatenated order list
                            Next
                            .Append(Left(params, Len(params) - 1))
                            .Append("'")
                        End With

                        Dim osql As New bc_om_sql(sql.ToString)
                        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                            osql.tmode = bc_cs_soap_base_class.tREAD
                            osql.transmit_to_server_and_receive(osql, True)
                        ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                            osql.db_read()
                        End If

                        If osql.success = True Then
                            Save = True
                            ' not good but ok for ado.  Will change when all offline
                            populateUnits()
                        Else
                            Save = False
                            Dim errLog As New bc_cs_error_log("bc_am_composite", "Save", bc_cs_error_codes.USER_DEFINED, "Failed to save composite")
                            Exit Try
                        End If
                End Select
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_composite", "Save", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            view.Cursor = Cursors.Default
            log = New bc_cs_activity_log("bc_am_composite", "Save", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

    Private Sub deleteUnit()

        Dim log = New bc_cs_activity_log("bc_am_composite", "deleteUnit", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            If MessageBox.Show("Are you sure you wish to delete the selected unit?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then

                Dim osql As New bc_om_sql(String.Concat("exec bcc_core_bp_delete_composite ", "null, ", results(2, view.uxPubTypes.SelectedItems(0).Tag)))
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                    osql.tmode = bc_cs_soap_base_class.tREAD
                    osql.transmit_to_server_and_receive(osql, True)
                ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    osql.db_read()
                End If

                If osql.success = True Then
                    view.uxPubTypes.SelectedItems(0).Remove()
                Else
                    Dim errLog As New bc_cs_error_log("bc_am_composite", "deleteUnit", bc_cs_error_codes.USER_DEFINED, "Failed to delete composite unit")
                    Exit Try
                End If
                ' reset the display
                Reset()
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_composite", "deleteUnit", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_composite", "deleteUnit", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Friend Function DeleteAllUnits(ByVal PubTypeID As Integer) As Boolean

        Dim log = New bc_cs_activity_log("bc_am_composite", "DeleteAllUnits", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            If MessageBox.Show("Are you sure you wish to delete the selected composite?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then

                Dim osql As New bc_om_sql(String.Concat("exec bcc_core_bp_delete_composite ", PubTypeID, ", null"))
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                    osql.tmode = bc_cs_soap_base_class.tREAD
                    osql.transmit_to_server_and_receive(osql, True)
                ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    osql.db_read()
                End If

                If osql.success = True Then
                    DeleteAllUnits = True
                Else
                    Dim errLog As New bc_cs_error_log("bc_am_composite", "Save", bc_cs_error_codes.USER_DEFINED, "Failed to delete composite.")
                    Exit Try
                End If
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_composite", "DeleteAllUnits", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_composite", "DeleteAllUnits", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

    Private Function buildSQL(ByVal pubTypeID As Integer) As String

        Dim log = New bc_cs_activity_log("bc_am_composite", "buildSQL", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim sql As New StringBuilder

            ' build default composite sql
            With sql
                .Append("select distinct doc_id, doc_title, doc_object, 1 ")
                .Append("from document_table, container_tbl ")
                .Append("where document_table.container_id=container_tbl.container_id ")
                .Append("and container_tbl.stage_id = ")
                .Append(view.uxWorkflowStage.SelectedItem.id)
                .Append(" and doc_pub_type_id = ")
                .Append(pubTypeID)
                .Append(" and document_table.user_Id = 0 ")
                If bc_cs_central_settings.userOfficeStatus = 2 Then
                    .Append("and doc_extension in(''.docx'', ''.pptx'') order by doc_title asc")
                Else
                    .Append("and doc_extension in(''.doc'', ''.ppt'') order by doc_title asc")
                End If
            End With

            Return sql.ToString

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_composite", "buildSQL", bc_cs_error_codes.USER_DEFINED, ex.Message)
            Return ""
        Finally
            log = New bc_cs_activity_log("bc_am_composite", "buildSQL", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

    Private Function populateUnits() As Boolean

        Dim log = New bc_cs_activity_log("bc_am_composite", "populateUnits", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim lvi As ListViewItem
            Dim i As Integer

            Dim osql As New bc_om_sql(String.Concat("exec bcc_core_bp_get_composite_units ", view.Tag))

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                osql.tmode = bc_cs_soap_base_class.tREAD
                osql.transmit_to_server_and_receive(osql, True)
            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                osql.db_read()
            End If
            If osql.success = True Then
                populateUnits = True
            Else
                populateUnits = False
                Exit Function
            End If

            view.uxPubTypes.Items.Clear()

            results = osql.results

            For i = 0 To UBound(results, 2)
                lvi = view.uxPubTypes.Items.Add(results(1, i), publicationTypes_icon)
                ' store the index for retrieval later on
                lvi.Tag = i
            Next

            Reset()

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_composite", "populateUnits", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_composite", "populateUnits", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

    Friend Sub ChangeOrder(ByVal moveUp As Boolean)

        Dim log = New bc_cs_activity_log("bc_am_composite", "ChangeOrder", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim lvi As ListViewItem
            Dim i As Integer

            ' re-orders the composite units
            If moveUp Then
                lvi = view.uxPubTypes.SelectedItems(0)
                i = lvi.Index
                If lvi.Index > 0 Then
                    view.uxPubTypes.SelectedItems(0).Remove()
                    view.uxPubTypes.Items.Insert(i - 1, lvi)
                End If
            Else
                lvi = view.uxPubTypes.SelectedItems(0)
                i = lvi.Index
                If lvi.Index < view.uxPubTypes.Items.Count - 1 Then
                    view.uxPubTypes.SelectedItems(0).Remove()
                    view.uxPubTypes.Items.Insert(i + 1, lvi)
                End If
            End If

            view.uxAdd.Enabled = False
            view.uxEdit.Enabled = False
            view.uxDelete.Enabled = False
            view.uxSaveOrder.Enabled = True
            view.uxRefresh.Enabled = True

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_composite", "ChangeOrder", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_composite", "ChangeOrder", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Friend Function Validate(Optional ByVal saveComposite As Boolean = False) As Boolean

        Dim log = New bc_cs_activity_log("bc_am_composite", "Validate", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim lvi As ListViewItem

            Validate = True

            ' ensure the same unit cannot be added twice to the same composite.
            If view.uxPublicationType.Enabled = True And saveComposite Then
                For Each lvi In view.uxPubTypes.Items
                    If results(2, lvi.Tag) = view.uxPublicationType.SelectedItem.id Then
                        Dim omessage As New bc_cs_message("BluePrint - Blue Curve", "This unit is already part of the composite document", bc_cs_message.MESSAGE)
                        Validate = False
                    End If
                Next
            End If

            ' ensure a composite cannot be part of itself.
            If view.uxPublicationType.Enabled = True And saveComposite Then
                If view.Tag = view.uxPublicationType.SelectedItem.id Then
                    Dim omessage As New bc_cs_message("BluePrint - Blue Curve", "A composite document cannot be part of itself.", bc_cs_message.MESSAGE)
                    Validate = False
                End If
            End If

            If Trim(view.uxDescription.Text) = "" And Not saveComposite Then
                Validate = False
            End If

            If view.uxPublicationType.SelectedItem Is Nothing And Not saveComposite Then
                Validate = False
            End If

            If view.uxWorkflowStage.SelectedIndex = 0 And Not saveComposite Then
                Validate = False
            End If

            If Validate Then
                view.uxSave.Enabled = True
            Else
                view.uxSave.Enabled = False
            End If

        Catch ex As Exception
            Validate = False
            Dim errLog As New bc_cs_error_log("bc_am_composite", "ChangeOrder", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_composite", "ChangeOrder", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

    Private Sub loadWorkflowStages()

        Dim log = New bc_cs_activity_log("bc_am_composite", "loadWorkflowStages", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim osql As New bc_om_sql("exec bcc_core_bp_get_workflow_stages")
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                osql.tmode = bc_cs_soap_base_class.tREAD
                osql.transmit_to_server_and_receive(osql, True)
            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                osql.db_read()
            End If

            If osql.success = True Then
                If Not osql.results Is Nothing Then
                    Dim i As Integer
                    Dim cbh As New ComboBoxHelper
                    For i = 0 To UBound(osql.results, 2)
                        cbh.Add(CType(osql.results(0, i), Integer), osql.results(1, i))
                    Next

                    view.uxWorkflowStage.DisplayMember = "Name"
                    view.uxWorkflowStage.ValueMember = "ID"
                    view.uxWorkflowStage.DataSource = cbh

                    view.uxWorkflowStage.SelectedIndex = 0
                End If
            Else
                Dim errLog As New bc_cs_error_log("bc_am_composite", "Workflow Stages", bc_cs_error_codes.USER_DEFINED, "Failed to load workflow stages.")
                Exit Try
            End If

        Catch ex As Exception

            Dim errLog As New bc_cs_error_log("bc_am_composite", "loadWorkflowStages", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_composite", "loadWorkflowStages", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub


    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class
