<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bc_am_cp_distribution
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_cp_distribution))
        Me.PanelControl3 = New DevExpress.XtraEditors.PanelControl()
        Me.LabelControl1 = New DevExpress.XtraEditors.LabelControl()
        Me.cproducttype = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.bsave = New DevExpress.XtraEditors.SimpleButton()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.bcancel = New DevExpress.XtraEditors.SimpleButton()
        Me.XtraTabControl1 = New DevExpress.XtraTab.XtraTabControl()
        Me.XtraTabPage1 = New DevExpress.XtraTab.XtraTabPage()
        Me.rauto = New DevExpress.XtraEditors.RadioGroup()
        Me.chkchannels = New DevExpress.XtraEditors.CheckedListBoxControl()
        Me.cbodytemplate = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.LabelControl3 = New DevExpress.XtraEditors.LabelControl()
        Me.XtraTabPage2 = New DevExpress.XtraTab.XtraTabPage()
        Me.lsel = New DevExpress.XtraEditors.ListBoxControl()
        Me.ptaxsettings = New DevExpress.XtraEditors.PanelControl()
        Me.LabelControl4 = New DevExpress.XtraEditors.LabelControl()
        Me.cdepclass = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.LabelControl2 = New DevExpress.XtraEditors.LabelControl()
        Me.cmaxnum = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.chkdefault = New DevExpress.XtraEditors.CheckEdit()
        Me.chkmandatory = New DevExpress.XtraEditors.CheckEdit()
        Me.bdn = New DevExpress.XtraEditors.SimpleButton()
        Me.bup = New DevExpress.XtraEditors.SimpleButton()
        Me.lall = New DevExpress.XtraEditors.ListBoxControl()
        Me.XtraTabPage3 = New DevExpress.XtraTab.XtraTabPage()
        Me.chkmodules = New DevExpress.XtraEditors.CheckedListBoxControl()
        Me.XtraTabPage4 = New DevExpress.XtraTab.XtraTabPage()
        Me.Dx_uc_attributes1 = New BlueCurve.CommonPlatform.AM.dx_uc_attributes()
        CType(Me.PanelControl3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl3.SuspendLayout()
        CType(Me.cproducttype.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.XtraTabControl1.SuspendLayout()
        Me.XtraTabPage1.SuspendLayout()
        CType(Me.rauto.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chkchannels, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cbodytemplate.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.XtraTabPage2.SuspendLayout()
        CType(Me.lsel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ptaxsettings, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ptaxsettings.SuspendLayout()
        CType(Me.cdepclass.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cmaxnum.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chkdefault.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chkmandatory.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lall, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.XtraTabPage3.SuspendLayout()
        CType(Me.chkmodules, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.XtraTabPage4.SuspendLayout()
        Me.SuspendLayout()
        '
        'PanelControl3
        '
        Me.PanelControl3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PanelControl3.Controls.Add(Me.LabelControl1)
        Me.PanelControl3.Controls.Add(Me.cproducttype)
        Me.PanelControl3.Controls.Add(Me.bsave)
        Me.PanelControl3.Controls.Add(Me.PictureBox2)
        Me.PanelControl3.Controls.Add(Me.bcancel)
        Me.PanelControl3.Location = New System.Drawing.Point(-50, 319)
        Me.PanelControl3.Name = "PanelControl3"
        Me.PanelControl3.Size = New System.Drawing.Size(985, 52)
        Me.PanelControl3.TabIndex = 51
        '
        'LabelControl1
        '
        Me.LabelControl1.Location = New System.Drawing.Point(549, 22)
        Me.LabelControl1.Name = "LabelControl1"
        Me.LabelControl1.Size = New System.Drawing.Size(64, 13)
        Me.LabelControl1.TabIndex = 6
        Me.LabelControl1.Text = "Product Type"
        '
        'cproducttype
        '
        Me.cproducttype.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cproducttype.Location = New System.Drawing.Point(619, 19)
        Me.cproducttype.Name = "cproducttype"
        Me.cproducttype.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.cproducttype.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        Me.cproducttype.Size = New System.Drawing.Size(175, 20)
        Me.cproducttype.TabIndex = 6
        '
        'bsave
        '
        Me.bsave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bsave.ImageIndex = 1
        Me.bsave.Location = New System.Drawing.Point(822, 12)
        Me.bsave.Name = "bsave"
        Me.bsave.Size = New System.Drawing.Size(74, 32)
        Me.bsave.TabIndex = 48
        Me.bsave.Text = "Save"
        '
        'PictureBox2
        '
        Me.PictureBox2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(51, 9)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(175, 35)
        Me.PictureBox2.TabIndex = 42
        Me.PictureBox2.TabStop = False
        '
        'bcancel
        '
        Me.bcancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bcancel.ImageIndex = 1
        Me.bcancel.Location = New System.Drawing.Point(902, 12)
        Me.bcancel.Name = "bcancel"
        Me.bcancel.Size = New System.Drawing.Size(74, 32)
        Me.bcancel.TabIndex = 47
        Me.bcancel.Text = "Cancel"
        '
        'XtraTabControl1
        '
        Me.XtraTabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.XtraTabControl1.Location = New System.Drawing.Point(12, 12)
        Me.XtraTabControl1.Name = "XtraTabControl1"
        Me.XtraTabControl1.SelectedTabPage = Me.XtraTabPage1
        Me.XtraTabControl1.Size = New System.Drawing.Size(907, 289)
        Me.XtraTabControl1.TabIndex = 52
        Me.XtraTabControl1.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.XtraTabPage1, Me.XtraTabPage2, Me.XtraTabPage3, Me.XtraTabPage4})
        '
        'XtraTabPage1
        '
        Me.XtraTabPage1.Controls.Add(Me.rauto)
        Me.XtraTabPage1.Controls.Add(Me.chkchannels)
        Me.XtraTabPage1.Controls.Add(Me.cbodytemplate)
        Me.XtraTabPage1.Controls.Add(Me.LabelControl3)
        Me.XtraTabPage1.Name = "XtraTabPage1"
        Me.XtraTabPage1.Size = New System.Drawing.Size(901, 261)
        Me.XtraTabPage1.Text = "Distribution Channels"
        '
        'rauto
        '
        Me.rauto.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rauto.Location = New System.Drawing.Point(639, 225)
        Me.rauto.Name = "rauto"
        Me.rauto.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.[False]
        Me.rauto.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.RadioGroupItem() {New DevExpress.XtraEditors.Controls.RadioGroupItem(Nothing, "Automatic"), New DevExpress.XtraEditors.Controls.RadioGroupItem(Nothing, "Manual")})
        Me.rauto.Size = New System.Drawing.Size(238, 33)
        Me.rauto.TabIndex = 46
        '
        'chkchannels
        '
        Me.chkchannels.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkchannels.Location = New System.Drawing.Point(12, 17)
        Me.chkchannels.Name = "chkchannels"
        Me.chkchannels.Size = New System.Drawing.Size(875, 202)
        Me.chkchannels.TabIndex = 0
        '
        'cbodytemplate
        '
        Me.cbodytemplate.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cbodytemplate.Location = New System.Drawing.Point(134, 225)
        Me.cbodytemplate.Name = "cbodytemplate"
        Me.cbodytemplate.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.cbodytemplate.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        Me.cbodytemplate.Size = New System.Drawing.Size(422, 20)
        Me.cbodytemplate.TabIndex = 45
        '
        'LabelControl3
        '
        Me.LabelControl3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LabelControl3.Location = New System.Drawing.Point(3, 228)
        Me.LabelControl3.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.LabelControl3.Name = "LabelControl3"
        Me.LabelControl3.Size = New System.Drawing.Size(125, 13)
        Me.LabelControl3.TabIndex = 44
        Me.LabelControl3.Text = "HTML Email body template"
        '
        'XtraTabPage2
        '
        Me.XtraTabPage2.Controls.Add(Me.lsel)
        Me.XtraTabPage2.Controls.Add(Me.ptaxsettings)
        Me.XtraTabPage2.Controls.Add(Me.bdn)
        Me.XtraTabPage2.Controls.Add(Me.bup)
        Me.XtraTabPage2.Controls.Add(Me.lall)
        Me.XtraTabPage2.Name = "XtraTabPage2"
        Me.XtraTabPage2.Size = New System.Drawing.Size(901, 261)
        Me.XtraTabPage2.Text = "Extended Taxonomy"
        '
        'lsel
        '
        Me.lsel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lsel.Location = New System.Drawing.Point(274, 14)
        Me.lsel.Name = "lsel"
        Me.lsel.Size = New System.Drawing.Size(242, 235)
        Me.lsel.TabIndex = 6
        '
        'ptaxsettings
        '
        Me.ptaxsettings.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ptaxsettings.Appearance.BackColor = System.Drawing.Color.White
        Me.ptaxsettings.Appearance.Options.UseBackColor = True
        Me.ptaxsettings.Controls.Add(Me.LabelControl4)
        Me.ptaxsettings.Controls.Add(Me.cdepclass)
        Me.ptaxsettings.Controls.Add(Me.LabelControl2)
        Me.ptaxsettings.Controls.Add(Me.cmaxnum)
        Me.ptaxsettings.Controls.Add(Me.chkdefault)
        Me.ptaxsettings.Controls.Add(Me.chkmandatory)
        Me.ptaxsettings.Enabled = False
        Me.ptaxsettings.Location = New System.Drawing.Point(597, 16)
        Me.ptaxsettings.Name = "ptaxsettings"
        Me.ptaxsettings.Size = New System.Drawing.Size(286, 232)
        Me.ptaxsettings.TabIndex = 51
        '
        'LabelControl4
        '
        Me.LabelControl4.Location = New System.Drawing.Point(25, 67)
        Me.LabelControl4.Name = "LabelControl4"
        Me.LabelControl4.Size = New System.Drawing.Size(98, 13)
        Me.LabelControl4.TabIndex = 5
        Me.LabelControl4.Text = "Dependent On Class"
        '
        'cdepclass
        '
        Me.cdepclass.Location = New System.Drawing.Point(162, 64)
        Me.cdepclass.Name = "cdepclass"
        Me.cdepclass.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.cdepclass.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        Me.cdepclass.Size = New System.Drawing.Size(119, 20)
        Me.cdepclass.TabIndex = 4
        '
        'LabelControl2
        '
        Me.LabelControl2.Location = New System.Drawing.Point(25, 41)
        Me.LabelControl2.Name = "LabelControl2"
        Me.LabelControl2.Size = New System.Drawing.Size(128, 13)
        Me.LabelControl2.TabIndex = 3
        Me.LabelControl2.Text = "Maximum Number Selected"
        '
        'cmaxnum
        '
        Me.cmaxnum.Location = New System.Drawing.Point(162, 38)
        Me.cmaxnum.Name = "cmaxnum"
        Me.cmaxnum.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.cmaxnum.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        Me.cmaxnum.Size = New System.Drawing.Size(119, 20)
        Me.cmaxnum.TabIndex = 2
        '
        'chkdefault
        '
        Me.chkdefault.Location = New System.Drawing.Point(23, 90)
        Me.chkdefault.Name = "chkdefault"
        Me.chkdefault.Properties.Caption = "Default Value"
        Me.chkdefault.Size = New System.Drawing.Size(190, 19)
        Me.chkdefault.TabIndex = 1
        Me.chkdefault.Visible = False
        '
        'chkmandatory
        '
        Me.chkmandatory.Location = New System.Drawing.Point(23, 13)
        Me.chkmandatory.Name = "chkmandatory"
        Me.chkmandatory.Properties.Caption = "Mandatory"
        Me.chkmandatory.Size = New System.Drawing.Size(190, 19)
        Me.chkmandatory.TabIndex = 0
        '
        'bdn
        '
        Me.bdn.Enabled = False
        Me.bdn.ImageIndex = 1
        Me.bdn.Location = New System.Drawing.Point(543, 64)
        Me.bdn.Name = "bdn"
        Me.bdn.Size = New System.Drawing.Size(37, 32)
        Me.bdn.TabIndex = 50
        Me.bdn.Text = "Dn"
        '
        'bup
        '
        Me.bup.Enabled = False
        Me.bup.ImageIndex = 1
        Me.bup.Location = New System.Drawing.Point(543, 16)
        Me.bup.Name = "bup"
        Me.bup.Size = New System.Drawing.Size(37, 32)
        Me.bup.TabIndex = 49
        Me.bup.Text = "Up"
        '
        'lall
        '
        Me.lall.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lall.Location = New System.Drawing.Point(13, 14)
        Me.lall.Name = "lall"
        Me.lall.Size = New System.Drawing.Size(242, 235)
        Me.lall.TabIndex = 1
        '
        'XtraTabPage3
        '
        Me.XtraTabPage3.Controls.Add(Me.chkmodules)
        Me.XtraTabPage3.Name = "XtraTabPage3"
        Me.XtraTabPage3.Size = New System.Drawing.Size(901, 261)
        Me.XtraTabPage3.Text = "Modules"
        '
        'chkmodules
        '
        Me.chkmodules.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkmodules.Location = New System.Drawing.Point(3, 3)
        Me.chkmodules.Name = "chkmodules"
        Me.chkmodules.Size = New System.Drawing.Size(875, 255)
        Me.chkmodules.TabIndex = 1
        '
        'XtraTabPage4
        '
        Me.XtraTabPage4.Controls.Add(Me.Dx_uc_attributes1)
        Me.XtraTabPage4.Name = "XtraTabPage4"
        Me.XtraTabPage4.Size = New System.Drawing.Size(901, 261)
        Me.XtraTabPage4.Text = "Attributes"
        '
        'Dx_uc_attributes1
        '
        Me.Dx_uc_attributes1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Dx_uc_attributes1.Location = New System.Drawing.Point(0, 3)
        Me.Dx_uc_attributes1.Name = "Dx_uc_attributes1"
        Me.Dx_uc_attributes1.Size = New System.Drawing.Size(898, 258)
        Me.Dx_uc_attributes1.TabIndex = 53
        '
        'bc_am_cp_distribution
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(931, 370)
        Me.Controls.Add(Me.XtraTabControl1)
        Me.Controls.Add(Me.PanelControl3)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimizeBox = False
        Me.Name = "bc_am_cp_distribution"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Publication Type Distribution"
        CType(Me.PanelControl3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl3.ResumeLayout(False)
        Me.PanelControl3.PerformLayout()
        CType(Me.cproducttype.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.XtraTabControl1.ResumeLayout(False)
        Me.XtraTabPage1.ResumeLayout(False)
        Me.XtraTabPage1.PerformLayout()
        CType(Me.rauto.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chkchannels, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cbodytemplate.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.XtraTabPage2.ResumeLayout(False)
        CType(Me.lsel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ptaxsettings, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ptaxsettings.ResumeLayout(False)
        Me.ptaxsettings.PerformLayout()
        CType(Me.cdepclass.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cmaxnum.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chkdefault.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chkmandatory.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lall, System.ComponentModel.ISupportInitialize).EndInit()
        Me.XtraTabPage3.ResumeLayout(False)
        CType(Me.chkmodules, System.ComponentModel.ISupportInitialize).EndInit()
        Me.XtraTabPage4.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents PanelControl3 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents bsave As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents bcancel As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents XtraTabControl1 As DevExpress.XtraTab.XtraTabControl
    Friend WithEvents XtraTabPage1 As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents chkchannels As DevExpress.XtraEditors.CheckedListBoxControl
    Friend WithEvents XtraTabPage2 As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents bdn As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents bup As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents lall As DevExpress.XtraEditors.ListBoxControl
    Friend WithEvents ptaxsettings As DevExpress.XtraEditors.PanelControl
    Friend WithEvents cbodytemplate As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents LabelControl3 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl4 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cdepclass As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents LabelControl2 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cmaxnum As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents chkdefault As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents chkmandatory As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents lsel As DevExpress.XtraEditors.ListBoxControl
    Friend WithEvents rauto As DevExpress.XtraEditors.RadioGroup
    Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cproducttype As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents XtraTabPage3 As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents chkmodules As DevExpress.XtraEditors.CheckedListBoxControl
    Friend WithEvents XtraTabPage4 As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents Dx_uc_attributes1 As BlueCurve.CommonPlatform.AM.dx_uc_attributes
End Class
