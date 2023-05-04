Public Class bc_am_bp_add_label
    Inherits System.Windows.Forms.Form

    Private ctrllr As bc_am_context

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
    Friend WithEvents uxLabelDetails As System.Windows.Forms.GroupBox
    Friend WithEvents uxLabelCde As System.Windows.Forms.Label
    Friend WithEvents uxLabelDesc As System.Windows.Forms.Label
    Friend WithEvents uxLabelVal As System.Windows.Forms.Label
    Friend WithEvents uxLabelCode As System.Windows.Forms.TextBox
    Friend WithEvents uxLabelDescription As System.Windows.Forms.TextBox
    Friend WithEvents uxLabelValue As System.Windows.Forms.TextBox
    Friend WithEvents uxCancel As System.Windows.Forms.Button
    Friend WithEvents uxOK As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_bp_add_label))
        Me.uxLabelDetails = New System.Windows.Forms.GroupBox
        Me.uxOK = New System.Windows.Forms.Button
        Me.uxCancel = New System.Windows.Forms.Button
        Me.uxLabelValue = New System.Windows.Forms.TextBox
        Me.uxLabelDescription = New System.Windows.Forms.TextBox
        Me.uxLabelCode = New System.Windows.Forms.TextBox
        Me.uxLabelVal = New System.Windows.Forms.Label
        Me.uxLabelDesc = New System.Windows.Forms.Label
        Me.uxLabelCde = New System.Windows.Forms.Label
        Me.uxLabelDetails.SuspendLayout()
        Me.SuspendLayout()
        '
        'uxLabelDetails
        '
        Me.uxLabelDetails.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.uxLabelDetails.Controls.Add(Me.uxOK)
        Me.uxLabelDetails.Controls.Add(Me.uxCancel)
        Me.uxLabelDetails.Controls.Add(Me.uxLabelValue)
        Me.uxLabelDetails.Controls.Add(Me.uxLabelDescription)
        Me.uxLabelDetails.Controls.Add(Me.uxLabelCode)
        Me.uxLabelDetails.Controls.Add(Me.uxLabelVal)
        Me.uxLabelDetails.Controls.Add(Me.uxLabelDesc)
        Me.uxLabelDetails.Controls.Add(Me.uxLabelCde)
        Me.uxLabelDetails.Location = New System.Drawing.Point(4, 0)
        Me.uxLabelDetails.Name = "uxLabelDetails"
        Me.uxLabelDetails.Size = New System.Drawing.Size(368, 136)
        Me.uxLabelDetails.TabIndex = 0
        Me.uxLabelDetails.TabStop = False
        '
        'uxOK
        '
        Me.uxOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.uxOK.Location = New System.Drawing.Point(204, 103)
        Me.uxOK.Name = "uxOK"
        Me.uxOK.Size = New System.Drawing.Size(75, 23)
        Me.uxOK.TabIndex = 6
        Me.uxOK.Text = "OK"
        '
        'uxCancel
        '
        Me.uxCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.uxCancel.Location = New System.Drawing.Point(285, 103)
        Me.uxCancel.Name = "uxCancel"
        Me.uxCancel.Size = New System.Drawing.Size(75, 23)
        Me.uxCancel.TabIndex = 7
        Me.uxCancel.Text = "Cancel"
        '
        'uxLabelValue
        '
        Me.uxLabelValue.Location = New System.Drawing.Point(86, 76)
        Me.uxLabelValue.MaxLength = 8000
        Me.uxLabelValue.Name = "uxLabelValue"
        Me.uxLabelValue.Size = New System.Drawing.Size(274, 21)
        Me.uxLabelValue.TabIndex = 5
        '
        'uxLabelDescription
        '
        Me.uxLabelDescription.Location = New System.Drawing.Point(86, 48)
        Me.uxLabelDescription.MaxLength = 255
        Me.uxLabelDescription.Name = "uxLabelDescription"
        Me.uxLabelDescription.Size = New System.Drawing.Size(274, 21)
        Me.uxLabelDescription.TabIndex = 4
        '
        'uxLabelCode
        '
        Me.uxLabelCode.Location = New System.Drawing.Point(86, 20)
        Me.uxLabelCode.MaxLength = 64
        Me.uxLabelCode.Name = "uxLabelCode"
        Me.uxLabelCode.Size = New System.Drawing.Size(274, 21)
        Me.uxLabelCode.TabIndex = 3
        '
        'uxLabelVal
        '
        Me.uxLabelVal.Location = New System.Drawing.Point(10, 76)
        Me.uxLabelVal.Name = "uxLabelVal"
        Me.uxLabelVal.Size = New System.Drawing.Size(72, 21)
        Me.uxLabelVal.TabIndex = 2
        Me.uxLabelVal.Text = "Value:"
        Me.uxLabelVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'uxLabelDesc
        '
        Me.uxLabelDesc.Location = New System.Drawing.Point(6, 48)
        Me.uxLabelDesc.Name = "uxLabelDesc"
        Me.uxLabelDesc.Size = New System.Drawing.Size(76, 21)
        Me.uxLabelDesc.TabIndex = 1
        Me.uxLabelDesc.Text = "Description:"
        Me.uxLabelDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'uxLabelCde
        '
        Me.uxLabelCde.Location = New System.Drawing.Point(10, 20)
        Me.uxLabelCde.Name = "uxLabelCde"
        Me.uxLabelCde.Size = New System.Drawing.Size(72, 21)
        Me.uxLabelCde.TabIndex = 0
        Me.uxLabelCde.Text = "Code:"
        Me.uxLabelCde.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'bc_am_bp_add_label
        '
        Me.AcceptButton = Me.uxOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.CancelButton = Me.uxCancel
        Me.ClientSize = New System.Drawing.Size(374, 140)
        Me.ControlBox = False
        Me.Controls.Add(Me.uxLabelDetails)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "bc_am_bp_add_label"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Add Label"
        Me.uxLabelDetails.ResumeLayout(False)
        Me.uxLabelDetails.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Friend WriteOnly Property Controller() As bc_am_context

        Set(ByVal Value As bc_am_context)
            ctrllr = Value
        End Set

    End Property

    Private Sub uxLabelCode_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles uxLabelCode.KeyPress

        If e.KeyChar = "," Or _
            e.KeyChar = "!" Or e.KeyChar = """" Or _
            e.KeyChar = "£" Or e.KeyChar = "$" Or _
            e.KeyChar = "%" Or e.KeyChar = "^" Or _
            e.KeyChar = "&" Or e.KeyChar = "*" Or _
            e.KeyChar = "|" Or e.KeyChar = "?" Or _
            e.KeyChar = "\" Or e.KeyChar = "/" Or _
            e.KeyChar = "<" Or e.KeyChar = ">" Or _
            e.KeyChar = "'" Or e.KeyChar = "@" Or _
            e.KeyChar = "~" Or e.KeyChar = "#" Or _
            e.KeyChar = "{" Or e.KeyChar = "}" Or _
            e.KeyChar = " " Then
            e.Handled = True
        End If

    End Sub

    Private Sub uxLabelCode_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxLabelCode.TextChanged

        ctrllr.ValidateLabel()

    End Sub

    Private Sub uxLabelDescription_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxLabelDescription.TextChanged

        ctrllr.ValidateLabel()

    End Sub

    Private Sub uxLabelValue_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxLabelValue.TextChanged

        ctrllr.ValidateLabel()

    End Sub
End Class
