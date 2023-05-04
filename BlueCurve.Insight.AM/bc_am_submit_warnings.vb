Imports BlueCurve.Core.CS
Public Class bc_am_submit_warnings
    Public oexcel As bc_ao_in_excel

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Hide()
    End Sub

    Private Sub Lvwarnings_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Lvwarnings.DoubleClick
        goto_cell()
    End Sub

    Private Sub Lvwarnings_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Lvwarnings.SelectedIndexChanged
        Me.Bgoto.Enabled = False

        If Me.Lvwarnings.SelectedItems.Count = 1 Then
            If Me.Lvwarnings.SelectedItems(0).SubItems(1).Text = "Data" Or Me.Lvwarnings.SelectedItems(0).SubItems(1).Text = "Warning" Then
                Me.Bgoto.Enabled = True
            End If
        End If
    End Sub
    Private Sub Bgoto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Bgoto.Click
        goto_cell()
    End Sub
    Private Sub goto_cell()
        Try
            oexcel.highlight_cell(Me.Lvwarnings.SelectedItems(0).Text, CInt(Me.Lvwarnings.SelectedItems(0).SubItems(3).Text), CInt(Me.Lvwarnings.SelectedItems(0).SubItems(4).Text), "", True)
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_submit_warnings", "goto_cell", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
End Class