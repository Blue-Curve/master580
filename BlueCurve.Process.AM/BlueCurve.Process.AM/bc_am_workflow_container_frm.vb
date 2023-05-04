Imports System.Windows.Forms
Imports BlueCurve.Core.CS
Imports BlueCurve.Core.AS
Imports BlueCurve.Create.AM
Imports System.Collections
Imports System.Threading
Imports System.Runtime.InteropServices

Public Class bc_am_workflow_container_frm
    Inherits System.Windows.Forms.Form
    Public Shared owfs As bc_am_workflow_summary
    Dim owf As New bc_am_workflow
    Public ofrmsummary As bc_am_workflow_summary
    Public just_refreshed As Boolean
    Private loading As Boolean
    Public low_res As Boolean = False
    Private isInitialised As Boolean = False
    Public columnorder As New ArrayList
    Public columnwidth As New ArrayList
    'Constants that map to indices in CheckBoxImages ImageList
    Private Const CHECK_OFF As Integer = 0
    Private Const CHECK_ON As Integer = 1
    Private Const CHECK_OFF_2 As Integer = 2
    Friend WithEvents madmin As System.Windows.Forms.MenuItem
    Friend WithEvents mforce As System.Windows.Forms.MenuItem
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Private Const CHECK_ON_2 As Integer = 3
    Public LastActivity As DateTime = Now()
    Public SummaryFormWidth As Long


