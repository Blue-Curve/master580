<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bc_am_excel_data_load
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
        Me.pwarnings = New System.Windows.Forms.Panel
        Me.bgoto = New System.Windows.Forms.Button
        Me.bclose = New System.Windows.Forms.Button
        Me.Lvwarnings = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader5 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader4 = New System.Windows.Forms.ColumnHeader
        Me.Button2 = New System.Windows.Forms.Button
        Me.Button1 = New System.Windows.Forms.Button
        Me.ptemplate = New System.Windows.Forms.Panel
        Me.Ltemplate = New System.Windows.Forms.ListBox
        Me.baction = New System.Windows.Forms.Button
        Me.Button3 = New System.Windows.Forms.Button
        Me.pwarnings.SuspendLayout()
        Me.ptemplate.SuspendLayout()
        Me.SuspendLayout()
        '
        'pwarnings
        '
        Me.pwarnings.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pwarnings.Controls.Add(Me.bgoto)
        Me.pwarnings.Controls.Add(Me.bclose)
        Me.pwarnings.Controls.Add(Me.Lvwarnings)
        Me.pwarnings.Controls.Add(Me.Button2)
        Me.pwarnings.Controls.Add(Me.Button1)
        Me.pwarnings.Location = New System.Drawing.Point(2, 12)
        Me.pwarnings.Name = "pwarnings"
        Me.pwarnings.Size = New System.Drawing.Size(570, 440)
        Me.pwarnings.TabIndex = 0
        '
        'bgoto
        '
        Me.bgoto.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bgoto.Enabled = False
        Me.bgoto.Location = New System.Drawing.Point(493, 408)
        Me.bgoto.Name = "bgoto"
        Me.bgoto.Size = New System.Drawing.Size(74, 29)
        Me.bgoto.TabIndex = 6
        Me.bgoto.Text = "GoTo"
        Me.bgoto.UseVisualStyleBackColor = True
        '
        'bclose
        '
        Me.bclose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bclose.Location = New System.Drawing.Point(413, 408)
        Me.bclose.Name = "bclose"
        Me.bclose.Size = New System.Drawing.Size(74, 29)
        Me.bclose.TabIndex = 5
        Me.bclose.Text = "Close"
        Me.bclose.UseVisualStyleBackColor = True
        '
        'Lvwarnings
        '
        Me.Lvwarnings.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Lvwarnings.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader5, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4})
        Me.Lvwarnings.FullRowSelect = True
        Me.Lvwarnings.HideSelection = False
        Me.Lvwarnings.Location = New System.Drawing.Point(3, 3)
        Me.Lvwarnings.MultiSelect = False
        Me.Lvwarnings.Name = "Lvwarnings"
        Me.Lvwarnings.Size = New System.Drawing.Size(564, 391)
        Me.Lvwarnings.TabIndex = 2
        Me.Lvwarnings.UseCompatibleStateImageBehavior = False
        Me.Lvwarnings.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Worksheet"
        Me.ColumnHeader1.Width = 105
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "Type"
        Me.ColumnHeader5.Width = 84
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Row"
        Me.ColumnHeader2.Width = 43
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Col"
        Me.ColumnHeader3.Width = 54
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Warning"
        Me.ColumnHeader4.Width = 348
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(342, 343)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(74, 29)
        Me.Button2.TabIndex = 1
        Me.Button2.Text = "Close"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(422, 343)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(74, 29)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "GoTo"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'ptemplate
        '
        Me.ptemplate.Controls.Add(Me.Ltemplate)
        Me.ptemplate.Controls.Add(Me.baction)
        Me.ptemplate.Controls.Add(Me.Button3)
        Me.ptemplate.Location = New System.Drawing.Point(2, 1)
        Me.ptemplate.Name = "ptemplate"
        Me.ptemplate.Size = New System.Drawing.Size(311, 199)
        Me.ptemplate.TabIndex = 1
        '
        'Ltemplate
        '
        Me.Ltemplate.FormattingEnabled = True
        Me.Ltemplate.Location = New System.Drawing.Point(3, 8)
        Me.Ltemplate.Name = "Ltemplate"
        Me.Ltemplate.Size = New System.Drawing.Size(299, 147)
        Me.Ltemplate.TabIndex = 4
        '
        'baction
        '
        Me.baction.Enabled = False
        Me.baction.Location = New System.Drawing.Point(228, 159)
        Me.baction.Name = "baction"
        Me.baction.Size = New System.Drawing.Size(74, 29)
        Me.baction.TabIndex = 3
        Me.baction.Text = "Upload"
        Me.baction.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(148, 159)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(74, 29)
        Me.Button3.TabIndex = 2
        Me.Button3.Text = "Close"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'bc_am_excel_data_load
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(575, 464)
        Me.Controls.Add(Me.pwarnings)
        Me.Controls.Add(Me.ptemplate)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.MaximizeBox = False
        Me.Name = "bc_am_excel_data_load"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Excel Data I/O - Blue Curve"
        Me.pwarnings.ResumeLayout(False)
        Me.ptemplate.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pwarnings As System.Windows.Forms.Panel
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents ptemplate As System.Windows.Forms.Panel
    Friend WithEvents baction As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Ltemplate As System.Windows.Forms.ListBox
    Friend WithEvents Lvwarnings As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents bgoto As System.Windows.Forms.Button
    Friend WithEvents bclose As System.Windows.Forms.Button
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
End Class
