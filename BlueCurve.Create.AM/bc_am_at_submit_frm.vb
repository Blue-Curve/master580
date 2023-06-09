Imports System.Windows.forms
Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports BlueCurve.Core.AS
Imports System.Drawing

#Region "changes"
'Changes:
'Tracker                 Initials                   Date                      Synopsis
'FIL 6835                PR                         8/1/2014                  Added default support doc
#End Region
Public Class bc_am_at_submit_frm
    Inherits System.Windows.Forms.Form
    Public ok_selected As Boolean
    Public class_id As Long
    Public tclass_id As Long
    Public taxonomy As Integer
    Public original_stage As String
    Public loading As Boolean
    Public attach_mode As Boolean = False
    Public attach_file As String
    Public edit_mode As Boolean = False
    Public ldoc As New bc_om_document
    Public list_start_index As Integer = -1
    Public register_only As Boolean = False
    Public regular_report As Boolean = False
    Public lead_entity As Boolean = True
    Public disclosures As Boolean = True
    Const form_name = "create.submit"

    Friend WithEvents dh As System.Windows.Forms.ComboBox
    Friend WithEvents ds As System.Windows.Forms.ComboBox
    Friend WithEvents uxAvailable As System.Windows.Forms.Label
    Friend WithEvents uxMoveScannedEntry As System.Windows.Forms.Button
    Friend WithEvents uxScannedList As bc_cc_listbox
    Friend WithEvents uxSelected As System.Windows.Forms.Label
    Friend WithEvents uxScanned As System.Windows.Forms.Label

    Protected form_strings As New bc_om_user_form

    Private scannedItemsList As String(,)
    Private scannedIDs As ArrayList

    Private Const C_AUTHOR = "AUTHOR"
    Private Const C_DISCLOSURE = "DISCLOSURE"
    Public WithEvents Cpub As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Lsummary As System.Windows.Forms.Label
    Friend WithEvents uxMoveDown As System.Windows.Forms.Button
    Friend WithEvents uxMoveUp As System.Windows.Forms.Button
    Friend WithEvents bsnew As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents uxImages As System.Windows.Forms.ImageList
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents cpts As System.Windows.Forms.ComboBox
    Friend WithEvents Btncancel As System.Windows.Forms.Button
    Friend WithEvents Panel5 As System.Windows.Forms.Panel
    Friend WithEvents lvwsupport As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents mimeimages As System.Windows.Forms.ImageList
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DeleteToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Btntoggle As System.Windows.Forms.Button

    REM Steve Wooderson Drexel 27/01/2014 Show prefs only
    Private btoggle As Boolean
    Private prev_toggle As Boolean
    Private Prev_entities As New ArrayList
    Friend WithEvents gec As System.Windows.Forms.GroupBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Chkurgent As System.Windows.Forms.CheckBox
    Friend WithEvents Cbehalf As System.Windows.Forms.ComboBox
    Friend WithEvents bdetails As System.Windows.Forms.Button

    Public import_support_doc As Boolean = False
    Public behalf_of_authors_ids As New ArrayList
    Public behalf_of_authors_names As New ArrayList


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
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtdate As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label6 As System.Windows.Forms.Label

    Friend WithEvents Label4 As System.Windows.Forms.Label
    Public WithEvents txttitle As System.Windows.Forms.TextBox

    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents btnsubmit As System.Windows.Forms.Button
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtsubtitle As System.Windows.Forms.TextBox

    Friend WithEvents ListBox1 As bc_cc_listbox
    Friend WithEvents ListBox2 As bc_cc_listbox
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents PictureBox3 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox4 As System.Windows.Forms.PictureBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents btnnetwork As System.Windows.Forms.RadioButton
    Friend WithEvents btnlocal As System.Windows.Forms.RadioButton
    Friend WithEvents cboworkflow As System.Windows.Forms.ComboBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Button5 As System.Windows.Forms.Button
    Friend WithEvents cbotaxonomy As System.Windows.Forms.ComboBox
    Friend WithEvents gattach As System.Windows.Forms.GroupBox
    Friend WithEvents Button6 As System.Windows.Forms.Button
    Friend WithEvents tattach As System.Windows.Forms.TextBox
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents txtsummary As System.Windows.Forms.TextBox
    Friend WithEvents tsummary As System.Windows.Forms.TextBox
    Friend WithEvents txtteaser As System.Windows.Forms.RichTextBox
    Friend WithEvents lstpages As System.Windows.Forms.ComboBox
    Friend WithEvents ltaxonomy As System.Windows.Forms.Label
    Friend WithEvents ttaxonomy As System.Windows.Forms.TabControl
    Friend WithEvents Author As System.Windows.Forms.TabPage
    Friend WithEvents Entity As System.Windows.Forms.TabPage
    Friend WithEvents Disc As System.Windows.Forms.TabPage
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_at_submit_frm))
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnsubmit = New System.Windows.Forms.Button()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtdate = New System.Windows.Forms.DateTimePicker()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txttitle = New System.Windows.Forms.TextBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Btntoggle = New System.Windows.Forms.Button()
        Me.lvwsupport = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.mimeimages = New System.Windows.Forms.ImageList()
        Me.Panel5 = New System.Windows.Forms.Panel()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeleteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.bsnew = New System.Windows.Forms.Button()
        Me.uxImages = New System.Windows.Forms.ImageList()
        Me.cpts = New System.Windows.Forms.ComboBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.uxMoveDown = New System.Windows.Forms.Button()
        Me.uxMoveUp = New System.Windows.Forms.Button()
        Me.Cpub = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtsubtitle = New System.Windows.Forms.TextBox()
        Me.uxMoveScannedEntry = New System.Windows.Forms.Button()
        Me.uxScannedList = New BlueCurve.Core.[AS].bc_cc_listbox()
        Me.ds = New System.Windows.Forms.ComboBox()
        Me.dh = New System.Windows.Forms.ComboBox()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.txtsummary = New System.Windows.Forms.TextBox()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.txtteaser = New System.Windows.Forms.RichTextBox()
        Me.ttaxonomy = New System.Windows.Forms.TabControl()
        Me.Author = New System.Windows.Forms.TabPage()
        Me.Entity = New System.Windows.Forms.TabPage()
        Me.Disc = New System.Windows.Forms.TabPage()
        Me.lstpages = New System.Windows.Forms.ComboBox()
        Me.Lsummary = New System.Windows.Forms.Label()
        Me.ltaxonomy = New System.Windows.Forms.Label()
        Me.cbotaxonomy = New System.Windows.Forms.ComboBox()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.btnlocal = New System.Windows.Forms.RadioButton()
        Me.btnnetwork = New System.Windows.Forms.RadioButton()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.cboworkflow = New System.Windows.Forms.ComboBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.ListBox1 = New BlueCurve.Core.[AS].bc_cc_listbox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.ListBox2 = New BlueCurve.Core.[AS].bc_cc_listbox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.tsummary = New System.Windows.Forms.TextBox()
        Me.uxAvailable = New System.Windows.Forms.Label()
        Me.uxSelected = New System.Windows.Forms.Label()
        Me.uxScanned = New System.Windows.Forms.Label()
        Me.gattach = New System.Windows.Forms.GroupBox()
        Me.tattach = New System.Windows.Forms.TextBox()
        Me.Button6 = New System.Windows.Forms.Button()
        Me.PictureBox4 = New System.Windows.Forms.PictureBox()
        Me.PictureBox3 = New System.Windows.Forms.PictureBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip()
        Me.Btncancel = New System.Windows.Forms.Button()
        Me.gec = New System.Windows.Forms.GroupBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Chkurgent = New System.Windows.Forms.CheckBox()
        Me.Cbehalf = New System.Windows.Forms.ComboBox()
        Me.bdetails = New System.Windows.Forms.Button()
        Me.Panel1.SuspendLayout()
        Me.Panel5.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.ttaxonomy.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.gattach.SuspendLayout()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gec.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.White
        Me.Label2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.WindowFrame
        Me.Label2.Location = New System.Drawing.Point(235, 9)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(40, 23)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "User"
        Me.Label2.Visible = False
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.White
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Label1.Location = New System.Drawing.Point(235, 32)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(40, 32)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Role"
        Me.Label1.Visible = False
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label3.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(4, 526)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(216, 15)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "© Blue Curve Limited. "
        '
        'btnsubmit
        '
        Me.btnsubmit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnsubmit.BackColor = System.Drawing.SystemColors.ControlLight
        Me.btnsubmit.Font = New System.Drawing.Font("Arial Unicode MS", 9.25!)
        Me.btnsubmit.Location = New System.Drawing.Point(472, 492)
        Me.btnsubmit.Name = "btnsubmit"
        Me.btnsubmit.Size = New System.Drawing.Size(72, 23)
        Me.btnsubmit.TabIndex = 11
        Me.btnsubmit.Text = "&Submit"
        Me.btnsubmit.UseVisualStyleBackColor = False
        '
        'Label7
        '
        Me.Label7.BackColor = System.Drawing.Color.Transparent
        Me.Label7.Font = New System.Drawing.Font("Arial Unicode MS", 9.25!, System.Drawing.FontStyle.Bold)
        Me.Label7.Location = New System.Drawing.Point(299, 65)
        Me.Label7.Name = "Label7"
        Me.Label7.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.Label7.Size = New System.Drawing.Size(40, 16)
        Me.Label7.TabIndex = 7
        Me.Label7.Text = "Date"
        '
        'txtdate
        '
        Me.txtdate.Font = New System.Drawing.Font("Arial Unicode MS", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtdate.Location = New System.Drawing.Point(341, 63)
        Me.txtdate.Name = "txtdate"
        Me.txtdate.Size = New System.Drawing.Size(119, 22)
        Me.txtdate.TabIndex = 5
        Me.txtdate.Value = New Date(2006, 2, 8, 11, 15, 34, 590)
        '
        'Label6
        '
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Font = New System.Drawing.Font("Arial Unicode MS", 9.25!, System.Drawing.FontStyle.Bold)
        Me.Label6.Location = New System.Drawing.Point(376, 39)
        Me.Label6.Name = "Label6"
        Me.Label6.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.Label6.Size = New System.Drawing.Size(96, 16)
        Me.Label6.TabIndex = 5
        Me.Label6.Text = "No. Of Pages"
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Font = New System.Drawing.Font("Arial Unicode MS", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(15, 39)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(40, 16)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "Title"
        '
        'txttitle
        '
        Me.txttitle.Font = New System.Drawing.Font("Arial Unicode MS", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txttitle.Location = New System.Drawing.Point(96, 37)
        Me.txttitle.MaxLength = 250
        Me.txttitle.Name = "txttitle"
        Me.txttitle.Size = New System.Drawing.Size(197, 22)
        Me.txttitle.TabIndex = 0
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.Btntoggle)
        Me.Panel1.Controls.Add(Me.lvwsupport)
        Me.Panel1.Controls.Add(Me.Panel5)
        Me.Panel1.Controls.Add(Me.uxMoveDown)
        Me.Panel1.Controls.Add(Me.uxMoveUp)
        Me.Panel1.Controls.Add(Me.Cpub)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.txtsubtitle)
        Me.Panel1.Controls.Add(Me.uxMoveScannedEntry)
        Me.Panel1.Controls.Add(Me.uxScannedList)
        Me.Panel1.Controls.Add(Me.ds)
        Me.Panel1.Controls.Add(Me.dh)
        Me.Panel1.Controls.Add(Me.TabControl1)
        Me.Panel1.Controls.Add(Me.ttaxonomy)
        Me.Panel1.Controls.Add(Me.lstpages)
        Me.Panel1.Controls.Add(Me.Lsummary)
        Me.Panel1.Controls.Add(Me.ltaxonomy)
        Me.Panel1.Controls.Add(Me.cbotaxonomy)
        Me.Panel1.Controls.Add(Me.Button5)
        Me.Panel1.Controls.Add(Me.Panel3)
        Me.Panel1.Controls.Add(Me.Panel2)
        Me.Panel1.Controls.Add(Me.Button4)
        Me.Panel1.Controls.Add(Me.Button3)
        Me.Panel1.Controls.Add(Me.Button2)
        Me.Panel1.Controls.Add(Me.ListBox1)
        Me.Panel1.Controls.Add(Me.Label9)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.txtdate)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.txttitle)
        Me.Panel1.Controls.Add(Me.ListBox2)
        Me.Panel1.Controls.Add(Me.Button1)
        Me.Panel1.Controls.Add(Me.tsummary)
        Me.Panel1.Controls.Add(Me.uxAvailable)
        Me.Panel1.Controls.Add(Me.uxSelected)
        Me.Panel1.Controls.Add(Me.uxScanned)
        Me.Panel1.Location = New System.Drawing.Point(0, 64)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(552, 422)
        Me.Panel1.TabIndex = 6
        '
        'Btntoggle
        '
        Me.Btntoggle.BackColor = System.Drawing.SystemColors.ControlLight
        Me.Btntoggle.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Btntoggle.Location = New System.Drawing.Point(211, 338)
        Me.Btntoggle.Name = "Btntoggle"
        Me.Btntoggle.Size = New System.Drawing.Size(72, 24)
        Me.Btntoggle.TabIndex = 49
        Me.Btntoggle.Text = "&Toggle"
        Me.Btntoggle.UseVisualStyleBackColor = False
        Me.Btntoggle.Visible = False
        '
        'lvwsupport
        '
        Me.lvwsupport.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvwsupport.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3})
        Me.lvwsupport.FullRowSelect = True
        Me.lvwsupport.Location = New System.Drawing.Point(15, 427)
        Me.lvwsupport.MultiSelect = False
        Me.lvwsupport.Name = "lvwsupport"
        Me.lvwsupport.Size = New System.Drawing.Size(525, 74)
        Me.lvwsupport.SmallImageList = Me.mimeimages
        Me.lvwsupport.TabIndex = 42
        Me.lvwsupport.UseCompatibleStateImageBehavior = False
        Me.lvwsupport.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Publication"
        Me.ColumnHeader1.Width = 89
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Title"
        Me.ColumnHeader2.Width = 144
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "File Path"
        Me.ColumnHeader3.Width = 281
        '
        'mimeimages
        '
        Me.mimeimages.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit
        Me.mimeimages.ImageSize = New System.Drawing.Size(16, 16)
        Me.mimeimages.TransparentColor = System.Drawing.Color.Transparent
        '
        'Panel5
        '
        Me.Panel5.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel5.Controls.Add(Me.Panel4)
        Me.Panel5.Controls.Add(Me.bsnew)
        Me.Panel5.Controls.Add(Me.cpts)
        Me.Panel5.Controls.Add(Me.Label10)
        Me.Panel5.Location = New System.Drawing.Point(15, 369)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(525, 74)
        Me.Panel5.TabIndex = 48
        '
        'Panel4
        '
        Me.Panel4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel4.Controls.Add(Me.MenuStrip1)
        Me.Panel4.Location = New System.Drawing.Point(-1, 30)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(525, 29)
        Me.Panel4.TabIndex = 48
        '
        'MenuStrip1
        '
        Me.MenuStrip1.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem1, Me.DeleteToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(525, 24)
        Me.MenuStrip1.TabIndex = 44
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(75, 20)
        Me.ToolStripMenuItem1.Text = "Categorize"
        '
        'DeleteToolStripMenuItem
        '
        Me.DeleteToolStripMenuItem.Name = "DeleteToolStripMenuItem"
        Me.DeleteToolStripMenuItem.Size = New System.Drawing.Size(52, 20)
        Me.DeleteToolStripMenuItem.Text = "Delete"
        '
        'bsnew
        '
        Me.bsnew.BackColor = System.Drawing.SystemColors.ControlLight
        Me.bsnew.Enabled = False
        Me.bsnew.Font = New System.Drawing.Font("Arial Unicode MS", 9.25!)
        Me.bsnew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.bsnew.ImageIndex = 0
        Me.bsnew.ImageList = Me.uxImages
        Me.bsnew.Location = New System.Drawing.Point(325, 2)
        Me.bsnew.Name = "bsnew"
        Me.bsnew.Size = New System.Drawing.Size(84, 24)
        Me.bsnew.TabIndex = 44
        Me.bsnew.Text = "Attach"
        Me.ToolTip1.SetToolTip(Me.bsnew, "Add a support document")
        Me.bsnew.UseVisualStyleBackColor = False
        '
        'uxImages
        '
        Me.uxImages.ImageStream = CType(resources.GetObject("uxImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.uxImages.TransparentColor = System.Drawing.Color.Transparent
        Me.uxImages.Images.SetKeyName(0, "")
        Me.uxImages.Images.SetKeyName(1, "")
        Me.uxImages.Images.SetKeyName(2, "")
        Me.uxImages.Images.SetKeyName(3, "")
        Me.uxImages.Images.SetKeyName(4, "")
        Me.uxImages.Images.SetKeyName(5, "")
        Me.uxImages.Images.SetKeyName(6, "")
        Me.uxImages.Images.SetKeyName(7, "")
        Me.uxImages.Images.SetKeyName(8, "")
        '
        'cpts
        '
        Me.cpts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cpts.FormattingEnabled = True
        Me.cpts.Location = New System.Drawing.Point(132, 3)
        Me.cpts.Name = "cpts"
        Me.cpts.Size = New System.Drawing.Size(183, 21)
        Me.cpts.TabIndex = 44
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(5, 6)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(121, 13)
        Me.Label10.TabIndex = 45
        Me.Label10.Text = "Add Support Publication"
        '
        'uxMoveDown
        '
        Me.uxMoveDown.BackColor = System.Drawing.SystemColors.ControlLight
        Me.uxMoveDown.Enabled = False
        Me.uxMoveDown.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxMoveDown.ForeColor = System.Drawing.SystemColors.WindowText
        Me.uxMoveDown.Location = New System.Drawing.Point(359, 304)
        Me.uxMoveDown.Name = "uxMoveDown"
        Me.uxMoveDown.Size = New System.Drawing.Size(24, 24)
        Me.uxMoveDown.TabIndex = 38
        Me.uxMoveDown.Text = "٧"
        Me.uxMoveDown.UseVisualStyleBackColor = False
        '
        'uxMoveUp
        '
        Me.uxMoveUp.BackColor = System.Drawing.SystemColors.ControlLight
        Me.uxMoveUp.Enabled = False
        Me.uxMoveUp.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxMoveUp.ForeColor = System.Drawing.SystemColors.WindowText
        Me.uxMoveUp.Location = New System.Drawing.Point(359, 279)
        Me.uxMoveUp.Name = "uxMoveUp"
        Me.uxMoveUp.Size = New System.Drawing.Size(24, 24)
        Me.uxMoveUp.TabIndex = 37
        Me.uxMoveUp.Text = "٨"
        Me.uxMoveUp.UseVisualStyleBackColor = False
        '
        'Cpub
        '
        Me.Cpub.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Cpub.Enabled = False
        Me.Cpub.Location = New System.Drawing.Point(96, 11)
        Me.Cpub.Name = "Cpub"
        Me.Cpub.Size = New System.Drawing.Size(447, 21)
        Me.Cpub.TabIndex = 16
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Font = New System.Drawing.Font("Arial Unicode MS", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(15, 12)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(85, 20)
        Me.Label5.TabIndex = 40
        Me.Label5.Text = "Publication"
        '
        'txtsubtitle
        '
        Me.txtsubtitle.Font = New System.Drawing.Font("Arial Unicode MS", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtsubtitle.Location = New System.Drawing.Point(96, 63)
        Me.txtsubtitle.MaxLength = 95
        Me.txtsubtitle.Name = "txtsubtitle"
        Me.txtsubtitle.Size = New System.Drawing.Size(197, 22)
        Me.txtsubtitle.TabIndex = 1
        '
        'uxMoveScannedEntry
        '
        Me.uxMoveScannedEntry.BackColor = System.Drawing.SystemColors.ControlLight
        Me.uxMoveScannedEntry.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxMoveScannedEntry.ForeColor = System.Drawing.SystemColors.WindowText
        Me.uxMoveScannedEntry.Location = New System.Drawing.Point(359, 249)
        Me.uxMoveScannedEntry.Name = "uxMoveScannedEntry"
        Me.uxMoveScannedEntry.Size = New System.Drawing.Size(24, 24)
        Me.uxMoveScannedEntry.TabIndex = 36
        Me.uxMoveScannedEntry.Text = "<"
        Me.uxMoveScannedEntry.UseVisualStyleBackColor = False
        '
        'uxScannedList
        '
        Me.uxScannedList.Font = New System.Drawing.Font("Arial Unicode MS", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxScannedList.ItemHeight = 15
        Me.uxScannedList.Location = New System.Drawing.Point(385, 249)
        Me.uxScannedList.Name = "uxScannedList"
        Me.uxScannedList.Size = New System.Drawing.Size(155, 34)
        Me.uxScannedList.TabIndex = 35
        '
        'ds
        '
        Me.ds.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ds.FormattingEnabled = True
        Me.ds.Location = New System.Drawing.Point(502, 63)
        Me.ds.Name = "ds"
        Me.ds.Size = New System.Drawing.Size(40, 21)
        Me.ds.TabIndex = 34
        '
        'dh
        '
        Me.dh.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.dh.FormattingEnabled = True
        Me.dh.Location = New System.Drawing.Point(461, 63)
        Me.dh.Name = "dh"
        Me.dh.Size = New System.Drawing.Size(40, 21)
        Me.dh.TabIndex = 33
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Location = New System.Drawing.Point(15, 91)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(529, 112)
        Me.TabControl1.TabIndex = 26
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.txtsummary)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Size = New System.Drawing.Size(521, 86)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Summary Text"
        '
        'txtsummary
        '
        Me.txtsummary.Font = New System.Drawing.Font("Arial Unicode MS", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtsummary.Location = New System.Drawing.Point(0, 0)
        Me.txtsummary.MaxLength = 4000
        Me.txtsummary.Multiline = True
        Me.txtsummary.Name = "txtsummary"
        Me.txtsummary.Size = New System.Drawing.Size(520, 80)
        Me.txtsummary.TabIndex = 4
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.txtteaser)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Size = New System.Drawing.Size(521, 86)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Teaser Text"
        '
        'txtteaser
        '
        Me.txtteaser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtteaser.Location = New System.Drawing.Point(0, 0)
        Me.txtteaser.Name = "txtteaser"
        Me.txtteaser.Size = New System.Drawing.Size(520, 80)
        Me.txtteaser.TabIndex = 0
        Me.txtteaser.Text = ""
        '
        'ttaxonomy
        '
        Me.ttaxonomy.Controls.Add(Me.Author)
        Me.ttaxonomy.Controls.Add(Me.Entity)
        Me.ttaxonomy.Controls.Add(Me.Disc)
        Me.ttaxonomy.Location = New System.Drawing.Point(15, 206)
        Me.ttaxonomy.Name = "ttaxonomy"
        Me.ttaxonomy.SelectedIndex = 0
        Me.ttaxonomy.Size = New System.Drawing.Size(528, 26)
        Me.ttaxonomy.TabIndex = 32
        '
        'Author
        '
        Me.Author.Location = New System.Drawing.Point(4, 22)
        Me.Author.Name = "Author"
        Me.Author.Size = New System.Drawing.Size(520, 0)
        Me.Author.TabIndex = 0
        Me.Author.Text = "Author"
        '
        'Entity
        '
        Me.Entity.Location = New System.Drawing.Point(4, 22)
        Me.Entity.Name = "Entity"
        Me.Entity.Size = New System.Drawing.Size(520, 0)
        Me.Entity.TabIndex = 1
        Me.Entity.Text = "Entity"
        '
        'Disc
        '
        Me.Disc.Location = New System.Drawing.Point(4, 22)
        Me.Disc.Name = "Disc"
        Me.Disc.Size = New System.Drawing.Size(520, 0)
        Me.Disc.TabIndex = 2
        Me.Disc.Text = "Disclosures"
        '
        'lstpages
        '
        Me.lstpages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.lstpages.Location = New System.Drawing.Point(478, 39)
        Me.lstpages.Name = "lstpages"
        Me.lstpages.Size = New System.Drawing.Size(64, 21)
        Me.lstpages.TabIndex = 31
        '
        'Lsummary
        '
        Me.Lsummary.BackColor = System.Drawing.Color.Transparent
        Me.Lsummary.Font = New System.Drawing.Font("Arial Unicode MS", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Lsummary.Location = New System.Drawing.Point(16, 91)
        Me.Lsummary.Name = "Lsummary"
        Me.Lsummary.Size = New System.Drawing.Size(135, 16)
        Me.Lsummary.TabIndex = 30
        Me.Lsummary.Text = "Summary"
        Me.Lsummary.Visible = False
        '
        'ltaxonomy
        '
        Me.ltaxonomy.Font = New System.Drawing.Font("Arial Unicode MS", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ltaxonomy.Location = New System.Drawing.Point(324, 179)
        Me.ltaxonomy.Name = "ltaxonomy"
        Me.ltaxonomy.Size = New System.Drawing.Size(104, 24)
        Me.ltaxonomy.TabIndex = 25
        Me.ltaxonomy.Text = "Other Taxonomy"
        Me.ltaxonomy.Visible = False
        '
        'cbotaxonomy
        '
        Me.cbotaxonomy.Font = New System.Drawing.Font("Arial Unicode MS", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbotaxonomy.Location = New System.Drawing.Point(432, 176)
        Me.cbotaxonomy.Name = "cbotaxonomy"
        Me.cbotaxonomy.Size = New System.Drawing.Size(112, 24)
        Me.cbotaxonomy.TabIndex = 24
        Me.cbotaxonomy.Text = "please select..."
        Me.cbotaxonomy.Visible = False
        '
        'Button5
        '
        Me.Button5.BackColor = System.Drawing.SystemColors.ControlLight
        Me.Button5.Font = New System.Drawing.Font("Arial Unicode MS", 8.25!)
        Me.Button5.Location = New System.Drawing.Point(176, 176)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(72, 24)
        Me.Button5.TabIndex = 23
        Me.Button5.Text = "&Disclosures"
        Me.Button5.UseVisualStyleBackColor = False
        Me.Button5.Visible = False
        '
        'Panel3
        '
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel3.Controls.Add(Me.btnlocal)
        Me.Panel3.Controls.Add(Me.btnnetwork)
        Me.Panel3.Location = New System.Drawing.Point(15, 334)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(190, 32)
        Me.Panel3.TabIndex = 22
        '
        'btnlocal
        '
        Me.btnlocal.Font = New System.Drawing.Font("Arial Unicode MS", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnlocal.Location = New System.Drawing.Point(8, 5)
        Me.btnlocal.Name = "btnlocal"
        Me.btnlocal.Size = New System.Drawing.Size(96, 20)
        Me.btnlocal.TabIndex = 12
        Me.btnlocal.Text = "&Local"
        '
        'btnnetwork
        '
        Me.btnnetwork.Font = New System.Drawing.Font("Arial Unicode MS", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnnetwork.Location = New System.Drawing.Point(112, 5)
        Me.btnnetwork.Name = "btnnetwork"
        Me.btnnetwork.Size = New System.Drawing.Size(96, 23)
        Me.btnnetwork.TabIndex = 11
        Me.btnnetwork.Text = "&Central"
        '
        'Panel2
        '
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.cboworkflow)
        Me.Panel2.Controls.Add(Me.Label8)
        Me.Panel2.Font = New System.Drawing.Font("Arial Unicode MS", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Panel2.Location = New System.Drawing.Point(324, 334)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(216, 32)
        Me.Panel2.TabIndex = 21
        '
        'cboworkflow
        '
        Me.cboworkflow.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboworkflow.Location = New System.Drawing.Point(80, 4)
        Me.cboworkflow.Name = "cboworkflow"
        Me.cboworkflow.Size = New System.Drawing.Size(128, 23)
        Me.cboworkflow.TabIndex = 0
        '
        'Label8
        '
        Me.Label8.Font = New System.Drawing.Font("Arial Unicode MS", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(8, 8)
        Me.Label8.Name = "Label8"
        Me.Label8.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.Label8.Size = New System.Drawing.Size(64, 16)
        Me.Label8.TabIndex = 15
        Me.Label8.Text = "Workflow"
        '
        'Button4
        '
        Me.Button4.BackColor = System.Drawing.SystemColors.ControlLight
        Me.Button4.Font = New System.Drawing.Font("Arial Unicode MS", 8.25!)
        Me.Button4.Location = New System.Drawing.Point(96, 176)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(72, 24)
        Me.Button4.TabIndex = 19
        Me.Button4.Text = "&Entity"
        Me.Button4.UseVisualStyleBackColor = False
        Me.Button4.Visible = False
        '
        'Button3
        '
        Me.Button3.BackColor = System.Drawing.SystemColors.ControlLight
        Me.Button3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button3.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Button3.Location = New System.Drawing.Point(173, 295)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(24, 24)
        Me.Button3.TabIndex = 18
        Me.Button3.Text = "<"
        Me.Button3.UseVisualStyleBackColor = False
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.SystemColors.ControlLight
        Me.Button2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button2.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Button2.Location = New System.Drawing.Point(173, 265)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(24, 24)
        Me.Button2.TabIndex = 17
        Me.Button2.Text = ">"
        Me.Button2.UseVisualStyleBackColor = False
        '
        'ListBox1
        '
        Me.ListBox1.Font = New System.Drawing.Font("Arial Unicode MS", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListBox1.ItemHeight = 15
        Me.ListBox1.Location = New System.Drawing.Point(15, 249)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(155, 34)
        Me.ListBox1.Sorted = True
        Me.ListBox1.TabIndex = 16
        '
        'Label9
        '
        Me.Label9.BackColor = System.Drawing.Color.Transparent
        Me.Label9.Font = New System.Drawing.Font("Arial Unicode MS", 9.25!, System.Drawing.FontStyle.Bold)
        Me.Label9.Location = New System.Drawing.Point(15, 65)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(76, 20)
        Me.Label9.TabIndex = 15
        Me.Label9.Text = "Sub Title"
        '
        'ListBox2
        '
        Me.ListBox2.Font = New System.Drawing.Font("Arial Unicode MS", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListBox2.ItemHeight = 15
        Me.ListBox2.Location = New System.Drawing.Point(201, 249)
        Me.ListBox2.Name = "ListBox2"
        Me.ListBox2.Size = New System.Drawing.Size(155, 34)
        Me.ListBox2.TabIndex = 10
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.SystemColors.ControlLight
        Me.Button1.Font = New System.Drawing.Font("Arial Unicode MS", 8.25!)
        Me.Button1.Location = New System.Drawing.Point(16, 176)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(72, 24)
        Me.Button1.TabIndex = 15
        Me.Button1.Text = "&Author"
        Me.Button1.UseVisualStyleBackColor = False
        Me.Button1.Visible = False
        '
        'tsummary
        '
        Me.tsummary.Location = New System.Drawing.Point(16, 110)
        Me.tsummary.MaxLength = 3998
        Me.tsummary.Multiline = True
        Me.tsummary.Name = "tsummary"
        Me.tsummary.Size = New System.Drawing.Size(528, 93)
        Me.tsummary.TabIndex = 27
        '
        'uxAvailable
        '
        Me.uxAvailable.AutoSize = True
        Me.uxAvailable.Location = New System.Drawing.Point(12, 233)
        Me.uxAvailable.Name = "uxAvailable"
        Me.uxAvailable.Size = New System.Drawing.Size(53, 13)
        Me.uxAvailable.TabIndex = 37
        Me.uxAvailable.Text = "Available:"
        '
        'uxSelected
        '
        Me.uxSelected.AutoSize = True
        Me.uxSelected.Location = New System.Drawing.Point(201, 233)
        Me.uxSelected.Name = "uxSelected"
        Me.uxSelected.Size = New System.Drawing.Size(52, 13)
        Me.uxSelected.TabIndex = 38
        Me.uxSelected.Text = "Selected:"
        '
        'uxScanned
        '
        Me.uxScanned.AutoSize = True
        Me.uxScanned.Location = New System.Drawing.Point(392, 233)
        Me.uxScanned.Name = "uxScanned"
        Me.uxScanned.Size = New System.Drawing.Size(53, 13)
        Me.uxScanned.TabIndex = 39
        Me.uxScanned.Text = "Scanned:"
        '
        'gattach
        '
        Me.gattach.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.gattach.Controls.Add(Me.tattach)
        Me.gattach.Controls.Add(Me.Button6)
        Me.gattach.Location = New System.Drawing.Point(208, 487)
        Me.gattach.Name = "gattach"
        Me.gattach.Size = New System.Drawing.Size(262, 58)
        Me.gattach.TabIndex = 17
        Me.gattach.TabStop = False
        '
        'tattach
        '
        Me.tattach.Location = New System.Drawing.Point(8, 38)
        Me.tattach.Name = "tattach"
        Me.tattach.Size = New System.Drawing.Size(232, 20)
        Me.tattach.TabIndex = 17
        '
        'Button6
        '
        Me.Button6.BackColor = System.Drawing.SystemColors.ControlLight
        Me.Button6.Font = New System.Drawing.Font("Arial Unicode MS", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button6.Location = New System.Drawing.Point(8, 10)
        Me.Button6.Name = "Button6"
        Me.Button6.Size = New System.Drawing.Size(72, 24)
        Me.Button6.TabIndex = 16
        Me.Button6.Text = "&Attach"
        Me.Button6.UseVisualStyleBackColor = False
        '
        'PictureBox4
        '
        Me.PictureBox4.Image = CType(resources.GetObject("PictureBox4.Image"), System.Drawing.Image)
        Me.PictureBox4.Location = New System.Drawing.Point(481, -3)
        Me.PictureBox4.Name = "PictureBox4"
        Me.PictureBox4.Size = New System.Drawing.Size(71, 67)
        Me.PictureBox4.TabIndex = 14
        Me.PictureBox4.TabStop = False
        '
        'PictureBox3
        '
        Me.PictureBox3.BackColor = System.Drawing.Color.White
        Me.PictureBox3.Location = New System.Drawing.Point(432, 0)
        Me.PictureBox3.Name = "PictureBox3"
        Me.PictureBox3.Size = New System.Drawing.Size(88, 64)
        Me.PictureBox3.TabIndex = 13
        Me.PictureBox3.TabStop = False
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(0, 0)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(520, 72)
        Me.PictureBox1.TabIndex = 3
        Me.PictureBox1.TabStop = False
        '
        'PictureBox2
        '
        Me.PictureBox2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(5, 487)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(184, 32)
        Me.PictureBox2.TabIndex = 6
        Me.PictureBox2.TabStop = False
        '
        'Btncancel
        '
        Me.Btncancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Btncancel.BackColor = System.Drawing.SystemColors.ControlLight
        Me.Btncancel.Font = New System.Drawing.Font("Arial Unicode MS", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Btncancel.Location = New System.Drawing.Point(472, 521)
        Me.Btncancel.Name = "Btncancel"
        Me.Btncancel.Size = New System.Drawing.Size(72, 24)
        Me.Btncancel.TabIndex = 12
        Me.Btncancel.Text = "&Cancel"
        Me.Btncancel.UseVisualStyleBackColor = False
        '
        'gec
        '
        Me.gec.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.gec.Controls.Add(Me.Label11)
        Me.gec.Controls.Add(Me.Chkurgent)
        Me.gec.Controls.Add(Me.Cbehalf)
        Me.gec.Controls.Add(Me.bdetails)
        Me.gec.Location = New System.Drawing.Point(195, 487)
        Me.gec.Name = "gec"
        Me.gec.Size = New System.Drawing.Size(275, 61)
        Me.gec.TabIndex = 30
        Me.gec.TabStop = False
        Me.gec.Visible = False
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(102, 16)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(162, 13)
        Me.Label11.TabIndex = 1
        Me.Label11.Text = "Please select on behalf of author"
        Me.Label11.Visible = False
        '
        'Chkurgent
        '
        Me.Chkurgent.AutoSize = True
        Me.Chkurgent.Location = New System.Drawing.Point(3, 15)
        Me.Chkurgent.Name = "Chkurgent"
        Me.Chkurgent.Size = New System.Drawing.Size(70, 17)
        Me.Chkurgent.TabIndex = 30
        Me.Chkurgent.Text = "Important"
        Me.Chkurgent.UseVisualStyleBackColor = True
        '
        'Cbehalf
        '
        Me.Cbehalf.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Cbehalf.FormattingEnabled = True
        Me.Cbehalf.Location = New System.Drawing.Point(105, 33)
        Me.Cbehalf.Name = "Cbehalf"
        Me.Cbehalf.Size = New System.Drawing.Size(166, 21)
        Me.Cbehalf.TabIndex = 0
        Me.Cbehalf.Visible = False
        '
        'bdetails
        '
        Me.bdetails.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.bdetails.BackColor = System.Drawing.SystemColors.ControlLight
        Me.bdetails.Enabled = False
        Me.bdetails.Font = New System.Drawing.Font("Arial Unicode MS", 9.25!)
        Me.bdetails.Location = New System.Drawing.Point(6, 32)
        Me.bdetails.Name = "bdetails"
        Me.bdetails.Size = New System.Drawing.Size(72, 24)
        Me.bdetails.TabIndex = 29
        Me.bdetails.Text = "Details"
        Me.bdetails.UseVisualStyleBackColor = False
        '
        'bc_am_at_submit_frm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(550, 557)
        Me.Controls.Add(Me.gec)
        Me.Controls.Add(Me.gattach)
        Me.Controls.Add(Me.PictureBox4)
        Me.Controls.Add(Me.PictureBox3)
        Me.Controls.Add(Me.btnsubmit)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Btncancel)
        Me.Controls.Add(Me.PictureBox2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "bc_am_at_submit_frm"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Blue Curve  - submit"
        Me.TopMost = True
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel5.ResumeLayout(False)
        Me.Panel5.PerformLayout()
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.ttaxonomy.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.gattach.ResumeLayout(False)
        Me.gattach.PerformLayout()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gec.ResumeLayout(False)
        Me.gec.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub Btncancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btncancel.Click
        ok_selected = False
        Me.Hide()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnsubmit.Click
        Dim otrace As New bc_cs_activity_log("bc_am_at_submit_frm", "submit_click", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Me.Cursor = Cursors.WaitCursor

            Dim bc_cs_message As bc_cs_message
            



            REM validate first

            Dim found As Boolean = False

            REM if publish check if mandatory support doc attached
            If Me.cboworkflow.Text = "Publish" Then
                For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                    If bc_am_load_objects.obc_pub_types.pubtype(i).id = ldoc.pub_type_id Then
                        If bc_am_load_objects.obc_pub_types.pubtype(i).mandatory_default_support_doc = True Then
                            For j = 0 To ldoc.support_documents.Count - 1
                                If ldoc.support_documents(j).pub_type_id = bc_am_load_objects.obc_pub_types.pubtype(i).default_support_pub_type Then
                                    found = True
                                    Exit For
                                End If
                            Next
                            If found = False Then
                                Dim omsg As New bc_cs_message
                                For j = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                                    If bc_am_load_objects.obc_pub_types.pubtype(j).id = bc_am_load_objects.obc_pub_types.pubtype(i).default_support_pub_type Then
                                        Dim omsgg As New bc_cs_message("Blue Curve", "Support Document: " + bc_am_load_objects.obc_pub_types.pubtype(j).name + " not attached prior to Publish. Do you wish to Continue?", bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
                                        If omsgg.cancel_selected = True Then
                                            Exit Sub
                                        End If
                                        Exit For
                                    End If
                                Next
                            End If
                        End If
                    End If
                Next
            End If

            REM validate first FIL 5.3
            If Me.Cbehalf.Visible = True And Me.Cbehalf.SelectedIndex = -1 Then
                Dim om As New bc_cs_message("Blue Curve", "Behalf of Author must be selected!", bc_cs_message.MESSAGE)
                Exit Sub
            End If


            If Me.Cbehalf.Visible = True Then
                If edit_mode = False Then
                    ldoc.originating_author = behalf_of_authors_ids(Me.Cbehalf.SelectedIndex)
                End If
            End If



            ldoc.urgent_flag = False

            If Me.Chkurgent.Checked = True Then
                If ldoc.urgent_text = "" Then
                    bc_cs_message = New bc_cs_message("Blue Curve", "Document is important but no important text added please select details", bc_cs_message.MESSAGE)
                    Exit Sub
                End If
                ldoc.urgent_flag = True
            Else
                ldoc.urgent_text = ""
                ldoc.oec.Lists.Clear()
            End If

            REM ================


            If Me.attach_mode = True And Me.tattach.Text = "" Then
                bc_cs_message = New bc_cs_message("Blue Curve", "File Attachment Must Be Entered!", bc_cs_message.MESSAGE)
                Me.tattach.Select()
                Exit Sub
            End If
            If Me.attach_mode = True Then
                Me.attach_file = Me.tattach.Text
            End If
            If Me.txttitle.Text = "" Then
                bc_cs_message = New bc_cs_message("Blue Curve", "Title Must Be Entered!", bc_cs_message.MESSAGE)
                Me.txttitle.Select()
                Exit Sub
            End If
            If ldoc.authors.Count = 0 Then
                bc_cs_message = New bc_cs_message("Blue Curve", "At least One Author Must be selected", bc_cs_message.MESSAGE)
                Exit Sub
            End If

            REM copy values into Object model
            REM remove aposostrophies from the title and summary
            Dim s As New bc_cs_string_services(Me.txttitle.Text)
            ldoc.title = s.translate_apos
            s = New bc_cs_string_services(Me.txtsubtitle.Text)
            ldoc.sub_title = s.translate_apos
            If bc_am_load_objects.obc_pub_types.show_teaser_text = False Then
                s = New bc_cs_string_services(Me.tsummary.Text)
                ldoc.summary = s.translate_apos
            Else
                s = New bc_cs_string_services(Me.txtsummary.Text)
                ldoc.summary = s.translate_apos
                s = New bc_cs_string_services(Me.txtteaser.Rtf)
                ldoc.teaser_text = s.translate_apos
                ldoc.teaser_text.Substring(1, Len(ldoc.teaser_text) - 2)
            End If
            ldoc.pages = Me.lstpages.SelectedIndex + 1
            Dim ocomm As New bc_cs_activity_log("btnsubmit", "click", bc_cs_activity_codes.COMMENTARY, "Attempting to set date.")

            Try

                REM 21/06/2011 Steve Date Changes
                Dim servicedate As New bc_cs_date_services
                ldoc.doc_date = CDate(servicedate.DateToLocalShort(Me.txtdate.Value) + " " + Me.dh.Text + ":" + Me.ds.Text + ":00")
                'ldoc.doc_date = Me.txtdate.Value.ToString.Substring(0, 10) + " " + Me.dh.Text + ":" + Me.ds.Text + ":00"
                ldoc.doc_date = ldoc.doc_date.ToUniversalTime



            Catch ex As Exception
                ocomm = New bc_cs_activity_log("btnsubmit", "click", bc_cs_activity_codes.COMMENTARY, "Failed to set date method1: " + ex.Message)
                Try
                    ldoc.doc_date = Me.txtdate.Value
                Catch exx As Exception
                    ocomm = New bc_cs_activity_log("btnsubmit", "click", bc_cs_activity_codes.COMMENTARY, "Failed to set date method2 " + exx.Message)
                End Try
            End Try

            ldoc.action_Ids.Clear()
            Dim smandatory_fields As String
            REM load entities
            Dim entities As New bc_om_entities
            entities = entities.read_data_from_file(bc_cs_central_settings.local_template_path + bc_am_load_objects.ENTITIES_FILENAME)
            Dim pub_types As New bc_om_pub_types
            pub_types = pub_types.read_data_from_file(bc_cs_central_settings.local_template_path + bc_am_load_objects.PUB_TYPES_FILENAME)
            Dim file_attach As Boolean
            smandatory_fields = ldoc.check_mandatory_fields(entities, pub_types, file_attach)
            If smandatory_fields <> "" Then
                bc_cs_message = New bc_cs_message("Blue Curve", smandatory_fields, bc_cs_message.MESSAGE)
                Exit Sub
            End If
            REM only update if workflow has changed
            If cboworkflow.SelectedIndex > 0 Then
                REM hold original stage
                ldoc.original_stage = ldoc.stage
                ldoc.original_stage_name = ldoc.stage_name
                ldoc.stage = ldoc.workflow_stages.stages(Me.cboworkflow.SelectedIndex).stage_id
                ldoc.stage_name = ldoc.workflow_stages.stages(Me.cboworkflow.SelectedIndex).stage_name
                For i = 0 To ldoc.workflow_stages.stages(Me.cboworkflow.SelectedIndex).action_ids.count - 1
                    ldoc.action_Ids.Add(ldoc.workflow_stages.stages(Me.cboworkflow.SelectedIndex).action_ids(i))
                Next
                If ldoc.reject = True Then
                    ldoc.reset_reject = True
                End If
                ldoc.set_reject = False
                If ldoc.workflow_stages.stages(Me.cboworkflow.SelectedIndex).stage_type = "R" Then
                    ldoc.stage = 1
                    ldoc.stage_name = "Creating"
                    ldoc.reject_stage = ldoc.original_stage
                    ldoc.reject_stage_name = ldoc.original_stage_name
                    ldoc.set_reject = True
                    ldoc.reset_reject = False
                End If
            End If
            ok_selected = True
            If Me.btnlocal.Checked = True Then
                ldoc.connection_method = bc_cs_central_settings.LOCAL
            End If
            REM if document isnt being first submitted rem checkask if mailing list needs to be installed
            If ldoc.id = 0 Then
                ldoc.generate_mailing_list = True
            Else
                If ldoc.mailing_list_installed = True Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Do you want to regenerate distribution list", bc_cs_message.MESSAGE, True, False, "Yes", "No")
                    If omsg.cancel_selected = True Then
                        ldoc.generate_mailing_list = False
                    Else
                        ldoc.generate_mailing_list = True
                    End If
                End If
            End If


            Me.Hide()
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_at_submit_frm", "submit_click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.Cursor = Cursors.Default
            otrace = New bc_cs_activity_log("bc_am_at_submit_frm", "submit_click", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub


    Private Sub bc_am_at_submit_frm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim otrace As New bc_cs_activity_log("bc_am_at_submit_frm", "bc_am_at_submit_frm_load", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            ' If bc_am_load_objects.obc_pub_types.show_doc_scan = False Then
            Me.uxScannedList.Visible = False
            Me.uxScanned.Visible = False
            Me.uxMoveScannedEntry.Visible = False
            Me.ListBox1.Width = 230
            Me.ListBox2.Width = 230
            Dim dp As New System.Drawing.Point
            dp.X = 251
            dp.Y = Me.Button2.Location.Y
            Me.Button2.Location = dp
            dp = New System.Drawing.Point
            dp.X = 251
            dp.Y = Me.Button3.Location.Y
            Me.Button3.Location = dp

            dp = New System.Drawing.Point
            dp.X = 281
            dp.Y = Me.ListBox2.Location.Y
            Me.ListBox2.Location = dp


            dp = New System.Drawing.Point
            dp.X = 281
            dp.Y = Me.uxSelected.Location.Y
            Me.uxSelected.Location = dp

            dp = New System.Drawing.Point
            dp.X = 515
            dp.Y = Me.Button2.Location.Y
            Me.uxMoveUp.Location = dp

            dp = New System.Drawing.Point
            dp.X = 515
            dp.Y = Me.Button3.Location.Y
            Me.uxMoveDown.Location = dp


            ' End If

            Me.ToolStripMenuItem1.Image = Me.uxImages.Images(1)
            Me.DeleteToolStripMenuItem.Image = Me.uxImages.Images(2)
            loading = True
            Me.gattach.Visible = False
            Dim i, j As Integer
            ldoc.doc_date = ldoc.doc_date.ToLocalTime
            REM FIL JUNE 2013
            Cpub.Items.Clear()
            Me.cpts.Items.Clear()

            REM FIL JIRA 6835 set default sipport doc
            Dim default_support_pub_type_id As Long
            default_support_pub_type_id = 0
            For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                If bc_am_load_objects.obc_pub_types.pubtype(i).id = ldoc.pub_type_id Then
                    default_support_pub_type_id = bc_am_load_objects.obc_pub_types.pubtype(i).default_support_pub_type
                    Exit For
                End If
            Next

            For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                Cpub.Items.Add(bc_am_load_objects.obc_pub_types.pubtype(i).name)
                cpts.Items.Add(bc_am_load_objects.obc_pub_types.pubtype(i).name)
                REM FIL JIRA 6835 set default sipport doc
                If bc_am_load_objects.obc_pub_types.pubtype(i).id = default_support_pub_type_id Then
                    cpts.SelectedIndex = i
                End If

            Next

            Cpub.Text = Me.ldoc.pub_type_name

            REM Setup submit text
            If bc_cs_central_settings.show_summary_text = False Then
                Me.TabControl1.Controls.Item(0).Enabled = False
            End If
            If bc_cs_central_settings.show_teaser_text = False Then
                Me.TabControl1.TabPages.RemoveAt(1)
            End If
            If bc_cs_central_settings.summary_text_display_name <> "" Then
                Me.TabControl1.Controls.Item(0).Text = bc_cs_central_settings.summary_text_display_name
            End If
            If bc_cs_central_settings.teaser_text_display_name <> "" AndAlso Me.TabControl1.TabPages.Count > 1 Then
                Me.TabControl1.Controls.Item(1).Text = bc_cs_central_settings.teaser_text_display_name
            End If

            Me.tsummary.Visible = False
            Me.TabControl1.Visible = True
            Me.Lsummary.Visible = False

            If Me.register_only = True Then
                Me.Panel2.Visible = False
                Me.Panel3.Visible = False

            End If
            Me.ttaxonomy.TabPages(2).Text = bc_am_load_objects.obc_entities.disclosure_tab_name

            If Me.attach_mode = True Or Me.edit_mode = True Then
                Me.gattach.Visible = False
                Me.Panel3.Visible = False
                Me.btnlocal.Visible = False
                Me.btnnetwork.Visible = False
                Me.Panel2.Visible = False
                Me.cboworkflow.Visible = False

                If Me.attach_mode = True Then
                    Me.gattach.Visible = True
                    Me.btnsubmit.Text = "Import"
                Else
                    Me.btnsubmit.Text = "Update"
                End If
            End If

            loading = True
            For i = 0 To bc_am_load_objects.obc_prefs.language_strings.forms.Count - 1
                If bc_am_load_objects.obc_prefs.language_strings.forms(i).form_name = form_name Then
                    form_strings = bc_am_load_objects.obc_prefs.language_strings.forms(i)
                    Exit For
                End If
            Next
            class_id = bc_am_load_objects.obc_entities.get_entity_class_for_entity(ldoc.entity_id)
            original_stage = ldoc.stage_name

            Me.btnnetwork.Checked = True
            Me.Label1.Text = "Version: " + bc_cs_central_settings.version + " " + CStr(ldoc.connection_method)
            If ldoc.connection_method = "ado" Then
                Me.Label1.Text = Me.Label1.Text + "- " + bc_cs_central_settings.servername
            End If
            If ldoc.connection_method = "soap" Then
                Me.Label1.Text = Me.Label1.Text + "- " + bc_cs_central_settings.soap_server
            End If
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.LOCAL Then
                REM If ldoc.connection_method = "local" Then
                Me.btnnetwork.Enabled = False
                Me.btnlocal.Checked = True
                Me.cboworkflow.Enabled = False
            End If
            For i = 0 To bc_am_load_objects.obc_users.user.Count - 1
                With bc_am_load_objects.obc_users.user(i)
                    If Trim(.os_user_name) = Trim(bc_cs_central_settings.logged_on_user_name) Then
                        Me.Label2.Text = "User: " + .first_name + " " + .surname
                    End If
                End With
            Next
            Me.lstpages.Items.Clear()
            For i = 1 To 1000
                Me.lstpages.Items.Add(CStr(i))
            Next
            REM number of pages
            Me.lstpages.SelectedIndex = CInt(ldoc.pages) - 1
            REM title
            Me.txttitle.Text = ldoc.title
            REM sub title
            Me.txtsubtitle.Text = ldoc.sub_title
            REM summary
            If bc_am_load_objects.obc_pub_types.show_teaser_text = False Then
                Me.tsummary.Text = ldoc.summary
            Else
                Me.txtsummary.Text = ldoc.summary
                Me.txtteaser.Rtf = ldoc.teaser_text
            End If
            REM date
            Me.dh.Items.Clear()
            For i = 0 To 23
                If CStr(i).Length = 1 Then
                    Me.dh.Items.Add("0" + CStr(i))
                Else
                    Me.dh.Items.Add(CStr(i))
                End If
            Next
            For i = 0 To 59
                If CStr(i).Length = 1 Then
                    Me.ds.Items.Add("0" + CStr(i))
                Else
                    Me.ds.Items.Add(CStr(i))
                End If

            Next

            ' configurable ability to backdate document submission
            If bc_cs_central_settings.deny_backdated_submission Then
                If ldoc.doc_date < Now Then
                    ldoc.doc_date = Now
                End If
            End If

            If CStr(ldoc.doc_date) <> "00:00:00" Then
                Me.txtdate.Value = ldoc.doc_date
                Me.dh.Text = Format(ldoc.doc_date, "HH")
                Me.ds.Text = Format(ldoc.doc_date, "mm")
            End If
            If (Me.attach_mode = True Or register_only = True) And import_support_doc = False Then
                If Me.regular_report = False Then
                    ldoc.authors.Clear()
                End If
            End If
            REM start up tab strip
            load_authors()
            REM workflow
            load_workflow()
            REM load taxonomy
            load_taxonomy(ldoc.pub_type_id)


            If ldoc.do_not_show_taxonomy = True Then
                Me.cbotaxonomy.Visible = False
                Me.ltaxonomy.Visible = False
                For i = 3 To Me.ttaxonomy.TabPages.Count - 1
                    Me.ttaxonomy.TabPages.RemoveAt(3)
                Next
            Else
                For i = 0 To bc_am_load_objects.obc_users.user.Count - 1
                    If bc_am_load_objects.obc_users.user(i).id = bc_cs_central_settings.logged_on_user_id Then
                        If bc_am_load_objects.obc_users.user(i).no_display_taxonomy = True Then
                            Me.cbotaxonomy.Visible = False
                            Me.ltaxonomy.Visible = False
                            For j = 3 To Me.ttaxonomy.TabPages.Count - 1
                                Me.ttaxonomy.TabPages.RemoveAt(3)
                            Next
                        End If
                        Exit For

                    End If
                Next
            End If
            If bc_am_load_objects.obc_entities.disclosure_entities.Count = 0 Then
                Me.disclosures = False
                Me.Button5.Visible = False
                Me.ttaxonomy.TabPages.Remove(Disc)
            End If
            If ldoc.entity_id = 0 Then
                Me.lead_entity = False
                Me.Button4.Visible = False
                Me.ttaxonomy.TabPages.Remove(Entity)
            Else
                class_id = bc_am_load_objects.obc_entities.get_entity_class_for_entity(ldoc.entity_id)
                For i = 0 To bc_am_load_objects.obc_entities.entity.Count - 1
                    With bc_am_load_objects.obc_entities.entity(i)
                        If .class_id = class_id Then
                            Me.ttaxonomy.TabPages(1).Text = .class_name
                        End If
                    End With
                Next
            End If

            Me.ttaxonomy.TabPages(0).Select()
            load_support_docs()
            'For i = 0 To form_strings.form_labels.Count - 1
            '    If Trim(form_strings.form_labels(i).label_code) = "create.submit.title" Then
            '        Me.Label4.Text() = form_strings.form_labels(i).text
            '    End If
            '    If Trim(form_strings.form_labels(i).label_code) = "create.submit.subtitle" Then
            '        Me.Label9.Text() = form_strings.form_labels(i).text
            '    End If
            '    If Trim(form_strings.form_labels(i).label_code) = "create.submit.pages" Then
            '        Me.Label6.Text() = form_strings.form_labels(i).text
            '    End If
            '    If Trim(form_strings.form_labels(i).label_code) = "create.submit.date" Then
            '        Me.Label7.Text() = form_strings.form_labels(i).text
            '    End If
            '    If Trim(form_strings.form_labels(i).label_code) = "create.submit.summary" Then
            '        Me.Lsummary.Text() = form_strings.form_labels(i).text
            '    End If
            '    If Trim(form_strings.form_labels(i).label_code) = "create.submit.workflow" Then
            '        Me.Label8.Text() = form_strings.form_labels(i).text
            '    End If
            '    If Trim(form_strings.form_labels(i).label_code) = "create.submit.author" Then
            '        Me.Button1.Text() = form_strings.form_labels(i).text
            '    End If
            '    If Trim(form_strings.form_labels(i).label_code) = "create.submit.local" Then
            '        Me.btnlocal.Text() = form_strings.form_labels(i).text
            '    End If
            '    If Trim(form_strings.form_labels(i).label_code) = "create.submit.central" Then
            '        Me.btnnetwork.Text() = form_strings.form_labels(i).text
            '    End If
            '    If Trim(form_strings.form_labels(i).label_code) = "create.submit.submit" Then
            '        Me.btnsubmit.Text() = form_strings.form_labels(i).text
            '    End If
            '    If Trim(form_strings.form_labels(i).label_code) = "create.submit.cancel" Then
            '        Me.Btncancel.Text() = form_strings.form_labels(i).text
            '    End If

            'Next

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_at_submit_frm", "new", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            loading = False
            otrace = New bc_cs_activity_log("bc_am_at_submit_frm", "bc_am_at_submit_frm_load", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    Private Sub load_taxonomy(ByVal pub_type_id As Long)
        Dim otrace As New bc_cs_activity_log("bc_am_at_submit_frm", "load_taxonomy", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim i, j As Integer
            Dim cid As Long
            Dim tpage As TabPage
            cid = 0
            Me.cbotaxonomy.Items.Clear()
            Me.cbotaxonomy.Items.Add("please select...")
            REM get pub type id
            For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                If bc_am_load_objects.obc_pub_types.pubtype(i).id = pub_type_id Then
                    For j = 0 To bc_am_load_objects.obc_pub_types.pubtype(i).taxonomy.count - 1
                        Me.cbotaxonomy.Items.Add(bc_am_load_objects.obc_pub_types.pubtype(i).taxonomy(j))
                        Me.ltaxonomy.Visible = True
                        Me.cbotaxonomy.Visible = True
                        Me.ttaxonomy.Visible = True
                        tpage = New TabPage(bc_am_load_objects.obc_pub_types.pubtype(i).taxonomy(j))
                        Me.ttaxonomy.TabPages.Add(tpage)
                    Next
                    Exit For
                End If
            Next
            'REM lists
            Me.list_start_index = Me.cbotaxonomy.Items.Count - 1

            For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                If bc_am_load_objects.obc_pub_types.pubtype(i).id = ldoc.pub_type_id Then
                    For j = 0 To bc_am_load_objects.obc_pub_types.pubtype(i).lists.count - 1
                        Me.cbotaxonomy.Items.Add(bc_am_load_objects.obc_pub_types.pubtype(i).lists(j).name)
                        Me.ltaxonomy.Visible = True
                        Me.cbotaxonomy.Visible = True
                        Me.ttaxonomy.Visible = True
                        tpage = New TabPage(bc_am_load_objects.obc_pub_types.pubtype(i).lists(j).name)
                        Me.ttaxonomy.TabPages.Add(tpage)
                    Next
                    Exit For
                End If
            Next
            Me.ttaxonomy.SelectedIndex = -1
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_at_submit_frm", "load_taxonomy", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_am_at_submit_frm", "load_taxonomy", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    Private Sub load_authors()
        Dim otrace As New bc_cs_activity_log("bc_am_at_submit_frm", "load_authors", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim i, k, currentIndex As Integer

            If Me.ListBox1.SelectedIndex > 0 Then
                currentIndex = Me.ListBox1.SelectedIndex
            Else
                currentIndex = 0
            End If

            Me.ListBox1.Items.Clear()
            Me.ListBox2.Items.Clear()
            REM users

            For i = 0 To bc_am_load_objects.obc_users.user.Count - 1
                Me.ListBox1.Items.Add(bc_am_load_objects.obc_users.user(i).first_name + " " + bc_am_load_objects.obc_users.user(i).surname)
            Next

            REM Steve Wooderson 14/03/2014 Fil IMSR-9433
            If Me.ListBox1.Items.Count - 1 < currentIndex Then
                currentIndex = 0
            End If
            Me.ListBox1.SelectedIndex = currentIndex

            For k = 0 To ldoc.authors.Count - 1
                For i = 0 To bc_am_load_objects.obc_users.user.Count - 1
                    If ldoc.authors(k).id = bc_am_load_objects.obc_users.user(i).id() Then
                        Me.ListBox2.Items.Add(bc_am_load_objects.obc_users.user(i).first_name + " " + bc_am_load_objects.obc_users.user(i).surname)
                    End If
                Next
            Next

            'For i = 0 To bc_am_load_objects.obc_users.user.Count - 1
            '    Me.ListBox1.Items.Add(bc_am_load_objects.obc_users.user(i).first_name + " " + bc_am_load_objects.obc_users.user(i).surname)
            '    For k = 0 To ldoc.authors.Count - 1
            '        If ldoc.authors(k).id = bc_am_load_objects.obc_users.user(i).id() Then
            '            Me.ListBox2.Items.Add(bc_am_load_objects.obc_users.user(i).first_name + " " + bc_am_load_objects.obc_users.user(i).surname)
            '        End If
            '    Next
            'Next


            Dim l As Integer
            If ldoc.entity_id = 0 Then
                Me.Button4.Visible = False
            Else
                For l = 0 To bc_am_load_objects.obc_entities.entity.Count - 1
                    If bc_am_load_objects.obc_entities.entity(l).id = ldoc.entity_id Then
                        Me.Button4.Text = bc_am_load_objects.obc_entities.entity(l).class_name + "s"
                    End If
                Next
            End If

            Me.uxMoveUp.Enabled = False
            Me.uxMoveDown.Enabled = False

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_at_submit_frm", "load_authors", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_am_at_submit_frm", "load_authors", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try


    End Sub
    Private Sub load_lead_entity(ByVal for_user As Boolean)
        Dim otrace As New bc_cs_activity_log("bc_am_at_submit_frm", "load_lead_entity", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            REM default entity
            Dim k As Integer

            Me.ListBox1.BeginUpdate()
            Me.ListBox2.BeginUpdate()

            Me.ListBox1.Items.Clear()
            Me.ListBox2.Items.Clear()
            If ldoc.entity_id > 0 Then
                Dim i As Integer
                Dim class_id As Long

                'class_id = bc_am_load_objects.obc_entities.get_entity_class_for_entity(ldoc.entity_id)
                'For i = 0 To bc_am_load_objects.obc_entities.entity.Count - 1
                '    With bc_am_load_objects.obc_entities.entity(i)
                Dim entity_list As New bc_om_entities
                If bc_cs_central_settings.alt_entity_for_submit = True Then
                    entity_list = bc_am_load_objects.obc_entities.alternate_entity_list
                Else
                    entity_list = bc_am_load_objects.obc_entities
                End If
                class_id = entity_list.get_entity_class_for_entity(ldoc.entity_id)


                'For i = 0 To entity_list.entity.Count - 1
                '    With entity_list.entity(i)

                '        If .class_id = class_id Then
                '            For k = 0 To ldoc.taxonomy.Count - 1
                '                If ldoc.taxonomy(k).entity_id = .id Then
                '                    Me.ListBox2.Items.Add(.name)
                '                End If
                '            Next
                '            Me.ListBox1.Items.Add(.name)
                '            Me.ttaxonomy.TabPages(1).Text = .class_name
                '        End If
                '    End With
                'Next

                REM Steve Wooderson Drexel 27/01/2014 Show prefs only
                If for_user = False Then
                    Me.Btntoggle.Text = "My Subjects"
                    For i = 0 To entity_list.entity.Count - 1
                        With entity_list.entity(i)
                            If .class_id = class_id Then
                                Me.ListBox1.Items.Add(.name)
                                If Me.ttaxonomy.TabPages(1).Text = "" Then
                                    Me.ttaxonomy.TabPages(1).Text = .class_name
                                End If
                            End If
                        End With
                    Next
                Else
                    Me.Btntoggle.Text = "All Subjects"
                    For i = 0 To bc_am_load_objects.obc_prefs.pref.Count - 1
                        With bc_am_load_objects.obc_prefs.pref(i)
                            If .class_id = class_id Then
                                Me.ListBox1.Items.Add(.entity_name)
                                If Me.ttaxonomy.TabPages(1).Text = "" Then
                                    Me.ttaxonomy.TabPages(1).Text = .class_name
                                End If
                            End If
                        End With
                    Next
                End If

                For i = 0 To ldoc.taxonomy.Count - 1
                    For k = 0 To entity_list.entity.Count - 1
                        If entity_list.entity(k).class_id = class_id Then
                            If ldoc.taxonomy(i).entity_id = entity_list.entity(k).id Then
                                Me.ListBox2.Items.Add(entity_list.entity(k).name)
                                Exit For
                            End If
                        End If
                    Next
                Next


            End If
            Me.ttaxonomy.TabPages(0).Select()

            Me.uxMoveUp.Enabled = False
            Me.uxMoveDown.Enabled = False

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_at_submit_frm", "load_lead_entity", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.ListBox1.EndUpdate()
            Me.ListBox2.EndUpdate()
            otrace = New bc_cs_activity_log("bc_am_at_submit_frm", "load_lead_entity", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Private Sub load_disclosures(ByVal for_user As Boolean)
        Dim otrace As New bc_cs_activity_log("bc_am_at_submit_frm", "load_disclosures", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            REM default entity
            Dim k As Integer
            Me.ListBox1.Items.Clear()
            Me.ListBox2.Items.Clear()
            Prev_entities.Clear()

            REM Steve Wooderson Drexel 27/01/2014 Show prefs only
            If Me.cbotaxonomy.Items.Count > 0 Then Me.cbotaxonomy.SelectedIndex = 0
            For i = 0 To bc_am_load_objects.obc_entities.disclosure_entities.Count - 1
                With bc_am_load_objects.obc_entities.disclosure_entities(i)
                    For k = 0 To ldoc.disclosures.Count - 1
                        If ldoc.disclosures(k).entity_id = .id Then
                            Me.ListBox2.Items.Add(.name)
                        End If
                    Next
                End With
            Next

            REM Steve Wooderson Drexel 27/01/2014 Show prefs only
            If for_user = False Then
                Dim i As Integer
                Me.Btntoggle.Text = "My Subjects"
                For i = 0 To bc_am_load_objects.obc_entities.disclosure_entities.Count - 1
                    With bc_am_load_objects.obc_entities.disclosure_entities(i)
                        Me.ListBox1.Items.Add(.name)
                        Prev_entities.Add(bc_am_load_objects.obc_entities.disclosure_entities(i))
                    End With
                Next
            Else
                Dim i, M As Integer
                Me.Btntoggle.Text = "All Subjects"
                For i = 0 To bc_am_load_objects.obc_entities.disclosure_entities.Count - 1
                    For M = 0 To bc_am_load_objects.obc_prefs.pref.Count - 1
                        If bc_am_load_objects.obc_prefs.pref(M).entity_id() = bc_am_load_objects.obc_entities.disclosure_entities(i).id And bc_am_load_objects.obc_prefs.pref(M).class_id = bc_am_load_objects.obc_entities.disclosure_entities(i).class_id Then
                            With bc_am_load_objects.obc_entities.disclosure_entities(i)
                                Me.ListBox1.Items.Add(.name)
                                Prev_entities.Add(bc_am_load_objects.obc_entities.disclosure_entities(i))
                            End With
                        End If
                    Next
                Next
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_at_submit_frm", "load_disclosures", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_am_at_submit_frm", "load_disclosures", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        add_item(sender, e)
    End Sub

    Private Sub add_item(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim i As Integer, j As Integer, idx As Integer, selectedIndex As Integer
        Dim otrace As New bc_cs_activity_log("bc_am_at_submit_frm", "button2_click", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim m As Integer
        Dim found As Boolean

        Dim entity_list As New bc_om_entities
        If bc_cs_central_settings.alt_entity_for_submit = True Then
            entity_list = bc_am_load_objects.obc_entities.alternate_entity_list
        Else
            entity_list = bc_am_load_objects.obc_entities
        End If

        Try
            If Me.ListBox1.SelectedIndex = -1 Then
                Exit Sub
            End If
            Select Case taxonomy
                REM authors
                Case 0
                    For Each selectedIndex In Me.ListBox1.SelectedIndices
                        Dim authorAlreadyAssociatedWithDocument As Boolean = False
                        REM check not already entered
                        For i = 0 To ldoc.authors.Count - 1
                            If ldoc.authors(i).id = bc_am_load_objects.obc_users.user(selectedIndex).id Then
                                authorAlreadyAssociatedWithDocument = True
                                Exit For
                            End If
                        Next
                        If Not authorAlreadyAssociatedWithDocument Then ldoc.authors.Add(bc_am_load_objects.obc_users.user(selectedIndex))
                    Next
                    'Rama Comments: check if the authors count is >1 or euqal to 1 and
                    'if the count is one then make this newly added author as originator.
                    'If ldoc.authors.Count = 1 Then
                    'ldoc.originating_author = bc_am_load_objects.obc_users.user(Me.ListBox1.SelectedIndex).id
                    'End If
                    load_authors()
                    REM lead entity
                Case 1
                    For Each selectedIndex In Me.ListBox1.SelectedIndices
                        idx = 0
                        class_id = bc_am_load_objects.obc_entities.get_entity_class_for_entity(ldoc.entity_id)
                        For i = 0 To entity_list.entity.Count - 1
                            With entity_list.entity(i)
                                If .class_id = class_id Then
                                    If Me.ListBox1.Items(selectedIndex) = .name Then
                                        'If selectedIndex = idx Then
                                        Dim entityAlreadyAssociatedWithDoc As Boolean = False
                                        For j = 0 To ldoc.taxonomy.Count - 1
                                            If ldoc.taxonomy(j).entity_id = .id Then
                                                entityAlreadyAssociatedWithDoc = True
                                                Exit For
                                            End If
                                        Next
                                        If Not entityAlreadyAssociatedWithDoc Then
                                            Dim default_entity As New bc_om_taxonomy
                                            default_entity.entity_id = .id
                                            default_entity.name = .name
                                            ldoc.taxonomy.Add(default_entity)
                                            Me.ListBox2.Items.Add(.name)
                                        End If
                                    End If
                                    idx = idx + 1
                                End If
                            End With
                        Next

                    Next
                Case 2
                    'disclosures()
                    REM check not already entered
                    For Each selectedIndex In Me.ListBox1.SelectedIndices
                        Dim disclosureAlreadyAssociatedWithDocument As Boolean = False
                        For i = 0 To ldoc.disclosures.Count - 1
                            REM If ldoc.disclosures(i).entity_id = bc_am_load_objects.obc_entities.disclosure_entities(selectedIndex).id Then
                            If ldoc.disclosures(i).entity_id = Prev_entities(selectedIndex).id Then
                                disclosureAlreadyAssociatedWithDocument = True
                                Exit For
                            End If
                        Next
                        If Not disclosureAlreadyAssociatedWithDocument Then
                            Dim default_entity As New bc_om_taxonomy
                            REM default_entity.entity_id = bc_am_load_objects.obc_entities.disclosure_entities(selectedIndex).id
                            default_entity.entity_id = Prev_entities(selectedIndex).id
                            ldoc.disclosures.Add(default_entity)
                            REM Me.ListBox2.Items.Add(bc_am_load_objects.obc_entities.disclosure_entities(selectedIndex).name)
                            Me.ListBox2.Items.Add(Prev_entities(selectedIndex).name)
                        End If
                    Next

                    REM Steve Wooderson Drexel 27/01/2014 Show prefs only
                    load_disclosures(btoggle)

                    REM other taxonomy
                Case 4
                    Dim has_filter As Boolean
                    For Each selectedIndex In Me.ListBox1.SelectedIndices
                        idx = 0
                        'class_id = bc_am_load_objects.obc_entities.get_entity_class_for_entity(ldoc.entity_id)
                        For i = 0 To entity_list.entity.Count - 1
                            has_filter = False
                            With entity_list.entity(i)
                                If .class_id = tclass_id Then
                                    found = False
                                    For m = 0 To ldoc.entity_classify_filters.filters.Count - 1
                                        If ldoc.entity_classify_filters.filters(m).class_id = .class_id Then
                                            has_filter = True
                                            Exit For
                                        End If
                                    Next
                                    For m = 0 To ldoc.entity_classify_filters.filters.Count - 1
                                        If ldoc.entity_classify_filters.filters(m).entity_id = .id Then
                                            idx = idx + 1
                                            Exit For
                                            found = True
                                        End If
                                    Next
                                    If found = False And has_filter = False Then
                                        idx = idx + 1
                                    End If
                                    If selectedIndex = idx - 1 Then
                                        Dim entityAlreadyAssociatedWithDoc As Boolean = False
                                        For j = 0 To ldoc.taxonomy.Count - 1
                                            If ldoc.taxonomy(j).entity_id = .id Then
                                                entityAlreadyAssociatedWithDoc = True
                                                Exit For
                                            End If
                                        Next
                                        If Not entityAlreadyAssociatedWithDoc Then
                                            Dim ot As New bc_om_taxonomy
                                            ot.entity_id = .id
                                            ldoc.taxonomy.Add(ot)
                                            Me.ListBox2.Items.Add(.name)
                                        End If
                                        idx = idx + 1
                                    End If
                                    'idx = idx + 1
                                End If

                            End With
                        Next
                    Next
                    REM list items
                Case 5
                    found = False
                    Dim mm As Integer
                    Dim iddx As Integer
                    iddx = Me.ListBox1.SelectedIndex
                    For mm = 0 To Me.ListBox2.Items.Count - 1
                        If Me.ListBox2.Items(mm) = Me.ListBox1.Items(Me.ListBox1.SelectedIndex) Then
                            found = True
                        End If
                    Next
                    If found = False Then
                        Dim olm As New bc_om_list_item
                        Dim iid As Integer
                        iid = Me.cbotaxonomy.SelectedIndex - Me.list_start_index - 1
                        olm.list_id = bc_am_load_objects.obc_pub_types.pubtype(i).lists(iid).id
                        olm.item_id = bc_am_load_objects.obc_pub_types.pubtype(i).lists(iid).items(iddx).item_id
                        ldoc.list_items.list_items.Add(olm)
                        Me.ListBox2.Items.Add(bc_am_load_objects.obc_pub_types.pubtype(i).lists(iid).items(iddx).name)
                    End If
            End Select
            Me.Button2.Enabled = False
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_at_submit_frm", "button2_click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_am_at_submit_frm", "button2_click", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        rem_item(sender, e)
    End Sub

    Private Sub rem_item(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim otrace As New bc_cs_activity_log("bc_am_at_submit_frm", "button3_click", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim i As Integer, j As Integer, selectedIndex As Integer

            If Me.ListBox2.SelectedIndex = -1 Then Exit Sub

            Select Case taxonomy
                REM authors
                Case 0
                    For Each selectedIndex In Me.ListBox2.SelectedIndices
                        REM check not already entered
                        For j = 0 To bc_am_load_objects.obc_users.user.Count - 1
                            If bc_am_load_objects.obc_users.user(j).first_name + " " + bc_am_load_objects.obc_users.user(j).surname = Me.ListBox2.Items(selectedIndex) Then
                                For i = 0 To ldoc.authors.Count - 1
                                    If bc_am_load_objects.obc_users.user(j).id = ldoc.authors(i).id Then
                                        ldoc.authors.RemoveAt(i)
                                        Exit For
                                    End If
                                Next
                            End If
                        Next
                    Next
                    load_authors()

                Case 1
                    For Each selectedIndex In Me.ListBox2.SelectedIndices
                        For j = 0 To bc_am_load_objects.obc_entities.entity.Count - 1
                            If bc_am_load_objects.obc_entities.entity(j).name = Me.ListBox2.Items(selectedIndex) Then
                                For i = 0 To ldoc.taxonomy.Count - 1
                                    With ldoc.taxonomy(i)
                                        If .entity_id = bc_am_load_objects.obc_entities.entity(j).id Then
                                            ldoc.taxonomy.RemoveAt(i)
                                            Exit For
                                        End If
                                    End With
                                Next
                            End If
                        Next
                    Next

                    REM Steve Wooderson Drexel 27/01/2014 Show prefs only
                    load_lead_entity(btoggle)

                Case 2
                    For Each selectedIndex In Me.ListBox2.SelectedIndices
                        For j = 0 To bc_am_load_objects.obc_entities.disclosure_entities.Count - 1
                            If bc_am_load_objects.obc_entities.disclosure_entities(j).name = Me.ListBox2.Items(selectedIndex) Then
                                For i = 0 To ldoc.disclosures.Count - 1
                                    With ldoc.disclosures(i)
                                        If .entity_id = bc_am_load_objects.obc_entities.disclosure_entities(j).id Then
                                            ldoc.disclosures.RemoveAt(i)
                                            Exit For
                                        End If
                                    End With
                                Next
                            End If
                        Next
                    Next
                    REM Steve Wooderson Drexel 27/01/2014 Show prefs only
                    load_disclosures(btoggle)

                Case 4
                    REM other(taxonomy)
                    For Each selectedIndex In Me.ListBox2.SelectedIndices
                        For j = 0 To bc_am_load_objects.obc_entities.entity.Count - 1
                            If bc_am_load_objects.obc_entities.entity(j).name = Me.ListBox2.Items(selectedIndex) Then
                                For i = 0 To ldoc.taxonomy.Count - 1
                                    With ldoc.taxonomy(i)
                                        If .entity_id = bc_am_load_objects.obc_entities.entity(j).id Then
                                            ldoc.taxonomy.RemoveAt(i)
                                            Exit For
                                        End If
                                    End With
                                Next
                            End If
                        Next
                    Next
                    cbotaxonomy_SelectedIndexChanged(sender, e)
                Case 5
                    Dim k, m As Integer
                    Dim iid As Integer
                    For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                        If bc_am_load_objects.obc_pub_types.pubtype(i).id = ldoc.pub_type_id Then
                            iid = Me.cbotaxonomy.SelectedIndex - Me.list_start_index - 1
                            For k = 0 To bc_am_load_objects.obc_pub_types.pubtype(i).lists(iid).items.count - 1
                                If bc_am_load_objects.obc_pub_types.pubtype(i).lists(iid).items(k).name = Me.ListBox2.Items(Me.ListBox2.SelectedIndex) Then
                                    For m = 0 To ldoc.list_items.list_items.Count - 1
                                        If ldoc.list_items.list_items(m).list_Id = bc_am_load_objects.obc_pub_types.pubtype(i).lists(iid).id And ldoc.list_items.list_items(m).item_id = bc_am_load_objects.obc_pub_types.pubtype(i).lists(iid).items(k).item_id Then
                                            ldoc.list_items.list_items.RemoveAt(m)
                                            Me.ListBox2.Items.RemoveAt(Me.ListBox2.SelectedIndex)
                                            Exit For
                                        End If
                                    Next
                                    Exit For
                                End If
                            Next
                            Exit For
                        End If
                    Next


            End Select
            Me.Button3.Enabled = False
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_at_submit_frm", "button3_click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_am_at_submit_frm", "button3_click", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub

    Private Sub ListBox1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListBox1.DoubleClick
        add_item(sender, e)
    End Sub
    Private Sub ListBox2_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListBox1.DoubleClick
        add_item(sender, e)
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListBox1.SelectedIndexChanged
        Me.Button2.Enabled = True
    End Sub


    Private Sub ListBox2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListBox2.SelectedIndexChanged

        Me.uxMoveUp.Enabled = False
        Me.uxMoveDown.Enabled = False

        If taxonomy = 0 Or taxonomy = 1 Or taxonomy = 4 Then
            If Me.ListBox2.SelectedIndex > -1 Then
                If Me.ListBox2.SelectedIndex > 0 Then
                    Me.uxMoveUp.Enabled = True
                End If
                If Me.ListBox2.SelectedIndex < Me.ListBox2.Items.Count - 1 Then
                    Me.uxMoveDown.Enabled = True
                End If
            End If
        End If

        Me.Button3.Enabled = True
    End Sub
    Private Sub Button1_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        load_authors()
        taxonomy = 0
        Me.cbotaxonomy.SelectedIndex = 0
        Me.ttaxonomy.SelectedIndex = -1
        Me.Button1.FlatStyle = FlatStyle.Flat

        Me.Button4.FlatStyle = FlatStyle.Standard
        Me.Button5.FlatStyle = FlatStyle.Standard
    End Sub
    Private Sub Button4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button4.Click

        REM Steve Wooderson Drexel 27/01/2014 Show prefs only
        load_lead_entity(btoggle)

        taxonomy = 1
        Me.ttaxonomy.SelectedIndex = -1
        Me.cbotaxonomy.SelectedIndex = 0
        Me.Button4.FlatStyle = FlatStyle.Flat
        Me.Button1.FlatStyle = FlatStyle.Standard
        Me.Button5.FlatStyle = FlatStyle.Standard
    End Sub
    Private Sub load_workflow()
        Dim i As Integer

        Me.cboworkflow.Items.Clear()
        REM workflow
        If Trim(ldoc.user_role) = "Research Associate" And ldoc.id = 0 Then
            Me.cboworkflow.Items.Add(ldoc.workflow_stages.stages(0).stage_name)
            Me.cboworkflow.Items.Add(ldoc.user_role)
            ldoc.workflow_stages.stages(1).stage_id = 26
            ldoc.workflow_stages.stages(1).stage_name = ldoc.user_role

            ldoc.workflow_stages.stages(1).action_ids.clear()
            ldoc.workflow_stages.stages(1).num_approvers = 0
        Else
            Try
                If ldoc.reject = True Then
                    Me.cboworkflow.Items.Add(ldoc.workflow_stages.stages(0).stage_name)
                    Me.cboworkflow.Items.Add(ldoc.reject_stage_name)
                    ldoc.workflow_stages.stages(1).stage_id = ldoc.reject_stage
                    ldoc.workflow_stages.stages(1).stage_name = ldoc.reject_stage_name

                    ldoc.workflow_stages.stages(1).action_ids.clear()
                    ldoc.workflow_stages.stages(1).num_approvers = 0

                Else
                    For i = 0 To ldoc.workflow_stages.stages.Count - 1
                        If ldoc.workflow_stages.stages(i).stage_type = "F" And ldoc.stage = 1 Then
                            REM fast track
                            Me.cboworkflow.Items.Add("Fast Track")
                        Else
                            REM stage routes
                            Me.cboworkflow.Items.Add(ldoc.workflow_stages.stages(i).stage_name)
                        End If
                    Next
                End If
            Catch
                Me.cboworkflow.Items.Clear()

                For i = 0 To ldoc.workflow_stages.stages.Count - 1
                    REM stage routes
                    Me.cboworkflow.Items.Add(ldoc.workflow_stages.stages(i).stage_name)
                Next
            End Try
        End If
        REM first stage is current stage
        If Me.cboworkflow.Visible = True Then
            If ldoc.workflow_stages.stages.Count > 0 Then
                Me.cboworkflow.SelectedIndex = 0
            Else
                Dim omessage As New bc_cs_message("Blue Curve - create", "No workflow configured for Publication", bc_cs_message.MESSAGE)
            End If
        End If
    End Sub

    Private Sub btnlocal_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnlocal.CheckedChanged
        If btnlocal.Checked = True Then
            Me.cboworkflow.Enabled = False
            Me.cboworkflow.SelectedItem = original_stage
            ldoc.support_documents.Clear()
            load_support_docs()
            Me.bsnew.Enabled = False

        Else
            Me.bsnew.Enabled = True
            Me.cboworkflow.Enabled = True

        End If
    End Sub



    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        REM Steve Wooderson Drexel 27/01/2014 Show prefs only
        load_disclosures(btoggle)
        Me.ttaxonomy.SelectedIndex = -1
        taxonomy = 2
        Me.Button5.FlatStyle = FlatStyle.Flat
        Me.Button1.FlatStyle = FlatStyle.Standard
        Me.Button4.FlatStyle = FlatStyle.Standard

    End Sub

    Private Sub cbotaxonomy_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbotaxonomy.SelectedIndexChanged
        change_taxonomy(Me.cbotaxonomy.SelectedIndex, btoggle)
    End Sub

    Private Sub change_taxonomy(ByVal idx As Integer, ByVal for_user As Boolean)

        Dim i, k, m As Integer
        Dim filter_found As Boolean
        Me.tclass_id = 0
        Dim iid As Integer
        If Me.cbotaxonomy.SelectedIndex > Me.list_start_index Then
            taxonomy = 5
            Me.ListBox1.Items.Clear()
            Me.ListBox2.Items.Clear()
            REM lists not classes
            For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count
                If bc_am_load_objects.obc_pub_types.pubtype(i).id = ldoc.pub_type_id Then
                    iid = Me.cbotaxonomy.SelectedIndex - Me.list_start_index - 1
                    For k = 0 To bc_am_load_objects.obc_pub_types.pubtype(i).lists(iid).items.count - 1
                        Me.ListBox1.Items.Add(bc_am_load_objects.obc_pub_types.pubtype(i).lists(iid).items(k).name)
                        REM selectreditems
                        For m = 0 To ldoc.list_items.list_items.Count - 1
                            If ldoc.list_items.list_items(m).list_Id = bc_am_load_objects.obc_pub_types.pubtype(i).lists(iid).id And ldoc.list_items.list_items(m).item_id = bc_am_load_objects.obc_pub_types.pubtype(i).lists(iid).items(k).item_id Then
                                Me.ListBox2.Items.Add(bc_am_load_objects.obc_pub_types.pubtype(i).lists(iid).items(k).name)
                                Exit For
                            End If
                        Next
                    Next
                    Exit For
                End If

            Next
        Else
            If Me.cbotaxonomy.SelectedIndex > 0 Then
                taxonomy = 4
                Me.ListBox1.Items.Clear()
                Me.ListBox2.Items.Clear()
                filter_found = False
                Dim entity_list As New bc_om_entities
                If bc_cs_central_settings.alt_entity_for_submit = True Then
                    entity_list = bc_am_load_objects.obc_entities.alternate_entity_list
                Else
                    entity_list = bc_am_load_objects.obc_entities
                End If
                For i = 0 To entity_list.entity.Count - 1
                    If entity_list.entity(i).class_name = Me.cbotaxonomy.Text Then

                        REM Steve Wooderson Drexel 27/01/2014 Show prefs only
                        If for_user = True Then
                            Me.Btntoggle.Text = "All Subjects"
                            For n = 0 To bc_am_load_objects.obc_prefs.pref.Count - 1
                                If bc_am_load_objects.obc_prefs.pref(n).entity_id() = entity_list.entity(i).id Then

                                    REM check it is not in exclude list
                                    For m = 0 To ldoc.entity_classify_filters.filters.Count - 1
                                        If ldoc.entity_classify_filters.filters(m).class_id = entity_list.entity(i).class_id Then
                                            filter_found = True
                                            If entity_list.entity(i).id = ldoc.entity_classify_filters.filters(m).entity_id Then
                                                Me.ListBox1.Items.Add(entity_list.entity(i).name)
                                                Exit For
                                            End If
                                        End If
                                    Next

                                    If filter_found = False Then
                                        Me.ListBox1.Items.Add(entity_list.entity(i).name)
                                    End If

                                End If
                            Next
                        Else
                            Me.Btntoggle.Text = "My Subjects"
                            REM check it is not in exclude list
                            For m = 0 To ldoc.entity_classify_filters.filters.Count - 1
                                If ldoc.entity_classify_filters.filters(m).class_id = entity_list.entity(i).class_id Then
                                    filter_found = True
                                    If entity_list.entity(i).id = ldoc.entity_classify_filters.filters(m).entity_id Then
                                        Me.ListBox1.Items.Add(entity_list.entity(i).name)
                                        Exit For
                                    End If
                                End If
                            Next

                            If filter_found = False Then
                                Me.ListBox1.Items.Add(entity_list.entity(i).name)
                            End If

                        End If

                        Me.tclass_id = entity_list.entity(i).class_id
                        'For k = 0 To ldoc.taxonomy.Count - 1
                        '    If ldoc.taxonomy(k).entity_id = entity_list.entity(i).id Then
                        '        Me.ListBox2.Items.Add(entity_list.entity(i).name)
                        '    End If
                        'Next
                    End If
                Next

                For k = 0 To ldoc.taxonomy.Count - 1
                    For i = 0 To entity_list.entity.Count - 1
                        REM Steve Wooderson Drexel 27/01/2014 Show prefs only
                        If ldoc.taxonomy(k).entity_id = entity_list.entity(i).id _
                           And entity_list.entity(i).class_name = Me.cbotaxonomy.Text Then
                            Me.ListBox2.Items.Add(entity_list.entity(i).name)
                            Exit For
                        End If
                    Next
                Next

            End If
        End If
    End Sub


    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        Dim odialog As New OpenFileDialog
        Dim extensionsize As Integer
        odialog.Title = "Import Document"
        odialog.ShowDialog()
        Me.tattach.Text = odialog.FileName

        If Len(Me.txttitle.Text) = 0 Then
            extensionsize = (Len(odialog.SafeFileName) - (InStrRev(odialog.SafeFileName, ".") - 1))
            Me.txttitle.Text = Microsoft.VisualBasic.Left(odialog.SafeFileName, Len(odialog.SafeFileName) - extensionsize)
        End If

    End Sub




    Private Sub ttaxonomy_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ttaxonomy.SelectedIndexChanged
        Try
            Dim ic As Integer
            btoggle = False
            ic = 3
            If Me.lead_entity = False And Me.disclosures = False Then
                ic = 1
            ElseIf Me.lead_entity = False Or Me.disclosures = False Then
                ic = 2
            End If
            If Me.ttaxonomy.SelectedIndex >= ic Then

                REM Steve Wooderson Drexel 27/01/2014 Show prefs only
                Select Case True
                    Case bc_cs_central_settings.my_entities_default_submit
                        Me.Btntoggle.Visible = True
                        btoggle = True
                    Case Not bc_cs_central_settings.show_all_entities_submit
                        btoggle = True
                        Me.Btntoggle.Visible = False
                    Case Else
                        Me.Btntoggle.Visible = True
                        btoggle = False
                End Select

                If Me.lead_entity = False And Me.disclosures = False Then
                    Me.cbotaxonomy.SelectedIndex = Me.ttaxonomy.SelectedIndex
                ElseIf Me.lead_entity = False Or Me.disclosures = False Then
                    Me.cbotaxonomy.SelectedIndex = Me.ttaxonomy.SelectedIndex - 1
                Else
                    Me.cbotaxonomy.SelectedIndex = Me.ttaxonomy.SelectedIndex - 2
                End If
            End If
            If Me.ttaxonomy.SelectedIndex = 0 Then
                Me.Btntoggle.Visible = False
                load_authors()
                taxonomy = 0
                Me.cbotaxonomy.SelectedIndex = 0
            End If
            If Me.ttaxonomy.SelectedIndex = 1 And Me.lead_entity = True Then

                REM Steve Wooderson Drexel 27/01/2014 Show prefs only
                Select Case True
                    Case bc_cs_central_settings.my_entities_default_submit
                        Me.Btntoggle.Visible = True
                        btoggle = True
                        load_lead_entity(True)
                    Case Not bc_cs_central_settings.show_all_entities_submit
                        Me.Btntoggle.Visible = False
                        btoggle = True
                        load_lead_entity(True)
                    Case Else
                        Me.Btntoggle.Visible = True
                        btoggle = False
                        load_lead_entity(False)
                End Select

                taxonomy = 1
                Me.cbotaxonomy.SelectedIndex = 0
            ElseIf Me.ttaxonomy.SelectedIndex = 1 And Me.lead_entity = False And Me.disclosures = True Then
                REM Steve Wooderson Drexel 27/01/2014 Show prefs only
                Select Case True
                    Case bc_cs_central_settings.my_entities_default_submit
                        Me.Btntoggle.Visible = True
                        btoggle = True
                        load_disclosures(True)
                    Case Not bc_cs_central_settings.show_all_entities_submit
                        Me.Btntoggle.Visible = False
                        btoggle = True
                        load_disclosures(True)
                    Case Else
                        Me.Btntoggle.Visible = True
                        btoggle = False
                        load_disclosures(False)
                End Select
                taxonomy = 2
            End If
            If Me.ttaxonomy.SelectedIndex = 2 And Me.lead_entity = True And Me.disclosures = True Then
                REM Steve Wooderson Drexel 27/01/2014 Show prefs only
                Select Case True
                    Case bc_cs_central_settings.my_entities_default_submit
                        Me.Btntoggle.Visible = True
                        btoggle = True
                        load_disclosures(True)
                    Case Not bc_cs_central_settings.show_all_entities_submit
                        Me.Btntoggle.Visible = False
                        load_disclosures(True)
                        btoggle = True
                    Case Else
                        Me.Btntoggle.Visible = True
                        btoggle = False
                        load_disclosures(False)
                End Select
                taxonomy = 2
            End If

            Me.uxMoveUp.Enabled = False
            Me.uxMoveDown.Enabled = False
            ' If bc_am_load_objects.obc_pub_types.show_doc_scan = True Then
            'LoadScannedList(scannedItemsList)
            'End If
        Catch

        End Try
    End Sub

    Private Sub uxMoveScannedEntry_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxMoveScannedEntry.Click

        add_scanned_item(sender, e)

    End Sub

    Private Sub add_scanned_item(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim i As Integer, selectedIndex As Integer
        Dim otrace As New bc_cs_activity_log("bc_am_at_submit_frm", "add_scanned_item", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            If Me.uxScannedList.SelectedIndex = -1 Then
                Exit Try
            End If
            Select Case taxonomy
                Case 0 'Author Tab
                    For Each selectedIndex In Me.uxScannedList.SelectedIndices
                        Dim authorAlreadyAssociatedWithDocument As Boolean = False
                        REM check not already entered
                        For i = 0 To ldoc.authors.Count - 1
                            If ldoc.authors(i).id = scannedIDs(selectedIndex) Then
                                authorAlreadyAssociatedWithDocument = True
                                Exit For
                            End If
                        Next
                        If Not authorAlreadyAssociatedWithDocument Then
                            For i = 0 To bc_am_load_objects.obc_users.user.Count - 1
                                If bc_am_load_objects.obc_users.user(i).id = scannedIDs(selectedIndex) Then
                                    ldoc.authors.Add(bc_am_load_objects.obc_users.user(i))
                                    Exit For
                                End If
                            Next
                        End If
                    Next
                    load_authors()
                Case 2 'Disclosure Tab
                    REM check not already entered
                    For Each selectedIndex In Me.uxScannedList.SelectedIndices
                        Dim disclosureAlreadyAssociatedWithDocument As Boolean = False
                        For i = 0 To ldoc.disclosures.Count - 1
                            If ldoc.disclosures(i).entity_id = scannedIDs(selectedIndex) Then
                                disclosureAlreadyAssociatedWithDocument = True
                                Exit For
                            End If
                        Next
                        If Not disclosureAlreadyAssociatedWithDocument Then
                            For i = 0 To bc_am_load_objects.obc_entities.disclosure_entities.Count - 1
                                If bc_am_load_objects.obc_entities.disclosure_entities(i).id = scannedIDs(selectedIndex) Then
                                    Dim default_entity As New bc_om_taxonomy
                                    default_entity.entity_id = bc_am_load_objects.obc_entities.disclosure_entities(i).id
                                    ldoc.disclosures.Add(default_entity)
                                    Exit For
                                End If
                            Next
                        End If
                    Next
                    REM Steve Wooderson Drexel 27/01/2014 Show prefs only
                    load_disclosures(btoggle)
                    'Select Case True
                    '    Case bc_cs_central_settings.my_entities_default_submit
                    '        load_disclosures(True)
                    '    Case Not bc_cs_central_settings.show_all_entities_submit
                    '        load_disclosures(True)
                    '    Case Else
                    '        load_disclosures(False)
                    'End Select
            End Select

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_at_submit_frm", "add_scanned_item", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_am_at_submit_frm", "add_scanned_item", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Friend Sub LoadScannedList(ByVal scannedList(,) As String)

        Dim otrace As New bc_cs_activity_log("bc_am_at_submit_frm", "LoadScannedList", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try

            scannedItemsList = scannedList
            uxScannedList.Items.Clear()

            If Not scannedList Is Nothing Then
                scannedIDs = New ArrayList

                Select Case taxonomy
                    Case 0 'Author Tab
                        For i = 0 To UBound(scannedList, 2)
                            If scannedList(2, i) = C_AUTHOR Then
                                uxScannedList.Items.Add(scannedList(0, i))
                                scannedIDs.Add(scannedList(1, i))
                            End If
                        Next
                    Case 2 'Disclosure Tab
                        For i = 0 To UBound(scannedList, 2)
                            If scannedList(2, i) = C_DISCLOSURE Then
                                uxScannedList.Items.Add(scannedList(0, i))
                                scannedIDs.Add(scannedList(1, i))
                            End If
                        Next
                End Select
            End If

            If uxScannedList.Items.Count = 0 Then
                uxScannedList.Enabled = False
                uxMoveScannedEntry.Enabled = False
            Else
                uxScannedList.Enabled = True
                uxMoveScannedEntry.Enabled = True
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_at_submit_frm", "LoadScannedList", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_am_at_submit_frm", "LoadScannedList", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Private Sub txtdate_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtdate.ValueChanged
        If bc_cs_central_settings.deny_backdated_submission Then
            Dim servicedate As New bc_cs_date_services
            If Trim(Me.dh.Text) = "" Then
                Me.dh.Text = "00"
            End If
            If Trim(Me.ds.Text) = "" Then
                Me.ds.Text = "00"
            End If
            If CDate(servicedate.DateToLocalShort(Me.txtdate.Value) + " " + Me.dh.Text + ":" + Me.ds.Text) < CDate(servicedate.DateToLocalShort(Now) + " " + Format(Now, "HH") + ":" + Format(Now, "mm")) Then
                Me.txtdate.Value = Now
                Me.dh.Text = Format(Now, "HH")
                Me.ds.Text = Format(Now, "mm")
            End If
        End If
    End Sub

    Private Sub dh_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dh.SelectedIndexChanged
        If bc_cs_central_settings.deny_backdated_submission Then
            Dim servicedate As New bc_cs_date_services
            If Trim(Me.dh.Text) = "" Then
                Me.dh.Text = "00"
            End If
            If Trim(Me.ds.Text) = "" Then
                Me.ds.Text = "00"
            End If
            If CDate(servicedate.DateToLocalShort(Me.txtdate.Value) + " " + Me.dh.Text + ":" + Me.ds.Text) < CDate(servicedate.DateToLocalShort(Now) + " " + Format(Now, "HH") + ":" + Format(Now, "mm")) Then
                Me.txtdate.Value = Now
                Me.dh.Text = Format(Now, "HH")
                Me.ds.Text = Format(Now, "mm")
            End If
        End If
    End Sub

    Private Sub ds_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ds.SelectedIndexChanged
        If bc_cs_central_settings.deny_backdated_submission Then
            Dim servicedate As New bc_cs_date_services
            If Trim(Me.dh.Text) = "" Then
                Me.dh.Text = "00"
            End If
            If Trim(Me.ds.Text) = "" Then
                Me.ds.Text = "00"
            End If
            If CDate(servicedate.DateToLocalShort(Me.txtdate.Value) + " " + Me.dh.Text + ":" + Me.ds.Text) < CDate(servicedate.DateToLocalShort(Now) + " " + Format(Now, "HH") + ":" + Format(Now, "mm")) Then
                Me.txtdate.Value = Now
                Me.dh.Text = Format(Now, "HH")
                Me.ds.Text = Format(Now, "mm")
            End If
        End If
    End Sub

    Private Sub uxMoveUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxMoveUp.Click

        Dim tx As String
        Dim idx As Integer
        Dim idx2 As Integer
        Dim tempUser As bc_om_user
        Dim tempTaxonomy As bc_om_taxonomy

        tx = Me.ListBox2.Text
        idx = Me.ListBox2.SelectedIndex
        Me.ListBox2.Items.RemoveAt(idx)
        Me.ListBox2.Items.Insert(idx - 1, tx)
        Me.ListBox2.SelectedIndex = idx - 1

        If taxonomy = 0 Then
            tempUser = ldoc.authors(idx)
            ldoc.authors.RemoveAt(idx)
            ldoc.authors.Insert(idx - 1, tempUser)
        End If

        If taxonomy = 1 Or taxonomy = 4 Then

            For i = 0 To ldoc.taxonomy.Count - 1
                If ldoc.taxonomy(i).name = ListBox2.Items(idx - 1).ToString Then
                    idx2 = i
                    tempTaxonomy = ldoc.taxonomy(idx2)
                    ldoc.taxonomy.RemoveAt(idx2)
                    ldoc.taxonomy.Insert(idx2 - 1, tempTaxonomy)
                    Exit For
                End If
            Next
        End If

        Me.ListBox2.Select()
        Me.ListBox2.Text = tx

        Me.uxMoveUp.Enabled = False
        Me.uxMoveDown.Enabled = False
        If taxonomy = 0 Or taxonomy = 1 Or taxonomy = 4 Then
            If Me.ListBox2.SelectedIndex > -1 Then
                If Me.ListBox2.SelectedIndex > 0 Then
                    Me.uxMoveUp.Enabled = True
                End If
                If Me.ListBox2.SelectedIndex < Me.ListBox2.Items.Count - 1 Then
                    Me.uxMoveDown.Enabled = True
                End If
            End If
        End If

        Me.btnsubmit.Enabled = True

    End Sub

    Private Sub uxMoveDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxMoveDown.Click

        Dim tx As String
        Dim idx As Integer
        Dim idx2 As Integer
        Dim tempUser As bc_om_user
        Dim tempTaxonomy As bc_om_taxonomy

        tx = Me.ListBox2.Text
        idx = Me.ListBox2.SelectedIndex
        Me.ListBox2.Items.RemoveAt(idx)
        Me.ListBox2.Items.Insert(idx + 1, tx)
        Me.ListBox2.SelectedIndex = idx + 1

        If taxonomy = 0 Then
            tempUser = ldoc.authors(idx)
            ldoc.authors.RemoveAt(idx)
            ldoc.authors.Insert(idx + 1, tempUser)
        End If

        If taxonomy = 1 Or taxonomy = 4 Then
            For i = 0 To ldoc.taxonomy.Count - 1
                If ldoc.taxonomy(i).name = ListBox2.Items(idx + 1).ToString Then
                    idx2 = i
                    tempTaxonomy = ldoc.taxonomy(idx2)
                    ldoc.taxonomy.RemoveAt(idx2)
                    ldoc.taxonomy.Insert(idx2 + 1, tempTaxonomy)
                    Exit For
                End If
            Next
        End If

        Me.ListBox2.Select()
        Me.ListBox2.Text = tx

        Me.uxMoveUp.Enabled = False
        Me.uxMoveDown.Enabled = False
        If taxonomy = 0 Or taxonomy = 1 Or taxonomy = 4 Then
            If Me.ListBox2.SelectedIndex > -1 Then
                If Me.ListBox2.SelectedIndex > 0 Then
                    Me.uxMoveUp.Enabled = True
                End If
                If Me.ListBox2.SelectedIndex < Me.ListBox2.Items.Count - 1 Then
                    Me.uxMoveDown.Enabled = True
                End If
            End If
        End If

        Me.btnsubmit.Enabled = True

    End Sub


    Private Sub bsnew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bsnew.Click
        Try
            REM =============
            Dim odialog As New OpenFileDialog
            odialog.Title = "Import Document"
            odialog.ShowDialog()

            If odialog.FileName = "" Then
                Exit Sub
            End If

            Dim sdoc As New bc_om_document
            sdoc.id = 0
            sdoc.master_flag = False
            For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                If bc_am_load_objects.obc_pub_types.pubtype(i).name = Me.cpts.Text Then
                    sdoc.pub_type_id = bc_am_load_objects.obc_pub_types.pubtype(i).id
                    Exit For
                End If
            Next
            sdoc.pub_type_name = Me.cpts.Text
            sdoc.originating_author = ldoc.originating_author
            sdoc.bus_area = ldoc.bus_area
            sdoc.checked_out_user = 0
            sdoc.stage = ldoc.stage
            'sdoc.doc_date = ldoc.doc_date
            sdoc.entity_id = ldoc.entity_id
            sdoc.originating_author = ldoc.originating_author
            sdoc.authors = ldoc.authors
            sdoc.taxonomy = ldoc.taxonomy
            sdoc.disclosures = ldoc.disclosures
            sdoc.workflow_stages = ldoc.workflow_stages
            'sdoc.title = Me.txttitle.Text
            sdoc.sub_title = Me.txtsubtitle.Text
            sdoc.doc_date = Now
            Dim filename As String
            filename = odialog.FileName

            sdoc.extension = filename.Substring(InStrRev(filename, ".") - 1, filename.Length - InStrRev(filename, ".") + 1)
            sdoc.register_only = False
            sdoc.filename = filename
            sdoc.bwith_document = True

            sdoc.title = Microsoft.VisualBasic.Left(odialog.SafeFileName, Len(odialog.SafeFileName) - Len(sdoc.extension))


            Dim fs As New bc_cs_file_transfer_services
            REM FIL FEB 2013
            If fs.write_document_to_bytestream(filename, sdoc.byteDoc, Nothing, True) = False Then
                'Dim omessage As New bc_cs_message("Blue Curve", "File: " + filename + " can't be accessed", bc_cs_message.MESSAGE)
                Exit Sub
            End If
            ldoc.support_documents.Add(sdoc)

            load_support_docs()
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bsnew", "click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Sub
    Private Sub load_support_docs()
        Try
            Dim lvew As ListViewItem
            Me.lvwsupport.Items.Clear()
            Dim bcs As New bc_cs_icon_services
            Dim image As Bitmap
            For i = 0 To Me.mimeimages.Images.Count - 1
                Me.mimeimages.Images.RemoveAt(0)
            Next

            For i = 0 To ldoc.support_documents.Count - 1
                If ldoc.support_documents(i).marked_for_delete = False Then
                    image = Nothing

                    image = bcs.get_icon_for_file_type(ldoc.support_documents(i).extension)

                    If Not IsNothing(image) Then
                        Me.mimeimages.Images.Add(image)
                        lvew = New ListViewItem(CStr(ldoc.support_documents(i).pub_type_name), Me.mimeimages.Images.Count - 1)
                    Else
                        lvew = New ListViewItem(CStr(ldoc.support_documents(i).pub_type_name))
                    End If
                    lvew.SubItems.Add(CStr(ldoc.support_documents(i).title))
                    lvew.SubItems.Add(CStr(ldoc.support_documents(i).filename))
                    Me.lvwsupport.Items.Add(lvew)
                End If
            Next
            Me.MenuStrip1.Enabled = False
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_at_submit_frm", "load_support_docs", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Sub

    Private Sub lvwsupport_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvwsupport.SelectedIndexChanged

        Me.MenuStrip1.Enabled = False
        If Me.lvwsupport.SelectedItems.Count = 1 Then
            'If ldoc.support_documents(Me.lvwsupport.SelectedItems(0).Index).support_doc_display_only = False Then
            Me.MenuStrip1.Enabled = True
            'End If
        End If
    End Sub

    Private Sub Cpub_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cpub.SelectedIndexChanged
        Try
            If loading = True Then
                Exit Sub
            End If


            Dim start_remove_pages = 1
            If ldoc.entity_id <> 0 Then
                start_remove_pages = start_remove_pages + 1
            End If
            If bc_am_load_objects.obc_entities.disclosure_entities.Count > 0 Then
                start_remove_pages = start_remove_pages + 1
            End If

            For i = start_remove_pages To Me.ttaxonomy.TabPages.Count - 1
                Me.ttaxonomy.TabPages.RemoveAt(start_remove_pages)
            Next


            If Me.Cpub.SelectedIndex > -1 Then
                For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                    If bc_am_load_objects.obc_pub_types.pubtype(i).name = Me.Cpub.Text Then
                        load_taxonomy(bc_am_load_objects.obc_pub_types.pubtype(i).id)
                        Exit For
                    End If
                Next
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("cpub", "SelectedIndexChanged", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub






    Private Sub DeleteToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteToolStripMenuItem.Click

        If Me.lvwsupport.SelectedItems.Count = -1 Then
            Exit Sub
        End If
        Dim idx As Integer = -1
        For i = 0 To ldoc.support_documents.Count - 1
            If ldoc.support_documents(i).marked_for_delete = False Then
                idx = idx + 1
            End If
            If idx = Me.lvwsupport.SelectedItems(0).Index Then
                If ldoc.support_documents(i).support_doc_display_only = False Then
                    ldoc.support_documents.RemoveAt(i)
                Else
                    'Dim j As bc_om_document
                    If ldoc.support_documents(i).checked_out_user <> 0 Then
                        Dim omsg As New bc_cs_message("Blue Curve", "Document is checked out cannot delete", bc_cs_message.MESSAGE, "false", False, "Yes", "No", True)
                    Else
                        ldoc.support_documents(i).marked_for_delete = True
                    End If
                End If
                Me.load_support_docs()
                Exit Sub
            End If

        Next

    End Sub

    Private Sub ToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem1.Click
        Try
            If Me.lvwsupport.SelectedItems.Count = 1 Then
                Dim osubmit As New bc_am_at_submit_frm
                osubmit.import_support_doc = True
                osubmit.register_only = False
                osubmit.edit_mode = True
                osubmit.Text = "Blue Curve - Support Document Categorisation"
                osubmit.attach_mode = False

                osubmit.Cpub.Enabled = True

                osubmit.ldoc = ldoc.support_documents(Me.lvwsupport.SelectedItems(0).Index)

                osubmit.ActiveControl = osubmit.txttitle
                osubmit.Focus()
                osubmit.ShowDialog()
                If Not osubmit.ok_selected Then
                    Me.MenuStrip1.Enabled = False
                    REM if cancel selected do nothing
                    Dim ocommentary As New bc_cs_activity_log("bc_am_workflow", "import_master_document", bc_cs_activity_codes.COMMENTARY, "Cancel Selected on Submit form.")
                    Exit Sub
                End If

                ldoc.support_documents(Me.lvwsupport.SelectedItems(0).Index).metadata_changed = True
                For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                    If osubmit.Cpub.Text = bc_am_load_objects.obc_pub_types.pubtype(i).name Then
                        ldoc.support_documents(Me.lvwsupport.SelectedItems(0).Index).pub_type_id = bc_am_load_objects.obc_pub_types.pubtype(i).id()
                        ldoc.support_documents(Me.lvwsupport.SelectedItems(0).Index).pub_type_name = bc_am_load_objects.obc_pub_types.pubtype(i).name
                        Exit For
                    End If
                Next
                Me.load_support_docs()
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bsdel", "Click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub

    Private Sub cpts_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cpts.SelectedIndexChanged
        Me.bsnew.Enabled = False
        If Me.cpts.SelectedIndex > -1 Then
            Me.bsnew.Enabled = True
        End If
    End Sub

    Private Sub Btntoggle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btntoggle.Click
        REM Steve Wooderson Drexel 27/01/2014 Show prefs only
        If btoggle = True Then
            btoggle = False
        Else
            btoggle = True
        End If
        prev_toggle = btoggle
        If Me.ttaxonomy.SelectedIndex = 1 Then
            load_lead_entity(btoggle)
        End If
        If Me.ttaxonomy.SelectedIndex = 2 Then
            load_disclosures(btoggle)
        End If

        If Me.ttaxonomy.SelectedIndex > 2 Then
            change_taxonomy(Me.cbotaxonomy.SelectedIndex, btoggle)
        End If

    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chkurgent.CheckedChanged
        Me.bdetails.Enabled = False
        If Me.Chkurgent.Checked = True Then
            Me.bdetails.Enabled = True
        End If
    End Sub

    Private Sub bdetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bdetails.Click
        Dim vfgc As New bc_am_extended_class
        Dim cfgc As New cttrl_bc_am_extended_class(vfgc, ldoc)
        cfgc.load_data()
        vfgc.TopMost = True
        vfgc.ShowDialog()
    End Sub

    Private Sub cboworkflow_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboworkflow.SelectedIndexChanged

    End Sub
End Class
