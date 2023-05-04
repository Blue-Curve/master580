<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bc_dx_doc_open
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_dx_doc_open))
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.bcancel = New DevExpress.XtraEditors.SimpleButton()
        Me.uxImages = New System.Windows.Forms.ImageList()
        Me.bok = New DevExpress.XtraEditors.SimpleButton()
        Me.uxdocs = New DevExpress.XtraTreeList.TreeList()
        Me.TreeListColumn6 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.TreeListColumn1 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.TreeListColumn2 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.TreeListColumn12 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.TreeListColumn14 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.TreeListColumn3 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.TreeListColumn4 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.TreeListColumn13 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.RepositoryItemImageEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemImageEdit()
        Me.uxDocumentDetailImages = New DevExpress.Utils.ImageCollection()
        Me.uxDLImageList = New DevExpress.Utils.ImageCollection()
        Me.mimeimages = New System.Windows.Forms.ImageList()
        Me.ImageCollection1 = New DevExpress.Utils.ImageCollection()
        Me.uxcurrent = New DevExpress.XtraEditors.CheckEdit()
        Me.uxto = New DevExpress.XtraEditors.DateEdit()
        Me.LabelControl2 = New DevExpress.XtraEditors.LabelControl()
        Me.LabelControl1 = New DevExpress.XtraEditors.LabelControl()
        Me.uxfrom = New DevExpress.XtraEditors.DateEdit()
        Me.uxpublish = New DevExpress.XtraEditors.CheckEdit()
        Me.rsource = New DevExpress.XtraEditors.RadioGroup()
        Me.SimpleButton1 = New DevExpress.XtraEditors.SimpleButton()
        Me.uxcomments = New DevExpress.XtraEditors.SimpleButton()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxdocs, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemImageEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxDocumentDetailImages, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxDLImageList, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ImageCollection1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxcurrent.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxto.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxto.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxfrom.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxfrom.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxpublish.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rsource.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PictureBox2
        '
        Me.PictureBox2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(3, 503)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(188, 34)
        Me.PictureBox2.TabIndex = 68
        Me.PictureBox2.TabStop = False
        '
        'bcancel
        '
        Me.bcancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bcancel.ImageIndex = 2
        Me.bcancel.ImageList = Me.uxImages
        Me.bcancel.Location = New System.Drawing.Point(684, 503)
        Me.bcancel.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.bcancel.Name = "bcancel"
        Me.bcancel.Size = New System.Drawing.Size(77, 32)
        Me.bcancel.TabIndex = 67
        Me.bcancel.Text = "Cancel"
        '
        'uxImages
        '
        Me.uxImages.ImageStream = CType(resources.GetObject("uxImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.uxImages.TransparentColor = System.Drawing.Color.Transparent
        Me.uxImages.Images.SetKeyName(0, "")
        Me.uxImages.Images.SetKeyName(1, "")
        Me.uxImages.Images.SetKeyName(2, "")
        Me.uxImages.Images.SetKeyName(3, "")
        Me.uxImages.Images.SetKeyName(4, "")
        Me.uxImages.Images.SetKeyName(5, "")
        Me.uxImages.Images.SetKeyName(6, "")
        Me.uxImages.Images.SetKeyName(7, "")
        Me.uxImages.Images.SetKeyName(8, "")
        '
        'bok
        '
        Me.bok.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bok.Enabled = False
        Me.bok.ImageIndex = 8
        Me.bok.ImageList = Me.uxImages
        Me.bok.Location = New System.Drawing.Point(589, 503)
        Me.bok.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.bok.Name = "bok"
        Me.bok.Size = New System.Drawing.Size(77, 32)
        Me.bok.TabIndex = 66
        Me.bok.Text = "Open"
        '
        'uxdocs
        '
        Me.uxdocs.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxdocs.Appearance.SelectedRow.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxdocs.Appearance.SelectedRow.Options.UseFont = True
        Me.uxdocs.BestFitVisibleOnly = True
        Me.uxdocs.Columns.AddRange(New DevExpress.XtraTreeList.Columns.TreeListColumn() {Me.TreeListColumn6, Me.TreeListColumn1, Me.TreeListColumn2, Me.TreeListColumn12, Me.TreeListColumn14, Me.TreeListColumn3, Me.TreeListColumn4, Me.TreeListColumn13})
        Me.uxdocs.Location = New System.Drawing.Point(3, 12)
        Me.uxdocs.Name = "uxdocs"
        Me.uxdocs.OptionsBehavior.EnableFiltering = True
        Me.uxdocs.OptionsFilter.ShowAllValuesInFilterPopup = True
        Me.uxdocs.OptionsMenu.EnableColumnMenu = False
        Me.uxdocs.OptionsMenu.EnableFooterMenu = False
        Me.uxdocs.OptionsSelection.EnableAppearanceFocusedCell = False
        Me.uxdocs.OptionsView.ShowHorzLines = False
        Me.uxdocs.OptionsView.ShowIndicator = False
        Me.uxdocs.OptionsView.ShowVertLines = False
        Me.uxdocs.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemImageEdit1})
        Me.uxdocs.Size = New System.Drawing.Size(758, 442)
        Me.uxdocs.StateImageList = Me.uxDLImageList
        Me.uxdocs.TabIndex = 69
        Me.uxdocs.TreeLineStyle = DevExpress.XtraTreeList.LineStyle.None
        '
        'TreeListColumn6
        '
        Me.TreeListColumn6.Caption = "Date"
        Me.TreeListColumn6.FieldName = "Stage"
        Me.TreeListColumn6.Format.FormatString = "dd-MMM-yyy HH:mm:ss"
        Me.TreeListColumn6.Format.FormatType = DevExpress.Utils.FormatType.DateTime
        Me.TreeListColumn6.ImageIndex = 24
        Me.TreeListColumn6.MinWidth = 49
        Me.TreeListColumn6.Name = "TreeListColumn6"
        Me.TreeListColumn6.OptionsColumn.AllowEdit = False
        Me.TreeListColumn6.OptionsFilter.FilterPopupMode = DevExpress.XtraTreeList.FilterPopupMode.CheckedList
        Me.TreeListColumn6.SortOrder = System.Windows.Forms.SortOrder.Descending
        Me.TreeListColumn6.UnboundType = DevExpress.XtraTreeList.Data.UnboundColumnType.DateTime
        Me.TreeListColumn6.Visible = True
        Me.TreeListColumn6.VisibleIndex = 0
        Me.TreeListColumn6.Width = 123
        '
        'TreeListColumn1
        '
        Me.TreeListColumn1.Caption = "Item"
        Me.TreeListColumn1.FieldName = "Item"
        Me.TreeListColumn1.Name = "TreeListColumn1"
        Me.TreeListColumn1.OptionsColumn.AllowEdit = False
        Me.TreeListColumn1.OptionsFilter.FilterPopupMode = DevExpress.XtraTreeList.FilterPopupMode.CheckedList
        Me.TreeListColumn1.Visible = True
        Me.TreeListColumn1.VisibleIndex = 1
        Me.TreeListColumn1.Width = 74
        '
        'TreeListColumn2
        '
        Me.TreeListColumn2.Caption = "Stage"
        Me.TreeListColumn2.FieldName = "Stage"
        Me.TreeListColumn2.Name = "TreeListColumn2"
        Me.TreeListColumn2.OptionsColumn.AllowEdit = False
        Me.TreeListColumn2.OptionsFilter.FilterPopupMode = DevExpress.XtraTreeList.FilterPopupMode.CheckedList
        Me.TreeListColumn2.Visible = True
        Me.TreeListColumn2.VisibleIndex = 2
        Me.TreeListColumn2.Width = 77
        '
        'TreeListColumn12
        '
        Me.TreeListColumn12.Caption = "Title"
        Me.TreeListColumn12.FieldName = "Title"
        Me.TreeListColumn12.ImageIndex = 6
        Me.TreeListColumn12.Name = "TreeListColumn12"
        Me.TreeListColumn12.OptionsColumn.AllowEdit = False
        Me.TreeListColumn12.OptionsFilter.FilterPopupMode = DevExpress.XtraTreeList.FilterPopupMode.CheckedList
        Me.TreeListColumn12.Visible = True
        Me.TreeListColumn12.VisibleIndex = 3
        Me.TreeListColumn12.Width = 174
        '
        'TreeListColumn14
        '
        Me.TreeListColumn14.Caption = "Type"
        Me.TreeListColumn14.FieldName = "Type"
        Me.TreeListColumn14.Name = "TreeListColumn14"
        Me.TreeListColumn14.OptionsColumn.AllowEdit = False
        Me.TreeListColumn14.OptionsFilter.FilterPopupMode = DevExpress.XtraTreeList.FilterPopupMode.CheckedList
        Me.TreeListColumn14.Visible = True
        Me.TreeListColumn14.VisibleIndex = 4
        Me.TreeListColumn14.Width = 68
        '
        'TreeListColumn3
        '
        Me.TreeListColumn3.Caption = "Author"
        Me.TreeListColumn3.FieldName = "Author"
        Me.TreeListColumn3.Name = "TreeListColumn3"
        Me.TreeListColumn3.OptionsColumn.AllowEdit = False
        Me.TreeListColumn3.OptionsFilter.FilterPopupMode = DevExpress.XtraTreeList.FilterPopupMode.CheckedList
        Me.TreeListColumn3.Visible = True
        Me.TreeListColumn3.VisibleIndex = 5
        Me.TreeListColumn3.Width = 55
        '
        'TreeListColumn4
        '
        Me.TreeListColumn4.Caption = "Bus Area"
        Me.TreeListColumn4.FieldName = "Bus Area"
        Me.TreeListColumn4.Name = "TreeListColumn4"
        Me.TreeListColumn4.OptionsColumn.AllowEdit = False
        Me.TreeListColumn4.OptionsFilter.FilterPopupMode = DevExpress.XtraTreeList.FilterPopupMode.CheckedList
        Me.TreeListColumn4.Visible = True
        Me.TreeListColumn4.VisibleIndex = 6
        Me.TreeListColumn4.Width = 57
        '
        'TreeListColumn13
        '
        Me.TreeListColumn13.Caption = "Checked Out To"
        Me.TreeListColumn13.FieldName = "Checked Out To"
        Me.TreeListColumn13.ImageIndex = 12
        Me.TreeListColumn13.Name = "TreeListColumn13"
        Me.TreeListColumn13.OptionsColumn.AllowEdit = False
        Me.TreeListColumn13.OptionsFilter.FilterPopupMode = DevExpress.XtraTreeList.FilterPopupMode.CheckedList
        Me.TreeListColumn13.Visible = True
        Me.TreeListColumn13.VisibleIndex = 7
        Me.TreeListColumn13.Width = 92
        '
        'RepositoryItemImageEdit1
        '
        Me.RepositoryItemImageEdit1.AutoHeight = False
        Me.RepositoryItemImageEdit1.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.RepositoryItemImageEdit1.Images = Me.uxDocumentDetailImages
        Me.RepositoryItemImageEdit1.Name = "RepositoryItemImageEdit1"
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
        'uxDLImageList
        '
        Me.uxDLImageList.ImageStream = CType(resources.GetObject("uxDLImageList.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
        Me.uxDLImageList.Images.SetKeyName(0, "document_32.ico")
        Me.uxDLImageList.Images.SetKeyName(1, "checkin_in_to_me_32.ico")
        Me.uxDLImageList.Images.SetKeyName(2, "checkin_in_to_another_32.ico")
        Me.uxDLImageList.Images.SetKeyName(3, "unregistered_60.png")
        '
        'mimeimages
        '
        Me.mimeimages.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit
        Me.mimeimages.ImageSize = New System.Drawing.Size(16, 16)
        Me.mimeimages.TransparentColor = System.Drawing.Color.Transparent
        '
        'ImageCollection1
        '
        Me.ImageCollection1.ImageStream = CType(resources.GetObject("ImageCollection1.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
        Me.ImageCollection1.Images.SetKeyName(0, "support_32.png")
        Me.ImageCollection1.Images.SetKeyName(1, "document_32.png")
        Me.ImageCollection1.Images.SetKeyName(2, "checkin_in_force_32.ico")
        Me.ImageCollection1.Images.SetKeyName(3, "checkin_in_to_another_32.ico")
        Me.ImageCollection1.Images.SetKeyName(4, "checkin_in_to_me_32.ico")
        Me.ImageCollection1.Images.SetKeyName(5, "comment_32.ico")
        Me.ImageCollection1.Images.SetKeyName(6, "edit_32.ico")
        Me.ImageCollection1.Images.SetKeyName(7, "history_32.ico")
        Me.ImageCollection1.Images.SetKeyName(8, "links_32.ico")
        Me.ImageCollection1.Images.SetKeyName(9, "locked_32.ico")
        Me.ImageCollection1.Images.SetKeyName(10, "permissions_32.ico")
        Me.ImageCollection1.Images.SetKeyName(11, "read_32.ico")
        Me.ImageCollection1.Images.SetKeyName(12, "user_32.ico")
        Me.ImageCollection1.Images.SetKeyName(13, "write_32.ico")
        Me.ImageCollection1.Images.SetKeyName(14, "attach_32.ico")
        Me.ImageCollection1.Images.SetKeyName(15, "categorise_32.ico")
        Me.ImageCollection1.Images.SetKeyName(16, "filter_32.ico")
        Me.ImageCollection1.Images.SetKeyName(17, "icon1_32.ico")
        Me.ImageCollection1.Images.SetKeyName(18, "icon2_32.ico")
        Me.ImageCollection1.Images.SetKeyName(19, "icon3_32.ico")
        Me.ImageCollection1.Images.SetKeyName(20, "icon4_32.ico")
        Me.ImageCollection1.Images.SetKeyName(21, "icon5_32.ico")
        Me.ImageCollection1.Images.SetKeyName(22, "icon6_32.ico")
        Me.ImageCollection1.Images.SetKeyName(23, "import_32.ico")
        Me.ImageCollection1.Images.SetKeyName(24, "refresh_32.ico")
        Me.ImageCollection1.Images.SetKeyName(25, "register_32.ico")
        Me.ImageCollection1.Images.SetKeyName(26, "registered_document_32.ico")
        Me.ImageCollection1.Images.SetKeyName(27, "settings_32.ico")
        Me.ImageCollection1.Images.SetKeyName(28, "delete_32.ico")
        Me.ImageCollection1.Images.SetKeyName(29, "move_32.ico")
        Me.ImageCollection1.Images.SetKeyName(30, "revert_32.ico")
        Me.ImageCollection1.Images.SetKeyName(31, "register_32.ico")
        Me.ImageCollection1.Images.SetKeyName(32, "delete_32.ico")
        Me.ImageCollection1.Images.SetKeyName(33, "move_32.ico")
        Me.ImageCollection1.Images.SetKeyName(34, "revert_32.ico")
        Me.ImageCollection1.Images.SetKeyName(35, "checkedin_IN_32.ico")
        Me.ImageCollection1.Images.SetKeyName(36, "checkedin_OUT_32.ico")
        Me.ImageCollection1.Images.SetKeyName(37, "unregistered_60.png")
        '
        'uxcurrent
        '
        Me.uxcurrent.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.uxcurrent.EditValue = True
        Me.uxcurrent.Location = New System.Drawing.Point(483, 470)
        Me.uxcurrent.Name = "uxcurrent"
        Me.uxcurrent.Properties.Caption = "Current"
        Me.uxcurrent.Size = New System.Drawing.Size(74, 19)
        Me.uxcurrent.TabIndex = 86
        '
        'uxto
        '
        Me.uxto.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.uxto.EditValue = New Date(2013, 10, 10, 0, 0, 0, 0)
        Me.uxto.Enabled = False
        Me.uxto.Location = New System.Drawing.Point(554, 469)
        Me.uxto.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.uxto.Name = "uxto"
        Me.uxto.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.uxto.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
        Me.uxto.Size = New System.Drawing.Size(88, 20)
        Me.uxto.TabIndex = 85
        '
        'LabelControl2
        '
        Me.LabelControl2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LabelControl2.Location = New System.Drawing.Point(289, 472)
        Me.LabelControl2.Name = "LabelControl2"
        Me.LabelControl2.Size = New System.Drawing.Size(50, 13)
        Me.LabelControl2.TabIndex = 84
        Me.LabelControl2.Text = "Date From"
        '
        'LabelControl1
        '
        Me.LabelControl1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LabelControl1.Location = New System.Drawing.Point(439, 472)
        Me.LabelControl1.Name = "LabelControl1"
        Me.LabelControl1.Size = New System.Drawing.Size(38, 13)
        Me.LabelControl1.TabIndex = 87
        Me.LabelControl1.Text = "Date To"
        '
        'uxfrom
        '
        Me.uxfrom.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.uxfrom.EditValue = New Date(2013, 10, 10, 0, 0, 0, 0)
        Me.uxfrom.Location = New System.Drawing.Point(345, 469)
        Me.uxfrom.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.uxfrom.Name = "uxfrom"
        Me.uxfrom.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.uxfrom.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
        Me.uxfrom.Size = New System.Drawing.Size(88, 20)
        Me.uxfrom.TabIndex = 88
        '
        'uxpublish
        '
        Me.uxpublish.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.uxpublish.Location = New System.Drawing.Point(186, 470)
        Me.uxpublish.Name = "uxpublish"
        Me.uxpublish.Properties.Caption = "Show Publish"
        Me.uxpublish.Size = New System.Drawing.Size(97, 19)
        Me.uxpublish.TabIndex = 89
        '
        'rsource
        '
        Me.rsource.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.rsource.Location = New System.Drawing.Point(3, 460)
        Me.rsource.Name = "rsource"
        Me.rsource.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(236, Byte), Integer), CType(CType(239, Byte), Integer))
        Me.rsource.Properties.Appearance.Options.UseBackColor = True
        Me.rsource.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.RadioGroupItem() {New DevExpress.XtraEditors.Controls.RadioGroupItem(CType(0, Short), "Group"), New DevExpress.XtraEditors.Controls.RadioGroupItem(CType(1, Short), "Mine"), New DevExpress.XtraEditors.Controls.RadioGroupItem(CType(2, Short), "Local")})
        Me.rsource.Size = New System.Drawing.Size(168, 29)
        Me.rsource.TabIndex = 90
        '
        'SimpleButton1
        '
        Me.SimpleButton1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SimpleButton1.ImageIndex = 7
        Me.SimpleButton1.ImageList = Me.uxImages
        Me.SimpleButton1.Location = New System.Drawing.Point(400, 503)
        Me.SimpleButton1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.SimpleButton1.Name = "SimpleButton1"
        Me.SimpleButton1.ShowToolTips = False
        Me.SimpleButton1.Size = New System.Drawing.Size(77, 32)
        Me.SimpleButton1.TabIndex = 91
        Me.SimpleButton1.Text = "Refresh"
        '
        'uxcomments
        '
        Me.uxcomments.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxcomments.Enabled = False
        Me.uxcomments.ImageIndex = 5
        Me.uxcomments.ImageList = Me.uxDocumentDetailImages
        Me.uxcomments.Location = New System.Drawing.Point(494, 503)
        Me.uxcomments.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.uxcomments.Name = "uxcomments"
        Me.uxcomments.ShowToolTips = False
        Me.uxcomments.Size = New System.Drawing.Size(77, 32)
        Me.uxcomments.TabIndex = 92
        Me.uxcomments.Text = "Comments"
        Me.uxcomments.ToolTip = "show comments for selected document"
        '
        'bc_dx_doc_open
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(773, 549)
        Me.Controls.Add(Me.uxcomments)
        Me.Controls.Add(Me.SimpleButton1)
        Me.Controls.Add(Me.rsource)
        Me.Controls.Add(Me.uxpublish)
        Me.Controls.Add(Me.uxfrom)
        Me.Controls.Add(Me.LabelControl1)
        Me.Controls.Add(Me.uxcurrent)
        Me.Controls.Add(Me.uxto)
        Me.Controls.Add(Me.LabelControl2)
        Me.Controls.Add(Me.uxdocs)
        Me.Controls.Add(Me.PictureBox2)
        Me.Controls.Add(Me.bcancel)
        Me.Controls.Add(Me.bok)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimizeBox = False
        Me.Name = "bc_dx_doc_open"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Blue Curve - Create Open"
        Me.TopMost = True
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxdocs, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemImageEdit1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxDocumentDetailImages, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxDLImageList, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ImageCollection1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxcurrent.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxto.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxto.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxfrom.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxfrom.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxpublish.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rsource.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents bcancel As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents bok As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents uxImages As System.Windows.Forms.ImageList
    Friend WithEvents uxdocs As DevExpress.XtraTreeList.TreeList
    Friend WithEvents TreeListColumn6 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents TreeListColumn12 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents TreeListColumn14 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents TreeListColumn13 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents uxDocumentDetailImages As DevExpress.Utils.ImageCollection
    Friend WithEvents ImageCollection1 As DevExpress.Utils.ImageCollection
    Friend WithEvents mimeimages As System.Windows.Forms.ImageList
    Friend WithEvents TreeListColumn1 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents TreeListColumn2 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents TreeListColumn3 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents TreeListColumn4 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents uxDLImageList As DevExpress.Utils.ImageCollection
    Friend WithEvents uxcurrent As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents uxto As DevExpress.XtraEditors.DateEdit
    Friend WithEvents LabelControl2 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents uxfrom As DevExpress.XtraEditors.DateEdit
    Friend WithEvents uxpublish As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents rsource As DevExpress.XtraEditors.RadioGroup
    Friend WithEvents SimpleButton1 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents RepositoryItemImageEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemImageEdit
    Friend WithEvents uxcomments As DevExpress.XtraEditors.SimpleButton
End Class
