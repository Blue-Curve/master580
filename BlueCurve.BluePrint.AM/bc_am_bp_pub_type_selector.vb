Friend Class bc_am_bp_pub_type_selector
    Inherits System.Windows.Forms.Form

    Private ctrllr As bc_am_composite

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

        uxOK.Enabled = False

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
    Friend WithEvents uxCompositeSelector As System.Windows.Forms.GroupBox
    Friend WithEvents uxPubType As System.Windows.Forms.ComboBox
    Friend WithEvents uxOK As System.Windows.Forms.Button
    Friend WithEvents uxCancel As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.uxCompositeSelector = New System.Windows.Forms.GroupBox
        Me.uxCancel = New System.Windows.Forms.Button
        Me.uxPubType = New System.Windows.Forms.ComboBox
        Me.uxOK = New System.Windows.Forms.Button
        Me.uxCompositeSelector.SuspendLayout()
        Me.SuspendLayout()
        '
        'uxCompositeSelector
        '
        Me.uxCompositeSelector.Controls.Add(Me.uxCancel)
        Me.uxCompositeSelector.Controls.Add(Me.uxPubType)
        Me.uxCompositeSelector.Controls.Add(Me.uxOK)
        Me.uxCompositeSelector.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxCompositeSelector.Location = New System.Drawing.Point(7, 0)
        Me.uxCompositeSelector.Name = "uxCompositeSelector"
        Me.uxCompositeSelector.Size = New System.Drawing.Size(260, 76)
        Me.uxCompositeSelector.TabIndex = 0
        Me.uxCompositeSelector.TabStop = False
        '
        'uxCancel
        '
        Me.uxCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.uxCancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxCancel.Location = New System.Drawing.Point(177, 43)
        Me.uxCancel.Name = "uxCancel"
        Me.uxCancel.Size = New System.Drawing.Size(75, 23)
        Me.uxCancel.TabIndex = 2
        Me.uxCancel.Text = "Cancel"
        '
        'uxPubType
        '
        Me.uxPubType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.uxPubType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxPubType.Location = New System.Drawing.Point(8, 13)
        Me.uxPubType.Name = "uxPubType"
        Me.uxPubType.Size = New System.Drawing.Size(244, 21)
        Me.uxPubType.TabIndex = 1
        '
        'uxOK
        '
        Me.uxOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.uxOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxOK.Location = New System.Drawing.Point(96, 43)
        Me.uxOK.Name = "uxOK"
        Me.uxOK.Size = New System.Drawing.Size(75, 23)
        Me.uxOK.TabIndex = 1
        Me.uxOK.Text = "OK"
        '
        'bc_am_bp_pub_type_selector
        '
        Me.AcceptButton = Me.uxOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.CancelButton = Me.uxCancel
        Me.ClientSize = New System.Drawing.Size(274, 80)
        Me.ControlBox = False
        Me.Controls.Add(Me.uxCompositeSelector)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "bc_am_bp_pub_type_selector"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Select Publication Type"
        Me.uxCompositeSelector.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Friend WriteOnly Property Controller() As bc_am_composite

        Set(ByVal Value As bc_am_composite)
            ctrllr = Value
        End Set

    End Property

    Private Sub uxPubType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxPubType.SelectedIndexChanged

        If Trim(uxPubType.Text) <> "" Then
            uxOK.Enabled = True
        Else
            uxOK.Enabled = False
        End If

    End Sub

End Class
