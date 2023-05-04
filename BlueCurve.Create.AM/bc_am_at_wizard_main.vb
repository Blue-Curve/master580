Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports BlueCurve.Core.AS
Imports System.Windows.Forms
Imports System.Collections
Imports System.Threading
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Diagnostics
Public Class bc_am_at_wizard_main
    Inherits bc_am_at_wizard_generic

    REM language dependent strings
    Public Const ALERT_POLL_INTERVAL = 300000
    'Public Const ALERT_POLL_INTERVAL = 20000
    Public form_name As String
    Public from_workflow As Boolean = False
    REM language dependent strings
    Public already_clicked As Boolean
    Public gpub_type_id As Integer
    Private gpub_type As New bc_om_pub_type
    Public gclass_id As Long
    Public bcprog As bc_cs_progress
    Public oopen_doc As bc_am_at_open_doc
    Private btoggle As Boolean
    Public blocal_docs As Boolean
    Public ball As Boolean
    Public attach_doc As Boolean = False
    Public attach_selected As Boolean = False
    Public Shared iselected_stage As String = "All Stages"
    Public Shared iselected_pub_type As String = "All Publication Types"
    Public Shared iselected_bus_area As String = "All Business Areas"
    Public loading As Boolean = False
    Public bsummary As Boolean
    Public bfrom_import As Boolean = False
    Public register_only As Boolean = False
    Public resetting As Boolean = False
    Public prev_mode As Integer
    Public prev_list_index As Integer
    Public alert_doc_id As Long
    Public prev_list_index2 As Integer
    Public prev_toggle As Boolean
    Public prev_child_category As Long
    Public entity_id As Long = 0
    Public has_params As Boolean
    Public all_string As String
    Public is_composite As Boolean
    Public bc_am_composite_doc As bc_am_composite_docs
    Public tsize As Object
    Public shown_entity_list As New ArrayList
    Public changed As Boolean
    Public odate_from As Date
    Public odate_to As Date
    Public ttsize As System.Drawing.Size
    Public gsettings As bc_om_create_settings
    Public col_1_def_width As System.Drawing.Size
    Public col_2_def_width As System.Drawing.Size
    Public col_3_def_width As System.Drawing.Size
    Public col_4_def_width As System.Drawing.Size
    Public col_5_def_width As System.Drawing.Size
    Public col_6_def_width As System.Drawing.Size
    Public col_7_def_width As System.Drawing.Size
    Public col_8_def_width As System.Drawing.Size
    Public tthread As Thread
    Public from_alert As Boolean = False
    Public suspend_poll As Boolean = False
    Public lxpos As Integer = 1
    Public lypos As Integer = 1
    Public regular_report As Boolean = False
    Public report_idx As Long
    Public from_create As Boolean = False
    Public sub_entity_id As Long = 0
    Public gentity_id As Long
    Public sub_entity_class As Long = 0
    Public gchild_category As Integer = 0
    Public last_screen As Boolean = False
    Friend WithEvents Chkclone As System.Windows.Forms.CheckBox

    Private entityBasedParams As Boolean
    Public params As New bc_om_parameters
    Public compositePubType As Integer
    Public buildMode As Integer
    Friend WithEvents BtnPlace1 As System.Windows.Forms.PictureBox
    Friend WithEvents BtnPlace2 As System.Windows.Forms.PictureBox
    Friend WithEvents BtnPlace3 As System.Windows.Forms.PictureBox
    Public clonePubType As Boolean


