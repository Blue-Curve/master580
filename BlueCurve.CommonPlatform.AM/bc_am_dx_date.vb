Imports BlueCurve.Core.CS

Public Class bc_am_dx_date
    Public selected_date As Date
    Public ok_selected As Boolean = False
    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        Me.Hide()
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        Try
            selected_date = Me.DateEdit1.DateTime.Date
            If selected_date < DateAdd(DateInterval.Day, -1, Now) Then
                Dim omsg As New bc_cs_message("Blue Curve", "Date cannot be in the past.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Sub
            End If
            Me.Hide()
            ok_selected = True
        Catch
            selected_date = Now
        End Try
    End Sub
End Class