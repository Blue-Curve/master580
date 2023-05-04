<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bc_am_adv_composite_build
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_adv_composite_build))
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.uxDocBuildList = New System.Windows.Forms.GroupBox
        Me.Lvwcomps = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader10 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader11 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader12 = New System.Windows.Forms.ColumnHeader
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.uxSearchResults = New System.Windows.Forms.GroupBox
        Me.lvwres = New System.Windows.Forms.ListView
        Me.ColumnHeader4 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader5 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader8 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader9 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader7 = New System.Windows.Forms.ColumnHeader
        Me.uxSearch = New System.Windows.Forms.GroupBox
        Me.Ttitle = New System.Windows.Forms.TextBox
        Me.dfrom = New System.Windows.Forms.DateTimePicker
        Me.Dto = New System.Windows.Forms.DateTimePicker
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Cpt = New System.Windows.Forms.ComboBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.Cst = New System.Windows.Forms.ComboBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.Button1 = New System.Windows.Forms.Button
        Me.Bdn = New System.Windows.Forms.Button
        Me.Bup = New System.Windows.Forms.Button
        Me.brm = New System.Windows.Forms.Button
        Me.badd = New System.Windows.Forms.Button
        Me.blogo = New System.Windows.Forms.PictureBox
        Me.PictureBox2 = New System.Windows.Forms.PictureBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.bleftgrey = New System.Windows.Forms.PictureBox
        Me.bokgrey = New System.Windows.Forms.PictureBox
        Me.bok = New System.Windows.Forms.PictureBox
        Me.Btnback = New System.Windows.Forms.PictureBox
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.uxDocBuildList.SuspendLayout()
        Me.uxSearchResults.SuspendLayout()
        Me.uxSearch.SuspendLayout()
        CType(Me.blogo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bleftgrey, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bokgrey, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bok, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Btnback, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(0, -1)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(404, 64)
        Me.PictureBox1.TabIndex = 79
        Me.PictureBox1.TabStop = False
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel1.Controls.Add(Me.uxDocBuildList)
        Me.Panel1.Controls.Add(Me.uxSearchResults)
        Me.Panel1.Controls.Add(Me.uxSearch)
        Me.Panel1.Controls.Add(Me.Bdn)
        Me.Panel1.Controls.Add(Me.Bup)
        Me.Panel1.Controls.Add(Me.brm)
        Me.Panel1.Controls.Add(Me.badd)
        Me.Panel1.Location = New System.Drawing.Point(0, 63)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(867, 537)
        Me.Panel1.TabIndex = 80
        '
        'uxDocBuildList
        '
        Me.uxDocBuildList.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxDocBuildList.Controls.Add(Me.Lvwcomps)
        Me.uxDocBuildList.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxDocBuildList.Location = New System.Drawing.Point(462, 6)
        Me.uxDocBuildList.Name = "uxDocBuildList"
        Me.uxDocBuildList.Size = New System.Drawing.Size(398, 524)
        Me.uxDocBuildList.TabIndex = 10
        Me.uxDocBuildList.TabStop = False
        Me.uxDocBuildList.Text = "Document Build List"
        '
        'Lvwcomps
        '
        Me.Lvwcomps.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Lvwcomps.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader3, Me.ColumnHeader10, Me.ColumnHeader11, Me.ColumnHeader12})
        Me.Lvwcomps.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Lvwcomps.FullRowSelect = True
        Me.Lvwcomps.HideSelection = False
        Me.Lvwcomps.Location = New System.Drawing.Point(6, 20)
        Me.Lvwcomps.MultiSelect = False
        Me.Lvwcomps.Name = "Lvwcomps"
        Me.Lvwcomps.ShowItemToolTips = True
        Me.Lvwcomps.Size = New System.Drawing.Size(388, 498)
        Me.Lvwcomps.SmallImageList = Me.ImageList1
        Me.Lvwcomps.TabIndex = 2
        Me.Lvwcomps.UseCompatibleStateImageBehavior = False
        Me.Lvwcomps.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Title"
        Me.ColumnHeader1.Width = 254
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Date"
        Me.ColumnHeader3.Width = 70
        '
        'ColumnHeader10
        '
        Me.ColumnHeader10.Text = "Pub Type"
        '
        'ColumnHeader11
        '
        Me.ColumnHeader11.Text = "Stage"
        '
        'ColumnHeader12
        '
        Me.ColumnHeader12.Text = "Checked out User"
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "")
        Me.ImageList1.Images.SetKeyName(1, "word.bmp")
        '
        'uxSearchResults
        '
        Me.uxSearchResults.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.uxSearchResults.Controls.Add(Me.lvwres)
        Me.uxSearchResults.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxSearchResults.Location = New System.Drawing.Point(3, 160)
        Me.uxSearchResults.Name = "uxSearchResults"
        Me.uxSearchResults.Size = New System.Drawing.Size(418, 370)
        Me.uxSearchResults.TabIndex = 9
        Me.uxSearchResults.TabStop = False
        Me.uxSearchResults.Text = "Search Results"
        '
        'lvwres
        '
        Me.lvwres.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvwres.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader4, Me.ColumnHeader5, Me.ColumnHeader2, Me.ColumnHeader8, Me.ColumnHeader9, Me.ColumnHeader7})
        Me.lvwres.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwres.FullRowSelect = True
        Me.lvwres.HideSelection = False
        Me.lvwres.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lvwres.Location = New System.Drawing.Point(6, 21)
        Me.lvwres.MultiSelect = False
        Me.lvwres.Name = "lvwres"
        Me.lvwres.ShowItemToolTips = True
        Me.lvwres.Size = New System.Drawing.Size(406, 343)
        Me.lvwres.SmallImageList = Me.ImageList1
        Me.lvwres.TabIndex = 2
        Me.lvwres.UseCompatibleStateImageBehavior = False
        Me.lvwres.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Title"
        Me.ColumnHeader4.Width = 245
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "Date"
        Me.ColumnHeader5.Width = 81
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Pub Type"
        '
        'ColumnHeader8
        '
        Me.ColumnHeader8.Text = "Stage"
        '
        'ColumnHeader9
        '
        Me.ColumnHeader9.Text = "Checked Out User"
        '
        'ColumnHeader7
        '
        Me.ColumnHeader7.Text = "Id"
        Me.ColumnHeader7.Width = 0
        '
        'uxSearch
        '
        Me.uxSearch.Controls.Add(Me.Ttitle)
        Me.uxSearch.Controls.Add(Me.dfrom)
        Me.uxSearch.Controls.Add(Me.Dto)
        Me.uxSearch.Controls.Add(Me.Label9)
        Me.uxSearch.Controls.Add(Me.Label8)
        Me.uxSearch.Controls.Add(Me.Label7)
        Me.uxSearch.Controls.Add(Me.Cpt)
        Me.uxSearch.Controls.Add(Me.Label6)
        Me.uxSearch.Controls.Add(Me.Cst)
        Me.uxSearch.Controls.Add(Me.Label5)
        Me.uxSearch.Controls.Add(Me.Button1)
        Me.uxSearch.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxSearch.Location = New System.Drawing.Point(3, 6)
        Me.uxSearch.Name = "uxSearch"
        Me.uxSearch.Size = New System.Drawing.Size(418, 148)
        Me.uxSearch.TabIndex = 8
        Me.uxSearch.TabStop = False
        Me.uxSearch.Text = "Search"
        '
        'Ttitle
        '
        Me.Ttitle.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Ttitle.Location = New System.Drawing.Point(15, 112)
        Me.Ttitle.Name = "Ttitle"
        Me.Ttitle.Size = New System.Drawing.Size(250, 21)
        Me.Ttitle.TabIndex = 18
        '
        'dfrom
        '
        Me.dfrom.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dfrom.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dfrom.Location = New System.Drawing.Point(309, 31)
        Me.dfrom.Name = "dfrom"
        Me.dfrom.Size = New System.Drawing.Size(94, 21)
        Me.dfrom.TabIndex = 17
        '
        'Dto
        '
        Me.Dto.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Dto.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.Dto.Location = New System.Drawing.Point(309, 60)
        Me.Dto.Name = "Dto"
        Me.Dto.Size = New System.Drawing.Size(94, 21)
        Me.Dto.TabIndex = 16
        '
        'Label9
        '
        Me.Label9.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(271, 60)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(44, 26)
        Me.Label9.TabIndex = 25
        Me.Label9.Text = "To"
        '
        'Label8
        '
        Me.Label8.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(271, 31)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(44, 26)
        Me.Label8.TabIndex = 24
        Me.Label8.Text = "From"
        '
        'Label7
        '
        Me.Label7.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(13, 91)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(208, 26)
        Me.Label7.TabIndex = 23
        Me.Label7.Text = "Search String in Title"
        '
        'Cpt
        '
        Me.Cpt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Cpt.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Cpt.FormattingEnabled = True
        Me.Cpt.Location = New System.Drawing.Point(85, 28)
        Me.Cpt.Name = "Cpt"
        Me.Cpt.Size = New System.Drawing.Size(180, 21)
        Me.Cpt.TabIndex = 14
        '
        'Label6
        '
        Me.Label6.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(13, 31)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(75, 26)
        Me.Label6.TabIndex = 22
        Me.Label6.Text = "Publication"
        '
        'Cst
        '
        Me.Cst.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Cst.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Cst.FormattingEnabled = True
        Me.Cst.Location = New System.Drawing.Point(85, 60)
        Me.Cst.Name = "Cst"
        Me.Cst.Size = New System.Drawing.Size(180, 21)
        Me.Cst.TabIndex = 15
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(12, 60)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(55, 26)
        Me.Label5.TabIndex = 21
        Me.Label5.Text = "Stage"
        '
        'Button1
        '
        Me.Button1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.Location = New System.Drawing.Point(328, 108)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 25)
        Me.Button1.TabIndex = 19
        Me.Button1.Text = "Search"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Bdn
        '
        Me.Bdn.Enabled = False
        Me.Bdn.Image = CType(resources.GetObject("Bdn.Image"), System.Drawing.Image)
        Me.Bdn.Location = New System.Drawing.Point(428, 285)
        Me.Bdn.Name = "Bdn"
        Me.Bdn.Size = New System.Drawing.Size(28, 24)
        Me.Bdn.TabIndex = 5
        Me.Bdn.UseVisualStyleBackColor = True
        '
        'Bup
        '
        Me.Bup.Enabled = False
        Me.Bup.Image = CType(resources.GetObject("Bup.Image"), System.Drawing.Image)
        Me.Bup.Location = New System.Drawing.Point(428, 244)
        Me.Bup.Name = "Bup"
        Me.Bup.Size = New System.Drawing.Size(28, 24)
        Me.Bup.TabIndex = 4
        Me.Bup.UseVisualStyleBackColor = True
        '
        'brm
        '
        Me.brm.Enabled = False
        Me.brm.Image = CType(resources.GetObject("brm.Image"), System.Drawing.Image)
        Me.brm.Location = New System.Drawing.Point(428, 329)
        Me.brm.Name = "brm"
        Me.brm.Size = New System.Drawing.Size(28, 24)
        Me.brm.TabIndex = 3
        Me.brm.UseVisualStyleBackColor = True
        '
        'badd
        '
        Me.badd.Enabled = False
        Me.badd.Image = CType(resources.GetObject("badd.Image"), System.Drawing.Image)
        Me.badd.Location = New System.Drawing.Point(428, 371)
        Me.badd.Name = "badd"
        Me.badd.Size = New System.Drawing.Size(28, 24)
        Me.badd.TabIndex = 2
        Me.badd.UseVisualStyleBackColor = True
        '
        'blogo
        '
        Me.blogo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.blogo.Image = CType(resources.GetObject("blogo.Image"), System.Drawing.Image)
        Me.blogo.Location = New System.Drawing.Point(788, -3)
        Me.blogo.Name = "blogo"
        Me.blogo.Size = New System.Drawing.Size(79, 66)
        Me.blogo.TabIndex = 81
        Me.blogo.TabStop = False
        '
        'PictureBox2
        '
        Me.PictureBox2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(5, 611)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(215, 48)
        Me.PictureBox2.TabIndex = 82
        Me.PictureBox2.TabStop = False
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(4, 641)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(289, 21)
        Me.Label3.TabIndex = 83
        Me.Label3.Text = "© Blue Curve Limited. All rights reserved."
        '
        'bleftgrey
        '
        Me.bleftgrey.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bleftgrey.Image = CType(resources.GetObject("bleftgrey.Image"), System.Drawing.Image)
        Me.bleftgrey.Location = New System.Drawing.Point(749, 603)
        Me.bleftgrey.Name = "bleftgrey"
        Me.bleftgrey.Size = New System.Drawing.Size(56, 56)
        Me.bleftgrey.TabIndex = 84
        Me.bleftgrey.TabStop = False
        '
        'bokgrey
        '
        Me.bokgrey.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bokgrey.BackColor = System.Drawing.Color.Transparent
        Me.bokgrey.Image = CType(resources.GetObject("bokgrey.Image"), System.Drawing.Image)
        Me.bokgrey.Location = New System.Drawing.Point(811, 603)
        Me.bokgrey.Name = "bokgrey"
        Me.bokgrey.Size = New System.Drawing.Size(56, 56)
        Me.bokgrey.TabIndex = 85
        Me.bokgrey.TabStop = False
        Me.bokgrey.Visible = False
        '
        'bok
        '
        Me.bok.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bok.BackColor = System.Drawing.Color.Transparent
        Me.bok.Image = CType(resources.GetObject("bok.Image"), System.Drawing.Image)
        Me.bok.Location = New System.Drawing.Point(811, 603)
        Me.bok.Name = "bok"
        Me.bok.Size = New System.Drawing.Size(56, 56)
        Me.bok.TabIndex = 86
        Me.bok.TabStop = False
        '
        'Btnback
        '
        Me.Btnback.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Btnback.Image = CType(resources.GetObject("Btnback.Image"), System.Drawing.Image)
        Me.Btnback.Location = New System.Drawing.Point(749, 603)
        Me.Btnback.Name = "Btnback"
        Me.Btnback.Size = New System.Drawing.Size(56, 56)
        Me.Btnback.TabIndex = 87
        Me.Btnback.TabStop = False
        Me.Btnback.Visible = False
        '
        'ToolTip1
        '
        Me.ToolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info
        Me.ToolTip1.ToolTipTitle = "Document Summary"
        '
        'bc_am_adv_composite_build
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(869, 662)
        Me.Controls.Add(Me.bleftgrey)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.PictureBox2)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.blogo)
        Me.Controls.Add(Me.bok)
        Me.Controls.Add(Me.Btnback)
        Me.Controls.Add(Me.bokgrey)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimizeBox = False
        Me.Name = "bc_am_adv_composite_build"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Blue Curve - Advanced Composite Document Build"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.uxDocBuildList.ResumeLayout(False)
        Me.uxSearchResults.ResumeLayout(False)
        Me.uxSearch.ResumeLayout(False)
        Me.uxSearch.PerformLayout()
        CType(Me.blogo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bleftgrey, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bokgrey, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bok, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Btnback, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents blogo As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Bdn As System.Windows.Forms.Button
    Friend WithEvents Bup As System.Windows.Forms.Button
    Friend WithEvents brm As System.Windows.Forms.Button
    Friend WithEvents badd As System.Windows.Forms.Button
    Friend WithEvents bleftgrey As System.Windows.Forms.PictureBox
    Friend WithEvents bokgrey As System.Windows.Forms.PictureBox
    Friend WithEvents bok As System.Windows.Forms.PictureBox
    Friend WithEvents Btnback As System.Windows.Forms.PictureBox
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents uxSearch As System.Windows.Forms.GroupBox
    Friend WithEvents uxSearchResults As System.Windows.Forms.GroupBox
    Friend WithEvents Ttitle As System.Windows.Forms.TextBox
    Friend WithEvents dfrom As System.Windows.Forms.DateTimePicker
    Friend WithEvents Dto As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Cpt As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Cst As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents lvwres As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader8 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader9 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader7 As System.Windows.Forms.ColumnHeader
    Friend WithEvents uxDocBuildList As System.Windows.Forms.GroupBox
    Friend WithEvents Lvwcomps As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader10 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader11 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader12 As System.Windows.Forms.ColumnHeader
End Class
