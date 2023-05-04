Friend Class bc_am_bp_publication_type
    Inherits System.Windows.Forms.Form

    Private ctrllr As bc_am_publication

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
    Friend WithEvents uxImageList As System.Windows.Forms.ImageList
    Friend WithEvents uxPubTypeDetails As System.Windows.Forms.GroupBox
    Friend WithEvents uxVariations As System.Windows.Forms.GroupBox
    Friend WithEvents uxDesc As System.Windows.Forms.Label
    Friend WithEvents uxLang As System.Windows.Forms.Label
    Friend WithEvents uxCat As System.Windows.Forms.Label
    Friend WithEvents uxVar As System.Windows.Forms.Label
    Friend WithEvents uxDescription As System.Windows.Forms.TextBox
    Friend WithEvents uxLanguage As System.Windows.Forms.ComboBox
    Friend WithEvents uxCategory As System.Windows.Forms.ComboBox
    Friend WithEvents uxVariation As System.Windows.Forms.ComboBox
    Friend WithEvents uxApply As System.Windows.Forms.Button
    Friend WithEvents uxTemplate As System.Windows.Forms.ComboBox
    Friend WithEvents uxAssign As System.Windows.Forms.Button
    Friend WithEvents uxTooltip As System.Windows.Forms.ToolTip
    Friend WithEvents uxTemp As System.Windows.Forms.Label
    Friend WithEvents uxVariationCol As System.Windows.Forms.ColumnHeader
    Friend WithEvents uxTemplateCol As System.Windows.Forms.ColumnHeader
    Friend WithEvents uxProducts As System.Windows.Forms.ListView
    Friend WithEvents uxClose As System.Windows.Forms.Button
    Friend WithEvents uxPubType As System.Windows.Forms.Label
    Friend WithEvents uxPublicationType As System.Windows.Forms.TextBox
    Friend WithEvents uxSaveVariations As System.Windows.Forms.Button
    Friend WithEvents uxSecondaryVariation As System.Windows.Forms.ComboBox
    Friend WithEvents uxUndoVariations As System.Windows.Forms.Button
    Friend WithEvents uxWorkflowStage As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents uxemailtemplate As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents uxsaveemailtemplate As System.Windows.Forms.Button
    Friend WithEvents uxSecVar As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_bp_publication_type))
        Me.uxImageList = New System.Windows.Forms.ImageList()
        Me.uxPubTypeDetails = New System.Windows.Forms.GroupBox()
        Me.uxWorkflowStage = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.uxSecondaryVariation = New System.Windows.Forms.ComboBox()
        Me.uxSecVar = New System.Windows.Forms.Label()
        Me.uxPublicationType = New System.Windows.Forms.TextBox()
        Me.uxPubType = New System.Windows.Forms.Label()
        Me.uxApply = New System.Windows.Forms.Button()
        Me.uxVariation = New System.Windows.Forms.ComboBox()
        Me.uxCategory = New System.Windows.Forms.ComboBox()
        Me.uxLanguage = New System.Windows.Forms.ComboBox()
        Me.uxDescription = New System.Windows.Forms.TextBox()
        Me.uxVar = New System.Windows.Forms.Label()
        Me.uxCat = New System.Windows.Forms.Label()
        Me.uxLang = New System.Windows.Forms.Label()
        Me.uxDesc = New System.Windows.Forms.Label()
        Me.uxVariations = New System.Windows.Forms.GroupBox()
        Me.uxUndoVariations = New System.Windows.Forms.Button()
        Me.uxSaveVariations = New System.Windows.Forms.Button()
        Me.uxTemp = New System.Windows.Forms.Label()
        Me.uxAssign = New System.Windows.Forms.Button()
        Me.uxTemplate = New System.Windows.Forms.ComboBox()
        Me.uxProducts = New System.Windows.Forms.ListView()
        Me.uxVariationCol = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.uxTemplateCol = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.uxClose = New System.Windows.Forms.Button()
        Me.uxTooltip = New System.Windows.Forms.ToolTip()
        Me.uxsaveemailtemplate = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.uxemailtemplate = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.uxPubTypeDetails.SuspendLayout()
        Me.uxVariations.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'uxImageList
        '
        Me.uxImageList.ImageStream = CType(resources.GetObject("uxImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.uxImageList.TransparentColor = System.Drawing.Color.Transparent
        Me.uxImageList.Images.SetKeyName(0, "")
        Me.uxImageList.Images.SetKeyName(1, "")
        Me.uxImageList.Images.SetKeyName(2, "")
        '
        'uxPubTypeDetails
        '
        Me.uxPubTypeDetails.Controls.Add(Me.uxWorkflowStage)
        Me.uxPubTypeDetails.Controls.Add(Me.Label1)
        Me.uxPubTypeDetails.Controls.Add(Me.uxSecondaryVariation)
        Me.uxPubTypeDetails.Controls.Add(Me.uxSecVar)
        Me.uxPubTypeDetails.Controls.Add(Me.uxPublicationType)
        Me.uxPubTypeDetails.Controls.Add(Me.uxPubType)
        Me.uxPubTypeDetails.Controls.Add(Me.uxApply)
        Me.uxPubTypeDetails.Controls.Add(Me.uxVariation)
        Me.uxPubTypeDetails.Controls.Add(Me.uxCategory)
        Me.uxPubTypeDetails.Controls.Add(Me.uxLanguage)
        Me.uxPubTypeDetails.Controls.Add(Me.uxDescription)
        Me.uxPubTypeDetails.Controls.Add(Me.uxVar)
        Me.uxPubTypeDetails.Controls.Add(Me.uxCat)
        Me.uxPubTypeDetails.Controls.Add(Me.uxLang)
        Me.uxPubTypeDetails.Controls.Add(Me.uxDesc)
        Me.uxPubTypeDetails.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxPubTypeDetails.Location = New System.Drawing.Point(4, 1)
        Me.uxPubTypeDetails.Name = "uxPubTypeDetails"
        Me.uxPubTypeDetails.Size = New System.Drawing.Size(592, 182)
        Me.uxPubTypeDetails.TabIndex = 0
        Me.uxPubTypeDetails.TabStop = False
        Me.uxPubTypeDetails.Text = "Details"
        '
        'uxWorkflowStage
        '
        Me.uxWorkflowStage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.uxWorkflowStage.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxWorkflowStage.Location = New System.Drawing.Point(372, 47)
        Me.uxWorkflowStage.Name = "uxWorkflowStage"
        Me.uxWorkflowStage.Size = New System.Drawing.Size(216, 21)
        Me.uxWorkflowStage.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(301, 43)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(67, 27)
        Me.Label1.TabIndex = 14
        Me.Label1.Text = "Workflow Stage:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'uxSecondaryVariation
        '
        Me.uxSecondaryVariation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.uxSecondaryVariation.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxSecondaryVariation.Location = New System.Drawing.Point(372, 129)
        Me.uxSecondaryVariation.Name = "uxSecondaryVariation"
        Me.uxSecondaryVariation.Size = New System.Drawing.Size(216, 21)
        Me.uxSecondaryVariation.TabIndex = 5
        '
        'uxSecVar
        '
        Me.uxSecVar.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxSecVar.Location = New System.Drawing.Point(304, 128)
        Me.uxSecVar.Name = "uxSecVar"
        Me.uxSecVar.Size = New System.Drawing.Size(64, 21)
        Me.uxSecVar.TabIndex = 12
        Me.uxSecVar.Text = "2nd Var.:"
        Me.uxSecVar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'uxPublicationType
        '
        Me.uxPublicationType.Enabled = False
        Me.uxPublicationType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxPublicationType.Location = New System.Drawing.Point(88, 21)
        Me.uxPublicationType.Name = "uxPublicationType"
        Me.uxPublicationType.Size = New System.Drawing.Size(204, 21)
        Me.uxPublicationType.TabIndex = 10
        '
        'uxPubType
        '
        Me.uxPubType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxPubType.Location = New System.Drawing.Point(8, 19)
        Me.uxPubType.Name = "uxPubType"
        Me.uxPubType.Size = New System.Drawing.Size(76, 21)
        Me.uxPubType.TabIndex = 9
        Me.uxPubType.Text = "Publication:"
        Me.uxPubType.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'uxApply
        '
        Me.uxApply.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxApply.ImageAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.uxApply.ImageIndex = 0
        Me.uxApply.ImageList = Me.uxImageList
        Me.uxApply.Location = New System.Drawing.Point(560, 152)
        Me.uxApply.Name = "uxApply"
        Me.uxApply.Size = New System.Drawing.Size(28, 24)
        Me.uxApply.TabIndex = 5
        Me.uxTooltip.SetToolTip(Me.uxApply, "Apply")
        '
        'uxVariation
        '
        Me.uxVariation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.uxVariation.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxVariation.Location = New System.Drawing.Point(372, 101)
        Me.uxVariation.Name = "uxVariation"
        Me.uxVariation.Size = New System.Drawing.Size(216, 21)
        Me.uxVariation.TabIndex = 4
        '
        'uxCategory
        '
        Me.uxCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.uxCategory.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxCategory.Location = New System.Drawing.Point(372, 74)
        Me.uxCategory.Name = "uxCategory"
        Me.uxCategory.Size = New System.Drawing.Size(216, 21)
        Me.uxCategory.TabIndex = 3
        '
        'uxLanguage
        '
        Me.uxLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.uxLanguage.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxLanguage.Location = New System.Drawing.Point(372, 20)
        Me.uxLanguage.Name = "uxLanguage"
        Me.uxLanguage.Size = New System.Drawing.Size(216, 21)
        Me.uxLanguage.TabIndex = 1
        '
        'uxDescription
        '
        Me.uxDescription.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxDescription.Location = New System.Drawing.Point(88, 48)
        Me.uxDescription.Multiline = True
        Me.uxDescription.Name = "uxDescription"
        Me.uxDescription.Size = New System.Drawing.Size(204, 75)
        Me.uxDescription.TabIndex = 0
        '
        'uxVar
        '
        Me.uxVar.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxVar.Location = New System.Drawing.Point(302, 101)
        Me.uxVar.Name = "uxVar"
        Me.uxVar.Size = New System.Drawing.Size(66, 21)
        Me.uxVar.TabIndex = 8
        Me.uxVar.Text = "Variation:"
        Me.uxVar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'uxCat
        '
        Me.uxCat.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxCat.Location = New System.Drawing.Point(302, 74)
        Me.uxCat.Name = "uxCat"
        Me.uxCat.Size = New System.Drawing.Size(66, 21)
        Me.uxCat.TabIndex = 7
        Me.uxCat.Text = "Category:"
        Me.uxCat.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'uxLang
        '
        Me.uxLang.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxLang.Location = New System.Drawing.Point(301, 18)
        Me.uxLang.Name = "uxLang"
        Me.uxLang.Size = New System.Drawing.Size(67, 21)
        Me.uxLang.TabIndex = 6
        Me.uxLang.Text = "Language:"
        Me.uxLang.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'uxDesc
        '
        Me.uxDesc.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxDesc.Location = New System.Drawing.Point(8, 46)
        Me.uxDesc.Name = "uxDesc"
        Me.uxDesc.Size = New System.Drawing.Size(76, 21)
        Me.uxDesc.TabIndex = 5
        Me.uxDesc.Text = "Description:"
        Me.uxDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'uxVariations
        '
        Me.uxVariations.Controls.Add(Me.uxUndoVariations)
        Me.uxVariations.Controls.Add(Me.uxSaveVariations)
        Me.uxVariations.Controls.Add(Me.uxTemp)
        Me.uxVariations.Controls.Add(Me.uxAssign)
        Me.uxVariations.Controls.Add(Me.uxTemplate)
        Me.uxVariations.Controls.Add(Me.uxProducts)
        Me.uxVariations.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxVariations.Location = New System.Drawing.Point(4, 189)
        Me.uxVariations.Name = "uxVariations"
        Me.uxVariations.Size = New System.Drawing.Size(592, 244)
        Me.uxVariations.TabIndex = 1
        Me.uxVariations.TabStop = False
        Me.uxVariations.Text = "Variations"
        '
        'uxUndoVariations
        '
        Me.uxUndoVariations.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxUndoVariations.Image = CType(resources.GetObject("uxUndoVariations.Image"), System.Drawing.Image)
        Me.uxUndoVariations.ImageAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.uxUndoVariations.Location = New System.Drawing.Point(414, 15)
        Me.uxUndoVariations.Name = "uxUndoVariations"
        Me.uxUndoVariations.Size = New System.Drawing.Size(28, 24)
        Me.uxUndoVariations.TabIndex = 6
        Me.uxTooltip.SetToolTip(Me.uxUndoVariations, "Save Variations")
        '
        'uxSaveVariations
        '
        Me.uxSaveVariations.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxSaveVariations.ImageAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.uxSaveVariations.ImageIndex = 0
        Me.uxSaveVariations.ImageList = Me.uxImageList
        Me.uxSaveVariations.Location = New System.Drawing.Point(380, 15)
        Me.uxSaveVariations.Name = "uxSaveVariations"
        Me.uxSaveVariations.Size = New System.Drawing.Size(28, 24)
        Me.uxSaveVariations.TabIndex = 5
        Me.uxTooltip.SetToolTip(Me.uxSaveVariations, "Save Variations")
        '
        'uxTemp
        '
        Me.uxTemp.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxTemp.Location = New System.Drawing.Point(12, 17)
        Me.uxTemp.Name = "uxTemp"
        Me.uxTemp.Size = New System.Drawing.Size(72, 21)
        Me.uxTemp.TabIndex = 3
        Me.uxTemp.Text = "Template:"
        Me.uxTemp.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'uxAssign
        '
        Me.uxAssign.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxAssign.ImageAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.uxAssign.ImageIndex = 1
        Me.uxAssign.ImageList = Me.uxImageList
        Me.uxAssign.Location = New System.Drawing.Point(346, 15)
        Me.uxAssign.Name = "uxAssign"
        Me.uxAssign.Size = New System.Drawing.Size(28, 24)
        Me.uxAssign.TabIndex = 1
        Me.uxTooltip.SetToolTip(Me.uxAssign, "Assign")
        '
        'uxTemplate
        '
        Me.uxTemplate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.uxTemplate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxTemplate.Location = New System.Drawing.Point(88, 17)
        Me.uxTemplate.Name = "uxTemplate"
        Me.uxTemplate.Size = New System.Drawing.Size(252, 21)
        Me.uxTemplate.TabIndex = 0
        '
        'uxProducts
        '
        Me.uxProducts.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.uxVariationCol, Me.uxTemplateCol})
        Me.uxProducts.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxProducts.FullRowSelect = True
        Me.uxProducts.GridLines = True
        Me.uxProducts.HideSelection = False
        Me.uxProducts.Location = New System.Drawing.Point(3, 41)
        Me.uxProducts.Name = "uxProducts"
        Me.uxProducts.Size = New System.Drawing.Size(585, 196)
        Me.uxProducts.SmallImageList = Me.uxImageList
        Me.uxProducts.TabIndex = 2
        Me.uxProducts.UseCompatibleStateImageBehavior = False
        Me.uxProducts.View = System.Windows.Forms.View.Details
        '
        'uxVariationCol
        '
        Me.uxVariationCol.Text = "Variation"
        Me.uxVariationCol.Width = 339
        '
        'uxTemplateCol
        '
        Me.uxTemplateCol.Text = "Template"
        Me.uxTemplateCol.Width = 220
        '
        'uxClose
        '
        Me.uxClose.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.uxClose.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxClose.Location = New System.Drawing.Point(511, 515)
        Me.uxClose.Name = "uxClose"
        Me.uxClose.Size = New System.Drawing.Size(75, 23)
        Me.uxClose.TabIndex = 3
        Me.uxClose.Text = "Close"
        '
        'uxsaveemailtemplate
        '
        Me.uxsaveemailtemplate.Enabled = False
        Me.uxsaveemailtemplate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxsaveemailtemplate.ImageAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.uxsaveemailtemplate.ImageIndex = 0
        Me.uxsaveemailtemplate.ImageList = Me.uxImageList
        Me.uxsaveemailtemplate.Location = New System.Drawing.Point(390, 15)
        Me.uxsaveemailtemplate.Name = "uxsaveemailtemplate"
        Me.uxsaveemailtemplate.Size = New System.Drawing.Size(28, 24)
        Me.uxsaveemailtemplate.TabIndex = 7
        Me.uxTooltip.SetToolTip(Me.uxsaveemailtemplate, "Save Variations")
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.uxsaveemailtemplate)
        Me.GroupBox1.Controls.Add(Me.uxemailtemplate)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(4, 439)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(592, 57)
        Me.GroupBox1.TabIndex = 7
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Html Email Output Template"
        '
        'uxemailtemplate
        '
        Me.uxemailtemplate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.uxemailtemplate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxemailtemplate.Location = New System.Drawing.Point(122, 18)
        Me.uxemailtemplate.Name = "uxemailtemplate"
        Me.uxemailtemplate.Size = New System.Drawing.Size(252, 21)
        Me.uxemailtemplate.TabIndex = 0
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(12, 17)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(123, 21)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Email Template:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'bc_am_bp_publication_type
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.CancelButton = Me.uxClose
        Me.ClientSize = New System.Drawing.Size(598, 550)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.uxVariations)
        Me.Controls.Add(Me.uxPubTypeDetails)
        Me.Controls.Add(Me.uxClose)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "bc_am_bp_publication_type"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Publication Type"
        Me.uxPubTypeDetails.ResumeLayout(False)
        Me.uxPubTypeDetails.PerformLayout()
        Me.uxVariations.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Friend WriteOnly Property Controller() As bc_am_publication

        Set(ByVal Value As bc_am_publication)
            ctrllr = Value
        End Set

    End Property

    Private Sub uxCategory_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxCategory.SelectedIndexChanged

        ctrllr.LoadVariations()
        ctrllr.Validate(True)

    End Sub

    Private Sub uxApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxApply.Click

        ctrllr.Apply()

    End Sub

    Private Sub uxDescription_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxDescription.TextChanged

        ctrllr.Validate(False)

    End Sub

    Private Sub uxLanguage_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxLanguage.SelectedIndexChanged

        ctrllr.Validate(False)

    End Sub

    'Sam 2.1.9 Publication Configuration
    Private Sub uxWorkflowStage_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxWorkflowStage.SelectedIndexChanged

        ctrllr.Validate(False)

    End Sub

    Private Sub uxVariation_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxVariation.SelectedIndexChanged

        ctrllr.Validate(True)

    End Sub

    Private Sub uxAssign_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxAssign.Click

        ctrllr.AssignTemplateToVariation()
        ctrllr.ValidateVariations(True)

    End Sub

    Private Sub uxSaveVariations_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxSaveVariations.Click

        ctrllr.SaveTemplatesAgainstVariations()

    End Sub

    Private Sub uxTemplate_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxTemplate.SelectedIndexChanged

        ctrllr.ValidateVariations(False)

    End Sub

    Private Sub uxProducts_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxProducts.SelectedIndexChanged

        ctrllr.ValidateVariations(False)

    End Sub

    Private Sub bc_am_bp_publication_type_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing

        If Not ctrllr.CheckConfigurationComplete() Then
            e.Cancel = True
        End If

    End Sub

    Private Sub uxSecondaryVariation_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxSecondaryVariation.SelectedIndexChanged

        ctrllr.Validate(True)

    End Sub

    Private Sub uxUndoVariations_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxUndoVariations.Click

        ctrllr.Undo()

    End Sub

    Private Sub uxemailtemplate_SelectedIndexChanged(sender As Object, e As EventArgs) Handles uxemailtemplate.SelectedIndexChanged
        Me.uxsaveemailtemplate.Enabled = False
        If Me.uxemailtemplate.SelectedIndex > -1 Then
            Me.uxsaveemailtemplate.Enabled = True
        End If

    End Sub

    Private Sub uxsaveemailtemplate_Click(sender As Object, e As EventArgs) Handles uxsaveemailtemplate.Click
        ctrllr.SaveEmailTemplate()
    End Sub
End Class
