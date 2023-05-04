<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bc_am_clone_document
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_clone_document))
        Me.Ldocs = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader4 = New System.Windows.Forms.ColumnHeader
        Me.bleftgrey = New System.Windows.Forms.PictureBox
        Me.bok = New System.Windows.Forms.PictureBox
        Me.Btnback = New System.Windows.Forms.PictureBox
        Me.bokgrey = New System.Windows.Forms.PictureBox
        Me.ltitle = New System.Windows.Forms.Label
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Label3 = New System.Windows.Forms.Label
        Me.PictureBox2 = New System.Windows.Forms.PictureBox
        CType(Me.bleftgrey, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bok, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Btnback, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bokgrey, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Ldocs
        '
        Me.Ldocs.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4})
        Me.Ldocs.FullRowSelect = True
        Me.Ldocs.Location = New System.Drawing.Point(17, 34)
        Me.Ldocs.Name = "Ldocs"
        Me.Ldocs.Size = New System.Drawing.Size(663, 250)
        Me.Ldocs.TabIndex = 0
        Me.Ldocs.UseCompatibleStateImageBehavior = False
        Me.Ldocs.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Title"
        Me.ColumnHeader1.Width = 534
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Date"
        Me.ColumnHeader2.Width = 122
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = ""
        Me.ColumnHeader3.Width = 0
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = ""
        Me.ColumnHeader4.Width = 0
        '
        'bleftgrey
        '
        Me.bleftgrey.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bleftgrey.Image = CType(resources.GetObject("bleftgrey.Image"), System.Drawing.Image)
        Me.bleftgrey.Location = New System.Drawing.Point(562, 298)
        Me.bleftgrey.Name = "bleftgrey"
        Me.bleftgrey.Size = New System.Drawing.Size(56, 56)
        Me.bleftgrey.TabIndex = 88
        Me.bleftgrey.TabStop = False
        '
        'bok
        '
        Me.bok.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bok.BackColor = System.Drawing.Color.Transparent
        Me.bok.Image = CType(resources.GetObject("bok.Image"), System.Drawing.Image)
        Me.bok.Location = New System.Drawing.Point(624, 298)
        Me.bok.Name = "bok"
        Me.bok.Size = New System.Drawing.Size(56, 56)
        Me.bok.TabIndex = 90
        Me.bok.TabStop = False
        '
        'Btnback
        '
        Me.Btnback.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Btnback.Image = CType(resources.GetObject("Btnback.Image"), System.Drawing.Image)
        Me.Btnback.Location = New System.Drawing.Point(562, 298)
        Me.Btnback.Name = "Btnback"
        Me.Btnback.Size = New System.Drawing.Size(56, 56)
        Me.Btnback.TabIndex = 91
        Me.Btnback.TabStop = False
        Me.Btnback.Visible = False
        '
        'bokgrey
        '
        Me.bokgrey.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bokgrey.BackColor = System.Drawing.Color.Transparent
        Me.bokgrey.Image = CType(resources.GetObject("bokgrey.Image"), System.Drawing.Image)
        Me.bokgrey.Location = New System.Drawing.Point(624, 298)
        Me.bokgrey.Name = "bokgrey"
        Me.bokgrey.Size = New System.Drawing.Size(56, 56)
        Me.bokgrey.TabIndex = 89
        Me.bokgrey.TabStop = False
        Me.bokgrey.Visible = False
        '
        'ltitle
        '
        Me.ltitle.AutoSize = True
        Me.ltitle.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ltitle.Location = New System.Drawing.Point(14, 9)
        Me.ltitle.Name = "ltitle"
        Me.ltitle.Size = New System.Drawing.Size(50, 13)
        Me.ltitle.TabIndex = 92
        Me.ltitle.Text = "Label1"
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.White
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.PictureBox2)
        Me.Panel1.ForeColor = System.Drawing.Color.Coral
        Me.Panel1.Location = New System.Drawing.Point(-6, 294)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(702, 72)
        Me.Panel1.TabIndex = 93
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label3.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Black
        Me.Label3.Location = New System.Drawing.Point(16, 39)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(248, 21)
        Me.Label3.TabIndex = 94
        Me.Label3.Text = "© Blue Curve Limited. All rights reserved."
        '
        'PictureBox2
        '
        Me.PictureBox2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(18, 4)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(184, 48)
        Me.PictureBox2.TabIndex = 94
        Me.PictureBox2.TabStop = False
        '
        'bc_am_clone_document
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(692, 363)
        Me.Controls.Add(Me.ltitle)
        Me.Controls.Add(Me.bleftgrey)
        Me.Controls.Add(Me.bok)
        Me.Controls.Add(Me.Btnback)
        Me.Controls.Add(Me.bokgrey)
        Me.Controls.Add(Me.Ldocs)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "bc_am_clone_document"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Blue Curve - Document Clone"
        CType(Me.bleftgrey, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bok, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Btnback, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bokgrey, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Ldocs As System.Windows.Forms.ListView
    Friend WithEvents bleftgrey As System.Windows.Forms.PictureBox
    Friend WithEvents bok As System.Windows.Forms.PictureBox
    Friend WithEvents Btnback As System.Windows.Forms.PictureBox
    Friend WithEvents bokgrey As System.Windows.Forms.PictureBox
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ltitle As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
End Class
