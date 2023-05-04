<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bc_am_in_ub_listentity
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_in_ub_listentity))
        Me.uxAdd = New System.Windows.Forms.PictureBox
        Me.cbOperator = New System.Windows.Forms.ComboBox
        Me.cbClasses = New System.Windows.Forms.ComboBox
        Me.txtParen1 = New System.Windows.Forms.TextBox
        Me.cbEntities = New System.Windows.Forms.ComboBox
        Me.cbAggregationLists = New System.Windows.Forms.ComboBox
        Me.txtParen2 = New System.Windows.Forms.TextBox
        Me.uxRemove = New System.Windows.Forms.PictureBox
        CType(Me.uxAdd, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxRemove, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'uxAdd
        '
        Me.uxAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxAdd.Image = CType(resources.GetObject("uxAdd.Image"), System.Drawing.Image)
        Me.uxAdd.Location = New System.Drawing.Point(603, 2)
        Me.uxAdd.Name = "uxAdd"
        Me.uxAdd.Size = New System.Drawing.Size(24, 24)
        Me.uxAdd.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.uxAdd.TabIndex = 9
        Me.uxAdd.TabStop = False
        '
        'cbOperator
        '
        Me.cbOperator.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbOperator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbOperator.FormattingEnabled = True
        Me.cbOperator.Items.AddRange(New Object() {"", "AND", "OR"})
        Me.cbOperator.Location = New System.Drawing.Point(546, 4)
        Me.cbOperator.Name = "cbOperator"
        Me.cbOperator.Size = New System.Drawing.Size(51, 21)
        Me.cbOperator.TabIndex = 7
        '
        'cbClasses
        '
        Me.cbClasses.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbClasses.FormattingEnabled = True
        Me.cbClasses.Location = New System.Drawing.Point(166, 4)
        Me.cbClasses.Name = "cbClasses"
        Me.cbClasses.Size = New System.Drawing.Size(139, 21)
        Me.cbClasses.TabIndex = 6
        '
        'txtParen1
        '
        Me.txtParen1.Location = New System.Drawing.Point(0, 4)
        Me.txtParen1.Name = "txtParen1"
        Me.txtParen1.Size = New System.Drawing.Size(15, 20)
        Me.txtParen1.TabIndex = 5
        Me.txtParen1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'cbEntities
        '
        Me.cbEntities.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbEntities.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbEntities.FormattingEnabled = True
        Me.cbEntities.Location = New System.Drawing.Point(320, 4)
        Me.cbEntities.Name = "cbEntities"
        Me.cbEntities.Size = New System.Drawing.Size(199, 21)
        Me.cbEntities.TabIndex = 10
        '
        'cbAggregationLists
        '
        Me.cbAggregationLists.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbAggregationLists.FormattingEnabled = True
        Me.cbAggregationLists.Location = New System.Drawing.Point(21, 4)
        Me.cbAggregationLists.Name = "cbAggregationLists"
        Me.cbAggregationLists.Size = New System.Drawing.Size(139, 21)
        Me.cbAggregationLists.TabIndex = 41
        '
        'txtParen2
        '
        Me.txtParen2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtParen2.Location = New System.Drawing.Point(525, 5)
        Me.txtParen2.Name = "txtParen2"
        Me.txtParen2.Size = New System.Drawing.Size(15, 20)
        Me.txtParen2.TabIndex = 42
        '
        'uxRemove
        '
        Me.uxRemove.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxRemove.Image = CType(resources.GetObject("uxRemove.Image"), System.Drawing.Image)
        Me.uxRemove.Location = New System.Drawing.Point(633, 2)
        Me.uxRemove.Name = "uxRemove"
        Me.uxRemove.Size = New System.Drawing.Size(22, 24)
        Me.uxRemove.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.uxRemove.TabIndex = 43
        Me.uxRemove.TabStop = False
        '
        'bc_am_in_ub_listentity
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.BackColor = System.Drawing.Color.Transparent
        Me.Controls.Add(Me.uxRemove)
        Me.Controls.Add(Me.txtParen2)
        Me.Controls.Add(Me.cbAggregationLists)
        Me.Controls.Add(Me.cbEntities)
        Me.Controls.Add(Me.uxAdd)
        Me.Controls.Add(Me.cbOperator)
        Me.Controls.Add(Me.cbClasses)
        Me.Controls.Add(Me.txtParen1)
        Me.Name = "bc_am_in_ub_listentity"
        Me.Size = New System.Drawing.Size(668, 30)
        CType(Me.uxAdd, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxRemove, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents uxAdd As System.Windows.Forms.PictureBox
    Friend WithEvents cbOperator As System.Windows.Forms.ComboBox
    Friend WithEvents cbClasses As System.Windows.Forms.ComboBox
    Friend WithEvents txtParen1 As System.Windows.Forms.TextBox
    Friend WithEvents cbEntities As System.Windows.Forms.ComboBox
    Friend WithEvents cbAggregationLists As System.Windows.Forms.ComboBox
    Friend WithEvents txtParen2 As System.Windows.Forms.TextBox
    Friend WithEvents uxRemove As System.Windows.Forms.PictureBox

End Class
