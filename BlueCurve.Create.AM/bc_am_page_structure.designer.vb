<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bc_am_page_structure
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_page_structure))
        Me.ppage = New System.Windows.Forms.Panel()
        Me.parea = New System.Windows.Forms.Panel()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.lstructures = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.ldesc = New DevExpress.XtraEditors.LabelControl()
        Me.lpos = New DevExpress.XtraEditors.LabelControl()
        Me.runits = New DevExpress.XtraEditors.RadioGroup()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl()
        Me.binsert = New DevExpress.XtraEditors.SimpleButton()
        Me.bcancel = New DevExpress.XtraEditors.SimpleButton()
        Me.PanelControl2 = New DevExpress.XtraEditors.PanelControl()
        CType(Me.lstructures.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.runits.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl1.SuspendLayout()
        CType(Me.PanelControl2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl2.SuspendLayout()
        Me.SuspendLayout()
        '
        'ppage
        '
        Me.ppage.BackColor = System.Drawing.Color.Silver
        Me.ppage.Location = New System.Drawing.Point(24, 75)
        Me.ppage.Name = "ppage"
        Me.ppage.Size = New System.Drawing.Size(420, 332)
        Me.ppage.TabIndex = 1
        '
        'parea
        '
        Me.parea.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.parea.Location = New System.Drawing.Point(24, 193)
        Me.parea.Name = "parea"
        Me.parea.Size = New System.Drawing.Size(420, 327)
        Me.parea.TabIndex = 2
        '
        'Button1
        '
        Me.Button1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button1.Location = New System.Drawing.Point(267, 552)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(65, 16)
        Me.Button1.TabIndex = 6
        Me.Button1.Text = "Load Structures "
        Me.Button1.UseVisualStyleBackColor = True
        Me.Button1.Visible = False
        '
        'lstructures
        '
        Me.lstructures.AllowHtmlTextInToolTip = DevExpress.Utils.DefaultBoolean.[True]
        Me.lstructures.Location = New System.Drawing.Point(24, 13)
        Me.lstructures.Name = "lstructures"
        Me.lstructures.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.lstructures.Size = New System.Drawing.Size(428, 20)
        Me.lstructures.TabIndex = 8
        '
        'ldesc
        '
        Me.ldesc.Location = New System.Drawing.Point(24, 39)
        Me.ldesc.Name = "ldesc"
        Me.ldesc.Size = New System.Drawing.Size(66, 13)
        Me.ldesc.TabIndex = 9
        Me.ldesc.Text = "LabelControl1"
        '
        'lpos
        '
        Me.lpos.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lpos.Location = New System.Drawing.Point(118, 532)
        Me.lpos.Name = "lpos"
        Me.lpos.Size = New System.Drawing.Size(0, 13)
        Me.lpos.TabIndex = 11
        '
        'runits
        '
        Me.runits.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.runits.Location = New System.Drawing.Point(24, 526)
        Me.runits.Name = "runits"
        Me.runits.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.RadioGroupItem() {New DevExpress.XtraEditors.Controls.RadioGroupItem(Nothing, "cm"), New DevExpress.XtraEditors.Controls.RadioGroupItem(Nothing, "pts")})
        Me.runits.Size = New System.Drawing.Size(88, 25)
        Me.runits.TabIndex = 0
        '
        'PictureBox1
        '
        Me.PictureBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(5, 14)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(175, 35)
        Me.PictureBox1.TabIndex = 40
        Me.PictureBox1.TabStop = False
        '
        'PanelControl1
        '
        Me.PanelControl1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PanelControl1.Controls.Add(Me.binsert)
        Me.PanelControl1.Controls.Add(Me.bcancel)
        Me.PanelControl1.Controls.Add(Me.PictureBox1)
        Me.PanelControl1.Location = New System.Drawing.Point(0, 557)
        Me.PanelControl1.Name = "PanelControl1"
        Me.PanelControl1.Size = New System.Drawing.Size(478, 60)
        Me.PanelControl1.TabIndex = 0
        '
        'binsert
        '
        Me.binsert.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.binsert.ImageIndex = 1
        Me.binsert.Location = New System.Drawing.Point(299, 17)
        Me.binsert.Name = "binsert"
        Me.binsert.Size = New System.Drawing.Size(74, 32)
        Me.binsert.TabIndex = 42
        Me.binsert.Text = "Insert"
        '
        'bcancel
        '
        Me.bcancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bcancel.ImageIndex = 1
        Me.bcancel.Location = New System.Drawing.Point(389, 17)
        Me.bcancel.Name = "bcancel"
        Me.bcancel.Size = New System.Drawing.Size(74, 32)
        Me.bcancel.TabIndex = 41
        Me.bcancel.Text = "Cancel"
        '
        'PanelControl2
        '
        Me.PanelControl2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PanelControl2.Controls.Add(Me.ldesc)
        Me.PanelControl2.Controls.Add(Me.lstructures)
        Me.PanelControl2.Location = New System.Drawing.Point(0, 0)
        Me.PanelControl2.Name = "PanelControl2"
        Me.PanelControl2.Size = New System.Drawing.Size(478, 57)
        Me.PanelControl2.TabIndex = 12
        '
        'bc_am_page_structure
        '
        Me.Appearance.BackColor = System.Drawing.Color.White
        Me.Appearance.Options.UseBackColor = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(475, 615)
        Me.Controls.Add(Me.PanelControl2)
        Me.Controls.Add(Me.PanelControl1)
        Me.Controls.Add(Me.runits)
        Me.Controls.Add(Me.lpos)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.parea)
        Me.Controls.Add(Me.ppage)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "bc_am_page_structure"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Blue Curve - Structure Wizard"
        CType(Me.lstructures.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.runits.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl1.ResumeLayout(False)
        CType(Me.PanelControl2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl2.ResumeLayout(False)
        Me.PanelControl2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ppage As System.Windows.Forms.Panel
    Friend WithEvents parea As System.Windows.Forms.Panel
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents lstructures As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents ldesc As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lpos As DevExpress.XtraEditors.LabelControl
    Friend WithEvents runits As DevExpress.XtraEditors.RadioGroup
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents PanelControl2 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents binsert As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents bcancel As DevExpress.XtraEditors.SimpleButton
End Class
