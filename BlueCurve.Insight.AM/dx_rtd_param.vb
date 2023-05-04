Public Class dx_rtd_param
    Public Event selbutton(ByRef baiul As dx_rtd_param)
    Public Event listchanged(ByRef baiul As dx_rtd_param)
    Public Event stextchanged(ByRef baiul As dx_rtd_param)
    Public delimit_value As Boolean = False
    Public list_id As Integer
    Public class_id As Long
    Public is_universe As Boolean = False
    Private Sub bsel_Click(sender As Object, e As EventArgs) Handles bsel.Click
        RaiseEvent selbutton(Me)

    End Sub

    Private Sub cvalue_KeyPress(sender As Object, e As Windows.Forms.KeyPressEventArgs) Handles cvalue.KeyPress
        RaiseEvent stextchanged(Me)
    End Sub

    Private Sub cvalue_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cvalue.SelectedIndexChanged
        RaiseEvent listchanged(Me)
    End Sub

End Class
