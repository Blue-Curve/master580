<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bc_am_in_universe_builder
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_in_universe_builder))
        Me.uxAggregations = New System.Windows.Forms.GroupBox()
        Me.Ccalenderized = New System.Windows.Forms.CheckBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.csource = New System.Windows.Forms.CheckBox()
        Me.cbtargetclass2 = New System.Windows.Forms.ComboBox()
        Me.lvAggregations = New System.Windows.Forms.ListView()
        Me.ColumnHeader15 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.uxListViewImages = New System.Windows.Forms.ImageList(Me.components)
        Me.Label10 = New System.Windows.Forms.Label()
        Me.cbNumberOfYears = New System.Windows.Forms.ComboBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.cbStartYear = New System.Windows.Forms.ComboBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.cbMonthEnd = New System.Windows.Forms.ComboBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.cbExchangeRate = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cbCurrency = New System.Windows.Forms.ComboBox()
        Me.cbSourceClass = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.cbType = New System.Windows.Forms.ComboBox()
        Me.txtName = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cbTargetClass = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Bc_am_calc_search1 = New BlueCurve.Insight.AM.bc_am_calc_search()
        Me.chklocal = New System.Windows.Forms.CheckBox()
        Me.uxDetails = New System.Windows.Forms.GroupBox()
        Me.tcAggregation = New System.Windows.Forms.TabControl()
        Me.tpComposition = New System.Windows.Forms.TabPage()
        Me.pComposition = New System.Windows.Forms.Panel()
        Me.uxUCPanel = New System.Windows.Forms.Panel()
        Me.Bc_am_in_ub_listentity1 = New BlueCurve.Insight.AM.bc_am_in_ub_listentity()
        Me.uxFormula = New System.Windows.Forms.RichTextBox()
        Me.tpPreview = New System.Windows.Forms.TabPage()
        Me.pPreview = New System.Windows.Forms.Panel()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.cexchtype = New System.Windows.Forms.ComboBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Cmult = New System.Windows.Forms.ComboBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Cdps = New System.Windows.Forms.ComboBox()
        Me.uxAudit = New System.Windows.Forms.Button()
        Me.uxPeriod = New System.Windows.Forms.ComboBox()
        Me.uxPrd = New System.Windows.Forms.Label()
        Me.uxYear = New System.Windows.Forms.ComboBox()
        Me.uxEntity = New System.Windows.Forms.ComboBox()
        Me.uxYr = New System.Windows.Forms.Label()
        Me.uxEnt = New System.Windows.Forms.Label()
        Me.cbPreviewPublish = New System.Windows.Forms.ComboBox()
        Me.btnPreview = New System.Windows.Forms.Button()
        Me.lvResults = New System.Windows.Forms.ListView()
        Me.chCalculation = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.chEntity = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.chYear = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.chPeriod = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.chStage = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.chResult = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Label12 = New System.Windows.Forms.Label()
        Me.cbPreviewContributor = New System.Windows.Forms.ComboBox()
        Me.cbPreviewCalculation = New System.Windows.Forms.ComboBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.uxAggregations.SuspendLayout()
        Me.uxDetails.SuspendLayout()
        Me.tcAggregation.SuspendLayout()
        Me.tpComposition.SuspendLayout()
        Me.pComposition.SuspendLayout()
        Me.uxUCPanel.SuspendLayout()
        Me.tpPreview.SuspendLayout()
        Me.pPreview.SuspendLayout()
        Me.SuspendLayout()
        '
        'uxAggregations
        '
        Me.uxAggregations.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxAggregations.Controls.Add(Me.Ccalenderized)
        Me.uxAggregations.Controls.Add(Me.Label15)
        Me.uxAggregations.Controls.Add(Me.csource)
        Me.uxAggregations.Controls.Add(Me.cbtargetclass2)
        Me.uxAggregations.Controls.Add(Me.lvAggregations)
        Me.uxAggregations.Controls.Add(Me.Label10)
        Me.uxAggregations.Controls.Add(Me.cbNumberOfYears)
        Me.uxAggregations.Controls.Add(Me.Label9)
        Me.uxAggregations.Controls.Add(Me.cbStartYear)
        Me.uxAggregations.Controls.Add(Me.Label8)
        Me.uxAggregations.Controls.Add(Me.cbMonthEnd)
        Me.uxAggregations.Controls.Add(Me.Label7)
        Me.uxAggregations.Controls.Add(Me.cbExchangeRate)
        Me.uxAggregations.Controls.Add(Me.Label2)
        Me.uxAggregations.Controls.Add(Me.cbCurrency)
        Me.uxAggregations.Controls.Add(Me.cbSourceClass)
        Me.uxAggregations.Controls.Add(Me.Label3)
        Me.uxAggregations.Controls.Add(Me.Label5)
        Me.uxAggregations.Controls.Add(Me.cbType)
        Me.uxAggregations.Controls.Add(Me.txtName)
        Me.uxAggregations.Controls.Add(Me.Label1)
        Me.uxAggregations.Controls.Add(Me.cbTargetClass)
        Me.uxAggregations.Controls.Add(Me.Label4)
        Me.uxAggregations.Controls.Add(Me.Bc_am_calc_search1)
        Me.uxAggregations.Controls.Add(Me.chklocal)
        Me.uxAggregations.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxAggregations.Location = New System.Drawing.Point(9, 12)
        Me.uxAggregations.Name = "uxAggregations"
        Me.uxAggregations.Size = New System.Drawing.Size(891, 317)
        Me.uxAggregations.TabIndex = 0
        Me.uxAggregations.TabStop = False
        Me.uxAggregations.Text = "Aggregations"
        '
        'Ccalenderized
        '
        Me.Ccalenderized.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Ccalenderized.AutoSize = True
        Me.Ccalenderized.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Ccalenderized.Location = New System.Drawing.Point(596, 271)
        Me.Ccalenderized.Name = "Ccalenderized"
        Me.Ccalenderized.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.Ccalenderized.Size = New System.Drawing.Size(288, 17)
        Me.Ccalenderized.TabIndex = 72
        Me.Ccalenderized.Text = "Consolidate All Data First - Calenderized Only"
        Me.Ccalenderized.UseVisualStyleBackColor = True
        Me.Ccalenderized.Visible = False
        '
        'Label15
        '
        Me.Label15.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(628, 112)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(73, 13)
        Me.Label15.TabIndex = 21
        Me.Label15.Text = "Dual Class:"
        '
        'csource
        '
        Me.csource.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.csource.AutoSize = True
        Me.csource.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.csource.Location = New System.Drawing.Point(556, 294)
        Me.csource.Name = "csource"
        Me.csource.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.csource.Size = New System.Drawing.Size(328, 17)
        Me.csource.TabIndex = 71
        Me.csource.Text = "Store Consolidated Source Data - Calenderized Only"
        Me.csource.UseVisualStyleBackColor = True
        Me.csource.Visible = False
        '
        'cbtargetclass2
        '
        Me.cbtargetclass2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbtargetclass2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbtargetclass2.Enabled = False
        Me.cbtargetclass2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbtargetclass2.FormattingEnabled = True
        Me.cbtargetclass2.Location = New System.Drawing.Point(707, 109)
        Me.cbtargetclass2.Name = "cbtargetclass2"
        Me.cbtargetclass2.Size = New System.Drawing.Size(177, 21)
        Me.cbtargetclass2.TabIndex = 20
        '
        'lvAggregations
        '
        Me.lvAggregations.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvAggregations.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader15, Me.ColumnHeader5})
        Me.lvAggregations.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvAggregations.FullRowSelect = True
        Me.lvAggregations.HideSelection = False
        Me.lvAggregations.Location = New System.Drawing.Point(8, 20)
        Me.lvAggregations.MultiSelect = False
        Me.lvAggregations.Name = "lvAggregations"
        Me.lvAggregations.Size = New System.Drawing.Size(579, 245)
        Me.lvAggregations.SmallImageList = Me.uxListViewImages
        Me.lvAggregations.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lvAggregations.TabIndex = 0
        Me.lvAggregations.UseCompatibleStateImageBehavior = False
        Me.lvAggregations.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader15
        '
        Me.ColumnHeader15.Text = "Aggregation Universes"
        Me.ColumnHeader15.Width = 403
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "Active"
        Me.ColumnHeader5.Width = 66
        '
        'uxListViewImages
        '
        Me.uxListViewImages.ImageStream = CType(resources.GetObject("uxListViewImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.uxListViewImages.TransparentColor = System.Drawing.Color.Transparent
        Me.uxListViewImages.Images.SetKeyName(0, "Settings.png")
        '
        'Label10
        '
        Me.Label10.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(593, 250)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(108, 13)
        Me.Label10.TabIndex = 17
        Me.Label10.Text = "Number of years:"
        '
        'cbNumberOfYears
        '
        Me.cbNumberOfYears.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbNumberOfYears.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbNumberOfYears.Enabled = False
        Me.cbNumberOfYears.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbNumberOfYears.FormattingEnabled = True
        Me.cbNumberOfYears.Location = New System.Drawing.Point(707, 247)
        Me.cbNumberOfYears.Name = "cbNumberOfYears"
        Me.cbNumberOfYears.Size = New System.Drawing.Size(177, 21)
        Me.cbNumberOfYears.TabIndex = 18
        '
        'Label9
        '
        Me.Label9.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(631, 226)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(70, 13)
        Me.Label9.TabIndex = 15
        Me.Label9.Text = "Start Year:"
        '
        'cbStartYear
        '
        Me.cbStartYear.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbStartYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbStartYear.Enabled = False
        Me.cbStartYear.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbStartYear.FormattingEnabled = True
        Me.cbStartYear.Location = New System.Drawing.Point(707, 223)
        Me.cbStartYear.Name = "cbStartYear"
        Me.cbStartYear.Size = New System.Drawing.Size(177, 21)
        Me.cbStartYear.TabIndex = 16
        '
        'Label8
        '
        Me.Label8.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(630, 202)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(71, 13)
        Me.Label8.TabIndex = 13
        Me.Label8.Text = "Month End:"
        '
        'cbMonthEnd
        '
        Me.cbMonthEnd.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbMonthEnd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbMonthEnd.Enabled = False
        Me.cbMonthEnd.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbMonthEnd.FormattingEnabled = True
        Me.cbMonthEnd.Location = New System.Drawing.Point(707, 199)
        Me.cbMonthEnd.Name = "cbMonthEnd"
        Me.cbMonthEnd.Size = New System.Drawing.Size(177, 21)
        Me.cbMonthEnd.TabIndex = 14
        '
        'Label7
        '
        Me.Label7.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(604, 178)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(97, 13)
        Me.Label7.TabIndex = 11
        Me.Label7.Text = "Exchange Rate:"
        '
        'cbExchangeRate
        '
        Me.cbExchangeRate.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbExchangeRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbExchangeRate.Enabled = False
        Me.cbExchangeRate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbExchangeRate.FormattingEnabled = True
        Me.cbExchangeRate.Location = New System.Drawing.Point(707, 175)
        Me.cbExchangeRate.Name = "cbExchangeRate"
        Me.cbExchangeRate.Size = New System.Drawing.Size(177, 21)
        Me.cbExchangeRate.TabIndex = 12
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(595, 136)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(106, 13)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "Target Currency:"
        '
        'cbCurrency
        '
        Me.cbCurrency.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbCurrency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbCurrency.Enabled = False
        Me.cbCurrency.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbCurrency.FormattingEnabled = True
        Me.cbCurrency.Location = New System.Drawing.Point(706, 133)
        Me.cbCurrency.Name = "cbCurrency"
        Me.cbCurrency.Size = New System.Drawing.Size(179, 21)
        Me.cbCurrency.TabIndex = 10
        '
        'cbSourceClass
        '
        Me.cbSourceClass.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbSourceClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbSourceClass.Enabled = False
        Me.cbSourceClass.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbSourceClass.FormattingEnabled = True
        Me.cbSourceClass.Location = New System.Drawing.Point(708, 58)
        Me.cbSourceClass.Name = "cbSourceClass"
        Me.cbSourceClass.Size = New System.Drawing.Size(177, 21)
        Me.cbSourceClass.TabIndex = 6
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(615, 61)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(87, 13)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "Source Class:"
        '
        'Label5
        '
        Me.Label5.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(662, 37)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(40, 13)
        Me.Label5.TabIndex = 3
        Me.Label5.Text = "Type:"
        '
        'cbType
        '
        Me.cbType.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbType.Enabled = False
        Me.cbType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbType.FormattingEnabled = True
        Me.cbType.Location = New System.Drawing.Point(708, 34)
        Me.cbType.Name = "cbType"
        Me.cbType.Size = New System.Drawing.Size(177, 21)
        Me.cbType.TabIndex = 4
        '
        'txtName
        '
        Me.txtName.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtName.Enabled = False
        Me.txtName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtName.Location = New System.Drawing.Point(708, 11)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(177, 21)
        Me.txtName.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(657, 14)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(45, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Name:"
        '
        'cbTargetClass
        '
        Me.cbTargetClass.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbTargetClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbTargetClass.Enabled = False
        Me.cbTargetClass.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbTargetClass.FormattingEnabled = True
        Me.cbTargetClass.Location = New System.Drawing.Point(708, 82)
        Me.cbTargetClass.Name = "cbTargetClass"
        Me.cbTargetClass.Size = New System.Drawing.Size(177, 21)
        Me.cbTargetClass.TabIndex = 8
        '
        'Label4
        '
        Me.Label4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(618, 85)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(84, 13)
        Me.Label4.TabIndex = 7
        Me.Label4.Text = "Target Class:"
        '
        'Bc_am_calc_search1
        '
        Me.Bc_am_calc_search1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Bc_am_calc_search1.Location = New System.Drawing.Point(0, 271)
        Me.Bc_am_calc_search1.Name = "Bc_am_calc_search1"
        Me.Bc_am_calc_search1.Size = New System.Drawing.Size(489, 28)
        Me.Bc_am_calc_search1.TabIndex = 19
        Me.Bc_am_calc_search1.TabStop = False
        '
        'chklocal
        '
        Me.chklocal.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chklocal.AutoSize = True
        Me.chklocal.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chklocal.Location = New System.Drawing.Point(675, 158)
        Me.chklocal.Name = "chklocal"
        Me.chklocal.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.chklocal.Size = New System.Drawing.Size(209, 17)
        Me.chklocal.TabIndex = 73
        Me.chklocal.Text = "Use Local Currency if applicable"
        Me.chklocal.UseVisualStyleBackColor = True
        '
        'uxDetails
        '
        Me.uxDetails.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxDetails.Controls.Add(Me.tcAggregation)
        Me.uxDetails.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxDetails.Location = New System.Drawing.Point(9, 325)
        Me.uxDetails.Name = "uxDetails"
        Me.uxDetails.Size = New System.Drawing.Size(891, 234)
        Me.uxDetails.TabIndex = 1
        Me.uxDetails.TabStop = False
        Me.uxDetails.Text = "Details"
        '
        'tcAggregation
        '
        Me.tcAggregation.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tcAggregation.Controls.Add(Me.tpComposition)
        Me.tcAggregation.Controls.Add(Me.tpPreview)
        Me.tcAggregation.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tcAggregation.Location = New System.Drawing.Point(7, 19)
        Me.tcAggregation.Name = "tcAggregation"
        Me.tcAggregation.SelectedIndex = 0
        Me.tcAggregation.Size = New System.Drawing.Size(877, 209)
        Me.tcAggregation.TabIndex = 0
        '
        'tpComposition
        '
        Me.tpComposition.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.tpComposition.Controls.Add(Me.pComposition)
        Me.tpComposition.Location = New System.Drawing.Point(4, 22)
        Me.tpComposition.Name = "tpComposition"
        Me.tpComposition.Padding = New System.Windows.Forms.Padding(3)
        Me.tpComposition.Size = New System.Drawing.Size(869, 183)
        Me.tpComposition.TabIndex = 0
        Me.tpComposition.Text = "Composition"
        Me.tpComposition.UseVisualStyleBackColor = True
        '
        'pComposition
        '
        Me.pComposition.AutoScroll = True
        Me.pComposition.Controls.Add(Me.uxUCPanel)
        Me.pComposition.Controls.Add(Me.uxFormula)
        Me.pComposition.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pComposition.Location = New System.Drawing.Point(3, 3)
        Me.pComposition.Name = "pComposition"
        Me.pComposition.Size = New System.Drawing.Size(863, 177)
        Me.pComposition.TabIndex = 0
        '
        'uxUCPanel
        '
        Me.uxUCPanel.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxUCPanel.AutoScroll = True
        Me.uxUCPanel.Controls.Add(Me.Bc_am_in_ub_listentity1)
        Me.uxUCPanel.Location = New System.Drawing.Point(0, 0)
        Me.uxUCPanel.Name = "uxUCPanel"
        Me.uxUCPanel.Size = New System.Drawing.Size(863, 103)
        Me.uxUCPanel.TabIndex = 44
        '
        'Bc_am_in_ub_listentity1
        '
        Me.Bc_am_in_ub_listentity1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Bc_am_in_ub_listentity1.BackColor = System.Drawing.Color.Transparent
        Me.Bc_am_in_ub_listentity1.Location = New System.Drawing.Point(4, 4)
        Me.Bc_am_in_ub_listentity1.Name = "Bc_am_in_ub_listentity1"
        Me.Bc_am_in_ub_listentity1.Size = New System.Drawing.Size(862, 32)
        Me.Bc_am_in_ub_listentity1.TabIndex = 0
        '
        'uxFormula
        '
        Me.uxFormula.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxFormula.Location = New System.Drawing.Point(3, 109)
        Me.uxFormula.Name = "uxFormula"
        Me.uxFormula.ReadOnly = True
        Me.uxFormula.Size = New System.Drawing.Size(854, 55)
        Me.uxFormula.TabIndex = 1
        Me.uxFormula.Text = ""
        '
        'tpPreview
        '
        Me.tpPreview.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.tpPreview.Controls.Add(Me.pPreview)
        Me.tpPreview.Location = New System.Drawing.Point(4, 22)
        Me.tpPreview.Name = "tpPreview"
        Me.tpPreview.Padding = New System.Windows.Forms.Padding(3)
        Me.tpPreview.Size = New System.Drawing.Size(869, 183)
        Me.tpPreview.TabIndex = 2
        Me.tpPreview.Text = "Preview"
        Me.tpPreview.UseVisualStyleBackColor = True
        '
        'pPreview
        '
        Me.pPreview.AutoScroll = True
        Me.pPreview.Controls.Add(Me.Label16)
        Me.pPreview.Controls.Add(Me.cexchtype)
        Me.pPreview.Controls.Add(Me.Button1)
        Me.pPreview.Controls.Add(Me.Cmult)
        Me.pPreview.Controls.Add(Me.Label14)
        Me.pPreview.Controls.Add(Me.Label13)
        Me.pPreview.Controls.Add(Me.Cdps)
        Me.pPreview.Controls.Add(Me.uxAudit)
        Me.pPreview.Controls.Add(Me.uxPeriod)
        Me.pPreview.Controls.Add(Me.uxPrd)
        Me.pPreview.Controls.Add(Me.uxYear)
        Me.pPreview.Controls.Add(Me.uxEntity)
        Me.pPreview.Controls.Add(Me.uxYr)
        Me.pPreview.Controls.Add(Me.uxEnt)
        Me.pPreview.Controls.Add(Me.cbPreviewPublish)
        Me.pPreview.Controls.Add(Me.btnPreview)
        Me.pPreview.Controls.Add(Me.lvResults)
        Me.pPreview.Controls.Add(Me.Label12)
        Me.pPreview.Controls.Add(Me.cbPreviewContributor)
        Me.pPreview.Controls.Add(Me.cbPreviewCalculation)
        Me.pPreview.Controls.Add(Me.Label11)
        Me.pPreview.Controls.Add(Me.Label6)
        Me.pPreview.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pPreview.Location = New System.Drawing.Point(3, 3)
        Me.pPreview.Name = "pPreview"
        Me.pPreview.Size = New System.Drawing.Size(863, 177)
        Me.pPreview.TabIndex = 0
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.Location = New System.Drawing.Point(291, 57)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(99, 13)
        Me.Label16.TabIndex = 21
        Me.Label16.Text = "Exchange Type:"
        '
        'cexchtype
        '
        Me.cexchtype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cexchtype.Enabled = False
        Me.cexchtype.FormattingEnabled = True
        Me.cexchtype.Location = New System.Drawing.Point(395, 51)
        Me.cexchtype.Name = "cexchtype"
        Me.cexchtype.Size = New System.Drawing.Size(201, 21)
        Me.cexchtype.TabIndex = 20
        '
        'Button1
        '
        Me.Button1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button1.BackColor = System.Drawing.SystemColors.Control
        Me.Button1.Location = New System.Drawing.Point(785, 27)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 8
        Me.Button1.Text = "Run Now"
        Me.Button1.UseVisualStyleBackColor = False
        '
        'Cmult
        '
        Me.Cmult.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Cmult.FormattingEnabled = True
        Me.Cmult.Location = New System.Drawing.Point(718, 51)
        Me.Cmult.Name = "Cmult"
        Me.Cmult.Size = New System.Drawing.Size(53, 21)
        Me.Cmult.TabIndex = 7
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(617, 56)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(82, 13)
        Me.Label14.TabIndex = 19
        Me.Label14.Text = "Scale Factor:"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(614, 13)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(98, 13)
        Me.Label13.TabIndex = 18
        Me.Label13.Text = "Decimal Places:"
        '
        'Cdps
        '
        Me.Cdps.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Cdps.FormattingEnabled = True
        Me.Cdps.Location = New System.Drawing.Point(718, 6)
        Me.Cdps.Name = "Cdps"
        Me.Cdps.Size = New System.Drawing.Size(50, 21)
        Me.Cdps.TabIndex = 6
        '
        'uxAudit
        '
        Me.uxAudit.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxAudit.BackColor = System.Drawing.SystemColors.Control
        Me.uxAudit.Location = New System.Drawing.Point(785, 51)
        Me.uxAudit.Name = "uxAudit"
        Me.uxAudit.Size = New System.Drawing.Size(75, 23)
        Me.uxAudit.TabIndex = 10
        Me.uxAudit.Text = "View Audit"
        Me.uxAudit.UseVisualStyleBackColor = False
        '
        'uxPeriod
        '
        Me.uxPeriod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.uxPeriod.FormattingEnabled = True
        Me.uxPeriod.Location = New System.Drawing.Point(220, 51)
        Me.uxPeriod.Name = "uxPeriod"
        Me.uxPeriod.Size = New System.Drawing.Size(65, 21)
        Me.uxPeriod.TabIndex = 3
        '
        'uxPrd
        '
        Me.uxPrd.AutoSize = True
        Me.uxPrd.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxPrd.Location = New System.Drawing.Point(166, 54)
        Me.uxPrd.Name = "uxPrd"
        Me.uxPrd.Size = New System.Drawing.Size(48, 13)
        Me.uxPrd.TabIndex = 15
        Me.uxPrd.Text = "Period:"
        '
        'uxYear
        '
        Me.uxYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.uxYear.FormattingEnabled = True
        Me.uxYear.Location = New System.Drawing.Point(84, 51)
        Me.uxYear.Name = "uxYear"
        Me.uxYear.Size = New System.Drawing.Size(65, 21)
        Me.uxYear.TabIndex = 2
        '
        'uxEntity
        '
        Me.uxEntity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.uxEntity.FormattingEnabled = True
        Me.uxEntity.Location = New System.Drawing.Point(395, 3)
        Me.uxEntity.Name = "uxEntity"
        Me.uxEntity.Size = New System.Drawing.Size(201, 21)
        Me.uxEntity.TabIndex = 4
        '
        'uxYr
        '
        Me.uxYr.AutoSize = True
        Me.uxYr.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxYr.Location = New System.Drawing.Point(3, 54)
        Me.uxYr.Name = "uxYr"
        Me.uxYr.Size = New System.Drawing.Size(38, 13)
        Me.uxYr.TabIndex = 14
        Me.uxYr.Text = "Year:"
        '
        'uxEnt
        '
        Me.uxEnt.AutoSize = True
        Me.uxEnt.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxEnt.Location = New System.Drawing.Point(291, 6)
        Me.uxEnt.Name = "uxEnt"
        Me.uxEnt.Size = New System.Drawing.Size(44, 13)
        Me.uxEnt.TabIndex = 16
        Me.uxEnt.Text = "Entity:"
        '
        'cbPreviewPublish
        '
        Me.cbPreviewPublish.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbPreviewPublish.FormattingEnabled = True
        Me.cbPreviewPublish.Location = New System.Drawing.Point(84, 27)
        Me.cbPreviewPublish.Name = "cbPreviewPublish"
        Me.cbPreviewPublish.Size = New System.Drawing.Size(201, 21)
        Me.cbPreviewPublish.TabIndex = 1
        '
        'btnPreview
        '
        Me.btnPreview.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPreview.BackColor = System.Drawing.SystemColors.Control
        Me.btnPreview.Location = New System.Drawing.Point(785, 3)
        Me.btnPreview.Name = "btnPreview"
        Me.btnPreview.Size = New System.Drawing.Size(75, 23)
        Me.btnPreview.TabIndex = 9
        Me.btnPreview.Text = "Preview"
        Me.btnPreview.UseVisualStyleBackColor = False
        '
        'lvResults
        '
        Me.lvResults.AllowColumnReorder = True
        Me.lvResults.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvResults.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.chCalculation, Me.chEntity, Me.chYear, Me.chPeriod, Me.chStage, Me.chResult, Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader6})
        Me.lvResults.FullRowSelect = True
        Me.lvResults.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.lvResults.HideSelection = False
        Me.lvResults.HoverSelection = True
        Me.lvResults.Location = New System.Drawing.Point(0, 78)
        Me.lvResults.Name = "lvResults"
        Me.lvResults.Size = New System.Drawing.Size(860, 94)
        Me.lvResults.TabIndex = 11
        Me.lvResults.UseCompatibleStateImageBehavior = False
        Me.lvResults.View = System.Windows.Forms.View.Details
        '
        'chCalculation
        '
        Me.chCalculation.Text = "Value"
        Me.chCalculation.Width = 134
        '
        'chEntity
        '
        Me.chEntity.Text = "Year"
        Me.chEntity.Width = 59
        '
        'chYear
        '
        Me.chYear.Text = "Period"
        Me.chYear.Width = 51
        '
        'chPeriod
        '
        Me.chPeriod.Text = "Entity"
        Me.chPeriod.Width = 93
        '
        'chStage
        '
        Me.chStage.Text = "Name"
        Me.chStage.Width = 87
        '
        'chResult
        '
        Me.chResult.Text = "Stage"
        Me.chResult.Width = 58
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Contributor"
        Me.ColumnHeader1.Width = 162
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Universe"
        Me.ColumnHeader2.Width = 74
        '
        'ColumnHeader6
        '
        Me.ColumnHeader6.Text = "Exchange Method"
        Me.ColumnHeader6.Width = 129
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(3, 30)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(45, 13)
        Me.Label12.TabIndex = 13
        Me.Label12.Text = "Stage:"
        '
        'cbPreviewContributor
        '
        Me.cbPreviewContributor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbPreviewContributor.FormattingEnabled = True
        Me.cbPreviewContributor.Location = New System.Drawing.Point(395, 27)
        Me.cbPreviewContributor.Name = "cbPreviewContributor"
        Me.cbPreviewContributor.Size = New System.Drawing.Size(201, 21)
        Me.cbPreviewContributor.TabIndex = 5
        '
        'cbPreviewCalculation
        '
        Me.cbPreviewCalculation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbPreviewCalculation.FormattingEnabled = True
        Me.cbPreviewCalculation.Location = New System.Drawing.Point(84, 3)
        Me.cbPreviewCalculation.Name = "cbPreviewCalculation"
        Me.cbPreviewCalculation.Size = New System.Drawing.Size(201, 21)
        Me.cbPreviewCalculation.TabIndex = 0
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(291, 30)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(77, 13)
        Me.Label11.TabIndex = 17
        Me.Label11.Text = "Contributor:"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(3, 6)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(75, 13)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = "Calculation:"
        '
        'bc_am_in_universe_builder
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(907, 573)
        Me.Controls.Add(Me.uxDetails)
        Me.Controls.Add(Me.uxAggregations)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Location = New System.Drawing.Point(50, 50)
        Me.Name = "bc_am_in_universe_builder"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "bc_am_in_universe_builder"
        Me.uxAggregations.ResumeLayout(False)
        Me.uxAggregations.PerformLayout()
        Me.uxDetails.ResumeLayout(False)
        Me.tcAggregation.ResumeLayout(False)
        Me.tpComposition.ResumeLayout(False)
        Me.pComposition.ResumeLayout(False)
        Me.uxUCPanel.ResumeLayout(False)
        Me.tpPreview.ResumeLayout(False)
        Me.pPreview.ResumeLayout(False)
        Me.pPreview.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents uxAggregations As System.Windows.Forms.GroupBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents cbNumberOfYears As System.Windows.Forms.ComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents cbStartYear As System.Windows.Forms.ComboBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents cbMonthEnd As System.Windows.Forms.ComboBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents cbExchangeRate As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cbCurrency As System.Windows.Forms.ComboBox
    Friend WithEvents cbSourceClass As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents cbType As System.Windows.Forms.ComboBox
    Friend WithEvents txtName As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cbTargetClass As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents lvAggregations As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader15 As System.Windows.Forms.ColumnHeader
    Friend WithEvents uxDetails As System.Windows.Forms.GroupBox
    Friend WithEvents tcAggregation As System.Windows.Forms.TabControl
    Friend WithEvents tpComposition As System.Windows.Forms.TabPage
    Friend WithEvents pComposition As System.Windows.Forms.Panel
    Friend WithEvents uxUCPanel As System.Windows.Forms.Panel
    Friend WithEvents Bc_am_in_ub_listentity1 As BlueCurve.Insight.AM.bc_am_in_ub_listentity
    Friend WithEvents uxFormula As System.Windows.Forms.RichTextBox
    Friend WithEvents tpPreview As System.Windows.Forms.TabPage
    Friend WithEvents pPreview As System.Windows.Forms.Panel
    Friend WithEvents btnPreview As System.Windows.Forms.Button
    Friend WithEvents lvResults As System.Windows.Forms.ListView
    Friend WithEvents chResult As System.Windows.Forms.ColumnHeader
    Friend WithEvents chYear As System.Windows.Forms.ColumnHeader
    Friend WithEvents chPeriod As System.Windows.Forms.ColumnHeader
    Friend WithEvents chEntity As System.Windows.Forms.ColumnHeader
    Friend WithEvents chCalculation As System.Windows.Forms.ColumnHeader
    Friend WithEvents chStage As System.Windows.Forms.ColumnHeader
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents cbPreviewContributor As System.Windows.Forms.ComboBox
    Friend WithEvents cbPreviewCalculation As System.Windows.Forms.ComboBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents uxPeriod As System.Windows.Forms.ComboBox
    Friend WithEvents uxPrd As System.Windows.Forms.Label
    Friend WithEvents uxYear As System.Windows.Forms.ComboBox
    Friend WithEvents uxEntity As System.Windows.Forms.ComboBox
    Friend WithEvents uxYr As System.Windows.Forms.Label
    Friend WithEvents uxEnt As System.Windows.Forms.Label
    Friend WithEvents cbPreviewPublish As System.Windows.Forms.ComboBox
    Friend WithEvents uxAudit As System.Windows.Forms.Button
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Cdps As System.Windows.Forms.ComboBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Cmult As System.Windows.Forms.ComboBox
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Bc_am_calc_search1 As BlueCurve.Insight.AM.bc_am_calc_search
    Friend WithEvents uxListViewImages As System.Windows.Forms.ImageList
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents cbtargetclass2 As System.Windows.Forms.ComboBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents cexchtype As System.Windows.Forms.ComboBox
    Friend WithEvents ColumnHeader6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Ccalenderized As System.Windows.Forms.CheckBox
    Friend WithEvents csource As System.Windows.Forms.CheckBox
    Friend WithEvents chklocal As System.Windows.Forms.CheckBox
End Class
