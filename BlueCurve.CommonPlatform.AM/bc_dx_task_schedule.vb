Imports System.Threading
Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM





Public Class bc_dx_task_schedule
    Implements Ibc_dx_task_schedule
    Dim _tasks As New bc_om_dl_tasks
    Event refresh_view() Implements Ibc_dx_task_schedule.refresh_view
    Event set_enable(task_id As Integer, enable As Integer) Implements Ibc_dx_task_schedule.set_enable
     Event new_task() Implements Ibc_dx_task_schedule.new_task
    Event maintain_task(task As bc_om_dl_tasks.bc_om_dl_task) Implements Ibc_dx_task_schedule.maintain_task
    Event load_history(task_id As Integer, task_name As String) Implements Ibc_dx_task_schedule.load_history
    Event delete_task(task_id As Integer) Implements Ibc_dx_task_schedule.delete_task



    Public t As Thread


    Public Function load_view(tasks As bc_om_dl_tasks) As Boolean Implements Ibc_dx_task_schedule.load_view
        load_view = False

        Try
            _tasks = tasks
            load_task_tree()
            t = New Thread(AddressOf poll_view)
            t.Start()
            load_view = True
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_dx_task_schedule", "load_view", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Function
    Sub poll_view()
        While True
            Thread.Sleep(10000)
            RaiseEvent refresh_view()
        End While
    End Sub

    Public Function update_view(tasks As bc_om_dl_tasks) As Boolean Implements Ibc_dx_task_schedule.update_view
        update_view = False

        Try
            _tasks = tasks
            
            load_task_tree()

          

            update_view = True
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_dx_task_schedule", "update_view", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Function
    Private Delegate Sub load_task_tree_Delegate()

    Sub load_task_tree()
        Try

            If Me.uxtasks.InvokeRequired Then
                Dim ed As New load_task_tree_Delegate(AddressOf load_task_tree)
                Me.uxtasks.Invoke(ed, New Object() {})
                Exit Sub
            End If

            Me.uxtasks.BeginUpdate()
            Dim sel_row As String = ""
            If Me.uxtasks.Nodes.Count > 0 Then
                sel_row = Me.uxtasks.Selection(0).Tag
            End If
            Try
                Me.uxtasks.Nodes.Clear()
            Catch

            End Try

            For i = 0 To _tasks.tasks.Count - 1
                lpoll.Text = "Last Updated: " + Format(_tasks.tasks(i).current_datetime.ToLocalTime, "dd MMM yyyy HH:mm:ss")
                Me.uxtasks.Nodes.Add()
                Me.uxtasks.Nodes(Me.uxtasks.Nodes.Count - 1).SetValue(0, _tasks.tasks(i).task_name)
                If _tasks.tasks(i).input = True Then
                    Me.uxtasks.Nodes(Me.uxtasks.Nodes.Count - 1).SetValue(1, "Input")
                Else
                    Me.uxtasks.Nodes(Me.uxtasks.Nodes.Count - 1).SetValue(1, "Output")
                End If
                Dim fstr As String = ""
                Dim hr As String
                Dim mn As String
                Dim se As String
                Dim da As New DateTime("99", "9", "9", _tasks.tasks(i).hour, "0", "0")
                hr = da.ToLocalTime.Hour.ToString
                mn = _tasks.tasks(i).minute.ToString()
                se = _tasks.tasks(i).second.ToString()
                If Len(hr) = 1 Then
                    hr = "0" + hr
                End If
                If Len(mn) = 1 Then
                    mn = "0" + mn
                End If
                If Len(se) = 1 Then
                    se = "0" + se
                End If

                Select Case _tasks.tasks(i).frequency_type
                    Case 1
                        fstr = "Day at " + hr + ":" + mn + ":" + se
                    Case 2
                        fstr = "Hour(s) at " + mn + " minutes & " + se + " seconds past the hour"
                    Case 3
                        fstr = "Minute(s) at " + se + " seconds past the minute"
                    Case 4
                        fstr = "Second(s)"
                End Select
                fstr = "Every " + _tasks.tasks(i).interval.ToString + " " + fstr
                Me.uxtasks.Nodes(Me.uxtasks.Nodes.Count - 1).SetValue(2, fstr)


               
                If (_tasks.tasks(i).status = bc_om_dl_tasks.ETASK_STATUS.SERVICING) Then
                    Me.uxtasks.Nodes(Me.uxtasks.Nodes.Count - 1).SetValue(3, Format(_tasks.tasks(i).start_time.ToLocalTime, "dd MMM yyyy HH:mm:ss"))
                    Me.uxtasks.Nodes(Me.uxtasks.Nodes.Count - 1).SetValue(4, Format(_tasks.tasks(i).last_run.ToLocalTime, "dd MMM yyyy HH:mm:ss"))
                ElseIf (_tasks.tasks(i).status = bc_om_dl_tasks.ETASK_STATUS.NOT_YET_RUN) Then
                    Me.uxtasks.Nodes(Me.uxtasks.Nodes.Count - 1).SetValue(3, Format(_tasks.tasks(i).start_time.ToLocalTime, "dd MMM yyyy HH:mm:ss"))
                    If _tasks.tasks(i).disabled = False Then
                        Me.uxtasks.Nodes(Me.uxtasks.Nodes.Count - 1).SetValue(5, Format(_tasks.tasks(i).next_run.ToLocalTime, "dd MMM yyyy HH:mm:ss"))
                    End If
                    Else
                        Me.uxtasks.Nodes(Me.uxtasks.Nodes.Count - 1).SetValue(3, Format(_tasks.tasks(i).start_time.ToLocalTime, "dd MMM yyyy HH:mm:ss"))
                        Me.uxtasks.Nodes(Me.uxtasks.Nodes.Count - 1).SetValue(4, Format(_tasks.tasks(i).last_run.ToLocalTime, "dd MMM yyyy HH:mm:ss"))
                        If _tasks.tasks(i).disabled = False Then
                            Me.uxtasks.Nodes(Me.uxtasks.Nodes.Count - 1).SetValue(5, Format(_tasks.tasks(i).next_run.ToLocalTime, "dd MMM yyyy HH:mm:ss"))
                        End If
                    End If
                    Select Case _tasks.tasks(i).status
                        Case bc_om_dl_tasks.ETASK_STATUS.NOT_YET_RUN
                            Me.uxtasks.Nodes(Me.uxtasks.Nodes.Count - 1).SetValue(6, "Not yet run")
                            Me.uxtasks.Nodes(Me.uxtasks.Nodes.Count - 1).StateImageIndex = -1

                        Case bc_om_dl_tasks.ETASK_STATUS.SUCCESS
                            Me.uxtasks.Nodes(Me.uxtasks.Nodes.Count - 1).SetValue(6, "Success")
                            Me.uxtasks.Nodes(Me.uxtasks.Nodes.Count - 1).StateImageIndex = 1

                        Case bc_om_dl_tasks.ETASK_STATUS.FAIL
                            Me.uxtasks.Nodes(Me.uxtasks.Nodes.Count - 1).SetValue(6, "Failed")
                            Me.uxtasks.Nodes(Me.uxtasks.Nodes.Count - 1).StateImageIndex = 0
                        Case bc_om_dl_tasks.ETASK_STATUS.SERVICING
                            Me.uxtasks.Nodes(Me.uxtasks.Nodes.Count - 1).SetValue(6, "Running")
                            Me.uxtasks.Nodes(Me.uxtasks.Nodes.Count - 1).StateImageIndex = 2
                    End Select
                    Me.uxtasks.Nodes(Me.uxtasks.Nodes.Count - 1).SetValue(7, _tasks.tasks(i).comment)

                    If _tasks.tasks(i).disabled = False Then
                        Me.uxtasks.Nodes(Me.uxtasks.Nodes.Count - 1).SetValue(8, "True")
                    Else
                        Me.uxtasks.Nodes(Me.uxtasks.Nodes.Count - 1).SetValue(8, "False")
                        Me.uxtasks.Nodes(Me.uxtasks.Nodes.Count - 1).StateImageIndex = 3
                    End If


                    Me.uxtasks.Nodes(Me.uxtasks.Nodes.Count - 1).Tag = _tasks.tasks(i).task_id.ToString
            Next
            If sel_row <> "" Then
                For i = 0 To Me.uxtasks.Nodes.Count - 1
                    If Me.uxtasks.Nodes(i).Tag = sel_row Then
                        Me.uxtasks.Nodes(i).Selected = True
                        Exit For
                    End If

                Next
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_dx_task_schedule", "load_task_tree", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.uxtasks.EndUpdate()
        End Try

    End Sub

    Private Sub bclose_Click(sender As Object, e As EventArgs) Handles bclose.Click
        Me.Close()

    End Sub

   
    Private Sub uxaudit_FocusedNodeChanged(sender As Object, e As DevExpress.XtraTreeList.FocusedNodeChangedEventArgs) Handles uxtasks.FocusedNodeChanged
        Me.bdelete.Enabled = False
        Me.bmodify.Enabled = False
        Me.bhistory.Enabled = False


        If Me.uxtasks.Selection.Count = 1 Then
            Me.bdelete.Enabled = True
            Me.bmodify.Enabled = True
            Me.bhistory.Enabled = True
           



        End If
    End Sub
    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs)
        load_comment()
    End Sub

    Sub load_comment()
        Dim fc As New bc_dx_html_task_comment
        Dim cc As New Cbc_dx_html_task_comment
        If cc.load_data(fc, uxtasks.Selection(0).GetValue(7), uxtasks.Selection(0).GetValue(0)) = True Then
            fc.ShowDialog()
        End If
    End Sub


    Private Sub uxhist_DoubleClick(sender As Object, e As EventArgs) Handles uxtasks.DoubleClick
        If uxtasks.Selection.Count > 0 AndAlso uxtasks.Selection(0).GetValue(7) <> "" Then
            load_comment()
        End If
    End Sub


    Private Sub brefresh_Click(sender As Object, e As EventArgs) Handles brefresh.Click
        RaiseEvent refresh_view()

    End Sub

    Private Sub bc_dx_task_schedule_FormClosing(sender As Object, e As Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        t.Abort()


    End Sub

    Private Sub RepositoryItemComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RepositoryItemComboBox2.SelectedIndexChanged

        If Me.uxtasks.Selection(0).GetValue(8) = "True" Then
            RaiseEvent set_enable(Me.uxtasks.Selection(0).Tag, 0)
        Else
            RaiseEvent set_enable(Me.uxtasks.Selection(0).Tag, 1)
        End If
    End Sub

   

    Private Sub bnew_Click(sender As Object, e As EventArgs) Handles bnew.Click
        RaiseEvent new_task()
    End Sub

    Private Sub bmodify_Click(sender As Object, e As EventArgs) Handles bmodify.Click
       

        For i = 0 To _tasks.tasks.Count - 1
            If CStr(_tasks.tasks(i).task_id) = Me.uxtasks.Selection(0).Tag Then
                RaiseEvent maintain_task(_tasks.tasks(i))
                Exit For
            End If
        Next
    End Sub

    Private Sub bhistory_Click(sender As Object, e As EventArgs) Handles bhistory.Click
        For i = 0 To _tasks.tasks.Count - 1
            If CStr(_tasks.tasks(i).task_id) = Me.uxtasks.Selection(0).Tag Then
                Me.Cursor = Windows.Forms.Cursors.WaitCursor


                RaiseEvent load_history(_tasks.tasks(i).task_id, _tasks.tasks(i).task_name)
                Me.Cursor = Windows.Forms.Cursors.Default

                Exit For
            End If
        Next
    End Sub

    Private Sub bdelete_Click(sender As Object, e As EventArgs) Handles bdelete.Click
        For i = 0 To _tasks.tasks.Count - 1
            If CStr(_tasks.tasks(i).task_id) = Me.uxtasks.Selection(0).Tag Then
                Dim omsg As New bc_cs_message("Blue Curve", "Are you sure you want to delete task: " + _tasks.tasks(i).task_name, bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
                If omsg.cancel_selected = True Then
                    Exit Sub
                End If
                RaiseEvent delete_task(_tasks.tasks(i).task_id)
                Exit For
            End If
        Next
    End Sub

    
End Class
Public Class Cbc_dx_task_schedule
    WithEvents _view As Ibc_dx_task_schedule
    Dim _tasks As New bc_om_dl_tasks
    Function load_data(view As Ibc_dx_task_schedule) As Boolean
        load_data = False
        Try
            _view = view
            If get_data() = True Then
                load_data = _view.load_view(_tasks)
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Cbc_dx_task_schedule", "load_data", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try
    End Function

    Function get_data() As Boolean
        get_data = False

        Try
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                _tasks.db_read()
            Else
                _tasks.tmode = bc_cs_soap_base_class.tREAD
                If _tasks.transmit_to_server_and_receive(_tasks, True) = False Then
                    Exit Function
                End If
            End If
            get_data = True
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Cbc_dx_task_schedule", "get_data", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Function
    Sub refresh_view() Handles _view.refresh_view
        If get_data() = True Then
            _view.update_view(_tasks)
        End If
    End Sub
    Sub new_task() Handles _view.new_task
        Dim fmt As New bc_dx_maintain_task
        Dim cmt As New Cbc_dx_maintain_task
        If cmt.load_data(fmt, Nothing) = True Then
            fmt.ShowDialog()
        End If
        refresh_view()

    End Sub
    Sub maintain_task(task As bc_om_dl_tasks.bc_om_dl_task) Handles _view.maintain_task

        Dim fmt As New bc_dx_maintain_task
        Dim cmt As New Cbc_dx_maintain_task
        If cmt.load_data(fmt, task) = True Then
            fmt.ShowDialog()
        End If
        refresh_view()

    End Sub
    Sub load_history(task_id As Integer, task_name As String) Handles _view.load_history
        Dim fhi As New bc_dx_task_history
        Dim chi As New Cbc_dx_task_history
                If chi.load_data(fhi, task_id, task_name) = True Then
            fhi.ShowDialog()
        End If
        refresh_view()

    End Sub

    Sub delete_task(task_id As Integer) Handles _view.delete_task
        Dim task As New bc_om_dl_tasks.bc_om_dl_task
        task.task_id = task_id
        task.tmode = bc_cs_soap_base_class.tWRITE
        task.write_mode = bc_om_dl_tasks.bc_om_dl_task.Ewrite_mode.delete

        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
            task.db_write()
        Else
            If task.transmit_to_server_and_receive(task, True) = False Then
                Exit Sub
            End If
        End If
        refresh_view()
    End Sub
    Sub set_enable(task_id As Integer, enable As Integer) Handles _view.set_enable
        Dim ot As New bc_om_dl_tasks.bc_om_dl_task
        ot.task_id = task_id

        If enable = 1 Then
            ot.write_mode = bc_om_dl_tasks.bc_om_dl_task.Ewrite_mode.enable
        Else
            ot.write_mode = bc_om_dl_tasks.bc_om_dl_task.Ewrite_mode.disable
        End If
        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
            ot.db_write()
        Else
            ot.tmode = bc_cs_soap_base_class.tWRITE
            If ot.transmit_to_server_and_receive(ot, True) = False Then
                Exit Sub
            End If


        End If
        refresh_view()
    End Sub

End Class
Public Interface Ibc_dx_task_schedule
    Function load_view(tasks As bc_om_dl_tasks) As Boolean
    Function update_view(tasks As bc_om_dl_tasks) As Boolean
    Event refresh_view()
    Event set_enable(task_id As Integer, enable As Integer)
    Event new_task()
    Event maintain_task(task As bc_om_dl_tasks.bc_om_dl_task)
    Event load_history(task_id As Integer, task_name As String)
    Event delete_task(task_id As Integer)
End Interface
