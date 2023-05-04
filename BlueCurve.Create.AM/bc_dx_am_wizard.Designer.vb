<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bc_dx_am_wizard
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
        Me.uxppubtype = New DevExpress.XtraEditors.PanelControl
        Me.uxallmine = New DevExpress.XtraEditors.RadioGroup
        Me.ltitle = New DevExpress.XtraEditors.LabelControl
        Me.uxdesc = New DevExpress.XtraEditors.LabelControl
        Me.uxpubtypelist = New DevExpress.XtraEditors.ListBoxControl
        Me.SimpleButton1 = New DevExpress.XtraEditors.SimpleButton
        Me.SimpleButton2 = New DevExpress.XtraEditors.SimpleButton
        Me.SimpleButton3 = New DevExpress.XtraEditors.SimpleButton
        Me.SimpleButton4 = New DevExpress.XtraEditors.SimpleButton
        CType(Me.uxppubtype, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.uxppubtype.SuspendLayout()
        CType(Me.uxallmine.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxpubtypelist, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'uxppubtype
        '
        Me.uxppubtype.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxppubtype.Controls.Add(Me.uxallmine)
        Me.uxppubtype.Controls.Add(Me.ltitle)
        Me.uxppubtype.Controls.Add(Me.uxdesc)
        Me.uxppubtype.Controls.Add(Me.uxpubtypelist)
        Me.uxppubtype.Location = New System.Drawing.Point(12, 12)
        Me.uxppubtype.Name = "uxppubtype"
        Me.uxppubtype.Size = New System.Drawing.Size(438, 257)
        Me.uxppubtype.TabIndex = 0
        '
        'uxallmine
        '
        Me.uxallmine.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxallmine.EditValue = "Mine"
        Me.uxallmine.Location = New System.Drawing.Point(328, 232)
        Me.uxallmine.Name = "uxallmine"
        Me.uxallmine.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.uxallmine.Properties.Appearance.Options.UseBackColor = True
        Me.uxallmine.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.RadioGroupItem() {New DevExpress.XtraEditors.Controls.RadioGroupItem("Mine", "Mine"), New DevExpress.XtraEditors.Controls.RadioGroupItem("All", "All")})
        Me.uxallmine.Size = New System.Drawing.Size(100, 19)
        Me.uxallmine.TabIndex = 3
        Me.uxallmine.Visible = False
        '
        'ltitle
        '
        Me.ltitle.Location = New System.Drawing.Point(5, 5)
        Me.ltitle.Name = "ltitle"
        Me.ltitle.Size = New System.Drawing.Size(66, 13)
        Me.ltitle.TabIndex = 2
        Me.ltitle.Text = "LabelControl1"
        '
        'uxdesc
        '
        Me.uxdesc.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.uxdesc.Location = New System.Drawing.Point(5, 238)
        Me.uxdesc.Name = "uxdesc"
        Me.uxdesc.Size = New System.Drawing.Size(0, 13)
        Me.uxdesc.TabIndex = 1
        '
        'uxpubtypelist
        '
        Me.uxpubtypelist.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxpubtypelist.Location = New System.Drawing.Point(5, 19)
        Me.uxpubtypelist.Name = "uxpubtypelist"
        Me.uxpubtypelist.Size = New System.Drawing.Size(423, 207)
        Me.uxpubtypelist.TabIndex = 0
        '
        'SimpleButton1
        '
        Me.SimpleButton1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SimpleButton1.Location = New System.Drawing.Point(375, 290)
        Me.SimpleButton1.Name = "SimpleButton1"
        Me.SimpleButton1.Size = New System.Drawing.Size(75, 23)
        Me.SimpleButton1.TabIndex = 1
        Me.SimpleButton1.Text = "SimpleButton1"
        Me.SimpleButton1.Visible = False
        '
        'SimpleButton2
        '
        Me.SimpleButton2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SimpleButton2.Location = New System.Drawing.Point(294, 290)
        Me.SimpleButton2.Name = "SimpleButton2"
        Me.SimpleButton2.Size = New System.Drawing.Size(75, 23)
        Me.SimpleButton2.TabIndex = 2
        Me.SimpleButton2.Text = "SimpleButton2"
        Me.SimpleButton2.Visible = False
        '
        'SimpleButton3
        '
        Me.SimpleButton3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SimpleButton3.Location = New System.Drawing.Point(213, 290)
        Me.SimpleButton3.Name = "SimpleButton3"
        Me.SimpleButton3.Size = New System.Drawing.Size(75, 23)
        Me.SimpleButton3.TabIndex = 3
        Me.SimpleButton3.Text = "SimpleButton3"
        Me.SimpleButton3.Visible = False
        '
        'SimpleButton4
        '
        Me.SimpleButton4.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SimpleButton4.Location = New System.Drawing.Point(132, 290)
        Me.SimpleButton4.Name = "SimpleButton4"
        Me.SimpleButton4.Size = New System.Drawing.Size(75, 23)
        Me.SimpleButton4.TabIndex = 4
        Me.SimpleButton4.Text = "SimpleButton4"
        Me.SimpleButton4.Visible = False
        '
        'bc_dx_am_wizard
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(459, 325)
        Me.Controls.Add(Me.SimpleButton4)
        Me.Controls.Add(Me.SimpleButton3)
        Me.Controls.Add(Me.SimpleButton2)
        Me.Controls.Add(Me.SimpleButton1)
        Me.Controls.Add(Me.uxppubtype)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "bc_dx_am_wizard"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Blue Curve - Create"
        CType(Me.uxppubtype, System.ComponentModel.ISupportInitialize).EndInit()
        Me.uxppubtype.ResumeLayout(False)
        Me.uxppubtype.PerformLayout()
        CType(Me.uxallmine.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxpubtypelist, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents uxppubtype As DevExpress.XtraEditors.PanelControl
    Friend WithEvents uxdesc As DevExpress.XtraEditors.LabelControl
    Friend WithEvents SimpleButton1 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SimpleButton2 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SimpleButton3 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SimpleButton4 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents ltitle As DevExpress.XtraEditors.LabelControl
    Friend WithEvents uxpubtypelist As DevExpress.XtraEditors.ListBoxControl
    Friend WithEvents uxallmine As DevExpress.XtraEditors.RadioGroup
End Class
