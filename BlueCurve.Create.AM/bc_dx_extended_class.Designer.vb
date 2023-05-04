<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bc_dx_extended_class
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_dx_extended_class))
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.btnsubmit = New DevExpress.XtraEditors.SimpleButton()
        Me.uxImages = New System.Windows.Forms.ImageList()
        Me.bcancel = New DevExpress.XtraEditors.SimpleButton()
        Me.LabelControl7 = New DevExpress.XtraEditors.LabelControl()
        Me.timportant = New DevExpress.XtraEditors.MemoEdit()
        Me.tabcontrol1 = New DevExpress.XtraTab.XtraTabControl()
        Me.litems = New DevExpress.XtraEditors.CheckedListBoxControl()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.timportant.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.tabcontrol1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.litems, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PictureBox2
        '
        Me.PictureBox2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(10, 352)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(188, 34)
        Me.PictureBox2.TabIndex = 64
        Me.PictureBox2.TabStop = False
        '
        'btnsubmit
        '
        Me.btnsubmit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnsubmit.ImageIndex = 1
        Me.btnsubmit.ImageList = Me.uxImages
        Me.btnsubmit.Location = New System.Drawing.Point(560, 351)
        Me.btnsubmit.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnsubmit.Name = "btnsubmit"
        Me.btnsubmit.Size = New System.Drawing.Size(75, 34)
        Me.btnsubmit.TabIndex = 71
        Me.btnsubmit.Text = "Save"
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
        'bcancel
        '
        Me.bcancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bcancel.ImageIndex = 2
        Me.bcancel.ImageList = Me.uxImages
        Me.bcancel.Location = New System.Drawing.Point(469, 352)
        Me.bcancel.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.bcancel.Name = "bcancel"
        Me.bcancel.Size = New System.Drawing.Size(75, 34)
        Me.bcancel.TabIndex = 70
        Me.bcancel.Text = "Cancel"
        '
        'LabelControl7
        '
        Me.LabelControl7.Location = New System.Drawing.Point(12, 20)
        Me.LabelControl7.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.LabelControl7.Name = "LabelControl7"
        Me.LabelControl7.Size = New System.Drawing.Size(53, 13)
        Me.LabelControl7.TabIndex = 73
        Me.LabelControl7.Text = "Description"
        '
        'timportant
        '
        Me.timportant.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.timportant.EditValue = "Description"
        Me.timportant.Location = New System.Drawing.Point(12, 37)
        Me.timportant.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.timportant.Name = "timportant"
        Me.timportant.Properties.Appearance.BackColor = System.Drawing.Color.White
        Me.timportant.Properties.Appearance.Options.UseBackColor = True
        Me.timportant.Size = New System.Drawing.Size(623, 141)
        Me.timportant.TabIndex = 72
        '
        'tabcontrol1
        '
        Me.tabcontrol1.Location = New System.Drawing.Point(12, 185)
        Me.tabcontrol1.Name = "tabcontrol1"
        Me.tabcontrol1.Size = New System.Drawing.Size(623, 24)
        Me.tabcontrol1.TabIndex = 74
        '
        'litems
        '
        Me.litems.Location = New System.Drawing.Point(12, 205)
        Me.litems.Name = "litems"
        Me.litems.Size = New System.Drawing.Size(623, 140)
        Me.litems.TabIndex = 75
        '
        'bc_dx_extended_class
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(647, 396)
        Me.ControlBox = False
        Me.Controls.Add(Me.litems)
        Me.Controls.Add(Me.tabcontrol1)
        Me.Controls.Add(Me.LabelControl7)
        Me.Controls.Add(Me.timportant)
        Me.Controls.Add(Me.btnsubmit)
        Me.Controls.Add(Me.bcancel)
        Me.Controls.Add(Me.PictureBox2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "bc_dx_extended_class"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Important Document"
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.timportant.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.tabcontrol1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.litems, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents btnsubmit As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents bcancel As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents uxImages As System.Windows.Forms.ImageList
    Friend WithEvents LabelControl7 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents timportant As DevExpress.XtraEditors.MemoEdit
    Friend WithEvents tabcontrol1 As DevExpress.XtraTab.XtraTabControl
    Friend WithEvents litems As DevExpress.XtraEditors.CheckedListBoxControl
End Class
