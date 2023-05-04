<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bc_dx_ao_component_swap
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
        Me.RichEditControl1 = New DevExpress.XtraRichEdit.RichEditControl()
        Me.SuspendLayout()
        '
        'RichEditControl1
        '
        Me.RichEditControl1.EnableToolTips = True
        Me.RichEditControl1.Location = New System.Drawing.Point(12, 12)
        Me.RichEditControl1.Name = "RichEditControl1"
        Me.RichEditControl1.Options.CopyPaste.MaintainDocumentSectionSettings = False
        Me.RichEditControl1.Options.Fields.UseCurrentCultureDateTimeFormat = False
        Me.RichEditControl1.Options.MailMerge.KeepLastParagraph = False
        Me.RichEditControl1.Size = New System.Drawing.Size(260, 200)
        Me.RichEditControl1.TabIndex = 0
        Me.RichEditControl1.Text = "RichEditControl1"
        '
        'bc_dx_ao_component_swap
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(284, 262)
        Me.Controls.Add(Me.RichEditControl1)
        Me.Name = "bc_dx_ao_component_swap"
        Me.Text = "bc_dx_ao_component_swap"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents RichEditControl1 As DevExpress.XtraRichEdit.RichEditControl
End Class
