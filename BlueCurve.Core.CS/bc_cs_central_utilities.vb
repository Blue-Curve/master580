REM ==========================================
REM Bluecurve Limited 2005
REM Module:       Central utilities
REM Type:         Common Services
REM Description:  Read Ini File
REM Version:      1
REM Change history
REM ==========================================
Imports Microsoft.Win32
Imports System.io
Imports System.Threading
Imports System.Data.SqlClient
Imports System.Globalization
Imports System.Xml
Imports System.Drawing
Imports System.Net.Mail
Imports System.Net.NetworkInformation
Imports System.Security.Cryptography.X509Certificates
Imports System.Reflection
Public Class bc_cs_max_address_services
    Public Function get_mac_address() As String
        get_mac_address = ""
        Try
            Dim nics() As NetworkInterface = NetworkInterface.GetAllNetworkInterfaces()

            For Each adapter In nics
                Select Case adapter.NetworkInterfaceType
                    'Exclude Tunnels, Loopbacks and PPP
                    Case NetworkInterfaceType.Tunnel, NetworkInterfaceType.Loopback, NetworkInterfaceType.Ppp
                    Case Else
                        If Not adapter.GetPhysicalAddress.ToString = String.Empty And Not adapter.GetPhysicalAddress.ToString = "00000000000000E0" Then
                            get_mac_address = adapter.GetPhysicalAddress.ToString
                            Exit For ' Got a mac so exit for
                        End If

                End Select
            Next adapter
        Catch
            get_mac_address = ""
        End Try
    End Function
End Class
Public Class bc_cs_icon_services
    Public Function get_icon_for_file_type(ByVal sfileext As String) As Bitmap
        Try
            Dim sProg As String
            Dim tmp As String

            tmp = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(sfileext).GetValue("")
            ' Get the program that will open files with this extension
            sProg = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(tmp).OpenSubKey("shell").OpenSubKey("open").OpenSubKey("command").GetValue("")

            ' strip the filename
            If sProg.Substring(0, 1) = Chr(34) Then
                sProg = sProg.Substring(1, sProg.IndexOf(Chr(34), 2) - 1)
            Else
                sProg = sProg.Substring(0, sProg.IndexOf(" ", 2))
            End If
            sProg = Replace(sProg, "%1", "")

            ' Extract the icon from the program
            Dim oicon As Icon

            oicon = System.Drawing.Icon.ExtractAssociatedIcon(sProg)
            get_icon_for_file_type = oicon.ToBitmap

        Catch ex As Exception
            Dim oerr As New bc_cs_activity_log("bc_cs_icon_services", "get_icon_for_file_type", bc_cs_activity_codes.COMMENTARY, ex.Message)
            get_icon_for_file_type = Nothing
        End Try

    End Function
