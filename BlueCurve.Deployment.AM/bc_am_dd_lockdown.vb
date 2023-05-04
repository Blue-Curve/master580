Public Class bc_am_dd_lockdown

    Private dt As DateTime
    Private strMessage As String
    Private boolCancelled As Boolean

    ReadOnly Property Cancelled() As Boolean
        Get
            Return boolCancelled
        End Get
    End Property

    ReadOnly Property LockTime() As DateTime
        Get
            Return dt
        End Get
    End Property

    ReadOnly Property Message() As String
        Get
            Return strMessage
        End Get
    End Property

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Dim strSelection As String = cbLockdown.SelectedItem.ToString
        If strSelection = "Now" Then
            dt = DateTime.Now
        ElseIf strSelection = "15 Minutes" Then
            dt = DateTime.Now.Add(TimeSpan.FromMinutes(15))
        ElseIf strSelection = "30 Minutes" Then
            dt = DateTime.Now.Add(TimeSpan.FromMinutes(30))
        ElseIf strSelection = "1 Hour" Then
            dt = DateTime.Now.Add(TimeSpan.FromHours(1))
        ElseIf strSelection = "2 Hours" Then
            dt = DateTime.Now.Add(TimeSpan.FromHours(2))
        ElseIf strSelection = "4 Hours" Then
            dt = DateTime.Now.Add(TimeSpan.FromHours(4))
        Else
            'Error
        End If
        strMessage = txtLock.Text
        Me.Close()
    End Sub

    Private Sub bc_am_dd_lockdown_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cbLockdown.SelectedIndex = 0
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        boolCancelled = True
    End Sub
End Class