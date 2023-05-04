
Public Class bc_cs_logon_form
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
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents tuser_name As System.Windows.Forms.TextBox
    Friend WithEvents Tpassword As System.Windows.Forms.TextBox
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents uxWorkOffline As System.Windows.Forms.Button
    Friend WithEvents chkremember As System.Windows.Forms.CheckBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_cs_logon_form))
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.uxWorkOffline = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.tuser_name = New System.Windows.Forms.TextBox()
        Me.Tpassword = New System.Windows.Forms.TextBox()
        Me.chkremember = New System.Windows.Forms.CheckBox()
        Me.Panel1.SuspendLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.SystemColors.Control
        Me.Button1.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.Location = New System.Drawing.Point(296, 16)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(72, 24)
        Me.Button1.TabIndex = 4
        Me.Button1.Text = "&Logon"
        Me.Button1.UseVisualStyleBackColor = False
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.White
        Me.Panel1.Controls.Add(Me.uxWorkOffline)
        Me.Panel1.Controls.Add(Me.Button2)
        Me.Panel1.Controls.Add(Me.Button1)
        Me.Panel1.Controls.Add(Me.PictureBox2)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Location = New System.Drawing.Point(-8, 130)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(390, 88)
        Me.Panel1.TabIndex = 17
        '
        'uxWorkOffline
        '
        Me.uxWorkOffline.BackColor = System.Drawing.SystemColors.Control
        Me.uxWorkOffline.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.uxWorkOffline.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxWorkOffline.Location = New System.Drawing.Point(200, 16)
        Me.uxWorkOffline.Name = "uxWorkOffline"
        Me.uxWorkOffline.Size = New System.Drawing.Size(88, 24)
        Me.uxWorkOffline.TabIndex = 5
        Me.uxWorkOffline.TabStop = False
        Me.uxWorkOffline.Text = "&Work Offline"
        Me.uxWorkOffline.UseVisualStyleBackColor = False
        Me.uxWorkOffline.Visible = False
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.SystemColors.Control
        Me.Button2.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button2.Location = New System.Drawing.Point(296, 48)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(72, 24)
        Me.Button2.TabIndex = 5
        Me.Button2.Text = "&Cancel"
        Me.Button2.UseVisualStyleBackColor = False
        '
        'PictureBox2
        '
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(12, 8)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(184, 40)
        Me.PictureBox2.TabIndex = 18
        Me.PictureBox2.TabStop = False
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(12, 56)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(280, 24)
        Me.Label3.TabIndex = 19
        Me.Label3.Text = "© Blue Curve Limited. All rights reserved. "
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(24, 24)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(88, 16)
        Me.Label1.TabIndex = 18
        Me.Label1.Text = "User Name:"
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(24, 56)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(88, 16)
        Me.Label2.TabIndex = 19
        Me.Label2.Text = "Password:"
        '
        'tuser_name
        '
        Me.tuser_name.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tuser_name.Location = New System.Drawing.Point(136, 24)
        Me.tuser_name.Name = "tuser_name"
        Me.tuser_name.Size = New System.Drawing.Size(224, 21)
        Me.tuser_name.TabIndex = 1
        '
        'Tpassword
        '
        Me.Tpassword.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Tpassword.Location = New System.Drawing.Point(136, 56)
        Me.Tpassword.Name = "Tpassword"
        Me.Tpassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.Tpassword.Size = New System.Drawing.Size(224, 21)
        Me.Tpassword.TabIndex = 2
        '
        'chkremember
        '
        Me.chkremember.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkremember.Location = New System.Drawing.Point(136, 88)
        Me.chkremember.Name = "chkremember"
        Me.chkremember.Size = New System.Drawing.Size(184, 24)
        Me.chkremember.TabIndex = 3
        Me.chkremember.Text = "Remember User Name"
        Me.chkremember.Visible = False
        '
        'bc_cs_logon_form
        '
        Me.AcceptButton = Me.Button1
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.CancelButton = Me.Button2
        Me.ClientSize = New System.Drawing.Size(374, 212)
        Me.ControlBox = False
        Me.Controls.Add(Me.chkremember)
        Me.Controls.Add(Me.Tpassword)
        Me.Controls.Add(Me.tuser_name)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "bc_cs_logon_form"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Blue Curve Authentication"
        Me.Panel1.ResumeLayout(False)
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region
    Public cancel As Boolean = True
    Public mode As Integer = 0
    Public offline_selected = False
    Public lastUser As String = ""
    Public remember As Boolean

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        bc_cs_central_settings.user_name = Me.tuser_name.Text
        bc_cs_central_settings.user_password = Me.Tpassword.Text
        Me.cancel = False
        Me.offline_selected = False
        Me.remember = Me.chkremember.Checked
        Me.Hide()
    End Sub

    Private Sub uxWorkOffline_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxWorkOffline.Click

        Me.cancel = False
        Me.offline_selected = True
        Me.Hide()

    End Sub

    Private Sub Panel1_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Panel1.Paint

        'Me.cancel = True
        'If mode = 1 Then
        '   Me.Button2.Text = "&Offline"
        'End If
    End Sub

    Private Sub bc_cs_logon_form_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If mode = 1 Then
            Me.cancel = False
            Me.offline_selected = True
        Else
            Me.cancel = True
            Me.offline_selected = False
        End If

        Me.tuser_name.Text = Me.lastUser
        Me.Tpassword.Text = ""
        bc_cs_central_settings.user_name = ""
        bc_cs_central_settings.user_password = ""

        If Me.lastUser <> "" Then
            Me.chkremember.Checked = True
        End If

    End Sub

    Private Sub bc_cs_logon_form_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated

        If Me.tuser_name.Text <> "" Then
            Me.Tpassword.Focus()
        Else
            Me.tuser_name.Focus()
        End If

    End Sub

    Public Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click

        If mode = 1 Then
            Me.cancel = False
            Me.offline_selected = True
        Else
            Me.cancel = True
            Me.offline_selected = False
        End If

        Me.Hide()
    End Sub


    Private Sub chkremember_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkremember.CheckedChanged

    End Sub


End Class
