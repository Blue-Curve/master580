Imports BlueCurve.Core.OM
Imports BlueCurve.Core.CS


Public Class bc_as_real_time_search
    Public Class_id As Long
    Public search_results As bc_om_real_time_search
    Public filter_attribute_id As Long = 0
    Public mine As Boolean = False
    Public inactive As Boolean = False
    Public selected_list As New List(Of Long)
    Public selected_entity As New bc_om_entity

    REM display
    Public bshow_in_list As Boolean = True
    Public bshowinactive = True
    Public bshowallmine = True
    Public bshowfilter = True
    Public select_on_double_click = False
    Public loading As Boolean = False
    'Dim fa As bc_om_filter_attribute_types

    Public Event entity_selected(entity As bc_om_entity)
    Public Event blank_search(mine As Boolean)
    Public Event search_complete(search_results As bc_om_real_time_search)

    Private Sub tsearchtext_EditValueChanged(sender As Object, e As EventArgs) Handles tsearchtext.EditValueChanged
        Timer1.Stop()
        Timer1.Start()
    End Sub
    Public Sub load_filter_attributes_for_class()
        Try


            Cursor = Windows.Forms.Cursors.WaitCursor
            Me.cfilter.Properties.Items.Clear()

            'fa = New bc_om_filter_attribute_types
            'If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
            '    fa.db_read()
            'Else
            '    fa.tmode = bc_cs_soap_base_class.tREAD
            '    If fa.transmit_to_server_and_receive(fa, True) = False Then
            '        Exit Sub
            '    End If
            'End If
            'Me.cfilter.Properties.Items.Clear()
            'Me.cfilter.Properties.Items.Add("All")
            'For i = 0 To fa.filter_attribute_types.Count - 1
            '    If fa.filter_attribute_types(i).class_id = Class_id Then
            '        Me.cfilter.Properties.Items.Add(fa.filter_attribute_types(i).display_name)
            '    End If
            'Next
            'Me.cfilter.SelectedIndex = 0
            'If Me.cfilter.Properties.Items.Count = 0 Then
            '    Me.cfilter.Visible = False
            '    Me.lfilter.Visible = False

            'Else
            '    Me.cfilter.Visible = True
            '    Me.lfilter.Visible = True
            'End If


        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_as_real_time_search", "load_filter_attributes_for_class", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            'Cursor = Windows.Forms.Cursors.Default

        End Try
    End Sub
    Public Sub run_search()
        Try
            If loading = True Then
                Exit Sub
            End If
            If bshow_in_list = True Then
                If LTrim(RTrim(Me.tsearchtext.Text)) = "" Then
                    RaiseEvent blank_search(mine)
                    Exit Sub
                End If
            Else
                If LTrim(RTrim(Me.ctestsearch.Text)) = "" Then
                    RaiseEvent blank_search(mine)
                    Exit Sub
                End If
            End If

            If Cursor <> Windows.Forms.Cursors.WaitCursor Then
                Cursor = Windows.Forms.Cursors.WaitCursor
            End If

            If bshow_in_list = True Then
                Me.lresults.BeginUpdate()
                Me.lresults.Items.Clear()
                Me.lresults.EndUpdate()
            End If
            search_results = New bc_om_real_time_search
            search_results.class_id = Class_id
            search_results.mine = mine
            search_results.inactive = inactive
            search_results.filter_attribute_id = 0

            'If Me.cfilter.SelectedIndex > 0 Then
            '    search_results.filter_attribute_id = fa.filter_attribute_types(Me.cfilter.SelectedIndex - 1).attribute_id
            'End If
            If bshow_in_list = True Then
                search_results.search_text = Me.tsearchtext.Text
            Else
                search_results.search_text = Me.ctestsearch.Text

            End If
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                search_results.db_read()
            Else
                search_results.tmode = bc_cs_soap_base_class.tREAD
                If search_results.transmit_to_server_and_receive(search_results, True) = False Then
                    Exit Sub
                End If
            End If
            Dim selected As Boolean
            If bshow_in_list = True Then
                Me.lresults.BeginUpdate()
                Me.lresults.Items.Clear()
            Else
                Me.ctestsearch.Properties.Items.Clear()
                Me.ctestsearch.Properties.BeginUpdate()
            End If
            For i = 0 To search_results.results.Count - 1
                selected = False
                For j = 0 To selected_list.Count - 1
                    If selected_list(j) = search_results.results(i).id Then
                        selected = True
                        Exit For
                    End If
                Next
                If selected = False Then
                    If (search_results.results(i).inactive) = True Then
                        If bshow_in_list = True Then
                            Me.lresults.Items.Add(search_results.results(i).name + " (inactive)")
                        Else
                            Me.ctestsearch.Properties.Items.Add(search_results.results(i).name + " (inactive)")
                        End If
                    Else
                        If bshow_in_list = True Then
                            Me.lresults.Items.Add(search_results.results(i).name)
                        Else
                            Me.ctestsearch.Properties.Items.Add(search_results.results(i).name)
                        End If
                    End If
                End If

            Next
            If bshow_in_list = True Then
                Me.lresults.EndUpdate()
            Else
                Me.ctestsearch.Properties.EndUpdate()
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_as_real_time_search", "RunSearch", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Cursor = Windows.Forms.Cursors.Default

        End Try
    End Sub
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Timer1.Stop()
        run_search()
    End Sub

    Private Sub pclear_EditValueChanged(sender As Object, e As EventArgs) Handles pclear.Click
        Me.tsearchtext.Text = ""
    End Sub

    Private Sub bc_as_real_time_search_Load(sender As Object, e As EventArgs) Handles Me.Load
        Timer1.Stop()
        If Me.bshowinactive = False Then
            Me.lresults.Height = Me.lresults.Height + 22
        End If
        If bshow_in_list = False Then
            Me.lresults.Visible = False
            Me.tsearchtext.Visible = False
            Me.ctestsearch.Visible = True
        Else
            Me.lresults.Visible = True
            Me.tsearchtext.Visible = True
            Me.ctestsearch.Visible = False
        End If


    End Sub
    Private Sub lresults_Click(sender As Object, e As EventArgs) Handles lresults.Click
        If select_on_double_click = True Then
            Exit Sub
        End If
        RaiseEvent entity_selected(search_results.results(Me.lresults.SelectedIndex))
    End Sub

    Private Sub lresults_DoubleClick(sender As Object, e As EventArgs) Handles lresults.DoubleClick
        If select_on_double_click = False Then
            Exit Sub
        End If
        RaiseEvent entity_selected(search_results.results(Me.lresults.SelectedIndex))
    End Sub

    Private Sub rallmine_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rallmine.SelectedIndexChanged
        If rallmine.SelectedIndex = 0 Then
            mine = False
        Else
            mine = True
        End If
        run_search()

    End Sub

    Private Sub cinactive_CheckedChanged(sender As Object, e As EventArgs) Handles cinactive.CheckedChanged
        If Me.cinactive.Checked = True Then
            inactive = True
        Else
            inactive = False
        End If
        run_search()
    End Sub



    Private Sub ctestsearch_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ctestsearch.EditValueChanged
        run_search()
    End Sub

    Private Sub cfilter_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cfilter.SelectedIndexChanged
        run_search()
    End Sub

    Private Sub pclear_EditValueChanged_1(sender As Object, e As EventArgs) Handles pclear.EditValueChanged

    End Sub

    Private Sub ctestsearch_SelectedIndexChanged_1(sender As Object, e As EventArgs) Handles ctestsearch.SelectedIndexChanged

    End Sub
End Class




