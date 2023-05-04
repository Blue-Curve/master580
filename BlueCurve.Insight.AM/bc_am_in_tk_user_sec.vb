Public Class bc_am_in_tk_user_sec
    Inherits System.Windows.Forms.Form

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
    Friend WithEvents Chkdraft As System.Windows.Forms.CheckBox
    Friend WithEvents chkpublish As System.Windows.Forms.CheckBox
    Friend WithEvents rdraft As System.Windows.Forms.RadioButton
    Friend WithEvents rpublish As System.Windows.Forms.RadioButton
    Friend WithEvents Ltitle As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents bapply As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_in_tk_user_sec))
        Me.Ltitle = New System.Windows.Forms.Label
        Me.Chkdraft = New System.Windows.Forms.CheckBox
        Me.chkpublish = New System.Windows.Forms.CheckBox
        Me.rdraft = New System.Windows.Forms.RadioButton
        Me.rpublish = New System.Windows.Forms.RadioButton
        Me.Button1 = New System.Windows.Forms.Button
        Me.bapply = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'Ltitle
        '
        Me.Ltitle.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Ltitle.Location = New System.Drawing.Point(8, 8)
        Me.Ltitle.Name = "Ltitle"
        Me.Ltitle.Size = New System.Drawing.Size(256, 40)
        Me.Ltitle.TabIndex = 37
        Me.Ltitle.Text = "Select allowed data submission for role"
        '
        'Chkdraft
        '
        Me.Chkdraft.Checked = True
        Me.Chkdraft.CheckState = System.Windows.Forms.CheckState.Checked
        Me.Chkdraft.Location = New System.Drawing.Point(24, 53)
        Me.Chkdraft.Name = "Chkdraft"
        Me.Chkdraft.Size = New System.Drawing.Size(104, 16)
        Me.Chkdraft.TabIndex = 1
        Me.Chkdraft.Text = "Draft"
        '
        'chkpublish
        '
        Me.chkpublish.Location = New System.Drawing.Point(24, 80)
        Me.chkpublish.Name = "chkpublish"
        Me.chkpublish.Size = New System.Drawing.Size(104, 16)
        Me.chkpublish.TabIndex = 3
        Me.chkpublish.Text = "Publish"
        '
        'rdraft
        '
        Me.rdraft.Checked = True
        Me.rdraft.Location = New System.Drawing.Point(104, 53)
        Me.rdraft.Name = "rdraft"
        Me.rdraft.Size = New System.Drawing.Size(128, 16)
        Me.rdraft.TabIndex = 2
        Me.rdraft.TabStop = True
        Me.rdraft.Text = "Draft as default"
        '
        'rpublish
        '
        Me.rpublish.Location = New System.Drawing.Point(104, 80)
        Me.rpublish.Name = "rpublish"
        Me.rpublish.Size = New System.Drawing.Size(136, 16)
        Me.rpublish.TabIndex = 4
        Me.rpublish.Text = "Publish  as default"
        '
        'Button1
        '
        Me.Button1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.Location = New System.Drawing.Point(192, 112)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 6
        Me.Button1.Text = "Cancel"
        '
        'bapply
        '
        Me.bapply.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.bapply.Enabled = False
        Me.bapply.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bapply.Location = New System.Drawing.Point(112, 112)
        Me.bapply.Name = "bapply"
        Me.bapply.Size = New System.Drawing.Size(75, 23)
        Me.bapply.TabIndex = 5
        Me.bapply.Text = "Ok"
        '
        'bc_am_in_tk_user_sec
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(280, 140)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.bapply)
        Me.Controls.Add(Me.rpublish)
        Me.Controls.Add(Me.rdraft)
        Me.Controls.Add(Me.chkpublish)
        Me.Controls.Add(Me.Chkdraft)
        Me.Controls.Add(Me.Ltitle)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "bc_am_in_tk_user_sec"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Properies for Role Submission"
        Me.ResumeLayout(False)

    End Sub

#End Region
    Public cancel_selected As Boolean = True
    Private Sub bc_am_in_tk_user_sec_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub bcancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Hide()
    End Sub

    Private Sub bapply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.cancel_selected = False
        Me.Hide()
    End Sub

    Private Sub Chkdraft_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chkdraft.CheckedChanged
        Me.bapply.Enabled = True
        Me.rdraft.Enabled = False
        Me.rdraft.Checked = False
        If Me.Chkdraft.Checked = True Then
            Me.rdraft.Enabled = True
            If Me.chkpublish.Checked = False Then
                Me.rdraft.Checked = True
            End If
            If Me.chkpublish.Checked = False Then
                Me.rpublish.Checked = False
            End If
        Else
            If Me.chkpublish.Checked = True Then
                Me.rpublish.Checked = True
            Else
                Me.bapply.Enabled = False
            End If
        End If
    End Sub
    Private Sub Chkpublish_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkpublish.CheckedChanged
        Me.bapply.Enabled = True
        Me.rpublish.Enabled = False
        Me.rpublish.Checked = False
        If Me.chkpublish.Checked = True Then
            Me.rpublish.Enabled = True
            If Me.Chkdraft.Checked = False Then
                Me.rpublish.Checked = True
            End If
            If Me.chkpublish.Checked = False Then
                Me.rdraft.Checked = False
            End If
        Else
            If Me.Chkdraft.Checked = True Then
                Me.rdraft.Checked = True
            Else
                Me.bapply.Enabled = False
            End If
        End If
    End Sub

    Private Sub rdraft_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdraft.CheckedChanged
        Me.bapply.Enabled = True
    End Sub

    Private Sub rpublish_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rpublish.CheckedChanged
        Me.bapply.Enabled = True
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Hide()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bapply.Click
        Me.cancel_selected = False
        Me.Hide()
    End Sub
End Class
