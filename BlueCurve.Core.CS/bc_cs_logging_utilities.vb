Imports System.Collections
Imports System.Threading
REM ==========================================
REM Bluecurve Limited 2005
REM Module:       Central Logging utilities
REM Type:         Common Services
REM Description:  Error Logging
REM               Activity Logging
REM Version:      1
REM Change history
REM ==========================================
Imports System.io
Public Class bc_cs_error_codes
    Public Const DB_ERR = "Database error"
    Public Const CONFIG_FILE_ERR = "Config file not found or corrupt"
    Public Const XML_LOAD_ERROR = "XML Error"
    Public Const USER_DEFINED = "User Defined"
    Public Const RETURN_ERROR = 900
End Class
Public Class bc_cs_error_response_codes
    Public Const ABORT = 3
    Public Const RETRY = 4
    Public Const IGNORE = 5
End Class
REM Error Logging class
Public Class bc_cs_error_log
    Dim strError As String
    Dim strdate As New System.DateTime
    Dim ostrdate As String
    Dim bc_cs_central_settings As bc_cs_central_settings
    Dim bc_cs_error_log_File As New bc_cs_error_log_File
    Public err_external_code As String
    Public ireturncode As Integer
    Public certificate As New bc_cs_security.certificate
    REM error logging class
    REM constructor for .Net method: try, catch, finally
    Public Sub New(ByVal strclass As String, ByVal strmethod As String, ByVal err_internal_code As String, ByVal err_external_desc As String, Optional ByRef certificate As bc_cs_security.certificate = Nothing)
        Try
            Me.certificate = certificate
            certificate.error_state = True
            certificate.server_errors.Add("Server Error: (" + strclass + ":" + strmethod + " - " + CStr(err_external_desc))

        Catch

        End Try
        log_error(strclass, strmethod, err_internal_code, err_external_code, err_external_desc, False, False)
    End Sub
    REM constructor for VB6 method: on error
    Public Sub New(ByVal strclass As String, ByVal strmethod As String, ByVal err_internal_code As String, ByVal err_external_code As String, ByVal err_external_desc As String, Optional ByRef certificate As bc_cs_security.certificate = Nothing)
        log_error(strclass, strmethod, err_internal_code, err_external_code, err_external_code, True, True)
    End Sub
    Private Sub log_error(ByVal strclass As String, ByVal strmethod As String, ByVal err_internal_code As String, ByVal err_external_code As String, ByVal err_external_desc As String, ByVal vb6_method As Boolean, ByVal vb6_error As String, Optional ByRef certificate As bc_cs_security.certificate = Nothing)
        Dim omessage As New bc_cs_message
        Try
            REM get error text
            If err_internal_code = bc_cs_error_codes.CONFIG_FILE_ERR Then
                strError = "Configuration File Error: " + err_external_desc
                omessage = New bc_cs_message("Blue Curve", strError, bc_cs_message.MESSAGE)
            Else
                Dim current_user As String
                REM get user
                If bc_cs_central_settings.server_flag = 1 Then
                    Try
                        current_user = Me.certificate.user_id
                    Catch
                        current_user = "unknown"
                    End Try
                    REM is server mode take autentoicated user
                Else
                    REM if client mode take workstation logged on iser
                    current_user = bc_cs_central_settings.logged_on_user_name
                End If

                Dim olog As New bc_cs_om_log
                olog.authenticated_user = current_user
                'olog.log_date = strdate.Now
                olog.class_name = strclass
                olog.method_name = strmethod
                olog.server_flag = bc_cs_central_settings.server_flag
                olog.error_flag = 1
                olog.type_flag = err_external_code
                olog.description = "Error: " + CStr(err_internal_code) + " Code: " + CStr(err_external_code) + ": " + err_external_desc

                strError = "Error: " + CStr(err_internal_code)
                strError = strError + ": " + err_external_desc + "; " + olog.class_name + ":" + olog.method_name
                bc_cs_progress.suspend = True

                REM produce message only if client
                If bc_cs_central_settings.server_flag = 0 Then
                    If vb6_method = True Then
                        omessage = New bc_cs_message("Blue Curve", strError, bc_cs_message.ERR)
                    Else
                        If err_external_desc <> "Thread was being aborted." Then
                            omessage = New bc_cs_message("Blue Curve", strError, bc_cs_message.MESSAGE)
                        End If
                    End If
                End If

                REM resume progress bar
                bc_cs_progress.suspend = False
                REM write error to activity log

                If bc_cs_central_settings.server_flag = 1 Then
                    REM write this to database
                    olog.db_write()
                Else
                    If bc_cs_central_settings.error_file = "on" Then
                        REM write out to log
                        olog.write_to_file(err_external_code, True)
                    End If
                    REM add error log to object model
                    REM write out XMl activity log to server if flag set
                    REM and running in soap mode
                    If bc_cs_central_settings.server_logging = 1 Then
                        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                            bc_cs_activity_log_file.xml_activity_log.bc_om_logs.Add(olog)
                            bc_cs_activity_log_file.xml_activity_log.db_write()
                            bc_cs_activity_log_file.xml_activity_log.bc_om_logs.Clear()
                        End If
                        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                            REM client side error to log on server
                            REM copy over logs trace so no more gets added
                            Dim onewlogs As New bc_cs_om_logs
                            'Dim i As Integer
                            If bc_cs_activity_log_file.xml_activity_log.bc_om_logs.Count >= 10 Then
                                For i = bc_cs_activity_log_file.xml_activity_log.bc_om_logs.Count - 10 To bc_cs_activity_log_file.xml_activity_log.bc_om_logs.Count - 1
                                    onewlogs.bc_om_logs.Add(bc_cs_activity_log_file.xml_activity_log.bc_om_logs(i))
                                Next
                            End If
                            onewlogs.bc_om_logs.Add(olog)
                            onewlogs.tmode = bc_cs_soap_base_class.tWRITE
                            onewlogs.no_send_back = True
                            onewlogs.transmit_to_server_and_receive(Nothing, True)
                            bc_cs_activity_log_file.xml_activity_log.bc_om_logs.Clear()
                        End If
                    End If
                    End If
                    bc_cs_central_settings = Nothing
            End If
            REM if a progress bar refresh
        Catch ex As Exception
            If bc_cs_central_settings.server_flag = 1 Then
                bc_cs_activity_log.logging_error(1, "bc_om_Log", "db_write", ex.Message)
            End If
        Finally
            'bc_cs_central_settings.progress_bar.refresh()
        End Try
    End Sub

