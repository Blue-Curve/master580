REM PR new DEV express comments control
imports BlueCurve.Core.cs
imports BlueCurve.Core.om
REM cant get this to work pn x86 for tjr time being
Public Class bc_am_dx_comments
    
    Public Const MOST_RECENT = 1
    Public Const ALL = 2

    Private ldoc_id As Long
    Private lspan As Integer


    Public Property doc_Id()
        Get
            Return ldoc_id
        End Get
        Set(ByVal value)
            ldoc_id = value
        End Set
    End Property

    Public Sub load_comments(ByVal span)
        span = lspan
    End Sub
    Private Sub load_comments_from_system()
        Dim ocomments As New bc_om_comments
        ocomments.doc_id = ldoc_id
        ocomments.span = lspan
        If bc_cs_central_settings.selected_conn_method = "ADO" Then
            ocomments.db_read()
        Else
            ocomments.tmode = bc_cs_soap_base_class.tREAD


            If ocomments.transmit_to_server_and_receive(ocomments, True) = False Then
                Exit Sub
            End If
        End If
        populate_comments(ocomments.comments)
    End Sub
    Private Sub populate_comments(ByVal comments As List(Of bc_om_comment))
        Me.ustcomments.Nodes.Clear()
        For i = 0 To comments.Count - 1
            Me.ustcomments.Nodes.Add(comments(i).user_name)
            Me.ustcomments.Nodes(0).Nodes.Add(comments(i).comment)
        Next

    End Sub
    Private Sub uxbuttonclear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxbuttonclear.Click
        Me.uxComment.Text = ""
    End Sub
    Private Sub uxsubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxbtnsubmit.Click
        Dim ocomment As New bc_om_comment
        ocomment.doc_id = ldoc_id
        ocomment.comment = Me.uxComment.Text
        If bc_cs_central_settings.selected_conn_method = "ADO" Then
            ocomment.db_write()
        Else
            ocomment.tmode = bc_cs_soap_base_class.tWRITE
            If ocomment.transmit_to_server_and_receive(ocomment, True) = False Then
                Exit Sub
            End If
        End If

        Me.uxComment.Text = ""
    End Sub

    Private Sub uxComment_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxComment.EditValueChanged
        Me.uxbtnsubmit.Enabled = False
        If LTrim(RTrim(uxComment.Text)) <> "" Then
            Me.uxbtnsubmit.Enabled = True
        End If
    End Sub


End Class
