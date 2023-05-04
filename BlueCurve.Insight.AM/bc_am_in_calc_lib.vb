Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports DevExpress.XtraTreeList
Public Class bc_am_in_calc_lib
    Implements Ibc_am_in_calc_lib

    Dim _calcs As bc_om_libary_calculations
    Dim _contributors As New List(Of bc_om_contributor)

    Public Sub load_view(calcs As bc_om_libary_calculations, contributors As List(Of bc_om_contributor)) Implements Ibc_am_in_calc_lib.load_view

        _calcs = calcs
        _contributors = contributors

        For i = -20 To 20
            If i <> 0 Then
                Me.cyb.Properties.Items.Add(CStr(i))
            End If
            Me.cyb.Text = "1"
        Next


        Me.Ct.Properties.Items.Add("Days")
        Me.Ct.Properties.Items.Add("Weeks")
        Me.Ct.Properties.Items.Add("Months")
        Me.Ct.Properties.Items.Add("Years")
        For i = 0 To _contributors.Count - 1
            Me.C1.Properties.Items.Add(_contributors(i).name)
            Me.C2.Properties.Items.Add(_contributors(i).name)
            Me.csc.Properties.Items.Add(_contributors(i).name)
        Next

        For i = 1 To 100
            Me.Ci.Properties.Items.Add(CStr(i))
        Next

        load_lib()

    End Sub
    Public Sub load_lib()
        Dim n As DevExpress.XtraTreeList.Nodes.TreeListNode
        Dim tx As String = ""
        Dim tln As Nodes.TreeListNode = Nothing
        uxlib.Nodes.Clear()
        uxlib.BeginUpdate()

        For i = 0 To _calcs.libray_calculations.Count - 1
            With _calcs.libray_calculations(i)
                n = uxlib.AppendNode(New Object() {.name}, tln)
                n.Tag = CStr(i)
                If .num_years <> 0 Then
                    n.SetValue(1, "Growth")
                    If .num_years = 1 Then
                        n.SetValue(2, "1 year back")
                    ElseIf .num_years = -1 Then
                        n.SetValue(2, "1 year forward")
                    ElseIf .num_years > 1 Then
                        n.SetValue(2, CStr(.num_years) + " years back CAGR")
                    Else
                        n.SetValue(2, CStr(0 - .num_years) + " years forward CAGR")
                    End If
                ElseIf .contributor1_id > 0 Then
                    n.SetValue(1, "Cross Contributor")
                    Dim cs As String = ""
                    For j = 0 To _contributors.Count - 1
                        If .contributor1_id = _contributors(j).id Then
                            cs = _contributors(j).name
                        End If
                    Next
                    cs = cs + " vs. "
                    For j = 0 To _contributors.Count - 1
                        If .contributor2_id = _contributors(j).id Then
                            cs = cs + _contributors(j).name
                        End If
                    Next
                    n.SetValue(2, cs)
                ElseIf .type = "static to period" Then
                    Dim cs As String = ""
                    n.SetValue(1, "Static to Period Cross Contributor")
                    For j = 0 To _contributors.Count - 1
                        If .contributor2_id = _contributors(j).id Then
                            cs = _contributors(j).name
                        End If
                    Next
                    n.SetValue(2, "Source from: " + cs)
                Else
                    n.SetValue(1, "Momentum")
                    Select Case .interval_type
                        Case 1
                            n.SetValue(2, CStr(.interval) + " day")
                        Case 2
                            n.SetValue(2, CStr(.interval) + " week")
                        Case 3
                            n.SetValue(2, CStr(.interval) + " month")
                        Case 4
                            n.SetValue(2, CStr(.interval) + " year")
                    End Select

                End If
            End With
        Next
        uxlib.EndUpdate()
        set_calc_data(0)
    End Sub
    Private Sub RadioGroup1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RadioGroup1.SelectedIndexChanged
        Me.lyb.Visible = False
        Me.cyb.Visible = False
        Me.L1.Visible = False
        Me.C1.Visible = False
        Me.c2.Visible = False
        Me.L2.Visible = False
        Me.csc.Visible = False
        Me.Ct.Visible = False
        Me.Lt.Visible = False
        Me.Ci.Visible = False
        Me.Li.Visible = False
        Me.lsc.Visible = False
        Me.csc.Visible = False



        Select Case RadioGroup1.EditValue
            Case 1
                Me.lyb.Visible = True
                Me.cyb.Visible = True

            Case 2

                Me.L1.Visible = True
                Me.C1.Visible = True
                Me.L2.Visible = True
                Me.c2.Visible = True
                Me.L2.Text = "Contributor 2"
            Case 3
                Me.Ct.Visible = True
                Me.Lt.Visible = True
                Me.Ci.Visible = True
                Me.Li.Visible = True
                Me.Ct.SelectedIndex = -1
                Me.Ci.SelectedIndex = -1
            Case 4
                Me.lsc.Visible = True
                Me.csc.Visible = True
        End Select
        check_add()
    End Sub

    Private Sub c1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cyb.SelectedIndexChanged

        If Me.lyb.Text = "Inteval Type" And Me.cyb.SelectedIndex > -1 Then
            Me.C1.Properties.Items.Clear()
            Me.C1.Enabled = True

            For i = 1 To 100
                Me.C1.Properties.Items.Add(CStr(i))
            Next

        End If
        check_add()
    End Sub

    Private Sub L2_Click(sender As Object, e As EventArgs) Handles L1.Click

    End Sub

    Private Sub bcancel_Click(sender As Object, e As EventArgs) Handles bcancel.Click
        Me.Hide()
    End Sub

    Public Event save(calcs As bc_om_libary_calculations) Implements Ibc_am_in_calc_lib.save

    Private Sub SimpleButton3_Click(sender As Object, e As EventArgs) Handles SimpleButton3.Click
        Me.Cursor = Windows.Forms.Cursors.WaitCursor


        RaiseEvent save(_calcs)
        Me.Cursor = Windows.Forms.Cursors.Default
        Me.Hide()

    End Sub

    Private Sub uxlib_FocusedNodeChanged(sender As Object, e As FocusedNodeChangedEventArgs) Handles uxlib.FocusedNodeChanged
        Try
            set_calc_data(CInt(e.Node.Tag))
        Catch ex As Exception



        End Try
    End Sub
    Sub check_add()
        Me.badd.Enabled = False

        If Trim(Me.Tname.Text) = "" Or Me.RadioGroup1.SelectedIndex = -1 Then
            Exit Sub
        End If
        Select Case Me.RadioGroup1.SelectedIndex
            Case 0
                If Me.cyb.SelectedIndex = -1 Then
                    Exit Sub
                End If
            Case 1
                If Me.C1.SelectedIndex = -1 Or Me.csc.SelectedIndex = -1 Then
                    Exit Sub
                End If
            Case 2
                If Me.Ct.SelectedIndex = -1 Or Me.Ci.SelectedIndex = -1 Then
                    Exit Sub
                End If
        End Select
        Me.badd.Enabled = True
    End Sub
    Sub set_calc_data(idx As Integer)
        Me.Tname.Text = ""
        Me.RadioGroup1.SelectedIndex = -1

        Me.bdel.Enabled = True
        Me.bupd.Enabled = True
        Me.badd.Enabled = False
        Try
            Me.Tname.Text = _calcs.libray_calculations(idx).name
            If _calcs.libray_calculations(idx).num_years <> 0 Then
                Me.RadioGroup1.SelectedIndex = 0
                Me.cyb.Text = CStr(_calcs.libray_calculations(idx).num_years)
            ElseIf _calcs.libray_calculations(idx).contributor1_id > 0 Then
                Me.RadioGroup1.SelectedIndex = 1
                For i = 0 To _contributors.Count - 1
                    If _contributors(i).id = _calcs.libray_calculations(idx).contributor1_id Then
                        Me.C1.SelectedIndex = i
                    End If
                    If _contributors(i).id = _calcs.libray_calculations(idx).contributor2_id Then
                        Me.c2.SelectedIndex = i
                    End If
                Next
            ElseIf _calcs.libray_calculations(idx).type = "static to period" Then
                Me.RadioGroup1.SelectedIndex = 3
                For i = 0 To _contributors.Count - 1
                    If _contributors(i).id = _calcs.libray_calculations(idx).contributor2_id Then
                        Me.csc.SelectedIndex = i
                    End If
                Next
            Else
                Me.RadioGroup1.SelectedIndex = 2
                Me.Ct.SelectedIndex = CStr(_calcs.libray_calculations(idx).interval_type) - 1
                Me.Ci.Text = CStr(_calcs.libray_calculations(idx).interval)
            End If
        Catch

        End Try
        If Me.RadioGroup1.SelectedIndex = -1 Then
            Me.bdel.Enabled = False
            Me.bupd.Enabled = False
        End If
        check_add()
    End Sub

    Private Sub bdel_Click(sender As Object, e As EventArgs) Handles bdel.Click

        For i = 0 To _calcs.libray_calculations.Count - 1

            If _calcs.libray_calculations(i).name = Me.uxlib.Selection(0).GetDisplayText(0) Then
                _calcs.libray_calculations.RemoveAt(i)

                Exit For
            End If
        Next
        Me.load_lib()

    End Sub

    Private Sub badd_Click(sender As Object, e As EventArgs) Handles badd.Click
        'add new
        set_calc(True)
    End Sub
    Private Sub set_calc(bnew As Boolean)
        Dim calc As New bc_om_calculation
        calc.name = Me.Tname.Text
        Select Case Me.RadioGroup1.SelectedIndex
            Case 0
                calc.num_years = Me.cyb.Text
            Case 1
                calc.contributor1_id = _contributors(Me.C1.SelectedIndex).id
                calc.contributor2_id = _contributors(Me.c2.SelectedIndex).id
                If calc.contributor1_id = calc.contributor2_id Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Contributors cant be the same", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub
                End If
            Case 2
                calc.interval_type = Me.Ct.SelectedIndex + 1
                calc.interval = Me.Ci.Text
            Case 3
                calc.contributor2_id = _contributors(Me.csc.SelectedIndex).id
                calc.type = "static to period"
        End Select

        If bnew = True Then
            For i = 0 To _calcs.libray_calculations.Count - 1
                If calc.name = _calcs.libray_calculations(i).name Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Calculation name: " + calc.name + " already exists", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub
                End If
                If calc.num_years > 0 And calc.num_years = _calcs.libray_calculations(i).num_years Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Calculation type for " + CStr(calc.num_years) + " years already exists", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub
                End If
                If calc.type <> "static to period" And calc.num_years = 0 And calc.contributor1_id > 0 And calc.contributor1_id = _calcs.libray_calculations(i).contributor1_id And calc.contributor2_id = _calcs.libray_calculations(i).contributor2_id Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Calculation type for  this cross contributor combination already exists", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub
                End If

                If calc.type <> "static to period" And calc.num_years = 0 And calc.contributor1_id = 0 And calc.interval_type = _calcs.libray_calculations(i).interval_type And calc.interval = _calcs.libray_calculations(i).interval Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Calculation type for this interval type and interval already exists.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub
                End If
                If calc.type = "static to period" And calc.contributor2_id = _calcs.libray_calculations(i).contributor2_id And _calcs.libray_calculations(i).type = "static to period" Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Calculation type for this cross contributor exists.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub
                End If

            Next
        Else
            For i = 0 To _calcs.libray_calculations.Count - 1
                If calc.name = _calcs.libray_calculations(i).name And calc.name <> Me.uxlib.Selection(0).GetDisplayText(0) Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Calculation name: " + calc.name + " already exists", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub
                End If
                If calc.num_years > 0 And calc.num_years = _calcs.libray_calculations(i).num_years And _calcs.libray_calculations(i).name <> Me.uxlib.Selection(0).GetDisplayText(0) Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Calculation type for " + CStr(calc.num_years) + " years already exists", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub
                End If
                If calc.type <> "static to period" And calc.num_years = 0 And calc.contributor1_id > 0 And _calcs.libray_calculations(i).name <> Me.uxlib.Selection(0).GetDisplayText(0) And calc.contributor1_id = _calcs.libray_calculations(i).contributor1_id And calc.contributor2_id = _calcs.libray_calculations(i).contributor2_id Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Calculation type for  this cross contributor combination already exists", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub
                End If
                If calc.type <> "static to period" And calc.num_years = 0 And calc.contributor1_id = 0 And _calcs.libray_calculations(i).name <> Me.uxlib.Selection(0).GetDisplayText(0) And calc.interval_type = _calcs.libray_calculations(i).interval_type And calc.interval = _calcs.libray_calculations(i).interval Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Calculation type for this interval type and interval already exists.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub
                End If
                If calc.type = "static to period" And calc.contributor2_id = _calcs.libray_calculations(i).contributor2_id And _calcs.libray_calculations(i).type = "static to period" And _calcs.libray_calculations(i).name <> Me.uxlib.Selection(0).GetDisplayText(0) Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Calculation type for this cross contributor exists.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub
                End If


            Next

        End If



        If bnew = True Then
            _calcs.libray_calculations.Add(calc)
        Else
            For i = 0 To _calcs.libray_calculations.Count - 1
                If _calcs.libray_calculations(i).name = Me.uxlib.Selection(0).GetDisplayText(0) Then
                    _calcs.libray_calculations(i) = calc
                    Exit For
                End If
            Next
        End If
        Me.load_lib()

    End Sub

    Private Sub bupd_Click(sender As Object, e As EventArgs) Handles bupd.Click
        set_calc(False)
    End Sub

    Private Sub Tname_EditValueChanged(sender As Object, e As EventArgs) Handles Tname.EditValueChanged
        check_add()
    End Sub

    Private Sub C1_SelectedIndexChanged_1(sender As Object, e As EventArgs) Handles C1.SelectedIndexChanged
        check_add()
    End Sub

    Private Sub C2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles csc.SelectedIndexChanged
        check_add()
    End Sub

    Private Sub Ct_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Ct.SelectedIndexChanged
        check_add()
    End Sub

    Private Sub Ci_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Ci.SelectedIndexChanged
        check_add()
    End Sub
