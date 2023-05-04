Imports System.Windows.forms
Public Class bc_am_in_tk_format
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
    Friend WithEvents bheadfont As System.Windows.Forms.Button
    Friend WithEvents beditfont As System.Windows.Forms.Button
    Friend WithEvents bflex As System.Windows.Forms.Button
    Friend WithEvents brow As System.Windows.Forms.Button
    Friend WithEvents bsection As System.Windows.Forms.Button
    Friend WithEvents bheadvalue As System.Windows.Forms.Button
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents bheadname As System.Windows.Forms.Button
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents bheadcol As System.Windows.Forms.Button
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents bcontcol As System.Windows.Forms.Button
    Friend WithEvents beditcol As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Bapply As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_in_tk_format))
        Me.bheadfont = New System.Windows.Forms.Button()
        Me.beditfont = New System.Windows.Forms.Button()
        Me.bflex = New System.Windows.Forms.Button()
        Me.brow = New System.Windows.Forms.Button()
        Me.bsection = New System.Windows.Forms.Button()
        Me.bheadvalue = New System.Windows.Forms.Button()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.bheadname = New System.Windows.Forms.Button()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.bheadcol = New System.Windows.Forms.Button()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.bcontcol = New System.Windows.Forms.Button()
        Me.beditcol = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Bapply = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'bheadfont
        '
        Me.bheadfont.BackColor = System.Drawing.Color.Silver
        Me.bheadfont.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bheadfont.Location = New System.Drawing.Point(219, 118)
        Me.bheadfont.Name = "bheadfont"
        Me.bheadfont.Size = New System.Drawing.Size(64, 24)
        Me.bheadfont.TabIndex = 5
        Me.bheadfont.Text = "Font"
        Me.bheadfont.UseVisualStyleBackColor = False
        '
        'beditfont
        '
        Me.beditfont.BackColor = System.Drawing.Color.White
        Me.beditfont.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.beditfont.Location = New System.Drawing.Point(219, 86)
        Me.beditfont.Name = "beditfont"
        Me.beditfont.Size = New System.Drawing.Size(64, 24)
        Me.beditfont.TabIndex = 3
        Me.beditfont.Text = "Font"
        Me.beditfont.UseVisualStyleBackColor = False
        '
        'bflex
        '
        Me.bflex.BackColor = System.Drawing.Color.DarkGray
        Me.bflex.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bflex.ForeColor = System.Drawing.Color.Magenta
        Me.bflex.Location = New System.Drawing.Point(131, 278)
        Me.bflex.Name = "bflex"
        Me.bflex.Size = New System.Drawing.Size(64, 24)
        Me.bflex.TabIndex = 10
        Me.bflex.Text = "Colour"
        Me.bflex.UseVisualStyleBackColor = False
        '
        'brow
        '
        Me.brow.BackColor = System.Drawing.Color.DarkGray
        Me.brow.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.brow.Location = New System.Drawing.Point(131, 246)
        Me.brow.Name = "brow"
        Me.brow.Size = New System.Drawing.Size(64, 24)
        Me.brow.TabIndex = 9
        Me.brow.Text = "Font"
        Me.brow.UseVisualStyleBackColor = False
        '
        'bsection
        '
        Me.bsection.BackColor = System.Drawing.Color.DarkGray
        Me.bsection.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bsection.Location = New System.Drawing.Point(131, 214)
        Me.bsection.Name = "bsection"
        Me.bsection.Size = New System.Drawing.Size(64, 24)
        Me.bsection.TabIndex = 8
        Me.bsection.Text = "Font"
        Me.bsection.UseVisualStyleBackColor = False
        '
        'bheadvalue
        '
        Me.bheadvalue.BackColor = System.Drawing.Color.DarkGray
        Me.bheadvalue.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bheadvalue.Location = New System.Drawing.Point(131, 182)
        Me.bheadvalue.Name = "bheadvalue"
        Me.bheadvalue.Size = New System.Drawing.Size(64, 24)
        Me.bheadvalue.TabIndex = 7
        Me.bheadvalue.Text = "Font"
        Me.bheadvalue.UseVisualStyleBackColor = False
        '
        'Label16
        '
        Me.Label16.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.Label16.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.Location = New System.Drawing.Point(16, 94)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(96, 16)
        Me.Label16.TabIndex = 63
        Me.Label16.Text = "Editable Area:"
        '
        'Label15
        '
        Me.Label15.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.Label15.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(16, 62)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(104, 16)
        Me.Label15.TabIndex = 62
        Me.Label15.Text = "Controlled Area:"
        '
        'bheadname
        '
        Me.bheadname.BackColor = System.Drawing.Color.DarkGray
        Me.bheadname.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bheadname.Location = New System.Drawing.Point(131, 150)
        Me.bheadname.Name = "bheadname"
        Me.bheadname.Size = New System.Drawing.Size(64, 24)
        Me.bheadname.TabIndex = 6
        Me.bheadname.Text = "Font"
        Me.bheadname.UseVisualStyleBackColor = False
        '
        'Label22
        '
        Me.Label22.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.Label22.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label22.Location = New System.Drawing.Point(16, 286)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(144, 16)
        Me.Label22.TabIndex = 60
        Me.Label22.Tag = ""
        Me.Label22.Text = "Flexible Label:"
        '
        'Label21
        '
        Me.Label21.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.Label21.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label21.Location = New System.Drawing.Point(16, 254)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(144, 16)
        Me.Label21.TabIndex = 59
        Me.Label21.Text = "Row Name:"
        '
        'Label20
        '
        Me.Label20.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.Label20.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label20.Location = New System.Drawing.Point(16, 222)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(144, 16)
        Me.Label20.TabIndex = 58
        Me.Label20.Text = "Section Name:"
        '
        'Label19
        '
        Me.Label19.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.Label19.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label19.Location = New System.Drawing.Point(16, 190)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(144, 16)
        Me.Label19.TabIndex = 57
        Me.Label19.Text = "Header Value:"
        '
        'Label18
        '
        Me.Label18.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.Label18.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.Location = New System.Drawing.Point(16, 158)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(144, 16)
        Me.Label18.TabIndex = 56
        Me.Label18.Text = "Header Name:"
        '
        'bheadcol
        '
        Me.bheadcol.BackColor = System.Drawing.Color.Silver
        Me.bheadcol.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bheadcol.Location = New System.Drawing.Point(131, 118)
        Me.bheadcol.Name = "bheadcol"
        Me.bheadcol.Size = New System.Drawing.Size(64, 24)
        Me.bheadcol.TabIndex = 4
        Me.bheadcol.Text = "Colour"
        Me.bheadcol.UseVisualStyleBackColor = False
        '
        'Label17
        '
        Me.Label17.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.Label17.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.Location = New System.Drawing.Point(16, 126)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(144, 16)
        Me.Label17.TabIndex = 54
        Me.Label17.Text = "Column Heading:"
        '
        'bcontcol
        '
        Me.bcontcol.BackColor = System.Drawing.Color.DarkGray
        Me.bcontcol.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bcontcol.Location = New System.Drawing.Point(131, 54)
        Me.bcontcol.Name = "bcontcol"
        Me.bcontcol.Size = New System.Drawing.Size(64, 24)
        Me.bcontcol.TabIndex = 1
        Me.bcontcol.Text = "Colour"
        Me.bcontcol.UseVisualStyleBackColor = False
        '
        'beditcol
        '
        Me.beditcol.BackColor = System.Drawing.Color.White
        Me.beditcol.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.beditcol.Location = New System.Drawing.Point(131, 86)
        Me.beditcol.Name = "beditcol"
        Me.beditcol.Size = New System.Drawing.Size(64, 24)
        Me.beditcol.TabIndex = 2
        Me.beditcol.Text = "Colour"
        Me.beditcol.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(17, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(208, 24)
        Me.Label1.TabIndex = 71
        Me.Label1.Text = "Create format sheet"
        '
        'Button1
        '
        Me.Button1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.Location = New System.Drawing.Point(208, 327)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 12
        Me.Button1.Text = "Cancel"
        '
        'Bapply
        '
        Me.Bapply.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Bapply.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Bapply.Location = New System.Drawing.Point(128, 327)
        Me.Bapply.Name = "Bapply"
        Me.Bapply.Size = New System.Drawing.Size(75, 23)
        Me.Bapply.TabIndex = 11
        Me.Bapply.Text = "Ok"
        '
        'bc_am_in_tk_format
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(294, 356)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Bapply)
        Me.Controls.Add(Me.bheadcol)
        Me.Controls.Add(Me.beditcol)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.bheadfont)
        Me.Controls.Add(Me.beditfont)
        Me.Controls.Add(Me.bflex)
        Me.Controls.Add(Me.brow)
        Me.Controls.Add(Me.bsection)
        Me.Controls.Add(Me.bheadvalue)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.Label15)
        Me.Controls.Add(Me.bheadname)
        Me.Controls.Add(Me.Label22)
        Me.Controls.Add(Me.Label21)
        Me.Controls.Add(Me.Label20)
        Me.Controls.Add(Me.Label19)
        Me.Controls.Add(Me.Label18)
        Me.Controls.Add(Me.Label17)
        Me.Controls.Add(Me.bcontcol)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "bc_am_in_tk_format"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Create Format Sheet Properties"
        Me.ResumeLayout(False)

    End Sub

