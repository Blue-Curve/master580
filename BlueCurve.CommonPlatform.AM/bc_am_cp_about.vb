Imports System.Windows.Forms

Public Class bc_am_cp_about

    Private Sub uxOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxOK.Click
        Me.Hide()
    End Sub

    Private Sub bc_am_cp_about_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub uxSystemInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxSystemInfo.Click
        MessageBox.Show(Environment.OSVersion.ToString(), "System Info", MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub
End Class