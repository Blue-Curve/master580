<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bc_ax_adv_doc_refresh
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_ax_adv_doc_refresh))
        Me.brefresh = New DevExpress.XtraEditors.SimpleButton()
        Me.uxImages = New System.Windows.Forms.ImageList()
        Me.bcancel = New DevExpress.XtraEditors.SimpleButton()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.bok = New DevExpress.XtraEditors.SimpleButton()
        Me.uxcomps = New DevExpress.XtraTreeList.TreeList()
        Me.TreeListColumn7 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.TreeListColumn4 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.RepositoryItemCheckEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit()
        Me.TreeListColumn2 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.TreeListColumn8 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.TreeListColumn9 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.RepositoryItemCheckEdit2 = New DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit()
        Me.Filter = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.TreeListColumn1 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.TreeListColumn3 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.RepositoryItemComboBox1 = New DevExpress.XtraEditors.Repository.RepositoryItemComboBox()
        Me.RepositoryItemButtonEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit()
        Me.RepositoryItemButtonEdit2 = New DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit()
        Me.RepositoryItemImageComboBox1 = New DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox()
        Me.uxstage = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.uxlang = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.LabelControl7 = New DevExpress.XtraEditors.LabelControl()
        Me.LabelControl1 = New DevExpress.XtraEditors.LabelControl()
        Me.LabelControl2 = New DevExpress.XtraEditors.LabelControl()
        Me.uxdate = New DevExpress.XtraEditors.DateEdit()
        Me.uxcurrent = New DevExpress.XtraEditors.CheckEdit()
        Me.uxparams = New DevExpress.XtraEditors.SimpleButton()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxcomps, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemCheckEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemCheckEdit2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemComboBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemButtonEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemButtonEdit2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemImageComboBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxstage.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxlang.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxdate.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxdate.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxcurrent.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'brefresh
        '
        Me.brefresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.brefresh.ImageIndex = 5
        Me.brefresh.ImageList = Me.uxImages
        Me.brefresh.Location = New System.Drawing.Point(672, 453)
        Me.brefresh.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.brefresh.Name = "brefresh"
        Me.brefresh.Size = New System.Drawing.Size(75, 33)
        Me.brefresh.TabIndex = 73
        Me.brefresh.Text = "Refresh"
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
        'bcancel
        '
        Me.bcancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bcancel.ImageIndex = 2
        Me.bcancel.ImageList = Me.uxImages
        Me.bcancel.Location = New System.Drawing.Point(578, 453)
        Me.bcancel.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.bcancel.Name = "bcancel"
        Me.bcancel.Size = New System.Drawing.Size(75, 33)
        Me.bcancel.TabIndex = 72
        Me.bcancel.Text = "Cancel"
        '
        'PictureBox2
        '
        Me.PictureBox2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(21, 453)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(161, 33)
        Me.PictureBox2.TabIndex = 71
        Me.PictureBox2.TabStop = False
        '
        'bok
        '
        Me.bok.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bok.ImageIndex = 4
        Me.bok.ImageList = Me.uxImages
        Me.bok.Location = New System.Drawing.Point(763, 453)
        Me.bok.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.bok.Name = "bok"
        Me.bok.Size = New System.Drawing.Size(75, 33)
        Me.bok.TabIndex = 70
        Me.bok.Text = "Save"
        '
        'uxcomps
        '
        Me.uxcomps.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxcomps.BestFitVisibleOnly = True
        Me.uxcomps.Columns.AddRange(New DevExpress.XtraTreeList.Columns.TreeListColumn() {Me.TreeListColumn7, Me.TreeListColumn4, Me.TreeListColumn2, Me.TreeListColumn8, Me.TreeListColumn9, Me.Filter, Me.TreeListColumn1, Me.TreeListColumn3})
        Me.uxcomps.Location = New System.Drawing.Point(12, 28)
        Me.uxcomps.Name = "uxcomps"
        Me.uxcomps.OptionsBehavior.EnableFiltering = True
        Me.uxcomps.OptionsFilter.ShowAllValuesInFilterPopup = True
        Me.uxcomps.OptionsMenu.EnableColumnMenu = False
        Me.uxcomps.OptionsMenu.EnableFooterMenu = False
        Me.uxcomps.OptionsSelection.EnableAppearanceFocusedCell = False
        Me.uxcomps.OptionsView.ShowFocusedFrame = False
        Me.uxcomps.OptionsView.ShowHorzLines = False
        Me.uxcomps.OptionsView.ShowIndicator = False
        Me.uxcomps.OptionsView.ShowVertLines = False
        Me.uxcomps.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemButtonEdit1, Me.RepositoryItemCheckEdit1, Me.RepositoryItemButtonEdit2, Me.RepositoryItemCheckEdit2, Me.RepositoryItemComboBox1, Me.RepositoryItemImageComboBox1})
        Me.uxcomps.Size = New System.Drawing.Size(826, 342)
        Me.uxcomps.TabIndex = 75
        Me.uxcomps.TreeLevelWidth = 12
        Me.uxcomps.TreeLineStyle = DevExpress.XtraTreeList.LineStyle.None
        '
        'TreeListColumn7
        '
        Me.TreeListColumn7.Caption = "Component"
        Me.TreeListColumn7.FieldName = "TreeListColumn7"
        Me.TreeListColumn7.ImageIndex = 5
        Me.TreeListColumn7.Name = "TreeListColumn7"
        Me.TreeListColumn7.OptionsColumn.ReadOnly = True
        Me.TreeListColumn7.OptionsFilter.AllowFilter = False
        Me.TreeListColumn7.Visible = True
        Me.TreeListColumn7.VisibleIndex = 1
        Me.TreeListColumn7.Width = 148
        '
        'TreeListColumn4
        '
        Me.TreeListColumn4.Caption = "Refresh"
        Me.TreeListColumn4.ColumnEdit = Me.RepositoryItemCheckEdit1
        Me.TreeListColumn4.FieldName = "Stage"
        Me.TreeListColumn4.ImageIndex = 7
        Me.TreeListColumn4.MinWidth = 33
        Me.TreeListColumn4.Name = "TreeListColumn4"
        Me.TreeListColumn4.OptionsFilter.AllowFilter = False
        Me.TreeListColumn4.Visible = True
        Me.TreeListColumn4.VisibleIndex = 0
        Me.TreeListColumn4.Width = 52
        '
        'RepositoryItemCheckEdit1
        '
        Me.RepositoryItemCheckEdit1.AutoHeight = False
        Me.RepositoryItemCheckEdit1.Caption = "Check"
        Me.RepositoryItemCheckEdit1.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio
        Me.RepositoryItemCheckEdit1.Name = "RepositoryItemCheckEdit1"
        '
        'TreeListColumn2
        '
        Me.TreeListColumn2.Caption = "Type"
        Me.TreeListColumn2.FieldName = "Type"
        Me.TreeListColumn2.Name = "TreeListColumn2"
        Me.TreeListColumn2.OptionsColumn.ReadOnly = True
        Me.TreeListColumn2.Visible = True
        Me.TreeListColumn2.VisibleIndex = 2
        Me.TreeListColumn2.Width = 90
        '
        'TreeListColumn8
        '
        Me.TreeListColumn8.Caption = "Params"
        Me.TreeListColumn8.FieldName = "TreeListColumn8"
        Me.TreeListColumn8.ImageIndex = 24
        Me.TreeListColumn8.Name = "TreeListColumn8"
        Me.TreeListColumn8.OptionsColumn.ReadOnly = True
        Me.TreeListColumn8.OptionsFilter.FilterPopupMode = DevExpress.XtraTreeList.FilterPopupMode.CheckedList
        Me.TreeListColumn8.Visible = True
        Me.TreeListColumn8.VisibleIndex = 3
        Me.TreeListColumn8.Width = 69
        '
        'TreeListColumn9
        '
        Me.TreeListColumn9.Caption = "Disabled"
        Me.TreeListColumn9.ColumnEdit = Me.RepositoryItemCheckEdit2
        Me.TreeListColumn9.FieldName = "TreeListColumn9"
        Me.TreeListColumn9.ImageIndex = 12
        Me.TreeListColumn9.Name = "TreeListColumn9"
        Me.TreeListColumn9.OptionsColumn.AllowSort = False
        Me.TreeListColumn9.OptionsFilter.FilterPopupMode = DevExpress.XtraTreeList.FilterPopupMode.CheckedList
        Me.TreeListColumn9.Visible = True
        Me.TreeListColumn9.VisibleIndex = 4
        Me.TreeListColumn9.Width = 71
        '
        'RepositoryItemCheckEdit2
        '
        Me.RepositoryItemCheckEdit2.AutoHeight = False
        Me.RepositoryItemCheckEdit2.Caption = "Check"
        Me.RepositoryItemCheckEdit2.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio
        Me.RepositoryItemCheckEdit2.Name = "RepositoryItemCheckEdit2"
        '
        'Filter
        '
        Me.Filter.Caption = "Last Refresh"
        Me.Filter.FieldName = "Filter"
        Me.Filter.ImageIndex = 16
        Me.Filter.Name = "Filter"
        Me.Filter.OptionsColumn.AllowSort = False
        Me.Filter.OptionsColumn.ReadOnly = True
        Me.Filter.OptionsFilter.FilterPopupMode = DevExpress.XtraTreeList.FilterPopupMode.CheckedList
        Me.Filter.Visible = True
        Me.Filter.VisibleIndex = 5
        Me.Filter.Width = 193
        '
        'TreeListColumn1
        '
        Me.TreeListColumn1.Caption = "Last Update"
        Me.TreeListColumn1.FieldName = "Last Update"
        Me.TreeListColumn1.Name = "TreeListColumn1"
        Me.TreeListColumn1.OptionsColumn.ReadOnly = True
        Me.TreeListColumn1.Visible = True
        Me.TreeListColumn1.VisibleIndex = 6
        Me.TreeListColumn1.Width = 149
        '
        'TreeListColumn3
        '
        Me.TreeListColumn3.Caption = "Contributor"
        Me.TreeListColumn3.ColumnEdit = Me.RepositoryItemComboBox1
        Me.TreeListColumn3.FieldName = "Contributor"
        Me.TreeListColumn3.Name = "TreeListColumn3"
        Me.TreeListColumn3.Visible = True
        Me.TreeListColumn3.VisibleIndex = 7
        Me.TreeListColumn3.Width = 109
        '
        'RepositoryItemComboBox1
        '
        Me.RepositoryItemComboBox1.AutoHeight = False
        Me.RepositoryItemComboBox1.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.RepositoryItemComboBox1.Name = "RepositoryItemComboBox1"
        Me.RepositoryItemComboBox1.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        '
        'RepositoryItemButtonEdit1
        '
        Me.RepositoryItemButtonEdit1.AutoHeight = False
        Me.RepositoryItemButtonEdit1.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
        Me.RepositoryItemButtonEdit1.Name = "RepositoryItemButtonEdit1"
        '
        'RepositoryItemButtonEdit2
        '
        Me.RepositoryItemButtonEdit2.AutoHeight = False
        Me.RepositoryItemButtonEdit2.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
        Me.RepositoryItemButtonEdit2.Name = "RepositoryItemButtonEdit2"
        '
        'RepositoryItemImageComboBox1
        '
        Me.RepositoryItemImageComboBox1.AutoHeight = False
        Me.RepositoryItemImageComboBox1.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.RepositoryItemImageComboBox1.Items.AddRange(New DevExpress.XtraEditors.Controls.ImageComboBoxItem() {New DevExpress.XtraEditors.Controls.ImageComboBoxItem("", CType(-1, Short), -1), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("", CType(0, Short), 1)})
        Me.RepositoryItemImageComboBox1.Name = "RepositoryItemImageComboBox1"
        Me.RepositoryItemImageComboBox1.SmallImages = Me.uxImages
        '
        'uxstage
        '
        Me.uxstage.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxstage.EditValue = ""
        Me.uxstage.Location = New System.Drawing.Point(436, 406)
        Me.uxstage.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.uxstage.Name = "uxstage"
        Me.uxstage.Properties.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.[True]
        Me.uxstage.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.uxstage.Properties.Items.AddRange(New Object() {"Draft", "Publish"})
        Me.uxstage.Properties.MaxLength = 250
        Me.uxstage.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        Me.uxstage.Size = New System.Drawing.Size(187, 20)
        Me.uxstage.TabIndex = 76
        '
        'uxlang
        '
        Me.uxlang.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxlang.EditValue = ""
        Me.uxlang.Location = New System.Drawing.Point(651, 406)
        Me.uxlang.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.uxlang.Name = "uxlang"
        Me.uxlang.Properties.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.[True]
        Me.uxlang.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.uxlang.Properties.MaxLength = 250
        Me.uxlang.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        Me.uxlang.Size = New System.Drawing.Size(187, 20)
        Me.uxlang.TabIndex = 77
        '
        'LabelControl7
        '
        Me.LabelControl7.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LabelControl7.Location = New System.Drawing.Point(651, 388)
        Me.LabelControl7.Name = "LabelControl7"
        Me.LabelControl7.Size = New System.Drawing.Size(47, 13)
        Me.LabelControl7.TabIndex = 79
        Me.LabelControl7.Text = "Language"
        '
        'LabelControl1
        '
        Me.LabelControl1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LabelControl1.Location = New System.Drawing.Point(436, 388)
        Me.LabelControl1.Name = "LabelControl1"
        Me.LabelControl1.Size = New System.Drawing.Size(54, 13)
        Me.LabelControl1.TabIndex = 80
        Me.LabelControl1.Text = "Data Stage"
        '
        'LabelControl2
        '
        Me.LabelControl2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LabelControl2.Location = New System.Drawing.Point(225, 388)
        Me.LabelControl2.Name = "LabelControl2"
        Me.LabelControl2.Size = New System.Drawing.Size(63, 13)
        Me.LabelControl2.TabIndex = 81
        Me.LabelControl2.Text = "Data At Date"
        '
        'uxdate
        '
        Me.uxdate.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxdate.EditValue = New Date(2013, 10, 10, 0, 0, 0, 0)
        Me.uxdate.Location = New System.Drawing.Point(303, 406)
        Me.uxdate.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.uxdate.Name = "uxdate"
        Me.uxdate.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.uxdate.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
        Me.uxdate.Size = New System.Drawing.Size(88, 20)
        Me.uxdate.TabIndex = 82
        '
        'uxcurrent
        '
        Me.uxcurrent.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxcurrent.Location = New System.Drawing.Point(223, 407)
        Me.uxcurrent.Name = "uxcurrent"
        Me.uxcurrent.Properties.Caption = "Current"
        Me.uxcurrent.Size = New System.Drawing.Size(74, 19)
        Me.uxcurrent.TabIndex = 83
        '
        'uxparams
        '
        Me.uxparams.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxparams.Enabled = False
        Me.uxparams.ImageIndex = 1
        Me.uxparams.ImageList = Me.uxImages
        Me.uxparams.Location = New System.Drawing.Point(486, 453)
        Me.uxparams.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.uxparams.Name = "uxparams"
        Me.uxparams.Size = New System.Drawing.Size(75, 33)
        Me.uxparams.TabIndex = 84
        Me.uxparams.Text = "Params"
        '
        'bc_ax_adv_doc_refresh
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(850, 497)
        Me.Controls.Add(Me.uxparams)
        Me.Controls.Add(Me.uxcurrent)
        Me.Controls.Add(Me.uxdate)
        Me.Controls.Add(Me.LabelControl2)
        Me.Controls.Add(Me.LabelControl1)
        Me.Controls.Add(Me.LabelControl7)
        Me.Controls.Add(Me.uxlang)
        Me.Controls.Add(Me.uxstage)
        Me.Controls.Add(Me.uxcomps)
        Me.Controls.Add(Me.brefresh)
        Me.Controls.Add(Me.bcancel)
        Me.Controls.Add(Me.PictureBox2)
        Me.Controls.Add(Me.bok)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimizeBox = False
        Me.Name = "bc_ax_adv_doc_refresh"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Blue Curve - Advanced Document Refresh"
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxcomps, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemCheckEdit1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemCheckEdit2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemComboBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemButtonEdit1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemButtonEdit2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemImageComboBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxstage.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxlang.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxdate.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxdate.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxcurrent.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents brefresh As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents bcancel As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents bok As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents uxImages As System.Windows.Forms.ImageList
    Friend WithEvents uxcomps As DevExpress.XtraTreeList.TreeList
    Friend WithEvents TreeListColumn4 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents TreeListColumn7 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents TreeListColumn8 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents TreeListColumn9 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents Filter As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents uxstage As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents uxlang As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents TreeListColumn2 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents TreeListColumn1 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents RepositoryItemButtonEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit
    Friend WithEvents LabelControl7 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents TreeListColumn3 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents LabelControl2 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents uxdate As DevExpress.XtraEditors.DateEdit
    Friend WithEvents uxcurrent As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents RepositoryItemCheckEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit
    Friend WithEvents RepositoryItemButtonEdit2 As DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit
    Friend WithEvents RepositoryItemCheckEdit2 As DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit
    Friend WithEvents RepositoryItemComboBox1 As DevExpress.XtraEditors.Repository.RepositoryItemComboBox
    Friend WithEvents RepositoryItemImageComboBox1 As DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox
    Friend WithEvents uxparams As DevExpress.XtraEditors.SimpleButton
End Class
