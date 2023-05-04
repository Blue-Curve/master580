Imports BlueCurve.Core.AS
Imports BlueCurve.Core.CS
Imports System.Windows.Forms
Public Class bc_am_bp_main
    Inherits System.Windows.Forms.Form

    Private ctrllr As bc_am_BluePrint
    Public lvwColumnSorter As ListViewColumnSorter

    ' nav bar icon constants
    Private Const templates_icon As Integer = 0
    Private Const components_icon As Integer = 1
    Private Const publicationTypes_icon As Integer = 2
    Private Const composites_icon As Integer = 3
    Friend WithEvents uxcreate As System.Windows.Forms.RadioButton
    Friend WithEvents uxemail As System.Windows.Forms.RadioButton
    Friend WithEvents MenuItem1 As System.Windows.Forms.MenuItem
    Private Const data_def_icon As Integer = 4

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        ' Create an instance of a ListView column sorter and assign it 
        ' to the ListView control.
        lvwColumnSorter = New ListViewColumnSorter
        uxGenericList.ListViewItemSorter = lvwColumnSorter

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
    Friend WithEvents uxFooterLogo As System.Windows.Forms.PictureBox
    Friend WithEvents uxFooterPanel As System.Windows.Forms.Panel
    Friend WithEvents uxHeaderPanel As System.Windows.Forms.Panel
    Friend WithEvents uxHeaderLogo As System.Windows.Forms.PictureBox
    Friend WithEvents uxBanner As System.Windows.Forms.PictureBox
    Friend WithEvents uxToolBarMain As System.Windows.Forms.ToolBar
    Friend WithEvents uxNavBarImages As System.Windows.Forms.ImageList
    Friend WithEvents uxToolBarImages As System.Windows.Forms.ImageList
    Friend WithEvents uxGenericListImages As System.Windows.Forms.ImageList
    Friend WithEvents uxOpenFileDlg As System.Windows.Forms.OpenFileDialog
    Friend WithEvents uxSaveFileDlg As System.Windows.Forms.SaveFileDialog
    Friend WithEvents uxNavBar As System.Windows.Forms.ListView
    Friend WithEvents uxDescription As System.Windows.Forms.ColumnHeader
    Friend WithEvents uxMainMenu As System.Windows.Forms.MainMenu
    Friend WithEvents uxFile As System.Windows.Forms.MenuItem
    Friend WithEvents uxEdit As System.Windows.Forms.MenuItem
    Friend WithEvents uxConfiguration As System.Windows.Forms.MenuItem
    Friend WithEvents uxHelp As System.Windows.Forms.MenuItem
    Friend WithEvents uxAbout As System.Windows.Forms.MenuItem
    Friend WithEvents uxExit As System.Windows.Forms.MenuItem
    Public WithEvents uxUser As System.Windows.Forms.Label
    Public WithEvents uxRole As System.Windows.Forms.Label
    Public WithEvents uxServer As System.Windows.Forms.Label
    Public WithEvents uxGenericList As System.Windows.Forms.ListView
    Friend WithEvents uxSync As System.Windows.Forms.MenuItem
    Friend WithEvents uxFolderBrowserDialog As System.Windows.Forms.FolderBrowserDialog
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_bp_main))
        Dim ListViewItem1 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem("Templates", 0)
        Dim ListViewItem2 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem("Components", 1)
        Dim ListViewItem3 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem("Publications", 2)
        Dim ListViewItem4 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem("Composites", 3)
        Dim ListViewItem5 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem("Data Definitions", 4)
        Me.uxFooterPanel = New System.Windows.Forms.Panel()
        Me.uxServer = New System.Windows.Forms.Label()
        Me.uxRole = New System.Windows.Forms.Label()
        Me.uxUser = New System.Windows.Forms.Label()
        Me.uxFooterLogo = New System.Windows.Forms.PictureBox()
        Me.uxHeaderPanel = New System.Windows.Forms.Panel()
        Me.uxemail = New System.Windows.Forms.RadioButton()
        Me.uxHeaderLogo = New System.Windows.Forms.PictureBox()
        Me.uxcreate = New System.Windows.Forms.RadioButton()
        Me.uxBanner = New System.Windows.Forms.PictureBox()
        Me.uxNavBarImages = New System.Windows.Forms.ImageList()
        Me.uxToolBarMain = New System.Windows.Forms.ToolBar()
        Me.uxToolBarImages = New System.Windows.Forms.ImageList()
        Me.uxGenericListImages = New System.Windows.Forms.ImageList()
        Me.uxOpenFileDlg = New System.Windows.Forms.OpenFileDialog()
        Me.uxSaveFileDlg = New System.Windows.Forms.SaveFileDialog()
        Me.uxNavBar = New System.Windows.Forms.ListView()
        Me.uxGenericList = New System.Windows.Forms.ListView()
        Me.uxDescription = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.uxMainMenu = New System.Windows.Forms.MainMenu()
        Me.uxFile = New System.Windows.Forms.MenuItem()
        Me.uxSync = New System.Windows.Forms.MenuItem()
        Me.uxExit = New System.Windows.Forms.MenuItem()
        Me.uxEdit = New System.Windows.Forms.MenuItem()
        Me.uxConfiguration = New System.Windows.Forms.MenuItem()
        Me.uxHelp = New System.Windows.Forms.MenuItem()
        Me.uxAbout = New System.Windows.Forms.MenuItem()
        Me.uxFolderBrowserDialog = New System.Windows.Forms.FolderBrowserDialog()
        Me.MenuItem1 = New System.Windows.Forms.MenuItem()
        Me.uxFooterPanel.SuspendLayout()
        CType(Me.uxFooterLogo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.uxHeaderPanel.SuspendLayout()
        CType(Me.uxHeaderLogo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxBanner, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.uxFooterPanel.Location = New System.Drawing.Point(0, 432)
        Me.uxFooterPanel.Name = "uxFooterPanel"
        Me.uxFooterPanel.Size = New System.Drawing.Size(702, 60)
        Me.uxFooterPanel.TabIndex = 5
        '
        'uxServer
        '
        Me.uxServer.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxServer.Location = New System.Drawing.Point(294, 34)
        Me.uxServer.Name = "uxServer"
        Me.uxServer.Size = New System.Drawing.Size(396, 16)
        Me.uxServer.TabIndex = 33
        Me.uxServer.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'uxRole
        '
        Me.uxRole.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxRole.Location = New System.Drawing.Point(500, 12)
        Me.uxRole.Name = "uxRole"
        Me.uxRole.Size = New System.Drawing.Size(192, 16)
        Me.uxRole.TabIndex = 30
        Me.uxRole.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'uxUser
        '
        Me.uxUser.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.uxUser.Location = New System.Drawing.Point(293, 12)
        Me.uxUser.Name = "uxUser"
        Me.uxUser.Size = New System.Drawing.Size(192, 16)
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
        Me.uxHeaderPanel.Controls.Add(Me.uxemail)
        Me.uxHeaderPanel.Controls.Add(Me.uxHeaderLogo)
        Me.uxHeaderPanel.Controls.Add(Me.uxcreate)
        Me.uxHeaderPanel.Controls.Add(Me.uxBanner)
        Me.uxHeaderPanel.Dock = System.Windows.Forms.DockStyle.Top
        Me.uxHeaderPanel.Location = New System.Drawing.Point(0, 0)
        Me.uxHeaderPanel.Name = "uxHeaderPanel"
        Me.uxHeaderPanel.Size = New System.Drawing.Size(702, 64)
        Me.uxHeaderPanel.TabIndex = 6
        '
        'uxemail
        '
        Me.uxemail.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxemail.AutoSize = True
        Me.uxemail.Location = New System.Drawing.Point(529, 44)
        Me.uxemail.Name = "uxemail"
        Me.uxemail.Size = New System.Drawing.Size(56, 17)
        Me.uxemail.TabIndex = 14
        Me.uxemail.Text = "Email"
        Me.uxemail.UseVisualStyleBackColor = True
        Me.uxemail.Visible = False
        '
        'uxHeaderLogo
        '
        Me.uxHeaderLogo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxHeaderLogo.Image = CType(resources.GetObject("uxHeaderLogo.Image"), System.Drawing.Image)
        Me.uxHeaderLogo.Location = New System.Drawing.Point(638, 0)
        Me.uxHeaderLogo.Name = "uxHeaderLogo"
        Me.uxHeaderLogo.Size = New System.Drawing.Size(64, 64)
        Me.uxHeaderLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.uxHeaderLogo.TabIndex = 6
        Me.uxHeaderLogo.TabStop = False
        '
        'uxcreate
        '
        Me.uxcreate.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxcreate.AutoSize = True
        Me.uxcreate.Checked = True
        Me.uxcreate.Location = New System.Drawing.Point(442, 44)
        Me.uxcreate.Name = "uxcreate"
        Me.uxcreate.Size = New System.Drawing.Size(64, 17)
        Me.uxcreate.TabIndex = 13
        Me.uxcreate.TabStop = True
        Me.uxcreate.Text = "Create"
        Me.uxcreate.UseVisualStyleBackColor = True
        Me.uxcreate.Visible = False
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
        'uxNavBarImages
        '
        Me.uxNavBarImages.ImageStream = CType(resources.GetObject("uxNavBarImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.uxNavBarImages.TransparentColor = System.Drawing.Color.Transparent
        Me.uxNavBarImages.Images.SetKeyName(0, "")
        Me.uxNavBarImages.Images.SetKeyName(1, "")
        Me.uxNavBarImages.Images.SetKeyName(2, "")
        Me.uxNavBarImages.Images.SetKeyName(3, "")
        Me.uxNavBarImages.Images.SetKeyName(4, "")
        '
        'uxToolBarMain
        '
        Me.uxToolBarMain.Appearance = System.Windows.Forms.ToolBarAppearance.Flat
        Me.uxToolBarMain.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.uxToolBarMain.ButtonSize = New System.Drawing.Size(16, 16)
        Me.uxToolBarMain.DropDownArrows = True
        Me.uxToolBarMain.ImageList = Me.uxToolBarImages
        Me.uxToolBarMain.Location = New System.Drawing.Point(0, 64)
        Me.uxToolBarMain.Name = "uxToolBarMain"
        Me.uxToolBarMain.ShowToolTips = True
        Me.uxToolBarMain.Size = New System.Drawing.Size(702, 24)
        Me.uxToolBarMain.TabIndex = 9
        '
        'uxToolBarImages
        '
        Me.uxToolBarImages.ImageStream = CType(resources.GetObject("uxToolBarImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.uxToolBarImages.TransparentColor = System.Drawing.Color.Transparent
        Me.uxToolBarImages.Images.SetKeyName(0, "")
        Me.uxToolBarImages.Images.SetKeyName(1, "")
        Me.uxToolBarImages.Images.SetKeyName(2, "")
        Me.uxToolBarImages.Images.SetKeyName(3, "")
        Me.uxToolBarImages.Images.SetKeyName(4, "")
        Me.uxToolBarImages.Images.SetKeyName(5, "")
        Me.uxToolBarImages.Images.SetKeyName(6, "")
        Me.uxToolBarImages.Images.SetKeyName(7, "")
        Me.uxToolBarImages.Images.SetKeyName(8, "")
        Me.uxToolBarImages.Images.SetKeyName(9, "")
        '
        'uxGenericListImages
        '
        Me.uxGenericListImages.ImageStream = CType(resources.GetObject("uxGenericListImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.uxGenericListImages.TransparentColor = System.Drawing.Color.Transparent
        Me.uxGenericListImages.Images.SetKeyName(0, "")
        Me.uxGenericListImages.Images.SetKeyName(1, "")
        Me.uxGenericListImages.Images.SetKeyName(2, "")
        Me.uxGenericListImages.Images.SetKeyName(3, "")
        Me.uxGenericListImages.Images.SetKeyName(4, "")
        '
        'uxOpenFileDlg
        '
        Me.uxOpenFileDlg.Filter = "Word Templates (*.dot)|*.dot|Powerpoint Templates (*.pot)|*.pot"
        Me.uxOpenFileDlg.Title = "Clone"
        '
        'uxSaveFileDlg
        '
        Me.uxSaveFileDlg.Filter = "Word Templates (*.dot)|*.dot|Powerpoint Templates (*.pot)|*.pot"
        Me.uxSaveFileDlg.Title = "Create New"
        '
        'uxNavBar
        '
        Me.uxNavBar.BackColor = System.Drawing.SystemColors.Window
        Me.uxNavBar.Dock = System.Windows.Forms.DockStyle.Left
        Me.uxNavBar.HideSelection = False
        Me.uxNavBar.Items.AddRange(New System.Windows.Forms.ListViewItem() {ListViewItem1, ListViewItem2, ListViewItem3, ListViewItem4, ListViewItem5})
        Me.uxNavBar.LargeImageList = Me.uxNavBarImages
        Me.uxNavBar.Location = New System.Drawing.Point(0, 88)
        Me.uxNavBar.MultiSelect = False
        Me.uxNavBar.Name = "uxNavBar"
        Me.uxNavBar.Size = New System.Drawing.Size(85, 344)
        Me.uxNavBar.TabIndex = 11
        Me.uxNavBar.UseCompatibleStateImageBehavior = False
        '
        'uxGenericList
        '
        Me.uxGenericList.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.uxDescription})
        Me.uxGenericList.Dock = System.Windows.Forms.DockStyle.Fill
        Me.uxGenericList.FullRowSelect = True
        Me.uxGenericList.GridLines = True
        Me.uxGenericList.HideSelection = False
        Me.uxGenericList.Location = New System.Drawing.Point(85, 88)
        Me.uxGenericList.MultiSelect = False
        Me.uxGenericList.Name = "uxGenericList"
        Me.uxGenericList.Size = New System.Drawing.Size(617, 344)
        Me.uxGenericList.SmallImageList = Me.uxGenericListImages
        Me.uxGenericList.TabIndex = 12
        Me.uxGenericList.UseCompatibleStateImageBehavior = False
        Me.uxGenericList.View = System.Windows.Forms.View.Details
        '
        'uxDescription
        '
        Me.uxDescription.Text = "Description"
        Me.uxDescription.Width = 589
        '
        'uxMainMenu
        '
        Me.uxMainMenu.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.uxFile, Me.uxEdit, Me.uxHelp})
        '
        'uxFile
        '
        Me.uxFile.Index = 0
        Me.uxFile.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.uxSync, Me.uxExit, Me.MenuItem1})
        Me.uxFile.Text = "&File"
        '
        'uxSync
        '
        Me.uxSync.Index = 0
        Me.uxSync.Text = "&Synchronize"
        '
        'uxExit
        '
        Me.uxExit.Index = 1
        Me.uxExit.Text = "E&xit"
        '
        'uxEdit
        '
        Me.uxEdit.Index = 1
        Me.uxEdit.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.uxConfiguration})
        Me.uxEdit.Text = "&Edit"
        '
        'uxConfiguration
        '
        Me.uxConfiguration.Index = 0
        Me.uxConfiguration.Text = "Confi&guration"
        '
        'uxHelp
        '
        Me.uxHelp.Index = 2
        Me.uxHelp.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.uxAbout})
        Me.uxHelp.Text = "&Help"
        '
        'uxAbout
        '
        Me.uxAbout.Index = 0
        Me.uxAbout.Text = "&About"
        '
        'MenuItem1
        '
        Me.MenuItem1.Index = 2
        Me.MenuItem1.Text = "Exit and Sign Out"
        '
        'bc_am_bp_main
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(702, 492)
        Me.Controls.Add(Me.uxGenericList)
        Me.Controls.Add(Me.uxNavBar)
        Me.Controls.Add(Me.uxToolBarMain)
        Me.Controls.Add(Me.uxHeaderPanel)
        Me.Controls.Add(Me.uxFooterPanel)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Menu = Me.uxMainMenu
        Me.Name = "bc_am_bp_main"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "BluePrint - Blue Curve"
        Me.uxFooterPanel.ResumeLayout(False)
        CType(Me.uxFooterLogo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.uxHeaderPanel.ResumeLayout(False)
        Me.uxHeaderPanel.PerformLayout()
        CType(Me.uxHeaderLogo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxBanner, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Friend WriteOnly Property Controller() As bc_am_blueprint

        Set(ByVal Value As bc_am_blueprint)
            ctrllr = Value
        End Set

    End Property

    Private Sub uxToolBarMain_ButtonClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles uxToolBarMain.ButtonClick

        ctrllr.ActionToolbar(e.Button)

    End Sub

    Private Sub uxNavBar_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxNavBar.SelectedIndexChanged

        ctrllr.NavigationBarSelection()

    End Sub

    Private Sub bc_am_bp_main_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Resize

        uxGenericList.Columns(0).Width = uxGenericList.Width - 22

        ' stop resizing when a certain size is reached
        If Me.Width < 650 Then
            Me.Width = 650
            Exit Sub
        End If

        If Me.Height < 630 Then
            Me.Height = 630
            Exit Sub
        End If

    End Sub

    Private Sub mnuExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxExit.Click

        Me.Close()

    End Sub

    Private Sub uxGenericList_ColumnClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles uxGenericList.ColumnClick

        ' Determine if the clicked column is already the column that is 
        ' being sorted.
        If (e.Column = lvwColumnSorter.SortColumn) Then
            ' Reverse the current sort direction for this column.
            If (lvwColumnSorter.Order = SortOrder.Ascending) Then
                lvwColumnSorter.Order = SortOrder.Descending
            Else
                lvwColumnSorter.Order = SortOrder.Ascending
            End If
        Else
            ' Set the column number that is to be sorted; default to ascending.
            lvwColumnSorter.SortColumn = e.Column
            lvwColumnSorter.Order = SortOrder.Ascending
        End If

        ' Perform the sort with these new sort options.
        uxGenericList.Sort()

    End Sub

    Private Sub uxConfiguration_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxConfiguration.Click

        Dim configMgmgt As bc_am_configuration
        Dim configView As New bc_am_bp_configuration

        configMgmgt = New bc_am_configuration(configView)
        If configMgmgt.Show() Then
            Dim bc_am_load As New bc_am_load("BluePrint")

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.LOCAL Then
                Dim message As New bc_cs_message("BluePrint - Blue Curve", "No Network Detected BluePrint Cannot be Loaded", bc_cs_message.MESSAGE)
                ' show the configuration dialog again
                uxConfiguration_Click(sender, e)
            Else
                ctrllr.Sync()
                ctrllr.NavigationBarSelection()
            End If

        End If

    End Sub

    Private Sub uxAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxAbout.Click

        Dim about As New bc_am_bp_about

        about.ShowDialog()

    End Sub

    Private Sub uxGenericList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxGenericList.SelectedIndexChanged

        If uxGenericList.SelectedItems.Count = 0 Then
            ctrllr.EnableToolBar(False)
        Else
            ctrllr.EnableToolBar(True, uxGenericList.SelectedItems(0))
        End If

    End Sub

    Private Sub uxGenericList_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxGenericList.DoubleClick

        Select Case uxNavBar.SelectedIndices(0)
            Case templates_icon
                ctrllr.ActionToolbar(uxToolBarMain.Buttons(3)) ' open
            Case components_icon
                ctrllr.ActionToolbar(uxToolBarMain.Buttons(1)) ' amend
            Case publicationTypes_icon
                ctrllr.ActionToolbar(uxToolBarMain.Buttons(1)) ' configure
            Case composites_icon
                ctrllr.ActionToolbar(uxToolBarMain.Buttons(1)) ' configure
            Case data_def_icon
                ctrllr.ActionToolbar(uxToolBarMain.Buttons(1)) ' amend
        End Select

    End Sub

    Private Sub uxSync_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles uxSync.Click

        ctrllr.Sync()

    End Sub

    Private Sub uxGenericList_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles uxGenericList.KeyPress

        If e.KeyChar = Chr(13) Then ' carriage return

            Select Case uxNavBar.SelectedIndices(0)
                Case templates_icon
                    ctrllr.ActionToolbar(uxToolBarMain.Buttons(3)) ' open
                Case components_icon
                    ctrllr.ActionToolbar(uxToolBarMain.Buttons(1)) ' amend
                Case publicationTypes_icon
                    ctrllr.ActionToolbar(uxToolBarMain.Buttons(1)) ' configure
                Case composites_icon
                    ctrllr.ActionToolbar(uxToolBarMain.Buttons(1)) ' configure
                Case data_def_icon
                    ctrllr.ActionToolbar(uxToolBarMain.Buttons(1)) ' amend
            End Select

        End If

    End Sub

    Private Sub uxFooterLogo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxFooterLogo.Click


    End Sub

    Private Sub uxcreate_CheckedChanged(sender As Object, e As EventArgs) Handles uxcreate.CheckedChanged

        Try
            If uxcreate.Checked = True Then
                If ctrllr.components_selected = False Then
                    ctrllr.loadTemplates()
                Else
                    ctrllr.LoadComponents(Me)


                End If
            End If
        Catch ex As Exception
            'MsgBox(ex.Message)

        End Try
    End Sub

    Private Sub uxemail_CheckedChanged(sender As Object, e As EventArgs) Handles uxemail.CheckedChanged
        Try
            If uxemail.Checked = True Then
                If ctrllr.components_selected = False Then
                    ctrllr.loadEmailTemplates()
                Else
                    ctrllr.LoadEmailComponents(Me)
                End If

            End If
        Catch ex As Exception
            'MsgBox(ex.Message)

        End Try
    End Sub

    Private Sub MenuItem1_Click(sender As Object, e As EventArgs) Handles MenuItem1.Click
        Dim cl As New Cbc_dx_full_logon
        cl.remove_credentials_file()

        Me.Close()
    End Sub
End Class
