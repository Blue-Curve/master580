<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bc_as_real_time_search
    Inherits DevExpress.XtraEditors.XtraUserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.tsearchtext = New DevExpress.XtraEditors.TextEdit()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.lresults = New DevExpress.XtraEditors.ListBoxControl()
        Me.rallmine = New DevExpress.XtraEditors.RadioGroup()
        Me.cfilter = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.cinactive = New DevExpress.XtraEditors.CheckEdit()
        Me.lfilter = New DevExpress.XtraEditors.LabelControl()
        Me.ctestsearch = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.pclear = New DevExpress.XtraEditors.PictureEdit()
        CType(Me.tsearchtext.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lresults, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rallmine.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cfilter.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cinactive.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ctestsearch.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pclear.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'tsearchtext
        '
        Me.tsearchtext.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tsearchtext.Location = New System.Drawing.Point(13, 389)
        Me.tsearchtext.Name = "tsearchtext"
        Me.tsearchtext.Size = New System.Drawing.Size(235, 20)
        Me.tsearchtext.TabIndex = 0
        '
        'Timer1
        '
        Me.Timer1.Interval = 1000
        '
        'lresults
        '
        Me.lresults.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lresults.Location = New System.Drawing.Point(3, 14)
        Me.lresults.Name = "lresults"
        Me.lresults.Size = New System.Drawing.Size(290, 261)
        Me.lresults.TabIndex = 63
        Me.lresults.Visible = False
        '
        'rallmine
        '
        Me.rallmine.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.rallmine.Location = New System.Drawing.Point(3, 360)
        Me.rallmine.Name = "rallmine"
        Me.rallmine.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.RadioGroupItem() {New DevExpress.XtraEditors.Controls.RadioGroupItem(Nothing, "All"), New DevExpress.XtraEditors.Controls.RadioGroupItem(Nothing, "Mine")})
        Me.rallmine.Size = New System.Drawing.Size(100, 23)
        Me.rallmine.TabIndex = 64
        '
        'cfilter
        '
        Me.cfilter.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cfilter.Location = New System.Drawing.Point(3, 325)
        Me.cfilter.Name = "cfilter"
        Me.cfilter.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.cfilter.Size = New System.Drawing.Size(290, 20)
        Me.cfilter.TabIndex = 65
        '
        'cinactive
        '
        Me.cinactive.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cinactive.Location = New System.Drawing.Point(164, 303)
        Me.cinactive.Name = "cinactive"
        Me.cinactive.Properties.Caption = "Show Inactive"
        Me.cinactive.Size = New System.Drawing.Size(129, 19)
        Me.cinactive.TabIndex = 66
        '
        'lfilter
        '
        Me.lfilter.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lfilter.Location = New System.Drawing.Point(5, 306)
        Me.lfilter.Name = "lfilter"
        Me.lfilter.Size = New System.Drawing.Size(43, 13)
        Me.lfilter.TabIndex = 67
        Me.lfilter.Text = "Filter By:"
        '
        'ctestsearch
        '
        Me.ctestsearch.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ctestsearch.Location = New System.Drawing.Point(3, 389)
        Me.ctestsearch.Name = "ctestsearch"
        Me.ctestsearch.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.ctestsearch.Size = New System.Drawing.Size(262, 20)
        Me.ctestsearch.TabIndex = 69
        '
        'pclear
        '
        Me.pclear.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pclear.EditValue = Global.BlueCurve.Core.[AS].My.Resources.Resources.Revert_enabled
        Me.pclear.Location = New System.Drawing.Point(271, 388)
        Me.pclear.Name = "pclear"
        Me.pclear.Size = New System.Drawing.Size(22, 21)
        Me.pclear.TabIndex = 62
        '
        'bc_as_real_time_search
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.ctestsearch)
        Me.Controls.Add(Me.lresults)
        Me.Controls.Add(Me.lfilter)
        Me.Controls.Add(Me.cinactive)
        Me.Controls.Add(Me.cfilter)
        Me.Controls.Add(Me.rallmine)
        Me.Controls.Add(Me.pclear)
        Me.Controls.Add(Me.tsearchtext)
        Me.Name = "bc_as_real_time_search"
        Me.Size = New System.Drawing.Size(296, 416)
        CType(Me.tsearchtext.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lresults, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rallmine.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cfilter.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cinactive.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ctestsearch.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pclear.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents tsearchtext As DevExpress.XtraEditors.TextEdit
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents pclear As DevExpress.XtraEditors.PictureEdit
    Friend WithEvents lresults As DevExpress.XtraEditors.ListBoxControl
    Friend WithEvents rallmine As DevExpress.XtraEditors.RadioGroup
    Friend WithEvents cfilter As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents cinactive As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents lfilter As DevExpress.XtraEditors.LabelControl
    Friend WithEvents ctestsearch As DevExpress.XtraEditors.ComboBoxEdit

End Class
