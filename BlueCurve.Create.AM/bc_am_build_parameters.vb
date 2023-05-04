Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports BlueCurve.Core.AS
Imports System.Windows.Forms
Imports System.Collections
REM view
'Public Class view_bc_am_build_parameters
'    Implements Iview_bc_am_build_parameters
'    Dim _adoc As bc_om_document
'    Dim _params As New List(Of build_parameter)
'    Dim _ctrll As ctrll_bc_am_build_parameters

'    Dim hold_vals As New ArrayList

'    Public Event save(ByVal params As List(Of build_parameter)) Implements Iview_bc_am_build_parameters.save
'    Public Sub load_list(ByVal params As List(Of build_parameter), ByVal ctrll As ctrll_bc_am_build_parameters) Implements Iview_bc_am_build_parameters.load_list
'        Try
'            _params = params
'            _ctrll = ctrll


'            Dim lu As DataGridViewComboBoxCell

'            Dim found As Boolean
'            For j = 0 To _params.Count - 1
'                DataGridView1.Rows.Add()
'                found = False

'                With _params(j).parameter
'                    If .mandatory = True Then
'                        DataGridView1.Item(0, DataGridView1.Rows.Count - 1).Value = tabimages.Images(9)
'                    Else
'                        DataGridView1.Item(0, DataGridView1.Rows.Count - 1).Value = New Drawing.Bitmap(1, 1)
'                    End If
'                    DataGridView1.Item(1, DataGridView1.Rows.Count - 1).Value = .name

'                    lu = New DataGridViewComboBoxCell
'                    lu.Style = DataGridView1.Columns(2).DefaultCellStyle

'                    lu.Style.Font = DataGridView1.Columns(1).DefaultCellStyle.Font



'                    'lu.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing


'                    If .system_defined = 1 Then
'                        lu.Items.Add("system defined")
'                    End If
'                    For i = 0 To .lookup_values.Count - 1
'                        lu.Items.Add(.lookup_values_ids(i))

'                        If .lookup_values_ids(i) = .default_value Then

'                            found = True
'                        End If
'                    Next


'                    If .system_defined = 1 Then
'                        lu.Value = "system defined"
'                    Else
'                        REM check value is in list
'                        If found = True Then
'                            lu.Value = .default_value
'                        End If
'                    End If

'                    DataGridView1.Item(2, DataGridView1.Rows.Count - 1) = lu

'                End With
'            Next

'            Me.Height = DataGridView1.Rows(0).Height * _params.Count - 1 + 140


'            Dim oapi As New API
'            API.SetWindowPos(Me.Handle.ToInt32, API.HWND_TOPMOST, 10, 10, 1, 1, 1)


'        Catch ex As Exception
'            Dim oerr As New bc_cs_error_log("view_bc_am_build_parameters", "load_list", bc_cs_error_codes.USER_DEFINED, ex.Message)
'        End Try
'    End Sub

'    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bok.Click
'        If check_ok() = False Then
'            Exit Sub
'        End If
'        RaiseEvent save(_params)
'        Me.Hide()
'    End Sub

'    Private Sub DataGridView1_CellValidating(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellValidatingEventArgs) Handles DataGridView1.CellValidating

'        If e.ColumnIndex = 2 Then
'            If e.FormattedValue <> "system defined" Then
'                _params(e.RowIndex).parameter.default_value = e.FormattedValue
'                _params(e.RowIndex).parameter.system_defined = 0
'                For m = 0 To _params(e.RowIndex).parameter.lookup_values.Count - 1
'                    If _params(e.RowIndex).parameter.lookup_values_ids(m) = _params(Me.DataGridView1.SelectedCells(0).RowIndex).parameter.default_value Then
'                        _params(e.RowIndex).parameter.default_value_id = _params(Me.DataGridView1.SelectedCells(0).RowIndex).parameter.lookup_values(m)
'                        Exit For
'                    End If
'                Next


'            Else
'                _params(e.RowIndex).parameter.system_defined = 1
'                _params(e.RowIndex).parameter.default_value = "0"
'                _params(e.RowIndex).parameter.default_value_id = "0"
'            End If

'            If _params(e.RowIndex).parameter.datatype = 10 _
'                       And e.RowIndex < DataGridView1.RowCount Then

'                If _params(e.RowIndex + 1).parameter.datatype = 10 Then
'                    Dim lu As New DataGridViewComboBoxCell

'                    Dim i As Integer
'                    _ctrll.dynamic_lookup(_params(e.RowIndex + 1), _params)

'                    For i = 0 To _params(e.RowIndex + 1).parameter.lookup_values.Count - 1
'                        lu.Items.Add(_params(e.RowIndex + 1).parameter.lookup_values_ids(i))
'                    Next

'                    lu.Value = ""
'                    DataGridView1.Item(2, e.RowIndex + 1) = lu

'                    'REM set boxes below to blank
'                    Dim dependant As Boolean = True

'                    i = e.RowIndex + 2
'                    While dependant = True And i < DataGridView1.Rows.Count
'                        If _params(i).parameter.datatype = 10 Then
'                            _params(i).parameter.lookup_values.Clear()
'                            _params(i).parameter.lookup_values_ids.Clear()
'                            _params(i).parameter.lookup_values.Add("0")
'                            _params(i).parameter.lookup_values_ids.Add("")
'                            _params(i).parameter.default_value_id = ""
'                            _params(i).parameter.default_value = 0
'                            lu = New DataGridViewComboBoxCell
'                            lu.Items.Add("")

'                            DataGridView1.Item(2, i) = lu
'                            DataGridView1.Item(2, i).Value = ""

'                        Else
'                            dependant = False

'                        End If
'                        i = i + 1
'                    End While


'                End If

'            End If
'        End If


'    End Sub

'    Function check_ok() As Boolean
'        check_ok = True
'        For i = 0 To _params.Count - 1
'            If _params(i).parameter.mandatory = True And _params(i).parameter.default_value = "" Then
'                'Dim msg As New bc_cs_message("Blue Curve", "Mandatory Values Not Set!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
'                check_ok = False

'                Exit Function
'            End If
'        Next
'    End Function




'End Class
