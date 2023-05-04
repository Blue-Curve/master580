Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports BlueCurve.Core.AS
Imports System.Windows.Forms
REM FIL JULY 2012
Friend Class bc_am_in_main_frm
    Inherits Create.AM.bc_am_at_wizard_generic
    Public ok_selected As Boolean = False
    Public class_id As Long
    Public class_name As String
    Public entity_id As Long
    Public entity_name As String
    Public contributor_id As Long
    Public contributor_name As String
    Public error_mode As Boolean = False
    Public submit_mode As Boolean = False
    Public close_mode As Integer
    Public Const LOCAL = 1
    Public Const CHECKED_OUT = 2
    Public Const CHECKED_IN = 3
    Dim bc_om_load_objects As bc_am_load_objects
    Friend WithEvents BlueCurve_TextSearch1 As BlueCurve.Core.AS.BlueCurve_TextSearch
    Friend WithEvents slitanal As System.Windows.Forms.ComboBox
    Public form_name As String


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
    Friend WithEvents Btnnext As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents description As System.Windows.Forms.Label
    'Friend WithEvents Label3 As System.Windows.Forms.Label
    'Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents RadioButton1 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton2 As System.Windows.Forms.RadioButton
    Friend WithEvents lstentities As System.Windows.Forms.ListBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents ListErrors As System.Windows.Forms.ListView
    Friend WithEvents err_num As System.Windows.Forms.ColumnHeader
    Friend WithEvents desc As System.Windows.Forms.ColumnHeader
    Friend WithEvents row As System.Windows.Forms.ColumnHeader
    Friend WithEvents col As System.Windows.Forms.ColumnHeader
    Friend WithEvents sheet As System.Windows.Forms.ColumnHeader
    Friend WithEvents txtcomment As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Comment As System.Windows.Forms.ColumnHeader
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents lworkbookname As System.Windows.Forms.Label
    Friend WithEvents sanalyst As System.Windows.Forms.Label
    Friend WithEvents sstage As System.Windows.Forms.Label
    Friend WithEvents stxtname As System.Windows.Forms.TextBox
    Friend WithEvents stxtanal As System.Windows.Forms.TextBox
    Friend WithEvents chkvalidate As System.Windows.Forms.CheckBox
    Friend WithEvents Chkclose As System.Windows.Forms.CheckBox
    Friend WithEvents scbostage As System.Windows.Forms.ComboBox
    Friend WithEvents CheckBox2 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox3 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox4 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox5 As System.Windows.Forms.CheckBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents lstcontributor As System.Windows.Forms.ComboBox
    Friend WithEvents lstclass As System.Windows.Forms.ComboBox
    Friend WithEvents CheckBox6 As System.Windows.Forms.CheckBox
    Friend WithEvents MainMenu1 As System.Windows.Forms.MainMenu
    Friend WithEvents PictureBox4 As System.Windows.Forms.PictureBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_in_main_frm))
        Me.lstentities = New System.Windows.Forms.ListBox
        Me.Btnnext = New System.Windows.Forms.Button
        Me.Button1 = New System.Windows.Forms.Button
        Me.description = New System.Windows.Forms.Label
        Me.RadioButton1 = New System.Windows.Forms.RadioButton
        Me.RadioButton2 = New System.Windows.Forms.RadioButton
        Me.Label8 = New System.Windows.Forms.Label
        Me.ListErrors = New System.Windows.Forms.ListView
        Me.desc = New System.Windows.Forms.ColumnHeader
        Me.Comment = New System.Windows.Forms.ColumnHeader
        Me.sheet = New System.Windows.Forms.ColumnHeader
        Me.row = New System.Windows.Forms.ColumnHeader
        Me.col = New System.Windows.Forms.ColumnHeader
        Me.err_num = New System.Windows.Forms.ColumnHeader
        Me.txtcomment = New System.Windows.Forms.TextBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.CheckBox1 = New System.Windows.Forms.CheckBox
        Me.lworkbookname = New System.Windows.Forms.Label
        Me.sanalyst = New System.Windows.Forms.Label
        Me.sstage = New System.Windows.Forms.Label
        Me.stxtname = New System.Windows.Forms.TextBox
        Me.stxtanal = New System.Windows.Forms.TextBox
        Me.chkvalidate = New System.Windows.Forms.CheckBox
        Me.Chkclose = New System.Windows.Forms.CheckBox
        Me.scbostage = New System.Windows.Forms.ComboBox
        Me.CheckBox2 = New System.Windows.Forms.CheckBox
        Me.CheckBox3 = New System.Windows.Forms.CheckBox
        Me.CheckBox4 = New System.Windows.Forms.CheckBox
        Me.CheckBox5 = New System.Windows.Forms.CheckBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.lstcontributor = New System.Windows.Forms.ComboBox
        Me.lstclass = New System.Windows.Forms.ComboBox
        Me.CheckBox6 = New System.Windows.Forms.CheckBox
        Me.MainMenu1 = New System.Windows.Forms.MainMenu(Me.components)
        Me.PictureBox4 = New System.Windows.Forms.PictureBox
        Me.BlueCurve_TextSearch1 = New BlueCurve.Core.[AS].BlueCurve_TextSearch
        Me.slitanal = New System.Windows.Forms.ComboBox
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Pubthumbnail, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LabelConStr
        '
        Me.LabelConStr.Text = ""
        '
        'Label1
        '
        Me.Label1.Text = ""
        '
        'Label4
        '
        Me.Label4.Text = "Logged in as:"
        '
        'lstentities
        '
        Me.lstentities.Font = New System.Drawing.Font("Arial Unicode MS", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstentities.ItemHeight = 16
        Me.lstentities.Location = New System.Drawing.Point(250, 95)
        Me.lstentities.Name = "lstentities"
        Me.lstentities.Size = New System.Drawing.Size(265, 132)
        Me.lstentities.TabIndex = 8
        '
        'Btnnext
        '
        Me.Btnnext.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Btnnext.Location = New System.Drawing.Point(468, 295)
        Me.Btnnext.Name = "Btnnext"
        Me.Btnnext.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.Btnnext.Size = New System.Drawing.Size(76, 32)
        Me.Btnnext.TabIndex = 15
        Me.Btnnext.Text = "&Ok"
        '
        'Button1
        '
        Me.Button1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button1.Location = New System.Drawing.Point(468, 332)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(76, 32)
        Me.Button1.TabIndex = 16
        Me.Button1.Text = "&Cancel"
        '
        'description
        '
        Me.description.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.description.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.description.Location = New System.Drawing.Point(80, 112)
        Me.description.Name = "description"
        Me.description.Size = New System.Drawing.Size(40, 16)
        Me.description.TabIndex = 21
        Me.description.Text = "Class"
        '
        'RadioButton1
        '
        Me.RadioButton1.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.RadioButton1.Checked = True
        Me.RadioButton1.Font = New System.Drawing.Font("Arial Unicode MS", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioButton1.ForeColor = System.Drawing.Color.Black
        Me.RadioButton1.Location = New System.Drawing.Point(80, 240)
        Me.RadioButton1.Name = "RadioButton1"
        Me.RadioButton1.Size = New System.Drawing.Size(88, 16)
        Me.RadioButton1.TabIndex = 24
        Me.RadioButton1.TabStop = True
        Me.RadioButton1.Text = "All Entities"
        Me.RadioButton1.UseVisualStyleBackColor = False
        '
        'RadioButton2
        '
        Me.RadioButton2.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.RadioButton2.Font = New System.Drawing.Font("Arial Unicode MS", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioButton2.Location = New System.Drawing.Point(176, 240)
        Me.RadioButton2.Name = "RadioButton2"
        Me.RadioButton2.Size = New System.Drawing.Size(88, 16)
        Me.RadioButton2.TabIndex = 25
        Me.RadioButton2.Text = "My Entities"
        Me.RadioButton2.UseVisualStyleBackColor = False
        '
        'Label8
        '
        Me.Label8.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.Label8.Font = New System.Drawing.Font("Arial Unicode MS", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(80, 112)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(96, 16)
        Me.Label8.TabIndex = 26
        Me.Label8.Text = "Schema"
        '
        'ListErrors
        '
        Me.ListErrors.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.desc, Me.Comment, Me.sheet, Me.row, Me.col, Me.err_num})
        Me.ListErrors.FullRowSelect = True
        Me.ListErrors.HideSelection = False
        Me.ListErrors.Location = New System.Drawing.Point(32, 72)
        Me.ListErrors.MultiSelect = False
        Me.ListErrors.Name = "ListErrors"
        Me.ListErrors.Size = New System.Drawing.Size(488, 216)
        Me.ListErrors.TabIndex = 27
        Me.ListErrors.UseCompatibleStateImageBehavior = False
        Me.ListErrors.View = System.Windows.Forms.View.Details
        '
        'desc
        '
        Me.desc.Text = "Description"
        Me.desc.Width = 246
        '
        'Comment
        '
        Me.Comment.Text = "Comment"
        '
        'sheet
        '
        Me.sheet.Text = "Sheet"
        Me.sheet.Width = 108
        '
        'row
        '
        Me.row.Text = "Row"
        Me.row.Width = 48
        '
        'col
        '
        Me.col.Text = "Col"
        Me.col.Width = 46
        '
        'err_num
        '
        Me.err_num.Text = "Error Number"
        Me.err_num.Width = 74
        '
        'txtcomment
        '
        Me.txtcomment.Location = New System.Drawing.Point(288, 304)
        Me.txtcomment.Multiline = True
        Me.txtcomment.Name = "txtcomment"
        Me.txtcomment.Size = New System.Drawing.Size(176, 48)
        Me.txtcomment.TabIndex = 29
        Me.txtcomment.Visible = False
        '
        'Label9
        '
        Me.Label9.Font = New System.Drawing.Font("Arial Unicode MS", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(200, 304)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(72, 16)
        Me.Label9.TabIndex = 30
        Me.Label9.Text = "Comment:"
        Me.Label9.Visible = False
        '
        'CheckBox1
        '
        Me.CheckBox1.Enabled = False
        Me.CheckBox1.Font = New System.Drawing.Font("Arial Unicode MS", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox1.Location = New System.Drawing.Point(200, 320)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(88, 16)
        Me.CheckBox1.TabIndex = 31
        Me.CheckBox1.Text = "Edit Existing"
        Me.CheckBox1.Visible = False
        '
        'lworkbookname
        '
        Me.lworkbookname.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.lworkbookname.Font = New System.Drawing.Font("Arial Unicode MS", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lworkbookname.Location = New System.Drawing.Point(56, 112)
        Me.lworkbookname.Name = "lworkbookname"
        Me.lworkbookname.Size = New System.Drawing.Size(112, 16)
        Me.lworkbookname.TabIndex = 32
        Me.lworkbookname.Text = "Workbook Name"
        Me.lworkbookname.Visible = False
        '
        'sanalyst
        '
        Me.sanalyst.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.sanalyst.Font = New System.Drawing.Font("Arial Unicode MS", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.sanalyst.Location = New System.Drawing.Point(56, 144)
        Me.sanalyst.Name = "sanalyst"
        Me.sanalyst.Size = New System.Drawing.Size(112, 24)
        Me.sanalyst.TabIndex = 33
        Me.sanalyst.Text = "Analyst"
        Me.sanalyst.Visible = False
        '
        'sstage
        '
        Me.sstage.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.sstage.Font = New System.Drawing.Font("Arial Unicode MS", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.sstage.Location = New System.Drawing.Point(56, 176)
        Me.sstage.Name = "sstage"
        Me.sstage.Size = New System.Drawing.Size(112, 24)
        Me.sstage.TabIndex = 34
        Me.sstage.Text = "Stage"
        Me.sstage.Visible = False
        '
        'stxtname
        '
        Me.stxtname.AllowDrop = True
        Me.stxtname.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.stxtname.Location = New System.Drawing.Point(192, 112)
        Me.stxtname.Name = "stxtname"
        Me.stxtname.Size = New System.Drawing.Size(312, 21)
        Me.stxtname.TabIndex = 35
        Me.stxtname.Visible = False
        '
        'stxtanal
        '
        Me.stxtanal.AllowDrop = True
        Me.stxtanal.Enabled = False
        Me.stxtanal.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.stxtanal.Location = New System.Drawing.Point(192, 144)
        Me.stxtanal.Name = "stxtanal"
        Me.stxtanal.Size = New System.Drawing.Size(312, 21)
        Me.stxtanal.TabIndex = 36
        Me.stxtanal.Visible = False
        '
        'chkvalidate
        '
        Me.chkvalidate.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.chkvalidate.Font = New System.Drawing.Font("Arial Unicode MS", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkvalidate.Location = New System.Drawing.Point(64, 224)
        Me.chkvalidate.Name = "chkvalidate"
        Me.chkvalidate.Size = New System.Drawing.Size(120, 16)
        Me.chkvalidate.TabIndex = 38
        Me.chkvalidate.Text = "&Validate Only"
        Me.chkvalidate.UseVisualStyleBackColor = False
        Me.chkvalidate.Visible = False
        '
        'Chkclose
        '
        Me.Chkclose.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.Chkclose.Font = New System.Drawing.Font("Arial Unicode MS", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Chkclose.Location = New System.Drawing.Point(192, 224)
        Me.Chkclose.Name = "Chkclose"
        Me.Chkclose.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Chkclose.Size = New System.Drawing.Size(120, 16)
        Me.Chkclose.TabIndex = 39
        Me.Chkclose.TabStop = False
        Me.Chkclose.Text = "&Close"
        Me.Chkclose.UseVisualStyleBackColor = False
        Me.Chkclose.Visible = False
        '
        'scbostage
        '
        Me.scbostage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.scbostage.Location = New System.Drawing.Point(192, 176)
        Me.scbostage.Name = "scbostage"
        Me.scbostage.Size = New System.Drawing.Size(136, 23)
        Me.scbostage.TabIndex = 40
        Me.scbostage.Visible = False
        '
        'CheckBox2
        '
        Me.CheckBox2.BackColor = System.Drawing.Color.Transparent
        Me.CheckBox2.Enabled = False
        Me.CheckBox2.Font = New System.Drawing.Font("Arial Unicode MS", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox2.Location = New System.Drawing.Point(448, 280)
        Me.CheckBox2.Name = "CheckBox2"
        Me.CheckBox2.Size = New System.Drawing.Size(80, 16)
        Me.CheckBox2.TabIndex = 42
        Me.CheckBox2.Text = "Follow Link"
        Me.CheckBox2.UseVisualStyleBackColor = False
        Me.CheckBox2.Visible = False
        '
        'CheckBox3
        '
        Me.CheckBox3.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.CheckBox3.Font = New System.Drawing.Font("Arial Unicode MS", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox3.Location = New System.Drawing.Point(256, 224)
        Me.CheckBox3.Name = "CheckBox3"
        Me.CheckBox3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox3.Size = New System.Drawing.Size(176, 16)
        Me.CheckBox3.TabIndex = 43
        Me.CheckBox3.TabStop = False
        Me.CheckBox3.Text = "&Leave Checked Out"
        Me.CheckBox3.UseVisualStyleBackColor = False
        Me.CheckBox3.Visible = False
        '
        'CheckBox4
        '
        Me.CheckBox4.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.CheckBox4.Font = New System.Drawing.Font("Arial Unicode MS", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox4.Location = New System.Drawing.Point(256, 248)
        Me.CheckBox4.Name = "CheckBox4"
        Me.CheckBox4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox4.Size = New System.Drawing.Size(120, 16)
        Me.CheckBox4.TabIndex = 44
        Me.CheckBox4.TabStop = False
        Me.CheckBox4.Text = "&Check In"
        Me.CheckBox4.UseVisualStyleBackColor = False
        Me.CheckBox4.Visible = False
        '
        'CheckBox5
        '
        Me.CheckBox5.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.CheckBox5.Font = New System.Drawing.Font("Arial Unicode MS", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox5.Location = New System.Drawing.Point(256, 272)
        Me.CheckBox5.Name = "CheckBox5"
        Me.CheckBox5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CheckBox5.Size = New System.Drawing.Size(56, 16)
        Me.CheckBox5.TabIndex = 45
        Me.CheckBox5.TabStop = False
        Me.CheckBox5.Text = "&Local"
        Me.CheckBox5.UseVisualStyleBackColor = False
        Me.CheckBox5.Visible = False
        '
        'Label10
        '
        Me.Label10.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.Label10.Font = New System.Drawing.Font("Arial Unicode MS", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(56, 176)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(112, 16)
        Me.Label10.TabIndex = 46
        Me.Label10.Text = "Class"
        Me.Label10.Visible = False
        '
        'lstcontributor
        '
        Me.lstcontributor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.lstcontributor.Location = New System.Drawing.Point(80, 144)
        Me.lstcontributor.Name = "lstcontributor"
        Me.lstcontributor.Size = New System.Drawing.Size(128, 23)
        Me.lstcontributor.TabIndex = 47
        '
        'lstclass
        '
        Me.lstclass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.lstclass.Location = New System.Drawing.Point(80, 200)
        Me.lstclass.Name = "lstclass"
        Me.lstclass.Size = New System.Drawing.Size(128, 23)
        Me.lstclass.TabIndex = 48
        '
        'CheckBox6
        '
        Me.CheckBox6.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.CheckBox6.Location = New System.Drawing.Point(384, 221)
        Me.CheckBox6.Name = "CheckBox6"
        Me.CheckBox6.Size = New System.Drawing.Size(136, 24)
        Me.CheckBox6.TabIndex = 49
        Me.CheckBox6.Text = "&Override Check Out"
        Me.CheckBox6.UseVisualStyleBackColor = False
        Me.CheckBox6.Visible = False
        '
        'PictureBox4
        '
        Me.PictureBox4.Image = CType(resources.GetObject("PictureBox4.Image"), System.Drawing.Image)
        Me.PictureBox4.Location = New System.Drawing.Point(480, 0)
        Me.PictureBox4.Name = "PictureBox4"
        Me.PictureBox4.Size = New System.Drawing.Size(64, 64)
        Me.PictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox4.TabIndex = 50
        Me.PictureBox4.TabStop = False
        '
        'BlueCurve_TextSearch1
        '
        Me.BlueCurve_TextSearch1.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.BlueCurve_TextSearch1.Location = New System.Drawing.Point(256, 248)
        Me.BlueCurve_TextSearch1.MaximumSize = New System.Drawing.Size(30000, 30000)
        Me.BlueCurve_TextSearch1.MinimumSize = New System.Drawing.Size(150, 21)
        Me.BlueCurve_TextSearch1.Name = "BlueCurve_TextSearch1"
        Me.BlueCurve_TextSearch1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.BlueCurve_TextSearch1.SearchAttributeList = ""
        Me.BlueCurve_TextSearch1.SearchAttributes = 0
        Me.BlueCurve_TextSearch1.SearchBuildEntitiesOnly = False
        Me.BlueCurve_TextSearch1.SearchClass = 0
        Me.BlueCurve_TextSearch1.SearchControl = "lstentities"
        Me.BlueCurve_TextSearch1.SearchCurrentAttribute = Nothing
        Me.BlueCurve_TextSearch1.SearchFireEventOnly = False
        Me.BlueCurve_TextSearch1.SearchGetInactive = False
        Me.BlueCurve_TextSearch1.SearchText = ""
        Me.BlueCurve_TextSearch1.SearchTimerOff = False
        Me.BlueCurve_TextSearch1.SearchUserEntitiesOnly = False
        Me.BlueCurve_TextSearch1.Size = New System.Drawing.Size(256, 34)
        Me.BlueCurve_TextSearch1.TabIndex = 53
        Me.BlueCurve_TextSearch1.Visible = False
        '
        'slitanal
        '
        Me.slitanal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.slitanal.FormattingEnabled = True
        Me.slitanal.Location = New System.Drawing.Point(192, 145)
        Me.slitanal.Name = "slitanal"
        Me.slitanal.Size = New System.Drawing.Size(313, 23)
        Me.slitanal.TabIndex = 54
        Me.slitanal.Visible = False
        '
        'bc_am_in_main_frm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 15)
        Me.ClientSize = New System.Drawing.Size(546, 375)
        Me.Controls.Add(Me.slitanal)
        Me.Controls.Add(Me.BlueCurve_TextSearch1)
        Me.Controls.Add(Me.PictureBox4)
        Me.Controls.Add(Me.CheckBox6)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.CheckBox5)
        Me.Controls.Add(Me.CheckBox4)
        Me.Controls.Add(Me.CheckBox3)
        Me.Controls.Add(Me.CheckBox2)
        Me.Controls.Add(Me.scbostage)
        Me.Controls.Add(Me.Chkclose)
        Me.Controls.Add(Me.chkvalidate)
        Me.Controls.Add(Me.stxtanal)
        Me.Controls.Add(Me.stxtname)
        Me.Controls.Add(Me.sstage)
        Me.Controls.Add(Me.sanalyst)
        Me.Controls.Add(Me.lworkbookname)
        Me.Controls.Add(Me.CheckBox1)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.txtcomment)
        Me.Controls.Add(Me.ListErrors)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.RadioButton2)
        Me.Controls.Add(Me.RadioButton1)
        Me.Controls.Add(Me.description)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Btnnext)
        Me.Controls.Add(Me.lstentities)
        Me.Controls.Add(Me.lstcontributor)
        Me.Controls.Add(Me.lstclass)
        Me.Font = New System.Drawing.Font("Arial Unicode MS", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Menu = Me.MainMenu1
        Me.Name = "bc_am_in_main_frm"
        Me.Text = "insight"
        Me.Controls.SetChildIndex(Me.lstclass, 0)
        Me.Controls.SetChildIndex(Me.lstcontributor, 0)
        Me.Controls.SetChildIndex(Me.lstentities, 0)
        Me.Controls.SetChildIndex(Me.Btnnext, 0)
        Me.Controls.SetChildIndex(Me.Button1, 0)
        Me.Controls.SetChildIndex(Me.description, 0)
        Me.Controls.SetChildIndex(Me.RadioButton1, 0)
        Me.Controls.SetChildIndex(Me.RadioButton2, 0)
        Me.Controls.SetChildIndex(Me.Label8, 0)
        Me.Controls.SetChildIndex(Me.ListErrors, 0)
        Me.Controls.SetChildIndex(Me.txtcomment, 0)
        Me.Controls.SetChildIndex(Me.Label9, 0)
        Me.Controls.SetChildIndex(Me.CheckBox1, 0)
        Me.Controls.SetChildIndex(Me.lworkbookname, 0)
        Me.Controls.SetChildIndex(Me.sanalyst, 0)
        Me.Controls.SetChildIndex(Me.sstage, 0)
        Me.Controls.SetChildIndex(Me.stxtname, 0)
        Me.Controls.SetChildIndex(Me.stxtanal, 0)
        Me.Controls.SetChildIndex(Me.chkvalidate, 0)
        Me.Controls.SetChildIndex(Me.Chkclose, 0)
        Me.Controls.SetChildIndex(Me.scbostage, 0)
        Me.Controls.SetChildIndex(Me.CheckBox2, 0)
        Me.Controls.SetChildIndex(Me.CheckBox3, 0)
        Me.Controls.SetChildIndex(Me.CheckBox4, 0)
        Me.Controls.SetChildIndex(Me.CheckBox5, 0)
        Me.Controls.SetChildIndex(Me.Label10, 0)
        Me.Controls.SetChildIndex(Me.CheckBox6, 0)
        Me.Controls.SetChildIndex(Me.PictureBox4, 0)
        Me.Controls.SetChildIndex(Me.BlueCurve_TextSearch1, 0)
        Me.Controls.SetChildIndex(Me.slitanal, 0)
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Pubthumbnail, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region
    Private Sub bc_am_in_main_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim slog As New bc_cs_activity_log("bc_am_in_main_load", "new", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            Dim i As Integer

            Me.CheckBox5.Visible = False
            Me.Text = "Insight - Blue Curve"
            Me.ListErrors.Visible = False
            populate_list_boxes()
            If Me.lstcontributor.Visible = True Then
                Try
                    Me.lstcontributor.SelectedIndex = 0
                Catch
                    Dim omessage As New bc_cs_message("Blue Curve - insight", "User not synchronized or not enough access rights!", bc_cs_message.MESSAGE)
                End Try
            End If
            MyBase.Pubthumbnail.Visible = False
            MyBase.PictureBox3.Visible = False
            If bc_am_insight_formats.controlled_Submission = 1 Then
                Me.Chkclose.Enabled = True
                REM controlled submission
                If bc_cs_central_settings.selected_conn_method <> bc_cs_central_settings.LOCAL Then
                    Me.CheckBox3.Checked = True
                Else
                    Me.CheckBox4.Enabled = False
                    Me.CheckBox3.Enabled = False
                    Me.CheckBox5.Checked = True
                End If
            Else
                REM adhoc submission
                Me.Chkclose.Enabled = False
                Me.CheckBox4.Enabled = False
                Me.CheckBox3.Enabled = False
                Me.CheckBox5.Enabled = False
                Me.CheckBox6.Enabled = False
                REM controlled submission
                If bc_cs_central_settings.selected_conn_method <> bc_cs_central_settings.LOCAL Then
                    Me.CheckBox3.Checked = True
                End If


            End If
            Me.Label1.Text = bc_cs_central_settings.version + " " + CStr(bc_cs_central_settings.selected_conn_method)
            If bc_cs_central_settings.selected_conn_method = "ado" Then
                Me.Label1.Text = Me.Label1.Text + "- " + bc_cs_central_settings.servername
            End If
            If bc_cs_central_settings.selected_conn_method = "soap" Then
                Me.Label1.Text = Me.Label1.Text + "- " + bc_cs_central_settings.soap_server
            End If
            For i = 0 To bc_am_load_objects.obc_users.user.Count - 1
                With bc_am_load_objects.obc_users.user(i)
                    If UCase(Trim(.os_user_name)) = UCase(Trim(bc_cs_central_settings.logged_on_user_id)) Then
                        Me.Label2.Text = .first_name + " " + .surname
                    End If
                End With
            Next

            'Me.Btnnext.Enabled = True

            form_name = "insight.submit"

            For i = 0 To bc_am_load_objects.obc_prefs.language_strings.forms.Count - 1
                If bc_am_load_objects.obc_prefs.language_strings.forms(i).form_name = form_name Then
                    form_strings = bc_am_load_objects.obc_prefs.language_strings.forms(i)
                    Exit For
                End If
            Next


            For i = 0 To form_strings.form_labels.Count - 1
                If form_strings.form_labels(i).label_code = "insight.submit.name" Then
                    Me.lworkbookname.Text = form_strings.form_labels(i).text
                End If
                If form_strings.form_labels(i).label_code = "insight.submit.analyst" Then
                    Me.sanalyst.Text = form_strings.form_labels(i).text
                End If
                If form_strings.form_labels(i).label_code = "insight.submit.stage" Then
                    Me.sstage.Text() = form_strings.form_labels(i).text
                End If

                If form_strings.form_labels(i).label_code = "insight.submit.checkout" Then
                    Me.CheckBox3.Text() = form_strings.form_labels(i).text
                End If
                If form_strings.form_labels(i).label_code = "insight.submit.checkin" Then
                    Me.CheckBox4.Text() = form_strings.form_labels(i).text
                End If
                If form_strings.form_labels(i).label_code = "insight.submit.local" Then
                    Me.CheckBox5.Text() = form_strings.form_labels(i).text
                End If
                If form_strings.form_labels(i).label_code = "insight.submit.validate" Then
                    Me.chkvalidate.Text() = form_strings.form_labels(i).text
                End If
                If form_strings.form_labels(i).label_code = "insight.submit.close" Then
                    Me.Chkclose.Text() = form_strings.form_labels(i).text
                End If

            Next

            form_name = "insight.build"

            For i = 0 To bc_am_load_objects.obc_prefs.language_strings.forms.Count - 1
                If bc_am_load_objects.obc_prefs.language_strings.forms(i).form_name = form_name Then
                    form_strings = bc_am_load_objects.obc_prefs.language_strings.forms(i)
                    Exit For
                End If
            Next

            For i = 0 To form_strings.form_labels.Count - 1
                If form_strings.form_labels(i).label_code = "insight.build.schema" Then
                    Me.Label8.Text() = form_strings.form_labels(i).text
                End If
                If form_strings.form_labels(i).label_code = "insight.build.all" Then
                    Me.RadioButton1.Text() = form_strings.form_labels(i).text
                End If
                If form_strings.form_labels(i).label_code = "insight.build.mine" Then
                    Me.RadioButton2.Text() = form_strings.form_labels(i).text
                End If
                If form_strings.form_labels(i).label_code = "insight.build.ok" Then
                    Me.Btnnext.Text() = form_strings.form_labels(i).text
                End If
                If form_strings.form_labels(i).label_code = "insight.build.cancel" Then
                    Me.Button1.Text() = form_strings.form_labels(i).text
                End If
            Next

            form_name = "create.main"
            For i = 0 To bc_am_load_objects.obc_prefs.language_strings.forms.Count - 1
                If bc_am_load_objects.obc_prefs.language_strings.forms(i).form_name = form_name Then
                    form_strings = bc_am_load_objects.obc_prefs.language_strings.forms(i)
                    Exit For
                End If
            Next

            'For i = 0 To form_strings.form_labels.Count - 1
            '    If form_strings.form_labels(i).label_code = "create.main.log" Then
            '        Me.Label4.Text = form_strings.form_labels(i).text
            '    End If
            '    If form_strings.form_labels(i).label_code = "create.main.role" Then
            '        Me.Label5.Text = form_strings.form_labels(i).text
            '    End If
            '    If form_strings.form_labels(i).label_code = "create.main.version" Then
            '        Me.Label7.Text = form_strings.form_labels(i).text
            '    End If
            'Next

            Me.Label1.Visible = False
            Me.Label6.Visible = False
            Me.Label2.Visible = False
            Me.Label7.Visible = False
            Me.Label5.Visible = False
            Me.Label4.Visible = False

            Me.LabelCon.Visible = False
            Me.LabelConStr.Visible = False

            'Me.BlueCurve_TextSearch1.Visible = True
            'BlueCurve_TextSearch1.SearchUserEntitiesOnly = bc_cs_central_settings.alt_entity_for_apref
            'BlueCurve_TextSearch1.SearchBuildEntitiesOnly = bc_cs_central_settings.alt_entity_for_build

            For i = 0 To bc_am_load_objects.obc_entities.entity.Count - 1
                If bc_am_load_objects.obc_entities.entity(i).class_name = Me.lstclass.SelectedItem Then
                    BlueCurve_TextSearch1.SearchClass = bc_am_load_objects.obc_entities.entity(i).class_id
                    Exit For
                End If
            Next
            BlueCurve_TextSearch1.SearchSetup(False)
            'lstentities.Location = New System.Drawing.Point(256, 95)
            'lstentities.Size = New System.Drawing.Size(265, 132)

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_main_load", "new", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_in_main_load", "new", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        ok_selected = False
        Me.Hide()
    End Sub
    Public Sub populate_errors_list(ByVal errors_list As bc_om_cell_errors)
        Dim slog As New bc_cs_activity_log("bc_am_in_main_load", "populate_errors_list", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            Btnnext.Enabled = True
            Dim i As Integer
            error_mode = True
            Me.Btnnext.Text = "&Goto"
            Me.Button1.Text = "&Close"
            Me.ListErrors.Visible = True
            Me.Label9.Visible = False
            Me.txtcomment.Visible = False
            Me.txtcomment.Enabled = False
            Me.CheckBox1.Visible = False
            Me.CheckBox2.Enabled = True
            Me.CheckBox2.Checked = True
            Me.BlueCurve_TextSearch1.Visible = False

            Me.Btnnext.Enabled = False
            ListErrors.Items.Clear()
            Me.Text = "Blue Curve insight submission errors"
            Beep()
            For i = 0 To errors_list.bc_om_cell_errors.Count - 1
                Dim item As New ListViewItem(CStr(errors_list.bc_om_cell_errors(i).msg))
                If errors_list.bc_om_cell_errors(i).comment_req = "1" Then
                    item.SubItems.Add("Y")
                Else
                    item.SubItems.Add("N")
                End If
                item.SubItems.Add(errors_list.bc_om_cell_errors(i).sheetname)
                item.SubItems.Add(errors_list.bc_om_cell_errors(i).row)
                item.SubItems.Add(errors_list.bc_om_cell_errors(i).col)
                item.SubItems.Add(CStr(i) + 1)
                ListErrors.Items.Add(item)
            Next
            Me.Activate()
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_main_load", "populate_errors_list", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_in_main_load", "populate_errors_list", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    Private Sub populate_list_boxes()
        Dim slog As New bc_cs_activity_log("bc_am_in_main_load", "populate_list_boxes", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            Dim i As Integer

            REM populate schemas
            Me.lstcontributor.Items.Clear()
            For i = 0 To bc_am_load_objects.obc_om_insight_submission_entity_links.bc_om_insight_submission_entity_links.Count - 1
                If i > 0 Then
                    If bc_am_load_objects.obc_om_insight_submission_entity_links.bc_om_insight_submission_entity_links(i).schema_id <> bc_am_load_objects.obc_om_insight_submission_entity_links.bc_om_insight_submission_entity_links(i - 1).schema_id Then
                        Me.lstcontributor.Items.Add(bc_am_load_objects.obc_om_insight_submission_entity_links.bc_om_insight_submission_entity_links(i).schema_name)
                    End If
                Else
                    Me.lstcontributor.Items.Add(bc_am_load_objects.obc_om_insight_submission_entity_links.bc_om_insight_submission_entity_links(i).schema_name)
                End If
            Next
            REM populate entity class's
            'Me.lstclass.Items.Clear()
            'For i = 0 To bc_om_load_objects.obc_om_insight_submission_entity_links.bc_om_insight_submission_entity_links.Count - 1
            'Me.lstclass.Items.Add(bc_om_load_objects.obc_om_insight_submission_entity_links.bc_om_insight_submission_entity_links(i).parent_class_name)
            'Next


        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_main_load", "populate_list_boxes", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_in_main_load", "populate_list_boxes", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    Private Sub populate_entities(ByVal class_id As Long, ByVal for_user As Boolean)
        Dim slog As New bc_cs_activity_log("bc_am_in_main_load", "populate_entities", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            'Dim i, j As Integer

            REM populate entity class's
            Me.BlueCurve_TextSearch1.Visible = True
            Me.lstentities.Items.Clear()

            BlueCurve_TextSearch1.SearchClass = class_id
            BlueCurve_TextSearch1.SearchRefresh()

            'For i = 0 To bc_am_load_objects.obc_entities.entity.Count - 1
            '    If bc_am_load_objects.obc_entities.entity(i).class_id = class_id Then
            '        If for_user = True Then
            '            REM check if entity is user pref
            '            For j = 0 To bc_am_load_objects.obc_prefs.pref.Count - 1
            '                If bc_am_load_objects.obc_prefs.pref(j).entity_id = bc_am_load_objects.obc_entities.entity(i).id Then
            '                    Me.lstentities.Items.Add(bc_am_load_objects.obc_entities.entity(i).name)
            '                End If
            '            Next
            '        Else
            '            Me.lstentities.Items.Add(bc_am_load_objects.obc_entities.entity(i).name)
            '        End If
            '    End If
            'Next

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_main_load", "populate_entities", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_in_main_load", "populate_entities", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    Private Sub lstclass_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstclass.SelectedIndexChanged
        If Me.lstclass.SelectedIndex > -1 Then
            Dim i As Integer

            Select Case True
                Case bc_am_insight_link_names.show_all_entities And Not bc_am_insight_link_names.my_entities_default
                    Me.RadioButton1.Visible = True
                    Me.RadioButton1.Checked = True
                    Me.RadioButton2.Visible = True
                    BlueCurve_TextSearch1.SearchUserEntitiesOnly = False
                Case bc_am_insight_link_names.show_all_entities And bc_am_insight_link_names.my_entities_default
                    Me.RadioButton1.Visible = True
                    Me.RadioButton2.Checked = True
                    Me.RadioButton2.Visible = True
                    BlueCurve_TextSearch1.SearchUserEntitiesOnly = True
                Case Not bc_am_insight_link_names.show_all_entities
                    Me.RadioButton1.Visible = False
                    Me.RadioButton2.Checked = True
                    Me.RadioButton2.Visible = False
                    BlueCurve_TextSearch1.SearchUserEntitiesOnly = True
            End Select

            For i = 0 To bc_am_load_objects.obc_entities.entity.Count - 1
                If bc_am_load_objects.obc_entities.entity(i).class_name = Me.lstclass.SelectedItem Then
                    populate_entities(bc_am_load_objects.obc_entities.entity(i).class_id, bc_am_insight_link_names.my_entities_default)
                    Exit For
                End If
            Next

        End If
        check_ready()
    End Sub
    Private Sub check_ready()
        If Me.lstclass.SelectedIndex = -1 Or Me.lstcontributor.SelectedIndex = -1 Or Me.lstentities.SelectedIndex = -1 Then
            Me.Btnnext.Enabled = False
        End If
        If Me.lstclass.SelectedIndex > -1 And Me.lstcontributor.SelectedIndex > -1 And Me.lstentities.SelectedIndex > -1 Then
            Me.Btnnext.Enabled = True
        End If
    End Sub
    Private Sub Btnnext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btnnext.Click
        Dim slog As New bc_cs_activity_log("bc_am_in_main_load", "Btnnext_Click", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Me.ok_selected = True
            If submit_mode = True Then
                If Me.scbostage.Text = "Publish" Then
                    Dim ocommentary As New bc_cs_activity_log("bc_am_in_main_load", "Btnnext_Click", bc_cs_activity_codes.COMMENTARY, "Financial Workflow Stage set to 8")
                    bc_am_load_objects.obc_om_insight_contribution_for_entity.workflow_stage = 8
                Else
                    Dim ocommentary As New bc_cs_activity_log("bc_am_in_main_load", "Btnnext_Click", bc_cs_activity_codes.COMMENTARY, "Financial Workflow Stage set to 1")
                    bc_am_load_objects.obc_om_insight_contribution_for_entity.workflow_stage = 1
                End If

                Me.Hide()
                Exit Sub
            End If
            If error_mode = True Then
                If ListErrors.SelectedItems(0).SubItems(1).Text = "Y" And Me.txtcomment.Text = "" Then
                    Dim omsg As New bc_cs_message("Blue Curve insight", "Comment must be entered!", bc_cs_message.MESSAGE)
                    Me.txtcomment.Select()
                    Exit Sub
                End If
                Dim sheetname = ListErrors.SelectedItems(0).SubItems(2).Text

                Dim row = ListErrors.SelectedItems(0).SubItems(3).Text
                Dim col = ListErrors.SelectedItems(0).SubItems(4).Text
                bc_am_load_objects.obc_in_ao_object.highlight_cell(sheetname, CInt(row), CInt(col), Me.txtcomment.Text, Me.CheckBox2.Checked)
                Me.Btnnext.Enabled = False
                Me.CheckBox1.Enabled = False
                Exit Sub
            End If

            Dim i, j As Integer
            j = 0
            For i = 0 To bc_am_load_objects.obc_om_insight_submission_entity_links.bc_om_insight_submission_entity_links.Count - 1
                If bc_am_load_objects.obc_om_insight_submission_entity_links.bc_om_insight_submission_entity_links(i).schema_name = Me.lstcontributor.Text Then
                    If j = Me.lstclass.SelectedIndex Then
                        Me.class_id = bc_am_load_objects.obc_om_insight_submission_entity_links.bc_om_insight_submission_entity_links(i).parent_entity_class_id
                        Me.class_name = bc_am_load_objects.obc_om_insight_submission_entity_links.bc_om_insight_submission_entity_links(i).parent_class_name()
                        Me.contributor_id = bc_am_load_objects.obc_om_insight_submission_entity_links.bc_om_insight_submission_entity_links(i).schema_id
                        Me.contributor_name = bc_am_load_objects.obc_om_insight_submission_entity_links.bc_om_insight_submission_entity_links(i).schema_name
                    End If
                    j = j + 1
                End If
            Next
            j = 0

            Me.entity_id = BlueCurve_TextSearch1.ShownEntityList(Me.lstentities.SelectedIndex)

            For i = 0 To bc_am_load_objects.obc_entities.entity.Count - 1
                If bc_am_load_objects.obc_entities.entity(i).id = Me.entity_id Then
                    If bc_am_load_objects.obc_entities.entity(i).class_id = Me.class_id Then
                        Me.entity_name = bc_am_load_objects.obc_entities.entity(i).name
                        Exit For
                    End If
                End If
            Next

            Me.Hide()
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_main_load", "Btnnext_Click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_in_main_load", "Btnnext_Click", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    

    Private Sub ListErrors_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ListErrors.MouseUp
        Dim slog As New bc_cs_activity_log("bc_am_in_main_load", "ListErrors_SelectedIndexChanged", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            If ListErrors.SelectedItems.Count > 0 Then
                Me.Btnnext.Enabled = True
                Me.txtcomment.Text = ""
                If ListErrors.SelectedItems(0).SubItems(1).Text = "Y" Then
                    Me.txtcomment.Enabled = True
                    Me.txtcomment.Visible = True
                    Me.CheckBox1.Visible = True
                    Me.CheckBox1.Enabled = True
                    Me.Btnnext.Text = "Insert"
                    Me.Label9.Visible = True
                Else
                    Me.txtcomment.Enabled = False
                    Me.txtcomment.Visible = False
                    Me.CheckBox1.Enabled = False
                    Me.CheckBox1.Visible = False
                    Me.Btnnext.Text = "Goto"
                    Me.Label9.Visible = False
                End If
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_main_load", "ListErrors_SelectedIndexChanged", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_in_main_load", "ListErrors_SelectedIndexChanged", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        If Me.CheckBox1.Checked = True Then
            Dim sheetname = ListErrors.SelectedItems(0).SubItems(2).Text
            Dim row = ListErrors.SelectedItems(0).SubItems(3).Text
            Dim col = ListErrors.SelectedItems(0).SubItems(4).Text
            Me.txtcomment.Text = bc_am_load_objects.obc_in_ao_object.get_cell_comment(sheetname, CInt(row), CInt(col))
        Else
            Me.txtcomment.Text = ""
        End If
    End Sub

    Private Sub ListErrors_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListErrors.SelectedIndexChanged
        If Me.ListErrors.SelectedItems.Count = 0 Then
            Try
                Btnnext.Enabled = False
                Me.CheckBox1.Checked = True
                Me.CheckBox1.Enabled = False
            Catch

            End Try
        Else
            Btnnext.Enabled = True

        End If
    End Sub
    Public Sub load_submit_controls()
        Dim i As Integer
        Me.Text = "Blue Curve insight Submit"
        Me.lworkbookname.Visible = True
        Me.BlueCurve_TextSearch1.Visible = False
        Me.BlueCurve_TextSearch1.Enabled = False
        Me.stxtname.Visible = True
        Me.stxtanal.Visible = True
        Me.sanalyst.Visible = True
        Me.sstage.Visible = True
        REM for p1 always submit data to Draft
        Me.scbostage.Items.Clear()
        Me.scbostage.Enabled = True
        REM populate based on workflow for user role
        If bc_am_load_objects.obc_prefs.financial_workflow_stages.Count = 0 Then
            Me.scbostage.Items.Add("Draft")
            Me.scbostage.Items.Add("Publish")
            Me.scbostage.SelectedIndex = 0
        Else
            For i = 0 To bc_am_load_objects.obc_prefs.financial_workflow_stages.Count - 1
                If bc_am_load_objects.obc_prefs.financial_workflow_stages(i) = 1 Then
                    Me.scbostage.Items.Add("Draft")
                End If
                If bc_am_load_objects.obc_prefs.financial_workflow_stages(i) = 8 Then
                    Me.scbostage.Items.Add("Publish")
                End If
            Next
            Me.scbostage.SelectedIndex = 0
            If bc_am_load_objects.obc_prefs.default_financial_workflow_stage = 0 And Me.scbostage.Items.Count = 2 Then
                Me.scbostage.SelectedIndex = 1
            End If
            If Me.scbostage.Items.Count = 1 Then
                Me.scbostage.SelectedIndex = 0
                Me.scbostage.Enabled = False
            End If
        End If
        Me.scbostage.Visible = True
        Me.Chkclose.Visible = True
        Me.chkvalidate.Visible = True
        Me.ListErrors.Visible = False
        Me.RadioButton1.Visible = False
        Me.RadioButton2.Visible = False
        Me.lstcontributor.Visible = False
        Me.lstclass.Visible = False
        Me.lstentities.Visible = False
        Me.description.Visible = False
        Me.Label8.Visible = False
        Me.Label9.Visible = False
        Me.CheckBox1.Visible = False
        Me.txtcomment.Visible = False
        'Me.Btnnext.Enabled = True
        Me.BlueCurve_TextSearch1.Visible = False
        Me.BlueCurve_TextSearch1.Enabled = False
    End Sub

    Private Sub lstcontributor_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstcontributor.SelectedIndexChanged
        Dim i As Integer
        REM populate entity class's
        Me.lstclass.Items.Clear()
        For i = 0 To bc_am_load_objects.obc_om_insight_submission_entity_links.bc_om_insight_submission_entity_links.Count - 1
            If bc_am_load_objects.obc_om_insight_submission_entity_links.bc_om_insight_submission_entity_links(i).schema_name = Me.lstcontributor.Text Then
                Me.lstclass.Items.Add(bc_am_load_objects.obc_om_insight_submission_entity_links.bc_om_insight_submission_entity_links(i).parent_class_name)
            End If
        Next
        If i > 0 Then
            Me.lstclass.SelectedIndex = 0
        End If
        check_ready()
    End Sub

    Private Sub lstentities_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstentities.SelectedIndexChanged
        check_ready()
    End Sub

    Private Sub RadioButton1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton1.CheckedChanged
        Dim i As Integer

        For i = 0 To bc_am_load_objects.obc_entities.entity.Count - 1
            If bc_am_load_objects.obc_entities.entity(i).class_name = Me.lstclass.SelectedItem Then
                BlueCurve_TextSearch1.SearchUserEntitiesOnly = False
                populate_entities(bc_am_load_objects.obc_entities.entity(i).class_id, False)
                Exit For
            End If
        Next
        Me.Btnnext.Enabled = False
    End Sub
    Private Sub RadioButton2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton2.CheckedChanged
        Dim i As Integer

        For i = 0 To bc_am_load_objects.obc_entities.entity.Count - 1
            If bc_am_load_objects.obc_entities.entity(i).class_name = Me.lstclass.SelectedItem Then
                BlueCurve_TextSearch1.SearchUserEntitiesOnly = True
                populate_entities(bc_am_load_objects.obc_entities.entity(i).class_id, True)
                Exit For
            End If
        Next

        Me.Btnnext.Enabled = False
    End Sub
    Private Sub Chkclose_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chkclose.CheckedChanged

    End Sub

    Private Sub chkvalidate_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkvalidate.CheckedChanged
        If Me.chkvalidate.Checked = True Then
            Me.Chkclose.Checked = False
            Me.Chkclose.Enabled = False
            Me.CheckBox3.Checked = False
            Me.CheckBox4.Checked = False
            Me.CheckBox5.Checked = False
        Else
            Me.Chkclose.Enabled = True
        End If
    End Sub

    Private Sub CheckBox3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox3.CheckedChanged
        If CheckBox3.Checked = True Then
            close_mode = CHECKED_OUT
            Me.CheckBox4.Checked = False
            Me.CheckBox5.Checked = False
            If bc_am_insight_formats.controlled_Submission = 1 Then
                Me.Chkclose.Enabled = True
            End If
            Me.Chkclose.Checked = False
            Me.chkvalidate.Checked = False
        End If
    End Sub

    Private Sub CheckBox4_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox4.CheckedChanged
        If CheckBox4.Checked = True Then
            close_mode = CHECKED_IN
            Me.CheckBox3.Checked = False
            Me.CheckBox5.Checked = False
            Me.Chkclose.Checked = True
            Me.Chkclose.Enabled = False
            Me.chkvalidate.Checked = False
        End If
    End Sub

    Private Sub CheckBox5_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox5.CheckedChanged
        If CheckBox5.Checked = True Then
            close_mode = LOCAL
            Me.CheckBox3.Checked = False
            Me.CheckBox4.Checked = False
            Me.Chkclose.Enabled = True
            Me.Chkclose.Checked = False
            Me.chkvalidate.Checked = False
        End If
    End Sub


    Private Sub lstclass_SelectedIndexChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstclass.SelectedIndexChanged

    End Sub

    Private Sub lstclass_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstclass.Click

    End Sub

    Private Sub lstclass_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lstclass.MouseUp

    End Sub
    Private Sub CheckBox6_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox6.CheckedChanged
        If Me.CheckBox6.Checked = True Then
            Dim omessage As New bc_cs_message("Blue Curve - insight", "Warning: if workbook is checked out to another user, those users changes could potentially be lost. If unsure please uncheck this option and contact the system administrator", bc_cs_message.MESSAGE)
        End If
    End Sub

    Private Sub txtcomment_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtcomment.TextChanged

    End Sub

    Private Sub scbostage_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles scbostage.SelectedIndexChanged

    End Sub

    Private Sub BlueCurve_TextSearch1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BlueCurve_TextSearch1.Load

    End Sub

    Private Sub slitanal_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles slitanal.SelectedIndexChanged
        If Me.slitanal.SelectedIndex > -1 Then
            Me.Btnnext.Enabled = True
        End If

    End Sub
   

End Class
