Public Class bc_am_bp_configuration
    Inherits System.Windows.Forms.Form

    Private ctrllr As bc_am_configuration

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
    Friend WithEvents uxConfig As System.Windows.Forms.GroupBox
    Friend WithEvents uxOK As System.Windows.Forms.Button
    Friend WithEvents uxCancel As System.Windows.Forms.Button
    Friend WithEvents uxConMeth As System.Windows.Forms.Label
    Friend WithEvents uxServ As System.Windows.Forms.Label
    Friend WithEvents uxTempCentralRepos As System.Windows.Forms.Label
    Friend WithEvents uxTempLocalRepos As System.Windows.Forms.Label
    Friend WithEvents uxEnviron As System.Windows.Forms.Label
    Friend WithEvents uxEnvironment As System.Windows.Forms.ComboBox
    Friend WithEvents uxConnectivityMethod As System.Windows.Forms.Label
    Friend WithEvents uxServer As System.Windows.Forms.Label
    Friend WithEvents uxTemplateLocalRepos As System.Windows.Forms.Label
    Friend WithEvents uxTemplateCentralRepos As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_bp_configuration))
        Me.uxConfig = New System.Windows.Forms.GroupBox
        Me.uxTemplateCentralRepos = New System.Windows.Forms.Label
        Me.uxTemplateLocalRepos = New System.Windows.Forms.Label
        Me.uxServer = New System.Windows.Forms.Label
        Me.uxTempCentralRepos = New System.Windows.Forms.Label
        Me.uxTempLocalRepos = New System.Windows.Forms.Label
        Me.uxServ = New System.Windows.Forms.Label
        Me.uxConMeth = New System.Windows.Forms.Label
        Me.uxEnviron = New System.Windows.Forms.Label
        Me.uxEnvironment = New System.Windows.Forms.ComboBox
        Me.uxCancel = New System.Windows.Forms.Button
        Me.uxOK = New System.Windows.Forms.Button
        Me.uxConnectivityMethod = New System.Windows.Forms.Label
        Me.uxConfig.SuspendLayout()
        Me.SuspendLayout()
        '
        'uxConfig
        '
        Me.uxConfig.Controls.Add(Me.uxTemplateCentralRepos)
        Me.uxConfig.Controls.Add(Me.uxTemplateLocalRepos)
        Me.uxConfig.Controls.Add(Me.uxServer)
        Me.uxConfig.Controls.Add(Me.uxTempCentralRepos)
        Me.uxConfig.Controls.Add(Me.uxTempLocalRepos)
        Me.uxConfig.Controls.Add(Me.uxServ)
        Me.uxConfig.Controls.Add(Me.uxConMeth)
        Me.uxConfig.Controls.Add(Me.uxEnviron)
        Me.uxConfig.Controls.Add(Me.uxEnvironment)
        Me.uxConfig.Controls.Add(Me.uxCancel)
        Me.uxConfig.Controls.Add(Me.uxOK)
        Me.uxConfig.Controls.Add(Me.uxConnectivityMethod)
        Me.uxConfig.Location = New System.Drawing.Point(8, 4)
        Me.uxConfig.Name = "uxConfig"
        Me.uxConfig.Size = New System.Drawing.Size(444, 244)
        Me.uxConfig.TabIndex = 0
        Me.uxConfig.TabStop = False
        '
        'uxTemplateCentralRepos
        '
        Me.uxTemplateCentralRepos.Location = New System.Drawing.Point(176, 165)
        Me.uxTemplateCentralRepos.Name = "uxTemplateCentralRepos"
        Me.uxTemplateCentralRepos.Size = New System.Drawing.Size(256, 32)
        Me.uxTemplateCentralRepos.TabIndex = 11
        Me.uxTemplateCentralRepos.Text = "xxx"
        '
        'uxTemplateLocalRepos
        '
        Me.uxTemplateLocalRepos.Location = New System.Drawing.Point(176, 125)
        Me.uxTemplateLocalRepos.Name = "uxTemplateLocalRepos"
        Me.uxTemplateLocalRepos.Size = New System.Drawing.Size(256, 32)
        Me.uxTemplateLocalRepos.TabIndex = 10
        Me.uxTemplateLocalRepos.Text = "xxx"
        '
        'uxServer
        '
        Me.uxServer.Location = New System.Drawing.Point(176, 88)
        Me.uxServer.Name = "uxServer"
        Me.uxServer.Size = New System.Drawing.Size(256, 32)
        Me.uxServer.TabIndex = 9
        Me.uxServer.Text = "xxx"
        '
        'uxTempCentralRepos
        '
        Me.uxTempCentralRepos.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxTempCentralRepos.Location = New System.Drawing.Point(4, 165)
        Me.uxTempCentralRepos.Name = "uxTempCentralRepos"
        Me.uxTempCentralRepos.Size = New System.Drawing.Size(164, 23)
        Me.uxTempCentralRepos.TabIndex = 8
        Me.uxTempCentralRepos.Text = "Template central repos:"
        Me.uxTempCentralRepos.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'uxTempLocalRepos
        '
        Me.uxTempLocalRepos.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxTempLocalRepos.Location = New System.Drawing.Point(4, 125)
        Me.uxTempLocalRepos.Name = "uxTempLocalRepos"
        Me.uxTempLocalRepos.Size = New System.Drawing.Size(164, 23)
        Me.uxTempLocalRepos.TabIndex = 7
        Me.uxTempLocalRepos.Text = "Template Local Repos:"
        Me.uxTempLocalRepos.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'uxServ
        '
        Me.uxServ.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxServ.Location = New System.Drawing.Point(4, 88)
        Me.uxServ.Name = "uxServ"
        Me.uxServ.Size = New System.Drawing.Size(164, 23)
        Me.uxServ.TabIndex = 6
        Me.uxServ.Text = "Server:"
        Me.uxServ.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'uxConMeth
        '
        Me.uxConMeth.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxConMeth.Location = New System.Drawing.Point(4, 56)
        Me.uxConMeth.Name = "uxConMeth"
        Me.uxConMeth.Size = New System.Drawing.Size(164, 23)
        Me.uxConMeth.TabIndex = 5
        Me.uxConMeth.Text = "Connectivity Method:"
        Me.uxConMeth.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'uxEnviron
        '
        Me.uxEnviron.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxEnviron.Location = New System.Drawing.Point(4, 21)
        Me.uxEnviron.Name = "uxEnviron"
        Me.uxEnviron.Size = New System.Drawing.Size(164, 23)
        Me.uxEnviron.TabIndex = 4
        Me.uxEnviron.Text = "Environment:"
        Me.uxEnviron.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'uxEnvironment
        '
        Me.uxEnvironment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.uxEnvironment.Location = New System.Drawing.Point(172, 21)
        Me.uxEnvironment.Name = "uxEnvironment"
        Me.uxEnvironment.Size = New System.Drawing.Size(260, 21)
        Me.uxEnvironment.TabIndex = 3
        '
        'uxCancel
        '
        Me.uxCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.uxCancel.Location = New System.Drawing.Point(357, 208)
        Me.uxCancel.Name = "uxCancel"
        Me.uxCancel.Size = New System.Drawing.Size(75, 23)
        Me.uxCancel.TabIndex = 2
        Me.uxCancel.Text = "Cancel"
        '
        'uxOK
        '
        Me.uxOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.uxOK.Location = New System.Drawing.Point(276, 208)
        Me.uxOK.Name = "uxOK"
        Me.uxOK.Size = New System.Drawing.Size(75, 23)
        Me.uxOK.TabIndex = 1
        Me.uxOK.Text = "OK"
        '
        'uxConnectivityMethod
        '
        Me.uxConnectivityMethod.Location = New System.Drawing.Point(176, 56)
        Me.uxConnectivityMethod.Name = "uxConnectivityMethod"
        Me.uxConnectivityMethod.Size = New System.Drawing.Size(256, 23)
        Me.uxConnectivityMethod.TabIndex = 1
        Me.uxConnectivityMethod.Text = "xxx"
        '
        'bc_am_bp_configuration
        '
        Me.AcceptButton = Me.uxOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.CancelButton = Me.uxCancel
        Me.ClientSize = New System.Drawing.Size(458, 256)
        Me.Controls.Add(Me.uxConfig)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "bc_am_bp_configuration"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Configuration"
        Me.uxConfig.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public WriteOnly Property Controller() As bc_am_configuration

        Set(ByVal Value As bc_am_configuration)
            ctrllr = Value
        End Set

    End Property

    Private Sub uxEnvironment_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxEnvironment.SelectedIndexChanged

        ctrllr.PopulateConfig()

    End Sub
End Class
