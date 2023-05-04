<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bc_am_user_defined_components
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_user_defined_components))
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.breload = New System.Windows.Forms.Button
        Me.rpublic = New System.Windows.Forms.RadioButton
        Me.rprivate = New System.Windows.Forms.RadioButton
        Me.Label2 = New System.Windows.Forms.Label
        Me.bsave = New System.Windows.Forms.Button
        Me.Button4 = New System.Windows.Forms.Button
        Me.PictureBox2 = New System.Windows.Forms.PictureBox
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.TabPage2 = New System.Windows.Forms.TabPage
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.Cwrap = New System.Windows.Forms.CheckBox
        Me.Ltitle = New System.Windows.Forms.Label
        Me.Ttitle = New System.Windows.Forms.TextBox
        Me.tpreview = New System.Windows.Forms.RichTextBox
        Me.Lcomps = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader4 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader5 = New System.Windows.Forms.ColumnHeader
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Cauthor = New System.Windows.Forms.ComboBox
        Me.Centity = New System.Windows.Forms.ComboBox
        Me.Cpubtype = New System.Windows.Forms.ComboBox
        Me.Dto = New System.Windows.Forms.DateTimePicker
        Me.Dfrom = New System.Windows.Forms.DateTimePicker
        Me.ColumnHeader6 = New System.Windows.Forms.ColumnHeader
        Me.Panel2.SuspendLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControl1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel2
        '
        Me.Panel2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel2.BackColor = System.Drawing.Color.White
        Me.Panel2.Controls.Add(Me.Label2)
        Me.Panel2.Controls.Add(Me.PictureBox2)
        Me.Panel2.Controls.Add(Me.breload)
        Me.Panel2.Controls.Add(Me.rpublic)
        Me.Panel2.Controls.Add(Me.rprivate)
        Me.Panel2.Controls.Add(Me.bsave)
        Me.Panel2.Controls.Add(Me.Button4)
        Me.Panel2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Panel2.Location = New System.Drawing.Point(-24, 400)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(636, 52)
        Me.Panel2.TabIndex = 29
        '
        'breload
        '
        Me.breload.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.breload.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.breload.Location = New System.Drawing.Point(371, 26)
        Me.breload.Name = "breload"
        Me.breload.Size = New System.Drawing.Size(80, 23)
        Me.breload.TabIndex = 32
        Me.breload.Text = "Reload"
        Me.breload.UseVisualStyleBackColor = True
        '
        'rpublic
        '
        Me.rpublic.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rpublic.AutoSize = True
        Me.rpublic.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rpublic.Location = New System.Drawing.Point(570, 3)
        Me.rpublic.Name = "rpublic"
        Me.rpublic.Size = New System.Drawing.Size(58, 17)
        Me.rpublic.TabIndex = 31
        Me.rpublic.Text = "Public"
        Me.rpublic.UseVisualStyleBackColor = True
        '
        'rprivate
        '
        Me.rprivate.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rprivate.AutoSize = True
        Me.rprivate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rprivate.Location = New System.Drawing.Point(485, 3)
        Me.rprivate.Name = "rprivate"
        Me.rprivate.Size = New System.Drawing.Size(65, 17)
        Me.rprivate.TabIndex = 30
        Me.rprivate.Text = "Private"
        Me.rprivate.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(29, 36)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(216, 14)
        Me.Label2.TabIndex = 29
        Me.Label2.Text = "© Copyright Corero Systems Ltd."
        '
        'bsave
        '
        Me.bsave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bsave.Enabled = False
        Me.bsave.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bsave.Location = New System.Drawing.Point(548, 26)
        Me.bsave.Name = "bsave"
        Me.bsave.Size = New System.Drawing.Size(80, 23)
        Me.bsave.TabIndex = 17
        Me.bsave.Text = "Save"
        Me.bsave.UseVisualStyleBackColor = True
        '
        'Button4
        '
        Me.Button4.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button4.Location = New System.Drawing.Point(461, 26)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(80, 23)
        Me.Button4.TabIndex = 21
        Me.Button4.Text = "Close"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'PictureBox2
        '
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(30, 3)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(184, 36)
        Me.PictureBox2.TabIndex = 27
        Me.PictureBox2.TabStop = False
        '
        'TabControl1
        '
        Me.TabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabControl1.Location = New System.Drawing.Point(6, 2)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(598, 392)
        Me.TabControl1.TabIndex = 30
        '
        'TabPage2
        '
        Me.TabPage2.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.TabPage2.Controls.Add(Me.Label8)
        Me.TabPage2.Controls.Add(Me.Label7)
        Me.TabPage2.Controls.Add(Me.Label5)
        Me.TabPage2.Controls.Add(Me.Label4)
        Me.TabPage2.Controls.Add(Me.Label3)
        Me.TabPage2.Controls.Add(Me.Cauthor)
        Me.TabPage2.Controls.Add(Me.Centity)
        Me.TabPage2.Controls.Add(Me.Cpubtype)
        Me.TabPage2.Controls.Add(Me.Dto)
        Me.TabPage2.Controls.Add(Me.Dfrom)
        Me.TabPage2.Controls.Add(Me.Lcomps)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(590, 366)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Component List"
        '
        'TabPage1
        '
        Me.TabPage1.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.TabPage1.Controls.Add(Me.Cwrap)
        Me.TabPage1.Controls.Add(Me.Ltitle)
        Me.TabPage1.Controls.Add(Me.Ttitle)
        Me.TabPage1.Controls.Add(Me.tpreview)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(590, 366)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Preview"
        '
        'Cwrap
        '
        Me.Cwrap.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Cwrap.AutoSize = True
        Me.Cwrap.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Cwrap.Location = New System.Drawing.Point(536, 344)
        Me.Cwrap.Name = "Cwrap"
        Me.Cwrap.Size = New System.Drawing.Size(56, 17)
        Me.Cwrap.TabIndex = 7
        Me.Cwrap.Text = "Wrap"
        Me.Cwrap.UseVisualStyleBackColor = True
        '
        'Ltitle
        '
        Me.Ltitle.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Ltitle.AutoSize = True
        Me.Ltitle.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Ltitle.Location = New System.Drawing.Point(6, 345)
        Me.Ltitle.Name = "Ltitle"
        Me.Ltitle.Size = New System.Drawing.Size(121, 13)
        Me.Ltitle.TabIndex = 6
        Me.Ltitle.Text = "Component Name"
        '
        'Ttitle
        '
        Me.Ttitle.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Ttitle.Location = New System.Drawing.Point(133, 339)
        Me.Ttitle.MaxLength = 250
        Me.Ttitle.Name = "Ttitle"
        Me.Ttitle.Size = New System.Drawing.Size(348, 21)
        Me.Ttitle.TabIndex = 5
        '
        'tpreview
        '
        Me.tpreview.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tpreview.BackColor = System.Drawing.Color.White
        Me.tpreview.Location = New System.Drawing.Point(7, 5)
        Me.tpreview.Name = "tpreview"
        Me.tpreview.ReadOnly = True
        Me.tpreview.Size = New System.Drawing.Size(578, 326)
        Me.tpreview.TabIndex = 4
        Me.tpreview.Text = ""
        Me.tpreview.WordWrap = False
        '
        'Lcomps
        '
        Me.Lcomps.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Lcomps.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4, Me.ColumnHeader5, Me.ColumnHeader6})
        Me.Lcomps.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Lcomps.FullRowSelect = True
        Me.Lcomps.HideSelection = False
        Me.Lcomps.Location = New System.Drawing.Point(0, 46)
        Me.Lcomps.MultiSelect = False
        Me.Lcomps.Name = "Lcomps"
        Me.Lcomps.Size = New System.Drawing.Size(587, 320)
        Me.Lcomps.TabIndex = 0
        Me.Lcomps.UseCompatibleStateImageBehavior = False
        Me.Lcomps.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Date"
        Me.ColumnHeader1.Width = 126
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Title"
        Me.ColumnHeader2.Width = 236
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Author"
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Publiction"
        Me.ColumnHeader4.Width = 84
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "Classification"
        Me.ColumnHeader5.Width = 105
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(473, 3)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(52, 13)
        Me.Label8.TabIndex = 24
        Me.Label8.Text = "Date To"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(356, 3)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(67, 13)
        Me.Label7.TabIndex = 23
        Me.Label7.Text = "Date From"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(2, 4)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(45, 13)
        Me.Label5.TabIndex = 21
        Me.Label5.Text = "Author"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(243, 3)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(82, 13)
        Me.Label4.TabIndex = 20
        Me.Label4.Text = "Classification"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(120, 3)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(72, 13)
        Me.Label3.TabIndex = 19
        Me.Label3.Text = "Publication "
        '
        'Cauthor
        '
        Me.Cauthor.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Cauthor.FormattingEnabled = True
        Me.Cauthor.Location = New System.Drawing.Point(3, 20)
        Me.Cauthor.Name = "Cauthor"
        Me.Cauthor.Size = New System.Drawing.Size(107, 21)
        Me.Cauthor.TabIndex = 17
        '
        'Centity
        '
        Me.Centity.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Centity.FormattingEnabled = True
        Me.Centity.Location = New System.Drawing.Point(246, 20)
        Me.Centity.Name = "Centity"
        Me.Centity.Size = New System.Drawing.Size(107, 21)
        Me.Centity.TabIndex = 16
        '
        'Cpubtype
        '
        Me.Cpubtype.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Cpubtype.FormattingEnabled = True
        Me.Cpubtype.Location = New System.Drawing.Point(123, 20)
        Me.Cpubtype.Name = "Cpubtype"
        Me.Cpubtype.Size = New System.Drawing.Size(111, 21)
        Me.Cpubtype.TabIndex = 15
        '
        'Dto
        '
        Me.Dto.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Dto.Location = New System.Drawing.Point(476, 20)
        Me.Dto.Name = "Dto"
        Me.Dto.Size = New System.Drawing.Size(111, 21)
        Me.Dto.TabIndex = 14
        '
        'Dfrom
        '
        Me.Dfrom.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Dfrom.Location = New System.Drawing.Point(359, 20)
        Me.Dfrom.Name = "Dfrom"
        Me.Dfrom.Size = New System.Drawing.Size(111, 21)
        Me.Dfrom.TabIndex = 13
        '
        'ColumnHeader6
        '
        Me.ColumnHeader6.Width = 0
        '
        'bc_am_user_defined_components
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(611, 453)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.Panel2)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.MaximizeBox = False
        Me.Name = "bc_am_user_defined_components"
        Me.Text = "Preview User Defined Component - Blue Curve"
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents rpublic As System.Windows.Forms.RadioButton
    Friend WithEvents rprivate As System.Windows.Forms.RadioButton
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents bsave As System.Windows.Forms.Button
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents breload As System.Windows.Forms.Button
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents Cwrap As System.Windows.Forms.CheckBox
    Friend WithEvents Ltitle As System.Windows.Forms.Label
    Friend WithEvents Ttitle As System.Windows.Forms.TextBox
    Friend WithEvents tpreview As System.Windows.Forms.RichTextBox
    Friend WithEvents Lcomps As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Cauthor As System.Windows.Forms.ComboBox
    Friend WithEvents Centity As System.Windows.Forms.ComboBox
    Friend WithEvents Cpubtype As System.Windows.Forms.ComboBox
    Friend WithEvents Dto As System.Windows.Forms.DateTimePicker
    Friend WithEvents Dfrom As System.Windows.Forms.DateTimePicker
    Friend WithEvents ColumnHeader6 As System.Windows.Forms.ColumnHeader
End Class
