<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bc_am_pr_document_list
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_pr_document_list))
        Dim GridLevelNode1 As DevExpress.XtraGrid.GridLevelNode = New DevExpress.XtraGrid.GridLevelNode()
        Me.uxDLImageList = New DevExpress.Utils.ImageCollection()
        Me.uxGridImageCombo = New DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox()
        Me.RepositoryItemPictureEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit()
        Me.RepositoryItemImageComboBox1 = New DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox()
        Me.uxDocumentListView = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.uxImageCol = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.uxTitleCol = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.uxDateCol = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.uxPublicationCol = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.uxStageCol = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.uxAuthorCol = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.uxDocumentStateCol = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.uxEntityCol = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.uxCompletedByCol = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.uxBusinessAreaCol = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.uxLanguageCol = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.uxdiststatus = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.uxattestations = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.uxattestationstatus = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.uxattestaionstatus = New DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox()
        Me.uxattestationimages = New DevExpress.Utils.ImageCollection()
        Me.uxfastTrack = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.uxftimages = New DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox()
        Me.uxrecchange = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.uxDocumentList = New DevExpress.XtraGrid.GridControl()
        Me.RepositoryItemImageComboBox2 = New DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox()
        Me.uxfasttrackimages = New DevExpress.XtraEditors.Repository.RepositoryItemCheckedComboBoxEdit()
        Me.uxassigned = New DevExpress.XtraGrid.Columns.GridColumn()
        CType(Me.uxDLImageList, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxGridImageCombo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemPictureEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemImageComboBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxDocumentListView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxattestaionstatus, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxattestationimages, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxftimages, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxDocumentList, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemImageComboBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxfasttrackimages, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'uxDLImageList
        '
        Me.uxDLImageList.ImageStream = CType(resources.GetObject("uxDLImageList.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
        Me.uxDLImageList.Images.SetKeyName(0, "document_32.ico")
        Me.uxDLImageList.Images.SetKeyName(1, "checkin_in_to_me_32.ico")
        Me.uxDLImageList.Images.SetKeyName(2, "checkin_in_to_another_32.ico")
        Me.uxDLImageList.Images.SetKeyName(3, "unregistered_60.png")
        '
        'uxGridImageCombo
        '
        Me.uxGridImageCombo.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.[True]
        Me.uxGridImageCombo.AutoHeight = False
        Me.uxGridImageCombo.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.uxGridImageCombo.Items.AddRange(New DevExpress.XtraEditors.Controls.ImageComboBoxItem() {New DevExpress.XtraEditors.Controls.ImageComboBoxItem("", CType(0, Short), 0), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("", CType(1, Short), 1), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("", CType(2, Short), 2), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("", CType(3, Short), 3)})
        Me.uxGridImageCombo.LargeImages = Me.uxDLImageList
        Me.uxGridImageCombo.Name = "uxGridImageCombo"
        Me.uxGridImageCombo.SmallImages = Me.uxDLImageList
        '
        'RepositoryItemPictureEdit1
        '
        Me.RepositoryItemPictureEdit1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me.RepositoryItemPictureEdit1.InitialImage = Nothing
        Me.RepositoryItemPictureEdit1.Name = "RepositoryItemPictureEdit1"
        Me.RepositoryItemPictureEdit1.NullText = "none"
        Me.RepositoryItemPictureEdit1.PictureStoreMode = DevExpress.XtraEditors.Controls.PictureStoreMode.Image
        Me.RepositoryItemPictureEdit1.ReadOnly = True
        Me.RepositoryItemPictureEdit1.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom
        '
        'RepositoryItemImageComboBox1
        '
        Me.RepositoryItemImageComboBox1.AutoHeight = False
        Me.RepositoryItemImageComboBox1.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.RepositoryItemImageComboBox1.Items.AddRange(New DevExpress.XtraEditors.Controls.ImageComboBoxItem() {New DevExpress.XtraEditors.Controls.ImageComboBoxItem("", CType(1, Short), 0), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("", CType(1, Short), 1), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("", CType(2, Short), 2)})
        Me.RepositoryItemImageComboBox1.Name = "RepositoryItemImageComboBox1"
        Me.RepositoryItemImageComboBox1.SmallImages = Me.uxDLImageList
        '
        'uxDocumentListView
        '
        Me.uxDocumentListView.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.uxImageCol, Me.uxTitleCol, Me.uxDateCol, Me.uxPublicationCol, Me.uxStageCol, Me.uxAuthorCol, Me.uxDocumentStateCol, Me.uxEntityCol, Me.uxCompletedByCol, Me.uxBusinessAreaCol, Me.uxLanguageCol, Me.uxdiststatus, Me.uxattestations, Me.uxattestationstatus, Me.uxfastTrack, Me.uxrecchange, Me.uxassigned})
        Me.uxDocumentListView.GridControl = Me.uxDocumentList
        Me.uxDocumentListView.Name = "uxDocumentListView"
        Me.uxDocumentListView.OptionsBehavior.Editable = False
        Me.uxDocumentListView.OptionsCustomization.AllowFilter = False
        Me.uxDocumentListView.OptionsFind.AlwaysVisible = True
        Me.uxDocumentListView.OptionsFind.FindDelay = 2000
        Me.uxDocumentListView.OptionsFind.FindMode = DevExpress.XtraEditors.FindMode.FindClick
        Me.uxDocumentListView.OptionsFind.HighlightFindResults = False
        Me.uxDocumentListView.OptionsFind.SearchInPreview = True
        Me.uxDocumentListView.OptionsMenu.EnableColumnMenu = False
        Me.uxDocumentListView.OptionsMenu.EnableFooterMenu = False
        Me.uxDocumentListView.OptionsMenu.EnableGroupPanelMenu = False
        Me.uxDocumentListView.OptionsSelection.EnableAppearanceFocusedCell = False
        Me.uxDocumentListView.OptionsSelection.EnableAppearanceFocusedRow = False
        Me.uxDocumentListView.OptionsView.ShowGroupPanel = False
        '
        'uxImageCol
        '
        Me.uxImageCol.Caption = " "
        Me.uxImageCol.ColumnEdit = Me.uxGridImageCombo
        Me.uxImageCol.Name = "uxImageCol"
        Me.uxImageCol.OptionsColumn.FixedWidth = True
        Me.uxImageCol.OptionsColumn.ReadOnly = True
        Me.uxImageCol.Visible = True
        Me.uxImageCol.VisibleIndex = 0
        Me.uxImageCol.Width = 20
        '
        'uxTitleCol
        '
        Me.uxTitleCol.Caption = "Title"
        Me.uxTitleCol.Name = "uxTitleCol"
        Me.uxTitleCol.Visible = True
        Me.uxTitleCol.VisibleIndex = 1
        Me.uxTitleCol.Width = 81
        '
        'uxDateCol
        '
        Me.uxDateCol.Caption = "Date"
        Me.uxDateCol.DisplayFormat.FormatString = "dd-MMM-yyyy HH:mm:ss"
        Me.uxDateCol.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        Me.uxDateCol.Name = "uxDateCol"
        Me.uxDateCol.Visible = True
        Me.uxDateCol.VisibleIndex = 2
        Me.uxDateCol.Width = 60
        '
        'uxPublicationCol
        '
        Me.uxPublicationCol.Caption = "Publication"
        Me.uxPublicationCol.Name = "uxPublicationCol"
        Me.uxPublicationCol.Visible = True
        Me.uxPublicationCol.VisibleIndex = 3
        Me.uxPublicationCol.Width = 83
        '
        'uxStageCol
        '
        Me.uxStageCol.Caption = "Stage"
        Me.uxStageCol.Name = "uxStageCol"
        Me.uxStageCol.Visible = True
        Me.uxStageCol.VisibleIndex = 4
        Me.uxStageCol.Width = 83
        '
        'uxAuthorCol
        '
        Me.uxAuthorCol.Caption = "Author"
        Me.uxAuthorCol.Name = "uxAuthorCol"
        Me.uxAuthorCol.Visible = True
        Me.uxAuthorCol.VisibleIndex = 5
        Me.uxAuthorCol.Width = 83
        '
        'uxDocumentStateCol
        '
        Me.uxDocumentStateCol.Caption = "Checked Out To"
        Me.uxDocumentStateCol.Name = "uxDocumentStateCol"
        Me.uxDocumentStateCol.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.[True]
        Me.uxDocumentStateCol.Visible = True
        Me.uxDocumentStateCol.VisibleIndex = 6
        Me.uxDocumentStateCol.Width = 82
        '
        'uxEntityCol
        '
        Me.uxEntityCol.Caption = "Entity"
        Me.uxEntityCol.Name = "uxEntityCol"
        Me.uxEntityCol.Visible = True
        Me.uxEntityCol.VisibleIndex = 7
        Me.uxEntityCol.Width = 83
        '
        'uxCompletedByCol
        '
        Me.uxCompletedByCol.Caption = "Completed By"
        Me.uxCompletedByCol.DisplayFormat.FormatString = "dd-MMM-yyyy HH:mm:dd"
        Me.uxCompletedByCol.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        Me.uxCompletedByCol.Name = "uxCompletedByCol"
        Me.uxCompletedByCol.Visible = True
        Me.uxCompletedByCol.VisibleIndex = 8
        Me.uxCompletedByCol.Width = 60
        '
        'uxBusinessAreaCol
        '
        Me.uxBusinessAreaCol.Caption = "Business Area"
        Me.uxBusinessAreaCol.Name = "uxBusinessAreaCol"
        Me.uxBusinessAreaCol.Visible = True
        Me.uxBusinessAreaCol.VisibleIndex = 9
        Me.uxBusinessAreaCol.Width = 90
        '
        'uxLanguageCol
        '
        Me.uxLanguageCol.Caption = "Language"
        Me.uxLanguageCol.Name = "uxLanguageCol"
        Me.uxLanguageCol.Visible = True
        Me.uxLanguageCol.VisibleIndex = 10
        Me.uxLanguageCol.Width = 113
        '
        'uxdiststatus
        '
        Me.uxdiststatus.Caption = "Distribution  Status"
        Me.uxdiststatus.Name = "uxdiststatus"
        Me.uxdiststatus.Visible = True
        Me.uxdiststatus.VisibleIndex = 11
        '
        'uxattestations
        '
        Me.uxattestations.Caption = "Attestations"
        Me.uxattestations.Name = "uxattestations"
        Me.uxattestations.Visible = True
        Me.uxattestations.VisibleIndex = 12
        '
        'uxattestationstatus
        '
        Me.uxattestationstatus.Caption = "Review"
        Me.uxattestationstatus.ColumnEdit = Me.uxattestaionstatus
        Me.uxattestationstatus.Name = "uxattestationstatus"
        Me.uxattestationstatus.Visible = True
        Me.uxattestationstatus.VisibleIndex = 13
        '
        'uxattestaionstatus
        '
        Me.uxattestaionstatus.AutoHeight = False
        Me.uxattestaionstatus.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.uxattestaionstatus.Items.AddRange(New DevExpress.XtraEditors.Controls.ImageComboBoxItem() {New DevExpress.XtraEditors.Controls.ImageComboBoxItem("", CType(0, Short), 0), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("", CType(1, Short), 1)})
        Me.uxattestaionstatus.LargeImages = Me.uxattestationimages
        Me.uxattestaionstatus.Name = "uxattestaionstatus"
        '
        'uxattestationimages
        '
        Me.uxattestationimages.ImageStream = CType(resources.GetObject("uxattestationimages.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
        Me.uxattestationimages.Images.SetKeyName(0, "2000px-Red_x.svg.png")
        Me.uxattestationimages.Images.SetKeyName(1, "green-tick.png")
        '
        'uxfastTrack
        '
        Me.uxfastTrack.Caption = "Fast Track"
        Me.uxfastTrack.ColumnEdit = Me.uxftimages
        Me.uxfastTrack.Name = "uxfastTrack"
        Me.uxfastTrack.Visible = True
        Me.uxfastTrack.VisibleIndex = 14
        '
        'uxftimages
        '
        Me.uxftimages.AutoHeight = False
        Me.uxftimages.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.uxftimages.Items.AddRange(New DevExpress.XtraEditors.Controls.ImageComboBoxItem() {New DevExpress.XtraEditors.Controls.ImageComboBoxItem("", CType(0, Short), 1)})
        Me.uxftimages.LargeImages = Me.uxattestationimages
        Me.uxftimages.Name = "uxftimages"
        '
        'uxrecchange
        '
        Me.uxrecchange.Caption = "Attribute Change"
        Me.uxrecchange.Name = "uxrecchange"
        Me.uxrecchange.Visible = True
        Me.uxrecchange.VisibleIndex = 15
        '
        'uxDocumentList
        '
        Me.uxDocumentList.Dock = System.Windows.Forms.DockStyle.Fill
        GridLevelNode1.RelationName = "Level1"
        Me.uxDocumentList.LevelTree.Nodes.AddRange(New DevExpress.XtraGrid.GridLevelNode() {GridLevelNode1})
        Me.uxDocumentList.Location = New System.Drawing.Point(0, 0)
        Me.uxDocumentList.MainView = Me.uxDocumentListView
        Me.uxDocumentList.Name = "uxDocumentList"
        Me.uxDocumentList.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.uxGridImageCombo, Me.RepositoryItemPictureEdit1, Me.RepositoryItemImageComboBox1, Me.RepositoryItemImageComboBox2, Me.uxattestaionstatus, Me.uxfasttrackimages, Me.uxftimages})
        Me.uxDocumentList.Size = New System.Drawing.Size(856, 582)
        Me.uxDocumentList.TabIndex = 0
        Me.uxDocumentList.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.uxDocumentListView})
        '
        'RepositoryItemImageComboBox2
        '
        Me.RepositoryItemImageComboBox2.AutoHeight = False
        Me.RepositoryItemImageComboBox2.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.RepositoryItemImageComboBox2.Name = "RepositoryItemImageComboBox2"
        '
        'uxfasttrackimages
        '
        Me.uxfasttrackimages.AutoHeight = False
        Me.uxfasttrackimages.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.uxfasttrackimages.Name = "uxfasttrackimages"
        '
        'uxassigned
        '
        Me.uxassigned.Caption = "Assigned To"
        Me.uxassigned.Name = "uxassigned"
        Me.uxassigned.Visible = True
        Me.uxassigned.VisibleIndex = 16
        '
        'bc_am_pr_document_list
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(856, 582)
        Me.ControlBox = False
        Me.Controls.Add(Me.uxDocumentList)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "bc_am_pr_document_list"
        CType(Me.uxDLImageList, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxGridImageCombo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemPictureEdit1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemImageComboBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxDocumentListView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxattestaionstatus, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxattestationimages, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxftimages, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxDocumentList, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemImageComboBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxfasttrackimages, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents uxDLImageList As DevExpress.Utils.ImageCollection
    Friend WithEvents uxGridImageCombo As DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox
    Friend WithEvents RepositoryItemPictureEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit
    Friend WithEvents RepositoryItemImageComboBox1 As DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox
    Friend WithEvents uxDocumentListView As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents uxImageCol As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents uxTitleCol As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents uxDateCol As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents uxPublicationCol As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents uxStageCol As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents uxAuthorCol As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents uxDocumentStateCol As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents uxEntityCol As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents uxCompletedByCol As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents uxBusinessAreaCol As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents uxLanguageCol As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents uxdiststatus As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents uxDocumentList As DevExpress.XtraGrid.GridControl
    Friend WithEvents uxattestations As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents uxattestationstatus As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents uxattestaionstatus As DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox
    Friend WithEvents RepositoryItemImageComboBox2 As DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox
    Friend WithEvents uxattestationimages As DevExpress.Utils.ImageCollection
    Friend WithEvents uxfastTrack As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents uxfasttrackimages As DevExpress.XtraEditors.Repository.RepositoryItemCheckedComboBoxEdit
    Friend WithEvents uxftimages As DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox
    Friend WithEvents uxrecchange As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents uxassigned As DevExpress.XtraGrid.Columns.GridColumn
End Class
