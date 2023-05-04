<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dx_rtd_param
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
        Me.cvalue = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.lname = New DevExpress.XtraEditors.LabelControl()
        Me.bsel = New DevExpress.XtraEditors.SimpleButton()
        CType(Me.cvalue.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cvalue
        '
        Me.cvalue.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cvalue.Location = New System.Drawing.Point(90, 3)
        Me.cvalue.Name = "cvalue"
        Me.cvalue.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.cvalue.Size = New System.Drawing.Size(212, 20)
        Me.cvalue.TabIndex = 0
        '
        'lname
        '
        Me.lname.Location = New System.Drawing.Point(3, 6)
        Me.lname.Name = "lname"
        Me.lname.Size = New System.Drawing.Size(66, 13)
        Me.lname.TabIndex = 1
        Me.lname.Text = "LabelControl1"
        '
        'bsel
        '
        Me.bsel.AllowFocus = False
        Me.bsel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bsel.Image = Global.BlueCurve.Insight.AM.My.Resources.Resources.Revert_disabled
        Me.bsel.Location = New System.Drawing.Point(308, 0)
        Me.bsel.Name = "bsel"
        Me.bsel.Size = New System.Drawing.Size(27, 25)
        Me.bsel.TabIndex = 2
        '
        'dx_rtd_param
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.bsel)
        Me.Controls.Add(Me.lname)
        Me.Controls.Add(Me.cvalue)
        Me.Name = "dx_rtd_param"
        Me.Size = New System.Drawing.Size(338, 29)
        CType(Me.cvalue.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cvalue As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents lname As DevExpress.XtraEditors.LabelControl
    Friend WithEvents bsel As DevExpress.XtraEditors.SimpleButton

End Class
