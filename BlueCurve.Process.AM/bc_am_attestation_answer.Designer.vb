<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bc_am_attestation_answer
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_attestation_answer))
        Me.uxq = New DevExpress.XtraEditors.MemoEdit()
        Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl()
        Me.ppass = New DevExpress.XtraEditors.PictureEdit()
        Me.pfail = New DevExpress.XtraEditors.PictureEdit()
        Me.la = New DevExpress.XtraEditors.LabelControl()
        CType(Me.uxq.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl1.SuspendLayout()
        CType(Me.ppass.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pfail.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'uxq
        '
        Me.uxq.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxq.Location = New System.Drawing.Point(5, 5)
        Me.uxq.Name = "uxq"
        Me.uxq.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(236, Byte), Integer), CType(CType(239, Byte), Integer))
        Me.uxq.Properties.Appearance.Options.UseBackColor = True
        Me.uxq.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me.uxq.Properties.ReadOnly = True
        Me.uxq.Size = New System.Drawing.Size(527, 42)
        Me.uxq.TabIndex = 7
        '
        'PanelControl1
        '
        Me.PanelControl1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PanelControl1.Controls.Add(Me.ppass)
        Me.PanelControl1.Controls.Add(Me.pfail)
        Me.PanelControl1.Controls.Add(Me.la)
        Me.PanelControl1.Controls.Add(Me.uxq)
        Me.PanelControl1.Location = New System.Drawing.Point(3, 3)
        Me.PanelControl1.Name = "PanelControl1"
        Me.PanelControl1.Size = New System.Drawing.Size(792, 56)
        Me.PanelControl1.TabIndex = 10
        '
        'ppass
        '
        Me.ppass.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ppass.EditValue = CType(resources.GetObject("ppass.EditValue"), Object)
        Me.ppass.Location = New System.Drawing.Point(538, 5)
        Me.ppass.Name = "ppass"
        Me.ppass.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.ppass.Properties.Appearance.Options.UseBackColor = True
        Me.ppass.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me.ppass.Properties.ReadOnly = True
        Me.ppass.Size = New System.Drawing.Size(61, 42)
        Me.ppass.TabIndex = 12
        Me.ppass.Visible = False
        '
        'pfail
        '
        Me.pfail.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pfail.EditValue = CType(resources.GetObject("pfail.EditValue"), Object)
        Me.pfail.Location = New System.Drawing.Point(538, 5)
        Me.pfail.Name = "pfail"
        Me.pfail.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.pfail.Properties.Appearance.Options.UseBackColor = True
        Me.pfail.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me.pfail.Properties.ReadOnly = True
        Me.pfail.Size = New System.Drawing.Size(61, 42)
        Me.pfail.TabIndex = 11
        Me.pfail.Visible = False
        '
        'la
        '
        Me.la.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.la.Appearance.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.la.Location = New System.Drawing.Point(605, 21)
        Me.la.Name = "la"
        Me.la.Size = New System.Drawing.Size(66, 13)
        Me.la.TabIndex = 10
        Me.la.Text = "LabelControl1"
        '
        'bc_am_attestation_answer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.PanelControl1)
        Me.Name = "bc_am_attestation_answer"
        Me.Size = New System.Drawing.Size(802, 64)
        CType(Me.uxq.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl1.ResumeLayout(False)
        Me.PanelControl1.PerformLayout()
        CType(Me.ppass.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pfail.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents uxq As DevExpress.XtraEditors.MemoEdit
    Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents la As DevExpress.XtraEditors.LabelControl
    Friend WithEvents ppass As DevExpress.XtraEditors.PictureEdit
    Friend WithEvents pfail As DevExpress.XtraEditors.PictureEdit

End Class
