<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bc_dx_mail_list_generate
    Inherits DevExpress.XtraEditors.XtraForm

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_dx_mail_list_generate))
        Me.PanelControl3 = New DevExpress.XtraEditors.PanelControl()
        Me.bcancel = New DevExpress.XtraEditors.SimpleButton()
        Me.bsave = New DevExpress.XtraEditors.SimpleButton()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.uxputypes = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.LabelControl4 = New DevExpress.XtraEditors.LabelControl()
        Me.tsearch = New DevExpress.XtraEditors.TextEdit()
        Me.uxtaxonomy = New DevExpress.XtraTab.XtraTabControl()
        Me.uxauthor = New DevExpress.XtraTab.XtraTabPage()
        Me.uximage = New DevExpress.Utils.ImageCollection()
        Me.uxDocumentDetailImages = New DevExpress.Utils.ImageCollection()
        Me.mimeimages = New System.Windows.Forms.ImageList()
        Me.LabelControl1 = New DevExpress.XtraEditors.LabelControl()
        Me.searchtimer = New System.Windows.Forms.Timer()
        Me.pclear = New DevExpress.XtraTreeList.TreeList()
        Me.TreeListColumn1 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.TreeListColumn2 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.RepositoryItemDateEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemDateEdit()
        Me.RepositoryItemDateEdit3 = New DevExpress.XtraEditors.Repository.RepositoryItemDateEdit()
        Me.LabelControl2 = New DevExpress.XtraEditors.LabelControl()
        Me.uxentity = New DevExpress.XtraEditors.ListBoxControl()
        Me.lsearch = New DevExpress.XtraEditors.PictureEdit()
        Me.PictureEdit1 = New DevExpress.XtraEditors.PictureEdit()
        CType(Me.PanelControl3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl3.SuspendLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxputypes.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.tsearch.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxtaxonomy, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.uxtaxonomy.SuspendLayout()
        CType(Me.uximage, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxDocumentDetailImages, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pclear, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemDateEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemDateEdit1.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemDateEdit3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemDateEdit3.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxentity, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lsearch.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureEdit1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PanelControl3
        '
        Me.PanelControl3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PanelControl3.Controls.Add(Me.bcancel)
        Me.PanelControl3.Controls.Add(Me.bsave)
        Me.PanelControl3.Controls.Add(Me.PictureBox2)
        Me.PanelControl3.Location = New System.Drawing.Point(1, 440)
        Me.PanelControl3.Name = "PanelControl3"
        Me.PanelControl3.Size = New System.Drawing.Size(511, 50)
        Me.PanelControl3.TabIndex = 4
        '
        'bcancel
        '
        Me.bcancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bcancel.ImageIndex = 4
        Me.bcancel.Location = New System.Drawing.Point(424, 13)
        Me.bcancel.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.bcancel.Name = "bcancel"
        Me.bcancel.Size = New System.Drawing.Size(75, 33)
        Me.bcancel.TabIndex = 86
        Me.bcancel.Text = "Cancel"
        '
        'bsave
        '
        Me.bsave.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bsave.Enabled = False
        Me.bsave.ImageIndex = 4
        Me.bsave.Location = New System.Drawing.Point(343, 10)
        Me.bsave.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.bsave.Name = "bsave"
        Me.bsave.Size = New System.Drawing.Size(75, 33)
        Me.bsave.TabIndex = 85
        Me.bsave.Text = "Generate"
        '
        'PictureBox2
        '
        Me.PictureBox2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(5, 10)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(175, 35)
        Me.PictureBox2.TabIndex = 42
        Me.PictureBox2.TabStop = False
        '
        'uxputypes
        '
        Me.uxputypes.EditValue = ""
        Me.uxputypes.Location = New System.Drawing.Point(21, 31)
        Me.uxputypes.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.uxputypes.Name = "uxputypes"
        Me.uxputypes.Properties.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.[True]
        Me.uxputypes.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.uxputypes.Properties.MaxLength = 250
        Me.uxputypes.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        Me.uxputypes.Size = New System.Drawing.Size(477, 20)
        Me.uxputypes.TabIndex = 63
        '
        'LabelControl4
        '
        Me.LabelControl4.Location = New System.Drawing.Point(22, 14)
        Me.LabelControl4.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.LabelControl4.Name = "LabelControl4"
        Me.LabelControl4.Size = New System.Drawing.Size(176, 13)
        Me.LabelControl4.TabIndex = 64
        Me.LabelControl4.Text = "Generate a mail based on publication"
        '
        'tsearch
        '
        Me.tsearch.EditValue = ""
        Me.tsearch.Location = New System.Drawing.Point(65, 415)
        Me.tsearch.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.tsearch.Name = "tsearch"
        Me.tsearch.Properties.MaxLength = 250
        Me.tsearch.Size = New System.Drawing.Size(386, 20)
        Me.tsearch.TabIndex = 70
        '
        'uxtaxonomy
        '
        Me.uxtaxonomy.Location = New System.Drawing.Point(23, 246)
        Me.uxtaxonomy.Name = "uxtaxonomy"
        Me.uxtaxonomy.SelectedTabPage = Me.uxauthor
        Me.uxtaxonomy.Size = New System.Drawing.Size(477, 24)
        Me.uxtaxonomy.TabIndex = 69
        Me.uxtaxonomy.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.uxauthor})
        '
        'uxauthor
        '
        Me.uxauthor.Name = "uxauthor"
        Me.uxauthor.Size = New System.Drawing.Size(471, 0)
        Me.uxauthor.Text = "Authors"
        '
        'uximage
        '
        Me.uximage.ImageStream = CType(resources.GetObject("uximage.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
        Me.uximage.Images.SetKeyName(0, "down_16.ico")
        Me.uximage.Images.SetKeyName(1, "search_16.ico")
        Me.uximage.Images.SetKeyName(2, "up_16.ico")
        Me.uximage.Images.SetKeyName(3, "read_16.ico")
        '
        'uxDocumentDetailImages
        '
        Me.uxDocumentDetailImages.ImageStream = CType(resources.GetObject("uxDocumentDetailImages.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
        Me.uxDocumentDetailImages.Images.SetKeyName(0, "support_32.png")
        Me.uxDocumentDetailImages.Images.SetKeyName(1, "document_32.png")
        Me.uxDocumentDetailImages.Images.SetKeyName(2, "checkin_in_force_32.ico")
        Me.uxDocumentDetailImages.Images.SetKeyName(3, "checkin_in_to_another_32.ico")
        Me.uxDocumentDetailImages.Images.SetKeyName(4, "checkin_in_to_me_32.ico")
        Me.uxDocumentDetailImages.Images.SetKeyName(5, "comment_32.ico")
        Me.uxDocumentDetailImages.Images.SetKeyName(6, "edit_32.ico")
        Me.uxDocumentDetailImages.Images.SetKeyName(7, "history_32.ico")
        Me.uxDocumentDetailImages.Images.SetKeyName(8, "links_32.ico")
        Me.uxDocumentDetailImages.Images.SetKeyName(9, "locked_32.ico")
        Me.uxDocumentDetailImages.Images.SetKeyName(10, "permissions_32.ico")
        Me.uxDocumentDetailImages.Images.SetKeyName(11, "read_32.ico")
        Me.uxDocumentDetailImages.Images.SetKeyName(12, "user_32.ico")
        Me.uxDocumentDetailImages.Images.SetKeyName(13, "write_32.ico")
        Me.uxDocumentDetailImages.Images.SetKeyName(14, "attach_32.ico")
        Me.uxDocumentDetailImages.Images.SetKeyName(15, "categorise_32.ico")
        Me.uxDocumentDetailImages.Images.SetKeyName(16, "filter_32.ico")
        Me.uxDocumentDetailImages.Images.SetKeyName(17, "icon1_32.ico")
        Me.uxDocumentDetailImages.Images.SetKeyName(18, "icon2_32.ico")
        Me.uxDocumentDetailImages.Images.SetKeyName(19, "icon3_32.ico")
        Me.uxDocumentDetailImages.Images.SetKeyName(20, "icon4_32.ico")
        Me.uxDocumentDetailImages.Images.SetKeyName(21, "icon5_32.ico")
        Me.uxDocumentDetailImages.Images.SetKeyName(22, "icon6_32.ico")
        Me.uxDocumentDetailImages.Images.SetKeyName(23, "import_32.ico")
        Me.uxDocumentDetailImages.Images.SetKeyName(24, "refresh_32.ico")
        Me.uxDocumentDetailImages.Images.SetKeyName(25, "register_32.ico")
        Me.uxDocumentDetailImages.Images.SetKeyName(26, "registered_document_32.ico")
        Me.uxDocumentDetailImages.Images.SetKeyName(27, "settings_32.ico")
        Me.uxDocumentDetailImages.Images.SetKeyName(28, "delete_32.ico")
        Me.uxDocumentDetailImages.Images.SetKeyName(29, "move_32.ico")
        Me.uxDocumentDetailImages.Images.SetKeyName(30, "revert_32.ico")
        Me.uxDocumentDetailImages.Images.SetKeyName(31, "register_32.ico")
        Me.uxDocumentDetailImages.Images.SetKeyName(32, "delete_32.ico")
        Me.uxDocumentDetailImages.Images.SetKeyName(33, "move_32.ico")
        Me.uxDocumentDetailImages.Images.SetKeyName(34, "revert_32.ico")
        Me.uxDocumentDetailImages.Images.SetKeyName(35, "checkedin_IN_32.ico")
        Me.uxDocumentDetailImages.Images.SetKeyName(36, "checkedin_OUT_32.ico")
        Me.uxDocumentDetailImages.Images.SetKeyName(37, "unregistered_60.png")
        '
        'mimeimages
        '
        Me.mimeimages.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit
        Me.mimeimages.ImageSize = New System.Drawing.Size(16, 16)
        Me.mimeimages.TransparentColor = System.Drawing.Color.Transparent
        '
        'LabelControl1
        '
        Me.LabelControl1.Location = New System.Drawing.Point(22, 55)
        Me.LabelControl1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.LabelControl1.Name = "LabelControl1"
        Me.LabelControl1.Size = New System.Drawing.Size(249, 13)
        Me.LabelControl1.TabIndex = 76
        Me.LabelControl1.Text = "Explicitly categorise against (double click to remove)"
        '
        'searchtimer
        '
        Me.searchtimer.Enabled = True
        Me.searchtimer.Interval = 1000
        '
        'pclear
        '
        Me.pclear.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pclear.Columns.AddRange(New DevExpress.XtraTreeList.Columns.TreeListColumn() {Me.TreeListColumn1, Me.TreeListColumn2})
        Me.pclear.Location = New System.Drawing.Point(21, 73)
        Me.pclear.Name = "pclear"
        Me.pclear.OptionsBehavior.Editable = False
        Me.pclear.OptionsView.ShowIndicator = False
        Me.pclear.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemDateEdit1, Me.RepositoryItemDateEdit3})
        Me.pclear.Size = New System.Drawing.Size(477, 150)
        Me.pclear.TabIndex = 78
        Me.pclear.TreeLevelWidth = 12
        '
        'TreeListColumn1
        '
        Me.TreeListColumn1.Caption = "Class"
        Me.TreeListColumn1.FieldName = "class"
        Me.TreeListColumn1.MinWidth = 33
        Me.TreeListColumn1.Name = "TreeListColumn1"
        Me.TreeListColumn1.OptionsColumn.AllowSort = False
        Me.TreeListColumn1.OptionsFilter.AllowFilter = False
        Me.TreeListColumn1.Visible = True
        Me.TreeListColumn1.VisibleIndex = 0
        Me.TreeListColumn1.Width = 116
        '
        'TreeListColumn2
        '
        Me.TreeListColumn2.Caption = "Name"
        Me.TreeListColumn2.FieldName = "Status"
        Me.TreeListColumn2.Name = "TreeListColumn2"
        Me.TreeListColumn2.OptionsColumn.AllowSort = False
        Me.TreeListColumn2.OptionsFilter.AllowFilter = False
        Me.TreeListColumn2.Visible = True
        Me.TreeListColumn2.VisibleIndex = 1
        Me.TreeListColumn2.Width = 359
        '
        'RepositoryItemDateEdit1
        '
        Me.RepositoryItemDateEdit1.AutoHeight = False
        Me.RepositoryItemDateEdit1.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.RepositoryItemDateEdit1.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.RepositoryItemDateEdit1.Name = "RepositoryItemDateEdit1"
        '
        'RepositoryItemDateEdit3
        '
        Me.RepositoryItemDateEdit3.AutoHeight = False
        Me.RepositoryItemDateEdit3.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.RepositoryItemDateEdit3.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.RepositoryItemDateEdit3.Name = "RepositoryItemDateEdit3"
        '
        'LabelControl2
        '
        Me.LabelControl2.Location = New System.Drawing.Point(21, 228)
        Me.LabelControl2.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.LabelControl2.Name = "LabelControl2"
        Me.LabelControl2.Size = New System.Drawing.Size(189, 13)
        Me.LabelControl2.TabIndex = 79
        Me.LabelControl2.Tag = ""
        Me.LabelControl2.Text = "Add categorisation (double click to add)"
        '
        'uxentity
        '
        Me.uxentity.Location = New System.Drawing.Point(21, 271)
        Me.uxentity.Name = "uxentity"
        Me.uxentity.Size = New System.Drawing.Size(476, 126)
        Me.uxentity.TabIndex = 80
        '
        'lsearch
        '
        Me.lsearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lsearch.EditValue = CType(resources.GetObject("lsearch.EditValue"), Object)
        Me.lsearch.Location = New System.Drawing.Point(21, 403)
        Me.lsearch.Name = "lsearch"
        Me.lsearch.Size = New System.Drawing.Size(38, 36)
        Me.lsearch.TabIndex = 108
        '
        'PictureEdit1
        '
        Me.PictureEdit1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PictureEdit1.EditValue = CType(resources.GetObject("PictureEdit1.EditValue"), Object)
        Me.PictureEdit1.Location = New System.Drawing.Point(457, 403)
        Me.PictureEdit1.Name = "PictureEdit1"
        Me.PictureEdit1.Size = New System.Drawing.Size(38, 36)
        Me.PictureEdit1.TabIndex = 110
        '
        'bc_dx_mail_list_generate
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(512, 490)
        Me.Controls.Add(Me.PictureEdit1)
        Me.Controls.Add(Me.lsearch)
        Me.Controls.Add(Me.uxentity)
        Me.Controls.Add(Me.LabelControl2)
        Me.Controls.Add(Me.pclear)
        Me.Controls.Add(Me.LabelControl1)
        Me.Controls.Add(Me.tsearch)
        Me.Controls.Add(Me.uxtaxonomy)
        Me.Controls.Add(Me.uxputypes)
        Me.Controls.Add(Me.LabelControl4)
        Me.Controls.Add(Me.PanelControl3)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "bc_dx_mail_list_generate"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Mail List Generation"
        CType(Me.PanelControl3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl3.ResumeLayout(False)
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxputypes.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.tsearch.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxtaxonomy, System.ComponentModel.ISupportInitialize).EndInit()
        Me.uxtaxonomy.ResumeLayout(False)
        CType(Me.uximage, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxDocumentDetailImages, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pclear, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemDateEdit1.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemDateEdit1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemDateEdit3.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemDateEdit3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxentity, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lsearch.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureEdit1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PanelControl3 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents bcancel As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents bsave As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents uxputypes As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents LabelControl4 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents tsearch As DevExpress.XtraEditors.TextEdit
    Friend WithEvents uxtaxonomy As DevExpress.XtraTab.XtraTabControl
    Friend WithEvents uxauthor As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents uximage As DevExpress.Utils.ImageCollection
    Friend WithEvents uxDocumentDetailImages As DevExpress.Utils.ImageCollection
    Friend WithEvents mimeimages As System.Windows.Forms.ImageList
    Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents searchtimer As System.Windows.Forms.Timer
    Friend WithEvents pclear As DevExpress.XtraTreeList.TreeList
    Friend WithEvents TreeListColumn1 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents TreeListColumn2 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents RepositoryItemDateEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemDateEdit
    Friend WithEvents RepositoryItemDateEdit3 As DevExpress.XtraEditors.Repository.RepositoryItemDateEdit
    Friend WithEvents LabelControl2 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents uxentity As DevExpress.XtraEditors.ListBoxControl
    Friend WithEvents lsearch As DevExpress.XtraEditors.PictureEdit
    Friend WithEvents PictureEdit1 As DevExpress.XtraEditors.PictureEdit
End Class
