Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports BlueCurve.Core.AS

Imports DevExpress.XtraTreeList
Public Class bc_ax_adv_doc_refresh
    Implements Ibc_ax_adv_doc_refresh

    Event save(ByVal ldoc As bc_om_document) Implements Ibc_ax_adv_doc_refresh.save

    Event doc_refresh(ByVal ldoc As bc_om_document) Implements Ibc_ax_adv_doc_refresh.doc_refresh


    Dim _ldoc As New bc_om_document
    Public Sub New()

        DevExpress.Skins.SkinManager.EnableFormSkins()
        DevExpress.UserSkins.BonusSkins.Register()
        DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle("Blue")
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
    Public Sub load_data(ByVal ldoc As bc_om_document) Implements Ibc_ax_adv_doc_refresh.load_data

        _ldoc = ldoc
        For i = 0 To bc_am_load_objects.obc_pub_types.languages.Count - 1
            Me.uxlang.Properties.Items.Add(bc_am_load_objects.obc_pub_types.languages(i).name)
        Next
        load_view()
    End Sub

    Public Sub load_view()
        Try
            Dim lidx As Integer = 0
            Me.uxcomps.Nodes.Clear()
         
            For i = 0 To bc_am_load_objects.obc_pub_types.languages.Count - 1
                If (bc_am_load_objects.obc_pub_types.languages(i).id = _ldoc.language_id) Then
                    Me.uxlang.SelectedIndex = i
                End If
            Next

            Me.uxlang.SelectedIndex = lidx
            If _ldoc.refresh_components.workflow_state = 1 Then
                Me.uxstage.SelectedIndex = 0
            Else
                Me.uxstage.SelectedIndex = 1
            End If

            If _ldoc.refresh_components.data_at_date <> "9-9-9999" Then
                Me.uxdate.Enabled = True
                Me.uxdate.DateTime = _ldoc.refresh_components.data_at_date
            Else
                Me.uxdate.Enabled = False
                Me.uxdate.DateTime = Now
                uxcurrent.Checked = True
            End If

            For j = 0 To bc_am_load_objects.obc_pub_types.contributors.Count - 1
                Me.RepositoryItemComboBox1.Items.Add(bc_am_load_objects.obc_pub_types.contributors(j).name)
            Next

            For i = 0 To _ldoc.refresh_components.refresh_components.Count - 1
                Dim tln As Nodes.TreeListNode = Nothing

                With _ldoc.refresh_components.refresh_components(i)
                    Me.uxcomps.AppendNode(New Object() {.name}, tln).Tag = CStr(.locator)

                    If (.last_refresh_date = "09-09-9999" Or .last_refresh_date < .last_update_date) And .disabled = False Then
                        uxcomps.Nodes(i).SetValue(1, True)
                    Else
                        uxcomps.Nodes(i).SetValue(1, False)
                    End If


                    If .mode = 8 Then
                        uxcomps.Nodes(i).SetValue(2, "rtf edit")
                    ElseIf .mode = 9 Then
                        uxcomps.Nodes(i).SetValue(2, "text edit")
                    ElseIf .mode = 1 Or .mode = 11 Or .mode = 111 Then
                        uxcomps.Nodes(i).SetValue(2, "cell")
                    ElseIf .mode = 2 Or .mode = 21 Or .mode = 111 Then
                        uxcomps.Nodes(i).SetValue(2, "table")
                    ElseIf .mode = 3 Or .mode = 31 Or .mode = 111 Then
                        uxcomps.Nodes(i).SetValue(2, "chart")
                    ElseIf .mode = 6 Then
                        uxcomps.Nodes(i).SetValue(2, "index")
                    ElseIf .mode = 4 Or .mode = 5 Then
                        uxcomps.Nodes(i).SetValue(2, "image")
                    Else
                        uxcomps.Nodes(i).SetValue(2, "system")
                    End If

                    If .parameters.component_template_parameters.Count = 0 Then
                        uxcomps.Nodes(i).SetValue(3, "none")
                    Else
                        uxcomps.Nodes(i).SetValue(3, .parameters.component_template_parameters.Count)
                    End If

                    If .disabled = 1 Then
                        uxcomps.Nodes(i).SetValue(4, True)
                    End If


                    If .last_refresh_date = "9-9-9999" Then
                        uxcomps.Nodes(i).SetValue(5, "await")
                    Else
                        uxcomps.Nodes(i).SetValue(5, Format("dd MMM yyyy Hh:mm:ss", .last_refresh_date.ToLocalTime))
                    End If
                    If .last_update_date = "9-9-9999" Then
                        uxcomps.Nodes(i).SetValue(6, "n/a")
                    Else
                        uxcomps.Nodes(i).SetValue(6, Format("dd MMM yyyy Hh:mm:ss", .last_update_date.ToLocalTime))
                    End If

                    For j = 0 To bc_am_load_objects.obc_pub_types.contributors.Count - 1
                        If bc_am_load_objects.obc_pub_types.contributors(j).id = .contributor_id Then
                            uxcomps.Nodes(i).SetValue(7, (bc_am_load_objects.obc_pub_types.contributors(j).name))
                            Exit For
                        End If
                    Next

                End With
            Next
            If _ldoc.refresh_components.refresh_components.Count > 0 Then
                If _ldoc.refresh_components.refresh_components(0).parameters.component_template_parameters.count > 0 Then
                    Me.uxparams.Enabled = True
                End If
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("view_bc_am_adv_doc_build", "load_data", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try
    End Sub

    Private Sub uxcomps_FocusedNodeChanged(ByVal sender As Object, ByVal e As DevExpress.XtraTreeList.FocusedNodeChangedEventArgs) Handles uxcomps.FocusedNodeChanged

        REM DX upgrade
        Try
            If uxcomps.Selection.Count = 1 Then
                Me.uxparams.Enabled = False
                If uxcomps.Selection.Item(0).GetValue(3).ToString <> "none" Then
                    Me.uxparams.Enabled = True
                End If
            End If
        Catch
        End Try
    End Sub
    Private Sub bcancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bcancel.Click
        Me.Hide()
    End Sub

    Private Sub bok_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bok.Click
        save_vals()
        RaiseEvent save(_ldoc)
        Me.Hide()
    End Sub

    Private Sub brefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles brefresh.Click
        save_vals()
        RaiseEvent doc_refresh(_ldoc)
        Me.Hide()
    End Sub
    Private Sub save_vals()

        REM language
        _ldoc.language_id = bc_am_load_objects.obc_pub_types.languages(Me.uxlang.SelectedIndex).id

        REM stage
        If Me.uxstage.SelectedIndex = 0 Then
            _ldoc.refresh_components.workflow_state = 1
        Else
            _ldoc.refresh_components.workflow_state = 8
        End If

        REM date at date
        If uxcurrent.Checked = True Then
            _ldoc.refresh_components.data_at_date = "9-9-9999"
        Else
            _ldoc.refresh_components.data_at_date = Me.uxdate.DateTime.ToUniversalTime
        End If

    End Sub

    Private Sub CheckEdit1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxcurrent.CheckedChanged



        If uxcurrent.CheckState = False Then
            Me.uxdate.Enabled = True
        Else
            Me.uxdate.Enabled = False
        End If
    End Sub

    Private Sub uxstage_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxstage.SelectedIndexChanged

    End Sub

    Private Sub LabelControl1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LabelControl1.Click

    End Sub

    Private Sub LabelControl2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LabelControl2.Click

    End Sub

    Private Sub uxdate_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxdate.EditValueChanged

    End Sub

    
    Private Sub uxparams_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxparams.Click
        Dim frefresh As New view_bc_dx_build_parameters
        Dim crefresh As New ctrll_bc_am_build_parameters(frefresh, bc_am_load_objects.obc_current_document, uxcomps.Selection.Item(0).Tag, True)
        If crefresh.load_data = False Then
            Dim omsg As New bc_cs_message("Blue Curve", "Failed to Load Display", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            Exit Sub
        End If
        frefresh.ShowDialog()
        If crefresh.cancel_selected = True Or crefresh.refresh = False Then
            Exit Sub
        End If
    End Sub

    
    Private Sub RepositoryItemCheckEdit1_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RepositoryItemCheckEdit1.CheckStateChanged

        For i = 0 To _ldoc.refresh_components.refresh_components.Count - 1
            If _ldoc.refresh_components.refresh_components(i).locator = Me.uxcomps.Selection.Item(0).Tag Then
                If Me.uxcomps.Selection.Item(0).GetValue(1) = True Then
                    _ldoc.refresh_components.refresh_components(i).no_refresh = 1
                Else
                    _ldoc.refresh_components.refresh_components(i).no_refresh = 0
                End If
                Exit For
            End If
        Next

    End Sub

    Private Sub RepositoryItemCheckEdit2_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RepositoryItemCheckEdit2.CheckStateChanged
        'For i = 0 To _ldoc.refresh_components.refresh_components.Count - 1
        '    If _ldoc.refresh_components.refresh_components(i).locator = Me.uxcomps.Selection.Item(0).Tag Then
        '        If Me.uxcomps.Selection.Item(0).GetValue(4) <> True Then
        '            _ldoc.refresh_components.refresh_components(i).disabled = 1
        '            _ldoc.refresh_components.refresh_components(i).no_refresh = 1
        '            Me.uxcomps.Selection.Item(0).SetValue(1, False)
        '        Else
        '            _ldoc.refresh_components.refresh_components(i).disabled = 0
        '            If (_ldoc.refresh_components.refresh_components(i).last_refresh_date = "09-09-9999" Or _ldoc.refresh_components.refresh_components(i).last_refresh_date < _ldoc.refresh_components.refresh_components(i).last_update_date) Then
        '                Me.uxcomps.Selection.Item(0).SetValue(1, True)
        '                _ldoc.refresh_components.refresh_components(i).no_refresh = 0
        '            End If
        '        End If
        '        Exit For
        '    End If
        'Next

    End Sub

   

    Private Sub uxcomps_ValidateNode(ByVal sender As Object, ByVal e As DevExpress.XtraTreeList.ValidateNodeEventArgs) Handles uxcomps.ValidateNode
    

        For i = 0 To _ldoc.refresh_components.refresh_components.Count - 1
            If _ldoc.refresh_components.refresh_components(i).locator = Me.uxcomps.Selection.Item(0).Tag Then
                For j = 0 To bc_am_load_objects.obc_pub_types.contributors.Count - 1
                    If Me.uxcomps.Selection.Item(0).GetValue(7) = bc_am_load_objects.obc_pub_types.contributors(j).name Then
                        _ldoc.refresh_components.refresh_components(i).contributor_id = bc_am_load_objects.obc_pub_types.contributors(j).id
                        Exit For
                    End If
                Next
                Exit For
            End If
        Next

    End Sub

    Private Sub RepositoryItemCheckEdit2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RepositoryItemCheckEdit2.Click
        For i = 0 To _ldoc.refresh_components.refresh_components.Count - 1
            If _ldoc.refresh_components.refresh_components(i).locator = Me.uxcomps.Selection.Item(0).Tag Then
                If Me.uxcomps.Selection.Item(0).GetValue(4) <> True Then
                    _ldoc.refresh_components.refresh_components(i).disabled = 1
                    _ldoc.refresh_components.refresh_components(i).no_refresh = 1
                    Me.uxcomps.Selection.Item(0).SetValue(1, False)
                Else
                    _ldoc.refresh_components.refresh_components(i).disabled = 0
                    If (_ldoc.refresh_components.refresh_components(i).last_refresh_date = "09-09-9999" Or _ldoc.refresh_components.refresh_components(i).last_refresh_date < _ldoc.refresh_components.refresh_components(i).last_update_date) Then
                        Me.uxcomps.Selection.Item(0).SetValue(1, True)
                        _ldoc.refresh_components.refresh_components(i).no_refresh = 0
                    End If
                End If
                Exit For
            End If
        Next

    End Sub
End Class

Public Class Cbc_ax_adv_doc_refresh
    Public cancel_selected As Boolean = True
    Public refresh_selected As Boolean = False
    Dim _ldoc As New bc_om_document
    WithEvents _view As Ibc_ax_adv_doc_refresh

    Public Sub New(ByVal view As Ibc_ax_adv_doc_refresh, ByVal ldoc As bc_om_document)
        _ldoc = ldoc
        _view = view
        _view.load_data(_ldoc)

    End Sub
    Public Sub refresh(ByVal ldoc As bc_om_document) Handles _view.doc_refresh
        cancel_selected = False
        refresh_selected = True
        save_metadata(ldoc)
    End Sub
    Public Sub save(ByVal ldoc As bc_om_document) Handles _view.save
        cancel_selected = False
        save_metadata(ldoc)
    End Sub

    Private Sub save_metadata(ByVal ldoc As bc_om_document)
        If ldoc.id = 0 Then
            Dim fn As String
            fn = Replace(ldoc.filename, ldoc.extension, "")
            ldoc.write_data_to_file(bc_cs_central_settings.local_repos_path + CStr(fn) + ".dat")
        Else
            ldoc.write_data_to_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ".dat")
        End If

    End Sub
End Class

Public Interface Ibc_ax_adv_doc_refresh
    Sub load_data(ByVal ldoc As bc_om_document)
    Event save(ByVal ldoc As bc_om_document)
    Event doc_refresh(ByVal ldoc As bc_om_document)
End Interface