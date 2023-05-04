<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bc_am_picture_attribute
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_picture_attribute))
        Me.PanelControl3 = New DevExpress.XtraEditors.PanelControl()
        Me.bdelete = New DevExpress.XtraEditors.SimpleButton()
        Me.badd = New DevExpress.XtraEditors.SimpleButton()
        Me.bsave = New DevExpress.XtraEditors.SimpleButton()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.bcancel = New DevExpress.XtraEditors.SimpleButton()
        Me.uxpicture = New DevExpress.XtraEditors.PictureEdit()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        CType(Me.PanelControl3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl3.SuspendLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxpicture.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PanelControl3
        '
        Me.PanelControl3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PanelControl3.Controls.Add(Me.bdelete)
        Me.PanelControl3.Controls.Add(Me.badd)
        Me.PanelControl3.Controls.Add(Me.bsave)
        Me.PanelControl3.Controls.Add(Me.PictureBox2)
        Me.PanelControl3.Controls.Add(Me.bcancel)
        Me.PanelControl3.Location = New System.Drawing.Point(0, 506)
        Me.PanelControl3.Name = "PanelControl3"
        Me.PanelControl3.Size = New System.Drawing.Size(966, 46)
        Me.PanelControl3.TabIndex = 51
        '
        'bdelete
        '
        Me.bdelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bdelete.Enabled = False
        Me.bdelete.ImageIndex = 1
        Me.bdelete.Location = New System.Drawing.Point(612, 7)
        Me.bdelete.Name = "bdelete"
        Me.bdelete.Size = New System.Drawing.Size(74, 32)
        Me.bdelete.TabIndex = 50
        Me.bdelete.Text = "Delete"
        '
        'badd
        '
        Me.badd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.badd.ImageIndex = 1
        Me.badd.Location = New System.Drawing.Point(701, 7)
        Me.badd.Name = "badd"
        Me.badd.Size = New System.Drawing.Size(74, 32)
        Me.badd.TabIndex = 49
        Me.badd.Text = "Load"
        '
        'bsave
        '
        Me.bsave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bsave.Enabled = False
        Me.bsave.ImageIndex = 1
        Me.bsave.Location = New System.Drawing.Point(792, 7)
        Me.bsave.Name = "bsave"
        Me.bsave.Size = New System.Drawing.Size(74, 32)
        Me.bsave.TabIndex = 48
        Me.bsave.Text = "Save"
        '
        'PictureBox2
        '
        Me.PictureBox2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(5, 6)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(175, 35)
        Me.PictureBox2.TabIndex = 42
        Me.PictureBox2.TabStop = False
        '
        'bcancel
        '
        Me.bcancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bcancel.ImageIndex = 1
        Me.bcancel.Location = New System.Drawing.Point(882, 6)
        Me.bcancel.Name = "bcancel"
        Me.bcancel.Size = New System.Drawing.Size(74, 32)
        Me.bcancel.TabIndex = 47
        Me.bcancel.Text = "Cancel"
        '
        'uxpicture
        '
        Me.uxpicture.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxpicture.Location = New System.Drawing.Point(5, 12)
        Me.uxpicture.Name = "uxpicture"
        Me.uxpicture.Properties.PictureStoreMode = DevExpress.XtraEditors.Controls.PictureStoreMode.Image
        Me.uxpicture.Properties.ShowMenu = False
        Me.uxpicture.Size = New System.Drawing.Size(961, 459)
        Me.uxpicture.TabIndex = 52
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'bc_am_picture_attribute
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(968, 551)
        Me.Controls.Add(Me.uxpicture)
        Me.Controls.Add(Me.PanelControl3)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimizeBox = False
        Me.Name = "bc_am_picture_attribute"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Upload Image"
        CType(Me.PanelControl3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl3.ResumeLayout(False)
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxpicture.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents PanelControl3 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents bcancel As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents bsave As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents uxpicture As DevExpress.XtraEditors.PictureEdit
    Friend WithEvents badd As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents bdelete As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
End Class
