<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bc_am_cp_edit
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_cp_edit))
        Me.ltitle = New System.Windows.Forms.TextBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Bok = New System.Windows.Forms.Button()
        Me.uxDetails = New System.Windows.Forms.GroupBox()
        Me.defsql = New System.Windows.Forms.TextBox()
        Me.chkdef = New System.Windows.Forms.CheckBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Tlength = New System.Windows.Forms.TextBox()
        Me.cpopup = New System.Windows.Forms.CheckBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Tsql = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.clookup = New System.Windows.Forms.CheckBox()
        Me.Caudit = New System.Windows.Forms.CheckBox()
        Me.cmandatory = New System.Windows.Forms.CheckBox()
        Me.Cworkflow = New System.Windows.Forms.CheckBox()
        Me.Cdt = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Tattname = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Tentry = New System.Windows.Forms.TextBox()
        Me.centry = New System.Windows.Forms.ComboBox()
        Me.userpic = New System.Windows.Forms.PictureBox()
        Me.defaultpic = New System.Windows.Forms.PictureBox()
        Me.uxImageList = New System.Windows.Forms.ImageList(Me.components)
        Me.uxRemovePicture = New System.Windows.Forms.Button()
        Me.uxAssignPicture = New System.Windows.Forms.Button()
        Me.uxDetails.SuspendLayout()
        CType(Me.userpic, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.defaultpic, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ltitle
        '
        Me.ltitle.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ltitle.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ltitle.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.ltitle.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ltitle.Location = New System.Drawing.Point(11, 5)
        Me.ltitle.Multiline = True
        Me.ltitle.Name = "ltitle"
        Me.ltitle.ReadOnly = True
        Me.ltitle.Size = New System.Drawing.Size(274, 68)
        Me.ltitle.TabIndex = 0
        Me.ltitle.TabStop = False
        Me.ltitle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Button1
        '
        Me.Button1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button1.Location = New System.Drawing.Point(227, 110)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(66, 21)
        Me.Button1.TabIndex = 3
        Me.Button1.Text = "Cancel"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Bok
        '
        Me.Bok.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Bok.Enabled = False
        Me.Bok.Location = New System.Drawing.Point(156, 110)
        Me.Bok.Name = "Bok"
        Me.Bok.Size = New System.Drawing.Size(66, 21)
        Me.Bok.TabIndex = 2
        Me.Bok.Text = "Ok"
        Me.Bok.UseVisualStyleBackColor = True
        '
        'uxDetails
        '
        Me.uxDetails.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.uxDetails.Controls.Add(Me.defsql)
        Me.uxDetails.Controls.Add(Me.chkdef)
        Me.uxDetails.Controls.Add(Me.Label3)
        Me.uxDetails.Controls.Add(Me.Tlength)
        Me.uxDetails.Controls.Add(Me.cpopup)
        Me.uxDetails.Controls.Add(Me.Label1)
        Me.uxDetails.Controls.Add(Me.Tsql)
        Me.uxDetails.Controls.Add(Me.Label7)
        Me.uxDetails.Controls.Add(Me.clookup)
        Me.uxDetails.Controls.Add(Me.Caudit)
        Me.uxDetails.Controls.Add(Me.cmandatory)
        Me.uxDetails.Controls.Add(Me.Cworkflow)
        Me.uxDetails.Controls.Add(Me.Cdt)
        Me.uxDetails.Controls.Add(Me.Label2)
        Me.uxDetails.Controls.Add(Me.Tattname)
        Me.uxDetails.Controls.Add(Me.Label4)
        Me.uxDetails.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxDetails.Location = New System.Drawing.Point(5, 45)
        Me.uxDetails.Name = "uxDetails"
        Me.uxDetails.Size = New System.Drawing.Size(288, 60)
        Me.uxDetails.TabIndex = 1
        Me.uxDetails.TabStop = False
        Me.uxDetails.Visible = False
        '
        'defsql
        '
        Me.defsql.Enabled = False
        Me.defsql.Location = New System.Drawing.Point(7, 332)
        Me.defsql.MaxLength = 250
        Me.defsql.Multiline = True
        Me.defsql.Name = "defsql"
        Me.defsql.Size = New System.Drawing.Size(274, 65)
        Me.defsql.TabIndex = 15
        '
        'chkdef
        '
        Me.chkdef.AutoSize = True
        Me.chkdef.Location = New System.Drawing.Point(10, 284)
        Me.chkdef.Name = "chkdef"
        Me.chkdef.Size = New System.Drawing.Size(103, 17)
        Me.chkdef.TabIndex = 14
        Me.chkdef.Text = "Default Value"
        Me.chkdef.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(129, 109)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(104, 13)
        Me.Label3.TabIndex = 13
        Me.Label3.Text = "Maximum Length"
        '
        'Tlength
        '
        Me.Tlength.Enabled = False
        Me.Tlength.Location = New System.Drawing.Point(131, 127)
        Me.Tlength.MaxLength = 6
        Me.Tlength.Name = "Tlength"
        Me.Tlength.Size = New System.Drawing.Size(96, 21)
        Me.Tlength.TabIndex = 12
        '
        'cpopup
        '
        Me.cpopup.AutoSize = True
        Me.cpopup.Enabled = False
        Me.cpopup.Location = New System.Drawing.Point(132, 154)
        Me.cpopup.Name = "cpopup"
        Me.cpopup.Size = New System.Drawing.Size(150, 17)
        Me.cpopup.TabIndex = 11
        Me.cpopup.Text = "Pop up text entry box"
        Me.cpopup.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cpopup.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(5, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(93, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Attribute Name"
        '
        'Tsql
        '
        Me.Tsql.Enabled = False
        Me.Tsql.Location = New System.Drawing.Point(8, 213)
        Me.Tsql.MaxLength = 250
        Me.Tsql.Multiline = True
        Me.Tsql.Name = "Tsql"
        Me.Tsql.Size = New System.Drawing.Size(274, 65)
        Me.Tsql.TabIndex = 10
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(5, 197)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(75, 13)
        Me.Label7.TabIndex = 9
        Me.Label7.Text = "Lookup SQL"
        '
        'clookup
        '
        Me.clookup.AutoSize = True
        Me.clookup.Location = New System.Drawing.Point(8, 170)
        Me.clookup.Name = "clookup"
        Me.clookup.Size = New System.Drawing.Size(67, 17)
        Me.clookup.TabIndex = 8
        Me.clookup.Text = "Lookup"
        Me.clookup.UseVisualStyleBackColor = True
        '
        'Caudit
        '
        Me.Caudit.AutoSize = True
        Me.Caudit.Location = New System.Drawing.Point(8, 149)
        Me.Caudit.Name = "Caudit"
        Me.Caudit.Size = New System.Drawing.Size(69, 17)
        Me.Caudit.TabIndex = 7
        Me.Caudit.Text = "Audited"
        Me.Caudit.UseVisualStyleBackColor = True
        '
        'cmandatory
        '
        Me.cmandatory.AutoSize = True
        Me.cmandatory.Location = New System.Drawing.Point(8, 108)
        Me.cmandatory.Name = "cmandatory"
        Me.cmandatory.Size = New System.Drawing.Size(86, 17)
        Me.cmandatory.TabIndex = 5
        Me.cmandatory.Text = "Mandatory"
        Me.cmandatory.UseVisualStyleBackColor = True
        '
        'Cworkflow
        '
        Me.Cworkflow.AutoSize = True
        Me.Cworkflow.Location = New System.Drawing.Point(8, 127)
        Me.Cworkflow.Name = "Cworkflow"
        Me.Cworkflow.Size = New System.Drawing.Size(79, 17)
        Me.Cworkflow.TabIndex = 6
        Me.Cworkflow.Text = "Workflow"
        Me.Cworkflow.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Cworkflow.UseVisualStyleBackColor = True
        '
        'Cdt
        '
        Me.Cdt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Cdt.FormattingEnabled = True
        Me.Cdt.Location = New System.Drawing.Point(8, 76)
        Me.Cdt.Name = "Cdt"
        Me.Cdt.Size = New System.Drawing.Size(269, 21)
        Me.Cdt.TabIndex = 4
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(5, 60)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(66, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Data Type"
        '
        'Tattname
        '
        Me.Tattname.Location = New System.Drawing.Point(8, 34)
        Me.Tattname.MaxLength = 250
        Me.Tattname.Name = "Tattname"
        Me.Tattname.Size = New System.Drawing.Size(269, 21)
        Me.Tattname.TabIndex = 1
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(7, 316)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(75, 13)
        Me.Label4.TabIndex = 16
        Me.Label4.Text = "Default SQL"
        '
        'Tentry
        '
        Me.Tentry.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Tentry.Location = New System.Drawing.Point(14, 74)
        Me.Tentry.MaxLength = 250
        Me.Tentry.Name = "Tentry"
        Me.Tentry.Size = New System.Drawing.Size(269, 21)
        Me.Tentry.TabIndex = 0
        '
        'centry
        '
        Me.centry.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.centry.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.centry.FormattingEnabled = True
        Me.centry.Location = New System.Drawing.Point(14, 47)
        Me.centry.Name = "centry"
        Me.centry.Size = New System.Drawing.Size(268, 21)
        Me.centry.Sorted = True
        Me.centry.TabIndex = 4
        Me.centry.Visible = False
        '
        'userpic
        '
        Me.userpic.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.userpic.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.userpic.Image = CType(resources.GetObject("userpic.Image"), System.Drawing.Image)
        Me.userpic.Location = New System.Drawing.Point(5, 8)
        Me.userpic.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.userpic.Name = "userpic"
        Me.userpic.Size = New System.Drawing.Size(117, 110)
        Me.userpic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.userpic.TabIndex = 62
        Me.userpic.TabStop = False
        Me.userpic.Visible = False
        '
        'defaultpic
        '
        Me.defaultpic.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.defaultpic.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.defaultpic.Image = CType(resources.GetObject("defaultpic.Image"), System.Drawing.Image)
        Me.defaultpic.Location = New System.Drawing.Point(239, 5)
        Me.defaultpic.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.defaultpic.Name = "defaultpic"
        Me.defaultpic.Size = New System.Drawing.Size(34, 50)
        Me.defaultpic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.defaultpic.TabIndex = 65
        Me.defaultpic.TabStop = False
        Me.defaultpic.Visible = False
        '
        'uxImageList
        '
        Me.uxImageList.ImageStream = CType(resources.GetObject("uxImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.uxImageList.TransparentColor = System.Drawing.Color.Transparent
        Me.uxImageList.Images.SetKeyName(0, "")
        Me.uxImageList.Images.SetKeyName(1, "")
        '
        'uxRemovePicture
        '
        Me.uxRemovePicture.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxRemovePicture.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxRemovePicture.ImageAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.uxRemovePicture.ImageIndex = 1
        Me.uxRemovePicture.ImageList = Me.uxImageList
        Me.uxRemovePicture.Location = New System.Drawing.Point(163, 8)
        Me.uxRemovePicture.Name = "uxRemovePicture"
        Me.uxRemovePicture.Size = New System.Drawing.Size(28, 24)
        Me.uxRemovePicture.TabIndex = 5
        Me.uxRemovePicture.Visible = False
        '
        'uxAssignPicture
        '
        Me.uxAssignPicture.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxAssignPicture.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxAssignPicture.ImageAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.uxAssignPicture.ImageIndex = 0
        Me.uxAssignPicture.ImageList = Me.uxImageList
        Me.uxAssignPicture.Location = New System.Drawing.Point(129, 8)
        Me.uxAssignPicture.Name = "uxAssignPicture"
        Me.uxAssignPicture.Size = New System.Drawing.Size(28, 24)
        Me.uxAssignPicture.TabIndex = 4
        Me.uxAssignPicture.Visible = False
        '
        'bc_am_cp_edit
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(297, 134)
        Me.ControlBox = False
        Me.Controls.Add(Me.uxDetails)
        Me.Controls.Add(Me.uxAssignPicture)
        Me.Controls.Add(Me.uxRemovePicture)
        Me.Controls.Add(Me.defaultpic)
        Me.Controls.Add(Me.userpic)
        Me.Controls.Add(Me.centry)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Bok)
        Me.Controls.Add(Me.Tentry)
        Me.Controls.Add(Me.ltitle)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "bc_am_cp_edit"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = " "
        Me.uxDetails.ResumeLayout(False)
        Me.uxDetails.PerformLayout()
        CType(Me.userpic, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.defaultpic, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ltitle As System.Windows.Forms.TextBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Bok As System.Windows.Forms.Button
    Friend WithEvents uxDetails As System.Windows.Forms.GroupBox
    Friend WithEvents Tsql As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents clookup As System.Windows.Forms.CheckBox
    Friend WithEvents Caudit As System.Windows.Forms.CheckBox
    Friend WithEvents cmandatory As System.Windows.Forms.CheckBox
    Friend WithEvents Cworkflow As System.Windows.Forms.CheckBox
    Friend WithEvents Cdt As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Tattname As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Tentry As System.Windows.Forms.TextBox
    Friend WithEvents centry As System.Windows.Forms.ComboBox
    Friend WithEvents userpic As System.Windows.Forms.PictureBox
    Friend WithEvents defaultpic As System.Windows.Forms.PictureBox
    Friend WithEvents uxImageList As System.Windows.Forms.ImageList
    Friend WithEvents uxRemovePicture As System.Windows.Forms.Button
    Friend WithEvents uxAssignPicture As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Tlength As System.Windows.Forms.TextBox
    Friend WithEvents cpopup As System.Windows.Forms.CheckBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents defsql As System.Windows.Forms.TextBox
    Friend WithEvents chkdef As System.Windows.Forms.CheckBox
End Class
