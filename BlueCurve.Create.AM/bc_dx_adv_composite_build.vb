Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports BlueCurve.Core.AS
Imports DevExpress.XtraTreeList
Imports System.Drawing
Imports System.Windows.Forms

Public Class bc_dx_adv_composite_build
    Implements Ibc_dx_adv_composite_build
    Public Event Build() Implements Ibc_dx_adv_composite_build.Build
    Public Event Cancel() Implements Ibc_dx_adv_composite_build.Cancel
    Dim oopen_doc As bc_am_at_open_doc
    Private Sub bcancel_Click(sender As Object, e As EventArgs) Handles bcancel.Click
        Me.Hide()
        RaiseEvent Cancel()
    End Sub

    Private Sub bok_Click(sender As Object, e As EventArgs) Handles bok.Click
        Me.Hide()
        RaiseEvent Build()
    End Sub

    Public Function load_data() As Boolean Implements Ibc_dx_adv_composite_build.load_data
        load_composite_list(0)
        load_lists()
        load_data = True
    End Function
    Private Sub load_lists()
        Try
            Dim t As System.TimeSpan
            t = New System.TimeSpan(28, 0, 0, 0)
            Me.dfrom.DateTime = Now.Subtract(t)
            Me.dto.DateTime = Now

            Me.cpt.Properties.Items.Add("All")
            Me.Cpt.SelectedIndex = 0
            Me.cst.Properties.Items.Add("All (exc publish)")
            For i = 0 To bc_am_load_objects.obc_pub_types.stages.Count - 1
                Me.cst.Properties.Items.Add(bc_am_load_objects.obc_pub_types.stages(i))
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
                                    Me.cpt.Properties.Items.Add(bc_am_load_objects.obc_pub_types.pubtype(i).name)
                                    Exit For
                                End If
                            Next
                        End If
                    End If
                Next
            Next

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_dx_adv_composite_build", "load_lists", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Private Sub load_composite_list(ByVal idx As Integer)
        Try
            Dim n As DevExpress.XtraTreeList.Nodes.TreeListNode
            Dim tln As Nodes.TreeListNode = Nothing
            Dim bcs As New bc_cs_icon_services
            Dim image As Bitmap
            Dim i As Integer
            For i = 4 To Me.uxDLImageList.Images.Count - 1
                Me.uxDLImageList.Images.RemoveAt(4)
            Next
            uxdocs.BeginUpdate()
            Try
                uxdocs.ClearNodes()
            Catch
            End Try
            For i = 0 To bc_am_load_objects.obc_om_composite_pub_type.composite_composition.documents.document.Count - 1
                With bc_am_load_objects.obc_om_composite_pub_type.composite_composition.documents.document(i)
                    n = uxdocs.AppendNode(New Object() {.title}, tln)
                    n.Tag = CStr(.id)
                    n.SetValue(1, .doc_date.tolocaltime)
                    n.SetValue(2, .pub_type_name)
                    n.SetValue(3, .stage_name)

                    If bc_am_load_objects.obc_om_composite_pub_type.composite_composition.documents.document(i).checked_out_user = 0 Then
                        n.SetValue(4, "in")
                        image = Nothing
                        image = bcs.get_icon_for_file_type(.extension)
                        If Not IsNothing(image) Then
                            Me.uxDLImageList.Images.Add(image)
                            n.StateImageIndex = Me.uxDLImageList.Images.Count - 1
                        Else
                            n.StateImageIndex = 0
                        End If
                    Else
                        For j = 0 To bc_am_load_objects.obc_users.user.Count - 1
                            If bc_am_load_objects.obc_users.user(j).id = bc_am_load_objects.obc_om_composite_pub_type.composite_composition.documents.document(i).checked_out_user Then
                                n.SetValue(4, bc_am_load_objects.obc_users.user(j).first_name + " " + bc_am_load_objects.obc_users.user(j).surname)
                                n.StateImageIndex = 2
                                Exit For
                            End If
                        Next
                    End If
                End With
            Next
            If Me.uxdocs.Nodes.Count > 0 Then
                Me.uxdocs.SetFocusedNode(Me.uxdocs.Nodes(idx))
            End If



        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_dx_adv_composite_build", "load_composite_list", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            uxdocs.EndUpdate()
        End Try
    End Sub

    Private Sub badd_Click(sender As Object, e As EventArgs) Handles badd.Click
        add_doc()
    End Sub
    Private Sub add_doc()
        Try
            If Me.uxsearch.Nodes.Count = 0 Then
                Exit Sub
            End If
           
            Me.Cursor = Cursors.WaitCursor

            Dim did As Long
            did = Me.uxsearch.Selection.Item(0).Tag

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
                    Exit For
                End If
            Next

            For i = 0 To Me.uxsearch.Nodes.Count - 1
                If Me.uxsearch.Nodes(i).Tag = did Then
                    Me.uxsearch.Nodes.RemoveAt(i)
                    Exit For
                End If
            Next
            Me.load_composite_list(Me.uxdocs.Nodes.Count)
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_adv_composite_build", "add_doc", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub

   
    Private Sub rem_doc()
        For i = 0 To bc_am_load_objects.obc_om_composite_pub_type.composite_composition.documents.document.Count - 1
            If bc_am_load_objects.obc_om_composite_pub_type.composite_composition.documents.document(i).id = Me.uxdocs.Selection.Item(0).Tag Then
                bc_am_load_objects.obc_om_composite_pub_type.composite_composition.documents.document.RemoveAt(i)
                Exit For
            End If
        Next
        Me.load_composite_list(0)
    End Sub
    Private Sub bup_Click(sender As Object, e As EventArgs) Handles bup.Click
        For i = 0 To bc_am_load_objects.obc_om_composite_pub_type.composite_composition.documents.document.Count - 1
            If bc_am_load_objects.obc_om_composite_pub_type.composite_composition.documents.document(i).id = Me.uxdocs.Selection.Item(0).Tag Then
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
                Exit For
            End If
        Next
    End Sub
    Private Sub bdn_Click(sender As Object, e As EventArgs) Handles bdn.Click
        For i = 0 To bc_am_load_objects.obc_om_composite_pub_type.composite_composition.documents.document.Count - 1
            If bc_am_load_objects.obc_om_composite_pub_type.composite_composition.documents.document(i).id = Me.uxdocs.Selection.Item(0).Tag Then
                If i = Me.uxdocs.Nodes.Count - 1 Then
                    Exit Sub
                End If
                Dim tmpdoc As New bc_om_document
                With bc_am_load_objects.obc_om_composite_pub_type.composite_composition
                    tmpdoc = .documents.document(i)
                    .documents.document.RemoveAt(i)
                    .documents.document.Insert(i + 1, tmpdoc)
                End With
                load_composite_list(i + 1)
                Exit For
            End If
        Next
    End Sub




    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        Dim slog = New bc_cs_activity_log("bc_am_adv_composite_build", "search", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim found As Boolean

        Me.Cursor = Cursors.WaitCursor


        Try

            Dim date_from As Date
            Dim date_to As Date
            Dim i As Integer
            date_from = Me.dfrom.DateTime
            date_to = Me.dto.DateTime
            If Me.cst.Text = "Publish" Then
                oopen_doc = New bc_am_at_open_doc(False, True, date_from, date_to, True, False)
            Else
                oopen_doc = New bc_am_at_open_doc(False, True, date_from, date_to, False, False)
            End If

            Dim n As DevExpress.XtraTreeList.Nodes.TreeListNode
            Dim tln As Nodes.TreeListNode = Nothing
            Dim bcs As New bc_cs_icon_services
            Dim image As Bitmap

            For i = 4 To Me.UXSearchimagelist.Images.Count - 1
                Me.UXSearchimagelist.Images.RemoveAt(4)
            Next
            uxsearch.BeginUpdate()
            Try
                uxsearch.ClearNodes()
            Catch
            End Try


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
                            ' Dim item As ListViewItem
                            If (Me.ttitle.Text = "" Or (Me.ttitle.Text <> "" And InStr(oopen_doc.onetwork_docs.document(i).title, Me.ttitle.Text) > 0)) And (Me.cst.Text = "All (exc publish)" Or (Me.cst.Text <> "All (exc publish)" And Me.cst.Text = oopen_doc.onetwork_docs.document(i).stage_name)) And (Me.cpt.Text = "All" Or (Me.cpt.Text <> "All" And Me.cpt.Text = oopen_doc.onetwork_docs.document(i).pub_type_name)) Then
                                REM If (all = False And oopen_doc.onetwork_docs.document(i).originating_author = bc_cs_central_settings.logged_on_user_id) Or all = True Or local_docs = False Then
                                With oopen_doc.onetwork_docs.document(i)
                                    n = uxsearch.AppendNode(New Object() {.title}, tln)
                                    n.Tag = CStr(.id)
                                    n.SetValue(1, .doc_date.tolocaltime)
                                    n.SetValue(2, .pub_type_name)
                                    n.SetValue(3, .stage_name)

                                    If .checked_out_user = 0 Then
                                        n.SetValue(4, "in")
                                        image = Nothing
                                        image = bcs.get_icon_for_file_type(.extension)
                                        If Not IsNothing(image) Then
                                            Me.UXSearchimagelist.Images.Add(image)
                                            n.StateImageIndex = Me.UXSearchimagelist.Images.Count - 1
                                        Else
                                            n.StateImageIndex = 0
                                        End If
                                    Else
                                        For j = 0 To bc_am_load_objects.obc_users.user.Count - 1
                                            If bc_am_load_objects.obc_users.user(j).id = .checked_out_user Then
                                                n.SetValue(4, bc_am_load_objects.obc_users.user(j).first_name + " " + bc_am_load_objects.obc_users.user(j).surname)
                                                n.StateImageIndex = 2
                                                Exit For
                                            End If
                                        Next
                                    End If

                                End With

                            End If
                        End If
                    End If
                Next
            End If


        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_adv_composite_build", "search", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally

            uxsearch.EndUpdate()
            Me.Cursor = Cursors.Default
            slog = New bc_cs_activity_log("bc_am_adv_composite_build", "search", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub


    

    Private Sub uxsearch_DoubleClick(sender As Object, e As EventArgs) Handles uxsearch.DoubleClick
        add_doc()

    End Sub

   
    Private Sub uxdocs_DoubleClick(sender As Object, e As EventArgs) Handles uxdocs.DoubleClick
        rem_doc()
    End Sub

    Private Sub brem_Click(sender As Object, e As EventArgs) Handles brem.Click
        rem_doc()
    End Sub
End Class
Public Class Cbc_dx_adv_composite_build
    WithEvents _view As Ibc_dx_adv_composite_build
    Public build_selected As Boolean = False
    Dim bc_am_composite_doc As bc_am_composite_docs
    Public Sub New(view As Ibc_dx_adv_composite_build)
        _view = view
    End Sub
    Public Sub cancel() Handles _view.Cancel
        build_selected = False
    End Sub
    Public Sub build() Handles _view.Build
        bc_am_composite_doc.merge()
        build_selected = True
    End Sub
    Public Function load_data(pub_type_Id As Long, entity_Id As Long)
        load_data = False
        Try
            For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                If bc_am_load_objects.obc_pub_types.pubtype(i).id = pub_type_Id Then
                    bc_am_load_objects.obc_om_composite_pub_type = bc_am_load_objects.obc_pub_types.pubtype(i)
                    bc_am_composite_doc = New bc_am_composite_docs(entity_Id, "")
                    If bc_am_composite_doc.get_component_documents = True Then
                        If _view.load_data() = True Then
                            load_data = True
                        End If
                    End If
                    Exit For
                End If
            Next

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Cbc_dx_adv_composite_build", "load_data", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Function
End Class
Public Interface Ibc_dx_adv_composite_build
    Event Cancel()
    Event Build()
    Function load_data() As Boolean
End Interface