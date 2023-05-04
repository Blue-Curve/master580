Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports System.Windows.Forms
Imports System.Drawing
Friend Class bc_am_ef_wizard
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
    Friend WithEvents Lclass As System.Windows.Forms.Label
    Friend WithEvents Cclass As System.Windows.Forms.ComboBox
    Friend WithEvents sclass As System.Windows.Forms.PictureBox
    Friend WithEvents sentity As System.Windows.Forms.PictureBox
    Friend WithEvents centity As System.Windows.Forms.ComboBox
    Friend WithEvents Lentity As System.Windows.Forms.Label
    Friend WithEvents sitem As System.Windows.Forms.PictureBox
    Friend WithEvents Citem As System.Windows.Forms.ComboBox
    Friend WithEvents Litem As System.Windows.Forms.Label
    Friend WithEvents syear As System.Windows.Forms.PictureBox
    Friend WithEvents Cyear As System.Windows.Forms.ComboBox
    Friend WithEvents Lyear As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Bhelp As System.Windows.Forms.Button
    Friend WithEvents bapply As System.Windows.Forms.PictureBox
    Friend WithEvents bclose As System.Windows.Forms.PictureBox
    Friend WithEvents Copen As System.Windows.Forms.CheckBox
    Friend WithEvents gdim As System.Windows.Forms.Panel
    Friend WithEvents Lseldim As System.Windows.Forms.ListBox
    Friend WithEvents Display As System.Windows.Forms.Label
    Friend WithEvents rdef As System.Windows.Forms.RadioButton
    Friend WithEvents Rch As System.Windows.Forms.RadioButton
    Friend WithEvents Badddim As System.Windows.Forms.Button
    Friend WithEvents Bdeldim As System.Windows.Forms.Button
    Friend WithEvents Cperiod As System.Windows.Forms.ComboBox
    Friend WithEvents Lperiod As System.Windows.Forms.Label
    Friend WithEvents scont As System.Windows.Forms.PictureBox
    Friend WithEvents Ccont As System.Windows.Forms.ComboBox
    Friend WithEvents Lcont As System.Windows.Forms.Label
    Friend WithEvents sgen2 As System.Windows.Forms.PictureBox
    Friend WithEvents Cgen2 As System.Windows.Forms.ComboBox
    Friend WithEvents Lgen2 As System.Windows.Forms.Label
    Friend WithEvents sgen1 As System.Windows.Forms.PictureBox
    Friend WithEvents Cgen1 As System.Windows.Forms.ComboBox
    Friend WithEvents Lgen1 As System.Windows.Forms.Label
    Friend WithEvents Lstage As System.Windows.Forms.Label
    Friend WithEvents Llentity As System.Windows.Forms.Label
    Friend WithEvents llclass As System.Windows.Forms.Label
    Friend WithEvents scontext As System.Windows.Forms.PictureBox
    Friend WithEvents Ccontext As System.Windows.Forms.ComboBox
    Friend WithEvents Lcontext As System.Windows.Forms.Label
    Friend WithEvents sstage As System.Windows.Forms.PictureBox
    Friend WithEvents Cstage As System.Windows.Forms.ComboBox
    Friend WithEvents slentity As System.Windows.Forms.PictureBox
    Friend WithEvents clentity As System.Windows.Forms.ComboBox
    Friend WithEvents slclass As System.Windows.Forms.PictureBox
    Friend WithEvents clclass As System.Windows.Forms.ComboBox
    Friend WithEvents Csection As System.Windows.Forms.ComboBox
    Friend WithEvents Lsection As System.Windows.Forms.Label
    Friend WithEvents ssection As System.Windows.Forms.PictureBox
    Friend WithEvents pdate As System.Windows.Forms.Panel
    Friend WithEvents ldate As System.Windows.Forms.Label
    Friend WithEvents dtpdate As System.Windows.Forms.DateTimePicker
    Friend WithEvents sdate As System.Windows.Forms.PictureBox
    Friend WithEvents tdate As System.Windows.Forms.TextBox
    Friend WithEvents Chkdate As System.Windows.Forms.CheckBox
    Friend WithEvents pdate_from As System.Windows.Forms.Panel
    Friend WithEvents prange As System.Windows.Forms.Panel
    Friend WithEvents sstart As System.Windows.Forms.PictureBox
    Friend WithEvents send As System.Windows.Forms.PictureBox
    Friend WithEvents Cstart As System.Windows.Forms.ComboBox
    Friend WithEvents cend As System.Windows.Forms.ComboBox
    Friend WithEvents Crange As System.Windows.Forms.ComboBox
    Friend WithEvents Lrange As System.Windows.Forms.Label
    Friend WithEvents Lto As System.Windows.Forms.Label
    Friend WithEvents Cfunction As System.Windows.Forms.ComboBox
    Friend WithEvents Thelp As System.Windows.Forms.TextBox
    Friend WithEvents tdate_from As System.Windows.Forms.TextBox
    Friend WithEvents chkdate_from As System.Windows.Forms.CheckBox
    Friend WithEvents sdate_from As System.Windows.Forms.PictureBox
    Friend WithEvents Lalldim As System.Windows.Forms.ListBox
    Friend WithEvents Ldate_from As System.Windows.Forms.Label
    Friend WithEvents dtpdate_from As System.Windows.Forms.DateTimePicker
    Friend WithEvents speriod As System.Windows.Forms.PictureBox
    Friend WithEvents Pdepress As System.Windows.Forms.PictureBox
    Friend WithEvents pcandn As System.Windows.Forms.PictureBox
    Friend WithEvents uxTypeNameHelp As System.Windows.Forms.PictureBox
    Friend WithEvents cgen3 As System.Windows.Forms.ComboBox
    Friend WithEvents lgen3 As System.Windows.Forms.Label
    Friend WithEvents Ccal As System.Windows.Forms.ComboBox
    Friend WithEvents Lcal As System.Windows.Forms.Label
    Friend WithEvents Cmon As System.Windows.Forms.ComboBox
    Friend WithEvents smon As System.Windows.Forms.PictureBox
    Friend WithEvents scal As System.Windows.Forms.PictureBox
    Friend WithEvents SearchTimer As System.Windows.Forms.Timer
    Friend WithEvents BlueCurve_EntitytSearch As BlueCurve.Core.AS.BlueCurve_TextSearch
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents pnlButtons As System.Windows.Forms.Panel
    Friend WithEvents sgen3 As System.Windows.Forms.PictureBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_ef_wizard))
        Me.Lclass = New System.Windows.Forms.Label
        Me.Cclass = New System.Windows.Forms.ComboBox
        Me.sclass = New System.Windows.Forms.PictureBox
        Me.sentity = New System.Windows.Forms.PictureBox
        Me.centity = New System.Windows.Forms.ComboBox
        Me.Lentity = New System.Windows.Forms.Label
        Me.sitem = New System.Windows.Forms.PictureBox
        Me.Citem = New System.Windows.Forms.ComboBox
        Me.Litem = New System.Windows.Forms.Label
        Me.syear = New System.Windows.Forms.PictureBox
        Me.Cyear = New System.Windows.Forms.ComboBox
        Me.Lyear = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Cfunction = New System.Windows.Forms.ComboBox
        Me.Bhelp = New System.Windows.Forms.Button
        Me.bapply = New System.Windows.Forms.PictureBox
        Me.bclose = New System.Windows.Forms.PictureBox
        Me.Copen = New System.Windows.Forms.CheckBox
        Me.gdim = New System.Windows.Forms.Panel
        Me.Bdeldim = New System.Windows.Forms.Button
        Me.Badddim = New System.Windows.Forms.Button
        Me.Rch = New System.Windows.Forms.RadioButton
        Me.rdef = New System.Windows.Forms.RadioButton
        Me.Display = New System.Windows.Forms.Label
        Me.Lseldim = New System.Windows.Forms.ListBox
        Me.Lalldim = New System.Windows.Forms.ListBox
        Me.Cperiod = New System.Windows.Forms.ComboBox
        Me.Lperiod = New System.Windows.Forms.Label
        Me.scont = New System.Windows.Forms.PictureBox
        Me.Ccont = New System.Windows.Forms.ComboBox
        Me.Lcont = New System.Windows.Forms.Label
        Me.sgen2 = New System.Windows.Forms.PictureBox
        Me.Cgen2 = New System.Windows.Forms.ComboBox
        Me.Lgen2 = New System.Windows.Forms.Label
        Me.sgen1 = New System.Windows.Forms.PictureBox
        Me.Cgen1 = New System.Windows.Forms.ComboBox
        Me.Lgen1 = New System.Windows.Forms.Label
        Me.scontext = New System.Windows.Forms.PictureBox
        Me.Ccontext = New System.Windows.Forms.ComboBox
        Me.Lcontext = New System.Windows.Forms.Label
        Me.sstage = New System.Windows.Forms.PictureBox
        Me.Cstage = New System.Windows.Forms.ComboBox
        Me.Lstage = New System.Windows.Forms.Label
        Me.slentity = New System.Windows.Forms.PictureBox
        Me.clentity = New System.Windows.Forms.ComboBox
        Me.Llentity = New System.Windows.Forms.Label
        Me.slclass = New System.Windows.Forms.PictureBox
        Me.clclass = New System.Windows.Forms.ComboBox
        Me.llclass = New System.Windows.Forms.Label
        Me.ssection = New System.Windows.Forms.PictureBox
        Me.Csection = New System.Windows.Forms.ComboBox
        Me.Lsection = New System.Windows.Forms.Label
        Me.pdate = New System.Windows.Forms.Panel
        Me.tdate = New System.Windows.Forms.TextBox
        Me.Chkdate = New System.Windows.Forms.CheckBox
        Me.sdate = New System.Windows.Forms.PictureBox
        Me.dtpdate = New System.Windows.Forms.DateTimePicker
        Me.ldate = New System.Windows.Forms.Label
        Me.pdate_from = New System.Windows.Forms.Panel
        Me.tdate_from = New System.Windows.Forms.TextBox
        Me.chkdate_from = New System.Windows.Forms.CheckBox
        Me.sdate_from = New System.Windows.Forms.PictureBox
        Me.dtpdate_from = New System.Windows.Forms.DateTimePicker
        Me.Ldate_from = New System.Windows.Forms.Label
        Me.prange = New System.Windows.Forms.Panel
        Me.Lto = New System.Windows.Forms.Label
        Me.Crange = New System.Windows.Forms.ComboBox
        Me.Lrange = New System.Windows.Forms.Label
        Me.cend = New System.Windows.Forms.ComboBox
        Me.send = New System.Windows.Forms.PictureBox
        Me.sstart = New System.Windows.Forms.PictureBox
        Me.Cstart = New System.Windows.Forms.ComboBox
        Me.Thelp = New System.Windows.Forms.TextBox
        Me.speriod = New System.Windows.Forms.PictureBox
        Me.Pdepress = New System.Windows.Forms.PictureBox
        Me.pcandn = New System.Windows.Forms.PictureBox
        Me.uxTypeNameHelp = New System.Windows.Forms.PictureBox
        Me.cgen3 = New System.Windows.Forms.ComboBox
        Me.lgen3 = New System.Windows.Forms.Label
        Me.sgen3 = New System.Windows.Forms.PictureBox
        Me.Ccal = New System.Windows.Forms.ComboBox
        Me.Lcal = New System.Windows.Forms.Label
        Me.Cmon = New System.Windows.Forms.ComboBox
        Me.smon = New System.Windows.Forms.PictureBox
        Me.scal = New System.Windows.Forms.PictureBox
        Me.SearchTimer = New System.Windows.Forms.Timer(Me.components)
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.pnlButtons = New System.Windows.Forms.Panel
        Me.BlueCurve_EntitytSearch = New BlueCurve.Core.[AS].BlueCurve_TextSearch
        CType(Me.sclass, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.sentity, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.sitem, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.syear, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bapply, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bclose, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gdim.SuspendLayout()
        CType(Me.scont, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.sgen2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.sgen1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.scontext, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.sstage, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.slentity, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.slclass, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ssection, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pdate.SuspendLayout()
        CType(Me.sdate, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pdate_from.SuspendLayout()
        CType(Me.sdate_from, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.prange.SuspendLayout()
        CType(Me.send, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.sstart, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.speriod, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Pdepress, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pcandn, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxTypeNameHelp, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.sgen3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.smon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.scal, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.pnlButtons.SuspendLayout()
        Me.SuspendLayout()
        '
        'Lclass
        '
        Me.Lclass.Location = New System.Drawing.Point(16, 72)
        Me.Lclass.Name = "Lclass"
        Me.Lclass.Size = New System.Drawing.Size(112, 24)
        Me.Lclass.TabIndex = 0
        Me.Lclass.Text = "Label1"
        '
        'Cclass
        '
        Me.Cclass.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.Cclass.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.Cclass.Location = New System.Drawing.Point(88, 72)
        Me.Cclass.MaxLength = 250
        Me.Cclass.Name = "Cclass"
        Me.Cclass.Size = New System.Drawing.Size(158, 21)
        Me.Cclass.TabIndex = 2
        Me.Cclass.Tag = "sizable"
        '
        'sclass
        '
        Me.sclass.Image = CType(resources.GetObject("sclass.Image"), System.Drawing.Image)
        Me.sclass.Location = New System.Drawing.Point(264, 72)
        Me.sclass.Name = "sclass"
        Me.sclass.Size = New System.Drawing.Size(24, 24)
        Me.sclass.TabIndex = 3
        Me.sclass.TabStop = False
        Me.sclass.Tag = "movable"
        '
        'sentity
        '
        Me.sentity.Image = CType(resources.GetObject("sentity.Image"), System.Drawing.Image)
        Me.sentity.Location = New System.Drawing.Point(264, 104)
        Me.sentity.Name = "sentity"
        Me.sentity.Size = New System.Drawing.Size(24, 24)
        Me.sentity.TabIndex = 6
        Me.sentity.TabStop = False
        Me.sentity.Tag = "movable"
        '
        'centity
        '
        Me.centity.Location = New System.Drawing.Point(88, 104)
        Me.centity.MaxLength = 250
        Me.centity.Name = "centity"
        Me.centity.Size = New System.Drawing.Size(158, 21)
        Me.centity.TabIndex = 5
        Me.centity.Tag = "sizable"
        '
        'Lentity
        '
        Me.Lentity.Location = New System.Drawing.Point(15, 104)
        Me.Lentity.Name = "Lentity"
        Me.Lentity.Size = New System.Drawing.Size(97, 24)
        Me.Lentity.TabIndex = 4
        Me.Lentity.Text = "Label1"
        '
        'sitem
        '
        Me.sitem.Image = CType(resources.GetObject("sitem.Image"), System.Drawing.Image)
        Me.sitem.Location = New System.Drawing.Point(248, 136)
        Me.sitem.Name = "sitem"
        Me.sitem.Size = New System.Drawing.Size(24, 24)
        Me.sitem.TabIndex = 9
        Me.sitem.TabStop = False
        Me.sitem.Tag = "movable"
        '
        'Citem
        '
        Me.Citem.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.Citem.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.Citem.Location = New System.Drawing.Point(88, 136)
        Me.Citem.MaxLength = 250
        Me.Citem.Name = "Citem"
        Me.Citem.Size = New System.Drawing.Size(158, 21)
        Me.Citem.TabIndex = 8
        Me.Citem.Tag = "sizable"
        '
        'Litem
        '
        Me.Litem.Location = New System.Drawing.Point(15, 136)
        Me.Litem.Name = "Litem"
        Me.Litem.Size = New System.Drawing.Size(97, 24)
        Me.Litem.TabIndex = 7
        Me.Litem.Text = "Label1"
        '
        'syear
        '
        Me.syear.Image = CType(resources.GetObject("syear.Image"), System.Drawing.Image)
        Me.syear.Location = New System.Drawing.Point(256, 168)
        Me.syear.Name = "syear"
        Me.syear.Size = New System.Drawing.Size(24, 24)
        Me.syear.TabIndex = 12
        Me.syear.TabStop = False
        Me.syear.Tag = "movable"
        '
        'Cyear
        '
        Me.Cyear.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.Cyear.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.Cyear.Location = New System.Drawing.Point(88, 168)
        Me.Cyear.MaxLength = 4
        Me.Cyear.Name = "Cyear"
        Me.Cyear.Size = New System.Drawing.Size(158, 21)
        Me.Cyear.TabIndex = 11
        Me.Cyear.Tag = "sizable"
        '
        'Lyear
        '
        Me.Lyear.Location = New System.Drawing.Point(16, 168)
        Me.Lyear.Name = "Lyear"
        Me.Lyear.Size = New System.Drawing.Size(112, 24)
        Me.Lyear.TabIndex = 10
        Me.Lyear.Text = "Label1"
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(15, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(112, 24)
        Me.Label1.TabIndex = 13
        Me.Label1.Text = "I would like to"
        '
        'Cfunction
        '
        Me.Cfunction.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.Cfunction.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.Cfunction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Cfunction.Location = New System.Drawing.Point(17, 25)
        Me.Cfunction.Name = "Cfunction"
        Me.Cfunction.Size = New System.Drawing.Size(235, 21)
        Me.Cfunction.TabIndex = 1
        Me.Cfunction.Tag = "sizable"
        '
        'Bhelp
        '
        Me.Bhelp.Enabled = False
        Me.Bhelp.Location = New System.Drawing.Point(256, 24)
        Me.Bhelp.Name = "Bhelp"
        Me.Bhelp.Size = New System.Drawing.Size(24, 22)
        Me.Bhelp.TabIndex = 2
        Me.Bhelp.Tag = "movable"
        '
        'bapply
        '
        Me.bapply.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bapply.Image = CType(resources.GetObject("bapply.Image"), System.Drawing.Image)
        Me.bapply.Location = New System.Drawing.Point(63, 2)
        Me.bapply.Name = "bapply"
        Me.bapply.Size = New System.Drawing.Size(40, 40)
        Me.bapply.TabIndex = 16
        Me.bapply.TabStop = False
        '
        'bclose
        '
        Me.bclose.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bclose.Image = CType(resources.GetObject("bclose.Image"), System.Drawing.Image)
        Me.bclose.Location = New System.Drawing.Point(17, 2)
        Me.bclose.Name = "bclose"
        Me.bclose.Size = New System.Drawing.Size(40, 40)
        Me.bclose.TabIndex = 17
        Me.bclose.TabStop = False
        '
        'Copen
        '
        Me.Copen.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Copen.Location = New System.Drawing.Point(8, 10)
        Me.Copen.Name = "Copen"
        Me.Copen.Size = New System.Drawing.Size(96, 24)
        Me.Copen.TabIndex = 18
        Me.Copen.Text = "Leave open"
        '
        'gdim
        '
        Me.gdim.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.gdim.Controls.Add(Me.Bdeldim)
        Me.gdim.Controls.Add(Me.Badddim)
        Me.gdim.Controls.Add(Me.Rch)
        Me.gdim.Controls.Add(Me.rdef)
        Me.gdim.Controls.Add(Me.Display)
        Me.gdim.Controls.Add(Me.Lseldim)
        Me.gdim.Controls.Add(Me.Lalldim)
        Me.gdim.Location = New System.Drawing.Point(0, 384)
        Me.gdim.Name = "gdim"
        Me.gdim.Size = New System.Drawing.Size(303, 110)
        Me.gdim.TabIndex = 19
        '
        'Bdeldim
        '
        Me.Bdeldim.Location = New System.Drawing.Point(136, 53)
        Me.Bdeldim.Name = "Bdeldim"
        Me.Bdeldim.Size = New System.Drawing.Size(32, 24)
        Me.Bdeldim.TabIndex = 6
        Me.Bdeldim.Text = "<"
        '
        'Badddim
        '
        Me.Badddim.Location = New System.Drawing.Point(136, 27)
        Me.Badddim.Name = "Badddim"
        Me.Badddim.Size = New System.Drawing.Size(32, 24)
        Me.Badddim.TabIndex = 5
        Me.Badddim.Text = ">"
        '
        'Rch
        '
        Me.Rch.Location = New System.Drawing.Point(189, 4)
        Me.Rch.Name = "Rch"
        Me.Rch.Size = New System.Drawing.Size(72, 16)
        Me.Rch.TabIndex = 4
        Me.Rch.Text = "Choose >>"
        '
        'rdef
        '
        Me.rdef.Location = New System.Drawing.Point(100, 4)
        Me.rdef.Name = "rdef"
        Me.rdef.Size = New System.Drawing.Size(72, 16)
        Me.rdef.TabIndex = 3
        Me.rdef.Text = "Default"
        '
        'Display
        '
        Me.Display.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Display.Location = New System.Drawing.Point(16, 4)
        Me.Display.Name = "Display"
        Me.Display.Size = New System.Drawing.Size(104, 16)
        Me.Display.TabIndex = 2
        Me.Display.Text = "Display"
        '
        'Lseldim
        '
        Me.Lseldim.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Lseldim.Location = New System.Drawing.Point(184, 24)
        Me.Lseldim.Name = "Lseldim"
        Me.Lseldim.Size = New System.Drawing.Size(103, 69)
        Me.Lseldim.TabIndex = 1
        '
        'Lalldim
        '
        Me.Lalldim.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Lalldim.Location = New System.Drawing.Point(16, 24)
        Me.Lalldim.Name = "Lalldim"
        Me.Lalldim.Size = New System.Drawing.Size(103, 69)
        Me.Lalldim.TabIndex = 0
        '
        'Cperiod
        '
        Me.Cperiod.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.Cperiod.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.Cperiod.Location = New System.Drawing.Point(88, 184)
        Me.Cperiod.MaxLength = 250
        Me.Cperiod.Name = "Cperiod"
        Me.Cperiod.Size = New System.Drawing.Size(158, 21)
        Me.Cperiod.TabIndex = 30
        Me.Cperiod.Tag = "sizable"
        '
        'Lperiod
        '
        Me.Lperiod.Location = New System.Drawing.Point(16, 184)
        Me.Lperiod.Name = "Lperiod"
        Me.Lperiod.Size = New System.Drawing.Size(64, 24)
        Me.Lperiod.TabIndex = 29
        Me.Lperiod.Text = "Label1"
        '
        'scont
        '
        Me.scont.Image = CType(resources.GetObject("scont.Image"), System.Drawing.Image)
        Me.scont.Location = New System.Drawing.Point(263, 152)
        Me.scont.Name = "scont"
        Me.scont.Size = New System.Drawing.Size(24, 24)
        Me.scont.TabIndex = 28
        Me.scont.TabStop = False
        Me.scont.Tag = "movable"
        '
        'Ccont
        '
        Me.Ccont.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.Ccont.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.Ccont.Location = New System.Drawing.Point(88, 152)
        Me.Ccont.MaxLength = 250
        Me.Ccont.Name = "Ccont"
        Me.Ccont.Size = New System.Drawing.Size(158, 21)
        Me.Ccont.TabIndex = 27
        Me.Ccont.Tag = "sizable"
        '
        'Lcont
        '
        Me.Lcont.Location = New System.Drawing.Point(15, 152)
        Me.Lcont.Name = "Lcont"
        Me.Lcont.Size = New System.Drawing.Size(105, 24)
        Me.Lcont.TabIndex = 26
        Me.Lcont.Text = "Label1"
        '
        'sgen2
        '
        Me.sgen2.Image = CType(resources.GetObject("sgen2.Image"), System.Drawing.Image)
        Me.sgen2.Location = New System.Drawing.Point(263, 120)
        Me.sgen2.Name = "sgen2"
        Me.sgen2.Size = New System.Drawing.Size(24, 24)
        Me.sgen2.TabIndex = 25
        Me.sgen2.TabStop = False
        Me.sgen2.Tag = "movable"
        '
        'Cgen2
        '
        Me.Cgen2.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.Cgen2.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.Cgen2.Location = New System.Drawing.Point(88, 120)
        Me.Cgen2.MaxLength = 250
        Me.Cgen2.Name = "Cgen2"
        Me.Cgen2.Size = New System.Drawing.Size(158, 21)
        Me.Cgen2.TabIndex = 24
        Me.Cgen2.Tag = "sizable"
        '
        'Lgen2
        '
        Me.Lgen2.Location = New System.Drawing.Point(15, 120)
        Me.Lgen2.Name = "Lgen2"
        Me.Lgen2.Size = New System.Drawing.Size(104, 24)
        Me.Lgen2.TabIndex = 23
        Me.Lgen2.Text = "Label1"
        '
        'sgen1
        '
        Me.sgen1.Image = CType(resources.GetObject("sgen1.Image"), System.Drawing.Image)
        Me.sgen1.Location = New System.Drawing.Point(264, 88)
        Me.sgen1.Name = "sgen1"
        Me.sgen1.Size = New System.Drawing.Size(24, 24)
        Me.sgen1.TabIndex = 22
        Me.sgen1.TabStop = False
        Me.sgen1.Tag = "movable"
        '
        'Cgen1
        '
        Me.Cgen1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.Cgen1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.Cgen1.Location = New System.Drawing.Point(88, 88)
        Me.Cgen1.MaxLength = 250
        Me.Cgen1.Name = "Cgen1"
        Me.Cgen1.Size = New System.Drawing.Size(158, 21)
        Me.Cgen1.TabIndex = 21
        Me.Cgen1.Tag = "sizable"
        '
        'Lgen1
        '
        Me.Lgen1.Location = New System.Drawing.Point(16, 88)
        Me.Lgen1.Name = "Lgen1"
        Me.Lgen1.Size = New System.Drawing.Size(104, 24)
        Me.Lgen1.TabIndex = 20
        Me.Lgen1.Text = "Label1"
        '
        'scontext
        '
        Me.scontext.Image = CType(resources.GetObject("scontext.Image"), System.Drawing.Image)
        Me.scontext.Location = New System.Drawing.Point(280, 192)
        Me.scontext.Name = "scontext"
        Me.scontext.Size = New System.Drawing.Size(24, 24)
        Me.scontext.TabIndex = 43
        Me.scontext.TabStop = False
        Me.scontext.Tag = "movable"
        '
        'Ccontext
        '
        Me.Ccontext.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.Ccontext.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.Ccontext.Location = New System.Drawing.Point(88, 184)
        Me.Ccontext.MaxLength = 250
        Me.Ccontext.Name = "Ccontext"
        Me.Ccontext.Size = New System.Drawing.Size(158, 21)
        Me.Ccontext.TabIndex = 42
        Me.Ccontext.Tag = "sizable"
        '
        'Lcontext
        '
        Me.Lcontext.Location = New System.Drawing.Point(16, 184)
        Me.Lcontext.Name = "Lcontext"
        Me.Lcontext.Size = New System.Drawing.Size(96, 24)
        Me.Lcontext.TabIndex = 41
        Me.Lcontext.Text = "Label1"
        '
        'sstage
        '
        Me.sstage.Image = CType(resources.GetObject("sstage.Image"), System.Drawing.Image)
        Me.sstage.Location = New System.Drawing.Point(272, 152)
        Me.sstage.Name = "sstage"
        Me.sstage.Size = New System.Drawing.Size(24, 24)
        Me.sstage.TabIndex = 40
        Me.sstage.TabStop = False
        Me.sstage.Tag = "movable"
        '
        'Cstage
        '
        Me.Cstage.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.Cstage.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.Cstage.Location = New System.Drawing.Point(88, 152)
        Me.Cstage.MaxLength = 250
        Me.Cstage.Name = "Cstage"
        Me.Cstage.Size = New System.Drawing.Size(158, 21)
        Me.Cstage.TabIndex = 39
        Me.Cstage.Tag = "sizable"
        '
        'Lstage
        '
        Me.Lstage.Location = New System.Drawing.Point(15, 152)
        Me.Lstage.Name = "Lstage"
        Me.Lstage.Size = New System.Drawing.Size(113, 24)
        Me.Lstage.TabIndex = 38
        Me.Lstage.Text = "Label1"
        '
        'slentity
        '
        Me.slentity.Image = CType(resources.GetObject("slentity.Image"), System.Drawing.Image)
        Me.slentity.Location = New System.Drawing.Point(272, 120)
        Me.slentity.Name = "slentity"
        Me.slentity.Size = New System.Drawing.Size(24, 24)
        Me.slentity.TabIndex = 37
        Me.slentity.TabStop = False
        Me.slentity.Tag = "movable"
        '
        'clentity
        '
        Me.clentity.Location = New System.Drawing.Point(88, 120)
        Me.clentity.MaxLength = 250
        Me.clentity.Name = "clentity"
        Me.clentity.Size = New System.Drawing.Size(158, 21)
        Me.clentity.TabIndex = 36
        Me.clentity.Tag = "sizable"
        '
        'Llentity
        '
        Me.Llentity.Location = New System.Drawing.Point(15, 120)
        Me.Llentity.Name = "Llentity"
        Me.Llentity.Size = New System.Drawing.Size(105, 24)
        Me.Llentity.TabIndex = 35
        Me.Llentity.Text = "Label1"
        '
        'slclass
        '
        Me.slclass.Image = CType(resources.GetObject("slclass.Image"), System.Drawing.Image)
        Me.slclass.Location = New System.Drawing.Point(256, 208)
        Me.slclass.Name = "slclass"
        Me.slclass.Size = New System.Drawing.Size(24, 24)
        Me.slclass.TabIndex = 34
        Me.slclass.TabStop = False
        Me.slclass.Tag = "movable"
        '
        'clclass
        '
        Me.clclass.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.clclass.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.clclass.Location = New System.Drawing.Point(88, 208)
        Me.clclass.MaxLength = 250
        Me.clclass.Name = "clclass"
        Me.clclass.Size = New System.Drawing.Size(158, 21)
        Me.clclass.TabIndex = 33
        Me.clclass.Tag = "sizable"
        '
        'llclass
        '
        Me.llclass.Location = New System.Drawing.Point(16, 203)
        Me.llclass.Name = "llclass"
        Me.llclass.Size = New System.Drawing.Size(88, 24)
        Me.llclass.TabIndex = 32
        Me.llclass.Text = "Label1"
        '
        'ssection
        '
        Me.ssection.Image = CType(resources.GetObject("ssection.Image"), System.Drawing.Image)
        Me.ssection.Location = New System.Drawing.Point(280, 224)
        Me.ssection.Name = "ssection"
        Me.ssection.Size = New System.Drawing.Size(24, 24)
        Me.ssection.TabIndex = 46
        Me.ssection.TabStop = False
        Me.ssection.Tag = "movable"
        '
        'Csection
        '
        Me.Csection.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.Csection.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.Csection.Location = New System.Drawing.Point(94, 216)
        Me.Csection.MaxLength = 250
        Me.Csection.Name = "Csection"
        Me.Csection.Size = New System.Drawing.Size(158, 21)
        Me.Csection.TabIndex = 45
        Me.Csection.Tag = "sizable"
        '
        'Lsection
        '
        Me.Lsection.Location = New System.Drawing.Point(15, 216)
        Me.Lsection.Name = "Lsection"
        Me.Lsection.Size = New System.Drawing.Size(105, 24)
        Me.Lsection.TabIndex = 44
        Me.Lsection.Text = "Label1"
        '
        'pdate
        '
        Me.pdate.Controls.Add(Me.tdate)
        Me.pdate.Controls.Add(Me.Chkdate)
        Me.pdate.Controls.Add(Me.sdate)
        Me.pdate.Controls.Add(Me.dtpdate)
        Me.pdate.Controls.Add(Me.ldate)
        Me.pdate.Location = New System.Drawing.Point(15, 248)
        Me.pdate.Name = "pdate"
        Me.pdate.Size = New System.Drawing.Size(280, 32)
        Me.pdate.TabIndex = 47
        '
        'tdate
        '
        Me.tdate.Location = New System.Drawing.Point(133, 5)
        Me.tdate.MaxLength = 250
        Me.tdate.Name = "tdate"
        Me.tdate.Size = New System.Drawing.Size(80, 21)
        Me.tdate.TabIndex = 5
        '
        'Chkdate
        '
        Me.Chkdate.Location = New System.Drawing.Point(80, 7)
        Me.Chkdate.Name = "Chkdate"
        Me.Chkdate.Size = New System.Drawing.Size(48, 16)
        Me.Chkdate.TabIndex = 6
        Me.Chkdate.Text = "Now"
        '
        'sdate
        '
        Me.sdate.Image = CType(resources.GetObject("sdate.Image"), System.Drawing.Image)
        Me.sdate.Location = New System.Drawing.Point(246, 5)
        Me.sdate.Name = "sdate"
        Me.sdate.Size = New System.Drawing.Size(24, 24)
        Me.sdate.TabIndex = 4
        Me.sdate.TabStop = False
        Me.sdate.Tag = ""
        '
        'dtpdate
        '
        Me.dtpdate.Location = New System.Drawing.Point(213, 5)
        Me.dtpdate.Name = "dtpdate"
        Me.dtpdate.Size = New System.Drawing.Size(24, 21)
        Me.dtpdate.TabIndex = 2
        '
        'ldate
        '
        Me.ldate.Location = New System.Drawing.Point(0, 5)
        Me.ldate.Name = "ldate"
        Me.ldate.Size = New System.Drawing.Size(80, 16)
        Me.ldate.TabIndex = 1
        Me.ldate.Text = "Data at date"
        '
        'pdate_from
        '
        Me.pdate_from.Controls.Add(Me.tdate_from)
        Me.pdate_from.Controls.Add(Me.chkdate_from)
        Me.pdate_from.Controls.Add(Me.sdate_from)
        Me.pdate_from.Controls.Add(Me.dtpdate_from)
        Me.pdate_from.Controls.Add(Me.Ldate_from)
        Me.pdate_from.Location = New System.Drawing.Point(15, 288)
        Me.pdate_from.Name = "pdate_from"
        Me.pdate_from.Size = New System.Drawing.Size(280, 32)
        Me.pdate_from.TabIndex = 48
        '
        'tdate_from
        '
        Me.tdate_from.Location = New System.Drawing.Point(133, 5)
        Me.tdate_from.MaxLength = 250
        Me.tdate_from.Name = "tdate_from"
        Me.tdate_from.Size = New System.Drawing.Size(80, 21)
        Me.tdate_from.TabIndex = 5
        '
        'chkdate_from
        '
        Me.chkdate_from.Location = New System.Drawing.Point(80, 7)
        Me.chkdate_from.Name = "chkdate_from"
        Me.chkdate_from.Size = New System.Drawing.Size(48, 16)
        Me.chkdate_from.TabIndex = 6
        Me.chkdate_from.Text = "Old"
        '
        'sdate_from
        '
        Me.sdate_from.Image = CType(resources.GetObject("sdate_from.Image"), System.Drawing.Image)
        Me.sdate_from.Location = New System.Drawing.Point(246, 5)
        Me.sdate_from.Name = "sdate_from"
        Me.sdate_from.Size = New System.Drawing.Size(24, 24)
        Me.sdate_from.TabIndex = 4
        Me.sdate_from.TabStop = False
        Me.sdate_from.Tag = ""
        '
        'dtpdate_from
        '
        Me.dtpdate_from.Location = New System.Drawing.Point(212, 5)
        Me.dtpdate_from.Name = "dtpdate_from"
        Me.dtpdate_from.Size = New System.Drawing.Size(24, 21)
        Me.dtpdate_from.TabIndex = 2
        '
        'Ldate_from
        '
        Me.Ldate_from.Location = New System.Drawing.Point(0, 5)
        Me.Ldate_from.Name = "Ldate_from"
        Me.Ldate_from.Size = New System.Drawing.Size(80, 16)
        Me.Ldate_from.TabIndex = 1
        Me.Ldate_from.Text = "Data at date"
        '
        'prange
        '
        Me.prange.Controls.Add(Me.Lto)
        Me.prange.Controls.Add(Me.Crange)
        Me.prange.Controls.Add(Me.Lrange)
        Me.prange.Controls.Add(Me.cend)
        Me.prange.Controls.Add(Me.send)
        Me.prange.Controls.Add(Me.sstart)
        Me.prange.Controls.Add(Me.Cstart)
        Me.prange.Location = New System.Drawing.Point(15, 328)
        Me.prange.Name = "prange"
        Me.prange.Size = New System.Drawing.Size(280, 53)
        Me.prange.TabIndex = 49
        '
        'Lto
        '
        Me.Lto.Location = New System.Drawing.Point(123, 32)
        Me.Lto.Name = "Lto"
        Me.Lto.Size = New System.Drawing.Size(28, 16)
        Me.Lto.TabIndex = 13
        Me.Lto.Text = "and"
        '
        'Crange
        '
        Me.Crange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Crange.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Crange.Location = New System.Drawing.Point(80, 3)
        Me.Crange.MaxLength = 250
        Me.Crange.Name = "Crange"
        Me.Crange.Size = New System.Drawing.Size(70, 21)
        Me.Crange.TabIndex = 11
        '
        'Lrange
        '
        Me.Lrange.Location = New System.Drawing.Point(0, 8)
        Me.Lrange.Name = "Lrange"
        Me.Lrange.Size = New System.Drawing.Size(64, 16)
        Me.Lrange.TabIndex = 12
        Me.Lrange.Text = "Criteria"
        '
        'cend
        '
        Me.cend.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cend.Location = New System.Drawing.Point(156, 28)
        Me.cend.MaxLength = 250
        Me.cend.Name = "cend"
        Me.cend.Size = New System.Drawing.Size(81, 21)
        Me.cend.TabIndex = 10
        Me.cend.Tag = "sizable"
        '
        'send
        '
        Me.send.Image = CType(resources.GetObject("send.Image"), System.Drawing.Image)
        Me.send.Location = New System.Drawing.Point(246, 28)
        Me.send.Name = "send"
        Me.send.Size = New System.Drawing.Size(24, 24)
        Me.send.TabIndex = 8
        Me.send.TabStop = False
        Me.send.Tag = "movable"
        '
        'sstart
        '
        Me.sstart.Image = CType(resources.GetObject("sstart.Image"), System.Drawing.Image)
        Me.sstart.Location = New System.Drawing.Point(246, 3)
        Me.sstart.Name = "sstart"
        Me.sstart.Size = New System.Drawing.Size(24, 24)
        Me.sstart.TabIndex = 7
        Me.sstart.TabStop = False
        Me.sstart.Tag = "movable"
        '
        'Cstart
        '
        Me.Cstart.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Cstart.Location = New System.Drawing.Point(156, 3)
        Me.Cstart.MaxLength = 250
        Me.Cstart.Name = "Cstart"
        Me.Cstart.Size = New System.Drawing.Size(81, 21)
        Me.Cstart.TabIndex = 9
        Me.Cstart.Tag = "sizable"
        '
        'Thelp
        '
        Me.Thelp.Location = New System.Drawing.Point(16, 56)
        Me.Thelp.MaxLength = 4000
        Me.Thelp.Multiline = True
        Me.Thelp.Name = "Thelp"
        Me.Thelp.ReadOnly = True
        Me.Thelp.Size = New System.Drawing.Size(272, 48)
        Me.Thelp.TabIndex = 50
        '
        'speriod
        '
        Me.speriod.Image = CType(resources.GetObject("speriod.Image"), System.Drawing.Image)
        Me.speriod.Location = New System.Drawing.Point(264, 104)
        Me.speriod.Name = "speriod"
        Me.speriod.Size = New System.Drawing.Size(24, 24)
        Me.speriod.TabIndex = 51
        Me.speriod.TabStop = False
        Me.speriod.Tag = "movable"
        '
        'Pdepress
        '
        Me.Pdepress.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Pdepress.Image = CType(resources.GetObject("Pdepress.Image"), System.Drawing.Image)
        Me.Pdepress.Location = New System.Drawing.Point(63, 2)
        Me.Pdepress.Name = "Pdepress"
        Me.Pdepress.Size = New System.Drawing.Size(40, 40)
        Me.Pdepress.TabIndex = 52
        Me.Pdepress.TabStop = False
        '
        'pcandn
        '
        Me.pcandn.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.pcandn.Image = CType(resources.GetObject("pcandn.Image"), System.Drawing.Image)
        Me.pcandn.Location = New System.Drawing.Point(17, 2)
        Me.pcandn.Name = "pcandn"
        Me.pcandn.Size = New System.Drawing.Size(40, 40)
        Me.pcandn.TabIndex = 53
        Me.pcandn.TabStop = False
        '
        'uxTypeNameHelp
        '
        Me.uxTypeNameHelp.Enabled = False
        Me.uxTypeNameHelp.Image = CType(resources.GetObject("uxTypeNameHelp.Image"), System.Drawing.Image)
        Me.uxTypeNameHelp.Location = New System.Drawing.Point(259, 27)
        Me.uxTypeNameHelp.Name = "uxTypeNameHelp"
        Me.uxTypeNameHelp.Size = New System.Drawing.Size(16, 16)
        Me.uxTypeNameHelp.TabIndex = 55
        Me.uxTypeNameHelp.TabStop = False
        Me.uxTypeNameHelp.Tag = "movable"
        '
        'cgen3
        '
        Me.cgen3.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cgen3.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cgen3.Location = New System.Drawing.Point(72, 252)
        Me.cgen3.MaxLength = 250
        Me.cgen3.Name = "cgen3"
        Me.cgen3.Size = New System.Drawing.Size(158, 21)
        Me.cgen3.TabIndex = 56
        Me.cgen3.Tag = "sizable"
        '
        'lgen3
        '
        Me.lgen3.Location = New System.Drawing.Point(99, 250)
        Me.lgen3.Name = "lgen3"
        Me.lgen3.Size = New System.Drawing.Size(105, 24)
        Me.lgen3.TabIndex = 57
        Me.lgen3.Text = "Label1"
        '
        'sgen3
        '
        Me.sgen3.Image = CType(resources.GetObject("sgen3.Image"), System.Drawing.Image)
        Me.sgen3.Location = New System.Drawing.Point(139, 250)
        Me.sgen3.Name = "sgen3"
        Me.sgen3.Size = New System.Drawing.Size(24, 24)
        Me.sgen3.TabIndex = 58
        Me.sgen3.TabStop = False
        '
        'Ccal
        '
        Me.Ccal.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.Ccal.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.Ccal.FormattingEnabled = True
        Me.Ccal.Location = New System.Drawing.Point(88, 208)
        Me.Ccal.MaxLength = 250
        Me.Ccal.Name = "Ccal"
        Me.Ccal.Size = New System.Drawing.Size(158, 21)
        Me.Ccal.TabIndex = 61
        Me.Ccal.Tag = "sizable"
        '
        'Lcal
        '
        Me.Lcal.Location = New System.Drawing.Point(16, 216)
        Me.Lcal.Name = "Lcal"
        Me.Lcal.Size = New System.Drawing.Size(66, 24)
        Me.Lcal.TabIndex = 63
        Me.Lcal.Text = "Lcal"
        '
        'Cmon
        '
        Me.Cmon.Location = New System.Drawing.Point(184, 232)
        Me.Cmon.MaxLength = 250
        Me.Cmon.Name = "Cmon"
        Me.Cmon.Size = New System.Drawing.Size(64, 21)
        Me.Cmon.TabIndex = 64
        '
        'smon
        '
        Me.smon.Image = CType(resources.GetObject("smon.Image"), System.Drawing.Image)
        Me.smon.Location = New System.Drawing.Point(280, 104)
        Me.smon.Name = "smon"
        Me.smon.Size = New System.Drawing.Size(24, 24)
        Me.smon.TabIndex = 65
        Me.smon.TabStop = False
        Me.smon.Tag = "movable"
        '
        'scal
        '
        Me.scal.Image = CType(resources.GetObject("scal.Image"), System.Drawing.Image)
        Me.scal.Location = New System.Drawing.Point(280, 168)
        Me.scal.Name = "scal"
        Me.scal.Size = New System.Drawing.Size(24, 24)
        Me.scal.TabIndex = 66
        Me.scal.TabStop = False
        Me.scal.Tag = "movable"
        '
        'SearchTimer
        '
        Me.SearchTimer.Interval = 1000
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.pnlButtons)
        Me.Panel1.Controls.Add(Me.Copen)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 410)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(309, 48)
        Me.Panel1.TabIndex = 68
        '
        'pnlButtons
        '
        Me.pnlButtons.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlButtons.Controls.Add(Me.bapply)
        Me.pnlButtons.Controls.Add(Me.bclose)
        Me.pnlButtons.Controls.Add(Me.pcandn)
        Me.pnlButtons.Controls.Add(Me.Pdepress)
        Me.pnlButtons.Location = New System.Drawing.Point(195, 3)
        Me.pnlButtons.Name = "pnlButtons"
        Me.pnlButtons.Size = New System.Drawing.Size(114, 42)
        Me.pnlButtons.TabIndex = 69
        '
        'BlueCurve_EntitytSearch
        '
        Me.BlueCurve_EntitytSearch.BackColor = System.Drawing.Color.Transparent
        Me.BlueCurve_EntitytSearch.ExcludeControl = Nothing
        Me.BlueCurve_EntitytSearch.Location = New System.Drawing.Point(22, 211)
        Me.BlueCurve_EntitytSearch.MaximumSize = New System.Drawing.Size(30000, 30000)
        Me.BlueCurve_EntitytSearch.MinimumSize = New System.Drawing.Size(150, 21)
        Me.BlueCurve_EntitytSearch.Name = "BlueCurve_EntitytSearch"
        Me.BlueCurve_EntitytSearch.SearchAttributeList = ""
        Me.BlueCurve_EntitytSearch.SearchAttributes = 2
        Me.BlueCurve_EntitytSearch.SearchBuildEntitiesOnly = True
        Me.BlueCurve_EntitytSearch.SearchClass = 0
        Me.BlueCurve_EntitytSearch.SearchControl = Nothing
        Me.BlueCurve_EntitytSearch.SearchCurrentAttribute = Nothing
        Me.BlueCurve_EntitytSearch.SearchFireEventOnly = False
        Me.BlueCurve_EntitytSearch.SearchGetInactive = False
        Me.BlueCurve_EntitytSearch.SearchText = ""
        Me.BlueCurve_EntitytSearch.SearchTimerOff = True
        Me.BlueCurve_EntitytSearch.SearchUserEntitiesOnly = False
        Me.BlueCurve_EntitytSearch.showinactive = -1
        Me.BlueCurve_EntitytSearch.Size = New System.Drawing.Size(150, 21)
        Me.BlueCurve_EntitytSearch.TabIndex = 67
        Me.BlueCurve_EntitytSearch.Visible = False
        '
        'bc_am_ef_wizard
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(309, 458)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.BlueCurve_EntitytSearch)
        Me.Controls.Add(Me.scal)
        Me.Controls.Add(Me.smon)
        Me.Controls.Add(Me.Cmon)
        Me.Controls.Add(Me.Lcal)
        Me.Controls.Add(Me.Ccal)
        Me.Controls.Add(Me.cgen3)
        Me.Controls.Add(Me.sgen3)
        Me.Controls.Add(Me.lgen3)
        Me.Controls.Add(Me.Cyear)
        Me.Controls.Add(Me.Citem)
        Me.Controls.Add(Me.centity)
        Me.Controls.Add(Me.gdim)
        Me.Controls.Add(Me.speriod)
        Me.Controls.Add(Me.prange)
        Me.Controls.Add(Me.pdate)
        Me.Controls.Add(Me.ssection)
        Me.Controls.Add(Me.Csection)
        Me.Controls.Add(Me.Lsection)
        Me.Controls.Add(Me.scontext)
        Me.Controls.Add(Me.Ccontext)
        Me.Controls.Add(Me.Lcontext)
        Me.Controls.Add(Me.sstage)
        Me.Controls.Add(Me.Cstage)
        Me.Controls.Add(Me.Lstage)
        Me.Controls.Add(Me.slentity)
        Me.Controls.Add(Me.clentity)
        Me.Controls.Add(Me.Llentity)
        Me.Controls.Add(Me.slclass)
        Me.Controls.Add(Me.clclass)
        Me.Controls.Add(Me.llclass)
        Me.Controls.Add(Me.Cperiod)
        Me.Controls.Add(Me.Lperiod)
        Me.Controls.Add(Me.scont)
        Me.Controls.Add(Me.Ccont)
        Me.Controls.Add(Me.Lcont)
        Me.Controls.Add(Me.sgen2)
        Me.Controls.Add(Me.Lgen2)
        Me.Controls.Add(Me.sgen1)
        Me.Controls.Add(Me.Cgen1)
        Me.Controls.Add(Me.Lgen1)
        Me.Controls.Add(Me.Cfunction)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.syear)
        Me.Controls.Add(Me.Lyear)
        Me.Controls.Add(Me.sitem)
        Me.Controls.Add(Me.Litem)
        Me.Controls.Add(Me.sentity)
        Me.Controls.Add(Me.Lentity)
        Me.Controls.Add(Me.sclass)
        Me.Controls.Add(Me.Cclass)
        Me.Controls.Add(Me.Lclass)
        Me.Controls.Add(Me.pdate_from)
        Me.Controls.Add(Me.Thelp)
        Me.Controls.Add(Me.uxTypeNameHelp)
        Me.Controls.Add(Me.Bhelp)
        Me.Controls.Add(Me.Cgen2)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "bc_am_ef_wizard"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Tag = "movable"
        Me.Text = "Blue Curve Excel Functions v5"
        CType(Me.sclass, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.sentity, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.sitem, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.syear, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bapply, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bclose, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gdim.ResumeLayout(False)
        CType(Me.scont, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.sgen2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.sgen1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.scontext, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.sstage, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.slentity, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.slclass, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ssection, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pdate.ResumeLayout(False)
        Me.pdate.PerformLayout()
        CType(Me.sdate, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pdate_from.ResumeLayout(False)
        Me.pdate_from.PerformLayout()
        CType(Me.sdate_from, System.ComponentModel.ISupportInitialize).EndInit()
        Me.prange.ResumeLayout(False)
        CType(Me.send, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.sstart, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.speriod, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Pdepress, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pcandn, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxTypeNameHelp, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.sgen3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.smon, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.scal, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.pnlButtons.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region
    Public activecell As Object
    Public tx As String
    Public cx As String
    Public Function_name As String
    Private FilterContolItems As New ArrayList
    Private EntityUpdating As Boolean = False

    Private Declare Function SendMessage Lib "user32" Alias "SendMessageA" (ByVal hwnd As IntPtr, ByVal wMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer

    Private Const WM_SETREDRAW As Integer = &HB

    Private formHeight As Integer = 496

    Private Sub bc_am_ef_wizard_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim cy As Integer
        Dim i As Integer
        Me.MinimumSize = Nothing
        Me.Height = 140
        Me.MinimumSize = New Size(500, Me.Height)
        For i = 0 To bc_am_ef_functions.excel_functions.Count - 1
            If bc_am_ef_functions.excel_functions(i).show_in_wizard > 0 Then
                Me.Cfunction.Items.Add(bc_am_ef_functions.excel_functions(i).display_name)
            End If
        Next
        Me.Cclass.Items.Clear()
        Me.clclass.Items.Clear()
        For i = 0 To bc_am_ef_functions.class_names.Count - 1
            Me.Cclass.Items.Add(bc_am_ef_functions.class_names(i))
            Me.clclass.Items.Add(bc_am_ef_functions.class_names(i))
        Next
        Me.Cstage.Items.Clear()
        For i = 0 To bc_am_ef_functions.stage_names.Count - 1
            Me.Cstage.Items.Add(bc_am_ef_functions.stage_names(i))
        Next
        REM fix july 200
        If bc_am_ef_functions.ousersettings.stages <> 0 Then
            Me.Cstage.Enabled = False
            If bc_am_ef_functions.ousersettings.stages = 1 Then
                Me.Cstage.SelectedIndex = 0
            Else
                Me.Cstage.SelectedIndex = 1
            End If
        Else
            Me.Cstage.SelectedIndex = bc_am_ef_functions.ousersettings.default_stage
        End If
        Me.Ccont.Items.Clear()
        For i = 0 To bc_am_ef_functions.contributor_names.Count - 1
            Me.Ccont.Items.Add(bc_am_ef_functions.contributor_names(i))
        Next
        Me.Cperiod.Items.Clear()
        For i = 0 To bc_am_ef_functions.period_names.Count - 1
            Me.Cperiod.Items.Add(bc_am_ef_functions.period_names(i))
        Next

        Me.Citem.Items.Clear()
        For i = 0 To bc_am_ef_functions.item_names.Count - 1
            Me.Citem.Items.Add(bc_am_ef_functions.item_names(i))
        Next
        cy = Year(Now())
        Me.Cyear.Items.Clear()
        For i = 0 To 20
            Me.Cyear.Items.Add(CStr(cy - 15 + i))
        Next
        Me.Ccal.Anchor = AnchorStyles.Top + AnchorStyles.Left
        Me.Cmon.Anchor = AnchorStyles.Top + AnchorStyles.Left
        Me.Ccal.Tag = ""
        Me.Cmon.Tag = ""
        Me.scal.Tag = ""
        Me.Ccal.Items.Clear()
        Me.Ccal.Items.Add("Fiscal")
        Me.Ccal.Items.Add("Calenderized")
        Me.Ccal.SelectedIndex = 1
        Me.Cmon.Items.Clear()
        Me.Cmon.Items.Add("Jan")
        Me.Cmon.Items.Add("Feb")
        Me.Cmon.Items.Add("Mar")
        Me.Cmon.Items.Add("Apr")
        Me.Cmon.Items.Add("May")
        Me.Cmon.Items.Add("Jun")
        Me.Cmon.Items.Add("Jul")
        Me.Cmon.Items.Add("Aug")
        Me.Cmon.Items.Add("Sep")
        Me.Cmon.Items.Add("Oct")
        Me.Cmon.Items.Add("Nov")
        Me.Cmon.Items.Add("Dec")
        Me.Cmon.Enabled = False
        Me.Cfunction.Enabled = True
        Me.Thelp.Visible = False
        Me.Bhelp.Enabled = False
        Me.Copen.Enabled = False
        load_params()
        Me.bapply.Enabled = False
        function_in_cell()
        If Me.Function_name <> "Then" Then
            Me.Cfunction.Text = Function_name
        End If

        BlueCurve_EntitytSearch.SearchSetup(False)
        













        resizeForm()

    End Sub
    Private Sub function_in_cell()

        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")
        Try
            Dim i As Integer
            tx = activecell.formula
            cx = tx
            If InStr(tx, "=bc_") > 0 Then
                tx = tx.Substring(1, InStr(tx, "(") - 2)
                cx = cx.Substring(InStr(cx, "("))
                For i = 0 To bc_am_ef_functions.excel_functions.Count - 1
                    If bc_am_ef_functions.excel_functions(i).name = tx Then
                        Me.Cfunction.Text = bc_am_ef_functions.excel_functions(i).display_name
                        Me.Cfunction.Enabled = False
                        Exit For
                    End If
                Next
            Else
                tx = ""
                cx = ""
            End If
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
        End Try

    End Sub
    Public Sub set_values()
        Try
            Dim nx As String
            Dim i, j, k As Integer
            For i = 0 To bc_am_ef_functions.excel_functions.Count - 1
                If bc_am_ef_functions.excel_functions(i).name = tx Then
                    REM now set parameter values
                    'cx = cx.Replace("""", "")
                    For j = 0 To bc_am_ef_functions.excel_functions(i).params.count - 1

                        If InStr(cx, ",") > 0 Then
                            If j > 0 Then
                                cx = cx.Substring(InStr(cx, ","), cx.Length - (InStr(cx, ",")))
                            End If
                            If InStr(cx, ",") > 0 Then
                                nx = cx.Substring(0, InStr(cx, ",") - 1)
                            Else
                                nx = cx.Substring(0, cx.Length - 1)
                            End If
                        Else
                            nx = cx.Substring(0, cx.Length - 1)
                        End If
                        If bc_am_ef_functions.excel_functions(i).params(j).name = "range" Then
                            Me.Crange.SelectedIndex = 0
                            Me.Cstart.Text = ""
                            Me.cend.Text = ""
                            If nx <> "" Then
                                Me.Crange.SelectedIndex = 1
                                nx = nx.Replace("""", "")
                                Me.Cstart.Text = nx
                            End If
                            cx = cx.Substring(InStr(cx, ","), cx.Length - (InStr(cx, ",")))
                            nx = cx.Substring(0, InStr(cx, ",") - 1)
                            If nx <> "" Then
                                Me.Crange.SelectedIndex = 2
                                nx = nx.Replace("""", "")
                                Me.Cstart.Text = nx
                            End If
                            cx = cx.Substring(InStr(cx, ","), cx.Length - (InStr(cx, ",")))
                            nx = cx.Substring(0, InStr(cx, ",") - 1)
                            If nx <> "" Then
                                If Me.Cstart.Text = "" Then
                                    nx = nx.Replace("""", "")
                                    Me.Cstart.Text = nx
                                    Me.Crange.SelectedIndex = 3
                                Else
                                    Me.Crange.SelectedIndex = 4
                                    nx = nx.Replace("""", "")
                                    Me.cend.Text = nx
                                End If
                            End If

                        ElseIf bc_am_ef_functions.excel_functions(i).params(j).name = "context" Then
                            nx = nx.Replace("""", "")
                            Me.Ccontext.Text = nx
                        ElseIf bc_am_ef_functions.excel_functions(i).params(j).name = "section" Then
                            nx = nx.Replace("""", "")
                            Me.Csection.Text = nx
                        ElseIf bc_am_ef_functions.excel_functions(i).params(j).name = "gen1" Then
                            nx = nx.Replace("""", "")
                            Me.Cgen1.Text = nx
                        ElseIf bc_am_ef_functions.excel_functions(i).params(j).name = "gen2" Then
                            nx = nx.Replace("""", "")
                            Me.Cgen2.Text = nx
                        ElseIf bc_am_ef_functions.excel_functions(i).params(j).name = "gen3" Then
                            nx = nx.Replace("""", "")
                            Me.cgen3.Text = nx
                        ElseIf bc_am_ef_functions.excel_functions(i).params(j).name = "class" Then
                            nx = nx.Replace("""", "")
                            Me.Cclass.Text = nx
                        ElseIf bc_am_ef_functions.excel_functions(i).params(j).name = "entity" Then

                            'nx = nx.Replace("""", "")
                            'Me.centity.Text = nx
                            Dim dx, gx As String

                            dx = cx.Substring(InStr(cx, ","), cx.Length - (InStr(cx, ",")))

                            If InStr(dx, ",") > 0 Then
                                gx = dx.Substring(0, InStr(dx, ",") - 1)

                                If gx.Trim.Substring(0, 1) <> """" And IsNumeric(gx) = False And is_formula(gx) = False Then
                                    cx = cx.Substring(InStr(cx, ","), cx.Length - (InStr(cx, ",")))
                                    nx = nx + "," + gx
                                End If
                        End If
                        nx = nx.Replace("""", "")
                        Me.centity.Text = nx

                        ElseIf bc_am_ef_functions.excel_functions(i).params(j).name = "lclass" Then

                            nx = nx.Replace("""", "")
                            Me.clclass.Text = nx
                            ElseIf bc_am_ef_functions.excel_functions(i).params(j).name = "cal" Then
                        nx = nx.Replace("""", "")
                        Me.Ccal.Text = nx
                        If InStr(cx, ",") > 0 Then
                            If j > 0 Then
                                cx = cx.Substring(InStr(cx, ","), cx.Length - (InStr(cx, ",")))
                            End If
                            If InStr(cx, ",") > 0 Then
                                nx = cx.Substring(0, InStr(cx, ",") - 1)
                            Else
                                nx = cx.Substring(0, cx.Length - 1)
                            End If
                        Else
                            nx = cx.Substring(0, cx.Length - 1)
                        End If
                        nx = nx.Replace("""", "")
                        Me.Cmon.Text = nx




                            ElseIf bc_am_ef_functions.excel_functions(i).params(j).name = "lentity" Then

                        nx = nx.Replace("""", "")
                        Me.clentity.Text = nx
                        ElseIf bc_am_ef_functions.excel_functions(i).params(j).name = "item" Then

                            Dim dx, gx As String
                            dx = cx.Substring(InStr(cx, ","), cx.Length - (InStr(cx, ",")))
                            If InStr(dx, ",") > 0 Then
                                gx = dx.Substring(0, InStr(dx, ",") - 1)
                                If Len(gx) > 0 Then
                                    If gx.Trim.Substring(0, 1) <> """" And IsNumeric(gx) = False And is_formula(gx) = False Then
                                        cx = cx.Substring(InStr(cx, ","), cx.Length - (InStr(cx, ",")))
                                        nx = nx + "," + gx
                                    End If
                                End If
                            End If

                            nx = nx.Replace("""", "")
                            Me.Citem.Text = nx
                            ElseIf bc_am_ef_functions.excel_functions(i).params(j).name = "year" Then
                                nx = nx.Replace("""", "")
                                Me.Cyear.Text = nx
                            ElseIf bc_am_ef_functions.excel_functions(i).params(j).name = "period" Then
                                nx = nx.Replace("""", "")
                                Me.Cperiod.Text = nx
                            ElseIf bc_am_ef_functions.excel_functions(i).params(j).name = "contributor" Then
                                nx = nx.Replace("""", "")
                                Me.Ccont.Text = nx
                            ElseIf bc_am_ef_functions.excel_functions(i).params(j).name = "stage" Then
                                nx = nx.Replace("""", "")
                                Me.Cstage.Text = nx
                            ElseIf bc_am_ef_functions.excel_functions(i).params(j).name = "date_at" Then
                                nx = nx.Replace("""", "")
                                If IsDate(nx) = True Then
                                    If CDate(nx) = "9-9-9999" Then
                                        Me.Chkdate.Checked = True
                                        Me.tdate.Text = ""
                                    Else
                                        Me.tdate.Text = nx
                                        Me.Chkdate.Checked = False
                                    End If
                                ElseIf nx = "" Then
                                    Me.Chkdate.Checked = True
                                    Me.tdate.Text = ""
                                Else
                                    Me.tdate.Text = nx
                                    Me.Chkdate.Checked = False
                                End If
                            ElseIf bc_am_ef_functions.excel_functions(i).params(j).name = "date_from" Then
                                nx = nx.Replace("""", "")
                                If IsDate(nx) = True Then
                                    If CDate(nx) = "1-1-1900" Then
                                        Me.chkdate_from.Checked = True
                                        Me.tdate_from.Text = ""
                                    Else
                                        Me.tdate_from.Text = nx
                                        Me.chkdate_from.Checked = False
                                    End If
                                ElseIf nx = "" Then
                                    Me.chkdate_from.Checked = True
                                    Me.tdate_from.Text = ""
                                Else
                                    Me.tdate_from.Text = nx
                                    Me.chkdate_from.Checked = False
                                End If
                            End If

                    Next
                    REM now do dimensions
                    While InStr(cx, ",") > 0
                        cx = cx.Replace("""", "")
                        cx = cx.Substring(InStr(cx, ","), cx.Length - (InStr(cx, ",")))
                        If InStr(cx, ",") > 0 Then
                            nx = cx.Substring(0, InStr(cx, ",") - 1)
                        Else
                            nx = cx.Substring(0, cx.Length - 1)
                        End If
                        For k = 0 To Me.Lalldim.Items.Count - 1
                            If Me.Lalldim.Items(k) = nx Then
                                Me.Lalldim.Items.RemoveAt(k)
                                Exit For
                            End If
                        Next
                        Dim found As Boolean = False
                        For k = 0 To Me.Lseldim.Items.Count - 1
                            If Me.Lseldim.Items(k) = nx Then
                                found = True
                                Exit For
                            End If
                        Next
                        If found = False Then
                            If (InStr(nx, ")") = 0) Then
                                Me.Lseldim.Items.Add(nx)
                            End If
                        End If

                    End While
                End If
            Next

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_ef_wizard", "set_values", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Private Sub load_params(Optional ByVal default_load As Boolean = True, Optional ByVal st As Double = 0)
        Try
            'SendMessage(Me.Handle, WM_SETREDRAW, 0, 0)
            Dim i, j, k As Integer
            Me.Cclass.Visible = False
            Me.clclass.Visible = False
            Me.slclass.Visible = False
            Me.llclass.Visible = False
            Me.Lcal.Visible = False
            Me.scal.Visible = False
            Me.Ccal.Visible = False
            Me.centity.Visible = False
            Me.clentity.Visible = False
            Me.slentity.Visible = False
            Me.Llentity.Visible = False
            Me.Cstage.Visible = False
            Me.Ccont.Visible = False
            Me.Citem.Visible = False
            Me.Lclass.Visible = False
            Me.Lentity.Visible = False
            Me.Lstage.Visible = False
            Me.Lcont.Visible = False
            Me.Litem.Visible = False
            Me.gdim.Visible = False
            Me.pdate.Visible = False
            Me.pdate_from.Visible = False
            Me.Cyear.Visible = False
            Me.Cperiod.Visible = False
            Me.Lyear.Visible = False
            Me.Lperiod.Visible = False
            Me.sclass.Visible = False
            Me.sentity.Visible = False
            Me.sitem.Visible = False
            Me.syear.Visible = False
            Me.speriod.Visible = False
            Me.smon.Visible = False
            Me.Cmon.Visible = False
            Me.sstage.Visible = False
            Me.scont.Visible = False
            Me.sgen1.Visible = False
            Me.sgen2.Visible = False
            Me.sgen3.Visible = False
            Me.Cgen1.Visible = False
            Me.Cgen2.Visible = False
            Me.cgen3.Visible = False
            Me.Lgen1.Visible = False
            Me.Lgen2.Visible = False
            Me.lgen3.Visible = False
            Me.Ccontext.Visible = False
            Me.Csection.Visible = False
            Me.Lcontext.Visible = False
            Me.Lsection.Visible = False
            Me.scontext.Visible = False
            Me.ssection.Visible = False

            Me.Ccontext.Enabled = False
            Me.Csection.Enabled = False
            Me.Lcontext.Enabled = False
            Me.Lsection.Enabled = False
            Me.scontext.Enabled = False
            Me.ssection.Enabled = False
            Me.Lto.Visible = False
            Me.prange.Visible = False
            Me.cend.Enabled = False
            Dim dp As System.Drawing.Point
            dp.X = 94
            dp.Y = 58 + st
            Dim ldp As System.Drawing.Point
            ldp.X = 15
            ldp.Y = 58 + st
            Dim sdp As System.Drawing.Point
            sdp.X = Me.Width - 43
            sdp.Y = 58 + st

            Cclass.Text = ""
            centity.Text = ""
            centity.Items.Clear()

            If Me.Cfunction.SelectedIndex = -1 Then
                Exit Sub
            End If

            For i = 0 To bc_am_ef_functions.excel_functions.Count - 1
                If bc_am_ef_functions.excel_functions(i).display_name = Me.Cfunction.Text Then
                    Me.Thelp.Text = bc_am_ef_functions.excel_functions(i).helptext
                    For j = 0 To bc_am_ef_functions.excel_functions(i).params.count - 1
                        If bc_am_ef_functions.excel_functions(i).params(j).name = "range" Then
                            Me.prange.Visible = True
                            Me.Crange.Items.Clear()
                            Me.Crange.Items.Add("All")
                            Me.Crange.Items.Add("=")
                            Me.Crange.Items.Add(">")
                            Me.Crange.Items.Add("<")
                            Me.Crange.Items.Add("between")
                            For k = 0 To bc_am_ef_functions.excel_functions(i).params(j).lookup_vals.count - 1
                                Me.Crange.Items.Add(bc_am_ef_functions.excel_functions(i).params(j).lookup_vals(k))
                            Next
                            If default_load = True Then
                                Me.Crange.SelectedIndex = 0
                            End If
                            Me.prange.Location = ldp
                            Me.Lrange.Text = bc_am_ef_functions.excel_functions(i).params(j).display_name
                            Me.Crange.TabIndex = j + 2
                            dp.Y = dp.Y + 27
                            ldp.Y = ldp.Y + 27
                            sdp.Y = sdp.Y + 27
                        End If
                        If bc_am_ef_functions.excel_functions(i).params(j).name = "context" Then
                            Me.Ccontext.Visible = True
                            Me.Lcontext.Visible = True
                            Me.scontext.Visible = True
                            Me.Ccontext.Items.Clear()
                            Me.Ccontext.Items.Add("Excel Submission")
                            Me.Ccontext.Items.Add("Calculated")
                            For k = 0 To bc_am_ef_functions.excel_functions(i).params(j).lookup_vals.count - 1
                                Me.Ccontext.Items.Add(bc_am_ef_functions.excel_functions(i).params(j).lookup_vals(k))
                            Next
                            If default_load = True Then
                                Me.Ccontext.SelectedIndex = -1
                            End If
                            Me.Ccontext.Location = dp
                            Me.Lcontext.Location = ldp
                            Me.scontext.Location = sdp
                            Me.Lcontext.Text = bc_am_ef_functions.excel_functions(i).params(j).display_name
                            Me.Ccontext.TabIndex = j + 2

                        End If
                        If bc_am_ef_functions.excel_functions(i).params(j).name = "section" Then
                            Me.Csection.Visible = True
                            Me.Lsection.Visible = True
                            Me.ssection.Visible = True
                            Me.Csection.Items.Clear()
                            Me.Csection.Items.Add("All")
                            For k = 0 To bc_am_ef_functions.excel_functions(i).params(j).lookup_vals.count - 1
                                Me.Csection.Items.Add(bc_am_ef_functions.excel_functions(i).params(j).lookup_vals(k))
                            Next
                            If default_load = True Then
                                Me.Csection.SelectedIndex = -1
                            End If
                            Me.Csection.Location = dp
                            Me.Lsection.Location = ldp
                            Me.ssection.Location = sdp
                            Me.Lsection.Text = bc_am_ef_functions.excel_functions(i).params(j).display_name
                            Me.Csection.TabIndex = j + 2
                        End If
                        If bc_am_ef_functions.excel_functions(i).params(j).name = "gen1" Then
                            Me.Cgen1.Visible = True
                            Me.Lgen1.Visible = True
                            Me.sgen1.Visible = True
                            Me.Cgen1.Text = ""
                            Me.Cgen1.Items.Clear()
                            For k = 0 To bc_am_ef_functions.excel_functions(i).params(j).lookup_vals.count - 1
                                Me.Cgen1.Items.Add(bc_am_ef_functions.excel_functions(i).params(j).lookup_vals(k))
                            Next
                            If default_load = True Then
                                Me.Cgen1.SelectedIndex = -1
                                If bc_am_ef_functions.excel_functions(i).params(j).has_default = True Then
                                    Try
                                        Me.Cgen1.SelectedIndex = bc_am_ef_functions.excel_functions(i).params(j).default_value
                                    Catch
                                        Me.Cgen1.Text = bc_am_ef_functions.excel_functions(i).params(j).default_value
                                    End Try
                                End If
                            End If

                            Me.Cgen1.Location = dp
                            Me.Lgen1.Location = ldp
                            Me.sgen1.Location = sdp
                            Me.Lgen1.Text = bc_am_ef_functions.excel_functions(i).params(j).display_name.ToString.Substring(0, 1).ToUpper + bc_am_ef_functions.excel_functions(i).params(j).display_name.ToString.Substring(1).ToLower
                            Me.Cgen1.TabIndex = j + 2
                        End If
                        If bc_am_ef_functions.excel_functions(i).params(j).name = "gen2" Then
                            Me.Cgen2.Visible = True
                            Me.Lgen2.Visible = True
                            Me.sgen2.Visible = True
                            Me.Cgen2.Items.Clear()

                            For k = 0 To bc_am_ef_functions.excel_functions(i).params(j).lookup_vals.count - 1
                                Me.Cgen2.Items.Add(bc_am_ef_functions.excel_functions(i).params(j).lookup_vals(k))
                            Next
                            If default_load = True Then
                                Me.Cgen2.SelectedIndex = -1
                                If bc_am_ef_functions.excel_functions(i).params(j).has_default = True Then
                                    Try
                                        Me.Cgen2.SelectedIndex = bc_am_ef_functions.excel_functions(i).params(j).default_value
                                    Catch ex As Exception
                                        Me.Cgen2.Text = bc_am_ef_functions.excel_functions(i).params(j).default_value
                                    End Try
                                End If
                            End If

                            Me.Cgen2.Location = dp
                            Me.Lgen2.Location = ldp
                            Me.sgen2.Location = sdp
                            Me.Lgen2.Text = bc_am_ef_functions.excel_functions(i).params(j).display_name.ToString.Substring(0, 1).ToUpper & bc_am_ef_functions.excel_functions(i).params(j).display_name.ToString.Substring(1).ToLower
                            Me.Cgen2.TabIndex = j + 2
                        End If
                        If bc_am_ef_functions.excel_functions(i).params(j).name = "gen3" Then
                            Me.cgen3.Visible = True
                            Me.lgen3.Visible = True
                            Me.sgen3.Visible = True
                            Me.cgen3.Items.Clear()

                            For k = 0 To bc_am_ef_functions.excel_functions(i).params(j).lookup_vals.count - 1
                                Me.cgen3.Items.Add(bc_am_ef_functions.excel_functions(i).params(j).lookup_vals(k))
                            Next
                            If default_load = True Then
                                Me.cgen3.SelectedIndex = -1
                                If bc_am_ef_functions.excel_functions(i).params(j).has_default = True Then
                                    Try
                                        Me.cgen3.SelectedIndex = bc_am_ef_functions.excel_functions(i).params(j).default_value
                                    Catch

                                    End Try
                                End If
                            End If

                            Me.cgen3.Location = dp
                            Me.lgen3.Location = ldp
                            Me.sgen3.Location = sdp
                            Me.lgen3.Text = bc_am_ef_functions.excel_functions(i).params(j).display_name
                            Me.cgen3.TabIndex = j + 2
                        End If
                        If bc_am_ef_functions.excel_functions(i).params(j).name = "class" Then
                            Me.Cclass.Visible = True
                            Me.Lclass.Visible = True
                            Me.sclass.Visible = True
                            If default_load = True Then
                                Me.Cclass.SelectedIndex = -1
                            End If
                            Me.Cclass.Location = dp
                            Me.Lclass.Location = ldp
                            Me.sclass.Location = sdp
                            Me.Lclass.Text = bc_am_ef_functions.excel_functions(i).params(j).display_name
                            Me.Cclass.TabIndex = j + 2
                        End If
                        If bc_am_ef_functions.excel_functions(i).params(j).name = "lclass" Then
                            Me.clclass.Visible = True
                            Me.llclass.Visible = True
                            Me.slclass.Visible = True
                            If default_load = True Then
                                Me.clclass.SelectedIndex = -1
                            End If
                            Me.clclass.Location = dp
                            Me.llclass.Location = ldp
                            Me.slclass.Location = sdp
                            Me.llclass.Text = bc_am_ef_functions.excel_functions(i).params(j).display_name
                            Me.clclass.TabIndex = j + 2
                        End If
                        If bc_am_ef_functions.excel_functions(i).params(j).name = "entity" Then
                            Me.centity.Visible = True
                            Me.Lentity.Visible = True
                            Me.sentity.Visible = True
                            If default_load = True Then
                                Me.centity.SelectedIndex = -1
                            End If
                            Me.centity.Location = dp
                            Me.Lentity.Location = ldp
                            Me.sentity.Location = sdp
                            Me.Lentity.Text = bc_am_ef_functions.excel_functions(i).params(j).display_name
                            Me.centity.TabIndex = j + 2
                        End If
                        If bc_am_ef_functions.excel_functions(i).params(j).name = "cal" Then
                            Me.Ccal.Visible = True

                            Me.Cmon.Width = 100

                            Me.Ccal.Width = Me.Cmon.Width
                            Me.Lcal.Visible = True
                            Me.scal.Visible = True
                            Me.Cmon.Visible = True
                            Me.smon.Visible = True
                            If default_load = True Then
                                Me.Ccal.SelectedIndex = 0
                            End If
                            Dim adp As System.Drawing.Point
                            adp.Y = dp.Y
                            Me.Ccal.Location = dp
                            Me.Lcal.Location = ldp
                            adp.X = dp.X + Ccal.Width + 3
                            Me.scal.Location = adp
                            adp.X = adp.X + scal.Width + 3
                            Me.Cmon.Location = adp
                            Me.smon.Location = sdp
                            Me.Lcal.Text = bc_am_ef_functions.excel_functions(i).params(j).display_name
                            Me.Ccal.TabIndex = j + 2
                        End If
                        If bc_am_ef_functions.excel_functions(i).params(j).name = "lentity" Then
                            Me.clentity.Visible = True
                            Me.Llentity.Visible = True
                            Me.slentity.Visible = True
                            If default_load = True Then
                                Me.clentity.SelectedIndex = -1
                            End If
                            Me.clentity.Location = dp
                            Me.Llentity.Location = ldp
                            Me.slentity.Location = sdp
                            Me.Llentity.Text = bc_am_ef_functions.excel_functions(i).params(j).display_name
                            Me.clentity.TabIndex = j + 2
                        End If

                        If bc_am_ef_functions.excel_functions(i).params(j).name = "item" Then
                            Me.Citem.Visible = True
                            Me.Litem.Visible = True
                            Me.sitem.Visible = True
                            Me.Citem.Text = ""
                            Me.Citem.Items.Clear()
                            For k = 0 To bc_am_ef_functions.item_names.Count - 1
                                Me.Citem.Items.Add(bc_am_ef_functions.item_names(k))
                            Next
                            If default_load = True Then
                                Me.Citem.SelectedIndex = -1
                            End If
                            Me.Citem.Location = dp
                            Me.Litem.Location = ldp
                            Me.sitem.Location = sdp
                            Me.Litem.Text = bc_am_ef_functions.excel_functions(i).params(j).display_name
                            Me.Citem.TabIndex = j + 2
                        End If
                        If bc_am_ef_functions.excel_functions(i).params(j).name = "contributor" Then
                            Me.Ccont.Visible = True
                            Me.Lcont.Visible = True
                            Me.scont.Visible = True
                            If default_load = True Then
                                If Me.Ccont.Items.Count > 0 Then
                                    Me.Ccont.SelectedIndex = 0
                                End If
                            End If
                            Me.Ccont.Location = dp
                            Me.Lcont.Location = ldp
                            Me.scont.Location = sdp
                            Me.Ccont.TabIndex = j + 2
                            Me.Lcont.Text = bc_am_ef_functions.excel_functions(i).params(j).display_name
                        End If
                        If bc_am_ef_functions.excel_functions(i).params(j).name = "stage" Then
                            Me.Cstage.Visible = True
                            Me.Lstage.Visible = True
                            Me.sstage.Visible = True
                            If default_load = True Then
                                REM PR JULY 2010
                                'If Me.Cstage.Items.Count > 0 Then
                                '    Me.Cstage.SelectedIndex = 0
                                'End If
                            End If
                            Me.Cstage.Location = dp
                            Me.Lstage.Location = ldp
                            Me.sstage.Location = sdp
                            Me.Lstage.Text = bc_am_ef_functions.excel_functions(i).params(j).display_name
                            Me.Cstage.TabIndex = j + 2
                        End If
                        If bc_am_ef_functions.excel_functions(i).params(j).name = "year" Then
                            Me.Cyear.Visible = True
                            Me.Lyear.Visible = True
                            Me.syear.Visible = True
                            If default_load = True Then
                                Me.Cyear.Text = CStr(Year(Now()))
                            End If
                            Me.Cyear.Location = dp
                            Me.Lyear.Location = ldp
                            Me.syear.Location = sdp
                            Me.Cyear.Enabled = True
                            Me.syear.Enabled = True
                            Me.Lyear.Text = bc_am_ef_functions.excel_functions(i).params(j).display_name
                            Me.Cyear.TabIndex = j + 2
                        End If
                        If bc_am_ef_functions.excel_functions(i).params(j).name = "period" Then
                            Me.Cperiod.Visible = True
                            Me.Lperiod.Visible = True
                            Me.speriod.Visible = True
                            If default_load = True Then
                                Me.Cperiod.SelectedIndex = -1
                            End If
                            Me.Cperiod.Location = dp
                            Me.Lperiod.Location = ldp
                            Me.speriod.Location = sdp
                            Me.Cperiod.Enabled = True
                            Me.speriod.Enabled = True

                            If Me.Cperiod.Items.Count > 0 Then
                                Me.Cperiod.SelectedIndex = 0
                            End If
                            Me.Lperiod.Text = bc_am_ef_functions.excel_functions(i).params(j).display_name
                            Me.Cperiod.TabIndex = j + 2
                        End If
                        If bc_am_ef_functions.excel_functions(i).params(j).name = "date_at" Then
                            Me.pdate.Visible = True
                            Me.Chkdate.Checked = True
                            Me.pdate.Location = ldp
                            Me.sdate.Visible = False
                            Me.ldate.Text = bc_am_ef_functions.excel_functions(i).params(j).display_name
                            Me.tdate.TabIndex = j + 2
                        End If
                        If bc_am_ef_functions.excel_functions(i).params(j).name = "date_from" Then
                            Me.pdate_from.Visible = True
                            Me.chkdate_from.Checked = True
                            Me.pdate_from.Location = ldp
                            Me.sdate_from.Visible = False
                            Me.Ldate_from.Text = bc_am_ef_functions.excel_functions(i).params(j).display_name
                            Me.tdate_from.TabIndex = j + 2
                        End If
                        dp.Y = dp.Y + 27
                        ldp.Y = ldp.Y + 27
                        sdp.Y = sdp.Y + 27
                    Next
                    REM now do dimensions
                    Me.Lseldim.Items.Clear()
                    Me.Lalldim.Items.Clear()
                    Me.gdim.Visible = False
                    If bc_am_ef_functions.excel_functions(i).dims.count > 0 Then
                        Dim fdp As System.Drawing.Point
                        fdp.X = 0
                        fdp.Y = ldp.Y
                        Me.gdim.Location = fdp
                        ldp.Y = ldp.Y + 23
                        Me.gdim.Visible = True
                        For j = 0 To bc_am_ef_functions.excel_functions(i).dims.count - 1
                            If bc_am_ef_functions.excel_functions(i).dims(j).ord = 0 Then
                                Me.Lalldim.Items.Add(bc_am_ef_functions.excel_functions(i).dims(j).name)
                            Else
                                Me.Lseldim.Items.Add(bc_am_ef_functions.excel_functions(i).dims(j).name)
                            End If
                        Next
                        Exit For
                    End If
                End If
            Next
            Me.MinimumSize = Nothing
            Me.MaximumSize = Nothing
            Me.Height = ldp.Y + 80
            Me.MinimumSize = New Size(500, Me.Height)
            'ResizeControls(Me)
            formHeight = Me.Height
            SetTabStops()
            'SendMessage(Me.Handle, WM_SETREDRAW, 1, 0)
            'Me.Refresh()
            'activecell.application.refreshall()
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_ef_wizard", "load_params", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub

    Private Sub ListControls(ByRef al As ArrayList, ByVal c As Control, ByRef intCount As Integer)
        For Each child As Control In c.Controls
            If child.Visible Then
                ListControls(al, child, intCount)
                al.Add(child)
                intCount += 1
            End If
        Next
    End Sub

    Private Sub SetTabStops()

        Dim intCount As Integer = 0
        Dim al As New ArrayList
        ListControls(al, Me, intCount)
        Dim controlsToBeSorted(intCount - 1) As Control
        controlsToBeSorted = New Control(intCount - 1) {}
        intCount = 0
        For Each c As Control In al
            controlsToBeSorted(intCount) = al.Item(intCount)
            intCount += 1
        Next
        Dim intX As Integer, intY As Integer, controlTemp As Control
        For intX = 0 To controlsToBeSorted.Length - 1
            For intY = 0 To controlsToBeSorted.Length - 2 - intX
                If CompareControls(controlsToBeSorted(intY), controlsToBeSorted(intY + 1)) Then
                    ' MsgBox(2.2)
                    controlTemp = controlsToBeSorted(intY)
                    controlsToBeSorted(intY) = controlsToBeSorted(intY + 1)
                    controlsToBeSorted(intY + 1) = controlTemp
                    ' MsgBox(2.3)
                End If
            Next
        Next
        intCount = 0
        While intCount < controlsToBeSorted.Length
            controlsToBeSorted(intCount).TabIndex = 3 + intCount
            controlsToBeSorted(intCount).Refresh()
            'controlsToBeSorted(intCount).Text = controlsToBeSorted(intCount).TabIndex
            intCount += 1
        End While

    End Sub

    Private Function CompareControls(ByRef c1 As Control, ByRef c2 As Control) As Boolean
        'MsgBox(2.1)
        If c1 Is Nothing Then
            Return False
        ElseIf c2 Is Nothing Then
            Return True
        End If
        Dim intX1, intY1, intX2, intY2 As Integer
        intX1 = 0
        intX2 = 0
        intY1 = 0
        intY2 = 0
        If TypeOf c1.Parent Is Panel Then
            intX1 = c1.Parent.Location.X
            intY1 = c1.Parent.Location.Y
        End If
        If TypeOf c2.Parent Is Panel Then
            intX2 = c2.Parent.Location.X
            intY2 = c2.Parent.Location.Y
        End If
        intX1 += c1.Location.X
        intY1 += c1.Location.Y
        intX2 += c2.Location.X
        intY2 += c2.Location.Y
        If intY1 > intY2 Then
            Return True
        ElseIf intY1 = intY2 Then
            Return intX1 > intX2
        Else
            Return False
        End If
    End Function

    Private Sub cfunction_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cfunction.SelectedIndexChanged
        Try
            If Me.Cfunction.SelectedIndex = -1 Then
                Exit Sub
            End If

            Me.Bhelp.Enabled = True
            Me.uxTypeNameHelp.Enabled = True

            Me.rdef.Checked = True

            Me.Badddim.Enabled = False
            Me.Bdeldim.Enabled = False
            Me.Thelp.Visible = False
            Me.bapply.Enabled = True
            Me.Copen.Enabled = True
            If Me.tx <> "" Then

                load_params(False, 0)
                If Me.tx <> "" Then
                    Me.set_values()
                    tx = ""
                    cx = ""
                End If
            Else

                load_params(True, 0)
            End If
            If gdim.Visible Then
                Me.Rch.Checked = True
            Else
                Me.MaximumSize = New Size(800, Me.Height)
            End If
            

        Catch ex As Exception

            Dim oerr As New bc_cs_error_log("bc_am_ef_wizard", "selectedIndexChanged", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try
    End Sub


    Private Function build_function() As String
        Dim i, j As Integer
        Dim fm As String
        Dim dims As String
        Dim ti As String
        Dim sti As String
        Dim eti As String
        Dim tm As String
        eti = ""
        sti = ""
        fm = ""
        dims = ""
        For i = 0 To bc_am_ef_functions.excel_functions.Count - 1
            If bc_am_ef_functions.excel_functions(i).display_name = Me.Cfunction.Text Then
                fm = "=" + bc_am_ef_functions.excel_functions(i).name + "("
                For j = 0 To bc_am_ef_functions.excel_functions(i).params.count - 1
                    ti = ""
                    tm = "none"
                    sti = ""
                    eti = ""
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "range" Then
                        sti = Me.Cstart.Text
                        If Me.Crange.SelectedIndex = 0 Then
                            sti = ",,,"
                        End If
                        If Me.Crange.SelectedIndex = 1 Then
                            If is_formula(sti) = False Then
                                If sti = "" Then
                                    sti = ","
                                Else
                                    sti = """" + sti + ""","
                                End If
                            Else
                                If sti = "" Then
                                    sti = +","
                                Else
                                    sti = sti + ","
                                End If
                            End If
                            sti = sti + ",,"
                        End If
                        If Me.Crange.SelectedIndex = 2 Then

                            If is_formula(sti) = False Then
                                If sti = "" Then
                                    sti = ","
                                Else
                                    sti = """" + sti + ""","
                                End If
                            Else
                                If sti = "" Then
                                    sti = ","
                                Else
                                    sti = sti + ","
                                End If
                            End If
                            sti = "," + sti + ","
                        End If
                        If Me.Crange.SelectedIndex = 3 Then
                            If is_formula(sti) = False Then
                                If sti = "" Then
                                    sti = ","
                                Else
                                    sti = """" + sti + ""","
                                End If
                            Else
                                If sti = "" Then
                                    sti = ","
                                Else
                                    sti = sti + ","
                                End If
                            End If
                            sti = ",," + sti
                        End If
                        If Me.Crange.SelectedIndex = 4 Then
                            If is_formula(sti) = False Then
                                If sti = "" Then
                                    sti = ","
                                Else
                                    sti = """" + sti + ""","
                                End If
                            Else
                                If sti = "" Then
                                    sti = ","
                                Else
                                    sti = sti + ","
                                End If
                            End If
                            sti = "," + sti
                            eti = Me.cend.Text
                            If is_formula(eti) = False Then
                                If eti = "" Then
                                    eti = ","
                                Else
                                    eti = """" + eti + ""","
                                End If
                            Else
                                If eti = "" Then
                                    eti = ","
                                Else
                                    eti = eti + ","
                                End If
                            End If
                            sti = sti + eti
                        End If
                    End If
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "context" Then
                        ti = Me.Ccontext.Text
                    End If
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "cal" Then
                        ti = Me.Ccal.Text
                        tm = Me.Cmon.Text
                    End If
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "section" Then
                        ti = Me.Csection.Text
                    End If
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "gen1" Then
                        ti = Me.Cgen1.Text
                    End If
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "gen2" Then
                        ti = Me.Cgen2.Text
                    End If
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "gen3" Then
                        ti = Me.cgen3.Text
                    End If
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "class" Then
                        ti = Me.Cclass.Text
                    End If
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "entity" Then
                        ti = Me.centity.Text
                    End If
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "lclass" Then
                        ti = Me.clclass.Text
                    End If
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "lentity" Then
                        ti = Me.clentity.Text
                    End If
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "item" Then
                        ti = Me.Citem.Text
                    End If
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "contributor" Then
                        ti = Me.Ccont.Text
                    End If
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "stage" Then
                        ti = Me.Cstage.Text
                    End If
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "year" Then
                        ti = Me.Cyear.Text
                    End If
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "period" Then
                        ti = Me.Cperiod.Text
                    End If
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "date_at" Then
                        If Me.Chkdate.Checked = True Then
                            ti = "9-9-9999"
                        Else
                            ti = CStr(Me.tdate.Text)
                        End If
                    End If
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "date_from" Then
                        If Me.chkdate_from.Checked = True Then
                            ti = "1-1-1900"
                        Else
                            ti = CStr(Me.tdate_from.Text)
                        End If
                    End If
                    If sti <> "" Then
                        fm = fm + sti
                    Else
                        If is_formula(ti) = False Then
                            If ti = "" Then
                                fm = fm + ","
                            Else
                                fm = fm + """" + ti + ""","
                            End If
                        Else
                            If ti = "" Then
                                fm = fm + ","
                            Else
                                fm = fm + ti + ","
                            End If
                        End If
                        If tm <> "none" Then
                            If is_formula(tm) = False Then
                                If tm = "" Then
                                    fm = fm + ","
                                Else
                                    fm = fm + """" + tm + ""","
                                End If
                            Else
                                If tm = "" Then
                                    fm = fm + ","
                                Else
                                    fm = fm + tm + ","
                                End If
                            End If
                        End If
                    End If
                Next
                Exit For
            End If
        Next
        REM now do dimnesions
        If Me.Lseldim.Items.Count > 0 Then
            For i = 0 To Me.Lseldim.Items.Count - 1
                If i > 0 Then
                    dims = dims + "," + Me.Lseldim.Items(i)
                Else
                    dims = Me.Lseldim.Items(i)
                End If
            Next
            dims = """" + dims + """"
            fm = fm.Substring(0, fm.Length - 1)
            fm = fm + "," + dims + ")"
        Else
            fm = fm.Substring(0, fm.Length - 1)
            fm = fm + ")"
        End If

        build_function = fm
    End Function

    Dim strLastClass As String
    Private Sub Cclass_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cclass.SelectedIndexChanged

        If strLastClass Is Nothing OrElse (Not Cclass.SelectedItem Is Nothing AndAlso strLastClass <> Cclass.SelectedItem.ToString) Then

            strLastClass = Cclass.SelectedItem.ToString

            check_context_set()

            If Cclass.SelectedIndex <> -1 Then
                BlueCurve_EntitytSearch.SearchClass = bc_am_ef_functions.class_ids(Me.Cclass.SelectedIndex)
                BlueCurve_EntitytSearch.SearchSetup(False)
                LoadSearchedEntities()
            End If

        End If

    End Sub
    Private Sub Clclass_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles clclass.SelectedIndexChanged
        Dim i As Integer
        If Me.clclass.SelectedIndex = -1 Then
            Exit Sub
        End If
        Me.clentity.Items.Clear()
        Me.clentity.Text = ""
        For i = 0 To bc_am_ef_functions.entities.Count - 1
            If bc_am_ef_functions.entities(i).class_id = bc_am_ef_functions.class_ids(Me.clclass.SelectedIndex) Then
                Me.clentity.Items.Add(bc_am_ef_functions.entities(i).name)
            End If
        Next
        check_context_set()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Hide()
    End Sub
    Private Sub Chkdate_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chkdate.CheckedChanged
        If Me.Chkdate.Checked = True Then
            Me.dtpdate.Visible = False
            Me.sdate.Visible = False
            Me.tdate.Visible = False
        Else
            Me.dtpdate.Visible = True
            Me.sdate.Visible = True
            Me.tdate.Visible = True
        End If
    End Sub
    Public Function is_formula(ByVal fm As String) As Boolean
        Try
            Dim i As Integer
            Dim now_numeric As Boolean = False
            Dim alphacount As Integer = 0
            is_formula = False
            REM if has a $ it is a formula
            If fm.Length < 2 Then
                Exit Function
            End If
            'Dim k As Integer
            If InStr(fm, "$") > 0 Then
                is_formula = True
                Exit Function
            End If

            REM if has alpha chracter followed by numeric it is as well
            REM first character must be alpha
            REM FIL MAY 2013 only allow up 2 to alphas before a numeric else any word
            REM that ends in a numeric will pass
            If (Asc(fm.Substring(0, 1)) >= Asc("a") And Asc(fm.Substring(0, 1)) <= Asc("z")) Or (Asc(fm.Substring(0, 1)) >= Asc("A") And Asc(fm.Substring(0, 1)) <= Asc("Z")) Then
                alphacount = 1
                For i = 1 To fm.Length - 1
                    If (Asc(fm.Substring(i, 1)) >= Asc("a") And Asc(fm.Substring(i, 1)) <= Asc("z")) Or (Asc(fm.Substring(i, 1)) >= Asc("A") And Asc(fm.Substring(i, 1)) <= Asc("Z")) Then
                        alphacount = alphacount + 1
                        If alphacount > 2 Then
                            Exit Function
                        End If
                        'k = 4
                        If now_numeric = True Then
                            Exit Function
                        End If
                    ElseIf IsNumeric(fm.Substring(i, 1)) = True Then
                        REM must now be numeric 
                        now_numeric = True
                        If i = fm.Length - 1 Then
                            is_formula = True
                            Exit Function
                        End If
                    Else
                        Exit Function
                    End If
                Next
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_ef_wizard", "is_formula", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            REM FIL JULY 2013 
            REM double check valid formula by pasting in excel cell
            If is_formula = True Then
                If test_valid_excel_address(fm) = False Then
                    is_formula = False
                End If
            End If
        End Try
    End Function
    Private Sub sclass_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles sclass.Click
        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")
        Try
            Me.sclass.BorderStyle = Windows.Forms.BorderStyle.Fixed3D
            Me.Cclass.Text = activecell.application.activecell.address
            Me.sclass.BorderStyle = Windows.Forms.BorderStyle.None
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
        End Try
    End Sub

    Private Sub sentity_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles sentity.Click
        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")
        Try
            Me.sentity.BorderStyle = Windows.Forms.BorderStyle.Fixed3D
            Me.centity.Text = activecell.application.activecell.address
            Me.sentity.BorderStyle = Windows.Forms.BorderStyle.None
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
        End Try
    End Sub

    Private Sub sitem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles sitem.Click
        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")
        Try
            Me.sitem.BorderStyle = Windows.Forms.BorderStyle.Fixed3D
            Me.Citem.Text = activecell.application.activecell.address
            Me.sitem.BorderStyle = Windows.Forms.BorderStyle.None
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
        End Try
    End Sub

    Private Sub sstage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles sstage.Click
        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")
        Try
            Me.sstage.BorderStyle = Windows.Forms.BorderStyle.Fixed3D
            Me.Cstage.Text = activecell.application.activecell.address
            Me.sstage.BorderStyle = Windows.Forms.BorderStyle.None
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
        End Try
    End Sub

    Private Sub scont_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles scont.Click
        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")
        Try
            Me.scont.BorderStyle = Windows.Forms.BorderStyle.Fixed3D
            Me.Ccont.Text = activecell.application.activecell.address
            Me.scont.BorderStyle = Windows.Forms.BorderStyle.None
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
        End Try
    End Sub

    Private Sub sdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles sdate.Click
        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")
        Try
            Me.sdate.BorderStyle = Windows.Forms.BorderStyle.Fixed3D
            Me.tdate.Text = activecell.application.activecell.address
            Me.sdate.BorderStyle = Windows.Forms.BorderStyle.None
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
        End Try
    End Sub

    Private Sub syear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles syear.Click
        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")
        Try
            Me.syear.BorderStyle = Windows.Forms.BorderStyle.Fixed3D
            Me.Cyear.Text = activecell.application.activecell.address
            Me.syear.BorderStyle = Windows.Forms.BorderStyle.None
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
        End Try
    End Sub

    Private Sub speriod_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles speriod.Click
        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")
        Try
            Me.speriod.BorderStyle = Windows.Forms.BorderStyle.Fixed3D
            Me.Cperiod.Text = activecell.application.activecell.address
            Me.speriod.BorderStyle = Windows.Forms.BorderStyle.None
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
        End Try
    End Sub

    Private Sub smon_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles smon.Click
        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")
        Try
            Me.smon.BorderStyle = Windows.Forms.BorderStyle.Fixed3D
            Me.Cmon.Text = activecell.application.activecell.address
            Me.smon.BorderStyle = Windows.Forms.BorderStyle.None
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
        End Try
    End Sub


    Private Sub Citem_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Citem.SelectedIndexChanged

        If Me.Citem.SelectedIndex = -1 Then
            Exit Sub
        End If
        check_period()
    End Sub
    Private Sub check_period()
        Dim i As Integer
        Me.Cperiod.Enabled = True
        Me.Cyear.Enabled = True
        Me.speriod.Enabled = True
        Me.syear.Enabled = True
        Me.Lyear.Enabled = True
        Me.Lperiod.Enabled = True

        For i = 0 To bc_am_ef_functions.item_names.Count - 1
            If bc_am_ef_functions.item_names(i) = Me.Citem.Text Then
                If bc_am_ef_functions.item_types(i) = "static" Then
                    Me.Cperiod.Enabled = False
                    Me.Cyear.Enabled = False
                    Me.speriod.Enabled = False
                    Me.syear.Enabled = False
                    Me.Lyear.Enabled = False
                    Me.Lperiod.Enabled = False
                    disable_calendar()
                    Exit Sub
                End If
            End If

        Next
        check_calendar()
    End Sub

    Private Sub disable_calendar()
        Me.Ccal.Enabled = False
        Me.scal.Enabled = False
        Me.Lcal.Enabled = False
        Me.Ccal.Text = "Fiscal"
    End Sub
    Private Sub enable_calendar()
        Me.Ccal.Enabled = True
        Me.scal.Enabled = True
        Me.Lcal.Enabled = True
    End Sub

    Private Sub Citem_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Citem.TextChanged
        check_period()

    End Sub
    Private Sub Cyear_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cyear.TextChanged
        check_calendar()
    End Sub
    Private Sub check_calendar()
        disable_calendar()
        If Me.Cyear.Enabled = True And Me.Cperiod.Enabled = True And (Me.Cperiod.Text = "Full Year" Or is_formula(Me.Cperiod.Text) = True) Then
            enable_calendar()
        End If

    End Sub
    Private Sub Cperiod_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cperiod.TextChanged
        check_calendar()
    End Sub

    Private Sub badddim_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Badddim.Click
        If Me.Lalldim.SelectedIndex = -1 Then
            Exit Sub
        End If
        Me.Lseldim.Items.Add(Me.Lalldim.Text)
        Me.Lalldim.Items.RemoveAt(Me.Lalldim.SelectedIndex)
        Me.Badddim.Enabled = False

    End Sub

    Private Sub badelim_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Bdeldim.Click
        If Me.Lseldim.SelectedIndex = -1 Then
            Exit Sub
        End If
        Me.Lalldim.Items.Add(Me.Lseldim.Text)
        Me.Lseldim.Items.RemoveAt(Me.Lseldim.SelectedIndex)
        Me.Bdeldim.Enabled = False
    End Sub

    Private Sub lalldim_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Lalldim.SelectedIndexChanged
        If Me.Lalldim.SelectedIndex = -1 Then
            Exit Sub
        End If
        Me.Badddim.Enabled = True
    End Sub

    Private Sub Lseldim_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Lseldim.SelectedIndexChanged
        If Me.Lseldim.SelectedIndex = -1 Then
            Exit Sub
        End If
        If Me.Lseldim.Items.Count = 1 Then
            Exit Sub
        End If
        Me.Bdeldim.Enabled = True
    End Sub

    Private Sub rdef_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdef.CheckedChanged
        If rdef.Checked = True Then
            'Me.gdim.Height = Me.gdim.Height - 73
            Me.MinimumSize = Nothing
            Me.MaximumSize = Nothing
            Me.Height = formHeight + 9
            Me.MinimumSize = New Size(500, Me.Height)
            Me.MaximumSize = New Size(800, Me.Height)
            resizeForm()
        End If
    End Sub

    Private Sub rch_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Rch.CheckedChanged
        If Rch.Checked = True Then
            'Me.gdim.Height = Me.gdim.Height + 73
            Me.MinimumSize = Nothing
            Me.MaximumSize = Nothing
            Me.Height = formHeight + gdim.Height
            Me.MinimumSize = New Size(500, Me.Height)
            Me.MaximumSize = New Size(800, Me.Height)
        End If
    End Sub

    Private Sub Dtpdate_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtpdate.ValueChanged
        Me.tdate.Text = Format(Me.dtpdate.Value, "dd-MM-yyyy")
    End Sub

    Private Sub sdate_from_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles sdate_from.Click
        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")
        Try
            Me.sdate_from.BorderStyle = Windows.Forms.BorderStyle.Fixed3D
            Me.tdate_from.Text = activecell.application.activecell.address
            Me.sdate_from.BorderStyle = Windows.Forms.BorderStyle.None
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
        End Try
    End Sub
    Private Sub Dtpdate_from_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtpdate_from.ValueChanged
        Me.tdate_from.Text = Format(Me.dtpdate_from.Value, "dd-MM-yyyy")
    End Sub

    Private Sub chkdate_from_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkdate_from.CheckedChanged
        If Me.chkdate_from.Checked = True Then
            Me.dtpdate_from.Visible = False
            Me.sdate_from.Visible = False
            Me.tdate_from.Visible = False
        Else
            Me.dtpdate_from.Visible = True
            Me.sdate_from.Visible = True
            Me.tdate_from.Visible = True
        End If
    End Sub

    Private Sub gdim_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles gdim.Paint

    End Sub

    Private Sub sgen1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles sgen1.Click
        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")
        Try
            Me.sgen1.BorderStyle = Windows.Forms.BorderStyle.Fixed3D
            Me.Cgen1.Text = activecell.application.activecell.address
            Me.sgen1.BorderStyle = Windows.Forms.BorderStyle.None
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
        End Try
    End Sub
    Private Sub sgen2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles sgen2.Click
        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")
        Try
            Me.sgen2.BorderStyle = Windows.Forms.BorderStyle.Fixed3D
            Me.Cgen2.Text = activecell.application.activecell.address
            Me.sgen2.BorderStyle = Windows.Forms.BorderStyle.None
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
        End Try
    End Sub
    Private Sub sgen3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles sgen3.Click
        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")
        Try
            Me.sgen3.BorderStyle = Windows.Forms.BorderStyle.Fixed3D
            Me.cgen3.Text = activecell.application.activecell.address
            Me.sgen3.BorderStyle = Windows.Forms.BorderStyle.None
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
        End Try
    End Sub

    Private Sub scontext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles scontext.Click
        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")
        Try
            Me.scontext.BorderStyle = Windows.Forms.BorderStyle.Fixed3D
            Me.Ccontext.Text = activecell.application.activecell.address
            Me.scontext.BorderStyle = Windows.Forms.BorderStyle.None
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
        End Try
    End Sub

    Private Sub ssection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ssection.Click
        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")
        Try
            Me.ssection.BorderStyle = Windows.Forms.BorderStyle.Fixed3D
            Me.Csection.Text = activecell.application.activecell.address
            Me.ssection.BorderStyle = Windows.Forms.BorderStyle.None
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
        End Try
    End Sub

    Dim strLastEntity As String
    Private Sub centity_DropDownClosed(ByVal sender As Object, ByVal e As System.EventArgs) Handles centity.DropDownClosed
        If centity.Items.Count < 1 Then
            Me.centity.Items.Add(" ")
            LoadSearchedEntities()
        End If
    End Sub

    Private Sub centity_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles centity.KeyDown
        If e.KeyCode <> Windows.Forms.Keys.Down And e.KeyCode <> Windows.Forms.Keys.Up And _
            e.KeyCode <> Windows.Forms.Keys.ShiftKey And e.KeyCode <> Windows.Forms.Keys.Shift Then
            centity.DroppedDown = False
        End If
    End Sub

    Private Sub centity_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles centity.KeyUp
        If e.KeyCode <> Windows.Forms.Keys.Tab Then
            If strLastEntity Is Nothing OrElse (Not centity.Text Is Nothing AndAlso centity.Text <> strLastEntity) Then
                strLastEntity = centity.Text
                If EntityUpdating = False Then
                    SearchTimer.Stop()
                    SearchTimer.Start()
                End If
            End If
        End If
    End Sub

    Private Sub centity_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles centity.SelectedIndexChanged
        If strLastEntity Is Nothing OrElse (Not centity.Text Is Nothing AndAlso centity.Text <> strLastEntity) Then
            strLastEntity = centity.Text
            Try

                If Me.Citem.Visible = True Then
                    If Me.Cclass.SelectedIndex = -1 Then
                        Exit Sub
                    End If
                    If Me.centity.SelectedIndex = -1 Then
                        Exit Sub
                    End If
                    Me.Cursor = Windows.Forms.Cursors.WaitCursor

                    Dim i, j As Integer
                    REM download specific items for entity
                    REM add all items 

                    Me.Citem.Items.Clear()
                    For i = 0 To bc_am_ef_functions.item_names.Count - 1
                        Me.Citem.Items.Add(bc_am_ef_functions.item_names(i))
                    Next
                    'Me.centity.Items.Add("All Entities")
                    For i = 0 To bc_am_ef_functions.entities.Count - 1
                        If bc_am_ef_functions.entities(i).class_id = bc_am_ef_functions.class_ids(Me.Cclass.SelectedIndex) Then
                            If bc_am_ef_functions.entities(i).name = Me.centity.Text Then
                                Dim oitems As New bc_om_ef_items(bc_am_ef_functions.entities(i).id)
                                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                                    oitems.db_read()
                                Else
                                    oitems.tmode = bc_cs_soap_base_class.tREAD
                                    If oitems.transmit_to_server_and_receive(oitems, True) = False Then
                                        Exit Sub
                                    End If
                                End If
                                If oitems.item_names.Count > 0 Then

                                    Me.Citem.Items.Clear()
                                    For j = 0 To oitems.item_names.Count - 1
                                        Me.Citem.Items.Add(oitems.item_names(j))
                                    Next
                                    Exit For
                                End If
                            End If
                        End If
                    Next
                End If
                Me.Cursor = Windows.Forms.Cursors.Default
                check_context_set()

            Catch ex As Exception
                Dim oerr As New bc_cs_error_log("bc_am_ef_wizard", "centity_SelectedIndexChanged", bc_cs_error_codes.USER_DEFINED, ex.Message)
            End Try

        End If

    End Sub
    Private Sub centity_textChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles centity.TextChanged
        If Me.centity.Text <> "" Then
            check_context_set()
        End If
    End Sub
    Private Sub cclass_textChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cclass.TextChanged
        If Me.centity.Text <> "" Then
            check_context_set()
        End If
    End Sub
    Private Sub check_context_set()

        Try
            Me.Ccontext.Enabled = True
            Me.scontext.Enabled = True
            Me.Lcontext.Enabled = True
            If Me.Ccontext.SelectedIndex > -1 Then
                Me.Ccontext.SelectedIndex = 0
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_ef_wizard", "check_context_set", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Sub

    Private Sub ccontext_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Ccontext.SelectedIndexChanged
        Me.Csection.SelectedIndex = -1
        Me.Csection.Enabled = False
        Me.Lsection.Enabled = False
        Me.ssection.Enabled = False

        If Me.Ccontext.SelectedIndex = 0 Then
            Me.Csection.Enabled = True
            Me.Csection.SelectedIndex = 0
            Me.Lsection.Enabled = True
            Me.ssection.Enabled = True
        End If
    End Sub
    Private Sub ccontext_textChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Ccontext.TextChanged
        Me.Csection.SelectedIndex = -1
        Me.Csection.Enabled = False
        Me.Lsection.Enabled = False
        Me.ssection.Enabled = False

        If Me.Ccontext.SelectedIndex = 0 Then
            Me.Csection.Enabled = True
            Me.Csection.SelectedIndex = 0
            Me.Lsection.Enabled = True
            Me.ssection.Enabled = True
        End If
    End Sub

    Private Sub bhelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Bhelp.Click
        If Me.Rch.Checked = True Then
            Me.rdef.Checked = True

        End If
        If Me.Thelp.Visible = False Then
            Me.Thelp.Visible = True
            load_params(False, Me.Thelp.Height)
        Else
            Me.Thelp.Visible = False
            load_params(False, 0)
        End If
    End Sub





    Private Sub Label4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub sstart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles sstart.Click
        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")
        Try
            Me.sstart.BorderStyle = Windows.Forms.BorderStyle.Fixed3D
            Me.Cstart.Text = activecell.application.activecell.address
            Me.sstart.BorderStyle = Windows.Forms.BorderStyle.None
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
        End Try
    End Sub

    Private Sub send_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles send.Click
        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")
        Try
            Me.send.BorderStyle = Windows.Forms.BorderStyle.Fixed3D
            Me.cend.Text = activecell.application.activecell.address
            Me.send.BorderStyle = Windows.Forms.BorderStyle.None
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
        End Try
    End Sub

    Private Sub crange_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Crange.SelectedIndexChanged
        Select Case Me.Crange.SelectedIndex
            Case 0
                Me.Cstart.Text = ""
                Me.cend.Text = ""
                Me.Cstart.Enabled = False
                Me.cend.Enabled = False
            Case 1, 2, 3
                Me.Cstart.Enabled = True
                Me.Lto.Visible = False
                Me.cend.Enabled = False
                Me.cend.Text = ""
            Case 4
                Me.Cstart.Enabled = True
                Me.Lto.Visible = True
                Me.cend.Enabled = True
        End Select
    End Sub

    Private Sub slclass_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles slclass.Click
        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")
        Try
            Me.slclass.BorderStyle = Windows.Forms.BorderStyle.Fixed3D
            Me.clclass.Text = activecell.application.activecell.address
            Me.slclass.BorderStyle = Windows.Forms.BorderStyle.None
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
        End Try
    End Sub

    Private Sub slentity_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles slentity.Click
        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")
        Try
            Me.slentity.BorderStyle = Windows.Forms.BorderStyle.Fixed3D
            Me.clentity.Text = activecell.application.activecell.address
            Me.slentity.BorderStyle = Windows.Forms.BorderStyle.None
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
        End Try
    End Sub

    Private Sub bapply_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles bapply.MouseDown
        Me.bapply.Visible = False
    End Sub
    Private Sub bapply_Mouseup(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles bapply.MouseUp
        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")
        Try
            Me.Cursor = Windows.Forms.Cursors.WaitCursor
            Me.bapply.Visible = True
            'Me.bapply.BorderStyle = Windows.Forms.BorderStyle.Fixed3D
            Dim tx As String
            tx = build_function()
            bc_am_excel_functions.batch_mode = 0
            Dim ocoom As New bc_cs_activity_log("bc_am_ef_wizard", "apply", bc_cs_activity_codes.COMMENTARY, "batch mode: " + CStr(bc_am_excel_functions.batch_mode))

            activecell.formula = tx
            activecell.select()
            If Me.Copen.Checked = False Then
                Me.Hide()
            End If
            Me.Cursor = Windows.Forms.Cursors.Default

            Me.bapply.Visible = True
        Catch ex As Exception
            Me.Cursor = Windows.Forms.Cursors.Default
            Dim oerr As New bc_cs_message("Blue Curve", "There is an error writing to selected cell: " + ex.Message, bc_cs_message.MESSAGE, False, False, "Yes", "no", True)
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
        End Try
    End Sub

    REM FIL 5.2 August 2014
    Private Function test_valid_excel_address(ByVal fm As String) As Boolean
        Dim ofm As String
        ofm = activecell.formula
        fm = "=" + fm
        test_valid_excel_address = True
        Try
            activecell.formula = fm
        Catch ex As Exception
            test_valid_excel_address = False
        Finally
            activecell.formula = ofm
        End Try
    End Function
    REM ==========================

    Private Sub bclose_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles bclose.MouseDown
        Me.bclose.Visible = False
    End Sub
    Private Sub bclose_Mouseup(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles bclose.MouseUp
        Me.bclose.Visible = True
        Me.Hide()
    End Sub

    Private Sub uxTypeNameHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxTypeNameHelp.Click
        If Me.Rch.Checked = True Then
            Me.rdef.Checked = True

        End If
        If Me.Thelp.Visible = False Then
            Me.Thelp.Visible = True
            load_params(False, Me.Thelp.Height)
        Else
            Me.Thelp.Visible = False
            load_params(False, 0)
        End If
    End Sub


    Private Sub scal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles scal.Click
        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")
        Try
            Me.scal.BorderStyle = Windows.Forms.BorderStyle.Fixed3D
            Me.Ccal.Text = activecell.application.activecell.address
            Me.scal.BorderStyle = Windows.Forms.BorderStyle.None
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
        End Try
    End Sub


    Private Sub Ccal_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Ccal.SelectedIndexChanged
        Me.Cmon.SelectedIndex = -1
        Me.Cmon.Enabled = False
        If Me.Ccal.SelectedIndex = 1 Then
            Me.Cmon.Enabled = True
        End If
    End Sub

    Private Sub Llentity_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Llentity.Click

    End Sub

    Private Sub SearchTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SearchTimer.Tick

        Try

            EntityUpdating = True

            SearchTimer.Stop()
            BlueCurve_EntitytSearch.SearchText = centity.Text
            If Len(centity.Text) <> 0 Then
                BlueCurve_EntitytSearch.SearchRefresh()
            End If

            LoadSearchedEntities()

            If centity.Items.Count = 1 Then
                centity.SelectedIndex = 0
                centity.DroppedDown = False
            Else
                centity.DroppedDown = True
            End If

            centity.SelectAll()

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_ef_wizard", "SearchTimer_Tick", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

            EntityUpdating = False

        End Try

    End Sub

    Private Sub LoadSearchedEntities()

        Dim i As Integer
        Dim isInSearech As Boolean
        Dim textHolder As String = ""

        Try
            If Me.Cclass.SelectedIndex = -1 Then
                Exit Sub
            End If

            Me.centity.BeginUpdate()

            Me.centity.Items.Clear()
            If BlueCurve_EntitytSearch.SearchText = "" Then
                Me.centity.Items.Add("All Entities")
            End If
            For i = 0 To bc_am_ef_functions.entities.Count - 1
                If bc_am_ef_functions.entities(i).class_id = bc_am_ef_functions.class_ids(Me.Cclass.SelectedIndex) Then

                    REM Check if in search
                    isInSearech = False
                    If BlueCurve_EntitytSearch.SearchText <> "" Then
                        If BlueCurve_EntitytSearch.ShownEntityList.Contains(bc_am_ef_functions.entities(i).id) = True Then
                            isInSearech = True
                        End If
                    Else
                        isInSearech = True
                    End If

                    If isInSearech = True Then
                        Me.centity.Items.Add(bc_am_ef_functions.entities(i).name)
                    End If
                End If
            Next

        Finally
            Me.centity.EndUpdate()
        End Try

    End Sub

    Private Sub bc_am_ef_wizard_ResizeEnd(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ResizeEnd

        resizeForm()

    End Sub

    Private Sub ResizeControls(ByVal parent As Control)
        For Each c As Control In parent.Controls
            If (TypeOf c Is Button OrElse TypeOf c Is PictureBox) AndAlso c.Tag = "movable" Then
                Dim offset As Integer = 43
                If TypeOf c.Parent Is Panel Then
                    offset += c.Parent.Location.X
                End If
                If TypeOf c Is Button Then
                    offset += 3
                End If
                c.Location = New Point(Me.Width - offset, c.Location.Y)
            End If
            If TypeOf c Is ComboBox AndAlso c.Tag = "sizable" Then
                Dim offset As Integer = 54
                If TypeOf c.Parent Is Panel Then
                    offset += c.Parent.Location.X
                End If
                c.Size = New Size(Me.Size.Width - offset - c.Location.X, c.Size.Height)
                CType(c, ComboBox).SelectionLength = 0
            End If
            If TypeOf c Is Panel Then
                c.Size = New Size(Me.Width - c.Location.X, c.Height)
            End If
            Application.DoEvents()
            ResizeControls(c)

        Next
    End Sub

    Private Sub resizeForm()

        ResizeControls(Me)

        gdim.Width = Me.Width

        Lalldim.Width = 103 * gdim.Width / 304
        Lseldim.Location = New Point(184 * gdim.Width / 304, Lseldim.Location.Y)
        Lseldim.Width = Me.Width - Lseldim.Location.X - 54

        Badddim.Location = New Point(136 / 304 * gdim.Width, Badddim.Location.Y)
        Bdeldim.Location = New Point(136 / 304 * gdim.Width, Bdeldim.Location.Y)

    End Sub

    Private Sub prange_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles prange.Paint

    End Sub
End Class



