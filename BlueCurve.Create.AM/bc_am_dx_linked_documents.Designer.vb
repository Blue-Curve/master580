<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bc_am_dx_linked_documents
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_dx_linked_documents))
        Me.bcancel = New DevExpress.XtraEditors.SimpleButton()
        Me.uxImages = New System.Windows.Forms.ImageList()
        Me.bsave = New DevExpress.XtraEditors.SimpleButton()
        Me.lvall = New DevExpress.XtraTreeList.TreeList()
        Me.TreeListColumn12 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.TreeListColumn6 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.TreeListColumn14 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.TreeListColumn3 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.TreeListColumn1 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.TreeListColumn10 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.RepositoryItemDateEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemDateEdit()
        Me.RepositoryItemDateEdit2 = New DevExpress.XtraEditors.Repository.RepositoryItemDateEdit()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.lvsel = New DevExpress.XtraTreeList.TreeList()
        Me.TreeListColumn2 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.TreeListColumn4 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.TreeListColumn5 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.TreeListColumn7 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.TreeListColumn8 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.TreeListColumn9 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.uxMoveDown = New DevExpress.XtraEditors.SimpleButton()
        Me.uximage = New DevExpress.Utils.ImageCollection()
        Me.uxMoveUp = New DevExpress.XtraEditors.SimpleButton()
        Me.dfrom = New DevExpress.XtraEditors.DateEdit()
        Me.dto = New DevExpress.XtraEditors.DateEdit()
        Me.RadioGroup1 = New DevExpress.XtraEditors.RadioGroup()
        Me.ldesc = New DevExpress.XtraEditors.LabelControl()
        Me.LabelControl1 = New DevExpress.XtraEditors.LabelControl()
        Me.LabelControl2 = New DevExpress.XtraEditors.LabelControl()
        Me.lres = New DevExpress.XtraEditors.LabelControl()
        Me.LabelControl4 = New DevExpress.XtraEditors.LabelControl()
        Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl()
        Me.lauthor = New DevExpress.XtraEditors.LabelControl()
        Me.Ltype = New DevExpress.XtraEditors.LabelControl()
        Me.cbotype = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.cboauthor = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.cbopropentity = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.cboentity = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.cbopropclass = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.cboclass = New DevExpress.XtraEditors.ComboBoxEdit()
        CType(Me.lvall, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemDateEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemDateEdit1.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemDateEdit2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemDateEdit2.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lvsel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uximage, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dfrom.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dfrom.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dto.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dto.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadioGroup1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl1.SuspendLayout()
        CType(Me.cbotype.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cboauthor.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cbopropentity.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cboentity.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cbopropclass.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cboclass.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'bcancel
        '
        Me.bcancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bcancel.ImageIndex = 2
        Me.bcancel.ImageList = Me.uxImages
        Me.bcancel.Location = New System.Drawing.Point(774, 374)
        Me.bcancel.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.bcancel.Name = "bcancel"
        Me.bcancel.Size = New System.Drawing.Size(75, 33)
        Me.bcancel.TabIndex = 5
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
        'bsave
        '
        Me.bsave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bsave.ImageIndex = 4
        Me.bsave.ImageList = Me.uxImages
        Me.bsave.Location = New System.Drawing.Point(862, 374)
        Me.bsave.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.bsave.Name = "bsave"
        Me.bsave.Size = New System.Drawing.Size(75, 33)
        Me.bsave.TabIndex = 4
        Me.bsave.Text = "Save"
        '
        'lvall
        '
        Me.lvall.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvall.Appearance.SelectedRow.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvall.Appearance.SelectedRow.Options.UseFont = True
        Me.lvall.BestFitVisibleOnly = True
        Me.lvall.Columns.AddRange(New DevExpress.XtraTreeList.Columns.TreeListColumn() {Me.TreeListColumn12, Me.TreeListColumn6, Me.TreeListColumn14, Me.TreeListColumn3, Me.TreeListColumn1, Me.TreeListColumn10})
        Me.lvall.Location = New System.Drawing.Point(12, 155)
        Me.lvall.Name = "lvall"
        Me.lvall.OptionsFilter.ShowAllValuesInFilterPopup = True
        Me.lvall.OptionsMenu.EnableColumnMenu = False
        Me.lvall.OptionsMenu.EnableFooterMenu = False
        Me.lvall.OptionsSelection.EnableAppearanceFocusedCell = False
        Me.lvall.OptionsView.ShowHorzLines = False
        Me.lvall.OptionsView.ShowIndicator = False
        Me.lvall.OptionsView.ShowVertLines = False
        Me.lvall.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemDateEdit1, Me.RepositoryItemDateEdit2})
        Me.lvall.Size = New System.Drawing.Size(525, 199)
        Me.lvall.TabIndex = 1
        Me.lvall.TreeLineStyle = DevExpress.XtraTreeList.LineStyle.None
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
        Me.TreeListColumn12.VisibleIndex = 0
        Me.TreeListColumn12.Width = 147
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
        Me.TreeListColumn6.VisibleIndex = 1
        Me.TreeListColumn6.Width = 95
        '
        'TreeListColumn14
        '
        Me.TreeListColumn14.Caption = "Type"
        Me.TreeListColumn14.FieldName = "Type"
        Me.TreeListColumn14.Name = "TreeListColumn14"
        Me.TreeListColumn14.OptionsColumn.AllowEdit = False
        Me.TreeListColumn14.OptionsFilter.FilterPopupMode = DevExpress.XtraTreeList.FilterPopupMode.CheckedList
        Me.TreeListColumn14.Visible = True
        Me.TreeListColumn14.VisibleIndex = 2
        Me.TreeListColumn14.Width = 41
        '
        'TreeListColumn3
        '
        Me.TreeListColumn3.Caption = "Author"
        Me.TreeListColumn3.FieldName = "Author"
        Me.TreeListColumn3.Name = "TreeListColumn3"
        Me.TreeListColumn3.OptionsColumn.AllowEdit = False
        Me.TreeListColumn3.OptionsFilter.FilterPopupMode = DevExpress.XtraTreeList.FilterPopupMode.CheckedList
        Me.TreeListColumn3.Visible = True
        Me.TreeListColumn3.VisibleIndex = 3
        Me.TreeListColumn3.Width = 28
        '
        'TreeListColumn1
        '
        Me.TreeListColumn1.Caption = "Entity"
        Me.TreeListColumn1.FieldName = "Item"
        Me.TreeListColumn1.Name = "TreeListColumn1"
        Me.TreeListColumn1.OptionsColumn.AllowEdit = False
        Me.TreeListColumn1.OptionsFilter.FilterPopupMode = DevExpress.XtraTreeList.FilterPopupMode.CheckedList
        Me.TreeListColumn1.Visible = True
        Me.TreeListColumn1.VisibleIndex = 4
        Me.TreeListColumn1.Width = 47
        '
        'TreeListColumn10
        '
        Me.TreeListColumn10.Caption = "extension"
        Me.TreeListColumn10.FieldName = "extension"
        Me.TreeListColumn10.Name = "TreeListColumn10"
        '
        'RepositoryItemDateEdit1
        '
        Me.RepositoryItemDateEdit1.AutoHeight = False
        Me.RepositoryItemDateEdit1.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.RepositoryItemDateEdit1.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.RepositoryItemDateEdit1.Name = "RepositoryItemDateEdit1"
        '
        'RepositoryItemDateEdit2
        '
        Me.RepositoryItemDateEdit2.AutoHeight = False
        Me.RepositoryItemDateEdit2.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.RepositoryItemDateEdit2.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.RepositoryItemDateEdit2.Name = "RepositoryItemDateEdit2"
        '
        'PictureBox2
        '
        Me.PictureBox2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(12, 373)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(161, 33)
        Me.PictureBox2.TabIndex = 71
        Me.PictureBox2.TabStop = False
        '
        'lvsel
        '
        Me.lvsel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvsel.Appearance.SelectedRow.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvsel.Appearance.SelectedRow.Options.UseFont = True
        Me.lvsel.BestFitVisibleOnly = True
        Me.lvsel.Columns.AddRange(New DevExpress.XtraTreeList.Columns.TreeListColumn() {Me.TreeListColumn2, Me.TreeListColumn4, Me.TreeListColumn5, Me.TreeListColumn7, Me.TreeListColumn8, Me.TreeListColumn9})
        Me.lvsel.Location = New System.Drawing.Point(568, 155)
        Me.lvsel.Name = "lvsel"
        Me.lvsel.OptionsBehavior.Editable = False
        Me.lvsel.OptionsFilter.AllowColumnMRUFilterList = False
        Me.lvsel.OptionsFilter.AllowFilterEditor = False
        Me.lvsel.OptionsFilter.AllowMRUFilterList = False
        Me.lvsel.OptionsFilter.ShowAllValuesInFilterPopup = True
        Me.lvsel.OptionsMenu.EnableColumnMenu = False
        Me.lvsel.OptionsMenu.EnableFooterMenu = False
        Me.lvsel.OptionsSelection.EnableAppearanceFocusedCell = False
        Me.lvsel.OptionsView.ShowHorzLines = False
        Me.lvsel.OptionsView.ShowIndicator = False
        Me.lvsel.OptionsView.ShowVertLines = False
        Me.lvsel.Size = New System.Drawing.Size(369, 199)
        Me.lvsel.TabIndex = 72
        Me.lvsel.TreeLineStyle = DevExpress.XtraTreeList.LineStyle.None
        '
        'TreeListColumn2
        '
        Me.TreeListColumn2.Caption = "Title"
        Me.TreeListColumn2.FieldName = "Title"
        Me.TreeListColumn2.ImageIndex = 6
        Me.TreeListColumn2.Name = "TreeListColumn2"
        Me.TreeListColumn2.OptionsColumn.AllowEdit = False
        Me.TreeListColumn2.OptionsColumn.AllowSort = False
        Me.TreeListColumn2.OptionsFilter.FilterPopupMode = DevExpress.XtraTreeList.FilterPopupMode.CheckedList
        Me.TreeListColumn2.Visible = True
        Me.TreeListColumn2.VisibleIndex = 0
        Me.TreeListColumn2.Width = 119
        '
        'TreeListColumn4
        '
        Me.TreeListColumn4.Caption = "Date"
        Me.TreeListColumn4.FieldName = "Stage"
        Me.TreeListColumn4.Format.FormatString = "dd-MMM-yyy HH:mm:ss"
        Me.TreeListColumn4.Format.FormatType = DevExpress.Utils.FormatType.DateTime
        Me.TreeListColumn4.ImageIndex = 24
        Me.TreeListColumn4.MinWidth = 49
        Me.TreeListColumn4.Name = "TreeListColumn4"
        Me.TreeListColumn4.OptionsColumn.AllowEdit = False
        Me.TreeListColumn4.OptionsColumn.AllowSort = False
        Me.TreeListColumn4.OptionsFilter.FilterPopupMode = DevExpress.XtraTreeList.FilterPopupMode.CheckedList
        Me.TreeListColumn4.UnboundType = DevExpress.XtraTreeList.Data.UnboundColumnType.DateTime
        Me.TreeListColumn4.Visible = True
        Me.TreeListColumn4.VisibleIndex = 1
        Me.TreeListColumn4.Width = 89
        '
        'TreeListColumn5
        '
        Me.TreeListColumn5.Caption = "Type"
        Me.TreeListColumn5.FieldName = "Type"
        Me.TreeListColumn5.Name = "TreeListColumn5"
        Me.TreeListColumn5.OptionsColumn.AllowEdit = False
        Me.TreeListColumn5.OptionsColumn.AllowSort = False
        Me.TreeListColumn5.OptionsFilter.FilterPopupMode = DevExpress.XtraTreeList.FilterPopupMode.CheckedList
        Me.TreeListColumn5.Visible = True
        Me.TreeListColumn5.VisibleIndex = 2
        Me.TreeListColumn5.Width = 56
        '
        'TreeListColumn7
        '
        Me.TreeListColumn7.Caption = "Author"
        Me.TreeListColumn7.FieldName = "Author"
        Me.TreeListColumn7.Name = "TreeListColumn7"
        Me.TreeListColumn7.OptionsColumn.AllowEdit = False
        Me.TreeListColumn7.OptionsColumn.AllowSort = False
        Me.TreeListColumn7.OptionsFilter.FilterPopupMode = DevExpress.XtraTreeList.FilterPopupMode.CheckedList
        Me.TreeListColumn7.Visible = True
        Me.TreeListColumn7.VisibleIndex = 3
        Me.TreeListColumn7.Width = 64
        '
        'TreeListColumn8
        '
        Me.TreeListColumn8.Caption = "Entity"
        Me.TreeListColumn8.FieldName = "Item"
        Me.TreeListColumn8.Name = "TreeListColumn8"
        Me.TreeListColumn8.OptionsColumn.AllowEdit = False
        Me.TreeListColumn8.OptionsColumn.AllowSort = False
        Me.TreeListColumn8.OptionsFilter.FilterPopupMode = DevExpress.XtraTreeList.FilterPopupMode.CheckedList
        Me.TreeListColumn8.Visible = True
        Me.TreeListColumn8.VisibleIndex = 4
        Me.TreeListColumn8.Width = 39
        '
        'TreeListColumn9
        '
        Me.TreeListColumn9.Caption = "extension"
        Me.TreeListColumn9.FieldName = "extension"
        Me.TreeListColumn9.Name = "TreeListColumn9"
        '
        'uxMoveDown
        '
        Me.uxMoveDown.AllowDrop = True
        Me.uxMoveDown.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxMoveDown.Enabled = False
        Me.uxMoveDown.ImageIndex = 0
        Me.uxMoveDown.ImageList = Me.uximage
        Me.uxMoveDown.Location = New System.Drawing.Point(543, 203)
        Me.uxMoveDown.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.uxMoveDown.Name = "uxMoveDown"
        Me.uxMoveDown.Size = New System.Drawing.Size(24, 24)
        Me.uxMoveDown.TabIndex = 74
        '
        'uximage
        '
        Me.uximage.ImageStream = CType(resources.GetObject("uximage.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
        Me.uximage.Images.SetKeyName(0, "down_16.ico")
        Me.uximage.Images.SetKeyName(1, "search_16.ico")
        Me.uximage.Images.SetKeyName(2, "up_16.ico")
        Me.uximage.Images.SetKeyName(3, "read_16.ico")
        '
        'uxMoveUp
        '
        Me.uxMoveUp.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxMoveUp.Enabled = False
        Me.uxMoveUp.ImageIndex = 2
        Me.uxMoveUp.ImageList = Me.uximage
        Me.uxMoveUp.Location = New System.Drawing.Point(543, 175)
        Me.uxMoveUp.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.uxMoveUp.Name = "uxMoveUp"
        Me.uxMoveUp.Size = New System.Drawing.Size(24, 24)
        Me.uxMoveUp.TabIndex = 73
        Me.uxMoveUp.Tag = " "
        '
        'dfrom
        '
        Me.dfrom.EditValue = New Date(2013, 10, 10, 0, 0, 0, 0)
        Me.dfrom.Location = New System.Drawing.Point(66, 21)
        Me.dfrom.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.dfrom.Name = "dfrom"
        Me.dfrom.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.dfrom.Properties.CalendarTimeEditing = DevExpress.Utils.DefaultBoolean.[False]
        Me.dfrom.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
        Me.dfrom.Properties.CalendarTimeProperties.DisplayFormat.FormatString = "dd-MMM-yyyy"
        Me.dfrom.Properties.CalendarTimeProperties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom
        Me.dfrom.Properties.CalendarTimeProperties.EditFormat.FormatString = "dd-MMM-yyyy"
        Me.dfrom.Properties.CalendarTimeProperties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom
        Me.dfrom.Properties.DisplayFormat.FormatString = "dd-MMM-yyyy"
        Me.dfrom.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        Me.dfrom.Properties.EditFormat.FormatString = "dd-MMM-yyyy"
        Me.dfrom.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        Me.dfrom.Size = New System.Drawing.Size(86, 20)
        Me.dfrom.TabIndex = 75
        '
        'dto
        '
        Me.dto.EditValue = New Date(2013, 10, 10, 0, 0, 0, 0)
        Me.dto.Location = New System.Drawing.Point(66, 54)
        Me.dto.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.dto.Name = "dto"
        Me.dto.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.dto.Properties.CalendarTimeEditing = DevExpress.Utils.DefaultBoolean.[False]
        Me.dto.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
        Me.dto.Properties.CalendarTimeProperties.DisplayFormat.FormatString = "dd-MMM-yyyy"
        Me.dto.Properties.CalendarTimeProperties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom
        Me.dto.Properties.CalendarTimeProperties.EditFormat.FormatString = "dd-MMM-yyyy"
        Me.dto.Properties.CalendarTimeProperties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom
        Me.dto.Properties.DisplayFormat.FormatString = "dd-MMM-yyyy"
        Me.dto.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        Me.dto.Size = New System.Drawing.Size(86, 20)
        Me.dto.TabIndex = 76
        '
        'RadioGroup1
        '
        Me.RadioGroup1.Location = New System.Drawing.Point(261, 13)
        Me.RadioGroup1.Name = "RadioGroup1"
        Me.RadioGroup1.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.RadioGroup1.Properties.Appearance.Options.UseBackColor = True
        Me.RadioGroup1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me.RadioGroup1.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.RadioGroupItem() {New DevExpress.XtraEditors.Controls.RadioGroupItem(CType(0, Short), "For"), New DevExpress.XtraEditors.Controls.RadioGroupItem(CType(1, Short), "Linked to")})
        Me.RadioGroup1.Size = New System.Drawing.Size(70, 71)
        Me.RadioGroup1.TabIndex = 83
        '
        'ldesc
        '
        Me.ldesc.Location = New System.Drawing.Point(10, 90)
        Me.ldesc.Name = "ldesc"
        Me.ldesc.Size = New System.Drawing.Size(24, 13)
        Me.ldesc.TabIndex = 84
        Me.ldesc.Text = "ldesc"
        '
        'LabelControl1
        '
        Me.LabelControl1.Location = New System.Drawing.Point(10, 26)
        Me.LabelControl1.Name = "LabelControl1"
        Me.LabelControl1.Size = New System.Drawing.Size(50, 13)
        Me.LabelControl1.TabIndex = 85
        Me.LabelControl1.Text = "Date From"
        '
        'LabelControl2
        '
        Me.LabelControl2.Location = New System.Drawing.Point(10, 57)
        Me.LabelControl2.Name = "LabelControl2"
        Me.LabelControl2.Size = New System.Drawing.Size(36, 13)
        Me.LabelControl2.TabIndex = 86
        Me.LabelControl2.Text = "Date to"
        '
        'lres
        '
        Me.lres.Location = New System.Drawing.Point(12, 136)
        Me.lres.Name = "lres"
        Me.lres.Size = New System.Drawing.Size(71, 13)
        Me.lres.TabIndex = 87
        Me.lres.Text = "Search Results"
        '
        'LabelControl4
        '
        Me.LabelControl4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LabelControl4.Location = New System.Drawing.Point(568, 136)
        Me.LabelControl4.Name = "LabelControl4"
        Me.LabelControl4.Size = New System.Drawing.Size(41, 13)
        Me.LabelControl4.TabIndex = 88
        Me.LabelControl4.Text = "Selected"
        '
        'PanelControl1
        '
        Me.PanelControl1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PanelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Style3D
        Me.PanelControl1.Controls.Add(Me.lauthor)
        Me.PanelControl1.Controls.Add(Me.Ltype)
        Me.PanelControl1.Controls.Add(Me.cbotype)
        Me.PanelControl1.Controls.Add(Me.cboauthor)
        Me.PanelControl1.Controls.Add(Me.cbopropentity)
        Me.PanelControl1.Controls.Add(Me.cboentity)
        Me.PanelControl1.Controls.Add(Me.cbopropclass)
        Me.PanelControl1.Controls.Add(Me.cboclass)
        Me.PanelControl1.Controls.Add(Me.dfrom)
        Me.PanelControl1.Controls.Add(Me.dto)
        Me.PanelControl1.Controls.Add(Me.ldesc)
        Me.PanelControl1.Controls.Add(Me.RadioGroup1)
        Me.PanelControl1.Controls.Add(Me.LabelControl2)
        Me.PanelControl1.Controls.Add(Me.LabelControl1)
        Me.PanelControl1.Location = New System.Drawing.Point(12, 19)
        Me.PanelControl1.Name = "PanelControl1"
        Me.PanelControl1.Size = New System.Drawing.Size(925, 111)
        Me.PanelControl1.TabIndex = 89
        '
        'lauthor
        '
        Me.lauthor.Location = New System.Drawing.Point(265, 57)
        Me.lauthor.Name = "lauthor"
        Me.lauthor.Size = New System.Drawing.Size(33, 13)
        Me.lauthor.TabIndex = 94
        Me.lauthor.Text = "Author"
        Me.lauthor.Visible = False
        '
        'Ltype
        '
        Me.Ltype.Location = New System.Drawing.Point(265, 26)
        Me.Ltype.Name = "Ltype"
        Me.Ltype.Size = New System.Drawing.Size(24, 13)
        Me.Ltype.TabIndex = 93
        Me.Ltype.Text = "Type"
        Me.Ltype.Visible = False
        '
        'cbotype
        '
        Me.cbotype.EditValue = ""
        Me.cbotype.Location = New System.Drawing.Point(337, 21)
        Me.cbotype.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.cbotype.Name = "cbotype"
        Me.cbotype.Properties.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.[True]
        Me.cbotype.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.cbotype.Properties.MaxLength = 250
        Me.cbotype.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        Me.cbotype.Size = New System.Drawing.Size(225, 20)
        Me.cbotype.TabIndex = 92
        Me.cbotype.Visible = False
        '
        'cboauthor
        '
        Me.cboauthor.EditValue = "v "
        Me.cboauthor.Location = New System.Drawing.Point(337, 54)
        Me.cboauthor.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.cboauthor.Name = "cboauthor"
        Me.cboauthor.Properties.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.[True]
        Me.cboauthor.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.cboauthor.Properties.MaxLength = 250
        Me.cboauthor.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        Me.cboauthor.Size = New System.Drawing.Size(225, 20)
        Me.cboauthor.TabIndex = 91
        Me.cboauthor.Visible = False
        '
        'cbopropentity
        '
        Me.cbopropentity.EditValue = ""
        Me.cbopropentity.Location = New System.Drawing.Point(568, 54)
        Me.cbopropentity.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.cbopropentity.Name = "cbopropentity"
        Me.cbopropentity.Properties.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.[True]
        Me.cbopropentity.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.cbopropentity.Properties.MaxLength = 250
        Me.cbopropentity.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        Me.cbopropentity.Size = New System.Drawing.Size(225, 20)
        Me.cbopropentity.TabIndex = 90
        '
        'cboentity
        '
        Me.cboentity.EditValue = ""
        Me.cboentity.Location = New System.Drawing.Point(568, 19)
        Me.cboentity.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.cboentity.Name = "cboentity"
        Me.cboentity.Properties.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.[True]
        Me.cboentity.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.cboentity.Properties.MaxLength = 250
        Me.cboentity.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        Me.cboentity.Size = New System.Drawing.Size(225, 20)
        Me.cboentity.TabIndex = 89
        '
        'cbopropclass
        '
        Me.cbopropclass.EditValue = ""
        Me.cbopropclass.Location = New System.Drawing.Point(337, 54)
        Me.cbopropclass.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.cbopropclass.Name = "cbopropclass"
        Me.cbopropclass.Properties.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.[True]
        Me.cbopropclass.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.cbopropclass.Properties.MaxLength = 250
        Me.cbopropclass.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        Me.cbopropclass.Size = New System.Drawing.Size(188, 20)
        Me.cbopropclass.TabIndex = 88
        '
        'cboclass
        '
        Me.cboclass.EditValue = ""
        Me.cboclass.Location = New System.Drawing.Point(337, 21)
        Me.cboclass.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.cboclass.Name = "cboclass"
        Me.cboclass.Properties.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.[True]
        Me.cboclass.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.cboclass.Properties.MaxLength = 250
        Me.cboclass.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        Me.cboclass.Size = New System.Drawing.Size(188, 20)
        Me.cboclass.TabIndex = 87
        '
        'bc_am_dx_linked_documents
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(949, 418)
        Me.Controls.Add(Me.PanelControl1)
        Me.Controls.Add(Me.LabelControl4)
        Me.Controls.Add(Me.lres)
        Me.Controls.Add(Me.uxMoveDown)
        Me.Controls.Add(Me.uxMoveUp)
        Me.Controls.Add(Me.lvsel)
        Me.Controls.Add(Me.PictureBox2)
        Me.Controls.Add(Me.lvall)
        Me.Controls.Add(Me.bcancel)
        Me.Controls.Add(Me.bsave)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimizeBox = False
        Me.Name = "bc_am_dx_linked_documents"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Blue Curve - Linked Documents"
        CType(Me.lvall, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemDateEdit1.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemDateEdit1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemDateEdit2.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemDateEdit2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lvsel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uximage, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dfrom.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dfrom.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dto.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dto.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadioGroup1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl1.ResumeLayout(False)
        Me.PanelControl1.PerformLayout()
        CType(Me.cbotype.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cboauthor.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cbopropentity.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cboentity.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cbopropclass.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cboclass.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents bcancel As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents bsave As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents uxImages As System.Windows.Forms.ImageList
    Friend WithEvents lvall As DevExpress.XtraTreeList.TreeList
    Friend WithEvents TreeListColumn6 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents TreeListColumn1 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents TreeListColumn12 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents TreeListColumn14 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents TreeListColumn3 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents lvsel As DevExpress.XtraTreeList.TreeList
    Friend WithEvents TreeListColumn2 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents TreeListColumn4 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents TreeListColumn5 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents TreeListColumn7 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents TreeListColumn8 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents uxMoveDown As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents uxMoveUp As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents uximage As DevExpress.Utils.ImageCollection
    Friend WithEvents dfrom As DevExpress.XtraEditors.DateEdit
    Friend WithEvents dto As DevExpress.XtraEditors.DateEdit
    Friend WithEvents RadioGroup1 As DevExpress.XtraEditors.RadioGroup
    Friend WithEvents ldesc As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl2 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lres As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl4 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents cbotype As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents cboauthor As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents cbopropentity As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents cboentity As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents cbopropclass As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents cboclass As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents RepositoryItemDateEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemDateEdit
    Friend WithEvents RepositoryItemDateEdit2 As DevExpress.XtraEditors.Repository.RepositoryItemDateEdit
    Friend WithEvents lauthor As DevExpress.XtraEditors.LabelControl
    Friend WithEvents Ltype As DevExpress.XtraEditors.LabelControl
    Friend WithEvents TreeListColumn10 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents TreeListColumn9 As DevExpress.XtraTreeList.Columns.TreeListColumn
End Class
