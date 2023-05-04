<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bc_am_user_defined_components
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_user_defined_components))
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.Psave = New System.Windows.Forms.Panel
        Me.Rspublic = New System.Windows.Forms.RadioButton
        Me.rsprivate = New System.Windows.Forms.RadioButton
        Me.bsave = New System.Windows.Forms.Button
        Me.Button4 = New System.Windows.Forms.Button
        Me.breload = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.PictureBox2 = New System.Windows.Forms.PictureBox
        Me.Pselitem = New System.Windows.Forms.Panel
        Me.Cclasssel = New System.Windows.Forms.ComboBox
        Me.Centitysel = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.rpublic = New System.Windows.Forms.RadioButton
        Me.rprivate = New System.Windows.Forms.RadioButton
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.TabPage2 = New System.Windows.Forms.TabPage
        Me.Lcomps = New System.Windows.Forms.ListView
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.Type = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader4 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader5 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader6 = New System.Windows.Forms.ColumnHeader
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.rsystem = New System.Windows.Forms.RadioButton
        Me.Ruser = New System.Windows.Forms.RadioButton
        Me.Rall = New System.Windows.Forms.RadioButton
        Me.pfilter = New System.Windows.Forms.Panel
        Me.budcall = New System.Windows.Forms.RadioButton
        Me.Dto = New System.Windows.Forms.DateTimePicker
        Me.Dfrom = New System.Windows.Forms.DateTimePicker
        Me.Centity = New System.Windows.Forms.ComboBox
        Me.Cpubtype = New System.Windows.Forms.ComboBox
        Me.Cauthor = New System.Windows.Forms.ComboBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.Cwrap = New System.Windows.Forms.CheckBox
        Me.Ltitle = New System.Windows.Forms.Label
        Me.Ttitle = New System.Windows.Forms.TextBox
        Me.tpreview = New System.Windows.Forms.RichTextBox
        Me.Panel2.SuspendLayout()
        Me.Psave.SuspendLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Pselitem.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.pfilter.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel2
        '
        Me.Panel2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel2.BackColor = System.Drawing.Color.White
        Me.Panel2.Controls.Add(Me.Psave)
        Me.Panel2.Controls.Add(Me.bsave)
        Me.Panel2.Controls.Add(Me.Button4)
        Me.Panel2.Controls.Add(Me.breload)
        Me.Panel2.Controls.Add(Me.Label2)
        Me.Panel2.Controls.Add(Me.PictureBox2)
        Me.Panel2.Controls.Add(Me.Pselitem)
        Me.Panel2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Panel2.Location = New System.Drawing.Point(-24, 401)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(675, 69)
        Me.Panel2.TabIndex = 29
        '
        'Psave
        '
        Me.Psave.Controls.Add(Me.Rspublic)
        Me.Psave.Controls.Add(Me.rsprivate)
        Me.Psave.Location = New System.Drawing.Point(260, 47)
        Me.Psave.Name = "Psave"
        Me.Psave.Size = New System.Drawing.Size(132, 22)
        Me.Psave.TabIndex = 36
        Me.Psave.Visible = False
        '
        'Rspublic
        '
        Me.Rspublic.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Rspublic.AutoSize = True
        Me.Rspublic.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Rspublic.Location = New System.Drawing.Point(71, 3)
        Me.Rspublic.Name = "Rspublic"
        Me.Rspublic.Size = New System.Drawing.Size(58, 17)
        Me.Rspublic.TabIndex = 32
        Me.Rspublic.Text = "Public"
        Me.Rspublic.UseVisualStyleBackColor = True
        '
        'rsprivate
        '
        Me.rsprivate.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rsprivate.AutoSize = True
        Me.rsprivate.Checked = True
        Me.rsprivate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rsprivate.Location = New System.Drawing.Point(3, 3)
        Me.rsprivate.Name = "rsprivate"
        Me.rsprivate.Size = New System.Drawing.Size(65, 17)
        Me.rsprivate.TabIndex = 31
        Me.rsprivate.TabStop = True
        Me.rsprivate.Text = "Private"
        Me.rsprivate.UseVisualStyleBackColor = True
        '
        'bsave
        '
        Me.bsave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bsave.Enabled = False
        Me.bsave.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bsave.Location = New System.Drawing.Point(587, 43)
        Me.bsave.Name = "bsave"
        Me.bsave.Size = New System.Drawing.Size(80, 23)
        Me.bsave.TabIndex = 17
        Me.bsave.Text = "Save"
        Me.bsave.UseVisualStyleBackColor = True
        '
        'Button4
        '
        Me.Button4.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button4.Location = New System.Drawing.Point(500, 43)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(80, 23)
        Me.Button4.TabIndex = 21
        Me.Button4.Text = "Cancel"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'breload
        '
        Me.breload.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.breload.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.breload.Location = New System.Drawing.Point(410, 43)
        Me.breload.Name = "breload"
        Me.breload.Size = New System.Drawing.Size(80, 23)
        Me.breload.TabIndex = 32
        Me.breload.Text = "Reload"
        Me.breload.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(28, 52)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(216, 14)
        Me.Label2.TabIndex = 29
        Me.Label2.Text = "© Copyright Blue Curve Ltd."
        '
        'PictureBox2
        '
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(30, 13)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(184, 36)
        Me.PictureBox2.TabIndex = 27
        Me.PictureBox2.TabStop = False
        '
        'Pselitem
        '
        Me.Pselitem.Controls.Add(Me.Cclasssel)
        Me.Pselitem.Controls.Add(Me.Centitysel)
        Me.Pselitem.Controls.Add(Me.Label1)
        Me.Pselitem.Enabled = False
        Me.Pselitem.Location = New System.Drawing.Point(224, 0)
        Me.Pselitem.Name = "Pselitem"
        Me.Pselitem.Size = New System.Drawing.Size(439, 44)
        Me.Pselitem.TabIndex = 35
        '
        'Cclasssel
        '
        Me.Cclasssel.FormattingEnabled = True
        Me.Cclasssel.Location = New System.Drawing.Point(155, 13)
        Me.Cclasssel.Name = "Cclasssel"
        Me.Cclasssel.Size = New System.Drawing.Size(122, 21)
        Me.Cclasssel.TabIndex = 34
        '
        'Centitysel
        '
        Me.Centitysel.FormattingEnabled = True
        Me.Centitysel.Location = New System.Drawing.Point(283, 13)
        Me.Centitysel.Name = "Centitysel"
        Me.Centitysel.Size = New System.Drawing.Size(156, 21)
        Me.Centitysel.TabIndex = 33
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(3, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(146, 13)
        Me.Label1.TabIndex = 35
        Me.Label1.Text = "Select item to supply data for "
        '
        'rpublic
        '
        Me.rpublic.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rpublic.AutoSize = True
        Me.rpublic.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rpublic.Location = New System.Drawing.Point(568, 49)
        Me.rpublic.Name = "rpublic"
        Me.rpublic.Size = New System.Drawing.Size(58, 17)
        Me.rpublic.TabIndex = 31
        Me.rpublic.Text = "Public"
        Me.rpublic.UseVisualStyleBackColor = True
        '
        'rprivate
        '
        Me.rprivate.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rprivate.AutoSize = True
        Me.rprivate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rprivate.Location = New System.Drawing.Point(497, 49)
        Me.rprivate.Name = "rprivate"
        Me.rprivate.Size = New System.Drawing.Size(65, 17)
        Me.rprivate.TabIndex = 30
        Me.rprivate.Text = "Private"
        Me.rprivate.UseVisualStyleBackColor = True
        '
        'TabControl1
        '
        Me.TabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabControl1.Location = New System.Drawing.Point(6, 2)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(637, 393)
        Me.TabControl1.TabIndex = 30
        '
        'TabPage2
        '
        Me.TabPage2.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.TabPage2.Controls.Add(Me.Lcomps)
        Me.TabPage2.Controls.Add(Me.Panel1)
        Me.TabPage2.Controls.Add(Me.pfilter)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(629, 367)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Component List"
        '
        'Lcomps
        '
        Me.Lcomps.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Lcomps.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader2, Me.Type, Me.ColumnHeader1, Me.ColumnHeader3, Me.ColumnHeader4, Me.ColumnHeader5, Me.ColumnHeader6})
        Me.Lcomps.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Lcomps.FullRowSelect = True
        Me.Lcomps.HideSelection = False
        Me.Lcomps.Location = New System.Drawing.Point(0, 0)
        Me.Lcomps.MultiSelect = False
        Me.Lcomps.Name = "Lcomps"
        Me.Lcomps.Size = New System.Drawing.Size(626, 368)
        Me.Lcomps.TabIndex = 0
        Me.Lcomps.UseCompatibleStateImageBehavior = False
        Me.Lcomps.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Title"
        Me.ColumnHeader2.Width = 437
        '
        'Type
        '
        Me.Type.Text = "Type"
        Me.Type.Width = 134
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Update Date"
        Me.ColumnHeader1.Width = 103
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Author"
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Publiction"
        Me.ColumnHeader4.Width = 84
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "Classification"
        Me.ColumnHeader5.Width = 105
        '
        'ColumnHeader6
        '
        Me.ColumnHeader6.Width = 0
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.rsystem)
        Me.Panel1.Controls.Add(Me.Ruser)
        Me.Panel1.Controls.Add(Me.Rall)
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(321, 32)
        Me.Panel1.TabIndex = 25
        Me.Panel1.Visible = False
        '
        'rsystem
        '
        Me.rsystem.AutoSize = True
        Me.rsystem.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rsystem.Location = New System.Drawing.Point(169, 9)
        Me.rsystem.Name = "rsystem"
        Me.rsystem.Size = New System.Drawing.Size(116, 17)
        Me.rsystem.TabIndex = 2
        Me.rsystem.Text = "System Defined"
        Me.rsystem.UseVisualStyleBackColor = True
        '
        'Ruser
        '
        Me.Ruser.AutoSize = True
        Me.Ruser.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Ruser.Location = New System.Drawing.Point(64, 9)
        Me.Ruser.Name = "Ruser"
        Me.Ruser.Size = New System.Drawing.Size(99, 17)
        Me.Ruser.TabIndex = 1
        Me.Ruser.Text = "User Defined"
        Me.Ruser.UseVisualStyleBackColor = True
        '
        'Rall
        '
        Me.Rall.AutoSize = True
        Me.Rall.Checked = True
        Me.Rall.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Rall.Location = New System.Drawing.Point(9, 8)
        Me.Rall.Name = "Rall"
        Me.Rall.Size = New System.Drawing.Size(39, 17)
        Me.Rall.TabIndex = 0
        Me.Rall.TabStop = True
        Me.Rall.Text = "All"
        Me.Rall.UseVisualStyleBackColor = True
        '
        'pfilter
        '
        Me.pfilter.Controls.Add(Me.budcall)
        Me.pfilter.Controls.Add(Me.Dto)
        Me.pfilter.Controls.Add(Me.Dfrom)
        Me.pfilter.Controls.Add(Me.Centity)
        Me.pfilter.Controls.Add(Me.rpublic)
        Me.pfilter.Controls.Add(Me.Cpubtype)
        Me.pfilter.Controls.Add(Me.rprivate)
        Me.pfilter.Controls.Add(Me.Cauthor)
        Me.pfilter.Controls.Add(Me.Label8)
        Me.pfilter.Controls.Add(Me.Label7)
        Me.pfilter.Controls.Add(Me.Label5)
        Me.pfilter.Controls.Add(Me.Label4)
        Me.pfilter.Controls.Add(Me.Label3)
        Me.pfilter.Location = New System.Drawing.Point(0, 31)
        Me.pfilter.Name = "pfilter"
        Me.pfilter.Size = New System.Drawing.Size(626, 75)
        Me.pfilter.TabIndex = 26
        Me.pfilter.Visible = False
        '
        'budcall
        '
        Me.budcall.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.budcall.AutoSize = True
        Me.budcall.Checked = True
        Me.budcall.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.budcall.Location = New System.Drawing.Point(437, 48)
        Me.budcall.Name = "budcall"
        Me.budcall.Size = New System.Drawing.Size(36, 19)
        Me.budcall.TabIndex = 35
        Me.budcall.TabStop = True
        Me.budcall.Text = "All"
        Me.budcall.UseCompatibleTextRendering = True
        Me.budcall.UseVisualStyleBackColor = False
        '
        'Dto
        '
        Me.Dto.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Dto.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.Dto.Location = New System.Drawing.Point(512, 19)
        Me.Dto.Name = "Dto"
        Me.Dto.Size = New System.Drawing.Size(111, 21)
        Me.Dto.TabIndex = 26
        '
        'Dfrom
        '
        Me.Dfrom.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Dfrom.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.Dfrom.Location = New System.Drawing.Point(387, 18)
        Me.Dfrom.Name = "Dfrom"
        Me.Dfrom.Size = New System.Drawing.Size(111, 21)
        Me.Dfrom.TabIndex = 25
        '
        'Centity
        '
        Me.Centity.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Centity.FormattingEnabled = True
        Me.Centity.Location = New System.Drawing.Point(266, 18)
        Me.Centity.Name = "Centity"
        Me.Centity.Size = New System.Drawing.Size(107, 21)
        Me.Centity.TabIndex = 28
        '
        'Cpubtype
        '
        Me.Cpubtype.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Cpubtype.FormattingEnabled = True
        Me.Cpubtype.Location = New System.Drawing.Point(137, 18)
        Me.Cpubtype.Name = "Cpubtype"
        Me.Cpubtype.Size = New System.Drawing.Size(111, 21)
        Me.Cpubtype.TabIndex = 27
        '
        'Cauthor
        '
        Me.Cauthor.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Cauthor.FormattingEnabled = True
        Me.Cauthor.Location = New System.Drawing.Point(6, 19)
        Me.Cauthor.Name = "Cauthor"
        Me.Cauthor.Size = New System.Drawing.Size(107, 21)
        Me.Cauthor.TabIndex = 29
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(510, 3)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(52, 13)
        Me.Label8.TabIndex = 34
        Me.Label8.Text = "Date To"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(384, 2)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(67, 13)
        Me.Label7.TabIndex = 33
        Me.Label7.Text = "Date From"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(6, 3)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(45, 13)
        Me.Label5.TabIndex = 32
        Me.Label5.Text = "Author"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(263, 2)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(82, 13)
        Me.Label4.TabIndex = 31
        Me.Label4.Text = "Classification"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(134, 2)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(72, 13)
        Me.Label3.TabIndex = 30
        Me.Label3.Text = "Publication "
        '
        'TabPage1
        '
        Me.TabPage1.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.TabPage1.Controls.Add(Me.Cwrap)
        Me.TabPage1.Controls.Add(Me.Ltitle)
        Me.TabPage1.Controls.Add(Me.Ttitle)
        Me.TabPage1.Controls.Add(Me.tpreview)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(629, 367)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Preview"
        '
        'Cwrap
        '
        Me.Cwrap.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Cwrap.AutoSize = True
        Me.Cwrap.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Cwrap.Location = New System.Drawing.Point(567, 345)
        Me.Cwrap.Name = "Cwrap"
        Me.Cwrap.Size = New System.Drawing.Size(56, 17)
        Me.Cwrap.TabIndex = 7
        Me.Cwrap.Text = "Wrap"
        Me.Cwrap.UseVisualStyleBackColor = True
        '
        'Ltitle
        '
        Me.Ltitle.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Ltitle.AutoSize = True
        Me.Ltitle.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Ltitle.Location = New System.Drawing.Point(6, 346)
        Me.Ltitle.Name = "Ltitle"
        Me.Ltitle.Size = New System.Drawing.Size(121, 13)
        Me.Ltitle.TabIndex = 6
        Me.Ltitle.Text = "Component Name"
        '
        'Ttitle
        '
        Me.Ttitle.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Ttitle.Location = New System.Drawing.Point(133, 340)
        Me.Ttitle.MaxLength = 250
        Me.Ttitle.Name = "Ttitle"
        Me.Ttitle.Size = New System.Drawing.Size(387, 21)
        Me.Ttitle.TabIndex = 5
        '
        'tpreview
        '
        Me.tpreview.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tpreview.BackColor = System.Drawing.Color.White
        Me.tpreview.Location = New System.Drawing.Point(7, 5)
        Me.tpreview.Name = "tpreview"
        Me.tpreview.ReadOnly = True
        Me.tpreview.Size = New System.Drawing.Size(616, 327)
        Me.tpreview.TabIndex = 4
        Me.tpreview.Text = ""
        Me.tpreview.WordWrap = False
        '
        'bc_am_user_defined_components
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(650, 471)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.Panel2)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.MaximizeBox = False
        Me.Name = "bc_am_user_defined_components"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Preview User Defined Component - Blue Curve"
        Me.Panel2.ResumeLayout(False)
        Me.Psave.ResumeLayout(False)
        Me.Psave.PerformLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Pselitem.ResumeLayout(False)
        Me.Pselitem.PerformLayout()
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage2.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.pfilter.ResumeLayout(False)
        Me.pfilter.PerformLayout()
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents rpublic As System.Windows.Forms.RadioButton
    Friend WithEvents rprivate As System.Windows.Forms.RadioButton
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents bsave As System.Windows.Forms.Button
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents breload As System.Windows.Forms.Button
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents Cwrap As System.Windows.Forms.CheckBox
    Friend WithEvents Ltitle As System.Windows.Forms.Label
    Friend WithEvents Ttitle As System.Windows.Forms.TextBox
    Friend WithEvents tpreview As System.Windows.Forms.RichTextBox
    Friend WithEvents Lcomps As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents rsystem As System.Windows.Forms.RadioButton
    Friend WithEvents Ruser As System.Windows.Forms.RadioButton
    Friend WithEvents Rall As System.Windows.Forms.RadioButton
    Friend WithEvents pfilter As System.Windows.Forms.Panel
    Friend WithEvents budcall As System.Windows.Forms.RadioButton
    Friend WithEvents Dto As System.Windows.Forms.DateTimePicker
    Friend WithEvents Dfrom As System.Windows.Forms.DateTimePicker
    Friend WithEvents Centity As System.Windows.Forms.ComboBox
    Friend WithEvents Cpubtype As System.Windows.Forms.ComboBox
    Friend WithEvents Cauthor As System.Windows.Forms.ComboBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Type As System.Windows.Forms.ColumnHeader
    Friend WithEvents Pselitem As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Cclasssel As System.Windows.Forms.ComboBox
    Friend WithEvents Centitysel As System.Windows.Forms.ComboBox
    Friend WithEvents Psave As System.Windows.Forms.Panel
    Friend WithEvents Rspublic As System.Windows.Forms.RadioButton
    Friend WithEvents rsprivate As System.Windows.Forms.RadioButton
End Class
