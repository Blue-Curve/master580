Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports System.Windows.Forms
Public Class bc_dx_extended_class
    Implements Iview_bc_am_extended_class
    Event save_data(ByVal oec As bc_om_extended_classification, ByVal tx As String) Implements Iview_bc_am_extended_class.save_data

    Dim _doc As bc_om_document


    Public Sub load_data(ByVal doc As bc_om_document) Implements Iview_bc_am_extended_class.load_data
        Try
            _doc = doc

            Me.timportant.Text = _doc.urgent_text
            For i = 0 To _doc.oec.Lists.Count - 1
                Me.TabControl1.TabPages.Add(_doc.oec.Lists(i).name)
                If i = 0 Then
                    load_list(0)
                End If
            Next
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_extended_class", "load_data", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Private Sub load_list(ByVal i As Integer)
        Try
            Me.litems.Items.Clear()
            For i = 0 To _doc.oec.Lists(i).items.Count - 1
                Me.litems.Items.Add(_doc.oec.Lists(Me.tabcontrol1.SelectedTabPageIndex).items(i).value)
                If _doc.oec.Lists(Me.tabcontrol1.SelectedTabPageIndex).items(i).selected = True Then
                    Me.litems.Items(Me.litems.Items.Count - 1).CheckState = CheckState.Checked
                End If
            Next
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_extended_class", "load_list", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Private Sub TabControl1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tabcontrol1.SelectedPageChanged
        Try
            load_list(Me.tabcontrol1.SelectedTabPageIndex)

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_extended_class", "TabControl1_SelectedIndexChanged", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub

    Private Sub bcancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Hide()
    End Sub
    Private Sub btnsubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnsubmit.Click
        RaiseEvent save_data(_doc.oec, Me.timportant.Text)
        Me.Hide()
    End Sub
    Private Sub check_ok()
        If IsNothing(_doc) = False Then
            Me.btnsubmit.Enabled = False
            Dim item_found As Boolean = False
            REM FIL 5.7
            If Trim(Me.timportant.Text) <> "" Then
                Me.btnsubmit.Enabled = True
                Exit Sub
            End If
            'If Trim(Me.timportant.Text) <> "" Then
            '    For i = 0 To _doc.oec.Lists.Count - 1
            '        For j = 0 To _doc.oec.Lists(i).items.Count - 1
            '            item_found = True
            '            If _doc.oec.Lists(i).items(j).selected = True Then
            '                Me.btnsubmit.Enabled = True
            '                Exit Sub
            '            End If
            '        Next
            '    Next
            '    If item_found = False Then
            '        Me.btnsubmit.Enabled = True
            '    End If
            'End If
        End If

    End Sub
    Private Sub litems_ItemCheck(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ItemCheckEventArgs) Handles litems.ItemCheck

        Try

            If e.State = CheckState.Checked Then
                _doc.oec.Lists(Me.tabcontrol1.SelectedTabPageIndex).items(e.Index).selected = True
            Else
                _doc.oec.Lists(Me.tabcontrol1.SelectedTabPageIndex).items(e.Index).selected = False
            End If

            check_ok()

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("cttrl_bc_am_extended_class", "litems_ItemCheck", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub

    Private Sub timportant_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles timportant.TextChanged
        check_ok()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bcancel.Click
        Me.Hide()
    End Sub



    Private Sub litems_SelectedIndexChanged(sender As Object, e As EventArgs) Handles litems.SelectedIndexChanged

    End Sub
End Class