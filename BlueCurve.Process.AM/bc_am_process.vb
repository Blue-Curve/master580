#Region " Imports "

Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports BlueCurve.Core.AS
Imports BlueCurve.Create.AM
Imports System.Threading
Imports System.Windows.Forms
Imports System.ComponentModel
Imports System.Runtime.InteropServices
Imports DevExpress.XtraBars
Imports DevExpress.XtraNavBar
Imports DevExpress.XtraTreeList
Imports System.Drawing




#End Region

Public Class bc_am_process

#Region " Constants "

    Private Const className = "bc_am_process"
    Private Const WORKFLOW_SETTINGS As String = "bc_am_process_settings.dat"

    ' Ribbon Action Constants
    Private Const DATE_FROM_CHECK_UNCHECK = "DATE_FROM_CHECK"
    Private Const DATE_TO_CHECK_UNCHECK = "DATE_TO_CHECK"
    Private Const DATE_FROM_VALUE_CHANGE = "DATE_FROM_VALUE"
    Private Const DATE_TO_VALUE_CHANGE = "DATE_TO_VALUE"
    Private Const REFRESH = "REFRESH"
    Private Const DOCUMENT_DETAILS_SHOW_HIDE = "DOC_DETAILS"
    Private Const DOCUMENT_FILTERS_SHOW_HIDE = "DOC_FILTERS"
    Private Const INCLUDE_PUBLISHED = "INCLUDE_PUBLISHED"
    Private Const CREATE = "CREATE"
    Private Const VIEW = "VIEW"
    Private Const DOCUMENT_STATE_CHANGE = "DOCUMENT_STATE_CHANGE"
    Private Const FORCE_CHECK_IN = "FORCE_CHECK_IN"
    Private Const CATEGORISE = "CATEGORISE"
    Private Const IMPORT_MASTER_DOCUMENT = "IMPORT_MASTER_DOCUMENT"
    Private Const IMPORT_SUPPORT_DOCUMENT = "IMPORT_SUPPORT_DOCUMENT"
    Private Const IMPORT_REGISTERED_DOCUMENT = "IMPORT_REGISTERED_DOCUMENT"
    Private Const REGISTER_MASTER_DOCUMENT = "REGISTER_MASTER_DOCUMENT"
    Private Const REATTACH_MASTER_DOCUMENT = "REATTACH_MASTER_DOCUMENT"
    Private Const APPLICATION_EXIT = "EXIT"
    Private Const DOCUMENT_RECENTLY_CHECKED_OUT = 0
    Private Const DOCUMENT_RECENTLY_STAGE_CHANGED = -1
    Private Const TAKE_REVISION_FAILED = -2
    Private Const FAILED_TO_REVERT_DOCUMENT = -3
    Private Const DOCUMENT_RECENTLY_CHECKED_IN = -4
    Private Const SETTINGS = "SETTINGS"
    Private Const COMPONENTS = "COMPONENTS"
    Private Const HTML_PREVIEW = "HTML_PREVIEW"
    Public Const RESET_FILTERS = "RESET_FILTERS"
    Public Const DISTRIBUTION = "DISTRIBUTION"
    Public Const PUBLISH_ONLY = "PUBLISH_ONLY"
    Public Const REGULAR = "REGULAR"
    Public Const ATTESTATION = "ATTESTATION"
    Public Const VIEWATTESTATION = "VIEWATTESTATION"
    Public Const CANCELDOC = "CANCELDOC"
    Public Const REJECTDOC = "REJECTDOC"
    Public Const ATTRIBUTES = "ATTRIBUTES"
    Public Const DISCLOSURE_FILE = "DISCLOSURE_FILE"
#End Region

#Region " Private Variables "

    Private pollingInterval As Long
    Private pollingEnabled As Boolean
    Private alerterEnabled As Boolean
    Private automaticdoclistupdate As Boolean
    Friend refreshsizechanged As Boolean
    Private screenPoll As Boolean
    Private userInactiveInterval As Long
    Private docs As New bc_om_documents
    Private ldocs As New bc_om_documents
    Private pDocs As New bc_om_documents
    Private nDocs As New ArrayList
    Private eDocs As New ArrayList
    Private uDocs As New ArrayList
    Private stDocs As New ArrayList
    Private processing As Boolean
    Private preExpireAlertNotify As Integer
    Private snapSummary As Boolean = True
    Private tPoll As Thread
    Private unReadMode As Boolean
    Private beepEnabled As Boolean
    Private fadeInterval As Integer
    Private selectedDocId As String
    Private daysBack As Integer


    REM filter settings
    Private oTaxonomies As New ArrayList
    Private sPubtypeID As New ArrayList
    Private sEntityID As New ArrayList
    Private sAuthorID As New ArrayList
    Private sDistID As New ArrayList
    Private sBusareaID As New ArrayList
    Private sStage As New ArrayList
    Private currDoc As New bc_om_document
    Private fDateFrom As Date
    Private fDateTo As Date
    Private lastScreenRefresh As Date
    Private fromScreen As Boolean
    Private screenRefreshInterval As Integer
    Private screenInactiveInterval As Integer
    Private formStateChanging As Boolean
    Private alertDoc As Long
    Private alertMode As Integer
    Private matchesOnly As Boolean
    Private msg As String
    Private fromService As Boolean = False
    Private doNotClear As Boolean
    Private workflowEnabled As Boolean
    Private sDoc As Boolean
    Private newStage As String
    Private newStageName As String
    Private createRunning As Boolean
    Private localDocument As New bc_om_document

    Private showpublishstate As Boolean = False

    Private containerView As bc_am_pr_main
    Private documentListView As bc_am_pr_document_list
    Private documentDetailsView As bc_am_pr_document_details
    Private galertmessage As String
    Public talertpoll As Thread

    Friend docList As New BindingList(Of documentListFields)

    Private Delegate Sub populateDocumentList(ByVal bd As BindingList(Of documentListFields))
    Private documentPopulation As populateDocumentList

    Private Delegate Sub pollingStatusDelegate()
    Private pollingStatus As pollingStatusDelegate

    Private Delegate Sub serverStatusDelegate(ByVal status As String)
    Private serverStatus As serverStatusDelegate

    Private gcomment As String
    'Private falert As bc_am_process_alert
    Private show_publish As Boolean = False
    Private bpublish_only As Boolean = False

    Public filter_items As New List(Of bc_om_documents.bc_om_document_filter_item)

    Private Class selected_filter_item
        Public type As String
        Public item As String
    End Class

#End Region

#Region " Friend Variables"

    Friend ScreenUpdateEnabled As Boolean
    Friend UserInactiveLast As Date
    Friend Mode As Integer
    Friend location As Integer = 0
    Friend polling_process As Boolean
    Friend screen_update_required As Boolean = False
    Friend AutoRefresh As Boolean = False

#End Region

#Region " Shared Variables "

    Friend Shared colors As New ProcessColors

#End Region

#Region " Constructors "

    Public Sub New()
        Try
            containerView = New bc_am_pr_main
            containerView.Controller = Me

            documentListView = New bc_am_pr_document_list
            documentListView.Controller = Me
            documentDetailsView = New bc_am_pr_document_details
            documentDetailsView.Controller = Me
            If bc_cs_central_settings.process_caption <> "" Then
                containerView.Text = bc_cs_central_settings.process_caption
            End If



            documentPopulation = New populateDocumentList(AddressOf bindDocumentList)
            pollingStatus = New pollingStatusDelegate(AddressOf updateRibbonStatusBar)
            serverStatus = New serverStatusDelegate(AddressOf setServerStatus)
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_process", "New", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Sub

    Public Sub New(ByRef container As bc_am_pr_main, ByRef documentList As bc_am_pr_document_list, ByRef documentDetails As bc_am_pr_document_details)

        containerView = container
        containerView.Controller = Me
        documentListView = documentList
        documentListView.Controller = Me
        documentDetailsView = documentDetails
        documentDetailsView.Controller = Me

        documentPopulation = New populateDocumentList(AddressOf bindDocumentList)
        pollingStatus = New pollingStatusDelegate(AddressOf updateRibbonStatusBar)
        serverStatus = New serverStatusDelegate(AddressOf setServerStatus)

    End Sub

