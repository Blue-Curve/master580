


<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bc_am_distribution_admin
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
    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()
        DevExpress.Skins.SkinManager.EnableFormSkins()
        DevExpress.UserSkins.BonusSkins.Register()
        DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle("Blue")
        ' Add any initialization after the InitializeComponent() call.

    End Sub
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_distribution_admin))
        Me.bcancel = New DevExpress.XtraEditors.SimpleButton()
        Me.bsave = New DevExpress.XtraEditors.SimpleButton()
        Me.PanelControl3 = New DevExpress.XtraEditors.PanelControl()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl()
        Me.LabelControl1 = New DevExpress.XtraEditors.LabelControl()
        Me.LabelControl4 = New DevExpress.XtraEditors.LabelControl()
        Me.uxsel = New DevExpress.XtraEditors.ListBoxControl()
        Me.uxall = New DevExpress.XtraEditors.ListBoxControl()
        Me.ptemplate = New DevExpress.XtraEditors.PanelControl()
        Me.uxbodytemplate = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.uxheadertemplate = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.LabelControl3 = New DevExpress.XtraEditors.LabelControl()
        Me.LabelControl2 = New DevExpress.XtraEditors.LabelControl()
        Me.rtype = New DevExpress.XtraEditors.RadioGroup()
        CType(Me.PanelControl3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl3.SuspendLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl1.SuspendLayout()
        CType(Me.uxsel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxall, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ptemplate, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ptemplate.SuspendLayout()
        CType(Me.uxbodytemplate.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxheadertemplate.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rtype.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'bcancel
        '
        Me.bcancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bcancel.ImageIndex = 1
        Me.bcancel.Location = New System.Drawing.Point(487, 9)
        Me.bcancel.Name = "bcancel"
        Me.bcancel.Size = New System.Drawing.Size(74, 32)
        Me.bcancel.TabIndex = 47
        Me.bcancel.Text = "Cancel"
        '
        'bsave
        '
        Me.bsave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bsave.ImageIndex = 1
        Me.bsave.Location = New System.Drawing.Point(394, 9)
        Me.bsave.Name = "bsave"
        Me.bsave.Size = New System.Drawing.Size(74, 32)
        Me.bsave.TabIndex = 48
        Me.bsave.Text = "Save"
        '
        'PanelControl3
        '
        Me.PanelControl3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PanelControl3.Controls.Add(Me.rtype)
        Me.PanelControl3.Controls.Add(Me.PictureBox2)
        Me.PanelControl3.Controls.Add(Me.bsave)
        Me.PanelControl3.Controls.Add(Me.bcancel)
        Me.PanelControl3.Location = New System.Drawing.Point(2, 253)
        Me.PanelControl3.Name = "PanelControl3"
        Me.PanelControl3.Size = New System.Drawing.Size(570, 49)
        Me.PanelControl3.TabIndex = 49
        '
        'PictureBox2
        '
        Me.PictureBox2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(5, 9)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(175, 35)
        Me.PictureBox2.TabIndex = 42
        Me.PictureBox2.TabStop = False
        '
        'PanelControl1
        '
        Me.PanelControl1.Controls.Add(Me.LabelControl1)
        Me.PanelControl1.Controls.Add(Me.LabelControl4)
        Me.PanelControl1.Controls.Add(Me.uxsel)
        Me.PanelControl1.Controls.Add(Me.uxall)
        Me.PanelControl1.Location = New System.Drawing.Point(2, 14)
        Me.PanelControl1.Name = "PanelControl1"
        Me.PanelControl1.Size = New System.Drawing.Size(569, 134)
        Me.PanelControl1.TabIndex = 50
        '
        'LabelControl1
        '
        Me.LabelControl1.Location = New System.Drawing.Point(10, 12)
        Me.LabelControl1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.LabelControl1.Name = "LabelControl1"
        Me.LabelControl1.Size = New System.Drawing.Size(90, 13)
        Me.LabelControl1.TabIndex = 38
        Me.LabelControl1.Text = "Available Channels"
        '
        'LabelControl4
        '
        Me.LabelControl4.Location = New System.Drawing.Point(291, 12)
        Me.LabelControl4.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.LabelControl4.Name = "LabelControl4"
        Me.LabelControl4.Size = New System.Drawing.Size(88, 13)
        Me.LabelControl4.TabIndex = 37
        Me.LabelControl4.Text = "Selected Channels"
        '
        'uxsel
        '
        Me.uxsel.Location = New System.Drawing.Point(291, 30)
        Me.uxsel.Name = "uxsel"
        Me.uxsel.Size = New System.Drawing.Size(263, 94)
        Me.uxsel.TabIndex = 2
        '
        'uxall
        '
        Me.uxall.Location = New System.Drawing.Point(10, 30)
        Me.uxall.Name = "uxall"
        Me.uxall.Size = New System.Drawing.Size(263, 94)
        Me.uxall.TabIndex = 1
        '
        'ptemplate
        '
        Me.ptemplate.Controls.Add(Me.uxbodytemplate)
        Me.ptemplate.Controls.Add(Me.uxheadertemplate)
        Me.ptemplate.Controls.Add(Me.LabelControl3)
        Me.ptemplate.Controls.Add(Me.LabelControl2)
        Me.ptemplate.Enabled = False
        Me.ptemplate.Location = New System.Drawing.Point(2, 154)
        Me.ptemplate.Name = "ptemplate"
        Me.ptemplate.Size = New System.Drawing.Size(569, 91)
        Me.ptemplate.TabIndex = 51
        '
        'uxbodytemplate
        '
        Me.uxbodytemplate.Location = New System.Drawing.Point(132, 54)
        Me.uxbodytemplate.Name = "uxbodytemplate"
        Me.uxbodytemplate.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.uxbodytemplate.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        Me.uxbodytemplate.Size = New System.Drawing.Size(422, 20)
        Me.uxbodytemplate.TabIndex = 43
        '
        'uxheadertemplate
        '
        Me.uxheadertemplate.Location = New System.Drawing.Point(132, 18)
        Me.uxheadertemplate.Name = "uxheadertemplate"
        Me.uxheadertemplate.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.uxheadertemplate.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        Me.uxheadertemplate.Size = New System.Drawing.Size(422, 20)
        Me.uxheadertemplate.TabIndex = 42
        '
        'LabelControl3
        '
        Me.LabelControl3.Location = New System.Drawing.Point(10, 57)
        Me.LabelControl3.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.LabelControl3.Name = "LabelControl3"
        Me.LabelControl3.Size = New System.Drawing.Size(71, 13)
        Me.LabelControl3.TabIndex = 41
        Me.LabelControl3.Text = "Body Template"
        '
        'LabelControl2
        '
        Me.LabelControl2.Location = New System.Drawing.Point(10, 21)
        Me.LabelControl2.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.LabelControl2.Name = "LabelControl2"
        Me.LabelControl2.Size = New System.Drawing.Size(82, 13)
        Me.LabelControl2.TabIndex = 39
        Me.LabelControl2.Text = "Header Template"
        '
        'rtype
        '
        Me.rtype.Location = New System.Drawing.Point(202, 10)
        Me.rtype.Name = "rtype"
        Me.rtype.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.rtype.Properties.Appearance.Options.UseBackColor = True
        Me.rtype.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.RadioGroupItem() {New DevExpress.XtraEditors.Controls.RadioGroupItem(Nothing, "Manual"), New DevExpress.XtraEditors.Controls.RadioGroupItem(Nothing, "Automatic")})
        Me.rtype.Size = New System.Drawing.Size(163, 30)
        Me.rtype.TabIndex = 49
        '
        'bc_am_distribution_admin
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(576, 304)
        Me.Controls.Add(Me.ptemplate)
        Me.Controls.Add(Me.PanelControl1)
        Me.Controls.Add(Me.PanelControl3)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "bc_am_distribution_admin"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Distribution for Publication Type"
        CType(Me.PanelControl3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl3.ResumeLayout(False)
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl1.ResumeLayout(False)
        Me.PanelControl1.PerformLayout()
        CType(Me.uxsel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxall, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ptemplate, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ptemplate.ResumeLayout(False)
        Me.ptemplate.PerformLayout()
        CType(Me.uxbodytemplate.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxheadertemplate.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rtype.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents bcancel As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents bsave As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents PanelControl3 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl4 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents uxsel As DevExpress.XtraEditors.ListBoxControl
    Friend WithEvents uxall As DevExpress.XtraEditors.ListBoxControl
    Friend WithEvents ptemplate As DevExpress.XtraEditors.PanelControl
    Friend WithEvents uxbodytemplate As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents uxheadertemplate As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents LabelControl3 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl2 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents rtype As DevExpress.XtraEditors.RadioGroup
End Class
