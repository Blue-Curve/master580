Imports BlueCurve.Core.CS
Imports System.Windows.Forms
Public Class bc_am_in_tk_cont
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
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ccp As System.Windows.Forms.ComboBox
    Friend WithEvents ccc As System.Windows.Forms.ComboBox
    Friend WithEvents ccs As System.Windows.Forms.ComboBox
    Friend WithEvents cgran As System.Windows.Forms.ComboBox
    Friend WithEvents ltitle As System.Windows.Forms.Label
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents Bok As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Cprime As System.Windows.Forms.ComboBox
    Friend WithEvents Csec As System.Windows.Forms.ComboBox
    Friend WithEvents bcancel As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_in_tk_cont))
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.ccp = New System.Windows.Forms.ComboBox
        Me.ccc = New System.Windows.Forms.ComboBox
        Me.ccs = New System.Windows.Forms.ComboBox
        Me.cgran = New System.Windows.Forms.ComboBox
        Me.ltitle = New System.Windows.Forms.Label
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Cprime = New System.Windows.Forms.ComboBox
        Me.Csec = New System.Windows.Forms.ComboBox
        Me.Bok = New System.Windows.Forms.Button
        Me.bcancel = New System.Windows.Forms.Button
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'Label8
        '
        Me.Label8.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(8, 40)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(88, 16)
        Me.Label8.TabIndex = 75
        Me.Label8.Text = "Parent Class:"
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(8, 136)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(80, 16)
        Me.Label1.TabIndex = 76
        Me.Label1.Text = "Child Class:"
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(8, 72)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(80, 16)
        Me.Label2.TabIndex = 77
        Me.Label2.Text = "Schema:"
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(8, 104)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(120, 24)
        Me.Label3.TabIndex = 78
        Me.Label3.Text = "Granularity Class:"
        '
        'ccp
        '
        Me.ccp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ccp.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ccp.Location = New System.Drawing.Point(132, 40)
        Me.ccp.Name = "ccp"
        Me.ccp.Size = New System.Drawing.Size(152, 21)
        Me.ccp.TabIndex = 1
        Me.ToolTip1.SetToolTip(Me.ccp, "entities of this class are used for contribution selection by user")
        '
        'ccc
        '
        Me.ccc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ccc.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ccc.Location = New System.Drawing.Point(132, 136)
        Me.ccc.Name = "ccc"
        Me.ccc.Size = New System.Drawing.Size(152, 21)
        Me.ccc.TabIndex = 4
        Me.ToolTip1.SetToolTip(Me.ccc, "Determines whether or not to contribute child entities of selected class at the s" & _
                "ame time as the parent.")
        '
        'ccs
        '
        Me.ccs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ccs.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ccs.Location = New System.Drawing.Point(132, 72)
        Me.ccs.Name = "ccs"
        Me.ccs.Size = New System.Drawing.Size(152, 21)
        Me.ccs.TabIndex = 2
        Me.ToolTip1.SetToolTip(Me.ccs, "users in this schema can see parent class")
        '
        'cgran
        '
        Me.cgran.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cgran.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cgran.Location = New System.Drawing.Point(132, 104)
        Me.cgran.Name = "cgran"
        Me.cgran.Size = New System.Drawing.Size(152, 21)
        Me.cgran.TabIndex = 3
        Me.ToolTip1.SetToolTip(Me.cgran, "Decided the number of templates to use for the contributing class based on progat" & _
                "ion of the selected entity to the entity of this class.")
        '
        'ltitle
        '
        Me.ltitle.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ltitle.Location = New System.Drawing.Point(8, 8)
        Me.ltitle.Name = "ltitle"
        Me.ltitle.Size = New System.Drawing.Size(240, 16)
        Me.ltitle.TabIndex = 83
        Me.ltitle.Text = "Enter a new contributing class"
        '
        'Cprime
        '
        Me.Cprime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Cprime.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Cprime.Location = New System.Drawing.Point(132, 186)
        Me.Cprime.Name = "Cprime"
        Me.Cprime.Size = New System.Drawing.Size(152, 21)
        Me.Cprime.TabIndex = 86
        Me.ToolTip1.SetToolTip(Me.Cprime, "Determines whether or not to contribute child entities of selected class at the s" & _
                "ame time as the parent.")
        '
        'Csec
        '
        Me.Csec.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Csec.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Csec.Location = New System.Drawing.Point(132, 229)
        Me.Csec.Name = "Csec"
        Me.Csec.Size = New System.Drawing.Size(152, 21)
        Me.Csec.TabIndex = 87
        Me.ToolTip1.SetToolTip(Me.Csec, "Determines whether or not to contribute child entities of selected class at the s" & _
                "ame time as the parent.")
        '
        'Bok
        '
        Me.Bok.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Bok.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Bok.Location = New System.Drawing.Point(122, 277)
        Me.Bok.Name = "Bok"
        Me.Bok.Size = New System.Drawing.Size(75, 23)
        Me.Bok.TabIndex = 5
        Me.Bok.Text = "Ok"
        '
        'bcancel
        '
        Me.bcancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.bcancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bcancel.Location = New System.Drawing.Point(204, 277)
        Me.bcancel.Name = "bcancel"
        Me.bcancel.Size = New System.Drawing.Size(75, 23)
        Me.bcancel.TabIndex = 6
        Me.bcancel.Text = "Cancel"
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(8, 169)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(271, 42)
        Me.Label4.TabIndex = 84
        Me.Label4.Text = "Primary User Preference Type List for Submission Security"
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(8, 211)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(256, 40)
        Me.Label5.TabIndex = 85
        Me.Label5.Text = "Secondary User Preference Type List for Submission Security"
        '
        'bc_am_in_tk_cont
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(296, 311)
        Me.Controls.Add(Me.Cprime)
        Me.Controls.Add(Me.Bok)
        Me.Controls.Add(Me.Csec)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.bcancel)
        Me.Controls.Add(Me.ltitle)
        Me.Controls.Add(Me.cgran)
        Me.Controls.Add(Me.ccs)
        Me.Controls.Add(Me.ccc)
        Me.Controls.Add(Me.ccp)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label8)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "bc_am_in_tk_cont"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Contributing Class Properties"
        Me.ResumeLayout(False)

    End Sub

