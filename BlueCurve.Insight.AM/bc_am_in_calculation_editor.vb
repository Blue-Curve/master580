Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports System.Collections
Imports System.Threading
Imports System.Windows.Forms.Screen
Imports System.IO
Imports Microsoft.Win32
Imports System.Windows.Forms
#Region "changes"
'Changes:
'Tracker                 Initials                   Date                      Synopsis
'FIL 8426                PR                         8/1/2014                  Enable Growths
#End Region
Public Class bc_am_in_calculation_editor
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
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents pperiod As System.Windows.Forms.Panel
    Friend WithEvents rbPeriodExist As System.Windows.Forms.RadioButton
    Friend WithEvents lresp As System.Windows.Forms.Label
    Friend WithEvents tresp As System.Windows.Forms.TextBox
    Friend WithEvents rbPeriodNew As System.Windows.Forms.RadioButton
    Friend WithEvents cresp As System.Windows.Forms.ComboBox
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents rbResultExist As System.Windows.Forms.RadioButton
    Friend WithEvents tresult As System.Windows.Forms.TextBox
    Friend WithEvents lres1 As System.Windows.Forms.Label
    Friend WithEvents rbResultNew As System.Windows.Forms.RadioButton
    Friend WithEvents lstresult As System.Windows.Forms.ComboBox
    Friend WithEvents poperands As System.Windows.Forms.Panel
    Friend WithEvents clib As System.Windows.Forms.ListBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents RadioButton5 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton3 As System.Windows.Forms.RadioButton
    Friend WithEvents lstoperandfixed As System.Windows.Forms.ListBox
    Friend WithEvents lstoperand As System.Windows.Forms.ListBox
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents ccalctype As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents bcancel As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents btnVerifyCalc As System.Windows.Forms.Button
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Chkextract As System.Windows.Forms.CheckBox
    Friend WithEvents chkinactive As System.Windows.Forms.CheckBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents maxtxt As System.Windows.Forms.TextBox
    Friend WithEvents mintxt As System.Windows.Forms.TextBox
    Friend WithEvents tmaxval As System.Windows.Forms.TextBox
    Friend WithEvents tminval As System.Windows.Forms.TextBox
    Friend WithEvents Chkmax As System.Windows.Forms.CheckBox
    Friend WithEvents Chkmin As System.Windows.Forms.CheckBox
    Friend WithEvents uxDoubleClick As System.Windows.Forms.Label
    Friend WithEvents uxFormulaPanel As System.Windows.Forms.Panel
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txformula As System.Windows.Forms.RichTextBox
    Friend WithEvents uxNATxt As System.Windows.Forms.Label
    Friend WithEvents uxNAText As System.Windows.Forms.TextBox
    Friend WithEvents Chkps As System.Windows.Forms.CheckBox
    Friend WithEvents ccurrp1 As System.Windows.Forms.ComboBox
    Friend WithEvents ccurr As System.Windows.Forms.ComboBox
    Friend WithEvents cweight As System.Windows.Forms.ComboBox
    Friend WithEvents lweight As System.Windows.Forms.Label
    Friend WithEvents cav As System.Windows.Forms.CheckBox
    Friend WithEvents ccal As System.Windows.Forms.CheckBox
    Friend WithEvents tsd As System.Windows.Forms.TextBox
    Friend WithEvents lsd As System.Windows.Forms.Label
    Friend WithEvents Bc_am_calc_search1 As BlueCurve.Insight.AM.bc_am_calc_search
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_in_calculation_editor))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.rbPeriodExist = New System.Windows.Forms.RadioButton()
        Me.rbPeriodNew = New System.Windows.Forms.RadioButton()
        Me.rbResultExist = New System.Windows.Forms.RadioButton()
        Me.rbResultNew = New System.Windows.Forms.RadioButton()
        Me.clib = New System.Windows.Forms.ListBox()
        Me.RadioButton5 = New System.Windows.Forms.RadioButton()
        Me.RadioButton3 = New System.Windows.Forms.RadioButton()
        Me.Chkps = New System.Windows.Forms.CheckBox()
        Me.pperiod = New System.Windows.Forms.Panel()
        Me.ccurrp1 = New System.Windows.Forms.ComboBox()
        Me.lresp = New System.Windows.Forms.Label()
        Me.tresp = New System.Windows.Forms.TextBox()
        Me.cresp = New System.Windows.Forms.ComboBox()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.ccurr = New System.Windows.Forms.ComboBox()
        Me.tresult = New System.Windows.Forms.TextBox()
        Me.lres1 = New System.Windows.Forms.Label()
        Me.lstresult = New System.Windows.Forms.ComboBox()
        Me.poperands = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lstoperand = New System.Windows.Forms.ListBox()
        Me.lstoperandfixed = New System.Windows.Forms.ListBox()
        Me.uxDoubleClick = New System.Windows.Forms.Label()
        Me.Bc_am_calc_search1 = New BlueCurve.Insight.AM.bc_am_calc_search()
        Me.cweight = New System.Windows.Forms.ComboBox()
        Me.lweight = New System.Windows.Forms.Label()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.ccalctype = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnOk = New System.Windows.Forms.Button()
        Me.bcancel = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.uxFormulaPanel = New System.Windows.Forms.Panel()
        Me.cav = New System.Windows.Forms.CheckBox()
        Me.ccal = New System.Windows.Forms.CheckBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txformula = New System.Windows.Forms.RichTextBox()
        Me.btnVerifyCalc = New System.Windows.Forms.Button()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.tsd = New System.Windows.Forms.TextBox()
        Me.lsd = New System.Windows.Forms.Label()
        Me.uxNATxt = New System.Windows.Forms.Label()
        Me.uxNAText = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.maxtxt = New System.Windows.Forms.TextBox()
        Me.mintxt = New System.Windows.Forms.TextBox()
        Me.tmaxval = New System.Windows.Forms.TextBox()
        Me.tminval = New System.Windows.Forms.TextBox()
        Me.Chkmax = New System.Windows.Forms.CheckBox()
        Me.Chkmin = New System.Windows.Forms.CheckBox()
        Me.Chkextract = New System.Windows.Forms.CheckBox()
        Me.chkinactive = New System.Windows.Forms.CheckBox()
        Me.pperiod.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.poperands.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.uxFormulaPanel.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'rbPeriodExist
        '
        Me.rbPeriodExist.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rbPeriodExist.BackColor = System.Drawing.Color.Transparent
        Me.rbPeriodExist.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbPeriodExist.Location = New System.Drawing.Point(384, 3)
        Me.rbPeriodExist.Name = "rbPeriodExist"
        Me.rbPeriodExist.Size = New System.Drawing.Size(57, 18)
        Me.rbPeriodExist.TabIndex = 4
        Me.rbPeriodExist.Text = "Exist"
        Me.ToolTip1.SetToolTip(Me.rbPeriodExist, "Existing Item for result")
        Me.rbPeriodExist.UseVisualStyleBackColor = False
        '
        'rbPeriodNew
        '
        Me.rbPeriodNew.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rbPeriodNew.BackColor = System.Drawing.Color.Transparent
        Me.rbPeriodNew.Checked = True
        Me.rbPeriodNew.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbPeriodNew.Location = New System.Drawing.Point(384, 21)
        Me.rbPeriodNew.Name = "rbPeriodNew"
        Me.rbPeriodNew.Size = New System.Drawing.Size(54, 18)
        Me.rbPeriodNew.TabIndex = 5
        Me.rbPeriodNew.TabStop = True
        Me.rbPeriodNew.Text = "New"
        Me.ToolTip1.SetToolTip(Me.rbPeriodNew, "New result item")
        Me.rbPeriodNew.UseVisualStyleBackColor = False
        '
        'rbResultExist
        '
        Me.rbResultExist.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rbResultExist.BackColor = System.Drawing.Color.Transparent
        Me.rbResultExist.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbResultExist.Location = New System.Drawing.Point(384, 2)
        Me.rbResultExist.Name = "rbResultExist"
        Me.rbResultExist.Size = New System.Drawing.Size(54, 18)
        Me.rbResultExist.TabIndex = 4
        Me.rbResultExist.Text = "Exist"
        Me.ToolTip1.SetToolTip(Me.rbResultExist, "Existing Item for result")
        Me.rbResultExist.UseVisualStyleBackColor = False
        '
        'rbResultNew
        '
        Me.rbResultNew.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rbResultNew.BackColor = System.Drawing.Color.Transparent
        Me.rbResultNew.Checked = True
        Me.rbResultNew.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbResultNew.Location = New System.Drawing.Point(384, 20)
        Me.rbResultNew.Name = "rbResultNew"
        Me.rbResultNew.Size = New System.Drawing.Size(52, 19)
        Me.rbResultNew.TabIndex = 5
        Me.rbResultNew.TabStop = True
        Me.rbResultNew.Text = "New"
        Me.ToolTip1.SetToolTip(Me.rbResultNew, "New result item")
        Me.rbResultNew.UseVisualStyleBackColor = False
        '
        'clib
        '
        Me.clib.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.clib.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.clib.Location = New System.Drawing.Point(3, 43)
        Me.clib.Name = "clib"
        Me.clib.Size = New System.Drawing.Size(143, 121)
        Me.clib.TabIndex = 6
        Me.ToolTip1.SetToolTip(Me.clib, "Prefix the operand")
        '
        'RadioButton5
        '
        Me.RadioButton5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioButton5.Location = New System.Drawing.Point(224, 6)
        Me.RadioButton5.Name = "RadioButton5"
        Me.RadioButton5.Size = New System.Drawing.Size(55, 16)
        Me.RadioButton5.TabIndex = 3
        Me.RadioButton5.Text = "Fixed"
        Me.ToolTip1.SetToolTip(Me.RadioButton5, "System Defined Operand")
        '
        'RadioButton3
        '
        Me.RadioButton3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioButton3.Location = New System.Drawing.Point(154, 2)
        Me.RadioButton3.Name = "RadioButton3"
        Me.RadioButton3.Size = New System.Drawing.Size(64, 23)
        Me.RadioButton3.TabIndex = 2
        Me.RadioButton3.Text = "Config"
        Me.ToolTip1.SetToolTip(Me.RadioButton3, "Configured Operand")
        '
        'Chkps
        '
        Me.Chkps.Checked = True
        Me.Chkps.CheckState = System.Windows.Forms.CheckState.Checked
        Me.Chkps.Enabled = False
        Me.Chkps.Location = New System.Drawing.Point(9, 59)
        Me.Chkps.Name = "Chkps"
        Me.Chkps.Size = New System.Drawing.Size(143, 18)
        Me.Chkps.TabIndex = 2
        Me.Chkps.Text = "Submission Only"
        Me.ToolTip1.SetToolTip(Me.Chkps, "check if result item monetary")
        '
        'pperiod
        '
        Me.pperiod.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pperiod.BackColor = System.Drawing.Color.Transparent
        Me.pperiod.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pperiod.Controls.Add(Me.ccurrp1)
        Me.pperiod.Controls.Add(Me.rbPeriodExist)
        Me.pperiod.Controls.Add(Me.lresp)
        Me.pperiod.Controls.Add(Me.tresp)
        Me.pperiod.Controls.Add(Me.rbPeriodNew)
        Me.pperiod.Controls.Add(Me.cresp)
        Me.pperiod.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pperiod.Location = New System.Drawing.Point(10, 110)
        Me.pperiod.Name = "pperiod"
        Me.pperiod.Size = New System.Drawing.Size(560, 40)
        Me.pperiod.TabIndex = 2
        '
        'ccurrp1
        '
        Me.ccurrp1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ccurrp1.FormattingEnabled = True
        Me.ccurrp1.Location = New System.Drawing.Point(441, 11)
        Me.ccurrp1.Name = "ccurrp1"
        Me.ccurrp1.Size = New System.Drawing.Size(111, 21)
        Me.ccurrp1.TabIndex = 6
        '
        'lresp
        '
        Me.lresp.BackColor = System.Drawing.Color.Transparent
        Me.lresp.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lresp.Location = New System.Drawing.Point(1, 11)
        Me.lresp.Name = "lresp"
        Me.lresp.Size = New System.Drawing.Size(118, 24)
        Me.lresp.TabIndex = 0
        Me.lresp.Text = "Period Result Item:"
        '
        'tresp
        '
        Me.tresp.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tresp.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tresp.Location = New System.Drawing.Point(124, 8)
        Me.tresp.Name = "tresp"
        Me.tresp.Size = New System.Drawing.Size(222, 21)
        Me.tresp.TabIndex = 1
        '
        'cresp
        '
        Me.cresp.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cresp.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cresp.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cresp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cresp.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cresp.ItemHeight = 13
        Me.cresp.Location = New System.Drawing.Point(124, 8)
        Me.cresp.Name = "cresp"
        Me.cresp.Size = New System.Drawing.Size(245, 21)
        Me.cresp.TabIndex = 2
        '
        'Panel3
        '
        Me.Panel3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel3.BackColor = System.Drawing.Color.Transparent
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel3.Controls.Add(Me.ccurr)
        Me.Panel3.Controls.Add(Me.rbResultExist)
        Me.Panel3.Controls.Add(Me.tresult)
        Me.Panel3.Controls.Add(Me.lres1)
        Me.Panel3.Controls.Add(Me.rbResultNew)
        Me.Panel3.Controls.Add(Me.lstresult)
        Me.Panel3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Panel3.Location = New System.Drawing.Point(10, 60)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(560, 44)
        Me.Panel3.TabIndex = 1
        '
        'ccurr
        '
        Me.ccurr.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ccurr.FormattingEnabled = True
        Me.ccurr.Location = New System.Drawing.Point(441, 7)
        Me.ccurr.Name = "ccurr"
        Me.ccurr.Size = New System.Drawing.Size(111, 21)
        Me.ccurr.TabIndex = 7
        '
        'tresult
        '
        Me.tresult.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tresult.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tresult.Location = New System.Drawing.Point(124, 8)
        Me.tresult.Name = "tresult"
        Me.tresult.Size = New System.Drawing.Size(222, 21)
        Me.tresult.TabIndex = 1
        '
        'lres1
        '
        Me.lres1.BackColor = System.Drawing.Color.Transparent
        Me.lres1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lres1.Location = New System.Drawing.Point(1, 12)
        Me.lres1.Name = "lres1"
        Me.lres1.Size = New System.Drawing.Size(83, 16)
        Me.lres1.TabIndex = 0
        Me.lres1.Text = "Result Item:"
        '
        'lstresult
        '
        Me.lstresult.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstresult.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.lstresult.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.lstresult.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.lstresult.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstresult.ItemHeight = 13
        Me.lstresult.Location = New System.Drawing.Point(124, 8)
        Me.lstresult.Name = "lstresult"
        Me.lstresult.Size = New System.Drawing.Size(245, 21)
        Me.lstresult.TabIndex = 0
        Me.lstresult.Tag = "sizable"
        '
        'poperands
        '
        Me.poperands.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.poperands.BackColor = System.Drawing.Color.Transparent
        Me.poperands.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.poperands.Controls.Add(Me.RadioButton5)
        Me.poperands.Controls.Add(Me.clib)
        Me.poperands.Controls.Add(Me.Label1)
        Me.poperands.Controls.Add(Me.RadioButton3)
        Me.poperands.Controls.Add(Me.lstoperand)
        Me.poperands.Controls.Add(Me.lstoperandfixed)
        Me.poperands.Controls.Add(Me.uxDoubleClick)
        Me.poperands.Controls.Add(Me.Bc_am_calc_search1)
        Me.poperands.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.poperands.Location = New System.Drawing.Point(10, 324)
        Me.poperands.Name = "poperands"
        Me.poperands.Size = New System.Drawing.Size(560, 204)
        Me.poperands.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(0, 27)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(145, 22)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Library:"
        '
        'lstoperand
        '
        Me.lstoperand.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstoperand.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstoperand.HorizontalScrollbar = True
        Me.lstoperand.Location = New System.Drawing.Point(152, 43)
        Me.lstoperand.Name = "lstoperand"
        Me.lstoperand.Size = New System.Drawing.Size(403, 121)
        Me.lstoperand.TabIndex = 23
        '
        'lstoperandfixed
        '
        Me.lstoperandfixed.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstoperandfixed.Location = New System.Drawing.Point(152, 56)
        Me.lstoperandfixed.Name = "lstoperandfixed"
        Me.lstoperandfixed.Size = New System.Drawing.Size(313, 95)
        Me.lstoperandfixed.TabIndex = 40
        '
        'uxDoubleClick
        '
        Me.uxDoubleClick.AutoSize = True
        Me.uxDoubleClick.Location = New System.Drawing.Point(151, 27)
        Me.uxDoubleClick.Name = "uxDoubleClick"
        Me.uxDoubleClick.Size = New System.Drawing.Size(234, 13)
        Me.uxDoubleClick.TabIndex = 8
        Me.uxDoubleClick.Text = "Double Click Item To Add To Formula..."
        '
        'Bc_am_calc_search1
        '
        Me.Bc_am_calc_search1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Bc_am_calc_search1.Location = New System.Drawing.Point(141, 171)
        Me.Bc_am_calc_search1.Margin = New System.Windows.Forms.Padding(4)
        Me.Bc_am_calc_search1.Name = "Bc_am_calc_search1"
        Me.Bc_am_calc_search1.Size = New System.Drawing.Size(414, 28)
        Me.Bc_am_calc_search1.TabIndex = 41
        '
        'cweight
        '
        Me.cweight.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cweight.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cweight.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cweight.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cweight.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cweight.ItemHeight = 13
        Me.cweight.Location = New System.Drawing.Point(307, 129)
        Me.cweight.Name = "cweight"
        Me.cweight.Size = New System.Drawing.Size(245, 21)
        Me.cweight.TabIndex = 7
        Me.cweight.Visible = False
        '
        'lweight
        '
        Me.lweight.AutoSize = True
        Me.lweight.Location = New System.Drawing.Point(151, 129)
        Me.lweight.Name = "lweight"
        Me.lweight.Size = New System.Drawing.Size(134, 13)
        Me.lweight.TabIndex = 42
        Me.lweight.Text = "Weight Operands With"
        Me.lweight.Visible = False
        '
        'Panel4
        '
        Me.Panel4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel4.BackColor = System.Drawing.Color.Transparent
        Me.Panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel4.Controls.Add(Me.ccalctype)
        Me.Panel4.Controls.Add(Me.Label2)
        Me.Panel4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Panel4.Location = New System.Drawing.Point(10, 10)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(560, 44)
        Me.Panel4.TabIndex = 0
        '
        'ccalctype
        '
        Me.ccalctype.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ccalctype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ccalctype.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ccalctype.ItemHeight = 13
        Me.ccalctype.Location = New System.Drawing.Point(124, 8)
        Me.ccalctype.Name = "ccalctype"
        Me.ccalctype.Size = New System.Drawing.Size(428, 21)
        Me.ccalctype.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(1, 10)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(118, 18)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Calculation Type:"
        '
        'btnOk
        '
        Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOk.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.btnOk.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOk.Location = New System.Drawing.Point(415, 629)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(75, 25)
        Me.btnOk.TabIndex = 5
        Me.btnOk.Text = "OK"
        Me.btnOk.UseVisualStyleBackColor = False
        '
        'bcancel
        '
        Me.bcancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bcancel.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.bcancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.bcancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bcancel.Location = New System.Drawing.Point(496, 629)
        Me.bcancel.Name = "bcancel"
        Me.bcancel.Size = New System.Drawing.Size(75, 25)
        Me.bcancel.TabIndex = 6
        Me.bcancel.Text = "Cancel"
        Me.bcancel.UseVisualStyleBackColor = False
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.uxFormulaPanel)
        Me.Panel1.Controls.Add(Me.Panel2)
        Me.Panel1.Controls.Add(Me.pperiod)
        Me.Panel1.Controls.Add(Me.Panel3)
        Me.Panel1.Controls.Add(Me.poperands)
        Me.Panel1.Controls.Add(Me.Panel4)
        Me.Panel1.Controls.Add(Me.btnOk)
        Me.Panel1.Controls.Add(Me.bcancel)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(580, 665)
        Me.Panel1.TabIndex = 0
        '
        'uxFormulaPanel
        '
        Me.uxFormulaPanel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxFormulaPanel.BackColor = System.Drawing.Color.Transparent
        Me.uxFormulaPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.uxFormulaPanel.Controls.Add(Me.cav)
        Me.uxFormulaPanel.Controls.Add(Me.ccal)
        Me.uxFormulaPanel.Controls.Add(Me.cweight)
        Me.uxFormulaPanel.Controls.Add(Me.Label6)
        Me.uxFormulaPanel.Controls.Add(Me.lweight)
        Me.uxFormulaPanel.Controls.Add(Me.txformula)
        Me.uxFormulaPanel.Controls.Add(Me.btnVerifyCalc)
        Me.uxFormulaPanel.Location = New System.Drawing.Point(10, 156)
        Me.uxFormulaPanel.Name = "uxFormulaPanel"
        Me.uxFormulaPanel.Size = New System.Drawing.Size(560, 162)
        Me.uxFormulaPanel.TabIndex = 2
        '
        'cav
        '
        Me.cav.AutoSize = True
        Me.cav.Location = New System.Drawing.Point(243, 8)
        Me.cav.Name = "cav"
        Me.cav.Size = New System.Drawing.Size(169, 17)
        Me.cav.TabIndex = 44
        Me.cav.Text = "Av Exch Rate For Actuals"
        Me.cav.UseVisualStyleBackColor = True
        Me.cav.Visible = False
        '
        'ccal
        '
        Me.ccal.AutoSize = True
        Me.ccal.Location = New System.Drawing.Point(66, 9)
        Me.ccal.Name = "ccal"
        Me.ccal.Size = New System.Drawing.Size(171, 17)
        Me.ccal.TabIndex = 43
        Me.ccal.Text = "Calenderize All Operands"
        Me.ccal.UseVisualStyleBackColor = True
        Me.ccal.Visible = False
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(2, 10)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(58, 13)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = "Formula:"
        '
        'txformula
        '
        Me.txformula.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txformula.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.txformula.EnableAutoDragDrop = True
        Me.txformula.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txformula.Location = New System.Drawing.Point(3, 31)
        Me.txformula.MaxLength = 10000
        Me.txformula.Name = "txformula"
        Me.txformula.Size = New System.Drawing.Size(552, 95)
        Me.txformula.TabIndex = 1
        Me.txformula.Text = ""
        '
        'btnVerifyCalc
        '
        Me.btnVerifyCalc.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnVerifyCalc.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.btnVerifyCalc.Location = New System.Drawing.Point(481, 3)
        Me.btnVerifyCalc.Name = "btnVerifyCalc"
        Me.btnVerifyCalc.Size = New System.Drawing.Size(75, 25)
        Me.btnVerifyCalc.TabIndex = 2
        Me.btnVerifyCalc.Text = "Verify"
        Me.btnVerifyCalc.UseVisualStyleBackColor = False
        '
        'Panel2
        '
        Me.Panel2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel2.BackColor = System.Drawing.Color.Transparent
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.tsd)
        Me.Panel2.Controls.Add(Me.lsd)
        Me.Panel2.Controls.Add(Me.Chkps)
        Me.Panel2.Controls.Add(Me.uxNATxt)
        Me.Panel2.Controls.Add(Me.uxNAText)
        Me.Panel2.Controls.Add(Me.Label5)
        Me.Panel2.Controls.Add(Me.Label4)
        Me.Panel2.Controls.Add(Me.maxtxt)
        Me.Panel2.Controls.Add(Me.mintxt)
        Me.Panel2.Controls.Add(Me.tmaxval)
        Me.Panel2.Controls.Add(Me.tminval)
        Me.Panel2.Controls.Add(Me.Chkmax)
        Me.Panel2.Controls.Add(Me.Chkmin)
        Me.Panel2.Controls.Add(Me.Chkextract)
        Me.Panel2.Controls.Add(Me.chkinactive)
        Me.Panel2.Enabled = False
        Me.Panel2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Panel2.Location = New System.Drawing.Point(10, 537)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(560, 86)
        Me.Panel2.TabIndex = 4
        '
        'tsd
        '
        Me.tsd.Location = New System.Drawing.Point(224, 58)
        Me.tsd.Name = "tsd"
        Me.tsd.Size = New System.Drawing.Size(107, 21)
        Me.tsd.TabIndex = 13
        Me.tsd.Visible = False
        '
        'lsd
        '
        Me.lsd.BackColor = System.Drawing.Color.Transparent
        Me.lsd.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lsd.Location = New System.Drawing.Point(139, 53)
        Me.lsd.Name = "lsd"
        Me.lsd.Size = New System.Drawing.Size(85, 31)
        Me.lsd.TabIndex = 14
        Me.lsd.Text = "Standard Deviation"
        Me.lsd.Visible = False
        '
        'uxNATxt
        '
        Me.uxNATxt.BackColor = System.Drawing.Color.Transparent
        Me.uxNATxt.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxNATxt.Location = New System.Drawing.Point(339, 61)
        Me.uxNATxt.Name = "uxNATxt"
        Me.uxNATxt.Size = New System.Drawing.Size(62, 18)
        Me.uxNATxt.TabIndex = 9
        Me.uxNATxt.Text = "N/A Text:"
        '
        'uxNAText
        '
        Me.uxNAText.Location = New System.Drawing.Point(407, 58)
        Me.uxNAText.Name = "uxNAText"
        Me.uxNAText.Size = New System.Drawing.Size(148, 21)
        Me.uxNAText.TabIndex = 12
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Enabled = False
        Me.Label5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(364, 36)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(37, 16)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "Text:"
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Enabled = False
        Me.Label4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(364, 9)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(37, 16)
        Me.Label4.TabIndex = 7
        Me.Label4.Text = "Text:"
        '
        'maxtxt
        '
        Me.maxtxt.Enabled = False
        Me.maxtxt.Location = New System.Drawing.Point(407, 32)
        Me.maxtxt.Name = "maxtxt"
        Me.maxtxt.Size = New System.Drawing.Size(148, 21)
        Me.maxtxt.TabIndex = 11
        '
        'mintxt
        '
        Me.mintxt.Enabled = False
        Me.mintxt.Location = New System.Drawing.Point(407, 6)
        Me.mintxt.Name = "mintxt"
        Me.mintxt.Size = New System.Drawing.Size(148, 21)
        Me.mintxt.TabIndex = 10
        '
        'tmaxval
        '
        Me.tmaxval.Enabled = False
        Me.tmaxval.Location = New System.Drawing.Point(224, 31)
        Me.tmaxval.Name = "tmaxval"
        Me.tmaxval.Size = New System.Drawing.Size(107, 21)
        Me.tmaxval.TabIndex = 6
        '
        'tminval
        '
        Me.tminval.Enabled = False
        Me.tminval.Location = New System.Drawing.Point(224, 5)
        Me.tminval.Name = "tminval"
        Me.tminval.Size = New System.Drawing.Size(107, 21)
        Me.tminval.TabIndex = 5
        '
        'Chkmax
        '
        Me.Chkmax.Location = New System.Drawing.Point(139, 33)
        Me.Chkmax.Name = "Chkmax"
        Me.Chkmax.Size = New System.Drawing.Size(96, 21)
        Me.Chkmax.TabIndex = 4
        Me.Chkmax.Text = "Max Value:"
        '
        'Chkmin
        '
        Me.Chkmin.Location = New System.Drawing.Point(139, 7)
        Me.Chkmin.Name = "Chkmin"
        Me.Chkmin.Size = New System.Drawing.Size(96, 22)
        Me.Chkmin.TabIndex = 3
        Me.Chkmin.Text = "Min Value:"
        '
        'Chkextract
        '
        Me.Chkextract.Checked = True
        Me.Chkextract.CheckState = System.Windows.Forms.CheckState.Checked
        Me.Chkextract.Location = New System.Drawing.Point(9, 34)
        Me.Chkextract.Name = "Chkextract"
        Me.Chkextract.Size = New System.Drawing.Size(116, 18)
        Me.Chkextract.TabIndex = 1
        Me.Chkextract.Text = "Extract Visible"
        '
        'chkinactive
        '
        Me.chkinactive.Location = New System.Drawing.Point(9, 9)
        Me.chkinactive.Name = "chkinactive"
        Me.chkinactive.Size = New System.Drawing.Size(79, 18)
        Me.chkinactive.TabIndex = 0
        Me.chkinactive.Text = "Inactive"
        '
        'bc_am_in_calculation_editor
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.Color.White
        Me.CancelButton = Me.bcancel
        Me.ClientSize = New System.Drawing.Size(580, 665)
        Me.ControlBox = False
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "bc_am_in_calculation_editor"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Calculation Editor - Blue Curve"
        Me.pperiod.ResumeLayout(False)
        Me.pperiod.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.poperands.ResumeLayout(False)
        Me.poperands.PerformLayout()
        Me.Panel4.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.uxFormulaPanel.ResumeLayout(False)
        Me.uxFormulaPanel.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public operands As New ArrayList
    Public fixed_operands As New ArrayList
    Public operands_ids As New ArrayList

    Public period_calc As Boolean = False

    Public changed As Boolean = False
    Public modify_calc As New bc_om_calculation
    Private boolIsNew As Boolean = False

    Public Property IsNew() As Boolean
        Get
            Return boolIsNew
        End Get
        Set(ByVal value As Boolean)
            boolIsNew = value
        End Set
    End Property

    Private Sub check_ok()
        Me.btnOk.Enabled = False
        Me.btnVerifyCalc.Enabled = False
        If (Me.tsd.Visible = False Or (Me.tsd.Visible = True And Me.tsd.Text <> "")) And Me.ccalctype.SelectedIndex <> -1 And (Me.tresult.Text <> "" Or Me.lstresult.SelectedIndex <> -1) And Me.txformula.Text <> "" And (Me.Chkmin.Checked = False Or (Me.Chkmin.Checked = True And IsNumeric(Me.tminval.Text) = True)) And (Me.Chkmax.Checked = False Or (Me.Chkmax.Checked = True And IsNumeric(Me.tmaxval.Text) = True)) And (Me.pperiod.Enabled = False Or (Me.pperiod.Enabled = True And (Me.tresp.Text <> "" Or Me.cresp.SelectedIndex <> -1))) Then
            Me.btnOk.Enabled = True
            Me.btnVerifyCalc.Enabled = True
        End If
    End Sub

    Private Sub bc_am_in_calculation_editor_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Dim i As Integer
            Me.ccalctype.Items.Clear()

            If IsNew = True Then
                Me.Panel3.Enabled = False
                Me.txformula.Enabled = False
                Me.btnOk.Enabled = False
                Me.btnVerifyCalc.Enabled = False

                Me.clib.Items.Clear()
         
            End If

            Me.ccurr.Items.Add("Non Monetary")
            Me.ccurr.Items.Add("Reporting")
            Me.ccurr.Items.Add("Target")
            Me.ccurr.Items.Add("Reporting Denom")
            Me.ccurr.Items.Add("Target Denom")
            Me.ccurrp1.Items.Add("Non Monetary")
            Me.ccurrp1.Items.Add("Reporting")
            Me.ccurrp1.Items.Add("Target")
            Me.ccurrp1.Items.Add("Reporting Denom")
            Me.ccurrp1.Items.Add("Target Denom")


            Me.ccurr.SelectedIndex = 1
            Me.ccurrp1.SelectedIndex = 1



            If bc_am_calculation_values.AggregationId > 0 Then
                REM certain controls arnt needed for aggregations
                Me.clib.Enabled = True
                Me.Label1.Enabled = True
                Me.RadioButton3.Visible = False
                Me.RadioButton5.Visible = False
                Me.cweight.Items.Add("None")
                Me.cweight.SelectedIndex = 0
                If bc_am_calculation_values.link_attributes.attributes.Count > 0 Then
                    'Me.lweight.Visible = True
                    'Me.cweight.Visible = True
                    Me.cweight.Items.Add(bc_am_calculation_values.link_attributes.attributes(i).attribute_name)
                End If
                Me.cweight.SelectedIndex = 0

                Me.Label1.Text = "Apply To Formula"
                For i = 0 To bc_am_calculation_values.calc_types.Count - 1
                    If bc_am_calculation_values.calc_types(i).aggregate = True Then
                        Me.ccalctype.Items.Add(bc_am_calculation_values.calc_types(i).name)
                    End If
                Next
            Else
                Me.Chkps.Enabled = True
                For i = 0 To bc_am_calculation_values.calc_types.Count - 1
                    If bc_am_calculation_values.calc_types(i).aggregate = False Then
                        Me.ccalctype.Items.Add(bc_am_calculation_values.calc_types(i).name)
                    End If
                Next
            End If
            Me.clear_period_result()
            Me.cresp.Enabled = True
            Me.rbPeriodExist.Visible = True
            Me.rbPeriodNew.Visible = True

            updateFormStatus()

            If Not IsNew Then
                Me.cresp.Enabled = True 'ALLRED
                Me.rbPeriodExist.Visible = False
                Me.rbPeriodNew.Visible = False
                Me.rbResultNew.Visible = True
                Me.rbResultExist.Visible = True
                Me.rbResultNew.Checked = False

                Me.ccalctype.SelectedIndex = -1
                Me.ccalctype.Text = modify_calc.type
               
                Me.rbResultExist.Checked = True
                Me.Panel3.Enabled = False
                Me.pperiod.Enabled = False

                Me.rbResultNew.Visible = False
                Me.rbResultExist.Visible = False

                Me.lstresult.Visible = False
                Me.tresult.Visible = True
                Me.cresp.Visible = False
                Me.tresp.Visible = True
                Me.Chkps.Checked = True
                If modify_calc.calc_when = 1 Then
                    Me.Chkps.Checked = False
                End If


                For i = 0 To bc_am_calculation_values.rows.Count - 1
                    If bc_am_calculation_values.rows(i).item_id = modify_calc.result_row_id Then
                        Me.tresult.Text = bc_am_calculation_values.rows(i).name
                        Me.Panel3.Enabled = True
                        Me.tresult.Enabled = False
                        Me.ccurr.Enabled = True
                        Me.ccurr.SelectedIndex = bc_am_calculation_values.rows(i).curr_type

                        Exit For
                    End If
                Next

                For i = 0 To bc_am_calculation_values.rows.Count - 1
                    If bc_am_calculation_values.rows(i).item_id = modify_calc.num_years Then
                        Me.ccurrp1.Enabled = True
                        Me.pperiod.Enabled = True
                        Me.tresp.Enabled = False
                        Me.tresp.Text = bc_am_calculation_values.rows(i).name
                        Me.ccurrp1.SelectedIndex = bc_am_calculation_values.rows(i).curr_type
                        Exit For
                    End If
                Next


                Me.uxNAText.Text = modify_calc.na_tx
                If Not IsNothing(modify_calc.min_val) Then
                    Me.Chkmin.Checked = True
                    Me.tminval.Text = CStr(modify_calc.min_val)
                    Me.mintxt.Text = CStr(modify_calc.min_tx)
                End If
                If Not IsNothing(modify_calc.max_val) Then
                    Me.Chkmax.Checked = True
                    Me.tmaxval.Text = CStr(modify_calc.max_val)
                    Me.maxtxt.Text = CStr(modify_calc.max_tx)
                End If
                If modify_calc.inactive = True Then
                    Me.chkinactive.Checked = True
                End If


                If modify_calc.is_extract = False Then
                    Me.Chkextract.Checked = False
                End If

                If modify_calc.exch_rate_method = 1 Then
                    Me.cav.Checked = True
                End If
                If modify_calc.cal_operands = True Then
                    Me.ccal.Checked = True
                End If

                Me.tsd.Text = modify_calc.standard_deviation
            End If

            Me.Cursor = Cursors.WaitCursor


            Me.lstoperand.Enabled = True
            Me.lstoperandfixed.Enabled = True
            Me.RadioButton3.Checked = True

            Me.Cursor = Cursors.Default
            If Not IsNew Then
                Me.btnOk.Text = "Update"
                Me.txformula.Text = modify_calc.display_formula
                Me.ccalctype.Enabled = False 'ALLRED
                Try
                   
                    'If bc_am_calculation_values.AggregationId > 0 And modify_calc.num_years = 0 And modify_calc.contributor1_id > 0 And modify_calc.contributor2_id > 0 Then
                    '    For i = 0 To bc_am_in_context.insight_items.libray_calcs.libray_calculations.Count - 1
                    '         If bc_am_in_context.insight_items.libray_calcs.libray_calculations(i).contributor1_id = modify_calc.contributor1_id And bc_am_in_context.insight_items.libray_calcs.libray_calculations(i).contributor2_id = modify_calc.contributor2_id Then
                    '            Me.clib.SelectedIndex = i + 1
                    '            Exit For
                    '        End If
                    '    Next
                    'End If
                    If LCase(modify_calc.type) = "static to period" AndAlso modify_calc.contributor2_id > 0 Then
                        For i = 0 To bc_am_in_context.insight_items.libray_calcs.libray_calculations.Count - 1
                            If LCase(bc_am_in_context.insight_items.libray_calcs.libray_calculations(i).type) = "static to period" And bc_am_in_context.insight_items.libray_calcs.libray_calculations(i).contributor2_id = modify_calc.contributor2_id Then
                                Me.clib.SelectedIndex = get_lib_display_index(bc_am_in_context.insight_items.libray_calcs.libray_calculations(i).name)
                                Exit For
                            End If
                        Next
                    Else

                        If modify_calc.num_years = 0 And modify_calc.contributor1_id > 0 And modify_calc.contributor2_id > 0 Then
                            For i = 0 To bc_am_in_context.insight_items.libray_calcs.libray_calculations.Count - 1
                                If bc_am_in_context.insight_items.libray_calcs.libray_calculations(i).contributor1_id = modify_calc.contributor1_id And bc_am_in_context.insight_items.libray_calcs.libray_calculations(i).contributor2_id = modify_calc.contributor2_id Then
                                    Me.clib.SelectedIndex = get_lib_display_index(bc_am_in_context.insight_items.libray_calcs.libray_calculations(i).name)
                                    Exit For
                                End If
                            Next
                        End If

                        If modify_calc.num_years = 0 And modify_calc.interval_type > 0 And modify_calc.interval > 0 Then
                            For i = 0 To bc_am_in_context.insight_items.libray_calcs.libray_calculations.Count - 1
                                If bc_am_in_context.insight_items.libray_calcs.libray_calculations(i).interval_type = modify_calc.interval_type And bc_am_in_context.insight_items.libray_calcs.libray_calculations(i).interval = modify_calc.interval Then
                                    Me.clib.SelectedIndex = get_lib_display_index(bc_am_in_context.insight_items.libray_calcs.libray_calculations(i).name)
                                    Exit For
                                End If
                            Next
                        End If

                        If modify_calc.num_years <> 0 Then
                            For i = 0 To bc_am_in_context.insight_items.libray_calcs.libray_calculations.Count - 1
                                If bc_am_in_context.insight_items.libray_calcs.libray_calculations(i).num_years = modify_calc.num_years Then
                                    Me.clib.SelectedIndex = get_lib_display_index(bc_am_in_context.insight_items.libray_calcs.libray_calculations(i).name)
                                    Exit For
                                End If
                            Next
                        End If

                        If Me.cweight.Visible = True Then
                            For i = 0 To bc_am_calculation_values.link_attributes.attributes.Count - 1
                                If bc_am_calculation_values.link_attributes.attributes(i).attribute_id = modify_calc.weighting_attribute Then
                                    If Me.cweight.Items.Count >= i + 1 Then
                                        Me.cweight.SelectedIndex = i + 1
                                        Exit For
                                    End If
                                End If
                            Next


                        End If
                    End If
                Catch

                End Try
            Else
                ccalctype.Enabled = True
            End If
            Me.txformula.Text = modify_calc.display_formula
            Me.btnOk.Enabled = False
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_in_calculation_editor", "load", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Private Function get_lib_display_index(name As String)
        For j = 0 To Me.clib.Items.Count - 1
            If Me.clib.Items(j) = name Then
                get_lib_display_index = j
                Exit Function
            End If
        Next
        get_lib_display_index = 0
    End Function

    Dim gstatic_flag As Boolean

    Private Sub load_result_rows(Optional ByVal static_flag As Boolean = False)
        Bc_am_calc_search1.complete_list.Clear()
        gstatic_flag = static_flag


        REM load all rows that are not already calculated in template
        Dim i, j As Integer
        Dim found As Boolean
        Me.lstoperand.Items.Clear()
        Me.cresp.Items.Clear()
        Me.lstoperandfixed.Items.Clear()
        Me.lstresult.Items.Clear()
        REM cant have a result row that is already a result riw
        For i = 0 To bc_am_calculation_values.rows.Count - 1
            If LTrim(RTrim(bc_am_calculation_values.rows(i).name)) <> "" Then
                found = False

                For j = 0 To bc_am_calculation_values.calculations.Count - 1

                    If (Me.ccalctype.Text = bc_am_calculation_values.calculations(j).type And bc_am_calculation_values.calculations(j).result_row_id = (bc_am_calculation_values.rows(i).item_id)) Or ((bc_am_calculation_values.calculations(j).num_years = (bc_am_calculation_values.rows(i).item_id)) And (LCase(bc_am_calculation_values.calculations(j).type) = "static to period" Or bc_am_calculation_values.calculations(j).type = "static to period cumulative")) Then
                        found = True
                        REM Exit For
                    End If

                Next
                REM If found = False Then
                If bc_am_calculation_values.rows(i).static_flag = static_flag Then
                    If found = False Then
                        Me.lstresult.Items.Add(bc_am_calculation_values.rows(i).name)
                    End If
                    Me.lstoperand.Items.Add(bc_am_calculation_values.rows(i).name)
                    Bc_am_calc_search1.complete_list.Add(bc_am_calculation_values.rows(i).name)
                End If
                If bc_am_calculation_values.rows(i).static_flag = 0 Then
                    If found = False Then
                        Me.cresp.Items.Add(bc_am_calculation_values.rows(i).name)
                    End If
                End If
                REM End If
            End If
        Next
        Me.lstoperandfixed.Items.Clear()
        For i = 0 To bc_am_calculation_values.fixed_operands.Count - 1
            Me.lstoperandfixed.Items.Add(bc_am_calculation_values.fixed_operands(i).name)
        Next
    End Sub
    Private Sub handler_operand_search(ByVal results As ArrayList) Handles Bc_am_calc_search1.results_ready
        Dim static_flag As Boolean = True
        For i = 0 To bc_am_calculation_values.calc_types.Count - 1
            If bc_am_calculation_values.calc_types(i).name = Me.ccalctype.Text Then
                If bc_am_calculation_values.calc_types(i).type = 1 Then
                    static_flag = False
                    Exit For
                End If
            End If
        Next

        Me.lstoperand.Items.Clear()
        For i = 0 To bc_am_calculation_values.rows.Count - 1
            If LTrim(RTrim(bc_am_calculation_values.rows(i).name)) <> "" Then

                If bc_am_calculation_values.rows(i).static_flag = static_flag Then
                    For j = 0 To results.Count - 1
                        If results(j) = bc_am_calculation_values.rows(i).name Then
                            Me.lstoperand.Items.Add(bc_am_calculation_values.rows(i).name)
                            Exit For
                        End If
                    Next
                End If
            End If
        Next

    End Sub
    Private Sub handler_result_search(ByVal results As ArrayList)
        Dim static_flag As Boolean = True
        For i = 0 To bc_am_calculation_values.calc_types.Count - 1
            If bc_am_calculation_values.calc_types(i).name = Me.ccalctype.Text Then
                If bc_am_calculation_values.calc_types(i).type = 1 Then
                    static_flag = False
                    Exit For
                End If
            End If
        Next

        Me.lstresult.Items.Clear()

        For i = 0 To bc_am_calculation_values.rows.Count - 1
            If LTrim(RTrim(bc_am_calculation_values.rows(i).name)) <> "" Then

                If bc_am_calculation_values.rows(i).static_flag = static_flag Then
                    For j = 0 To results.Count - 1
                        If results(j) = bc_am_calculation_values.rows(i).name Then
                            Me.lstresult.Items.Add(bc_am_calculation_values.rows(i).name)
                            Exit For
                        End If
                    Next
                End If
            End If
        Next
        Me.btnOk.Enabled = False

    End Sub
    Private Sub write_out_formula(ByVal tx As String)
        Dim stx As String
        stx = Me.txformula.Text + tx
        'Me.lformula.Add(stx)
        Me.txformula.Text = stx
        Me.Chkps.Checked = True
        Me.Chkps.Enabled = True
        If InStr(stx, "current_price") > 0 Or InStr(stx, "price_denominator") > 0 Then
            Me.Chkps.Checked = False
            Me.Chkps.Enabled = False
        End If

    End Sub

    Private Sub lstoperand_SelectedIndexChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstoperand.DoubleClick ' lstoperand.SelectedIndexChanged
        If lstoperand.SelectedIndex = -1 Then
            Exit Sub
        End If
        If Me.clib.SelectedIndex > 0 And LCase(Me.ccalctype.Text) = "static to period" Then
            write_out_formula(Me.lstoperand.SelectedItem)
        ElseIf clib.SelectedIndex > 0 And (IsNothing(bc_am_calculation_values.AggregationId) Or bc_am_calculation_values.AggregationId <= 0) Then
            Me.txformula.Clear()
            lib_formula(Me.get_Lib_calc_index)
        Else
            write_out_formula(Me.lstoperand.SelectedItem)
        End If

    End Sub
    Private Sub enable_value()
        Me.lstoperand.Visible = False
        Me.lstoperandfixed.Visible = False
    End Sub

    Private Sub lib_formula(ByVal idx As Long)
        Dim op As String
        Dim str As String

        op = Me.lstoperand.SelectedItem

        str = bc_am_in_context.insight_items.libray_calcs.libray_calculations(idx).formula
        str = str.Replace("value_1", op)
        str = str.Replace("value_2", op)
        write_out_formula(str)

    End Sub

    Private Function check_price_sensitive() As Boolean
        check_price_sensitive = 0
        REM loop through fixed operand and for prices sensitive ones check
        REM if in formula
        Dim i As Integer
        Dim tx As String
        For i = 0 To bc_am_calculation_values.fixed_operands.Count - 1
            If bc_am_calculation_values.fixed_operands(i).price_sensitive = 1 Then
                tx = Me.txformula.Text
                If InStr(tx, bc_am_calculation_values.fixed_operands(i).name) > 0 Then
                    check_price_sensitive = 1
                    Exit For
                End If
            End If
        Next
    End Function
    REM in built library calculations
    Private Sub library_calc(ByRef ocalc As bc_om_calculation)
        Dim idx As Integer
        idx = get_Lib_calc_index()

        ocalc.calc_when = bc_am_in_context.insight_items.libray_calcs.libray_calculations(idx).calc_when
        ocalc.num_years = bc_am_in_context.insight_items.libray_calcs.libray_calculations(idx).num_years
        ocalc.formula = bc_am_in_context.insight_items.libray_calcs.libray_calculations(idx).formula
        REM FIL vs ibes 
        ocalc.contributor1_id = bc_am_in_context.insight_items.libray_calcs.libray_calculations(idx).contributor1_id
        ocalc.contributor2_id = bc_am_in_context.insight_items.libray_calcs.libray_calculations(idx).contributor2_id


        REM Mometun
        ocalc.interval_type = bc_am_in_context.insight_items.libray_calcs.libray_calculations(idx).interval_type
        ocalc.interval = bc_am_in_context.insight_items.libray_calcs.libray_calculations(idx).interval


        Dim co As Integer = 0
        For i = 0 To bc_am_calculation_values.rows.Count - 1
            If bc_am_calculation_values.rows(i).static_flag = False And Me.lstoperand.Text = bc_am_calculation_values.rows(i).name Then
                ocalc.op1 = bc_am_calculation_values.rows(i).item_id
                ocalc.op2 = bc_am_calculation_values.rows(i).item_id
                Exit For
            End If
        Next

    End Sub

    Private Sub lstresult_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstresult.SelectedIndexChanged


        If Me.lstresult.SelectedIndex > -1 Then
            For i = 0 To bc_am_calculation_values.rows.Count - 1
                If bc_am_calculation_values.rows(i).static_flag = gstatic_flag And bc_am_calculation_values.rows(i).name = Me.lstresult.Text Then

                    Me.ccurr.SelectedIndex = bc_am_calculation_values.rows(i).curr_type
                End If

            Next

         End If
        check_ok()
        'End If
    End Sub

    Dim strNonBreaks As String = "abcdefghijklmnopqrstuvwxyz_"
    Shared Function Compare1(ByVal a As KeyValuePair(Of Integer, bc_om_calculations.bc_om_calc_item), ByVal b As KeyValuePair(Of Integer, bc_om_calculations.bc_om_calc_item))
        Return a.Key.CompareTo(b.Key)
    End Function
    Private Function parseOperands() As String
        Me.operands_ids.Clear()
        Dim strInput As String = txformula.Text + " "
        Dim static_flag As Boolean = True
        Dim item_list_name As New ArrayList
        Dim item_list_id As New ArrayList
        Dim len_item_list_name As New ArrayList
        Dim len_item_list_id As New ArrayList
        Dim lengths As New ArrayList
        Dim ml As Integer = 0

        REM temp remove reserved words
        strInput = Replace(strInput, "current_price", "_reserved_word1")
        strInput = Replace(strInput, "price_denominator", "_reserved_word2")
        strInput = Replace(strInput, "trading_denominator", "_reserved_word3")
        strInput = Replace(strInput, "currency_denominator", "_reserved_word4")




        For i = 0 To bc_am_calculation_values.calc_types.Count - 1
            If bc_am_calculation_values.calc_types(i).name = Me.ccalctype.Text Then
                If bc_am_calculation_values.calc_types(i).type = 1 Then
                    static_flag = False
                End If
                Exit For
            End If
        Next


        REM get maximum length or item  
        For i = 0 To bc_am_calculation_values.rows.Count - 1
            If bc_am_calculation_values.rows(i).static_flag = static_flag And LTrim(RTrim(bc_am_calculation_values.rows(i).name)) <> "" Then
                item_list_name.Add(bc_am_calculation_values.rows(i).name)
                item_list_id.Add(bc_am_calculation_values.rows(i).item_id)
                If Len(bc_am_calculation_values.rows(i).name) > ml Then
                    ml = Len(bc_am_calculation_values.rows(i).name)
                End If
            End If
        Next
        REM sort the list max length down to stop part of the word being found
        For i = 0 To ml
            For j = 0 To item_list_name.Count - 1
                If Len(item_list_name(j)) = ml Then
                    len_item_list_name.Add(item_list_name(j))
                    len_item_list_id.Add(item_list_id(j))
                End If
            Next
            ml = ml - 1
        Next

        For i = 0 To len_item_list_name.Count - 1
            If InStr(LCase(strInput), LCase(len_item_list_name(i))) > 0 Then
                Me.operands_ids.Add(len_item_list_id(i))
                strInput = Replace(LCase(strInput), "(" + LCase(len_item_list_name(i)) + ")", "( value_" + CStr(Me.operands_ids.Count) + " )")

                strInput = Replace(LCase(strInput), "(" + LCase(len_item_list_name(i)), "( value_" + CStr(Me.operands_ids.Count))

                strInput = Replace(LCase(strInput), "( " + LCase(len_item_list_name(i)), "( value_" + CStr(Me.operands_ids.Count))


                strInput = Replace(LCase(strInput), LCase(len_item_list_name(i)) + ")", " value_" + CStr(Me.operands_ids.Count) + " )")


                strInput = Replace(LCase(strInput), " " + LCase(len_item_list_name(i)) + " ", " value_" + CStr(Me.operands_ids.Count) + " ")
                strInput = Replace(LCase(" " + strInput), " " + LCase(len_item_list_name(i)) + " ", " value_" + CStr(Me.operands_ids.Count) + " ")

                strInput = Replace(LCase(strInput), LCase(len_item_list_name(i)), " value_" + CStr(Me.operands_ids.Count) + " ")
            End If
        Next
        REM FIL MAY 2013
        strInput = Replace(strInput, "_reserved_word1", "current_price")
        strInput = Replace(strInput, "_reserved_word2", "price_denominator")
        strInput = Replace(strInput, "_reserved_word3", "trading_denominator")
        strInput = Replace(strInput, "_reserved_word4", "currency_denominator")



        parseOperands = strInput
    End Function



    'Private Function parseOperandsOld() As String

    '    Try

    '        Me.operands_ids.Clear()
    '        Dim strInput As String = txformula.Text + " "
    '        Dim strOutput As String = ""

    '        Dim aRows As bc_om_calculations.bc_om_calc_item() = bc_am_calculation_values.rows.ToArray
    '        Dim l As New List(Of KeyValuePair(Of Integer, bc_om_calculations.bc_om_calc_item))
    '        For Each boci As bc_om_calculations.bc_om_calc_item In aRows
    '            If Trim(boci.name) <> "" Then
    '                Dim strTemp As String = strInput
    '                Dim intIndex As Integer = strTemp.IndexOf(boci.name)
    '                Dim intRemoved As Integer = 0
    '                While intIndex > -1
    '                    Dim c As Char = strInput.ToLower.ToCharArray()(intIndex + boci.name.Length)
    '                    If strNonBreaks.IndexOf(c) = -1 Then
    '                        Me.operands_ids.Add(boci.item_id)
    '                        l.Add(New KeyValuePair(Of Integer, bc_om_calculations.bc_om_calc_item)(intIndex + intRemoved, boci))
    '                    End If
    '                    intRemoved += intIndex + boci.name.Length
    '                    strTemp = strTemp.Substring(intIndex + boci.name.Length)
    '                    intIndex = strTemp.IndexOf(boci.name)
    '                End While
    '            End If
    '        Next
    '        l.Sort(AddressOf Compare1)
    '        Dim intKeyIndex As Integer = 1
    '        Dim intLast As Integer = 0
    '        For Each kvp As KeyValuePair(Of Integer, bc_om_calculations.bc_om_calc_item) In l
    '            If strInput.Length > kvp.Key Then
    '                strOutput = strOutput & strInput.Substring(intLast, kvp.Key - intLast)
    '                strOutput = strOutput & "value_" & intKeyIndex
    '            End If
    '            intLast = kvp.Value.name.Length + kvp.Key
    '            intKeyIndex += 1
    '        Next
    '        strOutput = strOutput + strInput.Substring(intLast)
    '        Return strOutput
    '    Catch ex As Exception
    '        Dim db_err As New bc_cs_error_log("bc_am_in_calculation_editor", "save", bc_cs_error_codes.USER_DEFINED, ex.Message)

    '        Return ""
    '    End Try

    'End Function
    Private Sub txformula_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txformula.TextChanged
        Me.Chkps.Enabled = True
        Dim stx As String
        stx = Me.txformula.Text
        If InStr(stx, "current_price") > 0 Or InStr(stx, "price_denominator") > 0 Then
            Me.Chkps.Checked = False
            Me.Chkps.Enabled = False
        End If
        Me.check_ok()

    End Sub

    Private Function isCalculationValid(ByVal strCalculation As String) As Boolean
        For Each strOperand As String In Me.lstoperand.Items
            strCalculation = Replace(strCalculation, strOperand, 0)
        Next
        Try
            Dim d As New DataTable()
            d.Compute(strCalculation, Nothing)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Sub updateFormStatus()
        If Not IsNew Then
            Me.tresult.Visible = False
            Me.lstresult.Visible = True
        Else
            Me.tresult.Visible = True
            Me.lstresult.Visible = False
        End If
        Me.lstresult.SelectedIndex = -1
        Me.tresult.Text = ""
    End Sub

    Private Sub rbResultNew_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbResultNew.CheckedChanged

        If Me.rbResultNew.Checked Then
            IsNew = True
            updateFormStatus()
            
        End If
        check_ok()
    End Sub

    Private Sub rbResultExist_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbResultExist.CheckedChanged
        If Me.rbResultExist.Checked Then
            IsNew = False
            updateFormStatus()
            'Me.btnOk.Enabled = True 'ALLRED
        End If
        check_ok()
    End Sub

    Private Sub tresult_MouseLeave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tresult.MouseLeave
        'REM check if row is not already a row
        'If IsNew And Not tresult.Text = "" Then
        '    Me.check_ok()
        '    Dim i As Integer
        '    For i = 0 To bc_am_calculation_values.calculations.Count - 1
        '        If bc_am_calculation_values.calculations(i).name = Me.tresult.Text Then
        '            Dim omsg As New bc_cs_message("Blue Curve", "Item: " + Me.tresult.Text + " already exists or is already in use as a  calculation result!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
        '            Me.tresult.Text = ""
        '            Me.tresult.Select()
        '            Me.check_ok()
        '        End If
        '    Next
        'End If
    End Sub

    Private Sub RadioButton3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton3.CheckedChanged
        If Me.RadioButton3.Checked = True Then
            Me.RadioButton5.Checked = False
            Me.lstoperand.Visible = True
            Me.lstoperandfixed.Visible = False
            Me.Bc_am_calc_search1.Enabled = True
        End If
    End Sub
    Private Sub RadioButton5_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton5.CheckedChanged
        If Me.RadioButton5.Checked = True Then
            Me.RadioButton3.Checked = False
            Me.lstoperand.Visible = False
            Me.lstoperandfixed.Visible = True
            Me.Bc_am_calc_search1.Enabled = False
        End If
    End Sub


    Private Sub clear_period_result()
        Me.lres1.Text = "Result Item:"
        Me.pperiod.Enabled = False

        Me.tresp.Text = ""
        Me.cresp.SelectedIndex = -1

    End Sub
    Private Sub set_period_result()
        Me.lres1.Text = "Static Result Item:"
        Me.pperiod.Enabled = True
        Me.rbPeriodNew.Checked = True
        Me.rbPeriodExist.Checked = True

    End Sub
    Private Sub Panel1_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Panel1.Paint

    End Sub

    Private Sub clib_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles clib.SelectedIndexChanged
        If Me.clib.SelectedIndex = -1 Then
            Exit Sub
        End If

        Me.lstoperand.SelectedIndex = -1
        If Me.clib.SelectedIndex > 0 And LCase(Me.ccalctype.Text) = "static to period" Then
            Me.txformula.Enabled = True
        ElseIf Me.clib.SelectedIndex > 0 And (IsNothing(bc_am_calculation_values.AggregationId) Or bc_am_calculation_values.AggregationId <= 0) Then
            Me.txformula.Text = ""
            Me.txformula.Enabled = False
            Me.RadioButton5.Enabled = False
            Me.RadioButton3.Checked = True
        Else
            'Me.RadioButton5.Enabled = True
            Me.txformula.Enabled = True
        End If
        check_ok()
    End Sub
    Dim llib_list As New ArrayList


    Private Sub ccalctype_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ccalctype.SelectedIndexChanged
        clear_period_result()
        Me.ccal.Visible = False
        Me.cav.Visible = False
        Me.ccal.Checked = False
        Me.cav.Checked = False

        If Me.ccalctype.SelectedIndex > -1 Then
            Me.Panel2.Enabled = True
            Me.Panel3.Enabled = True
            'ALLRED Me.bclearall.Enabled = True
            'If Me.tresult.Text <> "" Then
            '    set_operand_panel()
            'End If
        End If
        Me.lstresult.Items.Clear()

        For i = 0 To bc_am_calculation_values.calc_types.Count - 1
            If bc_am_calculation_values.calc_types(i).name = Me.ccalctype.Text Then
                If bc_am_calculation_values.calc_types(i).type = 1 Then
                    load_rows()
                    period_calc = True
                    If bc_am_calculation_values.TemplateId > 0 Then
                        Me.ccal.Visible = True
                        Me.cav.Visible = True
                    End If
                Else
                    load_attributes()
                    period_calc = False
                    If bc_am_calculation_values.calc_types(i).type = 2 Then
                        set_period_result()
                    End If
                End If
                Exit For
            End If
            Me.tresult.Clear()

            Me.txformula.Clear()
            Me.txformula.Enabled = True


        Next


        Me.clib.SelectedIndex = 0
        Me.tsd.Text = ""
        Me.lsd.Visible = False
        Me.tsd.Visible = False
        If Me.ccalctype.Text = "aggregate style" Or Me.ccalctype.Text = "aggregate style use mean" Then
            Me.lsd.Visible = True
            Me.tsd.Visible = True
        End If
        

        check_ok()


    End Sub
    Private Function get_Lib_calc_index()
        For i = 0 To bc_am_in_context.insight_items.libray_calcs.libray_calculations.Count - 1
            If bc_am_in_context.insight_items.libray_calcs.libray_calculations(i).name = Me.clib.Text Then
                get_Lib_calc_index = i
                Exit Function
            End If
        Next
        get_Lib_calc_index = 0
    End Function
    Private Sub load_rows()
        Dim i As Integer
        REM available libraries


        bc_am_in_context.insight_items.libray_calcs.libray_calculations.Clear()
        For i = 0 To bc_am_in_context.lib_list.Count - 1
            bc_am_in_context.insight_items.libray_calcs.libray_calculations.Add(bc_am_in_context.lib_list(i))
        Next


        Dim tn As New TreeNode("Custom")
        Me.clib.Items.Clear()
        If bc_am_calculation_values.AggregationId > 0 Then

            Me.clib.Items.Add("Nothing")
            For i = 0 To bc_am_in_context.insight_items.libray_calcs.libray_calculations.Count - 1
                If (LCase(bc_am_in_context.insight_items.libray_calcs.libray_calculations(i).type) <> "static to period") Then
                    Me.clib.Items.Add(bc_am_in_context.insight_items.libray_calcs.libray_calculations(i).name)
                    Dim tnParent As New TreeNode(bc_am_in_context.insight_items.libray_calcs.libray_calculations(i).name)
                End If
            Next
        Else
            Me.clib.Items.Add("Custom")
            For i = 0 To bc_am_in_context.insight_items.libray_calcs.libray_calculations.Count - 1
                If bc_am_in_context.insight_items.libray_calcs.libray_calculations(i).agg_only = False Then
                    If (LCase(bc_am_in_context.insight_items.libray_calcs.libray_calculations(i).type) <> "static to period") Then
                        Me.clib.Items.Add(bc_am_in_context.insight_items.libray_calcs.libray_calculations(i).name)
                        Dim tnParent As New TreeNode(bc_am_in_context.insight_items.libray_calcs.libray_calculations(i).name)
                    End If
                End If
            Next
            For i = 0 To bc_am_in_context.insight_items.libray_calcs.libray_calculations.Count - 1
                If bc_am_in_context.insight_items.libray_calcs.libray_calculations(i).agg_only = True Then
                    bc_am_in_context.insight_items.libray_calcs.libray_calculations.RemoveAt(i)
                    Exit For
                End If
            Next
        End If
        REM rows that are not already calculations
        load_result_rows(False)
    End Sub

    Private Sub load_attributes()
        REM available libraries
        Me.clib.Items.Clear()
        Me.clib.Items.Add("Custom")
        For i = 0 To bc_am_in_context.insight_items.libray_calcs.libray_calculations.Count - 1
            If LCase(Me.ccalctype.Text) = "static to period" AndAlso LCase(bc_am_in_context.insight_items.libray_calcs.libray_calculations(i).type) = "static to period" Then
                Me.clib.Items.Add(bc_am_in_context.insight_items.libray_calcs.libray_calculations(i).name)
                Dim tnParent As New TreeNode(bc_am_in_context.insight_items.libray_calcs.libray_calculations(i).name)
            End If
        Next

        REM attributes that are not already calculations
        load_result_rows(True)

    End Sub

    Private Sub tresult_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tresult.TextChanged
        If Not IsNew Then
            Exit Sub
        End If



        'Dim i As Integer
        'Me.txformula.Enabled = False

        'If Me.tresult.Text <> "" Then
        '    'ALLRED Me.Pparen.Enabled = True

        '    For i = 0 To Me.lstresult.Items.Count - 1
        '        If UCase(Me.lstresult.Items(i)) = UCase(Me.tresult.Text) Then
        '            Dim omessage As New bc_cs_message("Blue Curve", "Item: " + Me.tresult.Text + " already exists please select from exist option!", bc_cs_message.MESSAGE)
        '            Me.tresult.Text = ""
        '            Me.tresult.Focus()
        '            Exit Sub
        '        End If
        '    Next

        'End If

        Me.txformula.Enabled = True
        check_ok()


    End Sub

    Private Sub rpexist_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbPeriodExist.CheckedChanged
        If Me.rbPeriodExist.Checked = True Then
            Me.cresp.Visible = True
            Me.tresp.Visible = False
            Me.tresp.Text = ""
        End If
        check_ok()
    End Sub

    Private Sub rpnew_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbPeriodNew.CheckedChanged

        If Me.rbPeriodNew.Checked = True Then
            Me.cresp.Visible = False
            Me.cresp.SelectedIndex = -1
            Me.tresp.Visible = True
           
        End If
        check_ok()
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Dim slog = New bc_cs_activity_log("bc_am_in_calculation_editor", "save", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            REM check result item is valid
            Dim i As Integer
            If Me.tsd.Visible = True And IsNumeric(Me.tsd.Text) = False Then
                Dim omessage As New bc_cs_message("Blue Curve", "Standard Deviation must be numeric.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Sub
            End If

            'Me.txformula.Enabled = False
            If IsNew And Me.tresult.Text <> "" And Me.tresult.Enabled = True Then
                'ALLRED Me.Pparen.Enabled = True
                For i = 0 To Me.lstresult.Items.Count - 1
                    If UCase(Me.lstresult.Items(i)) = UCase(Me.tresult.Text) Then
                        Dim omessage As New bc_cs_message("Blue Curve", "Item: " + Me.tresult.Text + " already exists please select from exist option!", bc_cs_message.MESSAGE)
                        Me.tresult.Text = ""
                        Me.tresult.Focus()
                        Me.check_ok()
                        Exit Sub
                    End If
                Next
            End If
            REM check if row is not already a row
            If IsNew And Not tresult.Text = "" Then
                Me.check_ok()
                For i = 0 To bc_am_calculation_values.calculations.Count - 1
                    If UCase(bc_am_calculation_values.calculations(i).name) = UCase(Me.tresult.Text) And ccalctype.Text = bc_am_calculation_values.calculations(i).type Then
                        Dim omsg As New bc_cs_message("Blue Curve", "Item: " + Me.tresult.Text + " is already in use as a  calculation result!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                        Me.tresult.Text = ""
                        Me.tresult.Select()
                        Me.check_ok()
                        Exit Sub
                    End If
                Next
            End If

            REM static to period
            If Me.pperiod.Enabled = True And IsNew Then
                If Me.tresp.Text <> "" Then
                    'ALLRED Me.Pparen.Enabled = True
                    For i = 0 To Me.cresp.Items.Count - 1
                        If UCase(Me.cresp.Items(i)) = UCase(Me.tresp.Text) Then
                            Dim omessage As New bc_cs_message("Blue Curve", "Item: " + Me.tresp.Text + " already exists please select from exist option!", bc_cs_message.MESSAGE)
                            Me.tresp.Text = ""
                            Me.tresp.Focus()
                            Me.check_ok()
                            Exit Sub
                        End If
                    Next

                End If
                If tresp.Text <> "" Then
                    Me.check_ok()
                    For i = 0 To bc_am_calculation_values.calculations.Count - 1
                        For k = 0 To bc_am_calculation_values.rows.Count - 1
                            If bc_am_calculation_values.rows(k).static_flag = 0 Then
                                If bc_am_calculation_values.rows(k).name = tresp.Text Then
                                    If bc_am_calculation_values.calculations(i).num_years = bc_am_calculation_values.rows(k).item_id Then
                                        Dim omsg As New bc_cs_message("Blue Curve", "Item: " + Me.tresp.Text + " is already in use as a  static to period calculation result in calculation: " + bc_am_calculation_values.calculations(i).name, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                                        Me.tresp.Text = ""
                                        Me.tresp.Select()
                                        Me.check_ok()
                                        Exit Sub
                                    End If
                                End If
                            End If
                        Next
                    Next
                End If
            End If

            REM for an aggregation check result is not in use by another aggreagtion of the same taget class

            'If Me.lstresult.Visible = True Then
            '    REM check of static or period
            '    Dim static_flag As Integer = 1
            '    For j = 0 To bc_am_calculation_values.calc_types.Count - 1
            '        If bc_am_calculation_values.calc_types(j).name = Me.ccalctype.Text Then
            '            If bc_am_calculation_values.calc_types(j).type = 1 Then
            '                static_flag = 0
            '            End If
            '            For i = 0 To bc_am_calculation_values.agg_rows_in_use.Count - 1
            '                If bc_am_calculation_values.agg_rows_in_use(i).result_name = Me.lstresult.Text And static_flag = bc_am_calculation_values.agg_rows_in_use(i).static_flag Then
            '                    Dim omsg As New bc_cs_message("Blue Curve", "Item: " + Me.lstresult.Text + " is already in use as a  calculation result in Aggregation Universe: " + bc_am_calculation_values.agg_rows_in_use(i).agg_name + " please enter another item", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)

            '                    Me.lstresult.Select()
            '                    Exit Sub
            '                End If
            '            Next
            '        End If
            '    Next
            'End If

            Me.Cursor = Cursors.WaitCursor



            Dim ocalc As New bc_om_calculation
            If verify_calc(ocalc) = False Then
                Me.Cursor = Cursors.Default
                Exit Sub
            End If

            ocalc.VALIDATE = False

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                ocalc.db_write()
            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                ocalc.no_send_back = False
                ocalc.tmode = bc_cs_soap_base_class.tWRITE
                If ocalc.transmit_to_server_and_receive(ocalc, True) = False Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Calculation failed to save!", bc_cs_message.MESSAGE)
                    Exit Sub
                End If
            End If

            Me.changed = True
            Me.Hide()
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_calculation_editor", "save", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

            Me.Cursor = Cursors.Default
            slog = New bc_cs_activity_log("bc_am_in_calculation_editor", "save", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Private Function read_in_calc(ByRef ocalc As bc_om_calculation) As String

        Try
            Dim idx As Integer
            idx = get_Lib_calc_index()

            Dim i As Integer
            read_in_calc = ""
            'Dim ocalc As New bc_om_calculation
            ocalc.num_years = 0
            ocalc.type = Me.ccalctype.Text
            ocalc.intTemplateId = bc_am_calculation_values.TemplateId
            ocalc.intAggregationId = bc_am_calculation_values.AggregationId
            ocalc.monetary = Me.ccurr.SelectedIndex




            If Me.rbResultExist.Visible = False Then
                ocalc.id = modify_calc.id
                REM modify
                If period_calc = True Then
                    For i = 0 To bc_am_calculation_values.rows.Count - 1
                        REM period
                        If bc_am_calculation_values.rows(i).static_flag = False And bc_am_calculation_values.rows(i).name = Me.tresult.Text Then
                            ocalc.name = bc_am_calculation_values.rows(i).name
                            ocalc.result_row_id = bc_am_calculation_values.rows(i).item_id
                            Exit For
                        End If
                    Next
                Else
                    For i = 0 To bc_am_calculation_values.rows.Count - 1
                        REM static
                        If bc_am_calculation_values.rows(i).static_flag = True And bc_am_calculation_values.rows(i).name = Me.tresult.Text Then
                            ocalc.name = bc_am_calculation_values.rows(i).name
                            ocalc.result_row_id = bc_am_calculation_values.rows(i).item_id
                            Exit For
                        End If
                    Next
                End If
            Else

                If Me.rbResultExist.Checked And lstresult.Visible Then
                    If period_calc = True Then
                        For i = 0 To bc_am_calculation_values.rows.Count - 1
                            REM period
                            If bc_am_calculation_values.rows(i).static_flag = False And bc_am_calculation_values.rows(i).name = Me.lstresult.SelectedItem Then
                                ocalc.name = bc_am_calculation_values.rows(i).name
                                ocalc.result_row_id = bc_am_calculation_values.rows(i).item_id
                                Exit For
                            End If
                        Next
                    Else
                        For i = 0 To bc_am_calculation_values.rows.Count - 1
                            REM static
                            If bc_am_calculation_values.rows(i).static_flag = True And bc_am_calculation_values.rows(i).name = Me.lstresult.SelectedItem Then
                                ocalc.name = bc_am_calculation_values.rows(i).name
                                ocalc.result_row_id = bc_am_calculation_values.rows(i).item_id
                                Exit For
                            End If
                        Next
                    End If
                Else
                    REM new result row
                    ocalc.result_row_id = 0
                    ocalc.name = Me.tresult.Text
                End If
            End If
            REM static to period
            'If Me.rbPeriodExist.Visible = True Then

            If ocalc.intAggregationId = 0 Or IsNothing(ocalc.intAggregationId) Then
                If Me.rbPeriodExist.Checked = True And Me.rbPeriodExist.Visible = True Then
                    For i = 0 To bc_am_calculation_values.rows.Count - 1
                        If bc_am_calculation_values.rows(i).name = Me.cresp.SelectedItem Then
                            ocalc.num_years = bc_am_calculation_values.rows(i).item_id
                            Exit For
                        End If
                    Next
                ElseIf Me.rbPeriodExist.Visible = False Then
                    For i = 0 To bc_am_calculation_values.rows.Count - 1
                        If bc_am_calculation_values.rows(i).name = Me.tresp.Text Then
                            ocalc.num_years = bc_am_calculation_values.rows(i).item_id
                            Exit For
                        End If
                    Next
                Else
                    REM new result row
                    ocalc.period_name = Me.tresp.Text
                    ocalc.num_years = 0

                End If
             

            End If
                ocalc.monetary2 = Me.ccurrp1.SelectedIndex
                'End If
                If LCase(Me.ccalctype.Text) <> "static to period" And Me.clib.SelectedIndex > 0 And (IsNothing(bc_am_calculation_values.AggregationId) Or bc_am_calculation_values.AggregationId <= 0) Then
                    library_calc(ocalc)
                Else

                    REM aggregation libary functions

                    ocalc.contributor1_id = 0
                    ocalc.contributor2_id = 0
                    ocalc.interval_type = 0
                    ocalc.interval = 0

                    If Me.clib.SelectedIndex > 0 And bc_am_calculation_values.AggregationId > 0 Then

                        ocalc.num_years = bc_am_in_context.insight_items.libray_calcs.libray_calculations(idx).num_years
                        ocalc.contributor1_id = bc_am_in_context.insight_items.libray_calcs.libray_calculations(idx).contributor1_id
                        ocalc.contributor2_id = bc_am_in_context.insight_items.libray_calcs.libray_calculations(idx).contributor2_id
                        ocalc.interval_type = bc_am_in_context.insight_items.libray_calcs.libray_calculations(idx).interval_type
                        ocalc.interval = bc_am_in_context.insight_items.libray_calcs.libray_calculations(idx).interval
                    End If
                    If LCase(Me.ccalctype.Text) = "static to period" And Me.clib.SelectedIndex > 0 Then
                        ocalc.contributor2_id = bc_am_in_context.insight_items.libray_calcs.libray_calculations(idx).contributor2_id
                    End If

                    REM check if calc is price sensitive
                    ocalc.calc_when = 0

                    If check_price_sensitive() = True Then
                        ocalc.calc_when = 1
                    End If


                    Dim tx As String = parseOperands()

                    REM FIL MAY 2013
                    REM return text for later error reporting
                    read_in_calc = tx

                    REM 'PR removed FEB 2016 for linear denomnator
                    'If IsNothing(ocalc.intAggregationId) = False Then
                    '    tx = Replace(tx, " where ", " having ")
                    'End If

                    REM fill out calculation record
                    ocalc.formula = tx
                    REM existing result row

                    If Me.operands_ids.Count > 0 Then
                        ocalc.op1 = Me.operands_ids(0)
                    End If
                    If Me.operands_ids.Count > 1 Then
                        ocalc.op2 = Me.operands_ids(1)
                    End If
                    If Me.operands_ids.Count > 2 Then
                        ocalc.op3 = Me.operands_ids(2)
                    End If
                    If Me.operands_ids.Count > 3 Then
                        ocalc.op4 = Me.operands_ids(3)
                    End If
                    If Me.operands_ids.Count > 4 Then
                        ocalc.op5 = Me.operands_ids(4)
                    End If
                    If Me.operands_ids.Count > 5 Then
                        ocalc.op6 = Me.operands_ids(5)
                    End If
                    If Me.operands_ids.Count > 6 Then
                        ocalc.op7 = Me.operands_ids(6)
                    End If
                    If Me.operands_ids.Count > 7 Then
                        ocalc.op8 = Me.operands_ids(7)
                    End If

                    ocalc.na_tx = Me.uxNAText.Text

                    ocalc.inactive = False
                    If Me.chkinactive.Checked = True Then
                        ocalc.inactive = True
                    End If
                    ocalc.is_extract = True
                    If Me.Chkextract.Checked = False Then
                        ocalc.is_extract = False
                    End If
                    ocalc.min_val = ""
                    ocalc.max_val = ""
                    If Me.Chkmin.Checked = True Then
                        ocalc.min_val = Me.tminval.Text
                        ocalc.min_tx = Me.mintxt.Text
                    End If
                    If Me.Chkmax.Checked = True Then
                        ocalc.max_val = Me.tmaxval.Text
                        ocalc.max_tx = Me.maxtxt.Text
                    End If

                    ocalc.formula_error = ""
                    If Me.Chkps.Checked = True Then
                        ocalc.calc_when = 0
                    Else
                        ocalc.calc_when = 1
                    End If

                    ocalc.weighting_attribute = 0
                    If Me.cweight.Visible = True And Me.cweight.SelectedIndex > 0 Then
                        ocalc.weighting_attribute = bc_am_calculation_values.link_attributes.attributes(Me.cweight.SelectedIndex - 1).attribute_id
                    End If

              

            End If

            ocalc.exch_rate_method = 0
            ocalc.cal_operands = False
            If ocalc.intTemplateId > 0 Then
                If Me.ccal.Checked = True Then
                    ocalc.cal_operands = True
                End If
                If (Me.cav.Checked = True) Then
                    ocalc.exch_rate_method = 1
                Else
                    ocalc.exch_rate_method = 0
                End If

            End If
            If Me.tsd.Visible = True Then
                ocalc.standard_deviation = Me.tsd.Text
            End If
        Catch ex As Exception
            read_in_calc = ""
            Dim db_err As New bc_cs_error_log("bc_am_in_calculation_editor", "read_in_calc", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
        End Try
    End Function

    Private Sub bcancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bcancel.Click
        Me.Hide()
    End Sub
    Private Function check_brackets(ByVal tx As String) As String
        check_brackets = ""
        Dim bc As Integer = 0
        For i = 0 To tx.Count - 1
            If tx(i) = "(" Then
                bc = bc + 1
            ElseIf tx(i) = ")" Then
                bc = bc - 1
            End If
        Next
        If bc = 1 Then
            check_brackets = "parenthesis mismatch there is " + CStr(bc) + " more open bracket than close"
        ElseIf bc > 1 Then
            check_brackets = "parenthesis mismatch there are " + CStr(bc) + " more open brackets than close"
        ElseIf bc = -1 Then
            check_brackets = "parenthesis mismatch there is " + CStr(bc / -1) + " less open bracket than close"
        ElseIf bc < -1 Then
            check_brackets = "parenthesis mismatch there are " + CStr(bc / -1) + " less open brackets than close"
        End If

    End Function
    Private Function check_aggregates(ByVal tx As String) As Boolean
        Try
            Dim i As Integer
            Dim j As Integer
            Dim otx As String
            Dim idx As New List(Of Integer)
            check_aggregates = True
            otx = tx
            For i = 0 To bc_am_calculation_values.aggregation_types.Count - 1
                tx = otx
                REM check aggregates percede a (
                While InStr(UCase(tx), UCase(bc_am_calculation_values.aggregation_types(i)) + " ") > 0

                    j = InStr(UCase(tx), UCase(bc_am_calculation_values.aggregation_types(i)) + " ")

                    tx = tx.Substring(j + 3, Len(tx) - (j + 4))
                    tx = LTrim(tx)
                    If tx.Substring(0, 1) <> "(" Then
                        Return False
                    End If

                End While
            Next
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_in_calculation_editor", "check_aggregates", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try
    End Function

    Private Function check_items(ByVal tx As String) As String
        REM remove all reswerved words and see what ledt

        For i = 0 To bc_am_calculation_values.reserved_Words.Count - 1
            tx = tx.Replace(bc_am_calculation_values.reserved_Words(i), "|")
            tx = tx.Replace(LCase(bc_am_calculation_values.reserved_Words(i)), "|")
        Next

        'Dim utx As String

        'tx = tx.Replace("(", "|")
        'tx = tx.Replace(")", "|")
        'tx = tx.Replace("value_1", "|")
        'tx = tx.Replace("value_2", "|")
        'tx = tx.Replace("value_3", "|")
        'tx = tx.Replace("value_4", "|")
        'tx = tx.Replace("value_5", "|")
        'tx = tx.Replace("value_6", "|")
        'tx = tx.Replace("value_7", "|")
        'tx = tx.Replace("value_8", "|")
        'tx = tx.Replace("+", "|")
        'tx = tx.Replace("-", "|")
        'tx = tx.Replace("*", "|")
        'tx = tx.Replace("/", "|")
        'tx = tx.Replace("<>", "|")
        'tx = tx.Replace("case", "|")
        'tx = tx.Replace("isnull", "|")
        'tx = tx.Replace("power", "|")
        'tx = tx.Replace("decimal", "|")
        'tx = tx.Replace("30,15", "|")
        'tx = tx.Replace("abs", "|")
        'tx = tx.Replace("cast", "|")
        'tx = tx.Replace("SUM", "|")
        'tx = tx.Replace("AVG", "|")
        'tx = tx.Replace("MIN", "|")
        'tx = tx.Replace("MAX", "|")
        'tx = tx.Replace("WHERE", "|")
        'tx = tx.Replace("sum", "|")
        'tx = tx.Replace("avg", "|")
        'tx = tx.Replace("min", "|")
        'tx = tx.Replace("max", "|")
        'tx = tx.Replace("where", "|")
        'tx = tx.Replace(" as ", "|")
        'tx = tx.Replace("current_price", "|")
        'tx = tx.Replace("price_denominator", "|")
        'tx = tx.Replace("trading_denominator", "|")
        'tx = tx.Replace("currency_denominator", "|")
        'tx = tx.Replace(",0", "|")
        tx = tx.Replace("0", "")
        tx = tx.Replace("     ", " ")
        tx = tx.Replace("    ", " ")
        tx = tx.Replace("   ", " ")
        tx = tx.Replace("  ", " ")

        check_items = tx

    End Function

    Private Sub btnVerifyCalc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVerifyCalc.Click
        Dim ocalc As New bc_om_calculation
        Me.Cursor = Cursors.WaitCursor
        If verify_calc(ocalc) = True Then
            Dim omsg As New bc_cs_message("Blue Curve", "Valid Formula.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
        End If
        Me.Cursor = Cursors.Default
    End Sub
    Private Function verify_calc(ByRef ocalc As bc_om_calculation) As Boolean
        verify_calc = False
        Dim pformula As String

        Me.txformula.Text = Me.txformula.Text


        pformula = read_in_calc(ocalc)


        REM PR dont verify for a lib calc
        If Me.clib.SelectedIndex > 0 And (IsNothing(bc_am_calculation_values.AggregationId) Or bc_am_calculation_values.AggregationId <= 0) Then
            verify_calc = True
            Exit Function
        End If


        'ocalc.formula = parseOperands() 'txformula.Text
        If Not ccalctype.SelectedItem Is Nothing Then
            ocalc.type = ccalctype.SelectedItem.ToString
        Else
            ocalc.type = ""
        End If

        ocalc.VALIDATE = True

        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
            ocalc.db_write()
        Else
            ocalc.tmode = bc_cs_soap_base_class.tWRITE
            If ocalc.transmit_to_server_and_receive(ocalc, True) = False Then
                Exit Function
            End If
        End If
        Dim rtf As String
        If ocalc.formula_error <> "" Then
            REM FIL May 2013
            REM parse to get a more user friendly error message
            Dim etx As String
            Dim omsg As bc_cs_message
            etx = check_brackets(pformula)
            If etx <> "" Then
                omsg = New bc_cs_message("Blue Curve", "Error in formula: " + etx, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Function
            End If
            If IsNothing(ocalc.intAggregationId) = False Then
                If check_aggregates(pformula) = False Then
                    omsg = New bc_cs_message("Blue Curve", "Missing ( before aggregate: " + etx, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Function
                End If
            End If
            etx = ""
            etx = check_items(pformula)
            etx = Trim(etx)

            Dim markup_formula As String
            Dim bad_words As String = ""
            markup_formula = markup_formula_errors(Me.txformula.Text, etx, bad_words)


            If bad_words <> "" Then
                rtf = "{\rtf1\ansi\deff0{\fonttbl{\f0\fnil\fcharset0 Verdana;}}\viewkind4\uc1\pard\lang2057\f0\fs17 " + markup_formula + " \par"
                Me.txformula.Rtf = rtf
                omsg = New bc_cs_message("Blue Curve", "Error in formula with the following items: " + bad_words + vbCr + vbCr + "System error: " + ocalc.formula_error, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Function
            End If
            omsg = New bc_cs_message("Blue Curve", "Error in formula: " + ocalc.formula_error, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)

            Exit Function
        Else
            rtf = Me.txformula.Rtf
            rtf = rtf.Replace("\b", "")
            rtf = rtf.Replace("\b0", "")
            Me.txformula.Rtf = rtf
        End If

        verify_calc = True

    End Function
    Private Function markup_formula_errors(ByVal t As String, ByVal r As String, ByRef badwords As String) As String
        Try
            Dim words As New ArrayList
            Dim word As String
            Dim original_formula As String
            Dim lr As String
            lr = r
            original_formula = LCase(t)

            r = r + "|"

            While InStr(r, "|") > 0
                word = r.Substring(0, InStr(r, "|") - 1)
                r = r.Substring(InStr(r, "|"), Len(r) - InStr(r, "|"))
                If Trim(word) <> "" And IsNumeric(word) = False Then
                    words.Add(Trim(word))
                End If
            End While

            badwords = ""
            For i = 0 To words.Count - 1
                If badwords = "" Then
                    badwords = words(i)
                Else
                    badwords = badwords + "; " + words(i)
                End If
                original_formula = original_formula.Replace(words(i), "\b " + words(i) + " \b0")
            Next

            original_formula = original_formula.Replace("|", "")




            markup_formula_errors = original_formula
        Catch ex As Exception
            Dim ocomm As New bc_cs_activity_log("bc_am_in_calculation_editor", "markup_formula_errors", bc_cs_activity_codes.COMMENTARY, ex.Message)
            markup_formula_errors = ""
        End Try
    End Function
    Private Sub Chkmin_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chkmin.CheckedChanged
        Me.tminval.Text = ""
        Me.mintxt.Text = ""
        Me.tminval.Enabled = False
        Me.mintxt.Enabled = False
        Me.Label4.Enabled = False

        If Me.Chkmin.Checked = True Then
            Me.tminval.Enabled = True
            Me.mintxt.Enabled = True
            Me.Label4.Enabled = True
        End If
        check_ok()

    End Sub

    Private Sub Chkmax_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chkmax.CheckedChanged
        Me.tmaxval.Text = ""
        Me.maxtxt.Text = ""
        Me.tmaxval.Enabled = False
        Me.maxtxt.Enabled = False
        Me.Label5.Enabled = False

        If Me.Chkmax.Checked = True Then
            Me.tmaxval.Enabled = True
            Me.maxtxt.Enabled = True
            Me.Label5.Enabled = True
        End If
        check_ok()
    End Sub

    Private Sub tminval_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tminval.TextChanged
        check_ok()
    End Sub

    Private Sub tmaxval_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmaxval.TextChanged
        check_ok()
    End Sub

    Private Sub chkinactive_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkinactive.CheckedChanged
        check_ok()
    End Sub

    Private Sub Chkextract_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chkextract.CheckedChanged
        check_ok()
    End Sub

    Private Sub tresp_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tresp.TextChanged
        check_ok()
    End Sub

    Private Sub cresp_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cresp.SelectedIndexChanged
        If Me.cresp.SelectedIndex > -1 Then
            For i = 0 To bc_am_calculation_values.rows.Count - 1
                If bc_am_calculation_values.rows(i).static_flag = 0 And bc_am_calculation_values.rows(i).name = Me.cresp.Text Then
                    Me.ccurrp1.SelectedIndex = bc_am_calculation_values.rows(i).curr_type
                End If
            Next
        End If
        check_ok()
    End Sub
    Private Sub lstoperandfixed_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstoperandfixed.DoubleClick

        If lstoperandfixed.SelectedIndex = -1 Then
            Exit Sub
        End If
        write_out_formula(" " + Me.lstoperandfixed.SelectedItem + " ")
    End Sub

    Private Sub uxNAText_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxNAText.TextChanged
        check_ok()
    End Sub

    Private Sub Chkps_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chkps.CheckedChanged
        check_ok()
    End Sub

    Private Sub bc_am_in_calculation_editor_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize

        ' stop resizing when a certain size is reached
        If Me.Width < 596 Then
            Me.Width = 596
            Exit Sub
        End If

        If Me.Height < 703 Then
            Me.Height = 703
            Exit Sub
        End If

    End Sub

   
    Private Sub ccurr_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ccurr.SelectedIndexChanged
        Me.check_ok()

    End Sub

    Private Sub ccurrp1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ccurrp1.SelectedIndexChanged
        Me.check_ok()

    End Sub

    Private Sub cweight_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cweight.SelectedIndexChanged
        check_ok()
    End Sub

   
    Private Sub lstoperand_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstoperand.SelectedIndexChanged

    End Sub

    Private Sub ccal_CheckedChanged(sender As Object, e As EventArgs) Handles ccal.CheckedChanged
        Me.check_ok()
    End Sub

    Private Sub cav_CheckedChanged(sender As Object, e As EventArgs) Handles cav.CheckedChanged
        Me.check_ok()
    End Sub

    Private Sub tsd_TextChanged(sender As Object, e As EventArgs) Handles tsd.TextChanged
        check_ok()

    End Sub
End Class
