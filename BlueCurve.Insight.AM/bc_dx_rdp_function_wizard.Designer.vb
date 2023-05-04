<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bc_dx_rdp_function_wizard
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_dx_rdp_function_wizard))
        Me.cfunctions = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.LabelControl1 = New DevExpress.XtraEditors.LabelControl()
        Me.bcancel = New DevExpress.XtraEditors.SimpleButton()
        Me.bok = New DevExpress.XtraEditors.SimpleButton()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.chkopen = New DevExpress.XtraEditors.CheckEdit()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl()
        Me.bdn = New DevExpress.XtraEditors.SimpleButton()
        Me.uximage = New DevExpress.Utils.ImageCollection(Me.components)
        Me.bup = New DevExpress.XtraEditors.SimpleButton()
        Me.chkdim = New DevExpress.XtraEditors.CheckedListBoxControl()
        Me.LabelControl2 = New DevExpress.XtraEditors.LabelControl()
        Me.lrow = New DevExpress.XtraEditors.LabelControl()
        Me.crow = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.lstatus = New DevExpress.XtraEditors.LabelControl()
        CType(Me.cfunctions.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chkopen.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl1.SuspendLayout()
        CType(Me.uximage, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chkdim, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.crow.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cfunctions
        '
        Me.cfunctions.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cfunctions.Location = New System.Drawing.Point(12, 23)
        Me.cfunctions.Name = "cfunctions"
        Me.cfunctions.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.cfunctions.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        Me.cfunctions.Size = New System.Drawing.Size(550, 20)
        Me.cfunctions.TabIndex = 0
        '
        'LabelControl1
        '
        Me.LabelControl1.Appearance.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl1.Location = New System.Drawing.Point(12, 4)
        Me.LabelControl1.Name = "LabelControl1"
        Me.LabelControl1.Size = New System.Drawing.Size(79, 13)
        Me.LabelControl1.TabIndex = 1
        Me.LabelControl1.Text = "I would like to"
        '
        'bcancel
        '
        Me.bcancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bcancel.ImageIndex = 2
        Me.bcancel.Location = New System.Drawing.Point(486, 161)
        Me.bcancel.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.bcancel.Name = "bcancel"
        Me.bcancel.Size = New System.Drawing.Size(75, 33)
        Me.bcancel.TabIndex = 95
        Me.bcancel.Text = "Close"
        '
        'bok
        '
        Me.bok.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bok.ImageIndex = 4
        Me.bok.Location = New System.Drawing.Point(395, 161)
        Me.bok.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.bok.Name = "bok"
        Me.bok.Size = New System.Drawing.Size(75, 33)
        Me.bok.TabIndex = 94
        Me.bok.Text = "Apply"
        '
        'PictureBox2
        '
        Me.PictureBox2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(12, 159)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(188, 34)
        Me.PictureBox2.TabIndex = 98
        Me.PictureBox2.TabStop = False
        '
        'chkopen
        '
        Me.chkopen.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkopen.Location = New System.Drawing.Point(302, 175)
        Me.chkopen.Name = "chkopen"
        Me.chkopen.Properties.Caption = "Keep open"
        Me.chkopen.Size = New System.Drawing.Size(87, 19)
        Me.chkopen.TabIndex = 99
        '
        'Timer1
        '
        Me.Timer1.Interval = 1000
        '
        'PanelControl1
        '
        Me.PanelControl1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PanelControl1.Controls.Add(Me.bdn)
        Me.PanelControl1.Controls.Add(Me.bup)
        Me.PanelControl1.Controls.Add(Me.chkdim)
        Me.PanelControl1.Location = New System.Drawing.Point(12, 67)
        Me.PanelControl1.Name = "PanelControl1"
        Me.PanelControl1.Size = New System.Drawing.Size(549, 86)
        Me.PanelControl1.TabIndex = 100
        '
        'bdn
        '
        Me.bdn.AllowDrop = True
        Me.bdn.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bdn.Enabled = False
        Me.bdn.ImageIndex = 0
        Me.bdn.ImageList = Me.uximage
        Me.bdn.Location = New System.Drawing.Point(520, 37)
        Me.bdn.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.bdn.Name = "bdn"
        Me.bdn.Size = New System.Drawing.Size(24, 24)
        Me.bdn.TabIndex = 51
        '
        'uximage
        '
        Me.uximage.ImageStream = CType(resources.GetObject("uximage.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
        Me.uximage.Images.SetKeyName(0, "down_16.ico")
        Me.uximage.Images.SetKeyName(1, "search_16.ico")
        Me.uximage.Images.SetKeyName(2, "up_16.ico")
        Me.uximage.Images.SetKeyName(3, "read_16.ico")
        '
        'bup
        '
        Me.bup.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bup.Enabled = False
        Me.bup.ImageIndex = 2
        Me.bup.ImageList = Me.uximage
        Me.bup.Location = New System.Drawing.Point(520, 5)
        Me.bup.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.bup.Name = "bup"
        Me.bup.Size = New System.Drawing.Size(24, 24)
        Me.bup.TabIndex = 50
        Me.bup.Tag = " "
        '
        'chkdim
        '
        Me.chkdim.Location = New System.Drawing.Point(5, 5)
        Me.chkdim.Name = "chkdim"
        Me.chkdim.Size = New System.Drawing.Size(509, 76)
        Me.chkdim.TabIndex = 0
        '
        'LabelControl2
        '
        Me.LabelControl2.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.LabelControl2.Appearance.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl2.Location = New System.Drawing.Point(12, 49)
        Me.LabelControl2.Name = "LabelControl2"
        Me.LabelControl2.Size = New System.Drawing.Size(178, 13)
        Me.LabelControl2.TabIndex = 101
        Me.LabelControl2.Text = "I would like to show dimensions"
        '
        'lrow
        '
        Me.lrow.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lrow.Enabled = False
        Me.lrow.Location = New System.Drawing.Point(215, 159)
        Me.lrow.Name = "lrow"
        Me.lrow.Size = New System.Drawing.Size(76, 13)
        Me.lrow.TabIndex = 103
        Me.lrow.Text = "Number of rows"
        '
        'crow
        '
        Me.crow.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.crow.Enabled = False
        Me.crow.Location = New System.Drawing.Point(215, 173)
        Me.crow.Name = "crow"
        Me.crow.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.crow.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        Me.crow.Size = New System.Drawing.Size(81, 20)
        Me.crow.TabIndex = 104
        '
        'lstatus
        '
        Me.lstatus.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lstatus.Appearance.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstatus.Location = New System.Drawing.Point(304, 159)
        Me.lstatus.Name = "lstatus"
        Me.lstatus.Size = New System.Drawing.Size(0, 13)
        Me.lstatus.TabIndex = 105
        '
        'bc_dx_rdp_function_wizard
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(574, 195)
        Me.Controls.Add(Me.lstatus)
        Me.Controls.Add(Me.crow)
        Me.Controls.Add(Me.lrow)
        Me.Controls.Add(Me.LabelControl2)
        Me.Controls.Add(Me.PanelControl1)
        Me.Controls.Add(Me.chkopen)
        Me.Controls.Add(Me.PictureBox2)
        Me.Controls.Add(Me.bcancel)
        Me.Controls.Add(Me.bok)
        Me.Controls.Add(Me.LabelControl1)
        Me.Controls.Add(Me.cfunctions)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "bc_dx_rdp_function_wizard"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Blue Curve - Excel RTD Function Wizard"
        CType(Me.cfunctions.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chkopen.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl1.ResumeLayout(False)
        CType(Me.uximage, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chkdim, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.crow.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cfunctions As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents bcancel As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents bok As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents chkopen As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents chkdim As DevExpress.XtraEditors.CheckedListBoxControl
    Friend WithEvents LabelControl2 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents bdn As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents bup As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents uximage As DevExpress.Utils.ImageCollection
    Friend WithEvents lrow As DevExpress.XtraEditors.LabelControl
    Friend WithEvents crow As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents lstatus As DevExpress.XtraEditors.LabelControl
End Class
