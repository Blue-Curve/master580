Imports BlueCurve.Core.CS



<Serializable> Public Class bc_om_dl_tasks
    Inherits bc_cs_soap_base_class

    <Serializable> Public Class bc_om_dl_task_email
        Public email As String
        Public success As Boolean
        Public fail As Boolean
    End Class

    Public tasks As New List(Of bc_om_dl_task)
    Public Enum ETASK_TYPE
        INPUT = 1
        OUTPUT = 2
    End Enum
    Public Enum ETASK_STATUS
        NOT_YET_RUN = 0
        SERVICING = 1
        SUCCESS = 2
        FAIL = 3
    End Enum

    Public Overrides Sub process_object()
        Select Case tmode
            Case bc_cs_soap_base_class.tREAD
                db_read()

        End Select
    End Sub
    Public Sub db_read()
        Try
            Dim gdb As New bc_cs_db_services
            Dim res As Object
            res = gdb.executesql("exec dbo.bc_core_dl_view_get_tasks", certificate)
            Dim ot As New bc_om_dl_task
            tasks.Clear()
            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    ot = New bc_om_dl_task
                    ot.task_id = res(0, i)
                    ot.task_name = res(1, i)
                    ot.frequency_type = res(2, i)
                    ot.interval = res(3, i)
                    ot.start_time = res(4, i)
                    ot.last_run = res(5, i)
                    ot.next_run = res(6, i)
                    ot.disabled = res(7, i)
                    ot.status = res(8, i)
                    ot.comment = res(9, i)

                    ot.input = res(10, i)
                    ot.control_file_sp = res(11, i)
                    ot.filename_sp = res(12, i)
                    ot.channel_type_id = res(13, i)
                    ot.uri = res(14, i)
                    ot.username = res(15, i)
                    ot.password = res(16, i)
                    ot.fingerprint = res(17, i)
                    ot.port = res(18, i)
                    ot.staging_dir = res(19, i)
                    ot.ftp_dir = res(20, i)
                    ot.delete_from_ftp = res(21, i)
                    ot.archive = res(22, i)
                    ot.hour = res(23, i)
                    ot.minute = res(24, i)
                    ot.second = res(25, i)
                    ot.mon = res(26, i)
                    ot.tue = res(27, i)
                    ot.wed = res(28, i)
                    ot.thu = res(29, i)
                    ot.fri = res(30, i)
                    ot.sat = res(31, i)
                    ot.sun = res(32, i)
                    ot.current_datetime = res(33, i)
                    tasks.Add(ot)
                Next
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_dl_tasks", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub


    <Serializable> Public Class bc_om_dl_task
        Inherits bc_cs_soap_base_class
        Enum Ewrite_mode
            new_modify = 0
            enable = 1
            disable = 2
            delete = 3
        End Enum
        Public task_id As Integer
        Public task_name As String
        Public frequency_type As Integer
        Public interval As Integer
        Public start_time As DateTime
        Public last_run As DateTime
        Public next_run As DateTime
        Public disabled As Boolean
        Public status As ETASK_STATUS
        Public comment As String
        Public input As Boolean
        Public control_file_sp As String
        Public filename_sp As String
        Public channel_type_id As Integer
        Public uri As String
        Public username As String
        Public password As String
        Public fingerprint As String
        Public port As String
        Public staging_dir As String
        Public ftp_dir As String
        Public delete_from_ftp As Boolean
        Public archive As Boolean
        Public hour As Integer
        Public minute As Integer
        Public second As Integer
        Public mon As Boolean
        Public tue As Boolean
        Public wed As Boolean
        Public thu As Boolean
        Public fri As Boolean
        Public sat As Boolean
        Public sun As Boolean

        Public current_datetime As DateTime
        Public history As New List(Of bc_om_dl_task_history)
        Public email As New List(Of bc_om_dl_task_email)
        Public write_mode As Ewrite_mode

        Public Overrides Sub process_object()
            Select Case tmode
                Case bc_cs_soap_base_class.tREAD
                    db_read()
                Case bc_cs_soap_base_class.tWRITE
                    db_write()

            End Select
        End Sub
        Public Sub db_read()
            Try
                history.Clear()
                Dim gdb As New bc_cs_db_services
                Dim res As Object
                res = gdb.executesql("exec dbo.bc_core_dl_view_get_task_history " + CStr(task_id), certificate)
                Dim hi As bc_om_dl_task_history
                If IsArray(res) Then
                    For i = 0 To UBound(res, 2)
                        hi = New bc_om_dl_task_history
                        hi.run_date = res(0, i)
                        hi.complete_date = res(1, i)
                        hi.status = res(2, i)
                        hi.comment = res(3, i)
                        history.Add(hi)
                    Next
                End If
                email.Clear()
                res = gdb.executesql("exec dbo.bc_core_dl_view_get_task_emails " + CStr(task_id), certificate)
                Dim em As bc_om_dl_task_email
                If IsArray(res) Then
                    For i = 0 To UBound(res, 2)
                        em = New bc_om_dl_task_email
                        em.email = res(0, i)
                        em.success = res(1, i)
                        em.fail = res(2, i)
                        email.Add(em)
                    Next
                End If

            Catch ex As Exception
                Dim oerr As New bc_cs_error_log("bc_om_dl_task", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message)
            End Try
        End Sub
        Public Sub db_write()
            Try
                Dim gdb As New bc_cs_db_services

                Select Case write_mode
                    Case Ewrite_mode.disable
                        gdb.executesql("exec dbo.bc_core_dl_enable_disable_task " + CStr(task_id) + ",1", certificate)
                    Case Ewrite_mode.enable
                        gdb.executesql("exec dbo.bc_core_dl_enable_disable_task " + CStr(task_id) + ",0", certificate)
                    Case Ewrite_mode.new_modify
                        Dim fs As New bc_cs_string_services(task_name)
                        task_name = fs.delimit_apostrophies
                        Dim res As Object
                        res = gdb.executesql("exec dbo.bc_core_dl_maintain_task  " + CStr(task_id) + ",'" + task_name + "'," + CStr(frequency_type) + "," + CStr(interval) + "," + CStr(input) + ",'" + control_file_sp + "','" + filename_sp + "'," + CStr(channel_type_id) + ",'" + uri + "','" + username + "','" + password + "','" + fingerprint + "','" + port + "','" + staging_dir + "','" + ftp_dir + "'," + CStr(delete_from_ftp) + "," + CStr(archive) + "," + CStr(hour) + "," + CStr(minute) + "," + CStr(second) + "," + CStr(mon) + "," + CStr(tue) + "," + CStr(wed) + "," + CStr(thu) + "," + CStr(fri) + "," + CStr(sat) + "," + CStr(sun), certificate)
                        If UBound(res, 2) = 0 Then
                            task_id = res(0, 0)
                        End If
                        For i = 0 To email.Count - 1
                            gdb.executesql("exec dbo.bc_core_dl_add_task_email " + CStr(task_id) + ",'" + email(i).email + "'," + CStr(email(i).success) + "," + CStr(email(i).fail), certificate)
                        Next

                    Case Ewrite_mode.delete
                        gdb.executesql("exec dbo.bc_core_dl_delete_task  " + CStr(task_id), certificate)
                End Select

            Catch ex As Exception
                Dim oerr As New bc_cs_error_log("bc_om_dl_task", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message)
            End Try

        End Sub
    End Class
    <Serializable> Public Class bc_om_dl_task_history
        Public run_date As DateTime
        Public status As ETASK_STATUS
        Public comment As String
        Public complete_date As DateTime
    End Class
