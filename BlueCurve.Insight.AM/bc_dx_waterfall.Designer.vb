<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bc_dx_waterfall
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_dx_waterfall))
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.bsave = New DevExpress.XtraEditors.SimpleButton()
        Me.bcancel = New DevExpress.XtraEditors.SimpleButton()
        Me.uxres = New DevExpress.XtraTreeList.TreeList()
        Me.TreeListColumn21 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.TreeListColumn22 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.prules = New DevExpress.XtraEditors.PanelControl()
        Me.baddrule = New DevExpress.XtraEditors.SimpleButton()
        Me.uxrules = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.lrules = New DevExpress.XtraEditors.LabelControl()
        Me.labelControl1 = New DevExpress.XtraEditors.LabelControl()
        Me.uxc = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.badd = New DevExpress.XtraEditors.SimpleButton()
        Me.SimpleButton1 = New DevExpress.XtraEditors.SimpleButton()
        Me.bdelrule = New DevExpress.XtraEditors.SimpleButton()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxres, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.prules, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.prules.SuspendLayout()
        CType(Me.uxrules.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxc.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PictureBox2
        '
        Me.PictureBox2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(12, 395)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(188, 34)
        Me.PictureBox2.TabIndex = 94
        Me.PictureBox2.TabStop = False
        '
        'bsave
        '
        Me.bsave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bsave.Enabled = False
        Me.bsave.ImageIndex = 2
        Me.bsave.Location = New System.Drawing.Point(511, 397)
        Me.bsave.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.bsave.Name = "bsave"
        Me.bsave.Size = New System.Drawing.Size(75, 33)
        Me.bsave.TabIndex = 93
        Me.bsave.Text = "Save"
        '
        'bcancel
        '
        Me.bcancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bcancel.ImageIndex = 4
        Me.bcancel.Location = New System.Drawing.Point(607, 397)
        Me.bcancel.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.bcancel.Name = "bcancel"
        Me.bcancel.Size = New System.Drawing.Size(75, 33)
        Me.bcancel.TabIndex = 92
        Me.bcancel.Text = "Cancel"
        '
        'uxres
        '
        Me.uxres.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxres.Columns.AddRange(New DevExpress.XtraTreeList.Columns.TreeListColumn() {Me.TreeListColumn21, Me.TreeListColumn22})
        Me.uxres.Location = New System.Drawing.Point(12, 49)
        Me.uxres.Name = "uxres"
        Me.uxres.OptionsSelection.MultiSelect = True
        Me.uxres.Size = New System.Drawing.Size(661, 226)
        Me.uxres.TabIndex = 95
        '
        'TreeListColumn21
        '
        Me.TreeListColumn21.Caption = "Order"
        Me.TreeListColumn21.FieldName = "TreeListColumn21"
        Me.TreeListColumn21.Name = "TreeListColumn21"
        Me.TreeListColumn21.OptionsColumn.AllowEdit = False
        Me.TreeListColumn21.OptionsColumn.ReadOnly = True
        Me.TreeListColumn21.Visible = True
        Me.TreeListColumn21.VisibleIndex = 0
        Me.TreeListColumn21.Width = 153
        '
        'TreeListColumn22
        '
        Me.TreeListColumn22.Caption = "Contributor"
        Me.TreeListColumn22.FieldName = "TreeListColumn22"
        Me.TreeListColumn22.Name = "TreeListColumn22"
        Me.TreeListColumn22.OptionsColumn.AllowEdit = False
        Me.TreeListColumn22.OptionsColumn.ReadOnly = True
        Me.TreeListColumn22.Visible = True
        Me.TreeListColumn22.VisibleIndex = 1
        Me.TreeListColumn22.Width = 490
        '
        'prules
        '
        Me.prules.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.prules.Controls.Add(Me.baddrule)
        Me.prules.Controls.Add(Me.uxrules)
        Me.prules.Controls.Add(Me.lrules)
        Me.prules.Enabled = False
        Me.prules.Location = New System.Drawing.Point(13, 302)
        Me.prules.Name = "prules"
        Me.prules.Size = New System.Drawing.Size(660, 67)
        Me.prules.TabIndex = 96
        '
        'baddrule
        '
        Me.baddrule.AccessibleDescription = ""
        Me.baddrule.Enabled = False
        Me.baddrule.ImageIndex = 4
        Me.baddrule.Location = New System.Drawing.Point(331, 17)
        Me.baddrule.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.baddrule.Name = "baddrule"
        Me.baddrule.Size = New System.Drawing.Size(75, 33)
        Me.baddrule.TabIndex = 102
        Me.baddrule.Text = "Add"
        '
        'uxrules
        '
        Me.uxrules.Location = New System.Drawing.Point(5, 24)
        Me.uxrules.Name = "uxrules"
        Me.uxrules.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.uxrules.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        Me.uxrules.Size = New System.Drawing.Size(309, 20)
        Me.uxrules.TabIndex = 101
        '
        'lrules
        '
        Me.lrules.Location = New System.Drawing.Point(5, 5)
        Me.lrules.Name = "lrules"
        Me.lrules.Size = New System.Drawing.Size(26, 13)
        Me.lrules.TabIndex = 100
        Me.lrules.Text = "Rules"
        '
        'labelControl1
        '
        Me.labelControl1.Location = New System.Drawing.Point(12, 21)
        Me.labelControl1.Name = "labelControl1"
        Me.labelControl1.Size = New System.Drawing.Size(55, 13)
        Me.labelControl1.TabIndex = 97
        Me.labelControl1.Text = "Contributor"
        '
        'uxc
        '
        Me.uxc.Location = New System.Drawing.Point(84, 18)
        Me.uxc.Name = "uxc"
        Me.uxc.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.uxc.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        Me.uxc.Size = New System.Drawing.Size(242, 20)
        Me.uxc.TabIndex = 96
        '
        'badd
        '
        Me.badd.AccessibleDescription = ""
        Me.badd.Enabled = False
        Me.badd.ImageIndex = 4
        Me.badd.Location = New System.Drawing.Point(343, 11)
        Me.badd.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.badd.Name = "badd"
        Me.badd.Size = New System.Drawing.Size(75, 33)
        Me.badd.TabIndex = 93
        Me.badd.Text = "Add"
        '
        'SimpleButton1
        '
        Me.SimpleButton1.AccessibleDescription = ""
        Me.SimpleButton1.ImageIndex = 4
        Me.SimpleButton1.Location = New System.Drawing.Point(433, 11)
        Me.SimpleButton1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.SimpleButton1.Name = "SimpleButton1"
        Me.SimpleButton1.Size = New System.Drawing.Size(75, 33)
        Me.SimpleButton1.TabIndex = 98
        Me.SimpleButton1.Text = "Remove"
        '
        'bdelrule
        '
        Me.bdelrule.AccessibleDescription = ""
        Me.bdelrule.Enabled = False
        Me.bdelrule.ImageIndex = 4
        Me.bdelrule.Location = New System.Drawing.Point(523, 11)
        Me.bdelrule.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.bdelrule.Name = "bdelrule"
        Me.bdelrule.Size = New System.Drawing.Size(75, 33)
        Me.bdelrule.TabIndex = 99
        Me.bdelrule.Text = "Delete Rule"
        '
        'bc_dx_waterfall
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(694, 441)
        Me.Controls.Add(Me.bdelrule)
        Me.Controls.Add(Me.SimpleButton1)
        Me.Controls.Add(Me.prules)
        Me.Controls.Add(Me.uxres)
        Me.Controls.Add(Me.uxc)
        Me.Controls.Add(Me.labelControl1)
        Me.Controls.Add(Me.badd)
        Me.Controls.Add(Me.PictureBox2)
        Me.Controls.Add(Me.bsave)
        Me.Controls.Add(Me.bcancel)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimizeBox = False
        Me.Name = "bc_dx_waterfall"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Contributor Waterfall Attribute Exclusions"
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxres, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.prules, System.ComponentModel.ISupportInitialize).EndInit()
        Me.prules.ResumeLayout(False)
        Me.prules.PerformLayout()
        CType(Me.uxrules.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxc.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents bsave As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents bcancel As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents uxres As DevExpress.XtraTreeList.TreeList
    Friend WithEvents TreeListColumn21 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents TreeListColumn22 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents prules As DevExpress.XtraEditors.PanelControl
    Friend WithEvents badd As DevExpress.XtraEditors.SimpleButton
    Private WithEvents labelControl1 As DevExpress.XtraEditors.LabelControl
    Private WithEvents uxc As DevExpress.XtraEditors.ComboBoxEdit
    Private WithEvents lrules As DevExpress.XtraEditors.LabelControl
    Friend WithEvents SimpleButton1 As DevExpress.XtraEditors.SimpleButton
    Private WithEvents uxrules As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents baddrule As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents bdelrule As DevExpress.XtraEditors.SimpleButton
End Class
