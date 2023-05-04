Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports BlueCurve.Core.AS
Imports DevExpress.XtraTreeList
Imports System.Drawing
Imports System.Windows.Forms

Public Class bc_dx_doc_open
    Implements Ibc_dx_doc_open
    Dim _docs As New bc_om_documents
    Dim _ctrl As Cbc_dx_doc_Open
    Dim bloading As Boolean = True

    Event open_doc(ByVal doc_id As String, ByVal mode As Boolean) Implements Ibc_dx_doc_open.open_doc
    Public Sub New()

        DevExpress.Skins.SkinManager.EnableFormSkins()
        DevExpress.UserSkins.BonusSkins.Register()
        DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle("Blue")
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
   
    Public Sub fhide() Implements Ibc_dx_doc_open.fhide
        Me.Hide()
    End Sub
    Public Sub fminimize() Implements Ibc_dx_doc_open.fminimize
        Me.WindowState = FormWindowState.Minimized
    End Sub

    Public Sub refresh_view() Implements Ibc_dx_doc_open.refresh_view
        Try
            If bloading = True Then
                Exit Sub
            End If
            Me.Cursor = Cursors.WaitCursor

            If rsource.SelectedIndex = 0 Then
                _docs = _ctrl.load_data(False, True, uxpublish.CheckState)
            ElseIf rsource.SelectedIndex = 1 Then
                _docs = _ctrl.load_data(False, False, uxpublish.CheckState)
            Else
                _docs = _ctrl.load_data(True, False, uxpublish.CheckState)
            End If
            load_view()
        Catch

        Finally
            Me.Cursor = Cursors.Default

        End Try
    End Sub


    Public Sub wait_cursor() Implements Ibc_dx_doc_open.wait_cursor
        Me.Cursor = Cursors.WaitCursor
    End Sub
    Public Sub default_cursor() Implements Ibc_dx_doc_open.default_cursor
        Me.Cursor = Cursors.Default
    End Sub

    Public Sub load_doc_list(ByVal docs As bc_om_documents, ByVal ctrl As Cbc_dx_doc_Open) Implements Ibc_dx_doc_open.load_doc_list
        bloading = True
        _ctrl = ctrl
        _docs = docs
        load_view()
    End Sub
    Private Delegate Sub load_viewDelegate()

    Private Sub load_view()
        Try
            REM if called from poll marshall to UI thread
            'If Me.uxdocs.InvokeRequired Then
            '    Dim ed As New load_viewDelegate(AddressOf load_view)
            '    Me.Invoke(ed, New Object() {False, False, True})
            '    Exit Sub
            'End If


            Me.Cursor = Cursors.WaitCursor




            Dim tln As Nodes.TreeListNode = Nothing
            Dim bcs As New bc_cs_icon_services
            Dim image As Bitmap
            For i = 4 To Me.uxDLImageList.Images.Count - 1
                Me.uxDLImageList.Images.RemoveAt(4)
            Next

            uxdocs.BeginUpdate()
            Try
                uxdocs.ClearNodes()
            Catch
            End Try



            If IsNothing(_docs) Then
                Exit Sub
            End If

            Dim n As DevExpress.XtraTreeList.Nodes.TreeListNode
            Me.bok.Enabled = False
            For i = 0 To _docs.document.Count - 1
                Me.bok.Enabled = True

                With _docs.document(i)
                    n = uxdocs.AppendNode(New Object() {.doc_date.tolocaltime}, tln)

                    n.Tag = CStr(.filename)
                    If .entity_id = 0 Then
                        n.SetValue(1, "none")
                    Else
                        n.SetValue(1, .lead_entity_name)
                    End If
                    If .id = 0 Then
                        n.SetValue(2, "Unsubmitted")
                    Else
                        n.SetValue(2, .stage_name)
                    End If
                    n.SetValue(3, .title)
                    n.SetValue(4, .pub_type_name)
                    n.SetValue(5, .originator_name)
                    n.SetValue(6, .bus_area)
                    If .checked_out_user = bc_cs_central_settings.logged_on_user_id Then
                        n.SetValue(7, "me")
                        n.StateImageIndex = 1
                    ElseIf .checked_out_user = 0 Then
                        image = Nothing
                        image = bcs.get_icon_for_file_type(.extension)
                        If Not IsNothing(image) Then
                            Me.uxDLImageList.Images.Add(image)
                            n.StateImageIndex = Me.uxDLImageList.Images.Count - 1
                        Else
                            n.StateImageIndex = 0
                        End If
                        If .id = 0 Then
                            n.SetValue(7, "n/a")
                        Else
                            n.SetValue(7, "in")
                        End If
                    Else
                        n.SetValue(7, .checked_out_user_name)
                        n.StateImageIndex = 2
                    End If
                    n.SetValue(8, 5)
                End With

            Next
            uxdocs.Refresh()


        Catch ex As Exception
            Dim err As New bc_cs_error_log("bc_dx_doc_open", "load_view", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            bloading = False
            uxdocs.EndUpdate()
            Me.Cursor = Windows.Forms.Cursors.Default
        End Try


    End Sub
    Public Sub load_filter_values() Implements Ibc_dx_doc_open.load_filter_values
        Try
            Me.uxfrom.DateTime = Cbc_dx_doc_Open.gdopen_date_from
            If Cbc_dx_doc_Open.gdopen_date_to = "9-9-9999" Then
                Me.uxto.DateTime = Now
                Me.uxcurrent.CheckState = CheckState.Checked
                Me.uxto.Enabled = False
            Else
                Me.uxcurrent.CheckState = CheckState.Unchecked
                Me.uxto.Enabled = True
                Me.uxto.DateTime = Cbc_dx_doc_Open.gdopen_date_to
            End If
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.LOCAL Then
                Me.rsource.Properties.Items(0).Enabled = False
                Me.rsource.Properties.Items(1).Enabled = False
                Me.rsource.SelectedIndex = 2
            Else
                Me.rsource.SelectedIndex = 0
            End If


        Catch ex As Exception
            MsgBox(ex.Message)


        End Try
    End Sub
    Private Sub bcancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bcancel.Click
        Me.Hide()
    End Sub

    Private Sub bok_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bok.Click
        If Me.rsource.SelectedIndex = 2 Then
            RaiseEvent open_doc(Me.uxdocs.Selection.Item(0).Tag, True)
        Else
            RaiseEvent open_doc(Me.uxdocs.Selection.Item(0).Tag, False)
        End If
    End Sub



    Private Sub uxpublish_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles uxpublish.CheckStateChanged

        If rsource.SelectedIndex = 0 Then
            _docs = _ctrl.load_data(False, True, uxpublish.CheckState)
        ElseIf rsource.SelectedIndex = 1 Then
            _docs = _ctrl.load_data(False, False, uxpublish.CheckState)
        Else
            _docs = _ctrl.load_data(True, False, uxpublish.CheckState)
        End If

        load_view()
    End Sub

    Private Sub uxfrom_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxfrom.EditValueChanged
        Try
            If bloading = True Then
                Exit Sub
            End If
            Me.Cursor = Cursors.WaitCursor

            Cbc_dx_doc_Open.gdopen_date_from = Me.uxfrom.DateTime
            If rsource.SelectedIndex = 0 Then
                _docs = _ctrl.load_data(False, True, uxpublish.CheckState)
            ElseIf rsource.SelectedIndex = 1 Then
                _docs = _ctrl.load_data(False, False, uxpublish.CheckState)
            Else
                _docs = _ctrl.load_data(True, False, uxpublish.CheckState)
            End If
            load_view()
        Finally
            Me.Cursor = Cursors.Default

        End Try
    End Sub
    Private Sub uxto_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxto.EditValueChanged
        Try
            If bloading = True Then
                Exit Sub
            End If
            Me.Cursor = Cursors.WaitCursor


            Cbc_dx_doc_Open.gdopen_date_to = Me.uxto.DateTime
            If rsource.SelectedIndex = 0 Then
                _docs = _ctrl.load_data(False, True, uxpublish.CheckState)
            ElseIf rsource.SelectedIndex = 1 Then
                _docs = _ctrl.load_data(False, False, uxpublish.CheckState)
            Else
                _docs = _ctrl.load_data(True, False, uxpublish.CheckState)
            End If

            load_view()
        Catch

        Finally
            Me.Cursor = Cursors.Default

        End Try
    End Sub

    Private Sub uxcurrent_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxcurrent.CheckStateChanged
        Try
            Me.Cursor = Cursors.WaitCursor

            If uxcurrent.CheckState = CheckState.Unchecked Then
                Me.uxto.Enabled = True
            Else
                Me.uxto.Enabled = False
                Me.uxto.DateTime = Now
                Cbc_dx_doc_Open.gdopen_date_to = "9-9-9999"

            End If
        Finally
            Me.Cursor = Cursors.Default

        End Try
    End Sub

    Private Sub uxdocs_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles uxdocs.DoubleClick
        If check_open_doc() = True Then
            If Me.rsource.SelectedIndex = 2 Then
                RaiseEvent open_doc(Me.uxdocs.Selection.Item(0).Tag, True)
            Else
                RaiseEvent open_doc(Me.uxdocs.Selection.Item(0).Tag, False)
            End If
        End If
    End Sub

    Private Sub uxdocs_FocusedNodeChanged(ByVal sender As Object, ByVal e As DevExpress.XtraTreeList.FocusedNodeChangedEventArgs) Handles uxdocs.FocusedNodeChanged
        Try
            check_open_doc()
        Catch

        End Try
    End Sub


    Private Sub uxdocs_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles uxdocs.Click
        Try
            check_open_doc()
        Catch

        End Try

    End Sub
    Private Function check_open_doc() As Boolean
        Try



            Me.bok.Enabled = False
            Me.uxcomments.Enabled = False
            Dim doc_id As String
            Try
                doc_id = Me.uxdocs.Selection.Item(0).Tag.ToString.Substring(0, Len(Me.uxdocs.Selection.Item(0).Tag.ToString) - 4)

                If IsNumeric(doc_id) Then
                    Me.uxcomments.Enabled = True
                End If
            Catch

            End Try
            check_open_doc = True
            If Me.uxdocs.Selection.Item(0).StateImageIndex = 2 Then
                check_open_doc = False
                Exit Function
            End If
            If Me.uxdocs.Selection.Item(0).Tag = "" Then
                check_open_doc = False
                Exit Function
            End If
            Me.bok.Enabled = True
        Catch

        End Try
    End Function

    Private Sub rsource_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rsource.SelectedIndexChanged
        Try
            If bloading = True Then
                Exit Sub
            End If
            Me.Cursor = Cursors.WaitCursor


            If rsource.SelectedIndex = 0 Then
                _docs = _ctrl.load_data(False, True, uxpublish.CheckState)
            ElseIf rsource.SelectedIndex = 1 Then
                _docs = _ctrl.load_data(False, False, uxpublish.CheckState)
            Else
                   _docs = _ctrl.load_data(True, False, uxpublish.CheckState)
            End If
            load_view()
        Catch

        Finally
            Me.Cursor = Cursors.Default

        End Try

    End Sub

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        refresh_view()

    End Sub


    Private Sub uxcomments_Click(sender As Object, e As EventArgs) Handles uxcomments.Click
        Dim f As New bc_am_dx_show_comments
        Dim c As New Cbc_am_dx_show_comments
        Dim doc_id As String
        doc_id = Me.uxdocs.Selection.Item(0).Tag.ToString.Substring(0, Len(Me.uxdocs.Selection.Item(0).Tag.ToString) - 4)
       
        Me.Cursor = Cursors.WaitCursor


        If c.load_data(doc_id, Me.uxdocs.Selection.Item(0).GetValue(3), f) = True Then
            f.TopMost = True
            Me.Cursor = Cursors.Default
            f.ShowDialog()
        End If
        Me.Cursor = Cursors.Default

    End Sub
End Class
Public Class Cbc_dx_doc_Open
    WithEvents _view As Ibc_dx_doc_open
    Dim _docs As New bc_om_documents
    Public doc_opened As Boolean = False
    Public Shared gdopen_date_from As Date = "1-1-1900"
    Public Shared gdopen_date_to As Date = "9-9-9999"
    Public Sub New(ByVal view As Ibc_dx_doc_open)
        _view = view
    End Sub

    Public Function load_data(ByVal local As Boolean, ByVal all As Boolean, ByVal publish As Boolean) As bc_om_documents
        Try
            _view.wait_cursor()

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.LOCAL Then
                local = True
            End If

            If local = False Then
                _docs.workflow_mode = False
                If publish = False Then
                    _docs.show_publish = "False"
                Else
                    _docs.show_publish = "True"
                End If
                _docs.all = all
                _docs.date_from = gdopen_date_from

                _docs.date_to = gdopen_date_to

                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    _docs.db_read()
                Else
                    _docs.tmode = bc_cs_soap_base_class.tREAD
                    If _docs.transmit_to_server_and_receive(_docs, True) = False Then
                        Exit Function
                    End If
                End If
            Else
                Dim fs As New bc_cs_file_transfer_services
                _docs.document.Clear()
                If fs.check_document_exists_and_can_be_read(bc_cs_central_settings.local_repos_path + "bc_local_documents.dat") = True Then
                    _docs = _docs.read_data_from_file(bc_cs_central_settings.local_repos_path + "bc_local_documents.dat")
                    REM apply date filter
                    Dim i As Integer
                    Dim df As DateTime
                    Dim dt As DateTime
                    If InStr(gdopen_date_from, " ") > 0 Then
                        df = Left(gdopen_date_from, InStr(gdopen_date_from, " ")) + " 00:00"
                    Else
                        df = gdopen_date_from
                    End If
                    If InStr(gdopen_date_to, " ") > 0 Then
                        dt = Left(gdopen_date_to, InStr(gdopen_date_to, " ")) + " 23:59"
                    Else
                        dt = gdopen_date_to
                    End If

                    While i < _docs.document.Count
                        If _docs.document(i).doc_date < df Or _docs.document(i).doc_date > dt Then
                            _docs.document.RemoveAt(i)
                        ElseIf _docs.document(i).stage_name = "Publish" And publish = False Then
                            _docs.document.RemoveAt(i)
                        Else
                            For j = 0 To bc_am_load_objects.obc_pub_types.bus_areas.Count - 1
                                If bc_am_load_objects.obc_pub_types.bus_areas(j).id = _docs.document(i).bus_area Then
                                    _docs.document(i).bus_area = bc_am_load_objects.obc_pub_types.bus_areas(j).description
                                    Exit For
                                End If
                            Next
                            If _docs.document(i).entity_Id <> 0 Then
                                For j = 0 To bc_am_load_objects.obc_entities.entity.Count - 1
                                    If _docs.document(i).entity_Id = bc_am_load_objects.obc_entities.entity(j).id Then
                                        _docs.document(i).lead_entity_name = bc_am_load_objects.obc_entities.entity(j).name
                                        Exit For
                                    End If
                                Next
                            End If
                            For j = 0 To bc_am_load_objects.obc_users.user.Count - 1
                                If bc_am_load_objects.obc_users.user(j).id = _docs.document(i).originating_author Then
                                    _docs.document(i).originator_name = bc_am_load_objects.obc_users.user(j).first_name + " " + bc_am_load_objects.obc_users.user(j).surname
                                    Exit For
                                End If
                            Next

                            i = i + 1
                        End If
                    End While
                End If
                End If


        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Cbc_dx_doc_Open", "load_date", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            load_data = _docs
        End Try

    End Function
    Public Sub open_doc(ByVal doc_id As String, ByVal mode As Boolean) Handles _view.open_doc


        Try
            Dim odoc As New bc_am_at_open_doc
            doc_opened = False
            _view.wait_cursor()
            If mode = False Then
                For i = 0 To Me._docs.document.Count - 1
                    If Me._docs.document(i).filename = doc_id Then
                        If Me._docs.document(i).checked_out_user = 0 Then
                            REM checked in
                            If odoc.open(False, Me._docs.document(i), True) = True Then
                                If odoc.recent_change = False Then
                                    doc_opened = True
                                End If
                            End If
                        Else
                            REM checked out to me
                            If odoc.open(True, Me._docs.document(i), True) = True Then
                                doc_opened = True
                            End If
                        End If
                        Exit For
                    End If
                Next
            Else
                For i = 0 To Me._docs.document.Count - 1
                    If Me._docs.document(i).filename = doc_id Then
                        If odoc.open(True, Me._docs.document(i), True) = True Then
                            doc_opened = True

                        End If
                        Exit For
                    End If
                Next

            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("Cbc_dx_doc_Open", "open_doc", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            _view.default_cursor()
            If doc_opened = True Then
                _view.fminimize()
            End If
            _view.refresh_view()
        End Try

    End Sub



    Public Sub load_view()
        _view.load_filter_values()
        _view.load_doc_list(_docs, Me)
    End Sub

End Class
Public Interface Ibc_dx_doc_open
    Sub load_doc_list(ByVal docs As bc_om_documents, ByVal ctrl As Cbc_dx_doc_Open)
    Sub wait_cursor()
    Sub default_cursor()
    Sub load_filter_values()
    Sub fhide()
    Sub fminimize()
    Sub refresh_view()
   
    Event open_doc(ByVal doc_id As String, ByVal mode As Boolean)
End Interface

