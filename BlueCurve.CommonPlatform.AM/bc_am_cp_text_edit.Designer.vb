<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bc_am_text_edit
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
        Me.ttextentry = New System.Windows.Forms.TextBox
        Me.Button1 = New System.Windows.Forms.Button
        Me.Bok = New System.Windows.Forms.Button
        Me.Llength = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'ttextentry
        '
        Me.ttextentry.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ttextentry.Location = New System.Drawing.Point(12, 12)
        Me.ttextentry.Multiline = True
        Me.ttextentry.Name = "ttextentry"
        Me.ttextentry.Size = New System.Drawing.Size(432, 321)
        Me.ttextentry.TabIndex = 0
        '
        'Button1
        '
        Me.Button1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button1.Location = New System.Drawing.Point(378, 343)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(66, 21)
        Me.Button1.TabIndex = 5
        Me.Button1.Text = "Cancel"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Bok
        '
        Me.Bok.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Bok.Enabled = False
        Me.Bok.Location = New System.Drawing.Point(307, 343)
        Me.Bok.Name = "Bok"
        Me.Bok.Size = New System.Drawing.Size(66, 21)
        Me.Bok.TabIndex = 4
        Me.Bok.Text = "Ok"
        Me.Bok.UseVisualStyleBackColor = True
        '
        'Llength
        '
        Me.Llength.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Llength.AutoSize = True
        Me.Llength.Location = New System.Drawing.Point(12, 343)
        Me.Llength.Name = "Llength"
        Me.Llength.Size = New System.Drawing.Size(0, 13)
        Me.Llength.TabIndex = 6
        '
        'bc_am_text_edit
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(456, 372)
        Me.Controls.Add(Me.Llength)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Bok)
        Me.Controls.Add(Me.ttextentry)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "bc_am_text_edit"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ttextentry As System.Windows.Forms.TextBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Bok As System.Windows.Forms.Button
    Friend WithEvents Llength As System.Windows.Forms.Label
End Class
