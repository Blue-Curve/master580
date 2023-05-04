Imports System.Windows.Forms
Imports BlueCurve.Core.CS

Friend Class bc_am_bp_context
    Inherits System.Windows.Forms.Form

    Private ctrllr As bc_am_context
    Friend WithEvents uxLegend As System.Windows.Forms.Label
    Friend WithEvents uxPeriodFormatStyle As System.Windows.Forms.ComboBox
    Friend WithEvents uxPeriodFmtStyle As System.Windows.Forms.Label
    Friend WithEvents uxPeriodFormat As System.Windows.Forms.ComboBox
    Friend WithEvents uxPeriodFmt As System.Windows.Forms.Label
    Friend WithEvents uxDisplayPeriodLabels As System.Windows.Forms.CheckBox
    Friend WithEvents uxPeriodStartRowCol As System.Windows.Forms.Label
    Friend WithEvents uxPeriodStartRow As System.Windows.Forms.TextBox
    Friend WithEvents uxPeriodStartCol As System.Windows.Forms.TextBox
    Friend WithEvents uxForwardSlash4 As System.Windows.Forms.Label

    Private currentSelectedIndex As Integer = -1

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

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
    Friend WithEvents uxTableDefinition As System.Windows.Forms.GroupBox
    Friend WithEvents uxName As System.Windows.Forms.Label
    Friend WithEvents uxDataMdl As System.Windows.Forms.Label
    Friend WithEvents uxDataModel As System.Windows.Forms.ComboBox
    Friend WithEvents uxClone As System.Windows.Forms.Button
    Friend WithEvents uxTableSettings As System.Windows.Forms.GroupBox
    Friend WithEvents uxDataItemSettings As System.Windows.Forms.GroupBox
    Friend WithEvents uxDelete As System.Windows.Forms.Button
    Friend WithEvents uxSave As System.Windows.Forms.Button
    Friend WithEvents uxAdd As System.Windows.Forms.Button
    Friend WithEvents uxRemove As System.Windows.Forms.Button
    Friend WithEvents uxImageList As System.Windows.Forms.ImageList
    Friend WithEvents uxToolTip As System.Windows.Forms.ToolTip
    Friend WithEvents uxYearStartRow As System.Windows.Forms.TextBox
    Friend WithEvents uxDataItmDir As System.Windows.Forms.Label
    Friend WithEvents uxDataItemDirection As System.Windows.Forms.ComboBox
    Friend WithEvents uxYrStartRow As System.Windows.Forms.Label
    Friend WithEvents uxYrStyle As System.Windows.Forms.Label
    Friend WithEvents uxYearStyle As System.Windows.Forms.ComboBox
    Friend WithEvents uxNumberOfYears As System.Windows.Forms.TextBox
    Friend WithEvents uxLbl As System.Windows.Forms.Label
    Friend WithEvents uxLabel As System.Windows.Forms.ComboBox
    Friend WithEvents uxDataStyle As System.Windows.Forms.ComboBox
    Friend WithEvents uxLabelStyle As System.Windows.Forms.ComboBox
    Friend WithEvents uxDataStartRow As System.Windows.Forms.TextBox
    Friend WithEvents uxDataCol As System.Windows.Forms.Label
    Friend WithEvents uxDataRow As System.Windows.Forms.Label
    Friend WithEvents uxScaleFac As System.Windows.Forms.Label
    Friend WithEvents uxScaleSym As System.Windows.Forms.Label
    Friend WithEvents uxDtStyle As System.Windows.Forms.Label
    Friend WithEvents uxScaleSymbol As System.Windows.Forms.ComboBox
    Friend WithEvents uxYearStartColumn As System.Windows.Forms.TextBox
    Friend WithEvents uxAvDataItems As System.Windows.Forms.GroupBox
    Friend WithEvents uxSelDataItems As System.Windows.Forms.GroupBox
    Friend WithEvents uxFormat As System.Windows.Forms.Label
    Friend WithEvents uxFormatMask As System.Windows.Forms.ComboBox
    Friend WithEvents uxClose As System.Windows.Forms.Button
    Friend WithEvents uxContextName As System.Windows.Forms.TextBox
    Friend WithEvents uxScaleFactor As System.Windows.Forms.ComboBox
    Friend WithEvents uxDataStartColumn As System.Windows.Forms.TextBox
    Friend WithEvents ubLblStyle As System.Windows.Forms.Label
    Friend WithEvents uxAvailableDataItems As System.Windows.Forms.ListBox
    Friend WithEvents uxSelectedDataItems As System.Windows.Forms.ListBox
    Friend WithEvents uxMoveDown As System.Windows.Forms.Button
    Friend WithEvents uxMoveUp As System.Windows.Forms.Button
    Friend WithEvents uxSaveFileDlg As System.Windows.Forms.SaveFileDialog
    Friend WithEvents uxOpenFileDlg As System.Windows.Forms.OpenFileDialog
    Friend WithEvents uxSSCustom As System.Windows.Forms.TextBox
    Friend WithEvents uxNewLabel As System.Windows.Forms.Button
    Friend WithEvents uxAvailLinkCode As System.Windows.Forms.TextBox
    Friend WithEvents uxSelLinkCode As System.Windows.Forms.TextBox
    Friend WithEvents uxTtle As System.Windows.Forms.Label
    Friend WithEvents uxTitle As System.Windows.Forms.TextBox
    Friend WithEvents uxSrc As System.Windows.Forms.Label
    Friend WithEvents uxSource As System.Windows.Forms.TextBox
    Friend WithEvents uxNoActYears As System.Windows.Forms.Label
    Friend WithEvents uxNumberOfActualYears As System.Windows.Forms.TextBox
    Friend WithEvents uxTitleStyle As System.Windows.Forms.ComboBox
    Friend WithEvents uxTleStyle As System.Windows.Forms.Label
    Friend WithEvents uxSourceStyle As System.Windows.Forms.ComboBox
    Friend WithEvents uxSrcStyle As System.Windows.Forms.Label
    Friend WithEvents uxEditLabel As System.Windows.Forms.Button
    Friend WithEvents uxYearToText As System.Windows.Forms.TextBox
    Friend WithEvents uxYrToStyle As System.Windows.Forms.Label
    Friend WithEvents uxYrToText As System.Windows.Forms.Label
    Friend WithEvents uxYearToStyle As System.Windows.Forms.ComboBox
    Friend WithEvents uxForwardSlash1 As System.Windows.Forms.Label
    Friend WithEvents uxForwardSlash2 As System.Windows.Forms.Label
    Friend WithEvents uxEstActIdentifier As System.Windows.Forms.Label
    Friend WithEvents uxForwardSlash3 As System.Windows.Forms.Label
    Friend WithEvents uxEstimateIdentifier As System.Windows.Forms.TextBox
    Friend WithEvents uxActualIdentifier As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_bp_context))
        Me.uxTableDefinition = New System.Windows.Forms.GroupBox
        Me.uxLegend = New System.Windows.Forms.Label
        Me.uxDataItemSettings = New System.Windows.Forms.GroupBox
        Me.uxLabelStyle = New System.Windows.Forms.ComboBox
        Me.uxDataStyle = New System.Windows.Forms.ComboBox
        Me.uxEditLabel = New System.Windows.Forms.Button
        Me.uxImageList = New System.Windows.Forms.ImageList(Me.components)
        Me.uxNewLabel = New System.Windows.Forms.Button
        Me.uxSSCustom = New System.Windows.Forms.TextBox
        Me.uxScaleFactor = New System.Windows.Forms.ComboBox
        Me.uxScaleSym = New System.Windows.Forms.Label
        Me.uxScaleFac = New System.Windows.Forms.Label
        Me.uxFormat = New System.Windows.Forms.Label
        Me.uxDtStyle = New System.Windows.Forms.Label
        Me.uxDataCol = New System.Windows.Forms.Label
        Me.uxDataRow = New System.Windows.Forms.Label
        Me.uxDataStartRow = New System.Windows.Forms.TextBox
        Me.uxDataStartColumn = New System.Windows.Forms.TextBox
        Me.ubLblStyle = New System.Windows.Forms.Label
        Me.uxScaleSymbol = New System.Windows.Forms.ComboBox
        Me.uxFormatMask = New System.Windows.Forms.ComboBox
        Me.uxLabel = New System.Windows.Forms.ComboBox
        Me.uxLbl = New System.Windows.Forms.Label
        Me.uxSelDataItems = New System.Windows.Forms.GroupBox
        Me.uxSelLinkCode = New System.Windows.Forms.TextBox
        Me.uxMoveDown = New System.Windows.Forms.Button
        Me.uxMoveUp = New System.Windows.Forms.Button
        Me.uxSelectedDataItems = New System.Windows.Forms.ListBox
        Me.uxAvDataItems = New System.Windows.Forms.GroupBox
        Me.uxAvailLinkCode = New System.Windows.Forms.TextBox
        Me.uxAvailableDataItems = New System.Windows.Forms.ListBox
        Me.uxRemove = New System.Windows.Forms.Button
        Me.uxAdd = New System.Windows.Forms.Button
        Me.uxSave = New System.Windows.Forms.Button
        Me.uxDelete = New System.Windows.Forms.Button
        Me.uxClose = New System.Windows.Forms.Button
        Me.uxTableSettings = New System.Windows.Forms.GroupBox
        Me.uxPeriodStartRow = New System.Windows.Forms.TextBox
        Me.uxPeriodStartRowCol = New System.Windows.Forms.Label
        Me.uxPeriodStartCol = New System.Windows.Forms.TextBox
        Me.uxForwardSlash4 = New System.Windows.Forms.Label
        Me.uxDisplayPeriodLabels = New System.Windows.Forms.CheckBox
        Me.uxPeriodFormatStyle = New System.Windows.Forms.ComboBox
        Me.uxPeriodFmtStyle = New System.Windows.Forms.Label
        Me.uxPeriodFormat = New System.Windows.Forms.ComboBox
        Me.uxPeriodFmt = New System.Windows.Forms.Label
        Me.uxTitle = New System.Windows.Forms.TextBox
        Me.uxYearToStyle = New System.Windows.Forms.ComboBox
        Me.uxForwardSlash3 = New System.Windows.Forms.Label
        Me.uxEstActIdentifier = New System.Windows.Forms.Label
        Me.uxForwardSlash2 = New System.Windows.Forms.Label
        Me.uxEstimateIdentifier = New System.Windows.Forms.TextBox
        Me.uxActualIdentifier = New System.Windows.Forms.TextBox
        Me.uxYrToText = New System.Windows.Forms.Label
        Me.uxYrToStyle = New System.Windows.Forms.Label
        Me.uxYearToText = New System.Windows.Forms.TextBox
        Me.uxSourceStyle = New System.Windows.Forms.ComboBox
        Me.uxSrcStyle = New System.Windows.Forms.Label
        Me.uxTitleStyle = New System.Windows.Forms.ComboBox
        Me.uxTleStyle = New System.Windows.Forms.Label
        Me.uxNoActYears = New System.Windows.Forms.Label
        Me.uxNumberOfActualYears = New System.Windows.Forms.TextBox
        Me.uxSrc = New System.Windows.Forms.Label
        Me.uxSource = New System.Windows.Forms.TextBox
        Me.uxTtle = New System.Windows.Forms.Label
        Me.uxYearStyle = New System.Windows.Forms.ComboBox
        Me.uxYrStyle = New System.Windows.Forms.Label
        Me.uxYrStartRow = New System.Windows.Forms.Label
        Me.uxDataItemDirection = New System.Windows.Forms.ComboBox
        Me.uxDataItmDir = New System.Windows.Forms.Label
        Me.uxYearStartRow = New System.Windows.Forms.TextBox
        Me.uxYearStartColumn = New System.Windows.Forms.TextBox
        Me.uxNumberOfYears = New System.Windows.Forms.TextBox
        Me.uxForwardSlash1 = New System.Windows.Forms.Label
        Me.uxClone = New System.Windows.Forms.Button
        Me.uxDataModel = New System.Windows.Forms.ComboBox
        Me.uxDataMdl = New System.Windows.Forms.Label
        Me.uxContextName = New System.Windows.Forms.TextBox
        Me.uxName = New System.Windows.Forms.Label
        Me.uxToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.uxSaveFileDlg = New System.Windows.Forms.SaveFileDialog
        Me.uxOpenFileDlg = New System.Windows.Forms.OpenFileDialog
        Me.uxTableDefinition.SuspendLayout()
        Me.uxDataItemSettings.SuspendLayout()
        Me.uxSelDataItems.SuspendLayout()
        Me.uxAvDataItems.SuspendLayout()
        Me.uxTableSettings.SuspendLayout()
        Me.SuspendLayout()
        '
        'uxTableDefinition
        '
        Me.uxTableDefinition.Controls.Add(Me.uxLegend)
        Me.uxTableDefinition.Controls.Add(Me.uxDataItemSettings)
        Me.uxTableDefinition.Controls.Add(Me.uxSelDataItems)
        Me.uxTableDefinition.Controls.Add(Me.uxAvDataItems)
        Me.uxTableDefinition.Controls.Add(Me.uxRemove)
        Me.uxTableDefinition.Controls.Add(Me.uxAdd)
        Me.uxTableDefinition.Controls.Add(Me.uxSave)
        Me.uxTableDefinition.Controls.Add(Me.uxDelete)
        Me.uxTableDefinition.Controls.Add(Me.uxClose)
        Me.uxTableDefinition.Controls.Add(Me.uxTableSettings)
        Me.uxTableDefinition.Controls.Add(Me.uxClone)
        Me.uxTableDefinition.Controls.Add(Me.uxDataModel)
        Me.uxTableDefinition.Controls.Add(Me.uxDataMdl)
        Me.uxTableDefinition.Controls.Add(Me.uxContextName)
        Me.uxTableDefinition.Controls.Add(Me.uxName)
        Me.uxTableDefinition.Location = New System.Drawing.Point(4, 0)
        Me.uxTableDefinition.Name = "uxTableDefinition"
        Me.uxTableDefinition.Size = New System.Drawing.Size(640, 684)
        Me.uxTableDefinition.TabIndex = 0
        Me.uxTableDefinition.TabStop = False
        '
        'uxLegend
        '
        Me.uxLegend.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(210, Byte), Integer))
        Me.uxLegend.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxLegend.Location = New System.Drawing.Point(12, 652)
        Me.uxLegend.Name = "uxLegend"
        Me.uxLegend.Size = New System.Drawing.Size(348, 23)
        Me.uxLegend.TabIndex = 12
        Me.uxLegend.Text = "Mandatory Fields (excludes blank entry data items)"
        Me.uxLegend.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'uxDataItemSettings
        '
        Me.uxDataItemSettings.Controls.Add(Me.uxLabelStyle)
        Me.uxDataItemSettings.Controls.Add(Me.uxDataStyle)
        Me.uxDataItemSettings.Controls.Add(Me.uxEditLabel)
        Me.uxDataItemSettings.Controls.Add(Me.uxNewLabel)
        Me.uxDataItemSettings.Controls.Add(Me.uxSSCustom)
        Me.uxDataItemSettings.Controls.Add(Me.uxScaleFactor)
        Me.uxDataItemSettings.Controls.Add(Me.uxScaleSym)
        Me.uxDataItemSettings.Controls.Add(Me.uxScaleFac)
        Me.uxDataItemSettings.Controls.Add(Me.uxFormat)
        Me.uxDataItemSettings.Controls.Add(Me.uxDtStyle)
        Me.uxDataItemSettings.Controls.Add(Me.uxDataCol)
        Me.uxDataItemSettings.Controls.Add(Me.uxDataRow)
        Me.uxDataItemSettings.Controls.Add(Me.uxDataStartRow)
        Me.uxDataItemSettings.Controls.Add(Me.uxDataStartColumn)
        Me.uxDataItemSettings.Controls.Add(Me.ubLblStyle)
        Me.uxDataItemSettings.Controls.Add(Me.uxScaleSymbol)
        Me.uxDataItemSettings.Controls.Add(Me.uxFormatMask)
        Me.uxDataItemSettings.Controls.Add(Me.uxLabel)
        Me.uxDataItemSettings.Controls.Add(Me.uxLbl)
        Me.uxDataItemSettings.Location = New System.Drawing.Point(8, 512)
        Me.uxDataItemSettings.Name = "uxDataItemSettings"
        Me.uxDataItemSettings.Size = New System.Drawing.Size(628, 132)
        Me.uxDataItemSettings.TabIndex = 8
        Me.uxDataItemSettings.TabStop = False
        Me.uxDataItemSettings.Text = "Data Item Settings"
        '
        'uxLabelStyle
        '
        Me.uxLabelStyle.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(210, Byte), Integer))
        Me.uxLabelStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.uxLabelStyle.DropDownWidth = 300
        Me.uxLabelStyle.Location = New System.Drawing.Point(420, 18)
        Me.uxLabelStyle.Name = "uxLabelStyle"
        Me.uxLabelStyle.Size = New System.Drawing.Size(200, 21)
        Me.uxLabelStyle.TabIndex = 1
        '
        'uxDataStyle
        '
        Me.uxDataStyle.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(210, Byte), Integer))
        Me.uxDataStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.uxDataStyle.DropDownWidth = 300
        Me.uxDataStyle.Location = New System.Drawing.Point(420, 46)
        Me.uxDataStyle.Name = "uxDataStyle"
        Me.uxDataStyle.Size = New System.Drawing.Size(200, 21)
        Me.uxDataStyle.TabIndex = 4
        '
        'uxEditLabel
        '
        Me.uxEditLabel.ImageAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.uxEditLabel.ImageIndex = 6
        Me.uxEditLabel.ImageList = Me.uxImageList
        Me.uxEditLabel.Location = New System.Drawing.Point(37, 18)
        Me.uxEditLabel.Name = "uxEditLabel"
        Me.uxEditLabel.Size = New System.Drawing.Size(28, 24)
        Me.uxEditLabel.TabIndex = 18
        Me.uxEditLabel.TabStop = False
        Me.uxToolTip.SetToolTip(Me.uxEditLabel, "Edit")
        '
        'uxImageList
        '
        Me.uxImageList.ImageStream = CType(resources.GetObject("uxImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.uxImageList.TransparentColor = System.Drawing.Color.Transparent
        Me.uxImageList.Images.SetKeyName(0, "")
        Me.uxImageList.Images.SetKeyName(1, "")
        Me.uxImageList.Images.SetKeyName(2, "")
        Me.uxImageList.Images.SetKeyName(3, "")
        Me.uxImageList.Images.SetKeyName(4, "")
        Me.uxImageList.Images.SetKeyName(5, "")
        Me.uxImageList.Images.SetKeyName(6, "")
        '
        'uxNewLabel
        '
        Me.uxNewLabel.ImageAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.uxNewLabel.ImageIndex = 5
        Me.uxNewLabel.ImageList = Me.uxImageList
        Me.uxNewLabel.Location = New System.Drawing.Point(7, 18)
        Me.uxNewLabel.Name = "uxNewLabel"
        Me.uxNewLabel.Size = New System.Drawing.Size(28, 24)
        Me.uxNewLabel.TabIndex = 17
        Me.uxNewLabel.TabStop = False
        Me.uxToolTip.SetToolTip(Me.uxNewLabel, "Add")
        '
        'uxSSCustom
        '
        Me.uxSSCustom.Location = New System.Drawing.Point(420, 102)
        Me.uxSSCustom.MaxLength = 255
        Me.uxSSCustom.Name = "uxSSCustom"
        Me.uxSSCustom.Size = New System.Drawing.Size(200, 21)
        Me.uxSSCustom.TabIndex = 16
        '
        'uxScaleFactor
        '
        Me.uxScaleFactor.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(210, Byte), Integer))
        Me.uxScaleFactor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.uxScaleFactor.Location = New System.Drawing.Point(115, 102)
        Me.uxScaleFactor.Name = "uxScaleFactor"
        Me.uxScaleFactor.Size = New System.Drawing.Size(200, 21)
        Me.uxScaleFactor.TabIndex = 7
        '
        'uxScaleSym
        '
        Me.uxScaleSym.Location = New System.Drawing.Point(325, 74)
        Me.uxScaleSym.Name = "uxScaleSym"
        Me.uxScaleSym.Size = New System.Drawing.Size(95, 21)
        Me.uxScaleSym.TabIndex = 15
        Me.uxScaleSym.Text = "Scale Symbol:"
        Me.uxScaleSym.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'uxScaleFac
        '
        Me.uxScaleFac.Location = New System.Drawing.Point(4, 102)
        Me.uxScaleFac.Name = "uxScaleFac"
        Me.uxScaleFac.Size = New System.Drawing.Size(111, 21)
        Me.uxScaleFac.TabIndex = 14
        Me.uxScaleFac.Text = "Scale Factor:"
        Me.uxScaleFac.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'uxFormat
        '
        Me.uxFormat.Location = New System.Drawing.Point(4, 74)
        Me.uxFormat.Name = "uxFormat"
        Me.uxFormat.Size = New System.Drawing.Size(111, 21)
        Me.uxFormat.TabIndex = 13
        Me.uxFormat.Text = "Format Mask:"
        Me.uxFormat.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'uxDtStyle
        '
        Me.uxDtStyle.Location = New System.Drawing.Point(325, 46)
        Me.uxDtStyle.Name = "uxDtStyle"
        Me.uxDtStyle.Size = New System.Drawing.Size(95, 21)
        Me.uxDtStyle.TabIndex = 12
        Me.uxDtStyle.Text = "Data Style:"
        Me.uxDtStyle.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'uxDataCol
        '
        Me.uxDataCol.Location = New System.Drawing.Point(163, 46)
        Me.uxDataCol.Name = "uxDataCol"
        Me.uxDataCol.Size = New System.Drawing.Size(116, 21)
        Me.uxDataCol.TabIndex = 11
        Me.uxDataCol.Text = "Label Column:"
        Me.uxDataCol.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'uxDataRow
        '
        Me.uxDataRow.Location = New System.Drawing.Point(4, 46)
        Me.uxDataRow.Name = "uxDataRow"
        Me.uxDataRow.Size = New System.Drawing.Size(111, 21)
        Me.uxDataRow.TabIndex = 10
        Me.uxDataRow.Text = "Label Row:"
        Me.uxDataRow.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'uxDataStartRow
        '
        Me.uxDataStartRow.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(210, Byte), Integer))
        Me.uxDataStartRow.Location = New System.Drawing.Point(115, 47)
        Me.uxDataStartRow.MaxLength = 3
        Me.uxDataStartRow.Name = "uxDataStartRow"
        Me.uxDataStartRow.Size = New System.Drawing.Size(36, 21)
        Me.uxDataStartRow.TabIndex = 2
        '
        'uxDataStartColumn
        '
        Me.uxDataStartColumn.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(210, Byte), Integer))
        Me.uxDataStartColumn.Location = New System.Drawing.Point(279, 46)
        Me.uxDataStartColumn.MaxLength = 3
        Me.uxDataStartColumn.Name = "uxDataStartColumn"
        Me.uxDataStartColumn.Size = New System.Drawing.Size(36, 21)
        Me.uxDataStartColumn.TabIndex = 3
        '
        'ubLblStyle
        '
        Me.ubLblStyle.Location = New System.Drawing.Point(325, 20)
        Me.ubLblStyle.Name = "ubLblStyle"
        Me.ubLblStyle.Size = New System.Drawing.Size(95, 21)
        Me.ubLblStyle.TabIndex = 6
        Me.ubLblStyle.Text = "Label Style:"
        Me.ubLblStyle.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'uxScaleSymbol
        '
        Me.uxScaleSymbol.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(210, Byte), Integer))
        Me.uxScaleSymbol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.uxScaleSymbol.Location = New System.Drawing.Point(420, 74)
        Me.uxScaleSymbol.Name = "uxScaleSymbol"
        Me.uxScaleSymbol.Size = New System.Drawing.Size(200, 21)
        Me.uxScaleSymbol.TabIndex = 6
        '
        'uxFormatMask
        '
        Me.uxFormatMask.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(210, Byte), Integer))
        Me.uxFormatMask.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.uxFormatMask.Location = New System.Drawing.Point(115, 74)
        Me.uxFormatMask.Name = "uxFormatMask"
        Me.uxFormatMask.Size = New System.Drawing.Size(200, 21)
        Me.uxFormatMask.TabIndex = 5
        '
        'uxLabel
        '
        Me.uxLabel.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(210, Byte), Integer))
        Me.uxLabel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.uxLabel.Location = New System.Drawing.Point(115, 17)
        Me.uxLabel.Name = "uxLabel"
        Me.uxLabel.Size = New System.Drawing.Size(200, 21)
        Me.uxLabel.TabIndex = 0
        '
        'uxLbl
        '
        Me.uxLbl.Location = New System.Drawing.Point(1, 18)
        Me.uxLbl.Name = "uxLbl"
        Me.uxLbl.Size = New System.Drawing.Size(114, 21)
        Me.uxLbl.TabIndex = 0
        Me.uxLbl.Text = "Label:"
        Me.uxLbl.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'uxSelDataItems
        '
        Me.uxSelDataItems.Controls.Add(Me.uxSelLinkCode)
        Me.uxSelDataItems.Controls.Add(Me.uxMoveDown)
        Me.uxSelDataItems.Controls.Add(Me.uxMoveUp)
        Me.uxSelDataItems.Controls.Add(Me.uxSelectedDataItems)
        Me.uxSelDataItems.Location = New System.Drawing.Point(328, 285)
        Me.uxSelDataItems.Name = "uxSelDataItems"
        Me.uxSelDataItems.Size = New System.Drawing.Size(308, 224)
        Me.uxSelDataItems.TabIndex = 5
        Me.uxSelDataItems.TabStop = False
        Me.uxSelDataItems.Text = "Selected Data Items"
        '
        'uxSelLinkCode
        '
        Me.uxSelLinkCode.Location = New System.Drawing.Point(5, 195)
        Me.uxSelLinkCode.MaxLength = 3
        Me.uxSelLinkCode.Name = "uxSelLinkCode"
        Me.uxSelLinkCode.Size = New System.Drawing.Size(260, 21)
        Me.uxSelLinkCode.TabIndex = 10
        Me.uxSelLinkCode.TabStop = False
        Me.uxToolTip.SetToolTip(Me.uxSelLinkCode, "Link Code")
        '
        'uxMoveDown
        '
        Me.uxMoveDown.ImageAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.uxMoveDown.ImageIndex = 3
        Me.uxMoveDown.ImageList = Me.uxImageList
        Me.uxMoveDown.Location = New System.Drawing.Point(272, 102)
        Me.uxMoveDown.Name = "uxMoveDown"
        Me.uxMoveDown.Size = New System.Drawing.Size(28, 24)
        Me.uxMoveDown.TabIndex = 9
        Me.uxToolTip.SetToolTip(Me.uxMoveDown, "Move Down")
        '
        'uxMoveUp
        '
        Me.uxMoveUp.ImageAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.uxMoveUp.ImageIndex = 4
        Me.uxMoveUp.ImageList = Me.uxImageList
        Me.uxMoveUp.Location = New System.Drawing.Point(272, 74)
        Me.uxMoveUp.Name = "uxMoveUp"
        Me.uxMoveUp.Size = New System.Drawing.Size(28, 24)
        Me.uxMoveUp.TabIndex = 8
        Me.uxToolTip.SetToolTip(Me.uxMoveUp, "Move Up")
        '
        'uxSelectedDataItems
        '
        Me.uxSelectedDataItems.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxSelectedDataItems.Location = New System.Drawing.Point(5, 17)
        Me.uxSelectedDataItems.Name = "uxSelectedDataItems"
        Me.uxSelectedDataItems.Size = New System.Drawing.Size(260, 173)
        Me.uxSelectedDataItems.TabIndex = 0
        '
        'uxAvDataItems
        '
        Me.uxAvDataItems.Controls.Add(Me.uxAvailLinkCode)
        Me.uxAvDataItems.Controls.Add(Me.uxAvailableDataItems)
        Me.uxAvDataItems.Location = New System.Drawing.Point(8, 285)
        Me.uxAvDataItems.Name = "uxAvDataItems"
        Me.uxAvDataItems.Size = New System.Drawing.Size(272, 224)
        Me.uxAvDataItems.TabIndex = 4
        Me.uxAvDataItems.TabStop = False
        Me.uxAvDataItems.Text = "Available Data Items"
        '
        'uxAvailLinkCode
        '
        Me.uxAvailLinkCode.Location = New System.Drawing.Point(5, 195)
        Me.uxAvailLinkCode.MaxLength = 3
        Me.uxAvailLinkCode.Name = "uxAvailLinkCode"
        Me.uxAvailLinkCode.Size = New System.Drawing.Size(260, 21)
        Me.uxAvailLinkCode.TabIndex = 4
        Me.uxAvailLinkCode.TabStop = False
        Me.uxToolTip.SetToolTip(Me.uxAvailLinkCode, "Link Code")
        '
        'uxAvailableDataItems
        '
        Me.uxAvailableDataItems.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxAvailableDataItems.Location = New System.Drawing.Point(5, 17)
        Me.uxAvailableDataItems.Name = "uxAvailableDataItems"
        Me.uxAvailableDataItems.Size = New System.Drawing.Size(260, 173)
        Me.uxAvailableDataItems.Sorted = True
        Me.uxAvailableDataItems.TabIndex = 0
        '
        'uxRemove
        '
        Me.uxRemove.ImageAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.uxRemove.ImageIndex = 2
        Me.uxRemove.ImageList = Me.uxImageList
        Me.uxRemove.Location = New System.Drawing.Point(289, 386)
        Me.uxRemove.Name = "uxRemove"
        Me.uxRemove.Size = New System.Drawing.Size(28, 24)
        Me.uxRemove.TabIndex = 7
        Me.uxToolTip.SetToolTip(Me.uxRemove, "Remove")
        '
        'uxAdd
        '
        Me.uxAdd.ImageAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.uxAdd.ImageIndex = 1
        Me.uxAdd.ImageList = Me.uxImageList
        Me.uxAdd.Location = New System.Drawing.Point(289, 358)
        Me.uxAdd.Name = "uxAdd"
        Me.uxAdd.Size = New System.Drawing.Size(28, 24)
        Me.uxAdd.TabIndex = 6
        Me.uxToolTip.SetToolTip(Me.uxAdd, "Add")
        '
        'uxSave
        '
        Me.uxSave.Location = New System.Drawing.Point(400, 652)
        Me.uxSave.Name = "uxSave"
        Me.uxSave.Size = New System.Drawing.Size(75, 23)
        Me.uxSave.TabIndex = 9
        Me.uxSave.Text = "Save"
        '
        'uxDelete
        '
        Me.uxDelete.Location = New System.Drawing.Point(481, 652)
        Me.uxDelete.Name = "uxDelete"
        Me.uxDelete.Size = New System.Drawing.Size(75, 23)
        Me.uxDelete.TabIndex = 10
        Me.uxDelete.Text = "Delete"
        '
        'uxClose
        '
        'Me.uxClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.uxClose.Location = New System.Drawing.Point(561, 652)
        Me.uxClose.Name = "uxClose"
        Me.uxClose.Size = New System.Drawing.Size(75, 23)
        Me.uxClose.TabIndex = 11
        Me.uxClose.Text = "Close"
        '
        'uxTableSettings
        '
        Me.uxTableSettings.Controls.Add(Me.uxPeriodStartRow)
        Me.uxTableSettings.Controls.Add(Me.uxPeriodStartRowCol)
        Me.uxTableSettings.Controls.Add(Me.uxPeriodStartCol)
        Me.uxTableSettings.Controls.Add(Me.uxForwardSlash4)
        Me.uxTableSettings.Controls.Add(Me.uxDisplayPeriodLabels)
        Me.uxTableSettings.Controls.Add(Me.uxPeriodFormatStyle)
        Me.uxTableSettings.Controls.Add(Me.uxPeriodFmtStyle)
        Me.uxTableSettings.Controls.Add(Me.uxPeriodFormat)
        Me.uxTableSettings.Controls.Add(Me.uxPeriodFmt)
        Me.uxTableSettings.Controls.Add(Me.uxTitle)
        Me.uxTableSettings.Controls.Add(Me.uxYearToStyle)
        Me.uxTableSettings.Controls.Add(Me.uxForwardSlash3)
        Me.uxTableSettings.Controls.Add(Me.uxEstActIdentifier)
        Me.uxTableSettings.Controls.Add(Me.uxForwardSlash2)
        Me.uxTableSettings.Controls.Add(Me.uxEstimateIdentifier)
        Me.uxTableSettings.Controls.Add(Me.uxActualIdentifier)
        Me.uxTableSettings.Controls.Add(Me.uxYrToText)
        Me.uxTableSettings.Controls.Add(Me.uxYrToStyle)
        Me.uxTableSettings.Controls.Add(Me.uxYearToText)
        Me.uxTableSettings.Controls.Add(Me.uxSourceStyle)
        Me.uxTableSettings.Controls.Add(Me.uxSrcStyle)
        Me.uxTableSettings.Controls.Add(Me.uxTitleStyle)
        Me.uxTableSettings.Controls.Add(Me.uxTleStyle)
        Me.uxTableSettings.Controls.Add(Me.uxNoActYears)
        Me.uxTableSettings.Controls.Add(Me.uxNumberOfActualYears)
        Me.uxTableSettings.Controls.Add(Me.uxSrc)
        Me.uxTableSettings.Controls.Add(Me.uxSource)
        Me.uxTableSettings.Controls.Add(Me.uxTtle)
        Me.uxTableSettings.Controls.Add(Me.uxYearStyle)
        Me.uxTableSettings.Controls.Add(Me.uxYrStyle)
        Me.uxTableSettings.Controls.Add(Me.uxYrStartRow)
        Me.uxTableSettings.Controls.Add(Me.uxDataItemDirection)
        Me.uxTableSettings.Controls.Add(Me.uxDataItmDir)
        Me.uxTableSettings.Controls.Add(Me.uxYearStartRow)
        Me.uxTableSettings.Controls.Add(Me.uxYearStartColumn)
        Me.uxTableSettings.Controls.Add(Me.uxNumberOfYears)
        Me.uxTableSettings.Controls.Add(Me.uxForwardSlash1)
        Me.uxTableSettings.Location = New System.Drawing.Point(8, 68)
        Me.uxTableSettings.Name = "uxTableSettings"
        Me.uxTableSettings.Size = New System.Drawing.Size(628, 213)
        Me.uxTableSettings.TabIndex = 3
        Me.uxTableSettings.TabStop = False
        Me.uxTableSettings.Text = "Table Settings"
        '
        'uxPeriodStartRow
        '
        Me.uxPeriodStartRow.BackColor = System.Drawing.SystemColors.Window
        Me.uxPeriodStartRow.Enabled = False
        Me.uxPeriodStartRow.Location = New System.Drawing.Point(236, 99)
        Me.uxPeriodStartRow.MaxLength = 2
        Me.uxPeriodStartRow.Name = "uxPeriodStartRow"
        Me.uxPeriodStartRow.Size = New System.Drawing.Size(32, 21)
        Me.uxPeriodStartRow.TabIndex = 10
        '
        'uxPeriodStartRowCol
        '
        Me.uxPeriodStartRowCol.Location = New System.Drawing.Point(141, 99)
        Me.uxPeriodStartRowCol.Name = "uxPeriodStartRowCol"
        Me.uxPeriodStartRowCol.Size = New System.Drawing.Size(93, 21)
        Me.uxPeriodStartRowCol.TabIndex = 37
        Me.uxPeriodStartRowCol.Text = "Start Row/Col:"
        Me.uxPeriodStartRowCol.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'uxPeriodStartCol
        '
        Me.uxPeriodStartCol.BackColor = System.Drawing.SystemColors.Window
        Me.uxPeriodStartCol.Enabled = False
        Me.uxPeriodStartCol.Location = New System.Drawing.Point(285, 99)
        Me.uxPeriodStartCol.MaxLength = 2
        Me.uxPeriodStartCol.Name = "uxPeriodStartCol"
        Me.uxPeriodStartCol.Size = New System.Drawing.Size(32, 21)
        Me.uxPeriodStartCol.TabIndex = 11
        '
        'uxForwardSlash4
        '
        Me.uxForwardSlash4.Location = New System.Drawing.Point(271, 102)
        Me.uxForwardSlash4.Name = "uxForwardSlash4"
        Me.uxForwardSlash4.Size = New System.Drawing.Size(13, 18)
        Me.uxForwardSlash4.TabIndex = 36
        Me.uxForwardSlash4.Text = "/"
        '
        'uxDisplayPeriodLabels
        '
        Me.uxDisplayPeriodLabels.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.uxDisplayPeriodLabels.Location = New System.Drawing.Point(26, 100)
        Me.uxDisplayPeriodLabels.Name = "uxDisplayPeriodLabels"
        Me.uxDisplayPeriodLabels.Size = New System.Drawing.Size(116, 21)
        Me.uxDisplayPeriodLabels.TabIndex = 9
        Me.uxDisplayPeriodLabels.Text = "Display Period:"
        Me.uxDisplayPeriodLabels.UseVisualStyleBackColor = True
        '
        'uxPeriodFormatStyle
        '
        Me.uxPeriodFormatStyle.BackColor = System.Drawing.SystemColors.Window
        Me.uxPeriodFormatStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.uxPeriodFormatStyle.DropDownWidth = 300
        Me.uxPeriodFormatStyle.Enabled = False
        Me.uxPeriodFormatStyle.Location = New System.Drawing.Point(420, 99)
        Me.uxPeriodFormatStyle.Name = "uxPeriodFormatStyle"
        Me.uxPeriodFormatStyle.Size = New System.Drawing.Size(200, 21)
        Me.uxPeriodFormatStyle.TabIndex = 12
        '
        'uxPeriodFmtStyle
        '
        Me.uxPeriodFmtStyle.Location = New System.Drawing.Point(325, 99)
        Me.uxPeriodFmtStyle.Name = "uxPeriodFmtStyle"
        Me.uxPeriodFmtStyle.Size = New System.Drawing.Size(95, 21)
        Me.uxPeriodFmtStyle.TabIndex = 32
        Me.uxPeriodFmtStyle.Text = "Period Style:"
        Me.uxPeriodFmtStyle.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'uxPeriodFormat
        '
        Me.uxPeriodFormat.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(210, Byte), Integer))
        Me.uxPeriodFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.uxPeriodFormat.DropDownWidth = 500
        Me.uxPeriodFormat.Location = New System.Drawing.Point(117, 72)
        Me.uxPeriodFormat.Name = "uxPeriodFormat"
        Me.uxPeriodFormat.Size = New System.Drawing.Size(200, 21)
        Me.uxPeriodFormat.TabIndex = 8
        '
        'uxPeriodFmt
        '
        Me.uxPeriodFmt.Location = New System.Drawing.Point(22, 72)
        Me.uxPeriodFmt.Name = "uxPeriodFmt"
        Me.uxPeriodFmt.Size = New System.Drawing.Size(95, 21)
        Me.uxPeriodFmt.TabIndex = 30
        Me.uxPeriodFmt.Text = "Period Format:"
        Me.uxPeriodFmt.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'uxTitle
        '
        Me.uxTitle.Location = New System.Drawing.Point(117, 127)
        Me.uxTitle.Name = "uxTitle"
        Me.uxTitle.Size = New System.Drawing.Size(200, 21)
        Me.uxTitle.TabIndex = 13
        '
        'uxYearToStyle
        '
        Me.uxYearToStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.uxYearToStyle.DropDownWidth = 300
        Me.uxYearToStyle.Location = New System.Drawing.Point(420, 183)
        Me.uxYearToStyle.Name = "uxYearToStyle"
        Me.uxYearToStyle.Size = New System.Drawing.Size(200, 21)
        Me.uxYearToStyle.TabIndex = 18
        '
        'uxForwardSlash3
        '
        Me.uxForwardSlash3.Location = New System.Drawing.Point(573, 48)
        Me.uxForwardSlash3.Name = "uxForwardSlash3"
        Me.uxForwardSlash3.Size = New System.Drawing.Size(13, 18)
        Me.uxForwardSlash3.TabIndex = 28
        Me.uxForwardSlash3.Text = "/"
        '
        'uxEstActIdentifier
        '
        Me.uxEstActIdentifier.Location = New System.Drawing.Point(209, 45)
        Me.uxEstActIdentifier.Name = "uxEstActIdentifier"
        Me.uxEstActIdentifier.Size = New System.Drawing.Size(108, 21)
        Me.uxEstActIdentifier.TabIndex = 27
        Me.uxEstActIdentifier.Text = "Act/Est Identifier:"
        Me.uxEstActIdentifier.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'uxForwardSlash2
        '
        Me.uxForwardSlash2.Location = New System.Drawing.Point(356, 50)
        Me.uxForwardSlash2.Name = "uxForwardSlash2"
        Me.uxForwardSlash2.Size = New System.Drawing.Size(13, 18)
        Me.uxForwardSlash2.TabIndex = 26
        Me.uxForwardSlash2.Text = "/"
        '
        'uxEstimateIdentifier
        '
        Me.uxEstimateIdentifier.Location = New System.Drawing.Point(369, 46)
        Me.uxEstimateIdentifier.MaxLength = 2
        Me.uxEstimateIdentifier.Name = "uxEstimateIdentifier"
        Me.uxEstimateIdentifier.Size = New System.Drawing.Size(32, 21)
        Me.uxEstimateIdentifier.TabIndex = 5
        '
        'uxActualIdentifier
        '
        Me.uxActualIdentifier.Location = New System.Drawing.Point(320, 46)
        Me.uxActualIdentifier.MaxLength = 2
        Me.uxActualIdentifier.Name = "uxActualIdentifier"
        Me.uxActualIdentifier.Size = New System.Drawing.Size(32, 21)
        Me.uxActualIdentifier.TabIndex = 4
        '
        'uxYrToText
        '
        Me.uxYrToText.Location = New System.Drawing.Point(1, 183)
        Me.uxYrToText.Name = "uxYrToText"
        Me.uxYrToText.Size = New System.Drawing.Size(116, 21)
        Me.uxYrToText.TabIndex = 23
        Me.uxYrToText.Text = "Year To Text:"
        Me.uxYrToText.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'uxYrToStyle
        '
        Me.uxYrToStyle.Location = New System.Drawing.Point(325, 183)
        Me.uxYrToStyle.Name = "uxYrToStyle"
        Me.uxYrToStyle.Size = New System.Drawing.Size(95, 21)
        Me.uxYrToStyle.TabIndex = 22
        Me.uxYrToStyle.Text = "Year To Style:"
        Me.uxYrToStyle.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'uxYearToText
        '
        Me.uxYearToText.Location = New System.Drawing.Point(117, 183)
        Me.uxYearToText.Name = "uxYearToText"
        Me.uxYearToText.Size = New System.Drawing.Size(200, 21)
        Me.uxYearToText.TabIndex = 17
        '
        'uxSourceStyle
        '
        Me.uxSourceStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.uxSourceStyle.DropDownWidth = 300
        Me.uxSourceStyle.Location = New System.Drawing.Point(420, 155)
        Me.uxSourceStyle.Name = "uxSourceStyle"
        Me.uxSourceStyle.Size = New System.Drawing.Size(200, 21)
        Me.uxSourceStyle.TabIndex = 16
        '
        'uxSrcStyle
        '
        Me.uxSrcStyle.Location = New System.Drawing.Point(325, 155)
        Me.uxSrcStyle.Name = "uxSrcStyle"
        Me.uxSrcStyle.Size = New System.Drawing.Size(95, 21)
        Me.uxSrcStyle.TabIndex = 19
        Me.uxSrcStyle.Text = "Source Style:"
        Me.uxSrcStyle.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'uxTitleStyle
        '
        Me.uxTitleStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.uxTitleStyle.DropDownWidth = 300
        Me.uxTitleStyle.Location = New System.Drawing.Point(420, 127)
        Me.uxTitleStyle.Name = "uxTitleStyle"
        Me.uxTitleStyle.Size = New System.Drawing.Size(200, 21)
        Me.uxTitleStyle.TabIndex = 14
        '
        'uxTleStyle
        '
        Me.uxTleStyle.Location = New System.Drawing.Point(325, 127)
        Me.uxTleStyle.Name = "uxTleStyle"
        Me.uxTleStyle.Size = New System.Drawing.Size(95, 21)
        Me.uxTleStyle.TabIndex = 17
        Me.uxTleStyle.Text = "Title Style:"
        Me.uxTleStyle.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'uxNoActYears
        '
        Me.uxNoActYears.Location = New System.Drawing.Point(407, 45)
        Me.uxNoActYears.Name = "uxNoActYears"
        Me.uxNoActYears.Size = New System.Drawing.Size(129, 21)
        Me.uxNoActYears.TabIndex = 15
        Me.uxNoActYears.Text = "No. Actual/Total Yrs.:"
        Me.uxNoActYears.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'uxNumberOfActualYears
        '
        Me.uxNumberOfActualYears.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(210, Byte), Integer))
        Me.uxNumberOfActualYears.Location = New System.Drawing.Point(539, 45)
        Me.uxNumberOfActualYears.MaxLength = 2
        Me.uxNumberOfActualYears.Name = "uxNumberOfActualYears"
        Me.uxNumberOfActualYears.Size = New System.Drawing.Size(32, 21)
        Me.uxNumberOfActualYears.TabIndex = 6
        '
        'uxSrc
        '
        Me.uxSrc.Location = New System.Drawing.Point(1, 155)
        Me.uxSrc.Name = "uxSrc"
        Me.uxSrc.Size = New System.Drawing.Size(116, 21)
        Me.uxSrc.TabIndex = 13
        Me.uxSrc.Text = "Source:"
        Me.uxSrc.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'uxSource
        '
        Me.uxSource.Location = New System.Drawing.Point(117, 155)
        Me.uxSource.Name = "uxSource"
        Me.uxSource.Size = New System.Drawing.Size(200, 21)
        Me.uxSource.TabIndex = 15
        '
        'uxTtle
        '
        Me.uxTtle.Location = New System.Drawing.Point(1, 127)
        Me.uxTtle.Name = "uxTtle"
        Me.uxTtle.Size = New System.Drawing.Size(116, 21)
        Me.uxTtle.TabIndex = 11
        Me.uxTtle.Text = "Title:"
        Me.uxTtle.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'uxYearStyle
        '
        Me.uxYearStyle.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(210, Byte), Integer))
        Me.uxYearStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.uxYearStyle.DropDownWidth = 300
        Me.uxYearStyle.Location = New System.Drawing.Point(420, 18)
        Me.uxYearStyle.Name = "uxYearStyle"
        Me.uxYearStyle.Size = New System.Drawing.Size(200, 21)
        Me.uxYearStyle.TabIndex = 1
        '
        'uxYrStyle
        '
        Me.uxYrStyle.Location = New System.Drawing.Point(325, 18)
        Me.uxYrStyle.Name = "uxYrStyle"
        Me.uxYrStyle.Size = New System.Drawing.Size(95, 21)
        Me.uxYrStyle.TabIndex = 6
        Me.uxYrStyle.Text = "Year Style:"
        Me.uxYrStyle.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'uxYrStartRow
        '
        Me.uxYrStartRow.Location = New System.Drawing.Point(1, 47)
        Me.uxYrStartRow.Name = "uxYrStartRow"
        Me.uxYrStartRow.Size = New System.Drawing.Size(116, 21)
        Me.uxYrStartRow.TabIndex = 9
        Me.uxYrStartRow.Text = "Yr. Start Row/Col:"
        Me.uxYrStartRow.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'uxDataItemDirection
        '
        Me.uxDataItemDirection.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(210, Byte), Integer))
        Me.uxDataItemDirection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.uxDataItemDirection.Location = New System.Drawing.Point(117, 18)
        Me.uxDataItemDirection.Name = "uxDataItemDirection"
        Me.uxDataItemDirection.Size = New System.Drawing.Size(200, 21)
        Me.uxDataItemDirection.TabIndex = 0
        '
        'uxDataItmDir
        '
        Me.uxDataItmDir.Location = New System.Drawing.Point(1, 18)
        Me.uxDataItmDir.Name = "uxDataItmDir"
        Me.uxDataItmDir.Size = New System.Drawing.Size(116, 21)
        Me.uxDataItmDir.TabIndex = 8
        Me.uxDataItmDir.Text = "Data Item Dir:"
        Me.uxDataItmDir.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'uxYearStartRow
        '
        Me.uxYearStartRow.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(210, Byte), Integer))
        Me.uxYearStartRow.Location = New System.Drawing.Point(117, 46)
        Me.uxYearStartRow.MaxLength = 2
        Me.uxYearStartRow.Name = "uxYearStartRow"
        Me.uxYearStartRow.Size = New System.Drawing.Size(32, 21)
        Me.uxYearStartRow.TabIndex = 2
        '
        'uxYearStartColumn
        '
        Me.uxYearStartColumn.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(210, Byte), Integer))
        Me.uxYearStartColumn.Location = New System.Drawing.Point(166, 46)
        Me.uxYearStartColumn.MaxLength = 2
        Me.uxYearStartColumn.Name = "uxYearStartColumn"
        Me.uxYearStartColumn.Size = New System.Drawing.Size(32, 21)
        Me.uxYearStartColumn.TabIndex = 3
        '
        'uxNumberOfYears
        '
        Me.uxNumberOfYears.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(210, Byte), Integer))
        Me.uxNumberOfYears.Location = New System.Drawing.Point(586, 45)
        Me.uxNumberOfYears.MaxLength = 2
        Me.uxNumberOfYears.Name = "uxNumberOfYears"
        Me.uxNumberOfYears.Size = New System.Drawing.Size(32, 21)
        Me.uxNumberOfYears.TabIndex = 7
        '
        'uxForwardSlash1
        '
        Me.uxForwardSlash1.Location = New System.Drawing.Point(152, 49)
        Me.uxForwardSlash1.Name = "uxForwardSlash1"
        Me.uxForwardSlash1.Size = New System.Drawing.Size(13, 18)
        Me.uxForwardSlash1.TabIndex = 7
        Me.uxForwardSlash1.Text = "/"
        '
        'uxClone
        '
        Me.uxClone.ImageAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.uxClone.ImageIndex = 0
        Me.uxClone.ImageList = Me.uxImageList
        Me.uxClone.Location = New System.Drawing.Point(436, 38)
        Me.uxClone.Name = "uxClone"
        Me.uxClone.Size = New System.Drawing.Size(28, 24)
        Me.uxClone.TabIndex = 2
        Me.uxToolTip.SetToolTip(Me.uxClone, "Clone Definition")
        '
        'uxDataModel
        '
        Me.uxDataModel.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(210, Byte), Integer))
        Me.uxDataModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.uxDataModel.Location = New System.Drawing.Point(124, 39)
        Me.uxDataModel.Name = "uxDataModel"
        Me.uxDataModel.Size = New System.Drawing.Size(308, 21)
        Me.uxDataModel.TabIndex = 1
        '
        'uxDataMdl
        '
        Me.uxDataMdl.Location = New System.Drawing.Point(8, 39)
        Me.uxDataMdl.Name = "uxDataMdl"
        Me.uxDataMdl.Size = New System.Drawing.Size(116, 21)
        Me.uxDataMdl.TabIndex = 4
        Me.uxDataMdl.Text = "Data Model:"
        Me.uxDataMdl.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'uxContextName
        '
        Me.uxContextName.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(210, Byte), Integer))
        Me.uxContextName.Location = New System.Drawing.Point(124, 10)
        Me.uxContextName.Name = "uxContextName"
        Me.uxContextName.Size = New System.Drawing.Size(340, 21)
        Me.uxContextName.TabIndex = 0
        '
        'uxName
        '
        Me.uxName.Location = New System.Drawing.Point(8, 10)
        Me.uxName.Name = "uxName"
        Me.uxName.Size = New System.Drawing.Size(116, 21)
        Me.uxName.TabIndex = 3
        Me.uxName.Text = "Name:"
        Me.uxName.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'uxSaveFileDlg
        '
        Me.uxSaveFileDlg.Filter = "Word Templates (*.dot)|*.dot|Powerpoint Templates (*.pot)|*.pot"
        Me.uxSaveFileDlg.Title = "Create New"
        '
        'uxOpenFileDlg
        '
        Me.uxOpenFileDlg.Filter = "Word Templates (*.dot)|*.dot|Powerpoint Templates (*.pot)|*.pot"
        Me.uxOpenFileDlg.Title = "Clone"
        '
        'bc_am_bp_context
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(646, 688)
        Me.Controls.Add(Me.uxTableDefinition)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "bc_am_bp_context"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Data Definition"
        Me.uxTableDefinition.ResumeLayout(False)
        Me.uxTableDefinition.PerformLayout()
        Me.uxDataItemSettings.ResumeLayout(False)
        Me.uxDataItemSettings.PerformLayout()
        Me.uxSelDataItems.ResumeLayout(False)
        Me.uxSelDataItems.PerformLayout()
        Me.uxAvDataItems.ResumeLayout(False)
        Me.uxAvDataItems.PerformLayout()
        Me.uxTableSettings.ResumeLayout(False)
        Me.uxTableSettings.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public WriteOnly Property Controller() As bc_am_context

        Set(ByVal Value As bc_am_context)
            ctrllr = Value
        End Set

    End Property

    Private Sub uxDataModel_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxDataModel.SelectedIndexChanged

        'If uxDataModel.SelectedIndex > 0 Then

        If currentSelectedIndex = uxDataModel.SelectedIndex Then
            Exit Sub
        End If

        If uxSave.Enabled Then
            If MessageBox.Show("You have un-saved changes which will be lost.  Are you sure you wish to continue", _
                                "Data Definition", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                ctrllr.PopulateDataModelContextConfiguration()
            Else
                uxDataModel.SelectedIndex = currentSelectedIndex
            End If
        Else
            ctrllr.PopulateDataModelContextConfiguration()
        End If

        currentSelectedIndex = uxDataModel.SelectedIndex

        'End If

    End Sub

    Private Sub uxSelectedDataItems_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxSelectedDataItems.SelectedIndexChanged

        If uxSelectedDataItems.SelectedItems.Count > 0 Then
            ctrllr.PopulateLinkCode()
            ctrllr.PopulateFromSelectedDataItem()
            ctrllr.UpdateToSelectedDataItem(False)
        End If

    End Sub

    Private Sub uxAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxAdd.Click

        ctrllr.MoveDataItems(True)

    End Sub

    Private Sub uxRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxRemove.Click

        ctrllr.MoveDataItems(False)

    End Sub

    Private Sub uxSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxSave.Click

        ctrllr.Save()

    End Sub

    Private Sub uxDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxDelete.Click

        ctrllr.DeleteContext(Me.Tag, False, False)

    End Sub

    Private Sub uxClone_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxClone.Click

        ctrllr.CloneDataModel()

    End Sub

    Private Sub uxLabelStyle_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxLabelStyle.SelectedIndexChanged

        If ctrllr.updateSelectedItem Then
            ctrllr.UpdateToSelectedDataItem()
        End If

    End Sub

    Private Sub uxLabel_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxLabel.SelectedIndexChanged

        If ctrllr.updateSelectedItem Then
            ctrllr.UpdateToSelectedDataItem()
        End If

    End Sub

    Private Sub uxDataStartRow_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxDataStartRow.TextChanged

        If ctrllr.updateSelectedItem And uxDataStartRow.Text <> "" Then
            If uxDataItemDirection.Text = "Vertical" Then
                ctrllr.MoveSelectedDataItem(True, CInt(uxDataStartRow.Text) - 1)
            End If
            ctrllr.UpdateToSelectedDataItem()
            uxDataStartRow.SelectionStart = uxDataStartRow.TextLength
        End If

    End Sub

    Private Sub uxDataStartColumn_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxDataStartColumn.TextChanged

        If ctrllr.updateSelectedItem And uxDataStartColumn.Text <> "" Then
            If uxDataItemDirection.Text = "Horizontal" Then
                ctrllr.MoveSelectedDataItem(True, CInt(uxDataStartColumn.Text) - 1)
            End If
            ctrllr.UpdateToSelectedDataItem()
            uxDataStartColumn.SelectionStart = uxDataStartColumn.TextLength
        End If

    End Sub

    Private Sub uxDataStyle_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxDataStyle.SelectedIndexChanged

        If ctrllr.updateSelectedItem Then
            ctrllr.UpdateToSelectedDataItem()
        End If

    End Sub

    Private Sub uxFormatMask_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxFormatMask.SelectedIndexChanged

        If ctrllr.updateSelectedItem Then
            ctrllr.UpdateToSelectedDataItem()
        End If

    End Sub

    Private Sub uxScaleSymbol_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxScaleSymbol.SelectedIndexChanged

        If uxScaleSymbol.SelectedIndex = 0 Then
            uxSSCustom.Enabled = True
        Else
            uxSSCustom.Enabled = False
        End If

        If ctrllr.updateSelectedItem Then
            ctrllr.UpdateToSelectedDataItem()
        End If

    End Sub

    Private Sub uxScaleFactor_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxScaleFactor.SelectedIndexChanged

        If ctrllr.updateSelectedItem Then
            ctrllr.UpdateToSelectedDataItem()
        End If

    End Sub

    Private Sub uxDataItemDirection_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxDataItemDirection.SelectedIndexChanged

        ctrllr.Validate(False)

    End Sub

    Private Sub uxYearStyle_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxYearStyle.SelectedIndexChanged

        ctrllr.Validate(False)

    End Sub

    Private Sub uxYearStartRow_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxYearStartRow.TextChanged

        ctrllr.Validate(False)

    End Sub

    Private Sub uxYearStartColumn_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxYearStartColumn.TextChanged

        ctrllr.Validate(False)

    End Sub

    Private Sub uxNumberOfYears_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxNumberOfYears.TextChanged

        ctrllr.Validate(False)

    End Sub

    Private Sub uxYearStartRow_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles uxYearStartRow.KeyPress

        If e.KeyChar Like "[0-9]" Or e.KeyChar = vbBack Then
        Else
            e.Handled = True
        End If

    End Sub

    Private Sub uxYearStartColumn_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles uxYearStartColumn.KeyPress

        If e.KeyChar Like "[0-9]" Or e.KeyChar = vbBack Then
        Else
            e.Handled = True
        End If

    End Sub

    Private Sub uxNumberOfYears_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles uxNumberOfYears.KeyPress

        If e.KeyChar Like "[0-9]" Or e.KeyChar = vbBack Then
        Else
            e.Handled = True
        End If

    End Sub

    Private Sub uxDataStartRow_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles uxDataStartRow.KeyPress

        If e.KeyChar Like "[0-9]" Or e.KeyChar = vbBack Then
        Else
            e.Handled = True
        End If

    End Sub

    Private Sub uxDataStartColumn_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles uxDataStartColumn.KeyPress

        If e.KeyChar Like "[0-9]" Or e.KeyChar = vbBack Then
        Else
            e.Handled = True
        End If

    End Sub

    Private Sub uxScaleSymbol_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles uxScaleSymbol.TextChanged

        If ctrllr.updateSelectedItem Then
            ctrllr.UpdateToSelectedDataItem()
        End If

    End Sub

    Private Sub uxMoveUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxMoveUp.Click

        ctrllr.MoveSelectedDataItem(True)

    End Sub

    Private Sub uxMoveDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxMoveDown.Click

        ctrllr.MoveSelectedDataItem(False)

    End Sub

    Private Sub uxSSCustom_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxSSCustom.TextChanged

        If ctrllr.updateSelectedItem Then
            ctrllr.UpdateToSelectedDataItem()
        End If

    End Sub

    Private Sub uxNewLabel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxNewLabel.Click

        ctrllr.AddNewLabel()

    End Sub

    Private Sub uxAvailableDataItems_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxAvailableDataItems.SelectedIndexChanged

        ctrllr.PopulateLinkCode()

    End Sub

    Private Sub uxLinkCode_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles uxAvailLinkCode.KeyPress

        e.Handled = True

    End Sub

    Private Sub uxNumbeOfActualYears_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxNumberOfActualYears.TextChanged

        ctrllr.Validate(False)

    End Sub

    Private Sub uxNumbeOfActualYears_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles uxNumberOfActualYears.KeyPress

        If e.KeyChar Like "[0-9]" Or e.KeyChar = vbBack Then
        Else
            e.Handled = True
        End If

    End Sub

    Private Sub uxTitle_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxTitle.TextChanged

        ctrllr.Validate(False)

    End Sub

    Private Sub uxSource_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxSource.TextChanged

        ctrllr.Validate(False)

    End Sub

    Private Sub uxTitleStyle_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxTitleStyle.SelectedIndexChanged

        ctrllr.Validate(False)

    End Sub

    Private Sub uxSourceStyle_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxSourceStyle.SelectedIndexChanged

        ctrllr.Validate(False)

    End Sub

    Private Sub uxAvailableDataItems_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles uxAvailableDataItems.DoubleClick

        ctrllr.MoveDataItems(True)

    End Sub

    Private Sub uxSelectedDataItems_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles uxSelectedDataItems.DoubleClick

        ctrllr.MoveDataItems(False)

    End Sub

    Private Sub uxEditLabel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxEditLabel.Click

        ctrllr.EditLabel()

    End Sub

    Private Sub uxYearToText_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxYearToText.TextChanged

        ctrllr.Validate(False)

    End Sub

    Private Sub uxYearToStyle_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxYearToStyle.SelectedIndexChanged

        ctrllr.Validate(False)

    End Sub

    Private Sub uxContextName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxContextName.TextChanged

        ctrllr.Validate(False)

    End Sub

    Private Sub uxEstimateIdentifier_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxEstimateIdentifier.TextChanged

        ctrllr.Validate(False)

    End Sub

    Private Sub uxActualIdentifier_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxActualIdentifier.TextChanged

        ctrllr.Validate(False)

    End Sub

    Private Sub uxClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxClose.Click
        If uxSave.Enabled Then
            If MessageBox.Show("Unsaved changes will be lost are you sure you wish to exit?", "Save", _
                               MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
                Me.Close()
            End If
        Else
            Me.Close()
        End If

    End Sub

    Private Sub uxPeriodFormatStyle_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles uxPeriodFormatStyle.SelectedIndexChanged

        ctrllr.Validate(False)

    End Sub

    Private Sub uxPeriodFormat_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles uxPeriodFormat.SelectedIndexChanged

        ctrllr.Validate(False)

    End Sub

    Private Sub uxPeriodStartRow_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles uxPeriodStartRow.KeyPress

        If e.KeyChar Like "[0-9]" Or e.KeyChar = vbBack Then
        Else
            e.Handled = True
        End If

    End Sub

    Private Sub uxPeriodStartRow_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxPeriodStartRow.TextChanged

        ctrllr.Validate(False)

    End Sub

    Private Sub uxPeriodStartCol_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles uxPeriodStartCol.KeyPress

        If e.KeyChar Like "[0-9]" Or e.KeyChar = vbBack Then
        Else
            e.Handled = True
        End If

    End Sub

    Private Sub uxPeriodStartCol_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxPeriodStartCol.TextChanged

        ctrllr.Validate(False)

    End Sub

    Private Sub uxDisplayPeriodLabels_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxDisplayPeriodLabels.CheckedChanged

        If uxDisplayPeriodLabels.Checked Then
            uxPeriodStartRow.Enabled = True
            uxPeriodStartRow.BackColor = Drawing.Color.FromArgb(255, 255, 210)
            uxPeriodStartCol.Enabled = True
            uxPeriodStartCol.BackColor = Drawing.Color.FromArgb(255, 255, 210)
            uxPeriodFormatStyle.Enabled = True
            uxPeriodFormatStyle.BackColor = Drawing.Color.FromArgb(255, 255, 210)
        Else
            uxPeriodStartRow.Enabled = False
            uxPeriodStartRow.BackColor = Drawing.SystemColors.Window
            uxPeriodStartRow.Text = ""
            uxPeriodStartCol.Enabled = False
            uxPeriodStartCol.BackColor = Drawing.SystemColors.Window
            uxPeriodStartCol.Text = ""
            uxPeriodFormatStyle.Enabled = False
            uxPeriodFormatStyle.BackColor = Drawing.SystemColors.Window
            uxPeriodFormatStyle.SelectedIndex = 0
        End If

        ctrllr.Validate(False)

    End Sub
End Class
