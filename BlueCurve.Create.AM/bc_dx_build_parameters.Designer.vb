<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class view_bc_dx_build_parameters
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(view_bc_dx_build_parameters))
        Me.bok = New DevExpress.XtraEditors.SimpleButton()
        Me.uxImages = New System.Windows.Forms.ImageList()
        Me.uxparams = New DevExpress.XtraTreeList.TreeList()
        Me.Param = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.Value = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.RepositoryItemComboBox3 = New DevExpress.XtraEditors.Repository.RepositoryItemComboBox()
        Me.RepositoryItemTextEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemTextEdit()
        Me.RepositoryItemMemoEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit()
        Me.RepositoryItemComboBox2 = New DevExpress.XtraEditors.Repository.RepositoryItemComboBox()
        Me.RepositoryItemImageComboBox1 = New DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox()
        Me.RepositoryItemComboBox1 = New DevExpress.XtraEditors.Repository.RepositoryItemComboBox()
        Me.RepositoryItemTextEdit2 = New DevExpress.XtraEditors.Repository.RepositoryItemTextEdit()
        Me.tabimages = New System.Windows.Forms.ImageList()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.bcancel = New DevExpress.XtraEditors.SimpleButton()
        Me.brefresh = New DevExpress.XtraEditors.SimpleButton()
        Me.chkdisable = New DevExpress.XtraEditors.CheckEdit()
        Me.lcont = New DevExpress.XtraEditors.LabelControl()
        Me.uxcont = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.lrefresh = New DevExpress.XtraEditors.LabelControl()
        Me.pslider = New DevExpress.XtraEditors.PanelControl()
        Me.Button3 = New DevExpress.XtraEditors.SimpleButton()
        Me.chkna = New DevExpress.XtraEditors.CheckEdit()
        Me.lrange = New DevExpress.XtraEditors.LabelControl()
        Me.trange = New DevExpress.XtraEditors.RangeTrackBarControl()
        CType(Me.uxparams, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemComboBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemTextEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemMemoEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemComboBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemImageComboBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemComboBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemTextEdit2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chkdisable.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxcont.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pslider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pslider.SuspendLayout()
        CType(Me.chkna.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.trange, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.trange.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'bok
        '
        Me.bok.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bok.ImageIndex = 4
        Me.bok.ImageList = Me.uxImages
        Me.bok.Location = New System.Drawing.Point(424, 410)
        Me.bok.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.bok.Name = "bok"
        Me.bok.Size = New System.Drawing.Size(75, 33)
        Me.bok.TabIndex = 64
        Me.bok.Text = "Continue"
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
        'uxparams
        '
        Me.uxparams.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxparams.BestFitVisibleOnly = True
        Me.uxparams.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Style3D
        Me.uxparams.Columns.AddRange(New DevExpress.XtraTreeList.Columns.TreeListColumn() {Me.Param, Me.Value})
        Me.uxparams.Font = New System.Drawing.Font("Tahoma", 8.25!)
        Me.uxparams.Location = New System.Drawing.Point(10, 12)
        Me.uxparams.Name = "uxparams"
        Me.uxparams.OptionsMenu.EnableColumnMenu = False
        Me.uxparams.OptionsMenu.EnableFooterMenu = False
        Me.uxparams.OptionsView.ShowColumns = False
        Me.uxparams.OptionsView.ShowIndicator = False
        Me.uxparams.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemTextEdit1, Me.RepositoryItemMemoEdit1, Me.RepositoryItemComboBox2, Me.RepositoryItemImageComboBox1, Me.RepositoryItemComboBox1, Me.RepositoryItemTextEdit2, Me.RepositoryItemComboBox3})
        Me.uxparams.RowHeight = 30
        Me.uxparams.Size = New System.Drawing.Size(489, 349)
        Me.uxparams.StateImageList = Me.tabimages
        Me.uxparams.TabIndex = 66
        Me.uxparams.TreeLevelWidth = 12
        Me.uxparams.TreeLineStyle = DevExpress.XtraTreeList.LineStyle.None
        '
        'Param
        '
        Me.Param.Caption = "Param"
        Me.Param.FieldName = "Param"
        Me.Param.MinWidth = 59
        Me.Param.Name = "Param"
        Me.Param.OptionsColumn.AllowEdit = False
        Me.Param.Visible = True
        Me.Param.VisibleIndex = 0
        Me.Param.Width = 300
        '
        'Value
        '
        Me.Value.Caption = "Value"
        Me.Value.ColumnEdit = Me.RepositoryItemComboBox3
        Me.Value.FieldName = "Value"
        Me.Value.Name = "Value"
        Me.Value.Visible = True
        Me.Value.VisibleIndex = 1
        Me.Value.Width = 300
        '
        'RepositoryItemComboBox3
        '
        Me.RepositoryItemComboBox3.AutoHeight = False
        Me.RepositoryItemComboBox3.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.RepositoryItemComboBox3.Name = "RepositoryItemComboBox3"
        Me.RepositoryItemComboBox3.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        '
        'RepositoryItemTextEdit1
        '
        Me.RepositoryItemTextEdit1.AutoHeight = False
        Me.RepositoryItemTextEdit1.Name = "RepositoryItemTextEdit1"
        '
        'RepositoryItemMemoEdit1
        '
        Me.RepositoryItemMemoEdit1.Name = "RepositoryItemMemoEdit1"
        Me.RepositoryItemMemoEdit1.ReadOnly = True
        '
        'RepositoryItemComboBox2
        '
        Me.RepositoryItemComboBox2.AutoHeight = False
        Me.RepositoryItemComboBox2.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.RepositoryItemComboBox2.Name = "RepositoryItemComboBox2"
        '
        'RepositoryItemImageComboBox1
        '
        Me.RepositoryItemImageComboBox1.AutoHeight = False
        Me.RepositoryItemImageComboBox1.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.RepositoryItemImageComboBox1.Name = "RepositoryItemImageComboBox1"
        '
        'RepositoryItemComboBox1
        '
        Me.RepositoryItemComboBox1.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.[True]
        Me.RepositoryItemComboBox1.AutoHeight = False
        Me.RepositoryItemComboBox1.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.RepositoryItemComboBox1.Name = "RepositoryItemComboBox1"
        '
        'RepositoryItemTextEdit2
        '
        Me.RepositoryItemTextEdit2.AutoHeight = False
        Me.RepositoryItemTextEdit2.Name = "RepositoryItemTextEdit2"
        '
        'tabimages
        '
        Me.tabimages.ImageStream = CType(resources.GetObject("tabimages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.tabimages.TransparentColor = System.Drawing.Color.Transparent
        Me.tabimages.Images.SetKeyName(0, "data")
        Me.tabimages.Images.SetKeyName(1, "mandatory")
        Me.tabimages.Images.SetKeyName(2, "Revert_disabled.png")
        '
        'PictureBox2
        '
        Me.PictureBox2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(10, 409)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(161, 33)
        Me.PictureBox2.TabIndex = 67
        Me.PictureBox2.TabStop = False
        '
        'bcancel
        '
        Me.bcancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bcancel.ImageIndex = 2
        Me.bcancel.ImageList = Me.uxImages
        Me.bcancel.Location = New System.Drawing.Point(240, 410)
        Me.bcancel.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.bcancel.Name = "bcancel"
        Me.bcancel.Size = New System.Drawing.Size(75, 33)
        Me.bcancel.TabIndex = 68
        Me.bcancel.Text = "Cancel"
        Me.bcancel.Visible = False
        '
        'brefresh
        '
        Me.brefresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.brefresh.ImageIndex = 1
        Me.brefresh.ImageList = Me.uxImages
        Me.brefresh.Location = New System.Drawing.Point(331, 410)
        Me.brefresh.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.brefresh.Name = "brefresh"
        Me.brefresh.Size = New System.Drawing.Size(75, 33)
        Me.brefresh.TabIndex = 69
        Me.brefresh.Text = "Refresh"
        Me.brefresh.Visible = False
        '
        'chkdisable
        '
        Me.chkdisable.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkdisable.Location = New System.Drawing.Point(8, 374)
        Me.chkdisable.Name = "chkdisable"
        Me.chkdisable.Properties.Caption = "Disable"
        Me.chkdisable.Size = New System.Drawing.Size(74, 19)
        Me.chkdisable.TabIndex = 84
        Me.chkdisable.Visible = False
        '
        'lcont
        '
        Me.lcont.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lcont.Location = New System.Drawing.Point(251, 376)
        Me.lcont.Name = "lcont"
        Me.lcont.Size = New System.Drawing.Size(55, 13)
        Me.lcont.TabIndex = 86
        Me.lcont.Text = "Contributor"
        Me.lcont.Visible = False
        '
        'uxcont
        '
        Me.uxcont.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.uxcont.EditValue = ""
        Me.uxcont.Location = New System.Drawing.Point(312, 373)
        Me.uxcont.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.uxcont.Name = "uxcont"
        Me.uxcont.Properties.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.[True]
        Me.uxcont.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.uxcont.Properties.MaxLength = 250
        Me.uxcont.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        Me.uxcont.Size = New System.Drawing.Size(187, 20)
        Me.uxcont.TabIndex = 85
        Me.uxcont.Visible = False
        '
        'lrefresh
        '
        Me.lrefresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lrefresh.Location = New System.Drawing.Point(72, 376)
        Me.lrefresh.Name = "lrefresh"
        Me.lrefresh.Size = New System.Drawing.Size(61, 13)
        Me.lrefresh.TabIndex = 87
        Me.lrefresh.Text = "Last Refresh"
        Me.lrefresh.Visible = False
        '
        'pslider
        '
        Me.pslider.Controls.Add(Me.Button3)
        Me.pslider.Controls.Add(Me.chkna)
        Me.pslider.Controls.Add(Me.lrange)
        Me.pslider.Controls.Add(Me.trange)
        Me.pslider.Location = New System.Drawing.Point(12, 12)
        Me.pslider.Name = "pslider"
        Me.pslider.Size = New System.Drawing.Size(487, 105)
        Me.pslider.TabIndex = 88
        Me.pslider.Visible = False
        '
        'Button3
        '
        Me.Button3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button3.ImageIndex = 2
        Me.Button3.ImageList = Me.uxImages
        Me.Button3.Location = New System.Drawing.Point(407, 67)
        Me.Button3.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(75, 33)
        Me.Button3.TabIndex = 89
        Me.Button3.Text = "Reset"
        '
        'chkna
        '
        Me.chkna.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkna.Location = New System.Drawing.Point(5, 81)
        Me.chkna.Name = "chkna"
        Me.chkna.Properties.Caption = "n/a"
        Me.chkna.Size = New System.Drawing.Size(74, 19)
        Me.chkna.TabIndex = 85
        '
        'lrange
        '
        Me.lrange.Location = New System.Drawing.Point(228, 69)
        Me.lrange.Name = "lrange"
        Me.lrange.Size = New System.Drawing.Size(0, 13)
        Me.lrange.TabIndex = 1
        '
        'trange
        '
        Me.trange.EditValue = New DevExpress.XtraEditors.Repository.TrackBarRange(4, 4)
        Me.trange.Location = New System.Drawing.Point(16, 18)
        Me.trange.Name = "trange"
        Me.trange.Properties.LabelAppearance.Options.UseTextOptions = True
        Me.trange.Properties.LabelAppearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.trange.Properties.Maximum = 20
        Me.trange.Properties.ShowLabels = True
        Me.trange.Size = New System.Drawing.Size(455, 45)
        Me.trange.TabIndex = 0
        Me.trange.Value = New DevExpress.XtraEditors.Repository.TrackBarRange(4, 4)
        '
        'view_bc_dx_build_parameters
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(511, 445)
        Me.Controls.Add(Me.pslider)
        Me.Controls.Add(Me.lrefresh)
        Me.Controls.Add(Me.lcont)
        Me.Controls.Add(Me.uxcont)
        Me.Controls.Add(Me.chkdisable)
        Me.Controls.Add(Me.brefresh)
        Me.Controls.Add(Me.bcancel)
        Me.Controls.Add(Me.PictureBox2)
        Me.Controls.Add(Me.uxparams)
        Me.Controls.Add(Me.bok)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "view_bc_dx_build_parameters"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Blue Curve - Document Parameters"
        Me.TopMost = True
        CType(Me.uxparams, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemComboBox3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemTextEdit1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemMemoEdit1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemComboBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemImageComboBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemComboBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemTextEdit2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chkdisable.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxcont.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pslider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pslider.ResumeLayout(False)
        Me.pslider.PerformLayout()
        CType(Me.chkna.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.trange.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.trange, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents bok As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents uxparams As DevExpress.XtraTreeList.TreeList
    Friend WithEvents RepositoryItemMemoEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit
    Friend WithEvents Param As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents Value As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents RepositoryItemTextEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemTextEdit
    Friend WithEvents RepositoryItemComboBox2 As DevExpress.XtraEditors.Repository.RepositoryItemComboBox
    Friend WithEvents RepositoryItemImageComboBox1 As DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox
    Friend WithEvents tabimages As System.Windows.Forms.ImageList
    Friend WithEvents RepositoryItemComboBox1 As DevExpress.XtraEditors.Repository.RepositoryItemComboBox
    Friend WithEvents RepositoryItemTextEdit2 As DevExpress.XtraEditors.Repository.RepositoryItemTextEdit
    Friend WithEvents RepositoryItemComboBox3 As DevExpress.XtraEditors.Repository.RepositoryItemComboBox
    Friend WithEvents uxImages As System.Windows.Forms.ImageList
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents bcancel As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents brefresh As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents chkdisable As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents lcont As DevExpress.XtraEditors.LabelControl
    Friend WithEvents uxcont As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents lrefresh As DevExpress.XtraEditors.LabelControl
    Friend WithEvents pslider As DevExpress.XtraEditors.PanelControl
    Friend WithEvents lrange As DevExpress.XtraEditors.LabelControl
    Friend WithEvents trange As DevExpress.XtraEditors.RangeTrackBarControl
    Friend WithEvents Button3 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents chkna As DevExpress.XtraEditors.CheckEdit
End Class
