Imports System.Windows.Forms

Public Class bc_as_about

    Private Sub uxOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxOK.Click
        Me.Close()
    End Sub

    Public Sub SetProductAndVersion(ByVal strProduct As String, ByVal strVersion As String)

        uxProduct.Text = strProduct
        uxVersion.Text = strVersion
        Me.Text = "About " & strProduct

    End Sub

    Private Sub uxSystemInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxSystemInfo.Click
        MessageBox.Show(Environment.OSVersion.ToString(), "System Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

End Class