End Class
Public Class bc_cs_error_log_file
    Dim csettings As New bc_cs_central_settings
    Dim fstream As StreamWriter
    REM constructor for initiliazing log file
    Public Sub New()
        Dim fn As FileStream
        On Error GoTo error_handler
        REM PR removed 3/10
        Exit Sub
        'bc_cs_central_settings.logging_thread = New Thread(New ThreadStart(AddressOf dotask))
        'bc_cs_central_settings.logging_thread.Start()
        fn = New FileStream(get_log_file_name, FileMode.Truncate)
        fn.Close()
        REM initiate SQL logging process
        Exit Sub
error_handler:
        fn = New FileStream(get_log_file_name, FileMode.CreateNew)
        fn.Close()
    End Sub
    'Public Sub dotask()
    '    Dim gdb As New bc_cs_db_services
    '    gdb.dotask()
    'End Sub
    REM constructor for writing to log file
    Public Sub New(ByVal strerrortext As String)
        REM PR removed 3/10
        Exit Sub
        REM append file with stream
        On Error GoTo error_handler

        fstream = New StreamWriter(get_log_file_name, FileMode.Append)

        fstream.WriteLine(strerrortext)
        'write_stack_trace()
        fstream.Close()
        Exit Sub
error_handler:
        'Dim omessage = New bc_cs_message("Blue Curve", "Log File Not Initialed. Please initialize using bc_cs_error_log_file", bc_cs_message.MESSAGE)

    End Sub
    Private Function get_log_file_name() As String
        Dim cs As New bc_cs_central_settings
        If bc_cs_central_settings.server_flag = 0 Then
            get_log_file_name = bc_cs_central_settings.get_user_dir + "\bc_errors.txt"
        Else
            get_log_file_name = bc_cs_central_settings.server_log_file_path + "\bc_errors.txt"
        End If

    End Function
    Friend Sub write_stack_trace()
        Dim cst As New bc_cs_stack_trace
        Dim i As Integer
        Dim strtracetx As String

        fstream = New StreamWriter(get_log_file_name, FileMode.Append)
        fstream.WriteLine(" Stack Trace:")
        For i = 1 To bc_cs_stack_trace.cstack_trace_elements.Count
            If i = 1 Then
                strtracetx = " Raised in: " + bc_cs_stack_trace.cstack_trace_elements(bc_cs_stack_trace.cstack_trace_elements.Count - i + 1).strcls + ": " + bc_cs_stack_trace.cstack_trace_elements(bc_cs_stack_trace.cstack_trace_elements.Count - i + 1).strmeth
            Else
                strtracetx = "  called from: " + bc_cs_stack_trace.cstack_trace_elements(bc_cs_stack_trace.cstack_trace_elements.Count - i + 1).strcls + ": " + bc_cs_stack_trace.cstack_trace_elements(bc_cs_stack_trace.cstack_trace_elements.Count - i + 1).strmeth
            End If
            fstream.WriteLine(strtracetx)
        Next
        fstream.Close()
    End Sub

