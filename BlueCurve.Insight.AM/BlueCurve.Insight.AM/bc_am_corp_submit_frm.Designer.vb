<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bc_am_corp_submit_frm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_corp_submit_frm))
        Me.PictureBox4 = New System.Windows.Forms.PictureBox
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.uxFactorBox = New System.Windows.Forms.Label
        Me.uxAdjust = New System.Windows.Forms.Label
        Me.uxCheckAdjust = New System.Windows.Forms.CheckBox
        Me.uxActionBox = New System.Windows.Forms.Label
        Me.uxStockBox = New System.Windows.Forms.Label
        Me.uxAction = New System.Windows.Forms.Label
        Me.uxStock = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.PictureBox2 = New System.Windows.Forms.PictureBox
        Me.bcancel = New System.Windows.Forms.PictureBox
        Me.bok = New System.Windows.Forms.PictureBox
        Me.bokdisabled = New System.Windows.Forms.PictureBox
        Me.bcanceldisabled = New System.Windows.Forms.PictureBox
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bcancel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bok, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bokdisabled, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bcanceldisabled, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PictureBox4
        '
        Me.PictureBox4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PictureBox4.Image = CType(resources.GetObject("PictureBox4.Image"), System.Drawing.Image)
        Me.PictureBox4.Location = New System.Drawing.Point(442, 0)
        Me.PictureBox4.Name = "PictureBox4"
        Me.PictureBox4.Size = New System.Drawing.Size(64, 64)
        Me.PictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox4.TabIndex = 51
        Me.PictureBox4.TabStop = False
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(0, 0)
        Me.PictureBox1.Margin = New System.Windows.Forms.Padding(0)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(440, 64)
        Me.PictureBox1.TabIndex = 52
        Me.PictureBox1.TabStop = False
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.uxFactorBox)
        Me.Panel1.Controls.Add(Me.uxAdjust)
        Me.Panel1.Controls.Add(Me.uxCheckAdjust)
        Me.Panel1.Controls.Add(Me.uxActionBox)
        Me.Panel1.Controls.Add(Me.uxStockBox)
        Me.Panel1.Controls.Add(Me.uxAction)
        Me.Panel1.Controls.Add(Me.uxStock)
        Me.Panel1.Location = New System.Drawing.Point(0, 62)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(506, 272)
        Me.Panel1.TabIndex = 53
        '
        'uxFactorBox
        '
        Me.uxFactorBox.AutoSize = True
        Me.uxFactorBox.Location = New System.Drawing.Point(167, 84)
        Me.uxFactorBox.Name = "uxFactorBox"
        Me.uxFactorBox.Size = New System.Drawing.Size(44, 13)
        Me.uxFactorBox.TabIndex = 9
        Me.uxFactorBox.Text = "Label5"
        '
        'uxAdjust
        '
        Me.uxAdjust.AutoSize = True
        Me.uxAdjust.Location = New System.Drawing.Point(45, 84)
        Me.uxAdjust.Name = "uxAdjust"
        Me.uxAdjust.Size = New System.Drawing.Size(116, 13)
        Me.uxAdjust.TabIndex = 8
        Me.uxAdjust.Text = "Adjustment Factor:"
        '
        'uxCheckAdjust
        '
        Me.uxCheckAdjust.AutoSize = True
        Me.uxCheckAdjust.Location = New System.Drawing.Point(48, 202)
        Me.uxCheckAdjust.Name = "uxCheckAdjust"
        Me.uxCheckAdjust.Size = New System.Drawing.Size(307, 17)
        Me.uxCheckAdjust.TabIndex = 7
        Me.uxCheckAdjust.TabStop = False
        Me.uxCheckAdjust.Text = "Submit the adjustments for this corporate action."
        Me.uxCheckAdjust.UseVisualStyleBackColor = True
        '
        'uxActionBox
        '
        Me.uxActionBox.AutoSize = True
        Me.uxActionBox.Location = New System.Drawing.Point(166, 56)
        Me.uxActionBox.Name = "uxActionBox"
        Me.uxActionBox.Size = New System.Drawing.Size(44, 13)
        Me.uxActionBox.TabIndex = 6
        Me.uxActionBox.Text = "Label6"
        '
        'uxStockBox
        '
        Me.uxStockBox.AutoSize = True
        Me.uxStockBox.Location = New System.Drawing.Point(166, 31)
        Me.uxStockBox.Name = "uxStockBox"
        Me.uxStockBox.Size = New System.Drawing.Size(44, 13)
        Me.uxStockBox.TabIndex = 5
        Me.uxStockBox.Text = "Label5"
        '
        'uxAction
        '
        Me.uxAction.AutoSize = True
        Me.uxAction.Location = New System.Drawing.Point(45, 56)
        Me.uxAction.Name = "uxAction"
        Me.uxAction.Size = New System.Drawing.Size(109, 13)
        Me.uxAction.TabIndex = 4
        Me.uxAction.Text = "Corporate Action:"
        '
        'uxStock
        '
        Me.uxStock.AutoSize = True
        Me.uxStock.Location = New System.Drawing.Point(45, 31)
        Me.uxStock.Name = "uxStock"
        Me.uxStock.Size = New System.Drawing.Size(44, 13)
        Me.uxStock.TabIndex = 3
        Me.uxStock.Text = "Stock:"
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label3.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(4, 386)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(240, 21)
        Me.Label3.TabIndex = 55
        Me.Label3.Text = "© Blue Curve Limited. All rights reserved."
        '
        'PictureBox2
        '
        Me.PictureBox2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(5, 335)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(193, 48)
        Me.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox2.TabIndex = 54
        Me.PictureBox2.TabStop = False
        '
        'bcancel
        '
        Me.bcancel.Image = CType(resources.GetObject("bcancel.Image"), System.Drawing.Image)
        Me.bcancel.Location = New System.Drawing.Point(378, 343)
        Me.bcancel.Name = "bcancel"
        Me.bcancel.Size = New System.Drawing.Size(56, 56)
        Me.bcancel.TabIndex = 75
        Me.bcancel.TabStop = False
        '
        'bok
        '
        Me.bok.BackColor = System.Drawing.Color.Transparent
        Me.bok.Image = CType(resources.GetObject("bok.Image"), System.Drawing.Image)
        Me.bok.Location = New System.Drawing.Point(440, 343)
        Me.bok.Name = "bok"
        Me.bok.Size = New System.Drawing.Size(56, 56)
        Me.bok.TabIndex = 74
        Me.bok.TabStop = False
        '
        'bokdisabled
        '
        Me.bokdisabled.BackColor = System.Drawing.Color.Transparent
        Me.bokdisabled.Image = CType(resources.GetObject("bokdisabled.Image"), System.Drawing.Image)
        Me.bokdisabled.Location = New System.Drawing.Point(440, 343)
        Me.bokdisabled.Name = "bokdisabled"
        Me.bokdisabled.Size = New System.Drawing.Size(56, 56)
        Me.bokdisabled.TabIndex = 78
        Me.bokdisabled.TabStop = False
        '
        'bcanceldisabled
        '
        Me.bcanceldisabled.Image = CType(resources.GetObject("bcanceldisabled.Image"), System.Drawing.Image)
        Me.bcanceldisabled.Location = New System.Drawing.Point(378, 343)
        Me.bcanceldisabled.Name = "bcanceldisabled"
        Me.bcanceldisabled.Size = New System.Drawing.Size(56, 56)
        Me.bcanceldisabled.TabIndex = 79
        Me.bcanceldisabled.TabStop = False
        Me.bcanceldisabled.Visible = False
        '
        'bc_am_corp_submit_frm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.ClientSize = New System.Drawing.Size(508, 408)
        Me.Controls.Add(Me.bcancel)
        Me.Controls.Add(Me.bok)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.PictureBox2)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.PictureBox4)
        Me.Controls.Add(Me.bokdisabled)
        Me.Controls.Add(Me.bcanceldisabled)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "bc_am_corp_submit_frm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Corporate Action Submission"
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bcancel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bok, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bokdisabled, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bcanceldisabled, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents PictureBox4 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents uxStock As System.Windows.Forms.Label
    Friend WithEvents uxAction As System.Windows.Forms.Label
    Friend WithEvents uxActionBox As System.Windows.Forms.Label
    Friend WithEvents uxStockBox As System.Windows.Forms.Label
    Friend WithEvents uxCheckAdjust As System.Windows.Forms.CheckBox
    Friend WithEvents bcancel As System.Windows.Forms.PictureBox
    Friend WithEvents bok As System.Windows.Forms.PictureBox
    Friend WithEvents bokdisabled As System.Windows.Forms.PictureBox
    Friend WithEvents bcanceldisabled As System.Windows.Forms.PictureBox
    Friend WithEvents uxAdjust As System.Windows.Forms.Label
    Friend WithEvents uxFactorBox As System.Windows.Forms.Label

End Class
