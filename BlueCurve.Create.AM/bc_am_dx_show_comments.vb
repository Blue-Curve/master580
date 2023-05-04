Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM

Imports DevExpress.XtraTreeList


Public Class bc_am_dx_show_comments
    Implements Ibc_am_dx_show_comments



    Event get_comments(range As Integer) Implements Ibc_am_dx_show_comments.get_comments


    Public Function load_start(title As String) Implements Ibc_am_dx_show_comments.load_start
        Me.ltitle.Text = title
        Me.RadioGroup1.SelectedIndex = 1
        load_start = True

    End Function
    Public Function load_view(comments As List(Of bc_om_comment)) As Object Implements Ibc_am_dx_show_comments.load_view
        Try
            uxcommentlist.BeginUnboundLoad()
            uxcommentlist.Nodes.Clear()

            For i = 0 To comments.Count - 1

                Dim tln As Nodes.TreeListNode = Nothing
                If i > 0 Then
                    uxcommentlist.AppendNode(New Object() {comments(i).user_name + " (" + comments(i).stage_name + ") " + Format(comments(i).da.ToLocalTime, "dd-MMM-yyyy HH:mm")}, tln).Tag = "user"
                    uxcommentlist.AppendNode(New Object() {comments(i).comment}, uxcommentlist.Nodes(i)).Tag = "comment"
                Else
                    uxcommentlist.AppendNode(New Object() {comments(i).user_name + " (" + comments(i).stage_name + ") " + Format(comments(i).da.ToLocalTime, "dd-MMM-yyyy HH:mm")}, -1, -1, -1, 2).Tag = "topuser"
                    uxcommentlist.AppendNode(New Object() {comments(i).comment}, i * 2, -1, -1, 3).Tag = "topcomment"

                End If
                uxcommentlist.Nodes(uxcommentlist.Nodes.Count - 1).StateImageIndex = 12
                uxcommentlist.Nodes(uxcommentlist.Nodes.Count - 1).Nodes(0).StateImageIndex = 5


            Next
            uxcommentlist.EndUnboundLoad()
            If comments.Count > 0 Then
                uxcommentlist.Nodes(0).Expanded = True
            End If



            load_view = True

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_dx_show_comments", "load_view", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Function

    Private Sub bcancel_Click(sender As Object, e As EventArgs) Handles bcancel.Click
        Me.Hide()
    End Sub

    Private Sub RadioGroup1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RadioGroup1.SelectedIndexChanged
        Me.Cursor = Windows.Forms.Cursors.WaitCursor


        Select Case RadioGroup1.SelectedIndex
            Case 0
                RaiseEvent get_comments(-1)
            Case 1
                RaiseEvent get_comments(1)
            Case 2
                RaiseEvent get_comments(7)
            Case 3
                RaiseEvent get_comments(30)
            Case 4
                RaiseEvent get_comments(-2)
        End Select
        Me.Cursor = Windows.Forms.Cursors.Default

    End Sub
    Private Sub uxcomments_NodeCellStyle(ByVal sender As Object, ByVal e As DevExpress.XtraTreeList.GetCustomNodeCellStyleEventArgs) Handles uxcommentlist.NodeCellStyle


        Select Case e.Node.Tag
            Case "top"
                e.Appearance.ForeColor = System.Drawing.Color.Gray
            Case "topuser"
                e.Appearance.ForeColor = System.Drawing.Color.White
                e.Appearance.BackColor = System.Drawing.Color.Black
            Case "topcomment"
                e.Appearance.ForeColor = System.Drawing.Color.Black
            Case Else
                e.Appearance.ForeColor = System.Drawing.Color.Gray

        End Select
    End Sub
End Class
Public Class Cbc_am_dx_show_comments
    WithEvents _view As Ibc_am_dx_show_comments
    Dim _doc_Id As Long
    Dim _comments As New bc_om_comments

    Public Function load_data(doc_id As Long, title As String, view As Ibc_am_dx_show_comments) As Boolean
        Try
            load_data = False
            _doc_Id = doc_id
            _view = view
            _comments.doc_id = _doc_Id
            If _view.load_start(title) = False Then
                Exit Function
            End If

            load_data = True
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Ibc_am_dx_show_comments", "load_data", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Function
    Public Sub get_comments(range As Integer) Handles _view.get_comments
        load_comments(range)
    End Sub
    Public Function load_comments(range As Integer)

        Try
            load_comments = False
            _comments.span = range
            _comments.comments.Clear()
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                _comments.db_read()
            Else
                _comments.tmode = bc_cs_soap_base_class.tREAD



                If _comments.transmit_to_server_and_receive(_comments, True) = False Then
                    Exit Function
                End If
            End If
            If _view.load_view(_comments.comments) = False Then
                Exit Function
            End If
            load_comments = True
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Ibc_am_dx_show_comments", "get_comments", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Function
End Class
Public Interface Ibc_am_dx_show_comments
    Function load_view(comments As List(Of bc_om_comment))
    Function load_start(title As String)
    Event get_comments(range As Integer)

End Interface
