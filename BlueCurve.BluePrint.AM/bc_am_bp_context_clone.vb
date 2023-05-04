Public Class bc_am_bp_context_clone
    Inherits System.Windows.Forms.Form

    Private ctrllr As bc_am_clone

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents uxClone As System.Windows.Forms.GroupBox
    Friend WithEvents uxContextName As System.Windows.Forms.TextBox
    Friend WithEvents uxContxtName As System.Windows.Forms.Label
    Friend WithEvents uxCancel As System.Windows.Forms.Button
    Friend WithEvents uxOK As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.uxClone = New System.Windows.Forms.GroupBox
        Me.uxContextName = New System.Windows.Forms.TextBox
        Me.uxCancel = New System.Windows.Forms.Button
        Me.uxOK = New System.Windows.Forms.Button
        Me.uxContxtName = New System.Windows.Forms.Label
        Me.uxClone.SuspendLayout()
        Me.SuspendLayout()
        '
        'uxClone
        '
        Me.uxClone.Controls.Add(Me.uxContextName)
        Me.uxClone.Controls.Add(Me.uxCancel)
        Me.uxClone.Controls.Add(Me.uxOK)
        Me.uxClone.Controls.Add(Me.uxContxtName)
        Me.uxClone.Location = New System.Drawing.Point(3, 0)
        Me.uxClone.Name = "uxClone"
        Me.uxClone.Size = New System.Drawing.Size(348, 76)
        Me.uxClone.TabIndex = 0
        Me.uxClone.TabStop = False
        '
        'uxContextName
        '
        Me.uxContextName.Location = New System.Drawing.Point(143, 17)
        Me.uxContextName.MaxLength = 255
        Me.uxContextName.Name = "uxContextName"
        Me.uxContextName.Size = New System.Drawing.Size(199, 21)
        Me.uxContextName.TabIndex = 1
        '
        'uxCancel
        '
        Me.uxCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.uxCancel.Location = New System.Drawing.Point(267, 44)
        Me.uxCancel.Name = "uxCancel"
        Me.uxCancel.Size = New System.Drawing.Size(75, 23)
        Me.uxCancel.TabIndex = 3
        Me.uxCancel.Text = "Cancel"
        '
        'uxOK
        '
        Me.uxOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.uxOK.Location = New System.Drawing.Point(186, 44)
        Me.uxOK.Name = "uxOK"
        Me.uxOK.Size = New System.Drawing.Size(75, 23)
        Me.uxOK.TabIndex = 2
        Me.uxOK.Text = "OK"
        '
        'uxContxtName
        '
        Me.uxContxtName.Location = New System.Drawing.Point(6, 17)
        Me.uxContxtName.Name = "uxContxtName"
        Me.uxContxtName.Size = New System.Drawing.Size(137, 21)
        Me.uxContxtName.TabIndex = 0
        Me.uxContxtName.Text = "Data Definition Name:"
        Me.uxContxtName.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'bc_am_bp_context_clone
        '
        Me.AcceptButton = Me.uxOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.CancelButton = Me.uxCancel
        Me.ClientSize = New System.Drawing.Size(354, 80)
        Me.ControlBox = False
        Me.Controls.Add(Me.uxClone)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "bc_am_bp_context_clone"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Clone"
        Me.uxClone.ResumeLayout(False)
        Me.uxClone.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Friend WriteOnly Property Controller() As bc_am_clone

        Set(ByVal Value As bc_am_clone)
            ctrllr = Value
        End Set

    End Property

End Class
