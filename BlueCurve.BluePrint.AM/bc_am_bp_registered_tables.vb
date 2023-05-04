Imports BlueCurve.Core.AS

Public Class bc_am_bp_registered_tables
    Inherits System.Windows.Forms.Form

    Private ctrllr As bc_am_blueprint
    Friend allRegisteredTables As Object
    Friend distinctRegisteredTables As Object
    Public lvwColumnSorter As ListViewColumnSorter

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
    Friend WithEvents uxDetails As System.Windows.Forms.GroupBox
    Friend WithEvents uxAdd As System.Windows.Forms.Button
    Friend WithEvents uxCancel As System.Windows.Forms.Button
    Friend WithEvents uxFooterPanel As System.Windows.Forms.Panel
    Public WithEvents uxServer As System.Windows.Forms.Label
    Public WithEvents uxRole As System.Windows.Forms.Label
    Public WithEvents uxUser As System.Windows.Forms.Label
    Friend WithEvents uxFooterLogo As System.Windows.Forms.PictureBox
    Public WithEvents uxGenericList As System.Windows.Forms.ListView
    Friend WithEvents uxDescription As System.Windows.Forms.ColumnHeader
    Friend WithEvents uxHeaderLogo As System.Windows.Forms.PictureBox
    Friend WithEvents uxBanner As System.Windows.Forms.PictureBox
    Friend WithEvents uxTableAndComponents As System.Windows.Forms.RadioButton
    Friend WithEvents uxTableOnly As System.Windows.Forms.RadioButton
    Friend WithEvents uxGenericListImages As System.Windows.Forms.ImageList
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_bp_registered_tables))
        Me.uxDetails = New System.Windows.Forms.GroupBox()
        Me.uxTableOnly = New System.Windows.Forms.RadioButton()
        Me.uxTableAndComponents = New System.Windows.Forms.RadioButton()
        Me.uxAdd = New System.Windows.Forms.Button()
        Me.uxCancel = New System.Windows.Forms.Button()
        Me.uxFooterPanel = New System.Windows.Forms.Panel()
        Me.uxServer = New System.Windows.Forms.Label()
        Me.uxRole = New System.Windows.Forms.Label()
        Me.uxUser = New System.Windows.Forms.Label()
        Me.uxFooterLogo = New System.Windows.Forms.PictureBox()
        Me.uxGenericList = New System.Windows.Forms.ListView()
        Me.uxDescription = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.uxGenericListImages = New System.Windows.Forms.ImageList()
        Me.uxHeaderLogo = New System.Windows.Forms.PictureBox()
        Me.uxBanner = New System.Windows.Forms.PictureBox()
        Me.uxDetails.SuspendLayout()
        Me.uxFooterPanel.SuspendLayout()
        CType(Me.uxFooterLogo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxHeaderLogo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxBanner, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'uxDetails
        '
        Me.uxDetails.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.uxDetails.Controls.Add(Me.uxTableOnly)
        Me.uxDetails.Controls.Add(Me.uxTableAndComponents)
        Me.uxDetails.Controls.Add(Me.uxAdd)
        Me.uxDetails.Controls.Add(Me.uxCancel)
        Me.uxDetails.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxDetails.Location = New System.Drawing.Point(0, 380)
        Me.uxDetails.Name = "uxDetails"
        Me.uxDetails.Size = New System.Drawing.Size(500, 80)
        Me.uxDetails.TabIndex = 1
        Me.uxDetails.TabStop = False
        Me.uxDetails.Text = "Details"
        '
        'uxTableOnly
        '
        Me.uxTableOnly.Location = New System.Drawing.Point(18, 20)
        Me.uxTableOnly.Name = "uxTableOnly"
        Me.uxTableOnly.Size = New System.Drawing.Size(280, 24)
        Me.uxTableOnly.TabIndex = 0
        Me.uxTableOnly.Text = "Table Only (No components included)"
        '
        'uxTableAndComponents
        '
        Me.uxTableAndComponents.Location = New System.Drawing.Point(18, 48)
        Me.uxTableAndComponents.Name = "uxTableAndComponents"
        Me.uxTableAndComponents.Size = New System.Drawing.Size(299, 24)
        Me.uxTableAndComponents.TabIndex = 1
        Me.uxTableAndComponents.Text = "From another template (Includes components)"
        '
        'uxAdd
        '
        Me.uxAdd.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.uxAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxAdd.Location = New System.Drawing.Point(323, 32)
        Me.uxAdd.Name = "uxAdd"
        Me.uxAdd.Size = New System.Drawing.Size(75, 23)
        Me.uxAdd.TabIndex = 2
        Me.uxAdd.Text = "Add"
        '
        'uxCancel
        '
        Me.uxCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.uxCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxCancel.Location = New System.Drawing.Point(404, 32)
        Me.uxCancel.Name = "uxCancel"
        Me.uxCancel.Size = New System.Drawing.Size(75, 23)
        Me.uxCancel.TabIndex = 3
        Me.uxCancel.Text = "Cancel"
        '
        'uxFooterPanel
        '
        Me.uxFooterPanel.BackColor = System.Drawing.Color.White
        Me.uxFooterPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.uxFooterPanel.Controls.Add(Me.uxServer)
        Me.uxFooterPanel.Controls.Add(Me.uxRole)
        Me.uxFooterPanel.Controls.Add(Me.uxUser)
        Me.uxFooterPanel.Controls.Add(Me.uxFooterLogo)
        Me.uxFooterPanel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.uxFooterPanel.Location = New System.Drawing.Point(0, 462)
        Me.uxFooterPanel.Name = "uxFooterPanel"
        Me.uxFooterPanel.Size = New System.Drawing.Size(500, 64)
        Me.uxFooterPanel.TabIndex = 2
        '
        'uxServer
        '
        Me.uxServer.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxServer.Location = New System.Drawing.Point(252, 19)
        Me.uxServer.Name = "uxServer"
        Me.uxServer.Size = New System.Drawing.Size(244, 35)
        Me.uxServer.TabIndex = 3
        Me.uxServer.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'uxRole
        '
        Me.uxRole.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxRole.Location = New System.Drawing.Point(376, 3)
        Me.uxRole.Name = "uxRole"
        Me.uxRole.Size = New System.Drawing.Size(120, 16)
        Me.uxRole.TabIndex = 2
        Me.uxRole.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'uxUser
        '
        Me.uxUser.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxUser.Location = New System.Drawing.Point(252, 3)
        Me.uxUser.Name = "uxUser"
        Me.uxUser.Size = New System.Drawing.Size(116, 16)
        Me.uxUser.TabIndex = 1
        Me.uxUser.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'uxFooterLogo
        '
        Me.uxFooterLogo.BackColor = System.Drawing.Color.White
        Me.uxFooterLogo.Image = CType(resources.GetObject("uxFooterLogo.Image"), System.Drawing.Image)
        Me.uxFooterLogo.Location = New System.Drawing.Point(5, 3)
        Me.uxFooterLogo.Name = "uxFooterLogo"
        Me.uxFooterLogo.Size = New System.Drawing.Size(236, 35)
        Me.uxFooterLogo.TabIndex = 39
        Me.uxFooterLogo.TabStop = False
        '
        'uxGenericList
        '
        Me.uxGenericList.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.uxDescription})
        Me.uxGenericList.FullRowSelect = True
        Me.uxGenericList.GridLines = True
        Me.uxGenericList.HideSelection = False
        Me.uxGenericList.Location = New System.Drawing.Point(4, 68)
        Me.uxGenericList.MultiSelect = False
        Me.uxGenericList.Name = "uxGenericList"
        Me.uxGenericList.Size = New System.Drawing.Size(493, 312)
        Me.uxGenericList.SmallImageList = Me.uxGenericListImages
        Me.uxGenericList.TabIndex = 0
        Me.uxGenericList.UseCompatibleStateImageBehavior = False
        Me.uxGenericList.View = System.Windows.Forms.View.Details
        '
        'uxDescription
        '
        Me.uxDescription.Text = "Description"
        Me.uxDescription.Width = 463
        '
        'uxGenericListImages
        '
        Me.uxGenericListImages.ImageStream = CType(resources.GetObject("uxGenericListImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.uxGenericListImages.TransparentColor = System.Drawing.Color.Transparent
        Me.uxGenericListImages.Images.SetKeyName(0, "")
        Me.uxGenericListImages.Images.SetKeyName(1, "")
        Me.uxGenericListImages.Images.SetKeyName(2, "")
        Me.uxGenericListImages.Images.SetKeyName(3, "")
        Me.uxGenericListImages.Images.SetKeyName(4, "")
        '
        'uxHeaderLogo
        '
        Me.uxHeaderLogo.Image = CType(resources.GetObject("uxHeaderLogo.Image"), System.Drawing.Image)
        Me.uxHeaderLogo.Location = New System.Drawing.Point(437, 0)
        Me.uxHeaderLogo.Name = "uxHeaderLogo"
        Me.uxHeaderLogo.Size = New System.Drawing.Size(64, 64)
        Me.uxHeaderLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.uxHeaderLogo.TabIndex = 45
        Me.uxHeaderLogo.TabStop = False
        '
        'uxBanner
        '
        Me.uxBanner.Image = CType(resources.GetObject("uxBanner.Image"), System.Drawing.Image)
        Me.uxBanner.Location = New System.Drawing.Point(0, 0)
        Me.uxBanner.Name = "uxBanner"
        Me.uxBanner.Size = New System.Drawing.Size(416, 64)
        Me.uxBanner.TabIndex = 44
        Me.uxBanner.TabStop = False
        '
        'bc_am_bp_registered_tables
        '
        Me.AcceptButton = Me.uxAdd
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.Color.White
        Me.CancelButton = Me.uxCancel
        Me.ClientSize = New System.Drawing.Size(500, 526)
        Me.Controls.Add(Me.uxDetails)
        Me.Controls.Add(Me.uxFooterPanel)
        Me.Controls.Add(Me.uxGenericList)
        Me.Controls.Add(Me.uxHeaderLogo)
        Me.Controls.Add(Me.uxBanner)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "bc_am_bp_registered_tables"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Registered Tables"
        Me.uxDetails.ResumeLayout(False)
        Me.uxFooterPanel.ResumeLayout(False)
        CType(Me.uxFooterLogo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxHeaderLogo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxBanner, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Friend WriteOnly Property Controller() As bc_am_blueprint

        Set(ByVal Value As bc_am_blueprint)
            ctrllr = Value
        End Set

    End Property


    Private Sub uxTableOnly_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxTableOnly.CheckedChanged

        If uxTableOnly.Checked Then
            ctrllr.ToggleRegisteredTablesView(True)
        End If

    End Sub

    Private Sub uxTableAndComponents_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxTableAndComponents.CheckedChanged

        If uxTableAndComponents.Checked Then
            ctrllr.ToggleRegisteredTablesView(False)
        End If

    End Sub

    Private Sub uxGenericList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxGenericList.SelectedIndexChanged

        ctrllr.ValidateRegisteredTables()

    End Sub
End Class
