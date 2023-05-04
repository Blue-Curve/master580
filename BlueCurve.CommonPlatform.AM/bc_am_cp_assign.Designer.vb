<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bc_am_cp_assign
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_cp_assign))
        Me.ltitle = New System.Windows.Forms.GroupBox()
        Me.BlueCurve_TextSearch1 = New BlueCurve.Core.[AS].BlueCurve_TextSearch()
        Me.bedit = New System.Windows.Forms.Button()
        Me.bnew = New System.Windows.Forms.Button()
        Me.Clist = New System.Windows.Forms.ComboBox()
        Me.Tnew = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Bok = New System.Windows.Forms.Button()
        Me.Bcancel = New System.Windows.Forms.Button()
        Me.tlfilter = New System.Windows.Forms.TextBox()
        Me.lselparent = New System.Windows.Forms.ListBox()
        Me.lallparent = New System.Windows.Forms.ListBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.bdn = New System.Windows.Forms.Button()
        Me.bup = New System.Windows.Forms.Button()
        Me.llfilter = New System.Windows.Forms.PictureBox()
        Me.Bdel = New System.Windows.Forms.Button()
        Me.Badd = New System.Windows.Forms.Button()
        Me.ltitle.SuspendLayout()
        CType(Me.llfilter, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ltitle
        '
        Me.ltitle.Controls.Add(Me.BlueCurve_TextSearch1)
        Me.ltitle.Controls.Add(Me.bedit)
        Me.ltitle.Controls.Add(Me.bnew)
        Me.ltitle.Controls.Add(Me.Clist)
        Me.ltitle.Controls.Add(Me.bdn)
        Me.ltitle.Controls.Add(Me.bup)
        Me.ltitle.Controls.Add(Me.Tnew)
        Me.ltitle.Controls.Add(Me.Label2)
        Me.ltitle.Controls.Add(Me.Bok)
        Me.ltitle.Controls.Add(Me.Bcancel)
        Me.ltitle.Controls.Add(Me.tlfilter)
        Me.ltitle.Controls.Add(Me.llfilter)
        Me.ltitle.Controls.Add(Me.Bdel)
        Me.ltitle.Controls.Add(Me.Badd)
        Me.ltitle.Controls.Add(Me.lselparent)
        Me.ltitle.Controls.Add(Me.lallparent)
        Me.ltitle.Controls.Add(Me.Label1)
        Me.ltitle.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ltitle.Location = New System.Drawing.Point(6, 4)
        Me.ltitle.Name = "ltitle"
        Me.ltitle.Size = New System.Drawing.Size(521, 311)
        Me.ltitle.TabIndex = 0
        Me.ltitle.TabStop = False
        Me.ltitle.Text = "GroupBox1"
        '
        'BlueCurve_TextSearch1
        '
        Me.BlueCurve_TextSearch1.BackColor = System.Drawing.Color.Transparent
        Me.BlueCurve_TextSearch1.ExcludeControl = Nothing
        Me.BlueCurve_TextSearch1.Location = New System.Drawing.Point(17, 256)
        Me.BlueCurve_TextSearch1.MaximumSize = New System.Drawing.Size(35000, 30000)
        Me.BlueCurve_TextSearch1.MinimumSize = New System.Drawing.Size(175, 21)
        Me.BlueCurve_TextSearch1.Name = "BlueCurve_TextSearch1"
        Me.BlueCurve_TextSearch1.SearchAttributeList = ""
        Me.BlueCurve_TextSearch1.SearchAttributes = 2
        Me.BlueCurve_TextSearch1.SearchBuildEntitiesOnly = False
        Me.BlueCurve_TextSearch1.SearchClass = 0
        Me.BlueCurve_TextSearch1.SearchControl = "lallparent"
        Me.BlueCurve_TextSearch1.SearchCurrentAttribute = Nothing
        Me.BlueCurve_TextSearch1.SearchFireEventOnly = False
        Me.BlueCurve_TextSearch1.SearchGetInactive = True
        Me.BlueCurve_TextSearch1.SearchText = ""
        Me.BlueCurve_TextSearch1.SearchTimerOff = False
        Me.BlueCurve_TextSearch1.SearchUserEntitiesOnly = False
        Me.BlueCurve_TextSearch1.showinactive = -1
        Me.BlueCurve_TextSearch1.Size = New System.Drawing.Size(236, 28)
        Me.BlueCurve_TextSearch1.TabIndex = 73
        Me.BlueCurve_TextSearch1.Visible = False
        '
        'bedit
        '
        Me.bedit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bedit.Enabled = False
        Me.bedit.Location = New System.Drawing.Point(107, 282)
        Me.bedit.Name = "bedit"
        Me.bedit.Size = New System.Drawing.Size(75, 23)
        Me.bedit.TabIndex = 72
        Me.bedit.Text = "Edit"
        Me.bedit.UseVisualStyleBackColor = True
        Me.bedit.Visible = False
        '
        'bnew
        '
        Me.bnew.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnew.Location = New System.Drawing.Point(17, 282)
        Me.bnew.Name = "bnew"
        Me.bnew.Size = New System.Drawing.Size(75, 23)
        Me.bnew.TabIndex = 71
        Me.bnew.Text = "New"
        Me.bnew.UseVisualStyleBackColor = True
        Me.bnew.Visible = False
        '
        'Clist
        '
        Me.Clist.FormattingEnabled = True
        Me.Clist.Location = New System.Drawing.Point(17, 39)
        Me.Clist.Name = "Clist"
        Me.Clist.Size = New System.Drawing.Size(236, 21)
        Me.Clist.TabIndex = 70
        Me.Clist.Visible = False
        '
        'Tnew
        '
        Me.Tnew.Location = New System.Drawing.Point(17, 39)
        Me.Tnew.MaxLength = 250
        Me.Tnew.Name = "Tnew"
        Me.Tnew.Size = New System.Drawing.Size(236, 21)
        Me.Tnew.TabIndex = 67
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(14, 21)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(32, 15)
        Me.Label2.TabIndex = 66
        Me.Label2.Text = "New"
        '
        'Bok
        '
        Me.Bok.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Bok.Enabled = False
        Me.Bok.Location = New System.Drawing.Point(323, 282)
        Me.Bok.Name = "Bok"
        Me.Bok.Size = New System.Drawing.Size(75, 23)
        Me.Bok.TabIndex = 64
        Me.Bok.Text = "Ok"
        Me.Bok.UseVisualStyleBackColor = True
        '
        'Bcancel
        '
        Me.Bcancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Bcancel.Location = New System.Drawing.Point(404, 282)
        Me.Bcancel.Name = "Bcancel"
        Me.Bcancel.Size = New System.Drawing.Size(75, 23)
        Me.Bcancel.TabIndex = 63
        Me.Bcancel.Text = "Cancel"
        Me.Bcancel.UseVisualStyleBackColor = True
        '
        'tlfilter
        '
        Me.tlfilter.Location = New System.Drawing.Point(51, 256)
        Me.tlfilter.Name = "tlfilter"
        Me.tlfilter.Size = New System.Drawing.Size(202, 21)
        Me.tlfilter.TabIndex = 61
        '
        'lselparent
        '
        Me.lselparent.AllowDrop = True
        Me.lselparent.FormattingEnabled = True
        Me.lselparent.Location = New System.Drawing.Point(293, 39)
        Me.lselparent.Name = "lselparent"
        Me.lselparent.Size = New System.Drawing.Size(186, 238)
        Me.lselparent.TabIndex = 57
        '
        'lallparent
        '
        Me.lallparent.AllowDrop = True
        Me.lallparent.FormattingEnabled = True
        Me.lallparent.Location = New System.Drawing.Point(17, 89)
        Me.lallparent.Name = "lallparent"
        Me.lallparent.Size = New System.Drawing.Size(236, 160)
        Me.lallparent.Sorted = True
        Me.lallparent.TabIndex = 56
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(14, 73)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(51, 13)
        Me.Label1.TabIndex = 65
        Me.Label1.Text = "Existing"
        '
        'bdn
        '
        Me.bdn.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bdn.Enabled = False
        Me.bdn.Image = CType(resources.GetObject("bdn.Image"), System.Drawing.Image)
        Me.bdn.Location = New System.Drawing.Point(485, 167)
        Me.bdn.Name = "bdn"
        Me.bdn.Size = New System.Drawing.Size(28, 24)
        Me.bdn.TabIndex = 69
        Me.bdn.UseVisualStyleBackColor = True
        Me.bdn.Visible = False
        '
        'bup
        '
        Me.bup.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bup.Enabled = False
        Me.bup.Image = CType(resources.GetObject("bup.Image"), System.Drawing.Image)
        Me.bup.Location = New System.Drawing.Point(485, 137)
        Me.bup.Name = "bup"
        Me.bup.Size = New System.Drawing.Size(28, 24)
        Me.bup.TabIndex = 68
        Me.bup.UseVisualStyleBackColor = True
        Me.bup.Visible = False
        '
        'llfilter
        '
        Me.llfilter.Image = CType(resources.GetObject("llfilter.Image"), System.Drawing.Image)
        Me.llfilter.Location = New System.Drawing.Point(17, 255)
        Me.llfilter.Name = "llfilter"
        Me.llfilter.Size = New System.Drawing.Size(28, 28)
        Me.llfilter.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.llfilter.TabIndex = 60
        Me.llfilter.TabStop = False
        Me.llfilter.WaitOnLoad = True
        '
        'Bdel
        '
        Me.Bdel.Enabled = False
        Me.Bdel.Image = CType(resources.GetObject("Bdel.Image"), System.Drawing.Image)
        Me.Bdel.Location = New System.Drawing.Point(259, 167)
        Me.Bdel.Name = "Bdel"
        Me.Bdel.Size = New System.Drawing.Size(28, 24)
        Me.Bdel.TabIndex = 59
        Me.Bdel.UseVisualStyleBackColor = True
        '
        'Badd
        '
        Me.Badd.Enabled = False
        Me.Badd.Image = CType(resources.GetObject("Badd.Image"), System.Drawing.Image)
        Me.Badd.Location = New System.Drawing.Point(259, 137)
        Me.Badd.Name = "Badd"
        Me.Badd.Size = New System.Drawing.Size(28, 24)
        Me.Badd.TabIndex = 58
        Me.Badd.UseVisualStyleBackColor = True
        '
        'bc_am_cp_assign
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(530, 319)
        Me.ControlBox = False
        Me.Controls.Add(Me.ltitle)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "bc_am_cp_assign"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Assign"
        Me.ltitle.ResumeLayout(False)
        Me.ltitle.PerformLayout()
        CType(Me.llfilter, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ltitle As System.Windows.Forms.GroupBox
    Friend WithEvents Clist As System.Windows.Forms.ComboBox
    Friend WithEvents bdn As System.Windows.Forms.Button
    Friend WithEvents bup As System.Windows.Forms.Button
    Friend WithEvents Tnew As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Bok As System.Windows.Forms.Button
    Friend WithEvents Bcancel As System.Windows.Forms.Button
    Friend WithEvents tlfilter As System.Windows.Forms.TextBox
    Friend WithEvents llfilter As System.Windows.Forms.PictureBox
    Friend WithEvents Bdel As System.Windows.Forms.Button
    Friend WithEvents Badd As System.Windows.Forms.Button
    Friend WithEvents lselparent As System.Windows.Forms.ListBox
    Friend WithEvents lallparent As System.Windows.Forms.ListBox
    Friend WithEvents bnew As System.Windows.Forms.Button
    Friend WithEvents bedit As System.Windows.Forms.Button
    Friend WithEvents BlueCurve_TextSearch1 As BlueCurve.Core.AS.BlueCurve_TextSearch
End Class
