<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bc_dx_att_popup1
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_dx_att_popup1))
        Me.llen = New DevExpress.XtraEditors.LabelControl()
        Me.uxdate = New DevExpress.XtraEditors.DateEdit()
        Me.bsave = New DevExpress.XtraEditors.SimpleButton()
        Me.bcancel = New DevExpress.XtraEditors.SimpleButton()
        Me.uxmemo = New DevExpress.XtraEditors.MemoEdit()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        CType(Me.uxdate.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxdate.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxmemo.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'llen
        '
        Me.llen.Location = New System.Drawing.Point(12, 169)
        Me.llen.Name = "llen"
        Me.llen.Size = New System.Drawing.Size(0, 13)
        Me.llen.TabIndex = 59
        '
        'uxdate
        '
        Me.uxdate.EditValue = Nothing
        Me.uxdate.Location = New System.Drawing.Point(12, 3)
        Me.uxdate.Name = "uxdate"
        Me.uxdate.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.uxdate.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.uxdate.Properties.DisplayFormat.FormatString = "dd MMM yyyy"
        Me.uxdate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        Me.uxdate.Properties.EditFormat.FormatString = "dd MMM yyyy"
        Me.uxdate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        Me.uxdate.Properties.Mask.EditMask = "dd MMM yyyy"
        Me.uxdate.Size = New System.Drawing.Size(402, 20)
        Me.uxdate.TabIndex = 58
        '
        'bsave
        '
        Me.bsave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bsave.Enabled = False
        Me.bsave.ImageIndex = 1
        Me.bsave.Location = New System.Drawing.Point(260, 180)
        Me.bsave.Name = "bsave"
        Me.bsave.Size = New System.Drawing.Size(74, 32)
        Me.bsave.TabIndex = 56
        Me.bsave.Text = "Save"
        '
        'bcancel
        '
        Me.bcancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bcancel.ImageIndex = 1
        Me.bcancel.Location = New System.Drawing.Point(340, 180)
        Me.bcancel.Name = "bcancel"
        Me.bcancel.Size = New System.Drawing.Size(74, 32)
        Me.bcancel.TabIndex = 55
        Me.bcancel.Text = "Cancel"
        '
        'uxmemo
        '
        Me.uxmemo.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxmemo.Location = New System.Drawing.Point(12, 4)
        Me.uxmemo.Name = "uxmemo"
        Me.uxmemo.Size = New System.Drawing.Size(402, 159)
        Me.uxmemo.TabIndex = 54
        '
        'PictureBox2
        '
        Me.PictureBox2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(12, 186)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(175, 35)
        Me.PictureBox2.TabIndex = 57
        Me.PictureBox2.TabStop = False
        '
        'bc_dx_att_popup1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(426, 224)
        Me.ControlBox = False
        Me.Controls.Add(Me.llen)
        Me.Controls.Add(Me.uxdate)
        Me.Controls.Add(Me.PictureBox2)
        Me.Controls.Add(Me.bsave)
        Me.Controls.Add(Me.bcancel)
        Me.Controls.Add(Me.uxmemo)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MinimizeBox = False
        Me.Name = "bc_dx_att_popup1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        CType(Me.uxdate.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxdate.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxmemo.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents llen As DevExpress.XtraEditors.LabelControl
    Friend WithEvents uxdate As DevExpress.XtraEditors.DateEdit
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents bsave As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents bcancel As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents uxmemo As DevExpress.XtraEditors.MemoEdit
End Class
