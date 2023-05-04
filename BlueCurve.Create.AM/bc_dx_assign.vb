Imports BlueCurve.Core.CS

Public Class bc_dx_assign
    Implements I_bc_dx_assign
    Dim _items As New List(Of C_bc_dx_assign.assign_list_items)
    Dim _sel_items As New List(Of Long)
    Dim _max_number As Integer = -1
    Event save(ByVal _sel_items As List(Of Long)) Implements I_bc_dx_assign.save

    Public Sub load_data(ByVal sel_items As List(Of Long), ByVal max_number As Integer, ByVal caption As String, ByVal items As System.Collections.Generic.List(Of C_bc_dx_assign.assign_list_items)) Implements I_bc_dx_assign.load_data
        Me.Text = caption
        _items = items
        _sel_items = sel_items
        _max_number = max_number
        load_list()
        load_lsel_list()
    End Sub
    Private Sub load_list()
        Try
            Me.uxall.Items.Clear()
            If Trim(Me.tsearch.Text) = "" Then
                For i = 0 To _items.Count - 1
                    If _items(i).sel = False Then
                        Me.uxall.Items.Add(_items(i).val)
                    End If
                Next
            Else
                For i = 0 To _items.Count - 1
                    If InStr(LCase(_items(i).val), LCase(Me.tsearch.Text)) > 0 Then
                        If _items(i).sel = False Then
                            Me.uxall.Items.Add(_items(i).val)
                        End If
                    End If
                Next
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_dx_assign", "load_list", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Private Sub load_lsel_list()
        Try
            Dim sel_count As Integer = 0
            Dim co As Integer = 1
            Me.uxsel.Items.Clear()

            For i = 0 To _sel_items.Count - 1
                For j = 0 To _items.Count - 1
                    If _items(j).sel = True And _items(j).key = _sel_items(i) Then
                        Me.uxsel.Items.Add(_items(j).val)
                        Exit For
                    End If

                Next
            Next

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_dx_assign", "load_sel_list", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub

    Private Sub bok_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bok.Click
        Dim sel_items As New List(Of Long)
        For i = 0 To Me.uxsel.Items.Count - 1
            For j = 0 To _items.Count - 1
                If _items(j).val = Me.uxsel.Items(i) Then
                    sel_items.Add(_items(j).key)
                    Exit For
                End If
            Next
        Next
        Me.Hide()
        RaiseEvent save(sel_items)
    End Sub

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        Me.Hide()
    End Sub

    'Private Sub uxall_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles uxall.DoubleClick
    '    Me.uxsel.Items.Add(Me.uxall.SelectedItem.ToString)
    '    Me.uxall.Items.RemoveAt(Me.uxall.S



    'End Sub
    Private updating As Boolean
    Private lupdating As Boolean
    Private loading As Boolean
    Private Sub tsearch_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsearch.EditValueChanged
        searchtimer.Stop()
        searchtimer.Start()
    End Sub
    Private Sub lSearchTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles searchtimer.Tick
        searchtimer.Stop()
        Me.Cursor = Windows.Forms.Cursors.WaitCursor
        load_list()
        Me.Cursor = Windows.Forms.Cursors.Default
    End Sub

    Private Sub uxall_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxall.DoubleClick
        If _max_number <> 0 And _max_number < Me.uxsel.Items.Count + 1 Then
            Dim omsg As New bc_cs_message("Blue Curve", "Maximum of " + CStr(_max_number) + " can be seleced!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            Exit Sub
        End If

        For i = 0 To _items.Count - 1
            If _items(i).val = uxall.SelectedValue Then
                _items(i).sel = True
                Me.uxsel.Items.Add(uxall.SelectedValue)
            End If
        Next
        load_list()
    End Sub
    Private Sub uxsel_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxsel.DoubleClick
        For i = 0 To _items.Count - 1
            If _items(i).val = uxsel.SelectedValue Then
                _items(i).sel = False
                Me.uxsel.Items.RemoveAt(Me.uxsel.SelectedIndex)
                Exit For
            End If
        Next
        load_list()
    End Sub

    Private Sub uxsel_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxsel.SelectedIndexChanged
        Me.bup.Enabled = False
        Me.bdn.Enabled = False
        If Me.uxsel.Items.Count < 2 Then
            Exit Sub
        End If
        If Me.uxsel.SelectedIndex > 0 Then
            Me.bup.Enabled = True
        End If
        If Me.uxsel.SelectedIndex < Me.uxsel.Items.Count - 1 Then
            Me.bdn.Enabled = True
        End If

    End Sub

    Private Sub bup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bup.Click
        Dim tx As String
        Dim i As Integer
        i = Me.uxsel.SelectedIndex
        tx = Me.uxsel.SelectedItem
        Me.uxsel.Items.RemoveAt(i)
        Me.uxsel.Items.Insert(i - 1, tx)
        Me.uxsel.SelectedIndex = (i - 1)

    End Sub
    Private Sub bdn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bdn.Click
        Dim tx As String
        Dim i As Integer
        i = Me.uxsel.SelectedIndex
        tx = Me.uxsel.SelectedItem
        Me.uxsel.Items.RemoveAt(i)
        Me.uxsel.Items.Insert(i + 1, tx)
        Me.uxsel.SelectedIndex = (i + 1)
    End Sub
End Class

Public Class C_bc_dx_assign
    WithEvents _view As I_bc_dx_assign
    Dim _items As List(Of assign_list_items)
    Public sel_items As New List(Of Long)
    Public save_selected As Boolean = False

    Public Class assign_list_items
        Public key As Long
        Public val As String
        Public sel As Boolean = False

    End Class

    Public Sub New(ByVal sel_items As List(Of Long), ByVal max_number As Integer, ByVal caption As String, ByVal view As I_bc_dx_assign, ByVal items As List(Of assign_list_items))
        _view = view
        _items = items
        _view.load_data(sel_items, max_number, caption, _items)

    End Sub
    Public Sub save(ByVal sitems As List(Of Long)) Handles _view.save
        Try
            save_selected = True
            sel_items.Clear()
            sel_items = sitems


        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_dx_assign", "save", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
End Class
Public Interface I_bc_dx_assign
    Sub load_data(ByVal sel_items As List(Of Long), ByVal max_number As Integer, ByVal caption As String, ByVal items As List(Of C_bc_dx_assign.assign_list_items))
    Event save(ByVal sel_items As List(Of Long))
End Interface
