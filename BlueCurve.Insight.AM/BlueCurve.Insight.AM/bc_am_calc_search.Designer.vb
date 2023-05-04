<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bc_am_calc_search
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_calc_search))
        Me.tsearch = New System.Windows.Forms.TextBox
        Me.uxPFilter = New System.Windows.Forms.PictureBox
        CType(Me.uxPFilter, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'tsearch
        '
        Me.tsearch.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tsearch.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tsearch.Location = New System.Drawing.Point(40, 7)
        Me.tsearch.MaxLength = 250
        Me.tsearch.Name = "tsearch"
        Me.tsearch.Size = New System.Drawing.Size(176, 21)
        Me.tsearch.TabIndex = 0
        '
        'uxPFilter
        '
        Me.uxPFilter.Enabled = False
        Me.uxPFilter.Image = CType(resources.GetObject("uxPFilter.Image"), System.Drawing.Image)
        Me.uxPFilter.Location = New System.Drawing.Point(3, 3)
        Me.uxPFilter.Name = "uxPFilter"
        Me.uxPFilter.Size = New System.Drawing.Size(31, 30)
        Me.uxPFilter.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.uxPFilter.TabIndex = 63
        Me.uxPFilter.TabStop = False
        '
        'bc_am_calc_search
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.uxPFilter)
        Me.Controls.Add(Me.tsearch)
        Me.Name = "bc_am_calc_search"
        Me.Size = New System.Drawing.Size(236, 35)
        CType(Me.uxPFilter, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents tsearch As System.Windows.Forms.TextBox
    Friend WithEvents uxPFilter As System.Windows.Forms.PictureBox

End Class