#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        isInitialised = True
    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents MainMenu1 As System.Windows.Forms.MainMenu
    Friend WithEvents MenuItem5 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem9 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem10 As System.Windows.Forms.MenuItem
    Friend WithEvents TopPanel As System.Windows.Forms.Panel
    Friend WithEvents TopBannerPic As System.Windows.Forms.PictureBox
    Friend WithEvents ProcessImagePic As System.Windows.Forms.PictureBox
    Friend WithEvents lstatus As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents lpubtypes As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents lentities As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents lstage As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents lauthor As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ldate As System.Windows.Forms.GroupBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents pdateto As System.Windows.Forms.PictureBox
    Friend WithEvents pdatefrom As System.Windows.Forms.PictureBox
    Friend WithEvents BCLogoPic As System.Windows.Forms.PictureBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents lpoll As System.Windows.Forms.Label
    Friend WithEvents lserverstatus As System.Windows.Forms.Label
    Friend WithEvents lblServerConnection As System.Windows.Forms.Label
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents FilterDateTo As System.Windows.Forms.DateTimePicker
    Friend WithEvents FilterDateFrom As System.Windows.Forms.DateTimePicker
    Friend WithEvents RadioImages As System.Windows.Forms.ImageList
    Friend WithEvents CheckBoxImages As System.Windows.Forms.ImageList
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents SearchButton As System.Windows.Forms.PictureBox
    Friend WithEvents FileMenu As System.Windows.Forms.MenuItem
    Friend WithEvents FileMenu_QuitProcess As System.Windows.Forms.MenuItem
    Friend WithEvents CreateMenu As System.Windows.Forms.MenuItem
    Friend WithEvents CreateMenu_Document As System.Windows.Forms.MenuItem
    Friend WithEvents ImportMenu As System.Windows.Forms.MenuItem
    Friend WithEvents ImportMenu_MasteDocument As System.Windows.Forms.MenuItem
    Friend WithEvents ImportMenu_SupportDoc As System.Windows.Forms.MenuItem
    Friend WithEvents InsightMenu As System.Windows.Forms.MenuItem
    Friend WithEvents InsightMenuLaunchExcel As System.Windows.Forms.MenuItem
    Friend WithEvents DisplayMenu As System.Windows.Forms.MenuItem
    Friend WithEvents DisplayMenu_Refresh As System.Windows.Forms.MenuItem
    Friend WithEvents DisplayMenu_FiltersReset As System.Windows.Forms.MenuItem
    Friend WithEvents DisplayMenu_SearchClear As System.Windows.Forms.MenuItem
    Friend WithEvents DisplayMenu_MatchesOnly As System.Windows.Forms.MenuItem
    Friend WithEvents DisplayMenu_Settings As System.Windows.Forms.MenuItem
    Friend WithEvents HelpMenu As System.Windows.Forms.MenuItem
    Friend WithEvents HelpMenu_About As System.Windows.Forms.MenuItem
    Friend WithEvents FilterPanel As System.Windows.Forms.Panel
    Friend WithEvents MainMenu2 As System.Windows.Forms.MainMenu
    Friend WithEvents DisplayMenu_Filters_Hide As System.Windows.Forms.MenuItem
    Friend WithEvents tsearch As System.Windows.Forms.TextBox
    Friend WithEvents MenuItem1 As System.Windows.Forms.MenuItem
    Friend WithEvents Chkpublish As System.Windows.Forms.CheckBox
    Friend WithEvents mregister As System.Windows.Forms.MenuItem
    Friend WithEvents mregdoc As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem2 As System.Windows.Forms.MenuItem
    Friend WithEvents mimpreg As System.Windows.Forms.MenuItem
    Friend WithEvents Mregrep As System.Windows.Forms.MenuItem
    Friend WithEvents mreattach As System.Windows.Forms.MenuItem
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_workflow_container_frm))
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.lserverstatus = New System.Windows.Forms.Label
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.lblServerConnection = New System.Windows.Forms.Label
        Me.lpoll = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.BCLogoPic = New System.Windows.Forms.PictureBox
        Me.MainMenu1 = New System.Windows.Forms.MainMenu(Me.components)
        Me.FileMenu = New System.Windows.Forms.MenuItem
        Me.MenuItem5 = New System.Windows.Forms.MenuItem
        Me.FileMenu_QuitProcess = New System.Windows.Forms.MenuItem
        Me.CreateMenu = New System.Windows.Forms.MenuItem
        Me.CreateMenu_Document = New System.Windows.Forms.MenuItem
        Me.ImportMenu = New System.Windows.Forms.MenuItem
        Me.ImportMenu_MasteDocument = New System.Windows.Forms.MenuItem
        Me.Mregrep = New System.Windows.Forms.MenuItem
        Me.ImportMenu_SupportDoc = New System.Windows.Forms.MenuItem
        Me.MenuItem2 = New System.Windows.Forms.MenuItem
        Me.mimpreg = New System.Windows.Forms.MenuItem
        Me.mreattach = New System.Windows.Forms.MenuItem
        Me.mregister = New System.Windows.Forms.MenuItem
        Me.mregdoc = New System.Windows.Forms.MenuItem
        Me.madmin = New System.Windows.Forms.MenuItem
        Me.mforce = New System.Windows.Forms.MenuItem
        Me.InsightMenu = New System.Windows.Forms.MenuItem
        Me.InsightMenuLaunchExcel = New System.Windows.Forms.MenuItem
        Me.DisplayMenu = New System.Windows.Forms.MenuItem
        Me.DisplayMenu_Refresh = New System.Windows.Forms.MenuItem
        Me.MenuItem9 = New System.Windows.Forms.MenuItem
        Me.DisplayMenu_FiltersReset = New System.Windows.Forms.MenuItem
        Me.DisplayMenu_Filters_Hide = New System.Windows.Forms.MenuItem
        Me.MenuItem10 = New System.Windows.Forms.MenuItem
        Me.DisplayMenu_SearchClear = New System.Windows.Forms.MenuItem
        Me.DisplayMenu_MatchesOnly = New System.Windows.Forms.MenuItem
        Me.DisplayMenu_Settings = New System.Windows.Forms.MenuItem
        Me.MenuItem1 = New System.Windows.Forms.MenuItem
        Me.HelpMenu = New System.Windows.Forms.MenuItem
        Me.HelpMenu_About = New System.Windows.Forms.MenuItem
        Me.RadioImages = New System.Windows.Forms.ImageList(Me.components)
        Me.TopPanel = New System.Windows.Forms.Panel
        Me.ProcessImagePic = New System.Windows.Forms.PictureBox
        Me.TopBannerPic = New System.Windows.Forms.PictureBox
        Me.tsearch = New System.Windows.Forms.TextBox
        Me.SearchButton = New System.Windows.Forms.PictureBox
        Me.FilterPanel = New System.Windows.Forms.Panel
        Me.Chkpublish = New System.Windows.Forms.CheckBox
        Me.ldate = New System.Windows.Forms.GroupBox
        Me.FilterDateTo = New System.Windows.Forms.DateTimePicker
        Me.FilterDateFrom = New System.Windows.Forms.DateTimePicker
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.pdateto = New System.Windows.Forms.PictureBox
        Me.pdatefrom = New System.Windows.Forms.PictureBox
        Me.lauthor = New System.Windows.Forms.ListView
        Me.ColumnHeader5 = New System.Windows.Forms.ColumnHeader
        Me.CheckBoxImages = New System.Windows.Forms.ImageList(Me.components)
        Me.lstage = New System.Windows.Forms.ListView
        Me.ColumnHeader4 = New System.Windows.Forms.ColumnHeader
        Me.lentities = New System.Windows.Forms.ListView
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.lpubtypes = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.lstatus = New System.Windows.Forms.ListView
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.MainMenu2 = New System.Windows.Forms.MainMenu(Me.components)
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.Panel2.SuspendLayout()
        Me.Panel3.SuspendLayout()
        CType(Me.BCLogoPic, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TopPanel.SuspendLayout()
        CType(Me.ProcessImagePic, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TopBannerPic, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SearchButton, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.FilterPanel.SuspendLayout()
        Me.ldate.SuspendLayout()
        CType(Me.pdateto, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pdatefrom, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.White
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel2.Controls.Add(Me.lserverstatus)
        Me.Panel2.Controls.Add(Me.Panel3)
        Me.Panel2.Controls.Add(Me.lblServerConnection)
        Me.Panel2.Controls.Add(Me.lpoll)
        Me.Panel2.Controls.Add(Me.Label4)
        Me.Panel2.Controls.Add(Me.BCLogoPic)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel2.Location = New System.Drawing.Point(0, 613)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1028, 64)
        Me.Panel2.TabIndex = 2
        '
        'lserverstatus
        '
        Me.lserverstatus.Font = New System.Drawing.Font("Verdana", 7.25!)
        Me.lserverstatus.Location = New System.Drawing.Point(620, 16)
        Me.lserverstatus.Name = "lserverstatus"
        Me.lserverstatus.Size = New System.Drawing.Size(240, 16)
        Me.lserverstatus.TabIndex = 27
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.Label1)
        Me.Panel3.Controls.Add(Me.Label2)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Right
        Me.Panel3.Location = New System.Drawing.Point(768, 0)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(256, 60)
        Me.Panel3.TabIndex = 31
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Verdana", 7.25!)
        Me.Label1.Location = New System.Drawing.Point(8, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(232, 16)
        Me.Label1.TabIndex = 32
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Verdana", 7.25!)
        Me.Label2.Location = New System.Drawing.Point(8, 40)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(232, 16)
        Me.Label2.TabIndex = 31
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblServerConnection
        '
        Me.lblServerConnection.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblServerConnection.Font = New System.Drawing.Font("Verdana", 7.25!)
        Me.lblServerConnection.Location = New System.Drawing.Point(304, 40)
        Me.lblServerConnection.Name = "lblServerConnection"
        Me.lblServerConnection.Size = New System.Drawing.Size(456, 16)
        Me.lblServerConnection.TabIndex = 29
        '
        'lpoll
        '
        Me.lpoll.Font = New System.Drawing.Font("Verdana", 7.25!)
        Me.lpoll.Location = New System.Drawing.Point(304, 16)
        Me.lpoll.Name = "lpoll"
        Me.lpoll.Size = New System.Drawing.Size(344, 17)
        Me.lpoll.TabIndex = 26
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(4, 42)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(296, 24)
        Me.Label4.TabIndex = 25
        Me.Label4.Text = "© Blue Curve Limited. All rights reserved. "
        '
        'BCLogoPic
        '
        Me.BCLogoPic.Image = CType(resources.GetObject("BCLogoPic.Image"), System.Drawing.Image)
        Me.BCLogoPic.Location = New System.Drawing.Point(5, 6)
        Me.BCLogoPic.Name = "BCLogoPic"
        Me.BCLogoPic.Size = New System.Drawing.Size(296, 32)
        Me.BCLogoPic.TabIndex = 24
        Me.BCLogoPic.TabStop = False
        '
        'MainMenu1
        '
        Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.FileMenu, Me.CreateMenu, Me.ImportMenu, Me.mregister, Me.madmin, Me.InsightMenu, Me.DisplayMenu, Me.HelpMenu})
        '
        'FileMenu
        '
        Me.FileMenu.Index = 0
        Me.FileMenu.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem5, Me.FileMenu_QuitProcess})
        Me.FileMenu.Text = "File"
        '
        'MenuItem5
        '
        Me.MenuItem5.Index = 0
        Me.MenuItem5.Text = "-"
        '
        'FileMenu_QuitProcess
        '
        Me.FileMenu_QuitProcess.Index = 1
        Me.FileMenu_QuitProcess.Text = "Quit"
        '
        'CreateMenu
        '
        Me.CreateMenu.Index = 1
        Me.CreateMenu.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.CreateMenu_Document})
        Me.CreateMenu.Text = "Create"
        '
        'CreateMenu_Document
        '
        Me.CreateMenu_Document.Index = 0
        Me.CreateMenu_Document.Text = "Document"
        '
        'ImportMenu
        '
        Me.ImportMenu.Index = 2
        Me.ImportMenu.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.ImportMenu_MasteDocument, Me.Mregrep, Me.ImportMenu_SupportDoc, Me.MenuItem2, Me.mimpreg, Me.mreattach})
        Me.ImportMenu.Text = "Import"
        '
        'ImportMenu_MasteDocument
        '
        Me.ImportMenu_MasteDocument.Index = 0
        Me.ImportMenu_MasteDocument.Text = "Master Document"
        '
        'Mregrep
        '
        Me.Mregrep.Index = 1
        Me.Mregrep.Text = "Regular Report"
        Me.Mregrep.Visible = False
        '
        'ImportMenu_SupportDoc
        '
        Me.ImportMenu_SupportDoc.Enabled = False
        Me.ImportMenu_SupportDoc.Index = 2
        Me.ImportMenu_SupportDoc.Text = "Support Document"
        '
        'MenuItem2
        '
        Me.MenuItem2.Index = 3
        Me.MenuItem2.Text = "-"
        '
        'mimpreg
        '
        Me.mimpreg.Enabled = False
        Me.mimpreg.Index = 4
        Me.mimpreg.Text = "Registered Document"
        '
        'mreattach
        '
        Me.mreattach.Enabled = False
        Me.mreattach.Index = 5
        Me.mreattach.Text = "Reattach Master Document"
        '
        'mregister
        '
        Me.mregister.Index = 3
        Me.mregister.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mregdoc})
        Me.mregister.Text = "Register"
        '
        'mregdoc
        '
        Me.mregdoc.Index = 0
        Me.mregdoc.Text = "Master Document"
        '
        'madmin
        '
        Me.madmin.Index = 4
        Me.madmin.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mforce})
        Me.madmin.Text = "Administration"
        Me.madmin.Visible = False
        '
        'mforce
        '
        Me.mforce.Index = 0
        Me.mforce.Text = "Force Check In"
        '
        'InsightMenu
        '
        Me.InsightMenu.Index = 5
        Me.InsightMenu.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.InsightMenuLaunchExcel})
        Me.InsightMenu.Text = "Insight"
        Me.InsightMenu.Visible = False
        '
        'InsightMenuLaunchExcel
        '
        Me.InsightMenuLaunchExcel.Index = 0
        Me.InsightMenuLaunchExcel.Text = "Launch Excel"
        '
        'DisplayMenu
        '
        Me.DisplayMenu.Index = 6
        Me.DisplayMenu.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.DisplayMenu_Refresh, Me.MenuItem9, Me.MenuItem10, Me.DisplayMenu_Settings, Me.MenuItem1})
        Me.DisplayMenu.Text = "Display"
        '
        'DisplayMenu_Refresh
        '
        Me.DisplayMenu_Refresh.Index = 0
        Me.DisplayMenu_Refresh.Text = "Refresh"
        '
        'MenuItem9
        '
        Me.MenuItem9.Index = 1
        Me.MenuItem9.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.DisplayMenu_FiltersReset, Me.DisplayMenu_Filters_Hide})
        Me.MenuItem9.Text = "Filters"
        '
        'DisplayMenu_FiltersReset
        '
        Me.DisplayMenu_FiltersReset.Index = 0
        Me.DisplayMenu_FiltersReset.Text = "Reset"
        '
        'DisplayMenu_Filters_Hide
        '
        Me.DisplayMenu_Filters_Hide.Index = 1
        Me.DisplayMenu_Filters_Hide.Text = "Hide"
        '
        'MenuItem10
        '
        Me.MenuItem10.Index = 2
        Me.MenuItem10.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.DisplayMenu_SearchClear, Me.DisplayMenu_MatchesOnly})
        Me.MenuItem10.Text = "Search"
        '
        'DisplayMenu_SearchClear
        '
        Me.DisplayMenu_SearchClear.Index = 0
        Me.DisplayMenu_SearchClear.Text = "Clear"
        '
        'DisplayMenu_MatchesOnly
        '
        Me.DisplayMenu_MatchesOnly.Index = 1
        Me.DisplayMenu_MatchesOnly.Text = "Matches Only"
        '
        'DisplayMenu_Settings
        '
        Me.DisplayMenu_Settings.Index = 3
        Me.DisplayMenu_Settings.Text = "Settings"
        '
        'MenuItem1
        '
        Me.MenuItem1.Index = 4
        Me.MenuItem1.Text = "Deselect"
        '
        'HelpMenu
        '
        Me.HelpMenu.Index = 7
        Me.HelpMenu.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.HelpMenu_About})
        Me.HelpMenu.Text = "Help"
        '
        'HelpMenu_About
        '
        Me.HelpMenu_About.Index = 0
        Me.HelpMenu_About.Text = "About"
        '
        'RadioImages
        '
        Me.RadioImages.ImageStream = CType(resources.GetObject("RadioImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.RadioImages.TransparentColor = System.Drawing.Color.Transparent
        Me.RadioImages.Images.SetKeyName(0, "")
        Me.RadioImages.Images.SetKeyName(1, "")
        Me.RadioImages.Images.SetKeyName(2, "")
        Me.RadioImages.Images.SetKeyName(3, "")
        Me.RadioImages.Images.SetKeyName(4, "")
        '
        'TopPanel
        '
        Me.TopPanel.BackColor = System.Drawing.Color.White
        Me.TopPanel.Controls.Add(Me.ProcessImagePic)
        Me.TopPanel.Controls.Add(Me.TopBannerPic)
        Me.TopPanel.Controls.Add(Me.tsearch)
        Me.TopPanel.Controls.Add(Me.SearchButton)
        Me.TopPanel.Dock = System.Windows.Forms.DockStyle.Top
        Me.TopPanel.Location = New System.Drawing.Point(0, 0)
        Me.TopPanel.Name = "TopPanel"
        Me.TopPanel.Size = New System.Drawing.Size(1028, 64)
        Me.TopPanel.TabIndex = 9
        '
        'ProcessImagePic
        '
        Me.ProcessImagePic.Dock = System.Windows.Forms.DockStyle.Right
        Me.ProcessImagePic.Image = CType(resources.GetObject("ProcessImagePic.Image"), System.Drawing.Image)
        Me.ProcessImagePic.Location = New System.Drawing.Point(958, 0)
        Me.ProcessImagePic.Name = "ProcessImagePic"
        Me.ProcessImagePic.Size = New System.Drawing.Size(70, 64)
        Me.ProcessImagePic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.ProcessImagePic.TabIndex = 1
        Me.ProcessImagePic.TabStop = False
        '
        'TopBannerPic
        '
        Me.TopBannerPic.Image = CType(resources.GetObject("TopBannerPic.Image"), System.Drawing.Image)
        Me.TopBannerPic.Location = New System.Drawing.Point(0, 0)
        Me.TopBannerPic.Name = "TopBannerPic"
        Me.TopBannerPic.Size = New System.Drawing.Size(500, 66)
        Me.TopBannerPic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.TopBannerPic.TabIndex = 0
        Me.TopBannerPic.TabStop = False
        '
        'tsearch
        '
        Me.tsearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tsearch.Font = New System.Drawing.Font("Verdana", 7.25!)
        Me.tsearch.Location = New System.Drawing.Point(760, 16)
        Me.tsearch.Name = "tsearch"
        Me.tsearch.Size = New System.Drawing.Size(160, 19)
        Me.tsearch.TabIndex = 24
        '
        'SearchButton
        '
        Me.SearchButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SearchButton.BackColor = System.Drawing.Color.Transparent
        Me.SearchButton.Image = CType(resources.GetObject("SearchButton.Image"), System.Drawing.Image)
        Me.SearchButton.Location = New System.Drawing.Point(928, 16)
        Me.SearchButton.Name = "SearchButton"
        Me.SearchButton.Size = New System.Drawing.Size(24, 24)
        Me.SearchButton.TabIndex = 24
        Me.SearchButton.TabStop = False
        '
        'FilterPanel
        '
        Me.FilterPanel.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.FilterPanel.Controls.Add(Me.Chkpublish)
        Me.FilterPanel.Controls.Add(Me.ldate)
        Me.FilterPanel.Controls.Add(Me.lauthor)
        Me.FilterPanel.Controls.Add(Me.lstage)
        Me.FilterPanel.Controls.Add(Me.lentities)
        Me.FilterPanel.Controls.Add(Me.lpubtypes)
        Me.FilterPanel.Controls.Add(Me.lstatus)
        Me.FilterPanel.Dock = System.Windows.Forms.DockStyle.Top
        Me.FilterPanel.Location = New System.Drawing.Point(0, 64)
        Me.FilterPanel.Name = "FilterPanel"
        Me.FilterPanel.Padding = New System.Windows.Forms.Padding(0, 64, 0, 0)
        Me.FilterPanel.Size = New System.Drawing.Size(1028, 138)
        Me.FilterPanel.TabIndex = 10
        '
        'Chkpublish
        '
        Me.Chkpublish.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Chkpublish.Font = New System.Drawing.Font("Verdana", 7.0!)
        Me.Chkpublish.Location = New System.Drawing.Point(463, 122)
        Me.Chkpublish.Name = "Chkpublish"
        Me.Chkpublish.Size = New System.Drawing.Size(136, 16)
        Me.Chkpublish.TabIndex = 23
        Me.Chkpublish.Text = "Show Publish"
        '
        'ldate
        '
        Me.ldate.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ldate.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.ldate.Controls.Add(Me.FilterDateTo)
        Me.ldate.Controls.Add(Me.FilterDateFrom)
        Me.ldate.Controls.Add(Me.Label10)
        Me.ldate.Controls.Add(Me.Label9)
        Me.ldate.Controls.Add(Me.Label8)
        Me.ldate.Controls.Add(Me.Label6)
        Me.ldate.Controls.Add(Me.pdateto)
        Me.ldate.Controls.Add(Me.pdatefrom)
        Me.ldate.Font = New System.Drawing.Font("Verdana", 7.25!)
        Me.ldate.Location = New System.Drawing.Point(768, 4)
        Me.ldate.Name = "ldate"
        Me.ldate.Size = New System.Drawing.Size(257, 116)
        Me.ldate.TabIndex = 22
        Me.ldate.TabStop = False
        Me.ldate.Text = "Date"
        '
        'FilterDateTo
        '
        Me.FilterDateTo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FilterDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.FilterDateTo.Location = New System.Drawing.Point(48, 56)
        Me.FilterDateTo.Name = "FilterDateTo"
        Me.FilterDateTo.Size = New System.Drawing.Size(177, 19)
        Me.FilterDateTo.TabIndex = 1
        Me.FilterDateTo.Value = New Date(2008, 5, 21, 14, 48, 27, 382)
        '
        'FilterDateFrom
        '
        Me.FilterDateFrom.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FilterDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.FilterDateFrom.Location = New System.Drawing.Point(48, 24)
        Me.FilterDateFrom.Name = "FilterDateFrom"
        Me.FilterDateFrom.Size = New System.Drawing.Size(177, 19)
        Me.FilterDateFrom.TabIndex = 0
        Me.FilterDateFrom.Value = New Date(2008, 5, 21, 14, 48, 27, 428)
        '
        'Label10
        '
        Me.Label10.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.Label10.Location = New System.Drawing.Point(48, 56)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(96, 16)
        Me.Label10.TabIndex = 7
        Me.Label10.Text = "No Date range"
        '
        'Label9
        '
        Me.Label9.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.Label9.Location = New System.Drawing.Point(48, 24)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(104, 16)
        Me.Label9.TabIndex = 6
        Me.Label9.Text = "No Date Range"
        '
        'Label8
        '
        Me.Label8.BackColor = System.Drawing.Color.Transparent
        Me.Label8.Location = New System.Drawing.Point(8, 56)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(40, 16)
        Me.Label8.TabIndex = 5
        Me.Label8.Text = "To"
        '
        'Label6
        '
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Location = New System.Drawing.Point(8, 24)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(40, 16)
        Me.Label6.TabIndex = 4
        Me.Label6.Text = "From"
        '
        'pdateto
        '
        Me.pdateto.BackColor = System.Drawing.Color.Transparent
        Me.pdateto.Image = CType(resources.GetObject("pdateto.Image"), System.Drawing.Image)
        Me.pdateto.Location = New System.Drawing.Point(233, 56)
        Me.pdateto.Name = "pdateto"
        Me.pdateto.Size = New System.Drawing.Size(18, 24)
        Me.pdateto.TabIndex = 3
        Me.pdateto.TabStop = False
        '
        'pdatefrom
        '
        Me.pdatefrom.BackColor = System.Drawing.Color.Transparent
        Me.pdatefrom.Image = CType(resources.GetObject("pdatefrom.Image"), System.Drawing.Image)
        Me.pdatefrom.Location = New System.Drawing.Point(233, 24)
        Me.pdatefrom.Name = "pdatefrom"
        Me.pdatefrom.Size = New System.Drawing.Size(24, 24)
        Me.pdatefrom.TabIndex = 2
        Me.pdatefrom.TabStop = False
        '
        'lauthor
        '
        Me.lauthor.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lauthor.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader5})
        Me.lauthor.Font = New System.Drawing.Font("Verdana", 7.25!)
        Me.lauthor.Location = New System.Drawing.Point(615, 8)
        Me.lauthor.Name = "lauthor"
        Me.lauthor.Size = New System.Drawing.Size(150, 112)
        Me.lauthor.SmallImageList = Me.CheckBoxImages
        Me.lauthor.TabIndex = 21
        Me.ToolTip1.SetToolTip(Me.lauthor, "Select one or more authors to filter the list of documents by")
        Me.lauthor.UseCompatibleStateImageBehavior = False
        Me.lauthor.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "Author"
        Me.ColumnHeader5.Width = 400
        '
        'CheckBoxImages
        '
        Me.CheckBoxImages.ImageStream = CType(resources.GetObject("CheckBoxImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.CheckBoxImages.TransparentColor = System.Drawing.Color.Transparent
        Me.CheckBoxImages.Images.SetKeyName(0, "")
        Me.CheckBoxImages.Images.SetKeyName(1, "")
        Me.CheckBoxImages.Images.SetKeyName(2, "")
        Me.CheckBoxImages.Images.SetKeyName(3, "")
        '
        'lstage
        '
        Me.lstage.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lstage.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader4})
        Me.lstage.Font = New System.Drawing.Font("Verdana", 7.25!)
        Me.lstage.Location = New System.Drawing.Point(463, 8)
        Me.lstage.Name = "lstage"
        Me.lstage.Size = New System.Drawing.Size(150, 112)
        Me.lstage.SmallImageList = Me.CheckBoxImages
        Me.lstage.TabIndex = 20
        Me.ToolTip1.SetToolTip(Me.lstage, "Select one or more workflow stages from this list to filter the document list by")
        Me.lstage.UseCompatibleStateImageBehavior = False
        Me.lstage.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Stage"
        Me.ColumnHeader4.Width = 400
        '
        'lentities
        '
        Me.lentities.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lentities.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader2})
        Me.lentities.Font = New System.Drawing.Font("Verdana", 7.25!)
        Me.lentities.Location = New System.Drawing.Point(311, 8)
        Me.lentities.Name = "lentities"
        Me.lentities.Size = New System.Drawing.Size(150, 112)
        Me.lentities.SmallImageList = Me.CheckBoxImages
        Me.lentities.TabIndex = 19
        Me.ToolTip1.SetToolTip(Me.lentities, "Select one or more entities from this list to filter the list of documents by")
        Me.lentities.UseCompatibleStateImageBehavior = False
        Me.lentities.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Entity"
        Me.ColumnHeader2.Width = 400
        '
        'lpubtypes
        '
        Me.lpubtypes.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lpubtypes.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1})
        Me.lpubtypes.Font = New System.Drawing.Font("Verdana", 7.25!)
        Me.lpubtypes.Location = New System.Drawing.Point(160, 8)
        Me.lpubtypes.Name = "lpubtypes"
        Me.lpubtypes.Size = New System.Drawing.Size(150, 112)
        Me.lpubtypes.SmallImageList = Me.CheckBoxImages
        Me.lpubtypes.TabIndex = 18
        Me.ToolTip1.SetToolTip(Me.lpubtypes, "Select one or more different publication types from this list to filter the list " & _
                "of documents by")
        Me.lpubtypes.UseCompatibleStateImageBehavior = False
        Me.lpubtypes.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Publication Type"
        Me.ColumnHeader1.Width = 400
        '
        'lstatus
        '
        Me.lstatus.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lstatus.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader3})
        Me.lstatus.Font = New System.Drawing.Font("Verdana", 7.25!)
        Me.lstatus.Location = New System.Drawing.Point(4, 8)
        Me.lstatus.Name = "lstatus"
        Me.lstatus.Scrollable = False
        Me.lstatus.Size = New System.Drawing.Size(150, 112)
        Me.lstatus.SmallImageList = Me.CheckBoxImages
        Me.lstatus.TabIndex = 17
        Me.ToolTip1.SetToolTip(Me.lstatus, "Select the document status to filter the list of documents by")
        Me.lstatus.UseCompatibleStateImageBehavior = False
        Me.lstatus.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Status"
        Me.ColumnHeader3.Width = 185
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(61, 4)
        '
        'bc_am_workflow_container_frm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(1028, 677)
        Me.Controls.Add(Me.FilterPanel)
        Me.Controls.Add(Me.TopPanel)
        Me.Controls.Add(Me.Panel2)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.IsMdiContainer = True
        Me.Menu = Me.MainMenu1
        Me.Name = "bc_am_workflow_container_frm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Blue Curve - Process"
        Me.Panel2.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        CType(Me.BCLogoPic, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TopPanel.ResumeLayout(False)
        Me.TopPanel.PerformLayout()
        CType(Me.ProcessImagePic, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TopBannerPic, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SearchButton, System.ComponentModel.ISupportInitialize).EndInit()
        Me.FilterPanel.ResumeLayout(False)
        Me.ldate.ResumeLayout(False)
        CType(Me.pdateto, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pdatefrom, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub bc_am_workflow_container_frm_Layout(ByVal sender As Object, ByVal e As System.Windows.Forms.LayoutEventArgs) Handles Me.Layout

        If Me.WindowState = FormWindowState.Minimized Then
            'If Me.WindowState = FormWindowState.Minimized Then
            SummaryFormWidth = ofrmsummary.Width
        End If
    End Sub

    Private Sub bc_am_workflow_container_frm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim otrace As New bc_cs_activity_log("bc_am_workflow_container", "new", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try

            Me.loading = True
            Dim i As Integer
            For i = 0 To bc_am_load_objects.obc_users.user.Count - 1
                With bc_am_load_objects.obc_users.user(i)
                    If bc_cs_central_settings.logged_on_user_id = .id Then
                        Me.Label1.Text = "User: " + .first_name + " " + .surname
                        Me.Label2.Text = "Role: " + .role
                        Exit For
                    End If
                End With
            Next

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                Me.lblServerConnection.Text = "Connected via Ado: " + bc_cs_central_settings.servername
            Else
                Me.lblServerConnection.Text = "Connected via Soap: " + bc_cs_central_settings.soap_server
            End If
            bc_am_workflow.ofrm = New bc_am_workflow_frm
            bc_am_workflow.ofrm.MdiParent = Me
            bc_am_workflow.ofrm.Dock = Windows.Forms.DockStyle.Left

            ofrmsummary = New bc_am_workflow_summary
            ofrmsummary.MdiParent = Me
            ofrmsummary.Dock = Windows.Forms.DockStyle.Right
            ofrmsummary.setupForm(bc_am_workflow.summary_displayed, bc_am_workflow.summary_pinned)

            bc_am_workflow.ofrm.Show()
            ofrmsummary.Show()
            ofrmsummary.ScrollControlIntoView(ofrmsummary.ToggleSummaryForm)
            bc_am_workflow.snew = True
            ofrmsummary.ScrollControlIntoView(ofrmsummary.ToggleSummaryForm)
            SummaryFormWidth = ofrmsummary.Width

            setDefaultFormWidth()

            REM new status
            Me.lstatus.Items.Clear()
            Me.lstatus.SmallImageList = Me.RadioImages

            Dim oitem As New ListViewItem(CStr("All"))
            oitem.SubItems.Add("All")
            oitem.SubItems.Add("All")
            oitem.SubItems.Add("All")
            oitem.SubItems.Add("All")
            oitem.SubItems.Add("All")

            REM dates
            'Me.FilterDateFrom.Visible = True
            Me.FilterDateFrom.CustomFormat = "dd MMM yyyy"
            Me.pdatefrom.Image = Me.CheckBoxImages.Images(CHECK_OFF_2)
            If bc_am_workflow.fdatefrom <> "9-9-9999" Then
                'Me.FilterDateFrom.Visible = True
                Me.FilterDateFrom.Value = bc_am_workflow.fdatefrom
                Me.pdatefrom.Image = Me.CheckBoxImages.Images(CHECK_ON_2)
            Else
                Me.FilterDateFrom.Visible = True
                Me.pdatefrom.Image = Me.CheckBoxImages.Images(CHECK_ON_2)
                Dim t As New TimeSpan(28, 0, 0, 0, 0)
                bc_am_workflow.fdatefrom = Now.Subtract(t)
                Me.FilterDateFrom.Value = Now.Subtract(t)
            End If
            'Me.FilterDateTo.Visible = True
            Me.FilterDateTo.CustomFormat = "dd MMM yyyy"
            Me.pdateto.Image = Me.CheckBoxImages.Images(CHECK_OFF_2)
            If bc_am_workflow.fdateto <> "9-9-9999" Then
                'Me.FilterDateTo.Visible = True
                Me.FilterDateTo.Value = bc_am_workflow.fdateto
                Me.pdateto.Image = Me.CheckBoxImages.Images(CHECK_ON_2)
            Else
                Me.FilterDateTo.Visible = False
                Me.FilterDateTo.Value = Now
            End If
            If bc_am_load_objects.obc_pub_types.regular_reports.regular_reports.Count > 0 Then
                Me.Mregrep.Visible = True
            End If
            REM user  picture
            'Try
            'If bc_am_load_objects.obc_prefs.picture.Length > 0 Then
            'Dim fs As New bc_cs_file_transfer_services
            'fs.write_bytestream_to_document(bc_cs_central_settings.local_repos_path + "tmp.bmp", bc_am_load_objects.obc_prefs.picture, Nothing)
            'Dim im As New System.Drawing.Bitmap(bc_cs_central_settings.local_repos_path + "tmp.bmp")
            'Me.puser.Image = im
            'End If
            'Catch
            '    Dim ocomentary As New bc_cs_activity_log("bc_am_workflow_container", "new", bc_cs_activity_codes.COMMENTARY, "Failed to load users picture")
            'End Try
            If columnorder.Count > 0 Then
                For i = 0 To bc_am_workflow.ofrm.DocumentList.Columns.Count - 1
                    bc_am_workflow.ofrm.DocumentList.Columns(i).DisplayIndex = columnorder(i)
                    bc_am_workflow.ofrm.DocumentList.Columns(i).Width = columnwidth(i)
                Next
            End If
            bc_am_workflow.cfrm.FilterPanel.Height = bc_am_workflow.filterpanelheight
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_workflow_container", "new", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            adjust_for_resolution()
            size_filters()
            ofrmsummary.ScrollControlIntoView(ofrmsummary.DocTitle)
            bc_am_workflow.ofrm.DocumentList.Height = bc_am_workflow.ofrm.Height - 20
            Me.loading = False
            bc_am_workflow.snew = False
            otrace = New bc_cs_activity_log("bc_am_workflow_container", "new", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    Private Sub adjust_for_resolution()

        Dim intX As Integer = Screen.PrimaryScreen.Bounds.Width

        If intX = 1024 Then
            low_res = True
            Me.ofrmsummary.DocDetailsBox.Height = 120
            Me.ofrmsummary.Workflow.Height = 90
            Me.ofrmsummary.WorkflowBox.Height = Me.ofrmsummary.WorkflowBox.Height - 35

            Dim dp As Drawing.Point
            dp.X = Me.ofrmsummary.Workflow.Location.X
            dp.Y = Me.ofrmsummary.Workflow.Location.Y - 5
            Me.ofrmsummary.Workflow.Location = dp
            dp.X = Me.ofrmsummary.WorkflowBox.Location.X
            dp.Y = Me.ofrmsummary.WorkflowBox.Location.Y - 35
            Me.ofrmsummary.WorkflowBox.Location = dp

            dp.X = Me.ofrmsummary.DocDetailsTabs.Location.X()
            dp.Y = Me.ofrmsummary.DocDetailsTabs.Location.Y - 60
            Me.ofrmsummary.DocDetailsTabs.Location = dp

        End If
    End Sub

    'Make sure the workflow list form is preset to 
    'an initial width that leaves enough room for the 
    'summary form
    Private Sub setDefaultFormWidth()
        'CAB - may need a way to adjust form settings for 
        '      machines running at a much lower resolution
        'If ofrmsummary.is_form_displayed Then
        Dim intX As Integer = Screen.PrimaryScreen.Bounds.Width
        'Dim intY As Integer = Screen.PrimaryScreen.Bounds.Height
        If intX = 1024 Then
            bc_am_workflow.cfrm.ofrmsummary.Width = 54
        Else
            bc_am_workflow.cfrm.ofrmsummary.Width = 54
        End If
        bc_am_workflow.cfrm.ofrmsummary.ScrollControlIntoView(bc_am_workflow.cfrm.ofrmsummary.ToggleSummaryForm)
        'Else
        '   owf.ofrm.Width = Me.Width - 40  'Not sure why this needs to be 40 
        'End If
    End Sub

    Public Sub cursor_wait()
        Cursor.Current = Cursors.WaitCursor
    End Sub
    Public Sub cursor_default()
        Cursor.Current = Cursors.Default
    End Sub

    Private Sub MenuItem6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DisplayMenu_Settings.Click
        Dim ofrmsettings As New bc_am_workflow_settings
        ofrmsettings.ShowDialog()
    End Sub
    Public Sub set_server_status(ByVal status As String)
        Me.lserverstatus.Text = status
        Me.Refresh()
    End Sub
    Public Sub clear_server_status()
        Me.lserverstatus.Text = ""
    End Sub
    Public Sub set_polling_status(ByVal status As String)
        Me.lpoll.Text = status
        Me.Refresh()
    End Sub
    Public Sub clear_polling_status()
        Me.lpoll.Text = ""
    End Sub

    Public Sub setColors(ByVal newColors As ProcessColors)
        bc_am_workflow.colors = newColors
        bc_am_workflow.ofrm.load_data(bc_am_workflow.mode)
        bc_am_workflow.ofrm.apply_sort()
        If Not owfs Is Nothing Then
            owfs.load_doc()
            owfs.Refresh()
        End If
        If Not ofrmsummary Is Nothing And bc_am_workflow.ofrm.DocumentList.SelectedItems.Count > 0 Then
            ofrmsummary.load_doc()
            ofrmsummary.Refresh()
        End If

    End Sub

    Private Sub Form1_Quit(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Closed
        bc_cs_central_settings.process_client_poll_enabled = False
        Try
            workflow_exit()
        Catch

        End Try
    End Sub
    Private Sub workflow_exit()
        Try

            bc_am_workflow.write_user_settings_fo_file()
            Dim tclose As New Thread(AddressOf exit_thread)
            tclose.Start()
        Catch ex As Exception

        End Try
    End Sub
    Private Sub exit_thread()
        REM dont close if in a poll
        While bc_am_workflow.polling_process = True
            Thread.Sleep(1000)
        End While
        Thread.Sleep(1000)
        Me.Close()
    End Sub

    Private Sub Quit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FileMenu_QuitProcess.Click
        bc_am_workflow.write_user_settings_fo_file()
        Me.Close()

    End Sub
    Private Sub remove_duplicates(ByRef iarray As ArrayList)
        Dim i, j As Integer
        Dim oarray As New ArrayList
        Dim found As Boolean
        oarray.Clear()
        For i = 0 To iarray.Count - 1
            If i = 0 Then
                oarray.Add(iarray(0))
            End If
            For j = 0 To oarray.Count - 1
                found = False
                If oarray(j) = iarray(i) Then
                    found = True
                    Exit For
                End If
            Next
            If found = False Then
                oarray.Add(iarray(i))
            End If
        Next
        iarray = oarray
    End Sub

    Private Sub search(ByVal search_str As String)
        Dim otrace As New bc_cs_activity_log("bc_am_workflow_container_frm", "search", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim i, j, k As Integer
            Dim found As Boolean
            Dim col As System.Drawing.Color
            search_str = Me.tsearch.Text

            Me.lserverstatus.Text = "Searching"
            For i = 0 To bc_am_workflow.ofrm.DocumentList.Items.Count - 1
                found = False
                col = bc_am_workflow.ofrm.DocumentList.Items(i).BackColor
                For j = 0 To bc_am_workflow.ofrm.DocumentList.Items(i).SubItems.Count - 1
                    If InStr(bc_am_workflow.ofrm.DocumentList.Items(i).SubItems(j).Text, search_str, CompareMethod.Text) > 0 Then
                        bc_am_workflow.ofrm.DocumentList.Items(i).BackColor = bc_am_workflow.colors.doc_list_search_backcolor
                        bc_am_workflow.ofrm.DocumentList.Items(i).ForeColor = bc_am_workflow.colors.doc_list_search_forecolor
                        found = True
                        REM flag doc as searched
                        For k = 0 To bc_am_workflow.docs.document.Count - 1
                            If bc_am_workflow.docs.document(k).id = bc_am_workflow.ofrm.DocumentList.Items(i).SubItems(1).Text Then
                                bc_am_workflow.docs.document(k).search_flag = True
                                Exit For
                            End If
                        Next
                    End If
                Next
            Next
            bc_am_workflow.ofrm.DocumentList.Refresh()

            Me.lserverstatus.Text = "Search Complete"
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_workflow_container_frm", "search", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_am_workflow_container_frm", "search", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Private Sub MenuItem7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DisplayMenu_Refresh.Click
        Dim rstate As Boolean
        rstate = bc_am_workflow.auto_refresh
        bc_am_workflow.auto_refresh = True
        Dim owf As New bc_am_workflow
        owf.retrieve_docs(False)
        bc_am_workflow.auto_refresh = rstate

    End Sub

    Public Sub new_load_filters()
        bc_am_workflow.otaxonomies.Clear()
        new_load_pub_types()
        new_load_entities()
        new_load_stages()
        new_load_authors()
        REM now populate listboxes
        Dim i As Integer
        Dim lv As ListViewItem

        Me.lstatus.Items.Clear()
        lv = New ListViewItem("All")
        lv.ImageIndex = 0
        Me.lstatus.Items.Add(lv)
        lv = New ListViewItem("New")
        lv.ImageIndex = 0
        Me.lstatus.Items.Add(lv)
        lv = New ListViewItem("Urgent")
        lv.ImageIndex = 0
        Me.lstatus.Items.Add(lv)
        lv = New ListViewItem("Expired")
        lv.ImageIndex = 0
        Me.lstatus.Items.Add(lv)
        lv = New ListViewItem("Stage Changed")
        lv.ImageIndex = 0
        Me.lstatus.Items.Add(lv)
        Me.lstatus.Items(bc_am_workflow.mode).ImageIndex = 1

        REM pub types
        Me.lpubtypes.Items.Clear()
        lv = New ListViewItem("All")
        lv.ImageIndex = 0
        If bc_am_workflow.otaxonomies(0).all = True Then
            lv.ImageIndex = 1
        End If
        Me.lpubtypes.Items.Add(lv)

        For i = 0 To bc_am_workflow.otaxonomies(0).taxonomy_items.count - 1
            lv = New ListViewItem(CStr(bc_am_workflow.otaxonomies(0).taxonomy_items(i).name))
            lv.ImageIndex = 0
            If bc_am_workflow.otaxonomies(0).taxonomy_items(i).selected = True Then
                lv.ImageIndex = 1
            End If
            Me.lpubtypes.Items.Add(lv)
        Next
        REM entities
        Me.lentities.Items.Clear()
        lv = New ListViewItem("All")
        lv.ImageIndex = 0
        If bc_am_workflow.otaxonomies(1).all = True Then
            lv.ImageIndex = 1
        End If
        Me.lentities.Items.Add(lv)

        For i = 0 To bc_am_workflow.otaxonomies(1).taxonomy_items.count - 1
            lv = New ListViewItem(CStr(bc_am_workflow.otaxonomies(1).taxonomy_items(i).name))
            lv.ImageIndex = 0
            If bc_am_workflow.otaxonomies(1).taxonomy_items(i).selected = True Then
                lv.ImageIndex = 1
            End If
            Me.lentities.Items.Add(lv)
        Next
        REM stages
        Me.lstage.Items.Clear()
        lv = New ListViewItem("All")
        lv.ImageIndex = 0
        If bc_am_workflow.otaxonomies(2).all = True Then
            lv.ImageIndex = 1
        End If
        Me.lstage.Items.Add(lv)

        For i = 0 To bc_am_workflow.otaxonomies(2).taxonomy_items.count - 1
            lv = New ListViewItem(CStr(bc_am_workflow.otaxonomies(2).taxonomy_items(i).name))
            lv.ImageIndex = 0
            If bc_am_workflow.otaxonomies(2).taxonomy_items(i).selected = True Then
                lv.ImageIndex = 1
            End If
            Me.lstage.Items.Add(lv)
        Next
        REM author
        Me.lauthor.Items.Clear()
        lv = New ListViewItem("All")
        lv.ImageIndex = 0
        If bc_am_workflow.otaxonomies(3).all = True Then
            lv.ImageIndex = 1
        End If
        Me.lauthor.Items.Add(lv)

        For i = 0 To bc_am_workflow.otaxonomies(3).taxonomy_items.count - 1
            lv = New ListViewItem(CStr(bc_am_workflow.otaxonomies(3).taxonomy_items(i).name))
            lv.ImageIndex = 0
            If bc_am_workflow.otaxonomies(3).taxonomy_items(i).selected = True Then
                lv.ImageIndex = 1
            End If
            Me.lauthor.Items.Add(lv)
        Next


    End Sub

    Private Sub Create_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CreateMenu.Click

    End Sub
    Public at_running As Boolean = False
    Private Sub MenuItem8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CreateMenu_Document.Click
        Me.CreateMenu_Document.Enabled = False
        REM invoke author tool in new thread
        If at_running = True Then
            Exit Sub
        End If
        Dim atthread As New Thread(AddressOf at_start)
        atthread.SetApartmentState(System.Threading.ApartmentState.STA)
        atthread.Start()
    End Sub
    Private Sub at_start()
        Dim tpoll As Boolean
        Dim owf As New bc_am_workflow
        tpoll = bc_am_workflow.polling_enabled
        bc_am_workflow.polling_enabled = False
        bc_am_workflow.polling_enabled = False
        at_running = True
        'added to handle Office 2010 startup changes
        MessageFilter.Register()
        Dim fwizard_main As New bc_am_at_wizard_main
        fwizard_main.from_create = True
        fwizard_main.from_workflow = True
        fwizard_main.ShowDialog()
        fwizard_main = Nothing
        'added to handle Office 2010 startup changes
        MessageFilter.Revoke()
        at_running = False
        Me.CreateMenu_Document.Enabled = True
        bc_am_workflow.polling_enabled = tpoll
    End Sub
    Private Sub Document_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ImportMenu_MasteDocument.Click

        Dim owf As New bc_am_workflow
        owf.import_master_document(bc_am_workflow.cfrm.ofrmsummary.ldoc, False)

    End Sub

    Private Sub OpenFileDialog1_FileOk(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs)
        Dim odialog = New OpenFileDialog
        odialog.ShowDialog()
    End Sub

    Private Sub MenuItem9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ImportMenu_SupportDoc.Click
        Dim owf As New bc_am_workflow
        owf.import_support_document(bc_am_workflow.cfrm.ofrmsummary.ldoc)
    End Sub
    Public min_flag As Boolean = False
    Private Sub bc_am_workflow_container_frm_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.SizeChanged
        Try
            Dim tpoll As Boolean

            If Me.WindowState <> FormWindowState.Minimized And min_flag = True Then
                ofrmsummary.Width = SummaryFormWidth
            End If

            If Me.WindowState = FormWindowState.Minimized Then
                min_flag = True
                'Need to set form width again here otherwise when form is maximised 
                'it will look awful
                Dim intX As Integer = Screen.PrimaryScreen.Bounds.Width
                If bc_am_workflow.snapsummary = True Then
                    If intX <= 1024 Then
                        bc_am_workflow.ofrm.Width = 556
                    Else
                        bc_am_workflow.ofrm.Width = 856
                    End If
                Else
                    'ofrmsummary.Width = SummaryFormWidth
                    'setDefaultFormWidth()
                End If
            End If
            If Me.WindowState <> FormWindowState.Minimized And min_flag = True Then
                Dim owf As New bc_am_workflow
                tpoll = bc_am_workflow.polling_enabled
                bc_am_workflow.polling_enabled = False
                owf.retrieve_docs(False, False)
                min_flag = False
                bc_am_workflow.polling_enabled = tpoll
                If bc_am_workflow.selected_doc_id = 0 Then
                    bc_am_workflow.cfrm.ofrmsummary.no_selection()
                End If
            End If
            size_filters()
            bc_am_workflow.ofrm.DocumentList.Height = bc_am_workflow.ofrm.Height - 20
        Catch

        End Try

    End Sub

    Private Sub sort(ByRef oarray As ArrayList)
        Dim i, j As Integer
        For i = 1 To oarray.Count - 1
            For j = 0 To oarray.Count - 2
                swap(oarray, oarray.Count - 1 - j)
            Next
        Next
    End Sub
    Private Sub swap(ByRef oarray As ArrayList, ByVal i As Integer)
        Dim t As Object
        If oarray(i).name < oarray(i - 1).name Then
            t = oarray(i - 1)
            oarray(i - 1) = oarray(i)
            oarray(i) = t
        End If

    End Sub
    Private Sub new_load_pub_types()
        Dim i, j As Integer
        remove_duplicates(bc_am_workflow.lpub_types)
        Dim t1 As New taxonomy
        Dim k As Integer
        t1.name = "Publication type"
        t1.all = False
        If bc_am_workflow.spubtype_id.Count = 0 Then
            t1.all = True
        End If
        For i = 0 To bc_am_workflow.lpub_types.Count - 1
            For j = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                If bc_am_workflow.lpub_types(i) = bc_am_load_objects.obc_pub_types.pubtype(j).id Then
                    Dim t2 As New taxonomy_Item
                    t2.id = bc_am_load_objects.obc_pub_types.pubtype(j).id
                    t2.name = bc_am_load_objects.obc_pub_types.pubtype(j).name
                    t2.selected = False
                    REM see if pub type has been selected 
                    For k = 0 To bc_am_workflow.spubtype_id.Count - 1
                        If bc_am_workflow.spubtype_id(k) = bc_am_workflow.lpub_types(i) Then
                            t2.selected = True
                            Exit For
                        End If

                    Next
                    t1.taxonomy_items.Add(t2)
                    Exit For
                End If
            Next
        Next
        sort(t1.taxonomy_items)
        bc_am_workflow.otaxonomies.Add(t1)
    End Sub
    Private Sub new_load_entities()
        Dim i, j As Integer
        remove_duplicates(bc_am_workflow.lentity)
        Dim t1 As New taxonomy
        Dim k As Integer
        t1.name = "Entity"
        t1.all = False
        If bc_am_workflow.sentity_id.Count = 0 Then
            t1.all = True
        End If

        For i = 0 To bc_am_workflow.lentity.Count - 1
            For j = 0 To bc_am_load_objects.obc_entities.entity.Count - 1
                If bc_am_workflow.lentity(i) = bc_am_load_objects.obc_entities.entity(j).id Then
                    Dim t2 As New taxonomy_Item
                    t2.id = bc_am_load_objects.obc_entities.entity(j).id
                    t2.name = bc_am_load_objects.obc_entities.entity(j).name
                    t2.selected = False
                    REM see if pub type has been selected 
                    For k = 0 To bc_am_workflow.sentity_id.Count - 1
                        If bc_am_workflow.sentity_id(k) = bc_am_workflow.lentity(i) Then
                            t2.selected = True
                            Exit For
                        End If

                    Next
                    t1.taxonomy_items.Add(t2)
                    Exit For
                End If
            Next
        Next
        sort(t1.taxonomy_items)
        bc_am_workflow.otaxonomies.Add(t1)
    End Sub

    Public Sub new_load_stages()
        Dim i As Integer
        remove_duplicates(bc_am_workflow.lstages)
        Dim t1 As New taxonomy
        Dim k As Integer
        t1.name = "Stage"
        t1.all = False
        If bc_am_workflow.sstage.Count = 0 Then
            t1.all = True
        End If

        For i = 0 To bc_am_workflow.lstages.Count - 1
            Dim t2 As New taxonomy_Item
            t2.id = bc_am_workflow.lstages(i)
            t2.name = bc_am_workflow.lstages(i)
            t2.selected = False
            REM see if pub type has been selected 
            For k = 0 To bc_am_workflow.sstage.Count - 1
                If bc_am_workflow.sstage(k) = bc_am_workflow.lstages(i) Then
                    t2.selected = True
                    Exit For
                End If
            Next
            t1.taxonomy_items.Add(t2)
        Next
        sort(t1.taxonomy_items)
        bc_am_workflow.otaxonomies.Add(t1)
    End Sub
    Public Sub new_load_authors()
        Dim i, j As Integer
        remove_duplicates(bc_am_workflow.lauthor)
        Dim t1 As New taxonomy
        Dim k As Integer
        t1.name = "Authors"
        t1.all = False
        If bc_am_workflow.sauthor_id.Count = 0 Then
            t1.all = True
        End If

        For i = 0 To bc_am_workflow.lauthor.Count - 1
            For j = 0 To bc_am_load_objects.obc_users.user.Count - 1
                If bc_am_workflow.lauthor(i) = bc_am_load_objects.obc_users.user(j).id Then
                    Dim t2 As New taxonomy_Item
                    t2.id = bc_am_load_objects.obc_users.user(j).id
                    t2.name = bc_am_load_objects.obc_users.user(j).first_name + " " + bc_am_load_objects.obc_users.user(j).surname
                    t2.selected = False
                    REM see if pub type has been selected 
                    For k = 0 To bc_am_workflow.sauthor_id.Count - 1
                        If bc_am_workflow.sauthor_id(k) = bc_am_workflow.lauthor(i) Then
                            t2.selected = True
                            Exit For
                        End If

                    Next
                    t1.taxonomy_items.Add(t2)
                    Exit For
                End If
            Next
        Next
        sort(t1.taxonomy_items)
        bc_am_workflow.otaxonomies.Add(t1)

    End Sub
    Private Sub lpubtypes_mouseup(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lpubtypes.MouseUp
        Try
            REM if first item then all
            Dim id As String
            Dim i As Integer
            If Me.lpubtypes.SelectedItems(0).Index = 0 Then
                bc_am_workflow.spubtype_id.Clear()
            Else
                If Me.lpubtypes.SelectedItems(0).ImageIndex = 0 Then
                    bc_am_workflow.spubtype_id.Add(bc_am_workflow.otaxonomies(0).taxonomy_items(Me.lpubtypes.SelectedItems(0).Index - 1).id)
                Else
                    id = bc_am_workflow.otaxonomies(0).taxonomy_items(Me.lpubtypes.SelectedItems(0).Index - 1).id
                    For i = 0 To bc_am_workflow.spubtype_id.Count - 1
                        If bc_am_workflow.spubtype_id(i) = id Then
                            bc_am_workflow.spubtype_id.RemoveAt(i)
                            Exit For
                        End If
                    Next
                End If

            End If
            Dim owf As New bc_am_workflow
            owf.retrieve_docs(False, False, True, True)
        Catch

        End Try
    End Sub

    Private Sub lentities_mouseup(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lentities.MouseUp
        Try
            REM if first item then all
            Dim i As Integer
            Dim id As String
            If Me.lentities.SelectedItems(0).Index = 0 Then
                bc_am_workflow.sentity_id.Clear()
            Else
                If Me.lentities.SelectedItems(0).ImageIndex = 0 Then
                    bc_am_workflow.sentity_id.Add(bc_am_workflow.otaxonomies(1).taxonomy_items(Me.lentities.SelectedItems(0).Index - 1).id)
                Else
                    id = bc_am_workflow.otaxonomies(1).taxonomy_items(Me.lentities.SelectedItems(0).Index - 1).id
                    For i = 0 To bc_am_workflow.sentity_id.Count - 1
                        If bc_am_workflow.sentity_id(i) = id Then
                            bc_am_workflow.sentity_id.RemoveAt(i)
                            Exit For
                        End If
                    Next
                End If
            End If
            Dim owf As New bc_am_workflow
            owf.retrieve_docs(False, False, True, True)
        Catch ex As Exception
            

        End Try

    End Sub

    Private Sub SearchButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SearchButton.Click
        bc_am_workflow.matches_only = False
        Me.SearchButton.BorderStyle = BorderStyle.Fixed3D
        runSearch()
        Me.SearchButton.BorderStyle = BorderStyle.None
    End Sub

    Private Sub runSearch()
        REM clear last search
        Dim i As Integer
        For i = 0 To bc_am_workflow.docs.document.Count - 1
            bc_am_workflow.docs.document(i).search_flag = False
        Next
        owf.retrieve_docs(False)

        If Me.tsearch.Text <> "" Then
            search(Me.tsearch.Text)
        End If
    End Sub

    Private Sub MenuItem12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DisplayMenu_SearchClear.Click
        Me.tsearch.Clear()
        Dim i As Integer
        For i = 0 To bc_am_workflow.docs.document.Count - 1
            bc_am_workflow.docs.document(i).search_flag = False
        Next
        Dim rstate As Boolean
        rstate = bc_am_workflow.auto_refresh
        bc_am_workflow.auto_refresh = True
        just_refreshed = False
        bc_am_workflow.matches_only = False
        If just_refreshed = False Then
            owf.retrieve_docs(False)
        End If
        bc_am_workflow.auto_refresh = rstate
    End Sub

    Private Sub MenuItem11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DisplayMenu_FiltersReset.Click
        bc_am_workflow.spubtype_id.Clear()
        bc_am_workflow.sentity_id.Clear()
        bc_am_workflow.sstage.Clear()
        bc_am_workflow.sauthor_id.Clear()
        bc_am_workflow.sbusarea_id.Clear()
        Try
            Dim t As New TimeSpan(1, 0, 0, 0, 0)
            Me.FilterDateTo.Value = Now.Add(t)
            Me.FilterDateTo.Visible = False
            bc_am_workflow.fdateto = "9-9-9999"
            Me.pdatefrom.Image = Me.CheckBoxImages.Images(3)
            Me.pdateto.Image = Me.CheckBoxImages.Images(2)
            t = New TimeSpan(28, 0, 0, 0, 0)
            bc_am_workflow.fdatefrom = Now.Subtract(t)
            Me.FilterDateFrom.Value = Now.Subtract(t)
            Me.FilterDateFrom.Visible = True
        Catch

        End Try

        Dim owf As New bc_am_workflow
        owf.retrieve_docs(False, False)
    End Sub

    Private Sub MenuItem3_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DisplayMenu_MatchesOnly.Click
        Dim owf As New bc_am_workflow
        bc_am_workflow.matches_only = True
        owf.retrieve_docs(False, False)


    End Sub
    Private Sub pdatefrom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pdatefrom.Click
        If bc_am_workflow.fdatefrom = "9-9-9999" Then
            bc_am_workflow.fdatefrom = Me.FilterDateFrom.Value
            Me.pdatefrom.Image = Me.CheckBoxImages.Images(CHECK_ON_2)
            Me.FilterDateFrom.Visible = True
        Else
            Me.pdatefrom.Image = Me.CheckBoxImages.Images(CHECK_OFF_2)
            Me.FilterDateFrom.Visible = False
            bc_am_workflow.fdatefrom = "9-9-9999"
        End If
        Dim owf As New bc_am_workflow
        owf.retrieve_docs(False)

    End Sub
    Private Sub pdateto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pdateto.Click
        If Me.FilterDateTo.Visible = False Then
            bc_am_workflow.fdateto = Me.FilterDateTo.Value
            Me.pdateto.Image = Me.CheckBoxImages.Images(CHECK_ON_2)
            Me.FilterDateTo.Visible = True
        Else
            bc_am_workflow.fdateto = "9-9-9999"
            Me.pdateto.Image = Me.CheckBoxImages.Images(CHECK_OFF_2)
            Me.FilterDateTo.Visible = False
        End If

        Dim owf As New bc_am_workflow
        owf.retrieve_docs(False)

    End Sub
    Private Sub FilterDateFrom_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FilterDateFrom.ValueChanged
        If isInitialised And Not filterDateFromDropDownEnabled Then
            bc_am_workflow.fdatefrom = Me.FilterDateFrom.Value
            Dim owf As New bc_am_workflow
            If Me.loading = False Then
                owf.retrieve_docs(False)
            End If
        End If
    End Sub
    Private Sub FilterDateTo_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FilterDateTo.ValueChanged
        If isInitialised And Not filterDateToDropDownEnabled Then
            bc_am_workflow.fdateto = Me.FilterDateTo.Value
            Dim owf As New bc_am_workflow
            If Me.loading = False Then
                owf.retrieve_docs(False)
            End If
        End If
    End Sub
    Private Sub lstage_mouseup(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lstage.MouseUp
        Try
            Dim i As Integer
            Dim id As String
            REM if first item then all
          
            If Me.lstage.SelectedItems(0).Index = 0 Then
                bc_am_workflow.sstage.Clear()
            Else
                If Me.lstage.SelectedItems(0).ImageIndex = 0 Then
                    bc_am_workflow.sstage.Add(bc_am_workflow.otaxonomies(2).taxonomy_items(Me.lstage.SelectedItems(0).Index - 1).id)
                Else
                    id = bc_am_workflow.otaxonomies(2).taxonomy_items(Me.lstage.SelectedItems(0).Index - 1).id
                    For i = 0 To bc_am_workflow.sstage.Count - 1
                        If bc_am_workflow.sstage(i) = id Then
                            bc_am_workflow.sstage.RemoveAt(i)
                            Exit For
                        End If
                    Next
                End If
            End If
            Dim owf As New bc_am_workflow
            owf.retrieve_docs(False, False, True, True)
        Catch ex As Exception

        End Try
    End Sub
    Private Sub lauthor_mouseup(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lauthor.MouseUp
        Try
            REM if first item then all
            Dim i As Integer
            Dim id As String
            If Me.lauthor.SelectedItems(0).Index = 0 Then
                bc_am_workflow.sauthor_id.Clear()
            Else
                If Me.lauthor.SelectedItems(0).ImageIndex = 0 Then
                    bc_am_workflow.sauthor_id.Add(bc_am_workflow.otaxonomies(3).taxonomy_items(Me.lauthor.SelectedItems(0).Index - 1).id)
                Else

                    id = bc_am_workflow.otaxonomies(3).taxonomy_items(Me.lauthor.SelectedItems(0).Index - 1).id
                    For i = 0 To bc_am_workflow.sauthor_id.Count - 1
                        If CStr(bc_am_workflow.sauthor_id(i)) = CStr(id) Then
                            bc_am_workflow.sauthor_id.RemoveAt(i)
                            Exit For
                        End If
                    Next
                End If
            End If
            Dim owf As New bc_am_workflow
            owf.retrieve_docs(False, False, True, True)
        Catch

        End Try
    End Sub

    Private Sub lstatus_MouseUp1(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lstatus.MouseUp
        Try
            Dim owf As New bc_am_workflow
            bc_am_workflow.mode = Me.lstatus.SelectedItems(0).Index
            owf.retrieve_docs(False, False, True, True)
        Catch

        End Try
    End Sub

    Private Sub MenuItem13_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InsightMenuLaunchExcel.Click
        REM launch excel
        Try
            Dim texcel As Object
            texcel = CreateObject("excel.application")
            texcel.application.visible = True
            texcel.Application.Workbooks.Add()
        Catch
            Dim omessage As New bc_cs_message("Blue Curve", "Failed to start excel please start from desktop", bc_cs_message.MESSAGE)

        End Try


    End Sub

    Private Sub pdatefrom_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs)
        pdatefrom_Click(sender, e)
    End Sub

    Private Sub tsearch_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = Keys.Enter Then runSearch()
    End Sub

    Private Sub HelpMenu_About_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HelpMenu_About.Click
        'Dim msgbox As New bc_cs_message("Blue Curve Process", "Blue Curve Process" + vbCrLf + "Version: " + bc_cs_central_settings.version + vbCrLf + "Connection Mode: " + CStr(bc_cs_central_settings.selected_conn_method), bc_cs_message.MESSAGE)
        Dim fabout As New bc_am_workflow_about
        fabout.ShowDialog()
    End Sub

    'If user is using drop down for selecting date then we don't want
    'the form running a filter every time the user changes the month so wait
    'until drop down closes before changing the value
    Private filterDateFromDropDownEnabled As Boolean
    Private filterDateToDropDownEnabled As Boolean
    Private Sub FilterDateFrom_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles FilterDateFrom.DropDown
        filterDateFromDropDownEnabled = True
    End Sub

    Private Sub FilterDateFrom_CloseUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles FilterDateFrom.CloseUp
        filterDateFromDropDownEnabled = False
        FilterDateFrom_ValueChanged(sender, e)
    End Sub

    Private Sub FilterDateTo_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles FilterDateTo.DropDown
        filterDateToDropDownEnabled = True
    End Sub

    Private Sub FilterDateTo_CloseUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles FilterDateTo.CloseUp
        filterDateToDropDownEnabled = False
        FilterDateTo_ValueChanged(sender, e)
    End Sub

    Private Sub DisplayMenu_Filters_Hide_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles DisplayMenu_Filters_Hide.Click
        If FilterPanel.Visible Then
            FilterPanel.Visible = False
            DisplayMenu_Filters_Hide.Text = "Show"
        Else
            FilterPanel.Visible = True
            DisplayMenu_Filters_Hide.Text = "Hide"
        End If
    End Sub

    Private Sub tsearch_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub MenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem1.Click
        Dim rstate As Boolean
        bc_am_workflow.selected_doc_id = 0
        rstate = bc_am_workflow.auto_refresh
        bc_am_workflow.auto_refresh = True
        Dim owf As New bc_am_workflow
        bc_am_workflow.cfrm.madmin.Visible = False
        owf.retrieve_docs(False)
        bc_am_workflow.auto_refresh = rstate
        bc_am_workflow.cfrm.ofrmsummary.no_selection()
    End Sub

    Private Sub Chkpublish_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chkpublish.CheckedChanged
        If Me.Chkpublish.Checked = True Then
            bc_am_workflow.docs.show_publish = True
            owf.retrieve_docs(False)
        Else
            bc_am_workflow.docs.show_publish = False
            owf.retrieve_docs(False)
        End If
    End Sub


    Private Sub bc_am_workflow_container_frm_MaximumSizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.MaximumSizeChanged

    End Sub

    Private Sub mregdoc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mregdoc.Click
        Dim owf As New bc_am_workflow
        owf.import_master_document(bc_am_workflow.cfrm.ofrmsummary.ldoc, True)

    End Sub

    Private Sub mimpreg_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mimpreg.Click
        Dim owf As New bc_am_workflow
        owf.import_registered_document(bc_am_workflow.cfrm.ofrmsummary.ldoc)
    End Sub
    Private Sub MenuItem3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Mregrep.Click
        Dim owf As New bc_am_workflow
        owf.import_master_document(bc_am_workflow.cfrm.ofrmsummary.ldoc, False, True)
    End Sub

    Private Sub mreattach_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mreattach.Click
        Dim owf As New bc_am_workflow
        owf.attach_new_document(bc_am_workflow.cfrm.ofrmsummary.ldoc)
    End Sub

    Private Sub lstatus_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstatus.SelectedIndexChanged

    End Sub

    Private Sub mforce_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mforce.Click
        Dim owf As New bc_am_workflow
        owf.force_check_in(bc_am_workflow.cfrm.ofrmsummary.ldoc)
    End Sub

    Private Sub tsearch_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tsearch.KeyPress
        If Asc(e.KeyChar) = 13 Then
            Me.SearchButton.BorderStyle = BorderStyle.Fixed3D
            runSearch()
            Me.SearchButton.BorderStyle = BorderStyle.None
        End If
    End Sub

    Public Sub size_filters()
        Dim twidth As Double
        Dim iwidth As Double
        twidth = Me.Width - 20 - ldate.Width
        iwidth = twidth / 5
        Me.lstatus.Width = iwidth
        Me.lpubtypes.Width = iwidth
        Me.lentities.Width = iwidth
        Me.lstage.Width = iwidth
        Me.lauthor.Width = iwidth
        'Me.ldate.Width = iwidth
        Dim dp As System.Drawing.Point
        dp.Y = Me.lstatus.Location.Y
        dp.X = iwidth
        Me.lpubtypes.Location = dp
        dp.X = (iwidth + 2) * 2
        Me.lentities.Location = dp
        dp.X = (iwidth + 2) * 3
        Me.lstage.Location = dp
        dp.X = (iwidth + 2) * 4
        Me.lauthor.Location = dp
        dp.X = (iwidth + 2) * 5
        'Me.ldate.Location = dp
        'dp.Y = Me.Chkpublish.Location.Y
        'dp.X = (iwidth + 2) * 3
        'Me.Chkpublish.Location = dp
        'Me.FilterDateFrom.Width = iwidth * 0.68
        'Me.FilterDateTo.Width = iwidth * 0.68

    End Sub

    Private Sub lstage_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstage.SelectedIndexChanged

    End Sub

    Private Sub lentities_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lentities.SelectedIndexChanged

    End Sub

End Class

'added to handle Office 2010 startup changes
<ComImport(), Guid("00000016-0000-0000-C000-000000000046"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)> _
Public Interface IOleMessageFilter
    <PreserveSig()> Function HandleInComingCall(ByVal dwCallType As Integer, ByVal hTaskCaller As IntPtr, ByVal dwTickCount As Integer, ByVal lpInterfaceInfo As IntPtr) As Integer
    <PreserveSig()> Function RetryRejectedCall(ByVal hTaskCallee As IntPtr, ByVal dwTickCount As Integer, ByVal dwRejectType As Integer) As Integer
    <PreserveSig()> Function MessagePending(ByVal hTaskCallee As IntPtr, ByVal dwTickCount As Integer, ByVal dwPendingType As Integer) As Integer
End Interface

Public Class MessageFilter
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
