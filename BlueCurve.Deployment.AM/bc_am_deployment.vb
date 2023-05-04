Imports BlueCurve.Core.AS
Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports System.Windows.Forms
Imports System.IO
Imports System.Text
Imports System.Xml
Imports System.Xml.XPath
Imports System.Threading

'==========================================
' Bluecurve Limited 2012
' Module:         Deployment
' Type:           AM
' Desciption:     main form controller
' Public Methods: Show
' Version:        1.0
' Change history:
'
'==========================================
Public Class bc_am_deployment

    Private Const FORM_NAME = "bc_am_deployment"
    Private Const ARTEFACT_FILE = "bc_am_dd_artefacts.xml"
    Friend Const DIFFERENCE_FILE = "bc_am_dd_diff.xml"
    Friend Const DEFAULT_ERROR_FILE = "bc_am_dd_errors.xml"
    Friend Const FILE_LAST_LOAD = "bc_am_dd_history.xml"
    Private Const EXPORT_TEXT = "&Export"
    Private Const COMPARE_TEXT = "&Compare"
    Private Const UPDATE_TEXT = "&Import"
    Private Const SAVE_MASK_TEXT = "&Save Tree"
    Private Const OPEN_MASK_TEXT = "&Open Tree"
    Private Const SAVE_ARCHIVE_TEXT = "&Save Archive"
    Private Const LOCK_TEXT = "&Lock System"
    Private Const UNLOCK_TEXT = "&Unlock System"

    ' main view
    Private WithEvents view As bc_am_dd_main

    Private alUnlockedButtons As New ArrayList
    Private alLockedButtons As New ArrayList
    Private alActionButtons As New ArrayList

    Private xdInclude As XmlNode
    Private boolCheckDependencies, boolIncludeDependencies, boolValidateOnly, boolArchive, boolUpdateDestination, boolOverwriteAll As Boolean
    Private strSourcePath, strDestinationPath, strExclusionsFile, strIncludeFile, strArchiveExtension, strInstructionsFile, strErrorFile As String
    Dim intErrorCode As Int32

    Friend htRows As Hashtable
    Friend slAll As Hashtable
    Friend alTables, alListByCommand, alListByCommandChecked As ArrayList

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Property OverwriteAll() As Boolean
        Get
            Return boolOverwriteAll
        End Get
        Set(ByVal boolOverwriteAll As Boolean)
            Me.boolOverwriteAll = boolOverwriteAll
        End Set
    End Property

    Property IncludeDependencies() As Boolean
        Get
            Return boolIncludeDependencies
        End Get
        Set(ByVal boolIncludeDependencies As Boolean)
            Me.boolIncludeDependencies = boolIncludeDependencies
        End Set
    End Property

    Property SourceArchived() As Boolean
        Get
            Return boolArchive
        End Get
        Set(ByVal boolArchive As Boolean)
            Me.boolArchive = boolArchive
        End Set
    End Property

    Property UpdateDestinationFiles() As Boolean
        Get
            Return boolUpdateDestination
        End Get
        Set(ByVal boolUpdateDestination As Boolean)
            Me.boolUpdateDestination = boolUpdateDestination
        End Set
    End Property

    Property InstructionsFile() As String
        Get
            Return strInstructionsFile
        End Get
        Set(ByVal strInstructionsFile As String)
            If Not strInstructionsFile = "" And Not strInstructionsFile Is Nothing Then
                Me.strInstructionsFile = strInstructionsFile
            Else
                Me.strInstructionsFile = Nothing
            End If
        End Set
    End Property

    Property SourcePath() As String
        Get
            Return strSourcePath
        End Get
        Set(ByVal strSourcePath As String)
            If Not strSourcePath = "" And Not strSourcePath Is Nothing Then
                If Not strSourcePath.ToCharArray()(strSourcePath.Length - 1) = "\" Then
                    strSourcePath = strSourcePath & "\"
                End If
                Me.strSourcePath = strSourcePath
            Else
                Me.strSourcePath = Nothing
            End If
        End Set
    End Property

    Property DestinationPath() As String
        Get
            Return strDestinationPath
        End Get
        Set(ByVal strDestinationPath As String)
            If Not strDestinationPath = "" And Not strDestinationPath Is Nothing Then
                If Not strDestinationPath.ToCharArray()(strDestinationPath.Length - 1) = "\" Then
                    strDestinationPath = strDestinationPath & "\"
                End If
                Me.strDestinationPath = strDestinationPath
            Else
                Me.strDestinationPath = Nothing
            End If
        End Set
    End Property

    Property IncludeFile() As String
        Get
            Return strIncludeFile
        End Get
        Set(ByVal strIncludeFile As String)
            If Not strIncludeFile = "" And Not strIncludeFile Is Nothing Then
                Me.strIncludeFile = strIncludeFile
            Else
                Me.strIncludeFile = Nothing
            End If
        End Set
    End Property

    Property ExclusionsFile() As String
        Get
            Return strExclusionsFile
        End Get
        Set(ByVal strExclusionsFile As String)
            If Not strExclusionsFile = "" And Not strExclusionsFile Is Nothing Then
                Me.strExclusionsFile = strExclusionsFile
            Else
                Me.strExclusionsFile = Nothing
            End If
        End Set
    End Property

    Property ArchiveExtension() As String
        Get
            If Not strArchiveExtension = "" And Not strArchiveExtension Is Nothing Then
                Return strArchiveExtension
            Else
                Return "nolongerneeded"
            End If
        End Get
        Set(ByVal strArchiveExtension As String)
            Me.strArchiveExtension = strArchiveExtension
        End Set
    End Property

    Friend Property CheckDependencies() As Boolean
        Get
            Return boolCheckDependencies
        End Get
        Set(ByVal boolCheckDependencies As Boolean)
            Me.boolCheckDependencies = boolCheckDependencies
        End Set
    End Property

    Friend Property ValidateOnly() As Boolean
        Get
            Return boolValidateOnly
        End Get
        Set(ByVal boolValidateOnly As Boolean)
            Me.boolValidateOnly = boolValidateOnly
        End Set
    End Property

    Public Property ErrorCode() As bc_am_dd_enums.ErrorCodes
        Get
            Return intErrorCode
        End Get
        Set(ByVal intErrorCode As bc_am_dd_enums.ErrorCodes)
            Me.intErrorCode = intErrorCode
        End Set
    End Property

    Public Property ErrorFile() As String
        Get
            Return strErrorFile
        End Get
        Set(ByVal strErrorFile As String)
            Me.strErrorFile = strErrorFile
        End Set
    End Property

    Public Property IncludeXml() As XmlDocument
        Get
            Return xdInclude
        End Get
        Set(ByVal xdInclude As XmlDocument)
            Me.xdInclude = xdInclude
        End Set
    End Property

    Public Sub New(ByVal view As bc_am_dd_main)

        view.Controller = Me
        Me.view = view

        LoadDatabaseObjects(view.TreeView())

        If Not view.badupError Is Nothing Then
            Dim strRepo As String = bc_cs_central_settings.local_repos_path
            If Not strRepo.Substring(strRepo.Length - 1) = "\" Then
                strRepo = strRepo & "\"
            End If
            view.badupError.Uri = strRepo & DEFAULT_ERROR_FILE
            ErrorFile = strRepo & DEFAULT_ERROR_FILE
        End If

    End Sub

    Public Sub New(ByVal strOperationsFile As String)

        Dim METHOD_NAME As String = System.Reflection.MethodBase.GetCurrentMethod().Name
        Dim log = New bc_cs_activity_log(FORM_NAME, METHOD_NAME, bc_cs_activity_codes.TRACE_ENTRY, "")

        Try

            If File.Exists(strOperationsFile) Then
                Dim xdOperations As New XmlDocument
                xdOperations.Load(strOperationsFile)
                For Each xnOperation As XmlNode In xdOperations.SelectNodes("//operation")
                    Dim xnType As XmlNode = xnOperation.SelectSingleNode("type")
                    If Not xnType Is Nothing Then

                        Dim xnExport As XmlNode = xnOperation.SelectSingleNode("export")
                        Dim xnImport As XmlNode = xnOperation.SelectSingleNode("import")
                        Dim xnInclude As XmlNode = xnOperation.SelectSingleNode("include")
                        Dim xnExclude As XmlNode = xnOperation.SelectSingleNode("exclude")
                        Dim xnArchive As XmlNode = xnOperation.SelectSingleNode("archive_source")
                        Dim xnCheckDependencies As XmlNode = xnOperation.SelectSingleNode("check_dependencies")
                        Dim xnIncludeDependencies As XmlNode = xnOperation.SelectSingleNode("include_dependencies")
                        Dim xnRollback As XmlNode = xnOperation.SelectSingleNode("rollback")
                        Dim xnInstructions As XmlNode = xnOperation.SelectSingleNode("instructions")
                        Dim xnUpdateDestination As XmlNode = xnOperation.SelectSingleNode("update_destination")
                        Dim xnArchiveExtension As XmlNode = xnOperation.SelectSingleNode("archive_extension")
                        Dim xnErrors As XmlNode = xnOperation.SelectSingleNode("errors")
                        Dim xnOverwrite As XmlNode = xnOperation.SelectSingleNode("overwrite")

                        If Not xnExport Is Nothing Then
                            SourcePath = xnExport.InnerText
                        End If
                        If Not xnImport Is Nothing Then
                            DestinationPath = xnImport.InnerText
                        End If
                        If Not xnInclude Is Nothing Then
                            'IncludeFile = xnInclude.InnerText
                            If xnInclude.ChildNodes.Count > 0 Then
                                Dim xdMask As New XmlDocument
                                xdMask.AppendChild(xdMask.ImportNode(xnInclude.ChildNodes(0), True))
                                IncludeXml = xdMask
                            End If
                        End If
                        If Not xnArchive Is Nothing Then
                            SourceArchived = BitToBool(xnArchive.InnerText)
                        End If
                        If Not xnExclude Is Nothing Then
                            ExclusionsFile = xnExclude.InnerText
                        End If
                        If Not xnCheckDependencies Is Nothing Then
                            CheckDependencies = BitToBool(xnCheckDependencies.InnerText)
                        End If
                        If Not xnIncludeDependencies Is Nothing Then
                            IncludeDependencies = BitToBool(xnIncludeDependencies.InnerText)
                        End If
                        If Not xnRollback Is Nothing Then
                            ValidateOnly = BitToBool(xnRollback.InnerText)
                        End If
                        If Not xnInstructions Is Nothing Then
                            InstructionsFile = xnInstructions.InnerText
                        End If
                        If Not xnUpdateDestination Is Nothing Then
                            UpdateDestinationFiles = BitToBool(xnUpdateDestination.InnerText)
                        End If
                        If Not xnArchiveExtension Is Nothing Then
                            ArchiveExtension = xnArchiveExtension.InnerText
                        End If
                        If Not xnOverwrite Is Nothing Then
                            OverwriteAll = BitToBool(xnOverwrite.InnerText)
                        End If
                        If Not xnErrors Is Nothing Then
                            ErrorFile = xnErrors.InnerText
                        End If

                        If xnType.InnerText.ToLower = "export" Then
                            ExportDatabase()
                        ElseIf xnType.InnerText.ToLower = "compare" Then
                            CompareDatabases()
                        ElseIf xnType.InnerText.ToLower = "import" Then
                            ImportInstructions()
                        ElseIf xnType.InnerText.ToLower = "lock" Then
                            SetDatabaseStatus(True)
                        ElseIf xnType.InnerText.ToLower = "unlock" Then
                            SetDatabaseStatus(False)
                        End If

                        If Not ErrorCode = ErrorCodes.NONE Then
                            Exit Sub
                        End If

                    Else
                        'Error
                        ErrorCode = ErrorCodes.UNKNOWN
                    End If
                Next
            Else
                ErrorCode = ErrorCodes.LOAD
                Throw New Exception("The specified Operation file does not exist.")
            End If

        Catch ex As Exception
            If ErrorCode = ErrorCodes.NONE Then
                ErrorCode = ErrorCodes.UNKNOWN
            End If
            Dim errLog As New bc_cs_error_log(FORM_NAME, METHOD_NAME, bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log(FORM_NAME, METHOD_NAME, bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Public Sub ExportDatabase()

        If IncludeXml Is Nothing AndAlso Not strIncludeFile Is Nothing AndAlso (strIncludeFile.ToLower().IndexOf(".xml") <> strIncludeFile.Length - 4 OrElse Not File.Exists(strIncludeFile)) Then
            'Error
            ErrorCode = ErrorCodes.EXPORT
            DisplayMessage(view, "The entered inclusion mask is not valid.", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Not strExclusionsFile Is Nothing AndAlso (strExclusionsFile.ToLower().IndexOf(".xml") <> strExclusionsFile.Length - 4 OrElse Not File.Exists(strExclusionsFile)) Then
            'Error
            ErrorCode = ErrorCodes.EXPORT
            DisplayMessage(view, "The entered exclude mask is not valid.", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Dim tv As bc_am_dd_tree
        Dim mt As MaskType = MaskType.Include

        tv = New bc_am_dd_tree
        If Not isValidDirectory(SourcePath) Then
            'Error
            ErrorCode = ErrorCodes.EXPORT
            DisplayMessage(view, "The entered export path is not valid.", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        LoadDatabaseObjects(tv)

        ExportDatabase(tv, mt)

    End Sub

    Public Sub Show()

        Dim log = New bc_cs_activity_log(FORM_NAME, "Show", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            SetRoleUserDB(view)

            CreateToolBarButtons()

            SetToolBarButtonStatus()

            Application.DoEvents()

            ' show the main form
            view.ShowDialog()

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log(FORM_NAME, "Show", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log(FORM_NAME, "Show", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Friend Sub SetToolBarButtonStatus()
        Dim boolLocked As Boolean = GetDatabaseStatus()
        For Each tbb As ToolBarButton In alLockedButtons
            tbb.Enabled = boolLocked
        Next
        For Each tbb As ToolBarButton In alUnlockedButtons
            tbb.Enabled = Not boolLocked
        Next
        view.uxExit.Checked = boolLocked
    End Sub

    Friend Sub SetRoleUserDB(ByRef currentView As bc_am_dd_main)

        Dim log = New bc_cs_activity_log(FORM_NAME, "SetRoleUserDB", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            ' set user, role, database & server
            Dim i As Integer
            For i = 0 To bc_am_load_objects.obc_users.user.Count - 1
                With bc_am_load_objects.obc_users.user(i)
                    If (bc_cs_central_settings.show_authentication_form = 0 Or bc_cs_central_settings.show_authentication_form = 2) Then
                        If UCase(Trim(.os_user_name)) = UCase(Trim(bc_cs_central_settings.logged_on_user_name)) Then
                            currentView.uxUser.Text = "User: " + .first_name + " " + .surname
                            currentView.uxRole.Text = "Role: " + .role
                            Exit For
                        End If
                    Else
                        If UCase(Trim(.user_name)) = UCase(Trim(bc_cs_central_settings.user_name)) Then
                            currentView.uxUser.Text = "User: " + .user_name
                            currentView.uxRole.Text = "Role: " + .role
                            Exit For
                        End If
                    End If
                End With
            Next

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                currentView.uxServer.Text = "Connected via Ado: " + bc_cs_central_settings.servername
            Else
                currentView.uxServer.Text = "Connected via Soap: " + bc_cs_central_settings.soap_server
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log(FORM_NAME, "SetRoleUserDB", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log(FORM_NAME, "SetRoleUserDB", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Private Sub CreateToolBarButtons()
        Dim newindex As Integer
        With view.uxToolBarMain.Buttons

            newindex = .Add(EXPORT_TEXT)
            .Item(newindex).ImageIndex = 2
            .Item(newindex).ToolTipText = "Export an existing database for comparison."
            'alActionButtons.add(.Item(newindex))

            newindex = .Add(COMPARE_TEXT)
            .Item(newindex).ImageIndex = 1
            .Item(newindex).ToolTipText = "Compare to an existing database."
            'alActionButtons.add(.Item(newindex))

            'newindex = .Add("")
            '.Item(newindex).Style = ToolBarButtonStyle.Separator

            newindex = .Add(UPDATE_TEXT)
            .Item(newindex).ImageIndex = 3
            .Item(newindex).ToolTipText = "Update database from an instructions file."
            'alUnlockedButtons.Add(.Item(newindex))

            newindex = .Add("")
            .Item(newindex).Style = ToolBarButtonStyle.Separator

            'newindex = .Add(OPEN_MASK_TEXT)
            '.Item(newindex).ImageIndex = 2
            '.Item(newindex).ToolTipText = "Apply Database Mask."

            newindex = .Add(OPEN_MASK_TEXT)
            .Item(newindex).ImageIndex = 0
            .Item(newindex).ToolTipText = "Load Tree."

            newindex = .Add(SAVE_MASK_TEXT)
            .Item(newindex).ImageIndex = 6
            .Item(newindex).ToolTipText = "Save Tree."

            newindex = .Add("")
            .Item(newindex).Style = ToolBarButtonStyle.Separator

            newindex = .Add(LOCK_TEXT)
            .Item(newindex).ImageIndex = 5
            .Item(newindex).ToolTipText = "Lock System."
            alUnlockedButtons.Add(.Item(newindex))

            newindex = .Add(UNLOCK_TEXT)
            .Item(newindex).ImageIndex = 4
            .Item(newindex).ToolTipText = "Unlock System."
            alLockedButtons.Add(.Item(newindex))

            newindex = .Add("")
            .Item(newindex).Style = ToolBarButtonStyle.Separator

            newindex = .Add("Refresh Tree")
            .Item(newindex).ImageIndex = 7
            'alUnlockedButtons.Add(.Item(newindex))

            'newindex = .Add("")
            '.Item(newindex).Style = ToolBarButtonStyle.Separator

        End With
    End Sub

    Friend Sub SaveOperationFile(ByVal strOperation As String)

        Try

            Dim sfd As New SaveFileDialog()

            sfd.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*"
            sfd.RestoreDirectory = True

            If sfd.ShowDialog() = DialogResult.OK Then
            Else
                Exit Sub
            End If

            Dim xdOperation As New XmlDocument
            Dim xnRoot As XmlNode = xdOperation.CreateElement("operations")
            xdOperation.AppendChild(xnRoot)

            If strOperation = "All" Then
                CreateOperation(xdOperation, "Unlock", sfd.FileName)
                CreateOperation(xdOperation, "Export", sfd.FileName)
                CreateOperation(xdOperation, "Compare", sfd.FileName)
                CreateOperation(xdOperation, "Import", sfd.FileName)
                CreateOperation(xdOperation, "Lock", sfd.FileName)
            Else
                CreateOperation(xdOperation, strOperation, sfd.FileName)
            End If

            xdOperation.Save(sfd.FileName)

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log(FORM_NAME, "SaveOperationFile", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Sub

    Private Sub CreateOperation(ByRef xdOperation As XmlDocument, ByVal strOperation As String, ByVal strFileName As String)

        Dim xnOperation As XmlNode = xdOperation.CreateElement("operation")
        xdOperation.DocumentElement.AppendChild(xnOperation)

        Dim xnType As XmlNode = xdOperation.CreateElement("type")
        xnType.InnerText = strOperation.ToLower
        xnOperation.AppendChild(xnType)

        If strOperation.ToLower() = "lock" Then
        ElseIf strOperation.ToLower() = "unlock" Then
        ElseIf strOperation.ToLower() = "compare" Then

            If SourcePath Is Nothing Then
                Throw New Exception("You must specify a source path.")
            End If

            AddNodeToDocument(xnOperation, "export", SourcePath)

            If DestinationPath Is Nothing Then
                Throw New Exception("You must specify a destination path.")
            End If

            AddNodeToDocument(xnOperation, "import", DestinationPath)

            AddNodeToDocument(xnOperation, "check_dependencies", BoolToBit(CheckDependencies))

            If InstructionsFile Is Nothing Then
                Throw New Exception("You must specify an instruction files.")
            End If

            AddNodeToDocument(xnOperation, "instructions", InstructionsFile)

        ElseIf strOperation.ToLower() = "export" Then

            If SourcePath Is Nothing Then
                Throw New Exception("You must specify a destination path.")
            End If

            AddNodeToDocument(xnOperation, "export", SourcePath)

            'Dim strIncludeFile As String = strFileName.Replace(".xml", "_instr.xml")
            'SaveTreeMask(view.TreeView, strIncludeFile, MaskType.Include)

            If Not ExclusionsFile Is Nothing Then
                AddNodeToDocument(xnOperation, "exclude", ExclusionsFile)
            End If

            AddNodeToDocument(xnOperation, "archive_source", BoolToBit(SourceArchived))

            If SourceArchived Then
                AddNodeToDocument(xnOperation, "archive_extension", ArchiveExtension)
            End If

            AddNodeToDocument(xnOperation, "include_dependencies", BoolToBit(IncludeDependencies))

            AddNodeToDocument(xnOperation, "overwrite", BoolToBit(OverwriteAll))

            Dim xd As XmlDocument = xdOperation
            Dim xnChild As XmlNode = xd.CreateElement("include")
            xnChild.InnerXml = GetTreeMask(view.uxObjectTreeView, MaskType.Include).OuterXml
            xnOperation.AppendChild(xnChild)

        ElseIf strOperation.ToLower() = "import" Then

            If InstructionsFile Is Nothing Then
                Throw New Exception("You must specify an instruction files.")
            End If

            AddNodeToDocument(xnOperation, "instructions", InstructionsFile)

            AddNodeToDocument(xnOperation, "rollback", BoolToBit(ValidateOnly))

            AddNodeToDocument(xnOperation, "update_destination", BoolToBit(UpdateDestinationFiles))

            If UpdateDestinationFiles Then
                If DestinationPath Is Nothing Then
                    Throw New Exception("If Update Destination is checked you must specify a destination path.")
                End If
                If SourcePath Is Nothing Then
                    Throw New Exception("If Update Destination is checked you must specify a source path.")
                End If
                AddNodeToDocument(xnOperation, "export", SourcePath)
                AddNodeToDocument(xnOperation, "import", DestinationPath)                
            End If

            If UpdateDestinationFiles Then
                AddNodeToDocument(xnOperation, "archive_extension", ArchiveExtension)
            End If

            AddNodeToDocument(xnOperation, "errors", ErrorFile)

        Else
            'Error
            Throw New Exception(strOperation & " is not an operation type.")
        End If

    End Sub

    Friend Sub ToolBarButtonPressed(ByVal e As ToolBarButtonClickEventArgs)

        Try

            view.Cursor = Cursors.WaitCursor

            view.Enabled = False

            If e.Button.Text = EXPORT_TEXT Then
                ExportDatabase(view.TreeView(), MaskType.Display)
            End If

            If e.Button.Text = COMPARE_TEXT Then
                CompareDatabases()
            End If

            If e.Button.Text = UPDATE_TEXT Then
                ImportInstructions()
            End If

            If e.Button.Text = OPEN_MASK_TEXT Then

                Dim ofd As New OpenFileDialog()

                ofd.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*"
                ofd.RestoreDirectory = True

                If ofd.ShowDialog() = DialogResult.OK Then
                    ApplyTreeMask(view.TreeView(), ofd.FileName, MaskType.Display)
                End If

            End If

            If e.Button.Text = SAVE_MASK_TEXT Then

                RefreshView()

                Dim bads As New bc_am_dd_option("Save as...", MaskType.Include, MaskType.Exclude, MaskType.Archive)
                bads.ShowDialog(view)

                RefreshView()

                If Not bads.result = MaskType.Blank Then

                    Dim sfd As New SaveFileDialog()

                    sfd.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*"
                    sfd.RestoreDirectory = True

                    If sfd.ShowDialog() = DialogResult.OK Then
                        SaveTreeMask(view.TreeView(), sfd.FileName, bads.result)
                    End If

                End If

            End If

            If e.Button.Text = LOCK_TEXT Then
                LockDatabase()
                SetToolBarButtonStatus()
            End If

            If e.Button.Text = UNLOCK_TEXT Then
                UnlockDatabase()
                SetToolBarButtonStatus()
            End If

            If e.Button.Text = "Refresh Tree" Then
                view.TreeView().Nodes.Clear()
                LoadDatabaseObjects(view.TreeView())
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log(FORM_NAME, "ToolBarButtonPressed", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            view.Enabled = True
            view.Cursor = Cursors.Default
            view.BringToFront()
        End Try

    End Sub

    Private Sub LockDatabase()

        Try
            If Not view Is Nothing Then
                Dim badl As New bc_am_dd_lockdown
                badl.ShowDialog(view)
                view.Refresh()
                If Not badl.Cancelled Then
                    QueryDatabase("exec bc_core_lock_system '" & badl.Message & "', '" & badl.LockTime & "'")
                End If
            Else
                'Needs to be reviewed
                QueryDatabase("exec bc_core_lock_system '', '" & DateTime.Now & "'")
            End If
        Catch ex As Exception
            ErrorCode = ErrorCodes.LOCK
            Dim errLog As New bc_cs_error_log(FORM_NAME, "LockDatabase", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Sub

    Private Sub UnlockDatabase()
        'add message later
        Try
            QueryDatabase("exec bc_core_unlock_system ''")
        Catch ex As Exception
            ErrorCode = ErrorCodes.UNLOCK
            Dim errLog As New bc_cs_error_log(FORM_NAME, "UnlockDatabase", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub

    Friend Sub SetDatabaseStatus(ByVal boolLock As Boolean)
        If boolLock Then
            LockDatabase()
        Else
            UnlockDatabase()
        End If
        If Not view Is Nothing Then
            SetToolBarButtonStatus()
        End If
    End Sub

    Friend Function GetArtefactsFileName()
        Return bc_cs_central_settings.get_user_dir() & "\" & ARTEFACT_FILE
    End Function
    Friend Function GetConfigFileName()
        Return bc_cs_central_settings.get_user_dir() & "\" & bc_cs_central_settings.DEPLOYMENT_MANAGER_CONFIG_FILE
    End Function
    Private Const DEFAULT_BUFFER = 1000
    Friend Function GetInstructionsGranularity() As Integer
        Dim strFile As String = GetConfigFileName()
        Try
            If File.Exists(strFile) Then
                Dim xd As New XmlDocument
                xd.Load(strFile)
                Dim xn As XmlNode = xd.SelectSingleNode("//instructions_granularity")
                If Not xn Is Nothing Then
                    If xn.Attributes("on") Is Nothing OrElse xn.Attributes("size") Is Nothing Then
                        Return DEFAULT_BUFFER
                    Else
                        If xn.Attributes("on").Value = 1 Then
                            Dim intReturn As Integer = xn.Attributes("size").Value
                            Return intReturn
                        Else
                            Return -1
                        End If
                    End If
                Else
                    Return DEFAULT_BUFFER
                End If
            Else
                Return DEFAULT_BUFFER
            End If
        Catch
            Return DEFAULT_BUFFER
        End Try
    End Function

    'Friend Function GetDiffFileName()
    '    Return bc_cs_central_settings.local_repos_path & "\" & DIFFERENCE_FILE
    'End Function

    'Dim sb As New StringBuilder
    Dim hdSS As Hashtable

    Private Sub LoadDatabaseObjects(ByRef tvObjects As bc_am_dd_tree)

        Dim METHOD_NAME As String = System.Reflection.MethodBase.GetCurrentMethod().Name

        iTimeElapsed = 0

        Dim debugTimeElapsed As Long = DateTime.Now.Ticks

        Dim log = New bc_cs_activity_log(FORM_NAME, METHOD_NAME, bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            hdSS = New Hashtable

            Dim xdSS As New XmlDocument
            If File.Exists("C:\nodes.xml") Then
                xdSS.Load("C:\nodes.xml")
                For Each xn As XmlNode In xdSS.DocumentElement.ChildNodes
                    hdSS.Add(xn.OuterXml, xn.OuterXml)
                Next
            End If

            Dim strArtefactFile As String = GetArtefactsFileName()

            If File.Exists(strArtefactFile) Then

                WriteToConsole("Building tree...")

                SuspendDrawing(tvObjects)

                Dim xdArtefacts As XmlDocument

                If Not tvObjects Is Nothing Then
                    tvObjects.Nodes.Clear()
                End If

                xdArtefacts = New XmlDocument
                xdArtefacts.Load(strArtefactFile)

                Dim xnlArtefacts As XmlNodeList = xdArtefacts.GetElementsByTagName("artefacts")

                If xnlArtefacts.Count = 0 Then
                    Exit Try
                End If

                Dim badn As bc_am_dd_node = New bc_am_dd_node("Server", True)

                Dim intTableIndex, intTableCount, intNodeIndex, intNodeCount As Integer
                intTableIndex = 0
                intTableCount = GetNodeWithoutAttributeCount(xnlArtefacts(0), "select")
                intNodeIndex = 0

                Dim strLoadHistory As String = bc_cs_central_settings.get_user_dir() & "\" & FILE_LAST_LOAD
                Dim xdLoad As XmlDocument = Nothing
                Dim xnHistory As XmlNode = Nothing
                Try
                    If File.Exists(strLoadHistory) Then
                        xdLoad = New XmlDocument
                        xdLoad.Load(strLoadHistory)
                        xnHistory = xdLoad.SelectSingleNode("/history/load[@system=""" & bc_cs_central_settings.dbname & """]")
                        If Not xnHistory Is Nothing AndAlso Not xnHistory.Attributes("nodeCount") Is Nothing Then
                            intNodeCount = xnHistory.Attributes("nodeCount").Value
                        End If
                    End If
                Catch exp As Exception
                    Dim s As String = ""
                End Try

                If Not tvObjects Is Nothing Then
                    tvObjects.Nodes.Add(badn)
                End If

                CreateProgressBar(view, "Creating database tree...")

                htRows = New Hashtable
                alTables = New ArrayList
                slAll = New Hashtable
                alListByCommand = New ArrayList

                alTables.Add(badn)

                badn.Name = badn.Text

                'DebugLine((DateTime.Now.Ticks - debugTimeElapsed) / TimeSpan.TicksPerSecond)

                AddArtefactNode(badn, xnlArtefacts(0), Nothing, intTableIndex, intTableCount, intNodeIndex, intNodeCount, New ArrayList, New ArrayList, 0)

                'DebugLine(iTimeElapsed / TimeSpan.TicksPerSecond)

                'DebugLine((DateTime.Now.Ticks - debugTimeElapsed) / TimeSpan.TicksPerSecond)

                Try
                    If xdLoad Is Nothing Then
                        bc_am_dd_tools.WriteFile(strLoadHistory, "<history><load system=""" & bc_cs_central_settings.dbname & """ nodeCount=""" & intNodeIndex & """ /></history>")
                    ElseIf xnHistory Is Nothing Then
                        xnHistory = xdLoad.CreateElement("load")
                        xdLoad.ChildNodes(0).AppendChild(xnHistory)
                        Dim xa As XmlAttribute = xdLoad.CreateAttribute("system")
                        xa.Value = bc_cs_central_settings.dbname
                        xnHistory.Attributes.Append(xa)
                        xa = xdLoad.CreateAttribute("nodeCount")
                        xa.Value = intNodeIndex
                        xnHistory.Attributes.Append(xa)
                        xdLoad.Save(strLoadHistory)
                    Else
                        xnHistory.Attributes("nodeCount").Value = intNodeIndex
                        xdLoad.Save(strLoadHistory)
                    End If
                Catch
                End Try

                badn.Expand()

                ResumeDrawing(tvObjects)

            End If

        Catch ex As Exception
            ErrorCode = ErrorCodes.LOAD
            Dim errLog As New bc_cs_error_log(FORM_NAME, METHOD_NAME, bc_cs_error_codes.USER_DEFINED, ex.Message & ControlChars.Lf & ex.StackTrace)
        Finally
            log = New bc_cs_activity_log(FORM_NAME, METHOD_NAME, bc_cs_activity_codes.TRACE_EXIT, "")
            UnloadProgressBar(view)
            DebugLine((DateTime.Now.Ticks - debugTimeElapsed) / TimeSpan.TicksPerSecond)
        End Try

    End Sub

    Friend Sub AnalyseForeignKeys(ByVal xnArtefacts As XmlNode)

        Dim xnForeignKeys As XmlNode = xnArtefacts.SelectSingleNode("//foreign_keys")

        Dim alKeys As New ArrayList
        If Not xnForeignKeys Is Nothing Then
            For Each xnForeignKey As XmlNode In xnForeignKeys.ChildNodes

                Dim strParent As String = xnForeignKey.SelectSingleNode("parent/table").InnerText
                Dim strChild As String = xnForeignKey.SelectSingleNode("child/table").InnerText

                Dim tnParent, tnChild As bc_am_dd_key_relationship
                tnParent = Nothing
                tnChild = Nothing
                For Each tnKey As bc_am_dd_key_relationship In alKeys
                    If tnKey.Text = strParent Then
                        tnParent = tnKey
                    ElseIf tnKey.Text = strChild Then
                        tnChild = tnKey
                    End If
                Next
                If tnParent Is Nothing Then
                    tnParent = New bc_am_dd_key_relationship(strParent)
                    alKeys.Add(tnParent)
                End If
                If tnChild Is Nothing Then
                    tnChild = New bc_am_dd_key_relationship(strChild)
                    alKeys.Add(tnChild)
                End If
                tnParent.AddChild(tnChild)

            Next

            For Each tnKey As bc_am_dd_key_relationship In alKeys
                Dim xn As XmlNode = xnArtefacts.SelectSingleNode("//" & tnKey.Text)
                Dim intDepth As Integer = tnKey.GetDepth
                DebugLine(tnKey.Text)
                If Not xn Is Nothing Then
                    Dim xaDepth As XmlAttribute = xn.OwnerDocument.CreateAttribute("depth")
                    xaDepth.Value = intDepth
                    xn.Attributes.Append(xaDepth)
                End If
            Next
        End If

        If Not xnForeignKeys Is Nothing Then
            For Each xnForeignKey As XmlNode In xnForeignKeys.ChildNodes

                Dim xnParent As XmlNode = xnArtefacts.SelectSingleNode("//" & xnForeignKey.SelectSingleNode("parent/table").InnerText)
                If Not xnParent Is Nothing Then
                    Dim xnForeignKeysChild As XmlNode = xnParent.SelectSingleNode("foreign_keys")
                    If xnForeignKeysChild Is Nothing Then
                        xnForeignKeysChild = xnParent.OwnerDocument.CreateNode(XmlNodeType.Element, "foreign_keys", Nothing)
                        xnParent.AppendChild(xnForeignKeysChild)
                    End If
                    xnForeignKeysChild.AppendChild(xnParent.OwnerDocument.ImportNode(xnForeignKey, True))
                End If
                Dim xnChild As XmlNode = xnArtefacts.SelectSingleNode("//" & xnForeignKey.SelectSingleNode("child/table").InnerText)
                If Not xnChild Is Nothing Then
                    Dim xnForeignKeysChild As XmlNode = xnChild.SelectSingleNode("foreign_keys")
                    If xnForeignKeysChild Is Nothing Then
                        xnForeignKeysChild = xnParent.OwnerDocument.CreateNode(XmlNodeType.Element, "foreign_keys", Nothing)
                        xnChild.AppendChild(xnForeignKeysChild)
                    End If
                    xnForeignKeysChild.AppendChild(xnChild.OwnerDocument.ImportNode(xnForeignKey, True))
                End If

            Next
        End If

    End Sub
    Dim iTimeElapsed As Long = 0
    Dim debugAlTableList As New ArrayList
    Private Sub AddArtefactNode(ByRef tnParent As TreeNode, ByRef xnMetaParent As XmlNode, ByRef xnDataParent As XmlNode, _
                                ByRef intTableIndex As Integer, ByVal intTableCount As Integer, _
                                ByRef intNodeIndex As Integer, ByRef intNodeCount As Integer, _
                                ByRef alStoredData As ArrayList, ByVal alParentKeys As ArrayList, ByVal intDepth As Integer)

        intNodeIndex += 1

        If xnMetaParent.Name = "miscellaneous" Then
            Dim s As String = ""
        End If

        If intNodeCount > 0 Then

            'If intNodeIndex Mod 30 = 0 Then
            IncrementProgress(view, intNodeIndex, intNodeCount)
            'End If

        ElseIf xnMetaParent.Attributes("select") Is Nothing Then

            IncrementProgress(view, intTableIndex, intTableCount)

            intTableIndex += 1

        End If

        If Not xnMetaParent.SelectSingleNode("display") Is Nothing Then

            For Each xnChild As XmlNode In xnMetaParent.SelectSingleNode("display").ChildNodes

                Dim badn As bc_am_dd_node = Nothing

                If (xnChild.Attributes("visible") Is Nothing OrElse xnChild.Attributes("visible").Value = 1) Then

                    If Not xnChild.Attributes("select") Is Nothing Then

                        Dim strSqlCommand As String = xnChild.Attributes("select").Value

                        If strSqlCommand.ToLower = "bc_core_dd_get_insight_temp_assign" Then
                            Dim s As String = ""
                        End If

                        Dim xnlResults As XmlNodeList
                        Dim xnTable As XmlNode = xnMetaParent.SelectSingleNode("//" & xnChild.Name)
                        If xnDataParent Is Nothing OrElse xnTable.Attributes("dataSource") Is Nothing OrElse xnTable.Attributes("dataSource").Value = "select" Then
                            Dim ti As Long = DateTime.Now.Ticks
                            Try
                                xnlResults = GetXmlNodeFromQuery(alStoredData, strSqlCommand, xnChild.Name, alParentKeys)
                            Catch ex As Exception
                                Throw ex
                            End Try
                            iTimeElapsed = DateTime.Now.Ticks - ti + iTimeElapsed
                        Else
                            xnlResults = xnDataParent.SelectNodes(xnChild.Name)
                        End If

                        If Not debugAlTableList.Contains(xnChild.Name) Then
                            DebugLine("'" & xnChild.Name & "',")
                            debugAlTableList.Add(xnChild.Name)
                        End If

                        If Not xnlResults Is Nothing Then
                            Dim intResultIndex As Integer = 0
                            For Each xnResult As XmlNode In xnlResults
                                If xnResult.Attributes.Count > 0 OrElse xnResult.ChildNodes.Count > 0 Then
                                    Dim strExport As String = Nothing
                                    Dim strImport As String = Nothing
                                    If Not xnChild.Attributes("export") Is Nothing Then
                                        strExport = xnChild.Attributes("export").Value
                                    End If
                                    If Not xnChild.Attributes("import") Is Nothing Then
                                        strImport = xnChild.Attributes("import").Value
                                    End If
                                    Dim strName As String = Nothing
                                    Dim strFileName As String = Nothing

                                    Dim alKeys As New ArrayList
                                    For Each xnKey As XmlNode In xnChild.SelectNodes("keys/key")
                                        alKeys.Add(New bc_am_dd_key(xnKey.InnerXml, xnResult))
                                    Next

                                    If Not xnChild.Attributes("display") Is Nothing AndAlso Not xnResult.Attributes(xnChild.Attributes("display").Value) Is Nothing Then
                                        strName = xnResult.Attributes(xnChild.Attributes("display").Value).Value
                                    End If

                                    If Not xnChild.Attributes("fileName") Is Nothing Then
                                        strFileName = xnChild.Attributes("fileName").Value
                                    End If

                                    Dim strParentOverride As String = ""
                                    If Not xnChild.Attributes("parentOverride") Is Nothing Then
                                        strParentOverride = xnChild.Attributes("parentOverride").Value
                                    End If

                                    badn = New bc_am_dd_node(strName, strImport, xnChild.Name, xnChild.Attributes("display").Value, False, strFileName, tnParent.FullPath, strParentOverride, alKeys)
                                    badn.SelectStatement = strSqlCommand
                                    badn.ForeignKey = xnChild.SelectSingleNode("foreign_key")

                                    badn.Exportable = Not xnChild.Attributes("export") Is Nothing AndAlso xnChild.Attributes("export").Value = "1"
                                    badn.SelectAll = Not xnChild.Attributes("selectAll") Is Nothing AndAlso xnChild.Attributes("selectAll").Value = "1"
                                    badn.UseFolderGranularity = Not xnChild.Attributes("useFolderGranularity") Is Nothing AndAlso xnChild.Attributes("useFolderGranularity").Value = "1"

                                    badn.NodeXml = xnResult
                                    tnParent.Nodes.Add(badn)
                                    If TypeOf badn.Parent Is bc_am_dd_node Then
                                        badn.UniquePath = CType(badn.Parent, bc_am_dd_node).UniquePath & "\" & badn.UniqueIdentifier
                                    Else
                                        badn.UniquePath = badn.Parent.Name & "\" & badn.UniqueIdentifier
                                    End If
                                    AddToSortedList(badn)

                                    If Not strImport Is Nothing Then
                                        Dim badl As bc_am_dd_named_list = Nothing
                                        Dim boolFound As Boolean = False
                                        For Each badl In alListByCommand
                                            If badl.Name = xnChild.Name Then
                                                boolFound = True
                                                Exit For
                                            End If
                                        Next
                                        If Not boolFound Then
                                            badl = New bc_am_dd_named_list(xnChild.Name)
                                            alListByCommand.Add(badl)
                                        End If
                                        badl.Add(badn.UniqueIdentifier, badn)
                                    End If

                                    If Not strImport Is Nothing Then
                                        Try
                                            If badn.UniqueIdentifier.IndexOf("Market cap") > -1 Then
                                                Dim s As String = ""
                                            End If
                                            htRows.Add(badn.UniqueIdentifier, badn)
                                        Catch exp As Exception
                                            Throw exp
                                        End Try
                                    End If

                                    If xnChild.ChildNodes.Count > 0 Then
                                        AddArtefactNode(badn, xnChild, xnResult, intTableIndex, intTableCount, intNodeIndex, intNodeCount, alStoredData, alKeys, intDepth + 1)
                                    End If
                                    intResultIndex += 1
                                End If
                            Next
                            If xnlResults.Count = 0 Or intResultIndex = 0 Then
                                tnParent.Remove()
                                Exit Sub
                            End If
                        End If

                    Else

                        Dim strDisplayName As String = Nothing
                        If Not xnChild.Attributes("display") Is Nothing Then
                            strDisplayName = xnChild.Attributes("display").Value
                        End If
                        badn = New bc_am_dd_node(FixCapitalisation(xnChild.Name), Nothing, Nothing, xnChild.Name, True, strDisplayName, tnParent.FullPath, Nothing, Nothing)
                        tnParent.Nodes.Add(badn)
                        If TypeOf badn.Parent Is bc_am_dd_node Then
                            badn.UniquePath = CType(badn.Parent, bc_am_dd_node).UniquePath & "\" & badn.UniqueIdentifier
                        Else
                            badn.UniquePath = badn.Parent.Name & "\" & badn.UniqueIdentifier
                        End If
                        AddToSortedList(badn)
                        If badn.IsTable Then
                            alTables.Add(badn)
                        End If

                        AddArtefactNode(badn, xnChild, xnDataParent, intTableIndex, intTableCount, intNodeIndex, intNodeCount, alStoredData, alParentKeys, intDepth + 1)

                    End If

                End If

            Next

        End If

    End Sub

    Friend Sub AddToSortedList(ByVal badn As bc_am_dd_node)
        If Not badn Is Nothing Then
            Try
                slAll.Add(badn.UniquePath, badn)
                'Catch nex As NullReferenceException
                'iii += 1
                'Console.WriteLine("n")
                'Console.WriteLine(nex.Message)
            Catch ex As Exception
                'iij += 1
                'Console.WriteLine("x")
                'Console.WriteLine(ex.Message)
                Throw ex
            End Try
        End If
    End Sub

    Friend Sub ArchiveDestination(Optional ByVal alErrors As ArrayList = Nothing)

        Dim METHOD_NAME As String = System.Reflection.MethodBase.GetCurrentMethod().Name

        Dim log = New bc_cs_activity_log(FORM_NAME, METHOD_NAME, bc_cs_activity_codes.TRACE_ENTRY, "")

        Try

            Dim xdDifferences As New XmlDocument
            xdDifferences.Load(InstructionsFile)

            For Each xnTransaction As XmlNode In xdDifferences.SelectNodes("//transaction")

                Dim xnSource As XmlNode = xnTransaction.ChildNodes(0)

                If alErrors Is Nothing OrElse Not alErrors.Contains(xnSource.ParentNode.ChildNodes(0).OuterXml) Then
                    Dim strTransactionType As String = Nothing
                    If Not xnTransaction.Attributes("type") Is Nothing Then
                        strTransactionType = xnTransaction.Attributes("type").Value
                    End If
                    Dim strSource As String = XmlConvert.DecodeName(xnSource.InnerText)
                    Dim strXml As String = xnSource.ChildNodes(0).OuterXml
                    If strTransactionType = "INSERT" Or strTransactionType = "UPDATE" Then
                        Dim strTarget As String = strSource
                        If strTransactionType = "UPDATE" Then
                            strTarget = XmlConvert.DecodeName(xnTransaction.ChildNodes(1).InnerText)
                        ElseIf strTransactionType = "INSERT" Then
                            strTarget = Replace(strTarget, SourcePath, DestinationPath)
                        End If
                        If Not File.Exists(strTarget) OrElse Not ReadFile(strTarget) = strXml Then
                            WriteFile(strTarget, strXml)
                        End If
                    ElseIf strTransactionType = "DELETE" Then
                        ArchiveFile(strSource)
                    End If
                End If
            Next

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log(FORM_NAME, METHOD_NAME, bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log(FORM_NAME, METHOD_NAME, bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Public Sub ImportInstructions()

        Dim METHOD_NAME As String = System.Reflection.MethodBase.GetCurrentMethod().Name

        Dim log = New bc_cs_activity_log(FORM_NAME, METHOD_NAME, bc_cs_activity_codes.TRACE_ENTRY, "")

        Try

            If InstructionsFile Is Nothing OrElse Not File.Exists(InstructionsFile) OrElse Not isValidFile(InstructionsFile) Then
                ErrorCode = ErrorCodes.IMPORT
                DisplayMessage(view, "You must specify a valid instructions file.", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            If ErrorFile Is Nothing OrElse Not isValidFile(ErrorFile) Then
                ErrorCode = ErrorCodes.IMPORT
                DisplayMessage(view, "You must specify a valid errors file.", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            If UpdateDestinationFiles AndAlso (DestinationPath Is Nothing OrElse Not isValidDirectory(DestinationPath)) Then
                ErrorCode = ErrorCodes.IMPORT
                DisplayMessage(view, "You must specify a destination path if ""Update Files"" is checked.", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            If UpdateDestinationFiles AndAlso (SourcePath Is Nothing OrElse Not isValidDirectory(SourcePath)) Then
                ErrorCode = ErrorCodes.IMPORT
                DisplayMessage(view, "You must specify a source path if ""Update Files"" is checked.", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            Dim xdDifferences As New XmlDocument
            xdDifferences.Load(InstructionsFile)

            Dim boolSuccess As Boolean = True
            Dim alResults As New ArrayList

            Dim intOverride = 0
            Dim intBuffer As Integer = GetInstructionsGranularity()
            Dim intIndex As Integer = 0
            Dim intCount As Integer = xdDifferences.DocumentElement.ChildNodes.Count
            Dim alStringBuilders As New ArrayList()

            Dim boolDisplayProgress As Boolean = False
            If intBuffer > 0 AndAlso intCount > intBuffer Then

                If ValidateOnly AndAlso DisplayMessage(view, "The number of instructions exceeds the buffer count. ""Validate Only"" functionality will not work in this scenario. Proceed?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
                    ErrorCode = ErrorCodes.IMPORT
                    Exit Sub
                End If

                If intCount / intBuffer > 10 Then
                    boolDisplayProgress = True
                    ValidateOnly = False
                    intOverride = 1
                    CreateProgressBar(view, "Importing data...")
                End If

                Dim sb As StringBuilder = Nothing
                For Each xn As XmlNode In xdDifferences.DocumentElement.ChildNodes
                    If boolDisplayProgress Then
                        IncrementProgress(view, "Parsing data...", intIndex, intCount)
                    End If
                    If intIndex Mod intBuffer = 0 Then
                        If Not sb Is Nothing Then
                            sb.Append("</root>")
                            alStringBuilders.Add(sb)
                        End If
                        sb = New StringBuilder()
                        sb.Append("<root>")
                    End If
                    sb.Append(xn.OuterXml)
                    intIndex += 1
                Next

                GC.Collect()

                If Not alStringBuilders.Contains(sb) Then
                    sb.Append("</root>")
                    alStringBuilders.Add(sb)
                End If

                If intCount / intBuffer > 10 Then
                    UnloadProgressBar(view)
                End If

            Else

                Dim sb As New StringBuilder
                sb.Append(xdDifferences.OuterXml)

                alStringBuilders.Add(sb)

            End If

            CreateProgressBar(view, "Importing data...")
            If Not boolDisplayProgress Then
                IncrementProgress(view, "Importing data...", 1, 2, True)
            End If

            Dim xdErrors As New XmlDocument
            xdErrors.LoadXml("<root></root>")

            intIndex = 0
            intCount = alStringBuilders.Count
            For Each sbChunk As StringBuilder In alStringBuilders
                If boolDisplayProgress Then
                    IncrementProgress(view, "Importing data...", intIndex, intCount, True)
                End If
                Dim osql As bc_om_sql = QueryDatabase("exec [bc_core_dd_import] '" & sbChunk.ToString().Replace("'", "''") & "', " & intOverride & ", " & BoolToBit(ValidateOnly), False)
                boolSuccess = osql.success And boolSuccess
                If Not boolSuccess Then
                    Dim s As String = ""
                End If
                If Not osql.results Is Nothing Then
                    alResults.Add(osql.results)
                End If
                intIndex += 1
                GC.Collect()
            Next

            'If boolDisplayProgress Then
            UnloadProgressBar(view)
            'End If

            If alResults.Count > 0 AndAlso Not boolSuccess Then

                'Dim strError As String = ""
                'If alResults.Item(0).length = 2 Then
                '    strErrorFile = ControlChars.Lf & alResults.Item(0)(0) & ": " & alResults.Item(0)(1)
                'End If
                ErrorCode = ErrorCodes.IMPORT
                DisplayMessage(view, "There was a database error during the transaction. Please review.", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub

            ElseIf alResults.Count > 0 AndAlso boolSuccess Then

                DisplayMessage(view, "The query returned errors. Please review.", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Dim sbErrors As New StringBuilder
                sbErrors.Append("<errors>")
                For Each oResult In alResults

                    For i = 0 To UBound(oResult, 2)
                        sbErrors.Append("<error><transaction>")
                        sbErrors.Append(oResult(0, i).ToString())
                        sbErrors.Append("</transaction><message>")
                        sbErrors.Append(oResult(1, i).ToString())
                        sbErrors.Append("</message></error>")
                    Next

                Next

                sbErrors.Append("</errors>")

                Try
                    WriteFile(ErrorFile(), sbErrors.ToString())
                    WriteToConsole("There were errors during the update. Review log file " & ErrorFile() & " for more information.")
                Catch
                    'Error
                    'An error during the log write is not worth bubbling up.
                End Try

                If Not view Is Nothing Then

                    Dim bade = New bc_am_dd_errors

                    Dim alErrors As New ArrayList

                    For Each oResult In alResults
                        'If osql.success And Not osql.results Is Nothing Then
                        For i = 0 To UBound(oResult, 2)
                            Dim tn As TreeNode = bade.tvErrors.Nodes.Add(oResult(0, i).ToString())
                            tn.Nodes.Add(oResult(1, i).ToString())
                            Dim strXml As String = oResult(2, i)
                            If Not strXml Is Nothing AndAlso Not strXml = "" Then
                                Dim xdRow As New XmlDocument
                                xdRow.LoadXml(strXml.Substring(0, oResult(2, i).ToString().IndexOf("<location>")))
                                alErrors.Add(xdRow.OuterXml)
                            End If
                            strXml = oResult(3, i)
                            If Not strXml Is Nothing AndAlso Not strXml = "" Then
                                alErrors.Add(strXml.Substring(0, oResult(3, i).ToString().IndexOf("<location>")))
                            End If
                        Next
                        'End If
                    Next

                    bade.ShowDialog()

                    If bade.boolOverride And Not intOverride = 1 Then
                        QueryDatabase("exec [bc_core_dd_import] '" & xdDifferences.OuterXml.Replace("'", "''") & "', 1, " & BoolToBit(ValidateOnly), False)
                        'could manage individual nodes
                        If Not ValidateOnly AndAlso UpdateDestinationFiles Then
                            ArchiveDestination(alErrors)
                        End If
                        DisplayMessage(view, "Import completed.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    End If

                Else

                    'Console

                End If

            Else
                If Not ValidateOnly AndAlso UpdateDestinationFiles Then
                    ArchiveDestination()
                End If
                DisplayMessage(view, "Import completed.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If

        Catch ex As Exception
            ErrorCode = ErrorCodes.IMPORT
            Dim errLog As New bc_cs_error_log(FORM_NAME, METHOD_NAME, bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log(FORM_NAME, METHOD_NAME, bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Public Sub CompareDatabases()

        Dim METHOD_NAME As String = System.Reflection.MethodBase.GetCurrentMethod().Name

        Dim log = New bc_cs_activity_log(FORM_NAME, METHOD_NAME, bc_cs_activity_codes.TRACE_ENTRY, "")

        Try

            If InstructionsFile Is Nothing OrElse Not isValidFile(InstructionsFile) Then
                'If Not view Is Nothing Then
                DisplayMessage(view, "You must select a valid instructions file.", MessageBoxButtons.OK, MessageBoxIcon.Error)
                'End If
                ErrorCode = ErrorCodes.COMPARE
                Exit Sub
            End If

            If Not Directory.Exists(SourcePath) OrElse Not isValidDirectory(SourcePath) Then
                'If Not view Is Nothing Then
                ErrorCode = ErrorCodes.COMPARE
                DisplayMessage(view, "The source folder does not exist or is not a valid URI.", MessageBoxButtons.OK, MessageBoxIcon.Error)
                'End If
                Exit Sub
            End If

            If Not Directory.Exists(DestinationPath) OrElse Not isValidDirectory(DestinationPath) Then
                'If Not view Is Nothing Then
                DisplayMessage(view, "The destination folder does not exist or is not a valid URI.", MessageBoxButtons.OK, MessageBoxIcon.Error)
                'End If
                ErrorCode = ErrorCodes.COMPARE
                Exit Sub
            End If

            Dim xdResultsInit As New XmlDocument
            xdResultsInit.PreserveWhitespace = True
            xdResultsInit.LoadXml("<root></root>")

            Dim htFiles As New Hashtable

            Dim xdKeys As New XmlDocument
            xdKeys.Load(GetArtefactsFileName())

            AnalyseForeignKeys(xdKeys)

            Dim intIndex, intCount As Integer
            intIndex = 0
            GetFileCount(SourcePath, intCount)

            Dim sbRemovedNodes As New StringBuilder
            sbRemovedNodes.Append("<root>")

            CreateProgressBar(view, "Traversing source tree...")
            IterateFiles(view, htFiles, xdResultsInit, xdKeys, sbRemovedNodes, SourcePath, SourcePath, DestinationPath, QueryType.INSERT, True, intIndex, intCount)
            UnloadProgressBar(view)

            intIndex = 0
            GetFileCount(DestinationPath, intCount)
            CreateProgressBar(view, "Traversing destination tree...")
            IterateFiles(view, htFiles, xdResultsInit, xdKeys, sbRemovedNodes, DestinationPath, DestinationPath, SourcePath, QueryType.DELETE, False, intIndex, intCount)
            UnloadProgressBar(view)

            sbRemovedNodes.Append("</root>")

            Dim xdRemovedNodes As New XmlDocument
            xdRemovedNodes.PreserveWhitespace = True
            xdRemovedNodes.LoadXml(sbRemovedNodes.ToString())

            intCount = xdResultsInit.DocumentElement.ChildNodes.Count
            Dim badts(intCount) As bc_am_dd_transaction
            intIndex = 0
            For Each xnResult As XmlNode In xdResultsInit.DocumentElement.ChildNodes
                badts(intIndex) = New bc_am_dd_transaction(xnResult)
                intIndex += 1
            Next
            Array.Sort(badts)

            Dim xdResults As New XmlDocument
            xdResults.PreserveWhitespace = True
            xdResults.LoadXml("<root></root>")

            For Each badt As bc_am_dd_transaction In badts
                If Not badt Is Nothing Then
                    Dim xnResult As XmlNode = xdResults.ImportNode(badt.xn, True)
                    xdResults.DocumentElement.AppendChild(xnResult)
                End If
            Next

            If CheckDependencies Then

                For Each xnDelete As XmlNode In xdResults.SelectNodes("//transaction[@type=""DELETE""]")
                    For Each xnRow As XmlNode In xnDelete.ChildNodes
                        bc_am_dd_xmldiff.CheckDependencies(xnRow, xdKeys, QueryType.DELETE, xdRemovedNodes, xdResults)
                    Next
                Next

                For Each xnUpdate As XmlNode In xdResults.SelectNodes("//transaction[@type=""UPDATE""]")
                    For Each xnRow As XmlNode In xnUpdate.ChildNodes
                        bc_am_dd_xmldiff.CheckDependencies(xnRow, xdKeys, QueryType.UPDATE, xdRemovedNodes, xdResults)
                    Next
                Next

                For Each xnInsert As XmlNode In xdResults.SelectNodes("//transaction[@type=""INSERT""]")
                    If xnInsert.OuterXml.ToLower.IndexOf("user_table") > -1 Then
                        Dim s As String = ""
                    End If
                    For Each xnRow As XmlNode In xnInsert.ChildNodes
                        bc_am_dd_xmldiff.CheckDependencies(xnRow, xdKeys, QueryType.INSERT, xdRemovedNodes, xdResults)
                    Next
                Next

            End If

            xdResults.Save(InstructionsFile)

            DisplayMessage(view, "Comparison completed.", Nothing, Nothing)

        Catch ex As Exception
            ErrorCode = ErrorCodes.COMPARE
            Dim errLog As New bc_cs_error_log(FORM_NAME, METHOD_NAME, bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log(FORM_NAME, METHOD_NAME, bc_cs_activity_codes.TRACE_EXIT, "")
            UnloadProgressBar(view)
        End Try

    End Sub

    Friend Sub ApplyTreeMask(ByRef tv As bc_am_dd_tree, ByVal strSourceFile As String, ByVal mt As MaskType)

        If File.Exists(strSourceFile) Then

            Dim xdMask As New XmlDocument
            xdMask.Load(strSourceFile)
            ApplyTreeMask(tv, xdMask, mt)

        End If

    End Sub

    Friend Sub ApplyTreeMask(ByRef tv As bc_am_dd_tree, ByVal xdMask As XmlDocument, ByVal mt As MaskType)

        Dim METHOD_NAME As String = System.Reflection.MethodBase.GetCurrentMethod().Name

        Try

            Dim log = New bc_cs_activity_log(FORM_NAME, METHOD_NAME, bc_cs_activity_codes.TRACE_ENTRY, "")

            CreateProgressBar(view, "Applying " & mt.ToString & " mask...")

            Application.DoEvents()

            'tv.Loading = True
            SuspendDrawing(tv)

            Dim intIndex As Integer = 0
            Dim badnDict As DictionaryEntry
            For Each badnDict In htRows
                If mt = MaskType.Include Then
                    badnDict.Value.Export = False
                ElseIf mt = MaskType.Exclude Then
                    badnDict.Value.Exclude = False
                ElseIf mt = MaskType.Archive Then
                    badnDict.Value.Archive = False
                ElseIf mt = MaskType.Display Then
                    badnDict.Value.Checked = False
                End If
                'If intIndex Mod 30 = 0 Then
                IncrementProgress(view, intIndex, htRows.Count * 3)
                'End If
                intIndex += 1
            Next badnDict

            For Each badnTable As bc_am_dd_node In alTables
                If mt = MaskType.Include Then
                    badnTable.Export = False
                ElseIf mt = MaskType.Exclude Then
                    badnTable.Exclude = False
                ElseIf mt = MaskType.Archive Then
                    badnTable.Archive = False
                ElseIf mt = MaskType.Display Then
                    badnTable.Checked = False
                End If
            Next

            Application.DoEvents()

            Dim xnlMasks As XmlNodeList = xdMask.GetElementsByTagName("mask")
            intIndex = 0
            Dim intCount As Integer = xnlMasks.Count
            Dim badn As bc_am_dd_node = Nothing
            For Each xn As XmlNode In xnlMasks

                badn = slAll.Item(XmlConvert.DecodeName(xn.InnerXml))
                If Not badn Is Nothing Then
                    Dim bool As Boolean = xn.Attributes("on") Is Nothing OrElse xn.Attributes("on").Value <> 0
                    If mt = MaskType.Include Then
                        badn.Export = bool
                    ElseIf mt = MaskType.Exclude Then
                        badn.Exclude = bool
                    ElseIf mt = MaskType.Archive Then
                        badn.Archive = bool
                    ElseIf mt = MaskType.Display Then
                        badn.Checked = bool
                    End If
                    If Not mt = MaskType.Display Then
                        tv.PropagateStatus(badn, bool, mt)
                    End If
                End If
                'If intIndex Mod 10 = 0 Then
                IncrementProgress(view, intIndex + intCount * 2, intCount * 4)
                'End If

                intIndex += 1

            Next

            intIndex = 0
            alTables.Reverse()
            For Each badn In alTables

                If badn.Nodes.Count > 0 Then
                    Dim boolParentChecked As Boolean = True
                    For Each badnChild As bc_am_dd_node In badn.Nodes
                        boolParentChecked = boolParentChecked And badnChild.Checked
                    Next
                    If boolParentChecked Then
                        badn.Checked = boolParentChecked
                    End If
                End If

                'If intIndex Mod 3 = 0 Then
                IncrementProgress(view, intIndex + alTables.Count * 3, alTables.Count * 4)
                'End If
                intIndex += 1
            Next
            alTables.Reverse()

            Dim xnType As XmlNode = xdMask.SelectSingleNode("//type")
            If Not view Is Nothing AndAlso Not xnType Is Nothing Then
                view.TreeView().MaskType() = xnType.InnerText
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log(FORM_NAME, METHOD_NAME, bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            'tv.Loading = False
            ResumeDrawing(tv)
            Dim log = New bc_cs_activity_log(FORM_NAME, METHOD_NAME, bc_cs_activity_codes.TRACE_EXIT, "")
            UnloadProgressBar(view)
        End Try

    End Sub

    Friend Function GetTreeMask(ByRef tv As bc_am_dd_tree, ByVal mt As MaskType) As XmlDocument

        Try
            CreateProgressBar(view, "Saving " & mt.ToString & " mask...")

            Dim alResults As ArrayList = IterateNodes(tv.Nodes(0), MaskType.Display)

            For Each badnTable As bc_am_dd_node In alTables
                If badnTable.Checked Then
                    alResults.Add(badnTable)
                End If
            Next

            Dim xdMask As New XmlDocument
            xdMask.LoadXml("<masks><type>" & mt & "</type></masks>")

            tv.MaskType = mt

            Dim intIndex As Integer = 0
            For Each badn As bc_am_dd_node In alResults

                If Not badn.UniquePath Is Nothing Then

                    Dim xn As XmlNode = xdMask.CreateNode(XmlNodeType.Element, "mask", Nothing)
                    xn.InnerXml = XmlConvert.EncodeName(badn.UniquePath)
                    xdMask.ChildNodes(0).AppendChild(xn)

                    For Each badnChild As bc_am_dd_node In badn.Nodes
                        If Not badnChild.Checked Then
                            xn = xdMask.CreateNode(XmlNodeType.Element, "mask", Nothing)
                            xn.InnerXml = XmlConvert.EncodeName(badnChild.UniquePath)
                            Dim xa As XmlAttribute = xdMask.CreateAttribute("on")
                            xa.Value = 0
                            xn.Attributes.Append(xa)
                            xdMask.ChildNodes(0).AppendChild(xn)
                        End If
                    Next

                    IncrementProgress(view, intIndex, alResults.Count)

                    intIndex += 1

                End If

            Next

            view.Refresh()

            Return xdMask

        Catch ex As Exception
            Throw ex
        Finally
            UnloadProgressBar(view)
        End Try

    End Function

    Friend Sub SaveTreeMask(ByRef tv As bc_am_dd_tree, ByVal strDestinationFile As String, ByVal mt As MaskType)

        Dim METHOD_NAME As String = System.Reflection.MethodBase.GetCurrentMethod().Name

        Try

            Dim log = New bc_cs_activity_log(FORM_NAME, METHOD_NAME, bc_cs_activity_codes.TRACE_ENTRY, "")

            DebugLine(DateTime.Now)

            GetTreeMask(tv, mt).Save(strDestinationFile)

            DebugLine(DateTime.Now)

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log(FORM_NAME, METHOD_NAME, bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            '            UnloadProgressBar(view)
            'RefreshView()
            Dim log = New bc_cs_activity_log(FORM_NAME, METHOD_NAME, bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Friend Sub ArchiveSource()

        Dim intIndex, intCount As Integer
        intIndex = 1
        'GetFileCount(strSourcePath, intCount)

        CreateProgressBar(view, "Archiving data...")

        IncrementProgress(view, 0, 1)

        ArchiveSourceHelper(SourcePath, intIndex, intCount)

        UnloadProgressBar(view)

        DebugLine(intIndex)
    End Sub

    Friend Sub ArchiveFile(ByVal strFileName As String)
        If File.Exists(strFileName) Then
            File.Copy(strFileName, strFileName.Substring(0, strFileName.Length - 4) & "." & ArchiveExtension.Replace(".", ""))
            File.Delete(strFileName)
        End If
    End Sub

    Friend Sub ArchiveSourceHelper(ByVal strSourcePath As String, ByRef intIndex As Integer, ByVal intCount As Integer)

        intIndex += 1

        IncrementProgress(view, intIndex, intCount)

        Dim strFiles As String() = Directory.GetFiles(strSourcePath)
        For Each strFileName As String In strFiles
            If strFileName.IndexOf(".xml") = strFileName.Length - 4 Then
                Dim strKey As String = strFileName.Replace(SourcePath, "")
                strKey = strKey.Substring(0, strKey.Length - 4)
                If htRows.Item(strKey) Is Nothing Then
                    ArchiveFile(strFileName)
                End If
            End If
        Next

        Dim strDirectories As String() = Directory.GetDirectories(strSourcePath)
        For Each strDirectory As String In strDirectories
            If (File.GetAttributes(strDirectory) And FileAttributes.ReparsePoint) <> FileAttributes.ReparsePoint Then
                ArchiveSourceHelper(strDirectory, intIndex, intCount)
            End If
        Next

    End Sub

    Friend Sub AddDependencies(ByVal tv As bc_am_dd_tree, ByRef alResults As ArrayList)

        tv.Loading = True

        Dim ddd As DateTime = DateTime.Now

        Dim alResultsTemporary As New ArrayList
        For Each o As Object In alResults
            alResultsTemporary.Add(o)
        Next

        Dim xdKeys As New XmlDocument
        xdKeys.Load(GetArtefactsFileName())

        Dim xnForeignKeys As XmlNode = xdKeys.SelectSingleNode("//foreign_keys")

        Dim alTimes As New ArrayList

        Dim alChildren As New ArrayList
        If Not xnForeignKeys Is Nothing Then
            Dim boolFound As Boolean = False
            For Each xnForeignKey As XmlNode In xnForeignKeys.ChildNodes
                Dim strParent As String = xnForeignKey.SelectSingleNode("parent/table").InnerText
                Dim strChild As String = xnForeignKey.SelectSingleNode("child/table").InnerText
                alChildren.Add(strChild)
            Next
        End If

        Dim alListByNameChecked As New ArrayList
        For Each badnChecked As bc_am_dd_node In alResults
            UpdateList(alListByNameChecked, badnChecked, True, Nothing, badnChecked.TableName)
        Next

        Dim alListByTableKey As New ArrayList
        Dim alAdded As New ArrayList
        For Each xnForeignKey As XmlNode In xnForeignKeys.ChildNodes

            Dim strParent As String = xnForeignKey.SelectSingleNode("parent/table").InnerText
            Dim strParentColumn As String = xnForeignKey.SelectSingleNode("parent/column").InnerText
            Dim strTableKey As String = strParent & " - " & strParentColumn

            Dim boolFoundList As Boolean = False
            Dim badlParents As bc_am_dd_named_list = Nothing
            For Each badlParents In alListByCommand
                If badlParents.Name = strParent Then
                    boolFoundList = True
                    Exit For
                End If
            Next

            If boolFoundList AndAlso Not alAdded.Contains(strTableKey) Then
                alAdded.Add(strTableKey)
                Dim badl As bc_am_dd_named_list = Nothing
                Dim boolFound As Boolean = False
                For Each badl In alListByTableKey
                    If badl.Name = strTableKey Then
                        boolFound = True
                        Exit For
                    End If
                Next
                If Not boolFound Then
                    badl = New bc_am_dd_named_list(strTableKey)
                    alListByTableKey.Add(badl)
                End If
                For Each htRow In badlParents
                    Dim badnParent As bc_am_dd_node = htRow.value
                    badl.Add(badnParent.NodeXml.Attributes(strParentColumn).Value, badnParent)
                Next
            End If
        Next

        Dim alToRemove As New ArrayList
        Dim boolAddedNew As Boolean = False
        Dim boolInit As Boolean = True
        Dim alResultsToAdd As New ArrayList

        While boolInit OrElse boolAddedNew
            boolInit = False
            boolAddedNew = False

            For Each xnForeignKey As XmlNode In xnForeignKeys.ChildNodes

                Dim dt As DateTime = DateTime.Now

                Dim strParent As String = xnForeignKey.SelectSingleNode("parent/table").InnerText
                Dim strChild As String = xnForeignKey.SelectSingleNode("child/table").InnerText
                Dim strParentColumn As String = xnForeignKey.SelectSingleNode("parent/column").InnerText
                Dim strChildColumn As String = xnForeignKey.SelectSingleNode("child/column").InnerText

                If strParent.ToLower = "user_table" Then
                    Dim sss As String = ""
                End If

                Dim strTableKey As String = strParent & " - " & strParentColumn
                Dim boolChildFound As Boolean = False
                Dim boolParentFound As Boolean = False

                Dim badnlChild As bc_am_dd_named_list = Nothing
                Dim badnlParent As bc_am_dd_named_list = Nothing

                For Each badnl As bc_am_dd_named_list In alListByNameChecked
                    If badnl.Name = strChild Then
                        badnlChild = badnl
                        boolChildFound = True
                        Exit For
                    End If
                Next

                'If Not boolChildFound Then
                '    For Each badnl As bc_am_dd_named_list In alListByCommand
                '        If badnl.Name = strChild Then
                '            badnlChild = badnl
                '            boolChildFound = True
                '            Exit For
                '        End If
                '    Next
                'End If

                For Each badnl As bc_am_dd_named_list In alListByTableKey
                    If badnl.Name = strTableKey Then
                        badnlParent = badnl
                        boolParentFound = True
                        Exit For
                    End If
                Next

                Dim badnParent As bc_am_dd_node
                Dim boolFound As Boolean = False
                If boolChildFound And boolParentFound Then
                    For Each badnlRow In badnlChild
                        boolFound = False
                        Dim badn As bc_am_dd_node = badnlRow.Value
                        If Not badn.Parent Is Nothing AndAlso Not badn.Parent.Parent Is Nothing AndAlso TypeOf badn.Parent.Parent Is bc_am_dd_node Then
                            badnParent = badn.Parent.Parent
                            If badnParent.TableName = strParent AndAlso IsParent(badnParent, badn, strParent, strParentColumn, strChildColumn) Then
                                boolFound = True
                                If Not badnParent.Checked Then
                                    If Not alResultsToAdd.Contains(badnParent) Then
                                        alResultsToAdd.Add(badnParent)
                                        badnParent.Checked = True
                                        boolAddedNew = True
                                    End If
                                    'ElseIf Not alToRemove.Contains(badn) Then
                                    '    alToRemove.Add(badn)
                                    'End If
                                End If
                            End If
                            If Not boolFound AndAlso Not badn.NodeXml.Attributes(strChildColumn) Is Nothing Then
                                Dim badnSearched As bc_am_dd_node = badnlParent(badn.NodeXml.Attributes(strChildColumn).Value)
                                If badnSearched Is Nothing Then
                                    'Error
                                    Dim ss As String = ""
                                ElseIf Not badnSearched.Checked Then
                                    If Not alResultsToAdd.Contains(badnSearched) Then
                                        alResultsToAdd.Add(badnSearched)
                                        badnSearched.Checked = True
                                        boolAddedNew = True
                                    End If
                                    'Exit For
                                End If
                            End If
                        End If
                    Next
                End If

                Dim intSeconds As Integer = DateTime.Now.Subtract(dt).Ticks / TimeSpan.TicksPerSecond

                If intSeconds > 0 Then
                    alTimes.Add(strParent & " - " & strChild)
                    alTimes.Add(intSeconds)
                End If

            Next

            alResultsTemporary.Clear()
            For Each o As Object In alResultsToAdd
                alResultsTemporary.Add(o)
                If Not alResults.Contains(o) Then
                    alResults.Add(o)
                End If
            Next
            alResultsToAdd.Clear()

            alListByNameChecked = New ArrayList
            For Each badnChecked As bc_am_dd_node In alResultsTemporary
                UpdateList(alListByNameChecked, badnChecked, True, Nothing, badnChecked.TableName)
            Next

        End While

        Dim ddd2 As TimeSpan = DateTime.Now.Subtract(ddd)

        Dim s As String = ""

        tv.Loading = False

    End Sub

    Friend Sub ExportDatabase(ByVal tv As TreeView, ByVal mt As MaskType)

        Dim METHOD_NAME As String = System.Reflection.MethodBase.GetCurrentMethod().Name

        Try

            Dim log = New bc_cs_activity_log(FORM_NAME, METHOD_NAME, bc_cs_activity_codes.TRACE_ENTRY, "")

            DebugLine(DateTime.Now)

            If Not view Is Nothing AndAlso (view.TreeView().MaskType() = MaskType.Exclude OrElse view.TreeView.MaskType = MaskType.Archive) Then
                Dim strPrefix As String = " "
                If view.TreeView().MaskType().ToString().Substring(0, 1) = "E" Then
                    strPrefix = "n "
                End If
                'If DisplayMessage(view, _
                '    "You have selected a" & strPrefix & view.TreeView().MaskType().ToString().ToLower() & " tree for export. Are you sure you wish to continue?", _
                '        MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = DialogResult.No Then
                '    Exit Sub
                'End If
            End If

            If SourcePath Is Nothing OrElse Not isValidDirectory(SourcePath) Then
                ErrorCode = ErrorCodes.EXPORT
                DisplayMessage(view, "You must select a valid source path for export.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If

            If Not strIncludeFile Is Nothing Then
                ApplyTreeMask(tv, strIncludeFile, MaskType.Include)
            ElseIf Not IncludeXml Is Nothing Then
                ApplyTreeMask(tv, IncludeXml, MaskType.Include)
            End If

            Dim alResults As ArrayList = IterateNodes(tv.Nodes(0), mt)

            Dim intIndex, intCount As Integer
            intIndex = 0
            intCount = alResults.Count

            DebugLine(intCount)

            If intCount = 0 Then
                ErrorCode = ErrorCodes.EXPORT
                DisplayMessage(view, "You must select at least one node for export.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If

            For Each badn As bc_am_dd_node In alResults
                badn.Exclude = False
            Next

            ApplyTreeMask(tv, strExclusionsFile, MaskType.Exclude)

            If Not Directory.Exists(SourcePath) Then
                Directory.CreateDirectory(SourcePath)
            End If

            If SourceArchived Then
                ArchiveSource()
            End If

            If IncludeDependencies Then
                AddDependencies(tv, alResults)
            End If

            intCount = alResults.Count

            CreateProgressBar(view, "Exporting data...")

            For Each badn As bc_am_dd_node In alResults
                If badn.GetFullPath(SourcePath).Length > 264 Then
                    Throw New Exception("File path """ & badn.GetFullPath(SourcePath) & """ is greater than 264 characters in length. Export will be cancelled.")
                End If
            Next

            Dim alStoredData As New ArrayList
            For Each badn As bc_am_dd_node In alResults

                If Not badn.IsTable Then
                    IncrementProgress(view, intIndex, intCount)
                Else
                    IncrementProgress(view, "Exporting " & badn.Text & " data...", intIndex, intCount)
                End If

                If Not badn.Parent Is Nothing Then
                    badn.ExportNode(SourcePath, alStoredData, OverwriteAll)
                End If
                intIndex += 1
            Next

            DebugLine(DateTime.Now)

            UnloadProgressBar(view)

            DisplayMessage(view, "Export completed.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            'Catch ptle As PathTooLongException
            '    ErrorCode = ErrorCodes.EXPORT
            '    Dim errLog As New bc_cs_error_log(FORM_NAME, METHOD_NAME, bc_cs_error_codes.USER_DEFINED, "The file or folder name is too long.")
        Catch ex As Exception
            ErrorCode = ErrorCodes.EXPORT
            Dim errLog As New bc_cs_error_log(FORM_NAME, METHOD_NAME, bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Dim log = New bc_cs_activity_log(FORM_NAME, METHOD_NAME, bc_cs_activity_codes.TRACE_EXIT, "")
            UnloadProgressBar(view)
        End Try
    End Sub

    Private Function IterateNodes(ByVal badnRoot As bc_am_dd_node, ByVal mt As MaskType) As ArrayList

        Dim alResults As New ArrayList

        Dim badnCurrent As bc_am_dd_node = badnRoot

        Dim strTime As String = DateTime.Now.ToFileTime

        Dim badn As DictionaryEntry
        For Each badn In htRows
            If (mt = MaskType.Display And badn.Value.Checked) OrElse (mt = MaskType.Include And badn.Value.Export) OrElse (mt = MaskType.Archive And badn.Value.Archive) Then
                alResults.Add(badn.Value)
            End If
        Next badn

        Return alResults

    End Function

    Private Sub RefreshView()
        If Not view Is Nothing Then
            view.Refresh()
            Application.DoEvents()
        End If
    End Sub

    'Private Sub view_ConfigurationButtonPressed() Handles view.ConfigurationButtonPressed
    '    Dim view As New bc_am_cf_main
    '    Dim controller As New bc_am_configuration(view)
    '    controller.Show()
    'End Sub

    Private Sub view_AboutButtonPressed() Handles View.AboutButtonPressed
        Dim baa As New bc_as_about
        baa.SetProductAndVersion(Application.ProductName, Application.ProductVersion)
        baa.ShowDialog(view)
    End Sub

End Class