End Class

<Serializable> Public Class bc_om_dl_tasks_config_data
    Inherits bc_cs_soap_base_class
    Public channel_types As New List(Of bc_om_dl_task_channel_type)
    Public Overrides Sub process_object()
        Select Case tmode
            Case bc_cs_soap_base_class.tREAD
                db_read()
        End Select
    End Sub
    Public Sub db_read()
        Try
            Dim gdb As New bc_cs_db_services
            Dim res As Object
            Dim ct As bc_om_dl_task_channel_type
            res = gdb.executesql("exec dbo.bc_core_dl_view_get_channel_types", certificate)
            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    ct = New bc_om_dl_task_channel_type
                    ct.type_id = res(0, i)
                    ct.type_name = res(1, i)
                    ct.input = res(2, i)
                    ct.output = res(3, i)
                    ct.sp_only = res(4, i)
                    ct.req_channel_config = res(5, i)
                    ct.ftp = res(6, i)
                    channel_types.Add(ct)
                Next
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_dl_tasks_config_data", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    <Serializable> Public Class bc_om_dl_task_channel_type
        Public type_id As Integer
        Public type_name As String
        Public input As Boolean
        Public output As Boolean
        Public sp_only As Boolean
        Public req_channel_config As Boolean
        Public ftp As Boolean

    End Class

End Class


