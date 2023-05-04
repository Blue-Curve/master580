
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms
Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports DevExpress.XtraTreeList.TreeList

Imports System.ServiceModel
Imports System.ServiceModel.Channels
Imports System.Net.NetworkInformation




Public Class bc_dx_agg_preview
    Implements Ibc_am_aggs_preview

    Public Sub bc_dx_agg_preview()
        InitializeComponent()
    End Sub

    Dim _universes As bc_om_universes_preview
    Dim bloading As Boolean = True

    Dim _controller As Cbc_am_aggs_preview
    Dim _abc_calc_agg As List(Of ServiceReference1.abc_calc_agg)
    Dim _abc_calc_agg_growths As List(Of ServiceReference1.abc_calc_agg)
    Dim _abc_calc_agg_cc As List(Of ServiceReference1.abc_calc_agg)
    Dim results As List(Of ServiceReference1.agg_result)
    Dim ttest_results As List(Of ServiceReference1.ttest_result)


    

    Public Sub load_results(lresults As ServiceReference1.agg_results, ec As Boolean) Implements Ibc_am_aggs_preview.load_results
        Try
            If ec = True Then
                Me.uxic.Checked = False
            End If
            Me.uxstyle.Visible = False



            results = New List(Of ServiceReference1.agg_result)
            ttest_results = New List(Of ServiceReference1.ttest_result)
            If Not IsNothing(lresults.ttest) Then
                Me.uxstyle.Visible = True
                Dim ttest_result As ServiceReference1.ttest_result
                For i = 0 To lresults.ttest.Count - 1
                    ttest_result = New ServiceReference1.ttest_result
                    ttest_result.weighted_mean = lresults.ttest(i).weighted_mean
                    ttest_result.standard_deviation = lresults.ttest(i).standard_deviation
                    ttest_result.eff_number = lresults.ttest(i).eff_number
                    ttest_result.contributor_id = lresults.ttest(i).contributor_id
                    ttest_result.item_id = lresults.ttest(i).item_id
                    ttest_result.year = lresults.ttest(i).year
                    ttest_result.workflow_stage = lresults.ttest(i).workflow_stage
                    ttest_results.Add(ttest_result)
                Next
            End If


            _abc_calc_agg = New List(Of ServiceReference1.abc_calc_agg)
            _abc_calc_agg_growths = New List(Of ServiceReference1.abc_calc_agg)
            _abc_calc_agg_cc = New List(Of ServiceReference1.abc_calc_agg)
            Dim result As ServiceReference1.agg_result
            Dim abc_calc_agg As ServiceReference1.abc_calc_agg
            If Not IsNothing(lresults.results) Then
                For i = 0 To lresults.results.Count - 1
                    result = New ServiceReference1.agg_result
                    result.value = lresults.results(i).value
                    result.contributor_id = lresults.results(i).contributor_id
                    result.item_id = lresults.results(i).item_id
                    result.year = lresults.results(i).year
                    result.workflow_stage = lresults.results(i).workflow_stage
                    results.Add(result)
                Next
            End If



            If Not IsNothing(lresults.abc_calc_agg) Then
                For i = 0 To lresults.abc_calc_agg.Count - 1
                    abc_calc_agg = New ServiceReference1.abc_calc_agg
                    abc_calc_agg.result_row_id = lresults.abc_calc_agg(i).result_row_id
                    abc_calc_agg.year = lresults.abc_calc_agg(i).year
                    abc_calc_agg.contributor_id = lresults.abc_calc_agg(i).contributor_id
                    abc_calc_agg.value_1 = lresults.abc_calc_agg(i).value_1
                    abc_calc_agg.value_2 = lresults.abc_calc_agg(i).value_2
                    abc_calc_agg.value_3 = lresults.abc_calc_agg(i).value_3
                    abc_calc_agg.value_4 = lresults.abc_calc_agg(i).value_4
                    abc_calc_agg.value_5 = lresults.abc_calc_agg(i).value_5
                    abc_calc_agg.value_6 = lresults.abc_calc_agg(i).value_6
                    abc_calc_agg.value_7 = lresults.abc_calc_agg(i).value_7
                    abc_calc_agg.value_8 = lresults.abc_calc_agg(i).value_8
                    abc_calc_agg.entity_id = lresults.abc_calc_agg(i).entity_id
                    _abc_calc_agg.Add(abc_calc_agg)
                Next
            End If
            If Not IsNothing(lresults.abc_calc_agg_growths) Then
                For i = 0 To lresults.abc_calc_agg_growths.Count - 1
                    abc_calc_agg = New ServiceReference1.abc_calc_agg
                    abc_calc_agg.result_row_id = lresults.abc_calc_agg_growths(i).result_row_id
                    abc_calc_agg.year = lresults.abc_calc_agg_growths(i).year
                    abc_calc_agg.contributor_id = lresults.abc_calc_agg_growths(i).contributor_id
                    abc_calc_agg.value_1 = lresults.abc_calc_agg_growths(i).value_1
                    abc_calc_agg.value_2 = lresults.abc_calc_agg_growths(i).value_2
                    abc_calc_agg.value_3 = lresults.abc_calc_agg_growths(i).value_3
                    abc_calc_agg.value_4 = lresults.abc_calc_agg_growths(i).value_4
                    abc_calc_agg.value_5 = lresults.abc_calc_agg_growths(i).value_5
                    abc_calc_agg.value_6 = lresults.abc_calc_agg_growths(i).value_6
                    abc_calc_agg.value_7 = lresults.abc_calc_agg_growths(i).value_7
                    abc_calc_agg.value_8 = lresults.abc_calc_agg_growths(i).value_8
                    abc_calc_agg.entity_id = lresults.abc_calc_agg_growths(i).entity_id
                    abc_calc_agg.include_in_growthr = lresults.abc_calc_agg_growths(i).include_in_growthr
                    abc_calc_agg.include_in_growthl = lresults.abc_calc_agg_growths(i).include_in_growthl
                    abc_calc_agg.num_years = lresults.abc_calc_agg_growths(i).num_years
                    _abc_calc_agg_growths.Add(abc_calc_agg)
                Next
            End If
            If Not IsNothing(lresults.abc_calc_agg_cc) Then
                For i = 0 To lresults.abc_calc_agg_cc.Count - 1
                    abc_calc_agg = New ServiceReference1.abc_calc_agg
                    abc_calc_agg.result_row_id = lresults.abc_calc_agg_cc(i).result_row_id
                    abc_calc_agg.year = lresults.abc_calc_agg_cc(i).year
                    abc_calc_agg.contributor_id = lresults.abc_calc_agg_cc(i).contributor_id
                    abc_calc_agg.value_1 = lresults.abc_calc_agg_cc(i).value_1
                    abc_calc_agg.value_2 = lresults.abc_calc_agg_cc(i).value_2
                    abc_calc_agg.value_3 = lresults.abc_calc_agg_cc(i).value_3
                    abc_calc_agg.value_4 = lresults.abc_calc_agg_cc(i).value_4
                    abc_calc_agg.value_5 = lresults.abc_calc_agg_cc(i).value_5
                    abc_calc_agg.value_6 = lresults.abc_calc_agg_cc(i).value_6
                    abc_calc_agg.value_7 = lresults.abc_calc_agg_cc(i).value_7
                    abc_calc_agg.value_8 = lresults.abc_calc_agg_cc(i).value_8
                    abc_calc_agg.entity_id = lresults.abc_calc_agg_cc(i).entity_id
                    abc_calc_agg.contributor_1_id = lresults.abc_calc_agg_cc(i).contributor_1_id
                    abc_calc_agg.contributor_2_id = lresults.abc_calc_agg_cc(i).contributor_2_id
                    _abc_calc_agg_cc.Add(abc_calc_agg)
                Next
            End If
            Me.uxstyle.SelectedIndex = 0
            set_results()
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_dx_agg_preview", "load_results", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub



    Sub set_results(Optional ttest As Boolean = False)
        If bloading = True Then
            Exit Sub
        End If
        uxres3.Nodes.Clear()
        uxconts1.Nodes.Clear()

        Try
            Dim nc As Integer = 0

            Dim stage_id As Integer = 8
            uxres3.Columns(2).Visible = False
            uxres3.Columns(3).Visible = False
            uxres3.Columns(4).Visible = False
            uxres3.Columns(5).Visible = False



            If uxstage.Text = "Draft" Then
                stage_id = 1
            End If
            If ttest = False Then
                uxres3.Columns(2).Visible = True
                If (results.Count <> 0) Then

                    uxres3.BeginUnboundLoad()
                    uxres3.Nodes.Clear()

                    XtraTabControl1.TabPages(0).Text = "Results (" + results.Count.ToString() + ")"
                    For i = 0 To results.Count - 1
                        If (results(i).workflow_stage = stage_id And results(i).contributor_id = _universes.luniverses(uxuniverse.SelectedIndex).contributors(uxcont.SelectedIndex).contributor_id) Then
                            uxres3.Nodes.Add(results(i).value)
                            For j = 0 To _universes.luniverses(uxuniverse.SelectedIndex).metrics.Count - 1
                                If (_universes.luniverses(uxuniverse.SelectedIndex).metrics(j).metric_id = results(i).item_id) Then
                                    uxres3.Nodes(nc).SetValue(0, _universes.luniverses(uxuniverse.SelectedIndex).metrics(j).metric_name)
                                    uxres3.Nodes(nc).Tag = _universes.luniverses(uxuniverse.SelectedIndex).metrics(j).metric_id
                                    Exit For
                                End If
                            Next
                            uxres3.Nodes(nc).SetValue(1, results(i).year)
                            uxres3.Nodes(nc).SetValue(2, results(i).value)
                            nc = nc + 1
                        End If
                    Next
                End If
            Else
                uxres3.Columns(3).Visible = True
                uxres3.Columns(4).Visible = True
                uxres3.Columns(5).Visible = True
                If (ttest_results.Count <> 0) Then

                    uxres3.BeginUnboundLoad()
                    uxres3.Nodes.Clear()

                    XtraTabControl1.TabPages(0).Text = "Style Results (" + ttest_results.Count.ToString() + ")"
                    For i = 0 To ttest_results.Count - 1
                        If (ttest_results(i).workflow_stage = stage_id And ttest_results(i).contributor_id = _universes.luniverses(uxuniverse.SelectedIndex).contributors(uxcont.SelectedIndex).contributor_id) Then
                            uxres3.Nodes.Add(ttest_results(i).weighted_mean)

                            For j = 0 To _universes.luniverses(uxuniverse.SelectedIndex).metrics.Count - 1
                                If (_universes.luniverses(uxuniverse.SelectedIndex).metrics(j).metric_id = ttest_results(i).item_id) Then
                                    uxres3.Nodes(nc).SetValue(0, _universes.luniverses(uxuniverse.SelectedIndex).metrics(j).metric_name)
                                    uxres3.Nodes(nc).Tag = _universes.luniverses(uxuniverse.SelectedIndex).metrics(j).metric_id
                                    Exit For
                                End If
                            Next
                            uxres3.Nodes(nc).SetValue(1, ttest_results(i).year)
                            uxres3.Nodes(nc).SetValue(3, ttest_results(i).weighted_mean)
                            uxres3.Nodes(nc).SetValue(4, ttest_results(i).standard_deviation)
                            uxres3.Nodes(nc).SetValue(5, ttest_results(i).eff_number)
                            nc = nc + 1
                        End If
                    Next
                End If
            End If
        Catch ex As Exception

            Dim oerr As New bc_cs_error_log("bc_dx_agg_preview", "set_results", bc_cs_error_codes.USER_DEFINED, ex.Message)


        Finally
            uxres3.EndUnboundLoad()
            Me.SimpleButton1.Enabled = True
            Me.SimpleButton2.Enabled = True
        End Try
    End Sub


    Public Function load_view(universes As bc_om_universes_preview, controller As Cbc_am_aggs_preview) As Boolean Implements Ibc_am_aggs_preview.load_view
        Try
            load_view = True
            XtraTabPage2.PageVisible = False

            _controller = controller
            _universes = universes
            Dim i As Integer
            For i = 0 To universes.luniverses.Count - 1
                uxuniverse.Properties.Items.Add(universes.luniverses(i).universe_name)
            Next

            uxstage.Properties.Items.Add("Publish")
            uxstage.Enabled = False
            uxstage.SelectedIndex = 0

            If _universes.luniverses.Count = 1 Then
                uxuniverse.SelectedIndex = 0
                uxuniverse.Enabled = False
            End If

            load_view = True
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_dx_agg_preview", "load_view", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            bloading = False

        End Try

    End Function

    Private Sub bcancel_Click(sender As Object, e As EventArgs) Handles bcancel.Click
        Me.Close()
    End Sub







    Private Sub uxuniverse_SelectedIndexChanged(sender As Object, e As EventArgs) Handles uxuniverse.SelectedIndexChanged


        uxtargetclass.Properties.Items.Clear()
        uxentity.Properties.Items.Clear()
        uxdual.Properties.Items.Clear()
        uxcont.Properties.Items.Clear()
        uxexch.Properties.Items.Clear()
        uxtype.Properties.Items.Clear()
        uxtargetclass.SelectedIndex = -1
        uxentity.SelectedIndex = -1
        uxdual.SelectedIndex = -1
        uxcont.SelectedIndex = -1
        uxexch.SelectedIndex = -1
        uxtype.SelectedIndex = -1
        If uxuniverse.SelectedIndex > -1 Then

            Dim a As String
            Dim i As Integer

            If (_universes.luniverses(uxuniverse.SelectedIndex).exch_rate_method = 0 Or _universes.luniverses(uxuniverse.SelectedIndex).exch_rate_method = 3) Then
                uxexch.Properties.Items.Add("Current")
            End If
            If (_universes.luniverses(uxuniverse.SelectedIndex).exch_rate_method = 1) Then
                uxexch.Properties.Items.Add("Period End")
            End If

            If (_universes.luniverses(uxuniverse.SelectedIndex).exch_rate_method = 2 Or _universes.luniverses(uxuniverse.SelectedIndex).exch_rate_method = 3) Then
                uxexch.Properties.Items.Add("Period Average")
            End If
            uxexch.SelectedIndex = 0
            If (uxexch.Properties.Items.Count = 1) Then
                uxexch.Enabled = False
            Else
                uxexch.Enabled = True
            End If
            Dim bsty As Boolean = False
            Dim bac As Boolean = False
            For i = 0 To _universes.luniverses(uxuniverse.SelectedIndex).calc_types.Count - 1
                If (_universes.luniverses(uxuniverse.SelectedIndex).calc_types(i) = "aggregate style" Or _universes.luniverses(uxuniverse.SelectedIndex).calc_types(i) = "aggregate style use mean") Then
                    bsty = True
                Else
                    If _universes.luniverses(uxuniverse.SelectedIndex).calc_types(i) = "aggregate calenderized" Then
                        bac = True
                    End If
                    uxtype.Properties.Items.Add(_universes.luniverses(uxuniverse.SelectedIndex).calc_types(i))
                End If
            Next
            If bsty = True And bac = False Then
                uxtype.Properties.Items.Add("aggregate calenderized")
            End If
            If uxtype.Properties.Items.Count > 0 Then
                uxtype.SelectedIndex = 0
            End If
            If uxtype.Properties.Items.Count = 1 Then
                uxtype.Enabled = False
            Else
                uxtype.Enabled = True
            End If
            Dim s As String

            For i = 0 To _universes.luniverses(uxuniverse.SelectedIndex).target_classs.Count - 1
                s = _universes.luniverses(uxuniverse.SelectedIndex).target_classs(i).class_name
                If _universes.luniverses(uxuniverse.SelectedIndex).target_classs(i).dual_class_id > 0 Then
                    s = s + " & " + _universes.luniverses(uxuniverse.SelectedIndex).target_classs(i).dual_class_name
                End If
                uxtargetclass.Properties.Items.Add(s)

                If uxtargetclass.Properties.Items.Count = 1 Then
                    uxtargetclass.SelectedIndex = 0

                Else
                    uxtargetclass.Enabled = True
                End If
            Next

            For i = 0 To _universes.luniverses(uxuniverse.SelectedIndex).contributors.Count - 1
                uxcont.Properties.Items.Add(_universes.luniverses(uxuniverse.SelectedIndex).contributors(i).contributor_name)
                If uxcont.Properties.Items.Count = 1 Then
                    uxcont.SelectedIndex = 0
                    uxcont.Enabled = False
                Else
                    uxcont.SelectedIndex = 0
                    uxcont.Enabled = True
                End If
            Next
        End If


    End Sub



    Private Sub check_can_run()
        uxconts1.Nodes.Clear()
        uxres3.Nodes.Clear()
        Me.XtraTabPage2.PageVisible = False
        XtraTabPage1.Text = "Results"
        XtraTabPage2.Text = "Constitutents"

     
        bok.Enabled = False

        If uxtargetclass.Text = "Aggregation Universe" Then
            If (uxexch.SelectedIndex <> -1 And uxtype.SelectedIndex <> -1) Then
                bok.Enabled = True

            End If

        Else

            If (uxuniverse.SelectedIndex <> -1 And uxtargetclass.SelectedIndex <> -1 And uxentity.SelectedIndex <> -1 And uxexch.SelectedIndex <> -1 And uxtype.SelectedIndex <> -1) Then
                If (uxdual.Enabled = True) Then
                    If (uxdual.SelectedIndex <> -1) Then
                        bok.Enabled = True
                    End If
                Else
                    bok.Enabled = True
                End If
            End If
        End If
    End Sub


    Private Sub uxres_FocusedNodeChanged(sender As Object, e As DevExpress.XtraTreeList.FocusedNodeChangedEventArgs)
        Try

            uxconts1.Visible = False
            ldesc.Text = ""
            Dim year As Integer
            Dim metric_id As Integer
            Dim value As String
            Try
                year = CInt(uxres3.FocusedNode.GetValue(1))
            Catch
                Exit Sub
            End Try

            metric_id = CLng(uxres3.FocusedNode.Tag)

            value = uxres3.FocusedNode.GetValue(2)

            ldesc.Text = uxres3.FocusedNode.GetValue(0) + ": " + CStr(year) + ": " + uxcont.Text + ": " + value

            For i = 0 To _universes.luniverses(uxuniverse.SelectedIndex).metrics.Count - 1
                If (_universes.luniverses(uxuniverse.SelectedIndex).metrics(i).metric_id = metric_id) Then
                    lformula.Text = _universes.luniverses(uxuniverse.SelectedIndex).metrics(i).formula.Trim()
                End If
            Next

            uxconts1.Visible = True
            uxconts1.BeginUnboundLoad()
            uxconts1.Nodes.Clear()
            uxconts1.Columns(1).Visible = False
            uxconts1.Columns(2).Visible = False
            uxconts1.Columns(3).Visible = False
            uxconts1.Columns(4).Visible = False
            uxconts1.Columns(5).Visible = False
            uxconts1.Columns(6).Visible = False
            uxconts1.Columns(7).Visible = False
            uxconts1.Columns(8).Visible = False
            uxconts1.Columns(9).Visible = False
            uxconts1.Columns(10).Visible = False
            uxconts1.Columns(11).Visible = False
            uxconts1.Columns(12).Visible = False
            uxconts1.Columns(13).Visible = False
            uxconts1.Columns(14).Visible = False
            uxconts1.Columns(15).Visible = False
            uxconts1.Columns(16).Visible = False



            For i = 0 To _universes.luniverses(uxuniverse.SelectedIndex).metrics.Count - 1
                If (_universes.luniverses(uxuniverse.SelectedIndex).metrics(i).metric_id = metric_id) Then
                    If (_universes.luniverses(uxuniverse.SelectedIndex).metrics(i).num_years = 0 And _universes.luniverses(uxuniverse.SelectedIndex).metrics(i).contributor2 = 0) Then
                        load_conts(_abc_calc_agg, metric_id, year)
                    ElseIf (_universes.luniverses(uxuniverse.SelectedIndex).metrics(i).num_years > 0) Then
                        load_conts_growth(_abc_calc_agg_growths, metric_id, year)
                    Else
                        load_conts_cc(_abc_calc_agg_cc, metric_id, year)
                    End If
                End If
            Next
            uxconts1.EndUnboundLoad()


        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_dx_agg_preview", "uxresults_FocusedNodeChanged", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub

    Private Sub load_conts(stage As List(Of ServiceReference1.abc_calc_agg), metric_id As Long, year As Integer)
        Try
            Dim k As Integer
            Dim nc As Integer
            For k = 0 To stage.Count - 1
                If (stage(k).result_row_id = metric_id And stage(k).year = year And stage(k).contributor_id = _universes.luniverses(uxuniverse.SelectedIndex).contributors(uxcont.SelectedIndex).contributor_id) Then

                    'For l = 0 To _universes.entities.Count - 1

                    '    If (_universes.entities(l).entity_id = stage(k).entity_id) Then

                    '        uxconts1.Nodes.Add(_universes.entities(l).name + " [" + CStr(stage(k).entity_id) + "]")
                    '        uxconts1.Nodes(nc).SetValue(0, _universes.entities(l).name + " [" + CStr(stage(k).entity_id) + "]")
                    '        Exit For
                    '    End If
                    'Next
                    uxconts1.Nodes.Add(stage(k).entity_id)
                    uxconts1.Nodes(nc).SetValue(0, stage(k).entity_id)
                    If stage(k).value_1 IsNot Nothing Then
                        uxconts1.Nodes(nc).SetValue(1, stage(k).value_1)
                        uxconts1.Columns(1).Visible = True
                        uxconts1.Columns(1).VisibleIndex = 1
                    End If
                    If stage(k).value_2 IsNot Nothing Then
                        uxconts1.Nodes(nc).SetValue(3, stage(k).value_2)
                        uxconts1.Columns(3).Visible = True
                        uxconts1.Columns(3).VisibleIndex = 3
                    End If
                    If stage(k).value_3 IsNot Nothing Then
                        uxconts1.Nodes(nc).SetValue(5, stage(k).value_3)
                        uxconts1.Columns(5).Visible = True
                        uxconts1.Columns(5).VisibleIndex = 5
                    End If
                    If stage(k).value_4 IsNot Nothing Then
                        uxconts1.Nodes(nc).SetValue(7, stage(k).value_4)
                        uxconts1.Columns(7).Visible = True
                        uxconts1.Columns(7).VisibleIndex = 7
                    End If
                    If stage(k).value_5 IsNot Nothing Then
                        uxconts1.Nodes(nc).SetValue(9, stage(k).value_5)
                        uxconts1.Columns(9).Visible = True
                        uxconts1.Columns(9).VisibleIndex = 9
                    End If
                    If stage(k).value_6 IsNot Nothing Then
                        uxconts1.Nodes(nc).SetValue(11, stage(k).value_6)
                        uxconts1.Columns(11).Visible = True
                        uxconts1.Columns(11).VisibleIndex = 11
                    End If
                    If stage(k).value_7 IsNot Nothing Then
                        uxconts1.Nodes(nc).SetValue(13, stage(k).value_7)
                        uxconts1.Columns(13).Visible = True
                        uxconts1.Columns(13).VisibleIndex = 13
                    End If
                    If stage(k).value_8 IsNot Nothing Then
                        uxconts1.Nodes(nc).SetValue(15, stage(k).value_8)
                        uxconts1.Columns(15).Visible = True
                        uxconts1.Columns(15).VisibleIndex = 15
                    End If

                    nc = nc + 1
                End If
            Next

            XtraTabPage2.Text = "Constituents (" + CStr(nc) + ")"
            uxconts1.Visible = True
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_dx_agg_preview", "load_conts", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Private Sub load_conts_growth(stage As List(Of ServiceReference1.abc_calc_agg), metric_id As Long, year As Integer)
        Try
            Dim k As Integer
            Dim nc As Integer
            Dim lyear As Integer

            For k = 0 To stage.Count - 1
                If (stage(k).include_in_growthr = True And stage(k).result_row_id = metric_id And stage(k).year = year And stage(k).contributor_id = _universes.luniverses(uxuniverse.SelectedIndex).contributors(uxcont.SelectedIndex).contributor_id) Then
                    lyear = year - stage(k).num_years
                    'For l = 0 To _universes.entities.Count - 1

                    '    If (_universes.entities(l).entity_id = stage(k).entity_id) Then

                    '        uxconts1.Nodes.Add(_universes.entities(l).name + " [" + CStr(stage(k).entity_id) + "]")
                    '        uxconts1.Nodes(nc).SetValue(0, _universes.entities(l).name + " [" + CStr(stage(k).entity_id) + "]")
                    '        Exit For
                    '    End If
                    'Next
                    uxconts1.Nodes.Add(stage(k).entity_id)
                    uxconts1.Nodes(nc).SetValue(0, stage(k).entity_id)
                    If stage(k).value_1 IsNot Nothing Then
                        uxconts1.Nodes(nc).SetValue(1, stage(k).value_1)
                        uxconts1.Columns(1).Visible = True
                        uxconts1.Columns(1).VisibleIndex = 1
                    End If
                    If stage(k).value_2 IsNot Nothing Then
                        uxconts1.Nodes(nc).SetValue(3, stage(k).value_2)
                        uxconts1.Columns(3).Visible = True
                        uxconts1.Columns(3).VisibleIndex = 3
                    End If
                    If stage(k).value_3 IsNot Nothing Then
                        uxconts1.Nodes(nc).SetValue(5, stage(k).value_3)
                        uxconts1.Columns(5).Visible = True
                        uxconts1.Columns(5).VisibleIndex = 5
                    End If
                    If stage(k).value_4 IsNot Nothing Then
                        uxconts1.Nodes(nc).SetValue(7, stage(k).value_4)
                        uxconts1.Columns(7).Visible = True
                        uxconts1.Columns(7).VisibleIndex = 7
                    End If
                    If stage(k).value_5 IsNot Nothing Then
                        uxconts1.Nodes(nc).SetValue(9, stage(k).value_5)
                        uxconts1.Columns(9).Visible = True
                        uxconts1.Columns(9).VisibleIndex = 9
                    End If
                    If stage(k).value_6 IsNot Nothing Then
                        uxconts1.Nodes(nc).SetValue(11, stage(k).value_6)
                        uxconts1.Columns(11).Visible = True
                        uxconts1.Columns(11).VisibleIndex = 11
                    End If
                    If stage(k).value_7 IsNot Nothing Then
                        uxconts1.Nodes(nc).SetValue(13, stage(k).value_7)
                        uxconts1.Columns(13).Visible = True
                        uxconts1.Columns(13).VisibleIndex = 13
                    End If
                    If stage(k).value_8 IsNot Nothing Then
                        uxconts1.Nodes(nc).SetValue(15, stage(k).value_8)
                        uxconts1.Columns(15).Visible = True
                        uxconts1.Columns(15).VisibleIndex = 15
                    End If
                    nc = nc + 1
                End If
            Next
            nc = 0
            For k = 0 To stage.Count - 1
                If (stage(k).include_in_growthl = True And stage(k).result_row_id = metric_id And stage(k).year = lyear And stage(k).contributor_id = _universes.luniverses(uxuniverse.SelectedIndex).contributors(uxcont.SelectedIndex).contributor_id) Then
                    If stage(k).value_1 IsNot Nothing Then
                        uxconts1.Nodes(nc).SetValue(2, stage(k).value_1)
                        uxconts1.Columns(2).Visible = True
                        uxconts1.Columns(2).VisibleIndex = 2
                        uxconts1.Columns(2).Caption = "value_1_" + CStr(lyear)
                    End If
                    If stage(k).value_2 IsNot Nothing Then
                        uxconts1.Nodes(nc).SetValue(4, stage(k).value_2)
                        uxconts1.Columns(4).Visible = True
                        uxconts1.Columns(4).VisibleIndex = 4
                        uxconts1.Columns(4).Caption = "value_2_" + CStr(lyear)
                    End If
                    If stage(k).value_3 IsNot Nothing Then
                        uxconts1.Nodes(nc).SetValue(6, stage(k).value_3)
                        uxconts1.Columns(6).Visible = True
                        uxconts1.Columns(6).VisibleIndex = 6
                        uxconts1.Columns(6).Caption = "value_3_" + CStr(lyear)
                    End If
                    If stage(k).value_4 IsNot Nothing Then
                        uxconts1.Nodes(nc).SetValue(8, stage(k).value_4)
                        uxconts1.Columns(8).Visible = True
                        uxconts1.Columns(8).VisibleIndex = 8
                        uxconts1.Columns(8).Caption = "value_4_" + CStr(lyear)
                    End If
                    If stage(k).value_5 IsNot Nothing Then
                        uxconts1.Nodes(nc).SetValue(10, stage(k).value_5)
                        uxconts1.Columns(10).Visible = True
                        uxconts1.Columns(10).VisibleIndex = 10
                        uxconts1.Columns(10).Caption = "value_5_" + CStr(lyear)
                    End If
                    If stage(k).value_6 IsNot Nothing Then
                        uxconts1.Nodes(nc).SetValue(12, stage(k).value_6)
                        uxconts1.Columns(12).Visible = True
                        uxconts1.Columns(12).VisibleIndex = 12
                        uxconts1.Columns(12).Caption = "value_6_" + CStr(lyear)
                    End If
                    If stage(k).value_7 IsNot Nothing Then
                        uxconts1.Nodes(nc).SetValue(14, stage(k).value_7)
                        uxconts1.Columns(14).Visible = True
                        uxconts1.Columns(14).VisibleIndex = 14
                        uxconts1.Columns(14).Caption = "value_7_" + CStr(lyear)
                    End If
                    If stage(k).value_8 IsNot Nothing Then
                        uxconts1.Nodes(nc).SetValue(16, stage(k).value_8)
                        uxconts1.Columns(16).Visible = True
                        uxconts1.Columns(16).VisibleIndex = 16
                        uxconts1.Columns(16).Caption = "value_8_" + CStr(lyear)
                    End If
                    nc = nc + 1
                End If
            Next

            XtraTabPage2.Text = "Constituents (" + CStr(nc) + ")"
            uxconts1.Visible = True
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_dx_agg_preview", "load_conts_growth", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Private Sub load_conts_cc(stage As List(Of ServiceReference1.abc_calc_agg), metric_id As Long, year As Integer)
        Try
            Dim k As Integer
            Dim nc As Integer
            Dim lcont As Integer
            For k = 0 To stage.Count - 1
                If (stage(k).result_row_id = metric_id And stage(k).year = year And stage(k).contributor_id = _universes.luniverses(uxuniverse.SelectedIndex).contributors(uxcont.SelectedIndex).contributor_id) Then
                    lcont = stage(k).contributor_2_id
                    'For l = 0 To _universes.entities.Count - 1

                    '    If (_universes.entities(l).entity_id = stage(k).entity_id) Then

                    '        uxconts1.Nodes.Add(_universes.entities(l).name + " [" + CStr(stage(k).entity_id) + "]")
                    '        uxconts1.Nodes(nc).SetValue(0, _universes.entities(l).name + " [" + CStr(stage(k).entity_id) + "]")
                    '        Exit For
                    '    End If
                    'Next
                    uxconts1.Nodes.Add(stage(k).entity_id)
                    uxconts1.Nodes(nc).SetValue(0, stage(k).entity_id)
                    If stage(k).value_1 IsNot Nothing Then
                        uxconts1.Nodes(nc).SetValue(1, stage(k).value_1)
                        uxconts1.Columns(1).Visible = True
                        uxconts1.Columns(1).VisibleIndex = 1
                    End If
                    If stage(k).value_2 IsNot Nothing Then
                        uxconts1.Nodes(nc).SetValue(3, stage(k).value_2)
                        uxconts1.Columns(3).Visible = True
                        uxconts1.Columns(3).VisibleIndex = 3
                    End If
                    If stage(k).value_3 IsNot Nothing Then
                        uxconts1.Nodes(nc).SetValue(5, stage(k).value_3)
                        uxconts1.Columns(5).Visible = True
                        uxconts1.Columns(5).VisibleIndex = 5
                    End If
                    If stage(k).value_4 IsNot Nothing Then
                        uxconts1.Nodes(nc).SetValue(7, stage(k).value_4)
                        uxconts1.Columns(7).Visible = True
                        uxconts1.Columns(7).VisibleIndex = 7
                    End If
                    If stage(k).value_5 IsNot Nothing Then
                        uxconts1.Nodes(nc).SetValue(9, stage(k).value_5)
                        uxconts1.Columns(9).Visible = True
                        uxconts1.Columns(9).VisibleIndex = 9
                    End If
                    If stage(k).value_6 IsNot Nothing Then
                        uxconts1.Nodes(nc).SetValue(11, stage(k).value_6)
                        uxconts1.Columns(11).Visible = True
                        uxconts1.Columns(11).VisibleIndex = 11
                    End If
                    If stage(k).value_7 IsNot Nothing Then
                        uxconts1.Nodes(nc).SetValue(13, stage(k).value_7)
                        uxconts1.Columns(13).Visible = True
                        uxconts1.Columns(13).VisibleIndex = 13
                    End If
                    If stage(k).value_8 IsNot Nothing Then
                        uxconts1.Nodes(nc).SetValue(15, stage(k).value_8)
                        uxconts1.Columns(15).Visible = True
                        uxconts1.Columns(15).VisibleIndex = 15
                    End If
                    nc = nc + 1
                End If
            Next
            nc = 0
            For k = 0 To stage.Count - 1
                If (stage(k).contributor_id = lcont And stage(k).result_row_id = metric_id And stage(k).year = year) Then

                    If stage(k).value_1 IsNot Nothing Then
                        uxconts1.Nodes(nc).SetValue(2, stage(k).value_1)
                        uxconts1.Columns(2).Visible = True
                        uxconts1.Columns(2).VisibleIndex = 2
                        uxconts1.Columns(2).Caption = "value_1_" + CStr(lcont)
                    End If
                    If stage(k).value_2 IsNot Nothing Then
                        uxconts1.Nodes(nc).SetValue(4, stage(k).value_2)
                        uxconts1.Columns(4).Visible = True
                        uxconts1.Columns(4).VisibleIndex = 4
                        uxconts1.Columns(4).Caption = "value_2_" + CStr(lcont)
                    End If
                    If stage(k).value_3 IsNot Nothing Then
                        uxconts1.Nodes(nc).SetValue(6, stage(k).value_3)
                        uxconts1.Columns(6).Visible = True
                        uxconts1.Columns(6).VisibleIndex = 6
                        uxconts1.Columns(6).Caption = "value_3_" + CStr(lcont)
                    End If
                    If stage(k).value_4 IsNot Nothing Then
                        uxconts1.Nodes(nc).SetValue(8, stage(k).value_4)
                        uxconts1.Columns(8).Visible = True
                        uxconts1.Columns(8).VisibleIndex = 8
                        uxconts1.Columns(8).Caption = "value_4_" + CStr(lcont)
                    End If
                    If stage(k).value_5 IsNot Nothing Then
                        uxconts1.Nodes(nc).SetValue(10, stage(k).value_5)
                        uxconts1.Columns(10).Visible = True
                        uxconts1.Columns(10).VisibleIndex = 10
                        uxconts1.Columns(10).Caption = "value_5_" + CStr(lcont)
                    End If
                    If stage(k).value_6 IsNot Nothing Then
                        uxconts1.Nodes(nc).SetValue(12, stage(k).value_6)
                        uxconts1.Columns(12).Visible = True
                        uxconts1.Columns(12).VisibleIndex = 12
                        uxconts1.Columns(12).Caption = "value_6_" + CStr(lcont)
                    End If
                    If stage(k).value_7 IsNot Nothing Then
                        uxconts1.Nodes(nc).SetValue(14, stage(k).value_7)
                        uxconts1.Columns(14).Visible = True
                        uxconts1.Columns(14).VisibleIndex = 14
                        uxconts1.Columns(14).Caption = "value_7_" + CStr(lcont)
                    End If
                    If stage(k).value_8 IsNot Nothing Then
                        uxconts1.Nodes(nc).SetValue(16, stage(k).value_8)
                        uxconts1.Columns(16).Visible = True
                        uxconts1.Columns(16).VisibleIndex = 16
                        uxconts1.Columns(16).Caption = "value_8_" + CStr(lcont)
                    End If
                    nc = nc + 1
                End If
            Next

            XtraTabPage2.Text = "Constituents (" + CStr(nc) + ")"
            uxconts1.Visible = True
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_dx_agg_preview", "load_conts_cc", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub

    Private Sub uxtargetclass_SelectedIndexChanged(sender As Object, e As EventArgs) Handles uxtargetclass.SelectedIndexChanged
        Try
            Cursor = Cursors.WaitCursor

            uxentity.Properties.Items.Clear()
            uxdual.Properties.Items.Clear()
            uxentity.Text = ""
            uxdual.Text = ""
            uxentity.Enabled = False
            uxdual.Enabled = False
            uxentity.Properties.BeginUpdate()
            uxdual.Properties.BeginUpdate()


            If uxtargetclass.SelectedIndex <> -1 Then
                check_can_run()
                uxentity.Enabled = True
                Dim i As Integer
                uxentity.Properties.Items.Clear()
                uxdual.Properties.Items.Clear()
                For i = 0 To _universes.entities.Count - 1
                    If (_universes.entities(i).class_id = _universes.luniverses(uxuniverse.SelectedIndex).target_classs(uxtargetclass.SelectedIndex).class_id) Then

                        uxentity.Properties.Items.Add(_universes.entities(i).name)
                        If (uxtargetclass.Text = "Aggregation Universe") Then
                            uxentity.Text = uxuniverse.Text
                            uxentity.Enabled = False
                            uxtargetclass.Enabled = False
                            check_can_run()
                        End If

                    End If
                    If (_universes.entities(i).class_id = _universes.luniverses(uxuniverse.SelectedIndex).target_classs(uxtargetclass.SelectedIndex).dual_class_id) Then
                        uxdual.Properties.Items.Add(_universes.entities(i).name)
                        uxdual.Enabled = True
                    End If
                Next

                If uxentity.Properties.Items.Count = 1 Then
                    uxentity.SelectedIndex = 0
                    uxentity.Enabled = False
                    check_can_run()
                End If

            End If
        Catch

        Finally
            uxentity.Properties.EndUpdate()
            uxdual.Properties.EndUpdate()
            Cursor = Cursors.Default

        End Try
    End Sub

    Private Sub uxdual_SelectedIndexChanged(sender As Object, e As EventArgs) Handles uxdual.SelectedIndexChanged
        check_can_run()
    End Sub

    Private Sub uxentity_SelectedIndexChanged(sender As Object, e As EventArgs) Handles uxentity.SelectedIndexChanged
        check_can_run()
    End Sub

    Private Sub uxtype_SelectedIndexChanged(sender As Object, e As EventArgs) Handles uxtype.SelectedIndexChanged
        check_can_run()
    End Sub

    Private Sub uxexch_SelectedIndexChanged(sender As Object, e As EventArgs) Handles uxexch.SelectedIndexChanged
        check_can_run()
    End Sub

    Private Sub bok_Click(sender As Object, e As EventArgs) Handles bok.Click
        Try
            Me.SimpleButton1.Enabled = False
            Me.SimpleButton2.Enabled = False

            uxres3.Nodes.Clear()
            uxconts1.Nodes.Clear()
            bok.Enabled = False
            Me.XtraTabPage2.PageVisible = False
            Me.XtraTabPage1.Text = "Results"
            Me.XtraTabPage2.Text = "Constituents"

            Cursor = Cursors.WaitCursor

            XtraTabControl1.Enabled = False
            Dim entity_id As Long = 0
            Dim dual_entity_id As Long = 0
            Dim i As Integer
            For i = 0 To _universes.entities.Count - 1
                If (_universes.entities(i).name = uxentity.SelectedItem And _universes.entities(i).class_id = _universes.luniverses(uxuniverse.SelectedIndex).target_classs(uxtargetclass.SelectedIndex).class_id) Then
                    entity_id = _universes.entities(i).entity_id
                    If uxdual.SelectedIndex = -1 Then
                        Exit For
                    End If
                End If
                If (uxdual.SelectedIndex > -1) Then
                    If (_universes.entities(i).name = uxdual.SelectedItem And _universes.entities(i).class_id = _universes.luniverses(uxuniverse.SelectedIndex).target_classs(uxtargetclass.SelectedIndex).dual_class_id) Then
                        dual_entity_id = _universes.entities(i).entity_id
                        If entity_id <> 0 Then
                            Exit For
                        End If
                    End If
                End If
            Next
            Dim exch_type As Integer = 0
            If (uxexch.Text = "Period") Then
                exch_type = 1
            ElseIf uxexch.Text = "Period Average" Then
                exch_type = 2
            End If
            Dim ic As Boolean = False
            If uxic.CheckState = CheckState.Checked Then
                ic = True
            End If

            _controller.run(_universes.luniverses(uxuniverse.SelectedIndex).universe_id, entity_id, dual_entity_id, exch_type, uxtype.Text, ic)
            XtraTabControl1.Enabled = True
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_dx_agg_preview", "Click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Cursor = Cursors.Default
        End Try
    End Sub


    
    Private Sub uxcont_SelectedIndexChanged(sender As Object, e As EventArgs) Handles uxcont.SelectedIndexChanged
        set_results()
    End Sub

    Private Sub uxstage_SelectedIndexChanged(sender As Object, e As EventArgs) Handles uxstage.SelectedIndexChanged
        set_results()
    End Sub

    Private Sub uxres3_Click(sender As Object, e As EventArgs) Handles uxres3.Click
        Try

            If uxic.CheckState <> CheckState.Checked Then
                XtraTabPage2.PageVisible = False
                Exit Sub
            End If
            XtraTabPage2.PageVisible = True
            Cursor = Cursors.WaitCursor

            uxconts1.Visible = False
            ldesc.Text = ""
            lformula.Text = ""
            Dim year As Integer
            Dim metric_id As Long
            Dim value As String
            Try
                year = CInt(uxres3.FocusedNode.GetValue(1))
            Catch
                Exit Sub
            End Try
            metric_id = CLng(uxres3.FocusedNode.Tag)
            value = uxres3.FocusedNode.GetValue(2)
            ldesc.Text = uxres3.FocusedNode.GetValue(0) + ": " + CStr(year) + ": " + uxcont.Text + ": " + value

            For i = 0 To _universes.luniverses(uxuniverse.SelectedIndex).metrics.Count - 1
                If (_universes.luniverses(uxuniverse.SelectedIndex).metrics(i).metric_id = metric_id) Then
                    lformula.Text = _universes.luniverses(uxuniverse.SelectedIndex).metrics(i).formula
                    Exit For
                End If
            Next

            uxconts1.Visible = True
            uxconts1.BeginUnboundLoad()
            uxconts1.Nodes.Clear()
            uxconts1.Columns(1).Visible = False
            uxconts1.Columns(2).Visible = False
            uxconts1.Columns(3).Visible = False
            uxconts1.Columns(4).Visible = False
            uxconts1.Columns(5).Visible = False
            uxconts1.Columns(6).Visible = False
            uxconts1.Columns(7).Visible = False
            uxconts1.Columns(8).Visible = False
            uxconts1.Columns(9).Visible = False
            uxconts1.Columns(10).Visible = False
            uxconts1.Columns(11).Visible = False
            uxconts1.Columns(12).Visible = False
            uxconts1.Columns(13).Visible = False
            uxconts1.Columns(14).Visible = False
            uxconts1.Columns(15).Visible = False
            uxconts1.Columns(16).Visible = False



            For i = 0 To _universes.luniverses(uxuniverse.SelectedIndex).metrics.Count - 1
                If (_universes.luniverses(uxuniverse.SelectedIndex).metrics(i).metric_id = metric_id) Then
                    If (_universes.luniverses(uxuniverse.SelectedIndex).metrics(i).num_years = 0 And _universes.luniverses(uxuniverse.SelectedIndex).metrics(i).contributor2 = 0) Then
                        load_conts(_abc_calc_agg, metric_id, year)
                    ElseIf (_universes.luniverses(uxuniverse.SelectedIndex).metrics(i).num_years > 0) Then
                        load_conts_growth(_abc_calc_agg_growths, metric_id, year)
                    Else
                        load_conts_cc(_abc_calc_agg_cc, metric_id, year)
                    End If
                End If
            Next
            uxconts1.EndUnboundLoad()
            uxconts1.Visible = True
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_dx_agg_preview", "uxres3_Click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Cursor = Cursors.Default
        End Try
    End Sub


    Private Sub uxic_CheckedChanged(sender As Object, e As EventArgs) Handles uxic.CheckedChanged

        Me.XtraTabPage2.PageVisible = False

        check_can_run()

    End Sub

    Private Sub uxres3_FocusedNodeChanged(sender As Object, e As DevExpress.XtraTreeList.FocusedNodeChangedEventArgs) Handles uxres3.FocusedNodeChanged

    End Sub

    Private Sub uxstyle_SelectedIndexChanged(sender As Object, e As EventArgs) Handles uxstyle.SelectedIndexChanged

        If uxstyle.SelectedIndex = 1 Then
            set_results(True)
        Else
            set_results()
        End If
    End Sub

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        export_to_excel(0)
    End Sub
    

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        export_to_excel(1)
    End Sub
    Sub export_to_excel(mode)
        Dim oxlApp As Object 'Excel.Application4.   
        Cursor = Cursors.WaitCursor


        Try

            If Me.uxres3.Nodes.Count = 0 And mode = 0 Then
                Exit Sub
            End If
            If Me.uxconts1.Nodes.Count = 0 And mode = 1 Then
                Exit Sub
            End If


            Dim oxlOutWBook As Object 'Excel.Workbook5.     

            oxlApp = bc_ao_in_excel.CreateNewExcelInstance
            oxlOutWBook = oxlApp.Workbooks.Add
            Dim oexcel As New bc_ao_in_excel(oxlApp)


            Dim c As Integer
            If mode = 0 Then

                Dim values(uxres3.Nodes.Count, uxres3.Columns.Count - 1) As String

                For i = 0 To uxres3.Nodes.Count - 1
                    c = 1
                    For j = 0 To uxres3.Columns.Count - 1
                        If i = 0 Then

                        End If

                        If uxres3.Columns(j).Visible = True Then
                            If i = 0 Then
                                values(i, c) = uxres3.Columns(j).Caption

                            End If
                            values(i + 1, c) = uxres3.Nodes(i).GetValue(j)
                            c = c + 1
                        End If
                    Next

                Next

                oexcel.bc_array_excel_export(values, True, "Results")
            Else

                Dim values(uxconts1.Nodes.Count, uxconts1.Columns.Count - 1) As String

                For i = 0 To uxconts1.Nodes.Count - 1
                    c = 1
                    For j = 0 To uxconts1.Columns.Count - 1
                        If uxconts1.Columns(j).Visible = True Then
                            If i = 0 Then
                                values(i, c) = uxconts1.Columns(j).Caption
                            End If
                            values(i + 1, c) = uxconts1.Nodes(i).GetValue(j)
                            c = c + 1
                        End If
                    Next
                Next
                oexcel.bc_array_excel_export(values, True, "Constituents")
            End If


            oxlApp.application.visible = True

        Catch ex As Exception
            'Dim oerr As New bc_cs_error_log("bc_dx_agg_preview", "export_to_excel", bc_cs_error_codes.USER_DEFINED, "Failed to export to excel: " + ex.Message)
        Finally
            oxlApp.application.visible = True
            Cursor = Cursors.Default

        End Try
    End Sub
