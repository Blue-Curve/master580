Imports System.Windows.Forms
Imports System.Collections
Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports BlueCurve.Core.AS
Imports System.Drawing
Imports System.Threading


Public Class bc_am_component_refresh
    Inherits System.Windows.Forms.Form
    Public form_name As String
    Public slocator As String
    Public sload As Boolean
    Public component_mode As Boolean = False
    Public bfrombutton1 As Boolean = True
    Public bfrombutton2 As Boolean = False
    Public load_apply As Boolean
    Public ao_object As bc_ao_at_object
    Public closed_now As Boolean = False
    Private lvItem As ListViewItem
    Private selectedItemIndex As Integer
    Public highlight_selected As Boolean = False
    Public loading As Boolean = False
    Public sdc_colour_index As Integer = 255
    Public udc_colour_index As Integer = 32768
    Public prev_selected_index As Integer = -1
    Public master_refresh_disabled As Boolean = False

    Public disable_contributer As Boolean = False
    Public disable_refresh As Boolean = False
              



#Region " Windows Form Designer generated code "
    Public index As Integer
    Public index2 As Integer
    Public index3 As Integer
    Public checked As Boolean
    Public ok_selected As Boolean = False
    Public cancel_selected As Boolean = False
    Public context_items_count As Integer
    Public context_items_pos As Integer
    Friend WithEvents chighsdc As System.Windows.Forms.CheckBox
    Friend WithEvents bhighlight As System.Windows.Forms.Button
    Friend WithEvents prtf As System.Windows.Forms.Panel
    Friend WithEvents lrtf As System.Windows.Forms.Label
    Friend WithEvents uxNoCompParams As System.Windows.Forms.Label
    Friend WithEvents trtf As System.Windows.Forms.RichTextBox
    Friend WithEvents Rall As System.Windows.Forms.RadioButton
    Friend WithEvents Redit As System.Windows.Forms.RadioButton
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents tabimages As System.Windows.Forms.ImageList
    Friend WithEvents uxToolTip As System.Windows.Forms.ToolTip
    Public has_context_items As Boolean = False
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
    Friend WithEvents ListView1 As System.Windows.Forms.ListView
    Friend WithEvents Description As System.Windows.Forms.ColumnHeader
    Friend WithEvents Locator As System.Windows.Forms.ColumnHeader
    Friend WithEvents LastUpdate As System.Windows.Forms.ColumnHeader
    Friend WithEvents Contributor As System.Windows.Forms.ColumnHeader
    Friend WithEvents syslocator As System.Windows.Forms.ColumnHeader
    Friend WithEvents lstages As System.Windows.Forms.ComboBox
    Friend WithEvents btnopen As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents lblpubtypes As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents ListView3 As System.Windows.Forms.ListView
    Friend WithEvents name1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Locator1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Contributor1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents last_refresh_date As System.Windows.Forms.ColumnHeader
    Friend WithEvents Data_update_date As System.Windows.Forms.ColumnHeader
    Friend WithEvents system_locator As System.Windows.Forms.ColumnHeader
    Friend WithEvents ListView2 As BlueCurve.Core.AS.ComboBoxListView
    Friend WithEvents Names As System.Windows.Forms.ColumnHeader
    Friend WithEvents Value As System.Windows.Forms.ColumnHeader
    Friend WithEvents ListBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents DateTimePicker1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents GroupParamters As System.Windows.Forms.GroupBox
    Friend WithEvents bgoto As System.Windows.Forms.CheckBox
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    'Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents ComboBox2 As System.Windows.Forms.ComboBox
    'Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents RadioButton1 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton2 As System.Windows.Forms.RadioButton
    Friend WithEvents DateTimePicker2 As System.Windows.Forms.DateTimePicker
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents blogo As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_component_refresh))
        Me.Description = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Contributor = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.LastUpdate = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.syslocator = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lstages = New System.Windows.Forms.ComboBox()
        Me.btnopen = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.lblpubtypes = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.ListView3 = New System.Windows.Forms.ListView()
        Me.name1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Locator1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Contributor1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.last_refresh_date = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Data_update_date = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.system_locator = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.GroupParamters = New System.Windows.Forms.GroupBox()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.DateTimePicker1 = New System.Windows.Forms.DateTimePicker()
        Me.ListBox1 = New System.Windows.Forms.ComboBox()
        Me.ListView2 = New BlueCurve.Core.[AS].ComboBoxListView()
        Me.Names = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Value = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.tabimages = New System.Windows.Forms.ImageList(Me.components)
        Me.Label10 = New System.Windows.Forms.Label()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.bgoto = New System.Windows.Forms.CheckBox()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.ComboBox2 = New System.Windows.Forms.ComboBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.RadioButton1 = New System.Windows.Forms.RadioButton()
        Me.RadioButton2 = New System.Windows.Forms.RadioButton()
        Me.DateTimePicker2 = New System.Windows.Forms.DateTimePicker()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.prtf = New System.Windows.Forms.Panel()
        Me.trtf = New System.Windows.Forms.RichTextBox()
        Me.lrtf = New System.Windows.Forms.Label()
        Me.uxNoCompParams = New System.Windows.Forms.Label()
        Me.bhighlight = New System.Windows.Forms.Button()
        Me.blogo = New System.Windows.Forms.PictureBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.chighsdc = New System.Windows.Forms.CheckBox()
        Me.Rall = New System.Windows.Forms.RadioButton()
        Me.Redit = New System.Windows.Forms.RadioButton()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.uxToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.GroupParamters.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.prtf.SuspendLayout()
        CType(Me.blogo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Description
        '
        Me.Description.Text = "Description"
        Me.Description.Width = 178
        '
        'Contributor
        '
        Me.Contributor.Text = "Contributor"
        Me.Contributor.Width = 77
        '
        'LastUpdate
        '
        Me.LastUpdate.Text = "Last Update"
        Me.LastUpdate.Width = 73
        '
        'syslocator
        '
        Me.syslocator.Text = "System Locator"
        '
        'lstages
        '
        Me.lstages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.lstages.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstages.Location = New System.Drawing.Point(328, 324)
        Me.lstages.Name = "lstages"
        Me.lstages.Size = New System.Drawing.Size(203, 21)
        Me.lstages.TabIndex = 39
        '
        'btnopen
        '
        Me.btnopen.BackColor = System.Drawing.SystemColors.ControlLight
        Me.btnopen.Enabled = False
        Me.btnopen.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnopen.Location = New System.Drawing.Point(307, 405)
        Me.btnopen.Name = "btnopen"
        Me.btnopen.Size = New System.Drawing.Size(72, 24)
        Me.btnopen.TabIndex = 45
        Me.btnopen.Text = "&Refresh"
        Me.uxToolTip.SetToolTip(Me.btnopen, "Refreshes the selected component with the selected parameter values above.")
        Me.btnopen.UseVisualStyleBackColor = False
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.SystemColors.ControlLight
        Me.Button1.Enabled = False
        Me.Button1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.Location = New System.Drawing.Point(383, 405)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(72, 24)
        Me.Button1.TabIndex = 46
        Me.Button1.Text = "&Save"
        Me.uxToolTip.SetToolTip(Me.Button1, "Saves the component with the selected parameter values above.  These will take ef" & _
        "fect on future document refreshes.")
        Me.Button1.UseVisualStyleBackColor = False
        '
        'lblpubtypes
        '
        Me.lblpubtypes.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblpubtypes.Location = New System.Drawing.Point(250, 327)
        Me.lblpubtypes.Name = "lblpubtypes"
        Me.lblpubtypes.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblpubtypes.Size = New System.Drawing.Size(72, 16)
        Me.lblpubtypes.TabIndex = 47
        Me.lblpubtypes.Text = "&Workflow:"
        '
        'Label8
        '
        Me.Label8.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(196, 356)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(24, 16)
        Me.Label8.TabIndex = 48
        Me.Label8.Text = "&All:"
        Me.Label8.Visible = False
        '
        'CheckBox1
        '
        Me.CheckBox1.Location = New System.Drawing.Point(228, 356)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(16, 16)
        Me.CheckBox1.TabIndex = 49
        Me.CheckBox1.Text = "CheckBox1"
        Me.CheckBox1.Visible = False
        '
        'ListView3
        '
        Me.ListView3.CheckBoxes = True
        Me.ListView3.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.name1, Me.Locator1, Me.Contributor1, Me.last_refresh_date, Me.Data_update_date, Me.system_locator})
        Me.ListView3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListView3.FullRowSelect = True
        Me.ListView3.GridLines = True
        Me.ListView3.Location = New System.Drawing.Point(11, 72)
        Me.ListView3.MultiSelect = False
        Me.ListView3.Name = "ListView3"
        Me.ListView3.Size = New System.Drawing.Size(269, 187)
        Me.ListView3.SmallImageList = Me.ImageList1
        Me.ListView3.TabIndex = 57
        Me.ListView3.UseCompatibleStateImageBehavior = False
        Me.ListView3.View = System.Windows.Forms.View.Details
        '
        'name1
        '
        Me.name1.Text = "Name"
        Me.name1.Width = 183
        '
        'Locator1
        '
        Me.Locator1.Text = "Type"
        Me.Locator1.Width = 73
        '
        'Contributor1
        '
        Me.Contributor1.Text = "Contributor"
        Me.Contributor1.Width = 50
        '
        'last_refresh_date
        '
        Me.last_refresh_date.Text = "Last Refresh Date"
        Me.last_refresh_date.Width = 94
        '
        'Data_update_date
        '
        Me.Data_update_date.Text = "Data Last Update"
        Me.Data_update_date.Width = 101
        '
        'system_locator
        '
        Me.system_locator.Text = "System Locator"
        '
        'ImageList1
        '
        Me.ImageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit
        Me.ImageList1.ImageSize = New System.Drawing.Size(1, 21)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        '
        'GroupParamters
        '
        Me.GroupParamters.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.GroupParamters.Controls.Add(Me.TextBox1)
        Me.GroupParamters.Controls.Add(Me.DateTimePicker1)
        Me.GroupParamters.Controls.Add(Me.ListBox1)
        Me.GroupParamters.Controls.Add(Me.ListView2)
        Me.GroupParamters.Controls.Add(Me.Label10)
        Me.GroupParamters.Controls.Add(Me.TextBox2)
        Me.GroupParamters.Controls.Add(Me.Label11)
        Me.GroupParamters.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.GroupParamters.Location = New System.Drawing.Point(188, 66)
        Me.GroupParamters.Name = "GroupParamters"
        Me.GroupParamters.Size = New System.Drawing.Size(344, 192)
        Me.GroupParamters.TabIndex = 61
        Me.GroupParamters.TabStop = False
        Me.GroupParamters.Text = "GroupParameters"
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(8, 164)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(136, 21)
        Me.TextBox1.TabIndex = 61
        Me.TextBox1.Text = "TextBox1"
        '
        'DateTimePicker1
        '
        Me.DateTimePicker1.Location = New System.Drawing.Point(8, 164)
        Me.DateTimePicker1.Name = "DateTimePicker1"
        Me.DateTimePicker1.Size = New System.Drawing.Size(136, 21)
        Me.DateTimePicker1.TabIndex = 60
        '
        'ListBox1
        '
        Me.ListBox1.AllowDrop = True
        Me.ListBox1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.ListBox1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.ListBox1.BackColor = System.Drawing.SystemColors.Window
        Me.ListBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ListBox1.DropDownWidth = 300
        Me.ListBox1.ItemHeight = 13
        Me.ListBox1.Location = New System.Drawing.Point(8, 164)
        Me.ListBox1.MaxDropDownItems = 16
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(136, 21)
        Me.ListBox1.TabIndex = 59
        Me.ListBox1.Visible = False
        '
        'ListView2
        '
        Me.ListView2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListView2.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.Names, Me.Value})
        Me.ListView2.combo = Nothing
        Me.ListView2.Enabled = False
        Me.ListView2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListView2.FullRowSelect = True
        Me.ListView2.GridLines = True
        Me.ListView2.HideSelection = False
        Me.ListView2.Location = New System.Drawing.Point(8, 31)
        Me.ListView2.MultiSelect = False
        Me.ListView2.Name = "ListView2"
        Me.ListView2.Size = New System.Drawing.Size(328, 156)
        Me.ListView2.SmallImageList = Me.tabimages
        Me.ListView2.TabIndex = 52
        Me.ListView2.UseCompatibleStateImageBehavior = False
        Me.ListView2.View = System.Windows.Forms.View.Details
        '
        'Names
        '
        Me.Names.Text = "Item"
        Me.Names.Width = 152
        '
        'Value
        '
        Me.Value.Text = "Value"
        Me.Value.Width = 154
        '
        'tabimages
        '
        Me.tabimages.ImageStream = CType(resources.GetObject("tabimages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.tabimages.TransparentColor = System.Drawing.Color.Transparent
        Me.tabimages.Images.SetKeyName(0, "childparent")
        Me.tabimages.Images.SetKeyName(1, "parentchild")
        Me.tabimages.Images.SetKeyName(2, "Links")
        Me.tabimages.Images.SetKeyName(3, "Attributes")
        Me.tabimages.Images.SetKeyName(4, "Add")
        Me.tabimages.Images.SetKeyName(5, "Edit")
        Me.tabimages.Images.SetKeyName(6, "deactivate")
        Me.tabimages.Images.SetKeyName(7, "Delete")
        Me.tabimages.Images.SetKeyName(8, "class")
        Me.tabimages.Images.SetKeyName(9, "mandatory")
        Me.tabimages.Images.SetKeyName(10, "audited")
        Me.tabimages.Images.SetKeyName(11, "lookup")
        Me.tabimages.Images.SetKeyName(12, "workflow")
        Me.tabimages.Images.SetKeyName(13, "number")
        Me.tabimages.Images.SetKeyName(14, "date")
        Me.tabimages.Images.SetKeyName(15, "string")
        Me.tabimages.Images.SetKeyName(16, "boolean")
        Me.tabimages.Images.SetKeyName(17, "warning.ico")
        Me.tabimages.Images.SetKeyName(18, "data")
        Me.tabimages.Images.SetKeyName(19, "users")
        Me.tabimages.Images.SetKeyName(20, "user")
        Me.tabimages.Images.SetKeyName(21, "parent")
        Me.tabimages.Images.SetKeyName(22, "child")
        Me.tabimages.Images.SetKeyName(23, "selected")
        Me.tabimages.Images.SetKeyName(24, "push publish")
        '
        'Label10
        '
        Me.Label10.Location = New System.Drawing.Point(8, 32)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(112, 16)
        Me.Label10.TabIndex = 66
        Me.Label10.Text = "Last Refresh Date"
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(8, 56)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(88, 21)
        Me.TextBox2.TabIndex = 67
        Me.TextBox2.Text = "TextBox2"
        '
        'Label11
        '
        Me.Label11.Location = New System.Drawing.Point(8, 88)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(112, 16)
        Me.Label11.TabIndex = 68
        Me.Label11.Text = "Refresh Req"
        '
        'bgoto
        '
        Me.bgoto.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.bgoto.CheckAlign = System.Drawing.ContentAlignment.BottomRight
        Me.bgoto.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bgoto.ForeColor = System.Drawing.SystemColors.ControlText
        Me.bgoto.Location = New System.Drawing.Point(115, 224)
        Me.bgoto.Name = "bgoto"
        Me.bgoto.Size = New System.Drawing.Size(127, 16)
        Me.bgoto.TabIndex = 65
        Me.bgoto.Text = "Disable  Refresh:"
        Me.uxToolTip.SetToolTip(Me.bgoto, "Disables the component from being updated by any future document refreshes.")
        Me.bgoto.UseVisualStyleBackColor = False
        '
        'ComboBox1
        '
        Me.ComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox1.Font = New System.Drawing.Font("Verdana", 8.25!)
        Me.ComboBox1.Location = New System.Drawing.Point(349, 287)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(182, 21)
        Me.ComboBox1.TabIndex = 66
        '
        'Label9
        '
        Me.Label9.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Label9.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(256, 224)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(82, 16)
        Me.Label9.TabIndex = 67
        Me.Label9.Text = "Contributor:"
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.SystemColors.ControlLight
        Me.Button2.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button2.Location = New System.Drawing.Point(459, 405)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(72, 24)
        Me.Button2.TabIndex = 69
        Me.Button2.Text = "&Close"
        Me.Button2.UseVisualStyleBackColor = False
        '
        'ComboBox2
        '
        Me.ComboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox2.Location = New System.Drawing.Point(328, 351)
        Me.ComboBox2.Name = "ComboBox2"
        Me.ComboBox2.Size = New System.Drawing.Size(204, 21)
        Me.ComboBox2.TabIndex = 70
        '
        'Label12
        '
        Me.Label12.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(250, 356)
        Me.Label12.Name = "Label12"
        Me.Label12.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label12.Size = New System.Drawing.Size(78, 16)
        Me.Label12.TabIndex = 71
        Me.Label12.Text = "&Language:"
        '
        'Label13
        '
        Me.Label13.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Label13.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(5, 270)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(527, 16)
        Me.Label13.TabIndex = 72
        Me.Label13.Text = "Refresh enabled"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label14
        '
        Me.Label14.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(216, 381)
        Me.Label14.Name = "Label14"
        Me.Label14.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label14.Size = New System.Drawing.Size(96, 16)
        Me.Label14.TabIndex = 73
        Me.Label14.Text = "&Data at Date:"
        '
        'RadioButton1
        '
        Me.RadioButton1.AutoCheck = False
        Me.RadioButton1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioButton1.Location = New System.Drawing.Point(316, 381)
        Me.RadioButton1.Name = "RadioButton1"
        Me.RadioButton1.Size = New System.Drawing.Size(80, 16)
        Me.RadioButton1.TabIndex = 74
        Me.RadioButton1.Text = "&Current"
        '
        'RadioButton2
        '
        Me.RadioButton2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioButton2.Location = New System.Drawing.Point(416, 381)
        Me.RadioButton2.Name = "RadioButton2"
        Me.RadioButton2.Size = New System.Drawing.Size(80, 16)
        Me.RadioButton2.TabIndex = 75
        Me.RadioButton2.Text = "&Define"
        '
        'DateTimePicker2
        '
        Me.DateTimePicker2.Location = New System.Drawing.Point(391, 377)
        Me.DateTimePicker2.Name = "DateTimePicker2"
        Me.DateTimePicker2.Size = New System.Drawing.Size(140, 21)
        Me.DateTimePicker2.TabIndex = 76
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.Label9)
        Me.Panel1.Controls.Add(Me.bgoto)
        Me.Panel1.Controls.Add(Me.prtf)
        Me.Panel1.Location = New System.Drawing.Point(-7, 64)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(552, 252)
        Me.Panel1.TabIndex = 77
        '
        'prtf
        '
        Me.prtf.Controls.Add(Me.trtf)
        Me.prtf.Controls.Add(Me.lrtf)
        Me.prtf.Controls.Add(Me.uxNoCompParams)
        Me.prtf.Location = New System.Drawing.Point(11, 1)
        Me.prtf.Name = "prtf"
        Me.prtf.Size = New System.Drawing.Size(526, 201)
        Me.prtf.TabIndex = 85
        '
        'trtf
        '
        Me.trtf.BackColor = System.Drawing.Color.White
        Me.trtf.Location = New System.Drawing.Point(6, 18)
        Me.trtf.Name = "trtf"
        Me.trtf.ReadOnly = True
        Me.trtf.Size = New System.Drawing.Size(512, 181)
        Me.trtf.TabIndex = 85
        Me.trtf.Text = ""
        Me.trtf.Visible = False
        '
        'lrtf
        '
        Me.lrtf.AutoSize = True
        Me.lrtf.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lrtf.Location = New System.Drawing.Point(3, 2)
        Me.lrtf.Name = "lrtf"
        Me.lrtf.Size = New System.Drawing.Size(0, 13)
        Me.lrtf.TabIndex = 86
        '
        'uxNoCompParams
        '
        Me.uxNoCompParams.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.uxNoCompParams.Font = New System.Drawing.Font("Verdana", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxNoCompParams.Location = New System.Drawing.Point(73, 106)
        Me.uxNoCompParams.Name = "uxNoCompParams"
        Me.uxNoCompParams.Size = New System.Drawing.Size(376, 32)
        Me.uxNoCompParams.TabIndex = 84
        Me.uxNoCompParams.Text = "No Parameters For Selected Component"
        '
        'bhighlight
        '
        Me.bhighlight.BackColor = System.Drawing.SystemColors.ControlLight
        Me.bhighlight.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bhighlight.Location = New System.Drawing.Point(228, 405)
        Me.bhighlight.Name = "bhighlight"
        Me.bhighlight.Size = New System.Drawing.Size(72, 24)
        Me.bhighlight.TabIndex = 84
        Me.bhighlight.Text = "&Highlight"
        Me.bhighlight.UseVisualStyleBackColor = False
        '
        'blogo
        '
        Me.blogo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.blogo.Image = CType(resources.GetObject("blogo.Image"), System.Drawing.Image)
        Me.blogo.Location = New System.Drawing.Point(471, -4)
        Me.blogo.Name = "blogo"
        Me.blogo.Size = New System.Drawing.Size(63, 64)
        Me.blogo.TabIndex = 79
        Me.blogo.TabStop = False
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(0, 0)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(440, 64)
        Me.PictureBox1.TabIndex = 78
        Me.PictureBox1.TabStop = False
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label3.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(4, 416)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(248, 22)
        Me.Label3.TabIndex = 81
        Me.Label3.Text = "© Blue Curve Limited. All rights reserved."
        '
        'PictureBox2
        '
        Me.PictureBox2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(5, 381)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(184, 48)
        Me.PictureBox2.TabIndex = 80
        Me.PictureBox2.TabStop = False
        '
        'chighsdc
        '
        Me.chighsdc.AutoSize = True
        Me.chighsdc.Location = New System.Drawing.Point(8, 322)
        Me.chighsdc.Name = "chighsdc"
        Me.chighsdc.Size = New System.Drawing.Size(75, 17)
        Me.chighsdc.TabIndex = 82
        Me.chighsdc.Text = "Highlight"
        Me.chighsdc.UseVisualStyleBackColor = True
        '
        'Rall
        '
        Me.Rall.AutoSize = True
        Me.Rall.Location = New System.Drawing.Point(3, 6)
        Me.Rall.Name = "Rall"
        Me.Rall.Size = New System.Drawing.Size(39, 17)
        Me.Rall.TabIndex = 85
        Me.Rall.TabStop = True
        Me.Rall.Text = "All"
        Me.Rall.UseVisualStyleBackColor = True
        '
        'Redit
        '
        Me.Redit.AutoSize = True
        Me.Redit.Location = New System.Drawing.Point(3, 26)
        Me.Redit.Name = "Redit"
        Me.Redit.Size = New System.Drawing.Size(155, 17)
        Me.Redit.TabIndex = 86
        Me.Redit.TabStop = True
        Me.Redit.Text = "Components I can Edit"
        Me.Redit.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.Rall)
        Me.Panel2.Controls.Add(Me.Redit)
        Me.Panel2.Location = New System.Drawing.Point(29, 332)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(160, 43)
        Me.Panel2.TabIndex = 87
        '
        'bc_am_component_refresh
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.AutoSize = True
        Me.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.CancelButton = Me.Button2
        Me.ClientSize = New System.Drawing.Size(538, 438)
        Me.Controls.Add(Me.chighsdc)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.GroupParamters)
        Me.Controls.Add(Me.bhighlight)
        Me.Controls.Add(Me.blogo)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.DateTimePicker2)
        Me.Controls.Add(Me.RadioButton2)
        Me.Controls.Add(Me.RadioButton1)
        Me.Controls.Add(Me.Label14)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.ComboBox2)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.ComboBox1)
        Me.Controls.Add(Me.CheckBox1)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.btnopen)
        Me.Controls.Add(Me.lstages)
        Me.Controls.Add(Me.lblpubtypes)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.PictureBox2)
        Me.Controls.Add(Me.ListView3)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "bc_am_component_refresh"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "bc_am_component_refresh"
        Me.GroupParamters.ResumeLayout(False)
        Me.GroupParamters.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.prtf.ResumeLayout(False)
        Me.prtf.PerformLayout()
        CType(Me.blogo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private Sub bc_am_component_refresh_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        closed_now = True
    End Sub

    Private Sub bc_am_component_refresh_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If e.CloseReason = CloseReason.UserClosing Then
            e.Cancel = True
        End If
    End Sub
    Private Sub bc_am_component_refresh_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim i As Integer
        Me.ok_selected = False
        Me.DateTimePicker2.Visible = False
        Me.Text = "Blue Curve create - Component Refresh"
        load_apply = False
        'Me.Label7.Visible = False
        'Me.Pubthumbnail.Visible = False
        'Me.PictureBox3.Visible = False
        Me.GroupParamters.Visible = False
        Me.bgoto.Enabled = True
        Me.bhighlight.Enabled = True
        Me.lstages.Items.Clear()
        Me.ComboBox2.Items.Clear()

        Me.ListView2.combo = ListBox1

        checked = False
        load_components()

        If Me.component_mode = True Then
            Me.RadioButton1.Visible = False
            Me.RadioButton2.Visible = False
            Me.Label14.Visible = False
            Me.ComboBox2.Visible = False
            Me.lstages.Visible = False
            Me.Label12.Visible = False
            Me.CheckBox1.Visible = False
            Me.lblpubtypes.Visible = False
            Me.Label8.Visible = False
            Me.Width = 544
            Me.GroupParamters.Left = 5
            'Me.GroupParamters.Width = 528

            Me.GroupParamters.Width = Me.Width - 5
            Me.ListView2.Columns(0).Width = 245
            Me.ListView2.Columns(1).Width = 245
            Me.ListView3.Visible = False
            For i = 0 To bc_am_load_objects.obc_current_document.refresh_components.refresh_components.Count - 1
                'bc_am_load_objects.obc_current_document.refresh_components.refresh_components(i).no_refresh = 1
                Me.ListView3.Items(i).Checked = False
                If bc_am_load_objects.obc_current_document.refresh_components.refresh_components(i).locator = slocator Then
                    Me.ListView3.Items(i).Selected = True
                    Me.ListView3.Items(i).Checked = True
                    index = i
                    'bc_am_load_objects.obc_current_document.refresh_components.refresh_components(i).no_refresh = 0
                    If bc_am_load_objects.obc_current_document.refresh_components.refresh_components(i).disabled = 1 Then
                        Me.btnopen.Enabled = False
                    End If
                End If
            Next
        End If
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If Me.DateTimePicker1.Visible = True Then
            setValue()
            Me.DateTimePicker1.Visible = False
        End If
        save(True)
    End Sub
    Private Sub save(Optional ByVal close As Boolean = False)
        If Not validateItems() Then
            Exit Sub
        End If
        ok_selected = False
        cancel_selected = False
        set_checks()

        If IsNumeric(bc_am_load_objects.obc_current_document.id) And bc_am_load_objects.obc_current_document.id > 0 Then
            bc_am_load_objects.obc_current_document.write_data_to_file(bc_cs_central_settings.local_repos_path + CStr(bc_am_load_objects.obc_current_document.id) + ".dat")
        Else
            If bc_am_load_objects.obc_current_document.filename.Substring(bc_am_load_objects.obc_current_document.filename.Length - 4, 4) <> bc_am_load_objects.obc_current_document.extension Then
                bc_am_load_objects.obc_current_document.write_data_to_file(bc_cs_central_settings.local_repos_path + bc_am_load_objects.obc_current_document.filename + ".dat")
            Else
                bc_am_load_objects.obc_current_document.write_data_to_file(bc_cs_central_settings.local_repos_path + bc_am_load_objects.obc_current_document.filename.Substring(0, bc_am_load_objects.obc_current_document.filename.Length - 4) + ".dat")
            End If
        End If
        If close = True Then
            Me.Hide()
        End If
    End Sub
    Private Sub load_components()
        Dim slog As New bc_cs_activity_log("bc_am_component_refresh", "load_components", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim i, j As Integer
            Me.ListView3.Width = 530
            Me.TextBox1.Visible = False
            Me.DateTimePicker1.Visible = False

            Me.bgoto.Enabled = False
            Me.bhighlight.Enabled = False
            Me.ComboBox1.Enabled = False
            Me.loading = True
            If component_mode = False Then
                Me.chighsdc.Visible = False

                REM get sdc udc colour
                Dim idx As String
                Me.chighsdc.Visible = False
                Me.Rall.Visible = False
                Me.Redit.Visible = False
                REM only turn on highlight functionality if properties set
                Me.Rall.Enabled = False
                Me.Redit.Enabled = False

                idx = ao_object.get_property_value("xrnet_sdc_colour_idx")
                If IsNumeric(idx) Then
                    Me.chighsdc.Visible = True
                    Me.Rall.Visible = True
                    Me.Redit.Visible = True
                    Me.sdc_colour_index = idx
                End If
                idx = ao_object.get_property_value("xrnet_udc_colour_idx")
                If IsNumeric(idx) Then
                    Me.chighsdc.Visible = True
                    Me.Rall.Visible = True
                    Me.Redit.Visible = True
                    Me.udc_colour_index = idx
                End If
                If bc_am_load_objects.obc_current_document.refresh_components.show_sdcs = True Then

                    Me.chighsdc.Checked = True
                    Me.Rall.Checked = True
                End If
                If bc_am_load_objects.obc_current_document.refresh_components.show_udcs = True Then

                    Me.chighsdc.Checked = True
                    Me.Redit.Checked = True
                End If

            Else
                Me.chighsdc.Visible = False
                Me.Rall.Visible = False
                Me.Redit.Visible = False
            End If
            Me.loading = False

            Me.ListView3.Items.Clear()
            For i = 0 To bc_am_load_objects.obc_current_document.refresh_components.refresh_components.Count - 1
                With bc_am_load_objects.obc_current_document.refresh_components.refresh_components(i)
                    Dim item As ListViewItem = Nothing
                    REM if entity is not documemy entity append the name
                    If .entity_id = 0 Or .entity_Id = bc_am_load_objects.obc_current_document.entity_id Then
                        item = New ListViewItem(CStr(.name))
                    Else
                        For j = 0 To bc_am_load_objects.obc_entities.entity.Count - 1
                            If bc_am_load_objects.obc_entities.entity(j).id = .entity_id Then
                                item = New ListViewItem(CStr(.name + " [" + bc_am_load_objects.obc_entities.entity(j).name + "]"))
                                Exit For
                            End If

                        Next
                    End If
                    If InStr(.locator, "_at") > 0 Then
                        Select Case .mode
                            Case 1
                                item.SubItems.Add(CStr("system text"))
                            Case 2
                                item.SubItems.Add(CStr("system table"))
                            Case 3
                                item.SubItems.Add(CStr("system chart"))
                            Case 4, 12
                                item.SubItems.Add(CStr("system image"))
                            Case 5
                                item.SubItems.Add(CStr("system file insert"))
                            Case 6
                                item.SubItems.Add(CStr("system index"))
                            Case 8
                                item.SubItems.Add(CStr("system edit rtf"))
                            Case 9
                                item.SubItems.Add(CStr("system edit text"))
                            Case Else
                                item.SubItems.Add(CStr("system"))
                        End Select

                    Else
                        If .author_id = bc_cs_central_settings.logged_on_user_id Then
                            item.SubItems.Add(CStr("user edit"))
                        Else
                            item.SubItems.Add(CStr("user read"))
                        End If
                    End If
                    For j = 0 To bc_am_load_objects.obc_pub_types.contributors.Count - 1
                        If bc_am_load_objects.obc_pub_types.contributors(j).id = .contributor_id Then
                            item.SubItems.Add(bc_am_load_objects.obc_pub_types.contributors(j).name)

                            Exit For
                        End If
                    Next
                    If .disabled = 1 Then
                        item.SubItems.Add("disabled")
                        item.Checked = False
                    ElseIf Format(.last_refresh_date.tolocaltime, "d-M-yyyy") = "9-9-9999" Then
                        item.SubItems.Add("await refresh")
                        item.Checked = True
                        bc_am_load_objects.obc_current_document.refresh_components.refresh_components(i).no_refresh = 0
                    Else
                        item.SubItems.Add(Format(.last_refresh_date.tolocaltime, "dd-MMM-yyyy HH:mm:ss"))
                        If .no_refresh = 0 Then
                            item.Checked = True
                        Else
                            item.Checked = False
                        End If
                    End If
                    If .last_update_date = "9-9-9999" Then
                        item.SubItems.Add("n/a")
                    Else
                        item.SubItems.Add(Format(.last_update_date.tolocaltime, "dd-MMM-yyyy HH:mm:ss"))
                    End If
                    REM if component has had its database linked servered then make sure it is always
                    If bc_am_load_objects.obc_current_document.refresh_components.refresh_components(i).disabled = 1 Then
                        bc_am_load_objects.obc_current_document.refresh_components.refresh_components(i).no_refresh = 1
                        item.Checked = False
                    End If
                    item.SubItems.Add(.locator)
                    ListView3.Items.Add(item)
                    REM set disabled
                End With
            Next

            Me.bgoto.Checked = False
            Me.bhighlight.Enabled = False
            Me.ComboBox1.Items.Clear()
            For i = 0 To bc_am_load_objects.obc_pub_types.contributors.Count - 1
                Me.ComboBox1.Items.Add(bc_am_load_objects.obc_pub_types.contributors(i).name)
            Next
            If ComboBox1.Items.Count < 2 Then
                ComboBox1.Enabled = False
                'default to first entry until item selected
                If ComboBox1.Items.Count = 1 Then
                    ComboBox1.SelectedIndex = 0
                End If
            End If
            sload = True
            REM language
            Me.ComboBox2.Items.Clear()
            For i = 0 To bc_am_load_objects.obc_pub_types.languages.Count - 1
                Me.ComboBox2.Items.Add(bc_am_load_objects.obc_pub_types.languages(i).name)
                If bc_am_load_objects.obc_pub_types.languages(i).id = bc_am_load_objects.obc_current_document.language_id Then
                    Me.ComboBox2.SelectedIndex = i
                End If
            Next
            If ComboBox2.Items.Count < 2 Then
                ComboBox2.Enabled = False
            End If

            REM financial workflow
            Me.lstages.Items.Clear()
            For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                If bc_am_load_objects.obc_pub_types.pubtype(i).id = bc_am_load_objects.obc_current_document.pub_type_id Then
                    For j = 0 To bc_am_load_objects.obc_pub_types.pubtype(i).financial_workflow.stages.count - 1
                        Me.lstages.Items.Add(bc_am_load_objects.obc_pub_types.pubtype(i).financial_workflow.stages(j).stage_name)
                        If bc_am_load_objects.obc_pub_types.pubtype(i).financial_workflow.stages(j).stage_id = bc_am_load_objects.obc_current_document.refresh_components.workflow_state Then
                            Me.lstages.SelectedIndex = j
                        End If
                    Next
                    Exit For
                End If
            Next

            If lstages.Items.Count < 2 Then
                lstages.Enabled = False
            End If

            REM data at date
            If Format(CDate(bc_am_load_objects.obc_current_document.refresh_components.data_at_date), "d-M-yyyy") <> "9-9-9999" Then
                bfrombutton2 = True
                bfrombutton1 = False
                Me.RadioButton2.Checked = True
                Me.DateTimePicker2.Value = bc_am_load_objects.obc_current_document.refresh_components.data_at_date
                Me.DateTimePicker2.Visible = True
                Me.RadioButton1.Checked = False
            Else
                bfrombutton1 = True
                bfrombutton2 = False
                Me.RadioButton1.Checked = True
                Me.RadioButton2.Checked = False
            End If
            sload = False
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_component_refresh", "load_components", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_component_refresh", "load_components", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Private Sub ListView3_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView3.SelectedIndexChanged
        Try
            If Not ListView3.SelectedItems(0) Is Nothing Then
                validateItems()
            End If

            If disable_refresh = False Then
                Me.bgoto.Enabled = True
            End If

            Me.bhighlight.Enabled = True

            If disable_contributer = False Then
                If ComboBox1.Items.Count > 1 Then
                    Me.ComboBox1.Enabled = True
                End If
            End If

            index = Me.ListView3.SelectedItems(0).Index

            If bc_am_load_objects.obc_current_document.refresh_components.refresh_components(index).disabled = 1 Then
                index = Me.ListView3.SelectedItems(0).Index
                Me.bgoto.Checked = True
                Me.bhighlight.Enabled = True
                Me.Label13.Text = "Refresh Disabled"
            End If

            If bc_am_load_objects.obc_current_document.refresh_components.refresh_components(index).disabled = 0 Then
                Me.bgoto.Checked = False
                With bc_am_load_objects.obc_current_document.refresh_components.refresh_components(index)
                    If .last_refresh_date = "9-9-9999" Then
                        Me.Label13.Text = "Await First Refresh"
                    Else
                        Me.Label13.Text = "Last Refresh: " + Format(bc_am_load_objects.obc_current_document.refresh_components.refresh_components(index).last_refresh_date, "dd-MMM-yyyy HH:mm:ss")
                        If bc_am_load_objects.obc_current_document.refresh_components.refresh_components(index).no_refresh = 0 Then
                            Me.Label13.Text = Me.Label13.Text + " Refresh Required."
                        Else
                            Me.Label13.Text = Me.Label13.Text + " No Refresh Needed."
                        End If
                    End If
                End With
            End If
            Me.TextBox2.Text = Format(bc_am_load_objects.obc_current_document.refresh_components.refresh_components(index).last_refresh_date, "dd-MMM-yyyy, HH:mm:ss")
            Me.TextBox2.Visible = True
            load_params()
            Dim i As Integer
            For i = 0 To bc_am_load_objects.obc_pub_types.contributors.Count - 1
                If bc_am_load_objects.obc_pub_types.contributors(i).id = bc_am_load_objects.obc_current_document.refresh_components.refresh_components(index).contributor_id Then
                    Me.ComboBox1.SelectedIndex = i
                    Exit For
                End If
            Next
        Catch

        Finally

        End Try
    End Sub
    Private Sub load_params()
        Try
            Me.GroupParamters.Visible = False
            Me.ListView3.Width = 530

            Me.has_context_items = False

            ListView2.Items.Clear()

            REM set contributor
            If bc_am_load_objects.obc_current_document.refresh_components.refresh_components(Me.ListView3.SelectedItems(0).Index).parameters.component_template_parameters.count > 0 Then
                index = Me.ListView3.SelectedItems(0).Index
                Dim i, j As Integer
                Me.GroupParamters.Text = "Parameters for: " + Replace(bc_am_load_objects.obc_current_document.refresh_components.refresh_components(Me.ListView3.SelectedItems(0).Index).name, "&", "&&")
                Me.GroupParamters.Visible = True
                Me.ListView3.Width = 272
                Me.ListView2.Enabled = True

                Dim item As New ListViewItem

                ListView2.combo.Visible = False
                Me.btnopen.Enabled = False
                Me.Button1.Enabled = False

                REM FIL JUNE 2012
                REM if a lookup from an SP and system defined 2 then get current value
                REM only allow this for a type 7 SP key value pair parameter
                For i = 0 To bc_am_load_objects.obc_current_document.refresh_components.refresh_components(Me.ListView3.SelectedItems(0).Index).parameters.component_template_parameters.count - 1
                    With bc_am_load_objects.obc_current_document.refresh_components.refresh_components(Me.ListView3.SelectedItems(0).Index).parameters.component_template_parameters(i)
                        REM never been loaded so get from server
                        If (.datatype = 7 And .system_defined = 2) Then
                            If bc_am_load_objects.obc_current_document.id = 0 Then
                                bc_am_load_objects.obc_current_document.refresh_components.refresh_components(Me.ListView3.SelectedItems(0).Index).doc_id = bc_am_load_objects.obc_current_document.filename
                            Else
                                bc_am_load_objects.obc_current_document.refresh_components.refresh_components(Me.ListView3.SelectedItems(0).Index).doc_id = bc_am_load_objects.obc_current_document.id
                            End If
                            'bc_am_load_objects.obc_current_document.refresh_components.locator_of_interest = bc_am_load_objects.obc_current_document.refresh_components.refresh_components(Me.ListView3.SelectedItems(0).Index).locator
                            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                                bc_am_load_objects.obc_current_document.refresh_components.refresh_components(Me.ListView3.SelectedItems(0).Index).read_system_defined_value()
                            Else
                                bc_am_load_objects.obc_current_document.refresh_components.refresh_components(Me.ListView3.SelectedItems(0).Index).tmode = bc_om_refresh_component.tREAD_SYSTEM_DEFINED_VALUE
                                If bc_am_load_objects.obc_current_document.refresh_components.refresh_components(Me.ListView3.SelectedItems(0).Index).transmit_to_server_and_receive(bc_am_load_objects.obc_current_document.refresh_components.refresh_components(Me.ListView3.SelectedItems(0).Index), True) = False Then
                                    Exit Sub
                                End If
                            End If
                            Exit For
                        End If
                    End With
                Next

                For i = 0 To bc_am_load_objects.obc_current_document.refresh_components.refresh_components(Me.ListView3.SelectedItems(0).Index).parameters.component_template_parameters.count - 1
                    With bc_am_load_objects.obc_current_document.refresh_components.refresh_components(Me.ListView3.SelectedItems(0).Index).parameters.component_template_parameters(i)
                        REM never been loaded so get from server
                        If (.datatype = 9) Then
                            has_context_items = True
                            context_items_pos = i
                            context_items_count = .context_items.table_items.Count
                            For j = 0 To .context_items.table_items.Count - 1
                                item = New ListViewItem(CStr(.context_items.table_items(j).orig_item_name))
                                If .context_items.table_items(j).system_defined = 0 Then
                                    item.SubItems.Add(CStr(.context_items.table_items(j).new_item_name))
                                Else
                                    item.SubItems.Add("System Defined")
                                End If
                                Me.ListView2.Items.Add(item)
                            Next
                        End If
                        If .datatype = 6 Then
                            Dim na As String
                            na = .name
                            With bc_am_load_objects.obc_current_document.refresh_components.oselection_parameters
                                If .row = 0 Or .col = 0 Or .value = "" Then
                                    na = "No item Selected"
                                Else
                                    na = na + " " + .value
                                End If
                            End With
                            item = New ListViewItem(CStr(na))
                        Else
                            item = New ListViewItem(CStr(.name))
                        End If
                        If .system_defined = 1 Then
                            item.SubItems.Add("System Defined")

                        ElseIf .system_defined = 2 Then
                            item.SubItems.Add(.default_value)
                            .original_system_defined = 0
                        Else
                            If .datatype = 6 Then
                                If load_apply = True Then
                                    item.SubItems.Add(.default_value)
                                Else
                                    .default_value = ""
                                    item.SubItems.Add("")
                                End If
                            Else
                                item.SubItems.Add(.default_value)
                            End If
                        End If
                        If .mandatory = True Then
                            Me.ListView2.SmallImageList = tabimages
                            item.ImageIndex = 9
                        End If
                        If .datatype <> 9 Then
                            Me.ListView2.Items.Add(item)
                        End If

                    End With
                    REM see if read only

                Next
            End If

            check_mandatory_values()
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_component_refresh", "load_params", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

        End Try
    End Sub
    REM FIL AUG 2013
    Private Sub check_mandatory_values()
        Me.btnopen.Enabled = False
        Me.Button1.Enabled = False
        Dim mandatory As Boolean = False
        For i = 0 To bc_am_load_objects.obc_current_document.refresh_components.refresh_components(Me.ListView3.SelectedItems(0).Index).parameters.component_template_parameters.count - 1
            With bc_am_load_objects.obc_current_document.refresh_components.refresh_components(Me.ListView3.SelectedItems(0).Index).parameters.component_template_parameters(i)
                If .mandatory = True And Trim(.default_value) = "" Then
                    mandatory = True
                    Exit For
                End If
            End With
        Next

        'If Me.ListBox1.Visible = True And bc_am_load_objects.obc_current_document.refresh_components.refresh_components(Me.ListView3.SelectedItems(0).Index).parameters.component_template_parameters(me.ListView2.SelectedItems(0).index).datatype <> 8 Then
        '    If Me.ListBox1.SelectedIndex = -1 Then
        '        Me.btnopen.Enabled = False
        '        Me.Button1.Enabled = False
        '        Exit Sub
        '    End If
        'End If

        If mandatory = False Then
            If master_refresh_disabled = False Then

                Me.btnopen.Enabled = True
            End If
            Me.Button1.Enabled = True
        End If
    End Sub
    Private Sub ListView2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView2.SelectedIndexChanged

        Try

            inputSetup()

            If Me.ListView2.SelectedItems.Count > 0 Then
                check_mandatory_values()
            End If


            '    If Me.DateTimePicker1.Visible = True Then
            '        setValue()
            '        Me.DateTimePicker1.Visible = False
            '    End If


            '    index2 = Me.ListView2.SelectedItems(0).Index
            '    index3 = Me.ListView2.SelectedItems(0).Index
            '    If Me.has_context_items = True Then
            '        If index3 > Me.context_items_pos + Me.context_items_count - 1 Then
            '            index3 = index3 - Me.context_items_count + 1
            '            index2 = index3
            '        ElseIf index3 >= Me.context_items_pos And index3 <= (Me.context_items_pos + Me.context_items_count - 1) Then
            '            index3 = Me.context_items_pos
            '            index2 = index3
            '        End If
            '    End If

            '    Me.TextBox1.Visible = False
            '    Me.DateTimePicker1.Visible = False
            '    Me.ListBox1.Visible = False

            '    Application.DoEvents()
            '    load_param_values()

            '    ' Make sure that an item is clicked.
            '    If Not (lvItem Is Nothing) Then
            '        lvItem.EnsureVisible()

            '        ' Get the bounds of the item that is clicked.
            '        Dim ClickedItem As Rectangle = lvItem.Bounds

            '        ' Verify that the column is completely scrolled off to the left.
            '        If ((ClickedItem.Left + Me.ListView2.Columns(1).Width) < 0) Then
            '            ' If the cell is out of view to the left, do nothing.
            '            Return

            '            ' Verify that the column is partially scrolled off to the left.
            '        ElseIf (ClickedItem.Left < 0) Then
            '            ' Determine if column extends beyond right side of ListView.
            '            If ((ClickedItem.Left + Me.ListView2.Columns(1).Width) > Me.ListView2.Width) Then
            '                ' Set width of column to match width of ListView.
            '                ClickedItem.Width = Me.ListView2.Width
            '                ClickedItem.X = 0
            '            Else
            '                ' Right side of cell is in view.
            '                ClickedItem.Width = Me.ListView2.Columns(1).Width + ClickedItem.Left
            '                ClickedItem.X = 2
            '            End If

            '        ElseIf (Me.ListView2.Columns(1).Width > Me.ListView2.Width) Then
            '            ClickedItem.Width = Me.ListView2.Width

            '        Else
            '            ClickedItem.Width = Me.ListView2.Columns(1).Width
            '            ClickedItem.X = 2
            '        End If

            '        ' Adjust the top to account for the location of the ListView.
            '        ClickedItem.Y += Me.ListView2.Top + 3
            '        ClickedItem.X += Me.ListView2.Left + ListView2.Columns(0).Width

            '        With bc_am_load_objects.obc_current_document.refresh_components.refresh_components(index).parameters.component_template_parameters(index2)
            '            Select Case .datatype
            '                Case 0, 2
            '                    ' Assign calculated bounds to the ComboBox.
            '                    TextBox1.Bounds = ClickedItem

            '                    ' Set default text for TextBox to match the item that is clicked.
            '                    If lvItem.SubItems.Count > 1 Then
            '                        TextBox1.Text = lvItem.SubItems(1).Text
            '                    End If

            '                    ' Display the TextBox, and make sure that it is on top with focus.
            '                    TextBox1.Visible = True
            '                    TextBox1.Enabled = True
            '                    TextBox1.BringToFront()
            '                    TextBox1.Focus()

            '                    If .disabled_in_doc = 1 Then
            '                        TextBox1.Enabled = False
            '                    End If
            '                Case 1

            '                    ' Assign calculated bounds to the ComboBox.
            '                    DateTimePicker1.Bounds = ClickedItem

            '                    ' Set default text for DatePicker to match the item that is clicked.
            '                    If lvItem.SubItems.Count > 1 AndAlso lvItem.SubItems(1).Text <> "System Defined" Then
            '                        DateTimePicker1.Value = lvItem.SubItems(1).Text
            '                    End If

            '                    ' Display the DatePicker, and make sure that it is on top with focus.
            '                    DateTimePicker1.Visible = True
            '                    DateTimePicker1.Enabled = True
            '                    DateTimePicker1.BringToFront()
            '                    DateTimePicker1.Focus()

            '                    If .disabled_in_doc = 1 Then
            '                        DateTimePicker1.Enabled = False
            '                    End If
            '                Case Else
            '                    ' Assign calculated bounds to the ComboBox.
            '                    ListBox1.Bounds = ClickedItem

            '                    ' Set default text for ComboBox to match the item that is clicked.
            '                    If lvItem.SubItems.Count > 1 Then
            '                        ListBox1.Text = lvItem.SubItems(1).Text
            '                    End If

            '                    ' Display the ComboBox, and make sure that it is on top with focus.
            '                    ListBox1.Enabled = True
            '                    ListBox1.Visible = True
            '                    ListBox1.BringToFront()
            '                    ListBox1.Focus()
            '                    If .disabled_in_doc = 1 Then
            '                        ListBox1.Enabled = False
            '                    End If

            '            End Select
            '            selectedItemIndex = Me.ListView2.SelectedItems(0).Index
            '        End With
            '    End If
            '    Me.prev_selected_index = Me.ListView2.SelectedItems(0).Index




        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("ListView2", "SelectedIndexChanged", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

        End Try
    End Sub


    Private Sub inputSetup()

        Try

            If Me.ListView2.SelectedItems.Count > 0 Then
                If Me.DateTimePicker1.Visible = True Then
                    setValue()
                    Me.DateTimePicker1.Visible = False
                End If


                index2 = Me.ListView2.SelectedItems(0).Index
                index3 = Me.ListView2.SelectedItems(0).Index
                If Me.has_context_items = True Then
                    If index3 > Me.context_items_pos + Me.context_items_count - 1 Then
                        index3 = index3 - Me.context_items_count + 1
                        index2 = index3
                    ElseIf index3 >= Me.context_items_pos And index3 <= (Me.context_items_pos + Me.context_items_count - 1) Then
                        index3 = Me.context_items_pos
                        index2 = index3
                    End If
                End If

                Me.TextBox1.Visible = False
                Me.DateTimePicker1.Visible = False
                Me.ListBox1.Visible = False

                Application.DoEvents()
                load_param_values()

                ' Make sure that an item is clicked.
                If Not (lvItem Is Nothing) Then
                    lvItem.EnsureVisible()

                    ' Get the bounds of the item that is clicked.
                    Dim ClickedItem As Rectangle = lvItem.Bounds

                    ' Verify that the column is completely scrolled off to the left.
                    If ((ClickedItem.Left + Me.ListView2.Columns(1).Width) < 0) Then
                        ' If the cell is out of view to the left, do nothing.
                        Return

                        ' Verify that the column is partially scrolled off to the left.
                    ElseIf (ClickedItem.Left < 0) Then
                        ' Determine if column extends beyond right side of ListView.
                        If ((ClickedItem.Left + Me.ListView2.Columns(1).Width) > Me.ListView2.Width) Then
                            ' Set width of column to match width of ListView.
                            ClickedItem.Width = Me.ListView2.Width
                            ClickedItem.X = 0
                        Else
                            ' Right side of cell is in view.
                            ClickedItem.Width = Me.ListView2.Columns(1).Width + ClickedItem.Left
                            ClickedItem.X = 2
                        End If

                    ElseIf (Me.ListView2.Columns(1).Width > Me.ListView2.Width) Then
                        ClickedItem.Width = Me.ListView2.Width

                    Else
                        ClickedItem.Width = Me.ListView2.Columns(1).Width
                        ClickedItem.X = 2
                    End If

                    ' Adjust the top to account for the location of the ListView.
                    ClickedItem.Y += Me.ListView2.Top + 3
                    ClickedItem.X += Me.ListView2.Left + ListView2.Columns(0).Width

                    With bc_am_load_objects.obc_current_document.refresh_components.refresh_components(index).parameters.component_template_parameters(index2)
                        Select Case .datatype
                            Case 0, 2
                                ' Assign calculated bounds to the ComboBox.
                                TextBox1.Bounds = ClickedItem

                                ' Set default text for TextBox to match the item that is clicked.
                                If lvItem.SubItems.Count > 1 Then
                                    TextBox1.Text = lvItem.SubItems(1).Text
                                End If

                                ' Display the TextBox, and make sure that it is on top with focus.
                                TextBox1.Visible = True
                                TextBox1.Enabled = True
                                TextBox1.BringToFront()
                                TextBox1.Focus()

                                If .disabled_in_doc = 1 Then
                                    TextBox1.Enabled = False
                                End If
                            Case 1

                                ' Assign calculated bounds to the ComboBox.
                                DateTimePicker1.Bounds = ClickedItem

                                ' Set default text for DatePicker to match the item that is clicked.
                                If lvItem.SubItems.Count > 1 AndAlso lvItem.SubItems(1).Text <> "System Defined" Then
                                    DateTimePicker1.Value = lvItem.SubItems(1).Text
                                End If

                                ' Display the DatePicker, and make sure that it is on top with focus.
                                DateTimePicker1.Visible = True
                                DateTimePicker1.Enabled = True
                                DateTimePicker1.BringToFront()
                                DateTimePicker1.Focus()

                                If .disabled_in_doc = 1 Then
                                    DateTimePicker1.Enabled = False
                                End If
                            Case Else
                                ' Assign calculated bounds to the ComboBox.
                                ListBox1.Bounds = ClickedItem

                                ' Set default text for ComboBox to match the item that is clicked.
                                If lvItem.SubItems.Count > 1 Then
                                    ListBox1.Text = lvItem.SubItems(1).Text
                                End If

                                ' Display the ComboBox, and make sure that it is on top with focus.
                                ListBox1.Enabled = True
                                ListBox1.Visible = True
                                ListBox1.BringToFront()
                                ListBox1.Focus()
                                If .disabled_in_doc = 1 Then
                                    ListBox1.Enabled = False
                                End If

                        End Select
                        selectedItemIndex = Me.ListView2.SelectedItems(0).Index
                    End With
                End If
                Me.prev_selected_index = Me.ListView2.SelectedItems(0).Index
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_component_refresh", "inputSetup", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

        End Try
    End Sub








    Private Sub load_param_values()
        Try
            Dim i As Integer

            Me.TextBox1.Visible = False
            Me.DateTimePicker1.Visible = False

            Me.ListBox1.DropDownStyle = ComboBoxStyle.DropDownList
            With bc_am_load_objects.obc_current_document.refresh_components.refresh_components(index).parameters.component_template_parameters(index2)

                Me.ListBox1.BeginUpdate()
                Me.ListBox1.Items.Clear()

                If .original_system_defined = 1 Then
                    ListBox1.Items.Add("System Defined")
                End If

                Select Case .datatype
                    REM string numeric           
                    Case 0, 2
                        Me.TextBox1.Visible = True
                        Me.TextBox1.Text = .default_value
                        REM boolean
                    Case 1
                        REM date
                        Me.DateTimePicker1.Visible = True
                        If IsDate(.default_value) Then
                            Me.DateTimePicker1.Value = .default_value
                        End If
                    Case 3
                        REM boolean 
                        Me.ListBox1.Items.Add("True")
                        Me.ListBox1.Items.Add("False")
                        Me.ListBox1.SelectedIndex = 0
                        If .default_value = "False" Then
                            Me.ListBox1.SelectedIndex = 1
                        End If
                    Case 4
                        REM class
                        For i = 0 To bc_am_load_objects.obc_entities.entity.Count - 1
                            If bc_am_load_objects.obc_entities.entity(i).class_id = .class_id Then
                                Me.ListBox1.Items.Add(bc_am_load_objects.obc_entities.entity(i).name)
                                If .default_value = bc_am_load_objects.obc_entities.entity(i).name Then
                                    Me.ListBox1.SelectedIndex = Me.ListBox1.Items.Count - 1
                                End If
                            End If
                        Next
                        REM FIL AUG 2013
                    Case 5, 6, 7, 8, 9, 10
                        REM lookup, Document selection
                        REM if document selection default value is selected item

                        Me.ListBox1.DropDownStyle = ComboBoxStyle.DropDown
                        'If .datatype <> 8 Then
                        ListBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend
                        ListBox1.AutoCompleteSource = AutoCompleteSource.ListItems
                        'Else
                        'ListBox1.AutoCompleteMode = AutoCompleteMode.None
                        'ListBox1.AutoCompleteSource = AutoCompleteSource.None
                        'End If

                        If .datatype = 6 Then
                            '.default_value = bc_ao_word.get_selected_cell_item()
                        End If
                        If .datatype = 8 Then
                            REM Me.ListBox1.DropDownStyle = ComboBoxStyle.DropDown
                            Me.ListBox1.Text = ""
                        End If
                        REM if dynamic lookup and document is new request lookup list from server
                        REM FIL AUG 2013
                        If (.datatype = 7 Or .datatype = 10) Then

                            REM And bc_am_load_objects.obc_current_document.id = 0 Then
                            If bc_am_load_objects.obc_current_document.connection_method = bc_cs_central_settings.LOCAL Then
                                Dim omsg As New bc_cs_message("Blue Curve", "Server not connected so cant populate dynamic list", bc_cs_message.MESSAGE)
                                Exit Sub
                            End If
                            Dim oparam_values As New bc_om_parameter_lookup
                            If .datatype = 10 Then
                                For i = 0 To bc_am_load_objects.obc_current_document.refresh_components.refresh_components(index).parameters.component_template_parameters.count - 1
                                    If bc_am_load_objects.obc_current_document.refresh_components.refresh_components(index).parameters.component_template_parameters(i).datatype = 10 Then
                                        oparam_values.dependent_parameter_values.Add(bc_am_load_objects.obc_current_document.refresh_components.refresh_components(index).parameters.component_template_parameters(i).default_value_id)
                                    End If
                                Next
                            End If
                            oparam_values = Me.get_lookup_vals(.datatype, .lookup_sql, oparam_values.dependent_parameter_values)
                            .lookup_values.clear()
                            .lookup_values_ids.clear()
                            .lookup_values = oparam_values.lookup_vals
                            .lookup_values_ids = oparam_values.lookup_vals_ids

                        End If
                        If .lookup_values_ids.count > 0 Then
                            For i = 0 To .lookup_values_ids.count - 1
                                Me.ListBox1.Items.Add(.lookup_values_ids(i))
                                If .datatype <> 9 Then
                                    If .lookup_values_ids(i) = .default_value And (.datatype = 5 Or .datatype = 7 Or .datatype = 8) Then
                                        Me.ListBox1.SelectedIndex = Me.ListBox1.Items.Count - 1
                                    End If
                                End If
                                If .datatype = 9 Then
                                    If Me.ListView2.SelectedItems.Count > 0 Then
                                        If .lookup_values(i) = bc_am_load_objects.obc_current_document.refresh_components.refresh_components(index).parameters.component_template_parameters(index3).context_items.table_items(Me.ListView2.SelectedItems(0).Index - Me.context_items_pos).new_item_id And .datatype = 9 Then
                                            Me.ListBox1.SelectedIndex = Me.ListBox1.Items.Count - 1
                                        End If
                                    End If
                                End If
                            Next
                        Else
                            For i = 0 To .lookup_values.count - 1
                                Me.ListBox1.Items.Add(.lookup_values(i))
                                If .lookup_values(i) = .default_value And (.datatype = 5 Or .datatype = 7 Or .datatype = 8) Then
                                    Me.ListBox1.SelectedIndex = Me.ListBox1.Items.Count - 1
                                End If
                            Next
                        End If
                End Select
            End With
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_component_refresh", "load_param_values", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.ListBox1.EndUpdate()
            REM Application.DoEvents()
        End Try
    End Sub
    Private Function get_lookup_vals(ByVal datatype As Long, ByVal lookup_sql As String, ByVal dependent_parameter_values As List(Of String)) As bc_om_parameter_lookup
        Dim oparam_values As New bc_om_parameter_lookup
        oparam_values.type_id = datatype
        oparam_values.lookup_sql = lookup_sql
        oparam_values.entity_id = bc_am_load_objects.obc_current_document.entity_id
        oparam_values.doc_id = bc_am_load_objects.obc_current_document.filename
        oparam_values.dependent_parameter_values = dependent_parameter_values

        REM assune context_id is parameter 1
        Try
            oparam_values.context_id = bc_am_load_objects.obc_current_document.refresh_components.refresh_components(index).parameters.component_template_parameters(0).default_value_id
        Catch

        End Try
        Me.Cursor = Cursors.WaitCursor

        If bc_am_load_objects.obc_current_document.connection_method = bc_cs_central_settings.ADO Then
            oparam_values.db_read()
        Else
            oparam_values.tmode = bc_cs_soap_base_class.tREAD
            oparam_values.transmit_to_server_and_receive(oparam_values, True)
        End If
        Me.Cursor = Cursors.Default
        get_lookup_vals = oparam_values


    End Function
    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        Dim i As Integer

        If Me.CheckBox1.Checked = True Then
            For i = 0 To bc_am_load_objects.obc_current_document.refresh_components.refresh_components.Count - 1
                If bc_am_load_objects.obc_current_document.refresh_components.refresh_components(i).disabled = 0 Then
                    bc_am_load_objects.obc_current_document.refresh_components.refresh_components(i).no_refresh = 0
                End If
            Next
            Me.CheckBox1.Enabled = False
        End If
    End Sub
    Private Sub setValue()
        Dim slog As New bc_cs_activity_log("bc_am_component_refresh", "setValue", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim valid_item As Boolean = True

            With bc_am_load_objects.obc_current_document.refresh_components.refresh_components(index).parameters.component_template_parameters(index2)
                Select Case .datatype
                    Case 0, 2
                        .default_value = Me.TextBox1.Text

                    Case 1
                        .default_value = Me.DateTimePicker1.Value
                    Case 3, 4, 5, 7, 8, 10
                        valid_item = False
                        If ListBox1.Text <> "System Defined" Then
                            If Me.ListBox1.SelectedIndex > -1 Then
                                If .datatype <> 8 Then
                                    If .lookup_values_ids.count > 0 Then
                                        REM key value pairs
                                        For i = 0 To .lookup_values_ids.count - 1
                                            If .lookup_values_ids(i) = ListBox1.Text Then
                                                .default_value_id = .lookup_values(i)
                                                .default_value = Me.ListBox1.Text
                                                valid_item = True
                                                Exit For
                                            End If
                                        Next
                                    Else
                                        REM non key value pairs
                                        For i = 0 To .lookup_values.count - 1
                                            If .lookup_values(i) = ListBox1.Text Then
                                                .default_value = Me.ListBox1.Text
                                                valid_item = True
                                                Exit For
                                            End If
                                        Next
                                    End If

                                    '.default_value = Me.ListBox1.Text
                                    ''MsgBox(.default_value())
                                    'If .datatype <> 8 Then
                                    '    If .lookup_values_ids.count > 0 Then
                                    '        .default_value_id = .lookup_values(Me.ListBox1.SelectedIndex - .original_system_defined)
                                    '    End If
                                    'Else
                                    '    .default_value_id = ""
                                    'End If
                                Else
                                    .default_value = Me.ListBox1.Text
                                    valid_item = True
                                End If
                            Else
                                If .datatype = 8 Then
                                    .default_value = Me.ListBox1.Text
                                    valid_item = True
                                End If
                            End If
                            If valid_item = False Then
                                Me.btnopen.Enabled = False
                                Me.Button1.Enabled = False

                            End If
                        End If
                        REM if a type 10 blank out all other type 10s after this parameter
                        If .datatype = 10 Then
                            For i = index2 + 1 To bc_am_load_objects.obc_current_document.refresh_components.refresh_components(index).parameters.component_template_parameters.count - 1
                                If bc_am_load_objects.obc_current_document.refresh_components.refresh_components(index).parameters.component_template_parameters(i).datatype = 10 Then
                                    bc_am_load_objects.obc_current_document.refresh_components.refresh_components(index).parameters.component_template_parameters(i).default_value = ""
                                    bc_am_load_objects.obc_current_document.refresh_components.refresh_components(index).parameters.component_template_parameters(i).default_value_id = 0
                                    Me.ListView2.Items(i).SubItems(1).Text = ""
                                End If
                            Next
                        End If

                    Case 6
                        If ListBox1.Text <> "System Defined" Then
                            .default_value = Me.ListBox1.Text
                            If .lookup_values_ids.count > 0 Then
                                .default_value_id = .lookup_values(Me.ListBox1.SelectedIndex - .original_system_defined)
                            End If
                        End If
                        load_apply = True
                    Case 9
                        If ListBox1.Text <> "System Defined" Then
                            bc_am_load_objects.obc_current_document.refresh_components.refresh_components(index).parameters.component_template_parameters(index3).context_items.table_items(Me.ListView2.SelectedItems(0).Index - Me.context_items_pos).system_defined = 0
                            bc_am_load_objects.obc_current_document.refresh_components.refresh_components(index).parameters.component_template_parameters(index3).context_items.table_items(Me.ListView2.SelectedItems(0).Index - Me.context_items_pos).new_item_name = Me.ListBox1.Text
                            If .datatype <> 8 Then
                                If .lookup_values_ids.count > 0 Then
                                    bc_am_load_objects.obc_current_document.refresh_components.refresh_components(index).parameters.component_template_parameters(index3).context_items.table_items(Me.ListView2.SelectedItems(0).Index - Me.context_items_pos).new_item_id = .lookup_values(Me.ListBox1.SelectedIndex - .original_system_defined)
                                End If
                            End If
                        Else
                            bc_am_load_objects.obc_current_document.refresh_components.refresh_components(index).parameters.component_template_parameters(index3).context_items.table_items(Me.ListView2.SelectedItems(0).Index - Me.context_items_pos).system_defined = 1
                            bc_am_load_objects.obc_current_document.refresh_components.refresh_components(index).parameters.component_template_parameters(index3).context_items.table_items(Me.ListView2.SelectedItems(0).Index - Me.context_items_pos).new_item_name = ""
                            If .datatype <> 8 Then
                                If .lookup_values_ids.count > 0 Then
                                    bc_am_load_objects.obc_current_document.refresh_components.refresh_components(index).parameters.component_template_parameters(index3).context_items.table_items(Me.ListView2.SelectedItems(0).Index - Me.context_items_pos).new_item_id = 0
                                End If
                            End If
                        End If

                End Select
                If ListBox1.Text = "System Defined" And .datatype <> 9 Then
                    .system_defined = 1
                    .default_value = ""
                    .default_value_id = ""
                Else
                    .system_defined = 0
                End If
                REM reload data
                checked = False
                'load_components()
                'Me.ListView3.Items(index).Selected = True
                'load_params()
                'Me.ListView3.Width = 272
                'Me.GroupParamters.Visible = True

                bc_am_load_objects.obc_current_document.refresh_components.refresh_components(index).last_update_date = Format(Date.Now, "dd-MMM-yyyy HH:mm:ss")
                bc_am_load_objects.obc_current_document.refresh_components.refresh_components(index).no_refresh = 0
                ListView3.Items(index).SubItems(4).Text = bc_am_load_objects.obc_current_document.refresh_components.refresh_components(index).last_update_date
                ListView3.Items(index).Checked = True

                Select Case .datatype
                    Case 0, 2
                        ListView2.Items(selectedItemIndex).SubItems(1).Text = TextBox1.Text
                        TextBox1.Visible = False
                    Case 1
                        ListView2.Items(selectedItemIndex).SubItems(1).Text = DateTimePicker1.Value
                        DateTimePicker1.Visible = False
                    Case Else
                        ListView2.Items(selectedItemIndex).SubItems(1).Text = ListBox1.Text
                        If valid_item = True And bc_am_load_objects.obc_current_document.refresh_components.refresh_components(index).parameters.component_template_parameters(index2).datatype <> 8 Then
                            ListBox1.Visible = False
                        End If
                End Select

            End With
            REM FIL AUG 2013
            If valid_item = True Then
                check_mandatory_values()
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_component_refresh", "setValue", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_component_refresh", "setValue", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Private Sub ListView3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListView3.Click

        Try
            bc_am_load_objects.obc_current_document.refresh_components.refresh_components(Me.ListView3.SelectedItems(0).Index).no_refresh = 1
            If Me.ListView3.SelectedItems(0).Checked = True Then
                bc_am_load_objects.obc_current_document.refresh_components.refresh_components(Me.ListView3.SelectedItems(0).Index).no_refresh = 0
            End If
        Catch

        Finally

        End Try
    End Sub
    Private Sub CheckBox3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bgoto.CheckedChanged
        REM disabled flag
        index = Me.ListView3.SelectedItems(0).Index
        If Me.bgoto.Checked = False Then
            bc_am_load_objects.obc_current_document.refresh_components.refresh_components(index).disabled = 0
            If component_mode = True Then
                Me.btnopen.Enabled = True
            End If
            With bc_am_load_objects.obc_current_document.refresh_components.refresh_components(index)
                If .last_refresh_date = "9-9-9999" Then
                    Me.Label13.Text = "Await First Refresh"
                Else
                    Me.Label13.Text = "Last Refresh: " + Format(bc_am_load_objects.obc_current_document.refresh_components.refresh_components(index).last_refresh_date, "dd-MMM-yyyy HH:mm:ss")
                End If
            End With
            ListView3.Items(index).SubItems(3).Text = Format(bc_am_load_objects.obc_current_document.refresh_components.refresh_components(index).last_refresh_date, "dd-MMM-yyyy HH:mm:ss")
        Else
            Me.Label13.Text = "Refresh Disabled"
            If component_mode = True Then
                Me.btnopen.Enabled = False
            End If
            bc_am_load_objects.obc_current_document.refresh_components.refresh_components(index).disabled = 1
            ListView3.Items(index).SubItems(3).Text = "Refresh Disabled"
        End If
    End Sub
    Private Sub btnopen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnopen.Click
        If Me.DateTimePicker1.Visible = True Then
            setValue()
            Me.DateTimePicker1.Visible = False
        End If
        If Not validateItems() Then
            Exit Sub
        End If
        If Me.trtf.Visible = True Then
            Dim omsg As New bc_cs_message("Blue Curve", "Component will be refreshed from preview. If you have overtyped component in document changes will be lost. Proceed?", bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
            If omsg.cancel_selected = True Then
                Exit Sub
            End If
        End If
        ok_selected = True
        cancel_selected = False
        set_checks()
        If IsNumeric(bc_am_load_objects.obc_current_document.id) And bc_am_load_objects.obc_current_document.id > 0 Then
            bc_am_load_objects.obc_current_document.write_data_to_file(bc_cs_central_settings.local_repos_path + CStr(bc_am_load_objects.obc_current_document.id) + ".dat")
        Else
            If bc_am_load_objects.obc_current_document.filename.Substring(bc_am_load_objects.obc_current_document.filename.Length - 4, 4) <> bc_am_load_objects.obc_current_document.extension Then
                bc_am_load_objects.obc_current_document.write_data_to_file(bc_cs_central_settings.local_repos_path + bc_am_load_objects.obc_current_document.filename + ".dat")
            Else
                bc_am_load_objects.obc_current_document.write_data_to_file(bc_cs_central_settings.local_repos_path + bc_am_load_objects.obc_current_document.filename.Substring(0, bc_am_load_objects.obc_current_document.filename.Length - 4) + ".dat")
            End If
        End If
        Me.Hide()
    End Sub
    Private Sub set_checks()
        Dim i As Integer
        For i = 0 To ListView3.Items.Count - 1
            If ListView3.Items(i).Checked = True Then
                bc_am_load_objects.obc_current_document.refresh_components.refresh_components(Me.ListView3.Items(i).Index).no_refresh = 0
            Else
                bc_am_load_objects.obc_current_document.refresh_components.refresh_components(Me.ListView3.Items(i).Index).no_refresh = 1
            End If
        Next
    End Sub
    Private Sub ComboBox2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox2.SelectedIndexChanged
        If sload = True Then
            Exit Sub
        End If
        Dim i As Integer
        For i = 0 To bc_am_load_objects.obc_pub_types.languages.Count - 1
            If i = Me.ComboBox2.SelectedIndex Then
                bc_am_load_objects.obc_current_document.language_id = bc_am_load_objects.obc_pub_types.languages(i).id
            End If
        Next
        update_all()
    End Sub
    Private Sub update_all()
        Dim i As Integer
        Dim da As String
        da = Format(Date.Now, "dd-MMM-yyyy HH:mm:ss")
        For i = 0 To bc_am_load_objects.obc_current_document.refresh_components.refresh_components.Count - 1
            bc_am_load_objects.obc_current_document.refresh_components.refresh_components(i).last_update_date = da
            bc_am_load_objects.obc_current_document.refresh_components.refresh_components(i).no_refresh = 0
            Me.ListView3.Items(i).SubItems(4).Text = da
            Me.ListView3.Items(i).Checked = True
        Next
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        ok_selected = False
        cancel_selected = True
        Me.Hide()
    End Sub
    Private Sub lstages_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstages.SelectedIndexChanged
        If sload = True Then
            Exit Sub
        End If
        Dim i As Integer
        For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
            If bc_am_load_objects.obc_pub_types.pubtype(i).id = bc_am_load_objects.obc_current_document.pub_type_id Then
                bc_am_load_objects.obc_current_document.refresh_components.workflow_state = bc_am_load_objects.obc_pub_types.pubtype(i).financial_workflow.stages(Me.lstages.SelectedIndex).stage_id
            End If
        Next
        update_all()
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged

        If ListView3.SelectedItems.Count <> 0 Then
            index = Me.ListView3.SelectedItems(0).Index
            bc_am_load_objects.obc_current_document.refresh_components.refresh_components(index).contributor_id = bc_am_load_objects.obc_pub_types.contributors(Me.ComboBox1.SelectedIndex).id
            bc_am_load_objects.obc_current_document.refresh_components.refresh_components(index).last_update_date = Format(Now, "dd-MMM-yyyy HH:mm:ss")
            ListView3.Items(index).SubItems(4).Text = bc_am_load_objects.obc_current_document.refresh_components.refresh_components(index).last_update_date
            ListView3.Items(index).SubItems(2).Text = Me.ComboBox1.Text
            bc_am_load_objects.obc_current_document.refresh_components.refresh_components(index).last_update_date = Format(Now, "dd-MMM-yyyy HH:mm:ss")
            ListView3.Items(index).SubItems(4).Text = bc_am_load_objects.obc_current_document.refresh_components.refresh_components(index).last_update_date
            ListView3.Items(index).Checked = True
        End If
    End Sub

    Private Sub RadioButton1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton1.CheckedChanged


        Me.DateTimePicker2.Visible = False
        bc_am_load_objects.obc_current_document.refresh_components.data_at_date = "9-9-9999"
        Me.RadioButton2.Checked = False
        bfrombutton1 = True
    End Sub
    Private Sub RadioButton2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton2.CheckedChanged
        Me.DateTimePicker2.Visible = True
        If bc_am_load_objects.obc_current_document.refresh_components.data_at_date <> "9-9-9999" Then
            Me.DateTimePicker2.Value = bc_am_load_objects.obc_current_document.refresh_components.data_at_date
        End If
    End Sub
    Private Sub DateTimePicker2_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateTimePicker2.ValueChanged

        Dim dateAtDate As Date

        dateAtDate = DateSerial(DatePart(DateInterval.Year, Me.DateTimePicker2.Value), _
                                            DatePart(DateInterval.Month, Me.DateTimePicker2.Value), _
                                            DatePart(DateInterval.Day, Me.DateTimePicker2.Value))

        bc_am_load_objects.obc_current_document.refresh_components.data_at_date = DateAdd(DateInterval.Second, 86399, dateAtDate).ToString

    End Sub

    Private Sub ListView2_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ListView2.MouseDown

        ' Get the item on the row that is clicked.
        lvItem = Me.ListView2.GetItemAt(e.X, e.Y)

        If Not lvItem Is Nothing Then

            If Me.ListView2.Items(lvItem.Index).Selected = True Then
                inputSetup()
                check_mandatory_values()
            End If
        End If

    End Sub

    Private Sub ListBox1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ListBox1.KeyDown
        ListBox1.DroppedDown = False
    End Sub

    Private Sub item_not_valid(ByVal Item As String)
        'Dim omsg As New bc_cs_message("Blue Curve", "Item: " + Item + " not valud must be an item in the list", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
        Me.btnopen.Enabled = False
        Me.Button1.Enabled = False



    End Sub

    Private Sub ListBox1_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListBox1.Leave

        ' check_list_item_is_valid(me.ListBox1.SelectedIndex)
        'Dim currentText As String = ""

        'Try

        '    If Not IsNothing(ListBox1.SelectedItem) Then
        '        currentText = ListBox1.SelectedItem
        '    End If
        '    If UCase(currentText) <> UCase(ListBox1.Text) Then
        '        ListBox1.Text = ListBox1.Text
        '        If Not IsNothing(ListBox1.SelectedItem) Then
        '            currentText = ListBox1.SelectedItem
        '        End If

        '        If UCase(currentText) <> UCase(ListBox1.Text) And bc_am_load_objects.obc_current_document.refresh_components.refresh_components(index).parameters.component_template_parameters(index2).datatype <> 8 Then
        '            Me.ListBox1.Focus()
        '            'item_not_valid(ListBox1.Text)
        '            Exit Sub
        '        End If
        '    End If

        '    REM FIL AUG 2013
        '    'If Trim(CStr((ListBox1.Text))) <> "" AndAlso ListView2.SelectedItems.Count > 0 AndAlso Trim(CStr((ListBox1.Text))) <> Trim(ListView2.SelectedItems(0).SubItems(1).Text) Then
        '    If ListView2.SelectedItems.Count > 0 AndAlso Trim(CStr((ListBox1.Text))) <> Trim(ListView2.SelectedItems(0).SubItems(1).Text) Then
        '        setValue()
        '    End If

        'Catch ex As Exception
        '    Exit Sub
        'Finally

        'End Try

    End Sub
    'Private Sub listbox_leave()
    '    Dim currentText As String = ""

    '    Try

    '        If Not IsNothing(ListBox1.SelectedItem) Then
    '            currentText = ListBox1.SelectedItem
    '        End If
    '        If UCase(currentText) <> UCase(ListBox1.Text) Then
    '            ListBox1.Text = ListBox1.Text
    '            If Not IsNothing(ListBox1.SelectedItem) Then
    '                currentText = ListBox1.SelectedItem
    '            End If
    '            If UCase(currentText) <> UCase(ListBox1.Text) And bc_am_load_objects.obc_current_document.refresh_components.refresh_components(index).parameters.component_template_parameters(index2).datatype <> 8 Then

    '                ListBox1.Focus()
    '                'item_not_valid(ListBox1.Text)
    '                Exit Sub
    '            End If
    '        End If

    '        REM FIL AUG 2013
    '        'If Trim(CStr((ListBox1.Text))) <> "" AndAlso ListView2.SelectedItems.Count > 0 AndAlso Trim(CStr((ListBox1.Text))) <> Trim(ListView2.SelectedItems(0).SubItems(1).Text) Then
    '        If ListView2.SelectedItems.Count > 0 AndAlso Trim(CStr((ListBox1.Text))) <> Trim(ListView2.SelectedItems(0).SubItems(1).Text) Then
    '            setValue()
    '        End If

    '    Catch
    '        Exit Sub
    '    Finally

    '    End Try

    'End Sub

    Private Sub ListView2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles ListView2.KeyPress


        e.Handled = True
    End Sub

    Private Sub ListView2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ListView2.KeyDown

        e.Handled = True
    End Sub

    Private Sub TextBox1_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox1.Leave

        If Trim(TextBox1.Text) <> "" AndAlso ListView2.SelectedItems.Count > 0 AndAlso Trim(TextBox1.Text) <> Trim(ListView2.SelectedItems(0).SubItems(1).Text) Then
            setValue()
        End If

    End Sub

    Private Sub DateTimePicker1_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles DateTimePicker1.Leave


        'If Trim(DateTimePicker1.Value) <> "" AndAlso ListView2.SelectedItems.Count > 0 AndAlso Trim(DateTimePicker1.Value) <> Trim(ListView2.SelectedItems(0).SubItems(1).Text) Then

        'setValue()
        'End If

    End Sub

    Private Function validateItems() As Boolean

        Dim slog As New bc_cs_activity_log("bc_am_component_refresh", "validateItems", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim lvi As ListViewItem
            Dim lvi2 As ListViewItem = Nothing
            Dim itemName As String
            Dim itemIndex As Integer
            Dim foundDuplicate As Boolean = False
            Dim tmpIndex1 As Integer
            Dim tmpIndex2 As Integer
            REM PR dec 2009 only do in type 9 parameters
            validateItems = True

            For Each lvi In ListView2.Items
                itemName = lvi.SubItems(1).Text
                itemIndex = lvi.Index

                tmpIndex1 = lvi.Index
                tmpIndex2 = lvi.Index
                If Me.has_context_items = True Then
                    If tmpIndex2 > Me.context_items_pos + Me.context_items_count - 1 Then
                        tmpIndex2 = tmpIndex2 - Me.context_items_count + 1
                        tmpIndex1 = tmpIndex2
                    ElseIf tmpIndex2 >= Me.context_items_pos And tmpIndex2 <= (Me.context_items_pos + Me.context_items_count - 1) Then
                        tmpIndex2 = Me.context_items_pos
                        tmpIndex1 = tmpIndex2
                    End If
                End If

                If bc_am_load_objects.obc_current_document.refresh_components.refresh_components(index).parameters.component_template_parameters(tmpIndex1).datatype = 9 Then
                    For Each lvi2 In ListView2.Items
                        'If bc_am_load_objects.obc_current_document.refresh_components.refresh_components(index).parameters.component_template_parameters(lvi2.Index).datatype = 9 Then
                        If itemName = lvi2.SubItems(1).Text And lvi2.Index <> itemIndex And lvi2.SubItems(1).Text <> "System Defined" Then
                            foundDuplicate = True
                            Exit For
                        End If
                        'End If
                    Next
                    If foundDuplicate Then
                        Dim omessage = New bc_cs_message("Blue Curve - create", "Duplicate item " + itemName + " found. Original item is " + lvi2.Text, bc_cs_message.MESSAGE)
                        validateItems = False
                        Exit For
                    End If
                End If
            Next

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_component_refresh", "validateItems", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_component_refresh", "validateItems", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

    Private Sub ListView3_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ListView3.MouseUp

        If ListView3.SelectedItems.Count = 0 Then
            bgoto.Enabled = False
            Me.bhighlight.Enabled = False
        End If

    End Sub

    Private Sub Label13_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label13.Click

    End Sub

    Private Sub Label3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label3.Click

    End Sub

    Private Sub chighsdc_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chighsdc.CheckedChanged

        If Me.chighsdc.Checked = True Then
            Me.Rall.Enabled = True
            Me.Redit.Enabled = True
        Else
            Me.Rall.Enabled = False
            Me.Redit.Enabled = False
            Me.Rall.Checked = False
            Me.Redit.Checked = False
            Dim bk_colour As Integer = 0
            If loading = True Then
                Exit Sub
            End If
            Try
                Me.Cursor = Cursors.WaitCursor
                ao_object.disable_screen_updating()
                highlight_selected = True

                With bc_am_load_objects.obc_current_document
                    For i = 0 To .refresh_components.refresh_components.Count - 1
                        If .refresh_components.refresh_components(i).original_markup_colour_index <> 0 Then
                            ao_object.set_background_colour_for_locator(.refresh_components.refresh_components(i).locator, .refresh_components.refresh_components(i).original_markup_colour_index, .refresh_components.refresh_components(i).mode)
                            .refresh_components.refresh_components(i).original_markup_colour_index = 0
                            ao_object.update_refresh_status_bar("unhighlight component ", CStr(i + 1), CStr(.refresh_components.refresh_components.Count))
                        End If
                    Next
                    .refresh_components.show_sdcs = False
                    .refresh_components.show_udcs = False
                End With
                save()
            Catch ex As Exception
                Dim oerr As New bc_cs_error_log("chighsdc", "checkedchanged", bc_cs_error_codes.USER_DEFINED, ex.Message)

            Finally
                REM save the medata file

                ao_object.enable_screen_updating()
                ao_object.update_refresh_status_bar("Complete", 0, 0)
                Me.Cursor = Cursors.Default
            End Try
        End If
    End Sub

    Private Sub bhighlight_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bhighlight.Click
        ao_object.highlight_locator(bc_am_load_objects.obc_current_document.refresh_components.refresh_components(ListView3.SelectedItems(0).Index).locator, bc_am_load_objects.obc_current_document.refresh_components.refresh_components(ListView3.SelectedItems(0).Index).mode)
    End Sub

    Private Sub highlight_edit()
        Dim bk_colour As Integer = 0
        If loading = True Then
            Exit Sub
        End If
        Try
            Me.Cursor = Cursors.WaitCursor
            ao_object.disable_screen_updating()
            highlight_selected = True
            With bc_am_load_objects.obc_current_document

                For i = 0 To .refresh_components.refresh_components.Count - 1
                    If (InStr(.refresh_components.refresh_components(i).locator, "_udc") > 0 And .refresh_components.refresh_components(i).author_Id = bc_cs_central_settings.logged_on_user_id) Or .refresh_components.refresh_components(i).mode = 8 Or .refresh_components.refresh_components(i).mode = 9 Then
                        bk_colour = ao_object.get_background_color_for_locator(.refresh_components.refresh_components(i).locator)
                        If bk_colour <> udc_colour_index And .refresh_components.refresh_components(i).original_markup_colour_index = 0 Then
                            .refresh_components.refresh_components(i).original_markup_colour_index = bk_colour
                            ao_object.set_background_colour_for_locator(.refresh_components.refresh_components(i).locator, udc_colour_index, .refresh_components.refresh_components(i).mode)
                            ao_object.update_refresh_status_bar("highlight component ", CStr(i + 1), CStr(.refresh_components.refresh_components.Count))
                        End If
                    End If
                Next

                For i = 0 To .refresh_components.refresh_components.Count - 1
                    If InStr(.refresh_components.refresh_components(i).locator, "_at") > 0 And .refresh_components.refresh_components(i).mode <> 8 And .refresh_components.refresh_components(i).mode <> 9 Or (InStr(.refresh_components.refresh_components(i).locator, "_udc") > 0 And .refresh_components.refresh_components(i).author_id <> bc_cs_central_settings.logged_on_user_id) Then
                        If .refresh_components.refresh_components(i).original_markup_colour_index <> 0 Then
                            ao_object.set_background_colour_for_locator(.refresh_components.refresh_components(i).locator, .refresh_components.refresh_components(i).original_markup_colour_index, .refresh_components.refresh_components(i).mode)
                            .refresh_components.refresh_components(i).original_markup_colour_index = 0
                            ao_object.update_refresh_status_bar("unhighlight component ", CStr(i + 1), CStr(.refresh_components.refresh_components.Count))
                        End If
                    End If
                Next
                .refresh_components.show_udcs = True
                .refresh_components.show_sdcs = False
            End With

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("chighsdc", "checkedchanged", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            REM save the medata file

            ao_object.enable_screen_updating()
            ao_object.update_refresh_status_bar("Complete", 0, 0)
            Me.Cursor = Cursors.Default
        End Try

    End Sub
    Private Sub clear_all()



    End Sub
    Private Sub highlight_all()
        Dim bk_colour As Integer = 0
        If loading = True Then
            Exit Sub
        End If
        Me.Cursor = Cursors.WaitCursor
        ao_object.disable_screen_updating()
        highlight_selected = True
        With bc_am_load_objects.obc_current_document
            For i = 0 To .refresh_components.refresh_components.Count - 1
                If (InStr(.refresh_components.refresh_components(i).locator, "_udc") > 0 And .refresh_components.refresh_components(i).author_Id = bc_cs_central_settings.logged_on_user_id) Or .refresh_components.refresh_components(i).mode = 8 Or .refresh_components.refresh_components(i).mode = 9 Then
                    bk_colour = ao_object.get_background_color_for_locator(.refresh_components.refresh_components(i).locator)
                    If bk_colour <> udc_colour_index And .refresh_components.refresh_components(i).original_markup_colour_index = 0 Then
                        .refresh_components.refresh_components(i).original_markup_colour_index = bk_colour
                        ao_object.set_background_colour_for_locator(.refresh_components.refresh_components(i).locator, udc_colour_index, .refresh_components.refresh_components(i).mode)
                        ao_object.update_refresh_status_bar("highlight component ", CStr(i + 1), CStr(.refresh_components.refresh_components.Count))
                    End If
                End If
            Next
            .refresh_components.show_udcs = False
            For i = 0 To .refresh_components.refresh_components.Count - 1
                If (InStr(.refresh_components.refresh_components(i).locator, "_at") > 0 And .refresh_components.refresh_components(i).mode <> 8 And .refresh_components.refresh_components(i).mode <> 9) Or (InStr(.refresh_components.refresh_components(i).locator, "_udc") > 0 And .refresh_components.refresh_components(i).author_id <> bc_cs_central_settings.logged_on_user_id) Then
                    bk_colour = ao_object.get_background_color_for_locator(.refresh_components.refresh_components(i).locator)
                    If bk_colour <> sdc_colour_index And .refresh_components.refresh_components(i).original_markup_colour_index = 0 Then
                        .refresh_components.refresh_components(i).original_markup_colour_index = bk_colour
                        ao_object.set_background_colour_for_locator(.refresh_components.refresh_components(i).locator, sdc_colour_index, .refresh_components.refresh_components(i).mode)
                        ao_object.update_refresh_status_bar("highlight component ", CStr(i + 1), CStr(.refresh_components.refresh_components.Count))
                    End If
                End If
            Next
            .refresh_components.show_sdcs = True
            ao_object.enable_screen_updating()
            ao_object.update_refresh_status_bar("Complete", 0, 0)
            Me.Cursor = Cursors.Default
        End With
    End Sub

    Private Sub RadioButton4_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Redit.CheckedChanged
        REM k
        If loading = True Then

            Exit Sub
        End If
        If Redit.Checked = True Then
            highlight_edit()
            save()
        End If
    End Sub

    Private Sub Rall_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Rall.CheckedChanged

        If loading = True Then

            Exit Sub
        End If
        If Rall.Checked = True Then
            highlight_all()
            save()
        End If
    End Sub

    Private Sub ListView1_ControlAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.ControlEventArgs) Handles ListView1.ControlAdded

    End Sub

    Private Sub ListView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListView1.SelectedIndexChanged

    End Sub


    Private Sub ListBox1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListBox1.SelectedIndexChanged
        If Me.ListBox1.Visible = False Then
            Exit Sub
        End If
        If Me.ListBox1.SelectedIndex = -1 And bc_am_load_objects.obc_current_document.refresh_components.refresh_components(index).parameters.component_template_parameters(index2).datatype <> 8 Then
            REM index 
            Me.btnopen.Enabled = False
            Me.Button1.Enabled = False
            Exit Sub
        End If
        setValue()
    End Sub

    'Private Sub ListBox1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListBox1.TextChanged
    '    'If Me.ListBox1.Visible = False Then
    '    '    Exit Sub
    '    'End If
    '    'If Me.ListBox1.SelectedIndex = -1 And bc_am_load_objects.obc_current_document.refresh_components.refresh_components(index).parameters.component_template_parameters(index2).datatype <> 8 Then
    '    '    REM index 
    '    '    Me.btnopen.Enabled = False
    '    '    Me.Button1.Enabled = False
    '    '    Exit Sub
    '    'End If
    '    'setValue()
    'End Sub

    Private Sub ListBox1_TextUpdate(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListBox1.TextUpdate
        If Me.ListBox1.Visible = False Then
            Exit Sub
        End If
        If bc_am_load_objects.obc_current_document.refresh_components.refresh_components(index).parameters.component_template_parameters(index2).datatype <> 8 Then
            Me.btnopen.Enabled = False
            Me.Button1.Enabled = False
        Else
            setValue()
        End If
    End Sub

    Private Sub trtf_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub
End Class
