<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bc_am_dd_uri_picker
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.btnUri = New System.Windows.Forms.Button
        Me.txtUri = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'btnUri
        '
        Me.btnUri.Dock = System.Windows.Forms.DockStyle.Right
        Me.btnUri.Location = New System.Drawing.Point(373, 0)
        Me.btnUri.Name = "btnUri"
        Me.btnUri.Size = New System.Drawing.Size(31, 20)
        Me.btnUri.TabIndex = 5
        Me.btnUri.Text = "..."
        Me.btnUri.UseVisualStyleBackColor = True
        '
        'txtUri
        '
        Me.txtUri.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtUri.Location = New System.Drawing.Point(0, 0)
        Me.txtUri.Name = "txtUri"
        Me.txtUri.Size = New System.Drawing.Size(373, 20)
        Me.txtUri.TabIndex = 4
        '
        'bc_am_dd_uri_picker
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Transparent
        Me.Controls.Add(Me.txtUri)
        Me.Controls.Add(Me.btnUri)
        Me.Name = "bc_am_dd_uri_picker"
        Me.Size = New System.Drawing.Size(404, 20)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnUri As System.Windows.Forms.Button
    Friend WithEvents txtUri As System.Windows.Forms.TextBox

End Class
