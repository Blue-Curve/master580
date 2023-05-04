Public Class bc_am_bp_register_table
    Inherits System.Windows.Forms.Form

    Private ctrllr As bc_am_blueprint

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
    Friend WithEvents uxFooterPanel As System.Windows.Forms.Panel
    Public WithEvents uxServer As System.Windows.Forms.Label
    Public WithEvents uxRole As System.Windows.Forms.Label
    Public WithEvents uxUser As System.Windows.Forms.Label
    Friend WithEvents uxFooterLogo As System.Windows.Forms.PictureBox
    Friend WithEvents uxDetails As System.Windows.Forms.GroupBox
    Friend WithEvents uxTblName As System.Windows.Forms.Label
    Friend WithEvents uxTableName As System.Windows.Forms.TextBox
    Friend WithEvents uxCancel As System.Windows.Forms.Button
    Friend WithEvents uxOK As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_bp_register_table))
        Me.uxFooterPanel = New System.Windows.Forms.Panel()
        Me.uxServer = New System.Windows.Forms.Label()
        Me.uxRole = New System.Windows.Forms.Label()
        Me.uxUser = New System.Windows.Forms.Label()
        Me.uxFooterLogo = New System.Windows.Forms.PictureBox()
        Me.uxDetails = New System.Windows.Forms.GroupBox()
        Me.uxTblName = New System.Windows.Forms.Label()
        Me.uxTableName = New System.Windows.Forms.TextBox()
        Me.uxCancel = New System.Windows.Forms.Button()
        Me.uxOK = New System.Windows.Forms.Button()
        Me.uxFooterPanel.SuspendLayout()
        CType(Me.uxFooterLogo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.uxDetails.SuspendLayout()
        Me.SuspendLayout()
        '
        'uxFooterPanel
        '
        Me.uxFooterPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.uxFooterPanel.Controls.Add(Me.uxServer)
        Me.uxFooterPanel.Controls.Add(Me.uxRole)
        Me.uxFooterPanel.Controls.Add(Me.uxUser)
        Me.uxFooterPanel.Controls.Add(Me.uxFooterLogo)
        Me.uxFooterPanel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.uxFooterPanel.Location = New System.Drawing.Point(0, 76)
        Me.uxFooterPanel.Name = "uxFooterPanel"
        Me.uxFooterPanel.Size = New System.Drawing.Size(494, 60)
        Me.uxFooterPanel.TabIndex = 1
        '
        'uxServer
        '
        Me.uxServer.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxServer.Location = New System.Drawing.Point(244, 22)
        Me.uxServer.Name = "uxServer"
        Me.uxServer.Size = New System.Drawing.Size(244, 32)
        Me.uxServer.TabIndex = 3
        Me.uxServer.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'uxRole
        '
        Me.uxRole.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxRole.Location = New System.Drawing.Point(368, 2)
        Me.uxRole.Name = "uxRole"
        Me.uxRole.Size = New System.Drawing.Size(120, 16)
        Me.uxRole.TabIndex = 2
        Me.uxRole.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'uxUser
        '
        Me.uxUser.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxUser.Location = New System.Drawing.Point(244, 2)
        Me.uxUser.Name = "uxUser"
        Me.uxUser.Size = New System.Drawing.Size(120, 16)
        Me.uxUser.TabIndex = 1
        Me.uxUser.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'uxFooterLogo
        '
        Me.uxFooterLogo.BackColor = System.Drawing.Color.White
        Me.uxFooterLogo.Image = CType(resources.GetObject("uxFooterLogo.Image"), System.Drawing.Image)
        Me.uxFooterLogo.Location = New System.Drawing.Point(5, 0)
        Me.uxFooterLogo.Name = "uxFooterLogo"
        Me.uxFooterLogo.Size = New System.Drawing.Size(236, 36)
        Me.uxFooterLogo.TabIndex = 39
        Me.uxFooterLogo.TabStop = False
        '
        'uxDetails
        '
        Me.uxDetails.Controls.Add(Me.uxTblName)
        Me.uxDetails.Controls.Add(Me.uxTableName)
        Me.uxDetails.Controls.Add(Me.uxCancel)
        Me.uxDetails.Controls.Add(Me.uxOK)
        Me.uxDetails.Location = New System.Drawing.Point(4, -3)
        Me.uxDetails.Name = "uxDetails"
        Me.uxDetails.Size = New System.Drawing.Size(488, 75)
        Me.uxDetails.TabIndex = 0
        Me.uxDetails.TabStop = False
        '
        'uxTblName
        '
        Me.uxTblName.Location = New System.Drawing.Point(8, 15)
        Me.uxTblName.Name = "uxTblName"
        Me.uxTblName.Size = New System.Drawing.Size(80, 23)
        Me.uxTblName.TabIndex = 0
        Me.uxTblName.Text = "Table Name:"
        '
        'uxTableName
        '
        Me.uxTableName.Location = New System.Drawing.Point(92, 12)
        Me.uxTableName.MaxLength = 32
        Me.uxTableName.Name = "uxTableName"
        Me.uxTableName.Size = New System.Drawing.Size(392, 21)
        Me.uxTableName.TabIndex = 1
        '
        'uxCancel
        '
        Me.uxCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.uxCancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxCancel.Location = New System.Drawing.Point(408, 44)
        Me.uxCancel.Name = "uxCancel"
        Me.uxCancel.Size = New System.Drawing.Size(75, 23)
        Me.uxCancel.TabIndex = 3
        Me.uxCancel.Text = "Cancel"
        '
        'uxOK
        '
        Me.uxOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.uxOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxOK.Location = New System.Drawing.Point(328, 44)
        Me.uxOK.Name = "uxOK"
        Me.uxOK.Size = New System.Drawing.Size(75, 23)
        Me.uxOK.TabIndex = 2
        Me.uxOK.Text = "OK"
        '
        'bc_am_bp_register_table
        '
        Me.AcceptButton = Me.uxOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.Color.White
        Me.CancelButton = Me.uxCancel
        Me.ClientSize = New System.Drawing.Size(494, 136)
        Me.Controls.Add(Me.uxDetails)
        Me.Controls.Add(Me.uxFooterPanel)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "bc_am_bp_register_table"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Register New Table"
        Me.uxFooterPanel.ResumeLayout(False)
        CType(Me.uxFooterLogo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.uxDetails.ResumeLayout(False)
        Me.uxDetails.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Friend WriteOnly Property Controller() As bc_am_blueprint

        Set(ByVal Value As bc_am_blueprint)
            ctrllr = Value
        End Set

    End Property

    Private Sub uxTableName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles uxTableName.KeyPress

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
                    e.KeyChar = "{" Or e.KeyChar = "}" Or _
                    e.KeyChar = " " Then
            e.Handled = True
        End If

    End Sub

    Private Sub uxTableName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles uxTableName.TextChanged

        If Trim(uxTableName.Text) = "" OrElse uxTableName.Text.StartsWith("0") OrElse _
            uxTableName.Text.StartsWith("1") OrElse uxTableName.Text.StartsWith("2") OrElse _
            uxTableName.Text.StartsWith("3") OrElse uxTableName.Text.StartsWith("4") OrElse _
            uxTableName.Text.StartsWith("5") OrElse uxTableName.Text.StartsWith("6") OrElse _
            uxTableName.Text.StartsWith("7") OrElse uxTableName.Text.StartsWith("8") OrElse _
            uxTableName.Text.StartsWith("9") OrElse uxTableName.Text.StartsWith("_") Then
            uxOK.Enabled = False
        Else
            uxOK.Enabled = True
        End If

    End Sub

    Private Sub uxRole_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxRole.Click

    End Sub
End Class