#End Region
    Public cancel_selected As Boolean = True
    Private Sub bc_am_in_tk_cont_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.ccs.Enabled = False
        Me.ccc.Enabled = False
        Me.cgran.Enabled = False
        Me.bok.Enabled = False
    End Sub
    Private Sub check_apply()
        If Me.ccp.SelectedIndex > -1 And Me.ccs.SelectedIndex > -1 And Me.cgran.SelectedIndex > -1 And Me.ccc.SelectedIndex > -1 Then
            Me.bok.Enabled = True
        End If

    End Sub
    Private Sub load_class_hierarcy(ByVal child_class_id As Long)
        Dim i, j As Integer
        For i = 0 To bc_am_in_context.insight_items.class_links.Count - 1
            If bc_am_in_context.insight_items.class_links(i).schema_id = bc_am_in_context.insight_items.schema_ids(Me.ccs.SelectedIndex) And bc_am_in_context.insight_items.class_links(i).child_class_id = child_class_id Then
                For j = 0 To bc_am_in_context.insight_items.class_ids.Count - 1
                    If bc_am_in_context.insight_items.class_ids(j) = bc_am_in_context.insight_items.class_links(i).parent_class_id Then
                        Me.cgran.Items.Add(bc_am_in_context.insight_items.class_names(j))
                        Exit For
                    End If
                Next
                load_class_hierarcy(bc_am_in_context.insight_items.class_ids(j))
            End If
        Next
    End Sub
    Private Sub PictureBox2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Hide()
    End Sub

    Private Sub PictureBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        cancel_selected = False
        Me.Hide()
    End Sub

    Private Sub ccp_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ccp.SelectedIndexChanged
        If Me.ccp.SelectedIndex = -1 Then
            Exit Sub
        End If
        Me.ccc.Enabled = False
        Me.ccs.Enabled = True
        Me.ccs.SelectedIndex = -1

        Me.cgran.Enabled = False
        Me.cgran.SelectedIndex = -1
        Me.ccc.SelectedIndex = -1
        check_apply()
    End Sub
    Private Sub load_childs(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ccs.SelectedIndexChanged
        If Me.ccs.SelectedIndex = -1 Then
            Exit Sub
        End If
        Me.Cursor = Cursors.WaitCursor

        Me.ccc.Items.Clear()
        Me.ccc.Items.Add("None")
        REM load child class for schema parent entity contribution
        Dim i, j As Integer
        For i = 0 To bc_am_in_context.insight_items.class_links.Count - 1
            If bc_am_in_context.insight_items.class_links(i).schema_id = bc_am_in_context.insight_items.schema_ids(Me.ccs.SelectedIndex) And bc_am_in_context.insight_items.class_links(i).parent_class_id = bc_am_in_context.insight_items.class_ids(Me.ccp.SelectedIndex) Then
                For j = 0 To bc_am_in_context.insight_items.class_ids.Count - 1
                    If bc_am_in_context.insight_items.class_ids(j) = bc_am_in_context.insight_items.class_links(i).child_class_id Then
                        Me.ccc.Items.Add(bc_am_in_context.insight_items.class_names(j))
                        Exit For
                    End If
                Next
            End If
        Next
        REM load granularity class which can be the parent class itself or iteractive parents of class
        Me.cgran.Items.Clear()
        Me.cgran.Items.Add(Me.ccp.Text)
        Me.cgran.Items.Add("None")
        load_class_hierarcy(bc_am_in_context.insight_items.class_ids(Me.ccp.SelectedIndex))
        Me.Cursor = Cursors.Default

    End Sub
    Private Sub ccs_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ccs.SelectedIndexChanged
        If Me.ccs.SelectedIndex = -1 Then
            Exit Sub
        End If

        Me.cgran.Enabled = True
        Me.cgran.SelectedIndex = -1
        Me.ccc.Enabled = False
        Me.ccc.SelectedIndex = -1

        check_apply()
    End Sub

    Private Sub cgran_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cgran.SelectedIndexChanged
        If Me.cgran.SelectedIndex = -1 Then
            Exit Sub
        End If

        Me.ccc.Enabled = True
        Me.ccc.SelectedIndex = -1
        check_apply()
    End Sub

    Private Sub ccc_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ccc.SelectedIndexChanged
        If Me.ccc.SelectedIndex = -1 Then
            Exit Sub
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

    Private Sub Cprime_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cprime.SelectedIndexChanged
        If Cprime.SelectedIndex > 0 Then
            Me.Csec.Enabled = True
        Else
            Me.Csec.Enabled = False
            Me.Csec.SelectedIndex = 0
        End If
    End Sub

    Private Sub Label5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label5.Click

    End Sub
End Class
