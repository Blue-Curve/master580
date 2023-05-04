<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bc_am_attestation
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_attestation))
        Me.lgroup = New DevExpress.XtraEditors.LabelControl()
        Me.LabelControl2 = New DevExpress.XtraEditors.LabelControl()
        Me.LabelControl3 = New DevExpress.XtraEditors.LabelControl()
        Me.lq = New DevExpress.XtraEditors.LabelControl()
        Me.panel1 = New DevExpress.XtraEditors.XtraScrollableControl()
        Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl()
        Me.ppass = New DevExpress.XtraEditors.PictureEdit()
        Me.pfail = New DevExpress.XtraEditors.PictureEdit()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.bcancel = New DevExpress.XtraEditors.SimpleButton()
        Me.btnsubmit = New DevExpress.XtraEditors.SimpleButton()
        Me.uximages = New DevExpress.Utils.ImageCollection()
        Me.ledit = New DevExpress.XtraEditors.LabelControl()
        Me.PanelControl2 = New DevExpress.XtraEditors.PanelControl()
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl1.SuspendLayout()
        CType(Me.ppass.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pfail.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uximages, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PanelControl2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl2.SuspendLayout()
        Me.SuspendLayout()
        '
        'lgroup
        '
        Me.lgroup.Appearance.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lgroup.Location = New System.Drawing.Point(116, 14)
        Me.lgroup.Name = "lgroup"
        Me.lgroup.Size = New System.Drawing.Size(66, 13)
        Me.lgroup.TabIndex = 0
        Me.lgroup.Text = "LabelControl1"
        '
        'LabelControl2
        '
        Me.LabelControl2.Appearance.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl2.Location = New System.Drawing.Point(5, 35)
        Me.LabelControl2.Name = "LabelControl2"
        Me.LabelControl2.Size = New System.Drawing.Size(82, 13)
        Me.LabelControl2.TabIndex = 1
        Me.LabelControl2.Text = "Questionnaire:"
        '
        'LabelControl3
        '
        Me.LabelControl3.Appearance.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl3.Location = New System.Drawing.Point(5, 14)
        Me.LabelControl3.Name = "LabelControl3"
        Me.LabelControl3.Size = New System.Drawing.Size(93, 13)
        Me.LabelControl3.TabIndex = 2
        Me.LabelControl3.Text = "Research Group:"
        '
        'lq
        '
        Me.lq.Location = New System.Drawing.Point(116, 35)
        Me.lq.Name = "lq"
        Me.lq.Size = New System.Drawing.Size(66, 13)
        Me.lq.TabIndex = 3
        Me.lq.Text = "LabelControl4"
        '
        'panel1
        '
        Me.panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.panel1.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.panel1.Appearance.Options.UseBackColor = True
        Me.panel1.Location = New System.Drawing.Point(12, 68)
        Me.panel1.Name = "panel1"
        Me.panel1.Size = New System.Drawing.Size(927, 612)
        Me.panel1.TabIndex = 85
        '
        'PanelControl1
        '
        Me.PanelControl1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PanelControl1.Controls.Add(Me.ppass)
        Me.PanelControl1.Controls.Add(Me.pfail)
        Me.PanelControl1.Controls.Add(Me.PictureBox2)
        Me.PanelControl1.Controls.Add(Me.bcancel)
        Me.PanelControl1.Controls.Add(Me.btnsubmit)
        Me.PanelControl1.Location = New System.Drawing.Point(0, 699)
        Me.PanelControl1.Name = "PanelControl1"
        Me.PanelControl1.Size = New System.Drawing.Size(948, 54)
        Me.PanelControl1.TabIndex = 87
        '
        'ppass
        '
        Me.ppass.EditValue = CType(resources.GetObject("ppass.EditValue"), Object)
        Me.ppass.Location = New System.Drawing.Point(695, 7)
        Me.ppass.Name = "ppass"
        Me.ppass.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.ppass.Properties.Appearance.Options.UseBackColor = True
        Me.ppass.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me.ppass.Properties.ReadOnly = True
        Me.ppass.Size = New System.Drawing.Size(44, 42)
        Me.ppass.TabIndex = 87
        Me.ppass.Visible = False
        '
        'pfail
        '
        Me.pfail.EditValue = CType(resources.GetObject("pfail.EditValue"), Object)
        Me.pfail.Location = New System.Drawing.Point(695, 7)
        Me.pfail.Name = "pfail"
        Me.pfail.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.pfail.Properties.Appearance.Options.UseBackColor = True
        Me.pfail.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me.pfail.Properties.ReadOnly = True
        Me.pfail.Size = New System.Drawing.Size(44, 42)
        Me.pfail.TabIndex = 86
        Me.pfail.Visible = False
        '
        'PictureBox2
        '
        Me.PictureBox2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(8, 13)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(175, 32)
        Me.PictureBox2.TabIndex = 85
        Me.PictureBox2.TabStop = False
        '
        'bcancel
        '
        Me.bcancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bcancel.ImageIndex = 1
        Me.bcancel.Location = New System.Drawing.Point(864, 11)
        Me.bcancel.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.bcancel.Name = "bcancel"
        Me.bcancel.Size = New System.Drawing.Size(75, 34)
        Me.bcancel.TabIndex = 84
        Me.bcancel.Text = "Cancel"
        '
        'btnsubmit
        '
        Me.btnsubmit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnsubmit.ImageIndex = 1
        Me.btnsubmit.Location = New System.Drawing.Point(774, 11)
        Me.btnsubmit.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnsubmit.Name = "btnsubmit"
        Me.btnsubmit.Size = New System.Drawing.Size(75, 34)
        Me.btnsubmit.TabIndex = 72
        Me.btnsubmit.Text = "Save"
        '
        'uximages
        '
        Me.uximages.ImageStream = CType(resources.GetObject("uximages.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
        Me.uximages.Images.SetKeyName(0, "support_32.png")
        Me.uximages.Images.SetKeyName(1, "document_32.png")
        Me.uximages.Images.SetKeyName(2, "checkin_in_force_32.ico")
        Me.uximages.Images.SetKeyName(3, "checkin_in_to_another_32.ico")
        Me.uximages.Images.SetKeyName(4, "checkin_in_to_me_32.ico")
        Me.uximages.Images.SetKeyName(5, "comment_32.ico")
        Me.uximages.Images.SetKeyName(6, "edit_32.ico")
        Me.uximages.Images.SetKeyName(7, "history_32.ico")
        Me.uximages.Images.SetKeyName(8, "links_32.ico")
        Me.uximages.Images.SetKeyName(9, "locked_32.ico")
        Me.uximages.Images.SetKeyName(10, "permissions_32.ico")
        Me.uximages.Images.SetKeyName(11, "read_32.ico")
        Me.uximages.Images.SetKeyName(12, "user_32.ico")
        Me.uximages.Images.SetKeyName(13, "write_32.ico")
        Me.uximages.Images.SetKeyName(14, "attach_32.ico")
        Me.uximages.Images.SetKeyName(15, "categorise_32.ico")
        Me.uximages.Images.SetKeyName(16, "filter_32.ico")
        Me.uximages.Images.SetKeyName(17, "icon1_32.ico")
        Me.uximages.Images.SetKeyName(18, "icon2_32.ico")
        Me.uximages.Images.SetKeyName(19, "icon3_32.ico")
        Me.uximages.Images.SetKeyName(20, "icon4_32.ico")
        Me.uximages.Images.SetKeyName(21, "icon5_32.ico")
        Me.uximages.Images.SetKeyName(22, "icon6_32.ico")
        Me.uximages.Images.SetKeyName(23, "import_32.ico")
        Me.uximages.Images.SetKeyName(24, "refresh_32.ico")
        Me.uximages.Images.SetKeyName(25, "register_32.ico")
        Me.uximages.Images.SetKeyName(26, "registered_document_32.ico")
        Me.uximages.Images.SetKeyName(27, "settings_32.ico")
        Me.uximages.Images.SetKeyName(28, "delete_32.ico")
        Me.uximages.Images.SetKeyName(29, "move_32.ico")
        Me.uximages.Images.SetKeyName(30, "revert_32.ico")
        Me.uximages.Images.SetKeyName(31, "register_32.ico")
        Me.uximages.Images.SetKeyName(32, "delete_32.ico")
        Me.uximages.Images.SetKeyName(33, "move_32.ico")
        Me.uximages.Images.SetKeyName(34, "revert_32.ico")
        Me.uximages.Images.SetKeyName(35, "checkedin_IN_32.ico")
        Me.uximages.Images.SetKeyName(36, "checkedin_OUT_32.ico")
        Me.uximages.Images.SetKeyName(37, "unregistered_60.png")
        Me.uximages.Images.SetKeyName(38, "2000px-Red_x.svg.png")
        Me.uximages.Images.SetKeyName(39, "green-tick.png")
        '
        'ledit
        '
        Me.ledit.Appearance.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ledit.Location = New System.Drawing.Point(262, 14)
        Me.ledit.Name = "ledit"
        Me.ledit.Size = New System.Drawing.Size(0, 13)
        Me.ledit.TabIndex = 88
        '
        'PanelControl2
        '
        Me.PanelControl2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PanelControl2.Controls.Add(Me.LabelControl3)
        Me.PanelControl2.Controls.Add(Me.ledit)
        Me.PanelControl2.Controls.Add(Me.lgroup)
        Me.PanelControl2.Controls.Add(Me.LabelControl2)
        Me.PanelControl2.Controls.Add(Me.lq)
        Me.PanelControl2.Location = New System.Drawing.Point(0, 5)
        Me.PanelControl2.Name = "PanelControl2"
        Me.PanelControl2.Size = New System.Drawing.Size(951, 57)
        Me.PanelControl2.TabIndex = 89
        '
        'bc_am_attestation
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(951, 755)
        Me.Controls.Add(Me.PanelControl2)
        Me.Controls.Add(Me.PanelControl1)
        Me.Controls.Add(Me.panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "bc_am_attestation"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Attestation"
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl1.ResumeLayout(False)
        CType(Me.ppass.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pfail.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uximages, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PanelControl2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl2.ResumeLayout(False)
        Me.PanelControl2.PerformLayout()
        Me.ResumeLayout(False)

End Sub
    Friend WithEvents lgroup As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl2 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl3 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lq As DevExpress.XtraEditors.LabelControl
    Friend WithEvents panel1 As DevExpress.XtraEditors.XtraScrollableControl
    Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents bcancel As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnsubmit As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents ledit As DevExpress.XtraEditors.LabelControl
    Friend WithEvents uximages As DevExpress.Utils.ImageCollection
    Friend WithEvents ppass As DevExpress.XtraEditors.PictureEdit
    Friend WithEvents pfail As DevExpress.XtraEditors.PictureEdit
    Friend WithEvents PanelControl2 As DevExpress.XtraEditors.PanelControl
End Class
