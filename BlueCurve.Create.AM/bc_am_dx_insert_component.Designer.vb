<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bc_am_dx_insert_component
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_dx_insert_component))
        Me.bcancel = New DevExpress.XtraEditors.SimpleButton()
        Me.uxImages = New System.Windows.Forms.ImageList()
        Me.uxinsert = New DevExpress.XtraEditors.SimpleButton()
        Me.uxcomp = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.LabelControl8 = New DevExpress.XtraEditors.LabelControl()
        Me.LabelControl1 = New DevExpress.XtraEditors.LabelControl()
        Me.lclass = New DevExpress.XtraEditors.LabelControl()
        Me.uxclass = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.tsearch = New DevExpress.XtraEditors.TextEdit()
        Me.psearch = New DevExpress.XtraEditors.PictureEdit()
        Me.lsearchtimer = New System.Windows.Forms.Timer()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.RadioGroup1 = New DevExpress.XtraEditors.RadioGroup()
        Me.ListBoxControl1 = New DevExpress.XtraEditors.ListBoxControl()
        CType(Me.uxcomp.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxclass.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.tsearch.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.psearch.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadioGroup1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ListBoxControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'bcancel
        '
        Me.bcancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bcancel.ImageIndex = 2
        Me.bcancel.ImageList = Me.uxImages
        Me.bcancel.Location = New System.Drawing.Point(403, 304)
        Me.bcancel.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.bcancel.Name = "bcancel"
        Me.bcancel.Size = New System.Drawing.Size(75, 33)
        Me.bcancel.TabIndex = 3
        Me.bcancel.Text = "Cancel"
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
        'uxinsert
        '
        Me.uxinsert.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxinsert.Enabled = False
        Me.uxinsert.ImageIndex = 1
        Me.uxinsert.ImageList = Me.uxImages
        Me.uxinsert.Location = New System.Drawing.Point(491, 304)
        Me.uxinsert.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.uxinsert.Name = "uxinsert"
        Me.uxinsert.Size = New System.Drawing.Size(75, 33)
        Me.uxinsert.TabIndex = 2
        Me.uxinsert.Text = "Insert"
        '
        'uxcomp
        '
        Me.uxcomp.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxcomp.EditValue = ""
        Me.uxcomp.Location = New System.Drawing.Point(123, 60)
        Me.uxcomp.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.uxcomp.Name = "uxcomp"
        Me.uxcomp.Properties.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.[True]
        Me.uxcomp.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.uxcomp.Properties.MaxLength = 250
        Me.uxcomp.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        Me.uxcomp.Size = New System.Drawing.Size(423, 20)
        Me.uxcomp.TabIndex = 58
        '
        'LabelControl8
        '
        Me.LabelControl8.Location = New System.Drawing.Point(30, 63)
        Me.LabelControl8.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.LabelControl8.Name = "LabelControl8"
        Me.LabelControl8.Size = New System.Drawing.Size(55, 13)
        Me.LabelControl8.TabIndex = 57
        Me.LabelControl8.Text = "Component"
        '
        'LabelControl1
        '
        Me.LabelControl1.Location = New System.Drawing.Point(31, 24)
        Me.LabelControl1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.LabelControl1.Name = "LabelControl1"
        Me.LabelControl1.Size = New System.Drawing.Size(25, 13)
        Me.LabelControl1.TabIndex = 59
        Me.LabelControl1.Text = "Class"
        '
        'lclass
        '
        Me.lclass.Location = New System.Drawing.Point(31, 100)
        Me.lclass.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.lclass.Name = "lclass"
        Me.lclass.Size = New System.Drawing.Size(28, 13)
        Me.lclass.TabIndex = 60
        Me.lclass.Text = "Entity"
        '
        'uxclass
        '
        Me.uxclass.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxclass.EditValue = ""
        Me.uxclass.Location = New System.Drawing.Point(124, 21)
        Me.uxclass.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.uxclass.Name = "uxclass"
        Me.uxclass.Properties.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.[True]
        Me.uxclass.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.uxclass.Properties.MaxLength = 250
        Me.uxclass.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        Me.uxclass.Size = New System.Drawing.Size(423, 20)
        Me.uxclass.TabIndex = 61
        '
        'tsearch
        '
        Me.tsearch.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tsearch.EditValue = ""
        Me.tsearch.Location = New System.Drawing.Point(123, 262)
        Me.tsearch.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.tsearch.Name = "tsearch"
        Me.tsearch.Properties.MaxLength = 250
        Me.tsearch.Size = New System.Drawing.Size(424, 20)
        Me.tsearch.TabIndex = 63
        '
        'psearch
        '
        Me.psearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.psearch.EditValue = Global.BlueCurve.Create.AM.My.Resources.Resources.search2
        Me.psearch.Location = New System.Drawing.Point(81, 256)
        Me.psearch.Name = "psearch"
        Me.psearch.Size = New System.Drawing.Size(27, 26)
        Me.psearch.TabIndex = 64
        '
        'lsearchtimer
        '
        Me.lsearchtimer.Enabled = True
        Me.lsearchtimer.Interval = 1000
        '
        'PictureBox2
        '
        Me.PictureBox2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(12, 304)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(161, 33)
        Me.PictureBox2.TabIndex = 66
        Me.PictureBox2.TabStop = False
        '
        'RadioGroup1
        '
        Me.RadioGroup1.Location = New System.Drawing.Point(422, 233)
        Me.RadioGroup1.Name = "RadioGroup1"
        Me.RadioGroup1.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.RadioGroupItem() {New DevExpress.XtraEditors.Controls.RadioGroupItem(CType(0, Short), "All"), New DevExpress.XtraEditors.Controls.RadioGroupItem(CType(1, Short), "Mine")})
        Me.RadioGroup1.Size = New System.Drawing.Size(124, 24)
        Me.RadioGroup1.TabIndex = 67
        '
        'ListBoxControl1
        '
        Me.ListBoxControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListBoxControl1.Location = New System.Drawing.Point(123, 100)
        Me.ListBoxControl1.Name = "ListBoxControl1"
        Me.ListBoxControl1.Size = New System.Drawing.Size(423, 127)
        Me.ListBoxControl1.TabIndex = 70
        '
        'bc_am_dx_insert_component
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(589, 348)
        Me.Controls.Add(Me.ListBoxControl1)
        Me.Controls.Add(Me.RadioGroup1)
        Me.Controls.Add(Me.PictureBox2)
        Me.Controls.Add(Me.tsearch)
        Me.Controls.Add(Me.psearch)
        Me.Controls.Add(Me.uxclass)
        Me.Controls.Add(Me.lclass)
        Me.Controls.Add(Me.LabelControl1)
        Me.Controls.Add(Me.uxcomp)
        Me.Controls.Add(Me.LabelControl8)
        Me.Controls.Add(Me.bcancel)
        Me.Controls.Add(Me.uxinsert)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "bc_am_dx_insert_component"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Blue Curve - Insert Component"
        CType(Me.uxcomp.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxclass.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.tsearch.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.psearch.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadioGroup1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ListBoxControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout

End Sub
    Friend WithEvents bcancel As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents uxinsert As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents uxImages As System.Windows.Forms.ImageList
    Friend WithEvents uxcomp As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents LabelControl8 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lclass As DevExpress.XtraEditors.LabelControl
    Friend WithEvents uxclass As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents tsearch As DevExpress.XtraEditors.TextEdit
    Friend WithEvents psearch As DevExpress.XtraEditors.PictureEdit
    Friend WithEvents lsearchtimer As System.Windows.Forms.Timer
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents RadioGroup1 As DevExpress.XtraEditors.RadioGroup
    Friend WithEvents ListBoxControl1 As DevExpress.XtraEditors.ListBoxControl
End Class
