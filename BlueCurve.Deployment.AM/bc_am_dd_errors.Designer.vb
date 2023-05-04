<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bc_am_dd_errors
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_dd_errors))
        Me.tvErrors = New System.Windows.Forms.TreeView
        Me.uxHeaderPanel = New System.Windows.Forms.Panel
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.uxBanner = New System.Windows.Forms.PictureBox
        Me.bcancel = New System.Windows.Forms.PictureBox
        Me.bok = New System.Windows.Forms.PictureBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.PictureBox2 = New System.Windows.Forms.PictureBox
        Me.bokdisabled = New System.Windows.Forms.PictureBox
        Me.bcanceldisabled = New System.Windows.Forms.PictureBox
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.uxHeaderPanel.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxBanner, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bcancel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bok, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bokdisabled, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bcanceldisabled, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'tvErrors
        '
        Me.tvErrors.BackColor = System.Drawing.Color.White
        Me.tvErrors.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tvErrors.Location = New System.Drawing.Point(0, 64)
        Me.tvErrors.Name = "tvErrors"
        Me.tvErrors.Size = New System.Drawing.Size(694, 202)
        Me.tvErrors.TabIndex = 0
        '
        'uxHeaderPanel
        '
        Me.uxHeaderPanel.Controls.Add(Me.PictureBox1)
        Me.uxHeaderPanel.Controls.Add(Me.uxBanner)
        Me.uxHeaderPanel.Dock = System.Windows.Forms.DockStyle.Top
        Me.uxHeaderPanel.Location = New System.Drawing.Point(0, 0)
        Me.uxHeaderPanel.Name = "uxHeaderPanel"
        Me.uxHeaderPanel.Size = New System.Drawing.Size(694, 64)
        Me.uxHeaderPanel.TabIndex = 9
        '
        'PictureBox1
        '
        Me.PictureBox1.Dock = System.Windows.Forms.DockStyle.Right
        Me.PictureBox1.ErrorImage = Nothing
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.InitialImage = Nothing
        Me.PictureBox1.Location = New System.Drawing.Point(629, 0)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(65, 64)
        Me.PictureBox1.TabIndex = 11
        Me.PictureBox1.TabStop = False
        '
        'uxBanner
        '
        Me.uxBanner.Image = CType(resources.GetObject("uxBanner.Image"), System.Drawing.Image)
        Me.uxBanner.Location = New System.Drawing.Point(0, 0)
        Me.uxBanner.Name = "uxBanner"
        Me.uxBanner.Size = New System.Drawing.Size(432, 64)
        Me.uxBanner.TabIndex = 5
        Me.uxBanner.TabStop = False
        '
        'bcancel
        '
        Me.bcancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bcancel.Image = CType(resources.GetObject("bcancel.Image"), System.Drawing.Image)
        Me.bcancel.Location = New System.Drawing.Point(573, 3)
        Me.bcancel.Name = "bcancel"
        Me.bcancel.Size = New System.Drawing.Size(56, 56)
        Me.bcancel.TabIndex = 83
        Me.bcancel.TabStop = False
        '
        'bok
        '
        Me.bok.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bok.BackColor = System.Drawing.Color.Transparent
        Me.bok.Image = CType(resources.GetObject("bok.Image"), System.Drawing.Image)
        Me.bok.Location = New System.Drawing.Point(635, 3)
        Me.bok.Name = "bok"
        Me.bok.Size = New System.Drawing.Size(56, 56)
        Me.bok.TabIndex = 82
        Me.bok.TabStop = False
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label3.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(4, 57)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(240, 21)
        Me.Label3.TabIndex = 81
        Me.Label3.Text = "© Blue Curve Limited. All rights reserved."
        '
        'PictureBox2
        '
        Me.PictureBox2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(5, 3)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(193, 48)
        Me.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox2.TabIndex = 80
        Me.PictureBox2.TabStop = False
        '
        'bokdisabled
        '
        Me.bokdisabled.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bokdisabled.BackColor = System.Drawing.Color.Transparent
        Me.bokdisabled.Image = CType(resources.GetObject("bokdisabled.Image"), System.Drawing.Image)
        Me.bokdisabled.Location = New System.Drawing.Point(635, 3)
        Me.bokdisabled.Name = "bokdisabled"
        Me.bokdisabled.Size = New System.Drawing.Size(56, 56)
        Me.bokdisabled.TabIndex = 84
        Me.bokdisabled.TabStop = False
        '
        'bcanceldisabled
        '
        Me.bcanceldisabled.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bcanceldisabled.Image = CType(resources.GetObject("bcanceldisabled.Image"), System.Drawing.Image)
        Me.bcanceldisabled.Location = New System.Drawing.Point(573, 3)
        Me.bcanceldisabled.Name = "bcanceldisabled"
        Me.bcanceldisabled.Size = New System.Drawing.Size(56, 56)
        Me.bcanceldisabled.TabIndex = 85
        Me.bcanceldisabled.TabStop = False
        Me.bcanceldisabled.Visible = False
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.White
        Me.Panel1.Controls.Add(Me.bcancel)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.PictureBox2)
        Me.Panel1.Controls.Add(Me.bcanceldisabled)
        Me.Panel1.Controls.Add(Me.bok)
        Me.Panel1.Controls.Add(Me.bokdisabled)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 266)
        Me.Panel1.MinimumSize = New System.Drawing.Size(694, 82)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(694, 82)
        Me.Panel1.TabIndex = 86
        '
        'bc_am_dd_errors
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(694, 348)
        Me.Controls.Add(Me.tvErrors)
        Me.Controls.Add(Me.uxHeaderPanel)
        Me.Controls.Add(Me.Panel1)
        Me.MinimumSize = New System.Drawing.Size(702, 375)
        Me.Name = "bc_am_dd_errors"
        Me.Text = "Deployment Errors"
        Me.uxHeaderPanel.ResumeLayout(False)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxBanner, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bcancel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bok, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bokdisabled, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bcanceldisabled, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tvErrors As System.Windows.Forms.TreeView
    Friend WithEvents uxHeaderPanel As System.Windows.Forms.Panel
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents uxBanner As System.Windows.Forms.PictureBox
    Friend WithEvents bcancel As System.Windows.Forms.PictureBox
    Friend WithEvents bok As System.Windows.Forms.PictureBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents bokdisabled As System.Windows.Forms.PictureBox
    Friend WithEvents bcanceldisabled As System.Windows.Forms.PictureBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
End Class
