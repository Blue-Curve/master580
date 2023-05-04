Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports BlueCurve.Core.AS
Imports System.Windows.Forms
Public Class bc_am_adv_composite_build
    Public proceed As Boolean = False
    Public oopen_doc As bc_am_at_open_doc
    Private Sub bc_am_adv_composite_build_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        load_lists()
    End Sub
    Private Sub load_lists()
        Try
            Dim t As System.TimeSpan
            t = New System.TimeSpan(28, 0, 0, 0)
            Me.Dto.Value = Now
            Me.dfrom.Value = Now.Subtract(t)
            Me.Cpt.Items.Add("All")
            Me.Cpt.SelectedIndex = 0
            Me.Cst.Items.Add("All (exc publish)")
            For i = 0 To bc_am_load_objects.obc_pub_types.stages.Count - 1
                Me.Cst.Items.Add(bc_am_load_objects.obc_pub_types.stages(i))
            Next
            Me.Cst.SelectedIndex = 0
            For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                REM filter pub types for users bus area
                For j = 0 To bc_am_load_objects.obc_prefs.bus_areas.Count - 1
                    If bc_am_load_objects.obc_prefs.bus_areas(j) = bc_am_load_objects.obc_pub_types.pubtype(i).bus_area_id Then
                        If bc_am_load_objects.obc_pub_types.pubtype(i).show_in_wizard = True Then
                            REM see if search filter pub type
                            For k = 0 To bc_cs_central_settings.search_pub_type_list.Count - 1
                                If bc_cs_central_settings.search_pub_type_list(k) = bc_am_load_objects.obc_pub_types.pubtype(i).name Then
                                    Me.Cpt.Items.Add(bc_am_load_objects.obc_pub_types.pubtype(i).name)
                                    Exit For
                                End If
                            Next
                        End If
                    End If
                Next
            Next
            load_composite_list(-1)
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_adv_composite_build", "load_lists", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
        'bc_am_load_objects.obc_pub_types.

    End Sub

    Private Sub load_composite_list(ByVal idx As Integer)
        Try
            Dim lvw As ListViewItem
            Me.Lvwcomps.Items.Clear()
            Dim i As Integer
            For i = 0 To bc_am_load_objects.obc_om_composite_pub_type.composite_composition.documents.document.Count - 1
                With bc_am_load_objects.obc_om_composite_pub_type.composite_composition.documents.document(i)
                    If bc_am_load_objects.obc_om_composite_pub_type.composite_composition.documents.document(i).checked_out_user = 0 Then
                        lvw = New ListViewItem(CStr(.title), 1)
                        lvw.SubItems.Add(Format("dd-mmm-yyyy", CStr(.doc_date.tolocaltime())))
                        lvw.SubItems.Add(CStr(.pub_type_name))
                        lvw.SubItems.Add(CStr(.stage_name))
                        Me.Lvwcomps.Items.Add(lvw)
                        buildlist_tooltiptext(lvw)
                    Else
                        For j = 0 To bc_am_load_objects.obc_users.user.Count - 1
                            If bc_am_load_objects.obc_users.user(j).id = bc_am_load_objects.obc_om_composite_pub_type.composite_composition.documents.document(i).checked_out_user Then
                                lvw = New ListViewItem(CStr(.title), 0)
                                lvw.SubItems.Add(Format("dd-mmm-yyyy", CStr(.doc_date.tolocaltime())))
                                lvw.SubItems.Add(CStr(.pub_type_name))
                                lvw.SubItems.Add(CStr(.stage_name))
                                lvw.SubItems.Add(bc_am_load_objects.obc_users.user(j).first_name + " " + bc_am_load_objects.obc_users.user(j).surname)
                                Me.Lvwcomps.Items.Add(lvw)
                                buildlist_tooltiptext(lvw)
                                Exit For
                            End If
                        Next
                    End If
                End With
            Next
            If idx > -1 Then
                Me.Lvwcomps.Items(idx).Selected = True
            End If


        Catch ex As Exception
            MsgBox(ex.Message)

        End Try
    End Sub

    Private Sub PictureBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox1.Click

    End Sub



    Private Sub bok_mdn(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bok.MouseDown
        Me.bokgrey.Visible = True
        Me.bok.Visible = False
    End Sub
    Private Sub bok_mup(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bok.MouseUp
        'Me.Hide()
        Close()
        proceed = True
    End Sub

    Private Sub bleftgrey_mdn(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bleftgrey.MouseDown
        Me.bleftgrey.Visible = False
        Me.Btnback.Visible = True

    End Sub
    Private Sub bleftgrey_mup(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bleftgrey.MouseUp
        'Me.Hide()
        Close()
        proceed = False
    End Sub

    Private Sub Bup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Bup.Click
        Try
            Dim i As Integer
            i = Me.Lvwcomps.SelectedItems(0).Index
            REM swap over values
            If i = 0 Then
                Exit Sub
            End If
            Dim tmpdoc As New bc_om_document
            With bc_am_load_objects.obc_om_composite_pub_type.composite_composition
                tmpdoc = .documents.document(i)
                .documents.document.RemoveAt(i)
                .documents.document.Insert(i - 1, tmpdoc)
            End With
            load_composite_list(i - 1)
        Catch

        End Try
    End Sub
    Private Sub Bdn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Bdn.Click
        Try
            Dim i As Integer
            i = Me.Lvwcomps.SelectedItems(0).Index
            REM swap over values
            If i = Me.Lvwcomps.Items.Count - 1 Then
                Exit Sub
            End If
            Dim tmpdoc As New bc_om_document
            With bc_am_load_objects.obc_om_composite_pub_type.composite_composition
                tmpdoc = .documents.document(i)
                .documents.document.RemoveAt(i)
                .documents.document.Insert(i + 1, tmpdoc)
            End With
            load_composite_list(i + 1)
        Catch

        End Try
    End Sub

    Private Sub Lvwcomps_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Lvwcomps.Click
        'buildlist_tooltiptext()
    End Sub
    Private Sub Lvwcomps_dblclk(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Lvwcomps.DoubleClick
        remove_comp()
    End Sub

    Private Sub Lvwcomps_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Lvwcomps.SelectedIndexChanged
        Me.Bup.Enabled = False
        Me.Bdn.Enabled = False
        Me.brm.Enabled = False

        If Me.Lvwcomps.SelectedItems.Count = 0 Then
            Exit Sub
        End If
        Me.brm.Enabled = True
        If Me.Lvwcomps.SelectedItems(0).Index > 0 Then
            Me.Bup.Enabled = True
        End If
        If Me.Lvwcomps.SelectedItems(0).Index < Me.Lvwcomps.Items.Count - 1 Then
            Me.Bdn.Enabled = True
        End If
        'buildlist_tooltiptext()
    End Sub

    Private Sub lvwres_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvwres.Click
        'search_tooltiptext()
    End Sub


    Private Sub Lvw_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles lvwres.ColumnClick
        Dim slog = New bc_cs_activity_log("bc_am_at_wizard_main", "get_entity_id_selected", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Select Case CInt(e.Column.ToString)
                Case 1
                    REM date
                    lvwres.ListViewItemSorter() = New CompareBydate
                    If CompareBydate.toggle = False Then
                        CompareBydate.toggle = True
                    Else
                        CompareBydate.toggle = False
                    End If
                Case 0
                    REM item
                    lvwres.ListViewItemSorter() = New CompareByname
                    If CompareByname.toggle = False Then
                        CompareByname.toggle = True
                    Else
                        CompareByname.toggle = False
                    End If
                Case 2
                    REM item
                    lvwres.ListViewItemSorter() = New CompareBypt
                    If CompareBypt.toggle = False Then
                        CompareBypt.toggle = True
                    Else
                        CompareBypt.toggle = False
                    End If
                Case 3
                    REM item
                    lvwres.ListViewItemSorter() = New CompareByuser
                    If CompareByname.toggle = False Then
                        CompareByname.toggle = True
                    Else
                        CompareByname.toggle = False
                    End If
                Case 4
                    REM item
                    lvwres.ListViewItemSorter() = New CompareBystage
                    If CompareBystage.toggle = False Then
                        CompareBystage.toggle = True
                    Else
                        CompareBystage.toggle = False
                    End If


            End Select
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_at_wizard_main", "columnclick", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_at_wizard_main", "columnclick", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    Private Sub ListView1_dblclk(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvwres.DoubleClick
        add_comp()
    End Sub
    Private Sub ListView1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvwres.SelectedIndexChanged
        Me.badd.Enabled = False
        If Me.lvwres.SelectedItems.Count > 0 Then
            Me.badd.Enabled = True
            'search_tooltiptext()
        End If
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim slog = New bc_cs_activity_log("bc_am_adv_composite_build", "search", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim found As Boolean
        Dim lvi As ListViewItem
        Me.Cursor = Cursors.WaitCursor


        Try
            Dim date_from As Date
            Dim date_to As Date
            Dim i As Integer
            date_from = Me.dfrom.Value
            date_to = Me.Dto.Value
            If Me.Cst.Text = "Publish" Then
                oopen_doc = New bc_am_at_open_doc(False, True, date_from, date_to, True, False)
            Else
                oopen_doc = New bc_am_at_open_doc(False, True, date_from, date_to, False, False)
            End If
            Me.lvwres.Items.Clear()
            lvwres.Items.Clear()
            lvwres.SmallImageList = ImageList1
            Dim dfound As Boolean = False
            If Not IsNothing(oopen_doc.onetwork_docs) Then
                For i = 0 To oopen_doc.onetwork_docs.document.Count - 1
                    dfound = False
                    REM check not in composite list already
                    For j = 0 To bc_am_load_objects.obc_om_composite_pub_type.composite_composition.documents.document.Count - 1
                        If oopen_doc.onetwork_docs.document(i).id = bc_am_load_objects.obc_om_composite_pub_type.composite_composition.documents.document(j).id Then
                            dfound = True
                            Exit For
                        End If
                    Next
                    REM check in filter pub type list
                    If dfound = False Then
                        dfound = True
                        For k = 0 To bc_cs_central_settings.search_pub_type_list.Count - 1
                            If bc_cs_central_settings.search_pub_type_list(k) = oopen_doc.onetwork_docs.document(i).pub_type_name Then
                                dfound = False
                                Exit For
                            End If
                        Next
                        If dfound = False Then
                            REM apply data filter
                            REM if originating author flag is not set show all else filter on
                            REM logged on user
                            found = False
                            Dim item As ListViewItem
                            If (Me.Ttitle.Text = "" Or (Me.Ttitle.Text <> "" And InStr(oopen_doc.onetwork_docs.document(i).title, Me.Ttitle.Text) > 0)) And (Me.Cst.Text = "All (exc publish)" Or (Me.Cst.Text <> "All (exc publish)" And Me.Cst.Text = oopen_doc.onetwork_docs.document(i).stage_name)) And (Me.Cpt.Text = "All" Or (Me.Cpt.Text <> "All" And Me.Cpt.Text = oopen_doc.onetwork_docs.document(i).pub_type_name)) Then
                                REM If (all = False And oopen_doc.onetwork_docs.document(i).originating_author = bc_cs_central_settings.logged_on_user_id) Or all = True Or local_docs = False Then

                                If oopen_doc.onetwork_docs.document(i).checked_out_user = 0 Then
                                    item = New ListViewItem(CStr(oopen_doc.onetwork_docs.document(i).title), 1)

                                    item.SubItems.Add(CStr(oopen_doc.onetwork_docs.document(i).doc_date))
                                    item.SubItems.Add(CStr(oopen_doc.onetwork_docs.document(i).pub_type_name))
                                    item.SubItems.Add(CStr(oopen_doc.onetwork_docs.document(i).stage_name))

                                    item.SubItems.Add("")
                                Else
                                    item = New ListViewItem(CStr(oopen_doc.onetwork_docs.document(i).title), 0)
                                    item.SubItems.Add(CStr(oopen_doc.onetwork_docs.document(i).doc_date))
                                    item.SubItems.Add(CStr(oopen_doc.onetwork_docs.document(i).pub_type_name))
                                    item.SubItems.Add(CStr(oopen_doc.onetwork_docs.document(i).stage_name))
                                    For j = 0 To bc_am_load_objects.obc_users.user.Count - 1
                                        If bc_am_load_objects.obc_users.user(j).id = oopen_doc.onetwork_docs.document(i).checked_out_user Then
                                            item.SubItems.Add(bc_am_load_objects.obc_users.user(j).first_name + " " + bc_am_load_objects.obc_users.user(j).surname)
                                            Exit For
                                        End If
                                    Next
                                End If
                                item.SubItems.Add(oopen_doc.onetwork_docs.document(i).id)
                                lvi = lvwres.Items.Add(item)
                                search_tooltiptext(lvi)
                            End If
                        End If
                    End If
                Next
            End If


        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_adv_composite_build", "search", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            Me.badd.Enabled = False
            Me.Cursor = Cursors.Default

            slog = New bc_cs_activity_log("bc_am_adv_composite_build", "search", bc_cs_activity_codes.TRACE_EXIT, "")

        End Try
    End Sub

    Class CompareByname
        Implements System.Collections.IComparer
        Public Shared toggle As Boolean = False

        Function compare(ByVal x As Object, ByVal y As Object) As Integer _
        Implements System.Collections.IComparer.Compare

            Dim item1 As ListViewItem = CType(x, ListViewItem)
            Dim item2 As ListViewItem = CType(y, ListViewItem)
            If toggle = True Then
                Return String.Compare(item1.Text, item2.Text)
            Else
                Return String.Compare(item2.Text, item1.Text)
            End If
        End Function
    End Class
    Class CompareByuser
        Implements System.Collections.IComparer
        Public Shared toggle As Boolean = False

        Function compare(ByVal x As Object, ByVal y As Object) As Integer _
        Implements System.Collections.IComparer.Compare

            Dim item1 As ListViewItem = CType(x, ListViewItem)
            Dim item2 As ListViewItem = CType(y, ListViewItem)
            If toggle = True Then
                Return String.Compare(item1.SubItems(3).Text, item2.SubItems(3).Text)
            Else
                Return String.Compare(item2.SubItems(3).Text, item1.SubItems(3).Text)
            End If
        End Function
    End Class
    Class CompareBypt
        Implements System.Collections.IComparer
        Public Shared toggle As Boolean = False

        Function compare(ByVal x As Object, ByVal y As Object) As Integer _
        Implements System.Collections.IComparer.Compare

            Dim item1 As ListViewItem = CType(x, ListViewItem)
            Dim item2 As ListViewItem = CType(y, ListViewItem)
            If toggle = True Then
                Return String.Compare(item1.SubItems(2).Text, item2.SubItems(2).Text)
            Else
                Return String.Compare(item2.SubItems(2).Text, item1.SubItems(2).Text)
            End If
        End Function
    End Class
    Class CompareBystage
        Implements System.Collections.IComparer
        Public Shared toggle As Boolean = False

        Function compare(ByVal x As Object, ByVal y As Object) As Integer _
        Implements System.Collections.IComparer.Compare

            Dim item1 As ListViewItem = CType(x, ListViewItem)
            Dim item2 As ListViewItem = CType(y, ListViewItem)
            If toggle = True Then
                Return String.Compare(item1.SubItems(4).Text, item2.SubItems(4).Text)
            Else
                Return String.Compare(item2.SubItems(4).Text, item1.SubItems(4).Text)
            End If
        End Function
    End Class
    Class CompareBydate
        Implements System.Collections.IComparer
        Public Shared toggle As Boolean = False

        Function compare(ByVal x As Object, ByVal y As Object) As Integer _
        Implements System.Collections.IComparer.Compare
            Try
                Dim item1 As ListViewItem = CType(x, ListViewItem)
                Dim item2 As ListViewItem = CType(y, ListViewItem)
                If toggle = True Then
                    Return Date.Compare(Date.Parse(item1.SubItems(1).Text), Date.Parse(item2.SubItems(1).Text))
                Else
                    Return Date.Compare(Date.Parse(item2.SubItems(1).Text), Date.Parse(item1.SubItems(1).Text))
                End If
            Catch

            End Try
        End Function

        Public Sub New()

        End Sub
    End Class

    Private Sub bc_am_adv_composite_build_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize

        Me.Lvwcomps.Columns(0).Width = Me.Lvwcomps.Width * (4 / 10)
        Me.Lvwcomps.Columns(1).Width = Me.Lvwcomps.Width * (1.5 / 10)
        Me.Lvwcomps.Columns(2).Width = Me.Lvwcomps.Width * (1.5 / 10)
        Me.Lvwcomps.Columns(3).Width = Me.Lvwcomps.Width * (1.5 / 10)
        Me.Lvwcomps.Columns(4).Width = Me.Lvwcomps.Width * (1.5 / 10) - 22
        Me.lvwres.Columns(0).Width = Me.lvwres.Width * (4 / 10)
        Me.lvwres.Columns(1).Width = Me.lvwres.Width * (1.5 / 10)
        Me.lvwres.Columns(2).Width = Me.lvwres.Width * (1.5 / 10)
        Me.lvwres.Columns(3).Width = Me.lvwres.Width * (1.5 / 10)
        Me.lvwres.Columns(4).Width = Me.lvwres.Width * (1.5 / 10) - 22

        ' stop resizing when a certain size is reached
        If Me.Width < 885 Then
            Me.Width = 885
            Exit Sub
        End If

        If Me.Height < 700 Then
            Me.Height = 700
            Exit Sub
        End If

    End Sub

    Private Sub Panel1_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Panel1.Paint

    End Sub
    Private Sub add_comp()
        Try
            If Me.lvwres.SelectedItems.Count <> 1 Then
                Exit Sub
            End If
            Me.Cursor = Cursors.WaitCursor

            Dim did As Long
            did = Me.lvwres.SelectedItems(0).SubItems(5).Text

            For i = 0 To oopen_doc.onetwork_docs.document.Count - 1
                If oopen_doc.onetwork_docs.document(i).id = CLng(did) Then
                    REM now get actually physical document
                    With oopen_doc.onetwork_docs.document(i)
                        .bcheck_out = False
                        .btake_revision = False
                        .bwith_document = True
                        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                            '.db_read()
                            .db_read_physical_doc_view_only()
                        Else
                            .tmode = bc_cs_soap_base_class.tREAD
                            oopen_doc.onetwork_docs.document(i).read_mode = bc_om_document.GET_PHYSICAL_DOC_VIEW_ONLY
                            If .transmit_to_server_and_receive(oopen_doc.onetwork_docs.document(i), True) = False Then
                                Exit Sub
                            End If
                        End If
                        .id = did
                    End With
                    bc_am_load_objects.obc_om_composite_pub_type.composite_composition.documents.document.Add(oopen_doc.onetwork_docs.document(i))
                    Me.load_composite_list(bc_am_load_objects.obc_om_composite_pub_type.composite_composition.documents.document.Count - 1)
                    Me.lvwres.SelectedItems(0).Remove()
                    Exit For
                End If
            Next

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_adv_composite_build", "badd", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub badd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles badd.Click
        add_comp()

    End Sub

    Private Sub brm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles brm.Click
        remove_comp()
    End Sub
    Private Sub remove_comp()
        bc_am_load_objects.obc_om_composite_pub_type.composite_composition.documents.document.RemoveAt(Me.Lvwcomps.SelectedItems(0).Index)
        Me.load_composite_list(-1)
        Me.Bup.Enabled = False
        Me.Bdn.Enabled = False
        Me.brm.Enabled = False
    End Sub
    Private Sub search_tooltiptext(ByVal lvi As ListViewItem)
        Dim tx As String = ""
        Try
            'If Me.lvwres.SelectedItems.Count <> 1 Then
            'Exit Sub
            'End If
            Dim did As Long
            did = lvi.SubItems(5).Text
            For i = 0 To oopen_doc.onetwork_docs.document.Count - 1
                If oopen_doc.onetwork_docs.document(i).id = CLng(did) Then
                    With oopen_doc.onetwork_docs.document(i)
                        tx = "Title: " + .title + vbCrLf
                        tx = tx + "Date: " + Format("DD-mmm-yyyy", .doc_date) + vbCrLf
                        tx = tx + "Type: " + .pub_type_name + vbCrLf
                        tx = tx + "Stage: " + .stage_name + vbCrLf
                        If .checked_out_user <> 0 Then
                            tx = tx + "Checked Out to: " + lvi.SubItems(2).Text + vbCrLf
                        End If
                        If .entity_id <> 0 Then
                            For j = 0 To bc_am_load_objects.obc_entities.entity.Count - 1
                                If bc_am_load_objects.obc_entities.entity(j).id = .entity_id Then
                                    tx = tx + "Master Classification: " + bc_am_load_objects.obc_entities.entity(j).name + vbCrLf
                                    Exit For
                                End If
                            Next
                        End If
                        For j = 0 To bc_am_load_objects.obc_users.user.Count - 1
                            If bc_am_load_objects.obc_users.user(j).id = .originating_author Then
                                tx = tx + "Originating Author: " + bc_am_load_objects.obc_users.user(j).first_name + " " + bc_am_load_objects.obc_users.user(j).surname + vbCrLf
                                Exit For
                            End If
                        Next
                    End With
                    Exit For
                End If
            Next

            lvi.ToolTipText = tx

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_adv_compsite_build", "search_tooltiptext", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try


    End Sub
    Private Sub buildlist_tooltiptext(ByVal lvi As ListViewItem)
        Dim tx As String = ""
        Try
            'If Me.Lvwcomps.SelectedItems.Count <> 1 Then
            'Exit Sub
            'End If
            With bc_am_load_objects.obc_om_composite_pub_type.composite_composition.documents.document(lvi.Index)
                tx = "Title: " + .title + vbCrLf
                tx = tx + "Date: " + Format("DD-mmm-yyyy", .doc_date) + vbCrLf
                tx = tx + "Type: " + .pub_type_name + vbCrLf
                tx = tx + "Stage: " + .stage_name + vbCrLf
                If .checked_out_user <> 0 Then
                    tx = tx + "Checked Out to: " + lvi.SubItems(2).Text + vbCrLf
                End If
                If .entity_id <> 0 Then
                    For j = 0 To bc_am_load_objects.obc_entities.entity.Count - 1
                        If bc_am_load_objects.obc_entities.entity(j).id = .entity_id Then
                            tx = tx + "Master Classification: " + bc_am_load_objects.obc_entities.entity(j).name + vbCrLf
                            Exit For
                        End If
                    Next
                End If
                For j = 0 To bc_am_load_objects.obc_users.user.Count - 1
                    If bc_am_load_objects.obc_users.user(j).id = .originating_author Then
                        tx = tx + "Originating Author: " + bc_am_load_objects.obc_users.user(j).first_name + " " + bc_am_load_objects.obc_users.user(j).surname + vbCrLf
                        Exit For
                    End If
                Next

            End With
            lvi.ToolTipText = tx

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_adv_composite_build", "buildlist_tooltiptext", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try


    End Sub

    Private Sub bok_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bok.Click

    End Sub


End Class