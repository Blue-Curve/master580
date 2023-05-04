Public Class bc_am_in_tk_da_frm
    Inherits System.Windows.Forms.Form
    Public cancel_selected As Boolean = True
    Public fcontext As Object
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
    Friend WithEvents Ltitle As System.Windows.Forms.Label
    Friend WithEvents tinput As System.Windows.Forms.TextBox
    Friend WithEvents uxToolBarImages As System.Windows.Forms.ImageList
    Friend WithEvents clist As System.Windows.Forms.ComboBox
    Friend WithEvents rsecnew As System.Windows.Forms.RadioButton
    Friend WithEvents rsecexist As System.Windows.Forms.RadioButton
    Friend WithEvents Cstatic As System.Windows.Forms.CheckBox
    Friend WithEvents Clist2 As System.Windows.Forms.ComboBox
    Friend WithEvents L1 As System.Windows.Forms.Label
    Friend WithEvents L2 As System.Windows.Forms.Label
    Friend WithEvents bcancel As System.Windows.Forms.Button
    Friend WithEvents Bok As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(bc_am_in_tk_da_frm))
        Me.Ltitle = New System.Windows.Forms.Label
        Me.tinput = New System.Windows.Forms.TextBox
        Me.uxToolBarImages = New System.Windows.Forms.ImageList(Me.components)
        Me.clist = New System.Windows.Forms.ComboBox
        Me.rsecnew = New System.Windows.Forms.RadioButton
        Me.rsecexist = New System.Windows.Forms.RadioButton
        Me.Cstatic = New System.Windows.Forms.CheckBox
        Me.Clist2 = New System.Windows.Forms.ComboBox
        Me.L1 = New System.Windows.Forms.Label
        Me.L2 = New System.Windows.Forms.Label
        Me.bcancel = New System.Windows.Forms.Button
        Me.Bok = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'Ltitle
        '
        Me.Ltitle.Location = New System.Drawing.Point(0, 6)
        Me.Ltitle.Name = "Ltitle"
        Me.Ltitle.Size = New System.Drawing.Size(256, 16)
        Me.Ltitle.TabIndex = 2
        Me.Ltitle.Text = "cc"
        '
        'tinput
        '
        Me.tinput.Location = New System.Drawing.Point(56, 24)
        Me.tinput.MaxLength = 250
        Me.tinput.Name = "tinput"
        Me.tinput.Size = New System.Drawing.Size(208, 21)
        Me.tinput.TabIndex = 3
        Me.tinput.Text = ""
        Me.tinput.Visible = False
        '
        'uxToolBarImages
        '
        Me.uxToolBarImages.ImageSize = New System.Drawing.Size(16, 16)
        Me.uxToolBarImages.ImageStream = CType(resources.GetObject("uxToolBarImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.uxToolBarImages.TransparentColor = System.Drawing.Color.Transparent
        '
        'clist
        '
        Me.clist.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.clist.Location = New System.Drawing.Point(56, 24)
        Me.clist.MaxLength = 250
        Me.clist.Name = "clist"
        Me.clist.Size = New System.Drawing.Size(208, 21)
        Me.clist.TabIndex = 6
        Me.clist.Visible = False
        '
        'rsecnew
        '
        Me.rsecnew.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.rsecnew.Location = New System.Drawing.Point(56, 72)
        Me.rsecnew.Name = "rsecnew"
        Me.rsecnew.Size = New System.Drawing.Size(48, 32)
        Me.rsecnew.TabIndex = 7
        Me.rsecnew.Text = "New"
        Me.rsecnew.Visible = False
        '
        'rsecexist
        '
        Me.rsecexist.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.rsecexist.Location = New System.Drawing.Point(104, 72)
        Me.rsecexist.Name = "rsecexist"
        Me.rsecexist.Size = New System.Drawing.Size(80, 32)
        Me.rsecexist.TabIndex = 8
        Me.rsecexist.Text = "Existing"
        Me.rsecexist.Visible = False
        '
        'Cstatic
        '
        Me.Cstatic.Location = New System.Drawing.Point(1, 25)
        Me.Cstatic.Name = "Cstatic"
        Me.Cstatic.Size = New System.Drawing.Size(63, 24)
        Me.Cstatic.TabIndex = 9
        Me.Cstatic.Text = "Static"
        Me.Cstatic.Visible = False
        '
        'Clist2
        '
        Me.Clist2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Clist2.Location = New System.Drawing.Point(56, 53)
        Me.Clist2.MaxLength = 250
        Me.Clist2.Name = "Clist2"
        Me.Clist2.Size = New System.Drawing.Size(208, 21)
        Me.Clist2.TabIndex = 10
        Me.Clist2.Visible = False
        '
        'L1
        '
        Me.L1.Location = New System.Drawing.Point(0, 32)
        Me.L1.Name = "L1"
        Me.L1.Size = New System.Drawing.Size(64, 16)
        Me.L1.TabIndex = 11
        Me.L1.Text = "Label1"
        Me.L1.Visible = False
        '
        'L2
        '
        Me.L2.Location = New System.Drawing.Point(0, 61)
        Me.L2.Name = "L2"
        Me.L2.Size = New System.Drawing.Size(56, 16)
        Me.L2.TabIndex = 12
        Me.L2.Text = "Label2"
        Me.L2.Visible = False
        '
        'bcancel
        '
        Me.bcancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.bcancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bcancel.Location = New System.Drawing.Point(188, 112)
        Me.bcancel.Name = "bcancel"
        Me.bcancel.TabIndex = 87
        Me.bcancel.Text = "Cancel"
        '
        'Bok
        '
        Me.Bok.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Bok.Enabled = False
        Me.Bok.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Bok.Location = New System.Drawing.Point(108, 112)
        Me.Bok.Name = "Bok"
        Me.Bok.TabIndex = 86
        Me.Bok.Text = "Ok"
        '
        'bc_am_in_tk_da_frm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.Color.FromArgb(CType(235, Byte), CType(240, Byte), CType(249, Byte))
        Me.ClientSize = New System.Drawing.Size(274, 140)
        Me.Controls.Add(Me.bcancel)
        Me.Controls.Add(Me.Bok)
        Me.Controls.Add(Me.clist)
        Me.Controls.Add(Me.tinput)
        Me.Controls.Add(Me.L2)
        Me.Controls.Add(Me.L1)
        Me.Controls.Add(Me.Clist2)
        Me.Controls.Add(Me.Cstatic)
        Me.Controls.Add(Me.rsecexist)
        Me.Controls.Add(Me.rsecnew)
        Me.Controls.Add(Me.Ltitle)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "bc_am_in_tk_da_frm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Insight Toolkit - Blue Curve"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub PictureBox2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Hide()
    End Sub

    Private Sub PictureBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Hide()
    End Sub
    Private Sub check_apply()
        Me.Bok.Enabled = False
        If Me.tinput.Visible = True And Trim(Me.tinput.Text) <> "" Then
            Me.Bok.Enabled = True
        End If
        If Me.clist.Visible = True And Me.clist.SelectedIndex <> -1 Then
            If Me.Clist2.Visible = True And Me.Clist2.SelectedIndex <> -1 Then
                Me.Bok.Enabled = True
            End If
            If Me.Clist2.Visible = False Then
                Me.Bok.Enabled = True
            End If
        End If
    End Sub
    Private Sub bc_am_in_tk_da_frm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.cancel_selected = True
    End Sub
    Private Sub PictureBox2_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Hide()
    End Sub

    Private Sub PictureBox1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.cancel_selected = False
        Me.Hide()
    End Sub

    Private Sub rsecnew_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rsecnew.CheckedChanged
        If Me.rsecnew.Checked = True Then
            Me.tinput.Visible = True
            Me.clist.Visible = False
            Me.tinput.Text = ""
            Me.clist.SelectedIndex = -1
            check_apply()
        End If
    End Sub

    Private Sub rsecexist_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rsecexist.CheckedChanged
        If Me.rsecexist.Checked = True Then
            Me.tinput.Visible = False
            Me.clist.Visible = True
            Me.tinput.Text = ""
            check_apply()
        End If

    End Sub


    Private Sub clist_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles clist.SelectedIndexChanged
        If Me.clist.SelectedIndex = -1 Then
            Exit Sub
        End If
        If Me.Clist2.Visible = True Then
            Me.fcontext.load_sections_for_template(Me.clist.SelectedIndex, Me)
        End If
        check_apply()
    End Sub

    Private Sub bcancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bcancel.Click
        Me.Hide()
    End Sub

    Private Sub Bok_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Bok.Click
        Me.cancel_selected = False
        Me.Hide()
    End Sub

    Private Sub tinput_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tinput.TextChanged
        check_apply()
    End Sub

    Private Sub Clist2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Clist2.SelectedIndexChanged
        check_apply()
        If Me.clist.SelectedIndex = -1 Then
            Exit Sub
        End If

    End Sub

    Private Sub Cstatic_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cstatic.CheckedChanged
        check_apply()
    End Sub
End Class
