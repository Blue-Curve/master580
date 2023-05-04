Imports System.Windows.Forms
Public Class bc_am_bp_about
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

        uxVersion.Text = Application.ProductVersion
        uxProduct.Text = Application.ProductName

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
    Friend WithEvents uxFooterLogo As System.Windows.Forms.PictureBox
    Friend WithEvents uxHeaderLogo As System.Windows.Forms.PictureBox
    Friend WithEvents uxBanner As System.Windows.Forms.PictureBox
    Friend WithEvents uxOK As System.Windows.Forms.Button
    Friend WithEvents uxSystemInfo As System.Windows.Forms.Button
    Friend WithEvents uxLicense As System.Windows.Forms.Label
    Friend WithEvents uxVer As System.Windows.Forms.Label
    Friend WithEvents uxVersion As System.Windows.Forms.Label
    Friend WithEvents uxProd As System.Windows.Forms.Label
    Friend WithEvents uxProduct As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_bp_about))
        Me.uxFooterLogo = New System.Windows.Forms.PictureBox()
        Me.uxHeaderLogo = New System.Windows.Forms.PictureBox()
        Me.uxBanner = New System.Windows.Forms.PictureBox()
        Me.uxOK = New System.Windows.Forms.Button()
        Me.uxSystemInfo = New System.Windows.Forms.Button()
        Me.uxLicense = New System.Windows.Forms.Label()
        Me.uxVer = New System.Windows.Forms.Label()
        Me.uxVersion = New System.Windows.Forms.Label()
        Me.uxProd = New System.Windows.Forms.Label()
        Me.uxProduct = New System.Windows.Forms.Label()
        CType(Me.uxFooterLogo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxHeaderLogo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxBanner, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'uxFooterLogo
        '
        Me.uxFooterLogo.BackColor = System.Drawing.Color.White
        Me.uxFooterLogo.Image = CType(resources.GetObject("uxFooterLogo.Image"), System.Drawing.Image)
        Me.uxFooterLogo.Location = New System.Drawing.Point(5, 148)
        Me.uxFooterLogo.Name = "uxFooterLogo"
        Me.uxFooterLogo.Size = New System.Drawing.Size(287, 32)
        Me.uxFooterLogo.TabIndex = 28
        Me.uxFooterLogo.TabStop = False
        '
        'uxHeaderLogo
        '
        Me.uxHeaderLogo.Image = CType(resources.GetObject("uxHeaderLogo.Image"), System.Drawing.Image)
        Me.uxHeaderLogo.Location = New System.Drawing.Point(310, 0)
        Me.uxHeaderLogo.Name = "uxHeaderLogo"
        Me.uxHeaderLogo.Size = New System.Drawing.Size(64, 64)
        Me.uxHeaderLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.uxHeaderLogo.TabIndex = 27
        Me.uxHeaderLogo.TabStop = False
        '
        'uxBanner
        '
        Me.uxBanner.Image = CType(resources.GetObject("uxBanner.Image"), System.Drawing.Image)
        Me.uxBanner.Location = New System.Drawing.Point(0, 0)
        Me.uxBanner.Name = "uxBanner"
        Me.uxBanner.Size = New System.Drawing.Size(313, 64)
        Me.uxBanner.TabIndex = 26
        Me.uxBanner.TabStop = False
        '
        'uxOK
        '
        Me.uxOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.uxOK.Location = New System.Drawing.Point(297, 177)
        Me.uxOK.Name = "uxOK"
        Me.uxOK.Size = New System.Drawing.Size(75, 23)
        Me.uxOK.TabIndex = 30
        Me.uxOK.Text = "OK"
        '
        'uxSystemInfo
        '
        Me.uxSystemInfo.Location = New System.Drawing.Point(297, 148)
        Me.uxSystemInfo.Name = "uxSystemInfo"
        Me.uxSystemInfo.Size = New System.Drawing.Size(75, 23)
        Me.uxSystemInfo.TabIndex = 31
        Me.uxSystemInfo.Text = "System Info"
        '
        'uxLicense
        '
        Me.uxLicense.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxLicense.Location = New System.Drawing.Point(8, 72)
        Me.uxLicense.Name = "uxLicense"
        Me.uxLicense.Size = New System.Drawing.Size(264, 21)
        Me.uxLicense.TabIndex = 32
        Me.uxLicense.Text = "This product is licensed to Blue Curve."
        '
        'uxVer
        '
        Me.uxVer.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxVer.Location = New System.Drawing.Point(8, 121)
        Me.uxVer.Name = "uxVer"
        Me.uxVer.Size = New System.Drawing.Size(64, 20)
        Me.uxVer.TabIndex = 33
        Me.uxVer.Text = "Version:"
        '
        'uxVersion
        '
        Me.uxVersion.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxVersion.Location = New System.Drawing.Point(78, 121)
        Me.uxVersion.Name = "uxVersion"
        Me.uxVersion.Size = New System.Drawing.Size(192, 20)
        Me.uxVersion.TabIndex = 34
        '
        'uxProd
        '
        Me.uxProd.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxProd.Location = New System.Drawing.Point(8, 98)
        Me.uxProd.Name = "uxProd"
        Me.uxProd.Size = New System.Drawing.Size(64, 19)
        Me.uxProd.TabIndex = 35
        Me.uxProd.Text = "Product:"
        '
        'uxProduct
        '
        Me.uxProduct.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxProduct.Location = New System.Drawing.Point(80, 97)
        Me.uxProduct.Name = "uxProduct"
        Me.uxProduct.Size = New System.Drawing.Size(192, 20)
        Me.uxProduct.TabIndex = 36
        '
        'bc_am_bp_about
        '
        Me.AcceptButton = Me.uxOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(374, 204)
        Me.Controls.Add(Me.uxProduct)
        Me.Controls.Add(Me.uxProd)
        Me.Controls.Add(Me.uxVersion)
        Me.Controls.Add(Me.uxVer)
        Me.Controls.Add(Me.uxLicense)
        Me.Controls.Add(Me.uxSystemInfo)
        Me.Controls.Add(Me.uxOK)
        Me.Controls.Add(Me.uxFooterLogo)
        Me.Controls.Add(Me.uxHeaderLogo)
        Me.Controls.Add(Me.uxBanner)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "bc_am_bp_about"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "About BluePrint"
        CType(Me.uxFooterLogo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxHeaderLogo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxBanner, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region


    Private Sub uxSystemInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxSystemInfo.Click

        MessageBox.Show(Environment.OSVersion.ToString(), "System Info", MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub

    Private Sub uxOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxOK.Click

    End Sub

    Private Sub bc_am_bp_about_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class
