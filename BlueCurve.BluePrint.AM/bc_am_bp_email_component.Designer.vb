<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bc_am_bp_email_component
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
        Me.uxComponent = New System.Windows.Forms.GroupBox()
        Me.uxStoredProcName = New System.Windows.Forms.TextBox()
        Me.uxDescription = New System.Windows.Forms.TextBox()
        Me.uxSPName = New System.Windows.Forms.Label()
        Me.uxDesc = New System.Windows.Forms.Label()
        Me.uxOK = New System.Windows.Forms.Button()
        Me.uxCancel = New System.Windows.Forms.Button()
        Me.uxComponent.SuspendLayout()
        Me.SuspendLayout()
        '
        'uxComponent
        '
        Me.uxComponent.Controls.Add(Me.uxStoredProcName)
        Me.uxComponent.Controls.Add(Me.uxDescription)
        Me.uxComponent.Controls.Add(Me.uxSPName)
        Me.uxComponent.Controls.Add(Me.uxDesc)
        Me.uxComponent.Controls.Add(Me.uxOK)
        Me.uxComponent.Controls.Add(Me.uxCancel)
        Me.uxComponent.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxComponent.Location = New System.Drawing.Point(2, 3)
        Me.uxComponent.Name = "uxComponent"
        Me.uxComponent.Size = New System.Drawing.Size(452, 170)
        Me.uxComponent.TabIndex = 1
        Me.uxComponent.TabStop = False
        Me.uxComponent.Text = "Details"
        '
        'uxStoredProcName
        '
        Me.uxStoredProcName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxStoredProcName.Location = New System.Drawing.Point(156, 88)
        Me.uxStoredProcName.Name = "uxStoredProcName"
        Me.uxStoredProcName.Size = New System.Drawing.Size(287, 21)
        Me.uxStoredProcName.TabIndex = 3
        '
        'uxDescription
        '
        Me.uxDescription.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxDescription.Location = New System.Drawing.Point(155, 26)
        Me.uxDescription.Multiline = True
        Me.uxDescription.Name = "uxDescription"
        Me.uxDescription.Size = New System.Drawing.Size(288, 44)
        Me.uxDescription.TabIndex = 2
        '
        'uxSPName
        '
        Me.uxSPName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxSPName.Location = New System.Drawing.Point(45, 88)
        Me.uxSPName.Name = "uxSPName"
        Me.uxSPName.Size = New System.Drawing.Size(104, 20)
        Me.uxSPName.TabIndex = 10
        Me.uxSPName.Text = "Procedure Name:"
        Me.uxSPName.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'uxDesc
        '
        Me.uxDesc.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxDesc.Location = New System.Drawing.Point(73, 26)
        Me.uxDesc.Name = "uxDesc"
        Me.uxDesc.Size = New System.Drawing.Size(76, 23)
        Me.uxDesc.TabIndex = 9
        Me.uxDesc.Text = "Description:"
        Me.uxDesc.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'uxOK
        '
        Me.uxOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.uxOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxOK.Location = New System.Drawing.Point(282, 131)
        Me.uxOK.Name = "uxOK"
        Me.uxOK.Size = New System.Drawing.Size(75, 23)
        Me.uxOK.TabIndex = 11
        Me.uxOK.Text = "OK"
        '
        'uxCancel
        '
        Me.uxCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.uxCancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxCancel.Location = New System.Drawing.Point(363, 131)
        Me.uxCancel.Name = "uxCancel"
        Me.uxCancel.Size = New System.Drawing.Size(75, 23)
        Me.uxCancel.TabIndex = 12
        Me.uxCancel.Text = "Cancel"
        '
        'bc_am_bp_email_component
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(460, 174)
        Me.Controls.Add(Me.uxComponent)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "bc_am_bp_email_component"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Email Component"
        Me.uxComponent.ResumeLayout(False)
        Me.uxComponent.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents uxComponent As System.Windows.Forms.GroupBox
    Friend WithEvents uxStoredProcName As System.Windows.Forms.TextBox
    Friend WithEvents uxDescription As System.Windows.Forms.TextBox
    Friend WithEvents uxSPName As System.Windows.Forms.Label
    Friend WithEvents uxDesc As System.Windows.Forms.Label
    Friend WithEvents uxOK As System.Windows.Forms.Button
    Friend WithEvents uxCancel As System.Windows.Forms.Button
End Class
