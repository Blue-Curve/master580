Imports System.Drawing
Imports System.Windows.Forms
Public Class bc_am_process_settings
    Friend cancel_selected As Boolean
    Friend ctrllr As bc_am_process
    Friend restore_defaults As Boolean = False

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bcancel.Click
        cancel_selected = True
        Me.Hide()
    End Sub

    Private Sub Chkalert_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chkalert.CheckedChanged
        Me.lfade.Enabled = False
        Me.Chkbeep.Enabled = False
        If Me.Chkalert.Checked = True Then
            Me.lfade.Enabled = True
            Me.Chkbeep.Enabled = True
        End If
    End Sub

    Private Sub CheckEdit1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chkar.CheckedChanged
        Me.Lactivity.Enabled = False
        If Me.Chkar.Checked = True Then
            Me.Lactivity.Enabled = True
        End If
    End Sub

    Private Sub Chkpoll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chkpoll.CheckedChanged
        Me.Lpoll.Enabled = False
        If Me.Chkpoll.Checked = True Then
            Me.Lpoll.Enabled = True
        End If
    End Sub
    Private Sub bsave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bsave.Click
        cancel_selected = False
        Me.Hide()
    End Sub

    Private Sub SimpleButton1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        Dim cols As New List(Of String)

        For i = 0 To Me.chkcolumns.Items.Count - 1
            cols.Add(Me.chkcolumns.Items(i))
        Next
        Me.chkcolumns.Items.Clear()
        For i = 0 To cols.Count - 1
            Me.chkcolumns.Items.Add(cols(i), True)
        Next

        ctrllr.restore_defaults(Me)
        restore_defaults = True
    End Sub

   

    Private Sub bc1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bc1.Click
        Dim dialog As New ColorDialog
        dialog.Color = bc_am_process.colors.doc_list_read_backcolor
        dialog.ShowDialog()
        bc1.Appearance.BackColor = dialog.Color
    End Sub

    Private Sub bc2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bc2.Click

        Dim dialog As New ColorDialog
        dialog.Color = bc_am_process.colors.doc_list_unread_backcolor
        dialog.ShowDialog()
        bc2.Appearance.BackColor = dialog.Color
    End Sub

    Private Sub bc4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bc4.Click

        Dim dialog As New ColorDialog
        dialog.Color = bc_am_process.colors.doc_list_urgent_backcolor

        dialog.ShowDialog()
        bc4.Appearance.BackColor = dialog.Color
    End Sub

    Private Sub bc3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bc3.Click

        Dim dialog As New ColorDialog
        dialog.Color = bc_am_process.colors.doc_list_stage_changed_backcolor
        dialog.ShowDialog()
        bc3.Appearance.BackColor = dialog.Color
    End Sub

    Private Sub bc5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bc5.Click

        Dim dialog As New ColorDialog
        dialog.Color = bc_am_process.colors.doc_list_expired_backcolor
        dialog.ShowDialog()
        bc5.Appearance.BackColor = dialog.Color
    End Sub

    Private Sub bc6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bc6.Click
        Dim dialog As New ColorDialog
        dialog.Color = bc_am_process.colors.workflow_current_stage_backcolor
        dialog.ShowDialog()
        bc6.Appearance.BackColor = dialog.Color
    End Sub

    Private Sub bc7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bc7.Click
        Dim dialog As New ColorDialog
        dialog.Color = bc_am_process.colors.workflow_next_stage_backcolor
        dialog.ShowDialog()
        bc7.Appearance.BackColor = dialog.Color
    End Sub

    Private Sub fc1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles fc1.Click
        Dim dialog As New ColorDialog
        dialog.Color = bc_am_process.colors.doc_list_read_forecolor


        dialog.ShowDialog()
        fc1.Appearance.BackColor = dialog.Color
    End Sub

    Private Sub fc2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles fc2.Click
        Dim dialog As New ColorDialog
        dialog.Color = bc_am_process.colors.doc_list_unread_forecolor


        dialog.ShowDialog()
        fc2.Appearance.BackColor = dialog.Color
    End Sub

    Private Sub fc4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles fc4.Click
        Dim dialog As New ColorDialog
        dialog.Color = bc_am_process.colors.doc_list_urgent_forecolor
        dialog.ShowDialog()
        fc4.Appearance.BackColor = dialog.Color
    End Sub

    Private Sub fc3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles fc3.Click
        Dim dialog As New ColorDialog
        dialog.Color = bc_am_process.colors.doc_list_stage_changed_forecolor

        dialog.ShowDialog()
        fc3.Appearance.BackColor = dialog.Color
    End Sub

    Private Sub fc5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles fc5.Click
        Dim dialog As New ColorDialog
        dialog.Color = bc_am_process.colors.doc_list_expired_forecolor
        dialog.ShowDialog()
        fc5.Appearance.BackColor = dialog.Color
    End Sub

    Private Sub fc6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles fc6.Click
        Dim dialog As New ColorDialog
        dialog.Color = bc_am_process.colors.workflow_current_stage_forecolor
        dialog.ShowDialog()
        fc6.Appearance.BackColor = dialog.Color
    End Sub

    Private Sub fc7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles fc7.Click
        Dim dialog As New ColorDialog
        dialog.Color = bc_am_process.colors.workflow_next_stage_forecolor
        dialog.ShowDialog()
        fc7.Appearance.BackColor = dialog.Color
    End Sub
End Class