<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bc_dx_am_create_wizard_frm
    Inherits DevExpress.XtraEditors.XtraForm

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_dx_am_create_wizard_frm))
        Me.WizardControl1 = New DevExpress.XtraWizard.WizardControl()
        Me.chkclone = New System.Windows.Forms.CheckBox()
        Me.WelcomeWizardPage1 = New DevExpress.XtraWizard.WelcomeWizardPage()
        Me.uxDocOptions = New DevExpress.XtraEditors.RadioGroup()
        Me.lname = New DevExpress.XtraEditors.LabelControl()
        Me.lrole = New DevExpress.XtraEditors.LabelControl()
        Me.WizardPublicationPage = New DevExpress.XtraWizard.WizardPage()
        Me.npubtypes = New DevExpress.XtraTreeList.TreeList()
        Me.TreeListColumn12 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.RepositoryItemDateEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemDateEdit()
        Me.RepositoryItemDateEdit2 = New DevExpress.XtraEditors.Repository.RepositoryItemDateEdit()
        Me.uxDocumentDetailImages = New DevExpress.Utils.ImageCollection()
        Me.uxPubtypes = New DevExpress.XtraEditors.ListBoxControl()
        Me.uxSelectedpubtype = New DevExpress.XtraEditors.LabelControl()
        Me.CompletionWizardPage1 = New DevExpress.XtraWizard.CompletionWizardPage()
        Me.WizardEquityPage1 = New DevExpress.XtraWizard.WizardPage()
        Me.lfiltername1 = New System.Windows.Forms.Label()
        Me.PanelControl3 = New DevExpress.XtraEditors.PanelControl()
        Me.tsearch = New DevExpress.XtraEditors.TextEdit()
        Me.lsearch = New DevExpress.XtraEditors.PictureEdit()
        Me.uxTnToggle = New DevExpress.XtraEditors.SimpleButton()
        Me.uxEntity1 = New DevExpress.XtraEditors.ListBoxControl()
        Me.WizardEquityPage2 = New DevExpress.XtraWizard.WizardPage()
        Me.lfiltername2 = New System.Windows.Forms.Label()
        Me.PanelControl2 = New DevExpress.XtraEditors.PanelControl()
        Me.tsearch2 = New DevExpress.XtraEditors.TextEdit()
        Me.lsearch2 = New DevExpress.XtraEditors.PictureEdit()
        Me.uxTnToggle2 = New DevExpress.XtraEditors.SimpleButton()
        Me.uxEntity2 = New DevExpress.XtraEditors.ListBoxControl()
        Me.searchtimer = New System.Windows.Forms.Timer()
        Me.searchtimer2 = New System.Windows.Forms.Timer()
        Me.listboxtimer = New System.Windows.Forms.Timer()
        CType(Me.WizardControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.WizardControl1.SuspendLayout()
        Me.WelcomeWizardPage1.SuspendLayout()
        CType(Me.uxDocOptions.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.WizardPublicationPage.SuspendLayout()
        CType(Me.npubtypes, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemDateEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemDateEdit1.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemDateEdit2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemDateEdit2.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxDocumentDetailImages, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxPubtypes, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.WizardEquityPage1.SuspendLayout()
        CType(Me.PanelControl3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl3.SuspendLayout()
        CType(Me.tsearch.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lsearch.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxEntity1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.WizardEquityPage2.SuspendLayout()
        CType(Me.PanelControl2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl2.SuspendLayout()
        CType(Me.tsearch2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lsearch2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxEntity2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'WizardControl1
        '
        Me.WizardControl1.Appearance.Page.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.WizardControl1.Appearance.Page.Options.UseFont = True
        Me.WizardControl1.Controls.Add(Me.chkclone)
        Me.WizardControl1.Controls.Add(Me.WelcomeWizardPage1)
        Me.WizardControl1.Controls.Add(Me.WizardPublicationPage)
        Me.WizardControl1.Controls.Add(Me.CompletionWizardPage1)
        Me.WizardControl1.Controls.Add(Me.WizardEquityPage1)
        Me.WizardControl1.Controls.Add(Me.WizardEquityPage2)
        Me.WizardControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.WizardControl1.Image = Global.BlueCurve.Create.AM.My.Resources.Resources.cw13
        Me.WizardControl1.ImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.WizardControl1.Location = New System.Drawing.Point(0, 0)
        Me.WizardControl1.Name = "WizardControl1"
        Me.WizardControl1.Pages.AddRange(New DevExpress.XtraWizard.BaseWizardPage() {Me.WelcomeWizardPage1, Me.WizardPublicationPage, Me.WizardEquityPage1, Me.WizardEquityPage2, Me.CompletionWizardPage1})
        Me.WizardControl1.Size = New System.Drawing.Size(504, 421)
        '
        'chkclone
        '
        Me.chkclone.AutoSize = True
        Me.chkclone.Location = New System.Drawing.Point(27, 390)
        Me.chkclone.Name = "chkclone"
        Me.chkclone.Size = New System.Drawing.Size(53, 17)
        Me.chkclone.TabIndex = 11
        Me.chkclone.Text = "Clone"
        Me.chkclone.UseVisualStyleBackColor = True
        Me.chkclone.Visible = False
        '
        'WelcomeWizardPage1
        '
        Me.WelcomeWizardPage1.Controls.Add(Me.uxDocOptions)
        Me.WelcomeWizardPage1.Controls.Add(Me.lname)
        Me.WelcomeWizardPage1.Controls.Add(Me.lrole)
        Me.WelcomeWizardPage1.IntroductionText = "Please select from one of the following options:"
        Me.WelcomeWizardPage1.Name = "WelcomeWizardPage1"
        Me.WelcomeWizardPage1.ProceedText = ""
        Me.WelcomeWizardPage1.Size = New System.Drawing.Size(287, 288)
        Me.WelcomeWizardPage1.Text = "Create Document Wizard"
        '
        'uxDocOptions
        '
        Me.uxDocOptions.Location = New System.Drawing.Point(15, 49)
        Me.uxDocOptions.Name = "uxDocOptions"
        Me.uxDocOptions.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxDocOptions.Properties.Appearance.Options.UseFont = True
        Me.uxDocOptions.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me.uxDocOptions.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.RadioGroupItem() {New DevExpress.XtraEditors.Controls.RadioGroupItem(CType(0, Short), "New Document"), New DevExpress.XtraEditors.Controls.RadioGroupItem(CType(1, Short), "Open Document")})
        Me.uxDocOptions.Size = New System.Drawing.Size(188, 93)
        Me.uxDocOptions.TabIndex = 0
        '
        'lname
        '
        Me.lname.Appearance.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lname.Appearance.ForeColor = System.Drawing.Color.Black
        Me.lname.Location = New System.Drawing.Point(3, 252)
        Me.lname.Name = "lname"
        Me.lname.Size = New System.Drawing.Size(27, 13)
        Me.lname.TabIndex = 11
        Me.lname.Text = "Name"
        '
        'lrole
        '
        Me.lrole.Appearance.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lrole.Appearance.ForeColor = System.Drawing.Color.Black
        Me.lrole.Location = New System.Drawing.Point(3, 272)
        Me.lrole.Name = "lrole"
        Me.lrole.Size = New System.Drawing.Size(21, 13)
        Me.lrole.TabIndex = 12
        Me.lrole.Text = "Role"
        '
        'WizardPublicationPage
        '
        Me.WizardPublicationPage.CausesValidation = True
        Me.WizardPublicationPage.Controls.Add(Me.npubtypes)
        Me.WizardPublicationPage.Controls.Add(Me.uxPubtypes)
        Me.WizardPublicationPage.Controls.Add(Me.uxSelectedpubtype)
        Me.WizardPublicationPage.DescriptionText = "Please select a publication type"
        Me.WizardPublicationPage.Name = "WizardPublicationPage"
        Me.WizardPublicationPage.Size = New System.Drawing.Size(472, 276)
        Me.WizardPublicationPage.Text = "Publication"
        '
        'npubtypes
        '
        Me.npubtypes.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.npubtypes.Appearance.SelectedRow.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.npubtypes.Appearance.SelectedRow.Options.UseFont = True
        Me.npubtypes.BestFitVisibleOnly = True
        Me.npubtypes.Columns.AddRange(New DevExpress.XtraTreeList.Columns.TreeListColumn() {Me.TreeListColumn12})
        Me.npubtypes.Location = New System.Drawing.Point(11, 14)
        Me.npubtypes.Name = "npubtypes"
        Me.npubtypes.OptionsFilter.ShowAllValuesInFilterPopup = True
        Me.npubtypes.OptionsMenu.EnableColumnMenu = False
        Me.npubtypes.OptionsMenu.EnableFooterMenu = False
        Me.npubtypes.OptionsSelection.EnableAppearanceFocusedCell = False
        Me.npubtypes.OptionsView.ShowHorzLines = False
        Me.npubtypes.OptionsView.ShowIndicator = False
        Me.npubtypes.OptionsView.ShowVertLines = False
        Me.npubtypes.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemDateEdit1, Me.RepositoryItemDateEdit2})
        Me.npubtypes.Size = New System.Drawing.Size(439, 223)
        Me.npubtypes.StateImageList = Me.uxDocumentDetailImages
        Me.npubtypes.TabIndex = 6
        Me.npubtypes.TreeLineStyle = DevExpress.XtraTreeList.LineStyle.None
        '
        'TreeListColumn12
        '
        Me.TreeListColumn12.Caption = "Publication"
        Me.TreeListColumn12.FieldName = "Title"
        Me.TreeListColumn12.ImageIndex = 6
        Me.TreeListColumn12.MinWidth = 33
        Me.TreeListColumn12.Name = "TreeListColumn12"
        Me.TreeListColumn12.OptionsColumn.AllowEdit = False
        Me.TreeListColumn12.OptionsFilter.FilterPopupMode = DevExpress.XtraTreeList.FilterPopupMode.CheckedList
        Me.TreeListColumn12.Visible = True
        Me.TreeListColumn12.VisibleIndex = 0
        Me.TreeListColumn12.Width = 147
        '
        'RepositoryItemDateEdit1
        '
        Me.RepositoryItemDateEdit1.AutoHeight = False
        Me.RepositoryItemDateEdit1.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.RepositoryItemDateEdit1.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.RepositoryItemDateEdit1.Name = "RepositoryItemDateEdit1"
        '
        'RepositoryItemDateEdit2
        '
        Me.RepositoryItemDateEdit2.AutoHeight = False
        Me.RepositoryItemDateEdit2.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.RepositoryItemDateEdit2.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.RepositoryItemDateEdit2.Name = "RepositoryItemDateEdit2"
        '
        'uxDocumentDetailImages
        '
        Me.uxDocumentDetailImages.ImageStream = CType(resources.GetObject("uxDocumentDetailImages.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
        Me.uxDocumentDetailImages.Images.SetKeyName(0, "folder_icon__psd__by_softarea-d4xeaef (1).jpg")
        Me.uxDocumentDetailImages.Images.SetKeyName(1, "Edit_Disabled_Txt.png")
        '
        'uxPubtypes
        '
        Me.uxPubtypes.Location = New System.Drawing.Point(11, 14)
        Me.uxPubtypes.Name = "uxPubtypes"
        Me.uxPubtypes.Size = New System.Drawing.Size(439, 223)
        Me.uxPubtypes.TabIndex = 5
        '
        'uxSelectedpubtype
        '
        Me.uxSelectedpubtype.Location = New System.Drawing.Point(18, 243)
        Me.uxSelectedpubtype.Name = "uxSelectedpubtype"
        Me.uxSelectedpubtype.Size = New System.Drawing.Size(66, 13)
        Me.uxSelectedpubtype.TabIndex = 3
        Me.uxSelectedpubtype.Text = "LabelControl1"
        '
        'CompletionWizardPage1
        '
        Me.CompletionWizardPage1.FinishText = "All done"
        Me.CompletionWizardPage1.Name = "CompletionWizardPage1"
        Me.CompletionWizardPage1.Size = New System.Drawing.Size(287, 288)
        Me.CompletionWizardPage1.Text = "Create  wizard"
        '
        'WizardEquityPage1
        '
        Me.WizardEquityPage1.Controls.Add(Me.lfiltername1)
        Me.WizardEquityPage1.Controls.Add(Me.PanelControl3)
        Me.WizardEquityPage1.Controls.Add(Me.uxTnToggle)
        Me.WizardEquityPage1.Controls.Add(Me.uxEntity1)
        Me.WizardEquityPage1.DescriptionText = "Please Select an Equity"
        Me.WizardEquityPage1.Name = "WizardEquityPage1"
        Me.WizardEquityPage1.Size = New System.Drawing.Size(472, 276)
        Me.WizardEquityPage1.Text = "Equity"
        '
        'lfiltername1
        '
        Me.lfiltername1.AutoSize = True
        Me.lfiltername1.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lfiltername1.Location = New System.Drawing.Point(18, 260)
        Me.lfiltername1.Name = "lfiltername1"
        Me.lfiltername1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lfiltername1.Size = New System.Drawing.Size(38, 13)
        Me.lfiltername1.TabIndex = 3
        Me.lfiltername1.Text = "Label1"
        '
        'PanelControl3
        '
        Me.PanelControl3.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me.PanelControl3.Controls.Add(Me.tsearch)
        Me.PanelControl3.Controls.Add(Me.lsearch)
        Me.PanelControl3.Location = New System.Drawing.Point(18, 230)
        Me.PanelControl3.Name = "PanelControl3"
        Me.PanelControl3.Size = New System.Drawing.Size(266, 27)
        Me.PanelControl3.TabIndex = 2
        '
        'tsearch
        '
        Me.tsearch.EditValue = ""
        Me.tsearch.Location = New System.Drawing.Point(35, 3)
        Me.tsearch.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.tsearch.Name = "tsearch"
        Me.tsearch.Properties.MaxLength = 250
        Me.tsearch.Size = New System.Drawing.Size(218, 20)
        Me.tsearch.TabIndex = 45
        '
        'lsearch
        '
        Me.lsearch.EditValue = CType(resources.GetObject("lsearch.EditValue"), Object)
        Me.lsearch.Location = New System.Drawing.Point(3, 3)
        Me.lsearch.Name = "lsearch"
        Me.lsearch.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom
        Me.lsearch.Size = New System.Drawing.Size(26, 20)
        Me.lsearch.TabIndex = 1
        '
        'uxTnToggle
        '
        Me.uxTnToggle.Location = New System.Drawing.Point(382, 233)
        Me.uxTnToggle.Name = "uxTnToggle"
        Me.uxTnToggle.Size = New System.Drawing.Size(75, 24)
        Me.uxTnToggle.TabIndex = 1
        Me.uxTnToggle.Text = "My Subjects"
        '
        'uxEntity1
        '
        Me.uxEntity1.IncrementalSearch = True
        Me.uxEntity1.Location = New System.Drawing.Point(18, 14)
        Me.uxEntity1.Name = "uxEntity1"
        Me.uxEntity1.Size = New System.Drawing.Size(439, 200)
        Me.uxEntity1.TabIndex = 0
        '
        'WizardEquityPage2
        '
        Me.WizardEquityPage2.Controls.Add(Me.lfiltername2)
        Me.WizardEquityPage2.Controls.Add(Me.PanelControl2)
        Me.WizardEquityPage2.Controls.Add(Me.uxTnToggle2)
        Me.WizardEquityPage2.Controls.Add(Me.uxEntity2)
        Me.WizardEquityPage2.DescriptionText = "Please Select an Equity"
        Me.WizardEquityPage2.Name = "WizardEquityPage2"
        Me.WizardEquityPage2.Size = New System.Drawing.Size(472, 276)
        Me.WizardEquityPage2.Text = "Second Equity"
        '
        'lfiltername2
        '
        Me.lfiltername2.AutoSize = True
        Me.lfiltername2.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lfiltername2.Location = New System.Drawing.Point(18, 260)
        Me.lfiltername2.Name = "lfiltername2"
        Me.lfiltername2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lfiltername2.Size = New System.Drawing.Size(38, 13)
        Me.lfiltername2.TabIndex = 4
        Me.lfiltername2.Text = "Label1"
        '
        'PanelControl2
        '
        Me.PanelControl2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me.PanelControl2.Controls.Add(Me.tsearch2)
        Me.PanelControl2.Controls.Add(Me.lsearch2)
        Me.PanelControl2.Location = New System.Drawing.Point(18, 230)
        Me.PanelControl2.Name = "PanelControl2"
        Me.PanelControl2.Size = New System.Drawing.Size(280, 27)
        Me.PanelControl2.TabIndex = 3
        '
        'tsearch2
        '
        Me.tsearch2.EditValue = ""
        Me.tsearch2.Location = New System.Drawing.Point(35, 3)
        Me.tsearch2.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.tsearch2.Name = "tsearch2"
        Me.tsearch2.Properties.MaxLength = 250
        Me.tsearch2.Size = New System.Drawing.Size(218, 20)
        Me.tsearch2.TabIndex = 45
        '
        'lsearch2
        '
        Me.lsearch2.EditValue = CType(resources.GetObject("lsearch2.EditValue"), Object)
        Me.lsearch2.Location = New System.Drawing.Point(3, 3)
        Me.lsearch2.Name = "lsearch2"
        Me.lsearch2.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom
        Me.lsearch2.Size = New System.Drawing.Size(26, 20)
        Me.lsearch2.TabIndex = 1
        '
        'uxTnToggle2
        '
        Me.uxTnToggle2.Location = New System.Drawing.Point(382, 233)
        Me.uxTnToggle2.Name = "uxTnToggle2"
        Me.uxTnToggle2.Size = New System.Drawing.Size(75, 24)
        Me.uxTnToggle2.TabIndex = 2
        Me.uxTnToggle2.Text = "My Subjects"
        '
        'uxEntity2
        '
        Me.uxEntity2.IncrementalSearch = True
        Me.uxEntity2.Location = New System.Drawing.Point(18, 14)
        Me.uxEntity2.Name = "uxEntity2"
        Me.uxEntity2.Size = New System.Drawing.Size(439, 200)
        Me.uxEntity2.TabIndex = 1
        '
        'searchtimer
        '
        Me.searchtimer.Enabled = True
        Me.searchtimer.Interval = 500
        '
        'searchtimer2
        '
        Me.searchtimer2.Enabled = True
        Me.searchtimer2.Interval = 500
        '
        'listboxtimer
        '
        Me.listboxtimer.Interval = 3000
        '
        'bc_dx_am_create_wizard_frm
        '
        Me.Appearance.Options.UseFont = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(504, 421)
        Me.Controls.Add(Me.WizardControl1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "bc_dx_am_create_wizard_frm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Blue Curve Create - Document Wizard"
        CType(Me.WizardControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.WizardControl1.ResumeLayout(False)
        Me.WizardControl1.PerformLayout()
        Me.WelcomeWizardPage1.ResumeLayout(False)
        Me.WelcomeWizardPage1.PerformLayout()
        CType(Me.uxDocOptions.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.WizardPublicationPage.ResumeLayout(False)
        Me.WizardPublicationPage.PerformLayout()
        CType(Me.npubtypes, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemDateEdit1.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemDateEdit1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemDateEdit2.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemDateEdit2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxDocumentDetailImages, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxPubtypes, System.ComponentModel.ISupportInitialize).EndInit()
        Me.WizardEquityPage1.ResumeLayout(False)
        Me.WizardEquityPage1.PerformLayout()
        CType(Me.PanelControl3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl3.ResumeLayout(False)
        CType(Me.tsearch.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lsearch.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxEntity1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.WizardEquityPage2.ResumeLayout(False)
        Me.WizardEquityPage2.PerformLayout()
        CType(Me.PanelControl2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl2.ResumeLayout(False)
        CType(Me.tsearch2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lsearch2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxEntity2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents WizardControl1 As DevExpress.XtraWizard.WizardControl
    Friend WithEvents WelcomeWizardPage1 As DevExpress.XtraWizard.WelcomeWizardPage
    Friend WithEvents WizardPublicationPage As DevExpress.XtraWizard.WizardPage
    Friend WithEvents CompletionWizardPage1 As DevExpress.XtraWizard.CompletionWizardPage
    Friend WithEvents WizardEquityPage1 As DevExpress.XtraWizard.WizardPage
    Friend WithEvents uxSelectedpubtype As DevExpress.XtraEditors.LabelControl
    Friend WithEvents uxEntity1 As DevExpress.XtraEditors.ListBoxControl
    Friend WithEvents uxDocOptions As DevExpress.XtraEditors.RadioGroup
    Friend WithEvents uxPubtypes As DevExpress.XtraEditors.ListBoxControl
    Friend WithEvents WizardEquityPage2 As DevExpress.XtraWizard.WizardPage
    Friend WithEvents uxEntity2 As DevExpress.XtraEditors.ListBoxControl
    Friend WithEvents uxTnToggle As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents uxTnToggle2 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents PanelControl3 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents lsearch As DevExpress.XtraEditors.PictureEdit
    Friend WithEvents tsearch As DevExpress.XtraEditors.TextEdit
    Friend WithEvents searchtimer As System.Windows.Forms.Timer
    Friend WithEvents PanelControl2 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents tsearch2 As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lsearch2 As DevExpress.XtraEditors.PictureEdit
    Friend WithEvents searchtimer2 As System.Windows.Forms.Timer

    'Public Sub New()

    '    ' This call is required by the Windows Form Designer.
    '    InitializeComponent()

    '    ' Add any initialization after the InitializeComponent() call.



    'End Sub
    Friend WithEvents lrole As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lname As DevExpress.XtraEditors.LabelControl
    Friend WithEvents chkclone As System.Windows.Forms.CheckBox
    Friend WithEvents listboxtimer As System.Windows.Forms.Timer
    Friend WithEvents lfiltername1 As System.Windows.Forms.Label
    Friend WithEvents lfiltername2 As System.Windows.Forms.Label
    Friend WithEvents npubtypes As DevExpress.XtraTreeList.TreeList
    Friend WithEvents TreeListColumn12 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents RepositoryItemDateEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemDateEdit
    Friend WithEvents RepositoryItemDateEdit2 As DevExpress.XtraEditors.Repository.RepositoryItemDateEdit
    Friend WithEvents uxDocumentDetailImages As DevExpress.Utils.ImageCollection
End Class
