<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bc_am_cp_translation
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
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_cp_translation))
        Me.TraslatorSplit = New System.Windows.Forms.SplitContainer
        Me.uxGroupTree = New System.Windows.Forms.TreeView
        Me.tabimages = New System.Windows.Forms.ImageList(Me.components)
        Me.uxSearch = New BlueCurve.Core.[AS].BlueCurve_TextSearch
        Me.uxTrandiscard = New System.Windows.Forms.Button
        Me.uxTranchanges = New System.Windows.Forms.Button
        Me.TranslationGrid = New System.Windows.Forms.DataGridView
        Me.classname = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.title = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.fielddesc = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.defaultvalue = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.TraslatorSplit.Panel1.SuspendLayout()
        Me.TraslatorSplit.Panel2.SuspendLayout()
        Me.TraslatorSplit.SuspendLayout()
        CType(Me.TranslationGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TraslatorSplit
        '
        Me.TraslatorSplit.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.TraslatorSplit.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TraslatorSplit.Location = New System.Drawing.Point(0, 0)
        Me.TraslatorSplit.Name = "TraslatorSplit"
        '
        'TraslatorSplit.Panel1
        '
        Me.TraslatorSplit.Panel1.Controls.Add(Me.uxGroupTree)
        '
        'TraslatorSplit.Panel2
        '
        Me.TraslatorSplit.Panel2.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.TraslatorSplit.Panel2.Controls.Add(Me.uxSearch)
        Me.TraslatorSplit.Panel2.Controls.Add(Me.uxTrandiscard)
        Me.TraslatorSplit.Panel2.Controls.Add(Me.uxTranchanges)
        Me.TraslatorSplit.Panel2.Controls.Add(Me.TranslationGrid)
        Me.TraslatorSplit.Size = New System.Drawing.Size(639, 430)
        Me.TraslatorSplit.SplitterDistance = 153
        Me.TraslatorSplit.TabIndex = 0
        '
        'uxGroupTree
        '
        Me.uxGroupTree.Dock = System.Windows.Forms.DockStyle.Fill
        Me.uxGroupTree.ImageIndex = 12
        Me.uxGroupTree.ImageList = Me.tabimages
        Me.uxGroupTree.Location = New System.Drawing.Point(0, 0)
        Me.uxGroupTree.Name = "uxGroupTree"
        Me.uxGroupTree.SelectedImageIndex = 11
        Me.uxGroupTree.Size = New System.Drawing.Size(153, 430)
        Me.uxGroupTree.TabIndex = 11
        '
        'tabimages
        '
        Me.tabimages.ImageStream = CType(resources.GetObject("tabimages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.tabimages.TransparentColor = System.Drawing.Color.Transparent
        Me.tabimages.Images.SetKeyName(0, "Links")
        Me.tabimages.Images.SetKeyName(1, "Attributes")
        Me.tabimages.Images.SetKeyName(2, "publication")
        Me.tabimages.Images.SetKeyName(3, "number")
        Me.tabimages.Images.SetKeyName(4, "date")
        Me.tabimages.Images.SetKeyName(5, "string")
        Me.tabimages.Images.SetKeyName(6, "data")
        Me.tabimages.Images.SetKeyName(7, "users")
        Me.tabimages.Images.SetKeyName(8, "user")
        Me.tabimages.Images.SetKeyName(9, "parent")
        Me.tabimages.Images.SetKeyName(10, "child")
        Me.tabimages.Images.SetKeyName(11, "selected")
        Me.tabimages.Images.SetKeyName(12, "Folder")
        Me.tabimages.Images.SetKeyName(13, "New")
        '
        'uxSearch
        '
        Me.uxSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxSearch.BackColor = System.Drawing.Color.Transparent
        Me.uxSearch.Location = New System.Drawing.Point(196, 402)
        Me.uxSearch.MaximumSize = New System.Drawing.Size(30000, 30000)
        Me.uxSearch.MinimumSize = New System.Drawing.Size(150, 21)
        Me.uxSearch.Name = "uxSearch"
        Me.uxSearch.SearchAttributeList = Nothing
        Me.uxSearch.SearchAttributes = 0
        Me.uxSearch.SearchBuildEntitiesOnly = False
        Me.uxSearch.SearchClass = 0
        Me.uxSearch.SearchControl = Nothing
        Me.uxSearch.SearchCurrentAttribute = Nothing
        Me.uxSearch.SearchGetInactive = False
        Me.uxSearch.SearchText = ""
        Me.uxSearch.SearchUserEntitiesOnly = False
        Me.uxSearch.Size = New System.Drawing.Size(206, 21)
        Me.uxSearch.TabIndex = 58
        '
        'uxTrandiscard
        '
        Me.uxTrandiscard.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxTrandiscard.Enabled = False
        Me.uxTrandiscard.Image = CType(resources.GetObject("uxTrandiscard.Image"), System.Drawing.Image)
        Me.uxTrandiscard.Location = New System.Drawing.Point(442, 399)
        Me.uxTrandiscard.Name = "uxTrandiscard"
        Me.uxTrandiscard.Size = New System.Drawing.Size(28, 24)
        Me.uxTrandiscard.TabIndex = 57
        Me.uxTrandiscard.UseVisualStyleBackColor = True
        '
        'uxTranchanges
        '
        Me.uxTranchanges.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxTranchanges.Enabled = False
        Me.uxTranchanges.Image = CType(resources.GetObject("uxTranchanges.Image"), System.Drawing.Image)
        Me.uxTranchanges.Location = New System.Drawing.Point(408, 399)
        Me.uxTranchanges.Name = "uxTranchanges"
        Me.uxTranchanges.Size = New System.Drawing.Size(28, 24)
        Me.uxTranchanges.TabIndex = 56
        Me.uxTranchanges.UseVisualStyleBackColor = True
        '
        'TranslationGrid
        '
        Me.TranslationGrid.AllowUserToAddRows = False
        Me.TranslationGrid.AllowUserToDeleteRows = False
        Me.TranslationGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TranslationGrid.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.TranslationGrid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.TranslationGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.TranslationGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.classname, Me.title, Me.fielddesc, Me.defaultvalue})
        Me.TranslationGrid.Location = New System.Drawing.Point(2, 0)
        Me.TranslationGrid.MultiSelect = False
        Me.TranslationGrid.Name = "TranslationGrid"
        Me.TranslationGrid.RowHeadersVisible = False
        Me.TranslationGrid.Size = New System.Drawing.Size(477, 393)
        Me.TranslationGrid.TabIndex = 0
        '
        'classname
        '
        Me.classname.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.classname.HeaderText = "Class"
        Me.classname.Name = "classname"
        Me.classname.ReadOnly = True
        '
        'title
        '
        Me.title.HeaderText = "Item Title"
        Me.title.Name = "title"
        Me.title.ReadOnly = True
        '
        'fielddesc
        '
        Me.fielddesc.HeaderText = "Field Description"
        Me.fielddesc.Name = "fielddesc"
        '
        'defaultvalue
        '
        Me.defaultvalue.HeaderText = "Default Value"
        Me.defaultvalue.Name = "defaultvalue"
        '
        'bc_am_cp_language
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(639, 430)
        Me.Controls.Add(Me.TraslatorSplit)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "bc_am_cp_language"
        Me.TraslatorSplit.Panel1.ResumeLayout(False)
        Me.TraslatorSplit.Panel2.ResumeLayout(False)
        Me.TraslatorSplit.ResumeLayout(False)
        CType(Me.TranslationGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TraslatorSplit As System.Windows.Forms.SplitContainer
    Friend WithEvents TranslationGrid As System.Windows.Forms.DataGridView
    Friend WithEvents uxTrandiscard As System.Windows.Forms.Button
    Friend WithEvents uxTranchanges As System.Windows.Forms.Button
    Friend WithEvents classname As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents title As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents fielddesc As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents defaultvalue As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents uxGroupTree As System.Windows.Forms.TreeView
    Friend WithEvents tabimages As System.Windows.Forms.ImageList
    Friend WithEvents uxSearch As BlueCurve.Core.AS.BlueCurve_TextSearch

End Class
