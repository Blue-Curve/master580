<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bc_am_in_calc_lib
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_in_calc_lib))
        Me.uxlib = New DevExpress.XtraTreeList.TreeList()
        Me.TreeListColumn4 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.TreeListColumn5 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.TreeListColumn6 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.bcancel = New DevExpress.XtraEditors.SimpleButton()
        Me.uxImages = New System.Windows.Forms.ImageList(Me.components)
        Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl()
        Me.c2 = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.lsc = New DevExpress.XtraEditors.LabelControl()
        Me.badd = New DevExpress.XtraEditors.SimpleButton()
        Me.ToolTipController1 = New DevExpress.Utils.ToolTipController(Me.components)
        Me.bdel = New DevExpress.XtraEditors.SimpleButton()
        Me.bupd = New DevExpress.XtraEditors.SimpleButton()
        Me.Ci = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.Li = New DevExpress.XtraEditors.LabelControl()
        Me.Ct = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.Lt = New DevExpress.XtraEditors.LabelControl()
        Me.csc = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.L2 = New DevExpress.XtraEditors.LabelControl()
        Me.L1 = New DevExpress.XtraEditors.LabelControl()
        Me.lyb = New DevExpress.XtraEditors.LabelControl()
        Me.C1 = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.cyb = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.RadioGroup1 = New DevExpress.XtraEditors.RadioGroup()
        Me.LabelControl1 = New DevExpress.XtraEditors.LabelControl()
        Me.Tname = New DevExpress.XtraEditors.TextEdit()
        Me.SimpleButton3 = New DevExpress.XtraEditors.SimpleButton()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        CType(Me.uxlib, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl1.SuspendLayout()
        CType(Me.c2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Ci.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Ct.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.csc.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.C1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cyb.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadioGroup1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Tname.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'uxlib
        '
        Me.uxlib.ActiveFilterEnabled = False
        Me.uxlib.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxlib.Columns.AddRange(New DevExpress.XtraTreeList.Columns.TreeListColumn() {Me.TreeListColumn4, Me.TreeListColumn5, Me.TreeListColumn6})
        Me.uxlib.Location = New System.Drawing.Point(17, 12)
        Me.uxlib.Name = "uxlib"
        Me.uxlib.OptionsBehavior.Editable = False
        Me.uxlib.OptionsFilter.AllowColumnMRUFilterList = False
        Me.uxlib.OptionsFilter.AllowFilterEditor = False
        Me.uxlib.OptionsFilter.AllowMRUFilterList = False
        Me.uxlib.OptionsMenu.EnableColumnMenu = False
        Me.uxlib.OptionsSelection.EnableAppearanceFocusedCell = False
        Me.uxlib.OptionsView.ShowButtons = False
        Me.uxlib.OptionsView.ShowFocusedFrame = False
        Me.uxlib.OptionsView.ShowHorzLines = False
        Me.uxlib.OptionsView.ShowIndicator = False
        Me.uxlib.OptionsView.ShowRoot = False
        Me.uxlib.OptionsView.ShowVertLines = False
        Me.uxlib.Size = New System.Drawing.Size(615, 280)
        Me.uxlib.TabIndex = 10
        '
        'TreeListColumn4
        '
        Me.TreeListColumn4.Caption = "Name"
        Me.TreeListColumn4.FieldName = "Page"
        Me.TreeListColumn4.MinWidth = 33
        Me.TreeListColumn4.Name = "TreeListColumn4"
        Me.TreeListColumn4.OptionsColumn.ReadOnly = True
        Me.TreeListColumn4.UnboundType = DevExpress.XtraTreeList.Data.UnboundColumnType.[String]
        Me.TreeListColumn4.Visible = True
        Me.TreeListColumn4.VisibleIndex = 0
        Me.TreeListColumn4.Width = 168
        '
        'TreeListColumn5
        '
        Me.TreeListColumn5.Caption = "Type"
        Me.TreeListColumn5.FieldName = "Warning"
        Me.TreeListColumn5.Name = "TreeListColumn5"
        Me.TreeListColumn5.UnboundType = DevExpress.XtraTreeList.Data.UnboundColumnType.[String]
        Me.TreeListColumn5.Visible = True
        Me.TreeListColumn5.VisibleIndex = 1
        Me.TreeListColumn5.Width = 162
        '
        'TreeListColumn6
        '
        Me.TreeListColumn6.Caption = "Description"
        Me.TreeListColumn6.FieldName = "Created By"
        Me.TreeListColumn6.Name = "TreeListColumn6"
        Me.TreeListColumn6.UnboundType = DevExpress.XtraTreeList.Data.UnboundColumnType.[String]
        Me.TreeListColumn6.Visible = True
        Me.TreeListColumn6.VisibleIndex = 2
        Me.TreeListColumn6.Width = 277
        '
        'bcancel
        '
        Me.bcancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bcancel.ImageIndex = 2
        Me.bcancel.ImageList = Me.uxImages
        Me.bcancel.Location = New System.Drawing.Point(474, 456)
        Me.bcancel.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.bcancel.Name = "bcancel"
        Me.bcancel.Size = New System.Drawing.Size(75, 33)
        Me.bcancel.TabIndex = 12
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
        'PanelControl1
        '
        Me.PanelControl1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PanelControl1.Controls.Add(Me.c2)
        Me.PanelControl1.Controls.Add(Me.lsc)
        Me.PanelControl1.Controls.Add(Me.badd)
        Me.PanelControl1.Controls.Add(Me.bdel)
        Me.PanelControl1.Controls.Add(Me.bupd)
        Me.PanelControl1.Controls.Add(Me.Ci)
        Me.PanelControl1.Controls.Add(Me.Li)
        Me.PanelControl1.Controls.Add(Me.Ct)
        Me.PanelControl1.Controls.Add(Me.Lt)
        Me.PanelControl1.Controls.Add(Me.csc)
        Me.PanelControl1.Controls.Add(Me.L2)
        Me.PanelControl1.Controls.Add(Me.L1)
        Me.PanelControl1.Controls.Add(Me.lyb)
        Me.PanelControl1.Controls.Add(Me.C1)
        Me.PanelControl1.Controls.Add(Me.cyb)
        Me.PanelControl1.Controls.Add(Me.RadioGroup1)
        Me.PanelControl1.Controls.Add(Me.LabelControl1)
        Me.PanelControl1.Controls.Add(Me.Tname)
        Me.PanelControl1.Location = New System.Drawing.Point(17, 298)
        Me.PanelControl1.Name = "PanelControl1"
        Me.PanelControl1.Size = New System.Drawing.Size(615, 152)
        Me.PanelControl1.TabIndex = 13
        '
        'c2
        '
        Me.c2.EditValue = ""
        Me.c2.Location = New System.Drawing.Point(453, 66)
        Me.c2.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.c2.Name = "c2"
        Me.c2.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.c2.Properties.MaxLength = 250
        Me.c2.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        Me.c2.Size = New System.Drawing.Size(133, 20)
        Me.c2.TabIndex = 70
        Me.c2.Visible = False
        '
        'lsc
        '
        Me.lsc.Location = New System.Drawing.Point(149, 125)
        Me.lsc.Name = "lsc"
        Me.lsc.Size = New System.Drawing.Size(91, 13)
        Me.lsc.TabIndex = 69
        Me.lsc.Text = "Source Contributor"
        Me.lsc.Visible = False
        '
        'badd
        '
        Me.badd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.badd.Enabled = False
        Me.badd.ImageIndex = 5
        Me.badd.ImageList = Me.uxImages
        Me.badd.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter
        Me.badd.Location = New System.Drawing.Point(540, 28)
        Me.badd.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.badd.Name = "badd"
        Me.badd.Size = New System.Drawing.Size(30, 30)
        Me.badd.TabIndex = 68
        Me.badd.ToolTip = "New"
        Me.badd.ToolTipController = Me.ToolTipController1
        '
        'bdel
        '
        Me.bdel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bdel.Enabled = False
        Me.bdel.ImageIndex = 2
        Me.bdel.ImageList = Me.uxImages
        Me.bdel.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter
        Me.bdel.Location = New System.Drawing.Point(502, 28)
        Me.bdel.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.bdel.Name = "bdel"
        Me.bdel.Size = New System.Drawing.Size(30, 30)
        Me.bdel.TabIndex = 67
        Me.bdel.ToolTip = "Delete"
        Me.bdel.ToolTipController = Me.ToolTipController1
        '
        'bupd
        '
        Me.bupd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bupd.Enabled = False
        Me.bupd.ImageIndex = 4
        Me.bupd.ImageList = Me.uxImages
        Me.bupd.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter
        Me.bupd.Location = New System.Drawing.Point(576, 28)
        Me.bupd.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.bupd.Name = "bupd"
        Me.bupd.Size = New System.Drawing.Size(30, 30)
        Me.bupd.TabIndex = 14
        Me.bupd.ToolTip = "Update"
        Me.bupd.ToolTipController = Me.ToolTipController1
        '
        'Ci
        '
        Me.Ci.EditValue = ""
        Me.Ci.Location = New System.Drawing.Point(453, 94)
        Me.Ci.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Ci.Name = "Ci"
        Me.Ci.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.Ci.Properties.MaxLength = 250
        Me.Ci.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        Me.Ci.Size = New System.Drawing.Size(59, 20)
        Me.Ci.TabIndex = 66
        Me.Ci.Visible = False
        '
        'Li
        '
        Me.Li.Location = New System.Drawing.Point(383, 97)
        Me.Li.Name = "Li"
        Me.Li.Size = New System.Drawing.Size(38, 13)
        Me.Li.TabIndex = 65
        Me.Li.Text = "Interval"
        Me.Li.Visible = False
        '
        'Ct
        '
        Me.Ct.EditValue = ""
        Me.Ct.Location = New System.Drawing.Point(225, 94)
        Me.Ct.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Ct.Name = "Ct"
        Me.Ct.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.Ct.Properties.MaxLength = 250
        Me.Ct.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        Me.Ct.Size = New System.Drawing.Size(133, 20)
        Me.Ct.TabIndex = 64
        Me.Ct.Visible = False
        '
        'Lt
        '
        Me.Lt.Location = New System.Drawing.Point(149, 97)
        Me.Lt.Name = "Lt"
        Me.Lt.Size = New System.Drawing.Size(65, 13)
        Me.Lt.TabIndex = 63
        Me.Lt.Text = "Interval Type"
        Me.Lt.Visible = False
        '
        'csc
        '
        Me.csc.EditValue = ""
        Me.csc.Location = New System.Drawing.Point(247, 122)
        Me.csc.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.csc.Name = "csc"
        Me.csc.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.csc.Properties.MaxLength = 250
        Me.csc.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        Me.csc.Size = New System.Drawing.Size(133, 20)
        Me.csc.TabIndex = 62
        Me.csc.Visible = False
        '
        'L2
        '
        Me.L2.Location = New System.Drawing.Point(383, 69)
        Me.L2.Name = "L2"
        Me.L2.Size = New System.Drawing.Size(64, 13)
        Me.L2.TabIndex = 61
        Me.L2.Text = "Contributor 2"
        Me.L2.Visible = False
        '
        'L1
        '
        Me.L1.Location = New System.Drawing.Point(150, 69)
        Me.L1.Name = "L1"
        Me.L1.Size = New System.Drawing.Size(64, 13)
        Me.L1.TabIndex = 60
        Me.L1.Text = "Contributor 1"
        Me.L1.Visible = False
        '
        'lyb
        '
        Me.lyb.Location = New System.Drawing.Point(151, 39)
        Me.lyb.Name = "lyb"
        Me.lyb.Size = New System.Drawing.Size(52, 13)
        Me.lyb.TabIndex = 59
        Me.lyb.Text = "Years Back"
        Me.lyb.Visible = False
        '
        'C1
        '
        Me.C1.EditValue = ""
        Me.C1.Location = New System.Drawing.Point(225, 66)
        Me.C1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.C1.Name = "C1"
        Me.C1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.C1.Properties.MaxLength = 250
        Me.C1.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        Me.C1.Size = New System.Drawing.Size(133, 20)
        Me.C1.TabIndex = 58
        Me.C1.Visible = False
        '
        'cyb
        '
        Me.cyb.EditValue = ""
        Me.cyb.Location = New System.Drawing.Point(225, 36)
        Me.cyb.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.cyb.Name = "cyb"
        Me.cyb.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.cyb.Properties.MaxLength = 250
        Me.cyb.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        Me.cyb.Size = New System.Drawing.Size(59, 20)
        Me.cyb.TabIndex = 57
        Me.cyb.Visible = False
        '
        'RadioGroup1
        '
        Me.RadioGroup1.Location = New System.Drawing.Point(17, 31)
        Me.RadioGroup1.Name = "RadioGroup1"
        Me.RadioGroup1.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.RadioGroup1.Properties.Appearance.Options.UseBackColor = True
        Me.RadioGroup1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me.RadioGroup1.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.RadioGroupItem() {New DevExpress.XtraEditors.Controls.RadioGroupItem(CType(1, Short), "Growth"), New DevExpress.XtraEditors.Controls.RadioGroupItem(CType(2, Short), "Cross Contributor"), New DevExpress.XtraEditors.Controls.RadioGroupItem(CType(3, Short), "Momentum"), New DevExpress.XtraEditors.Controls.RadioGroupItem(CType(4, Short), "Static to Period ")})
        Me.RadioGroup1.Size = New System.Drawing.Size(146, 116)
        Me.RadioGroup1.TabIndex = 2
        '
        'LabelControl1
        '
        Me.LabelControl1.Location = New System.Drawing.Point(15, 8)
        Me.LabelControl1.Name = "LabelControl1"
        Me.LabelControl1.Size = New System.Drawing.Size(27, 13)
        Me.LabelControl1.TabIndex = 1
        Me.LabelControl1.Text = "Name"
        '
        'Tname
        '
        Me.Tname.Location = New System.Drawing.Point(61, 5)
        Me.Tname.Name = "Tname"
        Me.Tname.Size = New System.Drawing.Size(223, 20)
        Me.Tname.TabIndex = 0
        '
        'SimpleButton3
        '
        Me.SimpleButton3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SimpleButton3.ImageIndex = 4
        Me.SimpleButton3.ImageList = Me.uxImages
        Me.SimpleButton3.Location = New System.Drawing.Point(557, 456)
        Me.SimpleButton3.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.SimpleButton3.Name = "SimpleButton3"
        Me.SimpleButton3.Size = New System.Drawing.Size(75, 33)
        Me.SimpleButton3.TabIndex = 14
        Me.SimpleButton3.Text = "Save"
        '
        'PictureBox2
        '
        Me.PictureBox2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(17, 456)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(161, 33)
        Me.PictureBox2.TabIndex = 65
        Me.PictureBox2.TabStop = False
        '
        'bc_am_in_calc_lib
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(652, 499)
        Me.Controls.Add(Me.PictureBox2)
        Me.Controls.Add(Me.SimpleButton3)
        Me.Controls.Add(Me.PanelControl1)
        Me.Controls.Add(Me.bcancel)
        Me.Controls.Add(Me.uxlib)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "bc_am_in_calc_lib"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "BlueCurve - Insight Library Calculations"
        CType(Me.uxlib, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl1.ResumeLayout(False)
        Me.PanelControl1.PerformLayout()
        CType(Me.c2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Ci.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Ct.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.csc.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.C1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cyb.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadioGroup1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Tname.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents uxlib As DevExpress.XtraTreeList.TreeList
    Friend WithEvents TreeListColumn4 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents TreeListColumn5 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents TreeListColumn6 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents bcancel As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents uxImages As System.Windows.Forms.ImageList
    Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents L1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lyb As DevExpress.XtraEditors.LabelControl
    Friend WithEvents C1 As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents cyb As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents RadioGroup1 As DevExpress.XtraEditors.RadioGroup
    Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents Tname As DevExpress.XtraEditors.TextEdit
    Friend WithEvents csc As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents L2 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents Ci As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents Li As DevExpress.XtraEditors.LabelControl
    Friend WithEvents Ct As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents Lt As DevExpress.XtraEditors.LabelControl
    Friend WithEvents badd As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents bdel As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents bupd As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SimpleButton3 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents ToolTipController1 As DevExpress.Utils.ToolTipController
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents lsc As DevExpress.XtraEditors.LabelControl
    Friend WithEvents c2 As DevExpress.XtraEditors.ComboBoxEdit
End Class
