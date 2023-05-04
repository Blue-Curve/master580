Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Public Class bc_dx_waterfall
    Implements Ibc_dx_waterfall
    Dim _waterfall As bc_om_contributor_waterfall
    Dim _excl As bc_om_universe_excl
    Event save(waterfalll As bc_om_contributor_waterfall) Implements Ibc_dx_waterfall.save

    Public Sub hideform() Implements Ibc_dx_waterfall.hideform


        Me.Hide()
    End Sub
    Public Function load_view(waterfalll As bc_om_contributor_waterfall, excl As bc_om_universe_excl) As Boolean Implements Ibc_dx_waterfall.load_view
        load_view = False
        Try
            _waterfall = waterfalll
            _excl = excl
            If _excl.contributors(_excl.contributors.Count - 1).id <> -1 Then
                Dim ec As New bc_om_contributor(-1, "Nothing")
                _excl.contributors.Add(ec)
            End If
            For i = 0 To _excl.contributors.Count - 1
                uxc.Properties.Items.Add(_excl.contributors(i).name)
            Next
          
            load_tree()
            load_view = True
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_dx_waterfall", "load_view", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Function

    Private Sub bok_Click(sender As Object, e As EventArgs) Handles bcancel.Click
        Me.Hide()
    End Sub


    Private Sub bsave_Click(sender As Object, e As EventArgs) Handles bsave.Click
        Try
            Me.Cursor = Windows.Forms.Cursors.WaitCursor
            RaiseEvent save(_waterfall)

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_dx_waterfall", "save", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            Me.Cursor = Windows.Forms.Cursors.Default

        End Try
    End Sub
    Sub check_save()
        Me.badd.Enabled = False
        If Me.uxc.SelectedIndex > -1 Then
            Me.badd.Enabled = True

        End If
    End Sub
    Private Sub uxc_SelectedIndexChanged(sender As Object, e As EventArgs) Handles uxc.SelectedIndexChanged

        check_save()
        
    End Sub

   

    Private Sub badd_Click(sender As Object, e As EventArgs) Handles badd.Click
        Dim c As New bc_om_contributor_waterfall_alt
        _waterfall.alternates.Add(c)
        For i = 0 To _excl.contributors.Count - 1

            If Me.uxc.Text = _excl.contributors(i).name Then
                If Me.uxres.Nodes.Count = 0 Then
                    _waterfall.alternates(0).contributor_id = _excl.contributors(i).id
                Else
                    _waterfall.alternates(_waterfall.alternates.Count - 2).alt_contributor_id = _excl.contributors(i).id
                    _waterfall.alternates(_waterfall.alternates.Count - 1).contributor_id = _excl.contributors(i).id

                End If
            End If

        Next


        load_tree()
        Me.uxc.SelectedIndex = -1

        check_save()
    End Sub
    Sub check_all_save()
        Me.bsave.Enabled = False
        If _waterfall.alternates.Count = 0 Then
            Me.bsave.Enabled = True
            Exit Sub
        End If
        If _waterfall.alternates.Count < 2 Then
            Exit Sub
        End If
        For i = 0 To _waterfall.alternates.Count - 2
            If _waterfall.alternates(i).rules.Count = 0 Then
                Exit Sub
            End If
        Next
        Me.bsave.Enabled = True

    End Sub
    Private Sub load_tree()
        Me.uxres.BeginUpdate()
        Me.uxres.Nodes.Clear()
        Try
            Dim cn As String
            For i = 0 To _waterfall.alternates.Count - 1

                For j = 0 To _excl.contributors.Count - 1
                    If _waterfall.alternates(i).contributor_id = _excl.contributors(j).id Then
                        cn = _excl.contributors(j).name

                    End If


                Next

                uxres.Nodes.Add("")
                If i = 0 Then
                    uxres.Nodes(i).SetValue(0, "Primary")
                    uxres.Nodes(i).SetValue(1, cn)

                Else
                    uxres.Nodes(i).SetValue(0, "Alternate: " + CStr(i))
                    uxres.Nodes(i).SetValue(1, cn)
                End If
                uxres.Nodes(i).Tag = i
                For j = 0 To _waterfall.alternates(i).rules.Count - 1
                    For k = 0 To _excl.attribute_excls_types.Count - 1
                        If _waterfall.alternates(i).rules(j) = _excl.attribute_excls_types(k).id Then
                            uxres.Nodes(i).Nodes.Add("")
                            uxres.Nodes(i).Nodes(uxres.Nodes(i).Nodes.Count - 1).SetValue(0, "  rule")
                            uxres.Nodes(i).Nodes(uxres.Nodes(i).Nodes.Count - 1).SetValue(1, "  " + _excl.attribute_excls_types(k).name)
                            uxres.Nodes(i).Nodes(uxres.Nodes(i).Nodes.Count - 1).Tag = _excl.attribute_excls_types(k).id
                            Exit For
                        End If
                    Next
                Next

            Next
            Me.uxres.ExpandAll()
            Me.uxres.EndUpdate()

            Me.uxc.Properties.Items.Clear()
            If uxres.Nodes.Count > 0 AndAlso uxres.Nodes(uxres.Nodes.Count - 1).GetValue(1) = "Nothing" Then
                Exit Sub
            End If

            Dim found As Boolean
            For i = 0 To _excl.contributors.Count - 1
                found = False

                For j = 0 To Me.uxres.Nodes.Count - 1
                    If uxres.Nodes(j).GetValue(1) = _excl.contributors(i).name Then
                        found = True
                    End If
                Next
                If found = False Then
                    Me.uxc.Properties.Items.Add(_excl.contributors(i).name)

                End If
            Next
            Me.prules.Enabled = False

        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            check_all_save()
        End Try


    End Sub



    Private Sub uxres_FocusedNodeChanged(sender As Object, e As DevExpress.XtraTreeList.FocusedNodeChangedEventArgs) Handles uxres.FocusedNodeChanged
        Try
            Me.bdelrule.Enabled = False
            If Me.uxres.Nodes.Count = 0 Then
                Exit Sub
            End If
            If IsNumeric(e.Node.Tag) = False Then
                Exit Sub
            End If
            Dim parent As Boolean = False

            Try
                Dim k As String
                k = e.Node.ParentNode.Tag
            Catch ex As Exception
                parent = True
            End Try
            If e.Node.Tag = "0" Or parent = False Then
                Me.lrules.Text = "Rules"
                Me.prules.Enabled = False
                If e.Node.Tag <> "0" Then
                    Me.bdelrule.Enabled = True
                End If
                Exit Sub
            End If
            Me.prules.Enabled = True
            Me.lrules.Text = "Rules " + Me.uxres.Nodes(CInt(e.Node.Tag) - 1).GetValue(1) + " to " + Me.uxres.Nodes(CInt(e.Node.Tag)).GetValue(1)
            Me.uxrules.Properties.Items.Clear()
            Me.uxrules.SelectedIndex = -1
            Me.baddrule.Enabled = False
            Dim found As Boolean
            For i = 0 To _excl.attribute_excls_types.Count - 1
                found = False
                For j = 0 To _waterfall.alternates(CInt(e.Node.Tag) - 1).rules.Count - 1

                    If _waterfall.alternates(CInt(e.Node.Tag) - 1).rules(j) = _excl.attribute_excls_types(i).id Then
                        found = True
                        Exit For
                    End If
                Next
                If found = False Then
                    Me.uxrules.Properties.Items.Add(_excl.attribute_excls_types(i).name)
                End If
            Next
        Catch ex As Exception
            MsgBox("b: " + ex.Message)
        End Try
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        If Me.uxres.Nodes.Count = 0 Then
            Exit Sub
        End If

        _waterfall.alternates.RemoveAt(Me.uxres.Nodes.Count - 1)
        If Me.uxres.Nodes.Count > 1 Then
            _waterfall.alternates(Me.uxres.Nodes.Count - 2).alt_contributor_id = 0
            _waterfall.alternates(Me.uxres.Nodes.Count - 2).rules.Clear()
        End If
        load_tree()
    End Sub

    Private Sub uxrules_SelectedIndexChanged(sender As Object, e As EventArgs) Handles uxrules.SelectedIndexChanged
        Me.baddrule.Enabled = False
        If Me.uxrules.SelectedIndex > -1 Then
            Me.baddrule.Enabled = True
        End If
    End Sub

    Private Sub baddrule_Click(sender As Object, e As EventArgs) Handles baddrule.Click
        Try
            For i = 0 To _excl.attribute_excls_types.Count - 1
                If Me.uxrules.Text = _excl.attribute_excls_types(i).name Then

                    _waterfall.alternates(CInt(Me.uxres.Selection(0).Tag) - 1).rules.Add(_excl.attribute_excls_types(i).id)

                    load_tree()

                    Exit For
                End If
            Next
        Catch

        End Try
    End Sub

    Private Sub bdelrule_Click(sender As Object, e As EventArgs) Handles bdelrule.Click
        For i = 0 To _waterfall.alternates(CInt(uxres.Selection(0).ParentNode.Tag)).rules.Count - 1
            If _waterfall.alternates(CInt(uxres.Selection(0).ParentNode.Tag)).rules(i) = uxres.Selection(0).Tag Then
                _waterfall.alternates(CInt(uxres.Selection(0).ParentNode.Tag)).rules.RemoveAt(i)
                Exit For
            End If
        Next
        load_tree()
    End Sub
