Imports System.Windows.Forms
Imports System.Threading
Imports System.Drawing
Imports BlueCurve.Core.OM
Imports BlueCurve.Core.CS
Imports BlueCurve.Core.AS


Public Class bc_am_cp_main
    Public classes As New bc_om_entity_classes
    Public entities As New bc_om_entities
    Public sel_cls_idx As Integer
    Public class_name As String
    Public mode As Integer
    Public progress_thread As Thread
    Public schema_id As Long = 1
    Public attribute_mode As Integer = 1
    Public associated_entities As New ArrayList
    Public all_parent_links As New ArrayList
    Public all_child_links As New ArrayList
    Public sconn As String
    Public suser As String
    Public new_entity As Boolean = False
    Public ousers As New bc_om_users
    Friend controller As Object
    Public from_tree_view As Boolean = False
    Public not_from_filter As Boolean = False
    Private Const class_icon As Integer = 0
    Private Const selected_icon As Integer = 1
    Private Const unlinked_icon As Integer = 2
    Private Const data_icon As Integer = 3

    Private Const child_parent_icon = 0
    Private Const parent_child_icon = 1
    Private Const links_icon = 2
    Private Const attributes_icon = 3
    Private Const add_icon As Integer = 4
    Private Const edit_icon As Integer = 5
    Private Const deactivate_icon As Integer = 6
    Private Const delete_icon As Integer = 7
    Private Const selected_class_icon As Integer = 8
    Private Const mandatory_icon As Integer = 9
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



    Private Function has_links_for_schema(ByVal class_links As ArrayList, ByVal schema_id As Long) As Boolean
        Dim i As Integer
        has_links_for_schema = False
        For i = 0 To class_links.Count - 1
            If class_links(i).schema_id = schema_id Then
                has_links_for_schema = True
                Exit For
            End If
        Next
    End Function

    Private Sub propogate_class(ByRef node As TreeNode, ByVal class_id As Long, ByVal schema_id As Long, ByVal direction As Boolean, ByVal class_name As String, ByRef class_found As Boolean)
        Try
            Dim i, j, l As Integer
            Dim k As TreeNode
            For i = 0 To classes.classes.Count - 1
                If classes.classes(i).class_id = class_id Then
                    If direction = True Then
                        For j = 0 To classes.classes(i).child_links.count - 1
                            For l = 0 To classes.classes.Count - 1
                                If classes.classes(l).class_id = classes.classes(i).child_links(j).child_class_id And classes.classes(i).child_links(j).schema_id = schema_id Then
                                    If classes.classes(i).child_links(j).mandatory_pc = 0 Then
                                        k = New TreeNode(classes.classes(l).class_name)
                                    ElseIf classes.classes(i).child_links(j).mandatory_pc = -1 Then
                                        k = New TreeNode(classes.classes(l).class_name + "*")
                                    Else
                                        k = New TreeNode(classes.classes(l).class_name + " (" + CStr(classes.classes(i).child_links(j).mandatory_pc) + ")")
                                    End If
                                    propogate_class(k, classes.classes(i).child_links(j).child_class_id, schema_id, direction, class_name, class_found)
                                    k.ImageIndex = class_icon
                                    node.Nodes.Add(k)

                                    Exit For
                                End If
                            Next
                        Next
                        REM no more links

                    Else
                        For j = 0 To classes.classes(i).parent_links.count - 1
                            For l = 0 To classes.classes.Count - 1
                                If classes.classes(l).class_id = classes.classes(i).parent_links(j).parent_class_id And classes.classes(i).parent_links(j).schema_id = schema_id Then
                                    If classes.classes(i).parent_links(j).mandatory_cp = 0 Then
                                        k = New TreeNode(classes.classes(l).class_name)
                                    ElseIf classes.classes(i).parent_links(j).mandatory_cp = -1 Then
                                        k = New TreeNode(classes.classes(l).class_name + "*")
                                    Else
                                        k = New TreeNode(classes.classes(l).class_name + " (" + CStr(classes.classes(i).parent_links(j).mandatory_cp) + ")")
                                    End If
                                    propogate_class(k, classes.classes(i).parent_links(j).parent_class_id, schema_id, direction, class_name, class_found)
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
            Dim oerr As New bc_cs_error_log("bc_am_cp_main", "propogate_class", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub

    Public reset As Boolean = False

    Private Sub load_attribute_values()
        Try
            For j = 0 To Me.associated_entities(Me.tassociations.SelectedIndex).attribute_values.count - 1
                If Me.DataGridView1.RowCount > j Then
                    Me.DataGridView1.Item(3, j).Value = Me.associated_entities(Me.tassociations.SelectedIndex).attribute_values(j).value
                    If Me.associated_entities(Me.tassociations.SelectedIndex).attribute_values(j).show_workflow = False Then
                        Me.DataGridView1.Item(5, j).Value = "n/a"
                        Me.DataGridView1.Item(4, j).Value = New Drawing.Bitmap(1, 1)
                    Else
                        If Me.associated_entities(Me.tassociations.SelectedIndex).attribute_values(j).publish_draft_value = False Then
                            Me.DataGridView1.Item(4, j).Value = Me.tabimages.Images(push_publish_icon)
                            Me.DataGridView1.Item(5, j).Value = Me.associated_entities(Me.tassociations.SelectedIndex).attribute_values(j).published_value
                        Else
                            Me.DataGridView1.Item(4, j).Value = Me.tabimages.Images(selected_icon)
                            Me.DataGridView1.Item(5, j).Value = Me.DataGridView1.Item(3, j).Value

                        End If
                    End If
                    If Me.associated_entities(Me.tassociations.SelectedIndex).attribute_values(j).value_changed = True Then
                        Me.DataGridView1.Item(0, j).Value = Me.tabimages.Images(0)
                    End If
                End If
            Next
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_cp_main", "load_attribute_values", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try
    End Sub

    Public bload As Boolean = False
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try
          
            Dim bcs As New bc_cs_central_settings(True)
            Me.Cursor = Cursors.WaitCursor

            Me.entityContextMenuStrip.ImageList = tabimages

            Me.entityContextMenuStrip.Items(0).Image = tabimages.Images(add_icon)
            Me.entityContextMenuStrip.Items(1).Image = tabimages.Images(edit_icon)
            Me.entityContextMenuStrip.Items(3).Image = tabimages.Images(deactivate_icon)
            Me.entityContextMenuStrip.Items(5).Image = tabimages.Images(delete_icon)

            Me.schemacontextmenu.ImageList = tabimages

            Me.schemacontextmenu.Items(0).Image = tabimages.Images(add_icon)
            Me.schemacontextmenu.Items(1).Image = tabimages.Images(edit_icon)
            Me.schemacontextmenu.Items(2).Image = tabimages.Images(deactivate_icon)
            Me.schemacontextmenu.Items(4).Image = tabimages.Images(delete_icon)

            Me.classContextMenuStrip.ImageList = tabimages
            Me.classContextMenuStrip.Items(0).Image = tabimages.Images(add_icon)
            Me.classContextMenuStrip.Items(2).Image = tabimages.Images(edit_icon)
            Me.classContextMenuStrip.Items(3).Image = tabimages.Images(deactivate_icon)
            Me.classContextMenuStrip.Items(4).Image = tabimages.Images(delete_icon)
            Me.classContextMenuStrip.Items(6).Image = tabimages.Images(add_icon)
            Me.classContextMenuStrip.Items(7).Image = tabimages.Images(0)
            Me.classContextMenuStrip.Items(9).Image = tabimages.Images(delete_icon)

            Me.adminattributemenustrip.ImageList = tabimages

            Me.adminattributemenustrip.Items(0).Image = tabimages.Images(add_icon)
            Me.adminattributemenustrip.Items(1).Image = tabimages.Images(add_icon)
            Me.adminattributemenustrip.Items(3).Image = tabimages.Images(edit_icon)
            Me.adminattributemenustrip.Items(5).Image = tabimages.Images(delete_icon)
            Me.adminattributemenustrip.Items(7).Image = tabimages.Images(delete_icon)

            Me.assignentity.ImageList = tabimages
            Me.assignentity.Items(0).Image = tabimages.Images(edit_icon)
            Me.assignentity.Items(1).Image = tabimages.Images(0)
            Me.assignentity.Items(2).Image = tabimages.Images(0)

            tfilter.SearchSetup(False)


            Me.Cursor = Cursors.Default

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_cp_main", "load", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub

    Public Sub load_class_list()
        controller.load_class_list()
    End Sub

    Dim current_node As TreeNode
    Dim treset As Boolean = False

    Private Sub TreeView1_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles ttaxonomy.AfterSelect
        Try
            Me.from_tree_view = True
            Me.Cursor = Cursors.WaitCursor
            Me.controller.class_selected(Me.ttaxonomy.SelectedNode.Text, True)

        Catch ex As Exception

            Dim oerr As New bc_cs_error_log("ttaxonomy", "AfterSelect", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            If ttaxonomy.Visible = True Then
                controller.from_class_List = False
                controller.load_class_toolbar()
            End If
            Me.from_tree_view = False
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    'Private Sub tfilter_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    controller.load_entities(Me.cclass.Text)
    'End Sub

    Public gadd_links As New ArrayList
    Public gdel_links As New ArrayList
    Private Sub traverse_link(ByVal class_name As String, ByVal entity_name As String)
        REM if assocaited entity navigate there else reload
        Dim aname As String
        aname = entity_name + "(" + class_name + ")"
        For i = 0 To Me.tassociations.TabPages.Count - 1
            If Me.tassociations.TabPages(i).Text = aname Or Me.tassociations.TabPages(i).Text = entity_name Then
                Me.tassociations.SelectedIndex = i
                Exit Sub
            End If
        Next
        If Me.bentchanges.Enabled = True Then
            'Dim omsg As New bc_cs_message("Blue Curve", "You are in edit mode cannot navidate to " + Me.lselparent.Text + " as it is not in working portfolio", bc_cs_message.MESSAGE)
        Else

            Me.class_name = class_name
            Me.cclass.Text = class_name

            'Me.load_class_attributes(class_name)
            Me.ttaxonomy.HideSelection = True
            Me.Lentities.SelectedItems(0).Text = entity_name
        End If
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        System.Windows.Forms.Application.Exit()
    End Sub

    Dim rdrag As Boolean
    Dim ldrag As Boolean
    Private Sub lallparent_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        rdrag = True
    End Sub

    Private Sub DataGridView1_CellStateChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellStateChangedEventArgs) Handles DataGridView1.CellStateChanged

    End Sub

    Private Sub DataGridView1_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles DataGridView1.DataError

    End Sub

    Private Sub DataGridView2_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles DataGridView2.DataError

    End Sub

    Private Sub DataGridView1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridView1.DoubleClick
        Try
            If Me.DataGridView1.SelectedCells(0).ColumnIndex = 3 Or Me.DataGridView1.RowCount = 0 Then
                Exit Sub
            End If
            controller.attribute_details(False)
        Catch

        End Try

    End Sub
    Private Sub DataGridView2_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridView2.DoubleClick
        Try
            If Me.DataGridView2.SelectedCells(0).ColumnIndex = 3 Or Me.DataGridView2.RowCount = 0 Then
                Exit Sub
            End If
            controller.attribute_link_details()
        Catch

        End Try

    End Sub

    Private Sub DataGridView1_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridView1.SelectionChanged
        Try

            If Me.controller.container.mode = bc_am_cp_container.VIEW Or Me.DataGridView1.RowCount = 0 Then
                REM read only
                Exit Sub
            End If

            controller.load_entity_toolbar()
            If Me.DataGridView1.SelectedCells.Count = 0 Then

                Exit Sub
            End If

            If Me.DataGridView1.SelectedCells(0).ColumnIndex = 3 Then

                'If Me.DataGridView1.Columns(3).ReadOnly = False Then
                controller.set_data_type_control()
                'Else
                '    Exit Sub
                'End If
            End If
            If Me.DataGridView1.SelectedCells(0).ColumnIndex = 4 Then
                controller.set_publish()
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("DataGridView1", "SelectionChanged", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try
    End Sub
    Private Sub DataGridView2_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridView2.SelectionChanged
        Try
            If Me.controller.container.mode = bc_am_cp_container.VIEW Or Me.DataGridView2.RowCount = 0 Then
                REM read only
                Exit Sub
            End If
            controller.load_entity_toolbar()
            If Me.DataGridView2.SelectedCells.Count = 0 Then
                Exit Sub
            End If

            If Me.DataGridView2.SelectedCells(0).ColumnIndex = 3 Then

                'If Me.DataGridView2.Columns(3).ReadOnly = False Then

                controller.set_data_type_control_linked()
                'Else
                '    Exit Sub
                'End If
            End If
            If Me.DataGridView2.SelectedCells(0).ColumnIndex = 4 Then
                controller.set_publish_linked()
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("DataGridView2", "SelectionChanged", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try
    End Sub

    Public Sub clear_attribute_links()
        For i = 1 To Me.tattributes.TabPages.Count - 1
            Me.tattributes.TabPages.RemoveAt(1)
        Next
    End Sub

    Private Sub cschema_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles cschema.MouseEnter
        controller.load_class_toolbar()
    End Sub

    Private Sub cschema_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cschema.SelectedIndexChanged
        Me.Cursor = Cursors.WaitCursor
        controller.select_schema(Me.cschema.Text)
        controller.load_class_toolbar()
        Me.Cursor = Cursors.Default
    End Sub





    Private Sub tassociations_TabIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tassociations.SelectedIndexChanged
        Try
            Me.Cursor = Cursors.WaitCursor
            If Me.tattributes.TabPages.Count = 2 Then
                Me.tattributes.TabPages.RemoveAt(1)

            End If
            controller.load_entity_parts(Me.associated_entities(Me.tassociations.SelectedIndex).name, Me.associated_entities(Me.tassociations.SelectedIndex).class_name, False)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        Me.Cursor = Cursors.Default
        Me.passociations.Visible = True
    End Sub

    Public warnings As New ArrayList

    Private Sub lvwvalidate_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvwvalidate.DoubleClick
        Try
            If Me.lvwvalidate.SelectedItems.Count > 0 Then
                Me.tassociations.SelectedIndex = warnings(Me.lvwvalidate.SelectedItems(0).Index).association_tab
                Me.tattributes.SelectedIndex = warnings(Me.lvwvalidate.SelectedItems(0).Index).attribute_tab
                If warnings(Me.lvwvalidate.SelectedItems(0).Index).entity_tab > -1 Then
                    Me.tentities.SelectedNode = Me.tentities.Nodes(warnings(Me.lvwvalidate.SelectedItems(0).Index).entity_tab)
                Else
                    If warnings(Me.lvwvalidate.SelectedItems(0).Index).attribute_tab = 1 Then
                        'For i = 0 To Me.Tabparent.TabPages.Count - 1
                        '    cl = Me.Tabparent.TabPages(i).Text
                        '    If warnings(Me.lvwvalidate.SelectedItems(0).Index).linked_class_name.length <= cl.Length Then
                        '        If cl.Substring(0, warnings(Me.lvwvalidate.SelectedItems(0).Index).linked_class_name.length) = warnings(Me.lvwvalidate.SelectedItems(0).Index).linked_class_name Then
                        '            Me.Tabparent.SelectedIndex = i
                        '            Me.lselparent.Text = warnings(Me.lvwvalidate.SelectedItems(0).Index).linked_entity_name
                        '            Me.tattributes.SelectedIndex = 1
                        '            Exit For
                        '        End If
                        '    End If
                        'Next
                        Me.DataGridView1.Focus()
                        Me.DataGridView1.Item(1, warnings(Me.lvwvalidate.SelectedItems(0).Index).attribute).Selected = True
                    Else
                        Me.DataGridView1.Focus()
                        Me.DataGridView1.Item(1, warnings(Me.lvwvalidate.SelectedItems(0).Index).attribute).Selected = True
                    End If
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        controller.small_entity_filter()
    End Sub

    Private Sub tattributes_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tattributes.SelectedIndexChanged
        Try
            If Me.tattributes.SelectedIndex = 1 Then
                controller.load_associated_class_linked_attributes(Me.tentities.SelectedNode.Text)
                Me.DataGridView2.Visible = True
                Me.DataGridView1.Visible = False

            Else
                Me.DataGridView2.Visible = False
                Me.DataGridView1.Visible = True
            End If


            REM now set warnings
            For i = 0 To Me.warnings.Count - 1
                If Me.warnings(i).association_tab = Me.tassociations.SelectedIndex Then
                    If Me.warnings(i).entity_tab = -1 Then
                        If Me.warnings(i).attribute_tab = 0 And Me.tattributes.SelectedIndex = 0 Then
                            Me.DataGridView1.Item(0, Me.warnings(i).attribute).Value = Me.tabimages.Images(0)
                            Me.tattributes.TabPages(0).ImageIndex = attributes_icon
                        Else
                            If Me.tattributes.SelectedIndex = 1 Then
                                'If Me.warnings(i).linked_class_name = controller.get_real_class_name(Me.Tabparent.SelectedTab.Text) And Me.warnings(i).linked_entity_name = Me.lselparent.Text Then
                                '    Me.DataGridView1.Item(0, Me.warnings(i).attribute).Value = Me.tabimages.Images(3)
                                '    Me.tattributes.TabPages(1).ImageIndex = 2
                                'End If
                            End If
                        End If
                    End If
                End If

            Next
        Catch ex As Exception
            'Dim oerr As New bc_cs_error_log("tattributes", "SelecedIndexChanged", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try
    End Sub
    Private Function check_has_linked_attributes() As Boolean
        Dim selected_master_class_index As Long
        Dim selected_linked_class_index As Long
        Dim selected_linked_class As Long
        check_has_linked_attributes = False
        For i = 0 To Me.classes.classes.Count - 1
            If Me.associated_entities(Me.tassociations.SelectedIndex).class_id = Me.classes.classes(i).class_id Then
                selected_master_class_index = i
            End If
            If controller.get_real_class_name(Me.tentities.SelectedNode.Text) = Me.classes.classes(i).class_name Then
                selected_linked_class_index = i
                selected_linked_class = Me.classes.classes(i).class_id
            End If
        Next

        If controller.is_parent_class = True Then
            Dim pi As Integer
            pi = controller.get_parent_class_index

            REM  child parent link
            If Me.classes.classes(selected_master_class_index).parent_links(pi).linked_attributes.count > 0 Then
                check_has_linked_attributes = True
            End If
        Else
            Dim ci As Integer
            ci = controller.get_parent_class_index

            REM parent child link
            If Me.classes.classes(selected_master_class_index).child_links(ci).linked_attributes.count > 0 Then
                check_has_linked_attributes = True
            End If
        End If
    End Function

    Private Sub lclassattributes_SelectedIndexChanged1(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvwatt.SelectedIndexChanged
        Try
            Me.controller.load_class_toolbar()
            Dim class_name As String
            Me.breadonly.Enabled = False
            class_name = Me.controller.get_real_class_name(Me.ttaxonomy.SelectedNode.Text)
            If Me.lvwatt.SelectedItems.Count >= 0 Then
                REM And Me.tattributelinks.SelectedIndex = 0 Then
                Me.breadonly.Enabled = True
            End If
            If Me.lvwatt.SelectedItems.Count = -1 Or Me.lvwatt.Items.Count = 1 Then
                Me.Batup.Enabled = False
                Me.batdn.Enabled = False
            Else
                If Me.lvwatt.SelectedItems(0).Index = 0 Then
                    Me.Batup.Enabled = False
                    Me.batdn.Enabled = True
                End If
                If Me.lvwatt.SelectedItems(0).Index = Me.lvwatt.Items.Count - 1 Then
                    Me.Batup.Enabled = True
                    Me.batdn.Enabled = False
                End If
                If Me.lvwatt.SelectedItems(0).Index > 0 And Me.lvwatt.SelectedItems(0).Index < Me.lvwatt.Items.Count - 1 Then
                    Me.Batup.Enabled = True
                    Me.batdn.Enabled = True
                End If
            End If


            If Me.lvwatt.SelectedItems(0).Index > -1 Then
                Me.RemoveAttributeFromClassToolStripMenuItem.Enabled = True
                If Me.tattributelinks.SelectedIndex = 0 Then
                    Me.RemoveAttributeFromClassToolStripMenuItem.Text = "Remove " + Me.lvwatt.SelectedItems(0).Text + " from " + class_name
                Else
                    Me.RemoveAttributeFromClassToolStripMenuItem.Text = "Remove " + Me.lvwatt.SelectedItems(0).Text + " from " + class_name + " " + Me.tattributelinks.SelectedTab.Text
                End If
                Me.modatt.Enabled = True
                Me.RemoveAttributeFromClassToolStripMenuItem.Enabled = True

                Me.modatt.Text = "Modify attribute " + Me.lvwatt.SelectedItems(0).Text
                Me.RemoveAttributeFromClassToolStripMenuItem.Enabled = True
                Me.DeleteAttributeToolStripMenuItem.Enabled = True
                DeleteAttributeToolStripMenuItem.Text = "Delete attribute " + Me.lvwatt.SelectedItems(0).Text

            Else
                Me.RemoveAttributeFromClassToolStripMenuItem.Text = "Remove attribute from class"
                Me.RemoveAttributeFromClassToolStripMenuItem.Enabled = False
                Me.modatt.Enabled = False
                Me.DeleteAttributeToolStripMenuItem.Enabled = False
            End If
        Catch ex As Exception



        End Try
    End Sub

    Private Sub RemoveAttributeFromClassToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RemoveAttributeFromClassToolStripMenuItem.Click
        Me.Cursor = Cursors.WaitCursor
        controller.remove_attribute_from_class()
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub batup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Batup.Click
        Me.Cursor = Cursors.WaitCursor
        controller.move_attribute_up()
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub batdn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles batdn.Click
        Me.Cursor = Cursors.WaitCursor
        controller.move_attribute_down()
        Me.Cursor = Cursors.Default
    End Sub


    Private Sub tattributelinks_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tattributelinks.SelectedIndexChanged
        controller.set_up_class_attributes()
    End Sub



    Private Sub mrempl_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mrempl.Click
        controller.remove_class_link(Me.ttaxonomy.SelectedNode.Text)
    End Sub

    Private Sub DeleteSchemaToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteSchemaToolStripMenuItem.Click
        Me.Cursor = Cursors.WaitCursor
        controller.delete_schema(Me.cschema.Text)
        Me.Cursor = Cursors.Default
    End Sub


    Private Sub DeleteEntityToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteEntityToolStripMenuItem.Click
        controller.delete_entity()
    End Sub

    Private Sub mdelclass_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mdelclass.Click
        controller.delete_class(Me.ttaxonomy.SelectedNode.Text)
    End Sub


    Private Sub DeleteAttributeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteAttributeToolStripMenuItem.Click
        controller.delete_attribute()
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub PictureBox2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pdn.Click
        prop_pc()
    End Sub
    Private Sub prop_pc()

        If Me.bentchanges.Visible = True Then
            If Me.bentchanges.Enabled = True Then
                Dim omsg As New bc_cs_message("Blue Curve", "You have outstanding change data for " + Me.Lentities.SelectedItems(0).Text + " are you sure you wish to change taxonomy view ", bc_cs_message.MESSAGE, True, False, "Yes", "No")
                If omsg.cancel_selected Then
                    Exit Sub
                End If
            End If
        End If
        Me.pdn.BorderStyle = BorderStyle.Fixed3D
        Me.pup.BorderStyle = BorderStyle.None
        Me.Cursor = Cursors.WaitCursor
        Me.controller.load_class_view(True)

        Me.Cursor = Cursors.Default

    End Sub
    Private Sub PictureBox3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pup.Click
        prop_cp()
    End Sub
    Private Sub prop_cp()
        If Me.bentchanges.Visible = True Then
            If Me.bentchanges.Enabled = True Then
                Dim omsg As New bc_cs_message("Blue Curve", "You have outstanding change data for " + Me.Lentities.SelectedItems(0).Text + " are you sure you wish to change taxonomy view ", bc_cs_message.MESSAGE, True, False, "Yes", "No")
                If omsg.cancel_selected Then
                    Exit Sub
                End If
            End If
        End If
        Me.pup.BorderStyle = BorderStyle.Fixed3D
        Me.pdn.BorderStyle = BorderStyle.None
        Me.Cursor = Cursors.WaitCursor
        Me.controller.load_class_view(False)

        Me.Cursor = Cursors.Default
    End Sub


    Private Sub AboutCommonPlatformToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim oabout As New bc_cs_message("Blue Curve", "Common Platform " + bc_cs_central_settings.version + ". Copyright Blue Curve Limited " + CStr(Now.Year), bc_cs_message.MESSAGE, False, True)
    End Sub

    Private Sub cclass_SelectedIndexChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If Me.cclass.SelectedIndex = -1 Then
            Exit Sub
        End If
        Me.sel_cls_idx = Me.cclass.SelectedIndex
        controller.load_class_attributes(Me.cclass.Text, True)
        If Me.mode = 1 Then
            Me.pattributes.Visible = False
        End If
        Me.class_name = Me.cclass.Text
        Me.controller.load_unused_attributes()
        Me.Lentities.Visible = True
        Me.tfilter.Visible = True

    End Sub

    Private Sub tentities_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tentities.AfterSelect
        set_assign_menu()
    End Sub
    Private Sub set_assign_menu()

        Me.massignentity.Visible = False
        Me.massignentity.Text = ""
        Dim t As String
        Me.IncreaseRatingToolStripMenuItem.Visible = False
        Me.DecreaseRatingToolStripMenuItem.Visible = False
        Me.IncreaseRatingToolStripMenuItem.Image = uxTaxonomyImageList.Images(5)
        Me.DecreaseRatingToolStripMenuItem.Image = uxTaxonomyImageList.Images(6)
        Try
            Me.tattributes.Enabled = False
            If tattributes.TabPages.Count = 2 Then
                tattributes.TabPages.RemoveAt(1)
            End If
            If controller.load_linked_attributes = True Then
                tattributes.TabPages.Add("Linked to " + Me.tentities.SelectedNode.Text)
                tattributes.TabPages(1).ImageIndex = 2
                Me.tattributes.Enabled = True
            End If


            t = Me.tentities.SelectedNode.Parent.Text
            REM if entity selected
            If Me.tentities.SelectedNode.Index > 0 Then

                Me.IncreaseRatingToolStripMenuItem.Visible = True
            End If
            If Me.tentities.SelectedNode.Index < Me.tentities.SelectedNode.Parent.Nodes.Count - 1 Then

                Me.DecreaseRatingToolStripMenuItem.Visible = True
            End If
            If Len(t) > 5 Then
                If t.Substring(0, 5) = "Users" Then
                    t = "Users"
                End If
            End If

            If t = "Users" And InStr(Me.tentities.SelectedNode.Text, "(inactive)") = 0 Then
                Me.IncreaseRatingToolStripMenuItem.Text = "Increase Rating of " + Me.tentities.SelectedNode.Text.Substring(3, Me.tentities.SelectedNode.Text.Length - 3) + " in " + Me.tassociations.SelectedTab.Text
                Me.DecreaseRatingToolStripMenuItem.Text = "Decrease Rating of " + Me.tentities.SelectedNode.Text.Substring(3, Me.tentities.SelectedNode.Text.Length - 3) + " in " + Me.tassociations.SelectedTab.Text
            Else
                'If controller.is_parent_class(controller.get_real_class_name(Me.tentities.SelectedNode.Parent.Text)) = True Then
                'Me.IncreaseRatingToolStripMenuItem.Visible = False
                'Me.DecreaseRatingToolStripMenuItem.Visible = False
                'Else
                Me.IncreaseRatingToolStripMenuItem.Text = "Increase Rating of " + Me.tentities.SelectedNode.Text + " assigned to " + Me.tassociations.SelectedTab.Text
                Me.DecreaseRatingToolStripMenuItem.Text = "Decrease Rating of " + Me.tentities.SelectedNode.Text + " assigned to  " + Me.tassociations.SelectedTab.Text
                'End If
            End If

        Catch ex As Exception
            REM if class selected
            Me.massignentity.Visible = True

            t = controller.get_real_class_name(Me.tentities.SelectedNode.Text)
            If t = "" Then
                t = "Users"
            End If
            Me.massignentity.Text = "Assignment of  " + t + " to " + Me.tassociations.SelectedTab.Text

        End Try
        controller.load_entity_toolbar()

    End Sub

    Private Sub ChangeNameToolStripMenuItem_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChangeNameToolStripMenuItem.Click
        controller.change_entity_menu()
    End Sub
    Private Sub cschemael_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cschemael.SelectedIndexChanged
        Me.tfilter.Enabled = False
        Me.cschema.Text = Me.cschemael.Text
        Me.DataGridView1.Rows.Clear()
        Me.Lentities.Items.Clear()

        Me.tattributes.Enabled = False
        unload_entity()
        controller.load_entity_toolbar()
        Me.Lentities.ContextMenuStrip = Nothing

    End Sub

    Private Sub pfilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.tfilter.SearchText = ""
    End Sub


    Private Sub mschactive_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mschactive.Click
        Me.Cursor = Cursors.WaitCursor
        Me.controller.activate_schema(Me.cschema.Text)
        Me.Cursor = Cursors.Default

    End Sub


    Private Sub ChangeSchemaNameToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChangeSchemaNameToolStripMenuItem.Click
        controller.change_schema_menu()
    End Sub

    Private Sub mclactive_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mclactive.Click
        If Me.ttaxonomy.Text <> "Unlinked Classes" Then
            controller.activate_class(Me.ttaxonomy.SelectedNode.Text)
        End If
    End Sub

    Private Sub pclass_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles pclass.MouseEnter

        controller.load_class_toolbar()
    End Sub
    Private Sub pattributes_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles pattributes.MouseEnter
        controller.load_attribute_toolbar()
    End Sub

    Private Sub pclass_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs)

    End Sub

    Private Sub AddNewSchemaToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddNewSchemaToolStripMenuItem.Click
        controller.add_schema_menu()
    End Sub

    Private Sub ttaxonomy_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles ttaxonomy.MouseEnter
        controller.from_class_list = False
        controller.load_class_toolbar()
    End Sub

    Private Sub cschema_SelectedIndexChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cschema.SelectedIndexChanged
        controller.load_entity_toolbar()
        Me.Lentities.ContextMenuStrip = Nothing
    End Sub

    Private Sub ttaxonomy_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles ttaxonomy.AfterSelect

    End Sub
    Private Sub AddEntityToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddEntityToolStripMenuItem.Click
        controller.add_entity_menu()
    End Sub

    Private Sub OKToolStripMenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OKToolStripMenuItem2.Click
        controller.add_class_menu()
        Cursor = Cursors.Default
    End Sub

    Private Sub mrenameclass_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mrenameclass.Click
        controller.rename_class_menu()
        Cursor = Cursors.Default
    End Sub

    Private Sub maddplink_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles maddplink.Click
        controller.add_link_menu()
        Cursor = Cursors.Default
    End Sub

    Private Sub tmsetlinknumber_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmsetlinknumber.Click
        controller.set_number()
        Cursor = Cursors.Default
    End Sub


    Private Sub cclass_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cclass.SelectedIndexChanged
        Try


            Cursor = Cursors.WaitCursor

            not_from_filter = True
            Me.tfilter.SearchText = ""
            not_from_filter = False
            controller.from_class_list = False
            If Me.pclass.Visible = False Then
                controller.from_class_list = True
            End If
            unload_entity()
            Me.AddEntityToolStripMenuItem.Text = "Add new " + Me.cclass.Text
            If Me.cclass.SelectedIndex > -1 Then
                Me.tfilter.Enabled = True
            End If

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                classes.db_read()
            Else
                classes.tmode = bc_cs_soap_base_class.tREAD
                If classes.transmit_to_server_and_receive(classes, True) = False Then
                    Exit Sub
                End If
            End If

            For i = 0 To classes.classes.Count - 1
                If classes.classes(i).class_name = Me.cclass.Text Then

                    tfilter.SearchClass = classes.classes(i).class_id
                    REM If attribute search on build list of attributes for class
                    If tfilter.SearchAttributes <> 0 Then
                        'tfilter.AttributeControlBuild(True)
                        tfilter.SearchSetup(False)
                    End If

                End If
            Next i

            controller.set_class_in_tree_node(Me.cclass.Text)
            controller.load_entity_toolbar()

            load_attribute_filter(tfilter.SearchClass)

            REM new search


        Catch

        Finally
            Cursor = Cursors.Default

        End Try

    End Sub
    Public Sub real_time_serach_complete(entity As bc_om_entity)
        MsgBox("sss")
        load_entity_real_time_search(entity)
    End Sub

    Public Sub load_attribute_filter(class_id As Integer)
        controller.load_attribute_filter(class_id)

    End Sub
    Public Sub unload_entity()
        Me.passociations.Enabled = False
        Me.tassociations.Enabled = False
        Me.tassociations.TabPages(0).Text = "no selection"
        Me.tassociations.TabPages(0).ImageIndex = deactivate_icon

        Me.tattributes.TabPages(0).Text = "Attributes"
        Me.tattributes.TabPages(0).ImageIndex = attributes_icon
        clear_attribute_vals()
        Me.tentities.Nodes.Clear()
    End Sub

    Private Sub pclass_Paint_1(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles pclass.Paint

    End Sub
    'Private Sub tfilter_TextChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    If not_from_filter = True Then
    '        Exit Sub
    '    End If
    '    Me.clear_attribute_vals()

    '    controller.load_entities(Me.cclass.Text)
    '    'Me.Lentities.SelectedItems


    'End Sub


    Private Sub AddExistingAttributeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddExistingAttributeToolStripMenuItem.Click
        controller.add_exisiting_attribute_menu()
    End Sub

    Private Sub AddNewAttributeToClassToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddNewAttributeToClassToolStripMenuItem.Click
        Me.Cursor = Cursors.WaitCursor
        controller.add_new_attribute_menu()
        Me.Cursor = Cursors.Default
    End Sub



    Private Sub modatt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles modatt.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            controller.modify_attribute_menu()
        Catch
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub


    Private Sub clear_attribute_vals()
        Me.tattributes.TabPages(0).Text = "Attributes"
        Me.tattributes.TabPages(0).ImageIndex = attributes_icon
        Me.tentities.Nodes.Clear()

        For i = 0 To Me.DataGridView1.Rows.Count - 1
            Me.DataGridView1.Item(3, i).Value = ""
            Me.DataGridView1.Item(5, i).Value = ""
        Next
        Me.Refresh()
    End Sub

    Private Sub lentities_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Lentities.SelectedIndexChanged
        load_entity()
    End Sub
    Private Sub load_entity()
        Me.baudit.Enabled = False
        Me.bentaudit.Enabled = False
        If Me.Lentities.SelectedItems.Count = 0 Then
            Me.passociations.Enabled = False
            Me.tassociations.Enabled = False
            Me.ChangeNameToolStripMenuItem.Enabled = False
            Me.DeleteEntityToolStripMenuItem.Enabled = False
            Me.minactive.Enabled = False
            controller.load_entity_toolbar()
            Exit Sub
        End If
        Me.baudit.Enabled = True
        Me.bentaudit.Enabled = True


        Me.Cursor = Cursors.WaitCursor
        Me.tassociations.TabPages(0).Text = "loading..."
        Me.tassociations.TabPages(0).ImageIndex = deactivate_icon

        clear_attribute_vals()


        Me.passociations.Enabled = True
        Me.tassociations.Enabled = True
        Me.new_entity = False
        controller.new_entity = False


        controller.load_entity_parts(Me.Lentities.SelectedItems(0).Text, Me.cclass.Text, True)

        If InStr(Me.Lentities.SelectedItems(0).Text, "(inactive)") > 0 Then
            Me.DeleteEntityToolStripMenuItem.Enabled = True
            Me.ChangeNameToolStripMenuItem.Enabled = False
        Else
            Me.ChangeNameToolStripMenuItem.Enabled = True
            Me.DeleteEntityToolStripMenuItem.Enabled = False
        End If

        Me.bentchanges.Enabled = False
        Me.bentdiscard.Enabled = False
        controller.load_entity_toolbar()
        If controller.container.mode = bc_am_cp_container.EDIT_LITE Then
            Me.DeleteEntityToolStripMenuItem.Visible = False
        End If
        If controller.container.mode = bc_am_cp_container.VIEW Then
            Me.Lentities.ContextMenuStrip = Nothing
        End If

        Me.Cursor = Cursors.Default
    End Sub
    Private Sub load_entity_real_time_search(entity As bc_om_entity)
        Me.baudit.Enabled = False
        Me.bentaudit.Enabled = False
        'If Me.Lentities.SelectedItems.Count = 0 Then
        '    Me.passociations.Enabled = False
        '    Me.tassociations.Enabled = False
        '    Me.ChangeNameToolStripMenuItem.Enabled = False
        '    Me.DeleteEntityToolStripMenuItem.Enabled = False
        '    Me.minactive.Enabled = False
        '    controller.load_entity_toolbar()
        '    Exit Sub
        'End If
        Me.baudit.Enabled = True
        Me.bentaudit.Enabled = True


        Me.Cursor = Cursors.WaitCursor
        Me.tassociations.TabPages(0).Text = "loading..."
        Me.tassociations.TabPages(0).ImageIndex = deactivate_icon

        clear_attribute_vals()


        Me.passociations.Enabled = True
        Me.tassociations.Enabled = True
        Me.new_entity = False
        controller.new_entity = False


        controller.load_entity_parts_real_time_search(entity, Me.associated_entities(Me.tassociations.SelectedIndex).class_name, True)

        If InStr(entity.name, "(inactive)") > 0 Then
            Me.DeleteEntityToolStripMenuItem.Enabled = True
            Me.ChangeNameToolStripMenuItem.Enabled = False
        Else
            Me.ChangeNameToolStripMenuItem.Enabled = True
            Me.DeleteEntityToolStripMenuItem.Enabled = False
        End If

        Me.bentchanges.Enabled = False
        Me.bentdiscard.Enabled = False
        controller.load_entity_toolbar()
        If controller.container.mode = bc_am_cp_container.EDIT_LITE Then
            Me.DeleteEntityToolStripMenuItem.Visible = False
        End If
        If controller.container.mode = bc_am_cp_container.VIEW Then
            Me.Lentities.ContextMenuStrip = Nothing
        End If

        Me.Cursor = Cursors.Default
    End Sub
    Private Sub massignentity_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles massignentity.Click
        Try
            controller.load_entity_assign(Me.tentities.SelectedNode.Text)
        Catch ex As Exception
            MsgBox(ex.Message)

        End Try
    End Sub

    Private Sub tentities_Click(sender As Object, e As EventArgs) Handles tentities.Click

    End Sub

    Private Sub tentities_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tentities.DoubleClick
        Try
            controller.navigate()
        Catch
            'controller.load_entity_assign(Me.tentities.SelectedNode.Text)
            controller.show_linked_audit(Me.tentities.SelectedNode.Text)
        End Try

    End Sub
    Private Sub mremoveentity_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Cursor = Cursors.WaitCursor
        controller.remove_association()
        Me.Cursor = Cursors.Default
    End Sub
    REM ING JUNE 2012
    Private Sub bentdiscard_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bentdiscard.Click


        Dim tx As String
        tx = Me.Lentities.SelectedItems(0).Text

        controller.discard_changes()

        tfilter.SearchRefresh(True)
        controller.container.oentities = bc_am_load_objects.obc_entities

        For i = 0 To Lentities.Items.Count - 1
            If Lentities.Items(i).Text = tx Then
                Lentities.Items(i).Selected = True
                Exit For
            End If
        Next
        REM ---------------

    End Sub
    REM EFG JUNE 2012
    Private Sub bentchanges_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bentchanges.Click

        Dim tx As String
        Me.Cursor = Cursors.WaitCursor
        tx = Me.Lentities.SelectedItems(0).Text

        If controller.commit_changes = True Then

            tfilter.SearchRefresh(True, False)
            controller.container.oentities = bc_am_load_objects.obc_entities
            REM do search attribute in new thread for performance
            'Dim t As New Thread(AddressOf dothetask)
            't.Start()
            'REM ING JUNE 2012
            'For i = 0 To Lentities.Items.Count - 1
            '    If Lentities.Items(i).Text = tx Then
            '        Lentities.Items(i).Selected = True
            '        Exit For
            '    End If
            'Next
        End If
        REM ---------------
        Me.Cursor = Cursors.Default
    End Sub
    'Sub dothetask()
    '    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
    '        bc_am_load_objects.obc_entities.search_attributes.db_read()
    '    Else

    '        bc_am_load_objects.obc_entities.search_attributes.tmode = bc_cs_soap_base_class.tREAD

    '        bc_am_load_objects.obc_entities.search_attributes.transmit_to_server_and_receive(bc_am_load_objects.obc_entities.search_attributes, True)
    '    End If



    'End Sub
    Private Sub minactive_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles minactive.Click
        controller.activate_entity(Me.Lentities.SelectedItems(0).Text, controller.get_real_class_name(Me.cclass.Text))
    End Sub

    Private Sub entityContextMenuStrip_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles entityContextMenuStrip.Opening

    End Sub



    Private Sub DataGridView1_CellValidating(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellValidatingEventArgs) Handles DataGridView1.CellValidating

        If Me.Lentities.SelectedItems.Count = 1 Then

            If InStr(Me.Lentities.SelectedItems(0).Text, "(inactive)") > 0 Then
                Exit Sub
            End If

            If Me.controller.container.mode = bc_am_cp_container.VIEW Then
                REM read only
                Exit Sub
            End If
            REM EFG June 2012 FIL June 2012 ING June 2012
            If e.ColumnIndex = 3 And Me.DataGridView1.RowCount > 0 And (Me.DataGridView1.Columns(3).Name = "Values" Or Me.DataGridView1.Columns(3).Name = "Value") Then
                If controller.validate_attribute_value(e.FormattedValue) = False Then
                    e.Cancel = True
                End If
            End If
        End If
    End Sub
    Private Sub DataGridView2_CellValidating(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellValidatingEventArgs) Handles DataGridView2.CellValidating

        If Me.Lentities.SelectedItems.Count = 1 Then

            If InStr(Me.Lentities.SelectedItems(0).Text, "(inactive)") > 0 Then
                Exit Sub
            End If

            If Me.controller.container.mode = bc_am_cp_container.VIEW Then
                REM read only
                Exit Sub
            End If
            REM EFG June 2012 FIL June 2012 ING June 2012
            If e.ColumnIndex = 3 And Me.DataGridView2.RowCount > 0 And (Me.DataGridView2.Columns(3).Name = "Values" Or Me.DataGridView2.Columns(3).Name = "Value") Then
                'PR 5.6
                If controller.validate_linked_attribute_value(e.FormattedValue) = False Then
                    e.Cancel = True
                End If
            End If
        End If
    End Sub


    Private Sub DataGridView1_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridView1.MouseLeave
        Try
            If DataGridView1.SelectedCells(0).ColumnIndex = 3 And Me.DataGridView1.RowCount > 0 Then
                DataGridView1.Item(0, DataGridView1.SelectedCells(0).RowIndex).Selected = True
            End If
        Catch

        End Try
    End Sub

    Private Sub DataGridView2_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridView2.MouseLeave
        Try
            If DataGridView2.SelectedCells(0).ColumnIndex = 3 And Me.DataGridView2.RowCount > 0 Then
                DataGridView2.Item(0, DataGridView1.SelectedCells(0).RowIndex).Selected = True
            End If
        Catch

        End Try
    End Sub

    Private Sub lvwvalidate_SelectedIndexChanged_2(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvwvalidate.SelectedIndexChanged

    End Sub

    Private Sub IncreaseRatingToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles IncreaseRatingToolStripMenuItem.Click
        controller.change_rating(True)
    End Sub

    Private Sub DecreaseRatingToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DecreaseRatingToolStripMenuItem.Click
        controller.change_rating(False)
    End Sub
    Private Sub Tprop_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Tprop.SelectedIndexChanged
        Me.lvwatt.Items.Clear()
        For i = 1 To Me.tattributelinks.TabPages.Count - 1
            Me.tattributelinks.TabPages.RemoveAt(1)
        Next

        If Tprop.SelectedIndex = 0 Then
            Me.prop_pc()
        Else
            Me.prop_cp()
        End If
    End Sub


    Private Sub DataGridView1_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub tentities_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tentities.MouseDown
        Try
            If e.Button = Windows.Forms.MouseButtons.Right Then
                'Me.set_assign_menu()
            End If


        Catch ex As Exception
            'Dim oerr As New bc_cs_error_log("tentities", "MouseDown", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Sub

    Private Sub uxAttributesToolStrip_ItemClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles uxAttributesToolStrip.ItemClicked

        Select Case Trim(e.ClickedItem.Text)
            Case ("Add Existing Attribute")
                controller.add_exisiting_attribute_menu()
            Case ("Add New Attribute")
                controller.add_new_attribute_menu()
            Case ("Modify Attribute")
                controller.modify_attribute_menu()
            Case ("Remove Attribute")

                controller.remove_attribute_from_class()
            Case ("Delete Attribute")
                controller.delete_attribute()
        End Select

    End Sub

    Private Sub BlueCurve_TextSearch1_FireSearch(ByVal sender As Object, ByVal e As System.EventArgs) Handles tfilter.FireSearch
        Me.clear_attribute_vals()
    End Sub

    Private Sub DataGridView1_RowsRemoved(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsRemovedEventArgs) Handles DataGridView1.RowsRemoved

    End Sub

    Private Sub DataGridView1_RowLeave(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.RowLeave

    End Sub

    Private Sub DataGridView2_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView2.CellContentClick

    End Sub

    Private Sub baudit_Click(sender As Object, e As EventArgs)
        controller.show_audit()

    End Sub

    Private Sub breadonly_Click(sender As Object, e As EventArgs) Handles breadonly.Click
        Me.Cursor = Cursors.WaitCursor
        controller.toggle_read_only()
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub baudit_Click_1(sender As Object, e As EventArgs) Handles baudit.Click
        controller.attribute_details(True)
    End Sub

    Private Sub bentaudit_Click(sender As Object, e As EventArgs) Handles bentaudit.Click
        controller.show_audit()
    End Sub

    Private Sub chkinactive_CheckedChanged(sender As Object, e As EventArgs) Handles chkinactive.CheckedChanged

        controller.new_load_entities(Me.chkinactive.Checked)
    End Sub

    Private Sub tfilter_Load(sender As Object, e As EventArgs) Handles tfilter.Load

    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboefilter.SelectedIndexChanged
        If (cboefilter.SelectedIndex <> -1) Then
            controller.select_filter_class(cboefilter.SelectedIndex)
        End If
    End Sub

End Class

