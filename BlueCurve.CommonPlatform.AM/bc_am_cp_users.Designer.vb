<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bc_am_cp_users
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
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_cp_users))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.Column3 = New System.Windows.Forms.DataGridViewImageColumn()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewImageColumn1 = New System.Windows.Forms.DataGridViewImageColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Publish = New System.Windows.Forms.DataGridViewImageColumn()
        Me.z = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.puserpic = New System.Windows.Forms.PictureBox()
        Me.Pnouser = New System.Windows.Forms.PictureBox()
        Me.attributedetails = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mdettitle = New System.Windows.Forms.ToolStripTextBox()
        Me.mdetupdate = New System.Windows.Forms.ToolStripTextBox()
        Me.mdetuser = New System.Windows.Forms.ToolStripTextBox()
        Me.mdetpubupdate = New System.Windows.Forms.ToolStripTextBox()
        Me.mdetpubuser = New System.Windows.Forms.ToolStripTextBox()
        Me.UserContextMenuStrip = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AddEntityToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator31 = New System.Windows.Forms.ToolStripSeparator()
        Me.minactive = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator29 = New System.Windows.Forms.ToolStripSeparator()
        Me.DeleteEntityToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.pendingsync = New System.Windows.Forms.ToolStripMenuItem()
        Me.cpendingsync = New System.Windows.Forms.ToolStripMenuItem()
        Me.mpt = New System.Windows.Forms.ToolStripMenuItem()
        Me.mps = New System.Windows.Forms.ToolStripMenuItem()
        Me.mpe = New System.Windows.Forms.ToolStripMenuItem()
        Me.maintainitemscsm = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ChangeNameToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mactive = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeleteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.addpt = New System.Windows.Forms.ToolStripMenuItem()
        Me.rept = New System.Windows.Forms.ToolStripMenuItem()
        Me.dept = New System.Windows.Forms.ToolStripMenuItem()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.TabPage5 = New System.Windows.Forms.TabPage()
        Me.assignentity = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AddAssociatedClassToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.massignentity = New System.Windows.Forms.ToolStripMenuItem()
        Me.IncreaseRatingToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DecreaseRatingToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.pclass = New System.Windows.Forms.Panel()
        Me.lattributes = New System.Windows.Forms.Label()
        Me.tusers = New System.Windows.Forms.TreeView()
        Me.tabimages = New System.Windows.Forms.ImageList(Me.components)
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer()
        Me.pusers = New System.Windows.Forms.Panel()
        Me.tfilter = New BlueCurve.Core.[AS].BlueCurve_TextSearch()
        Me.Lusers = New System.Windows.Forms.ListView()
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.SplitContainer3 = New System.Windows.Forms.SplitContainer()
        Me.dpartialsync = New System.Windows.Forms.DataGridView()
        Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column5 = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.Ttitle = New System.Windows.Forms.TabControl()
        Me.TabPage6 = New System.Windows.Forms.TabPage()
        Me.tusername = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.TabPage7 = New System.Windows.Forms.TabPage()
        Me.uxuserpassword = New System.Windows.Forms.TextBox()
        Me.puser = New System.Windows.Forms.Panel()
        Me.lvwvalidate = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Tuserass = New System.Windows.Forms.TabControl()
        Me.TabPage4 = New System.Windows.Forms.TabPage()
        Me.bentdiscard = New System.Windows.Forms.Button()
        Me.bentchanges = New System.Windows.Forms.Button()
        Me.tentities = New System.Windows.Forms.TreeView()
        Me.puserdn = New System.Windows.Forms.PictureBox()
        Me.puserup = New System.Windows.Forms.PictureBox()
        Me.baudit = New System.Windows.Forms.Button()
        Me.buseraudit = New System.Windows.Forms.Button()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.puserpic, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Pnouser, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.attributedetails.SuspendLayout()
        Me.UserContextMenuStrip.SuspendLayout()
        Me.maintainitemscsm.SuspendLayout()
        Me.assignentity.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.pclass.SuspendLayout()
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.Panel2.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        Me.pusers.SuspendLayout()
        CType(Me.SplitContainer3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer3.Panel1.SuspendLayout()
        Me.SplitContainer3.Panel2.SuspendLayout()
        Me.SplitContainer3.SuspendLayout()
        CType(Me.dpartialsync, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Ttitle.SuspendLayout()
        Me.tusername.SuspendLayout()
        Me.puser.SuspendLayout()
        Me.Tuserass.SuspendLayout()
        CType(Me.puserdn, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.puserup, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.AllowUserToResizeRows = False
        Me.DataGridView1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGridView1.BackgroundColor = System.Drawing.SystemColors.Window
        Me.DataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column3, Me.Column1, Me.DataGridViewImageColumn1, Me.Column2, Me.Publish, Me.z})
        Me.DataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me.DataGridView1.Location = New System.Drawing.Point(0, 50)
        Me.DataGridView1.MultiSelect = False
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        Me.DataGridView1.RowHeadersVisible = False
        Me.DataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders
        Me.DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.DataGridView1.Size = New System.Drawing.Size(348, 129)
        Me.DataGridView1.TabIndex = 73
        Me.ToolTip1.SetToolTip(Me.DataGridView1, "Displays attributes and values")
        '
        'Column3
        '
        Me.Column3.HeaderText = ""
        Me.Column3.Name = "Column3"
        Me.Column3.ReadOnly = True
        Me.Column3.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Column3.Width = 20
        '
        'Column1
        '
        Me.Column1.HeaderText = "Attribute"
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        Me.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Column1.Width = 120
        '
        'DataGridViewImageColumn1
        '
        Me.DataGridViewImageColumn1.HeaderText = ""
        Me.DataGridViewImageColumn1.Name = "DataGridViewImageColumn1"
        Me.DataGridViewImageColumn1.ReadOnly = True
        Me.DataGridViewImageColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DataGridViewImageColumn1.Width = 20
        '
        'Column2
        '
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Column2.DefaultCellStyle = DataGridViewCellStyle3
        Me.Column2.HeaderText = "Value"
        Me.Column2.MaxInputLength = 250
        Me.Column2.Name = "Column2"
        Me.Column2.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Column2.Width = 120
        '
        'Publish
        '
        Me.Publish.HeaderText = ""
        Me.Publish.Name = "Publish"
        Me.Publish.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Publish.Width = 40
        '
        'z
        '
        Me.z.HeaderText = "Published Value"
        Me.z.Name = "z"
        Me.z.ReadOnly = True
        Me.z.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.z.Visible = False
        Me.z.Width = 120
        '
        'puserpic
        '
        Me.puserpic.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.puserpic.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.puserpic.Image = CType(resources.GetObject("puserpic.Image"), System.Drawing.Image)
        Me.puserpic.Location = New System.Drawing.Point(0, 217)
        Me.puserpic.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.puserpic.Name = "puserpic"
        Me.puserpic.Size = New System.Drawing.Size(69, 50)
        Me.puserpic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.puserpic.TabIndex = 61
        Me.puserpic.TabStop = False
        Me.ToolTip1.SetToolTip(Me.puserpic, "Click here to add a photo of the user")
        '
        'Pnouser
        '
        Me.Pnouser.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Pnouser.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Pnouser.Image = CType(resources.GetObject("Pnouser.Image"), System.Drawing.Image)
        Me.Pnouser.Location = New System.Drawing.Point(0, 217)
        Me.Pnouser.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Pnouser.Name = "Pnouser"
        Me.Pnouser.Size = New System.Drawing.Size(69, 50)
        Me.Pnouser.TabIndex = 54
        Me.Pnouser.TabStop = False
        Me.ToolTip1.SetToolTip(Me.Pnouser, "Click here to add a photo of the user")
        '
        'attributedetails
        '
        Me.attributedetails.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.attributedetails.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mdettitle, Me.mdetupdate, Me.mdetuser, Me.mdetpubupdate, Me.mdetpubuser})
        Me.attributedetails.Name = "attributedetails"
        Me.attributedetails.Size = New System.Drawing.Size(361, 129)
        '
        'mdettitle
        '
        Me.mdettitle.Name = "mdettitle"
        Me.mdettitle.ReadOnly = True
        Me.mdettitle.Size = New System.Drawing.Size(300, 23)
        '
        'mdetupdate
        '
        Me.mdetupdate.Name = "mdetupdate"
        Me.mdetupdate.ReadOnly = True
        Me.mdetupdate.Size = New System.Drawing.Size(300, 23)
        '
        'mdetuser
        '
        Me.mdetuser.Name = "mdetuser"
        Me.mdetuser.ReadOnly = True
        Me.mdetuser.Size = New System.Drawing.Size(300, 23)
        '
        'mdetpubupdate
        '
        Me.mdetpubupdate.Name = "mdetpubupdate"
        Me.mdetpubupdate.ReadOnly = True
        Me.mdetpubupdate.Size = New System.Drawing.Size(300, 23)
        '
        'mdetpubuser
        '
        Me.mdetpubuser.Name = "mdetpubuser"
        Me.mdetpubuser.ReadOnly = True
        Me.mdetpubuser.Size = New System.Drawing.Size(300, 23)
        '
        'UserContextMenuStrip
        '
        Me.UserContextMenuStrip.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UserContextMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddEntityToolStripMenuItem, Me.ToolStripSeparator31, Me.minactive, Me.ToolStripSeparator29, Me.DeleteEntityToolStripMenuItem, Me.ToolStripSeparator1, Me.pendingsync, Me.cpendingsync, Me.mpt, Me.mps, Me.mpe})
        Me.UserContextMenuStrip.Name = "entityContextMenuStrip"
        Me.UserContextMenuStrip.Size = New System.Drawing.Size(217, 198)
        '
        'AddEntityToolStripMenuItem
        '
        Me.AddEntityToolStripMenuItem.Image = CType(resources.GetObject("AddEntityToolStripMenuItem.Image"), System.Drawing.Image)
        Me.AddEntityToolStripMenuItem.Name = "AddEntityToolStripMenuItem"
        Me.AddEntityToolStripMenuItem.Size = New System.Drawing.Size(216, 22)
        Me.AddEntityToolStripMenuItem.Text = "Add User"
        '
        'ToolStripSeparator31
        '
        Me.ToolStripSeparator31.Name = "ToolStripSeparator31"
        Me.ToolStripSeparator31.Size = New System.Drawing.Size(213, 6)
        '
        'minactive
        '
        Me.minactive.Enabled = False
        Me.minactive.Image = CType(resources.GetObject("minactive.Image"), System.Drawing.Image)
        Me.minactive.Name = "minactive"
        Me.minactive.Size = New System.Drawing.Size(216, 22)
        Me.minactive.Text = "Inactive"
        '
        'ToolStripSeparator29
        '
        Me.ToolStripSeparator29.Name = "ToolStripSeparator29"
        Me.ToolStripSeparator29.Size = New System.Drawing.Size(213, 6)
        '
        'DeleteEntityToolStripMenuItem
        '
        Me.DeleteEntityToolStripMenuItem.Enabled = False
        Me.DeleteEntityToolStripMenuItem.Image = CType(resources.GetObject("DeleteEntityToolStripMenuItem.Image"), System.Drawing.Image)
        Me.DeleteEntityToolStripMenuItem.Name = "DeleteEntityToolStripMenuItem"
        Me.DeleteEntityToolStripMenuItem.Size = New System.Drawing.Size(216, 22)
        Me.DeleteEntityToolStripMenuItem.Text = "Delete User"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(213, 6)
        '
        'pendingsync
        '
        Me.pendingsync.Name = "pendingsync"
        Me.pendingsync.Size = New System.Drawing.Size(216, 22)
        Me.pendingsync.Text = "Pending sync"
        Me.pendingsync.Visible = False
        '
        'cpendingsync
        '
        Me.cpendingsync.Name = "cpendingsync"
        Me.cpendingsync.Size = New System.Drawing.Size(216, 22)
        Me.cpendingsync.Text = "Clear Pending Sync"
        Me.cpendingsync.Visible = False
        '
        'mpt
        '
        Me.mpt.Name = "mpt"
        Me.mpt.Size = New System.Drawing.Size(216, 22)
        Me.mpt.Text = "Pending Template Sync"
        Me.mpt.Visible = False
        '
        'mps
        '
        Me.mps.Name = "mps"
        Me.mps.Size = New System.Drawing.Size(216, 22)
        Me.mps.Text = "Pending Publication Sync"
        Me.mps.Visible = False
        '
        'mpe
        '
        Me.mpe.Name = "mpe"
        Me.mpe.Size = New System.Drawing.Size(216, 22)
        Me.mpe.Text = "Pending Entity Sync"
        Me.mpe.Visible = False
        '
        'maintainitemscsm
        '
        Me.maintainitemscsm.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.maintainitemscsm.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ChangeNameToolStripMenuItem, Me.mactive, Me.DeleteToolStripMenuItem, Me.addpt, Me.rept, Me.dept})
        Me.maintainitemscsm.Name = "maintainitemscsm"
        Me.maintainitemscsm.Size = New System.Drawing.Size(222, 136)
        '
        'ChangeNameToolStripMenuItem
        '
        Me.ChangeNameToolStripMenuItem.Image = CType(resources.GetObject("ChangeNameToolStripMenuItem.Image"), System.Drawing.Image)
        Me.ChangeNameToolStripMenuItem.Name = "ChangeNameToolStripMenuItem"
        Me.ChangeNameToolStripMenuItem.Size = New System.Drawing.Size(221, 22)
        Me.ChangeNameToolStripMenuItem.Text = "Change Name"
        '
        'mactive
        '
        Me.mactive.Image = CType(resources.GetObject("mactive.Image"), System.Drawing.Image)
        Me.mactive.Name = "mactive"
        Me.mactive.Size = New System.Drawing.Size(221, 22)
        Me.mactive.Text = "Inactive"
        '
        'DeleteToolStripMenuItem
        '
        Me.DeleteToolStripMenuItem.Image = CType(resources.GetObject("DeleteToolStripMenuItem.Image"), System.Drawing.Image)
        Me.DeleteToolStripMenuItem.Name = "DeleteToolStripMenuItem"
        Me.DeleteToolStripMenuItem.Size = New System.Drawing.Size(221, 22)
        Me.DeleteToolStripMenuItem.Text = "Delete"
        '
        'addpt
        '
        Me.addpt.Name = "addpt"
        Me.addpt.Size = New System.Drawing.Size(221, 22)
        Me.addpt.Text = "Add new Preference Type"
        Me.addpt.Visible = False
        '
        'rept
        '
        Me.rept.Name = "rept"
        Me.rept.Size = New System.Drawing.Size(221, 22)
        Me.rept.Text = "Rename Preference Type"
        Me.rept.Visible = False
        '
        'dept
        '
        Me.dept.Name = "dept"
        Me.dept.Size = New System.Drawing.Size(221, 22)
        Me.dept.Text = "Delete Preference Type"
        Me.dept.Visible = False
        '
        'TabPage2
        '
        Me.TabPage2.ImageIndex = 25
        Me.TabPage2.Location = New System.Drawing.Point(4, 23)
        Me.TabPage2.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Size = New System.Drawing.Size(423, 0)
        Me.TabPage2.TabIndex = 0
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'TabPage3
        '
        Me.TabPage3.ImageIndex = 25
        Me.TabPage3.Location = New System.Drawing.Point(4, 23)
        Me.TabPage3.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Size = New System.Drawing.Size(423, 0)
        Me.TabPage3.TabIndex = 0
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'TabPage5
        '
        Me.TabPage5.ImageIndex = 25
        Me.TabPage5.Location = New System.Drawing.Point(4, 23)
        Me.TabPage5.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.TabPage5.Name = "TabPage5"
        Me.TabPage5.Size = New System.Drawing.Size(423, 0)
        Me.TabPage5.TabIndex = 0
        Me.TabPage5.Text = "Attributes"
        Me.TabPage5.UseVisualStyleBackColor = True
        '
        'assignentity
        '
        Me.assignentity.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.assignentity.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddAssociatedClassToolStripMenuItem, Me.massignentity, Me.IncreaseRatingToolStripMenuItem, Me.DecreaseRatingToolStripMenuItem})
        Me.assignentity.Name = "newlinkContextMenuStrip"
        Me.assignentity.Size = New System.Drawing.Size(295, 92)
        '
        'AddAssociatedClassToolStripMenuItem
        '
        Me.AddAssociatedClassToolStripMenuItem.Image = CType(resources.GetObject("AddAssociatedClassToolStripMenuItem.Image"), System.Drawing.Image)
        Me.AddAssociatedClassToolStripMenuItem.Name = "AddAssociatedClassToolStripMenuItem"
        Me.AddAssociatedClassToolStripMenuItem.Size = New System.Drawing.Size(294, 22)
        Me.AddAssociatedClassToolStripMenuItem.Text = "Add New Associated Class and Entities"
        '
        'massignentity
        '
        Me.massignentity.Image = CType(resources.GetObject("massignentity.Image"), System.Drawing.Image)
        Me.massignentity.Name = "massignentity"
        Me.massignentity.Size = New System.Drawing.Size(294, 22)
        Me.massignentity.Text = "Assign Entity"
        Me.massignentity.Visible = False
        '
        'IncreaseRatingToolStripMenuItem
        '
        Me.IncreaseRatingToolStripMenuItem.Image = CType(resources.GetObject("IncreaseRatingToolStripMenuItem.Image"), System.Drawing.Image)
        Me.IncreaseRatingToolStripMenuItem.Name = "IncreaseRatingToolStripMenuItem"
        Me.IncreaseRatingToolStripMenuItem.Size = New System.Drawing.Size(294, 22)
        Me.IncreaseRatingToolStripMenuItem.Text = "Increase Rating"
        Me.IncreaseRatingToolStripMenuItem.Visible = False
        '
        'DecreaseRatingToolStripMenuItem
        '
        Me.DecreaseRatingToolStripMenuItem.Image = CType(resources.GetObject("DecreaseRatingToolStripMenuItem.Image"), System.Drawing.Image)
        Me.DecreaseRatingToolStripMenuItem.Name = "DecreaseRatingToolStripMenuItem"
        Me.DecreaseRatingToolStripMenuItem.Size = New System.Drawing.Size(294, 22)
        Me.DecreaseRatingToolStripMenuItem.Text = "Decrease Rating"
        Me.DecreaseRatingToolStripMenuItem.Visible = False
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.pclass)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.SplitContainer2)
        Me.SplitContainer1.Size = New System.Drawing.Size(863, 459)
        Me.SplitContainer1.SplitterDistance = 236
        Me.SplitContainer1.TabIndex = 4
        '
        'pclass
        '
        Me.pclass.Controls.Add(Me.lattributes)
        Me.pclass.Controls.Add(Me.tusers)
        Me.pclass.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pclass.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pclass.Location = New System.Drawing.Point(0, 0)
        Me.pclass.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.pclass.Name = "pclass"
        Me.pclass.Size = New System.Drawing.Size(236, 459)
        Me.pclass.TabIndex = 55
        '
        'lattributes
        '
        Me.lattributes.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lattributes.AutoSize = True
        Me.lattributes.Location = New System.Drawing.Point(28, 427)
        Me.lattributes.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lattributes.Name = "lattributes"
        Me.lattributes.Size = New System.Drawing.Size(0, 13)
        Me.lattributes.TabIndex = 27
        Me.lattributes.Visible = False
        '
        'tusers
        '
        Me.tusers.AllowDrop = True
        Me.tusers.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tusers.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tusers.FullRowSelect = True
        Me.tusers.HideSelection = False
        Me.tusers.ImageIndex = 5
        Me.tusers.ImageList = Me.tabimages
        Me.tusers.Location = New System.Drawing.Point(0, 0)
        Me.tusers.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.tusers.Name = "tusers"
        Me.tusers.SelectedImageIndex = 23
        Me.tusers.Size = New System.Drawing.Size(236, 459)
        Me.tusers.TabIndex = 25
        '
        'tabimages
        '
        Me.tabimages.ImageStream = CType(resources.GetObject("tabimages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.tabimages.TransparentColor = System.Drawing.Color.Transparent
        Me.tabimages.Images.SetKeyName(0, "childparent")
        Me.tabimages.Images.SetKeyName(1, "parentchild")
        Me.tabimages.Images.SetKeyName(2, "Links")
        Me.tabimages.Images.SetKeyName(3, "Attributes")
        Me.tabimages.Images.SetKeyName(4, "Add")
        Me.tabimages.Images.SetKeyName(5, "Edit")
        Me.tabimages.Images.SetKeyName(6, "deactivate")
        Me.tabimages.Images.SetKeyName(7, "Delete")
        Me.tabimages.Images.SetKeyName(8, "Publication")
        Me.tabimages.Images.SetKeyName(9, "mandatory")
        Me.tabimages.Images.SetKeyName(10, "audited")
        Me.tabimages.Images.SetKeyName(11, "lookup")
        Me.tabimages.Images.SetKeyName(12, "workflow")
        Me.tabimages.Images.SetKeyName(13, "number")
        Me.tabimages.Images.SetKeyName(14, "date")
        Me.tabimages.Images.SetKeyName(15, "string")
        Me.tabimages.Images.SetKeyName(16, "boolean")
        Me.tabimages.Images.SetKeyName(17, "warning.ico")
        Me.tabimages.Images.SetKeyName(18, "data")
        Me.tabimages.Images.SetKeyName(19, "users")
        Me.tabimages.Images.SetKeyName(20, "user")
        Me.tabimages.Images.SetKeyName(21, "parent")
        Me.tabimages.Images.SetKeyName(22, "child")
        Me.tabimages.Images.SetKeyName(23, "selected")
        Me.tabimages.Images.SetKeyName(24, "push publish")
        Me.tabimages.Images.SetKeyName(25, "Folder")
        Me.tabimages.Images.SetKeyName(26, "New")
        Me.tabimages.Images.SetKeyName(27, "active")
        Me.tabimages.Images.SetKeyName(28, "view")
        Me.tabimages.Images.SetKeyName(29, "applications")
        Me.tabimages.Images.SetKeyName(30, "Advanced")
        Me.tabimages.Images.SetKeyName(31, "stageend.bmp")
        Me.tabimages.Images.SetKeyName(32, "link.bmp")
        Me.tabimages.Images.SetKeyName(33, "Translation_icon.png")
        Me.tabimages.Images.SetKeyName(34, "ViewIcon_Disabled.png")
        '
        'SplitContainer2
        '
        Me.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer2.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer2.Name = "SplitContainer2"
        '
        'SplitContainer2.Panel1
        '
        Me.SplitContainer2.Panel1.Controls.Add(Me.pusers)
        '
        'SplitContainer2.Panel2
        '
        Me.SplitContainer2.Panel2.Controls.Add(Me.SplitContainer3)
        Me.SplitContainer2.Size = New System.Drawing.Size(623, 459)
        Me.SplitContainer2.SplitterDistance = 265
        Me.SplitContainer2.TabIndex = 0
        '
        'pusers
        '
        Me.pusers.Controls.Add(Me.tfilter)
        Me.pusers.Controls.Add(Me.Lusers)
        Me.pusers.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pusers.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pusers.Location = New System.Drawing.Point(0, 0)
        Me.pusers.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.pusers.Name = "pusers"
        Me.pusers.Size = New System.Drawing.Size(265, 459)
        Me.pusers.TabIndex = 59
        '
        'tfilter
        '
        Me.tfilter.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tfilter.BackColor = System.Drawing.Color.Transparent
        Me.tfilter.ExcludeControl = Nothing
        Me.tfilter.Location = New System.Drawing.Point(13, 421)
        Me.tfilter.MaximumSize = New System.Drawing.Size(35000, 30000)
        Me.tfilter.MinimumSize = New System.Drawing.Size(175, 21)
        Me.tfilter.Name = "tfilter"
        Me.tfilter.SearchAttributeList = Nothing
        Me.tfilter.SearchAttributes = 0
        Me.tfilter.SearchBuildEntitiesOnly = False
        Me.tfilter.SearchClass = 0
        Me.tfilter.SearchControl = Nothing
        Me.tfilter.SearchCurrentAttribute = Nothing
        Me.tfilter.SearchFireEventOnly = False
        Me.tfilter.SearchGetInactive = False
        Me.tfilter.SearchText = ""
        Me.tfilter.SearchTimerOff = False
        Me.tfilter.SearchUserEntitiesOnly = False
        Me.tfilter.showinactive = -1
        Me.tfilter.Size = New System.Drawing.Size(249, 26)
        Me.tfilter.TabIndex = 39
        '
        'Lusers
        '
        Me.Lusers.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Lusers.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader2})
        Me.Lusers.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Lusers.FullRowSelect = True
        Me.Lusers.GridLines = True
        Me.Lusers.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.Lusers.HideSelection = False
        Me.Lusers.LabelWrap = False
        Me.Lusers.Location = New System.Drawing.Point(3, 3)
        Me.Lusers.MultiSelect = False
        Me.Lusers.Name = "Lusers"
        Me.Lusers.Size = New System.Drawing.Size(259, 412)
        Me.Lusers.SmallImageList = Me.tabimages
        Me.Lusers.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.Lusers.TabIndex = 38
        Me.Lusers.UseCompatibleStateImageBehavior = False
        Me.Lusers.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = ""
        Me.ColumnHeader2.Width = 240
        '
        'SplitContainer3
        '
        Me.SplitContainer3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer3.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer3.Name = "SplitContainer3"
        Me.SplitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer3.Panel1
        '
        Me.SplitContainer3.Panel1.Controls.Add(Me.DataGridView1)
        Me.SplitContainer3.Panel1.Controls.Add(Me.dpartialsync)
        Me.SplitContainer3.Panel1.Controls.Add(Me.Ttitle)
        Me.SplitContainer3.Panel1.Controls.Add(Me.tusername)
        Me.SplitContainer3.Panel1.Controls.Add(Me.uxuserpassword)
        Me.SplitContainer3.Panel1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        '
        'SplitContainer3.Panel2
        '
        Me.SplitContainer3.Panel2.Controls.Add(Me.puser)
        Me.SplitContainer3.Size = New System.Drawing.Size(354, 459)
        Me.SplitContainer3.SplitterDistance = 185
        Me.SplitContainer3.TabIndex = 0
        '
        'dpartialsync
        '
        Me.dpartialsync.AllowUserToAddRows = False
        Me.dpartialsync.AllowUserToDeleteRows = False
        Me.dpartialsync.AllowUserToResizeColumns = False
        Me.dpartialsync.AllowUserToResizeRows = False
        Me.dpartialsync.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dpartialsync.BackgroundColor = System.Drawing.Color.White
        Me.dpartialsync.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.dpartialsync.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dpartialsync.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column4, Me.Column5})
        Me.dpartialsync.Location = New System.Drawing.Point(0, 50)
        Me.dpartialsync.MultiSelect = False
        Me.dpartialsync.Name = "dpartialsync"
        Me.dpartialsync.RowHeadersVisible = False
        Me.dpartialsync.ShowEditingIcon = False
        Me.dpartialsync.ShowRowErrors = False
        Me.dpartialsync.Size = New System.Drawing.Size(354, 133)
        Me.dpartialsync.TabIndex = 70
        '
        'Column4
        '
        Me.Column4.HeaderText = "Sync Type"
        Me.Column4.Name = "Column4"
        Me.Column4.ReadOnly = True
        Me.Column4.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        '
        'Column5
        '
        Me.Column5.HeaderText = "Pending"
        Me.Column5.Name = "Column5"
        Me.Column5.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Column5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        '
        'Ttitle
        '
        Me.Ttitle.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Ttitle.Controls.Add(Me.TabPage6)
        Me.Ttitle.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Ttitle.ImageList = Me.tabimages
        Me.Ttitle.Location = New System.Drawing.Point(0, 4)
        Me.Ttitle.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Ttitle.Name = "Ttitle"
        Me.Ttitle.SelectedIndex = 0
        Me.Ttitle.Size = New System.Drawing.Size(354, 23)
        Me.Ttitle.TabIndex = 64
        '
        'TabPage6
        '
        Me.TabPage6.ImageIndex = 6
        Me.TabPage6.Location = New System.Drawing.Point(4, 23)
        Me.TabPage6.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.TabPage6.Name = "TabPage6"
        Me.TabPage6.Size = New System.Drawing.Size(346, 0)
        Me.TabPage6.TabIndex = 0
        Me.TabPage6.Text = "no selection"
        Me.TabPage6.UseVisualStyleBackColor = True
        '
        'tusername
        '
        Me.tusername.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tusername.Controls.Add(Me.TabPage1)
        Me.tusername.Controls.Add(Me.TabPage7)
        Me.tusername.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tusername.ImageList = Me.tabimages
        Me.tusername.Location = New System.Drawing.Point(0, 27)
        Me.tusername.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.tusername.Name = "tusername"
        Me.tusername.SelectedIndex = 0
        Me.tusername.Size = New System.Drawing.Size(354, 20)
        Me.tusername.TabIndex = 63
        '
        'TabPage1
        '
        Me.TabPage1.ImageIndex = 3
        Me.TabPage1.Location = New System.Drawing.Point(4, 23)
        Me.TabPage1.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Size = New System.Drawing.Size(346, 0)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Attributes"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'TabPage7
        '
        Me.TabPage7.Location = New System.Drawing.Point(4, 23)
        Me.TabPage7.Name = "TabPage7"
        Me.TabPage7.Size = New System.Drawing.Size(346, 0)
        Me.TabPage7.TabIndex = 1
        Me.TabPage7.Text = "Synchronize"
        Me.TabPage7.UseVisualStyleBackColor = True
        '
        'uxuserpassword
        '
        Me.uxuserpassword.Location = New System.Drawing.Point(12, 59)
        Me.uxuserpassword.Name = "uxuserpassword"
        Me.uxuserpassword.Size = New System.Drawing.Size(56, 21)
        Me.uxuserpassword.TabIndex = 74
        '
        'puser
        '
        Me.puser.Controls.Add(Me.buseraudit)
        Me.puser.Controls.Add(Me.baudit)
        Me.puser.Controls.Add(Me.puserpic)
        Me.puser.Controls.Add(Me.lvwvalidate)
        Me.puser.Controls.Add(Me.Tuserass)
        Me.puser.Controls.Add(Me.bentdiscard)
        Me.puser.Controls.Add(Me.bentchanges)
        Me.puser.Controls.Add(Me.Pnouser)
        Me.puser.Controls.Add(Me.tentities)
        Me.puser.Controls.Add(Me.puserdn)
        Me.puser.Controls.Add(Me.puserup)
        Me.puser.Dock = System.Windows.Forms.DockStyle.Fill
        Me.puser.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.puser.Location = New System.Drawing.Point(0, 0)
        Me.puser.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.puser.Name = "puser"
        Me.puser.Size = New System.Drawing.Size(354, 270)
        Me.puser.TabIndex = 60
        '
        'lvwvalidate
        '
        Me.lvwvalidate.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvwvalidate.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1})
        Me.lvwvalidate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwvalidate.Location = New System.Drawing.Point(0, 119)
        Me.lvwvalidate.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.lvwvalidate.Name = "lvwvalidate"
        Me.lvwvalidate.Size = New System.Drawing.Size(354, 92)
        Me.lvwvalidate.SmallImageList = Me.tabimages
        Me.lvwvalidate.TabIndex = 59
        Me.lvwvalidate.UseCompatibleStateImageBehavior = False
        Me.lvwvalidate.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Warning"
        Me.ColumnHeader1.Width = 325
        '
        'Tuserass
        '
        Me.Tuserass.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Tuserass.Controls.Add(Me.TabPage4)
        Me.Tuserass.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Tuserass.ImageList = Me.tabimages
        Me.Tuserass.Location = New System.Drawing.Point(0, 0)
        Me.Tuserass.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Tuserass.Name = "Tuserass"
        Me.Tuserass.SelectedIndex = 0
        Me.Tuserass.Size = New System.Drawing.Size(354, 20)
        Me.Tuserass.TabIndex = 60
        '
        'TabPage4
        '
        Me.TabPage4.ImageIndex = 2
        Me.TabPage4.Location = New System.Drawing.Point(4, 23)
        Me.TabPage4.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.TabPage4.Name = "TabPage4"
        Me.TabPage4.Size = New System.Drawing.Size(346, 0)
        Me.TabPage4.TabIndex = 0
        Me.TabPage4.Text = "Associations"
        Me.TabPage4.UseVisualStyleBackColor = True
        '
        'bentdiscard
        '
        Me.bentdiscard.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bentdiscard.Enabled = False
        Me.bentdiscard.Image = CType(resources.GetObject("bentdiscard.Image"), System.Drawing.Image)
        Me.bentdiscard.Location = New System.Drawing.Point(322, 243)
        Me.bentdiscard.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.bentdiscard.Name = "bentdiscard"
        Me.bentdiscard.Size = New System.Drawing.Size(28, 24)
        Me.bentdiscard.TabIndex = 58
        Me.bentdiscard.UseVisualStyleBackColor = True
        '
        'bentchanges
        '
        Me.bentchanges.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bentchanges.Enabled = False
        Me.bentchanges.Image = CType(resources.GetObject("bentchanges.Image"), System.Drawing.Image)
        Me.bentchanges.Location = New System.Drawing.Point(286, 243)
        Me.bentchanges.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.bentchanges.Name = "bentchanges"
        Me.bentchanges.Size = New System.Drawing.Size(28, 24)
        Me.bentchanges.TabIndex = 57
        Me.bentchanges.UseVisualStyleBackColor = True
        '
        'tentities
        '
        Me.tentities.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tentities.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tentities.ImageKey = "ok.bmp"
        Me.tentities.ImageList = Me.tabimages
        Me.tentities.Location = New System.Drawing.Point(0, 20)
        Me.tentities.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.tentities.Name = "tentities"
        Me.tentities.SelectedImageIndex = 23
        Me.tentities.Size = New System.Drawing.Size(354, 93)
        Me.tentities.TabIndex = 42
        '
        'puserdn
        '
        Me.puserdn.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.puserdn.Image = CType(resources.GetObject("puserdn.Image"), System.Drawing.Image)
        Me.puserdn.Location = New System.Drawing.Point(636, 160)
        Me.puserdn.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.puserdn.Name = "puserdn"
        Me.puserdn.Size = New System.Drawing.Size(31, 24)
        Me.puserdn.TabIndex = 51
        Me.puserdn.TabStop = False
        '
        'puserup
        '
        Me.puserup.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.puserup.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.puserup.Image = CType(resources.GetObject("puserup.Image"), System.Drawing.Image)
        Me.puserup.Location = New System.Drawing.Point(636, 130)
        Me.puserup.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.puserup.Name = "puserup"
        Me.puserup.Size = New System.Drawing.Size(30, 24)
        Me.puserup.TabIndex = 50
        Me.puserup.TabStop = False
        '
        'baudit
        '
        Me.baudit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.baudit.Enabled = False
        Me.baudit.Location = New System.Drawing.Point(76, 243)
        Me.baudit.Name = "baudit"
        Me.baudit.Size = New System.Drawing.Size(105, 23)
        Me.baudit.TabIndex = 75
        Me.baudit.Text = "Attribute Audit"
        Me.baudit.UseVisualStyleBackColor = True
        '
        'buseraudit
        '
        Me.buseraudit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.buseraudit.Enabled = False
        Me.buseraudit.Location = New System.Drawing.Point(187, 243)
        Me.buseraudit.Name = "buseraudit"
        Me.buseraudit.Size = New System.Drawing.Size(78, 23)
        Me.buseraudit.TabIndex = 76
        Me.buseraudit.Text = "User Audit"
        Me.buseraudit.UseVisualStyleBackColor = True
        '
        'bc_am_cp_users
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(863, 459)
        Me.ControlBox = False
        Me.Controls.Add(Me.SplitContainer1)
        Me.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Name = "bc_am_cp_users"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.puserpic, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Pnouser, System.ComponentModel.ISupportInitialize).EndInit()
        Me.attributedetails.ResumeLayout(False)
        Me.attributedetails.PerformLayout()
        Me.UserContextMenuStrip.ResumeLayout(False)
        Me.maintainitemscsm.ResumeLayout(False)
        Me.assignentity.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.pclass.ResumeLayout(False)
        Me.pclass.PerformLayout()
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer2.ResumeLayout(False)
        Me.pusers.ResumeLayout(False)
        Me.SplitContainer3.Panel1.ResumeLayout(False)
        Me.SplitContainer3.Panel1.PerformLayout()
        Me.SplitContainer3.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer3.ResumeLayout(False)
        CType(Me.dpartialsync, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Ttitle.ResumeLayout(False)
        Me.tusername.ResumeLayout(False)
        Me.puser.ResumeLayout(False)
        Me.Tuserass.ResumeLayout(False)
        CType(Me.puserdn, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.puserup, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents attributedetails As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mdettitle As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents mdetupdate As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents mdetuser As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents mdetpubupdate As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents mdetpubuser As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents UserContextMenuStrip As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents AddEntityToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator31 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents minactive As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator29 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents DeleteEntityToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents maintainitemscsm As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ChangeNameToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DeleteToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mactive As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage5 As System.Windows.Forms.TabPage
    Friend WithEvents assignentity As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents AddAssociatedClassToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents massignentity As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents IncreaseRatingToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DecreaseRatingToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents pclass As System.Windows.Forms.Panel
    Friend WithEvents lattributes As System.Windows.Forms.Label
    Friend WithEvents tusers As System.Windows.Forms.TreeView
    Friend WithEvents SplitContainer2 As System.Windows.Forms.SplitContainer
    Friend WithEvents pusers As System.Windows.Forms.Panel
    Friend WithEvents Lusers As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents SplitContainer3 As System.Windows.Forms.SplitContainer
    Friend WithEvents Ttitle As System.Windows.Forms.TabControl
    Friend WithEvents TabPage6 As System.Windows.Forms.TabPage
    Friend WithEvents tusername As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents puser As System.Windows.Forms.Panel
    Friend WithEvents lvwvalidate As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Tuserass As System.Windows.Forms.TabControl
    Friend WithEvents TabPage4 As System.Windows.Forms.TabPage
    Friend WithEvents bentdiscard As System.Windows.Forms.Button
    Friend WithEvents bentchanges As System.Windows.Forms.Button
    Friend WithEvents Pnouser As System.Windows.Forms.PictureBox
    Friend WithEvents tentities As System.Windows.Forms.TreeView
    Friend WithEvents puserdn As System.Windows.Forms.PictureBox
    Friend WithEvents puserup As System.Windows.Forms.PictureBox
    Friend WithEvents tabimages As System.Windows.Forms.ImageList
    Friend WithEvents puserpic As System.Windows.Forms.PictureBox
    Friend WithEvents tfilter As BlueCurve.Core.AS.BlueCurve_TextSearch
    Friend WithEvents pendingsync As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents addpt As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents rept As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents dept As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TabPage7 As System.Windows.Forms.TabPage
    Friend WithEvents mpt As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mps As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mpe As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cpendingsync As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents dpartialsync As System.Windows.Forms.DataGridView
    Friend WithEvents Column4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column5 As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents Column3 As System.Windows.Forms.DataGridViewImageColumn
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewImageColumn1 As System.Windows.Forms.DataGridViewImageColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Publish As System.Windows.Forms.DataGridViewImageColumn
    Friend WithEvents z As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents uxuserpassword As System.Windows.Forms.TextBox
    Friend WithEvents baudit As System.Windows.Forms.Button
    Friend WithEvents buseraudit As System.Windows.Forms.Button
End Class
