<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bc_dx_insight_build
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_dx_insight_build))
        Me.bcancel = New DevExpress.XtraEditors.SimpleButton()
        Me.bok = New DevExpress.XtraEditors.SimpleButton()
        Me.uxschema = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.labelControl1 = New DevExpress.XtraEditors.LabelControl()
        Me.uxclass = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.LabelControl2 = New DevExpress.XtraEditors.LabelControl()
        Me.lentity = New DevExpress.XtraEditors.ListBoxControl()
        Me.tsearch = New DevExpress.XtraEditors.TextEdit()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.rallmine = New DevExpress.XtraEditors.RadioGroup()
        Me.pclear = New DevExpress.XtraEditors.PictureEdit()
        Me.lsearch = New DevExpress.XtraEditors.PictureEdit()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        CType(Me.uxschema.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxclass.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lentity, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.tsearch.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rallmine.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pclear.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lsearch.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'bcancel
        '
        Me.bcancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bcancel.ImageIndex = 2
        Me.bcancel.Location = New System.Drawing.Point(423, 445)
        Me.bcancel.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.bcancel.Name = "bcancel"
        Me.bcancel.Size = New System.Drawing.Size(75, 33)
        Me.bcancel.TabIndex = 93
        Me.bcancel.Text = "Cancel"
        '
        'bok
        '
        Me.bok.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bok.Enabled = False
        Me.bok.ImageIndex = 4
        Me.bok.Location = New System.Drawing.Point(332, 444)
        Me.bok.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.bok.Name = "bok"
        Me.bok.Size = New System.Drawing.Size(75, 33)
        Me.bok.TabIndex = 92
        Me.bok.Text = "Build"
        '
        'uxschema
        '
        Me.uxschema.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxschema.Location = New System.Drawing.Point(100, 12)
        Me.uxschema.Name = "uxschema"
        Me.uxschema.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.uxschema.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        Me.uxschema.Size = New System.Drawing.Size(371, 20)
        Me.uxschema.TabIndex = 98
        '
        'labelControl1
        '
        Me.labelControl1.Location = New System.Drawing.Point(41, 15)
        Me.labelControl1.Name = "labelControl1"
        Me.labelControl1.Size = New System.Drawing.Size(37, 13)
        Me.labelControl1.TabIndex = 99
        Me.labelControl1.Text = "Schema"
        '
        'uxclass
        '
        Me.uxclass.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxclass.Location = New System.Drawing.Point(100, 53)
        Me.uxclass.Name = "uxclass"
        Me.uxclass.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.uxclass.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        Me.uxclass.Size = New System.Drawing.Size(371, 20)
        Me.uxclass.TabIndex = 100
        '
        'LabelControl2
        '
        Me.LabelControl2.Location = New System.Drawing.Point(41, 56)
        Me.LabelControl2.Name = "LabelControl2"
        Me.LabelControl2.Size = New System.Drawing.Size(28, 13)
        Me.LabelControl2.TabIndex = 101
        Me.LabelControl2.Text = "Entity"
        '
        'lentity
        '
        Me.lentity.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lentity.Location = New System.Drawing.Point(32, 99)
        Me.lentity.Name = "lentity"
        Me.lentity.Size = New System.Drawing.Size(439, 260)
        Me.lentity.TabIndex = 102
        '
        'tsearch
        '
        Me.tsearch.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tsearch.EditValue = ""
        Me.tsearch.Location = New System.Drawing.Point(64, 389)
        Me.tsearch.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.tsearch.Name = "tsearch"
        Me.tsearch.Properties.MaxLength = 250
        Me.tsearch.Size = New System.Drawing.Size(375, 20)
        Me.tsearch.TabIndex = 104
        '
        'Timer1
        '
        Me.Timer1.Interval = 1000
        '
        'rallmine
        '
        Me.rallmine.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.rallmine.Location = New System.Drawing.Point(32, 362)
        Me.rallmine.Name = "rallmine"
        Me.rallmine.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(236, Byte), Integer), CType(CType(239, Byte), Integer))
        Me.rallmine.Properties.Appearance.Options.UseBackColor = True
        Me.rallmine.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me.rallmine.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.RadioGroupItem() {New DevExpress.XtraEditors.Controls.RadioGroupItem(Nothing, "All"), New DevExpress.XtraEditors.Controls.RadioGroupItem(Nothing, "Mine")})
        Me.rallmine.Size = New System.Drawing.Size(119, 22)
        Me.rallmine.TabIndex = 106
        Me.rallmine.Visible = False
        '
        'pclear
        '
        Me.pclear.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pclear.EditValue = Global.BlueCurve.Insight.AM.My.Resources.Resources.Revert_disabled
        Me.pclear.Location = New System.Drawing.Point(445, 385)
        Me.pclear.Name = "pclear"
        Me.pclear.Size = New System.Drawing.Size(26, 24)
        Me.pclear.TabIndex = 105
        '
        'lsearch
        '
        Me.lsearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lsearch.EditValue = Global.BlueCurve.Insight.AM.My.Resources.Resources.ViewIcon_Disabled
        Me.lsearch.Location = New System.Drawing.Point(32, 385)
        Me.lsearch.Name = "lsearch"
        Me.lsearch.Size = New System.Drawing.Size(26, 24)
        Me.lsearch.TabIndex = 103
        '
        'PictureBox2
        '
        Me.PictureBox2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(3, 443)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(188, 34)
        Me.PictureBox2.TabIndex = 94
        Me.PictureBox2.TabStop = False
        '
        'bc_dx_insight_build
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(510, 489)
        Me.Controls.Add(Me.rallmine)
        Me.Controls.Add(Me.pclear)
        Me.Controls.Add(Me.lsearch)
        Me.Controls.Add(Me.tsearch)
        Me.Controls.Add(Me.lentity)
        Me.Controls.Add(Me.uxclass)
        Me.Controls.Add(Me.LabelControl2)
        Me.Controls.Add(Me.uxschema)
        Me.Controls.Add(Me.labelControl1)
        Me.Controls.Add(Me.PictureBox2)
        Me.Controls.Add(Me.bcancel)
        Me.Controls.Add(Me.bok)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimizeBox = False
        Me.Name = "bc_dx_insight_build"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Insight - Build"
        CType(Me.uxschema.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxclass.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lentity, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.tsearch.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rallmine.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pclear.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lsearch.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents bcancel As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents bok As DevExpress.XtraEditors.SimpleButton
    Private WithEvents uxschema As DevExpress.XtraEditors.ComboBoxEdit
    Private WithEvents labelControl1 As DevExpress.XtraEditors.LabelControl
    Private WithEvents uxclass As DevExpress.XtraEditors.ComboBoxEdit
    Private WithEvents LabelControl2 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lentity As DevExpress.XtraEditors.ListBoxControl
    Friend WithEvents pclear As DevExpress.XtraEditors.PictureEdit
    Friend WithEvents lsearch As DevExpress.XtraEditors.PictureEdit
    Friend WithEvents tsearch As DevExpress.XtraEditors.TextEdit
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents rallmine As DevExpress.XtraEditors.RadioGroup
End Class
