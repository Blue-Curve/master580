Imports System.Windows.Forms.Screen
Imports System.io
Imports Microsoft.Win32
Imports System.Windows.forms
Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports BlueCurve.Create.AM
Imports BlueCurve.Core.AS
Public Class bc_am_in_settings
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
    Friend WithEvents FolderBrowserDialog1 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents uxGeneral As System.Windows.Forms.GroupBox
    Friend WithEvents Lsec As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader9 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader10 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader11 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader12 As System.Windows.Forms.ColumnHeader
    Friend WithEvents centdef As System.Windows.Forms.CheckBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents callent As System.Windows.Forms.CheckBox
    Friend WithEvents tformatsheet As System.Windows.Forms.Label
    Friend WithEvents tdir As System.Windows.Forms.TextBox
    Friend WithEvents clocalcopy As System.Windows.Forms.CheckBox
    Friend WithEvents ccontrol As System.Windows.Forms.CheckBox
    Friend WithEvents chide As System.Windows.Forms.CheckBox
    Friend WithEvents uxLinkOps As System.Windows.Forms.GroupBox
    Friend WithEvents bexclude As System.Windows.Forms.RadioButton
    Friend WithEvents cstart As System.Windows.Forms.ComboBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents cblanks As System.Windows.Forms.ComboBox
    Friend WithEvents cshowerrors As System.Windows.Forms.CheckBox
    Friend WithEvents tsheets As System.Windows.Forms.Label
    Friend WithEvents taddsheet As System.Windows.Forms.TextBox
    Friend WithEvents bdelsheet As System.Windows.Forms.Button
    Friend WithEvents baddsheet As System.Windows.Forms.Button
    Friend WithEvents lsheets As System.Windows.Forms.ListBox
    Friend WithEvents rinclude As System.Windows.Forms.RadioButton
    Friend WithEvents uxLinkCodes As System.Windows.Forms.GroupBox
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents tlabcol As System.Windows.Forms.TextBox
    Friend WithEvents tstcol As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents teprefix As System.Windows.Forms.TextBox
    Friend WithEvents taprefix As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents tacccode As System.Windows.Forms.TextBox
    Friend WithEvents teacode As System.Windows.Forms.TextBox
    Friend WithEvents tperiodcode As System.Windows.Forms.TextBox
    Friend WithEvents theadercode As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents uxMapCodes As System.Windows.Forms.GroupBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents lacccodes As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents racc As System.Windows.Forms.RadioButton
    Friend WithEvents rperiod As System.Windows.Forms.RadioButton
    Friend WithEvents lcodes As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader7 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader8 As System.Windows.Forms.ColumnHeader
    Friend WithEvents bdelcode As System.Windows.Forms.Button
    Friend WithEvents baddcode As System.Windows.Forms.Button
    Friend WithEvents textcode As System.Windows.Forms.TextBox
    Friend WithEvents cbccodes As System.Windows.Forms.ComboBox
    Friend WithEvents uxAggAudit As System.Windows.Forms.GroupBox
    Friend WithEvents uxListAggAudit As System.Windows.Forms.ListView
    Friend WithEvents uxlFilter As System.Windows.Forms.Label
    Friend WithEvents uxldate As System.Windows.Forms.Label
    Friend WithEvents uxAggUniverse As System.Windows.Forms.ComboBox
    Friend WithEvents uxAggDate As System.Windows.Forms.ComboBox
    Friend WithEvents uxlUniverse As System.Windows.Forms.Label
    Friend WithEvents uxDetails As System.Windows.Forms.Button
    Friend WithEvents universe As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader13 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader14 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader15 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader16 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader17 As System.Windows.Forms.ColumnHeader
    Friend WithEvents uxRefresh As System.Windows.Forms.Button
    Friend WithEvents uxExport As System.Windows.Forms.Button
    Friend WithEvents Tbatch As System.Windows.Forms.TextBox
    Friend WithEvents Crefreshlists As System.Windows.Forms.ComboBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents uxImages As System.Windows.Forms.ImageList
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_in_settings))
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Lsec = New System.Windows.Forms.ListView
        Me.ColumnHeader9 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader10 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader11 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader12 = New System.Windows.Forms.ColumnHeader
        Me.uxImages = New System.Windows.Forms.ImageList(Me.components)
        Me.centdef = New System.Windows.Forms.CheckBox
        Me.callent = New System.Windows.Forms.CheckBox
        Me.clocalcopy = New System.Windows.Forms.CheckBox
        Me.ccontrol = New System.Windows.Forms.CheckBox
        Me.chide = New System.Windows.Forms.CheckBox
        Me.bexclude = New System.Windows.Forms.RadioButton
        Me.cstart = New System.Windows.Forms.ComboBox
        Me.cblanks = New System.Windows.Forms.ComboBox
        Me.cshowerrors = New System.Windows.Forms.CheckBox
        Me.taddsheet = New System.Windows.Forms.TextBox
        Me.bdelsheet = New System.Windows.Forms.Button
        Me.baddsheet = New System.Windows.Forms.Button
        Me.lsheets = New System.Windows.Forms.ListBox
        Me.rinclude = New System.Windows.Forms.RadioButton
        Me.Button2 = New System.Windows.Forms.Button
        Me.tlabcol = New System.Windows.Forms.TextBox
        Me.tstcol = New System.Windows.Forms.TextBox
        Me.teprefix = New System.Windows.Forms.TextBox
        Me.taprefix = New System.Windows.Forms.TextBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.tacccode = New System.Windows.Forms.TextBox
        Me.teacode = New System.Windows.Forms.TextBox
        Me.tperiodcode = New System.Windows.Forms.TextBox
        Me.theadercode = New System.Windows.Forms.TextBox
        Me.lacccodes = New System.Windows.Forms.ListView
        Me.ColumnHeader5 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader6 = New System.Windows.Forms.ColumnHeader
        Me.racc = New System.Windows.Forms.RadioButton
        Me.rperiod = New System.Windows.Forms.RadioButton
        Me.lcodes = New System.Windows.Forms.ListView
        Me.ColumnHeader7 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader8 = New System.Windows.Forms.ColumnHeader
        Me.bdelcode = New System.Windows.Forms.Button
        Me.baddcode = New System.Windows.Forms.Button
        Me.textcode = New System.Windows.Forms.TextBox
        Me.cbccodes = New System.Windows.Forms.ComboBox
        Me.ColumnHeader4 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.uxGeneral = New System.Windows.Forms.GroupBox
        Me.Label15 = New System.Windows.Forms.Label
        Me.tformatsheet = New System.Windows.Forms.Label
        Me.tdir = New System.Windows.Forms.TextBox
        Me.uxLinkOps = New System.Windows.Forms.GroupBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.tsheets = New System.Windows.Forms.Label
        Me.uxLinkCodes = New System.Windows.Forms.GroupBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.uxMapCodes = New System.Windows.Forms.GroupBox
        Me.Label14 = New System.Windows.Forms.Label
        Me.Label13 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.uxAggAudit = New System.Windows.Forms.GroupBox
        Me.uxExport = New System.Windows.Forms.Button
        Me.uxRefresh = New System.Windows.Forms.Button
        Me.uxDetails = New System.Windows.Forms.Button
        Me.uxAggUniverse = New System.Windows.Forms.ComboBox
        Me.uxAggDate = New System.Windows.Forms.ComboBox
        Me.uxlUniverse = New System.Windows.Forms.Label
        Me.uxldate = New System.Windows.Forms.Label
        Me.uxlFilter = New System.Windows.Forms.Label
        Me.uxListAggAudit = New System.Windows.Forms.ListView
        Me.universe = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader13 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader14 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader15 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader16 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader17 = New System.Windows.Forms.ColumnHeader
        Me.Label16 = New System.Windows.Forms.Label
        Me.Crefreshlists = New System.Windows.Forms.ComboBox
        Me.Tbatch = New System.Windows.Forms.TextBox
        Me.uxGeneral.SuspendLayout()
        Me.uxLinkOps.SuspendLayout()
        Me.uxLinkCodes.SuspendLayout()
        Me.uxMapCodes.SuspendLayout()
        Me.uxAggAudit.SuspendLayout()
        Me.SuspendLayout()
        '
        'Lsec
        '
        Me.Lsec.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Lsec.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader9, Me.ColumnHeader10, Me.ColumnHeader11, Me.ColumnHeader12})
        Me.Lsec.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Lsec.FullRowSelect = True
        Me.Lsec.HideSelection = False
        Me.Lsec.Location = New System.Drawing.Point(37, 163)
        Me.Lsec.MultiSelect = False
        Me.Lsec.Name = "Lsec"
        Me.Lsec.Size = New System.Drawing.Size(792, 191)
        Me.Lsec.SmallImageList = Me.uxImages
        Me.Lsec.TabIndex = 8
        Me.ToolTip1.SetToolTip(Me.Lsec, "Detemines whether a user of a particular role can submit data to draft or publish" & _
                " or both.")
        Me.Lsec.UseCompatibleStateImageBehavior = False
        Me.Lsec.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader9
        '
        Me.ColumnHeader9.Text = "Role"
        Me.ColumnHeader9.Width = 177
        '
        'ColumnHeader10
        '
        Me.ColumnHeader10.Text = "Submit to Draft"
        Me.ColumnHeader10.Width = 161
        '
        'ColumnHeader11
        '
        Me.ColumnHeader11.Text = "Submit to Publish"
        Me.ColumnHeader11.Width = 134
        '
        'ColumnHeader12
        '
        Me.ColumnHeader12.Text = "Default"
        Me.ColumnHeader12.Width = 107
        '
        'uxImages
        '
        Me.uxImages.ImageStream = CType(resources.GetObject("uxImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.uxImages.TransparentColor = System.Drawing.Color.Transparent
        Me.uxImages.Images.SetKeyName(0, "")
        '
        'centdef
        '
        Me.centdef.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.centdef.Location = New System.Drawing.Point(261, 112)
        Me.centdef.Name = "centdef"
        Me.centdef.Size = New System.Drawing.Size(224, 24)
        Me.centdef.TabIndex = 6
        Me.centdef.Text = "Default to my entities"
        Me.ToolTip1.SetToolTip(Me.centdef, "if show all entities in build wizard is set determines whether default list in bu" & _
                "ild option is all entities or my entities")
        '
        'callent
        '
        Me.callent.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.callent.Location = New System.Drawing.Point(37, 111)
        Me.callent.Name = "callent"
        Me.callent.Size = New System.Drawing.Size(224, 24)
        Me.callent.TabIndex = 5
        Me.callent.Text = "Show all entities in build wizard"
        Me.ToolTip1.SetToolTip(Me.callent, "Determines whether user can see all entities and my entities or just my entities " & _
                "in build option")
        '
        'clocalcopy
        '
        Me.clocalcopy.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.clocalcopy.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.clocalcopy.Location = New System.Drawing.Point(205, 85)
        Me.clocalcopy.Name = "clocalcopy"
        Me.clocalcopy.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.clocalcopy.Size = New System.Drawing.Size(128, 16)
        Me.clocalcopy.TabIndex = 3
        Me.clocalcopy.Text = "Local Copy Save"
        Me.ToolTip1.SetToolTip(Me.clocalcopy, "If checked on controlled submission a read only copy of the workbook can be store" & _
                "d in the promted for location on submission of the main workbook. This is just f" & _
                "or reference purposes for the user.")
        Me.clocalcopy.UseVisualStyleBackColor = False
        Me.clocalcopy.Visible = False
        '
        'ccontrol
        '
        Me.ccontrol.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.ccontrol.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ccontrol.Location = New System.Drawing.Point(37, 85)
        Me.ccontrol.Name = "ccontrol"
        Me.ccontrol.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ccontrol.Size = New System.Drawing.Size(152, 16)
        Me.ccontrol.TabIndex = 2
        Me.ccontrol.Text = "Controlled Submission"
        Me.ToolTip1.SetToolTip(Me.ccontrol, "If checked then on submit of the workbook the workbook itself can be checked into" & _
                " the system and later opened using the Blue Curve Open function. The local workb" & _
                "ook itself will be removed.")
        Me.ccontrol.UseVisualStyleBackColor = False
        '
        'chide
        '
        Me.chide.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.chide.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chide.Location = New System.Drawing.Point(37, 53)
        Me.chide.Name = "chide"
        Me.chide.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chide.Size = New System.Drawing.Size(152, 16)
        Me.chide.TabIndex = 1
        Me.chide.Text = "Hide Sheets"
        Me.ToolTip1.SetToolTip(Me.chide, "If checked dont show the Blue Curve Insight Sheets after a build or rebuild")
        Me.chide.UseVisualStyleBackColor = False
        '
        'bexclude
        '
        Me.bexclude.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bexclude.Location = New System.Drawing.Point(24, 108)
        Me.bexclude.Name = "bexclude"
        Me.bexclude.Size = New System.Drawing.Size(120, 16)
        Me.bexclude.TabIndex = 6
        Me.bexclude.Text = "Exclude Sheets"
        Me.ToolTip1.SetToolTip(Me.bexclude, "If checked then the linker will not  look in specified worksheets for linking val" & _
                "ues. This can enhance the performance of the linker as it knows to ignore sheets" & _
                " as specified in right hand list")
        '
        'cstart
        '
        Me.cstart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cstart.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cstart.Location = New System.Drawing.Point(219, 45)
        Me.cstart.Name = "cstart"
        Me.cstart.Size = New System.Drawing.Size(56, 21)
        Me.cstart.TabIndex = 2
        Me.ToolTip1.SetToolTip(Me.cstart, "Determines on a client worksheet where headers columns start from")
        '
        'cblanks
        '
        Me.cblanks.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cblanks.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cblanks.Location = New System.Drawing.Point(136, 73)
        Me.cblanks.Name = "cblanks"
        Me.cblanks.Size = New System.Drawing.Size(56, 21)
        Me.cblanks.TabIndex = 4
        Me.ToolTip1.SetToolTip(Me.cblanks, "Specifies how many columns to check that are blank before the linking determines " & _
                "there are no more headers to link. This allows blank columns in the client works" & _
                "heets.")
        '
        'cshowerrors
        '
        Me.cshowerrors.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.cshowerrors.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cshowerrors.Location = New System.Drawing.Point(24, 21)
        Me.cshowerrors.Name = "cshowerrors"
        Me.cshowerrors.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cshowerrors.Size = New System.Drawing.Size(280, 18)
        Me.cshowerrors.TabIndex = 0
        Me.cshowerrors.Text = "Show linked header Errors as cell comment"
        Me.ToolTip1.SetToolTip(Me.cshowerrors, "If checked on build or rebuild of any of the headers in the client worksheets can" & _
                "not be parsed, it will write a cell comment highlighting why with a range of val" & _
                "id values.")
        Me.cshowerrors.UseVisualStyleBackColor = False
        '
        'taddsheet
        '
        Me.taddsheet.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.taddsheet.Location = New System.Drawing.Point(160, 124)
        Me.taddsheet.Name = "taddsheet"
        Me.taddsheet.Size = New System.Drawing.Size(128, 21)
        Me.taddsheet.TabIndex = 7
        Me.ToolTip1.SetToolTip(Me.taddsheet, "Enter here name of worksheet to include or exclude depending upon which option is" & _
                " checked.")
        '
        'bdelsheet
        '
        Me.bdelsheet.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.bdelsheet.Enabled = False
        Me.bdelsheet.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bdelsheet.Image = CType(resources.GetObject("bdelsheet.Image"), System.Drawing.Image)
        Me.bdelsheet.Location = New System.Drawing.Point(328, 141)
        Me.bdelsheet.Name = "bdelsheet"
        Me.bdelsheet.Size = New System.Drawing.Size(32, 26)
        Me.bdelsheet.TabIndex = 11
        Me.ToolTip1.SetToolTip(Me.bdelsheet, "Delete sheet name")
        Me.bdelsheet.UseVisualStyleBackColor = False
        '
        'baddsheet
        '
        Me.baddsheet.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.baddsheet.Enabled = False
        Me.baddsheet.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.baddsheet.Image = CType(resources.GetObject("baddsheet.Image"), System.Drawing.Image)
        Me.baddsheet.Location = New System.Drawing.Point(328, 112)
        Me.baddsheet.Name = "baddsheet"
        Me.baddsheet.Size = New System.Drawing.Size(32, 26)
        Me.baddsheet.TabIndex = 10
        Me.ToolTip1.SetToolTip(Me.baddsheet, "Add sheet name")
        Me.baddsheet.UseVisualStyleBackColor = False
        '
        'lsheets
        '
        Me.lsheets.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lsheets.Location = New System.Drawing.Point(376, 110)
        Me.lsheets.Name = "lsheets"
        Me.lsheets.Size = New System.Drawing.Size(264, 56)
        Me.lsheets.TabIndex = 12
        Me.ToolTip1.SetToolTip(Me.lsheets, "List of sheets to include or excludewhen linking depending on option checked when" & _
                " insight links a workbook.")
        '
        'rinclude
        '
        Me.rinclude.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rinclude.Location = New System.Drawing.Point(24, 140)
        Me.rinclude.Name = "rinclude"
        Me.rinclude.Size = New System.Drawing.Size(120, 16)
        Me.rinclude.TabIndex = 8
        Me.rinclude.Text = "Include Sheets"
        Me.ToolTip1.SetToolTip(Me.rinclude, "If checked then the linker will only  look in specified worksheets for linking va" & _
                "lues. This can enhance the performance of the linker as it knows to ignore sheet" & _
                "s as specified in right hand list")
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.Button2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button2.Image = CType(resources.GetObject("Button2.Image"), System.Drawing.Image)
        Me.Button2.Location = New System.Drawing.Point(604, 20)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(32, 26)
        Me.Button2.TabIndex = 16
        Me.ToolTip1.SetToolTip(Me.Button2, "Show an sample of how link codes are placed")
        Me.Button2.UseVisualStyleBackColor = False
        '
        'tlabcol
        '
        Me.tlabcol.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tlabcol.Location = New System.Drawing.Point(492, 124)
        Me.tlabcol.Name = "tlabcol"
        Me.tlabcol.Size = New System.Drawing.Size(112, 21)
        Me.tlabcol.TabIndex = 15
        Me.ToolTip1.SetToolTip(Me.tlabcol, "Code in client worksheet to determine that client labels are stored in this colum" & _
                "n. This is used to link flexible labels.")
        '
        'tstcol
        '
        Me.tstcol.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tstcol.Location = New System.Drawing.Point(492, 92)
        Me.tstcol.Name = "tstcol"
        Me.tstcol.Size = New System.Drawing.Size(112, 21)
        Me.tstcol.TabIndex = 13
        Me.ToolTip1.SetToolTip(Me.tstcol, "Code in excel worksheets that determine that staic data is stored in this column")
        '
        'teprefix
        '
        Me.teprefix.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.teprefix.Location = New System.Drawing.Point(444, 60)
        Me.teprefix.Name = "teprefix"
        Me.teprefix.Size = New System.Drawing.Size(28, 21)
        Me.teprefix.TabIndex = 11
        Me.ToolTip1.SetToolTip(Me.teprefix, "character to denote an actual period")
        '
        'taprefix
        '
        Me.taprefix.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.taprefix.Location = New System.Drawing.Point(444, 28)
        Me.taprefix.Name = "taprefix"
        Me.taprefix.Size = New System.Drawing.Size(28, 21)
        Me.taprefix.TabIndex = 9
        Me.ToolTip1.SetToolTip(Me.taprefix, "Client code to represent data is an Estimate. (e.g. E)")
        '
        'Label8
        '
        Me.Label8.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(342, 62)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(96, 16)
        Me.Label8.TabIndex = 8
        Me.Label8.Text = "Estimate Suffix"
        Me.ToolTip1.SetToolTip(Me.Label8, "character to denote an estimate period")
        '
        'tacccode
        '
        Me.tacccode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tacccode.Location = New System.Drawing.Point(204, 124)
        Me.tacccode.Name = "tacccode"
        Me.tacccode.Size = New System.Drawing.Size(112, 21)
        Me.tacccode.TabIndex = 7
        Me.ToolTip1.SetToolTip(Me.tacccode, "Optional Code used in client worksheets to depict row that the accounting code is" & _
                " stored in")
        Me.tacccode.Visible = False
        '
        'teacode
        '
        Me.teacode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.teacode.Location = New System.Drawing.Point(204, 60)
        Me.teacode.Name = "teacode"
        Me.teacode.Size = New System.Drawing.Size(112, 21)
        Me.teacode.TabIndex = 3
        Me.ToolTip1.SetToolTip(Me.teacode, "Code used in client worksheets to depict row that the estimate or actual prefix i" & _
                "s stored in")
        '
        'tperiodcode
        '
        Me.tperiodcode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tperiodcode.Location = New System.Drawing.Point(204, 92)
        Me.tperiodcode.Name = "tperiodcode"
        Me.tperiodcode.Size = New System.Drawing.Size(112, 21)
        Me.tperiodcode.TabIndex = 5
        Me.ToolTip1.SetToolTip(Me.tperiodcode, "Code used in client worksheets to depict row that the Period is stored in.")
        '
        'theadercode
        '
        Me.theadercode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.theadercode.Location = New System.Drawing.Point(204, 28)
        Me.theadercode.Name = "theadercode"
        Me.theadercode.Size = New System.Drawing.Size(112, 21)
        Me.theadercode.TabIndex = 1
        Me.ToolTip1.SetToolTip(Me.theadercode, "Code used in client worksheets to depict row that the header is stored in.")
        '
        'lacccodes
        '
        Me.lacccodes.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader5, Me.ColumnHeader6})
        Me.lacccodes.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lacccodes.FullRowSelect = True
        Me.lacccodes.Location = New System.Drawing.Point(376, 38)
        Me.lacccodes.Name = "lacccodes"
        Me.lacccodes.Size = New System.Drawing.Size(264, 88)
        Me.lacccodes.TabIndex = 9
        Me.ToolTip1.SetToolTip(Me.lacccodes, "list of all available accounting client codes to Blue Curve system codes")
        Me.lacccodes.UseCompatibleStateImageBehavior = False
        Me.lacccodes.View = System.Windows.Forms.View.Details
        Me.lacccodes.Visible = False
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "BC Code"
        Me.ColumnHeader5.Width = 114
        '
        'ColumnHeader6
        '
        Me.ColumnHeader6.Text = "Client Code"
        Me.ColumnHeader6.Width = 101
        '
        'racc
        '
        Me.racc.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.racc.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.racc.Location = New System.Drawing.Point(96, 32)
        Me.racc.Name = "racc"
        Me.racc.Size = New System.Drawing.Size(88, 16)
        Me.racc.TabIndex = 1
        Me.racc.Text = "Accounting"
        Me.ToolTip1.SetToolTip(Me.racc, "Map Accounting codes e.g IFRS, US GAAP")
        Me.racc.UseVisualStyleBackColor = False
        '
        'rperiod
        '
        Me.rperiod.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.rperiod.Checked = True
        Me.rperiod.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rperiod.Location = New System.Drawing.Point(24, 32)
        Me.rperiod.Name = "rperiod"
        Me.rperiod.Size = New System.Drawing.Size(64, 16)
        Me.rperiod.TabIndex = 0
        Me.rperiod.TabStop = True
        Me.rperiod.Text = "Period"
        Me.ToolTip1.SetToolTip(Me.rperiod, "Map period codes (e.g. Full Year Q1, H1)")
        Me.rperiod.UseVisualStyleBackColor = False
        '
        'lcodes
        '
        Me.lcodes.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader7, Me.ColumnHeader8})
        Me.lcodes.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lcodes.FullRowSelect = True
        Me.lcodes.Location = New System.Drawing.Point(376, 38)
        Me.lcodes.Name = "lcodes"
        Me.lcodes.Size = New System.Drawing.Size(264, 88)
        Me.lcodes.TabIndex = 10
        Me.ToolTip1.SetToolTip(Me.lcodes, "list of all available period client codes to Blue Cuvre system codes")
        Me.lcodes.UseCompatibleStateImageBehavior = False
        Me.lcodes.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader7
        '
        Me.ColumnHeader7.Text = "BC Code"
        Me.ColumnHeader7.Width = 112
        '
        'ColumnHeader8
        '
        Me.ColumnHeader8.Text = "Client Code"
        Me.ColumnHeader8.Width = 103
        '
        'bdelcode
        '
        Me.bdelcode.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.bdelcode.Enabled = False
        Me.bdelcode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bdelcode.Image = CType(resources.GetObject("bdelcode.Image"), System.Drawing.Image)
        Me.bdelcode.Location = New System.Drawing.Point(328, 88)
        Me.bdelcode.Name = "bdelcode"
        Me.bdelcode.Size = New System.Drawing.Size(32, 26)
        Me.bdelcode.TabIndex = 8
        Me.ToolTip1.SetToolTip(Me.bdelcode, "delete mapping")
        Me.bdelcode.UseVisualStyleBackColor = False
        '
        'baddcode
        '
        Me.baddcode.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.baddcode.Enabled = False
        Me.baddcode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.baddcode.Image = CType(resources.GetObject("baddcode.Image"), System.Drawing.Image)
        Me.baddcode.Location = New System.Drawing.Point(328, 54)
        Me.baddcode.Name = "baddcode"
        Me.baddcode.Size = New System.Drawing.Size(32, 26)
        Me.baddcode.TabIndex = 7
        Me.ToolTip1.SetToolTip(Me.baddcode, "add mapping")
        Me.baddcode.UseVisualStyleBackColor = False
        '
        'textcode
        '
        Me.textcode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.textcode.Location = New System.Drawing.Point(200, 53)
        Me.textcode.Name = "textcode"
        Me.textcode.Size = New System.Drawing.Size(104, 21)
        Me.textcode.TabIndex = 3
        Me.ToolTip1.SetToolTip(Me.textcode, "Period code used in client excel workbook.")
        '
        'cbccodes
        '
        Me.cbccodes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbccodes.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbccodes.Location = New System.Drawing.Point(200, 94)
        Me.cbccodes.Name = "cbccodes"
        Me.cbccodes.Size = New System.Drawing.Size(104, 21)
        Me.cbccodes.TabIndex = 6
        Me.ToolTip1.SetToolTip(Me.cbccodes, "List of System stored period codes.")
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Client Code"
        Me.ColumnHeader4.Width = 141
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "BC Code"
        Me.ColumnHeader3.Width = 106
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Client Code"
        Me.ColumnHeader2.Width = 141
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "BC Code"
        Me.ColumnHeader1.Width = 106
        '
        'uxGeneral
        '
        Me.uxGeneral.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxGeneral.Controls.Add(Me.Lsec)
        Me.uxGeneral.Controls.Add(Me.centdef)
        Me.uxGeneral.Controls.Add(Me.Label15)
        Me.uxGeneral.Controls.Add(Me.callent)
        Me.uxGeneral.Controls.Add(Me.tformatsheet)
        Me.uxGeneral.Controls.Add(Me.tdir)
        Me.uxGeneral.Controls.Add(Me.clocalcopy)
        Me.uxGeneral.Controls.Add(Me.ccontrol)
        Me.uxGeneral.Controls.Add(Me.chide)
        Me.uxGeneral.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxGeneral.Location = New System.Drawing.Point(8, 8)
        Me.uxGeneral.Name = "uxGeneral"
        Me.uxGeneral.Size = New System.Drawing.Size(845, 362)
        Me.uxGeneral.TabIndex = 0
        Me.uxGeneral.TabStop = False
        Me.uxGeneral.Text = "General"
        '
        'Label15
        '
        Me.Label15.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(35, 141)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(160, 16)
        Me.Label15.TabIndex = 7
        Me.Label15.Text = "Role Based Submission "
        '
        'tformatsheet
        '
        Me.tformatsheet.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tformatsheet.Location = New System.Drawing.Point(35, 21)
        Me.tformatsheet.Name = "tformatsheet"
        Me.tformatsheet.Size = New System.Drawing.Size(386, 24)
        Me.tformatsheet.TabIndex = 0
        Me.tformatsheet.Text = "Format sheet: none"
        '
        'tdir
        '
        Me.tdir.Enabled = False
        Me.tdir.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tdir.Location = New System.Drawing.Point(349, 77)
        Me.tdir.Name = "tdir"
        Me.tdir.Size = New System.Drawing.Size(291, 21)
        Me.tdir.TabIndex = 4
        Me.tdir.Visible = False
        '
        'uxLinkOps
        '
        Me.uxLinkOps.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxLinkOps.Controls.Add(Me.Tbatch)
        Me.uxLinkOps.Controls.Add(Me.Crefreshlists)
        Me.uxLinkOps.Controls.Add(Me.Label16)
        Me.uxLinkOps.Controls.Add(Me.bexclude)
        Me.uxLinkOps.Controls.Add(Me.cstart)
        Me.uxLinkOps.Controls.Add(Me.Label7)
        Me.uxLinkOps.Controls.Add(Me.Label6)
        Me.uxLinkOps.Controls.Add(Me.Label5)
        Me.uxLinkOps.Controls.Add(Me.cblanks)
        Me.uxLinkOps.Controls.Add(Me.cshowerrors)
        Me.uxLinkOps.Controls.Add(Me.tsheets)
        Me.uxLinkOps.Controls.Add(Me.taddsheet)
        Me.uxLinkOps.Controls.Add(Me.bdelsheet)
        Me.uxLinkOps.Controls.Add(Me.baddsheet)
        Me.uxLinkOps.Controls.Add(Me.lsheets)
        Me.uxLinkOps.Controls.Add(Me.rinclude)
        Me.uxLinkOps.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxLinkOps.Location = New System.Drawing.Point(8, 226)
        Me.uxLinkOps.Name = "uxLinkOps"
        Me.uxLinkOps.Size = New System.Drawing.Size(845, 174)
        Me.uxLinkOps.TabIndex = 1
        Me.uxLinkOps.TabStop = False
        Me.uxLinkOps.Text = "General"
        '
        'Label7
        '
        Me.Label7.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(373, 23)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(240, 16)
        Me.Label7.TabIndex = 5
        Me.Label7.Text = "Excel Functions Number per batch"
        '
        'Label6
        '
        Me.Label6.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(22, 76)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(112, 16)
        Me.Label6.TabIndex = 3
        Me.Label6.Text = "Stop Linking after"
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(22, 49)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(208, 16)
        Me.Label5.TabIndex = 1
        Me.Label5.Text = "Start Linking from header column"
        '
        'tsheets
        '
        Me.tsheets.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tsheets.Location = New System.Drawing.Point(328, 92)
        Me.tsheets.Name = "tsheets"
        Me.tsheets.Size = New System.Drawing.Size(280, 16)
        Me.tsheets.TabIndex = 9
        Me.tsheets.Text = "Search all sheets excluding these when linking"
        '
        'uxLinkCodes
        '
        Me.uxLinkCodes.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxLinkCodes.Controls.Add(Me.Button2)
        Me.uxLinkCodes.Controls.Add(Me.tlabcol)
        Me.uxLinkCodes.Controls.Add(Me.tstcol)
        Me.uxLinkCodes.Controls.Add(Me.Label11)
        Me.uxLinkCodes.Controls.Add(Me.Label10)
        Me.uxLinkCodes.Controls.Add(Me.teprefix)
        Me.uxLinkCodes.Controls.Add(Me.taprefix)
        Me.uxLinkCodes.Controls.Add(Me.Label9)
        Me.uxLinkCodes.Controls.Add(Me.Label8)
        Me.uxLinkCodes.Controls.Add(Me.tacccode)
        Me.uxLinkCodes.Controls.Add(Me.teacode)
        Me.uxLinkCodes.Controls.Add(Me.tperiodcode)
        Me.uxLinkCodes.Controls.Add(Me.theadercode)
        Me.uxLinkCodes.Controls.Add(Me.Label4)
        Me.uxLinkCodes.Controls.Add(Me.Label3)
        Me.uxLinkCodes.Controls.Add(Me.Label2)
        Me.uxLinkCodes.Controls.Add(Me.Label1)
        Me.uxLinkCodes.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxLinkCodes.Location = New System.Drawing.Point(8, 412)
        Me.uxLinkCodes.Name = "uxLinkCodes"
        Me.uxLinkCodes.Size = New System.Drawing.Size(845, 160)
        Me.uxLinkCodes.TabIndex = 2
        Me.uxLinkCodes.TabStop = False
        Me.uxLinkCodes.Text = "Link Codes"
        '
        'Label11
        '
        Me.Label11.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(334, 124)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(160, 32)
        Me.Label11.TabIndex = 14
        Me.Label11.Text = "Label Column Code  (flexible labels)"
        '
        'Label10
        '
        Me.Label10.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(332, 92)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(152, 32)
        Me.Label10.TabIndex = 12
        Me.Label10.Text = "Static Data Column Code"
        '
        'Label9
        '
        Me.Label9.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(342, 27)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(80, 16)
        Me.Label9.TabIndex = 10
        Me.Label9.Text = "Actual Suffix"
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(20, 126)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(168, 26)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Accounting Header Code (optional)"
        Me.Label4.Visible = False
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(20, 62)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(184, 26)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Estimate/Actual Header Code"
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(20, 93)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(118, 24)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Period Header Code"
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(20, 30)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(120, 16)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Year Header Code"
        '
        'uxMapCodes
        '
        Me.uxMapCodes.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxMapCodes.Controls.Add(Me.Label14)
        Me.uxMapCodes.Controls.Add(Me.Label13)
        Me.uxMapCodes.Controls.Add(Me.Label12)
        Me.uxMapCodes.Controls.Add(Me.lacccodes)
        Me.uxMapCodes.Controls.Add(Me.racc)
        Me.uxMapCodes.Controls.Add(Me.rperiod)
        Me.uxMapCodes.Controls.Add(Me.lcodes)
        Me.uxMapCodes.Controls.Add(Me.bdelcode)
        Me.uxMapCodes.Controls.Add(Me.baddcode)
        Me.uxMapCodes.Controls.Add(Me.textcode)
        Me.uxMapCodes.Controls.Add(Me.cbccodes)
        Me.uxMapCodes.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxMapCodes.Location = New System.Drawing.Point(8, 576)
        Me.uxMapCodes.Name = "uxMapCodes"
        Me.uxMapCodes.Size = New System.Drawing.Size(845, 136)
        Me.uxMapCodes.TabIndex = 3
        Me.uxMapCodes.TabStop = False
        Me.uxMapCodes.Text = "Mapping Codes"
        '
        'Label14
        '
        Me.Label14.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(135, 77)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(57, 16)
        Me.Label14.TabIndex = 4
        Me.Label14.Text = "maps to"
        '
        'Label13
        '
        Me.Label13.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(122, 99)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(56, 16)
        Me.Label13.TabIndex = 5
        Me.Label13.Text = "BC Code"
        '
        'Label12
        '
        Me.Label12.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(120, 56)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(72, 16)
        Me.Label12.TabIndex = 2
        Me.Label12.Text = "Client Code"
        '
        'uxAggAudit
        '
        Me.uxAggAudit.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxAggAudit.Controls.Add(Me.uxExport)
        Me.uxAggAudit.Controls.Add(Me.uxRefresh)
        Me.uxAggAudit.Controls.Add(Me.uxDetails)
        Me.uxAggAudit.Controls.Add(Me.uxAggUniverse)
        Me.uxAggAudit.Controls.Add(Me.uxAggDate)
        Me.uxAggAudit.Controls.Add(Me.uxlUniverse)
        Me.uxAggAudit.Controls.Add(Me.uxldate)
        Me.uxAggAudit.Controls.Add(Me.uxlFilter)
        Me.uxAggAudit.Controls.Add(Me.uxListAggAudit)
        Me.uxAggAudit.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxAggAudit.ForeColor = System.Drawing.SystemColors.WindowText
        Me.uxAggAudit.Location = New System.Drawing.Point(8, 718)
        Me.uxAggAudit.Name = "uxAggAudit"
        Me.uxAggAudit.Size = New System.Drawing.Size(845, 287)
        Me.uxAggAudit.TabIndex = 4
        Me.uxAggAudit.TabStop = False
        Me.uxAggAudit.Text = "Aggregation Audit Information"
        '
        'uxExport
        '
        Me.uxExport.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxExport.Location = New System.Drawing.Point(167, 55)
        Me.uxExport.Name = "uxExport"
        Me.uxExport.Size = New System.Drawing.Size(75, 23)
        Me.uxExport.TabIndex = 8
        Me.uxExport.Text = "Export"
        Me.uxExport.UseVisualStyleBackColor = True
        '
        'uxRefresh
        '
        Me.uxRefresh.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxRefresh.Location = New System.Drawing.Point(7, 55)
        Me.uxRefresh.Name = "uxRefresh"
        Me.uxRefresh.Size = New System.Drawing.Size(75, 23)
        Me.uxRefresh.TabIndex = 7
        Me.uxRefresh.Text = "Refresh"
        Me.uxRefresh.UseVisualStyleBackColor = True
        '
        'uxDetails
        '
        Me.uxDetails.Enabled = False
        Me.uxDetails.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxDetails.Location = New System.Drawing.Point(86, 55)
        Me.uxDetails.Name = "uxDetails"
        Me.uxDetails.Size = New System.Drawing.Size(75, 23)
        Me.uxDetails.TabIndex = 6
        Me.uxDetails.Text = "Details"
        Me.uxDetails.UseVisualStyleBackColor = True
        '
        'uxAggUniverse
        '
        Me.uxAggUniverse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.uxAggUniverse.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxAggUniverse.FormattingEnabled = True
        Me.uxAggUniverse.Location = New System.Drawing.Point(322, 28)
        Me.uxAggUniverse.Name = "uxAggUniverse"
        Me.uxAggUniverse.Size = New System.Drawing.Size(172, 21)
        Me.uxAggUniverse.TabIndex = 5
        '
        'uxAggDate
        '
        Me.uxAggDate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.uxAggDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxAggDate.FormattingEnabled = True
        Me.uxAggDate.Location = New System.Drawing.Point(115, 28)
        Me.uxAggDate.Name = "uxAggDate"
        Me.uxAggDate.Size = New System.Drawing.Size(127, 21)
        Me.uxAggDate.TabIndex = 4
        '
        'uxlUniverse
        '
        Me.uxlUniverse.AutoSize = True
        Me.uxlUniverse.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxlUniverse.Location = New System.Drawing.Point(258, 31)
        Me.uxlUniverse.Name = "uxlUniverse"
        Me.uxlUniverse.Size = New System.Drawing.Size(62, 13)
        Me.uxlUniverse.TabIndex = 3
        Me.uxlUniverse.Text = "Universe:"
        '
        'uxldate
        '
        Me.uxldate.AutoSize = True
        Me.uxldate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxldate.Location = New System.Drawing.Point(70, 31)
        Me.uxldate.Name = "uxldate"
        Me.uxldate.Size = New System.Drawing.Size(39, 13)
        Me.uxldate.TabIndex = 2
        Me.uxldate.Text = "Date:"
        '
        'uxlFilter
        '
        Me.uxlFilter.AutoSize = True
        Me.uxlFilter.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxlFilter.Location = New System.Drawing.Point(5, 31)
        Me.uxlFilter.Name = "uxlFilter"
        Me.uxlFilter.Size = New System.Drawing.Size(59, 13)
        Me.uxlFilter.TabIndex = 1
        Me.uxlFilter.Text = "Filter By:"
        '
        'uxListAggAudit
        '
        Me.uxListAggAudit.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxListAggAudit.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.universe, Me.ColumnHeader13, Me.ColumnHeader14, Me.ColumnHeader15, Me.ColumnHeader16, Me.ColumnHeader17})
        Me.uxListAggAudit.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxListAggAudit.FullRowSelect = True
        Me.uxListAggAudit.HideSelection = False
        Me.uxListAggAudit.Location = New System.Drawing.Point(6, 84)
        Me.uxListAggAudit.MultiSelect = False
        Me.uxListAggAudit.Name = "uxListAggAudit"
        Me.uxListAggAudit.Size = New System.Drawing.Size(833, 197)
        Me.uxListAggAudit.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.uxListAggAudit.TabIndex = 0
        Me.uxListAggAudit.UseCompatibleStateImageBehavior = False
        Me.uxListAggAudit.View = System.Windows.Forms.View.Details
        '
        'universe
        '
        Me.universe.Text = "Universe"
        Me.universe.Width = 185
        '
        'ColumnHeader13
        '
        Me.ColumnHeader13.Text = "Start Date/Time"
        Me.ColumnHeader13.Width = 140
        '
        'ColumnHeader14
        '
        Me.ColumnHeader14.Text = "Finish Date/Time"
        Me.ColumnHeader14.Width = 140
        '
        'ColumnHeader15
        '
        Me.ColumnHeader15.Text = "Time Elapsed"
        Me.ColumnHeader15.Width = 88
        '
        'ColumnHeader16
        '
        Me.ColumnHeader16.Text = "No. of Calcs."
        Me.ColumnHeader16.Width = 87
        '
        'ColumnHeader17
        '
        Me.ColumnHeader17.Text = "Result"
        Me.ColumnHeader17.Width = 170
        '
        'Label16
        '
        Me.Label16.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.Location = New System.Drawing.Point(373, 50)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(240, 16)
        Me.Label16.TabIndex = 13
        Me.Label16.Text = "Excel Functions Update Lists"
        '
        'Crefreshlists
        '
        Me.Crefreshlists.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Crefreshlists.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Crefreshlists.Location = New System.Drawing.Point(580, 50)
        Me.Crefreshlists.Name = "Crefreshlists"
        Me.Crefreshlists.Size = New System.Drawing.Size(56, 21)
        Me.Crefreshlists.TabIndex = 14
        Me.ToolTip1.SetToolTip(Me.Crefreshlists, "Specifies how many columns to check that are blank before the linking determines " & _
                "there are no more headers to link. This allows blank columns in the client works" & _
                "heets.")
        '
        'Tbatch
        '
        Me.Tbatch.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Tbatch.Location = New System.Drawing.Point(580, 20)
        Me.Tbatch.Name = "Tbatch"
        Me.Tbatch.Size = New System.Drawing.Size(73, 21)
        Me.Tbatch.TabIndex = 15
        Me.ToolTip1.SetToolTip(Me.Tbatch, "Enter here name of worksheet to include or exclude depending upon which option is" & _
                " checked.")
        '
        'bc_am_in_settings
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(861, 1017)
        Me.ControlBox = False
        Me.Controls.Add(Me.uxAggAudit)
        Me.Controls.Add(Me.uxMapCodes)
        Me.Controls.Add(Me.uxLinkCodes)
        Me.Controls.Add(Me.uxLinkOps)
        Me.Controls.Add(Me.uxGeneral)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "bc_am_in_settings"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.uxGeneral.ResumeLayout(False)
        Me.uxGeneral.PerformLayout()
        Me.uxLinkOps.ResumeLayout(False)
        Me.uxLinkOps.PerformLayout()
        Me.uxLinkCodes.ResumeLayout(False)
        Me.uxLinkCodes.PerformLayout()
        Me.uxMapCodes.ResumeLayout(False)
        Me.uxMapCodes.PerformLayout()
        Me.uxAggAudit.ResumeLayout(False)
        Me.uxAggAudit.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region
    Public oexcel As Object
    Public tk_main As Object
    Public template As String
    Dim otemplate As Byte()
    Dim from_load As Boolean = False
    Dim odir As New FolderBrowserDialog
    Public oparent As bc_am_in_context
    Private Sub ccontrol_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ccontrol.CheckedChanged
        Me.clocalcopy.Visible = False
        If Me.ccontrol.Checked = True Then
            Me.clocalcopy.Visible = True
            Me.tdir.Visible = False
            Me.tdir.Text = ""
            Me.clocalcopy.Checked = False
        Else
            Me.tdir.Visible = False
            Me.tdir.Text = ""
            Me.clocalcopy.Checked = False
            Me.clocalcopy.Visible = False
        End If

        set_changes()
    End Sub
    Public Sub delete_sheet()
        Dim omsg As New bc_cs_message
        omsg = New bc_cs_message("Blue Curve", "Are you sure you wish to delete " + Me.tformatsheet.Text + " proceed?", bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
        If omsg.cancel_selected = True Then
            Exit Sub
        End If
        set_changes()
        Me.tformatsheet.Visible = True
        Me.tformatsheet.Text = "Format Sheet: none"
        Me.template = ""
        tk_main.set_button_disabled(4)
        tk_main.set_button_disabled(5)
    End Sub
    Public Sub create_xml_file(Optional ByVal show_message As Boolean = True, Optional ByVal upload_to_server As Boolean = True)
        Dim sxml As String
        Dim ixml As String
        Dim fs As New bc_cs_file_transfer_services
        Dim i As Integer
        sxml = "<insight_settings>" + vbCrLf

        sxml = sxml + "<excel_functions>" + vbCrLf

        sxml = sxml + "<number_per_batch>" + Me.Tbatch.Text + "</number_per_batch>" + vbCrLf
        sxml = sxml + "<update_lists>" + CStr(Me.Crefreshlists.SelectedIndex) + "</update_lists>" + vbCrLf
        sxml = sxml + "</excel_functions>" + vbCrLf

        sxml = sxml + "<hide_sheets>"
        If Me.chide.Checked = True Then
            sxml = sxml + "1"
        Else
            sxml = sxml + "0"
        End If
        sxml = sxml + "</hide_sheets>" + vbCrLf
        sxml = sxml + "<show_all_entities>"
        If Me.callent.Checked = True Then
            sxml = sxml + "true"
        Else
            sxml = sxml + "false"
        End If
        sxml = sxml + "</show_all_entities>" + vbCrLf
        sxml = sxml + "<my_entities_default>"
        If Me.centdef.Checked = True Then
            sxml = sxml + "true"
        Else
            sxml = sxml + "false"
        End If
        sxml = sxml + "</my_entities_default>" + vbCrLf
        sxml = sxml + "<controlled_submission>"
        If Me.ccontrol.Checked = True Then
            sxml = sxml + "1"
        Else
            sxml = sxml + "0"
        End If
        sxml = sxml + "</controlled_submission>" + vbCrLf
        sxml = sxml + "<linked_header>"
        REM always have a linkd header
        'If Me.clinkhead.Checked = True Then
        sxml = sxml + "1"
        'Else
        '   sxml = sxml + "0"
        'End If
        sxml = sxml + "</linked_header>" + vbCrLf

        sxml = sxml + "<local_copy>" + vbCrLf
        If Me.clocalcopy.Checked = True Then
            sxml = sxml + "<enabled>1</enabled>" + vbCrLf + "<dir>" + odir.SelectedPath + "\</dir>" + vbCrLf
        Else
            sxml = sxml + "<enabled>0</enabled>" + vbCrLf + "<dir></dir>" + vbCrLf
        End If
        sxml = sxml + "</local_copy>" + vbCrLf

        sxml = sxml + "<show_header_errors_in_cell>"
        If Me.cshowerrors.Checked = True Then
            sxml = sxml + "1"
        Else
            sxml = sxml + "0"
        End If
        sxml = sxml + "</show_header_errors_in_cell>" + vbCrLf

        sxml = sxml + "<period_headers>" + vbCrLf
        sxml = sxml + "<year>" + Me.theadercode.Text + "</year>" + vbCrLf
        sxml = sxml + "<period>" + Me.tperiodcode.Text + "</period>" + vbCrLf
        sxml = sxml + "<e_a>" + Me.teacode.Text + "</e_a>" + vbCrLf
        sxml = sxml + "<accounting>" + Me.tacccode.Text + "</accounting>" + vbCrLf
        sxml = sxml + "<period_codes>" + vbCrLf
        For i = 0 To Me.lcodes.Items.Count - 1
            sxml = sxml + "<ext_code>" + Me.lcodes.Items(i).SubItems(1).Text + "</ext_code>" + vbCrLf
            sxml = sxml + "<bc_code>" + Me.lcodes.Items(i).SubItems(0).Text + "</bc_code>" + vbCrLf
        Next
        sxml = sxml + "</period_codes>" + vbCrLf
        Dim tstart As Integer
        Dim tblanks As Integer
        If Me.cstart.SelectedIndex = -1 Then
            tstart = 3
        Else
            tstart = Me.cstart.Text
        End If
        sxml = sxml + "<start_column>" + CStr(tstart) + "</start_column>" + vbCrLf
        If Me.cblanks.Text = "" Then
            tblanks = "20"
        Else
            tblanks = Me.cblanks.Text
        End If
        sxml = sxml + "<concurrent_blanks>" + CStr(tblanks) + "</concurrent_blanks>" + vbCrLf
        sxml = sxml + "<accounting_codes>" + vbCrLf
        For i = 0 To Me.lacccodes.Items.Count - 1
            sxml = sxml + "<ext_code>" + Me.lacccodes.Items(i).SubItems(1).Text + "</ext_code>" + vbCrLf
            sxml = sxml + "<bc_code>" + Me.lacccodes.Items(i).SubItems(0).Text + "</bc_code>" + vbCrLf
        Next
        sxml = sxml + "</accounting_codes>" + vbCrLf
        sxml = sxml + "<actual_prefix>" + Me.taprefix.Text + "</actual_prefix>" + vbCrLf
        sxml = sxml + "<estimate_prefix>" + Me.teprefix.Text + "</estimate_prefix>" + vbCrLf
        sxml = sxml + "</period_headers>" + vbCrLf
        sxml = sxml + "<static_headers>" + vbCrLf
        sxml = sxml + "<header>" + Me.tstcol.Text + "</header>" + vbCrLf
        sxml = sxml + "</static_headers>" + vbCrLf
        sxml = sxml + "<label_headers>" + vbCrLf
        sxml = sxml + "<header>" + Me.tlabcol.Text + "</header>" + vbCrLf
        sxml = sxml + "</label_headers>" + vbCrLf
        sxml = sxml + "<use_include_sheets>"
        If bexclude.Checked = True Then
            sxml = sxml + "false"
        Else
            sxml = sxml + "true"
        End If
        sxml = sxml + "</use_include_sheets>" + vbCrLf
        sxml = sxml + "<exclude_sheets>" + vbCrLf
        If bexclude.Checked = True Then
            For i = 0 To Me.lsheets.Items.Count - 1
                sxml = sxml + "<sheet>" + Me.lsheets.Items(i) + "</sheet>" + vbCrLf
            Next
        End If
        sxml = sxml + "</exclude_sheets>" + vbCrLf
        sxml = sxml + "<include_sheets>" + vbCrLf
        If bexclude.Checked = False Then
            For i = 0 To Me.lsheets.Items.Count - 1
                sxml = sxml + "<sheet>" + Me.lsheets.Items(i) + "</sheet>" + vbCrLf
            Next
        End If
        sxml = sxml + "</include_sheets>" + vbCrLf

        sxml = sxml + "<format>" + vbCrLf
        If Me.tformatsheet.Text <> "Format Sheet: none" Then
            sxml = sxml + "<template>" + Me.tformatsheet.Text.Substring(14, Me.tformatsheet.Text.Length - 14) + "</template>" + vbCrLf
        Else
            sxml = sxml + "<template></template>" + vbCrLf
        End If
        REM format codes
        sxml = sxml + "<section>" + vbCrLf
        sxml = sxml + "<style>bc_section</style>" + vbCrLf
        sxml = sxml + "</section>" + vbCrLf

        sxml = sxml + "<row>" + vbCrLf
        sxml = sxml + "<style>bc_row</style>" + vbCrLf
        sxml = sxml + "</row>" + vbCrLf

        sxml = sxml + "<flexible_label>" + vbCrLf
        sxml = sxml + "<style>bc_flexible</style>" + vbCrLf
        sxml = sxml + "</flexible_label>" + vbCrLf

        sxml = sxml + "<header_value>" + vbCrLf
        sxml = sxml + "<style>bc_header_value</style>" + vbCrLf
        sxml = sxml + "</header_value>" + vbCrLf

        sxml = sxml + "<header_name>" + vbCrLf
        sxml = sxml + "<style>bc_header_name</style>" + vbCrLf
        sxml = sxml + "</header_name>" + vbCrLf


        sxml = sxml + "<value>" + vbCrLf
        sxml = sxml + "<style>bc_value</style>" + vbCrLf
        sxml = sxml + "</value>" + vbCrLf

        sxml = sxml + "</format>" + vbCrLf

        fs = New bc_cs_file_transfer_services
        If fs.check_document_exists(bc_cs_central_settings.local_template_path + "bc_am_insight_config.xml") Then
            Dim ifs As New StreamReader(bc_cs_central_settings.local_template_path + "bc_am_insight_config.xml", False)
            ixml = ifs.ReadToEnd
            ifs.Close()
            REM strip out existing retrueve settings bit
            i = InStr(ixml, "<retrieve_values>", CompareMethod.Text)
            If i > 0 Then
                ixml = ixml.Substring(i - 1, ixml.Length - i + 1)
                sxml = sxml + vbCrLf + ixml + vbCrLf
            Else
                sxml = sxml + vbCrLf + "</insight_settings>"
            End If
        Else
            sxml = sxml + vbCrLf + "</insight_settings>"
        End If
        REM now write out file overwritting previous copy
        Dim sfs As New StreamWriter(bc_cs_central_settings.local_template_path + "bc_am_insight_config.xml", False)
        sfs.Write(sxml)
        sfs.Close()
        If upload_to_server = True Then
            fs = New bc_cs_file_transfer_services
            Dim oinsight_format As New bc_om_insight_config_files
            Me.template = Me.tformatsheet.Text.Substring(14, Me.tformatsheet.Text.Length - 14)
            fs.write_document_to_bytestream(bc_cs_central_settings.local_template_path + "bc_am_insight_config.xml", oinsight_format.xml_file, Nothing)
            If Me.template <> "" Then
                If fs.check_document_exists(bc_cs_central_settings.local_template_path + Me.template) Then
                    fs.write_document_to_bytestream(bc_cs_central_settings.local_template_path + Me.template, oinsight_format.format_file, Nothing)
                    oinsight_format.format_file_name = Me.template
                Else
                    oinsight_format.format_file_name = ""
                End If
            Else
                oinsight_format.format_file_name = ""
            End If

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                oinsight_format.db_write()
                If show_message = True Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Upload Complete", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                End If
            Else
                oinsight_format.no_send_back = True
                oinsight_format.tmode = bc_cs_soap_base_class.tWRITE
                If oinsight_format.transmit_to_server_and_receive(oinsight_format, True) = True Then
                    If show_message = True Then
                        Dim omsg As New bc_cs_message("Blue Curve", "Upload Complete", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    End If
                End If
            End If
        End If
    End Sub
    Private Sub clocalcopy_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles clocalcopy.CheckedChanged
        set_changes()
        Me.tdir.Visible = False
        If Me.clocalcopy.Checked = True Then
            If from_load = False Then
                odir = New FolderBrowserDialog
                odir.ShowDialog()
                If odir.SelectedPath <> "" Then
                    Me.tdir.Visible = True
                    Me.tdir.Text = odir.SelectedPath
                Else
                    Me.clocalcopy.Checked = False
                    Me.tdir.Visible = False
                End If
            Else
                Me.tdir.Visible = True
            End If
        End If
    End Sub
    Private Sub taddsheet_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles taddsheet.MouseLeave
        Dim i As Integer
        Me.baddsheet.Enabled = False
        For i = 0 To Me.lsheets.Items.Count - 1
            If UCase(Me.lsheets.Items(i)) = UCase(Me.taddsheet.Text) Then
                Me.baddsheet.Enabled = False
                Dim omsg As New bc_cs_message("Blue Curve", "Sheet name already exists!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Sub
            End If
        Next
        If Trim(Me.taddsheet.Text) <> "" Then
            Me.baddsheet.Enabled = True
        End If
    End Sub
    Private Sub baddsheet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles baddsheet.Click
        If Me.taddsheet.Text <> "" Then
            set_changes()
            Me.lsheets.Items.Add(Me.taddsheet.Text)
            Me.baddsheet.Enabled = False
            Me.taddsheet.Text = ""
            set_changes()
        End If
    End Sub
    Private Sub lsheets_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lsheets.SelectedIndexChanged
        Me.bdelsheet.Enabled = True
    End Sub
    Private Sub bdelsheet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bdelsheet.Click
        If Me.lsheets.SelectedIndex > -1 Then
            set_changes()
            Me.lsheets.Items.RemoveAt(Me.lsheets.SelectedIndex)
            Me.bdelsheet.Enabled = False
            set_changes()
        End If
    End Sub

    Private Sub bexclude_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bexclude.CheckedChanged

        If bexclude.Checked = True Then
            Me.tsheets.Text = "Search all sheets excluding these when linking"
            Me.tsheets.Refresh()
            set_changes()
        End If

    End Sub

    Private Sub rinclude_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rinclude.CheckedChanged
        If rinclude.Checked = True Then
            Me.tsheets.Text = "Only look in these sheets when linking"
            Me.tsheets.Refresh()
            set_changes()
        End If
    End Sub

    Public Sub load_settings()
        Dim i As Integer
        Try
            set_lists()
            from_load = True
            REM loads settings from config file
            Me.chide.Checked = False
            If bc_am_insight_formats.hide_Sheets = "1" Then
                Me.chide.Checked = True
            End If
            Me.ccontrol.Checked = False
            If bc_am_insight_formats.controlled_Submission = "1" Then
                Me.ccontrol.Checked = True
            End If
            Me.tformatsheet.Visible = True
            If bc_am_insight_formats.format_filename <> "" Then
                Me.tformatsheet.Text = "Format Sheet: " + bc_am_insight_formats.format_filename
            Else
                Me.tformatsheet.Text = "Format Sheet: none"
            End If
            Me.callent.Checked = False
            Me.centdef.Checked = False
            Me.centdef.Visible = False
            If bc_am_insight_link_names.show_all_entities = True Then
                Me.callent.Checked = True
                Me.centdef.Visible = True
                Me.centdef.Checked = False
            End If
            If bc_am_insight_link_names.my_entities_default = True Then
                Me.centdef.Checked = True
            End If

            Me.bexclude.Checked = True
            Me.rinclude.Checked = False
            Me.lsheets.Items.Clear()
            If bc_am_insight_link_names.use_include_sheets = True Then
                Me.rinclude.Checked = True
                For i = 0 To bc_am_insight_link_names.include_sheets.Count - 1
                    Me.lsheets.Items.Add(bc_am_insight_link_names.include_sheets(i))
                Next
            Else
                For i = 0 To bc_am_insight_link_names.exclude_sheets.Count - 1
                    Me.lsheets.Items.Add(bc_am_insight_link_names.exclude_sheets(i))
                Next
            End If
            Me.cshowerrors.Checked = False
            REM
            If bc_am_insight_link_names.show_header_errors_in_cell = True Then
                Me.cshowerrors.Checked = True
            End If

            Me.taprefix.Text = bc_am_insight_link_names.actual_prefix
            Me.teprefix.Text = bc_am_insight_link_names.estimate_prefix
            Me.theadercode.Text = bc_am_insight_link_names.year_head
            Me.tperiodcode.Text = bc_am_insight_link_names.period_head
            Me.teacode.Text = bc_am_insight_link_names.e_a_head
            Me.tacccode.Text = bc_am_insight_link_names.accounting_head
            Me.tstcol.Text = bc_am_insight_link_names.static_head
            Me.tlabcol.Text = bc_am_insight_link_names.label_head
            Me.cstart.SelectedIndex = CInt(bc_am_insight_link_names.start_column) - 1
            Me.cblanks.SelectedIndex = CInt(bc_am_insight_link_names.concurrent_blanks) - 1

            Me.Tbatch.Text = bc_am_insight_formats.number_per_batch
            Me.Crefreshlists.Items.Clear()
            Me.Crefreshlists.Items.Add("No")
            Me.Crefreshlists.Items.Add("Yes")

            If bc_am_insight_formats.update_lists = "1" Then
                Me.Crefreshlists.SelectedIndex = 1
            Else
                Me.Crefreshlists.SelectedIndex = 0
            End If



            Me.clocalcopy.Checked = False
            If bc_am_insight_formats.local_Save_Enabled = "1" Then
                Me.clocalcopy.Checked = True
                Me.tdir.Text = bc_am_insight_formats.local_Save_Dir
            End If
            Me.lcodes.Items.Clear()
            For i = 0 To bc_am_insight_link_names.period_values.Count - 1
                Dim lvw As New ListViewItem(CStr(bc_am_insight_link_names.period_values(i).bc_code))
                lvw.SubItems.Add(bc_am_insight_link_names.period_values(i).external_code)
                Me.lcodes.Items.Add(lvw)
            Next

        Catch ex As Exception
            Dim err As New bc_cs_error_log("bc_am_in_Settings", "load_settings", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.uxMapCodes.Visible = False
            Me.uxGeneral.Visible = True
            Me.uxLinkOps.Visible = False
            Me.uxLinkCodes.Visible = False
            from_load = False
        End Try

    End Sub

    Public Sub set_lists()
        Dim i As Integer
        Me.cstart.Items.Clear()
        Me.cblanks.Items.Clear()
        For i = 0 To 100
            Me.cstart.Items.Add(CStr(i) + 1)
            Me.cblanks.Items.Add(CStr(i) + 1)
        Next
        Me.populate_period_codes()
    End Sub
    Private Sub populate_period_codes()
        Dim i As Integer
        Me.cbccodes.Items.Clear()
        For i = 0 To bc_am_in_context.insight_items.result_types.bc_om_result_types.Count - 1
            Me.cbccodes.Items.Add(bc_am_in_context.insight_items.result_types.bc_om_result_types(i).description)
        Next
    End Sub
    Private Sub populate_acc_codes()
        Dim i As Integer
        Me.cbccodes.Items.Clear()
        For i = 0 To bc_am_in_context.insight_items.acc_standards.bc_om_accounting_standards.Count - 1
            Me.cbccodes.Items.Add(bc_am_in_context.insight_items.acc_standards.bc_om_accounting_standards(i).name)
        Next
    End Sub
    Private Sub rperiod_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rperiod.CheckedChanged
        If rperiod.Checked = True Then
            Me.populate_period_codes()
            Me.lcodes.Visible = True
            Me.lacccodes.Visible = False
            Me.bdelcode.Enabled = False
        End If
    End Sub
    Private Sub racc_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles racc.CheckedChanged
        If racc.Checked = True Then
            Me.populate_acc_codes()
            Me.lcodes.Visible = False
            Me.lacccodes.Visible = True
            Me.bdelcode.Enabled = False
        End If
    End Sub
    Private Sub textcode_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles textcode.TextChanged
        Me.baddcode.Enabled = False
        If Me.cbccodes.SelectedIndex > -1 Then
            If check_duplicate() = False Then
                Me.baddcode.Enabled = True
            End If
        End If
    End Sub
    Private Sub cbccodes_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbccodes.SelectedIndexChanged
        Me.baddcode.Enabled = False
        If Me.textcode.Text <> "" Then
            If check_duplicate() = False Then
                Me.baddcode.Enabled = True
            End If
        End If
    End Sub
    Private Function check_duplicate() As Boolean
        Try
            Dim i As Integer
            check_duplicate = False
            If Trim(Me.textcode.Text) = "" Then
                Exit Function
            End If
            If rperiod.Checked = True Then
                For i = 0 To Me.lcodes.Items.Count - 1
                    If UCase(Me.lcodes.Items(i).Text) = UCase(Me.cbccodes.Text) And UCase(Me.lcodes.Items(i).SubItems(1).Text) = UCase(Me.textcode.Text) Then
                        check_duplicate = True
                        Exit Function
                    End If
                Next
            Else
                For i = 0 To Me.lacccodes.Items.Count - 1
                    If Me.lacccodes.Items(i).Text = Me.cbccodes.Text And Me.lacccodes.Items(i).SubItems(1).Text = Me.textcode.Text Then
                        check_duplicate = True
                        Exit Function
                    End If
                Next
            End If
        Catch
        Finally
            If check_duplicate = True Then
                Dim omsg As New bc_cs_message("Blue Curve", "Code combination already exists!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            End If
        End Try
    End Function

    Private Sub baddcode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles baddcode.Click
        Dim lvew As New ListViewItem(Me.cbccodes.Text)
        set_changes()
        lvew.SubItems.Add(Me.textcode.Text)
        If Me.racc.Checked = False Then
            Me.lcodes.Items.Add(lvew)
        Else
            Me.lacccodes.Items.Add(lvew)
        End If
        Me.textcode.Text = ""
        Me.cbccodes.SelectedIndex = -1
        Me.baddcode.Enabled = False
    End Sub
    Private Sub bdelcode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bdelcode.Click
        set_changes()
        If Me.racc.Checked = False Then
            Me.lcodes.Items.RemoveAt(Me.lcodes.SelectedItems(0).Index)
        Else
            Me.lacccodes.Items.RemoveAt(Me.lacccodes.SelectedItems(0).Index)
        End If
        Me.bdelcode.Enabled = False
    End Sub

    Private Sub lcodes_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lcodes.SelectedIndexChanged
        Me.bdelcode.Enabled = False
        If Me.lcodes.SelectedItems.Count = 0 Then
            Exit Sub
        End If
        Me.bdelcode.Enabled = True
    End Sub

    Private Sub lacccodes_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lacccodes.SelectedIndexChanged
        Me.bdelcode.Enabled = False
        If Me.lacccodes.SelectedItems.Count = 0 Then
            Exit Sub
        End If
        Me.bdelcode.Enabled = True
    End Sub
    Public Sub set_changes()
        If Me.from_load = True Then
            Exit Sub
        End If
        Me.tk_main.set_change(1)
    End Sub


    Private Sub chide_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chide.CheckedChanged
        set_changes()
    End Sub

    Private Sub cstart_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        set_changes()
    End Sub

    Private Sub cblanks_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        set_changes()
    End Sub

    Private Sub theadercode_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        set_changes()
    End Sub

    Private Sub tperiodcode_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        set_changes()
    End Sub

    Private Sub teacode_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        set_changes()
    End Sub

    Private Sub tacccode_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        set_changes()
    End Sub

    Private Sub taprefix_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        set_changes()
    End Sub

    Private Sub teprefix_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        set_changes()
    End Sub

    Private Sub tstcol_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        set_changes()
    End Sub

    Private Sub tlabcol_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        set_changes()
    End Sub

    Public Sub edit_sheet()
        Me.Cursor = Cursors.WaitCursor
        Dim xl As Object = Nothing

        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try
            tk_main.set_status("Opening sheet in excel for edit...")
            REM previews current in memory settings
            Dim omsg As bc_cs_message
            Try
                xl = GetObject(, "excel.application")
            Catch
                Try
                    xl = CreateObject("excel.application")
                    xl.application.visible = False
                Catch
                    omsg = New bc_cs_message("Blue Curve", "Cannot invoke Excel", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub
                End Try
            End Try
            If xl.application.workbooks.count = 0 Then
                xl.application.workbooks.add()
            End If
            Dim excel_app As New bc_ao_in_excel(xl.application)
            excel_app.open(bc_cs_central_settings.local_template_path + Me.tformatsheet.Text.Substring(14, Me.tformatsheet.Text.Length - 14))
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Blue Curve", "bedit_click", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            tk_main.clear_status()
            Me.set_changes()
            xl.application.visible = True
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Public Sub import_sheet()
        Try
            Dim omsg As bc_cs_message
            If Me.tformatsheet.Text <> "Format Sheet: none" Then
                omsg = New bc_cs_message("Blue Curve", "Importing format sheet will overwrite reference to existing " + Me.tformatsheet.Text + " proceed?", bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
                If omsg.cancel_selected = True Then
                    Exit Sub
                End If
            End If

            Dim odialog As New OpenFileDialog
            odialog.Title = "Select format sheet"

            REM SW cope with office versions
            If bc_cs_central_settings.userOfficeStatus = 2 Then
                odialog.Filter = "Excel files (*.xls)|*.xls|(*.xlsx)|*.xlsx"
            Else
                odialog.Filter = "Excel files (*.xls)|*.xls"
            End If
            odialog.ShowDialog()

            If odialog.FileName <> "" Then
                Me.Cursor = Cursors.WaitCursor
                Me.tformatsheet.Visible = True
                Me.template = odialog.FileName.Substring(InStrRev(odialog.FileName, "\"))
                Dim fs As New bc_cs_file_transfer_services
                fs.write_document_to_bytestream(odialog.FileName, Me.otemplate, Nothing)
                REM write it back to controlled location
                fs.write_bytestream_to_document(bc_cs_central_settings.local_template_path + Me.template, Me.otemplate, Nothing)
                Me.tformatsheet.Text = "Format Sheet: " + Me.template
                Me.set_changes()
                tk_main.set_button_enabled(1)
                tk_main.set_button_enabled(3)
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_in_settings", "import_sheet", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    Private Sub bcreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim omsg As bc_cs_message
        If Me.tformatsheet.Text <> "" Then
            omsg = New bc_cs_message("Blue Curve", "Creating new format sheet will overwrite reference to existing  " + Me.tformatsheet.Text + " proceed?", bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
            If omsg.cancel_selected = True Then
                Exit Sub
            End If
        End If

    End Sub

    Public Sub load_user_security()
        Dim i, j As Integer
        Dim lvw As ListViewItem

        Try
            Me.Lsec.Items.Clear()
            For j = 0 To bc_am_in_context.insight_items.roles.Count - 1
                lvw = New ListViewItem(CStr(bc_am_in_context.insight_items.roles(j)), 0)
                lvw.SubItems.Add("Yes")
                lvw.SubItems.Add("Yes")
                lvw.SubItems.Add("Draft")
                Me.Lsec.Items.Add(lvw)
                Me.Lsec.Items(j).SubItems(1).Text = "No"
                Me.Lsec.Items(j).SubItems(2).Text = "No"

                For i = 0 To bc_am_in_context.insight_items.user_security.Count - 1
                    With bc_am_in_context.insight_items.user_security(i)
                        If .role_name = bc_am_in_context.insight_items.roles(j) Then
                            If .stage_name = "Draft" Then
                                Me.Lsec.Items(j).SubItems(1).Text = "Yes"
                            End If
                            If .stage_name = "Publish" Then
                                Me.Lsec.Items(j).SubItems(2).Text = "Yes"
                            End If
                            If .draft_default = False Then
                                Me.Lsec.Items(j).SubItems(3).Text = "Publish"
                            End If
                        End If
                    End With
                Next
            Next

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_in_settings", "load_user_security", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub

    Public Sub load_Aggregation_log(Optional ByVal refreshFilters As Boolean = False)
        Dim j As Integer
        Dim lvw As ListViewItem

        Dim showLog As Boolean

        Try

            If refreshFilters = True Then

                Me.uxAggDate.Items.Clear()
                Me.uxAggDate.Items.Add("None")
                For j = 0 To bc_am_in_context.insight_items.agg_audit_headers.Count - 1
                    If Me.uxAggDate.Items.Contains(Microsoft.VisualBasic.Left(bc_am_in_context.insight_items.agg_audit_headers(j).batch_date, 11)) = False Then
                        Me.uxAggDate.Items.Add(Microsoft.VisualBasic.Left(bc_am_in_context.insight_items.agg_audit_headers(j).batch_date, 11))
                    End If
                Next
                Me.uxAggDate.SelectedIndex = 0

                Me.uxAggUniverse.Items.Clear()
                Me.uxAggUniverse.Items.Add("None")
                For j = 0 To bc_am_in_context.insight_items.agg_audit_headers.Count - 1
                    If Me.uxAggUniverse.Items.Contains(bc_am_in_context.insight_items.agg_audit_headers(j).universe_name) = False Then
                        Me.uxAggUniverse.Items.Add(bc_am_in_context.insight_items.agg_audit_headers(j).universe_name)
                    End If
                Next
                Me.uxAggUniverse.SelectedIndex = 0

            End If

            uxDetails.Enabled = False
            Me.uxListAggAudit.Items.Clear()
            For j = 0 To bc_am_in_context.insight_items.agg_audit_headers.Count - 1

                REM Filter
                showLog = True
                If Me.uxAggDate.Text <> "None" Then
                    If Me.uxAggDate.Text <> Microsoft.VisualBasic.Left(bc_am_in_context.insight_items.agg_audit_headers(j).batch_date, 11) Then
                        showLog = False
                    End If
                End If
                If Me.uxAggUniverse.Text <> "None" Then
                    If Me.uxAggUniverse.Text <> CStr(bc_am_in_context.insight_items.agg_audit_headers(j).universe_name) Then
                        showLog = False
                    End If
                End If

                If showLog = True Then
                    lvw = New ListViewItem(CStr(bc_am_in_context.insight_items.agg_audit_headers(j).universe_name))
                    lvw.Tag = bc_am_in_context.insight_items.agg_audit_headers(j).batch_date
                    lvw.SubItems.Add(bc_am_in_context.insight_items.agg_audit_headers(j).start_date)
                    lvw.SubItems.Add(bc_am_in_context.insight_items.agg_audit_headers(j).finnish_date)
                    lvw.SubItems.Add(bc_am_in_context.insight_items.agg_audit_headers(j).elapsed10ths / 10)
                    lvw.SubItems.Add(bc_am_in_context.insight_items.agg_audit_headers(j).calculations)
                    If bc_am_in_context.insight_items.agg_audit_headers(j).warnings = 1 Then
                        lvw.SubItems.Add(bc_am_in_context.insight_items.agg_audit_headers(j).successtext + " with " + CStr(bc_am_in_context.insight_items.agg_audit_headers(j).warnings) + " warning")
                    ElseIf bc_am_in_context.insight_items.agg_audit_headers(j).warnings > 1 Then
                        lvw.SubItems.Add(bc_am_in_context.insight_items.agg_audit_headers(j).successtext + " with " + CStr(bc_am_in_context.insight_items.agg_audit_headers(j).warnings) + " warnings")
                    Else
                        lvw.SubItems.Add(bc_am_in_context.insight_items.agg_audit_headers(j).successtext)
                    End If

                    Me.uxListAggAudit.Items.Add(lvw)
                End If
            Next

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_in_settings", "load_Aggregation_log", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub


    Private Sub bc_am_in_settings_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim i As Integer
        For i = 1 To 100
            Me.cstart.Items.Add(CStr(i))
            Me.cblanks.Items.Add(CStr(i))
        Next
       

        load_user_security()
        load_Aggregation_log(True)
    End Sub

    Private Sub callent_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles callent.CheckedChanged
        If Me.callent.Checked = True Then
            Me.centdef.Visible = True
            Me.centdef.Checked = True
        Else
            Me.centdef.Visible = False
            Me.centdef.Checked = False
        End If
        set_changes()
    End Sub

    Private Sub centdef_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles centdef.CheckedChanged
        set_changes()
    End Sub

    Private Sub cstart_SelectedIndexChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cstart.SelectedIndexChanged
        set_changes()
    End Sub

    Private Sub cblanks_SelectedIndexChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cblanks.SelectedIndexChanged
        set_changes()
    End Sub

    Private Sub theadercode_TextChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles theadercode.TextChanged
        set_changes()
    End Sub

    Private Sub tperiodcode_TextChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tperiodcode.TextChanged
        set_changes()
    End Sub

    Private Sub teacode_TextChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles teacode.TextChanged
        set_changes()
    End Sub

    Private Sub tacccode_TextChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tacccode.TextChanged
        set_changes()
    End Sub

    Private Sub tlabcol_TextChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tlabcol.TextChanged
        set_changes()
    End Sub
    Private Sub taprefix_TextChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles taprefix.TextChanged
        set_changes()
    End Sub
    Private Sub teprefix_TextChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles teprefix.TextChanged
        set_changes()
    End Sub
    Private Sub tstcol_TextChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tstcol.TextChanged
        set_changes()
    End Sub
    Private Sub Button2_Click_3(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim osample As New bc_an_tk_link_sample
        Dim oapi As New API
        'API.SetWindowPos(osample.Handle.ToInt32, API.HWND_TOPMOST, 10, 10, 1, 1, 1)
        osample.Show()
        osample.TopMost = True
    End Sub
    Private Sub Lsec_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Lsec.SelectedIndexChanged
        If Me.Lsec.SelectedItems.Count > 0 Then
            tk_main.set_button_enabled(5)
        Else
            tk_main.set_button_disabled(5)
        End If
    End Sub

    Private Sub uxLinkOps_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxLinkOps.Enter

    End Sub

    Private Sub uxGeneral_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxGeneral.Enter

    End Sub

    Private Sub uxAggDate_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxAggDate.SelectedIndexChanged
        load_Aggregation_log(False)
    End Sub

    Private Sub uxAggUniverse_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxAggUniverse.SelectedIndexChanged
        load_Aggregation_log(False)
    End Sub

    Private Sub uxDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxDetails.Click

        Dim DetailUniverse As String
        Dim DetailBatch As String
        Dim DetailElapsed As String
        Dim DetailResult As String

        Dim slog = New bc_cs_activity_log("bc_am_in_settings", "uxDetails_Click", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try

            Me.Cursor = Cursors.WaitCursor

            If Me.uxListAggAudit.SelectedItems.Count > 0 Then

                DetailUniverse = Me.uxListAggAudit.SelectedItems(0).Text()
                DetailBatch = Me.uxListAggAudit.SelectedItems(0).Tag
                DetailElapsed = Me.uxListAggAudit.Items(Me.uxListAggAudit.SelectedItems(0).Index).SubItems(3).Text
                DetailResult = Me.uxListAggAudit.Items(Me.uxListAggAudit.SelectedItems(0).Index).SubItems(5).Text

                Dim oAuditDetails As New bc_am_audit_details
                oAuditDetails.DetailLogUniverse = DetailUniverse
                oAuditDetails.DetailLogBatch = DetailBatch
                oAuditDetails.DetailLogElapsed = DetailElapsed
                oAuditDetails.DetailLogResult = DetailResult
                oAuditDetails.ShowDialog()

            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_settings", "uxDetails_Click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_in_settings", "uxDetails_Click", bc_cs_activity_codes.TRACE_EXIT, "")
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    Private Sub uxlUniverse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxlUniverse.Click

    End Sub

    Private Sub uxRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxRefresh.Click

        Dim slog = New bc_cs_activity_log("bc_am_in_settings", "uxRefresh_Click", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try

            Me.Cursor = Cursors.WaitCursor

            REM refresh items
            bc_am_in_context.insight_items = New bc_om_insight_items
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                bc_am_in_context.insight_items.db_read()
            Else
                bc_am_in_context.insight_items.tmode = bc_cs_soap_base_class.tREAD
                If bc_am_in_context.insight_items.transmit_to_server_and_receive(bc_am_in_context.insight_items, True) = False Then
                    Exit Sub
                End If
            End If

            load_Aggregation_log(False)


        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_settings", "uxRefresh_Click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_in_settings", "uxRefresh_Click", bc_cs_activity_codes.TRACE_EXIT, "")
            Me.Cursor = Cursors.Default
        End Try


    End Sub

    Private Sub uxListAggAudit_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles uxListAggAudit.ColumnClick


        'Check the current sort
        If uxListAggAudit.ListViewItemSorter IsNot Nothing AndAlso TypeOf uxListAggAudit.ListViewItemSorter Is ListViewColumnSorter Then

            Dim sorter As ListViewColumnSorter
            sorter = CType(uxListAggAudit.ListViewItemSorter, ListViewColumnSorter)

            If sorter.SortColumn = e.Column And Not sorter.Order = SortOrder.Descending Then
                sorter.Order = SortOrder.Descending
                uxListAggAudit.Sorting = SortOrder.Descending

            Else
                uxListAggAudit.Sorting = SortOrder.Ascending
                sorter.Order = SortOrder.Ascending
                sorter.SortColumn = e.Column
            End If

        Else
            Dim sorter As New ListViewColumnSorter()
            uxListAggAudit.ListViewItemSorter = sorter
        End If

        'Do the sort
        uxListAggAudit.Sort()


        ''Check the current sort
        'If uxListAggAudit.ListViewItemSorter IsNot Nothing AndAlso TypeOf uxListAggAudit.ListViewItemSorter Is BCListViewItemSorter Then

        '    Dim sorter As BCListViewItemSorter
        '    sorter = CType(uxListAggAudit.ListViewItemSorter, BCListViewItemSorter)

        '    If sorter.column = e.Column And Not sorter.Desending Then
        '        sorter.Desending = True
        '        uxListAggAudit.Sorting = SortOrder.Descending

        '    Else
        '        uxListAggAudit.Sorting = SortOrder.Ascending
        '        sorter.Desending = False
        '        sorter.column = e.Column
        '    End If

        'Else
        '    Dim sorter As New BCListViewItemSorter(e.Column)
        '    uxListAggAudit.ListViewItemSorter = sorter
        'End If

        ''Do the sort
        'uxListAggAudit.Sort()

    End Sub

    Private Sub uxListAggAudit_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles uxListAggAudit.DoubleClick

        Dim DetailUniverse As String
        Dim DetailBatch As String
        Dim DetailElapsed As String
        Dim DetailResult As String

        If Me.uxListAggAudit.SelectedItems.Count > 0 Then

            DetailUniverse = Me.uxListAggAudit.SelectedItems(0).Text()
            DetailBatch = Me.uxListAggAudit.SelectedItems(0).Tag
            DetailElapsed = Me.uxListAggAudit.Items(Me.uxListAggAudit.SelectedItems(0).Index).SubItems(3).Text
            DetailResult = Me.uxListAggAudit.Items(Me.uxListAggAudit.SelectedItems(0).Index).SubItems(5).Text

            Dim oAuditDetails As New bc_am_audit_details
            oAuditDetails.DetailLogUniverse = DetailUniverse
            oAuditDetails.DetailLogBatch = DetailBatch
            oAuditDetails.DetailLogElapsed = DetailElapsed
            oAuditDetails.DetailLogResult = DetailResult
            oAuditDetails.ShowDialog()

        End If

    End Sub

    Private Sub uxListAggAudit_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxListAggAudit.SelectedIndexChanged

        If Me.uxListAggAudit.SelectedItems.Count > 0 Then
            uxDetails.Enabled = True
        Else
            uxDetails.Enabled = False
        End If
    End Sub

    Private Sub uxExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxExport.Click

        Dim slog = New bc_cs_activity_log("bc_am_in_settings", "uxExport_Click", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try

            Me.Cursor = Cursors.WaitCursor

            Dim oxlApp As Object 'Excel.Application4.        
            Dim oxlOutWBook As Object 'Excel.Workbook5.     

            oxlApp = CreateObject("Excel.Application")
            oxlOutWBook = oxlApp.Workbooks.Add
            Dim oexcel As New bc_ao_in_excel(oxlApp)

            'Open the Blue Curve addins 
            'Steve wooderson 31/03/2014
            oexcel.open_addins(oxlApp)

            Dim values(uxListAggAudit.Items.Count, uxListAggAudit.Items(0).SubItems.Count - 1) As String
            For i = 0 To uxListAggAudit.Columns.Count() - 1
                values(0, i) = uxListAggAudit.Columns(i).Text
            Next
            For i = 0 To uxListAggAudit.Items.Count - 1
                For j = 0 To uxListAggAudit.Items(i).SubItems.Count - 1
                    values(i + 1, j) = uxListAggAudit.Items(i).SubItems(j).Text
                Next j
            Next i

            oexcel.bc_array_excel_export(values)
            oxlApp.application.visible = True

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_settings", "uxExport_Click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_in_settings", "uxExport_Click", bc_cs_activity_codes.TRACE_EXIT, "")
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    Private Sub Tbatch_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Tbatch.TextChanged
        If Not IsNumeric(Me.Tbatch.Text) Then
            Dim omsg As New bc_cs_message("Blue Curve", "Number per batch must be a number", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            Exit Sub
        End If
        set_changes()
    End Sub

    Private Sub Crefreshlists_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Crefreshlists.SelectedIndexChanged
        set_changes()
    End Sub

    Private Sub cshowerrors_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cshowerrors.CheckedChanged
        set_changes()
    End Sub
End Class

