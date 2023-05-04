Public Class bc_dx_html_task_comment
    Implements Ibc_dx_html_task_comment

    Public Function load_view(tx As String, name As String) Implements Ibc_dx_html_task_comment.load_view
        Me.Text = "Blue Curve - Comment for Task: " + name

        Me.RichEditControl1.HtmlText = "<html><p>" + tx + "</p></html>"
        
        load_view = True
    End Function
    Private Sub bclose_Click(sender As Object, e As EventArgs) Handles bclose.Click
        Me.Hide()
    End Sub

    Private Sub RichEditControl1_Click(sender As Object, e As EventArgs) Handles RichEditControl1.Click

    End Sub
End Class
Public Class Cbc_dx_html_task_comment
    'WithEvents _view As Ibc_dx_html_task_comment


    Public Function load_data(view As Ibc_dx_html_task_comment, tx As String, name As String) As Boolean
        Return view.load_view(tx, name)
    End Function
End Class
Public Interface Ibc_dx_html_task_comment
    Function load_view(tx As String, name As String)
End Interface