Friend Class bc_am_bp_parameters
    Inherits System.Windows.Forms.Form

    Private ctrllr As bc_am_parameters

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
    Friend WithEvents uxImages As System.Windows.Forms.ImageList
    Friend WithEvents uxToolTip As System.Windows.Forms.ToolTip
    Friend WithEvents uxDown As System.Windows.Forms.Button
    Friend WithEvents uxUp As System.Windows.Forms.Button
    Friend WithEvents uxToolbarPanel As System.Windows.Forms.Panel
    Friend WithEvents uxToolbar As System.Windows.Forms.ToolBar
    Friend WithEvents uxAdd As System.Windows.Forms.ToolBarButton
    Friend WithEvents uxEdit As System.Windows.Forms.ToolBarButton
    Friend WithEvents uxDelete As System.Windows.Forms.ToolBarButton
    Friend WithEvents uxSaveOrder As System.Windows.Forms.ToolBarButton
    Friend WithEvents uxRefresh As System.Windows.Forms.ToolBarButton
    Friend WithEvents uxCancel As System.Windows.Forms.Button
    Friend WithEvents uxSave As System.Windows.Forms.Button
    Friend WithEvents uxClose As System.Windows.Forms.Button
    Friend WithEvents uxParameters As System.Windows.Forms.GroupBox
    Friend WithEvents uxDefinition As System.Windows.Forms.GroupBox
    Friend WithEvents uxParam As System.Windows.Forms.Label
    Friend WithEvents uxCurrentParameters As System.Windows.Forms.ListView
    Friend WithEvents uxParamDesc As System.Windows.Forms.ColumnHeader
    Friend WithEvents uxAvailableParameters As System.Windows.Forms.ComboBox
    Friend WithEvents uxDefValue As System.Windows.Forms.Label
    Friend WithEvents uxDefaultValue As System.Windows.Forms.ComboBox
    Friend WithEvents Chkman As System.Windows.Forms.CheckBox
    Friend WithEvents uxChangeInDoc As System.Windows.Forms.CheckBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_bp_parameters))
        Me.uxImages = New System.Windows.Forms.ImageList(Me.components)
        Me.uxToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.uxCancel = New System.Windows.Forms.Button
        Me.uxSave = New System.Windows.Forms.Button
        Me.uxParameters = New System.Windows.Forms.GroupBox
        Me.uxDown = New System.Windows.Forms.Button
        Me.uxUp = New System.Windows.Forms.Button
        Me.uxToolbarPanel = New System.Windows.Forms.Panel
        Me.uxToolbar = New System.Windows.Forms.ToolBar
        Me.uxAdd = New System.Windows.Forms.ToolBarButton
        Me.uxEdit = New System.Windows.Forms.ToolBarButton
        Me.uxDelete = New System.Windows.Forms.ToolBarButton
        Me.uxSaveOrder = New System.Windows.Forms.ToolBarButton
        Me.uxRefresh = New System.Windows.Forms.ToolBarButton
        Me.uxCurrentParameters = New System.Windows.Forms.ListView
        Me.uxParamDesc = New System.Windows.Forms.ColumnHeader
        Me.uxDefinition = New System.Windows.Forms.GroupBox
        Me.uxChangeInDoc = New System.Windows.Forms.CheckBox
        Me.Chkman = New System.Windows.Forms.CheckBox
        Me.uxDefValue = New System.Windows.Forms.Label
        Me.uxDefaultValue = New System.Windows.Forms.ComboBox
        Me.uxAvailableParameters = New System.Windows.Forms.ComboBox
        Me.uxParam = New System.Windows.Forms.Label
        Me.uxClose = New System.Windows.Forms.Button
        Me.uxParameters.SuspendLayout()
        Me.uxToolbarPanel.SuspendLayout()
        Me.uxDefinition.SuspendLayout()
        Me.SuspendLayout()
        '
        'uxImages
        '
        Me.uxImages.ImageStream = CType(resources.GetObject("uxImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.uxImages.TransparentColor = System.Drawing.Color.Transparent
        Me.uxImages.Images.SetKeyName(0, "")
        Me.uxImages.Images.SetKeyName(1, "")
        Me.uxImages.Images.SetKeyName(2, "")
        Me.uxImages.Images.SetKeyName(3, "")
        Me.uxImages.Images.SetKeyName(4, "")
        Me.uxImages.Images.SetKeyName(5, "")
        Me.uxImages.Images.SetKeyName(6, "")
        Me.uxImages.Images.SetKeyName(7, "")
        Me.uxImages.Images.SetKeyName(8, "")
        '
        'uxCancel
        '
        Me.uxCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxCancel.ImageAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.uxCancel.ImageIndex = 2
        Me.uxCancel.ImageList = Me.uxImages
        Me.uxCancel.Location = New System.Drawing.Point(356, 100)
        Me.uxCancel.Name = "uxCancel"
        Me.uxCancel.Size = New System.Drawing.Size(28, 24)
        Me.uxCancel.TabIndex = 4
        Me.uxCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.uxToolTip.SetToolTip(Me.uxCancel, "Cancel")
        '
        'uxSave
        '
        Me.uxSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxSave.ImageAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.uxSave.ImageIndex = 4
        Me.uxSave.ImageList = Me.uxImages
        Me.uxSave.Location = New System.Drawing.Point(322, 100)
        Me.uxSave.Name = "uxSave"
        Me.uxSave.Size = New System.Drawing.Size(28, 24)
        Me.uxSave.TabIndex = 3
        Me.uxSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.uxToolTip.SetToolTip(Me.uxSave, "Save")
        '
        'uxParameters
        '
        Me.uxParameters.Controls.Add(Me.uxDown)
        Me.uxParameters.Controls.Add(Me.uxUp)
        Me.uxParameters.Controls.Add(Me.uxToolbarPanel)
        Me.uxParameters.Controls.Add(Me.uxCurrentParameters)
        Me.uxParameters.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxParameters.Location = New System.Drawing.Point(4, 4)
        Me.uxParameters.Name = "uxParameters"
        Me.uxParameters.Size = New System.Drawing.Size(392, 219)
        Me.uxParameters.TabIndex = 0
        Me.uxParameters.TabStop = False
        Me.uxParameters.Text = "Parameters"
        '
        'uxDown
        '
        Me.uxDown.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxDown.ImageAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.uxDown.ImageIndex = 6
        Me.uxDown.ImageList = Me.uxImages
        Me.uxDown.Location = New System.Drawing.Point(356, 128)
        Me.uxDown.Name = "uxDown"
        Me.uxDown.Size = New System.Drawing.Size(28, 24)
        Me.uxDown.TabIndex = 2
        '
        'uxUp
        '
        Me.uxUp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxUp.ImageAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.uxUp.ImageIndex = 5
        Me.uxUp.ImageList = Me.uxImages
        Me.uxUp.Location = New System.Drawing.Point(356, 100)
        Me.uxUp.Name = "uxUp"
        Me.uxUp.Size = New System.Drawing.Size(28, 24)
        Me.uxUp.TabIndex = 1
        '
        'uxToolbarPanel
        '
        Me.uxToolbarPanel.Controls.Add(Me.uxToolbar)
        Me.uxToolbarPanel.Dock = System.Windows.Forms.DockStyle.Top
        Me.uxToolbarPanel.Location = New System.Drawing.Point(3, 17)
        Me.uxToolbarPanel.Name = "uxToolbarPanel"
        Me.uxToolbarPanel.Size = New System.Drawing.Size(386, 28)
        Me.uxToolbarPanel.TabIndex = 7
        '
        'uxToolbar
        '
        Me.uxToolbar.Appearance = System.Windows.Forms.ToolBarAppearance.Flat
        Me.uxToolbar.Buttons.AddRange(New System.Windows.Forms.ToolBarButton() {Me.uxAdd, Me.uxEdit, Me.uxDelete, Me.uxSaveOrder, Me.uxRefresh})
        Me.uxToolbar.Divider = False
        Me.uxToolbar.DropDownArrows = True
        Me.uxToolbar.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxToolbar.ImageList = Me.uxImages
        Me.uxToolbar.Location = New System.Drawing.Point(0, 0)
        Me.uxToolbar.Name = "uxToolbar"
        Me.uxToolbar.ShowToolTips = True
        Me.uxToolbar.Size = New System.Drawing.Size(386, 26)
        Me.uxToolbar.TabIndex = 0
        Me.uxToolbar.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right
        '
        'uxAdd
        '
        Me.uxAdd.ImageIndex = 0
        Me.uxAdd.Name = "uxAdd"
        Me.uxAdd.Text = "Add"
        Me.uxAdd.ToolTipText = "Add new unit"
        '
        'uxEdit
        '
        Me.uxEdit.ImageIndex = 1
        Me.uxEdit.Name = "uxEdit"
        Me.uxEdit.Text = "Edit"
        Me.uxEdit.ToolTipText = "Edit a unit"
        '
        'uxDelete
        '
        Me.uxDelete.ImageIndex = 2
        Me.uxDelete.Name = "uxDelete"
        Me.uxDelete.Text = "Delete"
        Me.uxDelete.ToolTipText = "Delete a unit"
        '
        'uxSaveOrder
        '
        Me.uxSaveOrder.ImageIndex = 4
        Me.uxSaveOrder.Name = "uxSaveOrder"
        Me.uxSaveOrder.Text = "Save Order"
        '
        'uxRefresh
        '
        Me.uxRefresh.ImageIndex = 7
        Me.uxRefresh.Name = "uxRefresh"
        Me.uxRefresh.Text = "Reset"
        '
        'uxCurrentParameters
        '
        Me.uxCurrentParameters.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.uxParamDesc})
        Me.uxCurrentParameters.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxCurrentParameters.FullRowSelect = True
        Me.uxCurrentParameters.GridLines = True
        Me.uxCurrentParameters.HideSelection = False
        Me.uxCurrentParameters.Location = New System.Drawing.Point(6, 45)
        Me.uxCurrentParameters.MultiSelect = False
        Me.uxCurrentParameters.Name = "uxCurrentParameters"
        Me.uxCurrentParameters.Size = New System.Drawing.Size(342, 167)
        Me.uxCurrentParameters.SmallImageList = Me.uxImages
        Me.uxCurrentParameters.TabIndex = 0
        Me.uxCurrentParameters.UseCompatibleStateImageBehavior = False
        Me.uxCurrentParameters.View = System.Windows.Forms.View.Details
        '
        'uxParamDesc
        '
        Me.uxParamDesc.Text = "Description"
        Me.uxParamDesc.Width = 317
        '
        'uxDefinition
        '
        Me.uxDefinition.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.uxDefinition.Controls.Add(Me.uxChangeInDoc)
        Me.uxDefinition.Controls.Add(Me.Chkman)
        Me.uxDefinition.Controls.Add(Me.uxDefValue)
        Me.uxDefinition.Controls.Add(Me.uxDefaultValue)
        Me.uxDefinition.Controls.Add(Me.uxCancel)
        Me.uxDefinition.Controls.Add(Me.uxAvailableParameters)
        Me.uxDefinition.Controls.Add(Me.uxSave)
        Me.uxDefinition.Controls.Add(Me.uxParam)
        Me.uxDefinition.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxDefinition.Location = New System.Drawing.Point(4, 228)
        Me.uxDefinition.Name = "uxDefinition"
        Me.uxDefinition.Size = New System.Drawing.Size(392, 128)
        Me.uxDefinition.TabIndex = 1
        Me.uxDefinition.TabStop = False
        Me.uxDefinition.Text = "Definition"
        '
        'uxChangeInDoc
        '
        Me.uxChangeInDoc.Checked = True
        Me.uxChangeInDoc.CheckState = System.Windows.Forms.CheckState.Checked
        Me.uxChangeInDoc.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxChangeInDoc.Location = New System.Drawing.Point(104, 80)
        Me.uxChangeInDoc.Name = "uxChangeInDoc"
        Me.uxChangeInDoc.Size = New System.Drawing.Size(183, 24)
        Me.uxChangeInDoc.TabIndex = 7
        Me.uxChangeInDoc.Text = "Allow Change In Document"
        '
        'Chkman
        '
        Me.Chkman.AutoSize = True
        Me.Chkman.Location = New System.Drawing.Point(104, 105)
        Me.Chkman.Name = "Chkman"
        Me.Chkman.Size = New System.Drawing.Size(86, 17)
        Me.Chkman.TabIndex = 8
        Me.Chkman.Text = "Mandatory"
        Me.Chkman.UseVisualStyleBackColor = True
        '
        'uxDefValue
        '
        Me.uxDefValue.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxDefValue.Location = New System.Drawing.Point(8, 53)
        Me.uxDefValue.Name = "uxDefValue"
        Me.uxDefValue.Size = New System.Drawing.Size(92, 21)
        Me.uxDefValue.TabIndex = 6
        Me.uxDefValue.Text = "Default Value:"
        Me.uxDefValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'uxDefaultValue
        '
        Me.uxDefaultValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.uxDefaultValue.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxDefaultValue.Location = New System.Drawing.Point(104, 53)
        Me.uxDefaultValue.Name = "uxDefaultValue"
        Me.uxDefaultValue.Size = New System.Drawing.Size(280, 21)
        Me.uxDefaultValue.TabIndex = 1
        '
        'uxAvailableParameters
        '
        Me.uxAvailableParameters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.uxAvailableParameters.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxAvailableParameters.Location = New System.Drawing.Point(104, 22)
        Me.uxAvailableParameters.Name = "uxAvailableParameters"
        Me.uxAvailableParameters.Size = New System.Drawing.Size(280, 21)
        Me.uxAvailableParameters.TabIndex = 0
        '
        'uxParam
        '
        Me.uxParam.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxParam.Location = New System.Drawing.Point(28, 22)
        Me.uxParam.Name = "uxParam"
        Me.uxParam.Size = New System.Drawing.Size(72, 21)
        Me.uxParam.TabIndex = 5
        Me.uxParam.Text = "Parameter:"
        Me.uxParam.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'uxClose
        '
        Me.uxClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.uxClose.Location = New System.Drawing.Point(313, 362)
        Me.uxClose.Name = "uxClose"
        Me.uxClose.Size = New System.Drawing.Size(75, 23)
        Me.uxClose.TabIndex = 2
        Me.uxClose.Text = "Close"
        '
        'bc_am_bp_parameters
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.CancelButton = Me.uxClose
        Me.ClientSize = New System.Drawing.Size(398, 391)
        Me.Controls.Add(Me.uxParameters)
        Me.Controls.Add(Me.uxDefinition)
        Me.Controls.Add(Me.uxClose)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "bc_am_bp_parameters"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Parameters"
        Me.uxParameters.ResumeLayout(False)
        Me.uxToolbarPanel.ResumeLayout(False)
        Me.uxToolbarPanel.PerformLayout()
        Me.uxDefinition.ResumeLayout(False)
        Me.uxDefinition.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public WriteOnly Property Controller() As bc_am_parameters

        Set(ByVal Value As bc_am_parameters)
            ctrllr = Value
        End Set

    End Property

    Private Sub uxToolbar_ButtonClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles uxToolbar.ButtonClick

        ctrllr.ActionToolbar(e.Button)

    End Sub

    Private Sub uxCurrentParameters_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxCurrentParameters.SelectedIndexChanged

        If uxCurrentParameters.SelectedItems.Count = 1 Then
            ctrllr.PopulateDetails()
        End If

    End Sub

    Private Sub uxType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxDefaultValue.SelectedIndexChanged

        ctrllr.validate()

    End Sub

    Private Sub uxAvailableParameters_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxAvailableParameters.SelectedIndexChanged

        ctrllr.LoadDefaultValues()
        ctrllr.validate()

    End Sub

    Private Sub uxSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxSave.Click

        ctrllr.Save()

    End Sub

    Private Sub uxCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxCancel.Click

        ctrllr.Reset()

    End Sub

    Private Sub uxUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxUp.Click

        ctrllr.ChangeOrder(True)

    End Sub

    Private Sub uxDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxDown.Click

        ctrllr.ChangeOrder(False)

    End Sub

End Class
