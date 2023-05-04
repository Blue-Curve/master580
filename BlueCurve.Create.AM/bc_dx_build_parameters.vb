Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports BlueCurve.Core.AS
Imports System.Threading
Imports System.Windows.Forms
Imports System.ComponentModel
Imports System.Runtime.InteropServices
Imports DevExpress.XtraBars

Imports DevExpress.XtraTreeList
Imports System.Drawing


Public Class view_bc_dx_build_parameters
    Implements Iview_bc_am_build_parameters
    Dim _adoc As bc_om_document
    Dim _params As New List(Of build_parameter)
    Dim _ctrll As ctrll_bc_am_build_parameters
    Dim _contributor As Long
    Dim _disabled As Integer
    Dim _last_refresh_date As DateTime
    Dim _locator As String
    Public Event save(ByVal params As List(Of build_parameter), ByVal contributor_Id As Long, ByVal disabled As Integer) Implements Iview_bc_am_build_parameters.save
    Public Event doc_refresh(ByVal params As List(Of build_parameter), ByVal contributor_Id As Long, ByVal disabled As Integer) Implements Iview_bc_am_build_parameters.Refresh
    Public Event cancel(ByVal locator As String) Implements Iview_bc_am_build_parameters.Cancel
    Dim has_context_items As Boolean = False
    Dim context_items_pos As Integer
    Dim context_items_count As Integer
    Public Sub New()

        DevExpress.Skins.SkinManager.EnableFormSkins()
        DevExpress.UserSkins.BonusSkins.Register()
        DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle("Blue")
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Public Sub disable_cancel() Implements Iview_bc_am_build_parameters.disable_cancel
        Me.bcancel.Enabled = False
    End Sub
    Public Sub disable_refresh_disabled() Implements Iview_bc_am_build_parameters.disable_refresh_disabled
        Me.chkdisable.Enabled = False
    End Sub
    Public Sub disable_refresh() Implements Iview_bc_am_build_parameters.disable_refresh
        Me.brefresh.Enabled = False
    End Sub
    Public Sub disable_contributer() Implements Iview_bc_am_build_parameters.disable_contributer
        Me.uxcont.Enabled = False
    End Sub
    Public Sub disable_save() Implements Iview_bc_am_build_parameters.disable_save
        Me.bok.Enabled = False
    End Sub
    Public Sub set_ammend_controls(ByVal _from_adv As Boolean) Implements Iview_bc_am_build_parameters.set_ammend_controls
        Me.bok.Text = "Save"
        Me.bcancel.Visible = True
        If _from_adv = False Then
            Me.brefresh.Visible = True
            Me.chkdisable.Visible = True
            Me.lcont.Visible = True
            Me.uxcont.Visible = True
            Me.lrefresh.Visible = True
        Else
            Dim dp As System.Drawing.Point
            dp.X = Me.brefresh.Location.X
            dp.Y = Me.brefresh.Location.Y
            Me.bcancel.Location = dp
        End If

        Dim idx As Integer

        For i = 0 To bc_am_load_objects.obc_pub_types.contributors.Count - 1

            uxcont.Properties.Items.Add(bc_am_load_objects.obc_pub_types.contributors(i).name)
            If _contributor = bc_am_load_objects.obc_pub_types.contributors(i).id Then
                idx = i
            End If
        Next
        Me.uxcont.SelectedIndex = idx
        If _disabled = 1 Then
            Me.chkdisable.Checked = True
        End If

        If _last_refresh_date = "9-9-9999" Then
            Me.lrefresh.Text = "Await Refresh"
        Else
            Me.lrefresh.Text = "Last Refresh: " + Format(_last_refresh_date.ToLocalTime, "dd-MMM-yyy HH:mm")
        End If
    End Sub
    Dim picker_vals As New List(Of kvp)
    Dim def1 As Integer
    Dim def2 As Integer
    Dim gap As Integer
    Class kvp
        Public id As Integer
        Public name As String
    End Class
    Sub load_picker_vals()
        Try
            Dim kvp As kvp
            With _params(0).parameter
                For i = 0 To .lookup_values.Count - 1
                    kvp = New kvp
                    kvp.id = .lookup_values(i)
                    kvp.name = .lookup_values_ids(i)
                    def1 = .default_value_id
                    If kvp.name = "gap" Then
                        gap = kvp.id
                    ElseIf kvp.id <> -99999 Then
                        picker_vals.Add(kvp)
                    End If
                Next
            End With
            With _params(1).parameter
                def2 = .default_value_id
            End With
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_component_refresh", "load_picker_vals", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Sub

        
    Public Sub load_slider()
        load_picker_vals()
        'Me.TrackBar1.Maximum = picker_vals.Count - 1
        Me.trange.Properties.Maximum = picker_vals.Count - 1

        Me.trange.Properties.TickFrequency = picker_vals.Count / 20
        Dim l As DevExpress.XtraEditors.Repository.TrackBarLabel
        'Me.trange.Properties.Labels.Clear()
        'For i = 0 To picker_vals.Count - 1
        '    l = New DevExpress.XtraEditors.Repository.TrackBarLabel(picker_vals(i).name, i, True)
        '    Me.trange.Properties.Labels.Add(l)
        'Next
        Dim r As DevExpress.XtraEditors.Repository.TrackBarRange
        
        If def1 = -99999 And def2 = -99999 Then
            Me.chkna.Checked = True
            Me.lrange.Text = "non applicable"
        Else
            For i = 0 To picker_vals.Count - 1
                If picker_vals(i).id = def1 Then
                    r.Minimum = i
                End If
                If picker_vals(i).id = def2 Then
                    r.Maximum = i
                End If
            Next
            Me.trange.Value = r
            set_range_text()
        End If


    End Sub
    Sub set_range_text()
        Try


            If picker_vals(Me.trange.Value.Minimum).id = picker_vals(Me.trange.Value.Maximum).id Then
                Me.lrange.Text = picker_vals(Me.trange.Value.Minimum).name
            Else
                Me.lrange.Text = picker_vals(Me.trange.Value.Minimum).name + " to " + picker_vals(Me.trange.Value.Maximum).name
            End If

            REM now set value
            _params(0).parameter.default_value_id = Me.picker_vals(Me.trange.Value.Minimum).id
            _params(0).parameter.default_value = Me.picker_vals(Me.trange.Value.Minimum).name
            _params(1).parameter.default_value_id = Me.picker_vals(Me.trange.Value.Maximum).id
            _params(1).parameter.default_value = Me.picker_vals(Me.trange.Value.Maximum).name

            pmax = Me.trange.Value.Maximum
            pmin = Me.trange.Value.Minimum

        Catch ex As Exception





        End Try
    End Sub
    Public Sub load_list(ByVal caption As String, ByVal params As List(Of build_parameter), ByVal ctrll As ctrll_bc_am_build_parameters, ByVal contributor As Long, ByVal disabled As Integer, ByVal last_refresh As DateTime, ByVal locator As String) Implements Iview_bc_am_build_parameters.load_list
        Try
            Me.Text = caption
            _params = params
            _ctrll = ctrll
            _contributor = contributor
            _last_refresh_date = last_refresh
            _disabled = disabled
            _locator = locator
            uxparams.BeginUnboundLoad()
            uxparams.Nodes.Clear()

            If _params.Count = 0 Then
                Me.uxparams.Visible = False
                Me.Height = 120
            ElseIf _params.Count = 2 AndAlso _params(0).parameter.datatype = 11 Then
                Me.pslider.Visible = True
                Me.uxparams.Visible = False
                Me.Height = 233
                load_slider()
            Else
                For j = 0 To _params.Count - 1
                    With _params(j).parameter
                        Dim tln As Nodes.TreeListNode = Nothing
                        If (.datatype = 9) Then
                            has_context_items = True
                            context_items_pos = j
                            context_items_count = .context_items.table_items.Count
                            For k = 0 To .context_items.table_items.Count - 1
                                uxparams.AppendNode(New Object() {CStr(.context_items.table_items(k).orig_item_name)}, tln).Tag = CStr(j + k)
                                If .context_items.table_items(k).system_defined = 0 Then
                                    uxparams.Nodes(j + k).SetValue(1, CStr(.context_items.table_items(k).new_item_name))
                                Else
                                    uxparams.Nodes(j + k).SetValue(1, "System Defined")
                                End If
                                uxparams.Nodes(j + k).StateImageIndex = 2
                            Next
                        Else
                            uxparams.AppendNode(New Object() {.name}, tln).Tag = CStr(j)

                            If .datatype = 15 Or .datatype = 16 Then
                                uxparams.Nodes(j).SetValue(1, "Multi Select...")
                                uxparams.Nodes(uxparams.Nodes.Count - 1).StateImageIndex = 0

                            ElseIf .system_defined = 1 Then
                                uxparams.Nodes(j).SetValue(1, "System Defined")
                            Else
                                If .default_value <> "0" Then
                                    uxparams.Nodes(j).SetValue(1, .default_value)
                                End If
                            End If

                            If .mandatory = True Then
                                uxparams.Nodes(uxparams.Nodes.Count - 1).StateImageIndex = 1
                            ElseIf .datatype <> 15 And .datatype <> 16 Then
                                uxparams.Nodes(uxparams.Nodes.Count - 1).StateImageIndex = 2
                            End If
                        End If
                    End With
                Next
                uxparams.EndUnboundLoad()
                If Me.uxparams.Nodes.Count > 30 Then
                    Me.Height = 600
                Else
                    Me.Height = (Me.uxparams.Nodes.Count * Me.uxparams.RowHeight) + 160
                End If
            End If
            Me.TopMost = True


        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("view_bc_am_build_parameters", "load_list", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bok.Click
        If check_ok() = False Then
            Exit Sub
        End If
        If Me.chkdisable.Visible = False Then
            RaiseEvent save(_params, Nothing, Nothing)
        Else
            If uxcont.SelectedIndex = -1 Then
                RaiseEvent save(_params, 1, Me.chkdisable.CheckState)
            Else
                RaiseEvent save(_params, bc_am_load_objects.obc_pub_types.contributors(uxcont.SelectedIndex).id, Me.chkdisable.CheckState)
            End If
        End If
        Me.Hide()
    End Sub
    Private Sub uxparams_ValidateNode(ByVal sender As Object, ByVal e As DevExpress.XtraTreeList.ValidateNodeEventArgs) Handles uxparams.ValidateNode
        Try

            Dim val As String
            Dim idx As Integer
            val = Me.uxparams.Selection.Item(0).GetValue(1)
            idx = CInt(Me.uxparams.Selection.Item(0).Tag)
            Dim cidx As Integer

            If Me.has_context_items = True Then
                If idx >= Me.context_items_pos Then
                    cidx = idx - Me.context_items_pos
                    idx = Me.context_items_pos
                End If
            End If

            If _params(idx).parameter.datatype = 0 And Not IsNumeric(val) Then
                Dim omsg As New bc_cs_message("Blue Curve", "Value must be numeric", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                e.Valid = False
                Exit Sub
            ElseIf _params(idx).parameter.datatype = 1 And Not IsDate(val) Then
                Dim omsg As New bc_cs_message("Blue Curve", "Value must be date", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                e.Valid = False
                Exit Sub
            End If

            If _params(idx).parameter.datatype = 9 Then
                If val <> "System Defined" Then
                    _params(idx).parameter.context_items.table_items(cidx).system_defined = 0
                    _params(idx).parameter.context_items.table_items(cidx).new_item_name = val
                    For m = 0 To _params(idx).parameter.lookup_values.Count - 1
                        If _params(idx).parameter.lookup_values_ids(m) = val Then
                            _params(idx).parameter.context_items.table_items(cidx).new_item_id = _params(idx).parameter.lookup_values(m)
                            Exit For
                        End If
                    Next
                Else
                    _params(idx).parameter.context_items.table_items(cidx).system_defined = 1
                    _params(idx).parameter.context_items.table_items(cidx).new_item_name = ""
                    _params(idx).parameter.context_items.table_items(cidx).new_item_id = 0
                End If
            ElseIf _params(idx).parameter.datatype = 0 Or _params(idx).parameter.datatype = 1 Or _params(idx).parameter.datatype = 2 Then
                _params(idx).parameter.default_value_id = val
                _params(idx).parameter.default_value = val
                _params(idx).parameter.system_defined = 0
            Else
                If val <> "System Defined" Then
                    REM And _params(idx).parameter.system_defined <> 2 Then
                    _params(idx).parameter.default_value = val
                    _params(idx).parameter.system_defined = 0
                    For m = 0 To _params(idx).parameter.lookup_values.Count - 1
                        If _params(idx).parameter.lookup_values_ids(m) = _params(idx).parameter.default_value Then
                            _params(idx).parameter.default_value_id = _params(idx).parameter.lookup_values(m)
                            Exit For
                        End If
                    Next
                Else
                    _params(idx).parameter.system_defined = 1
                    _params(idx).parameter.default_value = "0"
                    _params(idx).parameter.default_value_id = "0"
                End If

                If _params(idx).parameter.datatype = 10 _
                           And idx < uxparams.Nodes.Count - 1 Then


                    If _params(idx + 1).parameter.datatype = 10 Then

                        Dim i As Integer

                        Dim dependant As Boolean = True
                        i = idx + 1
                        While dependant = True And i < uxparams.Nodes.Count

                            If _params(i).parameter.datatype = 10 Then
                                _params(i).parameter.lookup_values.Clear()
                                _params(i).parameter.lookup_values_ids.Clear()
                                _params(i).parameter.lookup_values.Add("0")
                                _params(i).parameter.lookup_values_ids.Add("")
                                _params(i).parameter.default_value_id = ""
                                _params(i).parameter.default_value = 0
                                uxparams.Nodes(i).SetValue(1, "")
                            Else
                                dependant = False
                            End If
                            i = i + 1
                        End While
                    End If
                End If
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("uxparams", "ValidateNode", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub

    Function check_ok() As Boolean
        check_ok = True
        For i = 0 To _params.Count - 1
            REM FIL 5.7
            If _params(i).parameter.mandatory = True AndAlso (_params(i).parameter.datatype = 15 Or _params(i).parameter.datatype = 16) AndAlso _params(i).parameter.list_key_ids.Count = 0 Then
                Dim msg As New bc_cs_message("Blue Curve", "At Least 1 " + _params(i).parameter.name + " must be set", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                check_ok = False
                Exit Function
            End If

            If _params(i).parameter.mandatory = True And _params(i).parameter.default_value = "" Then
                Dim msg As New bc_cs_message("Blue Curve", "Mandatory Values Not Set For:" + _params(i).parameter.name, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                check_ok = False
                Exit Function
            End If
        Next
    End Function

    Private Sub uxparams__Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles uxparams.Click
        check_multi_select()
    End Sub
    Private Sub check_multi_select()
        Try
            Dim idx As Integer
            idx = uxparams.Selection.Item(0).Id
            If Me.has_context_items = True Then
                If idx >= Me.context_items_pos Then
                    Exit Sub
                End If
            End If

            If _params(idx).parameter.datatype = 15 Or _params(idx).parameter.datatype = 16 Then
                Dim vassign As New bc_dx_assign
                Dim items As New List(Of C_bc_dx_assign.assign_list_items)
                Dim item As C_bc_dx_assign.assign_list_items
                Dim sel_items As New List(Of Long)
                For i = 0 To _params(idx).parameter.lookup_values_ids.Count - 1
                    item = New C_bc_dx_assign.assign_list_items
                    item.key = _params(idx).parameter.lookup_values(i)
                    item.val = _params(idx).parameter.lookup_values_ids(i)
                    item.sel = False
                    For j = 0 To _params(idx).parameter.list_key_ids.Count - 1
                        If _params(idx).parameter.list_key_ids(j) = item.key Then
                            item.sel = True
                        End If
                    Next
                    items.Add(item)
                Next

                For j = 0 To _params(idx).parameter.list_key_ids.Count - 1
                    For i = 0 To _params(idx).parameter.lookup_values_ids.Count - 1
                        If _params(idx).parameter.list_key_ids(j) = _params(idx).parameter.lookup_values(i) Then
                            sel_items.Add(_params(idx).parameter.lookup_values(i))
                            Exit For
                        End If
                    Next
                Next

                Dim num As Integer = 0
                If IsNumeric(_params(idx).parameter.default_value) Then
                    num = CLng(_params(idx).parameter.default_value)
                End If
                Dim cassign As New C_bc_dx_assign(sel_items, num, "Blue Curve - Assign " + uxparams.Selection.Item(0).GetValue(0), vassign, items)
                vassign.ShowDialog()

                If cassign.save_selected = True Then
                    _params(idx).parameter.list_key_ids.Clear()
                    For i = 0 To cassign.sel_items.Count - 1
                        _params(idx).parameter.list_key_ids.Add(cassign.sel_items(i))
                    Next
                End If
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("uxparams", "check_multi_select", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub


    Private Sub uxparams_FocusedNodeChanged(ByVal sender As System.Object, ByVal e As DevExpress.XtraTreeList.FocusedNodeChangedEventArgs) Handles uxparams.FocusedNodeChanged
        Me.Cursor = Cursors.WaitCursor
        Try
            Dim idx As Integer
            Me.RepositoryItemComboBox3.Items.Clear()
            idx = e.Node.Id
            If Me.has_context_items = True Then
                If idx >= Me.context_items_pos Then
                    idx = Me.context_items_pos
                End If
            End If

            If _params(idx).parameter.datatype = 0 Or _params(idx).parameter.datatype = 1 Or _params(idx).parameter.datatype = 2 Then
                Me.RepositoryItemComboBox3.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard
            ElseIf _params(idx).parameter.datatype = 3 Then
                Me.RepositoryItemComboBox3.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
                Me.RepositoryItemComboBox3.Items.Add("true")
                Me.RepositoryItemComboBox3.Items.Add("false")
            Else
                Me.RepositoryItemComboBox3.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
                If _params(idx).parameter.datatype = 15 Or _params(idx).parameter.datatype = 16 Then
                    Exit Sub
                Else
                    If _params(idx).parameter.datatype = 10 Then
                        _ctrll.dynamic_lookup(_params(idx), _params)
                    End If
                    REM PR JULY 2016
                    If _params(idx).parameter.original_system_defined = 1 Or _params(idx).parameter.datatype = 9 Then
                        Me.RepositoryItemComboBox3.Items.Add("System Defined")
                    End If
                    For i = 0 To _params(idx).parameter.lookup_values.Count - 1
                        Me.RepositoryItemComboBox3.Items.Add(_params(idx).parameter.lookup_values_ids(i))
                    Next
                End If
            End If
            If _params(idx).parameter.disabled_in_doc = 1 Then
                Me.RepositoryItemComboBox3.ReadOnly = True
            Else
                Me.RepositoryItemComboBox3.ReadOnly = False
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("uxparams", "FocusedNodeChanged", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    Private Sub bcancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bcancel.Click
        RaiseEvent cancel(_locator)
        Me.Hide()
    End Sub

    Private Sub RepositoryItemComboBox3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RepositoryItemComboBox3.Click
        check_multi_select()
    End Sub

    Private Sub brefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles brefresh.Click
        Try
            If check_ok() = False Then
                Exit Sub
            End If
            If Me.chkdisable.Visible = False Then
                RaiseEvent doc_refresh(_params, Nothing, Nothing)
            Else
                If uxcont.SelectedIndex = -1 Then
                    RaiseEvent doc_refresh(_params, 1, Me.chkdisable.CheckState)
                Else
                    RaiseEvent doc_refresh(_params, bc_am_load_objects.obc_pub_types.contributors(uxcont.SelectedIndex).id, Me.chkdisable.CheckState)
                End If
            End If
            Me.Hide()
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("brefresh", "click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Private Sub chkdisable_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkdisable.CheckStateChanged
        If chkdisable.CheckState = 1 Then
            Me.brefresh.Enabled = False
        Else
            Me.brefresh.Enabled = True
        End If
    End Sub
    Private pmin As Integer
    Private pmax As Integer
    Private Sub trange_EditValueChanged(sender As Object, e As EventArgs) Handles trange.EditValueChanged
        If gap > 0 Then

            If Me.trange.Value.Maximum - Me.trange.Value.Minimum > gap Then
                Dim r As New DevExpress.XtraEditors.Repository.TrackBarRange
                If pmax <> Me.trange.Value.Maximum Then
                    r.Maximum = Me.trange.Value.Maximum
                    r.Minimum = Me.trange.Value.Maximum - gap
                Else
                    r.Maximum = Me.trange.Value.Minimum + gap
                    r.Minimum = Me.trange.Value.Minimum
                End If
             
                Me.trange.Value = r
            End If
        End If

        Me.set_range_text()

    End Sub

    Private Sub chkna_CheckedChanged(sender As Object, e As EventArgs) Handles chkna.CheckedChanged
        If Me.chkna.Checked = True Then
            Me.lrange.Text = "non applicable"
            Me.trange.Visible = False
            With _params(0).parameter
                .default_value_id = -99999
                .default_value = "n/a"
            End With
            With _params(1).parameter
                .default_value_id = -99999
                .default_value = "n/a"
            End With
            Me.Button3.Visible = False

        Else
            Me.trange.Visible = True
            Me.Button3.Visible = True
            Me.def1 = 0
            Me.def2 = 0
            Dim r As DevExpress.XtraEditors.Repository.TrackBarRange
            For i = 0 To picker_vals.Count - 1
                If picker_vals(i).id = def1 Then
                    r.Minimum = i
                End If
                If picker_vals(i).id = def2 Then
                    r.Maximum = i
                End If
            Next
            Me.trange.Value = r
            Me.lrange.Text = "no change"



        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim r As DevExpress.XtraEditors.Repository.TrackBarRange

        For i = 0 To picker_vals.Count - 1
            If picker_vals(i).id = 0 Then
                r.Minimum = i
            End If
            If picker_vals(i).id = 0 Then
                r.Maximum = i
            End If
        Next
        Me.trange.Value = r
        Me.lrange.Text = "no change"

    End Sub

    Private Sub view_bc_dx_build_parameters_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class
REM controller
Public Class build_parameter
    Public id As Integer
    Public ord As Integer
    Public parameter As bc_om_component_parameter
End Class
Public Class ctrll_bc_am_build_parameters
    Private WithEvents _view As Iview_bc_am_build_parameters
    Public cancel_selected As Boolean = True
    Public refresh As Boolean = False
    Dim _adoc As bc_om_document
    Dim _locator As String = ""
    Dim _params As New List(Of build_parameter)
    Dim _from_adv As Boolean = False
    Public browser = False

    Public Sub New(ByVal view As Iview_bc_am_build_parameters, ByVal adoc As bc_om_document)
        _view = view
        _adoc = adoc
        _locator = ""
    End Sub
    Public Sub New(ByVal view As Iview_bc_am_build_parameters, ByVal adoc As bc_om_document, ByVal locator As String, Optional enable_cancel As Boolean = True, Optional ByVal enable_refresh As Boolean = True, Optional ByVal disable_refresh As Boolean = False,
                       Optional ByVal disable_contributer As Boolean = False,
                       Optional ByVal enable_save As Boolean = True)
        _view = view
        _adoc = adoc
        _locator = locator

        If enable_cancel = False Then
            _view.disable_cancel()
        End If
        If enable_refresh = False Then
            _view.disable_refresh()
        End If
        If disable_refresh = True Then
            _view.disable_refresh_disabled()
        End If
        If disable_contributer = True Then
            _view.disable_contributer()
        End If
        If enable_save = False Then
            _view.disable_save()
        End If

    End Sub
    Public Sub New(ByVal view As Iview_bc_am_build_parameters, ByVal adoc As bc_om_document, ByVal locator As String, ByVal from_adv As Boolean)
        _view = view
        _adoc = adoc
        _locator = locator
        _from_adv = True
    End Sub
    Public Function load_data() As Boolean
        Try
            REM type 7 system_defined 2 required server read 
            load_data = False
            Dim oparam As build_parameter
            Dim oparam_values As New bc_om_parameter_lookup
          
            REM ammend component mode
            If _locator <> "" Then
                For k = 0 To _adoc.refresh_components.refresh_components.Count - 1
                    If _adoc.refresh_components.refresh_components(k).locator = _locator Then
                        REM FIL 5.7 launch a browser screeb


                        For m = 0 To _adoc.refresh_components.refresh_components(k).parameters.component_template_parameters.Count - 1
                            With _adoc.refresh_components.refresh_components(k).parameters.component_template_parameters(m)
                                REM FIL 5.7 launch a browser screeb
                                If (.datatype = 20) Then
                                    
                                    Dim url As String
                                    url = _adoc.refresh_components.refresh_components(k).parameters.component_template_parameters(m).lookup_sql
                                    url = url + "?doc_id=" + CStr(_adoc.id) + ",entity_id=" + CStr(_adoc.entity_id) + ",pub_type_id=" + CStr(_adoc.pub_type_id) + ",user_id=" + CStr(_adoc.originating_author)
                                    Try
                                        System.Diagnostics.Process.Start(url)
                                        load_data = True
                                        browser = True
                                    Catch
                                        Dim omsg As New bc_cs_message("Blue Curve", "Failed to invoke URL: " + url, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                                    End Try

                                   
                                    Exit Function
                                End If

                                If (.datatype = 7 And .system_defined = 2) Then
                                    If bc_am_load_objects.obc_current_document.id = 0 Then
                                        bc_am_load_objects.obc_current_document.refresh_components.refresh_components(k).doc_id = bc_am_load_objects.obc_current_document.filename
                                    Else
                                        bc_am_load_objects.obc_current_document.refresh_components.refresh_components(k).doc_id = bc_am_load_objects.obc_current_document.id
                                    End If
                                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                                        bc_am_load_objects.obc_current_document.refresh_components.refresh_components(k).read_system_defined_value()
                                    Else
                                        bc_am_load_objects.obc_current_document.refresh_components.refresh_components(k).tmode = bc_cs_soap_base_class.tREAD
                                        bc_am_load_objects.obc_current_document.refresh_components.refresh_components(k).tmode = bc_om_refresh_component.tREAD_SYSTEM_DEFINED_VALUE
                                        If bc_am_load_objects.obc_current_document.refresh_components.refresh_components(k).transmit_to_server_and_receive(bc_am_load_objects.obc_current_document.refresh_components.refresh_components(k), True) = False Then
                                            Exit Function
                                        End If
                                    End If
                                    Exit For
                                End If
                            End With
                        Next
                        Dim cis As bc_om_parameter_context_items
                        Dim ci As bc_om_parameter_context_item
                        For m = 0 To _adoc.refresh_components.refresh_components(k).parameters.component_template_parameters.Count - 1
                            With _adoc.refresh_components.refresh_components(k).parameters.component_template_parameters(m)
                                oparam = New build_parameter
                                oparam.parameter = New bc_om_component_parameter
                                oparam.id = k
                                oparam.ord = m
                                oparam.parameter.disabled_in_doc = .disabled_in_doc

                                If .datatype = 9 Then
                                    cis = New bc_om_parameter_context_items
                                    cis.context_id = .context_items.context_id
                                    cis.doc_id = .context_items.doc_id
                                    cis.entity_id = .context_items.entity_id
                                    cis.system_defined = .context_items.system_defined

                                    For h = 0 To .context_items.table_items.count - 1
                                        ci = New bc_om_parameter_context_item
                                        ci.col = .context_items.table_items(h).col
                                        ci.new_item_id = .context_items.table_items(h).new_item_id
                                        ci.new_item_name = .context_items.table_items(h).new_item_name
                                        ci.orig_item_id = .context_items.table_items(h).orig_item_id
                                        ci.orig_item_name = .context_items.table_items(h).orig_item_name
                                        ci.row = .context_items.table_items(h).row
                                        ci.system_defined = .context_items.table_items(h).system_defined
                                        cis.table_items.Add(ci)
                                    Next
                                    oparam.parameter.context_items = Nothing
                                    oparam.parameter.context_items = cis
                                End If
                                'oparam.parameter.context_items = .context_items
                                oparam.parameter.datatype = .datatype
                                oparam.parameter.default_value = .default_value
                                oparam.parameter.default_value_id = .default_value_id
                                oparam.parameter.dependent_parameter_values = .dependent_parameter_values
                                oparam.parameter.disabled_in_doc = .disabled_in_doc
                                For p = 0 To .list_key_ids.Count - 1
                                    oparam.parameter.list_key_ids.Add(.list_key_ids(p))
                                Next
                                oparam.parameter.locator = .locator
                                oparam.parameter.lookup_sql = .lookup_sql
                                oparam.parameter.lookup_values = .lookup_values
                                oparam.parameter.lookup_values_ids = .lookup_values_ids
                                oparam.parameter.mandatory = .mandatory
                                oparam.parameter.name = .name
                                oparam.parameter.order = .order
                                oparam.parameter.original_system_defined = .original_system_defined
                                oparam.parameter.system_defined = .system_defined
                                oparam.parameter.type_id = .type_id

                                If (.datatype = 7 Or .datatype = 10) Then
                                    oparam_values = get_lookup_vals(.datatype, .lookup_sql, .dependent_parameter_values)
                                    oparam.parameter.lookup_values = oparam_values.lookup_vals
                                    oparam.parameter.lookup_values_ids = oparam_values.lookup_vals_ids
                                End If
                                _params.Add(oparam)
                            End With
                        Next
                        _view.load_list("Blue Curve - Amend Component: " + _adoc.refresh_components.refresh_components(k).name, _params, Me, _adoc.refresh_components.refresh_components(k).contributor_id, _adoc.refresh_components.refresh_components(k).disabled, _adoc.refresh_components.refresh_components(k).last_refresh_date, _adoc.refresh_components.refresh_components(k).locator)
                        _view.set_ammend_controls(_from_adv)
                        load_data = True
                        Exit For
                    End If
                Next

            Else
                REM build paramters mode
                For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                    If bc_am_load_objects.obc_pub_types.pubtype(i).id = _adoc.pub_type_id Then
                        For j = 0 To bc_am_load_objects.obc_pub_types.pubtype(i).build_components.Count - 1
                            For k = 0 To _adoc.refresh_components.refresh_components.Count - 1
                                If bc_am_load_objects.obc_pub_types.pubtype(i).build_components(j) = _adoc.refresh_components.refresh_components(k).type Then
                                    For m = 0 To _adoc.refresh_components.refresh_components(k).parameters.component_template_parameters.Count - 1
                                        With _adoc.refresh_components.refresh_components(k).parameters.component_template_parameters(m)
                                            If (.datatype = 7 And .system_defined = 2) Then
                                                If bc_am_load_objects.obc_current_document.id = 0 Then
                                                    bc_am_load_objects.obc_current_document.refresh_components.refresh_components(k).doc_id = bc_am_load_objects.obc_current_document.filename
                                                Else
                                                    bc_am_load_objects.obc_current_document.refresh_components.refresh_components(k).doc_id = bc_am_load_objects.obc_current_document.id
                                                End If
                                                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                                                    bc_am_load_objects.obc_current_document.refresh_components.refresh_components(k).read_system_defined_value()
                                                Else
                                                    bc_am_load_objects.obc_current_document.refresh_components.refresh_components(k).tmode = bc_om_refresh_component.tREAD_SYSTEM_DEFINED_VALUE
                                                    If bc_am_load_objects.obc_current_document.refresh_components.refresh_components(k).transmit_to_server_and_receive(bc_am_load_objects.obc_current_document.refresh_components.refresh_components(k), True) = False Then
                                                        Exit Function
                                                    End If
                                                End If
                                                Exit For
                                            End If
                                        End With
                                    Next


                                    For m = 0 To _adoc.refresh_components.refresh_components(k).parameters.component_template_parameters.Count - 1
                                        With _adoc.refresh_components.refresh_components(k).parameters.component_template_parameters(m)
                                            oparam = New build_parameter
                                            oparam.id = k

                                            oparam.ord = m
                                            oparam.parameter = _adoc.refresh_components.refresh_components(k).parameters.component_template_parameters(m)
                                            If (.datatype = 7 Or .datatype = 10) Then
                                                oparam_values = get_lookup_vals(.datatype, .lookup_sql, .dependent_parameter_values)
                                                oparam.parameter.lookup_values = oparam_values.lookup_vals
                                                oparam.parameter.lookup_values_ids = oparam_values.lookup_vals_ids
                                            End If
                                            _params.Add(oparam)
                                        End With
                                    Next

                                End If
                            Next
                        Next
                        If bc_am_load_objects.obc_pub_types.pubtype(i).build_components.count > 0 Then
                            _view.load_list("Blue Curve - Document Build Parameters ", _params, Me, Nothing, Nothing, Nothing, Nothing)
                            load_data = True
                            Exit For
                        Else
                            cancel_selected = False

                        End If
                    End If
                Next
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("ctrll_bc_am_build_parameters", "load_data", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Function
    Private Function get_lookup_vals(ByVal datatype As Long, ByVal lookup_sql As String, ByVal dependent_parameter_values As List(Of String)) As bc_om_parameter_lookup
        Try
            Dim oparam_values As New bc_om_parameter_lookup
            oparam_values.type_id = datatype
            oparam_values.lookup_sql = lookup_sql
            oparam_values.entity_id = _adoc.entity_id
            oparam_values.doc_id = _adoc.filename
            oparam_values.dependent_parameter_values = dependent_parameter_values

            If bc_am_load_objects.obc_current_document.connection_method = bc_cs_central_settings.ADO Then
                oparam_values.db_read()
            Else
                oparam_values.tmode = bc_cs_soap_base_class.tREAD
                oparam_values.transmit_to_server_and_receive(oparam_values, True)
            End If

            get_lookup_vals = oparam_values
        Catch ex As Exception
            get_lookup_vals = Nothing
            Dim oerr As New bc_cs_error_log("ctrll_bc_am_build_parameters", "get_lookup_vals", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Function
    Public Sub dynamic_lookup(ByRef param As build_parameter, ByVal params As List(Of build_parameter))
        If (param.parameter.datatype = 10) Then
            Dim oparam_values As New bc_om_parameter_lookup
            For i = 0 To params.Count - 1
                If params(i).parameter.datatype = 10 Then
                    oparam_values.dependent_parameter_values.Add(params(i).parameter.default_value_id)
                End If
            Next
            oparam_values = get_lookup_vals(param.parameter.datatype, param.parameter.lookup_sql, oparam_values.dependent_parameter_values)
            param.parameter.lookup_values.Clear()
            param.parameter.lookup_values_ids.Clear()
            param.parameter.lookup_values = oparam_values.lookup_vals
            param.parameter.lookup_values_ids = oparam_values.lookup_vals_ids

        End If
    End Sub
    Public Sub save_parameter_values(ByVal params As List(Of build_parameter), ByVal contributor_id As Long, ByVal disabled As Integer) Handles _view.save
        save(params, contributor_id, disabled)
        cancel_selected = False
        refresh = False
    End Sub

    Public Sub doc_refresh(ByVal params As List(Of build_parameter), ByVal contributor_id As Long, ByVal disabled As Integer) Handles _view.Refresh
        save(params, contributor_id, disabled)
        cancel_selected = False
        refresh = True
    End Sub
    Public Sub cancel(ByVal locator As String) Handles _view.Cancel
        cancel_selected = True
        refresh = False
    End Sub


    Public Sub save(ByVal params As List(Of build_parameter), ByVal contributor_id As Long, ByVal disabled As Integer)
        Try
            If _locator <> "" Then
                For j = 0 To _adoc.refresh_components.refresh_components.Count - 1
                    If _adoc.refresh_components.refresh_components(j).locator = _locator Then
                        If IsNothing(contributor_id) = False Then
                            _adoc.refresh_components.refresh_components(j).contributor_id = contributor_id
                        End If
                        If IsNothing(contributor_id) = False Then
                            _adoc.refresh_components.refresh_components(j).disabled = disabled
                        End If
                        Exit For
                    End If
                Next
            End If

            For i = 0 To params.Count - 1
                For j = 0 To _adoc.refresh_components.refresh_components.Count - 1
                    If j = params(i).id Then

                        'If params(i).parameter.system_defined = 2 Then
                        '    params(i).parameter.system_defined = 0
                        'End If

                        _adoc.refresh_components.refresh_components(j).parameters.component_template_parameters(params(i).ord) = params(i).parameter
                    End If
                Next
            Next
            If _from_adv = False Then
                If _adoc.id = 0 Then
                    Dim fn As String
                    fn = Replace(_adoc.filename, _adoc.extension, "")
                    _adoc.write_data_to_file(bc_cs_central_settings.local_repos_path + CStr(fn) + ".dat")
                Else
                    _adoc.write_data_to_file(bc_cs_central_settings.local_repos_path + CStr(_adoc.id) + ".dat")
                End If
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("ctrll_bc_am_build_parameters", "save_parameter_values", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
End Class
REM view interface
Public Interface Iview_bc_am_build_parameters
    Sub load_list(ByVal caption As String, ByVal params As List(Of build_parameter), ByVal ctrll As ctrll_bc_am_build_parameters, ByVal contributor As Long, ByVal disabled As Integer, ByVal last_refresh As DateTime, ByVal locator As String)
    Event save(ByVal params As List(Of build_parameter), ByVal contributor_id As Long, ByVal disabled As Integer)
    Event Refresh(ByVal params As List(Of build_parameter), ByVal contributor_id As Long, ByVal disabled As Integer)
    Event Cancel(ByVal locator As String)
    Sub set_ammend_controls(ByVal _from_adv As Boolean)
    Sub disable_cancel()
    Sub disable_refresh_disabled()
    Sub disable_refresh()
    Sub disable_contributer()
    Sub disable_save()
    
End Interface