Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports System.Collections
Imports System.Windows.Forms
REM FIL 5.2 August 2014
Public Class bc_am_calculations
    Inherits System.Windows.Forms.Form

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
    Friend WithEvents cbTemplates As System.Windows.Forms.ComboBox
    Friend WithEvents Template As System.Windows.Forms.Label
    Friend WithEvents lvCalculations As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents tformula As System.Windows.Forms.RichTextBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents pcopy As System.Windows.Forms.Panel
    Friend WithEvents cbCopy As System.Windows.Forms.ComboBox
    Friend WithEvents lvCalculationsCopy As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ColumnHeader6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader7 As System.Windows.Forms.ColumnHeader
    Friend WithEvents uxToolBarImages As System.Windows.Forms.ImageList
    Friend WithEvents ToolBar1 As System.Windows.Forms.ToolBar
    Friend WithEvents bnew As System.Windows.Forms.ToolBarButton
    Friend WithEvents bmod As System.Windows.Forms.ToolBarButton
    Friend WithEvents bcopy As System.Windows.Forms.ToolBarButton
    Friend WithEvents bdel As System.Windows.Forms.ToolBarButton
    Friend WithEvents ToolBarButton1 As System.Windows.Forms.ToolBarButton
    Friend WithEvents bsel As System.Windows.Forms.ToolBarButton
    Friend WithEvents ball As System.Windows.Forms.ToolBarButton
    Friend WithEvents ilCopyFunctions As System.Windows.Forms.ImageList
    Friend WithEvents tbCopyFunctions As System.Windows.Forms.ToolBar
    Friend WithEvents ToolBarButton2 As System.Windows.Forms.ToolBarButton
    Friend WithEvents ToolBarButton3 As System.Windows.Forms.ToolBarButton
    Friend WithEvents ToolBarButton4 As System.Windows.Forms.ToolBarButton
    Friend WithEvents ToolBarButton5 As System.Windows.Forms.ToolBarButton
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cbAggregations As System.Windows.Forms.ComboBox
    Friend WithEvents pTemplates As System.Windows.Forms.Panel
    Friend WithEvents pAggregations As System.Windows.Forms.Panel
    Friend WithEvents ColumnHeader8 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader9 As System.Windows.Forms.ColumnHeader
    Friend WithEvents bcopyinto As System.Windows.Forms.ToolBarButton
    Friend WithEvents Bok As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_calculations))
        Me.cbTemplates = New System.Windows.Forms.ComboBox
        Me.Template = New System.Windows.Forms.Label
        Me.lvCalculations = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader6 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader4 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader7 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader8 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader9 = New System.Windows.Forms.ColumnHeader
        Me.tformula = New System.Windows.Forms.RichTextBox
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.pAggregations = New System.Windows.Forms.Panel
        Me.Label2 = New System.Windows.Forms.Label
        Me.cbAggregations = New System.Windows.Forms.ComboBox
        Me.pTemplates = New System.Windows.Forms.Panel
        Me.tbCopyFunctions = New System.Windows.Forms.ToolBar
        Me.ToolBarButton1 = New System.Windows.Forms.ToolBarButton
        Me.bsel = New System.Windows.Forms.ToolBarButton
        Me.ball = New System.Windows.Forms.ToolBarButton
        Me.uxToolBarImages = New System.Windows.Forms.ImageList(Me.components)
        Me.Bok = New System.Windows.Forms.Button
        Me.pcopy = New System.Windows.Forms.Panel
        Me.cbCopy = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.lvCalculationsCopy = New System.Windows.Forms.ListView
        Me.ColumnHeader5 = New System.Windows.Forms.ColumnHeader
        Me.ToolBar1 = New System.Windows.Forms.ToolBar
        Me.bnew = New System.Windows.Forms.ToolBarButton
        Me.bmod = New System.Windows.Forms.ToolBarButton
        Me.bcopy = New System.Windows.Forms.ToolBarButton
        Me.bdel = New System.Windows.Forms.ToolBarButton
        Me.bcopyinto = New System.Windows.Forms.ToolBarButton
        Me.ilCopyFunctions = New System.Windows.Forms.ImageList(Me.components)
        Me.ToolBarButton2 = New System.Windows.Forms.ToolBarButton
        Me.ToolBarButton3 = New System.Windows.Forms.ToolBarButton
        Me.ToolBarButton4 = New System.Windows.Forms.ToolBarButton
        Me.ToolBarButton5 = New System.Windows.Forms.ToolBarButton
        Me.Panel1.SuspendLayout()
        Me.pAggregations.SuspendLayout()
        Me.pTemplates.SuspendLayout()
        Me.pcopy.SuspendLayout()
        Me.SuspendLayout()
        '
        'cbTemplates
        '
        Me.cbTemplates.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbTemplates.Location = New System.Drawing.Point(70, 0)
        Me.cbTemplates.Name = "cbTemplates"
        Me.cbTemplates.Size = New System.Drawing.Size(152, 21)
        Me.cbTemplates.TabIndex = 0
        '
        'Template
        '
        Me.Template.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Template.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Template.Location = New System.Drawing.Point(0, 3)
        Me.Template.Name = "Template"
        Me.Template.Size = New System.Drawing.Size(64, 16)
        Me.Template.TabIndex = 1
        Me.Template.Text = "Template"
        '
        'lvCalculations
        '
        Me.lvCalculations.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvCalculations.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader6, Me.ColumnHeader4, Me.ColumnHeader3, Me.ColumnHeader2, Me.ColumnHeader7, Me.ColumnHeader8, Me.ColumnHeader9})
        Me.lvCalculations.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvCalculations.FullRowSelect = True
        Me.lvCalculations.HideSelection = False
        Me.lvCalculations.Location = New System.Drawing.Point(16, 96)
        Me.lvCalculations.MultiSelect = False
        Me.lvCalculations.Name = "lvCalculations"
        Me.lvCalculations.Size = New System.Drawing.Size(390, 216)
        Me.lvCalculations.TabIndex = 2
        Me.lvCalculations.UseCompatibleStateImageBehavior = False
        Me.lvCalculations.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Name"
        Me.ColumnHeader1.Width = 300
        '
        'ColumnHeader6
        '
        Me.ColumnHeader6.Text = "Type"
        Me.ColumnHeader6.Width = 130
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Formula"
        Me.ColumnHeader4.Width = 0
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Price Sensitve"
        Me.ColumnHeader3.Width = 100
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Order"
        Me.ColumnHeader2.Width = 50
        '
        'ColumnHeader7
        '
        Me.ColumnHeader7.Text = "id"
        Me.ColumnHeader7.Width = 0
        '
        'ColumnHeader8
        '
        Me.ColumnHeader8.Text = "Inactive"
        Me.ColumnHeader8.Width = 75
        '
        'ColumnHeader9
        '
        Me.ColumnHeader9.Text = "Visible"
        Me.ColumnHeader9.Width = 75
        '
        'tformula
        '
        Me.tformula.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tformula.BackColor = System.Drawing.Color.WhiteSmoke
        Me.tformula.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tformula.Location = New System.Drawing.Point(16, 320)
        Me.tformula.Name = "tformula"
        Me.tformula.ReadOnly = True
        Me.tformula.Size = New System.Drawing.Size(746, 72)
        Me.tformula.TabIndex = 7
        Me.tformula.Text = ""
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.pAggregations)
        Me.Panel1.Controls.Add(Me.pTemplates)
        Me.Panel1.Controls.Add(Me.tbCopyFunctions)
        Me.Panel1.Controls.Add(Me.Bok)
        Me.Panel1.Controls.Add(Me.pcopy)
        Me.Panel1.Controls.Add(Me.ToolBar1)
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(778, 432)
        Me.Panel1.TabIndex = 11
        '
        'pAggregations
        '
        Me.pAggregations.Controls.Add(Me.Label2)
        Me.pAggregations.Controls.Add(Me.cbAggregations)
        Me.pAggregations.Location = New System.Drawing.Point(276, 8)
        Me.pAggregations.Name = "pAggregations"
        Me.pAggregations.Size = New System.Drawing.Size(244, 26)
        Me.pAggregations.TabIndex = 90
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Label2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(0, 3)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(80, 16)
        Me.Label2.TabIndex = 88
        Me.Label2.Text = "Aggregation"
        '
        'cbAggregations
        '
        Me.cbAggregations.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbAggregations.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbAggregations.Location = New System.Drawing.Point(80, 0)
        Me.cbAggregations.Name = "cbAggregations"
        Me.cbAggregations.Size = New System.Drawing.Size(152, 21)
        Me.cbAggregations.TabIndex = 87
        '
        'pTemplates
        '
        Me.pTemplates.Controls.Add(Me.Template)
        Me.pTemplates.Controls.Add(Me.cbTemplates)
        Me.pTemplates.Location = New System.Drawing.Point(11, 8)
        Me.pTemplates.Name = "pTemplates"
        Me.pTemplates.Size = New System.Drawing.Size(244, 26)
        Me.pTemplates.TabIndex = 89
        '
        'tbCopyFunctions
        '
        Me.tbCopyFunctions.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbCopyFunctions.Appearance = System.Windows.Forms.ToolBarAppearance.Flat
        Me.tbCopyFunctions.AutoSize = False
        Me.tbCopyFunctions.Buttons.AddRange(New System.Windows.Forms.ToolBarButton() {Me.ToolBarButton1, Me.bsel, Me.ball})
        Me.tbCopyFunctions.Dock = System.Windows.Forms.DockStyle.None
        Me.tbCopyFunctions.DropDownArrows = True
        Me.tbCopyFunctions.ImageList = Me.uxToolBarImages
        Me.tbCopyFunctions.Location = New System.Drawing.Point(424, 40)
        Me.tbCopyFunctions.Name = "tbCopyFunctions"
        Me.tbCopyFunctions.ShowToolTips = True
        Me.tbCopyFunctions.Size = New System.Drawing.Size(190, 44)
        Me.tbCopyFunctions.TabIndex = 86
        Me.tbCopyFunctions.Visible = False
        '
        'ToolBarButton1
        '
        Me.ToolBarButton1.Name = "ToolBarButton1"
        Me.ToolBarButton1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator
        '
        'bsel
        '
        Me.bsel.ImageIndex = 6
        Me.bsel.Name = "bsel"
        Me.bsel.Text = "Copy &Checked"
        Me.bsel.ToolTipText = "Copy selected calculations into template"
        '
        'ball
        '
        Me.ball.ImageIndex = 2
        Me.ball.Name = "ball"
        Me.ball.Text = "Select &All"
        '
        'uxToolBarImages
        '
        Me.uxToolBarImages.ImageStream = CType(resources.GetObject("uxToolBarImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.uxToolBarImages.TransparentColor = System.Drawing.Color.Transparent
        Me.uxToolBarImages.Images.SetKeyName(0, "")
        Me.uxToolBarImages.Images.SetKeyName(1, "")
        Me.uxToolBarImages.Images.SetKeyName(2, "")
        Me.uxToolBarImages.Images.SetKeyName(3, "")
        Me.uxToolBarImages.Images.SetKeyName(4, "")
        Me.uxToolBarImages.Images.SetKeyName(5, "")
        Me.uxToolBarImages.Images.SetKeyName(6, "")
        Me.uxToolBarImages.Images.SetKeyName(7, "")
        Me.uxToolBarImages.Images.SetKeyName(8, "")
        Me.uxToolBarImages.Images.SetKeyName(9, "")
        Me.uxToolBarImages.Images.SetKeyName(10, "")
        Me.uxToolBarImages.Images.SetKeyName(11, "")
        Me.uxToolBarImages.Images.SetKeyName(12, "")
        '
        'Bok
        '
        Me.Bok.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Bok.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Bok.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Bok.Location = New System.Drawing.Point(690, 400)
        Me.Bok.Name = "Bok"
        Me.Bok.Size = New System.Drawing.Size(75, 23)
        Me.Bok.TabIndex = 85
        Me.Bok.Text = "Close"
        '
        'pcopy
        '
        Me.pcopy.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pcopy.Controls.Add(Me.cbCopy)
        Me.pcopy.Controls.Add(Me.Label1)
        Me.pcopy.Controls.Add(Me.lvCalculationsCopy)
        Me.pcopy.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pcopy.Location = New System.Drawing.Point(411, 95)
        Me.pcopy.Name = "pcopy"
        Me.pcopy.Size = New System.Drawing.Size(356, 218)
        Me.pcopy.TabIndex = 1
        Me.pcopy.Visible = False
        '
        'cbCopy
        '
        Me.cbCopy.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbCopy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbCopy.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbCopy.Location = New System.Drawing.Point(89, 1)
        Me.cbCopy.Name = "cbCopy"
        Me.cbCopy.Size = New System.Drawing.Size(259, 21)
        Me.cbCopy.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(11, 3)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(72, 18)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Copy from"
        '
        'lvCalculationsCopy
        '
        Me.lvCalculationsCopy.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvCalculationsCopy.CheckBoxes = True
        Me.lvCalculationsCopy.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader5})
        Me.lvCalculationsCopy.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvCalculationsCopy.FullRowSelect = True
        Me.lvCalculationsCopy.Location = New System.Drawing.Point(13, 24)
        Me.lvCalculationsCopy.Name = "lvCalculationsCopy"
        Me.lvCalculationsCopy.Size = New System.Drawing.Size(335, 191)
        Me.lvCalculationsCopy.TabIndex = 1
        Me.lvCalculationsCopy.UseCompatibleStateImageBehavior = False
        Me.lvCalculationsCopy.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "Calculation"
        Me.ColumnHeader5.Width = 298
        '
        'ToolBar1
        '
        Me.ToolBar1.Appearance = System.Windows.Forms.ToolBarAppearance.Flat
        Me.ToolBar1.AutoSize = False
        Me.ToolBar1.Buttons.AddRange(New System.Windows.Forms.ToolBarButton() {Me.bnew, Me.bmod, Me.bcopy, Me.bdel, Me.bcopyinto})
        Me.ToolBar1.Dock = System.Windows.Forms.DockStyle.None
        Me.ToolBar1.DropDownArrows = True
        Me.ToolBar1.ImageList = Me.uxToolBarImages
        Me.ToolBar1.Location = New System.Drawing.Point(16, 40)
        Me.ToolBar1.Name = "ToolBar1"
        Me.ToolBar1.ShowToolTips = True
        Me.ToolBar1.Size = New System.Drawing.Size(531, 44)
        Me.ToolBar1.TabIndex = 50
        '
        'bnew
        '
        Me.bnew.ImageIndex = 7
        Me.bnew.Name = "bnew"
        Me.bnew.Text = "&New"
        Me.bnew.ToolTipText = "Creates a new calcualtion"
        '
        'bmod
        '
        Me.bmod.ImageIndex = 4
        Me.bmod.Name = "bmod"
        Me.bmod.Text = "&Modify"
        Me.bmod.ToolTipText = "Modifies selected calculation"
        '
        'bcopy
        '
        Me.bcopy.ImageIndex = 9
        Me.bcopy.Name = "bcopy"
        Me.bcopy.Text = "&Copy From"
        Me.bcopy.ToolTipText = "Copies calculation(s) from another template"
        '
        'bdel
        '
        Me.bdel.ImageIndex = 3
        Me.bdel.Name = "bdel"
        Me.bdel.Text = "&Delete"
        Me.bdel.ToolTipText = "Deletes selected calculation from template"
        '
        'bcopyinto
        '
        Me.bcopyinto.Enabled = False
        Me.bcopyinto.ImageIndex = 9
        Me.bcopyinto.Name = "bcopyinto"
        Me.bcopyinto.Text = "Copy Into"
        Me.bcopyinto.ToolTipText = "Copies a Calculation into Selected Universes"
        '
        'ilCopyFunctions
        '
        Me.ilCopyFunctions.ImageStream = CType(resources.GetObject("ilCopyFunctions.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ilCopyFunctions.TransparentColor = System.Drawing.Color.Transparent
        Me.ilCopyFunctions.Images.SetKeyName(0, "")
        Me.ilCopyFunctions.Images.SetKeyName(1, "")
        Me.ilCopyFunctions.Images.SetKeyName(2, "")
        Me.ilCopyFunctions.Images.SetKeyName(3, "")
        Me.ilCopyFunctions.Images.SetKeyName(4, "")
        Me.ilCopyFunctions.Images.SetKeyName(5, "")
        Me.ilCopyFunctions.Images.SetKeyName(6, "")
        Me.ilCopyFunctions.Images.SetKeyName(7, "")
        Me.ilCopyFunctions.Images.SetKeyName(8, "")
        Me.ilCopyFunctions.Images.SetKeyName(9, "")
        Me.ilCopyFunctions.Images.SetKeyName(10, "")
        Me.ilCopyFunctions.Images.SetKeyName(11, "")
        Me.ilCopyFunctions.Images.SetKeyName(12, "")
        '
        'ToolBarButton2
        '
        Me.ToolBarButton2.Name = "ToolBarButton2"
        '
        'ToolBarButton3
        '
        Me.ToolBarButton3.Name = "ToolBarButton3"
        '
        'ToolBarButton4
        '
        Me.ToolBarButton4.Name = "ToolBarButton4"
        '
        'ToolBarButton5
        '
        Me.ToolBarButton5.Name = "ToolBarButton5"
        '
        'bc_am_calculations
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.Color.White
        Me.CancelButton = Me.Bok
        Me.ClientSize = New System.Drawing.Size(778, 430)
        Me.ControlBox = False
        Me.Controls.Add(Me.tformula)
        Me.Controls.Add(Me.lvCalculations)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "bc_am_calculations"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Insight Calculations - Blue Curve"
        Me.Panel1.ResumeLayout(False)
        Me.pAggregations.ResumeLayout(False)
        Me.pTemplates.ResumeLayout(False)
        Me.pcopy.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region
    Public obc_calcs As bc_om_calculations
    Public ocopy_calcs As New bc_om_calculations
    'Public from_toolkit_id As Integer = 1
    Public Shared column As Integer
    Public selected_calc As Integer
    Public item_added As Boolean = False

    Private Sub bc_am_calculations_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim bcs As New bc_cs_central_settings(True)
        Me.pcopy.Visible = False
        Me.bmod.Enabled = False
        Me.bdel.Enabled = False
        Me.bsel.Enabled = False
        Me.ball.Enabled = False

        bc_am_calculation_values.fcalc_list = Me
        Dim i As Integer
        REM load templates 
        load_lists()
        If Not bc_am_calculation_values.TemplateId Is Nothing Then
            Me.cbTemplates.Items.Clear()
            For i = 0 To obc_calcs.templates.Count - 1
                Me.cbTemplates.Items.Add(obc_calcs.templates(i).name)
            Next
            cbAggregations.Visible = False
            Me.cbTemplates.Enabled = False
            For i = 0 To obc_calcs.templates.Count - 1
                If obc_calcs.templates(i).id = bc_am_calculation_values.TemplateId Then
                    Me.cbTemplates.SelectedIndex = i
                    Exit For
                End If
            Next
            pAggregations.Visible = False
        ElseIf Not bc_am_calculation_values.AggregationId Is Nothing Then
            Me.lvCalculations.MultiSelect = True
            Me.cbAggregations.Items.Clear()
            For i = 0 To obc_calcs.alAggregations.Count - 1
                Me.cbAggregations.Items.Add(obc_calcs.alAggregations(i))
            Next
            cbTemplates.Visible = False
            Me.cbAggregations.Enabled = False
            For i = 0 To obc_calcs.alAggregations.Count - 1
                If obc_calcs.alAggregations(i).id = bc_am_calculation_values.AggregationId Then
                    Me.cbAggregations.SelectedIndex = i
                    Exit For
                End If
            Next
            pTemplates.Visible = False
            pAggregations.Location = pTemplates.Location
        End If

        If Not bc_am_calculation_values.TemplateId Is Nothing Then
            For i = 0 To obc_calcs.templates.Count - 1
                cbCopy.Items.Add(obc_calcs.templates(i))
            Next
        End If

        If Not bc_am_calculation_values.AggregationId Is Nothing Then
            For i = 0 To obc_calcs.alAggregations.Count - 1
                cbCopy.Items.Add(obc_calcs.alAggregations(i))
            Next
            cbTemplates.Visible = False
        End If

        Me.tformula.Clear()
        Me.lvCalculations.Width = Me.Width - 48
        Me.pcopy.Visible = False

        Me.Cursor = Cursors.Default

    End Sub
    Public Sub load_lists()
        Try
            Me.Cursor = Cursors.WaitCursor
            obc_calcs = bc_am_calculation_values.getCalcs()
            REM loads rows
            bc_am_calculation_values.rows.Clear()
            For i = 0 To obc_calcs.items.Count - 1
                bc_am_calculation_values.rows.Add(obc_calcs.items(i))
            Next
            bc_am_calculation_values.fixed_operands.Clear()
            For i = 0 To obc_calcs.fixed_operands.Count - 1
                bc_am_calculation_values.fixed_operands.Add(obc_calcs.fixed_operands(i))
            Next
            bc_am_calculation_values.calc_types.Clear()
            For i = 0 To obc_calcs.calc_types.Count - 1
                bc_am_calculation_values.calc_types.Add(obc_calcs.calc_types(i))
            Next
            bc_am_calculation_values.agg_rows_in_use.Clear()
            For i = 0 To obc_calcs.aggregation_results.Count - 1
                bc_am_calculation_values.agg_rows_in_use.Add(obc_calcs.aggregation_results(i))
            Next

            bc_am_calculation_values.reserved_Words = obc_calcs.reserved_words
            bc_am_calculation_values.aggregation_types = obc_calcs.aggregation_types





            Me.Cursor = Cursors.Default
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_calculations", "load_lists", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub

    Private Sub cbTemplates_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbTemplates.SelectedIndexChanged, cbAggregations.SelectedIndexChanged

        fillCalculations()
    End Sub

    Private Sub fillCalculations()

        Me.Cursor = Cursors.WaitCursor

        Dim id As Integer
        If Me.cbTemplates.Visible AndAlso Not Me.cbTemplates.SelectedItem Is Nothing Then
            id = obc_calcs.templates(Me.cbTemplates.SelectedIndex).id
            obc_calcs.intTemplateID = id
            bc_am_calculation_values.TemplateId = obc_calcs.intTemplateID
        ElseIf Me.cbAggregations.Visible AndAlso Not Me.cbAggregations.SelectedItem Is Nothing Then
            id = obc_calcs.alAggregations(Me.cbAggregations.SelectedIndex).id
            obc_calcs.intAggregationId = id
            bc_am_calculation_values.AggregationId = obc_calcs.intAggregationId
        Else
            Exit Sub
        End If

        'If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
        '    obc_calcs.db_read()
        'ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
        '    obc_calcs.no_send_back = False
        '    obc_calcs.tmode = bc_cs_soap_base_class.tREAD
        '    obc_calcs.transmit_to_server_and_receive(obc_calcs, True)
        'Else
        '    Exit Sub
        'End If

        populate_calcs()

        Me.Cursor = Cursors.Default

    End Sub

    Public Sub populate_calcs()
        Try
            Dim i As Integer
            Dim n As ListViewItem
            Me.lvCalculations.Items.Clear()
            Me.tformula.Text = ""
            bc_am_calculation_values.calculations.Clear()
            For i = 0 To obc_calcs.calculations.Count - 1
                bc_am_calculation_values.calculations.Add(obc_calcs.calculations(i))
            Next
            For i = 0 To obc_calcs.calculations.Count - 1
                n = New ListViewItem(CStr(obc_calcs.calculations(i).name))
                n.SubItems.Add(CStr(obc_calcs.calculations(i).type))
                n.SubItems.Add(CStr(obc_calcs.calculations(i).display_formula))
                If obc_calcs.calculations(i).calc_when = 0 Then
                    n.SubItems.Add("no")
                Else
                    n.SubItems.Add("yes")
                End If
                n.SubItems.Add(CStr(obc_calcs.calculations(i).ord))
                n.SubItems.Add(CStr(obc_calcs.calculations(i).id))

                If obc_calcs.calculations(i).inactive = True Then
                    n.SubItems.Add("Inactive")
                Else
                    n.SubItems.Add("Active")
                End If

                If obc_calcs.calculations(i).is_extract = True Then
                    n.SubItems.Add("Extract")
                Else
                    n.SubItems.Add("None")
                End If
                If IsNothing(bc_am_calculation_values.TemplateId) Then
                    Me.lvCalculations.Columns(3).Width = 0
                    Me.lvCalculations.Columns(4).Width = 0
                End If

                Me.lvCalculations.Items.Add(n)
                Dim jj As New bc_om_calculation
            Next
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_calculations", "populate_calcs", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Public Sub populate_copy_calcs()
        Dim i, j As Integer
        Dim n As ListViewItem
        Dim found As Boolean
        Dim tcalcs As New bc_om_calculations

        Me.lvCalculationsCopy.Items.Clear()
        tcalcs.calculations.Clear()
        For i = 0 To ocopy_calcs.calculations.Count - 1
            found = False
            REM check calc isnt already in set
            For j = 0 To bc_am_calculation_values.calculations.Count - 1
                If bc_am_calculation_values.calculations(j).result_row_id = ocopy_calcs.calculations(i).result_row_id Then
                    found = True
                    Exit For
                End If
            Next
            If found = False Then
                tcalcs.calculations.Add(ocopy_calcs.calculations(i))
            End If
        Next
        ocopy_calcs.calculations.Clear()
        For i = 0 To tcalcs.calculations.Count - 1
            ocopy_calcs.calculations.Add(tcalcs.calculations(i))
        Next
        For i = 0 To ocopy_calcs.calculations.Count - 1
            n = New ListViewItem(CStr(ocopy_calcs.calculations(i).name))
            Me.lvCalculationsCopy.Items.Add(n)
        Next


    End Sub

    REM FIL 5.2 AUG 2014
    Private Sub ListView1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvCalculations.SelectedIndexChanged
        Dim j As Integer
        Me.tformula.Clear()
        Try

            If Me.lvCalculations.SelectedItems.Count <> 1 Then
                Me.bmod.Enabled = False
                Me.bdel.Enabled = False
                If Me.lvCalculations.SelectedItems.Count = 0 Then
                    Me.bcopyinto.Enabled = False
                End If
                Exit Sub
            End If
            If Me.lvCalculations.SelectedItems.Count = 1 Then
                Me.bmod.Enabled = True
                Me.bdel.Enabled = True
                Me.bsel.Enabled = False
                Me.ball.Enabled = False




                selected_calc = Me.lvCalculations.SelectedItems(0).SubItems(5).Text


                For j = 0 To bc_am_calculation_values.calculations.Count - 1
                    If bc_am_calculation_values.calculations(j).id = selected_calc Then
                        Me.tformula.Text = bc_am_calculation_values.calculations(j).display_formula
                        selected_calc = j
                        Exit For
                    End If
                Next

                Me.lvCalculations.Width = Me.Width - 50
                Me.pcopy.Visible = False
                tbCopyFunctions.Visible = False
            End If
            If Me.pAggregations.Visible = True Then
                Me.bcopyinto.Enabled = True
            End If

        Catch ex As Exception
            MsgBox(ex.Message)

        End Try
    End Sub

    Private Sub delete_calc()
        Me.tformula.Clear()

        Dim omsg As bc_cs_message
        omsg = New bc_cs_message("Blue Curve", "Are you sure you want to remove calculation: " + bc_am_calculation_values.calculations(Me.lvCalculations.SelectedItems(0).Index).name, bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
        If omsg.cancel_selected = True Then
            Exit Sub
        End If

        Dim ocalc As New bc_om_calculation
        ocalc.delete_flag = bc_am_calculation_values.calculations(Me.selected_calc).id
        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
            ocalc.db_write()
        Else
            ocalc.tmode = bc_cs_soap_base_class.tWRITE
            ocalc.transmit_to_server_and_receive(ocalc, True)
        End If
        Me.load_lists()
        Me.populate_calcs()
        Me.bmod.Enabled = False
        Me.bdel.Enabled = False

    End Sub

    'Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Dim ofrmcalc As New bc_am_in_calculation_editor
    '    ofrmcalc.modify = True
    '    ofrmcalc.modify_calc = obc_calcs.calculations(selected_calc)
    '    item_added = False
    '    ofrmcalc.ShowDialog()
    '    item_added = ofrmcalc.item_added
    'End Sub

    'Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Me.ListView2.Items.Clear()
    '    Me.pcopy.Visible = True
    '    Me.ListView1.Width = Me.Width - Me.ListView2.Width - 50
    '    Me.ListView2.Enabled = False

    'End Sub
    Private Sub copy_selected()
        Dim otrace As New bc_cs_activity_log("bc_am_calculations", "Copy selected", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try

           

            Dim calc_count As Integer = 0
            Me.Cursor = Cursors.WaitCursor
            Me.pcopy.Visible = False

            Dim found As Boolean = False
            Dim i As Integer
            Dim onew_calcs As New bc_om_calculations
            Dim calc_exists_text As String
            Dim oc As bc_om_calculation
            Dim agg_id As Long
            REM copy into aggregation universe
            If Me.lvCalculationsCopy.Columns(0).Text = "Universes" Then
                For i = 0 To Me.lvCalculationsCopy.Items.Count - 1
                    If Me.lvCalculationsCopy.Items(i).Checked = True Then
                        For k = 0 To obc_calcs.alAggregations.Count - 1
                            If obc_calcs.alAggregations(k).name = Me.lvCalculationsCopy.Items(i).Text Then
                                agg_id = obc_calcs.alAggregations(k).id
                                Exit For
                            End If
                        Next
                        found = True
                        REM get calcs selected in left hand list
                        For j = 0 To Me.lvCalculations.Items.Count - 1
                            If Me.lvCalculations.Items(j).Selected = True Then
                                oc = New bc_om_calculation
                                oc.id = 0
                                oc.intAggregationId = agg_id
                                oc.op1 = obc_calcs.calculations(j).op1
                                oc.op2 = obc_calcs.calculations(j).op2
                                oc.op3 = obc_calcs.calculations(j).op3
                                oc.op4 = obc_calcs.calculations(j).op4
                                oc.op5 = obc_calcs.calculations(j).op5
                                oc.op6 = obc_calcs.calculations(j).op6
                                oc.op7 = obc_calcs.calculations(j).op7
                                oc.op8 = obc_calcs.calculations(j).op8
                                oc.result_row_id = obc_calcs.calculations(j).result_row_id
                                oc.formula = obc_calcs.calculations(j).formula
                                oc.num_years = obc_calcs.calculations(j).num_years
                                oc.calc_when = obc_calcs.calculations(j).calc_when
                                oc.max_tx = obc_calcs.calculations(j).max_tx
                                oc.min_tx = obc_calcs.calculations(j).min_tx
                                oc.max_val = obc_calcs.calculations(j).max_val
                                oc.min_val = obc_calcs.calculations(j).min_val
                                oc.inactive = obc_calcs.calculations(j).inactive
                                oc.is_extract = obc_calcs.calculations(j).is_extract
                                oc.name = obc_calcs.calculations(j).name
                                oc.ord = obc_calcs.calculations(j).ord
                                oc.type = obc_calcs.calculations(j).type
                                oc.exch_rate_method = obc_calcs.calculations(j).exch_rate_method
                                oc.na_tx = obc_calcs.calculations(j).na_tx
                                REM check calculation is not already in universe
                                onew_calcs.calculations.Add(oc)
                                calc_count = calc_count + 1
                            End If
                        Next
                    End If
                Next
            Else
                For i = 0 To Me.lvCalculationsCopy.Items.Count - 1
                    If Me.lvCalculationsCopy.Items(i).Checked = True Then
                        found = True
                        onew_calcs.calculations.Add(ocopy_calcs.calculations(i))
                        REM flag as new
                        CType(onew_calcs.calculations(calc_count), bc_om_calculation).intTemplateId = bc_am_calculation_values.TemplateId
                        CType(onew_calcs.calculations(calc_count), bc_om_calculation).intAggregationId = bc_am_calculation_values.AggregationId
                        onew_calcs.calculations(calc_count).id = 0
                        calc_count = calc_count + 1
                    End If
                Next

            End If

                    Me.lvCalculations.Width = Me.Width - 50
                    If found = True Then
                        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                            onew_calcs.db_write()
                        Else
                            onew_calcs.tmode = bc_cs_soap_base_class.tWRITE
                            onew_calcs.transmit_to_server_and_receive(onew_calcs, True)
                        End If
                    Else
                        If Me.lvCalculationsCopy.Columns(0).Text = "Universes" Then
                            Dim omsg As New bc_cs_message("Blue Curve", "No Universes Selected!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                        Else
                            Dim omsg As New bc_cs_message("Blue Curve", "No Calculations Selected!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                        End If
                    End If


                    Me.pcopy.Visible = False

                    Me.load_lists()
                    Me.populate_calcs()
                    Me.bsel.Enabled = False
                    Me.ball.Enabled = False

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_calculations", "Copy selected", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.Cursor = Cursors.Default
            otrace = New bc_cs_activity_log("bc_am_calculations", "Copy selected", bc_cs_activity_codes.TRACE_EXIT, "")

        End Try

    End Sub

    Private Sub bcopytemplate_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbCopy.SelectedIndexChanged
        If Me.cbCopy.SelectedIndex = -1 Then
            Exit Sub
        End If
        Me.Cursor = Cursors.WaitCursor
        Dim omsg As bc_cs_message

        ocopy_calcs.intTemplateID = Nothing
        ocopy_calcs.intAggregationId = Nothing

        Dim intSelected As Integer = Me.cbCopy.SelectedItem.id
        If TypeOf Me.cbCopy.SelectedItem Is bc_om_template Then
            ocopy_calcs.intTemplateID = intSelected
            REM check currently selected item isnt selected
            If ocopy_calcs.intTemplateID = obc_calcs.intTemplateID Then
                Me.Cursor = Cursors.Default
                omsg = New bc_cs_message("Blue Curve", "Can't copy template to itself select another item!", bc_cs_message.MESSAGE)
                Exit Sub
            End If
        ElseIf TypeOf Me.cbCopy.SelectedItem Is bc_om_entity Then
            ocopy_calcs.intAggregationId = intSelected
            REM check currently selected item isnt selected
            If ocopy_calcs.intAggregationId = obc_calcs.intAggregationId Then
                Me.Cursor = Cursors.Default
                omsg = New bc_cs_message("Blue Curve", "Can't copy aggregate to itself select another item!", bc_cs_message.MESSAGE)
                Exit Sub
            End If
        End If

        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
            ocopy_calcs.db_read()
        ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
            ocopy_calcs.no_send_back = False
            ocopy_calcs.tmode = bc_cs_soap_base_class.tREAD
            ocopy_calcs.transmit_to_server_and_receive(ocopy_calcs, True)
        Else
            Exit Sub
        End If
        Me.lvCalculationsCopy.Enabled = True
        Me.ball.Enabled = True
        Me.bsel.Enabled = True
        populate_copy_calcs()
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub select_all()
        Dim i As Integer
        For i = 0 To Me.lvCalculationsCopy.Items.Count - 1
            Me.lvCalculationsCopy.Items(i).Checked = True
        Next

    End Sub

    Private Sub ListView1_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles lvCalculations.ColumnClick


        column = CInt(e.Column.ToString)
        lvCalculations.ListViewItemSorter() = New CompareByname
        If CompareByname.toggle = False Then
            CompareByname.toggle = True
        Else
            CompareByname.toggle = False
        End If

    End Sub
    Class CompareByname
        Implements System.Collections.IComparer
        Public Shared toggle As Boolean = False
        Function compare(ByVal x As Object, ByVal y As Object) As Integer _
        Implements System.Collections.IComparer.Compare

            Dim item1 As ListViewItem = CType(x, ListViewItem)
            Dim item2 As ListViewItem = CType(y, ListViewItem)
            If toggle = True Then
                Return String.Compare(item1.SubItems(column).Text, item2.SubItems(column).Text)
            Else
                Return String.Compare(item2.SubItems(column).Text, item1.SubItems(column).Text)
            End If
        End Function
    End Class

    Private Sub PictureBox3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Hide()
    End Sub
    Private Sub reload_calcs()
        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
            obc_calcs.db_read()
        Else
            obc_calcs.tmode = bc_cs_soap_base_class.tREAD
            obc_calcs.transmit_to_server_and_receive(obc_calcs, True)
        End If
        populate_calcs()
        bc_am_calculation_values.rows.Clear()
        For i = 0 To obc_calcs.items.Count - 1
            bc_am_calculation_values.rows.Add(obc_calcs.items(i))
        Next

    End Sub
    REM FIL 5.2 Aug 2014
    Private Sub ToolBar1_ButtonClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles ToolBar1.ButtonClick

        If e.Button.Text = "&New" Then
            Me.pcopy.Visible = False

            Me.lvCalculations.Width = Me.Width - 50
            Dim fcalc As New bc_am_in_calculation_editor
            fcalc.IsNew = True
            Me.tformula.Clear()
            item_added = False
            fcalc.ShowDialog()
            Me.bmod.Enabled = False
            Me.bdel.Enabled = False
            Me.lvCalculations.SelectedItems.Clear()
            If fcalc.changed = True Then
                reload_calcs()
            End If
            tbCopyFunctions.Visible = False
        ElseIf e.Button.Text = "&Modify" Then
            Me.pcopy.Visible = False

            Me.lvCalculations.Width = Me.Width - 50
            Dim ofrmcalc As New bc_am_in_calculation_editor
            'ofrmcalc.modify = True
            ofrmcalc.IsNew = False
            ofrmcalc.modify_calc = obc_calcs.calculations(selected_calc)
            item_added = False
            ofrmcalc.ShowDialog()
            If ofrmcalc.changed = True Then
                reload_calcs()
            End If
            tbCopyFunctions.Visible = False
        ElseIf e.Button.Text = "&Copy From" Then
            Me.cbCopy.SelectedIndex = -1
            Me.Label1.Text = "Copy From:"
            Me.lvCalculationsCopy.Columns(0).Text = "Calculations"
            Me.cbCopy.Visible = True
            Me.lvCalculationsCopy.Items.Clear()
            Me.pcopy.Visible = True

            Me.lvCalculations.Width = Me.Width - Me.lvCalculationsCopy.Width - 70
            Me.lvCalculationsCopy.Enabled = False
            tbCopyFunctions.Visible = True
            Me.lvCalculationsCopy.Enabled = False
            Me.bsel.ToolTipText = "Copy Selected Selected Calculations"
        ElseIf e.Button.Text = "Copy Into" Then
            Me.cbCopy.SelectedIndex = -1
            Me.cbCopy.Visible = False
            Me.Label1.Text = "Copy Into:"
            Me.lvCalculationsCopy.Items.Clear()
            Me.pcopy.Visible = True
            Me.lvCalculations.Width = Me.Width - Me.lvCalculationsCopy.Width - 70
            Me.lvCalculationsCopy.Columns(0).Text = "Universes"
            Me.lvCalculationsCopy.Height = Me.lvCalculations.Height
            Me.lvCalculationsCopy.Enabled = False
            tbCopyFunctions.Visible = True
            Me.lvCalculationsCopy.Items.Clear()
            Dim n As ListViewItem

            For i = 0 To obc_calcs.alAggregations.Count - 1
                n = New ListViewItem(CStr(obc_calcs.alAggregations(i).name))
                If obc_calcs.alAggregations(i).name <> Me.cbAggregations.Text Then
                    Me.lvCalculationsCopy.Items.Add(n)
                    Me.lvCalculationsCopy.Enabled = True
                    Me.ball.Enabled = True
                    Me.bsel.ToolTipText = "Copy Calculation(s) into Selected Universe(s)"
                    Me.bsel.Enabled = True
                End If

            Next
        ElseIf e.Button.Text = "&Delete" Then
            Me.lvCalculations.Width = Me.Width - 50
            delete_calc()
        End If

    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub Bok_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Bok.Click
        Me.Hide()
    End Sub

    Private Sub tbCopyFunctions_ButtonClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles tbCopyFunctions.ButtonClick
        If e.Button.Text = "Copy &Checked" Then
            Me.pcopy.Visible = False

            Me.lvCalculations.Width = Me.Width - 50
            copy_selected()
        ElseIf e.Button.Text = "Select &All" Then
            select_all()
        End If
    End Sub

    Private Sub Panel1_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs)

    End Sub

    Private Sub Panel1_Paint_1(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Panel1.Paint

    End Sub

    Private Sub bc_am_calculations_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize

        ' stop resizing when a certain size is reached
        If Me.Width < 794 Then
            Me.Width = 794
            Exit Sub
        End If

        If Me.Height < 468 Then
            Me.Height = 468
            Exit Sub
        End If

    End Sub
End Class
Public Class bc_am_calculation_values

    Public Shared rows As New List(Of bc_om_calculations.bc_om_calc_item)
    Public Shared fixed_operands As New List(Of bc_om_calculations.bc_om_calc_item)

    Public Shared attributes As New ArrayList
    Public Shared calculations As New List(Of bc_om_calculation)

    Public Shared calc_types As New ArrayList
    Public Shared agg_rows_in_use As New ArrayList

    Private Shared intTemplateId, intAggregationId As Nullable(Of Long)
    Public Shared aggtarget_class_name As String
    Public Shared reserved_Words As New ArrayList
    Public Shared aggregation_types As New ArrayList


    Public Shared Property TemplateId() As Nullable(Of Long)
        Get
            Return intTemplateId
        End Get
        Set(ByVal value As Nullable(Of Long))
            intTemplateId = value
            intAggregationId = Nothing
        End Set
    End Property
    Public Shared Property AggregationId() As Nullable(Of Long)
        Get
            Return intAggregationId
        End Get
        Set(ByVal value As Nullable(Of Long))
            intAggregationId = value
            intTemplateId = Nothing
        End Set
    End Property
    Public Shared fcalc_list As bc_am_calculations

    Public Shared Function getCalcs() As bc_om_calculations
        Dim obc_calcs As bc_om_calculations = New bc_om_calculations
        obc_calcs.intTemplateID = TemplateId
        obc_calcs.intAggregationId = AggregationId
        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
            obc_calcs.db_read()
        ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
            obc_calcs.no_send_back = False
            obc_calcs.tmode = bc_cs_soap_base_class.tREAD
            obc_calcs.transmit_to_server_and_receive(obc_calcs, True)
        Else
            Return Nothing
        End If
        Return obc_calcs
    End Function

End Class