End Class
Public Class bc_cs_activity_codes
    Public Const TRACE_ENTRY = 1
    Public Const TRACE_EXIT = 2
    Public Const COMMENTARY = 3
    Public Const SQL = 4
End Class
Public Class bc_cs_activity_log
    Dim strError As String
    Dim strdate As New System.DateTime
    Dim ostrdate As String
    Dim bc_cs_central_settings As New bc_cs_central_settings
    Dim bc_cs_activity_log_file As bc_cs_activity_log_file
    Public ireturncode As Integer
    Public certificate As New bc_cs_security.certificate
    Dim progress As bc_cs_progress
    REM error logging class
    Public Sub New(ByVal strclass As String, ByVal strmethod As String, ByVal err_external_code As String, ByVal err_external_desc As String, Optional ByRef certificate As bc_cs_security.certificate = Nothing)
        Try
            Dim ost As bc_cs_stack_trace
            Dim current_user As String
            REM get user
            If bc_cs_central_settings.server_flag = 1 Then
                REM check server logging is enabled if not exit
                If bc_cs_central_settings.server_logging = 0 Then
                    Exit Sub
                End If
                REM is server mode take autentoicated user from certificate
                Try
                    Me.certificate = certificate
                    current_user = Me.certificate.user_id
                Catch
                    current_user = "unknown"
                End Try
            Else
                REM if client mode take workstation logged on iser
                current_user = bc_cs_central_settings.logged_on_user_name
            End If

            REM create log object
            Dim olog As New bc_cs_om_log
            olog.authenticated_user = current_user
            olog.log_date = Date.Now
            olog.class_name = strclass
            olog.method_name = strmethod
            olog.server_flag = bc_cs_central_settings.server_flag
            olog.error_flag = 0
            olog.type_flag = err_external_code
            olog.description = err_external_desc

            Select Case err_external_code
                REM for a server log everything
            Case bc_cs_activity_codes.TRACE_ENTRY
                    REM add to calling stack trace

                    'ost = New bc_cs_stack_trace(strclass, strmethod)
                    If bc_cs_central_settings.trace <> "on" And bc_cs_central_settings.server_flag = 0 Then
                        Exit Sub
                    End If

                Case bc_cs_activity_codes.TRACE_EXIT
                    REM remove from calling stack trace
                    'ost.cstack_trace_elements.Remove(ost.cstack_trace_elements.Count)
                    If bc_cs_central_settings.trace <> "on" And bc_cs_central_settings.server_flag = 0 Then
                        Exit Sub
                    End If
                    ost = New bc_cs_stack_trace

                Case bc_cs_activity_codes.COMMENTARY
                    REM update commentary globals status
                    bc_cs_progress.commentary = err_external_desc

                Case bc_cs_activity_codes.SQL
                    If bc_cs_central_settings.trace_sql <> "on" And bc_cs_central_settings.server_flag = 0 Then
                        Exit Sub
                    End If
            End Select
            If bc_cs_central_settings.server_flag = 1 Then
                REM write log to database if server mode
                olog.db_write()
            Else
                REM if client write to file
                REM write log to xml memory object
                bc_cs_activity_log_file.xml_activity_log.bc_om_logs.Add(olog)

                If bc_cs_central_settings.activity_file = "on" Then
                    olog.write_to_file(err_external_code, False)
                End If
            End If
        Catch ex As Exception
            logging_error(1, "bc_cs_activity_log", "new", ex.Message)
        Finally

        End Try

    End Sub

    REM used to report an error in the logging system
    REM this obviously cantbe reported through the logging system
    Public Shared Sub logging_error(ByVal error_flag, ByVal class_name, ByVal method, ByVal msg)
      
    

        Try
            Dim bc_cs_central_settings As New bc_cs_central_settings

            If bc_cs_central_settings.server_flag = 0 Then
                REM if client use message box
                MsgBox(msg + " called in: " + class_name + ": " + method, MsgBoxStyle.OkOnly, "Blue Curve Logging Services Error")
            Else
                REM if server write to log
                Dim err As Boolean = True
                Dim i As Integer = 0
                While err = True And i < 10
                    Try
                        Dim fs As New StreamWriter(bc_cs_central_settings.server_log_file_path + "bc_logging_services_errors.txt", True)
                        fs.WriteLine(Now + " type: " + CStr(error_flag) + " " + class_name + ": " + method + " " + msg)
                        fs.Close()
                        err = False

                    Catch
                        Thread.Sleep(1000)
                        i = i + 1
                    End Try
                End While
            End If

        Catch ex As Exception
            MsgBox("Fatal Logging Error Contact System Administrator", MsgBoxStyle.Critical, "Blue Curve Logging Services")
        End Try

    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class
