Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports System.Windows.Forms
Imports System.Drawing

Public Class bc_am_cp_users
    Public mode As Integer
    Public sconn As String
    Public suser As String
    Public bload As Boolean = False

    Public warnings As New ArrayList
    Public new_user As Boolean = False
    Public attribute_mode As Integer
    Public rdrag As Boolean = True
    Public ldrag As Boolean = True
    Public maintain_mode As Integer = 0
    Public node_id As Integer
    Public controller As Object

    REM password
    Public PassWordRowIndex As Integer = 0

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
    Private Const translation_icon As Integer = 33


    Private Sub tusers_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tusers.AfterSelect

        set_menu()
        Me.tfilter.SearchText = ""
        mactive.Enabled = True
        controller.user_mode("")

    End Sub
    Private Sub set_menu()

        tfilter.SearchClass = -1
        'tfilter.SearchSetup()

        Me.DeleteToolStripMenuItem.Text = ""
        Me.tusers.ContextMenuStrip = Nothing
        If controller.container.mode = 0 Then
            Exit Sub
        End If
        Me.addpt.Visible = False
        Me.dept.Visible = False
        Me.rept.Visible = False

        If InStr(tusers.SelectedNode.Text, "(inactive)") = 0 Then
            mactive.Image = tabimages.Images(deactivate_icon)
        Else
            mactive.Image = tabimages.Images(active_icon)
        End If
        Me.ChangeNameToolStripMenuItem.Visible = True
        Me.mactive.Visible = False
        Me.DeleteToolStripMenuItem.Visible = True

        If controller.Container.mode < 2 Then
            Me.DeleteToolStripMenuItem.Visible = False
        End If
       
        If Me.tusers.SelectedNode.Text = "Users by Role" Then
            maintain_mode = 0
            Me.ChangeNameToolStripMenuItem.Visible = False
            Me.mactive.Visible = False
            Me.DeleteToolStripMenuItem.Visible = False
            If controller.container.mode > 0 Then
                Me.tusers.ContextMenuStrip = Me.maintainitemscsm
            End If
            Exit Sub
        End If
      

        If Me.tusers.SelectedNode.Text = "Users by Office" Then
            maintain_mode = 1
            Me.ChangeNameToolStripMenuItem.Visible = False
            Me.mactive.Visible = False
            Me.DeleteToolStripMenuItem.Visible = False
            If controller.container.mode > 0 Then
                Me.tusers.ContextMenuStrip = Me.maintainitemscsm
            End If
            Exit Sub
        End If
        If Me.tusers.SelectedNode.Text = "Users by Business Area" Then
            maintain_mode = 2
            Me.ChangeNameToolStripMenuItem.Visible = False
            Me.mactive.Visible = False
            Me.DeleteToolStripMenuItem.Visible = False
            If controller.container.mode > 0 Then
                Me.tusers.ContextMenuStrip = Me.maintainitemscsm
            End If
            Exit Sub
        End If
        If Me.tusers.SelectedNode.Text = "Users by Language" Then
            maintain_mode = 3
            Me.ChangeNameToolStripMenuItem.Visible = False
            Me.mactive.Visible = False

            Me.DeleteToolStripMenuItem.Visible = False
            If controller.container.mode > 0 Then
                Me.tusers.ContextMenuStrip = Me.maintainitemscsm
            End If
            Exit Sub
        End If
        Try
            If Me.tusers.SelectedNode.Parent.Text = "Users by Role" Then
                maintain_mode = 0
                Me.ChangeNameToolStripMenuItem.Text = "Change name of " + Me.tusers.SelectedNode.Text

                If InStr(Me.tusers.SelectedNode.Text, "(inactive)") > 0 Then
                    Me.mactive.Text = "Make " + Me.tusers.SelectedNode.Text.Substring(0, Me.tusers.SelectedNode.Text.Length - 11) + " active"
                    Me.DeleteToolStripMenuItem.Enabled = True
                    Me.ChangeNameToolStripMenuItem.Enabled = False
                Else
                    Me.mactive.Text = "Make " + Me.tusers.SelectedNode.Text + " inactive"
                    Me.DeleteToolStripMenuItem.Enabled = False
                    Me.ChangeNameToolStripMenuItem.Enabled = True
                End If
                Me.mactive.Visible = True
                Me.DeleteToolStripMenuItem.Text = "Delete " + Me.tusers.SelectedNode.Text
                If controller.container.mode > 0 Then
                    Me.tusers.ContextMenuStrip = Me.maintainitemscsm
                End If

                Exit Sub
            End If
            If Me.tusers.SelectedNode.Parent.Text = "Users by Office" Then
                maintain_mode = 1
                Me.ChangeNameToolStripMenuItem.Text = "Change name of " + Me.tusers.SelectedNode.Text
                If InStr(Me.tusers.SelectedNode.Text, "(inactive)") > 0 Then
                    Me.mactive.Text = "Make " + Me.tusers.SelectedNode.Text.Substring(0, Me.tusers.SelectedNode.Text.Length - 11) + " active"
                    Me.DeleteToolStripMenuItem.Enabled = True
                    Me.ChangeNameToolStripMenuItem.Enabled = False
                Else
                    Me.mactive.Text = "Make " + Me.tusers.SelectedNode.Text + " inactive"
                    Me.DeleteToolStripMenuItem.Enabled = False
                    Me.ChangeNameToolStripMenuItem.Enabled = True
                End If
                Me.mactive.Visible = True
                Me.DeleteToolStripMenuItem.Text = "Delete " + Me.tusers.SelectedNode.Text
                If controller.container.mode > 0 Then
                    Me.tusers.ContextMenuStrip = Me.maintainitemscsm
                End If
                Exit Sub
            End If
            If Me.tusers.SelectedNode.Parent.Text = "Users by Business Area" Then
                maintain_mode = 2
                Me.ChangeNameToolStripMenuItem.Text = "Change name of " + Me.tusers.SelectedNode.Text
                If InStr(Me.tusers.SelectedNode.Text, "(inactive)") > 0 Then
                    Me.mactive.Text = "Make " + Me.tusers.SelectedNode.Text.Substring(0, Me.tusers.SelectedNode.Text.Length - 11) + " active"
                    Me.DeleteToolStripMenuItem.Enabled = True
                    Me.ChangeNameToolStripMenuItem.Enabled = False
                Else
                    Me.mactive.Text = "Make " + Me.tusers.SelectedNode.Text + " inactive"
                    Me.DeleteToolStripMenuItem.Enabled = False
                    Me.ChangeNameToolStripMenuItem.Enabled = True
                End If
                Me.mactive.Visible = True
                Me.DeleteToolStripMenuItem.Text = "Delete " + Me.tusers.SelectedNode.Text
                If controller.container.mode > 0 Then
                    Me.tusers.ContextMenuStrip = Me.maintainitemscsm
                End If
                Exit Sub
            End If
            If Me.tusers.SelectedNode.Parent.Text = "Users by Language" Then
                maintain_mode = 3
                Me.ChangeNameToolStripMenuItem.Text = "Change name of " + Me.tusers.SelectedNode.Text
                If InStr(Me.tusers.SelectedNode.Text, "(inactive)") > 0 Then
                    Me.mactive.Text = "Make " + Me.tusers.SelectedNode.Text.Substring(0, Me.tusers.SelectedNode.Text.Length - 11) + " active"
                    Me.DeleteToolStripMenuItem.Enabled = True
                    Me.ChangeNameToolStripMenuItem.Enabled = False
                Else
                    Me.mactive.Text = "Make " + Me.tusers.SelectedNode.Text + " inactive"
                    Me.DeleteToolStripMenuItem.Enabled = False
                    Me.ChangeNameToolStripMenuItem.Enabled = True
                End If
                Me.mactive.Visible = True
                Me.DeleteToolStripMenuItem.Text = "Delete " + Me.tusers.SelectedNode.Text
                If controller.container.mode > 0 Then
                    Me.tusers.ContextMenuStrip = Me.maintainitemscsm
                End If
                Exit Sub
            End If
        Catch

        End Try
        If InStr(Me.tusers.SelectedNode.Text, "Access Level") > 0 Then
            maintain_mode = 0
            Me.ChangeNameToolStripMenuItem.Text = "Change Access of role " + Me.tusers.SelectedNode.Parent.Text
            Me.DeleteToolStripMenuItem.Visible = False

            If controller.container.mode > 0 Then
                Me.tusers.ContextMenuStrip = Me.maintainitemscsm
            End If
            Exit Sub
        End If

        Try
            If InStr(Me.tusers.SelectedNode.Parent.Text, "Admin Applications") > 0 Then
                Me.maintain_mode = 11
                Me.node_id = Me.tusers.SelectedNode.Parent.Index

                Me.ChangeNameToolStripMenuItem.Visible = False
                Me.DeleteToolStripMenuItem.Enabled = True
                If Me.tusers.SelectedNode.ImageIndex <> deactivate_icon Then
                    Me.DeleteToolStripMenuItem.Text = "Remove admin application " + Me.tusers.SelectedNode.Text + " from role " + Me.tusers.SelectedNode.Parent.Parent.Text
                    Me.DeleteToolStripMenuItem.Image = Me.tabimages.Images(deactivate_icon)
                Else
                    If Me.tusers.SelectedNode.Text = "Taxonomy" Then
                        Me.DeleteToolStripMenuItem.Image = Me.tabimages.Images(class_icon)
                    ElseIf Me.tusers.SelectedNode.Text = "Users" Then
                        Me.DeleteToolStripMenuItem.Image = Me.tabimages.Images(users_icon)
                    ElseIf Me.tusers.SelectedNode.Text = "Translation" Then
                        Me.DeleteToolStripMenuItem.Image = Me.tabimages.Images(translation_icon)

                    Else
                        Me.DeleteToolStripMenuItem.Image = Me.tabimages.Images(publication_icon)
                    End If
                    Me.DeleteToolStripMenuItem.Text = "Add admin application " + Me.tusers.SelectedNode.Text + " to role " + Me.tusers.SelectedNode.Parent.Parent.Text
                End If
                Me.DeleteToolStripMenuItem.Visible = True
                If controller.container.mode > 0 Then
                    Me.tusers.ContextMenuStrip = Me.maintainitemscsm
                End If
                Exit Sub
            End If
        Catch

        End Try
        Try
            If InStr(Me.tusers.SelectedNode.Parent.Parent.Text, "Stage Access") > 0 Then
                Me.maintain_mode = 11
                Me.node_id = Me.tusers.SelectedNode.Parent.Index
                Me.ChangeNameToolStripMenuItem.Visible = False
                Me.DeleteToolStripMenuItem.Enabled = True
                Dim st As String
                st = Me.tusers.SelectedNode.Parent.Text
                st = st.Substring(0, InStr(st, "[") - 1)
                If Me.tusers.SelectedNode.ImageIndex <> deactivate_icon Then
                    Me.DeleteToolStripMenuItem.Text = "Remove access " + Me.tusers.SelectedNode.Text + " from stage " + st + " for " + Me.tusers.SelectedNode.Parent.Parent.Parent.Text
                    Me.DeleteToolStripMenuItem.Image = Me.tabimages.Images(deactivate_icon)
                Else
                    Me.DeleteToolStripMenuItem.Image = Me.tabimages.Images(20)

                    Me.DeleteToolStripMenuItem.Text = "Add access " + Me.tusers.SelectedNode.Text + " to stage " + st + " for " + Me.tusers.SelectedNode.Parent.Parent.Parent.Text
                End If
                Me.DeleteToolStripMenuItem.Visible = True
                If controller.container.mode > 0 Then
                    Me.tusers.ContextMenuStrip = Me.maintainitemscsm
                End If
                Exit Sub
            End If
        Catch

        End Try

        If Me.tusers.SelectedNode.Text = "User Preference Types" Then
            Me.tusers.ContextMenuStrip = Me.maintainitemscsm
            Me.ChangeNameToolStripMenuItem.Visible = False
            Me.DeleteToolStripMenuItem.Visible = False
            Me.tusers.ContextMenuStrip.Visible = True
            Me.addpt.Visible = True
        End If
        Try
            If Me.tusers.SelectedNode.Parent.Text = "User Preference Types" Then
                Me.tusers.ContextMenuStrip = Me.maintainitemscsm
                Me.dept.Text = "Delete Preference Type " + Me.tusers.SelectedNode.Text
                Me.rept.Text = "Rename Preference Type " + Me.tusers.SelectedNode.Text
                Me.ChangeNameToolStripMenuItem.Visible = False
                Me.DeleteToolStripMenuItem.Visible = False
                Me.dept.Visible = True
                Me.rept.Visible = True
            End If
        Catch

        End Try


    End Sub

    Private Sub lusers_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Lusers.SelectedIndexChanged
        controller.load_user_toolbar()
        Me.baudit.Enabled = False
        Me.buseraudit.Enabled = False

        If Me.Lusers.SelectedItems.Count = 0 Then
            Me.mactive.Enabled = False
            Me.DeleteEntityToolStripMenuItem.Enabled = False
            Exit Sub
        End If
        Me.Cursor = Cursors.WaitCursor
        controller.load_user()
        Me.Cursor = Cursors.Default
        Me.puser.BringToFront()
        Me.baudit.Enabled = True
        Me.buseraudit.Enabled = True
        'Password
        PassWordRowIndex = 0

    End Sub

    Private Sub DataGridView1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If Me.DataGridView1.SelectedCells.Count = 0 Then
            Exit Sub
        End If

        If Me.DataGridView1.SelectedCells(0).ColumnIndex <> 3 Then
            Exit Sub
        End If
        If Me.Lusers.SelectedItems.Count = 0 Then
            Exit Sub
        End If
        Select Case Me.DataGridView1.SelectedCells(0).RowIndex
            Case 3
                controller.add_new_role_menu()
            Case 7
                controller.add_new_language_menu()
            Case 12
                controller.add_new_office_menu()
        End Select

    End Sub

    'Private Sub DataGridView1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs)
    '    If Me.DataGridView1.SelectedCells(0).ColumnIndex <> 1 Then
    '        Exit Sub
    '    End If
    '    Me.mdettitle.Text = "Details for: " + Me.Ttitle.SelectedTab.Text
    '    Me.mdetupdate.Text = "Last Updated: " + controller.current_user.comment
    '    Me.mdetuser.Text = "By: Unknown"
    '    If Me.controller.current_user.change_user = CStr(bc_cs_central_settings.logged_on_user_id) Then
    '        Me.mdetuser.Text = "By: me"
    '    Else
    '        For i = 0 To Me.controller.container.ousers.user.Count - 1
    '            If Me.controller.container.ousers.user(i).id = Me.controller.current_user.change_user Then
    '                Me.mdetuser.Text = "By: " + Me.controller.container.ousers.user(i).first_name + " " + Me.controller.container.ousers.user(i).surname
    '                Exit For
    '            End If
    '        Next
    '    End If
    '    Me.mdetpubupdate.Visible = False
    '    Me.mdetpubuser.Visible = False
    '    Me.attributedetails.Show(Cursor.Position.X, Cursor.Position.Y)
    'End Sub


    Private Sub lvwvalidate_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvwvalidate.SelectedIndexChanged
        If Me.lvwvalidate.SelectedItems.Count > 0 Then
            If warnings(Me.lvwvalidate.SelectedItems(0).Index).entity_tab > -1 Then
                Me.tentities.SelectedNode = Me.tentities.Nodes(0)
            Else
                Me.DataGridView1.Focus()
                Me.DataGridView1.Item(1, warnings(Me.lvwvalidate.SelectedItems(0).Index).attribute).Selected = True
            End If
        End If
    End Sub

    Private Sub Pnouser_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Pnouser.Click
        controller.load_user_pic()
    End Sub

    Private Sub puserpic_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        controller.load_user_pic()
    End Sub


    Private Sub bentdiscard_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bentdiscard.Click

        'password
        uxUserPassword.Visible = False
        uxUserPassword.Text = ""
        DataGridView1.Rows(1).Cells(1).Selected = True

        controller.discard_change()
    End Sub

    Private Sub DeleteEntityToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteEntityToolStripMenuItem.Click
        controller.delete_user()
    End Sub


    Private Sub DeleteToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteToolStripMenuItem.Click

        Cursor = Cursors.WaitCursor
        If InStr(DeleteToolStripMenuItem.Text, "Remove admin application") > 0 Then
            controller.remove_app()
        ElseIf InStr(DeleteToolStripMenuItem.Text, "Add admin application") > 0 Then
            controller.add_app()
        ElseIf InStr(DeleteToolStripMenuItem.Text, "Remove access") > 0 Then
            controller.rem_stage_role_access()
        ElseIf InStr(DeleteToolStripMenuItem.Text, "Add access") > 0 Then
            controller.add_stage_role_access()
        Else
            controller.delete_item()
        End If
        Cursor = Cursors.Default
    End Sub



    Private Sub mactive_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mactive.Click
        Me.Cursor = Cursors.WaitCursor

        controller.item_active()
        Me.Cursor = Cursors.Default


    End Sub

    'Private Sub DataGridView1_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Try
    '        If DataGridView1.SelectedCells(0).ColumnIndex = 2 Then
    '            DataGridView1.Item(0, DataGridView1.SelectedCells(0).RowIndex).Selected = True
    '        End If
    '    Catch

    '    End Try
    'End Sub

    Private Sub bc_am_cp_users_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.SizeChanged
        If Not IsNothing(controller) Then
            controller.size_nicely()
        End If
    End Sub

    Private Sub bentdiscard_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bentdiscard.Click

    End Sub

    Private Sub AddEntityToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddEntityToolStripMenuItem.Click
        controller.add_user_menu()
    End Sub

    Private Sub minactive_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles minactive.Click
        controller.active()
    End Sub

    Private Sub bentchanges_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bentchanges.Click

        'password


        Dim osecurity As New bc_cs_security
        uxUserPassword.Visible = False

        If PassWordRowIndex <> 0 Then

            Dim bcs As New bc_cs_security
            Dim certificate As New bc_cs_security.certificate

            'osecurity.HashPassword(controller.current_user.id, DataGridView1.Rows(PassWordRowIndex).Tag, certificate)
            'controller.View.user_attributes_values(6).value() = DataGridView1.Rows(PassWordRowIndex).Tag
            controller.View.user_attributes_values(6).value() = osecurity.HashPassword(controller.current_user.id, DataGridView1.Rows(PassWordRowIndex).Tag, certificate)

            PassWordRowIndex = 0
        End If

        DataGridView1.Rows(1).Cells(1).Selected = True

        controller.commit_changes()
    End Sub

    'Private Sub DataGridView1_CellValidating(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellValidatingEventArgs)

    '    REM password
    '    If uxUserPassword.Visible = False Then
    '        If controller.Container.mode > bc_am_cp_container.VIEW Then
    '            If e.ColumnIndex = 2 And e.FormattedValue.ToString <> "System.Drawing.Bitmap" Then
    '                If controller.validate_attribute_value(e.FormattedValue) = False Then
    '                    e.Cancel = True
    '                End If
    '            End If
    '        End If
    '    End If

    'End Sub

    'Private Sub DataGridView1_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs)

    '    'password
    '    Dim PassRectangel As Rectangle

    '    If controller.Container.mode = bc_am_cp_container.VIEW Then
    '        Exit Sub
    '    End If
    '    If Me.DataGridView1.SelectedCells.Count = 0 Then
    '        Exit Sub
    '    End If

    '    If Me.DataGridView1.SelectedCells(0).ColumnIndex = 2 Then

    '        'Steve password
    '        If DataGridView1.Item(1, DataGridView1.CurrentCell.RowIndex).Value = "Password" Then
    '            DataGridView1.CurrentCell.Selected = False

    '            If DataGridView1.SelectedCells.Count > 1 Then
    '                DataGridView1.SelectedCells(0).Selected = False
    '            End If

    '            PassWordRowIndex = DataGridView1.CurrentCell.RowIndex
    '            PassRectangel = DataGridView1.GetCellDisplayRectangle(2, PassWordRowIndex, True)
    '            uxUserPassword.Top = PassRectangel.Top + DataGridView1.Top + 5
    '            uxUserPassword.Left = PassRectangel.Left + 2
    '            uxUserPassword.Size = New System.Drawing.Size(PassRectangel.Size.Width - 5, PassRectangel.Size.Height)
    '            uxUserPassword.Text = ""

    '            uxUserPassword.Visible = True
    '            uxUserPassword.SelectAll()
    '            uxUserPassword.Select()

    '            Exit Sub
    '        Else

    '            If Me.DataGridView1.Columns(2).ReadOnly = False Then
    '                controller.set_data_type_control()
    '            Else
    '                Exit Sub
    '            End If

    '        End If

    '    End If
    '    controller.load_user_toolbar()
    'End Sub




    Private Sub massignentity_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles massignentity.Click
        If Me.tentities.SelectedNode.Text = "Business Areas" Then
            controller.assign_bus_area()
        ElseIf Me.tentities.SelectedNode.Text = "Other Roles" Then
            controller.assign_other_roles()
        ElseIf Me.tentities.SelectedNode.Text = "User Associations" Then
            controller.assign_user_associations()
        Else
            controller.assign_entity()
        End If
    End Sub

    Private Sub tentities_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tentities.AfterSelect
        Me.AddAssociatedClassToolStripMenuItem.Image = Me.tabimages.Images(7)
        controller.set_assign_menu()
    End Sub

    Private Sub AddAssociatedClassToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddAssociatedClassToolStripMenuItem.Click
        controller.assign_new_class()
    End Sub

    Private Sub ChangeNameToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChangeNameToolStripMenuItem.Click
        If InStr(ChangeNameToolStripMenuItem.Text, "Change Access") > 0 Then
            controller.assign_access_menu()
        Else
            controller.change_item_menu()
        End If

    End Sub



    Private Sub IncreaseRatingToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles IncreaseRatingToolStripMenuItem.Click
        controller.increase_rating(True)
    End Sub

    Private Sub DecreaseRatingToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DecreaseRatingToolStripMenuItem.Click
        controller.increase_rating(False)
    End Sub


    Private Sub DataGridView1_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)

    End Sub

    Private Sub bc_am_cp_users_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub SplitContainer2_SplitterMoved(ByVal sender As System.Object, ByVal e As System.Windows.Forms.SplitterEventArgs) Handles SplitContainer2.SplitterMoved

    End Sub

    Private Sub puserpic_BackgroundImageLayoutChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles puserpic.BackgroundImageLayoutChanged

    End Sub
    'Private Sub DataGridView1_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs)


    'End Sub
    Private Sub puserpic_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles puserpic.Click
        controller.load_user_pic()
    End Sub

    Private Sub SplitContainer1_SplitterMoved(ByVal sender As System.Object, ByVal e As System.Windows.Forms.SplitterEventArgs) Handles SplitContainer1.SplitterMoved

    End Sub

    Private Sub DataGridView1_DockChanged(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub pusers_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles pusers.Paint

    End Sub

    Private Sub BlueCurve_TextSearch1_FireSearch(ByVal sender As Object, ByVal e As System.EventArgs) Handles tfilter.FireSearch
        controller.user_mode(Me.tfilter.SearchText)
    End Sub

    Private Sub puser_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles puser.Paint

    End Sub

    Private Sub tfilter_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tfilter.Load

    End Sub
    REM ING JUNE 2012

    Private Sub pendingsync_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pendingsync.Click
        controller.set_sync(1)
    End Sub
    REM ---

    Private Sub dept_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dept.Click
        controller.del_pref_type()
    End Sub

    Private Sub addpt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles addpt.Click
        controller.add_pref_type()
    End Sub

    Private Sub rept_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rept.Click
        controller.rename_pref_type()
    End Sub

    Private Sub uxUserPassword_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        'Password
        If Asc(e.KeyChar) = 13 Then
            uxUserPassword.Visible = False
            DataGridView1.Rows(7).Cells(2).Selected = True
            DataGridView1.Select()
        Else
            puser.Enabled = True
            bentchanges.Enabled = True
            bentdiscard.Enabled = True
            pusers.Enabled = False
            tusers.Enabled = False
        End If

    End Sub

    Private Sub uxUserPassword_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)

        'Password
        Dim pwd As Char = "*"

        uxUserPassword.Visible = False
        If Len(uxUserPassword.Text) > 0 Then
            DataGridView1.Item(3, PassWordRowIndex).Value = New String(pwd, 6)
            DataGridView1.Rows(PassWordRowIndex).Tag = uxUserPassword.Text
        End If

    End Sub

    Private Sub DataGridView1_Resize(ByVal sender As Object, ByVal e As System.EventArgs)

        'Password
        Dim PassRectangel As Rectangle

        If uxUserPassword.Visible = True Then
            PassRectangel = DataGridView1.GetCellDisplayRectangle(2, PassWordRowIndex, True)
            uxUserPassword.Top = PassRectangel.Top + DataGridView1.Top + 5
            uxUserPassword.Left = PassRectangel.Left + 2
            uxUserPassword.Size = New System.Drawing.Size(PassRectangel.Size.Width - 5, PassRectangel.Size.Height)
        End If

    End Sub

    Private Sub DataGridView1_Scroll(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs)

        'Password
        Dim PassRectangel As Rectangle

        If uxUserPassword.Visible = True Then
            PassRectangel = DataGridView1.GetCellDisplayRectangle(2, PassWordRowIndex, True)
            uxUserPassword.Top = PassRectangel.Top + DataGridView1.Top + 5
            uxUserPassword.Left = PassRectangel.Left + 2
            uxUserPassword.Size = New System.Drawing.Size(PassRectangel.Size.Width - 5, PassRectangel.Size.Height)
        End If


    End Sub
    REM FIL JUN 2013
    Private Sub tusername_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tusername.SelectedIndexChanged


        If Me.tusername.SelectedIndex = 0 Then
            Me.DataGridView1.Visible = True
            Me.dpartialsync.Visible = False
        Else
            Me.DataGridView1.Visible = False
            Me.dpartialsync.Visible = True
        End If
    End Sub

    Private Sub cphysical_templates_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        controller.set_change()
    End Sub

    Private Sub Cpublications_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        controller.set_change()
    End Sub

    Private Sub centities_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        controller.set_change()
    End Sub

    Private Sub cpendingsync_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cpendingsync.Click
        controller.set_sync(2)
    End Sub

    Private Sub mpt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mpt.Click
        controller.set_sync(3)
    End Sub

    Private Sub mps_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mps.Click
        controller.set_sync(4)
    End Sub

    Private Sub mpe_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mpe.Click
        controller.set_sync(5)
    End Sub

  

    Private Sub dpartialsync_CellValidating(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellValidatingEventArgs) Handles dpartialsync.CellValidating
        controller.set_change()
    End Sub

    Private Sub DataGridView1_CellErrorTextChanged(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellErrorTextChanged

    End Sub

  

    Private Sub DataGridView1_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridView1.SelectionChanged
        Try

            If Me.controller.container.mode = bc_am_cp_container.VIEW Or Me.DataGridView1.RowCount = 0 Then
                REM read only
                Exit Sub
            End If
            controller.load_user_toolbar()
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
                REM add new office or location
                Select Case Me.controller.user_attributes(Me.DataGridView1.SelectedCells(0).RowIndex).add_new
                    Case "office"
                        controller.add_new_office_menu()
                    Case "role"
                        controller.add_new_role_menu()
                    Case "language"
                        controller.add_new_language_menu()
                End Select



            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("DataGridView1", "SelectionChanged", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try
    End Sub
    Private Sub DataGridView1_CellValidating(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellValidatingEventArgs) Handles DataGridView1.CellValidating

        If Me.Lusers.SelectedItems.Count = 1 Then

            If InStr(Me.Lusers.SelectedItems(0).Text, "(inactive)") > 0 Then
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

    Private Sub DataGridView1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridView1.DoubleClick

        Try
            If Me.DataGridView1.SelectedCells(0).ColumnIndex = 3 Or Me.DataGridView1.RowCount = 0 Then
                Exit Sub
            End If
            controller.attribute_details(False)
        Catch

        End Try

    End Sub
    Private Sub DataGridView1_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridView1.MouseLeave

        Try
            If DataGridView1.SelectedCells(0).ColumnIndex = 3 And Me.DataGridView1.RowCount > 0 Then
                DataGridView1.Item(0, DataGridView1.SelectedCells(0).RowIndex).Selected = True
            End If
        Catch

        End Try
    End Sub


  
   
    Private Sub DataGridView1_CellContentClick_1(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub DataGridView1_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles DataGridView1.DataError

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)
        controller.attribute_history()
    End Sub

    Private Sub tentities_DoubleClick(sender As Object, e As EventArgs) Handles tentities.DoubleClick
        Try

            If Me.tentities.SelectedNode.Index > 2 Then
                controller.show_audit(Me.tentities.SelectedNode.Index, Me.tentities.SelectedNode.Text)
            Else
                controller.show_audit_other_areas(Me.tentities.SelectedNode.Index, Me.tentities.SelectedNode.Text)
            End If
        Catch


        End Try
    End Sub

    Private Sub baudit_Click(sender As Object, e As EventArgs)
        controller.show_user_audit()
    End Sub

    Private Sub baudit_Click_1(sender As Object, e As EventArgs) Handles baudit.Click
        controller.attribute_details(True)
    End Sub

    Private Sub buseraudit_Click(sender As Object, e As EventArgs) Handles buseraudit.Click
        controller.show_user_audit()
    End Sub
End Class

