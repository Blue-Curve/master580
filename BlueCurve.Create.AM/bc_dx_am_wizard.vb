Public Class bc_dx_am_wizard
    Friend controller As bc_dx_am_wizard_controller

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        Me.Cursor = Windows.Forms.Cursors.WaitCursor
        controller.button_Press(SimpleButton1.Tag.ToString)
        Me.Cursor = Windows.Forms.Cursors.Default
    End Sub
    Private Sub SimpleButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton2.Click
        Me.Cursor = Windows.Forms.Cursors.WaitCursor
        controller.button_Press(SimpleButton2.Tag.ToString)
        Me.Cursor = Windows.Forms.Cursors.Default
    End Sub
    Private Sub SimpleButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton3.Click
        Me.Cursor = Windows.Forms.Cursors.WaitCursor
        controller.button_Press(SimpleButton3.Tag.ToString)
        Me.Cursor = Windows.Forms.Cursors.Default
    End Sub
    Private Sub SimpleButton4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton4.Click
        Me.Cursor = Windows.Forms.Cursors.WaitCursor
        controller.button_Press(SimpleButton4.Tag.ToString)
        Me.Cursor = Windows.Forms.Cursors.Default
    End Sub
    Private Sub uxpubtypelist_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxpubtypelist.SelectedIndexChanged
        If uxpubtypelist.SelectedIndex > -1 Then
            controller.pub_type_selected(uxpubtypelist.Text)
        End If
    End Sub
    Private Sub uxallmine_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles uxallmine.SelectedIndexChanged
        Me.Cursor = Windows.Forms.Cursors.WaitCursor
        controller.toggle_all_mine(uxallmine.SelectedIndex)
        Me.Cursor = Windows.Forms.Cursors.Default
    End Sub
End Class