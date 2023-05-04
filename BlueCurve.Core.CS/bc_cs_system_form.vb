Public Class bc_cs_system_form

    Private strSystemName As String
    Private boolAbortStartup As Boolean

    ReadOnly Property AbortStartup()
        Get
            Return boolAbortStartup
        End Get
    End Property


    ReadOnly Property SystemName()
        Get
            Return strSystemName
        End Get
    End Property

    Private Sub bc_cs_system_form_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If strSystemName Is Nothing Then
            boolAbortStartup = True
        End If
    End Sub

    Private Sub cbSystems_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cbSystems.KeyPress
        ReturnSystem()
    End Sub

    Private Sub ReturnSystem()
        strSystemName = Me.cbSystems.SelectedItem.ToString()
        Me.Close()
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        ReturnSystem()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        boolAbortStartup = True
        Environment.Exit(0)
        Me.Close()
    End Sub
End Class