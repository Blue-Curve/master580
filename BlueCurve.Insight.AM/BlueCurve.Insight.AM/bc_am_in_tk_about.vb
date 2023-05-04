Imports System.Windows.Forms
Public Class bc_am_in_tk_about
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
    Friend WithEvents uxProd As System.Windows.Forms.Label
    Friend WithEvents uxVersion As System.Windows.Forms.Label
    Friend WithEvents uxVer As System.Windows.Forms.Label
    Friend WithEvents uxLicense As System.Windows.Forms.Label
    Friend WithEvents uxSystemInfo As System.Windows.Forms.Button
    Friend WithEvents uxOK As System.Windows.Forms.Button
    Friend WithEvents uxFooterLogo As System.Windows.Forms.PictureBox
    Friend WithEvents uxCompanyDesc As System.Windows.Forms.Label
    Friend WithEvents uxBanner As System.Windows.Forms.PictureBox
    Friend WithEvents uxInsightLogo As System.Windows.Forms.PictureBox
    Friend WithEvents uxProcessLogo As System.Windows.Forms.PictureBox
    Friend WithEvents uxProduct As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_in_tk_about))
        Me.uxProduct = New System.Windows.Forms.Label
        Me.uxProd = New System.Windows.Forms.Label
        Me.uxVersion = New System.Windows.Forms.Label
        Me.uxVer = New System.Windows.Forms.Label
        Me.uxLicense = New System.Windows.Forms.Label
        Me.uxSystemInfo = New System.Windows.Forms.Button
        Me.uxOK = New System.Windows.Forms.Button
        Me.uxFooterLogo = New System.Windows.Forms.PictureBox
        Me.uxCompanyDesc = New System.Windows.Forms.Label
        Me.uxBanner = New System.Windows.Forms.PictureBox
        Me.uxInsightLogo = New System.Windows.Forms.PictureBox
        Me.uxProcessLogo = New System.Windows.Forms.PictureBox
        CType(Me.uxFooterLogo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxBanner, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxInsightLogo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxProcessLogo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'uxProduct
        '
        Me.uxProduct.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxProduct.Location = New System.Drawing.Point(80, 96)
        Me.uxProduct.Name = "uxProduct"
        Me.uxProduct.Size = New System.Drawing.Size(192, 20)
        Me.uxProduct.TabIndex = 47
        '
        'uxProd
        '
        Me.uxProd.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxProd.Location = New System.Drawing.Point(8, 96)
        Me.uxProd.Name = "uxProd"
        Me.uxProd.Size = New System.Drawing.Size(64, 19)
        Me.uxProd.TabIndex = 46
        Me.uxProd.Text = "Product:"
        '
        'uxVersion
        '
        Me.uxVersion.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxVersion.Location = New System.Drawing.Point(80, 120)
        Me.uxVersion.Name = "uxVersion"
        Me.uxVersion.Size = New System.Drawing.Size(192, 20)
        Me.uxVersion.TabIndex = 45
        '
        'uxVer
        '
        Me.uxVer.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxVer.Location = New System.Drawing.Point(8, 120)
        Me.uxVer.Name = "uxVer"
        Me.uxVer.Size = New System.Drawing.Size(64, 20)
        Me.uxVer.TabIndex = 44
        Me.uxVer.Text = "Version:"
        '
        'uxLicense
        '
        Me.uxLicense.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxLicense.Location = New System.Drawing.Point(8, 72)
        Me.uxLicense.Name = "uxLicense"
        Me.uxLicense.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.uxLicense.Size = New System.Drawing.Size(352, 21)
        Me.uxLicense.TabIndex = 43
        Me.uxLicense.Text = "This product is licensed to Blue Curve."
        '
        'uxSystemInfo
        '
        Me.uxSystemInfo.BackColor = System.Drawing.Color.White
        Me.uxSystemInfo.Location = New System.Drawing.Point(280, 136)
        Me.uxSystemInfo.Name = "uxSystemInfo"
        Me.uxSystemInfo.Size = New System.Drawing.Size(84, 23)
        Me.uxSystemInfo.TabIndex = 42
        Me.uxSystemInfo.Text = "System Info"
        Me.uxSystemInfo.UseVisualStyleBackColor = False
        '
        'uxOK
        '
        Me.uxOK.BackColor = System.Drawing.Color.White
        Me.uxOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.uxOK.Location = New System.Drawing.Point(280, 168)
        Me.uxOK.Name = "uxOK"
        Me.uxOK.Size = New System.Drawing.Size(84, 23)
        Me.uxOK.TabIndex = 41
        Me.uxOK.Text = "OK"
        Me.uxOK.UseVisualStyleBackColor = False
        '
        'uxFooterLogo
        '
        Me.uxFooterLogo.BackColor = System.Drawing.Color.White
        Me.uxFooterLogo.Image = CType(resources.GetObject("uxFooterLogo.Image"), System.Drawing.Image)
        Me.uxFooterLogo.Location = New System.Drawing.Point(5, 142)
        Me.uxFooterLogo.Name = "uxFooterLogo"
        Me.uxFooterLogo.Size = New System.Drawing.Size(268, 48)
        Me.uxFooterLogo.TabIndex = 39
        Me.uxFooterLogo.TabStop = False
        '
        'uxCompanyDesc
        '
        Me.uxCompanyDesc.BackColor = System.Drawing.Color.White
        Me.uxCompanyDesc.Location = New System.Drawing.Point(4, 176)
        Me.uxCompanyDesc.Name = "uxCompanyDesc"
        Me.uxCompanyDesc.Size = New System.Drawing.Size(276, 24)
        Me.uxCompanyDesc.TabIndex = 40
        Me.uxCompanyDesc.Text = "© Blue Curve Limited. All rights reserved. "
        '
        'uxBanner
        '
        Me.uxBanner.Image = CType(resources.GetObject("uxBanner.Image"), System.Drawing.Image)
        Me.uxBanner.Location = New System.Drawing.Point(0, 0)
        Me.uxBanner.Name = "uxBanner"
        Me.uxBanner.Size = New System.Drawing.Size(436, 64)
        Me.uxBanner.TabIndex = 37
        Me.uxBanner.TabStop = False
        '
        'uxInsightLogo
        '
        Me.uxInsightLogo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxInsightLogo.Image = CType(resources.GetObject("uxInsightLogo.Image"), System.Drawing.Image)
        Me.uxInsightLogo.Location = New System.Drawing.Point(309, 0)
        Me.uxInsightLogo.Name = "uxInsightLogo"
        Me.uxInsightLogo.Size = New System.Drawing.Size(64, 64)
        Me.uxInsightLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.uxInsightLogo.TabIndex = 48
        Me.uxInsightLogo.TabStop = False
        '
        'uxProcessLogo
        '
        Me.uxProcessLogo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxProcessLogo.Image = CType(resources.GetObject("uxProcessLogo.Image"), System.Drawing.Image)
        Me.uxProcessLogo.Location = New System.Drawing.Point(309, 0)
        Me.uxProcessLogo.Name = "uxProcessLogo"
        Me.uxProcessLogo.Size = New System.Drawing.Size(64, 64)
        Me.uxProcessLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.uxProcessLogo.TabIndex = 49
        Me.uxProcessLogo.TabStop = False
        Me.uxProcessLogo.Visible = False
        '
        'bc_am_in_tk_about
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(372, 198)
        Me.Controls.Add(Me.uxInsightLogo)
        Me.Controls.Add(Me.uxProduct)
        Me.Controls.Add(Me.uxProd)
        Me.Controls.Add(Me.uxVersion)
        Me.Controls.Add(Me.uxVer)
        Me.Controls.Add(Me.uxLicense)
        Me.Controls.Add(Me.uxSystemInfo)
        Me.Controls.Add(Me.uxOK)
        Me.Controls.Add(Me.uxCompanyDesc)
        Me.Controls.Add(Me.uxBanner)
        Me.Controls.Add(Me.uxFooterLogo)
        Me.Controls.Add(Me.uxProcessLogo)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "bc_am_in_tk_about"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "About Insight Toolkit"
        CType(Me.uxFooterLogo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxBanner, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxInsightLogo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxProcessLogo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub uxSystemInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxSystemInfo.Click
        MessageBox.Show(Environment.OSVersion.ToString(), "System Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub blogo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxInsightLogo.Click

    End Sub

    Private Sub uxOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxOK.Click
        Me.Hide()
    End Sub

    Private Sub uxLicense_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxLicense.Click

    End Sub
End Class