#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()
        Dim da As New TimeSpan
        Me.DateTimePicker1.Value = Now.Subtract(TimeSpan.FromDays(28))
        Me.DateTimePicker2.Value = Now
        Me.changed = False
        gsettings = New bc_om_create_settings
        col_1_def_width.Width = Me.ListView1.Columns(0).Width
        col_2_def_width.Width = Me.ListView1.Columns(1).Width
        col_3_def_width.Width = Me.ListView1.Columns(2).Width
        col_4_def_width.Width = Me.ListView1.Columns(3).Width
        col_5_def_width.Width = Me.ListView1.Columns(4).Width
        col_6_def_width.Width = Me.ListView1.Columns(5).Width
        col_7_def_width.Width = Me.ListView1.Columns(6).Width
        col_8_def_width.Width = Me.ListView1.Columns(7).Width
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
    Friend WithEvents description As System.Windows.Forms.Label
    Friend WithEvents ListBox1 As bc_cc_listbox
    Friend WithEvents RadioButton2 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton1 As System.Windows.Forms.RadioButton
    Public WithEvents ListView1 As System.Windows.Forms.ListView
    Friend WithEvents DocDate As System.Windows.Forms.ColumnHeader
    Friend WithEvents Stage As System.Windows.Forms.ColumnHeader
    Friend WithEvents Type As System.Windows.Forms.ColumnHeader
    Friend WithEvents title As System.Windows.Forms.ColumnHeader
    Friend WithEvents checkedout As System.Windows.Forms.ColumnHeader
    Friend WithEvents id As System.Windows.Forms.ColumnHeader
    Friend WithEvents Btncancel As System.Windows.Forms.PictureBox
    Friend WithEvents Btnback As System.Windows.Forms.PictureBox
    Friend WithEvents btnnext As System.Windows.Forms.PictureBox
    Friend WithEvents btnforwardgrey As System.Windows.Forms.PictureBox
    Friend WithEvents bleftgrey As System.Windows.Forms.PictureBox
    Friend WithEvents Btntoggle As System.Windows.Forms.Button
    Friend WithEvents proddesc As System.Windows.Forms.Label
    Friend WithEvents bcancelgrey As System.Windows.Forms.PictureBox
    Friend WithEvents lpubtypes As System.Windows.Forms.ComboBox
    Friend WithEvents lbusareas As System.Windows.Forms.ComboBox
    Friend WithEvents lblpubtypes As System.Windows.Forms.Label
    Friend WithEvents Lblbusarea As System.Windows.Forms.Label
    Friend WithEvents btnopen As System.Windows.Forms.Button
    Friend WithEvents btncan As System.Windows.Forms.Button
    Friend WithEvents lstages As System.Windows.Forms.ComboBox
    Friend WithEvents psortbytitle As System.Windows.Forms.PictureBox
    Friend WithEvents psortbydate As System.Windows.Forms.PictureBox
    Friend WithEvents psortbyauthor As System.Windows.Forms.PictureBox
    Friend WithEvents psummary As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox4 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox5 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox6 As System.Windows.Forms.PictureBox
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents txtsummary As System.Windows.Forms.RichTextBox
    Friend WithEvents bus_area As System.Windows.Forms.ColumnHeader
    Friend WithEvents pLabel1 As System.Windows.Forms.Label
    Friend WithEvents pLabel2 As System.Windows.Forms.Label
    Friend WithEvents pLabel3 As System.Windows.Forms.Label
    Friend WithEvents pLabel4 As System.Windows.Forms.Label
    Friend WithEvents pLabel5 As System.Windows.Forms.Label
    Friend WithEvents pLabel6 As System.Windows.Forms.Label
    Friend WithEvents ptext2 As System.Windows.Forms.TextBox
    Friend WithEvents ptext3 As System.Windows.Forms.TextBox
    Friend WithEvents ptext4 As System.Windows.Forms.TextBox
    Friend WithEvents ptext5 As System.Windows.Forms.TextBox
    Friend WithEvents ptext6 As System.Windows.Forms.TextBox
    Friend WithEvents ptext1 As System.Windows.Forms.TextBox
    Friend WithEvents bok As System.Windows.Forms.PictureBox
    Friend WithEvents bokgrey As System.Windows.Forms.PictureBox
    Friend WithEvents entityname As System.Windows.Forms.ColumnHeader
    Friend WithEvents btnup As System.Windows.Forms.Button
    Friend WithEvents btndown As System.Windows.Forms.Button
    Friend WithEvents btnremove As System.Windows.Forms.Button
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents DateTimePicker1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents DateTimePicker2 As System.Windows.Forms.DateTimePicker
    Friend WithEvents bedit As System.Windows.Forms.Button
    Friend WithEvents cpublish As System.Windows.Forms.CheckBox
    Friend WithEvents lpublish As System.Windows.Forms.Label
    Friend WithEvents chksettings As System.Windows.Forms.CheckBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents Chkalert As System.Windows.Forms.CheckBox
    Friend WithEvents bskip As System.Windows.Forms.Button
    Friend WithEvents BlueCurve_TextSearch1 As BlueCurve.Core.AS.BlueCurve_TextSearch
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_at_wizard_main))
        Me.description = New System.Windows.Forms.Label
        Me.RadioButton2 = New System.Windows.Forms.RadioButton
        Me.RadioButton1 = New System.Windows.Forms.RadioButton
        Me.ListBox1 = New BlueCurve.Core.[AS].bc_cc_listbox
        Me.ListView1 = New System.Windows.Forms.ListView
        Me.DocDate = New System.Windows.Forms.ColumnHeader
        Me.entityname = New System.Windows.Forms.ColumnHeader
        Me.Stage = New System.Windows.Forms.ColumnHeader
        Me.title = New System.Windows.Forms.ColumnHeader
        Me.Type = New System.Windows.Forms.ColumnHeader
        Me.checkedout = New System.Windows.Forms.ColumnHeader
        Me.id = New System.Windows.Forms.ColumnHeader
        Me.bus_area = New System.Windows.Forms.ColumnHeader
        Me.Btncancel = New System.Windows.Forms.PictureBox
        Me.Btnback = New System.Windows.Forms.PictureBox
        Me.btnnext = New System.Windows.Forms.PictureBox
        Me.btnforwardgrey = New System.Windows.Forms.PictureBox
        Me.bleftgrey = New System.Windows.Forms.PictureBox
        Me.Btntoggle = New System.Windows.Forms.Button
        Me.proddesc = New System.Windows.Forms.Label
        Me.bcancelgrey = New System.Windows.Forms.PictureBox
        Me.lstages = New System.Windows.Forms.ComboBox
        Me.lpubtypes = New System.Windows.Forms.ComboBox
        Me.lbusareas = New System.Windows.Forms.ComboBox
        Me.lblpubtypes = New System.Windows.Forms.Label
        Me.Lblbusarea = New System.Windows.Forms.Label
        Me.btnopen = New System.Windows.Forms.Button
        Me.btncan = New System.Windows.Forms.Button
        Me.psortbytitle = New System.Windows.Forms.PictureBox
        Me.psortbydate = New System.Windows.Forms.PictureBox
        Me.psortbyauthor = New System.Windows.Forms.PictureBox
        Me.psummary = New System.Windows.Forms.PictureBox
        Me.PictureBox4 = New System.Windows.Forms.PictureBox
        Me.PictureBox5 = New System.Windows.Forms.PictureBox
        Me.PictureBox6 = New System.Windows.Forms.PictureBox
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.txtsummary = New System.Windows.Forms.RichTextBox
        Me.pLabel1 = New System.Windows.Forms.Label
        Me.pLabel2 = New System.Windows.Forms.Label
        Me.pLabel3 = New System.Windows.Forms.Label
        Me.pLabel4 = New System.Windows.Forms.Label
        Me.pLabel5 = New System.Windows.Forms.Label
        Me.pLabel6 = New System.Windows.Forms.Label
        Me.ptext1 = New System.Windows.Forms.TextBox
        Me.ptext2 = New System.Windows.Forms.TextBox
        Me.ptext3 = New System.Windows.Forms.TextBox
        Me.ptext4 = New System.Windows.Forms.TextBox
        Me.ptext5 = New System.Windows.Forms.TextBox
        Me.ptext6 = New System.Windows.Forms.TextBox
        Me.bok = New System.Windows.Forms.PictureBox
        Me.bokgrey = New System.Windows.Forms.PictureBox
        Me.btnup = New System.Windows.Forms.Button
        Me.btndown = New System.Windows.Forms.Button
        Me.btnremove = New System.Windows.Forms.Button
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.DateTimePicker1 = New System.Windows.Forms.DateTimePicker
        Me.DateTimePicker2 = New System.Windows.Forms.DateTimePicker
        Me.bedit = New System.Windows.Forms.Button
        Me.cpublish = New System.Windows.Forms.CheckBox
        Me.lpublish = New System.Windows.Forms.Label
        Me.chksettings = New System.Windows.Forms.CheckBox
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.bskip = New System.Windows.Forms.Button
        Me.Chkalert = New System.Windows.Forms.CheckBox
        Me.BlueCurve_TextSearch1 = New BlueCurve.Core.[AS].BlueCurve_TextSearch
        Me.Chkclone = New System.Windows.Forms.CheckBox
        Me.BtnPlace1 = New System.Windows.Forms.PictureBox
        Me.BtnPlace2 = New System.Windows.Forms.PictureBox
        Me.BtnPlace3 = New System.Windows.Forms.PictureBox
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Pubthumbnail, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Btncancel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Btnback, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.btnnext, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.btnforwardgrey, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bleftgrey, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bcancelgrey, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.psortbytitle, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.psortbydate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.psortbyauthor, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.psummary, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bok, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bokgrey, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnPlace1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnPlace2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BtnPlace3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LabelConStr
        '
        Me.LabelConStr.Text = ""
        '
        'Label1
        '
        Me.Label1.Text = " "
        '
        'Label4
        '
        Me.Label4.Text = "Logged in as:"
        '
        'description
        '
        Me.description.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.description.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.description.ForeColor = System.Drawing.Color.Black
        Me.description.Location = New System.Drawing.Point(88, 80)
        Me.description.Name = "description"
        Me.description.Size = New System.Drawing.Size(248, 32)
        Me.description.TabIndex = 20
        Me.description.Text = "Please select one of the following options:"
        '
        'RadioButton2
        '
        Me.RadioButton2.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RadioButton2.Font = New System.Drawing.Font("Arial Unicode MS", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioButton2.Location = New System.Drawing.Point(88, 144)
        Me.RadioButton2.Name = "RadioButton2"
        Me.RadioButton2.Size = New System.Drawing.Size(136, 32)
        Me.RadioButton2.TabIndex = 19
        Me.RadioButton2.Text = "&Open Document"
        Me.RadioButton2.UseVisualStyleBackColor = False
        '
        'RadioButton1
        '
        Me.RadioButton1.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RadioButton1.Font = New System.Drawing.Font("Arial Unicode MS", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioButton1.ForeColor = System.Drawing.Color.Black
        Me.RadioButton1.Location = New System.Drawing.Point(88, 120)
        Me.RadioButton1.Name = "RadioButton1"
        Me.RadioButton1.Size = New System.Drawing.Size(136, 24)
        Me.RadioButton1.TabIndex = 1
        Me.RadioButton1.Text = "New &Document"
        Me.RadioButton1.UseVisualStyleBackColor = False
        '
        'ListBox1
        '
        Me.ListBox1.Font = New System.Drawing.Font("Arial Unicode MS", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListBox1.ItemHeight = 15
        Me.ListBox1.Location = New System.Drawing.Point(88, 112)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(232, 4)
        Me.ListBox1.TabIndex = 13
        '
        'ListView1
        '
        Me.ListView1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListView1.BackColor = System.Drawing.Color.White
        Me.ListView1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.DocDate, Me.entityname, Me.Stage, Me.title, Me.Type, Me.checkedout, Me.id, Me.bus_area})
        Me.ListView1.Font = New System.Drawing.Font("Arial Unicode MS", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListView1.FullRowSelect = True
        Me.ListView1.GridLines = True
        Me.ListView1.HideSelection = False
        Me.ListView1.Location = New System.Drawing.Point(88, 112)
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(440, 168)
        Me.ListView1.TabIndex = 25
        Me.ListView1.UseCompatibleStateImageBehavior = False
        Me.ListView1.View = System.Windows.Forms.View.Details
        '
        'DocDate
        '
        Me.DocDate.Text = "Date"
        Me.DocDate.Width = 65
        '
        'entityname
        '
        Me.entityname.Text = "Item"
        Me.entityname.Width = 65
        '
        'Stage
        '
        Me.Stage.Text = "Stage"
        Me.Stage.Width = 65
        '
        'title
        '
        Me.title.Text = "Title"
        Me.title.Width = 220
        '
        'Type
        '
        Me.Type.Text = "Type"
        Me.Type.Width = 65
        '
        'checkedout
        '
        Me.checkedout.Text = "Author"
        Me.checkedout.Width = 191
        '
        'id
        '
        Me.id.Text = "id"
        Me.id.Width = 1
        '
        'bus_area
        '
        Me.bus_area.Text = "Bus Area"
        '
        'Btncancel
        '
        Me.Btncancel.Image = CType(resources.GetObject("Btncancel.Image"), System.Drawing.Image)
        Me.Btncancel.Location = New System.Drawing.Point(417, 304)
        Me.Btncancel.Name = "Btncancel"
        Me.Btncancel.Size = New System.Drawing.Size(56, 56)
        Me.Btncancel.TabIndex = 31
        Me.Btncancel.TabStop = False
        '
        'Btnback
        '
        Me.Btnback.Image = CType(resources.GetObject("Btnback.Image"), System.Drawing.Image)
        Me.Btnback.Location = New System.Drawing.Point(352, 304)
        Me.Btnback.Name = "Btnback"
        Me.Btnback.Size = New System.Drawing.Size(56, 56)
        Me.Btnback.TabIndex = 30
        Me.Btnback.TabStop = False
        '
        'btnnext
        '
        Me.btnnext.Image = CType(resources.GetObject("btnnext.Image"), System.Drawing.Image)
        Me.btnnext.Location = New System.Drawing.Point(480, 304)
        Me.btnnext.Name = "btnnext"
        Me.btnnext.Size = New System.Drawing.Size(56, 56)
        Me.btnnext.TabIndex = 32
        Me.btnnext.TabStop = False
        '
        'btnforwardgrey
        '
        Me.btnforwardgrey.BackColor = System.Drawing.Color.Transparent
        Me.btnforwardgrey.Image = CType(resources.GetObject("btnforwardgrey.Image"), System.Drawing.Image)
        Me.btnforwardgrey.Location = New System.Drawing.Point(480, 304)
        Me.btnforwardgrey.Name = "btnforwardgrey"
        Me.btnforwardgrey.Size = New System.Drawing.Size(56, 56)
        Me.btnforwardgrey.TabIndex = 33
        Me.btnforwardgrey.TabStop = False
        '
        'bleftgrey
        '
        Me.bleftgrey.Image = CType(resources.GetObject("bleftgrey.Image"), System.Drawing.Image)
        Me.bleftgrey.Location = New System.Drawing.Point(354, 304)
        Me.bleftgrey.Name = "bleftgrey"
        Me.bleftgrey.Size = New System.Drawing.Size(56, 56)
        Me.bleftgrey.TabIndex = 34
        Me.bleftgrey.TabStop = False
        '
        'Btntoggle
        '
        Me.Btntoggle.BackColor = System.Drawing.SystemColors.ControlLight
        Me.Btntoggle.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Btntoggle.Location = New System.Drawing.Point(249, 259)
        Me.Btntoggle.Name = "Btntoggle"
        Me.Btntoggle.Size = New System.Drawing.Size(72, 24)
        Me.Btntoggle.TabIndex = 35
        Me.Btntoggle.Text = "&Toggle"
        Me.Btntoggle.UseVisualStyleBackColor = False
        '
        'proddesc
        '
        Me.proddesc.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.proddesc.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.proddesc.ForeColor = System.Drawing.Color.Black
        Me.proddesc.Location = New System.Drawing.Point(88, 264)
        Me.proddesc.Name = "proddesc"
        Me.proddesc.Size = New System.Drawing.Size(232, 16)
        Me.proddesc.TabIndex = 36
        Me.proddesc.Visible = False
        '
        'bcancelgrey
        '
        Me.bcancelgrey.Image = CType(resources.GetObject("bcancelgrey.Image"), System.Drawing.Image)
        Me.bcancelgrey.Location = New System.Drawing.Point(416, 304)
        Me.bcancelgrey.Name = "bcancelgrey"
        Me.bcancelgrey.Size = New System.Drawing.Size(56, 56)
        Me.bcancelgrey.TabIndex = 37
        Me.bcancelgrey.TabStop = False
        '
        'lstages
        '
        Me.lstages.Font = New System.Drawing.Font("Arial Unicode MS", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstages.Location = New System.Drawing.Point(89, 80)
        Me.lstages.Name = "lstages"
        Me.lstages.Size = New System.Drawing.Size(104, 24)
        Me.lstages.TabIndex = 38
        Me.lstages.Text = "All Stages"
        Me.ToolTip1.SetToolTip(Me.lstages, "Filter By A Particular Stage")
        '
        'lpubtypes
        '
        Me.lpubtypes.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lpubtypes.Font = New System.Drawing.Font("Arial Unicode MS", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lpubtypes.Location = New System.Drawing.Point(144, 304)
        Me.lpubtypes.Name = "lpubtypes"
        Me.lpubtypes.Size = New System.Drawing.Size(136, 24)
        Me.lpubtypes.TabIndex = 39
        Me.lpubtypes.Text = "All Publication Types"
        Me.ToolTip1.SetToolTip(Me.lpubtypes, "Filter By A Particular Publication Type")
        '
        'lbusareas
        '
        Me.lbusareas.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lbusareas.Font = New System.Drawing.Font("Arial Unicode MS", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbusareas.Location = New System.Drawing.Point(144, 325)
        Me.lbusareas.Name = "lbusareas"
        Me.lbusareas.Size = New System.Drawing.Size(136, 24)
        Me.lbusareas.TabIndex = 40
        Me.lbusareas.Text = "All Business Areas"
        Me.ToolTip1.SetToolTip(Me.lbusareas, "Filter By A Particular Business Area")
        '
        'lblpubtypes
        '
        Me.lblpubtypes.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblpubtypes.Font = New System.Drawing.Font("Arial Unicode MS", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblpubtypes.Location = New System.Drawing.Point(16, 304)
        Me.lblpubtypes.Name = "lblpubtypes"
        Me.lblpubtypes.Size = New System.Drawing.Size(120, 15)
        Me.lblpubtypes.TabIndex = 41
        Me.lblpubtypes.Text = "&Publication Type:"
        '
        'Lblbusarea
        '
        Me.Lblbusarea.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Lblbusarea.Font = New System.Drawing.Font("Arial Unicode MS", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Lblbusarea.Location = New System.Drawing.Point(16, 325)
        Me.Lblbusarea.Name = "Lblbusarea"
        Me.Lblbusarea.Size = New System.Drawing.Size(112, 19)
        Me.Lblbusarea.TabIndex = 42
        Me.Lblbusarea.Text = "&Business Area:"
        '
        'btnopen
        '
        Me.btnopen.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnopen.BackColor = System.Drawing.SystemColors.ControlLight
        Me.btnopen.Font = New System.Drawing.Font("Arial Unicode MS", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnopen.Location = New System.Drawing.Point(464, 304)
        Me.btnopen.Name = "btnopen"
        Me.btnopen.Size = New System.Drawing.Size(72, 21)
        Me.btnopen.TabIndex = 44
        Me.btnopen.Text = "&Open"
        Me.ToolTip1.SetToolTip(Me.btnopen, "Open The Selected Document(s)")
        Me.btnopen.UseVisualStyleBackColor = False
        '
        'btncan
        '
        Me.btncan.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btncan.BackColor = System.Drawing.SystemColors.ControlLight
        Me.btncan.Font = New System.Drawing.Font("Arial Unicode MS", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btncan.Location = New System.Drawing.Point(464, 330)
        Me.btncan.Name = "btncan"
        Me.btncan.Size = New System.Drawing.Size(72, 21)
        Me.btncan.TabIndex = 45
        Me.btncan.Text = "&Cancel"
        Me.ToolTip1.SetToolTip(Me.btncan, "Close the Form")
        Me.btncan.UseVisualStyleBackColor = False
        '
        'psortbytitle
        '
        Me.psortbytitle.Image = CType(resources.GetObject("psortbytitle.Image"), System.Drawing.Image)
        Me.psortbytitle.Location = New System.Drawing.Point(200, 80)
        Me.psortbytitle.Name = "psortbytitle"
        Me.psortbytitle.Size = New System.Drawing.Size(24, 24)
        Me.psortbytitle.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.psortbytitle.TabIndex = 46
        Me.psortbytitle.TabStop = False
        Me.ToolTip1.SetToolTip(Me.psortbytitle, "Sort List By Title")
        '
        'psortbydate
        '
        Me.psortbydate.Image = CType(resources.GetObject("psortbydate.Image"), System.Drawing.Image)
        Me.psortbydate.Location = New System.Drawing.Point(232, 80)
        Me.psortbydate.Name = "psortbydate"
        Me.psortbydate.Size = New System.Drawing.Size(24, 24)
        Me.psortbydate.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.psortbydate.TabIndex = 47
        Me.psortbydate.TabStop = False
        Me.psortbydate.Tag = ""
        Me.ToolTip1.SetToolTip(Me.psortbydate, "Sort List By Date")
        '
        'psortbyauthor
        '
        Me.psortbyauthor.Image = CType(resources.GetObject("psortbyauthor.Image"), System.Drawing.Image)
        Me.psortbyauthor.Location = New System.Drawing.Point(264, 80)
        Me.psortbyauthor.Name = "psortbyauthor"
        Me.psortbyauthor.Size = New System.Drawing.Size(24, 24)
        Me.psortbyauthor.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.psortbyauthor.TabIndex = 48
        Me.psortbyauthor.TabStop = False
        Me.psortbyauthor.Tag = ""
        Me.ToolTip1.SetToolTip(Me.psortbyauthor, "Sort List By Author")
        '
        'psummary
        '
        Me.psummary.Image = CType(resources.GetObject("psummary.Image"), System.Drawing.Image)
        Me.psummary.Location = New System.Drawing.Point(296, 80)
        Me.psummary.Name = "psummary"
        Me.psummary.Size = New System.Drawing.Size(24, 24)
        Me.psummary.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.psummary.TabIndex = 50
        Me.psummary.TabStop = False
        Me.psummary.Tag = ""
        Me.ToolTip1.SetToolTip(Me.psummary, "Show Document Summary")
        '
        'PictureBox4
        '
        Me.PictureBox4.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox4.Image = CType(resources.GetObject("PictureBox4.Image"), System.Drawing.Image)
        Me.PictureBox4.Location = New System.Drawing.Point(16, 80)
        Me.PictureBox4.Name = "PictureBox4"
        Me.PictureBox4.Size = New System.Drawing.Size(72, 62)
        Me.PictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox4.TabIndex = 51
        Me.PictureBox4.TabStop = False
        Me.ToolTip1.SetToolTip(Me.PictureBox4, "Show All Document I am Allowed To See")
        '
        'PictureBox5
        '
        Me.PictureBox5.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox5.Image = CType(resources.GetObject("PictureBox5.Image"), System.Drawing.Image)
        Me.PictureBox5.Location = New System.Drawing.Point(16, 149)
        Me.PictureBox5.Name = "PictureBox5"
        Me.PictureBox5.Size = New System.Drawing.Size(72, 62)
        Me.PictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox5.TabIndex = 52
        Me.PictureBox5.TabStop = False
        Me.ToolTip1.SetToolTip(Me.PictureBox5, "Show All Documents Authored By Me")
        '
        'PictureBox6
        '
        Me.PictureBox6.Image = CType(resources.GetObject("PictureBox6.Image"), System.Drawing.Image)
        Me.PictureBox6.Location = New System.Drawing.Point(16, 216)
        Me.PictureBox6.Name = "PictureBox6"
        Me.PictureBox6.Size = New System.Drawing.Size(72, 62)
        Me.PictureBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox6.TabIndex = 53
        Me.PictureBox6.TabStop = False
        Me.ToolTip1.SetToolTip(Me.PictureBox6, "Show Documents Stored Offline")
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "")
        '
        'txtsummary
        '
        Me.txtsummary.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtsummary.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtsummary.Location = New System.Drawing.Point(352, 112)
        Me.txtsummary.Name = "txtsummary"
        Me.txtsummary.Size = New System.Drawing.Size(192, 168)
        Me.txtsummary.TabIndex = 54
        Me.txtsummary.Text = ""
        '
        'pLabel1
        '
        Me.pLabel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.pLabel1.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pLabel1.Location = New System.Drawing.Point(96, 112)
        Me.pLabel1.Name = "pLabel1"
        Me.pLabel1.Size = New System.Drawing.Size(128, 16)
        Me.pLabel1.TabIndex = 55
        Me.pLabel1.Text = "pLabel1"
        Me.pLabel1.Visible = False
        '
        'pLabel2
        '
        Me.pLabel2.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.pLabel2.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pLabel2.Location = New System.Drawing.Point(96, 136)
        Me.pLabel2.Name = "pLabel2"
        Me.pLabel2.Size = New System.Drawing.Size(128, 16)
        Me.pLabel2.TabIndex = 56
        Me.pLabel2.Text = "pLabel2"
        '
        'pLabel3
        '
        Me.pLabel3.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.pLabel3.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pLabel3.Location = New System.Drawing.Point(96, 160)
        Me.pLabel3.Name = "pLabel3"
        Me.pLabel3.Size = New System.Drawing.Size(128, 16)
        Me.pLabel3.TabIndex = 57
        Me.pLabel3.Text = "pLabel3"
        '
        'pLabel4
        '
        Me.pLabel4.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.pLabel4.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pLabel4.Location = New System.Drawing.Point(96, 184)
        Me.pLabel4.Name = "pLabel4"
        Me.pLabel4.Size = New System.Drawing.Size(128, 16)
        Me.pLabel4.TabIndex = 58
        Me.pLabel4.Text = "pLabel4"
        Me.pLabel4.Visible = False
        '
        'pLabel5
        '
        Me.pLabel5.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.pLabel5.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pLabel5.Location = New System.Drawing.Point(96, 208)
        Me.pLabel5.Name = "pLabel5"
        Me.pLabel5.Size = New System.Drawing.Size(128, 16)
        Me.pLabel5.TabIndex = 59
        Me.pLabel5.Text = "pLabel5"
        Me.pLabel5.Visible = False
        '
        'pLabel6
        '
        Me.pLabel6.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.pLabel6.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pLabel6.Location = New System.Drawing.Point(96, 232)
        Me.pLabel6.Name = "pLabel6"
        Me.pLabel6.Size = New System.Drawing.Size(128, 16)
        Me.pLabel6.TabIndex = 60
        Me.pLabel6.Text = "pLabel6"
        Me.pLabel6.Visible = False
        '
        'ptext1
        '
        Me.ptext1.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ptext1.Location = New System.Drawing.Point(240, 112)
        Me.ptext1.Name = "ptext1"
        Me.ptext1.Size = New System.Drawing.Size(248, 18)
        Me.ptext1.TabIndex = 61
        '
        'ptext2
        '
        Me.ptext2.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ptext2.Location = New System.Drawing.Point(240, 136)
        Me.ptext2.Name = "ptext2"
        Me.ptext2.Size = New System.Drawing.Size(248, 18)
        Me.ptext2.TabIndex = 62
        '
        'ptext3
        '
        Me.ptext3.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ptext3.Location = New System.Drawing.Point(240, 160)
        Me.ptext3.Name = "ptext3"
        Me.ptext3.Size = New System.Drawing.Size(248, 18)
        Me.ptext3.TabIndex = 63
        '
        'ptext4
        '
        Me.ptext4.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ptext4.Location = New System.Drawing.Point(240, 184)
        Me.ptext4.Name = "ptext4"
        Me.ptext4.Size = New System.Drawing.Size(248, 18)
        Me.ptext4.TabIndex = 64
        '
        'ptext5
        '
        Me.ptext5.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ptext5.Location = New System.Drawing.Point(240, 208)
        Me.ptext5.Name = "ptext5"
        Me.ptext5.Size = New System.Drawing.Size(248, 18)
        Me.ptext5.TabIndex = 65
        '
        'ptext6
        '
        Me.ptext6.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ptext6.Location = New System.Drawing.Point(240, 232)
        Me.ptext6.Name = "ptext6"
        Me.ptext6.Size = New System.Drawing.Size(248, 18)
        Me.ptext6.TabIndex = 66
        '
        'bok
        '
        Me.bok.BackColor = System.Drawing.Color.Transparent
        Me.bok.Image = CType(resources.GetObject("bok.Image"), System.Drawing.Image)
        Me.bok.Location = New System.Drawing.Point(480, 304)
        Me.bok.Name = "bok"
        Me.bok.Size = New System.Drawing.Size(56, 56)
        Me.bok.TabIndex = 67
        Me.bok.TabStop = False
        '
        'bokgrey
        '
        Me.bokgrey.BackColor = System.Drawing.Color.Transparent
        Me.bokgrey.Image = CType(resources.GetObject("bokgrey.Image"), System.Drawing.Image)
        Me.bokgrey.Location = New System.Drawing.Point(480, 304)
        Me.bokgrey.Name = "bokgrey"
        Me.bokgrey.Size = New System.Drawing.Size(56, 56)
        Me.bokgrey.TabIndex = 68
        Me.bokgrey.TabStop = False
        '
        'btnup
        '
        Me.btnup.BackColor = System.Drawing.SystemColors.ControlLight
        Me.btnup.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnup.Location = New System.Drawing.Point(8, 128)
        Me.btnup.Name = "btnup"
        Me.btnup.Size = New System.Drawing.Size(72, 24)
        Me.btnup.TabIndex = 69
        Me.btnup.Text = "&Up"
        Me.btnup.UseVisualStyleBackColor = False
        '
        'btndown
        '
        Me.btndown.BackColor = System.Drawing.SystemColors.ControlLight
        Me.btndown.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btndown.Location = New System.Drawing.Point(8, 224)
        Me.btndown.Name = "btndown"
        Me.btndown.Size = New System.Drawing.Size(72, 24)
        Me.btndown.TabIndex = 70
        Me.btndown.Text = "&Down"
        Me.btndown.UseVisualStyleBackColor = False
        '
        'btnremove
        '
        Me.btnremove.BackColor = System.Drawing.SystemColors.ControlLight
        Me.btnremove.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnremove.Location = New System.Drawing.Point(8, 176)
        Me.btnremove.Name = "btnremove"
        Me.btnremove.Size = New System.Drawing.Size(72, 24)
        Me.btnremove.TabIndex = 71
        Me.btnremove.Text = "&Remove"
        Me.btnremove.UseVisualStyleBackColor = False
        '
        'Label8
        '
        Me.Label8.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label8.Font = New System.Drawing.Font("Arial Unicode MS", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(288, 306)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(72, 16)
        Me.Label8.TabIndex = 72
        Me.Label8.Text = "&Date From:"
        '
        'Label9
        '
        Me.Label9.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label9.Font = New System.Drawing.Font("Arial Unicode MS", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(288, 330)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(72, 15)
        Me.Label9.TabIndex = 73
        Me.Label9.Text = "&Date To:"
        '
        'DateTimePicker1
        '
        Me.DateTimePicker1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.DateTimePicker1.Location = New System.Drawing.Point(368, 304)
        Me.DateTimePicker1.Name = "DateTimePicker1"
        Me.DateTimePicker1.Size = New System.Drawing.Size(88, 20)
        Me.DateTimePicker1.TabIndex = 74
        Me.ToolTip1.SetToolTip(Me.DateTimePicker1, "Start Date Of Range Of Documents To Retrieve")
        '
        'DateTimePicker2
        '
        Me.DateTimePicker2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.DateTimePicker2.Location = New System.Drawing.Point(368, 325)
        Me.DateTimePicker2.Name = "DateTimePicker2"
        Me.DateTimePicker2.Size = New System.Drawing.Size(88, 20)
        Me.DateTimePicker2.TabIndex = 75
        Me.ToolTip1.SetToolTip(Me.DateTimePicker2, "End Date Of Range Of Documents To Retrieve")
        '
        'bedit
        '
        Me.bedit.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bedit.BackColor = System.Drawing.SystemColors.ControlLight
        Me.bedit.Font = New System.Drawing.Font("Arial Unicode MS", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bedit.Location = New System.Drawing.Point(501, 114)
        Me.bedit.Name = "bedit"
        Me.bedit.Size = New System.Drawing.Size(41, 23)
        Me.bedit.TabIndex = 76
        Me.bedit.Text = "&Edit"
        Me.ToolTip1.SetToolTip(Me.bedit, "Edit The Categorisation Data For The Document")
        Me.bedit.UseVisualStyleBackColor = False
        '
        'cpublish
        '
        Me.cpublish.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cpublish.Location = New System.Drawing.Point(290, 349)
        Me.cpublish.Name = "cpublish"
        Me.cpublish.Size = New System.Drawing.Size(14, 22)
        Me.cpublish.TabIndex = 77
        Me.ToolTip1.SetToolTip(Me.cpublish, "Show/Hide Publish Documents. If Your User Security Allows This")
        Me.cpublish.Visible = False
        '
        'lpublish
        '
        Me.lpublish.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lpublish.BackColor = System.Drawing.Color.White
        Me.lpublish.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lpublish.Location = New System.Drawing.Point(309, 349)
        Me.lpublish.Name = "lpublish"
        Me.lpublish.Size = New System.Drawing.Size(64, 15)
        Me.lpublish.TabIndex = 78
        Me.lpublish.Text = "Show Publish"
        Me.lpublish.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.ToolTip1.SetToolTip(Me.lpublish, "Show/Hide Publish Documents. If Your User Security Allows This")
        Me.lpublish.Visible = False
        '
        'chksettings
        '
        Me.chksettings.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chksettings.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chksettings.Location = New System.Drawing.Point(2, 350)
        Me.chksettings.Name = "chksettings"
        Me.chksettings.Size = New System.Drawing.Size(142, 14)
        Me.chksettings.TabIndex = 79
        Me.chksettings.Text = "Restore Default Form Settinngs"
        Me.chksettings.Visible = False
        '
        'ToolTip1
        '
        Me.ToolTip1.AutomaticDelay = 50
        Me.ToolTip1.ShowAlways = True
        '
        'bskip
        '
        Me.bskip.BackColor = System.Drawing.SystemColors.ControlLight
        Me.bskip.Font = New System.Drawing.Font("Arial Unicode MS", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bskip.Location = New System.Drawing.Point(176, 264)
        Me.bskip.Name = "bskip"
        Me.bskip.Size = New System.Drawing.Size(72, 24)
        Me.bskip.TabIndex = 81
        Me.bskip.Text = "&Skip"
        Me.ToolTip1.SetToolTip(Me.bskip, "Open The Selected Document(s)")
        Me.bskip.UseVisualStyleBackColor = False
        Me.bskip.Visible = False
        '
        'Chkalert
        '
        Me.Chkalert.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Chkalert.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!)
        Me.Chkalert.Location = New System.Drawing.Point(144, 351)
        Me.Chkalert.Name = "Chkalert"
        Me.Chkalert.Size = New System.Drawing.Size(87, 17)
        Me.Chkalert.TabIndex = 80
        Me.Chkalert.Text = "Alerter"
        '
        'BlueCurve_TextSearch1
        '
        Me.BlueCurve_TextSearch1.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.BlueCurve_TextSearch1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.BlueCurve_TextSearch1.ExcludeControl = Nothing
        Me.BlueCurve_TextSearch1.Location = New System.Drawing.Point(16, 259)
        Me.BlueCurve_TextSearch1.Margin = New System.Windows.Forms.Padding(4)
        Me.BlueCurve_TextSearch1.MaximumSize = New System.Drawing.Size(30000, 30000)
        Me.BlueCurve_TextSearch1.MinimumSize = New System.Drawing.Size(150, 21)
        Me.BlueCurve_TextSearch1.Name = "BlueCurve_TextSearch1"
        Me.BlueCurve_TextSearch1.SearchAttributeList = ""
        Me.BlueCurve_TextSearch1.SearchAttributes = 0
        Me.BlueCurve_TextSearch1.SearchBuildEntitiesOnly = False
        Me.BlueCurve_TextSearch1.SearchClass = 0
        Me.BlueCurve_TextSearch1.SearchControl = "ListBox1"
        Me.BlueCurve_TextSearch1.SearchCurrentAttribute = Nothing
        Me.BlueCurve_TextSearch1.SearchFireEventOnly = False
        Me.BlueCurve_TextSearch1.SearchGetInactive = False
        Me.BlueCurve_TextSearch1.SearchText = ""
        Me.BlueCurve_TextSearch1.SearchTimerOff = False
        Me.BlueCurve_TextSearch1.SearchUserEntitiesOnly = False
        Me.BlueCurve_TextSearch1.showinactive = -1
        Me.BlueCurve_TextSearch1.Size = New System.Drawing.Size(230, 24)
        Me.BlueCurve_TextSearch1.TabIndex = 82
        '
        'Chkclone
        '
        Me.Chkclone.AutoSize = True
        Me.Chkclone.Font = New System.Drawing.Font("Verdana", 8.25!)
        Me.Chkclone.Location = New System.Drawing.Point(200, 303)
        Me.Chkclone.Name = "Chkclone"
        Me.Chkclone.Size = New System.Drawing.Size(207, 17)
        Me.Chkclone.TabIndex = 83
        Me.Chkclone.Text = "Use previously published report"
        Me.Chkclone.UseVisualStyleBackColor = True
        Me.Chkclone.Visible = False
        '
        'BtnPlace1
        '
        Me.BtnPlace1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BtnPlace1.BackColor = System.Drawing.Color.Transparent
        Me.BtnPlace1.Enabled = False
        Me.BtnPlace1.Image = CType(resources.GetObject("BtnPlace1.Image"), System.Drawing.Image)
        Me.BtnPlace1.Location = New System.Drawing.Point(352, 304)
        Me.BtnPlace1.Name = "BtnPlace1"
        Me.BtnPlace1.Size = New System.Drawing.Size(56, 56)
        Me.BtnPlace1.TabIndex = 87
        Me.BtnPlace1.TabStop = False
        Me.BtnPlace1.Visible = False
        '
        'BtnPlace2
        '
        Me.BtnPlace2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BtnPlace2.BackColor = System.Drawing.Color.Transparent
        Me.BtnPlace2.Enabled = False
        Me.BtnPlace2.Image = CType(resources.GetObject("BtnPlace2.Image"), System.Drawing.Image)
        Me.BtnPlace2.Location = New System.Drawing.Point(418, 304)
        Me.BtnPlace2.Name = "BtnPlace2"
        Me.BtnPlace2.Size = New System.Drawing.Size(56, 56)
        Me.BtnPlace2.TabIndex = 88
        Me.BtnPlace2.TabStop = False
        Me.BtnPlace2.Visible = False
        '
        'BtnPlace3
        '
        Me.BtnPlace3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BtnPlace3.BackColor = System.Drawing.Color.Transparent
        Me.BtnPlace3.Enabled = False
        Me.BtnPlace3.Image = CType(resources.GetObject("BtnPlace3.Image"), System.Drawing.Image)
        Me.BtnPlace3.Location = New System.Drawing.Point(480, 304)
        Me.BtnPlace3.Name = "BtnPlace3"
        Me.BtnPlace3.Size = New System.Drawing.Size(56, 56)
        Me.BtnPlace3.TabIndex = 89
        Me.BtnPlace3.TabStop = False
        Me.BtnPlace3.Visible = False
        '
        'bc_am_at_wizard_main
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(546, 368)
        Me.Controls.Add(Me.BtnPlace3)
        Me.Controls.Add(Me.BtnPlace2)
        Me.Controls.Add(Me.BtnPlace1)
        Me.Controls.Add(Me.Chkclone)
        Me.Controls.Add(Me.BlueCurve_TextSearch1)
        Me.Controls.Add(Me.bskip)
        Me.Controls.Add(Me.Chkalert)
        Me.Controls.Add(Me.chksettings)
        Me.Controls.Add(Me.lpublish)
        Me.Controls.Add(Me.cpublish)
        Me.Controls.Add(Me.bedit)
        Me.Controls.Add(Me.DateTimePicker2)
        Me.Controls.Add(Me.DateTimePicker1)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.btnremove)
        Me.Controls.Add(Me.btndown)
        Me.Controls.Add(Me.btnup)
        Me.Controls.Add(Me.PictureBox5)
        Me.Controls.Add(Me.btncan)
        Me.Controls.Add(Me.btnopen)
        Me.Controls.Add(Me.bokgrey)
        Me.Controls.Add(Me.bok)
        Me.Controls.Add(Me.ptext6)
        Me.Controls.Add(Me.ptext5)
        Me.Controls.Add(Me.ptext4)
        Me.Controls.Add(Me.ptext3)
        Me.Controls.Add(Me.ptext2)
        Me.Controls.Add(Me.ptext1)
        Me.Controls.Add(Me.pLabel6)
        Me.Controls.Add(Me.pLabel5)
        Me.Controls.Add(Me.pLabel4)
        Me.Controls.Add(Me.pLabel3)
        Me.Controls.Add(Me.pLabel2)
        Me.Controls.Add(Me.pLabel1)
        Me.Controls.Add(Me.txtsummary)
        Me.Controls.Add(Me.ListView1)
        Me.Controls.Add(Me.PictureBox6)
        Me.Controls.Add(Me.PictureBox4)
        Me.Controls.Add(Me.psummary)
        Me.Controls.Add(Me.psortbyauthor)
        Me.Controls.Add(Me.psortbydate)
        Me.Controls.Add(Me.psortbytitle)
        Me.Controls.Add(Me.Lblbusarea)
        Me.Controls.Add(Me.lblpubtypes)
        Me.Controls.Add(Me.lbusareas)
        Me.Controls.Add(Me.lpubtypes)
        Me.Controls.Add(Me.lstages)
        Me.Controls.Add(Me.bcancelgrey)
        Me.Controls.Add(Me.proddesc)
        Me.Controls.Add(Me.Btntoggle)
        Me.Controls.Add(Me.bleftgrey)
        Me.Controls.Add(Me.btnforwardgrey)
        Me.Controls.Add(Me.btnnext)
        Me.Controls.Add(Me.Btncancel)
        Me.Controls.Add(Me.Btnback)
        Me.Controls.Add(Me.description)
        Me.Controls.Add(Me.RadioButton2)
        Me.Controls.Add(Me.RadioButton1)
        Me.Controls.Add(Me.ListBox1)
        Me.Name = "bc_am_at_wizard_main"
        Me.Text = " Blue Curve create - Author Tool"
        Me.Controls.SetChildIndex(Me.ListBox1, 0)
        Me.Controls.SetChildIndex(Me.RadioButton1, 0)
        Me.Controls.SetChildIndex(Me.RadioButton2, 0)
        Me.Controls.SetChildIndex(Me.description, 0)
        Me.Controls.SetChildIndex(Me.Btnback, 0)
        Me.Controls.SetChildIndex(Me.Btncancel, 0)
        Me.Controls.SetChildIndex(Me.btnnext, 0)
        Me.Controls.SetChildIndex(Me.btnforwardgrey, 0)
        Me.Controls.SetChildIndex(Me.bleftgrey, 0)
        Me.Controls.SetChildIndex(Me.Btntoggle, 0)
        Me.Controls.SetChildIndex(Me.proddesc, 0)
        Me.Controls.SetChildIndex(Me.bcancelgrey, 0)
        Me.Controls.SetChildIndex(Me.lstages, 0)
        Me.Controls.SetChildIndex(Me.lpubtypes, 0)
        Me.Controls.SetChildIndex(Me.lbusareas, 0)
        Me.Controls.SetChildIndex(Me.lblpubtypes, 0)
        Me.Controls.SetChildIndex(Me.Lblbusarea, 0)
        Me.Controls.SetChildIndex(Me.psortbytitle, 0)
        Me.Controls.SetChildIndex(Me.psortbydate, 0)
        Me.Controls.SetChildIndex(Me.psortbyauthor, 0)
        Me.Controls.SetChildIndex(Me.psummary, 0)
        Me.Controls.SetChildIndex(Me.PictureBox4, 0)
        Me.Controls.SetChildIndex(Me.PictureBox6, 0)
        Me.Controls.SetChildIndex(Me.ListView1, 0)
        Me.Controls.SetChildIndex(Me.txtsummary, 0)
        Me.Controls.SetChildIndex(Me.pLabel1, 0)
        Me.Controls.SetChildIndex(Me.pLabel2, 0)
        Me.Controls.SetChildIndex(Me.pLabel3, 0)
        Me.Controls.SetChildIndex(Me.pLabel4, 0)
        Me.Controls.SetChildIndex(Me.pLabel5, 0)
        Me.Controls.SetChildIndex(Me.pLabel6, 0)
        Me.Controls.SetChildIndex(Me.ptext1, 0)
        Me.Controls.SetChildIndex(Me.ptext2, 0)
        Me.Controls.SetChildIndex(Me.ptext3, 0)
        Me.Controls.SetChildIndex(Me.ptext4, 0)
        Me.Controls.SetChildIndex(Me.ptext5, 0)
        Me.Controls.SetChildIndex(Me.ptext6, 0)
        Me.Controls.SetChildIndex(Me.bok, 0)
        Me.Controls.SetChildIndex(Me.bokgrey, 0)
        Me.Controls.SetChildIndex(Me.btnopen, 0)
        Me.Controls.SetChildIndex(Me.btncan, 0)
        Me.Controls.SetChildIndex(Me.PictureBox5, 0)
        Me.Controls.SetChildIndex(Me.btnup, 0)
        Me.Controls.SetChildIndex(Me.btndown, 0)
        Me.Controls.SetChildIndex(Me.btnremove, 0)
        Me.Controls.SetChildIndex(Me.Label8, 0)
        Me.Controls.SetChildIndex(Me.Label9, 0)
        Me.Controls.SetChildIndex(Me.DateTimePicker1, 0)
        Me.Controls.SetChildIndex(Me.DateTimePicker2, 0)
        Me.Controls.SetChildIndex(Me.bedit, 0)
        Me.Controls.SetChildIndex(Me.cpublish, 0)
        Me.Controls.SetChildIndex(Me.lpublish, 0)
        Me.Controls.SetChildIndex(Me.chksettings, 0)
        Me.Controls.SetChildIndex(Me.Chkalert, 0)
        Me.Controls.SetChildIndex(Me.bskip, 0)
        Me.Controls.SetChildIndex(Me.BlueCurve_TextSearch1, 0)
        Me.Controls.SetChildIndex(Me.Chkclone, 0)
        Me.Controls.SetChildIndex(Me.BtnPlace1, 0)
        Me.Controls.SetChildIndex(Me.BtnPlace2, 0)
        Me.Controls.SetChildIndex(Me.BtnPlace3, 0)
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Pubthumbnail, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Btncancel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Btnback, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.btnnext, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.btnforwardgrey, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bleftgrey, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bcancelgrey, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.psortbytitle, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.psortbydate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.psortbyauthor, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.psummary, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bok, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bokgrey, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnPlace1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnPlace2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BtnPlace3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private Sub bc_am_at_wizard_main_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        'Exit Sub
        'If Me.tsize <> Nothing Then
        '    If Me.Width <> Me.tsize.width Then
        '        Me.Size = Me.tsize
        '    End If
        'End If
        ' stop resizing when a certain size is reached
        If Me.Width < 552 Then
            Me.Width = 552
            Exit Sub
        End If

        If Me.Height < 400 Then
            Me.Height = 400
            Exit Sub
        End If

    End Sub

    Private Sub bc_am_at_wizard_main_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
           
            bc_am_at_wizard_generic.frm_loaded = True

            REM load lanaguage dependent strings for form
            Dim i As Integer
            Me.lxpos = Me.Location.X
            Me.lypos = Me.Location.Y
            tsize = Me.Size
            form_name = "create.open"
            For i = 0 To bc_am_load_objects.obc_prefs.language_strings.forms.Count - 1
                If bc_am_load_objects.obc_prefs.language_strings.forms(i).form_name = form_name Then
                    form_strings = bc_am_load_objects.obc_prefs.language_strings.forms(i)
                    Exit For
                End If
            Next

            REM this is slowing things down
            BlueCurve_TextSearch1.SearchSetup(False)

            For i = 0 To form_strings.form_labels.Count - 1
                If form_strings.form_labels(i).label_code = "create.open.title" Then
                    Me.ListView1.Columns(0).Text = form_strings.form_labels(i).text
                End If
                If form_strings.form_labels(i).label_code = "create.open.item" Then
                    Me.ListView1.Columns(1).Text = form_strings.form_labels(i).text
                End If
                If form_strings.form_labels(i).label_code = "create.open.date" Then
                    Me.ListView1.Columns(2).Text = form_strings.form_labels(i).text
                End If
                If form_strings.form_labels(i).label_code = "create.open.author" Then
                    Me.ListView1.Columns(5).Text = form_strings.form_labels(i).text
                End If
                If form_strings.form_labels(i).label_code = "create.open.pubtype" Then
                    Me.lblpubtypes.Text() = form_strings.form_labels(i).text
                End If
                If form_strings.form_labels(i).label_code = "create.open.busarea" Then
                    Me.Lblbusarea.Text() = form_strings.form_labels(i).text
                End If
                If form_strings.form_labels(i).label_code = "create.open.open" Then
                    Me.btnopen.Text() = form_strings.form_labels(i).text
                End If
                If form_strings.form_labels(i).label_code = "create.open.cancel" Then
                    Me.btncan.Text() = form_strings.form_labels(i).text
                End If
                If form_strings.form_labels(i).label_code = "create.open.all" Then
                    all_string = form_strings.form_labels(i).text
                End If
            Next

            form_name = "create.main"
            For i = 0 To bc_am_load_objects.obc_prefs.language_strings.forms.Count - 1
                If bc_am_load_objects.obc_prefs.language_strings.forms(i).form_name = form_name Then
                    form_strings = bc_am_load_objects.obc_prefs.language_strings.forms(i)
                    Exit For
                End If
            Next
            REM if service aleters on then client alerter always on

            If Me.from_alert = True Then
                centre_form()
                Me.WindowState = FormWindowState.Normal
                'Me.StartPosition = FormStartPosition.CenterScreen
                Exit Sub
            End If

            load_start()

            If Me.from_workflow = True Then
                load_start()
                load_build_wizard()
                If Me.regular_report = True Then
                    mode = 200
                    load_regular_reports()
                    Me.btnforwardgrey.Visible = False
                    Me.bok.Left = BtnPlace3.Left
                    Me.bok.Visible = True
                    Me.last_screen = True
                Else
                    mode = 1
                    load_pub_type()
                End If
                REM entity
                Me.bleftgrey.Visible = False
                Me.Btnback.Visible = False
                'Me.Btncancel.Left = 416
                'Me.bcancelgrey.Left = 416
                Me.Btncancel.Left = BtnPlace2.Left
                Me.bcancelgrey.Left = BtnPlace2.Left
                Me.Btncancel.Visible = False
                Me.bcancelgrey.Enabled = True
                Me.bcancelgrey.Visible = True
            End If


            For i = 0 To form_strings.form_labels.Count - 1
                If form_strings.form_labels(i).label_code = "create.main.newdoc" Then
                    Me.RadioButton1.Text() = form_strings.form_labels(i).text
                End If
                If form_strings.form_labels(i).label_code = "create.main.opendoc" Then
                    Me.RadioButton2.Text() = form_strings.form_labels(i).text
                End If
            Next

        Catch

        Finally

            PutFormOnTop()

        End Try


    End Sub
    Private Sub ListBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListBox1.SelectedIndexChanged
        index_changed()
    End Sub
    REM EFG June 2010
    Private Sub index_changed()

        Dim i As Integer

        Dim l, m, n As Integer
        Dim found As Boolean = False
        Me.bleftgrey.Left = BtnPlace2.Left
        Me.bleftgrey.Visible = True
        If mode = 1 Then
            Me.Chkclone.Visible = False

            For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                If Me.ListBox1.Text = bc_am_load_objects.obc_pub_types.pubtype(i).name Then
                    Me.proddesc.Text = bc_am_load_objects.obc_pub_types.pubtype(i).description()
                    If Me.bfrom_import = False And bc_am_load_objects.obc_pub_types.pubtype(i).is_clonable And bc_cs_central_settings.selected_conn_method <> bc_cs_central_settings.LOCAL Then
                        Me.Chkclone.Visible = True
                        Me.Chkclone.Checked = False
                    End If
                    Exit For
                End If
            Next i
            set_image()
            For l = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                REM filter pub types for users bus area
                If Me.from_workflow = False Or Me.from_create = True Then
                    If bc_am_load_objects.obc_pub_types.pubtype(l).show_in_wizard = True Then
                        For m = 0 To bc_am_load_objects.obc_prefs.bus_areas.Count - 1
                            If bc_am_load_objects.obc_prefs.bus_areas(m) = bc_am_load_objects.obc_pub_types.pubtype(l).bus_area_id Then
                                If n = Me.ListBox1.SelectedIndex Then
                                    gpub_type_id = bc_am_load_objects.obc_pub_types.pubtype(l).id
                                    found = True
                                    Exit For
                                End If
                                n = n + 1
                            End If
                        Next
                        If found = True Then
                            Exit For
                        End If
                    End If
                Else
                    If bc_am_load_objects.obc_pub_types.pubtype(l).create_only = False Then
                        For m = 0 To bc_am_load_objects.obc_prefs.bus_areas.Count - 1
                            If bc_am_load_objects.obc_prefs.bus_areas(m) = bc_am_load_objects.obc_pub_types.pubtype(l).bus_area_id Then
                                If n = Me.ListBox1.SelectedIndex Then
                                    gpub_type_id = bc_am_load_objects.obc_pub_types.pubtype(l).id
                                    found = True
                                    Exit For
                                End If
                                n = n + 1
                            End If
                        Next
                        If found = True Then
                            Exit For
                        End If
                    End If
                End If
            Next
            If (bc_am_load_objects.obc_pub_types.pubtype(l).child_category) = 0 And bc_am_load_objects.obc_pub_types.pubtype(l).composite = False And (Me.Chkclone.Checked = False Or Me.Chkclone.Visible = False) Then
                If check_for_parameters() = False Then
                    Me.bok.Left = BtnPlace3.Left
                    Me.bok.Visible = True
                    Me.btnforwardgrey.Visible = False
                    Me.last_screen = True
                Else
                    Me.bok.Visible = False
                    Me.btnforwardgrey.Left = BtnPlace3.Left
                    Me.btnforwardgrey.Visible = True
                End If
            Else
                Me.bok.Visible = False
                Me.bokgrey.Visible = False
                Me.btnforwardgrey.Left = BtnPlace3.Left
                Me.btnforwardgrey.Visible = True
            End If
        End If
        If mode = 20 Then
            Me.btnup.Enabled = True
            Me.btnremove.Enabled = True
            Me.btndown.Enabled = True
        End If
    End Sub
    Private Sub set_image()
        Pubthumbnail.Visible = False
        Try
            MyBase.PictureBox3.Visible = False
            Pubthumbnail.Visible = True
            Dim l As New ImageList
            Pubthumbnail.Image = Drawing.Image.FromFile(bc_cs_central_settings.local_template_path + Me.ListBox1.SelectedItem + ".jpg")
            Pubthumbnail.Show()
            Pubthumbnail.Update()
        Catch ex As Exception
            MyBase.PictureBox3.Visible = True
            Pubthumbnail.Visible = False
            Dim ocommentary As New bc_cs_activity_log("bc_am_wizard_main", "set_image", bc_cs_activity_codes.COMMENTARY, "Image file: " + bc_cs_central_settings.local_template_path + Me.ListBox1.SelectedItem + ".jpg not found")
        End Try
    End Sub
    Public Sub load_start(Optional ByVal doc_open As Boolean = False)
        Dim otrace As New bc_cs_activity_log("bc_am_at_build_wizard", "load_start", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            

            'Me.ttsize = Me.Size
            Try
                If doc_open = False Then
                    Me.WindowState = FormWindowState.Normal
                End If
            Catch

            End Try
            Dim gsettings = New bc_om_create_settings
            If Me.from_workflow = False Then
                If read_user_settings(gsettings) = True Then
                    Me.Chkalert.Checked = gsettings.alerter_on
                End If
            End If

            'Me.StartPosition = FormStartPosition.CenterScreen
            'Me.Size = tsize
            REM set date from and date to

            'Me.Width = 552
            'Me.Height = 400
            Me.Size = Me.tsize


            Me.FormBorderStyle = FormBorderStyle.FixedSingle
            Me.MaximizeBox = False
            Me.SizeGripStyle = SizeGripStyle.Hide

            Dim i As Integer
            description.Left = 88
            Me.description.Text = "Please select from one of the following options:"
            For i = 0 To form_strings.form_labels.Count - 1
                If form_strings.form_labels(i).label_code = "create.main.title" Then
                    Me.description.Text = form_strings.form_labels(i).text
                    Exit For
                End If
            Next
            REM EFG June 2010
            Me.Chkclone.Visible = False
            Me.Chkclone.Checked = False
            Me.bcancelgrey.Text = "&Exit"
            Me.bskip.Visible = False
            Me.RadioButton1.Visible = True
            Me.RadioButton2.Visible = True
            Me.ListBox1.Visible = False
            Me.ListView1.Visible = False
            Me.Btntoggle.Visible = False
            Me.bcancelgrey.Left = BtnPlace2.Left
            Me.bcancelgrey.Visible = True
            Me.bleftgrey.Visible = False
            Me.RadioButton1.Checked = False
            Me.RadioButton2.Checked = False
            Me.btnforwardgrey.Left = BtnPlace3.Left
            Me.btnforwardgrey.Visible = True
            Me.bleftgrey.Visible = False
            Me.bleftgrey.Visible = False
            Me.btnup.Visible = False
            Me.btndown.Visible = False
            Me.btnremove.Visible = False
            Me.ListView1.Visible = False
            Me.RadioButton1.Checked = True
            Me.btnnext.Visible = False
            Me.Btncancel.Visible = False
            Me.Btnback.Visible = False
            Me.bleftgrey.Visible = False
            Me.Btnback.Visible = False
            Me.ListBox1.Visible = False
            Me.RadioButton1.Visible = True
            Me.RadioButton2.Visible = True
            bsummary = False
            'Me.Btncancel.Left = 420
            'Me.bcancelgrey.Left = 420
            'Me.Btnback.Left = 420
            'Me.bleftgrey.Left = 420
            Me.Btncancel.Left = BtnPlace2.Left
            Me.bcancelgrey.Left = BtnPlace2.Left
            Me.Btnback.Left = BtnPlace2.Left
            Me.bleftgrey.Left = BtnPlace2.Left

            Me.ptext1.Text = ""
            Me.ptext2.Text = ""
            Me.ptext3.Text = ""
            Me.ptext4.Text = ""
            Me.ptext5.Text = ""
            Me.ptext6.Text = ""

            unload_open_controls()
            clear_parameters()
            MyBase.PictureBox1.Visible = True
            MyBase.Pubthumbnail.Visible = True
            Me.bok.Visible = False
            Me.bokgrey.Visible = False
            Me.PictureBox3.Visible = True
            Me.LabelCon.Visible = True
            Me.LabelConStr.Visible = True
            Me.Label7.Visible = True
            Me.Label7.Visible = True

            Me.Pubthumbnail.Visible = False
            Me.proddesc.Visible = False
            Me.BlueCurve_TextSearch1.Enabled = False
            Me.BlueCurve_TextSearch1.Visible = False

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_at_build_wizard", "load_start", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally

            otrace = New bc_cs_activity_log("bc_am_at_build_wizard", "load_start", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Public Sub load_open_controls()
        REM open controls
        Try
            'tsize = Me.Size
            Me.Label1.Visible = False
            Me.FormBorderStyle = FormBorderStyle.Sizable
            Me.MaximizeBox = False
            Me.SizeGripStyle = SizeGripStyle.Auto
            Me.lblpubtypes.Visible = True
            Me.Label8.Visible = True
            Me.Label9.Visible = True
            Me.DateTimePicker1.Visible = True
            Me.DateTimePicker2.Visible = True
            Me.Lblbusarea.Visible = True
            Me.btnopen.Visible = True
            Me.btncan.Visible = True
            Me.lbusareas.Visible = True
            Me.lpubtypes.Visible = True
            Me.lstages.Visible = True
            Me.PictureBox2.Visible = False
            Me.PictureBox3.Visible = False
            Me.LabelCon.Visible = False
            Me.LabelConStr.Visible = False
            Me.Label7.Visible = False
            Me.Label7.Visible = False
            'Me.Label3.Visible = False


            Me.btnforwardgrey.Visible = False
            Me.bcancelgrey.Visible = False
            Me.bleftgrey.Visible = False
            Me.description.Visible = False
            Me.lstages.Items.Clear()
            Me.lpubtypes.Items.Clear()
            Me.lbusareas.Items.Clear()
            Me.psortbyauthor.Visible = True
            Me.psortbydate.Visible = True
            Me.psortbytitle.Visible = True
            Me.psummary.Visible = True
            Me.PictureBox4.Visible = True
            Me.PictureBox5.Visible = True
            Me.PictureBox6.Visible = True
            Me.psummary.BorderStyle = BorderStyle.None
            Me.cpublish.Visible = True
            Me.lpublish.Visible = True
            Me.chksettings.Visible = True
            Me.Chkalert.Visible = True

            REM resize window
            gsettings = New bc_om_create_settings
            If read_user_settings(gsettings) = True Then
                Me.Size = gsettings.size
            Else
                Me.Width = 552
                Me.Height = 400
                Me.ListView1.Columns(0).Width = col_1_def_width.Width
                Me.ListView1.Columns(1).Width = col_2_def_width.Width
                Me.ListView1.Columns(2).Width = col_3_def_width.Width
                Me.ListView1.Columns(3).Width = col_4_def_width.Width
                Me.ListView1.Columns(4).Width = col_5_def_width.Width
                Me.ListView1.Columns(5).Width = col_6_def_width.Width
                Me.ListView1.Columns(6).Width = col_7_def_width.Width
                Me.ListView1.Columns(7).Width = col_8_def_width.Width
            End If
            'centre_form()
            If Process.GetProcessesByName("BlueCurveService").GetLength(0) > 0 Or Me.from_alert = True Then
                If bc_cs_central_settings.service_poll_enabled = 1 Then
                    Me.Chkalert.Checked = True
                Else
                    Me.Chkalert.Checked = False
                End If
                Me.Chkalert.Enabled = False
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub
    Private Sub centre_form()
        If gsettings.size.Width = 0 Then
            Exit Sub
        End If
        Dim h As New System.Drawing.Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height)
        Dim start As New System.Drawing.Point((h.Width - gsettings.size.Width) / 2, (h.Height - gsettings.size.Height) / 2)
        Me.Location = start
    End Sub
    Private Sub unload_open_controls()
        REM open controls
        Me.Label1.Visible = True
        Me.bskip.Visible = False
        Me.lblpubtypes.Visible = False
        Me.Lblbusarea.Visible = False
        Me.Label8.Visible = False
        Me.Label9.Visible = False
        Me.DateTimePicker1.Visible = False
        Me.DateTimePicker2.Visible = False
        Me.btnopen.Visible = False
        Me.btncan.Visible = False
        Me.lbusareas.Visible = False
        Me.lpubtypes.Visible = False
        Me.lstages.Visible = False
        Me.PictureBox2.Visible = True
        'Me.Label3.Visible = True
        Me.btnforwardgrey.Left = BtnPlace3.Left
        Me.bcancelgrey.Left = BtnPlace2.Left
        Me.btnforwardgrey.Visible = True
        Me.bcancelgrey.Visible = True
        Me.Chkalert.Visible = False
        'Me.bleftgrey.Visible = True
        Me.description.Visible = True
        Me.psortbyauthor.Visible = False
        Me.psortbydate.Visible = False
        Me.psortbytitle.Visible = False
        Me.psummary.Visible = False
        Me.PictureBox4.Visible = False
        Me.PictureBox5.Visible = False
        Me.PictureBox6.Visible = False
        Me.txtsummary.Visible = False
        Me.bedit.Visible = False
        Me.cpublish.Visible = False
        Me.lpublish.Visible = False
        Me.chksettings.Visible = False
        Me.BlueCurve_TextSearch1.Enabled = False
        Me.BlueCurve_TextSearch1.Visible = False

    End Sub

    Private Sub load_build_wizard()
        Dim otrace As New bc_cs_activity_log("bc_am_at_build_wizard", "load_build_wizard", bc_cs_activity_codes.TRACE_ENTRY, "")
        Me.description.Text = ""
        Me.RadioButton1.Visible = False
        Me.RadioButton2.Visible = False
        ListBox1.Size = New System.Drawing.Size(232, 139)
        Me.ListBox1.Visible = True
        Me.ListView1.Visible = False
        Me.Btntoggle.Visible = False
        Me.bcancelgrey.Left = BtnPlace3.Left
        Me.bleftgrey.Left = BtnPlace2.Left
        Me.bleftgrey.Visible = True
        Me.bcancelgrey.Visible = True
        Me.description.Text = "Please select a Publication type:"
        Me.LabelCon.Visible = False
        Me.LabelConStr.Visible = False
        Me.Label7.Visible = False
        Me.Label1.Visible = False

        Dim i As Integer
        For i = 0 To form_strings.form_labels.Count - 1
            If form_strings.form_labels(i).label_code = "create.main.pubtype" Then
                Me.description.Text = form_strings.form_labels(i).text
                Exit For
            End If
        Next
        Me.proddesc.Visible = False

        otrace = New bc_cs_activity_log("bc_am_at_build_wizard", "load_build_wizard", bc_cs_activity_codes.TRACE_ENTRY, "")
    End Sub
    Private Sub RadioButton1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton1.CheckedChanged
        mode = 0
    End Sub
    Private Sub RadioButton2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton2.CheckedChanged
        mode = 10
    End Sub
    Private Sub btntoggle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btntoggle.Click
        If btoggle = True Then
            btoggle = False
            BlueCurve_TextSearch1.SearchUserEntitiesOnly = False
        Else
            btoggle = True
            BlueCurve_TextSearch1.SearchUserEntitiesOnly = True
        End If
        prev_toggle = btoggle
        If mode = 2 Then
            load_entities(gclass_id, btoggle)
        Else
            load_entities(Me.sub_entity_class, btoggle)
        End If
    End Sub
    Public Sub load_docs(ByVal local_docs As Boolean, ByVal all As Boolean, Optional ByVal from_server As Boolean = True, Optional ByVal show_progress As Boolean = True)
        Dim slog = New bc_cs_activity_log("bc_am_at_wizard_main", "load_docs", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim l As Integer
        Dim found As Boolean
        blocal_docs = local_docs
        ball = all
        Try
            Dim date_from As Date
            Dim date_to As Date

            date_from = Me.DateTimePicker1.Value
            date_to = Me.DateTimePicker2.Value

            Dim i, j, k As Integer

            Dim na As String
            Dim selected_stage As String
            Dim selected_pubtype As String
            Dim selected_busarea As String
            If Me.lstages.SelectedItem <> "" Then
                selected_stage = Me.lstages.SelectedItem
            Else
                selected_stage = "All Stages"
            End If
            iselected_stage = selected_stage
            If Me.lpubtypes.SelectedItem <> "" Then
                selected_pubtype = Me.lpubtypes.SelectedItem
            Else
                selected_pubtype = "All Publication Types"
            End If
            iselected_pub_type = selected_pubtype
            If Me.lbusareas.SelectedItem <> "" Then
                selected_busarea = Me.lbusareas.SelectedItem
            Else
                selected_busarea = "All Business Areas"
            End If
            iselected_bus_area = selected_busarea
            Me.lstages.Items.Clear()
            If IsNothing(all_string) Then
                all_string = ""
            End If
            'If all_string = "All Types" Then
            Me.lstages.Items.Add("All Stages")
            'Else
            '   Me.lstages.Items.Clear()
            '  Me.lstages.Items.Add(all_string)
            'End If
            Me.lpubtypes.Items.Clear()
            'If all_string = "All Types" Then
            Me.lpubtypes.Items.Add("All Publication Types")
            'Else
            '   Me.lpubtypes.Items.Clear()
            '  Me.lpubtypes.Items.Add(all_string)
            'End If
            Me.lbusareas.Items.Clear()
            'If all_string = "All Types" Then
            Me.lbusareas.Items.Add("All Business Areas")
            'Else
            '   Me.lbusareas.Items.Clear()
            '  Me.lbusareas.Items.Add(all_string)


            date_from = Me.DateTimePicker1.Value
            date_to = Me.DateTimePicker2.Value

            If from_server = True Then
                oopen_doc = New bc_am_at_open_doc(local_docs, all, date_from, date_to, Me.cpublish.Checked, show_progress)
            End If
            Me.ListBox1.Items.Clear()
            ListView1.Items.Clear()
            ListView1.SmallImageList = ImageList1
            Me.ListView1.Visible = True
            If Not IsNothing(oopen_doc.onetwork_docs) Then
                For i = 0 To oopen_doc.onetwork_docs.document.Count - 1
                    REM apply data filter
                    REM if originating author flag is not set show all else filter on
                    REM logged on user
                    found = False
                    If (all = False And oopen_doc.onetwork_docs.document(i).originating_author = bc_cs_central_settings.logged_on_user_id) Or all = True Or local_docs = False Then
                        Dim item As New ListViewItem(CStr(oopen_doc.onetwork_docs.document(i).doc_date.tolocaltime))

                        For j = 0 To bc_am_load_objects.obc_entities.entity.Count - 1
                            If oopen_doc.onetwork_docs.document(i).entity_id = bc_am_load_objects.obc_entities.entity(j).id Then
                                item.SubItems.Add(CStr(bc_am_load_objects.obc_entities.entity(j).name))
                                found = True
                                Exit For
                            End If
                        Next
                        If found = False Then
                            item.SubItems.Add("")
                        End If

                        REM list view
                        item.SubItems.Add(CStr(oopen_doc.onetwork_docs.document(i).stage_name))
                        item.SubItems.Add(CStr(oopen_doc.onetwork_docs.document(i).title))
                        item.SubItems.Add(CStr(oopen_doc.onetwork_docs.document(i).pub_type_name))
                        add_pubtype_to_box(oopen_doc.onetwork_docs.document(i).pub_type_name)
                        found = False
                        REM add stage to stage box
                        add_stage_to_box(CStr(oopen_doc.onetwork_docs.document(i).stage_name))
                        REM author
                        found = False
                        For k = 0 To bc_am_load_objects.obc_users.user.Count - 1
                            If CStr(bc_am_load_objects.obc_users.user(k).id) = oopen_doc.onetwork_docs.document(i).originating_author Then
                                item.SubItems.Add(bc_am_load_objects.obc_users.user(k).first_name + " " + bc_am_load_objects.obc_users.user(k).surname)
                                found = True
                                Exit For
                            End If
                        Next
                        If found = False Then
                            item.SubItems.Add("")
                        End If
                        If oopen_doc.onetwork_docs.document(i).checked_out_user <> 0 Then
                            For j = 0 To bc_am_load_objects.obc_users.user.Count - 1
                                If bc_am_load_objects.obc_users.user(j).id = oopen_doc.onetwork_docs.document(i).checked_out_user Then
                                    na = bc_am_load_objects.obc_users.user(j).first_name + " " + bc_am_load_objects.obc_users.user(j).surname
                                    'found = True
                                    item.ImageIndex = 0
                                End If
                            Next
                        End If

                        If oopen_doc.onetwork_docs.document(i).id > 0 Then
                            item.SubItems.Add(CStr(oopen_doc.onetwork_docs.document(i).id))
                        Else
                            item.SubItems.Add(CStr(oopen_doc.onetwork_docs.document(i).filename))
                        End If
                        item.SubItems.Add(CStr(oopen_doc.onetwork_docs.document(i).bus_area))
                        add_bus_area_to_box(CStr(oopen_doc.onetwork_docs.document(i).bus_area))
                        REM check sort criteria to see if can add
                        If (iselected_stage = oopen_doc.onetwork_docs.document(i).stage_name Or iselected_stage = "All Stages") And (iselected_pub_type = CStr(oopen_doc.onetwork_docs.document(i).pub_type_name) Or iselected_pub_type = "All Publication Types") And (iselected_bus_area = CStr(oopen_doc.onetwork_docs.document(i).bus_area) Or iselected_bus_area = "All Business Areas") Then
                            ListView1.Items.Add(item)
                        End If
                    End If
                Next
            End If
            REM reset list boxes
            resetting = True
            For l = 0 To lstages.Items.Count - 1
                If lstages.Items(l) = iselected_stage Then
                    lstages.SelectedIndex = l
                    Exit For
                End If
            Next
            For l = 0 To lpubtypes.Items.Count - 1
                If lpubtypes.Items(l) = iselected_pub_type Then
                    lpubtypes.SelectedIndex = l
                    Exit For
                End If
            Next
            For l = 0 To lbusareas.Items.Count - 1
                If lbusareas.Items(l) = iselected_bus_area Then
                    lbusareas.SelectedIndex = l
                    Exit For
                End If
            Next
            resetting = False

            Me.RadioButton1.Visible = False
            Me.RadioButton2.Visible = False
            Me.btnforwardgrey.Text = "Open"
            Me.bcancelgrey.Text = "Cancel"
            Me.Btntoggle.Visible = False
            Me.bleftgrey.Visible = False
            Me.ListBox1.Visible = True
            Me.btnopen.Enabled = False

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_at_wizard_main", "load_docs", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            slog = New bc_cs_activity_log("bc_am_at_wizard_main", "load_docs", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Private Sub add_stage_to_box(ByVal stage_name As String)
        Dim i As Integer
        For i = 0 To Me.lstages.Items.Count - 1
            If Me.lstages.Items(i) = stage_name Then
                Exit Sub
            End If
        Next
        Me.lstages.Items.Add(stage_name)
    End Sub
    Private Sub add_pubtype_to_box(ByVal pubtype As String)

        Dim i As Integer
        For i = 0 To Me.lpubtypes.Items.Count - 1
            If Me.lpubtypes.Items(i) = pubtype Then
                Exit Sub
            End If
        Next
        Me.lpubtypes.Items.Add(pubtype)

    End Sub
    Private Sub add_bus_area_to_box(ByVal busarea As String)
        Dim i As Integer
        For i = 0 To Me.lbusareas.Items.Count - 1
            If Me.lbusareas.Items(i) = busarea Then
                Exit Sub
            End If
        Next
        Me.lbusareas.Items.Add(busarea)
    End Sub
    Private Sub load_pub_type()
        Me.Chkclone.Visible = False
        Me.Chkclone.Checked = False
        Dim i, j As Integer
        gchild_category = 0
        Me.ListBox1.Items.Clear()
        Me.description.Text = "Please select a Publication type:"
        For i = 0 To form_strings.form_labels.Count - 1
            If form_strings.form_labels(i).label_code = "create.main.pubtype" Then
                Me.description.Text = form_strings.form_labels(i).text
                Exit For
            End If
        Next
        For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
            REM filter pub types for users bus area
            For j = 0 To bc_am_load_objects.obc_prefs.bus_areas.Count - 1
                If bc_am_load_objects.obc_prefs.bus_areas(j) = bc_am_load_objects.obc_pub_types.pubtype(i).bus_area_id And bc_am_load_objects.obc_pub_types.pubtype(i).support_doc_only = False Then
                    If Me.from_workflow = False Or Me.from_create = True Then
                        If bc_am_load_objects.obc_pub_types.pubtype(i).show_in_wizard = True Then
                            Me.ListBox1.Items.Add(bc_am_load_objects.obc_pub_types.pubtype(i).name)
                            ListBox1.SelectedIndex = 0
                        End If
                    Else
                        If bc_am_load_objects.obc_pub_types.pubtype(i).create_only = False And bc_am_load_objects.obc_pub_types.pubtype(i).support_doc_only = False Then
                            Me.ListBox1.Items.Add(bc_am_load_objects.obc_pub_types.pubtype(i).name)
                            ListBox1.SelectedIndex = 0
                        End If
                    End If
                    Exit For
                End If
            Next
        Next
        If Me.ListBox1.Items.Count = 0 Then
            Dim omessage As New bc_cs_message("Blue Curve - create", "User has no access to pub types check business area", bc_cs_message.MESSAGE)
            Exit Sub
        End If
        mode = 1
        index_changed()
    End Sub
    Private Sub load_regular_reports()
        Dim i As Integer
        Me.ListBox1.Items.Clear()
        Me.description.Text = "Please select a Regular Report:"

        For i = 0 To bc_am_load_objects.obc_pub_types.regular_reports.regular_reports.Count - 1
            REM filter pub types for users bus area
            Me.ListBox1.Items.Add(bc_am_load_objects.obc_pub_types.regular_reports.regular_reports(i).name)
            ListBox1.SelectedIndex = 0
        Next
        mode = 200
    End Sub
    Private Sub load_entities(ByVal class_id As Long, ByVal for_user As Boolean)
        Dim slog = New bc_cs_activity_log("bc_am_at_wizard_main", "load_entities", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Select Case True
                Case bc_cs_central_settings.show_all_entities
                    Me.Btntoggle.Visible = True
                Case Not bc_cs_central_settings.show_all_entities
                    Me.Btntoggle.Visible = False
            End Select

            If BlueCurve_TextSearch1.SearchUserEntitiesOnly = False Then
                Me.Btntoggle.Text = "My Subjects"
                For k = 0 To form_strings.form_labels.Count - 1
                    If form_strings.form_labels(k).label_code = "create.main.mine" Then
                        Me.Btntoggle.Text = form_strings.form_labels(k).text
                        Exit For
                    End If
                Next
            End If
            If BlueCurve_TextSearch1.SearchUserEntitiesOnly = True Then
                Me.Btntoggle.Text = "All Subjects"
                For k = 0 To form_strings.form_labels.Count - 1
                    If form_strings.form_labels(k).label_code = "create.main.all" Then
                        Me.Btntoggle.Text = form_strings.form_labels(k).text
                        Exit For
                    End If
                Next
            End If

            Dim i As Integer
            Me.ListBox1.Items.Clear()
            shown_entity_list.Clear()
            Me.description.Text = "Please select an entity:"
            For i = 0 To bc_am_load_objects.obc_entities.entity.Count - 1
                If bc_am_load_objects.obc_entities.entity(i).class_id = class_id Then
                    If LCase(bc_am_load_objects.obc_entities.entity(i).class_name.ToString.Substring(0, 1)) = "a" Or _
                        LCase(bc_am_load_objects.obc_entities.entity(i).class_name.ToString.Substring(0, 1)) = "e" Or _
                        LCase(bc_am_load_objects.obc_entities.entity(i).class_name.ToString.Substring(0, 1)) = "i" Or _
                        LCase(bc_am_load_objects.obc_entities.entity(i).class_name.ToString.Substring(0, 1)) = "o" Or _
                        LCase(bc_am_load_objects.obc_entities.entity(i).class_name.ToString.Substring(0, 1)) = "u" Then
                        Me.description.Text = "Please select an " + bc_am_load_objects.obc_entities.entity(i).class_name
                        Exit For
                    Else
                        Me.description.Text = "Please select a " + bc_am_load_objects.obc_entities.entity(i).class_name
                        Exit For
                    End If
                End If
            Next
            'For i = 0 To form_strings.form_labels.Count - 1
            'If form_strings.form_labels(i).label_code = "create.main.entity" Then
            'Me.description.Text = form_strings.form_labels(i).text
            'Exit For
            'End If
            'Next

            entityBasedParams = False

            Dim entity_list As New bc_om_entities
            entity_list = bc_am_load_objects.obc_entities
            If for_user = False Then
                If bc_cs_central_settings.alt_entity_for_build = True Then
                    entity_list = bc_am_load_objects.obc_entities.alternate_entity_list
                End If
            Else
                If bc_cs_central_settings.alt_entity_for_apref = True Then
                    entity_list = bc_am_load_objects.obc_entities.alternate_entity_list
                End If
            End If

            BlueCurve_TextSearch1.SearchClass = class_id
            BlueCurve_TextSearch1.SearchBuildEntitiesOnly = False
            BlueCurve_TextSearch1.SearchRefresh()

            For i = 0 To entity_list.entity.Count - 1
                If entity_list.entity(i).class_id = class_id Then

                    entityBasedParams = True
                    Exit For
                End If
            Next


            '        If for_user = False Then
            '            Me.Btntoggle.Text = "My Subjects"

            '            For k = 0 To form_strings.form_labels.Count - 1
            '                If form_strings.form_labels(k).label_code = "create.main.mine" Then
            '                    Me.Btntoggle.Text = form_strings.form_labels(k).text
            '                    Exit For
            '                End If
            '            Next
            '            If bc_am_load_objects.obc_entities.entity(i).show_mode = 0 Then
            '                Me.ListBox1.Items.Add(entity_list.entity(i).name + " " + bc_am_load_objects.obc_entities.entity(i).show_text)
            '                Me.ListBox1.SelectedIndex = 0
            '                Me.shown_entity_list.Add(entity_list.entity(i).id)
            '            End If
            '        Else
            '            Me.Btntoggle.Text = "All Subjects"
            '            For k = 0 To form_strings.form_labels.Count - 1
            '                If form_strings.form_labels(k).label_code = "create.main.all" Then
            '                    Me.Btntoggle.Text = form_strings.form_labels(k).text
            '                    Exit For
            '                End If
            '            Next

            '            For j = 0 To bc_am_load_objects.obc_prefs.pref.Count - 1
            '                If bc_am_load_objects.obc_prefs.pref(j).entity_id = entity_list.entity(i).id Then
            '                    If bc_am_load_objects.obc_entities.entity(i).show_mode = 0 Then
            '                        Me.ListBox1.Items.Add(entity_list.entity(i).name)
            '                        Me.ListBox1.SelectedIndex = 0
            '                        Me.shown_entity_list.Add(entity_list.entity(i).id)
            '                    ElseIf bc_am_load_objects.obc_entities.entity(i).show_mode = 1 Then
            '                        Me.ListBox1.Items.Add(entity_list.entity(i).name + " " + entity_list.entity(i).show_text)
            '                        Me.ListBox1.SelectedIndex = 0
            '                        Me.shown_entity_list.Add(entity_list.entity(i).id)
            '                    End If
            '                End If
            '            Next
            '        End If
            '    End If
            'Next

            ' if no entities in the list then check for non entity based parameters
            If Not entityBasedParams Then
                Dim osql As New bc_om_sql(String.Concat("exec dbo.bcc_core_list_nonentities_for_class ", class_id))

                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                    osql.tmode = bc_cs_soap_base_class.tREAD
                    osql.transmit_to_server_and_receive(osql, True)
                ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    osql.db_read()
                End If
                If osql.success = True Then
                    Me.Btntoggle.Visible = False
                    For i = 0 To UBound(osql.results, 2)
                        Me.ListBox1.Items.Add(osql.results(1, i))
                        Me.ListBox1.SelectedIndex = 0
                        Me.shown_entity_list.Add(osql.results(0, i))
                    Next
                Else
                    Dim ocommentary As New bc_cs_activity_log("bc_am_at_wizard_main", "non entity based parameters", bc_cs_activity_codes.COMMENTARY, "Non entity based parameters not configured")
                End If
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_at_wizard_main", "load_entities", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

            slog = New bc_cs_activity_log("bc_am_at_wizard_main", "load_entities", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    Private Function get_entity_id_selected(ByVal index As Integer) As Long
        Dim slog = New bc_cs_activity_log("bc_am_at_wizard_main", "get_entity_id_selected", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            REM get filter id
            REM check of filter used
            get_entity_id_selected = 0
            For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count
                If bc_am_load_objects.obc_pub_types.pubtype(i).id = gpub_type_id Then
                    If bc_am_load_objects.obc_pub_types.pubtype(i).child_category = 0 Then
                        Exit Function
                    End If
                    Exit For
                End If
            Next

            Try
                get_entity_id_selected = Me.shown_entity_list(Me.ListBox1.SelectedIndex)
            Catch

            End Try

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_at_wizard_main", "get_entity_id_selected", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_at_wizard_main", "get_entity_id_selected", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function

    Private Sub ListView1_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles ListView1.ColumnClick
        Dim slog = New bc_cs_activity_log("bc_am_at_wizard_main", "get_entity_id_selected", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Select Case CInt(e.Column.ToString)
                Case 0
                    REM date
                    ListView1.ListViewItemSorter() = New CompareBydate
                    If CompareBydate.toggle = False Then
                        CompareBydate.toggle = True
                    Else
                        CompareBydate.toggle = False
                    End If
                Case 1
                    REM item
                    ListView1.ListViewItemSorter() = New CompareByitem
                    If CompareByitem.toggle = False Then
                        CompareByitem.toggle = True
                    Else
                        CompareByitem.toggle = False
                    End If
                Case 2
                    REM stage
                    ListView1.ListViewItemSorter() = New CompareBystage
                    If CompareBystage.toggle = False Then
                        CompareBystage.toggle = True
                    Else
                        CompareBystage.toggle = False
                    End If

                Case 3
                    ListView1.ListViewItemSorter() = New CompareByname
                    If CompareByname.toggle = False Then
                        CompareByname.toggle = True
                    Else
                        CompareByname.toggle = False
                    End If


                Case 4
                    ListView1.ListViewItemSorter() = New CompareBytype
                    If CompareBytype.toggle = False Then
                        CompareBytype.toggle = True
                    Else
                        CompareBytype.toggle = False
                    End If
                Case 5
                    ListView1.ListViewItemSorter() = New CompareByco
                    If CompareByco.toggle = False Then
                        CompareByco.toggle = True
                    Else
                        CompareByco.toggle = False
                    End If


            End Select
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_at_wizard_main", "columnclick", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_at_wizard_main", "columnclick", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    Class CompareByitem
        Implements System.Collections.IComparer
        Public Shared toggle As Boolean = False

        Function compare(ByVal x As Object, ByVal y As Object) As Integer _
        Implements System.Collections.IComparer.Compare

            Dim item1 As ListViewItem = CType(x, ListViewItem)
            Dim item2 As ListViewItem = CType(y, ListViewItem)
            If toggle = True Then
                Return String.Compare(item1.SubItems(1).Text, item2.SubItems(1).Text)
            Else
                Return String.Compare(item2.SubItems(1).Text, item1.SubItems(1).Text)
            End If
        End Function
    End Class
    Class CompareByname
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
    Class CompareBydate
        Implements System.Collections.IComparer
        Public Shared toggle As Boolean = False

        Function compare(ByVal x As Object, ByVal y As Object) As Integer _
        Implements System.Collections.IComparer.Compare
            Try
                Dim item1 As ListViewItem = CType(x, ListViewItem)
                Dim item2 As ListViewItem = CType(y, ListViewItem)
                If toggle = True Then
                    Return Date.Compare(Date.Parse(item1.Text), Date.Parse(item2.Text))
                Else
                    Return Date.Compare(Date.Parse(item2.Text), Date.Parse(item1.Text))
                End If
            Catch

            End Try
        End Function

        Public Sub New()

        End Sub
    End Class

    Class CompareBytype
        Implements System.Collections.IComparer
        Public Shared toggle As Boolean = False

        Function compare(ByVal x As Object, ByVal y As Object) As Integer _
        Implements System.Collections.IComparer.Compare
            Dim item1 As ListViewItem = CType(x, ListViewItem)
            Dim item2 As ListViewItem = CType(y, ListViewItem)
            If toggle = True Then
                Return String.Compare(item1.SubItems(4).Text, item2.SubItems(3).Text)
            Else
                Return String.Compare(item2.SubItems(4).Text, item1.SubItems(3).Text)
            End If
        End Function
    End Class
    Class CompareBystage
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

        Public Sub New()

        End Sub
    End Class
    Class CompareByco
        Implements System.Collections.IComparer
        Public Shared toggle As Boolean = False

        Function compare(ByVal x As Object, ByVal y As Object) As Integer _
        Implements System.Collections.IComparer.Compare
            Dim item1 As ListViewItem = CType(x, ListViewItem)
            Dim item2 As ListViewItem = CType(y, ListViewItem)
            If toggle = True Then
                Return String.Compare(item1.SubItems(5).Text, item2.SubItems(5).Text)
            Else
                Return String.Compare(item2.SubItems(5).Text, item1.SubItems(5).Text)
            End If
        End Function
    End Class

    Private Sub ListView1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView1.SelectedIndexChanged
        Try
            Me.btnforwardgrey.Visible = True
            Me.btnforwardgrey.Visible = False
            Me.btnopen.Enabled = True
            If Me.ListView1.SelectedItems(0).ImageIndex < 0 And Me.txtsummary.Visible = True Then
                Me.bedit.Visible = True
            Else
                Me.bedit.Visible = False
            End If
            set_summary_for_document()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnforwardgrey_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btnforwardgrey.MouseDown
        Me.btnforwardgrey.Visible = False
        Me.btnnext.Left = BtnPlace3.Left
        Me.btnnext.Visible = True
        'Me.btnnext_Click(sender, e)

    End Sub
    REM EFG June 2012
    Private Sub btnforwardgrey_Mouseup(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btnforwardgrey.MouseUp
        Dim otrace As New bc_cs_activity_log("bc_am_at_build_wizard", "btnforwardgrey_Mouseup", bc_cs_activity_codes.TRACE_ENTRY, "")
        Me.btnforwardgrey.Left = BtnPlace3.Left
        Me.btnforwardgrey.Visible = True
        Me.btnnext.Visible = False

        Try

            Me.last_screen = False

            Me.Btntoggle.Visible = False
            btoggle = bc_cs_central_settings.my_entities_default
            Dim oproductselection As New bc_am_product_selection
            Me.btnforwardgrey.Visible = False
            Me.btnforwardgrey.Left = BtnPlace3.Left
            Me.btnforwardgrey.Visible = True

            Dim i As Integer
            Dim intPubtypeId As Integer
            Dim intSelectedIndex As Integer
            is_composite = False

            Select Case mode
                REM pub types
                Case 0
                    load_build_wizard()
                    Me.proddesc.Visible = True
                    load_pub_type()
                    mode = 1
                    REM entity
                    'Me.Btncancel.Left = 356
                    'Me.bcancelgrey.Left = 356
                    'Me.Btnback.Left = 416
                    'Me.bleftgrey.Left = 416
                    Me.Btncancel.Left = BtnPlace1.Left
                    Me.bcancelgrey.Left = BtnPlace1.Left
                    Me.Btnback.Left = BtnPlace2.Left
                    Me.bleftgrey.Left = BtnPlace2.Left
                    Me.Btncancel.Visible = False
                    Me.bcancelgrey.Visible = False

                    BlueCurve_TextSearch1.Enabled = False
                    BlueCurve_TextSearch1.Visible = False
                    'ListBox1.Location = New System.Drawing.Point(88, 112)
                    ListBox1.Left = 88
                    description.Left = 88
                    ListBox1.Size = New System.Drawing.Size(232, 139)

                Case 1
                    'If obc_objects.obc_pub_types.pubtype(Me.ListBox1.SelectedIndex).composite = True Then
                    'Me.description.Text = "Default Composite Build List"
                    'bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve create", "Retrieving Composite Document List...", 1, False, True)
                    'bc_cs_central_settings.progress_bar.show()
                    'bc_am_load_objects.obc_om_composite_pub_type = obc_objects.obc_pub_types.pubtype(Me.ListBox1.SelectedIndex)
                    'bc_am_composite_doc = New bc_am_composite_docs(0)
                    'If bc_am_composite_doc.get_component_documents = True Then
                    'load_composite_list(-1)
                    'bc_cs_central_settings.progress_bar.unload()
                    'mode = 20
                    'Me.btnforwardgrey.Visible = False
                    'Me.bok.Visible = True
                    'Me.btnup.Visible = True
                    'Me.btnremove.Visible = True
                    'Me.btndown.Visible = True
                    'Me.btnup.Enabled = False
                    'Me.btnremove.Enabled = False
                    'Me.btndown.Enabled = False
                    'End If
                    'Exit Sub
                    'End If

                    If ListBox1.SelectedIndex = -1 Then
                        Exit Sub
                    End If

                    has_params = check_for_parameters()
                    prev_list_index = Me.ListBox1.SelectedIndex
                    'Me.Btncancel.Left = 356
                    'Me.bcancelgrey.Left = 356
                    Me.Btncancel.Left = BtnPlace1.Left
                    Me.bcancelgrey.Left = BtnPlace1.Left
                    Me.Btncancel.Visible = False
                    Me.bcancelgrey.Visible = False
                    'Me.Btnback.Left = 416
                    'Me.bleftgrey.Left = 416
                    Me.Btnback.Left = BtnPlace2.Left
                    Me.bleftgrey.Left = BtnPlace2.Left
                    intSelectedIndex = Me.ListBox1.SelectedIndex
                    Dim l, m, n As Integer
                    n = 0
                    Dim found As Boolean = False
                    For l = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                        REM filter pub types for users bus area
                        If from_workflow = False Or Me.from_create = True Then
                            If bc_am_load_objects.obc_pub_types.pubtype(l).show_in_wizard = True Then
                                For m = 0 To bc_am_load_objects.obc_prefs.bus_areas.Count - 1
                                    If bc_am_load_objects.obc_prefs.bus_areas(m) = bc_am_load_objects.obc_pub_types.pubtype(l).bus_area_id Then
                                        If n = intSelectedIndex Then
                                            intPubtypeId = bc_am_load_objects.obc_pub_types.pubtype(l).id
                                            found = True
                                            Exit For
                                        End If
                                        n = n + 1
                                    End If
                                Next
                            End If
                        Else
                            If bc_am_load_objects.obc_pub_types.pubtype(l).create_only = False Then
                                For m = 0 To bc_am_load_objects.obc_prefs.bus_areas.Count - 1
                                    If bc_am_load_objects.obc_prefs.bus_areas(m) = bc_am_load_objects.obc_pub_types.pubtype(l).bus_area_id Then
                                        If n = intSelectedIndex Then
                                            intPubtypeId = bc_am_load_objects.obc_pub_types.pubtype(l).id
                                            found = True
                                            Exit For
                                        End If
                                        n = n + 1
                                    End If
                                Next
                            End If
                        End If
                        If found = True Then
                            Exit For
                        End If
                    Next
                    gpub_type_id = intPubtypeId
                    gpub_type = bc_am_load_objects.obc_pub_types.pubtype(l)
                    gclass_id = bc_am_load_objects.obc_pub_types.pubtype(l).child_category

                    entity_id = get_entity_id_selected(Me.ListBox1.SelectedIndex)

                    gchild_category = bc_am_load_objects.obc_pub_types.pubtype(l).child_category

                    If (bc_am_load_objects.obc_pub_types.pubtype(l).child_category) = 0 Then
                        If bc_am_load_objects.obc_pub_types.pubtype(l).is_clonable = True And Me.Chkclone.Checked = True Then

                            REM Steve Wooderson 26/06/2013
                            REM If creating a new document from word do not invoke the clone here.
                            REM We will do it from AX
                            clonePubType = Me.Chkclone.Checked
                            If (from_create = True Or (from_create = False And from_workflow = False)) Then
                                clonePubType = Me.Chkclone.Checked
                                invoke_clone(bc_am_load_objects.obc_pub_types.pubtype(l).id, 0, 0)
                            Else
                                Me.Hide()
                            End If
                            Exit Sub
                        End If
                        REM check if composite PR fix 8-3-2007
                        If bc_am_load_objects.obc_pub_types.pubtype(l).composite = True Then
                            REM TBD

                            REM Steve Wooderson 26/06/2013
                            REM If creating a new document from word do not invoke the composite here.
                            REM We will do it from AX
                            compositePubType = l
                            If (from_create = True Or (from_create = False And from_workflow = False)) Or bc_cs_central_settings.use_advanced_composite_build = False Then
                                invoke_composite(0, bc_am_load_objects.obc_pub_types.pubtype(l))
                            Else
                                buildMode = mode
                                Me.Hide()
                            End If
                            Exit Sub
                        End If


                        REM none entity based products  
                        If check_for_parameters() = True Then
                            Me.bok.Left = BtnPlace3.Left
                            Me.bok.Visible = True
                            Me.last_screen = True
                            Me.btnforwardgrey.Left = BtnPlace3.Left
                            Me.btnforwardgrey.Visible = True
                            load_parameters()
                            MyBase.PictureBox3.Visible = False
                            MyBase.Pubthumbnail.Visible = False
                            mode = 100
                            prev_mode = 1
                        Else
                            Me.WindowState = System.Windows.Forms.FormWindowState.Minimized
                            bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve create", "Building Document...", 20, True, True)

                            If oproductselection.launch_product(gpub_type_id, 0, Nothing, 0) = bc_cs_error_codes.RETURN_ERROR Then
                                Me.WindowState = System.Windows.Forms.FormWindowState.Normal
                            End If
                            bc_cs_central_settings.progress_bar.unload()
                            load_pub_type()
                            mode = 1
                        End If

                        Me.proddesc.Visible = False
                        BlueCurve_TextSearch1.Enabled = False
                        BlueCurve_TextSearch1.Visible = False
                        'ListBox1.Location = New System.Drawing.Point(88, 112)
                        ListBox1.Left = 88
                        description.Left = 88
                        ListBox1.Size = New System.Drawing.Size(232, 139)

                    Else
                        If Me.bfrom_import = True Then
                            'Me.bskip.Visible = True
                        End If

                        is_composite = bc_am_load_objects.obc_pub_types.pubtype(l).composite
                        prev_child_category = bc_am_load_objects.obc_pub_types.pubtype(l).child_category
                        sub_entity_class = bc_am_load_objects.obc_pub_types.pubtype(l).sub_entity_class

                        BlueCurve_TextSearch1.SearchClass = bc_am_load_objects.obc_pub_types.pubtype(l).child_category
                        BlueCurve_TextSearch1.Enabled = True
                        BlueCurve_TextSearch1.AttributeControlBuild(False)
                        BlueCurve_TextSearch1.SearchUserEntitiesOnly = bc_cs_central_settings.my_entities_default
                        BlueCurve_TextSearch1.SearchBuildEntitiesOnly = bc_cs_central_settings.alt_entity_for_build
                        BlueCurve_TextSearch1.Enabled = True

                        Select Case True
                            Case bc_cs_central_settings.my_entities_default
                                load_entities(bc_am_load_objects.obc_pub_types.pubtype(l).child_category, True)
                            Case Not bc_cs_central_settings.show_all_entities
                                load_entities(bc_am_load_objects.obc_pub_types.pubtype(l).child_category, True)
                            Case Else
                                load_entities(bc_am_load_objects.obc_pub_types.pubtype(l).child_category, False)
                        End Select

                        'ListBox1.Location = New System.Drawing.Point(16, 112)
                        ListBox1.Left = 16
                        description.Left = 16

                        ListBox1.Size = New System.Drawing.Size(310, 139)
                        Me.proddesc.Visible = False
                        BlueCurve_TextSearch1.Visible = True
                        Application.DoEvents()

                        'Rama code changes : Set back the original value.
                        gpub_type_id = intPubtypeId
                        mode = 2
                        If has_params = False And is_composite = False And bc_am_load_objects.obc_pub_types.pubtype(l).sub_entity_class = 0 And (Me.Chkclone.Visible = False Or Me.Chkclone.Checked = False) Then
                            Me.btnforwardgrey.Visible = False
                            Me.bok.Left = BtnPlace3.Left
                            Me.bok.Visible = True
                        End If
                        If has_params = False And is_composite = False And bc_am_load_objects.obc_pub_types.pubtype(l).sub_entity_class = 0 Then
                            last_screen = True

                        End If
                    End If

                Case 2
                        REM entity based products
                        Me.proddesc.Visible = False

                        If Me.ListBox1.SelectedIndex = -1 Then
                            Dim bc_cs_message As New bc_cs_message("Blue Curve create", "Item must be selected!", bc_cs_message.MESSAGE)
                            Select Case True
                                Case bc_cs_central_settings.show_all_entities
                                Me.Btntoggle.Visible = True
                                Me.Invalidate()

                                Case Not bc_cs_central_settings.show_all_entities
                                    Me.Btntoggle.Visible = False
                            End Select
                            Exit Sub
                        End If
                        BlueCurve_TextSearch1.Enabled = False
                        BlueCurve_TextSearch1.Visible = False
                    'ListBox1.Location = New System.Drawing.Point(88, 112)
                    ListBox1.Left = 88
                    description.Left = 88
                    ListBox1.Size = New System.Drawing.Size(232, 139)

                        prev_list_index2 = Me.ListBox1.SelectedIndex
                        'entity_id = get_entity_id_selected(Me.ListBox1.SelectedIndex)
                        'PR fix July 2011
                        If BlueCurve_TextSearch1.ShownEntityList.Count > 0 Then
                            entity_id = BlueCurve_TextSearch1.ShownEntityList(Me.ListBox1.SelectedIndex)
                        Else
                            entity_id = 0
                        End If

                        gentity_id = entity_id

                        REM check if composite PR fix 8-3-2007
                        For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count
                            If bc_am_load_objects.obc_pub_types.pubtype(i).id = gpub_type_id Then
                                If bc_am_load_objects.obc_pub_types.pubtype(i).is_clonable = True And Me.Chkclone.Checked = True And sub_entity_class = 0 Then
                                    invoke_clone(gpub_type_id, entity_id, 0)
                                    BlueCurve_TextSearch1.Enabled = True
                                    BlueCurve_TextSearch1.Visible = True
                                'ListBox1.Location = New System.Drawing.Point(16, 112)
                                ListBox1.Left = 16
                                description.Left = 16
                                    ListBox1.Size = New System.Drawing.Size(310, 139)

                                    Exit Sub
                                End If
                                If bc_am_load_objects.obc_pub_types.pubtype(i).composite = True Then
                                    REM rem need to pass entity_id in 
                                    ' if not entity based params then this means there is a custom parameter
                                    ' and so needs retrieving. Entity_id needs to be -1 in this instance.
                                    Dim customParam As String = ""

                                    If Not entityBasedParams Then
                                        If entity_id = -1 Then
                                            customParam = ListBox1.SelectedItem.ToString
                                        Else
                                            customParam = entity_id
                                        End If
                                        entity_id = -1
                                    End If
                                    invoke_composite(entity_id, gpub_type, customParam)
                                    Exit Sub
                                End If

                                Exit For
                            End If
                        Next
                        REM see if there is a sub class selection
                        If sub_entity_class > 0 Then
                            prev_mode = 2
                            mode = 4

                            REM ==================================
                            BlueCurve_TextSearch1.SearchClass = sub_entity_class
                            BlueCurve_TextSearch1.AttributeControlBuild(False)
                            BlueCurve_TextSearch1.SearchUserEntitiesOnly = bc_cs_central_settings.my_entities_default
                            BlueCurve_TextSearch1.SearchBuildEntitiesOnly = bc_cs_central_settings.alt_entity_for_build
                            BlueCurve_TextSearch1.Enabled = True

                            Select Case True
                                Case bc_cs_central_settings.my_entities_default
                                    load_entities(sub_entity_class, True)
                                Case Not bc_cs_central_settings.show_all_entities
                                    load_entities(sub_entity_class, True)
                                Case Else
                                    load_entities(sub_entity_class, False)
                            End Select
                        'ListBox1.Location = New System.Drawing.Point(16, 112)
                        ListBox1.Left = 16
                        description.Left = 16

                            ListBox1.Size = New System.Drawing.Size(310, 139)
                            Me.proddesc.Visible = False
                            BlueCurve_TextSearch1.Visible = True
                            Application.DoEvents()
                            REM ==================================================
                            'Select Case True
                            '    Case bc_cs_central_settings.my_entities_default
                            '        load_entities(sub_entity_class, True)
                            '    Case Not bc_cs_central_settings.show_all_entities
                            '        load_entities(sub_entity_class, True)
                            '    Case Else
                            '        load_entities(sub_entity_class, False)
                            'End Select
                            If has_params = False And is_composite = False And (Me.Chkclone.Visible = False Or Me.Chkclone.Checked = False) Then
                            Me.btnforwardgrey.Visible = False
                            Me.bok.Left = BtnPlace3.Left
                                Me.bok.Visible = True
                            End If
                            If has_params = False And is_composite = False Then
                                Me.last_screen = True
                            End If
                            Exit Sub
                        End If
                        If check_for_parameters() = True Then
                            MyBase.PictureBox3.Visible = False
                            MyBase.Pubthumbnail.Visible = False
                        Me.bok.Left = BtnPlace3.Left
                        Me.bok.Visible = True
                            Me.last_screen = True
                            Me.btnforwardgrey.Visible = False
                            load_parameters()
                            prev_mode = 2
                            mode = 100
                        Else
                            Me.WindowState = System.Windows.Forms.FormWindowState.Minimized
                            bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve create", "Selecting Product...", 20, True, True)

                            If oproductselection.launch_product(gpub_type_id, entity_id, Nothing, Me.sub_entity_id) = bc_cs_error_codes.RETURN_ERROR Then
                                Me.WindowState = System.Windows.Forms.FormWindowState.Normal
                            End If
                            bc_cs_central_settings.progress_bar.unload()
                            load_pub_type()
                            mode = 1
                        End If
                Case 4

                        If Me.Chkclone.Visible = True And Me.Chkclone.Checked = True Then
                            sub_entity_id = BlueCurve_TextSearch1.ShownEntityList(Me.ListBox1.SelectedIndex)
                            invoke_clone(gpub_type_id, entity_id, sub_entity_id)
                            BlueCurve_TextSearch1.Enabled = True
                            BlueCurve_TextSearch1.Visible = True
                        'ListBox1.Location = New System.Drawing.Point(16, 112)
                        ListBox1.Left = 16
                        description.Left = 16
                            ListBox1.Size = New System.Drawing.Size(310, 139)

                            Exit Sub
                        End If

                        Me.proddesc.Visible = False
                        If check_for_parameters() = True Then
                            MyBase.PictureBox3.Visible = False
                        MyBase.Pubthumbnail.Visible = False
                        Me.bok.Left = BtnPlace3.Left
                            Me.bok.Visible = True
                            Me.last_screen = True
                            Me.btnforwardgrey.Visible = False
                            load_parameters()
                            prev_mode = 2
                            mode = 100
                        Else
                            Me.WindowState = System.Windows.Forms.FormWindowState.Minimized
                            bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve create", "Selecting Product...", 20, True, True)


                            If oproductselection.launch_product(gpub_type_id, entity_id, Nothing, sub_entity_id) = bc_cs_error_codes.RETURN_ERROR Then
                                Me.WindowState = System.Windows.Forms.FormWindowState.Normal
                            End If
                            bc_cs_central_settings.progress_bar.unload()
                            load_pub_type()
                            mode = 1
                        End If

                Case 10
                        REM load documents
                        Me.proddesc.Visible = False
                        load_open_controls()
                        Me.changed = False

                        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.LOCAL Then
                            Me.PictureBox4.Enabled = False
                            Me.PictureBox5.Enabled = False
                            Me.PictureBox6.Enabled = True
                            Me.PictureBox4.BorderStyle = BorderStyle.None
                            Me.PictureBox5.BorderStyle = BorderStyle.None
                            Me.PictureBox6.BorderStyle = BorderStyle.Fixed3D
                            load_docs(True, True)
                        Else
                            Me.PictureBox4.Enabled = True
                            Me.PictureBox5.Enabled = True
                            Me.PictureBox6.Enabled = True
                            Me.PictureBox4.BorderStyle = BorderStyle.Fixed3D
                            Me.PictureBox5.BorderStyle = BorderStyle.None
                            Me.PictureBox6.BorderStyle = BorderStyle.None
                            load_docs(False, True)
                        End If
                        'Me.ListView1.Width = 440
                        mode = 11
                        BlueCurve_TextSearch1.Enabled = False
                        BlueCurve_TextSearch1.Visible = False
                    'ListBox1.Location = New System.Drawing.Point(88, 112)
                    ListBox1.Left = 120
                    description.Left = 88
                        ListBox1.Size = New System.Drawing.Size(232, 139)

                Case 11
                        REM open documents
                        REM local documents
                        REM get doc_id
                        Dim doc_id As String
                        Me.proddesc.Visible = False
                        doc_id = ListView1.SelectedItems(0).SubItems(5).Text
                        For i = 0 To oopen_doc.onetwork_docs.document.Count - 1
                            If IsNumeric(doc_id) Then
                                If oopen_doc.onetwork_docs.document(i).id = doc_id Then
                                    Exit For
                                End If
                            Else
                                If oopen_doc.onetwork_docs.document(i).filename = doc_id Then
                                    Exit For
                                End If
                            End If
                        Next
                        If Me.PictureBox4.BorderStyle = BorderStyle.None Then
                            REM check document isnt checked out
                            If oopen_doc.onetwork_docs.document(i).checked_out_user > 0 Then
                                Dim omessage As New bc_cs_message("Blue Curve create", "Document is Checked Out Cannot Open.", bc_cs_message.MESSAGE)
                                Exit Sub
                            End If
                        End If

                        Me.WindowState = System.Windows.Forms.FormWindowState.Minimized
                        bcprog = New bc_cs_progress("Blue Curve create", "Opening Document...", 4, False, False)
                        oopen_doc.onetwork_docs.document(i).local_index = i
                        bc_am_load_objects.obc_current_document = oopen_doc.onetwork_docs.document(i)
                        oopen_doc.open(Me.PictureBox4.BorderStyle = BorderStyle.FixedSingle, bc_am_load_objects.obc_current_document)
                        bcprog.unload()
                        Me.ListView1.SelectedItems.Clear()
                        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.LOCAL Then
                            load_docs(True, True)
                        Else
                            If PictureBox5.BorderStyle = BorderStyle.Fixed3D Then
                                load_docs(False, False)
                            Else
                                load_docs(False, True)
                            End If
                        End If
                        Me.WindowState = System.Windows.Forms.FormWindowState.Minimized

                        BlueCurve_TextSearch1.Enabled = False
                        BlueCurve_TextSearch1.Visible = False
                    'ListBox1.Location = New System.Drawing.Point(88, 112)
                    ListBox1.Left = 88
                    description.Left = 88
                        ListBox1.Size = New System.Drawing.Size(232, 139)

            End Select
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_at_wizard_main", "btnforwardgrey_Mouseup", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            otrace = New bc_cs_activity_log("bc_am_at_wizard_main", "btnforwardgrey_Mouseup", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Private Sub invoke_composite(ByVal entity_id As Long, ByVal pub_type As bc_om_pub_type, Optional ByVal customParam As String = "")
        Me.description.Text = "Default Composite Build List"
        bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve create", "Retrieving Composite Document List...", 1, False, True)
        bc_cs_central_settings.progress_bar.show()
        bc_cs_central_settings.progress_bar.refresh()
        bc_am_load_objects.obc_om_composite_pub_type = pub_type
        Me.Cursor = Cursors.WaitCursor
        Me.Refresh()
        bc_am_composite_doc = New bc_am_composite_docs(entity_id, customParam)
        bc_cs_central_settings.progress_bar.unload()
        Dim bcs As New bc_cs_central_settings(True)
        If bc_am_composite_doc.get_component_documents = True Then
            If bc_cs_central_settings.use_advanced_composite_build = True Then
                Dim oadv As New bc_am_adv_composite_build
                Me.Cursor = Cursors.Default
                oadv.ShowDialog(Me)
                If oadv.proceed = True Then
                    Me.WindowState = FormWindowState.Minimized

                    bc_am_composite_doc.merge()
                    mode = 0

                    load_start()
                    Me.WindowState = FormWindowState.Minimized

                Else
                    Me.WindowState = FormWindowState.Normal
                End If
            Else

                load_composite_list(-1)
                Me.Cursor = Cursors.Default

                mode = 20

                Me.btnforwardgrey.Visible = False
                Me.bok.Left = BtnPlace3.Left
                Me.bok.Visible = True
                Me.last_screen = True
                Me.btnup.Visible = True
                Me.btnremove.Visible = True
                Me.btndown.Visible = True
                Me.btnup.Enabled = False
                Me.btnremove.Enabled = False
                Me.btndown.Enabled = False
            End If
        End If
        Me.Cursor = Cursors.Default
    End Sub
    Private Sub load_composite_list(ByVal index As Integer)
        Try
            Me.ListBox1.Items.Clear()
            Dim i As Integer
            For i = 0 To bc_am_load_objects.obc_om_composite_pub_type.composite_composition.documents.document.Count - 1
                With bc_am_load_objects.obc_om_composite_pub_type.composite_composition.documents.document(i)
                    Me.ListBox1.Items.Add(.title)
                End With
            Next
            If index > -1 Then
                ListBox1.SelectedIndex = index
            End If
        Catch ex As Exception
            MsgBox(ex.Message)

        End Try
    End Sub
    Private Function check_for_parameters() As Boolean
        Dim otrace As New bc_cs_activity_log("bc_am_at_build_wizard", "check_for_parameters", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim i As Integer
            check_for_parameters = False
            For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                With bc_am_load_objects.obc_pub_types.pubtype(i)
                    If .id = gpub_type_id Then
                        If (.parameters.parameters.count) > 0 Then
                            check_for_parameters = .parameters.parameters.count
                        End If
                        Exit For
                    End If
                End With
            Next

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_at_wizard_main", "check_for_parameters", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_am_at_wizard_main", "check_for_parameters", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
    Private Sub load_parameters()
        Dim otrace As New bc_cs_activity_log("bc_am_at_build_wizard", "load_parameters", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim i, j As Integer
            Me.ListBox1.Visible = False
            Me.description.Text = "Please enter the following options:"
            For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                With bc_am_load_objects.obc_pub_types.pubtype(i)
                    If .id = gpub_type_id Then
                        For j = 0 To .parameters.parameters.count - 1
                            REM allow up to six parameters
                            If j = 6 Then
                                Exit For
                            End If
                            With .parameters.parameters(j)
                                If .type = 10 Then
                                    Select Case j
                                        Case 0
                                            Me.ptext1.Visible = True
                                        Case 1
                                            Me.ptext2.Visible = True
                                        Case 2
                                            Me.ptext3.Visible = True
                                        Case 3
                                            Me.ptext4.Visible = True
                                        Case 4
                                            Me.ptext5.Visible = True
                                        Case 5
                                            Me.ptext6.Visible = True
                                    End Select
                                End If
                                Select Case j

                                    Case 0
                                        Me.pLabel1.Visible = True
                                        Me.pLabel1.Text = .caption
                                    Case 1
                                        Me.pLabel2.Visible = True
                                        Me.pLabel2.Text = .caption
                                    Case 2
                                        Me.pLabel3.Visible = True
                                        Me.pLabel3.Text = .caption
                                    Case 3
                                        Me.pLabel4.Visible = True
                                        Me.pLabel4.Text = .caption
                                    Case 4
                                        Me.pLabel5.Visible = True
                                        Me.pLabel5.Text = .caption
                                    Case 5
                                        Me.pLabel6.Visible = True
                                        Me.pLabel6.Text = .caption
                                        REM text entry
                                End Select
                            End With
                        Next
                    End If
                End With
            Next

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_at_wizard_main", "load_parameters", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_am_at_wizard_main", "load_parameters", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Private Sub bleftgrey_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles bleftgrey.MouseDown
        Me.bleftgrey.Visible = False
        Me.Btnback.Left = BtnPlace2.Left
        Me.Btnback.Visible = True

    End Sub
    Private Sub bleftgrey_Mouseup(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles bleftgrey.MouseUp
        Me.last_screen = False
        Me.bskip.Enabled = False
        Me.bleftgrey.Left = BtnPlace2.Left
        Me.bleftgrey.Visible = True
        Me.Btnback.Visible = False
        Me.Btntoggle.Visible = False
        Me.btnforwardgrey.Visible = False
        Me.btnforwardgrey.Left = BtnPlace3.Left
        Me.btnforwardgrey.Visible = True
        Me.proddesc.Visible = False
        Dim k As Integer
        Select Case mode
            Case 1
                If Me.from_workflow = True Then
                    Me.Hide()
                    Exit Sub
                End If
                load_start()
                mode = 0
            Case 2
                Me.proddesc.Visible = True
                load_pub_type()
                BlueCurve_TextSearch1.Enabled = False
                BlueCurve_TextSearch1.Visible = False
                'ListBox1.Location = New System.Drawing.Point(88, 112)
                ListBox1.Left = 88
                description.Left = 88
                ListBox1.Size = New System.Drawing.Size(232, 139)
                mode = 1
                Me.ListBox1.SelectedIndex = prev_list_index
            Case 4
                Me.proddesc.Visible = False
                load_entities(gclass_id, btoggle)
                mode = 2
                Me.btnforwardgrey.Left = BtnPlace3.Left
                Me.btnforwardgrey.Visible = True
                Me.bok.Visible = False
                'Me.ListBox1.SelectedIndex = prev_list_index
            Case 20
                Me.proddesc.Visible = True
                load_pub_type()
                Me.btnup.Visible = False
                Me.btndown.Visible = False
                Me.btnremove.Visible = False
                mode = 1
                Me.ListBox1.SelectedIndex = prev_list_index
            Case 100
                clear_parameters()
                Me.btnforwardgrey.Left = BtnPlace3.Left
                Me.btnforwardgrey.Visible = True
                Me.bok.Visible = False
                MyBase.PictureBox3.Visible = False
                MyBase.Pubthumbnail.Visible = False
                If prev_mode = 2 Then
                    Me.ListBox1.Visible = True
                    If prev_toggle = True Then
                        Me.Btntoggle.Text = "All Subjects"
                        For k = 0 To form_strings.form_labels.Count - 1
                            If form_strings.form_labels(k).label_code = "create.main.all" Then
                                Me.Btntoggle.Text = form_strings.form_labels(k).text
                                Exit For
                            End If
                        Next

                    Else
                        Me.Btntoggle.Text = "My Subjects"
                        For k = 0 To form_strings.form_labels.Count - 1
                            If form_strings.form_labels(k).label_code = "create.main.mine" Then
                                Me.Btntoggle.Text = form_strings.form_labels(k).text
                                Exit For
                            End If
                        Next

                    End If
                    load_entities(prev_child_category, prev_toggle)
                    mode = 2
                    Me.ListBox1.SelectedIndex = prev_list_index2
                    btoggle = Not btoggle
                ElseIf prev_mode = 1 Then
                    Me.ListBox1.Visible = True
                    mode = 1
                Else
                    load_pub_type()
                    mode = 1
                    Me.ListBox1.SelectedIndex = prev_list_index
                End If
        End Select

    End Sub

    Private Sub bcancelgrey_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles bcancelgrey.MouseDown
        Me.bcancelgrey.Visible = False
        Me.Btncancel.Visible = True
    End Sub
    Private Sub bcancelgrey_Mouseup(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles bcancelgrey.MouseUp
        Me.bcancelgrey.Visible = True
        Me.Btncancel.Visible = False
        Me.Btntoggle.Visible = False

        If mode = 0 Or from_workflow Then
            Try
                Me.tthread.Abort()
            Catch

            End Try
            Me.Hide()
        Else
            mode = 0
            load_start()
        End If
    End Sub

    Private Sub btncan_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btncan.Click
        Me.gsettings.size = Me.Size
        save_settings(Me.gsettings)
        load_start()
    End Sub

    Private Sub btnopen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnopen.Click

        open_doc()

        Me.WindowState = FormWindowState.Normal
        Me.btnforwardgrey.Left = BtnPlace3.Left
        Me.bcancelgrey.Left = BtnPlace2.Left
        Me.Btncancel.Left = BtnPlace2.Left
        Me.bcancelgrey.Left = BtnPlace2.Left
        Me.Btnback.Left = BtnPlace2.Left
        Me.bleftgrey.Left = BtnPlace2.Left
      
    End Sub
    Public Sub open_doc()
        Dim otrace As New bc_cs_activity_log("bc_am_at_wizard_main", "btnopen_click", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim abort_open As Boolean = False

        Try
            Dim k As Integer
            Me.gsettings.size = Me.Size
            save_settings(Me.gsettings)
            For k = 0 To ListView1.SelectedItems.Count - 1

                REM open documents
                REM local documents
                REM get doc_id
                Dim doc_id As String
                Dim i As Integer
                doc_id = ListView1.SelectedItems(k).SubItems(6).Text
                'Rama Comments: If working offline get the subitem 5 as it contains documentid
                If doc_id = "" Then
                    doc_id = ListView1.SelectedItems(k).SubItems(5).Text
                End If
                For i = 0 To oopen_doc.onetwork_docs.document.Count - 1
                    If IsNumeric(doc_id) Then
                        If oopen_doc.onetwork_docs.document(i).id = doc_id Then
                            Exit For
                        End If
                    Else
                        If oopen_doc.onetwork_docs.document(i).filename = doc_id Then
                            Exit For
                        End If
                    End If
                Next
                If Me.PictureBox6.BorderStyle = BorderStyle.None Then
                    REM check document isnt checked out
                    If oopen_doc.onetwork_docs.document(i).checked_out_user > 0 Then
                        Dim omessage As New bc_cs_message("Blue Curve create", "Document selection contains a checked Out Document Cannot Open.", bc_cs_message.MESSAGE)
                        abort_open = True
                        Exit Sub
                    End If
                End If


                Me.WindowState = System.Windows.Forms.FormWindowState.Minimized
                If ListView1.SelectedItems.Count = 1 Then
                    bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve create", "Opening Document...", 4, False, True)
                Else
                    bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve create", "Opening Document " + CStr(k + 1) + " of " + CStr(ListView1.SelectedItems.Count), 4, False, True)
                End If
                oopen_doc.onetwork_docs.document(i).local_index = i
                bc_am_load_objects.obc_current_document = oopen_doc.onetwork_docs.document(i)
                If Me.PictureBox6.BorderStyle = BorderStyle.Fixed3D Then
                    oopen_doc.open(True, bc_am_load_objects.obc_current_document)
                Else
                    oopen_doc.open(False, bc_am_load_objects.obc_current_document)
                End If
                bc_cs_central_settings.progress_bar.unload()
            Next
            Me.ListView1.SelectedItems.Clear()
            Me.WindowState = FormWindowState.Normal
            'load_start()
            'mode = 0
            'Me.Invalidate()



        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_at_wizard_main", "btnopen_click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            If abort_open = False Then
                load_start(True)
                Me.WindowState = FormWindowState.Minimized
                mode = 0
            End If
            otrace = New bc_cs_activity_log("bc_am_at_wizard_main", "btnopen_click", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Private Sub lstages_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstages.SelectedIndexChanged
        If resetting = False Then
            load_docs(blocal_docs, ball, False)
            iselected_stage = lstages.SelectedItem
        End If
    End Sub
    Private Sub lpubtypes_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lpubtypes.SelectedIndexChanged
        If resetting = False Then
            load_docs(blocal_docs, ball, False)
            iselected_pub_type = lpubtypes.SelectedItem
        End If
    End Sub

    Private Sub psortbytitle_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles psortbytitle.Click
        CompareByname.toggle = True
        ListView1.ListViewItemSorter() = New CompareByname
        psortbytitle.BorderStyle = BorderStyle.Fixed3D
        psortbydate.BorderStyle = BorderStyle.None
        psortbyauthor.BorderStyle = BorderStyle.None
    End Sub

    Private Sub psortbydate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles psortbydate.Click
        CompareBydate.toggle = True
        ListView1.ListViewItemSorter() = New CompareBydate
        psortbytitle.BorderStyle = BorderStyle.None
        psortbydate.BorderStyle = BorderStyle.Fixed3D
        psortbyauthor.BorderStyle = BorderStyle.None
    End Sub

    Private Sub psortbyauthor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles psortbyauthor.Click
        CompareByco.toggle = True
        ListView1.ListViewItemSorter() = New CompareByco
        psortbytitle.BorderStyle = BorderStyle.None
        psortbydate.BorderStyle = BorderStyle.None
        psortbyauthor.BorderStyle = BorderStyle.Fixed3D
    End Sub
    Private Sub psortbyitem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles psortbyauthor.Click
        CompareByco.toggle = True
        ListView1.ListViewItemSorter() = New CompareByitem
        psortbytitle.BorderStyle = BorderStyle.None
        psortbydate.BorderStyle = BorderStyle.None
        psortbyauthor.BorderStyle = BorderStyle.Fixed3D
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub
    Private Sub PictureBox4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox4.Click
        Dim otrace As New bc_cs_activity_log("bc_am_at_wizard_main", "PictureBox4_Click", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            PictureBox4.BorderStyle = BorderStyle.Fixed3D
            PictureBox5.BorderStyle = BorderStyle.None
            PictureBox6.BorderStyle = BorderStyle.None
            If mode = 11 Then
                load_docs(False, True)
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_at_wizard_main", "PictureBox4_Click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_am_at_wizard_main", "PictureBox4_Click", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    Private Sub PictureBox5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox5.Click
        Dim otrace As New bc_cs_activity_log("bc_am_at_wizard_main", "PictureBox5_Click", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            PictureBox4.BorderStyle = BorderStyle.None
            PictureBox5.BorderStyle = BorderStyle.Fixed3D
            PictureBox6.BorderStyle = BorderStyle.None
            If mode = 11 Then
                load_docs(False, False)
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_at_wizard_main", "PictureBox5_Click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_am_at_wizard_main", "PictureBox5_Click", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Private Sub PictureBox6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox6.Click
        Dim otrace As New bc_cs_activity_log("bc_am_at_wizard_main", "PictureBox6_Click", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            PictureBox4.BorderStyle = BorderStyle.None
            PictureBox5.BorderStyle = BorderStyle.None
            PictureBox6.BorderStyle = BorderStyle.Fixed3D
            If mode = 11 Then
                load_docs(True, True)
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_at_wizard_main", "PictureBox6_Click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_am_at_wizard_main", "PictureBox6_Click", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Private Sub set_summary_for_document()
        Dim otrace As New bc_cs_activity_log("bc_am_at_wizard_main", "set_summary_for_document", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            Dim i As Integer
            Dim j As Integer
            Dim doc_id As String = ""

            Me.txtsummary.Text = ""

            Try
                doc_id = ListView1.SelectedItems(0).SubItems(6).Text
            Catch

            End Try


            For i = 0 To oopen_doc.onetwork_docs.document.Count - 1
                With oopen_doc.onetwork_docs.document(i)
                    If CStr(.id) = doc_id Or (.filename) = doc_id Then
                        REM concatentate metadata
                        Me.txtsummary.Text = ""
                        Me.txtsummary.Text = "Title: " & vbCr
                        Me.txtsummary.Text = Me.txtsummary.Text + .title & vbCr
                        Me.txtsummary.Text = Me.txtsummary.Text + "Sub Title: " & vbCr
                        Me.txtsummary.Text = Me.txtsummary.Text + .sub_title & vbCr
                        Me.txtsummary.Text = Me.txtsummary.Text + "Summary: " & vbCr
                        Me.txtsummary.Text = Me.txtsummary.Text + .summary & vbCr
                        Me.txtsummary.Text = Me.txtsummary.Text + "Date: " & vbCr
                        Me.txtsummary.Text = Me.txtsummary.Text + .doc_date & vbCr
                        Me.txtsummary.Text = Me.txtsummary.Text + "Author: " & vbCr
                        For j = 0 To bc_am_load_objects.obc_users.user.Count - 1
                            If bc_am_load_objects.obc_users.user(j).id = .originating_author Then
                                Me.txtsummary.Text = Me.txtsummary.Text + bc_am_load_objects.obc_users.user(j).first_name + " " + bc_am_load_objects.obc_users.user(j).surname & vbCr
                            End If
                        Next
                        Me.txtsummary.Text = Me.txtsummary.Text + "Number of Pages: " & vbCr
                        Me.txtsummary.Text = Me.txtsummary.Text + CStr(.pages) & vbCr
                        Me.txtsummary.Text = Me.txtsummary.Text + "Document Status: " & vbCr
                        Me.txtsummary.Text = Me.txtsummary.Text + .stage_name & vbCr
                        If .checked_out_user > 0 Then
                            Me.txtsummary.Text = Me.txtsummary.Text + "Checked out by: " & vbCr
                            For j = 0 To bc_am_load_objects.obc_users.user.Count - 1
                                If bc_am_load_objects.obc_users.user(j).id = .checked_out_user Then
                                    Me.txtsummary.Text = Me.txtsummary.Text + bc_am_load_objects.obc_users.user(j).first_name + " " + bc_am_load_objects.obc_users.user(j).surname
                                End If
                            Next
                        End If
                    End If
                End With
            Next
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_at_wizard_main", "set_summary_for_document", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            otrace = New bc_cs_activity_log("bc_am_at_wizard_main", "set_summary_for_document", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    Private Sub psummary_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles psummary.Click
        toggle_summary()
    End Sub
    Public Sub invoke_clone(ByVal pt_id As Long, ByVal entity_id As Long, ByVal sub_entity_id As Long)
        Dim otrace As New bc_cs_activity_log("bc_am_at_wizard_main", "invoke_clone", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim bcs As New bc_cs_progress("Blue Curve", "Retrieving Document List", 0, False, True)

        Try

            Dim oclone As New bc_om_clone_document_list(pt_id, entity_id, sub_entity_id)
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                oclone.db_read()
            Else
                oclone.tmode = bc_cs_soap_base_class.tREAD
                If oclone.transmit_to_server_and_receive(oclone, True) = False Then
                    Exit Sub
                End If
            End If
            bcs.unload()

            If oclone.documents.document.Count = 0 Then
                Dim omsg As New bc_cs_message("Blue Curve", "There are no previous documents for this combination. Please create a new document.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Me.last_screen = True
                Me.Chkclone.Checked = False
                Exit Sub
            End If

            Dim fclone As New bc_am_clone_document
            Dim pt_name As String = ""
            Dim ent_name As String = ""
            Dim sub_ent_name As String = ""
            For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                If bc_am_load_objects.obc_pub_types.pubtype(i).id = pt_id Then
                    pt_name = bc_am_load_objects.obc_pub_types.pubtype(i).name
                    Exit For
                End If
            Next
            If entity_id <> 0 Then
                For i = 0 To bc_am_load_objects.obc_entities.entity.Count - 1
                    If bc_am_load_objects.obc_entities.entity(i).id = entity_id Then
                        ent_name = bc_am_load_objects.obc_entities.entity(i).name
                        If sub_entity_id = 0 Then
                            Exit For
                        End If
                    End If
                    If sub_entity_id <> 0 Then
                        If bc_am_load_objects.obc_entities.entity(i).id = sub_entity_id Then
                            sub_ent_name = bc_am_load_objects.obc_entities.entity(i).name
                        End If
                    End If
                Next
            End If
            If entity_id = 0 Then
                fclone.ltitle.Text = "Please select a report of type: " + pt_name + " to clone from"
            ElseIf sub_entity_id = 0 Then
                fclone.ltitle.Text = "Please select a report of type: " + pt_name + " for: " + ent_name + " to clone from"
            Else
                fclone.ltitle.Text = "Please select a report of type: " + pt_name + " for: " + ent_name + " and " + sub_ent_name + " to clone from"
            End If
            'fclone.load_list(oclone.documents.document)

            Dim oapi As New API
            API.SetWindowPos(fclone.Handle.ToInt32, API.HWND_TOPMOST, 10, 10, 1, 1, 1)
            fclone.ShowDialog()
            'If fclone.proceed = True Then
            '    mode = 0
            '    load_start()
            '    Me.WindowState = System.Windows.Forms.FormWindowState.Minimized
            '    Dim ocopydoc As New bc_am_copy_document(fclone.mdoc_id, fclone.sdoc_id)
            '    If ocopydoc.copy_doc = True Then
            '        mode = 0
            '        load_start()
            '        Me.WindowState = System.Windows.Forms.FormWindowState.Minimized
            '    End If
            'End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_at_wizard_main", "invoke_clone", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            bcs.unload()
            otrace = New bc_cs_activity_log("bc_am_at_wizard_main", "invoke_clone", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try



    End Sub


    Private Sub toggle_summary()
        If bsummary = True Then
            bsummary = False
            Me.txtsummary.Visible = False
            Me.bedit.Visible = False
            Me.psummary.BorderStyle = BorderStyle.None
            'Me.ListView1.Width = 440
        Else
            If Me.ListView1.SelectedItems.Count > 0 Then
                bsummary = True
                Me.txtsummary.Visible = True
                If bc_cs_central_settings.selected_conn_method <> bc_cs_central_settings.LOCAL And Me.ListView1.SelectedItems(0).ImageIndex < 0 Then
                    Me.bedit.Visible = True
                End If
                Me.psummary.BorderStyle = BorderStyle.Fixed3D
            End If
            'Me.ListView1.Width = 245
        End If
    End Sub
    Private Sub lbusareas_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbusareas.SelectedIndexChanged
        If resetting = False Then
            load_docs(blocal_docs, ball, False)
            iselected_bus_area = lbusareas.SelectedItem
        End If
    End Sub
    Private Sub clear_parameters()
        Me.pLabel1.Visible = False
        Me.pLabel2.Visible = False
        Me.pLabel3.Visible = False
        Me.pLabel4.Visible = False
        Me.pLabel5.Visible = False
        Me.pLabel6.Visible = False
        Me.ptext1.Visible = False
        Me.ptext2.Visible = False
        Me.ptext3.Visible = False
        Me.ptext4.Visible = False
        Me.ptext5.Visible = False
        Me.ptext6.Visible = False
    End Sub

    Private Sub bok_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles bok.MouseDown
        Me.bokgrey.Left = BtnPlace3.Left
        Me.bokgrey.Visible = True
        Me.bok.Visible = False
    End Sub

    Private Sub bok_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles bok.MouseUp
        sub_entity_id = 0

        report_idx = Me.ListBox1.SelectedIndex
        Me.WindowState = System.Windows.Forms.FormWindowState.Minimized
        REM load parameter values into object model
        Dim i As Integer
        Dim j As Integer = 0
        'Dim params As New bc_om_parameters
        Dim oproductselection As New bc_am_product_selection

        params = Nothing
        REM if import file 
        If attach_doc = True Then
            If BlueCurve_TextSearch1.ShownEntityList.Count > 0 Then
                If mode = 4 Then
                    sub_entity_id = BlueCurve_TextSearch1.ShownEntityList(Me.ListBox1.SelectedIndex)
                Else
                    entity_id = BlueCurve_TextSearch1.ShownEntityList(Me.ListBox1.SelectedIndex)
                End If
                'entity_id = get_entity_id_selected(Me.ListBox1.SelectedIndex)
            Else
                entity_id = 0
            End If

            attach_selected = True
            Me.Hide()
            Exit Sub
        End If
        If mode = 200 Then

            REM regular reports
            If BlueCurve_TextSearch1.ShownEntityList.Count > 0 Then
                'prev_list_index = Me.ListBox1.SelectedIndex
                prev_list_index = BlueCurve_TextSearch1.ShownEntityList(Me.ListBox1.SelectedIndex)
            Else
                entity_id = 0
            End If

            'Me.Btncancel.Left = 356
            'Me.bcancelgrey.Left = 356
            Me.Btncancel.Left = BtnPlace1.Left
            Me.bcancelgrey.Left = BtnPlace1.Left
            Me.Btncancel.Visible = False
            Me.bcancelgrey.Visible = False
            'Me.Btnback.Left = 416
            'Me.bleftgrey.Left = 416
            Me.Btnback.Left = BtnPlace2.Left
            Me.bleftgrey.Left = BtnPlace2.Left


            Exit Sub
        End If
        If mode = 20 Then
            REM composite build

            REM Steve Wooderson 26/06/2013
            REM If creating a new document from word do not create the composite here.
            REM We will do it from AX
            buildMode = mode
            If from_create = True Or (from_create = False And from_workflow = False) Then
                bc_am_composite_doc.merge()
            End If

        Else
            For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                With bc_am_load_objects.obc_pub_types.pubtype(i)
                    If .id = gpub_type_id Then
                        For j = 0 To .parameters.parameters.count - 1
                            With bc_am_load_objects.obc_pub_types.pubtype(i).parameters.parameters(j)
                                If .type = 10 Then
                                    Select Case j
                                        Case 0
                                            .value = ptext1.Text
                                        Case 1
                                            .value = ptext2.Text
                                        Case 2
                                            .value = ptext3.Text
                                        Case 3
                                            .value = ptext4.Text
                                        Case 4
                                            .value = ptext5.Text
                                        Case 5
                                            .value = ptext6.Text
                                    End Select
                                End If
                            End With
                        Next
                        params = bc_am_load_objects.obc_pub_types.pubtype(i).parameters
                        Exit For
                    End If
                End With
            Next
            If Me.ListBox1.SelectedIndex = -1 Then
                Dim bc_cs_message As New bc_cs_message("Blue Curve create", "Item must be selected!", bc_cs_message.MESSAGE)
                Select Case True
                    Case bc_cs_central_settings.show_all_entities
                        Me.Btntoggle.Visible = True
                    Case Not bc_cs_central_settings.show_all_entities
                        Me.Btntoggle.Visible = False
                End Select
                bc_cs_central_settings.progress_bar.unload()
                'Me.bok.Left = BtnPlace3.Left
                Me.bok.Visible = True
                Me.last_screen = True
                Me.bokgrey.Visible = False
                Me.WindowState = FormWindowState.Normal
                Exit Sub
            Else
                entity_id = 0

                If mode <> 4 And gchild_category <> 0 Then
                    If BlueCurve_TextSearch1.ShownEntityList.Count > 0 Then
                        Try
                            entity_id = BlueCurve_TextSearch1.ShownEntityList(Me.ListBox1.SelectedIndex)
                        Catch
                            entity_id = 0
                        End Try
                        'entity_id = get_entity_id_selected(Me.ListBox1.SelectedIndex)
                    Else
                        entity_id = 0
                    End If
                Else
                    REM EFG June 2012
                    entity_id = gentity_id
                End If
            End If
            REM BMO Sept 2012
            sub_entity_id = 0
            REM =============
            If mode = 4 Then
                If BlueCurve_TextSearch1.ShownEntityList.Count > 0 Then
                    'sub_entity_id = get_entity_id_selected(Me.ListBox1.SelectedIndex)
                    sub_entity_id = BlueCurve_TextSearch1.ShownEntityList(Me.ListBox1.SelectedIndex)
                Else
                    sub_entity_id = 0
                End If
            End If

            bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve create", "Initializing Build...", 20, True, True)

            REM Steve Wooderson 26/06/2013
            REM If creating a new document from word do not launch here.
            REM We will do it from AX
            buildMode = mode
            If from_create = True Or (from_create = False And from_workflow = False) Then
                If oproductselection.launch_product(gpub_type_id, entity_id, params, Me.sub_entity_id) = bc_cs_error_codes.RETURN_ERROR Then
                    Me.WindowState = System.Windows.Forms.FormWindowState.Normal
                End If
            End If

        End If

        Me.ListView1.SelectedItems.Clear()
        ptext1.Text = ""
        ptext2.Text = ""
        ptext3.Text = ""
        ptext4.Text = ""
        ptext5.Text = ""
        ptext6.Text = ""

        bc_cs_central_settings.progress_bar.unload()
        mode = 0
        load_start()
        Me.WindowState = System.Windows.Forms.FormWindowState.Minimized
        If Me.from_workflow = True Then
            Me.Hide()
        End If
    End Sub





    Private Sub btnup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnup.Click
        Try
            Dim i As Integer
            i = Me.ListBox1.SelectedIndex
            REM swap over values
            If i = 0 Then
                Exit Sub
            End If
            Dim tmpdoc As New bc_om_document
            With bc_am_load_objects.obc_om_composite_pub_type.composite_composition
                tmpdoc = .documents.document(i)
                .documents.document.RemoveAt(i)
                .documents.document.Insert(i - 1, tmpdoc)
            End With
            load_composite_list(i - 1)
        Catch

        End Try
    End Sub
    Private Sub btndown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btndown.Click
        Dim i As Integer
        REM swap over values
        i = Me.ListBox1.SelectedIndex
        If i = Me.ListBox1.Items.Count - 1 Then
            Exit Sub
        End If
        Dim tmpdoc As New bc_om_document
        With bc_am_load_objects.obc_om_composite_pub_type.composite_composition
            tmpdoc = .documents.document(i)
            .documents.document.RemoveAt(i)
            .documents.document.Insert(i + 1, tmpdoc)
        End With
        load_composite_list(i + 1)

    End Sub

    Private Sub btnremove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnremove.Click
        With bc_am_load_objects.obc_om_composite_pub_type.composite_composition
            .documents.document.RemoveAt(Me.ListBox1.SelectedIndex)
        End With
        Me.btnremove.Enabled = False
        Me.btnup.Enabled = False
        Me.btndown.Enabled = False
        load_composite_list(-1)
    End Sub

    Private Sub bleftgrey_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bleftgrey.Click

    End Sub

    Private Sub DateTimePicker1_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateTimePicker1.ValueChanged
        changed = True

    End Sub

    Private Sub DateTimePicker2_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateTimePicker2.ValueChanged
        changed = True
    End Sub

    Private Sub DateTimePicker1_mouseleave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateTimePicker1.Leave

        If changed = True Then
            If Me.PictureBox6.BorderStyle = BorderStyle.Fixed3D Then
                load_docs(True, True, True)
            ElseIf Me.PictureBox4.BorderStyle = BorderStyle.Fixed3D Then
                load_docs(False, True, True)
            Else
                load_docs(False, False, True)
            End If
            changed = False
        End If

    End Sub
    Private Sub DateTimePicker2_mouseleave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateTimePicker2.Leave
        If changed = True Then
            If Me.PictureBox6.BorderStyle = BorderStyle.Fixed3D Then
                load_docs(True, True, True)
            ElseIf Me.PictureBox4.BorderStyle = BorderStyle.Fixed3D Then
                load_docs(False, True, True)
            Else
                load_docs(False, False, True)
            End If
            changed = False
        End If
    End Sub

    Private Sub bedit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bedit.Click
        Try
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.LOCAL Then
                Exit Sub
            End If
            bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve", "Retrieving Metata...", 0, False, True)

            Dim tid As Long
            Dim ldoc As New bc_om_document

            ldoc.id = ListView1.SelectedItems(0).SubItems(6).Text
            tid = ldoc.id
            ldoc.bcheck_out = False
            ldoc.bwith_document = False
            ldoc.btake_revision = False
            REM changethis back for master this is ti et 5.4 to work
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                'ldoc.db_read()
                ldoc.db_read_for_create()
            Else
                ldoc.read_mode = bc_om_document.READ_FOR_CREATE
                ldoc.tmode = bc_cs_soap_base_class.tREAD
                ldoc.transmit_to_server_and_receive(ldoc, True)
            End If
            bc_cs_central_settings.progress_bar.unload()
            ldoc.id = tid
            Dim osubmit As New bc_am_at_submit_frm

            osubmit.Text = "Blue Curve - edit metadata"
            osubmit.edit_mode = True
            osubmit.ldoc = ldoc
            osubmit.ShowDialog()
            If osubmit.ok_selected Then
                bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve", "Updating Medata...", 0, False, True)
                ldoc = osubmit.ldoc
                ldoc.id = tid
                REM write metadata to system
                ldoc.history.Clear()
                ldoc.support_documents.Clear()
                'ldoc.history.Add("Metadata Changed")
                ldoc.bcheck_out = False
                ldoc.bwith_document = False
                ldoc.register_only = False
                ldoc.bimport_support_only = True
                ldoc.master_flag = True
                ldoc.id = tid
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    ldoc.db_write(Nothing)
                Else
                    ldoc.tmode = bc_cs_soap_base_class.tWRITE
                    ldoc.no_send_back = True
                    ldoc.transmit_to_server_and_receive(ldoc, True)
                End If
                bc_cs_central_settings.progress_bar.unload()
                If Me.PictureBox6.BorderStyle = BorderStyle.Fixed3D Then
                    load_docs(True, True, True)
                ElseIf Me.PictureBox4.BorderStyle = BorderStyle.Fixed3D Then
                    load_docs(False, True, True)
                Else
                    load_docs(False, False, True)
                End If
                toggle_summary()

            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally

        End Try

    End Sub

    Private Sub ListView1_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListView1.MouseEnter
        If Me.changed = True Then
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.LOCAL Then
                load_docs(True, True, True)
            Else
                If PictureBox5.BorderStyle = BorderStyle.Fixed3D Then
                    load_docs(False, False, True)
                Else
                    load_docs(False, True, True)
                End If
            End If
            Me.changed = False
        End If
    End Sub


    Private Sub bc_am_at_wizard_main_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.MouseEnter
        Try
            If Me.from_alert = True Then
                REM reset form from top most
                Dim oapi As New API
                Me.StartPosition = FormStartPosition.CenterScreen
                Me.lxpos = Me.Location.X
                Me.lypos = Me.Location.Y
                API.SetWindowPos(Me.Handle.ToInt32, API.HWND_NOTOPMOST, Me.lxpos, Me.lypos, 1, 1, 1)

                Me.PictureBox4.Enabled = True
                Me.PictureBox5.Enabled = True
                Me.PictureBox6.Enabled = True
                Me.PictureBox4.BorderStyle = BorderStyle.Fixed3D
                Me.PictureBox5.BorderStyle = BorderStyle.None
                Me.PictureBox6.BorderStyle = BorderStyle.None
                Me.PictureBox4.Visible = True
                Me.PictureBox5.Visible = True
                Me.PictureBox6.Visible = True
                Me.from_alert = False
                Exit Sub
            End If

            If Me.changed = True Then
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.LOCAL Then
                    load_docs(True, True, True)
                Else
                    If PictureBox5.BorderStyle = BorderStyle.Fixed3D Then
                        load_docs(False, False, True)
                    Else
                        load_docs(False, True, True)
                    End If
                End If
                Me.changed = False
            End If
        Catch

        End Try
    End Sub

    Private Sub cpublish_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cpublish.CheckedChanged
        If Me.cpublish.Checked = True Then
            Dim omessage As New bc_cs_message("Blue Curve", "Retrieving published documents may take a little time!", bc_cs_message.MESSAGE)
        End If
        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.LOCAL Then
            load_docs(True, True, True)
        Else
            If PictureBox5.BorderStyle = BorderStyle.Fixed3D Then
                load_docs(False, False, True)
            Else
                load_docs(False, True, True)
            End If
        End If
    End Sub

    Private Sub ListView1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListView1.DoubleClick
        open_doc()
    End Sub
    Private Function read_user_settings(ByRef osettings As bc_om_create_settings) As Boolean
        Dim fn As String = ""
        Try
            Dim fs As New bc_cs_file_transfer_services
            read_user_settings = False
            Dim bcs As New bc_cs_central_settings
            Dim dr As String
            dr = bc_cs_central_settings.get_user_dir
            If dr.Substring(dr.Length - 1, 1) = "\" Then
                fn = bc_cs_central_settings.get_user_dir + "bc_am_create_settings_" + CStr(bc_cs_central_settings.logged_on_user_id) + ".dat"
            Else
                fn = bc_cs_central_settings.get_user_dir + "\bc_am_create_settings_" + CStr(bc_cs_central_settings.logged_on_user_id) + ".dat"
            End If
            If fs.check_document_exists(fn) Then
                osettings = osettings.read_data_from_file(fn)
                read_user_settings = True
                Me.ListView1.Columns(0).Width = osettings.col_1_width.Width
                Me.ListView1.Columns(1).Width = osettings.col_2_width.Width
                Me.ListView1.Columns(2).Width = osettings.col_3_width.Width
                Me.ListView1.Columns(3).Width = osettings.col_4_width.Width
                Me.ListView1.Columns(4).Width = osettings.col_5_width.Width
                Me.ListView1.Columns(5).Width = osettings.col_6_width.Width
                Me.ListView1.Columns(6).Width = osettings.col_7_width.Width
                Me.ListView1.Columns(7).Width = osettings.col_8_width.Width

            End If
        Catch
            Dim ocommentary As New bc_cs_activity_log("bc_am_wizard_main", "read_user_settings", bc_cs_activity_codes.COMMENTARY, "Failed to read user settings: " + fn)

        End Try
    End Function
    Private Function save_settings(ByRef osettings As bc_om_create_settings) As Boolean
        Dim fn As String = ""
        Try
            osettings.alerter_on = Me.Chkalert.Checked
            osettings.col_1_width.Width = Me.ListView1.Columns(0).Width
            osettings.col_2_width.Width = Me.ListView1.Columns(1).Width
            osettings.col_3_width.Width = Me.ListView1.Columns(2).Width
            osettings.col_4_width.Width = Me.ListView1.Columns(3).Width
            osettings.col_5_width.Width = Me.ListView1.Columns(4).Width
            osettings.col_6_width.Width = Me.ListView1.Columns(5).Width
            osettings.col_7_width.Width = Me.ListView1.Columns(6).Width
            osettings.col_8_width.Width = Me.ListView1.Columns(7).Width

            Dim fs As New bc_cs_file_transfer_services
            Dim bcs As New bc_cs_central_settings
            Dim dr As String
            dr = bc_cs_central_settings.get_user_dir
            If dr.Substring(dr.Length - 1, 1) = "\" Then
                fn = bc_cs_central_settings.get_user_dir + "bc_am_create_settings_" + CStr(bc_cs_central_settings.logged_on_user_id) + ".dat"
            Else
                fn = bc_cs_central_settings.get_user_dir + "\bc_am_create_settings_" + CStr(bc_cs_central_settings.logged_on_user_id) + ".dat"
            End If
            osettings.write_data_to_file(fn)
        Catch
            Dim ocommentary As New bc_cs_activity_log("bc_am_wizard_main", "read_user_settings", bc_cs_activity_codes.COMMENTARY, "Failed to write user settings: " + fn)
        End Try

    End Function

    Private Sub bc_am_at_wizard_main_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        If bc_am_at_wizard_generic.mode = 11 Then
            Me.gsettings.size = Me.Size

            save_settings(Me.gsettings)
        End If
        Try
            Me.tthread.Abort()
        Catch

        End Try

        bc_am_at_wizard_generic.frm_loaded = False
    End Sub
    Private Sub restore_factory_settings()
        REM remove config file
        Dim bcs As New bc_cs_central_settings
        Dim fs As New bc_cs_file_transfer_services
        Dim fn As String
        fn = bc_cs_central_settings.get_user_dir + "bc_am_create_settings_" + CStr(bc_cs_central_settings.logged_on_user_id) + ".dat"
        If fs.check_document_exists(fn) Then
            fs.delete_file(fn)
        End If
        load_open_controls()
    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chksettings.CheckedChanged
        If Me.chksettings.Checked = True Then
            restore_factory_settings()
            Me.chksettings.Checked = False
        End If

    End Sub
    Private Sub CheckBox1_CheckedChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chkalert.CheckedChanged
        If Me.Chkalert.Checked = True Then
            'Dim turn_on As Boolean = True
            'Dim fs As New bc_cs_file_transfer_services
            'If fs.check_document_exists(bc_cs_central_settings.get_user_dir + "\" + CStr(bc_cs_central_settings.logged_on_user_id) + "_bc_am_workflow_settings.dat") = True Then
            'Dim ows As New bc_om_workflow_settings
            'ows = ows.read_xml_from_file(bc_cs_central_settings.get_user_dir + "\" + CStr(bc_cs_central_settings.logged_on_user_id) + "_bc_am_workflow_settings.dat")
            'If ows.turned_on = True And ows.polling_enabled = True And ows.alerter_enabled = True Then
            'Dim omessage As New bc_cs_message("Blue Curve Create", "Process Alerter Already Enabled", bc_cs_message.MESSAGE)
            'turn_on = False
            'Me.Chkalert.Checked = False
            'End If
            'If turn_on = True Then
            tthread = New Thread(AddressOf new_document)
            tthread.Start()
            'End If
        Else
            Try
                tthread.Abort()
            Catch ex As Exception

            End Try
        End If
    End Sub
    Private Sub new_document()
        Try
            Dim ondoc As New bc_om_documents
            Dim pdoc As bc_am_at_open_doc
            Dim i, j, k As Integer
            Dim found As Boolean
            Dim alert_flag As Boolean
            Dim atx As String
            Dim doc_id As Long
            Dim first_poll As Boolean
            Dim new_checked_out_docs As New ArrayList
            Dim blog As String
            first_poll = True
            While True
                atx = ""
                doc_id = 0
                If suspend_poll = False Then
                    Dim ocomm As New bc_cs_activity_log("Create", "Client Thread Poll", bc_cs_activity_codes.COMMENTARY, Now)
                    REM check all documents
                    blog = bc_cs_central_settings.activity_file
                    bc_cs_central_settings.activity_file = "off"
                    pdoc = New bc_am_at_open_doc(False, True, Me.DateTimePicker1.Value, Me.DateTimePicker2.Value, Me.cpublish.Checked, False)
                    bc_cs_central_settings.activity_file = blog
                    found = False
                    alert_flag = False
                    If ondoc.document.Count < pdoc.onetwork_docs.document.Count And first_poll = False Then
                        If ondoc.document.Count - pdoc.onetwork_docs.document.Count = -1 Then
                            For i = 0 To pdoc.onetwork_docs.document.Count - 1
                                found = False
                                For j = 0 To ondoc.document.Count - 1
                                    If ondoc.document(j).id = pdoc.onetwork_docs.document(i).id Then
                                        found = True
                                        Exit For
                                    End If
                                Next
                                If found = False Then
                                    If pdoc.onetwork_docs.document(i).checked_out_user = 0 Then
                                        alert_flag = True
                                        atx = "New Document have arrived in Create: " + pdoc.onetwork_docs.document(i).title + " for my teams in stage: " + pdoc.onetwork_docs.document(i).stage_name + ". "
                                        doc_id = pdoc.onetwork_docs.document(i).id
                                        Exit For
                                    Else
                                        REM do not alert yet has checed out
                                        alert_flag = False
                                        new_checked_out_docs.Add(pdoc.onetwork_docs.document(i).id)
                                    End If
                                    Exit For
                                End If
                            Next
                        Else
                            If pdoc.onetwork_docs.document.Count > ondoc.document.Count Then
                                alert_flag = True
                                atx = "New Documents have arrived in Create for my teams. "
                            End If
                        End If
                    End If
                    Dim patx As String
                    patx = atx
                    REM checked if new checked out document is now checked in
                    k = new_checked_out_docs.Count
                    i = 0

                    While i < k
                        For j = 0 To pdoc.onetwork_docs.document.Count - 1
                            If new_checked_out_docs(i) = pdoc.onetwork_docs.document(j).id And pdoc.onetwork_docs.document(j).checked_out_user = "0" Then
                                If alert_flag = False Then
                                    atx = "New Document has arrived in Create: " + pdoc.onetwork_docs.document(j).title + " for my teams in stage: " + pdoc.onetwork_docs.document(i).stage_name + ". "
                                    doc_id = pdoc.onetwork_docs.document(i).id
                                Else
                                    atx = "New Documents have arrived in Create."
                                End If
                                new_checked_out_docs.RemoveAt(i)
                                i = i - 1
                                k = k - 1
                                alert_flag = True
                                Exit For
                            End If
                        Next
                        Dim ocommentarys As New bc_cs_activity_log("bc_am_wizard_main", "new_document", bc_cs_activity_codes.COMMENTARY, "XXX: " + CStr(i) + ":" + CStr(k))
                        i = i + 1
                    End While
                    REM check for stage change
                    Dim more_than_one As Boolean = False
                    For i = 0 To ondoc.document.Count - 1
                        For j = 0 To pdoc.onetwork_docs.document.Count - 1
                            If pdoc.onetwork_docs.document(j).id = ondoc.document(i).id Then
                                If ondoc.document(i).stage_name <> pdoc.onetwork_docs.document(j).stage_name Then
                                    alert_flag = True
                                    If more_than_one = True Then
                                        atx = patx + "Documents have changed stage."
                                    Else
                                        atx = patx + "Document has changed stage: " + ondoc.document(i).title + " from " + ondoc.document(i).stage_name + " to  " + pdoc.onetwork_docs.document(j).stage_name
                                        doc_id = ondoc.document(i).id
                                        more_than_one = True
                                    End If
                                End If
                                Exit For
                            End If
                        Next
                    Next
                    If alert_flag = True Then
                        'mode = 10
                        'load_open_controls()
                        'Me.changed = False

                        'Me.PictureBox4.Enabled = False
                        'Me.PictureBox5.Visible = False
                        'Me.PictureBox6.Visible = False
                        'Me.PictureBox6.Enabled = True
                        'Me.PictureBox4.BorderStyle = BorderStyle.Fixed3D
                        'Me.PictureBox5.BorderStyle = BorderStyle.None
                        'Me.PictureBox6.BorderStyle = BorderStyle.None
                        'load_docs(False, True, True, False)
                        'Me.ListView1.Width = 440
                        'mode = 11

                        'If doc_id <> 0 Then
                        'For i = 0 To Me.ListView1.Items.Count - 1
                        'If Me.ListView1.Items(i).SubItems(6).Text = CStr(doc_id) Then
                        'Me.ListView1.Items(i).Selected = True
                        'Me.ListView1.Items(i).EnsureVisible()
                        'Exit For
                        'End If
                        'Next
                        'End If
                        Me.from_alert = True
                        alert(atx, doc_id)
                    End If

                    REM check my documents
                    ondoc.document.Clear()

                    For i = 0 To pdoc.onetwork_docs.document.Count - 1
                        ondoc.document.Add(pdoc.onetwork_docs.document(i))
                    Next

                End If
                first_poll = False
                Thread.Sleep(bc_cs_central_settings.poll_interval)
                Dim ocommentary As New bc_cs_activity_log("bc_am_wizard_main", "new_document", bc_cs_activity_codes.COMMENTARY, "POLL interval: " + CStr(bc_cs_central_settings.poll_interval))

            End While
        Catch ex As Exception
            'Dim oerr As New bc_cs_error_log("bc_am_at_wizard_main", "Alert", ex.Message, bc_cs_error_codes.USER_DEFINED)
        End Try
    End Sub
    Public oalertfrm As bc_am_workflow_alert
    Public msg As String
    Private Sub alert(ByVal tx As String, ByVal doc_id As Long)
        Dim slog As New bc_cs_activity_log("bc_am_at_wizard_main", "alert", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            msg = tx
            Me.alert_doc_id = doc_id
            Dim talertform As New Thread(AddressOf alert_form)
            Me.from_alert = False
            talertform.Start()
            Thread.Sleep(bc_cs_central_settings.alert_interval)
            Try
                oalertfrm.Close()
            Catch

            End Try


        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_workflow", "alert", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_workflow", "alert", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Private Sub alert_form()
        oalertfrm = New bc_am_workflow_alert
        oalertfrm.from_create = True
        bc_am_workflow_alert.openfrm = Me
        oalertfrm.doc_id = Me.alert_doc_id
        oalertfrm.lalert.Text = msg
        Dim oapi As New API
        API.SetWindowPos(oalertfrm.Handle.ToInt32, API.HWND_TOPMOST, Nothing, 1, 1, 1, 1)
        oalertfrm.ShowDialog()
    End Sub


    Private Sub bc_am_at_wizard_main_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseMove
        If Me.from_alert = True Then
            Dim oapi As New API
            Me.StartPosition = FormStartPosition.CenterScreen
            Me.lxpos = Me.Location.X
            Me.lypos = Me.Location.Y
            API.SetWindowPos(Me.Handle.ToInt32, API.HWND_NOTOPMOST, Me.lxpos, Me.lypos, 1, 1, 1)
            Me.PictureBox4.Enabled = True
            Me.PictureBox5.Enabled = True
            Me.PictureBox6.Enabled = True
            Me.PictureBox4.BorderStyle = BorderStyle.Fixed3D
            Me.PictureBox5.BorderStyle = BorderStyle.None
            Me.PictureBox6.BorderStyle = BorderStyle.None
            Me.PictureBox4.Visible = True
            Me.PictureBox5.Visible = True
            Me.PictureBox6.Visible = True
            Me.from_alert = False
            Exit Sub
        End If
    End Sub

    Private Sub bskip_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bskip.Click
        Dim oproductselection As New bc_am_product_selection
        Me.bskip.Visible = False
        Me.attach_selected = True
        Me.Hide()
    End Sub


    Private Sub ListBox1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListBox1.DoubleClick

        Dim mea As New MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0)

        If btnremove.Visible = True Then
            btnremove_Click(sender, e)
        ElseIf bok.Visible = True Then
            bok_MouseDown(sender, mea)
            bok_MouseUp(sender, mea)
        Else
            btnforwardgrey_MouseDown(sender, mea)
            btnforwardgrey_Mouseup(sender, mea)
        End If
        'Select Case True
        '    Case bok.Visible
        '        bok_MouseDown(sender, mea)
        '        bok_MouseUp(sender, mea)
        '    Case btnremove.Visible
        '        btnremove_Click(sender, e)
        '    Case Else
        '        btnforwardgrey_MouseDown(sender, mea)
        '        btnforwardgrey_Mouseup(sender, mea)
        'End Select

    End Sub

    Private Sub txtsummary_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtsummary.TextChanged

    End Sub

    Private Sub bok_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles bok.MouseClick

    End Sub

    Private Sub BlueCurve_TextSearch1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BlueCurve_TextSearch1.Load

    End Sub

    Private Sub PutFormOnTop()

        Try

            'REM Make this form the top window
            'REM Steve Wooderson 28/09/2011

            Me.TopMost = True
            Me.TopMost = False

        Catch ex As Exception

            Dim db_err As New bc_cs_error_log("bc_am_at_build_wizard", "PutFormOnTop", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

        End Try

    End Sub

    Private Sub Chkclone_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chkclone.CheckedChanged
        If Me.Chkclone.Checked = False And last_screen = True Then
            Me.bok.Visible = True
            Me.btnforwardgrey.Visible = False
        Else
            Me.bok.Visible = False
            Me.btnforwardgrey.Visible = True
        End If
    End Sub

    Private Sub Btncancel_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Btncancel.MouseMove

    End Sub

    Private Sub bokgrey_ChangeUICues(ByVal sender As Object, ByVal e As System.Windows.Forms.UICuesEventArgs) Handles bokgrey.ChangeUICues

    End Sub
End Class
' This code is written by Vishal Kulkarni
' For any querries feel free to contact Vishal at
' vishal_kulkarni@hotmail.com

Public Class API
    Public Const SWP_NOMOVE As Integer = 2
    Public Const SWP_NOSIZE As Integer = 1
    Public Const HWND_TOPMOST As Integer = -1
    Public Const HWND_NOTOPMOST As Integer = -2

    <DllImport("user32")> Public Shared Function _
        SetWindowPos(ByVal hwnd As Integer, _
        ByVal hWndInsertAfter As Integer, _
        ByVal x As Integer, _
        ByVal y As Integer, _
        ByVal cy As Long, _
        ByVal cx As Integer, _
       ByVal wFlags As Integer) As Integer
    End Function
    'setWindowPos :
    'This function changes the size, position, and z-order of a child, pop-up, or top-level window. Child, pop-up, and top-level windows are ordered according to their appearance on the screen. The topmost window receives the highest rank and is the first window in the z-order.

    ' PARAMETERS    
    'hWnd 
    'Handle to the window. 
    'hWndInsertAfter 
    'Handle to the window to precede the positioned window in the z-order. This parameter must be a window handle or one of the following values: 
    'HWND_BOTTOM 
    'Places the window at the bottom of the z-order. If the hWnd parameter identifies a topmost window, the window loses its topmost status and is placed at the bottom of all other windows. 
    'HWND_NOTOPMOST 
    'Places the window above all non-topmost windows (that is, behind all topmost windows). This flag has no effect if the window is already a non-topmost window. 
    'HWND_TOP 
    'Places the window at the top of the z-order. 
    'HWND_TOPMOST 
    'Places the window above all non-topmost windows. The window maintains its topmost position even when it is deactivated. 
    'For more information about how this parameter is used, see the following Remarks section. 

    'X 
    'Specifies the new position of the left side of the window, in client coordinates. 
    'Y 
    'Specifies the new position of the top of the window, in client coordinates. 
    'cx 
    'Specifies the new width of the window, in pixels. 
    'cy 
    'Specifies the new height of the window, in pixels. 
    'uFlags 
    'Specifies the window sizing and positioning flags. This parameter can be a combination of the following values: 
    'SWP_ASYNCWINDOWPOS 
    'If the calling thread and the thread that owns the window are attached to different input queues, the system posts the request to the thread that owns the window. This prevents the calling thread from blocking its execution while other threads process the request. 
    'SWP_DEFERERASE 
    'Prevents generation of the WM_SYNCPAINT message. 
    'SWP_DRAWFRAME 
    'Draws a frame (defined in the window’s class description) around the window. 
    'SWP_FRAMECHANGED 
    'Sends a WM_NCCALCSIZE message to the window, even if the window’s size is not being changed. If this flag is not specified, WM_NCCALCSIZE is sent only when the window’s size is being changed. 
    'SWP_HIDEWINDOW 
    'Hides the window. 
    'SWP_NOACTIVATE 
    'Does not activate the window. If this flag is not set, the window is activated and moved to the top of either the topmost or non-topmost group (depending on the setting of the hWndInsertAfter parameter). 
    'SWP_NOCOPYBITS 
    'Discards the entire contents of the client area. If this flag is not specified, the valid contents of the client area are saved and copied back into the client area after the window is sized or repositioned. 
    'SWP_NOMOVE 
    'Retains the current position (ignores the X and Y parameters). 
    'SWP_NOOWNERZORDER 
    'Does not change the owner window’s position in the z-order. 
    'SWP_NOREDRAW 
    'Does not redraw changes. If this flag is set, no repainting of any kind occurs. This applies to the client area, the nonclient area (including the title bar and scroll bars), and any part of the parent window uncovered as a result of the window being moved. When this flag is set, the application must explicitly invalidate or redraw any parts of the window and parent window that need redrawing. 
    'SWP_NOREPOSITION 
    'Same as the SWP_NOOWNERZORDER flag. 
    'SWP_NOSIZE 
    'Retains the current size (ignores the cx and cy parameters). 
    'SWP_NOZORDER 
    'Retains the current z-order (ignores the hWndInsertAfter parameter). 
    'SWP_SHOWWINDOW 
    'Displays the window. 

End Class