Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports BlueCurve.Core.AS
REM ==========================================
REM Bluecurve Limited 2005
REM Module:       Author Tool Generic Form
REM Type:         Application Module
REM Description:  Generic Form design for use 
REM               by inherited Forms
REM Version:      1
REM Change history
REM ==========================================
Public Class bc_am_at_wizard_generic
    Inherits System.Windows.Forms.Form

    REM language dependent strings
    Const form_name = "create.main"
    REM language dependent strings
    Protected form_strings As New bc_om_user_form
    Public Shared mode As Integer
    Protected Shared toggle As Boolean
    Public Shared frm_loaded As Boolean = False
    Public WithEvents LabelCon As System.Windows.Forms.Label
    Public WithEvents LabelConStr As System.Windows.Forms.TextBox
    Protected obc_objects As bc_am_load_objects


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
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents Label2 As System.Windows.Forms.Label
    Public WithEvents PictureBox3 As System.Windows.Forms.PictureBox
    Public WithEvents Label4 As System.Windows.Forms.Label
    Public WithEvents Label5 As System.Windows.Forms.Label
    Public WithEvents Label6 As System.Windows.Forms.Label
    Public WithEvents Label7 As System.Windows.Forms.Label
    Public WithEvents Pubthumbnail As System.Windows.Forms.PictureBox
    Friend WithEvents blogo As System.Windows.Forms.PictureBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_at_wizard_generic))
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.LabelConStr = New System.Windows.Forms.TextBox()
        Me.LabelCon = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.PictureBox3 = New System.Windows.Forms.PictureBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Pubthumbnail = New System.Windows.Forms.PictureBox()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.blogo = New System.Windows.Forms.PictureBox()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Pubthumbnail, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.blogo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(0, 0)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(297, 64)
        Me.PictureBox1.TabIndex = 2
        Me.PictureBox1.TabStop = False
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.LabelConStr)
        Me.Panel1.Controls.Add(Me.LabelCon)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.PictureBox3)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.Pubthumbnail)
        Me.Panel1.Location = New System.Drawing.Point(0, 64)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(552, 224)
        Me.Panel1.TabIndex = 3
        '
        'LabelConStr
        '
        Me.LabelConStr.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LabelConStr.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.LabelConStr.Font = New System.Drawing.Font("Verdana", 6.75!)
        Me.LabelConStr.Location = New System.Drawing.Point(96, 200)
        Me.LabelConStr.Name = "LabelConStr"
        Me.LabelConStr.ReadOnly = True
        Me.LabelConStr.Size = New System.Drawing.Size(255, 11)
        Me.LabelConStr.TabIndex = 17
        Me.LabelConStr.TabStop = False
        Me.LabelConStr.Text = "Connected"
        Me.LabelConStr.WordWrap = False
        '
        'LabelCon
        '
        Me.LabelCon.AutoSize = True
        Me.LabelCon.Font = New System.Drawing.Font("Arial Unicode MS", 6.75!, System.Drawing.FontStyle.Bold)
        Me.LabelCon.Location = New System.Drawing.Point(14, 200)
        Me.LabelCon.Name = "LabelCon"
        Me.LabelCon.Size = New System.Drawing.Size(82, 12)
        Me.LabelCon.TabIndex = 16
        Me.LabelCon.Text = "Connected via:"
        '
        'Label7
        '
        Me.Label7.BackColor = System.Drawing.Color.Transparent
        Me.Label7.Font = New System.Drawing.Font("Arial Unicode MS", 6.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Label7.Location = New System.Drawing.Point(14, 184)
        Me.Label7.Name = "Label7"
        Me.Label7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label7.Size = New System.Drawing.Size(48, 13)
        Me.Label7.TabIndex = 14
        Me.Label7.Text = "Version:"
        '
        'Label6
        '
        Me.Label6.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.SystemColors.WindowFrame
        Me.Label6.Location = New System.Drawing.Point(386, 200)
        Me.Label6.Name = "Label6"
        Me.Label6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label6.Size = New System.Drawing.Size(144, 15)
        Me.Label6.TabIndex = 13
        Me.Label6.Text = "Local"
        '
        'Label5
        '
        Me.Label5.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Font = New System.Drawing.Font("Arial Unicode MS", 6.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.SystemColors.WindowFrame
        Me.Label5.Location = New System.Drawing.Point(354, 200)
        Me.Label5.Name = "Label5"
        Me.Label5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label5.Size = New System.Drawing.Size(32, 15)
        Me.Label5.TabIndex = 12
        Me.Label5.Text = "Role:"
        '
        'PictureBox3
        '
        Me.PictureBox3.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox3.Image = CType(resources.GetObject("PictureBox3.Image"), System.Drawing.Image)
        Me.PictureBox3.Location = New System.Drawing.Point(352, 10)
        Me.PictureBox3.Name = "PictureBox3"
        Me.PictureBox3.Size = New System.Drawing.Size(152, 152)
        Me.PictureBox3.TabIndex = 0
        Me.PictureBox3.TabStop = False
        '
        'Label4
        '
        Me.Label4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Font = New System.Drawing.Font("Arial Unicode MS", 6.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.WindowFrame
        Me.Label4.Location = New System.Drawing.Point(354, 184)
        Me.Label4.Name = "Label4"
        Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label4.Size = New System.Drawing.Size(33, 15)
        Me.Label4.TabIndex = 11
        Me.Label4.Text = "User:"
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.WindowFrame
        Me.Label2.Location = New System.Drawing.Point(386, 184)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(145, 15)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "Local"
        '
        'Label1
        '
        Me.Label1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Label1.Location = New System.Drawing.Point(64, 184)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(147, 15)
        Me.Label1.TabIndex = 10
        Me.Label1.Text = "Version"
        '
        'Pubthumbnail
        '
        Me.Pubthumbnail.BackColor = System.Drawing.Color.Transparent
        Me.Pubthumbnail.Image = CType(resources.GetObject("Pubthumbnail.Image"), System.Drawing.Image)
        Me.Pubthumbnail.Location = New System.Drawing.Point(352, 10)
        Me.Pubthumbnail.Name = "Pubthumbnail"
        Me.Pubthumbnail.Size = New System.Drawing.Size(120, 153)
        Me.Pubthumbnail.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.Pubthumbnail.TabIndex = 15
        Me.Pubthumbnail.TabStop = False
        '
        'PictureBox2
        '
        Me.PictureBox2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(0, 320)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(184, 38)
        Me.PictureBox2.TabIndex = 7
        Me.PictureBox2.TabStop = False
        '
        'blogo
        '
        Me.blogo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.blogo.Image = CType(resources.GetObject("blogo.Image"), System.Drawing.Image)
        Me.blogo.Location = New System.Drawing.Point(476, -3)
        Me.blogo.Name = "blogo"
        Me.blogo.Size = New System.Drawing.Size(69, 67)
        Me.blogo.TabIndex = 9
        Me.blogo.TabStop = False
        '
        'bc_am_at_wizard_generic
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.ClientSize = New System.Drawing.Size(546, 360)
        Me.Controls.Add(Me.blogo)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.PictureBox2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "bc_am_at_wizard_generic"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Blue Curve create - Author Tool"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Pubthumbnail, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.blogo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region
    Private Sub bc_am_at_wizard_generic_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim i As Integer
        For i = 0 To bc_am_load_objects.obc_prefs.language_strings.forms.Count - 1
            If bc_am_load_objects.obc_prefs.language_strings.forms(i).form_name = form_name Then
                form_strings = bc_am_load_objects.obc_prefs.language_strings.forms(i)
                Exit For
            End If
        Next


        Me.Label1.Text = bc_cs_central_settings.version
        Me.LabelConStr.Text = CStr(bc_cs_central_settings.selected_conn_method)

        If bc_cs_central_settings.selected_conn_method = "ado" Then
            Me.LabelConStr.Text = Me.LabelConStr.Text + "- " + bc_cs_central_settings.servername
        End If
        If bc_cs_central_settings.selected_conn_method = "soap" Then
            Me.LabelConStr.Text = Me.LabelConStr.Text + "- " + bc_cs_central_settings.soap_server
        End If
        For i = 0 To bc_am_load_objects.obc_users.user.Count - 1
            With bc_am_load_objects.obc_users.user(i)
                If .id = bc_cs_central_settings.logged_on_user_id Then
                    Me.Label2.Text = .first_name + " " + .surname
                    Me.Label6.Text = .role
                    Exit For
                End If
            End With
        Next
        Me.Label4.Text = "Logged in as:"
        Me.Label5.Text = "Role:"
        Me.Label7.Text = "Version:"
        For i = 0 To form_strings.form_labels.Count - 1
            If form_strings.form_labels(i).label_code = "create.main.log" Then
                Me.Label4.Text = form_strings.form_labels(i).text
            End If
            If form_strings.form_labels(i).label_code = "create.main.role" Then
                Me.Label5.Text = form_strings.form_labels(i).text
            End If
            If form_strings.form_labels(i).label_code = "create.main.version" Then
                Me.Label7.Text = form_strings.form_labels(i).text
            End If
        Next

    End Sub

    Overridable Sub show_buttons()

    End Sub

    Private Sub Panel1_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Panel1.MouseMove
        show_buttons()
    End Sub

    Private Sub bc_am_at_wizard_generic_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        bc_cs_central_settings.create_client_poll_enabled = False
    End Sub

End Class

