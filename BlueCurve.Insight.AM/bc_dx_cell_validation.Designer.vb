<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bc_dx_cell_validation
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_dx_cell_validation))
        Me.bclose = New DevExpress.XtraEditors.SimpleButton()
        Me.bgoto = New DevExpress.XtraEditors.SimpleButton()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.uxres = New DevExpress.XtraTreeList.TreeList()
        Me.TreeListColumn1 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.TreeListColumn2 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.TreeListColumn3 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.TreeListColumn4 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.TreeListColumn5 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.TreeListColumn6 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.tabimages = New System.Windows.Forms.ImageList(Me.components)
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxres, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'bclose
        '
        Me.bclose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bclose.ImageIndex = 2
        Me.bclose.Location = New System.Drawing.Point(496, 199)
        Me.bclose.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.bclose.Name = "bclose"
        Me.bclose.Size = New System.Drawing.Size(75, 33)
        Me.bclose.TabIndex = 96
        Me.bclose.Text = "Close"
        '
        'bgoto
        '
        Me.bgoto.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bgoto.Enabled = False
        Me.bgoto.ImageIndex = 4
        Me.bgoto.Location = New System.Drawing.Point(415, 197)
        Me.bgoto.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.bgoto.Name = "bgoto"
        Me.bgoto.Size = New System.Drawing.Size(75, 33)
        Me.bgoto.TabIndex = 95
        Me.bgoto.Text = "Goto"
        '
        'PictureBox2
        '
        Me.PictureBox2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(2, 197)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(188, 34)
        Me.PictureBox2.TabIndex = 97
        Me.PictureBox2.TabStop = False
        '
        'uxres
        '
        Me.uxres.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxres.Columns.AddRange(New DevExpress.XtraTreeList.Columns.TreeListColumn() {Me.TreeListColumn1, Me.TreeListColumn2, Me.TreeListColumn3, Me.TreeListColumn4, Me.TreeListColumn5, Me.TreeListColumn6})
        Me.uxres.Location = New System.Drawing.Point(12, 12)
        Me.uxres.Name = "uxres"
        Me.uxres.OptionsSelection.MultiSelect = True
        Me.uxres.Size = New System.Drawing.Size(559, 167)
        Me.uxres.StateImageList = Me.tabimages
        Me.uxres.TabIndex = 98
        '
        'TreeListColumn1
        '
        Me.TreeListColumn1.Caption = "Sheet"
        Me.TreeListColumn1.FieldName = "Sheet"
        Me.TreeListColumn1.MinWidth = 33
        Me.TreeListColumn1.Name = "TreeListColumn1"
        Me.TreeListColumn1.OptionsColumn.AllowEdit = False
        Me.TreeListColumn1.Visible = True
        Me.TreeListColumn1.VisibleIndex = 0
        Me.TreeListColumn1.Width = 94
        '
        'TreeListColumn2
        '
        Me.TreeListColumn2.Caption = "Type"
        Me.TreeListColumn2.FieldName = "Type"
        Me.TreeListColumn2.Name = "TreeListColumn2"
        Me.TreeListColumn2.OptionsColumn.AllowEdit = False
        Me.TreeListColumn2.Visible = True
        Me.TreeListColumn2.VisibleIndex = 1
        Me.TreeListColumn2.Width = 59
        '
        'TreeListColumn3
        '
        Me.TreeListColumn3.Caption = "Warning"
        Me.TreeListColumn3.FieldName = "Warning"
        Me.TreeListColumn3.Name = "TreeListColumn3"
        Me.TreeListColumn3.OptionsColumn.AllowEdit = False
        Me.TreeListColumn3.Visible = True
        Me.TreeListColumn3.VisibleIndex = 2
        Me.TreeListColumn3.Width = 162
        '
        'TreeListColumn4
        '
        Me.TreeListColumn4.Caption = "Cell"
        Me.TreeListColumn4.FieldName = "Cell"
        Me.TreeListColumn4.Name = "TreeListColumn4"
        Me.TreeListColumn4.OptionsColumn.AllowFocus = False
        Me.TreeListColumn4.Visible = True
        Me.TreeListColumn4.VisibleIndex = 3
        Me.TreeListColumn4.Width = 160
        '
        'TreeListColumn5
        '
        Me.TreeListColumn5.Caption = "row"
        Me.TreeListColumn5.FieldName = "row"
        Me.TreeListColumn5.Name = "TreeListColumn5"
        Me.TreeListColumn5.OptionsColumn.AllowEdit = False
        '
        'TreeListColumn6
        '
        Me.TreeListColumn6.Caption = "col"
        Me.TreeListColumn6.FieldName = "col"
        Me.TreeListColumn6.Name = "TreeListColumn6"
        Me.TreeListColumn6.OptionsColumn.AllowEdit = False
        '
        'tabimages
        '
        Me.tabimages.ImageStream = CType(resources.GetObject("tabimages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.tabimages.TransparentColor = System.Drawing.Color.Transparent
        Me.tabimages.Images.SetKeyName(0, "warning.ico")
        '
        'bc_dx_cell_validation
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(574, 243)
        Me.Controls.Add(Me.uxres)
        Me.Controls.Add(Me.PictureBox2)
        Me.Controls.Add(Me.bclose)
        Me.Controls.Add(Me.bgoto)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "bc_dx_cell_validation"
        Me.Text = "Insight Submission Errors"
        Me.TopMost = True
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxres, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents bclose As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents bgoto As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents uxres As DevExpress.XtraTreeList.TreeList
    Friend WithEvents TreeListColumn1 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents TreeListColumn2 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents TreeListColumn3 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents TreeListColumn4 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents tabimages As System.Windows.Forms.ImageList
    Friend WithEvents TreeListColumn5 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents TreeListColumn6 As DevExpress.XtraTreeList.Columns.TreeListColumn
End Class
