<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bc_am_dd_option
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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
        Me.uxFileType = New System.Windows.Forms.GroupBox
        Me.rb2 = New System.Windows.Forms.RadioButton
        Me.rb1 = New System.Windows.Forms.RadioButton
        Me.uxFileType.SuspendLayout()
        Me.SuspendLayout()
        '
        'uxFileType
        '
        Me.uxFileType.Controls.Add(Me.rb2)
        Me.uxFileType.Controls.Add(Me.rb1)
        Me.uxFileType.Location = New System.Drawing.Point(12, 12)
        Me.uxFileType.Name = "uxFileType"
        Me.uxFileType.Size = New System.Drawing.Size(131, 52)
        Me.uxFileType.TabIndex = 15
        Me.uxFileType.TabStop = False
        '
        'rb2
        '
        Me.rb2.AutoSize = True
        Me.rb2.Location = New System.Drawing.Point(63, 19)
        Me.rb2.Name = "rb2"
        Me.rb2.Size = New System.Drawing.Size(63, 17)
        Me.rb2.TabIndex = 16
        Me.rb2.TabStop = True
        Me.rb2.Text = "Exclude"
        Me.rb2.UseVisualStyleBackColor = True
        '
        'rb1
        '
        Me.rb1.AutoSize = True
        Me.rb1.Location = New System.Drawing.Point(6, 19)
        Me.rb1.Name = "rb1"
        Me.rb1.Size = New System.Drawing.Size(55, 17)
        Me.rb1.TabIndex = 15
        Me.rb1.TabStop = True
        Me.rb1.Text = "Export"
        Me.rb1.UseVisualStyleBackColor = True
        '
        'bc_am_dd_option
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(157, 86)
        Me.ControlBox = False
        Me.Controls.Add(Me.uxFileType)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "bc_am_dd_option"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Save File As"
        Me.uxFileType.ResumeLayout(False)
        Me.uxFileType.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents uxFileType As System.Windows.Forms.GroupBox
    Friend WithEvents rb2 As System.Windows.Forms.RadioButton
    Friend WithEvents rb1 As System.Windows.Forms.RadioButton
End Class
