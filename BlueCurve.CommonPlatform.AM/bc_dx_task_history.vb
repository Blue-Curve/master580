Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM

Public Class bc_dx_task_history
    Implements Ibc_dx_task_history
    Dim _task_name As String
    Public Function load_view(task As bc_om_dl_tasks.bc_om_dl_task, task_name As String) As Boolean Implements Ibc_dx_task_history.load_view
        Try
            load_view = False
            _task_name = task_name

            Me.Text = "Blue Curve - Task History: " + task_name
            uxhist.BeginUpdate()
            For i = 0 To task.history.Count - 1
                uxhist.Nodes.Add()
                uxhist.Nodes(uxhist.Nodes.Count - 1).SetValue(0, Format(task.history(i).run_date.ToLocalTime, "dd MMM yyyy HH:mm:ss"))
                uxhist.Nodes(uxhist.Nodes.Count - 1).SetValue(1, Format(task.history(i).complete_date.ToLocalTime, "dd MMM yyyy HH:mm:ss"))
                Dim duration As Integer = 0
                Try
                    duration = DateDiff(DateInterval.Second, task.history(i).run_date, task.history(i).complete_date)
                Catch

                End Try
                uxhist.Nodes(uxhist.Nodes.Count - 1).SetValue(2, duration)

                If task.history(i).status = 2 Then
                    uxhist.Nodes(uxhist.Nodes.Count - 1).SetValue(3, "Success")
                    uxhist.Nodes(uxhist.Nodes.Count - 1).StateImageIndex = 1
                ElseIf task.history(i).status = 3 Then
                    uxhist.Nodes(uxhist.Nodes.Count - 1).SetValue(3, "Fail")
                    uxhist.Nodes(uxhist.Nodes.Count - 1).StateImageIndex = 0
                End If
                uxhist.Nodes(uxhist.Nodes.Count - 1).SetValue(4, task.history(i).comment)
            Next

            load_view = True
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_dx_task_history", "load_view", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            uxhist.EndUpdate()

        End Try
    End Function
    Private Sub bclose_Click(sender As Object, e As EventArgs) Handles bclose.Click
        Me.Hide()
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs)
        load_comment()
    End Sub

    Sub load_comment()
        Cursor = Windows.Forms.Cursors.WaitCursor


        Dim fc As New bc_dx_html_task_comment
        Dim cc As New Cbc_dx_html_task_comment
        If cc.load_data(fc, uxhist.Selection(0).GetValue(4), _task_name) = True Then
            Cursor = Windows.Forms.Cursors.Default
            fc.ShowDialog()
        End If
        Cursor = Windows.Forms.Cursors.Default
    End Sub


    Private Sub uxhist_DoubleClick(sender As Object, e As EventArgs) Handles uxhist.DoubleClick
        If uxhist.Selection.Count > 0 AndAlso uxhist.Selection(0).GetValue(4) <> "" Then

            load_comment()
        End If
    End Sub

  

End Class
Public Class Cbc_dx_task_history
    WithEvents _view As Ibc_dx_task_history
    Public Function load_data(view As Ibc_dx_task_history, task_id As Integer, task_name As String) As Boolean
        load_data = False
        Try
            _view = view
            Dim task As New bc_om_dl_tasks.bc_om_dl_task
            task.task_id = task_id
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                task.db_read()
            Else
                task.tmode = bc_cs_soap_base_class.tREAD
                If task.transmit_to_server_and_receive(task, True) = False Then
                    Exit Function
                End If
            End If
            load_data = _view.load_view(task, task_name)

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Cbc_dx_task_history", "load_data", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Function
End Class
Public Interface Ibc_dx_task_history
    Function load_view(task As bc_om_dl_tasks.bc_om_dl_task, task_name As String) As Boolean
End Interface