Public Class bc_cs_activity_log_file
    Dim csettings As New bc_cs_central_settings
    Public Shared xml_activity_log As New bc_cs_om_logs
    REM constructor for initiliazing log file
    Public Sub New()
        Dim fn As FileStream

        Dim logDate As DateTime
        Dim FileCreationDate As DateTime
        Dim objFileInfo As FileInfo

        On Error GoTo error_handler

        REM Steve Wooderson 10/03/2011 Activity Trace
        If File.Exists(get_log_file_name) Then
            REM check activity file date
            objFileInfo = New FileInfo(get_log_file_name)
            FileCreationDate = objFileInfo.CreationTime
            logDate = Date.Today

            REM if not for today archive files
            If FileCreationDate < logDate Then
                Me.archive_files()
            End If
            objFileInfo = Nothing
        End If

        fn = New FileStream(get_log_file_name, FileMode.Append)
        fn.Close()

        Exit Sub
error_handler:
        fn = New FileStream(get_log_file_name, FileMode.CreateNew)
        fn.Close()
    End Sub
    REM constructor for writing to log file
    Friend Sub New(ByVal strerrortext As String)
        REM append file with stream
        On Error GoTo error_handler

        Dim logDate As DateTime
        Dim FileCreationDate As DateTime
        Dim objFileInfo As FileInfo

        REM Steve Wooderson 10/03/2011 Activity Trace
        If File.Exists(get_log_file_name) Then
            REM check activity file date
            objFileInfo = New FileInfo(get_log_file_name)
            FileCreationDate = objFileInfo.CreationTime
            logDate = Date.Today

            REM if not for today archive files
            If FileCreationDate < logDate Then
                Me.archive_files()
            End If
            objFileInfo = Nothing
        End If

        Dim fstream As New StreamWriter(get_log_file_name, FileMode.Append)

        fstream.WriteLine(strerrortext)
        fstream.Close()

        Exit Sub

