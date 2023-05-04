Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM


Public Class bc_am_distribution_admin
    Implements Ibc_am_distribution_admin
    Dim _pt_dist As bc_om_distribution_for_pub_type
    Event save(pt_dist As bc_om_distribution_for_pub_type) Implements Ibc_am_distribution_admin.save
    Dim _sel_channel As Integer = 0
    Public Function load_view(pt_dist As bc_om_distribution_for_pub_type) As Boolean Implements Ibc_am_distribution_admin.load_view
        load_view = False
        Try
            _pt_dist = pt_dist
            If _pt_dist.automatic = True Then
                Me.rtype.SelectedIndex = 1
            End If
            load_lists()
            For i = 0 To _pt_dist.email_templates.Count - 1
                Me.uxheadertemplate.Properties.Items.Add(_pt_dist.email_templates(i).filename)
                Me.uxbodytemplate.Properties.Items.Add(_pt_dist.email_templates(i).filename)
            Next
            load_view = True
        Catch

        End Try


    End Function

    Private Sub bcancel_Click(sender As Object, e As EventArgs) Handles bcancel.Click
        Me.Close()
    End Sub
    Private Sub load_lists()
        Me.ptemplate.Enabled = False
        Dim found As Boolean = False
        Me.uxall.Items.Clear()
        Me.uxsel.Items.Clear()
        For i = 0 To _pt_dist.channels.Count - 1
            found = False
            For j = 0 To _pt_dist.sel_channels.Count - 1
                If _pt_dist.channels(i).channel_id = _pt_dist.sel_channels(j).channel_id Then
                    Me.uxsel.Items.Add(_pt_dist.channels(i).name)
                    found = True
                    Exit For
                End If
            Next
            If found = False Then
                Me.uxall.Items.Add(_pt_dist.channels(i).name)
            End If

        Next
    End Sub
    Private Sub uxsel_DoubleClick(sender As Object, e As EventArgs) Handles uxsel.DoubleClick
        If uxsel.Items.Count = 0 Then
            Exit Sub
        End If
        For i = 0 To _pt_dist.channels.Count - 1
            For j = 0 To _pt_dist.sel_channels.Count - 1
                If _pt_dist.channels(i).channel_id = _pt_dist.sel_channels(j).channel_id AndAlso _pt_dist.channels(i).name = Me.uxsel.SelectedItem.ToString Then
                    _pt_dist.sel_channels.RemoveAt(j)
                    load_lists()
                    Return
                End If
            Next

        Next
    End Sub
    Private Sub uxall_DoubleClick(sender As Object, e As EventArgs) Handles uxall.DoubleClick
        If uxall.Items.Count = 0 Then
            Exit Sub
        End If
        Dim c As bc_om_distribution.bc_om_doc_distribution_channel
        For i = 0 To _pt_dist.channels.Count - 1
            If _pt_dist.channels(i).name = Me.uxall.SelectedItem.ToString Then
                c = New bc_om_distribution.bc_om_doc_distribution_channel
                c.channel_id = _pt_dist.channels(i).channel_id
                _pt_dist.sel_channels.Add(c)
                load_lists()
                Return
            End If
        Next
    End Sub

    Private Sub uxsel_SelectedIndexChanged(sender As Object, e As EventArgs) Handles uxsel.SelectedIndexChanged
        If Me.uxsel.Items.Count = 0 Then
            Exit Sub
        End If
        Me.ptemplate.Enabled = False
        Me.uxheadertemplate.SelectedIndex = -1
        Me.uxbodytemplate.SelectedIndex = -1


        For i = 0 To _pt_dist.channels.Count - 1
            If _pt_dist.channels(i).name = Me.uxsel.SelectedItem.ToString AndAlso _pt_dist.channels(i).targetted = True Then
                For j = 0 To _pt_dist.sel_channels.Count - 1
                    If _pt_dist.channels(i).channel_id = _pt_dist.sel_channels(j).channel_id Then
                        _sel_channel = _pt_dist.channels(i).channel_id
                        If _pt_dist.sel_channels(j).title_template <> 0 Then
                            For k = 0 To _pt_dist.email_templates.Count - 1
                                If _pt_dist.email_templates(k).id = _pt_dist.sel_channels(j).title_template Then
                                    Me.uxheadertemplate.Text = _pt_dist.email_templates(k).filename
                                    Exit For
                                End If
                            Next
                        End If

                        If _pt_dist.sel_channels(j).body_template <> 0 Then
                            For k = 0 To _pt_dist.email_templates.Count - 1
                                If _pt_dist.email_templates(k).id = _pt_dist.sel_channels(j).body_template Then
                                    Me.uxbodytemplate.Text = _pt_dist.email_templates(k).filename
                                    Exit For
                                End If
                            Next
                        End If
                    End If
                Next


                Me.ptemplate.Enabled = True

            End If
        Next
    End Sub

    Private Sub uxheadertemplate_SelectedIndexChanged(sender As Object, e As EventArgs) Handles uxheadertemplate.SelectedIndexChanged
        If Me.uxheadertemplate.SelectedIndex = -1 Then
            Exit Sub
        End If
        For i = 0 To _pt_dist.sel_channels.Count - 1
            If _sel_channel = _pt_dist.sel_channels(i).channel_id Then
                For j = 0 To _pt_dist.email_templates.Count - 1
                    If _pt_dist.email_templates(j).filename = uxheadertemplate.Text Then
                        _pt_dist.sel_channels(i).title_template = _pt_dist.email_templates(j).id
                        Exit For
                    End If
                Next
                Exit For
            End If
        Next

     
    End Sub
    Private Sub uxbodytemplate_SelectedIndexChanged(sender As Object, e As EventArgs) Handles uxbodytemplate.SelectedIndexChanged
        If Me.uxbodytemplate.SelectedIndex = -1 Then
            Exit Sub
        End If
        For i = 0 To _pt_dist.sel_channels.Count - 1
            If _sel_channel = _pt_dist.sel_channels(i).channel_id Then
                For j = 0 To _pt_dist.email_templates.Count - 1
                    If _pt_dist.email_templates(j).filename = uxbodytemplate.Text Then
                        _pt_dist.sel_channels(i).body_template = _pt_dist.email_templates(j).id
                        Exit For
                    End If
                Next
                Exit For
            End If
        Next
       

    End Sub

    Private Sub bsave_Click(sender As Object, e As EventArgs) Handles bsave.Click
        _pt_dist.automatic = Me.rtype.SelectedIndex
        RaiseEvent save(_pt_dist)
        Me.Hide()
    End Sub
