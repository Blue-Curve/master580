Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports System.Windows.Forms
Imports System.Collections
Imports BlueCurve.Core.AS
Public Class load_scheduler
    Public Sub load()
        Dim fs As New bc_dx_task_schedule
        Dim cfs As New Cbc_dx_task_schedule
        If cfs.load_data(fs) = True Then
            fs.ShowDialog()
        End If
    End Sub
End Class

Public Class warning
    Public msg As String
    Public entity As String
    Public entity_tab As Integer
    Public association As String
    Public association_tab As Integer
    Public attribute_tab As Integer
    Public attribute As String
    Public linked_class_name As String
    Public linked_entity_name As String


    Public Sub New()

    End Sub
End Class
Friend Class bc_am_taxonomy

    Friend view As bc_am_cp_main
    Public container As bc_am_cp_container
    Private setting_text_box As Boolean = False
    Private schema_id As Integer
    Private class_name As String

    Private size_mode As String = "Structure"
    Public new_entity As Boolean = False


    Public from_class_list As Boolean = False
    Public hold_vals As New ArrayList
    Public no_Action As Boolean = False

    'icons
    Private Const add_icon As Integer = 0
    Private Const edit_icon As Integer = 1
    Private Const delete_icon As Integer = 2
    Private Const activate_icon As Integer = 3
    Private Const deactivate_icon As Integer = 6

    Private Const class_icon As Integer = 0
    Private Const selected_icon As Integer = 1
    Private Const unlinked_icon As Integer = 2
    Private Const data_icon As Integer = 3
    Private Const inactive_icon As Integer = 4

    Private Const child_parent_icon = 0
    Private Const parent_child_icon = 1
    Private Const links_icon = 2
    Private Const attributes_icon = 3
    Private Const att_edited_icon = 5
    Private Const selected_class_icon = 8
    Private Const mandatory_icon As Integer = 9
    Private Const read_only_icon As Integer = 25
    Private Const number_icon As Integer = 13
    Private Const date_icon As Integer = 14
    Private Const string_icon As Integer = 15
    Private Const boolean_icon As Integer = 16
    Private Const warning_icon As Integer = 17
    Private Const selected_data_icon As Integer = 18
    Private Const users_icon As Integer = 19
    Private Const user_icon As Integer = 20
    Private Const parent_icon As Integer = 21
    Private Const child_icon As Integer = 22
    Private Const push_publish_icon As Integer = 24
    Private Const step_icon As Integer = 26
    Public Sub set_default_attribute_values(lclass_name As String, from_assign As Boolean)
        Try
            If from_assign = False Then
                idx = sel_cls_idx(class_name)
                For j = 0 To Me.container.oClasses.classes(idx).attributes.count - 1
                    For i = 0 To Me.container.oClasses.attribute_pool.Count - 1
                        If Me.container.oClasses.attribute_pool(i).attribute_id = Me.container.oClasses.classes(idx).attributes(j).attribute_id Then
                            If Me.container.oClasses.attribute_pool(i).is_def = True Then
                                Dim oa As New bc_om_attribute
                                oa.attribute_id = Me.container.oClasses.attribute_pool(i).attribute_id
                                oa.read_default_value = True
                                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                                    oa.db_read_default_value()

                                Else
                                    oa.tmode = bc_cs_soap_base_class.tREAD
                                    If oa.transmit_to_server_and_receive(oa, True) = False Then
                                        Exit Sub
                                    End If
                                End If
                                view.DataGridView1.Item(3, j).Value = oa.default_value
                                For k = 0 To view.associated_entities(view.associated_entities.Count - 1).attribute_values.count - 1
                                    If view.associated_entities(view.associated_entities.Count - 1).attribute_values(k).attribute_id = Me.container.oClasses.attribute_pool(i).attribute_id Then
                                        view.associated_entities(view.associated_entities.Count - 1).attribute_values(k).value = oa.default_value
                                        view.associated_entities(view.associated_entities.Count - 1).attribute_values(k).value_changed = True
                                        Exit For
                                    End If
                                Next

                            End If
                        End If
                    Next
                Next
            Else
                idx = sel_cls_idx(lclass_name)
                For j = 0 To Me.container.oClasses.classes(idx).attributes.count - 1
                    For i = 0 To Me.container.oClasses.attribute_pool.Count - 1
                        If Me.container.oClasses.attribute_pool(i).attribute_id = Me.container.oClasses.classes(idx).attributes(j).attribute_id Then
                            If Me.container.oClasses.attribute_pool(i).is_def = True Then
                                Dim oa As New bc_om_attribute
                                oa.attribute_id = Me.container.oClasses.attribute_pool(i).attribute_id
                                oa.read_default_value = True
                                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                                    oa.db_read_default_value()

                                Else
                                    oa.tmode = bc_cs_soap_base_class.tREAD
                                    If oa.transmit_to_server_and_receive(oa, True) = False Then
                                        Exit Sub
                                    End If
                                End If
                                If view.DataGridView1.Item(3, j).Value = "" Then
                                    view.DataGridView1.Item(3, j).Value = oa.default_value
                                    For k = 0 To view.associated_entities(view.tassociations.SelectedIndex).attribute_values.count - 1
                                        If view.associated_entities(view.tassociations.SelectedIndex).attribute_values(k).attribute_id = Me.container.oClasses.attribute_pool(i).attribute_id Then
                                            view.associated_entities(view.tassociations.SelectedIndex).attribute_values(k).value = oa.default_value
                                            view.associated_entities(view.tassociations.SelectedIndex).attribute_values(k).value_changed = True
                                            Exit For
                                        End If
                                    Next
                                End If
                            End If
                        End If
                    Next
                Next
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_taxonomy", "set_default_attribute_values", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub

    Public Sub show_linked_audit(class_name As String)
        Dim fs As New bc_am_audit_entity_links
        Dim cs As New Cbc_am_audit_entity_links
        Dim entity_name As String
        Dim entity_id As Long
        Dim schema_id As Long
        Dim schema_name As String
        Dim user_mode As Boolean = False
        Dim class_id As Long
        Dim bparent As Boolean = False
        entity_name = view.associated_entities(view.tassociations.SelectedIndex).name
        entity_id = view.associated_entities(view.tassociations.SelectedIndex).id
        schema_id = Me.schema_id
        schema_name = view.cschema.Text

        If Len(class_name) >= 5 Then
            If Left(class_name, 5) <> "Users" Then
                class_name = Me.get_real_class_name(class_name)
            End If
        Else
            class_name = Me.get_real_class_name(class_name)
        End If

        If Left(class_name, 5) = "Users" Then
            user_mode = True
            If class_name <> "Users" Then
                class_name = Right(class_name, Len(class_name) - 6)
            Else
                class_name = ""
            End If
        Else
            For kk = 0 To container.oClasses.classes.Count - 1
                If container.oClasses.classes(kk).class_name = class_name Then
                    class_id = container.oClasses.classes(kk).class_id
                    Exit For
                End If
            Next
            If is_parent_class(class_name) = True Then
                bparent = True
            End If
        End If

        If user_mode = False Then
            If cs.load_data(fs, entity_id, class_id, schema_id, bparent, class_name, entity_name, schema_name, -1, 0, bc_om_audit_links.EAUDIT_TYPE.TAXONOMY) Then
                fs.ShowDialog()
            End If
        Else
            Dim pref_type_id As Integer
            For j = 0 To view.associated_entities(view.tassociations.SelectedIndex).user_prefs.count - 1
                For i = 0 To container.oUsers.user.Count - 1
                    If container.oUsers.user(i).id = view.associated_entities(view.tassociations.SelectedIndex).user_prefs(j).user_id And view.associated_entities(view.tassociations.SelectedIndex).user_prefs(j).pref_name = class_name Then
                        pref_type_id = view.associated_entities(view.tassociations.SelectedIndex).user_prefs(j).pref_type
                        Exit For
                    End If
                Next
            Next
            If cs.load_data(fs, entity_id, 0, 0, 0, class_name, entity_name, "", pref_type_id, 0, bc_om_audit_links.EAUDIT_TYPE.USERS_FOR_PREF) Then
                fs.ShowDialog()
            End If

        End If
    End Sub
    Friend Sub New(ByVal container As bc_am_cp_container, Optional ByVal view As bc_am_cp_main = Nothing)

        If Not view Is Nothing Then
            view.controller = Me
            Me.view = view
        End If

        If Not container Is Nothing Then
            Me.container = container
        End If

    End Sub

    Public Sub load_all()

        Dim log = New bc_cs_activity_log("bc_am_taxonomy", "load_all", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try

            With view
                .Size = view.Parent.Size
                .Cursor = Cursors.WaitCursor
                Select Case container.mode
                    Case bc_am_cp_container.VIEW, bc_am_cp_container.EDIT_LITE, bc_am_cp_container.EDIT_FULL
                        '.cschema.Items.Clear()
                        '.cschemael.Items.Clear()
                        If view.cschema.Items.Count = 0 Then
                            load_schemas()

                            load_class_list()
                        End If
                        If container.oClasses.schemas.Count > 0 Then
                            .cschema.SelectedIndex = 0
                        End If
                End Select
            End With
            If Me.container.mode = bc_am_cp_container.VIEW Then
                view.tentities.ContextMenuStrip = Nothing
                view.DataGridView1.Columns(0).Visible = False
                view.DataGridView1.Columns(3).Visible = False
                view.DataGridView1.Columns(1).ReadOnly = True
                view.DataGridView1.Columns(2).ReadOnly = True
                'view.DataGridView2.Columns(0).Visible = False
                'view.DataGridView2.Columns(3).Visible = False
                'view.DataGridView2.Columns(1).ReadOnly = True
                'view.DataGridView2.Columns(2).ReadOnly = True
                view.bentdiscard.Visible = False
                view.bentchanges.Visible = False
            End If
            view.tassociations.TabPages(0).ImageIndex = deactivate_icon
            view.tassociations.TabPages(0).Text = "no selection"


        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_taxonomy", "load_all", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_taxonomy", "load_all", bc_cs_activity_codes.TRACE_EXIT, "")
            view.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub load_schemas()

        Dim log = New bc_cs_activity_log("bc_am_taxonomy", "load_schemas", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            view.cschema.Items.Clear()
            view.cschemael.Items.Clear()
            For i = 0 To container.oClasses.schemas.Count - 1
                If container.oClasses.schemas(i).inactive = False Then
                    view.cschema.Items.Add(container.oClasses.schemas(i).schema_name)
                    view.cschemael.Items.Add(container.oClasses.schemas(i).schema_name)
                Else
                    view.cschema.Items.Add(container.oClasses.schemas(i).schema_name + " (inactive)")
                    'view.cschemael.Items.Add(container.oClasses.schemas(i).schema_name + " (inactive)")
                End If
            Next

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_taxonomy", "load_schemas", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_taxonomy", "load_schemas", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub

    Friend Sub SchemaChange()

        Dim log = New bc_cs_activity_log("bc_am_taxonomy", "SchemaChange", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try

            For i = 0 To container.oClasses.schemas.Count - 1
                If container.oClasses.schemas(i).schema_name = view.cschema.Text Then
                    schema_id = container.oClasses.schemas(i).schema_id
                    Exit For
                End If
            Next

            load_class_view(True)

            view.pdn.BorderStyle = BorderStyle.Fixed3D

            view.pup.BorderStyle = BorderStyle.None

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_taxonomy", "SchemaChange", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_taxonomy", "SchemaChange", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub

    Public Sub load_class_view(ByVal direction As Boolean)

        Dim log = New bc_cs_activity_log("bc_am_taxonomy", "load_class_view", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            Dim i As Integer
            Dim k As TreeNode
            Dim class_found As Boolean = False
            view.Cursor = Cursors.WaitCursor
            disable_class_attributes()

            view.ttaxonomy.Nodes.Clear()
            view.tfilter.SearchText = ""
            If direction = True Then

                For i = 0 To container.oClasses.classes.Count - 1
                    If Not (has_links_for_schema(container.oClasses.classes(i).parent_links, schema_id) = False And has_links_for_schema(container.oClasses.classes(i).child_links, schema_id) = False) Then
                        If has_links_for_schema(container.oClasses.classes(i).parent_links, schema_id) = False Then
                            k = New TreeNode(container.oClasses.classes(i).class_name)
                            k.ImageIndex = class_icon

                            propogate_class(k, container.oClasses.classes(i).class_id, schema_id, direction, Me.class_name, class_found)
                            view.ttaxonomy.Nodes.Add(k)
                        End If
                    End If
                Next
            Else
                For i = 0 To container.oClasses.classes.Count - 1
                    If Not (has_links_for_schema(container.oClasses.classes(i).parent_links, schema_id) = False And has_links_for_schema(container.oClasses.classes(i).child_links, schema_id) = False) Then
                        If has_links_for_schema(container.oClasses.classes(i).child_links, schema_id) = False Then
                            k = New TreeNode(container.oClasses.classes(i).class_name)
                            propogate_class(k, container.oClasses.classes(i).class_id, schema_id, direction, Me.class_name, class_found)
                            k.ImageIndex = class_icon
                            view.ttaxonomy.Nodes.Add(k)
                        End If
                    End If
                Next
            End If

            ' now load unused classes
            Dim new_node As New TreeNode("Unlinked classes")
            new_node.ImageIndex = unlinked_icon

            For i = 0 To container.oClasses.classes.Count - 1
                If has_links_for_schema(container.oClasses.classes(i).parent_links, schema_id) = False And has_links_for_schema(container.oClasses.classes(i).child_links, schema_id) = False Then
                    If container.oClasses.classes(i).inactive = False Then
                        new_node.Nodes.Add(container.oClasses.classes(i).class_name)
                        new_node.Nodes(new_node.Nodes.Count - 1).ImageIndex = class_icon
                    Else
                        new_node.Nodes.Add(container.oClasses.classes(i).class_name + " (inactive)")
                        new_node.Nodes(new_node.Nodes.Count - 1).ImageIndex = deactivate_icon
                    End If
                End If
            Next
            view.ttaxonomy.Nodes.Add(new_node)
            view.ttaxonomy.ExpandAll()
            If view.ttaxonomy.Nodes.Count > 1 Then
                view.ttaxonomy.Nodes(view.ttaxonomy.Nodes.Count - 1).Collapse()
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_taxonomy", "load_class_view", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            load_class_toolbar()
            log = New bc_cs_activity_log("bc_am_taxonomy", "load_class_view", bc_cs_activity_codes.TRACE_EXIT, "")
            reset_class_menu()
        End Try

    End Sub

    Private Function has_links_for_schema(ByVal class_links As ArrayList, ByVal schema_id As Long) As Boolean

        Dim log = New bc_cs_activity_log("bc_am_taxonomy", "has_links_for_schema", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            Dim i As Integer

            has_links_for_schema = False

            For i = 0 To class_links.Count - 1
                If class_links(i).schema_id = schema_id Then
                    has_links_for_schema = True
                    Exit For
                End If
            Next

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_taxonomy", "has_links_for_schema", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_taxonomy", "has_links_for_schema", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

    Private Sub propogate_class(ByRef node As TreeNode, ByVal class_id As Long, ByVal schema_id As Long, ByVal direction As Boolean, ByVal class_name As String, ByRef class_found As Boolean)
        Dim log = New bc_cs_activity_log("bc_am_taxonomy", "propogate_class", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            Dim i, j, l As Integer
            Dim k As TreeNode
            For i = 0 To container.oClasses.classes.Count - 1
                If container.oClasses.classes(i).class_id = class_id Then
                    If direction = True Then
                        For j = 0 To container.oClasses.classes(i).child_links.count - 1
                            For l = 0 To container.oClasses.classes.Count - 1
                                If container.oClasses.classes(l).class_id = container.oClasses.classes(i).child_links(j).child_class_id And container.oClasses.classes(i).child_links(j).schema_id = schema_id Then
                                    If container.oClasses.classes(i).child_links(j).mandatory_pc = 0 Then
                                        k = New TreeNode(container.oClasses.classes(l).class_name)
                                    ElseIf container.oClasses.classes(i).child_links(j).mandatory_pc = -1 Then
                                        k = New TreeNode(container.oClasses.classes(l).class_name + "*")
                                    Else
                                        k = New TreeNode(container.oClasses.classes(l).class_name + " (" + CStr(container.oClasses.classes(i).child_links(j).mandatory_pc) + ")")
                                    End If
                                    propogate_class(k, container.oClasses.classes(i).child_links(j).child_class_id, schema_id, direction, class_name, class_found)
                                    k.ImageIndex = class_icon
                                    node.Nodes.Add(k)

                                    Exit For
                                End If
                            Next
                        Next
                        REM no more links

                    Else
                        For j = 0 To container.oClasses.classes(i).parent_links.count - 1
                            For l = 0 To container.oClasses.classes.Count - 1
                                If container.oClasses.classes(l).class_id = container.oClasses.classes(i).parent_links(j).parent_class_id And container.oClasses.classes(i).parent_links(j).schema_id = schema_id Then
                                    If container.oClasses.classes(i).parent_links(j).mandatory_cp = 0 Then
                                        k = New TreeNode(container.oClasses.classes(l).class_name)
                                    ElseIf container.oClasses.classes(i).parent_links(j).mandatory_cp = -1 Then
                                        k = New TreeNode(container.oClasses.classes(l).class_name + "*")
                                    Else
                                        k = New TreeNode(container.oClasses.classes(l).class_name + " (" + CStr(container.oClasses.classes(i).parent_links(j).mandatory_cp) + ")")
                                    End If
                                    propogate_class(k, container.oClasses.classes(i).parent_links(j).parent_class_id, schema_id, direction, class_name, class_found)
                                    k.ImageIndex = class_icon
                                    node.Nodes.Add(k)
                                    Exit For
                                End If
                            Next
                        Next

                    End If
                End If
            Next
        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_taxonomy", "propogate_class", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_taxonomy", "propogate_class", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub

    Friend Sub ToggleTaxonomyView(ByVal parentChild As Boolean)

        Dim log = New bc_cs_activity_log("bc_am_taxonomy", "propogate_class", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try

            If parentChild Then
                view.pdn.BorderStyle = BorderStyle.Fixed3D

                view.pup.BorderStyle = BorderStyle.None

                load_class_view(True)
            Else
                view.pup.BorderStyle = BorderStyle.Fixed3D
                view.pdn.BorderStyle = BorderStyle.None

                load_class_view(False)
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_taxonomy", "propogate_class", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_taxonomy", "propogate_class", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    Public Sub load_class_links(ByVal name As String, ByVal mode As Integer)
        Dim i As Integer
        disable_class_attributes()
        For i = 0 To Me.container.oClasses.schemas.Count - 1
            If Me.container.oClasses.schemas(i).schema_name = name Or Me.container.oClasses.schemas(i).schema_name + " (inactive)" = name Then
                Me.schema_id = Me.container.oClasses.schemas(i).schema_id
                load_class_view(True)
                Exit For
            End If
        Next
    End Sub
    Public Sub show_structure_toolbar()
        Me.load_class_toolbar()
        view.pentities.Visible = False
        view.passociations.Visible = False
        view.tassociations.Visible = False
        view.pclass.Visible = True
        view.pattributes.Visible = True
        size_mode = "Structure"
        view.lvwatt.Columns(0).Width = view.lvwatt.Width - 170
    End Sub
    Friend Sub size_nicely()
        Try
            view.Lentities.Columns(0).Width = view.Lentities.Width - 5
            view.pclass.Dock = DockStyle.Fill
            view.pattributes.Dock = DockStyle.Fill
            view.pentities.Dock = DockStyle.Fill
            view.tassociations.Dock = DockStyle.Top
            view.passociations.Dock = DockStyle.Fill

        Catch ex As Exception
            MsgBox(ex.Message)

        End Try
    End Sub
    Public Sub data_grid_size()
        If view.DataGridView1.Columns(0).Visible = True Then
            view.DataGridView1.Columns(0).Width = view.DataGridView1.Width * (0.4 / 10)
            view.DataGridView1.Columns(1).Width = view.DataGridView1.Width * (2.8 / 10)
            view.DataGridView1.Columns(2).Width = view.DataGridView1.Width * (0.4 / 10)
            view.DataGridView1.Columns(3).Width = view.DataGridView1.Width * (2.8 / 10)
            view.DataGridView1.Columns(4).Width = view.DataGridView1.Width * (1 / 10)
            view.DataGridView1.Columns(5).Width = view.DataGridView1.Width * (2.8 / 10) - 42
        Else
            view.DataGridView1.Columns(1).Width = view.DataGridView1.Width * (1 / 3)
            view.DataGridView1.Columns(3).Width = view.DataGridView1.Width * (1 / 3)
            view.DataGridView1.Columns(5).Width = view.DataGridView1.Width * (1 / 3) - 42
        End If
    End Sub

    Public Sub show_data_toolbar()
        Try
            view.pclass.Visible = False
            view.pattributes.Visible = False
            view.pentities.Location = view.pclass.Location
            view.pentities.Visible = True
            view.passociations.Visible = True
            view.tassociations.Visible = True
            load_entity_toolbar()
            view.lvwvalidate.Columns(0).Width = view.lvwvalidate.Width - 22
            view.Lentities.Columns(0).Width = view.Lentities.Width - 22
        Catch

        End Try
    End Sub
    Private Sub disable_class_attributes()

        view.lvwatt.Items.Clear()
        For i = 1 To view.tattributelinks.TabPages.Count - 1
            view.tattributelinks.TabPages.RemoveAt(1)
        Next
        view.tattributelinks.TabPages(0).Text = "Class"
        view.tattributelinks.TabPages(0).ImageIndex = selected_class_icon

    End Sub
    Public Sub class_selected(ByVal class_name As String, Optional ByVal from_taxonomy As Boolean = True)
        disable_class_attributes()




        If class_name = "Unlinked classes" Then
            view.ttaxonomy.SelectedNode.ImageIndex = unlinked_icon
            reset_class_menu()
            Exit Sub
        End If

        If class_name = "" Then
            Exit Sub
        End If

        view.Cursor = Cursors.WaitCursor

        Me.class_name = Replace(class_name, "(inactive)", "")



        Me.class_name = get_real_class_name(class_name)



        REM set up context menu for class
        Try
            view.mrempl.Text = "Remove " + Me.class_name + " link to " + get_real_class_name(view.ttaxonomy.SelectedNode.Parent.Text)
            If LCase(view.ttaxonomy.SelectedNode.Parent.Text) <> "unlinked classes" Then
                view.tmsetlinknumber.Text = "Set number of " + Me.class_name + "(s) that " + get_real_class_name(view.ttaxonomy.SelectedNode.Parent.Text) + " can have."
                view.tmsetlinknumber.Visible = True
                If view.ttaxonomy.SelectedNode.Parent.Text <> "Unlinked classes" Then
                    view.mrempl.Visible = True
                End If
            Else
                view.mrempl.Visible = False
                view.tmsetlinknumber.Visible = False
            End If
        Catch
            view.mrempl.Visible = False
            view.tmsetlinknumber.Visible = False
        End Try

        view.mrempl.Enabled = True
        If view.pdn.BorderStyle = BorderStyle.Fixed3D Then
            view.maddplink.Text = "Add Class Link to " + Me.class_name

        Else
            view.maddplink.Text = "Add Class Link to " + Me.class_name
        End If
        view.mrenameclass.Text = "Change name of " + Me.class_name
        view.maddplink.Visible = True
        view.mdelclass.Visible = False

        REM set up attribute menu


        REM evaluate potential classes that can be lonled to
        populate_classes_to_link_to()
        view.ttaxonomy.HideSelection = False
        view.lattributes.Text = "Attributes of  " + Me.class_name

        REM set up attributes of class

        load_class_attributes(Me.class_name)

        view.Cursor = Cursors.WaitCursor
        REM set data class view
        Try
            If InStr(view.ttaxonomy.SelectedNode.Text, "inactive") = 0 Then
                view.cclass.Text = get_real_class_name(view.ttaxonomy.SelectedNode.Text)
            Else
                view.cclass.SelectedIndex = -1
            End If
        Catch

        End Try

        view.Cursor = Cursors.WaitCursor
        REM evakuate attributes not assignd to class 
        load_unused_attributes(class_name)
        view.mdelclass.Visible = True


        REM can only deactivate an unlinked class
        view.mclactive.Visible = False
        view.mdelclass.Visible = False
        Try
            If LCase(view.ttaxonomy.SelectedNode.Parent.Text) = "unlinked classes" Then
                view.mclactive.Visible = True
                view.mdelclass.Visible = True
            End If
        Catch

        End Try
        view.Lentities.ContextMenuStrip = view.entityContextMenuStrip
        view.lvwatt.ContextMenuStrip = view.adminattributemenustrip


        If InStr(view.ttaxonomy.SelectedNode.Text, "(inactive)") > 0 Then
            view.mclactive.Text = "Make " + Me.class_name + " Active"
            view.mrenameclass.Enabled = False
            view.mdelclass.Enabled = True
            view.maddplink.Enabled = False
            view.Lentities.ContextMenuStrip = Nothing
            view.lvwatt.ContextMenuStrip = Nothing
        Else
            view.mclactive.Text = "Make " + Me.class_name + " Inactive"
            view.mrenameclass.Enabled = True
            view.mdelclass.Enabled = False
            view.maddplink.Enabled = True
        End If

        If Me.container.mode = bc_am_cp_container.EDIT_LITE Then
            view.DeleteEntityToolStripMenuItem.Visible = False
        End If
        If Me.container.mode = bc_am_cp_container.VIEW Then
            view.Lentities.ContextMenuStrip = Nothing
        End If

        view.mdelclass.Text = "Delete class " + Me.class_name

        REM can only delete entity classes with no entities and no links
        Try
            If view.ttaxonomy.SelectedNode.Parent.Text <> "Unlinked classes" Then
                view.mdelclass.Enabled = False
            End If
        Catch

        End Try


        If view.ttaxonomy.SelectedNode.Nodes.Count > 0 Then
            view.mdelclass.Enabled = False
        End If


        view.ttaxonomy.SelectedNode.ImageIndex = class_icon
        If LCase(view.ttaxonomy.SelectedNode.Text) = "unlinked classes" Then
            view.mrenameclass.Enabled = False
            view.mclactive.Enabled = False
            view.mdelclass.Enabled = False
            view.maddplink.Enabled = False
            view.tmsetlinknumber.Enabled = False
            view.mrempl.Enabled = False
        Else
            view.tmsetlinknumber.Enabled = True
            view.mclactive.Enabled = True

        End If
        view.tattributelinks.TabPages(0).Text = Me.class_name

    End Sub
    Public Function get_real_class_name(ByVal iclass_name As String) As String
        Try


            Dim i As Integer
            Dim brac As String = ""
            If iclass_name.Substring(iclass_name.Length - 1, 1) = "*" Then
                iclass_name = iclass_name.Substring(0, iclass_name.Length - 1)
            ElseIf iclass_name.Substring(iclass_name.Length - 1, 1) = ")" Then
                brac = iclass_name.Substring(InStrRev(iclass_name, "("), Len(iclass_name) - InStrRev(iclass_name, "(") - 1)
            End If
            If IsNumeric(brac) And iclass_name.Substring(iclass_name.Length - 1, 1) = ")" Then
                i = 2
                While iclass_name.Substring(iclass_name.Length - i, 1) <> "("
                    i = i + 1
                End While
                iclass_name = Trim(iclass_name.Substring(0, iclass_name.Length - i))

            End If
            iclass_name = Trim(Replace(iclass_name, "(inactive)", ""))



            For i = 0 To Me.container.oClasses.classes.Count - 1
                If iclass_name = Me.container.oClasses.classes(i).class_name Then
                    get_real_class_name = Me.container.oClasses.classes(i).class_name
                    Exit Function
                End If
            Next
            REM 7/11 PR changed this as it doesnt work if classes liek index and index sector* exist.

            'For i = 0 To Me.container.oClasses.classes.Count - 1
            '    If iclass_name.Length >= Me.container.oClasses.classes(i).class_name.length Then
            '        If iclass_name.Substring(0, Me.container.oClasses.classes(i).class_name.length) = Me.container.oClasses.classes(i).class_name Then
            '            get_real_class_name = Me.container.oClasses.classes(i).class_name
            '            Exit Function
            '        End If
            '    End If
            'Next
            get_real_class_name = ""
        Catch ex As Exception
            'Dim oerr As New bc_cs_error_log("bc_am_taxonomy", "get_real_class_name", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try
    End Function
    Private Function populate_classes_to_link_to() As ArrayList
        Dim out As New ArrayList
        Try
            REM cannot link to a clss if alredy linked to to as a parent or child
            REM cant link a child class if it is already a parent class in the progration 
            REM and vice versa.
            Dim found As Boolean = False
            For i = 0 To Me.container.oClasses.classes.Count - 1
                If Me.container.oClasses.classes(i).class_name = Me.class_name Then
                    For k = 0 To Me.container.oClasses.classes.Count - 1
                        found = False
                        If Me.container.oClasses.classes(k).class_name = Me.class_name Then
                            found = True
                        End If
                        For j = 0 To Me.container.oClasses.classes(i).parent_links.count - 1
                            If Me.container.oClasses.classes(i).parent_links(j).schema_id = Me.schema_id Then
                                If Me.container.oClasses.classes(k).class_id = Me.container.oClasses.classes(i).parent_links(j).parent_class_id Then
                                    found = True

                                End If
                            End If
                        Next
                        For j = 0 To Me.container.oClasses.classes(i).child_links.count - 1
                            If Me.container.oClasses.classes(i).child_links(j).schema_id = Me.schema_id Then
                                If Me.container.oClasses.classes(k).class_id = Me.container.oClasses.classes(i).child_links(j).child_class_id Then
                                    found = True

                                End If
                            End If
                        Next
                        If found = False Then
                            REM now check it is not linked in the inverse propogation
                            If view.pdn.BorderStyle = BorderStyle.Fixed3D Then
                                If is_class_linked(Me.container.oClasses.classes(i).class_id, Me.container.oClasses.classes(k).class_id, True) = False Then
                                    out.Add(Me.container.oClasses.classes(k).class_name)
                                End If
                            Else
                                If is_class_linked(Me.container.oClasses.classes(i).class_id, Me.container.oClasses.classes(k).class_id, False) = False Then
                                    out.Add(Me.container.oClasses.classes(k).class_name)
                                End If
                            End If
                        End If
                    Next

                End If

            Next
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_cp_main", "populate_classes_to_link_to", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            populate_classes_to_link_to = out
        End Try
    End Function
    Private Function load_unused_attributes(ByVal class_name As String) As ArrayList
        Try
            Dim found As Boolean
            Dim idx As Integer
            Dim atts As New ArrayList
            idx = sel_cls_idx(class_name)


            If view.tattributelinks.SelectedIndex = 0 Then
                For i = 0 To Me.container.oClasses.attribute_pool.Count - 1
                    found = False
                    For j = 0 To Me.container.oClasses.classes(idx).attributes.count - 1
                        If Me.container.oClasses.attribute_pool(i).attribute_id = Me.container.oClasses.classes(idx).attributes(j).attribute_id Then
                            found = True
                        End If
                    Next
                    If found = False Then
                        atts.Add(Me.container.oClasses.attribute_pool(i).name)
                    End If
                Next
            Else
                For i = 0 To Me.container.oClasses.attribute_pool.Count - 1
                    found = False
                    For j = 0 To Me.container.oClasses.classes(idx).parent_links(Me.get_parent_class_index_from_attribute(idx)).linked_attributes.count - 1
                        If Me.container.oClasses.attribute_pool(i).attribute_id = Me.container.oClasses.classes(idx).parent_links(Me.get_parent_class_index_from_attribute(idx)).linked_attributes(j).attribute_id Then
                            found = True
                            Exit For
                        End If
                    Next
                    If found = False Then
                        atts.Add(Me.container.oClasses.attribute_pool(i).name)
                    End If
                Next
            End If
            load_unused_attributes = atts
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_cp_main", "load_unused_attributes", bc_cs_error_codes.USER_DEFINED, ex.Message)
            load_unused_attributes = Nothing
        End Try
    End Function
    Public Function is_class_linked(ByVal class_id, ByVal link_class_id, ByVal direction) As Boolean
        Try

            is_class_linked = False
            For i = 0 To Me.container.oClasses.classes.Count - 1
                If direction = True Then
                    If Me.container.oClasses.classes(i).class_id = class_id Then
                        For j = 0 To Me.container.oClasses.classes(i).parent_links.count - 1
                            If Me.container.oClasses.classes(i).parent_links(j).schema_id = Me.schema_id Then
                                If Me.container.oClasses.classes(i).parent_links(j).parent_class_id = link_class_id Then
                                    is_class_linked = True
                                    Exit Function
                                Else
                                    is_class_linked = is_class_linked(Me.container.oClasses.classes(i).parent_links(j).parent_class_id, link_class_id, direction)
                                    If is_class_linked = True Then
                                        Exit Function
                                    End If
                                End If
                            End If
                        Next
                    End If
                Else
                    If Me.container.oClasses.classes(i).class_id = class_id Then
                        For j = 0 To Me.container.oClasses.classes(i).child_links.count - 1
                            If Me.container.oClasses.classes(i).child_links(j).schema_id = Me.schema_id Then
                                If Me.container.oClasses.classes(i).child_links(j).child_class_id = link_class_id And Me.container.oClasses.classes(i).child_links(j).schema_id = Me.schema_id Then
                                    is_class_linked = True
                                    Exit Function
                                Else
                                    is_class_linked = is_class_linked(Me.container.oClasses.classes(i).child_links(j).child_class_id, link_class_id, direction)
                                    If is_class_linked = True Then
                                        Exit Function
                                    End If
                                End If
                            End If
                        Next
                    End If

                End If
            Next
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_cp_main", "is_class_linked", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Function
    Public Function get_parent_class_index_from_attribute(ByVal cls_idx As Integer) As Integer
        Dim co As Integer = 0

        For j = 0 To Me.container.oClasses.classes(cls_idx).parent_links.count - 1
            If Me.container.oClasses.classes(cls_idx).parent_links(j).schema_id = Me.schema_id Then
                If co = view.tattributelinks.SelectedIndex - 1 Then
                    get_parent_class_index_from_attribute = j
                    Exit Function
                End If
                co = co + 1
            End If
        Next

    End Function
    Private Function sel_cls_idx(ByVal class_name As String) As Integer
        Dim cn As String
        Dim i As Integer


        cn = Me.get_real_class_name(class_name)

        For i = 0 To Me.container.oClasses.classes.Count - 1
            If Me.container.oClasses.classes(i).class_name = cn Then
                Return i
            End If
        Next
    End Function
    Public Sub new_load_entities(inactive As Boolean)
        Try
            view.tfilter.inactive = view.chkinactive.Checked


            view.Lentities.BeginUpdate()
            'view.Lentities.Visible = False
            view.Lentities.Items.Clear()
            view.Lentities.Sorting = SortOrder.None


            view.tfilter.RunSearch()

            Me.container.oEntities = bc_am_load_objects.obc_entities
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_taxonomy", "new_load_entities", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

            'view.Lentities.Visible = True
            view.Cursor = Cursors.Default

            view.Lentities.EndUpdate()
            view.Lentities.Sorting = SortOrder.Ascending
        End Try
    End Sub
    Public Sub load_class_attributes(ByVal class_name As String, Optional ByVal show_screen As Boolean = False, Optional ByVal from_list As Boolean = True)
        Try
            Dim i, j As Integer
            Dim idx As Integer


            idx = sel_cls_idx(class_name)

            view.Cursor = Cursors.WaitCursor
            view.breadonly.Enabled = False
            view.Batup.Enabled = False
            view.batdn.Enabled = False
            view.lclsatttext.Text = "XXXAttributes for "
            view.RemoveAttributeFromClassToolStripMenuItem.Enabled = False

            view.modatt.Enabled = False
            view.DeleteAttributeToolStripMenuItem.Enabled = False
            view.RemoveAttributeFromClassToolStripMenuItem.Text = "Remove attribute from class"
            view.tattributes.SelectedIndex = 0


            view.modatt.Text = "Modify attribute"
            view.lvwatt.Items.Clear()


            set_up_class_attributes()


            REM menus
            view.ChangeNameToolStripMenuItem.Enabled = False
            view.minactive.Enabled = False
            view.DeleteEntityToolStripMenuItem.Enabled = False

            view.Refresh()

            If InStr(view.ttaxonomy.SelectedNode.Text, "(inactive)") > 0 Then
                view.Lentities.Items.Clear()
                view.tentities.Nodes.Clear()
                view.DataGridView1.Rows.Clear()
                Exit Sub
            End If

            REM entities of class
            If from_list = True Then
                If InStr(view.ttaxonomy.SelectedNode.Text, "(inactive)") = 0 Then
                    view.Lentities.BeginUpdate()
                    'view.Lentities.Visible = False
                    view.Lentities.Items.Clear()
                    view.Lentities.Sorting = SortOrder.None

                    view.tfilter.SearchClass = Me.container.oClasses.classes(idx).class_id

                    view.tfilter.inactive = view.chkinactive.Checked

                    view.tfilter.SearchRefresh()
                    Me.container.oEntities = bc_am_load_objects.obc_entities
                    'For i = 0 To Me.container.oEntities.entity.Count - 1
                    '    If Me.container.oEntities.entity(i).class_id = Me.container.oClasses.classes(idx).class_id Then
                    '        If Me.container.oEntities.entity(i).inactive = False Then
                    '            view.Lentities.Items.Add(CStr(Me.container.oEntities.entity(i).name), data_icon)
                    '        Else
                    '            view.Lentities.Items.Add(CStr(Me.container.oEntities.entity(i).name), inactive_icon)
                    '        End If
                    '    End If
                    'Next
                End If
            End If


            REM attributes of class
            Dim oatt As bc_om_attribute_value
            view.DataGridView1.Rows.Clear()
            Dim oent As New bc_om_entity
            If from_list = True Then
                view.associated_entities.Clear()
                view.associated_entities.Add(oent)
                oent.class_id = Me.container.oClasses.classes(idx).class_id
                oent.class_name = view.cclass.Text

            End If
            view.DataGridView1.Rows.Clear()
            idx = sel_cls_idx(class_name)
            For j = 0 To Me.container.oClasses.classes(idx).attributes.count - 1
                For i = 0 To Me.container.oClasses.attribute_pool.Count - 1
                    If Me.container.oClasses.attribute_pool(i).attribute_id = Me.container.oClasses.classes(idx).attributes(j).attribute_id Then
                        oatt = New bc_om_attribute_value
                        oatt.attribute_Id = Me.container.oClasses.attribute_pool(i).attribute_id
                        oatt.submission_code = Me.container.oClasses.attribute_pool(i).submission_code
                        oatt.nullable = Me.container.oClasses.attribute_pool(i).nullable
                        oatt.show_workflow = Me.container.oClasses.attribute_pool(i).show_workflow
                        view.associated_entities(0).attribute_values.add(oatt)
                        view.DataGridView1.Rows.Add()
                        view.DataGridView1.Item(1, view.DataGridView1.Rows.Count - 1).Value = Me.container.oClasses.attribute_pool(i).name


                        If oatt.nullable = True Then
                            view.DataGridView1.Item(0, j).Value = New Drawing.Bitmap(1, 1)
                        Else
                            view.DataGridView1.Item(0, j).Value = view.tabimages.Images(mandatory_icon)
                        End If

                        Select Case Me.container.oClasses.attribute_pool(i).type_id
                            Case 1
                                view.DataGridView1.Item(2, j).Value = view.tabimages.Images(string_icon)
                            Case 2
                                view.DataGridView1.Item(2, j).Value = view.tabimages.Images(number_icon)
                            Case 3
                                view.DataGridView1.Item(2, j).Value = view.tabimages.Images(boolean_icon)
                            Case 5
                                view.DataGridView1.Item(2, j).Value = view.tabimages.Images(date_icon)
                            Case 10
                                view.DataGridView1.Item(2, j).Value = view.tabimages.Images(step_icon)
                        End Select

                        If Me.container.oClasses.classes(idx).attributes(j).permission = 1 Then
                            view.DataGridView1.Item(2, j).Value = view.tabimages.Images(read_only_icon)
                        End If

                        If oatt.show_workflow = False Or Me.container.oClasses.classes(idx).attributes(j).permission = 1 Then
                            view.DataGridView1.Item(4, j).Value = New Drawing.Bitmap(1, 1)
                        Else
                            view.DataGridView1.Item(4, j).Value = view.tabimages.Images(push_publish_icon)
                        End If
                        Exit For
                    End If
                Next
            Next
            REM class attribute links
            For i = 1 To view.tattributelinks.TabPages.Count - 1
                view.tattributelinks.TabPages.RemoveAt(1)
            Next
            view.tattributelinks.TabPages(0).Text = class_name
            If InStr(view.ttaxonomy.SelectedNode.Text, "(inactive)") = 0 Then
                view.tattributelinks.TabPages(0).ImageIndex = selected_class_icon
            Else
                view.tattributelinks.TabPages(0).ImageIndex = deactivate_icon
            End If
            REM set up class links child to parent only
            For i = 0 To Me.container.oClasses.classes(idx).parent_links.count - 1
                For j = 0 To Me.container.oClasses.classes.Count - 1
                    If Me.container.oClasses.classes(j).class_id = Me.container.oClasses.classes(idx).parent_links(i).parent_class_id And Me.container.oClasses.classes(idx).parent_links(i).schema_id = Me.schema_id Then
                        view.tattributelinks.TabPages.Add("linked to " + Me.container.oClasses.classes(j).class_name)
                        view.tattributelinks.TabPages(view.tattributelinks.TabPages.Count - 1).ImageIndex = links_icon
                        Exit For
                    End If
                Next
            Next
            view.tattributelinks.SelectedIndex = 0
            REM user preferances

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_taxonomy", "load_class_attributes", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            'view.Lentities.Visible = True
            view.Cursor = Cursors.Default

            view.Lentities.EndUpdate()
            view.Lentities.Sorting = SortOrder.Ascending
        End Try

    End Sub
    Private Sub load_associated_class_attributes(ByVal class_name As String)
        Try
            Dim i, j As Integer
            Dim idx As Integer
            idx = sel_cls_idx(class_name)

            REM attributes of class
            Dim oatt As bc_om_attribute_value
            view.DataGridView1.Rows.Clear()
            Dim oent As New bc_om_entity
            'view.associated_entities(view.tassociations.SelectedIndex).attribute_values.clear()
            For j = 0 To Me.container.oClasses.classes(idx).attributes.count - 1
                For i = 0 To Me.container.oClasses.attribute_pool.Count - 1
                    If Me.container.oClasses.attribute_pool(i).attribute_id = Me.container.oClasses.classes(idx).attributes(j).attribute_id Then
                        oatt = New bc_om_attribute_value
                        oatt.entity_id = view.associated_entities(view.tassociations.SelectedIndex).id
                        oatt.attribute_Id = Me.container.oClasses.attribute_pool(i).attribute_id
                        oatt.submission_code = Me.container.oClasses.attribute_pool(i).submission_code
                        oatt.nullable = Me.container.oClasses.attribute_pool(i).nullable
                        oatt.show_workflow = Me.container.oClasses.attribute_pool(i).show_workflow
                        If view.associated_entities(view.tassociations.SelectedIndex).attribute_values.count < Me.container.oClasses.classes(idx).attributes.count Then
                            view.associated_entities(view.tassociations.SelectedIndex).attribute_values.add(oatt)
                        End If
                        view.DataGridView1.Rows.Add()
                        view.DataGridView1.Item(1, view.DataGridView1.Rows.Count - 1).Value = Me.container.oClasses.attribute_pool(i).name
                        view.DataGridView1.Item(3, j).Value = view.associated_entities(view.tassociations.SelectedIndex).attribute_values(j).value


                        If oatt.nullable = True Then
                            view.DataGridView1.Item(0, j).Value = New Drawing.Bitmap(1, 1)
                        Else
                            view.DataGridView1.Item(0, j).Value = view.tabimages.Images(mandatory_icon)
                        End If

                        If oatt.show_workflow = False Then
                            view.DataGridView1.Item(4, j).Value = New Drawing.Bitmap(1, 1)
                        Else
                            view.DataGridView1.Item(4, j).Value = view.tabimages.Images(push_publish_icon)
                        End If
                        REM set value
                        Select Case Me.container.oClasses.attribute_pool(i).type_id
                            Case 1
                                view.DataGridView1.Item(2, j).Value = view.tabimages.Images(string_icon)
                            Case 2
                                view.DataGridView1.Item(2, j).Value = view.tabimages.Images(number_icon)
                            Case 3
                                view.DataGridView1.Item(2, j).Value = view.tabimages.Images(boolean_icon)
                            Case 5
                                view.DataGridView1.Item(2, j).Value = view.tabimages.Images(date_icon)

                        End Select
                        If Me.container.oClasses.classes(idx).attributes(j).permission = 1 Then
                            view.DataGridView1.Item(2, j).Value = view.tabimages.Images(read_only_icon)
                        End If
                    End If
                Next
            Next
            If Me.container.oClasses.classes(idx).attributes.count > 0 Then
                view.DataGridView1.Item(3, 0).Selected = True
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_cp_main", "load_class_attributes", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

        End Try

    End Sub
    Public Sub add_schema(ByVal schemaname As String)
        Dim oschema As New bc_om_schema
        If schemaname <> "" Then
            oschema.schema_name = schemaname
            For i = 0 To Me.container.oClasses.schemas.Count - 1
                If Trim(UCase(Me.container.oClasses.schemas(i).schema_name)) = Trim(UCase(oschema.schema_name)) Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Schema: " + oschema.schema_name + " already exists!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub
                End If
            Next


            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                oschema.db_write()
            Else
                oschema.tmode = bc_om_schema.tWRITE
                If oschema.transmit_to_server_and_receive(oschema, True) = False Then
                    Exit Sub
                End If
            End If
            Me.container.oClasses.schemas.Add(oschema)
            view.cschema.Items.Add(oschema.schema_name)
            view.cschemael.Items.Add(oschema.schema_name)
            view.cschema.Text = oschema.schema_name

            load_class_view(view.pdn.BorderStyle = BorderStyle.FixedSingle)
            view.ttaxonomy.ExpandAll()
        Else
            Dim omsg As New bc_cs_message("Blue Curve", "Schema name must be entered!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
        End If

    End Sub
    Public Sub activate_schema(ByVal schema_name As String)
        Try
            Dim sidx As Integer
            For i = 0 To Me.container.oClasses.schemas.Count - 1
                If Me.container.oClasses.schemas(i).schema_name = schema_name Or Me.container.oClasses.schemas(i).schema_name + " (inactive)" = schema_name Then
                    sidx = i
                    Exit For
                End If
            Next

            If InStr(view.mschactive.Text, "Inactive") = 0 Then
                Me.container.oClasses.schemas(sidx).write_mode = bc_om_schema.SET_ACTIVE
            Else
                Me.container.oClasses.schemas(sidx).write_mode = bc_om_schema.SET_INACTIVE
            End If
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                Me.container.oClasses.schemas(sidx).db_write()
            Else
                Me.container.oClasses.schemas(sidx).tmode = bc_cs_soap_base_class.tWRITE
                If Me.container.oClasses.schemas(sidx).transmit_to_server_and_receive(Me.container.oClasses.schemas(sidx), True) = False Then
                    Exit Sub
                End If
            End If
            If InStr(view.mschactive.Text, "Inactive") = 0 Then
                Me.container.oClasses.schemas(sidx).inactive = 0
            Else
                Me.container.oClasses.schemas(sidx).inactive = 1
            End If
            Dim idx As Integer
            idx = view.cschema.SelectedIndex
            Me.load_schemas()
            view.cschema.SelectedIndex = idx

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("mschactive", "Click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

        End Try

    End Sub
    Public Sub select_schema(ByVal schema_name As String)
        Try
            Dim i As Integer
            For i = 0 To Me.container.oClasses.schemas.Count - 1
                If Me.container.oClasses.schemas(i).schema_name = schema_name Or Me.container.oClasses.schemas(i).schema_name + " (inactive)" = schema_name Then
                    Me.schema_id = Me.container.oClasses.schemas(i).schema_id
                    Exit For
                End If
            Next
            load_class_links(schema_name, 0)
            view.cclass.SelectedIndex = -1
            If InStr(view.cschema.Text, "inactive") = 0 Then
                view.cschemael.Text = view.cschema.Text
            Else
                view.cschemael.SelectedIndex = -1
            End If
            view.pdn.BorderStyle = BorderStyle.Fixed3D
            view.pup.BorderStyle = BorderStyle.None
            view.DeleteSchemaToolStripMenuItem.Enabled = False
            view.ChangeSchemaNameToolStripMenuItem.Enabled = True
            view.DeleteSchemaToolStripMenuItem.Text = "Delete Schema " + view.cschema.Text
            If InStr(view.cschema.Text, "(inactive)") > 0 Then
                view.mschactive.Text = "Make " + view.cschema.Text.Substring(0, view.cschema.Text.Length - 11) + " Active"
                view.DeleteSchemaToolStripMenuItem.Enabled = True
                view.ChangeSchemaNameToolStripMenuItem.Enabled = False
            Else
                view.mschactive.Text = "Make " + view.cschema.Text + " Inactive"
            End If
            If view.cschema.SelectedIndex > -1 Then
                view.ttaxonomy.Visible = True
                view.pdn.Visible = True
                view.pup.Visible = True
            End If
            view.ttaxonomy.Enabled = True
            If InStr(view.mschactive.Text, "Inactive") = 0 Then
                view.ttaxonomy.Enabled = False
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("cschema", "SelectedIndexChanged", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

        End Try
    End Sub
    Public Sub change_schmema_name(ByVal name As String)


        Dim oschema As New bc_om_schema
        If name <> "" Then
            oschema.schema_name = name
            oschema.schema_id = Me.schema_id

            For i = 0 To Me.container.oClasses.schemas.Count - 1
                If Trim(UCase(Me.container.oClasses.schemas(i).schema_name)) = Trim(UCase(oschema.schema_name)) Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Schema: " + oschema.schema_name + " already exists!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub
                End If
            Next
            oschema.write_mode = bc_om_schema.UPDATE
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                oschema.db_write()
            Else
                oschema.tmode = bc_om_schema.tWRITE
                If oschema.transmit_to_server_and_receive(oschema, True) = False Then
                    Exit Sub
                End If
            End If
            For i = 0 To Me.container.oClasses.schemas.Count - 1
                If Me.container.oClasses.schemas(i).schema_id = oschema.schema_id Then
                    Me.container.oClasses.schemas(i).schema_name = oschema.schema_name
                End If
            Next
            load_schemas()
            view.cschema.Text = oschema.schema_name
            view.cschema.Text = ""

        Else
            Dim omsg As New bc_cs_message("Blue Curve", "Schema name must be entered!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
        End If

    End Sub
    Public Sub delete_schema(ByVal name As String)
        Dim omsg As bc_cs_message

        Dim i As Integer
        For i = 0 To Me.container.oClasses.schemas.Count - 1
            If Me.container.oClasses.schemas(i).schema_name + " (inactive)" = name Then
                omsg = New bc_cs_message("Blue Curve", "Are you sure you wish to delete schema " + Me.container.oClasses.schemas(i).schema_name + "?", bc_cs_message.MESSAGE, True, False, "Yes", "no", True)
                If omsg.cancel_selected = True Then
                    Exit Sub
                End If
                view.Cursor = Cursors.WaitCursor
                Me.container.oClasses.schemas(i).write_mode = bc_om_schema.DELETE
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    Me.container.oClasses.schemas(i).db_write()
                Else
                    Me.container.oClasses.schemas(i).tmode = bc_cs_soap_base_class.tWRITE
                    If Me.container.oClasses.schemas(i).transmit_to_server_and_receive(Me.container.oClasses.schemas(i), True) = False Then
                        Exit Sub
                    End If
                End If
                If Me.container.oClasses.schemas(i).delete_error <> "" Then
                    omsg = New bc_cs_message("Blue Curve", "Failed to remove schema database error: " + Me.container.oClasses.schemas(i).delete_error, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub
                End If
                Me.container.oClasses.schemas.RemoveAt(i)
                Exit For
            End If
        Next
        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
            Me.container.oClasses.db_read()
        Else
            Me.container.oClasses.tmode = bc_cs_soap_base_class.tREAD
            If Me.container.oClasses.transmit_to_server_and_receive(Me.container.oClasses, True) = False Then
                Exit Sub
            End If
        End If
        load_schemas()
        view.ChangeSchemaNameToolStripMenuItem.Enabled = False
        view.DeleteSchemaToolStripMenuItem.Enabled = False
        If view.cschema.Items.Count > 0 Then
            view.cschema.SelectedIndex = 0
        End If
        omsg = New bc_cs_message("Blue Curve", "Schema Removed.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
    End Sub
    Public Sub load_attribute_toolbar()

    End Sub
    Public Sub load_entity_toolbar()
        Try
            Dim newitem As ToolStripItem
            Dim na As String
            Dim top_node As Boolean = False
            Dim unlinked_class As Boolean = False
            Me.container.uxGenericToolStrip.Items.Clear()
            If Me.container.mode = bc_am_cp_container.VIEW Then
                Exit Sub
            End If
            Me.container.uxGenericToolStrip.ImageList = Me.container.uxToolStripImageList
            With Me.container.uxGenericToolStrip.Items
                newitem = .Add("Add Entity")
                newitem.ToolTipText = "Add New Entity"
                newitem.TextImageRelation = TextImageRelation.ImageAboveText
                newitem.ImageIndex = add_icon
                newitem.Enabled = False
                newitem = .Add("Change Name")
                newitem.TextImageRelation = TextImageRelation.ImageAboveText
                newitem.ImageIndex = edit_icon
                newitem.ToolTipText = "Change Entity Name"
                newitem.Enabled = False
                newitem = .Add("Deactivate")
                newitem.TextImageRelation = TextImageRelation.ImageAboveText
                newitem.ToolTipText = "Deactivate Entity"
                newitem.ImageIndex = deactivate_icon
                newitem.Enabled = False
                newitem = .Add("Delete")
                newitem.TextImageRelation = TextImageRelation.ImageAboveText
                newitem.ToolTipText = "Delete Entity"
                newitem.ImageIndex = delete_icon
                newitem.Enabled = False
                If Me.container.mode = bc_am_cp_container.EDIT_LITE Then
                    newitem.Visible = False
                End If

                Dim separator As New ToolStripSeparator
                .Add(separator)

                newitem = .Add("Attribute Details")
                newitem.TextImageRelation = TextImageRelation.ImageAboveText
                newitem.ImageIndex = attributes_icon
                newitem.Enabled = False
                newitem = .Add("Navigate")
                newitem.TextImageRelation = TextImageRelation.ImageAboveText
                newitem.ImageIndex = 8
                newitem.Enabled = False
                newitem = .Add("Association")
                newitem.TextImageRelation = TextImageRelation.ImageAboveText
                newitem.ImageIndex = 7
                newitem.Enabled = False
                newitem = .Add("Remove Association")
                newitem.TextImageRelation = TextImageRelation.ImageAboveText
                newitem.Visible = False
                newitem.ImageIndex = delete_icon
                newitem.Enabled = False
            End With
            If view.cclass.SelectedIndex > -1 Then
                Me.container.uxGenericToolStrip.Items(0).ToolTipText = " Add New " + view.cclass.Text
                If view.bentchanges.Enabled = False Then
                    Me.container.uxGenericToolStrip.Items(0).Enabled = True
                End If
            End If
            If view.Lentities.SelectedItems.Count > 0 Then
                If view.bentchanges.Enabled = False Then
                    Me.container.uxGenericToolStrip.Items(1).Enabled = True
                    Me.container.uxGenericToolStrip.Items(2).Enabled = True

                End If

                na = view.Lentities.SelectedItems(0).Text
                If InStr(na, "(inactive)") > 0 Then
                    na = Left(na, InStr(na, "inactive") - 2)
                End If
                Me.container.uxGenericToolStrip.Items(1).ToolTipText = "Change name " + na
                Me.container.uxGenericToolStrip.Items(3).ToolTipText = "Delete " + na
                Me.container.uxGenericToolStrip.Items(2).Text = "Deactivate "
                Me.container.uxGenericToolStrip.Items(2).ImageIndex = deactivate_icon
                view.entityContextMenuStrip.Items(3).Image = view.tabimages.Images(deactivate_icon)


                If InStr(view.Lentities.SelectedItems(0).Text, "(inactive)") = 0 Then
                    Me.container.uxGenericToolStrip.Items(2).ToolTipText = "Deactivate " + na
                    Me.container.uxGenericToolStrip.Items(3).Enabled = False
                Else
                    Me.container.uxGenericToolStrip.Items(1).Enabled = False

                    If view.bentchanges.Enabled = False Then
                        Me.container.uxGenericToolStrip.Items(1).Enabled = False
                        Me.container.uxGenericToolStrip.Items(3).Enabled = True
                    End If
                    Me.container.uxGenericToolStrip.Items(2).ToolTipText = "Activate " + na
                    Me.container.uxGenericToolStrip.Items(2).Text = "Activate "
                    Me.container.uxGenericToolStrip.Items(2).ImageIndex = selected_icon
                    view.entityContextMenuStrip.Items(3).Image = view.tabimages.Images(activate_icon)


                End If
            End If


            If view.bentchanges.Enabled = True Then
                Me.container.uxGenericToolStrip.Items(0).Enabled = False
                Me.container.uxGenericToolStrip.Items(1).Enabled = False
                Me.container.uxGenericToolStrip.Items(2).Enabled = False
                Me.container.uxGenericToolStrip.Items(3).Enabled = False
            Else
            End If
            If view.DataGridView1.SelectedCells.Count = 1 Then
                Me.container.uxGenericToolStrip.Items(5).ToolTipText = "Details for " + view.DataGridView1.Item(1, view.DataGridView1.SelectedCells(0).RowIndex).Value
                Me.container.uxGenericToolStrip.Items(5).Enabled = True
            End If
            Dim tx As String



            tx = view.tentities.SelectedNode.Text
            REM now test if entity
            Try
                tx = view.tentities.SelectedNode.Parent.Text


                If tx.Substring(0, 5) <> "Users" Then
                    tx = view.tentities.SelectedNode.Text
                    Me.container.uxGenericToolStrip.Items(6).ToolTipText = "Navigate to " + tx
                    Me.container.uxGenericToolStrip.Items(6).Enabled = True
                End If
                If InStr(view.Lentities.SelectedItems(0).Text, "inactive") = 0 Then
                    tx = view.tentities.SelectedNode.Text
                    Me.container.uxGenericToolStrip.Items(8).Enabled = True
                    Me.container.uxGenericToolStrip.Items(8).ToolTipText = "Remove association to " + tx
                End If
            Catch
                REM top level
                If InStr(view.Lentities.SelectedItems(0).Text, "inactive") = 0 Then
                    Me.container.uxGenericToolStrip.Items(7).Enabled = True
                    Me.container.uxGenericToolStrip.Items(7).ToolTipText = "Assign " + tx + " Associations"
                End If

            End Try


        Catch

        End Try
    End Sub
    Public Sub load_class_toolbar()

        If Me.from_class_list = True Then
            Exit Sub
        End If

        Dim newitem As ToolStripItem
        Dim top_node As Boolean = False
        Dim unlinked_class As Boolean = False

        Me.container.uxGenericToolStrip.Items.Clear()
        Me.view.uxAttributesToolStrip.Items.Clear()

        With Me.container.uxGenericToolStrip.Items
            REM schema
            newitem = .Add("Add Schema")
            newitem.ImageIndex = add_icon
            newitem.ToolTipText = "Add new schema"
            newitem.TextImageRelation = TextImageRelation.ImageAboveText
            If view.cschema.SelectedIndex = -1 Then
                newitem = .Add("Edit Schema")
                newitem.Enabled = False
                newitem.ImageIndex = edit_icon
                newitem.ToolTipText = "Edit Schema"
                newitem.TextImageRelation = TextImageRelation.ImageAboveText
                newitem = .Add("Deactivate Schema")
                newitem.Enabled = False
                newitem.ImageIndex = deactivate_icon
                view.schemacontextmenu.Items(3).Image = view.tabimages.Images(deactivate_icon)
                newitem.ToolTipText = "Deactive schema"
                newitem.TextImageRelation = TextImageRelation.ImageAboveText
                newitem = .Add("Delete Schema")
                newitem.Enabled = False
                newitem.ImageIndex = delete_icon
                newitem.ToolTipText = "Delete schema"
                newitem.TextImageRelation = TextImageRelation.ImageAboveText
            Else
                newitem = .Add("Edit Schema")
                newitem.ToolTipText = "Change Name of Schema " + view.cschema.Text
                newitem.ImageIndex = edit_icon
                newitem.TextImageRelation = TextImageRelation.ImageAboveText
                If InStr(view.cschema.Text, "(inactive)") > 0 Then
                    newitem = .Add("Activate Schema")
                    newitem.ToolTipText = "Activate Schema " + view.cschema.Text
                    newitem.ImageIndex = activate_icon
                    newitem.TextImageRelation = TextImageRelation.ImageAboveText
                    view.schemacontextmenu.Items(3).Image = view.tabimages.Images(activate_icon)
                    newitem = .Add("Delete Schema")
                    newitem.ToolTipText = "Delete Schema " + view.cschema.Text
                    newitem.ImageIndex = delete_icon
                    newitem.TextImageRelation = TextImageRelation.ImageAboveText
                Else
                    newitem = .Add("Deactivate Schema")
                    newitem.ToolTipText = "Deactivate Schema " + view.cschema.Text
                    newitem.ImageIndex = deactivate_icon
                    newitem.TextImageRelation = TextImageRelation.ImageAboveText
                    newitem = .Add("Delete Schema")
                    newitem.ImageIndex = delete_icon
                    newitem.TextImageRelation = TextImageRelation.ImageAboveText
                    newitem.Enabled = False
                End If
            End If

            Dim separator As New ToolStripSeparator
            .Add(separator)

            newitem = .Add("Add Class")
            newitem.ImageIndex = add_icon
            newitem.TextImageRelation = TextImageRelation.ImageAboveText

            If view.cschema.SelectedIndex = -1 Then
                newitem.Enabled = False
            End If
            If IsNothing(view.ttaxonomy.SelectedNode) = True Then
                newitem = .Add("Edit Class")
                newitem.ImageIndex = edit_icon
                newitem.Enabled = False
                newitem.TextImageRelation = TextImageRelation.ImageAboveText
                newitem = .Add("Deactivate Class")
                newitem.ImageIndex = deactivate_icon
                newitem.Enabled = False
                newitem.TextImageRelation = TextImageRelation.ImageAboveText
                newitem = .Add("Delete Class")
                newitem.ImageIndex = delete_icon
                newitem.Enabled = False
                newitem.TextImageRelation = TextImageRelation.ImageAboveText
                newitem = .Add("Add Class Link")
                newitem.Enabled = False
                newitem.ImageIndex = add_icon
                newitem.TextImageRelation = TextImageRelation.ImageAboveText
                newitem = .Add("No.of Links")
                newitem.Enabled = False
                newitem.ImageIndex = edit_icon
                newitem.TextImageRelation = TextImageRelation.ImageAboveText
                newitem = .Add("Remove Class Link")
                newitem.ImageIndex = delete_icon
                newitem.Enabled = False
                newitem.TextImageRelation = TextImageRelation.ImageAboveText
            ElseIf UCase(view.ttaxonomy.SelectedNode.Text) = "UNLINKED CLASSES" Then
                newitem = .Add("Edit Class")
                newitem.ImageIndex = edit_icon
                newitem.Enabled = False
                newitem.TextImageRelation = TextImageRelation.ImageAboveText
                newitem = .Add("Deactivate Class")
                newitem.ImageIndex = deactivate_icon
                newitem.TextImageRelation = TextImageRelation.ImageAboveText
                view.classContextMenuStrip.Items(3).Image = view.tabimages.Images(deactivate_icon)
                newitem.Enabled = False
                newitem.TextImageRelation = TextImageRelation.ImageAboveText
                newitem = .Add("Delete Class")
                newitem.ImageIndex = delete_icon
                newitem.Enabled = False
                newitem.TextImageRelation = TextImageRelation.ImageAboveText
                newitem = .Add("Add Class Link")
                newitem.ImageIndex = add_icon
                newitem.Enabled = False
                newitem.TextImageRelation = TextImageRelation.ImageAboveText
                newitem = .Add("No.of Links")
                newitem.ImageIndex = edit_icon
                newitem.Enabled = False
                newitem.TextImageRelation = TextImageRelation.ImageAboveText
                newitem = .Add("Remove Class Link")
                newitem.ImageIndex = delete_icon
                newitem.Enabled = False
                newitem.TextImageRelation = TextImageRelation.ImageAboveText
            ElseIf UCase(view.ttaxonomy.SelectedNode.Text <> "UNLINKED CLASSES") Then
                Try
                    If UCase(view.ttaxonomy.SelectedNode.Parent.Text) = "UNLINKED CLASSES" Then
                        unlinked_class = True
                    End If
                Catch
                End Try
                newitem = .Add("Edit Class")
                newitem.ToolTipText = "Change Name of Class  " + Me.get_real_class_name(view.ttaxonomy.SelectedNode.Text)

                newitem.ImageIndex = edit_icon
                newitem.TextImageRelation = TextImageRelation.ImageAboveText
                If InStr(view.ttaxonomy.SelectedNode.Text, "(inactive)") = 0 Then
                    newitem = .Add("Deactivate Class")
                    newitem.ToolTipText = "Deactivate Class " + Me.get_real_class_name(view.ttaxonomy.SelectedNode.Text)
                    view.classContextMenuStrip.Items(3).Image = view.tabimages.Images(deactivate_icon)

                    newitem.ImageIndex = deactivate_icon
                    newitem.TextImageRelation = TextImageRelation.ImageAboveText
                    If unlinked_class = False Then
                        newitem.Enabled = False
                    End If
                    Try
                        If UCase(view.ttaxonomy.SelectedNode.Parent.Text) <> "UNLINKED CLASSES" Then
                            newitem.Enabled = False
                        End If
                    Catch
                        newitem.Enabled = False
                        top_node = True
                    End Try
                    newitem = .Add("Delete Class")
                    newitem.ToolTipText = "Delete Class " + Me.get_real_class_name(view.ttaxonomy.SelectedNode.Text)
                    newitem.ImageIndex = delete_icon
                    newitem.Enabled = False
                    newitem.TextImageRelation = TextImageRelation.ImageAboveText
                Else
                    newitem.Enabled = False
                    newitem = .Add("Activate Class")
                    newitem.ToolTipText = "Activate Class " + Me.get_real_class_name(view.ttaxonomy.SelectedNode.Text)
                    newitem.TextImageRelation = TextImageRelation.ImageAboveText
                    newitem.ImageIndex = activate_icon
                    newitem = .Add("Delete Class")
                    newitem.ToolTipText = "Delete Class " + Me.get_real_class_name(view.ttaxonomy.SelectedNode.Text)
                    newitem.ImageIndex = delete_icon
                    newitem.TextImageRelation = TextImageRelation.ImageAboveText
                End If
                If view.pdn.BorderStyle = BorderStyle.Fixed3D Then
                    newitem = .Add("Add Class Link")
                    newitem.ToolTipText = "Add Class Link to " + Me.get_real_class_name(view.ttaxonomy.SelectedNode.Text)
                    newitem.ImageIndex = add_icon
                    newitem.TextImageRelation = TextImageRelation.ImageAboveText
                    If InStr(view.ttaxonomy.SelectedNode.Text, "(inactive)") > 0 Then
                        newitem.Enabled = False
                    End If
                    If top_node = True Then
                        newitem = .Add("No.of Links")
                        newitem.ImageIndex = edit_icon
                        newitem.TextImageRelation = TextImageRelation.ImageAboveText
                        newitem.Enabled = False
                        newitem = .Add("Remove Class Link")
                        newitem.ImageIndex = delete_icon
                        newitem.TextImageRelation = TextImageRelation.ImageAboveText
                        newitem.Enabled = False
                    Else
                        If unlinked_class = True Then
                            newitem = .Add("No.of Links")
                            newitem.ImageIndex = edit_icon
                            newitem.Enabled = False
                            newitem.TextImageRelation = TextImageRelation.ImageAboveText
                            newitem = .Add("Remove Class Link")
                            newitem.ImageIndex = delete_icon
                            newitem.Enabled = False
                            newitem.TextImageRelation = TextImageRelation.ImageAboveText
                        Else
                            newitem = .Add("No.of Links")
                            newitem.ToolTipText = "Set number of " + Me.get_real_class_name(view.ttaxonomy.SelectedNode.Text) + " " + Me.get_real_class_name(view.ttaxonomy.SelectedNode.Parent.Text) + " can have"
                            newitem.ImageIndex = edit_icon
                            newitem.TextImageRelation = TextImageRelation.ImageAboveText
                            newitem = .Add("Remove Class Link")
                            newitem.ToolTipText = "Remove Class Link " + Me.get_real_class_name(view.ttaxonomy.SelectedNode.Text) + " to " + Me.get_real_class_name(view.ttaxonomy.SelectedNode.Parent.Text)
                            newitem.ImageIndex = delete_icon
                            newitem.TextImageRelation = TextImageRelation.ImageAboveText
                        End If
                    End If
                Else
                    newitem = .Add("Add Class Link")
                    newitem.ToolTipText = "Add Class Link to " + Me.get_real_class_name(view.ttaxonomy.SelectedNode.Text)
                    newitem.ImageIndex = add_icon
                    newitem.TextImageRelation = TextImageRelation.ImageAboveText
                    If InStr(view.ttaxonomy.SelectedNode.Text, "(inactive)") > 0 Then
                        newitem.Enabled = False
                    End If
                    If top_node = True Then
                        newitem = .Add("No.of Links")
                        newitem.ImageIndex = edit_icon
                        newitem.Enabled = False
                        newitem.TextImageRelation = TextImageRelation.ImageAboveText
                        newitem = .Add("Remove Class Link")
                        newitem.ImageIndex = delete_icon
                        newitem.Enabled = False
                        newitem.TextImageRelation = TextImageRelation.ImageAboveText
                    Else
                        If unlinked_class = True Then
                            newitem = .Add("No.of Links")
                            newitem.ImageIndex = edit_icon
                            newitem.Enabled = False
                            newitem.TextImageRelation = TextImageRelation.ImageAboveText
                            newitem = .Add("Remove Class Link")
                            newitem.ImageIndex = delete_icon
                            newitem.Enabled = False
                            newitem.TextImageRelation = TextImageRelation.ImageAboveText
                        Else
                            newitem = .Add("No.of Links")
                            newitem.ToolTipText = "Set number of " + Me.get_real_class_name(view.ttaxonomy.SelectedNode.Text) + " " + Me.get_real_class_name(view.ttaxonomy.SelectedNode.Parent.Text) + " can have"
                            newitem.ImageIndex = edit_icon
                            newitem.TextImageRelation = TextImageRelation.ImageAboveText
                            newitem = .Add("Remove Class Link")
                            newitem.ToolTipText = "Remove Class Link " + Me.get_real_class_name(view.ttaxonomy.SelectedNode.Text) + " to " + Me.get_real_class_name(view.ttaxonomy.SelectedNode.Parent.Text)
                            newitem.ImageIndex = delete_icon
                            newitem.TextImageRelation = TextImageRelation.ImageAboveText
                        End If
                    End If
                End If
            End If

        End With

        With view.uxAttributesToolStrip.Items

            view.uxAttributesToolStrip.ImageList = Me.container.uxToolStripImageList

            Try
                If UCase(view.ttaxonomy.SelectedNode.Text) <> "UNLINKED CLASSES" And InStr(view.ttaxonomy.SelectedNode.Text, "inactive") = 0 Then
                    newitem = .Add("Add Existing Attribute")
                    newitem.ImageIndex = add_icon
                    If view.tattributelinks.SelectedIndex = 0 Then
                        newitem.ToolTipText = "Assign an existing attribute to " + Me.get_real_class_name(view.ttaxonomy.SelectedNode.Text)
                    Else
                        newitem.ToolTipText = "Assign an existing attribute to " + Me.get_real_class_name(view.ttaxonomy.SelectedNode.Text) + " " + view.tattributelinks.TabPages(view.tattributelinks.SelectedIndex).Text
                    End If
                    newitem = .Add("Add New Attribute")
                    newitem.ImageIndex = add_icon
                    If view.tattributelinks.SelectedIndex = 0 Then
                        newitem.ToolTipText = "Add a new attribute and assign to " + Me.get_real_class_name(view.ttaxonomy.SelectedNode.Text)
                    Else
                        newitem.ToolTipText = "Add a new attribute and assign to " + Me.get_real_class_name(view.ttaxonomy.SelectedNode.Text) + " " + view.tattributelinks.TabPages(view.tattributelinks.SelectedIndex).Text
                    End If

                    If view.lvwatt.SelectedItems.Count > 0 Then
                        newitem = .Add("Modify Attribute")
                        newitem.ToolTipText = "Modify attribute " + view.lvwatt.SelectedItems(0).Text
                        newitem.ImageIndex = edit_icon
                        newitem = .Add("Remove Attribute")
                        If view.tattributelinks.SelectedIndex = 0 Then
                            newitem.ToolTipText = "Remove assignment of attribute " + view.lvwatt.SelectedItems(0).Text + " from " + Me.get_real_class_name(view.ttaxonomy.SelectedNode.Text)
                        Else
                            newitem.ToolTipText = "Remove assignment of attribute " + Me.get_real_class_name(view.ttaxonomy.SelectedNode.Text) + " " + view.tattributelinks.TabPages(view.tattributelinks.SelectedIndex).Text
                        End If
                        newitem.ImageIndex = delete_icon
                        newitem = .Add("Delete Attribute")
                        newitem.ToolTipText = "Delete attribute " + view.lvwatt.SelectedItems(0).Text + " from system"
                        newitem.ImageIndex = delete_icon
                    Else
                        newitem = .Add("Modify Attribute")
                        newitem.ImageIndex = edit_icon
                        newitem.Enabled = False
                        newitem = .Add("Remove Attribute")
                        newitem.ImageIndex = delete_icon
                        newitem.Enabled = False
                        newitem = .Add("Delete Attribute")
                        newitem.ImageIndex = delete_icon
                        newitem.Enabled = False
                    End If
                Else
                    newitem = .Add("Add Existing Attribute")
                    newitem.ImageIndex = add_icon
                    newitem.Enabled = False
                    newitem = .Add("Add New Attribute")
                    newitem.ImageIndex = add_icon
                    newitem.Enabled = False
                    newitem = .Add("Modify Attribute")
                    newitem.ImageIndex = edit_icon
                    newitem.Enabled = False
                    newitem = .Add("Remove Attribute")
                    newitem.ImageIndex = delete_icon
                    newitem.Enabled = False
                    newitem = .Add("Delete Attribute")
                    newitem.ImageIndex = delete_icon
                    newitem.Enabled = False
                End If
            Catch
                newitem = .Add("Add Existing Attribute")
                newitem.ImageIndex = add_icon
                newitem.Enabled = False
                newitem = .Add("Add New Attribute")
                newitem.ImageIndex = add_icon
                newitem.Enabled = False
                newitem = .Add("Modify Attribute")
                newitem.ImageIndex = edit_icon
                newitem.Enabled = False
                newitem = .Add("Remove Attribute")
                newitem.ImageIndex = delete_icon
                newitem.Enabled = False
                newitem = .Add("Delete Attribute")
                newitem.ImageIndex = delete_icon
                newitem.Enabled = False
            End Try

        End With
    End Sub
    Public Sub add_schema_menu()
        Dim fedit As New bc_am_cp_edit
        fedit.ltitle.Text = "Enter new schema name"
        fedit.ShowDialog()
        If fedit.cancel_selected = True Then
            Exit Sub
        Else
            view.Cursor = Cursors.WaitCursor
            add_schema(fedit.Tentry.Text)
            view.Cursor = Cursors.Default
        End If
    End Sub
    Public Sub change_schema_menu()
        Dim fedit As New bc_am_cp_edit
        fedit.ltitle.Text = "Change name of Schema " + view.cschema.Text
        fedit.ShowDialog()
        If fedit.cancel_selected = True Then
            Exit Sub
        Else
            view.Cursor = Cursors.WaitCursor
            Me.change_schmema_name(fedit.Tentry.Text)
        End If
    End Sub
    Public Sub add_class_menu()
        Dim fedit As New bc_am_cp_edit
        fedit.ltitle.Text = "Enter new class name "
        fedit.ShowDialog()
        If fedit.cancel_selected = True Then
            Exit Sub
        Else
            view.Cursor = Cursors.WaitCursor
            Me.add_class(fedit.Tentry.Text)
        End If
    End Sub
    Public Sub rename_class_menu()
        Dim fedit As New bc_am_cp_edit
        fedit.ltitle.Text = "Change name of " + view.ttaxonomy.SelectedNode.Text
        fedit.ShowDialog()
        If fedit.cancel_selected = True Then
            Exit Sub
        Else
            view.Cursor = Cursors.WaitCursor
            Me.rename_class(fedit.Tentry.Text)
        End If
    End Sub

    Private Sub add_class(ByVal name As String)
        Try

            Dim i As Integer
            If name <> "" Then
                For i = 0 To Me.container.oClasses.classes.Count - 1
                    If Trim(UCase(Me.container.oClasses.classes(i).class_name)) = Trim(UCase(name)) Then
                        Dim omsg As New bc_cs_message("Blue Curve", name + " already exists!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                        Exit Sub
                    End If
                Next
                Dim oclass As New bc_om_entity_class
                oclass.class_name = name
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    oclass.db_write()
                Else
                    oclass.tmode = bc_om_entity_class.tWRITE
                    If oclass.transmit_to_server_and_receive(oclass, True) = False Then

                        Exit Sub
                    End If

                End If
                Me.container.oClasses.classes.Add(oclass)
                If view.pdn.BorderStyle = True Then
                    Me.load_class_view(False)
                Else
                    Me.load_class_view(True)
                End If
                REM expand unlinked classed
                For i = 0 To view.ttaxonomy.Nodes.Count - 1
                    If view.ttaxonomy.Nodes(i).Text = "Unlinked classes" Then
                        view.ttaxonomy.Nodes(i).Expand()
                        For j = 0 To view.ttaxonomy.Nodes(i).Nodes.Count - 1
                            If view.ttaxonomy.Nodes(i).Nodes(j).Text = oclass.class_name Then
                                view.ttaxonomy.SelectedNode = view.ttaxonomy.Nodes(i).Nodes(j)
                                Exit For
                            End If
                        Next
                        Exit For
                    End If

                Next
            Else
                Dim omsg As New bc_cs_message("Blue Curve", "Class name must be entered!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            End If
            view.load_class_list()
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("OkToolStripMenuItem3", "click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
        End Try
    End Sub

    Private Sub rename_class(ByVal name As String)
        Try
            Dim i As Integer
            Dim oclass As New bc_om_entity_class
            If name <> "" Then
                For i = 0 To Me.container.oClasses.classes.Count - 1
                    If Trim(UCase(Me.container.oClasses.classes(i).class_name)) = Trim(UCase(name)) Then
                        Dim omsg As New bc_cs_message("Blue Curve", "Class name: " + name + " already exists!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                        Exit Sub
                    End If
                    If Me.container.oClasses.classes(i).class_name = view.ttaxonomy.SelectedNode.Text Then
                        oclass = Me.container.oClasses.classes(i)
                    End If
                Next
                oclass.class_name = name

                oclass.write_mode = bc_om_entity_class.UPDATE
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    oclass.db_write()
                Else
                    oclass.tmode = bc_om_entity_class.tWRITE
                    If oclass.transmit_to_server_and_receive(oclass, True) = False Then
                        Exit Sub
                    End If
                End If
                For i = 0 To Me.container.oClasses.classes.Count - 1
                    If Me.container.oClasses.classes(i).class_name = Me.class_name Then
                        oclass.class_id = Me.container.oClasses.classes(i).class_id
                        Me.container.oClasses.classes(i).class_name = oclass.class_name
                        Exit For
                    End If
                Next

                If view.pdn.BorderStyle = BorderStyle.Fixed3D Then
                    load_class_view(True)
                Else
                    load_class_view(False)
                End If
                view.load_class_list()
                set_class_in_tree_node(name)


            Else
                Dim omsg As New bc_cs_message("Blue Curve", "Class name must be entered!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("OkToolStripMenuItem5", "Click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Public Sub activate_class(ByVal name As String)

        Try
            view.Cursor = Cursors.WaitCursor
            Dim omsg As New bc_cs_message
            Dim i As Integer
            Dim sidx As Integer
            Dim found As Boolean = False
            For i = 0 To Me.container.oClasses.classes.Count - 1
                If Me.container.oClasses.classes(i).class_name = name Or Me.container.oClasses.classes(i).class_name + " (inactive)" = name Then
                    sidx = i
                    Exit For
                End If
            Next
            REM can only deactive a class that has no active entities and is not linked into any active schema
            If InStr(name, "inactive") = 0 Then
                For i = 0 To Me.container.oEntities.entity.Count - 1
                    If Me.container.oEntities.entity(i).class_id = Me.container.oClasses.classes(sidx).class_id And Me.container.oEntities.entity(i).inactive = False Then
                        omsg = New bc_cs_message("Blue Curve", "Cannot make class " + Me.container.oClasses.classes(sidx).class_name + " inactive as active entities of the class exist", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                        view.Cursor = Cursors.Default
                        Exit Sub
                    End If
                Next
                If Me.container.oClasses.classes(sidx).parent_links.count > 0 Or Me.container.oClasses.classes(sidx).child_links.count > 0 Then
                    omsg = New bc_cs_message("Blue Curve", "Cannot make class " + Me.container.oClasses.classes(sidx).class_name + " inactive as class has links in other schemas", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    view.Cursor = Cursors.Default
                    Exit Sub
                End If
            End If
            If InStr(name, "inactive") > 0 Then
                Me.container.oClasses.classes(sidx).write_mode = bc_om_schema.SET_ACTIVE
            Else
                Me.container.oClasses.classes(sidx).write_mode = bc_om_schema.SET_INACTIVE
            End If
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                Me.container.oClasses.classes(sidx).db_write()
            Else
                Me.container.oClasses.classes(sidx).tmode = bc_cs_soap_base_class.tWRITE
                If Me.container.oClasses.classes(sidx).transmit_to_server_and_receive(Me.container.oClasses.classes(sidx), True) = False Then
                    Exit Sub
                End If
            End If
            If InStr(name, "inactive") > 0 Then
                Me.container.oClasses.classes(sidx).inactive = 0
                view.Lentities.ContextMenuStrip = view.entityContextMenuStrip
                view.lvwatt.ContextMenuStrip = view.adminattributemenustrip
            Else
                Me.container.oClasses.classes(sidx).inactive = 1
                view.Lentities.ContextMenuStrip = Nothing
                view.lvwatt.ContextMenuStrip = Nothing
            End If
            If Me.container.mode = bc_am_cp_container.EDIT_LITE Then
                view.DeleteEntityToolStripMenuItem.Visible = False
            End If
            If Me.container.mode = bc_am_cp_container.VIEW Then
                view.Lentities.ContextMenuStrip = Nothing
            End If
            Me.load_class_view(True)
            view.ttaxonomy.CollapseAll()
            view.ttaxonomy.Nodes(view.ttaxonomy.Nodes.Count - 1).Expand()
            view.load_class_list()
            view.tattributes.Enabled = False
            reset_class_menu()
            If InStr(name, "inactive") > 0 Then
                set_class_in_tree_node(get_real_class_name(name))
            Else
                set_class_in_tree_node(name + " (inactive)")
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("mclactive", "Click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            view.Cursor = Cursors.Default
        End Try
    End Sub
    Public Sub reset_class_menu()
        view.mrenameclass.Enabled = False
        view.mclactive.Enabled = False
        view.mdelclass.Enabled = False
        view.tmsetlinknumber.Enabled = False
        view.mrempl.Enabled = False
        view.maddplink.Enabled = False
    End Sub
    Public Sub delete_class(ByVal name As String)
        Try
            name = Left(name, Len(name) - 10)
            Dim omsg As New bc_cs_message("Blue Curve", "Are you sure you wish to delete class " + name, bc_cs_message.MESSAGE, True, False, "Yes", "no", True)
            If omsg.cancel_selected = True Then
                Exit Sub
            End If
            view.Cursor = Cursors.WaitCursor
            Dim schema_name As String
            schema_name = view.cschema.Text
            For i = 0 To Me.container.oClasses.classes.Count - 1
                If Me.container.oClasses.classes(i).class_name = class_name Then
                    Me.container.oClasses.classes(i).write_mode = bc_om_entity_class.DELETE
                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        Me.container.oClasses.classes(i).db_write()
                    Else
                        Me.container.oClasses.classes(i).tmode = bc_om_entity_classes.tWRITE
                        If Me.container.oClasses.classes(i).transmit_to_server_and_receive(Me.container.oClasses.classes(i), True) = False Then
                            Exit Sub
                        End If
                    End If
                    If Me.container.oClasses.classes(i).delete_error <> "" Then
                        omsg = New bc_cs_message("Blue Curve", "Failed to delete class database error: " + Me.container.oClasses.classes(i).delete_error, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                        Exit Sub
                    Else
                        Dim co As Integer
                        REM now remove all links that use it
                        For j = 0 To Me.container.oClasses.classes.Count - 1
                            co = 0
                            For k = 0 To Me.container.oClasses.classes(j).parent_links.count - 1
                                If Me.container.oClasses.classes(j).parent_links(co).parent_class_id = Me.container.oClasses.classes(i).class_id Or Me.container.oClasses.classes(j).parent_links(co).child_class_id = Me.container.oClasses.classes(i).class_id Then
                                    Me.container.oClasses.classes(j).parent_links.removeat(co)
                                    co = co - 1
                                End If
                                co = co + 1
                            Next
                            co = 0
                            For k = 0 To Me.container.oClasses.classes(j).child_links.count - 1
                                If Me.container.oClasses.classes(j).child_links(co).parent_class_id = Me.container.oClasses.classes(i).class_id Or Me.container.oClasses.classes(j).child_links(co).child_class_id = Me.container.oClasses.classes(i).class_id Then
                                    Me.container.oClasses.classes(j).child_links.removeat(co)
                                    co = co - 1
                                End If
                                co = co + 1
                            Next

                        Next
                        REM remove class from memory
                        Me.container.oClasses.classes.RemoveAt(i)

                        If view.pdn.BorderStyle = BorderStyle.Fixed3D Then
                            Me.load_class_view(True)
                        Else
                            Me.load_class_view(False)
                        End If
                        view.ttaxonomy.Nodes(view.ttaxonomy.Nodes.Count - 1).Expand()
                        omsg = New bc_cs_message("Blue Curve", "Class " + name + " deleted.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)


                        Exit For
                    End If
                End If
            Next
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("mdelclass", "Click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            view.Cursor = Cursors.Default


        End Try
    End Sub
    Public Sub remove_class_link(ByVal name As String)
        Dim ocls_link As New bc_om_class_link_info
        ocls_link.schema_id = Me.schema_id
        Dim propdn As Boolean = True
        view.Cursor = Cursors.WaitCursor
        For i = 0 To Me.container.oClasses.classes.Count - 1
            If view.pdn.BorderStyle = BorderStyle.Fixed3D Then

                If Me.container.oClasses.classes(i).class_name = Me.class_name Then
                    ocls_link.child_class_id = Me.container.oClasses.classes(i).class_id
                End If
                If Me.container.oClasses.classes(i).class_name = get_real_class_name(view.ttaxonomy.SelectedNode.Parent.Text) Then
                    ocls_link.parent_class_id = Me.container.oClasses.classes(i).class_id
                End If
            Else
                propdn = False

                If Me.container.oClasses.classes(i).class_name = Me.class_name Then
                    ocls_link.parent_class_id = Me.container.oClasses.classes(i).class_id
                End If
                If Me.container.oClasses.classes(i).class_name = get_real_class_name(view.ttaxonomy.SelectedNode.Parent.Text) Then
                    ocls_link.child_class_id = Me.container.oClasses.classes(i).class_id
                End If

            End If
        Next

        For i = 0 To Me.container.oClasses.classes.Count - 1
            If Me.container.oClasses.classes(i).class_id = ocls_link.child_class_id Then
                For j = 0 To Me.container.oClasses.classes(i).parent_links.count - 1
                    If Me.container.oClasses.classes(i).parent_links(j).parent_class_id = ocls_link.parent_class_id And Me.container.oClasses.classes(i).parent_links(j).schema_id = Me.schema_id Then
                        Me.container.oClasses.classes(i).parent_links.removeat(j)
                        Exit For
                    End If
                Next
            End If

            If Me.container.oClasses.classes(i).class_id = ocls_link.parent_class_id Then
                For j = 0 To Me.container.oClasses.classes(i).child_links.count - 1
                    If Me.container.oClasses.classes(i).child_links(j).child_class_id = ocls_link.child_class_id And Me.container.oClasses.classes(i).child_links(j).schema_id = Me.schema_id Then
                        Me.container.oClasses.classes(i).child_links.removeat(j)
                        Exit For
                    End If
                Next
            End If
        Next



        ocls_link.write_mode = bc_om_class_link_info.DELETE
        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
            ocls_link.db_write()
        Else
            ocls_link.tmode = bc_cs_soap_base_class.tWRITE
            If ocls_link.transmit_to_server_and_receive(ocls_link, True) = False Then
                Exit Sub
            End If
        End If
        Me.load_class_view(propdn)
        view.Cursor = Cursors.Default
    End Sub
    Public Sub add_link_menu()
        Dim fedit As New bc_am_cp_edit
        Dim list As ArrayList
        If view.pdn.BorderStyle = BorderStyle.Fixed3D Then
            fedit.ltitle.Text = "Select child class for " + view.ttaxonomy.SelectedNode.Text
        Else
            fedit.ltitle.Text = "Select parent Class " + view.ttaxonomy.SelectedNode.Text
        End If
        fedit.Tentry.Visible = False
        fedit.centry.Visible = True
        list = Me.populate_classes_to_link_to()
        For i = 0 To list.Count - 1
            fedit.centry.Items.Add(list(i))
        Next
        fedit.ShowDialog()
        If fedit.cancel_selected = True Then
            Exit Sub
        Else
            If fedit.centry.SelectedIndex = -1 Then
                Exit Sub
            End If
            view.Cursor = Cursors.WaitCursor
            Me.add_link(fedit.centry.Text)
        End If
    End Sub
    Private Sub add_link(ByVal name As String)
        Dim ocls_link As New bc_om_class_link_info
        ocls_link.schema_id = Me.schema_id
        Dim propdn As Boolean = True

        For i = 0 To Me.container.oClasses.classes.Count - 1
            If view.pdn.BorderStyle = BorderStyle.Fixed3D Then
                If Me.container.oClasses.classes(i).class_name = Me.class_name Then
                    ocls_link.parent_class_id = Me.container.oClasses.classes(i).class_id
                End If
                If Me.container.oClasses.classes(i).class_name = name Then
                    ocls_link.child_class_id = Me.container.oClasses.classes(i).class_id
                End If
            Else
                propdn = False
                If Me.container.oClasses.classes(i).class_name = Me.class_name Then
                    ocls_link.child_class_id = Me.container.oClasses.classes(i).class_id
                End If
                If Me.container.oClasses.classes(i).class_name = name Then
                    ocls_link.parent_class_id = Me.container.oClasses.classes(i).class_id
                End If

            End If
        Next
        For i = 0 To Me.container.oClasses.classes.Count - 1
            If Me.container.oClasses.classes(i).class_id = ocls_link.child_class_id Then
                Me.container.oClasses.classes(i).parent_links.add(ocls_link)
            End If

            If Me.container.oClasses.classes(i).class_id = ocls_link.parent_class_id Then
                Me.container.oClasses.classes(i).child_links.add(ocls_link)
            End If
        Next

        ocls_link.write_mode = bc_om_class_link_info.INSERT
        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
            ocls_link.db_write()
        Else
            ocls_link.tmode = bc_cs_soap_base_class.tWRITE
            If ocls_link.transmit_to_server_and_receive(ocls_link, True) = False Then
                Exit Sub
            End If
        End If
        Me.load_class_view(propdn)
        view.Cursor = Cursors.Default

    End Sub
    Public Sub set_number()
        Dim fedit As New bc_am_cp_edit
        Dim list As ArrayList
        Dim i As Integer
        fedit.ltitle.Text = "Select amount of " + view.ttaxonomy.SelectedNode.Text + "(S) " + get_real_class_name(view.ttaxonomy.SelectedNode.Parent.Text) + " can have"
        fedit.Tentry.Visible = False
        fedit.centry.Visible = True
        list = Me.populate_classes_to_link_to()
        fedit.centry.Items.Clear()
        fedit.centry.Items.Add("Optional")
        fedit.centry.Items.Add("Mandatory")
        For i = 1 To 20
            fedit.centry.Items.Add(CStr(i) + " exactly")
        Next

        fedit.ShowDialog()
        If fedit.cancel_selected = True Then
            Exit Sub
        Else
            If fedit.centry.SelectedIndex = -1 Then
                Dim omsg As New bc_cs_message("Blue Curve", "Number must be set", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Sub
            End If
            view.Cursor = Cursors.WaitCursor
            set_link_number(fedit.centry.Text)
        End If
    End Sub
    Private Sub set_link_number(ByVal amount As String)
        Try

            Dim num As Integer
            If amount = "Optional" Then
                num = 0
            ElseIf amount = "Mandatory" Then
                num = -1
            Else
                num = CInt(Left(amount, InStr(amount, "exactly") - 1))
            End If
            Dim ocls_link As New bc_om_class_link_info
            ocls_link.schema_id = Me.schema_id
            Dim propdn As Boolean = True
            For i = 0 To Me.container.oClasses.classes.Count - 1
                If view.pdn.BorderStyle = BorderStyle.Fixed3D Then
                    If Me.container.oClasses.classes(i).class_name = Me.class_name Then
                        ocls_link.child_class_id = Me.container.oClasses.classes(i).class_id
                    End If
                    If Me.container.oClasses.classes(i).class_name = get_real_class_name(view.ttaxonomy.SelectedNode.Parent.Text) Then
                        ocls_link.parent_class_id = Me.container.oClasses.classes(i).class_id
                    End If
                    ocls_link.mandatory_pc = num

                Else
                    propdn = False
                    If Me.container.oClasses.classes(i).class_name = Me.class_name Then
                        ocls_link.parent_class_id = Me.container.oClasses.classes(i).class_id
                    End If
                    If Me.container.oClasses.classes(i).class_name = get_real_class_name(view.ttaxonomy.SelectedNode.Parent.Text) Then
                        ocls_link.child_class_id = Me.container.oClasses.classes(i).class_id
                    End If
                    ocls_link.mandatory_cp = num
                End If
            Next

            For i = 0 To Me.container.oClasses.classes.Count - 1
                If propdn = False Then
                    If Me.container.oClasses.classes(i).class_id = ocls_link.child_class_id Then
                        For j = 0 To Me.container.oClasses.classes(i).parent_links.count - 1
                            If Me.container.oClasses.classes(i).parent_links(j).parent_class_id = ocls_link.parent_class_id And Me.container.oClasses.classes(i).parent_links(j).schema_id = Me.schema_id Then
                                Me.container.oClasses.classes(i).parent_links(j).mandatory_cp = num
                                Exit For
                            End If
                        Next
                    End If
                Else
                    If Me.container.oClasses.classes(i).class_id = ocls_link.parent_class_id Then
                        For j = 0 To Me.container.oClasses.classes(i).child_links.count - 1
                            If Me.container.oClasses.classes(i).child_links(j).child_class_id = ocls_link.child_class_id And Me.container.oClasses.classes(i).child_links(j).schema_id = Me.schema_id Then
                                Me.container.oClasses.classes(i).child_links(j).mandatory_pc = num
                                Exit For
                            End If
                        Next
                    End If
                End If
            Next
            view.Cursor = Cursors.WaitCursor

            If propdn = True Then
                ocls_link.write_mode = bc_om_class_link_info.UPDATE_PC
            Else
                ocls_link.write_mode = bc_om_class_link_info.UPDATE_CP
            End If
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                ocls_link.db_write()
            Else
                ocls_link.tmode = bc_om_entity.tWRITE
                ocls_link.transmit_to_server_and_receive(ocls_link, True)
            End If
            Me.load_class_view(propdn)
            view.Cursor = Cursors.Default

        Catch ex As Exception
            Dim oerr As New bc_cs_activity_log("bsetlinknumber", "click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
        End Try

    End Sub
    Public Sub load_class_list()
        view.cclass.Items.Clear()
        For i = 0 To Me.container.oClasses.classes.Count - 1
            If Me.container.oClasses.classes(i).inactive = False Then
                view.cclass.Items.Add(Me.container.oClasses.classes(i).class_name)
            Else
                'view.cclass.Items.Add(Me.container.oClasses.classes(i).class_name + " (inactive)")
            End If
        Next
    End Sub
    Public Sub load_entities(ByVal class_name As String)
        Try


            Dim i, idx As Integer
            'Dim l As Integer
            Me.unload_entity()
            view.Cursor = Cursors.WaitCursor


            view.Lentities.BeginUpdate()

            view.Lentities.Visible = False
            view.Lentities.Sorting = SortOrder.None
            view.Lentities.Items.Clear()
            For i = 0 To Me.container.oClasses.classes.Count - 1
                If Me.container.oClasses.classes(i).class_name = class_name Or Me.container.oClasses.classes(i).class_name = " (inactive) = class_name Then" Then
                    idx = i
                    Exit For
                End If
            Next

            'Dim lvew As ListViewItem
            view.Lentities.Items.Clear()
            If view.tfilter.SearchText = "" Then
                view.Lentities.Visible = False
                REM entities of class

                view.tfilter.SearchClass = Me.container.oClasses.classes(idx).class_id

                view.tfilter.SearchRefresh()
                Me.container.oEntities = bc_am_load_objects.obc_entities
                'For i = 0 To Me.container.oEntities.entity.Count - 1
                '    If Me.container.oEntities.entity(i).class_id = Me.container.oClasses.classes(idx).class_id Then
                '        If Me.container.oEntities.entity(i).inactive = False Then
                '            lvew = New ListViewItem(CStr(Me.container.oEntities.entity(i).name), data_icon)
                '        Else
                '            lvew = New ListViewItem(CStr(Me.container.oEntities.entity(i).name + " (inactive)"), inactive_icon)
                '        End If
                '        view.Lentities.Items.Add(lvew)
                '    End If
                'Next
            Else

                view.tfilter.SearchClass = Me.container.oClasses.classes(idx).class_id

                view.tfilter.SearchRefresh()
                Me.container.oEntities = bc_am_load_objects.obc_entities
                'For i = 0 To Me.container.oEntities.entity.Count - 1
                '    If Me.container.oEntities.entity(i).class_id = Me.container.oClasses.classes(idx).class_id Then
                '        l = view.BlueCurve_TextSearch1.SearchText.Length
                '        If l <= Me.container.oEntities.entity(i).name.length Then
                '            If UCase(Me.container.oEntities.entity(i).name.Substring(0, l)) = UCase(view.BlueCurve_TextSearch1.SearchText) Then
                '                If Me.container.oEntities.entity(i).inactive = False Then
                '                    lvew = New ListViewItem(CStr(Me.container.oEntities.entity(i).name), data_icon)

                '                Else
                '                    lvew = New ListViewItem(CStr(Me.container.oEntities.entity(i).name + " (inactive)"), inactive_icon)
                '                End If
                '                view.Lentities.Items.Add(lvew)
                '            End If
                '        End If
                '    End If
                'Next
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("tfilter", "TextChanged", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            view.Lentities.Sorting = SortOrder.Ascending
            view.Lentities.Visible = True
            view.Cursor = Cursors.Default
            view.Lentities.EndUpdate()

        End Try
    End Sub
    Public Function set_class_in_tree_node(ByVal class_name As String) As Boolean
        If view.from_tree_view = True Then
            Exit Function
        End If
        Dim i As Integer
        For i = 0 To view.ttaxonomy.Nodes.Count - 1
            set_class_in_tree_node = set_class_in_tree(class_name, view.ttaxonomy.Nodes(i))
            If set_class_in_tree_node = True Then
                Exit Function
            End If
        Next
    End Function
    Public Function set_class_in_tree(ByVal class_name As String, ByVal node As TreeNode) As Boolean
        If node.Text = class_name Then
            view.ttaxonomy.SelectedNode = node
            set_class_in_tree = True
            Exit Function
        Else
            For i = 0 To node.Nodes.Count - 1
                If get_real_class_name(node.Nodes(i).Text) = class_name Then
                    view.ttaxonomy.SelectedNode = node.Nodes(i)
                    set_class_in_tree = True
                    Exit Function
                Else
                    set_class_in_tree = set_class_in_tree(class_name, node.Nodes(i))
                    If set_class_in_tree = True Then
                        Exit Function
                    End If
                End If
            Next
        End If

    End Function
    Public Sub set_up_class_attributes()
        Try
            Dim class_name As String
            Dim idx As Integer

            class_name = get_real_class_name(view.ttaxonomy.SelectedNode.Text)
            idx = sel_cls_idx(class_name)
            view.lvwatt.Items.Clear()

            view.modatt.Enabled = False
            view.DeleteAttributeToolStripMenuItem.Enabled = False
            view.batdn.Enabled = False
            view.Batup.Enabled = False


            view.lclsatttext.Text = "Attributes for " + view.tattributelinks.SelectedTab.Text
            If view.tattributelinks.SelectedIndex > 0 Then
                view.lclsatttext.Text = "Attributes for " + view.tattributelinks.TabPages(0).Text + " " + view.tattributelinks.SelectedTab.Text
            End If
            view.tattributelinks.Refresh()


            view.modatt.Text = "Modify attribute"
            class_name = Me.get_real_class_name(class_name)
            If view.tattributelinks.SelectedIndex = 0 Then

                view.AddExistingAttributeToolStripMenuItem.Text = "Add existing attribute to " + class_name
                view.AddNewAttributeToClassToolStripMenuItem.Text = "Add new attribute to " + class_name
                view.RemoveAttributeFromClassToolStripMenuItem.Text = "Remove attribute from " + class_name
            Else
                view.AddExistingAttributeToolStripMenuItem.Text = "Add existing attribute to " + class_name + " " + view.tattributelinks.SelectedTab.Text
                view.AddNewAttributeToClassToolStripMenuItem.Text = "Add new attribute to " + class_name + " " + view.tattributelinks.SelectedTab.Text
                view.RemoveAttributeFromClassToolStripMenuItem.Text = "Remove attribute from " + class_name + " " + view.tattributelinks.SelectedTab.Text
            End If
            Dim lvw As ListViewItem
            view.lvwatt.SmallImageList = view.tabimages

            If view.tattributelinks.SelectedIndex = 0 Then

                For j = 0 To Me.container.oClasses.classes(idx).attributes.count - 1
                    For i = 0 To Me.container.oClasses.attribute_pool.Count - 1
                        If Me.container.oClasses.attribute_pool(i).attribute_id = Me.container.oClasses.classes(idx).attributes(j).attribute_id Then
                            lvw = New ListViewItem(CStr(Me.container.oClasses.attribute_pool(i).name))
                            If Me.container.oClasses.attribute_pool(i).nullable = False Then
                                lvw.SubItems.Add("  X ")
                            Else
                                lvw.SubItems.Add("")
                            End If
                            If Me.container.oClasses.attribute_pool(i).show_workflow <> 0 Then
                                lvw.SubItems.Add("  X ")
                            Else
                                lvw.SubItems.Add("")
                            End If
                            If Me.container.oClasses.attribute_pool(i).submission_code = 2 Then
                                lvw.SubItems.Add("  X ")
                            Else
                                lvw.SubItems.Add("")
                            End If

                            If Me.container.oClasses.attribute_pool(i).is_lookup = True Then
                                lvw.SubItems.Add("  X ")
                            Else
                                lvw.SubItems.Add("")
                            End If
                            Select Case Me.container.oClasses.attribute_pool(i).type_id
                                Case 1
                                    lvw.ImageIndex = string_icon
                                Case 2
                                    lvw.ImageIndex = number_icon
                                Case 3
                                    lvw.ImageIndex = boolean_icon
                                Case 5
                                    lvw.ImageIndex = date_icon

                            End Select
                            If Me.container.oClasses.classes(idx).attributes(j).permission = 1 Then
                                lvw.SubItems.Add("  X ")
                            Else
                                lvw.SubItems.Add("")
                            End If
                            view.lvwatt.Items.Add(lvw)
                            Exit For
                        End If
                    Next
                Next
            Else

                REM linked attributes
                Dim pco As Integer = 0
                Dim pi As Integer = 0
                For i = 0 To Me.container.oClasses.classes(idx).parent_links.count - 1
                    If Me.container.oClasses.classes(idx).parent_links(i).schema_id = Me.schema_id Then
                        If pco = view.tattributelinks.SelectedIndex - 1 Then
                            pi = i
                        End If
                        pco = pco + 1
                    End If
                Next
                For j = 0 To Me.container.oClasses.classes(idx).parent_links(pi).linked_attributes.count - 1
                    For i = 0 To Me.container.oClasses.attribute_pool.Count - 1
                        If Me.container.oClasses.attribute_pool(i).attribute_id = Me.container.oClasses.classes(idx).parent_links(pi).linked_attributes(j).attribute_id Then
                            lvw = New ListViewItem(CStr(Me.container.oClasses.attribute_pool(i).name))

                            If Me.container.oClasses.attribute_pool(i).nullable = False Then
                                lvw.SubItems.Add("X")
                            Else
                                lvw.SubItems.Add("")
                            End If
                            If Me.container.oClasses.attribute_pool(i).show_workflow <> 0 Then
                                lvw.SubItems.Add("X")
                            Else
                                lvw.SubItems.Add("")
                            End If
                            If Me.container.oClasses.attribute_pool(i).submission_code = 2 Then
                                lvw.SubItems.Add("X")
                            Else
                                lvw.SubItems.Add("")
                            End If
                            If Me.container.oClasses.attribute_pool(i).is_lookup = True Then
                                lvw.SubItems.Add("  X ")
                            Else
                                lvw.SubItems.Add("")
                            End If
                            Select Case Me.container.oClasses.attribute_pool(i).type_id
                                Case 1
                                    lvw.ImageIndex = string_icon
                                Case 2
                                    lvw.ImageIndex = number_icon
                                Case 3
                                    lvw.ImageIndex = boolean_icon
                                Case 5
                                    lvw.ImageIndex = date_icon

                            End Select
                            If Me.container.oClasses.classes(idx).parent_links(pi).linked_attributes(j).permission = 1 Then
                                lvw.SubItems.Add("  X ")
                            Else
                                lvw.SubItems.Add("")
                            End If


                            view.lvwatt.Items.Add(lvw)
                            Exit For
                        End If
                    Next
                Next
            End If

            load_unused_attributes(class_name)
            Me.load_class_toolbar()

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("tattributelinks", "SelectedIndexChanged", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub

    Public Sub add_exisiting_attribute_menu()
        Dim fedit As New bc_am_cp_edit
        Dim atts As ArrayList
        Dim i As Integer
        fedit.ltitle.Text = "Add Existing Attribute to " + get_real_class_name(view.ttaxonomy.SelectedNode.Text)
        If view.tattributelinks.SelectedIndex > 0 Then
            fedit.ltitle.Text = fedit.ltitle.Text + " " + get_real_class_name(view.tattributelinks.SelectedTab.Text)
        End If
        fedit.Tentry.Visible = False
        fedit.centry.Visible = True
        atts = load_unused_attributes(view.ttaxonomy.SelectedNode.Text)
        For i = 0 To atts.Count - 1
            fedit.centry.Items.Add(atts(i))
        Next
        fedit.ShowDialog()
        If fedit.cancel_selected = True Then
            Exit Sub
        Else
            view.Cursor = Cursors.WaitCursor
            Me.assign_attribute_to_class(fedit.centry.Text, view.ttaxonomy.SelectedNode.Text)
        End If
    End Sub
    Private Sub assign_attribute_to_class(ByVal attribute As String, ByVal class_name As String)
        Try
            If attribute = "" Then
                Dim omsg As New bc_cs_message("Blue Curve", "Attribute must be entered", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            Else
                view.Cursor = Cursors.WaitCursor
                Dim i As Integer
                Dim idx As Integer
                class_name = get_real_class_name(class_name)
                idx = sel_cls_idx(class_name)

                Dim ocls_attribute As New bc_om_class_attribute
                For i = 0 To Me.container.oClasses.classes.Count - 1
                    If Me.container.oClasses.classes(i).class_name = class_name Then
                        ocls_attribute.class_id = Me.container.oClasses.classes(i).class_id
                        Exit For
                    End If
                Next
                For j = 0 To Me.container.oClasses.attribute_pool.Count - 1
                    If Me.container.oClasses.attribute_pool(j).name = attribute Then
                        ocls_attribute.attribute_id = Me.container.oClasses.attribute_pool(j).attribute_id
                        Exit For
                    End If
                Next
                ocls_attribute.order = view.lvwatt.Items.Count + 1
                Dim ts As Integer
                ts = view.tattributelinks.SelectedIndex

                If view.tattributelinks.SelectedIndex = 0 Then
                    ocls_attribute.write_mode = bc_om_class_attribute.INSERT_ATTRIBUTE
                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        ocls_attribute.db_write()
                    Else
                        ocls_attribute.tmode = bc_om_class_attribute.tWRITE
                        If ocls_attribute.transmit_to_server_and_receive(ocls_attribute, True) = False Then
                            Exit Sub
                        End If
                    End If
                    REM add attribute to class in memory
                    Me.container.oClasses.classes(i).attributes.add(ocls_attribute)

                Else
                    Dim pco As Integer = 0
                    Dim pi As Integer = 0
                    For i = 0 To Me.container.oClasses.classes(idx).parent_links.count - 1
                        If Me.container.oClasses.classes(idx).parent_links(i).schema_id = Me.schema_id Then
                            If pco = view.tattributelinks.SelectedIndex - 1 Then
                                pi = i
                                Exit For
                            End If
                            pco = pco + 1
                        End If
                    Next
                    Dim olinkcls_attribute = New bc_om_class_link_attribute
                    olinkcls_attribute.attribute_id = ocls_attribute.attribute_id
                    olinkcls_attribute.order = ocls_attribute.order
                    olinkcls_attribute.schema_id = Me.schema_id
                    olinkcls_attribute.class_id = ocls_attribute.class_id
                    olinkcls_attribute.parent_class_id = Me.container.oClasses.classes(idx).parent_links(pi).parent_class_id
                    olinkcls_attribute.write_mode = bc_om_class_attribute.INSERT_ATTRIBUTE
                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        olinkcls_attribute.db_write()
                    Else
                        olinkcls_attribute.tmode = bc_om_class_attribute.tWRITE
                        If olinkcls_attribute.transmit_to_server_and_receive(olinkcls_attribute, True) = False Then
                            Exit Sub
                        End If
                    End If
                    REM linked attribute

                    Me.container.oClasses.classes(idx).parent_links(pi).linked_attributes.add(ocls_attribute)
                End If
                REM redraw dialogue
                load_class_attributes(Me.class_name)

                view.tattributelinks.SelectedIndex = ts

            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("OkToolStripMenuItem6", "Click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            view.Cursor = Cursors.Default
        End Try

    End Sub
    Public Sub toggle_read_only()
        Try
            Dim cidx As Integer
            cidx = sel_cls_idx(get_real_class_name(view.ttaxonomy.SelectedNode.Text))



            Dim ordatt As New bc_om_class_attribute
            Dim idx As Integer
            idx = view.lvwatt.SelectedItems(0).Index
            If view.tattributelinks.SelectedIndex = 0 Then
                For i = 0 To Me.container.oClasses.classes.Count - 1
                    If Me.container.oClasses.classes(i).class_name = Me.class_name Then
                        If Me.container.oClasses.classes(i).attributes(view.lvwatt.SelectedItems(0).Index).permission = 1 Then
                            Me.container.oClasses.classes(i).attributes(view.lvwatt.SelectedItems(0).Index).permission = 3
                        Else
                            Me.container.oClasses.classes(i).attributes(view.lvwatt.SelectedItems(0).Index).permission = 1
                        End If
                        Me.container.oClasses.classes(i).attributes(view.lvwatt.SelectedItems(0).Index).class_id = Me.container.oClasses.classes(i).class_id
                        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                            Me.container.oClasses.classes(i).attributes(view.lvwatt.SelectedItems(0).Index).db_write()
                        Else
                            Me.container.oClasses.classes(i).attributes(view.lvwatt.SelectedItems(0).Index).tmode = bc_cs_soap_base_class.tWRITE
                            If Me.container.oClasses.classes(i).attributes(view.lvwatt.SelectedItems(0).Index).transmit_to_server_and_receive(Me.container.oClasses.classes(i).attributes(view.lvwatt.SelectedItems(0).Index), True) = False Then
                                Exit Sub
                            End If
                        End If
                        load_class_attributes(Me.class_name)
                        view.lvwatt.Items(idx).Selected = True
                        Exit For
                    End If
                Next
            Else

                For i = 0 To Me.container.oClasses.classes.Count - 1
                    If Me.container.oClasses.classes(i).class_name = Me.class_name Then
                        Dim pco As Integer = 0
                        Dim pi As Integer = 0
                        For k = 0 To Me.container.oClasses.classes(cidx).parent_links.count - 1
                            If Me.container.oClasses.classes(cidx).parent_links(k).schema_id = Me.schema_id Then
                                If pco = view.tattributelinks.SelectedIndex - 1 Then
                                    pi = k
                                    Exit For
                                End If
                                pco = pco + 1
                            End If
                        Next

                        Dim oclslink_attribute As New bc_om_class_link_attribute
                        oclslink_attribute.order = Me.container.oClasses.classes(i).parent_links(pi).linked_attributes(view.lvwatt.SelectedItems(0).Index).order
                        oclslink_attribute.attribute_id = Me.container.oClasses.classes(i).parent_links(pi).linked_attributes(view.lvwatt.SelectedItems(0).Index).attribute_id
                        oclslink_attribute.class_id = Me.container.oClasses.classes(i).class_id



                        oclslink_attribute.schema_id = Me.schema_id
                        oclslink_attribute.parent_class_id = Me.container.oClasses.classes(i).parent_links(pi).parent_class_id

                        If Me.container.oClasses.classes(i).parent_links(pi).linked_attributes(view.lvwatt.SelectedItems(0).Index).permission = 1 Then
                            oclslink_attribute.permission = 3
                            Me.container.oClasses.classes(i).parent_links(pi).linked_attributes(view.lvwatt.SelectedItems(0).Index).permission = 3
                        Else
                            oclslink_attribute.permission = 1
                            Me.container.oClasses.classes(i).parent_links(pi).linked_attributes(view.lvwatt.SelectedItems(0).Index).permission = 1
                        End If


                        oclslink_attribute.write_mode = bc_om_class_link_attribute.INSERT_ATTRIBUTE


                        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                            oclslink_attribute.db_write()
                        Else
                            oclslink_attribute.tmode = bc_cs_soap_base_class.tWRITE
                            If oclslink_attribute.transmit_to_server_and_receive(oclslink_attribute, True) = False Then
                                Exit Sub
                            End If
                        End If
                        Dim ts As Integer
                        ts = view.tattributelinks.SelectedIndex
                        load_class_attributes(Me.class_name)
                        view.tattributelinks.SelectedIndex = ts
                        view.lvwatt.Items(idx).Selected = True
                        Exit For
                    End If
                Next
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("toggle_read_only", "Click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

        End Try



    End Sub
    Public Sub move_attribute_up()
        Try
            Dim cidx As Integer
            cidx = sel_cls_idx(get_real_class_name(view.ttaxonomy.SelectedNode.Text))

            REM move attrbute up in order

            Dim ordatt As New bc_om_class_attribute
            Dim ord As Integer
            Dim idx As Integer
            Dim oclslink_attribute As bc_om_class_link_attribute
            idx = view.lvwatt.SelectedItems(0).Index
            If view.tattributelinks.SelectedIndex = 0 Then
                For i = 0 To Me.container.oClasses.classes.Count - 1
                    If Me.container.oClasses.classes(i).class_name = Me.class_name Then
                        ordatt = Me.container.oClasses.classes(i).attributes(view.lvwatt.SelectedItems(0).Index)
                        ord = ordatt.order
                        Me.container.oClasses.classes(i).attributes(view.lvwatt.SelectedItems(0).Index).order = Me.container.oClasses.classes(i).attributes(view.lvwatt.SelectedItems(0).Index - 1).order
                        Me.container.oClasses.classes(i).attributes(view.lvwatt.SelectedItems(0).Index - 1).order = ord
                        Me.container.oClasses.classes(i).attributes(view.lvwatt.SelectedItems(0).Index).class_id = Me.container.oClasses.classes(i).class_id
                        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                            Me.container.oClasses.classes(i).attributes(view.lvwatt.SelectedItems(0).Index).db_write()
                        Else
                            Me.container.oClasses.classes(i).attributes(view.lvwatt.SelectedItems(0).Index).tmode = bc_cs_soap_base_class.tWRITE
                            If Me.container.oClasses.classes(i).attributes(view.lvwatt.SelectedItems(0).Index).transmit_to_server_and_receive(Me.container.oClasses.classes(i).attributes(view.lvwatt.SelectedItems(0).Index), True) = False Then
                                Exit Sub
                            End If
                        End If
                        Me.container.oClasses.classes(i).attributes(view.lvwatt.SelectedItems(0).Index - 1).class_id = Me.container.oClasses.classes(i).class_id
                        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                            Me.container.oClasses.classes(i).attributes(view.lvwatt.SelectedItems(0).Index - 1).db_write()
                        Else
                            Me.container.oClasses.classes(i).attributes(view.lvwatt.SelectedItems(0).Index - 1).tmode = bc_cs_soap_base_class.tWRITE
                            If Me.container.oClasses.classes(i).attributes(view.lvwatt.SelectedItems(0).Index - 1).transmit_to_server_and_receive(Me.container.oClasses.classes(i).attributes(view.lvwatt.SelectedItems(0).Index - 1), True) = False Then
                                Exit Sub
                            End If
                        End If

                        Me.container.oClasses.classes(i).attributes.removeat(idx)
                        Me.container.oClasses.classes(i).attributes.insert(idx - 1, ordatt)
                        load_class_attributes(Me.class_name)
                        view.lvwatt.Items(idx - 1).Selected = True
                        Exit For
                    End If
                Next
            Else
                For i = 0 To Me.container.oClasses.classes.Count - 1
                    If Me.container.oClasses.classes(i).class_name = Me.class_name Then
                        Dim pco As Integer = 0
                        Dim pi As Integer = 0
                        For k = 0 To Me.container.oClasses.classes(cidx).parent_links.count - 1
                            If Me.container.oClasses.classes(cidx).parent_links(k).schema_id = Me.schema_id Then
                                If pco = view.tattributelinks.SelectedIndex - 1 Then
                                    pi = k
                                    Exit For
                                End If
                                pco = pco + 1
                            End If
                        Next
                        ordatt = Me.container.oClasses.classes(i).parent_links(pi).linked_attributes(view.lvwatt.SelectedItems(0).Index)
                        ord = ordatt.order
                        Me.container.oClasses.classes(i).parent_links(pi).linked_attributes(view.lvwatt.SelectedItems(0).Index).order = Me.container.oClasses.classes(i).parent_links(pi).linked_attributes(view.lvwatt.SelectedItems(0).Index - 1).order
                        Me.container.oClasses.classes(i).parent_links(pi).linked_attributes(view.lvwatt.SelectedItems(0).Index - 1).order = ord
                        Me.container.oClasses.classes(i).parent_links(pi).linked_attributes(view.lvwatt.SelectedItems(0).Index).class_id = Me.container.oClasses.classes(i).class_id
                        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                            Me.container.oClasses.classes(i).parent_links(pi).linked_attributes(view.lvwatt.SelectedItems(0).Index).db_write()
                        Else
                            Me.container.oClasses.classes(i).parent_links(pi).linked_attributes(view.lvwatt.SelectedItems(0).Index).tmode = bc_cs_soap_base_class.tWRITE
                            If Me.container.oClasses.classes(i).parent_links(pi).linked_attributes(view.lvwatt.SelectedItems(0).Index).transmit_to_server_and_receive(Me.container.oClasses.classes(i).parent_links(pi).linked_attributes(view.lvwatt.SelectedItems(0).Index), True) = False Then
                                Exit Sub
                            End If
                        End If

                        oclslink_attribute = New bc_om_class_link_attribute
                        oclslink_attribute.order = Me.container.oClasses.classes(i).parent_links(pi).linked_attributes(view.lvwatt.SelectedItems(0).Index).order
                        oclslink_attribute.attribute_id = Me.container.oClasses.classes(i).parent_links(pi).linked_attributes(view.lvwatt.SelectedItems(0).Index).attribute_id
                        oclslink_attribute.class_id = Me.container.oClasses.classes(i).parent_links(pi).linked_attributes(view.lvwatt.SelectedItems(0).Index).class_id
                        oclslink_attribute.schema_id = Me.schema_id
                        oclslink_attribute.parent_class_id = Me.container.oClasses.classes(i).parent_links(pi).parent_class_id
                        oclslink_attribute.write_mode = bc_om_class_link_attribute.INSERT_ATTRIBUTE
                        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                            oclslink_attribute.db_write()
                        Else
                            oclslink_attribute.tmode = bc_cs_soap_base_class.tWRITE
                            If oclslink_attribute.transmit_to_server_and_receive(oclslink_attribute, True) = False Then
                                Exit Sub
                            End If
                        End If
                        Me.container.oClasses.classes(i).parent_links(pi).linked_attributes(view.lvwatt.SelectedItems(0).Index - 1).class_id = Me.container.oClasses.classes(i).class_id
                        oclslink_attribute = New bc_om_class_link_attribute
                        oclslink_attribute.order = Me.container.oClasses.classes(i).parent_links(pi).linked_attributes(view.lvwatt.SelectedItems(0).Index - 1).order
                        oclslink_attribute.attribute_id = Me.container.oClasses.classes(i).parent_links(pi).linked_attributes(view.lvwatt.SelectedItems(0).Index - 1).attribute_id
                        oclslink_attribute.class_id = Me.container.oClasses.classes(i).parent_links(pi).linked_attributes(view.lvwatt.SelectedItems(0).Index - 1).class_id
                        oclslink_attribute.schema_id = Me.schema_id
                        oclslink_attribute.parent_class_id = Me.container.oClasses.classes(i).parent_links(pi).parent_class_id
                        oclslink_attribute.write_mode = bc_om_class_link_attribute.INSERT_ATTRIBUTE
                        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                            oclslink_attribute.db_write()
                        Else
                            oclslink_attribute.tmode = bc_cs_soap_base_class.tWRITE
                            If oclslink_attribute.transmit_to_server_and_receive(oclslink_attribute, True) = False Then
                                Exit Sub
                            End If
                        End If
                        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                            Me.container.oClasses.classes(i).parent_links(pi).linked_attributes(view.lvwatt.SelectedItems(0).Index - 1).db_write()
                        Else
                            Me.container.oClasses.classes(i).parent_links(pi).linked_attributes(view.lvwatt.SelectedItems(0).Index - 1).tmode = bc_cs_soap_base_class.tWRITE
                            If Me.container.oClasses.classes(i).parent_links(pi).linked_attributes(view.lvwatt.SelectedItems(0).Index - 1).transmit_to_server_and_receive(Me.container.oClasses.classes(i).parent_links(pi).linked_attributes(view.lvwatt.SelectedItems(0).Index - 1), True) = False Then
                                Exit Sub
                            End If
                        End If
                        Me.container.oClasses.classes(i).parent_links(pi).linked_attributes.removeat(idx)
                        Me.container.oClasses.classes(i).parent_links(pi).linked_attributes.insert(idx - 1, ordatt)
                        Dim ts As Integer
                        ts = view.tattributelinks.SelectedIndex
                        load_class_attributes(Me.class_name)
                        view.tattributelinks.SelectedIndex = ts
                        view.lvwatt.Items(idx - 1).Selected = True
                        Exit For
                    End If
                Next

            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("batup", "Click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

        End Try



    End Sub
    Public Sub move_attribute_down()
        Try
            Dim cidx As Integer
            cidx = sel_cls_idx(get_real_class_name(view.ttaxonomy.SelectedNode.Text))

            REM move attrbute up in order
            Dim ordatt As New bc_om_class_attribute
            Dim oclslink_attribute As New bc_om_class_link_attribute
            Dim ord As Integer
            Dim idx As Integer
            idx = view.lvwatt.SelectedItems(0).Index
            For i = 0 To Me.container.oClasses.classes.Count - 1
                If Me.container.oClasses.classes(i).class_name = Me.class_name Then
                    If view.tattributelinks.SelectedIndex = 0 Then
                        ordatt = Me.container.oClasses.classes(i).attributes(view.lvwatt.SelectedItems(0).Index)
                        ord = ordatt.order
                        Me.container.oClasses.classes(i).attributes(view.lvwatt.SelectedItems(0).Index).order = Me.container.oClasses.classes(i).attributes(view.lvwatt.SelectedItems(0).Index + 1).order
                        Me.container.oClasses.classes(i).attributes(view.lvwatt.SelectedItems(0).Index + 1).order = ord
                        Me.container.oClasses.classes(i).attributes(view.lvwatt.SelectedItems(0).Index).class_id = Me.container.oClasses.classes(i).class_id
                        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                            Me.container.oClasses.classes(i).attributes(view.lvwatt.SelectedItems(0).Index).db_write()
                        Else
                            Me.container.oClasses.classes(i).attributes(view.lvwatt.SelectedItems(0).Index).tmode = bc_cs_soap_base_class.tWRITE
                            If Me.container.oClasses.classes(i).attributes(view.lvwatt.SelectedItems(0).Index).transmit_to_server_and_receive(Me.container.oClasses.classes(i).attributes(view.lvwatt.SelectedItems(0).Index), True) = False Then
                                Exit Sub
                            End If
                        End If
                        Me.container.oClasses.classes(i).attributes(view.lvwatt.SelectedItems(0).Index + 1).class_id = Me.container.oClasses.classes(i).class_id
                        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                            Me.container.oClasses.classes(i).attributes(view.lvwatt.SelectedItems(0).Index + 1).db_write()
                        Else
                            Me.container.oClasses.classes(i).attributes(view.lvwatt.SelectedItems(0).Index + 1).tmode = bc_cs_soap_base_class.tWRITE
                            If Me.container.oClasses.classes(i).attributes(view.lvwatt.SelectedItems(0).Index + 1).transmit_to_server_and_receive(Me.container.oClasses.classes(i).attributes(view.lvwatt.SelectedItems(0).Index + 1), True) = False Then
                                Exit Sub
                            End If
                        End If
                        Me.container.oClasses.classes(i).attributes.removeat(idx)
                        Me.container.oClasses.classes(i).attributes.insert(idx + 1, ordatt)
                        load_class_attributes(Me.class_name)
                        view.lvwatt.Items(idx + 1).Selected = True
                        Exit For
                    Else
                        Dim pi As Integer = 0
                        Dim pco As Integer = 0
                        For k = 0 To Me.container.oClasses.classes(cidx).parent_links.count - 1
                            If Me.container.oClasses.classes(cidx).parent_links(k).schema_id = Me.schema_id Then
                                If pco = view.tattributelinks.SelectedIndex - 1 Then
                                    pi = k
                                    Exit For
                                End If
                                pco = pco + 1
                            End If
                        Next
                        ordatt = Me.container.oClasses.classes(i).parent_links(pi).linked_attributes(view.lvwatt.SelectedItems(0).Index)
                        ord = ordatt.order
                        Me.container.oClasses.classes(i).parent_links(pi).linked_attributes(view.lvwatt.SelectedItems(0).Index).order = Me.container.oClasses.classes(i).parent_links(pi).linked_attributes(view.lvwatt.SelectedItems(0).Index + 1).order
                        Me.container.oClasses.classes(i).parent_links(pi).linked_attributes(view.lvwatt.SelectedItems(0).Index + 1).order = ord
                        Me.container.oClasses.classes(i).parent_links(pi).linked_attributes(view.lvwatt.SelectedItems(0).Index).class_id = Me.container.oClasses.classes(i).class_id
                        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                            Me.container.oClasses.classes(i).parent_links(pi).linked_attributes(view.lvwatt.SelectedItems(0).Index).db_write()
                        Else
                            Me.container.oClasses.classes(i).parent_links(pi).linked_attributes(view.lvwatt.SelectedItems(0).Index).tmode = bc_cs_soap_base_class.tWRITE
                            If Me.container.oClasses.classes(i).parent_links(pi).linked_attributes(view.lvwatt.SelectedItems(0).Index).tranmit_to_server_and_receive(Me.container.oClasses.classes(i).parent_links(pi).linked_attributes(view.lvwatt.SelectedItems(0).Index), True) = False Then
                                Exit Sub
                            End If
                        End If

                        oclslink_attribute = New bc_om_class_link_attribute
                        oclslink_attribute.order = Me.container.oClasses.classes(i).parent_links(pi).linked_attributes(view.lvwatt.SelectedItems(0).Index).order
                        oclslink_attribute.attribute_id = Me.container.oClasses.classes(i).parent_links(pi).linked_attributes(view.lvwatt.SelectedItems(0).Index).attribute_id
                        oclslink_attribute.class_id = Me.container.oClasses.classes(i).parent_links(pi).linked_attributes(view.lvwatt.SelectedItems(0).Index).class_id
                        oclslink_attribute.schema_id = Me.schema_id
                        oclslink_attribute.parent_class_id = Me.container.oClasses.classes(i).parent_links(pi).parent_class_id
                        oclslink_attribute.write_mode = bc_om_class_link_attribute.INSERT_ATTRIBUTE
                        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                            oclslink_attribute.db_write()
                        Else
                            oclslink_attribute.tmode = bc_cs_soap_base_class.tWRITE
                            If oclslink_attribute.transmit_to_server_and_receive(oclslink_attribute, True) = False Then
                                Exit Sub
                            End If
                        End If

                        Me.container.oClasses.classes(i).parent_links(pi).linked_attributes(view.lvwatt.SelectedItems(0).Index + 1).class_id = Me.container.oClasses.classes(i).class_id

                        oclslink_attribute = New bc_om_class_link_attribute
                        oclslink_attribute.order = Me.container.oClasses.classes(i).parent_links(pi).linked_attributes(view.lvwatt.SelectedItems(0).Index + 1).order
                        oclslink_attribute.attribute_id = Me.container.oClasses.classes(i).parent_links(pi).linked_attributes(view.lvwatt.SelectedItems(0).Index + 1).attribute_id
                        oclslink_attribute.class_id = Me.container.oClasses.classes(i).parent_links(pi).linked_attributes(view.lvwatt.SelectedItems(0).Index + 1).class_id
                        oclslink_attribute.schema_id = Me.schema_id
                        oclslink_attribute.parent_class_id = Me.container.oClasses.classes(i).parent_links(pi).parent_class_id
                        oclslink_attribute.write_mode = bc_om_class_link_attribute.INSERT_ATTRIBUTE
                        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                            oclslink_attribute.db_write()
                            Me.container.oClasses.classes(i).parent_links(pi).linked_attributes(view.lvwatt.SelectedItems(0).Index + 1).db_write()

                        Else
                            oclslink_attribute.tmode = bc_cs_soap_base_class.tWRITE
                            If oclslink_attribute.transmit_to_server_and_receive(oclslink_attribute, True) = False Then
                                Exit Sub
                            End If
                            Me.container.oClasses.classes(i).parent_links(pi).linked_attributes(view.lvwatt.SelectedItems(0).Index + 1).tmode = bc_cs_soap_base_class.tWRITE
                            If Me.container.oClasses.classes(i).parent_links(pi).linked_attributes(view.lvwatt.SelectedItems(0).Index + 1).transmit_to_server_and_receive(Me.container.oClasses.classes(i).parent_links(pi).linked_attributes(view.lvwatt.SelectedItems(0).Index + 1), True) = False Then
                                Exit Sub
                            End If

                        End If

                        Me.container.oClasses.classes(i).parent_links(pi).linked_attributes.removeat(idx)
                        Me.container.oClasses.classes(i).parent_links(pi).linked_attributes.insert(idx + 1, ordatt)
                        Dim ts As Integer
                        ts = view.tattributelinks.SelectedIndex
                        load_class_attributes(Me.class_name)
                        view.tattributelinks.SelectedIndex = ts
                        view.lvwatt.Items(idx + 1).Selected = True
                        Exit For
                    End If
                End If
            Next

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("batdn", "Click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
        End Try
    End Sub
    Public Sub remove_attribute_from_class()
        Try
            Dim cidx As Integer
            cidx = sel_cls_idx(get_real_class_name(view.ttaxonomy.SelectedNode.Text))

            If view.lvwatt.SelectedItems(0).Index = -1 Then
                Exit Sub
            End If
            Dim omsg As bc_cs_message
            If view.tattributelinks.SelectedIndex = 0 Then
                omsg = New bc_cs_message("Blue Curve", "Are you sure you wish to remove attribute: " + view.lvwatt.SelectedItems(0).Text + " from being associated to " + Me.class_name, bc_cs_message.MESSAGE, True, False, "Yes", "no", True)
            Else
                omsg = New bc_cs_message("Blue Curve", "Are you sure you wish to remove attribute: " + view.lvwatt.SelectedItems(0).Text + " from being associated to " + Me.class_name + " " + view.tattributelinks.SelectedTab.Text, bc_cs_message.MESSAGE, True, False, "Yes", "no", True)
            End If
            If omsg.cancel_selected = True Then
                view.lvwatt.SelectedItems.Clear()
                Exit Sub
            End If
            Dim i As Integer
            Dim ocls_attribute As New bc_om_class_attribute
            Dim oclslink_attribute As New bc_om_class_link_attribute
            For i = 0 To Me.container.oClasses.classes.Count - 1
                If Me.container.oClasses.classes(i).class_name = Me.class_name Then
                    ocls_attribute.class_id = Me.container.oClasses.classes(i).class_id
                    oclslink_attribute.class_id = Me.container.oClasses.classes(i).class_id
                    Exit For
                End If
            Next
            For j = 0 To Me.container.oClasses.attribute_pool.Count - 1
                If Me.container.oClasses.attribute_pool(j).name = view.lvwatt.SelectedItems(0).Text Then
                    ocls_attribute.attribute_id = Me.container.oClasses.attribute_pool(j).attribute_id
                    oclslink_attribute.attribute_id = Me.container.oClasses.attribute_pool(j).attribute_id
                    Exit For
                End If
            Next
            Dim ts As Integer
            ts = view.tattributelinks.SelectedIndex


            If view.tattributelinks.SelectedIndex = 0 Then
                ocls_attribute.write_mode = bc_om_class_attribute.DELETE_ATTRIBUTE
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    ocls_attribute.db_write()
                Else
                    ocls_attribute.tmode = bc_cs_soap_base_class.tWRITE
                    If ocls_attribute.transmit_to_server_and_receive(ocls_attribute, True) = False Then
                        Exit Sub
                    End If
                End If

                REM remove attribute from class in memory
                Me.container.oClasses.classes(i).attributes.removeat(view.lvwatt.SelectedItems(0).Index)
            Else
                Dim pi As Integer = 0
                Dim pco As Integer = 0
                For k = 0 To Me.container.oClasses.classes(cidx).parent_links.count - 1
                    If Me.container.oClasses.classes(cidx).parent_links(k).schema_id = Me.schema_id Then
                        If pco = view.tattributelinks.SelectedIndex - 1 Then
                            pi = k
                            Exit For
                        End If
                        pco = pco + 1
                    End If
                Next
                oclslink_attribute.schema_id = Me.schema_id
                oclslink_attribute.parent_class_id = Me.container.oClasses.classes(cidx).parent_links(pi).parent_class_id
                oclslink_attribute.write_mode = bc_om_class_attribute.DELETE_ATTRIBUTE
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    oclslink_attribute.db_write()
                Else
                    oclslink_attribute.tmode = bc_cs_soap_base_class.tWRITE
                    If oclslink_attribute.transmit_to_server_and_receive(oclslink_attribute, True) = False Then
                        Exit Sub
                    End If
                End If

                Me.container.oClasses.classes(cidx).parent_links(pi).linked_attributes.removeat(view.lvwatt.SelectedItems(0).Index)
            End If
            REM redraw dialogue
            load_class_attributes(Me.class_name)
            view.RemoveAttributeFromClassToolStripMenuItem.Text = "Remove attribute from class"
            view.RemoveAttributeFromClassToolStripMenuItem.Enabled = False
            view.tattributelinks.SelectedIndex = ts
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("RemoveAttributeFromClassToolStripMenuItem", "Click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.load_class_toolbar()

        End Try


    End Sub
    REM ING JUNE 2012
    Public Sub add_new_attribute_menu()
        Dim fedit As New bc_am_cp_edit
        fedit.Height = 518
        fedit.ltitle.Text = "Add New Attribute to " + get_real_class_name(view.ttaxonomy.SelectedNode.Text)
        If view.tattributelinks.SelectedIndex > 0 Then
            fedit.ltitle.Text = fedit.ltitle.Text + " " + get_real_class_name(view.tattributelinks.SelectedTab.Text)
        End If
        fedit.Cdt.Items.Add("String")
        fedit.Cdt.Items.Add("Number")
        fedit.Cdt.Items.Add("Boolean")
        fedit.Cdt.Items.Add("Date")
        fedit.Cdt.SelectedIndex = 0
        fedit.Tentry.Visible = False
        fedit.centry.Visible = False
        fedit.uxDetails.Visible = True
        'fedit.Height = fedit.Height + fedit.uxDetails.Height
        fedit.ShowDialog()
        If fedit.cancel_selected = True Then
            Exit Sub
        Else
            view.Cursor = Cursors.WaitCursor
            add_new_attribute(fedit.Tattname.Text, fedit.Cdt.SelectedIndex, fedit.Caudit.Checked, fedit.cmandatory.Checked, fedit.Cworkflow.Checked, fedit.clookup.Checked, fedit.Tsql.Text, fedit.Tlength.Text, fedit.cpopup.Checked, fedit.chkdef.Checked, fedit.defsql.Text)
        End If
    End Sub
    REM ING JUNE 2012
    Private Sub add_new_attribute(ByVal name As String, ByVal dt As Integer, ByVal audit As Boolean, ByVal mandatory As Boolean, ByVal workflow As Boolean, ByVal lookup As Boolean, ByVal sql As String, ByVal length As String, ByVal popup As Boolean, def As Boolean, defsql As String)
        Try


            REM check attribute name is not already in use
            For i = 0 To Me.container.oClasses.attribute_pool.Count - 1
                If Trim(UCase(Me.container.oClasses.attribute_pool(i).name)) = Trim(UCase(name)) Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Attribute: " + name + " already exists please enter again!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub
                End If
            Next
            Dim oatt As New bc_om_attribute
            Dim cidx As Integer
            cidx = sel_cls_idx(view.ttaxonomy.SelectedNode.Text)
            oatt.repeats = 0

            oatt.name = name
            Select Case dt
                Case 0
                    oatt.type_id = 1
                    If IsNumeric(length) = True Then
                        oatt.length = CInt(length)
                    Else
                        oatt.length = 255
                    End If
                    If popup = True Then
                        oatt.repeats = 1
                    End If
                Case 1
                    oatt.type_id = 2
                Case 2
                    oatt.type_id = 3
                Case 3
                    oatt.type_id = 5
            End Select
            If audit = True Then
                oatt.submission_code = 2
            Else
                oatt.submission_code = 1
            End If
            oatt.nullable = True
            If mandatory = True Then
                oatt.nullable = False
            End If
            oatt.show_workflow = False
            If workflow = True Then
                oatt.show_workflow = 1
            End If
            oatt.is_lookup = False
            If lookup = True Then
                oatt.is_lookup = True
                oatt.lookup_sql = sql
            End If
            If def = True Then
                oatt.is_def = True
                oatt.def_sql = defsql
            Else
                oatt.is_def = False
                oatt.def_sql = ""
            End If

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                oatt.db_write()
            Else
                oatt.tmode = bc_cs_soap_base_class.tWRITE
                If oatt.transmit_to_server_and_receive(oatt, True) = False Then
                    Exit Sub
                End If

            End If
            REM if lookup reload attributes to get lookup
            If oatt.is_lookup Then
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    Me.container.oClasses.db_read()
                Else
                    Me.container.oClasses.tmode = bc_cs_soap_base_class.tREAD
                    If Me.container.oClasses.transmit_to_server_and_receive(Me.container.oClasses, True) = False Then
                        Exit Sub
                    End If
                End If
            Else
                REM now assign to pool if not read back
                Me.container.oClasses.attribute_pool.Add(oatt)
            End If


            REM now assign to class
            For i = 0 To Me.container.oClasses.classes.Count - 1
                If Me.container.oClasses.classes(i).class_name = Me.class_name Then
                    Dim ocls_attribute As New bc_om_class_attribute
                    ocls_attribute.attribute_id = oatt.attribute_id
                    ocls_attribute.order = view.lvwatt.Items.Count + 1
                    ocls_attribute.class_id = Me.container.oClasses.classes(i).class_id
                    If view.tattributelinks.SelectedIndex = 0 Then
                        ocls_attribute.write_mode = bc_om_class_attribute.INSERT_ATTRIBUTE
                        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                            ocls_attribute.db_write()
                        Else
                            ocls_attribute.tmode = bc_cs_soap_base_class.tWRITE
                            If ocls_attribute.transmit_to_server_and_receive(ocls_attribute, True) = False Then
                                Exit Sub
                            End If
                        End If
                        REM add attribute to class in memory
                        Me.container.oClasses.classes(i).attributes.add(ocls_attribute)
                    Else
                        Dim pco As Integer = 0
                        Dim pi As Integer = 0
                        For k = 0 To Me.container.oClasses.classes(cidx).parent_links.count - 1
                            If Me.container.oClasses.classes(cidx).parent_links(k).schema_id = Me.schema_id Then
                                If pco = view.tattributelinks.SelectedIndex - 1 Then
                                    pi = k
                                    Exit For
                                End If
                                pco = pco + 1
                            End If
                        Next
                        Dim oclaslink_attribute As New bc_om_class_link_attribute
                        oclaslink_attribute.attribute_id = oatt.attribute_id
                        oclaslink_attribute.order = view.lvwatt.Items.Count + 1
                        oclaslink_attribute.class_id = Me.container.oClasses.classes(i).class_id
                        oclaslink_attribute.schema_id = Me.schema_id
                        oclaslink_attribute.parent_class_id = Me.container.oClasses.classes(i).parent_links(pi).parent_class_id
                        oclaslink_attribute.write_mode = bc_om_class_attribute.INSERT_ATTRIBUTE
                        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                            oclaslink_attribute.db_write()
                        Else
                            oclaslink_attribute.tmode = bc_cs_soap_base_class.tWRITE
                            If oclaslink_attribute.transmit_to_server_and_receive(oclaslink_attribute, True) = False Then
                                Exit Sub
                            End If
                        End If

                        Me.container.oClasses.classes(i).parent_links(pi).linked_attributes.add(ocls_attribute)
                    End If
                    Exit For
                End If
            Next

            REM redraw dialogue
            Dim ts As Integer
            ts = view.tattributelinks.SelectedIndex
            load_class_attributes(Me.class_name)
            view.tattributelinks.SelectedIndex = ts
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("mokadminattribute", "Click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

        End Try

    End Sub
    Public Sub modify_attribute_menu()
        Dim fedit As New bc_am_cp_edit
        fedit.Height = 518
        fedit.ltitle.Text = "Modify Attribute " + view.lvwatt.SelectedItems(0).Text

        If view.tattributelinks.SelectedIndex > 0 Then
            fedit.ltitle.Text = fedit.ltitle.Text + " " + view.tattributelinks.SelectedTab.Text
        End If
        fedit.Tentry.Visible = False
        fedit.centry.Visible = False
        fedit.uxDetails.Visible = True
        For i = 0 To Me.container.oClasses.attribute_pool.Count - 1
            If Me.container.oClasses.attribute_pool(i).name = view.lvwatt.SelectedItems(0).Text Then
                fedit.Tattname.Text = Me.container.oClasses.attribute_pool(i).name
                If Me.container.oClasses.attribute_pool(i).show_workflow = 1 Then
                    fedit.Cworkflow.Checked = True
                End If
                If Me.container.oClasses.attribute_pool(i).nullable = 0 Then
                    fedit.cmandatory.Checked = True
                End If
                If Me.container.oClasses.attribute_pool(i).submission_code = 2 Then
                    fedit.Caudit.Checked = True
                End If

                If Me.container.oClasses.attribute_pool(i).is_lookup = True Then
                    fedit.clookup.Checked = True
                    fedit.Tsql.Text = Me.container.oClasses.attribute_pool(i).lookup_Sql
                End If
                If Me.container.oClasses.attribute_pool(i).is_def = True Then
                    fedit.chkdef.Checked = True
                    fedit.defsql.Text = Me.container.oClasses.attribute_pool(i).def_sql
                End If
                fedit.Cdt.Items.Add("String")
                fedit.Cdt.Items.Add("Number")
                fedit.Cdt.Items.Add("Boolean")
                fedit.Cdt.Items.Add("Date")
                Select Case Me.container.oClasses.attribute_pool(i).type_id
                    Case 1
                        fedit.Cdt.SelectedIndex = 0
                        fedit.Tlength.Text = Me.container.oClasses.attribute_pool(i).length
                        If Me.container.oClasses.attribute_pool(i).repeats = 1 Then
                            fedit.cpopup.Checked = True
                        End If
                    Case 2
                        fedit.Cdt.SelectedIndex = 1
                    Case 3
                        fedit.Cdt.SelectedIndex = 2
                    Case 5
                        fedit.Cdt.SelectedIndex = 3
                End Select
                Exit For
            End If
        Next

        'fedit.Height = fedit.Height + fedit.uxDetails.Height

        fedit.ShowDialog()
        If fedit.cancel_selected = True Then
            Exit Sub
        Else
            view.Cursor = Cursors.WaitCursor
            modify_attribute(fedit.Tattname.Text, fedit.Cdt.SelectedIndex, fedit.Caudit.Checked, fedit.cmandatory.Checked, fedit.Cworkflow.Checked, fedit.clookup.Checked, fedit.Tsql.Text, fedit.Tlength.Text, fedit.cpopup.Checked, fedit.chkdef.Checked, fedit.defsql.Text)
            view.Cursor = Cursors.Default

        End If
    End Sub
    REM ING JUNE 2012
    Public Sub modify_attribute(ByVal name As String, ByVal dt As Integer, ByVal audit As Boolean, ByVal mandatory As Boolean, ByVal workflow As Boolean, ByVal lookup As Boolean, ByVal sql As String, ByVal length As String, ByVal popup As Boolean, def As Boolean, defsql As String)
        Try
            Dim cidx As Integer
            cidx = sel_cls_idx(view.ttaxonomy.SelectedNode.Text)

            Dim oatt As New bc_om_attribute
            REM get attribute id
            oatt.name = name
            If name = "" Then
                Dim omsg As New bc_cs_message("Blue Curve", "attribtute name must be entered.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            End If
            If view.tattributelinks.SelectedIndex = 0 Then
                For j = 0 To Me.container.oClasses.classes.Count - 1
                    If Me.container.oClasses.classes(j).class_name = Me.class_name Then
                        oatt.attribute_id = Me.container.oClasses.classes(j).attributes(view.lvwatt.SelectedItems(0).Index).attribute_id
                        Exit For
                    End If
                Next
            Else
                oatt.attribute_id = Me.container.oClasses.classes(cidx).parent_links(get_parent_class_index_from_attribute(cidx)).linked_attributes(view.lvwatt.SelectedItems(0).Index).attribute_id
            End If
            REM now check name isnt if an existing attribute that isnt the selected one
            For i = 0 To Me.container.oClasses.attribute_pool.Count - 1
                If Trim(UCase(Me.container.oClasses.attribute_pool(i).name)) = Trim(UCase(name)) And Trim(UCase(Me.container.oClasses.attribute_pool(i).name)) <> Trim(UCase(oatt.name)) Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Attribute name: " + name + " is in use by another attribute!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub
                End If
            Next

            Select Case dt
                Case 0
                    oatt.type_id = 1
                    If IsNumeric(length) = True Then
                        oatt.length = CInt(length)
                    Else
                        oatt.length = 255
                    End If
                    If popup = True Then
                        oatt.repeats = 1
                    End If
                Case 1
                    oatt.type_id = 2
                Case 2
                    oatt.type_id = 3
                Case 3
                    oatt.type_id = 5
            End Select
            If audit = True Then
                oatt.submission_code = 2
            Else
                oatt.submission_code = 1
            End If
            oatt.nullable = True
            If mandatory Then
                oatt.nullable = False
            End If
            oatt.show_workflow = False
            If workflow = True Then
                oatt.show_workflow = 1
            End If
            oatt.is_lookup = False
            If lookup = True Then
                oatt.is_lookup = True
                oatt.lookup_sql = sql
            End If
            If def = True Then
                oatt.is_def = True
                oatt.def_sql = defsql
            Else
                oatt.is_def = False
                oatt.def_sql = ""
            End If


            oatt.write_mode = bc_om_attribute.UPDATE
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                oatt.db_write()
            Else
                oatt.tmode = bc_cs_soap_base_class.tWRITE
                If oatt.transmit_to_server_and_receive(oatt, True) = False Then
                    Exit Sub
                End If
            End If
            REM if lookup reload attributes to get lookup
            If oatt.is_lookup Then
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    Me.container.oClasses.db_read()
                Else
                    Me.container.oClasses.tmode = bc_cs_soap_base_class.tREAD
                    If Me.container.oClasses.transmit_to_server_and_receive(Me.container.oClasses, True) = False Then
                        Exit Sub
                    End If
                End If

            Else
                REM now assign to pool if not read back
                For i = 0 To Me.container.oClasses.attribute_pool.Count - 1
                    If Me.container.oClasses.attribute_pool(i).attribute_id = oatt.attribute_id Then
                        Me.container.oClasses.attribute_pool(i) = oatt
                        Exit For
                    End If
                Next
            End If
            REM redraw dialogue
            Dim ts As Integer
            ts = view.tattributelinks.SelectedIndex
            load_class_attributes(Me.class_name)
            view.tattributelinks.SelectedIndex = ts

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("ToolStripMenuItems2", "Click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Public Sub delete_attribute()
        REM cannot delete an attribute if it is associated to a class or class link
        Try

            Dim str As String = ""
            Dim attribute_id As Long
            For i = 0 To Me.container.oClasses.attribute_pool.Count - 1
                If Me.container.oClasses.attribute_pool(i).name = view.lvwatt.SelectedItems(0).Text Then
                    attribute_id = Me.container.oClasses.attribute_pool(i).attribute_id
                    Exit For
                End If
            Next

            For i = 0 To Me.container.oClasses.classes.Count - 1
                If Me.container.oClasses.classes(i).class_name <> Me.class_name Or view.tattributelinks.SelectedIndex <> 0 Then
                    REM atributes
                    For j = 0 To Me.container.oClasses.classes(i).attributes.count - 1
                        If Me.container.oClasses.classes(i).attributes(j).attribute_id = attribute_id Then
                            If str = "" Then
                                str = Me.container.oClasses.classes(i).class_name
                            Else
                                str = str + ", " + Me.container.oClasses.classes(i).class_name
                            End If
                            Exit For
                        End If

                    Next
                End If
                REM parent link attributes
                Dim schema_name As String
                schema_name = ""
                For j = 0 To Me.container.oClasses.classes(i).parent_links.count - 1
                    For k = 0 To Me.container.oClasses.classes(i).parent_links(j).linked_attributes.count - 1
                        If Me.container.oClasses.classes(i).parent_links(j).linked_attributes(k).attribute_id = attribute_id Then
                            For m = 0 To Me.container.oClasses.classes.Count - 1
                                If Me.container.oClasses.classes(m).class_id = Me.container.oClasses.classes(i).parent_links(j).parent_class_id Then
                                    Dim tstr As String
                                    For n = 0 To Me.container.oClasses.schemas.Count - 1
                                        If Me.container.oClasses.schemas(n).schema_id = Me.container.oClasses.classes(i).parent_links(j).schema_Id Then
                                            schema_name = Me.container.oClasses.schemas(n).schema_name
                                            Exit For
                                        End If

                                    Next
                                    tstr = "linked to " + Me.container.oClasses.classes(m).class_name
                                    If tstr <> view.tattributelinks.SelectedTab.Text Then
                                        If str = "" Then
                                            str = Me.container.oClasses.classes(i).class_name + " linked to " + Me.container.oClasses.classes(m).class_name + "(schema: " + schema_name + ")"
                                        Else
                                            str = str + ", " + Me.container.oClasses.classes(i).class_name + " linked to " + Me.container.oClasses.classes(m).class_name + "(schema: " + schema_name + ")"
                                        End If
                                        Exit For
                                    End If
                                End If

                            Next
                            Exit For
                        End If
                    Next
                Next

            Next
            If str <> "" Then
                str = "Attribute: " + view.lvwatt.SelectedItems(0).Text + " is also associated to " + str + " please remove these associations prior to deleting the attribute"
                Dim omsg As New bc_cs_message("Blue Curve", str, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Sub
            Else
                Dim attribute_name As String
                attribute_name = view.lvwatt.SelectedItems(0).Text
                Dim omsg As New bc_cs_message("Blue Curve", "Are you sure you wish to delete attribute: " + view.lvwatt.SelectedItems(0).Text + " from system?", bc_cs_message.MESSAGE, True, False, "Yes", "no", True)

                If omsg.cancel_selected = True Then
                    Exit Sub
                Else
                    view.Cursor = Cursors.WaitCursor
                    Dim schema_name As String
                    schema_name = view.cschema.Text
                    Dim oatt As New bc_om_attribute
                    oatt.attribute_id = attribute_id
                    oatt.write_mode = bc_om_attribute.DELETE
                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        oatt.db_write()
                    Else
                        oatt.tmode = bc_cs_soap_base_class.tWRITE
                        If oatt.transmit_to_server_and_receive(oatt, True) = False Then
                            Exit Sub
                        End If
                    End If
                    If oatt.delete_error <> "" Then
                        omsg = New bc_cs_message("Blue Curve", "Failed to delete attribuite Database error: " + oatt.delete_error, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                        Exit Sub
                    End If
                    For i = 0 To Me.container.oClasses.attribute_pool.Count - 1
                        If Me.container.oClasses.attribute_pool(i).attribute_id = oatt.attribute_id Then
                            Me.container.oClasses.attribute_pool.RemoveAt(i)
                            Exit For
                        End If
                    Next
                    REM remove attribute from class in memory
                    For i = 0 To Me.container.oClasses.classes.Count - 1
                        If Me.container.oClasses.classes(i).class_name = Me.class_name Then
                            For k = 0 To Me.container.oClasses.classes(i).attributes.count - 1
                                If Me.container.oClasses.classes(i).attributes(k).attribute_id = oatt.attribute_id Then
                                    Me.container.oClasses.classes(i).attributes.removeat(k)
                                    Exit For
                                End If
                            Next
                            Exit For
                        End If
                    Next

                    omsg = New bc_cs_message("Blue Curve", "attribute " + attribute_name + " deleted.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    load_class_attributes(Me.class_name)
                    Me.load_class_toolbar()

                End If
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("DeleteAttributeToolStripMenuItem", "Click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

        End Try

    End Sub
    Public Sub load_entity_parts(ByVal name As String, ByVal class_name As String, Optional ByVal from_list As Boolean = True)
        Try

            Dim i As Integer
            Dim oentity As New bc_om_entity
            Dim lent_name As String
            Dim class_id As Long
            If InStr(name, "(inactive)") > 0 Then
                lent_name = name.Substring(0, InStr(name, "(inactive)") - 2)
            Else
                lent_name = name
            End If
            REM reset item column
            Dim dtextbox As New DataGridViewTextBoxColumn



            dtextbox.SortMode = DataGridViewColumnSortMode.NotSortable



            dtextbox.Width = view.DataGridView1.Columns(3).Width
            view.DataGridView1.Columns.RemoveAt(3)
            dtextbox.Name = "Value"


            view.DataGridView1.Columns.Insert(3, dtextbox)


            class_name = get_real_class_name(class_name)
            class_id = get_class_id(class_name)
            view.tattributes.SelectedIndex = 0



            If from_list = False Then

                load_associated_class_attributes(class_name)

                set_default_attribute_values(class_name, True)


                'view.tattributes.TabPages(0).Text = "attributes for: " + view.associated_entities(view.tassociations.SelectedIndex).name
                view.tattributes.TabPages(0).ImageIndex = attributes_icon

            End If

            If from_list = True Then

                view.warnings.Clear()
                For i = 1 To view.associated_entities.Count - 1
                    view.associated_entities.RemoveAt(i)
                Next
                'view.associated_entities.Clear()
                view.tassociations.TabPages(0).Text = view.Lentities.SelectedItems(0).Text
                If InStr(view.Lentities.SelectedItems(0).Text, "(inactive)") > 0 Then
                    view.tassociations.TabPages(0).ImageIndex = deactivate_icon
                Else
                    view.tassociations.TabPages(0).ImageIndex = selected_data_icon
                End If
                'view.tattributes.TabPages(0).Text = "attributes for: " + view.Lentities.SelectedItems(0).Text
                view.tattributes.TabPages(0).ImageIndex = attributes_icon
                For i = 1 To view.tattributes.TabPages.Count - 1
                    view.tattributes.TabPages.RemoveAt(1)
                Next

                For i = 1 To view.tassociations.TabPages.Count - 1
                    view.tassociations.TabPages.RemoveAt(1)
                Next
                For i = 0 To container.oEntities.entity.Count - 1
                    If container.oEntities.entity(i).class_id = class_id Then
                        If container.oEntities.entity(i).name = lent_name Then

                            view.associated_entities(view.tassociations.SelectedIndex).id = container.oEntities.entity(i).id
                            view.associated_entities(view.tassociations.SelectedIndex).inactive = container.oEntities.entity(i).inactive
                            For j = 0 To view.associated_entities(view.tassociations.SelectedIndex).attribute_values.count - 1
                                view.associated_entities(view.tassociations.SelectedIndex).attribute_values(j).value = ""
                                view.associated_entities(view.tassociations.SelectedIndex).attribute_values(j).last_update_comment = ""
                            Next
                            view.associated_entities(view.tassociations.SelectedIndex).schema_id = Me.schema_id

                            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                                view.associated_entities(view.tassociations.SelectedIndex).db_read()
                            Else

                                view.associated_entities(view.tassociations.SelectedIndex).tmode = bc_om_entity.tREAD

                                If view.associated_entities(view.tassociations.SelectedIndex).transmit_to_server_and_receive(view.associated_entities(view.tassociations.SelectedIndex), True) = False Then
                                    Exit Sub
                                End If
                            End If
                            view.associated_entities(view.tassociations.SelectedIndex).name = lent_name
                            view.associated_entities(view.tassociations.SelectedIndex).class_name = container.oEntities.entity(i).class_name

                            Exit For
                        End If
                    End If
                Next
            End If

            REM attribute values
            For j = 0 To view.associated_entities(view.tassociations.SelectedIndex).attribute_values.count - 1


                If view.associated_entities(view.tassociations.SelectedIndex).attribute_values(j).nullable = True Then
                    view.DataGridView1.Item(0, j).Value = New Drawing.Bitmap(1, 1)
                Else
                    view.DataGridView1.Item(0, j).Value = view.tabimages.Images(mandatory_icon)
                End If


                REM key value pairs 
                For k = 0 To container.oClasses.attribute_pool.Count - 1
                    If container.oClasses.attribute_pool(k).name = view.DataGridView1.Item(1, j).Value Then
                        If container.oClasses.attribute_pool(k).lookup_keys.count > 0 Then
                            REM find key value for value
                            For l = 0 To container.oClasses.attribute_pool(k).lookup_values.count - 1
                                If RTrim(LTrim(container.oClasses.attribute_pool(k).lookup_keys(l))) = RTrim(LTrim((view.associated_entities(view.tassociations.SelectedIndex).attribute_values(j).value))) Then
                                    view.associated_entities(view.tassociations.SelectedIndex).attribute_values(j).value = container.oClasses.attribute_pool(k).lookup_values(l)
                                    Exit For
                                End If
                            Next
                            If view.associated_entities(view.tassociations.SelectedIndex).attribute_values(j).show_workflow = True Then
                                For l = 0 To container.oClasses.attribute_pool(k).lookup_values.count - 1
                                    If RTrim(LTrim(container.oClasses.attribute_pool(k).lookup_keys(l))) = RTrim(LTrim((view.associated_entities(view.tassociations.SelectedIndex).attribute_values(j).published_value))) Then
                                        view.associated_entities(view.tassociations.SelectedIndex).attribute_values(j).Published_value = container.oClasses.attribute_pool(k).lookup_values(l)
                                        Exit For
                                    End If
                                Next
                            End If
                            Exit For
                        End If
                    End If
                Next

                REM ===================================


                view.DataGridView1.Item(3, j).Value = view.associated_entities(view.tassociations.SelectedIndex).attribute_values(j).value

                If view.associated_entities(view.tassociations.SelectedIndex).attribute_values(j).value_changed = True Then
                    view.DataGridView1.Item(0, j).Value = view.tabimages.Images(att_edited_icon)
                    'view.DataGridView1.Item(0, j).Value = view.tabimages.Images(selected_icon)
                End If

                If view.associated_entities(view.tassociations.SelectedIndex).attribute_values(j).show_workflow = False Then

                    view.DataGridView1.Item(5, j).Value = "n/a"
                    'view.DataGridView1.Item(4, j).Value = New Drawing.Bitmap(1, 1)

                Else
                    'view.DataGridView1.Item(4, j).Value = view.tabimages.Images(push_publish_icon)
                    If Not IsNothing(view.associated_entities(view.tassociations.SelectedIndex).attribute_values(j).published_value) Then
                        view.DataGridView1.Item(5, j).Value = view.associated_entities(view.tassociations.SelectedIndex).attribute_values(j).published_value
                    End If
                End If
            Next

            REM reset warnings icons
            Dim co As Integer
            Dim pco As Integer
            co = 0
            pco = 0

            REM now set warnings
            view.tattributes.TabPages(0).ImageIndex = attributes_icon

            view.lassociations.TabPages(0).ImageIndex = 21

            For i = 0 To view.warnings.Count - 1
                If view.warnings(i).association_tab = view.tassociations.SelectedIndex Then
                    If view.warnings(i).entity_tab > -1 Then
                        view.tentities.Nodes(view.warnings(i).entity_tab).imageindex = warning_icon
                    Else
                        If view.warnings(i).attribute_tab = 0 Then
                            view.tattributes.TabPages(0).ImageIndex = warning_icon
                            view.DataGridView1.Item(0, view.warnings(i).attribute).Value = view.tabimages.Images(warning_icon)
                        End If
                    End If
                End If

            Next
            Dim ocoom As New bc_cs_activity_log("TIMING", "H", bc_cs_activity_codes.COMMENTARY, Now)

            load_entity_view(True)

            ocoom = New bc_cs_activity_log("TIMING", "I", bc_cs_activity_codes.COMMENTARY, Now)


            view.passociations.Visible = True
            'If Me.mode > 0 Then
            '    Me.pvalidate.Visible = True
            'End If
            Dim ia As Integer
            ia = InStr(view.Lentities.SelectedItems(0).Text, "(inactive)") - 1
            view.tassociations.Visible = True
            view.ChangeNameToolStripMenuItem.Enabled = True
            view.minactive.Enabled = True
            If ia > 0 Then
                view.minactive.Text = "Make " + view.Lentities.SelectedItems(0).Text.Substring(0, ia) + " active"
                view.ChangeNameToolStripMenuItem.Text = "Change name of " + view.Lentities.SelectedItems(0).Text.Substring(0, ia)
            Else
                view.minactive.Text = "Make " + view.Lentities.SelectedItems(0).Text + " inactive"
                view.ChangeNameToolStripMenuItem.Text = "Change name of " + view.Lentities.SelectedItems(0).Text
            End If
            view.DeleteEntityToolStripMenuItem.Enabled = False
            view.DeleteEntityToolStripMenuItem.Visible = True
            If view.controller.container.mode = bc_am_cp_container.EDIT_LITE Then
                view.DeleteEntityToolStripMenuItem.Visible = False
            End If

            If ia > -1 Then
                view.DeleteEntityToolStripMenuItem.Enabled = True
                view.DeleteEntityToolStripMenuItem.Text = "Delete " + lent_name
            End If


            view.passociations.Enabled = True
            If from_list = True Then
                Me.check_read_only_entity()
            End If

            set_Warnings()

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_cp_main", "load_entity_parts", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

        End Try
    End Sub

    Public Sub load_entity_parts_real_time_search(entity As bc_om_entity, class_name As String, Optional ByVal from_list As Boolean = True)
        Try

            Dim i As Integer
            Dim oentity As New bc_om_entity
            Dim lent_name As String
            Dim class_id As Long
            If InStr(entity.name, "(inactive)") > 0 Then
                lent_name = entity.name.Substring(0, InStr(entity.name, "(inactive)") - 2)
            Else
                lent_name = entity.name
            End If
            REM reset item column
            Dim dtextbox As New DataGridViewTextBoxColumn



            dtextbox.SortMode = DataGridViewColumnSortMode.NotSortable



            dtextbox.Width = view.DataGridView1.Columns(3).Width
            view.DataGridView1.Columns.RemoveAt(3)
            dtextbox.Name = "Value"


            view.DataGridView1.Columns.Insert(3, dtextbox)


            'class_name = get_real_class_name(class_name)
            'class_id = get_class_id(class_name)
            'class_name = get_real_class_name(class_name)
            class_id = entity.class_id


            view.tattributes.SelectedIndex = 0



            If from_list = False Then

                load_associated_class_attributes(class_name)

                set_default_attribute_values(class_name, True)


                'view.tattributes.TabPages(0).Text = "attributes for: " + view.associated_entities(view.tassociations.SelectedIndex).name
                view.tattributes.TabPages(0).ImageIndex = attributes_icon

            End If

            If from_list = True Then

                view.warnings.Clear()
                For i = 1 To view.associated_entities.Count - 1
                    view.associated_entities.RemoveAt(i)
                Next
                'view.associated_entities.Clear()
                view.tassociations.TabPages(0).Text = entity.name
                If InStr(entity.name, "(inactive)") > 0 Then
                    view.tassociations.TabPages(0).ImageIndex = deactivate_icon
                Else
                    view.tassociations.TabPages(0).ImageIndex = selected_data_icon
                End If
                'view.tattributes.TabPages(0).Text = "attributes for: " + view.Lentities.SelectedItems(0).Text
                view.tattributes.TabPages(0).ImageIndex = attributes_icon
                For i = 1 To view.tattributes.TabPages.Count - 1
                    view.tattributes.TabPages.RemoveAt(1)
                Next

                For i = 1 To view.tassociations.TabPages.Count - 1
                    view.tassociations.TabPages.RemoveAt(1)
                Next
                For i = 0 To container.oEntities.entity.Count - 1
                    If container.oEntities.entity(i).class_id = class_id Then
                        If container.oEntities.entity(i).name = lent_name Then

                            view.associated_entities(view.tassociations.SelectedIndex).id = entity.id
                            view.associated_entities(view.tassociations.SelectedIndex).inactive = entity.inactive
                            For j = 0 To view.associated_entities(view.tassociations.SelectedIndex).attribute_values.count - 1
                                view.associated_entities(view.tassociations.SelectedIndex).attribute_values(j).value = ""
                                view.associated_entities(view.tassociations.SelectedIndex).attribute_values(j).last_update_comment = ""
                            Next
                            view.associated_entities(view.tassociations.SelectedIndex).schema_id = Me.schema_id

                            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                                view.associated_entities(view.tassociations.SelectedIndex).db_read()
                            Else

                                view.associated_entities(view.tassociations.SelectedIndex).tmode = bc_om_entity.tREAD

                                If view.associated_entities(view.tassociations.SelectedIndex).transmit_to_server_and_receive(view.associated_entities(view.tassociations.SelectedIndex), True) = False Then
                                    Exit Sub
                                End If
                            End If
                            view.associated_entities(view.tassociations.SelectedIndex).name = lent_name
                            view.associated_entities(view.tassociations.SelectedIndex).class_name = container.oEntities.entity(i).class_name

                            Exit For
                        End If
                    End If
                Next
            End If

            REM attribute values
            For j = 0 To view.associated_entities(view.tassociations.SelectedIndex).attribute_values.count - 1


                If view.associated_entities(view.tassociations.SelectedIndex).attribute_values(j).nullable = True Then
                    view.DataGridView1.Item(0, j).Value = New Drawing.Bitmap(1, 1)
                Else
                    view.DataGridView1.Item(0, j).Value = view.tabimages.Images(mandatory_icon)
                End If


                REM key value pairs 
                For k = 0 To container.oClasses.attribute_pool.Count - 1
                    If container.oClasses.attribute_pool(k).name = view.DataGridView1.Item(1, j).Value Then
                        If container.oClasses.attribute_pool(k).lookup_keys.count > 0 Then
                            REM find key value for value
                            For l = 0 To container.oClasses.attribute_pool(k).lookup_values.count - 1
                                If RTrim(LTrim(container.oClasses.attribute_pool(k).lookup_keys(l))) = RTrim(LTrim((view.associated_entities(view.tassociations.SelectedIndex).attribute_values(j).value))) Then
                                    view.associated_entities(view.tassociations.SelectedIndex).attribute_values(j).value = container.oClasses.attribute_pool(k).lookup_values(l)
                                    Exit For
                                End If
                            Next
                            If view.associated_entities(view.tassociations.SelectedIndex).attribute_values(j).show_workflow = True Then
                                For l = 0 To container.oClasses.attribute_pool(k).lookup_values.count - 1
                                    If RTrim(LTrim(container.oClasses.attribute_pool(k).lookup_keys(l))) = RTrim(LTrim((view.associated_entities(view.tassociations.SelectedIndex).attribute_values(j).published_value))) Then
                                        view.associated_entities(view.tassociations.SelectedIndex).attribute_values(j).Published_value = container.oClasses.attribute_pool(k).lookup_values(l)
                                        Exit For
                                    End If
                                Next
                            End If
                            Exit For
                        End If
                    End If
                Next

                REM ===================================


                view.DataGridView1.Item(3, j).Value = view.associated_entities(view.tassociations.SelectedIndex).attribute_values(j).value

                If view.associated_entities(view.tassociations.SelectedIndex).attribute_values(j).value_changed = True Then
                    view.DataGridView1.Item(0, j).Value = view.tabimages.Images(att_edited_icon)
                    'view.DataGridView1.Item(0, j).Value = view.tabimages.Images(selected_icon)
                End If

                If view.associated_entities(view.tassociations.SelectedIndex).attribute_values(j).show_workflow = False Then

                    view.DataGridView1.Item(5, j).Value = "n/a"
                    'view.DataGridView1.Item(4, j).Value = New Drawing.Bitmap(1, 1)

                Else
                    'view.DataGridView1.Item(4, j).Value = view.tabimages.Images(push_publish_icon)
                    If Not IsNothing(view.associated_entities(view.tassociations.SelectedIndex).attribute_values(j).published_value) Then
                        view.DataGridView1.Item(5, j).Value = view.associated_entities(view.tassociations.SelectedIndex).attribute_values(j).published_value
                    End If
                End If
            Next

            REM reset warnings icons
            Dim co As Integer
            Dim pco As Integer
            co = 0
            pco = 0

            REM now set warnings
            view.tattributes.TabPages(0).ImageIndex = attributes_icon

            view.lassociations.TabPages(0).ImageIndex = 21

            For i = 0 To view.warnings.Count - 1
                If view.warnings(i).association_tab = view.tassociations.SelectedIndex Then
                    If view.warnings(i).entity_tab > -1 Then
                        view.tentities.Nodes(view.warnings(i).entity_tab).imageindex = warning_icon
                    Else
                        If view.warnings(i).attribute_tab = 0 Then
                            view.tattributes.TabPages(0).ImageIndex = warning_icon
                            view.DataGridView1.Item(0, view.warnings(i).attribute).Value = view.tabimages.Images(warning_icon)
                        End If
                    End If
                End If

            Next
            Dim ocoom As New bc_cs_activity_log("TIMING", "H", bc_cs_activity_codes.COMMENTARY, Now)

            load_entity_view(True)

            ocoom = New bc_cs_activity_log("TIMING", "I", bc_cs_activity_codes.COMMENTARY, Now)


            view.passociations.Visible = True
            'If Me.mode > 0 Then
            '    Me.pvalidate.Visible = True
            'End If
            Dim ia As Integer
            ia = InStr(entity.name, "(inactive)") - 1
            view.tassociations.Visible = True
            view.ChangeNameToolStripMenuItem.Enabled = True
            view.minactive.Enabled = True
            If ia > 0 Then
                view.minactive.Text = "Make " + entity.name.Substring(0, ia) + " active"
                view.ChangeNameToolStripMenuItem.Text = "Change name of " + entity.name.Substring(0, ia)
            Else
                view.minactive.Text = "Make " + entity.name + " inactive"
                view.ChangeNameToolStripMenuItem.Text = "Change name of " + entity.name
            End If
            view.DeleteEntityToolStripMenuItem.Enabled = False
            view.DeleteEntityToolStripMenuItem.Visible = True
            If view.controller.container.mode = bc_am_cp_container.EDIT_LITE Then
                view.DeleteEntityToolStripMenuItem.Visible = False
            End If

            If ia > -1 Then
                view.DeleteEntityToolStripMenuItem.Enabled = True
                view.DeleteEntityToolStripMenuItem.Text = "Delete " + lent_name
            End If


            view.passociations.Enabled = True
            If from_list = True Then
                Me.check_read_only_entity_real_time(entity)
            End If

            set_Warnings()

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_cp_main", "load_entity_parts_real_time_search", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

        End Try
    End Sub

    Private Sub inactive_entity()
        REM read only mode
        Dim dpt As New System.Drawing.Point


        view.DataGridView1.Columns(0).Visible = False
        view.DataGridView1.Columns(2).Visible = False
        view.DataGridView1.Columns(4).Visible = False
        view.DataGridView1.Columns(1).ReadOnly = True
        view.DataGridView1.Columns(3).ReadOnly = True


    End Sub
    Public Function is_parent_class(ByVal class_name As String) As Boolean
        Dim co As Integer = 0
        Dim class_Id As Long
        class_Id = get_class_id(class_name)
        is_parent_class = False
        For i = 0 To container.oClasses.classes.Count - 1
            If container.oClasses.classes(i).class_id = view.associated_entities(view.tassociations.SelectedIndex).class_id Then
                For j = 0 To container.oClasses.classes(i).parent_links.count - 1
                    If container.oClasses.classes(i).parent_links(j).schema_id = Me.schema_id And container.oClasses.classes(i).parent_links(j).parent_class_id = class_Id Then
                        is_parent_class = True
                        Exit Function
                    End If
                Next
                Exit For
            End If
        Next

    End Function
    Public Function is_parent_class(ByVal class_name As String, ByVal k As Integer) As Boolean
        Dim co As Integer = 0
        Dim class_Id As Long
        class_Id = get_class_id(class_name)
        is_parent_class = False
        For i = 0 To container.oClasses.classes.Count - 1
            If container.oClasses.classes(i).class_id = class_Id Then
                For j = 0 To container.oClasses.classes(i).parent_links.count - 1
                    If container.oClasses.classes(i).parent_links(j).schema_id = Me.schema_id And container.oClasses.classes(i).parent_links(j).parent_class_id = view.associated_entities(k).class_id Then
                        is_parent_class = True
                        Exit Function
                    End If
                Next
                Exit For
            End If
        Next

    End Function
    Public Function get_parent_class_index() As Integer
        Dim co As Integer = 0
        For i = 0 To container.oClasses.classes.Count - 1
            If container.oClasses.classes(i).class_id = view.associated_entities(view.tassociations.SelectedIndex).class_id Then

                For j = 0 To container.oClasses.classes(i).parent_links.count - 1
                    If container.oClasses.classes(i).parent_links(j).schema_id = Me.schema_id Then
                        If co = view.tentities.SelectedNode.Index Then
                            get_parent_class_index = j
                            Exit Function
                        End If
                        co = co + 1
                    End If
                Next
                Exit For
            End If
        Next
    End Function
    Public Function get_child_class_index() As Integer
        Dim co As Integer = 0
        Dim coo As Integer = 0
        REM first evaluate number of parents
        For i = 0 To container.oClasses.classes.Count - 1
            If container.oClasses.classes(i).class_id = view.associated_entities(view.tassociations.SelectedIndex).class_id Then
                For j = 0 To container.oClasses.classes(i).parent_links.count - 1
                    If container.oClasses.classes(i).parent_links(j).schema_id = Me.schema_id Then

                        co = co + 1
                    End If
                Next
                Exit For
            End If
        Next
        REM now find child
        For i = 0 To container.oClasses.classes.Count - 1
            If container.oClasses.classes(i).class_id = view.associated_entities(view.tassociations.SelectedIndex).class_id Then
                For j = 0 To container.oClasses.classes(i).child_links.count - 1
                    If container.oClasses.classes(i).child_links(j).schema_id = Me.schema_id Then
                        If coo = view.tentities.SelectedNode.Index - co Then
                            get_child_class_index = j
                            Exit Function
                        End If
                        coo = coo + 1
                    End If
                Next
                Exit For
            End If
        Next

    End Function

    Public Sub load_entity_view(ByVal direction As Boolean)
        Try
            REM parent class propogatio
            Dim tnode As TreeNode
            Dim num As String
            view.tentities.Nodes.Clear()

            For i = 0 To container.oClasses.classes.Count - 1
                If container.oClasses.classes(i).class_id = view.associated_entities(view.tassociations.SelectedIndex).class_id Then
                    For j = 0 To container.oClasses.classes(i).parent_links.count - 1
                        If container.oClasses.classes(i).parent_links(j).schema_id = Me.schema_id Then
                            For k = 0 To container.oClasses.classes.Count - 1
                                If container.oClasses.classes(k).class_id = container.oClasses.classes(i).parent_links(j).parent_class_id Then
                                    num = container.oClasses.classes(i).parent_links(j).mandatory_cp
                                    If num = 0 Then
                                        view.tentities.Nodes.Add(container.oClasses.classes(k).class_name)
                                    ElseIf num = -1 Then
                                        view.tentities.Nodes.Add(container.oClasses.classes(k).class_name + "*")
                                    Else
                                        view.tentities.Nodes.Add(container.oClasses.classes(k).class_name + " (" + CStr(num) + ")")
                                    End If
                                    view.tentities.Nodes(view.tentities.Nodes.Count - 1).ImageIndex = parent_icon

                                    For l = 0 To view.associated_entities(view.tassociations.SelectedIndex).parent_entities.count - 1
                                        With view.associated_entities(view.tassociations.SelectedIndex).parent_entities(l)
                                            If .class_id = container.oClasses.classes(k).class_id Then
                                                If .inactive = 0 Then
                                                    view.tentities.Nodes(view.tentities.Nodes.Count - 1).Nodes.Add(.name)
                                                    view.tentities.Nodes(view.tentities.Nodes.Count - 1).Nodes(view.tentities.Nodes(view.tentities.Nodes.Count - 1).Nodes.Count - 1).ImageIndex = selected_data_icon
                                                Else
                                                    view.tentities.Nodes(view.tentities.Nodes.Count - 1).Nodes.Add(.name + " (inactive)")
                                                    view.tentities.Nodes(view.tentities.Nodes.Count - 1).Nodes(view.tentities.Nodes(view.tentities.Nodes.Count - 1).Nodes.Count - 1).ImageIndex = deactivate_icon

                                                End If
                                            End If
                                        End With
                                    Next

                                    REM entities of class
                                    'For m = 0 To container.oEntities.entity.Count - 1
                                    '    If container.oEntities.entity(m).class_id = container.oClasses.classes(k).class_id Then
                                    '        For l = 0 To view.associated_entities(view.tassociations.SelectedIndex).parent_entities.count - 1
                                    '            If container.oEntities.entity(m).id = view.associated_entities(view.tassociations.SelectedIndex).parent_entities(l).id And container.oEntities.entity(m).class_id = container.oClasses.classes(k).class_id Then
                                    '                If container.oEntities.entity(m).inactive = 0 Then
                                    '                    view.tentities.Nodes(view.tentities.Nodes.Count - 1).Nodes.Add(container.oEntities.entity(m).name)
                                    '                    view.tentities.Nodes(view.tentities.Nodes.Count - 1).Nodes(view.tentities.Nodes(view.tentities.Nodes.Count - 1).Nodes.Count - 1).ImageIndex = selected_data_icon

                                    '                Else
                                    '                    view.tentities.Nodes(view.tentities.Nodes.Count - 1).Nodes.Add(container.oEntities.entity(m).name + " (inactive)")
                                    '                    view.tentities.Nodes(view.tentities.Nodes.Count - 1).Nodes(view.tentities.Nodes(view.tentities.Nodes.Count - 1).Nodes.Count - 1).ImageIndex = deactivate_icon

                                    '                End If
                                    '                Exit For

                                    '            End If
                                    '        Next
                                    '    End If
                                    'Next
                                    Exit For
                                End If
                            Next
                        End If
                    Next


                    REM child
                    For j = 0 To container.oClasses.classes(i).child_links.count - 1
                        If container.oClasses.classes(i).child_links(j).schema_id = Me.schema_id Then
                            For k = 0 To container.oClasses.classes.Count - 1
                                If container.oClasses.classes(k).class_id = container.oClasses.classes(i).child_links(j).child_class_id Then
                                    num = container.oClasses.classes(i).child_links(j).mandatory_pc
                                    If num = 0 Then
                                        view.tentities.Nodes.Add(container.oClasses.classes(k).class_name)
                                    ElseIf num = -1 Then
                                        view.tentities.Nodes.Add(container.oClasses.classes(k).class_name + "*")
                                    Else
                                        view.tentities.Nodes.Add(container.oClasses.classes(k).class_name + " (" + CStr(num) + ")")
                                    End If
                                    view.tentities.Nodes(view.tentities.Nodes.Count - 1).ImageIndex = child_icon
                                    REM entities of class
                                    For l = 0 To view.associated_entities(view.tassociations.SelectedIndex).child_entities.count - 1
                                        With view.associated_entities(view.tassociations.SelectedIndex).child_entities(l)
                                            If .class_id = container.oClasses.classes(k).class_id Then
                                                If .inactive = 0 Then
                                                    view.tentities.Nodes(view.tentities.Nodes.Count - 1).Nodes.Add(.name)
                                                    view.tentities.Nodes(view.tentities.Nodes.Count - 1).Nodes(view.tentities.Nodes(view.tentities.Nodes.Count - 1).Nodes.Count - 1).ImageIndex = selected_data_icon
                                                Else
                                                    view.tentities.Nodes(view.tentities.Nodes.Count - 1).Nodes.Add(.name + " (inactive)")
                                                    view.tentities.Nodes(view.tentities.Nodes.Count - 1).Nodes(view.tentities.Nodes(view.tentities.Nodes.Count - 1).Nodes.Count - 1).ImageIndex = deactivate_icon

                                                End If
                                            End If
                                        End With
                                    Next
                                    'For m = 0 To container.oEntities.entity.Count - 1
                                    '    If container.oEntities.entity(m).class_id = container.oClasses.classes(k).class_id Then
                                    '        For l = 0 To view.associated_entities(view.tassociations.SelectedIndex).child_entities.count - 1
                                    '            If container.oEntities.entity(m).id = view.associated_entities(view.tassociations.SelectedIndex).child_entities(l).id And container.oEntities.entity(m).class_id = container.oClasses.classes(k).class_id Then
                                    '                If container.oEntities.entity(m).inactive = 0 Then
                                    '                    view.tentities.Nodes(view.tentities.Nodes.Count - 1).Nodes.Add(container.oEntities.entity(m).name)
                                    '                    view.tentities.Nodes(view.tentities.Nodes.Count - 1).Nodes(view.tentities.Nodes(view.tentities.Nodes.Count - 1).Nodes.Count - 1).ImageIndex = selected_data_icon

                                    '                Else
                                    '                    view.tentities.Nodes(view.tentities.Nodes.Count - 1).Nodes.Add(container.oEntities.entity(m).name + " (inactive)")
                                    '                    view.tentities.Nodes(view.tentities.Nodes.Count - 1).Nodes(view.tentities.Nodes(view.tentities.Nodes.Count - 1).Nodes.Count - 1).ImageIndex = deactivate_icon

                                    '                End If
                                    '                Exit For
                                    '            End If
                                    '        Next
                                    '    End If
                                    'Next
                                    Exit For
                                End If
                            Next
                        End If
                    Next
                    Exit For
                End If

            Next

            REM FIL JULY 2012
            Dim rating As Integer

            For n = 0 To Me.container.oUsers.pref_types.Count - 1
                rating = 0
                If Me.container.oUsers.pref_types(n).name = "" Then
                    view.tentities.Nodes.Add("Users")
                Else
                    view.tentities.Nodes.Add("Users-" + Me.container.oUsers.pref_types(n).name)
                End If
                view.tentities.Nodes(view.tentities.Nodes.Count - 1).ImageIndex = users_icon
                For i = 0 To view.associated_entities(view.tassociations.SelectedIndex).user_prefs.count - 1
                    If view.associated_entities(view.tassociations.SelectedIndex).user_prefs(i).pref_type = Me.container.oUsers.pref_types(n).id Then
                        For j = 0 To container.oUsers.user.Count - 1
                            REM FIL JULY 2012
                            If view.associated_entities(view.tassociations.SelectedIndex).user_prefs(i).user_id = container.oUsers.user(j).id Then
                                view.associated_entities(view.tassociations.SelectedIndex).user_prefs(i).rating = rating + 1
                                tnode = New TreeNode
                                If container.oUsers.user(j).inactive = 0 Then
                                    tnode.Text = CStr(rating + 1) + ". " + container.oUsers.user(j).first_name + " " + container.oUsers.user(j).surname
                                    tnode.ImageIndex = user_icon
                                Else
                                    tnode.Text = CStr(rating + 1) + ". " + container.oUsers.user(j).first_name + " " + container.oUsers.user(j).surname + " (inactive)"
                                    tnode.ImageIndex = deactivate_icon
                                End If
                                view.tentities.Nodes(view.tentities.Nodes.Count - 1).Nodes.Add(tnode)
                                Exit For
                            End If
                        Next
                        rating = rating + 1
                    End If
                Next
            Next

            view.tentities.ExpandAll()

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_cp_main", "load_entity_view", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    REM EFG JULY 2013
    Public Sub load_entity_assign(ByVal class_name As String)
        Try
            Dim num As Integer
            Dim prev_entities As New ArrayList
            REM FIL July 2012
            If Len(class_name) >= 5 Then
                If Left(class_name, 5) <> "Users" Then
                    class_name = Me.get_real_class_name(class_name)
                End If
            Else
                class_name = Me.get_real_class_name(class_name)
            End If
            Dim ofrm As New bc_am_cp_assign


            ofrm.lselparent.Sorted = False
            ofrm.bup.Visible = False
            ofrm.bdn.Visible = False
            ofrm.BlueCurve_TextSearch1.Visible = False

            'ofrm.baudit.Visible = False

            If class_name = "Users" Then
                ofrm.bup.Visible = False
                ofrm.bdn.Visible = False

                ofrm.Tnew.Visible = False
                ofrm.Label1.Visible = False
                ofrm.Label2.Visible = False
                ofrm.lselparent.Sorted = False
                ofrm.bup.Visible = True
                ofrm.bdn.Visible = True
                Dim dp As System.Drawing.Point
                dp.X = ofrm.lallparent.Location.X
                dp.Y = ofrm.lselparent.Location.Y
                ofrm.lallparent.Location = dp
                ofrm.lallparent.Height = ofrm.lselparent.Height - 25
            Else

                'ofrm.baudit.Visible = True
                ofrm.entity_name = view.associated_entities(view.tassociations.SelectedIndex).name
                ofrm.entity_id = view.associated_entities(view.tassociations.SelectedIndex).id
                ofrm.schema_id = Me.schema_id
                ofrm.schema_name = view.cschema.Text
                ofrm.class_name = class_name
                For kk = 0 To container.oClasses.classes.Count - 1
                    If container.oClasses.classes(kk).class_name = class_name Then
                        ofrm.BlueCurve_TextSearch1.SearchClass = container.oClasses.classes(kk).class_id
                        ofrm.class_id = container.oClasses.classes(kk).class_id

                        Exit For

                    End If
                Next
                ofrm.BlueCurve_TextSearch1.Visible = True
                ofrm.BlueCurve_TextSearch1.Enabled = True
                ofrm.BlueCurve_TextSearch1.AttributeControlBuild(False)
                ofrm.BlueCurve_TextSearch1.SearchUserEntitiesOnly = False
                ofrm.BlueCurve_TextSearch1.SearchBuildEntitiesOnly = False
                ofrm.BlueCurve_TextSearch1.showinactive = False
                ofrm.BlueCurve_TextSearch1.ExcludeControl = "lselparent"

            End If

            ofrm.filter_class = class_name
            ofrm.fcontainer = Me.container
            ofrm.ltitle.Text = "Assign " + class_name + "(s)" + " to " + view.Lentities.SelectedItems(0).Text
            Dim i, k As Integer
            Dim co As Integer = 0
            Dim user_mode As Boolean = False
            REM FIL July 2012

            If Left(class_name, 5) = "Users" Then
                ofrm.BlueCurve_TextSearch1.Visible = False
                user_mode = True
                If class_name <> "Users" Then
                    class_name = Right(class_name, Len(class_name) - 6)
                Else
                    class_name = ""
                End If
                ofrm.class_name = class_name
                ofrm.user_mode = True

                ofrm.lselparent.Sorted = False
                For j = 0 To view.associated_entities(view.tassociations.SelectedIndex).user_prefs.count - 1
                    For i = 0 To container.oUsers.user.Count - 1
                        If container.oUsers.user(i).id = view.associated_entities(view.tassociations.SelectedIndex).user_prefs(j).user_id And view.associated_entities(view.tassociations.SelectedIndex).user_prefs(j).pref_name = class_name Then
                            ofrm.pref_type_id = view.associated_entities(view.tassociations.SelectedIndex).user_prefs(j).pref_type

                            If container.oUsers.user(i).inactive = 0 Then
                                ofrm.lselparent.Items.Add(container.oUsers.user(i).first_name + " " + container.oUsers.user(i).surname)
                            Else
                                ofrm.lselparent.Items.Add(container.oUsers.user(i).first_name + " " + container.oUsers.user(i).surname + " (inactive)")
                            End If
                        End If
                    Next
                Next

                Dim ufound As Boolean
                For i = 0 To container.oUsers.user.Count - 1
                    ufound = False
                    If container.oUsers.user(i).inactive = 0 Then
                        For j = 0 To ofrm.lselparent.Items.Count - 1
                            If ofrm.lselparent.Items(j) = (container.oUsers.user(i).first_name + " " + container.oUsers.user(i).surname) Then
                                ufound = True
                                Exit For
                            End If
                        Next
                        If ufound = False Then
                            ofrm.lallparent.Items.Add(container.oUsers.user(i).first_name + " " + container.oUsers.user(i).surname)
                        End If
                    End If
                Next
            Else
                'ofrm.lselparent.Sorted = True
                'If is_parent_class(get_real_class_name(view.tentities.SelectedNode.Text)) = False Then
                ofrm.bup.Visible = True
                ofrm.bdn.Visible = True
                ofrm.lselparent.Sorted = False
                ' End If
                ofrm.bparent = False
                Dim idx As Long
                idx = sel_cls_idx(view.associated_entities(view.tassociations.SelectedIndex).class_name)
                If is_parent_class(get_real_class_name(view.tentities.SelectedNode.Text)) = True Then
                    Dim pi As Integer
                    ofrm.bparent = True
                    pi = get_parent_class_index()
                    num = container.oClasses.classes(idx).parent_links(pi).mandatory_cp
                    For i = 0 To container.oEntities.entity.Count - 1
                        If container.oEntities.entity(i).class_id = container.oClasses.classes(idx).parent_links(pi).parent_class_id Then
                            REM add entities of class
                            If container.oEntities.entity(i).inactive = 0 Then
                                ofrm.lallparent.Items.Add(container.oEntities.entity(i).name)
                            End If
                        End If
                    Next
                    REM now load selected entities
                    For k = 0 To view.associated_entities(view.tassociations.SelectedIndex).parent_entities.Count - 1
                        If view.associated_entities(view.tassociations.SelectedIndex).parent_entities(k).class_id = container.oClasses.classes(idx).parent_links(pi).parent_class_id Then
                            If view.associated_entities(view.tassociations.SelectedIndex).parent_entities(k).inactive = 0 Then
                                ofrm.lselparent.Items.Add(view.associated_entities(view.tassociations.SelectedIndex).parent_entities(k).name)
                            Else
                                ofrm.lselparent.Items.Add(view.associated_entities(view.tassociations.SelectedIndex).parent_entities(k).name + " (inactive)")
                            End If
                            prev_entities.Add(view.associated_entities(view.tassociations.SelectedIndex).parent_entities(k).name)
                        End If
                    Next

                Else
                    Dim ci As Integer
                    ci = get_child_class_index()
                    num = container.oClasses.classes(idx).child_links(ci).mandatory_pc
                    For i = 0 To container.oEntities.entity.Count - 1
                        If container.oEntities.entity(i).class_id = container.oClasses.classes(idx).child_links(ci).child_class_id Then
                            REM add entities of class
                            If container.oEntities.entity(i).inactive = 0 Then
                                ofrm.lallparent.Items.Add(container.oEntities.entity(i).name)
                            End If
                        End If
                    Next
                    REM now load selected entities
                    For k = 0 To view.associated_entities(view.tassociations.SelectedIndex).child_entities.Count - 1
                        If view.associated_entities(view.tassociations.SelectedIndex).child_entities(k).class_id = container.oClasses.classes(idx).child_links(ci).child_class_id Then
                            If view.associated_entities(view.tassociations.SelectedIndex).child_entities(k).inactive = 0 Then
                                ofrm.lselparent.Items.Add(view.associated_entities(view.tassociations.SelectedIndex).child_entities(k).name)
                            Else
                                ofrm.lselparent.Items.Add(view.associated_entities(view.tassociations.SelectedIndex).child_entities(k).name + " (inactive)")
                            End If
                            prev_entities.Add(view.associated_entities(view.tassociations.SelectedIndex).child_entities(k).name)
                        End If
                    Next
                End If
                REM now remove selected ones from al list
                Dim aco As Integer = 0
                While aco < ofrm.lallparent.Items.Count
                    For i = 0 To ofrm.lselparent.Items.Count - 1
                        If ofrm.lselparent.Items(i) = ofrm.lallparent.Items(aco) Then
                            ofrm.lallparent.Items.RemoveAt(aco)
                            aco = aco - 1
                            Exit For
                        End If
                    Next
                    aco = aco + 1
                End While
                REM set up mandatory number
                If num = 0 Then
                    ofrm.ltitle.Text = "Assign " + class_name + "(s)" + " to " + view.Lentities.SelectedItems(0).Text
                ElseIf num = -1 Then
                    ofrm.ltitle.Text = "Assign at least one " + class_name + "(s)" + " to " + view.Lentities.SelectedItems(0).Text
                Else
                    If num = 1 Then
                        ofrm.ltitle.Text = "Assign 1 " + class_name + " to " + view.Lentities.SelectedItems(0).Text
                    Else
                        ofrm.ltitle.Text = "Assign " + CStr(num) + " " + class_name + "(s) to " + view.Lentities.SelectedItems(0).Text
                    End If
                End If
            End If
            ofrm.num = num
            ofrm.ShowDialog()
            view.Cursor = Cursors.WaitCursor


            Dim change_mode As Integer = 0
            Dim found As Boolean = False
            If ofrm.cancel_selected = False Then
                If user_mode = False Then
                    For i = 0 To ofrm.lselparent.Items.Count - 1
                        change_mode = 0
                        For j = 0 To prev_entities.Count - 1
                            If ofrm.lselparent.Items(i) = prev_entities(j) Or ofrm.lselparent.Items(i) = prev_entities(j) + " (inactive)" Then
                                REM already exists
                                change_mode = 1
                                Exit For
                            End If
                        Next
                        Dim enew As Boolean = False
                        If change_mode = 0 Then
                            REM check if entity is new
                            Dim m As Integer
                            For m = 0 To ofrm.new_entities.Count - 1
                                If ofrm.new_entities(m) = ofrm.lselparent.Items(i) Then
                                    Me.add_entity(ofrm.new_entities(m), class_name, True)
                                    enew = True
                                    view.associated_entities.Add(container.oEntities.entity(container.oEntities.entity.Count - 1))
                                    view.tassociations.TabPages.Add(ofrm.new_entities(m) + " [" + class_name + "]")
                                    If Me.is_parent_class(class_name) Then
                                        view.tassociations.TabPages(view.tassociations.TabPages.Count - 1).ImageIndex = 0
                                    Else
                                        view.tassociations.TabPages(view.tassociations.TabPages.Count - 1).ImageIndex = 0
                                    End If

                                End If
                            Next
                            add_link(class_name, ofrm.lselparent.Items(i), enew)

                        End If
                    Next
                    REM now see if any no longer exists
                    For i = 0 To prev_entities.Count - 1
                        found = False
                        For j = 0 To ofrm.lselparent.Items.Count - 1
                            If ofrm.lselparent.Items(j) = prev_entities(i) Or ofrm.lselparent.Items(j) = prev_entities(i) + " (inactive)" Then
                                found = True
                                Exit For
                            End If
                        Next
                        If found = False Then
                            remove_link(class_name, prev_entities(i))
                        End If
                    Next
                    REM now realign in  ratings order
                    Dim tents As New ArrayList
                    Dim ico As Integer = 0

                    If Me.is_parent_class(class_name) = False Then
                        While ico <= view.associated_entities(view.tassociations.SelectedIndex).child_entities.count - 1
                            If view.associated_entities(view.tassociations.SelectedIndex).child_entities(ico).class_name = class_name Then
                                tents.Add(view.associated_entities(view.tassociations.SelectedIndex).child_entities(ico))
                                view.associated_entities(view.tassociations.SelectedIndex).child_entities.removeat(ico)
                            Else
                                ico = ico + 1
                            End If
                        End While

                        REM now reassign
                        For i = 0 To ofrm.lselparent.Items.Count - 1
                            For j = 0 To tents.Count - 1
                                If tents(j).class_name = class_name And (tents(j).name = ofrm.lselparent.Items(i) Or tents(j).name + " (inactive)" = ofrm.lselparent.Items(i)) Then
                                    tents(j).rating = CStr(i + 1)
                                    view.associated_entities(view.tassociations.SelectedIndex).child_entities.add(tents(j))
                                    Exit For
                                End If
                            Next
                        Next
                    Else
                        While ico <= view.associated_entities(view.tassociations.SelectedIndex).parent_entities.count - 1
                            If view.associated_entities(view.tassociations.SelectedIndex).parent_entities(ico).class_name = class_name Then
                                tents.Add(view.associated_entities(view.tassociations.SelectedIndex).parent_entities(ico))
                                view.associated_entities(view.tassociations.SelectedIndex).parent_entities.removeat(ico)
                            Else
                                ico = ico + 1
                            End If
                        End While

                        REM now reassign
                        For i = 0 To ofrm.lselparent.Items.Count - 1
                            For j = 0 To tents.Count - 1
                                If tents(j).class_name = class_name And (tents(j).name = ofrm.lselparent.Items(i) Or tents(j).name + " (inactive)" = ofrm.lselparent.Items(i)) Then
                                    tents(j).rating = CStr(i + 1)
                                    view.associated_entities(view.tassociations.SelectedIndex).parent_entities.add(tents(j))
                                    Exit For
                                End If
                            Next
                        Next

                    End If
                Else
                    i = 0
                    While i <= view.associated_entities(view.tassociations.SelectedIndex).user_prefs.count - 1
                        If view.associated_entities(view.tassociations.SelectedIndex).user_prefs(i).pref_name = class_name Then
                            view.associated_entities(view.tassociations.SelectedIndex).user_prefs.removeat(i)
                            i = i - 1
                        End If
                        i = i + 1
                    End While
                    For i = 0 To ofrm.lselparent.Items.Count - 1
                        add_user(ofrm.lselparent.Items(i), class_name)
                    Next
                End If
                load_entity_view(True)
                entity_edit_mode()
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_taxonomy", "load_entity_assign", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            view.Cursor = Cursors.Default
        End Try

    End Sub
    REM FIL July 2012
    Public Sub add_user(ByVal name As String, ByVal class_name As String)
        For i = 0 To container.oUsers.user.Count - 1
            If container.oUsers.user(i).first_name + " " + container.oUsers.user(i).surname = name Then
                Dim opref As New bc_om_user_pref
                opref.user_id = container.oUsers.user(i).id
                For k = 0 To container.oUsers.pref_types.Count - 1
                    If container.oUsers.pref_types(k).name = class_name Then
                        opref.pref_type = container.oUsers.pref_types(k).id
                        opref.pref_name = class_name
                    End If
                Next
                view.associated_entities(view.tassociations.SelectedIndex).user_prefs.add(opref)
                Exit For
            End If
        Next

    End Sub
    Public Sub change_rating(ByVal increase As Boolean)
        Try
            Dim name As String
            Dim po As Integer



            name = view.tentities.SelectedNode.Text
            po = InStr(name, ".")

            name = name.Substring(po + 1, name.Length - po - 1)


            REM users
            Dim tx As String
            Dim pn As String = ""
            tx = view.tentities.SelectedNode.Parent.Text
            REM FIL July 2012

            If Len(tx) >= 5 Then
                If tx.Substring(0, 5) = "Users" And tx <> "Users" Then
                    pn = tx.Substring(6, Len(tx) - 6)
                    tx = "Users"
                Else
                    pn = ""
                End If
            End If


            If tx = "Users" Then
                Dim opref As bc_om_user_pref
                For i = 0 To container.oUsers.user.Count - 1
                    If container.oUsers.user(i).first_name + " " + container.oUsers.user(i).surname = name Then
                        For j = 0 To view.associated_entities(view.tassociations.SelectedIndex).user_prefs.count - 1
                            If view.associated_entities(view.tassociations.SelectedIndex).user_prefs(j).user_id = container.oUsers.user(i).id And view.associated_entities(view.tassociations.SelectedIndex).user_prefs(j).pref_name = pn Then
                                opref = view.associated_entities(view.tassociations.SelectedIndex).user_prefs(j)
                                If increase = True And j > 0 Then
                                    view.associated_entities(view.tassociations.SelectedIndex).user_prefs.removeat(j)
                                    view.associated_entities(view.tassociations.SelectedIndex).user_prefs.insert(j - 1, opref)
                                End If
                                If increase = False And j < view.associated_entities(view.tassociations.SelectedIndex).user_prefs.count - 1 Then
                                    view.associated_entities(view.tassociations.SelectedIndex).user_prefs.removeat(j)
                                    view.associated_entities(view.tassociations.SelectedIndex).user_prefs.insert(j + 1, opref)
                                End If
                                Exit For
                            End If
                        Next
                        Exit For
                    End If
                Next
            Else
                Dim eclass As String
                Dim ename As String
                Dim le As bc_om_linked_entity
                Dim rating As Integer
                ename = view.tentities.SelectedNode.Text
                eclass = get_real_class_name(view.tentities.SelectedNode.Parent.Text)
                If is_parent_class(get_real_class_name(view.tentities.SelectedNode.Parent.Text)) = False Then
                    For k = 0 To view.associated_entities(view.tassociations.SelectedIndex).child_entities.Count - 1
                        If view.associated_entities(view.tassociations.SelectedIndex).child_entities(k).class_name = eclass Then
                            If view.associated_entities(view.tassociations.SelectedIndex).child_entities(k).name() = ename Or view.associated_entities(view.tassociations.SelectedIndex).child_entities(k).name() + " (inative)" = ename Then
                                le = view.associated_entities(view.tassociations.SelectedIndex).child_entities(k)
                                rating = view.associated_entities(view.tassociations.SelectedIndex).child_entities(k).rating
                                If increase = True Then
                                    view.associated_entities(view.tassociations.SelectedIndex).child_entities.removeat(k)
                                    view.associated_entities(view.tassociations.SelectedIndex).child_entities.insert(k - 1, le)
                                    view.associated_entities(view.tassociations.SelectedIndex).child_entities(k - 1).rating = rating - 1
                                    view.associated_entities(view.tassociations.SelectedIndex).child_entities(k).rating = rating
                                Else
                                    view.associated_entities(view.tassociations.SelectedIndex).child_entities.removeat(k)
                                    view.associated_entities(view.tassociations.SelectedIndex).child_entities.insert(k + 1, le)
                                    view.associated_entities(view.tassociations.SelectedIndex).child_entities(k + 1).rating = rating + 1
                                    view.associated_entities(view.tassociations.SelectedIndex).child_entities(k).rating = rating
                                End If
                                Exit For
                            End If
                        End If
                    Next
                Else
                    For k = 0 To view.associated_entities(view.tassociations.SelectedIndex).parent_entities.Count - 1
                        If view.associated_entities(view.tassociations.SelectedIndex).parent_entities(k).class_name = eclass Then
                            If view.associated_entities(view.tassociations.SelectedIndex).parent_entities(k).name() = ename Or view.associated_entities(view.tassociations.SelectedIndex).parent_entities(k).name() + " (inative)" = ename Then
                                le = view.associated_entities(view.tassociations.SelectedIndex).parent_entities(k)
                                rating = view.associated_entities(view.tassociations.SelectedIndex).parent_entities(k).rating
                                If increase = True Then
                                    view.associated_entities(view.tassociations.SelectedIndex).parent_entities.removeat(k)
                                    view.associated_entities(view.tassociations.SelectedIndex).parent_entities.insert(k - 1, le)
                                    view.associated_entities(view.tassociations.SelectedIndex).parent_entities(k - 1).rating = rating - 1
                                    view.associated_entities(view.tassociations.SelectedIndex).parent_entities(k).rating = rating
                                Else
                                    view.associated_entities(view.tassociations.SelectedIndex).parent_entities.removeat(k)
                                    view.associated_entities(view.tassociations.SelectedIndex).parent_entities.insert(k + 1, le)
                                    view.associated_entities(view.tassociations.SelectedIndex).parent_entities(k + 1).rating = rating + 1
                                    view.associated_entities(view.tassociations.SelectedIndex).parent_entities(k).rating = rating
                                End If
                                Exit For
                            End If
                        End If
                    Next
                End If

            End If
            load_entity_view(True)
            entity_edit_mode()
            view.IncreaseRatingToolStripMenuItem.Visible = False
            view.DecreaseRatingToolStripMenuItem.Visible = False

        Catch ex As Exception
            MsgBox(ex.Message)

        End Try
    End Sub
    Private Function get_class_id(ByVal class_name As String) As Long
        Dim i As Integer
        class_name = get_real_class_name(class_name)
        For i = 0 To container.oClasses.classes.Count - 1
            If container.oClasses.classes(i).class_name = class_name Then
                get_class_id = container.oClasses.classes(i).class_id
                Exit For
            End If
        Next
    End Function
    Public filter_attributes As New List(Of Long)
    Public Sub select_filter_class(id As Long)
        If id > 0 Then
            view.tfilter.filter_attribute_id = filter_attributes(id - 1)
        Else
            view.tfilter.filter_attribute_id = 0
        End If

        view.tfilter.SearchRefresh(True)
        Me.container.oEntities = bc_am_load_objects.obc_entities
    End Sub
    Public Sub load_attribute_filter(class_id As Integer)
        filter_attributes.Clear()

        view.cboefilter.Enabled = False

        view.cboefilter.Items.Clear()
        view.cboefilter.Items.Add("All")
        For i = 0 To bc_am_load_objects.obc_entities.filter_attributes_types.Count - 1
            If class_id = bc_am_load_objects.obc_entities.filter_attributes_types(i).class_id Then
                view.cboefilter.Items.Add(bc_am_load_objects.obc_entities.filter_attributes_types(i).attribute_name)
                view.cboefilter.Enabled = True
                filter_attributes.Add(bc_am_load_objects.obc_entities.filter_attributes_types(i).attribute_id)
            End If
        Next
        view.cboefilter.SelectedIndex = 0

    End Sub

    Private Function get_entity_id(ByVal class_name As String, ByVal entity_name As String) As Long
        Dim i As Integer
        For i = 0 To container.oEntities.entity.Count - 1
            If container.oEntities.entity(i).class_name = class_name And container.oEntities.entity(i).name = entity_name Then
                get_entity_id = container.oEntities.entity(i).id
                Exit For
            End If
        Next
    End Function
    Private Sub add_link(ByVal class_name As String, ByVal entity_name As String, Optional ByVal reverse_link As Boolean = False)
        Try
            If entity_name = "" Then
                Exit Sub
            End If
            Dim oentity As New bc_om_linked_entity
            Dim olink As New bc_om_entity_link
            olink.schema_id = Me.schema_id
            class_name = get_real_class_name(class_name)
            oentity.id = get_entity_id(class_name, entity_name)
            oentity.name = entity_name
            oentity.class_name = class_name
            oentity.class_id = get_class_id(class_name)
            oentity.schema_id = Me.schema_id
            If is_parent_class(class_name) = True Then
                olink.parent_entity_id = oentity.id
                olink.child_entity_id = view.associated_entities(view.tassociations.SelectedIndex).id
                olink.child_class_id = view.associated_entities(view.tassociations.SelectedIndex).class_id
                olink.parent_class_id = oentity.class_id
            Else
                olink.parent_entity_id = view.associated_entities(view.tassociations.SelectedIndex).id
                olink.child_entity_id = oentity.id
                olink.child_class_id = oentity.class_id
                olink.parent_class_id = view.associated_entities(view.tassociations.SelectedIndex).class_id
            End If
            olink.child_parent_rating = -1
            olink.parent_child_rating = -1

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                olink.db_write()
            Else
                olink.tmode = bc_om_entity.tWRITE
                If olink.transmit_to_server_and_receive(olink, True) = False Then
                    Exit Sub
                End If
            End If
            oentity.linked_attribute_values = olink.attributes
            Dim new_link As New bc_om_linked_entity
            new_link.id = view.associated_entities(view.tassociations.SelectedIndex).id
            new_link.name = view.associated_entities(view.tassociations.SelectedIndex).name
            new_link.class_id = view.associated_entities(view.tassociations.SelectedIndex).class_id
            new_link.class_name = view.associated_entities(view.tassociations.SelectedIndex).class_name
            If is_parent_class(class_name) = True Then
                view.associated_entities(view.tassociations.SelectedIndex).parent_entities.Add(oentity)
                If reverse_link = True Then
                    view.associated_entities(view.tassociations.TabPages.Count - 1).child_entities.Add(new_link)
                End If
            Else
                view.associated_entities(view.tassociations.SelectedIndex).child_entities.Add(oentity)
                If reverse_link = True Then
                    view.associated_entities(view.tassociations.TabPages.Count - 1).parent_entities.Add(new_link)
                End If
            End If

            REM add link for rollback purposes
            If view.tassociations.SelectedIndex = 0 Then
                view.gadd_links.Add(olink)
            End If


        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_cp_main", "add_link", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

        End Try

    End Sub

    Public Sub remove_link(ByVal class_name As String, ByVal entity_name As String)
        Try
            If class_name = "" Then
                Exit Sub

            End If
            Dim tx As String
            tx = ""
            Try
                tx = view.tentities.SelectedNode.Parent.Text
            Catch

            End Try
            If tx = "Users" Then
                entity_name = Right(entity_name, entity_name.Length - 3)
                For i = 0 To container.oUsers.user.Count - 1
                    If container.oUsers.user(i).first_name + " " + container.oUsers.user(i).surname = entity_name Then
                        For j = 0 To view.associated_entities(view.tassociations.SelectedIndex).user_prefs.count - 1
                            If view.associated_entities(view.tassociations.SelectedIndex).user_prefs(j) = container.oUsers.user(i).id Then
                                view.associated_entities(view.tassociations.SelectedIndex).user_prefs.removeat(j)
                                Exit For
                            End If
                        Next
                        Exit For
                    End If
                Next
                load_entity_view(True)

            Else

                If InStr(entity_name, " (inactive)") > 0 Then
                    entity_name = entity_name.Substring(0, entity_name.Length - 11)
                End If
                Dim i As Integer
                Dim olink As New bc_om_entity_link
                olink.schema_id = Me.schema_id
                olink.write_mode = bc_om_entity_link.DELETE
                class_name = get_real_class_name(class_name)

                If is_parent_class(class_name) = True Then
                    olink.parent_entity_id = get_entity_id(class_name, entity_name)
                    olink.child_entity_id = view.associated_entities(view.tassociations.SelectedIndex).id
                Else
                    olink.child_entity_id = get_entity_id(class_name, entity_name)
                    olink.parent_entity_id = view.associated_entities(view.tassociations.SelectedIndex).id
                End If
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    olink.db_write()
                Else
                    olink.tmode = bc_om_entity.tWRITE
                    If olink.transmit_to_server_and_receive(olink, True) = False Then
                        Exit Sub
                    End If
                End If
                If is_parent_class(class_name) = True Then
                    For i = 0 To view.associated_entities(view.tassociations.SelectedIndex).parent_entities.Count - 1
                        If view.associated_entities(view.tassociations.SelectedIndex).parent_entities(i).id = get_entity_id(class_name, entity_name) Then
                            view.associated_entities(view.tassociations.SelectedIndex).parent_entities.RemoveAt(i)
                            Exit For
                        End If
                    Next
                Else
                    For i = 0 To view.associated_entities(view.tassociations.SelectedIndex).child_entities.Count - 1
                        If view.associated_entities(view.tassociations.SelectedIndex).child_entities(i).id = get_entity_id(class_name, entity_name) Then
                            view.associated_entities(view.tassociations.SelectedIndex).child_entities.RemoveAt(i)
                            Exit For
                        End If
                    Next
                End If
                '    REM delete link for rollback purposes
                If view.tassociations.SelectedIndex = 0 Then
                    view.gdel_links.Add(olink)
                End If
                delete_associated_entity(entity_name, class_name)
                load_entity_view(True)

            End If
            entity_edit_mode()

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_cp_main", "remove_link", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

            view.Cursor = Cursors.Default
        End Try
    End Sub
    Public Sub entity_edit_mode()
        view.bentchanges.Enabled = True
        view.bentdiscard.Enabled = True
        view.pentities.Enabled = False
        container.uxNavBar.Enabled = False
        container.uxViewToolStrip.Enabled = False
        container.uxView.Enabled = False
        container.uncomitted_data = True
        'If view.lvwvalidate.Enabled = True Then
        '    compile_warnings()
        '    show_warnings()
        'End If
        view.baudit.Enabled = False
        view.bentaudit.Enabled = False



        Me.load_entity_toolbar()

    End Sub
    Public Sub entity_edit_mode_linked()
        view.bentchanges.Enabled = True
        view.bentdiscard.Enabled = True
        view.pentities.Enabled = False
        container.uxNavBar.Enabled = False
        container.uxViewToolStrip.Enabled = False
        container.uxView.Enabled = False
        container.uncomitted_data = True
        If view.lvwvalidate.Enabled = True Then
            compile_warnings()
            show_warnings()
        End If
        'Me.load_entity_toolbar()

    End Sub

    Public Sub exit_entity_edit_mode()
        view.bentchanges.Enabled = False
        view.bentdiscard.Enabled = False
        view.pentities.Enabled = True
        container.uxNavBar.Enabled = True
        container.uxViewToolStrip.Enabled = True
        container.uxView.Enabled = True
        container.uncomitted_data = False
        view.baudit.Enabled = True
        view.bentaudit.Enabled = True
    End Sub
    Public Sub discard_changes()
        Dim omsg As New bc_cs_message("Blue Curve", "Are you sure you wish to discard changes made to " + view.Lentities.SelectedItems(0).Text + " and its associations", bc_cs_message.MESSAGE, True, False, "Yes", "no", True)
        If omsg.cancel_selected = True Then
            Exit Sub
        End If
        REM rollback changes
        view.Cursor = Cursors.WaitCursor
        Try
            REM remove all entities that are not the first associated entity
            REM firstly out of memory
            Dim co As Long
            co = container.oEntities.entity.Count - 1
            For i = (co - (view.associated_entities.Count - 2)) To co
                container.oEntities.entity.RemoveAt(co - (view.associated_entities.Count - 2))
            Next

            REM then out of system
            For i = 1 To view.associated_entities.Count - 1
                view.associated_entities(1).write_mode = bc_om_entity.DELETE
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    view.associated_entities(1).db_write()
                Else
                    view.associated_entities(1).tmode = bc_om_entity.tWRITE
                    If view.associated_entities(1).transmit_to_server_and_receive(view.associated_entities(1), True) = False Then
                        omsg = New bc_cs_message("Blue Curve", "Failed to delete entity " + view.associated_entities(1).name + " check log file", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                        Exit Sub
                    End If
                End If
                view.associated_entities.RemoveAt(1)
            Next
            REM remove tab strios
            For i = 1 To view.tassociations.TabPages.Count - 1
                view.tassociations.TabPages.RemoveAt(1)
            Next

            REM now remove new links for  master entity
            REM TBD

            REM now roll back for master entity added or deleted links

            For i = 0 To view.gadd_links.Count - 1
                view.gadd_links(i).write_mode = bc_om_entity_link.DELETE
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    view.gadd_links(i).db_write()
                Else
                    view.gadd_links(i).tmode = bc_om_entity.tWRITE
                    view.gadd_links(i).transmit_to_server_and_receive(view.gadd_links(i), True)
                End If
            Next
            Dim found As Boolean = False
            For i = 0 To view.gdel_links.Count - 1
                found = False
                REM make sure link hasnt been added and deleted in same session
                For j = 0 To view.gadd_links.Count - 1
                    If view.gadd_links(j).parent_entity_id = view.gdel_links(i).parent_entity_id And view.gadd_links(j).child_entity_id = view.gdel_links(i).child_entity_id Then
                        found = True
                        Exit For
                    End If

                Next
                If found = False Then
                    view.gdel_links(i).write_mode = bc_om_entity_link.INSERT
                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then

                        view.gdel_links(i).db_write()
                    Else
                        view.gdel_links(i).tmode = bc_om_entity.tWRITE
                        view.gdel_links(i).transmit_to_server_and_receive(view.gdel_links(i), True)
                    End If
                End If
            Next
            view.gadd_links.Clear()
            view.gdel_links.Clear()

            If view.new_entity = False Then
                Dim name As String
                name = view.Lentities.SelectedItems(0).Text
                load_entity_parts(name, Me.class_name, True)

            Else
                view.associated_entities(0).write_mode = bc_om_entity.DELETE
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    view.associated_entities(0).db_write()
                Else
                    view.associated_entities(0).tmode = bc_om_entity.tWRITE
                    If view.associated_entities(0).transmit_to_server_and_receive(view.associated_entities(0), True) = False Then
                        omsg = New bc_cs_message("Blue Curve", "Failed to delete entity " + view.associated_entities(0).name + " check log file", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                        Exit Sub
                    End If
                End If
                For i = 0 To container.oEntities.entity.Count - 1
                    If container.oEntities.entity(i).id = view.associated_entities(0).id Then
                        container.oEntities.entity.RemoveAt(i)
                        Exit For
                    End If
                Next
                view.new_entity = False
                new_entity = False
                view.Lentities.Items.RemoveAt(view.Lentities.SelectedItems(0).Index)
                view.unload_entity()

            End If
            REM EFG June 2012
            view.lvwvalidate.Items.Clear()
            view.lvwvalidate.Enabled = False


        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bentdiscard", "click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

            exit_entity_edit_mode()
            Me.load_entity_toolbar()

            view.tfilter.SearchRefresh(True)
            Me.container.oEntities = bc_am_load_objects.obc_entities
            view.Cursor = Cursors.Default
        End Try

    End Sub
    REM EFG June 2012
    Public Function commit_changes() As Boolean
        view.tattributes.SelectedIndex = 0
        check_warnings(True)
        If view.warnings.Count > 0 Then
            view.lvwvalidate.Enabled = True
            commit_changes = False
        Else
            commit_changes = True
            exit_entity_edit_mode()
            view.new_entity = False
            view.gadd_links.Clear()
            view.gdel_links.Clear()
            view.lvwvalidate.Enabled = False
            Dim na As String
            na = view.Lentities.SelectedItems(0).Text

            Dim ca As String
            ca = view.cclass.Text
            'view.cclass.SelectedIndex = -1
            'view.lentities.SelectedIndex = -1
            'view.cclass.Text = ca

            REM ----------------------
            view.pentities.Enabled = True
            Me.load_entity_toolbar()
            reset_changes_icons()
            container.set_sync = True
            container.oAdmin.sync_types.sync_types(2).sync_set = True

        End If
    End Function
    Private Sub reset_changes_icons()
        For j = 0 To view.associated_entities(view.tassociations.SelectedIndex).attribute_values.count - 1

            If view.associated_entities(view.tassociations.SelectedIndex).attribute_values(j).nullable = True Then
                view.DataGridView1.Item(0, j).Value = New Drawing.Bitmap(1, 1)
            Else
                view.DataGridView1.Item(0, j).Value = view.tabimages.Images(mandatory_icon)
            End If

            If view.associated_entities(view.tassociations.SelectedIndex).attribute_values(j).show_workflow = False Then
                view.DataGridView1.Item(4, j).Value = New Drawing.Bitmap(1, 1)
            Else
                view.DataGridView1.Item(4, j).Value = view.tabimages.Images(push_publish_icon)
            End If
        Next
    End Sub
    Private Sub compile_warnings()
        Try

            Dim found As Boolean = False
            Dim str As String = ""
            view.warnings.Clear()
            For i = 0 To view.tassociations.TabPages.Count - 1
                If i = 0 Then
                    view.tassociations.TabPages(i).ImageIndex = data_icon

                Else
                    If Me.is_parent_class(Me.class_name, i) = True Then
                        view.tassociations.TabPages(i).ImageIndex = 0
                    Else
                        view.tassociations.TabPages(i).ImageIndex = 0
                    End If
                End If
            Next

            REM validate all associated entities
            For i = 0 To view.associated_entities.Count - 1

                REM check mandatory attributes are filled out
                REM entity attributes
                For j = 0 To view.associated_entities(i).attribute_values.count - 1
                    If view.associated_entities(i).attribute_values(j).nullable = False Then
                        If view.associated_entities(i).attribute_values(j).value = "" Then
                            Dim owarning As New warning
                            For k = 0 To container.oClasses.attribute_pool.Count - 1
                                If container.oClasses.attribute_pool(k).attribute_id = view.associated_entities(i).attribute_values(j).attribute_id Then
                                    owarning.msg = view.associated_entities(i).name + " requires  " + container.oClasses.attribute_pool(k).name + " to be entered"
                                    Exit For
                                End If
                            Next
                            owarning.entity = view.associated_entities(i).name
                            owarning.association = view.associated_entities(i).class_name
                            owarning.association_tab = i
                            owarning.entity_tab = -1
                            owarning.attribute = j
                            owarning.attribute_tab = 0
                            view.warnings.Add(owarning)
                        End If
                    End If

                    REM data type check
                    If view.associated_entities(i).attribute_values(j).value <> "" Then
                        For k = 0 To container.oClasses.attribute_pool.Count - 1
                            If container.oClasses.attribute_pool(k).attribute_id = view.associated_entities(i).attribute_values(j).attribute_id Then
                                If container.oClasses.attribute_pool(k).is_lookup = False Then
                                    Select Case container.oClasses.attribute_pool(k).type_id

                                        Case 2
                                            If IsNumeric(view.associated_entities(i).attribute_values(j).value) = False Then
                                                Dim owarning As New warning
                                                owarning.msg = view.associated_entities(i).name + " requires  " + container.oClasses.attribute_pool(k).name + " to be numeric"
                                                owarning.entity = view.associated_entities(i).name
                                                owarning.association = view.associated_entities(i).class_name
                                                owarning.association_tab = i
                                                owarning.entity_tab = -1
                                                owarning.attribute = j
                                                owarning.attribute_tab = 0
                                                view.warnings.Add(owarning)
                                            End If

                                        Case 5
                                            If IsDate(view.associated_entities(i).attribute_values(j).value) = False Then
                                                Dim owarning As New warning
                                                owarning.msg = view.associated_entities(i).name + " requires  " + container.oClasses.attribute_pool(k).name + " to be date"
                                                owarning.entity = view.associated_entities(i).name
                                                owarning.association = view.associated_entities(i).class_name
                                                owarning.association_tab = i
                                                owarning.entity_tab = -1
                                                owarning.attribute = j
                                                owarning.attribute_tab = 0
                                                view.warnings.Add(owarning)
                                            End If
                                    End Select
                                End If
                                Exit For
                            End If
                        Next
                    End If
                Next

                REM class link attributes
                For j = 0 To view.associated_entities(i).parent_entities.count - 1
                    For k = 0 To view.associated_entities(i).parent_entities(j).linked_attribute_values.count - 1
                        If view.associated_entities(i).parent_entities(j).linked_attribute_values(k).nullable = False Then
                            If view.associated_entities(i).parent_entities(j).linked_attribute_values(k).value = "" Then
                                Dim owarning As New warning
                                For l = 0 To container.oClasses.attribute_pool.Count - 1
                                    If container.oClasses.attribute_pool(l).attribute_id = view.associated_entities(i).parent_entities(j).linked_attribute_values(k).attribute_id Then
                                        For p = 0 To container.oEntities.entity.Count - 1
                                            If container.oEntities.entity(p).id = view.associated_entities(i).parent_entities(j).id Then
                                                owarning.msg = view.associated_entities(i).name + " linked to " + container.oEntities.entity(p).name + " requires :  " + container.oClasses.attribute_pool(l).name + " to be entered"
                                                owarning.linked_class_name = container.oEntities.entity(p).class_name
                                                owarning.linked_entity_name = container.oEntities.entity(p).name

                                                Exit For
                                            End If
                                        Next
                                        Exit For
                                    End If
                                Next
                                owarning.entity = view.associated_entities(i).name
                                owarning.association = view.associated_entities(i).class_name
                                owarning.association_tab = i
                                owarning.entity_tab = -1
                                owarning.attribute = k
                                owarning.attribute_tab = 1
                                view.warnings.Add(owarning)
                            Else
                                REM datatype
                                For m = 0 To container.oClasses.attribute_pool.Count - 1
                                    If container.oClasses.attribute_pool(m).attribute_id = view.associated_entities(i).parent_entities(j).linked_attribute_values(k).attribute_id Then
                                        If container.oClasses.attribute_pool(m).is_lookup = False Then
                                            Select Case container.oClasses.attribute_pool(m).type_id

                                                Case 2
                                                    If IsNumeric(view.associated_entities(i).parent_entities(j).linked_attribute_values(k).value) = False Then
                                                        Dim owarning As New warning
                                                        For p = 0 To container.oEntities.entity.Count - 1
                                                            If container.oEntities.entity(p).id = view.associated_entities(i).parent_entities(j).id Then
                                                                owarning.msg = view.associated_entities(i).name + " linked to " + container.oEntities.entity(p).name + " requires :  " + container.oClasses.attribute_pool(m).name + " to be a number"
                                                                owarning.linked_class_name = container.oEntities.entity(p).class_name
                                                                owarning.linked_entity_name = container.oEntities.entity(p).name

                                                                Exit For
                                                            End If
                                                        Next
                                                        owarning.entity = view.associated_entities(i).name
                                                        owarning.association = view.associated_entities(i).class_name
                                                        owarning.association_tab = i
                                                        owarning.entity_tab = -1
                                                        owarning.attribute = k
                                                        owarning.attribute_tab = 1
                                                        view.warnings.Add(owarning)
                                                    End If

                                                Case 5
                                                    If IsDate(view.associated_entities(i).parent_entities(j).linked_attribute_values(k).value) = False Then
                                                        Dim owarning As New warning
                                                        For p = 0 To container.oEntities.entity.Count - 1
                                                            If container.oEntities.entity(p).id = view.associated_entities(i).parent_entities(j).id Then
                                                                owarning.msg = view.associated_entities(i).name + " linked to " + container.oEntities.entity(p).name + " requires :  " + container.oClasses.attribute_pool(m).name + " to be a date"
                                                                owarning.linked_class_name = container.oEntities.entity(p).class_name
                                                                owarning.linked_entity_name = container.oEntities.entity(p).name
                                                                Exit For
                                                            End If
                                                        Next
                                                        owarning.entity = view.associated_entities(i).name
                                                        owarning.association = view.associated_entities(i).class_name
                                                        owarning.association_tab = i
                                                        owarning.entity_tab = -1
                                                        owarning.attribute = k
                                                        owarning.attribute_tab = 1
                                                        view.warnings.Add(owarning)
                                                    End If
                                            End Select

                                        End If
                                    End If
                                Next


                            End If
                        End If
                    Next
                Next
                For j = 0 To view.associated_entities(i).child_entities.count - 1
                    For k = 0 To view.associated_entities(i).child_entities(j).linked_attribute_values.count - 1
                        If view.associated_entities(i).child_entities(j).linked_attribute_values(k).nullable = False Then
                            If view.associated_entities(i).child_entities(j).linked_attribute_values(k).value = "" Then
                                Dim owarning As New warning
                                For l = 0 To container.oClasses.attribute_pool.Count - 1
                                    If container.oClasses.attribute_pool(l).attribute_id = view.associated_entities(i).child_entities(j).linked_attribute_values(k).attribute_id Then
                                        For p = 0 To container.oEntities.entity.Count - 1
                                            If container.oEntities.entity(p).id = view.associated_entities(i).child_entities(j).id Then
                                                owarning.msg = view.associated_entities(i).name + " linked to " + container.oEntities.entity(p).name + " requires :  " + container.oClasses.attribute_pool(l).name + " to be entered"
                                                owarning.linked_class_name = container.oEntities.entity(p).class_name
                                                owarning.linked_entity_name = container.oEntities.entity(p).name

                                                Exit For
                                            End If
                                        Next
                                        Exit For
                                    End If
                                Next
                                owarning.entity = view.associated_entities(i).name
                                owarning.association = view.associated_entities(i).class_name
                                owarning.association_tab = i
                                owarning.entity_tab = -1
                                owarning.attribute = k
                                owarning.attribute_tab = 1
                                view.warnings.Add(owarning)
                            Else
                                For m = 0 To container.oClasses.attribute_pool.Count - 1
                                    If container.oClasses.attribute_pool(m).attribute_id = view.associated_entities(i).child_entities(j).linked_attribute_values(k).attribute_id Then
                                        If container.oClasses.attribute_pool(m).is_lookup = False Then
                                            Select Case container.oClasses.attribute_pool(m).type_id

                                                Case 2
                                                    If IsNumeric(view.associated_entities(i).child_entities(j).linked_attribute_values(k).value) = False Then
                                                        Dim owarning As New warning
                                                        For p = 0 To container.oEntities.entity.Count - 1
                                                            If container.oEntities.entity(p).id = view.associated_entities(i).child_entities(j).id Then
                                                                owarning.msg = view.associated_entities(i).name + " linked to " + container.oEntities.entity(p).name + " requires :  " + container.oClasses.attribute_pool(m).name + " to be a number"
                                                                owarning.linked_class_name = container.oEntities.entity(p).class_name
                                                                owarning.linked_entity_name = container.oEntities.entity(p).name

                                                                Exit For
                                                            End If
                                                        Next
                                                        owarning.entity = view.associated_entities(i).name
                                                        owarning.association = view.associated_entities(i).class_name
                                                        owarning.association_tab = i
                                                        owarning.entity_tab = -1
                                                        owarning.attribute = k
                                                        owarning.attribute_tab = 1
                                                        view.warnings.Add(owarning)
                                                    End If

                                                Case 5
                                                    If IsDate(view.associated_entities(i).child_entities(j).linked_attribute_values(k).value) = False Then
                                                        Dim owarning As New warning
                                                        For p = 0 To container.oEntities.entity.Count - 1
                                                            If container.oEntities.entity(p).id = view.associated_entities(i).child_entities(j).id Then
                                                                owarning.msg = view.associated_entities(i).name + " linked to " + container.oEntities.entity(p).name + " requires :  " + container.oClasses.attribute_pool(m).name + " to be a date"
                                                                owarning.linked_class_name = container.oEntities.entity(p).class_name
                                                                owarning.linked_entity_name = container.oEntities.entity(p).name
                                                                Exit For
                                                            End If
                                                        Next
                                                        owarning.entity = view.associated_entities(i).name
                                                        owarning.association = view.associated_entities(i).class_name
                                                        owarning.association_tab = i
                                                        owarning.entity_tab = -1
                                                        owarning.attribute = k
                                                        owarning.attribute_tab = 1
                                                        view.warnings.Add(owarning)
                                                    End If
                                            End Select

                                        End If
                                    End If
                                Next



                                REM ---
                            End If
                        End If
                    Next
                Next
                'REM class link attributes

                For j = 0 To container.oClasses.classes.Count - 1
                    If container.oClasses.classes(j).class_id = view.associated_entities(i).class_id Then
                        Dim co As Integer = 0

                        For k = 0 To container.oClasses.classes(j).parent_links.count - 1
                            If container.oClasses.classes(j).parent_links(k).schema_id = Me.schema_id Then
                                found = False
                                If container.oClasses.classes(j).parent_links(k).mandatory_cp = 0 Then
                                    found = True
                                ElseIf container.oClasses.classes(j).parent_links(k).mandatory_cp = -1 Then
                                    REM now check entity has at least one entity of this class claffified against it
                                    For l = 0 To view.associated_entities(i).parent_entities.count - 1
                                        If view.associated_entities(i).parent_entities(l).class_id = container.oClasses.classes(j).parent_links(k).parent_class_id Then
                                            found = True
                                            Exit For
                                        End If
                                    Next
                                Else
                                    Dim eco As Integer = 0
                                    For l = 0 To view.associated_entities(i).parent_entities.count - 1
                                        If view.associated_entities(i).parent_entities(l).class_id = container.oClasses.classes(j).parent_links(k).parent_class_id Then
                                            eco = eco + 1
                                        End If
                                    Next
                                    If eco = container.oClasses.classes(j).parent_links(k).mandatory_cp Then
                                        found = True
                                    End If
                                End If
                                If found = False Then
                                    For m = 0 To container.oClasses.classes.Count - 1
                                        If container.oClasses.classes(m).class_id = container.oClasses.classes(j).parent_links(k).parent_class_id And container.oClasses.classes(j).parent_links(k).schema_id = Me.schema_id Then
                                            Dim owarning As New warning
                                            If container.oClasses.classes(j).parent_links(k).mandatory_cp = -1 Then
                                                owarning.msg = view.associated_entities(i).name + " requires assignment of at least one " + container.oClasses.classes(m).class_name
                                            Else
                                                owarning.msg = view.associated_entities(i).name + " requires assignment of exactly " + CStr(container.oClasses.classes(j).parent_links(k).mandatory_cp) + " " + container.oClasses.classes(m).class_name + "(s)"
                                            End If
                                            owarning.entity = view.associated_entities(i).name
                                            owarning.association = container.oClasses.classes(m).class_name
                                            owarning.association_tab = i
                                            co = 0
                                            For n = 0 To container.oClasses.classes(j).parent_links.count - 1
                                                If container.oClasses.classes(j).parent_links(n).schema_id = Me.schema_id Then
                                                    If container.oClasses.classes(j).parent_links(n).parent_class_id = container.oClasses.classes(m).class_id Then
                                                        owarning.entity_tab = co
                                                    End If
                                                    co = co + 1
                                                End If
                                            Next
                                            view.warnings.Add(owarning)
                                            Exit For
                                        End If
                                    Next
                                End If

                            End If
                        Next
                        For k = 0 To container.oClasses.classes(j).child_links.count - 1
                            If container.oClasses.classes(j).child_links(k).schema_id = Me.schema_id Then
                                found = False
                                If container.oClasses.classes(j).child_links(k).mandatory_pc = 0 Then
                                    found = True
                                ElseIf container.oClasses.classes(j).child_links(k).mandatory_pc = -1 Then
                                    REM now check entity has at least one entity of this class claffified against it
                                    For l = 0 To view.associated_entities(i).child_entities.count - 1
                                        If view.associated_entities(i).child_entities(l).class_id = container.oClasses.classes(j).child_links(k).child_class_id Then
                                            found = True
                                            Exit For
                                        End If
                                    Next
                                Else
                                    Dim eco As Integer = 0
                                    For l = 0 To view.associated_entities(i).child_entities.count - 1
                                        If view.associated_entities(i).child_entities(l).class_id = container.oClasses.classes(j).child_links(k).child_class_id Then
                                            eco = eco + 1
                                        End If
                                    Next
                                    If eco = container.oClasses.classes(j).child_links(k).mandatory_pc Then
                                        found = True
                                    End If
                                End If
                                If found = False Then
                                    For m = 0 To container.oClasses.classes.Count - 1
                                        If container.oClasses.classes(m).class_id = container.oClasses.classes(j).child_links(k).child_class_id Then
                                            Dim owarning As New warning
                                            If container.oClasses.classes(j).child_links(k).mandatory_pc = -1 Then
                                                owarning.msg = view.associated_entities(i).name + " requires assignment of at least one " + container.oClasses.classes(m).class_name
                                            Else
                                                owarning.msg = view.associated_entities(i).name + " requires assignment of exactly " + CStr(container.oClasses.classes(j).child_links(k).mandatory_pc) + " " + container.oClasses.classes(m).class_name + "(s)"
                                            End If
                                            owarning.entity = view.associated_entities(i).name
                                            owarning.association = container.oClasses.classes(m).class_name
                                            owarning.association_tab = i
                                            Dim cco As Integer = 0
                                            co = 0
                                            For n = 0 To container.oClasses.classes(j).parent_links.count - 1
                                                If container.oClasses.classes(j).parent_links(n).schema_id = Me.schema_id Then

                                                    co = co + 1
                                                End If
                                            Next
                                            For n = 0 To container.oClasses.classes(j).child_links.count - 1
                                                If container.oClasses.classes(j).child_links(n).schema_id = Me.schema_id Then
                                                    If container.oClasses.classes(j).child_links(n).child_class_id = container.oClasses.classes(m).class_id Then
                                                        owarning.entity_tab = co + cco
                                                        Exit For
                                                    End If
                                                    cco = cco + 1
                                                End If
                                            Next
                                            view.warnings.Add(owarning)
                                            Exit For
                                        End If
                                    Next
                                End If
                            End If
                        Next
                        Exit For
                    End If
                Next

                REM check links have been referanced

            Next
            view.tattributes.TabPages(0).ImageIndex = attributes_icon
            view.tattributes.SelectedIndex = 0

            For i = 1 To view.tattributes.TabPages.Count - 1
                view.tattributes.TabPages.RemoveAt(1)
            Next
            view.lvwvalidate.Items.Clear()
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_taxonomy", "compile_warnings", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try

    End Sub
    REM EFG June 2012
    Private Sub show_warnings()
        Try
            Dim lwarn As ListViewItem
            view.lvwvalidate.Items.Clear()
            view.lassociations.TabPages(0).ImageIndex = 21
            For i = 0 To view.warnings.Count - 1

                lwarn = New ListViewItem(CStr(view.warnings(i).msg))
                view.tassociations.TabPages(view.warnings(i).association_tab).imageindex = warning_icon

                If view.warnings(i).entity_tab > -1 Then

                    lwarn.ImageIndex = warning_icon
                    If view.warnings(i).association_tab = view.tassociations.SelectedIndex Then
                        view.tentities.Nodes(view.warnings(i).entity_tab).imageindex = warning_icon
                        view.lassociations.TabPages(0).ImageIndex = warning_icon

                    End If
                Else
                    lwarn.ImageIndex = warning_icon
                    If view.warnings(i).association_tab = view.tassociations.SelectedIndex Then
                        If view.warnings(i).attribute_tab = 0 Then
                            view.DataGridView1.Item(0, view.warnings(i).attribute).Value = view.tabimages.Images(warning_icon)
                            view.tattributes.TabPages(view.warnings(i).attribute_tab).ImageIndex = warning_icon
                        End If
                    End If
                End If
                view.lvwvalidate.Items.Add(lwarn)
            Next


            If view.warnings.Count = 0 Then
                view.lvwvalidate.Columns(0).Text = "Warnings"
            Else
                view.lvwvalidate.Columns(0).Text = CStr(view.warnings.Count) + " Warnings"
            End If
            REM --------------
            If view.warnings.Count = 0 Then
                view.lvwvalidate.Enabled = False
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_taxonomy", "show_warnings", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try

    End Sub
    Private Sub set_Warnings()
        view.lassociations.TabPages(0).ImageIndex = 21
        For i = 0 To view.warnings.Count - 1
            view.tassociations.TabPages(view.warnings(i).association_tab).imageindex = warning_icon

            If view.warnings(i).entity_tab > -1 Then
                If view.warnings(i).association_tab = view.tassociations.SelectedIndex Then
                    view.tentities.Nodes(view.warnings(i).entity_tab).imageindex = warning_icon
                    view.lassociations.TabPages(0).ImageIndex = warning_icon

                End If
            Else
                If view.warnings(i).association_tab = view.tassociations.SelectedIndex Then
                    If view.warnings(i).attribute_tab = 0 Then
                        view.DataGridView1.Item(0, view.warnings(i).attribute).Value = view.tabimages.Images(warning_icon)
                        view.tattributes.TabPages(view.warnings(i).attribute_tab).ImageIndex = warning_icon
                    End If
                End If
            End If
        Next
    End Sub
    Private Sub check_warnings(Optional ByVal show_msg As Boolean = False)
        Try
            REM make sure alll atrtibutes are set up
            Dim i As Integer
            For i = 1 To view.tassociations.TabPages.Count - 1
                view.tassociations.SelectedIndex = i
            Next
            view.tassociations.SelectedIndex = 0
            compile_warnings()
            If view.warnings.Count > 0 Then
                Dim omsg As New bc_cs_message("Blue Curve", "Changes cannot be applied. Please enter the missing or invalid attributes/associations.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                view.Cursor = Cursors.WaitCursor
                show_warnings()
            Else
                write_associated_entities()
                reset_change_indicator()
                view.tassociations.SelectedIndex = 0
                REM PRRRRRR
                'Me.set_selected_entity(view.associated_entities(0).name)

                REM load_entity_parts(view.associated_entities(0).name, view.associated_entities(0).class_id, t)
                REM take out associated entities
                For i = 1 To view.associated_entities.Count - 1
                    view.associated_entities.RemoveAt(1)
                Next
                For i = 1 To view.tassociations.TabPages.Count - 1
                    view.tassociations.TabPages.RemoveAt(1)
                Next
                Dim omsg As New bc_cs_message("Blue Curve", view.Lentities.SelectedItems(0).Text + " updated sucessfully.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_cp_main", "check_warnings", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally

        End Try
    End Sub
    Private Sub write_associated_entities()
        Try
            For i = 0 To view.associated_entities.Count - 1

                view.associated_entities(i).schema_id = Me.schema_id
                view.associated_entities(i).write_mode = 2
                view.associated_entities(i).certificate.user_id = bc_cs_central_settings.logged_on_user_id
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    view.associated_entities(i).db_write()
                Else
                    view.associated_entities(i).tmode = bc_om_entity.tWRITE
                    view.associated_entities(i).transmit_to_server_and_receive(view.associated_entities(i), True)
                End If
            Next
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_cp_main", "write_associated_entities", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally

        End Try
    End Sub

    Private Sub reset_change_indicator()
        For i = 0 To view.associated_entities.Count - 1
            For j = 0 To view.associated_entities(i).attribute_values.count - 1
                view.associated_entities(i).attribute_values(j).value_changed = False
            Next
        Next
    End Sub
    Public Sub add_entity_menu()
        Try
            Dim fedit As New bc_am_cp_edit
            fedit.ltitle.Text = "Enter new " + view.cclass.Text

            For i = 0 To Me.container.oClasses.classes.Count - 1
                If Me.container.oClasses.classes(i).class_name = view.cclass.Text AndAlso Me.container.oClasses.classes(i).entry_description <> "" Then
                    fedit.ltitle.Text = Me.container.oClasses.classes(i).entry_description
                End If
            Next

            fedit.ShowDialog()
            If fedit.cancel_selected = True Then
                Exit Sub
            End If
            view.Cursor = Cursors.WaitCursor
            If Me.add_entity(fedit.Tentry.Text, view.cclass.Text, False) = False Then
                Exit Sub
            End If

            load_entities_of_class(class_name)
            Me.set_selected_entity(fedit.Tentry.Text)

            view.associated_entities(0).name = fedit.Tentry.Text

            set_default_attribute_values("", False)

            view.new_entity = True
            Me.new_entity = True
            entity_edit_mode()
        Catch
        Finally
            view.Cursor = Cursors.Default
        End Try

    End Sub
    Public Sub change_entity_menu()
        Try
            Dim fedit As New bc_am_cp_edit
            fedit.ltitle.Text = "Enter new name for " + view.Lentities.SelectedItems(0).Text
            fedit.ShowDialog()
            If fedit.cancel_selected = True Then
                Exit Sub
            End If
            Me.change_entity(fedit.Tentry.Text)
        Catch

        Finally
            view.Cursor = Cursors.Default
        End Try
    End Sub
    Public Sub show_audit()
        Dim fs As New bc_am_entity_user_audit
        Dim cfs As New Cbc_am_entity_user_audit
        If cfs.load_data(fs, view.associated_entities(0).id, bc_om_entity_user_audit.EKEY_TYPE.ENTITY, view.associated_entities(0).name) = True Then
            fs.ShowDialog()
        End If
    End Sub
    Public Function add_entity(ByVal name As String, ByVal class_name As String, from_assign As Boolean) As Boolean
        Try
            REM check not a duplicate
            add_entity = False
            Dim oentity As New bc_om_entity
            oentity.class_id = get_class_id(class_name)
            oentity.name = name
            oentity.class_name = class_name
            Dim i As Integer

            For i = 0 To container.oEntities.entity.Count - 1
                If container.oEntities.entity(i).class_id = oentity.class_id And Trim(UCase(container.oEntities.entity(i).name)) = Trim(UCase(name)) Then
                    Dim osg As New bc_cs_message("Blue Curve", class_name + " already exists please try again!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Function
                End If
            Next
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                oentity.db_write()
            Else
                oentity.tmode = bc_cs_soap_base_class.tWRITE
                If (oentity.transmit_to_server_and_receive(oentity, True) = False) Then
                    Exit Function
                End If
            End If

            container.oEntities.entity.Add(oentity)





            add_entity = True
        Catch ex As Exception

        Finally

        End Try
    End Function
    Public Sub change_entity(ByVal name As String)
        REM check not a duplicate
        view.associated_entities(0).name = name

        Dim i As Integer
        For i = 0 To container.oEntities.entity.Count - 1
            If container.oEntities.entity(i).class_id = view.associated_entities(0).class_id And Trim(UCase(container.oEntities.entity(i).name)) = Trim(UCase(name)) And container.oEntities.entity(i).id <> view.associated_entities(0).id Then
                Dim osg As New bc_cs_message("Blue Curve", class_name + ": " + name + " already exists please try again!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Sub
            End If
        Next
        view.associated_entities(0).write_mode = bc_om_entity.UPDATE
        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
            view.associated_entities(0).db_write()
        Else
            view.associated_entities(0).tmode = bc_cs_soap_base_class.tWRITE
            If (view.associated_entities(0).transmit_to_server_and_receive(view.associated_entities(0), True) = False) Then
                Exit Sub
            End If
        End If

        For i = 0 To container.oEntities.entity.Count - 1
            If container.oEntities.entity(i).id = view.associated_entities(0).id Then
                container.oEntities.entity(i).name = name
                Exit For
            End If
        Next
        load_entities_of_class(class_name)
        Me.set_selected_entity(name)

    End Sub
    Private Sub load_entities_of_class(ByVal class_name As String)
        Try

            'Dim i As Integer
            Dim class_id As Long
            class_id = get_class_id(class_name)
            view.tfilter.SearchText = ""

            'Dim lvew As ListViewItem

            view.Lentities.Visible = False
            view.Lentities.Items.Clear()
            view.Lentities.Sorting = SortOrder.None

            view.tfilter.SearchClass = class_id

            view.tfilter.SearchRefresh(True)

            'PR FIL JULY 2012
            Me.container.oEntities = bc_am_load_objects.obc_entities


            'For i = 0 To Me.container.oEntities.entity.Count - 1
            '    If Me.container.oEntities.entity(i).class_id = class_id Then
            '        If Me.container.oEntities.entity(i).inactive = False Then
            '            lvew = New ListViewItem(CStr(Me.container.oEntities.entity(i).name), data_icon)

            '        Else
            '            lvew = New ListViewItem(CStr(Me.container.oEntities.entity(i).name) + " (inactive)", inactive_icon)

            '        End If
            '        view.Lentities.Items.Add(lvew)
            '    End If
            'Next
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_taxonomy", "load_entities_of_class", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            view.Lentities.Visible = True
            view.Lentities.Sorting = SortOrder.Ascending
        End Try
    End Sub
    Public Sub unload_entity()
        view.passociations.Enabled = False
        view.tassociations.Enabled = False
        view.tassociations.TabPages(0).Text = "no selection"
        view.tassociations.TabPages(0).ImageIndex = deactivate_icon
        'view.tattributes.TabPages(0).Text = "Attributes"
        view.tattributes.TabPages(0).ImageIndex = attributes_icon
        view.tentities.Nodes.Clear()
    End Sub
    Public Sub activate_entity_menu()
        activate_entity(view.Lentities.SelectedItems(0).Text, view.cclass.Text)
    End Sub
    Public Sub delete_entity_menu()
        delete_entity()
    End Sub
    Public Sub activate_entity(ByVal name As String, ByVal class_name As String)
        Try
            Dim new_na As String = ""
            view.Cursor = Cursors.WaitCursor
            For i = 0 To container.oEntities.entity.Count - 1
                If view.associated_entities(0).id = container.oEntities.entity(i).id Then
                    If InStr(name, "(inactive)") > 0 Then
                        'view.associated_entities(0).inactive = False
                        'container.oEntities.entity(i).inactive = False
                        view.associated_entities(0).write_mode = bc_om_entity.SET_ACTIVE
                        'new_na = name.Substring(0, InStr(name, "(inactive)") - 2)
                    Else
                        'view.associated_entities(0).inactive = True
                        'container.oEntities.entity(i).inactive = True
                        view.associated_entities(0).write_mode = bc_om_entity.SET_INACTIVE
                        'new_na = name + " (inactive)"
                    End If
                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        view.associated_entities(0).db_write()
                    Else
                        view.associated_entities(0).tmode = bc_om_entity.tWRITE
                        view.associated_entities(0).transmit_to_server_and_receive(view.associated_entities(0), True)
                    End If
                    If view.associated_entities(0).deactivate_fail_text <> "" Then
                        Dim omsg As New bc_cs_message("Blue Curve", view.associated_entities(0).deactivate_fail_text, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                        Exit Sub
                    End If

                    If InStr(name, "(inactive)") > 0 Then
                        view.associated_entities(0).inactive = False
                        container.oEntities.entity(i).inactive = False
                        'view.associated_entities(0).write_mode = bc_om_entity.SET_ACTIVE
                        new_na = name.Substring(0, InStr(name, "(inactive)") - 2)
                    Else
                        view.associated_entities(0).inactive = True
                        container.oEntities.entity(i).inactive = True
                        'view.associated_entities(0).write_mode = bc_om_entity.SET_INACTIVE
                        new_na = name + " (inactive)"
                    End If

                    Exit For
                End If
            Next

            Me.load_entities_of_class(class_name)
            set_selected_entity(new_na)

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("minactive", "Click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            view.Cursor = Cursors.Default
        End Try
    End Sub
    Private Sub set_selected_entity(ByVal na As String)
        For i = 0 To view.Lentities.Items.Count - 1
            If view.Lentities.Items(i).Text = na Then
                view.Lentities.Items(i).Selected = True
                view.Lentities.Items(i).EnsureVisible()

                Exit For
            End If
        Next
    End Sub
    Public Sub delete_entity()
        Try
            Dim omsg As New bc_cs_message("Blue Curve", "Are you sure you wish to delete " + view.associated_entities(0).name, bc_cs_message.MESSAGE, True, False, "Yes", "no", True)
            If omsg.cancel_selected = True Then
                Exit Sub
            End If
            view.Cursor = Cursors.WaitCursor
            Dim name As String
            name = view.associated_entities(0).name

            view.associated_entities(0).write_mode = bc_om_entity.DELETE
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                view.associated_entities(0).db_write()
            Else
                view.associated_entities(0).tmode = bc_om_entity.tWRITE
                If view.associated_entities(0).transmit_to_server_and_receive(view.associated_entities(0), True) = False Then
                    omsg = New bc_cs_message("Blue Curve", "Failed to delete entity " + view.associated_entities(0).delete_error + " check log file", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub
                End If
            End If
            For i = 0 To container.oEntities.entity.Count - 1
                If container.oEntities.entity(i).id = view.associated_entities(0).id Then
                    container.oEntities.entity.RemoveAt(i)
                    Exit For
                End If
            Next
            view.Lentities.Items.RemoveAt(view.Lentities.SelectedItems(0).Index)

            Me.unload_entity()

            view.tfilter.SearchRefresh(True)
            Me.container.oEntities = bc_am_load_objects.obc_entities

            omsg = New bc_cs_message("Blue Curve", name + " deleted", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_taxonomy", "delete_entity", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally

            view.Cursor = Cursors.Default
        End Try

    End Sub
    REM ING JUNE 2012
    Public Function validate_attribute_value(ByVal val As String, Optional ByVal tb As Boolean = False) As Boolean
        no_Action = True
        Try
            validate_attribute_value = True
            'If Trim(val) = "" Then
            '    Exit Function
            'End If
            Try
                If view.DataGridView1.SelectedCells(0).ColumnIndex <> 3 And tb = False Then
                    Exit Function
                End If
            Catch
                Exit Function
            End Try
            set_val(val, tb)
            'Dim failed As Boolean = False
            'For i = 0 To container.oClasses.attribute_pool.Count - 1
            '    If container.oClasses.attribute_pool(i).name = view.DataGridView1.Item(1, view.DataGridView1.SelectedCells(0).RowIndex).Value Then
            '        Select Case container.oClasses.attribute_pool(i).type_id And container.oClasses.attribute_pool(i).is_lookup = False
            '            Case 2
            '                If IsNumeric(val) = False Then
            '                    view.DataGridView1.Item(2, view.DataGridView1.SelectedCells(0).RowIndex).Value = "Item must be numeric"
            '                    view.DataGridView1.Item(2, view.DataGridView1.SelectedCells(0).RowIndex).Selected = True
            '                    'Dim omsg As New bc_cs_message("Blue Curve", "Item must be numeric", bc_cs_message.MESSAGE,false,false,"Yes","No",true)
            '                    failed = True
            '                End If
            '            Case 5
            '                If IsDate(val) = False Then
            '                    Dim omsg As New bc_cs_message("Blue Curve", "Item must be Date and Time", bc_cs_message.MESSAGE,false,false,"Yes","No",true)
            '                    failed = True
            '                End If
            '        End Select
            '        If failed = True Then
            '            validate_attribute_value = True
            '        Else
            '            set_val(val)
            '        End If
            '        Exit Function
            '    End If
            'Next
        Catch ex As Exception
            'MsgBox(ex.Message)
        Finally
            no_Action = False
        End Try
    End Function
    REM ING JUNE 2012
    Public Sub set_val(ByVal val As String, Optional ByVal ext As Boolean = False)
        Try

            If val <> view.associated_entities(view.tassociations.SelectedIndex).attribute_values(view.DataGridView1.SelectedCells(0).RowIndex).value Then

                REM see if item is a key value pair
                For i = 0 To container.oClasses.attribute_pool.Count - 1
                    If container.oClasses.attribute_pool(i).name = view.DataGridView1.Item(1, view.DataGridView1.SelectedCells(0).RowIndex).Value Then
                        If container.oClasses.attribute_pool(i).lookup_keys.count > 0 Then
                            REM find key value for value
                            For j = 0 To container.oClasses.attribute_pool(i).lookup_values.count - 1
                                If RTrim(LTrim(container.oClasses.attribute_pool(i).lookup_values(j))) = RTrim(LTrim(val)) Then
                                    val = container.oClasses.attribute_pool(i).lookup_keys(j)
                                End If
                            Next
                        End If
                        Exit For
                    End If

                Next
                view.associated_entities(view.tassociations.SelectedIndex).attribute_values(view.DataGridView1.SelectedCells(0).RowIndex).value = val
                view.associated_entities(view.tassociations.SelectedIndex).attribute_values(view.DataGridView1.SelectedCells(0).RowIndex).value_changed = True
                view.DataGridView1.Item(0, view.DataGridView1.SelectedCells(0).RowIndex).Value = view.tabimages.Images(att_edited_icon)
                If ext = True Then
                    view.DataGridView1.Item(3, view.DataGridView1.SelectedCells(0).RowIndex).Value = val
                End If

                REM if item workflowed and publish requested reset this
                For i = 0 To container.oClasses.attribute_pool.Count - 1
                    If container.oClasses.attribute_pool(i).name = view.DataGridView1.Item(1, view.DataGridView1.SelectedCells(0).RowIndex).Value Then

                        If container.oClasses.attribute_pool(i).show_workflow = 1 Then
                            If view.associated_entities(view.tassociations.SelectedIndex).attribute_values(view.DataGridView1.SelectedCells(0).RowIndex).publish_draft_value = True Then
                                view.associated_entities(view.tassociations.SelectedIndex).attribute_values(view.DataGridView1.SelectedCells(0).RowIndex).publish_draft_value = False


                                view.DataGridView1.Item(5, view.DataGridView1.SelectedCells(0).RowIndex).Value = view.associated_entities(view.tassociations.SelectedIndex).attribute_values(view.DataGridView1.SelectedCells(0).RowIndex).original_published_value

                                view.DataGridView1.Item(4, view.DataGridView1.SelectedCells(0).RowIndex).Value = view.tabimages.Images(push_publish_icon)

                            End If
                        End If
                        Exit For
                    End If
                Next
                Me.entity_edit_mode()

            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_taxonomy", "set_val", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    REM ING JUNE 2012
    Public Function set_data_type_Control() As Boolean
        Try





            Dim length As Integer
            If setting_text_box = True Then
                Exit Function

            End If
            Dim lus As New ArrayList
            set_data_type_Control = False
            If no_Action = True Then
                Exit Function
            End If


            If view.DataGridView1.ColumnCount <> 6 Then
                Exit Function
            End If
            REM temporary hold current values
            hold_vals.Clear()
            For i = 0 To view.DataGridView1.Rows.Count - 1
                hold_vals.Add(view.DataGridView1.Item(3, i).Value)
                If IsNothing(hold_vals(i)) Then
                    hold_vals(i) = ""
                End If
            Next

            Dim lu As DataGridViewComboBoxCell
            Dim use_combo As Boolean = False
            Dim use_date As Boolean = False
            Dim dtextbox As New DataGridViewTextBoxColumn
            dtextbox.SortMode = DataGridViewColumnSortMode.NotSortable



            Dim Dcombobox As New DataGridViewComboBoxColumn
            Dim row_id As Integer
            row_id = view.DataGridView1.SelectedCells(0).RowIndex

            Dim j As Integer
            REM 1 string 2 number 3 boolean 5 date
            view.DataGridView1.Item(1, row_id).Selected = True


            REM read only attribute
            'For jj = 0 To Me.container.oClasses.classes.Count - 1
            '    If Me.container.oClasses.classes(jj).class_id = view.associated_entities(view.tassociations.SelectedIndex).class_id Then
            '        If Me.container.oClasses.classes(jj).attributes(view.DataGridView1.SelectedCells(0).RowIndex).permission = 1 Then
            '            Exit Function
            '        End If
            '        Exit For
            '    End If
            'Next


            REM build up array of all lookup values
            Dim lu_found As Boolean = False
            For i = 0 To view.DataGridView1.Rows.Count - 1

                lu_found = False
                For j = 0 To container.oClasses.attribute_pool.Count - 1
                    If container.oClasses.attribute_pool(j).name = view.DataGridView1.Item(1, i).Value Then

                        If container.oClasses.attribute_pool(j).is_lookup = True Then

                            lu = New DataGridViewComboBoxCell
                            lu.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing

                            For k = 0 To container.oClasses.attribute_pool(j).lookup_values.count - 1
                                lu.Items.Add(container.oClasses.attribute_pool(j).lookup_values(k))
                                If (container.oClasses.attribute_pool(j).lookup_values(k) = view.DataGridView1.Item(3, i).Value) Then
                                    lu_found = True
                                End If
                            Next

                            lu.Items.Add("")

                            If lu_found = False Then

                                Try
                                    lu.Items.Add(view.DataGridView1.Item(3, i).Value)
                                Catch ex As Exception


                                End Try

                            End If
                            lus.Add(lu)

                        ElseIf container.oClasses.attribute_pool(j).type_id = 3 Then
                            lu = New DataGridViewComboBoxCell
                            lu.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing

                            lu.Items.Add("")
                            lu.Items.Add("True")
                            lu.Items.Add("False")
                            If hold_vals(i) <> "True" And hold_vals(i) <> "False" Then
                                hold_vals(i) = ""
                            End If
                            lus.Add(lu)
                        Else
                            lu = New DataGridViewComboBoxCell
                            lu.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing

                            If IsNothing(hold_vals(i)) Then
                                lu.Items.Add("")
                            Else
                                lu.Items.Add(hold_vals(i))
                            End If
                            lus.Add(lu)

                        End If
                        Exit For
                    End If
                Next
            Next

            Dim beditbox As Boolean = False
            Dim bdatebox As Boolean = False
            For i = 0 To container.oClasses.attribute_pool.Count - 1
                If container.oClasses.attribute_pool(i).name = view.DataGridView1.Item(1, view.DataGridView1.SelectedCells(0).RowIndex).Value Then
                    length = container.oClasses.attribute_pool(i).length

                    If container.oClasses.attribute_pool(i).is_lookup = True Then
                        use_combo = True
                    ElseIf setting_text_box = False And container.oClasses.attribute_pool(i).type_id = 1 And container.oClasses.attribute_pool(i).repeats = 1 Then
                        Dim fedit As New bc_am_text_edit
                        beditbox = True
                        fedit.Text = "Edit: " + container.oClasses.attribute_pool(i).name
                        fedit.maxlength = container.oClasses.attribute_pool(i).length
                        fedit.ttextentry.MaxLength = container.oClasses.attribute_pool(i).length
                        fedit.ttextentry.Text = view.DataGridView1.Item(3, view.DataGridView1.SelectedCells(0).RowIndex).Value
                        fedit.Llength.Text = "Maximum length: " + CStr(container.oClasses.attribute_pool(i).length)
                        setting_text_box = True
                        'fedit.Initializing = False
                        fedit.ShowDialog()
                        If fedit.ok_selected = True Then
                            setting_text_box = True
                            validate_attribute_value(fedit.ttextentry.Text, True)
                        End If
                        view.DataGridView1.Item(0, row_id).Selected = True
                        setting_text_box = False
                    ElseIf container.oClasses.attribute_pool(i).repeats = 2 Then
                        setting_text_box = True
                        Dim fsa As New bc_dx_step_attribute
                        Dim csa As New Cbc_dx_step_attribute
                        If csa.load_data(fsa, container.oClasses.attribute_pool(i).name, view.associated_entities(view.tassociations.SelectedIndex).attribute_values(view.DataGridView1.SelectedCells(0).RowIndex).steps) = True Then
                            fsa.ShowDialog()
                            If csa.bsave = True Then
                                view.associated_entities(view.tassociations.SelectedIndex).attribute_values(view.DataGridView1.SelectedCells(0).RowIndex).steps = csa._steps
                                REM set value to step in range
                                Dim def As String = ""
                                For s = 0 To csa._steps.Count - 1
                                    With csa._steps(s)
                                        If Now.ToUniversalTime >= .date_from And Now.ToUniversalTime < .date_to Then
                                            def = .value
                                            Exit For
                                        End If
                                    End With
                                Next
                                validate_attribute_value(def, True)
                                view.associated_entities(view.tassociations.SelectedIndex).attribute_values(view.DataGridView1.SelectedCells(0).RowIndex).value_changed = True
                                view.DataGridView1.Item(0, view.DataGridView1.SelectedCells(0).RowIndex).Value = view.tabimages.Images(att_edited_icon)
                                entity_edit_mode()
                            End If
                        End If
                        view.DataGridView1.Item(0, row_id).Selected = True
                        setting_text_box = False
                        Exit Function
                    ElseIf container.oClasses.attribute_pool(i).type_id = 5 Then
                        Dim fdate As New bc_am_dx_date
                        bdatebox = True
                        setting_text_box = True
                        'fedit.Initializing = False
                        If IsDate(view.DataGridView1.Item(3, view.DataGridView1.SelectedCells(0).RowIndex).Value) Then
                            fdate.DateEdit1.EditValue = view.DataGridView1.Item(3, view.DataGridView1.SelectedCells(0).RowIndex).Value
                        Else
                            fdate.DateEdit1.EditValue = Now
                        End If
                        'Dim dp As System.Drawing.Point
                        'dp.X = view.DataGridView1.Location.X

                        'dp.Y = view.DataGridView1.Location.Y


                        fdate.ShowDialog()
                        If fdate.ok_selected = True Then
                            setting_text_box = True
                            validate_attribute_value(fdate.selected_date, True)
                        End If
                        view.DataGridView1.Item(0, row_id).Selected = True
                        setting_text_box = False
                    ElseIf container.oClasses.attribute_pool(i).type_id = 3 Then
                        use_combo = True
                    End If
                    Exit For
                End If
            Next
            REM 

            Dcombobox.Width = view.DataGridView1.Columns(3).Width

            dtextbox.Width = view.DataGridView1.Columns(3).Width
            'Dim ro As Boolean
            'ro = view.DataGridView1.Columns(3).ReadOnly
            If beditbox = False And bdatebox = False Then
                view.DataGridView1.Columns.RemoveAt(3)
                If use_combo = True Then
                    Dcombobox.Name = "Values"
                    view.DataGridView1.Columns.Insert(3, Dcombobox)
                Else
                    dtextbox.Name = "Values"
                    dtextbox.MaxInputLength = length
                    view.DataGridView1.Columns.Insert(3, dtextbox)
                End If
                REM reset value
                no_Action = True
                For i = 0 To view.DataGridView1.Rows.Count - 1
                    If use_combo = True Then
                        view.DataGridView1.Item(3, i) = lus(i)
                    End If
                    If hold_vals(i) <> "" Then
                        view.DataGridView1.Item(3, i).Value = hold_vals(i)
                    End If
                Next
                view.DataGridView1.Item(3, row_id).Selected = True
            End If
            view.DataGridView1.Columns(3).ReadOnly = False

            For jj = 0 To Me.container.oClasses.classes.Count - 1
                If Me.container.oClasses.classes(jj).class_id = view.associated_entities(view.tassociations.SelectedIndex).class_id Then
                    If Me.container.oClasses.classes(jj).attributes(view.DataGridView1.SelectedCells(0).RowIndex).permission = 1 Then
                        view.DataGridView1.Columns(3).ReadOnly = True
                        Exit Function
                    End If
                    Exit For
                End If
            Next

            no_Action = False
            set_data_type_Control = True
        Catch ex As Exception
            Dim ocomm As New bc_cs_activity_log("bc_am_taxonomy", "set_data_type_comtrol", bc_cs_activity_codes.COMMENTARY, "Error: " + ex.Message)



        End Try

    End Function

    Public Sub set_publish()
        Dim idx As Integer
        idx = sel_cls_idx(class_name)
        REM if read only dont allow

        For j = 0 To Me.container.oClasses.classes(idx).attributes.count - 1
            For i = 0 To Me.container.oClasses.attribute_pool.Count - 1
                If Me.container.oClasses.attribute_pool(i).attribute_id = Me.container.oClasses.classes(idx).attributes(j).attribute_id Then
                    If container.oClasses.attribute_pool(i).name = view.DataGridView1.Item(1, view.DataGridView1.SelectedCells(0).RowIndex).Value Then
                        If Me.container.oClasses.classes(idx).attributes(j).permission = 1 Then
                            Exit Sub
                        End If

                    End If
                End If
            Next
        Next

        For i = 0 To container.oClasses.attribute_pool.Count - 1
            If container.oClasses.attribute_pool(i).name = view.DataGridView1.Item(1, view.DataGridView1.SelectedCells(0).RowIndex).Value Then


                If container.oClasses.attribute_pool(i).show_workflow = 1 Then
                    If view.associated_entities(view.tassociations.SelectedIndex).attribute_values(view.DataGridView1.SelectedCells(0).RowIndex).publish_draft_value = False Then
                        view.DataGridView1.Item(5, view.DataGridView1.SelectedCells(0).RowIndex).Value = view.DataGridView1.Item(3, view.DataGridView1.SelectedCells(0).RowIndex).Value
                        view.associated_entities(view.tassociations.SelectedIndex).attribute_values(view.DataGridView1.SelectedCells(0).RowIndex).publish_draft_value = True
                        view.DataGridView1.Item(4, view.DataGridView1.SelectedCells(0).RowIndex).Value = view.tabimages.Images(att_edited_icon)
                        view.associated_entities(view.tassociations.SelectedIndex).attribute_values(view.DataGridView1.SelectedCells(0).RowIndex).value_changed = True
                        Me.entity_edit_mode()
                    Else
                        view.associated_entities(view.tassociations.SelectedIndex).attribute_values(view.DataGridView1.SelectedCells(0).RowIndex).publish_draft_value = False
                        view.DataGridView1.Item(5, view.DataGridView1.SelectedCells(0).RowIndex).Value = view.associated_entities(view.tassociations.SelectedIndex).attribute_values(view.DataGridView1.SelectedCells(0).RowIndex).original_published_value
                        view.DataGridView1.Item(4, view.DataGridView1.SelectedCells(0).RowIndex).Value = view.tabimages.Images(push_publish_icon)
                    End If
                    REM mae sure key pair selected
                    If container.oClasses.attribute_pool(i).lookup_keys.count > 0 Then
                        For j = 0 To container.oClasses.attribute_pool(i).lookup_values.count - 1
                            If RTrim(LTrim(container.oClasses.attribute_pool(i).lookup_values(j))) = RTrim(LTrim(view.DataGridView1.Item(3, view.DataGridView1.SelectedCells(0).RowIndex).Value)) Then
                                view.associated_entities(view.tassociations.SelectedIndex).attribute_values(view.DataGridView1.SelectedCells(0).RowIndex).value = container.oClasses.attribute_pool(i).lookup_keys(j)
                            End If
                        Next

                    End If
                End If


                view.DataGridView1.Item(1, view.DataGridView1.SelectedCells(0).RowIndex).Selected = True
                Exit For
            End If
        Next

    End Sub
    Public Sub delete_associated_entity(ByVal name As String, ByVal class_name As String)
        REM deletes an entity is it isnt as yet commited
        For i = 1 To view.associated_entities.Count - 1
            If view.associated_entities(i).name = name And view.associated_entities(i).class_name = class_name Then
                view.associated_entities(i).write_mode = bc_om_entity.DELETE

                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    view.associated_entities(i).db_write()
                Else
                    view.associated_entities(i).tmode = bc_cs_soap_base_class.tWRITE
                    If view.associated_entities(i).transmit_to_server_and_receive(view.associated_entities(i), True) = False Then
                        Exit Sub
                    End If
                End If
                For j = 0 To container.oEntities.entity.Count - 1
                    If container.oEntities.entity(j).id = view.associated_entities(i).id Then
                        container.oEntities.entity.RemoveAt(j)
                        Exit For
                    End If
                Next
                view.associated_entities.RemoveAt(i)
                view.tassociations.TabPages.RemoveAt(i)
                Exit For
            End If

        Next
    End Sub
    Public Sub check_read_only_entity()
        If Me.container.mode = bc_am_cp_container.VIEW Or InStr(view.Lentities.SelectedItems(0).Text, "(inactive)") > 0 Then
            view.tentities.ContextMenuStrip = Nothing
            view.DataGridView1.Columns(0).Visible = False
            view.DataGridView1.Columns(2).Visible = False
            view.DataGridView1.Columns(4).Visible = False
            view.DataGridView1.Columns(1).ReadOnly = True
            view.DataGridView1.Columns(3).ReadOnly = True
            view.DataGridView2.Columns(0).Visible = False
            view.DataGridView2.Columns(2).Visible = False
            view.DataGridView2.Columns(4).Visible = False
            view.DataGridView2.Columns(1).ReadOnly = True
            view.DataGridView2.Columns(3).ReadOnly = True


        Else
            view.passociations.Enabled = True
            view.tentities.ContextMenuStrip = view.assignentity
            view.DataGridView1.Columns(0).Visible = True
            view.DataGridView1.Columns(2).Visible = True
            view.DataGridView1.Columns(1).ReadOnly = True
            view.DataGridView1.Columns(3).ReadOnly = False
            view.DataGridView1.Columns(4).Visible = True
            view.DataGridView2.Columns(0).Visible = True
            view.DataGridView2.Columns(2).Visible = True
            view.DataGridView2.Columns(1).ReadOnly = True
            view.DataGridView2.Columns(3).ReadOnly = False
            view.DataGridView2.Columns(4).Visible = True
        End If
        data_grid_size()

    End Sub
    Public Sub check_read_only_entity_real_time(entity As bc_om_entity)
        If Me.container.mode = bc_am_cp_container.VIEW Or InStr(entity.name, "(inactive)") > 0 Then
            view.tentities.ContextMenuStrip = Nothing
            view.DataGridView1.Columns(0).Visible = False
            view.DataGridView1.Columns(2).Visible = False
            view.DataGridView1.Columns(4).Visible = False
            view.DataGridView1.Columns(1).ReadOnly = True
            view.DataGridView1.Columns(3).ReadOnly = True
            view.DataGridView2.Columns(0).Visible = False
            view.DataGridView2.Columns(2).Visible = False
            view.DataGridView2.Columns(4).Visible = False
            view.DataGridView2.Columns(1).ReadOnly = True
            view.DataGridView2.Columns(3).ReadOnly = True


        Else
            view.passociations.Enabled = True
            view.tentities.ContextMenuStrip = view.assignentity
            view.DataGridView1.Columns(0).Visible = True
            view.DataGridView1.Columns(2).Visible = True
            view.DataGridView1.Columns(1).ReadOnly = True
            view.DataGridView1.Columns(3).ReadOnly = False
            view.DataGridView1.Columns(4).Visible = True
            view.DataGridView2.Columns(0).Visible = True
            view.DataGridView2.Columns(2).Visible = True
            view.DataGridView2.Columns(1).ReadOnly = True
            view.DataGridView2.Columns(3).ReadOnly = False
            view.DataGridView2.Columns(4).Visible = True
        End If
        data_grid_size()

    End Sub
    Public Sub remove_association()
        view.Cursor = Cursors.WaitCursor


        remove_link(view.tentities.SelectedNode.Parent.Text, view.tentities.SelectedNode.Text)
        load_entity_view(True)
        view.Cursor = Cursors.Default

    End Sub
    Public Sub attribute_details(all As Boolean)
        Try

            If view.DataGridView1.SelectedCells.Count = 0 Then
                Exit Sub
            End If
            REM XXXXX
            Dim fa As New bc_am_attribute_audit
            Dim ca As New Cbc_am_attribute_audit
            Dim aname, kname, aid As String
            Dim kid As Long
            aname = view.DataGridView1.Item(1, view.DataGridView1.SelectedCells(0).RowIndex).Value
            kid = view.associated_entities(view.tassociations.SelectedIndex).id
            kname = view.Lentities.SelectedItems(0).Text
            aid = view.associated_entities(view.tassociations.SelectedIndex).attribute_values(view.DataGridView1.SelectedCells(0).RowIndex).attribute_id

            If ca.load_data(fa, kname, aname, aid, kid, bc_om_attribute_audit.EATTRIBUTE_TYPE.ENTITY, True, all) = True Then
                fa.ShowDialog()
            Else
                view.mdettitle.Text = "Details for: " + view.DataGridView1.Item(1, view.DataGridView1.SelectedCells(0).RowIndex).Value

                If view.tattributes.SelectedIndex = 0 Then

                    If view.associated_entities(view.tassociations.SelectedIndex).attribute_values(view.DataGridView1.SelectedCells(0).RowIndex).show_workflow = True Then
                        view.mdetupdate.Text = "Draft Last Updated: " + view.associated_entities(view.tassociations.SelectedIndex).attribute_values(view.DataGridView1.SelectedCells(0).RowIndex).last_update_comment
                    Else
                        view.mdetupdate.Text = "Last Updated: " + view.associated_entities(view.tassociations.SelectedIndex).attribute_values(view.DataGridView1.SelectedCells(0).RowIndex).last_update_comment

                    End If
                    view.mdetuser.Text = "By: " + view.associated_entities(view.tassociations.SelectedIndex).attribute_values(view.DataGridView1.SelectedCells(0).RowIndex).last_update_user
                    view.mdetpubupdate.Visible = False
                    view.mdetpubuser.Visible = False
                    If view.associated_entities(view.tassociations.SelectedIndex).attribute_values(view.DataGridView1.SelectedCells(0).RowIndex).show_workflow = True Then
                        view.mdetpubupdate.Visible = True
                        view.mdetpubuser.Visible = True
                        view.mdetpubupdate.Text = "Published Last Updated: " + view.associated_entities(view.tassociations.SelectedIndex).attribute_values(view.DataGridView1.SelectedCells(0).RowIndex).last_update_published_comment
                        view.mdetpubuser.Text = "Published By: " + view.associated_entities(view.tassociations.SelectedIndex).attribute_values(view.DataGridView1.SelectedCells(0).RowIndex).last_update_published_user
                    End If
                    'Else
                    '    Dim entity_id As Long
                    '    'entity_id = Me.get_entity_id(controller.get_real_class_name(Me.Tabparent.SelectedTab.Text), Me.lselparent.Text)

                    '    If is_parent_class() = True Then
                    '        For i = 0 To view.associated_entities(view.tassociations.SelectedIndex).parent_entities.count - 1
                    '            If view.associated_entities(view.tassociations.SelectedIndex).parent_entities(i).id = entity_id Then
                    '                If view.associated_entities(view.tassociations.SelectedIndex).parent_entities(i).linked_attribute_values(view.DataGridView1.SelectedCells(0).RowIndex).show_workflow = True Then
                    '                    Me.mdetupdate.Text = "Draft Last Updated: " + view.associated_entities(view.tassociations.SelectedIndex).parent_entities(i).linked_attribute_values(view.DataGridView1.SelectedCells(0).RowIndex).last_update_comment
                    '                Else
                    '                    Me.mdetupdate.Text = "Last Updated: " + view.associated_entities(view.tassociations.SelectedIndex).parent_entities(i).linked_attribute_values(view.DataGridView1.SelectedCells(0).RowIndex).last_update_comment

                    '                End If
                    '                Me.mdetuser.Text = "By: " + view.associated_entities(view.tassociations.SelectedIndex).parent_entities(i).linked_attribute_values(view.DataGridView1.SelectedCells(0).RowIndex).last_update_user
                    '                Me.mdetpubupdate.Visible = False
                    '                Me.mdetpubuser.Visible = False

                    '                If view.associated_entities(view.tassociations.SelectedIndex).parent_entities(i).linked_attribute_values(view.DataGridView1.SelectedCells(0).RowIndex).show_workflow = True Then
                    '                    Me.mdetpubupdate.Visible = True
                    '                    Me.mdetpubuser.Visible = True
                    '                    Me.mdetpubupdate.Text = "Published Last Updated: " + view.associated_entities(view.tassociations.SelectedIndex).parent_entities(i).linked_attribute_values(view.DataGridView1.SelectedCells(0).RowIndex).last_update_published_comment
                    '                    Me.mdetpubuser.Text = "Published By: " + view.associated_entities(view.tassociations.SelectedIndex).parent_entities(i).linked_attribute_values(view.DataGridView1.SelectedCells(0).RowIndex).last_update_published_user
                    '                End If
                    '                Exit For
                    '            End If
                    '        Next
                    '    Else
                    '        For i = 0 To view.associated_entities(view.tassociations.SelectedIndex).child_entities.count - 1
                    '            If view.associated_entities(view.tassociations.SelectedIndex).child_entities(i).id = entity_id Then
                    '                If view.associated_entities(view.tassociations.SelectedIndex).child_entities(i).linked_attribute_values(view.DataGridView1.SelectedCells(0).RowIndex).show_workflow = True Then
                    '                    Me.mdetupdate.Text = "Draft Last Updated: " + view.associated_entities(view.tassociations.SelectedIndex).child_entities(i).linked_attribute_values(view.DataGridView1.SelectedCells(0).RowIndex).last_update_comment
                    '                Else
                    '                    Me.mdetupdate.Text = "Last Updated: " + view.associated_entities(view.tassociations.SelectedIndex).child_entities(i).linked_attribute_values(view.DataGridView1.SelectedCells(0).RowIndex).last_update_comment

                    '                End If
                    '                Me.mdetuser.Text = "By: " + view.associated_entities(view.tassociations.SelectedIndex).child_entities(i).linked_attribute_values(view.DataGridView1.SelectedCells(0).RowIndex).last_update_user
                    '                Me.mdetpubupdate.Visible = False
                    '                Me.mdetpubuser.Visible = False

                    '                If view.associated_entities(view.tassociations.SelectedIndex).child_entities(i).linked_attribute_values(view.DataGridView1.SelectedCells(0).RowIndex).show_workflow = True Then
                    '                    Me.mdetpubupdate.Visible = True
                    '                    Me.mdetpubuser.Visible = True
                    '                    Me.mdetpubupdate.Text = "Published Last Updated: " + view.associated_entities(view.tassociations.SelectedIndex).child_entities(i).linked_attribute_values(view.DataGridView1.SelectedCells(0).RowIndex).last_update_published_comment
                    '                    Me.mdetpubuser.Text = "Published By: " + view.associated_entities(view.tassociations.SelectedIndex).child_entities(i).linked_attribute_values(view.DataGridView1.SelectedCells(0).RowIndex).last_update_published_user
                    '                End If
                    '                Exit For
                    '            End If
                    '        Next
                    '    End If
                End If
                view.attributedetails.Show(Cursor.Position.X, Cursor.Position.Y)
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("DataGridView1", "DoubleClick", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            view.Cursor = Cursors.Default
        End Try
    End Sub
    Public Sub navigate()

        Dim t As String
        Dim en As String
        Dim i As Integer
        Try
            If view.bentchanges.Enabled = False Then
                t = view.tentities.SelectedNode.Parent.Text
                en = view.tentities.SelectedNode.Text
                view.cclass.Text = get_real_class_name(t)
                Me.set_selected_entity(en)

            Else
                For i = 0 To view.tassociations.TabPages.Count - 1
                    If view.tassociations.TabPages(i).Text = view.tentities.SelectedNode.Text Then
                        view.tassociations.SelectedIndex = i
                        Exit For
                    End If

                Next
            End If
        Catch ex As Exception
            'load_entity_assign(view.tentities.SelectedNode.Text)
            show_linked_audit(view.tentities.SelectedNode.Text)

        End Try
    End Sub
    Dim sel_aclass_id As Long
    Dim sel_aclass_parent As Boolean
    Public Function load_linked_attributes() As Boolean

        load_linked_attributes = False
        Dim class_id As Long = 0
        Dim aclass_id As Long = 0
        Dim aentity_Id As Long = 0
        Dim schema_Id As Long
        sel_aclass_id = 0
        Try

            linked_attributes.Clear()
            For i = 0 To container.oClasses.schemas.Count - 1
                If container.oClasses.schemas(i).schema_name = view.cschemael.Text Then
                    schema_Id = container.oClasses.schemas(i).schema_id
                    Exit For
                End If
            Next

            Try
                aclass_id = container.oClasses.classes(sel_cls_idx(get_real_class_name(view.tentities.SelectedNode.Parent.Text))).class_id


                class_id = container.oClasses.classes(sel_cls_idx(get_real_class_name(view.cclass.Text))).class_id
                For i = 0 To Me.container.oEntities.entity.Count - 1
                    If container.oEntities.entity(i).class_id = aclass_id Then

                        aclass_id = container.oEntities.entity(i).class_id
                        If container.oEntities.entity(i).name = view.tentities.SelectedNode.Text Then
                            aentity_Id = container.oEntities.entity(i).id
                            For j = 0 To container.oClasses.classes(sel_cls_idx(get_real_class_name(view.cclass.Text))).parent_links.count - 1
                                If container.oClasses.classes(sel_cls_idx(get_real_class_name(view.cclass.Text))).parent_links(j).schema_Id = schema_Id And container.oClasses.classes(sel_cls_idx(get_real_class_name(view.cclass.Text))).parent_links(j).parent_class_id = aclass_id Then
                                    If container.oClasses.classes(sel_cls_idx(get_real_class_name(view.cclass.Text))).parent_links(j).linked_attributes.Count > 0 Then
                                        For k = 0 To container.oClasses.classes(sel_cls_idx(get_real_class_name(view.cclass.Text))).parent_links(j).linked_attributes.Count - 1
                                            linked_attributes.Add(container.oClasses.classes(sel_cls_idx(get_real_class_name(view.cclass.Text))).parent_links(j).linked_attributes(k))
                                        Next
                                        sel_aclass_parent = True
                                        sel_aclass_id = aclass_id
                                        load_linked_attributes = True

                                        Exit For
                                    End If
                                End If
                            Next
                            For j = 0 To container.oClasses.classes(sel_cls_idx(get_real_class_name(view.cclass.Text))).child_links.count - 1
                                If container.oClasses.classes(sel_cls_idx(get_real_class_name(view.cclass.Text))).child_links(j).schema_Id = schema_Id And container.oClasses.classes(sel_cls_idx(get_real_class_name(view.cclass.Text))).child_links(j).child_class_id = aclass_id Then
                                    If container.oClasses.classes(sel_cls_idx(get_real_class_name(view.cclass.Text))).child_links(j).linked_attributes.Count > 0 Then
                                        For k = 0 To container.oClasses.classes(sel_cls_idx(get_real_class_name(view.cclass.Text))).child_links(j).linked_attributes.Count - 1
                                            linked_attributes.Add(container.oClasses.classes(sel_cls_idx(get_real_class_name(view.cclass.Text))).child_links(j).linked_attributes(k))
                                        Next
                                        sel_aclass_parent = False
                                        sel_aclass_id = aclass_id
                                        load_linked_attributes = True
                                        Exit For
                                    End If
                                End If
                            Next


                            Exit For
                        End If
                    End If
                Next


            Catch ex As Exception
                ' MsgBox(ex.Message)

            End Try

        Catch ex As Exception
            Dim err As New bc_cs_error_log("Blue Curve", "load_linked_attributes", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Function
    Dim linked_attributes As New List(Of bc_om_class_attribute)
    Dim idx As Integer
    Public Sub load_associated_class_linked_attributes(name As String)
        Try

            If sel_aclass_id = 0 Then
                Exit Sub
            End If

            REM attributes of class
            view.DataGridView2.Rows.Clear()
            Dim oent As New bc_om_entity
            'Dim idx As Integer
            idx = -1
            If sel_aclass_parent = True Then
                For i = 0 To view.associated_entities(view.tassociations.SelectedIndex).parent_entities.count - 1
                    If view.associated_entities(view.tassociations.SelectedIndex).parent_entities(i).schema_Id = schema_id And
                        view.associated_entities(view.tassociations.SelectedIndex).parent_entities(i).name = name Then
                        idx = i
                        Exit For
                    End If
                Next
            Else

                For i = 0 To view.associated_entities(view.tassociations.SelectedIndex).child_entities.count - 1
                    If view.associated_entities(view.tassociations.SelectedIndex).child_entities(i).schema_Id = schema_id And
                        view.associated_entities(view.tassociations.SelectedIndex).child_entities(i).name = name Then
                        idx = i
                        Exit For
                    End If
                Next
            End If

            If idx = -1 Then
                Exit Sub
            End If
            Dim class_id As Long
            Dim parent_class_id As String
            If sel_aclass_parent = True Then
                For j = 0 To view.associated_entities(view.tassociations.SelectedIndex).parent_entities(idx).linked_Attribute_values.count - 1
                    With view.associated_entities(view.tassociations.SelectedIndex).parent_entities(idx).linked_Attribute_values(j)
                        For i = 0 To Me.container.oClasses.attribute_pool.Count - 1
                            If Me.container.oClasses.attribute_pool(i).attribute_id = .attribute_id Then


                                view.DataGridView2.Rows.Add()
                                view.DataGridView2.Item(1, view.DataGridView2.Rows.Count - 1).Value = Me.container.oClasses.attribute_pool(i).name


                                REM KVP
                                REM see if item is a key value pair
                                If container.oClasses.attribute_pool(i).lookup_keys.count > 0 Then
                                    REM find key value for value
                                    For k = 0 To container.oClasses.attribute_pool(i).lookup_values.count - 1
                                        If RTrim(LTrim(container.oClasses.attribute_pool(i).lookup_keys(k))) = .value Then
                                            view.DataGridView2.Item(3, j).Value = container.oClasses.attribute_pool(i).lookup_values(k)
                                        End If
                                    Next
                                Else
                                    view.DataGridView2.Item(3, j).Value = .value
                                End If


                                REM

                                If Me.container.oClasses.attribute_pool(i).nullable = True Then
                                    view.DataGridView2.Item(0, j).Value = New Drawing.Bitmap(1, 1)
                                Else
                                    view.DataGridView2.Item(0, j).Value = view.tabimages.Images(mandatory_icon)
                                End If
                                If .show_workflow = False Then
                                    view.DataGridView2.Item(4, j).Value = New Drawing.Bitmap(1, 1)
                                Else
                                    view.DataGridView2.Item(4, j).Value = view.tabimages.Images(push_publish_icon)
                                End If
                                REM set value
                                Select Case Me.container.oClasses.attribute_pool(i).type_id
                                    Case 1
                                        view.DataGridView2.Item(2, j).Value = view.tabimages.Images(string_icon)
                                    Case 2
                                        view.DataGridView2.Item(2, j).Value = view.tabimages.Images(number_icon)
                                    Case 3
                                        view.DataGridView2.Item(2, j).Value = view.tabimages.Images(boolean_icon)
                                    Case 5
                                        view.DataGridView2.Item(2, j).Value = view.tabimages.Images(date_icon)

                                End Select
                                If .value_changed = True Then
                                    view.DataGridView2.Item(0, j).Value = view.tabimages.Images(att_edited_icon)
                                End If
                                If .publish_draft_value = True Then
                                    view.DataGridView2.Item(5, j).Value = .value
                                    view.DataGridView2.Item(4, j).Value = view.tabimages.Images(att_edited_icon)
                                End If

                                REM read only


                                class_id = view.associated_entities(view.tassociations.SelectedIndex).class_id()
                                For m = 0 To view.associated_entities(view.tassociations.SelectedIndex).parent_entities.count - 1
                                    If name = view.associated_entities(view.tassociations.SelectedIndex).parent_entities(m).name Then
                                        parent_class_id = view.associated_entities(view.tassociations.SelectedIndex).parent_entities(m).class_id
                                        For l = 0 To Me.container.oClasses.classes.Count - 1
                                            If Me.container.oClasses.classes(l).class_id = class_id Then
                                                For ll = 0 To Me.container.oClasses.classes(l).parent_links.count - 1
                                                    If Me.container.oClasses.classes(l).parent_links(ll).parent_class_id = parent_class_id Then
                                                        For lll = 0 To Me.container.oClasses.classes(l).parent_links(ll).linked_attributes.count - 1
                                                            If Me.container.oClasses.classes(l).parent_links(ll).linked_attributes(lll).attribute_id = .attribute_id Then
                                                                If Me.container.oClasses.classes(l).parent_links(ll).linked_attributes(lll).permission = 1 Then
                                                                    view.DataGridView2.Item(2, j).Value = view.tabimages.Images(read_only_icon)
                                                                End If
                                                                Exit For
                                                            End If
                                                        Next
                                                        Exit For
                                                    End If
                                                Next
                                                Exit For
                                            End If
                                        Next
                                        Exit For
                                    End If
                                Next
                                Exit For
                            End If
                        Next
                    End With
                Next
                If view.associated_entities(view.tassociations.SelectedIndex).parent_entities(idx).linked_Attribute_values.count > 0 Then
                    view.DataGridView2.Item(3, 0).Selected = True
                End If

            Else

                For j = 0 To view.associated_entities(view.tassociations.SelectedIndex).child_entities(idx).linked_Attribute_values.count - 1
                    With view.associated_entities(view.tassociations.SelectedIndex).child_entities(idx).linked_Attribute_values(j)
                        For i = 0 To Me.container.oClasses.attribute_pool.Count - 1
                            If Me.container.oClasses.attribute_pool(i).attribute_id = .attribute_id Then
                                view.DataGridView2.Rows.Add()
                                view.DataGridView2.Item(1, view.DataGridView2.Rows.Count - 1).Value = Me.container.oClasses.attribute_pool(i).name
                                view.DataGridView2.Item(3, j).Value = .value

                                If container.oClasses.attribute_pool(i).lookup_keys.count > 0 Then
                                    REM find key value for value
                                    For k = 0 To container.oClasses.attribute_pool(i).lookup_values.count - 1
                                        If RTrim(LTrim(container.oClasses.attribute_pool(i).lookup_keys(k))) = .value Then
                                            view.DataGridView2.Item(3, j).Value = container.oClasses.attribute_pool(i).lookup_values(k)
                                        End If
                                    Next
                                Else
                                    view.DataGridView2.Item(3, j).Value = .value
                                End If

                                If Me.container.oClasses.attribute_pool(i).nullable = True Then
                                    view.DataGridView2.Item(0, j).Value = New Drawing.Bitmap(1, 1)
                                Else
                                    view.DataGridView2.Item(0, j).Value = view.tabimages.Images(mandatory_icon)
                                End If
                                If .show_workflow = False Then
                                    view.DataGridView2.Item(4, j).Value = New Drawing.Bitmap(1, 1)
                                Else
                                    view.DataGridView2.Item(4, j).Value = view.tabimages.Images(push_publish_icon)
                                End If
                                REM set value
                                Select Case Me.container.oClasses.attribute_pool(i).type_id
                                    Case 1
                                        view.DataGridView2.Item(2, j).Value = view.tabimages.Images(string_icon)
                                    Case 2
                                        view.DataGridView2.Item(2, j).Value = view.tabimages.Images(number_icon)
                                    Case 3
                                        view.DataGridView2.Item(2, j).Value = view.tabimages.Images(boolean_icon)
                                    Case 5
                                        view.DataGridView2.Item(2, j).Value = view.tabimages.Images(date_icon)

                                End Select
                                If .value_changed = True Then
                                    view.DataGridView2.Item(0, j).Value = view.tabimages.Images(att_edited_icon)
                                End If
                                If .publish_draft_value = True Then
                                    view.DataGridView2.Item(5, j).Value = .value
                                    view.DataGridView2.Item(4, j).Value = view.tabimages.Images(att_edited_icon)
                                End If

                                class_id = view.associated_entities(view.tassociations.SelectedIndex).class_id()
                                Dim child_class_id As Long
                                For m = 0 To view.associated_entities(view.tassociations.SelectedIndex).child_entities.count - 1
                                    If name = view.associated_entities(view.tassociations.SelectedIndex).child_entities(m).name Then
                                        child_class_id = view.associated_entities(view.tassociations.SelectedIndex).child_entities(m).class_id
                                        For l = 0 To Me.container.oClasses.classes.Count - 1
                                            If Me.container.oClasses.classes(l).class_id = class_id Then
                                                For ll = 0 To Me.container.oClasses.classes(l).child_links.count - 1
                                                    If Me.container.oClasses.classes(l).child_links(ll).child_class_id = child_class_id Then
                                                        For lll = 0 To Me.container.oClasses.classes(l).child_links(ll).linked_attributes.count - 1
                                                            If Me.container.oClasses.classes(l).child_links(ll).linked_attributes(lll).attribute_id = .attribute_id Then
                                                                If Me.container.oClasses.classes(l).child_links(ll).linked_attributes(lll).permission = 1 Then
                                                                    view.DataGridView2.Item(2, j).Value = view.tabimages.Images(read_only_icon)
                                                                End If
                                                                Exit For
                                                            End If
                                                        Next
                                                        Exit For
                                                    End If
                                                Next
                                                Exit For
                                            End If
                                        Next
                                        Exit For
                                    End If
                                Next

                                Exit For
                            End If
                        Next
                    End With
                Next
                If view.associated_entities(view.tassociations.SelectedIndex).child_entities(idx).linked_Attribute_values.count > 0 And view.DataGridView2.Columns(3).Visible = True Then
                    view.DataGridView2.Item(3, 0).Selected = True
                End If

            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_cp_main", "load_associated_class_linked_attributes", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

        End Try

    End Sub
    Public Function set_data_type_Control_linked() As Boolean
        Try
            Dim length As Integer

            If setting_text_box = True Then
                Exit Function

            End If
            Dim lus As New ArrayList
            set_data_type_Control_linked = False
            If no_Action = True Then
                Exit Function
            End If


            If view.DataGridView2.ColumnCount <> 6 Then
                Exit Function
            End If

            REM read only attribute



            REM temporary hold current values
            hold_vals.Clear()
            For i = 0 To view.DataGridView2.Rows.Count - 1
                hold_vals.Add(view.DataGridView2.Item(3, i).Value)
                If IsNothing(hold_vals(i)) Then
                    hold_vals(i) = ""
                End If
            Next
            Dim lu As DataGridViewComboBoxCell
            Dim use_combo As Boolean = False
            Dim dtextbox As New DataGridViewTextBoxColumn
            dtextbox.SortMode = DataGridViewColumnSortMode.NotSortable
            REM HHHH


            Dim Dcombobox As New DataGridViewComboBoxColumn
            Dim row_id As Integer
            row_id = view.DataGridView2.SelectedCells(0).RowIndex




            Dim j As Integer
            REM 1 string 2 number 3 boolean 5 date
            view.DataGridView2.Item(1, row_id).Selected = True

            REM build up array of all lookup values
            For i = 0 To view.DataGridView2.Rows.Count - 1
                For j = 0 To container.oClasses.attribute_pool.Count - 1
                    If container.oClasses.attribute_pool(j).name = view.DataGridView2.Item(1, i).Value Then
                        If container.oClasses.attribute_pool(j).is_lookup = True Then
                            lu = New DataGridViewComboBoxCell
                            lu.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing

                            For k = 0 To container.oClasses.attribute_pool(j).lookup_values.count - 1
                                lu.Items.Add(container.oClasses.attribute_pool(j).lookup_values(k))
                            Next
                            lu.Items.Add("")
                            lus.Add(lu)

                        ElseIf container.oClasses.attribute_pool(j).type_id = 3 Then
                            lu = New DataGridViewComboBoxCell
                            lu.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing

                            lu.Items.Add("")
                            lu.Items.Add("True")
                            lu.Items.Add("False")
                            If hold_vals(i) <> "True" And hold_vals(i) <> "False" Then
                                hold_vals(i) = ""
                            End If
                            lus.Add(lu)
                        Else
                            lu = New DataGridViewComboBoxCell
                            lu.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing

                            If IsNothing(hold_vals(i)) Then
                                lu.Items.Add("")
                            Else
                                lu.Items.Add(hold_vals(i))
                            End If
                            lus.Add(lu)

                        End If
                        Exit For
                    End If
                Next
            Next
            Dim beditbox As Boolean = False
            For i = 0 To container.oClasses.attribute_pool.Count - 1
                If container.oClasses.attribute_pool(i).name = view.DataGridView2.Item(1, view.DataGridView2.SelectedCells(0).RowIndex).Value Then
                   
                    length = container.oClasses.attribute_pool(i).length

                    If container.oClasses.attribute_pool(i).is_lookup = True Then
                        use_combo = True
                    ElseIf setting_text_box = False And container.oClasses.attribute_pool(i).type_id = 1 And container.oClasses.attribute_pool(i).repeats = 1 Then
                        Dim fedit As New bc_am_text_edit
                        beditbox = True
                        fedit.Text = "Edit: " + container.oClasses.attribute_pool(i).name
                        fedit.maxlength = container.oClasses.attribute_pool(i).length
                        fedit.ttextentry.MaxLength = container.oClasses.attribute_pool(i).length
                        fedit.ttextentry.Text = view.DataGridView2.Item(3, view.DataGridView1.SelectedCells(0).RowIndex).Value
                        fedit.Llength.Text = "Maximum length: " + CStr(container.oClasses.attribute_pool(i).length)
                        setting_text_box = True
                        'fedit.Initializing = False
                        fedit.ShowDialog()
                        If fedit.ok_selected = True Then
                            setting_text_box = True
                            validate_attribute_value(fedit.ttextentry.Text, True)
                        End If
                        view.DataGridView2.Item(0, row_id).Selected = True
                        setting_text_box = False
                  
                    ElseIf container.oClasses.attribute_pool(i).type_id = 3 Then
                        use_combo = True
                    End If
                    Exit For
                End If
            Next
            REM 
            Dcombobox.Width = view.DataGridView2.Columns(3).Width

            dtextbox.Width = view.DataGridView2.Columns(3).Width

            If beditbox = False Then
                view.DataGridView2.Columns.RemoveAt(3)
                If use_combo = True Then
                    Dcombobox.Name = "Values"
                    view.DataGridView2.Columns.Insert(3, Dcombobox)
                Else
                    dtextbox.Name = "Values"
                    dtextbox.MaxInputLength = length
                    view.DataGridView2.Columns.Insert(3, dtextbox)
                End If
                REM reset value
                no_Action = True
                For i = 0 To view.DataGridView2.Rows.Count - 1
                    If use_combo = True Then
                        view.DataGridView2.Item(3, i) = lus(i)
                    End If
                    If hold_vals(i) <> "" Then
                        view.DataGridView2.Item(3, i).Value = hold_vals(i)
                    End If
                Next
                view.DataGridView2.Item(3, row_id).Selected = True
            End If

            Dim name As String
            name = view.tattributes.SelectedTab.Text
            name = Replace(name, "Linked to ", "")
            Dim class_id As Long
            class_id = view.associated_entities(view.tassociations.SelectedIndex).class_id()
            Dim child_class_id As Long
            For m = 0 To view.associated_entities(view.tassociations.SelectedIndex).child_entities.count - 1
                If name = view.associated_entities(view.tassociations.SelectedIndex).child_entities(m).name Then
                    child_class_id = view.associated_entities(view.tassociations.SelectedIndex).child_entities(m).class_id
                    For l = 0 To Me.container.oClasses.classes.Count - 1
                        If Me.container.oClasses.classes(l).class_id = class_id Then
                            For ll = 0 To Me.container.oClasses.classes(l).child_links.count - 1
                                If Me.container.oClasses.classes(l).child_links(ll).child_class_id = child_class_id Then
                                    If Me.container.oClasses.classes(l).child_links(ll).linked_attributes(row_id).permission = 1 Then
                                        view.DataGridView2.Columns(3).ReadOnly = True
                                        Exit For
                                    End If
                                    Exit For
                                End If
                            Next
                            Exit For
                        End If
                    Next
                    Exit For
                End If
            Next
            If view.DataGridView2.Columns(3).ReadOnly = False Then
                Dim parent_class_id As Long
                For m = 0 To view.associated_entities(view.tassociations.SelectedIndex).parent_entities.count - 1
                    If name = view.associated_entities(view.tassociations.SelectedIndex).parent_entities(m).name Then
                        parent_class_id = view.associated_entities(view.tassociations.SelectedIndex).parent_entities(m).class_id
                        For l = 0 To Me.container.oClasses.classes.Count - 1
                            If Me.container.oClasses.classes(l).class_id = class_id Then
                                For ll = 0 To Me.container.oClasses.classes(l).parent_links.count - 1
                                    If Me.container.oClasses.classes(l).parent_links(ll).parent_class_id = parent_class_id Then
                                        If Me.container.oClasses.classes(l).parent_links(ll).linked_attributes(row_id).permission = 1 Then
                                            view.DataGridView2.Columns(3).ReadOnly = True
                                            Exit For
                                        End If
                                        Exit For
                                    End If
                                Next
                                Exit For
                            End If
                        Next
                        Exit For
                    End If
                Next
            End If


            no_Action = False
            set_data_type_Control_linked = True
        Catch ex As Exception
            Dim ocomm As New bc_cs_activity_log("bc_am_taxonomy", "set_data_type_comtrol", bc_cs_activity_codes.COMMENTARY, "Error: " + ex.Message)

        End Try

    End Function

    Public Function validate_linked_attribute_value(ByVal val As String, Optional ByVal tb As Boolean = False) As Boolean
        no_Action = True
        Try
            validate_linked_attribute_value = True
            'If Trim(val) = "" Then
            '    Exit Function
            'End If
            Try
                If view.DataGridView2.SelectedCells(0).ColumnIndex <> 3 And tb = False Then
                    Exit Function
                End If
            Catch
                Exit Function
            End Try

            set_val_linked(val, tb)

            'Dim failed As Boolean = False
            'For i = 0 To container.oClasses.attribute_pool.Count - 1
            '    If container.oClasses.attribute_pool(i).name = view.DataGridView1.Item(1, view.DataGridView1.SelectedCells(0).RowIndex).Value Then
            '        Select Case container.oClasses.attribute_pool(i).type_id And container.oClasses.attribute_pool(i).is_lookup = False
            '            Case 2
            '                If IsNumeric(val) = False Then
            '                    view.DataGridView1.Item(2, view.DataGridView1.SelectedCells(0).RowIndex).Value = "Item must be numeric"
            '                    view.DataGridView1.Item(2, view.DataGridView1.SelectedCells(0).RowIndex).Selected = True
            '                    'Dim omsg As New bc_cs_message("Blue Curve", "Item must be numeric", bc_cs_message.MESSAGE,false,false,"Yes","No",true)
            '                    failed = True
            '                End If
            '            Case 5
            '                If IsDate(val) = False Then
            '                    Dim omsg As New bc_cs_message("Blue Curve", "Item must be Date and Time", bc_cs_message.MESSAGE,false,false,"Yes","No",true)
            '                    failed = True
            '                End If
            '        End Select
            '        If failed = True Then
            '            validate_attribute_value = True
            '        Else
            '            set_val(val)
            '        End If
            '        Exit Function
            '    End If
            'Next
        Catch ex As Exception
            'MsgBox(ex.Message)
        Finally
            no_Action = False
        End Try
    End Function
    Public Sub set_val_linked(ByVal val As String, Optional ByVal ext As Boolean = False)
        Try
            If sel_aclass_parent = True Then
                If val <> view.associated_entities(view.tassociations.SelectedIndex).parent_entities(idx).linked_Attribute_values(view.DataGridView2.SelectedCells(0).RowIndex).value Then
                    REM see if item is a key value pair
                    For i = 0 To container.oClasses.attribute_pool.Count - 1
                        If container.oClasses.attribute_pool(i).name = view.DataGridView2.Item(1, view.DataGridView2.SelectedCells(0).RowIndex).Value Then

                            REM mae sure key pair selected
                            If container.oClasses.attribute_pool(i).lookup_keys.count > 0 Then
                                REM find key value for value
                                For j = 0 To container.oClasses.attribute_pool(i).lookup_values.count - 1
                                    If RTrim(LTrim(container.oClasses.attribute_pool(i).lookup_values(j))) = RTrim(LTrim(val)) Then
                                        val = container.oClasses.attribute_pool(i).lookup_keys(j)
                                        Exit For
                                    End If
                                Next
                            End If
                            Exit For
                        End If

                    Next
                    view.associated_entities(view.tassociations.SelectedIndex).parent_entities(idx).linked_Attribute_values(view.DataGridView2.SelectedCells(0).RowIndex).value = val
                    view.associated_entities(view.tassociations.SelectedIndex).parent_entities(idx).linked_Attribute_values(view.DataGridView2.SelectedCells(0).RowIndex).value_changed = True
                    view.DataGridView2.Item(0, view.DataGridView2.SelectedCells(0).RowIndex).Value = view.tabimages.Images(att_edited_icon)
                    If ext = True Then
                        view.DataGridView2.Item(3, view.DataGridView1.SelectedCells(0).RowIndex).Value = val
                    End If

                    REM if item workflowed and publish requested reset this
                    For i = 0 To container.oClasses.attribute_pool.Count - 1
                        If container.oClasses.attribute_pool(i).name = view.DataGridView2.Item(1, view.DataGridView2.SelectedCells(0).RowIndex).Value Then

                            If container.oClasses.attribute_pool(i).show_workflow = 1 Then
                                If view.associated_entities(view.tassociations.SelectedIndex).parent_entities(idx).linked_Attribute_values(view.DataGridView2.SelectedCells(0).RowIndex).publish_draft_value = True Then
                                    view.associated_entities(view.tassociations.SelectedIndex).parent_entities(idx).linked_Attribute_values(view.DataGridView2.SelectedCells(0).RowIndex).publish_draft_value = False


                                    view.DataGridView2.Item(5, view.DataGridView2.SelectedCells(0).RowIndex).Value = view.associated_entities(view.tassociations.SelectedIndex).parent_entities(idx).linked_Attribute_values(view.DataGridView2.SelectedCells(0).RowIndex).original_published_value

                                    view.DataGridView2.Item(4, view.DataGridView2.SelectedCells(0).RowIndex).Value = view.tabimages.Images(push_publish_icon)

                                End If
                            End If
                            Exit For
                        End If
                    Next
                    Me.entity_edit_mode_linked()
                End If

            Else
                If val <> view.associated_entities(view.tassociations.SelectedIndex).child_entities(idx).linked_Attribute_values(view.DataGridView2.SelectedCells(0).RowIndex).value Then
                    REM see if item is a key value pair
                    For i = 0 To container.oClasses.attribute_pool.Count - 1
                        If container.oClasses.attribute_pool(i).name = view.DataGridView2.Item(1, view.DataGridView2.SelectedCells(0).RowIndex).Value Then
                            If container.oClasses.attribute_pool(i).lookup_keys.count > 0 Then
                                REM find key value for value
                                For j = 0 To container.oClasses.attribute_pool(i).lookup_values.count - 1
                                    If RTrim(LTrim(container.oClasses.attribute_pool(i).lookup_values(j))) = RTrim(LTrim(val)) Then
                                        val = container.oClasses.attribute_pool(i).lookup_keys(j)
                                        Exit For
                                    End If
                                Next
                            End If
                        End If

                    Next
                    view.associated_entities(view.tassociations.SelectedIndex).child_entities(idx).linked_Attribute_values(view.DataGridView2.SelectedCells(0).RowIndex).value = val
                    view.associated_entities(view.tassociations.SelectedIndex).child_entities(idx).linked_Attribute_values(view.DataGridView2.SelectedCells(0).RowIndex).value_changed = True
                    view.DataGridView2.Item(0, view.DataGridView2.SelectedCells(0).RowIndex).Value = view.tabimages.Images(att_edited_icon)
                    If ext = True Then
                        view.DataGridView2.Item(3, view.DataGridView1.SelectedCells(0).RowIndex).Value = val
                    End If

                    REM if item workflowed and publish requested reset this
                    For i = 0 To container.oClasses.attribute_pool.Count - 1
                        If container.oClasses.attribute_pool(i).name = view.DataGridView2.Item(1, view.DataGridView2.SelectedCells(0).RowIndex).Value Then

                            If container.oClasses.attribute_pool(i).show_workflow = 1 Then
                                If view.associated_entities(view.tassociations.SelectedIndex).child_entities(idx).linked_Attribute_values(view.DataGridView2.SelectedCells(0).RowIndex).publish_draft_value = True Then
                                    view.associated_entities(view.tassociations.SelectedIndex).child_entities(idx).linked_Attribute_values(view.DataGridView2.SelectedCells(0).RowIndex).publish_draft_value = False
                                    view.DataGridView2.Item(5, view.DataGridView2.SelectedCells(0).RowIndex).Value = view.associated_entities(view.tassociations.SelectedIndex).child_entities(idx).linked_Attribute_values(view.DataGridView2.SelectedCells(0).RowIndex).original_published_value
                                    view.DataGridView2.Item(4, view.DataGridView2.SelectedCells(0).RowIndex).Value = view.tabimages.Images(push_publish_icon)

                                End If
                            End If
                            Exit For
                        End If
                    Next
                    Me.entity_edit_mode_linked()
                End If

            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_taxonomy", "set_val", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Public Sub set_publish_linked()

        If sel_aclass_parent = True Then
            For i = 0 To container.oClasses.attribute_pool.Count - 1
                If container.oClasses.attribute_pool(i).name = view.DataGridView2.Item(1, view.DataGridView2.SelectedCells(0).RowIndex).Value Then
                    If container.oClasses.attribute_pool(i).show_workflow = 1 Then
                        If view.associated_entities(view.tassociations.SelectedIndex).parent_entities(idx).linked_Attribute_values(view.DataGridView2.SelectedCells(0).RowIndex).publish_draft_value = False Then
                            view.DataGridView2.Item(5, view.DataGridView2.SelectedCells(0).RowIndex).Value = view.DataGridView2.Item(3, view.DataGridView2.SelectedCells(0).RowIndex).Value
                            view.associated_entities(view.tassociations.SelectedIndex).parent_entities(idx).linked_Attribute_values(view.DataGridView2.SelectedCells(0).RowIndex).publish_draft_value = True
                            view.DataGridView2.Item(4, view.DataGridView2.SelectedCells(0).RowIndex).Value = view.tabimages.Images(att_edited_icon)
                            view.associated_entities(view.tassociations.SelectedIndex).parent_entities(idx).linked_Attribute_values(view.DataGridView2.SelectedCells(0).RowIndex).value_changed = True
                            Me.entity_edit_mode_linked()
                        Else
                            view.associated_entities(view.tassociations.SelectedIndex).parent_entities(idx).linked_Attribute_values(view.DataGridView2.SelectedCells(0).RowIndex).publish_draft_value = False
                            view.DataGridView2.Item(5, view.DataGridView2.SelectedCells(0).RowIndex).Value = view.associated_entities(view.tassociations.SelectedIndex).parent_entities(idx).linked_Attribute_values(view.DataGridView2.SelectedCells(0).RowIndex).original_published_value
                            view.DataGridView2.Item(4, view.DataGridView2.SelectedCells(0).RowIndex).Value = view.tabimages.Images(push_publish_icon)
                        End If
                        REM mae sure key pair selected
                        If container.oClasses.attribute_pool(i).lookup_keys.count > 0 Then
                            For j = 0 To container.oClasses.attribute_pool(i).lookup_values.count - 1
                                If RTrim(LTrim(container.oClasses.attribute_pool(i).lookup_values(j))) = RTrim(LTrim(view.DataGridView2.Item(3, view.DataGridView2.SelectedCells(0).RowIndex).Value)) Then
                                    view.associated_entities(view.tassociations.SelectedIndex).parent_entities(idx).linked_Attribute_values(view.DataGridView2.SelectedCells(0).RowIndex).value = container.oClasses.attribute_pool(i).lookup_keys(j)
                                End If
                            Next

                        End If
                    End If


                    view.DataGridView2.Item(1, view.DataGridView2.SelectedCells(0).RowIndex).Selected = True
                    Exit For
                End If
            Next
        Else
            For i = 0 To container.oClasses.attribute_pool.Count - 1
                If container.oClasses.attribute_pool(i).name = view.DataGridView2.Item(1, view.DataGridView2.SelectedCells(0).RowIndex).Value Then
                    If container.oClasses.attribute_pool(i).show_workflow = 1 Then
                        If view.associated_entities(view.tassociations.SelectedIndex).child_entities(idx).linked_Attribute_values(view.DataGridView2.SelectedCells(0).RowIndex).publish_draft_value = False Then
                            view.DataGridView2.Item(5, view.DataGridView2.SelectedCells(0).RowIndex).Value = view.DataGridView2.Item(3, view.DataGridView2.SelectedCells(0).RowIndex).Value
                            view.associated_entities(view.tassociations.SelectedIndex).child_entities(idx).linked_Attribute_values(view.DataGridView2.SelectedCells(0).RowIndex).publish_draft_value = True
                            view.DataGridView2.Item(4, view.DataGridView2.SelectedCells(0).RowIndex).Value = view.tabimages.Images(att_edited_icon)
                            view.associated_entities(view.tassociations.SelectedIndex).child_entities(idx).linked_Attribute_values(view.DataGridView2.SelectedCells(0).RowIndex).value_changed = True
                            Me.entity_edit_mode_linked()
                        Else
                            view.associated_entities(view.tassociations.SelectedIndex).child_entities(idx).linked_Attribute_values(view.DataGridView2.SelectedCells(0).RowIndex).publish_draft_value = False
                            view.DataGridView2.Item(5, view.DataGridView2.SelectedCells(0).RowIndex).Value = view.associated_entities(view.tassociations.SelectedIndex).child_entities(idx).linked_Attribute_values(view.DataGridView2.SelectedCells(0).RowIndex).original_published_value
                            view.DataGridView2.Item(4, view.DataGridView2.SelectedCells(0).RowIndex).Value = view.tabimages.Images(push_publish_icon)
                        End If
                        REM mae sure key pair selected
                        If container.oClasses.attribute_pool(i).lookup_keys.count > 0 Then
                            For j = 0 To container.oClasses.attribute_pool(i).lookup_values.count - 1
                                If RTrim(LTrim(container.oClasses.attribute_pool(i).lookup_values(j))) = RTrim(LTrim(view.DataGridView2.Item(3, view.DataGridView2.SelectedCells(0).RowIndex).Value)) Then
                                    view.associated_entities(view.tassociations.SelectedIndex).child_entities(idx).linked_Attribute_values(view.DataGridView2.SelectedCells(0).RowIndex).value = container.oClasses.attribute_pool(i).lookup_keys(j)
                                End If
                            Next

                        End If
                    End If


                    view.DataGridView2.Item(1, view.DataGridView2.SelectedCells(0).RowIndex).Selected = True
                    Exit For
                End If
            Next

        End If

    End Sub

    Public Sub attribute_link_details()
        Try
            If view.DataGridView2.SelectedCells.Count = 0 Then
                Exit Sub

            End If
            view.mdettitle.Text = "Details for: " + view.DataGridView2.Item(1, view.DataGridView2.SelectedCells(0).RowIndex).Value

            If sel_aclass_parent = True Then
                If view.associated_entities(view.tassociations.SelectedIndex).parent_entities(idx).linked_Attribute_values(view.DataGridView2.SelectedCells(0).RowIndex).show_workflow = True Then
                    view.mdetupdate.Text = "Draft Last Updated: " + view.associated_entities(view.tassociations.SelectedIndex).parent_entities(idx).linked_Attribute_values(view.DataGridView2.SelectedCells(0).RowIndex).last_update_comment
                Else
                    view.mdetupdate.Text = "Last Updated: " + view.associated_entities(view.tassociations.SelectedIndex).parent_entities(idx).linked_Attribute_values(view.DataGridView2.SelectedCells(0).RowIndex).last_update_comment

                End If
                view.mdetuser.Text = "By: " + view.associated_entities(view.tassociations.SelectedIndex).parent_entities(idx).linked_Attribute_values(view.DataGridView2.SelectedCells(0).RowIndex).last_update_user
                view.mdetpubupdate.Visible = False
                view.mdetpubuser.Visible = False
                If view.associated_entities(view.tassociations.SelectedIndex).parent_entities(idx).linked_Attribute_values(view.DataGridView2.SelectedCells(0).RowIndex).show_workflow = True Then
                    view.mdetpubupdate.Visible = True
                    view.mdetpubuser.Visible = True
                    view.mdetpubupdate.Text = "Published Last Updated: " + view.associated_entities(view.tassociations.SelectedIndex).parent_entities(idx).linked_Attribute_values(view.DataGridView2.SelectedCells(0).RowIndex).last_update_published_comment
                    view.mdetpubuser.Text = "Published By: " + view.associated_entities(view.tassociations.SelectedIndex).parent_entities(idx).linked_Attribute_values(view.DataGridView2.SelectedCells(0).RowIndex).last_update_published_user
                End If
            Else
                If view.associated_entities(view.tassociations.SelectedIndex).child_entities(idx).linked_Attribute_values(view.DataGridView2.SelectedCells(0).RowIndex).show_workflow = True Then
                    view.mdetupdate.Text = "Draft Last Updated: " + view.associated_entities(view.tassociations.SelectedIndex).child_entities(idx).linked_Attribute_values(view.DataGridView2.SelectedCells(0).RowIndex).last_update_comment
                Else
                    view.mdetupdate.Text = "Last Updated: " + view.associated_entities(view.tassociations.SelectedIndex).child_entities(idx).linked_Attribute_values(view.DataGridView2.SelectedCells(0).RowIndex).last_update_comment

                End If
                view.mdetuser.Text = "By: " + view.associated_entities(view.tassociations.SelectedIndex).child_entities(idx).linked_Attribute_values(view.DataGridView2.SelectedCells(0).RowIndex).last_update_user
                view.mdetpubupdate.Visible = False
                view.mdetpubuser.Visible = False
                If view.associated_entities(view.tassociations.SelectedIndex).child_entities(idx).linked_Attribute_values(view.DataGridView2.SelectedCells(0).RowIndex).show_workflow = True Then
                    view.mdetpubupdate.Visible = True
                    view.mdetpubuser.Visible = True
                    view.mdetpubupdate.Text = "Published Last Updated: " + view.associated_entities(view.tassociations.SelectedIndex).child_entities(idx).linked_Attribute_values(view.DataGridView2.SelectedCells(0).RowIndex).last_update_published_comment
                    view.mdetpubuser.Text = "Published By: " + view.associated_entities(view.tassociations.SelectedIndex).child_entities(idx).linked_Attribute_values(view.DataGridView2.SelectedCells(0).RowIndex).last_update_published_user
                End If
            End If

            view.attributedetails.Show(Cursor.Position.X, Cursor.Position.Y)
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("DataGridView2", "DoubleClick", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            view.Cursor = Cursors.Default
        End Try
    End Sub

End Class