End Class

Public Class Cbc_am_in_calc_lib
    WithEvents _view As Ibc_am_in_calc_lib
    Dim _contributors As List(Of bc_om_contributor)
    Public bsave As Boolean = False
    Public calc_lib As bc_om_libary_calculations
    Public Sub New(view As Ibc_am_in_calc_lib, contributors As List(Of bc_om_contributor))
        _contributors = contributors
        _view = view
    End Sub
    Public Function load_data() As Boolean

        load_data = False
      

        Dim calcs As New bc_om_libary_calculations
        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then

            calcs.db_read()
        Else

            calcs.tmode = bc_cs_soap_base_class.tREAD
            If calcs.transmit_to_server_and_receive(calcs, True) = False Then
                Exit Function
            End If

        End If

        _view.load_view(calcs, _contributors)
        load_data = True
    End Function
    Public Sub save(calcs As bc_om_libary_calculations) Handles _view.save
        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
            calcs.db_write()
        Else
            calcs.tmode = bc_cs_soap_base_class.tWRITE
            If calcs.transmit_to_server_and_receive(calcs, True) = False Then
                Exit Sub
            End If
        End If
        calc_lib = New bc_om_libary_calculations
        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
            calc_lib.db_read()
        Else
            calc_lib.tmode = bc_cs_soap_base_class.tREAD
            If calc_lib.transmit_to_server_and_receive(calc_lib, True) = False Then
                Exit Sub
            End If
        End If

        bsave = True
    End Sub


End Class
Public Interface Ibc_am_in_calc_lib
    Sub load_view(calcs As bc_om_libary_calculations, contributors As List(Of bc_om_contributor))
    Event save(calcs As bc_om_libary_calculations)

End Interface