#End Region

    Public Sub Load()
        Dim moduleName As String = "Load"
        Dim slog As New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim i As Integer
            mainFormDefaults()
            Dim ows As New bc_om_workflow_settings

            REM load user settings if set else use defaults
            Dim fs As New bc_cs_file_transfer_services

            If fs.check_document_exists_and_can_be_read(bc_cs_central_settings.get_user_dir + "\" + CStr(bc_cs_central_settings.logged_on_user_id) + "_" + WORKFLOW_SETTINGS) = True Then
                Try
                    ows = New bc_om_workflow_settings
                    ows = ows.read_data_from_file(bc_cs_central_settings.get_user_dir + "\" + CStr(bc_cs_central_settings.logged_on_user_id) + "_" + WORKFLOW_SETTINGS)
                    ows.turned_on = True
                    ows.write_data_to_file(bc_cs_central_settings.get_user_dir + "\" + CStr(bc_cs_central_settings.logged_on_user_id) + "_" + WORKFLOW_SETTINGS)
                    pollingInterval = ows.polling_interval

                    snapSummary = ows.snapmode
                    userInactiveInterval = ows.user_inactive_interval
                    automaticdoclistupdate = ows.automaticdoclistupdate
                    pollingEnabled = ows.polling_enabled
                    alerterEnabled = ows.alerter_enabled
                    refreshsizechanged = ows.refreshsizechanged
                    preExpireAlertNotify = ows.pre_expire_alert_notify
                    screenPoll = ows.screen_update
                    If fromService = False Then
                        Mode = ows.mode
                    End If
                    unReadMode = ows.unread_mode
                    beepEnabled = ows.beep_enabled
                    fadeInterval = ows.fade_interval
                    AutoRefresh = CBool(ows.auto_refresh)
                    sEntityID = ows.sentity
                    sAuthorID = ows.sauthor
                    sStage = ows.sstage
                    sPubtypeID = ows.spubtype
                    sBusareaID = ows.sbus
                    daysBack = ows.days_back

                    Dim t As New TimeSpan(daysBack, 0, 0, 0, 0)
                    fDateFrom = Now.Subtract(t)

                    screenRefreshInterval = ows.screen_refresh_interval
                    screenInactiveInterval = ows.screen_inactive_interval
                    colors.loadFromConfig(ows.colors)
                    docs.document.Clear()
                    If Not IsNothing(ows.ndocs) Then
                        For i = 0 To ows.ndocs.document.Count - 1
                            docs.document.Add(ows.ndocs.document(i))
                        Next
                    End If
                Catch

                End Try
            End If

          
            If bc_am_load_objects.obc_pub_types.process_switches.show_publish_default = True Then
                containerView.uxIncludePublished.EditValue = True
                show_publish = Not show_publish
            End If
            Me.RetrieveDocs(True, True)


            tPoll = New Thread(AddressOf documentListPolling)
            tPoll.Priority = ThreadPriority.Normal
            tPoll.Start()

            containerView.columnorder = ows.columnorder
            containerView.columnwidth = ows.columnwidth
            containerView.WindowState = FormWindowState.Maximized


            'DEV EXPRESS
            mainFormState()
            set_status_text()


            'check pub types to see if create and import can work
            containerView.uxCreate.Enabled = False
            containerView.uxImportMasterDocument.Enabled = False
            containerView.uxRegisterMasterDocument.Enabled = False



            For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                REM filter pub types for users bus area
                For j = 0 To bc_am_load_objects.obc_prefs.bus_areas.Count - 1
                    If bc_am_load_objects.obc_prefs.bus_areas(j) = bc_am_load_objects.obc_pub_types.pubtype(i).bus_area_id And bc_am_load_objects.obc_pub_types.pubtype(i).support_doc_only = False Then
                        If bc_am_load_objects.obc_pub_types.pubtype(i).show_in_wizard = True Then
                            containerView.uxCreate.Enabled = True
                        End If
                        containerView.uxImportMasterDocument.Enabled = True
                        containerView.uxRegisterMasterDocument.Enabled = True


                    End If
                Next
            Next

            REM if 1 or more pub types are distributable show button group
            Me.containerView.uxdiststatus.Visible = False




            containerView.uxregular.Visibility = BarItemVisibility.Never

            If bc_am_load_objects.obc_pub_types.process_switches.regular_reports = True Then
                containerView.uxregular.Visibility = BarItemVisibility.Always
            End If
            containerView.uxattributes.Visibility = BarItemVisibility.Never
            If bc_am_load_objects.obc_pub_types.process_switches.attributes_screen = True Then
                containerView.uxattributes.Visibility = BarItemVisibility.Always
            End If

            REM register
            If bc_am_load_objects.obc_pub_types.process_switches.register_document = False Then

                containerView.uxRegisterMasterDocument.Visibility = BarItemVisibility.Never

                containerView.uxImportRegisteredDocument.Visibility = BarItemVisibility.Never
            End If
            REM reattach
            If bc_am_load_objects.obc_pub_types.process_switches.reattach_master = False Then
                containerView.uxReattachMasterDocument.Visibility = BarItemVisibility.Never
            End If
            REM master
            If bc_am_load_objects.obc_pub_types.process_switches.import_master = False Then
                containerView.uxImportMasterDocument.Visibility = BarItemVisibility.Never
            End If
            REM attestation

            containerView.uxattestationgroup.Visible = False

            If bc_am_load_objects.obc_pub_types.process_switches.attestation_screen = True Then
                containerView.uxattestationgroup.Visible = True
                If containerView.columnorder.Count > 11 AndAlso containerView.columnorder(12) = -1 Then
                    Me.documentListView.uxDocumentListView.Columns(12).Visible = False
                End If
                If containerView.columnorder.Count > 12 AndAlso containerView.columnorder(13) = -1 Then
                    Me.documentListView.uxDocumentListView.Columns(13).Visible = False
                End If
            Else
                Me.documentListView.uxDocumentListView.Columns(12).Visible = False
                Me.documentListView.uxDocumentListView.Columns(13).Visible = False
            End If
            containerView.uxstopdocgroup.Visible = False
            If bc_am_load_objects.obc_pub_types.process_switches.stop_document Then
                containerView.uxstopdocgroup.Visible = True
            End If
            If bc_am_load_objects.obc_pub_types.process_switches.fast_track = True Then
                If containerView.columnorder.Count > 13 AndAlso containerView.columnorder(14) = -1 Then
                    Me.documentListView.uxDocumentListView.Columns(14).Visible = False
                End If
            Else
                Me.documentListView.uxDocumentListView.Columns(14).Visible = False
            End If

            Dim dfound As Boolean = False
            For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                If bc_am_load_objects.obc_pub_types.pubtype(i).distributable = True AndAlso bc_am_load_objects.obc_pub_types.process_switches.distribute = True Then
                    containerView.rdistribute.Visible = True
                    containerView.rdistribute.Enabled = False
                    containerView.uxpublishonly.Visibility = BarItemVisibility.Always
                    If containerView.columnorder.Count > 10 AndAlso containerView.columnorder(11) = -1 Then
                        Me.documentListView.uxDocumentListView.Columns(11).Visible = False
                    Else
                        Me.documentListView.uxDocumentListView.Columns(11).Visible = True
                    End If
                    Me.containerView.uxdiststatus.Visible = True
                    documentDetailsView.lsubdate.Text = "Distribution:"
                    documentDetailsView.btax.Visible = True
                    dfound = True
                    Exit For
                End If
            Next
            If dfound = False Then
                Me.documentListView.uxDocumentListView.Columns(11).Visible = False
            End If
            If bc_am_load_objects.obc_pub_types.process_switches.attribute_change = True Then
                If containerView.columnorder.Count > 15 AndAlso containerView.columnorder(15) = -1 Then
                    Me.documentListView.uxDocumentListView.Columns(15).Visible = False
                End If
            Else
                Me.documentListView.uxDocumentListView.Columns(15).Visible = False
            End If
            containerView.uxgenerate.Visible = False

            If bc_am_load_objects.obc_pub_types.process_switches.disclosure_file = True Then
                containerView.uxgenerate.Visible = True

            End If


            containerView.ShowDialog()

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log(className, moduleName, bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

            slog = New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub


    Public Function submitcomment(ByVal tx As String) As Boolean
        Dim ocomment As New bc_om_comment
        Try
            submitcomment = False
            ocomment.comment = tx
            ocomment.doc_id = localDocument.id

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                ocomment.db_write()
                submitcomment = True
            Else
                ocomment.tmode = bc_cs_soap_base_class.tWRITE

                If ocomment.transmit_to_server_and_receive(ocomment, True) = False Then
                    Exit Function
                Else
                    submitcomment = True
                End If
            End If
            REM reload most recent comment
            load_document_comments_from_system(1)
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_process", "submitcomment", bc_cs_error_codes.USER_DEFINED, ex.Message)
            submitcomment = False
        Finally
            If submitcomment = False Then
                Dim omsg As New bc_cs_message("Blue Curve", "Failed to Submit Comment", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            End If
        End Try
    End Function


    Private Sub mainFormDefaults()
        Dim moduleName As String = "mainFormDefaults"
        Dim slog As New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            pollingInterval = 60
            pollingEnabled = False
            alerterEnabled = False
            refreshsizechanged = False
            screenPoll = False
            ScreenUpdateEnabled = True
            preExpireAlertNotify = 4
            beepEnabled = False
            daysBack = 1
            fadeInterval = 5
            automaticdoclistupdate = False

            unReadMode = False
            AutoRefresh = False
            lastScreenRefresh = "1-1-1900"

            Try
                Dim t As New TimeSpan(1, 0, 0, 0, 0)
                fDateTo = "9-9-9999"
                fDateFrom = Now.Subtract(t)
                selectedDocId = 0
                snapSummary = True
                'setPollingStatus()
            Catch

            End Try

            fromScreen = False
            screenRefreshInterval = 5
            screenInactiveInterval = 60
            userInactiveInterval = 10

            containerView.uxShowFilters.Checked = True

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log(className, moduleName, bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Private Sub writeUserSettingsToFile(Optional ByVal turned_on As Boolean = False)
        Dim moduleName = "write_user_settings_to_file"
        Dim slog As New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            REM write down system settings prior to closing app
            Dim ows As New bc_om_workflow_settings
            ows.turned_on = turned_on
            ows.polling_enabled = pollingEnabled
            ows.polling_interval = pollingInterval
            ows.automaticdoclistupdate = automaticdoclistupdate
            ows.alerter_enabled = alerterEnabled
            ows.refreshsizechanged = refreshsizechanged
            ows.screen_update = screenPoll
            ows.user_inactive_interval = userInactiveInterval

            ows.snapmode = snapSummary
            ows.pre_expire_alert_notify = preExpireAlertNotify
            ows.ndocs.document.Clear()
            ows.ndocs = docs
            ows.mode = Mode
            ows.unread_mode = unReadMode
            ows.beep_enabled = beepEnabled
            ows.fade_interval = fadeInterval
            ows.auto_refresh = AutoRefresh
            ows.sentity = sEntityID
            ows.sauthor = sAuthorID
            ows.sstage = sStage
            ows.spubtype = sPubtypeID
            ows.sbus = sBusareaID
            ows.days_back = daysBack
            ows.fdatefrom = fDateFrom
            ows.fdateto = fDateTo
            ows.screen_refresh_interval = screenRefreshInterval
            ows.screen_inactive_interval = screenInactiveInterval
            ows.colors = colors.convertForConfig
            For i = 0 To documentListView.uxDocumentListView.Columns.Count - 1
                ows.columnorder.Add(documentListView.uxDocumentListView.Columns(i).VisibleIndex)
            Next
            For i = 0 To documentListView.uxDocumentListView.Columns.Count - 1
                ows.columnwidth.Add(documentListView.uxDocumentListView.Columns(i).Width)
            Next



            ows.write_data_to_file(bc_cs_central_settings.get_user_dir + "\" + CStr(bc_cs_central_settings.logged_on_user_id) + "_" + WORKFLOW_SETTINGS)
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log(className, moduleName, bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Public Sub includepublished(ByVal show_publish)
        Me.show_publish = show_publish
        RetrieveDocs()


    End Sub
    Private Delegate Sub RetrieveDocsDelegate(ByVal update_display As Boolean, ByVal always_refresh As Boolean, ByVal from_poll As Boolean)
    Public Sub RetrieveDocs(Optional ByVal update_display As Boolean = True, Optional ByVal always_refresh As Boolean = False, Optional ByVal from_poll As Boolean = False)
        Dim moduleName As String = "retrieveDocsAfterPollingCheck"
        Dim slog As New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_ENTRY, "auto: " + CStr(AutoRefresh))
        Try
            If processing = True Then
                Exit Sub
            End If


            REM if called from poll marshall to UI thread
            If Me.documentListView.uxDocumentList.InvokeRequired Then
                Dim ed As New RetrieveDocsDelegate(AddressOf RetrieveDocs)
                Me.documentListView.Invoke(ed, New Object() {False, False, True})
                Exit Sub
            End If


            processing = False
            If from_poll = False Then
                Cursor.Current = Cursors.WaitCursor
            End If

            If location = 0 Or from_poll = True Then
                pDocs.document.Clear()
                For i = 0 To docs.document.Count - 1
                    pDocs.document.Add(docs.document(i))
                Next
                docs = New bc_om_documents
                docs.document.Clear()
                docs.workflow_mode = True
                If fDateFrom <> "9-9-9999" Then
                    docs.date_from = fDateFrom.ToUniversalTime
                Else
                    docs.date_from = "9-9-9999"
                End If

                If fDateTo <> "9-9-9999" Then
                    docs.date_to = fDateTo.ToUniversalTime
                Else
                    docs.date_to = "9-9-9999"
                End If
                docs.show_publish = show_publish
                docs.publish_only = bpublish_only


                If update_display = True Then
                    docs.packet_code = "process"
                Else
                    docs.packet_code = "processpoll"
                End If

                Me.filter_items.Clear()
                docs.filter_items.Clear()


                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    docs.db_read()
                Else
                    docs.tmode = bc_cs_soap_base_class.tREAD
                    If docs.transmit_to_server_and_receive(docs, True) = False Then
                        Exit Sub
                    End If
                End If
                Dim evnd As Boolean
                evnd = False
                evnd = evalNewDoc()
                filter_items = docs.filter_items

                If pDocs = docs And screen_update_required = False And always_refresh = False And evnd = False Then
                    Exit Sub
                End If

                If from_poll = True AndAlso Me.automaticdoclistupdate = True Then
                    If pDocs <> docs Or always_refresh = True Or evnd = True Then
                        Me.location = 0

                        loadData()
                    End If
                Else
                    If update_display = True Then
                        If pDocs <> docs Or screen_update_required = True Or always_refresh = True Then
                            loadData()
                        End If
                        screen_update_required = False
                    Else
                        If pDocs <> docs Or evnd = True Then
                            REM a poll has deteced that the screen requires updating
                            screen_update_required = True
                        End If
                    End If
                End If

                pDocs.document.Clear()
            End If
            If location = 1 Then
                ldocs = New bc_om_documents
                Dim fs As New bc_cs_file_transfer_services
                If fs.check_document_exists_and_can_be_read(bc_cs_central_settings.local_repos_path + "bc_local_documents.dat") = True Then
                    ldocs = New bc_om_documents
                    ldocs = ldocs.read_data_from_file(bc_cs_central_settings.local_repos_path + "bc_local_documents.dat")
                    Dim i As Integer
                    REM entity_name
                    Dim fi As bc_om_documents.bc_om_document_filter_item
                    fi = New bc_om_documents.bc_om_document_filter_item
                    fi.type_name = "Stage"
                    fi.item = "Unsubmitted"
                    ldocs.filter_items.Add(fi)

                    REM eliminate docs out of date range and docs that are checked out
                    While i < ldocs.document.Count
                        If ldocs.document(i).id <> 0 Or ldocs.document(i).doc_date < fDateFrom Or ldocs.document(i).doc_date > fDateTo Then
                            ldocs.document.RemoveAt(i)
                        Else

                            REM set up required metadata and add to filters
                            REM entity_name
                            Dim found As Boolean
                            fi = New bc_om_documents.bc_om_document_filter_item
                            fi.type_name = "Publication Type"
                            fi.item = ldocs.document(i).pub_type_name
                            ldocs.filter_items.Add(fi)

                            ldocs.document(i).stage_name = "Unsubmitted"
                            ldocs.document(i).pub_type_name = ldocs.document(i).pub_type_name
                            ldocs.document(i).checked_out_user_name = "me"
                            ldocs.document(i).checked_out_user = bc_cs_central_settings.logged_on_user_id

                            For j = 0 To bc_am_load_objects.obc_pub_types.languages.Count - 1
                                If bc_am_load_objects.obc_pub_types.languages(j).id = ldocs.document(i).language_id Then
                                    ldocs.document(i).language_name = bc_am_load_objects.obc_pub_types.languages(j).name
                                    Exit For
                                End If
                            Next

                            For j = 0 To bc_am_load_objects.obc_pub_types.bus_areas.Count - 1
                                If bc_am_load_objects.obc_pub_types.bus_areas(j).id = ldocs.document(i).bus_area Then
                                    ldocs.document(i).bus_area = bc_am_load_objects.obc_pub_types.bus_areas(j).description
                                    Exit For
                                End If
                            Next

                            found = False
                            If ldocs.document(i).entity_id = 0 Then
                                ldocs.document(i).lead_entity_name = "none"

                                For j = 0 To ldocs.filter_items.Count - 1
                                    If ldocs.filter_items(j).type_name = "Lead Classification" And ldocs.filter_items(j).item = "none" Then
                                        found = True
                                        Exit For
                                    End If
                                Next

                                If found = False Then
                                    fi = New bc_om_documents.bc_om_document_filter_item
                                    fi.type_name = "Lead Classification"
                                    fi.item = "none"
                                    ldocs.filter_items.Add(fi)
                                End If

                            Else
                                For k = 0 To bc_am_load_objects.obc_entities.entity.Count - 1
                                    If bc_am_load_objects.obc_entities.entity(k).id = ldocs.document(i).entity_id Then
                                        ldocs.document(i).lead_entity_name = bc_am_load_objects.obc_entities.entity(k).name
                                        For j = 0 To ldocs.filter_items.Count - 1
                                            If ldocs.filter_items(j).type_name = "Lead Classification" And ldocs.filter_items(j).item = ldocs.document(i).lead_entity_name Then
                                                found = True
                                                Exit For
                                            End If

                                        Next
                                        If found = False Then
                                            fi = New bc_om_documents.bc_om_document_filter_item

                                            fi.type_name = "Lead Classification"
                                            fi.item = ldocs.document(i).lead_entity_name
                                            ldocs.filter_items.Add(fi)
                                        End If
                                        Exit For
                                    End If
                                Next
                            End If
                            REM originator name
                            found = False
                            For k = 0 To bc_am_load_objects.obc_users.user.Count - 1
                                If bc_am_load_objects.obc_users.user(k).id = ldocs.document(i).originating_author Then
                                    ldocs.document(i).originator_name = bc_am_load_objects.obc_users.user(k).first_name + " " + bc_am_load_objects.obc_users.user(k).surname
                                    For j = 0 To ldocs.filter_items.Count - 1
                                        If ldocs.filter_items(j).type_name = "Originator" And ldocs.filter_items(j).item = ldocs.document(i).originator_name Then
                                            found = True
                                            Exit For
                                        End If
                                    Next
                                    If found = False Then
                                        fi = New bc_om_documents.bc_om_document_filter_item
                                        fi.type_name = "Originator"
                                        fi.item = ldocs.document(i).originator_name
                                        ldocs.filter_items.Add(fi)
                                    End If

                                    Exit For
                                End If
                            Next
                            i = i + 1
                        End If
                    End While
                End If
                filter_items = ldocs.filter_items
                loadData(True)
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log(className, moduleName, bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

            Cursor.Current = Cursors.Default
            processing = False
            slog = New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_EXIT, "auto: " + CStr(AutoRefresh))
        End Try
    End Sub

    Private Sub setServerStatus(ByVal status As String)

        containerView.uxStatus.Caption = String.Concat("Status: ", status)

    End Sub
    Public last_refresh_date = "1-1-1900"
    Friend Sub set_status_text()
        containerView.uxPollingRefresh.Caption = ""
        If AutoRefresh = True Then
            containerView.uxPollingRefresh.Caption = containerView.uxPollingRefresh.Caption + "Auto Refresh After " + CStr(screenInactiveInterval) + " Seconds. "
        End If
        If pollingEnabled = True Then
            containerView.uxPollingRefresh.Caption = containerView.uxPollingRefresh.Caption + "Poll Every " + CStr(pollingInterval) + " Seconds. "
            If automaticdoclistupdate Then
                containerView.uxPollingRefresh.Caption = containerView.uxPollingRefresh.Caption + "(auto update list). "
            End If
        End If
        If alerterEnabled = True Then
            containerView.uxPollingRefresh.Caption = containerView.uxPollingRefresh.Caption + "Alter enabled. "
        End If
        If refreshsizechanged = True Then
            containerView.uxPollingRefresh.Caption = containerView.uxPollingRefresh.Caption + "Refresh After Size Changed. "
        End If
        containerView.uxPollingRefresh.Caption = containerView.uxPollingRefresh.Caption + "Last Refreshed " + Format(last_refresh_date, "dd-MMM-yyyy HH:mm:ss")
    End Sub

    Private Sub documentListPolling()
        Dim moduleName As String = "documentListPolling"
        Dim slog As New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_ENTRY, "auto: " + CStr(AutoRefresh))

        Try
            Dim k As Integer

            Thread.Sleep(1000)

            While k = 0
                If pollingEnabled = True Then
                    Thread.Sleep(pollingInterval * 1000)
                    Dim ocommentary As New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.COMMENTARY, "DOCUMENT POLL")
                    Me.RetrieveDocs(False)

                Else
                    Thread.Sleep(10000)

                End If
            End While
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log(className, moduleName, bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub

    Private Function getDocTitle(ByVal doc_id As Long) As String
        Dim moduleName As String = "getDocTitle"
        Dim slog As New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            Dim i As Integer

            getDocTitle = ""

            For i = 0 To docs.document.Count - 1
                If doc_id = docs.document(i).id Then
                    getDocTitle = docs.document(i).title
                    Exit For
                End If
            Next

        Catch ex As Exception
            getDocTitle = ""
            Dim db_err As New bc_cs_error_log(className, moduleName, bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            polling_process = False
            slog = New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function

    Private Sub alert()
        Dim moduleName As String = "alert"
        Dim slog As New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            alertDoc = 0
            alertMode = 0
            msg = ""

            If nDocs.Count = 1 Then
                msg = "New Document has Arrived: " + getDocTitle(nDocs(0)) + ""
                alertDoc = nDocs(0)
                alertMode = 1
            End If
            If nDocs.Count > 1 Then
                If msg = "" Then
                    msg = CStr(nDocs.Count) + " Documents have Arrived"
                    alertMode = 1
                Else
                    alertMode = 0
                    msg = msg + "; " + CStr(nDocs.Count) + " Documents have Arrived"
                End If
            End If
            If eDocs.Count = 1 Then
                If msg = "" Then
                    msg = msg + "Document has Expired: " + getDocTitle(eDocs(0))
                    alertDoc = eDocs(0)
                    alertMode = 3
                Else
                    alertMode = 0
                    msg = msg + "; Document has Expired"
                End If
            End If
            If eDocs.Count > 1 Then
                If msg = "" Then
                    msg = CStr(eDocs.Count) + " Documents have Expired"
                    alertMode = 3
                Else
                    alertMode = 0
                    msg = msg + "; " + CStr(eDocs.Count) + " Documents have Expired"
                End If
            End If
            If uDocs.Count = 1 Then
                If msg = "" Then
                    msg = msg + "Document has become urgent: " + getDocTitle(uDocs(0))
                    alertDoc = uDocs(0)
                    alertMode = 2
                Else
                    alertMode = 0
                    msg = msg + "; Document has become urgent"
                End If
            End If
            If uDocs.Count > 1 Then
                If msg = "" Then
                    msg = CStr(uDocs.Count) + " Document have become urgent"
                    alertMode = 2
                Else
                    msg = msg + "; " + CStr(uDocs.Count) + " Documents have become urgent"
                    alertMode = 0
                End If
            End If
            If stDocs.Count = 1 Then
                If msg = "" Then
                    msg = msg + "Document has changed stage: " + getDocTitle(stDocs(0))
                    alertDoc = stDocs(0)
                    alertMode = 4
                Else
                    msg = msg + "; " + "Document has changed stage: " + getDocTitle(stDocs(0))
                    alertMode = 0
                End If
            End If
            If stDocs.Count > 1 Then
                If msg = "" Then
                    msg = CStr(stDocs.Count) + " Documents have changed stage"
                    alertMode = 4
                Else
                    msg = msg + "; " + CStr(stDocs.Count) + " Documents have changed stage"
                    alertMode = 0
                End If
            End If
            Me.containerView.uxalert.AutoFormDelay = (Me.fadeInterval * 1000)
            Me.containerView.uxalert.FormLocation = Alerter.AlertFormLocation.BottomRight
            Me.containerView.uxalert.Show(Nothing, "Blue Curve - Process", msg)

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log(className, moduleName, bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Friend Sub alert_clicked()
        Dim modulename As String = "alert_clicked"
        Dim slog As New bc_cs_activity_log(className, modulename, bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Me.containerView.WindowState = FormWindowState.Maximized

            clear_filters()
            If location = 1 Then
                location = 0
                RetrieveDocs()
            End If
            If alertDoc <> 0 Then
                Dim rowselected As Boolean = False
                For i = 0 To docList.Count - 1
                    If alertDoc = docList(documentListView.uxDocumentListView.GetDataSourceRowIndex(i)).ID Then
                        documentListView.uxDocumentListView.FocusedRowHandle = i
                        selectDocument()
                        Exit For
                    End If
                Next
            End If

            Me.containerView.Activate()


        Catch ex As Exception
            Dim db_err As New bc_cs_error_log(className, modulename, bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

            slog = New bc_cs_activity_log(className, modulename, bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub


    Private Function evalNewDoc() As Boolean
        Dim slog As New bc_cs_activity_log("bc_am_process", "eval_new_doc", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            evalNewDoc = False
            Dim i, j As Integer
            Dim found As Boolean
            nDocs.Clear()
            eDocs.Clear()
            uDocs.Clear()
            stDocs.Clear()
            REM see if new docs have arrived           
            For i = 0 To docs.document.Count - 1
                docs.document(i).new_flag = False
                found = False
                For j = 0 To pDocs.document.Count - 1
                    If docs.document(i).id = pDocs.document(j).id Then
                        docs.document(i).new_flag = pDocs.document(j).new_flag
                        docs.document(i).expire_flag = pDocs.document(j).expire_flag
                        docs.document(i).urgent_flag = pDocs.document(j).urgent_flag
                        docs.document(i).unread = pDocs.document(j).unread
                        docs.document(i).search_flag = pDocs.document(j).search_flag
                        docs.document(i).acknowledged = pDocs.document(j).acknowledged
                        docs.document(i).arrive = pDocs.document(j).arrive
                        docs.document(i).stage_change_flag = pDocs.document(j).stage_change_flag
                        REM check here if existing document has now become urgent or expired
                        If docs.document(i).stage_expire_date.tolocaltime < Now And docs.document(i).expire_flag = False Then
                            docs.document(i).expire_flag = True
                            docs.document(i).urgent_flag = False
                            docs.document(i).arrive = Now
                            eDocs.Add(docs.document(i).id)
                            evalNewDoc = True
                        End If
                        If Now.AddDays(preExpireAlertNotify) > docs.document(i).stage_expire_date.tolocaltime And Now < docs.document(i).stage_expire_date.tolocaltime And docs.document(i).urgent_flag = False Then
                            docs.document(i).urgent_flag = True
                            docs.document(i).expire_flag = False
                            docs.document(i).arrive = Now
                            uDocs.Add(docs.document(i).id)
                            evalNewDoc = True
                        End If
                        If docs.document(i).stage_expire_date.tolocaltime > Now Then
                            docs.document(i).expire_flag = False
                        End If
                        If docs.document(i).stage_expire_date.tolocaltime > Now.AddDays(preExpireAlertNotify) Then
                            docs.document(i).urgent_flag = False
                        End If
                        REM check if document has changed stage
                        If docs.document(i).stage <> pDocs.document(j).stage Then
                            docs.document(i).stage_change_flag = True
                            docs.document(i).arrive = Now
                            stDocs.Add(docs.document(i).id)
                        End If
                        found = True
                        Exit For
                    End If
                Next
                If found = False Then
                    docs.document(i).new_flag = True
                    docs.document(i).unread = True
                    docs.document(i).arrive = Now
                    nDocs.Add(docs.document(i).id)
                End If
            Next
            If (nDocs.Count > 0 Or eDocs.Count > 0 Or uDocs.Count > 0 Or stDocs.Count > 0) And alerterEnabled = True Then
                alert()
                If Me.beepEnabled = True Then
                    Beep()
                End If
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_process", "eval_new_doc", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_process", "eval_new_doc", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
    Private Function get_filename_from_revision_tag(ByVal tag As String)
        get_filename_from_revision_tag = Left(tag, InStr(tag, "<") - 1)
    End Function

    Private Function get_stage_from_revision_tag(ByVal tag As String)
        get_stage_from_revision_tag = Right(tag, Len(tag) - InStr(tag, "<"))
        get_stage_from_revision_tag = Left(get_stage_from_revision_tag, Len(get_stage_from_revision_tag) - 1)
    End Function

    Friend Sub selectdocumentrevision(ByVal tag As String)
        Dim stage As String
        stage = get_stage_from_revision_tag(tag)
        Me.documentDetailsView.uxViewRevision.Enabled = False
        Me.documentDetailsView.uxRevertRevision.Enabled = False
        If localDocument.read = True Then
            Me.documentDetailsView.uxViewRevision.Enabled = True
        End If
        If localDocument.write = True And localDocument.stage_name = stage And localDocument.checked_out_user = 0 Then
            Me.documentDetailsView.uxRevertRevision.Enabled = True
        End If

    End Sub

    Friend Sub selectsupportdocument(ByVal tag As String)
        Me.documentDetailsView.uxViewSupportDocument.Enabled = False
        If localDocument.read = True And Me.documentDetailsView.uxsupportdocs.Nodes.Count > 0 Then
            Me.documentDetailsView.uxViewSupportDocument.Enabled = True
        End If
        Me.documentDetailsView.uxcatsupportdoc.Enabled = False
        Me.documentDetailsView.uxCheckInOutSupportDoc.Enabled = False
        Me.documentDetailsView.uxDeleteSupportDoc.Enabled = False

        If localDocument.write = True Then
            For i = 0 To localDocument.support_documents.Count - 1
                If localDocument.support_documents(i).id = tag Then
                    If localDocument.support_documents(i).checked_out_user = 0 And (localDocument.checked_out_user = 0 Or localDocument.checked_out_user = bc_cs_central_settings.logged_on_user_id) Then
                        Me.documentDetailsView.uxcatsupportdoc.Enabled = True
                        Me.documentDetailsView.uxDeleteSupportDoc.Enabled = True
                        Me.documentDetailsView.uxViewSupportDocument.Text = "View"
                        Me.documentDetailsView.uxCheckInOutSupportDoc.Text = "Check Out"
                        Me.documentDetailsView.uxCheckInOutSupportDoc.Enabled = True
                        Me.documentDetailsView.uxCheckInOutSupportDoc.ImageIndex = 36

                    ElseIf localDocument.support_documents(i).checked_out_user = bc_cs_central_settings.logged_on_user_id Then
                        Me.documentDetailsView.uxcatsupportdoc.Enabled = False
                        Me.documentDetailsView.uxDeleteSupportDoc.Enabled = False
                        Me.documentDetailsView.uxViewSupportDocument.Text = "Edit"
                        Me.documentDetailsView.uxCheckInOutSupportDoc.Text = "Check In"
                        Me.documentDetailsView.uxCheckInOutSupportDoc.Enabled = True
                        Me.documentDetailsView.uxCheckInOutSupportDoc.ImageIndex = 35
                    End If
                    Exit For
                End If
            Next
        End If
    End Sub
    Friend Sub viewsupportdoc(ByVal tag As String)
        Dim slog As New bc_cs_activity_log("bc_am_process", "viewsupportdoc", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try

            Dim sdoc As New bc_om_document
            sdoc.id = tag

            For i = 0 To localDocument.support_documents.Count - 1
                If localDocument.support_documents(i).id = sdoc.id Then
                    sdoc.filename = localDocument.support_documents(i).filename
                    sdoc.extension = localDocument.support_documents(i).extension
                    Exit For
                End If
            Next

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                sdoc.db_read_physical_doc_view_only()

            Else
                sdoc.tmode = bc_cs_soap_base_class.tREAD
                sdoc.read_mode = bc_om_document.GET_PHYSICAL_DOC_VIEW_ONLY

                If sdoc.transmit_to_server_and_receive(sdoc, True) = False Then
                    Exit Sub
                End If
            End If
            If sdoc.doc_not_found = True Then
                Dim omsg As New bc_cs_message("Blue Curve", "Support Document not found on server", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Sub
            End If

            open_read_only(sdoc)
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_process", "viewsupportdoc", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_process", "viewsupportdoc", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Private Sub open_read_only(ByVal sdoc As bc_om_document)

        Dim slog As New bc_cs_activity_log("bc_am_process", "open_read_only", bc_cs_activity_codes.TRACE_EXIT, "")
        Try
            Dim fn As String
            fn = saveViewOnlyFile(sdoc)

            Dim omime As New bc_am_mime_types
            Dim ext As String
            ext = Right(fn, Len(fn) - InStrRev(fn, ".") + 1)

            Dim odoc As bc_ao_at_object
            If ext = ".doc" Or ext = ".docx" Then
                odoc = New bc_ao_word
                odoc.open_read_only(bc_cs_central_settings.local_repos_path + fn, fn)
            ElseIf sdoc.extension = ".ppt" Or sdoc.extension = ".pptx" Then
                odoc = New bc_ao_powerpoint
                odoc.open_read_only(bc_cs_central_settings.local_repos_path + fn, fn)
            Else
                omime = New bc_am_mime_types
                omime.view(bc_cs_central_settings.local_repos_path + fn, ext)
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_process", "open_read_only", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_process", "open_read_only", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Private Sub clear_down_local_doc()
        localDocument.byteDoc = Nothing
        localDocument.comments.comments.Clear()
        localDocument.history.Clear()
        localDocument.links.Clear()
        localDocument.support_documents.Clear()
    End Sub
    Friend Sub viewrevision(ByVal tag As String)
        Dim slog As New bc_cs_activity_log("bc_am_process", "viewrevision", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            Dim fn As String
            Dim sdoc As New bc_om_document
            fn = get_filename_from_revision_tag(tag)


            sdoc.revision_filename = fn
            sdoc.extension = Right(fn, Len(fn) - InStrRev(fn, ".") + 1)
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                sdoc.db_get_revision()
            Else
                sdoc.tmode = bc_cs_soap_base_class.tREAD
                sdoc.read_mode = bc_om_document.GET_REVISION

                If sdoc.transmit_to_server_and_receive(sdoc, True) = False Then
                    Exit Sub
                End If
            End If
            If sdoc.doc_not_found = True Then
                Dim omsg As New bc_cs_message("Blue Curve", "Revision: " + fn + " not found on server", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)

                Exit Sub
            End If

            open_read_only(sdoc)

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_process", "viewrevision", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_process", "viewrevision", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Friend Sub revertdocument(ByVal tag As String)
        Dim slog As New bc_cs_activity_log("bc_am_process", "revertdocument", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim omsg As New bc_cs_message("Blue Curve", "A revision will be made to the current Master Copy then it will be reverted to this document are you sure you wish to proceed", bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
            Dim fn As String
            fn = get_filename_from_revision_tag(tag)

            If omsg.cancel_selected = True Then
                Exit Sub
            End If

            clear_down_local_doc()

            localDocument.revision_filename = fn
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                localDocument.db_revert()

            Else
                localDocument.tmode = bc_cs_soap_base_class.tREAD
                localDocument.read_mode = bc_om_document.REVERT

                If localDocument.transmit_to_server_and_receive(localDocument, True) = False Then
                    Exit Sub
                End If
            End If
            If localDocument.doc_not_found = True Then
                omsg = New bc_cs_message("Blue Curve", "Revision: " + fn + " not found on server", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)

                Exit Sub
            ElseIf check_document_state(localDocument.id, True) = False Then
                Me.RetrieveDocs(True, True)
                Exit Sub
            End If
            Me.RetrieveDocs(True, True)

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_process", "revertdocument", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_process", "revertdocument", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub



    Private Sub loadDoc(ByVal ldoc As bc_om_document)
        Dim slog As New bc_cs_activity_log("bc_am_process", "load_doc", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            If location = 1 Then
                Me.documentDetailsView.uxDocumentDetailsTab.Visible = False
                ldoc.id = 0
                ldoc.filename = Replace(ldoc.filename, ldoc.extension, "")
                ldoc = ldoc.read_data_from_file(bc_cs_central_settings.local_repos_path + ldoc.filename + ".dat")
                localDocument = ldoc
                formStateChanging = True
                mainFormRibbonState()
                loadDocumentDetails()
                ldoc.id = 0
                currDoc = ldoc
                Exit Sub
            End If
            Me.documentDetailsView.uxDocumentDetailsTab.Visible = True
            REM assign selected document
            Dim tid As Long
            tid = ldoc.id
            'setServerStatus("Loading Document...")
            clear_down_local_doc()
            ldoc = New bc_om_document
            ldoc.id = tid

            If location = 0 Then

                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    ldoc.db_read_for_process()

                ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                    ldoc.tmode = bc_cs_soap_base_class.tREAD
                    ldoc.read_mode = bc_om_document.READ_FOR_PROCESS

                    ldoc.transmit_to_server_and_receive(ldoc, True)
                End If
            End If

            If ldoc.id = -1 Then
                Dim omessage As New bc_cs_message("Blue Curve - process", "Another user has just changed the stage of this document you may no longer be able to view", bc_cs_message.MESSAGE)
                Me.RetrieveDocs()

            Else
                ldoc.id = tid
                localDocument = ldoc
                formStateChanging = True
                mainFormRibbonState()
                loadDocumentDetails()
            End If
            ldoc.id = tid
            currDoc = ldoc
            Me.containerView.uxcomponents.Enabled = False
            REM beed  a check component mode for pub type at a later stage
            If ldoc.tables_on_Demand = True Or Right(ldoc.extension, 4) = "pptx" Then
                Me.containerView.uxcomponents.Enabled = True
            End If
            Me.containerView.rdistribute.Enabled = False
            REM see if it is distributable
            For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                If bc_am_load_objects.obc_pub_types.pubtype(i).id = ldoc.pub_type_id AndAlso bc_am_load_objects.obc_pub_types.pubtype(i).distributable = True Then
                    Me.containerView.rdistribute.Enabled = True
                    Exit For
                End If
            Next

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_process", "load_doc", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

            processing = False
            formStateChanging = False
            slog = New bc_cs_activity_log("bc_am_process", "load_doc", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub

    Public Sub mainFormRibbonState()
        Dim slog As New bc_cs_activity_log(className, "load_doc", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            With containerView
                .uxImportSupportDocument.Enabled = False
                .uxdisclosurefile.Enabled = False

                .uxImportRegisteredDocument.Enabled = False
                .uxReattachMasterDocument.Enabled = False
                .uxattestationgroup.Enabled = True
                .uxstopdocgroup.Enabled = True


                ' establish if force check in can be shown
                If localDocument.force_check_in = True Then
                    If localDocument.checked_out_user <> 0 Then
                        REM And localDocument.checked_out_user <> bc_cs_central_settings.logged_on_user_id Then
                        .uxForceCheckIn.Enabled = True
                    Else
                        .uxForceCheckIn.Enabled = False
                    End If
                Else
                    .uxForceCheckIn.Enabled = False
                End If
                '.uxViewRibbonPageGroup.Text = "Open"
                .BarButtonItem3.Caption = "View"

                If localDocument.checked_out_user = bc_cs_central_settings.logged_on_user_id Then
                    .BarButtonItem3.Caption = "Edit"
                End If

                localDocument.title = Trim(localDocument.title)
                .uxViewDocument.Enabled = False
                .uxDocumentState.Enabled = False

                .uxcomponents.Enabled = False
                .uxhtmlpreview.Enabled = False
                REM access rights
                If localDocument.read = True Then
                    .uxViewDocument.Enabled = True

                    .uxcomponents.Enabled = True
                    If localDocument.html_preview_exists = True Then
                        .uxhtmlpreview.Enabled = True
                        .uxhtmlpreview.Visibility = BarItemVisibility.Always
                    Else
                        .uxhtmlpreview.Visibility = BarItemVisibility.Never
                    End If
                End If

                .uxCategoriseDocument.Enabled = True

                If localDocument.write = True Then
                    Dim omime As New bc_am_mime_types
                    If localDocument.checked_out_user = 0 Then
                        'Document Currently Checked in
                        .uxDocumentState.Enabled = True
                        .uxDocumentState.Caption = "Check Out"
                        '.uxDocumentState.Checked = True
                        .uxDocumentState.ImageIndex = -1 ' needs to be the correct value

                        'LS DEV EXPRESS COMMENTED
                        'Me.EditButtonIcon.Visible = True
                        'Me.EditButtonIcon.Image = Me.mimeIcons.Images(CInt(omime.get_image_index_for_mime_type(ldoc.extension, True)))

                        REM enable import support document
                        .uxImportSupportDocument.Enabled = True
                        If localDocument.allow_disclosures = True Then
                            .uxdisclosurefile.Enabled = True
                        End If


                        .uxReattachMasterDocument.Enabled = False
                        If localDocument.extension = "" Then
                            .uxImportRegisteredDocument.Enabled = True
                        Else
                            If localDocument.write = True And localDocument.checked_out_user = "0" Then
                                If LCase(localDocument.extension) <> ".html" Then
                                    .uxReattachMasterDocument.Enabled = True
                                End If
                            End If
                        End If

                        .uxCategoriseDocument.Enabled = True
                    Else
                        If localDocument.checked_out_user = bc_cs_central_settings.logged_on_user_id Then
                            'Checked out to current user
                            .uxDocumentState.Enabled = True
                            '.uxDocumentState.Checked = True
                            .uxDocumentState.Caption = "Check In"
                            .uxDocumentState.ImageIndex = -1 ' needs to be the correct value
                            '.uxCategoriseDocument.Enabled = False
                        Else
                            'Checked out to other user
                            .uxDocumentState.Enabled = False
                            '.uxCategoriseDocument.Enabled = False
                        End If
                    End If
                End If


                REM if document has only been registered disable bits
                If localDocument.extension = "" Then
                    .uxImportRegisteredDocument.Enabled = True
                    .uxViewDocument.Enabled = False
                    .uxDocumentState.Enabled = False

                    .uxcomponents.Enabled = False
                    .uxhtmlpreview.Enabled = False
                End If
                If location = 1 Then
                    .uxDocumentState.Enabled = True
                    '.uxDocumentState.Checked = True
                    .uxDocumentState.Caption = "Check In"
                    .uxDocumentState.ImageIndex = -1 ' needs to be the correct value
                    .uxViewDocument.Enabled = True
                    .uxViewDocument.Caption = "Edit"

                End If
            End With
            With documentDetailsView
                .uxViewRevision.Enabled = False
                .uxRevertRevision.Enabled = False
                .uxViewSupportDocument.Enabled = False
                .uxcatsupportdoc.Enabled = False
                .uxDeleteSupportDoc.Enabled = False
                .uxCheckInOutSupportDoc.Enabled = False

            End With



        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_workflow_summary", "load_doc", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            documentDetailsView.uxDocTitleValue.Text = localDocument.title
            Cursor.Current = Cursors.Default
            slog = New bc_cs_activity_log("bc_am_workflow_summary", "load_doc", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try


    End Sub
    Friend Sub load_html_Preview()
        Try
            Dim ohtml As New bc_om_html_preview
            ohtml.doc_Id = localDocument.id
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                ohtml.db_read()
            Else
                ohtml.tmode = bc_cs_soap_base_class.tREAD
                If ohtml.transmit_to_server_and_receive(ohtml, True) = False Then
                    Exit Sub
                End If
            End If
            If ohtml.success = True Then
                Dim fn As String = bc_cs_central_settings.local_repos_path + "\" + CStr(ohtml.doc_Id) + ".html"
                Dim fs As New bc_cs_file_transfer_services
                If fs.write_bytestream_to_document(fn, ohtml.html, Nothing, True) = False Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Could not open preview for document.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                End If
                System.Diagnostics.Process.Start(fn)
            Else
                Dim omsg As New bc_cs_message("Blue Curve", "Could not locate html preview for document on server.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_workflow_summary", "load_html_Preview", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Sub
    Private Sub show_attestation()
        Try
            Dim fa As New bc_am_attestation
            Dim ca As New Cbc_am_attestation()
            If ca.load_data(selectedDocId, bc_cs_central_settings.logged_on_user_id, localDocument.title, fa) = True Then
                fa.ShowDialog()
                Me.RetrieveDocs()
                Exit Sub
            Else

                If ca.err_text <> "" Then
                    'Dim omsg As New bc_cs_message("Blue Curve", "Failed to load attestation: " + ca.err_text, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                End If
            End If


            REM log answers
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_process", "show_attestation", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try

    End Sub
    Private Sub view_attestations_for_doc()
        Try
            Dim fa As New bc_am_dx_view_attestations
            Dim ca As New Cbc_am_dx_view_attestations()
            If ca.load_data(fa, selectedDocId, localDocument.title) Then
                fa.ShowDialog()
                Exit Sub
            Else
                Dim omsg As New bc_cs_message("Blue Curve", "No Attestations have been submitted for document.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            End If
            REM log answers
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_process", "view_attestations_for_doc", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try

    End Sub
    Private Sub stop_doc(mode As bc_om_stop_documemt.DOC_STOP_MODE)
        Try
            Dim osd As New bc_om_stop_documemt
            osd.mode = mode
            osd.doc_id = localDocument.id



            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                osd.db_read()
            Else
                osd.tmode = bc_cs_soap_base_class.tREAD

                If osd.transmit_to_server_and_receive(osd, True) = False Then
                    Exit Sub
                End If
            End If
            RetrieveDocs()
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_process", "stop_doc", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Friend Sub generate_disclosure_file()
        Dim otrace As New bc_cs_activity_log("bc_am_process", "generate_disclosure_file", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            Dim tid As Long

            tid = localDocument.id

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                localDocument.db_read_for_categorize()

            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                localDocument.tmode = bc_cs_soap_base_class.tREAD
                localDocument.read_mode = bc_om_document.READ_FOR_CATEGORIZE
                localDocument.transmit_to_server_and_receive(localDocument, True)
            End If
            If check_document_state(localDocument.id, True) = False Then
                Me.RetrieveDocs(True, True)
                Exit Sub
            End If

            localDocument.id = tid

            Dim bd As New bc_am_build_automated_support_document
            bd.template_name = localDocument.disclosure_file
            If bd.template_name = "" Then
                Dim omsg As New bc_cs_message("Blue Curve", "Dislosure File not configured for this publication.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Sub
            End If

            bd.master_doc = localDocument

            If bd.create_doc_from_process_toolbar(localDocument) = False Then
                Dim omsg As New bc_cs_message("Blue Curve", "Failed to Generate Dislosure File: " + bd.err_text, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            Else
                Dim omsg As New bc_cs_message("Blue Curve", "Disclosure File Generated Please View in Support Documents.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Me.RetrieveDocs(True, True)
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_process", "generate_disclosure_file", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_am_process", "generate_disclosure_file", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Friend Sub mainFormRibbonAction(ByVal action As String)

        Dim otrace As New bc_cs_activity_log("bc_am_process", "mainFormRibbonAction", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try

            If Not formStateChanging Then

                Select Case action
                    Case DISCLOSURE_FILE
                        generate_disclosure_file()
                    Case ATTRIBUTES
                        Dim fass As New bc_am_edit_attributes
                        Dim cass As New Cbc_am_edit_attributes()
                        If cass.load_data(fass, localDocument) = True Then
                            fass.ShowDialog()
                            Me.RetrieveDocs()
                        Else
                            Dim omsg As New bc_cs_message("Blue Curve", "No Entities categorized or Attributes Configured", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                        End If

                    Case CANCELDOC
                        stop_doc(bc_om_stop_documemt.DOC_STOP_MODE.CANCEL)
                    Case REJECTDOC
                        stop_doc(bc_om_stop_documemt.DOC_STOP_MODE.REJECT)
                    Case ATTESTATION
                        show_attestation()
                    Case VIEWATTESTATION
                        view_attestations_for_doc()
                    Case REGULAR
                        Dim freg As New bc_am_dx_regular_reports
                        Dim creg As New Cbc_am_dx_regular_reports
                        Dim badmin As Boolean = False

                        If bc_am_load_objects.obc_pub_types.process_switches.regular_report_admin_only = False Then

                            badmin = True
                        Else
                            If Me.containerView.uxRole.Caption = "Role: Administrator" Then
                                badmin = True
                            End If
                        End If

                        If creg.load_data(freg, badmin) = True Then
                            freg.ShowDialog()
                            If Not IsNothing(creg.selected_doc) Then
                                importRegularReport(creg.selected_doc)
                            End If
                        End If

                    Case DISTRIBUTION
                        Dim fdist As New bc_am_distribution
                        Dim cdist As New Cbc_am_distribution
                        If cdist.load_data(fdist, selectedDocId, localDocument.title) = True Then
                            fdist.ShowDialog()
                        End If
                    Case HTML_PREVIEW
                        load_html_Preview()
                    Case COMPONENTS
                        show_components()
                    Case SETTINGS
                        invoke_settings()
                    Case DATE_FROM_CHECK_UNCHECK
                        If fDateFrom = "9-9-9999" Then
                            containerView.uxFilterDateFrom.EditValue = Now.Subtract(New TimeSpan(1, 0, 0, 0, 0))
                            fDateFrom = containerView.uxFilterDateFrom.EditValue
                            containerView.uxFilterDateFrom.Enabled = True
                        Else
                            If containerView.uxIncludePublished.EditValue = False Then
                                fDateFrom = "9-9-9999"
                                containerView.uxFilterDateFrom.Enabled = False
                                containerView.uxFilterDateFrom.EditValue = Nothing
                            Else
                                Dim omessage As New bc_cs_message("Blue Curve - Process", "A date from must be specified in order to view published documents", bc_cs_message.MESSAGE)
                                containerView.uxFilterDateFromCheck.EditValue = True
                            End If
                        End If

                    Case DATE_TO_CHECK_UNCHECK
                        If containerView.uxFilterDateTo.Enabled = False Then
                            containerView.uxFilterDateTo.EditValue = Now
                            fDateTo = containerView.uxFilterDateTo.EditValue
                            containerView.uxFilterDateToCheck.EditValue = True
                            containerView.uxFilterDateTo.Enabled = True
                        Else
                            fDateTo = "9-9-9999"
                            containerView.uxFilterDateToCheck.EditValue = False
                            containerView.uxFilterDateTo.Enabled = False
                            containerView.uxFilterDateTo.EditValue = Nothing
                        End If

                    Case DATE_FROM_VALUE_CHANGE
                        If Not containerView.uxFilterDateFrom.EditValue Is Nothing Then
                            fDateFrom = containerView.uxFilterDateFrom.EditValue
                        End If
                        Me.RetrieveDocs()


                    Case DATE_TO_VALUE_CHANGE
                        If Not containerView.uxFilterDateTo.EditValue Is Nothing Then
                            fDateTo = containerView.uxFilterDateTo.EditValue
                        End If
                        Me.RetrieveDocs()

                    Case REFRESH
                        Me.RetrieveDocs()


                    Case DOCUMENT_DETAILS_SHOW_HIDE
                        If containerView.uxShowDetails.Checked Then
                            If containerView.uxSplitContainer.IsPanelCollapsed Then
                                containerView.uxSplitContainer.Collapsed = False
                            End If
                            containerView.uxShowDetails.Caption = "Hide"
                        Else
                            If Not containerView.uxSplitContainer.IsPanelCollapsed Then
                                containerView.uxSplitContainer.Collapsed = True
                            End If
                            containerView.uxShowDetails.Caption = "Show"
                        End If

                    Case DOCUMENT_FILTERS_SHOW_HIDE
                        If containerView.uxShowFilters.Checked Then
                            containerView.uxFilterWindow.Visible = True
                            containerView.uxShowFilters.Caption = "Hide"
                        Else
                            containerView.uxFilterWindow.Visible = False
                            containerView.uxShowFilters.Caption = "Show"
                        End If
                    Case RESET_FILTERS
                        Me.clear_filters()

                    Case INCLUDE_PUBLISHED

                        If containerView.uxFilterDateFromCheck.EditValue = True Then
                            Me.documentListView.uxDocumentListView.Columns(11).Visible = False
                            show_publish = Not show_publish
                            Me.RetrieveDocs()
                        Else

                            Dim omessage As New bc_cs_message("Blue Curve - Process", "A date from must be specified in order to view published documents", bc_cs_message.MESSAGE)
                            containerView.uxIncludePublished.EditValue = False
                        End If
                    Case PUBLISH_ONLY
                        If containerView.uxFilterDateFromCheck.EditValue = True Then
                            bpublish_only = Not bpublish_only
                            If Me.bpublish_only = True Then
                                containerView.uxIncludePublished.Enabled = False
                                containerView.uxStageFilter.Visible = False
                                containerView.Uxlocation.Visible = False

                                sStage.Clear()

                            Else

                                containerView.uxIncludePublished.Enabled = True
                                containerView.uxStageFilter.Visible = True
                                containerView.Uxlocation.Visible = True
                            End If
                            Me.RetrieveDocs()
                        Else

                            Dim omessage As New bc_cs_message("Blue Curve - Process", "A date from must be specified in order to view published documents", bc_cs_message.MESSAGE)
                            containerView.uxpublishonly.EditValue = False
                        End If

                    Case CREATE
                        launchCreate()

                    Case VIEW
                        viewDoc()

                    Case DOCUMENT_STATE_CHANGE
                        documentStateChange(containerView.uxDocumentState.Checked)
                        If containerView.uxDocumentState.Checked Then
                            containerView.uxDocumentState.ImageIndex = 0
                        Else
                            containerView.uxDocumentState.ImageIndex = 0
                        End If

                    Case FORCE_CHECK_IN
                        forceCheckIn(localDocument)

                    Case CATEGORISE
                        editMetadata(localDocument)

                    Case IMPORT_MASTER_DOCUMENT
                        importMasterDocument(localDocument, False)

                    Case IMPORT_SUPPORT_DOCUMENT
                        importSupportDocument(localDocument)

                    Case IMPORT_REGISTERED_DOCUMENT
                        importRegisteredDocument(localDocument)

                    Case REGISTER_MASTER_DOCUMENT
                        importMasterDocument(localDocument, True)

                    Case REATTACH_MASTER_DOCUMENT
                        attachNewDocument(localDocument)

                    Case APPLICATION_EXIT
                        containerView.Close()

                End Select
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_process", "mainFormRibbonAction", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_am_process", "mainFormRibbonAction", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub

    Friend Sub mainFormFilterAction(ByVal action As String, ByVal itmLnk As NavBarItem)

        Dim otrace As New bc_cs_activity_log("bc_am_process", "mainFormFilterAction", bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim id As String
        Dim i As Integer

        Try
            Select Case action
                Case bc_am_pr_main.PUBLICATION_TYPE
                    If itmLnk.Tag = 0 Then
                        sPubtypeID.Clear()
                    Else
                        If itmLnk.Appearance.Font.Bold = True Then
                            sPubtypeID.Add(filter_items(itmLnk.Tag - 1).item)
                        Else
                            id = filter_items(itmLnk.Tag - 1).item
                            For i = 0 To sPubtypeID.Count - 1
                                If sPubtypeID(i) = id Then
                                    sPubtypeID.RemoveAt(i)
                                    Exit For
                                End If
                            Next
                        End If

                    End If
                Case bc_am_pr_main.ENTITY
                    If itmLnk.Tag = 0 Then
                        sEntityID.Clear()
                    Else
                        If itmLnk.Appearance.Font.Bold = True Then
                            sEntityID.Add(filter_items(itmLnk.Tag - 1).item)
                        Else
                            id = filter_items(itmLnk.Tag - 1).item
                            For i = 0 To sEntityID.Count - 1
                                If sEntityID(i) = id Then
                                    sEntityID.RemoveAt(i)
                                    Exit For
                                End If
                            Next
                        End If
                    End If
                Case bc_am_pr_main.STAGE
                    If itmLnk.Tag = 0 Then
                        sStage.Clear()
                    Else
                        If itmLnk.Appearance.Font.Bold = True Then
                            sStage.Add(filter_items(itmLnk.Tag - 1).item)
                        Else
                            id = filter_items(itmLnk.Tag - 1).item
                            For i = 0 To sStage.Count - 1
                                If sStage(i) = id Then
                                    sStage.RemoveAt(i)
                                    Exit For
                                End If
                            Next
                        End If
                    End If
                Case bc_am_pr_main.AUTHOR
                    If itmLnk.Tag = 0 Then
                        sAuthorID.Clear()
                    Else
                        If itmLnk.Appearance.Font.Bold = True Then
                            sAuthorID.Add(filter_items(itmLnk.Tag - 1).item)
                        Else

                            id = filter_items(itmLnk.Tag - 1).item
                            For i = 0 To sAuthorID.Count - 1
                                If CStr(sAuthorID(i)) = CStr(id) Then
                                    sAuthorID.RemoveAt(i)
                                    Exit For
                                End If
                            Next
                        End If
                    End If
                Case bc_am_pr_main.DIST
                    If itmLnk.Tag = 0 Then
                        sDistID.Clear()
                    Else
                        If itmLnk.Appearance.Font.Bold = True Then
                            sDistID.Add(filter_items(itmLnk.Tag - 1).item)
                        Else

                            id = filter_items(itmLnk.Tag - 1).item
                            For i = 0 To sDistID.Count - 1
                                If CStr(sDistID(i)) = CStr(id) Then
                                    sDistID.RemoveAt(i)
                                    Exit For
                                End If
                            Next
                        End If
                    End If
            End Select

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_process", "mainFormFilterAction", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_am_process", "mainFormFilterAction", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub

    Private Sub mainFormState()

        Dim otrace As New bc_cs_activity_log("bc_am_process", "mainFormState", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            formStateChanging = True

            'If bc_cs_central_settings.generic_filter = True Then
            '    'LS COMMENTED OUT DEV EXPRESS
            '    'Me.lauthor.Columns(0).Text = bc_cs_central_settings.generic_filter_title
            'End If

            'LS COMMENTED OUT DEV EXPRESS
            'Me.loading = True
            Dim i As Integer
            For i = 0 To bc_am_load_objects.obc_users.user.Count - 1
                With bc_am_load_objects.obc_users.user(i)
                    If bc_cs_central_settings.logged_on_user_id = .id Then
                        containerView.uxUser.Caption = "User: " + .first_name + " " + .surname
                        containerView.uxRole.Caption = "Role: " + .role
                        Exit For
                    End If
                End With
            Next

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                containerView.uxConnection.Caption = "Connected via Ado: " + bc_cs_central_settings.servername
            Else
                containerView.uxConnection.Caption = "Connected via WebServices: " + bc_cs_central_settings.soap_server
            End If

            documentListView.TopLevel = False
            documentListView.Parent = containerView.uxDocumentListPanel
            documentListView.Dock = DockStyle.Fill

            documentDetailsView.TopLevel = False
            documentDetailsView.Parent = containerView.uxDocumentDetailsPanel
            documentDetailsView.Dock = DockStyle.Fill

            documentListView.Show()
            documentDetailsView.Show()

            REM dates            
            If fDateFrom <> "9-9-9999" Then
                containerView.uxFilterDateFrom.Enabled = True
                containerView.uxFilterDateFrom.EditValue = fDateFrom
                containerView.uxFilterDateFromCheck.EditValue = True
            Else
                containerView.uxFilterDateFrom.Enabled = True
                containerView.uxFilterDateFromCheck.EditValue = True
                Dim t As New TimeSpan(28, 0, 0, 0, 0)
                fDateFrom = Now.Subtract(t)
                containerView.uxFilterDateFrom.EditValue = Now.Subtract(t)
            End If


            If fDateTo <> "9-9-9999" Then
                containerView.uxFilterDateTo.Enabled = True
                containerView.uxFilterDateTo.EditValue = fDateTo
                containerView.uxFilterDateToCheck.EditValue = True
            Else
                containerView.uxFilterDateTo.Enabled = False
                containerView.uxFilterDateTo.EditValue = Nothing
                containerView.uxFilterDateToCheck.EditValue = False
            End If
            For i = 0 To documentListView.uxDocumentListView.Columns.Count - 1
                documentListView.uxDocumentListView.Columns(i).VisibleIndex = i
            Next
            If containerView.columnorder.Count > 0 Then
                For i = 0 To documentListView.uxDocumentListView.Columns.Count - 1
                    documentListView.uxDocumentListView.Columns(i).VisibleIndex = containerView.columnorder(i)
                    documentListView.uxDocumentListView.Columns(i).Width = containerView.columnwidth(i)
                Next
            End If

            With documentListView
                .uxDocumentListView.Columns(0).FieldName = "Row_Icon"
                .uxDocumentListView.Columns(0).Width = 20
                .uxDocumentListView.Columns(1).FieldName = "Title"
                .uxDocumentListView.Columns(2).FieldName = "Doc_Date"
                .uxDocumentListView.Columns(3).FieldName = "Pub_Type"
                .uxDocumentListView.Columns(4).FieldName = "Stage"
                .uxDocumentListView.Columns(5).FieldName = "Author"
                .uxDocumentListView.Columns(6).FieldName = "Checked_Out_User"
                .uxDocumentListView.Columns(7).FieldName = "Entity_Name"
                .uxDocumentListView.Columns(8).FieldName = "Completed_By"
                .uxDocumentListView.Columns(9).FieldName = "Bus_Area"
                .uxDocumentListView.Columns(10).FieldName = "Language"
                .uxDocumentListView.Columns(11).FieldName = "Distribution_status"
                .uxDocumentListView.Columns(12).FieldName = "attestation_num"
                .uxDocumentListView.Columns(13).FieldName = "attestation_pass_icon"
                .uxDocumentListView.Columns(13).Width = 20
                .uxDocumentListView.Columns(14).FieldName = "fast_track_icon"
                .uxDocumentListView.Columns(15).FieldName = "attribute_change"
                .uxDocumentListView.Columns(16).FieldName = "assign_to"
            End With

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_process", "mainFormState", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            formStateChanging = False
            otrace = New bc_cs_activity_log("bc_am_process", "mainFormState", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Friend Sub processExit()
        Try

            writeUserSettingsToFile()

            While polling_process = True
                Thread.Sleep(1000)
            End While
            tPoll.Abort()
            GC.Collect()
        Catch ex As Exception
            Dim err As New bc_cs_error_log("bc_am_process", "workflow_exit", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub

    Friend Function loadData(Optional ByVal local_docs As Boolean = False) As Boolean


        Dim slog As New bc_cs_activity_log("bc_am_process", "load_data", bc_cs_activity_codes.TRACE_ENTRY, "")
        'Dim efound As Boolean
        Try

            'Dim exclude As Boolean
            Dim selected_row_handle As Integer = -1
            Dim i, k As Integer

            docList.Clear()


            k = 0
            For i = 4 To Me.documentListView.uxDLImageList.Images.Count - 1
                Me.documentListView.uxDLImageList.Images.RemoveAt(4)
            Next

            If local_docs = False Then
                load_list(docs)
            Else
                load_list(ldocs)
            End If
            'For i = 0 To docs.document.Count - 1
            '    REM collate filter lists
            '    efound = False
            '    If (Mode = 0 Or (Mode = 1 AndAlso docs.document(i).unread = True) Or (Mode = 3 AndAlso docs.document(i).stage_expire_date.tolocaltime < Now) Or _
            '        (Mode = 2 AndAlso (Now.AddDays(preExpireAlertNotify) > docs.document(i).stage_expire_date.tolocaltime) AndAlso _
            '         Now < docs.document(i).stage_expire_date.tolocaltime) Or (Mode = 4 AndAlso docs.document(i).stage_change_flag = True)) Then
            '        exclude = False
            '        If sPubtypeID.Count > 0 Then
            '            If inPubTypeFilter(docs.document(i).pub_type_name) = False Then
            '                exclude = True
            '                Continue For
            '            End If
            '        End If
            '        If sEntityID.Count > 0 Then
            '            If inEntityFilter(docs.document(i).lead_entity_name) = False Then
            '                exclude = True
            '                Continue For
            '            End If
            '        End If
            '        If sStage.Count > 0 Then
            '            If inStageFilter(docs.document(i).stage_name) = False Then
            '                exclude = True
            '                Continue For
            '            End If
            '        End If
            '        If sAuthorID.Count > 0 Then
            '            If inAuthorFilter(docs.document(i).originator_name) = False Then
            '                exclude = True
            '                Continue For
            '            End If
            '        End If



            '        Dim docListFields As New documentListFields
            '        If selectedDocId = "0" Then
            '            selectedDocId = CStr(docs.document(i).id)
            '            selected_row_handle = 0
            '        ElseIf CStr(docs.document(i).id) = selectedDocId Then
            '            selected_row_handle = i
            '        End If

            '        If docs.document(i).id = "0" Then
            '            docListFields.ID = docs.document(i).filename
            '        Else
            '            docListFields.ID = docs.document(i).id
            '        End If
            '        'allows direct acces from the grid data source
            '        docListFields.DocObjectIndex = i


            '        docListFields.Title = docs.document(i).title

            '        If docs.document(i).checked_out_user <> "0" Then
            '            If docs.document(i).checked_out_user = bc_cs_central_settings.logged_on_user_id Then
            '                docListFields.Row_Icon = 1
            '            Else
            '                docListFields.Row_Icon = 2
            '            End If
            '        Else
            '            REM display mime type icon
            '            Dim image As Bitmap
            '            If docs.document(i).extension = "" Then
            '                docListFields.Row_Icon = 3
            '            Else
            '                Dim bcs As New bc_cs_icon_services
            '                image = bcs.get_icon_for_file_type(Replace(docs.document(i).extension, "[imp]", ""))
            '                If IsNothing(image) Then
            '                    docListFields.Row_Icon = 0
            '                Else
            '                    Me.documentListView.uxDLImageList.Images.Add(image)
            '                    docListFields.Row_Icon = Me.documentListView.uxDLImageList.Images.Count - 1
            '                    Dim v As Int16
            '                    Dim im As New DevExpress.XtraEditors.Controls.ImageComboBoxItem
            '                    v = Me.documentListView.uxDLImageList.Images.Count - 1
            '                    im.ImageIndex = Me.documentListView.uxDLImageList.Images.Count - 1
            '                    im.Description = ""
            '                    im.Value = v
            '                    Me.documentListView.uxGridImageCombo.Items.Add(im)

            '                End If
            '            End If
            '        End If


            '        docListFields.Doc_Date = Format(docs.document(i).doc_date.tolocaltime, "dd-MMM-yyyy HH:mm")
            '        docListFields.Pub_Type = docs.document(i).pub_type_name

            '        docListFields.Stage = docs.document(i).stage_name
            '        REM originating author and checked out user
            '        Dim fauthor As Boolean
            '        fauthor = False
            '        Dim fcheckedout As Boolean
            '        fcheckedout = False

            '        docListFields.Author = docs.document(i).originator_name

            '        docListFields.Checked_Out_User = docs.document(i).checked_out_user_name

            '        docListFields.Entity_Name = docs.document(i).lead_entity_name

            '        If docs.document(i).stage_expire_date <> CDate("09/09/9999") Then
            '            docListFields.Completed_By = Format(docs.document(i).stage_expire_date.tolocaltime, "dd-MMM-yyyy HH:mm")
            '        Else
            '            docListFields.Completed_By = "n/a"
            '        End If
            '        If Not IsNumeric(docs.document(i).bus_area) Then
            '            docListFields.Bus_Area = docs.document(i).bus_area
            '        End If

            '        docListFields.Language = docs.document(i).language_name

            '        docListFields.Unread = docs.document(i).unread
            '        docListFields.Urgent_Flag = docs.document(i).urgent_flag
            '        docListFields.Expire_Flag = docs.document(i).expire_flag
            '        docListFields.Stage_Change_Flag = docs.document(i).stage_change_flag

            '        docList.Add(docListFields)

            '    End If
            'Next


            'bindDocumentList(docList)



            REM load filters
            loadFilters()

            Dim rowselected As Boolean = False
            For i = 0 To docList.Count - 1

                If selectedDocId = docList(documentListView.uxDocumentListView.GetDataSourceRowIndex(i)).ID Then
                    documentListView.uxDocumentListView.FocusedRowHandle = i
                    rowselected = True
                    Exit For
                End If
            Next
            Me.containerView.uxShowDetails.Enabled = False
            If docList.Count > 0 Then
                Me.containerView.uxShowDetails.Enabled = True
                selectDocument()
            Else
                If Not containerView.uxSplitContainer.IsPanelCollapsed Then
                    containerView.uxSplitContainer.Collapsed = True
                End If
                containerView.uxShowDetails.Caption = "Show"
                containerView.uxCategoriseDocument.Enabled = False
                containerView.uxImportRegisteredDocument.Enabled = False
                containerView.uxImportSupportDocument.Enabled = False
                containerView.uxdisclosurefile.Enabled = False
                containerView.uxForceCheckIn.Enabled = False
                containerView.uxReattachMasterDocument.Enabled = False
                containerView.uxViewDocument.Enabled = False

                containerView.uxcomponents.Enabled = False
                containerView.uxhtmlpreview.Enabled = False


                containerView.uxDocumentState.Enabled = False
                containerView.uxstopdocgroup.Enabled = False
                containerView.uxattestationgroup.Enabled = False


            End If

        Catch ex As Exception
            Dim err As New bc_cs_error_log("bc_am_process", "load_data", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            Me.last_refresh_date = Now
            last_mouse_move = last_refresh_date
            set_status_text()

            slog = New bc_cs_activity_log("bc_am_process", "load_data", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

    Private Sub load_list(ByVal docs As bc_om_documents)
        Dim slog As New bc_cs_activity_log("bc_am_process", "load_data", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim efound As Boolean
        Dim exclude As Boolean
        Dim selected_row_handle As Integer = -1
        Try
            For i = 0 To docs.document.Count - 1
                REM collate filter lists
                efound = False
                If (Mode = 0 Or (Mode = 1 AndAlso docs.document(i).unread = True) Or (Mode = 3 AndAlso docs.document(i).stage_expire_date.tolocaltime < Now) Or _
                    (Mode = 2 AndAlso (Now.AddDays(preExpireAlertNotify) > docs.document(i).stage_expire_date.tolocaltime) AndAlso _
                     Now < docs.document(i).stage_expire_date.tolocaltime) Or (Mode = 4 AndAlso docs.document(i).stage_change_flag = True)) Then
                    exclude = False
                    If sPubtypeID.Count > 0 Then
                        If inPubTypeFilter(docs.document(i).pub_type_name) = False Then
                            exclude = True
                            Continue For
                        End If
                    End If
                    If sEntityID.Count > 0 Then
                        If inEntityFilter(docs.document(i).lead_entity_name) = False Then
                            exclude = True
                            Continue For
                        End If
                    End If
                    If sStage.Count > 0 Then
                        If inStageFilter(docs.document(i).stage_name) = False Then
                            exclude = True
                            Continue For
                        End If
                    End If
                    If sAuthorID.Count > 0 Then
                        If inAuthorFilter(docs.document(i).originator_name) = False Then
                            exclude = True
                            Continue For
                        End If
                    End If

                    If sDistID.Count > 0 Then
                        If inDistFilter(docs.document(i).distribution_status) = False Then
                            exclude = True
                            Continue For
                        End If
                    End If

                    Dim docListFields As New documentListFields
                    If selectedDocId = "0" Then
                        selectedDocId = CStr(docs.document(i).id)
                        selected_row_handle = 0
                    ElseIf CStr(docs.document(i).id) = selectedDocId Then
                        selected_row_handle = i
                    End If

                    If docs.document(i).id = "0" Then
                        docListFields.ID = docs.document(i).filename
                    Else
                        docListFields.ID = docs.document(i).id
                    End If
                    'allows direct acces from the grid data source
                    docListFields.DocObjectIndex = i


                    docListFields.Title = docs.document(i).title

                    If docs.document(i).checked_out_user <> "0" Then
                        If docs.document(i).checked_out_user = bc_cs_central_settings.logged_on_user_id Then
                            docListFields.Row_Icon = 1
                        Else
                            docListFields.Row_Icon = 2
                        End If
                    Else
                        REM display mime type icon
                        Dim image As Bitmap
                        If docs.document(i).extension = "" Then
                            docListFields.Row_Icon = 3
                        Else
                            Dim bcs As New bc_cs_icon_services
                            image = bcs.get_icon_for_file_type(Replace(docs.document(i).extension, "[imp]", ""))
                            If IsNothing(image) Then
                                docListFields.Row_Icon = 0
                            Else
                                Me.documentListView.uxDLImageList.Images.Add(image)
                                docListFields.Row_Icon = Me.documentListView.uxDLImageList.Images.Count - 1
                                Dim v As Int16
                                Dim im As New DevExpress.XtraEditors.Controls.ImageComboBoxItem
                                v = Me.documentListView.uxDLImageList.Images.Count - 1
                                im.ImageIndex = Me.documentListView.uxDLImageList.Images.Count - 1
                                im.Description = ""
                                im.Value = v
                                Me.documentListView.uxGridImageCombo.Items.Add(im)

                            End If
                        End If
                    End If


                    'docListFields.Doc_Date = Format(docs.document(i).doc_date.tolocaltime, "dd-MMM-yyyy HH:mm")
                    docListFields.Doc_Date = docs.document(i).doc_date.tolocaltime
                    docListFields.Pub_Type = docs.document(i).pub_type_name

                    docListFields.Stage = docs.document(i).stage_name
                    REM originating author and checked out user
                    Dim fauthor As Boolean
                    fauthor = False
                    Dim fcheckedout As Boolean
                    fcheckedout = False

                    docListFields.Author = docs.document(i).originator_name

                    docListFields.Checked_Out_User = docs.document(i).checked_out_user_name

                    docListFields.Entity_Name = docs.document(i).lead_entity_name

                    If docs.document(i).stage_expire_date <> CDate("09/09/9999") Then
                        'docListFields.Completed_By = Format(docs.document(i).stage_expire_date.tolocaltime, "dd-MMM-yyyy HH:mm")
                        docListFields.Completed_By = docs.document(i).stage_expire_date.tolocaltime
                    Else
                        docListFields.Completed_By = docs.document(i).stage_expire_date.tolocaltime
                    End If
                    If Not IsNumeric(docs.document(i).bus_area) Then
                        docListFields.Bus_Area = docs.document(i).bus_area
                    End If

                    docListFields.Language = docs.document(i).language_name

                    docListFields.Unread = docs.document(i).unread
                    docListFields.Urgent_Flag = docs.document(i).urgent_flag
                    docListFields.Expire_Flag = docs.document(i).expire_flag
                    docListFields.Stage_Change_Flag = docs.document(i).stage_change_flag
                    docListFields.Distribution_status = docs.document(i).distribution_status
                    docListFields.attestation_num = CStr(docs.document(i).num_attestations)
                    docListFields.attestation_pass_icon = -1
                    If docs.document(i).num_attestations > 0 Then
                        If docs.document(i).attestation_pass = False Then
                            docListFields.attestation_pass_icon = -1
                        Else
                            docListFields.attestation_pass_icon = 1
                        End If
                    End If
                    docListFields.fast_track_icon = -1
                    If docs.document(i).fast_track = 2 Then
                        docListFields.fast_track_icon = 0
                    End If
                    docListFields.attribute_change = docs.document(i).attribute_change
                    docListFields.assign_to = docs.document(i).assign_to_name

                    docList.Add(docListFields)
                End If
            Next

            bindDocumentList(docList)
        Catch ex As Exception
            Dim err As New bc_cs_error_log("bc_am_process", "load_list", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try
    End Sub

    Private Sub bindDocumentList(ByVal docList As BindingList(Of documentListFields))
        documentListView.uxDocumentList.DataSource = Nothing
        documentListView.uxDocumentList.DataSource = docList
        documentListView.uxDocumentListView.BestFitColumns()





    End Sub

    Private Sub updateRibbonStatusBar()

    End Sub

    Private Function inPubTypeFilter(ByVal pub_type_id As String) As Boolean
        Dim i As Integer
        inPubTypeFilter = False
        For i = 0 To sPubtypeID.Count - 1
            If sPubtypeID(i) = pub_type_id Then
                inPubTypeFilter = True
                Exit Function
            End If
        Next
    End Function

    Private Function inEntityFilter(ByVal entity_id As String) As Boolean
        Dim i As Integer
        inEntityFilter = False
        For i = 0 To sEntityID.Count - 1
            If sEntityID(i) = entity_id Then
                inEntityFilter = True
                Exit Function
            End If
        Next
    End Function

    Private Function inStageFilter(ByVal stage As String) As Boolean
        Dim i As Integer
        inStageFilter = False
        For i = 0 To sStage.Count - 1
            If sStage(i) = stage Then
                inStageFilter = True
                Exit Function
            End If
        Next
    End Function

    Private Function inAuthorFilter(ByVal author_id As String) As Boolean
        Dim i As Integer
        inAuthorFilter = False
        For i = 0 To sAuthorID.Count - 1
            If sAuthorID(i) = author_id Then
                inAuthorFilter = True
                Exit Function
            End If
        Next
    End Function
    Private Function inDistFilter(ByVal dist_status_id As String) As Boolean
        Dim i As Integer
        inDistFilter = False
        For i = 0 To sDistID.Count - 1
            If sDistID(i) = dist_status_id Then
                inDistFilter = True
                Exit Function
            End If
        Next
    End Function

    Public Sub loadFilters()

        Dim boldAllp, boldAlls, boldAlll, boldAllo As Boolean

        boldAllp = True
        boldAlls = True
        boldAlll = True
        boldAllo = True

        REM now populate listboxes
        Dim i As Integer
        Dim nbi As NavBarItem

        containerView.Uxlocation.ItemLinks.Clear()
        nbi = New NavBarItem("Submitted")
        nbi.Tag = 0
        containerView.Uxlocation.ItemLinks.Add(nbi)
        AddHandler nbi.LinkClicked, AddressOf containerView.filterAction
        nbi = New NavBarItem("Local")
        nbi.Tag = 1
        containerView.Uxlocation.ItemLinks.Add(nbi)
        AddHandler nbi.LinkClicked, AddressOf containerView.filterAction

        containerView.Uxlocation.ItemLinks(Me.location).Item.Appearance.Font = _
           New Font(containerView.Uxlocation.ItemLinks(Me.location).Item.Appearance.Font, FontStyle.Bold)

        containerView.uxDocumentStatusFilter.ItemLinks.Clear()
        nbi = New NavBarItem("All")
        nbi.Tag = 0
        containerView.uxDocumentStatusFilter.ItemLinks.Add(nbi)
        AddHandler nbi.LinkClicked, AddressOf containerView.filterAction
        nbi = New NavBarItem("New")
        nbi.Tag = 1
        containerView.uxDocumentStatusFilter.ItemLinks.Add(nbi)
        AddHandler nbi.LinkClicked, AddressOf containerView.filterAction
        nbi = New NavBarItem("Urgent")
        nbi.Tag = 2
        containerView.uxDocumentStatusFilter.ItemLinks.Add(nbi)
        AddHandler nbi.LinkClicked, AddressOf containerView.filterAction
        nbi = New NavBarItem("Expired")
        nbi.Tag = 3
        containerView.uxDocumentStatusFilter.ItemLinks.Add(nbi)
        AddHandler nbi.LinkClicked, AddressOf containerView.filterAction
        nbi = New NavBarItem("Stage Changed")
        nbi.Tag = 4
        containerView.uxDocumentStatusFilter.ItemLinks.Add(nbi)
        AddHandler nbi.LinkClicked, AddressOf containerView.filterAction

        containerView.uxDocumentStatusFilter.ItemLinks(Mode).Item.Appearance.Font = _
            New Font(containerView.uxDocumentStatusFilter.ItemLinks(Mode).Item.Appearance.Font, FontStyle.Bold)



        REM pub types
        containerView.uxPubTypeFilter.ItemLinks.Clear()
        nbi = New NavBarItem("All")
        nbi.Tag = 0
        containerView.uxPubTypeFilter.ItemLinks.Add(nbi)
        AddHandler nbi.LinkClicked, AddressOf containerView.filterAction
        REM entities
        containerView.uxEntityFilter.ItemLinks.Clear()
        nbi = New NavBarItem("All")
        nbi.Tag = 0
        containerView.uxEntityFilter.ItemLinks.Add(nbi)
        AddHandler nbi.LinkClicked, AddressOf containerView.filterAction

        REM stages
        containerView.uxStageFilter.ItemLinks.Clear()
        nbi = New NavBarItem("All")
        nbi.Tag = 0
        containerView.uxStageFilter.ItemLinks.Add(nbi)
        AddHandler nbi.LinkClicked, AddressOf containerView.filterAction

        REM author
        containerView.uxAuthorFilter.ItemLinks.Clear()
        nbi = New NavBarItem("All")
        nbi.Tag = 0
        containerView.uxAuthorFilter.ItemLinks.Add(nbi)
        AddHandler nbi.LinkClicked, AddressOf containerView.filterAction

        REM distribution statis
        containerView.uxdiststatus.ItemLinks.Clear()
        nbi = New NavBarItem("All")
        nbi.Tag = 0
        containerView.uxdiststatus.ItemLinks.Add(nbi)
        AddHandler nbi.LinkClicked, AddressOf containerView.filterAction


        For i = 0 To filter_items.Count - 1
            If filter_items(i).type_name = "Publication Type" Then
                nbi = New NavBarItem(filter_items(i).item)
                nbi.Tag = i + 1
                containerView.uxPubTypeFilter.ItemLinks.Add(nbi)
                If inPubTypeFilter(filter_items(i).item) = True Then
                    containerView.uxPubTypeFilter.ItemLinks(containerView.uxPubTypeFilter.ItemLinks.Count - 1).Item.Appearance.Font = New Font(nbi.Appearance.Font, FontStyle.Bold)
                    boldAllp = False
                End If
                AddHandler nbi.LinkClicked, AddressOf containerView.filterAction
            End If
            If filter_items(i).type_name = "Stage" Then
                nbi = New NavBarItem(filter_items(i).item)
                nbi.Tag = i + 1
                containerView.uxStageFilter.ItemLinks.Add(nbi)
                If inStageFilter(filter_items(i).item) = True Then
                    containerView.uxStageFilter.ItemLinks(containerView.uxStageFilter.ItemLinks.Count - 1).Item.Appearance.Font = New Font(nbi.Appearance.Font, FontStyle.Bold)
                    boldAlls = False
                End If
                AddHandler nbi.LinkClicked, AddressOf containerView.filterAction
            End If
            If filter_items(i).type_name = "Lead Classification" Then
                nbi = New NavBarItem(filter_items(i).item)
                nbi.Tag = i + 1
                containerView.uxEntityFilter.ItemLinks.Add(nbi)
                If inEntityFilter(filter_items(i).item) = True Then
                    containerView.uxEntityFilter.ItemLinks(containerView.uxEntityFilter.ItemLinks.Count - 1).Item.Appearance.Font = New Font(nbi.Appearance.Font, FontStyle.Bold)
                    boldAlll = False
                End If
                AddHandler nbi.LinkClicked, AddressOf containerView.filterAction
            End If
            If filter_items(i).type_name = "Originator" Then
                nbi = New NavBarItem(filter_items(i).item)
                nbi.Tag = i + 1
                containerView.uxAuthorFilter.ItemLinks.Add(nbi)
                If inAuthorFilter(filter_items(i).item) = True Then
                    containerView.uxAuthorFilter.ItemLinks(containerView.uxAuthorFilter.ItemLinks.Count - 1).Item.Appearance.Font = New Font(nbi.Appearance.Font, FontStyle.Bold)
                    boldAllo = False
                End If
                AddHandler nbi.LinkClicked, AddressOf containerView.filterAction
            End If

            If filter_items(i).type_name = "Distribution Status" Then
                nbi = New NavBarItem(filter_items(i).item)
                nbi.Tag = i + 1
                containerView.uxdiststatus.ItemLinks.Add(nbi)
                If inDistFilter(filter_items(i).item) = True Then
                    containerView.uxdiststatus.ItemLinks(containerView.uxdiststatus.ItemLinks.Count - 1).Item.Appearance.Font = New Font(nbi.Appearance.Font, FontStyle.Bold)
                    boldAllo = False
                End If
                AddHandler nbi.LinkClicked, AddressOf containerView.filterAction
            End If
        Next

        If boldAllp Then
            containerView.uxPubTypeFilter.ItemLinks(0).Item.Appearance.Font = _
                New Font(containerView.uxPubTypeFilter.ItemLinks(0).Item.Appearance.Font, FontStyle.Bold)
        End If



        If boldAlll Then
            containerView.uxEntityFilter.ItemLinks(0).Item.Appearance.Font = _
                New Font(containerView.uxEntityFilter.ItemLinks(0).Item.Appearance.Font, FontStyle.Bold)
        End If


        If boldAlls Then
            containerView.uxStageFilter.ItemLinks(0).Item.Appearance.Font = _
                New Font(containerView.uxStageFilter.ItemLinks(0).Item.Appearance.Font, FontStyle.Bold)
        End If



        If boldAllo Then
            containerView.uxAuthorFilter.ItemLinks(0).Item.Appearance.Font = _
                New Font(containerView.uxAuthorFilter.ItemLinks(0).Item.Appearance.Font, FontStyle.Bold)
        End If

        If boldAllo Then
            containerView.uxdiststatus.ItemLinks(0).Item.Appearance.Font = _
                New Font(containerView.uxdiststatus.ItemLinks(0).Item.Appearance.Font, FontStyle.Bold)
        End If


    End Sub

    Public Sub display_tax()
        Dim f As New bc_dx_display_taxonomy
        Dim c As New Cbc_dx_display_taxonomy(f, localDocument)
        If c.load_data = True Then
            f.ShowDialog()
        End If

    End Sub
    Public Sub invokelink(ByVal item As Integer)
        Try
            System.Diagnostics.Process.Start(Me.localDocument.links(item).url)
        Catch
            Dim omsg As New bc_cs_message("Blue Curve", "Failed to invoke URL: " + Me.localDocument.links(item).url, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
        End Try

    End Sub




    Private Sub launchCreate()
        ' invoke Create on a new thread
        runCreate()
        Exit Sub

        If createRunning = True Then
            Exit Sub
        End If

        Dim atthread As New Thread(AddressOf runCreate)
        atthread.SetApartmentState(System.Threading.ApartmentState.STA)
        atthread.Start()
    End Sub

    Private Sub runCreate()
        Try






            containerView.uxCreate.Enabled = False


            createRunning = True

            'added to handle Office 2010 startup changes
            MessageFilter.Register()

            'Dim fwizard_main As New bc_am_at_wizard_main
            'fwizard_main.from_create = True
            'fwizard_main.from_workflow = True
            'fwizard_main.ShowDialog()
            'fwizard_main = Nothing
            Dim fxwizard As New bc_dx_am_create_wizard_frm
            fxwizard.build_mode = True
            fxwizard.bc_am_create_wizard_frm_load()

            'buildMode = mode
            Dim oproductselection As New bc_am_product_selection

            If fxwizard.sel_pub_type_Id = 0 Then
                fxwizard.Hide()
                Exit Sub
            End If
            Dim wstate As Integer
            wstate = containerView.WindowState
            containerView.WindowState = FormWindowState.Minimized
            If fxwizard.is_clone = True Then
                Dim bic As New bc_am_clone_document
                fxwizard.WindowState = Windows.Forms.FormWindowState.Minimized
                If bic.invoke_clone(fxwizard.sel_pub_type_Id, fxwizard.sel_entity_Id1, fxwizard.sel_entity_id2) = False Then
                    bic = Nothing
                    fxwizard.Hide()
                    containerView.WindowState = wstate
                End If

            ElseIf fxwizard.is_composite = True Then
                Dim vcb As New bc_dx_adv_composite_build
                Dim ccb As New Cbc_dx_adv_composite_build(vcb)
                If ccb.load_data(fxwizard.sel_pub_type_Id, fxwizard.sel_entity_Id1) = True Then
                    fxwizard.Hide()
                    vcb.ShowDialog()

                Else
                    fxwizard.Hide()
                    containerView.WindowState = wstate
                End If
            Else

                If oproductselection.launch_product(fxwizard.sel_pub_type_Id, fxwizard.sel_entity_Id1, Nothing, fxwizard.sel_entity_id2) = bc_cs_error_codes.RETURN_ERROR Then
                    fxwizard.Hide()
                Else
                    fxwizard.WindowState = wstate
                End If
            End If

            'fxwizard.ShowDialog()



        Catch

        Finally

            'added to handle Office 2010 startup changes
            MessageFilter.Revoke()
            createRunning = False

            containerView.uxCreate.Enabled = True
        End Try


    End Sub

    Private Sub show_components()
        Try
            Dim ffc = New bc_am_dx_document_composition
            Dim cfs As New ctrll_bc_am_document_composition(localDocument.id, ffc)
            Dim read_only As Boolean = False
            If localDocument.stage = 8 Then
                read_only = True
            End If
            If cfs.load_data(localDocument.title, False, False, read_only) = True Then
                ffc.ShowDialog()
            Else
                Dim omsg As New bc_cs_message("Blue Curve", "No Components in Document", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_process", "show_components", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub


    Private Sub viewDoc()

        If LCase(localDocument.extension = ".html") Then
            If localDocument.checked_out_user = bc_cs_central_settings.logged_on_user_id Or location = 1 Then


                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    localDocument.db_check_out_html_product()
                ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                    localDocument.tmode = bc_cs_soap_base_class.tREAD
                    localDocument.read_mode = bc_om_document.CHECK_OUT_HTML_PRODUCT
                    localDocument.transmit_to_server_and_receive(localDocument, True)
                End If

                Dim hfn As String

                Dim oguid As New bc_om_get_guid_for_html_product
                oguid.entity_id = localDocument.entity_id
                oguid.pub_type_id = localDocument.pub_type_id
                oguid.doc_id = localDocument.id
                oguid.user_id = bc_cs_central_settings.logged_on_user_id
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    oguid.db_read()
                Else
                    oguid.tmode = bc_cs_soap_base_class.tREAD
                    If oguid.transmit_to_server_and_receive(oguid, True) = False Then
                        Exit Sub
                    End If
                End If

                hfn = bc_cs_central_settings.web_process_url + localDocument.html_template + "?guid=" + oguid.sguid
                Try
                    System.Diagnostics.Process.Start(hfn)
                Catch
                    Dim omsg As New bc_cs_message("Blue Curve", "Failed to invoke URL: " + hfn, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                End Try



            Else
                Dim hfn As String
                hfn = bc_cs_central_settings.email_preview_url + CStr(localDocument.id) + ".html"
                Try
                    System.Diagnostics.Process.Start(hfn)
                Catch
                    Dim omsg As New bc_cs_message("Blue Curve", "Failed to invoke URL: " + hfn, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                End Try
            End If
            Exit Sub
        End If
        If localDocument.checked_out_user = bc_cs_central_settings.logged_on_user_id Or location = 1 Then
            localDocument.history.Clear()
            localDocument.support_documents.Clear()
            If Len(localDocument.extension) > 5 Or (localDocument.extension <> ".doc" And localDocument.extension <> ".docx" And _
               localDocument.extension <> ".ppt" And localDocument.extension <> ".pptx") Then
                Dim omime As New bc_am_mime_types
                omime.view(bc_cs_central_settings.local_repos_path + localDocument.filename, localDocument.extension)
            Else

                REM Blue curve open from local
                Dim odoc As New bc_am_at_open_doc
                odoc.open(True, localDocument, True)
            End If
        Else
            viewSupportDocument(localDocument, True)
        End If
        If bc_am_load_objects.obc_pub_types.process_switches.minimize_on_view = True Then

            Me.containerView.WindowState = FormWindowState.Minimized
            'Else
            '    Me.containerView.WindowState = FormWindowState.Normal
        End If

    End Sub

    Private Sub viewSupportDocument(ByVal ldoc As bc_om_document, Optional ByVal view_only As Boolean = True)


        Dim slog As New bc_cs_activity_log("bc_am_process", "viewSupportDocument", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim sdoc As bc_om_document
            Try
                setServerStatus("Requesting...")
            Catch

            End Try
            sdoc = New bc_om_document
            sdoc = ldoc
            REM get support doc
            Dim i As Integer
            For i = 0 To ldoc.support_documents.Count - 1
                If ldoc.support_documents(i).id = ldoc.id Then
                    sdoc = New bc_om_document
                    sdoc = ldoc.support_documents(i)
                    Exit For
                End If

            Next
            Dim tdoc_id As Long
            tdoc_id = sdoc.id

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                sdoc.db_read_physical_doc_view_only()

            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                sdoc.certificate.name = bc_cs_central_settings.logged_on_user_name
                sdoc.certificate.user_id = bc_cs_central_settings.logged_on_user_id
                sdoc.tmode = bc_cs_soap_base_class.tREAD
                sdoc.read_mode = bc_om_document.GET_PHYSICAL_DOC_VIEW_ONLY
                If sdoc.transmit_to_server_and_receive(sdoc, False) = False Then
                    Dim omessage As New bc_cs_message("Blue Curve", "Document not found on server contact system administrator!", bc_cs_message.MESSAGE, False, False, "Ok", "Cancel", True)
                    Exit Sub
                End If
            End If
            If sdoc.doc_not_found Then
                Dim omessage As New bc_cs_message("Blue Curve", "Document not found on server contact system administrator!", bc_cs_message.MESSAGE, False, False, "Ok", "Cancel", True)
                Exit Sub
            End If

            sdoc.id = tdoc_id

            open_read_only(sdoc)

            If bc_am_load_objects.obc_pub_types.process_switches.minimize_on_view = True Then

                Me.containerView.WindowState = FormWindowState.Minimized
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_process", "viewSupportDocument", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            processing = False

            slog = New bc_cs_activity_log("bc_am_process", "viewSupportDocument", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Private Function saveEditFile(ByVal ldoc As bc_om_document) As String
        Dim slog As New bc_cs_activity_log("bc_am_process", "save_edit_file", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim fs As New bc_cs_file_transfer_services
            Dim fn As String
            Dim i As Integer = 0

            'fn = CStr(ldoc.id) + ldoc.extension
            fn = ldoc.filename
            fs.write_bytestream_to_document(bc_cs_central_settings.local_repos_path + fn, ldoc.byteDoc, Nothing)
            saveEditFile = fn
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_process", "save_edit_file", bc_cs_error_codes.USER_DEFINED, ex.Message)
            saveEditFile = ""
        Finally
            slog = New bc_cs_activity_log("bc_am_process", "save_edit_file", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

    Private Function saveViewOnlyFile(ByVal ldoc As bc_om_document) As String
        Dim slog As New bc_cs_activity_log("bc_am_process", "save_view_only_file", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim fs As New bc_cs_file_transfer_services
            Dim fn As String
            Dim i As Integer = 0
            Dim found As Boolean
            saveViewOnlyFile = ""
            fn = "view" + CStr(i) + ldoc.extension
            found = True
            While found = True
                i = i + 1
                fn = "view" + CStr(i) + ldoc.extension
                REM tidy up other files
                fs.delete_file(bc_cs_central_settings.local_repos_path + fn, Nothing, False)
                If fs.check_document_exists(bc_cs_central_settings.local_repos_path + fn) = False Then
                    fs.write_bytestream_to_document(bc_cs_central_settings.local_repos_path + fn, ldoc.byteDoc, Nothing)
                    found = False
                End If
            End While
            saveViewOnlyFile = fn
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_process", "save_view_only_file", bc_cs_error_codes.USER_DEFINED, ex.Message)
            saveViewOnlyFile = ""
        Finally
            slog = New bc_cs_activity_log("bc_am_process", "save_view_only_file", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

    Friend Sub selectDocument()
        Dim moduleName As String = "selectDocument"
        Dim slog As New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim selectedDocIndex As Integer

            Cursor.Current = Cursors.WaitCursor

            If documentListView.uxDocumentListView.SelectedRowsCount = 0 Then
                Exit Try
            End If

            selectedDocId = docList(documentListView.uxDocumentListView.GetDataSourceRowIndex(documentListView.uxDocumentListView.GetSelectedRows(0))).ID
            'selectedDocIndex = docList(documentListView.uxDocumentListView.GetDataSourceRowIndex(documentListView.uxDocumentListView.GetSelectedRows(0))).DocObjectIndex

            For i = 0 To docList.Count - 1
                If selectedDocId = docList(i).ID Then
                    selectedDocIndex = i

                End If
            Next

            If docs.document(selectedDocIndex).urgent_flag = True Then
                docList(selectedDocIndex).Urgent_Flag = True
            End If
            If docs.document(selectedDocIndex).expire_flag = True Then
                docList(selectedDocIndex).Expire_Flag = True
            End If
            If docs.document(selectedDocIndex).stage_change_flag = True Then
                docList(selectedDocIndex).Stage_Change_Flag = True
            End If

            If docs.document(selectedDocIndex).unread = True Then
                docs.document(selectedDocIndex).unread = False
                docList(selectedDocIndex).Unread = False
                docs.document(selectedDocIndex).acknowledged = Now
            End If
            If docs.document(selectedDocIndex).stage_change_flag = True Then
                docs.document(selectedDocIndex).stage_change_flag = False
                docList(selectedDocIndex).Stage_Change_Flag = False
                docs.document(selectedDocIndex).acknowledged = Now
            End If

            Dim tdoc As New bc_om_document
            If location = 0 Then
                tdoc.id = selectedDocId
            Else
                tdoc.filename = selectedDocId
                tdoc.stage_name = ldocs.document(selectedDocIndex).stage_name
                tdoc.doc_date = ldocs.document(selectedDocIndex).doc_date
                tdoc.pub_type_name = ldocs.document(selectedDocIndex).pub_type_name
                tdoc.extension = ldocs.document(selectedDocIndex).extension
                tdoc.behalf_of_author_id = ldocs.document(selectedDocIndex).behalf_of_author_id
            End If
            tdoc.stage = docs.document(selectedDocIndex).stage
            tdoc.checked_out_user = docs.document(selectedDocIndex).checked_out_user
            tdoc.master_flag = True
            tdoc.arrive = docs.document(selectedDocIndex).arrive
            tdoc.acknowledged = docs.document(selectedDocIndex).acknowledged
            loadDoc(tdoc)


        Catch ex As Exception
            Dim db_err As New bc_cs_error_log(className, moduleName, bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            Cursor.Current = Cursors.Default

            slog = New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub


    Private Sub documentStateChange(ByVal checkOut As Boolean)
        Dim moduleName As String = "documentStateChange"
        Dim slog As New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Cursor.Current = Cursors.WaitCursor
            If localDocument.checked_out_user <> 0 Or location = 1 Then
                checkInMasterDoc(True, localDocument)
                setServerStatus("")
            Else
                REM SW cope with office versions
                If Len(localDocument.extension) > 5 Or (localDocument.extension <> ".doc" And localDocument.extension <> ".docx" And _
                    localDocument.extension <> ".ppt" And localDocument.extension <> ".pptx") Then
                    checkoutMasterDoc(localDocument)
                Else
                    REM author tool document
                    openDocument(localDocument)
                End If
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log(className, moduleName, bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            Cursor.Current = Cursors.Default
            slog = New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub

    Private Sub openDocument(ByVal ldoc As bc_om_document)
        Dim moduleName As String = "openDocument"
        Dim slog As New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_ENTRY, "")


        Try
            Dim odoc As New bc_am_at_open_doc



            clear_down_local_doc()
            odoc.open(False, ldoc, True)


            Me.RetrieveDocs()

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log(className, moduleName, bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

            slog = New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    Public Sub clear_filters()
        If Mode <> 0 Or sPubtypeID.Count > 0 Or sEntityID.Count > 0 Or sAuthorID.Count > 0 Or sStage.Count > 0 Or sDistID.Count > 0 Then
            Mode = 0
            sPubtypeID.Clear()
            sEntityID.Clear()
            sAuthorID.Clear()
            sStage.Clear()
            sDistID.Clear()
            Me.RetrieveDocs(True, True)
        End If
    End Sub
    Private Sub checkoutMasterDoc(ByVal ldoc As bc_om_document)
        Dim moduleName As String = "checkoutMasterDoc"
        Dim omime As bc_am_mime_types
        Dim fn As String

        Dim slog As New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            If ldoc.extension.Length >= 5 AndAlso LCase(Left(ldoc.extension, 5)) = ".html" Then



                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    ldoc.db_check_out_html_product()
                ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                    ldoc.tmode = bc_cs_soap_base_class.tREAD
                    ldoc.read_mode = bc_om_document.CHECK_OUT_HTML_PRODUCT
                    ldoc.transmit_to_server_and_receive(ldoc, True)
                End If
                If check_document_state(ldoc.id, True) = False Then
                    Exit Sub
                End If
                Dim hfn As String

                Dim oguid As New bc_om_get_guid_for_html_product
                oguid.entity_id = ldoc.entity_id
                oguid.pub_type_id = ldoc.pub_type_id
                oguid.doc_id = ldoc.id
                oguid.user_id = bc_cs_central_settings.logged_on_user_id
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    oguid.db_read()
                Else
                    oguid.tmode = bc_cs_soap_base_class.tREAD
                    If oguid.transmit_to_server_and_receive(oguid, True) = False Then
                        Exit Sub
                    End If
                End If

                hfn = bc_cs_central_settings.web_process_url + ldoc.html_template + "?guid=" + oguid.sguid
                Try
                    System.Diagnostics.Process.Start(hfn)
                Catch
                    Dim omsg As New bc_cs_message("Blue Curve", "Failed to invoke URL: " + hfn, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                End Try


                Exit Sub
            End If



            setServerStatus("Requesting...")
            Dim omessage As bc_cs_message

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                ldoc.db_check_out_non_create_doc()
            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                ldoc.tmode = bc_cs_soap_base_class.tREAD
                ldoc.read_mode = bc_om_document.CHECK_OUT_NON_CREATE_DOC
                ldoc.transmit_to_server_and_receive(ldoc, True)
            End If

            If check_document_state(ldoc.id, True) = False Then
                Exit Sub
            End If


            If ldoc.doc_not_found Then
                omessage = New bc_cs_message("Blue Curve", "Document not found on server contact system administrator!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Sub
            End If


            REM can only quick open a word document
            REM save as view only file
            setServerStatus("Saving Document for Edit Locally")
            fn = saveEditFile(ldoc)
            omime = New bc_am_mime_types
            omime.view(bc_cs_central_settings.local_repos_path + fn, ldoc.extension)
            If bc_am_load_objects.obc_pub_types.process_switches.minimize_on_view = True Then

                Me.containerView.WindowState = FormWindowState.Minimized
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log(className, moduleName, bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.RetrieveDocs()


            slog = New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    Public Sub editsupportdoc(ByVal tag As String)
        Dim moduleName As String = "checkoutSupportDoc"

        Dim slog As New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_ENTRY, "")

        Try

            For i = 0 To localDocument.support_documents.Count - 1
                If localDocument.support_documents(i).id = tag Then
                    Dim omime As New bc_am_mime_types
                    omime.view(bc_cs_central_settings.local_repos_path + localDocument.support_documents(i).filename, localDocument.support_documents(i).extension)
                    Exit For
                End If
            Next
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log(className, moduleName, bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Public Sub checkoutSupportDoc(ByVal tag As String)
        Dim moduleName As String = "checkoutSupportDoc"
        Dim omime As bc_am_mime_types
        Dim fn As String

        Dim slog As New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_ENTRY, "")

        Try

            REM firstly check master document status hasnt changed
            If localDocument.checked_out_user <> bc_cs_central_settings.logged_on_user_id Then
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    localDocument.db_check_document_state()
                ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                    localDocument.tmode = bc_cs_soap_base_class.tREAD
                    localDocument.read_mode = bc_om_document.CHECK_DOCUMENT_STATE
                    If localDocument.transmit_to_server_and_receive(localDocument, True) = False Then
                        Exit Sub

                    End If
                End If
                If check_document_state(localDocument.id, True, True) = False Then
                    Me.RetrieveDocs()
                    Exit Sub
                End If
            End If


            For i = 0 To localDocument.support_documents.Count - 1
                If localDocument.support_documents(i).id = tag Then
                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        localDocument.support_documents(i).db_check_out_support_doc()
                    ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                        localDocument.support_documents(i).tmode = bc_cs_soap_base_class.tREAD
                        localDocument.support_documents(i).read_mode = bc_om_document.CHECK_OUT_SUPPORT_DOC
                        If localDocument.support_documents(i).transmit_to_server_and_receive(localDocument.support_documents(i), True) = False Then
                            Exit Sub
                        End If
                    End If

                    If check_document_state(localDocument.support_documents(i).id, True) = False Then

                        Exit Sub
                    End If
                    If localDocument.support_documents(i).doc_not_found Then
                        Dim omessage As New bc_cs_message("Blue Curve", "Document not found on server contact system administrator!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                        Exit Sub
                    End If
                    REM can only quick open a word document
                    REM save as view only file
                    setServerStatus("Saving Document for Edit Locally")
                    fn = saveEditFile(localDocument.support_documents(i))
                    omime = New bc_am_mime_types
                    omime.view(bc_cs_central_settings.local_repos_path + fn, localDocument.support_documents(i).extension)
                    If bc_am_load_objects.obc_pub_types.process_switches.minimize_on_view = True Then

                        Me.containerView.WindowState = FormWindowState.Minimized
                    End If


                    Exit For
                End If
            Next






        Catch ex As Exception
            Dim db_err As New bc_cs_error_log(className, moduleName, bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.RetrieveDocs(True, True)


            slog = New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub


    Private Function check_document_state(ByVal id As Long, ByVal checkedin As Boolean, Optional ByVal master_text As Boolean = False) As Boolean
        check_document_state = False

        If id = DOCUMENT_RECENTLY_STAGE_CHANGED Then
            If master_text = False Then
                Dim omessage As New bc_cs_message("Blue Curve - process", "Another user has just changed the stage of this document.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            Else
                Dim omessage As New bc_cs_message("Blue Curve - process", "Another user has just changed the stage of the master document.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            End If

        ElseIf id = DOCUMENT_RECENTLY_CHECKED_OUT And checkedin = True Then
            If master_text = False Then
                Dim omessage As New bc_cs_message("Blue Curve - process", "Another user has just checked out  this document.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            Else
                Dim omessage As New bc_cs_message("Blue Curve - process", "Another user has just checked out  the master document.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            End If
        ElseIf id = TAKE_REVISION_FAILED Then
            Dim omessage As New bc_cs_message("Blue Curve - process", "Failed to Take Document Revision, Revert aborted", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
        ElseIf id = FAILED_TO_REVERT_DOCUMENT Then
            Dim omessage As New bc_cs_message("Blue Curve - process", "Failed to Revert Document", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
        ElseIf id = DOCUMENT_RECENTLY_CHECKED_IN And checkedin = False Then
            Dim omessage As New bc_cs_message("Blue Curve - process", "Another user has just checked in this document", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
        Else
            check_document_state = True
        End If

    End Function
    Public Function checkInSupportDoc(ByVal tag As String) As Boolean
        Dim slog As New bc_cs_activity_log("bc_am_workflow", "checkInSupportDoc", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            Dim i As Integer
            Dim omessage As bc_cs_message
            Dim fn As String


            checkInSupportDoc = False
            Dim fs As New bc_cs_file_transfer_services

            REM match id to correct support doc
            For i = 0 To localDocument.support_documents.Count - 1
                If localDocument.support_documents(i).id = tag Then

                    fn = bc_cs_central_settings.local_repos_path + localDocument.support_documents(i).filename

                    If fs.check_document_exists(fn, Nothing) = False Then
                        omessage = New bc_cs_message("Blue Curve - process", "Document: " + fn + " could not be found.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                        Exit Function
                    End If
                    If fs.in_use_by_another_process(fn, Nothing) = True Then
                        omessage = New bc_cs_message("Blue Curve - process", "Document: " + fn + " is open please save and close prior to check in.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                        Exit Function
                    End If
                    fs.write_document_to_bytestream(fn, localDocument.support_documents(i).byteDoc, Nothing, True)



                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        localDocument.support_documents(i).db_write(Nothing)
                    Else
                        localDocument.support_documents(i).tmode = bc_cs_soap_base_class.tWRITE
                        If localDocument.support_documents(i).transmit_to_server_and_receive(localDocument.support_documents(i), True) = False Then
                            Exit Function
                        End If
                    End If
                    If localDocument.support_documents(i).doc_write_success = False Then
                        Dim omsg As New bc_cs_message("Blue Curve", "Failed to check in document: " + localDocument.support_documents(i).doc_write_error_text, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                        Exit Function
                    End If
                    If fs.delete_file(fn, Nothing, False) = "in use" Then
                        omessage = New bc_cs_message("Blue Curve - process", "Document is Open please save and close prior to check in.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                        Exit Function
                    End If
                    checkInSupportDoc = True
                    Exit For
                End If
            Next





        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_workflow", "checkInSupportDoc", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.RetrieveDocs(True, True)


            slog = New bc_cs_activity_log("bc_am_workflow", "checkInSupportDoc", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function
    Private Function checkInMasterDoc(ByVal master_doc As Boolean, ByVal ldoc As bc_om_document) As Boolean
        Dim slog As New bc_cs_activity_log("bc_am_workflow", "check_in", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            Dim i As Integer
            Dim omessage As bc_cs_message

            If master_doc = True And ldoc.extension.Length >= 5 AndAlso LCase(Left(ldoc.extension, 5)) = ".html" Then
                ldoc.mode = bc_om_document.F
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    ldoc.db_write(Nothing)
                Else
                    ldoc.tmode = bc_cs_soap_base_class.tWRITE
                    ldoc.transmit_to_server_and_receive(ldoc, True)
                End If
                If ldoc.doc_write_success = False Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Failed to  Check In Document: " + ldoc.doc_write_error_text, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                End If
                checkInMasterDoc = True
                Exit Function
            End If
            checkInMasterDoc = False
            Dim fs As New bc_cs_file_transfer_services
            If master_doc = False Then
                REM match id to correct support doc
                For i = 0 To ldoc.support_documents.Count - 1
                    If ldoc.support_documents(i).id = ldoc.id Then
                        ldoc = ldoc.support_documents(i)
                        Exit For
                    End If
                Next

            Else
                ldoc.support_documents.Clear()
                ldoc = ldoc
            End If
            Dim fn As String
            If location = 1 Then
                ldoc.filename = Replace(ldoc.filename, ldoc.extension, "")
                fn = bc_cs_central_settings.local_repos_path + ldoc.filename + ldoc.extension
            Else
                fn = bc_cs_central_settings.local_repos_path + ldoc.filename
            End If

            If fs.check_document_exists(fn, Nothing) = False Then
                omessage = New bc_cs_message("Blue Curve - process", "Document: " + fn + " could not be found locally please locate or force check in.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Function
            End If

            If fs.in_use_by_another_process(fn, Nothing) = True Then
                omessage = New bc_cs_message("Blue Curve - process", "Document: " + fn + " is open please save and close prior to check in.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Function
            End If


            REM if file has metadata in local read this in
            REM 5.6 FIX
            If fs.check_document_exists_and_can_be_read(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ".dat") Then
                ldoc = ldoc.read_data_from_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ".dat")
            End If

            REM FIL JIRA 17287 JAN 2016 file read needs to happen after metddata read else get last checkedout document
            fs.write_document_to_bytestream(fn, ldoc.byteDoc, Nothing, True)

            ldoc.history.Clear()
            ldoc.bwith_document = True
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                ldoc.db_write(Nothing)
            Else
                ldoc.tmode = bc_cs_soap_base_class.tWRITE
                If ldoc.transmit_to_server_and_receive(ldoc, True) = False Then
                    Exit Function
                End If
            End If
            If ldoc.doc_write_success = False Then
                Dim omsg As New bc_cs_message("Blue Curve", "Failed to check in document: " + ldoc.doc_write_error_text, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Function
            End If
            If fs.delete_file(fn, Nothing, False) = "in use" Then
                omessage = New bc_cs_message("Blue Curve - process", "Document is Open please save and close prior to check in.", bc_cs_message.MESSAGE)
                Exit Function
            End If
            REM delete local metadata file if exists
            If location = 0 Then
                fs.delete_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ".dat", Nothing, False)
            Else
                fs.delete_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.filename) + ".dat", Nothing, False)
            End If
            checkInMasterDoc = True

            REM remove record from local documents
            REM delete local record if exists
            Dim local_docs As New bc_om_documents
            REM read in local documents list
            If fs.check_document_exists(bc_cs_central_settings.local_repos_path + "bc_local_documents.dat") Then
                local_docs = local_docs.read_data_from_file(bc_cs_central_settings.local_repos_path + "bc_local_documents.dat")
                REM delete this document
                For i = 0 To local_docs.document.Count - 1
                    If (local_docs.document(i).id = ldoc.id And ldoc.id <> 0) Or (local_docs.document(i).id = 0 And (local_docs.document(i).filename = ldoc.filename Or local_docs.document(i).filename = ldoc.filename + ldoc.extension)) Then
                        local_docs.document.RemoveAt(i)
                        Exit For
                    End If
                Next
                REM save back down
                local_docs.write_data_to_file(bc_cs_central_settings.local_repos_path + "bc_local_documents.dat")
            End If


        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_workflow", "check_in", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

            Me.RetrieveDocs()

            slog = New bc_cs_activity_log("bc_am_workflow", "check_in", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

    Private Sub forceCheckIn(ByVal ldoc As bc_om_document)
        Dim moduleName As String = "forceCheckIn"
        Dim slog As New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim omsg As bc_cs_message
            omsg = New bc_cs_message("Blue Curve", "Are you sure you wish to force check in: " + ldoc.title + ". This operation will remove checked out users edits", bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
            If omsg.cancel_selected = True Then
                Exit Sub
            End If
            ldoc.mode = bc_om_document.F
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                ldoc.db_write(Nothing)
            Else
                ldoc.tmode = bc_cs_soap_base_class.tWRITE
                ldoc.transmit_to_server_and_receive(ldoc, True)
            End If
            If ldoc.doc_write_success = False Then
                omsg = New bc_cs_message("Blue Curve", "Failed to Force Check In Document: " + ldoc.doc_write_error_text, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            End If
            Me.RetrieveDocs()


        Catch ex As Exception
            Dim db_err As New bc_cs_error_log(className, moduleName, bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Public last_mouse_move As Date
    Public Sub check_doc_list_inactivity()
        If screen_update_required = True And pollingEnabled = True Then
            Me.location = 0
            RetrieveDocs()
        Else
            If AutoRefresh = True Then
                If DateDiff(DateInterval.Second, last_mouse_move, Now) > screenInactiveInterval Then
                    Me.location = 0
                    RetrieveDocs()
                End If
            End If
            last_mouse_move = Now
        End If

    End Sub

    Private Sub invoke_settings()
        Dim fs As New bc_am_process_settings
        fs.ctrllr = Me

        load_settings(fs)




        fs.ShowDialog()
        If fs.cancel_selected Then
            Exit Sub
        End If

        alerterEnabled = fs.Chkalert.Checked
        fadeInterval = fs.lfade.Text
        pollingEnabled = fs.Chkpoll.Checked
        AutoRefresh = fs.Chkar.Checked
        screenInactiveInterval = fs.Lactivity.Text
        pollingInterval = fs.Lpoll.Text
        refreshsizechanged = fs.Chksizechanged.Checked
        beepEnabled = fs.Chkbeep.Checked
        daysBack = fs.cxdaysback.Text
        automaticdoclistupdate = fs.chkupdate.Checked

        preExpireAlertNotify = fs.uxurgent.Text


        bc_am_process.colors.doc_list_read_backcolor = fs.bc1.Appearance.BackColor
        bc_am_process.colors.doc_list_read_forecolor = fs.fc1.Appearance.BackColor
        bc_am_process.colors.doc_list_unread_backcolor = fs.bc2.Appearance.BackColor
        bc_am_process.colors.doc_list_unread_forecolor = fs.fc2.Appearance.BackColor
        bc_am_process.colors.doc_list_stage_changed_backcolor = fs.bc3.Appearance.BackColor
        bc_am_process.colors.doc_list_stage_changed_forecolor = fs.fc3.Appearance.BackColor
        bc_am_process.colors.doc_list_urgent_backcolor = fs.bc4.Appearance.BackColor
        bc_am_process.colors.doc_list_urgent_forecolor = fs.fc4.Appearance.BackColor
        bc_am_process.colors.doc_list_expired_backcolor = fs.bc5.Appearance.BackColor
        bc_am_process.colors.doc_list_expired_forecolor = fs.fc5.Appearance.BackColor
        bc_am_process.colors.workflow_current_stage_backcolor = fs.bc6.Appearance.BackColor
        bc_am_process.colors.workflow_current_stage_forecolor = fs.fc6.Appearance.BackColor
        bc_am_process.colors.workflow_next_stage_backcolor = fs.bc7.Appearance.BackColor
        bc_am_process.colors.workflow_next_stage_forecolor = fs.fc7.Appearance.BackColor

        If fs.restore_defaults = True Then
            For i = 0 To documentListView.uxDocumentListView.Columns.Count - 1
                documentListView.uxDocumentListView.Columns(i).VisibleIndex = i
            Next
            For i = 0 To documentListView.uxDocumentListView.Columns.Count - 1
                If i = 0 Or i = 13 Or i = 14 Then
                    documentListView.uxDocumentListView.Columns(i).Width = 20
                ElseIf i > 0 And i < 8 Then
                    documentListView.uxDocumentListView.Columns(i).Width = 74
                ElseIf i = 8 Then
                    documentListView.uxDocumentListView.Columns(i).Width = 83
                ElseIf i = 9 Then
                    documentListView.uxDocumentListView.Columns(i).Width = 68
                ElseIf i = 10 Or i = 11 Then
                    documentListView.uxDocumentListView.Columns(i).Width = 78
                ElseIf i = 12 Then
                    documentListView.uxDocumentListView.Columns(i).Width = 68
                End If
            Next
        End If



        For i = 0 To documentListView.uxDocumentListView.Columns.Count - 1
            documentListView.uxDocumentListView.Columns(i).Visible = False
            For j = 0 To fs.chkcolumns.CheckedItems.Count - 1
                If fs.chkcolumns.CheckedItems(j) = documentListView.uxDocumentListView.Columns(i).GetTextCaption Then
                    documentListView.uxDocumentListView.Columns(i).Visible = True
                    Exit For
                End If
            Next
        Next
        set_status_text()
    End Sub
    Public Sub restore_defaults(ByVal fs As bc_am_process_settings)
        fs.Chkalert.Checked = False
        fs.Chkpoll.Checked = False
        fs.Chkar.Checked = False
        fs.Chkbeep.Checked = False
        fs.Chksizechanged.Checked = False
        fs.chkupdate.Checked = False
        fs.Lactivity.Text = "60"
        fs.lfade.Text = "5"
        fs.Lpoll.Text = "60"
        fs.cxdaysback.Text = "1"
        fs.uxurgent.Text = "4"

        fs.bc5.Appearance.BackColor = System.Drawing.Color.LightCoral
        fs.fc5.Appearance.BackColor = System.Drawing.Color.Black
        fs.bc1.Appearance.BackColor = System.Drawing.Color.LightGray
        fs.fc1.Appearance.BackColor = System.Drawing.Color.Black
        fs.bc3.Appearance.BackColor = System.Drawing.Color.LightYellow
        fs.fc3.Appearance.BackColor = System.Drawing.Color.Black
        fs.bc2.Appearance.BackColor = System.Drawing.Color.LightGreen
        fs.fc2.Appearance.BackColor = System.Drawing.Color.Black
        fs.bc4.Appearance.BackColor = System.Drawing.Color.LightPink
        fs.fc4.Appearance.BackColor = System.Drawing.Color.Black
        fs.bc6.Appearance.BackColor = System.Drawing.Color.LightGreen
        fs.fc6.Appearance.BackColor = System.Drawing.Color.Black
        fs.bc7.Appearance.BackColor = System.Drawing.Color.LightCoral
        fs.fc7.Appearance.BackColor = System.Drawing.Color.Black


    End Sub
    Private Sub load_settings(ByVal fs As bc_am_process_settings)
        fs.Chkalert.Checked = alerterEnabled
        fs.Chksizechanged.Checked = refreshsizechanged
        For i = 0 To fs.lfade.Items.Count - 1
            If fs.lfade.Items(i).ToString = fadeInterval Then
                fs.lfade.SelectedIndex = i
                Exit For
            End If
        Next
        fs.Chkpoll.Checked = pollingEnabled
        fs.Chkar.Checked = AutoRefresh
        For i = 0 To fs.Lactivity.Items.Count - 1
            If fs.Lactivity.Items(i).ToString = screenInactiveInterval Then
                fs.Lactivity.SelectedIndex = i
                Exit For
            End If
        Next
        fs.chkupdate.Checked = automaticdoclistupdate
        For i = 0 To fs.Lpoll.Items.Count - 1
            If fs.Lpoll.Items(i).ToString = pollingInterval Then
                fs.Lpoll.SelectedIndex = i
                Exit For
            End If
        Next

        For i = 0 To fs.cxdaysback.Items.Count - 1
            If fs.cxdaysback.Items(i).ToString = Me.daysBack Then
                fs.cxdaysback.SelectedIndex = i
                Exit For
            End If
        Next



        For i = 0 To fs.uxurgent.Items.Count - 1
            If fs.uxurgent.Items(i).ToString = Me.preExpireAlertNotify Then
                fs.uxurgent.SelectedIndex = i
                Exit For
            End If
        Next
        fs.bc1.Appearance.BackColor = bc_am_process.colors.doc_list_read_backcolor
        fs.fc1.Appearance.BackColor = bc_am_process.colors.doc_list_read_forecolor
        fs.bc2.Appearance.BackColor = bc_am_process.colors.doc_list_unread_backcolor
        fs.fc2.Appearance.BackColor = bc_am_process.colors.doc_list_unread_forecolor
        fs.bc3.Appearance.BackColor = bc_am_process.colors.doc_list_stage_changed_backcolor
        fs.fc3.Appearance.BackColor = bc_am_process.colors.doc_list_stage_changed_forecolor
        fs.bc4.Appearance.BackColor = bc_am_process.colors.doc_list_urgent_backcolor
        fs.fc4.Appearance.BackColor = bc_am_process.colors.doc_list_urgent_forecolor
        fs.bc5.Appearance.BackColor = bc_am_process.colors.doc_list_expired_backcolor
        fs.fc5.Appearance.BackColor = bc_am_process.colors.doc_list_expired_forecolor
        fs.bc6.Appearance.BackColor = bc_am_process.colors.workflow_current_stage_backcolor
        fs.fc6.Appearance.BackColor = bc_am_process.colors.workflow_current_stage_forecolor
        fs.bc7.Appearance.BackColor = bc_am_process.colors.workflow_next_stage_backcolor
        fs.fc7.Appearance.BackColor = bc_am_process.colors.workflow_next_stage_forecolor

        For i = 0 To documentListView.uxDocumentListView.Columns.Count - 1
            For j = 0 To documentListView.uxDocumentListView.Columns.Count - 1
                If documentListView.uxDocumentListView.Columns(j).VisibleIndex = i Then
                    If documentListView.uxDocumentListView.Columns(j).GetTextCaption <> "" Then
                        If documentListView.uxDocumentListView.Columns(j).Visible = True Then
                            fs.chkcolumns.Items.Add(documentListView.uxDocumentListView.Columns(j).GetTextCaption, True)
                        Else
                            fs.chkcolumns.Items.Add(documentListView.uxDocumentListView.Columns(j).GetTextCaption, False)
                        End If
                    End If
                End If
            Next
        Next
        For i = 0 To documentListView.uxDocumentListView.Columns.Count - 1
            If i = 11 And bc_am_load_objects.obc_pub_types.process_switches.distribute = False Then
                Continue For
            End If
            If (i = 12 Or i = 13) And bc_am_load_objects.obc_pub_types.process_switches.attestation_screen = False Then
                Continue For
            End If
            If i = 14 And bc_am_load_objects.obc_pub_types.process_switches.fast_track = False Then
                Continue For
            End If
            If i = 15 And bc_am_load_objects.obc_pub_types.process_switches.attribute_change = False Then
                Continue For
            End If
            If documentListView.uxDocumentListView.Columns(i).VisibleIndex = -1 Then
                If documentListView.uxDocumentListView.Columns(i).GetTextCaption <> "" Then
                    If documentListView.uxDocumentListView.Columns(i).Visible = True Then
                        fs.chkcolumns.Items.Add(documentListView.uxDocumentListView.Columns(i).GetTextCaption, True)
                    Else
                        fs.chkcolumns.Items.Add(documentListView.uxDocumentListView.Columns(i).GetTextCaption, False)
                    End If
                End If
            End If
        Next

    End Sub
    Private Sub editMetadata(ByVal ldoc As bc_om_document)
        Dim moduleName As String = "editMetadata"
        Dim slog As New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_ENTRY, "")

        Try

            REM invoke submit form
            REM show submit disalogue
            REM can only edit if write access on document
            Dim catreadonly As Boolean = False
            If location = 0 Then
                If localDocument.checked_out_user <> 0 Or localDocument.write <> True Then
                    catreadonly = True
                Else
                    Dim tid As Long
                    tid = ldoc.id

                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        ldoc.db_read_for_categorize()

                    ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                        ldoc.tmode = bc_cs_soap_base_class.tREAD
                        ldoc.read_mode = bc_om_document.READ_FOR_CATEGORIZE
                        ldoc.transmit_to_server_and_receive(ldoc, True)
                    End If

                    If check_document_state(ldoc.id, True) = False Then
                        Exit Sub
                    End If


                    ldoc.id = tid
                End If
                'Else
                '   ldoc = ldoc.read_data_from_file(bc_cs_central_settings.local_repos_path + Left(ldoc.filename, InStrRev(ldoc.filename, ".") - 1) + ".dat")



            End If
            'REM ===
            ldoc.doc_date = ldoc.doc_date.ToLocalTime
            Dim osubmit As New bc_dx_as_categorize
            osubmit.show_stage_change = False
            osubmit.show_local_submit = False

            osubmit.enable_pub_types = True
            osubmit.document = ldoc
            osubmit.enable_lead_entity = False

            osubmit.show_ext = True


            osubmit.ok_button_caption = "Update"

            Dim pts As New List(Of String)
            pts.Add(ldoc.pub_type_name)
            osubmit.set_pub_types = pts
            If catreadonly = True Then
                osubmit.caption = "Blue Curve Process - Show Metadata For: " + ldoc.title
                osubmit.read_only = True
            Else
                osubmit.caption = "Blue Curve Process - Update Metadata For: " + ldoc.title
            End If

            osubmit.ShowDialog()

            If catreadonly = True Then
                Exit Sub
            End If

            Cursor.Current = Cursors.WaitCursor

            If osubmit.ok_selected = True Then
                REM write metadata to system
                ldoc.history.Clear()
                'ldoc.history.Add("Metadata Changed")
                ldoc.support_documents.Clear()
                ldoc.bimport_support_only = True
                ldoc.bwith_document = False
                If location = 0 Then
                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        ldoc.db_write(Nothing)
                    Else
                        ldoc.tmode = bc_cs_soap_base_class.tWRITE

                        If ldoc.transmit_to_server_and_receive(ldoc, True) = False Then
                            Exit Sub
                        End If
                    End If

                    If ldoc.doc_write_success = False Then
                        Dim omsg As New bc_cs_message("Blue Curve", "Failed to Change Metadata: " + ldoc.doc_write_error_text, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    End If
                Else
                    ldoc.write_data_to_file(bc_cs_central_settings.local_repos_path + ldoc.filename + ".dat")
                    REM now put back in local document collection
                    Dim local_documents As New bc_om_documents
                    local_documents = local_documents.read_data_from_file(bc_cs_central_settings.local_repos_path + "bc_local_documents.dat")
                    For i = 0 To local_documents.document.Count - 1
                        If local_documents.document(i).id = 0 AndAlso (local_documents.document(i).filename = ldoc.filename + ldoc.extension Or local_documents.document(i).filename = ldoc.filename) Then
                            local_documents.document(i) = ldoc
                            local_documents.write_data_to_file(bc_cs_central_settings.local_repos_path + "bc_local_documents.dat")
                            Exit For
                        End If
                    Next


                End If

            End If


        Catch ex As Exception
            Dim db_err As New bc_cs_error_log(className, moduleName, bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

            Me.RetrieveDocs(True, True)
            Cursor.Current = Cursors.Default

            slog = New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Private Sub importRegularReport(ByVal sdoc As bc_om_document)
        Dim moduleName As String = "importRegularReport"
        Dim slog As New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_EXIT, "")

        Try
            'Dim i As Integer
            'Dim extensionsize As Integer

            Dim fs As New bc_cs_file_transfer_services
            Dim ocommentary As bc_cs_activity_log


            REM ===================================
            'Dim sdoc As New bc_om_document


            Dim osubmit As New bc_dx_as_categorize
            sdoc.allow_disclosures = False
            For i = 0 To bc_am_load_objects.obc_prefs.stage_role_access_codes.Count - 1
                If bc_am_load_objects.obc_prefs.stage_role_access_codes(i).stage_id = 1 AndAlso bc_am_load_objects.obc_prefs.stage_role_access_codes(i).access_id = "D" Then
                    sdoc.allow_disclosures = True
                    Exit For
                End If
            Next

            osubmit.show_stage_change = False
            osubmit.show_local_submit = False
            osubmit.enable_pub_types = True
            osubmit.enable_lead_entity = False

            osubmit.import_master_mode = True


            sdoc.id = 0
            sdoc.stage = 1
            sdoc.originating_author = bc_cs_central_settings.logged_on_user_id
            Dim ouser As New bc_om_user
            ouser.id = sdoc.originating_author
            'sdoc.authors.Add(ouser)
            osubmit._doc = sdoc
            Dim pts As New List(Of String)

            For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                If bc_am_load_objects.obc_pub_types.pubtype(i).id = sdoc.pub_type_id Then
                    pts.Add(bc_am_load_objects.obc_pub_types.pubtype(i).name)
                    Exit For
                End If
            Next
            osubmit.set_pub_types = pts

            osubmit.caption = "Blue Curve Process - Import Regular Report"
            osubmit.ok_button_caption = "Import"
            osubmit.file = True
            osubmit.attachment = True
            'osubmit.enable_lead = True

            osubmit.import_regular_report = True
            osubmit.ShowDialog()
            If Not osubmit.ok_selected Then
                REM if cancel selected do nothing
                ocommentary = New bc_cs_activity_log("bc_am_workflow", "import_master_document", bc_cs_activity_codes.COMMENTARY, "Cancel Selected on Submit form.")
                Exit Sub
            End If


            sdoc.extension = "[imp]" + sdoc.extension

            Cursor.Current = Cursors.WaitCursor
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                sdoc.db_write(Nothing)
            Else
                sdoc.tmode = bc_cs_soap_base_class.tWRITE
                sdoc.transmit_to_server_and_receive(sdoc, True)
            End If
            If sdoc.doc_write_success = False Then
                Dim omsg As New bc_cs_message("Blue Curve", "Failed to Import Master Document: " + sdoc.doc_write_error_text, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            End If

            Me.RetrieveDocs()


        Catch ex As Exception
            Dim db_err As New bc_cs_error_log(className, moduleName, bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Cursor.Current = Cursors.Default

            slog = New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    Private Sub importMasterDocument(ByVal ldoc As bc_om_document, Optional ByVal register_only As Boolean = False, Optional ByVal regular_report As Boolean = False)
        Dim moduleName As String = "importMasterDocument"
        Dim slog As New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_EXIT, "")

        Try
            'Dim i As Integer
            'Dim extensionsize As Integer

            Dim fs As New bc_cs_file_transfer_services
            Dim ocommentary As bc_cs_activity_log


            REM ===================================
            Dim sdoc As New bc_om_document
            Dim osubmit As New bc_dx_as_categorize
            osubmit.import_master_mode = True

            osubmit.show_stage_change = False
            osubmit.show_local_submit = False
            osubmit.enable_pub_types = True
            osubmit.enable_lead_entity = True

            sdoc.id = 0
            sdoc.stage = 1
            sdoc.originating_author = bc_cs_central_settings.logged_on_user_id
            REM get default disclosues tabe setting
            sdoc.allow_disclosures = False
            For i = 0 To bc_am_load_objects.obc_prefs.stage_role_access_codes.Count - 1
                If bc_am_load_objects.obc_prefs.stage_role_access_codes(i).stage_id = 1 AndAlso bc_am_load_objects.obc_prefs.stage_role_access_codes(i).access_id = "D" Then
                    sdoc.allow_disclosures = True
                    Exit For
                End If
            Next
            Dim ouser As New bc_om_user
            ouser.id = sdoc.originating_author
            If bc_am_load_objects.obc_pub_types.process_switches.assign_author_on_import = True Then
                sdoc.authors.Add(ouser)
            End If




            osubmit._doc = sdoc
            Dim pts As New List(Of String)
            For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                If bc_am_load_objects.obc_pub_types.pubtype(i).support_doc_only = False Then
                    pts.Add(bc_am_load_objects.obc_pub_types.pubtype(i).name)
                End If
            Next
            osubmit.set_pub_types = pts

            If register_only = False Then
                osubmit.caption = "Blue Curve Process - Import Master Document"
                osubmit.ok_button_caption = "Import"
                osubmit.file = True
            Else
                osubmit.caption = "Blue Curve Process - Register Master Document"
                osubmit.ok_button_caption = "Register"
                osubmit.file = False
            End If

            osubmit.ShowDialog()
            If Not osubmit.ok_selected Then
                REM if cancel selected do nothing
                ocommentary = New bc_cs_activity_log("bc_am_workflow", "import_master_document", bc_cs_activity_codes.COMMENTARY, "Cancel Selected on Submit form.")
                Exit Sub
            End If

            If register_only = True Then
                sdoc.register_template = True
            Else
                sdoc.extension = "[imp]" + sdoc.extension
            End If

            Cursor.Current = Cursors.WaitCursor
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                sdoc.db_write(Nothing)
            Else
                sdoc.tmode = bc_cs_soap_base_class.tWRITE
                sdoc.transmit_to_server_and_receive(sdoc, True)
            End If
            If sdoc.doc_write_success = False Then
                Dim omsg As New bc_cs_message("Blue Curve", "Failed to Import Master Document: " + sdoc.doc_write_error_text, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            End If

            Me.RetrieveDocs()


        Catch ex As Exception
            Dim db_err As New bc_cs_error_log(className, moduleName, bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Cursor.Current = Cursors.Default

            slog = New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Private Sub importSupportDocument(ByVal ldoc As bc_om_document)
        Dim moduleName As String = "importSupportDocument"
        Dim slog As New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_ENTRY, "")

        Try

            Dim tid As Long

            tid = ldoc.id

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                ldoc.db_read_for_categorize()

            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                ldoc.tmode = bc_cs_soap_base_class.tREAD
                ldoc.read_mode = bc_om_document.READ_FOR_CATEGORIZE
                ldoc.transmit_to_server_and_receive(ldoc, True)
            End If

            If check_document_state(ldoc.id, True) = False Then
                Exit Sub
            End If

            ldoc.id = tid

            'Dim fs As New bc_cs_file_transfer_services
            'Dim odialog As New OpenFileDialog
            'Dim extensionsize As Integer


            REM =============
            Dim sdoc As New bc_om_document
            sdoc.id = 0
            sdoc.master_flag = False
            sdoc.pub_type_id = ldoc.pub_type_id
            sdoc.pub_type_name = ldoc.pub_type_name

            For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                If bc_am_load_objects.obc_pub_types.pubtype(i).id = ldoc.pub_type_id Then
                    If bc_am_load_objects.obc_pub_types.pubtype(i).default_support_pub_type <> 0 Then
                        sdoc.pub_type_id = bc_am_load_objects.obc_pub_types.pubtype(i).default_support_pub_type
                        sdoc.pub_type_name = ldoc.pub_type_name
                        For j = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                            If bc_am_load_objects.obc_pub_types.pubtype(j).id = sdoc.pub_type_id Then
                                sdoc.pub_type_name = bc_am_load_objects.obc_pub_types.pubtype(j).name
                                Exit For
                            End If
                        Next
                        Exit For
                    End If
                End If
            Next

            sdoc.allow_disclosures = False
            For i = 0 To bc_am_load_objects.obc_prefs.stage_role_access_codes.Count - 1
                If bc_am_load_objects.obc_prefs.stage_role_access_codes(i).stage_id = 1 AndAlso bc_am_load_objects.obc_prefs.stage_role_access_codes(i).access_id = "D" Then
                    sdoc.allow_disclosures = True
                    Exit For
                End If
            Next

            sdoc.originating_author = ldoc.originating_author
            sdoc.bus_area = ldoc.bus_area
            sdoc.checked_out_user = 0
            sdoc.stage = ldoc.stage
            'sdoc.doc_date = ldoc.doc_date
            sdoc.entity_id = ldoc.entity_id
            sdoc.originating_author = ldoc.originating_author
            sdoc.authors = ldoc.authors
            sdoc.taxonomy = ldoc.taxonomy
            sdoc.disclosures = ldoc.disclosures
            sdoc.workflow_stages = ldoc.workflow_stages
            sdoc.doc_date = Now
            sdoc.doc_date = sdoc.doc_date.ToLocalTime
            Dim osubmit As New bc_dx_as_categorize
            osubmit.show_stage_change = False
            osubmit.show_local_submit = False

            osubmit.enable_pub_types = True
            osubmit.enable_lead_entity = True
            osubmit.document = sdoc
            osubmit.file = True
            osubmit.caption = "Blue Curve Process - Import Support Document For: " + ldoc.title
            osubmit.ok_button_caption = "Import"
            osubmit.master_pub_type_id = ldoc.pub_type_id


            Dim pts As New List(Of String)
            For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                pts.Add(bc_am_load_objects.obc_pub_types.pubtype(i).name)
            Next
            osubmit.set_pub_types = pts

            osubmit._doc = sdoc
            osubmit.Focus()
            osubmit.import_support_document = True

            osubmit.ShowDialog()
            If osubmit.ok_selected = False Then
                REM if cancel selected do nothing
                Dim ocommentary As New bc_cs_activity_log("bc_am_workflow", "import_master_document", bc_cs_activity_codes.COMMENTARY, "Cancel Selected on Submit form.")
                Exit Sub
            End If



            ldoc.support_documents.Clear()
            ldoc.support_documents.Add(sdoc)
            ldoc.register_only = False
            ldoc.bcheck_out = False
            ldoc.bwith_document = False
            ldoc.history.Clear()
            ldoc.bimport_support_only = True


            Cursor.Current = Cursors.WaitCursor
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                ldoc.db_write(Nothing)
            Else
                ldoc.tmode = bc_cs_soap_base_class.tWRITE
                If ldoc.transmit_to_server_and_receive(ldoc, True) = False Then
                    Exit Sub
                End If
            End If
            If ldoc.doc_write_success = False Then
                Dim omsg As New bc_cs_message("Blue Curve", "Failed to imports support document: " + ldoc.doc_write_error_text, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log(className, moduleName, bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.RetrieveDocs(True, True)

            Cursor.Current = Cursors.Default
            slog = New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Private Sub importRegisteredDocument(ByVal ldoc As bc_om_document)
        Dim moduleName As String = "importRegisteredDocument"
        Dim slog As New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_ENTRY, "")

        Try


            Dim fs As New bc_cs_file_transfer_services

            Dim odialog As New OpenFileDialog
            Dim extensionsize As Integer

            odialog.Title = "Import Registered Document for " + ldoc.title

            odialog.Filter = ""
            For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                If bc_am_load_objects.obc_pub_types.pubtype(i).id = ldoc.pub_type_id Then
                    odialog.Filter = bc_am_load_objects.obc_pub_types.pubtype(i).master_doc_filter()
                    Exit For
                End If

            Next

            odialog.ShowDialog()

            REM SW cope with office versions
            extensionsize = (Len(odialog.FileName) - (InStrRev(odialog.FileName, ".") - 1))

            If odialog.FileName <> "" Then
                ldoc.register_template = False
                ldoc.extension = "[imp]" + Right(odialog.FileName, extensionsize)
                ldoc.register_only = False
                ldoc.filename = odialog.FileName
                ldoc.bwith_document = True
                If fs.write_document_to_bytestream(odialog.FileName, ldoc.byteDoc, Nothing, False) = False Then
                    Dim omessage As New bc_cs_message("Blue Curve", "File: " + odialog.FileName + " can't be accessed", bc_cs_message.MESSAGE)
                    Exit Sub
                End If
                ldoc.support_documents.Clear()
                ldoc.register_only = False
                ldoc.register_template = True
                ldoc.bcheck_out = False
                ldoc.history.Clear()
                'ldoc.history.Add("Registered Document Attached")
                Cursor.Current = Cursors.WaitCursor
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    ldoc.db_write(Nothing)
                Else
                    ldoc.tmode = bc_cs_soap_base_class.tWRITE
                    If ldoc.transmit_to_server_and_receive(ldoc, True) = False Then
                        Exit Sub
                    End If
                End If
                If ldoc.doc_write_success = False Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Failed to Import Registed Document: " + ldoc.doc_write_error_text, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                End If
                Me.RetrieveDocs(True, True)

            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_workflow", "import_registered_document", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

            Cursor.Current = Cursors.Default
            slog = New bc_cs_activity_log("bc_am_workflow", "import_registered_document", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Private Sub attachNewDocument(ByVal ldoc As bc_om_document)
        Dim moduleName As String = "attachNewDocument"
        Dim slog As New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_ENTRY, "")

        Try


            Dim fs As New bc_cs_file_transfer_services
            Dim extensionsize As Integer

            Dim odialog As New OpenFileDialog
            odialog.Title = "Attach New Document for " + ldoc.title

            REM filter for pub type
            odialog.Filter = ""
            For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                If bc_am_load_objects.obc_pub_types.pubtype(i).id = ldoc.pub_type_id Then
                    odialog.Filter = bc_am_load_objects.obc_pub_types.pubtype(i).master_doc_filter()
                    Exit For
                End If

            Next
            odialog.ShowDialog()
            ldoc.original_extension = ldoc.extension
            If odialog.FileName <> "" Then

                REM SW cope with office versions
                extensionsize = (Len(odialog.FileName) - (InStrRev(odialog.FileName, ".") - 1))
                ldoc.extension = "[imp]" + Right(odialog.FileName, extensionsize)
                'store current filename
                ldoc.revision_filename = ldoc.filename
                ldoc.bwith_document = True


                '========


                If bc_am_load_objects.obc_pub_types.process_switches.import_if_open = True Then

                    If fs.write_document_to_bytestream_doc_open(odialog.FileName, ldoc.byteDoc, Nothing, True) = False Then
                        Dim omessage As New bc_cs_message("Blue Curve", "File: " + odialog.FileName + " cannot be accessed.", bc_cs_message.MESSAGE, False, False, "Yes", "Ok", True)
                        Exit Sub
                    End If

                Else
                    If fs.write_document_to_bytestream(odialog.FileName, ldoc.byteDoc, Nothing, False) = False Then
                        Dim omessage As New bc_cs_message("Blue Curve", "File: " + odialog.FileName + " can't be accessed", bc_cs_message.MESSAGE)
                        Exit Sub
                    End If
                End If

                '========


                ldoc.support_documents.Clear()
                ldoc.register_only = False
                ldoc.register_template = True
                ldoc.bcheck_out = False
                ldoc.history.Clear()
                ldoc.btake_revision = True
                Cursor.Current = Cursors.WaitCursor

                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then

                    ldoc.db_write(Nothing)
                Else
                    ldoc.tmode = bc_cs_soap_base_class.tWRITE
                    ldoc.transmit_to_server_and_receive(ldoc, True)
                End If
                If ldoc.doc_write_success = False Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Failed to Attach New Document: " + ldoc.doc_write_error_text, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                End If
                Me.RetrieveDocs(True, True)

            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log(className, moduleName, bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

            Cursor.Current = Cursors.Default
            slog = New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Private Sub loadDocumentDetails()
        Dim moduleName As String = "loadDocumentDetails"
        Dim slog As New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            If location = 0 Then
                populateWorkflowTree()

                gcomment = ""
                workflowEnabled = False
                documentDetailsView.uxCurrentStageIndicator.BackColor = colors.workflow_current_stage_backcolor
                documentDetailsView.uxNextStageIndicator.BackColor = colors.workflow_next_stage_backcolor

                If localDocument.move = False Then
                    gcomment = "You do not have permission."
                ElseIf localDocument.checked_out_user <> "0" Then
                    gcomment = "Document is Checked out."
                End If
                If localDocument.move = True And localDocument.checked_out_user = "0" Then
                    REM if support docs checked out can't move
                    sDoc = False
                    For i = 0 To localDocument.support_documents.Count - 1
                        If localDocument.support_documents(i).checked_out_user <> "0" Then
                            sDoc = True
                            'DISCUSS WAHT TO DO WITH THIS
                            gcomment = "Support Document(s) checked out so can't change stage."

                            Exit For
                        End If
                    Next
                    If sDoc = False Then
                        workflowEnabled = True
                        REM if has move rights then allow workflow                    
                        If localDocument.reject = True Then
                            localDocument.workflow_stages.stages(1).stage_id = localDocument.reject_stage
                            localDocument.workflow_stages.stages(1).stage_name = localDocument.reject_stage_name
                            localDocument.workflow_stages.stages(1).action_ids.clear()
                            localDocument.workflow_stages.stages(1).num_approvers = 0
                        End If
                    End If

                End If


                documentDetailsView.uxWorkflowTree.CollapseAll()
                highlightCurrent()

                documentDetailsView.lurgent.Visible = False
                If localDocument.urgent_flag = True Then
                    documentDetailsView.lurgent.Visible = True
                End If
            End If
            documentDetailsView.uxStageLabel.Text = localDocument.stage_name
            documentDetailsView.uxPubTypeLabel.Text = localDocument.pub_type_name

            If location = 0 Then
                documentDetailsView.uxupdatelbl.Text = Format(localDocument.update_date.ToLocalTime, "dd-MMM-yyyy HH:mm")
                documentDetailsView.uxcreatedate.Text = Format(localDocument.create_date.ToLocalTime, "dd-MMM-yyyy HH:mm")
            Else
                documentDetailsView.uxStageLabel.Text = "Unsubmitted"
                documentDetailsView.uxupdatelbl.Text = "n/a"
                documentDetailsView.uxcreatedate.Text = "n/a"
            End If

            documentDetailsView.uxSubmissionDate.Text = Format(localDocument.doc_date.ToLocalTime, "dd-MMM-yyyy HH:mm")

            If localDocument.stage_expire_date <> CDate("09/09/9999") Then
                documentDetailsView.uxcomp.Text = Format(localDocument.stage_expire_date.ToLocalTime, "dd-MMM-yyyy HH:mm")
            Else
                documentDetailsView.uxcomp.Text = "n/a"
            End If

            Me.documentDetailsView.Pdiss.Visible = False
            If localDocument.completed_date.Year <> 1900 Or localDocument.disemination_date.Year <> 1900 Then
                Me.documentDetailsView.Pdiss.Visible = True
                If localDocument.completed_date.Year <> 1900 Then
                    Me.documentDetailsView.dmardate.Text = Format(localDocument.completed_date.ToLocalTime, "dd-MMM-yyyy HH:mm")
                Else
                    Me.documentDetailsView.dmardate.Text = "not set"
                End If
                If localDocument.disemination_date.Year <> 1900 Then
                    Me.documentDetailsView.dmardisdate.Text = Format(localDocument.disemination_date.ToLocalTime, "dd-MMM-yyyy HH:mm")
                Else
                    Me.documentDetailsView.dmardisdate.Text = "not set"
                End If
            End If

            documentDetailsView.uxoriglabel.Text = docList(documentListView.uxDocumentListView.GetDataSourceRowIndex(documentListView.uxDocumentListView.GetSelectedRows(0))).Author
            Dim na As String = ""

            If localDocument.behalf_of_author_id > 0 Then
                For i = 0 To bc_am_load_objects.obc_users.user.Count - 1
                    If bc_am_load_objects.obc_users.user(i).id = localDocument.behalf_of_author_id Then
                        na = bc_am_load_objects.obc_users.user(i).first_name + " " + bc_am_load_objects.obc_users.user(i).surname
                        Exit For
                    End If
                Next
                documentDetailsView.uxoriglabel.Text = documentDetailsView.uxoriglabel.Text + " (by " + na + ")"
            End If

            documentDetailsView.uxclasslbl.Text = docList(documentListView.uxDocumentListView.GetDataSourceRowIndex(documentListView.uxDocumentListView.GetSelectedRows(0))).Entity_Name

            If location = 0 Then
                load_document_comments(localDocument.comments.comments)
                documentDetailsView.Hlatest.Focus()
                load_document_history(localDocument.history)

                load_document_links(localDocument.links)

                'load_permissions_text(localDocument)

                load_support_docs(localDocument.support_documents)

                load_document_revisions(localDocument.history)
            End If
            REM load mime type image
            Dim image As Bitmap
            image = Nothing
            Dim bcs As New bc_cs_icon_services

            image = bcs.get_icon_for_file_type(Replace(localDocument.extension, "[imp]", ""))
            If IsNothing(image) = False Then
                documentDetailsView.uxmime.Image = image
            Else
                If localDocument.extension <> "" Then
                    documentDetailsView.uxmime.Image = documentDetailsView.uxlargeimages.Images(0)
                Else
                    documentDetailsView.uxmime.Image = documentDetailsView.uxlargeimages.Images(1)
                End If
            End If
            documentDetailsView.s5.Enabled = False
            documentDetailsView.s4.Enabled = False
            documentDetailsView.s3.Enabled = False
            documentDetailsView.s2.Enabled = False
            documentDetailsView.s1.Enabled = False

            documentDetailsView.s5.Image = Nothing
            documentDetailsView.s4.Image = Nothing
            documentDetailsView.s3.Image = Nothing
            documentDetailsView.s2.Image = Nothing
            documentDetailsView.s1.Image = Nothing
            If localDocument.read = True Then
                documentDetailsView.s5.Image = documentDetailsView.uxDocumentDetailImages.Images(11)

                If localDocument.extension <> "" Then
                    documentDetailsView.s5.Enabled = True
                End If
            End If
            If localDocument.write = True Then
                documentDetailsView.s4.Image = documentDetailsView.uxDocumentDetailImages.Images(13)
                If localDocument.extension <> "" And (localDocument.checked_out_user = 0 Or localDocument.checked_out_user = bc_cs_central_settings.logged_on_user_id) Then
                    documentDetailsView.s4.Enabled = True
                End If
                If localDocument.locked = True Then
                    documentDetailsView.s1.Image = documentDetailsView.uxDocumentDetailImages.Images(9)
                    If localDocument.extension = ".doc" Or localDocument.extension = ".docx" Or localDocument.extension = ".ppt" Or localDocument.extension = ".pptx " Then
                        If localDocument.checked_out_user = 0 Or localDocument.checked_out_user = bc_cs_central_settings.logged_on_user_id Then
                            documentDetailsView.s1.Enabled = True
                        End If
                    End If

                End If
            End If
            If localDocument.move = True Then
                documentDetailsView.s3.Image = documentDetailsView.uxDocumentDetailImages.Images(29)
                If localDocument.checked_out_user = 0 Then
                    documentDetailsView.s3.Enabled = True
                End If
            End If
            If localDocument.force_check_in = True Then
                documentDetailsView.s2.Image = documentDetailsView.uxDocumentDetailImages.Images(2)
                If localDocument.checked_out_user <> 0 And localDocument.checked_out_user <> bc_cs_central_settings.logged_on_user_id Then
                    documentDetailsView.s2.Enabled = True
                End If
            End If
            documentDetailsView.RadioGroup1.SelectedIndex = 1


        Catch ex As Exception
            Dim db_err As New bc_cs_error_log(className, moduleName, bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    Public Sub load_document_comments_from_system(ByVal span As Integer)
        Dim moduleName As String = "load_document_comments_from_system"
        Dim slog As New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim ocomments As New bc_om_comments
            ocomments.doc_id = localDocument.id
            ocomments.span = span
            documentDetailsView.Cursor = Cursors.WaitCursor

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                ocomments.db_read()
            Else
                ocomments.tmode = bc_cs_soap_base_class.tREAD
                If ocomments.transmit_to_server_and_receive(ocomments, True) = False Then
                    Exit Sub
                End If
            End If
            load_document_comments(ocomments.comments)
            documentDetailsView.Cursor = Cursors.Default

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log(className, moduleName, bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Private Sub load_document_comments(ByVal comments As List(Of bc_om_comment))
        Dim moduleName As String = "load_document_comments"
        Dim slog As New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            documentDetailsView.uxcommentlist.BeginUnboundLoad()
            documentDetailsView.uxcommentlist.Nodes.Clear()

            For i = 0 To comments.Count - 1
                Dim tln As Nodes.TreeListNode = Nothing
                If i > 0 Then
                    documentDetailsView.uxcommentlist.AppendNode(New Object() {comments(i).user_name + " (" + comments(i).stage_name + ") " + Format(comments(i).da.ToLocalTime, "dd-MMM-yyyy HH:mm")}, tln).Tag = "user"
                    documentDetailsView.uxcommentlist.AppendNode(New Object() {comments(i).comment}, documentDetailsView.uxcommentlist.Nodes(i)).Tag = "comment"
                Else
                    documentDetailsView.uxcommentlist.AppendNode(New Object() {comments(i).user_name + " (" + comments(i).stage_name + ") " + Format(comments(i).da.ToLocalTime, "dd-MMM-yyyy HH:mm")}, -1, -1, -1, 2).Tag = "topuser"
                    documentDetailsView.uxcommentlist.AppendNode(New Object() {comments(i).comment}, i * 2, -1, -1, 3).Tag = "topcomment"

                End If
                documentDetailsView.uxcommentlist.Nodes(documentDetailsView.uxcommentlist.Nodes.Count - 1).StateImageIndex = 12
                documentDetailsView.uxcommentlist.Nodes(documentDetailsView.uxcommentlist.Nodes.Count - 1).Nodes(0).StateImageIndex = 5


            Next
            documentDetailsView.uxcommentlist.EndUnboundLoad()
            If comments.Count > 0 Then
                documentDetailsView.uxcommentlist.Nodes(0).Expanded = True

            End If




        Catch ex As Exception
            Dim db_err As New bc_cs_error_log(className, moduleName, bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Private Sub load_document_links(ByVal links As List(Of bc_om_document.bc_om_document_link))

        Dim moduleName As String = "load_document_links"
        Dim slog As New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim nbi As NavBarItem
            Me.documentDetailsView.NavBarGroup2.ItemLinks.Clear()
            Me.documentDetailsView.NavBarGroup2.Caption = "Links"

            For i = 0 To localDocument.links.Count - 1
                nbi = New NavBarItem(links(i).display_name)
                nbi.SmallImage = Me.documentDetailsView.uxDocumentDetailImages.Images(8)
                nbi.Tag = i
                Me.documentDetailsView.NavBarGroup2.ItemLinks.Add(nbi)
                AddHandler nbi.LinkClicked, AddressOf Me.documentDetailsView.linkAction
            Next
            Me.documentDetailsView.NavBarGroup2.Expanded = True

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log(className, moduleName, bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    'Private Sub load_permissions_text(ByVal doc As bc_om_document)
    '    Dim moduleName As String = "load_permissions_text"
    '    Dim slog As New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_ENTRY, "")
    '    Try

    '        Dim nbi As NavBarItem
    '        Me.documentDetailsView.NavBarGroup4.ItemLinks.Clear()
    '        Me.documentDetailsView.NavBarGroup4.Caption = "Permissions"

    '        If doc.read = True Then
    '            nbi = New NavBarItem("Read")
    '            nbi.SmallImage = Me.documentDetailsView.uxDocumentDetailImages.Images(11)
    '            nbi.Tag = 0
    '            Me.documentDetailsView.NavBarGroup4.ItemLinks.Add(nbi)
    '        End If
    '        If doc.write = True Then
    '            nbi = New NavBarItem("Write")
    '            nbi.SmallImage = Me.documentDetailsView.uxDocumentDetailImages.Images(13)
    '            nbi.Tag = 0
    '            Me.documentDetailsView.NavBarGroup4.ItemLinks.Add(nbi)
    '        End If

    '        If doc.move = True Then
    '            nbi = New NavBarItem("Move")
    '            nbi.SmallImage = Me.documentDetailsView.uxDocumentDetailImages.Images(5)

    '            nbi.Tag = 0
    '            Me.documentDetailsView.NavBarGroup4.ItemLinks.Add(nbi)
    '        End If

    '        If doc.force_check_in = True Then
    '            nbi = New NavBarItem("Force Check In")
    '            nbi.SmallImage = Me.documentDetailsView.uxDocumentDetailImages.Images(2)

    '            nbi.Tag = 0
    '            Me.documentDetailsView.NavBarGroup4.ItemLinks.Add(nbi)
    '        End If
    '        If doc.locked = True And doc.write = True Then
    '            nbi = New NavBarItem("Locked")
    '            nbi.SmallImage = Me.documentDetailsView.uxDocumentDetailImages.Images(10)

    '            nbi.Tag = 0
    '            Me.documentDetailsView.NavBarGroup4.ItemLinks.Add(nbi)
    '        End If

    '        Me.documentDetailsView.NavBarGroup4.Expanded = True


    '    Catch ex As Exception
    '        Dim db_err As New bc_cs_error_log(className, moduleName, bc_cs_error_codes.USER_DEFINED, ex.Message)
    '    Finally
    '        slog = New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_EXIT, "")
    '    End Try

    'End Sub
    Private Sub load_document_history(ByVal history As List(Of bc_om_history))
        Dim moduleName As String = "load_document_history"
        Dim slog As New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            documentDetailsView.uxhistorylist.BeginUnboundLoad()
            documentDetailsView.uxhistorylist.Nodes.Clear()

            For i = 0 To history.Count - 1
                Dim tln As Nodes.TreeListNode = Nothing
                documentDetailsView.uxhistorylist.AppendNode(New Object() {Format(history(i).da.ToLocalTime, "dd-MMM-yyyy HH:mm")}, tln).Tag = Format("dd-MMM-yyyy HH:mm", history(i).da.ToLocalTime)
                'documentDetailsView.uxhistorylist.AppendNode(New Object() {history(i).desc}, documentDetailsView.uxhistorylist.Nodes(i)).Tag = history(i).desc
                'documentDetailsView.uxhistorylist.Nodes(documentDetailsView.uxhistorylist.Nodes.Count - 1).StateImageIndex = 7

                documentDetailsView.uxhistorylist.Nodes(i).SetValue(1, history(i).desc)
                documentDetailsView.uxhistorylist.Nodes(i).SetValue(2, history(i).stage)
                documentDetailsView.uxhistorylist.Nodes(i).SetValue(3, history(i).user)
                documentDetailsView.uxhistorylist.Nodes(i).SetValue(4, history(i).code)


            Next
            documentDetailsView.uxhistorylist.EndUnboundLoad()


        Catch ex As Exception
            Dim db_err As New bc_cs_error_log(className, moduleName, bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Private Sub load_document_revisions(ByVal history As List(Of bc_om_history))
        Dim moduleName As String = "load_document_revisions"
        Dim slog As New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim image As Bitmap
            Dim imageindex As Integer = 1

            documentDetailsView.lvwrevisions.BeginUnboundLoad()
            documentDetailsView.lvwrevisions.Nodes.Clear()
            Dim ext As String
            Dim tx, st, fn As String
            For i = 0 To history.Count - 1
                If history(i).code = "Revision Taken" Then
                    tx = Replace(history(i).desc, "Revision taken stage: ", "")
                    st = Left(tx, InStr(tx, "file:") - 2)
                    fn = Replace(tx, st + " file: ", "")
                    ext = fn.Substring(InStrRev(fn, ".") - 1, Len(fn) - InStrRev(fn, ".") + 1)
                    image = Nothing
                    Dim bcs As New bc_cs_icon_services
                    image = bcs.get_icon_for_file_type(Replace(ext, "[imp]", ""))
                    If Not IsNothing(image) Then
                        Me.documentDetailsView.uxDocumentDetailImages.Images.Add(image)
                        imageindex = Me.documentDetailsView.uxDocumentDetailImages.Images.Count - 1
                    End If

                    Dim tln As Nodes.TreeListNode = Nothing
                    documentDetailsView.lvwrevisions.AppendNode(New Object() {Format(history(i).da.ToLocalTime, "dd-MMM-yyyy HH:mm")}, tln).Tag = fn + "<" + history(i).stage + ">"
                    documentDetailsView.lvwrevisions.AppendNode(New Object() {fn}, documentDetailsView.lvwrevisions.Nodes(documentDetailsView.lvwrevisions.Nodes.Count - 1)).Tag = fn + "<" + history(i).stage + ">"
                    documentDetailsView.lvwrevisions.Nodes(documentDetailsView.lvwrevisions.Nodes.Count - 1).StateImageIndex = imageindex
                    documentDetailsView.lvwrevisions.Nodes(documentDetailsView.lvwrevisions.Nodes.Count - 1).SetValue(1, history(i).stage)
                    documentDetailsView.lvwrevisions.Nodes(documentDetailsView.lvwrevisions.Nodes.Count - 1).SetValue(2, history(i).user)

                End If
            Next
            documentDetailsView.lvwrevisions.EndUnboundLoad()


        Catch ex As Exception
            Dim db_err As New bc_cs_error_log(className, moduleName, bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Private sloading As Boolean
    Private Sub load_support_docs(ByVal documents As List(Of bc_om_document))

        sloading = True
        Dim moduleName As String = "load_support_docs"
        Dim slog As New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            documentDetailsView.uxsupportdocs.BeginUnboundLoad()
            documentDetailsView.uxsupportdocs.Nodes.Clear()
            Dim image As Bitmap
            Dim imageindex As Integer = 1
            Dim user_found As Boolean
            Dim user_tx As String
            user_tx = "Unknown"
            Dim icon As Int16 = 3
            For i = 0 To documents.Count - 1
                user_found = False
                image = Nothing
                Dim bcs As New bc_cs_icon_services
                image = bcs.get_icon_for_file_type(Replace(documents(i).extension, "[imp]", ""))
                If Not IsNothing(image) Then
                    Me.documentDetailsView.uxDocumentDetailImages.Images.Add(image)
                    imageindex = Me.documentDetailsView.uxDocumentDetailImages.Images.Count - 1
                End If
                Dim tln As Nodes.TreeListNode = Nothing

                If documents(i).checked_out_user <> 0 Then
                    For j = 0 To bc_am_load_objects.obc_users.user.Count - 1
                        If bc_am_load_objects.obc_users.user(j).id = documents(i).checked_out_user Then
                            user_found = True
                            If documents(i).checked_out_user = bc_cs_central_settings.logged_on_user_id Then
                                icon = 4
                                user_tx = "me"
                            Else
                                icon = 3
                                user_tx = bc_am_load_objects.obc_users.user(j).first_name + " " + bc_am_load_objects.obc_users.user(j).surname
                            End If
                            Exit For
                        End If
                    Next

                Else
                    icon = imageindex
                    user_tx = "checked in"
                End If
                documentDetailsView.uxsupportdocs.AppendNode(New Object() {Format(documents(i).doc_date.ToLocalTime, "dd-MMM-yyyy HH:mm")}, tln).Tag = CStr(documents(i).id)
                documentDetailsView.uxsupportdocs.Nodes(i).SetValue(1, documents(i).title)
                documentDetailsView.uxsupportdocs.Nodes(i).SetValue(2, documents(i).pub_type_name)
                documentDetailsView.uxsupportdocs.Nodes(i).SetValue(3, user_tx)
                documentDetailsView.uxsupportdocs.Nodes(documentDetailsView.uxsupportdocs.Nodes.Count - 1).StateImageIndex = icon

            Next
            documentDetailsView.uxsupportdocs.EndUnboundLoad()

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log(className, moduleName, bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

            slog = New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub

    Public Sub delete_supporting_document(ByVal tag As String)

        REM Steve Wooderson 24/10/2013 Delete support doc

        Dim moduleName As String = "delete_supporting_document"
        Dim slog As New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim sdoc As New bc_om_document

        Try

            Dim omsg As New bc_cs_message("Blue Curve", "The support document will be deleted." + Chr(13) + "Are you sure you wish to proceed?", bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
            If omsg.cancel_selected = True Then
                Exit Sub
            End If

            REM firstly check master document status hasnt changed
            If localDocument.checked_out_user <> bc_cs_central_settings.logged_on_user_id Then
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    localDocument.db_check_document_state()
                ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                    localDocument.tmode = bc_cs_soap_base_class.tREAD
                    localDocument.read_mode = bc_om_document.CHECK_DOCUMENT_STATE
                    If localDocument.transmit_to_server_and_receive(localDocument, True) = False Then
                        Exit Sub

                    End If
                End If
                If check_document_state(localDocument.id, True, True) = False Then
                    Me.RetrieveDocs()

                    Exit Sub
                End If
            End If


            REM get support doc
            Dim i As Integer
            For i = 0 To localDocument.support_documents.Count - 1
                If localDocument.support_documents(i).id = tag Then
                    sdoc = New bc_om_document
                    sdoc = localDocument.support_documents(i)
                    Exit For
                End If
            Next

            sdoc.mode = 14
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                sdoc.db_write(Nothing)
            Else
                sdoc.tmode = bc_cs_soap_base_class.tWRITE
                If sdoc.transmit_to_server_and_receive(sdoc, True) = False Then
                    Exit Sub
                End If
            End If
            If sdoc.doc_write_success = False Then
                omsg = New bc_cs_message("Blue Curve", sdoc.doc_write_error_text, bc_cs_message.MESSAGE, False, False, "Yes", "no", True)
            End If
            REM refresh support documents
            Me.RetrieveDocs(True, True)

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log(className, moduleName, bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Public Sub changesupportcategory(ByVal tag As String)
        Dim moduleName As String = "changesupportcategory"
        Dim slog As New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim sdoc As bc_om_document

            sdoc = New bc_om_document

            REM make sure document is chcjed in 


            REM get support doc
            Dim i As Integer
            For i = 0 To localDocument.support_documents.Count - 1
                If localDocument.support_documents(i).id = tag Then

                    sdoc = New bc_om_document
                    sdoc = localDocument.support_documents(i)
                    Exit For
                End If
            Next
            Dim tdoc_id, ldoc_id As Long
            tdoc_id = sdoc.id
            ldoc_id = localDocument.id
            REM firstly check master document status hasnt changed
            If localDocument.checked_out_user <> bc_cs_central_settings.logged_on_user_id Then
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    localDocument.db_check_document_state()
                ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                    localDocument.tmode = bc_cs_soap_base_class.tREAD
                    localDocument.read_mode = bc_om_document.CHECK_DOCUMENT_STATE
                    If localDocument.transmit_to_server_and_receive(localDocument, True) = False Then
                        Exit Sub

                    End If
                End If
                If check_document_state(localDocument.id, True, True) = False Then
                    Me.RetrieveDocs()

                    Exit Sub
                End If
            End If
            localDocument.id = ldoc_id

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                sdoc.db_read_for_categorize()
            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                sdoc.tmode = bc_cs_soap_base_class.tREAD
                sdoc.read_mode = bc_om_document.READ_FOR_CATEGORIZE
                If sdoc.transmit_to_server_and_receive(sdoc, True) = False Then
                    Exit Sub
                End If
            End If

            If check_document_state(sdoc.id, True) = False Then
                Me.RetrieveDocs()
                Exit Sub
            End If
            sdoc.id = tdoc_id

            REM
            sdoc.doc_date = sdoc.doc_date.ToLocalTime
            Dim osubmit As New bc_dx_as_categorize
            osubmit.support_doc_cat = True
            osubmit.attachment = True
            sdoc.doc_date = sdoc.doc_date.ToLocalTime
            osubmit.show_stage_change = False
            osubmit.show_local_submit = False

            osubmit.enable_pub_types = True
            osubmit.enable_lead_entity = False
            osubmit.document = sdoc
            osubmit.caption = "Blue Curve Process - Update Support Document Metadata: " + sdoc.title

            osubmit.ok_button_caption = "Update"

            Dim pts As New List(Of String)
            For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                pts.Add(bc_am_load_objects.obc_pub_types.pubtype(i).name)
            Next
            osubmit.set_pub_types = pts

            Cursor.Current = Cursors.WaitCursor
            osubmit.master_pub_type_id = sdoc.pub_type_id

            osubmit.Focus()
            osubmit.ShowDialog()
            If osubmit.ok_selected = False Then
                REM if cancel selected do nothing
                Dim ocommentary As New bc_cs_activity_log("bc_am_workflow", "import_master_document", bc_cs_activity_codes.COMMENTARY, "Cancel Selected on Submit form.")
                Exit Sub
            End If

            sdoc.bwith_document = False

            sdoc.support_documents.Clear()

            sdoc.register_only = False
            sdoc.bcheck_out = False
            sdoc.bwith_document = False
            sdoc.history.Clear()
            sdoc.bimport_support_only = True


            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                sdoc.db_write(Nothing)
            Else
                sdoc.tmode = bc_cs_soap_base_class.tWRITE
                sdoc.transmit_to_server_and_receive(sdoc, True)
            End If
            If sdoc.doc_write_error_text <> "" Then
                Dim omsg As New bc_cs_message("Blue Curve", "Change Support Document Categorisation Failed: " + sdoc.doc_write_error_text, bc_cs_message.MESSAGE, False, False, "yes", "no", True)
            End If
            Me.RetrieveDocs(True, True)





        Catch ex As Exception
            Dim db_err As New bc_cs_error_log(className, moduleName, bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub


    Public Sub highlightCurrent()
        Dim moduleName As String = "highlightCurrent"
        Dim slog As New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            REM find current in node tree
            Dim i As Integer
            checkNode(documentDetailsView.uxWorkflowTree.Nodes, i)

            documentDetailsView.uxWorkflowTree.Nodes(0).ExpandAll()

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log(className, moduleName, bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub

    Public Function checkNode(ByVal onode As Nodes.TreeListNodes, ByRef selnode As Integer) As Integer
        Dim moduleName As String = "checkNode"
        Dim slog As New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim i, j As Integer
            For i = 0 To onode.Count - 1
                If onode(i).Item(0) = localDocument.stage_name Then
                    onode(i).StateImageIndex = 1 ' needs to be the correct value
                    onode(i).ExpandAll()
                    documentDetailsView.uxWorkflowTree.SetFocusedNode(onode(i))
                End If

                With localDocument.pub_type_workflow
                    For j = 0 To .next_stages.Count - 1
                        If onode(i).Item(0) = .next_stages(j) Then
                            onode(i).StateImageIndex = 24 ' needs to be the correct value
                            onode(i).ExpandAll()
                        End If
                    Next
                End With
                checkNode(onode(i).Nodes, selnode)
            Next
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log(className, moduleName, bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function

    Private Sub populateWorkflowTree()
        Dim moduleName As String = "populateWorkflowTree"
        Dim slog As New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim j As Integer
        Try
            documentDetailsView.uxWorkflowTree.BeginUnboundLoad()
            documentDetailsView.uxWorkflowTree.Nodes.Clear()

            With localDocument.pub_type_workflow.nstage
                Dim tln As Nodes.TreeListNode = Nothing



                documentDetailsView.uxWorkflowTree.AppendNode(New Object() {.current_stage_name}, tln).Tag = .current_stage
                'LS DEV EXPRESS COMMENTED
                'documentDetailsView.uxWorkflowTree.Nodes(0).StateImageIndex = 1 ' needs to be the correct index
                If .current_stage_name = localDocument.stage_name Then
                    For j = 0 To .routes.Count - 1
                        .routes(j).next_stage = True
                    Next
                    documentDetailsView.uxWorkflowTree.Nodes(0).Expanded = True
                End If
                addNode(documentDetailsView.uxWorkflowTree.Nodes(0), .routes)
            End With

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log(className, moduleName, bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            documentDetailsView.uxWorkflowTree.EndUnboundLoad()
            slog = New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub

    Public Sub addNode(ByVal start_node As Nodes.TreeListNode, ByVal lstage As List(Of bc_om_pub_type_workflow_stage))
        Dim moduleName As String = "addNode"
        Dim slog As New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim i As Integer
        Dim ncount As Integer
        ncount = 0

        Try
            If Not lstage Is Nothing Then
                For i = 0 To lstage.Count - 1
                    If lstage(i).duplicate = False Then
                        'If lstage(i).next_stage = True Then
                        documentDetailsView.uxWorkflowTree.AppendNode(New Object() {lstage(i).current_stage_name}, start_node).Tag = lstage(i).current_stage
                        'End If
                        'If lstage(i).next_stage = True Then
                        '    start_node.Nodes(ncount).StateImageIndex = 24
                        'Else
                        '    start_node.Nodes(ncount).StateImageIndex = -1
                        'End If
                        addNode(start_node.Nodes(ncount), lstage(i).routes)
                        ncount = ncount + 1
                    End If
                Next
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log(className, moduleName, bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub

    'Private Sub evaluateStageChange(ByVal index As Integer)
    '    Dim moduleName As String = "stageChange"
    '    Dim slog As New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_ENTRY, "")

    '    Dim i As Integer
    '    Dim tpoll As Boolean
    '    Dim ta As Integer
    '    Dim na As Integer
    '    Try
    '        tpoll = pollingEnabled
    '        pollingEnabled = False


    '        If localDocument.workflow_stages.stages(index).num_approvers > 1 Then
    '            Dim adoc As New bc_om_document
    '            'adoc.approval_only = True
    '            adoc.approved_by = bc_cs_central_settings.logged_on_user_id
    '            adoc.original_stage = localDocument.stage
    '            adoc.stage = localDocument.workflow_stages.stages(index).stage_id
    '            adoc.stage_name = localDocument.workflow_stages.stages(index).stage_name
    '            adoc.id = localDocument.id
    '            ta = localDocument.workflow_stages.stages(index).num_approvers
    '            na = localDocument.workflow_stages.stages(index).approved_by.count
    '            REM check current user hasnt already aspproved document
    '            For i = 0 To localDocument.workflow_stages.stages(index).approved_by.count - 1
    '                If CStr(bc_cs_central_settings.logged_on_user_id) = CStr(localDocument.workflow_stages.stages(index).approved_by(i)) Then
    '                    Dim omessage As New bc_cs_message("Blue Curve - Process", "You have already approved this document to this stage!" + CStr(ta - na) + " other approver(s) still required.", bc_cs_message.MESSAGE)
    '                    Exit Try
    '                End If
    '            Next
    '            If localDocument.workflow_stages.stages(index).num_approvers > localDocument.workflow_stages.stages(index).approved_by.count + 1 Then
    '                Dim omessage As New bc_cs_message("Blue Curve - Process", CStr(localDocument.workflow_stages.stages(index).approved_by.count + 1) + " of " + CStr(localDocument.workflow_stages.stages(index).num_approvers) + " required approvers have approved this document so document can't change stage.", bc_cs_message.MESSAGE)
    '                approveDoc(adoc)
    '                Exit Try
    '            End If
    '        End If
    '        'Dim ofrmcomment As New bc_am_workflow_params
    '        'ofrmcomment.ldoc = localDocument
    '        'ofrmcomment.ShowDialog()


    '        Dim ofrmcomment As New bc_am_stage_change
    '        ofrmcomment.Ltitle = "Comment for users in stage " + localDocument.workflow_stages.stages(index).stage_name

    '        ofrmcomment.ShowDialog()
    '        If ofrmcomment.cancel_selected = True Then
    '            Exit Sub
    '        End If
    '        If ofrmcomment.uxComment.Text <> "" Then
    '            localDocument.main_note = ofrmcomment.uxComment.Text
    '            If ofrmcomment.Chkcompletion.Checked = True Then
    '                Dim da As DateTime
    '                da = ofrmcomment.DateEdit1.DateTime
    '                MsgBox(da)
    '                da.AddHours(CInt(ofrmcomment.Lhr.Text))
    '                da.AddMinutes(CInt(ofrmcomment.Lmin.Text))
    '                localDocument.stage_expire_date = da.ToUniversalTime
    '                MsgBox(localDocument.stage_expire_date)
    '            End If
    '        End If
    '        REM hold original stage
    '        localDocument.original_stage = localDocument.stage
    '        localDocument.original_stage_name = localDocument.stage_name
    '        localDocument.stage = localDocument.workflow_stages.stages(index).stage_id
    '        localDocument.stage_name = localDocument.workflow_stages.stages(index).stage_name
    '        localDocument.action_Ids.Clear()
    '        localDocument.action_Ids = localDocument.workflow_stages.stages(index).action_ids
    '        newStage = localDocument.stage
    '        newStageName = localDocument.stage_name
    '        REM reject 
    '        If localDocument.reject = True Then
    '            localDocument.reset_reject = True
    '        End If
    '        localDocument.set_reject = False
    '        If localDocument.workflow_stages.stages(index).stage_type = "R" Then
    '            newStage = "Creating"
    '            localDocument.stage = 1
    '            localDocument.stage_name = "Creating"
    '            localDocument.reject_stage = localDocument.original_stage
    '            localDocument.reject_stage_name = localDocument.original_stage_name
    '            localDocument.set_reject = True
    '            localDocument.reset_reject = False
    '            changeStage(False, 1, "Creating", localDocument)
    '        Else
    '            localDocument.reset_reject = True
    '            changeStage(False, localDocument.workflow_stages.stages(index).stage_id, localDocument.workflow_stages.stages(index).stage_name, localDocument)
    '        End If
    '    Catch ex As Exception
    '        Dim db_err As New bc_cs_error_log(className, moduleName, bc_cs_error_codes.USER_DEFINED, ex.Message)
    '    Finally
    '        pollingEnabled = tpoll
    '        slog = New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_EXIT, "")

    '    End Try
    'End Sub

    Private Sub changeStage(ByVal with_document As Boolean, ByVal stage As Long, ByVal stage_name As String, ByVal ldoc As bc_om_document)
        Dim moduleName As String = "changeStage"
        Dim slog As New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim csi As Boolean = True

        Try


            Cursor.Current = Cursors.WaitCursor

            Dim doc_open As Boolean = False
            Dim weh As bc_am_workflow_events_handler = Nothing
            REM before stage change check out doc and
            REM make sure it hasnt recently befcome checked out elsewhere or
            REM changed stage
            REM TBD

            ldoc.bcheck_out = False
            ldoc.bwith_document = False
            ldoc.btake_revision = False
            ldoc.master_flag = True

            Dim tstage_id As Long
            Dim tstage_name As String
            tstage_id = ldoc.stage
            tstage_name = ldoc.stage_name

            ldoc.stage = ldoc.original_stage
            Dim lcomment As String
            Dim lexpire As DateTime
            lcomment = ldoc.main_note
            lexpire = ldoc.stage_expire_date



            ldoc.stage = tstage_id
            ldoc.stage_name = tstage_name
            ldoc.main_note = lcomment
            ldoc.stage_expire_date = lexpire

            Dim found As Boolean = False
            If tstage_name = "Publish" Then
                For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                    If bc_am_load_objects.obc_pub_types.pubtype(i).id = ldoc.pub_type_id Then
                        If bc_am_load_objects.obc_pub_types.pubtype(i).mandatory_default_support_doc = True Then
                            For j = 0 To ldoc.support_documents.Count - 1
                                If ldoc.support_documents(j).pub_type_id = bc_am_load_objects.obc_pub_types.pubtype(i).default_support_pub_type Then
                                    found = True
                                    Exit For
                                End If
                            Next
                            If found = False Then
                                Dim omsg As New bc_cs_message
                                For j = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                                    If bc_am_load_objects.obc_pub_types.pubtype(j).id = bc_am_load_objects.obc_pub_types.pubtype(i).default_support_pub_type Then
                                        Dim omsgg As New bc_cs_message("Blue Curve", "Support Document: " + bc_am_load_objects.obc_pub_types.pubtype(j).name + " not attached prior to Publish. Do you wish to Continue?", bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
                                        If omsgg.cancel_selected = True Then
                                            Exit Sub
                                        End If
                                        Exit For
                                    End If
                                Next
                            End If
                        End If
                    End If
                Next
            End If


            REM need to add actions to this soon
            ldoc.history.Clear()
            ldoc.support_documents.Clear()

            If ldoc.action_Ids.Count > 0 Then
                REM if registered only doc stop stage change here
                If ldoc.extension = "" Then
                    ldoc.server_side_events.events.Clear()
                    ldoc.stage = ldoc.original_stage
                    ldoc.stage_name = ldoc.original_stage_name
                    Dim omessage As New bc_cs_message("Blue Curve - Workflow", "Transitional Events Failed as document is only registered; document will not change Stage.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    ldoc.action_Ids.Clear()
                    csi = False
                Else
                    REM actions required prior to stage change
                    weh = New bc_am_workflow_events_handler(Nothing, ldoc, True)
                    weh.doc_open = False
                    weh.stage_id_to = ldoc.stage
                    weh.stage_name_to = ldoc.stage_name
                    REM set these so open doenst detect a stage change
                    ldoc.stage = ldoc.original_stage
                    ldoc.stage_name = ldoc.original_stage_name
                    weh.run_events(ldoc, False)
                    If weh.success = True Then
                        ldoc.stage = weh.stage_id_to
                        ldoc.stage_name = weh.stage_name_to
                        REM if document was opened read save it
                        REM resave xml and doc
                        If weh.doc_open = True Then
                            ldoc.stage = weh.stage_id_to
                            ldoc.stage_name = weh.stage_name_to
                            ldoc.write_data_to_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ".dat")
                            ldoc.docobject = weh.obj
                            ldoc.docobject.save()
                        End If
                    Else
                        REM remove server side events
                        ldoc.server_side_events.events.Clear()
                        ldoc.stage = ldoc.original_stage
                        ldoc.stage_name = ldoc.original_stage_name
                        If weh.suppress_no_trans = False And weh.suppress_failure_message = False Then
                            Dim etx As String
                            etx = "Transitional Events Failed so document will not change Stage." + vbCrLf
                            If ldoc.history.Count > 0 Then
                                etx = etx + ldoc.history(ldoc.history.Count - 1).comment
                            End If
                            Dim omessage As New bc_cs_message("Blue Curve - Workflow", etx, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                        End If
                        doc_open = False
                    End If
                End If
            End If
            Try
                bc_cs_central_settings.progress_bar.unload()
            Catch

            End Try


            If ldoc.action_Ids.Count > 0 Then

                If weh.doc_open = False Or weh.success = False Then
                    ldoc.component_componetize = False
                    ldoc.register_only = True
                    If weh.success = False Then
                        csi = False
                        Dim oh As New bc_om_history
                        oh.comment = "Force Check in as client side event failed"
                        ldoc.history.Add(oh)
                    End If
                    ldoc.write_history = True
                    ldoc.comments.comments.Clear()
                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then


                        ldoc.db_write(Nothing)
                    ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                        ldoc.tmode = bc_cs_soap_base_class.tWRITE
                        If ldoc.transmit_to_server_and_receive(ldoc, True) = False Then
                            Exit Sub
                        End If
                    End If
                    If ldoc.doc_write_success = False Then
                        Dim omsg As New bc_cs_message("Blue Curve", "Failed to Change Stage: " + ldoc.doc_write_error_text, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                        Exit Sub
                    End If

                    REM delete xml if exists
                    Dim fs As New bc_cs_file_transfer_services
                    If weh.doc_open = True Then
                        weh.close()
                        If fs.check_document_exists(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ".dat") = True Then
                            fs.delete_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ".dat")
                        End If
                        If fs.check_document_exists(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + Replace(ldoc.extension, "[imp]", "")) = True Then
                            fs.delete_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + Replace(ldoc.extension, "[imp]", ""))
                        End If
                    End If

                Else
                    ldoc.main_note = lcomment
                    ldoc.stage_expire_date = lexpire

                    ldoc.write_data_to_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ".dat")
                    REM events opened document so full submission is required
                    REM only call full submit on BC document


                    Dim osubmit As New bc_am_at_submit

                    If InStr(ldoc.extension, "xls") > 0 Then
                        osubmit.submit_after_stage_change(ldoc, ldoc.docobject, "excel", False, True, False)
                    ElseIf ldoc.extension = ".ppt" Or ldoc.extension = ".pptx" Or ldoc.extension = ".pptm" Then
                        osubmit.submit_after_stage_change(ldoc, ldoc.docobject, "powerpoint", False, True, False)
                    Else
                        osubmit.submit_after_stage_change(ldoc, ldoc.docobject, "word", False, True, False)
                    End If

                End If
            Else
                ldoc.register_only = True
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    ldoc.db_write(Nothing)
                ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                    ldoc.tmode = bc_cs_soap_base_class.tWRITE
                    If ldoc.transmit_to_server_and_receive(ldoc, True) = False Then
                        Exit Sub
                    End If
                End If
                If ldoc.doc_write_success = False Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Failed to Change Stage: " + ldoc.doc_write_error_text, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub
                End If


            End If
            If ldoc.server_side_events_failed <> "" Then
                Dim omsg As New bc_cs_message("Blue Curve", ldoc.server_side_events_failed, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log(className, moduleName, bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            If ldoc.doc_write_success = True And ldoc.server_side_events_failed = "" And csi = True Then
                Dim omsg As New bc_cs_message("Blue Curve", "Stage Change Successful.  You might not have access to the document anymore.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            End If
            Me.RetrieveDocs(True, True)
            Try
                bc_cs_central_settings.progress_bar.hide()
            Catch

            End Try

            Cursor.Current = Cursors.Default
            slog = New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Friend Sub toggleshow()

        If containerView.uxShowDetails.Checked = False Then
            If containerView.uxSplitContainer.IsPanelCollapsed Then
                containerView.uxSplitContainer.Collapsed = False
            End If
            containerView.uxShowDetails.Caption = "Hide"
        Else
            If Not containerView.uxSplitContainer.IsPanelCollapsed Then
                containerView.uxSplitContainer.Collapsed = True
            End If
            containerView.uxShowDetails.Caption = "Show"
        End If
    End Sub



    Friend Sub stageSelect()
        Try

            REM first check of node is change stage node
            Dim st As String
            Dim i, j As Integer
            Dim ta As Integer
            Dim na As Integer

            If workflowEnabled = False Then
                Dim omessage As New bc_cs_message("Blue Curve - Process", "You cannot change stage of document: " + gcomment, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Sub
            End If

            Dim tid As Long
            tid = localDocument.id
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                localDocument.db_check_document_state()
            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                localDocument.tmode = bc_cs_soap_base_class.tREAD
                localDocument.read_mode = bc_om_document.CHECK_DOCUMENT_STATE
                If localDocument.transmit_to_server_and_receive(localDocument, True) = False Then
                    Exit Sub
                End If
            End If


            If check_document_state(localDocument.id, True) = False Then
                localDocument.id = tid
                RetrieveDocs()
                Exit Sub
            End If
            localDocument.id = tid


            st = documentDetailsView.uxWorkflowTree.Selection(0).GetDisplayText(0)
            For i = 0 To localDocument.workflow_stages.stages.Count - 1
                If localDocument.workflow_stages.stages(i).stage_name = st And localDocument.stage_name <> st Then
                    Dim smandatory_fields As String
                    Dim file_attach As Boolean = True
                    smandatory_fields = localDocument.check_mandatory_fields(bc_am_load_objects.obc_entities, bc_am_load_objects.obc_pub_types, file_attach)
                    If smandatory_fields <> "" Then
                        Dim omessage As New bc_cs_message("Blue Curve", smandatory_fields, bc_cs_message.MESSAGE)
                        Exit Sub
                    End If
                    If file_attach = False Then
                        Dim omessage As New bc_cs_message("Blue Curve", "File must be attached!", bc_cs_message.MESSAGE)
                        Exit Sub
                    End If

                    Dim adoc As New bc_om_document
                    adoc.stage_name = localDocument.workflow_stages.stages(i).stage_name
                    adoc.approved_by = bc_cs_central_settings.logged_on_user_id
                    adoc.original_stage = localDocument.stage
                    adoc.stage = localDocument.workflow_stages.stages(i).stage_id
                    adoc.stage_name = localDocument.workflow_stages.stages(i).stage_name
                    adoc.id = localDocument.id
                    ta = localDocument.workflow_stages.stages(i).num_approvers
                    na = localDocument.workflow_stages.stages(i).approved_by.count

                    If ta > 0 Then
                        REM check if already approved
                        For j = 0 To localDocument.workflow_stages.stages(i).approved_by.count - 1
                            If CStr(bc_cs_central_settings.logged_on_user_id) = CStr(localDocument.workflow_stages.stages(i).approved_by(j)) Then
                                Dim omessage As New bc_cs_message("Blue Curve - Process", "You have already approved this document to this stage! " + CStr(ta - na) + " other approver(s) still required.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                                Exit Sub
                            End If
                        Next
                        REM check if author can approve
                        Dim allow_approval As Boolean = False
                        Dim approver_role_found As Boolean = False
                        REM author can approve and no specific role
                        If localDocument.workflow_stages.stages(i).author_approval <> 1 And localDocument.workflow_stages.stages(i).approval_role <= 0 Then
                            allow_approval = True
                        ElseIf localDocument.workflow_stages.stages(i).author_approval = 1 And localDocument.originating_author = bc_cs_central_settings.logged_on_user_id Then
                            REM author cannot approve his own document
                            Dim omessage As New bc_cs_message("Blue Curve - Process", "This user is the author, and is unable to approve this report. " + CStr(ta - na) + " other approver(s) still required.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                            Exit Sub
                            REM PR JAN 2018 
                            allow_approval = True
                            REM check if user is in approver role
                        ElseIf localDocument.workflow_stages.stages(i).approval_role > 0 Then
                            For j = 0 To bc_am_load_objects.obc_prefs.secondary_roles.Count - 1
                                If bc_am_load_objects.obc_prefs.secondary_roles(j) = localDocument.workflow_stages.stages(i).approval_role Then
                                    Dim omsg As New bc_cs_message("Blue Curve - Process", "You are authorized to approve this report do you wish to approve?", bc_cs_message.MESSAGE, True, False, "Approve", "Cancel", True)
                                    If omsg.cancel_selected = False Then
                                        allow_approval = True
                                    Else
                                        Exit Sub
                                    End If
                                    approver_role_found = True
                                    Exit For
                                End If
                            Next
                            If approver_role_found = False Then
                                Dim omessage As New bc_cs_message("Blue Curve - Process", "This user is not setup as an approver, and is unable to approve this report.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)

                                Exit Sub
                            End If
                        End If

                        If allow_approval = True Then
                            If localDocument.workflow_stages.stages(i).num_approvers > localDocument.workflow_stages.stages(i).approved_by.count + 1 Then
                                Dim omessage As New bc_cs_message("Blue Curve - Process", CStr(localDocument.workflow_stages.stages(i).approved_by.count + 1) + " of " + CStr(localDocument.workflow_stages.stages(i).num_approvers) + " required approvers have approved this document so document can't change stage.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                                approveDoc(adoc)
                                Exit Sub
                            End If
                        End If

                    End If


                    Dim sta As New bc_om_next_stage_users
                    sta.doc_id = localDocument.id
                    sta.pub_type_id = localDocument.pub_type_id



                    sta.next_stage = localDocument.workflow_stages.stages(i).stage_id




                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        sta.db_read()
                    Else
                        sta.tmode = bc_cs_soap_base_class.tREAD
                        If sta.transmit_to_server_and_receive(sta, True) = False Then
                            Exit Sub
                        End If
                    End If

                    Dim ofrmcomment As New bc_am_stage_change
                    If sta.users.Count > 0 Then
                        ofrmcomment.lnextuser.Visible = True
                        ofrmcomment.uxnextuser.Visible = True
                        For j = 0 To sta.users.Count - 1
                            ofrmcomment.uxnextuser.Properties.Items.Add(sta.users(j).user_name)
                        Next
                    End If



                    ofrmcomment.Ltitle.Text = "Comment for users for next stage " + localDocument.workflow_stages.stages(i).stage_name
                    For k = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                        If bc_am_load_objects.obc_pub_types.pubtype(k).id = localDocument.pub_type_id Then
                            If bc_am_load_objects.obc_pub_types.pubtype(k).hide_next_stage_date = True Then
                                ofrmcomment.Chkcompletion.Visible = False
                                ofrmcomment.DateEdit1.Visible = False
                                ofrmcomment.uxtime.Visible = False
                            End If
                            Exit For
                        End If
                    Next
                    ofrmcomment.ShowDialog()
                    If ofrmcomment.cancel_selected = True Then
                        Exit Sub
                    End If
                    If ofrmcomment.Chkcompletion.Checked = True Then
                        Dim da As DateTime
                        da = ofrmcomment.DateEdit1.DateTime.Date
                        Try
                            da = da.AddHours(ofrmcomment.uxtime.Time.Hour)
                            da = da.AddMinutes(ofrmcomment.uxtime.Time.Minute)
                            da = da.ToUniversalTime
                        Catch

                        End Try
                        localDocument.stage_expire_date = da.ToUniversalTime
                    Else
                        localDocument.stage_expire_date = "09/09/9999"
                    End If

                    If ofrmcomment.uxnextuser.SelectedIndex > -1 Then
                        localDocument.assigned_user = sta.users(ofrmcomment.uxnextuser.SelectedIndex).id

                    End If

                    REM hold original stage
                    localDocument.stage_change_comment = ofrmcomment.uxComment.Text

                    localDocument.original_stage = localDocument.stage
                    localDocument.original_stage_name = localDocument.stage_name
                    localDocument.stage = localDocument.workflow_stages.stages(i).stage_id
                    localDocument.stage_name = localDocument.workflow_stages.stages(i).stage_name
                    localDocument.action_Ids.Clear()
                    localDocument.action_Ids = localDocument.workflow_stages.stages(i).action_ids
                    newStage = localDocument.stage
                    newStageName = localDocument.stage_name

                    localDocument.reset_reject = True
                    changeStage(False, localDocument.workflow_stages.stages(i).stage_id, localDocument.workflow_stages.stages(i).stage_name, localDocument)
                    Exit For
                End If

            Next
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_process", "stageSelect", bc_cs_error_codes.USER_DEFINED, ex.Message)


        Finally

        End Try

    End Sub

    Private Sub approveDoc(ByVal adoc As bc_om_document)
        Dim moduleName As String = "approveDoc"
        Dim slog As New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            adoc.certificate.user_id = bc_cs_central_settings.logged_on_user_id
            adoc.certificate.user_id = bc_cs_central_settings.logged_on_user_name
            adoc.approve_stage = adoc.stage
            adoc.stage = adoc.original_stage


            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                adoc.db_approve_doc_only()


            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                adoc.read_mode = bc_om_document.APPROVE_DOC_ONLY
                adoc.tmode = bc_cs_soap_base_class.tREAD
                adoc.transmit_to_server_and_receive(adoc, True)
                adoc.read_mode = 0
            End If
            Me.check_document_state(adoc.id, True)
            Me.RetrieveDocs(True, True)


        Catch ex As Exception
            Dim db_err As New bc_cs_error_log(className, moduleName, bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    'Public Function get_icon_for_file_type(ByVal sfileext As String) As Bitmap
    '    Try
    '        Dim sProg As String
    '        Dim tmp As String

    '        tmp = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(sfileext).GetValue("")
    '        ' Get the program that will open files with this extension
    '        sProg = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(tmp).OpenSubKey("shell").OpenSubKey("open").OpenSubKey("command").GetValue("")

    '        ' strip the filename
    '        If sProg.Substring(0, 1) = Chr(34) Then
    '            sProg = sProg.Substring(1, sProg.IndexOf(Chr(34), 2) - 1)
    '        Else
    '            sProg = sProg.Substring(0, sProg.IndexOf(" ", 2))
    '        End If
    '        sProg = Replace(sProg, "%1", "")

    '        ' Extract the icon from the program
    '        Dim oicon As Icon

    '        oicon = System.Drawing.Icon.ExtractAssociatedIcon(sProg)
    '        get_icon_for_file_type = oicon.ToBitmap

    '    Catch
    '        get_icon_for_file_type = Nothing
    '    End Try

    'End Function


    'added to handle Office 2010 startup changes
    <ComImport(), Guid("00000016-0000-0000-C000-000000000046"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)> _
    Private Interface IOleMessageFilter
        <PreserveSig()> Function HandleInComingCall(ByVal dwCallType As Integer, ByVal hTaskCaller As IntPtr, ByVal dwTickCount As Integer, ByVal lpInterfaceInfo As IntPtr) As Integer
        <PreserveSig()> Function RetryRejectedCall(ByVal hTaskCallee As IntPtr, ByVal dwTickCount As Integer, ByVal dwRejectType As Integer) As Integer
        <PreserveSig()> Function MessagePending(ByVal hTaskCallee As IntPtr, ByVal dwTickCount As Integer, ByVal dwPendingType As Integer) As Integer
    End Interface

    Private Class MessageFilter
        Implements IOleMessageFilter
        <DllImport("Ole32.dll")> _
        Private Shared Function CoRegisterMessageFilter(ByVal newFilter As IOleMessageFilter, ByRef oldFilter As IOleMessageFilter) As Integer
        End Function
        Public Shared Sub Register()
            Dim newFilter As IOleMessageFilter = New MessageFilter()
            Dim oldFilter As IOleMessageFilter = Nothing
            CoRegisterMessageFilter(newFilter, oldFilter)
        End Sub
        Public Shared Sub Revoke()
            Dim oldFilter As IOleMessageFilter = Nothing
            CoRegisterMessageFilter(Nothing, oldFilter)
        End Sub
        Public Function HandleInComingCall(ByVal dwCallType As Integer, ByVal hTaskCaller As System.IntPtr, ByVal dwTickCount As Integer, ByVal lpInterfaceInfo As System.IntPtr) As Integer Implements IOleMessageFilter.HandleInComingCall
            Return 0
        End Function
        Public Function MessagePending(ByVal hTaskCallee As System.IntPtr, ByVal dwTickCount As Integer, ByVal dwPendingType As Integer) As Integer Implements IOleMessageFilter.MessagePending
            Return 2
        End Function
        Public Function RetryRejectedCall(ByVal hTaskCallee As System.IntPtr, ByVal dwTickCount As Integer, ByVal dwRejectType As Integer) As Integer Implements IOleMessageFilter.RetryRejectedCall
            If (dwRejectType = 2) Then
                Return 99
                'Value >=0 and <100: the call is to be retried immediately
                'Value >=100: COM will wait for this many milliseconds and then retry the call
                'Value -1: the call should be canceled. COM the Returns RPC_E_CALL_REJECTED for the original method call
            Else
                Return -1
            End If
        End Function
    End Class

    Private Class taxonomy
        Public name As String
        Public all As Boolean
        Public taxonomy_items As New ArrayList

        Public Sub New()

        End Sub
    End Class
    Private Class taxonomy_Item
        Public name As String
        Public id As String
        Public selected As Boolean
    End Class

#Region " Process Colors "

    Friend Class ProcessColors
        Public workflow_current_stage_backcolor As System.Drawing.Color
        Public workflow_current_stage_forecolor As System.Drawing.Color
        Public workflow_next_stage_backcolor As System.Drawing.Color
        Public workflow_next_stage_forecolor As System.Drawing.Color
        Public doc_list_read_backcolor As System.Drawing.Color
        Public doc_list_read_forecolor As System.Drawing.Color
        Public doc_list_unread_backcolor As System.Drawing.Color
        Public doc_list_unread_forecolor As System.Drawing.Color
        Public doc_list_search_backcolor As System.Drawing.Color
        Public doc_list_search_forecolor As System.Drawing.Color
        Public doc_list_urgent_backcolor As System.Drawing.Color
        Public doc_list_urgent_forecolor As System.Drawing.Color
        Public doc_list_expired_backcolor As System.Drawing.Color
        Public doc_list_expired_forecolor As System.Drawing.Color
        Public doc_list_stage_changed_backcolor As System.Drawing.Color
        Public doc_list_stage_changed_forecolor As System.Drawing.Color

        'Set colors to default in the constructor
        Public Sub New()
            resetDefaultColors()
        End Sub

        'Set the default color scheme    
        Public Sub resetDefaultColors()
            Me.doc_list_expired_backcolor = System.Drawing.Color.LightCoral
            Me.doc_list_expired_forecolor = System.Drawing.Color.Black
            Me.doc_list_read_backcolor = System.Drawing.Color.Black
            Me.doc_list_read_forecolor = System.Drawing.Color.White
            Me.doc_list_search_backcolor = System.Drawing.Color.LightBlue
            Me.doc_list_search_forecolor = System.Drawing.Color.Black
            Me.doc_list_stage_changed_backcolor = System.Drawing.Color.LightYellow
            Me.doc_list_stage_changed_forecolor = System.Drawing.Color.Black
            Me.doc_list_unread_backcolor = System.Drawing.Color.LightGreen
            Me.doc_list_unread_forecolor = System.Drawing.Color.Black
            Me.doc_list_urgent_backcolor = System.Drawing.Color.LightPink
            Me.doc_list_urgent_forecolor = System.Drawing.Color.Black
            Me.workflow_current_stage_backcolor = System.Drawing.Color.LightGreen
            Me.workflow_current_stage_forecolor = System.Drawing.Color.Black
            Me.workflow_next_stage_backcolor = System.Drawing.Color.LightCoral
            Me.workflow_next_stage_forecolor = System.Drawing.Color.Black

        End Sub

        Public Function convertForConfig() As Hashtable
            Dim output As New Hashtable
            output.Add("doc_list_expired_backcolor", Me.doc_list_expired_backcolor.ToArgb())
            output.Add("doc_list_expired_forecolor", Me.doc_list_expired_forecolor.ToArgb())
            output.Add("doc_list_read_backcolor", Me.doc_list_read_backcolor.ToArgb())
            output.Add("doc_list_read_forecolor", Me.doc_list_read_forecolor.ToArgb())
            output.Add("doc_list_search_backcolor", Me.doc_list_search_backcolor.ToArgb())
            output.Add("doc_list_search_forecolor", Me.doc_list_search_forecolor.ToArgb())
            output.Add("doc_list_stage_changed_backcolor", Me.doc_list_stage_changed_backcolor.ToArgb())
            output.Add("doc_list_stage_changed_forecolor", Me.doc_list_stage_changed_forecolor.ToArgb())
            output.Add("doc_list_unread_backcolor", Me.doc_list_unread_backcolor.ToArgb())
            output.Add("doc_list_unread_forecolor", Me.doc_list_unread_forecolor.ToArgb())
            output.Add("doc_list_urgent_backcolor", Me.doc_list_urgent_backcolor.ToArgb())
            output.Add("doc_list_urgent_forecolor", Me.doc_list_urgent_forecolor.ToArgb())
            output.Add("workflow_current_stage_backcolor", Me.workflow_current_stage_backcolor.ToArgb())
            output.Add("workflow_current_stage_forecolor", Me.workflow_current_stage_forecolor.ToArgb())
            output.Add("workflow_next_stage_backcolor", Me.workflow_next_stage_backcolor.ToArgb())
            output.Add("workflow_next_stage_forecolor", Me.workflow_next_stage_forecolor.ToArgb())

            convertForConfig = output
        End Function

        Public Sub loadFromConfig(ByVal savedValues As Hashtable)
            Me.doc_list_expired_backcolor = System.Drawing.Color.FromArgb(savedValues("doc_list_expired_backcolor"))
            Me.doc_list_expired_forecolor = System.Drawing.Color.FromArgb(savedValues("doc_list_expired_forecolor"))
            Me.doc_list_read_backcolor = System.Drawing.Color.FromArgb(savedValues("doc_list_read_backcolor"))
            Me.doc_list_read_forecolor = System.Drawing.Color.FromArgb(savedValues("doc_list_read_forecolor"))
            Me.doc_list_search_backcolor = System.Drawing.Color.FromArgb(savedValues("doc_list_search_backcolor"))
            Me.doc_list_search_forecolor = System.Drawing.Color.FromArgb(savedValues("doc_list_search_forecolor"))
            Me.doc_list_stage_changed_backcolor = System.Drawing.Color.FromArgb(savedValues("doc_list_stage_changed_backcolor"))
            Me.doc_list_stage_changed_forecolor = System.Drawing.Color.FromArgb(savedValues("doc_list_stage_changed_forecolor"))
            Me.doc_list_unread_backcolor = System.Drawing.Color.FromArgb(savedValues("doc_list_unread_backcolor"))
            Me.doc_list_unread_forecolor = System.Drawing.Color.FromArgb(savedValues("doc_list_unread_forecolor"))
            Me.doc_list_urgent_backcolor = System.Drawing.Color.FromArgb(savedValues("doc_list_urgent_backcolor"))
            Me.doc_list_urgent_forecolor = System.Drawing.Color.FromArgb(savedValues("doc_list_urgent_forecolor"))
            Me.workflow_current_stage_backcolor = System.Drawing.Color.FromArgb(savedValues("workflow_current_stage_backcolor"))
            Me.workflow_current_stage_backcolor = System.Drawing.Color.FromArgb(savedValues("workflow_current_stage_backcolor"))
            Me.workflow_next_stage_backcolor = System.Drawing.Color.FromArgb(savedValues("workflow_next_stage_backcolor"))
            Me.workflow_next_stage_forecolor = System.Drawing.Color.FromArgb(savedValues("workflow_next_stage_forecolor"))

        End Sub
    End Class

#End Region

    Friend Class documentListFields
        Private _docObjectIndex As Integer
        Private _id As String
        Private _title As String
        Private _doc_date As Date
        'Private _doc_date As String
        Private _pub_type As String
        Private _stage As String
        Private _author As String
        Private _checked_out_user As String
        Private _entity_name As String
        Private _completed_by As Date
        'Private _completed_by As String
        Private _bus_area As String
        Private _language As String
        Private _unread As Boolean
        Private _urgent_flag As Boolean
        Private _expire_flag As Boolean
        Private _stage_change_flag As Boolean
        Private _row_icon As Int16
        Private _distribution_status As String
        Private _attestation_num As String
        Private _attestation_pass_icon As Int16
        Private _fast_track_icon
        Private _attribute_change As String
        Private _assign_to As String

        Public Property assign_to() As String
            Get
                assign_to = _assign_to
            End Get
            Set(ByVal value As String)
                _assign_to = value
            End Set
        End Property

        Public Property attribute_change() As String
            Get
                attribute_change = _attribute_change
            End Get
            Set(ByVal value As String)
                _attribute_change = value
            End Set
        End Property

        Public Property attestation_num() As String
            Get
                attestation_num = _attestation_num
            End Get
            Set(ByVal value As String)
                _attestation_num = value
            End Set
        End Property
        Public Property fast_track_icon() As Int16
            Get
                fast_track_icon = _fast_track_icon
            End Get
            Set(ByVal value As Int16)
                _fast_track_icon = value
            End Set
        End Property

        Public Property attestation_pass_icon() As Int16
            Get
                attestation_pass_icon = _attestation_pass_icon
            End Get
            Set(ByVal value As Int16)
                _attestation_pass_icon = value
            End Set
        End Property
        Friend Property DocObjectIndex()
            Get
                DocObjectIndex = _docObjectIndex
            End Get
            Set(ByVal value)
                _docObjectIndex = value
            End Set
        End Property

        Friend Property ID() As String
            Get
                ID = _id
            End Get
            Set(ByVal value As String)
                _id = value
            End Set
        End Property
        Public Property Title() As String
            Get
                Title = _title
            End Get
            Set(ByVal value As String)
                _title = value
            End Set
        End Property
        Public Property Distribution_status() As String
            Get
                Distribution_status = _distribution_status
            End Get
            Set(ByVal value As String)
                _distribution_status = value
            End Set
        End Property

        'Public Property Doc_Date() As String
        '    Get
        '        Doc_Date = _doc_date
        '    End Get
        '    Set(ByVal value As String)
        '        _doc_date = value
        '    End Set
        'End Property
        Public Property Doc_Date() As Date
            Get
                Doc_Date = _doc_date
            End Get
            Set(ByVal value As Date)
                _doc_date = value
            End Set
        End Property

        Public Property Pub_Type() As String
            Get
                Pub_Type = _pub_type
            End Get
            Set(ByVal value As String)
                _pub_type = value
            End Set
        End Property

        Public Property Stage() As String
            Get
                Stage = _stage
            End Get
            Set(ByVal value As String)
                _stage = value
            End Set
        End Property

        Public Property Author() As String
            Get
                Author = _author
            End Get
            Set(ByVal value As String)
                _author = value
            End Set
        End Property

        Public Property Checked_Out_User()
            Get
                Checked_Out_User = _checked_out_user
            End Get
            Set(ByVal value)
                _checked_out_user = value
            End Set
        End Property

        Public Property Entity_Name() As String
            Get
                Entity_Name = _entity_name
            End Get
            Set(ByVal value As String)
                _entity_name = value
            End Set
        End Property

        Public Property Completed_By() As Date
            Get
                Completed_By = _completed_by
            End Get
            Set(ByVal value As Date)
                _completed_by = value
            End Set
        End Property

        Public Property Bus_Area() As String
            Get
                Bus_Area = _bus_area
            End Get
            Set(ByVal value As String)
                _bus_area = value
            End Set
        End Property

        Public Property Language() As String
            Get
                Language = _language
            End Get
            Set(ByVal value As String)
                _language = value
            End Set
        End Property

        Friend Property Unread() As Boolean
            Get
                Unread = _unread
            End Get
            Set(ByVal value As Boolean)
                _unread = value
            End Set
        End Property

        Friend Property Urgent_Flag() As Boolean
            Get
                Urgent_Flag = _urgent_flag
            End Get
            Set(ByVal value As Boolean)
                _urgent_flag = value
            End Set
        End Property

        Friend Property Expire_Flag() As Boolean
            Get
                Expire_Flag = _expire_flag
            End Get
            Set(ByVal value As Boolean)
                _expire_flag = value
            End Set
        End Property

        Friend Property Stage_Change_Flag() As Boolean
            Get
                Stage_Change_Flag = _stage_change_flag
            End Get
            Set(ByVal value As Boolean)
                _stage_change_flag = value
            End Set
        End Property

        Public Property Row_Icon() As Int16
            Get
                Row_Icon = _row_icon
            End Get
            Set(ByVal value As Int16)
                _row_icon = value
            End Set
        End Property

        Public Sub New()

        End Sub
    End Class

    Private Sub XXXXXXx()
        Throw New NotImplementedException
    End Sub

End Class







