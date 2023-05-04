Imports DevExpress.XtraGrid

Public Class bc_am_pr_document_list

    Private ctrllr As bc_am_process
    Private last_mouse_move As Date
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.



        DevExpress.Skins.SkinManager.EnableFormSkins()
        DevExpress.UserSkins.BonusSkins.Register()
        DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle("Blue")

    End Sub

    Friend WriteOnly Property Controller() As bc_am_process

        Set(ByVal Value As bc_am_process)
            ctrllr = Value
        End Set

    End Property

    Private Sub uxDocumentListView_ColumnFilterChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles uxDocumentListView.ColumnFilterChanged
        ctrllr.selectDocument()
    End Sub

    Private Sub uxDocumentListView_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles uxDocumentListView.DoubleClick
        ctrllr.toggleshow()
    End Sub

  

    Private Sub uxDocumentListView_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles uxDocumentListView.KeyUp
        Select Case e.KeyCode
            Case 38, 40
                ctrllr.selectDocument()
            Case 116
                ctrllr.RetrieveDocs()
        End Select
    End Sub

    Private Sub uxDocumentListView_RowStyle(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs) Handles uxDocumentListView.RowStyle




        If Not ctrllr.docList Is Nothing And e.RowHandle <> -1 Then
            If uxDocumentListView.GetSelectedRows(0) = e.RowHandle Then
                e.Appearance.BackColor = bc_am_process.colors.doc_list_read_backcolor
                e.Appearance.ForeColor = bc_am_process.colors.doc_list_read_forecolor
                Exit Sub
            End If
            If ctrllr.docList(uxDocumentListView.GetDataSourceRowIndex(e.RowHandle)).Unread = True Then
                e.Appearance.BackColor = bc_am_process.colors.doc_list_unread_backcolor
                e.Appearance.ForeColor = bc_am_process.colors.doc_list_unread_forecolor
                Exit Sub
            Else
                e.Appearance.BackColor = System.Drawing.Color.White
                e.Appearance.ForeColor = System.Drawing.Color.Black

            End If
            If ctrllr.docList(uxDocumentListView.GetDataSourceRowIndex(e.RowHandle)).Stage_Change_Flag = True Then
                e.Appearance.BackColor = bc_am_process.colors.doc_list_stage_changed_backcolor
                e.Appearance.ForeColor = bc_am_process.colors.doc_list_stage_changed_forecolor
                Exit Sub
            End If
            If ctrllr.docList(uxDocumentListView.GetDataSourceRowIndex(e.RowHandle)).Urgent_Flag = True Then
                ctrllr.docList(uxDocumentListView.GetDataSourceRowIndex(e.RowHandle)).Expire_Flag = False
                e.Appearance.BackColor = bc_am_process.colors.doc_list_urgent_backcolor
                e.Appearance.ForeColor = bc_am_process.colors.doc_list_urgent_forecolor
                Exit Sub
            End If
            If ctrllr.docList(uxDocumentListView.GetDataSourceRowIndex(e.RowHandle)).Expire_Flag = True Then
                ctrllr.docList(uxDocumentListView.GetDataSourceRowIndex(e.RowHandle)).Urgent_Flag = False
                e.Appearance.BackColor = bc_am_process.colors.doc_list_expired_backcolor
                e.Appearance.ForeColor = bc_am_process.colors.doc_list_expired_forecolor
                Exit Sub
            End If





        End If


    End Sub

    Private Sub uxDocumentList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxDocumentList.Click
        ctrllr.selectDocument()
    End Sub


  

    Private Sub uxDocumentList_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles uxDocumentList.MouseMove
        If Not IsNothing(ctrllr) Then
            ctrllr.check_doc_list_inactivity()
        End If

    End Sub
End Class