End Class
Public Class bc_cs_central_settings
    REM config file location parameters
    Const BC_CONFIG_FILE = "bc_config.xml"
    Const BC_APPLICATION_USER_AREA = "Bluecurve"
    Public Const DEPLOYMENT_MANAGER_CONFIG_FILE = "bc_am_dd_config.xml"
    Public Const DEPLOYMENT_MANAGER_APPLICATION_NAME = "Deployment"

    REM available operability methods
    Public Const ADO = "ado"
    Public Const SOAP = "soap"
    Public Const LOCAL = "local"
    Public Const UNC = "unc"
    Public Const FTP = "ftp"
    Public Const SFTP = "sftp"

    Public Const NATIVE = "native"
    Public Const XML_UNIT = "xml_unit"
    Public Const XML_ALL = "xml_all"
    Public Shared check_components_image_url As String
    Public Shared use_rest_post As Boolean = False
    Public Shared use_rest_unboxed_post As Boolean = False
    Public Shared use_rest_compression As Boolean = False
    Public Shared connection_method As String
    Public Shared connection_type As String
    Public Shared dbname As String
    Public Shared servername As String
    Public Shared username As String
    Public Shared password As String
    Public Shared local_template_path As String
    Public Shared local_repos_path As String
    Public Shared central_template_path As String
    Public Shared central_repos_path As String
    Public Shared timeout As String
    Public Shared extended_timeout As String
    'Public Shared proxy As String
    Public Shared not_keep_alive As Integer
    REM Auto Proxy support
    'Public Shared pac_file As String
    REM Proxy Authentication Support
    'Public Shared authenticate As Boolean

    Public Shared process_caption As String

    Public Shared soap_server As String
    Public Shared debug_level As String
    Public Shared error_file As String
    Public Shared activity_file As String
    Public Shared trace As String
    Public Shared trace_sql As String
    Public Shared trace_history_days As Integer = 14
    Public Shared trace_log_archive As String = "\archive"

    Public Shared search_attributes As String
    Public Shared search_attributes_list As String

    Public Shared logged_on_user_name As String
    Public Shared logged_on_user_id As Long
    Public Shared version As String
    Public Shared file_transfer_method As String
    Public Shared file_storage_method_local As String
    Public Shared server_flag As Integer
    Public Shared server_log_file_path As String
    Public Shared authenticated_server_user As String
    Public Shared server_logging As Integer
    Public Shared project_name As String
    Public Shared show_authentication_form As Integer
    Public Shared user_authentication_method As String
    Public Shared ad_group As String
    'Public Shared allow_ad_prompt As Integer
    Public Shared show_keep_open_check_out As Boolean = False
    Public Shared ldap_path As String
    Public Shared authentication_retries As Integer
    Public Shared web_process_url As String
    Public Shared email_preview_url As String
    Public Shared aggregation_system_url As String

    Public Shared user_name As String
    Public Shared user_password As String
    Public Shared user_token As IntPtr = IntPtr.Zero
    Public Shared custom_library As String
    Public Shared common_platform As String
    Public Shared ado_timeout As String
    Public Shared suppress_server_error As Integer
    Public Shared retry As Integer
    REM flag to determine if config file successfully loaded
    Public Shared bloaderror As Boolean
    REM connection method selected by system when system attempts to load
    Public Shared ad_excel_token As String
    Public Shared selected_conn_method As String
    Public Shared selected_autenticated_method As Integer = -1
    REM progress bar
    Public Shared progress_bar As New bc_cs_progress
    Public Shared authentication_method As Integer = 0
    Public Shared logging_thread As Thread
    REM componentize section added by Rama Boya on 4-10-2006
    Public Shared ComponentizeBy As String
    Public Shared ComponentizeTableString1 As String
    Public Shared ComponentizeTableString2 As String
    Public Shared ComponentizeImageString1 As String
    Public Shared ComponentizeImageString2 As String
    Public Shared strPrefixorSuffix As String = "PrefixorSuffix"
    Public Shared strAnyWhere As String = "Anywhere"
    Public Shared merges As New ArrayList
    Public Shared loaded = False
    Public Shared environment_name As String
    Public Shared poll_interval As Long = 300000
    Public Shared alert_interval As Long = 5000
    Public Shared user_inactive_interval As Long = 10
    Public Shared service_poll_date_range As Long = 28
    Public Shared service_poll_enabled As Integer = 1
    Public Shared service_poll_type As String = "Create"
    Public Shared create_client_poll_enabled As Boolean = False
    Public Shared process_client_poll_enabled As Boolean = False
    Public Shared temp_save As Boolean = False
    'Public Shared server_errored As Boolean = False
    REM encryption details
    Public Shared IsEncrpted As String
    'Public Shared generic_filter As Boolean = False
    'Public Shared generic_filter_title As String
    Public Shared show_all_entities As Boolean = True
    Public Shared my_entities_default As Boolean = True

    REM Steve Wooderson Drexel 27/01/2014 Show prefs only
    Public Shared show_all_entities_submit As Boolean = True
    Public Shared my_entities_default_submit As Boolean = True

    REM external web services
    Public Shared external_web_serivce_url As String
    'Public Shared external_web_proxy As String
    'Public Shared external_web_proxy_fullname As String
    Public Shared log_external_data As Integer
    Public Shared run_time_pref_list As Boolean = False
    REM alternate entity list for create
    Public Shared alt_entity_for_build As Boolean = False
    Public Shared alt_entity_for_apref As Boolean = False
    Public Shared alt_entity_for_submit As Boolean = False
    REM advancec composite engine
    Public Shared use_advanced_composite_build As Boolean = False
    Public Shared search_pub_type_list As New ArrayList
    REM process custom events
    Public Shared custom_process_events_assembly_fullname As String
    Public Shared custom_process_events_assembly_namespace As String
    'Public Const CREATE_CONFIG_FILE = "bc_am_create_config.xml"
    Public Const PROCESS_CONFIG_FILE = "bc_am_process_config.xml"
    REM office version
    Public Shared office_version As Integer = 11
    Public Shared user_office_version As Integer = 0
    Public Shared external_files_path As String
    Public Shared multiple_instance As Boolean = False
    'Public Shared word_format As Integer = 0
    'Public Shared word_template_format As Integer = 0
    'Public Shared powerpoint_format As Integer = 0
    'Public Shared powerpoint_template_format = 0
    'Public Shared excel_format As Integer = 0
    Public Shared document_scan_namespace As String
    Public Shared document_scan_node As String

    REM Create Submit form settings
    Public Shared show_summary_text As Boolean = True
    Public Shared show_teaser_text As Boolean = True
    Public Shared summary_text_display_name As String = ""
    Public Shared teaser_text_display_name As String = ""
    REM FIL NOV 2013
    Public Shared allow_support_import As Boolean = False
    Public Shared deny_backdated_submission As Boolean = False

    Private Shared xnSystem As XmlNode

    Public Shared ap_log_poll As Integer
    Public Shared ap_pending_interval As Integer
    Public Shared ap_failure_interval As Integer

    Public Shared async_events_central_error_text As New ArrayList
    Public Shared selected_system As String = ""

    Private Shared boolCommandLine As Boolean = False
    Public Shared webconfig_maxRequestLength = 2000000
    Public Shared parallel_refresh_mode As Integer = 0
    Public Shared num_min_idle_thread As Integer = 0
    Public Shared save_doc_metadata As Integer = 0
    Public Shared doc_metadata_path As String = ""
    Public Shared always_authenticate As Integer = 0

    REM reach may 2015
    REM EMail
    Public Shared smtp_gateways As New List(Of bc_cs_email_services.bc_cs_smtp_gateway)
    Public Shared email_service_enabled As Boolean = False
    Public Shared email_service_poll_interval As Integer = 5000
    Public Shared email_service_maximum_concurrent_threads = 64
    REM Bulk email 
    Public Shared bulk_email_service_api_key As String
    Public Shared bulk_email_service_enabled As Boolean = False
    Public Shared bulk_email_service_poll_interval As Integer = 5000
    Public Shared bulk_email_service_maximum_concurrent_threads As Integer = 64

    REM Email previw
    Public Shared email_preview_service_enabled As Boolean = False
    Public Shared email_preview_service_poll_interval As Integer = 5000

    REM mailig list
    Public Shared mailing_list_service_enabled As Boolean = False
    Public Shared mailing_list_service_poll_interval As Integer = 5000

    REM distribution
    Public Shared distribution_service_enabled As Boolean = False
    Public Shared distribution_service_poll_interval As Integer = 5000


    REM aggregation
    Public Shared aggregation_service_enabled As Boolean = False
    Public Shared aggregation_service_poll_interval As Integer = 5000
    Public Shared aggregation_service_num_active_threads As Integer = 32
    Public Shared aggregation_service_entities_per_batch = 20
    Public Shared aggregation_service_run_universes_concurrently As Boolean = False
    Public Shared aggregation_service_run_under_IIS As Boolean = False


    REM calcs
    Public Shared calc_service_enabled As Boolean = False
    Public Shared calc_service_poll_interval As Integer = 5000
    Public Shared calc_service_num_active_threads As Integer = 32
    Public Shared calc_service_entities_per_batch = 20
    Public Shared calc_service_run_entities_concurrently As Boolean = False
    Public Shared calc_service_run_under_IIS As Boolean = False

    REM Data load
    Public Shared data_load_service_enabled As Boolean = False
    Public Shared data_load_poll_interval As Integer = 5000
    Public Shared data_load_run_under_IIS As Boolean = False

    REM user session
    Public Shared log_user_session As Boolean = False

    REM wcf binding
    Public Shared wcf_binding As String

    Public Shared loaded_for_rtd As Boolean = False
    Public Shared override_username_password As Boolean = False


    REM web repos
    Public Shared repos_file_system As Boolean = True
    Public Shared repos_db As Boolean = False
    Public Shared repos_html_db As Boolean = False
    Public Shared repos_components_db As Boolean = False


    Public Shared Property CommandLine() As Boolean
        Get
            Return boolCommandLine
        End Get
        Set(ByVal value As Boolean)
            boolCommandLine = value
        End Set
    End Property

    Private Shared strApplicationName As String
    Public Shared Property ApplicationName() As String
        Get
            Return strApplicationName
        End Get
        Set(ByVal value As String)
            strApplicationName = value
        End Set
    End Property

    <Serializable()> Public Class pub_type_merges
        Public pub_type_id As String
        Public merge_name As String
        Public bookmark_name As String
        Public Sub New()

        End Sub
    End Class
    REM constructor to load from file
    Public Sub New(ByVal strloadfromfile As Boolean, Optional ByVal system As String = "")

        ApplicationName = strApplicationName
        Dim blogging As String
        bloaderror = False
        log_external_data = 1
        REM read .ini file
        read_ini_file(system)
        REM get logged on user
        blogging = bc_cs_central_settings.activity_file
        bc_cs_central_settings.activity_file = "off"
        logged_on_user_name = GetLoginName()
        bc_cs_central_settings.activity_file = blogging



        'If bc_cs_central_settings.server_flag = 0 Then
        '    'If InStr(LCase(Diagnostics.Process.GetCurrentProcess.MainModule.ModuleName), "create") <> 0 Or _
        '    '    InStr(LCase(Diagnostics.Process.GetCurrentProcess.MainModule.ModuleName), "process") <> 0 Or _
        '    '    InStr(LCase(Diagnostics.Process.GetCurrentProcess.MainModule.ModuleName), "winword") <> 0 Then
        '    '    readConfigFile()
        '    'End If
        '    If InStr(LCase(Diagnostics.Process.GetCurrentProcess.MainModule.ModuleName), "process") <> 0 Then
        '        readprocessConfigFile()
        '    End If
        'End If
    End Sub
    REM constructor to use memory shared settings
    Public Sub New()
    End Sub

    'Private Sub readprocessConfigFile()

    '    Dim xmlload As New Xml.XmlDocument
    '    Dim myXmlNodeList As Xml.XmlNodeList

    '    Try

    '        xmlload.Load(bc_cs_central_settings.local_template_path + PROCESS_CONFIG_FILE)
    '        REM PR genric filter switch
    '        myXmlNodeList = xmlload.SelectNodes("/process_settings/generic_filter")
    '        If myXmlNodeList.Count > 0 Then
    '            generic_filter = xmlload.SelectSingleNode("/process_settings/generic_filter").InnerXml()
    '        End If
    '        myXmlNodeList = xmlload.SelectNodes("/process_settings/generic_filter_title")
    '        If myXmlNodeList.Count > 0 Then
    '            generic_filter_title = xmlload.SelectSingleNode("/process_settings/generic_filter_title").InnerXml()
    '        End If

    '    Catch ex As Exception
    '        Dim slog As New bc_cs_activity_log("Error Reading Process Config File", "readprocessConfigFile", CStr(bc_cs_activity_codes.COMMENTARY), ex.Message)
    '    End Try
    'End Sub
    'Private Sub readConfigFile()

    '    Dim xmlload As New Xml.XmlDocument
    '    Dim myXmlNodeList As Xml.XmlNodeList
    '    Dim myxmlnode As Xml.XmlNode
    '    Dim opubtypemerge As pub_type_merges

    '    Dim i As Integer
    '    Try

    '        REM FIL JUN 2013
    '        xmlload.Load(bc_cs_central_settings.central_template_path + CREATE_CONFIG_FILE)
    '        'xmlload.Load(bc_cs_central_settings.local_template_path + CREATE_CONFIG_FILE)
    '        REM PR genric filter switch

    '        myXmlNodeList = xmlload.SelectNodes("/create_settings/show_all_entities")
    '        If myXmlNodeList.Count > 0 Then
    '            show_all_entities = xmlload.SelectSingleNode("/create_settings/show_all_entities").InnerXml()
    '        End If
    '        myXmlNodeList = xmlload.SelectNodes("/create_settings/my_entities_default")
    '        If myXmlNodeList.Count > 0 Then
    '            my_entities_default = xmlload.SelectSingleNode("/create_settings/my_entities_default").InnerXml()
    '        End If
    '        myXmlNodeList = xmlload.SelectNodes("/create_settings/alt_entity_for_build")
    '        If myXmlNodeList.Count > 0 Then
    '            alt_entity_for_build = xmlload.SelectSingleNode("/create_settings/alt_entity_for_build").InnerXml()
    '        End If
    '        myXmlNodeList = xmlload.SelectNodes("/create_settings/alt_entity_for_apref")
    '        If myXmlNodeList.Count > 0 Then
    '            alt_entity_for_apref = xmlload.SelectSingleNode("/create_settings/alt_entity_for_apref").InnerXml()
    '        End If
    '        myXmlNodeList = xmlload.SelectNodes("/create_settings/alt_entity_for_submit")
    '        If myXmlNodeList.Count > 0 Then
    '            alt_entity_for_submit = xmlload.SelectSingleNode("/create_settings/alt_entity_for_submit").InnerXml()
    '        End If
    '        merges.Clear()
    '        myXmlNodeList = xmlload.SelectNodes("/create_settings/custom_library/pub_types/id")
    '        i = 0
    '        For Each myxmlnode In myXmlNodeList
    '            opubtypemerge = New pub_type_merges
    '            opubtypemerge.pub_type_id = myxmlnode.InnerXml
    '            merges.Add(opubtypemerge)
    '            i = i + 1
    '        Next
    '        myXmlNodeList = xmlload.SelectNodes("/create_settings/custom_library/pub_types/merge_name")
    '        i = 0
    '        For Each myxmlnode In myXmlNodeList
    '            merges(i).merge_name = myxmlnode.InnerXml
    '            i = i + 1
    '        Next
    '        myXmlNodeList = xmlload.SelectNodes("/create_settings/custom_library/pub_types/bookmark_name")
    '        i = 0
    '        For Each myxmlnode In myXmlNodeList
    '            merges(i).bookmark_name = myxmlnode.InnerXml
    '            i = i + 1
    '        Next

    '        myXmlNodeList = xmlload.SelectNodes("/create_settings/custom_library/name")
    '        If myXmlNodeList.Count > 0 Then
    '            custom_library = xmlload.SelectSingleNode("/create_settings/custom_library/name").InnerXml()
    '        End If

    '        myXmlNodeList = xmlload.SelectNodes("/create_settings/adv_composite_build")
    '        If myXmlNodeList.Count > 0 Then
    '            use_advanced_composite_build = xmlload.SelectSingleNode("/create_settings/adv_composite_build").InnerXml()
    '        End If

    '        myXmlNodeList = xmlload.SelectNodes("/create_settings/adv_composite_search/pub_type_name")
    '        search_pub_type_list.Clear()
    '        For Each myxmlnode In myXmlNodeList
    '            search_pub_type_list.Add(myxmlnode.InnerXml())
    '        Next

    '        myXmlNodeList = xmlload.SelectNodes("/create_settings/document_scan_namespace")
    '        If myXmlNodeList.Count > 0 Then
    '            document_scan_namespace = xmlload.SelectSingleNode("/create_settings/document_scan_namespace").InnerXml()
    '        End If

    '        myXmlNodeList = xmlload.SelectNodes("/create_settings/document_scan_node")
    '        If myXmlNodeList.Count > 0 Then
    '            document_scan_node = xmlload.SelectSingleNode("/create_settings/document_scan_node").InnerXml()
    '        End If

    '        myXmlNodeList = xmlload.SelectNodes("/create_settings/document_scan_node")
    '        If myXmlNodeList.Count > 0 Then
    '            document_scan_node = xmlload.SelectSingleNode("/create_settings/document_scan_node").InnerXml()
    '        End If

    '        myXmlNodeList = xmlload.SelectNodes("/create_settings/deny_backdated_submission")
    '        If myXmlNodeList.Count > 0 Then
    '            deny_backdated_submission = xmlload.SelectSingleNode("/create_settings/deny_backdated_submission").InnerXml()
    '        End If

    '        myXmlNodeList = xmlload.SelectNodes("/create_settings/show_summary_text")
    '        If myXmlNodeList.Count > 0 Then
    '            show_summary_text = xmlload.SelectSingleNode("/create_settings/show_summary_text").InnerXml()
    '        End If
    '        myXmlNodeList = xmlload.SelectNodes("/create_settings/show_teaser_text")
    '        If myXmlNodeList.Count > 0 Then
    '            show_teaser_text = xmlload.SelectSingleNode("/create_settings/show_teaser_text").InnerXml()
    '        End If
    '        myXmlNodeList = xmlload.SelectNodes("/create_settings/summary_text_display_name")
    '        If myXmlNodeList.Count > 0 Then
    '            summary_text_display_name = xmlload.SelectSingleNode("/create_settings/summary_text_display_name").InnerXml()
    '        End If
    '        myXmlNodeList = xmlload.SelectNodes("/create_settings/teaser_text_display_name")
    '        If myXmlNodeList.Count > 0 Then
    '            teaser_text_display_name = xmlload.SelectSingleNode("/create_settings/teaser_text_display_name").InnerXml()
    '        End If


    '    Catch ex As Exception
    '        Dim slog As New bc_cs_activity_log("Error Reading Create Config File", "readConfigFile", CStr(bc_cs_activity_codes.COMMENTARY), ex.Message)
    '    End Try
    'End Sub

    Private Function GetSystemNode(ByRef xdSystems As XmlDocument, Optional ByVal system As String = "") As XmlNode

        GetSystemNode = Nothing


        REM oct 2018 multi version clients
        REM get application versin
       

        Dim xnlSystems As XmlNodeList = xdSystems.GetElementsByTagName("system")
        If xnlSystems.Count = 0 Then
            'Error
            Return xdSystems.DocumentElement
        ElseIf xnlSystems.Count > 1 Then
           
            If Environment.GetCommandLineArgs().Length > 1 Then
                For Each xnSystem As XmlNode In xnlSystems
                    If xnSystem.SelectSingleNode("name").InnerText.ToLower.Replace(" ", "_") = Environment.GetCommandLineArgs(1).ToLower() Then
                        Return xnSystem
                    End If
                Next
            End If

            Dim bcsf As New bc_cs_system_form
            bcsf.cbSystems.Items.Clear()
            For Each xnSystem As XmlNode In xnlSystems
                bcsf.cbSystems.Items.Add(xnSystem.SelectSingleNode("name").InnerText)
            Next
            bcsf.cbSystems.SelectedIndex = 0

          

            REM PR May 2013 pass system through if known
            If system = "" Then
                Dim version As String
                version = Assembly.GetExecutingAssembly().GetName().Version.ToString()
              
                For Each xnSystem As XmlNode In xnlSystems
                    If version = xnSystem.SelectSingleNode("name").InnerText Then
                        selected_system = bcsf.SystemName

                        Return xnSystem
                    End If
                Next

                bcsf.ShowDialog()
                If bcsf.AbortStartup Then

                ElseIf Not bcsf.SystemName Is Nothing Then
                    For Each xnSystem As XmlNode In xnlSystems
                        If bcsf.SystemName = xnSystem.SelectSingleNode("name").InnerText Then
                            selected_system = bcsf.SystemName
                            Return xnSystem
                        End If
                    Next
                Else
                    'Error
                End If
            Else
                For Each xnSystem As XmlNode In xnlSystems
                    If system = xnSystem.SelectSingleNode("name").InnerText Then

                        Return xnSystem
                    End If
                Next
            End If
        Else
            Return xnlSystems(0)
        End If

    End Function

    Private Sub read_ini_file(Optional ByVal system As String = "")
        Dim xmlload As New Xml.XmlDocument
        Dim strfn As String
        Dim myXmlNodeList As Xml.XmlNodeList
        Dim myXmlNodeDBTypeList As Xml.XmlNodeList
        Dim myxmlnode As Xml.XmlNode
        Dim userDir As String

        Dim i As Integer
        Dim opubtypemerge As pub_type_merges
        'Export registry keys into current user key if there are no settinsg existing
        Me.ExportRegistryKeys()
        Try
            userDir = get_user_dir()
            strfn = userDir + "\" + BC_CONFIG_FILE
            xmlload.Load(strfn)
            server_flag = 0

            If xnSystem Is Nothing Or system <> "" Then
                xnSystem = GetSystemNode(xmlload, system)
            End If


            connection_method = xnSystem.SelectSingleNode("connectivity/method").InnerXml()
            myXmlNodeDBTypeList = xnSystem.SelectNodes("connectivity/ado/type")
            If myXmlNodeDBTypeList.Count > 0 Then
                connection_type = xnSystem.SelectSingleNode("connectivity/ado/type").InnerXml.Trim
            Else
                'if there is no configuration information use normal connection type.
                connection_type = "normal"
            End If


            myXmlNodeList = xnSystem.SelectNodes("multiple_instance ")
            If myXmlNodeList.Count > 0 Then
                multiple_instance = xnSystem.SelectSingleNode("multiple_instance").InnerXml()
            End If



            'alerter polling interval
            myXmlNodeList = xnSystem.SelectNodes("alerter/poll_interval")
            If myXmlNodeList.Count > 0 Then
                poll_interval = xnSystem.SelectSingleNode("alerter/poll_interval").InnerXml()
            End If
            myXmlNodeList = xnSystem.SelectNodes("alerter/alert_interval")
            If myXmlNodeList.Count > 0 Then
                alert_interval = xnSystem.SelectSingleNode("alerter/alert_interval").InnerXml()
            End If
            myXmlNodeList = xnSystem.SelectNodes("alerter/service_poll_date_range")
            If myXmlNodeList.Count > 0 Then
                service_poll_date_range = xnSystem.SelectSingleNode("alerter/service_poll_date_range").InnerXml()
            End If
            myXmlNodeList = xnSystem.SelectNodes("alerter/service_poll_enabled")
            If myXmlNodeList.Count > 0 Then
                service_poll_enabled = xnSystem.SelectSingleNode("alerter/service_poll_enabled").InnerXml()
            End If
            myXmlNodeList = xnSystem.SelectNodes("alerter/service_poll_type")
            If myXmlNodeList.Count > 0 Then
                service_poll_type = xnSystem.SelectSingleNode("alerter/service_poll_type").InnerXml()
            End If
            myXmlNodeList = xnSystem.SelectNodes("temp_save")
            If myXmlNodeList.Count > 0 Then
                temp_save = xnSystem.SelectSingleNode("temp_save").InnerXml()
            End If
            myXmlNodeList = xnSystem.SelectNodes("environment")
            If myXmlNodeList.Count > 0 Then
                environment_name = xnSystem.SelectSingleNode("environment").InnerXml()
            End If
            REM Polling steve wooderson 27/09/2011
            myXmlNodeList = xnSystem.SelectNodes("alerter/user_inactive_interval")
            If myXmlNodeList.Count > 0 Then
                user_inactive_interval = xnSystem.SelectSingleNode("alerter/user_inactive_interval").InnerXml()
            End If


            REM security steve wooderson 03/09/2010
            myXmlNodeList = xnSystem.SelectNodes("authentication/method")
            If myXmlNodeList.Count > 0 Then
                user_authentication_method = xnSystem.SelectSingleNode("authentication/method").InnerXml()
            Else
                user_authentication_method = "cleartext"
            End If

            REM security steve wooderson 03/09/2010
            myXmlNodeList = xnSystem.SelectNodes("authentication/method")
            If myXmlNodeList.Count > 0 Then
                user_authentication_method = xnSystem.SelectSingleNode("authentication/method").InnerXml()
            Else
                user_authentication_method = "cleartext"
            End If

            ' ldap authentication 
            ' path
            'myXmlNodeList = xnSystem.SelectNodes("authentication/ldap_path")
            'If myXmlNodeList.Count > 0 Then
            '    ldap_path = xnSystem.SelectSingleNode("authentication/ldap_path").InnerXml()
            'Else
            '    ldap_path = ""
            'End If

            '' group
            'myXmlNodeList = xnSystem.SelectNodes("authentication/ad_group")
            'If myXmlNodeList.Count > 0 Then
            '    ad_group = xnSystem.SelectSingleNode("authentication/ad_group").InnerXml()
            'Else
            '    ad_group = ""
            'End If

            ' retries
            myXmlNodeList = xnSystem.SelectNodes("authentication/retries")
            If myXmlNodeList.Count > 0 Then
                authentication_retries = xnSystem.SelectSingleNode("authentication/retries").InnerXml()
            Else
                authentication_retries = 3
            End If

            servername = xnSystem.SelectSingleNode("connectivity/ado/server").InnerXml()
            dbname = xnSystem.SelectSingleNode("connectivity/ado/name").InnerXml()
            username = xnSystem.SelectSingleNode("connectivity/ado/user").InnerXml()
            password = xnSystem.SelectSingleNode("connectivity/ado/password").InnerXml()

            local_template_path = xnSystem.SelectSingleNode("templates/local_repos").InnerXml()
            ' append profile directory to path if relative
            If local_template_path = "templates\" Then
                local_template_path = String.Concat(userDir, "\", local_template_path)
            End If

            ' blue print may have the same path as local so again append profile directory to path if relative
            central_template_path = xnSystem.SelectSingleNode("templates/central_repos").InnerXml()
            If central_template_path = "templates\" Then
                central_template_path = String.Concat(userDir, "\", central_template_path)
            End If

            local_repos_path = xnSystem.SelectSingleNode("documents/local_repos").InnerXml()
            ' append profile directory to path if relative
            If local_repos_path = "local documents\" Then
                local_repos_path = String.Concat(userDir, "\", local_repos_path)
            End If
            central_repos_path = xnSystem.SelectSingleNode("documents/central_repos").InnerXml()

            soap_server = xnSystem.SelectSingleNode("connectivity/soap/server").InnerXml()
            debug_level = xnSystem.SelectSingleNode("client/error_logging/level").InnerXml()
            error_file = xnSystem.SelectSingleNode("client/error_logging/file").InnerXml()
            activity_file = xnSystem.SelectSingleNode("client/activity_logging/file").InnerXml()
            custom_library = xnSystem.SelectSingleNode("custom_library/name").InnerXml()
            trace = xnSystem.SelectSingleNode("client/activity_logging/trace").InnerXml()
            trace_sql = xnSystem.SelectSingleNode("client/activity_logging/trace_sql").InnerXml()

            REM Steve Wooderson 10/03/2011 Activity Trace
            myXmlNodeList = xnSystem.SelectNodes("client/activity_logging/trace_history_days")
            If myXmlNodeList.Count > 0 Then
                trace_history_days = xnSystem.SelectSingleNode("client/activity_logging/trace_history_days").InnerXml
            End If
            myXmlNodeList = xnSystem.SelectNodes("client/activity_logging/trace_log_archive")
            If myXmlNodeList.Count > 0 Then
                trace_log_archive = xnSystem.SelectSingleNode("client/activity_logging/trace_log_archive").InnerXml
            End If

            REM Steve Wooderson 24/03/2011 Search Settings 
            myXmlNodeList = xnSystem.SelectNodes("text_search/search_attributes")
            If myXmlNodeList.Count > 0 Then
                search_attributes = xnSystem.SelectSingleNode("text_search/search_attributes").InnerXml
            End If
            myXmlNodeList = xnSystem.SelectNodes("text_search/search_attributes_list")
            If myXmlNodeList.Count > 0 Then
                search_attributes_list = xnSystem.SelectSingleNode("text_search/search_attributes_list").InnerXml
            End If

            file_storage_method_local = xnSystem.SelectSingleNode("client/file_storage_local/method").InnerXml()
            server_flag = xnSystem.SelectSingleNode("connectivity/soap/server_flag").InnerXml()
            server_logging = xnSystem.SelectSingleNode("connectivity/soap/server_logging").InnerXml()
            server_log_file_path = xnSystem.SelectSingleNode("connectivity/soap/server_logging_path").InnerXml()

            myXmlNodeList = xnSystem.SelectNodes("connectivity/soap/webconfig_maxRequestLength")
            If myXmlNodeList.Count > 0 Then
                webconfig_maxRequestLength = xnSystem.SelectSingleNode("connectivity/soap/webconfig_maxRequestLength").InnerXml()
            End If


            show_authentication_form = xnSystem.SelectSingleNode("authentication/prompt").InnerXml()





            'myXmlNodeList = xnSystem.SelectNodes("authentication/allow_ad_prompt")
            'If myXmlNodeList.Count > 0 Then

            '    allow_ad_prompt = xnSystem.SelectSingleNode("authentication/allow_ad_prompt").InnerXml()
            'End If


            REM timeout
            myXmlNodeList = xnSystem.SelectNodes("connectivity/soap/timeout")
            If myXmlNodeList.Count > 0 Then
                timeout = xnSystem.SelectSingleNode("connectivity/soap/timeout").InnerXml()
            End If

            REM proxy
            'myXmlNodeList = xnSystem.SelectNodes("connectivity/soap/proxy")
            'If myXmlNodeList.Count > 0 Then
            '    proxy = xnSystem.SelectSingleNode("connectivity/soap/proxy").InnerXml()
            'End If

            'REM pac file
            'myXmlNodeList = xnSystem.SelectNodes("connectivity/soap/pac")
            'If myXmlNodeList.Count > 0 Then
            '    pac_file = xnSystem.SelectSingleNode("connectivity/soap/pac").InnerXml()
            'End If

            REM proxy authentication
            myXmlNodeList = xnSystem.SelectNodes("connectivity/soap/authenticate")
            If myXmlNodeList.Count > 0 Then
                'authenticate = xnSystem.SelectSingleNode("connectivity/soap/authenticate").InnerXml()
            End If

            REM common platform
            myXmlNodeList = xnSystem.SelectNodes("connectivity/ado/common_platform")
            If myXmlNodeList.Count > 0 Then
                common_platform = xnSystem.SelectSingleNode("connectivity/ado/common_platform").InnerXml()
            End If
            REM ado timeout
            ado_timeout = 30
            myXmlNodeList = xnSystem.SelectNodes("connectivity/ado/timeout")
            If myXmlNodeList.Count > 0 Then
                ado_timeout = xnSystem.SelectSingleNode("connectivity/ado/timeout").InnerXml()
            End If
            REM exteded ado timeout
            extended_timeout = 600
            myXmlNodeList = xnSystem.SelectNodes("connectivity/ado/extended_timeout")
            If myXmlNodeList.Count > 0 Then
                extended_timeout = xnSystem.SelectSingleNode("connectivity/ado/extended_timeout").InnerXml()
            End If
            REM suppress server error
            myXmlNodeList = xnSystem.SelectNodes("connectivity/soap/suppress_server_error")
            suppress_server_error = 0
            If myXmlNodeList.Count > 0 Then
                suppress_server_error = xnSystem.SelectSingleNode("connectivity/soap/suppress_server_error").InnerXml()
            End If
            REM web service retry
            bc_cs_central_settings.retry = 3
            myXmlNodeList = xnSystem.SelectNodes("connectivity/soap/retry")
            If myXmlNodeList.Count > 0 Then
                bc_cs_central_settings.retry = xnSystem.SelectSingleNode("connectivity/soap/retry").InnerXml()
            End If
            REM proxy
            bc_cs_central_settings.not_keep_alive = 0
            myXmlNodeList = xnSystem.SelectNodes("connectivity/soap/not_keep_alive")
            If myXmlNodeList.Count > 0 Then
                bc_cs_central_settings.not_keep_alive = xnSystem.SelectSingleNode("connectivity/soap/not_keep_alive").InnerXml()
            End If
            REM external data
            myXmlNodeList = xnSystem.SelectNodes("connectivity/ado/external_data/external_web_service_url")
            If myXmlNodeList.Count > 0 Then
                bc_cs_central_settings.external_web_serivce_url = xnSystem.SelectSingleNode("connectivity/ado/external_data/external_web_service_url").InnerXml
            End If
            myXmlNodeList = xnSystem.SelectNodes("connectivity/ado/external_data/log_external_data")
            If myXmlNodeList.Count > 0 Then
                bc_cs_central_settings.log_external_data = xnSystem.SelectSingleNode("connectivity/ado/external_data/log_external_data").InnerXml
            End If
            myXmlNodeList = xnSystem.SelectNodes("process_custom_events/namespace")

            If myXmlNodeList.Count > 0 Then
                bc_cs_central_settings.custom_process_events_assembly_namespace = xnSystem.SelectSingleNode("process_custom_events/namespace").InnerXml
            End If
            myXmlNodeList = xnSystem.SelectNodes("process_custom_events/fullname")
            If myXmlNodeList.Count > 0 Then
                bc_cs_central_settings.custom_process_events_assembly_fullname = xnSystem.SelectSingleNode("process_custom_events/fullname").InnerXml
            End If

            myXmlNodeList = xnSystem.SelectNodes("documents/office_version")
            If myXmlNodeList.Count > 0 Then
                bc_cs_central_settings.office_version = xnSystem.SelectSingleNode("documents/office_version").InnerXml
            End If

            REM Steve Wooderson 11/01/2012 External File Settings 
            myXmlNodeList = xnSystem.SelectNodes("documents/external_files_path")
            If myXmlNodeList.Count > 0 Then
                bc_cs_central_settings.external_files_path = xnSystem.SelectSingleNode("documents/external_files_path").InnerXml
            Else
                bc_cs_central_settings.external_files_path = bc_cs_central_settings.central_repos_path
            End If
            bc_cs_central_settings.ap_log_poll = 0
            myXmlNodeList = xnSystem.SelectNodes("aync_process_events/log_poll")
            If myXmlNodeList.Count > 0 Then
                Try
                    bc_cs_central_settings.ap_log_poll = xnSystem.SelectSingleNode("aync_process_events/log_poll").InnerXml
                Catch

                End Try
            End If
            bc_cs_central_settings.ap_pending_interval = 10000
            myXmlNodeList = xnSystem.SelectNodes("aync_process_events/pending_poll_interval")
            If myXmlNodeList.Count > 0 Then
                Try
                    bc_cs_central_settings.ap_pending_interval = xnSystem.SelectSingleNode("aync_process_events/pending_poll_interval").InnerXml
                Catch

                End Try
            End If
            bc_cs_central_settings.ap_failure_interval = 10000
            myXmlNodeList = xnSystem.SelectNodes("aync_process_events/failure_poll_interval")
            If myXmlNodeList.Count > 0 Then
                Try
                    bc_cs_central_settings.ap_failure_interval = xnSystem.SelectSingleNode("aync_process_events/failure_poll_interval").InnerXml
                Catch

                End Try
            End If
            bc_cs_central_settings.parallel_refresh_mode = False
            myXmlNodeList = xnSystem.SelectNodes("parallelisation/mode")
            If myXmlNodeList.Count > 0 Then
                Try
                    bc_cs_central_settings.parallel_refresh_mode = xnSystem.SelectSingleNode("parallelisation/mode").InnerXml
                Catch

                End Try
            End If


            myXmlNodeList = xnSystem.SelectNodes("parallelisation/num_min_idle_thread")
            If myXmlNodeList.Count > 0 Then
                Try
                    bc_cs_central_settings.num_min_idle_thread = xnSystem.SelectSingleNode("parallelisation/num_min_idle_thread").InnerXml
                Catch

                End Try
            End If
            process_caption = ""
            myXmlNodeList = xnSystem.SelectNodes("process_caption")
            If myXmlNodeList.Count > 0 Then
                Try
                    bc_cs_central_settings.process_caption = xnSystem.SelectSingleNode("process_caption").InnerXml
                Catch

                End Try
            End If
            REM 
            myXmlNodeList = xnSystem.SelectNodes("documents/save_doc_metadata")
            If myXmlNodeList.Count > 0 Then
                Try
                    bc_cs_central_settings.save_doc_metadata = xnSystem.SelectSingleNode("documents/save_doc_metadata").InnerXml
                Catch

                End Try
            End If
            myXmlNodeList = xnSystem.SelectNodes("documents/doc_metadata_path")
            If myXmlNodeList.Count > 0 Then
                Try
                    bc_cs_central_settings.doc_metadata_path = xnSystem.SelectSingleNode("documents/doc_metadata_path").InnerXml
                Catch

                End Try
            End If

            myXmlNodeList = xnSystem.SelectNodes("always_authenticate")
            If myXmlNodeList.Count > 0 Then
                Try
                    bc_cs_central_settings.always_authenticate = xnSystem.SelectSingleNode("always_authenticate").InnerXml
                Catch
                    always_authenticate = 0
                End Try
            End If
            read_smtp_gateways()

            myXmlNodeList = xnSystem.SelectNodes("connectivity/soap/wcf_binding ")
            bc_cs_central_settings.wcf_binding = "binary"
            If myXmlNodeList.Count > 0 Then
                Try
                    bc_cs_central_settings.wcf_binding = xnSystem.SelectSingleNode("connectivity/soap/wcf_binding ").InnerXml
                Catch
                    bc_cs_central_settings.wcf_binding = "binary"
                End Try
            End If

            myXmlNodeList = xnSystem.SelectNodes("connectivity/soap/web_process_url")
            If myXmlNodeList.Count > 0 Then
                Try
                    bc_cs_central_settings.web_process_url = xnSystem.SelectSingleNode("connectivity/soap/web_process_url").InnerXml
                Catch
                    bc_cs_central_settings.web_process_url = ""
                End Try
            End If
            myXmlNodeList = xnSystem.SelectNodes("connectivity/soap/email_preview_url")
            If myXmlNodeList.Count > 0 Then
                Try
                    bc_cs_central_settings.email_preview_url = xnSystem.SelectSingleNode("connectivity/soap/email_preview_url").InnerXml
                Catch
                    bc_cs_central_settings.email_preview_url = ""
                End Try
            End If

            myXmlNodeList = xnSystem.SelectNodes("connectivity/soap/aggregation_system_url")
            If myXmlNodeList.Count > 0 Then
                Try
                    bc_cs_central_settings.aggregation_system_url = xnSystem.SelectSingleNode("connectivity/soap/aggregation_system_url").InnerXml
                Catch
                    bc_cs_central_settings.aggregation_system_url = "localhost"
                End Try
            End If
            bc_cs_central_settings.check_components_image_url = "localhost"
            myXmlNodeList = xnSystem.SelectNodes("connectivity/soap/check_components_image_url")
            If myXmlNodeList.Count > 0 Then
                Try
                    bc_cs_central_settings.check_components_image_url = xnSystem.SelectSingleNode("connectivity/soap/check_components_image_url").InnerXml
                Catch
                    bc_cs_central_settings.check_components_image_url = "localhost"
                End Try
            End If

            bc_cs_central_settings.use_rest_post = False
            myXmlNodeList = xnSystem.SelectNodes("connectivity/soap/use_rest_post")
            If myXmlNodeList.Count > 0 Then
                Try
                    bc_cs_central_settings.use_rest_post = xnSystem.SelectSingleNode("connectivity/soap/use_rest_post").InnerXml
                Catch
                    bc_cs_central_settings.use_rest_post = False
                End Try
            End If
            bc_cs_central_settings.use_rest_unboxed_post = False
            myXmlNodeList = xnSystem.SelectNodes("connectivity/soap/use_rest_unboxed_post")
            If myXmlNodeList.Count > 0 Then
                Try
                    bc_cs_central_settings.use_rest_unboxed_post = xnSystem.SelectSingleNode("connectivity/soap/use_rest_unboxed_post").InnerXml
                Catch
                    bc_cs_central_settings.use_rest_unboxed_post = False
                End Try
            End If
            bc_cs_central_settings.use_rest_compression = False
            myXmlNodeList = xnSystem.SelectNodes("connectivity/soap/use_rest_compression")
            If myXmlNodeList.Count > 0 Then
                Try
                    bc_cs_central_settings.use_rest_compression = xnSystem.SelectSingleNode("connectivity/soap/use_rest_compression").InnerXml
                Catch
                    bc_cs_central_settings.use_rest_compression = False
                End Try
            End If

            bc_cs_central_settings.repos_file_system = True

            myXmlNodeList = xnSystem.SelectNodes("documents/repos_file_system")
            If myXmlNodeList.Count > 0 Then
                Try
                    bc_cs_central_settings.wcf_binding = xnSystem.SelectSingleNode("documents/repos_file_system").InnerXml
                Catch
                    bc_cs_central_settings.repos_file_system = True
                End Try
            End If

            bc_cs_central_settings.repos_db = False

            myXmlNodeList = xnSystem.SelectNodes("documents/repos_db")
            If myXmlNodeList.Count > 0 Then
                Try
                    bc_cs_central_settings.wcf_binding = xnSystem.SelectSingleNode("documents/repos_db").InnerXml
                Catch
                    bc_cs_central_settings.repos_db = False
                End Try
            End If


            bc_cs_central_settings.repos_html_db = False

            myXmlNodeList = xnSystem.SelectNodes("documents/repos_html_db")
            If myXmlNodeList.Count > 0 Then
                Try
                    bc_cs_central_settings.repos_html_db = xnSystem.SelectSingleNode("documents/repos_html_db").InnerXml
                Catch
                    bc_cs_central_settings.repos_html_db = False
                End Try
            End If

            bc_cs_central_settings.repos_components_db = False

            myXmlNodeList = xnSystem.SelectNodes("documents/repos_components_db")
            If myXmlNodeList.Count > 0 Then
                Try
                    bc_cs_central_settings.repos_components_db = xnSystem.SelectSingleNode("documents/repos_components_db").InnerXml
                Catch
                    bc_cs_central_settings.repos_components_db = False
                End Try
            End If

            loaded = True
            loaded_for_rtd = True
        Catch ex As Exception
            Try
                strfn = "c:\bluecurve" + "\" + BC_CONFIG_FILE
                Dim fs As New bc_cs_file_transfer_services
                If fs.check_document_exists(strfn) = False Then
                    strfn = "c:\home\site\wwwroot" + "\" + BC_CONFIG_FILE
                End If



                xmlload.Load(strfn)


                'PR may 2012 commented this out as we dont want to
                'stop and start IIS every time we change the log file
                'If xnSystem Is Nothing Then
                xnSystem = GetSystemNode(xmlload)
                'End If

                server_flag = 1

                connection_method = xnSystem.SelectSingleNode("connectivity/method").InnerXml()

                myXmlNodeDBTypeList = xnSystem.SelectNodes("connectivity/ado/type")
                If myXmlNodeDBTypeList.Count > 0 Then
                    connection_type = xnSystem.SelectSingleNode("connectivity/ado/type").InnerXml.Trim
                Else
                    'if there is no configuration information use normal connection type.
                    connection_type = "normal"
                End If

                myXmlNodeList = xnSystem.SelectNodes("environment")
                If myXmlNodeList.Count > 0 Then
                    environment_name = xnSystem.SelectSingleNode("environment").InnerXml()
                End If

                REM security steve wooderson 03/09/2010
                myXmlNodeList = xnSystem.SelectNodes("authentication/method")
                If myXmlNodeList.Count > 0 Then
                    user_authentication_method = xnSystem.SelectSingleNode("authentication/method").InnerXml()
                Else
                    user_authentication_method = "cleartext"
                End If

                REM ldap(authentication)
                REM Path()
                myXmlNodeList = xnSystem.SelectNodes("authentication/ldap_path")
                If myXmlNodeList.Count > 0 Then
                    ldap_path = xnSystem.SelectSingleNode("authentication/ldap_path").InnerXml()
                Else
                    ldap_path = ""
                End If

                'Try
                '    myXmlNodeList = xnSystem.SelectNodes("authentication/allow_ad_prompt")
                '    If myXmlNodeList.Count > 0 Then
                '        allow_ad_prompt = xnSystem.SelectSingleNode("authentication/allow_ad_prompt").InnerXml()
                '    Else
                '        allow_ad_prompt = 0
                '    End If
                'Catch
                '    allow_ad_prompt = 0
                'End Try
                ' group
                myXmlNodeList = xnSystem.SelectNodes("authentication/ad_group")
                If myXmlNodeList.Count > 0 Then
                    ad_group = xnSystem.SelectSingleNode("authentication/ad_group").InnerXml()
                Else
                    ad_group = ""
                End If



                '' retries
                myXmlNodeList = xnSystem.SelectNodes("authentication/retries")
                If myXmlNodeList.Count > 0 Then
                    authentication_retries = xnSystem.SelectSingleNode("authentication/retries").InnerXml()
                Else
                    authentication_retries = 3
                End If

                servername = xnSystem.SelectSingleNode("connectivity/ado/server").InnerXml()
                dbname = xnSystem.SelectSingleNode("connectivity/ado/name").InnerXml()
                username = xnSystem.SelectSingleNode("connectivity/ado/user").InnerXml()
                password = xnSystem.SelectSingleNode("connectivity/ado/password").InnerXml()
                local_template_path = xnSystem.SelectSingleNode("templates/local_repos").InnerXml()
                central_template_path = xnSystem.SelectSingleNode("templates/central_repos").InnerXml()
                local_repos_path = xnSystem.SelectSingleNode("documents/local_repos").InnerXml()
                central_repos_path = xnSystem.SelectSingleNode("documents/central_repos").InnerXml()
                soap_server = xnSystem.SelectSingleNode("connectivity/soap/server").InnerXml()
                debug_level = xnSystem.SelectSingleNode("client/error_logging/level").InnerXml()
                error_file = xnSystem.SelectSingleNode("client/error_logging/file").InnerXml()
                activity_file = xnSystem.SelectSingleNode("client/activity_logging/file").InnerXml()
                trace = xnSystem.SelectSingleNode("client/activity_logging/trace").InnerXml()
                trace_sql = xnSystem.SelectSingleNode("client/activity_logging/trace_sql").InnerXml()



                REM Steve Wooderson 10/03/2011 Activity Trace
                myXmlNodeList = xnSystem.SelectNodes("client/activity_logging/trace_history_days")
                If myXmlNodeList.Count > 0 Then
                    trace_history_days = xnSystem.SelectSingleNode("client/activity_logging/trace_history_days").InnerXml
                End If
                myXmlNodeList = xnSystem.SelectNodes("client/activity_logging/trace_log_directory")
                If myXmlNodeList.Count > 0 Then
                    trace_log_archive = xnSystem.SelectSingleNode("client/activity_logging/trace_log_directory").InnerXml
                End If

                REM Steve Wooderson 24/03/2011 Search Settings 
                myXmlNodeList = xnSystem.SelectNodes("text_search/search_attributes")
                If myXmlNodeList.Count > 0 Then
                    search_attributes = xnSystem.SelectSingleNode("text_search/search_attributes").InnerXml
                End If
                myXmlNodeList = xnSystem.SelectNodes("text_search/search_attributes_list")
                If myXmlNodeList.Count > 0 Then
                    search_attributes_list = xnSystem.SelectSingleNode("text_search/search_attributes_list").InnerXml
                End If

                file_storage_method_local = xnSystem.SelectSingleNode("client/file_storage_local/method").InnerXml()
                server_flag = xnSystem.SelectSingleNode("connectivity/soap/server_flag").InnerXml()
                server_logging = xnSystem.SelectSingleNode("connectivity/soap/server_logging").InnerXml()
                server_log_file_path = xnSystem.SelectSingleNode("connectivity/soap/server_logging_path").InnerXml()
                show_authentication_form = xnSystem.SelectSingleNode("authentication/prompt").InnerXml()

                merges.Clear()
                myXmlNodeList = xnSystem.SelectNodes("custom_library/pub_types/id")
                i = 0
                For Each myxmlnode In myXmlNodeList
                    opubtypemerge = New pub_type_merges
                    opubtypemerge.pub_type_id = myxmlnode.InnerXml
                    merges.Add(opubtypemerge)
                    i = i + 1
                Next
                myXmlNodeList = xnSystem.SelectNodes("custom_library/pub_types/merge_name")
                i = 0
                For Each myxmlnode In myXmlNodeList
                    merges(i).merge_name = myxmlnode.InnerXml
                    i = i + 1
                Next
                myXmlNodeList = xnSystem.SelectNodes("custom_library/pub_types/bookmark_name")
                i = 0
                For Each myxmlnode In myXmlNodeList
                    merges(i).bookmark_name = myxmlnode.InnerXml
                    i = i + 1
                Next
                REM timeout
                myXmlNodeList = xnSystem.SelectNodes("connectivity/soap/timeout")
                If myXmlNodeList.Count > 0 Then
                    timeout = xnSystem.SelectSingleNode("connectivity/soap/timeout").InnerXml()
                End If

                REM web service retry
                bc_cs_central_settings.retry = 3
                myXmlNodeList = xnSystem.SelectNodes("connectivity/soap/retry")
                If myXmlNodeList.Count > 0 Then
                    bc_cs_central_settings.retry = xnSystem.SelectSingleNode("connectivity/soap/retry").InnerXml()
                End If

                REM proxy
                'myXmlNodeList = xnSystem.SelectNodes("connectivity/soap/proxy")
                'If myXmlNodeList.Count > 0 Then
                '    proxy = xnSystem.SelectSingleNode("connectivity/soap/proxy").InnerXml()
                'End If

                'REM pac file
                'myXmlNodeList = xnSystem.SelectNodes("connectivity/soap/pac")
                'If myXmlNodeList.Count > 0 Then
                '    pac_file = xnSystem.SelectSingleNode("connectivity/soap/pac").InnerXml()
                'End If

                REM proxy authentication
                myXmlNodeList = xnSystem.SelectNodes("connectivity/soap/authenticate")
                'If myXmlNodeList.Count > 0 Then
                '    authenticate = xnSystem.SelectSingleNode("connectivity/soap/authenticate").InnerXml()
                'End If

                REM common platform
                myXmlNodeList = xnSystem.SelectNodes("connectivity/ado/common_platform")
                If myXmlNodeList.Count > 0 Then
                    common_platform = xnSystem.SelectSingleNode("connectivity/ado/common_platform").InnerXml()
                End If
                ado_timeout = 30
                myXmlNodeList = xnSystem.SelectNodes("connectivity/ado/timeout")
                If myXmlNodeList.Count > 0 Then
                    ado_timeout = xnSystem.SelectSingleNode("connectivity/ado/timeout").InnerXml()
                End If
                REM exteded ado timeout
                extended_timeout = 600
                myXmlNodeList = xnSystem.SelectNodes("connectivity/ado/extended_timeout")
                If myXmlNodeList.Count > 0 Then
                    extended_timeout = xnSystem.SelectSingleNode("connectivity/ado/extended_timeout").InnerXml()
                End If
                REM suppress server error
                myXmlNodeList = xnSystem.SelectNodes("connectivity/soap/suppress_server_error")
                suppress_server_error = 0
                REM external data
                myXmlNodeList = xnSystem.SelectNodes("connectivity/ado/external_data/external_web_service_url")
                If myXmlNodeList.Count > 0 Then
                    bc_cs_central_settings.external_web_serivce_url = xnSystem.SelectSingleNode("connectivity/ado/external_data/external_web_service_url").InnerXml
                End If
                myXmlNodeList = xnSystem.SelectNodes("connectivity/ado/external_data/log_external_data")
                If myXmlNodeList.Count > 0 Then
                    bc_cs_central_settings.log_external_data = xnSystem.SelectSingleNode("connectivity/ado/external_data/log_external_data").InnerXml
                End If

                REM Steve Wooderson 11/01/2012 External File Settings 
                myXmlNodeList = xnSystem.SelectNodes("documents/external_files_path")
                If myXmlNodeList.Count > 0 Then
                    bc_cs_central_settings.external_files_path = xnSystem.SelectSingleNode("documents/external_files_path").InnerXml
                Else
                    bc_cs_central_settings.external_files_path = bc_cs_central_settings.central_repos_path
                End If

                bc_cs_central_settings.parallel_refresh_mode = False
                myXmlNodeList = xnSystem.SelectNodes("parallelisation/mode")
                If myXmlNodeList.Count > 0 Then
                    Try
                        bc_cs_central_settings.parallel_refresh_mode = xnSystem.SelectSingleNode("parallelisation/mode").InnerXml
                    Catch

                    End Try
                End If
                myXmlNodeList = xnSystem.SelectNodes("parallelisation/num_min_idle_thread")
                If myXmlNodeList.Count > 0 Then
                    Try
                        bc_cs_central_settings.num_min_idle_thread = xnSystem.SelectSingleNode("parallelisation/num_min_idle_thread").InnerXml
                    Catch

                    End Try
                End If
                myXmlNodeList = xnSystem.SelectNodes("documents/save_doc_metadata")
                If myXmlNodeList.Count > 0 Then
                    Try
                        bc_cs_central_settings.save_doc_metadata = xnSystem.SelectSingleNode("documents/save_doc_metadata").InnerXml
                    Catch

                    End Try
                End If
                myXmlNodeList = xnSystem.SelectNodes("documents/doc_metadata_path")
                If myXmlNodeList.Count > 0 Then
                    Try
                        bc_cs_central_settings.doc_metadata_path = xnSystem.SelectSingleNode("documents/doc_metadata_path").InnerXml
                    Catch

                    End Try
                End If
                read_smtp_gateways()


                myXmlNodeList = xnSystem.SelectNodes("connectivity/soap/log_user_session")
                If myXmlNodeList.Count > 0 Then
                    Try
                        bc_cs_central_settings.log_user_session = xnSystem.SelectSingleNode("connectivity/soap/log_user_session").InnerXml
                    Catch
                        bc_cs_central_settings.log_user_session = False
                    End Try
                End If

                myXmlNodeList = xnSystem.SelectNodes("connectivity/soap/wcf_binding ")
                bc_cs_central_settings.wcf_binding = "binary"
                If myXmlNodeList.Count > 0 Then
                    Try
                        bc_cs_central_settings.wcf_binding = xnSystem.SelectSingleNode("connectivity/soap/wcf_binding ").InnerXml
                    Catch
                        bc_cs_central_settings.wcf_binding = "binary"
                    End Try
                End If



                bc_cs_central_settings.repos_file_system = True

                myXmlNodeList = xnSystem.SelectNodes("documents/repos_file_system")
                If myXmlNodeList.Count > 0 Then
                    Try
                        bc_cs_central_settings.repos_file_system = xnSystem.SelectSingleNode("documents/repos_file_system").InnerXml
                    Catch
                        bc_cs_central_settings.repos_file_system = True
                    End Try
                End If

                bc_cs_central_settings.repos_db = False

                myXmlNodeList = xnSystem.SelectNodes("documents/repos_db")
                If myXmlNodeList.Count > 0 Then
                    Try
                        bc_cs_central_settings.repos_db = xnSystem.SelectSingleNode("documents/repos_db").InnerXml
                    Catch
                        bc_cs_central_settings.repos_db = False
                    End Try
                End If


                bc_cs_central_settings.repos_html_db = False

                myXmlNodeList = xnSystem.SelectNodes("documents/repos_html_db")
                If myXmlNodeList.Count > 0 Then
                    Try
                        bc_cs_central_settings.repos_html_db = xnSystem.SelectSingleNode("documents/repos_html_db").InnerXml
                    Catch
                        bc_cs_central_settings.repos_html_db = False
                    End Try
                End If

                bc_cs_central_settings.repos_components_db = False

                myXmlNodeList = xnSystem.SelectNodes("documents/repos_components_db")
                If myXmlNodeList.Count > 0 Then
                    Try
                        bc_cs_central_settings.repos_components_db = xnSystem.SelectSingleNode("documents/repos_components_db").InnerXml
                    Catch
                        bc_cs_central_settings.repos_components_db = False
                    End Try
                End If

                loaded = True
            Catch exx As Exception
                Try
                    strfn = "c:\bluecurve" + "\" + BC_CONFIG_FILE
                    Dim cfs As New bc_cs_file_transfer_services
                    Dim fs As StreamWriter
                    Try
                        fs = New StreamWriter("c:\bluecurve\config_file_error.txt", FileMode.Create)
                    Catch
                        fs = New StreamWriter("c:\home\site\wwwroot\config_file_error.txt", FileMode.Create)
                    End Try
                    Dim str As String
                    str = "config file failed to load at: " + Format(Now, "dd-MM-yyyy HH:mm:ss") + ":" + exx.Message
                    fs.WriteLine(str)
                    fs.Close()
                Catch
                    'MsgBox("Config File Error: " + Err.Description, MsgBoxStyle.Critical, "Blue Curve")
                End Try
                bloaderror = True
            End Try
        End Try

    End Sub
    Public Sub read_services()
        REM email
        Dim myXmlNodeList As Xml.XmlNodeList
        myXmlNodeList = xnSystem.SelectNodes("services/email")
        If myXmlNodeList.Count = 1 Then

            Try
                bc_cs_central_settings.email_service_enabled = myXmlNodeList(0).SelectSingleNode("email_service_enabled").InnerXml
            Catch
                bc_cs_central_settings.email_service_enabled = False
            End Try

            Try
                bc_cs_central_settings.email_service_poll_interval = CInt(myXmlNodeList(0).SelectSingleNode("email_service_poll_Interval").InnerXml)
            Catch
                bc_cs_central_settings.email_service_poll_interval = 5000
            End Try

            Try
                bc_cs_central_settings.email_service_maximum_concurrent_threads = CInt(myXmlNodeList(0).SelectSingleNode("email_service_maximum_concurrent_threads").InnerXml)
            Catch
                bc_cs_central_settings.email_service_maximum_concurrent_threads = 64
            End Try

        End If
        REM Email Preview
        myXmlNodeList = xnSystem.SelectNodes("services/email_preview")
        If myXmlNodeList.Count = 1 Then

            Try
                bc_cs_central_settings.email_preview_service_enabled = myXmlNodeList(0).SelectSingleNode("email_preview_service_enabled").InnerXml
            Catch
                bc_cs_central_settings.email_preview_service_enabled = False
            End Try

            Try
                bc_cs_central_settings.email_preview_service_poll_interval = CInt(myXmlNodeList(0).SelectSingleNode("email_preview_service_poll_Interval").InnerXml)
            Catch
                bc_cs_central_settings.email_preview_service_poll_interval = 5000
            End Try

        End If

        REM Email Preview
        myXmlNodeList = xnSystem.SelectNodes("services/mailing_list")
        If myXmlNodeList.Count = 1 Then

            Try
                bc_cs_central_settings.mailing_list_service_enabled = myXmlNodeList(0).SelectSingleNode("mailing_list_service_enabled").InnerXml
            Catch
                bc_cs_central_settings.mailing_list_service_enabled = False
            End Try

            Try
                bc_cs_central_settings.mailing_list_service_poll_interval = CInt(myXmlNodeList(0).SelectSingleNode("mailing_list_service_poll_Interval").InnerXml)
            Catch
                bc_cs_central_settings.mailing_list_service_poll_interval = 5000
            End Try

        End If

        REM Email Preview
        myXmlNodeList = xnSystem.SelectNodes("services/distribution")
        If myXmlNodeList.Count = 1 Then

            Try
                bc_cs_central_settings.distribution_service_enabled = myXmlNodeList(0).SelectSingleNode("distribution_service_enabled").InnerXml
            Catch
                bc_cs_central_settings.distribution_service_enabled = False
            End Try

            Try
                bc_cs_central_settings.distribution_service_poll_interval = CInt(myXmlNodeList(0).SelectSingleNode("distribution_service_poll_Interval").InnerXml)
            Catch
                bc_cs_central_settings.distribution_service_poll_interval = 5000
            End Try

        End If
        REM Email Preview
        myXmlNodeList = xnSystem.SelectNodes("services/aggregation")
        If myXmlNodeList.Count = 1 Then

            Try
                bc_cs_central_settings.aggregation_service_enabled = myXmlNodeList(0).SelectSingleNode("aggregation_service_enabled").InnerXml
            Catch
                bc_cs_central_settings.aggregation_service_enabled = False
            End Try


            Try
                bc_cs_central_settings.aggregation_service_poll_interval = CInt(myXmlNodeList(0).SelectSingleNode("aggregation_service_poll_Interval").InnerXml)
            Catch
                bc_cs_central_settings.aggregation_service_poll_interval = 5000
            End Try
            Try
                bc_cs_central_settings.aggregation_service_num_active_threads = CInt(myXmlNodeList(0).SelectSingleNode("aggregation_service_num_active_threads").InnerXml)
            Catch
                bc_cs_central_settings.aggregation_service_num_active_threads = 32
            End Try
            Try
                bc_cs_central_settings.aggregation_service_entities_per_batch = CInt(myXmlNodeList(0).SelectSingleNode("aggregation_service_entities_per_batch").InnerXml)
            Catch
                bc_cs_central_settings.aggregation_service_entities_per_batch = 20
            End Try
            Try
                bc_cs_central_settings.aggregation_service_run_universes_concurrently = CInt(myXmlNodeList(0).SelectSingleNode("aggregation_service_run_universes_concurrently").InnerXml)
            Catch
                bc_cs_central_settings.aggregation_service_run_universes_concurrently = False
            End Try
            Try
                bc_cs_central_settings.aggregation_service_run_under_IIS = CInt(myXmlNodeList(0).SelectSingleNode("aggregation_service_run_under_IIS").InnerXml)
            Catch
                bc_cs_central_settings.aggregation_service_run_under_IIS = False
            End Try
        End If

        myXmlNodeList = xnSystem.SelectNodes("services/calcs")
        If myXmlNodeList.Count = 1 Then

            Try
                bc_cs_central_settings.calc_service_enabled = myXmlNodeList(0).SelectSingleNode("calc_service_enabled").InnerXml
            Catch
                bc_cs_central_settings.calc_service_enabled = False
            End Try


            Try
                bc_cs_central_settings.calc_service_poll_interval = CInt(myXmlNodeList(0).SelectSingleNode("calc_service_poll_interval").InnerXml)
            Catch
                bc_cs_central_settings.calc_service_poll_interval = 8000
            End Try
            Try
                bc_cs_central_settings.calc_service_num_active_threads = CInt(myXmlNodeList(0).SelectSingleNode("calc_service_num_active_threads").InnerXml)
            Catch
                bc_cs_central_settings.calc_service_num_active_threads = 32
            End Try
            Try
                bc_cs_central_settings.calc_service_entities_per_batch = CInt(myXmlNodeList(0).SelectSingleNode("calc_service_entities_per_batch").InnerXml)
            Catch
                bc_cs_central_settings.calc_service_entities_per_batch = 100
            End Try
            Try
                bc_cs_central_settings.calc_service_run_entities_concurrently = CInt(myXmlNodeList(0).SelectSingleNode("calc_service_run_requests_concurrently").InnerXml)
            Catch
                bc_cs_central_settings.calc_service_run_entities_concurrently = False
            End Try
            Try
                bc_cs_central_settings.calc_service_run_under_IIS = CInt(myXmlNodeList(0).SelectSingleNode("calc_service_run_under_IIS").InnerXml)
            Catch
                bc_cs_central_settings.calc_service_run_under_IIS = False
            End Try
        End If
        myXmlNodeList = xnSystem.SelectNodes("services/data_load")
        If myXmlNodeList.Count = 1 Then

            Try
                bc_cs_central_settings.data_load_service_enabled = myXmlNodeList(0).SelectSingleNode("data_load_service_enabled").InnerXml
            Catch
                bc_cs_central_settings.data_load_service_enabled = False
            End Try


            Try
                bc_cs_central_settings.data_load_poll_interval = CInt(myXmlNodeList(0).SelectSingleNode("data_load_poll_interval").InnerXml)
            Catch
                bc_cs_central_settings.data_load_poll_interval = 5000
            End Try
            Try
                bc_cs_central_settings.data_load_run_under_IIS = CInt(myXmlNodeList(0).SelectSingleNode("data_load_run_under_IIS").InnerXml)
            Catch
                bc_cs_central_settings.data_load_run_under_IIS = False
            End Try

        End If

        REM bulk email
        myXmlNodeList = xnSystem.SelectNodes("services/bulk_email")
        If myXmlNodeList.Count = 1 Then

            Try
                bc_cs_central_settings.bulk_email_service_enabled = myXmlNodeList(0).SelectSingleNode("bulk_email_service_enabled").InnerXml
            Catch
                bc_cs_central_settings.bulk_email_service_enabled = False
            End Try

            Try
                bc_cs_central_settings.bulk_email_service_poll_interval = CInt(myXmlNodeList(0).SelectSingleNode("bulk_email_service_poll_Interval").InnerXml)
            Catch
                bc_cs_central_settings.bulk_email_service_poll_interval = 5000
            End Try

            Try
                bc_cs_central_settings.bulk_email_service_maximum_concurrent_threads = CInt(myXmlNodeList(0).SelectSingleNode("bulk_email_service_maximum_concurrent_threads").InnerXml)
            Catch
                bc_cs_central_settings.bulk_email_service_maximum_concurrent_threads = 64
            End Try
            Try
                bc_cs_central_settings.bulk_email_service_api_key = myXmlNodeList(0).SelectSingleNode("bulk_email_service_api_key ").InnerXml
            Catch
                bc_cs_central_settings.bulk_email_service_api_key = ""
            End Try

        End If
    End Sub
    Public Sub read_smtp_gateways()
        read_services()
        Dim myXmlNodeList As Xml.XmlNodeList
        Dim gw As bc_cs_email_services.bc_cs_smtp_gateway
        myXmlNodeList = xnSystem.SelectNodes("smtpgateways/gateway")
        For i = 0 To myXmlNodeList.Count - 1
            gw = New bc_cs_email_services.bc_cs_smtp_gateway

            Try
                gw._host = myXmlNodeList(i).SelectSingleNode("host").InnerXml
                Try
                    gw._port = CInt(myXmlNodeList(i).SelectSingleNode("port").InnerXml)
                Catch
                    gw._port = 0
                End Try
                Try
                    gw._async = myXmlNodeList(i).SelectSingleNode("async").InnerXml
                Catch
                    gw._async = False
                End Try
                Try
                    gw._delivery_method = CInt(myXmlNodeList(i).SelectSingleNode("delivery_method").InnerXml)
                Catch
                    gw._delivery_method = SmtpDeliveryMethod.Network
                End Try
                gw._password = myXmlNodeList(i).SelectSingleNode("password").InnerXml

                Try
                    gw._timeout = CInt(myXmlNodeList(i).SelectSingleNode("timeout").InnerXml)
                Catch
                    gw._timeout = 60
                End Try
                Try
                    gw._userdefaultcredentials = CBool(myXmlNodeList(i).SelectSingleNode("userdefaultcredentials").InnerXml)
                Catch
                    gw._userdefaultcredentials = False
                End Try
                Try
                    gw._enable_ssl = myXmlNodeList(i).SelectSingleNode("enable_ssl").InnerXml
                Catch
                    gw._enable_ssl = False
                End Try

                gw._username = myXmlNodeList(i).SelectSingleNode("username").InnerXml
                smtp_gateways.Add(gw)

            Catch ex As Exception
                Dim ocomm As New bc_cs_activity_log("bc_cs_central_settings", "read_smtp_gateways", bc_cs_activity_codes.COMMENTARY, "Failed to read host or port:" + gw._host + ": " + ex.Message)
            End Try
        Next
    End Sub
    Public Function RunRegistrycmd() As Boolean
        Dim result As Boolean = False
        Try
            Dim proc As System.Diagnostics.Process
            Dim winroot As String
            winroot = System.Environment.GetEnvironmentVariable("SystemRoot")
            Dim strCmdLine As String
            strCmdLine = winroot & "\regedit.exe"
            Dim strCmdParam As String
            Dim strRegFileName As String
            strRegFileName = ControlChars.Quote & bc_cs_central_settings.get_user_dir() & "\Binary\setup.reg" & ControlChars.Quote
            strCmdParam = " /s /c " & strRegFileName
            'proc = System.Diagnostics.Process.Start(strCmdLine, strCmdParam)
            proc = System.Diagnostics.Process.Start(strCmdLine, strCmdParam)
            '  proc.Close()
            result = True
        Catch ex As Exception
            Dim slog As New bc_cs_activity_log("blucurve.create.cs :error in ", "RunRegistrycmd .", CStr(bc_cs_activity_codes.COMMENTARY), ex.Message)
            result = False
        Finally
            RunRegistrycmd = result
        End Try
    End Function
    Public Function ExportRegistryKeys() As Boolean
        Dim strpath As String = ""
        Dim blnResult As Boolean = True
        Try
            ' strpath = Environment.GetEnvironmentVariable("BCConfigurationFilePath")
            Dim regLocalMachine As RegistryKey
            regLocalMachine = Registry.LocalMachine.OpenSubKey("SOFTWARE\Blue Curve\Installer", False)
            If Not regLocalMachine Is Nothing Then
                blnResult = Me.RunRegistrycmd()
            End If
        Catch ex As Exception
            blnResult = False
        Finally
            'MsgBox("aa: " + strpath)
            ExportRegistryKeys = blnResult
        End Try
    End Function
    Public Shared Function get_user_dir() As String
        Dim strpath As String = ""
        Try
            Dim strProfileDirectory As String = get_profile_dir()

            Dim strAppConfig As String = "app.config"
            If ApplicationName = DEPLOYMENT_MANAGER_APPLICATION_NAME AndAlso File.Exists(strAppConfig) Then
                Dim xd As New XmlDocument
                xd.Load(strAppConfig)
                For Each xn As XmlNode In xd.SelectNodes("//add")
                    If Not xn.Attributes("key") Is Nothing AndAlso Not xn.Attributes("value") Is Nothing AndAlso xn.Attributes("key").Value = "TargetDirectory" AndAlso Directory.Exists(xn.Attributes("value").Value) AndAlso File.Exists(xn.Attributes("value").Value & "\" & BC_CONFIG_FILE) Then
                        strpath = xn.Attributes("value").Value
                    End If
                Next
            End If

            If strpath Is Nothing Or strpath.Trim = "" Then
                Dim regLocalMachine As RegistryKey
                regLocalMachine = Registry.LocalMachine.OpenSubKey("SOFTWARE\Blue Curve\BCConfigurationFile", False)
                If Not regLocalMachine Is Nothing Then
                    strpath = regLocalMachine.GetValue("Path")
                End If
            End If
            'if there is no path value then use default user profile's directory to locate config file
            If strpath Is Nothing Or strpath.Trim = "" Then
                strpath = strProfileDirectory
            End If
        Catch ex As Exception
            strpath = get_profile_dir()
        Finally
            'MsgBox("aa: " + strpath)
            get_user_dir = strpath
        End Try
    End Function
    Public Shared Function get_profile_dir() As String
        Dim strpath As String
        Try

            Dim regappdata As RegistryKey
            Dim regada As RegistryKey
            Dim regcurrentuser As RegistryKey = Registry.CurrentUser

            Try

                regappdata = regcurrentuser.OpenSubKey("software\Microsoft\Windows\CurrentVersion\Explorer\User Shell Folders")
                regada = regappdata.OpenSubKey("AppData")
                strpath = regappdata.GetValue("AppData")
                If strpath.Trim <> "" Then
                    strpath = strpath + "\" + BC_APPLICATION_USER_AREA
                End If
            Catch
                regappdata = regcurrentuser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders")
                regada = regappdata.OpenSubKey("AppData")
                strpath = regappdata.GetValue("AppData")
                If strpath.Trim <> "" Then
                    strpath = strpath + "\" + BC_APPLICATION_USER_AREA
                End If

            End Try
            get_profile_dir = strpath
        Catch ex As Exception
            MsgBox("Config  File Error: " + ex.Message, MsgBoxStyle.Critical, "Blue Curve")
            Return ""
        End Try
    End Function
    Public Shared Function GetLoginName() As String
        Dim fs As New bc_cs_file_transfer_services
        Dim xml As New bc_cs_data_services
        Try
            REM is cookie file exists get from here
            REM else get from oS
            'If bc_cs_central_settings.server_flag = 0 Then
            '    If fs.check_document_exists_and_can_be_read(bc_cs_central_settings.get_user_dir + "\bc_current_user.dat", Nothing) Then
            '        ouser = xml.soap_deserialize_file_to_object(bc_cs_central_settings.get_user_dir + "\bc_current_user.dat", Nothing, False)
            '        GetLoginName = ouser.os_user_name
            '    Else
            '        GetLoginName = Environment.UserName
            '    End If
            'Else
            GetLoginName = Environment.UserName
            'End If
        Catch ex As Exception

            GetLoginName = Environment.UserName

        End Try
    End Function
    Public Shared Function GetLoginNameos() As String
        Dim fs As New bc_cs_file_transfer_services
        Dim xml As New bc_cs_data_services

        REM is cookie file exists get from here
        GetLoginNameos = Environment.UserName
    End Function
    <Serializable()> Public Class bc_om_user

        Public id As Long
        Public os_user_name As String
        Public surname As String
        Public first_name As String
        Public sync_level As Integer
        Public role As String
        Public user_name As String
        Public password As String
        Public no_display_taxonomy As Boolean

        Public Sub New()

        End Sub
    End Class
    Public Function check_connection(ByVal method As String, ByVal soap_connection As Boolean) As Boolean
        '   Dim slog = New bc_cs_activity_log("bc_cs_central_settings", "check_connection", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            '  Dim ocommentary As bc_cs_activity_log
            Dim bc_cs_central_settings As New bc_cs_central_settings

            check_connection = False
            bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.LOCAL

            If method = bc_cs_central_settings.ADO Then
                Dim dbc As New bc_cs_db_services(True)
                Dim sqlcn As New SqlConnection
                sqlcn.ConnectionString = dbc.GetConnectionString(Nothing)
                sqlcn.Open()
                'Check the connection to see if all is working fine otherwise use soap.
                If sqlcn.State = 1 Then
                    bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO
                    check_connection = True
                    sqlcn.Close()
                Else
                    If soap_connection = True Then
                        check_connection = True
                        bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP
                    End If
                End If
            Else
                If method = bc_cs_central_settings.SOAP Then
                    If soap_connection = True Then
                        check_connection = True
                        bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP
                    End If
                Else
                    '   slog = New bc_cs_activity_log("bc_am_load", "check_connection", bc_cs_activity_codes.COMMENTARY, "Connection Method: " + method + " not supported System can only work off-line")
                    check_connection = False
                End If
            End If
            If check_connection = False Then
                bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.LOCAL
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_cs_central_settings", "check_connection", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            ' slog = New bc_cs_activity_log("bc_cs_central_settings", "check_connection", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
    Public Sub system_shutdown()
        Try
            bc_cs_central_settings.logging_thread.Abort()
        Catch
        End Try
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Public Shared Function userOfficeStatus() As Integer

        REM SW cope with office versions
        '1 office 2003 or less
        '2 office 2007 or above
        '3 office 2007 but use as 2003

        Dim strpath As String

        Dim regcurrentuser As RegistryKey
        Dim regappdata As RegistryKey
        Dim myFileVersionInfo As FileVersionInfo

        If bc_cs_central_settings.user_office_version = 0 Then
            'Find which office user has
            Try
                regcurrentuser = Registry.CurrentUser
                regappdata = regcurrentuser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\App Paths\Winword.exe")
                strpath = regappdata.GetValue("path").ToString
                myFileVersionInfo = FileVersionInfo.GetVersionInfo(strpath + "\winword.exe")
                bc_cs_central_settings.user_office_version = myFileVersionInfo.FileMajorPart
            Catch
                Try
                    regcurrentuser = Registry.LocalMachine
                    regappdata = regcurrentuser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\App Paths\Winword.exe")
                    strpath = regappdata.GetValue("path").ToString
                    myFileVersionInfo = FileVersionInfo.GetVersionInfo(strpath + "\winword.exe")
                    bc_cs_central_settings.user_office_version = myFileVersionInfo.FileMajorPart
                Catch
                    bc_cs_central_settings.user_office_version = 11
                End Try
            End Try

            'Try
            '    'Find which office user has
            '    Dim regcurrentuser As RegistryKey = Registry.LocalMachine
            '    Dim regappdata As RegistryKey = regcurrentuser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\App Paths\Winword.exe")
            '    strpath = regappdata.GetValue("path").ToString
            '    Dim myFileVersionInfo As FileVersionInfo = FileVersionInfo.GetVersionInfo(strpath + "\winword.exe")
            '    bc_cs_central_settings.user_office_version = myFileVersionInfo.FileMajorPart
            'Catch
            '    bc_cs_central_settings.user_office_version = 11
            'End Try

        End If

        Try
            If bc_cs_central_settings.office_version < 12 And bc_cs_central_settings.user_office_version >= 12 Then
                userOfficeStatus = 3
            ElseIf bc_cs_central_settings.office_version < 12 Then
                userOfficeStatus = 1
            ElseIf bc_cs_central_settings.office_version >= 12 Then
                userOfficeStatus = 2
            End If
        Catch
            userOfficeStatus = 1
        End Try


    End Function


End Class
REM ==========================================
REM Bluecurve Limited 2005
REM Module:       File Services
REM Type:         CS
REM Description:  file copy
REM Version:      1
REM Change history
REM ==========================================
Public Class bc_cs_file_transfer_services
    Public updated_om As Boolean = False
    Public err_text As String
    Public Function delete_file(ByVal fn As String, Optional ByVal certificate As bc_cs_security.certificate = Nothing, Optional ByVal show_error As Boolean = True) As String
        '  Dim otrace As New bc_cs_activity_log("bc_cs_file_transfer_services", "delete_file", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            '  Dim ocommentary As New bc_cs_activity_log("bc_cs_file_transfer_services", "delete_file", bc_cs_activity_codes.COMMENTARY, "Attempting to Delete file: " + fn)
            delete_file = False
            Kill(fn)
            delete_file = True
        Catch ex As Exception
            REM is in use by another process flag this
            If InStr(ex.Message, "used by another process.", CompareMethod.Text) > 1 Then
                show_error = False
                delete_file = "in use"
            End If
            If show_error = True Then
                Dim db_err As New bc_cs_error_log("bc_cs_file_transfer_services", "delete_file", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            End If
            delete_file = False
        Finally
            '     otrace = New bc_cs_activity_log("bc_cs_file_transfer_services", "delete_file", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function




    Public Function file_copy(ByVal source As String, ByVal destination As String, Optional ByVal show_no_message As Boolean = False) As Integer
        '  Dim otrace As New bc_cs_activity_log("bc_cs_file_transfer_services", "file_Copy", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            '  Dim ocommentary As bc_cs_activity_log
            file_copy = bc_cs_error_codes.RETURN_ERROR
            REM copy file using file transfer method
            If bc_cs_central_settings.file_transfer_method = "" Or bc_cs_central_settings.file_transfer_method = bc_cs_central_settings.UNC Then
                '    ocommentary = New bc_cs_activity_log("bc_cs_file_transfer_services", "file_copy", bc_cs_activity_codes.COMMENTARY, "Attempting to transfer file: " + source + " to destination: " + destination + " using:UNC")
                FileCopy(source, destination)
                '   ocommentary = New bc_cs_activity_log("bc_cs_file_transfer_services", "file_copy", bc_cs_activity_codes.COMMENTARY, "File Transfer Successful: " + source + " to destination: " + destination + " using:UNC")
                file_copy = 0
            Else
                '    ocommentary = New bc_cs_activity_log("bc_cs_file_transfer_services", "file_copy", bc_cs_activity_codes.COMMENTARY, "File Transfer Method: " + bc_cs_central_settings.file_transfer_method + " not supported.")
                If show_no_message = False Then
                    Dim omessage = New bc_cs_message("Blue Curve create", "File Transfer Method: " + bc_cs_central_settings.file_transfer_method + " not supported.", bc_cs_message.MESSAGE)
                End If
            End If
        Catch ex As Exception
            If show_no_message = False Then
                Dim db_err As New bc_cs_error_log("bc_cs_file_transfer_services", "file_copy", bc_cs_error_codes.USER_DEFINED, ex.Message)
            End If
        Finally
            '    otrace = New bc_cs_activity_log("bc_cs_file_transfer_services", "file_Copy", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

    Public Function file_copy_report_error(ByVal source As String, ByVal destination As String) As String
        file_copy_report_error = ""
        Try
            FileCopy(source, destination)
        Catch ex As Exception
            file_copy_report_error = ex.Message
        End Try

    End Function

    Public Function delete_file_report_error(ByVal fn As String) As String
        delete_file_report_error = ""
        Try
            Kill(fn)
            delete_file_report_error = True
        Catch ex As Exception
            delete_file_report_error = ex.Message
        End Try
    End Function


    REM writes bytestream interpretation of document to actual file
    Public Function write_document_to_bytestream_doc_open(ByVal filename As String, ByRef bytedoc As Byte(), ByRef certificate As bc_cs_security.certificate, Optional ByVal show_error As Boolean = True) As Boolean
        Dim otrace As New bc_cs_activity_log("bc_cs_file_transfer_services", "write_document_to_bytestream_doc_open", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Dim tfile As String = bc_cs_central_settings.local_repos_path + "keepopen.txt"
            file_copy(filename, tfile)
            write_document_to_bytestream_doc_open = write_document_to_bytestream(tfile, bytedoc, certificate, show_error)
            delete_file(tfile, certificate, show_error)

        Catch ex As Exception
            write_document_to_bytestream_doc_open = False
            If show_error = True Then
                Dim db_err As New bc_cs_error_log("bc_cs_file_transfer_services", "write_document_to_bytestream_doc_open", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            Else
                err_text = "bc_cs_file_transfer_services : write_document_to_bytestream_doc_open " + ex.Message
            End If
        Finally
            otrace = New bc_cs_activity_log("bc_cs_file_transfer_services", "write_document_to_bytestream_doc_open", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Function
    REM FIL 5.2 AUG 2014

    Public Function write_document_to_bytestream(ByVal filename As String, ByRef bytedoc As Byte(), ByRef certificate As bc_cs_security.certificate, Optional ByVal show_error As Boolean = True) As Boolean
        Dim otrace As New bc_cs_activity_log("bc_cs_file_transfer_services", "write_document_to_bytestream", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Dim commentary As New bc_cs_activity_log("bc_cs_file_transfer_services", "write_document_to_bytestream", bc_cs_activity_codes.COMMENTARY, "Attempting to write file: " + filename + " to byte stream.", certificate)
            Dim locked As Boolean = True
            write_document_to_bytestream = False
            Dim i As Integer = 1

            'While locked = True
            Try
                write_document_to_bytestream = True
                Dim fs As New FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read, 65536, True)
                locked = False
                ReDim bytedoc(CInt(fs.Length) - 1)
                Using fs
                    fs.Read(bytedoc, 0, UBound(bytedoc) + 1)
                    fs.Close()
                End Using

                write_document_to_bytestream = True
            Catch ex As Exception
                If InStr(ex.Message, "being used by another process.", CompareMethod.Text) = 0 Then
                    write_document_to_bytestream = False
                    If show_error = True Then
                        Dim db_err As New bc_cs_error_log("bc_cs_file_transfer_services", "write_document_to_bytestream", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
                    End If
                    locked = False
                Else
                    write_document_to_bytestream = False
                    REM FIL FEB 2013
                    If show_error = True Then
                        Dim omsg As New bc_cs_message("Blue Curve", "The document " + filename + " is open please close and retry.", bc_cs_message.MESSAGE)
                        'Dim ddb_err = New bc_cs_error_log("bc_cs_file_transfer_services", "write_document_to_bytestream", bc_cs_error_codes.USER_DEFINED, "Failed to read document as document  is open (" + filename + ") Please close and try again.", certificate)
                    End If
                End If
            End Try


        Catch ex As Exception
            write_document_to_bytestream = False
            If show_error = True Then
                Dim db_err As New bc_cs_error_log("bc_cs_file_transfer_services", "write_document_to_bytestream", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            Else
                err_text = "bc_cs_file_transfer_services : write_document_to_bytestream " + ex.Message
            End If
        Finally
            otrace = New bc_cs_activity_log("bc_cs_file_transfer_services", "write_document_to_bytestream", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Function
    Public Function in_use_by_another_process(ByVal filename As String, ByRef certificate As bc_cs_security.certificate) As Boolean
        Dim otrace As New bc_cs_activity_log("bc_cs_file_transfer_services", "in_use_by_another_process", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            in_use_by_another_process = False
            Dim bytedoc As Byte()
            Dim commentary As New bc_cs_activity_log("bc_cs_file_transfer_services", "write_document_to_bytestream", bc_cs_activity_codes.COMMENTARY, "Attempting to write file: " + filename + " to byte stream.", certificate)
            Dim locked As Boolean = True
            Dim fs As New FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read, 65536, True)
            locked = False
            ReDim bytedoc(CInt(fs.Length) - 1)
            fs.Read(bytedoc, 0, UBound(bytedoc) + 1)
            fs.Close()
        Catch ex As Exception

            If InStr(ex.Message, "being used by another process.", CompareMethod.Text) <> 0 Then
                in_use_by_another_process = True
            End If

        Finally
            otrace = New bc_cs_activity_log("bc_cs_file_transfer_services", "in_use_by_another_process", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Function
    REM FIL 5.2 August 2014
    Public Function write_bytestream_to_document_readonly(ByVal filename As String, ByRef bytedoc As Byte(), ByRef certificate As bc_cs_security.certificate, Optional ByVal show_err As Boolean = True) As Boolean
        'Dim otrace As New bc_cs_activity_log("bc_cs_file_transfer_services", "write_bytestream_to_document", bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim locked As Boolean = True
        Dim i As Integer = 1

        write_bytestream_to_document_readonly = False

        While locked = True
            Try
                'Dim commentary As New bc_cs_activity_log("bc_cs_file_transfer_services", "write_bytestream_to_document", bc_cs_activity_codes.COMMENTARY, "Attempting to write byte stream to file: " + filename)
                If check_document_exists(filename) Then
                    System.IO.File.SetAttributes(filename, IO.FileAttributes.Normal)
                End If
                Dim fs As New FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.Read, 65536, False)
                fs.Write(bytedoc, 0, UBound(bytedoc) + 1)
                fs.Close()
                System.IO.File.SetAttributes(filename, IO.FileAttributes.ReadOnly)
                locked = False
                write_bytestream_to_document_readonly = True


            Catch ex As Exception
                If InStr(ex.Message, "mapped", CompareMethod.Text) > 0 Then
                    If bc_cs_central_settings.server_flag = 0 Then
                        'Dim omsg As New bc_cs_message("Blue Curve", "Failed to synchronize document as template is open (" + filename + ") please close all documents and restart application", bc_cs_message.MESSAGE)
                        Dim commentary As New bc_cs_activity_log("bc_cs_file_transfer_services", "write_bytestream_to_document_readonly", bc_cs_activity_codes.COMMENTARY, "Error: Failed to synchronize document as template is open (" + filename + ")", certificate)
                    End If
                    locked = False
                ElseIf InStr(ex.Message, "being used by another process.", CompareMethod.Text) <> 0 Then
                    If bc_cs_central_settings.server_flag = 0 Then
                        While i <= 4
                            If i = 4 Then
                                'Dim omsgFinal As New bc_cs_message("Blue Curve", "Failed to synchronize document as template is open (" + filename + "), document has not been synchronized", bc_cs_message.MESSAGE)
                                Dim commentary As New bc_cs_activity_log("bc_cs_file_transfer_services", "write_bytestream_to_document_readonly", bc_cs_activity_codes.COMMENTARY, "Error: Failed to synchronize document as template is open (" + filename + "), document has not been synchronized", certificate)
                                locked = False
                                Exit While
                            End If
                            'Dim omsg As New bc_cs_message("Blue Curve", "Failed to synchronize document as template is open (" + filename + ") please close the document and click ok", bc_cs_message.MESSAGE)
                            Dim commentary2 = New bc_cs_activity_log("bc_cs_file_transfer_services", "write_bytestream_to_document_readonly", bc_cs_activity_codes.COMMENTARY, "Error: Failed to synchronize document as template is open (" + filename + ")", certificate)
                            i = i + 1
                            Exit While
                        End While
                    Else
                        locked = False
                    End If
                Else
                    If show_err = True Then
                        Dim db_err As New bc_cs_error_log("bc_cs_file_transfer_services", "write_bytestream_to_document_readonly", bc_cs_error_codes.USER_DEFINED, ex.Message + ": " + filename, certificate)
                    Else
                        Me.err_text = "bc_cs_file_transfer_services: write_bytestream_to_document " + ex.Message

                    End If
                    locked = False

                End If
            Finally
                'otrace = New bc_cs_activity_log("bc_cs_file_transfer_services", "write_bytestream_to_document", bc_cs_activity_codes.TRACE_EXIT, "")
            End Try

        End While
    End Function
    REM writes bytestream interpretation of document to actual file
    Public Function write_bytestream_to_document(ByVal filename As String, bytedoc As Byte(), ByRef certificate As bc_cs_security.certificate, Optional ByVal show_err As Boolean = True) As Boolean

        '    Public Function write_bytestream_to_document(ByVal filename As String, ByRef bytedoc As Byte(), ByRef certificate As bc_cs_security.certificate, Optional ByVal show_err As Boolean = True) As Boolean
        'Dim otrace As New bc_cs_activity_log("bc_cs_file_transfer_services", "write_bytestream_to_document", bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim locked As Boolean = True
        Dim i As Integer = 1

        write_bytestream_to_document = False

        While locked = True
            Try
                'Dim commentary As New bc_cs_activity_log("bc_cs_file_transfer_services", "write_bytestream_to_document", bc_cs_activity_codes.COMMENTARY, "Attempting to write byte stream to file: " + filename)
                Dim fs As New FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.ReadWrite, 65536, False)
                Using fs
                    fs.Write(bytedoc, 0, UBound(bytedoc) + 1)

                    fs.Close()
                End Using
                locked = False
                write_bytestream_to_document = True

            Catch ex As Exception
                If InStr(ex.Message, "mapped", CompareMethod.Text) > 0 Then
                    If bc_cs_central_settings.server_flag = 0 Then
                        'Dim omsg As New bc_cs_message("Blue Curve", "Failed to synchronize document as template is open (" + filename + ") please close all documents and restart application", bc_cs_message.MESSAGE)
                        Dim commentary As New bc_cs_activity_log("bc_cs_file_transfer_services", "write_bytestream_to_document", bc_cs_activity_codes.COMMENTARY, "Error: Failed to synchronize document as template is open (" + filename + ")", certificate)
                    End If
                    locked = False
                ElseIf InStr(ex.Message, "being used by another process.", CompareMethod.Text) <> 0 Then
                    If bc_cs_central_settings.server_flag = 0 Then
                        While i <= 4
                            If i = 4 Then
                                'Dim omsgFinal As New bc_cs_message("Blue Curve", "Failed to synchronize document as template is open (" + filename + "), document has not been synchronized", bc_cs_message.MESSAGE)
                                Dim commentary As New bc_cs_activity_log("bc_cs_file_transfer_services", "write_bytestream_to_document", bc_cs_activity_codes.COMMENTARY, "Error: Failed to synchronize document as template is open (" + filename + "), document has not been synchronized", certificate)
                                locked = False
                                Exit While
                            End If
                            'Dim omsg As New bc_cs_message("Blue Curve", "Failed to synchronize document as template is open (" + filename + ") please close the document and click ok", bc_cs_message.MESSAGE)
                            Dim commentary2 = New bc_cs_activity_log("bc_cs_file_transfer_services", "write_bytestream_to_document", bc_cs_activity_codes.COMMENTARY, "Error: Failed to synchronize document as template is open (" + filename + ")", certificate)
                            i = i + 1
                            Exit While
                        End While
                    Else
                        locked = False
                        Dim commentary2 = New bc_cs_activity_log("bc_cs_file_transfer_services", "write_bytestream_to_document", bc_cs_activity_codes.COMMENTARY, "Error writing: " + filename + ": " + ex.Message, certificate)
                        Me.err_text = ex.Message
                    End If
                Else
                    If show_err = True Then
                        Dim db_err As New bc_cs_error_log("bc_cs_file_transfer_services", "write_bytestream_to_document", bc_cs_error_codes.USER_DEFINED, ex.Message + ": " + filename, certificate)
                    Else
                        Me.err_text = "bc_cs_file_transfer_services: write_bytestream_to_document " + ex.Message

                    End If
                    locked = False

                End If
            Finally
                'otrace = New bc_cs_activity_log("bc_cs_file_transfer_services", "write_bytestream_to_document", bc_cs_activity_codes.TRACE_EXIT, "")
            End Try

        End While
    End Function
    Public Function write_document_to_string(ByVal filename As String) As String
        Dim otrace As New bc_cs_activity_log("bc_cs_file_transfer_services", "write_bytestream_to_document", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim commentary As New bc_cs_activity_log("bc_cs_file_transfer_services", "write_document_to_string", bc_cs_activity_codes.COMMENTARY, "Attempting to write document to string: " + filename)
            Dim fs As New StreamReader(filename, FileMode.Open)
            write_document_to_string = fs.ReadToEnd()
            fs.Close()
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_cs_file_transfer_services", "write_document_to_string", bc_cs_error_codes.USER_DEFINED, ex.Message)
            write_document_to_string = ""
        Finally
            otrace = New bc_cs_activity_log("bc_cs_file_transfer_services", "write_bytestream_to_document", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
    Public Function check_document_exists(ByVal filename As String, Optional ByRef certificate As bc_cs_security.certificate = Nothing) As Boolean
        ' Dim otrace As New bc_cs_activity_log("bc_cs_file_transfer_services", "check_document_exists", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            check_document_exists = True

            Dim fs As New FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.None, 65536, True)
            fs.Close()
        Catch ex As Exception
            If InStr(ex.Message, "used by another process") > 1 Then
                Dim commentary As New bc_cs_activity_log("bc_cs_file_transfer_services", "check_document_exists", bc_cs_activity_codes.COMMENTARY, "File: " + filename + " user mapped section open.", certificate)
                check_document_exists = True
            Else
                Dim commentary As New bc_cs_activity_log("bc_cs_file_transfer_services", "check_document_exists", bc_cs_activity_codes.COMMENTARY, "File: " + filename + " does not exist: " + ex.Message, certificate)
                check_document_exists = False
            End If
        End Try
        ' otrace = New bc_cs_activity_log("bc_cs_file_transfer_services", "check_document_exists", bc_cs_activity_codes.TRACE_EXIT, "")
    End Function
    Public Function file_already_open(ByVal filename As String) As Boolean
        Try
            file_already_open = False
            Dim fs As New FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.None, 65536, True)
            fs.Close()
        Catch ex As Exception
            If InStr(ex.Message, "used by another process") > 1 Then
                Dim commentary As New bc_cs_activity_log("bc_cs_file_transfer_services", "check_document_exists", bc_cs_activity_codes.COMMENTARY, "File: " + filename + " user mapped section open.")
                file_already_open = True
            End If
        End Try
    End Function
    Public Function check_document_exists_and_can_be_read(ByVal filename As String, Optional ByRef certificate As bc_cs_security.certificate = Nothing) As Boolean
        ' Dim otrace As New bc_cs_activity_log("bc_cs_file_transfer_services", "check_document_exists", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Me.updated_om = False

            check_document_exists_and_can_be_read = True
            '  Dim commentary As New bc_cs_activity_log("bc_cs_file_transfer_services", "check_document_exists", bc_cs_activity_codes.COMMENTARY, "Checking for existance of file: " + filename)
            Dim fs As New FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.None, 65536, True)
            fs.Close()
            REM if hasnt errored here xml file exists but now check it can be read
            Dim oxml As New bc_cs_data_services
            If oxml.soap_check_deserialize_file_to_object(filename, certificate) = False Then
                check_document_exists_and_can_be_read = False
                Me.updated_om = True
            End If

        Catch ex As Exception
            Dim commentary As New bc_cs_activity_log("bc_cs_file_transfer_services", "check_document_exists", bc_cs_activity_codes.COMMENTARY, "File: " + filename + " does not exist: " + ex.Message, certificate)
            check_document_exists_and_can_be_read = False
        End Try
        ' otrace = New bc_cs_activity_log("bc_cs_file_transfer_services", "check_document_exists", bc_cs_activity_codes.TRACE_EXIT, "")
    End Function
    <Serializable()> Class file_transfer_soap_object
        Public bytedoc As Byte()
        Public Sub New()

        End Sub
    End Class

    Public Sub New()

    End Sub
End Class
Public Class bc_cs_string_services
    Dim str As String
    REM
    Public Sub New(ByVal str As String)
        Me.str = str
    End Sub
    REM delimits apostrophies for SQL input
    Public Function delimit_apostrophies() As String
        Try
            Dim pos As Integer
            Dim tmpstr As String
            Dim dstr = str
            pos = 1
            While (InStr(pos, dstr, "'", vbTextCompare)) >= pos
                tmpstr = Left(dstr, InStr(pos, dstr, "'", CompareMethod.Text) - 1)
                tmpstr = tmpstr + "''"
                tmpstr = tmpstr + Right(dstr, Len(dstr) - InStr(pos, dstr, "'", vbTextCompare))
                dstr = tmpstr
                pos = InStr(pos, dstr, "'", vbTextCompare) + 2
            End While
            delimit_apostrophies = dstr
        Catch ex As Exception
            MsgBox(ex.Message)
            delimit_apostrophies = ""
        End Try

    End Function
    REM translate aspostrophies
    Public Function translate_apos()
        '  Dim otrace As New bc_cs_activity_log("bc_cs_file_transfer_services", "translate_apos", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim c, dhh As String
            Dim i As Integer
            dhh = str
            For i = 1 To Len(str)
                c = Mid(str, i, 1)
                If Asc(c) = 146 Then
                    dhh = Replace(dhh, c, "'", 1)
                End If
            Next
            translate_apos = dhh
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_cs_file_transfer_services", "translate_apos", bc_cs_error_codes.USER_DEFINED, ex.Message)
            translate_apos = ""
        Finally
            '     otrace = New bc_cs_activity_log("bc_cs_file_transfer_services", "translate_apos", bc_cs_activity_codes.TRACE_ENTRY, "")
        End Try
    End Function

    Public Function ReplaceSpecialChars(ByVal s As String) As String
        Dim strValid As String = s.Trim
        Try
            Dim i As Integer
            For i = 1 To 144
                If Len(strValid) = 0 Then
                    Exit Function
                End If

                If (i < 32 Or i > 125) Then
                    strValid = Replace(strValid, Chr(i).ToString, "", 1, , CompareMethod.Binary)
                ElseIf (i = 38 Or i = 60 Or i = 62) Then
                    strValid = Replace(strValid, Chr(i).ToString, "", 1, , CompareMethod.Binary)
                End If
            Next
            For i = 145 To 255
                If Len(strValid) = 0 Then
                    Exit Function
                End If
                If (Not (i > 144 And i < 149)) AndAlso (i <> 150 And i <> 153 And i <> 169 And i <> 174) Then
                    strValid = Replace(strValid, Chr(i).ToString, " ", 1, , CompareMethod.Binary)
                End If
            Next
        Catch
        Finally
            ReplaceSpecialChars = strValid
        End Try
    End Function


    REM FBW: removes/substitutes illegal SOAP serialise characters
    Public Function xml_compliant() As String
        Dim i As Integer
        'Replace all special characters with null value until 125
        For i = 1 To 144
            If Len(str) = 0 Then
                Return ""
            End If
            If (i < 32 Or i > 125) Then
                str = Replace(str, Chr(i).ToString, "", 1, , CompareMethod.Binary)
            End If
        Next
        'Allow left quote,right quote and copy right ,registered, trademark symbols to be placed in components.
        For i = 145 To 255
            If Len(str) = 0 Then
                Return ""
            End If
            If (Not (i > 144 And i < 147)) AndAlso (i <> 150 And i <> 153 And i <> 169 And i <> 174) Then
                str = Replace(str, Chr(i).ToString, " ", 1, , CompareMethod.Binary)
            End If
        Next
        xml_compliant = str
    End Function

End Class


Public Class bc_cs_date_services

    Private isoDateFormatString As String
    Private isoTimeFormatString As String
    Private localLongDateFormatString As String
    Private LocalShortDateFormatString As String
    Private localLongTimeFormatString As String
    Private LocalShortTimeFormatString As String

    Public ReadOnly Property ISODateFormat() As String
        Get
            ISODateFormat = isoDateFormatString
        End Get

    End Property

    Public ReadOnly Property ISOtimeFormat() As String
        Get
            ISOtimeFormat = isoTimeFormatString
        End Get

    End Property

    Public Property LocalLongDateFormat() As String
        Get
            LocalLongDateFormat = localLongDateFormatString
        End Get
        Set(ByVal value As String)
            localLongDateFormatString = value
        End Set
    End Property

    Public Property LocalShortDateFormat() As String
        Get
            LocalShortDateFormat = LocalShortDateFormatString
        End Get
        Set(ByVal value As String)
            LocalShortDateFormatString = value
        End Set
    End Property


    Public Property LocalLongTimeFormat() As String
        Get
            LocalLongTimeFormat = localLongTimeFormatString
        End Get
        Set(ByVal value As String)
            localLongTimeFormatString = value
        End Set
    End Property

    Public Property LocalShortTimeFormat() As String
        Get
            LocalShortTimeFormat = LocalShortTimeFormatString
        End Get
        Set(ByVal value As String)
            LocalShortTimeFormatString = value
        End Set
    End Property

    Public Sub New()

        isoDateFormatString = "yyyyMMdd"
        isoTimeFormatString = "HH:mm:ss"

        localLongDateFormatString = System.Globalization.DateTimeFormatInfo.CurrentInfo.LongDatePattern
        LocalShortDateFormatString = System.Globalization.DateTimeFormatInfo.CurrentInfo.ShortDatePattern
        localLongTimeFormatString = System.Globalization.DateTimeFormatInfo.CurrentInfo.LongTimePattern
        LocalShortTimeFormatString = System.Globalization.DateTimeFormatInfo.CurrentInfo.ShortTimePattern

    End Sub


    Public Function DateToSql(ByVal inDate As Object, Optional ByVal includeTime As Boolean = False) As String

        Dim SqlDate As String = ""
        Dim DateTimeFormat As String = ""
        Dim ChangeDate As DateTime
        Dim SystemCulture As String


        Try

            SystemCulture = System.Globalization.CultureInfo.CurrentUICulture.Name()

            If IsDate(inDate) Then

                If TypeOf (inDate) Is Date Then
                    ChangeDate = inDate
                ElseIf TypeOf (inDate) Is String Then
                    REM ChangeDate = Convert.ToDateTime(inDate)
                    Dim dtfi As DateTimeFormatInfo
                    dtfi = CType(DateTimeFormatInfo.CurrentInfo.Clone, DateTimeFormatInfo)
                    dtfi.ShortDatePattern = LocalShortDateFormatString
                    dtfi.ShortTimePattern = LocalShortTimeFormat
                    dtfi.LongDatePattern = localLongDateFormatString
                    dtfi.LongTimePattern = LocalLongTimeFormat
                    ChangeDate = Date.Parse(inDate, dtfi)
                End If

                If includeTime = False Then
                    REM ISO Date()
                    SqlDate = Format(ChangeDate, isoDateFormatString)
                Else
                    REM ISO Date Time format
                    DateTimeFormat = isoDateFormatString + " " + isoTimeFormatString
                    SqlDate = Format(ChangeDate, DateTimeFormat)
                End If

            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_cs_date_services", "DateToSql", bc_cs_error_codes.USER_DEFINED, ex.Message)
            DateToSql = ""
        Finally

            DateToSql = SqlDate
        End Try

    End Function


    Public Function DateToLocalShort(ByVal inDate As Object, Optional ByVal includeTime As Boolean = False) As String

        Dim LocalShortDate As String = ""
        Dim DateTimeFormat As String = ""
        Dim ChangeDate As DateTime

        Try

            If IsDate(inDate) Then

                If TypeOf (inDate) Is Date Then
                    ChangeDate = inDate
                ElseIf TypeOf (inDate) Is String Then
                    REM ChangeDate = Convert.ToDateTime(inDate)
                    Dim dtfi As DateTimeFormatInfo
                    dtfi = CType(DateTimeFormatInfo.CurrentInfo.Clone, DateTimeFormatInfo)
                    dtfi.ShortDatePattern = LocalShortDateFormatString
                    dtfi.ShortTimePattern = LocalShortTimeFormat
                    dtfi.LongDatePattern = localLongDateFormatString
                    dtfi.LongTimePattern = LocalLongTimeFormat
                    ChangeDate = Date.Parse(inDate, dtfi)
                End If

                If includeTime = False Then
                    REM ISO Date()
                    LocalShortDate = Format(ChangeDate, LocalShortDateFormatString)
                Else
                    REM ISO Date Time format
                    DateTimeFormat = LocalShortDateFormatString + " " + LocalShortTimeFormat
                    LocalShortDate = Format(ChangeDate, DateTimeFormat)
                End If

            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_cs_date_services", "DateToLocalShort", bc_cs_error_codes.USER_DEFINED, ex.Message)
            DateToLocalShort = ""
        Finally

            DateToLocalShort = LocalShortDate
        End Try

    End Function

    Public Function DateToLocalLong(ByVal inDate As Object, Optional ByVal includeTime As Boolean = False) As String

        Dim LocalLongDate As String = ""
        Dim DateTimeFormat As String = ""
        Dim ChangeDate As DateTime

        Try

            If IsDate(inDate) Then

                If TypeOf (inDate) Is Date Then
                    ChangeDate = inDate
                ElseIf TypeOf (inDate) Is String Then
                    REM ChangeDate = Convert.ToDateTime(inDate)
                    Dim dtfi As DateTimeFormatInfo
                    dtfi = CType(DateTimeFormatInfo.CurrentInfo.Clone, DateTimeFormatInfo)
                    dtfi.ShortDatePattern = LocalShortDateFormatString
                    dtfi.ShortTimePattern = LocalShortTimeFormat
                    dtfi.LongDatePattern = localLongDateFormatString
                    dtfi.LongTimePattern = LocalLongTimeFormat
                    ChangeDate = Date.Parse(inDate, dtfi)
                End If

                If includeTime = False Then
                    REM ISO Date()
                    LocalLongDate = Format(ChangeDate, localLongDateFormatString)
                Else
                    REM ISO Date Time format
                    DateTimeFormat = localLongDateFormatString + " " + LocalLongTimeFormat
                    LocalLongDate = Format(ChangeDate, DateTimeFormat)
                End If

            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_cs_date_services", "DateToLocalLong", bc_cs_error_codes.USER_DEFINED, ex.Message)
            DateToLocalLong = ""
        Finally

            DateToLocalLong = LocalLongDate
        End Try

    End Function


End Class