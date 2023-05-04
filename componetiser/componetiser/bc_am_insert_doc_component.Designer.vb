<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bc_am_insert_doc_component
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
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_insert_doc_component))
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.lstatus = New System.Windows.Forms.Label
        Me.Button1 = New System.Windows.Forms.Button
        Me.rtfpreview = New System.Windows.Forms.RichTextBox
        Me.binsert = New System.Windows.Forms.Button
        Me.Button4 = New System.Windows.Forms.Button
        Me.pviewtype = New System.Windows.Forms.Panel
        Me.Label10 = New System.Windows.Forms.Label
        Me.cwrap1 = New System.Windows.Forms.CheckBox
        Me.bshowplain = New System.Windows.Forms.RadioButton
        Me.bshowRtf = New System.Windows.Forms.RadioButton
        Me.rstyle = New System.Windows.Forms.RadioButton
        Me.rplain = New System.Windows.Forms.RadioButton
        Me.rrtf = New System.Windows.Forms.RadioButton
        Me.bcopyselection = New System.Windows.Forms.Button
        Me.Pdocs = New System.Windows.Forms.Panel
        Me.Label11 = New System.Windows.Forms.Label
        Me.ldocs = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader4 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader5 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader6 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader7 = New System.Windows.Forms.ColumnHeader
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Cstage = New System.Windows.Forms.ComboBox
        Me.Cauthor = New System.Windows.Forms.ComboBox
        Me.Centity = New System.Windows.Forms.ComboBox
        Me.Cpubtype = New System.Windows.Forms.ComboBox
        Me.Dto = New System.Windows.Forms.DateTimePicker
        Me.Dfrom = New System.Windows.Forms.DateTimePicker
        Me.Psearch = New System.Windows.Forms.Panel
        Me.bsearch = New System.Windows.Forms.Button
        Me.Label12 = New System.Windows.Forms.Label
        Me.Tsearch = New System.Windows.Forms.TextBox
        Me.ralldocs = New System.Windows.Forms.RadioButton
        Me.Rhighlight = New System.Windows.Forms.RadioButton
        Me.Bclear = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.Type = New System.Windows.Forms.Label
        Me.citemtype = New System.Windows.Forms.ComboBox
        Me.Cpage = New System.Windows.Forms.ComboBox
        Me.Cstyle = New System.Windows.Forms.ComboBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.tcomponents = New System.Windows.Forms.TreeView
        Me.Pfilter = New System.Windows.Forms.Panel
        Me.PictureBox2 = New System.Windows.Forms.PictureBox
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Label1 = New System.Windows.Forms.Label
        Me.papplytype = New System.Windows.Forms.Panel
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.pviewtype.SuspendLayout()
        Me.Pdocs.SuspendLayout()
        Me.Psearch.SuspendLayout()
        Me.Pfilter.SuspendLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.papplytype.SuspendLayout()
        Me.SuspendLayout()
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "entity.png")
        Me.ImageList1.Images.SetKeyName(1, "add.png")
        Me.ImageList1.Images.SetKeyName(2, "makeinactive.png")
        Me.ImageList1.Images.SetKeyName(3, "publications.bmp")
        Me.ImageList1.Images.SetKeyName(4, "change name.png")
        Me.ImageList1.Images.SetKeyName(5, "table.bmp")
        Me.ImageList1.Images.SetKeyName(6, "chart.bmp")
        '
        'lstatus
        '
        Me.lstatus.AutoSize = True
        Me.lstatus.Location = New System.Drawing.Point(422, 12)
        Me.lstatus.Name = "lstatus"
        Me.lstatus.Size = New System.Drawing.Size(0, 13)
        Me.lstatus.TabIndex = 1
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(287, 26)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(107, 26)
        Me.Button1.TabIndex = 2
        Me.Button1.Text = "Componetize"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'rtfpreview
        '
        Me.rtfpreview.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rtfpreview.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.rtfpreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.rtfpreview.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rtfpreview.Location = New System.Drawing.Point(378, 323)
        Me.rtfpreview.Name = "rtfpreview"
        Me.rtfpreview.ReadOnly = True
        Me.rtfpreview.Size = New System.Drawing.Size(328, 357)
        Me.rtfpreview.TabIndex = 16
        Me.rtfpreview.Text = ""
        '
        'binsert
        '
        Me.binsert.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.binsert.Enabled = False
        Me.binsert.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.binsert.Location = New System.Drawing.Point(629, 24)
        Me.binsert.Name = "binsert"
        Me.binsert.Size = New System.Drawing.Size(80, 26)
        Me.binsert.TabIndex = 17
        Me.binsert.Text = "Insert All"
        Me.binsert.UseVisualStyleBackColor = True
        '
        'Button4
        '
        Me.Button4.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button4.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button4.Location = New System.Drawing.Point(434, 23)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(81, 29)
        Me.Button4.TabIndex = 21
        Me.Button4.Text = "Close"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'pviewtype
        '
        Me.pviewtype.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pviewtype.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pviewtype.Controls.Add(Me.Label10)
        Me.pviewtype.Controls.Add(Me.cwrap1)
        Me.pviewtype.Controls.Add(Me.bshowplain)
        Me.pviewtype.Controls.Add(Me.bshowRtf)
        Me.pviewtype.Enabled = False
        Me.pviewtype.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pviewtype.Location = New System.Drawing.Point(378, 294)
        Me.pviewtype.Name = "pviewtype"
        Me.pviewtype.Size = New System.Drawing.Size(328, 37)
        Me.pviewtype.TabIndex = 22
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(1, 5)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(62, 14)
        Me.Label10.TabIndex = 28
        Me.Label10.Text = "Preview"
        '
        'cwrap1
        '
        Me.cwrap1.AutoSize = True
        Me.cwrap1.Checked = True
        Me.cwrap1.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cwrap1.Location = New System.Drawing.Point(270, 4)
        Me.cwrap1.Name = "cwrap1"
        Me.cwrap1.Size = New System.Drawing.Size(56, 17)
        Me.cwrap1.TabIndex = 2
        Me.cwrap1.Text = "Wrap"
        Me.cwrap1.UseVisualStyleBackColor = True
        '
        'bshowplain
        '
        Me.bshowplain.AutoSize = True
        Me.bshowplain.Location = New System.Drawing.Point(120, 4)
        Me.bshowplain.Name = "bshowplain"
        Me.bshowplain.Size = New System.Drawing.Size(116, 17)
        Me.bshowplain.TabIndex = 1
        Me.bshowplain.Text = "Show Plain Text"
        Me.bshowplain.UseVisualStyleBackColor = True
        Me.bshowplain.Visible = False
        '
        'bshowRtf
        '
        Me.bshowRtf.AutoSize = True
        Me.bshowRtf.Checked = True
        Me.bshowRtf.Location = New System.Drawing.Point(69, 3)
        Me.bshowRtf.Name = "bshowRtf"
        Me.bshowRtf.Size = New System.Drawing.Size(85, 17)
        Me.bshowRtf.TabIndex = 0
        Me.bshowRtf.TabStop = True
        Me.bshowRtf.Text = "Show  RTF"
        Me.bshowRtf.UseVisualStyleBackColor = True
        Me.bshowRtf.Visible = False
        '
        'rstyle
        '
        Me.rstyle.AutoSize = True
        Me.rstyle.Checked = True
        Me.rstyle.Location = New System.Drawing.Point(93, 3)
        Me.rstyle.Name = "rstyle"
        Me.rstyle.Size = New System.Drawing.Size(90, 17)
        Me.rstyle.TabIndex = 23
        Me.rstyle.TabStop = True
        Me.rstyle.Text = "Apply Style"
        Me.rstyle.UseVisualStyleBackColor = True
        '
        'rplain
        '
        Me.rplain.AutoSize = True
        Me.rplain.Location = New System.Drawing.Point(6, 3)
        Me.rplain.Name = "rplain"
        Me.rplain.Size = New System.Drawing.Size(81, 17)
        Me.rplain.TabIndex = 22
        Me.rplain.Text = "Plain Text"
        Me.rplain.UseVisualStyleBackColor = True
        '
        'rrtf
        '
        Me.rrtf.AutoSize = True
        Me.rrtf.Enabled = False
        Me.rrtf.Location = New System.Drawing.Point(195, 3)
        Me.rrtf.Name = "rrtf"
        Me.rrtf.Size = New System.Drawing.Size(78, 17)
        Me.rrtf.TabIndex = 21
        Me.rrtf.Text = "Rich Text"
        Me.rrtf.UseVisualStyleBackColor = True
        '
        'bcopyselection
        '
        Me.bcopyselection.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bcopyselection.Enabled = False
        Me.bcopyselection.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bcopyselection.Location = New System.Drawing.Point(529, 23)
        Me.bcopyselection.Name = "bcopyselection"
        Me.bcopyselection.Size = New System.Drawing.Size(81, 29)
        Me.bcopyselection.TabIndex = 24
        Me.bcopyselection.Text = "Insert Sel"
        Me.bcopyselection.UseVisualStyleBackColor = True
        '
        'Pdocs
        '
        Me.Pdocs.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Pdocs.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Pdocs.Controls.Add(Me.Label11)
        Me.Pdocs.Controls.Add(Me.ldocs)
        Me.Pdocs.Controls.Add(Me.Label8)
        Me.Pdocs.Controls.Add(Me.Label7)
        Me.Pdocs.Controls.Add(Me.Label6)
        Me.Pdocs.Controls.Add(Me.Label5)
        Me.Pdocs.Controls.Add(Me.Label4)
        Me.Pdocs.Controls.Add(Me.Label3)
        Me.Pdocs.Controls.Add(Me.Cstage)
        Me.Pdocs.Controls.Add(Me.Cauthor)
        Me.Pdocs.Controls.Add(Me.Centity)
        Me.Pdocs.Controls.Add(Me.Cpubtype)
        Me.Pdocs.Controls.Add(Me.Dto)
        Me.Pdocs.Controls.Add(Me.Dfrom)
        Me.Pdocs.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Pdocs.Location = New System.Drawing.Point(5, 2)
        Me.Pdocs.Name = "Pdocs"
        Me.Pdocs.Size = New System.Drawing.Size(702, 239)
        Me.Pdocs.TabIndex = 25
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(1, 5)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(138, 14)
        Me.Label11.TabIndex = 21
        Me.Label11.Text = "Document Selection"
        '
        'ldocs
        '
        Me.ldocs.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ldocs.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4, Me.ColumnHeader5, Me.ColumnHeader6, Me.ColumnHeader7})
        Me.ldocs.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ldocs.FullRowSelect = True
        Me.ldocs.HideSelection = False
        Me.ldocs.Location = New System.Drawing.Point(3, 66)
        Me.ldocs.MultiSelect = False
        Me.ldocs.Name = "ldocs"
        Me.ldocs.Size = New System.Drawing.Size(692, 166)
        Me.ldocs.TabIndex = 13
        Me.ldocs.UseCompatibleStateImageBehavior = False
        Me.ldocs.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Date"
        Me.ColumnHeader1.Width = 147
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Title"
        Me.ColumnHeader2.Width = 200
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Type"
        Me.ColumnHeader3.Width = 83
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Classification"
        Me.ColumnHeader4.Width = 94
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "Author"
        Me.ColumnHeader5.Width = 76
        '
        'ColumnHeader6
        '
        Me.ColumnHeader6.Text = "Stage"
        Me.ColumnHeader6.Width = 85
        '
        'ColumnHeader7
        '
        Me.ColumnHeader7.Text = ""
        Me.ColumnHeader7.Width = 0
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(580, 25)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(52, 13)
        Me.Label8.TabIndex = 12
        Me.Label8.Text = "Date To"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(463, 25)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(67, 13)
        Me.Label7.TabIndex = 11
        Me.Label7.Text = "Date From"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(342, 25)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(97, 13)
        Me.Label6.TabIndex = 10
        Me.Label6.Text = "Workflow Stage"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(232, 25)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(45, 13)
        Me.Label5.TabIndex = 9
        Me.Label5.Text = "Author"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(119, 25)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(82, 13)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "Classification"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(1, 25)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(100, 13)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "Publication Type"
        '
        'Cstage
        '
        Me.Cstage.FormattingEnabled = True
        Me.Cstage.Location = New System.Drawing.Point(348, 41)
        Me.Cstage.Name = "Cstage"
        Me.Cstage.Size = New System.Drawing.Size(107, 21)
        Me.Cstage.TabIndex = 6
        '
        'Cauthor
        '
        Me.Cauthor.FormattingEnabled = True
        Me.Cauthor.Location = New System.Drawing.Point(235, 41)
        Me.Cauthor.Name = "Cauthor"
        Me.Cauthor.Size = New System.Drawing.Size(107, 21)
        Me.Cauthor.TabIndex = 5
        '
        'Centity
        '
        Me.Centity.FormattingEnabled = True
        Me.Centity.Location = New System.Drawing.Point(122, 41)
        Me.Centity.Name = "Centity"
        Me.Centity.Size = New System.Drawing.Size(107, 21)
        Me.Centity.TabIndex = 4
        '
        'Cpubtype
        '
        Me.Cpubtype.FormattingEnabled = True
        Me.Cpubtype.Location = New System.Drawing.Point(3, 41)
        Me.Cpubtype.Name = "Cpubtype"
        Me.Cpubtype.Size = New System.Drawing.Size(111, 21)
        Me.Cpubtype.TabIndex = 3
        '
        'Dto
        '
        Me.Dto.Location = New System.Drawing.Point(583, 41)
        Me.Dto.Name = "Dto"
        Me.Dto.Size = New System.Drawing.Size(111, 21)
        Me.Dto.TabIndex = 2
        '
        'Dfrom
        '
        Me.Dfrom.Location = New System.Drawing.Point(466, 41)
        Me.Dfrom.Name = "Dfrom"
        Me.Dfrom.Size = New System.Drawing.Size(111, 21)
        Me.Dfrom.TabIndex = 1
        '
        'Psearch
        '
        Me.Psearch.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Psearch.Controls.Add(Me.bsearch)
        Me.Psearch.Controls.Add(Me.Label12)
        Me.Psearch.Controls.Add(Me.Tsearch)
        Me.Psearch.Controls.Add(Me.ralldocs)
        Me.Psearch.Controls.Add(Me.Rhighlight)
        Me.Psearch.Controls.Add(Me.Bclear)
        Me.Psearch.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Psearch.Location = New System.Drawing.Point(5, 247)
        Me.Psearch.Name = "Psearch"
        Me.Psearch.Size = New System.Drawing.Size(702, 41)
        Me.Psearch.TabIndex = 26
        '
        'bsearch
        '
        Me.bsearch.Enabled = False
        Me.bsearch.Location = New System.Drawing.Point(560, 6)
        Me.bsearch.Name = "bsearch"
        Me.bsearch.Size = New System.Drawing.Size(68, 26)
        Me.bsearch.TabIndex = 25
        Me.bsearch.Text = "Search"
        Me.bsearch.UseVisualStyleBackColor = True
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(1, 12)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(76, 14)
        Me.Label12.TabIndex = 30
        Me.Label12.Text = "Search for"
        '
        'Tsearch
        '
        Me.Tsearch.Location = New System.Drawing.Point(81, 9)
        Me.Tsearch.MaxLength = 250
        Me.Tsearch.Name = "Tsearch"
        Me.Tsearch.Size = New System.Drawing.Size(185, 21)
        Me.Tsearch.TabIndex = 26
        '
        'ralldocs
        '
        Me.ralldocs.AutoSize = True
        Me.ralldocs.Checked = True
        Me.ralldocs.Enabled = False
        Me.ralldocs.Location = New System.Drawing.Point(441, 10)
        Me.ralldocs.Name = "ralldocs"
        Me.ralldocs.Size = New System.Drawing.Size(121, 17)
        Me.ralldocs.TabIndex = 29
        Me.ralldocs.TabStop = True
        Me.ralldocs.Text = "in All Documents"
        Me.ralldocs.UseVisualStyleBackColor = True
        '
        'Rhighlight
        '
        Me.Rhighlight.AutoSize = True
        Me.Rhighlight.Enabled = False
        Me.Rhighlight.Location = New System.Drawing.Point(272, 9)
        Me.Rhighlight.Name = "Rhighlight"
        Me.Rhighlight.Size = New System.Drawing.Size(164, 17)
        Me.Rhighlight.TabIndex = 28
        Me.Rhighlight.Text = "in Highlighted Document"
        Me.Rhighlight.UseVisualStyleBackColor = True
        '
        'Bclear
        '
        Me.Bclear.Location = New System.Drawing.Point(634, 6)
        Me.Bclear.Name = "Bclear"
        Me.Bclear.Size = New System.Drawing.Size(60, 26)
        Me.Bclear.TabIndex = 27
        Me.Bclear.Text = "Clear"
        Me.Bclear.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(186, 363)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(36, 13)
        Me.Label2.TabIndex = 17
        Me.Label2.Text = "Style"
        '
        'Type
        '
        Me.Type.AutoSize = True
        Me.Type.Location = New System.Drawing.Point(1, 363)
        Me.Type.Name = "Type"
        Me.Type.Size = New System.Drawing.Size(35, 13)
        Me.Type.TabIndex = 18
        Me.Type.Text = "Type"
        '
        'citemtype
        '
        Me.citemtype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.citemtype.FormattingEnabled = True
        Me.citemtype.Location = New System.Drawing.Point(36, 355)
        Me.citemtype.Name = "citemtype"
        Me.citemtype.Size = New System.Drawing.Size(136, 21)
        Me.citemtype.TabIndex = 19
        '
        'Cpage
        '
        Me.Cpage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Cpage.FormattingEnabled = True
        Me.Cpage.Location = New System.Drawing.Point(151, 3)
        Me.Cpage.Name = "Cpage"
        Me.Cpage.Size = New System.Drawing.Size(209, 21)
        Me.Cpage.TabIndex = 14
        '
        'Cstyle
        '
        Me.Cstyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Cstyle.FormattingEnabled = True
        Me.Cstyle.Location = New System.Drawing.Point(228, 355)
        Me.Cstyle.Name = "Cstyle"
        Me.Cstyle.Size = New System.Drawing.Size(132, 21)
        Me.Cstyle.TabIndex = 16
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(0, 4)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(58, 14)
        Me.Label9.TabIndex = 20
        Me.Label9.Text = "Content"
        '
        'tcomponents
        '
        Me.tcomponents.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tcomponents.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tcomponents.HideSelection = False
        Me.tcomponents.ImageIndex = 0
        Me.tcomponents.ImageList = Me.ImageList1
        Me.tcomponents.Location = New System.Drawing.Point(3, 27)
        Me.tcomponents.Name = "tcomponents"
        Me.tcomponents.SelectedImageIndex = 1
        Me.tcomponents.Size = New System.Drawing.Size(357, 320)
        Me.tcomponents.TabIndex = 0
        '
        'Pfilter
        '
        Me.Pfilter.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Pfilter.Controls.Add(Me.Label9)
        Me.Pfilter.Controls.Add(Me.Cpage)
        Me.Pfilter.Controls.Add(Me.citemtype)
        Me.Pfilter.Controls.Add(Me.Type)
        Me.Pfilter.Controls.Add(Me.Label2)
        Me.Pfilter.Controls.Add(Me.Cstyle)
        Me.Pfilter.Controls.Add(Me.tcomponents)
        Me.Pfilter.Enabled = False
        Me.Pfilter.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Pfilter.Location = New System.Drawing.Point(5, 294)
        Me.Pfilter.Name = "Pfilter"
        Me.Pfilter.Size = New System.Drawing.Size(367, 383)
        Me.Pfilter.TabIndex = 14
        '
        'PictureBox2
        '
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(3, 3)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(184, 32)
        Me.PictureBox2.TabIndex = 27
        Me.PictureBox2.TabStop = False
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.White
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.Button1)
        Me.Panel1.Controls.Add(Me.binsert)
        Me.Panel1.Controls.Add(Me.bcopyselection)
        Me.Panel1.Controls.Add(Me.Button4)
        Me.Panel1.Controls.Add(Me.papplytype)
        Me.Panel1.Controls.Add(Me.PictureBox2)
        Me.Panel1.Location = New System.Drawing.Point(-8, 683)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(719, 58)
        Me.Panel1.TabIndex = 28
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(11, 34)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(216, 16)
        Me.Label1.TabIndex = 29
        Me.Label1.Text = "© Copyright Corero Systems Ltd."
        '
        'papplytype
        '
        Me.papplytype.Controls.Add(Me.rplain)
        Me.papplytype.Controls.Add(Me.rrtf)
        Me.papplytype.Controls.Add(Me.rstyle)
        Me.papplytype.Location = New System.Drawing.Point(427, 0)
        Me.papplytype.Name = "papplytype"
        Me.papplytype.Size = New System.Drawing.Size(289, 32)
        Me.papplytype.TabIndex = 28
        '
        'bc_am_insert_doc_component
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(709, 735)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.rtfpreview)
        Me.Controls.Add(Me.pviewtype)
        Me.Controls.Add(Me.Psearch)
        Me.Controls.Add(Me.Pdocs)
        Me.Controls.Add(Me.lstatus)
        Me.Controls.Add(Me.Pfilter)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.MaximizeBox = False
        Me.Name = "bc_am_insert_doc_component"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Text = "Insert Previous Document Content - Blue Curve"
        Me.pviewtype.ResumeLayout(False)
        Me.pviewtype.PerformLayout()
        Me.Pdocs.ResumeLayout(False)
        Me.Pdocs.PerformLayout()
        Me.Psearch.ResumeLayout(False)
        Me.Psearch.PerformLayout()
        Me.Pfilter.ResumeLayout(False)
        Me.Pfilter.PerformLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.papplytype.ResumeLayout(False)
        Me.papplytype.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lstatus As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents rtfpreview As System.Windows.Forms.RichTextBox
    Friend WithEvents binsert As System.Windows.Forms.Button
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents pviewtype As System.Windows.Forms.Panel
    Friend WithEvents bshowplain As System.Windows.Forms.RadioButton
    Friend WithEvents bshowRtf As System.Windows.Forms.RadioButton
    Friend WithEvents rstyle As System.Windows.Forms.RadioButton
    Friend WithEvents rplain As System.Windows.Forms.RadioButton
    Friend WithEvents rrtf As System.Windows.Forms.RadioButton
    Friend WithEvents cwrap1 As System.Windows.Forms.CheckBox
    Friend WithEvents bcopyselection As System.Windows.Forms.Button
    Friend WithEvents Pdocs As System.Windows.Forms.Panel
    Friend WithEvents Cauthor As System.Windows.Forms.ComboBox
    Friend WithEvents Centity As System.Windows.Forms.ComboBox
    Friend WithEvents Cpubtype As System.Windows.Forms.ComboBox
    Friend WithEvents Dto As System.Windows.Forms.DateTimePicker
    Friend WithEvents Dfrom As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Cstage As System.Windows.Forms.ComboBox
    Friend WithEvents ldocs As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Psearch As System.Windows.Forms.Panel
    Friend WithEvents Tsearch As System.Windows.Forms.TextBox
    Friend WithEvents Bclear As System.Windows.Forms.Button
    Friend WithEvents bsearch As System.Windows.Forms.Button
    Friend WithEvents ralldocs As System.Windows.Forms.RadioButton
    Friend WithEvents Rhighlight As System.Windows.Forms.RadioButton
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Type As System.Windows.Forms.Label
    Friend WithEvents citemtype As System.Windows.Forms.ComboBox
    Friend WithEvents Cpage As System.Windows.Forms.ComboBox
    Friend WithEvents Cstyle As System.Windows.Forms.ComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents tcomponents As System.Windows.Forms.TreeView
    Friend WithEvents Pfilter As System.Windows.Forms.Panel
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents papplytype As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ColumnHeader7 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip

End Class
