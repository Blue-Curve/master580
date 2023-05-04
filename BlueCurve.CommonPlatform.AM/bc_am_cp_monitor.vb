Imports BlueCurve.Core.OM
Imports BlueCurve.Core.CS
Imports System.Windows.Forms
Imports System.Drawing



Public Class bc_am_cp_monitor
    Implements Ibc_am_cp_monitor

    Public controller As Object
    Dim SRS_Clients As New bc_srs_clientlist
    Dim SRS_Jobs As New bc_srs_joblist
    Dim SRS_Actions As New bc_srs_actionlist
    Dim SRS_Errors As New bc_srs_errorlist
    Dim SRS_Stats As New bc_srs_statlist
    Dim SRS_Emails As New bc_srs_emaillist
    Dim SRS_Emailfilters As New bc_srs_emailfilters
    Dim SRS_JobTypes As bc_srs_typelist
    Dim selectedClientId As Long
    Dim refreshing As Boolean = False
    Dim uncomitted_data As Boolean = False

    Event save_monitor() Implements Ibc_am_cp_monitor.save_monitor
    Event save_sched(joblist As ArrayList, actionlist As ArrayList) Implements Ibc_am_cp_monitor.save_sched
    Event refresh() Implements Ibc_am_cp_monitor.refresh

    Public Function load_view(SRSClients As bc_srs_clientlist, SRSJobs As bc_srs_joblist, SRSActions As bc_srs_actionlist, SRSErrors As bc_srs_errorlist, SRSStats As bc_srs_statlist, SRSEmails As bc_srs_emaillist, SRSEmailfilters As bc_srs_emailfilters, SRSJobTypes As bc_srs_typelist) As Boolean Implements Ibc_am_cp_monitor.load_view

        Dim selectedJob As Long
        Dim selectedClientName As String
        Dim selectedActionId As Long
        Dim rowClient As Long
        Dim TreeClientid As Long

        Me.Cursor = Cursors.WaitCursor

        Dim otrace As New bc_cs_activity_log("bc_am_cp_monitor", "load_view", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try

            SRS_Clients = SRSClients
            SRS_Jobs = SRSJobs
            SRS_Actions = SRSActions
            SRS_Errors = SRSErrors
            SRS_Stats = SRSStats
            SRS_Emails = SRSEmails
            SRS_Emailfilters = SRSEmailfilters
            SRS_JobTypes = SRSJobTypes

            refreshing = True

            'Load clients combo
            uxClients.Items.Clear()
            rowClient = uxClients.Items.Add("All")
            'uxClients.Items(rowClient).tag = 0
            For x = 0 To SRS_Clients.clientlist.Count - 1
                rowClient = uxClients.Items.Add(SRS_Clients.clientlist(x).client_name)
                'uxClients.Items(rowClient).tag = SRSClients.clientlist(x).clientid
            Next
            uxClients.Text = "All"

            '------------
            REM Schedule ---------------------
            '-------------
            selectedClientName = uxClients.Text
            If selectedClientName = "All" Then
                selectedClientId = 0
            Else
                For x = 0 To SRS_Clients.clientlist.Count - 1
                    If SRS_Clients.clientlist(x).client_name = selectedClientName Then
                        selectedClientId = SRS_Clients.clientlist(x).clientid
                    End If
                Next
            End If

            'Load jobs Listview
            Dim lvew = New ListViewItem
            Me.uxLJobs.Items.Clear()
            For x = 0 To SRS_Jobs.joblist.Count - 1
                lvew = New ListViewItem(SRS_Jobs.joblist(x).checkname.ToString)
                lvew.Tag = SRS_Jobs.joblist(x).jobid
                lvew.ImageIndex = 6
                Me.uxLJobs.Items.Add(lvew)
            Next
            Me.uxLJobs.Items(0).Selected = True
            selectedJob = Me.uxLJobs.Items(0).Tag

            'Load jobs detail
            buildJobsDetail(selectedJob)

            'Load schedule grid
            selectedJob = SRS_Jobs.joblist(0).Jobid
            build_schedule(SRS_Actions, selectedJob, selectedClientId)
            SchedSave.Enabled = False
            SchedReset.Enabled = False
            uncomitted_data = False

            '------------
            REM Reporting ---------------------
            '------------

            'Load Jobs tree
            RefreshTJobs()
            uxTJobs.SelectedNode = uxTJobs.Nodes(0)
            uxTJobs.ExpandAll()
            uxTJobs.Select()

            If Not IsNothing(uxTJobs.SelectedNode.Parent) Then
                'Load Errors grid
                selectedActionId = uxTJobs.SelectedNode.Tag
                build_ErrorGrid(SRS_Errors, selectedActionId)

                'Load Stats grid
                build_StatsGrid(SRS_Stats, uxTJobs.SelectedNode.Tag, selectedClientId)
            End If

            '------------
            REM Client Email Detales ---------------------------------
            '------------

            'Load Client detail
            RefreshTClients()
            buildClientDetail(uxClientTree.Nodes(0).Tag)

            'Load email
            RefreshTEmail()
            uxEmailTree.ExpandAll()

            uxEmailTree.SelectedNode = uxEmailTree.Nodes(0)
            TreeClientid = uxEmailTree.SelectedNode.Tag
            build_EmailGrid(TreeClientid)

            REM set error Icon
            'If SRS_Errors.errorlist.Count > 0 Then
            '    uxErrorAlertIcon.Visible = True
            '    uxNoErrorsIcon.Visible = False
            '    uxReportText.Text = "Errors Reported"
            '    uxClear.Enabled = True
            'Else
            '    uxErrorAlertIcon.Visible = False
            '    uxNoErrorsIcon.Visible = True
            '    uxReportText.Text = ""
            '    uxClear.Enabled = False
            'End If


        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_cp_monitor", "load_view", bc_cs_error_codes.USER_DEFINED, ex.Message)
            load_view = False

        Finally
            load_view = True
            otrace = New bc_cs_activity_log("bc_am_cp_monitor", "load_view", bc_cs_activity_codes.TRACE_EXIT, "")
            refreshing = False

            Me.Cursor = Cursors.Default
        End Try

    End Function

    Public Sub build_EmailGrid(client_id As Long)

        'Load Email Address details grid

        Dim rowId As Integer

        Me.uxEmailDetails.Rows.Clear()
        For x = 0 To SRS_Emails.emaillist.Count - 1

            If (client_id = 0 Or client_id = SRS_Emails.emaillist(x).clientid) Then

                rowId = uxEmailDetails.Rows.Add(SRS_Emails.emaillist(x).name)
                'uxEmailDetails.Rows(rowId).ReadOnly = True

                uxEmailDetails.Rows(rowId).Cells("emailid").Value = SRS_Emails.emaillist(x).emailid
                uxEmailDetails.Rows(rowId).Cells("eclient").Value = SRS_Emails.emaillist(x).client_name
                uxEmailDetails.Rows(rowId).Cells("ename").Value = SRS_Emails.emaillist(x).name
                uxEmailDetails.Rows(rowId).Cells("edescription").Value = SRS_Emails.emaillist(x).description
                uxEmailDetails.Rows(rowId).Cells("eaddress").Value = SRS_Emails.emaillist(x).email

                If SRS_Emails.emaillist(x).onerror = 1 Then
                    uxEmailDetails.Rows(rowId).Cells("eonerror").Value = True
                Else
                    uxEmailDetails.Rows(rowId).Cells("eonerror").Value = False
                End If

                If SRS_Emails.emaillist(x).onstatuscheck = 1 Then
                    uxEmailDetails.Rows(rowId).Cells("eonstatuscheck").Value = True
                Else
                    uxEmailDetails.Rows(rowId).Cells("eonstatuscheck").Value = False
                End If

            End If
        Next

    End Sub

    Public Sub build_PrefGrid(clientemail_id As Long, client_name As String)

        'Load email filter grid

        Dim rowId As Integer

        Me.uxPrefDetails.Rows.Clear()
        For x = 0 To SRS_Emailfilters.emailfilter.Count - 1

            If (clientemail_id = SRS_Emailfilters.emailfilter(x).clientemailid) Then

                rowId = uxPrefDetails.Rows.Add(SRS_Emailfilters.emailfilter(x).checkname)

                uxPrefDetails.Rows(rowId).Cells("filterid").Value = SRS_Emailfilters.emailfilter(x).filterid
                uxPrefDetails.Rows(rowId).Cells("prefid").Value = SRS_Emailfilters.emailfilter(x).prefid
                uxPrefDetails.Rows(rowId).Cells("pclientemailid").Value = SRS_Emailfilters.emailfilter(x).clientemailid
                uxPrefDetails.Rows(rowId).Cells("pclient").Value = client_name
                'uxPrefDetails.Rows(rowId).Cells("pdescription").Value = SRS_Emailfilters.emailfilter(x).checkname
                uxPrefDetails.Rows(rowId).Cells("preftype").Value = SRS_Emailfilters.emailfilter(x).prfname

                If SRS_Emailfilters.emailfilter(x).prfname = "Severity" Then
                    uxPrefDetails.Rows(rowId).Cells("value1").Value = SRS_Emailfilters.emailfilter(x).value1
                    uxPrefDetails.Rows(rowId).Cells("pdescription").Value = "Filter by severity level"
                Else
                    uxPrefDetails.Rows(rowId).Cells("pdescription").Value = "Filter by Job"
                    uxPrefDetails.Rows(rowId).Cells("value1").Value = SRS_Emailfilters.emailfilter(x).checkname
                End If

                uxPrefDetails.Rows(rowId).Cells("value2").Value = SRS_Emailfilters.emailfilter(x).value2

                If SRS_Emailfilters.emailfilter(x).prfname = "Severity" Then
                    uxPrefDetails.Rows(rowId).Cells("value1").ReadOnly = False
                    uxPrefDetails.Rows(rowId).Cells("value2").ReadOnly = False
                    'uxPrefDetails.Rows(rowId).Cells("pdescription").ReadOnly = True
                Else
                    uxPrefDetails.Rows(rowId).Cells("value1").ReadOnly = False
                    uxPrefDetails.Rows(rowId).Cells("value2").ReadOnly = True
                    'uxPrefDetails.Rows(rowId).Cells("pdescription").ReadOnly = False
                End If
            End If
        Next

    End Sub


    Public Sub buildClientDetail(selectedClientId As Long)
        ''Load jobs grid
        Dim rowId As Integer

        Me.uxClientDetails.Rows.Clear()
        For x = 0 To SRS_Clients.clientlist.Count - 1

            If selectedClientId <> 0 And selectedClientId = SRS_Clients.clientlist(x).clientid Then

                rowId = uxClientDetails.Rows.Add("Client Id")
                uxClientDetails.Rows(rowId).ReadOnly = True
                uxClientDetails.Rows(rowId).Cells("cvalue").Value = SRS_Clients.clientlist(x).clientid.ToString

                rowId = uxClientDetails.Rows.Add("Client Name")
                uxClientDetails.Rows(rowId).ReadOnly = True
                uxClientDetails.Rows(rowId).Cells("cvalue").Value = SRS_Clients.clientlist(x).client_name

                rowId = uxClientDetails.Rows.Add("Server Name")
                'uxClientDetails.Rows(rowId).ReadOnly = True
                uxClientDetails.Rows(rowId).Cells("cvalue").Value = SRS_Clients.clientlist(x).server_name

                rowId = uxClientDetails.Rows.Add("DB Name")
                'uxClientDetails.Rows(rowId).ReadOnly = True
                uxClientDetails.Rows(rowId).Cells("cvalue").Value = SRS_Clients.clientlist(x).db_name

                rowId = uxClientDetails.Rows.Add("DB Server")
                uxClientDetails.Rows(rowId).Cells("cvalue").Value = SRS_Clients.clientlist(x).db_server

                rowId = uxClientDetails.Rows.Add("Acount")
                'uxClientDetails.Rows(rowId).ReadOnly = True
                uxClientDetails.Rows(rowId).Cells("cvalue").Value = SRS_Clients.clientlist(x).conn_account

                rowId = uxClientDetails.Rows.Add("URL")
                'uxClientDetails.Rows(rowId).ReadOnly = True
                uxClientDetails.Rows(rowId).Cells("cvalue").Value = SRS_Clients.clientlist(x).url

                rowId = uxClientDetails.Rows.Add("From Email")
                'uxClientDetails.Rows(rowId).ReadOnly = True
                uxClientDetails.Rows(rowId).Cells("cvalue").Value = SRS_Clients.clientlist(x).from_email

                rowId = uxClientDetails.Rows.Add("Active")
                'uxClientDetails.Rows(rowId).ReadOnly = True
                If SRS_Clients.clientlist(x).active = 1 Then
                    uxClientDetails.Rows(rowId).Cells("cvalue").Value = True
                Else
                    uxClientDetails.Rows(rowId).Cells("cvalue").Value = False
                End If

            End If
        Next

    End Sub

    Public Sub buildJobsDetail(selectedJob As Long)
        ''Load jobs grid
        Dim rowId As Integer

        Me.uxJobsGrid.Rows.Clear()
        For x = 0 To SRS_Jobs.joblist.Count - 1

            If selectedJob = SRS_Jobs.joblist(x).jobid Then

                rowId = uxJobsGrid.Rows.Add("Job Id")
                'uxJobsGrid.Rows(rowId).ReadOnly = True
                uxJobsGrid.Rows(rowId).Cells("value").Value = SRS_Jobs.joblist(x).jobid
                uxJobsGrid.Rows(rowId).Visible = False

                rowId = uxJobsGrid.Rows.Add("Job Name")
                uxJobsGrid.Rows(rowId).ReadOnly = True
                uxJobsGrid.Rows(rowId).Cells("value").Value = SRS_Jobs.joblist(x).checkname

                rowId = uxJobsGrid.Rows.Add("Description")
                'uxJobsGrid.Rows(rowId).ReadOnly = True
                uxJobsGrid.Rows(rowId).Cells("value").Value = SRS_Jobs.joblist(x).checkdecscription

                rowId = uxJobsGrid.Rows.Add("Check Type")
                'uxJobsGrid.Rows(rowId).ReadOnly = True
                uxJobsGrid.Rows(rowId).Cells("value").Value = SRS_Jobs.joblist(x).type_name

                rowId = uxJobsGrid.Rows.Add("Fault Check")
                'uxJobsGrid.Rows(rowId).ReadOnly = True
                If SRS_Jobs.joblist(x).faultcheck = 1 Then
                    uxJobsGrid.Rows(rowId).Cells("value").Value = True
                Else
                    uxJobsGrid.Rows(rowId).Cells("value").Value = False
                End If

                rowId = uxJobsGrid.Rows.Add("Status Check")
                'uxJobsGrid.Rows(rowId).ReadOnly = True
                If SRS_Jobs.joblist(x).statuscheck = 1 Then
                    uxJobsGrid.Rows(rowId).Cells("value").Value = True
                Else
                    uxJobsGrid.Rows(rowId).Cells("value").Value = False
                End If

                rowId = uxJobsGrid.Rows.Add("Error Text")
                'uxJobsGrid.Rows(rowId).ReadOnly = True
                uxJobsGrid.Rows(rowId).Cells("value").Value = SRS_Jobs.joblist(x).errortext

                rowId = uxJobsGrid.Rows.Add("Default Severity")
                'uxJobsGrid.Rows(rowId).ReadOnly = True
                uxJobsGrid.Rows(rowId).Cells("value").Value = SRS_Jobs.joblist(x).errorseverity

                rowId = uxJobsGrid.Rows.Add("Stored Procedure")
                'uxJobsGrid.Rows(rowId).ReadOnly = True
                uxJobsGrid.Rows(rowId).Cells("value").Value = SRS_Jobs.joblist(x).storedproc

                rowId = uxJobsGrid.Rows.Add("Service to test")
                'uxJobsGrid.Rows(rowId).ReadOnly = True
                uxJobsGrid.Rows(rowId).Cells("value").Value = SRS_Jobs.joblist(x).Service

                rowId = uxJobsGrid.Rows.Add("User Name")
                ' uxJobsGrid.Rows(rowId).ReadOnly = True
                uxJobsGrid.Rows(rowId).Cells("value").Value = SRS_Jobs.joblist(x).username

                rowId = uxJobsGrid.Rows.Add("User Password")
                uxJobsGrid.Rows(rowId).ReadOnly = True
                uxJobsGrid.Rows(rowId).Cells("value").Value = SRS_Jobs.joblist(x).userpassword

                rowId = uxJobsGrid.Rows.Add("Restart Procedure")
                'uxJobsGrid.Rows(rowId).ReadOnly = True
                uxJobsGrid.Rows(rowId).Cells("value").Value = SRS_Jobs.joblist(x).restartproc
            End If
        Next

    End Sub


    Public Sub build_schedule(SRSAction As bc_srs_actionlist, Jobid As Long, clientId As Long)

        Dim rowId As Integer
        Me.uxSheduleGrid.Rows.Clear()

        'Load schedule grid
        For x = 0 To SRSAction.actionlist.Count - 1

            If Jobid = SRSAction.actionlist(x).jobid And (clientId = 0 Or clientId = SRSAction.actionlist(x).clientid) Then

                rowId = uxSheduleGrid.Rows.Add(SRSAction.actionlist(x).checkname)

                uxSheduleGrid.Rows(rowId).Cells("schedid").Value = SRSAction.actionlist(x).scheduleid
                uxSheduleGrid.Rows(rowId).Cells("clientname").Value = SRSAction.actionlist(x).client_name
                uxSheduleGrid.Rows(rowId).Cells("clientname").ReadOnly = True
                uxSheduleGrid.Rows(rowId).Cells("JobNameS").Value = SRSAction.actionlist(x).checkname
                uxSheduleGrid.Rows(rowId).Cells("ItemName").Value = SRSAction.actionlist(x).item_name
                uxSheduleGrid.Rows(rowId).Cells("Frequency").Value = SRSAction.actionlist(x).Frequency

                If SRSAction.actionlist(x).active = 1 Then
                    uxSheduleGrid.Rows(rowId).Cells("active").Value = True
                Else
                    uxSheduleGrid.Rows(rowId).Cells("active").Value = False
                End If

                If Format(SRSAction.actionlist(x).scheduletime, "hh:mm:ss") <> "12:00:00" Then
                    uxSheduleGrid.Rows(rowId).Cells("scheduletime").Value = SRSAction.actionlist(x).scheduletime
                End If

                If Format(SRSAction.actionlist(x).monitorstarttime, "hh:mm:ss") <> "12:00:00" Then
                    uxSheduleGrid.Rows(rowId).Cells("monitorstarttime").Value = SRSAction.actionlist(x).monitorstarttime
                    uxSheduleGrid.Rows(rowId).Cells("monitorendtime").Value = SRSAction.actionlist(x).monitorendtime
                End If

                If SRSAction.actionlist(x).monitormonday = 1 Then
                    uxSheduleGrid.Rows(rowId).Cells("monitormonday").Value = True
                Else
                    uxSheduleGrid.Rows(rowId).Cells("monitormonday").Value = False
                End If

                If SRSAction.actionlist(x).monitortuesday = 1 Then
                    uxSheduleGrid.Rows(rowId).Cells("monitortuesday").Value = True
                Else
                    uxSheduleGrid.Rows(rowId).Cells("monitortuesday").Value = False
                End If

                If SRSAction.actionlist(x).monitorwednesday = 1 Then
                    uxSheduleGrid.Rows(rowId).Cells("monitorwednesday").Value = True
                Else
                    uxSheduleGrid.Rows(rowId).Cells("monitorwednesday").Value = False
                End If

                If SRSAction.actionlist(x).monitorthursday = 1 Then
                    uxSheduleGrid.Rows(rowId).Cells("monitorthursday").Value = True
                Else
                    uxSheduleGrid.Rows(rowId).Cells("monitorthursday").Value = False
                End If

                If SRSAction.actionlist(x).monitorfriday = 1 Then
                    uxSheduleGrid.Rows(rowId).Cells("monitorfriday").Value = True
                Else
                    uxSheduleGrid.Rows(rowId).Cells("monitorfriday").Value = False
                End If

                If SRSAction.actionlist(x).monitorsaturday = 1 Then
                    uxSheduleGrid.Rows(rowId).Cells("monitorsaturday").Value = True
                Else
                    uxSheduleGrid.Rows(rowId).Cells("monitorsaturday").Value = False
                End If

                If SRSAction.actionlist(x).monitorsunday = 1 Then
                    uxSheduleGrid.Rows(rowId).Cells("monitorsunday").Value = True
                Else
                    uxSheduleGrid.Rows(rowId).Cells("monitorsunday").Value = False
                End If
                uxSheduleGrid.Rows(rowId).Cells("errorfrequency").Value = SRSAction.actionlist(x).errorfrequency
                uxSheduleGrid.Rows(rowId).Cells("restartattemps").Value = SRSAction.actionlist(x).restartattemps
                uxSheduleGrid.Rows(rowId).Cells("restartfrequency").Value = SRSAction.actionlist(x).restartfrequency
            End If
        Next

    End Sub

    Public Sub build_ErrorGrid(SRSErrors As bc_srs_errorlist, schedule_id As Long)

        'Load Errors grid
        Dim rowId As Integer
        Dim errorsFound As Boolean = False
        Dim lasterrorId As Long = 0
        Dim lasterrorDate As Date

        For x = 0 To SRS_Stats.statlist.Count - 1
            If schedule_id = SRS_Stats.statlist(x).scheduleid Then
                If SRS_Stats.statlist(x).errorsfound = 1 Then
                    errorsFound = True
                Else
                    errorsFound = False
                End If
                lasterrorId = SRS_Stats.statlist(x).lasterrorid
                lasterrorDate = SRS_Stats.statlist(x).lasterrordate
            End If
        Next

        Me.uxErrorGrid.Rows.Clear()
        uxClear.Enabled = False
        For x = 0 To SRSErrors.errorlist.Count - 1

            If schedule_id = SRSErrors.errorlist(x).scheduleid Then
                rowId = uxErrorGrid.Rows.Add(SRSErrors.errorlist(x).errorid)
                uxErrorGrid.Rows(rowId).ReadOnly = True
                uxErrorGrid.Rows(rowId).Cells("errordate").Value = SRSErrors.errorlist(x).errordate.ToLocalTime()
                uxErrorGrid.Rows(rowId).Cells("srserrortext").Value = SRSErrors.errorlist(x).errordescription
                uxErrorGrid.Rows(rowId).Cells("severity").Value = SRSErrors.errorlist(x).severity

                If errorsFound = True And lasterrorId <> 0 Then
                    If SRSErrors.errorlist(x).errordate >= lasterrorDate Then
                        uxErrorGrid.Rows(rowId).DefaultCellStyle.BackColor = Color.Tomato
                        uxClear.Enabled = True
                    End If
                End If
            End If
        Next

        If uxErrorGrid.SelectedRows.Count > 0 Then
            If uxErrorGrid.SelectedRows(0).DefaultCellStyle.BackColor = Color.Tomato Then
                uxErrorGrid.RowsDefaultCellStyle.SelectionBackColor = Color.Red
            Else
                uxErrorGrid.RowsDefaultCellStyle.SelectionBackColor = Color.Empty
            End If
        End If

    End Sub

    Public Sub build_StatsGrid(SRSStats As bc_srs_statlist, schedule_id As Long, client_id As Long)

        'Load Stats grid

        Dim rowId As Integer

        Me.uxStatGrid.Rows.Clear()
        For x = 0 To SRSStats.statlist.Count - 1

            If schedule_id = SRSStats.statlist(x).scheduleid And (client_id = 0 Or client_id = SRSStats.statlist(x).clientid) Then
                rowId = uxStatGrid.Rows.Add(SRSStats.statlist(x).itemname)
                uxStatGrid.Rows(rowId).ReadOnly = True

                If SRSStats.statlist(x).lastrundate <> "01-01-1900" Then
                    uxStatGrid.Rows(rowId).Cells("lastrundate").Value = SRSStats.statlist(x).lastrundate.ToLocalTime()
                End If

                If SRSStats.statlist(x).errorsfound = 1 Then
                    uxStatGrid.Rows(rowId).Cells("errorfound").Value = True
                Else
                    uxStatGrid.Rows(rowId).Cells("errorfound").Value = False
                End If

                If SRSStats.statlist(x).lasterrordate <> "01-01-1900" Then
                    uxStatGrid.Rows(rowId).Cells("lasterrordate").Value = SRSStats.statlist(x).lasterrordate.ToLocalTime()
                End If
                uxStatGrid.Rows(rowId).Cells("lasterrorid").Value = SRSStats.statlist(x).lasterrorid

                If SRSStats.statlist(x).lastrestart <> "01-01-1900" Then
                    uxStatGrid.Rows(rowId).Cells("lastrestart").Value = SRSStats.statlist(x).lastrestart.ToLocalTime()
                End If
                uxStatGrid.Rows(rowId).Cells("restartresult").Value = SRSStats.statlist(x).restartresult
                uxStatGrid.Rows(rowId).Cells("restartcount").Value = SRSStats.statlist(x).restartcount

                uxStatGrid.Rows(rowId).Tag = SRSStats.statlist(x).stateid
            End If
        Next

    End Sub

    Public Sub build_JobGrid(SRSJobs As bc_srs_joblist, job_id As Long)

        'Load Job grid
        Dim rowId As Integer

        Me.uxGridJob.Rows.Clear()
        For x = 0 To SRSJobs.joblist.Count - 1

            If job_id = SRSJobs.joblist(x).jobid Then
                rowId = uxGridJob.Rows.Add(SRSJobs.joblist(x).checkname)
                uxGridJob.Rows(rowId).ReadOnly = True
                uxGridJob.Rows(rowId).Cells("jobdescription").Value = SRSJobs.joblist(x).checkdecscription
            End If
        Next

    End Sub

    Public Sub RefreshTJobs()

        Dim clientid As Long
        Dim clientNode As TreeNode = Nothing
        Dim jobNode As TreeNode = Nothing
        Dim schedNode As TreeNode = Nothing
        Dim currentCollection As String
        Dim clientnodeid As Integer

        uxTJobs.Nodes.Clear()
        currentCollection = ""

        For i = 0 To SRS_Clients.clientlist.Count - 1

            'TreeView Parents
            If (selectedClientId = 0 Or selectedClientId = SRS_Clients.clientlist(i).clientid) Then
                clientNode = New TreeNode
                clientNode.Text = SRS_Clients.clientlist(i).client_name
                clientNode.Tag = SRS_Clients.clientlist(i).clientid
                uxTJobs.Nodes.Add(clientNode)
                clientNode.ImageIndex = 15
                clientid = SRS_Clients.clientlist(i).clientid
                clientnodeid = clientNode.Index

                'TreeView Children
                For x = 0 To SRS_Actions.actionlist.Count - 1
                    If SRS_Actions.actionlist(x).clientid = clientid And SRS_Actions.actionlist(x).active = 1 Then
                        schedNode = New TreeNode
                        schedNode.Text = SRS_Actions.actionlist(x).item_name
                        schedNode.Tag = SRS_Actions.actionlist(x).scheduleid
                        If SRS_Actions.actionlist(x).errorfound = 1 Then
                            schedNode.ImageIndex = 16
                        Else
                            schedNode.ImageIndex = 14
                        End If
                        uxTJobs.Nodes(clientnodeid).Nodes.Add(schedNode)
                    End If
                Next
            End If
        Next

    End Sub


    Public Sub RefreshTEmail()

        Dim clientid As Long
        Dim emailID As Long
        Dim clientnodeid As Integer

        Dim emailnodeid As Integer
        Dim emailNode As TreeNode = Nothing
        Dim jobNode As TreeNode = Nothing
        Dim filterNode As TreeNode = Nothing
        Dim clientNode As TreeNode = Nothing
        Dim currentCollection As String

        uxEmailTree.Nodes.Clear()
        currentCollection = ""

        For j = 0 To SRS_Clients.clientlist.Count - 1

            'TreeView Parents

            If (selectedClientId = 0 Or selectedClientId = SRS_Clients.clientlist(j).clientid) Then

                'TreeView Clients (SRS Clients)
                clientNode = New TreeNode
                clientNode.Text = SRS_Clients.clientlist(j).client_name
                clientNode.Tag = SRS_Clients.clientlist(j).clientid
                uxEmailTree.Nodes.Add(clientNode)
                clientNode.ImageIndex = 15
                clientid = SRS_Clients.clientlist(j).clientid
                clientnodeid = clientNode.Index

                For i = 0 To SRS_Emails.emaillist.Count - 1

                    'TreeView Parents (Email addresses)
                    If (clientid = SRS_Emails.emaillist(i).clientid) Then
                        emailNode = New TreeNode
                        emailNode.Text = SRS_Emails.emaillist(i).name
                        emailNode.Tag = SRS_Emails.emaillist(i).emailid
                        uxEmailTree.Nodes(clientnodeid).Nodes.Add(emailNode)
                        emailNode.ImageIndex = 7
                        emailID = SRS_Emails.emaillist(i).emailid
                        emailnodeid = emailNode.Index

                        'TreeView Children (Email filters)
                        For x = 0 To SRS_Emailfilters.emailfilter.Count - 1
                            If SRS_Emailfilters.emailfilter(x).clientemailid = emailID Then

                                filterNode = New TreeNode
                                If SRS_Emailfilters.emailfilter(x).preftype = "RANGE" Then
                                    filterNode.Text = SRS_Emailfilters.emailfilter(x).prfname + " " + SRS_Emailfilters.emailfilter(x).value1 + " To " + SRS_Emailfilters.emailfilter(x).value2
                                Else
                                    filterNode.Text = SRS_Emailfilters.emailfilter(x).prfname + " " + SRS_Emailfilters.emailfilter(x).value1 + " " + SRS_Emailfilters.emailfilter(x).checkname
                                End If

                                filterNode.Tag = SRS_Emailfilters.emailfilter(x).prefid
                                filterNode.ImageIndex = 1
                                uxEmailTree.Nodes(clientnodeid).Nodes(emailnodeid).Nodes.Add(filterNode)
                            End If
                        Next
                    End If

                Next
            End If
        Next
    End Sub


    Public Sub RefreshTClients()

        Dim clientid As Long
        Dim clientnodeid As Integer
        Dim emailNode As TreeNode = Nothing
        Dim jobNode As TreeNode = Nothing
        Dim filterNode As TreeNode = Nothing
        Dim clientNode As TreeNode = Nothing
        Dim currentCollection As String

        uxClientTree.Nodes.Clear()
        currentCollection = ""

        For j = 0 To SRS_Clients.clientlist.Count - 1
            'TreeView Parents
            If (selectedClientId = 0 Or selectedClientId = SRS_Clients.clientlist(j).clientid) Then
                clientNode = New TreeNode
                clientNode.Text = SRS_Clients.clientlist(j).client_name
                clientNode.Tag = SRS_Clients.clientlist(j).clientid
                uxClientTree.Nodes.Add(clientNode)
                clientNode.ImageIndex = 15
                clientid = SRS_Clients.clientlist(j).clientid
                clientnodeid = clientNode.Index
            End If
        Next

        uxClientTree.SelectedNode = uxClientTree.Nodes(0)

    End Sub


    Private Sub uxClear_Click(sender As Object, e As EventArgs) Handles uxClear.Click
        Dim statsID As Long
        Dim sclientId As Long = 0
        Dim selectedScheduleid As Long = 0
        Dim sjobid As Long = 0
        Dim selectedNodeText As String = ""
        Dim selectedNodeTag As Long = 0

        Me.Cursor = Cursors.WaitCursor
        Me.Enabled = False

        If uxStatGrid.SelectedRows.Count > 0 Then
            statsID = uxStatGrid.SelectedRows(0).Tag

            For x = 0 To SRS_Stats.statlist.Count - 1
                If SRS_Stats.statlist(x).stateid = statsID Then
                    SRS_Stats.statlist(x).errorsfound = 0
                    SRS_Stats.statlist(x).restartcount = 0

                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        SRS_Stats.statlist(x).write_mode = 1
                        SRS_Stats.statlist(x).db_write()
                    Else
                        SRS_Stats.statlist(x).tmode = bc_srs_client.tWRITE
                        SRS_Stats.statlist(x).write_mode = bc_srs_client.UPDATE
                        If SRS_Stats.statlist(x).transmit_to_server_and_receive(SRS_Stats.statlist(x), True) = False Then
                            Exit Sub
                        End If
                    End If

                End If
            Next

            selectedNodeText = uxTJobs.SelectedNode.Text
            selectedNodeTag = uxTJobs.SelectedNode.Tag

            'Refresh objects
            RaiseEvent refresh()

            'refresh tree view 
            RefreshTJobs()
            uxTJobs.ExpandAll()

            REM reselect node
            Dim n As TreeNode
            Dim tn As TreeNode

            For Each n In uxTJobs.Nodes
                If n.Text = selectedNodeText And n.Tag = selectedNodeTag Then
                    uxTJobs.SelectedNode = n
                End If
                For Each tn In n.Nodes
                    If tn.Text = selectedNodeText And tn.Tag = selectedNodeTag Then
                        uxTJobs.SelectedNode = tn
                    End If
                Next
            Next

            If IsNothing(uxTJobs.SelectedNode.Parent) Then
                sclientId = uxTJobs.SelectedNode.Tag
                Me.uxGridJob.Rows.Clear()
                Me.uxStatGrid.Rows.Clear()
                Me.uxErrorGrid.Rows.Clear()

            Else
                selectedScheduleid = uxTJobs.SelectedNode.Tag
                sclientId = uxTJobs.SelectedNode.Parent.Tag

                For x = 0 To SRS_Actions.actionlist.Count - 1
                    If SRS_Actions.actionlist(x).scheduleid = selectedScheduleid Then
                        sjobid = SRS_Actions.actionlist(x).jobid
                    End If
                Next

                'load Job grid
                build_JobGrid(SRS_Jobs, sjobid)

                'Load Errors grid
                build_ErrorGrid(SRS_Errors, selectedScheduleid)

                'Load Stats grid
                build_StatsGrid(SRS_Stats, selectedScheduleid, sclientId)

            End If

            uxTJobs.ExpandAll()
            uxTJobs.Select()

            Me.Enabled = True
            Me.Cursor = Cursors.Default

        End If
    End Sub


    Private Sub uxRefresh_Click(sender As Object, e As EventArgs) Handles uxRefresh.Click
        Dim sjobid As Long = 0
        Dim sclientId As Long = 0
        Dim selectedScheduleid As Long = 0
        Dim selectedNodeText As String = ""
        Dim selectedNodeTag As Long = 0

        Me.Cursor = Cursors.WaitCursor
        Me.Enabled = False

        selectedNodeText = uxTJobs.SelectedNode.Text
        selectedNodeTag = uxTJobs.SelectedNode.Tag

        'Refresh objects
        RaiseEvent refresh()

        'refresh tree view 
        RefreshTJobs()
        uxTJobs.ExpandAll()

        REM reselect node
        Dim n As TreeNode
        Dim tn As TreeNode

        For Each n In uxTJobs.Nodes
            If n.Text = selectedNodeText And n.Tag = selectedNodeTag Then
                uxTJobs.SelectedNode = n
            End If
            For Each tn In n.Nodes
                If tn.Text = selectedNodeText And tn.Tag = selectedNodeTag Then
                    uxTJobs.SelectedNode = tn
                End If
            Next
        Next


        If IsNothing(uxTJobs.SelectedNode.Parent) Then
            sclientId = uxTJobs.SelectedNode.Tag
            Me.uxGridJob.Rows.Clear()
            Me.uxStatGrid.Rows.Clear()
            Me.uxErrorGrid.Rows.Clear()

        Else
            selectedScheduleid = uxTJobs.SelectedNode.Tag
            sclientId = uxTJobs.SelectedNode.Parent.Tag

            For x = 0 To SRS_Actions.actionlist.Count - 1
                If SRS_Actions.actionlist(x).scheduleid = selectedScheduleid Then
                    sjobid = SRS_Actions.actionlist(x).jobid
                End If
            Next

            'load Job grid
            build_JobGrid(SRS_Jobs, sjobid)

            'Load Errors grid
            build_ErrorGrid(SRS_Errors, selectedScheduleid)

            'Load Stats grid
            build_StatsGrid(SRS_Stats, selectedScheduleid, sclientId)

        End If

        uxTJobs.ExpandAll()
        uxTJobs.Select()

        Me.Enabled = True
        Me.Cursor = Cursors.Default

    End Sub

    Private Sub uxJobsGrid_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles uxJobsGrid.CellBeginEdit

        REM set dirty flag
        Dim selectedJob As Long

        selectedJob = Me.uxLJobs.SelectedItems.Item(0).Tag

        For x = 0 To SRS_Jobs.joblist.Count - 1
            If SRS_Jobs.joblist(x).jobid = selectedJob Then
                SRS_Jobs.joblist(x).dirtyrecord = True
            End If
        Next

        uxClients.Enabled = False
        uxLJobs.Enabled = False
        uxSheduleGrid.Enabled = False
        SchedSave.Enabled = True
        SchedReset.Enabled = True
        uncomitted_data = True
    End Sub

    Private Sub uxLJobs_SelectedIndexChanged(sender As Object, e As EventArgs) Handles uxLJobs.SelectedIndexChanged

        Dim selectedJob As Long

        'Load schedule grid
        Me.refreshing = True
        If uxLJobs.SelectedItems.Count = 1 Then
            selectedJob = uxLJobs.SelectedItems.Item(0).Tag
            build_schedule(SRS_Actions, selectedJob, selectedClientId)
            buildJobsDetail(selectedJob)

            srsContextMenuStrip1.Items("DeleteItem").Visible = False
            If uxSheduleGrid.RowCount > 0 Then
                srsContextMenuStrip1.Items("DeleteItem").Visible = True
            End If


        End If
        Me.refreshing = False

    End Sub

    Private Sub uxClients_SelectedIndexChanged(sender As Object, e As EventArgs) Handles uxClients.SelectedIndexChanged

        Dim selectedClientName As String
        Dim selectedJob As Long

        selectedClientName = uxClients.Text
        If selectedClientName = "All" Then
            selectedClientId = 0
        Else
            For x = 0 To SRS_Clients.clientlist.Count - 1
                If SRS_Clients.clientlist(x).client_name = selectedClientName Then
                    selectedClientId = SRS_Clients.clientlist(x).clientid
                End If
            Next
        End If

        If uxLJobs.Items.Count > 0 Then

            'Load Jobs tree
            RefreshTJobs()
            uxTJobs.SelectedNode = uxTJobs.Nodes(0)
            uxTJobs.ExpandAll()
            uxTJobs.Select()

            'Load schedule grid
            selectedJob = uxLJobs.Items(0).Tag
            build_schedule(SRS_Actions, selectedJob, selectedClientId)

            'Load Client detail
            RefreshTClients()
            buildClientDetail(uxClientTree.Nodes(0).Tag)

            RefreshTEmail()
            uxEmailTree.ExpandAll()
            build_EmailGrid(uxEmailTree.Nodes(0).Tag)

        End If

    End Sub


    Private Sub uxErrorGrid_SelectionChanged(sender As Object, e As EventArgs) Handles uxErrorGrid.SelectionChanged

        If uxErrorGrid.SelectedRows.Count = 1 Then

            If uxErrorGrid.SelectedRows(0).DefaultCellStyle.BackColor = Color.Tomato Then
                uxErrorGrid.RowsDefaultCellStyle.SelectionBackColor = Color.Red
            Else
                uxErrorGrid.RowsDefaultCellStyle.SelectionBackColor = Color.Empty
            End If
        End If

    End Sub

    Private Sub uxClientTree_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles uxClientTree.AfterSelect

        buildClientDetail(uxClientTree.SelectedNode.Tag)

    End Sub


    Private Sub SchedSave_Click(sender As Object, e As EventArgs) Handles SchedSave.Click

        Dim uxSheduleGridRow As DataGridViewRow
        Dim schedid As Long
        Dim jobid, clientid As Long
        Dim oinput As bc_action_item

        Dim selectedJob As Long
        Dim jobcheckname As String
        Dim jobinput As bc_srs_job

        'uxClients.Enabled = True
        'uxLJobs.Enabled = True
        'uxJobsGrid.Enabled = True
        'uxSheduleGrid.Enabled = True
        'SchedSave.Enabled = False
        'SchedReset.Enabled = False
        'uncomitted_data = False

        REM save Job to database
        If Me.uxLJobs.SelectedItems.Count > 0 Then

            selectedJob = Me.uxLJobs.SelectedItems.Item(0).Tag
            jobcheckname = uxLJobs.SelectedItems.Item(0).Text

            For Each jobinput In SRS_Jobs.joblist
                If jobinput.checkname = jobcheckname And jobinput.dirtyrecord = True Then

                    jobinput.jobid = selectedJob
                    jobinput.checkdecscription = uxJobsGrid.Rows(2).Cells("value").Value
                    jobinput.type_name = uxJobsGrid.Rows(3).Cells("value").Value

                    If uxJobsGrid.Rows(4).Cells("value").Value = True Then
                        jobinput.faultcheck = 1
                    Else
                        jobinput.faultcheck = 0
                    End If
                    If uxJobsGrid.Rows(5).Cells("value").Value = True Then
                        jobinput.statuscheck = 1
                    Else
                        jobinput.statuscheck = 0
                    End If
                    jobinput.errortext = uxJobsGrid.Rows(6).Cells("value").Value
                    jobinput.errorseverity = uxJobsGrid.Rows(7).Cells("value").Value
                    jobinput.storedproc = uxJobsGrid.Rows(8).Cells("value").Value
                    jobinput.service = uxJobsGrid.Rows(9).Cells("value").Value
                    jobinput.username = uxJobsGrid.Rows(10).Cells("value").Value
                    jobinput.userpassword = uxJobsGrid.Rows(11).Cells("value").Value
                    jobinput.restartproc = uxJobsGrid.Rows(12).Cells("value").Value

                    'If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    '    jobinput.write_mode = bc_schedule_item.UPDATE
                    '    jobinput.db_write()
                    'Else
                    '    jobinput.write_mode = bc_schedule_item.UPDATE
                    '    If jobinput.transmit_to_server_and_receive(jobinput, True) = False Then
                    '        Exit Sub
                    '    End If
                    'End If

                End If
            Next
        End If




        REM save schedule
        REM set values into the object
        For Each uxSheduleGridRow In uxSheduleGrid.Rows
            schedid = uxSheduleGridRow.Cells("schedid").Value
            If schedid <> 0 Then
                REM Record Updated
                For x = 0 To SRS_Actions.actionlist.Count - 1
                    If schedid = SRS_Actions.actionlist(x).scheduleid Then

                        SRS_Actions.actionlist(x).client_name = uxSheduleGridRow.Cells("clientname").Value
                        SRS_Actions.actionlist(x).checkname = uxSheduleGridRow.Cells("JobNameS").Value
                        SRS_Actions.actionlist(x).item_name = uxSheduleGridRow.Cells("ItemName").Value
                        SRS_Actions.actionlist(x).Frequency = uxSheduleGridRow.Cells("Frequency").Value

                        If uxSheduleGridRow.Cells("active").Value = True Then
                            SRS_Actions.actionlist(x).active = 1
                        Else
                            SRS_Actions.actionlist(x).active = 0
                        End If

                        If Not uxSheduleGridRow.Cells("scheduletime").Value = Nothing Then
                            SRS_Actions.actionlist(x).scheduletime = uxSheduleGridRow.Cells("scheduletime").Value
                        End If
                        If Not uxSheduleGridRow.Cells("monitorstarttime").Value = Nothing Then
                            SRS_Actions.actionlist(x).monitorstarttime = uxSheduleGridRow.Cells("monitorstarttime").Value
                        End If
                        If Not uxSheduleGridRow.Cells("monitorendtime").Value = Nothing Then
                            SRS_Actions.actionlist(x).monitorendtime = uxSheduleGridRow.Cells("monitorendtime").Value
                        End If

                        If uxSheduleGridRow.Cells("monitormonday").Value = True Then
                            SRS_Actions.actionlist(x).monitormonday = 1
                        Else
                            SRS_Actions.actionlist(x).monitormonday = 0
                        End If

                        If uxSheduleGridRow.Cells("monitortuesday").Value = True Then
                            SRS_Actions.actionlist(x).monitortuesday = 1
                        Else
                            SRS_Actions.actionlist(x).monitortuesday = 0
                        End If

                        If uxSheduleGridRow.Cells("monitorwednesday").Value = True Then
                            SRS_Actions.actionlist(x).monitorwednesday = 1
                        Else
                            SRS_Actions.actionlist(x).monitorwednesday = 0
                        End If

                        If uxSheduleGridRow.Cells("monitorthursday").Value = True Then
                            SRS_Actions.actionlist(x).monitorthursday = 1
                        Else
                            SRS_Actions.actionlist(x).monitorthursday = 0
                        End If

                        If uxSheduleGridRow.Cells("monitorfriday").Value = True Then
                            SRS_Actions.actionlist(x).monitorfriday = 1
                        Else
                            SRS_Actions.actionlist(x).monitorfriday = 0
                        End If

                        If uxSheduleGridRow.Cells("monitorsaturday").Value = True Then
                            SRS_Actions.actionlist(x).monitorsaturday = 1
                        Else
                            SRS_Actions.actionlist(x).monitorsaturday = 0
                        End If

                        If uxSheduleGridRow.Cells("monitorsunday").Value = True Then
                            SRS_Actions.actionlist(x).monitorsunday = 1
                        Else
                            SRS_Actions.actionlist(x).monitorsunday = 0
                        End If

                        SRS_Actions.actionlist(x).errorfrequency = uxSheduleGridRow.Cells("errorfrequency").Value

                        SRS_Actions.actionlist(x).restartattemps = uxSheduleGridRow.Cells("restartattemps").Value
                        SRS_Actions.actionlist(x).restartfrequency = uxSheduleGridRow.Cells("restartfrequency").Value

                        SRS_Actions.actionlist(x).dirtyrecord = True

                    End If
                Next x
            Else
                REM New record added
                REM get clientid
                For x = 0 To SRS_Clients.clientlist.Count - 1
                    If uxSheduleGridRow.Cells("clientname").Value = SRS_Clients.clientlist(x).client_name Then
                        clientid = SRS_Clients.clientlist(x).clientid
                    End If
                Next

                REM Get jobid
                For x = 0 To SRS_Jobs.joblist.Count - 1
                    If uxSheduleGridRow.Cells("JobNameS").Value = SRS_Jobs.joblist(x).checkname Then
                        jobid = SRS_Jobs.joblist(x).jobid
                    End If
                Next

                oinput = New bc_action_item()

                REM Save back to object
                oinput.scheduleid = schedid
                oinput.clientid = clientid
                oinput.jobid = jobid
                oinput.item_name = uxSheduleGridRow.Cells("ItemName").Value
                oinput.client_name = uxSheduleGridRow.Cells("clientname").Value
                oinput.checkname = uxSheduleGridRow.Cells("JobNameS").Value
                oinput.item_name = uxSheduleGridRow.Cells("ItemName").Value
                oinput.frequency = uxSheduleGridRow.Cells("Frequency").Value

                If uxSheduleGridRow.Cells("active").Value = True Then
                    oinput.active = 1
                Else
                    oinput.active = 0
                End If

                If IsDBNull(uxSheduleGridRow.Cells("scheduletime").Value) OrElse IsNothing(uxSheduleGridRow.Cells("scheduletime").Value) Then
                    oinput.scheduletime = "01Jan1900 00:00:00"
                Else
                    oinput.scheduletime = uxSheduleGridRow.Cells("scheduletime").Value
                End If

                If IsDBNull(uxSheduleGridRow.Cells("monitorstarttime").Value) OrElse IsNothing(uxSheduleGridRow.Cells("monitorstarttime").Value) Then
                    oinput.monitorstarttime = "01Jan1900 00:00:00"
                Else
                    oinput.monitorstarttime = uxSheduleGridRow.Cells("monitorstarttime").Value
                End If

                If IsDBNull(uxSheduleGridRow.Cells("monitorendtime").Value) OrElse IsNothing(uxSheduleGridRow.Cells("monitorendtime").Value) Then
                    oinput.monitorendtime = "01Jan1900 00:00:00"
                Else
                    oinput.monitorendtime = uxSheduleGridRow.Cells("monitorendtime").Value
                End If

                REM  oinput.scheduletime = uxSheduleGridRow.Cells("scheduletime").Value
                REM oinput.monitorstarttime = uxSheduleGridRow.Cells("monitorstarttime").Value
                REM oinput.monitorendtime = uxSheduleGridRow.Cells("monitorendtime").Value

                If uxSheduleGridRow.Cells("monitormonday").Value = True Then
                    oinput.monitormonday = 1
                Else
                    oinput.monitormonday = 0
                End If

                If uxSheduleGridRow.Cells("monitortuesday").Value = True Then
                    oinput.monitortuesday = 1
                Else
                    oinput.monitortuesday = 0
                End If

                If uxSheduleGridRow.Cells("monitorwednesday").Value = True Then
                    oinput.monitorwednesday = 1
                Else
                    oinput.monitorwednesday = 0
                End If

                If uxSheduleGridRow.Cells("monitorthursday").Value = True Then
                    oinput.monitorthursday = 1
                Else
                    oinput.monitorthursday = 0
                End If

                If uxSheduleGridRow.Cells("monitorfriday").Value = True Then
                    oinput.monitorfriday = 1
                Else
                    oinput.monitorfriday = 0
                End If

                If uxSheduleGridRow.Cells("monitorsaturday").Value = True Then
                    oinput.monitorsaturday = 1
                Else
                    oinput.monitorsaturday = 0
                End If

                If uxSheduleGridRow.Cells("monitorsunday").Value = True Then
                    oinput.monitorsunday = 1
                Else
                    oinput.monitorsunday = 0
                End If

                oinput.errorfrequency = uxSheduleGridRow.Cells("errorfrequency").Value
                oinput.restartattemps = uxSheduleGridRow.Cells("restartattemps").Value
                oinput.restartfrequency = uxSheduleGridRow.Cells("restartfrequency").Value

                oinput.dirtyrecord = True

                SRS_Actions.actionlist.Add(oinput)
            End If
        Next

        'REM save to database
        'For Each oinput In SRS_Actions.actionlist
        '    If oinput.dirtyrecord = True Then
        '        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
        '            oinput.write_mode = bc_schedule_item.UPDATE
        '            oinput.db_write()
        '        Else
        '            oinput.write_mode = bc_schedule_item.UPDATE
        '            If oinput.transmit_to_server_and_receive(oinput, True) = False Then
        '                Exit Sub
        '            End If
        '        End If

        '    End If
        'Next

        If uxSheduleGrid.Rows.Count > 0 Then
            Me.uxSheduleGrid.Rows(uxSheduleGrid.Rows.Count - 1).Cells(1).ReadOnly = True
        End If

        RaiseEvent save_sched(SRS_Jobs.joblist, SRS_Actions.actionlist)

        uxClients.Enabled = True
        uxLJobs.Enabled = True
        uxJobsGrid.Enabled = True
        uxSheduleGrid.Enabled = True
        SchedSave.Enabled = False
        SchedReset.Enabled = False
        uncomitted_data = False

    End Sub

    Private Sub SchedReset_Click(sender As Object, e As EventArgs) Handles SchedReset.Click

        Dim selectedJob As Long

        Me.refreshing = True
        uncomitted_data = False

        'Refresh objects
        RaiseEvent refresh()

        'ReLoad schedule grid
        If uxJobsGrid.SelectedCells.Count > 0 Then
            uxJobsGrid.SelectedRows(0).Cells(1).Value = Nothing
        End If

        If uxLJobs.SelectedItems.Count = 1 Then
            selectedJob = uxLJobs.SelectedItems.Item(0).Tag
            build_schedule(SRS_Actions, selectedJob, selectedClientId)
            buildJobsDetail(selectedJob)
        End If
        Me.refreshing = False

        uxClients.Enabled = True
        uxLJobs.Enabled = True
        uxJobsGrid.Enabled = True
        uxSheduleGrid.Enabled = True
        SchedSave.Enabled = False
        SchedReset.Enabled = False

    End Sub

    Private Sub Additem_Click(sender As Object, e As EventArgs) Handles AddItem.Click
        Dim selectedJob As Long = -1

        'Ccombo box if variables.
        Dim lu As DataGridViewComboBoxCell
        Dim lus As New ArrayList
        Dim use_combo As Boolean = False
        Dim currentvalue As String


        If srsContextMenuStrip1.SourceControl.Name = "uxSheduleGrid" Then

            Me.uxSheduleGrid.Rows.Add(selectedJob, SRS_Clients.clientlist(0).client_name)
            Me.uxSheduleGrid.Rows(uxSheduleGrid.Rows.Count - 1).Cells(1).ReadOnly = False

            REM set selected job type
            If uxLJobs.SelectedItems.Count = 1 Then
                selectedJob = uxLJobs.SelectedItems.Item(0).Tag
                For x = 0 To SRS_Jobs.joblist.Count - 1
                    If SRS_Jobs.joblist(x).jobid = selectedJob Then
                        Me.uxSheduleGrid.Rows(uxSheduleGrid.Rows.Count - 1).Cells(0).Value = SRS_Jobs.joblist(x).checkname
                    End If
                Next x
            End If

            uxClients.Enabled = False
            uxLJobs.Enabled = False
            uxJobsGrid.Enabled = False
            SchedSave.Enabled = True
            SchedReset.Enabled = True
            uncomitted_data = True

            Me.uxSheduleGrid.Rows(uxSheduleGrid.Rows.Count - 1).Cells(1).Selected = True
            uxSheduleGrid.CurrentCell = Me.uxSheduleGrid.Rows(uxSheduleGrid.Rows.Count - 1).Cells(1)


            'Load combo box if needed.
            use_combo = False

            If uxSheduleGrid.CurrentCell.ColumnIndex = 1 Then

                lu = New DataGridViewComboBoxCell
                lu.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing
                For x = 0 To SRS_Clients.clientlist.Count - 1
                    lu.Items.Add(SRS_Clients.clientlist(x).client_name)
                Next

                uncomitted_data = False
                lu.Value = uxSheduleGrid.SelectedRows(0).Cells(1).Value
                lus.Add(lu)
                currentvalue = uxSheduleGrid.SelectedRows(0).Cells(1).Value
                uxSheduleGrid.SelectedRows(0).Cells(1) = lus(0)
                lu.Value = currentvalue
                uncomitted_data = True
                use_combo = True
            End If
        End If

    End Sub
    Private Sub DeleteItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteItem.Click

        Dim schedid As Long
        Dim clientname As String
        Dim jobName As String

        'Rem delete sched
        If uxSheduleGrid.SelectedRows.Count > 0 Then

            schedid = uxSheduleGrid.SelectedRows.Item(0).Cells("schedid").Value()
            jobName = uxSheduleGrid.SelectedRows.Item(0).Cells("JobNameS").Value()
            clientname = uxSheduleGrid.SelectedRows.Item(0).Cells("clientname").Value()

            delete_schedule(schedid, jobName, clientname)

        End If

    End Sub


    Public Sub delete_schedule(schedid As Long, JobName As String, clientname As String)

        Dim oinput As bc_action_item = Nothing
        Dim db_Actionlist As New bc_srs_action_db
        Dim certificate As New bc_cs_security.certificate
        Dim item As String

        REM find the schedule object
        For x = 0 To SRS_Actions.actionlist.Count - 1
            If SRS_Actions.actionlist(x).scheduleid = schedid And SRS_Actions.actionlist(x).client_name = clientname Then
                oinput = SRS_Actions.actionlist(x)
            End If
        Next

        item = uxSheduleGrid.SelectedRows.Item(0).Cells("ItemName").Value()
        item = Trim(item)
        Dim omsg As New bc_cs_message("Blue Curve", "Are you sure you wish to delete " + item + "?", bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
        If omsg.cancel_selected = True Then
            Exit Sub
        End If

        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
            oinput.write_mode = bc_action_item.DELETE
            oinput.db_write()
        Else
            oinput.tmode = bc_cs_soap_base_class.tWRITE
            oinput.write_mode = bc_action_item.DELETE
            If oinput.transmit_to_server_and_receive(oinput, True) = False Then
                Exit Sub
            End If
        End If

        Me.refreshing = True
        uncomitted_data = False

        'Refresh objects
        RaiseEvent refresh()

        REM refresh
        build_schedule(SRS_Actions, oinput.jobid, oinput.clientid)

        Me.refreshing = False

        uxEmailTree.Enabled = True
        EmailSave.Enabled = False
        Emailreset.Enabled = False
        uncomitted_data = False

    End Sub





    'Private Sub srsContextMenuStrip1_MouseClick(sender As Object, e As MouseEventArgs) Handles srsContextMenuStrip1.MouseClick

    '    Dim selectedJob As Long = -1

    '    'Ccombo box if variables.
    '    Dim lu As DataGridViewComboBoxCell
    '    Dim lus As New ArrayList
    '    Dim use_combo As Boolean = False
    '    Dim currentvalue As String


    '    If srsContextMenuStrip1.SourceControl.Name = "uxSheduleGrid" Then

    '        Me.uxSheduleGrid.Rows.Add(selectedJob, SRS_Clients.clientlist(0).client_name)
    '        Me.uxSheduleGrid.Rows(uxSheduleGrid.Rows.Count - 1).Cells(1).ReadOnly = False

    '        REM set selected job type
    '        If uxLJobs.SelectedItems.Count = 1 Then
    '            selectedJob = uxLJobs.SelectedItems.Item(0).Tag
    '            For x = 0 To SRS_Jobs.joblist.Count - 1
    '                If SRS_Jobs.joblist(x).jobid = selectedJob Then
    '                    Me.uxSheduleGrid.Rows(uxSheduleGrid.Rows.Count - 1).Cells(0).Value = SRS_Jobs.joblist(x).checkname
    '                End If
    '            Next x
    '        End If

    '        uxClients.Enabled = False
    '        uxLJobs.Enabled = False
    '        uxJobsGrid.Enabled = False
    '        SchedSave.Enabled = True
    '        SchedReset.Enabled = True
    '        uncomitted_data = True

    '        Me.uxSheduleGrid.Rows(uxSheduleGrid.Rows.Count - 1).Cells(1).Selected = True
    '        uxSheduleGrid.CurrentCell = Me.uxSheduleGrid.Rows(uxSheduleGrid.Rows.Count - 1).Cells(1)


    '        'Load combo box if needed.
    '        use_combo = False

    '        If uxSheduleGrid.CurrentCell.ColumnIndex = 1 Then

    '            lu = New DataGridViewComboBoxCell
    '            lu.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing
    '            For x = 0 To SRS_Clients.clientlist.Count - 1
    '                lu.Items.Add(SRS_Clients.clientlist(x).client_name)
    '            Next

    '            uncomitted_data = False
    '            lu.Value = uxSheduleGrid.SelectedRows(0).Cells(1).Value
    '            lus.Add(lu)
    '            currentvalue = uxSheduleGrid.SelectedRows(0).Cells(1).Value
    '            uxSheduleGrid.SelectedRows(0).Cells(1) = lus(0)
    '            lu.Value = currentvalue
    '            uncomitted_data = True
    '            use_combo = True
    '        End If
    '    End If

    'End Sub

    Public Sub add_new_jobtype(newitem As String)

        Dim oinput As bc_srs_job
        Dim db_joblist As New bc_srs_jobs_db
        Dim certificate As New bc_cs_security.certificate
        Dim rowJob As Long


        REM check if name already exsits
        For x = 0 To SRS_Jobs.joblist.Count - 1
            If SRS_Jobs.joblist(x).checkname = newitem Then
                Dim omsg As New bc_cs_message("Blue Curve", "Job: " + newitem + " already exists!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Sub
            End If
        Next

        REM add to uxLJobs
        oinput = New bc_srs_job(0, 0, newitem, "", certificate)
        oinput.typeid = 1
        oinput.type_name = "Service"
        oinput.dirtyrecord = True
        rowJob = SRS_Jobs.joblist.Add(oinput)

        REM refresh
        'Load jobs Listview
        Dim lvew = New ListViewItem
        Me.uxLJobs.Items.Clear()
        For x = 0 To SRS_Jobs.joblist.Count - 1
            lvew = New ListViewItem(SRS_Jobs.joblist(x).checkname.ToString)
            lvew.Tag = SRS_Jobs.joblist(x).jobid
            lvew.ImageIndex = 6
            Me.uxLJobs.Items.Add(lvew)
        Next

        Me.uxLJobs.Items(uxLJobs.Items.Count - 1).Selected = True
        buildJobsDetail(0)

    End Sub

    Public Sub add_new_client(newitem As String)

        Dim oinput As bc_srs_client
        Dim db_Clientlist As New bc_srs_clients_db
        Dim certificate As New bc_cs_security.certificate
        Dim rowClient As Long
        Dim newclientid As Long = 0


        REM check if client already exsits
        For x = 0 To SRS_Clients.clientlist.Count - 1
            If SRS_Clients.clientlist(x).client_name = newitem Then
                Dim omsg As New bc_cs_message("Blue Curve", "Client: " + newitem + " already exists!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Sub
            End If

            If SRS_Clients.clientlist(x).clientid > newclientid Then
                newclientid = SRS_Clients.clientlist(x).clientid
            End If

        Next

        REM add to uxClients
        oinput = New bc_srs_client(newclientid + 1, newitem, "", "", certificate)
        oinput.client_name = newitem
        oinput.server_name = " "
        oinput.url = " "
        oinput.active = 0
        oinput.dirtyrecord = True

        rowClient = SRS_Clients.clientlist.Add(oinput)

        REM refresh
        RefreshTClients()

        uxClientTree.SelectedNode = uxClientTree.Nodes(uxClientTree.Nodes.Count - 1)

        buildClientDetail(uxClientTree.SelectedNode.Tag)

    End Sub


    Public Sub add_new_emailaddress(newitem As String, clientid As Long)

        Dim oinput As bc_email_item
        Dim db_Emaillist As New bc_srs_email_db
        Dim certificate As New bc_cs_security.certificate
        Dim rowClient As Long
        Dim newEmailid As Long = 0

        Dim rowId As Integer
        Dim clientname As String = Nothing

        'REM get new emailid
        For x = 0 To SRS_Emails.emaillist.Count - 1
            If SRS_Emails.emaillist(x).emailid > newEmailid Then
                newEmailid = SRS_Emails.emaillist(x).emailid
            End If
        Next
        newEmailid = newEmailid + 1

        REM get name
        For x = 0 To SRS_Clients.clientlist.Count - 1
            If clientid = SRS_Clients.clientlist(x).clientid Then
                clientname = SRS_Clients.clientlist(x).client_name
            End If
        Next

        REM Add objct
        oinput = New bc_email_item(newEmailid, "", "", newitem, certificate)
        oinput.email = newitem
        oinput.clientid = clientid
        oinput.client_name = clientname
        oinput.dirtyrecord = True

        rowClient = SRS_Emails.emaillist.Add(oinput)

        REM add to uxEmailDetail
        rowId = uxEmailDetails.Rows.Add(newitem)
        uxEmailDetails.Rows(rowId).Cells("emailid").Value = newEmailid
        uxEmailDetails.Rows(rowId).Cells("eclient").Value = clientname
        uxEmailDetails.Rows(rowId).Cells("eaddress").Value = newitem
        uxEmailDetails.Rows(uxEmailDetails.Rows.Count - 1).Selected = True

        uxEmailTree.Enabled = False
        EmailSave.Enabled = True
        Emailreset.Enabled = True
        uncomitted_data = True

    End Sub
    Public Sub delete_emailaddress(emailid As String, clientid As Long)

        Dim oinput As bc_email_item = Nothing
        Dim db_Emaillist As New bc_srs_email_db
        Dim certificate As New bc_cs_security.certificate
        Dim clientname As String = Nothing
        Dim item As String

        REM find the email object
        For x = 0 To SRS_Emails.emaillist.Count - 1
            If SRS_Emails.emaillist(x).emailid = emailid Then
                oinput = SRS_Emails.emaillist(x)
            End If
        Next

        item = uxEmailDetails.SelectedRows.Item(0).Cells("ename").Value()
        item = Trim(item)
        Dim omsg As New bc_cs_message("Blue Curve", "Are you sure you wish to delete " + item + "?", bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
        If omsg.cancel_selected = True Then
            Exit Sub
        End If

        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
            oinput.write_mode = bc_email_item.DELETE
            oinput.db_write()
        Else
            oinput.tmode = bc_cs_soap_base_class.tWRITE
            oinput.write_mode = bc_email_item.DELETE
            If oinput.transmit_to_server_and_receive(oinput, True) = False Then
                Exit Sub
            End If
        End If

        Me.refreshing = True
        uncomitted_data = False

        'Refresh objects
        RaiseEvent refresh()

        REM refresh
        build_EmailGrid(uxEmailTree.Nodes(0).Tag)
        Me.refreshing = False

        uxEmailTree.Enabled = True
        EmailSave.Enabled = False
        Emailreset.Enabled = False
        uncomitted_data = False

    End Sub

    Public Sub add_new_userpref(clientemailid As Long, clientid As Long)

        Dim oinput As bc_emailfilter_item
        Dim db_Emaipref As New bc_srs_emailfilters_db
        Dim certificate As New bc_cs_security.certificate
        Dim rowClient As Long
        Dim newfilterid As Long = 0

        Dim rowId As Integer
        Dim clientname As String = Nothing

        'REM get new filterid
        For x = 0 To SRS_Emailfilters.emailfilter.Count - 1
            If SRS_Emailfilters.emailfilter(x).filterid > newfilterid Then
                newfilterid = SRS_Emailfilters.emailfilter(x).filterid
            End If
        Next

        REM get name
        For x = 0 To SRS_Clients.clientlist.Count - 1
            If clientid = SRS_Clients.clientlist(x).clientid Then
                clientname = SRS_Clients.clientlist(x).client_name
            End If
        Next

        REM Add objct
        oinput = New bc_emailfilter_item(newfilterid + 1, clientemailid, 2, "Job", certificate)
        oinput.value1 = ""
        oinput.value2 = ""

        oinput.dirtyrecord = True
        rowClient = SRS_Emailfilters.emailfilter.Add(oinput)

        REM add to uxPreflDetail
        rowId = uxPrefDetails.Rows.Add(clientname)
        uxPrefDetails.Rows(rowId).Cells("filterid").Value = newfilterid
        uxPrefDetails.Rows(rowId).Cells("prefid").Value = oinput.prefid
        uxPrefDetails.Rows(rowId).Cells("pclientemailid").Value = oinput.clientemailid
        uxPrefDetails.Rows(rowId).Cells("pdescription").Value = "Filter By Job"
        uxPrefDetails.Rows(rowId).Cells("preftype").Value = oinput.prfname
        uxPrefDetails.Rows(rowId).Cells("pclient").Value = clientname

        uxPrefDetails.Rows(uxPrefDetails.Rows.Count - 1).Selected = True

        Me.uxPrefDetails.SelectedRows(0).Cells(4).ReadOnly = False
        Me.uxPrefDetails.SelectedRows(0).Cells(6).ReadOnly = False

        uxEmailTree.Enabled = False
        EmailSave.Enabled = True
        Emailreset.Enabled = True
        uncomitted_data = True

    End Sub




    Private Sub uxSheduleGrid_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles uxSheduleGrid.DataError

        If e.Exception.Message = "DataGridViewComboBoxCell value is not valid." Then
            e.ThrowException = False
        End If

    End Sub

    Private Sub uxSheduleGrid_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles uxSheduleGrid.CellBeginEdit

        If uncomitted_data = False Then
            uxClients.Enabled = False
            uxLJobs.Enabled = False
            uxJobsGrid.Enabled = False
            SchedSave.Enabled = True
            SchedReset.Enabled = True
            uncomitted_data = True
        End If

    End Sub

    Private Sub uxJobsGrid_RowValidating(sender As Object, e As DataGridViewCellCancelEventArgs) Handles uxJobsGrid.RowValidating

    End Sub

    Private Sub uxJobsGrid_SelectionChanged(sender As Object, e As EventArgs) Handles uxJobsGrid.SelectionChanged

        'Load combo box if needed.
        Dim lu As DataGridViewComboBoxCell
        Dim lus As New ArrayList
        Dim use_combo As Boolean = False
        Dim currentvalue As String

        use_combo = False

        If uxJobsGrid.SelectedRows.Count > 0 Then

            If uxJobsGrid.SelectedRows(0).Cells(0).Value = "Check Type" Then
                lu = New DataGridViewComboBoxCell
                lu.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing

                For x = 0 To SRS_JobTypes.typelist.Count - 1
                    lu.Items.Add(SRS_JobTypes.typelist(x).type_name)
                Next

                lu.Value = uxJobsGrid.SelectedRows(0).Cells(1).Value
                lus.Add(lu)
                currentvalue = uxJobsGrid.SelectedRows(0).Cells(1).Value
                uxJobsGrid.SelectedRows(0).Cells(1) = lus(0)
                lu.Value = currentvalue
                use_combo = True
            End If

            If uxJobsGrid.SelectedRows(0).Cells(0).Value = "Fault Check" Then
                lu = New DataGridViewComboBoxCell
                lu.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing
                lu.Items.Add("True")
                lu.Items.Add("False")
                lu.Value = uxJobsGrid.SelectedRows(0).Cells(1).Value
                lus.Add(lu)
                currentvalue = uxJobsGrid.SelectedRows(0).Cells(1).Value
                uxJobsGrid.SelectedRows(0).Cells(1) = lus(0)
                lu.Value = currentvalue
                use_combo = True
            End If

            If uxJobsGrid.SelectedRows(0).Cells(0).Value = "Status Check" Then
                lu = New DataGridViewComboBoxCell
                lu.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing
                lu.Items.Add("True")
                lu.Items.Add("False")
                lus.Add(lu)
                currentvalue = uxJobsGrid.SelectedRows(0).Cells(1).Value
                uxJobsGrid.SelectedRows(0).Cells(1) = lus(0)
                lu.Value = currentvalue
                use_combo = True
            End If

            uxJobsGrid.SelectedRows(0).Cells(1).Selected = True
        End If
    End Sub


    Private Sub bc_am_cp_monitor_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If uncomitted_data = True Then
            MsgBox("You have uncommitted data please commit or discard before closing.", MsgBoxStyle.Information, "Blue Curve")
            e.Cancel = True
        End If

    End Sub

    Private Sub TabControl1_Selecting(sender As Object, e As TabControlCancelEventArgs) Handles TabControl1.Selecting

        If uncomitted_data = True Then
            MsgBox("You have uncommitted data please commit or discard before changing view.", MsgBoxStyle.Information, "Blue Curve")
            e.Cancel = True
        End If

    End Sub

    Private Sub uxSheduleGrid_RowValidating(sender As Object, e As DataGridViewCellCancelEventArgs) Handles uxSheduleGrid.RowValidating

        Dim row As DataGridViewRow = uxSheduleGrid.Rows(e.RowIndex)

        If uncomitted_data = True Then

            REM schedule Valadation
            If Len(row.Cells("ItemName").Value) < 1 Then
                MsgBox("Task Name can not be blank.", MsgBoxStyle.Information, "Blue Curve")
                e.Cancel = True
            End If

            If row.Cells("Frequency").Value > 0 And Not row.Cells("scheduletime").Value = Nothing Then
                MsgBox("A 'Frequency' and 'One Off time' can not both be set.", MsgBoxStyle.Information, "Blue Curve")
                e.Cancel = True
            End If

            If row.Cells("Frequency").Value = 0 And row.Cells("scheduletime").Value = Nothing Then
                MsgBox("A 'Frequency' and 'One Off time' can not both be blank.", MsgBoxStyle.Information, "Blue Curve")
                e.Cancel = True
            End If


            If Not row.Cells("scheduletime").Value = Nothing And (Not row.Cells("monitorstarttime").Value = Nothing Or Not row.Cells("monitorendtime").Value = Nothing) Then
                MsgBox("A 'One off time' and a time range can not both be set.", MsgBoxStyle.Information, "Blue Curve")
                e.Cancel = True
            End If

            If (row.Cells("monitormonday").Value = False And row.Cells("monitortuesday").Value = False And row.Cells("monitorwednesday").Value = False _
                    And row.Cells("monitorthursday").Value = False And row.Cells("monitorfriday").Value = False And row.Cells("monitorsaturday").Value = False _
                    And row.Cells("monitorsunday").Value = True) And (Not row.Cells("scheduletime").Value = Nothing And Not row.Cells("Frequency").Value = Nothing) Then

                MsgBox("Days should only be set when a fequency or 'One off time' is used.", MsgBoxStyle.Information, "Blue Curve")
                e.Cancel = True
            End If

        End If

    End Sub

    Private Sub srsJobContextMenuStrip_Click(sender As Object, e As EventArgs) Handles srsJobContextMenuStrip.Click

        If srsJobContextMenuStrip.SourceControl.Name = "uxLJobs" Or srsJobContextMenuStrip.SourceControl.Name = "uxJobsGrid" Then

            Dim fedit As New bc_am_cp_edit
            fedit.ltitle.Text = "Add New SRS Job "
            fedit.Cdt.Items.Add("String")
            fedit.Tentry.Visible = True
            fedit.centry.Visible = False
            fedit.ShowDialog()
            If fedit.cancel_selected = True Then
                Exit Sub
            Else
                'View.Cursor = Cursors.WaitCursor
                add_new_jobtype(fedit.Tentry.Text)
            End If

            Me.uxJobsGrid.Columns(1).ReadOnly = False

            uxClients.Enabled = False
            uxLJobs.Enabled = False
            uxSheduleGrid.Enabled = False
            SchedSave.Enabled = True
            SchedReset.Enabled = True
            uncomitted_data = True
        End If

    End Sub

    Private Sub uxJobsGrid_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles uxJobsGrid.Validating

        If uxJobsGrid.Rows(4).Cells(1).Value = True And uxJobsGrid.Rows(5).Cells(1).Value = True Then
            MsgBox("'Fault Check' and 'Status Check' can not both be True.", MsgBoxStyle.Information, "Blue Curve")
            e.Cancel = True
        End If

        If uxJobsGrid.Rows(4).Cells(1).Value = False And uxJobsGrid.Rows(5).Cells(1).Value = False Then
            MsgBox("'Fault Check' and 'Status Check' can not both be False.", MsgBoxStyle.Information, "Blue Curve")
            e.Cancel = True
        End If

    End Sub

    Private Sub srsClientContextMenuStrip_Click(sender As Object, e As EventArgs) Handles srsClientContextMenuStrip.Click

        If srsClientContextMenuStrip.SourceControl.Name = "uxClientTree" Or srsClientContextMenuStrip.SourceControl.Name = "uxClientDetails" Then

            Dim fedit As New bc_am_cp_edit
            fedit.ltitle.Text = "Add New SRS Client "
            fedit.Cdt.Items.Add("String")
            fedit.Tentry.Visible = True
            fedit.centry.Visible = False
            fedit.ShowDialog()

            If fedit.cancel_selected = True Then
                Exit Sub
            Else
                add_new_client(fedit.Tentry.Text)
            End If

            Me.uxClientDetails.Columns(1).ReadOnly = False

            uxClients.Enabled = False
            uxLJobs.Enabled = False
            uxSheduleGrid.Enabled = False
            ClientSave.Enabled = True
            ClientReset.Enabled = True
            uncomitted_data = True
        End If

    End Sub

    Private Sub uxClientDetails_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles uxClientDetails.CellBeginEdit

        REM set dirty flag
        Dim selectedClient As Long

        selectedClient = uxClientTree.SelectedNode.Tag

        For x = 0 To SRS_Clients.clientlist.Count - 1
            If SRS_Clients.clientlist(x).clientid = selectedClient Then
                SRS_Clients.clientlist(x).dirtyrecord = True
            End If
        Next

        uxClients.Enabled = False
        ClientSave.Enabled = True
        ClientReset.Enabled = True
        uncomitted_data = True

    End Sub



    Private Sub uxClientDetails_SelectionChanged(sender As Object, e As EventArgs) Handles uxClientDetails.SelectionChanged

        'Load combo box if needed.
        Dim lu As DataGridViewComboBoxCell
        Dim lus As New ArrayList
        Dim use_combo As Boolean = False
        Dim currentvalue As String

        use_combo = False

        If uxClientDetails.SelectedRows.Count > 0 Then

            If uxClientDetails.SelectedRows(0).Cells(0).Value = "Active" Then
                lu = New DataGridViewComboBoxCell
                lu.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing
                lu.Items.Add("True")
                lu.Items.Add("False")
                lu.Value = uxClientDetails.SelectedRows(0).Cells(1).Value
                lus.Add(lu)
                currentvalue = uxClientDetails.SelectedRows(0).Cells(1).Value
                uxClientDetails.SelectedRows(0).Cells(1) = lus(0)
                lu.Value = currentvalue
                use_combo = True
            End If

            uxClientDetails.SelectedRows(0).Cells(1).Selected = True
        End If

    End Sub

    Private Sub ClientSave_Click(sender As Object, e As EventArgs) Handles ClientSave.Click

        Dim clientname As String
        Dim selectedClient As Long
        Dim clientinput As bc_srs_client

        REM save client details to database
        If Me.uxClientDetails.SelectedRows.Count > 0 Then

            selectedClient = uxClientTree.SelectedNode.Tag
            clientname = uxClientTree.SelectedNode.Text

            For Each clientinput In SRS_Clients.clientlist
                If clientinput.client_name = clientname And clientinput.dirtyrecord = True Then

                    clientinput.clientid = selectedClient
                    clientinput.client_name = clientname
                    clientinput.server_name = uxClientDetails.Rows(2).Cells("cvalue").Value
                    clientinput.db_name = uxClientDetails.Rows(3).Cells("cvalue").Value
                    clientinput.db_server = uxClientDetails.Rows(4).Cells("cvalue").Value
                    clientinput.conn_account = uxClientDetails.Rows(5).Cells("cvalue").Value
                    clientinput.url = uxClientDetails.Rows(6).Cells("cvalue").Value
                    clientinput.from_email = uxClientDetails.Rows(7).Cells("cvalue").Value

                    If uxClientDetails.Rows(8).Cells("cvalue").Value = True Then
                        clientinput.active = 1
                    Else
                        clientinput.active = 0
                    End If

                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        clientinput.write_mode = bc_srs_client.UPDATE
                        clientinput.db_write()
                    Else
                        clientinput.tmode = bc_srs_client.tWRITE
                        clientinput.write_mode = bc_srs_client.UPDATE
                        If clientinput.transmit_to_server_and_receive(clientinput, True) = False Then
                            Exit Sub
                        End If
                    End If

                End If
            Next
        End If

        'Refresh objects
        RaiseEvent refresh()

        ClientSave.Enabled = False
        ClientReset.Enabled = False
        uncomitted_data = False

    End Sub

    Private Sub ClientReset_Click(sender As Object, e As EventArgs) Handles ClientReset.Click

        Me.refreshing = True
        uncomitted_data = False

        'Refresh objects
        RaiseEvent refresh()

        REM refresh
        RefreshTClients()
        uxClientTree.SelectedNode = uxClientTree.Nodes(0)
        buildClientDetail(uxClientTree.SelectedNode.Tag)

        Me.refreshing = False

        uxClients.Enabled = True
        uxLJobs.Enabled = True
        uxJobsGrid.Enabled = True
        uxSheduleGrid.Enabled = True
        ClientSave.Enabled = False
        ClientReset.Enabled = False

    End Sub


    Private Sub uxEmailTree_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles uxEmailTree.AfterSelect

        Dim TreeClientid As Long
        Dim TreeEmailid As Long
        Dim TreeClientName As String

        If uxEmailTree.SelectedNode.Level = 0 Then
            ' Get the client id from the tree view
            TreeClientid = uxEmailTree.SelectedNode.Tag
            TreeClientName = uxEmailTree.SelectedNode.Text
            TreeEmailid = 0

            uxEmailDetails.Left = 3
            uxEmailDetails.Top = 21
            uxEmailDetails.Height = SplitContainer7.Panel2.PreferredSize.Height - 57
            uxEmailDetails.Visible = True
            uxPrefDetails.Visible = False

            srsEmailContextMenuStrip.Items("InsertAddress").Visible = True
            srsEmailContextMenuStrip.Items("InertPref").Visible = False
            srsEmailContextMenuStrip.Items("DeletePref").Visible = False
            srsEmailContextMenuStrip.Items("DeleteAddress").Visible = True

            build_EmailGrid(TreeClientid)

            If uxEmailDetails.RowCount > 0 Then
                srsEmailContextMenuStrip.Items("DeleteAddress").Visible = True
            End If

        End If

        If uxEmailTree.SelectedNode.Level = 1 Then
            ' Get the client id from the tree view
            TreeClientid = uxEmailTree.SelectedNode.Parent.Tag()
            TreeClientName = uxEmailTree.SelectedNode.Parent.Text
            TreeEmailid = uxEmailTree.SelectedNode.Tag

            uxPrefDetails.Left = 3
            uxPrefDetails.Top = 21

            uxPrefDetails.Height = SplitContainer7.Panel2.PreferredSize.Height - 57
            uxPrefDetails.Visible = True
            uxEmailDetails.Visible = False

            srsEmailContextMenuStrip.Items("InsertAddress").Visible = False
            srsEmailContextMenuStrip.Items("InertPref").Visible = True
            srsEmailContextMenuStrip.Items("DeleteAddress").Visible = False
            srsEmailContextMenuStrip.Items("DeletePref").Visible = False

            build_PrefGrid(TreeEmailid, TreeClientName)

            If uxPrefDetails.RowCount > 0 Then
                srsEmailContextMenuStrip.Items("DeletePref").Visible = True
            End If


        End If

    End Sub


    Private Sub InsertAddress_Click(sender As Object, e As EventArgs) Handles InsertAddress.Click

        'Load combo box if needed.
        Dim lu As DataGridViewComboBoxCell
        Dim lus As New ArrayList
        Dim use_combo As Boolean = False

        'Dim s As String = CType(sender, ContextMenuStrip).GetItemAt( _
        '        CType(sender, ContextMenuStrip).DisplayRectangle.X, _
        '        CType(sender, ContextMenuStrip).DisplayRectangle.Y).Text.Trim()

        Dim fedit As New bc_am_cp_edit
        fedit.Text = "Add new emaill"
        fedit.ltitle.Text = "Type the New address: "
        fedit.Cdt.Items.Add("String")
        fedit.Tentry.Visible = True
        fedit.centry.Visible = False
        fedit.ShowDialog()

        If fedit.cancel_selected = True Then
            Exit Sub
        Else
            add_new_emailaddress(fedit.Tentry.Text, uxEmailTree.SelectedNode.Tag)
        End If

        Me.uxEmailDetails.Columns(1).ReadOnly = False
        uxEmailTree.Enabled = False
        uncomitted_data = True

    End Sub

    Private Sub InertPref_Click(sender As Object, e As EventArgs) Handles InertPref.Click

        'Load combo box if needed.
        Dim lus As New ArrayList
        Dim use_combo As Boolean = False

        'Dim s As String = CType(sender, ContextMenuStrip).GetItemAt( _
        '        CType(sender, ContextMenuStrip).DisplayRectangle.X, _
        '        CType(sender, ContextMenuStrip).DisplayRectangle.Y).Text.Trim()


        add_new_userpref(uxEmailTree.SelectedNode.Tag, uxEmailTree.SelectedNode.Parent.Tag)

        uxEmailTree.Enabled = False
        uncomitted_data = True

    End Sub


    Private Sub DeleteAddress_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteAddress.Click

        Dim EmailID As Long
        Dim clientid As Long

        'rem get emailid to delete
        If uxEmailDetails.SelectedRows.Count > 0 Then


            EmailID = uxEmailDetails.SelectedRows.Item(0).Cells("emailid").Value()
            clientid = uxEmailTree.SelectedNode.Tag

            delete_emailaddress(EmailID, clientid)

        End If

    End Sub


    Private Sub srsEmailContextMenuStrip_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles srsEmailContextMenuStrip.Opening

    End Sub

    Private Sub uxEmailDetails_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles uxEmailDetails.CellContentClick

    End Sub

    Private Sub uxEmailDetails_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles uxEmailDetails.CellBeginEdit

        Dim selectedEmail As Long

        selectedEmail = uxEmailDetails.SelectedRows(0).Cells("emailid").Value
        For x = 0 To SRS_Emails.emaillist.Count - 1
            If SRS_Emails.emaillist(x).emailid = selectedEmail Then
                SRS_Emails.emaillist(x).dirtyrecord = True
            End If
        Next

        uxEmailTree.Enabled = False
        EmailSave.Enabled = True
        Emailreset.Enabled = True
        uncomitted_data = True

    End Sub

    Private Sub Emailreset_Click(sender As Object, e As EventArgs) Handles Emailreset.Click

        Me.refreshing = True
        uncomitted_data = False

        'Refresh objects
        RaiseEvent refresh()

        If uxEmailTree.SelectedNode.Level = 0 Then
            build_EmailGrid(uxEmailTree.SelectedNode.Tag)
        End If

        If uxEmailTree.SelectedNode.Level = 1 Then
            build_PrefGrid(uxEmailTree.SelectedNode.Tag, uxEmailTree.SelectedNode.Parent.Text)
        End If

        uxEmailTree.Enabled = True
        EmailSave.Enabled = False
        Emailreset.Enabled = False

    End Sub

    Private Sub EmailSave_Click(sender As Object, e As EventArgs) Handles EmailSave.Click

        Dim clientname As String
        Dim selectedClient As Long
        Dim emailinput As bc_email_item
        Dim uxEmailGridRow As DataGridViewRow

        Dim filterid As Long
        Dim selectedpref As Long
        Dim prefinput As bc_emailfilter_item
        Dim uxpreflGridRow As DataGridViewRow = Nothing

        REM save Email details to database

        If uxEmailDetails.Visible = True Then

            If Me.uxEmailDetails.SelectedRows.Count > 0 Then

                selectedClient = uxEmailTree.SelectedNode.Tag
                clientname = uxEmailTree.SelectedNode.Text

                For Each uxEmailGridRow In uxEmailDetails.Rows

                    For Each emailinput In SRS_Emails.emaillist
                        If emailinput.client_name = clientname And emailinput.emailid = uxEmailGridRow.Cells("emailid").Value And emailinput.dirtyrecord = True Then

                            emailinput.emailid = uxEmailGridRow.Cells("emailid").Value
                            emailinput.clientid = selectedClient
                            emailinput.client_name = clientname
                            emailinput.name = uxEmailGridRow.Cells("ename").Value
                            emailinput.description = uxEmailGridRow.Cells("edescription").Value
                            emailinput.email = uxEmailGridRow.Cells("eaddress").Value

                            If uxEmailGridRow.Cells("eonerror").Value = True Then
                                emailinput.onerror = 1
                            Else
                                emailinput.onerror = 0
                            End If

                            If uxEmailGridRow.Cells("eonstatuscheck").Value = True Then
                                emailinput.onstatuscheck = 1
                            Else
                                emailinput.onstatuscheck = 0
                            End If

                            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                                emailinput.write_mode = bc_srs_client.UPDATE
                                emailinput.db_write()
                            Else
                                emailinput.tmode = bc_cs_soap_base_class.tWRITE
                                emailinput.write_mode = bc_email_item.UPDATE
                                If emailinput.transmit_to_server_and_receive(emailinput, True) = False Then
                                    Exit Sub
                                End If
                            End If

                        End If
                    Next
                Next
            End If
        End If

        REM save prf info
        If uxPrefDetails.Visible = True Then

            If Me.uxPrefDetails.SelectedRows.Count > 0 Then

                selectedpref = uxEmailTree.SelectedNode.Tag

                For Each uxPrefGridRow In uxPrefDetails.Rows

                    filterid = uxPrefGridRow.Cells("filterid").Value

                    For Each prefinput In SRS_Emailfilters.emailfilter
                        If prefinput.clientemailid = selectedpref And prefinput.dirtyrecord = True Then

                            prefinput.prefid = uxPrefGridRow.Cells("prefid").Value
                            prefinput.clientemailid = uxPrefGridRow.Cells("pclientemailid").Value

                            If prefinput.prefid = 2 Then
                                REM set object value 1 to the jobid
                                For x = 0 To SRS_Jobs.joblist.Count - 1
                                    If SRS_Jobs.joblist(x).checkname.ToString = uxPrefGridRow.Cells("value1").Value Then
                                        prefinput.value1 = SRS_Jobs.joblist(x).jobid
                                        Exit For
                                    End If
                                Next
                            Else
                                prefinput.value1 = uxPrefGridRow.Cells("value1").Value
                            End If

                            prefinput.value2 = uxPrefGridRow.Cells("value2").Value
                            'prefinput.checkname = uxPrefGridRow.Cells("pdescription").Value

                            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                                prefinput.write_mode = bc_emailfilter_item.UPDATE
                                prefinput.db_write()
                            Else
                                prefinput.tmode = bc_cs_soap_base_class.tWRITE
                                prefinput.write_mode = bc_emailfilter_item.UPDATE

                                If prefinput.transmit_to_server_and_receive(prefinput, True) = False Then
                                    Exit Sub
                                End If
                            End If

                        End If
                    Next
                Next
            End If
        End If


        Me.refreshing = True
        uncomitted_data = False

        'Refresh objects
        RaiseEvent refresh()

        REM refresh
        build_EmailGrid(uxEmailTree.Nodes(0).Tag)
        Me.refreshing = False

        uxEmailTree.Enabled = True
        EmailSave.Enabled = False
        Emailreset.Enabled = False
        uncomitted_data = False

    End Sub

    Private Sub uxPrefDetails_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles uxPrefDetails.CellBeginEdit

        'Load combo box if needed.
        Dim lu As DataGridViewComboBoxCell
        Dim lus As New ArrayList
        Dim use_combo As Boolean = False
        Dim currentvalue As String

        Dim filterid As Long
        Dim selectedclientid As Long

        use_combo = False

        If uxPrefDetails.SelectedRows.Count > 0 Then

            'Pref Type
            If uxPrefDetails.CurrentCell.ColumnIndex = 4 Then
                lu = New DataGridViewComboBoxCell
                lu.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing
                lu.Items.Add("Job")
                lu.Items.Add("Severity")
                lu.Value = uxPrefDetails.SelectedRows(0).Cells(4).Value
                lus.Add(lu)
                currentvalue = uxPrefDetails.SelectedRows(0).Cells(4).Value
                uxPrefDetails.SelectedRows(0).Cells(4) = lus(0)
                lu.Value = currentvalue
                use_combo = True
                uxPrefDetails.SelectedRows(0).Cells(4).Selected = True
            End If

            'Combo of job type
            If uxPrefDetails.CurrentCell.ColumnIndex = 6 And uxPrefDetails.SelectedRows(0).Cells(4).Value = "Job" Then
                lu = New DataGridViewComboBoxCell
                lu.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing

                For x = 0 To SRS_Jobs.joblist.Count - 1
                    lu.Items.Add(SRS_Jobs.joblist(x).checkname.ToString)
                Next

                lu.Value = uxPrefDetails.SelectedRows(0).Cells(6).Value
                lus.Add(lu)
                currentvalue = uxPrefDetails.SelectedRows(0).Cells(6).Value
                uxPrefDetails.SelectedRows(0).Cells(6) = lus(0)
                lu.Value = currentvalue
                use_combo = True
                uxPrefDetails.SelectedRows(0).Cells(6).Selected = True
            End If

        End If

        filterid = uxPrefDetails.SelectedRows(0).Cells("filterid").Value
        selectedclientid = uxPrefDetails.SelectedRows(0).Cells("pclientemailid").Value

        For x = 0 To SRS_Emailfilters.emailfilter.Count - 1
            If SRS_Emailfilters.emailfilter(x).filterid = filterid And SRS_Emailfilters.emailfilter(x).clientemailid = selectedclientid Then
                SRS_Emailfilters.emailfilter(x).dirtyrecord = True
            End If
        Next

        uxEmailTree.Enabled = False
        EmailSave.Enabled = True
        Emailreset.Enabled = True
        uncomitted_data = True

    End Sub


    Private Sub uxPrefDetails_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles uxPrefDetails.CellContentClick

    End Sub

    Private Sub uxPrefDetails_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles uxPrefDetails.CellEndEdit


        Dim jobid As Long
        If uxPrefDetails.SelectedRows.Count <> 0 Then

            If sender.currentcell.columnindex = 4 Then

                If sender.currentcell.value = "JOB" Then
                    uxPrefDetails.Rows(sender.currentrow.index).Cells("pdescription").Value = "Filter by Job"
                End If

                If sender.currentcell.value = "Severity" Then
                    uxPrefDetails.Rows(sender.currentrow.index).Cells("pdescription").Value = "Filter by Severity level"
                End If

            End If
        End If

    End Sub

    Private Sub uxPrefDetails_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles uxPrefDetails.CellValueChanged


    End Sub

    Private Sub bc_am_cp_monitor_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub uxSheduleGrid_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles uxSheduleGrid.CellContentClick

    End Sub

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub uxTJobs_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles uxTJobs.AfterSelect

        Dim sjobid As Long
        Dim sclientId As Long
        Dim selectedScheduleid As Long

        If IsNothing(uxTJobs.SelectedNode.Parent) Then
            sclientId = uxTJobs.SelectedNode.Tag

            Me.uxGridJob.Rows.Clear()
            Me.uxStatGrid.Rows.Clear()
            Me.uxErrorGrid.Rows.Clear()

        Else

            selectedScheduleid = uxTJobs.SelectedNode.Tag
            sclientId = uxTJobs.SelectedNode.Parent.Tag

            For x = 0 To SRS_Actions.actionlist.Count - 1
                If SRS_Actions.actionlist(x).scheduleid = selectedScheduleid Then
                    sjobid = SRS_Actions.actionlist(x).jobid
                End If
            Next

            'load Job grid
            build_JobGrid(SRS_Jobs, sjobid)

            'Load Errors grid
            build_ErrorGrid(SRS_Errors, selectedScheduleid)

            'Load Stats grid
            build_StatsGrid(SRS_Stats, selectedScheduleid, sclientId)
        End If
    End Sub

    Private Sub DeletePref_Click(sender As Object, e As EventArgs) Handles DeletePref.Click
        Dim EmailfilterID As Long
        Dim clientid As Long

        'rem get emailid to delete
        If uxPrefDetails.SelectedRows.Count > 0 Then

            EmailfilterID = uxPrefDetails.SelectedRows.Item(0).Cells("filterid").Value()
            clientid = uxEmailTree.SelectedNode.Tag

            delete_emailfilter(EmailfilterID, clientid)

        End If
    End Sub

    Public Sub delete_emailfilter(emailfilterid As String, clientid As Long)

        Dim oinput As bc_emailfilter_item = Nothing
        Dim db_Emailfilterlist As New bc_srs_emailfilters_db
        Dim certificate As New bc_cs_security.certificate
        Dim clientname As String = Nothing
        Dim item As String

        REM find the emailfilter object
        For x = 0 To SRS_Emailfilters.emailfilter.Count - 1
            If SRS_Emailfilters.emailfilter(x).filterid = emailfilterid Then
                oinput = SRS_Emailfilters.emailfilter(x)
            End If
        Next

        item = uxPrefDetails.SelectedRows.Item(0).Cells("pdescription").Value() + " " + oinput.checkname
        item = Trim(item)
        Dim omsg As New bc_cs_message("Blue Curve", "Are you sure you wish to delete " + item + "?", bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
        If omsg.cancel_selected = True Then
            Exit Sub
        End If

        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
            oinput.write_mode = bc_emailfilter_item.DELETE
            oinput.db_write()
        Else

            oinput.tmode = bc_cs_soap_base_class.tWRITE
            oinput.write_mode = bc_emailfilter_item.DELETE

            If oinput.transmit_to_server_and_receive(oinput, True) = False Then
                Exit Sub
            End If
        End If

        Me.refreshing = True
        uncomitted_data = False

        'Refresh objects
        RaiseEvent refresh()

        REM refresh
        build_EmailGrid(uxEmailTree.Nodes(0).Tag)
        Me.refreshing = False

        uxEmailTree.Enabled = True
        EmailSave.Enabled = False
        Emailreset.Enabled = False
        uncomitted_data = False

    End Sub


End Class


Public Class Cbc_am_cp_monitor

    Private WithEvents _view As Ibc_am_cp_monitor
    Public Sub New(view As Ibc_am_cp_monitor)
        _view = view

    End Sub

    Public Sub refresh() Handles _view.refresh
        load_data()

    End Sub

    Public Sub save_sched(joblist As ArrayList, actionList As ArrayList) Handles _view.save_sched

        Dim oinput As bc_action_item
        Dim jobinput As bc_srs_job

        REM save Job to database
        For Each jobinput In joblist
            If jobinput.dirtyrecord = True Then

                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    jobinput.write_mode = bc_schedule_item.UPDATE
                    jobinput.db_write()
                Else
                    jobinput.tmode = bc_schedule_item.tWRITE
                    jobinput.write_mode = bc_schedule_item.UPDATE
                    If jobinput.transmit_to_server_and_receive(jobinput, True) = False Then
                        Exit Sub
                    End If
                End If

            End If
        Next

        REM save schedule to database
        For Each oinput In actionList
            If oinput.dirtyrecord = True Then
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    oinput.write_mode = bc_action_item.UPDATE
                    oinput.db_write()
                Else
                    oinput.tmode = bc_cs_soap_base_class.tWRITE
                    oinput.write_mode = bc_action_item.UPDATE
                    If oinput.transmit_to_server_and_receive(oinput, True) = False Then
                        Exit Sub
                    End If
                End If

            End If
        Next

    End Sub


    Public Function load_data() As Boolean

        Dim SRSClients As New bc_srs_clientlist
        Dim SRSJobs As New bc_srs_joblist
        Dim SRSActions As New bc_srs_actionlist
        Dim SRSErrors As New bc_srs_errorlist
        Dim SRSStats As New bc_srs_statlist
        Dim SRSEmails As New bc_srs_emaillist
        Dim SRSEmailfilters As New bc_srs_emailfilters
        Dim SRSJobTypes As New bc_srs_typelist

        Dim blnConnectionOK As Boolean
        Dim objCentralSettings As New bc_cs_central_settings(True)
        Dim certificate As New bc_cs_security.certificate

        Try
            load_data = True
            blnConnectionOK = objCentralSettings.check_connection(bc_cs_central_settings.connection_method, True)

            'Get Clients
            If SRSClients.clientlist.Count = 0 Then
                If bc_cs_central_settings.connection_method = bc_cs_central_settings.ADO Then
                    SRSClients.db_read()
                ElseIf bc_cs_central_settings.connection_method = bc_cs_central_settings.SOAP Then
                    SRSClients.tmode = bc_cs_soap_base_class.tREAD
                    If SRSClients.transmit_to_server_and_receive(SRSClients, True) = False Then
                        Exit Function
                    End If
                Else
                    Dim omsg As New bc_cs_message("BlueCurveServices", "Cannot connect to " + bc_cs_central_settings.soap_server + " srs clients.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Function
                End If
            End If

            'Get Jobs
            If SRSJobs.joblist.Count = 0 Then
                If bc_cs_central_settings.connection_method = bc_cs_central_settings.ADO Then
                    SRSJobs.db_read()
                ElseIf bc_cs_central_settings.connection_method = bc_cs_central_settings.SOAP Then
                    SRSJobs.tmode = bc_cs_soap_base_class.tREAD
                    If SRSJobs.transmit_to_server_and_receive(SRSJobs, True) = False Then
                        Exit Function
                    End If
                Else
                    Dim omsg As New bc_cs_message("BlueCurveServices", "Cannot connect to " + bc_cs_central_settings.soap_server + " srs jobs.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Function
                End If
            End If

            'Get Actions
            If SRSActions.actionlist.Count = 0 Then
                If bc_cs_central_settings.connection_method = bc_cs_central_settings.ADO Then
                    SRSActions.db_read()
                ElseIf bc_cs_central_settings.connection_method = bc_cs_central_settings.SOAP Then
                    SRSActions.tmode = bc_cs_soap_base_class.tREAD
                    If SRSActions.transmit_to_server_and_receive(SRSActions, True) = False Then
                        Exit Function
                    End If
                Else
                    Dim omsg As New bc_cs_message("BlueCurveServices", "Cannot connect to " + bc_cs_central_settings.soap_server + " srs Actions", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Function
                End If
            End If

            'Get Stats
            If SRSStats.statlist.Count = 0 Then
                If bc_cs_central_settings.connection_method = bc_cs_central_settings.ADO Then
                    SRSStats.db_read()
                ElseIf bc_cs_central_settings.connection_method = bc_cs_central_settings.SOAP Then
                    SRSStats.tmode = bc_cs_soap_base_class.tREAD
                    If SRSStats.transmit_to_server_and_receive(SRSStats, True) = False Then
                        Exit Function
                    End If
                Else
                    Dim omsg As New bc_cs_message("BlueCurveServices", "Cannot connect to " + bc_cs_central_settings.soap_server + "  srs Stats", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Function
                End If
            End If

            'Get Errors
            If SRSErrors.errorlist.Count = 0 Then
                If bc_cs_central_settings.connection_method = bc_cs_central_settings.ADO Then
                    SRSErrors.db_read()
                ElseIf bc_cs_central_settings.connection_method = bc_cs_central_settings.SOAP Then
                    SRSErrors.tmode = bc_cs_soap_base_class.tREAD
                    If SRSErrors.transmit_to_server_and_receive(SRSErrors, True) = False Then
                        Exit Function
                    End If
                Else
                    Dim omsg As New bc_cs_message("BlueCurveServices", "Cannot connect to " + bc_cs_central_settings.soap_server + "  srs Errors", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Function
                End If
            End If

            'Get Email
            If SRSEmails.emaillist.Count = 0 Then
                If bc_cs_central_settings.connection_method = bc_cs_central_settings.ADO Then
                    SRSEmails.db_read()
                ElseIf bc_cs_central_settings.connection_method = bc_cs_central_settings.SOAP Then
                    SRSEmails.tmode = bc_cs_soap_base_class.tREAD
                    If SRSEmails.transmit_to_server_and_receive(SRSEmails, True) = False Then
                        Exit Function
                    End If
                Else
                    Dim omsg As New bc_cs_message("BlueCurveServices", "Cannot connect to " + bc_cs_central_settings.soap_server + "  srs Email", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Function
                End If
            End If

            'Get Email Filters
            If SRSEmailfilters.emailfilter.Count = 0 Then
                If bc_cs_central_settings.connection_method = bc_cs_central_settings.ADO Then
                    SRSEmailfilters.db_read()
                ElseIf bc_cs_central_settings.connection_method = bc_cs_central_settings.SOAP Then
                    SRSEmailfilters.tmode = bc_cs_soap_base_class.tREAD
                    If SRSEmailfilters.transmit_to_server_and_receive(SRSEmailfilters, True) = False Then
                        Exit Function
                    End If
                Else
                    Dim omsg As New bc_cs_message("BlueCurveServices", "Cannot connect to " + bc_cs_central_settings.soap_server + "  srs Email", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Function
                End If
            End If

            'Get Job Types
            If SRSJobTypes.typelist.Count = 0 Then
                If bc_cs_central_settings.connection_method = bc_cs_central_settings.ADO Then
                    SRSJobTypes.db_read()
                ElseIf bc_cs_central_settings.connection_method = bc_cs_central_settings.SOAP Then
                    SRSJobTypes.tmode = bc_cs_soap_base_class.tREAD
                    If SRSJobTypes.transmit_to_server_and_receive(SRSJobTypes, True) = False Then
                        Exit Function
                    End If
                Else
                    Dim omsg As New bc_cs_message("BlueCurveServices", "Cannot connect to " + bc_cs_central_settings.soap_server + " srs jobs types.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Function
                End If
            End If


            _view.load_view(SRSClients, SRSJobs, SRSActions, SRSErrors, SRSStats, SRSEmails, SRSEmailfilters, SRSJobTypes)

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_srs_schedule", "load_data", bc_cs_error_codes.USER_DEFINED, ex.Message)
            load_data = False
        End Try

    End Function


End Class

Public Interface Ibc_am_cp_monitor
    Function load_view(SRSClients As bc_srs_clientlist, SRSJobs As bc_srs_joblist, SRSActions As bc_srs_actionlist, SRSErrors As bc_srs_errorlist, SRSStates As bc_srs_statlist, SRSEmails As bc_srs_emaillist, SRSEmailfilters As bc_srs_emailfilters, SRSJobTypes As bc_srs_typelist) As Boolean
    Event save_monitor()
    Event save_sched(joblist As ArrayList, actionlist As ArrayList)
    Event refresh()
End Interface


Public Class TimeColumn
    Inherits DataGridViewColumn
    Public Sub New()
        MyBase.New(New TimeCell())
    End Sub
    Public Overrides Property CellTemplate() As DataGridViewCell
        Get
            Return MyBase.CellTemplate
        End Get
        Set(ByVal value As DataGridViewCell)
            If Not (value Is Nothing) AndAlso Not value.GetType().IsAssignableFrom(GetType(TimeCell)) Then
                Throw New InvalidCastException("Must be a TimeCell")
            End If
            MyBase.CellTemplate = value
        End Set
    End Property
End Class
Public Class TimeCell
    Inherits DataGridViewTextBoxCell
    Public Sub New()
        Me.Style.Format = "hh:mm:ss"  ' Use time format
    End Sub
    Public Overrides Sub InitializeEditingControl(ByVal rowIndex As Integer, ByVal initialFormattedValue As Object, ByVal dataGridViewCellStyle As DataGridViewCellStyle)

        MyBase.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle)

        Dim TheControl As TimeEditingControl = CType(DataGridView.EditingControl, TimeEditingControl)

        If IsDBNull(Me.Value) OrElse IsNothing(Me.Value) Then
            TheControl.Value = "#01/01/1900 9:00:00 AM#"

            ' Steve test 25/04/2018
            Me.Value = "#01/01/1900 9:00:00 AM#"

            TheControl.valueIsChanged = True

        Else
            TheControl.Value = CType(Me.Value, DateTime)
        End If

    End Sub
    Public Overrides ReadOnly Property EditType() As Type
        Get
            Return GetType(TimeEditingControl)
        End Get
    End Property
    Public Overrides ReadOnly Property ValueType() As Type
        Get
            Return GetType(DateTime)
        End Get
    End Property
    Public Overrides ReadOnly Property DefaultNewRowValue() As Object
        Get
            Return DateTime.Now
        End Get
    End Property
End Class

Class TimeEditingControl
    Inherits DateTimePicker
    Implements IDataGridViewEditingControl

    Private dataGridViewControl As DataGridView
    Public valueIsChanged As Boolean = False
    Private rowIndexNumber As Integer
    Public Sub New()
        Me.Format = DateTimePickerFormat.Custom
        Me.CustomFormat = "HH:mm:ss"
        Me.ShowUpDown = True

    End Sub
    Public Property EditingControlFormattedValue() As Object Implements IDataGridViewEditingControl.EditingControlFormattedValue
        Get
            Return Me.Value.ToShortTimeString
        End Get

        Set(ByVal value As Object)
            If TypeOf value Is String Then
                Me.Value = DateTime.Parse(CStr(value))
            End If
        End Set
    End Property
    Public Function GetEditingControlFormattedValue(ByVal context As DataGridViewDataErrorContexts) As Object _
        Implements IDataGridViewEditingControl.GetEditingControlFormattedValue

        'Return Me.Value.ToShortTimeString
        Return Me.Value.ToString("HH:mm:ss")

    End Function
    Public Sub ApplyCellStyleToEditingControl(ByVal dataGridViewCellStyle As DataGridViewCellStyle) Implements IDataGridViewEditingControl.ApplyCellStyleToEditingControl
        Me.Font = dataGridViewCellStyle.Font
        Me.CalendarForeColor = dataGridViewCellStyle.ForeColor
        Me.CalendarMonthBackground = dataGridViewCellStyle.BackColor
    End Sub
    Public Property EditingControlRowIndex() As Integer Implements IDataGridViewEditingControl.EditingControlRowIndex
        Get
            Return rowIndexNumber
        End Get
        Set(ByVal value As Integer)
            rowIndexNumber = value
        End Set
    End Property
    Public Function EditingControlWantsInputKey(ByVal key As Keys, ByVal dataGridViewWantsInputKey As Boolean) As Boolean Implements IDataGridViewEditingControl.EditingControlWantsInputKey
        Select Case key And Keys.KeyCode
            Case Keys.Left, Keys.Up, Keys.Down, Keys.Right, Keys.Home, Keys.End, Keys.PageDown, Keys.PageUp
                Return True
            Case Else
                Return False
        End Select
    End Function
    Public Sub PrepareEditingControlForEdit(ByVal selectAll As Boolean) Implements IDataGridViewEditingControl.PrepareEditingControlForEdit
        ' No preparation needs to be done.
    End Sub
    Public ReadOnly Property RepositionEditingControlOnValueChange() As Boolean Implements IDataGridViewEditingControl.RepositionEditingControlOnValueChange
        Get
            Return False
        End Get
    End Property
    Public Property EditingControlDataGridView() As DataGridView Implements IDataGridViewEditingControl.EditingControlDataGridView
        Get
            Return dataGridViewControl
        End Get
        Set(ByVal value As DataGridView)
            dataGridViewControl = value
        End Set
    End Property
    Public Property EditingControlValueChanged() As Boolean Implements IDataGridViewEditingControl.EditingControlValueChanged
        Get
            Return valueIsChanged
        End Get
        Set(ByVal value As Boolean)
            valueIsChanged = value
        End Set
    End Property
    Public ReadOnly Property EditingControlCursor() As Cursor Implements IDataGridViewEditingControl.EditingPanelCursor
        Get
            Return MyBase.Cursor
        End Get
    End Property
    Protected Overrides Sub OnValueChanged(ByVal eventargs As EventArgs)
        valueIsChanged = True
        Me.EditingControlDataGridView.NotifyCurrentCellDirty(True)
        MyBase.OnValueChanged(eventargs)
    End Sub
End Class










