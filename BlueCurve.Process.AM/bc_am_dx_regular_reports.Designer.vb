<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bc_am_dx_regular_reports
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_dx_regular_reports))
        Me.bcancel = New DevExpress.XtraEditors.SimpleButton()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.bcontinue = New DevExpress.XtraEditors.SimpleButton()
        Me.uxreports = New DevExpress.XtraTreeList.TreeList()
        Me.TreeListColumn1 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.TreeListColumn2 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.TreeListColumn3 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.RepositoryItemDateEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemDateEdit()
        Me.RepositoryItemDateEdit3 = New DevExpress.XtraEditors.Repository.RepositoryItemDateEdit()
        Me.bdelete = New DevExpress.XtraEditors.SimpleButton()
        Me.rtype = New DevExpress.XtraEditors.RadioGroup()
        Me.bedit = New DevExpress.XtraEditors.SimpleButton()
        Me.bnew = New DevExpress.XtraEditors.SimpleButton()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxreports, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemDateEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemDateEdit1.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemDateEdit3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemDateEdit3.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rtype.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'bcancel
        '
        Me.bcancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bcancel.ImageIndex = 2
        Me.bcancel.Location = New System.Drawing.Point(771, 412)
        Me.bcancel.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.bcancel.Name = "bcancel"
        Me.bcancel.Size = New System.Drawing.Size(75, 33)
        Me.bcancel.TabIndex = 73
        Me.bcancel.Text = "Cancel"
        '
        'PictureBox2
        '
        Me.PictureBox2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(12, 410)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(188, 34)
        Me.PictureBox2.TabIndex = 75
        Me.PictureBox2.TabStop = False
        '
        'bcontinue
        '
        Me.bcontinue.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bcontinue.ImageIndex = 2
        Me.bcontinue.Location = New System.Drawing.Point(680, 412)
        Me.bcontinue.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.bcontinue.Name = "bcontinue"
        Me.bcontinue.Size = New System.Drawing.Size(75, 33)
        Me.bcontinue.TabIndex = 76
        Me.bcontinue.Text = "Import"
        Me.bcontinue.ToolTip = "Import a regular report"
        '
        'uxreports
        '
        Me.uxreports.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxreports.Columns.AddRange(New DevExpress.XtraTreeList.Columns.TreeListColumn() {Me.TreeListColumn1, Me.TreeListColumn2, Me.TreeListColumn3})
        Me.uxreports.Location = New System.Drawing.Point(12, 25)
        Me.uxreports.Name = "uxreports"
        Me.uxreports.OptionsBehavior.Editable = False
        Me.uxreports.OptionsBehavior.EnableFiltering = True
        Me.uxreports.OptionsView.ShowRoot = False
        Me.uxreports.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemDateEdit1, Me.RepositoryItemDateEdit3})
        Me.uxreports.Size = New System.Drawing.Size(834, 379)
        Me.uxreports.TabIndex = 77
        Me.uxreports.TreeLevelWidth = 12
        '
        'TreeListColumn1
        '
        Me.TreeListColumn1.Caption = "Report Name"
        Me.TreeListColumn1.FieldName = "Report Name"
        Me.TreeListColumn1.MinWidth = 33
        Me.TreeListColumn1.Name = "TreeListColumn1"
        Me.TreeListColumn1.Visible = True
        Me.TreeListColumn1.VisibleIndex = 0
        Me.TreeListColumn1.Width = 92
        '
        'TreeListColumn2
        '
        Me.TreeListColumn2.Caption = "Publication Type"
        Me.TreeListColumn2.FieldName = "Status"
        Me.TreeListColumn2.Name = "TreeListColumn2"
        Me.TreeListColumn2.Visible = True
        Me.TreeListColumn2.VisibleIndex = 1
        '
        'TreeListColumn3
        '
        Me.TreeListColumn3.Caption = "Created By"
        Me.TreeListColumn3.FieldName = "Created By"
        Me.TreeListColumn3.Name = "TreeListColumn3"
        Me.TreeListColumn3.Visible = True
        Me.TreeListColumn3.VisibleIndex = 2
        '
        'RepositoryItemDateEdit1
        '
        Me.RepositoryItemDateEdit1.AutoHeight = False
        Me.RepositoryItemDateEdit1.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.RepositoryItemDateEdit1.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.RepositoryItemDateEdit1.Name = "RepositoryItemDateEdit1"
        '
        'RepositoryItemDateEdit3
        '
        Me.RepositoryItemDateEdit3.AutoHeight = False
        Me.RepositoryItemDateEdit3.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.RepositoryItemDateEdit3.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.RepositoryItemDateEdit3.Name = "RepositoryItemDateEdit3"
        '
        'bdelete
        '
        Me.bdelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.bdelete.ImageIndex = 2
        Me.bdelete.Location = New System.Drawing.Point(379, 412)
        Me.bdelete.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.bdelete.Name = "bdelete"
        Me.bdelete.Size = New System.Drawing.Size(75, 33)
        Me.bdelete.TabIndex = 78
        Me.bdelete.Text = "Delete"
        Me.bdelete.ToolTip = "Delete a regular report template."
        '
        'rtype
        '
        Me.rtype.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rtype.Location = New System.Drawing.Point(497, 417)
        Me.rtype.Name = "rtype"
        Me.rtype.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.rtype.Properties.Appearance.Options.UseBackColor = True
        Me.rtype.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.RadioGroupItem() {New DevExpress.XtraEditors.Controls.RadioGroupItem(Nothing, "Global"), New DevExpress.XtraEditors.Controls.RadioGroupItem(Nothing, "Mine")})
        Me.rtype.Size = New System.Drawing.Size(164, 24)
        Me.rtype.TabIndex = 79
        '
        'bedit
        '
        Me.bedit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.bedit.Enabled = False
        Me.bedit.ImageIndex = 2
        Me.bedit.Location = New System.Drawing.Point(298, 412)
        Me.bedit.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.bedit.Name = "bedit"
        Me.bedit.Size = New System.Drawing.Size(75, 33)
        Me.bedit.TabIndex = 80
        Me.bedit.Text = "Update"
        Me.bedit.ToolTip = "Update a regular report template."
        '
        'bnew
        '
        Me.bnew.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.bnew.ImageIndex = 2
        Me.bnew.Location = New System.Drawing.Point(217, 412)
        Me.bnew.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.bnew.Name = "bnew"
        Me.bnew.Size = New System.Drawing.Size(75, 33)
        Me.bnew.TabIndex = 81
        Me.bnew.Text = "New"
        Me.bnew.ToolTip = "Create a regular report template."
        '
        'bc_am_dx_regular_reports
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(858, 456)
        Me.Controls.Add(Me.bnew)
        Me.Controls.Add(Me.bedit)
        Me.Controls.Add(Me.rtype)
        Me.Controls.Add(Me.bdelete)
        Me.Controls.Add(Me.uxreports)
        Me.Controls.Add(Me.bcontinue)
        Me.Controls.Add(Me.PictureBox2)
        Me.Controls.Add(Me.bcancel)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimizeBox = False
        Me.Name = "bc_am_dx_regular_reports"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Import Regular Report"
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxreports, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemDateEdit1.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemDateEdit1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemDateEdit3.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemDateEdit3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rtype.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents bcancel As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents bcontinue As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents uxreports As DevExpress.XtraTreeList.TreeList
    Friend WithEvents TreeListColumn1 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents TreeListColumn2 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents RepositoryItemDateEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemDateEdit
    Friend WithEvents RepositoryItemDateEdit3 As DevExpress.XtraEditors.Repository.RepositoryItemDateEdit
    Friend WithEvents bdelete As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents TreeListColumn3 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents rtype As DevExpress.XtraEditors.RadioGroup
    Friend WithEvents bedit As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents bnew As DevExpress.XtraEditors.SimpleButton
End Class
