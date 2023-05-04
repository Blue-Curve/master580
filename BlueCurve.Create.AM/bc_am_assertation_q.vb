Public Class bc_am_assertation_q
    Event option_changed(sel As Integer)
    Private Sub uxo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles uxo.SelectedIndexChanged

        RaiseEvent option_changed(uxo.SelectedIndex)

    End Sub

    
    Private Sub uxpass_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub
    Private Sub PanelControl1_Paint(sender As Object, e As Windows.Forms.PaintEventArgs) Handles PanelControl1.Paint

    End Sub
    Private Sub uxq_EditValueChanged(sender As Object, e As EventArgs) Handles uxq.EditValueChanged

    End Sub

    Private Sub ppass_EditValueChanged(sender As Object, e As EventArgs) Handles ppass.EditValueChanged

    End Sub
End Class
