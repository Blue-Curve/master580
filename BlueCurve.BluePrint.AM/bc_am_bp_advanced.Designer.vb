<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bc_am_bp_advanced
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
        Me.uxTempDefinitionList = New System.Windows.Forms.ListView
        Me.uxComponent = New System.Windows.Forms.ColumnHeader
        Me.uxSubComponent = New System.Windows.Forms.ColumnHeader
        Me.uxXPos = New System.Windows.Forms.ColumnHeader
        Me.uxYPos = New System.Windows.Forms.ColumnHeader
        Me.uxDbDefinitionList = New System.Windows.Forms.ListView
        Me.uxDbComponent = New System.Windows.Forms.ColumnHeader
        Me.uxDbSubComponent = New System.Windows.Forms.ColumnHeader
        Me.uxDbXPos = New System.Windows.Forms.ColumnHeader
        Me.uxDbYPos = New System.Windows.Forms.ColumnHeader
        Me.uxAdd = New System.Windows.Forms.Button
        Me.uxDelete = New System.Windows.Forms.Button
        Me.uxStyle = New System.Windows.Forms.ColumnHeader
        Me.uxDbStyle = New System.Windows.Forms.ColumnHeader
        Me.SuspendLayout()
        '
        'uxTempDefinitionList
        '
        Me.uxTempDefinitionList.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.uxComponent, Me.uxSubComponent, Me.uxXPos, Me.uxYPos, Me.uxStyle})
        Me.uxTempDefinitionList.FullRowSelect = True
        Me.uxTempDefinitionList.GridLines = True
        Me.uxTempDefinitionList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.uxTempDefinitionList.HideSelection = False
        Me.uxTempDefinitionList.Location = New System.Drawing.Point(3, 110)
        Me.uxTempDefinitionList.MultiSelect = False
        Me.uxTempDefinitionList.Name = "uxTempDefinitionList"
        Me.uxTempDefinitionList.ShowItemToolTips = True
        Me.uxTempDefinitionList.Size = New System.Drawing.Size(407, 436)
        Me.uxTempDefinitionList.TabIndex = 1
        Me.uxTempDefinitionList.UseCompatibleStateImageBehavior = False
        Me.uxTempDefinitionList.View = System.Windows.Forms.View.Details
        '
        'uxComponent
        '
        Me.uxComponent.Text = "Component"
        Me.uxComponent.Width = 200
        '
        'uxSubComponent
        '
        Me.uxSubComponent.Text = "SubComponent"
        Me.uxSubComponent.Width = 141
        '
        'uxXPos
        '
        Me.uxXPos.Text = "X"
        Me.uxXPos.Width = 30
        '
        'uxYPos
        '
        Me.uxYPos.Text = "Y"
        Me.uxYPos.Width = 30
        '
        'uxDbDefinitionList
        '
        Me.uxDbDefinitionList.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.uxDbComponent, Me.uxDbSubComponent, Me.uxDbXPos, Me.uxDbYPos, Me.uxDbStyle})
        Me.uxDbDefinitionList.FullRowSelect = True
        Me.uxDbDefinitionList.GridLines = True
        Me.uxDbDefinitionList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.uxDbDefinitionList.HideSelection = False
        Me.uxDbDefinitionList.Location = New System.Drawing.Point(497, 110)
        Me.uxDbDefinitionList.MultiSelect = False
        Me.uxDbDefinitionList.Name = "uxDbDefinitionList"
        Me.uxDbDefinitionList.ShowItemToolTips = True
        Me.uxDbDefinitionList.Size = New System.Drawing.Size(407, 436)
        Me.uxDbDefinitionList.TabIndex = 2
        Me.uxDbDefinitionList.UseCompatibleStateImageBehavior = False
        Me.uxDbDefinitionList.View = System.Windows.Forms.View.Details
        '
        'uxDbComponent
        '
        Me.uxDbComponent.Text = "Component"
        Me.uxDbComponent.Width = 200
        '
        'uxDbSubComponent
        '
        Me.uxDbSubComponent.Text = "SubComponent"
        Me.uxDbSubComponent.Width = 140
        '
        'uxDbXPos
        '
        Me.uxDbXPos.Text = "X"
        Me.uxDbXPos.Width = 30
        '
        'uxDbYPos
        '
        Me.uxDbYPos.Text = "Y"
        Me.uxDbYPos.Width = 30
        '
        'uxAdd
        '
        Me.uxAdd.Location = New System.Drawing.Point(429, 220)
        Me.uxAdd.Name = "uxAdd"
        Me.uxAdd.Size = New System.Drawing.Size(50, 50)
        Me.uxAdd.TabIndex = 3
        Me.uxAdd.Text = "Add"
        Me.uxAdd.UseVisualStyleBackColor = True
        '
        'uxDelete
        '
        Me.uxDelete.Location = New System.Drawing.Point(429, 291)
        Me.uxDelete.Name = "uxDelete"
        Me.uxDelete.Size = New System.Drawing.Size(50, 50)
        Me.uxDelete.TabIndex = 4
        Me.uxDelete.Text = "Delete"
        Me.uxDelete.UseVisualStyleBackColor = True
        '
        'uxStyle
        '
        Me.uxStyle.Text = "Style"
        '
        'uxDbStyle
        '
        Me.uxDbStyle.Text = "Style"
        '
        'bc_am_bp_advanced
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(912, 558)
        Me.Controls.Add(Me.uxDelete)
        Me.Controls.Add(Me.uxAdd)
        Me.Controls.Add(Me.uxDbDefinitionList)
        Me.Controls.Add(Me.uxTempDefinitionList)
        Me.Name = "bc_am_bp_advanced"
        Me.Text = "Advanced"
        Me.ResumeLayout(False)

    End Sub
    Public WithEvents uxTempDefinitionList As System.Windows.Forms.ListView
    Friend WithEvents uxComponent As System.Windows.Forms.ColumnHeader
    Friend WithEvents uxSubComponent As System.Windows.Forms.ColumnHeader
    Friend WithEvents uxXPos As System.Windows.Forms.ColumnHeader
    Friend WithEvents uxYPos As System.Windows.Forms.ColumnHeader
    Public WithEvents uxDbDefinitionList As System.Windows.Forms.ListView
    Friend WithEvents uxDbComponent As System.Windows.Forms.ColumnHeader
    Friend WithEvents uxDbSubComponent As System.Windows.Forms.ColumnHeader
    Friend WithEvents uxDbXPos As System.Windows.Forms.ColumnHeader
    Friend WithEvents uxDbYPos As System.Windows.Forms.ColumnHeader
    Friend WithEvents uxAdd As System.Windows.Forms.Button
    Friend WithEvents uxDelete As System.Windows.Forms.Button
    Friend WithEvents uxStyle As System.Windows.Forms.ColumnHeader
    Friend WithEvents uxDbStyle As System.Windows.Forms.ColumnHeader
End Class