End Class

Public Class Cbc_am_aggs_preview
    WithEvents _view As Ibc_am_aggs_preview
    Dim universes As bc_om_universes_preview


    Public Function load_data(view As Ibc_am_aggs_preview, Optional universe_id As Long = 0) As String
        Try
            load_data = False
            Dim bcs = New bc_cs_central_settings(True)
            _view = view

            universes = New bc_om_universes_preview
            universes.universe_id = universe_id


            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                universes.db_read()
            Else
                If (universes.transmit_to_server_and_receive(universes, True) = False) Then
                    Exit Function
                End If
            End If

            If _view.load_view(universes, Me) = True Then
                load_data = True
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Cbc_am_aggs_preview", "load_data", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try

    End Function

    Public Function run(universe_id As Long, target_entity_id As Long, dual_entity_id As Long, exch_rate_method As Integer, type As String, ic As Boolean) As Boolean



        Dim bsql = New bc_cs_sql()


        Dim res As Object

        Dim audit_id As Integer
        Dim audit_date As DateTime

        If (bsql.exec_sql("exec dbo.bc_core_aggs_services_preview_get_audit_details", bc_cs_sql.SQL_TYPE.NO_TIMEOUT, res) = False) Then
            Return False
        End If

        If IsArray(res) Then
            audit_id = res(0, 0)
            audit_date = res(1, 0)

            Dim binding = New BasicHttpBinding()
            binding.MaxReceivedMessageSize = 2147483647
            binding.MaxBufferSize = 2147483647
            binding.MaxBufferPoolSize = 2147483647
            binding.ReaderQuotas.MaxDepth = 32
            binding.ReaderQuotas.MaxStringContentLength = 2147483647
            binding.ReaderQuotas.MaxArrayLength = 2147483647
            binding.ReaderQuotas.MaxBytesPerRead = 2147483647
            binding.ReaderQuotas.MaxNameTableCharCount = 16384


            Dim ea As New EndpointAddress(bc_cs_central_settings.aggregation_system_url)

            Dim s As New ServiceReference1.BCIISServicesClient(binding, ea)
            s.Endpoint.Binding.SendTimeout = TimeSpan.FromMilliseconds(9999999)
            s.Endpoint.Binding.ReceiveTimeout = TimeSpan.FromMilliseconds(9999999)


            Dim results As New ServiceReference1.agg_results
            Dim ec As Boolean = False
            Try
                Try
                    results = s.AggregateUniverseDebug(0, universe_id, audit_id, audit_date, target_entity_id, dual_entity_id, exch_rate_method, type, ic)
                Catch
                    ec = True
                    results = s.AggregateUniverseDebug(0, universe_id, audit_id, audit_date, target_entity_id, dual_entity_id, exch_rate_method, type, False)
                End Try

                If (results.error <> "") Then
                    Dim msg As New bc_cs_message("Blue Curve", "Error Aggregating Universe: " + results.error, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                ElseIf IsNothing(results.results) And IsNothing(results.ttest) Then
                    Dim msg As New bc_cs_message("Blue Curve", "No Results for Target", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Else
                    If ec = True Then
                        Dim msg As New bc_cs_message("Blue Curve", "The constituent data set is to large to preview results only displayed.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    End If
                    _view.load_results(results, ec)
                    Return True
                End If
            Catch ex As Exception
                Dim msg As New bc_cs_message("Blue Curve", "Error Aggregating Universe: " + ex.Message, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            End Try
        End If
    End Function
End Class

Public Interface Ibc_am_aggs_preview
    Function load_view(universes As bc_om_universes_preview, controller As Cbc_am_aggs_preview) As Boolean
    Sub load_results(results As ServiceReference1.agg_results, ec As Boolean)
End Interface


