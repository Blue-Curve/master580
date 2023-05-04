<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bc_am_cp_about
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_cp_about))
        Me.uxProduct = New System.Windows.Forms.Label
        Me.uxProd = New System.Windows.Forms.Label
        Me.uxVersion = New System.Windows.Forms.Label
        Me.uxVer = New System.Windows.Forms.Label
        Me.uxLicense = New System.Windows.Forms.Label
        Me.uxSystemInfo = New System.Windows.Forms.Button
        Me.uxOK = New System.Windows.Forms.Button
        Me.uxFooterLogo = New System.Windows.Forms.PictureBox
        Me.uxHeaderLogo = New System.Windows.Forms.PictureBox
        Me.uxCompanyDesc = New System.Windows.Forms.Label
        Me.uxBanner = New System.Windows.Forms.PictureBox
        CType(Me.uxFooterLogo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxHeaderLogo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxBanner, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'uxProduct
        '
        Me.uxProduct.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxProduct.Location = New System.Drawing.Point(80, 95)
        Me.uxProduct.Name = "uxProduct"
        Me.uxProduct.Size = New System.Drawing.Size(252, 20)
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
        Me.uxVersion.Location = New System.Drawing.Point(78, 119)
        Me.uxVersion.Name = "uxVersion"
        Me.uxVersion.Size = New System.Drawing.Size(192, 20)
        Me.uxVersion.TabIndex = 45
        '
        'uxVer
        '
        Me.uxVer.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxVer.Location = New System.Drawing.Point(8, 119)
        Me.uxVer.Name = "uxVer"
        Me.uxVer.Size = New System.Drawing.Size(64, 20)
        Me.uxVer.TabIndex = 44
        Me.uxVer.Text = "Version:"
        '
        'uxLicense
        '
        Me.uxLicense.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxLicense.Location = New System.Drawing.Point(8, 70)
        Me.uxLicense.Name = "uxLicense"
        Me.uxLicense.Size = New System.Drawing.Size(264, 21)
        Me.uxLicense.TabIndex = 43
        Me.uxLicense.Text = "This product is licensed to Blue Curve."
        Me.uxLicense.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'uxSystemInfo
        '
        Me.uxSystemInfo.Location = New System.Drawing.Point(290, 146)
        Me.uxSystemInfo.Name = "uxSystemInfo"
        Me.uxSystemInfo.Size = New System.Drawing.Size(75, 23)
        Me.uxSystemInfo.TabIndex = 42
        Me.uxSystemInfo.Text = "System Info"
        '
        'uxOK
        '
        Me.uxOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.uxOK.Location = New System.Drawing.Point(290, 176)
        Me.uxOK.Name = "uxOK"
        Me.uxOK.Size = New System.Drawing.Size(75, 23)
        Me.uxOK.TabIndex = 41
        Me.uxOK.Text = "OK"
        '
        'uxFooterLogo
        '
        Me.uxFooterLogo.BackColor = System.Drawing.Color.White
        Me.uxFooterLogo.Image = CType(resources.GetObject("uxFooterLogo.Image"), System.Drawing.Image)
        Me.uxFooterLogo.Location = New System.Drawing.Point(5, 146)
        Me.uxFooterLogo.Name = "uxFooterLogo"
        Me.uxFooterLogo.Size = New System.Drawing.Size(287, 32)
        Me.uxFooterLogo.TabIndex = 39
        Me.uxFooterLogo.TabStop = False
        '
        'uxHeaderLogo
        '
        Me.uxHeaderLogo.Image = CType(resources.GetObject("uxHeaderLogo.Image"), System.Drawing.Image)
        Me.uxHeaderLogo.Location = New System.Drawing.Point(310, -2)
        Me.uxHeaderLogo.Name = "uxHeaderLogo"
        Me.uxHeaderLogo.Size = New System.Drawing.Size(64, 64)
        Me.uxHeaderLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.uxHeaderLogo.TabIndex = 38
        Me.uxHeaderLogo.TabStop = False
        '
        'uxCompanyDesc
        '
        Me.uxCompanyDesc.BackColor = System.Drawing.Color.White
        Me.uxCompanyDesc.Location = New System.Drawing.Point(4, 181)
        Me.uxCompanyDesc.Name = "uxCompanyDesc"
        Me.uxCompanyDesc.Size = New System.Drawing.Size(284, 24)
        Me.uxCompanyDesc.TabIndex = 40
        Me.uxCompanyDesc.Text = "© Blue Curve Limited. All rights reserved. "
        '
        'uxBanner
        '
        Me.uxBanner.Image = CType(resources.GetObject("uxBanner.Image"), System.Drawing.Image)
        Me.uxBanner.Location = New System.Drawing.Point(0, -2)
        Me.uxBanner.Name = "uxBanner"
        Me.uxBanner.Size = New System.Drawing.Size(313, 64)
        Me.uxBanner.TabIndex = 37
        Me.uxBanner.TabStop = False
        '
        'bc_am_cp_about
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(372, 203)
        Me.Controls.Add(Me.uxProduct)
        Me.Controls.Add(Me.uxProd)
        Me.Controls.Add(Me.uxVersion)
        Me.Controls.Add(Me.uxVer)
        Me.Controls.Add(Me.uxLicense)
        Me.Controls.Add(Me.uxSystemInfo)
        Me.Controls.Add(Me.uxOK)
        Me.Controls.Add(Me.uxFooterLogo)
        Me.Controls.Add(Me.uxHeaderLogo)
        Me.Controls.Add(Me.uxCompanyDesc)
        Me.Controls.Add(Me.uxBanner)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "bc_am_cp_about"
        Me.Text = "About Common Platform"
        CType(Me.uxFooterLogo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxHeaderLogo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxBanner, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents uxProduct As System.Windows.Forms.Label
    Friend WithEvents uxProd As System.Windows.Forms.Label
    Friend WithEvents uxVersion As System.Windows.Forms.Label
    Friend WithEvents uxVer As System.Windows.Forms.Label
    Friend WithEvents uxLicense As System.Windows.Forms.Label
    Friend WithEvents uxSystemInfo As System.Windows.Forms.Button
    Friend WithEvents uxOK As System.Windows.Forms.Button
    Friend WithEvents uxFooterLogo As System.Windows.Forms.PictureBox
    Friend WithEvents uxHeaderLogo As System.Windows.Forms.PictureBox
    Friend WithEvents uxCompanyDesc As System.Windows.Forms.Label
    Friend WithEvents uxBanner As System.Windows.Forms.PictureBox
End Class
