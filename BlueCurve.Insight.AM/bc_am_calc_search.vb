Public Class bc_am_calc_search
    Public complete_list As New ArrayList
    Public Event results_ready(ByVal results_list As ArrayList)
    Private Sub bc_am_calc_search_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsearch.TextChanged
        If Trim(Me.tsearch.Text) = "" Then
            RaiseEvent results_ready(complete_list)
        Else
            Dim results_list As New ArrayList
            run_search(results_list)
            RaiseEvent results_ready(results_list)
        End If
    End Sub
    Private Sub run_search(ByRef results_list As ArrayList)
        For i = 0 To complete_list.Count - 1
            If InStr(" " + UCase(complete_list(i)), UCase(Me.tsearch.Text)) > 0 Then
                results_list.Add(complete_list(i))
            End If
        Next
    End Sub
End Class
