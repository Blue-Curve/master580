<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bc_am_cp_deploy
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
        Me.components = New System.ComponentModel.Container
        Me.treleases = New System.Windows.Forms.TreeView
        Me.releasemenustrip = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mcr = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripTextBox1 = New System.Windows.Forms.ToolStripTextBox
        Me.mreldesc = New System.Windows.Forms.ToolStripTextBox
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.OkToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.CancelToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.mdep = New System.Windows.Forms.ToolStripMenuItem
        Me.mdel = New System.Windows.Forms.ToolStripMenuItem
        Me.mset = New System.Windows.Forms.ToolStripMenuItem
        Me.ListView1 = New System.Windows.Forms.ListView
        Me.releasemenustrip.SuspendLayout()
        Me.SuspendLayout()
        '
        'treleases
        '
        Me.treleases.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.treleases.ContextMenuStrip = Me.releasemenustrip
        Me.treleases.Location = New System.Drawing.Point(12, 9)
        Me.treleases.Name = "treleases"
        Me.treleases.Size = New System.Drawing.Size(208, 356)
        Me.treleases.TabIndex = 0
        '
        'releasemenustrip
        '
        Me.releasemenustrip.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.releasemenustrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mcr, Me.mdep, Me.mdel, Me.mset})
        Me.releasemenustrip.Name = "releasemenustrip"
        Me.releasemenustrip.Size = New System.Drawing.Size(275, 92)
        '
        'mcr
        '
        Me.mcr.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripTextBox1, Me.mreldesc, Me.ToolStripSeparator1, Me.OkToolStripMenuItem, Me.CancelToolStripMenuItem})
        Me.mcr.Name = "mcr"
        Me.mcr.Size = New System.Drawing.Size(274, 22)
        Me.mcr.Text = "Create New Working Release"
        '
        'ToolStripTextBox1
        '
        Me.ToolStripTextBox1.Enabled = False
        Me.ToolStripTextBox1.Name = "ToolStripTextBox1"
        Me.ToolStripTextBox1.Size = New System.Drawing.Size(200, 21)
        Me.ToolStripTextBox1.Text = "Enter Description"
        '
        'mreldesc
        '
        Me.mreldesc.MaxLength = 250
        Me.mreldesc.Name = "mreldesc"
        Me.mreldesc.Size = New System.Drawing.Size(200, 21)
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(257, 6)
        '
        'OkToolStripMenuItem
        '
        Me.OkToolStripMenuItem.Name = "OkToolStripMenuItem"
        Me.OkToolStripMenuItem.Size = New System.Drawing.Size(260, 22)
        Me.OkToolStripMenuItem.Text = "Ok"
        '
        'CancelToolStripMenuItem
        '
        Me.CancelToolStripMenuItem.Name = "CancelToolStripMenuItem"
        Me.CancelToolStripMenuItem.Size = New System.Drawing.Size(260, 22)
        Me.CancelToolStripMenuItem.Text = "Cancel"
        '
        'mdep
        '
        Me.mdep.Name = "mdep"
        Me.mdep.Size = New System.Drawing.Size(274, 22)
        Me.mdep.Text = "Deploy Working Release"
        '
        'mdel
        '
        Me.mdel.Name = "mdel"
        Me.mdel.Size = New System.Drawing.Size(274, 22)
        Me.mdel.Text = "Delete Working Release"
        '
        'mset
        '
        Me.mset.Name = "mset"
        Me.mset.Size = New System.Drawing.Size(274, 22)
        Me.mset.Text = "Set Previous Release as Working"
        '
        'ListView1
        '
        Me.ListView1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListView1.Location = New System.Drawing.Point(242, 9)
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(473, 197)
        Me.ListView1.TabIndex = 1
        Me.ListView1.UseCompatibleStateImageBehavior = False
        '
        'bc_am_cp_deploy
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(737, 380)
        Me.ControlBox = False
        Me.Controls.Add(Me.ListView1)
        Me.Controls.Add(Me.treleases)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "bc_am_cp_deploy"
        Me.Text = "bc_am_cp_deploy"
        Me.releasemenustrip.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents treleases As System.Windows.Forms.TreeView
    Friend WithEvents ListView1 As System.Windows.Forms.ListView
    Friend WithEvents releasemenustrip As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mcr As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripTextBox1 As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents mreldesc As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents OkToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CancelToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mdep As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mdel As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mset As System.Windows.Forms.ToolStripMenuItem
End Class
