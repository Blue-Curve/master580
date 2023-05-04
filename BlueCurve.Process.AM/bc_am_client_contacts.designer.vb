<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bc_am_client_contacts
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_client_contacts))
        Me.PanelControl3 = New DevExpress.XtraEditors.PanelControl()
        Me.bcancel = New DevExpress.XtraEditors.SimpleButton()
        Me.bsave = New DevExpress.XtraEditors.SimpleButton()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.uxrecipient = New DevExpress.XtraTreeList.TreeList()
        Me.TreeListColumn15 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.TreeListColumn16 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.TreeListColumn18 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.RepositoryItemDateEdit4 = New DevExpress.XtraEditors.Repository.RepositoryItemDateEdit()
        Me.RepositoryItemDateEdit5 = New DevExpress.XtraEditors.Repository.RepositoryItemDateEdit()
        Me.uxsel = New DevExpress.XtraTreeList.TreeList()
        Me.TreeListColumn1 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.TreeListColumn2 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.TreeListColumn3 = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.RepositoryItemDateEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemDateEdit()
        Me.RepositoryItemDateEdit2 = New DevExpress.XtraEditors.Repository.RepositoryItemDateEdit()
        Me.tsearch = New DevExpress.XtraEditors.TextEdit()
        Me.ldesc = New DevExpress.XtraEditors.LabelControl()
        Me.Timer1 = New System.Windows.Forms.Timer()
        Me.ball = New DevExpress.XtraEditors.SimpleButton()
        CType(Me.PanelControl3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl3.SuspendLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxrecipient, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemDateEdit4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemDateEdit4.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemDateEdit5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemDateEdit5.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxsel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemDateEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemDateEdit1.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemDateEdit2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemDateEdit2.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.tsearch.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PanelControl3
        '
        Me.PanelControl3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PanelControl3.Controls.Add(Me.bcancel)
        Me.PanelControl3.Controls.Add(Me.bsave)
        Me.PanelControl3.Controls.Add(Me.PictureBox2)
        Me.PanelControl3.Location = New System.Drawing.Point(2, 464)
        Me.PanelControl3.Name = "PanelControl3"
        Me.PanelControl3.Size = New System.Drawing.Size(982, 49)
        Me.PanelControl3.TabIndex = 3
        '
        'bcancel
        '
        Me.bcancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bcancel.ImageIndex = 4
        Me.bcancel.Location = New System.Drawing.Point(872, 9)
        Me.bcancel.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.bcancel.Name = "bcancel"
        Me.bcancel.Size = New System.Drawing.Size(75, 33)
        Me.bcancel.TabIndex = 86
        Me.bcancel.Text = "Cancel"
        '
        'bsave
        '
        Me.bsave.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bsave.ImageIndex = 4
        Me.bsave.Location = New System.Drawing.Point(780, 9)
        Me.bsave.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.bsave.Name = "bsave"
        Me.bsave.Size = New System.Drawing.Size(75, 33)
        Me.bsave.TabIndex = 85
        Me.bsave.Text = "Save"
        '
        'PictureBox2
        '
        Me.PictureBox2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(5, 9)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(175, 35)
        Me.PictureBox2.TabIndex = 42
        Me.PictureBox2.TabStop = False
        '
        'uxrecipient
        '
        Me.uxrecipient.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxrecipient.Columns.AddRange(New DevExpress.XtraTreeList.Columns.TreeListColumn() {Me.TreeListColumn15, Me.TreeListColumn16, Me.TreeListColumn18})
        Me.uxrecipient.Location = New System.Drawing.Point(7, 57)
        Me.uxrecipient.Name = "uxrecipient"
        Me.uxrecipient.OptionsBehavior.Editable = False
        Me.uxrecipient.OptionsBehavior.EnableFiltering = True
        Me.uxrecipient.OptionsFilter.FilterMode = DevExpress.XtraTreeList.FilterMode.Standard
        Me.uxrecipient.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemDateEdit4, Me.RepositoryItemDateEdit5})
        Me.uxrecipient.Size = New System.Drawing.Size(448, 384)
        Me.uxrecipient.TabIndex = 47
        Me.uxrecipient.TreeLevelWidth = 12
        '
        'TreeListColumn15
        '
        Me.TreeListColumn15.Caption = "Name"
        Me.TreeListColumn15.FieldName = "Channel"
        Me.TreeListColumn15.MinWidth = 33
        Me.TreeListColumn15.Name = "TreeListColumn15"
        Me.TreeListColumn15.Visible = True
        Me.TreeListColumn15.VisibleIndex = 0
        '
        'TreeListColumn16
        '
        Me.TreeListColumn16.Caption = "Organisation"
        Me.TreeListColumn16.FieldName = "Status"
        Me.TreeListColumn16.Name = "TreeListColumn16"
        Me.TreeListColumn16.Visible = True
        Me.TreeListColumn16.VisibleIndex = 1
        '
        'TreeListColumn18
        '
        Me.TreeListColumn18.Caption = "Email"
        Me.TreeListColumn18.FieldName = "Mail List Date"
        Me.TreeListColumn18.Name = "TreeListColumn18"
        Me.TreeListColumn18.Visible = True
        Me.TreeListColumn18.VisibleIndex = 2
        '
        'RepositoryItemDateEdit4
        '
        Me.RepositoryItemDateEdit4.AutoHeight = False
        Me.RepositoryItemDateEdit4.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.RepositoryItemDateEdit4.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.RepositoryItemDateEdit4.Name = "RepositoryItemDateEdit4"
        '
        'RepositoryItemDateEdit5
        '
        Me.RepositoryItemDateEdit5.AutoHeight = False
        Me.RepositoryItemDateEdit5.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.RepositoryItemDateEdit5.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.RepositoryItemDateEdit5.Name = "RepositoryItemDateEdit5"
        '
        'uxsel
        '
        Me.uxsel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxsel.Columns.AddRange(New DevExpress.XtraTreeList.Columns.TreeListColumn() {Me.TreeListColumn1, Me.TreeListColumn2, Me.TreeListColumn3})
        Me.uxsel.Location = New System.Drawing.Point(501, 57)
        Me.uxsel.Name = "uxsel"
        Me.uxsel.OptionsBehavior.Editable = False
        Me.uxsel.OptionsBehavior.EnableFiltering = True
        Me.uxsel.OptionsFilter.FilterMode = DevExpress.XtraTreeList.FilterMode.Standard
        Me.uxsel.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemDateEdit1, Me.RepositoryItemDateEdit2})
        Me.uxsel.Size = New System.Drawing.Size(448, 384)
        Me.uxsel.TabIndex = 48
        Me.uxsel.TreeLevelWidth = 12
        '
        'TreeListColumn1
        '
        Me.TreeListColumn1.Caption = "Name"
        Me.TreeListColumn1.FieldName = "Channel"
        Me.TreeListColumn1.MinWidth = 33
        Me.TreeListColumn1.Name = "TreeListColumn1"
        Me.TreeListColumn1.Visible = True
        Me.TreeListColumn1.VisibleIndex = 0
        '
        'TreeListColumn2
        '
        Me.TreeListColumn2.Caption = "Organisation"
        Me.TreeListColumn2.FieldName = "Status"
        Me.TreeListColumn2.Name = "TreeListColumn2"
        Me.TreeListColumn2.Visible = True
        Me.TreeListColumn2.VisibleIndex = 1
        '
        'TreeListColumn3
        '
        Me.TreeListColumn3.Caption = "Email"
        Me.TreeListColumn3.FieldName = "Mail List Date"
        Me.TreeListColumn3.Name = "TreeListColumn3"
        Me.TreeListColumn3.Visible = True
        Me.TreeListColumn3.VisibleIndex = 2
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
        'tsearch
        '
        Me.tsearch.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tsearch.Location = New System.Drawing.Point(7, 21)
        Me.tsearch.Name = "tsearch"
        Me.tsearch.Size = New System.Drawing.Size(335, 20)
        Me.tsearch.TabIndex = 49
        '
        'ldesc
        '
        Me.ldesc.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ldesc.Location = New System.Drawing.Point(501, 28)
        Me.ldesc.Name = "ldesc"
        Me.ldesc.Size = New System.Drawing.Size(41, 13)
        Me.ldesc.TabIndex = 50
        Me.ldesc.Text = "Selected"
        '
        'Timer1
        '
        Me.Timer1.Interval = 1000
        '
        'ball
        '
        Me.ball.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ball.ImageIndex = 4
        Me.ball.Location = New System.Drawing.Point(380, 14)
        Me.ball.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.ball.Name = "ball"
        Me.ball.Size = New System.Drawing.Size(75, 33)
        Me.ball.TabIndex = 84
        Me.ball.Text = "All"
        '
        'bc_am_client_contacts
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(983, 513)
        Me.Controls.Add(Me.ball)
        Me.Controls.Add(Me.ldesc)
        Me.Controls.Add(Me.tsearch)
        Me.Controls.Add(Me.uxsel)
        Me.Controls.Add(Me.uxrecipient)
        Me.Controls.Add(Me.PanelControl3)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimizeBox = False
        Me.Name = "bc_am_client_contacts"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = " "
        CType(Me.PanelControl3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl3.ResumeLayout(False)
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxrecipient, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemDateEdit4.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemDateEdit4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemDateEdit5.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemDateEdit5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxsel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemDateEdit1.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemDateEdit1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemDateEdit2.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemDateEdit2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.tsearch.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PanelControl3 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents uxrecipient As DevExpress.XtraTreeList.TreeList
    Friend WithEvents TreeListColumn15 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents TreeListColumn16 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents TreeListColumn18 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents RepositoryItemDateEdit4 As DevExpress.XtraEditors.Repository.RepositoryItemDateEdit
    Friend WithEvents RepositoryItemDateEdit5 As DevExpress.XtraEditors.Repository.RepositoryItemDateEdit
    Friend WithEvents uxsel As DevExpress.XtraTreeList.TreeList
    Friend WithEvents TreeListColumn1 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents TreeListColumn2 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents TreeListColumn3 As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents RepositoryItemDateEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemDateEdit
    Friend WithEvents RepositoryItemDateEdit2 As DevExpress.XtraEditors.Repository.RepositoryItemDateEdit
    Friend WithEvents tsearch As DevExpress.XtraEditors.TextEdit
    Friend WithEvents ldesc As DevExpress.XtraEditors.LabelControl
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents bcancel As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents bsave As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents ball As DevExpress.XtraEditors.SimpleButton
End Class
