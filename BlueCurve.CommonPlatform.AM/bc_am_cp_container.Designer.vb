<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bc_am_cp_container
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_cp_container))
        Dim ListViewItem6 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem("Taxonomy", 0)
        Dim ListViewItem7 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem("Users", 1)
        Dim ListViewItem8 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem("Publications", 2)
        Dim ListViewItem9 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem("Translation", 3)
        Dim ListViewItem10 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem("Monitoring", "activity_monitor.png")
        Me.uxMainMenu = New System.Windows.Forms.MenuStrip()
        Me.uxFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.SetSynchronizeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SetSynchroinzeAllToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SetSynchronizeFilesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SetSynchronizeOPublicatiosToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SetSynchronizeEntitiesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.uxExit = New System.Windows.Forms.ToolStripMenuItem()
        Me.uxView = New System.Windows.Forms.ToolStripMenuItem()
        Me.uxTaxonomy = New System.Windows.Forms.ToolStripMenuItem()
        Me.uxUsers = New System.Windows.Forms.ToolStripMenuItem()
        Me.uxPubTypes = New System.Windows.Forms.ToolStripMenuItem()
        Me.SchedulerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ShowToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.uxHelp = New System.Windows.Forms.ToolStripMenuItem()
        Me.uxAbout = New System.Windows.Forms.ToolStripMenuItem()
        Me.uxFooterPanel = New System.Windows.Forms.Panel()
        Me.uxConnection = New System.Windows.Forms.Label()
        Me.uxUser = New System.Windows.Forms.Label()
        Me.uxFooterLogo = New System.Windows.Forms.PictureBox()
        Me.uxHeaderPanel = New System.Windows.Forms.Panel()
        Me.uxHeaderLogo = New System.Windows.Forms.PictureBox()
        Me.uxBanner = New System.Windows.Forms.PictureBox()
        Me.uxViewToolStrip = New System.Windows.Forms.ToolStrip()
        Me.uxData = New System.Windows.Forms.ToolStripButton()
        Me.uxStructure = New System.Windows.Forms.ToolStripButton()
        Me.uxNavImageList = New System.Windows.Forms.ImageList()
        Me.uxGenericToolStrip = New System.Windows.Forms.ToolStrip()
        Me.uxNavBar = New System.Windows.Forms.ListView()
        Me.uxMainPanel = New System.Windows.Forms.Panel()
        Me.uxToolStripImageList = New System.Windows.Forms.ImageList()
        Me.uxTranslate = New System.Windows.Forms.ToolStripMenuItem()
        Me.HelpProvider1 = New System.Windows.Forms.HelpProvider()
        Me.ExitSignOutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.uxMainMenu.SuspendLayout()
        Me.uxFooterPanel.SuspendLayout()
        CType(Me.uxFooterLogo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.uxHeaderPanel.SuspendLayout()
        CType(Me.uxHeaderLogo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxBanner, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.uxViewToolStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        'uxMainMenu
        '
        Me.uxMainMenu.BackColor = System.Drawing.Color.White
        Me.uxMainMenu.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxMainMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.uxFile, Me.uxView, Me.SchedulerToolStripMenuItem, Me.uxHelp})
        Me.uxMainMenu.Location = New System.Drawing.Point(0, 0)
        Me.uxMainMenu.Name = "uxMainMenu"
        Me.uxMainMenu.Padding = New System.Windows.Forms.Padding(7, 2, 0, 2)
        Me.uxMainMenu.Size = New System.Drawing.Size(1016, 24)
        Me.uxMainMenu.TabIndex = 0
        '
        'uxFile
        '
        Me.uxFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SetSynchronizeToolStripMenuItem, Me.SetSynchroinzeAllToolStripMenuItem, Me.SetSynchronizeFilesToolStripMenuItem, Me.SetSynchronizeOPublicatiosToolStripMenuItem, Me.SetSynchronizeEntitiesToolStripMenuItem, Me.ToolStripSeparator1, Me.uxExit, Me.ExitSignOutToolStripMenuItem})
        Me.uxFile.Name = "uxFile"
        Me.uxFile.Size = New System.Drawing.Size(38, 20)
        Me.uxFile.Text = "File"
        '
        'SetSynchronizeToolStripMenuItem
        '
        Me.SetSynchronizeToolStripMenuItem.Name = "SetSynchronizeToolStripMenuItem"
        Me.SetSynchronizeToolStripMenuItem.Size = New System.Drawing.Size(238, 22)
        Me.SetSynchronizeToolStripMenuItem.Text = "Set Synchronize"
        '
        'SetSynchroinzeAllToolStripMenuItem
        '
        Me.SetSynchroinzeAllToolStripMenuItem.DoubleClickEnabled = True
        Me.SetSynchroinzeAllToolStripMenuItem.Name = "SetSynchroinzeAllToolStripMenuItem"
        Me.SetSynchroinzeAllToolStripMenuItem.Size = New System.Drawing.Size(238, 22)
        Me.SetSynchroinzeAllToolStripMenuItem.Text = "Set Synchronize All"
        '
        'SetSynchronizeFilesToolStripMenuItem
        '
        Me.SetSynchronizeFilesToolStripMenuItem.Name = "SetSynchronizeFilesToolStripMenuItem"
        Me.SetSynchronizeFilesToolStripMenuItem.Size = New System.Drawing.Size(238, 22)
        Me.SetSynchronizeFilesToolStripMenuItem.Text = "Set Synchronize Templates"
        '
        'SetSynchronizeOPublicatiosToolStripMenuItem
        '
        Me.SetSynchronizeOPublicatiosToolStripMenuItem.Name = "SetSynchronizeOPublicatiosToolStripMenuItem"
        Me.SetSynchronizeOPublicatiosToolStripMenuItem.Size = New System.Drawing.Size(238, 22)
        Me.SetSynchronizeOPublicatiosToolStripMenuItem.Text = "Set Synchronize Publications"
        '
        'SetSynchronizeEntitiesToolStripMenuItem
        '
        Me.SetSynchronizeEntitiesToolStripMenuItem.Name = "SetSynchronizeEntitiesToolStripMenuItem"
        Me.SetSynchronizeEntitiesToolStripMenuItem.Size = New System.Drawing.Size(238, 22)
        Me.SetSynchronizeEntitiesToolStripMenuItem.Text = "Set Synchronize Entities"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(235, 6)
        '
        'uxExit
        '
        Me.uxExit.Name = "uxExit"
        Me.uxExit.Size = New System.Drawing.Size(238, 22)
        Me.uxExit.Text = "Exit"
        '
        'uxView
        '
        Me.uxView.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.uxTaxonomy, Me.uxUsers, Me.uxPubTypes})
        Me.uxView.Name = "uxView"
        Me.uxView.Size = New System.Drawing.Size(46, 20)
        Me.uxView.Text = "View"
        '
        'uxTaxonomy
        '
        Me.uxTaxonomy.Image = CType(resources.GetObject("uxTaxonomy.Image"), System.Drawing.Image)
        Me.uxTaxonomy.Name = "uxTaxonomy"
        Me.uxTaxonomy.Size = New System.Drawing.Size(173, 22)
        Me.uxTaxonomy.Text = "Taxonomy"
        Me.uxTaxonomy.Visible = False
        '
        'uxUsers
        '
        Me.uxUsers.Image = CType(resources.GetObject("uxUsers.Image"), System.Drawing.Image)
        Me.uxUsers.Name = "uxUsers"
        Me.uxUsers.Size = New System.Drawing.Size(173, 22)
        Me.uxUsers.Text = "Users"
        Me.uxUsers.Visible = False
        '
        'uxPubTypes
        '
        Me.uxPubTypes.Image = CType(resources.GetObject("uxPubTypes.Image"), System.Drawing.Image)
        Me.uxPubTypes.Name = "uxPubTypes"
        Me.uxPubTypes.Size = New System.Drawing.Size(173, 22)
        Me.uxPubTypes.Text = "Publication Types"
        Me.uxPubTypes.Visible = False
        '
        'SchedulerToolStripMenuItem
        '
        Me.SchedulerToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ShowToolStripMenuItem})
        Me.SchedulerToolStripMenuItem.Name = "SchedulerToolStripMenuItem"
        Me.SchedulerToolStripMenuItem.Size = New System.Drawing.Size(76, 20)
        Me.SchedulerToolStripMenuItem.Text = "Scheduler"
        Me.SchedulerToolStripMenuItem.Visible = False
        '
        'ShowToolStripMenuItem
        '
        Me.ShowToolStripMenuItem.Name = "ShowToolStripMenuItem"
        Me.ShowToolStripMenuItem.Size = New System.Drawing.Size(105, 22)
        Me.ShowToolStripMenuItem.Text = "Show"
        '
        'uxHelp
        '
        Me.uxHelp.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.uxAbout})
        Me.uxHelp.Name = "uxHelp"
        Me.uxHelp.Size = New System.Drawing.Size(44, 20)
        Me.uxHelp.Text = "Help"
        '
        'uxAbout
        '
        Me.uxAbout.Name = "uxAbout"
        Me.uxAbout.Size = New System.Drawing.Size(107, 22)
        Me.uxAbout.Text = "About"
        '
        'uxFooterPanel
        '
        Me.uxFooterPanel.BackColor = System.Drawing.Color.White
        Me.uxFooterPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.uxFooterPanel.Controls.Add(Me.uxConnection)
        Me.uxFooterPanel.Controls.Add(Me.uxUser)
        Me.uxFooterPanel.Controls.Add(Me.uxFooterLogo)
        Me.uxFooterPanel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.uxFooterPanel.Location = New System.Drawing.Point(0, 656)
        Me.uxFooterPanel.Name = "uxFooterPanel"
        Me.uxFooterPanel.Size = New System.Drawing.Size(1016, 60)
        Me.uxFooterPanel.TabIndex = 7
        '
        'uxConnection
        '
        Me.uxConnection.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxConnection.Location = New System.Drawing.Point(303, 30)
        Me.uxConnection.Name = "uxConnection"
        Me.uxConnection.Size = New System.Drawing.Size(703, 22)
        Me.uxConnection.TabIndex = 33
        Me.uxConnection.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'uxUser
        '
        Me.uxUser.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxUser.Location = New System.Drawing.Point(303, 8)
        Me.uxUser.Name = "uxUser"
        Me.uxUser.Size = New System.Drawing.Size(703, 22)
        Me.uxUser.TabIndex = 29
        Me.uxUser.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'uxFooterLogo
        '
        Me.uxFooterLogo.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.uxFooterLogo.BackColor = System.Drawing.Color.White
        Me.uxFooterLogo.Image = CType(resources.GetObject("uxFooterLogo.Image"), System.Drawing.Image)
        Me.uxFooterLogo.Location = New System.Drawing.Point(3, 20)
        Me.uxFooterLogo.Name = "uxFooterLogo"
        Me.uxFooterLogo.Size = New System.Drawing.Size(280, 32)
        Me.uxFooterLogo.TabIndex = 24
        Me.uxFooterLogo.TabStop = False
        '
        'uxHeaderPanel
        '
        Me.uxHeaderPanel.Controls.Add(Me.uxHeaderLogo)
        Me.uxHeaderPanel.Controls.Add(Me.uxBanner)
        Me.uxHeaderPanel.Dock = System.Windows.Forms.DockStyle.Top
        Me.uxHeaderPanel.Location = New System.Drawing.Point(0, 24)
        Me.uxHeaderPanel.Name = "uxHeaderPanel"
        Me.uxHeaderPanel.Size = New System.Drawing.Size(1016, 64)
        Me.uxHeaderPanel.TabIndex = 35
        '
        'uxHeaderLogo
        '
        Me.uxHeaderLogo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxHeaderLogo.Image = CType(resources.GetObject("uxHeaderLogo.Image"), System.Drawing.Image)
        Me.uxHeaderLogo.Location = New System.Drawing.Point(952, 0)
        Me.uxHeaderLogo.Name = "uxHeaderLogo"
        Me.uxHeaderLogo.Size = New System.Drawing.Size(64, 64)
        Me.uxHeaderLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.uxHeaderLogo.TabIndex = 7
        Me.uxHeaderLogo.TabStop = False
        '
        'uxBanner
        '
        Me.uxBanner.Image = CType(resources.GetObject("uxBanner.Image"), System.Drawing.Image)
        Me.uxBanner.Location = New System.Drawing.Point(0, 0)
        Me.uxBanner.Name = "uxBanner"
        Me.uxBanner.Size = New System.Drawing.Size(436, 64)
        Me.uxBanner.TabIndex = 5
        Me.uxBanner.TabStop = False
        '
        'uxViewToolStrip
        '
        Me.uxViewToolStrip.BackColor = System.Drawing.Color.White
        Me.uxViewToolStrip.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxViewToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.uxViewToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.uxData, Me.uxStructure})
        Me.uxViewToolStrip.Location = New System.Drawing.Point(0, 88)
        Me.uxViewToolStrip.Name = "uxViewToolStrip"
        Me.uxViewToolStrip.Size = New System.Drawing.Size(1016, 25)
        Me.uxViewToolStrip.TabIndex = 36
        '
        'uxData
        '
        Me.uxData.Checked = True
        Me.uxData.CheckOnClick = True
        Me.uxData.CheckState = System.Windows.Forms.CheckState.Checked
        Me.uxData.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.uxData.Image = CType(resources.GetObject("uxData.Image"), System.Drawing.Image)
        Me.uxData.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.uxData.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.uxData.Name = "uxData"
        Me.uxData.Size = New System.Drawing.Size(38, 22)
        Me.uxData.Text = "Data"
        '
        'uxStructure
        '
        Me.uxStructure.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.uxStructure.Image = CType(resources.GetObject("uxStructure.Image"), System.Drawing.Image)
        Me.uxStructure.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.uxStructure.Name = "uxStructure"
        Me.uxStructure.Size = New System.Drawing.Size(64, 22)
        Me.uxStructure.Text = "Structure"
        '
        'uxNavImageList
        '
        Me.uxNavImageList.ImageStream = CType(resources.GetObject("uxNavImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.uxNavImageList.TransparentColor = System.Drawing.Color.Transparent
        Me.uxNavImageList.Images.SetKeyName(0, "uxTaxonomyIMG")
        Me.uxNavImageList.Images.SetKeyName(1, "uxUsersIMG")
        Me.uxNavImageList.Images.SetKeyName(2, "uxPublicationsIMG")
        Me.uxNavImageList.Images.SetKeyName(3, "Translation_icon.png")
        Me.uxNavImageList.Images.SetKeyName(4, "activity_monitor.png")
        '
        'uxGenericToolStrip
        '
        Me.uxGenericToolStrip.BackColor = System.Drawing.Color.White
        Me.uxGenericToolStrip.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxGenericToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.uxGenericToolStrip.Location = New System.Drawing.Point(0, 113)
        Me.uxGenericToolStrip.MinimumSize = New System.Drawing.Size(0, 36)
        Me.uxGenericToolStrip.Name = "uxGenericToolStrip"
        Me.uxGenericToolStrip.Size = New System.Drawing.Size(1016, 36)
        Me.uxGenericToolStrip.TabIndex = 39
        '
        'uxNavBar
        '
        Me.uxNavBar.Dock = System.Windows.Forms.DockStyle.Left
        Me.uxNavBar.HideSelection = False
        ListViewItem6.ToolTipText = "Manage the taxonomy"
        ListViewItem7.ToolTipText = "Manage users"
        ListViewItem8.ToolTipText = "Manage the Publications"
        ListViewItem9.ToolTipText = "Manage the Publications"
        Me.uxNavBar.Items.AddRange(New System.Windows.Forms.ListViewItem() {ListViewItem6, ListViewItem7, ListViewItem8, ListViewItem9, ListViewItem10})
        Me.uxNavBar.LargeImageList = Me.uxNavImageList
        Me.uxNavBar.Location = New System.Drawing.Point(0, 149)
        Me.uxNavBar.MultiSelect = False
        Me.uxNavBar.Name = "uxNavBar"
        Me.uxNavBar.Size = New System.Drawing.Size(105, 507)
        Me.uxNavBar.TabIndex = 41
        Me.uxNavBar.UseCompatibleStateImageBehavior = False
        '
        'uxMainPanel
        '
        Me.uxMainPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.uxMainPanel.Location = New System.Drawing.Point(105, 149)
        Me.uxMainPanel.Name = "uxMainPanel"
        Me.uxMainPanel.Size = New System.Drawing.Size(911, 507)
        Me.uxMainPanel.TabIndex = 42
        '
        'uxToolStripImageList
        '
        Me.uxToolStripImageList.ImageStream = CType(resources.GetObject("uxToolStripImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.uxToolStripImageList.TransparentColor = System.Drawing.Color.Transparent
        Me.uxToolStripImageList.Images.SetKeyName(0, "New")
        Me.uxToolStripImageList.Images.SetKeyName(1, "Edit")
        Me.uxToolStripImageList.Images.SetKeyName(2, "Delete")
        Me.uxToolStripImageList.Images.SetKeyName(3, "Activate")
        Me.uxToolStripImageList.Images.SetKeyName(4, "data")
        Me.uxToolStripImageList.Images.SetKeyName(5, "structure")
        Me.uxToolStripImageList.Images.SetKeyName(6, "none.ico")
        Me.uxToolStripImageList.Images.SetKeyName(7, "association")
        Me.uxToolStripImageList.Images.SetKeyName(8, "navigate")
        '
        'uxTranslate
        '
        Me.uxTranslate.Name = "uxTranslate"
        Me.uxTranslate.Size = New System.Drawing.Size(32, 19)
        '
        'ExitSignOutToolStripMenuItem
        '
        Me.ExitSignOutToolStripMenuItem.Name = "ExitSignOutToolStripMenuItem"
        Me.ExitSignOutToolStripMenuItem.Size = New System.Drawing.Size(238, 22)
        Me.ExitSignOutToolStripMenuItem.Text = "Exit and Sign Out"
        '
        'bc_am_cp_container
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(1016, 716)
        Me.Controls.Add(Me.uxMainPanel)
        Me.Controls.Add(Me.uxNavBar)
        Me.Controls.Add(Me.uxGenericToolStrip)
        Me.Controls.Add(Me.uxViewToolStrip)
        Me.Controls.Add(Me.uxHeaderPanel)
        Me.Controls.Add(Me.uxFooterPanel)
        Me.Controls.Add(Me.uxMainMenu)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.uxMainMenu
        Me.Name = "bc_am_cp_container"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Common Platform - Blue Curve"
        Me.uxMainMenu.ResumeLayout(False)
        Me.uxMainMenu.PerformLayout()
        Me.uxFooterPanel.ResumeLayout(False)
        CType(Me.uxFooterLogo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.uxHeaderPanel.ResumeLayout(False)
        CType(Me.uxHeaderLogo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxBanner, System.ComponentModel.ISupportInitialize).EndInit()
        Me.uxViewToolStrip.ResumeLayout(False)
        Me.uxViewToolStrip.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents uxMainMenu As System.Windows.Forms.MenuStrip
    Friend WithEvents uxFile As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents uxExit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents uxHelp As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents uxAbout As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents uxView As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents uxTaxonomy As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents uxUsers As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents uxPubTypes As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents uxTranslate As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents uxFooterPanel As System.Windows.Forms.Panel
    Public WithEvents uxConnection As System.Windows.Forms.Label
    Public WithEvents uxUser As System.Windows.Forms.Label
    Friend WithEvents uxFooterLogo As System.Windows.Forms.PictureBox
    Friend WithEvents uxHeaderPanel As System.Windows.Forms.Panel
    Friend WithEvents uxBanner As System.Windows.Forms.PictureBox
    Friend WithEvents uxViewToolStrip As System.Windows.Forms.ToolStrip
    Friend WithEvents uxNavImageList As System.Windows.Forms.ImageList
    Friend WithEvents uxStructure As System.Windows.Forms.ToolStripButton
    Friend WithEvents uxNavBar As System.Windows.Forms.ListView
    Friend WithEvents uxMainPanel As System.Windows.Forms.Panel
    Protected Friend WithEvents uxData As System.Windows.Forms.ToolStripButton
    Friend WithEvents uxGenericToolStrip As System.Windows.Forms.ToolStrip
    Friend WithEvents uxToolStripImageList As System.Windows.Forms.ImageList
    Friend WithEvents uxHeaderLogo As System.Windows.Forms.PictureBox
    Friend WithEvents HelpProvider1 As System.Windows.Forms.HelpProvider
    Friend WithEvents SetSynchronizeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SetSynchroinzeAllToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SetSynchronizeFilesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SetSynchronizeOPublicatiosToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SetSynchronizeEntitiesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents SchedulerToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ShowToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitSignOutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
