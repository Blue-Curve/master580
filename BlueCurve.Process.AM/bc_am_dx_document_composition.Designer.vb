<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bc_am_dx_document_composition
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_dx_document_composition))
        Me.uxcomps = New DevExpress.XtraTreeList.TreeList()
        Me.TreeListColumn1 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.TreeListColumn2 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.TreeListColumn12 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.TreeListColumn6 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.TreeListColumn7 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.TreeListColumn8 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.TreeListColumn10 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.uximage = New DevExpress.Utils.ImageCollection()
        Me.bcancel = New DevExpress.XtraEditors.SimpleButton()
        Me.uxImages = New System.Windows.Forms.ImageList()
        Me.bok = New DevExpress.XtraEditors.SimpleButton()
        Me.chkshowtext = New DevExpress.XtraEditors.CheckEdit()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.uxselcomps = New DevExpress.XtraTreeList.TreeList()
        Me.TreeListColumn3 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.TreeListColumn4 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.TreeListColumn5 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.Component = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.Paragraph = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.TreeListColumn9 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.LabelControl1 = New DevExpress.XtraEditors.LabelControl()
        Me.LabelControl2 = New DevExpress.XtraEditors.LabelControl()
        Me.bdn = New DevExpress.XtraEditors.SimpleButton()
        Me.bup = New DevExpress.XtraEditors.SimpleButton()
        Me.rtfpreview = New DevExpress.XtraRichEdit.RichEditControl()
        Me.breset = New DevExpress.XtraEditors.SimpleButton()
        Me.bupdate = New DevExpress.XtraEditors.SimpleButton()
        Me.tinfo = New DevExpress.XtraEditors.MemoEdit()
        Me.LabelControl3 = New DevExpress.XtraEditors.LabelControl()
        Me.LabelControl4 = New DevExpress.XtraEditors.LabelControl()
        Me.ipreview = New DevExpress.XtraEditors.PictureEdit()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip()
        CType(Me.uxcomps, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uximage, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chkshowtext.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxselcomps, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.tinfo.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ipreview.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'uxcomps
        '
        Me.uxcomps.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.uxcomps.Appearance.SelectedRow.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxcomps.Appearance.SelectedRow.Options.UseFont = True
        Me.uxcomps.BestFitVisibleOnly = True
        Me.uxcomps.Columns.AddRange(New DevExpress.XtraTreeList.Columns.TreeListColumn() {Me.TreeListColumn1, Me.TreeListColumn2, Me.TreeListColumn12, Me.TreeListColumn6, Me.TreeListColumn7, Me.TreeListColumn8, Me.TreeListColumn10})
        Me.uxcomps.Location = New System.Drawing.Point(14, 31)
        Me.uxcomps.Name = "uxcomps"
        Me.uxcomps.OptionsBehavior.EnableFiltering = True
        Me.uxcomps.OptionsFilter.ShowAllValuesInFilterPopup = True
        Me.uxcomps.OptionsMenu.EnableColumnMenu = False
        Me.uxcomps.OptionsMenu.EnableFooterMenu = False
        Me.uxcomps.OptionsSelection.EnableAppearanceFocusedCell = False
        Me.uxcomps.OptionsView.ShowHorzLines = False
        Me.uxcomps.OptionsView.ShowIndicator = False
        Me.uxcomps.OptionsView.ShowVertLines = False
        Me.uxcomps.Size = New System.Drawing.Size(444, 493)
        Me.uxcomps.StateImageList = Me.uximage
        Me.uxcomps.TabIndex = 70
        Me.ToolTip1.SetToolTip(Me.uxcomps, "Double click to select component")
        Me.uxcomps.TreeLineStyle = DevExpress.XtraTreeList.LineStyle.None
        '
        'TreeListColumn1
        '
        Me.TreeListColumn1.Caption = "Description"
        Me.TreeListColumn1.FieldName = "Item"
        Me.TreeListColumn1.MinWidth = 33
        Me.TreeListColumn1.Name = "TreeListColumn1"
        Me.TreeListColumn1.OptionsColumn.AllowEdit = False
        Me.TreeListColumn1.OptionsFilter.FilterPopupMode = DevExpress.XtraTreeList.FilterPopupMode.CheckedList
        Me.TreeListColumn1.Visible = True
        Me.TreeListColumn1.VisibleIndex = 0
        Me.TreeListColumn1.Width = 184
        '
        'TreeListColumn2
        '
        Me.TreeListColumn2.Caption = "Type"
        Me.TreeListColumn2.FieldName = "Stage"
        Me.TreeListColumn2.Name = "TreeListColumn2"
        Me.TreeListColumn2.OptionsColumn.AllowEdit = False
        Me.TreeListColumn2.OptionsFilter.FilterPopupMode = DevExpress.XtraTreeList.FilterPopupMode.CheckedList
        Me.TreeListColumn2.Visible = True
        Me.TreeListColumn2.VisibleIndex = 1
        Me.TreeListColumn2.Width = 91
        '
        'TreeListColumn12
        '
        Me.TreeListColumn12.Caption = "Page"
        Me.TreeListColumn12.FieldName = "Title"
        Me.TreeListColumn12.ImageIndex = 6
        Me.TreeListColumn12.Name = "TreeListColumn12"
        Me.TreeListColumn12.OptionsColumn.AllowEdit = False
        Me.TreeListColumn12.OptionsFilter.FilterPopupMode = DevExpress.XtraTreeList.FilterPopupMode.CheckedList
        Me.TreeListColumn12.Visible = True
        Me.TreeListColumn12.VisibleIndex = 2
        Me.TreeListColumn12.Width = 36
        '
        'TreeListColumn6
        '
        Me.TreeListColumn6.Caption = "Component"
        Me.TreeListColumn6.FieldName = "Component"
        Me.TreeListColumn6.Name = "TreeListColumn6"
        Me.TreeListColumn6.OptionsColumn.AllowEdit = False
        Me.TreeListColumn6.Width = 20
        '
        'TreeListColumn7
        '
        Me.TreeListColumn7.Caption = "Paragraph"
        Me.TreeListColumn7.FieldName = "Paragraph"
        Me.TreeListColumn7.Name = "TreeListColumn7"
        Me.TreeListColumn7.OptionsColumn.AllowEdit = False
        Me.TreeListColumn7.Width = 20
        '
        'TreeListColumn8
        '
        Me.TreeListColumn8.Caption = "table locator"
        Me.TreeListColumn8.FieldName = "table locator"
        Me.TreeListColumn8.Name = "TreeListColumn8"
        Me.TreeListColumn8.OptionsColumn.AllowEdit = False
        '
        'TreeListColumn10
        '
        Me.TreeListColumn10.Caption = "Style"
        Me.TreeListColumn10.FieldName = "Style"
        Me.TreeListColumn10.Name = "TreeListColumn10"
        Me.TreeListColumn10.OptionsColumn.AllowEdit = False
        Me.TreeListColumn10.Visible = True
        Me.TreeListColumn10.VisibleIndex = 3
        Me.TreeListColumn10.Width = 131
        '
        'uximage
        '
        Me.uximage.ImageStream = CType(resources.GetObject("uximage.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
        Me.uximage.Images.SetKeyName(0, "down_16.ico")
        Me.uximage.Images.SetKeyName(1, "search_16.ico")
        Me.uximage.Images.SetKeyName(2, "up_16.ico")
        Me.uximage.Images.SetKeyName(3, "read_16.ico")
        Me.uximage.Images.SetKeyName(4, "excel.bmp")
        Me.uximage.Images.SetKeyName(5, "table.png")
        Me.uximage.Images.SetKeyName(6, "ct.png")
        '
        'bcancel
        '
        Me.bcancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bcancel.ImageIndex = 2
        Me.bcancel.ImageList = Me.uxImages
        Me.bcancel.Location = New System.Drawing.Point(898, 544)
        Me.bcancel.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.bcancel.Name = "bcancel"
        Me.bcancel.Size = New System.Drawing.Size(75, 33)
        Me.bcancel.TabIndex = 72
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
        Me.bok.ImageIndex = 4
        Me.bok.ImageList = Me.uxImages
        Me.bok.Location = New System.Drawing.Point(986, 544)
        Me.bok.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.bok.Name = "bok"
        Me.bok.Size = New System.Drawing.Size(75, 33)
        Me.bok.TabIndex = 71
        Me.bok.Text = "Submit"
        '
        'chkshowtext
        '
        Me.chkshowtext.Location = New System.Drawing.Point(321, 9)
        Me.chkshowtext.Name = "chkshowtext"
        Me.chkshowtext.Properties.Caption = "Show Text Components"
        Me.chkshowtext.Size = New System.Drawing.Size(137, 19)
        Me.chkshowtext.TabIndex = 73
        '
        'PictureBox2
        '
        Me.PictureBox2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(14, 543)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(188, 34)
        Me.PictureBox2.TabIndex = 74
        Me.PictureBox2.TabStop = False
        '
        'uxselcomps
        '
        Me.uxselcomps.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxselcomps.Appearance.SelectedRow.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxselcomps.Appearance.SelectedRow.Options.UseFont = True
        Me.uxselcomps.BestFitVisibleOnly = True
        Me.uxselcomps.Columns.AddRange(New DevExpress.XtraTreeList.Columns.TreeListColumn() {Me.TreeListColumn3, Me.TreeListColumn4, Me.TreeListColumn5, Me.Component, Me.Paragraph, Me.TreeListColumn9})
        Me.uxselcomps.Location = New System.Drawing.Point(528, 31)
        Me.uxselcomps.Name = "uxselcomps"
        Me.uxselcomps.OptionsFilter.ShowAllValuesInFilterPopup = True
        Me.uxselcomps.OptionsMenu.EnableColumnMenu = False
        Me.uxselcomps.OptionsMenu.EnableFooterMenu = False
        Me.uxselcomps.OptionsSelection.EnableAppearanceFocusedCell = False
        Me.uxselcomps.OptionsView.ShowHorzLines = False
        Me.uxselcomps.OptionsView.ShowIndicator = False
        Me.uxselcomps.OptionsView.ShowVertLines = False
        Me.uxselcomps.Size = New System.Drawing.Size(533, 182)
        Me.uxselcomps.StateImageList = Me.uximage
        Me.uxselcomps.TabIndex = 75
        Me.uxselcomps.Tag = "Double click to remove component"
        Me.uxselcomps.TreeLineStyle = DevExpress.XtraTreeList.LineStyle.None
        '
        'TreeListColumn3
        '
        Me.TreeListColumn3.Caption = "Description"
        Me.TreeListColumn3.FieldName = "Item"
        Me.TreeListColumn3.MinWidth = 33
        Me.TreeListColumn3.Name = "TreeListColumn3"
        Me.TreeListColumn3.OptionsColumn.AllowEdit = False
        Me.TreeListColumn3.OptionsColumn.AllowSort = False
        Me.TreeListColumn3.OptionsFilter.FilterPopupMode = DevExpress.XtraTreeList.FilterPopupMode.CheckedList
        Me.TreeListColumn3.Visible = True
        Me.TreeListColumn3.VisibleIndex = 0
        Me.TreeListColumn3.Width = 201
        '
        'TreeListColumn4
        '
        Me.TreeListColumn4.Caption = "Type"
        Me.TreeListColumn4.FieldName = "Stage"
        Me.TreeListColumn4.Name = "TreeListColumn4"
        Me.TreeListColumn4.OptionsColumn.AllowEdit = False
        Me.TreeListColumn4.OptionsColumn.AllowSort = False
        Me.TreeListColumn4.OptionsFilter.FilterPopupMode = DevExpress.XtraTreeList.FilterPopupMode.CheckedList
        Me.TreeListColumn4.Visible = True
        Me.TreeListColumn4.VisibleIndex = 1
        Me.TreeListColumn4.Width = 87
        '
        'TreeListColumn5
        '
        Me.TreeListColumn5.Caption = "Page"
        Me.TreeListColumn5.FieldName = "Title"
        Me.TreeListColumn5.ImageIndex = 6
        Me.TreeListColumn5.Name = "TreeListColumn5"
        Me.TreeListColumn5.OptionsColumn.AllowEdit = False
        Me.TreeListColumn5.OptionsColumn.AllowSort = False
        Me.TreeListColumn5.OptionsFilter.FilterPopupMode = DevExpress.XtraTreeList.FilterPopupMode.CheckedList
        Me.TreeListColumn5.Visible = True
        Me.TreeListColumn5.VisibleIndex = 2
        Me.TreeListColumn5.Width = 101
        '
        'Component
        '
        Me.Component.Caption = "Paragrpah"
        Me.Component.FieldName = "Paragrpah"
        Me.Component.Name = "Component"
        Me.Component.OptionsColumn.AllowEdit = False
        Me.Component.OptionsColumn.AllowSort = False
        Me.Component.Width = 20
        '
        'Paragraph
        '
        Me.Paragraph.Caption = "Component"
        Me.Paragraph.FieldName = "Component"
        Me.Paragraph.Name = "Paragraph"
        Me.Paragraph.OptionsColumn.AllowEdit = False
        Me.Paragraph.OptionsColumn.AllowSort = False
        Me.Paragraph.Width = 20
        '
        'TreeListColumn9
        '
        Me.TreeListColumn9.Caption = "table locator"
        Me.TreeListColumn9.FieldName = "table locator"
        Me.TreeListColumn9.Name = "TreeListColumn9"
        Me.TreeListColumn9.OptionsColumn.AllowEdit = False
        '
        'LabelControl1
        '
        Me.LabelControl1.Location = New System.Drawing.Point(14, 12)
        Me.LabelControl1.Name = "LabelControl1"
        Me.LabelControl1.Size = New System.Drawing.Size(74, 13)
        Me.LabelControl1.TabIndex = 76
        Me.LabelControl1.Text = "All Components"
        '
        'LabelControl2
        '
        Me.LabelControl2.Location = New System.Drawing.Point(528, 12)
        Me.LabelControl2.Name = "LabelControl2"
        Me.LabelControl2.Size = New System.Drawing.Size(104, 13)
        Me.LabelControl2.TabIndex = 77
        Me.LabelControl2.Text = "Selected Components"
        '
        'bdn
        '
        Me.bdn.AllowDrop = True
        Me.bdn.ImageIndex = 0
        Me.bdn.ImageList = Me.uximage
        Me.bdn.Location = New System.Drawing.Point(482, 91)
        Me.bdn.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.bdn.Name = "bdn"
        Me.bdn.Size = New System.Drawing.Size(24, 24)
        Me.bdn.TabIndex = 79
        '
        'bup
        '
        Me.bup.ImageIndex = 2
        Me.bup.ImageList = Me.uximage
        Me.bup.Location = New System.Drawing.Point(482, 54)
        Me.bup.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.bup.Name = "bup"
        Me.bup.Size = New System.Drawing.Size(24, 24)
        Me.bup.TabIndex = 78
        Me.bup.Tag = " "
        '
        'rtfpreview
        '
        Me.rtfpreview.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rtfpreview.EnableToolTips = True
        Me.rtfpreview.Location = New System.Drawing.Point(528, 337)
        Me.rtfpreview.Name = "rtfpreview"
        Me.rtfpreview.Options.CopyPaste.MaintainDocumentSectionSettings = False
        Me.rtfpreview.Options.Fields.UseCurrentCultureDateTimeFormat = False
        Me.rtfpreview.Options.HorizontalRuler.ShowLeftIndent = False
        Me.rtfpreview.Options.HorizontalRuler.ShowRightIndent = False
        Me.rtfpreview.Options.HorizontalRuler.ShowTabs = False
        Me.rtfpreview.Options.HorizontalRuler.Visibility = DevExpress.XtraRichEdit.RichEditRulerVisibility.Hidden
        Me.rtfpreview.Options.MailMerge.KeepLastParagraph = False
        Me.rtfpreview.Options.VerticalRuler.Visibility = DevExpress.XtraRichEdit.RichEditRulerVisibility.Hidden
        Me.rtfpreview.ReadOnly = True
        Me.rtfpreview.ShowCaretInReadOnly = False
        Me.rtfpreview.Size = New System.Drawing.Size(533, 187)
        Me.rtfpreview.TabIndex = 80
        Me.rtfpreview.Text = "RichEditControl1"
        '
        'breset
        '
        Me.breset.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.breset.Enabled = False
        Me.breset.ImageIndex = 2
        Me.breset.ImageList = Me.uxImages
        Me.breset.Location = New System.Drawing.Point(898, 300)
        Me.breset.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.breset.Name = "breset"
        Me.breset.Size = New System.Drawing.Size(75, 33)
        Me.breset.TabIndex = 82
        Me.breset.Text = "Reset"
        '
        'bupdate
        '
        Me.bupdate.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bupdate.Enabled = False
        Me.bupdate.ImageIndex = 4
        Me.bupdate.ImageList = Me.uxImages
        Me.bupdate.Location = New System.Drawing.Point(986, 300)
        Me.bupdate.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.bupdate.Name = "bupdate"
        Me.bupdate.Size = New System.Drawing.Size(75, 33)
        Me.bupdate.TabIndex = 81
        Me.bupdate.Text = "Update"
        '
        'tinfo
        '
        Me.tinfo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tinfo.EditValue = ""
        Me.tinfo.Location = New System.Drawing.Point(528, 234)
        Me.tinfo.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.tinfo.Name = "tinfo"
        Me.tinfo.Properties.Appearance.BackColor = System.Drawing.Color.White
        Me.tinfo.Properties.Appearance.Options.UseBackColor = True
        Me.tinfo.Size = New System.Drawing.Size(533, 58)
        Me.tinfo.TabIndex = 83
        '
        'LabelControl3
        '
        Me.LabelControl3.Location = New System.Drawing.Point(528, 318)
        Me.LabelControl3.Name = "LabelControl3"
        Me.LabelControl3.Size = New System.Drawing.Size(38, 13)
        Me.LabelControl3.TabIndex = 84
        Me.LabelControl3.Text = "Preview"
        '
        'LabelControl4
        '
        Me.LabelControl4.Location = New System.Drawing.Point(528, 216)
        Me.LabelControl4.Name = "LabelControl4"
        Me.LabelControl4.Size = New System.Drawing.Size(183, 13)
        Me.LabelControl4.TabIndex = 85
        Me.LabelControl4.Text = "Additional Information For Component"
        '
        'ipreview
        '
        Me.ipreview.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ipreview.Location = New System.Drawing.Point(528, 337)
        Me.ipreview.Name = "ipreview"
        Me.ipreview.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat
        Me.ipreview.Size = New System.Drawing.Size(533, 187)
        Me.ipreview.TabIndex = 86
        Me.ipreview.Visible = False
        '
        'bc_am_dx_document_composition
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1073, 589)
        Me.Controls.Add(Me.ipreview)
        Me.Controls.Add(Me.LabelControl4)
        Me.Controls.Add(Me.LabelControl3)
        Me.Controls.Add(Me.tinfo)
        Me.Controls.Add(Me.breset)
        Me.Controls.Add(Me.bupdate)
        Me.Controls.Add(Me.rtfpreview)
        Me.Controls.Add(Me.bdn)
        Me.Controls.Add(Me.bup)
        Me.Controls.Add(Me.LabelControl2)
        Me.Controls.Add(Me.LabelControl1)
        Me.Controls.Add(Me.uxselcomps)
        Me.Controls.Add(Me.PictureBox2)
        Me.Controls.Add(Me.chkshowtext)
        Me.Controls.Add(Me.bcancel)
        Me.Controls.Add(Me.bok)
        Me.Controls.Add(Me.uxcomps)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimizeBox = False
        Me.Name = "bc_am_dx_document_composition"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Blue Curve - Document Composition"
        CType(Me.uxcomps, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uximage, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chkshowtext.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxselcomps, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.tinfo.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ipreview.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents uxcomps As DevExpress.XtraTreeList.TreeList
    Friend WithEvents TreeListColumn1 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents TreeListColumn2 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents TreeListColumn12 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents bcancel As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents bok As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents uxImages As System.Windows.Forms.ImageList
    Friend WithEvents chkshowtext As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents uxselcomps As DevExpress.XtraTreeList.TreeList
    Friend WithEvents TreeListColumn3 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents TreeListColumn4 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents TreeListColumn5 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl2 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents bdn As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents bup As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents uximage As DevExpress.Utils.ImageCollection
    Friend WithEvents rtfpreview As DevExpress.XtraRichEdit.RichEditControl
    Friend WithEvents breset As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents bupdate As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents tinfo As DevExpress.XtraEditors.MemoEdit
    Friend WithEvents LabelControl3 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl4 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents TreeListColumn6 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents TreeListColumn7 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents Component As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents Paragraph As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents ipreview As DevExpress.XtraEditors.PictureEdit
    Friend WithEvents TreeListColumn8 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents TreeListColumn9 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents TreeListColumn10 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
End Class