End Class
Public Class Cbc_dx_waterfall

    WithEvents _view As Ibc_dx_waterfall
    Public Function load_data(view As Ibc_dx_waterfall, universe_id As Long, ByVal excl As bc_om_universe_excl)
        load_data = False

        Try
            _view = view
            Dim _waterfall As New bc_om_contributor_waterfall
            _waterfall.universe_id = universe_id
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                _waterfall.db_read()
            Else
                _waterfall.tmode = bc_cs_soap_base_class.tREAD

                If _waterfall.transmit_to_server_and_receive(_waterfall, True) = False Then
                    'Exit Function
                End If

            End If
            load_data = _view.load_view(_waterfall, excl)


        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Cbc_dx_waterfall", "load_data", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Function

    Public Function save(_waterfall As bc_om_contributor_waterfall) As Boolean Handles _view.save
        Try
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                _waterfall.db_write()
            Else
                _waterfall.tmode = bc_cs_soap_base_class.tWRITE

                If _waterfall.transmit_to_server_and_receive(_waterfall, True) = True Then
                    save = True
                    _view.hideform()
                End If
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Cbc_dx_waterfall", "load_data", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Function
End Class

Public Interface Ibc_dx_waterfall
    Function load_view(waterfalll As bc_om_contributor_waterfall, excl As bc_om_universe_excl) As Boolean
    Event save(waterfalll As bc_om_contributor_waterfall)
    Sub hideform()
End Interface
