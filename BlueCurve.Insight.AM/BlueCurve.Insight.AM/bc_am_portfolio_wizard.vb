Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports BlueCurve.Create.AM
Imports System.Collections
Imports system.windows.Forms

Public Class bc_am_portfolio_wizard
    Inherits System.Windows.Forms.Form
    Public oseltemplates As New bc_om_ef_templates
    Public max_rows As Integer = 100
    Public excelapp As Object
    Public selected_portfolio_id As Long
    Public user_entity_sets As bc_om_ef_entity_sets
    Public user_items_sets As bc_om_ef_item_sets
    Public loading As Boolean = True
    Public filter_item_ids As New ArrayList
    Public filter_item_types As New ArrayList
    Public filter_item_factor As New ArrayList
    Public filter_item_monatary As New ArrayList
    Public filter_item_symbol As New ArrayList
    Public filter_item_assoc_class As New ArrayList
    Public item_sel_type As New ArrayList
    Public item_sel_factor As New ArrayList
    Public item_sel_monatary As New ArrayList
    Public item_sel_symbol As New ArrayList
    Public item_sel_assoc_class As New ArrayList
    Public tx_f As String
    Public tx_c As String
    Public tx_u As String
    Public tx_filt As String
    Public tx_curr As String
    Public use_class_ids As New ArrayList
    Public use_class_types As New ArrayList
    Public oportfolios As New bc_om_ef_portolios
    Public propentitymaster As New ArrayList
    Public propentityslave As New ArrayList

    'REM Steve Wooderson 18/01/2013 DA5
    Public chart_method As String
    Public chart_passrangeasparam As Boolean
    Public chart_active As Boolean
    Public chart_data As Boolean
    Public tx_chart As String
    Public chart_focus As String

    Friend WithEvents Rpredefined As System.Windows.Forms.RadioButton
    Friend WithEvents Ppredefined As System.Windows.Forms.Panel
    Friend WithEvents Bopenpd As System.Windows.Forms.Button
    Friend WithEvents Lpredefined As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents cass As System.Windows.Forms.TabControl
    Friend WithEvents TabPage5 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage6 As System.Windows.Forms.TabPage
    Friend WithEvents lallass As System.Windows.Forms.ListBox
    Friend WithEvents lselentity As System.Windows.Forms.ListBox
    Friend WithEvents Lselass As System.Windows.Forms.ListBox
    Friend WithEvents pdatetype As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Rcalendar As System.Windows.Forms.RadioButton
    Friend WithEvents Rworking As System.Windows.Forms.RadioButton
    Friend WithEvents Pyearenddate As System.Windows.Forms.Panel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents DateTimePicker3 As System.Windows.Forms.DateTimePicker
    Friend WithEvents Cfentity As System.Windows.Forms.ComboBox
    Friend WithEvents Cfclass As System.Windows.Forms.ComboBox
    Friend WithEvents Lfilter As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Pparam1 As System.Windows.Forms.Panel
    Friend WithEvents Cparam1 As System.Windows.Forms.ComboBox
    Friend WithEvents lparam1 As System.Windows.Forms.Label
    Public tprogress As System.Threading.Thread
    Friend WithEvents BlueCurve_Unversefilter As BlueCurve.Core.AS.BlueCurve_TextSearch
    Friend WithEvents BlueCurve_ItemsFilter As BlueCurve.Core.AS.BlueCurve_TextSearch
    Public success As Boolean
    Friend WithEvents cchart As System.Windows.Forms.CheckBox
    Friend WithEvents cchartfocus As System.Windows.Forms.ComboBox
    Friend WithEvents lchartfocus As System.Windows.Forms.Label
    Private Const MAX_ATTEMPTS = 10000
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
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents Universe As System.Windows.Forms.TabPage
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage4 As System.Windows.Forms.TabPage
    Friend WithEvents cfunction As System.Windows.Forms.ComboBox
    Friend WithEvents twizard As System.Windows.Forms.TabControl
    Friend WithEvents cclass As System.Windows.Forms.ComboBox
    Friend WithEvents lclass As System.Windows.Forms.Label
    Friend WithEvents ccontext As System.Windows.Forms.ComboBox
    Friend WithEvents csection As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents ctemplate As System.Windows.Forms.ComboBox
    Friend WithEvents Output As System.Windows.Forms.TabPage
    Friend WithEvents lselitem As System.Windows.Forms.ListBox
    Friend WithEvents lallitem As System.Windows.Forms.ListBox
    Friend WithEvents bselitem As System.Windows.Forms.Button
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents cstage As System.Windows.Forms.ComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Ccont As System.Windows.Forms.ComboBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents rnow As System.Windows.Forms.RadioButton
    Friend WithEvents Rdate As System.Windows.Forms.RadioButton
    Friend WithEvents DateTimePicker1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents Cyearstart As System.Windows.Forms.ComboBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents cyearend As System.Windows.Forms.ComboBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents lperiods As System.Windows.Forms.ListBox
    Friend WithEvents ldesc As System.Windows.Forms.TextBox
    Friend WithEvents rcell As System.Windows.Forms.RadioButton
    Friend WithEvents rsheet As System.Windows.Forms.RadioButton
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents rcurrconv As System.Windows.Forms.RadioButton
    Friend WithEvents rcurrshow As System.Windows.Forms.RadioButton
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents rcurrnone As System.Windows.Forms.RadioButton
    Friend WithEvents bdn As System.Windows.Forms.Button
    Friend WithEvents bup As System.Windows.Forms.Button
    Friend WithEvents bdelper As System.Windows.Forms.Button
    Friend WithEvents baddper As System.Windows.Forms.Button
    Friend WithEvents ccurr As System.Windows.Forms.ComboBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents tname As System.Windows.Forms.TextBox
    Friend WithEvents cperiods As System.Windows.Forms.ListBox
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents cschema As System.Windows.Forms.ComboBox
    Friend WithEvents bsaveitem As System.Windows.Forms.Button
    Friend WithEvents Popen As System.Windows.Forms.Panel
    Friend WithEvents rglobal As System.Windows.Forms.RadioButton
    Friend WithEvents bopport As System.Windows.Forms.Button
    Friend WithEvents bdelport As System.Windows.Forms.Button
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Rprivate As System.Windows.Forms.RadioButton
    Friend WithEvents Rprivatesave As System.Windows.Forms.RadioButton
    Friend WithEvents Rnosave As System.Windows.Forms.RadioButton
    Friend WithEvents rglobalsave As System.Windows.Forms.RadioButton
    Friend WithEvents lportfolios As System.Windows.Forms.ListView
    Friend WithEvents id As System.Windows.Forms.ColumnHeader
    Friend WithEvents rnew As System.Windows.Forms.RadioButton
    Friend WithEvents ropen As System.Windows.Forms.RadioButton
    Friend WithEvents citemset As System.Windows.Forms.ComboBox
    Friend WithEvents ritemset As System.Windows.Forms.RadioButton
    Friend WithEvents rportitems As System.Windows.Forms.RadioButton
    Friend WithEvents rallitems As System.Windows.Forms.RadioButton
    Friend WithEvents titemset As System.Windows.Forms.TextBox
    Friend WithEvents bdelitemset As System.Windows.Forms.Button
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents PictureBox3 As System.Windows.Forms.PictureBox
    Friend WithEvents bcancel As System.Windows.Forms.PictureBox
    Friend WithEvents bleft As System.Windows.Forms.PictureBox
    Friend WithEvents bleftdisabled As System.Windows.Forms.PictureBox
    Friend WithEvents bforward As System.Windows.Forms.PictureBox
    Friend WithEvents bcanceldisabled As System.Windows.Forms.PictureBox
    Friend WithEvents bforwarddisabled As System.Windows.Forms.PictureBox
    Friend WithEvents bokdisabled As System.Windows.Forms.PictureBox
    Friend WithEvents bok As System.Windows.Forms.PictureBox
    Friend WithEvents ppropress As System.Windows.Forms.ProgressBar
    Friend WithEvents ball As System.Windows.Forms.Button
    Friend WithEvents rdefined As System.Windows.Forms.RadioButton
    Friend WithEvents lcontext As System.Windows.Forms.Label
    Friend WithEvents ltemplate As System.Windows.Forms.Label
    Friend WithEvents lsection As System.Windows.Forms.Label
    Friend WithEvents bdelall As System.Windows.Forms.Button
    Friend WithEvents c_show_ea As System.Windows.Forms.CheckBox
    Friend WithEvents uxToolBarImages As System.Windows.Forms.ImageList
    Friend WithEvents pformat As System.Windows.Forms.Panel
    Friend WithEvents cformat As System.Windows.Forms.CheckBox
    Friend WithEvents cprec As System.Windows.Forms.ComboBox
    Friend WithEvents cprectype As System.Windows.Forms.ComboBox
    Friend WithEvents bdelitem As System.Windows.Forms.Button
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ltype As System.Windows.Forms.Label
    Friend WithEvents puniverse As System.Windows.Forms.Panel
    Friend WithEvents cclass2 As System.Windows.Forms.ComboBox
    Friend WithEvents bsaveent As System.Windows.Forms.Button
    Friend WithEvents Bdel As System.Windows.Forms.Button
    Friend WithEvents centityset As System.Windows.Forms.ComboBox
    Friend WithEvents rentity_set As System.Windows.Forms.RadioButton
    Friend WithEvents tentname As System.Windows.Forms.TextBox
    Friend WithEvents brement As System.Windows.Forms.Button
    Friend WithEvents bselent As System.Windows.Forms.Button
    Friend WithEvents centity As System.Windows.Forms.ComboBox
    Friend WithEvents lallentity As System.Windows.Forms.ListBox
    Friend WithEvents rclass As System.Windows.Forms.RadioButton
    Friend WithEvents rall As System.Windows.Forms.RadioButton
    Friend WithEvents rchoose As System.Windows.Forms.RadioButton
    Friend WithEvents pleadentity As System.Windows.Forms.Panel
    Friend WithEvents lleadentity As System.Windows.Forms.Label
    Friend WithEvents cleadenity As System.Windows.Forms.ComboBox
    Friend WithEvents pdatefrom As System.Windows.Forms.Panel
    Friend WithEvents DateTimePicker2 As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents rdatefrom As System.Windows.Forms.RadioButton
    Friend WithEvents rold As System.Windows.Forms.RadioButton
    Friend WithEvents ldateto As System.Windows.Forms.Label
    Friend WithEvents tsource As System.Windows.Forms.TextBox
    Friend WithEvents ttitle As System.Windows.Forms.TextBox
    Friend WithEvents Title As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents tsubtitle As System.Windows.Forms.TextBox
    Friend WithEvents llink As System.Windows.Forms.Label
    Friend WithEvents clink As System.Windows.Forms.ComboBox
    Friend WithEvents cand As System.Windows.Forms.CheckBox
    Friend WithEvents cclass3 As System.Windows.Forms.ComboBox
    Friend WithEvents centity2 As System.Windows.Forms.ComboBox
    Friend WithEvents ImageList2 As System.Windows.Forms.ImageList
    Friend WithEvents bentdn As System.Windows.Forms.Button
    Friend WithEvents bentup As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_portfolio_wizard))
        Me.twizard = New System.Windows.Forms.TabControl
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.Rpredefined = New System.Windows.Forms.RadioButton
        Me.Popen = New System.Windows.Forms.Panel
        Me.Ppredefined = New System.Windows.Forms.Panel
        Me.Bopenpd = New System.Windows.Forms.Button
        Me.uxToolBarImages = New System.Windows.Forms.ImageList(Me.components)
        Me.Lpredefined = New System.Windows.Forms.ListView
        Me.ColumnHeader5 = New System.Windows.Forms.ColumnHeader
        Me.bdelport = New System.Windows.Forms.Button
        Me.Rprivate = New System.Windows.Forms.RadioButton
        Me.rglobal = New System.Windows.Forms.RadioButton
        Me.bopport = New System.Windows.Forms.Button
        Me.lportfolios = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.id = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader4 = New System.Windows.Forms.ColumnHeader
        Me.ltype = New System.Windows.Forms.Label
        Me.rnew = New System.Windows.Forms.RadioButton
        Me.ropen = New System.Windows.Forms.RadioButton
        Me.tname = New System.Windows.Forms.TextBox
        Me.Label12 = New System.Windows.Forms.Label
        Me.cfunction = New System.Windows.Forms.ComboBox
        Me.TabPage2 = New System.Windows.Forms.TabPage
        Me.clink = New System.Windows.Forms.ComboBox
        Me.llink = New System.Windows.Forms.Label
        Me.cschema = New System.Windows.Forms.ComboBox
        Me.Label18 = New System.Windows.Forms.Label
        Me.lclass = New System.Windows.Forms.Label
        Me.cclass = New System.Windows.Forms.ComboBox
        Me.Universe = New System.Windows.Forms.TabPage
        Me.puniverse = New System.Windows.Forms.Panel
        Me.BlueCurve_Unversefilter = New BlueCurve.Core.[AS].BlueCurve_TextSearch
        Me.Lfilter = New System.Windows.Forms.Label
        Me.Cfentity = New System.Windows.Forms.ComboBox
        Me.Cfclass = New System.Windows.Forms.ComboBox
        Me.tentname = New System.Windows.Forms.TextBox
        Me.Lselass = New System.Windows.Forms.ListBox
        Me.cass = New System.Windows.Forms.TabControl
        Me.TabPage5 = New System.Windows.Forms.TabPage
        Me.TabPage6 = New System.Windows.Forms.TabPage
        Me.bentdn = New System.Windows.Forms.Button
        Me.ImageList2 = New System.Windows.Forms.ImageList(Me.components)
        Me.cclass3 = New System.Windows.Forms.ComboBox
        Me.cand = New System.Windows.Forms.CheckBox
        Me.centityset = New System.Windows.Forms.ComboBox
        Me.rchoose = New System.Windows.Forms.RadioButton
        Me.centity2 = New System.Windows.Forms.ComboBox
        Me.cclass2 = New System.Windows.Forms.ComboBox
        Me.bsaveent = New System.Windows.Forms.Button
        Me.Bdel = New System.Windows.Forms.Button
        Me.rentity_set = New System.Windows.Forms.RadioButton
        Me.centity = New System.Windows.Forms.ComboBox
        Me.rclass = New System.Windows.Forms.RadioButton
        Me.rall = New System.Windows.Forms.RadioButton
        Me.lallass = New System.Windows.Forms.ListBox
        Me.lallentity = New System.Windows.Forms.ListBox
        Me.lselentity = New System.Windows.Forms.ListBox
        Me.bselent = New System.Windows.Forms.Button
        Me.brement = New System.Windows.Forms.Button
        Me.Button1 = New System.Windows.Forms.Button
        Me.bentup = New System.Windows.Forms.Button
        Me.pleadentity = New System.Windows.Forms.Panel
        Me.cleadenity = New System.Windows.Forms.ComboBox
        Me.lleadentity = New System.Windows.Forms.Label
        Me.TabPage3 = New System.Windows.Forms.TabPage
        Me.BlueCurve_ItemsFilter = New BlueCurve.Core.[AS].BlueCurve_TextSearch
        Me.bdelitemset = New System.Windows.Forms.Button
        Me.bdelall = New System.Windows.Forms.Button
        Me.csection = New System.Windows.Forms.ComboBox
        Me.lsection = New System.Windows.Forms.Label
        Me.ctemplate = New System.Windows.Forms.ComboBox
        Me.ltemplate = New System.Windows.Forms.Label
        Me.lallitem = New System.Windows.Forms.ListBox
        Me.rdefined = New System.Windows.Forms.RadioButton
        Me.ball = New System.Windows.Forms.Button
        Me.bsaveitem = New System.Windows.Forms.Button
        Me.titemset = New System.Windows.Forms.TextBox
        Me.citemset = New System.Windows.Forms.ComboBox
        Me.ritemset = New System.Windows.Forms.RadioButton
        Me.rportitems = New System.Windows.Forms.RadioButton
        Me.rallitems = New System.Windows.Forms.RadioButton
        Me.bdelitem = New System.Windows.Forms.Button
        Me.bselitem = New System.Windows.Forms.Button
        Me.Label5 = New System.Windows.Forms.Label
        Me.lcontext = New System.Windows.Forms.Label
        Me.ccontext = New System.Windows.Forms.ComboBox
        Me.bdn = New System.Windows.Forms.Button
        Me.lselitem = New System.Windows.Forms.ListBox
        Me.bup = New System.Windows.Forms.Button
        Me.TabPage4 = New System.Windows.Forms.TabPage
        Me.Pparam1 = New System.Windows.Forms.Panel
        Me.Cparam1 = New System.Windows.Forms.ComboBox
        Me.lparam1 = New System.Windows.Forms.Label
        Me.cperiods = New System.Windows.Forms.ListBox
        Me.lperiods = New System.Windows.Forms.ListBox
        Me.bdelper = New System.Windows.Forms.Button
        Me.baddper = New System.Windows.Forms.Button
        Me.Label15 = New System.Windows.Forms.Label
        Me.Label14 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.cyearend = New System.Windows.Forms.ComboBox
        Me.Label13 = New System.Windows.Forms.Label
        Me.Cyearstart = New System.Windows.Forms.ComboBox
        Me.DateTimePicker1 = New System.Windows.Forms.DateTimePicker
        Me.Rdate = New System.Windows.Forms.RadioButton
        Me.rnow = New System.Windows.Forms.RadioButton
        Me.ldateto = New System.Windows.Forms.Label
        Me.Ccont = New System.Windows.Forms.ComboBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.cstage = New System.Windows.Forms.ComboBox
        Me.pdatefrom = New System.Windows.Forms.Panel
        Me.DateTimePicker2 = New System.Windows.Forms.DateTimePicker
        Me.rdatefrom = New System.Windows.Forms.RadioButton
        Me.rold = New System.Windows.Forms.RadioButton
        Me.Label17 = New System.Windows.Forms.Label
        Me.Pyearenddate = New System.Windows.Forms.Panel
        Me.DateTimePicker3 = New System.Windows.Forms.DateTimePicker
        Me.Label2 = New System.Windows.Forms.Label
        Me.pdatetype = New System.Windows.Forms.Panel
        Me.Label1 = New System.Windows.Forms.Label
        Me.Rcalendar = New System.Windows.Forms.RadioButton
        Me.Rworking = New System.Windows.Forms.RadioButton
        Me.Output = New System.Windows.Forms.TabPage
        Me.pformat = New System.Windows.Forms.Panel
        Me.Label11 = New System.Windows.Forms.Label
        Me.tsubtitle = New System.Windows.Forms.TextBox
        Me.tsource = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.ttitle = New System.Windows.Forms.TextBox
        Me.Title = New System.Windows.Forms.Label
        Me.rsheet = New System.Windows.Forms.RadioButton
        Me.rcell = New System.Windows.Forms.RadioButton
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.rglobalsave = New System.Windows.Forms.RadioButton
        Me.Rprivatesave = New System.Windows.Forms.RadioButton
        Me.Rnosave = New System.Windows.Forms.RadioButton
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.lchartfocus = New System.Windows.Forms.Label
        Me.cchartfocus = New System.Windows.Forms.ComboBox
        Me.cchart = New System.Windows.Forms.CheckBox
        Me.ccurr = New System.Windows.Forms.ComboBox
        Me.cprectype = New System.Windows.Forms.ComboBox
        Me.rcurrconv = New System.Windows.Forms.RadioButton
        Me.rcurrshow = New System.Windows.Forms.RadioButton
        Me.cprec = New System.Windows.Forms.ComboBox
        Me.cformat = New System.Windows.Forms.CheckBox
        Me.c_show_ea = New System.Windows.Forms.CheckBox
        Me.rcurrnone = New System.Windows.Forms.RadioButton
        Me.Label16 = New System.Windows.Forms.Label
        Me.ldesc = New System.Windows.Forms.TextBox
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.ppropress = New System.Windows.Forms.ProgressBar
        Me.bcancel = New System.Windows.Forms.PictureBox
        Me.Label20 = New System.Windows.Forms.Label
        Me.PictureBox3 = New System.Windows.Forms.PictureBox
        Me.bcanceldisabled = New System.Windows.Forms.PictureBox
        Me.bok = New System.Windows.Forms.PictureBox
        Me.bforwarddisabled = New System.Windows.Forms.PictureBox
        Me.bforward = New System.Windows.Forms.PictureBox
        Me.bokdisabled = New System.Windows.Forms.PictureBox
        Me.bleft = New System.Windows.Forms.PictureBox
        Me.bleftdisabled = New System.Windows.Forms.PictureBox
        Me.twizard.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.Popen.SuspendLayout()
        Me.Ppredefined.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.Universe.SuspendLayout()
        Me.puniverse.SuspendLayout()
        Me.cass.SuspendLayout()
        Me.pleadentity.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        Me.TabPage4.SuspendLayout()
        Me.Pparam1.SuspendLayout()
        Me.pdatefrom.SuspendLayout()
        Me.Pyearenddate.SuspendLayout()
        Me.pdatetype.SuspendLayout()
        Me.Output.SuspendLayout()
        Me.pformat.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel3.SuspendLayout()
        CType(Me.bcancel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bcanceldisabled, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bok, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bforwarddisabled, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bforward, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bokdisabled, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bleft, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bleftdisabled, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'twizard
        '
        Me.twizard.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.twizard.Controls.Add(Me.TabPage1)
        Me.twizard.Controls.Add(Me.TabPage2)
        Me.twizard.Controls.Add(Me.Universe)
        Me.twizard.Controls.Add(Me.TabPage3)
        Me.twizard.Controls.Add(Me.TabPage4)
        Me.twizard.Controls.Add(Me.Output)
        Me.twizard.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.twizard.ImageList = Me.uxToolBarImages
        Me.twizard.Location = New System.Drawing.Point(16, 17)
        Me.twizard.Name = "twizard"
        Me.twizard.SelectedIndex = 0
        Me.twizard.Size = New System.Drawing.Size(512, 388)
        Me.twizard.TabIndex = 0
        '
        'TabPage1
        '
        Me.TabPage1.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.TabPage1.Controls.Add(Me.Rpredefined)
        Me.TabPage1.Controls.Add(Me.Popen)
        Me.TabPage1.Controls.Add(Me.ltype)
        Me.TabPage1.Controls.Add(Me.rnew)
        Me.TabPage1.Controls.Add(Me.ropen)
        Me.TabPage1.Controls.Add(Me.tname)
        Me.TabPage1.Controls.Add(Me.Label12)
        Me.TabPage1.Controls.Add(Me.cfunction)
        Me.TabPage1.ImageIndex = 2
        Me.TabPage1.Location = New System.Drawing.Point(4, 23)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Size = New System.Drawing.Size(504, 361)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Extract"
        '
        'Rpredefined
        '
        Me.Rpredefined.Location = New System.Drawing.Point(328, 16)
        Me.Rpredefined.Name = "Rpredefined"
        Me.Rpredefined.Size = New System.Drawing.Size(184, 16)
        Me.Rpredefined.TabIndex = 9
        Me.Rpredefined.Text = "Open Predefined Extract"
        '
        'Popen
        '
        Me.Popen.Controls.Add(Me.Ppredefined)
        Me.Popen.Controls.Add(Me.bdelport)
        Me.Popen.Controls.Add(Me.Rprivate)
        Me.Popen.Controls.Add(Me.rglobal)
        Me.Popen.Controls.Add(Me.bopport)
        Me.Popen.Controls.Add(Me.lportfolios)
        Me.Popen.Location = New System.Drawing.Point(0, 32)
        Me.Popen.Name = "Popen"
        Me.Popen.Size = New System.Drawing.Size(500, 296)
        Me.Popen.TabIndex = 5
        Me.Popen.Visible = False
        '
        'Ppredefined
        '
        Me.Ppredefined.Controls.Add(Me.Bopenpd)
        Me.Ppredefined.Controls.Add(Me.Lpredefined)
        Me.Ppredefined.Location = New System.Drawing.Point(3, 1)
        Me.Ppredefined.Name = "Ppredefined"
        Me.Ppredefined.Size = New System.Drawing.Size(501, 295)
        Me.Ppredefined.TabIndex = 7
        '
        'Bopenpd
        '
        Me.Bopenpd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Bopenpd.BackColor = System.Drawing.SystemColors.ControlLight
        Me.Bopenpd.Enabled = False
        Me.Bopenpd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Bopenpd.ImageList = Me.uxToolBarImages
        Me.Bopenpd.Location = New System.Drawing.Point(417, 261)
        Me.Bopenpd.Name = "Bopenpd"
        Me.Bopenpd.Size = New System.Drawing.Size(80, 24)
        Me.Bopenpd.TabIndex = 6
        Me.Bopenpd.Text = "Open"
        Me.Bopenpd.UseVisualStyleBackColor = False
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
        'Lpredefined
        '
        Me.Lpredefined.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Lpredefined.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader5})
        Me.Lpredefined.FullRowSelect = True
        Me.Lpredefined.HideSelection = False
        Me.Lpredefined.Location = New System.Drawing.Point(12, 5)
        Me.Lpredefined.MultiSelect = False
        Me.Lpredefined.Name = "Lpredefined"
        Me.Lpredefined.Size = New System.Drawing.Size(493, 287)
        Me.Lpredefined.TabIndex = 3
        Me.Lpredefined.UseCompatibleStateImageBehavior = False
        Me.Lpredefined.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "Name"
        Me.ColumnHeader5.Width = 481
        '
        'bdelport
        '
        Me.bdelport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bdelport.BackColor = System.Drawing.SystemColors.ControlLight
        Me.bdelport.Enabled = False
        Me.bdelport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.bdelport.Location = New System.Drawing.Point(308, 264)
        Me.bdelport.Name = "bdelport"
        Me.bdelport.Size = New System.Drawing.Size(80, 24)
        Me.bdelport.TabIndex = 6
        Me.bdelport.Text = "Delete"
        Me.bdelport.UseVisualStyleBackColor = False
        '
        'Rprivate
        '
        Me.Rprivate.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Rprivate.Checked = True
        Me.Rprivate.Location = New System.Drawing.Point(16, 264)
        Me.Rprivate.Name = "Rprivate"
        Me.Rprivate.Size = New System.Drawing.Size(72, 24)
        Me.Rprivate.TabIndex = 1
        Me.Rprivate.TabStop = True
        Me.Rprivate.Text = "Private"
        '
        'rglobal
        '
        Me.rglobal.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.rglobal.Location = New System.Drawing.Point(96, 264)
        Me.rglobal.Name = "rglobal"
        Me.rglobal.Size = New System.Drawing.Size(112, 24)
        Me.rglobal.TabIndex = 0
        Me.rglobal.Text = "Public"
        '
        'bopport
        '
        Me.bopport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bopport.BackColor = System.Drawing.SystemColors.ControlLight
        Me.bopport.Enabled = False
        Me.bopport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.bopport.ImageList = Me.uxToolBarImages
        Me.bopport.Location = New System.Drawing.Point(404, 264)
        Me.bopport.Name = "bopport"
        Me.bopport.Size = New System.Drawing.Size(80, 24)
        Me.bopport.TabIndex = 5
        Me.bopport.Text = "Open"
        Me.bopport.UseVisualStyleBackColor = False
        '
        'lportfolios
        '
        Me.lportfolios.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lportfolios.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.id, Me.ColumnHeader4})
        Me.lportfolios.FullRowSelect = True
        Me.lportfolios.HideSelection = False
        Me.lportfolios.Location = New System.Drawing.Point(15, 8)
        Me.lportfolios.MultiSelect = False
        Me.lportfolios.Name = "lportfolios"
        Me.lportfolios.Size = New System.Drawing.Size(464, 248)
        Me.lportfolios.TabIndex = 2
        Me.lportfolios.UseCompatibleStateImageBehavior = False
        Me.lportfolios.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Name"
        Me.ColumnHeader1.Width = 183
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Last Update Date"
        Me.ColumnHeader2.Width = 112
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Created By"
        Me.ColumnHeader3.Width = 113
        '
        'id
        '
        Me.id.Width = 0
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Type"
        '
        'ltype
        '
        Me.ltype.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ltype.Location = New System.Drawing.Point(24, 40)
        Me.ltype.Name = "ltype"
        Me.ltype.Size = New System.Drawing.Size(37, 24)
        Me.ltype.TabIndex = 8
        Me.ltype.Text = "Type"
        '
        'rnew
        '
        Me.rnew.Checked = True
        Me.rnew.Location = New System.Drawing.Point(16, 16)
        Me.rnew.Name = "rnew"
        Me.rnew.Size = New System.Drawing.Size(144, 16)
        Me.rnew.TabIndex = 6
        Me.rnew.TabStop = True
        Me.rnew.Text = " New Extract"
        '
        'ropen
        '
        Me.ropen.Location = New System.Drawing.Point(168, 16)
        Me.ropen.Name = "ropen"
        Me.ropen.Size = New System.Drawing.Size(184, 16)
        Me.ropen.TabIndex = 7
        Me.ropen.Text = "Open Existing Extract"
        '
        'tname
        '
        Me.tname.Location = New System.Drawing.Point(24, 128)
        Me.tname.MaxLength = 250
        Me.tname.Name = "tname"
        Me.tname.Size = New System.Drawing.Size(272, 21)
        Me.tname.TabIndex = 3
        '
        'Label12
        '
        Me.Label12.Location = New System.Drawing.Point(24, 96)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(232, 24)
        Me.Label12.TabIndex = 2
        Me.Label12.Text = "Extract Name"
        '
        'cfunction
        '
        Me.cfunction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cfunction.Enabled = False
        Me.cfunction.Location = New System.Drawing.Point(24, 64)
        Me.cfunction.Name = "cfunction"
        Me.cfunction.Size = New System.Drawing.Size(272, 21)
        Me.cfunction.TabIndex = 0
        '
        'TabPage2
        '
        Me.TabPage2.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.TabPage2.Controls.Add(Me.clink)
        Me.TabPage2.Controls.Add(Me.llink)
        Me.TabPage2.Controls.Add(Me.cschema)
        Me.TabPage2.Controls.Add(Me.Label18)
        Me.TabPage2.Controls.Add(Me.lclass)
        Me.TabPage2.Controls.Add(Me.cclass)
        Me.TabPage2.ImageIndex = 8
        Me.TabPage2.Location = New System.Drawing.Point(4, 23)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Size = New System.Drawing.Size(504, 361)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Focus"
        '
        'clink
        '
        Me.clink.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.clink.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.clink.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.clink.Location = New System.Drawing.Point(32, 168)
        Me.clink.Name = "clink"
        Me.clink.Size = New System.Drawing.Size(264, 21)
        Me.clink.TabIndex = 6
        Me.clink.Visible = False
        '
        'llink
        '
        Me.llink.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.llink.Location = New System.Drawing.Point(30, 140)
        Me.llink.Name = "llink"
        Me.llink.Size = New System.Drawing.Size(280, 24)
        Me.llink.TabIndex = 5
        Me.llink.Text = "Include associations"
        Me.llink.Visible = False
        '
        'cschema
        '
        Me.cschema.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cschema.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cschema.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cschema.Location = New System.Drawing.Point(32, 40)
        Me.cschema.Name = "cschema"
        Me.cschema.Size = New System.Drawing.Size(264, 21)
        Me.cschema.TabIndex = 4
        '
        'Label18
        '
        Me.Label18.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.Location = New System.Drawing.Point(32, 16)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(280, 24)
        Me.Label18.TabIndex = 3
        Me.Label18.Text = "Schema"
        '
        'lclass
        '
        Me.lclass.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lclass.Location = New System.Drawing.Point(29, 80)
        Me.lclass.Name = "lclass"
        Me.lclass.Size = New System.Drawing.Size(280, 24)
        Me.lclass.TabIndex = 1
        Me.lclass.Text = "Primary focus"
        '
        'cclass
        '
        Me.cclass.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cclass.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cclass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cclass.Location = New System.Drawing.Point(32, 104)
        Me.cclass.Name = "cclass"
        Me.cclass.Size = New System.Drawing.Size(264, 21)
        Me.cclass.Sorted = True
        Me.cclass.TabIndex = 5
        '
        'Universe
        '
        Me.Universe.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.Universe.Controls.Add(Me.puniverse)
        Me.Universe.Controls.Add(Me.pleadentity)
        Me.Universe.ImageIndex = 5
        Me.Universe.Location = New System.Drawing.Point(4, 23)
        Me.Universe.Name = "Universe"
        Me.Universe.Size = New System.Drawing.Size(504, 361)
        Me.Universe.TabIndex = 2
        Me.Universe.Text = "Universe"
        '
        'puniverse
        '
        Me.puniverse.Controls.Add(Me.BlueCurve_Unversefilter)
        Me.puniverse.Controls.Add(Me.Lfilter)
        Me.puniverse.Controls.Add(Me.Cfentity)
        Me.puniverse.Controls.Add(Me.Cfclass)
        Me.puniverse.Controls.Add(Me.tentname)
        Me.puniverse.Controls.Add(Me.Lselass)
        Me.puniverse.Controls.Add(Me.cass)
        Me.puniverse.Controls.Add(Me.bentdn)
        Me.puniverse.Controls.Add(Me.cclass3)
        Me.puniverse.Controls.Add(Me.cand)
        Me.puniverse.Controls.Add(Me.centityset)
        Me.puniverse.Controls.Add(Me.rchoose)
        Me.puniverse.Controls.Add(Me.centity2)
        Me.puniverse.Controls.Add(Me.cclass2)
        Me.puniverse.Controls.Add(Me.bsaveent)
        Me.puniverse.Controls.Add(Me.Bdel)
        Me.puniverse.Controls.Add(Me.rentity_set)
        Me.puniverse.Controls.Add(Me.centity)
        Me.puniverse.Controls.Add(Me.rclass)
        Me.puniverse.Controls.Add(Me.rall)
        Me.puniverse.Controls.Add(Me.lallass)
        Me.puniverse.Controls.Add(Me.lallentity)
        Me.puniverse.Controls.Add(Me.lselentity)
        Me.puniverse.Controls.Add(Me.bselent)
        Me.puniverse.Controls.Add(Me.brement)
        Me.puniverse.Controls.Add(Me.Button1)
        Me.puniverse.Controls.Add(Me.bentup)
        Me.puniverse.Location = New System.Drawing.Point(8, 8)
        Me.puniverse.Name = "puniverse"
        Me.puniverse.Size = New System.Drawing.Size(488, 345)
        Me.puniverse.TabIndex = 0
        '
        'BlueCurve_Unversefilter
        '
        Me.BlueCurve_Unversefilter.BackColor = System.Drawing.Color.Transparent
        Me.BlueCurve_Unversefilter.Location = New System.Drawing.Point(4, 312)
        Me.BlueCurve_Unversefilter.MaximumSize = New System.Drawing.Size(30000, 30000)
        Me.BlueCurve_Unversefilter.MinimumSize = New System.Drawing.Size(150, 21)
        Me.BlueCurve_Unversefilter.Name = "BlueCurve_Unversefilter"
        Me.BlueCurve_Unversefilter.SearchAttributeList = ""
        Me.BlueCurve_Unversefilter.SearchAttributes = 2
        Me.BlueCurve_Unversefilter.SearchBuildEntitiesOnly = False
        Me.BlueCurve_Unversefilter.SearchClass = 0
        Me.BlueCurve_Unversefilter.SearchControl = "lallentity"
        Me.BlueCurve_Unversefilter.SearchCurrentAttribute = Nothing
        Me.BlueCurve_Unversefilter.SearchFireEventOnly = False
        Me.BlueCurve_Unversefilter.SearchGetInactive = False
        Me.BlueCurve_Unversefilter.SearchText = ""
        Me.BlueCurve_Unversefilter.SearchTimerOff = False
        Me.BlueCurve_Unversefilter.SearchUserEntitiesOnly = False
        Me.BlueCurve_Unversefilter.Size = New System.Drawing.Size(192, 22)
        Me.BlueCurve_Unversefilter.TabIndex = 38
        '
        'Lfilter
        '
        Me.Lfilter.AutoSize = True
        Me.Lfilter.Location = New System.Drawing.Point(245, 26)
        Me.Lfilter.Name = "Lfilter"
        Me.Lfilter.Size = New System.Drawing.Size(53, 13)
        Me.Lfilter.TabIndex = 44
        Me.Lfilter.Text = "Filter by"
        '
        'Cfentity
        '
        Me.Cfentity.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.Cfentity.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.Cfentity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Cfentity.Enabled = False
        Me.Cfentity.Location = New System.Drawing.Point(305, 51)
        Me.Cfentity.Name = "Cfentity"
        Me.Cfentity.Size = New System.Drawing.Size(173, 21)
        Me.Cfentity.Sorted = True
        Me.Cfentity.TabIndex = 43
        '
        'Cfclass
        '
        Me.Cfclass.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.Cfclass.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.Cfclass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Cfclass.Location = New System.Drawing.Point(304, 23)
        Me.Cfclass.Name = "Cfclass"
        Me.Cfclass.Size = New System.Drawing.Size(173, 21)
        Me.Cfclass.Sorted = True
        Me.Cfclass.TabIndex = 42
        '
        'tentname
        '
        Me.tentname.Enabled = False
        Me.tentname.Location = New System.Drawing.Point(249, 133)
        Me.tentname.MaxLength = 250
        Me.tentname.Name = "tentname"
        Me.tentname.Size = New System.Drawing.Size(192, 21)
        Me.tentname.TabIndex = 26
        Me.tentname.Visible = False
        '
        'Lselass
        '
        Me.Lselass.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Lselass.HorizontalScrollbar = True
        Me.Lselass.Location = New System.Drawing.Point(249, 159)
        Me.Lselass.Name = "Lselass"
        Me.Lselass.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.Lselass.Size = New System.Drawing.Size(193, 147)
        Me.Lselass.Sorted = True
        Me.Lselass.TabIndex = 40
        Me.Lselass.Visible = False
        '
        'cass
        '
        Me.cass.Controls.Add(Me.TabPage5)
        Me.cass.Controls.Add(Me.TabPage6)
        Me.cass.Location = New System.Drawing.Point(4, 137)
        Me.cass.Name = "cass"
        Me.cass.SelectedIndex = 0
        Me.cass.Size = New System.Drawing.Size(236, 22)
        Me.cass.TabIndex = 36
        '
        'TabPage5
        '
        Me.TabPage5.Location = New System.Drawing.Point(4, 22)
        Me.TabPage5.Name = "TabPage5"
        Me.TabPage5.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage5.Size = New System.Drawing.Size(228, 0)
        Me.TabPage5.TabIndex = 0
        Me.TabPage5.Text = "TabPage5"
        Me.TabPage5.UseVisualStyleBackColor = True
        '
        'TabPage6
        '
        Me.TabPage6.Location = New System.Drawing.Point(4, 22)
        Me.TabPage6.Name = "TabPage6"
        Me.TabPage6.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage6.Size = New System.Drawing.Size(228, 0)
        Me.TabPage6.TabIndex = 1
        Me.TabPage6.Text = "TabPage6"
        Me.TabPage6.UseVisualStyleBackColor = True
        '
        'bentdn
        '
        Me.bentdn.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.bentdn.Enabled = False
        Me.bentdn.ForeColor = System.Drawing.SystemColors.ControlText
        Me.bentdn.ImageIndex = 11
        Me.bentdn.ImageList = Me.ImageList2
        Me.bentdn.Location = New System.Drawing.Point(448, 216)
        Me.bentdn.Name = "bentdn"
        Me.bentdn.Size = New System.Drawing.Size(32, 24)
        Me.bentdn.TabIndex = 34
        Me.bentdn.UseVisualStyleBackColor = False
        '
        'ImageList2
        '
        Me.ImageList2.ImageStream = CType(resources.GetObject("ImageList2.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList2.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList2.Images.SetKeyName(0, "")
        Me.ImageList2.Images.SetKeyName(1, "")
        Me.ImageList2.Images.SetKeyName(2, "")
        Me.ImageList2.Images.SetKeyName(3, "")
        Me.ImageList2.Images.SetKeyName(4, "")
        Me.ImageList2.Images.SetKeyName(5, "")
        Me.ImageList2.Images.SetKeyName(6, "")
        Me.ImageList2.Images.SetKeyName(7, "")
        Me.ImageList2.Images.SetKeyName(8, "")
        Me.ImageList2.Images.SetKeyName(9, "")
        Me.ImageList2.Images.SetKeyName(10, "")
        Me.ImageList2.Images.SetKeyName(11, "")
        '
        'cclass3
        '
        Me.cclass3.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cclass3.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cclass3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cclass3.Location = New System.Drawing.Point(184, 110)
        Me.cclass3.Name = "cclass3"
        Me.cclass3.Size = New System.Drawing.Size(144, 21)
        Me.cclass3.Sorted = True
        Me.cclass3.TabIndex = 31
        Me.cclass3.Visible = False
        '
        'cand
        '
        Me.cand.Location = New System.Drawing.Point(136, 102)
        Me.cand.Name = "cand"
        Me.cand.Size = New System.Drawing.Size(48, 16)
        Me.cand.TabIndex = 30
        Me.cand.Text = "and"
        Me.cand.Visible = False
        '
        'centityset
        '
        Me.centityset.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.centityset.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.centityset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.centityset.Location = New System.Drawing.Point(248, 51)
        Me.centityset.Name = "centityset"
        Me.centityset.Size = New System.Drawing.Size(192, 21)
        Me.centityset.Sorted = True
        Me.centityset.TabIndex = 28
        Me.centityset.Visible = False
        '
        'rchoose
        '
        Me.rchoose.Checked = True
        Me.rchoose.Location = New System.Drawing.Point(4, 14)
        Me.rchoose.Name = "rchoose"
        Me.rchoose.Size = New System.Drawing.Size(236, 37)
        Me.rchoose.TabIndex = 17
        Me.rchoose.TabStop = True
        Me.rchoose.Text = "Choose Entity"
        '
        'centity2
        '
        Me.centity2.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.centity2.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.centity2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.centity2.Location = New System.Drawing.Point(336, 110)
        Me.centity2.Name = "centity2"
        Me.centity2.Size = New System.Drawing.Size(144, 21)
        Me.centity2.Sorted = True
        Me.centity2.TabIndex = 32
        Me.centity2.Visible = False
        '
        'cclass2
        '
        Me.cclass2.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cclass2.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cclass2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cclass2.Location = New System.Drawing.Point(184, 86)
        Me.cclass2.Name = "cclass2"
        Me.cclass2.Size = New System.Drawing.Size(144, 21)
        Me.cclass2.Sorted = True
        Me.cclass2.TabIndex = 19
        Me.cclass2.Visible = False
        '
        'bsaveent
        '
        Me.bsaveent.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.bsaveent.Enabled = False
        Me.bsaveent.ForeColor = System.Drawing.SystemColors.ControlText
        Me.bsaveent.ImageIndex = 1
        Me.bsaveent.ImageList = Me.ImageList2
        Me.bsaveent.Location = New System.Drawing.Point(448, 133)
        Me.bsaveent.Name = "bsaveent"
        Me.bsaveent.Size = New System.Drawing.Size(32, 26)
        Me.bsaveent.TabIndex = 20
        Me.bsaveent.UseVisualStyleBackColor = False
        Me.bsaveent.Visible = False
        '
        'Bdel
        '
        Me.Bdel.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.Bdel.CausesValidation = False
        Me.Bdel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Bdel.ImageIndex = 3
        Me.Bdel.ImageList = Me.ImageList2
        Me.Bdel.Location = New System.Drawing.Point(448, 48)
        Me.Bdel.Name = "Bdel"
        Me.Bdel.Size = New System.Drawing.Size(32, 24)
        Me.Bdel.TabIndex = 29
        Me.Bdel.UseVisualStyleBackColor = False
        Me.Bdel.Visible = False
        '
        'rentity_set
        '
        Me.rentity_set.Location = New System.Drawing.Point(3, 51)
        Me.rentity_set.Name = "rentity_set"
        Me.rentity_set.Size = New System.Drawing.Size(236, 24)
        Me.rentity_set.TabIndex = 27
        Me.rentity_set.Text = "Load previously saved universe"
        '
        'centity
        '
        Me.centity.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.centity.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.centity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.centity.Enabled = False
        Me.centity.Location = New System.Drawing.Point(336, 86)
        Me.centity.Name = "centity"
        Me.centity.Size = New System.Drawing.Size(144, 21)
        Me.centity.Sorted = True
        Me.centity.TabIndex = 23
        Me.centity.Visible = False
        '
        'rclass
        '
        Me.rclass.Location = New System.Drawing.Point(3, 70)
        Me.rclass.Name = "rclass"
        Me.rclass.Size = New System.Drawing.Size(180, 50)
        Me.rclass.TabIndex = 18
        Me.rclass.Text = "All Entity in Class"
        '
        'rall
        '
        Me.rall.Location = New System.Drawing.Point(3, 113)
        Me.rall.Name = "rall"
        Me.rall.Size = New System.Drawing.Size(468, 32)
        Me.rall.TabIndex = 16
        Me.rall.Text = "All Entities"
        '
        'lallass
        '
        Me.lallass.HorizontalScrollbar = True
        Me.lallass.Location = New System.Drawing.Point(4, 159)
        Me.lallass.Name = "lallass"
        Me.lallass.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lallass.Size = New System.Drawing.Size(192, 147)
        Me.lallass.Sorted = True
        Me.lallass.TabIndex = 37
        Me.lallass.Visible = False
        '
        'lallentity
        '
        Me.lallentity.HorizontalScrollbar = True
        Me.lallentity.Location = New System.Drawing.Point(4, 160)
        Me.lallentity.Name = "lallentity"
        Me.lallentity.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lallentity.Size = New System.Drawing.Size(192, 147)
        Me.lallentity.Sorted = True
        Me.lallentity.TabIndex = 21
        '
        'lselentity
        '
        Me.lselentity.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lselentity.HorizontalScrollbar = True
        Me.lselentity.Location = New System.Drawing.Point(248, 160)
        Me.lselentity.Name = "lselentity"
        Me.lselentity.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lselentity.Size = New System.Drawing.Size(194, 147)
        Me.lselentity.Sorted = True
        Me.lselentity.TabIndex = 41
        '
        'bselent
        '
        Me.bselent.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.bselent.Enabled = False
        Me.bselent.ForeColor = System.Drawing.SystemColors.ControlText
        Me.bselent.ImageIndex = 5
        Me.bselent.ImageList = Me.uxToolBarImages
        Me.bselent.Location = New System.Drawing.Point(208, 184)
        Me.bselent.Name = "bselent"
        Me.bselent.Size = New System.Drawing.Size(32, 24)
        Me.bselent.TabIndex = 24
        Me.bselent.UseVisualStyleBackColor = False
        '
        'brement
        '
        Me.brement.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.brement.Enabled = False
        Me.brement.ForeColor = System.Drawing.SystemColors.ControlText
        Me.brement.ImageIndex = 6
        Me.brement.ImageList = Me.uxToolBarImages
        Me.brement.Location = New System.Drawing.Point(208, 216)
        Me.brement.Name = "brement"
        Me.brement.Size = New System.Drawing.Size(32, 24)
        Me.brement.TabIndex = 25
        Me.brement.UseVisualStyleBackColor = False
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.Button1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button1.ImageIndex = 4
        Me.Button1.ImageList = Me.uxToolBarImages
        Me.Button1.Location = New System.Drawing.Point(208, 246)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(32, 24)
        Me.Button1.TabIndex = 45
        Me.Button1.UseVisualStyleBackColor = False
        '
        'bentup
        '
        Me.bentup.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.bentup.Enabled = False
        Me.bentup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.bentup.ImageIndex = 10
        Me.bentup.ImageList = Me.ImageList2
        Me.bentup.Location = New System.Drawing.Point(448, 184)
        Me.bentup.Name = "bentup"
        Me.bentup.Size = New System.Drawing.Size(32, 24)
        Me.bentup.TabIndex = 33
        Me.bentup.UseVisualStyleBackColor = False
        '
        'pleadentity
        '
        Me.pleadentity.Controls.Add(Me.cleadenity)
        Me.pleadentity.Controls.Add(Me.lleadentity)
        Me.pleadentity.Location = New System.Drawing.Point(124, 321)
        Me.pleadentity.Name = "pleadentity"
        Me.pleadentity.Size = New System.Drawing.Size(384, 60)
        Me.pleadentity.TabIndex = 1
        Me.pleadentity.Visible = False
        '
        'cleadenity
        '
        Me.cleadenity.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cleadenity.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cleadenity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cleadenity.Location = New System.Drawing.Point(8, 32)
        Me.cleadenity.Name = "cleadenity"
        Me.cleadenity.Size = New System.Drawing.Size(216, 21)
        Me.cleadenity.Sorted = True
        Me.cleadenity.TabIndex = 29
        Me.cleadenity.Visible = False
        '
        'lleadentity
        '
        Me.lleadentity.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lleadentity.Location = New System.Drawing.Point(7, 8)
        Me.lleadentity.Name = "lleadentity"
        Me.lleadentity.Size = New System.Drawing.Size(313, 24)
        Me.lleadentity.TabIndex = 16
        Me.lleadentity.Text = "I would like historic data"
        Me.lleadentity.Visible = False
        '
        'TabPage3
        '
        Me.TabPage3.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.TabPage3.Controls.Add(Me.BlueCurve_ItemsFilter)
        Me.TabPage3.Controls.Add(Me.bdelitemset)
        Me.TabPage3.Controls.Add(Me.bdelall)
        Me.TabPage3.Controls.Add(Me.csection)
        Me.TabPage3.Controls.Add(Me.lsection)
        Me.TabPage3.Controls.Add(Me.ctemplate)
        Me.TabPage3.Controls.Add(Me.ltemplate)
        Me.TabPage3.Controls.Add(Me.lallitem)
        Me.TabPage3.Controls.Add(Me.rdefined)
        Me.TabPage3.Controls.Add(Me.ball)
        Me.TabPage3.Controls.Add(Me.bsaveitem)
        Me.TabPage3.Controls.Add(Me.titemset)
        Me.TabPage3.Controls.Add(Me.citemset)
        Me.TabPage3.Controls.Add(Me.ritemset)
        Me.TabPage3.Controls.Add(Me.rportitems)
        Me.TabPage3.Controls.Add(Me.rallitems)
        Me.TabPage3.Controls.Add(Me.bdelitem)
        Me.TabPage3.Controls.Add(Me.bselitem)
        Me.TabPage3.Controls.Add(Me.Label5)
        Me.TabPage3.Controls.Add(Me.lcontext)
        Me.TabPage3.Controls.Add(Me.ccontext)
        Me.TabPage3.Controls.Add(Me.bdn)
        Me.TabPage3.Controls.Add(Me.lselitem)
        Me.TabPage3.Controls.Add(Me.bup)
        Me.TabPage3.ImageIndex = 9
        Me.TabPage3.Location = New System.Drawing.Point(4, 23)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Size = New System.Drawing.Size(504, 361)
        Me.TabPage3.TabIndex = 3
        Me.TabPage3.Text = "Items"
        '
        'BlueCurve_ItemsFilter
        '
        Me.BlueCurve_ItemsFilter.BackColor = System.Drawing.Color.Transparent
        Me.BlueCurve_ItemsFilter.Location = New System.Drawing.Point(20, 317)
        Me.BlueCurve_ItemsFilter.MaximumSize = New System.Drawing.Size(30000, 30000)
        Me.BlueCurve_ItemsFilter.MinimumSize = New System.Drawing.Size(150, 21)
        Me.BlueCurve_ItemsFilter.Name = "BlueCurve_ItemsFilter"
        Me.BlueCurve_ItemsFilter.SearchAttributeList = ""
        Me.BlueCurve_ItemsFilter.SearchAttributes = 3
        Me.BlueCurve_ItemsFilter.SearchBuildEntitiesOnly = True
        Me.BlueCurve_ItemsFilter.SearchClass = 0
        Me.BlueCurve_ItemsFilter.SearchControl = "lallitem"
        Me.BlueCurve_ItemsFilter.SearchCurrentAttribute = Nothing
        Me.BlueCurve_ItemsFilter.SearchFireEventOnly = False
        Me.BlueCurve_ItemsFilter.SearchGetInactive = False
        Me.BlueCurve_ItemsFilter.SearchText = ""
        Me.BlueCurve_ItemsFilter.SearchTimerOff = False
        Me.BlueCurve_ItemsFilter.SearchUserEntitiesOnly = False
        Me.BlueCurve_ItemsFilter.Size = New System.Drawing.Size(188, 22)
        Me.BlueCurve_ItemsFilter.TabIndex = 1
        '
        'bdelitemset
        '
        Me.bdelitemset.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bdelitemset.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.bdelitemset.Enabled = False
        Me.bdelitemset.ForeColor = System.Drawing.SystemColors.ControlText
        Me.bdelitemset.ImageIndex = 3
        Me.bdelitemset.ImageList = Me.uxToolBarImages
        Me.bdelitemset.Location = New System.Drawing.Point(296, 16)
        Me.bdelitemset.Name = "bdelitemset"
        Me.bdelitemset.Size = New System.Drawing.Size(32, 24)
        Me.bdelitemset.TabIndex = 28
        Me.bdelitemset.UseVisualStyleBackColor = False
        Me.bdelitemset.Visible = False
        '
        'bdelall
        '
        Me.bdelall.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.bdelall.ForeColor = System.Drawing.SystemColors.ControlText
        Me.bdelall.ImageIndex = 3
        Me.bdelall.ImageList = Me.uxToolBarImages
        Me.bdelall.Location = New System.Drawing.Point(220, 232)
        Me.bdelall.Name = "bdelall"
        Me.bdelall.Size = New System.Drawing.Size(32, 24)
        Me.bdelall.TabIndex = 33
        Me.bdelall.UseVisualStyleBackColor = False
        '
        'csection
        '
        Me.csection.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.csection.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.csection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.csection.Location = New System.Drawing.Point(312, 56)
        Me.csection.Name = "csection"
        Me.csection.Size = New System.Drawing.Size(184, 21)
        Me.csection.TabIndex = 12
        Me.csection.Visible = False
        '
        'lsection
        '
        Me.lsection.Location = New System.Drawing.Point(312, 40)
        Me.lsection.Name = "lsection"
        Me.lsection.Size = New System.Drawing.Size(56, 16)
        Me.lsection.TabIndex = 32
        Me.lsection.Text = "Section"
        Me.lsection.Visible = False
        '
        'ctemplate
        '
        Me.ctemplate.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.ctemplate.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.ctemplate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ctemplate.Location = New System.Drawing.Point(112, 56)
        Me.ctemplate.Name = "ctemplate"
        Me.ctemplate.Size = New System.Drawing.Size(184, 21)
        Me.ctemplate.TabIndex = 17
        Me.ctemplate.Visible = False
        '
        'ltemplate
        '
        Me.ltemplate.Location = New System.Drawing.Point(112, 40)
        Me.ltemplate.Name = "ltemplate"
        Me.ltemplate.Size = New System.Drawing.Size(64, 16)
        Me.ltemplate.TabIndex = 31
        Me.ltemplate.Text = "Template"
        Me.ltemplate.Visible = False
        '
        'lallitem
        '
        Me.lallitem.HorizontalScrollbar = True
        Me.lallitem.Location = New System.Drawing.Point(16, 112)
        Me.lallitem.Name = "lallitem"
        Me.lallitem.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lallitem.Size = New System.Drawing.Size(192, 199)
        Me.lallitem.Sorted = True
        Me.lallitem.TabIndex = 0
        '
        'rdefined
        '
        Me.rdefined.Location = New System.Drawing.Point(16, 56)
        Me.rdefined.Name = "rdefined"
        Me.rdefined.Size = New System.Drawing.Size(96, 16)
        Me.rdefined.TabIndex = 30
        Me.rdefined.Text = "Defined set"
        '
        'ball
        '
        Me.ball.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.ball.Enabled = False
        Me.ball.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ball.ImageIndex = 4
        Me.ball.ImageList = Me.uxToolBarImages
        Me.ball.Location = New System.Drawing.Point(220, 200)
        Me.ball.Name = "ball"
        Me.ball.Size = New System.Drawing.Size(32, 24)
        Me.ball.TabIndex = 29
        Me.ball.UseVisualStyleBackColor = False
        '
        'bsaveitem
        '
        Me.bsaveitem.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bsaveitem.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.bsaveitem.Enabled = False
        Me.bsaveitem.ForeColor = System.Drawing.SystemColors.ControlText
        Me.bsaveitem.ImageIndex = 1
        Me.bsaveitem.ImageList = Me.uxToolBarImages
        Me.bsaveitem.Location = New System.Drawing.Point(462, 88)
        Me.bsaveitem.Name = "bsaveitem"
        Me.bsaveitem.Size = New System.Drawing.Size(32, 24)
        Me.bsaveitem.TabIndex = 5
        Me.bsaveitem.UseVisualStyleBackColor = False
        '
        'titemset
        '
        Me.titemset.Enabled = False
        Me.titemset.Location = New System.Drawing.Point(264, 88)
        Me.titemset.MaxLength = 250
        Me.titemset.Name = "titemset"
        Me.titemset.Size = New System.Drawing.Size(192, 21)
        Me.titemset.TabIndex = 27
        '
        'citemset
        '
        Me.citemset.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.citemset.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.citemset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.citemset.Location = New System.Drawing.Point(112, 16)
        Me.citemset.Name = "citemset"
        Me.citemset.Size = New System.Drawing.Size(184, 21)
        Me.citemset.TabIndex = 26
        Me.citemset.Visible = False
        '
        'ritemset
        '
        Me.ritemset.Checked = True
        Me.ritemset.Location = New System.Drawing.Point(16, 24)
        Me.ritemset.Name = "ritemset"
        Me.ritemset.Size = New System.Drawing.Size(80, 16)
        Me.ritemset.TabIndex = 25
        Me.ritemset.TabStop = True
        Me.ritemset.Text = "Saved set"
        '
        'rportitems
        '
        Me.rportitems.Location = New System.Drawing.Point(16, 88)
        Me.rportitems.Name = "rportitems"
        Me.rportitems.Size = New System.Drawing.Size(224, 16)
        Me.rportitems.TabIndex = 24
        Me.rportitems.Text = "Items used in selected Universe"
        '
        'rallitems
        '
        Me.rallitems.Location = New System.Drawing.Point(416, 16)
        Me.rallitems.Name = "rallitems"
        Me.rallitems.Size = New System.Drawing.Size(88, 16)
        Me.rallitems.TabIndex = 23
        Me.rallitems.Text = "All Items"
        Me.rallitems.Visible = False
        '
        'bdelitem
        '
        Me.bdelitem.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.bdelitem.Enabled = False
        Me.bdelitem.ForeColor = System.Drawing.SystemColors.ControlText
        Me.bdelitem.ImageIndex = 6
        Me.bdelitem.ImageList = Me.uxToolBarImages
        Me.bdelitem.Location = New System.Drawing.Point(220, 168)
        Me.bdelitem.Name = "bdelitem"
        Me.bdelitem.Size = New System.Drawing.Size(32, 24)
        Me.bdelitem.TabIndex = 20
        Me.bdelitem.UseVisualStyleBackColor = False
        '
        'bselitem
        '
        Me.bselitem.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.bselitem.Enabled = False
        Me.bselitem.ForeColor = System.Drawing.SystemColors.ControlText
        Me.bselitem.ImageIndex = 5
        Me.bselitem.ImageList = Me.uxToolBarImages
        Me.bselitem.Location = New System.Drawing.Point(220, 136)
        Me.bselitem.Name = "bselitem"
        Me.bselitem.Size = New System.Drawing.Size(32, 24)
        Me.bselitem.TabIndex = 19
        Me.bselitem.UseVisualStyleBackColor = False
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(320, 16)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(56, 24)
        Me.Label5.TabIndex = 14
        Me.Label5.Text = "Section"
        Me.Label5.Visible = False
        '
        'lcontext
        '
        Me.lcontext.Location = New System.Drawing.Point(240, 40)
        Me.lcontext.Name = "lcontext"
        Me.lcontext.Size = New System.Drawing.Size(56, 16)
        Me.lcontext.TabIndex = 13
        Me.lcontext.Text = "Context"
        Me.lcontext.Visible = False
        '
        'ccontext
        '
        Me.ccontext.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.ccontext.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.ccontext.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ccontext.Location = New System.Drawing.Point(288, 56)
        Me.ccontext.Name = "ccontext"
        Me.ccontext.Size = New System.Drawing.Size(128, 21)
        Me.ccontext.TabIndex = 11
        Me.ccontext.Visible = False
        '
        'bdn
        '
        Me.bdn.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.bdn.Enabled = False
        Me.bdn.ForeColor = System.Drawing.SystemColors.ControlText
        Me.bdn.ImageIndex = 11
        Me.bdn.ImageList = Me.ImageList2
        Me.bdn.Location = New System.Drawing.Point(464, 168)
        Me.bdn.Name = "bdn"
        Me.bdn.Size = New System.Drawing.Size(32, 24)
        Me.bdn.TabIndex = 10
        Me.bdn.UseVisualStyleBackColor = False
        '
        'lselitem
        '
        Me.lselitem.HorizontalScrollbar = True
        Me.lselitem.Location = New System.Drawing.Point(264, 112)
        Me.lselitem.Name = "lselitem"
        Me.lselitem.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lselitem.Size = New System.Drawing.Size(192, 199)
        Me.lselitem.TabIndex = 1
        '
        'bup
        '
        Me.bup.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.bup.Enabled = False
        Me.bup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.bup.ImageIndex = 10
        Me.bup.ImageList = Me.ImageList2
        Me.bup.Location = New System.Drawing.Point(464, 136)
        Me.bup.Name = "bup"
        Me.bup.Size = New System.Drawing.Size(32, 24)
        Me.bup.TabIndex = 9
        Me.bup.UseVisualStyleBackColor = False
        '
        'TabPage4
        '
        Me.TabPage4.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.TabPage4.Controls.Add(Me.Pparam1)
        Me.TabPage4.Controls.Add(Me.cperiods)
        Me.TabPage4.Controls.Add(Me.lperiods)
        Me.TabPage4.Controls.Add(Me.bdelper)
        Me.TabPage4.Controls.Add(Me.baddper)
        Me.TabPage4.Controls.Add(Me.Label15)
        Me.TabPage4.Controls.Add(Me.Label14)
        Me.TabPage4.Controls.Add(Me.Label10)
        Me.TabPage4.Controls.Add(Me.cyearend)
        Me.TabPage4.Controls.Add(Me.Label13)
        Me.TabPage4.Controls.Add(Me.Cyearstart)
        Me.TabPage4.Controls.Add(Me.DateTimePicker1)
        Me.TabPage4.Controls.Add(Me.Rdate)
        Me.TabPage4.Controls.Add(Me.rnow)
        Me.TabPage4.Controls.Add(Me.ldateto)
        Me.TabPage4.Controls.Add(Me.Ccont)
        Me.TabPage4.Controls.Add(Me.Label9)
        Me.TabPage4.Controls.Add(Me.cstage)
        Me.TabPage4.Controls.Add(Me.pdatefrom)
        Me.TabPage4.Controls.Add(Me.Pyearenddate)
        Me.TabPage4.Controls.Add(Me.pdatetype)
        Me.TabPage4.ImageIndex = 7
        Me.TabPage4.Location = New System.Drawing.Point(4, 23)
        Me.TabPage4.Name = "TabPage4"
        Me.TabPage4.Size = New System.Drawing.Size(504, 361)
        Me.TabPage4.TabIndex = 4
        Me.TabPage4.Text = "Criteria"
        '
        'Pparam1
        '
        Me.Pparam1.Controls.Add(Me.Cparam1)
        Me.Pparam1.Controls.Add(Me.lparam1)
        Me.Pparam1.Location = New System.Drawing.Point(7, 300)
        Me.Pparam1.Name = "Pparam1"
        Me.Pparam1.Size = New System.Drawing.Size(493, 24)
        Me.Pparam1.TabIndex = 28
        Me.Pparam1.Visible = False
        '
        'Cparam1
        '
        Me.Cparam1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.Cparam1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.Cparam1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Cparam1.FormattingEnabled = True
        Me.Cparam1.Location = New System.Drawing.Point(323, 1)
        Me.Cparam1.Name = "Cparam1"
        Me.Cparam1.Size = New System.Drawing.Size(153, 21)
        Me.Cparam1.TabIndex = 30
        '
        'lparam1
        '
        Me.lparam1.Location = New System.Drawing.Point(1, 4)
        Me.lparam1.Name = "lparam1"
        Me.lparam1.Size = New System.Drawing.Size(104, 24)
        Me.lparam1.TabIndex = 29
        Me.lparam1.Text = "Param1"
        '
        'cperiods
        '
        Me.cperiods.Enabled = False
        Me.cperiods.Location = New System.Drawing.Point(88, 145)
        Me.cperiods.Name = "cperiods"
        Me.cperiods.Size = New System.Drawing.Size(152, 56)
        Me.cperiods.TabIndex = 23
        '
        'lperiods
        '
        Me.lperiods.Enabled = False
        Me.lperiods.Location = New System.Drawing.Point(328, 145)
        Me.lperiods.Name = "lperiods"
        Me.lperiods.Size = New System.Drawing.Size(152, 56)
        Me.lperiods.TabIndex = 22
        '
        'bdelper
        '
        Me.bdelper.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.bdelper.ForeColor = System.Drawing.SystemColors.ControlText
        Me.bdelper.ImageIndex = 6
        Me.bdelper.ImageList = Me.uxToolBarImages
        Me.bdelper.Location = New System.Drawing.Point(256, 177)
        Me.bdelper.Name = "bdelper"
        Me.bdelper.Size = New System.Drawing.Size(40, 24)
        Me.bdelper.TabIndex = 21
        Me.bdelper.UseVisualStyleBackColor = False
        '
        'baddper
        '
        Me.baddper.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.baddper.ForeColor = System.Drawing.SystemColors.ControlText
        Me.baddper.ImageIndex = 5
        Me.baddper.ImageList = Me.uxToolBarImages
        Me.baddper.Location = New System.Drawing.Point(256, 145)
        Me.baddper.Name = "baddper"
        Me.baddper.Size = New System.Drawing.Size(40, 24)
        Me.baddper.TabIndex = 20
        Me.baddper.UseVisualStyleBackColor = False
        '
        'Label15
        '
        Me.Label15.Location = New System.Drawing.Point(6, 145)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(104, 24)
        Me.Label15.TabIndex = 19
        Me.Label15.Text = "Periods"
        '
        'Label14
        '
        Me.Label14.Location = New System.Drawing.Point(248, 104)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(72, 24)
        Me.Label14.TabIndex = 18
        Me.Label14.Text = "End Year"
        '
        'Label10
        '
        Me.Label10.Location = New System.Drawing.Point(6, 60)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(80, 24)
        Me.Label10.TabIndex = 5
        Me.Label10.Text = "Contributor"
        '
        'cyearend
        '
        Me.cyearend.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cyearend.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cyearend.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cyearend.Enabled = False
        Me.cyearend.Location = New System.Drawing.Point(328, 104)
        Me.cyearend.Name = "cyearend"
        Me.cyearend.Size = New System.Drawing.Size(152, 21)
        Me.cyearend.TabIndex = 17
        '
        'Label13
        '
        Me.Label13.Location = New System.Drawing.Point(8, 104)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(72, 24)
        Me.Label13.TabIndex = 16
        Me.Label13.Text = "Start Year"
        '
        'Cyearstart
        '
        Me.Cyearstart.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.Cyearstart.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.Cyearstart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Cyearstart.Enabled = False
        Me.Cyearstart.Location = New System.Drawing.Point(88, 104)
        Me.Cyearstart.Name = "Cyearstart"
        Me.Cyearstart.Size = New System.Drawing.Size(152, 21)
        Me.Cyearstart.TabIndex = 14
        '
        'DateTimePicker1
        '
        Me.DateTimePicker1.Location = New System.Drawing.Point(331, 243)
        Me.DateTimePicker1.Name = "DateTimePicker1"
        Me.DateTimePicker1.Size = New System.Drawing.Size(152, 21)
        Me.DateTimePicker1.TabIndex = 9
        '
        'Rdate
        '
        Me.Rdate.Location = New System.Drawing.Point(207, 238)
        Me.Rdate.Name = "Rdate"
        Me.Rdate.Size = New System.Drawing.Size(112, 32)
        Me.Rdate.TabIndex = 8
        Me.Rdate.Text = "at"
        '
        'rnow
        '
        Me.rnow.Location = New System.Drawing.Point(119, 237)
        Me.rnow.Name = "rnow"
        Me.rnow.Size = New System.Drawing.Size(88, 32)
        Me.rnow.TabIndex = 7
        Me.rnow.Text = "Current"
        '
        'ldateto
        '
        Me.ldateto.Location = New System.Drawing.Point(7, 245)
        Me.ldateto.Name = "ldateto"
        Me.ldateto.Size = New System.Drawing.Size(104, 24)
        Me.ldateto.TabIndex = 6
        Me.ldateto.Text = "Date of data"
        '
        'Ccont
        '
        Me.Ccont.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.Ccont.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.Ccont.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Ccont.Location = New System.Drawing.Point(88, 60)
        Me.Ccont.Name = "Ccont"
        Me.Ccont.Size = New System.Drawing.Size(152, 21)
        Me.Ccont.TabIndex = 3
        '
        'Label9
        '
        Me.Label9.Location = New System.Drawing.Point(8, 16)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(72, 24)
        Me.Label9.TabIndex = 2
        Me.Label9.Text = "Stage"
        '
        'cstage
        '
        Me.cstage.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cstage.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cstage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cstage.Location = New System.Drawing.Point(88, 16)
        Me.cstage.Name = "cstage"
        Me.cstage.Size = New System.Drawing.Size(152, 21)
        Me.cstage.TabIndex = 0
        '
        'pdatefrom
        '
        Me.pdatefrom.Controls.Add(Me.DateTimePicker2)
        Me.pdatefrom.Controls.Add(Me.rdatefrom)
        Me.pdatefrom.Controls.Add(Me.rold)
        Me.pdatefrom.Controls.Add(Me.Label17)
        Me.pdatefrom.Location = New System.Drawing.Point(3, 207)
        Me.pdatefrom.Name = "pdatefrom"
        Me.pdatefrom.Size = New System.Drawing.Size(480, 32)
        Me.pdatefrom.TabIndex = 25
        Me.pdatefrom.Visible = False
        '
        'DateTimePicker2
        '
        Me.DateTimePicker2.Location = New System.Drawing.Point(328, 3)
        Me.DateTimePicker2.Name = "DateTimePicker2"
        Me.DateTimePicker2.Size = New System.Drawing.Size(152, 21)
        Me.DateTimePicker2.TabIndex = 13
        '
        'rdatefrom
        '
        Me.rdatefrom.Location = New System.Drawing.Point(204, -3)
        Me.rdatefrom.Name = "rdatefrom"
        Me.rdatefrom.Size = New System.Drawing.Size(112, 32)
        Me.rdatefrom.TabIndex = 12
        Me.rdatefrom.Text = "from"
        '
        'rold
        '
        Me.rold.Checked = True
        Me.rold.Location = New System.Drawing.Point(116, -3)
        Me.rold.Name = "rold"
        Me.rold.Size = New System.Drawing.Size(88, 32)
        Me.rold.TabIndex = 11
        Me.rold.TabStop = True
        Me.rold.Text = "Oldest"
        '
        'Label17
        '
        Me.Label17.Location = New System.Drawing.Point(5, 5)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(104, 24)
        Me.Label17.TabIndex = 10
        Me.Label17.Text = "Date from"
        '
        'Pyearenddate
        '
        Me.Pyearenddate.Controls.Add(Me.DateTimePicker3)
        Me.Pyearenddate.Controls.Add(Me.Label2)
        Me.Pyearenddate.Location = New System.Drawing.Point(3, 265)
        Me.Pyearenddate.Name = "Pyearenddate"
        Me.Pyearenddate.Size = New System.Drawing.Size(497, 33)
        Me.Pyearenddate.TabIndex = 27
        Me.Pyearenddate.Visible = False
        '
        'DateTimePicker3
        '
        Me.DateTimePicker3.Location = New System.Drawing.Point(328, 9)
        Me.DateTimePicker3.Name = "DateTimePicker3"
        Me.DateTimePicker3.Size = New System.Drawing.Size(152, 21)
        Me.DateTimePicker3.TabIndex = 14
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(3, 9)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(104, 24)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "Year End Date"
        '
        'pdatetype
        '
        Me.pdatetype.Controls.Add(Me.Label1)
        Me.pdatetype.Controls.Add(Me.Rcalendar)
        Me.pdatetype.Controls.Add(Me.Rworking)
        Me.pdatetype.Location = New System.Drawing.Point(12, 276)
        Me.pdatetype.Name = "pdatetype"
        Me.pdatetype.Size = New System.Drawing.Size(316, 25)
        Me.pdatetype.TabIndex = 26
        Me.pdatetype.Visible = False
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(-4, -2)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(104, 24)
        Me.Label1.TabIndex = 27
        Me.Label1.Text = "Date Type"
        '
        'Rcalendar
        '
        Me.Rcalendar.Location = New System.Drawing.Point(195, -7)
        Me.Rcalendar.Name = "Rcalendar"
        Me.Rcalendar.Size = New System.Drawing.Size(88, 32)
        Me.Rcalendar.TabIndex = 9
        Me.Rcalendar.Text = "Calendar"
        '
        'Rworking
        '
        Me.Rworking.Checked = True
        Me.Rworking.Location = New System.Drawing.Point(107, -5)
        Me.Rworking.Name = "Rworking"
        Me.Rworking.Size = New System.Drawing.Size(88, 32)
        Me.Rworking.TabIndex = 8
        Me.Rworking.TabStop = True
        Me.Rworking.Text = "Working"
        '
        'Output
        '
        Me.Output.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.Output.Controls.Add(Me.pformat)
        Me.Output.Controls.Add(Me.rsheet)
        Me.Output.Controls.Add(Me.rcell)
        Me.Output.Controls.Add(Me.Panel1)
        Me.Output.Controls.Add(Me.Label7)
        Me.Output.Controls.Add(Me.Panel2)
        Me.Output.ImageIndex = 10
        Me.Output.Location = New System.Drawing.Point(4, 23)
        Me.Output.Name = "Output"
        Me.Output.Size = New System.Drawing.Size(504, 361)
        Me.Output.TabIndex = 5
        Me.Output.Text = "Results"
        '
        'pformat
        '
        Me.pformat.Controls.Add(Me.Label11)
        Me.pformat.Controls.Add(Me.tsubtitle)
        Me.pformat.Controls.Add(Me.tsource)
        Me.pformat.Controls.Add(Me.Label4)
        Me.pformat.Controls.Add(Me.ttitle)
        Me.pformat.Controls.Add(Me.Title)
        Me.pformat.Location = New System.Drawing.Point(0, 174)
        Me.pformat.Name = "pformat"
        Me.pformat.Size = New System.Drawing.Size(512, 96)
        Me.pformat.TabIndex = 13
        '
        'Label11
        '
        Me.Label11.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(8, 40)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(64, 16)
        Me.Label11.TabIndex = 5
        Me.Label11.Text = "Sub title"
        '
        'tsubtitle
        '
        Me.tsubtitle.Location = New System.Drawing.Point(72, 40)
        Me.tsubtitle.MaxLength = 250
        Me.tsubtitle.Name = "tsubtitle"
        Me.tsubtitle.Size = New System.Drawing.Size(224, 21)
        Me.tsubtitle.TabIndex = 4
        '
        'tsource
        '
        Me.tsource.Location = New System.Drawing.Point(72, 72)
        Me.tsource.MaxLength = 250
        Me.tsource.Name = "tsource"
        Me.tsource.Size = New System.Drawing.Size(224, 21)
        Me.tsource.TabIndex = 0
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(8, 72)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(56, 16)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Source"
        '
        'ttitle
        '
        Me.ttitle.Location = New System.Drawing.Point(72, 8)
        Me.ttitle.MaxLength = 250
        Me.ttitle.Name = "ttitle"
        Me.ttitle.Size = New System.Drawing.Size(224, 21)
        Me.ttitle.TabIndex = 1
        '
        'Title
        '
        Me.Title.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Title.Location = New System.Drawing.Point(8, 9)
        Me.Title.Name = "Title"
        Me.Title.Size = New System.Drawing.Size(40, 16)
        Me.Title.TabIndex = 2
        Me.Title.Text = "Title"
        '
        'rsheet
        '
        Me.rsheet.Checked = True
        Me.rsheet.Location = New System.Drawing.Point(96, 16)
        Me.rsheet.Name = "rsheet"
        Me.rsheet.Size = New System.Drawing.Size(112, 16)
        Me.rsheet.TabIndex = 0
        Me.rsheet.TabStop = True
        Me.rsheet.Tag = ""
        Me.rsheet.Text = "new worksheet"
        '
        'rcell
        '
        Me.rcell.Location = New System.Drawing.Point(246, 14)
        Me.rcell.Name = "rcell"
        Me.rcell.Size = New System.Drawing.Size(208, 16)
        Me.rcell.TabIndex = 1
        Me.rcell.Text = "current worksheet selected cell"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.rglobalsave)
        Me.Panel1.Controls.Add(Me.Rprivatesave)
        Me.Panel1.Controls.Add(Me.Rnosave)
        Me.Panel1.Controls.Add(Me.Label8)
        Me.Panel1.Location = New System.Drawing.Point(0, 267)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(456, 91)
        Me.Panel1.TabIndex = 9
        '
        'rglobalsave
        '
        Me.rglobalsave.Location = New System.Drawing.Point(16, 64)
        Me.rglobalsave.Name = "rglobalsave"
        Me.rglobalsave.Size = New System.Drawing.Size(402, 24)
        Me.rglobalsave.TabIndex = 12
        Me.rglobalsave.Tag = "If Me.rglobal.Checked = True Then"
        Me.rglobalsave.Text = "for public use"
        '
        'Rprivatesave
        '
        Me.Rprivatesave.Location = New System.Drawing.Point(16, 41)
        Me.Rprivatesave.Name = "Rprivatesave"
        Me.Rprivatesave.Size = New System.Drawing.Size(410, 24)
        Me.Rprivatesave.TabIndex = 11
        Me.Rprivatesave.Tag = "If Me.rglobal.Checked = True Then"
        Me.Rprivatesave.Text = "for private use"
        '
        'Rnosave
        '
        Me.Rnosave.Checked = True
        Me.Rnosave.Location = New System.Drawing.Point(16, 24)
        Me.Rnosave.Name = "Rnosave"
        Me.Rnosave.Size = New System.Drawing.Size(416, 16)
        Me.Rnosave.TabIndex = 10
        Me.Rnosave.TabStop = True
        Me.Rnosave.Text = "no save"
        '
        'Label8
        '
        Me.Label8.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(8, 6)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(228, 24)
        Me.Label8.TabIndex = 9
        Me.Label8.Text = "Extract save options"
        '
        'Label7
        '
        Me.Label7.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(8, 15)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(192, 24)
        Me.Label7.TabIndex = 3
        Me.Label7.Text = "Output"
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.lchartfocus)
        Me.Panel2.Controls.Add(Me.cchartfocus)
        Me.Panel2.Controls.Add(Me.cchart)
        Me.Panel2.Controls.Add(Me.ccurr)
        Me.Panel2.Controls.Add(Me.cprectype)
        Me.Panel2.Controls.Add(Me.rcurrconv)
        Me.Panel2.Controls.Add(Me.rcurrshow)
        Me.Panel2.Controls.Add(Me.cprec)
        Me.Panel2.Controls.Add(Me.cformat)
        Me.Panel2.Controls.Add(Me.c_show_ea)
        Me.Panel2.Controls.Add(Me.rcurrnone)
        Me.Panel2.Controls.Add(Me.Label16)
        Me.Panel2.Location = New System.Drawing.Point(-1, 40)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(502, 136)
        Me.Panel2.TabIndex = 12
        '
        'lchartfocus
        '
        Me.lchartfocus.AutoSize = True
        Me.lchartfocus.Location = New System.Drawing.Point(109, 110)
        Me.lchartfocus.Name = "lchartfocus"
        Me.lchartfocus.Size = New System.Drawing.Size(39, 13)
        Me.lchartfocus.TabIndex = 32
        Me.lchartfocus.Text = "Focus"
        '
        'cchartfocus
        '
        Me.cchartfocus.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cchartfocus.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cchartfocus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cchartfocus.Enabled = False
        Me.cchartfocus.Location = New System.Drawing.Point(176, 106)
        Me.cchartfocus.Name = "cchartfocus"
        Me.cchartfocus.Size = New System.Drawing.Size(264, 21)
        Me.cchartfocus.TabIndex = 31
        '
        'cchart
        '
        Me.cchart.AutoSize = True
        Me.cchart.Enabled = False
        Me.cchart.Location = New System.Drawing.Point(16, 108)
        Me.cchart.Name = "cchart"
        Me.cchart.Size = New System.Drawing.Size(89, 17)
        Me.cchart.TabIndex = 30
        Me.cchart.Text = "Chart Data"
        Me.cchart.UseVisualStyleBackColor = True
        '
        'ccurr
        '
        Me.ccurr.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.ccurr.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.ccurr.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ccurr.Enabled = False
        Me.ccurr.Location = New System.Drawing.Point(411, 54)
        Me.ccurr.Name = "ccurr"
        Me.ccurr.Size = New System.Drawing.Size(88, 21)
        Me.ccurr.TabIndex = 13
        '
        'cprectype
        '
        Me.cprectype.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cprectype.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cprectype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cprectype.Location = New System.Drawing.Point(176, 78)
        Me.cprectype.Name = "cprectype"
        Me.cprectype.Size = New System.Drawing.Size(152, 21)
        Me.cprectype.TabIndex = 29
        '
        'rcurrconv
        '
        Me.rcurrconv.Location = New System.Drawing.Point(227, 45)
        Me.rcurrconv.Name = "rcurrconv"
        Me.rcurrconv.Size = New System.Drawing.Size(192, 37)
        Me.rcurrconv.TabIndex = 11
        Me.rcurrconv.Text = "convert all items to currency"
        '
        'rcurrshow
        '
        Me.rcurrshow.Checked = True
        Me.rcurrshow.Location = New System.Drawing.Point(16, 54)
        Me.rcurrshow.Name = "rcurrshow"
        Me.rcurrshow.Size = New System.Drawing.Size(96, 17)
        Me.rcurrshow.TabIndex = 10
        Me.rcurrshow.TabStop = True
        Me.rcurrshow.Text = "no currency "
        '
        'cprec
        '
        Me.cprec.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cprec.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cprec.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cprec.Location = New System.Drawing.Point(104, 78)
        Me.cprec.Name = "cprec"
        Me.cprec.Size = New System.Drawing.Size(64, 21)
        Me.cprec.TabIndex = 28
        '
        'cformat
        '
        Me.cformat.Location = New System.Drawing.Point(16, 81)
        Me.cformat.Name = "cformat"
        Me.cformat.Size = New System.Drawing.Size(224, 16)
        Me.cformat.TabIndex = 27
        Me.cformat.Text = "Format to "
        '
        'c_show_ea
        '
        Me.c_show_ea.Location = New System.Drawing.Point(16, 28)
        Me.c_show_ea.Name = "c_show_ea"
        Me.c_show_ea.Size = New System.Drawing.Size(224, 16)
        Me.c_show_ea.TabIndex = 26
        Me.c_show_ea.Text = "estimate/actual indicator"
        '
        'rcurrnone
        '
        Me.rcurrnone.Location = New System.Drawing.Point(112, 54)
        Me.rcurrnone.Name = "rcurrnone"
        Me.rcurrnone.Size = New System.Drawing.Size(112, 16)
        Me.rcurrnone.TabIndex = 12
        Me.rcurrnone.Text = " item currency"
        '
        'Label16
        '
        Me.Label16.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.Location = New System.Drawing.Point(7, 5)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(192, 24)
        Me.Label16.TabIndex = 9
        Me.Label16.Text = "Include the following options"
        '
        'ldesc
        '
        Me.ldesc.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ldesc.BackColor = System.Drawing.Color.White
        Me.ldesc.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ldesc.Location = New System.Drawing.Point(16, 409)
        Me.ldesc.Multiline = True
        Me.ldesc.Name = "ldesc"
        Me.ldesc.ReadOnly = True
        Me.ldesc.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal
        Me.ldesc.Size = New System.Drawing.Size(512, 96)
        Me.ldesc.TabIndex = 4
        Me.ldesc.WordWrap = False
        '
        'Panel3
        '
        Me.Panel3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel3.BackColor = System.Drawing.Color.White
        Me.Panel3.Controls.Add(Me.ppropress)
        Me.Panel3.Controls.Add(Me.bcancel)
        Me.Panel3.Controls.Add(Me.Label20)
        Me.Panel3.Controls.Add(Me.PictureBox3)
        Me.Panel3.Controls.Add(Me.bcanceldisabled)
        Me.Panel3.Controls.Add(Me.bok)
        Me.Panel3.Controls.Add(Me.bforwarddisabled)
        Me.Panel3.Controls.Add(Me.bforward)
        Me.Panel3.Controls.Add(Me.bokdisabled)
        Me.Panel3.Controls.Add(Me.bleft)
        Me.Panel3.Controls.Add(Me.bleftdisabled)
        Me.Panel3.Location = New System.Drawing.Point(0, 508)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(553, 72)
        Me.Panel3.TabIndex = 9
        '
        'ppropress
        '
        Me.ppropress.Location = New System.Drawing.Point(344, 61)
        Me.ppropress.Name = "ppropress"
        Me.ppropress.Size = New System.Drawing.Size(176, 8)
        Me.ppropress.TabIndex = 77
        Me.ppropress.Visible = False
        '
        'bcancel
        '
        Me.bcancel.Image = CType(resources.GetObject("bcancel.Image"), System.Drawing.Image)
        Me.bcancel.Location = New System.Drawing.Point(344, 8)
        Me.bcancel.Name = "bcancel"
        Me.bcancel.Size = New System.Drawing.Size(56, 56)
        Me.bcancel.TabIndex = 71
        Me.bcancel.TabStop = False
        '
        'Label20
        '
        Me.Label20.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label20.Location = New System.Drawing.Point(4, 48)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(216, 16)
        Me.Label20.TabIndex = 10
        Me.Label20.Text = "© Blue Curve Limited. All rights reserved."
        '
        'PictureBox3
        '
        Me.PictureBox3.Image = CType(resources.GetObject("PictureBox3.Image"), System.Drawing.Image)
        Me.PictureBox3.Location = New System.Drawing.Point(5, 8)
        Me.PictureBox3.Name = "PictureBox3"
        Me.PictureBox3.Size = New System.Drawing.Size(184, 32)
        Me.PictureBox3.TabIndex = 9
        Me.PictureBox3.TabStop = False
        '
        'bcanceldisabled
        '
        Me.bcanceldisabled.Image = CType(resources.GetObject("bcanceldisabled.Image"), System.Drawing.Image)
        Me.bcanceldisabled.Location = New System.Drawing.Point(344, 8)
        Me.bcanceldisabled.Name = "bcanceldisabled"
        Me.bcanceldisabled.Size = New System.Drawing.Size(56, 56)
        Me.bcanceldisabled.TabIndex = 76
        Me.bcanceldisabled.TabStop = False
        Me.bcanceldisabled.Visible = False
        '
        'bok
        '
        Me.bok.BackColor = System.Drawing.Color.Transparent
        Me.bok.Image = CType(resources.GetObject("bok.Image"), System.Drawing.Image)
        Me.bok.Location = New System.Drawing.Point(472, 8)
        Me.bok.Name = "bok"
        Me.bok.Size = New System.Drawing.Size(56, 56)
        Me.bok.TabIndex = 70
        Me.bok.TabStop = False
        Me.bok.Visible = False
        '
        'bforwarddisabled
        '
        Me.bforwarddisabled.Image = CType(resources.GetObject("bforwarddisabled.Image"), System.Drawing.Image)
        Me.bforwarddisabled.Location = New System.Drawing.Point(472, 8)
        Me.bforwarddisabled.Name = "bforwarddisabled"
        Me.bforwarddisabled.Size = New System.Drawing.Size(56, 56)
        Me.bforwarddisabled.TabIndex = 75
        Me.bforwarddisabled.TabStop = False
        Me.bforwarddisabled.Visible = False
        '
        'bforward
        '
        Me.bforward.BackColor = System.Drawing.Color.Transparent
        Me.bforward.Image = CType(resources.GetObject("bforward.Image"), System.Drawing.Image)
        Me.bforward.Location = New System.Drawing.Point(472, 8)
        Me.bforward.Name = "bforward"
        Me.bforward.Size = New System.Drawing.Size(56, 56)
        Me.bforward.TabIndex = 74
        Me.bforward.TabStop = False
        '
        'bokdisabled
        '
        Me.bokdisabled.BackColor = System.Drawing.Color.Transparent
        Me.bokdisabled.Image = CType(resources.GetObject("bokdisabled.Image"), System.Drawing.Image)
        Me.bokdisabled.Location = New System.Drawing.Point(472, 8)
        Me.bokdisabled.Name = "bokdisabled"
        Me.bokdisabled.Size = New System.Drawing.Size(56, 56)
        Me.bokdisabled.TabIndex = 69
        Me.bokdisabled.TabStop = False
        Me.bokdisabled.Visible = False
        '
        'bleft
        '
        Me.bleft.Image = CType(resources.GetObject("bleft.Image"), System.Drawing.Image)
        Me.bleft.Location = New System.Drawing.Point(408, 8)
        Me.bleft.Name = "bleft"
        Me.bleft.Size = New System.Drawing.Size(56, 56)
        Me.bleft.TabIndex = 72
        Me.bleft.TabStop = False
        Me.bleft.Visible = False
        '
        'bleftdisabled
        '
        Me.bleftdisabled.Image = CType(resources.GetObject("bleftdisabled.Image"), System.Drawing.Image)
        Me.bleftdisabled.Location = New System.Drawing.Point(408, 8)
        Me.bleftdisabled.Name = "bleftdisabled"
        Me.bleftdisabled.Size = New System.Drawing.Size(56, 56)
        Me.bleftdisabled.TabIndex = 73
        Me.bleftdisabled.TabStop = False
        Me.bleftdisabled.Visible = False
        '
        'bc_am_portfolio_wizard
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(544, 576)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.ldesc)
        Me.Controls.Add(Me.twizard)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "bc_am_portfolio_wizard"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Data Extraction Tool - Blue Curve"
        Me.twizard.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.Popen.ResumeLayout(False)
        Me.Ppredefined.ResumeLayout(False)
        Me.TabPage2.ResumeLayout(False)
        Me.Universe.ResumeLayout(False)
        Me.puniverse.ResumeLayout(False)
        Me.puniverse.PerformLayout()
        Me.cass.ResumeLayout(False)
        Me.pleadentity.ResumeLayout(False)
        Me.TabPage3.ResumeLayout(False)
        Me.TabPage3.PerformLayout()
        Me.TabPage4.ResumeLayout(False)
        Me.Pparam1.ResumeLayout(False)
        Me.pdatefrom.ResumeLayout(False)
        Me.Pyearenddate.ResumeLayout(False)
        Me.pdatetype.ResumeLayout(False)
        Me.Output.ResumeLayout(False)
        Me.pformat.ResumeLayout(False)
        Me.pformat.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        CType(Me.bcancel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bcanceldisabled, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bok, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bforwarddisabled, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bforward, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bokdisabled, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bleft, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bleftdisabled, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region
    Private Class formulas_header
        Public title As String
        Public stage As String
        Public contributor As String
        Public date_at As DateTime
        Public date_from As DateTime
        Public date_to As DateTime
        Public entity_class As String
        Public row_offset As Integer
        Public col_offset As Integer
        Public item_row_offset As Integer
        Public year_row_offset As Integer
        Public period_row_offset As Integer
        Public currency_code As String
        Public prop As String

        Public Sub New()

        End Sub
    End Class
    Private Class formula_entity
        Public header As formulas_header
        Public name As String
        Public associated_name As String
        Public row_offset As Integer
        Public col_offset As Integer

        Public Sub New()

        End Sub
    End Class
    Private Class formula
        Public formula_entity As New formula_entity
        Public row_offset As Integer
        Public col_offset As Integer
        Public item As String
        Public year As String
        Public period As String
        Public formula As String
        Public item_type As String
        Public item_factor As String
        Public item_monatary As Boolean
        Public item_symbol As String
        Public first As Boolean = True
        Public assoc_entity As Boolean = False
        Public Sub New()

        End Sub
    End Class

    Private Function save_portfolio(ByVal globalPF As Boolean) As Boolean
        Try
            Dim i As Integer
            save_portfolio = True


            Dim oportfolio As New bc_om_ef_portolio
            If globalPF = True Then
                oportfolio.public_flag = True
            Else
                oportfolio.public_flag = False
            End If
            If Me.rnew.Checked = True Then
                oportfolio.portfolio_id = 0
            Else
                oportfolio.portfolio_id = selected_portfolio_id

            End If
            oportfolio.user_id = bc_cs_central_settings.logged_on_user_id
            oportfolio.function_name = Me.cfunction.Text
            oportfolio.title = Me.tname.Text
            oportfolio.schema_name = Me.cschema.Text
            oportfolio.class_name = Me.cclass.Text
            oportfolio.table_title = Me.ttitle.Text
            oportfolio.sub_title = Me.tsubtitle.Text
            oportfolio.source = Me.tsource.Text
            oportfolio.associated_class = ""
            If Me.cand.Checked = True And Me.cclass3.SelectedIndex > -1 And Me.centity2.SelectedIndex > -1 Then
                oportfolio.sec_entity_prop_class_name = Me.cclass3.Text
                oportfolio.sec_entity_prop_entity_name = Me.centity2.Text
            End If
            If Me.clink.SelectedIndex > 0 Then
                oportfolio.associated_class = Me.clink.Text
            End If
            If Me.c_show_ea.Checked = True Then
                oportfolio.show_e_a = True
            Else
                oportfolio.show_e_a = False
            End If
            oportfolio.format_type = 0
            oportfolio.precision = 0
            If Me.cformat.Checked = True Then
                oportfolio.format_type = Me.cprectype.SelectedIndex + 1
                If Me.cprec.SelectedIndex > -1 Then
                    oportfolio.precision = CInt(Me.cprec.Text)
                End If
            End If
            If Me.rchoose.Checked = True Then
                oportfolio.universe_flag = 0
            ElseIf Me.rentity_set.Checked = True Then
                oportfolio.universe_flag = 1
                oportfolio.entity_set_name = Me.centityset.Text
            ElseIf Me.rclass.Checked = True Then
                oportfolio.universe_flag = 2
                oportfolio.entity_prop_class_name = Me.cclass2.Text
                oportfolio.entity_prop_entity_name = Me.centity.Text
            ElseIf Me.rall.Checked = True Then
                oportfolio.universe_flag = 3
            End If
            REM now assign entities
            For i = 0 To Me.lselentity.Items.Count - 1
                oportfolio.entities.Add(Me.lselentity.Items(i))
            Next
            REM items
            For i = 0 To Me.lselitem.Items.Count - 1
                oportfolio.items.Add(Me.lselitem.Items(i))
                oportfolio.item_types.Add(Me.item_sel_type(i))
                oportfolio.item_factor.Add(Me.item_sel_factor(i))
                oportfolio.item_monatary.Add(Me.item_sel_monatary(i))
                oportfolio.item_symbol.Add(Me.item_sel_symbol(i))
                oportfolio.item_assoc.Add(Me.item_sel_assoc_class(i))
            Next
            REM criteria
            oportfolio.stage = Me.cstage.Text
            oportfolio.contributor = Me.Ccont.Text
            If Me.rnow.Checked = True Then
                oportfolio.date_at = "9-9-9999"
            Else
                oportfolio.date_at = Me.DateTimePicker1.Value
            End If
            If Me.rold.Checked = True Then
                oportfolio.date_from = "1-1-1900"
            Else
                oportfolio.date_from = Me.DateTimePicker2.Value
            End If
            oportfolio.start_year = Me.Cyearstart.Text
            oportfolio.end_year = Me.cyearend.Text
            For i = 0 To Me.lperiods.Items.Count - 1
                oportfolio.periods.Add(Me.lperiods.Items(i))
            Next
            If Me.rcurrnone.Checked = True Then
                oportfolio.convert_type = 1

            ElseIf Me.rcurrshow.Checked = True Then
                oportfolio.convert_type = 0

            ElseIf Me.rcurrconv.Checked = True Then
                oportfolio.convert_type = 2
                oportfolio.currency = Me.ccurr.Text
            End If

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                oportfolio.db_write()
            Else
                oportfolio.tmode = bc_cs_soap_base_class.tWRITE
                oportfolio.transmit_to_server_and_receive(oportfolio, True)
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_portfolio_main", "save_portfolio", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally

        End Try

    End Function
    Private Function build_portfolio() As Boolean
        REM first get all paramters constant across data set

        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try
            Dim otrace As New bc_cs_activity_log("bc_am_portfolio_wizard", "build_portfolio", bc_cs_activity_codes.TRACE_ENTRY, "")

            Me.Cursor = Cursors.WaitCursor
            build_portfolio = True

            Dim i, j, k, l As Integer
            Dim start_row As Integer = 1
            Dim start_col As Integer = 1
            Dim fh As New formulas_header
            Dim entity_formulas As New ArrayList
            Dim formulas As New ArrayList
            Dim item_col As Integer
            Dim multiple_entity As Boolean = True
            REM check workbook is opne
            Try
                i = excelapp.activeworkbook.worksheets.count
            Catch
                Dim omsg As New bc_cs_message("Blue Curve", "Please open a workbook  in Excel and run portfolio again", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Me.Hide()
                Exit Function
            End Try
            REM if only one entity selected and more than one item display data vertically
            If Me.lselentity.Items.Count = 1 Or Me.cfunction.SelectedIndex = 1 Then
                multiple_entity = False
            End If


            If Me.rcell.Checked = True Then
                start_row = excelapp.activecell.row
                start_col = excelapp.activecell.column
            ElseIf Me.rsheet.Checked = True Then
                excelapp.activeworkbook.worksheets.add()
                start_row = 1
                start_col = 1
            End If
            excelapp.visible = True
            excelapp.statusbar = "Generating Extract..."
            Dim period As String = False
            fh.stage = Me.cstage.Text

            fh.contributor = Me.Ccont.Text

            fh.row_offset = start_row + 1
            fh.item_row_offset = start_row + 4
            fh.title = Me.tname.Text

            If Me.rnow.Checked = True Then
                fh.date_at = "9-9-9999"
            Else
                fh.date_at = Me.DateTimePicker1.Value
            End If
            fh.entity_class = Me.cclass.Text
            If Me.rclass.Checked = True Then
                If InStr(Me.rclass.Text, "used") = 0 Then
                    fh.prop = "in " + Me.cclass2.Text + " " + Me.centity.Text
                Else
                    fh.prop = "used by " + Me.cclass2.Text + " " + Me.centity.Text
                End If
            End If

            If Me.ccurr.SelectedIndex > -1 And Me.rcurrconv.Checked = True Then
                fh.currency_code = Me.ccurr.Text
            End If
            REM now get entity list
            REM if portfolio as child entities as well deduce what these are
            deduce_child_entities()
            Dim oent_formula As formula_entity
            If Me.clink.SelectedIndex > 0 Then
                Me.get_propogating_entities()
                If Me.propentitymaster.Count > 1 Then
                    multiple_entity = True
                End If

                Me.lselentity.Items.Clear()
                For i = 0 To Me.propentitymaster.Count - 1
                    Me.lselentity.Items.Add(Me.propentitymaster(i))
                Next

            End If
            For i = 0 To Me.lselentity.Items.Count - 1
                Dim ocomm As New bc_cs_activity_log("bc_am_portfolio_wizard", "build_portfolio", bc_cs_activity_codes.COMMENTARY, "Creating formula for entity: " + CStr(i + 1) + " of " + CStr(Me.lselentity.Items.Count))
                oent_formula = New formula_entity
                oent_formula.row_offset = start_row + 11 + i
                oent_formula.col_offset = start_col
                oent_formula.header = fh
                oent_formula.name = Me.lselentity.Items(i)

                If Me.clink.SelectedIndex > 0 Then
                    oent_formula.associated_name = Me.propentityslave(i)
                End If
                entity_formulas.Add(oent_formula)
                REM now build cells that hold formula
                REM item
                If multiple_entity = True Then
                    item_col = oent_formula.col_offset + 2
                Else
                    item_col = oent_formula.col_offset + 3
                End If

                For j = 0 To Me.lselitem.Items.Count - 1
                    REM if item is period related
                    If Me.item_sel_type(j) = "period" Then
                        If multiple_entity = False Then
                            item_col = start_col + 7
                        End If
                        Dim oform As formula = Nothing

                        If item_col > 255 Then
                            Me.WindowState = FormWindowState.Minimized
                            Dim omsg As New bc_cs_message("Blue Curve", "Excel column limit exeeded (255), please go back and choose less items years or periods", bc_cs_message.MESSAGE)
                            Me.WindowState = FormWindowState.Normal
                            Me.twizard.SelectedIndex = 3
                            Me.tprogress.Abort()
                            Me.ppropress.Visible = False
                            build_portfolio = False
                            Exit Function
                        End If

                        For k = CInt(Me.Cyearstart.Text) To CInt(Me.cyearend.Text)
                            For l = 0 To Me.lperiods.Items.Count - 1
                                oform = New formula
                                oform.formula_entity = oent_formula
                                oform.item_type = "item"
                                If k > CInt(Me.Cyearstart.Text) Then
                                    oform.first = False
                                End If
                                If l > 0 Then
                                    oform.first = False
                                End If
                                oform.item = Me.lselitem.Items(j)
                                oform.item_factor = Me.item_sel_factor(j)
                                oform.item_monatary = Me.item_sel_monatary(j)
                                oform.item_symbol = Me.item_sel_symbol(j)
                                oform.year = k
                                oform.assoc_entity = False
                                If Me.item_sel_assoc_class(j) = Me.clink.Text Then
                                    oform.assoc_entity = True
                                End If


                                oform.period = Me.lperiods.Items(l)
                                oform.row_offset = oent_formula.row_offset
                                oform.col_offset = item_col
                                If multiple_entity = True Then
                                    If Me.rcurrconv.Checked = True Then
                                        currency_convert(oform)
                                    Else
                                        build_function_multiple(oform)
                                    End If
                                Else
                                    oform.row_offset = start_row + 9 + j
                                    oform.col_offset = item_col
                                    If Me.rcurrconv.Checked = True Then
                                        currency_convert(oform, True, False)
                                    Else
                                        build_function_single(oform)
                                    End If
                                End If
                                oform.formula = oform.formula.Replace(",)", ")")
                                If Me.cformat.Checked = True Then

                                    REM wrap function aound format
                                    oform.formula = oform.formula.Substring(1, oform.formula.Length - 1)
                                    If multiple_entity = True Then
                                        oform.formula = "=bc_format(" + oform.formula + "," + excel_address(start_row + 2, oform.col_offset, True, False) + "," + excel_address(start_row + 1, oform.col_offset, True, False) + "," + excel_address(start_row + 3, oform.col_offset, True, False) + "," + excel_address(start_row + 9, oform.col_offset, True, False) + ")"
                                    Else
                                        oform.formula = "=bc_format(" + oform.formula + "," + excel_address(oform.row_offset, start_col + 1, False, True) + "," + excel_address(oform.row_offset, start_col, False, True) + "," + excel_address(oform.row_offset, start_col + 2, False, True) + "," + excel_address(oform.row_offset, start_col + 3, False, True) + ")"
                                    End If
                                End If
                                formulas.Add(oform)
                                If Me.cformat.Checked = True Then
                                    REM add scale symbol formula
                                    If multiple_entity = True Then
                                        Dim oform1 As New formula
                                        oform1.formula_entity = oent_formula
                                        oform1.item = ""
                                        oform1.item_type = "symbol"
                                        oform1.row_offset = start_row + 10
                                        oform1.col_offset = item_col
                                        oform1.formula = "=bc_get_scale_symbol(" + excel_address(start_row + 9, item_col) + "," + excel_address(start_row + 3, item_col)
                                        formulas.Add(oform1)
                                    End If
                                End If
                                If Me.c_show_ea.Checked = True And multiple_entity = False Then
                                    Dim oform1 As New formula
                                    oform1.formula_entity = oent_formula
                                    oform1.item_type = "e_a"
                                    oform1.item = "E/A"
                                    oform1.assoc_entity = False
                                    If Me.item_sel_assoc_class(j) = Me.clink.Text Then
                                        oform1.assoc_entity = True
                                    End If

                                    oform1.year = ""
                                    oform1.period = ""
                                    oform1.row_offset = start_row + 8
                                    oform1.formula_entity.row_offset = start_row + 9 + j
                                    oform1.col_offset = item_col
                                    build_function_single(oform1, True, False, False, True)
                                    oform1.formula = oform1.formula.Replace(",)", ",""e_a_flag"")")
                                    oform1.formula = oform1.formula.Replace("=bc", "=IF(bc")
                                    oform1.formula = oform1.formula.Replace(")", ")=0,""A"",""E"")")
                                    formulas.Add(oform1)

                                End If
                                item_col = item_col + 1

                                If Me.c_show_ea.Checked = True And multiple_entity = True Then

                                    REM add e_a-after item
                                    Dim oform1 As New formula
                                    oform1.formula_entity = oent_formula
                                    oform1.item_type = "e_a"
                                    oform1.item = "E/A"
                                    oform1.assoc_entity = False
                                    If Me.item_sel_assoc_class(j) = Me.clink.Text Then
                                        oform1.assoc_entity = True
                                    End If

                                    oform1.year = ""
                                    oform1.period = ""
                                    oform1.row_offset = oent_formula.row_offset
                                    oform1.col_offset = item_col
                                    item_col = item_col + 1
                                    build_function_multiple(oform1, True, False, False, True)
                                    oform1.formula = oform1.formula.Replace(",)", ",""e_a_flag"")")
                                    oform1.formula = oform1.formula.Replace("=bc", "=IF(bc")
                                    oform1.formula = oform1.formula.Replace(")", ")=""0"",""A"",""E"")")
                                    formulas.Add(oform1)
                                End If



                            Next
                        Next
                        If Me.rcurrnone.Checked = True And oform.item_monatary = True Then
                            REM add currency after item
                            oform = New formula
                            oform.formula_entity = oent_formula
                            oform.item = "Currency"
                            oform.item_type = "curr"
                            oform.assoc_entity = False
                            If Me.item_sel_assoc_class(j) = Me.clink.Text Then
                                oform.assoc_entity = True
                            End If

                            If multiple_entity = True Then
                                oform.row_offset = oent_formula.row_offset
                                oform.col_offset = item_col
                                build_function_multiple(oform, True, True, True, Me.c_show_ea.Checked)
                            Else
                                oform.formula_entity.header.col_offset = 3 + start_col
                                oform.row_offset = start_row + 9 + j
                                oform.col_offset = start_col + 5
                                build_function_single(oform, True, True, True, Me.c_show_ea.Checked)
                            End If

                            oform.formula = oform.formula.Replace(")", """item_currency"")")
                            item_col = item_col + 1
                            formulas.Add(oform)
                        End If
                        REM add scale symbol formula
                        If multiple_entity = False And Me.cformat.Checked = True Then
                            Dim oform1 As New formula
                            oform1.formula_entity = oent_formula
                            oform1.item = ""
                            oform1.item_type = "symbol"
                            oform1.row_offset = start_row + 9 + j
                            oform1.col_offset = start_col + 4
                            oform1.formula = "=bc_get_scale_symbol(" + excel_address(start_row + 9 + j, start_col + 3) + "," + excel_address(start_row + 9 + j, start_col + 2)
                            formulas.Add(oform1)
                        End If
                    Else
                        Dim oform As New formula
                        oform.formula_entity = oent_formula
                        oform.item = Me.lselitem.Items(j)
                        oform.item_type = "item"
                        oform.item_factor = Me.item_sel_factor(j)
                        oform.item_monatary = Me.item_sel_monatary(j)
                        oform.item_symbol = Me.item_sel_symbol(j)
                        oform.row_offset = oent_formula.row_offset
                        oform.col_offset = item_col
                        oform.assoc_entity = False
                        If Me.item_sel_assoc_class(j) = Me.clink.Text Then
                            oform.assoc_entity = True
                        End If


                        If multiple_entity = True Then
                            If Me.rcurrconv.Checked = True Then
                                currency_convert(oform)
                            Else
                                build_function_multiple(oform)
                            End If
                        Else
                            oform.row_offset = start_row + 9 + j
                            oform.col_offset = start_col + 6
                            If Me.rcurrconv.Checked = True Then
                                currency_convert(oform, True, False)
                            Else
                                build_function_single(oform)
                            End If
                        End If
                        oform.formula = oform.formula.Replace(",)", ")")
                        If Me.cformat.Checked = True Then
                            REM wrap function aound format
                            oform.formula = oform.formula.Substring(1, oform.formula.Length - 1)
                            If multiple_entity = True Then
                                oform.formula = "=bc_format(" + oform.formula + "," + excel_address(start_row + 2, oform.col_offset, True, False) + "," + excel_address(start_row + 1, oform.col_offset, True, False) + "," + excel_address(start_row + 3, oform.col_offset, True, False) + "," + excel_address(start_row + 9, oform.col_offset, True, False) + ")"
                            Else
                                oform.formula = "=bc_format(" + oform.formula + "," + excel_address(oform.row_offset, start_col + 1, False, True) + "," + excel_address(oform.row_offset, start_col, False, True) + "," + excel_address(oform.row_offset, start_col + 2, False, True) + "," + excel_address(oform.row_offset, start_col + 9, False, True) + ")"
                            End If
                        End If
                        formulas.Add(oform)
                        If Me.cformat.Checked = True And multiple_entity = True Then
                            REM add scale symbol formula
                            Dim oform1 As New formula
                            oform1.formula_entity = oent_formula
                            oform1.item = ""
                            oform1.item_type = "symbol"
                            oform1.row_offset = start_row + 10
                            oform1.col_offset = item_col
                            oform1.formula = "=bc_get_scale_symbol(" + excel_address(start_row + 9, item_col) + "," + excel_address(start_row + 3, item_col)
                            formulas.Add(oform1)
                        End If
                        item_col = item_col + 1

                        If Me.rcurrnone.Checked = True And oform.item_monatary = True Then
                            REM add currency after item
                            oform = New formula
                            oform.formula_entity = oent_formula
                            oform.item = "Currency"
                            oform.item_type = "curr"
                            oform.assoc_entity = False
                            If Me.item_sel_assoc_class(j) = Me.clink.Text Then
                                oform.assoc_entity = True
                            End If

                            If multiple_entity = True Then
                                oform.row_offset = oent_formula.row_offset
                                oform.col_offset = item_col
                                build_function_multiple(oform, True, True, True)
                            Else
                                oform.formula_entity.header.col_offset = 2 + start_col
                                oform.row_offset = start_row + 9 + j
                                oform.col_offset = start_col + 5
                                build_function_single(oform, True, True, True, Me.c_show_ea.Checked)
                            End If
                            oform.formula = oform.formula.Replace(")", """item_currency"")")
                            item_col = item_col + 1
                            formulas.Add(oform)
                        End If
                        If Me.cformat.Checked = True Then
                            REM add scale symbol formula
                            If multiple_entity = False Then
                                Dim oform1 As New formula
                                oform1.formula_entity = oent_formula
                                oform1.item = ""
                                oform1.item_type = "symbol"
                                oform1.row_offset = start_row + 9 + j
                                oform1.col_offset = start_col + 4
                                oform1.formula = "=bc_get_scale_symbol(" + excel_address(start_row + 9 + j, start_col + 3) + "," + excel_address(start_row + 9 + j, start_col + 2)
                                formulas.Add(oform1)
                            End If
                        End If
                    End If
                Next
            Next

            build_portfolio = output_to_excel(start_row, start_col, fh, entity_formulas, formulas, multiple_entity)



        Catch ex As Exception
            build_portfolio = False
            Dim oerr As New bc_cs_error_log("bc_am_portfolio_wizard", "build_portfolio", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            Dim otrace As New bc_cs_activity_log("bc_am_portfolio_wizard", "build_portfolio", bc_cs_activity_codes.TRACE_EXIT, "")

            Me.Cursor = Cursors.Default
        End Try

    End Function
    Private Sub deduce_child_entities()
        If Me.clink.SelectedIndex > -1 Then
            REM for each selected entity deduce child entity



        End If
    End Sub
    Private Function build_portfolio_historic() As Boolean
        REM first get all paramters constant across data set

        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try
            Me.Cursor = Cursors.WaitCursor
            build_portfolio_historic = True

            Dim i, j, k, l As Integer
            Dim start_row As Integer = 1
            Dim start_col As Integer = 1
            Dim fh As New formulas_header
            Dim entity_formulas As New ArrayList
            Dim formulas As New ArrayList
            Dim item_col As Integer
            Dim multiple_entity As Boolean = True

            REM check workbook is opne
            Try
                i = excelapp.activeworkbook.worksheets.count
            Catch
                Dim omsg As New bc_cs_message("Blue Curve", "Please open a workbook in Excel and run portfolio again", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Function
            End Try
            REM if only one entity selected and more than one item display data vertically
            If Me.lselentity.Items.Count = 1 Or Me.cfunction.SelectedIndex = 1 Then
                multiple_entity = False
            End If

            If Me.rcell.Checked = True Then
                start_row = excelapp.activecell.row
                start_col = excelapp.activecell.column
            ElseIf Me.rsheet.Checked = True Then
                excelapp.activeworkbook.worksheets.add()
                start_row = 1
                start_col = 1
            End If
            Dim trows As Integer = 0
            REM evaluate how many items
            trows = evaluate_num_time_series()

            excelapp.visible = True
            excelapp.statusbar = "Generating Portfolio..."
            Dim period As String = False
            fh.stage = Me.cstage.Text

            fh.contributor = Me.Ccont.Text

            fh.row_offset = start_row + 1
            fh.col_offset = start_col
            fh.item_row_offset = start_row + 4
            fh.title = Me.tname.Text

            If Me.rnow.Checked = True Then
                fh.date_at = "9-9-9999"
            Else
                fh.date_at = Me.DateTimePicker1.Value
            End If
            If Me.rold.Checked = True Then
                fh.date_from = "1-1-1900"
            Else
                fh.date_from = Me.DateTimePicker2.Value
            End If
            fh.entity_class = Me.cclass.Text

            If Me.ccurr.SelectedIndex > -1 And Me.rcurrconv.Checked = True Then
                fh.currency_code = Me.ccurr.Text
            End If
            REM now get entity list
            Dim oent_formula As formula_entity
            oent_formula = New formula_entity
            oent_formula.row_offset = start_row + 8 + i
            oent_formula.col_offset = start_col
            oent_formula.header = fh
            oent_formula.name = Me.lselentity.Items(0)
            entity_formulas.Add(oent_formula)
            REM build default time series formula
            Dim oform As New formula

            oform.formula_entity = oent_formula
            REM lead item

            oform.item = Me.lselitem.Items(0)

            oform.item_type = "item"
            oform.item_factor = Me.item_sel_factor(0)
            oform.row_offset = start_row + 9
            oform.col_offset = start_col
            oform.item = "Date"
            build_function_single(oform, True, False, False, False, True, True)
            oform.formula = oform.formula.Replace(",)", ")")
            oform.formula = oform.formula.Replace(")", ",""date_from"")")
            formulas.Add(oform)
            REM need to evalute how many rows returned
            Me.cfunction.SelectedIndex = 0
            REM now set up cells
            item_col = start_col + 3

            REM now do items
            Me.cfunction.SelectedIndex = 0

            For j = 0 To Me.lselitem.Items.Count - 1
                REM if item is period related
                If Me.item_sel_type(j) = "period" Then
                    For k = CInt(Me.Cyearstart.Text) To CInt(Me.cyearend.Text)
                        For l = 0 To Me.lperiods.Items.Count - 1
                            oform = New formula
                            oform.formula_entity = oent_formula
                            oform.item_type = "item"
                            If k > CInt(Me.Cyearstart.Text) Then
                                oform.first = False
                            End If
                            If l > 0 Then
                                oform.first = False
                            End If
                            oform.item = Me.lselitem.Items(j)
                            oform.item_factor = Me.item_sel_factor(j)
                            oform.year = k
                            oform.period = Me.lperiods.Items(l)
                            oform.row_offset = start_row + 8
                            oform.col_offset = item_col
                            item_col = item_col + 1
                            oform.row_offset = start_row + 9 + j
                            oform.col_offset = item_col
                            If Me.rcurrconv.Checked = True Then
                                currency_convert(oform, True, False)
                            Else
                                build_function_single(oform)
                            End If
                            oform.formula = oform.formula.Replace(",)", ")")
                            If Me.cformat.Checked = True Then
                                REM wrap function aound format
                                oform.formula = oform.formula.Substring(1, oform.formula.Length - 1)
                                If multiple_entity = True Then
                                    oform.formula = "=bc_format(" + oform.formula + "," + excel_address(start_row + 2, oform.col_offset) + "," + excel_address(start_row + 1, oform.col_offset) + "," + excel_address(start_row + 3, oform.col_offset) + ")"
                                Else
                                    oform.formula = "=bc_format(" + oform.formula + "," + excel_address(oform.row_offset, start_col + 1, False, True) + "," + excel_address(oform.row_offset, start_col, False, True) + "," + excel_address(oform.row_offset, start_col, False, True) + ")"

                                End If
                            End If
                            formulas.Add(oform)
                            If Me.c_show_ea.Checked = True And multiple_entity = True Then
                                REM add e_a-after item
                                oform = New formula
                                oform.formula_entity = oent_formula
                                oform.item_type = "e_a"
                                oform.item = "E/A"
                                oform.year = ""
                                oform.period = ""
                                oform.row_offset = oent_formula.row_offset
                                oform.col_offset = item_col
                                item_col = item_col + 1
                                build_function_multiple(oform, True, False, False, True)
                                oform.formula = oform.formula.Replace(",)", ",""e_a_flag"")")
                                oform.formula = oform.formula.Replace("=bc", "=IF(bc")
                                oform.formula = oform.formula.Replace(")", ")=""0"",""A"",""E"")")
                                formulas.Add(oform)
                            End If
                            If Me.c_show_ea.Checked = True And multiple_entity = False Then
                                oform = New formula
                                oform.formula_entity = oent_formula
                                oform.item_type = "e_a"
                                oform.item = "E/A"
                                oform.year = ""
                                oform.period = ""
                                oform.row_offset = start_row + 9
                                oform.formula_entity.row_offset = start_row + 9 + j
                                oform.col_offset = item_col
                                build_function_single(oform, True, False, False, True)
                                oform.formula = oform.formula.Replace(",)", ",""e_a_flag"")")
                                oform.formula = oform.formula.Replace("=bc", "=IF(bc")
                                oform.formula = oform.formula.Replace(")", ")=""0"",""A"",""E"")")
                                formulas.Add(oform)

                            End If
                        Next
                    Next
                    If Me.rcurrnone.Checked = True Then
                        REM add currency after item
                        oform = New formula
                        oform.formula_entity = oent_formula
                        oform.item = "Currency"
                        oform.item_type = "curr"
                        If multiple_entity = True Then
                            oform.row_offset = oent_formula.row_offset
                            oform.col_offset = item_col
                            build_function_multiple(oform, True, True, True, Me.c_show_ea.Checked)
                        Else
                            oform.formula_entity.header.col_offset = 3 + start_col
                            oform.row_offset = start_row + 9 + j
                            oform.col_offset = start_col + 1
                            build_function_single(oform, True, True, True, Me.c_show_ea.Checked)
                        End If

                        oform.formula = oform.formula.Replace(")", """item_currency"")")
                        item_col = item_col + 1
                        formulas.Add(oform)
                    End If
                Else
                    REM static values
                    REM do this for each time series result
                    For i = 0 To trows
                        oform = New formula
                        oform.formula_entity = oent_formula
                        oform.item = Me.lselitem.Items(j)
                        oform.item_type = "item"
                        oform.item_factor = Me.item_sel_factor(j)
                        oform.row_offset = start_row + 9 + i
                        oform.col_offset = item_col
                        If Me.rcurrconv.Checked = True Then
                            currency_convert(oform, True, False, True)
                        Else
                            build_function_single(oform, True, False, False, False, True, False)
                        End If
                        oform.formula = oform.formula.Replace(",)", ")")
                        If Me.cformat.Checked = True Then
                            REM wrap function aound format
                            oform.formula = oform.formula.Substring(1, oform.formula.Length - 1)
                            oform.formula = "=bc_format(" + oform.formula + "," + excel_address(oform.row_offset - 7 - i, oform.col_offset) + "," + excel_address(oform.row_offset - 8 - i, oform.col_offset) + "," + excel_address(oform.row_offset - 6 - i, oform.col_offset) + ")"
                        End If
                        'oform.formula = oform.formula.Replace(")", ",""date_from"")")
                        formulas.Add(oform)
                        If Me.rcurrnone.Checked = True And i = 0 Then
                            REM add currency after item
                            oform = New formula
                            oform.formula_entity = oent_formula
                            oform.item = "Currency"
                            oform.item_type = "curr"
                            oform.formula_entity.header.col_offset = 2 + start_col
                            oform.row_offset = start_row + 8 + i
                            oform.col_offset = item_col
                            build_function_single(oform, True, True, True, Me.c_show_ea.Checked, True, False)
                            oform.formula = oform.formula.Replace(")", """item_currency"")")
                            formulas.Add(oform)
                        End If
                    Next
                    item_col = item_col + 1
                End If
            Next

            Me.cfunction.SelectedIndex = 1
            build_portfolio_historic = output_to_excel(start_row, start_col, fh, entity_formulas, formulas, multiple_entity, True, trows)




            Me.Hide()

        Catch ex As Exception
            build_portfolio_historic = False
            Dim omsg As New bc_cs_message("Blue Curve", "Error building potfolio in Excel please try again", bc_cs_message.MESSAGE, False, False, "OK", "Cancel", True)

        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            Me.cfunction.SelectedIndex = 1
            Me.Cursor = Cursors.Default
        End Try

    End Function

    Private Function output_to_excel(ByVal start_row As Integer, ByVal start_col As Integer, ByVal fh As formulas_header, ByVal entity_formulas As ArrayList, ByVal formulas As ArrayList, Optional ByVal multiple_entity As Boolean = True, Optional ByVal time_series As Boolean = False, Optional ByVal tk_count As Integer = 0) As Boolean
        Dim calc_state As Integer
        Dim style_error As String = ""

        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try
            Dim format_filename As String = "bc_pt_default.xls"
            If bc_cs_central_settings.userOfficeStatus = 2 Then
                format_filename = "bc_pt_default.xlsx"
            End If
            Dim activesheet As String
            Dim i As Integer

            calc_state = excelapp.application.calculation
            excelapp.application.calculation = -4135

            output_to_excel = True
            Dim oexcel As New bc_ao_in_excel(excelapp)
            REM check if formats are already in excel workbook
            If check_formats_exists() = False Then
                If Me.check_format_file_exists(format_filename) = True Then
                    REM load it
                    activesheet = excelapp.application.activeworkbook.activesheet.name
                    oexcel.disable_application_alerts()
                    oexcel.insert_sheet("bc_pt_formats", bc_cs_central_settings.local_template_path + format_filename)
                    oexcel.enable_application_alerts()
                    excelapp.application.activeworkbook.activesheet.visible = False
                    REM swicth back activesheet
                    For i = 1 To excelapp.application.activeworkbook.worksheets.count
                        If excelapp.application.activeworkbook.worksheets(i).name = activesheet Then
                            excelapp.application.activeworkbook.worksheets(i).select()
                            Exit For
                        End If
                    Next
                Else
                    Dim omsg As New bc_cs_message("Blue Curve", "Format filename " + format_filename + "doesnt exist", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                End If
            End If



            Dim yr_str As String = ""
            Dim pe_str As String = ""
            Dim en_str As String = ""
            For i = Year(Now) - 15 To Year(Now) + 15
                If i = Year(Now) - 15 Then
                    yr_str = CStr(i)
                Else
                    yr_str = yr_str + "," + CStr(i)
                End If
            Next
            For i = 0 To bc_am_ef_functions.period_names.Count - 1
                If i = 0 Then
                    pe_str = bc_am_ef_functions.period_names(0)
                Else
                    pe_str = pe_str + "," + bc_am_ef_functions.period_names(i)
                End If
            Next
            If multiple_entity = False Then
                For i = 0 To bc_am_ef_functions.entities.Count - 1
                    If bc_am_ef_functions.entities(i).class_id = bc_am_ef_functions.class_ids(Me.cclass.SelectedIndex) Then
                        Me.lallentity.Items.Add(bc_am_ef_functions.entities(i).name)
                        If i = 0 Then
                            en_str = bc_am_ef_functions.entities(i).name
                        Else
                            en_str = en_str + "," + bc_am_ef_functions.entities(i).name
                        End If
                    End If
                Next
            End If

            REM set whole area into header colour
            Dim startcell As String
            Dim endcell As String
            Dim starttablecell As String
            Dim endtablecell As String = ""
            startcell = excel_address(start_row, start_col)
            If multiple_entity = True Then
                starttablecell = excel_address(start_row + 7, start_col)
            Else
                starttablecell = excel_address(start_row + 3, start_col + 3)
            End If
            Dim limit_exc As Boolean = False
            If multiple_entity = True Then
                If formulas(formulas.Count - 1).col_offset > 255 Then
                    limit_exc = True
                End If
            ElseIf multiple_entity = False And time_series = False Then
                If Me.Cyearstart.Enabled = True Then
                    If start_col + 6 + (((CInt(cyearend.Text) - CInt(Cyearstart.Text) + 1) * Me.lperiods.Items.Count)) > 255 Then
                        limit_exc = True
                    End If
                End If
            Else
                If formulas(formulas.Count - 1).col_offset > 255 Then
                    limit_exc = True
                End If
            End If
            If limit_exc = True Then
                Me.WindowState = FormWindowState.Minimized
                Dim omsg As New bc_cs_message("Blue Curve", "Excel column limit exeeded (255), please go back and choose less items years or periods", bc_cs_message.MESSAGE)
                Me.WindowState = FormWindowState.Normal
                Me.twizard.SelectedIndex = 3
                Me.tprogress.Abort()
                Me.ppropress.Visible = False
                output_to_excel = False
                Exit Function
            End If


            If multiple_entity = True Then

                endcell = excel_address(start_row + 6, formulas(formulas.Count - 1).col_offset)
                If Me.tsource.Text = "" Then
                    endtablecell = excel_address(start_row + 10 + Me.lselentity.Items.Count, formulas(formulas.Count - 1).col_offset)
                Else
                    endtablecell = excel_address(start_row + 11 + Me.lselentity.Items.Count, formulas(formulas.Count - 1).col_offset)
                End If
            ElseIf multiple_entity = False And time_series = False Then
                If Me.Cyearstart.Enabled = True Then
                    endcell = excel_address(start_row + 8 + Me.lselitem.Items.Count + 1, start_col + 6 + (((CInt(cyearend.Text) - CInt(Cyearstart.Text) + 1) * Me.lperiods.Items.Count)))
                    endtablecell = excel_address(start_row + 8 + Me.lselitem.Items.Count + 1, start_col + 6 + (((CInt(cyearend.Text) - CInt(Cyearstart.Text) + 1) * Me.lperiods.Items.Count)))
                Else
                    endcell = excel_address(start_row + 8 + Me.lselitem.Items.Count + 1, start_col + 6)
                    endtablecell = excel_address(start_row + 8 + Me.lselitem.Items.Count + 1, start_col + 6)
                End If
            Else
                endcell = excel_address(start_row + 6 + tk_count + 3, formulas(formulas.Count - 1).col_offset)
            End If
            REM header background
            Try
                excelapp.Application.ActiveSheet.Range(startcell + ":" + endcell).style = "bc_pt_bg"
            Catch
                style_error = " bc_pt_bg"
            End Try
            REM table background
            Try
                excelapp.Application.ActiveSheet.Range(starttablecell + ":" + endtablecell).style = "bc_pt_table_bg"
            Catch
                style_error = " bc_pt_table_bg"
            End Try

            excelapp.application.activeworkbook.activesheet.cells(start_row, start_col) = "Blue Curve Extract"
            excelapp.application.activeworkbook.activesheet.cells(start_row, start_col).font.bold = True
            excelapp.application.activeworkbook.activesheet.cells(start_row, start_col + 1) = fh.title

            start_row = start_row + 1


            REM write out static headers
            REM workflow stage class
            If time_series = True Then
                excelapp.application.activeworkbook.activesheet.cells(start_row + 7, start_col) = "Date"
                excelapp.application.activeworkbook.activesheet.cells(start_row + 7, start_col).font.bold = True

                If Me.rcurrnone.Checked = True Then
                    excelapp.application.activeworkbook.activesheet.cells(start_row + 7, start_col + 2) = "Currency"
                    excelapp.application.activeworkbook.activesheet.cells(start_row + 7, start_col + 2).font.bold = True

                End If
            End If

            REM workflow stage
            excelapp.application.activeworkbook.activesheet.cells(start_row, start_col) = "Stage"
            excelapp.application.activeworkbook.activesheet.cells(start_row, start_col).HorizontalAlignment = -4152


            excelapp.application.activeworkbook.activesheet.cells(start_row, start_col).font.bold = True

            Dim ltx As String = ""
            For i = 0 To bc_am_ef_functions.stage_names.Count - 1
                If i = 0 Then
                    ltx = bc_am_ef_functions.stage_names(i)
                Else
                    ltx = ltx + "," + bc_am_ef_functions.stage_names(i)
                End If

            Next
            If bc_am_ef_functions.ousersettings.stages <> 0 Then
                Me.cstage.Enabled = False
                If bc_am_ef_functions.ousersettings.stages = 1 Then
                    ltx = "Draft"
                Else
                    ltx = "Publish"
                End If
            End If

            With excelapp.application.activeworkbook.activesheet.cells(start_row, start_col + 1).Validation()
                .Delete()
                .Add(Type:=3, AlertStyle:=1, Operator:= _
                1, Formula1:=ltx)
                .IgnoreBlank = True
                .InCellDropdown = True
                .InputTitle = ""
                .ErrorTitle = "Input Error"
                .InputMessage = ""
                .ErrorMessage = "Invalid Workflow Stage"
                .ShowInput = True
                .ShowError = True
            End With
            excelapp.application.activeworkbook.activesheet.cells(start_row, start_col + 1) = fh.stage
            Try
                excelapp.application.activeworkbook.activesheet.cells(start_row, start_col + 1).style = "bc_pt_filter"
            Catch
                style_error = style_error + "bc_pt_filter"
            End Try


            REM class
            If multiple_entity = True Then
                excelapp.application.activeworkbook.activesheet.cells(fh.item_row_offset + 1, start_col) = fh.entity_class
                excelapp.application.activeworkbook.activesheet.cells(fh.item_row_offset + 1, start_col).font.bold = True

                excelapp.application.activeworkbook.activesheet.cells(fh.item_row_offset + 2, start_col) = fh.prop
                excelapp.application.activeworkbook.activesheet.cells(fh.item_row_offset + 2, start_col).font.bold = True

                If Me.clink.SelectedIndex > 0 Then
                    excelapp.application.activeworkbook.activesheet.cells(fh.item_row_offset + 1, start_col + 1) = Me.clink.Text
                    excelapp.application.activeworkbook.activesheet.cells(fh.item_row_offset + 1, start_col + 1).font.bold = True
                End If

                REM title sub title
                If Me.ttitle.Text <> "" Then
                    excelapp.application.activeworkbook.activesheet.cells(fh.item_row_offset + 3, start_col) = Me.ttitle.Text
                    Try
                        excelapp.application.activeworkbook.activesheet.cells(fh.item_row_offset + 3, start_col).style = "bc_pt_title"
                    Catch
                        style_error = style_error + " bc_pt_title"
                    End Try
                End If
                If Me.tsubtitle.Text <> "" Then
                    excelapp.application.activeworkbook.activesheet.cells(fh.item_row_offset + 4, start_col) = Me.tsubtitle.Text
                    Try
                        excelapp.application.activeworkbook.activesheet.cells(fh.item_row_offset + 4, start_col).style = "bc_pt_subtitle"
                    Catch
                        style_error = style_error + " bc_pt_subtitle"
                    End Try
                End If
                If Me.tsource.Text <> "" Then
                    excelapp.application.activeworkbook.activesheet.cells(start_row + 11 + Me.lselentity.Items.Count - 1, start_col) = Me.tsource.Text
                    Try
                        excelapp.application.activeworkbook.activesheet.cells(start_row + 11 + Me.lselentity.Items.Count - 1, start_col).style = "bc_pt_source"
                    Catch
                        style_error = style_error + " bc_pt_title"
                    End Try
                End If

            Else
                If Me.ttitle.Text <> "" Then
                    excelapp.application.activeworkbook.activesheet.cells(start_row + 2, start_col + 3) = Me.ttitle.Text
                    Try
                        excelapp.application.activeworkbook.activesheet.cells(start_row + 2, start_col + 3).style = "bc_pt_title"
                    Catch
                        style_error = style_error + " bc_pt_title"
                    End Try
                End If
                If Me.tsubtitle.Text <> "" Then
                    excelapp.application.activeworkbook.activesheet.cells(start_row + 3, start_col + 3) = Me.tsubtitle.Text
                    Try
                        excelapp.application.activeworkbook.activesheet.cells(start_row + 3, start_col + 3).style = "bc_pt_subtitle"
                    Catch
                        style_error = style_error + " bc_pt_subtitle"
                    End Try
                End If
                If Me.tsource.Text <> "" Then
                    excelapp.application.activeworkbook.activesheet.cells(start_row + 9 + Me.lselitem.Items.Count - 1, start_col + 3) = Me.tsource.Text
                    Try
                        excelapp.application.activeworkbook.activesheet.cells(start_row + 9 + Me.lselitem.Items.Count - 1, start_col + 3).style = "bc_pt_source"
                    Catch
                        style_error = style_error + " bc_pt_title"
                    End Try
                End If
                excelapp.application.activeworkbook.activesheet.cells(start_row + 4, start_col) = fh.entity_class
                excelapp.application.activeworkbook.activesheet.cells(start_row + 4, start_col).font.bold = True
                excelapp.application.activeworkbook.activesheet.cells(start_row + 4, start_col).HorizontalAlignment = -4152
                If Me.clink.SelectedIndex > 0 Then
                    excelapp.application.activeworkbook.activesheet.cells(start_row + 5, start_col) = Me.clink.Text
                    excelapp.application.activeworkbook.activesheet.cells(start_row + 5, start_col).font.bold = True
                    excelapp.application.activeworkbook.activesheet.cells(start_row + 5, start_col).HorizontalAlignment = -4152
                    REM evaluate potentials
                    Dim j As Integer

                    Dim fm As String = ""
                    For i = 0 To bc_am_ef_functions.class_ids.Count - 1
                        For j = 0 To Me.use_class_ids.Count - 1
                            If Me.use_class_ids(j) = bc_am_ef_functions.class_ids(i) And bc_am_ef_functions.class_names(i) = Me.clink.Text Then
                                If Me.use_class_types(j) = "Parent" Then
                                    fm = "=bc_get_parent_associations(" + excel_address(start_row + 4, start_col) + "," + excel_address(start_row + 4, start_col + 1) + "," + excel_address(start_row + 5, start_col) + ",""" + Me.cschema.Text + """,""all"",""name"")"
                                Else
                                    fm = "=bc_get_child_associations(" + excel_address(start_row + 4, start_col) + "," + excel_address(start_row + 4, start_col + 1) + "," + excel_address(start_row + 5, start_col) + ",""" + Me.cschema.Text + """,""all"",""name"")"
                                End If
                                Exit For
                            End If
                        Next
                    Next
                    excelapp.application.activeworkbook.activesheet.cells(start_row + 5, start_col + 1).formula = fm
                End If
            End If
            REM contributor
            excelapp.application.activeworkbook.activesheet.cells(start_row + 1, start_col) = "Contributor"
            excelapp.application.activeworkbook.activesheet.cells(start_row + 1, start_col).HorizontalAlignment = -4152
            excelapp.application.activeworkbook.activesheet.cells(start_row + 1, start_col).font.bold = True
            ltx = ""
            For i = 0 To bc_am_ef_functions.contributor_names.Count - 1
                If i = 0 Then
                    ltx = bc_am_ef_functions.contributor_names(i)

                Else
                    ltx = ltx + "," + bc_am_ef_functions.contributor_names(i)
                End If

            Next

            With excelapp.application.activeworkbook.activesheet.cells(start_row + 1, start_col + 1).Validation()
                .Delete()
                .Add(Type:=3, AlertStyle:=1, Operator:= _
                1, Formula1:=ltx)
                .IgnoreBlank = True
                .InCellDropdown = True
                .InputTitle = ""
                .ErrorTitle = ""
                .InputMessage = ""
                .ErrorMessage = ""
                .ShowInput = True
                .ShowError = True
            End With
            Try
                excelapp.application.activeworkbook.activesheet.cells(start_row + 1, start_col + 1).style = "bc_pt_filter"
            Catch
                style_error = style_error + "bc_pt_filter"
            End Try
            excelapp.application.activeworkbook.activesheet.cells(start_row + 1, start_col + 1) = fh.contributor

            REM dateat
            Try
                With excelapp.application.activeworkbook.activesheet.cells(start_row + 2, start_col + 1).Validation()

                    .Delete()
                    .Add(Type:=4, AlertStyle:=1, Operator:= _
                    1, Formula1:="1/1/1900", Formula2:="9/9/9999")
                    .IgnoreBlank = True
                    .InCellDropdown = True
                    .InputTitle = ""
                    .ErrorTitle = "Input Error"
                    .InputMessage = ""
                    .ErrorMessage = "Value must be date"
                    .ShowInput = True
                    .ShowError = True
                End With
            Catch

            End Try
            If time_series = False Then
                excelapp.application.activeworkbook.activesheet.cells(start_row + 2, start_col) = "Date at"
            Else
                excelapp.application.activeworkbook.activesheet.cells(start_row + 2, start_col) = "Date Range"
            End If
            excelapp.application.activeworkbook.activesheet.cells(start_row + 2, start_col).HorizontalAlignment = -4152

            excelapp.application.activeworkbook.activesheet.cells(start_row + 2, start_col).font.bold = True
            If time_series = False Then
                excelapp.application.activeworkbook.activesheet.cells(start_row + 2, start_col + 1) = Format(fh.date_at, "dd-MMM-yyyy")
                Try
                    excelapp.application.activeworkbook.activesheet.cells(start_row + 2, start_col + 1).style = "bc_pt_filter"
                Catch
                    style_error = style_error + " bc_pt_filter"
                End Try
                Try
                    With excelapp.application.activeworkbook.activesheet.cells(start_row + 2, start_col + 1).Validation()

                        .Delete()
                        .Add(Type:=4, AlertStyle:=1, Operator:= _
                        1, Formula1:="1/1/1900", Formula2:="9/9/9999")
                        .IgnoreBlank = True
                        .InCellDropdown = True
                        .InputTitle = ""
                        .ErrorTitle = "Input Error"
                        .InputMessage = ""
                        .ErrorMessage = "Value must be date"
                        .ShowInput = True
                        .ShowError = True
                    End With
                Catch

                End Try
                excelapp.application.activeworkbook.activesheet.cells(start_row + 2, start_col + 1) = Format(fh.date_at, "dd-MMM-yyyy")
            Else
                'excelapp.application.activeworkbook.activesheet.cells(start_row + 2, start_col + 1) = Format(fh.date_from, "dd-MMM-yyyy")
                'Try
                '    excelapp.application.activeworkbook.activesheet.cells(start_row + 2, start_col + 1).style = "bc_pt_filter"
                'Catch
                '    style_error = style_error + " bc_pt_filter"
                'End Try
                'excelapp.application.activeworkbook.activesheet.cells(start_row + 1, start_col + 2) = Format(fh.date_at, "dd-MMM-yyyy")


                'Try
                '    excelapp.application.activeworkbook.activesheet.cells(start_row + 2, start_col + 2).style = "bc_pt_filter"
                'Catch
                '    style_error = style_error + " bc_pt_filter"
                'End Try
                'Try
                '    With excelapp.application.activeworkbook.activesheet.cells(start_row + 2, start_col + 2).Validation()

                '        .Delete()
                '        .Add(Type:=4, AlertStyle:=1, Operator:= _
                '        1, Formula1:="1/1/1900", Formula2:="9/9/9999")
                '        .IgnoreBlank = True
                '        .InCellDropdown = True
                '        .InputTitle = ""
                '        .ErrorTitle = "Input Error"
                '        .InputMessage = ""
                '        .ErrorMessage = "Value must be date"
                '        .ShowInput = True
                '        .ShowError = True
                '    End With

                excelapp.application.activeworkbook.activesheet.cells(start_row + 2, start_col + 1) = Format(fh.date_from, "dd-MMM-yyyy")
                Try
                    excelapp.application.activeworkbook.activesheet.cells(start_row + 2, start_col + 1).style = "bc_pt_filter"
                Catch
                    style_error = style_error + " bc_pt_filter"
                End Try
                excelapp.application.activeworkbook.activesheet.cells(start_row + 1, start_col + 2) = Format(fh.date_at, "dd-MMM-yyyy")


                Try
                    excelapp.application.activeworkbook.activesheet.cells(start_row + 2, start_col + 2).style = "bc_pt_filter"
                Catch
                    style_error = style_error + " bc_pt_filter"
                End Try
                Try
                    With excelapp.application.activeworkbook.activesheet.cells(start_row + 2, start_col + 2).Validation()

                        .Delete()
                        .Add(Type:=4, AlertStyle:=1, Operator:= _
                        1, Formula1:="1/1/1900", Formula2:="9/9/9999")
                        .IgnoreBlank = True
                        .InCellDropdown = True
                        .InputTitle = ""
                        .ErrorTitle = "Input Error"
                        .InputMessage = ""
                        .ErrorMessage = "Value must be date"
                        .ShowInput = True
                        .ShowError = True
                    End With

                Catch

                End Try
                excelapp.application.activeworkbook.activesheet.cells(start_row + 2, start_col + 2) = Format(fh.date_at, "dd-MMM-yyyy")

            End If

            If Me.rcurrconv.Checked = True Then
                excelapp.application.activeworkbook.activesheet.cells(start_row + 3, start_col) = "Currency"
                excelapp.application.activeworkbook.activesheet.cells(start_row + 3, start_col).HorizontalAlignment = -4152

                excelapp.application.activeworkbook.activesheet.cells(start_row + 3, start_col).font.bold = True
                For i = 0 To bc_am_ef_functions.currency_codes.Count - 1
                    If i = 0 Then
                        ltx = bc_am_ef_functions.currency_codes(i)
                    Else
                        ltx = ltx + "," + bc_am_ef_functions.currency_codes(i)
                    End If

                Next
                With excelapp.application.activeworkbook.activesheet.cells(start_row + 3, start_col + 1).Validation()
                    .Delete()
                    .Add(Type:=3, AlertStyle:=1, Operator:= _
                    1, Formula1:=ltx)
                    .IgnoreBlank = True
                    .InCellDropdown = True
                    .InputTitle = ""
                    .ErrorTitle = ""
                    .InputMessage = ""
                    .ErrorMessage = ""
                    .ShowInput = True
                    .ShowError = True
                End With
                excelapp.application.activeworkbook.activesheet.cells(start_row + 3, start_col + 1) = fh.currency_code
                Try
                    excelapp.application.activeworkbook.activesheet.cells(start_row + 3, start_col + 1).style = "bc_pt_filter"
                Catch
                    style_error = style_error + "bc_pt_filter"
                End Try

            End If

            REM table starts here
            REM write out entities

            If multiple_entity = True Then
                For i = 0 To entity_formulas.Count - 1
                    excelapp.application.activeworkbook.activesheet.cells(entity_formulas(i).row_offset, entity_formulas(i).col_offset) = entity_formulas(i).name

                    Try
                        excelapp.application.activeworkbook.activesheet.cells(entity_formulas(i).row_offset, entity_formulas(i).col_offset).style = "bc_pt_title_left"
                    Catch
                        style_error = style_error + "bc_pt_title_left"
                    End Try
                    If Me.clink.SelectedIndex > 0 Then

                        excelapp.application.activeworkbook.activesheet.cells(entity_formulas(i).row_offset, entity_formulas(i).col_offset + 1) = entity_formulas(i).associated_name
                        Try
                            excelapp.application.activeworkbook.activesheet.cells(entity_formulas(i).row_offset, entity_formulas(i).col_offset + 1).style = "bc_pt_title_left"
                        Catch
                            style_error = style_error + "bc_pt_title_left"
                        End Try
                    End If

                Next
            Else
                Try

                    With excelapp.application.activeworkbook.activesheet.cells(start_row + 4, start_col + 1).Validation()
                        .Delete()
                        .Add(Type:=3, AlertStyle:=1, Operator:= _
                        1, Formula1:=en_str)
                        .IgnoreBlank = True
                        .InCellDropdown = True
                        .InputTitle = ""
                        .ErrorTitle = ""
                        .InputMessage = ""
                        .ErrorMessage = ""
                        .ShowInput = True
                        .ShowError = True
                    End With
                Catch ex As Exception
                End Try
                excelapp.application.activeworkbook.activesheet.cells(start_row + 4, start_col + 1) = entity_formulas(0).name

                Try
                    excelapp.application.activeworkbook.activesheet.cells(start_row + 4, start_col + 1).style = "bc_pt_filter"
                Catch
                    style_error = style_error + "bc_pt_filter"
                End Try
                excelapp.application.activeworkbook.activesheet.cells(start_row + 5, start_col + 6) = "Static"
                Try
                    excelapp.application.activeworkbook.activesheet.cells(start_row + 5, start_col + 6).style = "bc_pt_title_top"
                Catch
                    style_error = style_error + "bc_pt_title_top"
                End Try
                If Me.rcurrnone.Checked = True Then
                    excelapp.application.activeworkbook.activesheet.cells(start_row + 5, start_col + 5) = "Currency"
                    Try
                        excelapp.application.activeworkbook.activesheet.cells(start_row + 5, start_col + 5).style = "bc_pt_title_top"
                    Catch
                        style_error = style_error + "bc_pt_title_top"
                    End Try
                End If
                If Me.cformat.Checked = True Then
                    If time_series = False Then
                        excelapp.application.activeworkbook.activesheet.cells(start_row + 5, start_col + 4) = "Symbol"
                        Try
                            excelapp.application.activeworkbook.activesheet.cells(start_row + 5, start_col + 4).style = "bc_pt_title_top"
                        Catch
                            style_error = style_error + "bc_pt_title_top"
                        End Try
                        excelapp.application.activeworkbook.activesheet.cells(start_row + 7, start_col + 0) = "Precision"
                        excelapp.application.activeworkbook.activesheet.cells(start_row + 7, start_col + 1) = "Type"
                        excelapp.application.activeworkbook.activesheet.cells(start_row + 7, start_col + 2) = "Factor"
                    End If
                End If
            End If
            excelapp.Calculation = -4135

            Dim max_col As Integer
            If multiple_entity = True Or time_series = True Then
                For i = 0 To formulas.Count - 1
                    If time_series = False Or (time_series = True And i > 0) Then
                        If (time_series = False Or formulas(i).item_type = "item") And formulas(i).item_type <> "symbol" Then
                            excelapp.application.activeworkbook.activesheet.cells(formulas(i).formula_entity.header.item_row_offset + 5, formulas(i).col_offset) = formulas(i).item
                            Try
                                excelapp.application.activeworkbook.activesheet.cells(formulas(i).formula_entity.header.item_row_offset + 5, formulas(i).col_offset).style = "bc_pt_title_top"
                            Catch
                                style_error = style_error + "bc_pt_title_top"
                            End Try
                        End If
                    End If
                    REM if precsion requested and value then set up
                    If Me.cformat.Checked = True Then
                        If formulas(i).item_type = "item" Then
                            If time_series = False Or (time_series = True And i > 0) Then

                                If Me.cprectype.SelectedIndex = 0 Then
                                    excelapp.application.activeworkbook.activesheet.cells(start_row + 1, formulas(i).col_offset) = "dps"
                                Else
                                    excelapp.application.activeworkbook.activesheet.cells(start_row + 1, formulas(i).col_offset) = "sfs"
                                End If
                                With excelapp.application.activeworkbook.activesheet.cells(start_row + 1, formulas(i).col_offset).Validation()
                                    .Delete()
                                    .Add(Type:=3, AlertStyle:=1, Operator:= _
                                    1, Formula1:="dps,sfs")
                                    .IgnoreBlank = True
                                    .InCellDropdown = True
                                    .InputTitle = ""
                                    .ErrorTitle = "Error"
                                    .InputMessage = "Select decimal places or sig figs"
                                    .ErrorMessage = "Please select dps or sfs"
                                    .ShowInput = True
                                    .ShowError = True
                                End With
                                excelapp.application.activeworkbook.activesheet.cells(start_row - 1, formulas(i).col_offset) = "Precision"
                                excelapp.application.activeworkbook.activesheet.cells(start_row - 1, formulas(i).col_offset).HorizontalAlignment = -4152
                                excelapp.application.activeworkbook.activesheet.cells(start_row, formulas(i).col_offset) = Me.cprec.Text
                                With excelapp.application.activeworkbook.activesheet.cells(start_row, formulas(i).col_offset).Validation()
                                    .Delete()
                                    .Add(Type:=3, AlertStyle:=1, Operator:= _
                                    1, Formula1:="0,1,2,3,4,5,6,7,8,9,10")
                                    .IgnoreBlank = True
                                    .InCellDropdown = True
                                    .InputTitle = ""
                                    .ErrorTitle = ""
                                    .InputMessage = "Select number of decimal places or sig figs"
                                    .ErrorMessage = ""
                                    .ShowInput = True
                                    .ShowError = True
                                End With
                                Try
                                    excelapp.application.activeworkbook.activesheet.cells(start_row, formulas(i).col_offset).style = "bc_pt_filter"
                                Catch
                                    style_error = style_error + " bc_pt_filter"
                                End Try
                                Try
                                    excelapp.application.activeworkbook.activesheet.cells(start_row + 1, formulas(i).col_offset).style = "bc_pt_filter"
                                Catch
                                    style_error = style_error + " bc_pt_filter"
                                End Try
                                With excelapp.application.activeworkbook.activesheet.cells(start_row + 2, formulas(i).col_offset).Validation()
                                    .Delete()
                                    .Add(Type:=3, AlertStyle:=1, Operator:= _
                                    1, Formula1:="1,10,100,1000,1000000,0.01")
                                    .IgnoreBlank = True
                                    .InCellDropdown = True
                                    .InputTitle = ""
                                    .ErrorTitle = ""
                                    .InputMessage = "Select scale factor"
                                    .ErrorMessage = ""
                                    .ShowInput = True
                                    .ShowError = True
                                End With
                                excelapp.application.activeworkbook.activesheet.cells(start_row + 2, formulas(i).col_offset) = formulas(i).item_factor
                                Try
                                    excelapp.application.activeworkbook.activesheet.cells(start_row + 2, formulas(i).col_offset).style = "bc_pt_filter"
                                Catch
                                    style_error = style_error + "bc_pt_filter"
                                End Try

                            End If
                        End If
                    End If



                    If formulas(i).year <> "" Then
                        REM year list
                        With excelapp.application.activeworkbook.activesheet.cells(formulas(i).formula_entity.header.item_row_offset + 3, formulas(i).col_offset).Validation()
                            .Delete()
                            .Add(Type:=3, AlertStyle:=1, Operator:= _
                            1, Formula1:=yr_str)
                            .IgnoreBlank = True
                            .InCellDropdown = True
                            .InputTitle = ""
                            .ErrorTitle = ""
                            .InputMessage = ""
                            .ErrorMessage = ""
                            .ShowInput = True
                            .ShowError = True
                        End With

                        excelapp.application.activeworkbook.activesheet.cells(formulas(i).formula_entity.header.item_row_offset + 3, formulas(i).col_offset) = formulas(i).year
                        Try
                            excelapp.application.activeworkbook.activesheet.cells(formulas(i).formula_entity.header.item_row_offset + 3, formulas(i).col_offset).style = "bc_pt_year"
                        Catch
                            style_error = style_error + "bc_pt_year"
                        End Try
                        If formulas(i).period <> "" Then
                            REM period list
                            With excelapp.application.activeworkbook.activesheet.cells(formulas(i).formula_entity.header.item_row_offset + 4, formulas(i).col_offset).Validation()
                                .Delete()
                                .Add(Type:=3, AlertStyle:=1, Operator:= _
                                1, Formula1:=pe_str)
                                .IgnoreBlank = True
                                .InCellDropdown = True
                                .InputTitle = ""
                                .ErrorTitle = ""
                                .InputMessage = ""
                                .ErrorMessage = ""
                                .ShowInput = True
                                .ShowError = True
                            End With
                        End If
                        excelapp.application.activeworkbook.activesheet.cells(formulas(i).formula_entity.header.item_row_offset + 4, formulas(i).col_offset) = formulas(i).period
                        Try
                            excelapp.application.activeworkbook.activesheet.cells(formulas(i).formula_entity.header.item_row_offset + 4, formulas(i).col_offset).style = "bc_pt_period"
                        Catch
                            style_error = style_error + " " + "bc_pt_period"

                        End Try
                    End If
                    excelapp.application.activeworkbook.activesheet.cells(formulas(i).row_offset, formulas(i).col_offset).formula = formulas(i).formula
                    Try
                        If formulas(i).item_type = "item" Then
                            excelapp.application.activeworkbook.activesheet.cells(formulas(i).row_offset, formulas(i).col_offset).style = "bc_pt_data"
                            If formulas(i).item_symbol = "Date" Then
                                excelapp.application.activeworkbook.activesheet.cells(formulas(i).row_offset, formulas(i).col_offset).numberformat = "dd/mm/yyyy"
                            End If
                        ElseIf formulas(i).item_type = "curr" Then
                            excelapp.application.activeworkbook.activesheet.cells(formulas(i).row_offset, formulas(i).col_offset).style = "bc_pt_currency"
                        ElseIf formulas(i).item_type = "symbol" Then
                            excelapp.application.activeworkbook.activesheet.cells(formulas(i).row_offset, formulas(i).col_offset).style = "bc_pt_symbol"
                        Else
                            excelapp.application.activeworkbook.activesheet.cells(formulas(i).row_offset, formulas(i).col_offset).style = "bc_pt_ea"
                        End If
                    Catch ex As Exception
                        style_error = style_error + "bc_pt_data or bc_pt_currency or bt_pt_ea"
                    End Try
                    'With excelapp.application.activeworkbook.activesheet.cells(formulas(i).row_offset, formulas(i).col_offset)
                    '    .style = "bc_pt_data"
                    'End With
                    max_col = formulas(i).col_offset

                Next
            Else
                Dim rc As Integer
                rc = 0
                rc = rc + 3
                Dim j As Integer
                REM write out year and periods
                If Me.Cyearstart.Enabled = True Then
                    rc = rc + 1
                    For i = CInt(Me.Cyearstart.Text) To CInt(Me.cyearend.Text)
                        For j = 0 To Me.lperiods.Items.Count - 1
                            With excelapp.application.activeworkbook.activesheet.cells(start_row + 5, start_col + 3 + rc).Validation()
                                .Delete()
                                .Add(Type:=3, AlertStyle:=1, Operator:= _
                                1, Formula1:=yr_str)
                                .IgnoreBlank = True
                                .InCellDropdown = True
                                .InputTitle = ""
                                .ErrorTitle = ""
                                .InputMessage = ""
                                .ErrorMessage = ""
                                .ShowInput = True
                                .ShowError = True
                            End With
                            With excelapp.application.activeworkbook.activesheet.cells(start_row + 6, start_col + 3 + rc).Validation()
                                .Delete()
                                .Add(Type:=3, AlertStyle:=1, Operator:= _
                                1, Formula1:=pe_str)
                                .IgnoreBlank = True
                                .InCellDropdown = True
                                .InputTitle = ""
                                .ErrorTitle = ""
                                .InputMessage = ""
                                .ErrorMessage = ""
                                .ShowInput = True
                                .ShowError = True
                            End With

                            excelapp.application.activeworkbook.activesheet.cells(start_row + 5, start_col + 3 + rc) = i
                            excelapp.application.activeworkbook.activesheet.cells(start_row + 6, start_col + 3 + rc) = Me.lperiods.Items(j)
                            Try
                                excelapp.application.activeworkbook.activesheet.cells(start_row + 5, start_col + 3 + rc).style = "bc_pt_year"
                            Catch
                                style_error = style_error + " bc_pt_year"
                            End Try
                            Try
                                excelapp.application.activeworkbook.activesheet.cells(start_row + 6, start_col + 3 + rc).style = "bc_pt_period"
                            Catch
                                style_error = style_error + " bc_pt_period"
                            End Try

                            rc = rc + 1
                        Next
                    Next
                End If
                rc = 0
                For i = 0 To formulas.Count - 1
                    If formulas(i).item_type = "item" And formulas(i).first = True Then
                        If time_series = True And i = 0 Then
                            excelapp.application.activeworkbook.activesheet.cells(start_row + 8, start_col) = "Date"
                            excelapp.application.activeworkbook.activesheet.cells(start_row + 8, start_col + 2) = formulas(i).item
                            excelapp.application.activeworkbook.activesheet.cells(start_row + 8, start_col).font.bold = True
                            excelapp.application.activeworkbook.activesheet.cells(start_row + 8, start_col + 2).font.bold = True
                        Else
                            excelapp.application.activeworkbook.activesheet.cells(start_row + 8 + rc, start_col + 3) = formulas(i).item
                            Try
                                excelapp.application.activeworkbook.activesheet.cells(start_row + 8 + rc, start_col + 3).style = "bc_pt_title_left"
                            Catch
                                style_error = style_error + " bc_pt_title_left"
                            End Try
                        End If

                        If Me.cformat.Checked = True Then

                            With excelapp.application.activeworkbook.activesheet.cells(start_row + 8 + rc, start_col + 1).Validation()
                                .Delete()
                                .Add(Type:=3, AlertStyle:=1, Operator:= _
                                1, Formula1:="dps,sfs")
                                .IgnoreBlank = True
                                .InCellDropdown = True
                                .InputTitle = ""
                                .ErrorTitle = "Error"
                                .InputMessage = ""
                                .ErrorMessage = "Please select dps or sfs"
                                .ShowInput = True
                                .ShowError = True
                            End With
                            If Me.cprectype.SelectedIndex = 0 Then
                                excelapp.application.activeworkbook.activesheet.cells(start_row + 8 + rc, start_col + 1) = "dps"
                            Else
                                excelapp.application.activeworkbook.activesheet.cells(start_row + 8 + rc, start_col + 1) = "sfs"
                            End If
                            excelapp.application.activeworkbook.activesheet.cells(start_row + 8 + rc, start_col) = Me.cprec.Text
                            Try
                                excelapp.application.activeworkbook.activesheet.cells(start_row + 8 + rc, start_col).style = "bc_pt_filter"
                            Catch
                                style_error = style_error + " bc_pt_filter"
                            End Try
                            With excelapp.application.activeworkbook.activesheet.cells(start_row + 8 + rc, start_col).Validation()
                                .Delete()
                                .Add(Type:=3, AlertStyle:=1, Operator:= _
                                1, Formula1:="0,1,2,3,4,5,6,7,8,9,10")
                                .IgnoreBlank = True
                                .InCellDropdown = True
                                .InputTitle = ""
                                .ErrorTitle = ""
                                .InputMessage = ""
                                .ErrorMessage = ""
                                .ShowInput = True
                                .ShowError = True
                            End With
                            Try
                                excelapp.application.activeworkbook.activesheet.cells(start_row + 8 + rc, start_col + 2).style = "bc_pt_filter"
                            Catch
                                style_error = style_error + " bc_pt_filter"
                            End Try
                            excelapp.application.activeworkbook.activesheet.cells(start_row + 8 + rc, start_col + 2) = formulas(i).item_factor
                            Try
                                excelapp.application.activeworkbook.activesheet.cells(start_row + 8 + rc, start_col + 2).style = "bc_pt_filter"
                            Catch
                                style_error = style_error + " bc_pt_filter"
                            End Try
                            With excelapp.application.activeworkbook.activesheet.cells(start_row + 8 + rc, start_col + 2).Validation()
                                .Delete()
                                .Add(Type:=3, AlertStyle:=1, Operator:= _
                                1, Formula1:="1,10,100,1000,1000000,0.01")
                                .IgnoreBlank = True
                                .InCellDropdown = True
                                .InputTitle = ""
                                .ErrorTitle = ""
                                .InputMessage = ""
                                .ErrorMessage = ""
                                .ShowInput = True
                                .ShowError = True
                            End With
                            Try
                                excelapp.application.activeworkbook.activesheet.cells(start_row + 8 + rc, start_col + 1).style = "bc_pt_filter"
                            Catch
                                style_error = style_error + " bc_pt_filter"
                            End Try

                        End If
                        rc = rc + 1
                    End If


                    excelapp.application.activesheet.cells(formulas(i).row_offset, formulas(i).col_offset).formula = formulas(i).formula
                    Try
                        If formulas(i).item_type = "item" Then
                            excelapp.application.activeworkbook.activesheet.cells(formulas(i).row_offset, formulas(i).col_offset).style = "bc_pt_data"
                            If formulas(i).item_symbol = "Date" Then
                                excelapp.application.activeworkbook.activesheet.cells(formulas(i).row_offset, formulas(i).col_offset).numberformat = "dd/mm/yyyy"
                            End If
                        ElseIf formulas(i).item_type = "curr" Then
                            excelapp.application.activeworkbook.activesheet.cells(formulas(i).row_offset, formulas(i).col_offset).style = "bc_pt_currency"
                        ElseIf formulas(i).item_type = "symbol" Then
                            excelapp.application.activeworkbook.activesheet.cells(formulas(i).row_offset, formulas(i).col_offset).style = "bc_pt_symbol"
                        Else
                            excelapp.application.activeworkbook.activesheet.cells(formulas(i).row_offset, formulas(i).col_offset).style = "bc_pt_ea"
                        End If
                    Catch ex As Exception
                        style_error = style_error + "bc_pt_data or bc_pt_currency or bt_pt_ea"
                    End Try
                    max_col = formulas(i).col_offset
                Next
            End If

            REM autosize used columns
            For i = start_col To max_col
                excelapp.activeworkbook.activesheet.Columns(i).Select()
                excelapp.selection.Columns.AutoFit()
            Next
            REM remove style sheet
            For i = 1 To excelapp.application.activeworkbook.worksheets.count
                If excelapp.application.activeworkbook.worksheets(i).name = "BC Portfolio Style Sheet" Then
                    oexcel.disable_application_alerts()
                    excelapp.application.activeworkbook.worksheets(i).delete()
                    oexcel.enable_application_alerts()
                    Exit For
                End If
            Next
            excelapp.application.activeworkbook.activesheet.cells(start_row - 1, start_col + 1).select()

            If time_series = True Then
                For i = 0 To tk_count - 1
                    Try
                        excelapp.application.activeworkbook.activesheet.cells(start_row + 9 + i, start_col).style = "bc_pt_data"
                    Catch
                        style_error = style_error + " bc_pt_data"
                    End Try
                Next
            End If

            'For i = 1 To excelapp.Application.AddIns2.Count
            '    If excelapp.Application.AddIns2(i).Name = "BC Excel Functions.xla" Then

            '        Try
            '            Try
            '                excelapp.application.activeworkbook.VBProject.References.AddFromFile(excelapp.Application.AddIns2(i).FullName)
            '            Catch

            '            End Try
            '            Application.DoEvents()
            '            excelapp.Application.Run("ASPXLDBFunctions.ThisWorkbook.run_after_extract")
            '        Catch
            '            Dim ocomm As New bc_cs_message("Blue Curve", "Failed to find run_after_extract batch mode failed", bc_cs_message.MESSAGE, False, False, "Ok", "Cancel", True)
            '        End Try
            '            Exit For
            '    End If
            'Next

            'excelapp.Application.Calculation = calc_state

        Catch ex As Exception
            MsgBox(ex.Message)

            output_to_excel = False
            Me.WindowState = FormWindowState.Minimized
            Dim omsg As New bc_cs_message("Blue Curve", "Error building extract in Excel into current worksheet. Please try again using new worksheet option.", bc_cs_message.MESSAGE, False, False, "OK", "Cancel", True)
            Dim ocomm As New bc_cs_activity_log("ba_am_portfolio_wizard", "output_to_excel", bc_cs_activity_codes.COMMENTARY, ex.Message)
            Me.rsheet.Checked = True
            Me.tprogress.Abort()
            Me.ppropress.Visible = False
            Me.WindowState = FormWindowState.Normal

        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            If style_error <> "" And output_to_excel = True Then
                Dim omsg As New bc_cs_message("Blue Curve", "Warnings some styles do not exist Extract may not look as required. Please check style sheet.", bc_cs_message.MESSAGE, False, False, "OK", "Cancel", True)
                Dim ocomm As New bc_cs_activity_log("ba_am_portfolio_wizard", "output_to_excel", bc_cs_activity_codes.COMMENTARY, style_error)

            End If
            excelapp.statusbar = False
            If output_to_excel = True Then
                excelapp = Nothing
            End If
            Me.Cursor = Cursors.Default
        End Try
    End Function
    Private Function excel_address(ByVal row As Integer, ByVal col As Integer, Optional ByVal abs_row As Boolean = False, Optional ByVal abs_col As Boolean = False) As String
        Dim j As Double
        Dim k As Double
        Dim l As Integer

        excel_address = ""

        j = col
        k = j Mod 26
        If k = 0 Then
            k = 26
        End If
        l = j / 26 - 0.51
        If l > 0 Then
            excel_address = Chr(l + 64)
        End If
        If abs_row = False And abs_col = False Then
            excel_address = excel_address + Chr(k + 64) + CStr(row)
        ElseIf abs_row = True And abs_col = True Then
            excel_address = "$" + excel_address + Chr(k + 64) + "$" + CStr(row)
        ElseIf abs_row = True And abs_col = False Then
            excel_address = excel_address + Chr(k + 64) + "$" + CStr(row)
        Else
            excel_address = "$" + excel_address + Chr(k + 64) + CStr(row)
        End If
    End Function
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If Me.twizard.SelectedIndex <> 0 Then
            Me.twizard.SelectedIndex = Me.twizard.SelectedIndex - 1
        End If
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.loading = True

        load_start()
        Me.ldesc.Text = ""
        Me.loading = False
        Dim dp As System.Drawing.Point
        dp.Y = 8
        dp.X = 472
        Me.bcancel.Location = dp
        Me.bcanceldisabled.Location = dp

        BlueCurve_Unversefilter.SearchSetup(False)
        BlueCurve_ItemsFilter.SearchSetup(False)

    End Sub
    Private Sub load_start()
        Dim i As Integer
        REM Dim bcs As New bc_cs_central_settings(True)
        bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.connection_method
        REM bc_cs_central_settings.logged_on_user_id = 1
        REM dont do this if already loaded
        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.LOCAL Then
            Dim omsg As New bc_cs_message("Blue Curve", "Cannot use BC functions as system is either off line or you are not an authenticated user", bc_cs_message.MESSAGE)
            Exit Sub
        End If

        For i = 1 To Me.twizard.TabPages.Count - 1
            Me.twizard.TabPages(i).Enabled = False
        Next
        Me.bforward.Visible = False
        Me.rnow.Checked = True
        For i = 0 To bc_am_ef_functions.stage_names.Count - 1
            Me.cstage.Items.Add(bc_am_ef_functions.stage_names(i))
        Next
        For i = 0 To bc_am_ef_functions.currency_codes.Count - 1
            Me.ccurr.Items.Add(bc_am_ef_functions.currency_codes(i))
        Next
        For i = 0 To 10
            Me.cprec.Items.Add(CStr(i))
        Next
        Me.cprectype.Items.Add("decimal places")
        Me.cprectype.Items.Add("significant figures")
        Me.cprectype.SelectedIndex = 0
        Me.cformat.Checked = True
        'Me.cprectype.SelectedIndex = 1
        Me.cprec.Text = "2"
        If Me.cstage.Items.Count > 0 Then
            Me.cstage.SelectedIndex = 0
        End If
        For i = 0 To bc_am_ef_functions.contributor_names.Count - 1
            Me.Ccont.Items.Add(bc_am_ef_functions.contributor_names(i))
        Next
        If Me.Ccont.Items.Count < 2 Then
            Me.Ccont.Enabled = False
        End If
        If Me.Ccont.Items.Count > 0 Then
            Me.Ccont.SelectedIndex = 0
        End If
        For i = 0 To bc_am_ef_functions.period_names.Count - 1
            Me.cperiods.Items.Add(bc_am_ef_functions.period_names(i))
        Next
        If Me.cperiods.Items.Count > -1 Then
            Me.lperiods.Items.Add(Me.cperiods.Items(0))
            Me.cperiods.Items.RemoveAt(0)
        End If
        If Me.cperiods.Items.Count = 0 Then
            Me.cperiods.Enabled = False
            Me.baddper.Enabled = False
            Me.bdelper.Enabled = False
        End If
        Me.c_show_ea.Enabled = False
        Me.c_show_ea.Checked = False

        For i = 1985 To 2025
            Me.Cyearstart.Items.Add(CStr(i))
            Me.cyearend.Items.Add(CStr(i))
        Next
        Me.Cyearstart.Text = Year(Now)
        Me.cyearend.Text = Year(Now)
        Me.rnow.Checked = True



        For i = 1 To Me.twizard.TabPages.Count - 1
            Me.twizard.TabPages(i).Enabled = False
        Next
        For i = 0 To bc_am_ef_functions.excel_functions.Count - 1
            If bc_am_ef_functions.excel_functions(i).show_in_wizard = 2 Then
                Me.cfunction.Items.Add(bc_am_ef_functions.excel_functions(i).display_name)
            End If
        Next
        Me.cfunction.Text = "get financial data"
        For i = 0 To bc_am_ef_functions.schema_names.Count - 1
            Me.cschema.Items.Add(bc_am_ef_functions.schema_names(i))
        Next
        If bc_am_ef_functions.schema_names.Count > 0 Then
            Me.cschema.SelectedIndex = 0
        End If

        For i = 0 To bc_am_ef_functions.class_names.Count - 1
            Me.cclass.Items.Add(bc_am_ef_functions.class_names(i))
            Me.cclass2.Items.Add(bc_am_ef_functions.class_names(i))
            Me.cclass3.Items.Add(bc_am_ef_functions.class_names(i))
        Next
        Me.ritemset.Checked = True
        Me.tx_filt = ""
        REM set role based user settings
        If bc_am_ef_functions.ousersettings.global_use = 1 Then
            Me.rglobal.Visible = False
            Me.Rprivate.Visible = False
            Me.rglobalsave.Visible = False
        End If
        If bc_am_ef_functions.ousersettings.stages <> 0 Then
            Me.cstage.Enabled = False
            If bc_am_ef_functions.ousersettings.stages = 1 Then
                Me.cstage.SelectedIndex = 0
            Else
                Me.cstage.SelectedIndex = 1
            End If
        Else
            Me.cstage.SelectedIndex = bc_am_ef_functions.ousersettings.default_stage
        End If

        'Steve Wooderson DA5
        For i = 0 To bc_am_ef_functions.chart_tools.Count - 1
            If bc_am_ef_functions.chart_tools(i).tool_active = True Then
                Me.cchart.Enabled = True
                chart_method = bc_am_ef_functions.chart_tools(i).tool_method_call
                chart_passrangeasparam = bc_am_ef_functions.chart_tools(i).tool_passrange
                chart_active = bc_am_ef_functions.chart_tools(i).tool_active
                chart_focus = "Primary"
            End If
        Next

        set_desc()

        'Dim oapi As New API
        'API.SetWindowPos(Me.Handle.ToInt32, API.HWND_TOPMOST, 10, 10, 1, 1, 1)
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Hide()
    End Sub

    Private Sub cclass_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cclass.SelectedIndexChanged
        Me.tx_c = ""
        Me.llink.Visible = False
        Me.clink.Visible = False
        If Me.cclass.SelectedIndex > -1 Then
            Me.rchoose.Checked = True
            Me.ritemset.Checked = True

            oseltemplates.entity_ids.Clear()
            Me.rall.Text = "All " + Me.cclass.Text + "(s)"
            Me.rchoose.Text = "Choose selected " + Me.cclass.Text + "(s)"
            Me.rclass.Text = "All " + Me.cclass.Text + "(s) within "
            REM deduce all parent classes
            Me.lallentity.Items.Clear()
            Me.cleadenity.Items.Clear()
            Me.lselentity.Items.Clear()
            Me.lleadentity.Text = "I would like to select " + Me.cclass.Text

            If BlueCurve_Unversefilter.SearchClass <> bc_am_ef_functions.class_ids(Me.cclass.SelectedIndex) Then
                BlueCurve_Unversefilter.SearchClass = bc_am_ef_functions.class_ids(Me.cclass.SelectedIndex)
                BlueCurve_Unversefilter.SearchSetup(False)
                If BlueCurve_Unversefilter.SearchText <> "" Then
                    BlueCurve_Unversefilter.SearchRefresh()
                End If
            End If
            LoadEntities()

            'For i = 0 To bc_am_ef_functions.entities.Count - 1

            '    If Len(Me.BlueCurve_Unversefilter.SearchText) = 0 Or (Me.BlueCurve_Unversefilter.SearchCurrentAttribute = "Name" And InStr(UCase(bc_am_ef_functions.entities(i).name), UCase(BlueCurve_Unversefilter.SearchText)) <> 0) Then
            '        If bc_am_ef_functions.entities(i).class_id = bc_am_ef_functions.class_ids(Me.cclass.SelectedIndex) Then
            '            Me.lallentity.Items.Add(bc_am_ef_functions.entities(i).name)
            '            Me.cleadenity.Items.Add((bc_am_ef_functions.entities(i).name))
            '        End If
            '    End If
            'Next

            If Me.rchoose.Checked = True Then
                Me.tx_c = "for selected " + Me.cclass.Text + "(s)"
            ElseIf Me.rall.Checked = True Then
                Me.tx_c = "for all " + Me.cclass.Text + "(s)"
                If Me.clink.SelectedIndex > 0 Then
                    Me.tx_c = Me.tx_c + " and associated " + Me.clink.Text + "(s)"
                End If

            End If
            set_desc()
            Me.twizard.TabPages(2).Enabled = True
            set_forward_enabled()

            Me.bok.Visible = False

            REM setup  porpgating class

            set_linked_classes()

            If Me.cclass2.Items.Count > 0 Then
                Me.llink.Visible = True
                Me.clink.Visible = True
            End If
            load_saved_sets()
        End If
    End Sub

    Public Sub LoadEntities()

        Dim i, j As Integer
        Dim list1 As New ArrayList
        Dim outstr As String
        Dim istr As String
        Dim ty As String = ""
        Dim outid As Long
        Dim isInSearech As Boolean = False

        Try

            lallentity.BeginUpdate()

            If rclass.Checked Then

                consolidate_entities()
                Exit Sub
            End If



            If Me.Cfclass.SelectedIndex <= 0 Then

                REM No filter
                Me.cleadenity.Items.Clear()
                Me.lallentity.Items.Clear()

                For i = 0 To bc_am_ef_functions.entities.Count - 1

                    REM Check if in search
                    isInSearech = False
                    If BlueCurve_Unversefilter.SearchText <> "" Then


                        If BlueCurve_Unversefilter.ShownEntityList.Contains(bc_am_ef_functions.entities(i).id) = True Then
                            isInSearech = True
                        End If

                        'For x = 0 To BlueCurve_Unversefilter.ShownEntityList.Count - 1
                        '    If bc_am_ef_functions.entities(i).id = BlueCurve_Unversefilter.ShownEntityList(x) Then
                        '        isInSearech = True
                        '        Exit For
                        '    End If
                        'Next x
                    Else
                        isInSearech = True
                    End If

                    If isInSearech Then
                        'If Len(Me.BlueCurve_Unversefilter.SearchText) = 0 Or (Me.BlueCurve_Unversefilter.SearchCurrentAttribute = "Name" And InStr(UCase(bc_am_ef_functions.entities(i).name), UCase(BlueCurve_Unversefilter.SearchText)) <> 0) Then
                        If bc_am_ef_functions.entities(i).class_id = bc_am_ef_functions.class_ids(Me.cclass.SelectedIndex) Then
                            If Me.lselentity.Items.Contains(bc_am_ef_functions.entities(i).name) = False Then
                                Me.lallentity.Items.Add(bc_am_ef_functions.entities(i).name)
                            End If
                            Me.cleadenity.Items.Add((bc_am_ef_functions.entities(i).name))
                        End If
                    End If
                Next
                Exit Sub
            End If


            If Me.Cfclass.SelectedIndex > 0 Then

                REM primary filter
                For i = 0 To bc_am_ef_functions.class_ids.Count - 1
                    For j = 0 To Me.use_class_ids.Count - 1
                        If Me.use_class_ids(j) = bc_am_ef_functions.class_ids(i) And bc_am_ef_functions.class_names(i) = Me.Cfclass.Text Then
                            If Me.use_class_types(j) = "Parent" Then
                                ty = 5006
                            Else
                                ty = 5005
                            End If
                            Exit For
                        End If
                    Next
                Next
                outstr = load_propogating_entities(ty, Me.Cfclass.Text, Me.Cfentity.Text, Me.cclass.Text)
                If outstr <> "" And InStr(outstr, ";") = 0 Then
                    list1.Add(outstr)
                Else
                    REM chop it up
                    While InStr(outstr, ";") > 0
                        istr = outstr.Substring(0, InStr(outstr, ";") - 1)
                        list1.Add(istr)
                        outstr = outstr.Substring(InStr(outstr, ";"), Len(outstr) - InStr(outstr, ";"))
                    End While
                End If

                Me.lallentity.Items.Clear()
                For i = 0 To list1.Count - 1

                    REM Check if in search
                    isInSearech = False
                    If BlueCurve_Unversefilter.SearchText <> "" Then
                        outid = 0
                        For x = 0 To bc_am_ef_functions.entities.Count - 1
                            If list1(i) = bc_am_ef_functions.entities(x).name Then
                                outid = bc_am_ef_functions.entities(x).id
                                Exit For
                            End If
                        Next
                        If outid <> 0 And BlueCurve_Unversefilter.ShownEntityList.Count > 0 Then


                            If BlueCurve_Unversefilter.ShownEntityList.Contains(outid) = True Then
                                isInSearech = True
                            End If

                            'For x = 0 To BlueCurve_Unversefilter.ShownEntityList.Count - 1
                            '    If outid = BlueCurve_Unversefilter.ShownEntityList(x) Then
                            '        'If bc_am_ef_functions.entities(i).id = BlueCurve_Unversefilter.ShownEntityList(x) Then
                            '        isInSearech = True
                            '        Exit For
                            '    End If
                            'Next x
                        End If
                    Else
                        isInSearech = True
                    End If

                    If isInSearech Then
                        If Me.lselentity.Items.Contains(list1(i)) = False Then
                            Me.lallentity.Items.Add(list1(i))
                        End If
                    End If
                Next
            End If

            'For i = 0 To bc_am_ef_functions.entities.Count - 1
            '    If Len(Me.BlueCurve_Unversefilter.SearchText) = 0 Or (Me.BlueCurve_Unversefilter.SearchCurrentAttribute = "Name" And InStr(UCase(bc_am_ef_functions.entities(i).name), UCase(BlueCurve_Unversefilter.SearchText)) <> 0) Then
            '        If bc_am_ef_functions.entities(i).class_id = bc_am_ef_functions.class_ids(Me.cclass.SelectedIndex) Then
            '            Me.lallentity.Items.Add(bc_am_ef_functions.entities(i).name)
            '            Me.cleadenity.Items.Add((bc_am_ef_functions.entities(i).name))
            '        End If
            '    End If
            'Next

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_portfolio_wizaed", "LoadEntities", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            lallentity.EndUpdate()

        End Try

    End Sub



    Private Sub set_linked_classes()
        Dim i, j As Integer
        Dim schema_id As Long
        Dim host_class_id As Long
        Me.llink.Visible = False
        Me.clink.Visible = False
        Me.clink.SelectedIndex = -1
        Dim del_list As New ArrayList
        del_list.Clear()
        Me.cclass2.Items.Clear()
        Me.use_class_ids.Clear()
        Me.use_class_types.Clear()
        Me.cclass2.Items.Clear()
        Me.Cfclass.Items.Clear()
        Me.Cfentity.Items.Clear()
        Me.Cfentity.Enabled = False
        schema_id = bc_am_ef_functions.schema_ids(Me.cschema.SelectedIndex)
        host_class_id = bc_am_ef_functions.class_ids(Me.cclass.SelectedIndex)
        prop_dn(host_class_id, schema_id, use_class_ids, use_class_types)

        prop_up(host_class_id, schema_id, use_class_ids, use_class_types)

        del_list.Clear()
        Me.clink.Items.Clear()
        REM remove duplicates
        For i = 0 To use_class_ids.Count - 1
            del_list.Add(0)
            For j = 0 To use_class_ids.Count - 1
                If j > i Then
                    If use_class_ids(j) = use_class_ids(i) Then
                        del_list(i) = 1
                    End If
                End If
            Next
        Next
        Me.cclass2.Items.Clear()
        Me.cclass3.Items.Clear()
        Me.Cfclass.Items.Clear()
        Me.Cfentity.Items.Clear()
        Me.Cfentity.Enabled = False

        Me.Cfclass.Items.Add("None")
        Me.Cfclass.Sorted = False
        Me.clink.Items.Add("none")
        Me.clink.Sorted = False
        Me.clink.SelectedIndex = 0
        For i = 0 To use_class_ids.Count - 1
            For j = 0 To bc_am_ef_functions.class_ids.Count - 1
                If bc_am_ef_functions.class_ids(j) = use_class_ids(i) Then
                    If del_list(i) = 0 And UCase(bc_am_ef_functions.class_names(j)) <> "ROOT " Then
                        Me.cclass2.Items.Add(bc_am_ef_functions.class_names(j))
                        Me.cclass3.Items.Add(bc_am_ef_functions.class_names(j))
                        Me.Cfclass.Items.Add(bc_am_ef_functions.class_names(j))
                        Me.clink.Items.Add(bc_am_ef_functions.class_names(j))

                    End If
                    Exit For
                End If
            Next
        Next
        REM add index
        Dim found As Boolean = False
        If Me.cclass.Text = "Instrument" Then
            For i = 0 To Me.clink.Items.Count - 1
                If Me.clink.Items(i) = "Index" Then
                    found = True
                End If
            Next
            If found = False Then
                Me.clink.Items.Add("Index")
            End If
        End If
    End Sub
    Private Sub prop_up(ByVal host_class_id As Long, ByVal schema_id As Long, ByRef use_class_ids As ArrayList, ByRef use_class_types As ArrayList)
        Dim i As Integer
        For i = 0 To bc_am_ef_functions.class_links.Count - 1
            If bc_am_ef_functions.class_links(i).child_class_id = host_class_id And bc_am_ef_functions.class_links(i).schema_id = schema_id Then
                use_class_ids.Add(bc_am_ef_functions.class_links(i).parent_class_id)
                use_class_types.Add("Parent")
                prop_up(bc_am_ef_functions.class_links(i).parent_class_id, schema_id, use_class_ids, use_class_types)
            End If
        Next
    End Sub
    Private Sub prop_dn(ByVal host_class_id As Long, ByVal schema_id As Long, ByRef use_class_ids As ArrayList, ByRef use_class_types As ArrayList)
        Dim i As Integer
        For i = 0 To bc_am_ef_functions.class_links.Count - 1
            If bc_am_ef_functions.class_links(i).parent_class_id = host_class_id And bc_am_ef_functions.class_links(i).schema_id = schema_id Then
                use_class_ids.Add(bc_am_ef_functions.class_links(i).child_class_id)
                use_class_types.Add("child")
                prop_dn(bc_am_ef_functions.class_links(i).child_class_id, schema_id, use_class_ids, use_class_types)
            End If
        Next
    End Sub

    Private Sub cclass2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cclass2.SelectedIndexChanged
        If Me.cclass2.SelectedIndex = -1 Then
            Exit Sub
        End If
        Dim i, j As Integer
        Me.centity.Items.Clear()
        For i = 0 To bc_am_ef_functions.class_names.Count - 1
            If bc_am_ef_functions.class_names(i) = Me.cclass2.Text Then
                For j = 0 To bc_am_ef_functions.entities.Count - 1
                    Me.centity.Enabled = True
                    If bc_am_ef_functions.entities(j).class_id = bc_am_ef_functions.class_ids(i) Then
                        Me.centity.Items.Add(bc_am_ef_functions.entities(j).name)
                    End If
                Next
                For j = 0 To Me.use_class_ids.Count - 1
                    If Me.use_class_ids(j) = bc_am_ef_functions.class_ids(i) Then
                        If Me.use_class_types(j) = "Parent" Then
                            Me.rclass.Text = "All " + Me.cclass.Text + "(s) within"
                        Else
                            Me.rclass.Text = "All " + Me.cclass.Text + "(s) used by"
                        End If
                        Exit For
                    End If

                Next
                Exit For
            End If
        Next
        tx_c = ""
        set_desc()

    End Sub
    Private Sub cfclass_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cfclass.SelectedIndexChanged
        Try
            If Me.Cfclass.SelectedIndex = -1 Then
                Exit Sub
            End If
            Me.Cursor = Cursors.WaitCursor

            Dim i, j As Integer
            Me.lallentity.Items.Clear()

            'If Me.Cfclass.SelectedIndex = 0 Then
            '    For i = 0 To bc_am_ef_functions.entities.Count - 1
            '        If bc_am_ef_functions.entities(i).class_id = bc_am_ef_functions.class_ids(Me.cclass.SelectedIndex) Then
            '            Me.lallentity.Items.Add(bc_am_ef_functions.entities(i).name)
            '        End If
            '    Next
            '    Exit Sub
            'End If

            Me.Cfentity.Items.Clear()
            Me.Cfentity.Enabled = False
            For i = 0 To bc_am_ef_functions.class_names.Count - 1
                If bc_am_ef_functions.class_names(i) = Me.Cfclass.Text Then
                    For j = 0 To bc_am_ef_functions.entities.Count - 1
                        Me.Cfentity.Enabled = True
                        If bc_am_ef_functions.entities(j).class_id = bc_am_ef_functions.class_ids(i) Then
                            Me.Cfentity.Items.Add(bc_am_ef_functions.entities(j).name)
                        End If
                    Next
                    Exit For
                End If
            Next
        Catch

        Finally
            Me.Cursor = Cursors.Default

        End Try
    End Sub
    Private Sub rall_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rall.CheckedChanged

        If rall.Checked = True Then
            Me.tentname.Visible = False
            Me.bsaveent.Visible = False

            Me.Lfilter.Visible = False
            Me.Cfclass.Visible = False
            Me.Cfentity.Visible = False
            Me.bentup.Visible = False
            Me.bentdn.Visible = False
            Me.ritemset.Checked = True
            Me.bsaveent.Visible = False
            Me.tentname.Visible = False

            oseltemplates.entity_ids.Clear()
            Me.lallentity.Visible = False
            Me.lselentity.Visible = True
            Me.lselentity.Width = 480
            Me.lselentity.Location = Me.lallentity.Location
            Me.lselentity.SelectionMode = SelectionMode.MultiSimple



            Me.bselent.Visible = False
            Me.brement.Visible = False
            Me.cclass2.Visible = False
            Me.centity.Visible = False
            Me.cclass2.SelectedIndex = -1
            Me.centity.Items.Clear()
            'Dim i As Integer
            Me.lselentity.Items.Clear()
            Me.lallentity.Items.Clear()

            BlueCurve_Unversefilter.SearchControl = "lselentity"
            BlueCurve_Unversefilter.SearchRefresh()

            'For i = 0 To bc_am_ef_functions.entities.Count - 1
            '    If bc_am_ef_functions.entities(i).class_id = bc_am_ef_functions.class_ids(Me.cclass.SelectedIndex) Then
            '        Me.lselentity.Items.Add(bc_am_ef_functions.entities(i).name)
            '    End If
            'Next
        End If
        Me.tx_c = "for all " + Me.cclass.Text + "'s"
        Me.set_desc()

        set_forward_enabled()
        Me.bok.Visible = False
        Me.twizard.TabPages(3).Enabled = True

    End Sub

    Private Sub rchoose_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rchoose.CheckedChanged
      
        Dim i As Integer
        Me.tx_c = ""
        Me.lselentity.Sorted = True
        If rchoose.Checked = True And Me.cclass.SelectedIndex > -1 Then
            If rchoose.Enabled = False Then
                Me.tentname.Visible = False
                Me.bsaveent.Visible = False
            Else
                Me.tentname.Visible = True
                Me.bsaveent.Visible = True
            End If
            Me.Cfclass.SelectedIndex = -1
            Me.Cfentity.Items.Clear()
            Me.Lfilter.Visible = True
            Me.Cfclass.Visible = True
            Me.Cfentity.Visible = True
            Me.bentup.Visible = True
            Me.bentdn.Visible = True
            Me.bentdn.Enabled = False
            Me.bentup.Enabled = False

            Dim dp As System.Drawing.Point
            dp.Y = Me.lselentity.Location.Y
            dp.X = 248
            Me.lselentity.Enabled = True
            Me.lselentity.SelectionMode = SelectionMode.One

            Me.lselentity.Location = dp
            Me.bsaveent.Visible = True
            Me.tentname.Visible = True
            Me.bsaveent.Enabled = False
            Me.tentname.Enabled = False
            Me.tentname.Text = ""


            oseltemplates.entity_ids.Clear()
            Me.lallentity.Visible = True
            Me.lselentity.Visible = True
            Me.bselent.Visible = True
            Me.brement.Visible = True
            Me.cclass2.Visible = False
            Me.centity.Visible = False
            Me.lselentity.Width = Me.lallentity.Width

            Me.cclass2.SelectedIndex = -1
            Me.centity.Items.Clear()
            Me.lselentity.Items.Clear()
            Me.lallentity.Items.Clear()

            BlueCurve_Unversefilter.SearchControl = "lallentity"
            BlueCurve_Unversefilter.SearchRefresh()

            'For i = 0 To bc_am_ef_functions.entities.Count - 1
            '    If bc_am_ef_functions.entities(i).class_id = bc_am_ef_functions.class_ids(Me.cclass.SelectedIndex) Then
            '        Me.lallentity.Items.Add(bc_am_ef_functions.entities(i).name)
            '    End If
            'Next

            Me.tx_c = ""
            If Me.lselentity.Items.Count = 1 Then
                Me.tx_c = "for selected " + Me.cclass.Text + " " + Me.lselentity.Items(0)
            ElseIf Me.centity.Items.Count > 1 Then
                Me.tx_c = "for selected " + Me.cclass.Text + "'s " + Me.lselentity.Items(0)
                For i = 1 To Me.centity.Items.Count - 1
                    Me.tx_c = Me.tx_c + ";" + Me.centity.Items(i)
                Next
            Else
                Me.tx_c = "for no selected " + Me.cclass.Text + "'s"
            End If
        End If
        set_desc()
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bselent.Click
        If Me.cass.Visible = True And Me.cass.SelectedIndex = 1 Then
            set_ass_entity()
        Else
            set_entity()
        End If
    End Sub
    Private Sub set_entity()
        'allred
        Dim attempts As Integer = 0
        While Me.lallentity.SelectedIndex > -1 And attempts < MAX_ATTEMPTS
            REM if predefined then check if limit exceeded
            If Me.Rpredefined.Checked = True Then
                If Me.oportfolios.portfolios(Me.Lpredefined.SelectedItems(0).Index).num_entities_select > 0 Then
                    If Me.oportfolios.portfolios(Me.Lpredefined.SelectedItems(0).Index).num_entities_select = Me.lselentity.Items.Count Then
                        Dim omsg As New bc_cs_message("Blue Curve", "You can only set: " + CStr(Me.oportfolios.portfolios(Me.Lpredefined.SelectedItems(0).Index).num_entities_select) + " " + Me.cclass.Text + "(s) for this pre defined extract", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                        Exit Sub
                    End If
                End If
                Me.cass.Enabled = False
                If Me.lselentity.Items.Count = 0 Then
                    Me.cass.Enabled = True
                End If
            End If
            Me.ritemset.Checked = True
            Me.lselentity.Sorted = False
            Me.tentname.Enabled = True
            Me.lselentity.Items.Add(Me.lallentity.SelectedItem)
            Me.lallentity.Items.RemoveAt(Me.lallentity.SelectedIndex)
            If Me.cass.Visible = False Then
                set_forward_enabled()
                Me.twizard.TabPages(3).Enabled = True

            Else
                If Me.Lselass.Items.Count > 0 Then
                    set_forward_enabled()
                    Me.twizard.TabPages(3).Enabled = True

                End If
            End If
            Me.bok.Visible = False
            set_desc()
            If Me.lselitem.Items.Count > 0 And Me.lselitem.Enabled = False Then
                Me.bok.Visible = False
                Me.twizard.TabPages(4).Enabled = True
                Me.twizard.TabPages(5).Enabled = True
            End If

            attempts += 1
            'allred
        End While
    End Sub
    Private Sub set_ass_entity()
        'allred
        Dim attempts As Integer = 0
        While Me.lallass.SelectedIndex > -1 And attempts < MAX_ATTEMPTS
            REM if predefined then check if limit exceeded
            If Me.Rpredefined.Checked = True Then
                If Me.oportfolios.portfolios(Me.Lpredefined.SelectedItems(0).Index).num_entities_select > 0 Then
                    If Me.oportfolios.portfolios(Me.Lpredefined.SelectedItems(0).Index).num_entities_select = Me.Lselass.Items.Count Then
                        Dim omsg As New bc_cs_message("Blue Curve", "You can only set: " + CStr(Me.oportfolios.portfolios(Me.Lpredefined.SelectedItems(0).Index).num_entities_select) + " " + Me.clink.Text + "(s) for this pre defined extract", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                        Exit Sub
                    End If
                End If
            End If
            Me.ritemset.Checked = True
            Me.lselentity.Sorted = False
            Me.tentname.Enabled = True
            Me.Lselass.Items.Add(Me.lallass.Text)
            Me.lallass.Items.RemoveAt(Me.lallass.SelectedIndex)
            If Me.lselentity.Items.Count > 0 Then
                set_forward_enabled()
                Me.twizard.TabPages(3).Enabled = True

            End If
            Me.bok.Visible = False
            Me.twizard.TabPages(3).Enabled = True
            set_desc()
            attempts += 1
            'allred
        End While
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles brement.Click

        If Me.cass.Visible = True And Me.cass.SelectedIndex = 1 Then
            clear_ass_entity()
        Else
            clear_entity()
        End If
    End Sub
    Private Sub clear_entity()
        If Me.lselentity.Items.Count = 1 Then
            Me.bsaveent.Enabled = False
            Me.tentname.Enabled = False
            Me.tentname.Text = ""
        End If
        'allred
        Dim attempts As Integer = 0
        While Me.lselentity.SelectedIndex > -1 And attempts < MAX_ATTEMPTS
            Me.ritemset.Checked = True
            Me.lallentity.Items.Add(Me.lselentity.Text)
            Me.lselentity.Items.RemoveAt(Me.lselentity.SelectedIndex)
            attempts += 1
            'allred
        End While
    End Sub
    Private Sub clear_ass_entity()
        If Me.Lselass.Items.Count = 1 Then
            Me.bsaveent.Enabled = False

        End If
        'allred
        Dim attempts As Integer = 0
        While Me.Lselass.SelectedIndex > -1 And attempts < MAX_ATTEMPTS
            Me.lallass.Items.Add(Me.Lselass.Text)
            Me.Lselass.Items.RemoveAt(Me.Lselass.SelectedIndex)
            attempts += 1
            'allred
        End While
    End Sub
    Private Sub set_desc()
        Try
            Dim i, j As Integer

            If Me.tx_u Is Nothing Then
                Exit Sub
            End If

            If Me.rnew.Checked = True Then
                Me.tx_f = "I would like to " + Me.cfunction.Text + " and place in new extract " + Me.tname.Text
            ElseIf Me.ropen.Checked = True Then
                Me.tx_f = "I would like to " + Me.cfunction.Text + " and place in existing extract " + Me.tname.Text
            Else
                If Me.Lpredefined.SelectedItems.Count = 1 Then
                    Me.tx_f = "I would like to " + Me.cfunction.Text + " and place in predfined extract " + Me.Lpredefined.SelectedItems(0).Text
                End If
            End If
            If Me.lselitem.Items.Count = 1 Then
                Me.tx_u = "I would like to show item " + Me.lselitem.Items(0)
            ElseIf Me.lselitem.Items.Count > 1 Then
                Me.tx_u = "I would like to show items " + Me.lselitem.Items(0)
                For j = 1 To Me.lselitem.Items.Count - 1
                    Me.tx_u = Me.tx_u + ";" + Me.lselitem.Items(j)
                Next
            End If
            If Me.tx_u.Length > 151 Then
                Me.tx_u = Me.tx_u.Substring(0, 150) + "...."
            End If
            'If Me.cfunction.SelectedIndex = 0 Then
            If Me.Rpredefined.Checked = False Or Me.rchoose.Checked = False Then
                If Me.rchoose.Checked = True And Me.cclass.SelectedIndex > -1 Then
                    Me.tx_c = ""
                    If Me.lselentity.Items.Count = 1 Then
                        Me.tx_c = "for selected " + Me.cclass.Text + " "
                        If Me.clink.SelectedIndex > 0 Then
                            Me.tx_c = Me.tx_c + " and associated " + Me.clink.Text + "(s) "
                        End If
                        Me.tx_c = Me.tx_c + Me.lselentity.Items(0)
                    ElseIf Me.lselentity.Items.Count > 1 Then
                        Me.tx_c = "for selected " + Me.cclass.Text + "(s) "
                        If Me.clink.SelectedIndex > 0 Then
                            Me.tx_c = Me.tx_c + " and associated " + Me.clink.Text + "(s) "
                        End If
                        Me.tx_c = Me.tx_c + Me.lselentity.Items(0)
                    End If
                    For i = 1 To Me.lselentity.Items.Count - 1
                        Me.tx_c = Me.tx_c + ";" + Me.lselentity.Items(i)
                    Next
                End If
            Else
                If Me.rchoose.Checked = True And Me.cclass.SelectedIndex > -1 Then
                    Me.tx_c = ""
                    Dim tx_p As String = ""
                    For i = 0 To Me.lselentity.Items.Count - 1
                        If i = 0 Then
                            Me.tx_c = Me.lselentity.Items(i)
                        Else
                            Me.tx_c = Me.tx_c + ";" + Me.lselentity.Items(i)
                        End If
                    Next
                    For i = 0 To Me.Lselass.Items.Count - 1
                        If i = 0 Then
                            tx_p = Me.Lselass.Items(i)
                        Else
                            tx_p = tx_p + ";" + Me.lselentity.Items(i)
                        End If
                    Next

                    Me.tx_c = "for selected " + Me.cclass.Text + "(s) " + Me.tx_c
                    If Me.clink.SelectedIndex > 0 Then
                        Me.tx_c = Me.tx_c + " and associated " + Me.clink.Text + "(s) " + tx_p
                    End If

                End If
            End If
            If Me.rall.Checked = True And Me.cclass.SelectedIndex > -1 Then
                Me.tx_c = "for all " + Me.cclass.Text + "(s)"
                If Me.clink.SelectedIndex > 0 Then
                    Me.tx_c = Me.tx_c + " and associated " + Me.clink.Text + "(s) "
                End If
            End If
            If Me.rclass.Checked = True And Me.cclass.SelectedIndex > -1 Then
                Me.tx_c = "for all " + Me.cclass.Text + " in " + Me.cclass2.Text + " " + Me.centity.Text
                If Me.centity2.SelectedIndex > -1 Then
                    Me.tx_c = Me.tx_c + " and " + Me.cclass3.Text + " " + Me.centity2.Text
                End If
                If Me.clink.SelectedIndex > 0 Then
                    Me.tx_c = Me.tx_c + " and associated " + Me.clink.Text + "'(s) "
                End If
            End If
            If Me.rentity_set.Checked = True And Me.cclass.SelectedIndex > -1 Then
                Me.tx_c = "for all " + Me.cclass.Text + " in user set " + Me.centityset.Text
                If Me.clink.SelectedIndex > 0 Then
                    Me.tx_c = Me.tx_c + " and associated " + Me.clink.Text + "(s) "
                End If
            End If
            'Else
            'Me.tx_c = "for " + Me.cclass.Text + " " + Me.cleadenity.Text
            'End If
            If Me.tx_c.Length > 151 Then
                Me.tx_c = Me.tx_c.Substring(0, 150) + "...."
            End If
            If Me.twizard.TabPages(5).Enabled = True Then
                If Me.rcurrconv.Checked = True Then
                    If Me.ccurr.SelectedIndex > -1 Then
                        tx_curr = "I would like to convert all the output to currency " + Me.ccurr.Text
                    End If
                ElseIf Me.rcurrnone.Checked = True Then
                    tx_curr = "I would to show the currency each item is in"
                Else
                    tx_curr = "I would not like to currency convert the output"
                End If
                If Me.c_show_ea.Checked = True And Me.c_show_ea.Enabled = True Then
                    tx_curr = tx_curr + "; show estimate/actual flag"
                End If
                If Me.cformat.Checked = True Then
                    If Me.cprectype.SelectedIndex = 0 Then
                        tx_curr = tx_curr + "; format to " + Me.cprec.Text + " dps"
                    Else
                        tx_curr = tx_curr + "; format to " + Me.cprec.Text + " sig figs"
                    End If
                End If
            Else
                tx_curr = ""
            End If
            If Me.pformat.Visible = False Then
                tx_curr = ""
            End If
            If Me.twizard.TabPages(4).Enabled = True Then
                tx_filt = criteria_tx()
            Else
                tx_filt = ""
            End If

            If cchart.Checked = True Then
                tx_chart = "I would like to produce a chart from the output"
            Else
                tx_chart = ""
            End If

            Dim tx As String = ""
            Me.ldesc.Text = tx_f + vbCrLf + tx_c + vbCrLf + tx_u + vbCrLf + tx_filt + vbCrLf + tx_curr + vbCrLf + tx_chart
        Catch ex As Exception
            'Dim oerr As New bc_cs_error_log("bc_am_portfolio_wizard", "set_desc", bc_cs_error_codes.USER_DEFINED, ex.Message)


        End Try

    End Sub

    Private Sub cfunction_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cfunction.SelectedIndexChanged


        tx_f = "I would like to " + Me.cfunction.Text
        If Trim(Me.tname.Text) <> "" Then
            set_desc()
            set_forward_enabled()
            Me.bok.Visible = False


            Me.twizard.TabPages(1).Enabled = True
            Me.lclass.Text = "I would like to " + Me.cfunction.Text + " for "
        End If
        Me.puniverse.Visible = True
        Me.pleadentity.Visible = False
        Me.pdatefrom.Visible = False
        Me.ldateto.Text = "Data at date"
        Me.Rdate.Text = "at"
        Me.rnow.Text = "current"

        If Me.cfunction.SelectedIndex = 1 Then
            Me.pleadentity.Visible = True
            Me.ldateto.Text = "Data to"
            Me.Rdate.Text = "to"
            Me.rnow.Text = "current"
            Me.pdatefrom.Visible = True
        End If
    End Sub

    Private Sub rclass_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rclass.CheckedChanged
        Me.cclass2.Visible = False
        Me.centity.Visible = False
        Me.lallentity.Visible = False
        Me.lselentity.Visible = True
        Me.lselentity.Width = 480
        Me.cand.Visible = False
        Me.cand.Checked = False
        Me.ritemset.Checked = True
        Me.bentup.Visible = False
        Me.bentdn.Visible = False

        Me.lselentity.Location = Me.lallentity.Location
        If Me.cclass.SelectedIndex > -1 And Me.rclass.Checked = True Then
            Me.tentname.Visible = False
            Me.bsaveent.Visible = False

            Me.Lfilter.Visible = False
            Me.Cfclass.Visible = False
            Me.Cfentity.Visible = False
            Me.lselentity.Items.Clear()
            Me.bsaveent.Visible = False
            Me.tentname.Visible = False
            Me.lselentity.Visible = True
            Me.lselentity.Width = 480
            Me.lselentity.SelectionMode = SelectionMode.MultiSimple

            Me.centity.Enabled = False
            Me.cclass2.SelectedIndex = -1
            oseltemplates.entity_ids.Clear()
            Me.lallentity.Visible = False
            Me.bselent.Visible = True
            Me.brement.Visible = False
            Me.cclass2.Visible = True
            Me.centity.Visible = True
            Me.tx_c = ""
            set_desc()

            BlueCurve_Unversefilter.SearchControl = "lselentity"

        End If

    End Sub

    Private Sub centity_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles centity.SelectedIndexChanged
        Try

            If Me.centity.SelectedIndex > -1 Then
                Me.Cursor = Cursors.WaitCursor




                Me.cand.Visible = True
                If InStr(Me.rclass.Text, "used") > 0 Then
                    Me.tx_c = "for all " + Me.cclass.Text + "'s" + " used by " + Me.cclass2.Text + ": " + Me.centity.Text
                Else
                    Me.tx_c = "for all " + Me.cclass.Text + "'s" + " in " + Me.cclass2.Text + ": " + Me.centity.Text
                End If

                If Me.clink.SelectedIndex > 0 Then
                    Me.tx_c = Me.tx_c + " and associated " + Me.clink.Text + "'s"
                End If

                Me.lselentity.Width = 480
                Me.lselentity.Location = Me.lallentity.Location
                Me.lselentity.Visible = True
                set_forward_enabled()
                Me.bok.Visible = False

                Me.twizard.TabPages(3).Enabled = True

                LoadEntities()

                set_desc()
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("centity", "selectedindexchanged", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            Me.Cursor = Cursors.Default

        End Try
    End Sub
    Private Function load_entities_of_class(ByVal class_name As String)
        Dim str As String
        str = "<excel_function>" + vbCr + _
         "<type>5001</type>" + vbCr + _
         "<class_id>" + class_name + "</class_id>" + vbCr + _
         "<dimensions>name</dimensions>" + vbCr + _
         "</excel_function>"
        Dim oef As New bc_am_excel_functions
        load_entities_of_class = oef.execute_function(str)

    End Function

    Private Function load_propogating_entities(ByVal ty As Integer, ByVal pclass As String, ByVal entity As String, ByVal child_class As String) As String
        Dim str As String

        str = "<excel_function>" + vbCr + _
        "<type>" + CStr(ty) + "</type>" + vbCr + _
        "<class_id>" + pclass + "</class_id>" + vbCr + _
        "<entity_id>" + entity + "</entity_id>" + vbCr + _
        "<ass_class_id>" + child_class + "</ass_class_id>" + vbCr + _
        "<schema_id>" + Me.cschema.Text + "</schema_id>" + vbCr + _
        "<priority>all</priority>" + vbCr + _
        "<dimensions>name</dimensions>" + vbCr + _
        "</excel_function>"

        Dim oef As New bc_am_excel_functions
        load_propogating_entities = oef.execute_function(str)


    End Function

    Private Sub twizard_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles twizard.SelectedIndexChanged
        Try
            set_desc()

            If Me.twizard.SelectedIndex <> 0 Then
                Dim dp As System.Drawing.Point
                dp.Y = 8
                dp.X = 344
                Me.bcancel.Location = dp
                Me.bcanceldisabled.Location = dp
            End If
            If Me.twizard.SelectedIndex > 0 Then
                Dim dp As System.Drawing.Point
                dp.Y = 8
                dp.X = 408
                Me.bleft.Location = dp
                Me.bleftdisabled.Location = dp
            End If
            If Me.twizard.SelectedIndex < 5 And Me.bforward.Visible = False Then
                Dim dp As System.Drawing.Point
                dp.Y = 8
                dp.X = 408
                Me.bcancel.Location = dp
                Me.bcanceldisabled.Location = dp
                dp.X = 472
                Me.bleft.Location = dp
                Me.bleftdisabled.Location = dp
            End If

            Dim i As Integer
            Me.bforward.Visible = False
            If Me.twizard.SelectedIndex < Me.twizard.TabPages.Count - 2 Then
                If Me.twizard.TabPages(Me.twizard.SelectedIndex + 1).Enabled = True Then
                    set_forward_enabled()
                    Me.bok.Visible = False
                End If
            End If
            If Me.twizard.SelectedIndex = 0 Then
                Me.bleft.Visible = False
                Me.bleftdisabled.Visible = False
            Else
                Me.bleft.Visible = True
            End If

            If Me.twizard.SelectedIndex = 3 And Me.lselitem.Items.Count > 0 And Me.lselitem.Enabled = False Then
                set_forward_enabled()
                Me.twizard.TabPages(4).Enabled = True
                Me.bok.Visible = False
                Me.twizard.TabPages(4).Enabled = True
                Me.twizard.TabPages(5).Enabled = True
            End If

            If Me.twizard.SelectedIndex = 4 Or Me.twizard.SelectedIndex = 5 Then

                tx_filt = criteria_tx()
                'set_desc()
                If Me.twizard.TabPages(3).Enabled = True Then
                    set_forward_enabled()
                    Me.bok.Visible = False
                End If
                Me.Cyearstart.Enabled = False
                Me.cyearend.Enabled = False
                Me.baddper.Enabled = False
                Me.bdelper.Enabled = False
                Me.lperiods.Enabled = False
                Me.cperiods.Enabled = False

                If Me.Rpredefined.Checked = True Then
                    If Me.oportfolios.portfolios(Me.Lpredefined.SelectedItems(0).Index).enable_years_periods = True Then
                        Me.Cyearstart.Enabled = True
                    End If
                End If
                For i = 0 To Me.item_sel_type.Count - 1
                    If Me.item_sel_type(i) = "period" Then
                        Me.Cyearstart.Enabled = True
                        Me.cyearend.Enabled = True
                        Me.baddper.Enabled = True
                        Me.bdelper.Enabled = True
                        Me.lperiods.Enabled = True
                        Me.cperiods.Enabled = True

                        Exit For
                    End If
                Next
            End If

            Me.bok.Visible = False
            Me.c_show_ea.Enabled = True
            If Me.twizard.SelectedIndex = 5 And Me.twizard.TabPages(5).Enabled = True Then
                Me.bok.Visible = False
                Me.bforward.Visible = False
                Me.bokdisabled.Visible = True
                If Me.lselentity.Items.Count > 0 And Me.lselitem.Items.Count > 0 Then
                    If Me.Rpredefined.Checked = False Or Me.clink.SelectedIndex = 0 Then
                        Me.bok.Visible = True
                        Me.bforward.Visible = False
                        Me.bokdisabled.Visible = False
                    Else
                        If Me.Lselass.Items.Count > 0 Then
                            Me.bok.Visible = True
                            Me.bforward.Visible = False
                            Me.bokdisabled.Visible = False
                        End If
                    End If
                End If
                Me.c_show_ea.Enabled = True
                If Me.Cyearstart.Enabled = False Then
                    Me.c_show_ea.Checked = False
                    Me.c_show_ea.Enabled = False
                End If

                If Me.lselentity.Items.Count = 1 Then
                    Me.c_show_ea.Enabled = False
                    If Me.Cyearstart.Enabled = True Then
                        Me.c_show_ea.Checked = True
                    Else
                        Me.c_show_ea.Checked = False
                    End If

                End If
            End If
            If Me.twizard.SelectedIndex = 5 And Me.rcurrconv.Visible = True And Me.Rpredefined.Checked = True Then
                Me.bok.Visible = False
                Me.bokdisabled.Visible = True
                If Me.ccurr.SelectedIndex > -1 Then
                    Me.bok.Visible = True
                    Me.bokdisabled.Visible = False
                End If
            End If
        Catch ex As Exception
            'Dim oerr As New bc_cs_error_log("twizard", "selectedindexchanged", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub TabPage1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabPage1.Click

    End Sub

    Private Sub lselentity_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lselentity.SelectedIndexChanged
        Me.brement.Enabled = False
        Me.bentup.Enabled = False
        Me.bentdn.Enabled = False
        If Me.lselentity.SelectedIndex > -1 And Me.rchoose.Checked = True Then
            Me.brement.Enabled = True
            Me.bentup.Visible = True
            Me.bentdn.Visible = True
            If Me.lselentity.SelectedIndex > 0 And Me.lselentity.SelectedIndex < Me.lselentity.Items.Count - 1 Then
                Me.bentup.Enabled = True
                Me.bentdn.Enabled = True
            ElseIf Me.lselentity.SelectedIndex > 0 Then
                Me.bentup.Enabled = True
            ElseIf Me.lselentity.Items.Count > 1 Then
                Me.bentdn.Enabled = True
            End If
        End If
    End Sub

    Private Sub Button4_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub ctemplate_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ctemplate.SelectedIndexChanged
        Dim i As Integer
        If Me.ctemplate.SelectedIndex = -1 Then
            Exit Sub
        End If
        Me.bselent.Enabled = False

        Me.lallitem.Items.Clear()
        Me.csection.Visible = True
        Me.lsection.Visible = True
        'Me.ccontext.Visible = True
        'Me.lcontext.Visible = True
        Me.ccontext.Items.Clear()
        Me.ccontext.Items.Add("Excel Submission")
        Me.ccontext.Items.Add("Calculated")
        Me.ccontext.SelectedIndex = 0

        REM now add template specitif contexts
        Dim tid As Long
        For i = 0 To bc_am_ef_functions.template_names.Count - 1
            If bc_am_ef_functions.template_names(i) = Me.ctemplate.Text Then
                tid = bc_am_ef_functions.template_ids(i)
                Exit For
            End If

        Next
        For i = 0 To oseltemplates.context_names.Count - 1
            If oseltemplates.context_templates(i) = tid Then
                Me.ccontext.Items.Add(oseltemplates.context_names(i))
            End If
        Next

    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bselitem.Click
        set_item()
        Me.lallitem.SelectedIndex = -1
    End Sub
    Private Sub set_item()

        Dim i As Integer
        If Me.lallitem.SelectedIndex = -1 Then
            Exit Sub
        End If
        Dim attempts As Integer = 0
        While Me.lallitem.SelectedIndex > -1 And attempts < MAX_ATTEMPTS

            REM if predefined then check if limit exceeded
            If Me.Rpredefined.Checked = True Then
                If Me.oportfolios.portfolios(Me.Lpredefined.SelectedItems(0).Index).num_items_select > 0 Then
                    If Me.oportfolios.portfolios(Me.Lpredefined.SelectedItems(0).Index).num_items_select = Me.lselitem.Items.Count Then
                        Dim omsg As New bc_cs_message("Blue Curve", "You can only set: " + CStr(Me.oportfolios.portfolios(Me.Lpredefined.SelectedItems(0).Index).num_items_select) + " items(s) for this pre defined extract", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                        Exit Sub
                    End If
                End If
            End If


            Me.lselentity.Sorted = False

            Me.titemset.Enabled = True
            Me.tx_u = ""
            Me.lselitem.Items.Add(Me.lallitem.Text)
            REM list is sorted so check it againts array
            For i = 0 To Me.filter_item_ids.Count - 1
                If Me.filter_item_ids(i) = Me.lallitem.Text Then
                    Me.item_sel_type.Add(Me.filter_item_types(i))
                    Me.item_sel_factor.Add(Me.filter_item_factor(i))
                    Me.item_sel_monatary.Add(Me.filter_item_monatary(i))
                    Me.item_sel_symbol.Add(Me.filter_item_symbol(i))
                    Me.item_sel_assoc_class.Add(Me.filter_item_assoc_class(i))
                    Exit For
                End If
            Next

            'allred
            Me.lallitem.Items.RemoveAt(Me.lallitem.SelectedIndex)
            attempts += 1

            set_forward_enabled()
            Me.bok.Visible = False

        End While

        Me.twizard.TabPages(4).Enabled = True
        Me.twizard.TabPages(5).Enabled = True
        Me.titemset.Text = ""
        set_desc()

    End Sub

    Private Sub rnow_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rnow.CheckedChanged
        If rnow.Checked = True Then
            Me.DateTimePicker1.Visible = False
            Me.tx_filt = criteria_tx()
            set_desc()
        Else
            Me.DateTimePicker1.Visible = True
        End If
        set_desc()
    End Sub

    Private Sub currency_convert(ByRef oform As formula, Optional ByVal linked_header_values As Boolean = True, Optional ByVal multiple_entity As Boolean = True, Optional ByVal time_series As Boolean = False)
        Dim fn As String
        Dim cfn As String
        REM get base formula
        If multiple_entity = True Then
            build_function_multiple(oform, linked_header_values)
        Else
            build_function_single(oform, linked_header_values, False, False, False, time_series, False)
        End If
        fn = oform.formula
        REM now make currency formula
        If multiple_entity = True Then
            cfn = build_function_multiple(oform, linked_header_values, True)
        Else
            cfn = build_function_single(oform, linked_header_values, True, False, False, time_series, False)
        End If
        fn = fn.Substring(1, fn.Length - 1)
        cfn = cfn.Replace("""VALUE""", fn)
        oform.formula = cfn

    End Sub

    Private Function build_function_multiple(ByRef oform As formula, Optional ByVal linked_header_values As Boolean = True, Optional ByVal currency_conv As Boolean = False, Optional ByVal item_currency As Boolean = False, Optional ByVal e_a As Boolean = False) As String
        Dim i, j As Integer
        Dim fm As String
        Dim dims As String
        Dim ti As String
        Dim sti As String
        Dim eti As String
        eti = ""
        sti = ""
        fm = ""
        dims = ""
        For i = 0 To bc_am_ef_functions.excel_functions.Count - 1
            If (bc_am_ef_functions.excel_functions(i).display_name = Me.cfunction.Text And currency_conv = False) Or (currency_conv = True And bc_am_ef_functions.excel_functions(i).display_name = "currency convert" And item_currency = False) Or (item_currency = True And bc_am_ef_functions.excel_functions(i).display_name = "currency convert" And currency_conv = True) Then
                fm = "=" + bc_am_ef_functions.excel_functions(i).name + "("
                For j = 0 To bc_am_ef_functions.excel_functions(i).params.count - 1
                    ti = ""
                    sti = ""
                    eti = ""

                    If bc_am_ef_functions.excel_functions(i).params(j).name = "context" Then
                        ti = Me.ccontext.Text
                    End If
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "section" Then
                        ti = Me.csection.Text
                    End If

                    If bc_am_ef_functions.excel_functions(i).params(j).name = "class" Then
                        If oform.assoc_entity = False Then
                            ti = excel_address(oform.formula_entity.header.item_row_offset + 1, oform.formula_entity.col_offset, True, True)
                        Else
                            ti = excel_address(oform.formula_entity.header.item_row_offset + 1, oform.formula_entity.col_offset + 1, True, True)
                        End If
                    End If
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "entity" Then
                        If oform.assoc_entity = False Then
                            ti = excel_address(oform.row_offset, oform.formula_entity.col_offset, False, True)
                        Else
                            ti = excel_address(oform.row_offset, oform.formula_entity.col_offset + 1, False, True)
                        End If
                    End If

                    If bc_am_ef_functions.excel_functions(i).params(j).name = "item" Then
                        If item_currency = False And e_a = False Then
                            ti = excel_address(oform.formula_entity.header.item_row_offset + 5, oform.col_offset, True, False)
                        ElseIf item_currency = False Or e_a = False Then
                            ti = excel_address(oform.formula_entity.header.item_row_offset + 5, oform.col_offset - 1, True, False)
                        ElseIf item_currency = True And e_a = True Then

                            ti = excel_address(oform.formula_entity.header.item_row_offset + 5, oform.col_offset - 2, True, False)
                        End If
                    End If
                    REM cusbstiute value
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "gen1" Then
                        If currency_conv = True And item_currency = False Then
                            ti = "VALUE"
                        ElseIf item_currency = True And e_a = False Then

                            ti = excel_address(oform.formula_entity.row_offset, oform.col_offset - 1)
                        Else
                            ti = excel_address(oform.formula_entity.row_offset, oform.col_offset - 2)


                        End If
                    End If
                    REM currency code convert
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "gen2" Then
                        If currency_conv = True And item_currency = False Then
                            If linked_header_values = True Then
                                ti = excel_address(oform.formula_entity.header.row_offset + 3, oform.formula_entity.col_offset + 1)
                            Else
                                ti = oform.formula_entity.header.currency_code
                            End If
                        Else
                            REM item currency
                            REM just use a randon one
                            ti = Me.ccurr.Items(0)
                        End If
                    End If


                    If bc_am_ef_functions.excel_functions(i).params(j).name = "contributor" Then
                        If linked_header_values = True Then
                            ti = excel_address(oform.formula_entity.header.row_offset + 1, oform.formula_entity.col_offset + 1, True, True)
                        Else
                            ti = oform.formula_entity.header.contributor
                        End If

                    End If
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "stage" Then
                        If linked_header_values = True Then
                            ti = excel_address(oform.formula_entity.header.row_offset, oform.formula_entity.col_offset + 1, True, True)
                        Else
                            ti = oform.formula_entity.header.stage
                        End If
                    End If
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "year" Then
                        If e_a = False Then
                            ti = excel_address(oform.formula_entity.header.item_row_offset + 3, oform.col_offset, True, False)
                        Else
                            ti = excel_address(oform.formula_entity.header.item_row_offset + 3, oform.col_offset - 1, True, False)

                        End If
                    End If
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "period" Then
                        If e_a = False Then
                            ti = excel_address(oform.formula_entity.header.item_row_offset + 4, oform.col_offset, True, False)
                        Else
                            ti = excel_address(oform.formula_entity.header.item_row_offset + 4, oform.col_offset - 1, True, False)

                        End If
                    End If

                    If bc_am_ef_functions.excel_functions(i).params(j).name = "date_at" Then
                        If linked_header_values = True Then
                            ti = excel_address(oform.formula_entity.header.row_offset + 2, oform.formula_entity.col_offset + 1, True, True)
                        Else
                            ti = oform.formula_entity.header.date_at
                        End If
                    End If
                    'If bc_am_ef_functions.excel_functions(i).params(j).name = "date_from" Then
                    '    If Me.chkdate_from.Checked = True Then
                    '        ti = "1-1-1900"
                    '    Else
                    '        ti = CStr(Me.tdate_from.Text)
                    '    End If
                    'End If
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
                    End If
                Next
                '        Exit For
            End If
        Next

        fm = fm + ")"
        oform.formula = fm
        build_function_multiple = fm
    End Function
    Private Function build_function_single(ByRef oform As formula, Optional ByVal linked_header_values As Boolean = True, Optional ByVal currency_conv As Boolean = False, Optional ByVal item_currency As Boolean = False, Optional ByVal e_a As Boolean = False, Optional ByVal time_series As Boolean = False, Optional ByVal time_series_first As Boolean = False) As String
        Dim i, j As Integer
        Dim fm As String
        Dim dims As String
        Dim ti As String
        Dim sti As String
        Dim eti As String
        eti = ""
        sti = ""
        fm = ""
        dims = ""
        For i = 0 To bc_am_ef_functions.excel_functions.Count - 1
            If (bc_am_ef_functions.excel_functions(i).display_name = Me.cfunction.Text And currency_conv = False) Or (currency_conv = True And bc_am_ef_functions.excel_functions(i).display_name = "currency convert" And item_currency = False) Or (item_currency = True And bc_am_ef_functions.excel_functions(i).display_name = "currency convert" And currency_conv = True) Then
                fm = "=" + bc_am_ef_functions.excel_functions(i).name + "("
                For j = 0 To bc_am_ef_functions.excel_functions(i).params.count - 1
                    ti = ""

                    sti = ""
                    eti = ""

                    If bc_am_ef_functions.excel_functions(i).params(j).name = "context" Then
                        ti = Me.ccontext.Text
                    End If
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "section" Then
                        ti = Me.csection.Text
                    End If

                    If bc_am_ef_functions.excel_functions(i).params(j).name = "class" Then
                        If oform.assoc_entity = False Then
                            ti = excel_address(oform.formula_entity.header.item_row_offset + 1, oform.formula_entity.col_offset, True, True)
                        Else
                            ti = excel_address(oform.formula_entity.header.item_row_offset + 2, oform.formula_entity.col_offset, True, True)
                        End If
                    End If
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "entity" Then
                        If oform.assoc_entity = False Then
                            ti = excel_address(oform.formula_entity.header.item_row_offset + 1, oform.formula_entity.col_offset + 1, True, True)
                        Else
                            ti = excel_address(oform.formula_entity.header.item_row_offset + 2, oform.formula_entity.col_offset + 1, True, True)
                        End If
                    End If

                    If bc_am_ef_functions.excel_functions(i).params(j).name = "item" Then
                        If time_series = False Then
                            If e_a = False Or item_currency = True Then
                                ti = excel_address(oform.row_offset, oform.formula_entity.col_offset + 3, False, True)
                            Else
                                ti = excel_address(oform.formula_entity.row_offset, oform.formula_entity.col_offset + 3)
                            End If
                        Else
                            If time_series_first = True Then
                                ti = excel_address(oform.row_offset - 5, oform.formula_entity.col_offset + 3)
                            Else
                                If item_currency = False Then
                                    ti = excel_address(oform.formula_entity.row_offset - 7, oform.col_offset)
                                Else
                                    ti = excel_address(oform.formula_entity.row_offset - 6, oform.col_offset)
                                End If
                            End If
                        End If
                    End If
                    REM cusbstiute value
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "gen1" Then
                        If currency_conv = True And item_currency = False Then
                            ti = "VALUE"
                        Else
                            If time_series = False Then
                                ti = excel_address(oform.row_offset, oform.formula_entity.header.col_offset + 4)
                            Else
                                ti = excel_address(oform.row_offset + 1, oform.col_offset)
                            End If
                        End If
                    End If
                    REM currency code convert
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "gen2" Then
                        If currency_conv = True And item_currency = False Then
                            ti = excel_address(oform.formula_entity.header.row_offset + 3, oform.formula_entity.col_offset + 1)
                        Else
                            REM item currency
                            REM just use a randon one
                            ti = Me.ccurr.Items(0)
                        End If
                    End If


                    If bc_am_ef_functions.excel_functions(i).params(j).name = "contributor" Then
                        ti = excel_address(oform.formula_entity.header.row_offset + 1, oform.formula_entity.col_offset + 1, True, True)
                    End If
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "stage" Then
                        ti = excel_address(oform.formula_entity.header.row_offset, oform.formula_entity.col_offset + 1, True, True)
                    End If
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "year" Then
                        If time_series = False Then
                            ti = excel_address(oform.formula_entity.header.item_row_offset + 2, oform.col_offset, True, False)
                        Else

                            ti = excel_address(oform.formula_entity.header.item_row_offset + 1, oform.col_offset)
                        End If
                    End If

                    If bc_am_ef_functions.excel_functions(i).params(j).name = "period" Then
                        If time_series = False Then
                            ti = excel_address(oform.formula_entity.header.item_row_offset + 3, oform.col_offset, True, False)
                        Else
                            ti = excel_address(oform.formula_entity.header.item_row_offset + 2, oform.col_offset)
                        End If
                    End If

                    If bc_am_ef_functions.excel_functions(i).params(j).name = "date_at" Then
                        If time_series = False Then
                            ti = excel_address(oform.formula_entity.header.row_offset + 2, oform.formula_entity.col_offset + 1, True, True)
                        Else
                            If time_series_first = True Then
                                ti = excel_address(oform.formula_entity.header.row_offset + 2, oform.formula_entity.col_offset + 2)
                            Else
                                If bc_am_ef_functions.excel_functions(i).display_name = "currency convert" Then
                                    REM always currency convert at current exhange rate for time series
                                    ti = "9-9-9999"
                                Else
                                    ti = excel_address(oform.row_offset, oform.formula_entity.col_offset)
                                End If
                            End If
                        End If
                    End If
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "date_from" Then
                        ti = excel_address(oform.formula_entity.header.row_offset + 2, oform.formula_entity.col_offset + 1)
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
                    End If
                Next
                '        Exit For
            End If
        Next

        fm = fm + ")"
        oform.formula = fm
        build_function_single = fm
    End Function
    Public Function is_formula(ByVal fm As String) As Boolean
        Try
            Dim i As Integer
            Dim now_numeric As Boolean = False
            is_formula = False
            REM if has a $ it is a formula
            If fm.Length < 2 Then
                Exit Function
            End If
            Dim k As Integer
            If InStr(fm, "$") > 0 Then
                is_formula = True
                Exit Function
            End If
            REM if has alpha chracter followed by numeric it is as well
            REM first character must be alpha
            If (Asc(fm.Substring(0, 1)) >= Asc("a") And Asc(fm.Substring(0, 1)) <= Asc("z")) Or (Asc(fm.Substring(0, 1)) >= Asc("A") And Asc(fm.Substring(0, 1)) <= Asc("Z")) Then
                For i = 1 To fm.Length - 1
                    If (Asc(fm.Substring(i, 1)) >= Asc("a") And Asc(fm.Substring(i, 1)) <= Asc("z")) Or (Asc(fm.Substring(i, 1)) >= Asc("A") And Asc(fm.Substring(i, 1)) <= Asc("Z")) Then
                        k = 4
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
            If Me.lselitem.Items.Count = 1 Then
                Me.titemset.Enabled = False
                Me.titemset.Text = ""

            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_ef_wizard", "is_formula", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Function

    Private Sub rcell_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rcell.CheckedChanged

    End Sub


    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bdelitem.Click
        rem_item()
    End Sub
    Private Sub rem_item()
        If Me.lselitem.SelectedIndex = -1 Then
            Exit Sub
        End If
        Dim attempts As Integer = 0
        While Me.lselitem.SelectedIndex > -1 And attempts < MAX_ATTEMPTS

            'allred
            Me.lallitem.Items.Add(Me.lselitem.Text)

            Me.item_sel_type.RemoveAt(Me.lselitem.SelectedIndex)
            Me.item_sel_factor.RemoveAt(Me.lselitem.SelectedIndex)
            Me.item_sel_monatary.RemoveAt(Me.lselitem.SelectedIndex)
            Me.item_sel_symbol.RemoveAt(Me.lselitem.SelectedIndex)
            Me.item_sel_assoc_class.RemoveAt(Me.lselitem.SelectedIndex)
            Me.lselitem.Items.RemoveAt(Me.lselitem.SelectedIndex)
            Me.tx_u = ""

            set_desc()
            If Me.lselitem.Items.Count = 0 Then
                Me.titemset.Text = ""
                Me.titemset.Enabled = False
            End If

            attempts += 1

        End While

    End Sub

    Private Function criteria_tx() As String
        Try
            Dim tx As String
            Dim p As String = ""
            Dim i As Integer
            If Me.cfunction.SelectedIndex = 0 Then

                If Me.rnow.Checked = True Then
                    tx = "I would like  to extract " + Me.cstage.Text + " current data"
                Else
                    tx = "I would like  to extract " + Me.cstage.Text + "  data from " + Format(Me.DateTimePicker1.Value, "dd-MMM-yyyy")
                End If
            Else
                tx = "I would like  to extract " + Me.cstage.Text
                If Me.rold.Checked = True Then
                    tx = tx + " oldest data"
                Else
                    tx = tx + " data from " + Format(Me.DateTimePicker2.Value, "dd-MMM-yyyy")
                End If

                If Me.rnow.Checked = True Then
                    tx = tx + " to current data"
                Else
                    tx = tx + " to " + Format(Me.DateTimePicker1.Value, "dd-MMM-yyyy")
                End If

            End If
            tx = tx + " from " + Me.Ccont.Text
            If Me.Cyearstart.Enabled = True Then
                For i = 0 To Me.lperiods.Items.Count - 1
                    If i = 0 Then
                        p = Me.lperiods.Items(i)
                    Else
                        p = p + "; " + Me.lperiods.Items(i)
                    End If
                Next
                If Me.lperiods.Items.Count = 1 Then
                    tx = tx + " for years " + Me.Cyearstart.Text + " to " + Me.cyearend.Text + " period " + p
                Else
                    tx = tx + " for years " + Me.Cyearstart.Text + " to " + Me.cyearend.Text + " periods " + p
                End If
            End If

            criteria_tx = tx
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_portfolio_wizard", "criteria_tx", bc_cs_error_codes.USER_DEFINED, ex.Message)
            criteria_tx = ""

        End Try
    End Function

    Private Sub cstage_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cstage.SelectedIndexChanged
        Me.tx_filt = criteria_tx()
        set_desc()
    End Sub
    Private Sub Ccont_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Ccont.SelectedIndexChanged
        Me.tx_filt = criteria_tx()
        If Me.rnew.Checked = True Then
            Me.tsource.Text = "Source: " + Me.Ccont.Text
        End If
        set_desc()
    End Sub

    Private Sub DateTimePicker1_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateTimePicker1.ValueChanged
        Me.tx_filt = criteria_tx()
        set_desc()
    End Sub

    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bup.Click
        If lselitem.SelectedItems.Count <= 1 Then
            Dim tx As String
            Dim ty As String
            Dim tz As String
            Dim ta As String
            Dim tb As String
            Dim tc As String
            Dim i As Integer
            If Me.lselitem.SelectedIndex > 0 Then
                i = Me.lselitem.SelectedIndex
                tx = Me.lselitem.Items(Me.lselitem.SelectedIndex)
                ty = Me.item_sel_type(Me.lselitem.SelectedIndex)
                tz = Me.item_sel_factor(Me.lselitem.SelectedIndex)
                ta = Me.item_sel_monatary(Me.lselitem.SelectedIndex)
                tb = Me.item_sel_symbol(Me.lselitem.SelectedIndex)
                tc = Me.item_sel_assoc_class(Me.lselitem.SelectedIndex)
                Me.lselitem.Items.RemoveAt(i)
                Me.item_sel_type.RemoveAt(i)
                Me.item_sel_factor.RemoveAt(i)
                Me.item_sel_monatary.RemoveAt(i)
                Me.item_sel_symbol.RemoveAt(i)
                Me.item_sel_assoc_class.RemoveAt(i)
                Me.item_sel_type.Insert(i - 1, ty)
                Me.item_sel_factor.Insert(i - 1, tz)
                Me.item_sel_monatary.Insert(i - 1, ta)
                Me.item_sel_symbol.Insert(i - 1, tb)
                Me.item_sel_assoc_class.Insert(i - 1, tc)
                Me.lselitem.Items.Insert(i - 1, tx)
            End If
            Me.bup.Enabled = False
            Me.bdn.Enabled = False
            set_desc()
        End If
    End Sub

    Private Sub lselitem_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lselitem.SelectedIndexChanged
        Me.bup.Enabled = False
        Me.bdn.Enabled = False
        If Me.lselitem.SelectedIndex > 0 Then
            Me.bup.Enabled = True
        End If
        If Me.lselitem.SelectedIndex < Me.lselitem.Items.Count - 1 Then
            Me.bdn.Enabled = True
        End If
        Me.bdelitem.Enabled = False
        If Me.lselitem.SelectedIndex > -1 Then
            Me.bdelitem.Enabled = True
        End If


    End Sub

    Private Sub bdn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bdn.Click
        If lselitem.SelectedItems.Count <= 1 Then
            Dim tx As String
            Dim ty As String
            Dim tz As String
            Dim ta As String
            Dim tb As String
            Dim tc As String
            Dim i As Integer
            If Me.lselitem.SelectedIndex = -1 Then
                Exit Sub
            End If
            If Me.lselitem.SelectedIndex < Me.lselitem.Items.Count - 1 Then
                i = Me.lselitem.SelectedIndex
                tx = Me.lselitem.Items(Me.lselitem.SelectedIndex)
                ty = Me.item_sel_type(Me.lselitem.SelectedIndex)
                tz = Me.item_sel_factor(Me.lselitem.SelectedIndex)
                ta = Me.item_sel_monatary(Me.lselitem.SelectedIndex)
                tb = Me.item_sel_symbol(Me.lselitem.SelectedIndex)
                tc = Me.item_sel_assoc_class(Me.lselitem.SelectedIndex)

                Me.lselitem.Items.RemoveAt(i)
                Me.item_sel_type.RemoveAt(i)
                Me.item_sel_factor.RemoveAt(i)
                Me.item_sel_monatary.RemoveAt(i)
                Me.item_sel_symbol.RemoveAt(i)
                Me.item_sel_assoc_class.RemoveAt(i)
                Me.item_sel_type.Insert(i + 1, ty)
                Me.item_sel_factor.Insert(i + 1, tz)
                Me.item_sel_monatary.Insert(i + 1, ta)
                Me.item_sel_symbol.Insert(i + 1, tb)
                Me.item_sel_assoc_class.Insert(i + 1, tc)
                Me.lselitem.Items.Insert(i + 1, tx)
            End If
            Me.bup.Enabled = False
            Me.bdn.Enabled = False
            set_desc()
        End If
    End Sub

    Private Sub rcurrconv_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rcurrconv.CheckedChanged
        Me.ccurr.SelectedIndex = -1
        Me.ccurr.Enabled = False
        Me.bokdisabled.Visible = True
        If Me.rcurrconv.Checked = True Then
            Me.ccurr.SelectedIndex = -1
            Me.ccurr.Enabled = True
            Me.bok.Visible = False
        End If
    End Sub

    Private Sub ccurr_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ccurr.SelectedIndexChanged
        If Me.ccurr.SelectedIndex > -1 Then
            Me.bok.Visible = True
            set_desc()
        End If
    End Sub

    Private Sub tname_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tname.TextChanged
        If Trim(tname.Text) <> "" Then
            REM check it doesnt exist already
            If Me.rnew.Checked = True Then
                Me.ttitle.Text = Me.tname.Text
            End If

            Me.twizard.TabPages(1).Enabled = True
            set_forward_enabled()
            Me.bok.Visible = False
            Me.Rnosave.Text = "do not save extract " + Me.tname.Text
            Me.Rprivatesave.Text = "save extact " + Me.tname.Text + " for private use only"
            Me.rglobalsave.Text = "save extract " + Me.tname.Text + " for global use"
            Me.Text = "Data Extraction Tool - Blue Curve (" + Me.tname.Text + ")"
            set_desc()
            Dim dp As System.Drawing.Point
            dp.Y = 8
            dp.X = 408
            Me.bcancel.Location = dp
            Me.bcanceldisabled.Location = dp

        Else
            Dim dp As System.Drawing.Point
            dp.Y = 8
            dp.X = 472
            Me.bcancel.Location = dp
            Me.bcanceldisabled.Location = dp

            Me.bforward.Visible = False
        End If
    End Sub

    Private Sub Panel2_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Panel2.Paint

    End Sub

    Private Sub baddper_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles baddper.Click

        add_period()
    End Sub
    Private Sub add_period()
        If Me.cperiods.SelectedIndex > -1 Then

            Me.lperiods.Items.Add(Me.cperiods.Text)
            Me.cperiods.Items.RemoveAt(Me.cperiods.SelectedIndex)
            set_desc()

        End If
    End Sub

    Private Sub bdelper_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bdelper.Click
        del_period()
    End Sub
    Private Sub del_period()

        If Me.lperiods.SelectedIndex > -1 Then

            Me.cperiods.Items.Add(Me.lperiods.Text)
            Me.lperiods.Items.RemoveAt(Me.lperiods.SelectedIndex)
            set_desc()
        End If
    End Sub
    Private Sub load_entity_sets()
        Dim i As Integer

        user_entity_sets = New bc_om_ef_entity_sets
        user_entity_sets.user_id = bc_cs_central_settings.logged_on_user_id
        user_entity_sets.class_name = Me.cclass.Text
        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
            user_entity_sets.db_read()
        Else
            user_entity_sets.tmode = bc_cs_soap_base_class.tREAD
            If user_entity_sets.transmit_to_server_and_receive(user_entity_sets, True) = False Then
                Exit Sub
            End If
        End If
        Me.centityset.Items.Clear()
        Me.centityset.Enabled = False
        For i = 0 To user_entity_sets.entity_sets.Count - 1
            Me.centityset.Items.Add(user_entity_sets.entity_sets(i).name)
            Me.centityset.Enabled = True
        Next
        Me.centityset.SelectedIndex = -1

    End Sub
    Private Sub rportfolio_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rentity_set.CheckedChanged

        Try
            Me.Bdel.Visible = False
            Me.centityset.Visible = False
            Me.ritemset.Checked = True

            Me.bentup.Visible = False
            Me.bentdn.Visible = False


            If Me.rentity_set.Checked = True Then
                Me.tentname.Visible = False
                Me.bsaveent.Visible = False

                Me.Lfilter.Visible = False
                Me.Cfclass.Visible = False
                Me.Cfentity.Visible = False
                Me.lselentity.Sorted = False
                Me.lselentity.Enabled = False
                Me.Cursor = Cursors.WaitCursor
                Me.tentname.Visible = False
                Me.bsaveent.Visible = False
                Me.lallentity.Visible = False
                Me.lselentity.Visible = True
                Me.lselentity.Width = 480
                Me.lselentity.SelectionMode = SelectionMode.MultiSimple
                Me.lselentity.Items.Clear()
                Me.lselentity.Location = Me.lallentity.Location
                Me.Bdel.Visible = True
                Me.centityset.Visible = True
                Me.centityset.Items.Clear()
                Me.Bdel.Enabled = False
                load_entity_sets()
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("rentity_set", "checked", bc_cs_error_codes.USER_DEFINED, ex.Message)



        Finally
            Me.Cursor = Cursors.Default

        End Try
    End Sub

    Private Sub cportolfio_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles centityset.SelectedIndexChanged
        Dim i As Integer
        If Me.centityset.SelectedIndex > -1 Then
            Me.tx_c = "for all " + Me.cclass.Text + "'s" + " in user set " + Me.centityset.Text
            Me.lselentity.Enabled = True
            Me.Bdel.Enabled = True
            Me.lselentity.Items.Clear()
            For i = 0 To Me.user_entity_sets.entity_sets(Me.centityset.SelectedIndex).entities.count - 1
                Me.lselentity.Items.Add(Me.user_entity_sets.entity_sets(Me.centityset.SelectedIndex).entities(i))
                set_forward_enabled()
                Me.bok.Visible = False

            Next
            If Me.lselentity.Items.Count > 0 Then
                Me.twizard.TabPages(3).Enabled = True
            End If
            set_desc()
        End If
    End Sub


    Private Sub load_user_defined_portfolios()
        Try
            Dim i As Integer
            Dim lport As ListViewItem
            If Me.loading = True Then
                Exit Sub
            End If
            Me.lportfolios.Items.Clear()
            Me.Cursor = Cursors.WaitCursor
            oportfolios.user_id = bc_cs_central_settings.logged_on_user_id
            oportfolios.predefined = False
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                oportfolios.db_read()
            Else
                oportfolios.tmode = bc_cs_soap_base_class.tREAD
                If oportfolios.transmit_to_server_and_receive(oportfolios, True) = False Then
                    Exit Sub
                End If
            End If
            For i = 0 To oportfolios.portfolios.Count - 1
                lport = New ListViewItem(CStr(oportfolios.portfolios(i).title))
                lport.SubItems.Add(Format(oportfolios.portfolios(i).create_date, "dd-MMM-yyyy"))
                If oportfolios.portfolios(i).user_id = bc_cs_central_settings.logged_on_user_id Then
                    lport.SubItems.Add("me")
                Else
                    lport.SubItems.Add(oportfolios.portfolios(i).user_name)
                End If
                lport.SubItems.Add(CStr(oportfolios.portfolios(i).portfolio_id))

                lport.SubItems.Add(CStr(oportfolios.portfolios(i).function_name))
                If Me.Rprivate.Checked = True Then
                    If oportfolios.portfolios(i).public_flag = False Then
                        Me.lportfolios.Items.Add(lport)
                    End If
                ElseIf Me.Rprivate.Checked = False Then

                    If oportfolios.portfolios(i).public_flag = True Then
                        Me.lportfolios.Items.Add(lport)
                    End If
                End If
            Next
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub
    Private Sub load_predefined_portfolios()
        Try
            Dim i As Integer
            Dim lport As ListViewItem
            If Me.loading = True Then
                Exit Sub
            End If
            Me.Lpredefined.Items.Clear()
            Me.Cursor = Cursors.WaitCursor
            oportfolios.user_id = bc_cs_central_settings.logged_on_user_id
            oportfolios.predefined = True
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                oportfolios.db_read()
            Else
                oportfolios.tmode = bc_cs_soap_base_class.tREAD
                If oportfolios.transmit_to_server_and_receive(oportfolios, True) = False Then
                    Exit Sub
                End If
            End If
            For i = 0 To oportfolios.portfolios.Count - 1
                lport = New ListViewItem(CStr(oportfolios.portfolios(i).title))
                Me.Lpredefined.Items.Add(lport)
            Next
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_portfolio_wizard", "load_predefined_portfolios", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub
    Private Sub lportfolios_dblclick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles lportfolios.ColumnClick

        Select Case CInt(e.Column.ToString)
            Case 1
                REM date
                lportfolios.ListViewItemSorter() = New CompareBydate
                If CompareBydate.toggle = False Then
                    CompareBydate.toggle = True
                Else
                    CompareBydate.toggle = False
                End If
            Case 0
                REM item
                lportfolios.ListViewItemSorter() = New CompareByname
                If CompareByname.toggle = False Then
                    CompareByname.toggle = True
                Else
                    CompareByname.toggle = False
                End If
            Case 2
                REM item
                lportfolios.ListViewItemSorter() = New CompareByuser
                If CompareByuser.toggle = False Then
                    CompareByuser.toggle = True
                Else
                    CompareByuser.toggle = False
                End If
            Case 3
                REM type
                lportfolios.ListViewItemSorter() = New CompareBytype
                If CompareBytype.toggle = False Then
                    CompareBytype.toggle = True
                Else
                    CompareBytype.toggle = False
                End If
            Case 2
        End Select

    End Sub
    Private Sub ListView1_dblclick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lportfolios.DoubleClick
        open_user_defined_portfolio()
        Me.bopport.Enabled = False
        Me.twizard.SelectedIndex = 5


    End Sub

    Private Sub ListView1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lportfolios.SelectedIndexChanged
        Dim i As Integer

        Me.bdelport.Enabled = False
        If Me.lportfolios.SelectedItems.Count = 1 Then
            Me.bopport.Enabled = True
            REM can only delete a portfolio created by user
            For i = 0 To Me.oportfolios.portfolios.Count - 1
                If Me.oportfolios.portfolios(i).portfolio_id = CInt(Me.lportfolios.SelectedItems(0).SubItems(3).Text) Then
                    If Me.oportfolios.portfolios(i).user_id = bc_cs_central_settings.logged_on_user_id Then
                        Me.bdelport.Enabled = True
                    End If
                    Exit For
                End If
            Next
            open_user_defined_portfolio()
            'Why?
            Me.twizard.SelectedIndex = 5
            'Me.twizard.SelectedIndex = 0
            set_desc()
        End If
    End Sub

    Private Sub RadioButton2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Rprivate.CheckedChanged
        If Me.loading = True Then
            Exit Sub
        End If
        If Me.Rprivate.Checked = True Then
            Me.bopport.Enabled = False
            Me.bdelport.Enabled = False
            load_user_defined_portfolios()
        End If
    End Sub


    Private Sub rglobal_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rglobal.CheckedChanged
        If Me.rglobal.Checked = True Then
            Me.bopport.Enabled = False
            Me.bdelport.Enabled = False
            load_user_defined_portfolios()
        End If
    End Sub

    Private Sub bopport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bopport.Click
        open_user_defined_portfolio()
        Me.bopport.Enabled = False

        Me.twizard.SelectedIndex = 5

    End Sub
    Private Sub open_user_defined_portfolio()
        Try
            Dim i, j As Integer
            Dim pid As Long
            If Me.lportfolios.SelectedItems.Count <> 1 Then
                Exit Sub
            End If
            For i = 0 To Me.twizard.TabPages.Count - 1
                Me.twizard.TabPages(i).Enabled = True
            Next
            Me.Rprivatesave.Enabled = True
            Me.rglobalsave.Enabled = True
            If Me.ropen.Checked = True Then
                pid = CLng(Me.lportfolios.SelectedItems(0).SubItems(3).Text)
            Else
                pid = Me.oportfolios.portfolios(Me.Lpredefined.SelectedItems(0).Index)
            End If
            REM write out settings for selected portfoli to wizard
            For i = 0 To Me.oportfolios.portfolios.Count - 1
                If Me.oportfolios.portfolios(i).portfolio_id = pid Then
                    Me.selected_portfolio_id = Me.oportfolios.portfolios(i).portfolio_id

                    REM if portfolio was not created by user cannot update it back
                    If Me.oportfolios.portfolios(i).user_id <> bc_cs_central_settings.logged_on_user_id Then
                        Me.Rprivatesave.Enabled = False
                        Me.rglobalsave.Enabled = False
                    End If
                    Me.tname.Text = Me.oportfolios.portfolios(i).title
                    Me.Rnosave.Text = "do not save updated extract " + Me.tname.Text
                    Me.Rprivatesave.Text = "save updated extract " + Me.tname.Text + " for private use"
                    Me.rglobalsave.Text = "save updated extract " + Me.tname.Text + " for global use"
                    Me.c_show_ea.Checked = False

                    Me.cfunction.Text = Me.oportfolios.portfolios(i).function_name
                    Me.cclass.Text = Me.oportfolios.portfolios(i).class_name
                    Me.cschema.Text = Me.oportfolios.portfolios(i).schema_name
                    Me.lselentity.Items.Clear()
                    Me.lselentity.Sorted = True

                    Select Case Me.oportfolios.portfolios(i).universe_flag
                        Case 0
                            Me.rchoose.Checked = True
                            Me.lselentity.Items.Clear()
                            Me.lselentity.Sorted = False
                            For j = 0 To Me.oportfolios.portfolios(i).entities.count - 1
                                Me.lselentity.Items.Add(Me.oportfolios.portfolios(i).entities(j))
                                Dim iItems As Integer = 0
                                Do While iItems < lallentity.Items.Count
                                    Dim strItem As String = lallentity.Items(iItems)
                                    If strItem = Me.oportfolios.portfolios(i).entities(j) Then                                        
                                        lallentity.Items.Remove(strItem)
                                    Else
                                        iItems += 1
                                    End If
                                Loop                                
                            Next
                        Case 1
                            Me.rentity_set.Checked = True
                            Me.lselentity.Sorted = False
                            Me.centityset.SelectedIndex = -1
                            Me.centityset.Text = Me.oportfolios.portfolios(i).entity_set_name


                        Case 2
                            Me.rchoose.Checked = True
                            Me.rclass.Checked = True
                            Me.cclass2.Text = Me.oportfolios.portfolios(i).entity_prop_class_name

                            Me.centity.Text = Me.oportfolios.portfolios(i).entity_prop_entity_name
                            If Me.oportfolios.portfolios(i).sec_entity_prop_class_name <> "" Then
                                Me.cand.Checked = True
                                Me.cclass3.Text = Me.oportfolios.portfolios(i).sec_entity_prop_class_name
                                Me.centity2.Text = Me.oportfolios.portfolios(i).sec_entity_prop_entity_name
                            End If
                        Case 3
                            Me.rchoose.Checked = True
                            Me.rall.Checked = True
                    End Select
                    If Me.cfunction.SelectedIndex = 1 Then
                        If Me.lselentity.Items.Count > 0 Then
                            Me.cleadenity.Text = Me.lselentity.Items(0)
                        End If
                    End If



                    Me.lselitem.Items.Clear()
                    Me.item_sel_type.Clear()
                    Me.item_sel_factor.Clear()
                    Me.item_sel_monatary.Clear()
                    Me.item_sel_symbol.Clear()
                    Me.item_sel_assoc_class.Clear()
                    For j = 0 To Me.oportfolios.portfolios(i).items.count - 1
                        Me.lselitem.Items.Add(Me.oportfolios.portfolios(i).items(j))
                        Dim iItems As Integer = 0
                        Do While iItems < lallitem.Items.Count
                            Dim strItem As String = lallitem.Items(iItems)
                            If strItem = Me.oportfolios.portfolios(i).items(j) Then
                                lallitem.Items.Remove(strItem)
                            Else
                                iItems += 1
                            End If
                        Loop
                        Me.item_sel_type.Add(Me.oportfolios.portfolios(i).item_types(j))
                        Me.item_sel_factor.Add(Me.oportfolios.portfolios(i).item_factor(j))
                        Me.item_sel_monatary.Add(Me.oportfolios.portfolios(i).item_monatary(j))
                        Me.item_sel_symbol.Add(Me.oportfolios.portfolios(i).item_symbol(j))
                        Me.item_sel_assoc_class.Add(Me.oportfolios.portfolios(i).item_assoc(j))
                    Next
                    REM set role based user settings
                    If bc_am_ef_functions.ousersettings.stages <> 0 Then
                        Me.cstage.Enabled = False
                        If bc_am_ef_functions.ousersettings.stages = 1 Then
                            Me.cstage.SelectedIndex = 0
                        Else
                            Me.cstage.SelectedIndex = 1
                        End If
                    Else
                        Me.cstage.Text = Me.oportfolios.portfolios(i).stage
                    End If
                    Me.Ccont.Text = Me.oportfolios.portfolios(i).contributor

                    If Me.oportfolios.portfolios(i).date_at = "9-9-9999" Then
                        Me.rnow.Checked = True
                    Else
                        Me.Rdate.Checked = True
                        Me.DateTimePicker1.Value = Me.oportfolios.portfolios(i).date_at
                    End If
                    If Me.oportfolios.portfolios(i).date_from = "1-1-1900" Then
                        Me.rold.Checked = True
                    Else
                        Me.rdatefrom.Checked = True
                        Me.DateTimePicker2.Value = Me.oportfolios.portfolios(i).date_from
                    End If
                    Me.ttitle.Text = Me.oportfolios.portfolios(i).table_title
                    Me.tsubtitle.Text = Me.oportfolios.portfolios(i).sub_title
                    Me.tsource.Text = Me.oportfolios.portfolios(i).source
                    Me.Cyearstart.Text = Me.oportfolios.portfolios(i).start_year
                    Me.cyearend.Text = Me.oportfolios.portfolios(i).end_year
                    Me.lperiods.Items.Clear()
                    For j = 0 To Me.oportfolios.portfolios(i).periods.count - 1
                        Me.lperiods.Items.Add(Me.oportfolios.portfolios(i).periods(j))
                    Next
                    Select Case Me.oportfolios.portfolios(i).convert_type
                        Case 0
                            Me.rcurrshow.Checked = True
                        Case 1
                            Me.rcurrnone.Checked = True

                        Case 2
                            Me.rcurrconv.Checked = True
                            Me.ccurr.Text = Me.oportfolios.portfolios(i).currency
                    End Select
                    If Me.oportfolios.portfolios(i).show_e_a = True Then
                        Me.c_show_ea.Checked = True
                    End If
                    If Me.oportfolios.portfolios(i).format_type = 0 Then
                        Me.cformat.Checked = False
                    ElseIf Me.oportfolios.portfolios(i).format_type = 1 Then
                        Me.cformat.Checked = True
                        Me.cprectype.SelectedIndex = 0
                        Me.cprec.Text = CStr(Me.oportfolios.portfolios(i).precision)
                    ElseIf Me.oportfolios.portfolios(i).format_type = 2 Then
                        Me.cformat.Checked = True
                        Me.cprectype.SelectedIndex = 1
                        Me.cprec.Text = CStr(Me.oportfolios.portfolios(i).precision)
                    End If
                    If Me.oportfolios.portfolios(i).associated_class <> "" Then
                        Me.clink.Text = Me.oportfolios.portfolios(i).associated_class
                    End If
                    Exit For
                End If

                set_desc()
            Next
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_potfolio_wizard", "open_potfolio", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try
    End Sub
    Private Sub pre_defined_controls(ByVal disable As Boolean)

        Me.rcell.Visible = True
        Me.Panel1.Visible = True
        Me.Panel2.Visible = True
        Me.pformat.Visible = True
        Me.pdatetype.Visible = False
        Me.Pyearenddate.Visible = False
        Me.Label16.Visible = True
        Me.c_show_ea.Visible = True
        Me.rcurrshow.Visible = True
        Me.rcurrnone.Visible = True
        Me.cformat.Visible = True
        Me.cprec.Visible = True
        Me.cprectype.Visible = True
        Me.rcurrnone.Checked = True

        If disable = True Then
            REM disable universe selection
            Me.bsaveent.Visible = False
            Me.tentname.Visible = False
            Me.tentname.Enabled = False
            Me.rchoose.Enabled = False
            Me.rentity_set.Enabled = False
            Me.rchoose.Enabled = False
            Me.rclass.Enabled = False
            Me.rall.Enabled = False
            Me.cclass2.Enabled = False
            Me.cclass3.Enabled = False
            Me.centity.Enabled = False
            Me.centity2.Enabled = False
            Me.cand.Enabled = False
            Me.cschema.Enabled = False
            Me.cclass.Enabled = False
            Me.clink.Enabled = False
            Me.tentname.Enabled = False
            Me.bsaveent.Enabled = False
            Me.ball.Enabled = False
            Me.ritemset.Enabled = False
            Me.rdefined.Enabled = False
            Me.rportitems.Enabled = False
            Me.titemset.Visible = False
            Me.bsaveitem.Visible = False
            Dim dp As System.Drawing.Point
            dp.X = 12
            dp.Y = 3
            Me.rcurrconv.Location = dp
            dp.X = 196
            dp.Y = 12
            Me.ccurr.Location = dp

        Else
            REM disable universe selection
            Me.bsaveent.Visible = True
            Me.tentname.Visible = True
            Me.tentname.Enabled = True
            Me.rchoose.Enabled = True
            Me.rclass.Enabled = True
            Me.rentity_set.Enabled = True
            Me.rchoose.Enabled = True
            Me.rall.Enabled = True
            Me.cclass2.Enabled = True
            Me.cclass3.Enabled = True
            Me.centity.Enabled = True
            Me.centity2.Enabled = True
            Me.cand.Enabled = True
            Me.cschema.Enabled = True
            Me.cclass.Enabled = True
            Me.clink.Enabled = True
            Me.tentname.Enabled = True
            Me.bsaveent.Enabled = True
            Me.ball.Enabled = True
            Me.ritemset.Enabled = True
            Me.rdefined.Enabled = True
            Me.rportitems.Enabled = True
            Me.titemset.Visible = True
            Me.bsaveitem.Visible = True
            Me.cstage.Enabled = True
            Me.Ccont.Enabled = True
            Dim dp As System.Drawing.Point
            dp.X = 227
            dp.Y = 45
            Me.rcurrconv.Location = dp
            dp.X = 411
            dp.Y = 54
            Me.ccurr.Location = dp
        End If
    End Sub


    Private Sub open_predefined_portfolio()
        Try
            Me.Cursor = Cursors.WaitCursor


            clear_down()

            Dim i, j As Integer
            Dim pid As Long
            If Me.Lpredefined.SelectedItems.Count <> 1 Then
                Exit Sub
            End If
            For i = 0 To Me.twizard.TabPages.Count - 1
                Me.twizard.TabPages(i).Enabled = True
            Next
            Me.Rprivatesave.Enabled = True
            Me.rglobalsave.Enabled = True
            Me.Pparam1.Visible = False
            pid = Me.oportfolios.portfolios(Me.Lpredefined.SelectedItems(0).Index).portfolio_id
            REM write out settings for selected portfoli to wizard
            For i = 0 To Me.oportfolios.portfolios.Count - 1
                If Me.oportfolios.portfolios(i).portfolio_id = pid Then

                    Me.selected_portfolio_id = Me.oportfolios.portfolios(i).portfolio_id
                    Me.c_show_ea.Checked = False
                    Me.cfunction.Text = Me.oportfolios.portfolios(i).function_name
                    Me.cclass.Text = Me.oportfolios.portfolios(i).class_name
                    Me.cschema.Text = Me.oportfolios.portfolios(i).schema_name
                    Me.lselentity.Items.Clear()
                    Me.lselentity.Sorted = True

                    Me.rcell.Visible = True
                    Me.Panel1.Visible = True
                    Me.Panel2.Visible = True
                    Me.pformat.Visible = True
                    If Me.oportfolios.portfolios(i).sp_name <> "" Then
                        Me.rcell.Visible = False
                        Me.Panel1.Visible = False
                        Me.Panel2.Visible = False
                        Me.pformat.Visible = False
                        REM if currency selection required enable
                        If Me.oportfolios.portfolios(i).show_currency = True Then
                            Me.Panel2.Visible = True
                            Me.Label16.Visible = False
                            Me.c_show_ea.Visible = False
                            Me.rcurrshow.Visible = False
                            Me.rcurrnone.Visible = False
                            Me.cformat.Visible = False
                            Me.cprec.Visible = False
                            Me.cprectype.Visible = False
                            Me.rcurrconv.Checked = True
                        End If
                    End If

                    Select Case Me.oportfolios.portfolios(i).universe_flag
                        Case 0
                            Me.rchoose.Checked = True
                            Me.lselentity.Items.Clear()
                            Me.lselentity.Sorted = False
                            For j = 0 To Me.oportfolios.portfolios(i).entities.count - 1
                                Me.lselentity.Items.Add(Me.oportfolios.portfolios(i).entities(j))
                                Dim iItems As Integer = 0
                                Do While iItems < lallentity.Items.Count
                                    Dim strItem As String = lallentity.Items(iItems)
                                    If strItem = Me.oportfolios.portfolios(i).entities(j) Then
                                        lallentity.Items.Remove(strItem)
                                    Else
                                        iItems += 1
                                    End If
                                Loop
                            Next
                        Case 1
                            Me.rentity_set.Checked = True
                            Me.lselentity.Sorted = False
                            Me.centityset.SelectedIndex = -1
                            Me.centityset.Text = Me.oportfolios.portfolios(i).entity_set_name


                        Case 2
                            Me.rchoose.Checked = True
                            Me.rclass.Checked = True
                            Me.cclass2.Text = Me.oportfolios.portfolios(i).entity_prop_class_name

                            Me.centity.Text = Me.oportfolios.portfolios(i).entity_prop_entity_name
                            If Me.oportfolios.portfolios(i).sec_entity_prop_class_name <> "" Then
                                Me.cand.Checked = True
                                Me.cclass3.Text = Me.oportfolios.portfolios(i).sec_entity_prop_class_name
                                Me.centity2.Text = Me.oportfolios.portfolios(i).sec_entity_prop_entity_name
                            End If
                        Case 3
                            Me.rchoose.Checked = True
                            Me.rall.Checked = True
                            Me.twizard.TabPages(3).Enabled = True
                    End Select

                    REM list of allowed entities
                    If Me.oportfolios.portfolios(i).item_set <> "" Then
                        Me.ritemset.Checked = True
                        Me.citemset.Text = Me.oportfolios.portfolios(i).item_set
                    End If
                    Me.citemset.Enabled = False
                    Me.bdelitemset.Enabled = False

                    Me.lselitem.Items.Clear()
                    Me.item_sel_type.Clear()
                    Me.item_sel_factor.Clear()
                    Me.item_sel_monatary.Clear()
                    Me.item_sel_symbol.Clear()
                    Me.item_sel_assoc_class.Clear()
                    For j = 0 To Me.oportfolios.portfolios(i).items.count - 1
                        Me.lselitem.Items.Add(Me.oportfolios.portfolios(i).items(j))
                        Dim iItems As Integer = 0
                        Do While iItems < lallitem.Items.Count
                            Dim strItem As String = lallitem.Items(iItems)
                            If strItem = Me.oportfolios.portfolios(i).items(j) Then
                                lallitem.Items.Remove(strItem)
                            Else
                                iItems += 1
                            End If
                        Loop
                        Me.item_sel_type.Add(Me.oportfolios.portfolios(i).item_types(j))
                        Me.item_sel_factor.Add(Me.oportfolios.portfolios(i).item_factor(j))
                        Me.item_sel_monatary.Add(Me.oportfolios.portfolios(i).item_monatary(j))
                        Me.item_sel_symbol.Add(Me.oportfolios.portfolios(i).item_symbol(j))
                        Me.item_sel_assoc_class.Add(Me.oportfolios.portfolios(i).item_assoc(j))
                    Next
                    REM set role based user settings
                    If bc_am_ef_functions.ousersettings.stages <> 0 Then
                        Me.cstage.Enabled = False
                        If bc_am_ef_functions.ousersettings.stages = 1 Then
                            Me.cstage.SelectedIndex = 0
                        Else
                            Me.cstage.SelectedIndex = 1
                        End If
                    Else
                        Me.cstage.Text = Me.oportfolios.portfolios(i).stage
                    End If
                    Me.Ccont.Text = Me.oportfolios.portfolios(i).contributor

                    If Me.oportfolios.portfolios(i).date_at = "9-9-9999" Then
                        Me.rnow.Checked = True
                    Else
                        Me.Rdate.Checked = True
                        Me.DateTimePicker1.Value = Me.oportfolios.portfolios(i).date_at
                    End If
                    If Me.oportfolios.portfolios(i).date_from = "1-1-1900" Then
                        Me.rold.Checked = True
                    Else
                        Me.rdatefrom.Checked = True
                        Me.DateTimePicker2.Value = Me.oportfolios.portfolios(i).date_from
                    End If
                    Me.Cyearstart.Text = Me.oportfolios.portfolios(i).start_year
                    Me.cyearend.Text = Me.oportfolios.portfolios(i).end_year
                    Me.lperiods.Items.Clear()
                    For j = 0 To Me.oportfolios.portfolios(i).periods.count - 1
                        Me.lperiods.Items.Add(Me.oportfolios.portfolios(i).periods(j))
                    Next
                    Select Case Me.oportfolios.portfolios(i).convert_type
                        Case 0
                            Me.rcurrshow.Checked = True
                        Case 1
                            Me.rcurrnone.Checked = True

                        Case 2
                            Me.rcurrconv.Checked = True
                            Me.ccurr.Text = Me.oportfolios.portfolios(i).currency
                    End Select
                    If Me.oportfolios.portfolios(i).show_e_a = True Then
                        Me.c_show_ea.Checked = True
                    End If
                    If Me.oportfolios.portfolios(i).format_type = 0 Then
                        Me.cformat.Checked = False
                    ElseIf Me.oportfolios.portfolios(i).format_type = 1 Then
                        Me.cformat.Checked = True
                        Me.cprectype.SelectedIndex = 0
                        Me.cprec.Text = CStr(Me.oportfolios.portfolios(i).precision)
                    ElseIf Me.oportfolios.portfolios(i).format_type = 2 Then
                        Me.cformat.Checked = True
                        Me.cprectype.SelectedIndex = 1
                        Me.cprec.Text = CStr(Me.oportfolios.portfolios(i).precision)
                    End If
                    If Me.oportfolios.portfolios(i).associated_class <> "" Then
                        Me.clink.Text = Me.oportfolios.portfolios(i).associated_class
                    Else
                        Me.clink.SelectedIndex = 0
                    End If

                    If Me.oportfolios.portfolios(i).enable_contributor = False Then
                        Me.Ccont.Enabled = False
                    End If
                    If Me.oportfolios.portfolios(i).enable_stage = False Then
                        Me.cstage.Enabled = False
                    End If

                    If Me.oportfolios.portfolios(i).enable_years_periods = False Then
                        Me.Cyearstart.Enabled = False
                        Me.cyearend.Enabled = False
                        Me.cperiods.Enabled = False
                        Me.lperiods.Enabled = False
                    Else
                        Me.Cyearstart.Enabled = True
                    End If
                    Me.rnow.Enabled = False
                    Me.Rdate.Enabled = False
                    Me.pdatefrom.Enabled = False
                    If Me.oportfolios.portfolios(i).enable_date_at = True Then
                        Me.rnow.Enabled = True
                        Me.Rdate.Enabled = True
                    End If

                    If Me.oportfolios.portfolios(i).enable_date_range = True Then
                        Me.pdatefrom.Enabled = True
                        Me.rnow.Enabled = True
                        Me.Rdate.Enabled = True
                    End If
                    Me.pdatetype.Visible = False
                    If Me.oportfolios.portfolios(i).show_date_type = True Then
                        Me.pdatetype.Visible = True
                    End If
                    Me.Pyearenddate.Visible = False
                    If Me.oportfolios.portfolios(i).show_year_end_date = True Then
                        Me.Pyearenddate.Visible = True
                    End If

                    Me.lallitem.Enabled = True
                    Me.lselitem.Enabled = True
                    Me.bdelall.Enabled = True
                    If Me.oportfolios.portfolios(i).enable_items = False Then
                        Me.lallitem.Enabled = False
                        Me.lselitem.Enabled = False
                        Me.bdelall.Enabled = False
                    End If
                    Me.Cparam1.Items.Clear()
                    If Me.oportfolios.portfolios(i).param1_list.count > 0 Then
                        Me.Pparam1.Visible = True
                        Me.lparam1.Text = Me.oportfolios.portfolios(i).param1_name
                        Me.Cparam1.Items.Clear()
                        For j = 0 To Me.oportfolios.portfolios(i).param1_list.count - 1
                            Me.Cparam1.Items.Add(Me.oportfolios.portfolios(i).param1_list(j))
                        Next
                        Me.Cparam1.SelectedIndex = 0
                    End If
                    Me.cass.SelectedIndex = 0
                    Me.Lselass.Items.Clear()
                    Me.lallass.Items.Clear()
                    set_desc()
                    For j = 3 To 5
                        Me.twizard.TabPages(j).Enabled = False
                    Next
                    If Me.rchoose.Checked = False Then
                        Me.twizard.TabPages(3).Enabled = True
                    End If

                    Exit For
                End If
            Next
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_potfolio_wizard", "open_potfolio", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub
    Private Sub clear_down()
        Try
            Dim i, j As Integer
            Me.cass.Visible = False
            Me.Lselass.Visible = False
            Me.lallass.Visible = False
            Me.lselentity.Visible = True
            Me.lallentity.Visible = True
            Me.tx_u = ""
            Me.tx_filt = ""
            Me.tx_c = ""
            Me.tx_f = ""
            Me.tx_curr = ""
            Me.cformat.Checked = False
            Me.ttitle.Text = ""
            Me.tsubtitle.Text = ""
            Me.tsource.Text = ""
            Me.cformat.Checked = True
            Me.cprectype.SelectedIndex = 1
            Me.cprec.Text = "3"
            Me.cand.Checked = False
            Me.Ccont.SelectedIndex = -1
            Me.Ccont.SelectedIndex = 0
            Me.rnow.Checked = True
            Me.ldesc.Text = ""
            Me.lselentity.Items.Clear()
            Me.lperiods.Items.Clear()
            Me.cperiods.Items.Clear()
            For i = 0 To bc_am_ef_functions.period_names.Count - 1
                Me.cperiods.Items.Add(bc_am_ef_functions.period_names(i))
            Next
            If Me.cperiods.Items.Count > -1 Then
                Me.lperiods.Items.Add(Me.cperiods.Items(0))
                Me.cperiods.Items.RemoveAt(0)
            End If

            Me.lselitem.Items.Clear()
            Me.cschema.SelectedIndex = 0

            Me.Ccont.SelectedIndex = 0
            Me.lallentity.Items.Clear()
            Me.ritemset.Checked = True
            Me.Cyearstart.Text = Year(Now)
            Me.cyearend.Text = Year(Now)
            Me.rcurrnone.Checked = True
            Me.ccurr.SelectedIndex = -1
            Me.cclass.SelectedIndex = -1
            Me.centity.SelectedIndex = -1
            Me.cclass2.SelectedIndex = -1
            For j = 1 To Me.twizard.TabPages.Count - 1
                Me.twizard.TabPages(j).Enabled = False
            Next
            Me.ldesc.Text = ""
            If bc_am_ef_functions.ousersettings.stages <> 0 Then
                Me.cstage.Enabled = False
                If bc_am_ef_functions.ousersettings.stages = 1 Then
                    Me.cstage.SelectedIndex = 0
                Else
                    Me.cstage.SelectedIndex = 1
                End If
            Else
                Me.cstage.SelectedIndex = 0
            End If
        Catch

        Finally
            Me.tx_u = ""
            Me.tx_filt = ""
            Me.tx_c = ""
            Me.tx_f = ""
            Me.tx_curr = ""
        End Try

    End Sub
    Private Sub rnew_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rnew.CheckedChanged
        If Me.rnew.Checked = True Then
            pre_defined_controls(False)
            Me.Ppredefined.Visible = False
            clear_down()
            Me.tname.Text = ""
            Me.ldesc.Text = ""
            Me.Rprivatesave.Enabled = True
            Me.rglobalsave.Enabled = True
            Me.Popen.Visible = False
            Me.Ppredefined.Visible = False
            Me.cfunction.Visible = True
            Me.cfunction.Text = "get financial data"
        End If

    End Sub

    Private Sub ropen_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ropen.CheckedChanged
        If Me.ropen.Checked = True Then
            pre_defined_controls(False)
            Me.Ppredefined.Visible = False
            clear_down()
            Me.Rprivatesave.Enabled = True
            Me.rglobalsave.Enabled = True
            load_user_defined_portfolios()
            Me.Popen.Visible = True
            Me.bopport.Enabled = False
            Me.bdelport.Enabled = False
            Me.cfunction.Visible = False
        End If
    End Sub

    Private Sub rcurrnone_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rcurrnone.CheckedChanged
        If Me.rcurrnone.Checked = True Then
            set_desc()
            If Me.lselentity.Items.Count > 0 And Me.lselentity.Items.Count > 0 Then
                Me.bok.Visible = True
            End If

        End If
    End Sub

    Private Sub bdelport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bdelport.Click
        Dim i As Integer
        Try
            If Me.lportfolios.SelectedItems.Count <> 1 Then
                Exit Sub
            End If
            Dim omsg As New bc_cs_message("Blue Curve", "Are you sure you wish to delete portfolio " + Me.lportfolios.SelectedItems(0).Text, bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
            If omsg.cancel_selected = True Then
                Exit Sub
            End If
            Me.ldesc.Clear()
            Me.Cursor = Cursors.WaitCursor
            For i = 0 To Me.oportfolios.portfolios.Count - 1
                If Me.oportfolios.portfolios(i).portfolio_id = CInt(Me.lportfolios.SelectedItems(0).SubItems(3).Text) Then
                    If Me.oportfolios.portfolios(i).user_id = bc_cs_central_settings.logged_on_user_id Then
                        Me.oportfolios.portfolios(i).delete_only = True
                        Me.Cursor = Cursors.WaitCursor

                        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                            Me.oportfolios.portfolios(i).db_write()
                        Else
                            Me.oportfolios.portfolios(i).tmode = bc_cs_soap_base_class.tWRITE
                            If Me.oportfolios.portfolios(i).transmit_to_server_and_receive(Me.oportfolios.portfolios(i), True) = False Then
                                Exit Sub
                            End If
                        End If
                        clear_down()
                        Me.Rprivatesave.Enabled = True
                        Me.rglobalsave.Enabled = True
                        load_user_defined_portfolios()
                        Me.Popen.Visible = True
                        Me.bforward.Visible = False

                    End If
                    Exit For
                End If
            Next
            Me.bdelport.Enabled = False
            Me.bopport.Enabled = False

        Catch

        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub rcurrshow_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rcurrshow.CheckedChanged
        If Me.rcurrshow.Checked = True Then
            set_desc()
            If Me.lselentity.Items.Count > 0 And Me.lselentity.Items.Count > 0 Then
                Me.bok.Visible = True
            End If
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
                Return String.Compare(item1.SubItems(0).Text, item2.SubItems(0).Text)
            Else
                Return String.Compare(item2.SubItems(0).Text, item1.SubItems(0).Text)
            End If
        End Function
    End Class
    Class CompareBytype
        Implements System.Collections.IComparer
        Public Shared toggle As Boolean = False

        Function compare(ByVal x As Object, ByVal y As Object) As Integer _
        Implements System.Collections.IComparer.Compare


            Dim item1 As ListViewItem = CType(x, ListViewItem)
            Dim item2 As ListViewItem = CType(y, ListViewItem)
            If toggle = True Then
                Return String.Compare(item1.SubItems(3).Text, item2.SubItems(3).Text)
            Else
                Return String.Compare(item2.SubItems(3).Text, item1.SubItems(3).Text)
            End If
        End Function
    End Class
    Class CompareByuser
        Implements System.Collections.IComparer
        Public Shared toggle As Boolean = False

        Function compare(ByVal x As Object, ByVal y As Object) As Integer _
        Implements System.Collections.IComparer.Compare


            Dim item1 As ListViewItem = CType(x, ListViewItem)
            Dim item2 As ListViewItem = CType(y, ListViewItem)
            If toggle = True Then
                Return String.Compare(item1.SubItems(2).Text, item2.SubItems(2).Text)
            Else
                Return String.Compare(item2.SubItems(2).Text, item1.SubItems(2).Text)
            End If
        End Function
    End Class
    Class CompareBydate
        Implements System.Collections.IComparer
        Public Shared toggle As Boolean = False

        Function compare(ByVal x As Object, ByVal y As Object) As Integer _
        Implements System.Collections.IComparer.Compare
            Try
                Dim item1 As ListViewItem = CType(x, ListViewItem)
                Dim item2 As ListViewItem = CType(y, ListViewItem)
                If toggle = True Then
                    Return Date.Compare(Date.Parse(item1.SubItems(1).Text), Date.Parse(item2.SubItems(1).Text))
                Else
                    Return Date.Compare(Date.Parse(item2.SubItems(1).Text), Date.Parse(item1.SubItems(1).Text))
                End If
            Catch

            End Try
        End Function
    End Class

    Private Sub bsaveent_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bsaveent.Click
        Dim oentity_set As New bc_om_ef_entity_set
        Dim i As Integer
        REM check set name isnt already used
        Me.Cursor = Cursors.WaitCursor
        Try

            Me.load_entity_sets()
            For i = 0 To Me.user_entity_sets.entity_sets.Count - 1
                If UCase(Me.user_entity_sets.entity_sets(i).name) = UCase(Me.tentname.Text) Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Set name: " + Me.tentname.Text + " already in use please enter another name", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub
                End If
            Next
            oentity_set.user_id = bc_cs_central_settings.logged_on_user_id
            oentity_set.class_name = Me.cclass.Text
            oentity_set.name = Me.tentname.Text
            For i = 0 To Me.lselentity.Items.Count - 1
                oentity_set.entities.Add(Me.lselentity.Items(i))
            Next
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                oentity_set.db_write()
            Else
                oentity_set.tmode = bc_cs_soap_base_class.tWRITE
                If oentity_set.transmit_to_server_and_receive(oentity_set, True) = False Then
                    Exit Sub
                End If
            End If
            Me.bsaveent.Enabled = False
            Me.tentname.Enabled = False
            Me.tentname.Text = ""
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bsavenet", "click", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally

            Me.Cursor = Cursors.Default
        End Try

    End Sub

    Private Sub tentname_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tentname.TextChanged

        If Trim(Me.tentname.Text) <> "" Then
            Me.bsaveent.Enabled = True
        Else
            Me.bsaveent.Enabled = False
        End If
    End Sub

    Private Sub Bdel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Bdel.Click

        Try
            If Me.centityset.SelectedIndex > -1 Then
                Dim omsg As New bc_cs_message("Blue Curve", "Are you wish to delete set: " + Me.centityset.Text + "?", bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
                If omsg.cancel_selected = True Then
                    Exit Sub
                End If

                Me.Cursor = Cursors.WaitCursor

                Dim oentity_set As New bc_om_ef_entity_set
                oentity_set.delete_flag = True
                oentity_set.set_id = user_entity_sets.entity_sets(Me.centityset.SelectedIndex).set_id
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    oentity_set.db_write()

                Else
                    oentity_set.tmode = bc_cs_soap_base_class.tWRITE
                    If oentity_set.transmit_to_server_and_receive(oentity_set, True) = False Then
                        Exit Sub
                    End If
                End If
                Me.lselentity.Items.Clear()
                Me.load_entity_sets()
                Me.Bdel.Enabled = False
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bdel", "click", bc_cs_error_codes.USER_DEFINED, ex.Message)


        Finally

            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub RadioButton3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ritemset.CheckedChanged
        Me.bselitem.Enabled = False
        load_saved_sets()
    End Sub
    Private Sub load_saved_sets()

        Me.ball.Enabled = False
        If Me.ritemset.Checked = True Then
            Me.citemset.Visible = True
            Me.bdelitemset.Visible = True
            Me.bdelitemset.Enabled = False
            Me.lallitem.Sorted = False

            Me.lallitem.Items.Clear()
            load_item_sets()
        Else
            Me.bdelitemset.Visible = False
            Me.citemset.Visible = False
        End If
    End Sub
    Private Sub load_item_sets()
        Try
            Me.citemset.Enabled = False

            Dim i As Integer
            Me.Cursor = Cursors.WaitCursor
            Me.citemset.Items.Clear()
            user_items_sets = New bc_om_ef_item_sets
            If Me.Rpredefined.Checked = True Then
                user_items_sets.user_id = 0
            Else
                user_items_sets.user_id = bc_cs_central_settings.logged_on_user_id
            End If
            user_items_sets.class_name = Me.cclass.Text
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                user_items_sets.db_read()
            Else
                user_items_sets.tmode = bc_cs_soap_base_class.tREAD
                If user_items_sets.transmit_to_server_and_receive(user_items_sets, True) = False Then
                    Exit Sub
                End If
            End If
            For i = 0 To user_items_sets.item_sets.Count - 1
                Me.citemset.Items.Add(user_items_sets.item_sets(i).name)
                Me.citemset.Enabled = True
            Next




        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Portfolio Form", "load_item_sets", bc_cs_error_codes.USER_DEFINED, ex.Message)


        Finally
            Me.Cursor = Cursors.Default


        End Try
    End Sub


    Private Sub rallitems_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rallitems.CheckedChanged
        Dim i As Integer
        Try
            Me.lallitem.Sorted = True
            If Me.rallitems.Checked = True Then
                Me.lallitem.Sorted = False

                Me.lallitem.Items.Clear()
                Me.filter_item_ids.Clear()
                Me.filter_item_types.Clear()
                Me.filter_item_factor.Clear()
                Me.filter_item_monatary.Clear()
                Me.filter_item_symbol.Clear()
                Me.Cursor = Cursors.WaitCursor
                For i = 0 To bc_am_ef_functions.item_names.Count - 1
                    Me.filter_item_ids.Add(bc_am_ef_functions.item_names(i))
                    Me.filter_item_types.Add(bc_am_ef_functions.item_types(i))
                    Me.filter_item_factor.Add(bc_am_ef_functions.item_factors(i))
                    Me.filter_item_monatary.Add(bc_am_ef_functions.item_monatarys(i))
                    Me.filter_item_symbol.Add(bc_am_ef_functions.item_symbols(i))
                    Me.lallitem.Items.Add(bc_am_ef_functions.item_names(i))
                Next
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub
    Private Function get_ef_id() As Integer
        For i = 0 To bc_am_ef_functions.class_names.Count - 1
            If bc_am_ef_functions.class_names(i) = Me.clink.Text Then
                For j = 0 To Me.use_class_ids.Count - 1
                    If Me.use_class_ids(j) = bc_am_ef_functions.class_ids(i) Then
                        If Me.use_class_types(j) = "Parent" Then
                            get_ef_id = 5005
                        Else
                            get_ef_id = 5006
                        End If
                        Exit For
                    End If

                Next
                Exit For
            End If
        Next
    End Function
    Public Sub get_propogating_entities()
        Dim i As Integer
        Dim outstr As String
        Dim istr As String
        Dim ty As Integer
        If Me.clink.SelectedIndex > 0 Then


            ty = get_ef_id()
            propentitymaster.Clear()
            propentityslave.Clear()

            REM propogating entities for selecting entities
            For i = 0 To Me.lselentity.Items.Count - 1
                outstr = load_propogating_entities(ty, Me.cclass.Text, Me.lselentity.Items(i), Me.clink.Text)
                If InStr(outstr, ";") = 0 Then
                    propentitymaster.Add(Me.lselentity.Items(i))
                    propentityslave.Add(outstr)
                Else
                    While InStr(outstr, ";") > 0
                        istr = outstr.Substring(0, InStr(outstr, ";") - 1)
                        propentitymaster.Add(Me.lselentity.Items(i))
                        propentityslave.Add(istr)
                        outstr = outstr.Substring(InStr(outstr, ";"), Len(outstr) - InStr(outstr, ";"))
                    End While
                End If
            Next
        End If

    End Sub
    Private Sub rportitems_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rportitems.CheckedChanged
        Dim i As Integer

        Dim type_show As New ArrayList
        Dim cross_show As New ArrayList
        Dim oitem_set As New bc_om_portfolio_universe_set

        Try
            If Me.rportitems.Checked = False Then
                Exit Sub
            End If
            Me.Cursor = Cursors.WaitCursor
            Me.filter_item_ids.Clear()
            Me.filter_item_types.Clear()
            Me.filter_item_factor.Clear()
            Me.filter_item_monatary.Clear()
            Me.filter_item_symbol.Clear()
            Me.filter_item_assoc_class.Clear()
            type_show.Clear()
            cross_show.Clear()
            Me.lallitem.Items.Clear()
            Me.bselitem.Enabled = False
            oitem_set.schema_id = bc_am_ef_functions.schema_ids(Me.cschema.SelectedIndex)
            Me.lallitem.Sorted = True
            REM load items for each template of entity used
            For i = 0 To Me.lselentity.Items.Count - 1
                oitem_set.entities.Add(Me.lselentity.Items(i))
                oitem_set.assoc_classes.Add(Me.cclass.Text)
            Next
            REM if propogating class
            If Me.clink.SelectedIndex > 0 Then
                Me.get_propogating_entities()
                REM load items for each template of entity used
                For i = 0 To Me.propentityslave.Count - 1
                    oitem_set.entities.Add(Me.propentityslave(i))
                    oitem_set.assoc_classes.Add(Me.clink.Text)
                Next
            End If
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                oitem_set.db_read()
            Else
                oitem_set.tmode = bc_cs_soap_base_class.tREAD
                If oitem_set.transmit_to_server_and_receive(oitem_set, True) = False Then
                    Exit Sub
                End If
            End If
            For i = 0 To oitem_set.item_names.Count - 1
                Me.filter_item_ids.Add(oitem_set.item_names(i))
                Me.filter_item_types.Add(oitem_set.item_type(i))
                Me.filter_item_factor.Add(oitem_set.factor(i))
                Me.filter_item_monatary.Add(oitem_set.monetary(i))
                Me.filter_item_symbol.Add(oitem_set.symbol(i))
                Me.filter_item_assoc_class.Add(oitem_set.iclass(i))
                If oitem_set.class_dup(i) = 0 Then
                    cross_show.Add(False)
                Else
                    cross_show.Add(True)
                End If
                If oitem_set.type_dup(i) = 0 Then
                    type_show.Add(False)
                Else
                    type_show.Add(True)
                End If
            Next
            Dim icount As Integer
            icount = Me.filter_item_ids.Count
            For i = 0 To icount - 1
                If type_show(i) = False And cross_show(i) = False Then
                    Me.lallitem.Items.Add(Me.filter_item_ids(i))
                ElseIf type_show(i) = True And cross_show(i) = False Then
                    Me.lallitem.Items.Add(Me.filter_item_ids(i) + " {static}")
                    Me.lallitem.Items.Add(Me.filter_item_ids(i) + " {period}")
                    REM now add a new one
                    Me.filter_item_ids.Add(Me.filter_item_ids(i) + " {period}")
                    Me.filter_item_ids(i) = Me.filter_item_ids(i) + " {static}"
                    Me.filter_item_types.Add("period")
                    Me.filter_item_factor.Add(Me.filter_item_factor(i))
                    Me.filter_item_monatary.Add(Me.filter_item_monatary(i))
                    Me.filter_item_symbol.Add(Me.filter_item_symbol(i))
                    Me.filter_item_assoc_class.Add(Me.filter_item_assoc_class(i))
                ElseIf type_show(i) = False And cross_show(i) = True Then
                    Me.lallitem.Items.Add(Me.filter_item_ids(i) + " {" + Me.cclass.Text + "}")
                    REM now add another one
                    Me.lallitem.Items.Add(Me.filter_item_ids(i) + " {" + Me.clink.Text + "}")
                    Me.filter_item_ids.Add(Me.filter_item_ids(i) + " {" + Me.clink.Text + "}")
                    Me.filter_item_types.Add(Me.filter_item_types(i))
                    Me.filter_item_factor.Add(Me.filter_item_factor(i))
                    Me.filter_item_monatary.Add(Me.filter_item_monatary(i))
                    Me.filter_item_symbol.Add(Me.filter_item_symbol(i))
                    Me.filter_item_assoc_class.Add(Me.clink.Text)
                    Me.filter_item_ids(i) = Me.filter_item_ids(i) + " {" + Me.cclass.Text + "}"

                ElseIf type_show(i) = True And cross_show(i) = True Then
                    Me.lallitem.Items.Add(Me.filter_item_ids(i) + " {static}{" + Me.cclass.Text + "}")
                    REM now add another 3
                    Me.lallitem.Items.Add(Me.filter_item_ids(i) + " {period}{" + Me.cclass.Text + "}")
                    Me.lallitem.Items.Add(Me.filter_item_ids(i) + " {static}{" + Me.clink.Text + "}")
                    Me.lallitem.Items.Add(Me.filter_item_ids(i) + " {period}{" + Me.clink.Text + "}")
                    Me.filter_item_ids.Add(Me.filter_item_ids(i) + " {period}{" + Me.cclass.Text + "}")
                    Me.filter_item_ids.Add(Me.filter_item_ids(i) + " {static}{" + Me.clink.Text + "}")
                    Me.filter_item_ids.Add(Me.filter_item_ids(i) + " {period}{" + Me.clink.Text + "}")
                    Me.filter_item_types.Add("period")
                    Me.filter_item_factor.Add(Me.filter_item_factor(i))
                    Me.filter_item_monatary.Add(Me.filter_item_monatary(i))
                    Me.filter_item_symbol.Add(Me.filter_item_symbol(i))
                    Me.filter_item_assoc_class.Add(Me.filter_item_assoc_class(i))
                    Me.filter_item_types.Add(Me.filter_item_types(i))
                    Me.filter_item_factor.Add(Me.filter_item_factor(i))
                    Me.filter_item_monatary.Add(Me.filter_item_monatary(i))
                    Me.filter_item_symbol.Add(Me.filter_item_symbol(i))
                    Me.filter_item_assoc_class.Add(Me.clink.Text)
                    Me.filter_item_types.Add("period")
                    Me.filter_item_factor.Add(Me.filter_item_factor(i))
                    Me.filter_item_monatary.Add(Me.filter_item_monatary(i))
                    Me.filter_item_symbol.Add(Me.filter_item_symbol(i))
                    Me.filter_item_assoc_class.Add(Me.clink.Text)
                    Me.filter_item_ids(i) = Me.filter_item_ids(i) + " {static}{" + Me.cclass.Text + "}"

                End If
            Next
            Me.ball.Enabled = True

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("rportitems", "checkedchanged", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.Cursor = Cursors.Default

        End Try
    End Sub

    Private Sub titemset_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles titemset.TextChanged
        If Trim(Me.titemset.Text) <> "" Then
            Me.bsaveitem.Enabled = True
        Else
            Me.bsaveitem.Enabled = False
        End If
    End Sub

    Private Sub bsaveitem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bsaveitem.Click
        Try
            Dim i As Integer
            Me.Cursor = Cursors.WaitCursor
            REM set name doesnt already exist
            Me.load_item_sets()
            For i = 0 To Me.user_items_sets.item_sets.Count - 1
                If UCase(Me.user_items_sets.item_sets(i).name) = UCase(Me.titemset.Text) Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Item set name " + Me.titemset.Text + " already exists!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub
                End If
            Next
            Me.Cursor = Cursors.WaitCursor
            Dim osaveitems As New bc_om_ef_item_set
            osaveitems.user_id = bc_cs_central_settings.logged_on_user_id
            osaveitems.class_name = Me.cclass.Text
            osaveitems.name = Me.titemset.Text
            For i = 0 To Me.lselitem.Items.Count - 1
                osaveitems.items.Add(Me.lselitem.Items(i))
                osaveitems.types.Add(Me.item_sel_type(i))
                osaveitems.factors.Add(Me.item_sel_factor(i))
                osaveitems.monatarys.Add(Me.item_sel_monatary(i))
                osaveitems.symbols.Add(Me.item_sel_symbol(i))
                osaveitems.assoc_classes.Add(Me.item_sel_assoc_class(i))
            Next
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                osaveitems.db_write()
            Else
                osaveitems.tmode = bc_cs_soap_base_class.tWRITE
                If osaveitems.transmit_to_server_and_receive(osaveitems, True) = False Then
                    Exit Sub
                End If
            End If
            Me.load_item_sets()
            Me.titemset.Enabled = False
            Me.bsaveitem.Enabled = False

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bsaveiten", "click", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub
    Private Sub citemset_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles citemset.SelectedIndexChanged
        Me.bdelitemset.Enabled = False
        If Me.citemset.SelectedIndex = -1 Then
            Exit Sub
        End If
        Dim i As Integer
        Me.lallitem.Items.Clear()
        Me.lallitem.Sorted = False
        Me.ball.Enabled = False
        Me.filter_item_ids.Clear()
        Me.filter_item_types.Clear()
        Me.filter_item_factor.Clear()
        Me.filter_item_monatary.Clear()
        Me.filter_item_symbol.Clear()
        Me.filter_item_assoc_class.Clear()
        For i = 0 To Me.user_items_sets.item_sets(Me.citemset.SelectedIndex).items.count - 1
            If Me.user_items_sets.item_sets(Me.citemset.SelectedIndex).assoc_classes(i) = Me.cclass.Text Or (Me.clink.Text = Me.user_items_sets.item_sets(Me.citemset.SelectedIndex).assoc_classes(i)) Then
                Me.filter_item_ids.Add(Me.user_items_sets.item_sets(Me.citemset.SelectedIndex).items(i))
                Me.filter_item_types.Add(Me.user_items_sets.item_sets(Me.citemset.SelectedIndex).types(i))
                Me.filter_item_monatary.Add(Me.user_items_sets.item_sets(Me.citemset.SelectedIndex).monatarys(i))
                Me.filter_item_symbol.Add(Me.user_items_sets.item_sets(Me.citemset.SelectedIndex).symbols(i))
                Me.filter_item_assoc_class.Add(Me.user_items_sets.item_sets(Me.citemset.SelectedIndex).assoc_classes(i))
                Me.filter_item_factor.Add(Me.user_items_sets.item_sets(Me.citemset.SelectedIndex).factors(i))
                Me.lallitem.Items.Add(Me.user_items_sets.item_sets(Me.citemset.SelectedIndex).items(i))
                Me.ball.Enabled = True
            End If
        Next

        Me.bdelitemset.Enabled = True

    End Sub

    Private Sub bdelitemset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bdelitemset.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Dim omsg As New bc_cs_message("Blue Curve", "Are you sure you wish to delete item set " + Me.citemset.Text + "?", bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
            If omsg.cancel_selected = True Then
                Exit Sub
            End If
            Me.user_items_sets.item_sets(Me.citemset.SelectedIndex).delete_flag = True
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                Me.user_items_sets.item_sets(Me.citemset.SelectedIndex).db_write()
            Else
                If Me.user_items_sets.item_sets(Me.citemset.SelectedIndex).transmit_to_server_and_receive(Me.user_items_sets.item_sets(Me.citemset.SelectedIndex), True) = False Then
                    Exit Sub
                End If
            End If


            Me.load_item_sets()

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bdelitemset", "click", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            Me.Cursor = Cursors.Default


        End Try
    End Sub
    Private Sub bok_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles bok.MouseUp
         Dim i As Integer
        success = False
        Me.bok.Visible = True
        Me.Refresh()
        Me.bokdisabled.Visible = False
        tprogress = New System.Threading.Thread(AddressOf run_progress_bar)



        Try

            Me.Cursor = Cursors.WaitCursor
            REM predefined and overridable 
            If Me.Rpredefined.Checked = True Then
                If Me.oportfolios.portfolios(Me.Lpredefined.SelectedItems(0).Index).sp_name <> "" Then
                    custom_predefined_extract()
                    Exit Sub
                End If
            End If
            REM get portfolio name list
            If Me.rnew.Checked = True And Me.Rnosave.Checked = False Then
                Dim onames As New bc_om_portoflio_names
                onames.user_id = bc_cs_central_settings.logged_on_user_id
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    onames.db_read()
                Else
                    onames.tmode = bc_cs_soap_base_class.tREAD
                    onames.transmit_to_server_and_receive(onames, True)
                End If
                For i = 0 To onames.names.Count - 1
                    If Trim(UCase(tname.Text)) = Trim(UCase(onames.names(i))) Then
                        Dim omsg As New bc_cs_message("Blue Curve", "Portofolio Name: " + Me.tname.Text + " already exists, please change", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                        Me.twizard.SelectedIndex = 0
                        Me.tname.Focus()
                        Exit Sub
                    End If
                Next
            End If

            'REM Steve Wooderson 18/01/2013 DA5
            If cchart.Checked = True Then
                chart_data = True
            Else
                chart_data = False
            End If

            Me.ppropress.Visible = True
            tprogress.Start()

            Me.ppropress.Increment(2)

            If Me.cfunction.SelectedIndex = 1 Then
                If build_portfolio_historic() = True Then
                    success = True
                End If
            Else
                'If build_portfolio() = True Then
                success = True
                'End If
            End If
            'If Me.Rnosave.Checked = False And success = True Then
            '    If save_portfolio(Me.rglobalsave.Checked) = False Then
            '        Exit Sub
            '    End If
            'End If
            If success = True Then
                Me.Hide()
            End If


        Catch ex As Exception
            MsgBox(ex.Message)


        Finally

            If tprogress.ThreadState = Threading.ThreadState.Running Then
                tprogress.Abort()
            End If
            Me.bok.Visible = True
            Me.Cursor = Cursors.Default
        End Try


    End Sub

    Private Sub bok_Mousedn(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles bok.MouseDown
        Me.Cursor = Cursors.WaitCursor
        Me.bok.Visible = False
        Me.bokdisabled.Visible = True
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub bleft_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles bleft.MouseDown
        Me.Cursor = Cursors.WaitCursor
        Me.bleft.Visible = False
        Me.bleftdisabled.Visible = True
        Me.Cursor = Cursors.Default

    End Sub
    Private Sub bleft_Mouseup(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles bleft.MouseUp
        If Me.twizard.SelectedIndex > 0 Then
            Me.bleft.Visible = True
        End If
        Me.bleftdisabled.Visible = False
        If Me.twizard.SelectedIndex <> 0 Then
            Me.twizard.SelectedIndex = Me.twizard.SelectedIndex - 1
        End If
        If Me.twizard.SelectedIndex = 0 Then
            Me.bleft.Visible = False
            Dim dp As System.Drawing.Point
            dp.Y = 8
            dp.X = 408
            Me.bcancel.Location = dp
            Me.bcanceldisabled.Location = dp

        End If
        If Me.twizard.TabPages(Me.twizard.SelectedIndex + 1).Enabled = True Then
            set_forward_enabled()
            Me.bok.Visible = False

        End If

    End Sub
    Private Sub bcancel_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles bcancel.MouseDown
        Me.Cursor = Cursors.WaitCursor
        Me.bcancel.Visible = False
        Me.bcanceldisabled.Visible = True
        Me.Cursor = Cursors.Default

    End Sub
    Private Sub bcancel_Mouseup(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles bcancel.MouseUp
        Me.success = False
        Me.Hide()
    End Sub
    Private Sub set_forward_enabled()
        Me.bforward.Visible = True
        Dim dp As System.Drawing.Point
        dp.Y = 8
        If Me.twizard.SelectedIndex = 0 Then
            dp.X = 408

        Else
            dp.X = 408
            Me.bleft.Location = dp
            Me.bleftdisabled.Location = dp
            dp.X = 344
        End If
        Me.bcancel.Location = dp
        Me.bcanceldisabled.Location = dp
    End Sub
    Private Sub bforward_Mousedown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles bforward.MouseDown
        Me.Cursor = Cursors.WaitCursor
        Me.bforward.Visible = False
        Me.bforwarddisabled.Visible = True
        Me.Cursor = Cursors.Default
    End Sub
    Private Sub bforward_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles bforward.MouseUp

        Me.bforwarddisabled.Visible = False
        If Me.twizard.SelectedIndex <> 3 And Me.twizard.SelectedIndex <> 4 Then
            Me.bforward.Visible = False
        End If

        If Me.twizard.SelectedIndex < Me.twizard.TabPages.Count - 1 Then
            If Me.twizard.TabPages(Me.twizard.SelectedIndex + 1).Enabled = True Then
                Me.twizard.SelectedIndex = Me.twizard.SelectedIndex + 1
            End If
        End If

    End Sub
    Private Sub run_progress_bar()
        Me.ppropress.Visible = True
        Me.ppropress.BringToFront()
        While True
            Threading.Thread.Sleep(1000)
            Me.ppropress.Increment(5)
            Me.ppropress.Update()
            Me.ppropress.Refresh()
            If Me.ppropress.Value = 100 Then
                Me.ppropress.Value = 0
            End If
        End While

        Me.ppropress.Visible = False

    End Sub

    Private Sub Label20_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label20.Click

    End Sub

    Private Sub bok_MouseUp1(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles bok.MouseUp

    End Sub

    Private Sub bforward_CursorChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles bforward.CursorChanged

    End Sub

    Private Sub RadioButton1_CheckedChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdefined.CheckedChanged
        Dim i, j, k As Integer
        Try

            Me.Cursor = Cursors.WaitCursor



            Me.ctemplate.Visible = False
            Me.ccontext.Visible = False
            Me.csection.Visible = False
            Me.ltemplate.Visible = False
            Me.lcontext.Visible = False
            Me.lsection.Visible = False
            If Me.rdefined.Checked = True Then
                Me.lallitem.Sorted = False
                Me.bselitem.Enabled = False
                Me.lallitem.Items.Clear()
                Me.filter_item_ids.Clear()
                Me.filter_item_types.Clear()
                Me.ctemplate.Visible = True
                Me.ltemplate.Visible = True
                If Me.twizard.SelectedIndex = 3 Then
                    oseltemplates.schema_id = bc_am_ef_functions.schema_ids(Me.cschema.SelectedIndex)
                    oseltemplates.entity_ids.Clear()
                    oseltemplates.class_names.Clear()
                    REM get template and unsiom item list based on entities selected
                    REM if single entity
                    If Me.clink.SelectedIndex <= 0 Then

                        For j = 0 To Me.lselentity.Items.Count - 1
                            For i = 0 To bc_am_ef_functions.entities.Count - 1
                                If bc_am_ef_functions.entities(i).class_id = bc_am_ef_functions.class_ids(Me.cclass.SelectedIndex) Then
                                    If bc_am_ef_functions.entities(i).name = Me.lselentity.Items(j) Then
                                        oseltemplates.entity_ids.Add(bc_am_ef_functions.entities(i).id)
                                        oseltemplates.class_names.Add(Me.cclass.Text)
                                        Exit For
                                    End If
                                End If
                            Next
                        Next
                    Else
                        Me.get_propogating_entities()
                        For j = 0 To Me.propentitymaster.Count - 1
                            For i = 0 To bc_am_ef_functions.entities.Count - 1
                                If bc_am_ef_functions.entities(i).class_id = bc_am_ef_functions.class_ids(Me.cclass.SelectedIndex) Then
                                    If bc_am_ef_functions.entities(i).name = Me.propentitymaster(j) Then
                                        oseltemplates.entity_ids.Add(bc_am_ef_functions.entities(i).id)
                                        oseltemplates.class_names.Add(Me.cclass.Text)
                                        Exit For
                                    End If
                                End If
                            Next
                        Next
                        Dim found As Boolean = False
                        For k = 0 To bc_am_ef_functions.class_names.Count - 1
                            If bc_am_ef_functions.class_names(k) = Me.clink.Text Then
                                For j = 0 To Me.propentityslave.Count - 1
                                    For i = 0 To bc_am_ef_functions.entities.Count - 1
                                        If bc_am_ef_functions.entities(i).class_id = bc_am_ef_functions.class_ids(k) Then
                                            If bc_am_ef_functions.entities(i).name = Me.propentityslave(j) Then
                                                oseltemplates.entity_ids.Add(bc_am_ef_functions.entities(i).id)
                                                oseltemplates.class_names.Add(Me.clink.Text)
                                                Exit For
                                            End If
                                        End If
                                    Next
                                Next
                                Exit For
                            End If
                        Next
                    End If
                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        oseltemplates.db_read()
                    ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                        oseltemplates.tmode = bc_cs_soap_base_class.tREAD
                        If oseltemplates.transmit_to_server_and_receive(oseltemplates, True) = False Then
                            Exit Sub
                        End If
                    End If
                    Me.ctemplate.Items.Clear()
                    For i = 0 To oseltemplates.template_ids.Count - 1
                        For j = 0 To bc_am_ef_functions.template_names.Count - 1
                            If oseltemplates.template_ids(i) = bc_am_ef_functions.template_ids(j) Then
                                Me.ctemplate.Items.Add(bc_am_ef_functions.template_names(j))
                            End If
                        Next
                    Next
                End If
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("rdefined", "checkechanged", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub ccontext_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ccontext.SelectedIndexChanged
        Dim i As Integer
        If Me.ccontext.SelectedIndex = -1 Then
            Exit Sub
        End If
        Me.csection.Visible = False
        Me.lsection.Visible = False
        Me.csection.SelectedIndex = -1
        Dim tid As Long
        If Me.ccontext.SelectedIndex = 0 Then
            Me.csection.Visible = True
            Me.lsection.Visible = True
            For i = 0 To bc_am_ef_functions.template_names.Count - 1
                If bc_am_ef_functions.template_names(i) = Me.ctemplate.Text Then
                    tid = bc_am_ef_functions.template_ids(i)
                    Exit For
                End If
            Next
            Me.csection.Items.Clear()
            Me.csection.Items.Add("All")
            For i = 0 To oseltemplates.section_names.Count - 1
                If oseltemplates.section_templates(i) = tid Then
                    Me.csection.Items.Add(oseltemplates.section_names(i))
                End If
            Next
            'Else
            '    data_dictionary()
        End If

    End Sub

    Private Sub csection_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles csection.SelectedIndexChanged
        If Me.csection.SelectedIndex = -1 Then
            Exit Sub
        End If
        If Me.rnew.Checked = True Then
            Me.tsubtitle.Text = Me.csection.Text
        End If
        Me.bselent.Enabled = False
        data_dictionary()
    End Sub
    Private Function find_entity_in_template() As String
        Dim i, j, k As Integer
        Dim tid As Long

        find_entity_in_template = ""

        For i = 0 To bc_am_ef_functions.template_names.Count - 1
            If bc_am_ef_functions.template_names(i) = Me.ctemplate.Text Then
                tid = bc_am_ef_functions.template_ids(i)
                Exit For
            End If

        Next
        For i = 0 To Me.oseltemplates.entity_template_ids.Count - 1
            If Me.oseltemplates.entity_template_ids(i) = tid Then
                If Me.clink.SelectedIndex <= 0 Then
                    For j = 0 To Me.lselentity.Items.Count - 1
                        For k = 0 To bc_am_ef_functions.entities.Count - 1
                            If bc_am_ef_functions.entities(k).name = Me.lselentity.Items(j) Then
                                If bc_am_ef_functions.entities(k).id = Me.oseltemplates.entity_ids(i) Then
                                    find_entity_in_template = Me.lselentity.Items(j)
                                    Exit Function
                                End If
                            End If
                        Next
                    Next
                Else
                    For j = 0 To Me.propentitymaster.Count - 1
                        For k = 0 To bc_am_ef_functions.entities.Count - 1
                            If bc_am_ef_functions.entities(k).name = Me.propentitymaster(j) Then
                                If bc_am_ef_functions.entities(k).id = Me.oseltemplates.entity_ids(i) Then
                                    find_entity_in_template = Me.propentitymaster(j)
                                    Exit Function
                                End If
                            End If
                        Next
                    Next
                    For j = 0 To Me.propentityslave.Count - 1
                        For k = 0 To bc_am_ef_functions.entities.Count - 1
                            If bc_am_ef_functions.entities(k).name = Me.propentityslave(j) Then
                                If bc_am_ef_functions.entities(k).id = Me.oseltemplates.entity_ids(i) Then
                                    find_entity_in_template = Me.propentityslave(j)
                                    Exit Function
                                End If
                            End If
                        Next
                    Next
                End If
            End If
        Next
    End Function
    Private Sub data_dictionary()
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.lallitem.Items.Clear()
            Dim str As String
            Dim istr As String
            Dim outstr As String
            Dim entity As String

            REM find an entity of selected template

            entity = find_entity_in_template()
            Dim cclass As String
            cclass = oseltemplates.template_class_names(Me.ctemplate.SelectedIndex)

            str = "<excel_function>" + vbCr + _
           "<type>10000</type>" + vbCr + _
           "<class_id>" + cclass + "</class_id>" + vbCr + _
           "<entity_id>" + entity + "</entity_id>" + vbCr + _
           "<context>" + Me.ccontext.Text + "</context>" + vbCr + _
           "<section>" + Me.csection.Text + "</section>" + vbCr + _
           "<dimensions>name,type,factor,monetary,symbol</dimensions>" + vbCr + _
           "</excel_function>"

            Dim oef As New bc_am_excel_functions
            outstr = oef.execute_function(str)
            Me.lallitem.Items.Clear()

            If outstr <> "" And InStr(outstr, ";") = 0 Then
                Me.lallitem.Items.Add(outstr)
            Else
                REM chop it up
                Me.filter_item_ids.Clear()
                Me.filter_item_types.Clear()
                Me.filter_item_factor.Clear()
                Me.filter_item_monatary.Clear()
                Me.filter_item_symbol.Clear()
                Me.filter_item_assoc_class.Clear()
                While InStr(outstr, ";") > 0
                    istr = outstr.Substring(0, InStr(outstr, ";") - 1)
                    Me.lallitem.Items.Add(istr)
                    Me.filter_item_ids.Add(istr)
                    outstr = outstr.Substring(InStr(outstr, ";"), Len(outstr) - InStr(outstr, ";"))
                    istr = outstr.Substring(0, InStr(outstr, ";") - 1)
                    Me.filter_item_types.Add(istr)
                    outstr = outstr.Substring(InStr(outstr, ";"), Len(outstr) - InStr(outstr, ";"))
                    istr = outstr.Substring(0, InStr(outstr, ";") - 1)
                    Me.filter_item_factor.Add(istr)
                    outstr = outstr.Substring(InStr(outstr, ";"), Len(outstr) - InStr(outstr, ";"))
                    istr = outstr.Substring(0, InStr(outstr, ";") - 1)
                    Me.filter_item_monatary.Add(istr)
                    outstr = outstr.Substring(InStr(outstr, ";"), Len(outstr) - InStr(outstr, ";"))
                    istr = outstr.Substring(0, InStr(outstr, ";") - 1)
                    Me.filter_item_symbol.Add(istr)
                    outstr = outstr.Substring(InStr(outstr, ";"), Len(outstr) - InStr(outstr, ";"))
                    REM need to know if template is for prime or assoc
                    Me.filter_item_assoc_class.Add(cclass)

                End While

            End If
            Me.ball.Enabled = False
            If Me.lallitem.Items.Count > 0 Then
                Me.ball.Enabled = True
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_portfolio_wizard", "data_dictionary", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            Me.Cursor = Cursors.Default

        End Try

    End Sub

    Private Sub ltemplate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ltemplate.Click

    End Sub

    Private Sub ball_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ball.Click

        'allred
        Dim i As Integer = 0
        While Me.lallitem.Items.Count > 0 And i < MAX_ATTEMPTS
            Me.lallitem.SelectedIndex = 0
            set_item()
            i += 1
        End While
        Me.lallitem.SelectedIndex = -1

        If (i = MAX_ATTEMPTS) Then
            MsgBox("There was a problem adding the items.")
        End If

    End Sub

    Private Sub bdelall_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bdelall.Click
        For Each oItem As Object In lselitem.Items
            lallitem.Items.Add(oItem)
        Next
        Me.lselitem.Items.Clear()
        Me.item_sel_type.Clear()
        Me.item_sel_factor.Clear()
        Me.item_sel_monatary.Clear()
        Me.item_sel_symbol.Clear()
        Me.item_sel_assoc_class.Clear()
    End Sub
    Private Sub c_show_ea_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles c_show_ea.CheckedChanged
        set_desc()
    End Sub

    Private Sub Cyearstart_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cyearstart.SelectedIndexChanged
        Try
            If CInt(Me.Cyearstart.Text) > CInt(Me.cyearend.Text) Then
                Dim omsg As New bc_cs_message("Blue Curve", "Start year is greater than end year!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            Else
                set_desc()
            End If
        Catch

        End Try

    End Sub
    Private Sub Cyearend_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cyearend.SelectedIndexChanged
        Try
            If CInt(Me.Cyearstart.Text) > CInt(Me.cyearend.Text) Then
                Dim omsg As New bc_cs_message("Blue Curve", "Start year is greater than end year!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            Else
                set_desc()
            End If
        Catch

        End Try
    End Sub

    Private Sub custom_predefined_extract()
        Try
            Dim opt As New bc_om_pt_predefined_extract
            opt.sp_name = Me.oportfolios.portfolios(Me.Lpredefined.SelectedItems(0).Index).sp_name
            opt.schema = Me.cschema.Text
            If Me.lselentity.Items.Count > 0 Then
                opt.entity_name = Me.lselentity.Items(0)
            End If
            Dim oent As bc_om_entity
            For i = 0 To Me.lselentity.Items.Count - 1
                oent = New bc_om_entity
                oent.name = Me.lselentity.Items(i)
                opt.entity_list.Add(oent)
            Next
            opt.class_name = Me.cclass.Text
            opt.parent_class_name = Me.clink.Text
            If Me.Lselass.Items.Count > 0 Then
                opt.parent_entity_name = Me.Lselass.Items(0)
            End If
            For i = 0 To Me.Lselass.Items.Count - 1
                oent = New bc_om_entity
                oent.name = Me.lselentity.Items(i)
                opt.parent_entity_list.Add(oent)
            Next
            opt.stage = Me.cstage.Text
            opt.contributor = Me.Ccont.Text
            If Me.rold.Checked = True Then
                opt.date_from = "1-1-1900"
            Else
                opt.date_from = Me.DateTimePicker2.Value.Date + " 00:00:00"
            End If
            If Me.rnow.Checked = True Then
                opt.date_to = "9-9-9999"
            Else
                opt.date_to = Me.DateTimePicker1.Value.Date + " 23:59:00"
            End If
            For i = 0 To Me.lselitem.Items.Count - 1
                opt.items.Add(Me.lselitem.Items(i))
            Next
            opt.start_year = Me.Cyearstart.Text
            opt.end_year = Me.cyearend.Text

            For i = 0 To Me.lperiods.Items.Count - 1
                opt.items.Add(Me.lperiods.Items(i))
            Next
            opt.working_days = False
            If Me.Rworking.Checked = True Then
                opt.working_days = True
            End If
            opt.year_end_date = Me.DateTimePicker3.Value.Date + " 00:00:00"
            If Me.ccurr.SelectedIndex > -1 Then
                opt.currency = Me.ccurr.Text
            End If
            opt.param1 = Me.Cparam1.Text

            Me.ppropress.Visible = True
            tprogress.Start()

           

            opt.format_filename = Me.oportfolios.portfolios(Me.Lpredefined.SelectedItems(0).Index).format_filename

            Dim xml As String
            xml = opt.write_data_to_xml(Nothing)

            Me.ppropress.Increment(2)
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                opt.db_read()
            Else
                opt.tmode = bc_cs_soap_base_class.tREAD
                If opt.transmit_to_server_and_receive(opt, True) = False Then
                    Exit Sub
                End If
            End If
            If IsArray(opt.results) Then
                If output_custom_extract_results_to_excel(opt.results, Me.oportfolios.portfolios(Me.Lpredefined.SelectedItems(0).Index).format_filename, xml) = True Then
                    Me.Hide()
                End If
            Else
                Dim omsg As New bc_cs_message("Blue Curve", "No results for predefiend extract", bc_cs_message.MESSAGE, False, False, "yes", "no", True)
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_portfolio_wizard", "custom_predefined_extract", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Public Function output_custom_extract_results_to_excel(ByVal results As Object, ByVal format_filename As String, Optional ByVal xml As String = "") As Boolean
        Try
            tprogress = New System.Threading.Thread(AddressOf run_progress_bar)
            Dim fs As New bc_cs_file_transfer_services
            Dim oexcel As New bc_ao_in_excel(excelapp)
            If format_filename <> "" Then
                If Me.check_format_file_exists(format_filename) = True Then
                    REM load it
                    oexcel.disable_application_alerts()
                    oexcel.insert_sheet(format_filename, bc_cs_central_settings.local_template_path + format_filename)
                    oexcel.enable_application_alerts()
                Else
                    Dim omsg As New bc_cs_message("Blue Curve", "Format filename " + format_filename + "doesnt exist", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                End If
            End If
            excelapp.screenupdating = False

            Dim str As String
            Dim url As String

            For i = 0 To UBound(results, 2)
                str = CStr(results(0, i))
                If str.Length > 12 Then
                    If str.Substring(0, 10) = "[hyperlink" Then

                        url = str.Substring(10, InStr(str, "]") - 11)
                        excelapp.activeworkbook.activesheet.cells(results(1, i), results(2, i)).select()

                        Try
                            excelapp.activeworkbook.ActiveSheet.Hyperlinks.Add(Anchor:=excelapp.activeworkbook.activesheet.cells(results(1, i), results(2, i)), Address:=url, TextToDisplay:="aa")
                            results(0, i) = str.Substring(InStr(str, "]"), str.Length - InStr(str, "]"))

                        Catch ex As Exception
                            Dim ocomm As New bc_cs_activity_log("bc_am_portfolio_wizard", "output_custom_extract_results_to_excel", bc_cs_activity_codes.COMMENTARY, "cant set hyperlink: " + ex.Message)
                        End Try
                    End If
                End If

                If IsNumeric(results(0, i)) = True Then
                    excelapp.activeworkbook.activesheet.cells(results(1, i), results(2, i)).value = CDbl(results(0, i))
                ElseIf IsDate(results(0, i)) = True Then
                    excelapp.activeworkbook.activesheet.cells(results(1, i), results(2, i)).value = CDate(results(0, i))
                Else
                    excelapp.activeworkbook.activesheet.cells(results(1, i), results(2, i)).value = results(0, i)
                End If
                Try
                    excelapp.activeworkbook.activesheet.cells(results(1, i), results(2, i)).style = results(3, i)
                Catch

                End Try
            Next
            If xml <> "" Then
                set_parameter_property(xml)
            End If


            output_custom_extract_results_to_excel = True
        Catch ex As Exception
            output_custom_extract_results_to_excel = False
            Dim oerr As New bc_cs_error_log("bc_am_portfolio_wizard", " output_custom_extract_results_to_excel", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            excelapp.screenupdating = True
            If tprogress.ThreadState = Threading.ThreadState.Running Then
                tprogress.Abort()
            End If
        End Try
    End Function
    Public Sub set_parameter_property(ByVal xml As String)
        Dim sname, pname As String
        Dim i As Integer
        Dim co As Integer
        co = xml.Length / 250
        For i = 0 To co
            sname = excelapp.ActiveWorkbook.ActiveSheet.name
            sname = sname.Replace(" ", "_")
            pname = "rnet_extact" + sname + "_" + CStr(i)
            Try
                excelapp.ActiveWorkbook.CustomDocumentProperties.Add(pname, False, 4, "")
            Catch

            End Try
            If xml.Length >= 249 Then
                excelapp.ActiveWorkbook.CustomDocumentProperties(pname).value = xml.Substring(0, 250)
                xml = xml.Substring(250, xml.Length - 250)
            Else
                excelapp.ActiveWorkbook.CustomDocumentProperties(pname).value = xml
                Exit Sub
            End If
        Next

    End Sub
    



    Private Sub cformat_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cformat.CheckedChanged
        Me.cprectype.Visible = False
        Me.cprec.Visible = False
        Me.bok.Visible = True
        set_desc()
        If Me.cformat.Checked = True Then
            Me.bok.Visible = True
            Me.cprectype.Visible = True
            Me.cprectype.Text = "significant figures"
            Me.cprec.Text = 3
            Me.cprec.Visible = True
        End If
    End Sub

    Private Sub cprec_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cprec.SelectedIndexChanged
        If Me.cprec.SelectedIndex > -1 Then
            Me.bok.Visible = True
            set_desc()
        End If
    End Sub


    Private Sub cprectype_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cprectype.SelectedIndexChanged
        set_desc()
    End Sub

    Private Sub lallentity_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lallentity.SelectedIndexChanged
        Me.bselent.Enabled = False
        If Me.lallentity.SelectedIndex > -1 Then
            Me.bselent.Enabled = True
        End If
    End Sub
    Private Sub lallitem_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lallitem.SelectedIndexChanged
        Me.bselitem.Enabled = False
        If Me.lallitem.SelectedIndex > -1 Then
            Me.bselitem.Enabled = True
        End If
    End Sub


    Private Sub cleadenity_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cleadenity.SelectedIndexChanged
        If Me.cleadenity.SelectedIndex > -1 Then
            Me.twizard.TabPages(3).Enabled = True
            set_forward_enabled()
            set_desc()
            Me.lselentity.Items.Clear()
            Me.lselentity.Items.Add(Me.cleadenity.Text)
        End If
    End Sub

    Private Sub RadioButton2_CheckedChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rold.CheckedChanged
        If Me.rold.Checked = True Then
            Me.DateTimePicker2.Visible = False
        Else
            Me.DateTimePicker2.Visible = True
        End If
        set_desc()
    End Sub
    Public Function evaluate_num_time_series()
        Try
            Dim str As String
            Dim outstr As String
            Dim date_from As Date
            Dim date_to As Date
            Dim year As String
            Dim period As String = ""
            Dim i As Integer

            If Me.rold.Checked = True Then
                date_from = "1-1-1900"
            Else
                date_from = Me.DateTimePicker2.Value
            End If

            If Me.rnow.Checked = True Then
                date_to = "9-9-9999"
            Else
                date_to = Me.DateTimePicker1.Value
            End If

            year = Me.Cyearstart.Text
            If Me.lperiods.Items.Count > -1 Then
                period = Me.lperiods.Text
            End If

            str = "<excel_function>" + vbCr + _
            "<type>5003</type>" + vbCr + _
            "<class_id>" + Me.cclass.Text + "</class_id>" + vbCr + _
            "<entity_id>" + Me.lselentity.Items(0) + "</entity_id>" + vbCr + _
            "<item_id>" + Me.lselitem.Items(0) + "</item_id>" + vbCr + _
            "<stage_id>" + Me.cstage.Text + "</stage_id>" + vbCr + _
            "<contributor_id>" + Me.Ccont.Text + "</contributor_id>" + vbCr + _
            "<date_from>" + CStr(date_from) + "</date_from>" + vbCr + _
            "<date_to>" + CStr(date_to) + "</date_to>" + vbCr + _
            "<year>" + year + "</year>" + vbCr + _
            "<period>" + period + "</period>" + vbCr + _
            "<dimensions>date_from</dimensions>" + vbCr + _
            "</excel_function>"
            Dim oef As New bc_am_excel_functions
            outstr = oef.execute_function(str)
            evaluate_num_time_series = 0
            For i = 0 To outstr.Length - 1
                If outstr.Chars(i) = ";" Then
                    evaluate_num_time_series = evaluate_num_time_series + 1
                End If
            Next

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_portfolio_wizard", "evaluate_num_time_series", bc_cs_error_codes.USER_DEFINED, ex.Message)
            evaluate_num_time_series = Nothing
        End Try

    End Function

    Private Sub bc_am_portfolio_wizard_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        excelapp = Nothing
        Try
            tprogress.Abort()
        Catch
        End Try

    End Sub

    Private Sub rdatefrom_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdatefrom.CheckedChanged
        set_desc()
    End Sub

    Private Sub Rdate_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Rdate.CheckedChanged
        set_desc()
    End Sub

    Private Sub DateTimePicker2_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateTimePicker2.ValueChanged
        set_desc()
    End Sub
    Public Function check_formats_exists() As Boolean
        Dim i As Integer
        check_formats_exists = False
        For i = 1 To excelapp.ActiveWorkbook.Styles.Count
            If excelapp.ActiveWorkbook.Styles(i).Name = "bc_pt_title" Then
                check_formats_exists = True
                Exit Function
            End If
        Next
    End Function
    Public Function check_format_file_exists(ByVal fn As String) As Boolean

        Dim fs As New bc_cs_file_transfer_services
        check_format_file_exists = False
        If fs.check_document_exists(bc_cs_central_settings.local_template_path + fn) Then
            check_format_file_exists = True
        End If
    End Function

    Private Sub Title_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Title.Click

    End Sub

    Private Sub cschema_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cschema.SelectedIndexChanged
        Me.clink.Visible = False
        Me.clink.SelectedIndex = -1
        Me.cclass.SelectedIndex = -1

    End Sub

    Private Sub clink_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles clink.SelectedIndexChanged
        Me.ritemset.Checked = True
        Me.cass.Visible = False
        If Me.clink.SelectedIndex > 0 Then
            If Me.Rpredefined.Checked = True Then
                Me.cass.Visible = True
                Me.cass.TabPages(0).Text = Me.cclass.Text
                Me.cass.TabPages(1).Text = Me.clink.Text
                Me.cass.Enabled = False
            End If

            If chart_active = True Then
                'REM Steve Wooderson 05/02/2013 DA5
                'If primary and associated entity chosen, Allow user to select 1
                Me.cchartfocus.Items.Clear()
                Me.cchartfocus.Items.Add(Me.cclass.Text)
                Me.cchartfocus.Items.Add(Me.clink.Text)
                Me.cchartfocus.SelectedIndex = 0
                chart_focus = "Primary"
                Me.cchartfocus.Enabled = True
            Else
                Me.cchartfocus.Items.Clear()
                Me.cchartfocus.Items.Add(Me.cclass.Text)
                Me.cchartfocus.SelectedIndex = 0
            End If

        Else

            If chart_active = True Then
                Me.cchartfocus.Items.Clear()
                Me.cchartfocus.Items.Add(Me.cclass.Text)
                Me.cchartfocus.SelectedIndex = 0
                chart_focus = "Primary"
                Me.cchartfocus.Enabled = False
            Else
                Me.cchartfocus.Items.Clear()
                Me.cchartfocus.Items.Add(Me.cclass.Text)
                Me.cchartfocus.SelectedIndex = 0
            End If

            End If
            set_desc()
    End Sub

    Private Sub cand_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cand.CheckedChanged
        Me.cclass3.Visible = False
        Me.centity2.Visible = False
        Me.cclass3.SelectedIndex = -1
        Me.centity2.Items.Clear()
        set_desc()
        If Me.cand.Checked = True Then
            Me.cclass3.Visible = True
            Me.centity2.Visible = True
            Me.centity2.Enabled = False
        Else
            consolidate_entities()
        End If
    End Sub

    Private Sub cclass3_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cclass3.SelectedIndexChanged
        Try

            Me.Cursor = Cursors.WaitCursor
            If Me.cclass3.SelectedIndex = -1 Then
                Exit Sub
            End If
            Dim i, j As Integer
            Me.centity2.Items.Clear()
            For i = 0 To bc_am_ef_functions.class_names.Count - 1
                If bc_am_ef_functions.class_names(i) = Me.cclass3.Text Then
                    For j = 0 To bc_am_ef_functions.entities.Count - 1
                        Me.centity.Enabled = True
                        If bc_am_ef_functions.entities(j).class_id = bc_am_ef_functions.class_ids(i) Then
                            Me.centity2.Items.Add(bc_am_ef_functions.entities(j).name)
                        End If
                    Next
                    Exit For
                End If
            Next
            consolidate_entities()
            Me.centity2.Enabled = True
            tx_c = ""
            set_desc()
        Catch

        Finally
            Me.Cursor = Cursors.Default

        End Try

    End Sub

    Private Sub centity2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles centity2.SelectedIndexChanged
        consolidate_entities()

    End Sub
    Private Sub consolidate_entities()
        REM primary filter
        Dim i, j As Integer
        Dim list1 As New ArrayList
        Dim list2 As New ArrayList
        Dim outstr As String
        Dim istr As String
        Dim ty As String = ""
        'LoadEntities()
        Dim outid As Long
        Dim isInSearech As Boolean = False


        For i = 0 To bc_am_ef_functions.class_ids.Count - 1
            For j = 0 To Me.use_class_ids.Count - 1
                If Me.use_class_ids(j) = bc_am_ef_functions.class_ids(i) And bc_am_ef_functions.class_names(i) = Me.cclass2.Text Then
                    If Me.use_class_types(j) = "Parent" Then
                        ty = 5006
                    Else
                        ty = 5005
                    End If
                    Exit For
                End If
            Next
        Next
        outstr = load_propogating_entities(ty, Me.cclass2.Text, Me.centity.Text, Me.cclass.Text)
        If outstr <> "" And InStr(outstr, ";") = 0 Then
            list1.Add(outstr)
        Else
            REM chop it up
            While InStr(outstr, ";") > 0
                istr = outstr.Substring(0, InStr(outstr, ";") - 1)
                list1.Add(istr)
                outstr = outstr.Substring(InStr(outstr, ";"), Len(outstr) - InStr(outstr, ";"))
            End While
        End If

        REM secondary filter
        If Me.centity2.SelectedIndex > -1 Then
            For i = 0 To bc_am_ef_functions.class_ids.Count - 1
                For j = 0 To Me.use_class_ids.Count - 1
                    If Me.use_class_ids(j) = bc_am_ef_functions.class_ids(i) And bc_am_ef_functions.class_names(i) = Me.cclass3.Text Then
                        If Me.use_class_types(j) = "Parent" Then
                            ty = 5006
                        Else
                            ty = 5005
                        End If
                        Exit For
                    End If
                Next
            Next
            outstr = load_propogating_entities(ty, Me.cclass3.Text, Me.centity2.Text, Me.cclass.Text)
            If outstr <> "" And InStr(outstr, ";") = 0 Then
                list2.Add(outstr)
            Else
                REM chop it up
                While InStr(outstr, ";") > 0
                    istr = outstr.Substring(0, InStr(outstr, ";") - 1)
                    list2.Add(istr)
                    outstr = outstr.Substring(InStr(outstr, ";"), Len(outstr) - InStr(outstr, ";"))
                End While
            End If
        End If
        Me.lselentity.Items.Clear()
        If Me.centity2.SelectedIndex > -1 Then
            For i = 0 To list1.Count - 1
                For j = 0 To list2.Count - 1
                    If list1(i) = list2(j) Then

                        REM Check if in search
                        isInSearech = False
                        If BlueCurve_Unversefilter.SearchText <> "" Then
                            outid = 0
                            For x = 0 To bc_am_ef_functions.entities.Count - 1
                                If list1(i) = bc_am_ef_functions.entities(x).name Then
                                    outid = bc_am_ef_functions.entities(x).id
                                End If
                            Next
                            If outid <> 0 Then

                                If BlueCurve_Unversefilter.ShownEntityList.Contains(outid) = True Then
                                    isInSearech = True
                                End If
                                'For x = 0 To BlueCurve_Unversefilter.ShownEntityList.Count - 1
                                '    If outid = BlueCurve_Unversefilter.ShownEntityList(x) Then
                                '        isInSearech = True
                                '        Exit For
                                '    End If
                                'Next x
                            End If
                        Else
                            isInSearech = True
                        End If
                        If isInSearech = True Then
                            Me.lselentity.Items.Add(list2(j))
                        End If
                        Exit For
                    End If
                Next
            Next
        Else
            For i = 0 To list1.Count - 1

                REM Check if in search
                isInSearech = False
                If BlueCurve_Unversefilter.SearchText <> "" Then
                    outid = 0
                    For x = 0 To bc_am_ef_functions.entities.Count - 1
                        If list1(i) = bc_am_ef_functions.entities(x).name Then
                            outid = bc_am_ef_functions.entities(x).id
                        End If
                    Next
                    If outid <> 0 Then

                        If BlueCurve_Unversefilter.ShownEntityList.Contains(outid) = True Then
                            isInSearech = True
                        End If

                        'For x = 0 To BlueCurve_Unversefilter.ShownEntityList.Count - 1
                        '    If outid = BlueCurve_Unversefilter.ShownEntityList(x) Then
                        '        isInSearech = True
                        '        Exit For
                        '    End If
                        'Next x
                    End If
                Else
                    isInSearech = True
                End If

                If isInSearech = True Then
                    Me.lselentity.Items.Add(list1(i))
                End If
            Next
        End If
        set_desc()
    End Sub

    Private Sub lallitem_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lallitem.DoubleClick
        set_item()
        Me.lallitem.SelectedIndex = -1
    End Sub

    Private Sub lselitem_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lselitem.DoubleClick
        rem_item()
    End Sub

    Private Sub lallentity_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lallentity.DoubleClick
        set_entity()
    End Sub

    Private Sub lselentity_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lselentity.DoubleClick

        clear_entity()
    End Sub
    Private Sub lallass_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lallass.DoubleClick
        set_ass_entity()
    End Sub

    Private Sub lselass_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Lselass.DoubleClick
        clear_ass_entity()
    End Sub

    Private Sub lperiods_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lperiods.DoubleClick
        del_period()
    End Sub
    Private Sub cperiods_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles cperiods.DoubleClick
        add_period()
    End Sub

    Private Sub bentup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bentup.Click
        If lselentity.SelectedItems.Count <= 1 Then
            Dim tx As String
            Dim i As Integer
            Me.lselentity.Sorted = False
            tx = Me.lselentity.Text
            i = Me.lselentity.SelectedIndex
            Me.lselentity.Items.RemoveAt(Me.lselentity.SelectedIndex)
            Me.lselentity.Items.Insert(i - 1, tx)
            set_desc()
        End If
    End Sub

    Private Sub bentdn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bentdn.Click
        If lselentity.SelectedItems.Count <= 1 Then
            Me.lselentity.Sorted = False
            Dim tx As String
            Dim i As Integer
            Me.lselentity.Sorted = False
            tx = Me.lselentity.Text
            i = Me.lselentity.SelectedIndex
            Me.lselentity.Items.RemoveAt(Me.lselentity.SelectedIndex)
            Me.lselentity.Items.Insert(i + 1, tx)
            set_desc()
        End If
    End Sub
    Private Sub Rpredefined_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Rpredefined.CheckedChanged
        If Me.Rpredefined.Checked = True Then
            pre_defined_controls(True)
            Me.Popen.Visible = False
            Me.Ppredefined.Visible = True
            clear_down()
            REM disable other bits as required
            load_predefined_portfolios()
            Me.Popen.Visible = True
            Me.cfunction.Visible = False
        End If
    End Sub

    Private Sub Lpredefined_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Lpredefined.DoubleClick
        goto_tab()
    End Sub

    Private Sub ListView1_SelectedIndexChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Lpredefined.SelectedIndexChanged

        Me.Bopenpd.Enabled = False
        If Me.Lpredefined.SelectedItems.Count = 1 Then
            Me.Bopenpd.Enabled = True
            open_predefined_portfolio()
            set_desc()


        End If

    End Sub

    Private Sub Bopenpd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Bopenpd.Click
        open_predefined_portfolio()
        goto_tab()

    End Sub
    Private Sub goto_tab()
        Me.twizard.SelectedIndex = 2
        If Me.rchoose.Checked = False Then
            Me.twizard.SelectedIndex = 3
            If Me.lselitem.Enabled = False Then
                Me.twizard.SelectedIndex = 4
            End If
        End If
    End Sub

    Private Sub cass_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cass.SelectedIndexChanged
        Me.bselent.Enabled = False
        Me.brement.Enabled = False
        Me.Lselass.SelectedIndex = -1
        Me.lallass.SelectedIndex = -1
        Me.lselentity.SelectedIndex = -1
        Me.lallentity.SelectedIndex = -1
        Dim found As Boolean = False
        Select Case cass.SelectedIndex
            Case 0
                Me.lselentity.Visible = True
                Me.lallentity.Visible = True
                Me.Lselass.Visible = False
                Me.lallass.Visible = False
            Case 1
                REM list of all propogating entities for selecting entities
                Dim ty As Integer
                Dim istr, outstr As String
                Me.lallass.Items.Clear()
                ty = get_ef_id()

                For i = 0 To Me.lselentity.Items.Count - 1
                    REM get all entities of class
                    outstr = load_entities_of_class(Me.clink.Text)
                    REM get all entities of propgating class
                    'outstr = load_propogating_entities(ty, Me.cclass.Text, Me.lselentity.Items(i), Me.clink.Text)
                    If InStr(outstr, ";") = 0 Then
                        For j = 0 To Me.Lselass.Items.Count - 1
                            If Me.Lselass.Items(j) = outstr Then
                                found = True
                                Exit For
                            End If
                        Next
                        If found = False Then
                            Me.lallass.Items.Add(outstr)
                        End If
                    Else
                        While InStr(outstr, ";") > 0
                            istr = outstr.Substring(0, InStr(outstr, ";") - 1)
                            propentitymaster.Add(Me.lselentity.Items(i))
                            propentityslave.Add(istr)
                            outstr = outstr.Substring(InStr(outstr, ";"), Len(outstr) - InStr(outstr, ";"))
                            For j = 0 To Me.Lselass.Items.Count - 1
                                If Me.Lselass.Items(j) = istr Then
                                    found = True
                                    Exit For
                                End If
                            Next
                            If found = False Then
                                Me.lallass.Items.Add(istr)
                            End If
                        End While
                    End If
                Next
                Me.lselentity.Visible = False
                Me.lallentity.Visible = False
                Me.Lselass.Visible = True
                Me.lallass.Visible = True
                Me.twizard.SelectedIndex = 2
        End Select
    End Sub

    Private Sub lselentity_SelectedIndexChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lselentity.SelectedIndexChanged

    End Sub

    Private Sub lallass_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lallass.SelectedIndexChanged
        Me.bselent.Enabled = False
        If Me.lallass.SelectedItems.Count > 0 Then
            Me.bselent.Enabled = True
        End If
    End Sub

    Private Sub Lselass_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Lselass.SelectedIndexChanged
        Me.brement.Enabled = False
        Me.bentup.Enabled = False
        Me.bentdn.Enabled = False
        If Me.Lselass.SelectedIndex > -1 And Me.rchoose.Checked = True Then
            Me.brement.Enabled = True
        End If
    End Sub

    Private Sub Pyearenddate_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Pyearenddate.Paint

    End Sub

    Private Sub Cfentity_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cfentity.SelectedIndexChanged
        Try
            Me.Cursor = Cursors.WaitCursor

            LoadEntities()

        Catch

        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub Lfilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Lfilter.Click

    End Sub

    Private Sub Panel3_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs)

    End Sub

    Private Sub Panel3_Paint_1(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Panel3.Paint

    End Sub

    Private Sub Button1_Click_2(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If Me.Rpredefined.Checked = True Then
            If Me.oportfolios.portfolios(Me.Lpredefined.SelectedItems(0).Index).num_entities_select > 0 Then
                If Me.oportfolios.portfolios(Me.Lpredefined.SelectedItems(0).Index).num_entities_select < Me.lselentity.Items.Count + Me.lallentity.Items.Count Then
                    Dim omsg As New bc_cs_message("Blue Curve", "You can only set: " + CStr(Me.oportfolios.portfolios(Me.Lpredefined.SelectedItems(0).Index).num_entities_select) + " " + Me.cclass.Text + "(s) for this pre defined extract", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub
                End If
            End If
            Me.cass.Enabled = False
            If Me.lselentity.Items.Count = 0 Then
                Me.cass.Enabled = True
            End If
        End If
        For i = 0 To Me.lallentity.Items.Count - 1
            Me.lselentity.Items.Add(Me.lallentity.Items(i))
        Next
        set_forward_enabled()
        Me.twizard.TabPages(3).Enabled = True

        Me.lallentity.Items.Clear()
    End Sub

    Private Sub Rcalendar_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Rcalendar.CheckedChanged
        Me.rold.Enabled = True
        If Me.Rcalendar.Checked = True Then
            Me.rold.Enabled = False
            Me.rdatefrom.Checked = True
        End If
    End Sub

    Private Sub BlueCurve_Unversefilter_FireSearch(ByVal sender As Object, ByVal e As System.EventArgs) Handles BlueCurve_Unversefilter.FireSearch
        If Me.lallentity.Items.Count > 0 Then
            LoadEntities()
            If Me.lallentity.Items.Count > 0 Then
                Me.lallentity.SelectedIndex = 0
            End If
            Me.Cursor = Cursors.Default
        End If
    End Sub

    Private Sub cchart_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cchart.CheckedChanged
        set_desc()
    End Sub

    Private Sub cchartfocus_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cchartfocus.SelectedIndexChanged
        If Me.cchartfocus.SelectedIndex = 0 Then
            chart_focus = "Primary"
        Else
            chart_focus = "Associated"
        End If
    End Sub
End Class





