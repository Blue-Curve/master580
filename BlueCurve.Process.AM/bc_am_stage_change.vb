Imports BlueCurve.Core.CS

Public Class bc_am_stage_change
    Public cancel_selected As Boolean = True

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
      
        If Me.Chkcompletion.Checked = True Then
            Dim da As Date
            da = Me.DateEdit1.DateTime.Date
            da = da.AddHours(uxtime.Time.Hour)
            da = da.AddMinutes(uxtime.Time.Minute)

            If da <= Now Then
                Dim omsg As New bc_cs_message("Blue Curve", "Completion Date Cannot Be in the Past", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Sub
            End If
        End If

        Me.cancel_selected = False
        Me.Hide()
    End Sub

    Private Sub Chkcompletion_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chkcompletion.CheckedChanged
        Me.DateEdit1.Enabled = False
        Me.uxtime.Enabled = False
        If Me.Chkcompletion.Checked Then
            Me.DateEdit1.Enabled = True
            Me.uxtime.Enabled = True
        End If
    End Sub

    Private Sub bc_am_stage_change_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.DateEdit1.Enabled = False
        Me.DateEdit1.DateTime = Now
        Me.uxtime.Enabled = False
        Me.uxtime.Time = Now



    End Sub

    Private Sub SimpleButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton2.Click
        Me.Hide()
    End Sub

    Private Sub DateEdit1_EditValueChanged(sender As Object, e As EventArgs) Handles DateEdit1.EditValueChanged

    End Sub
End Class