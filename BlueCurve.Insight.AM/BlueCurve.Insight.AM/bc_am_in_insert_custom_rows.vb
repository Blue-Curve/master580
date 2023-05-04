Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports BlueCurve.Core.AS
Imports System.Windows.Forms
Imports System.Collections
Public Class bc_am_in_insert_custom_rows
    Inherits Create.AM.bc_am_at_wizard_generic

    Public ok_selected As Boolean
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
    Friend WithEvents btnsubmit As System.Windows.Forms.Button
    Friend WithEvents Btncancel As System.Windows.Forms.Button
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
    Friend WithEvents ListBox2 As System.Windows.Forms.ListBox
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents ComboBox2 As System.Windows.Forms.ComboBox
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents ListBox3 As System.Windows.Forms.ListBox
    Friend WithEvents ListView1 As System.Windows.Forms.ListView
    Friend WithEvents Period As System.Windows.Forms.ColumnHeader
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents ListView2 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_in_insert_custom_rows))
        Me.btnsubmit = New System.Windows.Forms.Button
        Me.Btncancel = New System.Windows.Forms.Button
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.Label13 = New System.Windows.Forms.Label
        Me.Label14 = New System.Windows.Forms.Label
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.ListBox1 = New System.Windows.Forms.ListBox
        Me.ListBox2 = New System.Windows.Forms.ListBox
        Me.ComboBox1 = New System.Windows.Forms.ComboBox
        Me.ComboBox2 = New System.Windows.Forms.ComboBox
        Me.CheckBox1 = New System.Windows.Forms.CheckBox
        Me.TextBox2 = New System.Windows.Forms.TextBox
        Me.ListBox3 = New System.Windows.Forms.ListBox
        Me.ListView1 = New System.Windows.Forms.ListView
        Me.Period = New System.Windows.Forms.ColumnHeader
        Me.Label15 = New System.Windows.Forms.Label
        Me.Button1 = New System.Windows.Forms.Button
        Me.Button2 = New System.Windows.Forms.Button
        Me.Button3 = New System.Windows.Forms.Button
        Me.Button4 = New System.Windows.Forms.Button
        Me.ListView2 = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Pubthumbnail, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LabelConStr
        '
        Me.LabelConStr.Text = ""
        '
        'Label1
        '
        Me.Label1.Text = ""
        '
        'Label4
        '
        Me.Label4.Text = "Logged in as:"
        '
        'btnsubmit
        '
        Me.btnsubmit.BackColor = System.Drawing.SystemColors.ControlLight
        Me.btnsubmit.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnsubmit.Location = New System.Drawing.Point(456, 304)
        Me.btnsubmit.Name = "btnsubmit"
        Me.btnsubmit.Size = New System.Drawing.Size(72, 24)
        Me.btnsubmit.TabIndex = 13
        Me.btnsubmit.Text = "&Ok"
        Me.btnsubmit.UseVisualStyleBackColor = False
        '
        'Btncancel
        '
        Me.Btncancel.BackColor = System.Drawing.SystemColors.ControlLight
        Me.Btncancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Btncancel.Location = New System.Drawing.Point(456, 328)
        Me.Btncancel.Name = "Btncancel"
        Me.Btncancel.Size = New System.Drawing.Size(72, 24)
        Me.Btncancel.TabIndex = 14
        Me.Btncancel.Text = "&Cancel"
        Me.Btncancel.UseVisualStyleBackColor = False
        '
        'Label8
        '
        Me.Label8.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.Label8.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(288, 152)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(80, 21)
        Me.Label8.TabIndex = 34
        Me.Label8.Text = "Scale Symbol"
        '
        'Label9
        '
        Me.Label9.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.Label9.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(288, 128)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(80, 16)
        Me.Label9.TabIndex = 35
        Me.Label9.Text = "Data Type"
        '
        'Label10
        '
        Me.Label10.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.Label10.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(288, 176)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(72, 16)
        Me.Label10.TabIndex = 36
        Me.Label10.Text = "Scale Factor"
        '
        'Label11
        '
        Me.Label11.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.Label11.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(288, 200)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(72, 21)
        Me.Label11.TabIndex = 37
        Me.Label11.Text = "Flexible Label"
        '
        'Label12
        '
        Me.Label12.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.Label12.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(288, 224)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(64, 21)
        Me.Label12.TabIndex = 38
        Me.Label12.Text = "Link Code"
        '
        'Label13
        '
        Me.Label13.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.Label13.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(288, 248)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(88, 16)
        Me.Label13.TabIndex = 39
        Me.Label13.Text = "Repeating Count"
        '
        'Label14
        '
        Me.Label14.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.Label14.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(288, 104)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(80, 16)
        Me.Label14.TabIndex = 40
        Me.Label14.Text = "Dimension Type"
        '
        'TextBox1
        '
        Me.TextBox1.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox1.Location = New System.Drawing.Point(384, 80)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(144, 18)
        Me.TextBox1.TabIndex = 41
        '
        'ListBox1
        '
        Me.ListBox1.AllowDrop = True
        Me.ListBox1.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListBox1.ItemHeight = 12
        Me.ListBox1.Location = New System.Drawing.Point(384, 104)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(104, 16)
        Me.ListBox1.TabIndex = 42
        '
        'ListBox2
        '
        Me.ListBox2.AllowDrop = True
        Me.ListBox2.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListBox2.ItemHeight = 12
        Me.ListBox2.Location = New System.Drawing.Point(384, 128)
        Me.ListBox2.Name = "ListBox2"
        Me.ListBox2.Size = New System.Drawing.Size(104, 16)
        Me.ListBox2.TabIndex = 43
        '
        'ComboBox1
        '
        Me.ComboBox1.AllowDrop = True
        Me.ComboBox1.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox1.Location = New System.Drawing.Point(384, 152)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(96, 20)
        Me.ComboBox1.TabIndex = 44
        '
        'ComboBox2
        '
        Me.ComboBox2.AllowDrop = True
        Me.ComboBox2.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox2.Location = New System.Drawing.Point(384, 176)
        Me.ComboBox2.Name = "ComboBox2"
        Me.ComboBox2.Size = New System.Drawing.Size(96, 20)
        Me.ComboBox2.TabIndex = 45
        '
        'CheckBox1
        '
        Me.CheckBox1.Location = New System.Drawing.Point(384, 200)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(16, 16)
        Me.CheckBox1.TabIndex = 46
        Me.CheckBox1.Text = "CheckBox1"
        '
        'TextBox2
        '
        Me.TextBox2.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox2.Location = New System.Drawing.Point(384, 224)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(144, 18)
        Me.TextBox2.TabIndex = 47
        '
        'ListBox3
        '
        Me.ListBox3.AllowDrop = True
        Me.ListBox3.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListBox3.ItemHeight = 12
        Me.ListBox3.Location = New System.Drawing.Point(384, 248)
        Me.ListBox3.Name = "ListBox3"
        Me.ListBox3.Size = New System.Drawing.Size(104, 16)
        Me.ListBox3.TabIndex = 48
        '
        'ListView1
        '
        Me.ListView1.AllowColumnReorder = True
        Me.ListView1.BackColor = System.Drawing.Color.White
        Me.ListView1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.Period})
        Me.ListView1.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListView1.FullRowSelect = True
        Me.ListView1.GridLines = True
        Me.ListView1.HideSelection = False
        Me.ListView1.Location = New System.Drawing.Point(112, 80)
        Me.ListView1.MultiSelect = False
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(96, 200)
        Me.ListView1.TabIndex = 50
        Me.ListView1.UseCompatibleStateImageBehavior = False
        Me.ListView1.View = System.Windows.Forms.View.Details
        '
        'Period
        '
        Me.Period.Text = "Period"
        Me.Period.Width = 91
        '
        'Label15
        '
        Me.Label15.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.Label15.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(288, 80)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(88, 11)
        Me.Label15.TabIndex = 51
        Me.Label15.Text = "Label"
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.SystemColors.ControlLight
        Me.Button1.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.Location = New System.Drawing.Point(216, 80)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(56, 24)
        Me.Button1.TabIndex = 52
        Me.Button1.Text = "&Modify"
        Me.Button1.UseVisualStyleBackColor = False
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.SystemColors.ControlLight
        Me.Button2.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button2.Location = New System.Drawing.Point(216, 112)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(56, 24)
        Me.Button2.TabIndex = 53
        Me.Button2.Text = "&Delete"
        Me.Button2.UseVisualStyleBackColor = False
        '
        'Button3
        '
        Me.Button3.BackColor = System.Drawing.SystemColors.ControlLight
        Me.Button3.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button3.Location = New System.Drawing.Point(224, 168)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(40, 24)
        Me.Button3.TabIndex = 54
        Me.Button3.Text = "&Up"
        Me.Button3.UseVisualStyleBackColor = False
        '
        'Button4
        '
        Me.Button4.BackColor = System.Drawing.SystemColors.ControlLight
        Me.Button4.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button4.Location = New System.Drawing.Point(224, 200)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(40, 24)
        Me.Button4.TabIndex = 55
        Me.Button4.Text = "&Dn"
        Me.Button4.UseVisualStyleBackColor = False
        '
        'ListView2
        '
        Me.ListView2.AllowColumnReorder = True
        Me.ListView2.BackColor = System.Drawing.Color.White
        Me.ListView2.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1})
        Me.ListView2.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListView2.FullRowSelect = True
        Me.ListView2.GridLines = True
        Me.ListView2.HideSelection = False
        Me.ListView2.Location = New System.Drawing.Point(16, 80)
        Me.ListView2.MultiSelect = False
        Me.ListView2.Name = "ListView2"
        Me.ListView2.Size = New System.Drawing.Size(96, 201)
        Me.ListView2.TabIndex = 56
        Me.ListView2.UseCompatibleStateImageBehavior = False
        Me.ListView2.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Static"
        Me.ColumnHeader1.Width = 91
        '
        'bc_am_in_insert_custom_rows
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(538, 359)
        Me.Controls.Add(Me.ListView2)
        Me.Controls.Add(Me.Button4)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Label15)
        Me.Controls.Add(Me.ListView1)
        Me.Controls.Add(Me.ListBox3)
        Me.Controls.Add(Me.TextBox2)
        Me.Controls.Add(Me.CheckBox1)
        Me.Controls.Add(Me.ComboBox2)
        Me.Controls.Add(Me.ComboBox1)
        Me.Controls.Add(Me.ListBox2)
        Me.Controls.Add(Me.ListBox1)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.Label14)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.btnsubmit)
        Me.Controls.Add(Me.Btncancel)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "bc_am_in_insert_custom_rows"
        Me.Text = "bc_am_in_insert_custom_rows"
        Me.Controls.SetChildIndex(Me.Btncancel, 0)
        Me.Controls.SetChildIndex(Me.btnsubmit, 0)
        Me.Controls.SetChildIndex(Me.Label8, 0)
        Me.Controls.SetChildIndex(Me.Label9, 0)
        Me.Controls.SetChildIndex(Me.Label10, 0)
        Me.Controls.SetChildIndex(Me.Label11, 0)
        Me.Controls.SetChildIndex(Me.Label12, 0)
        Me.Controls.SetChildIndex(Me.Label13, 0)
        Me.Controls.SetChildIndex(Me.Label14, 0)
        Me.Controls.SetChildIndex(Me.TextBox1, 0)
        Me.Controls.SetChildIndex(Me.ListBox1, 0)
        Me.Controls.SetChildIndex(Me.ListBox2, 0)
        Me.Controls.SetChildIndex(Me.ComboBox1, 0)
        Me.Controls.SetChildIndex(Me.ComboBox2, 0)
        Me.Controls.SetChildIndex(Me.CheckBox1, 0)
        Me.Controls.SetChildIndex(Me.TextBox2, 0)
        Me.Controls.SetChildIndex(Me.ListBox3, 0)
        Me.Controls.SetChildIndex(Me.ListView1, 0)
        Me.Controls.SetChildIndex(Me.Label15, 0)
        Me.Controls.SetChildIndex(Me.Button1, 0)
        Me.Controls.SetChildIndex(Me.Button2, 0)
        Me.Controls.SetChildIndex(Me.Button3, 0)
        Me.Controls.SetChildIndex(Me.Button4, 0)
        Me.Controls.SetChildIndex(Me.ListView2, 0)
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Pubthumbnail, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private Sub bc_am_in_insert_custom_rows_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = "Blue Curve - insight custom rows"
        Me.Pubthumbnail.Visible = False
        Me.PictureBox3.Visible = False
        MyBase.Label1.Enabled = False
        MyBase.Label4.Visible = False
        MyBase.Label5.Visible = False
        MyBase.Label6.Visible = False
        MyBase.Label7.Visible = False
        load_items()
        load_list_view()
    End Sub

    Private Sub Btncancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btncancel.Click
        Me.ok_selected = False
        Me.Hide()
    End Sub
    Private Sub load_items()
        Me.ListBox1.Items.Clear()
        Me.ListBox1.Items.Add("Period")
        Me.ListBox1.Items.Add("Static")
        Me.ListBox2.Items.Clear()
        Me.ListBox2.Items.Add("String")
        Me.ListBox2.Items.Add("Boolean")
        Me.ListBox2.Items.Add("Number")
        Me.ListBox2.Items.Add("Date")
        Me.ComboBox1.Items.Clear()
        Me.ComboBox2.Items.Clear()
        Me.ListBox3.Items.Clear()
        Me.ListBox3.Items.Add("0")
        Me.ListBox3.Items.Add("1")
        Me.ListBox3.Items.Add("2")
        Me.ListBox3.Items.Add("3")
        Me.ListBox3.Items.Add("4")
        Me.ListBox3.Items.Add("5")
        Me.ListBox3.Items.Add("6")
        Me.ListBox3.Items.Add("7")
        Me.ListBox3.Items.Add("8")
        Me.ListBox3.Items.Add("9")
        Me.ComboBox2.Enabled = False
        Me.Button1.Enabled = False
        Me.Button2.Enabled = False
        Me.Button3.Enabled = False
        Me.Button4.Enabled = False
        Me.Button1.Text = "Add"
        Me.ListBox3.SelectedIndex = 0

    End Sub
    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        If Me.ListBox2.SelectedIndex = 2 Then
            If Me.ComboBox1.SelectedIndex <> -1 Then
                Me.ComboBox2.SelectedIndex() = Me.ComboBox1.SelectedIndex
            End If
        End If
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox2.SelectedIndexChanged

        If Me.ListBox2.SelectedIndex = 2 Then
            If Me.ComboBox2.SelectedIndex <> -1 Then
                Me.ComboBox1.SelectedIndex() = Me.ComboBox2.SelectedIndex
            End If
        End If
        check_correct()
    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged
        check_correct()
        Dim i As Integer
        Dim found As Boolean = False
        For i = 0 To bc_am_load_objects.obc_om_custom_period_section.rows.Count - 1
            If Me.TextBox1.Text = bc_am_load_objects.obc_om_custom_period_section.rows(i).label Then
                found = True
                Exit For
            End If
        Next
        If found = False Then
            For i = 0 To bc_am_load_objects.obc_om_custom_static_section.rows.Count - 1
                If Me.TextBox1.Text = bc_am_load_objects.obc_om_custom_static_section.rows(i).label Then
                    found = True
                    Exit For
                End If
            Next
        End If
        If found = False Then
            Me.Button1.Text = "Add"
            Me.ListBox1.Enabled = True
        Else
            Me.Button1.Text = "Modify"
        End If

    End Sub

    Private Sub ListBox2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListBox2.SelectedIndexChanged
        Me.ComboBox1.Text = ""
        Me.ComboBox2.Text = ""
        Me.ComboBox1.Items.Clear()
        Me.ComboBox2.Items.Clear()
        If Me.ListBox2.SelectedIndex = 2 Then
            Me.ComboBox2.Enabled = True
            Me.ComboBox1.Items.Add("(%)")
            Me.ComboBox1.Items.Add("(unit)")
            Me.ComboBox1.Items.Add("'000")
            Me.ComboBox1.Items.Add("(m)")
            Me.ComboBox2.Items.Add("0.01")
            Me.ComboBox2.Items.Add("1")
            Me.ComboBox2.Items.Add("100")
            Me.ComboBox2.Items.Add("1000000")
        ElseIf (Me.ListBox2.SelectedIndex = 3) Then
            Me.ComboBox2.Text = ""
            Me.ComboBox2.Enabled = False
            Me.ComboBox1.Text = ""
            Me.ComboBox1.Items.Add("(dd-mm-yy")
            Me.ComboBox1.Items.Add("(dd-mm-yyyy")
            Me.ComboBox1.Items.Add("(dd-mmm-yyyy")
        Else
            Me.ComboBox2.Text = ""
            Me.ComboBox2.Enabled = False
            Me.ComboBox1.Text = ""
        End If
        check_correct()
    End Sub
    Private Sub check_correct()
        Me.Button1.Enabled = False
        If Me.TextBox1.Text <> "" Then
            If Me.ListBox1.SelectedIndex > -1 Then
                If Me.ListBox2.SelectedIndex > -1 Then
                    If Me.ListBox2.SelectedIndex = 2 Then
                        If Me.ComboBox2.SelectedIndex > -1 Then
                            Me.Button1.Enabled = True
                        End If
                    Else
                        Me.Button1.Enabled = True
                    End If
                End If
            End If
        End If
    End Sub
    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox2.TextChanged

    End Sub

    Private Sub ListBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListBox1.SelectedIndexChanged
        If Me.ListBox1.SelectedIndex = 0 Then
            Me.ListBox3.SelectedIndex = 0
            Me.ListBox3.Enabled = False
        Else
            Me.ListBox3.SelectedIndex = 0
            Me.ListBox3.Enabled = True
        End If
        check_correct()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim slog = New bc_cs_activity_log("bc_am_in_insert_custom_rows", "button1_click", bc_cs_activity_codes.TRACE_ENTRY, "")
        
        Try
            Dim submission_code As Integer

            Select Case Me.ListBox1.SelectedIndex
                Case 0
                    If Me.ListView1.SelectedItems.Count = 0 And Me.Button1.Text = "modify" Then
                        Exit Sub
                    End If
                    submission_code = 0
                Case 1
                    If Me.ListView2.SelectedItems.Count = 0 And Me.Button1.Text = "modify" Then
                        Exit Sub
                    End If

                    If Me.ListBox3.SelectedIndex = 0 Then
                        submission_code = 2
                    Else
                        submission_code = 3
                    End If
            End Select
            Dim orow As New bc_om_insight_row("", Me.ComboBox1.Text, Me.ComboBox2.Text, 0, Me.CheckBox1.Checked, Me.TextBox2.Text, CStr(Me.ListBox2.SelectedIndex), Me.ListBox1.SelectedIndex, submission_code, Me.ListBox3.SelectedIndex, "")
            REM zero indicates row is static
            orow.row_id = 0
            orow.label = Me.TextBox1.Text

            REM if modify update
            REM if new add
            If Me.Button1.Text = "Add" Then
                If Me.ListBox1.SelectedIndex = 0 Then
                    REM add to period list
                    orow.order = bc_am_load_objects.obc_om_custom_period_section.rows.Count
                    bc_am_load_objects.obc_om_custom_period_section.rows.Add(orow)
                Else
                    REM add to static list
                    orow.order = bc_am_load_objects.obc_om_custom_static_section.rows.Count
                    bc_am_load_objects.obc_om_custom_static_section.rows.Add(orow)
                End If
            Else
                If Me.ListBox1.SelectedIndex = 0 Then
                    orow.order = Me.ListView1.SelectedItems(0).Index
                    bc_am_load_objects.obc_om_custom_period_section.rows(Me.ListView1.SelectedItems(0).Index) = orow
                Else
                    orow.order = Me.ListView2.SelectedItems(0).Index
                    bc_am_load_objects.obc_om_custom_static_section.rows(Me.ListView2.SelectedItems(0).Index) = orow
                End If
            End If

            load_list_view()
            Me.Button1.Enabled = False

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_insert_custom_rows", "button1_click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_in_insert_custom_rows", "button1_click", bc_cs_activity_codes.TRACE_ENTRY, "")
        End Try
    End Sub
    Private Sub load_list_view()
        Dim i As Integer
        Me.ListView1.Items.Clear()
        For i = 0 To bc_am_load_objects.obc_om_custom_period_section.rows.Count - 1
            Dim item As New ListViewItem(CStr(bc_am_load_objects.obc_om_custom_period_section.rows(i).label))
            Me.ListView1.Items.Add(item)
        Next
        Me.ListView2.Items.Clear()
        For i = 0 To bc_am_load_objects.obc_om_custom_static_section.rows.Count - 1
            Dim item As New ListViewItem(CStr(bc_am_load_objects.obc_om_custom_static_section.rows(i).label))
            Me.ListView2.Items.Add(item)
        Next
    End Sub

    Private Sub ListView1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView1.SelectedIndexChanged

        Dim slog = New bc_cs_activity_log("bc_am_in_insert_custom_rows", "ListView1_SelectedIndexChanged", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            Me.Button1.Text = "Modify"
            Me.Button1.Enabled = True
            Me.Button2.Enabled = True
            Me.Button3.Enabled = True
            Me.Button4.Enabled = True
            Me.ListBox1.Enabled = False
            If Me.ListView1.SelectedItems.Count = 1 Then
                Me.TextBox1.Text = bc_am_load_objects.obc_om_custom_period_section.rows(Me.ListView1.SelectedItems(0).Index).label
                Me.ListBox1.SelectedIndex = 0
                Me.ListBox2.SelectedIndex = CInt(bc_am_load_objects.obc_om_custom_period_section.rows(Me.ListView1.SelectedItems(0).Index).datatype)
                Me.ComboBox1.Text = bc_am_load_objects.obc_om_custom_period_section.rows(Me.ListView1.SelectedItems(0).Index).scale_symbol
                Me.ComboBox2.Text = bc_am_load_objects.obc_om_custom_period_section.rows(Me.ListView1.SelectedItems(0).Index).scale_factor
                Me.CheckBox1.Checked = bc_am_load_objects.obc_om_custom_period_section.rows(Me.ListView1.SelectedItems(0).Index).flexible_label_flag
                Me.TextBox2.Text = bc_am_load_objects.obc_om_custom_period_section.rows(Me.ListView1.SelectedItems(0).Index).link_code
                Me.ListBox3.SelectedIndex = bc_am_load_objects.obc_om_custom_period_section.rows(Me.ListView1.SelectedItems(0).Index).repeating_count
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_insert_custom_rows", "ListView1_SelectedIndexChanged", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_in_insert_custom_rows", "ListView1_SelectedIndexChanged", bc_cs_activity_codes.TRACE_ENTRY, "")
        End Try

    End Sub
    Private Sub ListView2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView2.SelectedIndexChanged
        Dim slog = New bc_cs_activity_log("bc_am_in_insert_custom_rows", "ListView1_SelectedIndexChanged", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            Me.Button1.Text = "Modify"
            Me.Button1.Enabled = True
            Me.Button2.Enabled = True
            Me.Button3.Enabled = True
            Me.Button4.Enabled = True
            Me.ListBox1.Enabled = False
            If Me.ListView2.SelectedItems.Count = 1 Then
                Me.TextBox1.Text = bc_am_load_objects.obc_om_custom_static_section.rows(Me.ListView2.SelectedItems(0).Index).label
                Me.ListBox1.SelectedIndex = 1
                Me.ListBox2.SelectedIndex = CInt(bc_am_load_objects.obc_om_custom_static_section.rows(Me.ListView2.SelectedItems(0).Index).datatype)
                Me.ComboBox1.Text = bc_am_load_objects.obc_om_custom_static_section.rows(Me.ListView2.SelectedItems(0).Index).scale_symbol
                Me.ComboBox2.Text = bc_am_load_objects.obc_om_custom_static_section.rows(Me.ListView2.SelectedItems(0).Index).scale_factor
                Me.CheckBox1.Checked = bc_am_load_objects.obc_om_custom_static_section.rows(Me.ListView2.SelectedItems(0).Index).flexible_label_flag
                Me.TextBox2.Text = bc_am_load_objects.obc_om_custom_static_section.rows(Me.ListView2.SelectedItems(0).Index).link_code
                Me.ListBox3.SelectedIndex = bc_am_load_objects.obc_om_custom_static_section.rows(Me.ListView2.SelectedItems(0).Index).repeating_count
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_insert_custom_rows", "ListView1_SelectedIndexChanged", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_in_insert_custom_rows", "ListView1_SelectedIndexChanged", bc_cs_activity_codes.TRACE_ENTRY, "")
        End Try

    End Sub

    Private Sub btnsubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnsubmit.Click
        Me.ok_selected = True
        Me.Hide()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim slog As New bc_cs_activity_log("bc_am_in_insert_custom_rows", "Button2_Click", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            If Me.ListBox1.SelectedIndex = 0 Then
                bc_am_load_objects.obc_om_custom_period_section.rows.RemoveAt(Me.ListView1.SelectedItems(0).Index)
            Else
                bc_am_load_objects.obc_om_custom_static_section.rows.RemoveAt(Me.ListView2.SelectedItems(0).Index)
            End If
            load_list_view()
            Me.Button2.Enabled = False
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_insert_custom_rows", "Button2_Click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_in_insert_custom_rows", "Button2_Click", bc_cs_activity_codes.TRACE_ENTRY, "")
        End Try
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Dim slog As New bc_cs_activity_log("bc_am_in_insert_custom_rows", "Button3_Click", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim orow As New bc_om_insight_row
            Dim i, j As Integer
            If Me.ListBox1.SelectedIndex = 0 Then
                i = Me.ListView1.SelectedItems(0).Index
                j = Me.ListView1.Items.Count
                If i = 0 Then
                    Exit Sub
                End If
                orow = bc_am_load_objects.obc_om_custom_period_section.rows(i - 1)
                bc_am_load_objects.obc_om_custom_period_section.rows.RemoveAt(i - 1)
                If i < j - 1 Then
                    bc_am_load_objects.obc_om_custom_period_section.rows.Insert(i, orow)
                Else
                    bc_am_load_objects.obc_om_custom_period_section.rows.Add(orow)
                End If
                Me.ListView1.Items(i - 1).Selected = True

            Else
                i = Me.ListView2.SelectedItems(0).Index
                j = Me.ListView2.Items.Count
                If i = 0 Then
                    Exit Sub
                End If
                orow = bc_am_load_objects.obc_om_custom_static_section.rows(i - 1)
                bc_am_load_objects.obc_om_custom_static_section.rows.RemoveAt(i - 1)
                If i < j - 1 Then
                    bc_am_load_objects.obc_om_custom_static_section.rows.Insert(i + 1, orow)
                Else
                    bc_am_load_objects.obc_om_custom_static_section.rows.Add(orow)
                End If
                Me.ListView2.Items(i - 1).Selected = True
            End If
            load_list_view()
            Me.Button1.Enabled = False
            Me.Button2.Enabled = False
            Me.Button3.Enabled = False
            Me.Button4.Enabled = False

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_insert_custom_rows", "Button3_Click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_in_insert_custom_rows", "Button3_Click", bc_cs_activity_codes.TRACE_ENTRY, "")
        End Try
    End Sub
    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Dim slog As New bc_cs_activity_log("bc_am_in_insert_custom_rows", "Button4_Click", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim orow As New bc_om_insight_row
            Dim i, j As Integer
            If Me.ListBox1.SelectedIndex = 0 Then
                i = Me.ListView1.SelectedItems(0).Index
                j = Me.ListView1.Items.Count
                If i = j - 1 Then
                    Exit Sub
                End If
                orow = bc_am_load_objects.obc_om_custom_period_section.rows(i + 1)
                bc_am_load_objects.obc_om_custom_period_section.rows.RemoveAt(i + 1)
                If i > 0 Then
                    bc_am_load_objects.obc_om_custom_period_section.rows.Insert(i, orow)
                Else
                    bc_am_load_objects.obc_om_custom_period_section.rows.Insert(0, orow)
                End If
            Else
                i = Me.ListView2.SelectedItems(0).Index
                j = Me.ListView2.Items.Count
                If i = j - 1 Then
                    Exit Sub
                End If
                orow = bc_am_load_objects.obc_om_custom_static_section.rows(i + 1)
                bc_am_load_objects.obc_om_custom_static_section.rows.RemoveAt(i + 1)
                If i > 0 Then
                    bc_am_load_objects.obc_om_custom_static_section.rows.Insert(i, orow)
                Else
                    bc_am_load_objects.obc_om_custom_static_section.rows.Insert(0, orow)
                End If
            End If
            load_list_view()
            Me.Button1.Enabled = False
            Me.Button2.Enabled = False
            Me.Button3.Enabled = False
            Me.Button4.Enabled = False

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_insert_custom_rows", "Button4_Click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_in_insert_custom_rows", "Button4_Click", bc_cs_activity_codes.TRACE_ENTRY, "")
        End Try
    End Sub

    Private Sub Button2_ControlAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.ControlEventArgs) Handles Button2.ControlAdded

    End Sub
End Class
