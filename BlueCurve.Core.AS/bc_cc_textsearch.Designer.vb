<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BlueCurve_TextSearch
    Inherits System.Windows.Forms.UserControl

    'UserControl1 overrides dispose to clean up the component list.
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(BlueCurve_TextSearch))
        Me.SearchTimer = New System.Windows.Forms.Timer(Me.components)
        Me.uxTextFilter = New System.Windows.Forms.TextBox
        Me.uxPFilter = New System.Windows.Forms.PictureBox
        Me.uxAttribSelect = New System.Windows.Forms.ComboBox
        CType(Me.uxPFilter, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'SearchTimer
        '
        Me.SearchTimer.Interval = 1000
        '
        'uxTextFilter
        '
        Me.uxTextFilter.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxTextFilter.BackColor = System.Drawing.SystemColors.Window
        Me.uxTextFilter.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxTextFilter.Location = New System.Drawing.Point(243, 0)
        Me.uxTextFilter.MaximumSize = New System.Drawing.Size(9999999, 21)
        Me.uxTextFilter.MinimumSize = New System.Drawing.Size(50, 21)
        Me.uxTextFilter.Multiline = True
        Me.uxTextFilter.Name = "uxTextFilter"
        Me.uxTextFilter.Size = New System.Drawing.Size(256, 21)
        Me.uxTextFilter.TabIndex = 2
        '
        'uxPFilter
        '
        Me.uxPFilter.Enabled = False
        Me.uxPFilter.Image = CType(resources.GetObject("uxPFilter.Image"), System.Drawing.Image)
        Me.uxPFilter.Location = New System.Drawing.Point(0, 0)
        Me.uxPFilter.Name = "uxPFilter"
        Me.uxPFilter.Size = New System.Drawing.Size(28, 23)
        Me.uxPFilter.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.uxPFilter.TabIndex = 62
        Me.uxPFilter.TabStop = False
        '
        'uxAttribSelect
        '
        Me.uxAttribSelect.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxAttribSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.uxAttribSelect.FormattingEnabled = True
        Me.uxAttribSelect.Location = New System.Drawing.Point(37, 0)
        Me.uxAttribSelect.MaximumSize = New System.Drawing.Size(200, 0)
        Me.uxAttribSelect.MinimumSize = New System.Drawing.Size(100, 0)
        Me.uxAttribSelect.Name = "uxAttribSelect"
        Me.uxAttribSelect.Size = New System.Drawing.Size(200, 21)
        Me.uxAttribSelect.TabIndex = 1
        '
        'BlueCurve_TextSearch
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Transparent
        Me.Controls.Add(Me.uxAttribSelect)
        Me.Controls.Add(Me.uxPFilter)
        Me.Controls.Add(Me.uxTextFilter)
        Me.MaximumSize = New System.Drawing.Size(30000, 30000)
        Me.MinimumSize = New System.Drawing.Size(150, 21)
        Me.Name = "BlueCurve_TextSearch"
        Me.Size = New System.Drawing.Size(499, 28)
        CType(Me.uxPFilter, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents SearchTimer As System.Windows.Forms.Timer
    Public WithEvents uxTextFilter As System.Windows.Forms.TextBox
    Friend WithEvents uxPFilter As System.Windows.Forms.PictureBox
    Friend WithEvents uxAttribSelect As System.Windows.Forms.ComboBox

End Class
