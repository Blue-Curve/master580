<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bc_dx_clone_document
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_dx_clone_document))
        Me.bcancel = New DevExpress.XtraEditors.SimpleButton()
        Me.uxImages = New System.Windows.Forms.ImageList(Me.components)
        Me.bok = New DevExpress.XtraEditors.SimpleButton()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.uxdocs = New DevExpress.XtraTreeList.TreeList()
        Me.TreeListColumn1 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.TreeListColumn3 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.uxDLImageList = New DevExpress.Utils.ImageCollection(Me.components)
        Me.ltitle = New DevExpress.XtraEditors.LabelControl()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxdocs, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxDLImageList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'bcancel
        '
        Me.bcancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bcancel.ImageIndex = 2
        Me.bcancel.ImageList = Me.uxImages
        Me.bcancel.Location = New System.Drawing.Point(470, 373)
        Me.bcancel.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.bcancel.Name = "bcancel"
        Me.bcancel.Size = New System.Drawing.Size(75, 33)
        Me.bcancel.TabIndex = 75
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
        Me.bok.ImageIndex = 3
        Me.bok.ImageList = Me.uxImages
        Me.bok.Location = New System.Drawing.Point(561, 373)
        Me.bok.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.bok.Name = "bok"
        Me.bok.Size = New System.Drawing.Size(75, 33)
        Me.bok.TabIndex = 74
        Me.bok.Text = "Clone"
        '
        'PictureBox2
        '
        Me.PictureBox2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(12, 372)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(161, 33)
        Me.PictureBox2.TabIndex = 76
        Me.PictureBox2.TabStop = False
        '
        'uxdocs
        '
        Me.uxdocs.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxdocs.Appearance.SelectedRow.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxdocs.Appearance.SelectedRow.Options.UseFont = True
        Me.uxdocs.BestFitVisibleOnly = True
        Me.uxdocs.Columns.AddRange(New DevExpress.XtraTreeList.Columns.TreeListColumn() {Me.TreeListColumn1, Me.TreeListColumn3})
        Me.uxdocs.Location = New System.Drawing.Point(12, 39)
        Me.uxdocs.Name = "uxdocs"
        Me.uxdocs.OptionsBehavior.EnableFiltering = True
        Me.uxdocs.OptionsFilter.ShowAllValuesInFilterPopup = True
        Me.uxdocs.OptionsMenu.EnableColumnMenu = False
        Me.uxdocs.OptionsMenu.EnableFooterMenu = False
        Me.uxdocs.OptionsSelection.EnableAppearanceFocusedCell = False
        Me.uxdocs.OptionsView.ShowHorzLines = False
        Me.uxdocs.OptionsView.ShowIndicator = False
        Me.uxdocs.OptionsView.ShowVertLines = False
        Me.uxdocs.Size = New System.Drawing.Size(624, 313)
        Me.uxdocs.StateImageList = Me.uxDLImageList
        Me.uxdocs.TabIndex = 80
        Me.uxdocs.TreeLineStyle = DevExpress.XtraTreeList.LineStyle.None
        '
        'TreeListColumn1
        '
        Me.TreeListColumn1.Caption = "Title"
        Me.TreeListColumn1.FieldName = "Title"
        Me.TreeListColumn1.ImageIndex = 6
        Me.TreeListColumn1.MinWidth = 33
        Me.TreeListColumn1.Name = "TreeListColumn1"
        Me.TreeListColumn1.OptionsColumn.AllowEdit = False
        Me.TreeListColumn1.OptionsFilter.FilterPopupMode = DevExpress.XtraTreeList.FilterPopupMode.CheckedList
        Me.TreeListColumn1.Visible = True
        Me.TreeListColumn1.VisibleIndex = 0
        Me.TreeListColumn1.Width = 457
        '
        'TreeListColumn3
        '
        Me.TreeListColumn3.Caption = "Date"
        Me.TreeListColumn3.FieldName = "Stage"
        Me.TreeListColumn3.Format.FormatString = "dd-MMM-yyy HH:mm:ss"
        Me.TreeListColumn3.Format.FormatType = DevExpress.Utils.FormatType.DateTime
        Me.TreeListColumn3.ImageIndex = 24
        Me.TreeListColumn3.MinWidth = 49
        Me.TreeListColumn3.Name = "TreeListColumn3"
        Me.TreeListColumn3.OptionsColumn.AllowEdit = False
        Me.TreeListColumn3.OptionsFilter.FilterPopupMode = DevExpress.XtraTreeList.FilterPopupMode.CheckedList
        Me.TreeListColumn3.SortOrder = System.Windows.Forms.SortOrder.Descending
        Me.TreeListColumn3.UnboundType = DevExpress.XtraTreeList.Data.UnboundColumnType.DateTime
        Me.TreeListColumn3.Visible = True
        Me.TreeListColumn3.VisibleIndex = 1
        Me.TreeListColumn3.Width = 165
        '
        'uxDLImageList
        '
        Me.uxDLImageList.ImageStream = CType(resources.GetObject("uxDLImageList.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
        Me.uxDLImageList.Images.SetKeyName(0, "document_32.ico")
        Me.uxDLImageList.Images.SetKeyName(1, "checkin_in_to_me_32.ico")
        Me.uxDLImageList.Images.SetKeyName(2, "checkin_in_to_another_32.ico")
        Me.uxDLImageList.Images.SetKeyName(3, "unregistered_60.png")
        '
        'ltitle
        '
        Me.ltitle.Location = New System.Drawing.Point(12, 11)
        Me.ltitle.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.ltitle.Name = "ltitle"
        Me.ltitle.Size = New System.Drawing.Size(71, 13)
        Me.ltitle.TabIndex = 83
        Me.ltitle.Text = "Search Criteria"
        '
        'bc_dx_clone_document
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(651, 417)
        Me.Controls.Add(Me.ltitle)
        Me.Controls.Add(Me.uxdocs)
        Me.Controls.Add(Me.PictureBox2)
        Me.Controls.Add(Me.bcancel)
        Me.Controls.Add(Me.bok)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimizeBox = False
        Me.Name = "bc_dx_clone_document"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Clone Document"
        Me.TopMost = True
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxdocs, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxDLImageList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents bcancel As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents bok As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents uxImages As System.Windows.Forms.ImageList
    Friend WithEvents uxdocs As DevExpress.XtraTreeList.TreeList
    Friend WithEvents TreeListColumn1 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents TreeListColumn3 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents uxDLImageList As DevExpress.Utils.ImageCollection
    Friend WithEvents ltitle As DevExpress.XtraEditors.LabelControl
End Class
