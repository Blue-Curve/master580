Imports System.Windows.Forms

Friend Class bc_am_bp_pub_type_params
    Inherits System.Windows.Forms.Form

    Private ctrllr As bc_am_publication_params

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
    Friend WithEvents uxSubmissionClassification As System.Windows.Forms.GroupBox
    Friend WithEvents uxReportOption As System.Windows.Forms.ComboBox
    Friend WithEvents uxCategory As System.Windows.Forms.ComboBox
    Friend WithEvents uxRepOpt As System.Windows.Forms.Label
    Friend WithEvents uxCat As System.Windows.Forms.Label
    Friend WithEvents uxTooltip As System.Windows.Forms.ToolTip
    Friend WithEvents uxCategoryCol As System.Windows.Forms.ColumnHeader
    Friend WithEvents uxClose As System.Windows.Forms.Button
    Friend WithEvents uxReportOpts As System.Windows.Forms.GroupBox
    Friend WithEvents uxSaveReportOptions As System.Windows.Forms.Button
    Friend WithEvents uxAssignReportOption As System.Windows.Forms.Button
    Friend WithEvents uxReportOptions As System.Windows.Forms.ListView
    Friend WithEvents uxSaveCategories As System.Windows.Forms.Button
    Friend WithEvents uxAssignCategory As System.Windows.Forms.Button
    Friend WithEvents uxCategories As System.Windows.Forms.ListView
    Friend WithEvents uxReportOptCol As System.Windows.Forms.ColumnHeader
    Friend WithEvents uxRemoveCategory As System.Windows.Forms.Button
    Friend WithEvents uxRemoveReportOption As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_bp_pub_type_params))
        Me.uxReportOpts = New System.Windows.Forms.GroupBox()
        Me.uxRemoveReportOption = New System.Windows.Forms.Button()
        Me.uxImageList = New System.Windows.Forms.ImageList()
        Me.uxSaveReportOptions = New System.Windows.Forms.Button()
        Me.uxRepOpt = New System.Windows.Forms.Label()
        Me.uxAssignReportOption = New System.Windows.Forms.Button()
        Me.uxReportOption = New System.Windows.Forms.ComboBox()
        Me.uxReportOptions = New System.Windows.Forms.ListView()
        Me.uxReportOptCol = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.uxSubmissionClassification = New System.Windows.Forms.GroupBox()
        Me.uxRemoveCategory = New System.Windows.Forms.Button()
        Me.uxSaveCategories = New System.Windows.Forms.Button()
        Me.uxCat = New System.Windows.Forms.Label()
        Me.uxAssignCategory = New System.Windows.Forms.Button()
        Me.uxCategory = New System.Windows.Forms.ComboBox()
        Me.uxCategories = New System.Windows.Forms.ListView()
        Me.uxCategoryCol = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.uxClose = New System.Windows.Forms.Button()
        Me.uxTooltip = New System.Windows.Forms.ToolTip()
        Me.uxReportOpts.SuspendLayout()
        Me.uxSubmissionClassification.SuspendLayout()
        Me.SuspendLayout()
        '
        'uxReportOpts
        '
        Me.uxReportOpts.Controls.Add(Me.uxRemoveReportOption)
        Me.uxReportOpts.Controls.Add(Me.uxSaveReportOptions)
        Me.uxReportOpts.Controls.Add(Me.uxRepOpt)
        Me.uxReportOpts.Controls.Add(Me.uxAssignReportOption)
        Me.uxReportOpts.Controls.Add(Me.uxReportOption)
        Me.uxReportOpts.Controls.Add(Me.uxReportOptions)
        Me.uxReportOpts.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxReportOpts.Location = New System.Drawing.Point(6, 4)
        Me.uxReportOpts.Name = "uxReportOpts"
        Me.uxReportOpts.Size = New System.Drawing.Size(406, 228)
        Me.uxReportOpts.TabIndex = 0
        Me.uxReportOpts.TabStop = False
        Me.uxReportOpts.Text = "Author Tool Wizard Options"
        '
        'uxRemoveReportOption
        '
        Me.uxRemoveReportOption.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxRemoveReportOption.ImageAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.uxRemoveReportOption.ImageIndex = 2
        Me.uxRemoveReportOption.ImageList = Me.uxImageList
        Me.uxRemoveReportOption.Location = New System.Drawing.Point(338, 18)
        Me.uxRemoveReportOption.Name = "uxRemoveReportOption"
        Me.uxRemoveReportOption.Size = New System.Drawing.Size(28, 24)
        Me.uxRemoveReportOption.TabIndex = 8
        Me.uxTooltip.SetToolTip(Me.uxRemoveReportOption, "Assign Category")
        '
        'uxImageList
        '
        Me.uxImageList.ImageStream = CType(resources.GetObject("uxImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.uxImageList.TransparentColor = System.Drawing.Color.Transparent
        Me.uxImageList.Images.SetKeyName(0, "")
        Me.uxImageList.Images.SetKeyName(1, "")
        Me.uxImageList.Images.SetKeyName(2, "")
        Me.uxImageList.Images.SetKeyName(3, "")
        '
        'uxSaveReportOptions
        '
        Me.uxSaveReportOptions.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxSaveReportOptions.ImageAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.uxSaveReportOptions.ImageIndex = 1
        Me.uxSaveReportOptions.ImageList = Me.uxImageList
        Me.uxSaveReportOptions.Location = New System.Drawing.Point(372, 18)
        Me.uxSaveReportOptions.Name = "uxSaveReportOptions"
        Me.uxSaveReportOptions.Size = New System.Drawing.Size(28, 24)
        Me.uxSaveReportOptions.TabIndex = 7
        Me.uxTooltip.SetToolTip(Me.uxSaveReportOptions, "Save Variations")
        '
        'uxRepOpt
        '
        Me.uxRepOpt.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxRepOpt.Location = New System.Drawing.Point(15, 20)
        Me.uxRepOpt.Name = "uxRepOpt"
        Me.uxRepOpt.Size = New System.Drawing.Size(58, 21)
        Me.uxRepOpt.TabIndex = 3
        Me.uxRepOpt.Text = "Option:"
        Me.uxRepOpt.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'uxAssignReportOption
        '
        Me.uxAssignReportOption.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxAssignReportOption.ImageAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.uxAssignReportOption.ImageIndex = 0
        Me.uxAssignReportOption.ImageList = Me.uxImageList
        Me.uxAssignReportOption.Location = New System.Drawing.Point(304, 17)
        Me.uxAssignReportOption.Name = "uxAssignReportOption"
        Me.uxAssignReportOption.Size = New System.Drawing.Size(28, 24)
        Me.uxAssignReportOption.TabIndex = 2
        Me.uxTooltip.SetToolTip(Me.uxAssignReportOption, "Assign Report Option")
        '
        'uxReportOption
        '
        Me.uxReportOption.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.uxReportOption.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxReportOption.Location = New System.Drawing.Point(76, 20)
        Me.uxReportOption.Name = "uxReportOption"
        Me.uxReportOption.Size = New System.Drawing.Size(222, 21)
        Me.uxReportOption.TabIndex = 1
        '
        'uxReportOptions
        '
        Me.uxReportOptions.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.uxReportOptCol})
        Me.uxReportOptions.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxReportOptions.FullRowSelect = True
        Me.uxReportOptions.GridLines = True
        Me.uxReportOptions.HideSelection = False
        Me.uxReportOptions.Location = New System.Drawing.Point(4, 44)
        Me.uxReportOptions.Name = "uxReportOptions"
        Me.uxReportOptions.Size = New System.Drawing.Size(396, 180)
        Me.uxReportOptions.SmallImageList = Me.uxImageList
        Me.uxReportOptions.TabIndex = 0
        Me.uxReportOptions.UseCompatibleStateImageBehavior = False
        Me.uxReportOptions.View = System.Windows.Forms.View.Details
        '
        'uxReportOptCol
        '
        Me.uxReportOptCol.Text = "Option"
        Me.uxReportOptCol.Width = 375
        '
        'uxSubmissionClassification
        '
        Me.uxSubmissionClassification.Controls.Add(Me.uxRemoveCategory)
        Me.uxSubmissionClassification.Controls.Add(Me.uxSaveCategories)
        Me.uxSubmissionClassification.Controls.Add(Me.uxCat)
        Me.uxSubmissionClassification.Controls.Add(Me.uxAssignCategory)
        Me.uxSubmissionClassification.Controls.Add(Me.uxCategory)
        Me.uxSubmissionClassification.Controls.Add(Me.uxCategories)
        Me.uxSubmissionClassification.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxSubmissionClassification.Location = New System.Drawing.Point(7, 297)
        Me.uxSubmissionClassification.Name = "uxSubmissionClassification"
        Me.uxSubmissionClassification.Size = New System.Drawing.Size(405, 171)
        Me.uxSubmissionClassification.TabIndex = 1
        Me.uxSubmissionClassification.TabStop = False
        Me.uxSubmissionClassification.Text = "Document Submission Classification"
        Me.uxSubmissionClassification.Visible = False
        '
        'uxRemoveCategory
        '
        Me.uxRemoveCategory.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxRemoveCategory.ImageAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.uxRemoveCategory.ImageIndex = 2
        Me.uxRemoveCategory.ImageList = Me.uxImageList
        Me.uxRemoveCategory.Location = New System.Drawing.Point(337, 18)
        Me.uxRemoveCategory.Name = "uxRemoveCategory"
        Me.uxRemoveCategory.Size = New System.Drawing.Size(28, 24)
        Me.uxRemoveCategory.TabIndex = 7
        Me.uxTooltip.SetToolTip(Me.uxRemoveCategory, "Assign Category")
        '
        'uxSaveCategories
        '
        Me.uxSaveCategories.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxSaveCategories.ImageAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.uxSaveCategories.ImageIndex = 1
        Me.uxSaveCategories.ImageList = Me.uxImageList
        Me.uxSaveCategories.Location = New System.Drawing.Point(371, 18)
        Me.uxSaveCategories.Name = "uxSaveCategories"
        Me.uxSaveCategories.Size = New System.Drawing.Size(28, 24)
        Me.uxSaveCategories.TabIndex = 6
        Me.uxTooltip.SetToolTip(Me.uxSaveCategories, "Save Variations")
        '
        'uxCat
        '
        Me.uxCat.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxCat.Location = New System.Drawing.Point(6, 20)
        Me.uxCat.Name = "uxCat"
        Me.uxCat.Size = New System.Drawing.Size(66, 21)
        Me.uxCat.TabIndex = 4
        Me.uxCat.Text = "Category:"
        Me.uxCat.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'uxAssignCategory
        '
        Me.uxAssignCategory.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxAssignCategory.ImageAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.uxAssignCategory.ImageIndex = 0
        Me.uxAssignCategory.ImageList = Me.uxImageList
        Me.uxAssignCategory.Location = New System.Drawing.Point(303, 18)
        Me.uxAssignCategory.Name = "uxAssignCategory"
        Me.uxAssignCategory.Size = New System.Drawing.Size(28, 24)
        Me.uxAssignCategory.TabIndex = 3
        Me.uxTooltip.SetToolTip(Me.uxAssignCategory, "Assign Category")
        '
        'uxCategory
        '
        Me.uxCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.uxCategory.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxCategory.Location = New System.Drawing.Point(75, 20)
        Me.uxCategory.Name = "uxCategory"
        Me.uxCategory.Size = New System.Drawing.Size(222, 21)
        Me.uxCategory.TabIndex = 2
        '
        'uxCategories
        '
        Me.uxCategories.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.uxCategoryCol})
        Me.uxCategories.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxCategories.FullRowSelect = True
        Me.uxCategories.GridLines = True
        Me.uxCategories.HideSelection = False
        Me.uxCategories.Location = New System.Drawing.Point(4, 43)
        Me.uxCategories.Name = "uxCategories"
        Me.uxCategories.Size = New System.Drawing.Size(395, 180)
        Me.uxCategories.SmallImageList = Me.uxImageList
        Me.uxCategories.TabIndex = 0
        Me.uxCategories.UseCompatibleStateImageBehavior = False
        Me.uxCategories.View = System.Windows.Forms.View.Details
        '
        'uxCategoryCol
        '
        Me.uxCategoryCol.Text = "Category"
        Me.uxCategoryCol.Width = 375
        '
        'uxClose
        '
        Me.uxClose.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.uxClose.Location = New System.Drawing.Point(337, 238)
        Me.uxClose.Name = "uxClose"
        Me.uxClose.Size = New System.Drawing.Size(75, 23)
        Me.uxClose.TabIndex = 2
        Me.uxClose.Text = "Close"
        '
        'bc_am_bp_pub_type_params
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(418, 272)
        Me.Controls.Add(Me.uxClose)
        Me.Controls.Add(Me.uxSubmissionClassification)
        Me.Controls.Add(Me.uxReportOpts)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "bc_am_bp_pub_type_params"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Parameters"
        Me.uxReportOpts.ResumeLayout(False)
        Me.uxSubmissionClassification.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Friend WriteOnly Property Controller() As bc_am_publication_params

        Set(ByVal Value As bc_am_publication_params)
            ctrllr = Value
        End Set

    End Property

    Private Sub uxAssignCategory_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxAssignCategory.Click

        ctrllr.AssignClassification()

    End Sub

    Private Sub uxRemoveCategory_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxRemoveCategory.Click

        ctrllr.RemoveClassification()

    End Sub

    Private Sub uxSaveCategories_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxSaveCategories.Click

        ctrllr.SaveClassifications()

    End Sub

    Private Sub uxAssignReportOption_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxAssignReportOption.Click

        ctrllr.AssignReportOption()

    End Sub

    Private Sub uxRemoveReportOption_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxRemoveReportOption.Click

        ctrllr.RemoveReportOption()

    End Sub

    Private Sub uxSaveReportOptions_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxSaveReportOptions.Click

        ctrllr.SaveReportOptions()

    End Sub

    Private Sub uxReportOption_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxReportOption.SelectedIndexChanged

        ctrllr.Validate()

    End Sub

    Private Sub uxReportOptions_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxReportOptions.SelectedIndexChanged

        ctrllr.Validate()

    End Sub

    Private Sub uxCategory_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxCategory.SelectedIndexChanged

        ctrllr.Validate()

    End Sub

    Private Sub uxCategories_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxCategories.SelectedIndexChanged

        ctrllr.Validate()

    End Sub

    Private Sub bc_am_bp_pub_type_params_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing

        If uxSaveCategories.Enabled Or uxSaveReportOptions.Enabled Then
            If MessageBox.Show("You have unsaved changes.  Are you sure you wish to exit?", "Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
                e.Cancel = True
            End If
        End If

    End Sub

    Private Sub bc_am_bp_pub_type_params_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub uxClose_Click(sender As Object, e As EventArgs) Handles uxClose.Click

    End Sub
End Class
