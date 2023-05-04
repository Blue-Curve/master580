Imports BlueCurve.Core.CS
Friend Class bc_am_bp_composite
    Inherits System.Windows.Forms.Form

    Private ctrllr As bc_am_composite

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
    Friend WithEvents uxPubType As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents uxSave As System.Windows.Forms.Button
    Friend WithEvents uxPublicationType As System.Windows.Forms.ComboBox
    Friend WithEvents uxDetails As System.Windows.Forms.GroupBox
    Friend WithEvents uxDescription As System.Windows.Forms.TextBox
    Friend WithEvents uxClose As System.Windows.Forms.Button
    Friend WithEvents uxCancel As System.Windows.Forms.Button
    Friend WithEvents uxPubTypes As System.Windows.Forms.ListView
    Friend WithEvents uxPubTypeDesc As System.Windows.Forms.ColumnHeader
    Friend WithEvents uxUnits As System.Windows.Forms.GroupBox
    Friend WithEvents uxToolbarPanel As System.Windows.Forms.Panel
    Friend WithEvents uxToolbar As System.Windows.Forms.ToolBar
    Friend WithEvents uxEdit As System.Windows.Forms.ToolBarButton
    Friend WithEvents uxDelete As System.Windows.Forms.ToolBarButton
    Friend WithEvents uxAdd As System.Windows.Forms.ToolBarButton
    Friend WithEvents uxSaveOrder As System.Windows.Forms.ToolBarButton
    Friend WithEvents uxUp As System.Windows.Forms.Button
    Friend WithEvents uxDown As System.Windows.Forms.Button
    Friend WithEvents uxRefresh As System.Windows.Forms.ToolBarButton
    Friend WithEvents uxToolTip As System.Windows.Forms.ToolTip
    Friend WithEvents uxWorkflowStage As System.Windows.Forms.ComboBox
    Friend WithEvents uxWFStage As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_bp_composite))
        Me.uxClose = New System.Windows.Forms.Button
        Me.uxImages = New System.Windows.Forms.ImageList(Me.components)
        Me.uxDetails = New System.Windows.Forms.GroupBox
        Me.uxWorkflowStage = New System.Windows.Forms.ComboBox
        Me.uxWFStage = New System.Windows.Forms.Label
        Me.uxCancel = New System.Windows.Forms.Button
        Me.uxPublicationType = New System.Windows.Forms.ComboBox
        Me.uxSave = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.uxPubType = New System.Windows.Forms.Label
        Me.uxDescription = New System.Windows.Forms.TextBox
        Me.uxUnits = New System.Windows.Forms.GroupBox
        Me.uxDown = New System.Windows.Forms.Button
        Me.uxUp = New System.Windows.Forms.Button
        Me.uxToolbarPanel = New System.Windows.Forms.Panel
        Me.uxToolbar = New System.Windows.Forms.ToolBar
        Me.uxAdd = New System.Windows.Forms.ToolBarButton
        Me.uxEdit = New System.Windows.Forms.ToolBarButton
        Me.uxDelete = New System.Windows.Forms.ToolBarButton
        Me.uxSaveOrder = New System.Windows.Forms.ToolBarButton
        Me.uxRefresh = New System.Windows.Forms.ToolBarButton
        Me.uxPubTypes = New System.Windows.Forms.ListView
        Me.uxPubTypeDesc = New System.Windows.Forms.ColumnHeader
        Me.uxToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.uxDetails.SuspendLayout()
        Me.uxUnits.SuspendLayout()
        Me.uxToolbarPanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'uxClose
        '
        Me.uxClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.uxClose.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxClose.Location = New System.Drawing.Point(325, 410)
        Me.uxClose.Name = "uxClose"
        Me.uxClose.Size = New System.Drawing.Size(75, 23)
        Me.uxClose.TabIndex = 2
        Me.uxClose.Text = "Close"
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
        'uxDetails
        '
        Me.uxDetails.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.uxDetails.Controls.Add(Me.uxWorkflowStage)
        Me.uxDetails.Controls.Add(Me.uxWFStage)
        Me.uxDetails.Controls.Add(Me.uxCancel)
        Me.uxDetails.Controls.Add(Me.uxPublicationType)
        Me.uxDetails.Controls.Add(Me.uxSave)
        Me.uxDetails.Controls.Add(Me.Label2)
        Me.uxDetails.Controls.Add(Me.uxPubType)
        Me.uxDetails.Controls.Add(Me.uxDescription)
        Me.uxDetails.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxDetails.Location = New System.Drawing.Point(8, 232)
        Me.uxDetails.Name = "uxDetails"
        Me.uxDetails.Size = New System.Drawing.Size(392, 172)
        Me.uxDetails.TabIndex = 1
        Me.uxDetails.TabStop = False
        Me.uxDetails.Text = "Details"
        '
        'uxWorkflowStage
        '
        Me.uxWorkflowStage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.uxWorkflowStage.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxWorkflowStage.Location = New System.Drawing.Point(152, 115)
        Me.uxWorkflowStage.Name = "uxWorkflowStage"
        Me.uxWorkflowStage.Size = New System.Drawing.Size(232, 21)
        Me.uxWorkflowStage.TabIndex = 2
        '
        'uxWFStage
        '
        Me.uxWFStage.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxWFStage.Location = New System.Drawing.Point(12, 115)
        Me.uxWFStage.Name = "uxWFStage"
        Me.uxWFStage.Size = New System.Drawing.Size(136, 21)
        Me.uxWFStage.TabIndex = 8
        Me.uxWFStage.Text = "Workflow Stage:"
        Me.uxWFStage.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'uxCancel
        '
        Me.uxCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxCancel.ImageAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.uxCancel.ImageIndex = 2
        Me.uxCancel.ImageList = Me.uxImages
        Me.uxCancel.Location = New System.Drawing.Point(356, 141)
        Me.uxCancel.Name = "uxCancel"
        Me.uxCancel.Size = New System.Drawing.Size(28, 24)
        Me.uxCancel.TabIndex = 5
        Me.uxCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.uxToolTip.SetToolTip(Me.uxCancel, "Cancel")
        '
        'uxPublicationType
        '
        Me.uxPublicationType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.uxPublicationType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxPublicationType.Location = New System.Drawing.Point(152, 22)
        Me.uxPublicationType.Name = "uxPublicationType"
        Me.uxPublicationType.Size = New System.Drawing.Size(232, 21)
        Me.uxPublicationType.TabIndex = 0
        '
        'uxSave
        '
        Me.uxSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxSave.ImageAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.uxSave.ImageIndex = 4
        Me.uxSave.ImageList = Me.uxImages
        Me.uxSave.Location = New System.Drawing.Point(322, 141)
        Me.uxSave.Name = "uxSave"
        Me.uxSave.Size = New System.Drawing.Size(28, 24)
        Me.uxSave.TabIndex = 4
        Me.uxSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.uxToolTip.SetToolTip(Me.uxSave, "Save")
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(32, 51)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(116, 21)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "Description:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'uxPubType
        '
        Me.uxPubType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxPubType.Location = New System.Drawing.Point(12, 22)
        Me.uxPubType.Name = "uxPubType"
        Me.uxPubType.Size = New System.Drawing.Size(136, 21)
        Me.uxPubType.TabIndex = 6
        Me.uxPubType.Text = "Unit Publication Type:"
        Me.uxPubType.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'uxDescription
        '
        Me.uxDescription.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxDescription.Location = New System.Drawing.Point(152, 52)
        Me.uxDescription.Multiline = True
        Me.uxDescription.Name = "uxDescription"
        Me.uxDescription.Size = New System.Drawing.Size(232, 56)
        Me.uxDescription.TabIndex = 1
        '
        'uxUnits
        '
        Me.uxUnits.Controls.Add(Me.uxDown)
        Me.uxUnits.Controls.Add(Me.uxUp)
        Me.uxUnits.Controls.Add(Me.uxToolbarPanel)
        Me.uxUnits.Controls.Add(Me.uxPubTypes)
        Me.uxUnits.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxUnits.Location = New System.Drawing.Point(8, 5)
        Me.uxUnits.Name = "uxUnits"
        Me.uxUnits.Size = New System.Drawing.Size(392, 219)
        Me.uxUnits.TabIndex = 0
        Me.uxUnits.TabStop = False
        Me.uxUnits.Text = "Units"
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
        'uxPubTypes
        '
        Me.uxPubTypes.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.uxPubTypeDesc})
        Me.uxPubTypes.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxPubTypes.FullRowSelect = True
        Me.uxPubTypes.GridLines = True
        Me.uxPubTypes.HideSelection = False
        Me.uxPubTypes.Location = New System.Drawing.Point(6, 45)
        Me.uxPubTypes.MultiSelect = False
        Me.uxPubTypes.Name = "uxPubTypes"
        Me.uxPubTypes.Size = New System.Drawing.Size(342, 167)
        Me.uxPubTypes.SmallImageList = Me.uxImages
        Me.uxPubTypes.TabIndex = 0
        Me.uxPubTypes.UseCompatibleStateImageBehavior = False
        Me.uxPubTypes.View = System.Windows.Forms.View.Details
        '
        'uxPubTypeDesc
        '
        Me.uxPubTypeDesc.Text = "Description"
        Me.uxPubTypeDesc.Width = 317
        '
        'bc_am_bp_composite
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.CancelButton = Me.uxClose
        Me.ClientSize = New System.Drawing.Size(406, 440)
        Me.Controls.Add(Me.uxUnits)
        Me.Controls.Add(Me.uxDetails)
        Me.Controls.Add(Me.uxClose)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "bc_am_bp_composite"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Composite"
        Me.uxDetails.ResumeLayout(False)
        Me.uxDetails.PerformLayout()
        Me.uxUnits.ResumeLayout(False)
        Me.uxToolbarPanel.ResumeLayout(False)
        Me.uxToolbarPanel.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public WriteOnly Property Controller() As bc_am_composite

        Set(ByVal Value As bc_am_composite)
            ctrllr = Value
        End Set

    End Property

    Private Sub uxPubTypes_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxPubTypes.SelectedIndexChanged

        If uxPubTypes.SelectedItems.Count = 1 Then
            ctrllr.PopulateDetails()
        Else
            ctrllr.Reset()
        End If

    End Sub

    Private Sub uxToolbar_ButtonClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles uxToolbar.ButtonClick

        ctrllr.ActionToolbar(e.Button)

    End Sub

    Private Sub uxCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxCancel.Click

        ctrllr.Reset()

    End Sub

    Private Sub uxSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxSave.Click

        ctrllr.Save()

    End Sub

    Private Sub uxUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxUp.Click

        ctrllr.ChangeOrder(True)

    End Sub

    Private Sub uxDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxDown.Click

        ctrllr.ChangeOrder(False)

    End Sub

    Private Sub uxPublicationType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxPublicationType.SelectedIndexChanged

        ctrllr.Validate()

    End Sub

    Private Sub uxDescription_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxDescription.TextChanged

        ctrllr.Validate()

    End Sub

    Private Sub uxWorkflowStage_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxWorkflowStage.SelectedIndexChanged

        ctrllr.Validate()

    End Sub

    Private Sub uxClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxClose.Click

    End Sub
End Class