error_handler:
        'Dim omessage = New bc_cs_message("Blue Curve", "Activity Log File Not Initialed. Please initialize using bc_cs_activity_log_file", bc_cs_message.MESSAGE)
    End Sub
    Private Function get_log_file_name() As String

        Dim cs As New bc_cs_central_settings
        If bc_cs_central_settings.server_flag = 0 Then
            get_log_file_name = bc_cs_central_settings.get_user_dir + "\bc_activity.txt"
        Else
            get_log_file_name = bc_cs_central_settings.server_log_file_path + "\bc_activity.txt"
        End If

    End Function


    Private Sub archive_files()

        Dim logDate As DateTime
        Dim keepDate As DateTime
        Dim fileDate As String
        Dim logPath As String
        Dim archiveFolder As String
        Dim activityFile As String
        Dim folderFile As String
        Dim archiveFileName As String

        Dim objFileInfo As FileInfo
        Dim FileCreationDate As DateTime
        Dim fileCount As Long

        REM Steve Wooderson 10/03/2011 Activity Trace
        Try

            If bc_cs_central_settings.server_flag = 0 Then
                logPath = bc_cs_central_settings.get_user_dir
            Else
                logPath = bc_cs_central_settings.server_log_file_path
            End If

            logDate = Date.Today
            archiveFolder = logPath + bc_cs_central_settings.trace_log_archive

            REM Archive old activity file
            If Len(bc_cs_central_settings.trace_log_archive) > 0 Then

                REM check if archive directory exists
                If Not (Directory.Exists(archiveFolder)) Then
                    Directory.CreateDirectory(archiveFolder)
                End If

                activityFile = get_log_file_name()
                If File.Exists(activityFile) Then
                    REM move old files to archive
                    fileDate = ""
                    objFileInfo = New FileInfo(activityFile)
                    FileCreationDate = objFileInfo.CreationTime
                    fileDate = Format(CDate(FileCreationDate), "yyyyMMMdd")
                    archiveFileName = "bc_activity_" + fileDate + ".txt"

                    fileCount = 1
                    Do While True
                        If Not File.Exists(archiveFolder + "\" + archiveFileName) Then
                            File.Move(activityFile, archiveFolder + "\" + archiveFileName)
                            Dim fs As New StreamWriter(activityFile)
                            fs.Close()
                            File.SetCreationTime(activityFile, Date.Now)
                            Exit Do
                        Else
                            archiveFileName = "bc_activity" + fileCount.ToString + "_" + fileDate + ".txt"
                            fileCount = fileCount + 1
                        End If
                    Loop

                    'If Directory.GetFiles(logPath).Length > 0 Then
                    '    For Each folderFile In Directory.GetFiles(logPath)
                    '        If InStr(folderFile, "bc_activity") > 0 Then
                    '            fileDate = ""
                    '            objFileInfo = New FileInfo(folderFile)
                    '            FileCreationDate = objFileInfo.CreationTime
                    '            fileDate = Format(CDate(FileCreationDate), "yyyyMMMdd")
                    '            fileName = "bc_activity_" + fileDate + ".txt"
                    '            File.Move(folderFile, archiveFolder + "\" + fileName)
                    '        End If
                    '    Next
                    'End If
                End If

                REM Delete any files from archive that are older than trace_history_days
                If bc_cs_central_settings.trace_history_days > 0 Then
                    keepDate = DateAdd(DateInterval.Day, -bc_cs_central_settings.trace_history_days, logDate)
                    For Each folderFile In Directory.GetFiles(archiveFolder)
                        If InStr(activityFile, "activity") > 0 Then
                            fileDate = ""
                            fileDate = Mid(folderFile, InStrRev(folderFile, "_") + 1, 9)
                            If IsDate(fileDate) Then
                                If CDate(fileDate) < keepDate Then
                                    File.Delete(folderFile)
                                End If
                            End If
                        End If
                    Next
                End If
            End If

        Catch ex As Exception
            bc_cs_activity_log.logging_error(1, "bc_cs_activity_log_file", "archive_files", ex.Message)
        End Try

    End Sub

End Class
REM stack trace class initialises a trace entry point and add to collection 
Friend Class bc_cs_stack_trace
    Public strcls As String
    Public strmeth As String
    Public Shared cstack_trace_elements As New Collection
    Public Sub New(ByVal strclass As String, ByVal strmethod As String)
        strcls = strclass
        strmeth = strmethod
        cstack_trace_elements.Add(Me)
    End Sub
    Public Sub New()

    End Sub
End Class
REM ===============================================
REM Bluecurve Limited 2005
REM Module:       Logging
REM Type:         Object Model
REM Description:  Error Activity Log class
REM Components, 
REM sub Components
REM Version:      1
REM Change history
REM ===============================================
<Serializable()> Public Class bc_cs_om_logs
    Inherits bc_cs_soap_base_class
    Public bc_om_logs As New ArrayList
    Public Overrides Sub process_object()
        Select Case tmode
            Case tWRITE
                db_write()
        End Select
    End Sub
    Public Shadows Sub db_write()
        Dim i As Integer
        Try
            For i = 0 To bc_om_logs.Count - 1
                bc_om_logs(i).db_write()
            Next

        Catch ex As Exception
            bc_cs_activity_log.logging_error(1, "bc_om_Log", "db_write", ex.Message)
        End Try
    End Sub
    'Public Overloads Overrides Function call_web_service(ByVal os As String) As String
    '    webservice.Uploaderror(os)
    'End Function
    'Public Overrides Function write_xml_via_soap_client_request() As Object
    '    Try
    '        Dim ocommentary As bc_cs_activity_log

    '        Dim s, os As String
    '        Dim bc_cs_accountinfoheader As New localhost.bc_cs_accountinfoheader
    '        webservice = New localhost.Service1
    '        webservice.Url = bc_cs_central_settings.soap_server
    '        bc_cs_accountinfoheader.os_logon_name = bc_cs_central_settings.logged_on_user_name
    '        webservice.bc_cs_accountinfoheaderValue = bc_cs_accountinfoheader
    '        REM call object model specific web service
    '        os = write_xml_to_string()
    '        s = call_web_service(os)
    '        If InStr(1, s, "Authentication") > 0 Then
    '            write_xml_via_soap_client_request = Nothing
    '        End If
    '    Catch ex As Exception
    '        bc_cs_activity_log.logging_error("bc_om_Log", "write_xml_via_soap_client_request", ex.Message)
    '    End Try

    'End Function

End Class
<Serializable()> Public Class bc_cs_om_log
    Public authenticated_user As String
    Public log_date As DateTime
    Public class_name As String
    Public method_name As String
    Public server_flag As Integer
    Public error_flag As Integer
    Public type_flag As Integer
    Public description As String
    REM Public t As Thread
    Public Sub New()

    End Sub
    Public Sub db_write()
        Try
            Dim gbc_db As New bc_om_logging_db
            gbc_db.write_log(authenticated_user, log_date, class_name, method_name, server_flag, error_flag, type_flag, description)
        Catch ex As Exception
            bc_cs_activity_log.logging_error(error_flag, "bc_om_log", "db_write", ex.Message)
        End Try
    End Sub
    Private Sub dotask()
        Dim gbc_db As New bc_om_logging_db
        gbc_db.write_log(authenticated_user, log_date, class_name, method_name, server_flag, error_flag, type_flag, description)
    End Sub
    Public Sub write_to_file(ByVal err_external_code As Integer, ByVal error_flag As Boolean)
        Try
            Dim bc_cs_activity_log_file As bc_cs_activity_log_file
            Dim bc_cs_error_log_file As bc_cs_error_log_file
            Dim stractivity As String

            stractivity = Me.log_date + ":" + CStr(Me.log_date.Millisecond)
            If error_flag = False Then
                REM activity log
                Select Case err_external_code

                    Case bc_cs_activity_codes.TRACE_ENTRY
                        stractivity = stractivity + " Trace Entry: "

                    Case bc_cs_activity_codes.TRACE_EXIT
                        stractivity = stractivity + " Trace Exit : "

                    Case bc_cs_activity_codes.COMMENTARY
                        stractivity = stractivity + " Commentary: "

                    Case bc_cs_activity_codes.SQL
                        stractivity = stractivity + " SQL: "
                End Select
            End If

            stractivity = stractivity + Me.description
            stractivity = stractivity + " called from: " + Me.class_name + "." + Me.method_name

            If error_flag = False Then
                bc_cs_activity_log_file = New bc_cs_activity_log_file(stractivity)
            Else
                bc_cs_error_log_file = New bc_cs_error_log_file(stractivity)
            End If
        Catch ex As Exception
            bc_cs_activity_log.logging_error(1, "bc_om_Log", "db_write", ex.Message)
        End Try
    End Sub

End Class
Public Class bc_om_logging_db
    Private gbc_db As New bc_cs_db_services
    REM reads all pub type in database
    Public Sub write_log(ByVal user, ByVal log_date, ByVal class_name, ByVal method_name, ByVal server_flag, ByVal error_flag, ByVal type_flag, ByVal description)
        Dim sql As String
        Dim res As Object

        Try
            REM if encryped mode dont allow logs until connection established
            If bc_cs_central_settings.connection_type = "encrypted" And bc_cs_central_settings.server_flag = 1 Then
                REM write errors to log file
                If error_flag = 1 Or bc_cs_central_settings.server_logging = 1 Then
                    bc_cs_activity_log.logging_error(error_flag, class_name, method_name, description)
                End If
                Exit Sub
            End If
            
            Dim ostr As New bc_cs_string_services(description)
            description = ostr.delimit_apostrophies
           
            REM PR use identity field to stop this slowing down
            sql = "insert into dbo.bc_error_activity_handler_logs (authenticated_user,log_date,class_name,method_name,server_flag,error_flag,type_flag,[description])  select '" + CStr(user) + "',getutcdate(),'" + CStr(class_name) + "','" + CStr(method_name) + "'," + CStr(server_flag) + "," + CStr(error_flag) + "," + CStr(type_flag) + ",'" + CStr(description) + "'"
            
            res = gbc_db.executesql_no_log(sql)
            If IsArray(res) Then
                If UBound(res, 2) >= 0 Then
                    If UCase(res(0, 0)) = UCase("Error") Then
                        If bc_cs_central_settings.server_flag = 1 Then
                            REM write out error to file instead
                            bc_cs_activity_log.logging_error(1, class_name, method_name, description)
                        End If
                    End If
                End If
            End If

        Catch ex As Exception
            If bc_cs_central_settings.server_flag = 0 Then
                MsgBox("bc_om_logging_db:write_log: ", MsgBoxStyle.Critical, "Logging Services")
            Else
                bc_cs_activity_log.logging_error(1, "bc_om_Log", "db_write", ex.Message)
            End If

        Finally
        End Try
    End Sub
End Class
