Public Class bc_am_bp_data_model_clone
    Inherits System.Windows.Forms.Form

    Private ctrllr As bc_am_clone

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
    Friend WithEvents uxDMClone As System.Windows.Forms.GroupBox
    Friend WithEvents uxOK As System.Windows.Forms.Button
    Friend WithEvents uxCancel As System.Windows.Forms.Button
    Friend WithEvents uxDataModels As System.Windows.Forms.ListView
    Friend WithEvents uxDataModelCol As System.Windows.Forms.ColumnHeader
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_bp_data_model_clone))
        Me.uxDMClone = New System.Windows.Forms.GroupBox
        Me.uxDataModels = New System.Windows.Forms.ListView
        Me.uxDataModelCol = New System.Windows.Forms.ColumnHeader
        Me.uxOK = New System.Windows.Forms.Button
        Me.uxCancel = New System.Windows.Forms.Button
        Me.uxDMClone.SuspendLayout()
        Me.SuspendLayout()
        '
        'uxDMClone
        '
        Me.uxDMClone.Controls.Add(Me.uxDataModels)
        Me.uxDMClone.Controls.Add(Me.uxOK)
        Me.uxDMClone.Controls.Add(Me.uxCancel)
        Me.uxDMClone.Location = New System.Drawing.Point(4, 0)
        Me.uxDMClone.Name = "uxDMClone"
        Me.uxDMClone.Size = New System.Drawing.Size(332, 216)
        Me.uxDMClone.TabIndex = 0
        Me.uxDMClone.TabStop = False
        '
        'uxDataModels
        '
        Me.uxDataModels.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.uxDataModelCol})
        Me.uxDataModels.FullRowSelect = True
        Me.uxDataModels.GridLines = True
        Me.uxDataModels.HideSelection = False
        Me.uxDataModels.Location = New System.Drawing.Point(3, 11)
        Me.uxDataModels.MultiSelect = False
        Me.uxDataModels.Name = "uxDataModels"
        Me.uxDataModels.Size = New System.Drawing.Size(324, 168)
        Me.uxDataModels.TabIndex = 4
        Me.uxDataModels.UseCompatibleStateImageBehavior = False
        Me.uxDataModels.View = System.Windows.Forms.View.Details
        '
        'uxDataModelCol
        '
        Me.uxDataModelCol.Text = "Data Model to Clone To"
        Me.uxDataModelCol.Width = 300
        '
        'uxOK
        '
        Me.uxOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.uxOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.uxOK.Location = New System.Drawing.Point(171, 185)
        Me.uxOK.Name = "uxOK"
        Me.uxOK.Size = New System.Drawing.Size(75, 23)
        Me.uxOK.TabIndex = 3
        Me.uxOK.Text = "OK"
        '
        'uxCancel
        '
        Me.uxCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.uxCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.uxCancel.Location = New System.Drawing.Point(252, 185)
        Me.uxCancel.Name = "uxCancel"
        Me.uxCancel.Size = New System.Drawing.Size(75, 23)
        Me.uxCancel.TabIndex = 2
        Me.uxCancel.Text = "Cancel"
        '
        'bc_am_bp_data_model_clone
        '
        Me.AcceptButton = Me.uxOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.CancelButton = Me.uxCancel
        Me.ClientSize = New System.Drawing.Size(340, 218)
        Me.Controls.Add(Me.uxDMClone)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.SystemColors.ControlText
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "bc_am_bp_data_model_clone"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Data Model Clone"
        Me.uxDMClone.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Friend WriteOnly Property Controller() As bc_am_clone

        Set(ByVal Value As bc_am_clone)
            ctrllr = Value
        End Set

    End Property

End Class
