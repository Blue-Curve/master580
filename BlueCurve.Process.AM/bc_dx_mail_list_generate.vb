Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports BlueCurve.Core.AS

Imports DevExpress.XtraTreeList

Public Class bc_dx_mail_list_generate
    Implements Ibc_dx_mail_list_generate

    Dim _doc_distribution As bc_om_distribution
    Dim classes As New List(Of bc_om_entity_class)
    Dim active_ent_list As New List(Of bc_om_entity)
    Public Function load_data(doc_distribution As bc_om_distribution) As Boolean Implements Ibc_dx_mail_list_generate.load_data
        load_data = False
        Try
            _doc_distribution = doc_distribution
            Me.uxputypes.Properties.Items.Clear()
            For i = 0 To _doc_distribution.pub_types.Count - 1
                Me.uxputypes.Properties.Items.Add(_doc_distribution.pub_types(i).name)
            Next
            Dim cl As bc_om_entity_class
            cl = New bc_om_entity_class
            cl.class_id = 0
            cl.class_name = "Author"
            classes.Add(cl)

            For i = 0 To _doc_distribution.mail_list_classes.Count - 1
                uxtaxonomy.TabPages.Add(_doc_distribution.mail_list_classes(i).class_name)
                cl = New bc_om_entity_class
                cl.class_id = _doc_distribution.mail_list_classes(i).class_id
                cl.class_name = _doc_distribution.mail_list_classes(i).class_name
                classes.Add(cl)
            Next
            load_class(0, "")
            load_data = True

        Catch ex As Exception
            Dim err As New bc_cs_error_log("bc_dx_mail_list_generate", "load_data", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Function
    Sub load_class(idx As Integer, tx As String)

        Try
            uxentity.Items.Clear()
            uxentity.BeginUpdate()

            active_ent_list.Clear()

            Dim sel_user As New List(Of Long)
            Dim sel_ent As New List(Of Long)

            For i = 0 To pclear.Nodes.Count - 1
                If pclear.Nodes(i).GetValue(0) = "Author" Then
                    sel_user.Add(pclear.Nodes(i).Tag)
                Else
                    sel_ent.Add(pclear.Nodes(i).Tag)
                End If
            Next

            Dim found As Boolean = True
            If idx = 0 Then
                For i = 0 To bc_am_load_objects.obc_users.user.Count - 1
                    found = False
                    For j = 0 To sel_user.Count - 1
                        If sel_user(j) = bc_am_load_objects.obc_users.user(i).id Then
                            found = True
                            Exit For
                        End If
                    Next
                    If found = False Then
                        If tx = "" Then
                            uxentity.Items.Add(bc_am_load_objects.obc_users.user(i).first_name + " " + bc_am_load_objects.obc_users.user(i).surname)
                        ElseIf InStr(UCase(bc_am_load_objects.obc_users.user(i).first_name + " " + bc_am_load_objects.obc_users.user(i).surname), tx) > 0 Then
                            uxentity.Items.Add(bc_am_load_objects.obc_users.user(i).first_name + " " + bc_am_load_objects.obc_users.user(i).surname)
                        End If
                    End If

                Next
            Else
                For i = 0 To bc_am_load_objects.obc_entities.entity.Count - 1
                    found = False
                    For j = 0 To sel_ent.Count - 1
                        If sel_ent(j) = bc_am_load_objects.obc_entities.entity(i).id Then
                            found = True
                            Exit For
                        End If
                    Next
                    If found = False Then
                        If bc_am_load_objects.obc_entities.entity(i).class_id = classes(idx).class_id Then
                            If tx = "" Then
                                uxentity.Items.Add(bc_am_load_objects.obc_entities.entity(i).name)
                                active_ent_list.Add(bc_am_load_objects.obc_entities.entity(i))
                            ElseIf InStr(UCase(bc_am_load_objects.obc_entities.entity(i).name), tx) > 0 Then
                                uxentity.Items.Add(bc_am_load_objects.obc_entities.entity(i).name)
                                active_ent_list.Add(bc_am_load_objects.obc_entities.entity(i))
                            End If
                        End If
                    End If
                Next

            End If

        Catch ex As Exception
            Dim err As New bc_cs_error_log("bc_dx_mail_list_generate", "load_data", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            uxentity.EndUpdate()
        End Try
    End Sub

    Public Event run(cat_doc As bc_om_document) Implements Ibc_dx_mail_list_generate.run

    Private Sub bcancel_Click(sender As Object, e As EventArgs) Handles bcancel.Click
        Me.Hide()
    End Sub

    Private Sub bsave_Click(sender As Object, e As EventArgs) Handles bsave.Click
        Try
            Dim omsg As New bc_cs_message("Blue Curve", "Are you sure wish to generate a mail list and merge into document mail list?", bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
            If omsg.cancel_selected = True Then
                Exit Sub
            End If
            Cursor = Windows.Forms.Cursors.WaitCursor

            Dim cat_doc As New bc_om_document
            cat_doc.pub_type_id = _doc_distribution.pub_types(Me.uxputypes.SelectedIndex).id

            For i = 0 To pclear.Nodes.Count - 1
                If pclear.Nodes(i).GetValue(0) = "Author" Then
                    Dim ouser As New bc_om_user
                    ouser.id = pclear.Nodes(i).Tag
                    cat_doc.authors.Add(ouser)

                Else
                    Dim otax As New bc_om_taxonomy
                    otax.entity_id = pclear.Nodes(i).Tag
                    cat_doc.taxonomy.Add(otax)
                End If

            Next

            RaiseEvent run(cat_doc)
            Me.Hide()
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bsave", "click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Cursor = Windows.Forms.Cursors.Default

        End Try
    End Sub



    Private Sub uxputypes_SelectedIndexChanged(sender As Object, e As EventArgs) Handles uxputypes.SelectedIndexChanged
        Me.bsave.Enabled = False
        If Me.uxputypes.SelectedIndex > -1 Then
            Me.bsave.Enabled = True
        End If
    End Sub

    Private Sub pclear_EditValueChanged(sender As Object, e As EventArgs)
        Me.tsearch.Text = ""

    End Sub

    Private Sub tsearch_EditValueChanged(sender As Object, e As EventArgs) Handles tsearch.EditValueChanged

        searchtimer.Start()

    End Sub

    Private Sub searchtimer_Tick(sender As Object, e As EventArgs) Handles searchtimer.Tick
        Cursor = Windows.Forms.Cursors.WaitCursor
        searchtimer.Stop()


        load_class(Me.uxtaxonomy.SelectedTabPageIndex, UCase(tsearch.Text))
        Cursor = Windows.Forms.Cursors.Default
    End Sub



    Private Sub uxtaxonomy_SelectedPageChanged(sender As Object, e As DevExpress.XtraTab.TabPageChangedEventArgs) Handles uxtaxonomy.SelectedPageChanged
        load_class(Me.uxtaxonomy.SelectedTabPageIndex, "")
        lsearch.Text = ""
    End Sub

    Private Sub uxentity_SelectedIndexChanged(sender As Object, e As EventArgs) Handles uxentity.DoubleClick
        Try
            If uxentity.SelectedIndex = -1 Then
                Exit Sub
            End If
            pclear.BeginUpdate()
            Dim tln As Nodes.TreeListNode = Nothing
            If uxtaxonomy.SelectedTabPageIndex = 0 Then
                For i = 0 To bc_am_load_objects.obc_users.user.Count - 1
                    If bc_am_load_objects.obc_users.user(i).first_name + " " + bc_am_load_objects.obc_users.user(i).surname = uxentity.Text Then
                        pclear.AppendNode(New Object() {"Author"}, tln).Tag = bc_am_load_objects.obc_users.user(i).id
                        pclear.Nodes(pclear.Nodes.Count - 1).SetValue(1, uxentity.Text)
                        Exit For
                    End If
                Next
            Else
                For i = 0 To active_ent_list.Count - 1
                    If active_ent_list(i).name = uxentity.Text Then
                        pclear.AppendNode(New Object() {classes(Me.uxtaxonomy.SelectedTabPageIndex).class_name}, tln).Tag = active_ent_list(i).id
                        pclear.Nodes(pclear.Nodes.Count - 1).SetValue(1, uxentity.Text)
                        Exit For

                    End If
                Next
            End If

            uxentity.Items.RemoveAt(uxentity.SelectedIndex)
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("uxentity", "DoubleClick", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            pclear.EndUpdate()

        End Try
    End Sub

    Private Sub uxchannels_DoubleClick(sender As Object, e As EventArgs) Handles pclear.Click
        For i = 0 To pclear.Nodes.Count - 1
            If pclear.Nodes(i).Selected = True Then
                pclear.Nodes.RemoveAt(i)
                Exit Sub
            End If
        Next

    End Sub
  
    Private Sub uxchannels_FocusedNodeChanged(sender As Object, e As DevExpress.XtraTreeList.FocusedNodeChangedEventArgs) Handles pclear.FocusedNodeChanged

    End Sub

    Private Sub uxentity_SelectedIndexChanged_1(sender As Object, e As EventArgs) Handles uxentity.SelectedIndexChanged

    End Sub

    Private Sub PictureEdit1_Click(sender As Object, e As EventArgs) Handles PictureEdit1.Click
        Me.tsearch.Text = ""

    End Sub

    Private Sub PictureEdit1_EditValueChanged(sender As Object, e As EventArgs) Handles PictureEdit1.EditValueChanged

    End Sub
End Class
Public Class Cbc_dx_mail_list_generate
    WithEvents _view As Ibc_dx_mail_list_generate
    Dim _doc_distribution As bc_om_distribution
    Public cancel = True
    Function load_data(view As bc_dx_mail_list_generate, doc_distribution As bc_om_distribution) As Boolean
        Try
            _doc_distribution = doc_distribution
            _view = view

            _doc_distribution.operation = bc_om_distribution.DIST_OPERATION.MAIL_LIST_CLASSES

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                _doc_distribution.db_read()
            Else
                _doc_distribution.tmode = bc_cs_soap_base_class.tREAD
                _doc_distribution.transmit_to_server_and_receive(_doc_distribution, True)
            End If


            Return _view.load_data(_doc_distribution)
        Catch ex As Exception
            Dim err As New bc_cs_error_log("Cbc_dx_mail_list_generate", "load_data", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Function
    Sub run_list(cat_doc As bc_om_document) Handles _view.run
        Try
            cat_doc.id = "99999999999" + CStr(bc_cs_central_settings.logged_on_user_id)
            _doc_distribution.merge_from_doc = cat_doc
            _doc_distribution.operation = bc_om_distribution.DIST_OPERATION.MERGE_LIST
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                _doc_distribution.db_read()
            Else
                _doc_distribution.tmode = bc_cs_soap_base_class.tREAD
                _doc_distribution.transmit_to_server_and_receive(_doc_distribution, True)
            End If

            cancel = False
        Catch ex As Exception
            Dim err As New bc_cs_error_log("Cbc_dx_mail_list_generate", "run_list", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Sub
End Class
Public Interface Ibc_dx_mail_list_generate
    Function load_data(doc_distribution As bc_om_distribution) As Boolean
    Event run(cat_doc As bc_om_document)
End Interface
