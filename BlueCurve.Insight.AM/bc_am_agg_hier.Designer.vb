<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bc_am_agg_hier
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_agg_hier))
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.badd = New System.Windows.Forms.Button()
        Me.lduals = New System.Windows.Forms.ListBox()
        Me.ccdual = New System.Windows.Forms.ComboBox()
        Me.ctarget = New System.Windows.Forms.ComboBox()
        Me.cdual = New System.Windows.Forms.Label()
        Me.lt = New System.Windows.Forms.Label()
        Me.hh = New System.Windows.Forms.Label()
        Me.lta = New System.Windows.Forms.CheckedListBox()
        Me.bsave = New System.Windows.Forms.Button()
        Me.bcancel = New System.Windows.Forms.Button()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.at = New System.Windows.Forms.Label()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.chkall = New System.Windows.Forms.CheckBox()
        Me.Centity = New System.Windows.Forms.ComboBox()
        Me.cclass = New System.Windows.Forms.ComboBox()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.SuspendLayout()
        '
        'PictureBox2
        '
        Me.PictureBox2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(14, 286)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(215, 36)
        Me.PictureBox2.TabIndex = 64
        Me.PictureBox2.TabStop = False
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel1.Controls.Add(Me.badd)
        Me.Panel1.Controls.Add(Me.lduals)
        Me.Panel1.Controls.Add(Me.ccdual)
        Me.Panel1.Controls.Add(Me.ctarget)
        Me.Panel1.Controls.Add(Me.cdual)
        Me.Panel1.Controls.Add(Me.lt)
        Me.Panel1.Controls.Add(Me.hh)
        Me.Panel1.Location = New System.Drawing.Point(14, 12)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(467, 219)
        Me.Panel1.TabIndex = 66
        '
        'badd
        '
        Me.badd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.badd.Enabled = False
        Me.badd.Location = New System.Drawing.Point(382, 43)
        Me.badd.Margin = New System.Windows.Forms.Padding(5, 3, 5, 3)
        Me.badd.Name = "badd"
        Me.badd.Size = New System.Drawing.Size(76, 30)
        Me.badd.TabIndex = 71
        Me.badd.Text = "Add"
        Me.badd.UseVisualStyleBackColor = True
        '
        'lduals
        '
        Me.lduals.FormattingEnabled = True
        Me.lduals.Location = New System.Drawing.Point(16, 91)
        Me.lduals.Name = "lduals"
        Me.lduals.Size = New System.Drawing.Size(347, 108)
        Me.lduals.TabIndex = 13
        '
        'ccdual
        '
        Me.ccdual.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ccdual.FormattingEnabled = True
        Me.ccdual.Location = New System.Drawing.Point(209, 49)
        Me.ccdual.Name = "ccdual"
        Me.ccdual.Size = New System.Drawing.Size(154, 21)
        Me.ccdual.TabIndex = 12
        '
        'ctarget
        '
        Me.ctarget.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ctarget.FormattingEnabled = True
        Me.ctarget.Location = New System.Drawing.Point(16, 49)
        Me.ctarget.Name = "ctarget"
        Me.ctarget.Size = New System.Drawing.Size(154, 21)
        Me.ctarget.TabIndex = 11
        '
        'cdual
        '
        Me.cdual.AutoSize = True
        Me.cdual.Location = New System.Drawing.Point(206, 33)
        Me.cdual.Name = "cdual"
        Me.cdual.Size = New System.Drawing.Size(41, 13)
        Me.cdual.TabIndex = 8
        Me.cdual.Text = " Dual "
        '
        'lt
        '
        Me.lt.AutoSize = True
        Me.lt.Location = New System.Drawing.Point(13, 33)
        Me.lt.Name = "lt"
        Me.lt.Size = New System.Drawing.Size(48, 13)
        Me.lt.TabIndex = 7
        Me.lt.Text = "Target "
        '
        'hh
        '
        Me.hh.AutoSize = True
        Me.hh.Location = New System.Drawing.Point(148, 10)
        Me.hh.Name = "hh"
        Me.hh.Size = New System.Drawing.Size(165, 13)
        Me.hh.TabIndex = 5
        Me.hh.Text = "Target  - Dual Intersections"
        '
        'lta
        '
        Me.lta.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lta.CheckOnClick = True
        Me.lta.FormattingEnabled = True
        Me.lta.Location = New System.Drawing.Point(16, 49)
        Me.lta.Name = "lta"
        Me.lta.Size = New System.Drawing.Size(216, 148)
        Me.lta.TabIndex = 0
        '
        'bsave
        '
        Me.bsave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bsave.Enabled = False
        Me.bsave.Location = New System.Drawing.Point(680, 292)
        Me.bsave.Margin = New System.Windows.Forms.Padding(5, 3, 5, 3)
        Me.bsave.Name = "bsave"
        Me.bsave.Size = New System.Drawing.Size(76, 30)
        Me.bsave.TabIndex = 68
        Me.bsave.Text = " Submit"
        Me.bsave.UseVisualStyleBackColor = True
        '
        'bcancel
        '
        Me.bcancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bcancel.Location = New System.Drawing.Point(581, 292)
        Me.bcancel.Margin = New System.Windows.Forms.Padding(5, 3, 5, 3)
        Me.bcancel.Name = "bcancel"
        Me.bcancel.Size = New System.Drawing.Size(76, 30)
        Me.bcancel.TabIndex = 69
        Me.bcancel.Text = "Cancel"
        Me.bcancel.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel2.Controls.Add(Me.at)
        Me.Panel2.Controls.Add(Me.lta)
        Me.Panel2.Location = New System.Drawing.Point(498, 12)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(258, 219)
        Me.Panel2.TabIndex = 67
        '
        'at
        '
        Me.at.AutoSize = True
        Me.at.Location = New System.Drawing.Point(13, 10)
        Me.at.Name = "at"
        Me.at.Size = New System.Drawing.Size(21, 13)
        Me.at.TabIndex = 5
        Me.at.Text = "All"
        '
        'Panel3
        '
        Me.Panel3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel3.Controls.Add(Me.Label2)
        Me.Panel3.Controls.Add(Me.Label1)
        Me.Panel3.Controls.Add(Me.chkall)
        Me.Panel3.Controls.Add(Me.Centity)
        Me.Panel3.Controls.Add(Me.cclass)
        Me.Panel3.Location = New System.Drawing.Point(14, 237)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(740, 37)
        Me.Panel3.TabIndex = 68
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(492, 10)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(39, 13)
        Me.Label2.TabIndex = 10
        Me.Label2.Text = "Entity"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(66, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(210, 13)
        Me.Label1.TabIndex = 9
        Me.Label1.Text = "Propogate (and store in) from class"
        '
        'chkall
        '
        Me.chkall.AutoSize = True
        Me.chkall.Location = New System.Drawing.Point(16, 11)
        Me.chkall.Name = "chkall"
        Me.chkall.Size = New System.Drawing.Size(40, 17)
        Me.chkall.TabIndex = 8
        Me.chkall.Text = "All"
        Me.chkall.UseVisualStyleBackColor = True
        '
        'Centity
        '
        Me.Centity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Centity.Enabled = False
        Me.Centity.FormattingEnabled = True
        Me.Centity.Location = New System.Drawing.Point(538, 7)
        Me.Centity.Name = "Centity"
        Me.Centity.Size = New System.Drawing.Size(194, 21)
        Me.Centity.TabIndex = 7
        '
        'cclass
        '
        Me.cclass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cclass.Enabled = False
        Me.cclass.FormattingEnabled = True
        Me.cclass.Location = New System.Drawing.Point(290, 7)
        Me.cclass.Name = "cclass"
        Me.cclass.Size = New System.Drawing.Size(194, 21)
        Me.cclass.TabIndex = 6
        '
        'bc_am_agg_hier
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(771, 334)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.bcancel)
        Me.Controls.Add(Me.bsave)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.PictureBox2)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "bc_am_agg_hier"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Hierarchies"
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents hh As System.Windows.Forms.Label
    Friend WithEvents lta As System.Windows.Forms.CheckedListBox
    Friend WithEvents bsave As System.Windows.Forms.Button
    Friend WithEvents bcancel As System.Windows.Forms.Button
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents at As System.Windows.Forms.Label
    Friend WithEvents lt As System.Windows.Forms.Label
    Friend WithEvents cdual As System.Windows.Forms.Label
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents chkall As System.Windows.Forms.CheckBox
    Friend WithEvents Centity As System.Windows.Forms.ComboBox
    Friend WithEvents cclass As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ccdual As System.Windows.Forms.ComboBox
    Friend WithEvents ctarget As System.Windows.Forms.ComboBox
    Friend WithEvents lduals As System.Windows.Forms.ListBox
    Friend WithEvents badd As System.Windows.Forms.Button
End Class