#End Region
    Public cancel_selected As Boolean = True
    Private Sub bcancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Hide()
    End Sub

    Private Sub bcreatesheet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.cancel_selected = False
        Me.Hide()
    End Sub

    Private Sub bcontcol_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bcontcol.Click
        Dim dialog As New ColorDialog
        dialog.ShowDialog()
        Me.bcontcol.BackColor = dialog.Color
        Me.bheadname.BackColor = Me.bcontcol.BackColor
        Me.bheadvalue.BackColor = Me.bcontcol.BackColor
        Me.bsection.BackColor = Me.bcontcol.BackColor
        Me.brow.BackColor = Me.bcontcol.BackColor
        Me.bflex.BackColor = Me.bcontcol.BackColor
    End Sub

    Private Sub bc_am_in_tk_format_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub beditcol_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles beditcol.Click
        Dim dialog As New ColorDialog
        dialog.ShowDialog()
        Me.beditfont.BackColor = dialog.Color
        Me.beditcol.BackColor = dialog.Color

    End Sub

    Private Sub bheadcol_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bheadcol.Click
        Dim dialog As New ColorDialog
        dialog.ShowDialog()
        Me.bheadfont.BackColor = dialog.Color
        Me.bheadcol.BackColor = dialog.Color
    End Sub

    Private Sub bheadname_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bheadname.Click
        Dim dialog As New FontDialog
        dialog.ShowDialog()
        bheadname.Font = dialog.Font
    End Sub

    Private Sub bheadvalue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bheadvalue.Click
        Dim dialog As New FontDialog
        dialog.ShowDialog()
        bheadvalue.Font = dialog.Font
    End Sub

    Private Sub bsection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bsection.Click
        Dim dialog As New FontDialog
        dialog.ShowDialog()
        bsection.Font = dialog.Font
    End Sub

    Private Sub brow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles brow.Click
        Dim dialog As New FontDialog
        dialog.ShowDialog()
        brow.Font = dialog.Font
        bflex.BackColor = brow.BackColor
        bflex.Font = brow.Font
    End Sub

    Private Sub bflex_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bflex.Click
        Dim dialog As New ColorDialog
        dialog.ShowDialog()
        bflex.ForeColor = dialog.Color
    End Sub

    Private Sub beditfont_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles beditfont.Click
        Dim dialog As New FontDialog
        dialog.ShowDialog()
        Me.beditfont.Font = dialog.Font
    End Sub

    Private Sub bheadfont_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bheadfont.Click
        Dim dialog As New FontDialog
        dialog.ShowDialog()
        Me.bheadfont.Font = dialog.Font
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Hide()
    End Sub

    Private Sub Bapply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Bapply.Click
        Me.cancel_selected = False
        Me.Hide()
    End Sub
End Class
