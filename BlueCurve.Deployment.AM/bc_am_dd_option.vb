Public Class bc_am_dd_option

    Friend result As Object

    Public Sub New(ByVal strText As String, ByVal o1 As Object, ByVal o2 As Object, ByVal o3 As Object)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        Me.Text = strText

        ' Add any initialization after the InitializeComponent() call.
        rb1.Text = o1.ToString
        rb1.Tag = o1
        rb2.Text = o2.ToString
        rb2.Tag = o2
        'rb3.Text = o3.ToString
        'rb3.Tag = o3

    End Sub

    Private Sub rb_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rb1.CheckedChanged, rb2.CheckedChanged
        result = sender.tag
        Me.Close()
    End Sub

    Private Sub bc_am_dd_save_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'rb3.TabStop = False
        rb2.TabStop = False
        rb1.TabStop = False
        'rb3.Checked = False
        rb2.Checked = False
        rb1.Checked = False
    End Sub
End Class