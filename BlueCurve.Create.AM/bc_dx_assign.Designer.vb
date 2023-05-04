<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bc_dx_assign
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_dx_assign))
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.bok = New DevExpress.XtraEditors.SimpleButton()
        Me.uxImages = New System.Windows.Forms.ImageList()
        Me.LabelControl7 = New DevExpress.XtraEditors.LabelControl()
        Me.LabelControl1 = New DevExpress.XtraEditors.LabelControl()
        Me.lsearch = New DevExpress.XtraEditors.PictureEdit()
        Me.bdn = New DevExpress.XtraEditors.SimpleButton()
        Me.uximage = New DevExpress.Utils.ImageCollection()
        Me.bup = New DevExpress.XtraEditors.SimpleButton()
        Me.tsearch = New DevExpress.XtraEditors.TextEdit()
        Me.uxsel = New DevExpress.XtraEditors.ListBoxControl()
        Me.uxall = New DevExpress.XtraEditors.ListBoxControl()
        Me.SimpleButton1 = New DevExpress.XtraEditors.SimpleButton()
        Me.searchtimer = New System.Windows.Forms.Timer()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lsearch.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uximage, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.tsearch.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxsel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxall, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PictureBox2
        '
        Me.PictureBox2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(12, 273)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(161, 33)
        Me.PictureBox2.TabIndex = 68
        Me.PictureBox2.TabStop = False
        '
        'bok
        '
        Me.bok.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bok.ImageIndex = 4
        Me.bok.ImageList = Me.uxImages
        Me.bok.Location = New System.Drawing.Point(511, 273)
        Me.bok.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.bok.Name = "bok"
        Me.bok.Size = New System.Drawing.Size(75, 33)
        Me.bok.TabIndex = 69
        Me.bok.Text = "Save"
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
        'LabelControl7
        '
        Me.LabelControl7.Location = New System.Drawing.Point(12, 11)
        Me.LabelControl7.Name = "LabelControl7"
        Me.LabelControl7.Size = New System.Drawing.Size(43, 13)
        Me.LabelControl7.TabIndex = 70
        Me.LabelControl7.Text = "Available"
        '
        'LabelControl1
        '
        Me.LabelControl1.Location = New System.Drawing.Point(306, 10)
        Me.LabelControl1.Name = "LabelControl1"
        Me.LabelControl1.Size = New System.Drawing.Size(41, 13)
        Me.LabelControl1.TabIndex = 71
        Me.LabelControl1.Text = "Selected"
        '
        'lsearch
        '
        Me.lsearch.EditValue = Global.BlueCurve.Create.AM.My.Resources.Resources.search2
        Me.lsearch.Location = New System.Drawing.Point(12, 235)
        Me.lsearch.Name = "lsearch"
        Me.lsearch.Size = New System.Drawing.Size(26, 24)
        Me.lsearch.TabIndex = 73
        '
        'bdn
        '
        Me.bdn.AllowDrop = True
        Me.bdn.Enabled = False
        Me.bdn.ImageIndex = 0
        Me.bdn.ImageList = Me.uximage
        Me.bdn.Location = New System.Drawing.Point(562, 57)
        Me.bdn.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.bdn.Name = "bdn"
        Me.bdn.Size = New System.Drawing.Size(24, 24)
        Me.bdn.TabIndex = 77
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
        Me.bup.Enabled = False
        Me.bup.ImageIndex = 2
        Me.bup.ImageList = Me.uximage
        Me.bup.Location = New System.Drawing.Point(562, 29)
        Me.bup.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.bup.Name = "bup"
        Me.bup.Size = New System.Drawing.Size(24, 24)
        Me.bup.TabIndex = 76
        Me.bup.Tag = " "
        '
        'tsearch
        '
        Me.tsearch.EditValue = ""
        Me.tsearch.Location = New System.Drawing.Point(45, 237)
        Me.tsearch.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.tsearch.Name = "tsearch"
        Me.tsearch.Properties.MaxLength = 250
        Me.tsearch.Size = New System.Drawing.Size(218, 20)
        Me.tsearch.TabIndex = 75
        '
        'uxsel
        '
        Me.uxsel.Location = New System.Drawing.Point(306, 29)
        Me.uxsel.Name = "uxsel"
        Me.uxsel.Size = New System.Drawing.Size(250, 199)
        Me.uxsel.TabIndex = 74
        '
        'uxall
        '
        Me.uxall.Location = New System.Drawing.Point(12, 30)
        Me.uxall.Name = "uxall"
        Me.uxall.Size = New System.Drawing.Size(252, 199)
        Me.uxall.TabIndex = 72
        '
        'SimpleButton1
        '
        Me.SimpleButton1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SimpleButton1.ImageIndex = 2
        Me.SimpleButton1.ImageList = Me.uxImages
        Me.SimpleButton1.Location = New System.Drawing.Point(412, 273)
        Me.SimpleButton1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.SimpleButton1.Name = "SimpleButton1"
        Me.SimpleButton1.Size = New System.Drawing.Size(75, 33)
        Me.SimpleButton1.TabIndex = 78
        Me.SimpleButton1.Text = "Cancel"
        '
        'searchtimer
        '
        Me.searchtimer.Enabled = True
        Me.searchtimer.Interval = 1000
        '
        'bc_dx_assign
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(602, 318)
        Me.ControlBox = False
        Me.Controls.Add(Me.SimpleButton1)
        Me.Controls.Add(Me.lsearch)
        Me.Controls.Add(Me.bdn)
        Me.Controls.Add(Me.bup)
        Me.Controls.Add(Me.tsearch)
        Me.Controls.Add(Me.uxsel)
        Me.Controls.Add(Me.uxall)
        Me.Controls.Add(Me.LabelControl1)
        Me.Controls.Add(Me.LabelControl7)
        Me.Controls.Add(Me.bok)
        Me.Controls.Add(Me.PictureBox2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Name = "bc_dx_assign"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Blue Curve - Assigment"
        Me.TopMost = True
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lsearch.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uximage, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.tsearch.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxsel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxall, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents bok As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LabelControl7 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lsearch As DevExpress.XtraEditors.PictureEdit
    Friend WithEvents bdn As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents bup As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents tsearch As DevExpress.XtraEditors.TextEdit
    Friend WithEvents uxsel As DevExpress.XtraEditors.ListBoxControl
    Friend WithEvents uxall As DevExpress.XtraEditors.ListBoxControl
    Friend WithEvents uximage As DevExpress.Utils.ImageCollection
    Friend WithEvents SimpleButton1 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents uxImages As System.Windows.Forms.ImageList
    Friend WithEvents searchtimer As System.Windows.Forms.Timer
End Class