End Class
Public Class Cbc_am_distribution_admin
    WithEvents _view As Ibc_am_distribution_admin

    Dim _pt_dist As New bc_om_distribution_for_pub_type
    Public Function load_data(view As Ibc_am_distribution_admin, pub_type_id As Long)
        load_data = False
        Try
            _view = view
            _pt_dist.pub_type_id = pub_type_id
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                _pt_dist.db_read()
            Else
                _pt_dist.tmode = bc_cs_soap_base_class.tREAD
                If _pt_dist.transmit_to_server_and_receive(_pt_dist, True) = False Then
                    Exit Function
                End If
            End If
            load_data = _view.load_view(_pt_dist)
        Catch

        End Try

    End Function
    Private Sub save(_pt_dist As bc_om_distribution_for_pub_type) Handles _view.save
        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
            _pt_dist.db_write()
        Else
            _pt_dist.tmode = bc_cs_soap_base_class.tWRITE
            If _pt_dist.transmit_to_server_and_receive(_pt_dist, True) = False Then
                Exit Sub
            End If
        End If
    End Sub
End Class
Public Interface Ibc_am_distribution_admin
    Function load_view(pt_dist As bc_om_distribution_for_pub_type) As Boolean
    Event save(pt_dist As bc_om_distribution_for_pub_type)
End Interface