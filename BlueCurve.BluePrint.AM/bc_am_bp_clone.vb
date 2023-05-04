Friend Class bc_am_bp_clone
    Inherits System.Windows.Forms.Form

    Private ctrllr As bc_am_template

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
    Friend WithEvents uxCancel As System.Windows.Forms.Button
    Friend WithEvents uxOK As System.Windows.Forms.Button
    Friend WithEvents uxTempName As System.Windows.Forms.Label
    Friend WithEvents uxCreatePhysicalFile As System.Windows.Forms.CheckBox
    Friend WithEvents uxOpenNow As System.Windows.Forms.CheckBox
    Friend WithEvents uxTemplateName As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.uxClone = New System.Windows.Forms.GroupBox
        Me.uxOpenNow = New System.Windows.Forms.CheckBox
        Me.uxTemplateName = New System.Windows.Forms.TextBox
        Me.uxTempName = New System.Windows.Forms.Label
        Me.uxCancel = New System.Windows.Forms.Button
        Me.uxOK = New System.Windows.Forms.Button
        Me.uxCreatePhysicalFile = New System.Windows.Forms.CheckBox
        Me.uxClone.SuspendLayout()
        Me.SuspendLayout()
        '
        'uxClone
        '
        Me.uxClone.Controls.Add(Me.uxOpenNow)
        Me.uxClone.Controls.Add(Me.uxTemplateName)
        Me.uxClone.Controls.Add(Me.uxTempName)
        Me.uxClone.Controls.Add(Me.uxCancel)
        Me.uxClone.Controls.Add(Me.uxOK)
        Me.uxClone.Controls.Add(Me.uxCreatePhysicalFile)
        Me.uxClone.Location = New System.Drawing.Point(4, 0)
        Me.uxClone.Name = "uxClone"
        Me.uxClone.Size = New System.Drawing.Size(360, 112)
        Me.uxClone.TabIndex = 0
        Me.uxClone.TabStop = False
        '
        'uxOpenNow
        '
        Me.uxOpenNow.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.uxOpenNow.Location = New System.Drawing.Point(199, 45)
        Me.uxOpenNow.Name = "uxOpenNow"
        Me.uxOpenNow.Size = New System.Drawing.Size(98, 24)
        Me.uxOpenNow.TabIndex = 2
        Me.uxOpenNow.Text = "Open Now:"
        '
        'uxTemplateName
        '
        Me.uxTemplateName.Location = New System.Drawing.Point(122, 17)
        Me.uxTemplateName.Name = "uxTemplateName"
        Me.uxTemplateName.Size = New System.Drawing.Size(230, 21)
        Me.uxTemplateName.TabIndex = 0
        '
        'uxTempName
        '
        Me.uxTempName.Location = New System.Drawing.Point(12, 17)
        Me.uxTempName.Name = "uxTempName"
        Me.uxTempName.Size = New System.Drawing.Size(104, 21)
        Me.uxTempName.TabIndex = 5
        Me.uxTempName.Text = "Template Name:"
        Me.uxTempName.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'uxCancel
        '
        Me.uxCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.uxCancel.Location = New System.Drawing.Point(277, 80)
        Me.uxCancel.Name = "uxCancel"
        Me.uxCancel.Size = New System.Drawing.Size(75, 23)
        Me.uxCancel.TabIndex = 4
        Me.uxCancel.Text = "Cancel"
        '
        'uxOK
        '
        Me.uxOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.uxOK.Location = New System.Drawing.Point(196, 80)
        Me.uxOK.Name = "uxOK"
        Me.uxOK.Size = New System.Drawing.Size(75, 23)
        Me.uxOK.TabIndex = 3
        Me.uxOK.Text = "OK"
        '
        'uxCreatePhysicalFile
        '
        Me.uxCreatePhysicalFile.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.uxCreatePhysicalFile.Location = New System.Drawing.Point(43, 45)
        Me.uxCreatePhysicalFile.Name = "uxCreatePhysicalFile"
        Me.uxCreatePhysicalFile.Size = New System.Drawing.Size(150, 24)
        Me.uxCreatePhysicalFile.TabIndex = 1
        Me.uxCreatePhysicalFile.Text = "Create Physical File:"
        '
        'bc_am_bp_clone
        '
        Me.AcceptButton = Me.uxOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.CancelButton = Me.uxCancel
        Me.ClientSize = New System.Drawing.Size(368, 116)
        Me.ControlBox = False
        Me.Controls.Add(Me.uxClone)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "bc_am_bp_clone"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Clone"
        Me.uxClone.ResumeLayout(False)
        Me.uxClone.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Friend WriteOnly Property Controller() As bc_am_template

        Set(ByVal Value As bc_am_template)
            ctrllr = Value
        End Set

    End Property

    Private Sub uxTemplateName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxTemplateName.TextChanged

        ctrllr.validate()

    End Sub

    Private Sub uxTemplateName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles uxTemplateName.KeyPress

        If e.KeyChar = "." Or e.KeyChar = "," Or _
            e.KeyChar = "!" Or e.KeyChar = """" Or _
            e.KeyChar = "£" Or e.KeyChar = "$" Or _
            e.KeyChar = "%" Or e.KeyChar = "^" Or _
            e.KeyChar = "&" Or e.KeyChar = "*" Or _
            e.KeyChar = "|" Or e.KeyChar = "?" Or _
            e.KeyChar = "\" Or e.KeyChar = "/" Or _
            e.KeyChar = "<" Or e.KeyChar = ">" Or _
            e.KeyChar = "'" Or e.KeyChar = "@" Or _
            e.KeyChar = "~" Or e.KeyChar = "#" Or _
            e.KeyChar = "{" Or e.KeyChar = "}" Then
            e.Handled = True
        End If

    End Sub
End Class
