<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bc_am_dd_main
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_dd_main))
        Me.uxFooterPanel = New System.Windows.Forms.Panel()
        Me.uxServer = New System.Windows.Forms.Label()
        Me.uxRole = New System.Windows.Forms.Label()
        Me.uxUser = New System.Windows.Forms.Label()
        Me.uxFooterLogo = New System.Windows.Forms.PictureBox()
        Me.uxHeaderPanel = New System.Windows.Forms.Panel()
        Me.uxBanner = New System.Windows.Forms.PictureBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.uxMainMenu = New System.Windows.Forms.MainMenu(Me.components)
        Me.uxFile = New System.Windows.Forms.MenuItem()
        Me.uxExit = New System.Windows.Forms.MenuItem()
        Me.MenuItem2 = New System.Windows.Forms.MenuItem()
        Me.miCompare = New System.Windows.Forms.MenuItem()
        Me.miExport = New System.Windows.Forms.MenuItem()
        Me.miImport = New System.Windows.Forms.MenuItem()
        Me.miLock = New System.Windows.Forms.MenuItem()
        Me.miUnlock = New System.Windows.Forms.MenuItem()
        Me.miExit = New System.Windows.Forms.MenuItem()
        Me.uxHelp = New System.Windows.Forms.MenuItem()
        Me.uxAbout = New System.Windows.Forms.MenuItem()
        Me.uxBodyPanel = New System.Windows.Forms.Panel()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.uxObjectTreeView = New BlueCurve.Deployment.AM.bc_am_dd_tree()
        Me.uxSettingsPanel = New System.Windows.Forms.Panel()
        Me.pnlCompare = New System.Windows.Forms.GroupBox()
        Me.badupError = New BlueCurve.Deployment.AM.bc_am_dd_uri_picker()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cbValidateOnly = New System.Windows.Forms.CheckBox()
        Me.cbUpdateDestination = New System.Windows.Forms.CheckBox()
        Me.badupDifferenceFile = New BlueCurve.Deployment.AM.bc_am_dd_uri_picker()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.uxImportOptions = New System.Windows.Forms.GroupBox()
        Me.badupImportPath = New BlueCurve.Deployment.AM.bc_am_dd_uri_picker()
        Me.lblDestination = New System.Windows.Forms.Label()
        Me.cbCheckDependencies = New System.Windows.Forms.CheckBox()
        Me.uxExportOptions = New System.Windows.Forms.GroupBox()
        Me.cbOverwriteAll = New System.Windows.Forms.CheckBox()
        Me.cbArchiveSource = New System.Windows.Forms.CheckBox()
        Me.lblMask = New System.Windows.Forms.Label()
        Me.badupExclusionFile = New BlueCurve.Deployment.AM.bc_am_dd_uri_picker()
        Me.txtArchiveExtension = New System.Windows.Forms.TextBox()
        Me.cbIncludeDependencies = New System.Windows.Forms.CheckBox()
        Me.badupExportPath = New BlueCurve.Deployment.AM.bc_am_dd_uri_picker()
        Me.uxArchiveFileExt = New System.Windows.Forms.Label()
        Me.lblSource = New System.Windows.Forms.Label()
        Me.uxToolBarMain = New System.Windows.Forms.ToolBar()
        Me.uxToolBarImages = New System.Windows.Forms.ImageList(Me.components)
        Me.ilTree = New System.Windows.Forms.ImageList(Me.components)
        Me.uxFooterPanel.SuspendLayout()
        CType(Me.uxFooterLogo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.uxHeaderPanel.SuspendLayout()
        CType(Me.uxBanner, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.uxBodyPanel.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.uxSettingsPanel.SuspendLayout()
        Me.pnlCompare.SuspendLayout()
        Me.uxImportOptions.SuspendLayout()
        Me.uxExportOptions.SuspendLayout()
        Me.SuspendLayout()
        '
        'uxFooterPanel
        '
        Me.uxFooterPanel.BackColor = System.Drawing.Color.White
        Me.uxFooterPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.uxFooterPanel.Controls.Add(Me.uxServer)
        Me.uxFooterPanel.Controls.Add(Me.uxRole)
        Me.uxFooterPanel.Controls.Add(Me.uxUser)
        Me.uxFooterPanel.Controls.Add(Me.uxFooterLogo)
        Me.uxFooterPanel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.uxFooterPanel.Location = New System.Drawing.Point(0, 482)
        Me.uxFooterPanel.Name = "uxFooterPanel"
        Me.uxFooterPanel.Size = New System.Drawing.Size(702, 60)
        Me.uxFooterPanel.TabIndex = 6
        '
        'uxServer
        '
        Me.uxServer.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxServer.Location = New System.Drawing.Point(282, 34)
        Me.uxServer.Name = "uxServer"
        Me.uxServer.Size = New System.Drawing.Size(406, 15)
        Me.uxServer.TabIndex = 33
        Me.uxServer.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'uxRole
        '
        Me.uxRole.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxRole.Location = New System.Drawing.Point(466, 12)
        Me.uxRole.Name = "uxRole"
        Me.uxRole.Size = New System.Drawing.Size(222, 16)
        Me.uxRole.TabIndex = 30
        Me.uxRole.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'uxUser
        '
        Me.uxUser.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.uxUser.Location = New System.Drawing.Point(246, 12)
        Me.uxUser.Name = "uxUser"
        Me.uxUser.Size = New System.Drawing.Size(238, 16)
        Me.uxUser.TabIndex = 29
        '
        'uxFooterLogo
        '
        Me.uxFooterLogo.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.uxFooterLogo.BackColor = System.Drawing.Color.White
        Me.uxFooterLogo.Image = CType(resources.GetObject("uxFooterLogo.Image"), System.Drawing.Image)
        Me.uxFooterLogo.Location = New System.Drawing.Point(5, 3)
        Me.uxFooterLogo.Name = "uxFooterLogo"
        Me.uxFooterLogo.Size = New System.Drawing.Size(280, 32)
        Me.uxFooterLogo.TabIndex = 24
        Me.uxFooterLogo.TabStop = False
        '
        'uxHeaderPanel
        '
        Me.uxHeaderPanel.Controls.Add(Me.uxBanner)
        Me.uxHeaderPanel.Controls.Add(Me.PictureBox1)
        Me.uxHeaderPanel.Dock = System.Windows.Forms.DockStyle.Top
        Me.uxHeaderPanel.Location = New System.Drawing.Point(0, 0)
        Me.uxHeaderPanel.Name = "uxHeaderPanel"
        Me.uxHeaderPanel.Size = New System.Drawing.Size(702, 64)
        Me.uxHeaderPanel.TabIndex = 8
        '
        'uxBanner
        '
        Me.uxBanner.ErrorImage = Nothing
        Me.uxBanner.Image = CType(resources.GetObject("uxBanner.Image"), System.Drawing.Image)
        Me.uxBanner.InitialImage = Nothing
        Me.uxBanner.Location = New System.Drawing.Point(0, 0)
        Me.uxBanner.Name = "uxBanner"
        Me.uxBanner.Size = New System.Drawing.Size(432, 64)
        Me.uxBanner.TabIndex = 12
        Me.uxBanner.TabStop = False
        '
        'PictureBox1
        '
        Me.PictureBox1.Dock = System.Windows.Forms.DockStyle.Right
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(637, 0)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(65, 64)
        Me.PictureBox1.TabIndex = 11
        Me.PictureBox1.TabStop = False
        '
        'uxMainMenu
        '
        Me.uxMainMenu.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.uxFile, Me.uxHelp})
        '
        'uxFile
        '
        Me.uxFile.Index = 0
        Me.uxFile.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.uxExit, Me.MenuItem2, Me.miExit})
        Me.uxFile.Text = "&File"
        '
        'uxExit
        '
        Me.uxExit.Index = 0
        Me.uxExit.Text = "Lock System"
        Me.uxExit.Visible = False
        '
        'MenuItem2
        '
        Me.MenuItem2.Index = 1
        Me.MenuItem2.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.miCompare, Me.miExport, Me.miImport, Me.miLock, Me.miUnlock})
        Me.MenuItem2.Text = "Save Operation"
        '
        'miCompare
        '
        Me.miCompare.Index = 0
        Me.miCompare.Text = "Compare"
        '
        'miExport
        '
        Me.miExport.Index = 1
        Me.miExport.Text = "Export"
        '
        'miImport
        '
        Me.miImport.Index = 2
        Me.miImport.Text = "Import"
        '
        'miLock
        '
        Me.miLock.Index = 3
        Me.miLock.Text = "Lock"
        '
        'miUnlock
        '
        Me.miUnlock.Index = 4
        Me.miUnlock.Text = "Unlock"
        '
        'miExit
        '
        Me.miExit.Index = 2
        Me.miExit.Text = "Exit"
        '
        'uxHelp
        '
        Me.uxHelp.Index = 1
        Me.uxHelp.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.uxAbout})
        Me.uxHelp.Text = "&Help"
        '
        'uxAbout
        '
        Me.uxAbout.Index = 0
        Me.uxAbout.Text = "&About"
        '
        'uxBodyPanel
        '
        Me.uxBodyPanel.Controls.Add(Me.SplitContainer1)
        Me.uxBodyPanel.Controls.Add(Me.uxToolBarMain)
        Me.uxBodyPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.uxBodyPanel.Location = New System.Drawing.Point(0, 64)
        Me.uxBodyPanel.Name = "uxBodyPanel"
        Me.uxBodyPanel.Size = New System.Drawing.Size(702, 418)
        Me.uxBodyPanel.TabIndex = 9
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2
        Me.SplitContainer1.IsSplitterFixed = True
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 42)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.uxObjectTreeView)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.uxSettingsPanel)
        Me.SplitContainer1.Size = New System.Drawing.Size(702, 376)
        Me.SplitContainer1.SplitterDistance = 174
        Me.SplitContainer1.TabIndex = 11
        '
        'uxObjectTreeView
        '
        Me.uxObjectTreeView.CheckBoxes = True
        Me.uxObjectTreeView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.uxObjectTreeView.Location = New System.Drawing.Point(0, 0)
        Me.uxObjectTreeView.Name = "uxObjectTreeView"
        Me.uxObjectTreeView.Size = New System.Drawing.Size(174, 376)
        Me.uxObjectTreeView.TabIndex = 1
        '
        'uxSettingsPanel
        '
        Me.uxSettingsPanel.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.uxSettingsPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.uxSettingsPanel.Controls.Add(Me.pnlCompare)
        Me.uxSettingsPanel.Controls.Add(Me.uxImportOptions)
        Me.uxSettingsPanel.Controls.Add(Me.uxExportOptions)
        Me.uxSettingsPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.uxSettingsPanel.Location = New System.Drawing.Point(0, 0)
        Me.uxSettingsPanel.MinimumSize = New System.Drawing.Size(524, 376)
        Me.uxSettingsPanel.Name = "uxSettingsPanel"
        Me.uxSettingsPanel.Size = New System.Drawing.Size(524, 376)
        Me.uxSettingsPanel.TabIndex = 0
        '
        'pnlCompare
        '
        Me.pnlCompare.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.pnlCompare.Controls.Add(Me.badupError)
        Me.pnlCompare.Controls.Add(Me.Label2)
        Me.pnlCompare.Controls.Add(Me.cbValidateOnly)
        Me.pnlCompare.Controls.Add(Me.cbUpdateDestination)
        Me.pnlCompare.Controls.Add(Me.badupDifferenceFile)
        Me.pnlCompare.Controls.Add(Me.Label1)
        Me.pnlCompare.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlCompare.Location = New System.Drawing.Point(7, 212)
        Me.pnlCompare.Name = "pnlCompare"
        Me.pnlCompare.Size = New System.Drawing.Size(505, 97)
        Me.pnlCompare.TabIndex = 16
        Me.pnlCompare.TabStop = False
        Me.pnlCompare.Text = "Import Options"
        '
        'badupError
        '
        Me.badupError.BackColor = System.Drawing.Color.Transparent
        Me.badupError.DefaultUri = Nothing
        Me.badupError.Description = "Select an Import Path"
        Me.badupError.FileMask = "xml files (*.xml)|*.xml|All files (*.*)|*.*"
        Me.badupError.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.badupError.Location = New System.Drawing.Point(97, 65)
        Me.badupError.Name = "badupError"
        Me.badupError.SaveDialog = True
        Me.badupError.Size = New System.Drawing.Size(401, 20)
        Me.badupError.TabIndex = 13
        Me.badupError.Type = BlueCurve.Deployment.AM.bc_am_dd_enums.UriPickerType.File
        Me.badupError.Uri = ""
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(6, 72)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(56, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Errors File:"
        '
        'cbValidateOnly
        '
        Me.cbValidateOnly.AutoSize = True
        Me.cbValidateOnly.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cbValidateOnly.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbValidateOnly.Location = New System.Drawing.Point(284, 42)
        Me.cbValidateOnly.Name = "cbValidateOnly"
        Me.cbValidateOnly.Size = New System.Drawing.Size(88, 17)
        Me.cbValidateOnly.TabIndex = 11
        Me.cbValidateOnly.Text = "Validate Only"
        Me.cbValidateOnly.UseVisualStyleBackColor = True
        '
        'cbUpdateDestination
        '
        Me.cbUpdateDestination.AutoSize = True
        Me.cbUpdateDestination.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cbUpdateDestination.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbUpdateDestination.Location = New System.Drawing.Point(378, 42)
        Me.cbUpdateDestination.Name = "cbUpdateDestination"
        Me.cbUpdateDestination.Size = New System.Drawing.Size(85, 17)
        Me.cbUpdateDestination.TabIndex = 12
        Me.cbUpdateDestination.Text = "Update Files"
        Me.cbUpdateDestination.UseVisualStyleBackColor = True
        '
        'badupDifferenceFile
        '
        Me.badupDifferenceFile.BackColor = System.Drawing.Color.Transparent
        Me.badupDifferenceFile.DefaultUri = Nothing
        Me.badupDifferenceFile.Description = "Select an Import Path"
        Me.badupDifferenceFile.FileMask = "xml files (*.xml)|*.xml|All files (*.*)|*.*"
        Me.badupDifferenceFile.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.badupDifferenceFile.Location = New System.Drawing.Point(97, 16)
        Me.badupDifferenceFile.Name = "badupDifferenceFile"
        Me.badupDifferenceFile.SaveDialog = True
        Me.badupDifferenceFile.Size = New System.Drawing.Size(401, 20)
        Me.badupDifferenceFile.TabIndex = 10
        Me.badupDifferenceFile.Type = BlueCurve.Deployment.AM.bc_am_dd_enums.UriPickerType.File
        Me.badupDifferenceFile.Uri = ""
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(6, 23)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(83, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Instructions File:"
        '
        'uxImportOptions
        '
        Me.uxImportOptions.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.uxImportOptions.Controls.Add(Me.badupImportPath)
        Me.uxImportOptions.Controls.Add(Me.lblDestination)
        Me.uxImportOptions.Controls.Add(Me.cbCheckDependencies)
        Me.uxImportOptions.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxImportOptions.Location = New System.Drawing.Point(7, 143)
        Me.uxImportOptions.Name = "uxImportOptions"
        Me.uxImportOptions.Size = New System.Drawing.Size(505, 65)
        Me.uxImportOptions.TabIndex = 5
        Me.uxImportOptions.TabStop = False
        Me.uxImportOptions.Text = "Compare Options"
        '
        'badupImportPath
        '
        Me.badupImportPath.BackColor = System.Drawing.Color.Transparent
        Me.badupImportPath.DefaultUri = Nothing
        Me.badupImportPath.Description = "Select an Import Path"
        Me.badupImportPath.FileMask = Nothing
        Me.badupImportPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.badupImportPath.Location = New System.Drawing.Point(97, 16)
        Me.badupImportPath.Name = "badupImportPath"
        Me.badupImportPath.SaveDialog = False
        Me.badupImportPath.Size = New System.Drawing.Size(401, 20)
        Me.badupImportPath.TabIndex = 8
        Me.badupImportPath.Type = BlueCurve.Deployment.AM.bc_am_dd_enums.UriPickerType.Folder
        Me.badupImportPath.Uri = ""
        '
        'lblDestination
        '
        Me.lblDestination.AutoSize = True
        Me.lblDestination.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDestination.Location = New System.Drawing.Point(6, 23)
        Me.lblDestination.Name = "lblDestination"
        Me.lblDestination.Size = New System.Drawing.Size(88, 13)
        Me.lblDestination.TabIndex = 4
        Me.lblDestination.Text = "Destination Path:"
        '
        'cbCheckDependencies
        '
        Me.cbCheckDependencies.AutoSize = True
        Me.cbCheckDependencies.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cbCheckDependencies.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbCheckDependencies.Location = New System.Drawing.Point(334, 42)
        Me.cbCheckDependencies.Name = "cbCheckDependencies"
        Me.cbCheckDependencies.Size = New System.Drawing.Size(129, 17)
        Me.cbCheckDependencies.TabIndex = 9
        Me.cbCheckDependencies.Text = "Check Dependancies"
        Me.cbCheckDependencies.UseVisualStyleBackColor = True
        '
        'uxExportOptions
        '
        Me.uxExportOptions.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.uxExportOptions.Controls.Add(Me.cbOverwriteAll)
        Me.uxExportOptions.Controls.Add(Me.cbArchiveSource)
        Me.uxExportOptions.Controls.Add(Me.lblMask)
        Me.uxExportOptions.Controls.Add(Me.badupExclusionFile)
        Me.uxExportOptions.Controls.Add(Me.txtArchiveExtension)
        Me.uxExportOptions.Controls.Add(Me.cbIncludeDependencies)
        Me.uxExportOptions.Controls.Add(Me.badupExportPath)
        Me.uxExportOptions.Controls.Add(Me.uxArchiveFileExt)
        Me.uxExportOptions.Controls.Add(Me.lblSource)
        Me.uxExportOptions.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxExportOptions.Location = New System.Drawing.Point(7, 9)
        Me.uxExportOptions.Name = "uxExportOptions"
        Me.uxExportOptions.Size = New System.Drawing.Size(505, 128)
        Me.uxExportOptions.TabIndex = 4
        Me.uxExportOptions.TabStop = False
        Me.uxExportOptions.Text = "Export Options"
        '
        'cbOverwriteAll
        '
        Me.cbOverwriteAll.AutoSize = True
        Me.cbOverwriteAll.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cbOverwriteAll.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbOverwriteAll.Location = New System.Drawing.Point(378, 42)
        Me.cbOverwriteAll.Name = "cbOverwriteAll"
        Me.cbOverwriteAll.Size = New System.Drawing.Size(85, 17)
        Me.cbOverwriteAll.TabIndex = 4
        Me.cbOverwriteAll.Text = "Overwrite All"
        Me.cbOverwriteAll.UseVisualStyleBackColor = True
        '
        'cbArchiveSource
        '
        Me.cbArchiveSource.AutoSize = True
        Me.cbArchiveSource.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cbArchiveSource.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbArchiveSource.Location = New System.Drawing.Point(243, 103)
        Me.cbArchiveSource.Name = "cbArchiveSource"
        Me.cbArchiveSource.Size = New System.Drawing.Size(62, 17)
        Me.cbArchiveSource.TabIndex = 6
        Me.cbArchiveSource.Text = "Archive"
        Me.cbArchiveSource.UseVisualStyleBackColor = True
        '
        'lblMask
        '
        Me.lblMask.AutoSize = True
        Me.lblMask.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMask.Location = New System.Drawing.Point(6, 77)
        Me.lblMask.Name = "lblMask"
        Me.lblMask.Size = New System.Drawing.Size(74, 13)
        Me.lblMask.TabIndex = 1
        Me.lblMask.Text = "Exclusion File:"
        '
        'badupExclusionFile
        '
        Me.badupExclusionFile.BackColor = System.Drawing.Color.Transparent
        Me.badupExclusionFile.DefaultUri = Nothing
        Me.badupExclusionFile.Description = "Select an Exclusions File"
        Me.badupExclusionFile.FileMask = "xml files (*.xml)|*.xml|All files (*.*)|*.*"
        Me.badupExclusionFile.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.badupExclusionFile.Location = New System.Drawing.Point(97, 71)
        Me.badupExclusionFile.Name = "badupExclusionFile"
        Me.badupExclusionFile.SaveDialog = False
        Me.badupExclusionFile.Size = New System.Drawing.Size(401, 20)
        Me.badupExclusionFile.TabIndex = 5
        Me.badupExclusionFile.Type = BlueCurve.Deployment.AM.bc_am_dd_enums.UriPickerType.File
        Me.badupExclusionFile.Uri = Nothing
        '
        'txtArchiveExtension
        '
        Me.txtArchiveExtension.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtArchiveExtension.Location = New System.Drawing.Point(399, 101)
        Me.txtArchiveExtension.Name = "txtArchiveExtension"
        Me.txtArchiveExtension.Size = New System.Drawing.Size(64, 20)
        Me.txtArchiveExtension.TabIndex = 7
        '
        'cbIncludeDependencies
        '
        Me.cbIncludeDependencies.AutoSize = True
        Me.cbIncludeDependencies.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cbIncludeDependencies.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbIncludeDependencies.Location = New System.Drawing.Point(239, 42)
        Me.cbIncludeDependencies.Name = "cbIncludeDependencies"
        Me.cbIncludeDependencies.Size = New System.Drawing.Size(133, 17)
        Me.cbIncludeDependencies.TabIndex = 3
        Me.cbIncludeDependencies.Text = "Include Dependancies"
        Me.cbIncludeDependencies.UseVisualStyleBackColor = True
        '
        'badupExportPath
        '
        Me.badupExportPath.BackColor = System.Drawing.Color.Transparent
        Me.badupExportPath.DefaultUri = Nothing
        Me.badupExportPath.Description = "Select an Export Path"
        Me.badupExportPath.FileMask = Nothing
        Me.badupExportPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.badupExportPath.Location = New System.Drawing.Point(97, 16)
        Me.badupExportPath.Name = "badupExportPath"
        Me.badupExportPath.SaveDialog = False
        Me.badupExportPath.Size = New System.Drawing.Size(401, 20)
        Me.badupExportPath.TabIndex = 2
        Me.badupExportPath.Type = BlueCurve.Deployment.AM.bc_am_dd_enums.UriPickerType.Folder
        Me.badupExportPath.Uri = ""
        '
        'uxArchiveFileExt
        '
        Me.uxArchiveFileExt.AutoSize = True
        Me.uxArchiveFileExt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxArchiveFileExt.Location = New System.Drawing.Point(315, 104)
        Me.uxArchiveFileExt.Name = "uxArchiveFileExt"
        Me.uxArchiveFileExt.Size = New System.Drawing.Size(83, 13)
        Me.uxArchiveFileExt.TabIndex = 13
        Me.uxArchiveFileExt.Text = "Archive File Ext."
        '
        'lblSource
        '
        Me.lblSource.AutoSize = True
        Me.lblSource.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSource.Location = New System.Drawing.Point(6, 23)
        Me.lblSource.Name = "lblSource"
        Me.lblSource.Size = New System.Drawing.Size(69, 13)
        Me.lblSource.TabIndex = 0
        Me.lblSource.Text = "Source Path:"
        '
        'uxToolBarMain
        '
        Me.uxToolBarMain.Appearance = System.Windows.Forms.ToolBarAppearance.Flat
        Me.uxToolBarMain.AutoSize = False
        Me.uxToolBarMain.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.uxToolBarMain.ButtonSize = New System.Drawing.Size(16, 16)
        Me.uxToolBarMain.DropDownArrows = True
        Me.uxToolBarMain.ImageList = Me.uxToolBarImages
        Me.uxToolBarMain.Location = New System.Drawing.Point(0, 0)
        Me.uxToolBarMain.Name = "uxToolBarMain"
        Me.uxToolBarMain.ShowToolTips = True
        Me.uxToolBarMain.Size = New System.Drawing.Size(702, 42)
        Me.uxToolBarMain.TabIndex = 10
        '
        'uxToolBarImages
        '
        Me.uxToolBarImages.ImageStream = CType(resources.GetObject("uxToolBarImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.uxToolBarImages.TransparentColor = System.Drawing.Color.Transparent
        Me.uxToolBarImages.Images.SetKeyName(0, "")
        Me.uxToolBarImages.Images.SetKeyName(1, "")
        Me.uxToolBarImages.Images.SetKeyName(2, "")
        Me.uxToolBarImages.Images.SetKeyName(3, "")
        Me.uxToolBarImages.Images.SetKeyName(4, "lock-break-icon.png")
        Me.uxToolBarImages.Images.SetKeyName(5, "lock-icon.png")
        Me.uxToolBarImages.Images.SetKeyName(6, "saveHS.png")
        Me.uxToolBarImages.Images.SetKeyName(7, "RefreshDocViewHS.png")
        '
        'ilTree
        '
        Me.ilTree.ImageStream = CType(resources.GetObject("ilTree.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ilTree.TransparentColor = System.Drawing.Color.Transparent
        Me.ilTree.Images.SetKeyName(0, "unchecked.png")
        Me.ilTree.Images.SetKeyName(1, "checked.png")
        '
        'bc_am_dd_main
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(702, 542)
        Me.Controls.Add(Me.uxBodyPanel)
        Me.Controls.Add(Me.uxHeaderPanel)
        Me.Controls.Add(Me.uxFooterPanel)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Menu = Me.uxMainMenu
        Me.MinimumSize = New System.Drawing.Size(710, 569)
        Me.Name = "bc_am_dd_main"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Deployment Manager - Blue Curve"
        Me.uxFooterPanel.ResumeLayout(False)
        CType(Me.uxFooterLogo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.uxHeaderPanel.ResumeLayout(False)
        CType(Me.uxBanner, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.uxBodyPanel.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.uxSettingsPanel.ResumeLayout(False)
        Me.pnlCompare.ResumeLayout(False)
        Me.pnlCompare.PerformLayout()
        Me.uxImportOptions.ResumeLayout(False)
        Me.uxImportOptions.PerformLayout()
        Me.uxExportOptions.ResumeLayout(False)
        Me.uxExportOptions.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents uxFooterPanel As System.Windows.Forms.Panel
    Public WithEvents uxServer As System.Windows.Forms.Label
    Public WithEvents uxRole As System.Windows.Forms.Label
    Public WithEvents uxUser As System.Windows.Forms.Label
    Friend WithEvents uxFooterLogo As System.Windows.Forms.PictureBox
    Friend WithEvents uxHeaderPanel As System.Windows.Forms.Panel
    Friend WithEvents uxMainMenu As System.Windows.Forms.MainMenu
    Friend WithEvents uxFile As System.Windows.Forms.MenuItem
    Friend WithEvents uxExit As System.Windows.Forms.MenuItem
    Friend WithEvents uxHelp As System.Windows.Forms.MenuItem
    Friend WithEvents uxAbout As System.Windows.Forms.MenuItem
    Friend WithEvents uxBodyPanel As System.Windows.Forms.Panel
    Friend WithEvents uxToolBarMain As System.Windows.Forms.ToolBar
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents uxToolBarImages As System.Windows.Forms.ImageList
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents uxObjectTreeView As bc_am_dd_tree
    Friend WithEvents miExit As System.Windows.Forms.MenuItem
    Friend WithEvents uxSettingsPanel As System.Windows.Forms.Panel
    Friend WithEvents uxImportOptions As System.Windows.Forms.GroupBox
    Friend WithEvents lblDestination As System.Windows.Forms.Label
    Friend WithEvents uxExportOptions As System.Windows.Forms.GroupBox
    Friend WithEvents lblMask As System.Windows.Forms.Label
    Friend WithEvents lblSource As System.Windows.Forms.Label
    Friend WithEvents badupExportPath As BlueCurve.Deployment.AM.bc_am_dd_uri_picker
    Friend WithEvents badupImportPath As BlueCurve.Deployment.AM.bc_am_dd_uri_picker
    Friend WithEvents badupExclusionFile As BlueCurve.Deployment.AM.bc_am_dd_uri_picker
    Friend WithEvents ilTree As System.Windows.Forms.ImageList
    Friend WithEvents cbValidateOnly As System.Windows.Forms.CheckBox
    Friend WithEvents cbIncludeDependencies As System.Windows.Forms.CheckBox
    Friend WithEvents cbCheckDependencies As System.Windows.Forms.CheckBox
    Friend WithEvents uxArchiveFileExt As System.Windows.Forms.Label
    Friend WithEvents txtArchiveExtension As System.Windows.Forms.TextBox
    Friend WithEvents pnlCompare As System.Windows.Forms.GroupBox
    Friend WithEvents badupDifferenceFile As BlueCurve.Deployment.AM.bc_am_dd_uri_picker
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents MenuItem2 As System.Windows.Forms.MenuItem
    Friend WithEvents miCompare As System.Windows.Forms.MenuItem
    Friend WithEvents miExport As System.Windows.Forms.MenuItem
    Friend WithEvents miImport As System.Windows.Forms.MenuItem
    Friend WithEvents miLock As System.Windows.Forms.MenuItem
    Friend WithEvents miUnlock As System.Windows.Forms.MenuItem
    Friend WithEvents cbArchiveSource As System.Windows.Forms.CheckBox
    Friend WithEvents cbUpdateDestination As System.Windows.Forms.CheckBox
    Friend WithEvents cbOverwriteAll As System.Windows.Forms.CheckBox
    Friend WithEvents uxBanner As System.Windows.Forms.PictureBox
    Friend WithEvents badupError As BlueCurve.Deployment.AM.bc_am_dd_uri_picker
    Friend WithEvents Label2 As System.Windows.Forms.Label
End Class
