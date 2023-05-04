REM this part sits on monitroing server
Imports System.ServiceProcess



Public Class bc_cs_monitor
    REM polls as specified for the monitoring service
    Public Sub poll()
        REM read things that are being monitored to see if they now require a poll


        REM call each ine

        REM log results
    End Sub

End Class

REM this parts sits on server being monitored to make the calls to check things
<Serializable()> Public Class bc_cs_monitor_core_web_service
    Inherits bc_cs_soap_base_class
    Public status As Boolean = False
    Public error_monitoring As Boolean = False
    Public check_type As echeck_type
    Public custom_sp As String
    Public results As Object
    Public check_date_from As DateTime
    Public schedule_id As Long
    Public remote_service As String
    Public computer_name As String
    Public Enum echeck_type
        SERVICE_UP = 1
        LOG_FILE = 2
        CUSTOM_CHECK = 3
        CHECK_REMOTE_SERVICE_UP = 4
        START_REMOTE_SERVICE = 5

    End Enum
    Public Overrides Sub process_object()
        db_read()
    End Sub

    Public Sub db_read()
        Dim db As db_bc_cs_monitor_core_web_service
        Dim certificate As bc_cs_security.certificate
        Try

            Select Case Me.check_type
                Case echeck_type.SERVICE_UP
                    status = True
                Case echeck_type.LOG_FILE
                    db = New db_bc_cs_monitor_core_web_service
                    certificate = New bc_cs_security.certificate
                    certificate.user_id = "BCMonitoringServicesLogFile"
                    results = db.run_check("bc_core_monitor_check_log", check_date_from, schedule_id, certificate)
                    If certificate.server_errors.Count > 0 Then
                        error_monitoring = True
                        results = certificate.server_errors(0)
                    End If
                Case echeck_type.CUSTOM_CHECK
                    db = New db_bc_cs_monitor_core_web_service
                    certificate = New bc_cs_security.certificate
                    certificate.user_id = "BCMonitoringServicesCustomCheck: " + custom_sp
                    results = db.run_check(custom_sp, check_date_from, schedule_id, certificate)
                    If certificate.server_errors.Count > 0 Then
                        error_monitoring = True
                        results = certificate.server_errors(0)
                    End If
                Case echeck_type.CHECK_REMOTE_SERVICE_UP
                    Dim scService As ServiceController
                    scService = New ServiceController(remote_service, computer_name)
                    If scService.Status.Equals(ServiceControllerStatus.Running) Then
                        status = True
                    Else
                        status = False
                    End If

                Case echeck_type.START_REMOTE_SERVICE
                    Dim scService As ServiceController
                    scService = New ServiceController(remote_service)
                    If Not scService.Status.Equals(ServiceControllerStatus.Running) Then
                        scService.Start()
                        status = True
                    End If

            End Select
        Catch ex As Exception
            results = ex.Message

        End Try
    End Sub
    Private Class db_bc_cs_monitor_core_web_service
        Dim gdb As New bc_cs_db_services
        Public Function run_check(sp_name As String, from_date As DateTime, schedule_id As Integer, ByRef certificate As bc_cs_security.certificate) As Object
            Try
                Dim param As New bc_cs_db_services.bc_cs_sql_parameter
                param.name = "from_date"
                param.value = from_date
                Dim params As New List(Of bc_cs_db_services.bc_cs_sql_parameter)
                params.Add(param)
                param = New bc_cs_db_services.bc_cs_sql_parameter
                param.name = "schedule_id"
                param.value = schedule_id
                params.Add(param)

                run_check = gdb.executesql_with_parameters(sp_name, params, certificate)
            Catch ex As Exception
                run_check = "Error check_log_file_db: " + ex.Message
            End Try

        End Function
    End Class
End Class
