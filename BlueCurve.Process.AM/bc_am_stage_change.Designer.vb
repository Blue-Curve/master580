<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bc_am_stage_change
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_stage_change))
        Me.uxComment = New DevExpress.XtraEditors.MemoEdit()
        Me.Ltitle = New DevExpress.XtraEditors.LabelControl()
        Me.SimpleButton1 = New DevExpress.XtraEditors.SimpleButton()
        Me.SimpleButton2 = New DevExpress.XtraEditors.SimpleButton()
        Me.Chkcompletion = New DevExpress.XtraEditors.CheckEdit()
        Me.DateEdit1 = New DevExpress.XtraEditors.DateEdit()
        Me.uxtime = New DevExpress.XtraEditors.TimeEdit()
        Me.uxnextuser = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.lnextuser = New DevExpress.XtraEditors.LabelControl()
        CType(Me.uxComment.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Chkcompletion.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DateEdit1.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DateEdit1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxtime.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxnextuser.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'uxComment
        '
        Me.uxComment.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxComment.EditValue = ""
        Me.uxComment.Location = New System.Drawing.Point(12, 31)
        Me.uxComment.Name = "uxComment"
        Me.uxComment.Properties.Appearance.BackColor = System.Drawing.Color.White
        Me.uxComment.Properties.Appearance.Options.UseBackColor = True
        Me.uxComment.Size = New System.Drawing.Size(351, 140)
        Me.uxComment.TabIndex = 19
        '
        'Ltitle
        '
        Me.Ltitle.Location = New System.Drawing.Point(12, 12)
        Me.Ltitle.Name = "Ltitle"
        Me.Ltitle.Size = New System.Drawing.Size(116, 13)
        Me.Ltitle.TabIndex = 21
        Me.Ltitle.Text = "Stage Change Comment"
        '
        'SimpleButton1
        '
        Me.SimpleButton1.Location = New System.Drawing.Point(302, 210)
        Me.SimpleButton1.Name = "SimpleButton1"
        Me.SimpleButton1.Size = New System.Drawing.Size(61, 29)
        Me.SimpleButton1.TabIndex = 20
        Me.SimpleButton1.Text = "Ok"
        '
        'SimpleButton2
        '
        Me.SimpleButton2.Location = New System.Drawing.Point(229, 210)
        Me.SimpleButton2.Name = "SimpleButton2"
        Me.SimpleButton2.Size = New System.Drawing.Size(63, 29)
        Me.SimpleButton2.TabIndex = 22
        Me.SimpleButton2.Text = "Cancel"
        '
        'Chkcompletion
        '
        Me.Chkcompletion.Location = New System.Drawing.Point(10, 177)
        Me.Chkcompletion.Name = "Chkcompletion"
        Me.Chkcompletion.Properties.Caption = "Next Stage Completed By"
        Me.Chkcompletion.Size = New System.Drawing.Size(144, 19)
        Me.Chkcompletion.TabIndex = 23
        '
        'DateEdit1
        '
        Me.DateEdit1.EditValue = New Date(2013, 10, 10, 0, 0, 0, 0)
        Me.DateEdit1.Enabled = False
        Me.DateEdit1.Location = New System.Drawing.Point(204, 178)
        Me.DateEdit1.Name = "DateEdit1"
        Me.DateEdit1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.DateEdit1.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
        Me.DateEdit1.Size = New System.Drawing.Size(88, 20)
        Me.DateEdit1.TabIndex = 24
        '
        'uxtime
        '
        Me.uxtime.EditValue = New Date(2013, 11, 8, 0, 0, 0, 0)
        Me.uxtime.Location = New System.Drawing.Point(298, 178)
        Me.uxtime.Name = "uxtime"
        Me.uxtime.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
        Me.uxtime.Properties.EditFormat.FormatString = """HH:mm"""
        Me.uxtime.Size = New System.Drawing.Size(65, 20)
        Me.uxtime.TabIndex = 46
        '
        'uxnextuser
        '
        Me.uxnextuser.Location = New System.Drawing.Point(12, 219)
        Me.uxnextuser.Name = "uxnextuser"
        Me.uxnextuser.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.uxnextuser.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        Me.uxnextuser.Size = New System.Drawing.Size(211, 20)
        Me.uxnextuser.TabIndex = 47
        Me.uxnextuser.Visible = False
        '
        'lnextuser
        '
        Me.lnextuser.AccessibleRole = System.Windows.Forms.AccessibleRole.IpAddress
        Me.lnextuser.Location = New System.Drawing.Point(12, 202)
        Me.lnextuser.Name = "lnextuser"
        Me.lnextuser.Size = New System.Drawing.Size(44, 13)
        Me.lnextuser.TabIndex = 48
        Me.lnextuser.Text = "Assign to"
        Me.lnextuser.Visible = False
        '
        'bc_am_stage_change
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(375, 251)
        Me.Controls.Add(Me.lnextuser)
        Me.Controls.Add(Me.uxnextuser)
        Me.Controls.Add(Me.uxtime)
        Me.Controls.Add(Me.DateEdit1)
        Me.Controls.Add(Me.Chkcompletion)
        Me.Controls.Add(Me.SimpleButton2)
        Me.Controls.Add(Me.Ltitle)
        Me.Controls.Add(Me.SimpleButton1)
        Me.Controls.Add(Me.uxComment)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "bc_am_stage_change"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Blue Curve  Stage Change"
        CType(Me.uxComment.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Chkcompletion.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DateEdit1.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DateEdit1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxtime.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxnextuser.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents uxComment As DevExpress.XtraEditors.MemoEdit
    Friend WithEvents SimpleButton1 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents Ltitle As DevExpress.XtraEditors.LabelControl
    Friend WithEvents SimpleButton2 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents Chkcompletion As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents DateEdit1 As DevExpress.XtraEditors.DateEdit
    Friend WithEvents uxtime As DevExpress.XtraEditors.TimeEdit
    Friend WithEvents uxnextuser As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents lnextuser As DevExpress.XtraEditors.LabelControl
End Class
