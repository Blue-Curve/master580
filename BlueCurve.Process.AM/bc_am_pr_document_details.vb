


Public Class bc_am_pr_document_details

    Private ctrllr As bc_am_process

    'Private Sub uxWorkflowTree_AfterFocusNode(ByVal sender As Object, ByVal e As DevExpress.XtraTreeList.NodeEventArgs) Handles uxWorkflowTree.AfterFocusNode
    '    ctrllr.stageSelect()
    'End Sub

    'Private Sub uxWorkflowTree_AfterFocusNode(ByVal sender As Object, ByVal e As DevExpress.XtraTreeList.NodeEventArgs) Handles uxWorkflowTree.MouseClick

    '    ctrllr.stageSelect()
    'End Sub

    Friend WriteOnly Property Controller() As bc_am_process

        Set(ByVal Value As bc_am_process)
            ctrllr = Value
        End Set

    End Property

    Private Sub uxWorkflowTree_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles uxWorkflowTree.MouseClick
        ctrllr.stageSelect()
    End Sub

    Private Sub uxWorkflowTree_NodeCellStyle(ByVal sender As Object, ByVal e As DevExpress.XtraTreeList.GetCustomNodeCellStyleEventArgs) Handles uxWorkflowTree.NodeCellStyle
        Select Case e.Node.StateImageIndex
            Case 1
                e.Appearance.ForeColor = bc_am_process.colors.workflow_current_stage_forecolor
                e.Appearance.BackColor = bc_am_process.colors.workflow_current_stage_backcolor
            Case 24
                e.Appearance.ForeColor = bc_am_process.colors.workflow_next_stage_forecolor
                e.Appearance.BackColor = bc_am_process.colors.workflow_next_stage_backcolor
            Case Else
                e.Appearance.ForeColor = System.Drawing.Color.Gray
        End Select
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


    Private Sub uxbuttonclear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.uxComment.Text = ""

    End Sub

    Private Sub uxbtnsubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxbtnsubmit.Click
        ctrllr.submitcomment(Me.uxComment.Text)
        Me.RadioGroup1.SelectedIndex = 1


        Me.Hlatest.Focus()
        Me.uxComment.Text = ""
    End Sub
    Private Sub uxComment_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxComment.EditValueChanged
        Me.uxbtnsubmit.Enabled = False
        If RTrim(LTrim(Me.uxComment.Text) <> "") Then
            Me.uxbtnsubmit.Enabled = True
        End If
    End Sub

    Private Sub HyperLinkEdit2_OpenLink(ByVal sender As System.Object, ByVal e As DevExpress.XtraEditors.Controls.OpenLinkEventArgs) Handles HyperLinkEdit2.OpenLink
        ctrllr.load_document_comments_from_system(1)
    End Sub

    Private Sub HyperLinkEdit5_OpenLink(ByVal sender As System.Object, ByVal e As DevExpress.XtraEditors.Controls.OpenLinkEventArgs) Handles HyperLinkEdit5.OpenLink
        ctrllr.load_document_comments_from_system(7)
    End Sub

    Private Sub HyperLinkEdit1_OpenLink(ByVal sender As System.Object, ByVal e As DevExpress.XtraEditors.Controls.OpenLinkEventArgs) Handles HyperLinkEdit1.OpenLink
        ctrllr.load_document_comments_from_system(31)
    End Sub

    Private Sub HyperLinkEdit3_OpenLink(ByVal sender As System.Object, ByVal e As DevExpress.XtraEditors.Controls.OpenLinkEventArgs)
        ctrllr.load_document_comments_from_system(90)
    End Sub

    Private Sub HyperLinkEdit4_OpenLink(ByVal sender As System.Object, ByVal e As DevExpress.XtraEditors.Controls.OpenLinkEventArgs) Handles HyperLinkEdit4.OpenLink
        ctrllr.load_document_comments_from_system(-2)
    End Sub
    Friend Sub linkAction(ByVal sender As DevExpress.XtraNavBar.NavBarItem, ByVal e As DevExpress.XtraNavBar.NavBarLinkEventArgs)
        ctrllr.invokelink(e.Link.Item.Tag)
    End Sub
    Private Sub lvwrevisions_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvwrevisions.Click
        If lvwrevisions.Selection.Count = 1 Then
            ctrllr.selectdocumentrevision(lvwrevisions.Selection.Item(0).Tag)
        End If
    End Sub


    Private Sub uxsupportdocs__Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles uxsupportdocs.Click
        If uxsupportdocs.Selection.Count = 1 Then
            ctrllr.selectsupportdocument(uxsupportdocs.Selection.Item(0).Tag)
        End If
    End Sub

    Private Sub uxViewRevision_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxViewRevision.Click
        Me.Cursor = Windows.Forms.Cursors.WaitCursor
        ctrllr.viewrevision(lvwrevisions.Selection.Item(0).Tag)
        Me.Cursor = Windows.Forms.Cursors.Default
    End Sub

    Private Sub uxRevertRevision_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxRevertRevision.Click
        Me.Cursor = Windows.Forms.Cursors.WaitCursor
        ctrllr.revertdocument(lvwrevisions.Selection.Item(0).Tag)
        Me.Cursor = Windows.Forms.Cursors.Default
    End Sub
    Private Sub uxViewSupportDocument_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxViewSupportDocument.Click
        Me.Cursor = Windows.Forms.Cursors.WaitCursor

        If uxViewSupportDocument.Text = "View" Then
            ctrllr.viewsupportdoc(uxsupportdocs.Selection.Item(0).Tag)
        Else
            ctrllr.editsupportdoc(uxsupportdocs.Selection.Item(0).Tag)
        End If
        Me.Cursor = Windows.Forms.Cursors.Default
    End Sub

    Private Sub uxCheckInOutSupportDoc_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxCheckInOutSupportDoc.CheckedChanged
        Me.Cursor = Windows.Forms.Cursors.WaitCursor
        If uxCheckInOutSupportDoc.Text = "Check Out" Then
            ctrllr.checkoutSupportDoc(uxsupportdocs.Selection.Item(0).Tag)
        Else
            ctrllr.checkInSupportDoc(uxsupportdocs.Selection.Item(0).Tag)
        End If
        Me.Cursor = Windows.Forms.Cursors.Default

    End Sub

    Private Sub uxcatsupportdoc_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxcatsupportdoc.CheckedChanged
        Me.Cursor = Windows.Forms.Cursors.WaitCursor
        ctrllr.changesupportcategory(uxsupportdocs.Selection.Item(0).Tag)
        Me.Cursor = Windows.Forms.Cursors.Default
    End Sub
    Private Sub uxDeleteSupportDoc_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxDeleteSupportDoc.CheckedChanged
        REM Steve Wooderson 24/10/2013 Delete support doc
        Me.Cursor = Windows.Forms.Cursors.WaitCursor
        ctrllr.delete_supporting_document(uxsupportdocs.Selection.Item(0).Tag)
        Me.Cursor = Windows.Forms.Cursors.Default
    End Sub

    Private Sub uxsupportdocs_NodeChanged(ByVal sender As Object, ByVal e As DevExpress.XtraTreeList.NodeChangedEventArgs) Handles uxsupportdocs.NodeChanged
        Try
            ctrllr.selectsupportdocument(uxsupportdocs.Selection.Item(0).Tag)
        Catch
        End Try
    End Sub
    Private Sub lvwrevisions_NodeChanged(ByVal sender As Object, ByVal e As DevExpress.XtraTreeList.NodeChangedEventArgs) Handles lvwrevisions.NodeChanged
        Try
            ctrllr.selectdocumentrevision(lvwrevisions.Selection.Item(0).Tag)
        Catch
        End Try
    End Sub


    Private Sub Hlatest_OpenLink(ByVal sender As System.Object, ByVal e As DevExpress.XtraEditors.Controls.OpenLinkEventArgs) Handles Hlatest.OpenLink
        ctrllr.load_document_comments_from_system(-1)
    End Sub


    Private Sub uxsupportdocs_FocusedNodeChanged(ByVal sender As System.Object, ByVal e As DevExpress.XtraTreeList.FocusedNodeChangedEventArgs) Handles uxsupportdocs.FocusedNodeChanged

    End Sub

    Private Sub uxhistorylist_FocusedNodeChanged(ByVal sender As System.Object, ByVal e As DevExpress.XtraTreeList.FocusedNodeChangedEventArgs) Handles uxhistorylist.FocusedNodeChanged

    End Sub

    Private Sub uxlinks_Click(sender As Object, e As EventArgs) Handles uxlinks.Click

    End Sub

    Private Sub uxSupportDocumentsGroup_Paint(sender As Object, e As Windows.Forms.PaintEventArgs) Handles uxSupportDocumentsGroup.Paint

    End Sub

    Private Sub GroupControl2_Paint(sender As Object, e As Windows.Forms.PaintEventArgs)

    End Sub



    Private Sub LabelControl13_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub uxWorkflowTree_FocusedNodeChanged(sender As Object, e As DevExpress.XtraTreeList.FocusedNodeChangedEventArgs) Handles uxWorkflowTree.FocusedNodeChanged

    End Sub

    Private Sub RadioGroup1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RadioGroup1.SelectedIndexChanged
        Select Case RadioGroup1.SelectedIndex
            Case 0
                ctrllr.load_document_comments_from_system(-1)
            Case 1
                ctrllr.load_document_comments_from_system(1)
            Case 2
                ctrllr.load_document_comments_from_system(7)
            Case 3
                ctrllr.load_document_comments_from_system(30)
            Case 4
                ctrllr.load_document_comments_from_system(-2)
        End Select
    End Sub

    Private Sub LabelControl6_Click(sender As Object, e As EventArgs) Handles LabelControl6.Click

    End Sub

    Private Sub uxKeyInfoPanels_Paint(sender As Object, e As Windows.Forms.PaintEventArgs) Handles uxKeyInfoPanels.Paint

    End Sub

    Private Sub btax_Click(sender As Object, e As EventArgs) Handles btax.Click
        Me.Cursor = Windows.Forms.Cursors.WaitCursor
        ctrllr.display_tax()
        Me.Cursor = Windows.Forms.Cursors.Default

    End Sub
End Class