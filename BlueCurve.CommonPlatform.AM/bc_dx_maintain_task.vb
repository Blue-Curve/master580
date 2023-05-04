Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Public Class bc_dx_maintain_task
    Implements Ibc_dx_maintain_task
    Dim _config As bc_om_dl_tasks_config_data
    Dim _task As bc_om_dl_tasks.bc_om_dl_task
    Event save(task As bc_om_dl_tasks.bc_om_dl_task) Implements Ibc_dx_maintain_task.save

    Public Function load_view(config As bc_om_dl_tasks_config_data, task As bc_om_dl_tasks.bc_om_dl_task) As Boolean Implements Ibc_dx_maintain_task.load_view
        load_view = False
        Try
            _config = config
            _task = task
           
           
            'Me.uxinttype.Properties.Items.Add("Week")
            Me.uxinttype.Properties.Items.Add("Day")
            Me.uxinttype.Properties.Items.Add("Hour")
            Me.uxinttype.Properties.Items.Add("Minute")
            Me.uxinttype.Properties.Items.Add("Second")
            For i = 0 To 23
                If i < 10 Then
                    Me.uxhour.Properties.Items.Add("0" + CStr(i))
                Else
                    Me.uxhour.Properties.Items.Add(CStr(i))
                End If
            Next
            For i = 0 To 59
                If i < 10 Then
                    Me.uxmin.Properties.Items.Add("0" + CStr(i))
                Else
                    Me.uxmin.Properties.Items.Add(CStr(i))
                End If
            Next
            For i = 0 To 59
                If i < 10 Then
                    Me.uxsec.Properties.Items.Add("0" + CStr(i))
                Else
                    Me.uxsec.Properties.Items.Add(CStr(i))
                End If
            Next
            If IsNothing(task) Then
                Me.Text = "Blue Curve - Add New Task"
                Me.bsave.Text = "Add"
                _task = New bc_om_dl_tasks.bc_om_dl_task
                _task.task_id = 0
            Else
                Me.Text = "Blue Curve - Update Task"
                Me.bsave.Text = "Update"
                uxname.Text = _task.task_name

                If _task.input = False Then
                    Me.uxio.SelectedIndex = 0
                Else
                    Me.uxio.SelectedIndex = 1
                End If
                For i = 0 To _config.channel_types.Count - 1
                    If _config.channel_types(i).type_id = _task.channel_type_id Then
                        Me.uxchanneltypes.SelectedIndex = i
                        Exit For
                    End If
                Next
                Me.uxcfp.Text = _task.control_file_sp
                Me.uxfnsp.Text = _task.filename_sp
                If _task.filename_sp <> "" Then
                    Me.chkfiles.CheckState = Windows.Forms.CheckState.Checked
                End If
                Me.tdir.Text = _task.staging_dir
                Me.turi.Text = _task.uri
                Me.tun.Text = _task.username
                Me.tpw.Text = _task.password
                Me.tfp.Text = _task.fingerprint
                Me.tport.Text = _task.port
                Me.tftpdir.Text = _task.ftp_dir

                If _task.delete_from_ftp = True Then
                    Me.chkdel.CheckState = Windows.Forms.CheckState.Checked
                End If
                If _task.archive = True Then
                    Me.chkarchive.CheckState = Windows.Forms.CheckState.Checked
                End If

                Me.uxinttype.SelectedIndex = _task.frequency_type - 1
                Me.uxint.SelectedIndex = _task.interval - 1
                Dim lda As New DateTime(99, 9, 9, _task.hour, 0, 0)
                Me.uxhour.SelectedIndex = lda.ToLocalTime.Hour


                Me.uxmin.SelectedIndex = _task.minute
                Me.uxsec.SelectedIndex = _task.second
                Me.CheckEdit1.CheckState = Windows.Forms.CheckState.Unchecked
                Me.CheckEdit2.CheckState = Windows.Forms.CheckState.Unchecked
                Me.CheckEdit3.CheckState = Windows.Forms.CheckState.Unchecked
                Me.CheckEdit4.CheckState = Windows.Forms.CheckState.Unchecked
                Me.CheckEdit5.CheckState = Windows.Forms.CheckState.Unchecked
                Me.CheckEdit6.CheckState = Windows.Forms.CheckState.Unchecked
                Me.CheckEdit7.CheckState = Windows.Forms.CheckState.Unchecked
                If _task.frequency_type = 1 Then
                    If _task.mon = True Then
                        Me.CheckEdit1.CheckState = Windows.Forms.CheckState.Checked
                    End If
                    If _task.tue = True Then
                        Me.CheckEdit2.CheckState = Windows.Forms.CheckState.Checked
                    End If
                    If _task.wed = True Then
                        Me.CheckEdit3.CheckState = Windows.Forms.CheckState.Checked
                    End If
                    If _task.thu = True Then
                        Me.CheckEdit4.CheckState = Windows.Forms.CheckState.Checked
                    End If
                    If _task.fri = True Then
                        Me.CheckEdit5.CheckState = Windows.Forms.CheckState.Checked
                    End If
                    If _task.sat = True Then
                        Me.CheckEdit6.CheckState = Windows.Forms.CheckState.Checked
                    End If
                    If _task.sun = True Then
                        Me.CheckEdit7.CheckState = Windows.Forms.CheckState.Checked
                    End If


                End If
                load_emails()

                End If


                load_view = True
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_dx_maintain_task", "load_view", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try
    End Function


    Private Sub bclose_Click(sender As Object, e As EventArgs) Handles bclose.Click
        Me.Hide()
    End Sub

    Private Sub LabelControl4_Click(sender As Object, e As EventArgs) Handles LabelControl4.Click

    End Sub

    Private Sub RadioGroup1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles uxio.SelectedIndexChanged
        Me.uxchanneltypes.Enabled = False

        uxio.Text = ""
        uxchanneltypes.SelectedIndex = -1


        If uxio.SelectedIndex = -1 Then
            Exit Sub
        End If
        Me.uxchanneltypes.Enabled = True
        Me.uxchanneltypes.Properties.Items.Clear()
        For i = 0 To _config.channel_types.Count - 1
            If uxio.SelectedIndex = 0 AndAlso _config.channel_types(i).output = True Then
                Me.uxchanneltypes.Properties.Items.Add(_config.channel_types(i).type_name)
            ElseIf uxio.SelectedIndex = 1 AndAlso _config.channel_types(i).input = True Then
                Me.uxchanneltypes.Properties.Items.Add(_config.channel_types(i).type_name)
            End If
        Next
    End Sub

    Private Sub uxchanneltypes_SelectedIndexChanged(sender As Object, e As EventArgs) Handles uxchanneltypes.SelectedIndexChanged
        Me.pout.Enabled = False
        uxcfp.Text = ""
        uxfnsp.Text = ""
        lfilesp.Enabled = False
        uxfnsp.Enabled = False
        Me.chkfiles.Enabled = False
        Me.chkfiles.CheckState = Windows.Forms.CheckState.Unchecked





        ldir.Enabled = False
        tdir.Enabled = False
        lruri.Enabled = False
        turi.Enabled = False
        Me.turi.Text = ""
        Lport.Enabled = False
        tport.Enabled = False
        Me.tport.Text = ""
        Me.tun.Text = ""
        lun.Enabled = False
        lpw.Enabled = False
        tun.Enabled = False
        Me.tun.Text = ""
        tpw.Enabled = False
        Me.tpw.Text = ""
        lftpdir.Enabled = False
        tftpdir.Enabled = False
        Me.tftpdir.Text = ""
        lfp.Enabled = False
        tfp.Enabled = False
        Me.tfp.Text = ""
        Me.chkarchive.Enabled = False
        Me.chkdel.Enabled = False
        Me.tdir.Text = ""
        Me.chkdel.CheckState = Windows.Forms.CheckState.Unchecked
        Me.chkarchive.CheckState = Windows.Forms.CheckState.Unchecked



        If Me.uxchanneltypes.SelectedIndex = -1 Then
            Exit Sub
        End If
      
        lcsp.Text = "Control File Sp"
        Me.pout.Enabled = True
        If _config.channel_types(uxchanneltypes.SelectedIndex).sp_only = True Then
            lcsp.Text = "Sp Name"
        ElseIf _config.channel_types(uxchanneltypes.SelectedIndex).ftp = True Or _config.channel_types(uxchanneltypes.SelectedIndex).req_channel_config = False Then
            lfilesp.Enabled = True
            uxfnsp.Enabled = True

            ldir.Enabled = True
            tdir.Enabled = True
            Me.chkfiles.Enabled = False
            Me.chkfiles.CheckState = Windows.Forms.CheckState.Checked
        Else
            Me.chkfiles.Enabled = True
            Me.chkfiles.CheckState = Windows.Forms.CheckState.Unchecked
        End If
        If uxio.SelectedIndex = 1 Then
            lcsp.Text = "Input Data Sp"
        End If
        If _config.channel_types(uxchanneltypes.SelectedIndex).req_channel_config = True Then
            lruri.Enabled = True
            turi.Enabled = True
        End If
        If _config.channel_types(uxchanneltypes.SelectedIndex).ftp = True Then
            Lport.Enabled = True
            tport.Enabled = True
            lun.Enabled = True
            lpw.Enabled = True
            tun.Enabled = True
            tpw.Enabled = True
            lftpdir.Enabled = True
            tftpdir.Enabled = True
            lfp.Enabled = True
            tfp.Enabled = True
            If uxio.SelectedIndex = 1 Then
                Me.chkarchive.Enabled = True
                Me.chkdel.Enabled = True
            End If

        End If

    End Sub

    Private Sub TextEdit4_EditValueChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub LabelControl8_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub PanelControl1_Paint(sender As Object, e As Windows.Forms.PaintEventArgs) Handles PanelControl1.Paint

    End Sub

    Private Sub XtraTabPage2_Paint(sender As Object, e As Windows.Forms.PaintEventArgs) Handles XtraTabPage2.Paint

    End Sub
    Sub set_desc()
        Try
            ldesc.Text = ""
            Me.bsave.Enabled = False

            If Me.uxinttype.SelectedIndex > -1 AndAlso Me.uxint.SelectedIndex > -1 Then
                If Me.uxinttype.SelectedIndex = 0 Then
                    ldesc.Text = "Run every day at " + Me.uxhour.Text + ":" + Me.uxmin.Text + ":" + Me.uxsec.Text + " on "
                    If Me.CheckEdit1.CheckState = Windows.Forms.CheckState.Checked Then
                        ldesc.Text = ldesc.Text + " monday "
                    End If
                    If Me.CheckEdit2.CheckState = Windows.Forms.CheckState.Checked Then
                        ldesc.Text = ldesc.Text + " tuesday "
                    End If
                    If Me.CheckEdit3.CheckState = Windows.Forms.CheckState.Checked Then
                        ldesc.Text = ldesc.Text + " wednesday "
                    End If
                    If Me.CheckEdit4.CheckState = Windows.Forms.CheckState.Checked Then
                        ldesc.Text = ldesc.Text + " thursday "
                    End If
                    If Me.CheckEdit5.CheckState = Windows.Forms.CheckState.Checked Then
                        ldesc.Text = ldesc.Text + " friday"
                    End If
                    If Me.CheckEdit6.CheckState = Windows.Forms.CheckState.Checked Then
                        ldesc.Text = ldesc.Text + " saturday"
                    End If
                    If Me.CheckEdit7.CheckState = Windows.Forms.CheckState.Checked Then
                        ldesc.Text = ldesc.Text + " sunday "
                    End If
                ElseIf Me.uxinttype.SelectedIndex = 1 Then
                    ldesc.Text = "Run every " + Me.uxint.Text + " hours(s) at " + Me.uxmin.Text + " minutes and  " + Me.uxsec.Text + " seconds " + " past the hour "
                ElseIf Me.uxinttype.SelectedIndex = 2 Then
                    ldesc.Text = "Run every " + Me.uxint.Text + " minutes(s) at " + Me.uxsec.Text + " seconds " + " past the minute "
                ElseIf Me.uxinttype.SelectedIndex = 3 Then
                    ldesc.Text = "Run every " + Me.uxint.Text + " seconds(s) "
                End If
                Me.bsave.Enabled = True
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_dx_maintain_task", "set_desc", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Private Sub uxinttype_SelectedIndexChanged(sender As Object, e As EventArgs) Handles uxinttype.SelectedIndexChanged
        Me.uxint.Enabled = False
        If Me.uxinttype.SelectedIndex = -1 Then
            Exit Sub
        End If
        Me.pdaysofweek.Enabled = False
        Me.ldaysofweek.Enabled = False
       


        Me.uxint.Enabled = True
        Me.lint.Text = Me.uxinttype.Text + "(s)"
        Dim co As Integer
        Me.uxhour.Enabled = False
        Me.uxmin.Enabled = False
        Me.uxsec.Enabled = False
        Me.lhour.Enabled = False
        Me.lmin.Enabled = False
        Me.lsec.Enabled = False

        Me.uxhour.SelectedIndex = -1
        Me.uxmin.SelectedIndex = -1
        Me.uxsec.SelectedIndex = -1



        If Me.uxinttype.Text = "Day" Then
            Me.uxhour.Enabled = True
            Me.uxmin.Enabled = True
            Me.uxsec.Enabled = True
            Me.lhour.Enabled = True
            Me.lmin.Enabled = True
            Me.lsec.Enabled = True
            Me.pdaysofweek.Enabled = True
            Me.ldaysofweek.Enabled = True
            co = 1
        End If
        If Me.uxinttype.Text = "Hour" Then
            Me.uxmin.Enabled = True
            Me.uxsec.Enabled = True
            Me.lmin.Enabled = True
            Me.lsec.Enabled = True
            co = 24
        End If
        If Me.uxinttype.Text = "Minute" Or Me.uxinttype.Text = "Second" Then
            co = 60
            If Me.uxinttype.Text = "Minute" Then
                Me.uxsec.Enabled = True
                Me.lsec.Enabled = True
            End If
        End If
        Me.uxint.Properties.Items.Clear()
        For i = 1 To co
            Me.uxint.Properties.Items.Add(CStr(i))
        Next
        Me.uxint.SelectedIndex = 0
        Me.uxhour.SelectedIndex = 0
        Me.uxmin.SelectedIndex = 0
        Me.uxsec.SelectedIndex = 0
        set_desc()
    End Sub
   
    
   

   
    Private Sub uxint_SelectedIndexChanged(sender As Object, e As EventArgs) Handles uxint.SelectedIndexChanged
        set_desc()
    End Sub

    Private Sub uxhour_SelectedIndexChanged(sender As Object, e As EventArgs) Handles uxhour.SelectedIndexChanged
        set_desc()
    End Sub

    Private Sub uxmin_SelectedIndexChanged(sender As Object, e As EventArgs) Handles uxmin.SelectedIndexChanged
        set_desc()
    End Sub

    Private Sub uxsec_SelectedIndexChanged(sender As Object, e As EventArgs) Handles uxsec.SelectedIndexChanged
        set_desc()
    End Sub

    Private Sub CheckEdit1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckEdit1.CheckedChanged
        set_desc()
    End Sub
    Private Sub CheckEdit2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckEdit2.CheckedChanged
        set_desc()
    End Sub
    Private Sub CheckEdit3_CheckedChanged(sender As Object, e As EventArgs) Handles CheckEdit3.CheckedChanged
        set_desc()
    End Sub
    Private Sub CheckEdit4_CheckedChanged(sender As Object, e As EventArgs) Handles CheckEdit4.CheckedChanged
        set_desc()
    End Sub
    Private Sub CheckEdit5_CheckedChanged(sender As Object, e As EventArgs) Handles CheckEdit5.CheckedChanged
        set_desc()
    End Sub
    Private Sub CheckEdit6_CheckedChanged(sender As Object, e As EventArgs) Handles CheckEdit6.CheckedChanged
        set_desc()
    End Sub
    Private Sub CheckEdit7_CheckedChanged(sender As Object, e As EventArgs) Handles CheckEdit7.CheckedChanged
        set_desc()
    End Sub

    Private Sub bsave_Click(sender As Object, e As EventArgs) Handles bsave.Click
        _task.task_name = uxname.Text
        If Me.uxio.SelectedIndex = 0 Then
            _task.input = False
        Else
            _task.input = True
        End If


        If Me.uxio.SelectedIndex = -1 Then
            Dim omsg As New bc_cs_message("Blue Curve", "input or output must be entered", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            Exit Sub
        End If

        If Me.uxchanneltypes.SelectedIndex = -1 Then
            Dim omsg As New bc_cs_message("Blue Curve", "Channel type must be entered", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            Exit Sub
        End If

        _task.channel_type_id = _config.channel_types(Me.uxchanneltypes.SelectedIndex).type_id


        If _task.task_name = "" Then
            Dim omsg As New bc_cs_message("Blue Curve", "Task name must be entered", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            Exit Sub
        End If
        _task.control_file_sp = Me.uxcfp.Text
        If _task.control_file_sp = "" And Me.uxcfp.Enabled = True Then
            Dim omsg As New bc_cs_message("Blue Curve", "control file SP name must be entered", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            Exit Sub
        End If

        _task.filename_sp = Me.uxfnsp.Text
        If _task.filename_sp = "" And Me.uxfnsp.Enabled = True Then
            Dim omsg As New bc_cs_message("Blue Curve", "filename SP name must be entered", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            Exit Sub
        End If
        _task.staging_dir = Me.tdir.Text
        If _task.staging_dir = "" And Me.tdir.Enabled = True Then
            Dim omsg As New bc_cs_message("Blue Curve", "Staging directory name must be entered", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            Exit Sub
        End If


        _task.uri = Me.turi.Text
        If _task.uri = "" And Me.turi.Enabled = True Then
            Dim omsg As New bc_cs_message("Blue Curve", "URI must be entered", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            Exit Sub
        End If
        _task.username = Me.tun.Text
        If _task.username = "" And Me.tun.Enabled = True Then
            Dim omsg As New bc_cs_message("Blue Curve", "Username must be entered", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            Exit Sub
        End If

        _task.password = Me.tpw.Text
        If _task.password = "" And Me.tpw.Enabled = True Then
            Dim omsg As New bc_cs_message("Blue Curve", "Password must be entered", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            Exit Sub
        End If

        _task.ftp_dir = Me.tftpdir.Text
        _task.fingerprint = Me.tfp.Text
        _task.port = Me.tport.Text



        REM scheduler
        _task.frequency_type = uxinttype.SelectedIndex + 1
        _task.interval = uxint.SelectedIndex + 1
        _task.hour = uxhour.SelectedIndex
        _task.minute = uxmin.SelectedIndex
        _task.second = uxsec.SelectedIndex
        Dim lda As New DateTime(99, 9, 9, _task.hour, 0, 0)
        _task.hour = lda.ToUniversalTime.Hour
        _task.mon = False
        _task.tue = False
        _task.wed = False
        _task.thu = False
        _task.fri = False
        _task.sat = False
        _task.sun = False
        If _task.frequency_type = 1 Then
            Dim day_found As Boolean = False
            If CheckEdit1.CheckState = Windows.Forms.CheckState.Checked Then
                _task.mon = True
                day_found = True
            End If
            If CheckEdit2.CheckState = Windows.Forms.CheckState.Checked Then
                _task.tue = True
                day_found = True
            End If
            If CheckEdit3.CheckState = Windows.Forms.CheckState.Checked Then
                _task.wed = True
                day_found = True
            End If
            If CheckEdit4.CheckState = Windows.Forms.CheckState.Checked Then
                _task.thu = True
                day_found = True
            End If
            If CheckEdit5.CheckState = Windows.Forms.CheckState.Checked Then
                _task.fri = True
                day_found = True
            End If
            If CheckEdit6.CheckState = Windows.Forms.CheckState.Checked Then
                _task.sat = True
                day_found = True
            End If
            If CheckEdit7.CheckState = Windows.Forms.CheckState.Checked Then
                _task.sun = True
                day_found = True
            End If
            If day_found = False Then
                Dim omsg As New bc_cs_message("Blue Curve", "A Daily frequency has been selected but no days have been checked", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Sub
            End If
        End If

        For i = 0 To _task.email.Count - 1
            _task.email(i).success = True
            _task.email(i).fail = True
            If Me.uxemails.Nodes(i).GetValue(1) = False Then
                _task.email(i).success = False
            End If
            If Me.uxemails.Nodes(i).GetValue(2) = False Then
                _task.email(i).fail = False
            End If
        Next
        _task.delete_from_ftp = False
        _task.archive = False
        If chkdel.Enabled = True And chkdel.CheckState = Windows.Forms.CheckState.Checked Then
            _task.delete_from_ftp = True
        End If
        If chkarchive.Enabled = True And chkarchive.CheckState = Windows.Forms.CheckState.Checked Then
            _task.archive = True
        End If

        RaiseEvent save(_task)
        Me.Hide()
    End Sub

   
    Private Sub load_emails()
        Try
            Me.uxemails.EndUpdate()
            Me.uxemails.Nodes.Clear()

            For i = 0 To _task.email.Count - 1
                Me.uxemails.Nodes.Add()
                Me.uxemails.Nodes(Me.uxemails.Nodes.Count - 1).SetValue(0, _task.email(i).email)
                If _task.email(i).success = True Then
                    Me.uxemails.Nodes(Me.uxemails.Nodes.Count - 1).SetValue(1, "True")
                Else
                    Me.uxemails.Nodes(Me.uxemails.Nodes.Count - 1).SetValue(1, "False")
                End If
                If _task.email(i).fail = True Then
                    Me.uxemails.Nodes(Me.uxemails.Nodes.Count - 1).SetValue(2, "True")
                Else
                    Me.uxemails.Nodes(Me.uxemails.Nodes.Count - 1).SetValue(2, "False")
                End If
                Me.uxemails.Nodes(Me.uxemails.Nodes.Count - 1).Tag = _task.email(i).email
            Next
        Catch ex As Exception

        Finally
            Me.uxemails.EndUpdate()


        End Try
    End Sub

    Private Sub temail_EditValueChanged(sender As Object, e As EventArgs) Handles temail.EditValueChanged
        Me.baddemail.Enabled = False
        If InStr(Me.temail.Text, "@") > 1 Then
            Me.baddemail.Enabled = True
        End If

    End Sub

    Private Sub baddemail_Click(sender As Object, e As EventArgs) Handles baddemail.Click
        Dim uem As New bc_om_dl_tasks.bc_om_dl_task_email
        uem.email = Me.temail.Text
        uem.success = True
        uem.fail = True
        _task.email.Add(uem)
        load_emails()
        Me.temail.Text = ""
    End Sub

    Private Sub uxemails_FocusedNodeChanged(sender As Object, e As DevExpress.XtraTreeList.FocusedNodeChangedEventArgs) Handles uxemails.FocusedNodeChanged
        If Me.uxemails.Selection.Count > 0 Then
            Me.brememail.Enabled = True
        End If
    End Sub

    Private Sub brememail_Click(sender As Object, e As EventArgs) Handles brememail.Click
        For i = 0 To _task.email.Count - 1
            If _task.email(i).email = Me.uxemails.Selection(0).GetValue(0) Then
                _task.email.RemoveAt(i)
                Me.brememail.Enabled = False
                load_emails()
                Exit For
            End If
        Next
    End Sub

    Private Sub chkfiles_CheckedChanged(sender As Object, e As EventArgs) Handles chkfiles.CheckedChanged
        
        If chkfiles.CheckState = Windows.Forms.CheckState.Checked Then
            Me.tdir.Enabled = True
            Me.ldir.Enabled = True
            Me.uxfnsp.Enabled = True
            Me.lfilesp.Enabled = True
        Else
            Me.tdir.Enabled = False
            Me.ldir.Enabled = False
            Me.uxfnsp.Enabled = False
            Me.lfilesp.Enabled = False
            Me.tdir.Text = ""
            Me.uxfnsp.Text = ""
        End If
    End Sub
End Class
Public Class Cbc_dx_maintain_task
    WithEvents _view As Ibc_dx_maintain_task
    Public Function load_data(view As Ibc_dx_maintain_task, task As bc_om_dl_tasks.bc_om_dl_task) As Boolean
        load_data = False
        Try
            _view = view
            Dim _config As New bc_om_dl_tasks_config_data
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                _config.db_read()
            Else
                _config.tmode = bc_cs_soap_base_class.tREAD
                If _config.transmit_to_server_and_receive(_config, True) = False Then
                    Exit Function
                End If
            End If
            If Not IsNothing(task) Then
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    task.db_read()
                Else
                    task.tmode = bc_cs_soap_base_class.tREAD
                    If task.transmit_to_server_and_receive(task, True) = False Then
                        Exit Function
                    End If
                End If
            End If
            load_data = _view.load_view(_config, task)
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Cbc_dx_maintain_task", "load_data", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Function
    Sub save(task As bc_om_dl_tasks.bc_om_dl_task) Handles _view.save
        Try
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                task.db_write()
            Else
                task.tmode = bc_cs_soap_base_class.tWRITE
                If task.transmit_to_server_and_receive(task, True) Then
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Cbc_dx_maintain_task", "save", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try
    End Sub

End Class
Public Interface Ibc_dx_maintain_task
    Function load_view(config As bc_om_dl_tasks_config_data, task As bc_om_dl_tasks.bc_om_dl_task) As Boolean
    Event save(task As bc_om_dl_tasks.bc_om_dl_task)
End Interface
