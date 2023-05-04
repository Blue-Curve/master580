Public Class bc_am_text_edit
    Public ok_selected As Boolean = False
    Public maxlength As Integer
    Public Initializing As Boolean = True

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        Initializing = False

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Hide()
    End Sub

    Private Sub Bok_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Bok.Click
        ok_selected = True
        Me.Hide()
    End Sub

    Private Sub ttextentry_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ttextentry.TextChanged

        Me.Llength.Text = "Maximum length: " + CStr(maxlength)
        If Me.ttextentry.Text <> "" Then
            Me.Llength.Text = CStr(Me.ttextentry.Text.Length) + " of " + CStr(maxlength)
        End If

        If Initializing = True Then
            Initializing = False
            Exit Sub
        End If

        Me.Bok.Enabled = True

    End Sub
End Class