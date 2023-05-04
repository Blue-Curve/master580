Imports BlueCurve.Core.CS
Imports System.Windows.Forms

Friend Class bc_am_bp_component
    Inherits System.Windows.Forms.Form

    Private ctrllr As bc_am_component

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
    Friend WithEvents uxStoredProcName As System.Windows.Forms.TextBox
    Friend WithEvents uxType As System.Windows.Forms.ComboBox
    Friend WithEvents uxDescription As System.Windows.Forms.TextBox
    Friend WithEvents uxSPName As System.Windows.Forms.Label
    Friend WithEvents uxTyp As System.Windows.Forms.Label
    Friend WithEvents uxDesc As System.Windows.Forms.Label
    Friend WithEvents uxOK As System.Windows.Forms.Button
    Friend WithEvents uxCancel As System.Windows.Forms.Button
    Friend WithEvents uxComponent As System.Windows.Forms.GroupBox
    Friend WithEvents uxTypeNameHelp As System.Windows.Forms.PictureBox
    Friend WithEvents uxToolTip As System.Windows.Forms.ToolTip
    Friend WithEvents uxSPHelp As System.Windows.Forms.PictureBox
    Friend WithEvents uxImageList As System.Windows.Forms.ImageList
    Friend WithEvents uxTypeName As System.Windows.Forms.TextBox
    Friend WithEvents uxTypName As System.Windows.Forms.Label
    Friend WithEvents uxExternalDetails As System.Windows.Forms.GroupBox
    Friend WithEvents uxExternalComponentName As System.Windows.Forms.ComboBox
    Friend WithEvents uxExtCompName As System.Windows.Forms.Label
    Friend WithEvents uxFmtFileHelp As System.Windows.Forms.PictureBox
    Friend WithEvents uxFormatFile As System.Windows.Forms.TextBox
    Friend WithEvents uxFmtFile As System.Windows.Forms.Label
    Friend WithEvents uxWebServiceName As System.Windows.Forms.TextBox
    Friend WithEvents uxWebServName As System.Windows.Forms.Label
    Friend WithEvents uxRefreshType As System.Windows.Forms.ComboBox
    Friend WithEvents uxRefType As System.Windows.Forms.Label
    Friend WithEvents uxCaching As System.Windows.Forms.GroupBox
    Friend WithEvents uxCache As System.Windows.Forms.Label
    Friend WithEvents uxNoCache As System.Windows.Forms.RadioButton
    Friend WithEvents uxCacheWebServiceNameOnly As System.Windows.Forms.RadioButton
    Friend WithEvents uxCacheWebServiceNameAndParams As System.Windows.Forms.RadioButton
    Friend WithEvents uxCacheHelp As System.Windows.Forms.PictureBox
    Friend WithEvents uxWebServiceNameHelp As System.Windows.Forms.PictureBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(bc_am_bp_component))
        Me.uxComponent = New System.Windows.Forms.GroupBox
        Me.uxRefreshType = New System.Windows.Forms.ComboBox
        Me.uxRefType = New System.Windows.Forms.Label
        Me.uxExternalDetails = New System.Windows.Forms.GroupBox
        Me.uxCache = New System.Windows.Forms.Label
        Me.uxCaching = New System.Windows.Forms.GroupBox
        Me.uxCacheHelp = New System.Windows.Forms.PictureBox
        Me.uxCacheWebServiceNameAndParams = New System.Windows.Forms.RadioButton
        Me.uxCacheWebServiceNameOnly = New System.Windows.Forms.RadioButton
        Me.uxNoCache = New System.Windows.Forms.RadioButton
        Me.uxWebServiceNameHelp = New System.Windows.Forms.PictureBox
        Me.uxWebServiceName = New System.Windows.Forms.TextBox
        Me.uxWebServName = New System.Windows.Forms.Label
        Me.uxExternalComponentName = New System.Windows.Forms.ComboBox
        Me.uxExtCompName = New System.Windows.Forms.Label
        Me.uxFmtFileHelp = New System.Windows.Forms.PictureBox
        Me.uxFormatFile = New System.Windows.Forms.TextBox
        Me.uxFmtFile = New System.Windows.Forms.Label
        Me.uxSPHelp = New System.Windows.Forms.PictureBox
        Me.uxTypeNameHelp = New System.Windows.Forms.PictureBox
        Me.uxStoredProcName = New System.Windows.Forms.TextBox
        Me.uxTypeName = New System.Windows.Forms.TextBox
        Me.uxType = New System.Windows.Forms.ComboBox
        Me.uxDescription = New System.Windows.Forms.TextBox
        Me.uxSPName = New System.Windows.Forms.Label
        Me.uxTypName = New System.Windows.Forms.Label
        Me.uxTyp = New System.Windows.Forms.Label
        Me.uxDesc = New System.Windows.Forms.Label
        Me.uxOK = New System.Windows.Forms.Button
        Me.uxCancel = New System.Windows.Forms.Button
        Me.uxToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.uxImageList = New System.Windows.Forms.ImageList(Me.components)
        Me.uxComponent.SuspendLayout()
        Me.uxExternalDetails.SuspendLayout()
        Me.uxCaching.SuspendLayout()
        Me.SuspendLayout()
        '
        'uxComponent
        '
        Me.uxComponent.Controls.Add(Me.uxRefreshType)
        Me.uxComponent.Controls.Add(Me.uxRefType)
        Me.uxComponent.Controls.Add(Me.uxExternalDetails)
        Me.uxComponent.Controls.Add(Me.uxSPHelp)
        Me.uxComponent.Controls.Add(Me.uxTypeNameHelp)
        Me.uxComponent.Controls.Add(Me.uxStoredProcName)
        Me.uxComponent.Controls.Add(Me.uxTypeName)
        Me.uxComponent.Controls.Add(Me.uxType)
        Me.uxComponent.Controls.Add(Me.uxDescription)
        Me.uxComponent.Controls.Add(Me.uxSPName)
        Me.uxComponent.Controls.Add(Me.uxTypName)
        Me.uxComponent.Controls.Add(Me.uxTyp)
        Me.uxComponent.Controls.Add(Me.uxDesc)
        Me.uxComponent.Controls.Add(Me.uxOK)
        Me.uxComponent.Controls.Add(Me.uxCancel)
        Me.uxComponent.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxComponent.Location = New System.Drawing.Point(4, 1)
        Me.uxComponent.Name = "uxComponent"
        Me.uxComponent.Size = New System.Drawing.Size(452, 479)
        Me.uxComponent.TabIndex = 0
        Me.uxComponent.TabStop = False
        Me.uxComponent.Text = "Details"
        '
        'uxRefreshType
        '
        Me.uxRefreshType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.uxRefreshType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxRefreshType.Location = New System.Drawing.Point(156, 176)
        Me.uxRefreshType.Name = "uxRefreshType"
        Me.uxRefreshType.Size = New System.Drawing.Size(288, 21)
        Me.uxRefreshType.TabIndex = 5
        '
        'uxRefType
        '
        Me.uxRefType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxRefType.Location = New System.Drawing.Point(61, 176)
        Me.uxRefType.Name = "uxRefType"
        Me.uxRefType.Size = New System.Drawing.Size(88, 23)
        Me.uxRefType.TabIndex = 11
        Me.uxRefType.Text = "Refresh Type:"
        Me.uxRefType.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'uxExternalDetails
        '
        Me.uxExternalDetails.Controls.Add(Me.uxCache)
        Me.uxExternalDetails.Controls.Add(Me.uxCaching)
        Me.uxExternalDetails.Controls.Add(Me.uxWebServiceNameHelp)
        Me.uxExternalDetails.Controls.Add(Me.uxWebServiceName)
        Me.uxExternalDetails.Controls.Add(Me.uxWebServName)
        Me.uxExternalDetails.Controls.Add(Me.uxExternalComponentName)
        Me.uxExternalDetails.Controls.Add(Me.uxExtCompName)
        Me.uxExternalDetails.Controls.Add(Me.uxFmtFileHelp)
        Me.uxExternalDetails.Controls.Add(Me.uxFormatFile)
        Me.uxExternalDetails.Controls.Add(Me.uxFmtFile)
        Me.uxExternalDetails.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxExternalDetails.Location = New System.Drawing.Point(4, 204)
        Me.uxExternalDetails.Name = "uxExternalDetails"
        Me.uxExternalDetails.Size = New System.Drawing.Size(444, 240)
        Me.uxExternalDetails.TabIndex = 6
        Me.uxExternalDetails.TabStop = False
        Me.uxExternalDetails.Text = "External Components"
        '
        'uxCache
        '
        Me.uxCache.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxCache.Location = New System.Drawing.Point(54, 132)
        Me.uxCache.Name = "uxCache"
        Me.uxCache.Size = New System.Drawing.Size(92, 20)
        Me.uxCache.TabIndex = 7
        Me.uxCache.Text = "Caching Level:"
        Me.uxCache.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'uxCaching
        '
        Me.uxCaching.Controls.Add(Me.uxCacheHelp)
        Me.uxCaching.Controls.Add(Me.uxCacheWebServiceNameAndParams)
        Me.uxCaching.Controls.Add(Me.uxCacheWebServiceNameOnly)
        Me.uxCaching.Controls.Add(Me.uxNoCache)
        Me.uxCaching.Location = New System.Drawing.Point(152, 124)
        Me.uxCaching.Name = "uxCaching"
        Me.uxCaching.Size = New System.Drawing.Size(288, 112)
        Me.uxCaching.TabIndex = 3
        Me.uxCaching.TabStop = False
        '
        'uxCacheHelp
        '
        Me.uxCacheHelp.Image = CType(resources.GetObject("uxCacheHelp.Image"), System.Drawing.Image)
        Me.uxCacheHelp.Location = New System.Drawing.Point(268, 16)
        Me.uxCacheHelp.Name = "uxCacheHelp"
        Me.uxCacheHelp.Size = New System.Drawing.Size(16, 16)
        Me.uxCacheHelp.TabIndex = 37
        Me.uxCacheHelp.TabStop = False
        Me.uxToolTip.SetToolTip(Me.uxCacheHelp, "Setting caching to on can improve performance but must only be used for web servi" & _
        "ces that support it.")
        '
        'uxCacheWebServiceNameAndParams
        '
        Me.uxCacheWebServiceNameAndParams.Location = New System.Drawing.Point(8, 76)
        Me.uxCacheWebServiceNameAndParams.Name = "uxCacheWebServiceNameAndParams"
        Me.uxCacheWebServiceNameAndParams.Size = New System.Drawing.Size(268, 28)
        Me.uxCacheWebServiceNameAndParams.TabIndex = 2
        Me.uxCacheWebServiceNameAndParams.Text = "Caching based on Web Service Name and Params"
        '
        'uxCacheWebServiceNameOnly
        '
        Me.uxCacheWebServiceNameOnly.Location = New System.Drawing.Point(8, 44)
        Me.uxCacheWebServiceNameOnly.Name = "uxCacheWebServiceNameOnly"
        Me.uxCacheWebServiceNameOnly.Size = New System.Drawing.Size(268, 24)
        Me.uxCacheWebServiceNameOnly.TabIndex = 1
        Me.uxCacheWebServiceNameOnly.Text = "Caching based on Web Service Name Only"
        '
        'uxNoCache
        '
        Me.uxNoCache.Checked = True
        Me.uxNoCache.Location = New System.Drawing.Point(8, 16)
        Me.uxNoCache.Name = "uxNoCache"
        Me.uxNoCache.Size = New System.Drawing.Size(104, 20)
        Me.uxNoCache.TabIndex = 0
        Me.uxNoCache.TabStop = True
        Me.uxNoCache.Text = "No Caching"
        '
        'uxWebServiceNameHelp
        '
        Me.uxWebServiceNameHelp.Image = CType(resources.GetObject("uxWebServiceNameHelp.Image"), System.Drawing.Image)
        Me.uxWebServiceNameHelp.Location = New System.Drawing.Point(420, 24)
        Me.uxWebServiceNameHelp.Name = "uxWebServiceNameHelp"
        Me.uxWebServiceNameHelp.Size = New System.Drawing.Size(16, 16)
        Me.uxWebServiceNameHelp.TabIndex = 39
        Me.uxWebServiceNameHelp.TabStop = False
        Me.uxToolTip.SetToolTip(Me.uxWebServiceNameHelp, "The external web service name.")
        '
        'uxWebServiceName
        '
        Me.uxWebServiceName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxWebServiceName.Location = New System.Drawing.Point(152, 20)
        Me.uxWebServiceName.Name = "uxWebServiceName"
        Me.uxWebServiceName.Size = New System.Drawing.Size(264, 21)
        Me.uxWebServiceName.TabIndex = 0
        Me.uxWebServiceName.Text = ""
        '
        'uxWebServName
        '
        Me.uxWebServName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxWebServName.Location = New System.Drawing.Point(25, 21)
        Me.uxWebServName.Name = "uxWebServName"
        Me.uxWebServName.Size = New System.Drawing.Size(120, 20)
        Me.uxWebServName.TabIndex = 4
        Me.uxWebServName.Text = "Web Service Name:"
        Me.uxWebServName.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'uxExternalComponentName
        '
        Me.uxExternalComponentName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.uxExternalComponentName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxExternalComponentName.Location = New System.Drawing.Point(152, 56)
        Me.uxExternalComponentName.Name = "uxExternalComponentName"
        Me.uxExternalComponentName.Size = New System.Drawing.Size(286, 21)
        Me.uxExternalComponentName.TabIndex = 1
        '
        'uxExtCompName
        '
        Me.uxExtCompName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxExtCompName.Location = New System.Drawing.Point(6, 57)
        Me.uxExtCompName.Name = "uxExtCompName"
        Me.uxExtCompName.Size = New System.Drawing.Size(140, 20)
        Me.uxExtCompName.TabIndex = 5
        Me.uxExtCompName.Text = "Ext. Component Name:"
        Me.uxExtCompName.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'uxFmtFileHelp
        '
        Me.uxFmtFileHelp.Image = CType(resources.GetObject("uxFmtFileHelp.Image"), System.Drawing.Image)
        Me.uxFmtFileHelp.Location = New System.Drawing.Point(420, 96)
        Me.uxFmtFileHelp.Name = "uxFmtFileHelp"
        Me.uxFmtFileHelp.Size = New System.Drawing.Size(16, 16)
        Me.uxFmtFileHelp.TabIndex = 36
        Me.uxFmtFileHelp.TabStop = False
        Me.uxToolTip.SetToolTip(Me.uxFmtFileHelp, "The xsl file that will format the response from the external component.")
        '
        'uxFormatFile
        '
        Me.uxFormatFile.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxFormatFile.Location = New System.Drawing.Point(152, 92)
        Me.uxFormatFile.Name = "uxFormatFile"
        Me.uxFormatFile.Size = New System.Drawing.Size(264, 21)
        Me.uxFormatFile.TabIndex = 2
        Me.uxFormatFile.Text = ""
        '
        'uxFmtFile
        '
        Me.uxFmtFile.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxFmtFile.Location = New System.Drawing.Point(70, 93)
        Me.uxFmtFile.Name = "uxFmtFile"
        Me.uxFmtFile.Size = New System.Drawing.Size(76, 20)
        Me.uxFmtFile.TabIndex = 6
        Me.uxFmtFile.Text = "Format File:"
        Me.uxFmtFile.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'uxSPHelp
        '
        Me.uxSPHelp.Image = CType(resources.GetObject("uxSPHelp.Image"), System.Drawing.Image)
        Me.uxSPHelp.Location = New System.Drawing.Point(424, 145)
        Me.uxSPHelp.Name = "uxSPHelp"
        Me.uxSPHelp.Size = New System.Drawing.Size(16, 16)
        Me.uxSPHelp.TabIndex = 22
        Me.uxSPHelp.TabStop = False
        Me.uxToolTip.SetToolTip(Me.uxSPHelp, "The stored procedure or web service name that returns the data for the component." & _
        "  If an image is selected then this field requires the attribute code.")
        '
        'uxTypeNameHelp
        '
        Me.uxTypeNameHelp.Image = CType(resources.GetObject("uxTypeNameHelp.Image"), System.Drawing.Image)
        Me.uxTypeNameHelp.Location = New System.Drawing.Point(424, 56)
        Me.uxTypeNameHelp.Name = "uxTypeNameHelp"
        Me.uxTypeNameHelp.Size = New System.Drawing.Size(16, 16)
        Me.uxTypeNameHelp.TabIndex = 20
        Me.uxTypeNameHelp.TabStop = False
        Me.uxToolTip.SetToolTip(Me.uxTypeNameHelp, "Depending on the type this will be either a word table bookmark name, Excel chart" & _
        " file name or image name. ")
        '
        'uxStoredProcName
        '
        Me.uxStoredProcName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxStoredProcName.Location = New System.Drawing.Point(156, 142)
        Me.uxStoredProcName.Name = "uxStoredProcName"
        Me.uxStoredProcName.Size = New System.Drawing.Size(264, 21)
        Me.uxStoredProcName.TabIndex = 3
        Me.uxStoredProcName.Text = ""
        '
        'uxTypeName
        '
        Me.uxTypeName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxTypeName.Location = New System.Drawing.Point(156, 54)
        Me.uxTypeName.Name = "uxTypeName"
        Me.uxTypeName.Size = New System.Drawing.Size(264, 21)
        Me.uxTypeName.TabIndex = 1
        Me.uxTypeName.Text = ""
        '
        'uxType
        '
        Me.uxType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.uxType.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxType.Location = New System.Drawing.Point(156, 23)
        Me.uxType.Name = "uxType"
        Me.uxType.Size = New System.Drawing.Size(288, 21)
        Me.uxType.TabIndex = 0
        '
        'uxDescription
        '
        Me.uxDescription.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxDescription.Location = New System.Drawing.Point(156, 85)
        Me.uxDescription.Multiline = True
        Me.uxDescription.Name = "uxDescription"
        Me.uxDescription.Size = New System.Drawing.Size(288, 44)
        Me.uxDescription.TabIndex = 2
        Me.uxDescription.Text = ""
        '
        'uxSPName
        '
        Me.uxSPName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxSPName.Location = New System.Drawing.Point(45, 142)
        Me.uxSPName.Name = "uxSPName"
        Me.uxSPName.Size = New System.Drawing.Size(104, 20)
        Me.uxSPName.TabIndex = 10
        Me.uxSPName.Text = "Procedure Name:"
        Me.uxSPName.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'uxTypName
        '
        Me.uxTypName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxTypName.Location = New System.Drawing.Point(75, 54)
        Me.uxTypName.Name = "uxTypName"
        Me.uxTypName.Size = New System.Drawing.Size(76, 23)
        Me.uxTypName.TabIndex = 8
        Me.uxTypName.Text = "Type Name:"
        Me.uxTypName.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'uxTyp
        '
        Me.uxTyp.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxTyp.Location = New System.Drawing.Point(110, 23)
        Me.uxTyp.Name = "uxTyp"
        Me.uxTyp.Size = New System.Drawing.Size(40, 23)
        Me.uxTyp.TabIndex = 7
        Me.uxTyp.Text = "Type:"
        Me.uxTyp.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'uxDesc
        '
        Me.uxDesc.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxDesc.Location = New System.Drawing.Point(74, 85)
        Me.uxDesc.Name = "uxDesc"
        Me.uxDesc.Size = New System.Drawing.Size(76, 23)
        Me.uxDesc.TabIndex = 9
        Me.uxDesc.Text = "Description:"
        Me.uxDesc.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'uxOK
        '
        Me.uxOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.uxOK.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxOK.Location = New System.Drawing.Point(288, 448)
        Me.uxOK.Name = "uxOK"
        Me.uxOK.TabIndex = 11
        Me.uxOK.Text = "OK"
        '
        'uxCancel
        '
        Me.uxCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.uxCancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxCancel.Location = New System.Drawing.Point(368, 448)
        Me.uxCancel.Name = "uxCancel"
        Me.uxCancel.TabIndex = 12
        Me.uxCancel.Text = "Cancel"
        '
        'uxImageList
        '
        Me.uxImageList.ImageSize = New System.Drawing.Size(16, 16)
        Me.uxImageList.ImageStream = CType(resources.GetObject("uxImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.uxImageList.TransparentColor = System.Drawing.Color.Transparent
        '
        'bc_am_bp_component
        '
        Me.AcceptButton = Me.uxOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.Color.FromArgb(CType(235, Byte), CType(240, Byte), CType(249, Byte))
        Me.CancelButton = Me.uxCancel
        Me.ClientSize = New System.Drawing.Size(458, 484)
        Me.Controls.Add(Me.uxComponent)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "bc_am_bp_component"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Component"
        Me.uxComponent.ResumeLayout(False)
        Me.uxExternalDetails.ResumeLayout(False)
        Me.uxCaching.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public WriteOnly Property Controller() As bc_am_component

        Set(ByVal Value As bc_am_component)
            ctrllr = Value
        End Set

    End Property

    Private Sub uxOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxOK.Click

        If Not ctrllr.Validate(True) Then
            Me.DialogResult = DialogResult.None
        End If

    End Sub

    Private Sub uxType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxType.SelectedIndexChanged

        Select Case uxType.Text
            Case "Single Cell"
                uxTypName.Text = "Type Name:"
                uxSPName.Text = "Procedure Name:"
                uxStoredProcName.Text = ""
            Case "Table", "Table (Componentized)"
                uxTypName.Text = "Bookmark:"
                uxSPName.Text = "Procedure Name:"
                uxStoredProcName.Text = "bcc_core_bp_data_table" 'default stored proc for a table
            Case "Index Table", "Index Table (Componentized)"
                uxTypName.Text = "Bookmark:"
                uxSPName.Text = "Procedure Name:"
            Case "Chart", "Chart (Componentized)"
                uxTypName.Text = "Chart:"
                uxSPName.Text = "Procedure Name:"
                uxStoredProcName.Text = ""
            Case "File System Image", "Database Image", "File Insertion"
                uxTypName.Text = "Image:"
                uxSPName.Text = "File Type:"
                uxStoredProcName.Text = ""
            Case Else
                uxTypName.Text = "Type Name:"
                uxSPName.Text = "Procedure Name:"
                uxStoredProcName.Text = ""
        End Select

        ctrllr.Validate(False)

    End Sub

    Private Sub uxTypeName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxTypeName.TextChanged

        ctrllr.Validate(False)

    End Sub

    Private Sub uxDescription_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxDescription.TextChanged

        ctrllr.Validate(False)

    End Sub

    Private Sub uxStoredProcName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxStoredProcName.TextChanged

        ctrllr.Validate(False)

    End Sub

    Private Sub uxServiceName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxWebServiceName.TextChanged

        ctrllr.Validate(False)


    End Sub

    Private Sub uxFormatFile_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxFormatFile.TextChanged

        ctrllr.Validate(False)

    End Sub

    Private Sub uxExternalComponentName_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxExternalComponentName.SelectedIndexChanged

        ctrllr.Validate(False)

    End Sub

    Private Sub uxRefreshType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxRefreshType.SelectedIndexChanged

        ctrllr.Validate(False)

    End Sub
End Class
