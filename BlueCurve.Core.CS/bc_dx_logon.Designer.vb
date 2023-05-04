<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bc_dx_logon
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_dx_logon))
        Me.bcancel = New DevExpress.XtraEditors.SimpleButton()
        Me.bok = New DevExpress.XtraEditors.SimpleButton()
        Me.uxusername = New DevExpress.XtraEditors.TextEdit()
        Me.LabelControl4 = New DevExpress.XtraEditors.LabelControl()
        Me.LabelControl1 = New DevExpress.XtraEditors.LabelControl()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.XtraTabControl1 = New DevExpress.XtraTab.XtraTabControl()
        Me.XtraTabPage1 = New DevExpress.XtraTab.XtraTabPage()
        Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl()
        Me.uxpassword = New DevExpress.XtraEditors.TextEdit()
        Me.XtraTabPage2 = New DevExpress.XtraTab.XtraTabPage()
        Me.SimpleButton1 = New DevExpress.XtraEditors.SimpleButton()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.PanelControl2 = New DevExpress.XtraEditors.PanelControl()
        Me.LabelControl2 = New DevExpress.XtraEditors.LabelControl()
        Me.uxpasswordresetemail = New DevExpress.XtraEditors.TextEdit()
        Me.XtraTabPage3 = New DevExpress.XtraTab.XtraTabPage()
        Me.SimpleButton2 = New DevExpress.XtraEditors.SimpleButton()
        Me.PictureBox3 = New System.Windows.Forms.PictureBox()
        Me.PanelControl3 = New DevExpress.XtraEditors.PanelControl()
        Me.uxcode = New DevExpress.XtraEditors.TextEdit()
        Me.LabelControl7 = New DevExpress.XtraEditors.LabelControl()
        Me.LabelControl3 = New DevExpress.XtraEditors.LabelControl()
        Me.uxp2 = New DevExpress.XtraEditors.TextEdit()
        Me.LabelControl5 = New DevExpress.XtraEditors.LabelControl()
        Me.uxchemail = New DevExpress.XtraEditors.TextEdit()
        Me.LabelControl6 = New DevExpress.XtraEditors.LabelControl()
        Me.uxp1 = New DevExpress.XtraEditors.TextEdit()
        CType(Me.uxusername.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.XtraTabControl1.SuspendLayout()
        Me.XtraTabPage1.SuspendLayout()
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl1.SuspendLayout()
        CType(Me.uxpassword.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.XtraTabPage2.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PanelControl2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl2.SuspendLayout()
        CType(Me.uxpasswordresetemail.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.XtraTabPage3.SuspendLayout()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PanelControl3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl3.SuspendLayout()
        CType(Me.uxcode.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxp2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxchemail.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxp1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'bcancel
        '
        Me.bcancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bcancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.bcancel.ImageIndex = 2
        Me.bcancel.Location = New System.Drawing.Point(338, 183)
        Me.bcancel.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.bcancel.Name = "bcancel"
        Me.bcancel.Size = New System.Drawing.Size(75, 33)
        Me.bcancel.TabIndex = 3
        Me.bcancel.Text = "Cancel"
        '
        'bok
        '
        Me.bok.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bok.Enabled = False
        Me.bok.ImageIndex = 4
        Me.bok.Location = New System.Drawing.Point(250, 183)
        Me.bok.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.bok.Name = "bok"
        Me.bok.Size = New System.Drawing.Size(75, 33)
        Me.bok.TabIndex = 2
        Me.bok.Text = "Logon"
        '
        'uxusername
        '
        Me.uxusername.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxusername.Location = New System.Drawing.Point(11, 42)
        Me.uxusername.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.uxusername.Name = "uxusername"
        Me.uxusername.Properties.MaxLength = 100
        Me.uxusername.Size = New System.Drawing.Size(402, 20)
        Me.uxusername.TabIndex = 4
        '
        'LabelControl4
        '
        Me.LabelControl4.Location = New System.Drawing.Point(11, 25)
        Me.LabelControl4.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.LabelControl4.Name = "LabelControl4"
        Me.LabelControl4.Size = New System.Drawing.Size(66, 13)
        Me.LabelControl4.TabIndex = 37
        Me.LabelControl4.Text = "Email Address"
        '
        'LabelControl1
        '
        Me.LabelControl1.Location = New System.Drawing.Point(11, 66)
        Me.LabelControl1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.LabelControl1.Name = "LabelControl1"
        Me.LabelControl1.Size = New System.Drawing.Size(46, 13)
        Me.LabelControl1.TabIndex = 38
        Me.LabelControl1.Text = "Password"
        '
        'PictureBox2
        '
        Me.PictureBox2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(3, 184)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(188, 34)
        Me.PictureBox2.TabIndex = 66
        Me.PictureBox2.TabStop = False
        '
        'XtraTabControl1
        '
        Me.XtraTabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.XtraTabControl1.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(236, Byte), Integer), CType(CType(239, Byte), Integer))
        Me.XtraTabControl1.Appearance.Options.UseBackColor = True
        Me.XtraTabControl1.Location = New System.Drawing.Point(-2, 12)
        Me.XtraTabControl1.Name = "XtraTabControl1"
        Me.XtraTabControl1.SelectedTabPage = Me.XtraTabPage1
        Me.XtraTabControl1.Size = New System.Drawing.Size(434, 249)
        Me.XtraTabControl1.TabIndex = 67
        Me.XtraTabControl1.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.XtraTabPage1, Me.XtraTabPage2, Me.XtraTabPage3})
        '
        'XtraTabPage1
        '
        Me.XtraTabPage1.Appearance.PageClient.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(236, Byte), Integer), CType(CType(239, Byte), Integer))
        Me.XtraTabPage1.Appearance.PageClient.Options.UseBackColor = True
        Me.XtraTabPage1.Controls.Add(Me.PanelControl1)
        Me.XtraTabPage1.Controls.Add(Me.PictureBox2)
        Me.XtraTabPage1.Controls.Add(Me.bok)
        Me.XtraTabPage1.Controls.Add(Me.bcancel)
        Me.XtraTabPage1.Name = "XtraTabPage1"
        Me.XtraTabPage1.Size = New System.Drawing.Size(428, 221)
        Me.XtraTabPage1.Text = "Logon"
        '
        'PanelControl1
        '
        Me.PanelControl1.Controls.Add(Me.uxpassword)
        Me.PanelControl1.Controls.Add(Me.LabelControl4)
        Me.PanelControl1.Controls.Add(Me.uxusername)
        Me.PanelControl1.Controls.Add(Me.LabelControl1)
        Me.PanelControl1.Location = New System.Drawing.Point(0, 0)
        Me.PanelControl1.Name = "PanelControl1"
        Me.PanelControl1.Size = New System.Drawing.Size(431, 178)
        Me.PanelControl1.TabIndex = 67
        '
        'uxpassword
        '
        Me.uxpassword.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxpassword.EditValue = ""
        Me.uxpassword.Location = New System.Drawing.Point(11, 83)
        Me.uxpassword.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.uxpassword.Name = "uxpassword"
        Me.uxpassword.Properties.DisplayFormat.FormatString = "****"
        Me.uxpassword.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom
        Me.uxpassword.Properties.MaxLength = 100
        Me.uxpassword.Properties.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.uxpassword.Size = New System.Drawing.Size(402, 20)
        Me.uxpassword.TabIndex = 39
        '
        'XtraTabPage2
        '
        Me.XtraTabPage2.Controls.Add(Me.SimpleButton1)
        Me.XtraTabPage2.Controls.Add(Me.PictureBox1)
        Me.XtraTabPage2.Controls.Add(Me.PanelControl2)
        Me.XtraTabPage2.Name = "XtraTabPage2"
        Me.XtraTabPage2.Size = New System.Drawing.Size(428, 221)
        Me.XtraTabPage2.Text = "Password Reset Request"
        '
        'SimpleButton1
        '
        Me.SimpleButton1.Enabled = False
        Me.SimpleButton1.Location = New System.Drawing.Point(338, 184)
        Me.SimpleButton1.Name = "SimpleButton1"
        Me.SimpleButton1.Size = New System.Drawing.Size(75, 33)
        Me.SimpleButton1.TabIndex = 40
        Me.SimpleButton1.Text = "Request"
        '
        'PictureBox1
        '
        Me.PictureBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(3, 184)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(188, 34)
        Me.PictureBox1.TabIndex = 71
        Me.PictureBox1.TabStop = False
        '
        'PanelControl2
        '
        Me.PanelControl2.Controls.Add(Me.LabelControl2)
        Me.PanelControl2.Controls.Add(Me.uxpasswordresetemail)
        Me.PanelControl2.Location = New System.Drawing.Point(0, 0)
        Me.PanelControl2.Name = "PanelControl2"
        Me.PanelControl2.Size = New System.Drawing.Size(431, 178)
        Me.PanelControl2.TabIndex = 68
        '
        'LabelControl2
        '
        Me.LabelControl2.Location = New System.Drawing.Point(11, 25)
        Me.LabelControl2.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.LabelControl2.Name = "LabelControl2"
        Me.LabelControl2.Size = New System.Drawing.Size(361, 13)
        Me.LabelControl2.TabIndex = 37
        Me.LabelControl2.Text = "Please enter your Email Address to receive a password reset code via Email"
        '
        'uxpasswordresetemail
        '
        Me.uxpasswordresetemail.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxpasswordresetemail.Location = New System.Drawing.Point(11, 54)
        Me.uxpasswordresetemail.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.uxpasswordresetemail.Name = "uxpasswordresetemail"
        Me.uxpasswordresetemail.Properties.MaxLength = 100
        Me.uxpasswordresetemail.Size = New System.Drawing.Size(402, 20)
        Me.uxpasswordresetemail.TabIndex = 4
        '
        'XtraTabPage3
        '
        Me.XtraTabPage3.Controls.Add(Me.SimpleButton2)
        Me.XtraTabPage3.Controls.Add(Me.PictureBox3)
        Me.XtraTabPage3.Controls.Add(Me.PanelControl3)
        Me.XtraTabPage3.Name = "XtraTabPage3"
        Me.XtraTabPage3.Size = New System.Drawing.Size(428, 221)
        Me.XtraTabPage3.Text = "Reset Password"
        '
        'SimpleButton2
        '
        Me.SimpleButton2.Location = New System.Drawing.Point(340, 184)
        Me.SimpleButton2.Name = "SimpleButton2"
        Me.SimpleButton2.Size = New System.Drawing.Size(75, 33)
        Me.SimpleButton2.TabIndex = 72
        Me.SimpleButton2.Text = "Reset"
        '
        'PictureBox3
        '
        Me.PictureBox3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PictureBox3.Image = CType(resources.GetObject("PictureBox3.Image"), System.Drawing.Image)
        Me.PictureBox3.Location = New System.Drawing.Point(3, 184)
        Me.PictureBox3.Name = "PictureBox3"
        Me.PictureBox3.Size = New System.Drawing.Size(188, 34)
        Me.PictureBox3.TabIndex = 71
        Me.PictureBox3.TabStop = False
        '
        'PanelControl3
        '
        Me.PanelControl3.Controls.Add(Me.uxcode)
        Me.PanelControl3.Controls.Add(Me.LabelControl7)
        Me.PanelControl3.Controls.Add(Me.LabelControl3)
        Me.PanelControl3.Controls.Add(Me.uxp2)
        Me.PanelControl3.Controls.Add(Me.LabelControl5)
        Me.PanelControl3.Controls.Add(Me.uxchemail)
        Me.PanelControl3.Controls.Add(Me.LabelControl6)
        Me.PanelControl3.Controls.Add(Me.uxp1)
        Me.PanelControl3.Location = New System.Drawing.Point(0, 0)
        Me.PanelControl3.Name = "PanelControl3"
        Me.PanelControl3.Size = New System.Drawing.Size(433, 178)
        Me.PanelControl3.TabIndex = 68
        '
        'uxcode
        '
        Me.uxcode.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxcode.Location = New System.Drawing.Point(11, 20)
        Me.uxcode.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.uxcode.Name = "uxcode"
        Me.uxcode.Properties.MaxLength = 100
        Me.uxcode.Size = New System.Drawing.Size(404, 20)
        Me.uxcode.TabIndex = 42
        '
        'LabelControl7
        '
        Me.LabelControl7.Location = New System.Drawing.Point(11, 4)
        Me.LabelControl7.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.LabelControl7.Name = "LabelControl7"
        Me.LabelControl7.Size = New System.Drawing.Size(25, 13)
        Me.LabelControl7.TabIndex = 41
        Me.LabelControl7.Text = "Code"
        '
        'LabelControl3
        '
        Me.LabelControl3.Location = New System.Drawing.Point(11, 126)
        Me.LabelControl3.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.LabelControl3.Name = "LabelControl3"
        Me.LabelControl3.Size = New System.Drawing.Size(86, 13)
        Me.LabelControl3.TabIndex = 40
        Me.LabelControl3.Text = "Confirm Password"
        '
        'uxp2
        '
        Me.uxp2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxp2.EditValue = ""
        Me.uxp2.Location = New System.Drawing.Point(11, 143)
        Me.uxp2.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.uxp2.Name = "uxp2"
        Me.uxp2.Properties.DisplayFormat.FormatString = "****"
        Me.uxp2.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom
        Me.uxp2.Properties.MaxLength = 100
        Me.uxp2.Properties.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.uxp2.Size = New System.Drawing.Size(404, 20)
        Me.uxp2.TabIndex = 39
        '
        'LabelControl5
        '
        Me.LabelControl5.Location = New System.Drawing.Point(11, 44)
        Me.LabelControl5.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.LabelControl5.Name = "LabelControl5"
        Me.LabelControl5.Size = New System.Drawing.Size(66, 13)
        Me.LabelControl5.TabIndex = 37
        Me.LabelControl5.Text = "Email Address"
        '
        'uxchemail
        '
        Me.uxchemail.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxchemail.Location = New System.Drawing.Point(11, 61)
        Me.uxchemail.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.uxchemail.Name = "uxchemail"
        Me.uxchemail.Properties.MaxLength = 100
        Me.uxchemail.Size = New System.Drawing.Size(404, 20)
        Me.uxchemail.TabIndex = 4
        '
        'LabelControl6
        '
        Me.LabelControl6.Location = New System.Drawing.Point(11, 85)
        Me.LabelControl6.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.LabelControl6.Name = "LabelControl6"
        Me.LabelControl6.Size = New System.Drawing.Size(46, 13)
        Me.LabelControl6.TabIndex = 38
        Me.LabelControl6.Text = "Password"
        '
        'uxp1
        '
        Me.uxp1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxp1.EditValue = ""
        Me.uxp1.Location = New System.Drawing.Point(11, 102)
        Me.uxp1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.uxp1.Name = "uxp1"
        Me.uxp1.Properties.DisplayFormat.FormatString = "****"
        Me.uxp1.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom
        Me.uxp1.Properties.MaxLength = 100
        Me.uxp1.Properties.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.uxp1.Size = New System.Drawing.Size(404, 20)
        Me.uxp1.TabIndex = 5
        '
        'bc_dx_logon
        '
        Me.AcceptButton = Me.bok
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.bcancel
        Me.ClientSize = New System.Drawing.Size(425, 255)
        Me.Controls.Add(Me.XtraTabControl1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "bc_dx_logon"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Blue Curve - Logon"
        CType(Me.uxusername.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.XtraTabControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.XtraTabControl1.ResumeLayout(False)
        Me.XtraTabPage1.ResumeLayout(False)
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl1.ResumeLayout(False)
        Me.PanelControl1.PerformLayout()
        CType(Me.uxpassword.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.XtraTabPage2.ResumeLayout(False)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PanelControl2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl2.ResumeLayout(False)
        Me.PanelControl2.PerformLayout()
        CType(Me.uxpasswordresetemail.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.XtraTabPage3.ResumeLayout(False)
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PanelControl3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl3.ResumeLayout(False)
        Me.PanelControl3.PerformLayout()
        CType(Me.uxcode.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxp2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxchemail.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxp1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents bcancel As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents bok As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents uxusername As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LabelControl4 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents XtraTabControl1 As DevExpress.XtraTab.XtraTabControl
    Friend WithEvents XtraTabPage1 As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents uxpassword As DevExpress.XtraEditors.TextEdit
    Friend WithEvents XtraTabPage2 As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents PanelControl2 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents LabelControl2 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents uxpasswordresetemail As DevExpress.XtraEditors.TextEdit
    Friend WithEvents XtraTabPage3 As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents PictureBox3 As System.Windows.Forms.PictureBox
    Friend WithEvents PanelControl3 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents uxcode As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LabelControl7 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl3 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents uxp2 As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LabelControl5 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents uxchemail As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LabelControl6 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents uxp1 As DevExpress.XtraEditors.TextEdit
    Friend WithEvents SimpleButton1 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SimpleButton2 As DevExpress.XtraEditors.SimpleButton
End Class
