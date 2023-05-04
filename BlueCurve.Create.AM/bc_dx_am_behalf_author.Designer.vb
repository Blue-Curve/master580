<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bc_dx_am_behalf_author
    Inherits DevExpress.XtraEditors.XtraForm

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_dx_am_behalf_author))
        Me.btnsubmit = New DevExpress.XtraEditors.SimpleButton()
        Me.uxImages = New System.Windows.Forms.ImageList()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.LabelControl7 = New DevExpress.XtraEditors.LabelControl()
        Me.ComboBox1 = New DevExpress.XtraEditors.ComboBoxEdit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ComboBox1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnsubmit
        '
        Me.btnsubmit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnsubmit.ImageIndex = 4
        Me.btnsubmit.ImageList = Me.uxImages
        Me.btnsubmit.Location = New System.Drawing.Point(244, 78)
        Me.btnsubmit.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnsubmit.Name = "btnsubmit"
        Me.btnsubmit.Size = New System.Drawing.Size(75, 33)
        Me.btnsubmit.TabIndex = 65
        Me.btnsubmit.Text = "Continue"
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
        'PictureBox2
        '
        Me.PictureBox2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(12, 77)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(161, 33)
        Me.PictureBox2.TabIndex = 64
        Me.PictureBox2.TabStop = False
        '
        'LabelControl7
        '
        Me.LabelControl7.Location = New System.Drawing.Point(12, 22)
        Me.LabelControl7.Name = "LabelControl7"
        Me.LabelControl7.Size = New System.Drawing.Size(121, 13)
        Me.LabelControl7.TabIndex = 66
        Me.LabelControl7.Text = "Please Choose an Author"
        '
        'ComboBox1
        '
        Me.ComboBox1.Location = New System.Drawing.Point(12, 41)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.ComboBox1.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        Me.ComboBox1.Size = New System.Drawing.Size(307, 20)
        Me.ComboBox1.TabIndex = 67
        '
        'bc_dx_am_behalf_author
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(331, 122)
        Me.Controls.Add(Me.ComboBox1)
        Me.Controls.Add(Me.LabelControl7)
        Me.Controls.Add(Me.btnsubmit)
        Me.Controls.Add(Me.PictureBox2)
        Me.DoubleBuffered = True
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "bc_dx_am_behalf_author"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Blue Curve"
        Me.TopMost = True
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ComboBox1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents btnsubmit As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents uxImages As System.Windows.Forms.ImageList
    Friend WithEvents LabelControl7 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents ComboBox1 As DevExpress.XtraEditors.ComboBoxEdit
End Class
