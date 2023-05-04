<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bc_am_pr_main
    Inherits DevExpress.XtraBars.Ribbon.RibbonForm

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim SplashScreenManager As DevExpress.XtraSplashScreen.SplashScreenManager = New DevExpress.XtraSplashScreen.SplashScreenManager(Me, GetType(Global.BlueCurve.Process.AM.bc_am_pr_splash_screen), True, True)
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_pr_main))
        Me.uxMainRibbon = New DevExpress.XtraBars.Ribbon.RibbonControl()
        Me.uxFileMenuPanel = New DevExpress.XtraBars.Ribbon.BackstageViewControl()
        Me.uxsplash = New DevExpress.Utils.ImageCollection()
        Me.uxFileMenuExit = New DevExpress.XtraBars.Ribbon.BackstageViewButtonItem()
        Me.uxRibbonImagesSmall = New DevExpress.Utils.ImageCollection()
        Me.uxCreate = New DevExpress.XtraBars.BarButtonItem()
        Me.uxReattachMasterDocument = New DevExpress.XtraBars.BarButtonItem()
        Me.uxRegisterMasterDocument = New DevExpress.XtraBars.BarButtonItem()
        Me.uxDocumentState = New DevExpress.XtraBars.BarCheckItem()
        Me.uxImportMasterDocument = New DevExpress.XtraBars.BarButtonItem()
        Me.uxImportSupportDocument = New DevExpress.XtraBars.BarButtonItem()
        Me.uxImportRegisteredDocument = New DevExpress.XtraBars.BarButtonItem()
        Me.uxRefresh = New DevExpress.XtraBars.BarButtonItem()
        Me.uxShowFilters = New DevExpress.XtraBars.BarCheckItem()
        Me.uxSettings = New DevExpress.XtraBars.BarButtonItem()
        Me.uxFilterDateFrom = New DevExpress.XtraBars.BarEditItem()
        Me.uxFilterDateFromControl = New DevExpress.XtraEditors.Repository.RepositoryItemDateEdit()
        Me.uxFilterDateTo = New DevExpress.XtraBars.BarEditItem()
        Me.uxFilterDateToControl = New DevExpress.XtraEditors.Repository.RepositoryItemDateEdit()
        Me.uxUser = New DevExpress.XtraBars.BarStaticItem()
        Me.uxConnection = New DevExpress.XtraBars.BarStaticItem()
        Me.uxRole = New DevExpress.XtraBars.BarStaticItem()
        Me.uxFilterDateToCheck = New DevExpress.XtraBars.BarEditItem()
        Me.uxFilterDateToCheckControl = New DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit()
        Me.uxFilterDateFromCheck = New DevExpress.XtraBars.BarEditItem()
        Me.uxFilterDateFromCheckControl = New DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit()
        Me.uxForceCheckIn = New DevExpress.XtraBars.BarButtonItem()
        Me.uxPollingRefresh = New DevExpress.XtraBars.BarStaticItem()
        Me.uxIncludePublished = New DevExpress.XtraBars.BarEditItem()
        Me.uxIncludePublishedControl = New DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit()
        Me.uxShowDetails = New DevExpress.XtraBars.BarCheckItem()
        Me.uxCategoriseDocument = New DevExpress.XtraBars.BarButtonItem()
        Me.uxViewDocument = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonGroup1 = New DevExpress.XtraBars.BarButtonGroup()
        Me.uxresetfilters = New DevExpress.XtraBars.BarButtonItem()
        Me.uxcomponents = New DevExpress.XtraBars.BarButtonItem()
        Me.uxhtmlpreview = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem1 = New DevExpress.XtraBars.BarButtonItem()
        Me.uxdistribution = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem2 = New DevExpress.XtraBars.BarButtonItem()
        Me.BarCheckItem1 = New DevExpress.XtraBars.BarCheckItem()
        Me.uxpublishonly = New DevExpress.XtraBars.BarEditItem()
        Me.RepositoryItemCheckEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit()
        Me.BarCheckItem2 = New DevExpress.XtraBars.BarCheckItem()
        Me.BarEditItem4 = New DevExpress.XtraBars.BarEditItem()
        Me.RepositoryItemCheckEdit2 = New DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit()
        Me.BarEditItem5 = New DevExpress.XtraBars.BarEditItem()
        Me.RepositoryItemComboBox1 = New DevExpress.XtraEditors.Repository.RepositoryItemComboBox()
        Me.BarEditItem8 = New DevExpress.XtraBars.BarEditItem()
        Me.RepositoryItemCheckEdit5 = New DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit()
        Me.uxregular = New DevExpress.XtraBars.BarButtonItem()
        Me.uxattestation = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonGroup2 = New DevExpress.XtraBars.BarButtonGroup()
        Me.uxstop = New DevExpress.XtraBars.BarButtonItem()
        Me.uxrejectdoc = New DevExpress.XtraBars.BarButtonItem()
        Me.uxviewattestation = New DevExpress.XtraBars.BarButtonItem()
        Me.uxattributes = New DevExpress.XtraBars.BarButtonItem()
        Me.BarButtonItem3 = New DevExpress.XtraBars.BarButtonItem()
        Me.uxdisclosurefile = New DevExpress.XtraBars.BarButtonItem()
        Me.uxRibbonImagesLarge = New DevExpress.Utils.ImageCollection()
        Me.uxDocumentRibbonPage = New DevExpress.XtraBars.Ribbon.RibbonPage()
        Me.uxCreateRibbonPageGroup = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
        Me.uxViewRibbonPageGroup = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
        Me.uxCategoriseRibbonPageGroup = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
        Me.uxImportRibbonPageGroup = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
        Me.uxgenerate = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
        Me.rdistribute = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
        Me.uxattestationgroup = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
        Me.uxstopdocgroup = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
        Me.uxViewRibbonPage = New DevExpress.XtraBars.Ribbon.RibbonPage()
        Me.uxRefreshRibbonPageGroup = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
        Me.uxDetailsRibbonPageGroup = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
        Me.uxSettingsRibbonPageGroup = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
        Me.uxFiltersRibbonGroupPage = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
        Me.RepositoryItemDateEdit3 = New DevExpress.XtraEditors.Repository.RepositoryItemDateEdit()
        Me.RepositoryItemDateEdit4 = New DevExpress.XtraEditors.Repository.RepositoryItemDateEdit()
        Me.RepositoryItemSearchLookUpEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit()
        Me.RepositoryItemSearchLookUpEdit1View = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.RepositoryItemCheckEdit3 = New DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit()
        Me.RepositoryItemCheckEdit4 = New DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit()
        Me.uxFilterSearchText = New DevExpress.XtraEditors.Repository.RepositoryItemTextEdit()
        Me.RibbonStatusBar = New DevExpress.XtraBars.Ribbon.RibbonStatusBar()
        Me.uxFilterWindow = New DevExpress.XtraNavBar.NavBarControl()
        Me.uxDocumentStatusFilter = New DevExpress.XtraNavBar.NavBarGroup()
        Me.uxPubTypeFilter = New DevExpress.XtraNavBar.NavBarGroup()
        Me.uxEntityFilter = New DevExpress.XtraNavBar.NavBarGroup()
        Me.uxStageFilter = New DevExpress.XtraNavBar.NavBarGroup()
        Me.uxAuthorFilter = New DevExpress.XtraNavBar.NavBarGroup()
        Me.Uxlocation = New DevExpress.XtraNavBar.NavBarGroup()
        Me.uxdiststatus = New DevExpress.XtraNavBar.NavBarGroup()
        Me.uxSplitContainer = New DevExpress.XtraEditors.SplitContainerControl()
        Me.uxDocumentListPanel = New DevExpress.XtraEditors.PanelControl()
        Me.uxDocumentDetailsPanel = New DevExpress.XtraEditors.PanelControl()
        Me.BackstageViewButtonItem1 = New DevExpress.XtraBars.Ribbon.BackstageViewButtonItem()
        Me.uxStatus = New DevExpress.XtraBars.BarStaticItem()
        Me.BarEditItem1 = New DevExpress.XtraBars.BarEditItem()
        Me.BarEditItem2 = New DevExpress.XtraBars.BarEditItem()
        Me.uxalert = New DevExpress.XtraBars.Alerter.AlertControl()
        Me.BarEditItem3 = New DevExpress.XtraBars.BarEditItem()
        Me.BarEditItem6 = New DevExpress.XtraBars.BarEditItem()
        Me.BarEditItem7 = New DevExpress.XtraBars.BarEditItem()
        Me.BarButtonItem4 = New DevExpress.XtraBars.BarButtonItem()
        Me.RibbonPageGroup1 = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
        CType(Me.uxMainRibbon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxsplash, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxRibbonImagesSmall, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxFilterDateFromControl, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxFilterDateFromControl.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxFilterDateToControl, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxFilterDateToControl.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxFilterDateToCheckControl, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxFilterDateFromCheckControl, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxIncludePublishedControl, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemCheckEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemCheckEdit2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemComboBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemCheckEdit5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxRibbonImagesLarge, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemDateEdit3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemDateEdit3.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemDateEdit4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemDateEdit4.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemSearchLookUpEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemSearchLookUpEdit1View, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemCheckEdit3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemCheckEdit4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxFilterSearchText, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxFilterWindow, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxSplitContainer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.uxSplitContainer.SuspendLayout()
        CType(Me.uxDocumentListPanel, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.uxDocumentListPanel.SuspendLayout()
        CType(Me.uxDocumentDetailsPanel, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'uxMainRibbon
        '
        Me.uxMainRibbon.ApplicationButtonDropDownControl = Me.uxFileMenuPanel
        Me.uxMainRibbon.ApplicationButtonText = "File"
        Me.uxMainRibbon.ExpandCollapseItem.Id = 0
        Me.uxMainRibbon.Images = Me.uxRibbonImagesSmall
        Me.uxMainRibbon.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.uxMainRibbon.ExpandCollapseItem, Me.uxCreate, Me.uxReattachMasterDocument, Me.uxRegisterMasterDocument, Me.uxDocumentState, Me.uxImportMasterDocument, Me.uxImportSupportDocument, Me.uxImportRegisteredDocument, Me.uxRefresh, Me.uxShowFilters, Me.uxSettings, Me.uxFilterDateFrom, Me.uxFilterDateTo, Me.uxUser, Me.uxConnection, Me.uxRole, Me.uxFilterDateToCheck, Me.uxFilterDateFromCheck, Me.uxForceCheckIn, Me.uxPollingRefresh, Me.uxIncludePublished, Me.uxShowDetails, Me.uxCategoriseDocument, Me.uxViewDocument, Me.BarButtonGroup1, Me.uxresetfilters, Me.uxcomponents, Me.uxhtmlpreview, Me.BarButtonItem1, Me.uxdistribution, Me.BarButtonItem2, Me.BarCheckItem1, Me.uxpublishonly, Me.BarCheckItem2, Me.BarEditItem4, Me.BarEditItem5, Me.BarEditItem8, Me.uxregular, Me.uxattestation, Me.BarButtonGroup2, Me.uxstop, Me.uxrejectdoc, Me.uxviewattestation, Me.uxattributes, Me.BarButtonItem3, Me.uxdisclosurefile})
        Me.uxMainRibbon.LargeImages = Me.uxRibbonImagesLarge
        Me.uxMainRibbon.Location = New System.Drawing.Point(0, 0)
        Me.uxMainRibbon.MaxItemId = 83
        Me.uxMainRibbon.Name = "uxMainRibbon"
        Me.uxMainRibbon.Pages.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPage() {Me.uxDocumentRibbonPage, Me.uxViewRibbonPage})
        Me.uxMainRibbon.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.uxFilterDateFromControl, Me.uxFilterDateToControl, Me.RepositoryItemDateEdit3, Me.RepositoryItemDateEdit4, Me.uxFilterDateFromCheckControl, Me.uxFilterDateToCheckControl, Me.RepositoryItemSearchLookUpEdit1, Me.RepositoryItemCheckEdit3, Me.RepositoryItemCheckEdit4, Me.uxFilterSearchText, Me.uxIncludePublishedControl, Me.RepositoryItemCheckEdit1, Me.RepositoryItemCheckEdit2, Me.RepositoryItemComboBox1, Me.RepositoryItemCheckEdit5})
        Me.uxMainRibbon.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonControlStyle.Office2010
        Me.uxMainRibbon.ShowToolbarCustomizeItem = False
        Me.uxMainRibbon.Size = New System.Drawing.Size(1393, 144)
        Me.uxMainRibbon.StatusBar = Me.RibbonStatusBar
        Me.uxMainRibbon.Toolbar.ShowCustomizeItem = False
        Me.uxMainRibbon.TransparentEditors = True
        '
        'uxFileMenuPanel
        '
        Me.uxFileMenuPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.uxFileMenuPanel.ColorScheme = DevExpress.XtraBars.Ribbon.RibbonControlColorScheme.Yellow
        Me.uxFileMenuPanel.Image = CType(resources.GetObject("uxFileMenuPanel.Image"), System.Drawing.Image)
        Me.uxFileMenuPanel.Images = Me.uxsplash
        Me.uxFileMenuPanel.Items.Add(Me.uxFileMenuExit)
        Me.uxFileMenuPanel.Location = New System.Drawing.Point(61, 19)
        Me.uxFileMenuPanel.Name = "uxFileMenuPanel"
        Me.uxFileMenuPanel.Ribbon = Me.uxMainRibbon
        Me.uxFileMenuPanel.SelectedTab = Nothing
        Me.uxFileMenuPanel.Size = New System.Drawing.Size(692, 415)
        Me.uxFileMenuPanel.TabIndex = 6
        Me.uxFileMenuPanel.Text = "BackstageViewControl1"
        '
        'uxsplash
        '
        Me.uxsplash.ImageSize = New System.Drawing.Size(32, 32)
        Me.uxsplash.ImageStream = CType(resources.GetObject("uxsplash.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
        Me.uxsplash.Images.SetKeyName(0, "jovus2.0.png")
        '
        'uxFileMenuExit
        '
        Me.uxFileMenuExit.Caption = "Exit"
        Me.uxFileMenuExit.CloseBackstageViewOnClick = False
        Me.uxFileMenuExit.Name = "uxFileMenuExit"
        Me.uxFileMenuExit.Tag = "EXIT"
        '
        'uxRibbonImagesSmall
        '
        Me.uxRibbonImagesSmall.ImageStream = CType(resources.GetObject("uxRibbonImagesSmall.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
        Me.uxRibbonImagesSmall.Images.SetKeyName(0, "Generic_Document.png")
        Me.uxRibbonImagesSmall.Images.SetKeyName(1, "attach_32.ico")
        Me.uxRibbonImagesSmall.Images.SetKeyName(2, "categorise_32.ico")
        Me.uxRibbonImagesSmall.Images.SetKeyName(3, "checkedin_IN_32.ico")
        Me.uxRibbonImagesSmall.Images.SetKeyName(4, "checkedin_OUT_32.ico")
        Me.uxRibbonImagesSmall.Images.SetKeyName(5, "checkin_in_force_32.ico")
        Me.uxRibbonImagesSmall.Images.SetKeyName(6, "checkin_in_to_another_32.ico")
        Me.uxRibbonImagesSmall.Images.SetKeyName(7, "checkin_in_to_me_32.ico")
        Me.uxRibbonImagesSmall.Images.SetKeyName(8, "comment_32.ico")
        Me.uxRibbonImagesSmall.Images.SetKeyName(9, "document_32.ico")
        Me.uxRibbonImagesSmall.Images.SetKeyName(10, "edit_32.ico")
        Me.uxRibbonImagesSmall.Images.SetKeyName(11, "filter_32.ico")
        Me.uxRibbonImagesSmall.Images.SetKeyName(12, "history_32.ico")
        Me.uxRibbonImagesSmall.Images.SetKeyName(13, "icon1_32.ico")
        Me.uxRibbonImagesSmall.Images.SetKeyName(14, "icon2_32.ico")
        Me.uxRibbonImagesSmall.Images.SetKeyName(15, "icon3_32.ico")
        Me.uxRibbonImagesSmall.Images.SetKeyName(16, "icon4_32.ico")
        Me.uxRibbonImagesSmall.Images.SetKeyName(17, "icon5_32.ico")
        Me.uxRibbonImagesSmall.Images.SetKeyName(18, "icon6_32.ico")
        Me.uxRibbonImagesSmall.Images.SetKeyName(19, "import_32.ico")
        Me.uxRibbonImagesSmall.Images.SetKeyName(20, "links_32.ico")
        Me.uxRibbonImagesSmall.Images.SetKeyName(21, "locked_32.ico")
        Me.uxRibbonImagesSmall.Images.SetKeyName(22, "master_document_32.ico")
        Me.uxRibbonImagesSmall.Images.SetKeyName(23, "new_32.ico")
        Me.uxRibbonImagesSmall.Images.SetKeyName(24, "permissions_32.ico")
        Me.uxRibbonImagesSmall.Images.SetKeyName(25, "read_32.ico")
        Me.uxRibbonImagesSmall.Images.SetKeyName(26, "readonly_32.ico")
        Me.uxRibbonImagesSmall.Images.SetKeyName(27, "refresh_32.ico")
        Me.uxRibbonImagesSmall.Images.SetKeyName(28, "register_32.ico")
        Me.uxRibbonImagesSmall.Images.SetKeyName(29, "registered_document_32.ico")
        Me.uxRibbonImagesSmall.Images.SetKeyName(30, "settings_32.ico")
        Me.uxRibbonImagesSmall.Images.SetKeyName(31, "support_document_32.ico")
        Me.uxRibbonImagesSmall.Images.SetKeyName(32, "user_32.ico")
        Me.uxRibbonImagesSmall.Images.SetKeyName(33, "write_32.ico")
        Me.uxRibbonImagesSmall.Images.SetKeyName(34, "ie.bmp")
        '
        'uxCreate
        '
        Me.uxCreate.Caption = "Create"
        Me.uxCreate.Id = 1
        Me.uxCreate.LargeImageIndex = 0
        Me.uxCreate.Name = "uxCreate"
        Me.uxCreate.Tag = "CREATE"
        '
        'uxReattachMasterDocument
        '
        Me.uxReattachMasterDocument.Caption = "Re-attach Master Document"
        Me.uxReattachMasterDocument.Id = 2
        Me.uxReattachMasterDocument.LargeImageIndex = 8
        Me.uxReattachMasterDocument.Name = "uxReattachMasterDocument"
        Me.uxReattachMasterDocument.Tag = "REATTACH_MASTER_DOCUMENT"
        '
        'uxRegisterMasterDocument
        '
        Me.uxRegisterMasterDocument.Caption = "Register Master Document"
        Me.uxRegisterMasterDocument.Id = 3
        Me.uxRegisterMasterDocument.LargeImageIndex = 5
        Me.uxRegisterMasterDocument.Name = "uxRegisterMasterDocument"
        Me.uxRegisterMasterDocument.Tag = "REGISTER_MASTER_DOCUMENT"
        '
        'uxDocumentState
        '
        Me.uxDocumentState.Caption = "Check Out"
        Me.uxDocumentState.Id = 4
        Me.uxDocumentState.LargeImageIndex = 2
        Me.uxDocumentState.Name = "uxDocumentState"
        Me.uxDocumentState.Tag = "DOCUMENT_STATE_CHANGE"
        '
        'uxImportMasterDocument
        '
        Me.uxImportMasterDocument.Caption = "Master Document"
        Me.uxImportMasterDocument.Id = 5
        Me.uxImportMasterDocument.LargeImageIndex = 5
        Me.uxImportMasterDocument.Name = "uxImportMasterDocument"
        Me.uxImportMasterDocument.Tag = "IMPORT_MASTER_DOCUMENT"
        '
        'uxImportSupportDocument
        '
        Me.uxImportSupportDocument.Caption = "Support Document"
        Me.uxImportSupportDocument.Id = 6
        Me.uxImportSupportDocument.LargeImageIndex = 6
        Me.uxImportSupportDocument.Name = "uxImportSupportDocument"
        Me.uxImportSupportDocument.Tag = "IMPORT_SUPPORT_DOCUMENT"
        '
        'uxImportRegisteredDocument
        '
        Me.uxImportRegisteredDocument.Caption = "Registered Document"
        Me.uxImportRegisteredDocument.Id = 8
        Me.uxImportRegisteredDocument.LargeImageIndex = 7
        Me.uxImportRegisteredDocument.Name = "uxImportRegisteredDocument"
        Me.uxImportRegisteredDocument.Tag = "IMPORT_REGISTERED_DOCUMENT"
        '
        'uxRefresh
        '
        Me.uxRefresh.Caption = "Refresh"
        Me.uxRefresh.Hint = "Refresh Document List"
        Me.uxRefresh.Id = 9
        Me.uxRefresh.LargeImageIndex = 9
        Me.uxRefresh.Name = "uxRefresh"
        Me.uxRefresh.Tag = "REFRESH"
        '
        'uxShowFilters
        '
        Me.uxShowFilters.Caption = "Hide"
        Me.uxShowFilters.Id = 11
        Me.uxShowFilters.LargeImageIndex = 0
        Me.uxShowFilters.Name = "uxShowFilters"
        Me.uxShowFilters.Tag = "DOC_FILTERS"
        '
        'uxSettings
        '
        Me.uxSettings.Caption = "Settings"
        Me.uxSettings.Id = 12
        Me.uxSettings.LargeImageIndex = 13
        Me.uxSettings.Name = "uxSettings"
        Me.uxSettings.Tag = "SETTINGS"
        '
        'uxFilterDateFrom
        '
        Me.uxFilterDateFrom.Edit = Me.uxFilterDateFromControl
        Me.uxFilterDateFrom.Id = 15
        Me.uxFilterDateFrom.Name = "uxFilterDateFrom"
        Me.uxFilterDateFrom.Tag = "DATE_FROM_VALUE"
        Me.uxFilterDateFrom.Width = 200
        '
        'uxFilterDateFromControl
        '
        Me.uxFilterDateFromControl.AutoHeight = False
        Me.uxFilterDateFromControl.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.uxFilterDateFromControl.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
        Me.uxFilterDateFromControl.DisplayFormat.FormatString = "dd MMM yyyy"
        Me.uxFilterDateFromControl.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        Me.uxFilterDateFromControl.EditFormat.FormatString = "dd MMM yyyy"
        Me.uxFilterDateFromControl.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        Me.uxFilterDateFromControl.Mask.EditMask = "dd MMM yyyy"
        Me.uxFilterDateFromControl.Name = "uxFilterDateFromControl"
        Me.uxFilterDateFromControl.NullText = "No Date Range"
        '
        'uxFilterDateTo
        '
        Me.uxFilterDateTo.Edit = Me.uxFilterDateToControl
        Me.uxFilterDateTo.Id = 16
        Me.uxFilterDateTo.Name = "uxFilterDateTo"
        Me.uxFilterDateTo.Tag = "DATE_TO_VALUE"
        Me.uxFilterDateTo.Width = 200
        '
        'uxFilterDateToControl
        '
        Me.uxFilterDateToControl.AutoHeight = False
        Me.uxFilterDateToControl.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.uxFilterDateToControl.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
        Me.uxFilterDateToControl.DisplayFormat.FormatString = "dd MMM yyyy"
        Me.uxFilterDateToControl.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        Me.uxFilterDateToControl.EditFormat.FormatString = "dd MMM yyyy"
        Me.uxFilterDateToControl.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        Me.uxFilterDateToControl.Mask.EditMask = "dd MMM yyyy"
        Me.uxFilterDateToControl.Name = "uxFilterDateToControl"
        Me.uxFilterDateToControl.NullText = "No Date Range"
        '
        'uxUser
        '
        Me.uxUser.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right
        Me.uxUser.AutoSize = DevExpress.XtraBars.BarStaticItemSize.Spring
        Me.uxUser.Caption = "User:"
        Me.uxUser.Id = 20
        Me.uxUser.Name = "uxUser"
        Me.uxUser.TextAlignment = System.Drawing.StringAlignment.Near
        '
        'uxConnection
        '
        Me.uxConnection.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right
        Me.uxConnection.AutoSize = DevExpress.XtraBars.BarStaticItemSize.Spring
        Me.uxConnection.Caption = "Connection:"
        Me.uxConnection.Id = 21
        Me.uxConnection.Name = "uxConnection"
        Me.uxConnection.TextAlignment = System.Drawing.StringAlignment.Near
        '
        'uxRole
        '
        Me.uxRole.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right
        Me.uxRole.AutoSize = DevExpress.XtraBars.BarStaticItemSize.Spring
        Me.uxRole.Caption = "Role:"
        Me.uxRole.Id = 22
        Me.uxRole.Name = "uxRole"
        Me.uxRole.TextAlignment = System.Drawing.StringAlignment.Near
        '
        'uxFilterDateToCheck
        '
        Me.uxFilterDateToCheck.Caption = "Date to:"
        Me.uxFilterDateToCheck.CaptionAlignment = DevExpress.Utils.HorzAlignment.Far
        Me.uxFilterDateToCheck.Edit = Me.uxFilterDateToCheckControl
        Me.uxFilterDateToCheck.EditValue = False
        Me.uxFilterDateToCheck.Id = 29
        Me.uxFilterDateToCheck.Name = "uxFilterDateToCheck"
        Me.uxFilterDateToCheck.Tag = "DATE_TO_CHECK"
        Me.uxFilterDateToCheck.Width = 18
        '
        'uxFilterDateToCheckControl
        '
        Me.uxFilterDateToCheckControl.AutoHeight = False
        Me.uxFilterDateToCheckControl.AutoWidth = True
        Me.uxFilterDateToCheckControl.Caption = ""
        Me.uxFilterDateToCheckControl.Name = "uxFilterDateToCheckControl"
        '
        'uxFilterDateFromCheck
        '
        Me.uxFilterDateFromCheck.Caption = "Date from:"
        Me.uxFilterDateFromCheck.CaptionAlignment = DevExpress.Utils.HorzAlignment.Far
        Me.uxFilterDateFromCheck.Edit = Me.uxFilterDateFromCheckControl
        Me.uxFilterDateFromCheck.EditValue = True
        Me.uxFilterDateFromCheck.Id = 30
        Me.uxFilterDateFromCheck.Name = "uxFilterDateFromCheck"
        Me.uxFilterDateFromCheck.Tag = "DATE_FROM_CHECK"
        Me.uxFilterDateFromCheck.Width = 18
        '
        'uxFilterDateFromCheckControl
        '
        Me.uxFilterDateFromCheckControl.AutoHeight = False
        Me.uxFilterDateFromCheckControl.AutoWidth = True
        Me.uxFilterDateFromCheckControl.Caption = ""
        Me.uxFilterDateFromCheckControl.Name = "uxFilterDateFromCheckControl"
        '
        'uxForceCheckIn
        '
        Me.uxForceCheckIn.Caption = "Force Check In"
        Me.uxForceCheckIn.Id = 32
        Me.uxForceCheckIn.LargeImageIndex = 3
        Me.uxForceCheckIn.Name = "uxForceCheckIn"
        Me.uxForceCheckIn.Tag = "FORCE_CHECK_IN"
        '
        'uxPollingRefresh
        '
        Me.uxPollingRefresh.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right
        Me.uxPollingRefresh.AutoSize = DevExpress.XtraBars.BarStaticItemSize.Spring
        Me.uxPollingRefresh.Caption = "Refresh:"
        Me.uxPollingRefresh.Id = 34
        Me.uxPollingRefresh.Name = "uxPollingRefresh"
        Me.uxPollingRefresh.TextAlignment = System.Drawing.StringAlignment.Near
        '
        'uxIncludePublished
        '
        Me.uxIncludePublished.Caption = "Include Published"
        Me.uxIncludePublished.Edit = Me.uxIncludePublishedControl
        Me.uxIncludePublished.EditValue = False
        Me.uxIncludePublished.Id = 37
        Me.uxIncludePublished.Name = "uxIncludePublished"
        Me.uxIncludePublished.Tag = "INCLUDE_PUBLISHED"
        Me.uxIncludePublished.Width = 18
        '
        'uxIncludePublishedControl
        '
        Me.uxIncludePublishedControl.AutoHeight = False
        Me.uxIncludePublishedControl.Caption = "Check"
        Me.uxIncludePublishedControl.EditValueChangedDelay = 1
        Me.uxIncludePublishedControl.Name = "uxIncludePublishedControl"
        '
        'uxShowDetails
        '
        Me.uxShowDetails.Caption = "Show"
        Me.uxShowDetails.Id = 38
        Me.uxShowDetails.LargeImageIndex = 0
        Me.uxShowDetails.Name = "uxShowDetails"
        Me.uxShowDetails.Tag = "DOC_DETAILS"
        '
        'uxCategoriseDocument
        '
        Me.uxCategoriseDocument.Caption = "Categorise Document"
        Me.uxCategoriseDocument.Id = 39
        Me.uxCategoriseDocument.LargeImageIndex = 4
        Me.uxCategoriseDocument.Name = "uxCategoriseDocument"
        Me.uxCategoriseDocument.Tag = "CATEGORISE"
        '
        'uxViewDocument
        '
        Me.uxViewDocument.Caption = "View Document"
        Me.uxViewDocument.Id = 40
        Me.uxViewDocument.LargeImageIndex = 1
        Me.uxViewDocument.Name = "uxViewDocument"
        Me.uxViewDocument.Tag = "VIEW"
        '
        'BarButtonGroup1
        '
        Me.BarButtonGroup1.Caption = "BarButtonGroup1"
        Me.BarButtonGroup1.Id = 45
        Me.BarButtonGroup1.Name = "BarButtonGroup1"
        '
        'uxresetfilters
        '
        Me.uxresetfilters.Caption = "Reset"
        Me.uxresetfilters.Id = 46
        Me.uxresetfilters.LargeImageIndex = 10
        Me.uxresetfilters.Name = "uxresetfilters"
        Me.uxresetfilters.Tag = "RESET_FILTERS"
        '
        'uxcomponents
        '
        Me.uxcomponents.Caption = "Check Components"
        Me.uxcomponents.Id = 51
        Me.uxcomponents.ImageIndex = 25
        Me.uxcomponents.LargeImageIndex = 4
        Me.uxcomponents.Name = "uxcomponents"
        Me.uxcomponents.Tag = "COMPONENTS"
        '
        'uxhtmlpreview
        '
        Me.uxhtmlpreview.Caption = "HTML Preview"
        Me.uxhtmlpreview.Id = 52
        Me.uxhtmlpreview.ImageIndex = 25
        Me.uxhtmlpreview.Name = "uxhtmlpreview"
        Me.uxhtmlpreview.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large
        Me.uxhtmlpreview.Tag = "HTML_PREVIEW"
        Me.uxhtmlpreview.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        '
        'BarButtonItem1
        '
        Me.BarButtonItem1.Caption = "BarButtonItem1"
        Me.BarButtonItem1.Id = 56
        Me.BarButtonItem1.Name = "BarButtonItem1"
        '
        'uxdistribution
        '
        Me.uxdistribution.Caption = "View Distribution"
        Me.uxdistribution.Id = 57
        Me.uxdistribution.LargeImageIndex = 12
        Me.uxdistribution.Name = "uxdistribution"
        Me.uxdistribution.Tag = "DISTRIBUTION"
        '
        'BarButtonItem2
        '
        Me.BarButtonItem2.Caption = "BarButtonItem2"
        Me.BarButtonItem2.Id = 58
        Me.BarButtonItem2.Name = "BarButtonItem2"
        '
        'BarCheckItem1
        '
        Me.BarCheckItem1.Caption = "BarCheckItem1"
        Me.BarCheckItem1.Id = 59
        Me.BarCheckItem1.Name = "BarCheckItem1"
        '
        'uxpublishonly
        '
        Me.uxpublishonly.Caption = "Show Published Only"
        Me.uxpublishonly.Edit = Me.RepositoryItemCheckEdit1
        Me.uxpublishonly.EditValue = False
        Me.uxpublishonly.Id = 60
        Me.uxpublishonly.Name = "uxpublishonly"
        Me.uxpublishonly.Tag = "PUBLISH_ONLY"
        Me.uxpublishonly.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        Me.uxpublishonly.Width = 18
        '
        'RepositoryItemCheckEdit1
        '
        Me.RepositoryItemCheckEdit1.AutoHeight = False
        Me.RepositoryItemCheckEdit1.Caption = "Check"
        Me.RepositoryItemCheckEdit1.EditValueChangedDelay = 1
        Me.RepositoryItemCheckEdit1.Name = "RepositoryItemCheckEdit1"
        '
        'BarCheckItem2
        '
        Me.BarCheckItem2.Caption = "BarCheckItem2"
        Me.BarCheckItem2.Id = 61
        Me.BarCheckItem2.Name = "BarCheckItem2"
        '
        'BarEditItem4
        '
        Me.BarEditItem4.Caption = "BarEditItem4"
        Me.BarEditItem4.Edit = Me.RepositoryItemCheckEdit2
        Me.BarEditItem4.Id = 62
        Me.BarEditItem4.Name = "BarEditItem4"
        '
        'RepositoryItemCheckEdit2
        '
        Me.RepositoryItemCheckEdit2.AutoHeight = False
        Me.RepositoryItemCheckEdit2.Caption = "Check"
        Me.RepositoryItemCheckEdit2.Name = "RepositoryItemCheckEdit2"
        '
        'BarEditItem5
        '
        Me.BarEditItem5.Edit = Me.RepositoryItemComboBox1
        Me.BarEditItem5.Id = 63
        Me.BarEditItem5.Name = "BarEditItem5"
        '
        'RepositoryItemComboBox1
        '
        Me.RepositoryItemComboBox1.AutoHeight = False
        Me.RepositoryItemComboBox1.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.RepositoryItemComboBox1.Name = "RepositoryItemComboBox1"
        '
        'BarEditItem8
        '
        Me.BarEditItem8.Caption = "BarEditItem8"
        Me.BarEditItem8.Edit = Me.RepositoryItemCheckEdit5
        Me.BarEditItem8.Id = 64
        Me.BarEditItem8.Name = "BarEditItem8"
        '
        'RepositoryItemCheckEdit5
        '
        Me.RepositoryItemCheckEdit5.AutoHeight = False
        Me.RepositoryItemCheckEdit5.Caption = "Check"
        Me.RepositoryItemCheckEdit5.Name = "RepositoryItemCheckEdit5"
        '
        'uxregular
        '
        Me.uxregular.Caption = "Regular Reports"
        Me.uxregular.Id = 65
        Me.uxregular.LargeImageIndex = 14
        Me.uxregular.Name = "uxregular"
        Me.uxregular.Tag = "REGULAR"
        Me.uxregular.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        '
        'uxattestation
        '
        Me.uxattestation.Caption = "Submit / Update"
        Me.uxattestation.Id = 69
        Me.uxattestation.LargeImageIndex = 3
        Me.uxattestation.Name = "uxattestation"
        Me.uxattestation.Tag = "ATTESTATION"
        '
        'BarButtonGroup2
        '
        Me.BarButtonGroup2.Caption = "BarButtonGroup2"
        Me.BarButtonGroup2.Id = 72
        Me.BarButtonGroup2.Name = "BarButtonGroup2"
        '
        'uxstop
        '
        Me.uxstop.Caption = "Cancel"
        Me.uxstop.Id = 73
        Me.uxstop.LargeImageIndex = 16
        Me.uxstop.Name = "uxstop"
        Me.uxstop.Tag = "CANCELDOC"
        '
        'uxrejectdoc
        '
        Me.uxrejectdoc.Caption = "Reject"
        Me.uxrejectdoc.Id = 74
        Me.uxrejectdoc.LargeImageIndex = 9
        Me.uxrejectdoc.Name = "uxrejectdoc"
        Me.uxrejectdoc.Tag = "REJECTDOC"
        '
        'uxviewattestation
        '
        Me.uxviewattestation.Caption = "View"
        Me.uxviewattestation.Id = 79
        Me.uxviewattestation.LargeImageIndex = 14
        Me.uxviewattestation.Name = "uxviewattestation"
        Me.uxviewattestation.Tag = "VIEWATTESTATION"
        '
        'uxattributes
        '
        Me.uxattributes.Caption = "Recommendations"
        Me.uxattributes.Id = 80
        Me.uxattributes.LargeImageIndex = 9
        Me.uxattributes.Name = "uxattributes"
        Me.uxattributes.Tag = "ATTRIBUTES"
        Me.uxattributes.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        '
        'BarButtonItem3
        '
        Me.BarButtonItem3.Caption = "View"
        Me.BarButtonItem3.Id = 81
        Me.BarButtonItem3.ImageIndex = 9
        Me.BarButtonItem3.Name = "BarButtonItem3"
        Me.BarButtonItem3.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large
        '
        'uxdisclosurefile
        '
        Me.uxdisclosurefile.Caption = "Disclosure File"
        Me.uxdisclosurefile.Enabled = False
        Me.uxdisclosurefile.Id = 82
        Me.uxdisclosurefile.LargeImageIndex = 8
        Me.uxdisclosurefile.Name = "uxdisclosurefile"
        '
        'uxRibbonImagesLarge
        '
        Me.uxRibbonImagesLarge.ImageSize = New System.Drawing.Size(32, 32)
        Me.uxRibbonImagesLarge.ImageStream = CType(resources.GetObject("uxRibbonImagesLarge.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
        Me.uxRibbonImagesLarge.Images.SetKeyName(0, "new_32.png")
        Me.uxRibbonImagesLarge.Images.SetKeyName(1, "document_32.png")
        Me.uxRibbonImagesLarge.Images.SetKeyName(2, "checkin_OUT_32.png")
        Me.uxRibbonImagesLarge.Images.SetKeyName(3, "checkin_IN_32.png")
        Me.uxRibbonImagesLarge.Images.SetKeyName(4, "category_32.png")
        Me.uxRibbonImagesLarge.Images.SetKeyName(5, "master_document_32.png")
        Me.uxRibbonImagesLarge.Images.SetKeyName(6, "support_32.png")
        Me.uxRibbonImagesLarge.Images.SetKeyName(7, "register_document_32.png")
        Me.uxRibbonImagesLarge.Images.SetKeyName(8, "attach_32.png")
        Me.uxRibbonImagesLarge.Images.SetKeyName(9, "refresh_32.png")
        Me.uxRibbonImagesLarge.Images.SetKeyName(10, "filter_32.png")
        Me.uxRibbonImagesLarge.Images.SetKeyName(11, "readonly_32.png")
        Me.uxRibbonImagesLarge.Images.SetKeyName(12, "import_32.png")
        Me.uxRibbonImagesLarge.Images.SetKeyName(13, "settings_32.png")
        Me.uxRibbonImagesLarge.Images.SetKeyName(14, "register_32.png")
        Me.uxRibbonImagesLarge.Images.SetKeyName(15, "html.bmp")
        Me.uxRibbonImagesLarge.Images.SetKeyName(16, "2000px-Red_x.svg.png")
        Me.uxRibbonImagesLarge.Images.SetKeyName(17, "green-tick.png")
        '
        'uxDocumentRibbonPage
        '
        Me.uxDocumentRibbonPage.Groups.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPageGroup() {Me.uxCreateRibbonPageGroup, Me.uxViewRibbonPageGroup, Me.uxCategoriseRibbonPageGroup, Me.uxImportRibbonPageGroup, Me.uxgenerate, Me.rdistribute, Me.uxattestationgroup, Me.uxstopdocgroup})
        Me.uxDocumentRibbonPage.Name = "uxDocumentRibbonPage"
        Me.uxDocumentRibbonPage.Text = "Document"
        '
        'uxCreateRibbonPageGroup
        '
        Me.uxCreateRibbonPageGroup.ItemLinks.Add(Me.uxCreate)
        Me.uxCreateRibbonPageGroup.ItemLinks.Add(Me.uxRegisterMasterDocument)
        Me.uxCreateRibbonPageGroup.Name = "uxCreateRibbonPageGroup"
        Me.uxCreateRibbonPageGroup.ShowCaptionButton = False
        Me.uxCreateRibbonPageGroup.Text = "Create"
        '
        'uxViewRibbonPageGroup
        '
        Me.uxViewRibbonPageGroup.ItemLinks.Add(Me.BarButtonItem3)
        Me.uxViewRibbonPageGroup.ItemLinks.Add(Me.uxhtmlpreview)
        Me.uxViewRibbonPageGroup.ItemLinks.Add(Me.uxDocumentState)
        Me.uxViewRibbonPageGroup.ItemLinks.Add(Me.uxForceCheckIn)
        Me.uxViewRibbonPageGroup.ItemLinks.Add(Me.uxcomponents)
        Me.uxViewRibbonPageGroup.ItemLinks.Add(Me.uxReattachMasterDocument)
        Me.uxViewRibbonPageGroup.Name = "uxViewRibbonPageGroup"
        Me.uxViewRibbonPageGroup.ShowCaptionButton = False
        Me.uxViewRibbonPageGroup.Text = "Document"
        '
        'uxCategoriseRibbonPageGroup
        '
        Me.uxCategoriseRibbonPageGroup.ItemLinks.Add(Me.uxCategoriseDocument)
        Me.uxCategoriseRibbonPageGroup.ItemLinks.Add(Me.uxattributes)
        Me.uxCategoriseRibbonPageGroup.Name = "uxCategoriseRibbonPageGroup"
        Me.uxCategoriseRibbonPageGroup.ShowCaptionButton = False
        Me.uxCategoriseRibbonPageGroup.Text = "Data"
        '
        'uxImportRibbonPageGroup
        '
        Me.uxImportRibbonPageGroup.ItemLinks.Add(Me.uxImportMasterDocument)
        Me.uxImportRibbonPageGroup.ItemLinks.Add(Me.uxImportSupportDocument)
        Me.uxImportRibbonPageGroup.ItemLinks.Add(Me.uxImportRegisteredDocument)
        Me.uxImportRibbonPageGroup.ItemLinks.Add(Me.uxregular)
        Me.uxImportRibbonPageGroup.Name = "uxImportRibbonPageGroup"
        Me.uxImportRibbonPageGroup.ShowCaptionButton = False
        Me.uxImportRibbonPageGroup.Text = "Import"
        '
        'uxgenerate
        '
        Me.uxgenerate.AllowTextClipping = False
        Me.uxgenerate.ItemLinks.Add(Me.uxdisclosurefile)
        Me.uxgenerate.Name = "uxgenerate"
        Me.uxgenerate.ShowCaptionButton = False
        Me.uxgenerate.Text = "Generate"
        Me.uxgenerate.Visible = False
        '
        'rdistribute
        '
        Me.rdistribute.ItemLinks.Add(Me.uxdistribution)
        Me.rdistribute.Name = "rdistribute"
        Me.rdistribute.ShowCaptionButton = False
        Me.rdistribute.Text = "Distribution"
        Me.rdistribute.Visible = False
        '
        'uxattestationgroup
        '
        Me.uxattestationgroup.AllowTextClipping = False
        Me.uxattestationgroup.ItemLinks.Add(Me.uxattestation)
        Me.uxattestationgroup.ItemLinks.Add(Me.uxviewattestation)
        Me.uxattestationgroup.Name = "uxattestationgroup"
        Me.uxattestationgroup.ShowCaptionButton = False
        Me.uxattestationgroup.Text = "Attestation"
        Me.uxattestationgroup.Visible = False
        '
        'uxstopdocgroup
        '
        Me.uxstopdocgroup.AllowTextClipping = False
        Me.uxstopdocgroup.ItemLinks.Add(Me.uxstop)
        Me.uxstopdocgroup.ItemLinks.Add(Me.uxrejectdoc)
        Me.uxstopdocgroup.Name = "uxstopdocgroup"
        Me.uxstopdocgroup.ShowCaptionButton = False
        Me.uxstopdocgroup.Text = "Stop Document"
        Me.uxstopdocgroup.Visible = False
        '
        'uxViewRibbonPage
        '
        Me.uxViewRibbonPage.Groups.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPageGroup() {Me.uxRefreshRibbonPageGroup, Me.uxDetailsRibbonPageGroup, Me.uxSettingsRibbonPageGroup, Me.uxFiltersRibbonGroupPage})
        Me.uxViewRibbonPage.Name = "uxViewRibbonPage"
        Me.uxViewRibbonPage.Text = "View"
        '
        'uxRefreshRibbonPageGroup
        '
        Me.uxRefreshRibbonPageGroup.ItemLinks.Add(Me.uxRefresh)
        Me.uxRefreshRibbonPageGroup.Name = "uxRefreshRibbonPageGroup"
        Me.uxRefreshRibbonPageGroup.ShowCaptionButton = False
        Me.uxRefreshRibbonPageGroup.Text = "Refresh"
        '
        'uxDetailsRibbonPageGroup
        '
        Me.uxDetailsRibbonPageGroup.AllowTextClipping = False
        Me.uxDetailsRibbonPageGroup.ItemLinks.Add(Me.uxShowDetails)
        Me.uxDetailsRibbonPageGroup.Name = "uxDetailsRibbonPageGroup"
        Me.uxDetailsRibbonPageGroup.ShowCaptionButton = False
        Me.uxDetailsRibbonPageGroup.Text = "Document Details"
        '
        'uxSettingsRibbonPageGroup
        '
        Me.uxSettingsRibbonPageGroup.ItemLinks.Add(Me.uxSettings)
        Me.uxSettingsRibbonPageGroup.Name = "uxSettingsRibbonPageGroup"
        Me.uxSettingsRibbonPageGroup.ShowCaptionButton = False
        Me.uxSettingsRibbonPageGroup.Text = "Settings"
        '
        'uxFiltersRibbonGroupPage
        '
        Me.uxFiltersRibbonGroupPage.ItemLinks.Add(Me.uxShowFilters)
        Me.uxFiltersRibbonGroupPage.ItemLinks.Add(Me.uxresetfilters)
        Me.uxFiltersRibbonGroupPage.ItemLinks.Add(Me.uxFilterDateFromCheck, True)
        Me.uxFiltersRibbonGroupPage.ItemLinks.Add(Me.uxFilterDateToCheck)
        Me.uxFiltersRibbonGroupPage.ItemLinks.Add(Me.uxFilterDateFrom, True)
        Me.uxFiltersRibbonGroupPage.ItemLinks.Add(Me.uxFilterDateTo)
        Me.uxFiltersRibbonGroupPage.ItemLinks.Add(Me.uxIncludePublished, True)
        Me.uxFiltersRibbonGroupPage.ItemLinks.Add(Me.uxpublishonly)
        Me.uxFiltersRibbonGroupPage.Name = "uxFiltersRibbonGroupPage"
        Me.uxFiltersRibbonGroupPage.ShowCaptionButton = False
        Me.uxFiltersRibbonGroupPage.Text = "Filters"
        '
        'RepositoryItemDateEdit3
        '
        Me.RepositoryItemDateEdit3.AutoHeight = False
        Me.RepositoryItemDateEdit3.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.RepositoryItemDateEdit3.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
        Me.RepositoryItemDateEdit3.Name = "RepositoryItemDateEdit3"
        '
        'RepositoryItemDateEdit4
        '
        Me.RepositoryItemDateEdit4.AutoHeight = False
        Me.RepositoryItemDateEdit4.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.RepositoryItemDateEdit4.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
        Me.RepositoryItemDateEdit4.Name = "RepositoryItemDateEdit4"
        '
        'RepositoryItemSearchLookUpEdit1
        '
        Me.RepositoryItemSearchLookUpEdit1.AutoHeight = False
        Me.RepositoryItemSearchLookUpEdit1.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.RepositoryItemSearchLookUpEdit1.Name = "RepositoryItemSearchLookUpEdit1"
        Me.RepositoryItemSearchLookUpEdit1.View = Me.RepositoryItemSearchLookUpEdit1View
        '
        'RepositoryItemSearchLookUpEdit1View
        '
        Me.RepositoryItemSearchLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
        Me.RepositoryItemSearchLookUpEdit1View.Name = "RepositoryItemSearchLookUpEdit1View"
        Me.RepositoryItemSearchLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = False
        Me.RepositoryItemSearchLookUpEdit1View.OptionsView.ShowGroupPanel = False
        '
        'RepositoryItemCheckEdit3
        '
        Me.RepositoryItemCheckEdit3.AutoHeight = False
        Me.RepositoryItemCheckEdit3.Caption = "Check"
        Me.RepositoryItemCheckEdit3.Name = "RepositoryItemCheckEdit3"
        '
        'RepositoryItemCheckEdit4
        '
        Me.RepositoryItemCheckEdit4.AutoHeight = False
        Me.RepositoryItemCheckEdit4.Caption = "Check"
        Me.RepositoryItemCheckEdit4.Name = "RepositoryItemCheckEdit4"
        '
        'uxFilterSearchText
        '
        Me.uxFilterSearchText.AutoHeight = False
        Me.uxFilterSearchText.Name = "uxFilterSearchText"
        '
        'RibbonStatusBar
        '
        Me.RibbonStatusBar.Cursor = System.Windows.Forms.Cursors.Default
        Me.RibbonStatusBar.ItemLinks.Add(Me.uxPollingRefresh)
        Me.RibbonStatusBar.ItemLinks.Add(Me.uxConnection)
        Me.RibbonStatusBar.ItemLinks.Add(Me.uxUser)
        Me.RibbonStatusBar.ItemLinks.Add(Me.uxRole)
        Me.RibbonStatusBar.Location = New System.Drawing.Point(0, 684)
        Me.RibbonStatusBar.Name = "RibbonStatusBar"
        Me.RibbonStatusBar.Ribbon = Me.uxMainRibbon
        Me.RibbonStatusBar.Size = New System.Drawing.Size(1393, 31)
        '
        'uxFilterWindow
        '
        Me.uxFilterWindow.ActiveGroup = Me.uxDocumentStatusFilter
        Me.uxFilterWindow.Appearance.ItemHotTracked.Options.UseFont = True
        Me.uxFilterWindow.Dock = System.Windows.Forms.DockStyle.Left
        Me.uxFilterWindow.Groups.AddRange(New DevExpress.XtraNavBar.NavBarGroup() {Me.uxDocumentStatusFilter, Me.uxPubTypeFilter, Me.uxEntityFilter, Me.uxStageFilter, Me.uxAuthorFilter, Me.Uxlocation, Me.uxdiststatus})
        Me.uxFilterWindow.LargeImages = Me.uxRibbonImagesLarge
        Me.uxFilterWindow.Location = New System.Drawing.Point(0, 144)
        Me.uxFilterWindow.Name = "uxFilterWindow"
        Me.uxFilterWindow.OptionsNavPane.ExpandedWidth = 200
        Me.uxFilterWindow.Size = New System.Drawing.Size(200, 540)
        Me.uxFilterWindow.SmallImages = Me.uxRibbonImagesSmall
        Me.uxFilterWindow.TabIndex = 2
        Me.uxFilterWindow.View = New DevExpress.XtraNavBar.ViewInfo.SkinExplorerBarViewInfoRegistrator()
        '
        'uxDocumentStatusFilter
        '
        Me.uxDocumentStatusFilter.Caption = "Document Status"
        Me.uxDocumentStatusFilter.Expanded = True
        Me.uxDocumentStatusFilter.Name = "uxDocumentStatusFilter"
        Me.uxDocumentStatusFilter.SmallImageIndex = 11
        '
        'uxPubTypeFilter
        '
        Me.uxPubTypeFilter.Caption = "Publication Type"
        Me.uxPubTypeFilter.Name = "uxPubTypeFilter"
        Me.uxPubTypeFilter.SmallImageIndex = 0
        '
        'uxEntityFilter
        '
        Me.uxEntityFilter.Caption = "Entity"
        Me.uxEntityFilter.Name = "uxEntityFilter"
        Me.uxEntityFilter.SmallImageIndex = 20
        '
        'uxStageFilter
        '
        Me.uxStageFilter.Caption = "Stage"
        Me.uxStageFilter.Expanded = True
        Me.uxStageFilter.Name = "uxStageFilter"
        Me.uxStageFilter.SmallImageIndex = 27
        '
        'uxAuthorFilter
        '
        Me.uxAuthorFilter.Caption = "Author"
        Me.uxAuthorFilter.Name = "uxAuthorFilter"
        Me.uxAuthorFilter.SmallImageIndex = 32
        '
        'Uxlocation
        '
        Me.Uxlocation.Caption = "Location"
        Me.Uxlocation.Expanded = True
        Me.Uxlocation.Name = "Uxlocation"
        Me.Uxlocation.SmallImageIndex = 19
        '
        'uxdiststatus
        '
        Me.uxdiststatus.Caption = "Distribution Status"
        Me.uxdiststatus.Name = "uxdiststatus"
        Me.uxdiststatus.SmallImageIndex = 19
        '
        'uxSplitContainer
        '
        Me.uxSplitContainer.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(236, Byte), Integer), CType(CType(239, Byte), Integer))
        Me.uxSplitContainer.Appearance.Options.UseBackColor = True
        Me.uxSplitContainer.Collapsed = True
        Me.uxSplitContainer.CollapsePanel = DevExpress.XtraEditors.SplitCollapsePanel.Panel2
        Me.uxSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.uxSplitContainer.FixedPanel = DevExpress.XtraEditors.SplitFixedPanel.Panel2
        Me.uxSplitContainer.Location = New System.Drawing.Point(200, 144)
        Me.uxSplitContainer.Name = "uxSplitContainer"
        Me.uxSplitContainer.Panel1.Controls.Add(Me.uxDocumentListPanel)
        Me.uxSplitContainer.Panel2.AppearanceCaption.Font = New System.Drawing.Font("Tahoma", 10.0!)
        Me.uxSplitContainer.Panel2.AppearanceCaption.Options.UseFont = True
        Me.uxSplitContainer.Panel2.AppearanceCaption.Options.UseTextOptions = True
        Me.uxSplitContainer.Panel2.AppearanceCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
        Me.uxSplitContainer.Panel2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.[Default]
        Me.uxSplitContainer.Panel2.CaptionLocation = DevExpress.Utils.Locations.Left
        Me.uxSplitContainer.Panel2.Controls.Add(Me.uxDocumentDetailsPanel)
        Me.uxSplitContainer.Panel2.MinSize = 366
        Me.uxSplitContainer.Panel2.ShowCaption = True
        Me.uxSplitContainer.Panel2.Text = "Document Details"
        Me.uxSplitContainer.Size = New System.Drawing.Size(1193, 540)
        Me.uxSplitContainer.SplitterPosition = 366
        Me.uxSplitContainer.TabIndex = 3
        '
        'uxDocumentListPanel
        '
        Me.uxDocumentListPanel.Controls.Add(Me.uxFileMenuPanel)
        Me.uxDocumentListPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.uxDocumentListPanel.Location = New System.Drawing.Point(0, 0)
        Me.uxDocumentListPanel.Name = "uxDocumentListPanel"
        Me.uxDocumentListPanel.Size = New System.Drawing.Size(1188, 540)
        Me.uxDocumentListPanel.TabIndex = 0
        '
        'uxDocumentDetailsPanel
        '
        Me.uxDocumentDetailsPanel.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(236, Byte), Integer), CType(CType(239, Byte), Integer))
        Me.uxDocumentDetailsPanel.Appearance.Options.UseBackColor = True
        Me.uxDocumentDetailsPanel.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me.uxDocumentDetailsPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.uxDocumentDetailsPanel.Location = New System.Drawing.Point(0, 0)
        Me.uxDocumentDetailsPanel.Name = "uxDocumentDetailsPanel"
        Me.uxDocumentDetailsPanel.Size = New System.Drawing.Size(0, 0)
        Me.uxDocumentDetailsPanel.TabIndex = 0
        '
        'BackstageViewButtonItem1
        '
        Me.BackstageViewButtonItem1.Caption = "BackstageViewButtonItem1"
        Me.BackstageViewButtonItem1.Name = "BackstageViewButtonItem1"
        '
        'uxStatus
        '
        Me.uxStatus.Caption = "Status:"
        Me.uxStatus.Id = 19
        Me.uxStatus.Name = "uxStatus"
        Me.uxStatus.TextAlignment = System.Drawing.StringAlignment.Near
        Me.uxStatus.Width = 200
        '
        'BarEditItem1
        '
        Me.BarEditItem1.Caption = "Date from:"
        Me.BarEditItem1.CaptionAlignment = DevExpress.Utils.HorzAlignment.Far
        Me.BarEditItem1.Edit = Me.uxFilterDateFromCheckControl
        Me.BarEditItem1.EditValue = True
        Me.BarEditItem1.Id = 30
        Me.BarEditItem1.Name = "BarEditItem1"
        Me.BarEditItem1.Tag = "DATE_FROM_CHECK"
        Me.BarEditItem1.Width = 18
        '
        'BarEditItem2
        '
        Me.BarEditItem2.Caption = "Date from:"
        Me.BarEditItem2.CaptionAlignment = DevExpress.Utils.HorzAlignment.Far
        Me.BarEditItem2.Edit = Me.uxFilterDateFromCheckControl
        Me.BarEditItem2.EditValue = True
        Me.BarEditItem2.Id = 30
        Me.BarEditItem2.Name = "BarEditItem2"
        Me.BarEditItem2.Tag = "DATE_FROM_CHECK"
        Me.BarEditItem2.Width = 18
        '
        'uxalert
        '
        Me.uxalert.AppearanceCaption.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxalert.AppearanceCaption.Image = CType(resources.GetObject("uxalert.AppearanceCaption.Image"), System.Drawing.Image)
        Me.uxalert.AppearanceCaption.Options.UseFont = True
        Me.uxalert.AppearanceCaption.Options.UseImage = True
        Me.uxalert.AppearanceHotTrackedText.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxalert.AppearanceHotTrackedText.Options.UseFont = True
        Me.uxalert.AppearanceText.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxalert.AppearanceText.Options.UseFont = True
        Me.uxalert.AutoFormDelay = 1000
        Me.uxalert.AutoHeight = True
        Me.uxalert.Images = Me.uxRibbonImagesSmall
        Me.uxalert.ShowPinButton = False
        Me.uxalert.ShowToolTips = False
        '
        'BarEditItem3
        '
        Me.BarEditItem3.Caption = "Include Published"
        Me.BarEditItem3.Edit = Me.uxIncludePublishedControl
        Me.BarEditItem3.EditValue = False
        Me.BarEditItem3.Id = 37
        Me.BarEditItem3.Name = "BarEditItem3"
        Me.BarEditItem3.Tag = "INCLUDE_PUBLISHED"
        Me.BarEditItem3.Width = 18
        '
        'BarEditItem6
        '
        Me.BarEditItem6.Caption = "Include Published"
        Me.BarEditItem6.Edit = Me.uxIncludePublishedControl
        Me.BarEditItem6.EditValue = False
        Me.BarEditItem6.Id = 37
        Me.BarEditItem6.Name = "BarEditItem6"
        Me.BarEditItem6.Tag = "INCLUDE_PUBLISHED"
        Me.BarEditItem6.Width = 18
        '
        'BarEditItem7
        '
        Me.BarEditItem7.Caption = "Include Published"
        Me.BarEditItem7.Edit = Me.uxIncludePublishedControl
        Me.BarEditItem7.EditValue = False
        Me.BarEditItem7.Id = 37
        Me.BarEditItem7.Name = "BarEditItem7"
        Me.BarEditItem7.Tag = "INCLUDE_PUBLISHED"
        Me.BarEditItem7.Width = 18
        '
        'BarButtonItem4
        '
        Me.BarButtonItem4.Caption = "Regular Reports"
        Me.BarButtonItem4.Id = 65
        Me.BarButtonItem4.LargeImageIndex = 14
        Me.BarButtonItem4.Name = "BarButtonItem4"
        Me.BarButtonItem4.Tag = "REGULAR"
        Me.BarButtonItem4.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        '
        'RibbonPageGroup1
        '
        Me.RibbonPageGroup1.AllowTextClipping = False
        Me.RibbonPageGroup1.ItemLinks.Add(Me.uxstop)
        Me.RibbonPageGroup1.ItemLinks.Add(Me.uxrejectdoc)
        Me.RibbonPageGroup1.Name = "RibbonPageGroup1"
        Me.RibbonPageGroup1.ShowCaptionButton = False
        Me.RibbonPageGroup1.Text = "Stop Document"
        Me.RibbonPageGroup1.Visible = False
        '
        'bc_am_pr_main
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1393, 715)
        Me.Controls.Add(Me.uxSplitContainer)
        Me.Controls.Add(Me.uxFilterWindow)
        Me.Controls.Add(Me.RibbonStatusBar)
        Me.Controls.Add(Me.uxMainRibbon)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "bc_am_pr_main"
        Me.Ribbon = Me.uxMainRibbon
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.StatusBar = Me.RibbonStatusBar
        Me.Text = "Blue Curve - Process"
        CType(Me.uxMainRibbon, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxsplash, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxRibbonImagesSmall, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxFilterDateFromControl.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxFilterDateFromControl, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxFilterDateToControl.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxFilterDateToControl, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxFilterDateToCheckControl, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxFilterDateFromCheckControl, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxIncludePublishedControl, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemCheckEdit1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemCheckEdit2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemComboBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemCheckEdit5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxRibbonImagesLarge, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemDateEdit3.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemDateEdit3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemDateEdit4.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemDateEdit4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemSearchLookUpEdit1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemSearchLookUpEdit1View, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemCheckEdit3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemCheckEdit4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxFilterSearchText, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxFilterWindow, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxSplitContainer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.uxSplitContainer.ResumeLayout(False)
        CType(Me.uxDocumentListPanel, System.ComponentModel.ISupportInitialize).EndInit()
        Me.uxDocumentListPanel.ResumeLayout(False)
        CType(Me.uxDocumentDetailsPanel, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents uxMainRibbon As DevExpress.XtraBars.Ribbon.RibbonControl
    Friend WithEvents uxDocumentRibbonPage As DevExpress.XtraBars.Ribbon.RibbonPage
    Friend WithEvents uxCreateRibbonPageGroup As DevExpress.XtraBars.Ribbon.RibbonPageGroup
    Friend WithEvents RibbonStatusBar As DevExpress.XtraBars.Ribbon.RibbonStatusBar
    Friend WithEvents uxFilterWindow As DevExpress.XtraNavBar.NavBarControl
    Friend WithEvents uxDocumentStatusFilter As DevExpress.XtraNavBar.NavBarGroup
    Friend WithEvents uxSplitContainer As DevExpress.XtraEditors.SplitContainerControl
    Friend WithEvents uxPubTypeFilter As DevExpress.XtraNavBar.NavBarGroup
    Friend WithEvents uxEntityFilter As DevExpress.XtraNavBar.NavBarGroup
    Friend WithEvents uxStageFilter As DevExpress.XtraNavBar.NavBarGroup
    Friend WithEvents uxAuthorFilter As DevExpress.XtraNavBar.NavBarGroup
    Friend WithEvents uxViewRibbonPage As DevExpress.XtraBars.Ribbon.RibbonPage
    Friend WithEvents uxImportRibbonPageGroup As DevExpress.XtraBars.Ribbon.RibbonPageGroup
    Friend WithEvents uxCreate As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents uxReattachMasterDocument As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents uxRegisterMasterDocument As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents uxDocumentState As DevExpress.XtraBars.BarCheckItem
    Friend WithEvents uxImportMasterDocument As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents uxImportSupportDocument As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents uxImportRegisteredDocument As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents uxRefreshRibbonPageGroup As DevExpress.XtraBars.Ribbon.RibbonPageGroup
    Friend WithEvents uxRefresh As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents uxShowFilters As DevExpress.XtraBars.BarCheckItem
    Friend WithEvents uxSettings As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents uxFiltersRibbonGroupPage As DevExpress.XtraBars.Ribbon.RibbonPageGroup
    Friend WithEvents uxSettingsRibbonPageGroup As DevExpress.XtraBars.Ribbon.RibbonPageGroup
    Friend WithEvents uxFileMenuPanel As DevExpress.XtraBars.Ribbon.BackstageViewControl
    Friend WithEvents uxFileMenuExit As DevExpress.XtraBars.Ribbon.BackstageViewButtonItem
    Friend WithEvents BackstageViewButtonItem1 As DevExpress.XtraBars.Ribbon.BackstageViewButtonItem
    Friend WithEvents uxRibbonImagesSmall As DevExpress.Utils.ImageCollection
    Friend WithEvents uxRibbonImagesLarge As DevExpress.Utils.ImageCollection
    Friend WithEvents uxFilterDateFromControl As DevExpress.XtraEditors.Repository.RepositoryItemDateEdit
    Friend WithEvents uxFilterDateFrom As DevExpress.XtraBars.BarEditItem
    Friend WithEvents RepositoryItemDateEdit3 As DevExpress.XtraEditors.Repository.RepositoryItemDateEdit
    Friend WithEvents uxFilterDateTo As DevExpress.XtraBars.BarEditItem
    Friend WithEvents RepositoryItemDateEdit4 As DevExpress.XtraEditors.Repository.RepositoryItemDateEdit
    Friend WithEvents uxFilterDateToControl As DevExpress.XtraEditors.Repository.RepositoryItemDateEdit
    Friend WithEvents uxDocumentDetailsPanel As DevExpress.XtraEditors.PanelControl
    Friend WithEvents uxUser As DevExpress.XtraBars.BarStaticItem
    Friend WithEvents uxConnection As DevExpress.XtraBars.BarStaticItem
    Friend WithEvents uxRole As DevExpress.XtraBars.BarStaticItem
    Friend WithEvents uxFilterDateFromCheckControl As DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit
    Friend WithEvents uxFilterDateToCheckControl As DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit
    Friend WithEvents RepositoryItemSearchLookUpEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit
    Friend WithEvents RepositoryItemSearchLookUpEdit1View As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents uxFilterDateToCheck As DevExpress.XtraBars.BarEditItem
    Friend WithEvents RepositoryItemCheckEdit3 As DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit
    Friend WithEvents uxFilterDateFromCheck As DevExpress.XtraBars.BarEditItem
    Friend WithEvents RepositoryItemCheckEdit4 As DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit
    Friend WithEvents uxFilterSearchText As DevExpress.XtraEditors.Repository.RepositoryItemTextEdit
    Friend WithEvents uxForceCheckIn As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents uxDetailsRibbonPageGroup As DevExpress.XtraBars.Ribbon.RibbonPageGroup
    Friend WithEvents uxPollingRefresh As DevExpress.XtraBars.BarStaticItem
    Friend WithEvents uxIncludePublished As DevExpress.XtraBars.BarEditItem
    Friend WithEvents uxIncludePublishedControl As DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit
    Friend WithEvents uxShowDetails As DevExpress.XtraBars.BarCheckItem
    Friend WithEvents uxDocumentListPanel As DevExpress.XtraEditors.PanelControl
    Friend WithEvents uxCategoriseDocument As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents uxCategoriseRibbonPageGroup As DevExpress.XtraBars.Ribbon.RibbonPageGroup
    Friend WithEvents uxViewDocument As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents uxViewRibbonPageGroup As DevExpress.XtraBars.Ribbon.RibbonPageGroup
    Friend WithEvents uxStatus As DevExpress.XtraBars.BarStaticItem
    Friend WithEvents BarEditItem1 As DevExpress.XtraBars.BarEditItem
    Friend WithEvents BarEditItem2 As DevExpress.XtraBars.BarEditItem
    Friend WithEvents uxalert As DevExpress.XtraBars.Alerter.AlertControl
    Friend WithEvents BarButtonGroup1 As DevExpress.XtraBars.BarButtonGroup
    Friend WithEvents uxresetfilters As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents uxsplash As DevExpress.Utils.ImageCollection
    Friend WithEvents uxcomponents As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents Uxlocation As DevExpress.XtraNavBar.NavBarGroup
    Friend WithEvents uxhtmlpreview As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem1 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents rdistribute As DevExpress.XtraBars.Ribbon.RibbonPageGroup
    Friend WithEvents uxdistribution As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem2 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarCheckItem1 As DevExpress.XtraBars.BarCheckItem
    Friend WithEvents uxpublishonly As DevExpress.XtraBars.BarEditItem
    Friend WithEvents RepositoryItemCheckEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit
    Friend WithEvents BarEditItem3 As DevExpress.XtraBars.BarEditItem
    Friend WithEvents BarCheckItem2 As DevExpress.XtraBars.BarCheckItem
    Friend WithEvents BarEditItem4 As DevExpress.XtraBars.BarEditItem
    Friend WithEvents RepositoryItemCheckEdit2 As DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit
    Friend WithEvents BarEditItem5 As DevExpress.XtraBars.BarEditItem
    Friend WithEvents RepositoryItemComboBox1 As DevExpress.XtraEditors.Repository.RepositoryItemComboBox
    Friend WithEvents BarEditItem6 As DevExpress.XtraBars.BarEditItem
    Friend WithEvents BarEditItem7 As DevExpress.XtraBars.BarEditItem
    Friend WithEvents BarEditItem8 As DevExpress.XtraBars.BarEditItem
    Friend WithEvents RepositoryItemCheckEdit5 As DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit
    Friend WithEvents uxdiststatus As DevExpress.XtraNavBar.NavBarGroup
    Friend WithEvents uxregular As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents uxattestation As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents uxattestationgroup As DevExpress.XtraBars.Ribbon.RibbonPageGroup
    Friend WithEvents BarButtonGroup2 As DevExpress.XtraBars.BarButtonGroup
    Friend WithEvents uxstop As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents uxrejectdoc As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents uxviewattestation As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents uxstopdocgroup As DevExpress.XtraBars.Ribbon.RibbonPageGroup
    Friend WithEvents uxattributes As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents BarButtonItem3 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents uxdisclosurefile As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents uxgenerate As DevExpress.XtraBars.Ribbon.RibbonPageGroup
    Friend WithEvents BarButtonItem4 As DevExpress.XtraBars.BarButtonItem
    Friend WithEvents RibbonPageGroup1 As DevExpress.XtraBars.Ribbon.RibbonPageGroup


End Class
