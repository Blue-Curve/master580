Imports BlueCurve.Core.CS

REM -----------------------------
REM  Service Classes
REM------------------------------


<Serializable()> Public Class bc_srs_schedule
    Inherits bc_cs_soap_base_class
    Public schedule As New ArrayList
    Public Const SENDEMAIL = 2
    Public write_mode As Long


    Public Overrides Sub process_object()
        Dim otrace As New bc_cs_activity_log("bc_srs_schedule", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try

            Select Case tmode
                Case bc_cs_soap_base_class.tREAD
                    db_read()
                Case bc_cs_soap_base_class.tWRITE
                    db_write()
            End Select

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_srs_schedule", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_srs_schedule", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub
    Public Sub db_read()
        Dim otrace As New bc_cs_activity_log("bc_srs_schedule", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try

            Dim i As Integer
            Dim vSRSSchedule As Object
            Dim oinput As bc_schedule_item
            Dim db_schedule As New bc_srs_schedule_db

            vSRSSchedule = db_schedule.ReadAllschedule(MyBase.certificate)

            If IsArray(vSRSSchedule) Then
                For i = 0 To UBound(vSRSSchedule, 2)
                    oinput = New bc_schedule_item(vSRSSchedule(0, i), vSRSSchedule(1, i), vSRSSchedule(2, i), vSRSSchedule(3, i), MyBase.certificate)

                    oinput.frequency = vSRSSchedule(4, i)
                    oinput.scheduletime = Format(vSRSSchedule(5, i), "dd-MMM-yyyy HH:mm:ss")
                    oinput.active = vSRSSchedule(6, i)
                    oinput.typeid = vSRSSchedule(7, i)
                    oinput.checkname = vSRSSchedule(8, i)
                    oinput.checkservice = vSRSSchedule(9, i)
                    oinput.calltext = vSRSSchedule(10, i)
                    oinput.storedproc = vSRSSchedule(11, i)
                    oinput.logtable = vSRSSchedule(12, i)
                    oinput.faultcheck = vSRSSchedule(13, i)
                    oinput.statuscheck = vSRSSchedule(14, i)
                    oinput.lastrundate = Format(vSRSSchedule(15, i), "ddMMMyyyy HH:mm:ss")
                    oinput.errorfound = vSRSSchedule(16, i)
                    oinput.url = vSRSSchedule(17, i)
                    oinput.conaccount = vSRSSchedule(18, i)
                    oinput.username = vSRSSchedule(19, i)
                    oinput.userpassword = vSRSSchedule(20, i)
                    oinput.errorseverity = 0
                    oinput.servername = vSRSSchedule(21, i)
                    schedule.Add(oinput)

                Next
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_srs_schedule", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_srs_schedule", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub

    Public Sub db_write()

        Dim otrace As New bc_cs_activity_log("bc_srs_schedule", "db_write", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try

            Dim vSRSSchedule As New bc_srs_schedule_db

            MyBase.certificate = certificate

            Select Case write_mode
                Case SENDEMAIL
                    vSRSSchedule.process_email(MyBase.certificate)

            End Select

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_srs_schedule", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_srs_schedule", "db_write", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Sub


End Class


<Serializable()> Public Class bc_schedule_item
    Inherits bc_cs_soap_base_class
    Public write_mode As Long
    Public scheduleid As Long
    Public eventid As Long
    Public jobid As Long
    Public clientid As Long
    Public item_name As String
    Public frequency As Long
    Public scheduletime As DateTime
    Public active As Integer
    Public typeid As Long
    Public checkname As String
    Public checkservice As String
    Public calltext As String
    Public storedproc As String
    Public logtable As String
    Public faultcheck As Integer
    Public statuscheck As Integer
    Public lastrundate As Date
    Public errorfound As Integer
    Public errortext As String
    Public url As String
    Public conaccount As String
    Public errorseverity As Integer
    Public username As String
    Public userpassword As String
    Public statusgood As Integer
    Public servername As String
    Public sourceitemid As String

    Public Const UPDATE = 1

    Public Sub New()

    End Sub

    Public Sub New(ByVal id As Integer, ByVal input_jobid As Integer, ByVal input_clientid As Integer, ByVal strinput_name As String, ByVal certificate As bc_cs_security.certificate)
        scheduleid = id
        jobid = input_jobid
        clientid = input_clientid
        item_name = strinput_name
    End Sub


    Public Overrides Sub process_object()
        Dim otrace As New bc_cs_activity_log("bc_schedule_item", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try

            Select Case tmode
                Case bc_cs_soap_base_class.tREAD
                    db_read()
                Case bc_cs_soap_base_class.tWRITE
                    db_write()
            End Select

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_schedule_item", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_schedule_item", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub


    Public Sub db_read()

        Dim otrace As New bc_cs_activity_log("bc_schedule_item", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try

            Dim vSRSChecks As Object
            Dim db_schedule As New bc_srs_schedule_db
            Dim sql As String

            Me.statusgood = 0
            sql = Me.storedproc + " '" + Me.lastrundate + "', " + Str(Me.scheduleid)

            vSRSChecks = db_schedule.ReadStatusCheck(Sql, MyBase.certificate)

            If IsArray(vSRSChecks) Then
                If vSRSChecks(0, 0) = 1 Then
                    Me.statusgood = 1
                End If
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_schedule_item", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_schedule_item", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub


    Public Sub db_write()

        Dim otrace As New bc_cs_activity_log("bc_schedule_item", "db_write", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try

            Dim vSRSScheduleitem As New bc_srs_schedule_db

            MyBase.certificate = certificate

            Select Case write_mode
                Case UPDATE

                    vSRSScheduleitem.write_item_results(Me.scheduleid, Me.jobid, Me.lastrundate, Me.errorfound, Me.errortext, Me.errorseverity, Me.sourceitemid, MyBase.certificate)

            End Select

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_schedule_item", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_schedule_item", "db_write", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Sub

End Class


Public Class bc_srs_schedule_db
    Private gbc_db As New bc_cs_db_services
    REM write a tempate record and retuns Id
    Public Sub New()

    End Sub

    REM reads all valid scheduled tasks 
    Public Function ReadAllschedule(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "bc_core_monitor_get_schedule"
        ReadAllschedule = gbc_db.executesql(sql, certificate)
    End Function

    REM Run status checks 
    Public Function ReadStatusCheck(sql As String, ByRef certificate As bc_cs_security.certificate) As Object
        ReadStatusCheck = gbc_db.executesql(sql, certificate)
    End Function



    Public Sub write_item_results(ByVal scheduleid As Long, ByVal jobid As Long, ByVal lastrun As Date, ByVal errorfound As Integer, ByVal errordesc As String, ByVal errorseverity As Integer, ByVal sourceitemid As String, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String

        Dim suser_id As String
        If bc_cs_central_settings.server_flag = 1 Then
            suser_id = certificate.user_id
        Else
            suser_id = bc_cs_central_settings.logged_on_user_id
        End If
        Dim str As New bc_cs_string_services(errordesc)
        errordesc = str.delimit_apostrophies

        sql = "exec dbo.bc_core_monitor_write_results " + CStr(scheduleid) + "," + CStr(jobid) + ",'" + Format(lastrun, "yyyyMMdd HH:mm:ss") + "'," + CStr(errorfound) + ",'" + errordesc + "'," + CStr(errorseverity) + ",'" + sourceitemid + "'"
        gbc_db.executesql(sql, certificate)

    End Sub

    Public Sub process_email(ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        Dim suser_id As String

        If bc_cs_central_settings.server_flag = 1 Then
            suser_id = certificate.user_id
        Else
            suser_id = bc_cs_central_settings.logged_on_user_id
        End If

        sql = "exec dbo.bc_core_monitor_send_emails"
        gbc_db.executesql(sql, certificate)

    End Sub

End Class


REM Restart class
<Serializable()> Public Class bc_srs_restarts
    Inherits bc_cs_soap_base_class
    Public restartlist As New ArrayList
    Public Const SENDEMAIL = 2
    Public write_mode As Long

    Public Overrides Sub process_object()
        Dim otrace As New bc_cs_activity_log("bc_srs_restarts", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            REM this is always specific to object
            If Me.tmode = bc_cs_soap_base_class.tREAD Then
                db_read()
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_srs_restarts", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_srs_restarts", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub

    Public Sub db_read()
        Dim otrace As New bc_cs_activity_log("bc_srs_restarts", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try

            Dim i As Integer
            Dim vSRSRestart As Object
            Dim oinput As bc_srs_restart_item
            Dim db_restart As New bc_srs_restart_db

            vSRSRestart = db_restart.ReadAllrestarts(MyBase.certificate)

            If IsArray(vSRSRestart) Then
                For i = 0 To UBound(vSRSRestart, 2)
                    oinput = New bc_srs_restart_item(vSRSRestart(0, i), vSRSRestart(1, i), vSRSRestart(2, i), vSRSRestart(3, i), MyBase.certificate)

                    oinput.frequency = vSRSRestart(4, i)
                    oinput.scheduletime = vSRSRestart(5, i)
                    oinput.active = vSRSRestart(6, i)
                    oinput.typeid = vSRSRestart(7, i)
                    oinput.checkname = vSRSRestart(8, i)
                    oinput.checkservice = vSRSRestart(9, i)
                    oinput.calltext = vSRSRestart(10, i)
                    oinput.restartproc = vSRSRestart(11, i)
                    oinput.lastrundate = Format(vSRSRestart(12, i), "dd-MMM-yyyy HH:mm:ss")
                    oinput.errorfound = vSRSRestart(13, i)
                    oinput.url = vSRSRestart(14, i)
                    oinput.conaccount = vSRSRestart(15, i)
                    oinput.username = vSRSRestart(16, i)
                    oinput.userpassword = vSRSRestart(17, i)
                    oinput.errorseverity = 0
                    oinput.servername = vSRSRestart(18, i)
                    oinput.restartattemps = vSRSRestart(19, i)
                    oinput.restartfrequency = vSRSRestart(20, i)
                    oinput.lastrestart = Format(vSRSRestart(21, i), "dd-MMM-yyyy HH:mm:ss")
                    oinput.restartcount = vSRSRestart(22, i)
                    oinput.restartresult = vSRSRestart(23, i)
                    restartlist.Add(oinput)

                Next
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_srs_restarts", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_srs_restarts", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub

    Public Sub db_write()

        Dim otrace As New bc_cs_activity_log("bc_srs_restarts", "db_write", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try

            Dim vSRSSchedule As New bc_srs_restart_db

            MyBase.certificate = certificate

            Select Case write_mode
                Case SENDEMAIL
                    vSRSSchedule.process_email(MyBase.certificate)
            End Select

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_srs_restarts", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_srs_restarts", "db_write", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub

End Class

<Serializable()> Public Class bc_srs_restart_item
    Inherits bc_cs_soap_base_class
    Public write_mode As Long
    Public scheduleid As Long
    Public eventid As Long
    Public jobid As Long
    Public clientid As Long
    Public item_name As String
    Public frequency As Long
    Public scheduletime As DateTime
    Public active As Integer
    Public typeid As Long
    Public checkname As String
    Public checkservice As String
    Public calltext As String
    Public restartproc As String
    Public lastrundate As Date
    Public errorfound As Integer
    Public errortext As String
    Public url As String
    Public conaccount As String
    Public errorseverity As Integer
    Public username As String
    Public userpassword As String
    Public statusgood As Integer
    Public servername As String
    Public restartattemps As Integer
    Public restartfrequency As Integer
    Public lastrestart As DateTime
    Public restartcount As Integer
    Public restartresult As String
    Public Const UPDATE = 1

    Public Sub New()

    End Sub
    Public Sub New(ByVal id As Integer, ByVal input_jobid As Integer, ByVal input_clientid As Integer, ByVal strinput_name As String, ByVal certificate As bc_cs_security.certificate)
        scheduleid = id
        jobid = input_jobid
        clientid = input_clientid
        item_name = strinput_name
    End Sub


    Public Overrides Sub process_object()
        Dim otrace As New bc_cs_activity_log("bc_srs_restart", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            REM this is always specific to object
            If Me.tmode = bc_cs_soap_base_class.tREAD Then
                db_read()
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_srs_restart", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_srs_restart", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub


    Public Sub db_read()

        Dim otrace As New bc_cs_activity_log("bc_srs_restart", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try

            Dim vSRSChecks As Object
            Dim db_schedule As New bc_srs_restart_db
            Dim sql As String

            Me.statusgood = 0
            sql = Me.restartproc + " '" + Me.lastrundate + "', " + Str(Me.scheduleid)

            vSRSChecks = db_schedule.ReadStatusCheck(sql, MyBase.certificate)

            If IsArray(vSRSChecks) Then
                If vSRSChecks(0, 0) = 1 Then
                    Me.statusgood = 1
                End If
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_srs_restart", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_srs_restart", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub


    Public Sub db_write()

        Dim otrace As New bc_cs_activity_log("bc_srs_restart", "db_write", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try

            Dim vSRSScheduleitem As New bc_srs_restart_db

            MyBase.certificate = certificate

            Select Case write_mode
                Case UPDATE
                    vSRSScheduleitem.write_item_restart_results(Me.scheduleid, Me.jobid, Me.lastrundate, Me.errorfound, Me.restartcount, Me.restartresult, MyBase.certificate)

            End Select

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_srs_restart", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_srs_restart", "db_write", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Sub

End Class


Public Class bc_srs_restart_db
    Private gbc_db As New bc_cs_db_services
    REM write a tempate record and retuns Id
    Public Sub New()

    End Sub

    REM reads all valid restart tasks 
    Public Function ReadAllrestarts(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "bc_core_monitor_get_restarts"
        ReadAllrestarts = gbc_db.executesql(sql, certificate)
    End Function

    REM Run status checks 
    Public Function ReadStatusCheck(sql As String, ByRef certificate As bc_cs_security.certificate) As Object
        ReadStatusCheck = gbc_db.executesql(sql, certificate)
    End Function

    Public Sub write_item_restart_results(ByVal scheduleid As Long, ByVal jobid As Long, ByVal lastrestart As Date, ByVal errorfound As Integer, ByVal restartcount As Integer, ByVal restartresult As String, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String

        Dim suser_id As String
        If bc_cs_central_settings.server_flag = 1 Then
            suser_id = certificate.user_id
        Else
            suser_id = bc_cs_central_settings.logged_on_user_id
        End If

        sql = "exec dbo.bc_core_monitor_write_restart_results " + CStr(scheduleid) + "," + CStr(jobid) + ",'" + Format(lastrestart, "yyyyMMdd HH:mm:ss") + "'," + CStr(errorfound) + "," + CStr(restartcount) + ",'" + restartresult + "'"
        gbc_db.executesql(sql, certificate)

    End Sub

    Public Sub process_email(ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        Dim suser_id As String

        If bc_cs_central_settings.server_flag = 1 Then
            suser_id = certificate.user_id
        Else
            suser_id = bc_cs_central_settings.logged_on_user_id
        End If

        sql = "exec dbo.bc_core_monitor_send_restart_emails"
        gbc_db.executesql(sql, certificate)

    End Sub

End Class





REM -----------------------------
REM  GUI Classes
REM------------------------------


REM clients
<Serializable()> Public Class bc_srs_clientlist
    Inherits bc_cs_soap_base_class
    Public clientlist As New ArrayList


    Public Overrides Sub process_object()
        Dim otrace As New bc_cs_activity_log("bc_srs_clientlist", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            REM this is always specific to object
            If Me.tmode = bc_cs_soap_base_class.tREAD Then
                db_read()
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_srs_clientlist", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_srs_clientlist", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub
    Public Sub db_read()
        Dim otrace As New bc_cs_activity_log("bc_srs_clientlist", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try

            Dim i As Integer
            Dim vClientList As Object
            Dim oinput As bc_srs_client
            Dim db_clientlist As New bc_srs_clients_db

            vClientList = db_clientlist.ReadAllClients(MyBase.certificate)

            If IsArray(vClientList) Then
                For i = 0 To UBound(vClientList, 2)
                    oinput = New bc_srs_client(vClientList(0, i), vClientList(1, i), vClientList(2, i), vClientList(3, i), MyBase.certificate)
                    oinput.conn_account = vClientList(4, i)
                    oinput.active = vClientList(5, i)
                    oinput.db_name = vClientList(6, i)
                    oinput.db_server = vClientList(7, i)
                    oinput.from_email = vClientList(8, i)
                    clientlist.Add(oinput)
                Next
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_srs_clientlist", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_srs_clientlist", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub

End Class

<Serializable()> Public Class bc_srs_client
    Inherits bc_cs_soap_base_class
    Public write_mode As Long
    Public clientid As Long
    Public client_name As String
    Public server_name As String
    Public url As String
    Public conn_account As String
    Public active As Integer
    Public db_name As String
    Public db_server As String
    Public dirtyrecord As Boolean = False
    Public from_email As String

    Public Const UPDATE = 1

    Public Sub New()

    End Sub
    Public Sub New(ByVal input_id As Integer, ByVal input_name As String, ByVal input_server As String, ByVal input_url As String, ByVal certificate As bc_cs_security.certificate)
        clientid = input_id
        client_name = input_name
        server_name = input_server
        url = input_url
    End Sub

    Public Overrides Sub process_object()
        Dim otrace As New bc_cs_activity_log("bc_srs_client", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "Entry", certificate)
        Try
            Select Case Me.tmode
                Case tWRITE
                    db_write()
            End Select
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_srs_client", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, MyBase.certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_srs_client", "process_object", bc_cs_activity_codes.TRACE_EXIT, "Exit", certificate)

        End Try
    End Sub

    Public Sub db_read()

    End Sub


    Public Sub db_write()

        Dim otrace As New bc_cs_activity_log("bc_srs_client", "db_write", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try

            Dim vSRSClient As New bc_srs_clients_db

            MyBase.certificate = certificate

            Select Case write_mode
                Case UPDATE

                    vSRSClient.write_client(Me.clientid, Me.client_name, Me.server_name, Me.db_name, Me.db_server, Me.conn_account, Me.url, Me.active, Me.from_email, MyBase.certificate)

            End Select

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_srs_client", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_srs_client", "db_write", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Sub

End Class


Public Class bc_srs_clients_db
    Private gbc_db As New bc_cs_db_services
    REM write a tempate record and retuns Id
    Public Sub New()

    End Sub

    REM reads all clients
    Public Function ReadAllClients(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "bc_core_monitor_get_clients"
        ReadAllClients = gbc_db.executesql(sql, certificate)
    End Function

    Public Sub write_client(ByVal clientid As Long, ByVal clientname As String, ByVal servername As String, ByVal dbname As String, ByVal dbserver As String, ByVal conacount As String, ByVal url As String, ByVal active As Integer, ByVal from_email As String, ByRef certificate As bc_cs_security.certificate)

        Dim sql As String

        Dim suser_id As String
        If bc_cs_central_settings.server_flag = 1 Then
            suser_id = certificate.user_id
        Else
            suser_id = bc_cs_central_settings.logged_on_user_id
        End If
        sql = "exec dbo.bc_core_monitor_write_client " + CStr(clientid) + ",'" + clientname + "','" + servername + "','" + dbname + "','" + dbserver + "','" + conacount + "','" + url + "'," + CStr(active) + ",'" + from_email + "'"
        gbc_db.executesql(sql, certificate)

    End Sub

End Class



REM jobs --------------------------
<Serializable()> Public Class bc_srs_joblist
    Inherits bc_cs_soap_base_class
    Public joblist As New ArrayList


    Public Overrides Sub process_object()
        Dim otrace As New bc_cs_activity_log("bc_srs_joblist", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            REM this is always specific to object
            If Me.tmode = bc_cs_soap_base_class.tREAD Then
                db_read()
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_srs_joblist", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_srs_joblist", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub
    Public Sub db_read()
        Dim otrace As New bc_cs_activity_log("bc_srs_joblist", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try

            Dim i As Integer
            Dim vJobList As Object
            Dim oinput As bc_srs_job
            Dim db_joblist As New bc_srs_jobs_db

            vJobList = db_joblist.ReadAllJobs(MyBase.certificate)

            If IsArray(vJobList) Then
                For i = 0 To UBound(vJobList, 2)
                    oinput = New bc_srs_job(vJobList(0, i), vJobList(1, i), vJobList(2, i), vJobList(3, i), MyBase.certificate)

                    oinput.service = vJobList(4, i)
                    oinput.calltext = vJobList(5, i)
                    oinput.storedproc = vJobList(6, i)
                    oinput.logtable = vJobList(7, i)
                    oinput.faultcheck = vJobList(8, i)
                    oinput.statuscheck = vJobList(9, i)
                    oinput.checkstate = vJobList(10, i)
                    oinput.statemaxtime = vJobList(11, i)
                    oinput.errortext = vJobList(12, i)
                    oinput.errorseverity = vJobList(13, i)
                    oinput.type_name = vJobList(14, i)
                    oinput.username = vJobList(15, i)
                    oinput.userpassword = vJobList(16, i)
                    oinput.restartproc = vJobList(17, i)
                    joblist.Add(oinput)

                Next
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_srs_joblist", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_srs_joblist", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub

End Class


<Serializable()> Public Class bc_srs_job
    Inherits bc_cs_soap_base_class
    Public write_mode As Long

    Public jobid As Long
    Public typeid As Long
    Public checkname As String
    Public checkdecscription As String
    Public service As String
    Public calltext As String
    Public storedproc As String
    Public logtable As String
    Public faultcheck As Integer
    Public statuscheck As Integer
    Public checkstate As String
    Public statemaxtime As Long
    Public errortext As String
    Public errorseverity As Integer
    Public type_name As String
    Public username As String
    Public userpassword As String
    Public restartproc As String
    Public dirtyrecord As Boolean = False

    Public Const UPDATE = 1

    Public Sub New()

    End Sub
    Public Sub New(ByVal input_id As Integer, ByVal input_typeid As Integer, ByVal input_checkname As String, input_decscription As String, ByVal certificate As bc_cs_security.certificate)

        jobid = input_id
        typeid = input_typeid
        checkname = input_checkname
        checkdecscription = input_decscription

    End Sub

    Public Overrides Sub process_object()
        Dim otrace As New bc_cs_activity_log("bc_srs_job", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Select Case Me.tmode
                Case tWRITE
                    db_write()
            End Select
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_srs_job", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, MyBase.certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_srs_job", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)

        End Try
    End Sub

    Public Sub db_read()

    End Sub


    Public Sub db_write()

        Dim otrace As New bc_cs_activity_log("bc_srs_job", "db_write", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try

            Dim vSRSJob As New bc_srs_jobs_db
            MyBase.certificate = certificate

            Select Case write_mode
                Case UPDATE
                    vSRSJob.write_job(Me.jobid, Me.typeid, Me.checkname, Me.checkdecscription, Me.service, Me.calltext, Me.storedproc, Me.logtable, Me.faultcheck, Me.statuscheck, Me.checkstate, Me.statemaxtime, Me.errortext, Me.errorseverity, Me.type_name, Me.username, Me.userpassword, Me.restartproc, MyBase.certificate)
            End Select

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_srs_job", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_srs_job", "db_write", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Sub

End Class


Public Class bc_srs_jobs_db
    Private gbc_db As New bc_cs_db_services
    REM write a tempate record and retuns Id
    Public Sub New()

    End Sub

    REM reads all jobs
    Public Function ReadAllJobs(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "bc_core_monitor_get_jobs"
        ReadAllJobs = gbc_db.executesql(sql, certificate)
    End Function

    Public Sub write_job(ByVal jobid As Long, ByVal typeid As Long, ByVal checkname As String, ByVal checkdecscription As String, ByVal service As String, ByVal calltext As String, _
     ByVal storedproc As String, ByVal logtable As String, ByVal faultcheck As String, ByVal statuscheck As String, ByVal checkstate As String, ByVal statemaxtime As String, ByVal errortext As String, _
     ByVal errorseverity As String, ByVal type_name As String, ByVal username As String, ByVal userpassword As String, ByVal restartproc As String, ByRef certificate As bc_cs_security.certificate)

        Dim sql As String

        Dim suser_id As String
        If bc_cs_central_settings.server_flag = 1 Then
            suser_id = certificate.user_id
        Else
            suser_id = bc_cs_central_settings.logged_on_user_id
        End If

        sql = "exec dbo.bc_core_monitor_write_job " + CStr(jobid) + "," + CStr(typeid) + ",'" + checkname + "','" + checkdecscription + "'"
        sql = sql + ",'" + service + "'"
        sql = sql + ",'" + calltext + "'"
        sql = sql + ",'" + storedproc + "'"
        sql = sql + ",'" + logtable + "'"
        sql = sql + "," + faultcheck
        sql = sql + "," + statuscheck
        sql = sql + ",'" + checkstate + "'"
        sql = sql + "," + statemaxtime
        sql = sql + ",'" + errortext + "'"
        sql = sql + "," + errorseverity
        sql = sql + ",'" + username + "'"
        sql = sql + ",'" + userpassword + "'"
        sql = sql + ",'" + restartproc + "'"

        gbc_db.executesql(sql, certificate)

    End Sub

End Class


REM stats
<Serializable()> Public Class bc_srs_statlist
    Inherits bc_cs_soap_base_class
    Public statlist As New ArrayList

    Public Overrides Sub process_object()
        Dim otrace As New bc_cs_activity_log("bc_srs_statlist", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            REM this is always specific to object
            If Me.tmode = bc_cs_soap_base_class.tREAD Then
                db_read()
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_srs_statlist", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_srs_statlist", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub
    Public Sub db_read()
        Dim otrace As New bc_cs_activity_log("bc_srs_statlist", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try

            Dim i As Integer
            Dim vstatList As Object
            Dim oinput As bc_srs_stat
            Dim db_statlist As New bc_srs_stats_db

            vstatList = db_statlist.ReadAllStats(MyBase.certificate)

            If IsArray(vstatList) Then
                For i = 0 To UBound(vstatList, 2)
                    oinput = New bc_srs_stat(vstatList(0, i), vstatList(1, i), vstatList(2, i), vstatList(3, i), MyBase.certificate)
                    oinput.errorsfound = vstatList(4, i)
                    oinput.lasterrorid = vstatList(5, i)
                    oinput.lasterrordate = Format(vstatList(6, i), "dd-MMM-yyyy HH:mm:ss")
                    oinput.laststate = vstatList(7, i)
                    oinput.laststatechange = Format(vstatList(8, i), "dd-MMM-yyyy HH:mm:ss")
                    oinput.clientid = vstatList(9, i)
                    oinput.lastrestart = Format(vstatList(10, i), "dd-MMM-yyyy HH:mm:ss")
                    oinput.restartcount = vstatList(11, i)
                    oinput.restartresult = vstatList(12, i)
                    statlist.Add(oinput)
                Next
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_srs_statlist", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_srs_statlist", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub

End Class


<Serializable()> Public Class bc_srs_stat
    Inherits bc_cs_soap_base_class
    Public write_mode As Long
    Public stateid As Long
    Public scheduleid As Long
    Public itemname As String
    Public lastrundate As Date
    Public errorsfound As Integer
    Public lasterrorid As Long
    Public lasterrordate As Date
    Public laststate As String
    Public laststatechange As Date
    Public clientid As Long
    Public lastrestart As Date
    Public restartcount As Integer
    Public restartresult As String

    Public Const UPDATE = 1

    Public Sub New()

    End Sub


    Public Sub New(ByVal input_id As Integer, ByVal input_scheduleid As Integer, ByVal input_itemname As String, input_lastrundate As Date, ByVal certificate As bc_cs_security.certificate)

        stateid = input_id
        scheduleid = input_scheduleid
        itemname = input_itemname
        lastrundate = input_lastrundate

    End Sub


    Public Overrides Sub process_object()
        Dim otrace As New bc_cs_activity_log(" bc_srs_stat", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            REM this is always specific to object
            If Me.tmode = bc_cs_soap_base_class.tREAD Then
                db_read()
            End If

            Select Case Me.tmode
                Case tWRITE
                    db_write()
            End Select


        Catch ex As Exception
            Dim db_err As New bc_cs_error_log(" bc_srs_stat", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log(" bc_srs_stat", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub

    Public Sub db_read()

    End Sub


    Public Sub db_write()

        Dim otrace As New bc_cs_activity_log("bc_srs_stat", "db_write", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try

            Dim vSRSStats As New bc_srs_stats_db
            MyBase.certificate = certificate

            Select Case write_mode
                Case UPDATE
                    vSRSStats.write_state(Me.stateid, Me.scheduleid, Me.errorsfound, Me.restartcount, MyBase.certificate)

            End Select

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_srs_stat", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_srs_stat", "db_write", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Sub

End Class


Public Class bc_srs_stats_db
    Private gbc_db As New bc_cs_db_services
    REM write a tempate record and retuns Id
    Public Sub New()

    End Sub

    REM reads all stats
    Public Function ReadAllStats(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "bc_core_monitor_get_stats"
        ReadAllStats = gbc_db.executesql(sql, certificate)
    End Function

    Public Sub write_state(ByVal stateid As Long, ByVal scheduleid As Long, ByVal errorfound As Integer, ByVal restartcount As Integer, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String

        Dim suser_id As String
        If bc_cs_central_settings.server_flag = 1 Then
            suser_id = certificate.user_id
        Else
            suser_id = bc_cs_central_settings.logged_on_user_id
        End If
        sql = "exec dbo.bc_core_monitor_write_states " + CStr(stateid) + "," + CStr(scheduleid) + "," + CStr(errorfound) + "," + CStr(restartcount)
        gbc_db.executesql(sql, certificate)

    End Sub

End Class


REM Errors
<Serializable()> Public Class bc_srs_errorlist
    Inherits bc_cs_soap_base_class
    Public errorlist As New ArrayList
    Public scheduleid As Long

    Public Overrides Sub process_object()
        Dim otrace As New bc_cs_activity_log("bc_srs_errorlist", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            REM this is always specific to object
            If Me.tmode = bc_cs_soap_base_class.tREAD Then
                db_read()
            End If
            If Me.tmode = 4 Then
                db_read_open()
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_srs_errorlist", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_srs_errorlist", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub
    Public Sub db_read()
        Dim otrace As New bc_cs_activity_log("bc_srs_errorlist", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try

            Dim i As Integer
            Dim verrorsList As Object
            Dim oinput As bc_srs_error
            Dim db_errorslist As New bc_srs_Errors_db

            verrorsList = db_errorslist.ReadAllErrors(MyBase.certificate)

            If IsArray(verrorsList) Then
                For i = 0 To UBound(verrorsList, 2)
                    oinput = New bc_srs_error(verrorsList(0, i), verrorsList(1, i), verrorsList(2, i), verrorsList(3, i), MyBase.certificate)

                    oinput.checkname = verrorsList(4, i)
                    oinput.scheduleid = verrorsList(5, i)
                    oinput.itemname = verrorsList(6, i)
                    oinput.errordescription = verrorsList(7, i)
                    oinput.emailsent = verrorsList(8, i)
                    oinput.severity = verrorsList(9, i)
                    oinput.errordate = Format(verrorsList(10, i), "dd-MMM-yyyy HH:mm:ss")
                    errorlist.Add(oinput)

                Next
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_srs_errorlist", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_srs_errorlist", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub

    Public Sub db_read_open()
        Dim otrace As New bc_cs_activity_log("bc_srs_errorlist", "db_read_open", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try

            Dim i As Integer
            Dim verrorsList As Object
            Dim oinput As bc_srs_error
            Dim db_errorslist As New bc_srs_Errors_db

            verrorsList = db_errorslist.ReadOpenErrors(Me.scheduleid, MyBase.certificate)

            If IsArray(verrorsList) Then
                For i = 0 To UBound(verrorsList, 2)
                    oinput = New bc_srs_error(verrorsList(0, i), verrorsList(1, i), verrorsList(2, i), verrorsList(3, i), MyBase.certificate)
                    oinput.checkname = verrorsList(4, i)
                    oinput.scheduleid = verrorsList(5, i)
                    oinput.itemname = verrorsList(6, i)
                    oinput.errordescription = verrorsList(7, i)
                    oinput.emailsent = verrorsList(8, i)
                    oinput.severity = verrorsList(9, i)
                    oinput.errordate = Format(verrorsList(10, i), "dd-MMM-yyyy HH:mm:ss")
                    oinput.sourceitemid = verrorsList(11, i)
                    errorlist.Add(oinput)
                Next
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_srs_errorlist", "db_read_open", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_srs_errorlist", "db_read_open", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub

End Class


<Serializable()> Public Class bc_srs_error
    Inherits bc_cs_soap_base_class
    Public write_mode As Long
    Public errorid As Long
    Public clientid As Long
    Public client_name As String
    Public jobid As Long
    Public checkname As String
    Public scheduleid As Long
    Public itemname As String
    Public errordescription As String
    Public emailsent As Integer
    Public severity As Integer
    Public errordate As Date
    Public sourceitemid As String

    Public Const UPDATE = 1

    Public Sub New()

    End Sub
    Public Sub New(ByVal input_id As Integer, ByVal input_clientid As Integer, ByVal input_client_name As String, jobid As Integer, ByVal certificate As bc_cs_security.certificate)

        errorid = input_id
        clientid = input_clientid
        client_name = input_client_name
        jobid = jobid

    End Sub

    Public Sub db_read()

    End Sub

End Class

Public Class bc_srs_Errors_db
    Private gbc_db As New bc_cs_db_services
    REM write a tempate record and retuns Id
    Public Sub New()

    End Sub

    REM reads all errors
    Public Function ReadAllErrors(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "bc_core_monitor_get_Errors"
        ReadAllErrors = gbc_db.executesql(sql, certificate)
    End Function

    REM reads open errors for one scheduleid
    Public Function ReadOpenErrors(ByRef scheduleid As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "bc_core_monitor_get_OpenErrors " + CStr(scheduleid)
        ReadOpenErrors = gbc_db.executesql(sql, certificate)
    End Function

End Class



REM action  (schedulelist Display/Edit)
<Serializable()> Public Class bc_srs_actionlist
    Inherits bc_cs_soap_base_class
    Public actionlist As New ArrayList


    Public Overrides Sub process_object()
        Dim otrace As New bc_cs_activity_log("bc_srs_actions", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            REM this is always specific to object
            If Me.tmode = bc_cs_soap_base_class.tREAD Then
                db_read()
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_srs_actions", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_srs_actions", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub
    Public Sub db_read()
        Dim otrace As New bc_cs_activity_log("bc_srs_actions", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try

            Dim i As Integer
            Dim vSRSAction As Object
            Dim oinput As bc_action_item
            Dim db_action As New bc_srs_action_db

            vSRSAction = db_action.ReadAllactions(MyBase.certificate)

            If IsArray(vSRSAction) Then
                For i = 0 To UBound(vSRSAction, 2)
                    oinput = New bc_action_item(vSRSAction(0, i), vSRSAction(1, i), vSRSAction(2, i), vSRSAction(3, i), MyBase.certificate)

                    oinput.frequency = vSRSAction(4, i)
                    oinput.scheduletime = Format(vSRSAction(5, i), "dd-MMM-yyyy HH:mm:ss")
                    oinput.active = vSRSAction(6, i)
                    oinput.typeid = vSRSAction(7, i)
                    oinput.checkname = vSRSAction(8, i)
                    oinput.checkservice = vSRSAction(9, i)
                    oinput.calltext = vSRSAction(10, i)
                    oinput.storedproc = vSRSAction(11, i)
                    oinput.logtable = vSRSAction(12, i)
                    oinput.faultcheck = vSRSAction(13, i)
                    oinput.statuscheck = vSRSAction(14, i)
                    oinput.lastrundate = Format(vSRSAction(15, i), "dd-MMM-yyyy HH:mm:ss")
                    oinput.errorfound = vSRSAction(16, i)
                    oinput.monitorstarttime = Format(vSRSAction(17, i), "dd-MMM-yyyy HH:mm:ss")
                    oinput.monitorendtime = Format(vSRSAction(18, i), "dd-MMM-yyyy HH:mm:ss")
                    oinput.monitormonday = vSRSAction(19, i)
                    oinput.monitortuesday = vSRSAction(20, i)
                    oinput.monitorwednesday = vSRSAction(21, i)
                    oinput.monitorthursday = vSRSAction(22, i)
                    oinput.monitorfriday = vSRSAction(23, i)
                    oinput.monitorsaturday = vSRSAction(24, i)
                    oinput.monitorsunday = vSRSAction(25, i)
                    oinput.client_name = vSRSAction(26, i)
                    oinput.errorfrequency = vSRSAction(27, i)
                    oinput.restartattemps = vSRSAction(28, i)
                    oinput.restartfrequency = vSRSAction(29, i)
                    actionlist.Add(oinput)

                Next
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_srs_actions", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_srs_actions", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub

End Class


<Serializable()> Public Class bc_action_item
    Inherits bc_cs_soap_base_class
    Public write_mode As Long
    Public scheduleid As Long
    Public eventid As Long
    Public jobid As Long
    Public clientid As Long
    Public item_name As String
    Public frequency As Long
    Public scheduletime As Date
    Public active As Integer
    Public typeid As Long
    Public checkname As String
    Public checkservice As String
    Public calltext As String
    Public storedproc As String
    Public logtable As String
    Public faultcheck As Integer
    Public statuscheck As Integer
    Public lastrundate As Date
    Public errorfound As Integer
    Public monitorstarttime As Date
    Public monitorendtime As Date
    Public monitormonday As Integer
    Public monitortuesday As Integer
    Public monitorwednesday As Integer
    Public monitorthursday As Integer
    Public monitorfriday As Integer
    Public monitorsaturday As Integer
    Public monitorsunday As Integer
    Public client_name As String
    Public errorfrequency As Integer
    Public restartattemps As Integer
    Public restartfrequency As Integer

    Public dirtyrecord As Boolean = False
    Public Const UPDATE = 1
    Public Const DELETE = 2
    Public Sub New()

    End Sub
    Public Sub New(ByVal id As Integer, ByVal input_jobid As Integer, ByVal input_clientid As Integer, ByVal strinput_name As String, ByVal certificate As bc_cs_security.certificate)
        scheduleid = id
        jobid = input_jobid
        clientid = input_clientid
        item_name = strinput_name
    End Sub


    Public Overrides Sub process_object()
        Dim otrace As New bc_cs_activity_log("bc_action_item", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Select Case Me.tmode
                Case bc_cs_soap_base_class.tWRITE
                    db_write()
                Case bc_cs_soap_base_class.tREAD
                    db_read()
            End Select
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_action_item", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_action_item", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)

        End Try
    End Sub

    Public Sub db_read()

    End Sub


    Public Sub db_write()
        Dim otrace As New bc_cs_activity_log("bc_action_item", "db_write", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try

            Dim dbAction As New bc_srs_action_db

            MyBase.certificate = certificate

            Select Case write_mode
                Case UPDATE

                    dbAction.write_action(Me.scheduleid, Me.jobid, Me.clientid, Me.item_name, Me.frequency, Me.scheduletime, Me.active, Me.typeid, Me.checkname, Me.checkservice, Me.calltext, Me.storedproc, Me.logtable, Me.faultcheck, Me.statuscheck, Me.lastrundate, Me.errorfound, Me.monitorstarttime, Me.monitorendtime, Me.monitormonday, Me.monitortuesday, Me.monitorwednesday, Me.monitorthursday, Me.monitorfriday, Me.monitorsaturday, Me.monitorsunday, Me.client_name, Me.errorfrequency, Me.restartattemps, Me.restartfrequency, MyBase.certificate)

                Case DELETE

                    dbAction.delete_action(Me.scheduleid, Me.jobid, Me.clientid, MyBase.certificate)

                    'Me.RowsDeleted = ""
                    'vDeleteRows = dbAction.delete_time_series(Me.ActionId, Me.ActionClassId, Me.ActionEntityId, entityDesc, Me.ActionEventId, Me.DeleteClassId, Me.DeleteFrom, Me.DeleteTo, MyBase.certificate)
                    'Me.RowsDeleted = vDeleteRows(0, 0)

            End Select

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_action_item", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_action_item", "db_write", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Sub


End Class


Public Class bc_srs_action_db
    Private gbc_db As New bc_cs_db_services
    REM write a tempate record and retuns Id
    Public Sub New()

    End Sub

    REM reads all valid scheduled tasks 
    Public Function ReadAllactions(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "bc_core_monitor_get_actions"

        ReadAllactions = gbc_db.executesql(sql, certificate)
    End Function

    REM update scheduled item
    Public Sub write_action(ByVal scheduleid As Long, ByVal jobid As Long, ByVal clientid As Long, ByVal item_name As String, ByVal frequency As Long, ByVal scheduletime As Date, ByVal active As Integer, ByVal typeid As Long, ByVal checkname As String, ByVal checkservice As String, ByVal calltext As String, ByVal storedproc As String, ByVal logtable As String, ByVal faultcheck As Integer, ByVal statuscheck As Integer, ByVal lastrundate As Date, ByVal errorfound As Integer, ByVal monitorstarttime As Date, ByVal monitorendtime As Date, ByVal monitormonday As Integer, ByVal monitortuesday As Integer, ByVal monitorwednesday As Integer, ByVal monitorthursday As Integer, ByVal monitorfriday As Integer, ByVal monitorsaturday As Integer, ByVal monitorsunday As Integer, ByVal client_name As String, ByVal errorfrequency As Integer, ByVal restartattemps As Integer, ByVal restartfrequency As Integer, certificate As bc_cs_security.certificate)
        Dim sql As String

        sql = "[bc_core_monitor_write_action] " + CStr(scheduleid) + ","
        sql = sql + CStr(jobid) + ","
        sql = sql + CStr(clientid) + ","
        sql = sql + "'" + CStr(item_name) + "',"
        sql = sql + CStr(frequency) + ","

        If scheduletime.ToString = "01/01/1900 00:00:00" Then
            sql = sql + "null,"
        Else
            sql = sql + "'" + Format(scheduletime, "ddMMMyyyy HH:mm:ss") + "',"
        End If

        sql = sql + CStr(active) + ","
        sql = sql + CStr(typeid) + ","
        sql = sql + "'" + CStr(checkname) + "',"
        sql = sql + "'" + CStr(checkservice) + "',"

        If monitorstarttime.ToString = "01/01/1900 00:00:00" Then
            sql = sql + "null,"
        Else
            sql = sql + "'" + Format(monitorstarttime, "ddMMMyyyy HH:mm:ss") + "',"
        End If

        If monitorendtime.ToString = "01/01/1900 00:00:00" Then
            sql = sql + "null,"
        Else
            sql = sql + "'" + Format(monitorendtime, "ddMMMyyyy HH:mm:ss") + "',"
        End If

        sql = sql + CStr(monitormonday) + ","
        sql = sql + CStr(monitortuesday) + ","
        sql = sql + CStr(monitorwednesday) + ","
        sql = sql + CStr(monitorthursday) + ","
        sql = sql + CStr(monitorfriday) + ","
        sql = sql + CStr(monitorsaturday) + ","
        sql = sql + CStr(monitorsunday) + ","
        sql = sql + CStr(errorfrequency) + ","
        sql = sql + CStr(restartattemps) + ","
        sql = sql + CStr(restartfrequency)

        gbc_db.executesql(sql, certificate)
    End Sub


    Public Sub delete_action(ByVal schedid As Long, ByVal jobid As Long, ByVal clientid As Long, ByRef certificate As bc_cs_security.certificate)

        Dim sql As String
        Dim suser_id As String

        If bc_cs_central_settings.server_flag = 1 Then
            suser_id = certificate.user_id
        Else
            suser_id = bc_cs_central_settings.logged_on_user_id
        End If
        sql = "exec dbo.bc_core_monitor_delete_action " + CStr(schedid) + "," + CStr(jobid) + "," + CStr(clientid)
        gbc_db.executesql(sql, certificate)

    End Sub

End Class


REM Email 
<Serializable()> Public Class bc_srs_emaillist
    Inherits bc_cs_soap_base_class
    Public emaillist As New ArrayList


    Public Overrides Sub process_object()
        Dim otrace As New bc_cs_activity_log("bc_srs_emails", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            REM this is always specific to object
            If Me.tmode = bc_cs_soap_base_class.tREAD Then
                db_read()
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_srs_emails", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_srs_emails", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub

    Public Sub db_read()
        Dim otrace As New bc_cs_activity_log("bc_srs_emails", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try

            Dim i As Integer
            Dim vSRSEmail As Object
            Dim oinput As bc_email_item
            Dim db_email As New bc_srs_email_db

            vSRSEmail = db_email.ReadAllemails(MyBase.certificate)

            If IsArray(vSRSEmail) Then
                For i = 0 To UBound(vSRSEmail, 2)
                    oinput = New bc_email_item(vSRSEmail(0, i), vSRSEmail(1, i), vSRSEmail(2, i), vSRSEmail(3, i), MyBase.certificate)
                    oinput.onerror = vSRSEmail(4, i)
                    oinput.onstatuscheck = vSRSEmail(5, i)
                    oinput.clientid = vSRSEmail(6, i)
                    oinput.client_name = vSRSEmail(7, i)
                    emaillist.Add(oinput)

                Next
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_srs_email", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_srs_email", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub

End Class

<Serializable()> Public Class bc_email_item
    Inherits bc_cs_soap_base_class
    Public write_mode As Long
    Public emailid As Long
    Public name As String
    Public description As String
    Public email As String
    Public onerror As Integer
    Public onstatuscheck As Integer
    Public clientid As Long
    Public client_name As String

    Public dirtyrecord As Boolean = False

    Public Const UPDATE = 1
    Public Const DELETE = 3

    Public Sub New()

    End Sub
    Public Sub New(ByVal id As Integer, ByVal input_name As String, ByVal input_description As String, ByVal input_email As String, ByVal certificate As bc_cs_security.certificate)
        emailid = id
        name = input_name
        description = input_description
        email = input_email
    End Sub

    Public Overrides Sub process_object()
        Dim otrace As New bc_cs_activity_log("bc_email_item", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Select Case Me.tmode
                Case bc_cs_soap_base_class.tWRITE
                    db_write()
                Case bc_cs_soap_base_class.tREAD
                    db_read()
            End Select
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_email_item", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_email_item", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)

        End Try
    End Sub

    Public Sub db_read()

    End Sub


    Public Sub db_write()

        Dim otrace As New bc_cs_activity_log("bc_email_item", "db_write", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try

            Dim vSRSEmail As New bc_srs_email_db

            MyBase.certificate = certificate

            Select Case write_mode
                Case UPDATE
                    vSRSEmail.write_email(Me.emailid, Me.clientid, Me.name, Me.description, Me.email, Me.onerror, Me.onstatuscheck, MyBase.certificate)

                Case DELETE
                    vSRSEmail.delete_email(emailid, clientid, MyBase.certificate)

            End Select

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_email_item", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_email_item", "db_write", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Sub

End Class

Public Class bc_srs_email_db
    Private gbc_db As New bc_cs_db_services
    REM write a tempate record and retuns Id
    Public Sub New()

    End Sub

    REM reads all valid scheduled tasks 
    Public Function ReadAllemails(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "bc_core_monitor_get_emails"
        ReadAllemails = gbc_db.executesql(sql, certificate)
    End Function

    Public Sub write_email(ByVal emailid As Long, ByVal clientid As Long, ByVal name As String, ByVal description As String, ByVal email As String, ByVal onerror As Integer, ByVal onstatuscheck As Integer, ByRef certificate As bc_cs_security.certificate)

        Dim sql As String

        Dim suser_id As String
        If bc_cs_central_settings.server_flag = 1 Then
            suser_id = certificate.user_id
        Else
            suser_id = bc_cs_central_settings.logged_on_user_id
        End If
        sql = "exec dbo.bc_core_monitor_write_email " + CStr(emailid) + "," + CStr(clientid) + ",'" + name + "','" + description + "','" + email + "'," + CStr(onerror) + "," + CStr(onstatuscheck)
        gbc_db.executesql(sql, certificate)

    End Sub


    Public Sub delete_email(ByVal emailid As Long, ByVal clientid As Long, ByRef certificate As bc_cs_security.certificate)

        Dim sql As String
        Dim suser_id As String

        If bc_cs_central_settings.server_flag = 1 Then
            suser_id = certificate.user_id
        Else
            suser_id = bc_cs_central_settings.logged_on_user_id
        End If
        sql = "exec dbo.bc_core_monitor_delete_email " + CStr(emailid) + "," + CStr(clientid)
        gbc_db.executesql(sql, certificate)

    End Sub

End Class



REM Email Filter
<Serializable()> Public Class bc_srs_emailfilters
    Inherits bc_cs_soap_base_class
    Public emailfilter As New ArrayList


    Public Overrides Sub process_object()
        Dim otrace As New bc_cs_activity_log("bc_srs_emailfilter", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            REM this is always specific to object
            If Me.tmode = bc_cs_soap_base_class.tREAD Then
                db_read()
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_srs_emailfilter", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_srs_emailfilter", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub
    Public Sub db_read()
        Dim otrace As New bc_cs_activity_log("bc_srs_emailfilter", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try

            Dim i As Integer
            Dim vSRSEmail As Object
            Dim oinput As bc_emailfilter_item
            Dim db_email As New bc_srs_emailfilters_db

            vSRSEmail = db_email.ReadAllemails(MyBase.certificate)

            If IsArray(vSRSEmail) Then
                For i = 0 To UBound(vSRSEmail, 2)
                    oinput = New bc_emailfilter_item(vSRSEmail(0, i), vSRSEmail(1, i), vSRSEmail(2, i), vSRSEmail(3, i), MyBase.certificate)

                    oinput.value1 = vSRSEmail(4, i)
                    oinput.value2 = vSRSEmail(5, i)
                    oinput.preftype = vSRSEmail(6, i)
                    oinput.checkname = vSRSEmail(7, i)

                    emailfilter.Add(oinput)

                Next
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_srs_emailfilter", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_srs_emailfilter", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub

End Class

<Serializable()> Public Class bc_emailfilter_item
    Inherits bc_cs_soap_base_class
    Public write_mode As Long
    Public clientemailid As Long
    Public prefid As Long
    Public prfname As String
    Public value1 As String
    Public value2 As String
    Public preftype As String
    Public checkname As String
    Public dirtyrecord As Boolean = False
    Public filterid As Long

    Public Const UPDATE = 1
    Public Const DELETE = 3
    Public Sub New()

    End Sub
    Public Sub New(ByVal id As Integer, ByVal input_clientemailid As Integer, ByVal input_prefid As String, ByVal input_prefname As String, ByVal certificate As bc_cs_security.certificate)
        filterid = id
        clientemailid = input_clientemailid
        prefid = input_prefid
        prfname = input_prefname
    End Sub

    Public Overrides Sub process_object()
        Dim otrace As New bc_cs_activity_log("bc_emailfilter_item", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Select Case Me.tmode
                Case bc_cs_soap_base_class.tWRITE
                    db_write()
                Case bc_cs_soap_base_class.tREAD
                    db_read()
            End Select
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_emailfilter_item", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_emailfilter_item", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)

        End Try
    End Sub

    Public Sub db_read()

    End Sub

    Public Sub db_write()

        Dim otrace As New bc_cs_activity_log("bc_emailfilter_item", "db_write", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try

            Dim vSRSEmailFilter As New bc_srs_emailfilter_item_db

            MyBase.certificate = certificate

            Select Case write_mode
                Case UPDATE
                    vSRSEmailFilter.write_emailfilter(Me.filterid, Me.clientemailid, Me.prefid, Me.value1, Me.value2, MyBase.certificate)

                Case DELETE
                    vSRSEmailFilter.delete_emailfilter(Me.filterid, Me.clientemailid, MyBase.certificate)


            End Select

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_emailfilter_item", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_emailfilter_item", "db_write", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Sub

End Class

Public Class bc_srs_emailfilters_db
    Private gbc_db As New bc_cs_db_services
    REM write a tempate record and retuns Id
    Public Sub New()

    End Sub

    REM reads all valid scheduled tasks 
    Public Function ReadAllemails(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "bc_core_monitor_get_emailsfilters"
        ReadAllemails = gbc_db.executesql(sql, certificate)
    End Function

End Class


Public Class bc_srs_emailfilter_item_db
    Private gbc_db As New bc_cs_db_services

    Public Sub New()

    End Sub

    Public Sub write_emailfilter(ByVal filterid As Long, ByVal clientemailid As Long, ByVal prefid As Long, ByVal value1 As String, ByVal value2 As String, ByRef certificate As bc_cs_security.certificate)

        Dim sql As String

        Dim suser_id As String
        If bc_cs_central_settings.server_flag = 1 Then
            suser_id = certificate.user_id
        Else
            suser_id = bc_cs_central_settings.logged_on_user_id
        End If
        sql = "exec dbo.bc_core_monitor_write_emailfilter " + CStr(filterid) + "," + CStr(clientemailid) + "," + CStr(prefid) + ",'" + value1 + "','" + value2 + "'"
        gbc_db.executesql(sql, certificate)
    End Sub

    Public Sub delete_emailfilter(ByVal filterid As Long, ByVal clientemailid As Long, ByRef certificate As bc_cs_security.certificate)

        Dim sql As String
        Dim suser_id As String

        If bc_cs_central_settings.server_flag = 1 Then
            suser_id = certificate.user_id
        Else
            suser_id = bc_cs_central_settings.logged_on_user_id
        End If
        sql = "bc_core_monitor_delete_emailfilter " + CStr(filterid) + "," + CStr(clientemailid)
        gbc_db.executesql(sql, certificate)

    End Sub


End Class



REM Job Types 
<Serializable()> Public Class bc_srs_typelist
    Inherits bc_cs_soap_base_class
    Public typelist As New ArrayList


    Public Overrides Sub process_object()
        Dim otrace As New bc_cs_activity_log("bc_srs_typelist", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            REM this is always specific to object
            If Me.tmode = bc_cs_soap_base_class.tREAD Then
                db_read()
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_srs_typelist", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_srs_typelist", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub
    Public Sub db_read()
        Dim otrace As New bc_cs_activity_log("bc_srs_typelist", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try

            Dim i As Integer
            Dim vTypeList As Object
            Dim oinput As bc_srs_type
            Dim db_typelist As New bc_srs_Types_db

            vTypeList = db_typelist.ReadAllTypes(MyBase.certificate)

            If IsArray(vTypeList) Then
                For i = 0 To UBound(vTypeList, 2)
                    oinput = New bc_srs_type(vTypeList(0, i), vTypeList(1, i), vTypeList(2, i), MyBase.certificate)
                    typelist.Add(oinput)
                Next
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_srs_typelist", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_srs_typelist", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub

End Class


<Serializable()> Public Class bc_srs_type
    Inherits bc_cs_soap_base_class
    Public write_mode As Long

    Public typeid As Long
    Public type_name As String
    Public checkdecscription As String

    Public Const UPDATE = 1

    Public Sub New()

    End Sub
    Public Sub New(ByVal input_typeid As Integer, ByVal input_checkname As String, input_decscription As String, ByVal certificate As bc_cs_security.certificate)

        typeid = input_typeid
        type_name = input_checkname
        checkdecscription = input_decscription

    End Sub

    Public Sub db_read()

    End Sub

End Class


Public Class bc_srs_types_db
    Private gbc_db As New bc_cs_db_services
    REM write a tempate record and retuns Id
    Public Sub New()

    End Sub

    REM reads all types
    Public Function ReadAllTypes(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "bc_core_monitor_get_types"
        ReadAllTypes = gbc_db.executesql(sql, certificate)
    End Function

End Class
