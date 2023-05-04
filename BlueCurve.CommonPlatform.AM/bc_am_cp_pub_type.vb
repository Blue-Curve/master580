Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports System.Drawing
Imports System.Windows.Forms
#Region "changes"
'Changes:
'Tracker                 Initials                   Date                      Synopsis
'FIL 6835                PR                         8/1/2014                  Added default support doc
#End Region

Public Class bc_am_cp_pub_type
    Public mode As Integer
    Public bload As Boolean = False
    Public user_attributes_values As New ArrayList
    Public warnings As New ArrayList
    Public new_pt As Boolean = False
    Public attribute_mode As Integer
    Public rdrag As Boolean = True
    Public ldrag As Boolean = True
    Public maintain_mode As Integer = 0
    Public ROUTE_COLOUR As Color = Color.LightSalmon
    Friend controller As Object
    Public from_Set_menu As Boolean = False
    'icons
    Private Const add_icon As Integer = 0
    Private Const edit_icon As Integer = 1
    Private Const delete_icon As Integer = 2

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
    Private Const publication_icon = 8
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
    Private Const folder_icon As Integer = 25
    Private Const new_icon As Integer = 26
    Private Const active_icon As Integer = 27

    Private Sub bc_am_cp_users_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.puserup.Visible = False
        Me.puserdn.Visible = False
    End Sub
    Private Sub tpubtypes_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tpubtypes.AfterSelect

        BlueCurve_TextSearch1.SearchClass = -1
        'tfilter.SearchSetup()

        Me.tpubtypename.TabPages(0).Text = "no selection"
        'Me.DataGridView1.Rows.Clear()
        Me.controller.load_pub_types()
    End Sub

    Private Sub DataGridView1_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellEnter

    End Sub
    Private Sub DataGridView1_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridView1.MouseLeave
        Try
            If DataGridView1.SelectedCells(0).ColumnIndex = 2 Then
                DataGridView1.Item(0, DataGridView1.SelectedCells(0).RowIndex).Selected = True
            End If
        Catch

        End Try
    End Sub
    Private Sub tfilter_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'controller.pt_mode(Me.tfilter.Text)
    End Sub
    Private Sub DataGridView1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridView1.Click
        If Me.DataGridView1.SelectedCells.Count = 0 Then
            Exit Sub
        End If

        If Me.DataGridView1.SelectedCells(0).ColumnIndex <> 3 Then
            Exit Sub
        End If
        Select Case Me.DataGridView1.SelectedCells(0).RowIndex
            Case 0
                controller.add_new_busarea_menu()
            Case 1
                controller.add_new_language_menu()
        End Select
    End Sub

    Private Sub lpubtypes_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Lpubtypes.SelectedIndexChanged

        If Me.Lpubtypes.SelectedItems.Count = 0 Then
            controller.clear_pub_type()
            controller.set_menu()
            Exit Sub

        End If
        controller.select_pubtype()
    End Sub



    Private Sub DeleteToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteToolStripMenuItem.Click
        controller.delete_item_menu()
    End Sub

    Private Sub DataGridView1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridView1.DoubleClick
        If Me.DataGridView1.SelectedCells(0).ColumnIndex <> 1 Then
            Exit Sub
        End If
        controller.attribute_details()
    End Sub

    Private Sub set_change()
        Me.bentchanges.Enabled = True
        Me.bentdiscard.Enabled = True
        Me.ppubtypes.Enabled = False
        Me.tpubtypes.Enabled = False
        Me.MdiParent.MainMenuStrip.Enabled = False
    End Sub


    Private Sub bentdiscard_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bentdiscard.Click
        Me.Cursor = Cursors.WaitCursor
        controller.discard_changes()
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub bentchanges_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bentchanges.Click
        Me.Cursor = Cursors.WaitCursor
        controller.commit_changes()
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub OKToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        controller.add_pub_type()
    End Sub


    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub pclass_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles pclass.Paint

    End Sub

    Private Sub minactive_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles minactive.Click
        controller.activate_pubtype()
    End Sub


    Private Sub DeleteEntityToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteEntityToolStripMenuItem.Click
        controller.del_pubtype()
    End Sub
    Private Sub DataGridView1_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub
    Private Sub mactive_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mactive.Click
        controller.activate_items()
    End Sub
    Private Sub OkToolStripMenuItem3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        controller.new_pubtype_name()
    End Sub



    Private Sub lvwvalidate_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvwvalidate.SelectedIndexChanged
        If Me.lvwvalidate.SelectedItems.Count > 0 Then

            Me.DataGridView1.Focus()
            Me.DataGridView1.Item(1, warnings(Me.lvwvalidate.SelectedItems(0).Index).attribute).Selected = True
        End If
    End Sub

    Private Sub OkToolStripMenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        controller.change_name()
    End Sub

    Friend Sub load_warnings()
        lvwvalidate.Items.Clear()
        REM ING JUNE 2012
        If warnings.Count = 0 Then
            lvwvalidate.Columns(0).Text = "Warnings"
        Else
            lvwvalidate.Columns(0).Text = CStr(warnings.Count) + " Warnings"
        End If
        REM --------------
        If warnings.Count > 0 Then
            Dim lwarn As ListViewItem
            For i = 0 To warnings.Count - 1
                lwarn = New ListViewItem(CStr(warnings(i).msg))
                'view.tpubtypenaview.TabPages(0).ImageIndex = 2
                lwarn.ImageIndex = warning_icon
                DataGridView1.Item(0, warnings(i).attribute).Value = tabimages.Images(warning_icon)
                lvwvalidate.Items.Add(lwarn)
            Next
        End If
    End Sub
    Public Sub show_attribute_details()
        attributedetails.Show(Cursor.Position.X, Cursor.Position.Y)
    End Sub

    Private Sub AddEntityToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddEntityToolStripMenuItem.Click
        controller.add_pub_type_menu()
    End Sub

    Private Sub mchname_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mchname.Click
        controller.change_pub_type_menu()
    End Sub
    Private Sub DataGridView1_CellValidating(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellValidatingEventArgs) Handles DataGridView1.CellValidating
        If controller.Container.mode > bc_am_cp_container.VIEW Then
            If e.ColumnIndex = 2 And e.FormattedValue.ToString <> "System.Drawing.Bitmap" Then
                If controller.validate_attribute_value(e.FormattedValue) = False Then
                    e.Cancel = True
                End If
            End If
        End If
    End Sub
    Private Sub DataGridView1_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridView1.SelectionChanged
        If controller.Container.mode = bc_am_cp_container.VIEW Then
            Exit Sub
        End If

        'If Me.mode = 0 Then
        REM read only
        'Exit Sub
        'End If
        'controller.load_entity_toolbar()
        If Me.DataGridView1.SelectedCells.Count = 0 Then
            Exit Sub
        End If

        If Me.DataGridView1.SelectedCells(0).ColumnIndex = 2 Then
            If Me.DataGridView1.Columns(2).ReadOnly = False Then
                controller.set_data_type_control()
            Else
                Exit Sub
            End If
        End If
        controller.load_pubtype_toolbar()
    End Sub

    Private Sub ChangeNameToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChangeNameToolStripMenuItem.Click
        controller.change_item_menu()
    End Sub
    Public Sub load_all()
        Try

            Me.workflow.Nodes.Clear()
            clear_menu()


            'controller.opt.db_read()
            populate_tree(controller.opt)
            clear_background_colours(Me.workflow.Nodes(0))
            'Me.workflow.ExpandAll()
         Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_cp_pub_type", "load_all", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub

    Public Sub populate_tree(ByRef opt As bc_om_pub_type_workflow)
        Try

            Me.workflow.Nodes.Clear()
            'Me.Workflow.Nodes.Add("Workflow (Green current stage; Orange stage can move to)")
            'Me.Workflow.Nodes(0).ForeColor = System.Drawing.Color.Black
            With controller.opt.nstage
                If .current_stage_name = "" Then
                    .current_stage_name = "Draft"
                End If
                Me.workflow.Nodes.Add(CStr(.current_stage_name), CStr(.current_stage_name), 4)
                add_node(Me.workflow.Nodes(0), .routes)
            End With

        Catch ex As Exception
            MsgBox(ex.Message)

        End Try
    End Sub
    Public Sub add_node(ByVal start_node As TreeNode, ByRef lstage As List(Of bc_om_pub_type_workflow_stage))
        Dim i As Integer
        Dim ncount As Integer
        ncount = 0
        Try

            For i = 0 To lstage.Count - 1
                Dim ax As String = ""
                Dim sfrom As String
                sfrom = start_node.Text
                If InStr(sfrom, "[") > 0 Then
                    sfrom = sfrom.Substring(0, InStr(sfrom, "[") - 1)
                End If

                start_node.Nodes.Add("Actions", "actions  " + sfrom + " to " + lstage(i).current_stage_name, 3)
                start_node.Nodes(start_node.Nodes.Count - 1).ForeColor = Color.DarkGray

                ncount = ncount + 1
                For j = 0 To lstage(i).actions.Count - 1
                    For k = 0 To Me.controller.container.opubtypes.actions.actions.Count - 1
                        If Me.controller.container.opubtypes.actions.actions(k).id = lstage(i).actions(j) Then
                            start_node.Nodes(start_node.Nodes.Count - 1).Nodes.Add("  " + Me.controller.container.opubtypes.actions.actions(k).name, "  " + Me.controller.container.opubtypes.actions.actions(k).name, 1)
                            start_node.Nodes(start_node.Nodes.Count - 1).Nodes(start_node.Nodes(start_node.Nodes.Count - 1).Nodes.Count - 1).ForeColor = Color.Gray
                            Exit For
                            'ncount = ncount + 1
                        End If
                    Next
                Next

                If ax <> "" Then
                    ax = "         [" + ax + "]"
                End If
                'start_node.nodes.Add(CStr(lstage(i).actions(j)), CStr(lstage(i).actions(j)), 1)
                'Next

                If lstage(i).approvers > 0 Then
                    Dim role As String = ""
                    If lstage(i).approval_role > 0 Then
                        For j = 0 To Me.controller.container.oadmin.roles.count - 1
                            If Me.controller.container.oadmin.roles(j).id = lstage(i).approval_role Then
                                role = Me.controller.container.oadmin.roles(j).description
                                Exit For
                            End If
                        Next
                    End If

                    If lstage(i).author_approval = -1 And lstage(i).approval_role = -1 Then
                        start_node.Nodes.Add(CStr(lstage(i).current_stage_name), CStr(lstage(i).current_stage_name) + " [" + CStr(lstage(i).approvers) + " approvers]", 0)
                    ElseIf lstage(i).author_approval <> 1 And lstage(i).approval_role = -1 Then
                        start_node.Nodes.Add(CStr(lstage(i).current_stage_name), CStr(lstage(i).current_stage_name) + " [" + CStr(lstage(i).approvers) + " approvers allow author]", 0)
                    ElseIf lstage(i).author_approval = 1 And lstage(i).approval_role = -1 Then
                        start_node.Nodes.Add(CStr(lstage(i).current_stage_name), CStr(lstage(i).current_stage_name) + " [" + CStr(lstage(i).approvers) + " approvers disallow author]", 0)
                    ElseIf lstage(i).author_approval <> 1 And lstage(i).approval_role > -1 Then
                        start_node.Nodes.Add(CStr(lstage(i).current_stage_name), CStr(lstage(i).current_stage_name) + " [" + CStr(lstage(i).approvers) + " approvers allow author for role " + role + " ]", 0)
                    ElseIf lstage(i).author_approval = 1 And lstage(i).approval_role > -1 Then
                        start_node.Nodes.Add(CStr(lstage(i).current_stage_name), CStr(lstage(i).current_stage_name) + " [" + CStr(lstage(i).approvers) + " approvers disallow author for role " + role + " ]", 0)
                    End If
                Else
                    start_node.Nodes.Add(CStr(lstage(i).current_stage_name), CStr(lstage(i).current_stage_name), 0)
                End If

                start_node.Nodes(start_node.Nodes.Count - 1).ForeColor = Color.Black
                start_node.Expand()
                'start_node.nodes(ncount).forecolor = System.Drawing.Color.Gray
                add_node(start_node.Nodes(ncount), lstage(i).routes)

                ncount = ncount + 1
                'End If
            Next
        Catch ex As Exception

        End Try
    End Sub



    Private Sub workflow_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.DoubleClick


        Dim tx As New ArrayList
        load_all()
        Exit Sub
        load_process(tx)
        Me.workflow.Nodes.Clear()

        For i = 0 To tx.Count - 1
            'MsgBox((tx(i)))
            Me.workflow.Nodes.Add(tx(i), tx(i), 2)
        Next
    End Sub

    Private Sub load_process(ByRef tx As ArrayList)
        Try
            Dim str As String
            Dim i As Integer
            If Me.controller.container.mode = 0 Then
                Me.workflow.ContextMenuStrip = Nothing
            Else
                Me.workflow.ContextMenuStrip = Me.maddaction

            End If
            For i = 0 To Me.workflow.Nodes.Count - 1
                str = Me.workflow.Nodes(i).Text
                tx.Add(str)
                'MsgBox(tx(tx.Count - 1))
                parse_node(Me.workflow.Nodes(i), tx)
            Next
            While i < Me.workflow.Nodes.Count
                Me.workflow.Nodes(i).Nodes.Clear()
            End While
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_cp_pub_type", "load_process", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Private Sub parse_node(ByRef node As Object, ByRef tx As ArrayList)
        Dim str As String

        For i = 0 To node.Nodes.Count - 1
            str = node.Nodes(i).Text
            tx.Add(str)
            parse_node(node.Nodes(i), tx)
            'MsgBox(tx(tx.Count - 1))
        Next
        node.nodes.clear()
    End Sub



    Private Sub workflow_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles workflow.AfterSelect

        clear_background_colours(Me.workflow.Nodes(0))

        If Me.workflow.SelectedNode.ImageIndex = 0 Or Me.workflow.SelectedNode.ImageIndex = 4 Or Me.workflow.SelectedNode.ImageIndex = 8 Then
            Dim found As Boolean = False
            'If Me.workflow.SelectedNode.Nodes.Count = 0 Then
            '    Dim sfrom As String
            '    sfrom = Me.workflow.SelectedNode.Text
            '    If InStr(sfrom, "[") > 0 Then
            '        sfrom = sfrom.Substring(0, InStr(sfrom, "[") - 1)
            '    End If
            '    If found = False Then
            '        set_node_first_instance(sfrom, Me.workflow.Nodes(0), found)
            '    End If

            '    Exit Sub
            'End If
            Try
                Me.workflow.SelectedNode.Parent.BackColor = Color.LightGreen

            Catch
            End Try

            For i = 0 To Me.workflow.SelectedNode.Nodes.Count - 1
                If Me.workflow.SelectedNode.Nodes(i).ImageIndex = 0 Or Me.workflow.SelectedNode.Nodes(i).ImageIndex = 4 Or Me.workflow.SelectedNode.Nodes(i).ImageIndex = 8 Then
                    Me.workflow.SelectedNode.Nodes(i).BackColor = ROUTE_COLOUR
                End If
            Next
            set_new_stages()
        End If
        set_menu()
    End Sub
    Private Sub set_new_stages()
        Try
            Me.mcstages.Items.Clear()
            Dim sfrom, sto As String
            sfrom = Me.workflow.SelectedNode.Text
            If InStr(sfrom, "[") > 0 Then
                sfrom = sfrom.Substring(0, InStr(sfrom, "[") - 2)
            End If
            Dim found As Boolean
            For i = 0 To Me.controller.container.oPubtypes.stages.Count - 1
                If Me.controller.container.oPubtypes.stages(i) <> sfrom Then
                    found = False
                    For j = 0 To Me.workflow.SelectedNode.Nodes.Count - 1
                        If Me.workflow.SelectedNode.Nodes(j).ImageIndex = 0 Or Me.workflow.SelectedNode.Nodes(j).ImageIndex = 4 Or Me.workflow.SelectedNode.Nodes(j).ImageIndex = 8 Then
                            sto = Me.workflow.SelectedNode.Nodes(j).Text
                            If InStr(sto, "[") > 0 Then
                                sto = sto.Substring(0, InStr(sto, "[") - 2)
                            End If
                            If Me.controller.container.oPubtypes.stages(i) = sto Then
                                found = True
                                Exit For
                            End If
                        End If
                    Next
                    If found = False Then
                        Me.mcstages.Items.Add(Me.controller.container.oPubtypes.stages(i))
                    End If
                End If
            Next
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_cp_pup_type", "set_new_stages", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try

    End Sub
    REM EFG June 2012
    Private Sub set_node_first_instance(ByVal stage As String, ByVal node As TreeNode, ByRef found As Boolean)
        If found = True Then
            Exit Sub
        End If
        If node.Text = stage Then
            Me.workflow.SelectedNode = node
            found = True
            Exit Sub
        End If
        For i = 0 To node.Nodes.Count - 1
            'If InStr(node.Nodes(i).Text, stage) > 0 And (node.Nodes(i).ImageIndex = 4 Or node.Nodes(i).ImageIndex = 0 Or node.Nodes(i).ImageIndex = 8) And node.Nodes(i).Nodes.Count > 0 Then
            If node.Nodes(i).Text = stage And (node.Nodes(i).ImageIndex = 4 Or node.Nodes(i).ImageIndex = 0 Or node.Nodes(i).ImageIndex = 8) And node.Nodes(i).Nodes.Count > 0 Then
                Me.workflow.SelectedNode = node.Nodes(i)
                found = True
                Exit Sub
            Else
                set_node_first_instance(stage, node.Nodes(i), found)
            End If
        Next

    End Sub
    Private Function is_end_node_cyclical(ByVal stage As String, ByVal node As TreeNode) As Boolean
        is_end_node_cyclical = False

        If node.Text = stage Then
            If node.Nodes.Count > 0 Then
                is_end_node_cyclical = True
                Exit Function
            End If
        End If
        For i = 0 To node.Nodes.Count - 1
            If InStr(node.Nodes(i).Text, stage) > 0 And (node.Nodes(i).ImageIndex = 4 Or node.Nodes(i).ImageIndex = 0 Or node.Nodes(i).ImageIndex = 8) Then
                If node.Nodes(i).Nodes.Count > 0 Then
                    is_end_node_cyclical = True
                    Exit Function
                End If
            Else
                is_end_node_cyclical = is_end_node_cyclical(stage, node.Nodes(i))
                If is_end_node_cyclical = True Then
                    Exit Function
                End If
            End If
        Next

    End Function
    Private Sub clear_background_colours(ByVal node As TreeNode)
        'If node.ImageIndex = 0 Or node.ImageIndex = 4 Then
        node.BackColor = Color.White
        'End If
        For i = 0 To node.Nodes.Count - 1
            node.Nodes(i).BackColor = Color.White
            If node.Nodes(i).ImageIndex = 0 Or node.Nodes(i).ImageIndex = 4 Or node.Nodes(i).ImageIndex = 8 Then
                If node.Nodes(i).Nodes.Count = 0 Then
                    node.Nodes(i).ImageIndex = 4
                    Dim found As Boolean = False
                    Dim sfrom As String
                    sfrom = node.Nodes(i).Text
                    If InStr(sfrom, "[") > 0 Then
                        sfrom = sfrom.Substring(0, InStr(sfrom, "[") - 2)
                    End If
                    If is_end_node_cyclical(sfrom, Me.workflow.Nodes(0)) = True Then
                        node.Nodes(i).ImageIndex = 8
                    End If
                End If
            End If
            clear_background_colours(node.Nodes(i))
        Next
    End Sub
    Private Sub clear_menu()


        Me.mactionadd.Visible = False
        Me.mactiondown.Visible = False
        Me.mactionup.Visible = False
        Me.mdelaction.Visible = False
        Me.maddroute.Visible = False
        Me.mremroute.Visible = False
        Me.mapprovers.Visible = False
        Me.mapp1.Visible = False
        Me.mapp2.Visible = False

        Me.mcstages.Visible = False
        Me.mcaction.Visible = False
        Me.mcapprovers.Visible = False
        Me.mexceditor.Visible = False
        Me.mrole.Visible = False
        Me.ToolStripSeparator1.Visible = False
        Me.ToolStripSeparator3.Visible = False



    End Sub

    Private Sub set_menu()
        Try

            If controller.container.mode = 0 Then
                Me.workflow.ContextMenuStrip = Nothing
                Exit Sub
            End If
            clear_menu()
            Try
                Dim stx As String
                stx = Me.workflow.SelectedNode.Text
            Catch ex As Exception
                Exit Sub
            End Try


            Me.maddroute.Image = Me.processadminimages.Images(0)
            Me.mactionadd.Image = Me.processadminimages.Images(1)
            Me.mremroute.Image = Me.processadminimages.Images(5)
            Me.mdelaction.Image = Me.processadminimages.Images(5)
            Me.mactionup.Image = Me.processadminimages.Images(6)
            Me.mactiondown.Image = Me.processadminimages.Images(7)
            Me.mremproc.Image = Me.processadminimages.Images(5)
            Me.mapprovers.Image = Me.processadminimages.Images(2)
            Me.mcopyproc.Image = Me.processadminimages.Images(0)

            Me.mremproc.Visible = False
            Me.mcopyproc.Visible = False
            Me.mccopyproc.Visible = False
            Me.ToolStripSeparator1.Visible = False
            Me.ToolStripSeparator2.Visible = False
            Me.ToolStripSeparator3.Visible = False
            Me.ToolStripSeparator4.Visible = False

            Me.mcstages.Visible = False
            Me.mcaction.Visible = False
            Me.mcapprovers.Visible = False
            Me.mexceditor.Visible = False
            Me.mrole.Visible = False
            Me.mactionadd.Text = ""
            Me.mactiondown.Text = ""
            Me.mactionup.Text = ""
            Me.mdelaction.Text = ""
            Me.maddroute.Text = ""
            Me.mremroute.Text = ""
            Me.mcstages.Text = ""
            Dim sfrom, sto As String
            Dim tapp As String
            Dim app As String
            Dim aa As Integer = -1
            Dim ar As String = ""

            app = 0
            from_Set_menu = True

            For i = 0 To 10
                Me.mcapprovers.Items.Add(CStr(i))
                Me.mcapprovers.SelectedIndex = 0
            Next
            Me.mccopyproc.Items.Clear()
            For i = 0 To Me.controller.container.opubtypes.pubtype.count - 1
                Me.mccopyproc.Items.Add(Me.controller.container.opubtypes.pubtype(i).name)
            Next
            If Me.workflow.SelectedNode.ImageIndex = 0 Or Me.workflow.SelectedNode.ImageIndex = 4 Or Me.workflow.SelectedNode.ImageIndex = 8 Then
                Try
                    sfrom = Me.workflow.SelectedNode.Parent.Text
                    If InStr(sfrom, "[") > 0 Then
                        sfrom = sfrom.Substring(0, InStr(sfrom, "[") - 2)
                    End If
                    sto = Me.workflow.SelectedNode.Text

                    Me.mrole.Items.Clear()
                    Me.mrole.Items.Add("All")
                    For i = 0 To Me.controller.container.oadmin.roles.count - 1
                        Me.mrole.Items.Add(Me.controller.container.oadmin.roles(i).description)
                    Next
                    Me.mrole.SelectedIndex = 0
                    Me.mexceditor.SelectedIndex = 0

                    If InStr(sto, "[") > 0 Then
                        sto = sto.Substring(0, InStr(sto, "[") - 2)
                        app = Me.workflow.SelectedNode.Text
                        tapp = app.Substring(InStr(app, "["), Len(app) - InStr(app, "["))

                        app = tapp.Substring(0, InStr(tapp, "approvers") - 2)
                        Me.mexceditor.SelectedIndex = 0
                        If InStr(tapp, "disallow author") > 0 Then
                            Me.mexceditor.SelectedIndex = 1
                        End If
                        tapp = tapp.Replace("allow author", "")
                        tapp = tapp.Replace("disallow author", "")

                        For i = 0 To Me.controller.container.oadmin.roles.count - 1
                            If InStr(tapp, Me.controller.container.oadmin.roles(i).description) > 0 Then
                                Me.mrole.Text = Me.controller.container.oadmin.roles(i).description
                                Exit For
                            End If
                        Next


                        'app = app.Substring(InStr(app, "["), Len(app) - 11 - InStr(app, "["))
                        'app = app.Substring(InStr(app, "["), Len(app) - 11 - InStr(app, "["))

                    End If

                    Me.mremroute.Text = "Remove route " + sfrom + " to " + sto
                    Me.maddroute.Visible = True
                    Me.maddroute.Text = "Add a route from " + sto
                    Me.mremroute.Visible = True
                    Me.mapprovers.Text = "Approvers from " + sfrom + " to " + sto
                    Me.mapprovers.Visible = True
                    Me.mapp1.Visible = False
                    Me.mapp2.Visible = False

                    Me.mcstages.Visible = True
                    Me.mcapprovers.Visible = True



                    Me.ToolStripSeparator1.Visible = True
                    Me.ToolStripSeparator3.Visible = True
                    Me.mcapprovers.Text = app

                Catch
                    sto = Me.workflow.SelectedNode.Text
                    If InStr(sto, "[") > 0 Then
                        sto = sto.Substring(0, InStr(sto, "[") - 1)
                    End If
                    REM top node
                    Me.maddroute.Visible = True
                    Me.maddroute.Text = "Add a route from " + sto
                    Me.mcstages.Visible = True
                    Me.mremproc.Visible = True
                    Me.mcopyproc.Visible = True
                    Me.mccopyproc.Visible = True
                    Me.ToolStripSeparator1.Visible = True
                    Me.ToolStripSeparator4.Visible = True

                End Try
            End If
            If Me.workflow.SelectedNode.ImageIndex = 3 Then
                Me.mactionadd.Visible = True
                sfrom = Me.workflow.SelectedNode.Parent.Text
                If InStr(sfrom, "[") > 0 Then
                    sfrom = sfrom.Substring(0, InStr(sfrom, "[") - 2)
                End If
                sto = Me.workflow.SelectedNode.NextNode.Text

                If InStr(sto, "[") > 0 Then
                    sto = sto.Substring(0, InStr(sto, "[") - 2)
                End If
                Me.mactionadd.Text = "Add action for route " + sfrom + " to " + sto
                Me.mcaction.Visible = True
                Me.mcaction.Items.Clear()
                Dim afound As Boolean = False
                For i = 0 To Me.controller.container.opubtypes.actions.actions.count - 1
                    afound = False
                    For j = 0 To Me.workflow.SelectedNode.Nodes.Count - 1
                        If Trim(Me.workflow.SelectedNode.Nodes(j).Text) = Me.controller.container.opubtypes.actions.actions(i).name Then
                            afound = True
                            Exit For
                        End If
                    Next
                    If afound = False Then
                        Me.mcaction.Items.Add(Me.controller.container.opubtypes.actions.actions(i).name)
                    End If
                Next
                Me.mcaction.SelectedIndex = -1
            End If
            If Me.workflow.SelectedNode.ImageIndex = 1 Then
                sfrom = Me.workflow.SelectedNode.Parent.Parent.Text
                If InStr(sfrom, "[") > 0 Then
                    sfrom = sfrom.Substring(0, InStr(sfrom, "[") - 1)
                End If
                sto = Me.workflow.SelectedNode.Parent.NextNode.Text

                If InStr(sto, "[") > 0 Then
                    sto = sto.Substring(0, InStr(sto, "[") - 1)
                End If
                Me.mdelaction.Visible = True
                Me.mdelaction.Text = "Remove action" + Me.workflow.SelectedNode.Text + " from route " + sfrom + " to " + sto
                Try
                    Dim t As String
                    t = Me.workflow.SelectedNode.PrevNode.Text
                    Me.mactionup.Text = "Move action" + Me.workflow.SelectedNode.Text + " higher in sequence"
                    Me.mactionup.Visible = True
                Catch

                End Try
                Try
                    Dim t As String
                    t = Me.workflow.SelectedNode.NextNode.Text
                    Me.mactiondown.Text = "Move action" + Me.workflow.SelectedNode.Text + " lower in sequence"
                    Me.mactiondown.Visible = True
                Catch

                End Try


            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_cp_pup_type", "set_menu", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            from_Set_menu = False

        End Try

    End Sub

    Private Sub workflow_DoubleClick1(ByVal sender As Object, ByVal e As System.EventArgs) Handles workflow.DoubleClick
        Try
            Dim i As Integer
            i = workflow.SelectedNode.ImageIndex
            find_original_stage()
        Catch ex As Exception

        End Try
    End Sub
    Private Sub find_original_stage()

        If Me.workflow.SelectedNode.ImageIndex = 0 Or Me.workflow.SelectedNode.ImageIndex = 4 Or Me.workflow.SelectedNode.ImageIndex = 8 Then
            Dim found As Boolean = False
            If Me.workflow.SelectedNode.Nodes.Count = 0 Then
                Dim sfrom As String
                sfrom = Me.workflow.SelectedNode.Text
                If InStr(sfrom, "[") > 0 Then
                    sfrom = sfrom.Substring(0, InStr(sfrom, "[") - 2)
                End If
                set_node_first_instance(sfrom, Me.workflow.Nodes(0), found)
            End If
            If found = False Then
                Exit Sub
            End If
        End If

        clear_background_colours(Me.workflow.Nodes(0))
        Try
            Me.workflow.SelectedNode.Parent.BackColor = Color.LightGreen
        Catch
        End Try
        For i = 0 To Me.workflow.SelectedNode.Nodes.Count - 1
            If Me.workflow.SelectedNode.Nodes(i).ImageIndex = 0 Or Me.workflow.SelectedNode.Nodes(i).ImageIndex = 4 Or Me.workflow.SelectedNode.Nodes(i).ImageIndex = 8 Then
                Me.workflow.SelectedNode.Nodes(i).BackColor = ROUTE_COLOUR
            End If
        Next

        set_menu()
    End Sub
    Private Sub add_route()
        Dim sfrom, sstage As String
        Dim omsg As bc_cs_message
        sfrom = Me.workflow.SelectedNode.Text
        If InStr(sfrom, "[") > 0 Then
            sfrom = sfrom.Substring(0, InStr(sfrom, "[") - 2)
        End If
        Dim new_stage As String
        new_stage = Me.mcstages.Text

        REM cant add stage route to itself
        If UCase(new_stage) = UCase(sfrom) Then
            omsg = New bc_cs_message("Blue Curve", "Cannot Add route to itself!", bc_cs_message.MESSAGE, False, False, "Yes", "no", True)
            Exit Sub
        End If
        REM cant add route if route already exixts
        Me.find_original_stage()
        For i = 0 To Me.workflow.SelectedNode.Nodes.Count - 1
            sstage = Me.workflow.SelectedNode.Nodes(i).Text

            If InStr(sstage, "[") > 0 Then
                sstage = sstage.Substring(0, InStr(sstage, "[") - 2)
            End If
            If UCase(sstage) = UCase(new_stage) Then
                omsg = New bc_cs_message("Blue Curve", "Route: " + sfrom + " to " + new_stage + " already exists!", bc_cs_message.MESSAGE, False, False, "Yes", "no", True)
                Exit Sub
            End If
        Next

        REM add it to appropriate stage
        Me.workflow.ContextMenuStrip.Visible = False
        add_route_to_wf(controller.opt.nstage, new_stage, sfrom, False)
        add_route_to_ui(Me.workflow.Nodes(0), new_stage, sfrom, False)

        Me.controller.edit_mode()

        REM
        'Me.load_all()
    End Sub
    REM add new route to first node of prent stage
    Private Sub add_route_to_ui(ByVal node As TreeNode, ByVal new_stage As String, ByVal stage_from As String, ByRef found As Boolean)
        If found = True Then
            Exit Sub
        End If
        Dim tx As String
        tx = node.Text
        If InStr(tx, "[") > 0 Then
            tx = tx.Substring(0, InStr(tx, "[") - 2)
        End If

        If tx = stage_from Then
            node.Nodes.Add("actions", "actions " + stage_from + " to " + new_stage, 3)
            node.Nodes(node.Nodes.Count - 1).ForeColor = Color.Gray
            If is_end_node_cyclical(new_stage, Me.workflow.Nodes(0)) = True Then
                node.Nodes.Add(new_stage, new_stage, 8)
            Else
                node.Nodes.Add(new_stage, new_stage, 4)
            End If
            node.Nodes(node.Nodes.Count - 1).BackColor = ROUTE_COLOUR
            Me.workflow.SelectedNode.Expand()
            found = True
        Else
            For i = 0 To node.Nodes.Count - 1
                add_route_to_ui(node.Nodes(i), new_stage, stage_from, False)
            Next
        End If

    End Sub




    Private Sub add_route_to_wf(ByVal stage As bc_om_pub_type_workflow_stage, ByVal new_stage As String, ByVal stage_from As String, ByRef found As Boolean)
        If found = True Then
            Exit Sub
        End If
        If Trim(stage.current_stage_name) = Trim(stage_from) Then
            Dim ows As New bc_om_pub_type_workflow_stage
            ows.current_stage_name = new_stage
            If IsNothing(stage.routes) = True Then
                stage.routes = New List(Of bc_om_pub_type_workflow_stage)
            End If

            stage.routes.Add(ows)
            found = True
        Else
            If IsNothing(stage.routes) = False Then
                For i = 0 To stage.routes.Count - 1
                    add_route_to_wf(stage.routes(i), new_stage, stage_from, found)
                Next
            End If
        End If
    End Sub
    Private Sub set_approvers()
        Dim sfrom, sto As String
        sto = Me.workflow.SelectedNode.Text
        If InStr(sto, "[") > 0 Then
            sto = sto.Substring(0, InStr(sto, "[") - 2)
        End If
        sfrom = Me.workflow.SelectedNode.Parent.Text
        If InStr(sfrom, "[") > 0 Then
            sfrom = sfrom.Substring(0, InStr(sfrom, "[") - 2)
        End If

        REM add it to appropriate stage
        Dim approvers As String
        approvers = Me.mcapprovers.Text
        set_approvers_to_wf(controller.opt.nstage, sfrom, sto, approvers, Me.mexceditor.SelectedIndex, Me.mrole.Text, False)
        If approvers = 0 Then
            Me.workflow.SelectedNode.Text = sto
        Else
            If Me.mexceditor.SelectedIndex = -1 And Me.mrole.SelectedIndex < 1 Then
                Me.workflow.SelectedNode.Text = sto + " [" + CStr(approvers) + " approvers]"
            ElseIf Me.mexceditor.SelectedIndex = 0 And Me.mrole.SelectedIndex < 1 Then
                Me.workflow.SelectedNode.Text = sto + " [" + CStr(approvers) + " approvers allow author]"
            ElseIf Me.mexceditor.SelectedIndex = 1 And Me.mrole.SelectedIndex < 1 Then
                Me.workflow.SelectedNode.Text = sto + " [" + CStr(approvers) + " approvers disallow author]"
            ElseIf Me.mexceditor.SelectedIndex = 0 And Me.mrole.SelectedIndex > 0 Then
                Me.workflow.SelectedNode.Text = sto + " [" + CStr(approvers) + " approvers allow author for role " + Me.mrole.Text + " ]"
            ElseIf Me.mexceditor.SelectedIndex = 1 And Me.mrole.SelectedIndex > 0 Then
                Me.workflow.SelectedNode.Text = sto + " [" + CStr(approvers) + " approvers disallow author for role " + Me.mrole.Text + " ]"
            End If

        End If
        Me.workflow.ContextMenuStrip.Visible = False
        Me.controller.edit_mode()

        'Me.load_all()
    End Sub
    Private Sub rem_route()
        Dim sfrom, sto As String
        sto = Me.workflow.SelectedNode.Text
        If InStr(sto, "[") > 0 Then
            sto = sto.Substring(0, InStr(sto, "[") - 2)
        End If
        sfrom = Me.workflow.SelectedNode.Parent.Text
        If InStr(sfrom, "[") > 0 Then
            sfrom = sfrom.Substring(0, InStr(sfrom, "[") - 2)
        End If

        Dim rest_route As New bc_om_pub_type_workflow_stage
        REM add it to appropriate stage
        del_route_from_wf(controller.opt.nstage, sfrom, sto, rest_route, False)

        REM now add rest of propogation to next copy of current stage if it exists
        If Not IsNothing(rest_route.routes) Then
            Dim found As Boolean = False
            prop_rest_route(rest_route, controller.opt.nstage.routes, found)
        End If
        Me.load_all()
        Me.controller.edit_mode()

    End Sub
    Private Sub prop_rest_route(ByVal rest_route As bc_om_pub_type_workflow_stage, ByVal lstage As List(Of bc_om_pub_type_workflow_stage), ByRef found As Boolean)
        If found = True Then
            Exit Sub
        End If
        add_rest_route(rest_route, controller.opt.nstage.routes, found)
        If Not IsNothing(rest_route.routes) Then
            If found = False Then
                For i = 0 To rest_route.routes.Count - 1
                    prop_rest_route(rest_route.routes(i), controller.opt.nstage.routes, found)
                    If found = True Then
                        Exit Sub
                    End If
                Next
            End If
        End If
    End Sub
    Private Sub add_rest_route(ByVal rest_route As bc_om_pub_type_workflow_stage, ByVal lstage As List(Of bc_om_pub_type_workflow_stage), ByRef found As Boolean)
        If found = True Then
            Exit Sub
        End If
        If Not IsNothing(lstage) Then
            For i = 0 To lstage.Count - 1
                If lstage(i).current_stage_name = rest_route.current_stage_name Then
                    found = True
                    lstage(i).routes = rest_route.routes
                Else
                    add_rest_route(rest_route, lstage(i).routes, found)
                End If
            Next
        End If
    End Sub

    Private Sub del_route_from_wf(ByVal stage As bc_om_pub_type_workflow_stage, ByVal stage_from As String, ByVal stage_to As String, ByRef rest_route As bc_om_pub_type_workflow_stage, ByRef found As Boolean)
        Try
            If found = True Then
                Exit Sub
            End If
            If Not IsNothing(stage.routes) Then
                For i = 0 To stage.routes.Count - 1
                    If stage.routes(i).current_stage_name = stage_to And stage.current_stage_name = stage_from Then
                        REM need to assign routing if same stage exists elsewhere to next node
                        REM TBD
                        rest_route = stage.routes(i)
                        stage.routes.RemoveAt(i)
                        found = True
                        Exit Sub
                    Else
                        del_route_from_wf(stage.routes(i), stage_from, stage_to, rest_route, found)
                    End If
                Next
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_cp_pub_type", "del_route_from_wf", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub


    Private Sub set_approvers_to_wf(ByVal stage As bc_om_pub_type_workflow_stage, ByVal stage_from As String, ByVal stage_to As String, ByVal approvers As Integer, ByVal author_approval As Integer, ByVal approval_role As String, ByRef found As Boolean)
        If found = True Then
            Exit Sub
        End If
        If Not IsNothing(stage.routes) Then
            For i = 0 To stage.routes.Count - 1

                If stage.routes(i).current_stage_name = stage_to And stage.current_stage_name = stage_from Then
                    REM need to assign routing if same stage exists elsewhere to next node
                    REM TBD
                    stage.routes(i).approvers = approvers
                    If author_approval = -1 Then
                        author_approval = 0
                    End If
                    stage.routes(i).author_approval = author_approval
                    stage.routes(i).approval_role = -1
                    For j = 0 To Me.controller.container.oadmin.roles.count - 1
                        If Me.controller.container.oadmin.roles(j).description = approval_role Then
                            stage.routes(i).approval_role = Me.controller.container.oadmin.roles(j).id
                            Exit For
                        End If
                    Next
                    found = True
                    Exit Sub
                Else
                    set_approvers_to_wf(stage.routes(i), stage_from, stage_to, approvers, author_approval, approval_role, found)
                End If
            Next
        End If

    End Sub

    Private Sub add_action()
        Dim sfrom, sto As String
        sto = Me.workflow.SelectedNode.NextNode.Text
        If InStr(sto, "[") > 0 Then
            sto = sto.Substring(0, InStr(sto, "[") - 2)
        End If
        sfrom = Me.workflow.SelectedNode.Parent.Text
        If InStr(sfrom, "[") > 0 Then
            sfrom = sfrom.Substring(0, InStr(sfrom, "[") - 2)
        End If

        REM add it to appropriate stage
        Dim action_id As Integer = 1
        For i = 0 To controller.container.oPubtypes.actions.actions.Count - 1
            If controller.container.oPubtypes.actions.actions(i).name = Me.mcaction.Text Then
                add_action_to_wf(controller.opt.nstage, sfrom, sto, controller.container.oPubtypes.actions.actions(i).id, False)
                Me.workflow.SelectedNode.Nodes.Add("  " + controller.container.oPubtypes.actions.actions(i).name, "  " + controller.container.oPubtypes.actions.actions(i).name, 1)
                Me.workflow.SelectedNode.Nodes(Me.workflow.SelectedNode.Nodes.Count - 1).ForeColor = Color.Gray
            End If
        Next
        Me.mcaction.SelectedIndex = -1
        Me.workflow.ContextMenuStrip.Visible = False

        Dim afound As Boolean
        Me.workflow.SelectedNode.Expand()
        Me.mcaction.Items.Clear()
        For i = 0 To Me.controller.container.opubtypes.actions.actions.count - 1
            afound = False
            For j = 0 To Me.workflow.SelectedNode.Nodes.Count - 1
                If Trim(Me.workflow.SelectedNode.Nodes(j).Text) = Me.controller.container.opubtypes.actions.actions(i).name Then
                    afound = True
                    Exit For
                End If
            Next
            If afound = False Then
                Me.mcaction.Items.Add(Me.controller.container.opubtypes.actions.actions(i).name)
            End If
        Next
        Me.controller.edit_mode()

        'Me.load_all()
    End Sub
    Private Sub remove_action()
        Dim sfrom, sto As String
        sto = Me.workflow.SelectedNode.Parent.NextNode.Text
        If InStr(sto, "[") > 0 Then
            sto = sto.Substring(0, InStr(sto, "[") - 2)
        End If
        sfrom = Me.workflow.SelectedNode.Parent.Parent.Text
        If InStr(sfrom, "[") > 0 Then
            sfrom = sfrom.Substring(0, InStr(sfrom, "[") - 2)
        End If

        REM add it to appropriate stage
        delete_action_from_wf(controller.opt.nstage, sfrom, sto, Me.workflow.SelectedNode.Index, False)
        Me.workflow.SelectedNode.Remove()
        Me.workflow.ContextMenuStrip.Visible = False
        Me.controller.edit_mode()
        'Me.load_all()
    End Sub
    Private Sub action_sequence(ByVal up As Boolean)
        Dim sfrom, sto As String
        sto = Me.workflow.SelectedNode.Parent.NextNode.Text
        If InStr(sto, "[") > 0 Then
            sto = sto.Substring(0, InStr(sto, "[") - 2)
        End If
        sfrom = Me.workflow.SelectedNode.Parent.Parent.Text
        If InStr(sfrom, "[") > 0 Then
            sfrom = sfrom.Substring(0, InStr(sfrom, "[") - 2)
        End If

        REM add it to appropriate stage
        action_sequence_wf(controller.opt.nstage, sfrom, sto, Me.workflow.SelectedNode.Index, up, False)
        Dim tnode As TreeNode
        tnode = Me.workflow.SelectedNode.Parent

        Dim tx1 As String
        Dim idx As Integer
        If up = True Then
            idx = Me.workflow.SelectedNode.Index
            tx1 = Me.workflow.SelectedNode.Text
            Me.workflow.SelectedNode.Parent.Nodes.Insert(idx - 1, tx1, tx1, 1)
            Me.workflow.SelectedNode.Parent.Nodes(idx - 1).ForeColor = Color.Gray
            Me.workflow.SelectedNode = Me.workflow.SelectedNode.Parent.Nodes(idx - 1)
            Me.workflow.SelectedNode.Parent.Nodes(idx + 1).Remove()
        Else
            idx = Me.workflow.SelectedNode.Index
            tx1 = Me.workflow.SelectedNode.Text
            Me.workflow.SelectedNode.Parent.Nodes.Insert(idx + 2, tx1, tx1, 1)
            Me.workflow.SelectedNode.Parent.Nodes(idx + 2).ForeColor = Color.Gray
            Me.workflow.SelectedNode = Me.workflow.SelectedNode.Parent.Nodes(idx + 2)
            Me.workflow.SelectedNode.Parent.Nodes(idx).Remove()
        End If
        Me.controller.edit_mode()
    End Sub

    Private Sub add_action_to_wf(ByVal stage As bc_om_pub_type_workflow_stage, ByVal stage_from As String, ByVal stage_to As String, ByVal action As Integer, ByRef found As Boolean)
        If found = True Then
            Exit Sub
        End If
        If Not IsNothing(stage.routes) Then
            For i = 0 To stage.routes.Count - 1

                If stage.routes(i).current_stage_name = stage_to And stage.current_stage_name = stage_from Then
                    REM need to assign routing if same stage exists elsewhere to next node
                    REM TBD
                    stage.routes(i).actions.add(action)
                    found = True
                    Exit Sub
                Else
                    add_action_to_wf(stage.routes(i), stage_from, stage_to, action, found)
                End If
            Next
        End If

    End Sub
    Private Sub delete_action_from_wf(ByVal stage As bc_om_pub_type_workflow_stage, ByVal stage_from As String, ByVal stage_to As String, ByVal idx As Integer, ByRef found As Boolean)
        If found = True Then
            Exit Sub
        End If
        If Not IsNothing(stage.routes) Then
            For i = 0 To stage.routes.Count - 1

                If stage.routes(i).current_stage_name = stage_to And stage.current_stage_name = stage_from Then
                    REM need to assign routing if same stage exists elsewhere to next node
                    REM TBD
                    stage.routes(i).actions.removeat(idx)
                    found = True
                    Exit Sub
                Else
                    delete_action_from_wf(stage.routes(i), stage_from, stage_to, idx, found)
                End If
            Next
        End If

    End Sub
    Private Sub action_sequence_wf(ByVal stage As bc_om_pub_type_workflow_stage, ByVal stage_from As String, ByVal stage_to As String, ByVal idx As Integer, ByVal up As Boolean, ByRef found As Boolean)
        If found = True Then
            Exit Sub
        End If
        Dim taction As Integer
        If Not IsNothing(stage.routes) Then
            For i = 0 To stage.routes.Count - 1

                If stage.routes(i).current_stage_name = stage_to And stage.current_stage_name = stage_from Then
                    REM need to assign routing if same stage exists elsewhere to next node
                    REM TBD
                    taction = stage.routes(i).actions(idx)
                    stage.routes(i).actions.removeat(idx)
                    If up = True Then
                        stage.routes(i).actions.insert(idx - 1, taction)
                    Else
                        stage.routes(i).actions.insert(idx + 1, taction)
                    End If
                    found = True
                    Exit Sub
                Else
                    action_sequence_wf(stage.routes(i), stage_from, stage_to, idx, up, found)
                End If
            Next
        End If

    End Sub

    Private Sub maddroute_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles maddroute.Click
        find_original_stage()

        REM no

    End Sub

    Private Sub mremroute_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mremroute.Click
        rem_route()

    End Sub

    Private Sub mapprovers_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mapprovers.Click
        set_approvers()
    End Sub

    Private Sub mactionadd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mactionadd.Click
        add_action()
    End Sub

    Private Sub mdelaction_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mdelaction.Click
        remove_action()
    End Sub

    Private Sub mactionup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mactionup.Click
        Me.action_sequence(True)
    End Sub
    Private Sub mactiondn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mactiondown.Click
        Me.action_sequence(False)
    End Sub

    'Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
    '    load_all()
    'End Sub

    'Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
    '    Me.controller.opt.nstage.routes.Clear()
    '    Me.load_all()
    'End Sub

    'Private Sub Cstage_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cstage.SelectedIndexChanged
    '    If Me.Cstage.SelectedIndex > -1 Then
    '        Me.add_route()

    '    End If
    'End Sub

    'Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
    '    controller.opt.db_write()
    'End Sub

    Private Sub mstages_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        If Me.mcstages.SelectedIndex > -1 Then
            Me.add_route()
            set_new_stages()
        End If
    End Sub

    Private Sub SplitContainer2_SplitterMoved(ByVal sender As System.Object, ByVal e As System.Windows.Forms.SplitterEventArgs) Handles SplitContainer2.SplitterMoved

    End Sub

    Private Sub mcstages_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles mcstages.KeyPress
        If Asc(e.KeyChar) = 13 Then
            add_route()
        End If

    End Sub

    Private Sub mcstages_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mcstages.SelectedIndexChanged
        If Me.mcstages.SelectedIndex = -1 Then

            Exit Sub
        End If
        Me.add_route()
        Me.set_new_stages()
        Me.workflow.Refresh()
    End Sub

    Private Sub mcapprovers_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles mcapprovers.Enter

    End Sub

    Private Sub mcapprovers_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mcapprovers.SelectedIndexChanged
        If Me.from_Set_menu = False Then
            set_approvers()
        End If
    End Sub

    Private Sub mremproc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mremproc.Click
        Dim omsg As New bc_cs_message("Blue Curve", "Are you sure you wish to clear current process?", bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
        If omsg.cancel_selected = False Then
            Me.controller.opt.nstage.routes.Clear()
            load_all()
            controller.edit_mode()
        End If

    End Sub

    Private Sub mccopyproc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mccopyproc.SelectedIndexChanged
        If mccopyproc.SelectedIndex = -1 Then
            Exit Sub
        End If
        Try
            Dim omsg As New bc_cs_message("Blue Curve", "Copying process: " + Me.mccopyproc.Text + "  will clear current process proceed?", bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
            If omsg.cancel_selected = False Then
                If IsNothing(Me.controller.opt.nstage.routes) = False Then
                    Me.controller.opt.nstage.routes.Clear()
                End If
                Me.workflow.ContextMenuStrip.Visible = False
                Me.Cursor = Cursors.WaitCursor
                Dim nopt As New bc_om_pub_type_workflow
                nopt.id = Me.controller.container.opubtypes.pubtype(Me.mccopyproc.SelectedIndex).id
                nopt.bwithactions_and_approvers = True
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    nopt.db_read()
                Else
                    nopt.tmode = bc_cs_soap_base_class.tREAD
                    If nopt.transmit_to_server_and_receive(nopt, True) = False Then
                        Exit Sub
                    End If
                End If
                Me.controller.opt = nopt
                load_all()
                controller.edit_mode()
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("mcopyproc", "selectedindexchanged", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub mcaction_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mcaction.SelectedIndexChanged
        If mcaction.SelectedIndex = -1 Then
            Exit Sub
        End If
        add_action()
    End Sub


    Private Sub mccopyproc_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mccopyproc.Click

    End Sub

    Private Sub BlueCurve_TextSearch1_FireSearch(ByVal sender As Object, ByVal e As System.EventArgs) Handles BlueCurve_TextSearch1.FireSearch
        controller.pt_mode(Me.BlueCurve_TextSearch1.SearchText)
    End Sub

    Private Sub BlueCurve_TextSearch1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BlueCurve_TextSearch1.Load

    End Sub

    Private Sub mcapprovers_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mcapprovers.SelectedIndexChanged

        If Me.mcapprovers.SelectedIndex > 0 Then
            Me.mrole.Visible = True
            Me.mexceditor.Visible = True
            Me.mapp1.Visible = True
            Me.mapp2.Visible = True
        Else
            Me.mrole.Visible = False
            Me.mexceditor.Visible = False
            If Me.mrole.Items.Count > 0 Then
                Me.mrole.SelectedIndex = 0
            End If
            If Me.mexceditor.Items.Count > 0 Then
                Me.mexceditor.SelectedIndex = 0
            End If
        End If
    End Sub


    Private Sub mexceditor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mexceditor.SelectedIndexChanged
        If Me.from_Set_menu = False Then
            set_approvers()
        End If
    End Sub

    Private Sub mrole_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mrole.SelectedIndexChanged
        If Me.from_Set_menu = False Then
            set_approvers()
        End If
    End Sub


    Private Sub mcapprovers_Click_2(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mcapprovers.Click

    End Sub

    Private Sub DataGridView1_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles DataGridView1.DataError

    End Sub

    Private Sub uxdistribution_Click(sender As Object, e As EventArgs) Handles uxdistribution.Click
        controller.load_distribution()
       
    End Sub
End Class

