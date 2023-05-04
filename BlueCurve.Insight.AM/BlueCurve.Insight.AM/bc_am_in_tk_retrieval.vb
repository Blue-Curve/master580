Imports BlueCurve.Core.CS

Public Class bc_am_in_tk_retrieval
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
    Friend WithEvents Citem As System.Windows.Forms.ComboBox
    Friend WithEvents cdimension As System.Windows.Forms.ComboBox
    Friend WithEvents Cstorage As System.Windows.Forms.ComboBox
    Friend WithEvents Crow As System.Windows.Forms.ComboBox
    Friend WithEvents Ccol As System.Windows.Forms.ComboBox
    Friend WithEvents sscale As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents rsystem As System.Windows.Forms.RadioButton
    Friend WithEvents ruser As System.Windows.Forms.RadioButton
    Friend WithEvents tsheet As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Corder As System.Windows.Forms.ComboBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents bapply As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_in_tk_retrieval))
        Me.Citem = New System.Windows.Forms.ComboBox
        Me.cdimension = New System.Windows.Forms.ComboBox
        Me.Cstorage = New System.Windows.Forms.ComboBox
        Me.Corder = New System.Windows.Forms.ComboBox
        Me.Crow = New System.Windows.Forms.ComboBox
        Me.Ccol = New System.Windows.Forms.ComboBox
        Me.sscale = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.rsystem = New System.Windows.Forms.RadioButton
        Me.ruser = New System.Windows.Forms.RadioButton
        Me.tsheet = New System.Windows.Forms.TextBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Button1 = New System.Windows.Forms.Button
        Me.bapply = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'Citem
        '
        Me.Citem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Citem.Location = New System.Drawing.Point(112, 24)
        Me.Citem.Name = "Citem"
        Me.Citem.Size = New System.Drawing.Size(160, 21)
        Me.Citem.TabIndex = 1
        '
        'cdimension
        '
        Me.cdimension.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cdimension.Location = New System.Drawing.Point(112, 56)
        Me.cdimension.Name = "cdimension"
        Me.cdimension.Size = New System.Drawing.Size(160, 21)
        Me.cdimension.TabIndex = 2
        '
        'Cstorage
        '
        Me.Cstorage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Cstorage.Location = New System.Drawing.Point(112, 88)
        Me.Cstorage.Name = "Cstorage"
        Me.Cstorage.Size = New System.Drawing.Size(160, 21)
        Me.Cstorage.TabIndex = 3
        '
        'Corder
        '
        Me.Corder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Corder.Location = New System.Drawing.Point(112, 120)
        Me.Corder.Name = "Corder"
        Me.Corder.Size = New System.Drawing.Size(160, 21)
        Me.Corder.TabIndex = 4
        '
        'Crow
        '
        Me.Crow.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Crow.Location = New System.Drawing.Point(112, 224)
        Me.Crow.Name = "Crow"
        Me.Crow.Size = New System.Drawing.Size(56, 21)
        Me.Crow.TabIndex = 8
        '
        'Ccol
        '
        Me.Ccol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Ccol.Location = New System.Drawing.Point(216, 224)
        Me.Ccol.Name = "Ccol"
        Me.Ccol.Size = New System.Drawing.Size(56, 21)
        Me.Ccol.TabIndex = 9
        '
        'sscale
        '
        Me.sscale.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.sscale.Location = New System.Drawing.Point(216, 258)
        Me.sscale.Name = "sscale"
        Me.sscale.Size = New System.Drawing.Size(56, 21)
        Me.sscale.TabIndex = 10
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(8, 24)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(96, 16)
        Me.Label1.TabIndex = 44
        Me.Label1.Text = "Retrieve Item:"
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(8, 56)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(96, 16)
        Me.Label2.TabIndex = 45
        Me.Label2.Text = "Dimension:"
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(8, 88)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(96, 16)
        Me.Label3.TabIndex = 46
        Me.Label3.Text = "Storage:"
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(8, 123)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(96, 16)
        Me.Label4.TabIndex = 47
        Me.Label4.Text = "Repeat order:"
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(8, 228)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(96, 16)
        Me.Label5.TabIndex = 48
        Me.Label5.Text = "Excel row:"
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(182, 227)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(32, 16)
        Me.Label6.TabIndex = 49
        Me.Label6.Text = "Col:"
        '
        'Label7
        '
        Me.Label7.Location = New System.Drawing.Point(8, 262)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(176, 16)
        Me.Label7.TabIndex = 50
        Me.Label7.Text = "Apply on output scale factor:"
        '
        'rsystem
        '
        Me.rsystem.Checked = True
        Me.rsystem.Location = New System.Drawing.Point(112, 150)
        Me.rsystem.Name = "rsystem"
        Me.rsystem.Size = New System.Drawing.Size(64, 24)
        Me.rsystem.TabIndex = 5
        Me.rsystem.TabStop = True
        Me.rsystem.Text = "system"
        '
        'ruser
        '
        Me.ruser.Location = New System.Drawing.Point(184, 151)
        Me.ruser.Name = "ruser"
        Me.ruser.Size = New System.Drawing.Size(48, 24)
        Me.ruser.TabIndex = 6
        Me.ruser.Text = "user"
        '
        'tsheet
        '
        Me.tsheet.Enabled = False
        Me.tsheet.Location = New System.Drawing.Point(112, 184)
        Me.tsheet.MaxLength = 100
        Me.tsheet.Name = "tsheet"
        Me.tsheet.Size = New System.Drawing.Size(160, 21)
        Me.tsheet.TabIndex = 7
        '
        'Label8
        '
        Me.Label8.Location = New System.Drawing.Point(8, 154)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(104, 16)
        Me.Label8.TabIndex = 54
        Me.Label8.Text = "Output to sheet:"
        '
        'Label9
        '
        Me.Label9.Enabled = False
        Me.Label9.Location = New System.Drawing.Point(8, 188)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(88, 16)
        Me.Label9.TabIndex = 55
        Me.Label9.Text = "sheet name:"
        '
        'Button1
        '
        Me.Button1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.Location = New System.Drawing.Point(196, 303)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 12
        Me.Button1.Text = "Cancel"
        '
        'bapply
        '
        Me.bapply.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.bapply.Enabled = False
        Me.bapply.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bapply.Location = New System.Drawing.Point(118, 304)
        Me.bapply.Name = "bapply"
        Me.bapply.Size = New System.Drawing.Size(75, 23)
        Me.bapply.TabIndex = 11
        Me.bapply.Text = "Ok"
        '
        'bc_am_in_tk_retrieval
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(280, 332)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.bapply)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.tsheet)
        Me.Controls.Add(Me.ruser)
        Me.Controls.Add(Me.rsystem)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.sscale)
        Me.Controls.Add(Me.Ccol)
        Me.Controls.Add(Me.Crow)
        Me.Controls.Add(Me.Corder)
        Me.Controls.Add(Me.Cstorage)
        Me.Controls.Add(Me.cdimension)
        Me.Controls.Add(Me.Citem)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "bc_am_in_tk_retrieval"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Date Retrieval Item Properties"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region
    Public cancel_selected As Boolean = True
    Public from_edit As Boolean = False
    Public oret As bc_am_in_retrieval
    Public orig_sheet As String
    Public orig_row As String
    Public orig_col As String
    Private Sub bc_am_in_tk_retrieval_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim i As Integer
        Me.Cstorage.Items.Clear()
        Me.Cstorage.Items.Add("class")
        Me.Cstorage.Items.Add("value")
        Me.Cstorage.Items.Add("time series")
        Me.Cstorage.Items.Add("repeating")
        Me.Cstorage.Items.Add("repeating time series")
        Me.Crow.Items.Clear()
        Me.Ccol.Items.Clear()
        Me.Corder.Items.Clear()
        For i = 1 To 2000
            Me.Crow.Items.Add(CStr(i))
            Me.Ccol.Items.Add(CStr(i))
            If i <= 30 Then
                Me.Corder.Items.Add(CStr(i))
            End If
        Next
        Me.sscale.Items.Clear()
        Me.sscale.Items.Add("1")
        Me.sscale.Items.Add("0.01")
        Me.sscale.Items.Add("10")
        Me.sscale.Items.Add("100")
        Me.sscale.Items.Add("1000")
        Me.sscale.Items.Add("100000")
        Me.Citem.Items.Clear()
        REM load classes first
        For i = 0 To bc_am_in_context.insight_items.class_names.Count - 1
            Me.Citem.Items.Add(bc_am_in_context.insight_items.class_names(i))
        Next
        REM now load items static only
        For i = 0 To bc_am_in_context.insight_items.insight_items.Count - 1
            If bc_am_in_context.insight_items.insight_items(i).row_flag = 0 Then
                Me.Citem.Items.Add(bc_am_in_context.insight_items.insight_items(i).desc)
            End If
        Next
        If from_edit = True Then

            Me.bapply.Enabled = False
            Me.Citem.Text = oret.litems.SelectedItems(0).Text
            If oret.litems.SelectedItems(0).SubItems(1).Text() = "System Sheets" Then
                Me.rsystem.Checked = True
                Me.orig_sheet = ""

            Else
                Me.ruser.Checked = True
                Me.tsheet.Text = oret.litems.SelectedItems(0).SubItems(1).Text()
                Me.orig_sheet = Me.tsheet.Text

            End If

            Me.Crow.Text = oret.litems.SelectedItems(0).SubItems(2).Text
            Me.orig_row = oret.litems.SelectedItems(0).SubItems(2).Text
            Me.orig_col = oret.litems.SelectedItems(0).SubItems(3).Text

            Me.Ccol.Text = oret.litems.SelectedItems(0).SubItems(3).Text
            Me.sscale.Text = oret.litems.SelectedItems(0).SubItems(4).Text
            Me.Cstorage.Text = oret.litems.SelectedItems(0).SubItems(5).Text
            Me.cdimension.Text = oret.litems.SelectedItems(0).SubItems(6).Text
            Me.Corder.Text = oret.litems.SelectedItems(0).SubItems(7).Text
        End If
    End Sub
    Private Sub bcancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Hide()
    End Sub

    Private Sub bapply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        cancel_selected = False
        Me.Hide()
    End Sub

    Private Sub ruser_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ruser.CheckedChanged
        If ruser.Checked = True Then
            Me.tsheet.Enabled = True
            Me.tsheet.Text = ""
            Me.Label9.Enabled = True
        End If
    End Sub

    Private Sub rsystem_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rsystem.CheckedChanged
        If rsystem.Checked = True Then
            Me.tsheet.Enabled = False
            Me.tsheet.Text = ""
            Me.Label9.Enabled = False
        End If
    End Sub
    Private Sub set_apply()
        If Me.Citem.SelectedIndex > -1 And Me.Cstorage.SelectedIndex > -1 And Me.sscale.SelectedIndex > -1 And Me.Crow.SelectedIndex > -1 And Me.Ccol.SelectedIndex > -1 And Me.cdimension.SelectedIndex > -1 Then
            Me.bapply.Enabled = True
            If Me.Cstorage.SelectedIndex > 1 And Me.Corder.SelectedIndex = -1 Then
                Me.bapply.Enabled = False
                Exit Sub
            End If
            If Me.ruser.Checked = True And Trim(Me.tsheet.Text) = "" Then
                Me.bapply.Enabled = False
                Exit Sub
            End If
        End If
    End Sub
    Private Sub Citem_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Citem.SelectedIndexChanged
        Me.Cstorage.Enabled = False
        Me.cdimension.Enabled = False
        Me.Cstorage.Items.Clear()
        Me.cdimension.Items.Clear()
        If Me.Citem.SelectedIndex <= bc_am_in_context.insight_items.class_names.Count - 1 Then
            Me.Cstorage.Items.Add("class")
            Me.Cstorage.SelectedIndex = 0
            Me.ruser.Checked = True
            Me.rsystem.Enabled = False
            Me.cdimension.Items.Add("Value")
            Me.cdimension.Enabled = False
            Me.cdimension.SelectedIndex = 0
        Else
            Me.rsystem.Checked = True
            Me.rsystem.Enabled = True
            Me.Cstorage.Items.Clear()
            Me.Cstorage.Enabled = True
            Me.Cstorage.Items.Add("value")
            Me.Cstorage.Items.Add("time series")
            Me.Cstorage.Items.Add("repeating")
            Me.Cstorage.Items.Add("repeating time series")
            Me.cdimension.Items.Add("Value")
            Me.cdimension.Items.Add("Date")
            Me.cdimension.SelectedIndex = 0
            Me.cdimension.Enabled = True
        End If
        Me.sscale.SelectedIndex = 0
        set_apply()
    End Sub

    Private Sub Cstorage_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cstorage.SelectedIndexChanged
        Me.Corder.Enabled = False
        If Me.Cstorage.SelectedIndex > 1 Then
            Me.Corder.Enabled = True
        Else
            Me.Corder.SelectedIndex = -1
        End If
        set_apply()
    End Sub

    Private Sub cdimension_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cdimension.SelectedIndexChanged
        set_apply()
    End Sub

    Private Sub Corder_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Corder.SelectedIndexChanged
        set_apply()
    End Sub

    Private Sub tsheet_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsheet.TextChanged
        set_apply()
    End Sub

    Private Sub Crow_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Crow.SelectedIndexChanged
        set_apply()
    End Sub

    Private Sub Ccol_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Ccol.SelectedIndexChanged
        set_apply()
    End Sub

    Private Sub sscale_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles sscale.SelectedIndexChanged
        set_apply()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.cancel_selected = True
        Me.Hide()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bapply.Click
        cancel_selected = False
        Dim i As Integer
        Dim omsg As bc_cs_message
        If Me.from_edit = True Then
            If Me.orig_sheet = "" Or Me.orig_sheet = tsheet.Text Then
                If Me.orig_row = Crow.Text And Me.orig_col = Ccol.Text Then
                    Me.Hide()
                    Exit Sub
                End If
            End If
            If Me.orig_sheet = "" Then
                If Me.orig_row = Crow.Text And Me.orig_col = Ccol.Text Then
                    Me.Hide()
                    Exit Sub
                End If
            End If
        End If
        REM check no duplicates rows and columns for sheet
        If rsystem.Checked = True Then
            For i = 0 To Me.oret.litems.Items.Count - 1
                If Crow.Text = Me.oret.litems.Items(i).SubItems(2).Text And Ccol.Text = Me.oret.litems.Items(i).SubItems(3).Text Then
                    omsg = New bc_cs_message("Blue Curve", "Item already exists in system sheet row " + Crow.Text + " Column " + Ccol.Text, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub
                End If
            Next
        Else
            For i = 0 To Me.oret.litems.Items.Count - 1
                If tsheet.Text = Me.oret.litems.Items(i).SubItems(1).Text And Crow.Text = Me.oret.litems.Items(i).SubItems(2).Text And Ccol.Text = Me.oret.litems.Items(i).SubItems(3).Text Then
                    omsg = New bc_cs_message("Blue Curve", "Item already exists in sheet " + tsheet.Text + "  row " + Crow.Text + " Column " + Ccol.Text, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub
                End If
            Next
        End If
        Me.Hide()
    End Sub
End Class
