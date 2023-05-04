<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bc_am_dx_show_comments
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_dx_show_comments))
        Me.bcancel = New DevExpress.XtraEditors.SimpleButton()
        Me.uxcommentlist = New DevExpress.XtraTreeList.TreeList()
        Me.TreeListColumn2 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.RepositoryItemMemoEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit()
        Me.RepositoryItemTextEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemTextEdit()
        Me.uxDocumentDetailImages = New DevExpress.Utils.ImageCollection()
        Me.RadioGroup1 = New DevExpress.XtraEditors.RadioGroup()
        Me.Hlatest = New DevExpress.XtraEditors.HyperLinkEdit()
        Me.HyperLinkEdit2 = New DevExpress.XtraEditors.HyperLinkEdit()
        Me.HyperLinkEdit1 = New DevExpress.XtraEditors.HyperLinkEdit()
        Me.HyperLinkEdit4 = New DevExpress.XtraEditors.HyperLinkEdit()
        Me.HyperLinkEdit5 = New DevExpress.XtraEditors.HyperLinkEdit()
        Me.LabelControl6 = New DevExpress.XtraEditors.LabelControl()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.ltitle = New DevExpress.XtraEditors.LabelControl()
        CType(Me.uxcommentlist, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemMemoEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemTextEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxDocumentDetailImages, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadioGroup1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Hlatest.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.HyperLinkEdit2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.HyperLinkEdit1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.HyperLinkEdit4.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.HyperLinkEdit5.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'bcancel
        '
        Me.bcancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bcancel.ImageIndex = 2
        Me.bcancel.Location = New System.Drawing.Point(755, 368)
        Me.bcancel.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.bcancel.Name = "bcancel"
        Me.bcancel.Size = New System.Drawing.Size(74, 32)
        Me.bcancel.TabIndex = 68
        Me.bcancel.Text = "Close"
        '
        'uxcommentlist
        '
        Me.uxcommentlist.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxcommentlist.BestFitVisibleOnly = True
        Me.uxcommentlist.Columns.AddRange(New DevExpress.XtraTreeList.Columns.TreeListColumn() {Me.TreeListColumn2})
        Me.uxcommentlist.Location = New System.Drawing.Point(12, 32)
        Me.uxcommentlist.Name = "uxcommentlist"
        Me.uxcommentlist.OptionsBehavior.Editable = False
        Me.uxcommentlist.OptionsMenu.EnableColumnMenu = False
        Me.uxcommentlist.OptionsMenu.EnableFooterMenu = False
        Me.uxcommentlist.OptionsView.ShowColumns = False
        Me.uxcommentlist.OptionsView.ShowIndicator = False
        Me.uxcommentlist.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemTextEdit1, Me.RepositoryItemMemoEdit1})
        Me.uxcommentlist.Size = New System.Drawing.Size(817, 292)
        Me.uxcommentlist.StateImageList = Me.uxDocumentDetailImages
        Me.uxcommentlist.TabIndex = 69
        Me.uxcommentlist.TreeLineStyle = DevExpress.XtraTreeList.LineStyle.None
        '
        'TreeListColumn2
        '
        Me.TreeListColumn2.Caption = "Stage"
        Me.TreeListColumn2.ColumnEdit = Me.RepositoryItemMemoEdit1
        Me.TreeListColumn2.FieldName = "Stage"
        Me.TreeListColumn2.MinWidth = 33
        Me.TreeListColumn2.Name = "TreeListColumn2"
        Me.TreeListColumn2.Visible = True
        Me.TreeListColumn2.VisibleIndex = 0
        Me.TreeListColumn2.Width = 280
        '
        'RepositoryItemMemoEdit1
        '
        Me.RepositoryItemMemoEdit1.Name = "RepositoryItemMemoEdit1"
        Me.RepositoryItemMemoEdit1.ReadOnly = True
        '
        'RepositoryItemTextEdit1
        '
        Me.RepositoryItemTextEdit1.AutoHeight = False
        Me.RepositoryItemTextEdit1.Name = "RepositoryItemTextEdit1"
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
        'RadioGroup1
        '
        Me.RadioGroup1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.RadioGroup1.Location = New System.Drawing.Point(54, 330)
        Me.RadioGroup1.Name = "RadioGroup1"
        Me.RadioGroup1.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.RadioGroupItem() {New DevExpress.XtraEditors.Controls.RadioGroupItem(Nothing, "Latest"), New DevExpress.XtraEditors.Controls.RadioGroupItem(Nothing, "1 day"), New DevExpress.XtraEditors.Controls.RadioGroupItem(Nothing, "1 week"), New DevExpress.XtraEditors.Controls.RadioGroupItem(Nothing, "1 month"), New DevExpress.XtraEditors.Controls.RadioGroupItem(Nothing, "All")})
        Me.RadioGroup1.Size = New System.Drawing.Size(361, 25)
        Me.RadioGroup1.TabIndex = 76
        '
        'Hlatest
        '
        Me.Hlatest.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Hlatest.EditValue = "Latest"
        Me.Hlatest.Location = New System.Drawing.Point(54, 330)
        Me.Hlatest.Name = "Hlatest"
        Me.Hlatest.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.Hlatest.Properties.Appearance.Options.UseBackColor = True
        Me.Hlatest.Size = New System.Drawing.Size(49, 20)
        Me.Hlatest.TabIndex = 75
        '
        'HyperLinkEdit2
        '
        Me.HyperLinkEdit2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.HyperLinkEdit2.EditValue = "1 day"
        Me.HyperLinkEdit2.Location = New System.Drawing.Point(113, 330)
        Me.HyperLinkEdit2.Name = "HyperLinkEdit2"
        Me.HyperLinkEdit2.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.HyperLinkEdit2.Properties.Appearance.Options.UseBackColor = True
        Me.HyperLinkEdit2.Size = New System.Drawing.Size(47, 20)
        Me.HyperLinkEdit2.TabIndex = 74
        '
        'HyperLinkEdit1
        '
        Me.HyperLinkEdit1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.HyperLinkEdit1.EditValue = "1 month"
        Me.HyperLinkEdit1.Location = New System.Drawing.Point(219, 330)
        Me.HyperLinkEdit1.Name = "HyperLinkEdit1"
        Me.HyperLinkEdit1.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.HyperLinkEdit1.Properties.Appearance.Options.UseBackColor = True
        Me.HyperLinkEdit1.Size = New System.Drawing.Size(48, 20)
        Me.HyperLinkEdit1.TabIndex = 72
        '
        'HyperLinkEdit4
        '
        Me.HyperLinkEdit4.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.HyperLinkEdit4.EditValue = "All"
        Me.HyperLinkEdit4.Location = New System.Drawing.Point(276, 330)
        Me.HyperLinkEdit4.Name = "HyperLinkEdit4"
        Me.HyperLinkEdit4.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.HyperLinkEdit4.Properties.Appearance.Options.UseBackColor = True
        Me.HyperLinkEdit4.Size = New System.Drawing.Size(48, 20)
        Me.HyperLinkEdit4.TabIndex = 71
        '
        'HyperLinkEdit5
        '
        Me.HyperLinkEdit5.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.HyperLinkEdit5.EditValue = "1 wk"
        Me.HyperLinkEdit5.Location = New System.Drawing.Point(166, 330)
        Me.HyperLinkEdit5.Name = "HyperLinkEdit5"
        Me.HyperLinkEdit5.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.HyperLinkEdit5.Properties.Appearance.Options.UseBackColor = True
        Me.HyperLinkEdit5.Size = New System.Drawing.Size(47, 20)
        Me.HyperLinkEdit5.TabIndex = 73
        '
        'LabelControl6
        '
        Me.LabelControl6.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LabelControl6.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical
        Me.LabelControl6.Location = New System.Drawing.Point(12, 333)
        Me.LabelControl6.Name = "LabelControl6"
        Me.LabelControl6.Size = New System.Drawing.Size(107, 13)
        Me.LabelControl6.TabIndex = 70
        Me.LabelControl6.Text = "Show"
        '
        'PictureBox2
        '
        Me.PictureBox2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(12, 365)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(188, 34)
        Me.PictureBox2.TabIndex = 77
        Me.PictureBox2.TabStop = False
        '
        'ltitle
        '
        Me.ltitle.Location = New System.Drawing.Point(12, 12)
        Me.ltitle.Name = "ltitle"
        Me.ltitle.Size = New System.Drawing.Size(0, 13)
        Me.ltitle.TabIndex = 78
        '
        'bc_am_dx_show_comments
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(841, 411)
        Me.Controls.Add(Me.ltitle)
        Me.Controls.Add(Me.PictureBox2)
        Me.Controls.Add(Me.RadioGroup1)
        Me.Controls.Add(Me.Hlatest)
        Me.Controls.Add(Me.HyperLinkEdit2)
        Me.Controls.Add(Me.HyperLinkEdit1)
        Me.Controls.Add(Me.HyperLinkEdit4)
        Me.Controls.Add(Me.HyperLinkEdit5)
        Me.Controls.Add(Me.LabelControl6)
        Me.Controls.Add(Me.uxcommentlist)
        Me.Controls.Add(Me.bcancel)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimizeBox = False
        Me.Name = "bc_am_dx_show_comments"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Blue Curve  - View  Document Comments"
        CType(Me.uxcommentlist, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemMemoEdit1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemTextEdit1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxDocumentDetailImages, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadioGroup1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Hlatest.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.HyperLinkEdit2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.HyperLinkEdit1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.HyperLinkEdit4.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.HyperLinkEdit5.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents bcancel As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents uxcommentlist As DevExpress.XtraTreeList.TreeList
    Friend WithEvents TreeListColumn2 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents RepositoryItemMemoEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit
    Friend WithEvents RepositoryItemTextEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemTextEdit
    Friend WithEvents RadioGroup1 As DevExpress.XtraEditors.RadioGroup
    Friend WithEvents Hlatest As DevExpress.XtraEditors.HyperLinkEdit
    Friend WithEvents HyperLinkEdit2 As DevExpress.XtraEditors.HyperLinkEdit
    Friend WithEvents HyperLinkEdit1 As DevExpress.XtraEditors.HyperLinkEdit
    Friend WithEvents HyperLinkEdit4 As DevExpress.XtraEditors.HyperLinkEdit
    Friend WithEvents HyperLinkEdit5 As DevExpress.XtraEditors.HyperLinkEdit
    Friend WithEvents LabelControl6 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents uxDocumentDetailImages As DevExpress.Utils.ImageCollection
    Friend WithEvents ltitle As DevExpress.XtraEditors.LabelControl
End Class
