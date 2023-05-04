Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports BlueCurve.Core.AS
Imports System.Collections
Imports System.Threading
Imports System.IO
Imports System.Windows.Forms
Public Class bc_am_in_context
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        Try
            InitializeComponent()
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_in_context", "new", bc_cs_error_codes.USER_DEFINED, ex.Message + " " + ex.StackTrace.ToString)
        End Try


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
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents uxToolBarImages As System.Windows.Forms.ImageList
    Friend WithEvents uxTemplates As System.Windows.Forms.GroupBox
    Friend WithEvents lintsheet As System.Windows.Forms.Label
    Friend WithEvents ttemplatenew As System.Windows.Forms.TextBox
    Friend WithEvents ctemplate As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents uxSections As System.Windows.Forms.GroupBox
    Friend WithEvents bsecdn As System.Windows.Forms.Button
    Friend WithEvents bsecup As System.Windows.Forms.Button
    Friend WithEvents Lselsec As System.Windows.Forms.ListBox
    Friend WithEvents bdn As System.Windows.Forms.Button
    Friend WithEvents bup As System.Windows.Forms.Button
    Friend WithEvents litems As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader9 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader7 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader8 As System.Windows.Forms.ColumnHeader
    Friend WithEvents uxItems As System.Windows.Forms.GroupBox
    Friend WithEvents uxSectionToolBarPanel As System.Windows.Forms.Panel
    Friend WithEvents uxSectionToolbar As System.Windows.Forms.ToolBar
    Friend WithEvents uxItemToolbarPanel As System.Windows.Forms.Panel
    Friend WithEvents uxItemToolbar As System.Windows.Forms.ToolBar
    Friend WithEvents ColumnHeader10 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader11 As System.Windows.Forms.ColumnHeader
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_in_context))
        Me.uxToolBarImages = New System.Windows.Forms.ImageList(Me.components)
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.uxItems = New System.Windows.Forms.GroupBox
        Me.uxItemToolbarPanel = New System.Windows.Forms.Panel
        Me.uxItemToolbar = New System.Windows.Forms.ToolBar
        Me.bdn = New System.Windows.Forms.Button
        Me.bup = New System.Windows.Forms.Button
        Me.litems = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader4 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader5 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader9 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader6 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader7 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader8 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader10 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader11 = New System.Windows.Forms.ColumnHeader
        Me.uxSections = New System.Windows.Forms.GroupBox
        Me.uxSectionToolBarPanel = New System.Windows.Forms.Panel
        Me.uxSectionToolbar = New System.Windows.Forms.ToolBar
        Me.bsecdn = New System.Windows.Forms.Button
        Me.bsecup = New System.Windows.Forms.Button
        Me.Lselsec = New System.Windows.Forms.ListBox
        Me.uxTemplates = New System.Windows.Forms.GroupBox
        Me.lintsheet = New System.Windows.Forms.Label
        Me.ttemplatenew = New System.Windows.Forms.TextBox
        Me.ctemplate = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Panel1.SuspendLayout()
        Me.uxItems.SuspendLayout()
        Me.uxItemToolbarPanel.SuspendLayout()
        Me.uxSections.SuspendLayout()
        Me.uxSectionToolBarPanel.SuspendLayout()
        Me.uxTemplates.SuspendLayout()
        Me.SuspendLayout()
        '
        'uxToolBarImages
        '
        Me.uxToolBarImages.ImageStream = CType(resources.GetObject("uxToolBarImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.uxToolBarImages.TransparentColor = System.Drawing.Color.Transparent
        Me.uxToolBarImages.Images.SetKeyName(0, "")
        Me.uxToolBarImages.Images.SetKeyName(1, "")
        Me.uxToolBarImages.Images.SetKeyName(2, "")
        Me.uxToolBarImages.Images.SetKeyName(3, "")
        Me.uxToolBarImages.Images.SetKeyName(4, "")
        Me.uxToolBarImages.Images.SetKeyName(5, "")
        Me.uxToolBarImages.Images.SetKeyName(6, "")
        Me.uxToolBarImages.Images.SetKeyName(7, "")
        Me.uxToolBarImages.Images.SetKeyName(8, "")
        Me.uxToolBarImages.Images.SetKeyName(9, "")
        Me.uxToolBarImages.Images.SetKeyName(10, "")
        Me.uxToolBarImages.Images.SetKeyName(11, "")
        Me.uxToolBarImages.Images.SetKeyName(12, "")
        '
        'Panel1
        '
        Me.Panel1.AutoScroll = True
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.Panel1.Controls.Add(Me.uxItems)
        Me.Panel1.Controls.Add(Me.uxSections)
        Me.Panel1.Controls.Add(Me.uxTemplates)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(680, 550)
        Me.Panel1.TabIndex = 0
        '
        'uxItems
        '
        Me.uxItems.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxItems.Controls.Add(Me.uxItemToolbarPanel)
        Me.uxItems.Controls.Add(Me.bdn)
        Me.uxItems.Controls.Add(Me.bup)
        Me.uxItems.Controls.Add(Me.litems)
        Me.uxItems.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxItems.Location = New System.Drawing.Point(8, 233)
        Me.uxItems.Name = "uxItems"
        Me.uxItems.Size = New System.Drawing.Size(664, 311)
        Me.uxItems.TabIndex = 2
        Me.uxItems.TabStop = False
        Me.uxItems.Text = "Items"
        '
        'uxItemToolbarPanel
        '
        Me.uxItemToolbarPanel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxItemToolbarPanel.Controls.Add(Me.uxItemToolbar)
        Me.uxItemToolbarPanel.Location = New System.Drawing.Point(96, 24)
        Me.uxItemToolbarPanel.Name = "uxItemToolbarPanel"
        Me.uxItemToolbarPanel.Size = New System.Drawing.Size(560, 24)
        Me.uxItemToolbarPanel.TabIndex = 54
        '
        'uxItemToolbar
        '
        Me.uxItemToolbar.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxItemToolbar.Appearance = System.Windows.Forms.ToolBarAppearance.Flat
        Me.uxItemToolbar.Dock = System.Windows.Forms.DockStyle.None
        Me.uxItemToolbar.DropDownArrows = True
        Me.uxItemToolbar.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxItemToolbar.ImageList = Me.uxToolBarImages
        Me.uxItemToolbar.Location = New System.Drawing.Point(0, 0)
        Me.uxItemToolbar.Name = "uxItemToolbar"
        Me.uxItemToolbar.ShowToolTips = True
        Me.uxItemToolbar.Size = New System.Drawing.Size(560, 28)
        Me.uxItemToolbar.TabIndex = 0
        Me.uxItemToolbar.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right
        '
        'bdn
        '
        Me.bdn.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.bdn.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bdn.ImageIndex = 11
        Me.bdn.ImageList = Me.uxToolBarImages
        Me.bdn.Location = New System.Drawing.Point(48, 80)
        Me.bdn.Name = "bdn"
        Me.bdn.Size = New System.Drawing.Size(32, 28)
        Me.bdn.TabIndex = 1
        Me.ToolTip1.SetToolTip(Me.bdn, "Moves highlighted item down in display order.")
        Me.bdn.UseVisualStyleBackColor = False
        '
        'bup
        '
        Me.bup.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.bup.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bup.ImageIndex = 10
        Me.bup.ImageList = Me.uxToolBarImages
        Me.bup.Location = New System.Drawing.Point(48, 48)
        Me.bup.Name = "bup"
        Me.bup.Size = New System.Drawing.Size(32, 28)
        Me.bup.TabIndex = 0
        Me.ToolTip1.SetToolTip(Me.bup, "Moves highlighted item up in display order.")
        Me.bup.UseVisualStyleBackColor = False
        '
        'litems
        '
        Me.litems.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.litems.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4, Me.ColumnHeader5, Me.ColumnHeader9, Me.ColumnHeader6, Me.ColumnHeader7, Me.ColumnHeader8, Me.ColumnHeader10, Me.ColumnHeader11})
        Me.litems.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.litems.FullRowSelect = True
        Me.litems.HideSelection = False
        Me.litems.Location = New System.Drawing.Point(96, 49)
        Me.litems.MultiSelect = False
        Me.litems.Name = "litems"
        Me.litems.Size = New System.Drawing.Size(560, 255)
        Me.litems.SmallImageList = Me.uxToolBarImages
        Me.litems.TabIndex = 2
        Me.ToolTip1.SetToolTip(Me.litems, "Displays items and attributes in highlighted section.")
        Me.litems.UseCompatibleStateImageBehavior = False
        Me.litems.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Item"
        Me.ColumnHeader1.Width = 170
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Symbol"
        Me.ColumnHeader2.Width = 65
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Factor"
        Me.ColumnHeader3.Width = 64
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Data Type"
        Me.ColumnHeader4.Width = 84
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "Storage"
        Me.ColumnHeader5.Width = 59
        '
        'ColumnHeader9
        '
        Me.ColumnHeader9.Text = "Link Code"
        '
        'ColumnHeader6
        '
        Me.ColumnHeader6.Text = "Flexible"
        Me.ColumnHeader6.Width = 49
        '
        'ColumnHeader7
        '
        Me.ColumnHeader7.Text = "Repeating"
        Me.ColumnHeader7.Width = 59
        '
        'ColumnHeader8
        '
        Me.ColumnHeader8.Text = "Lookup"
        Me.ColumnHeader8.Width = 84
        '
        'ColumnHeader10
        '
        Me.ColumnHeader10.Text = "Monetary"
        '
        'ColumnHeader11
        '
        Me.ColumnHeader11.Text = "Price Related"
        '
        'uxSections
        '
        Me.uxSections.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxSections.Controls.Add(Me.uxSectionToolBarPanel)
        Me.uxSections.Controls.Add(Me.bsecdn)
        Me.uxSections.Controls.Add(Me.bsecup)
        Me.uxSections.Controls.Add(Me.Lselsec)
        Me.uxSections.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxSections.Location = New System.Drawing.Point(8, 92)
        Me.uxSections.Name = "uxSections"
        Me.uxSections.Size = New System.Drawing.Size(664, 136)
        Me.uxSections.TabIndex = 1
        Me.uxSections.TabStop = False
        Me.uxSections.Text = "Sections"
        '
        'uxSectionToolBarPanel
        '
        Me.uxSectionToolBarPanel.Controls.Add(Me.uxSectionToolbar)
        Me.uxSectionToolBarPanel.Location = New System.Drawing.Point(96, 22)
        Me.uxSectionToolBarPanel.Name = "uxSectionToolBarPanel"
        Me.uxSectionToolBarPanel.Size = New System.Drawing.Size(232, 24)
        Me.uxSectionToolBarPanel.TabIndex = 54
        '
        'uxSectionToolbar
        '
        Me.uxSectionToolbar.Appearance = System.Windows.Forms.ToolBarAppearance.Flat
        Me.uxSectionToolbar.DropDownArrows = True
        Me.uxSectionToolbar.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxSectionToolbar.ImageList = Me.uxToolBarImages
        Me.uxSectionToolbar.Location = New System.Drawing.Point(0, 0)
        Me.uxSectionToolbar.Name = "uxSectionToolbar"
        Me.uxSectionToolbar.ShowToolTips = True
        Me.uxSectionToolbar.Size = New System.Drawing.Size(232, 28)
        Me.uxSectionToolbar.TabIndex = 0
        Me.uxSectionToolbar.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right
        '
        'bsecdn
        '
        Me.bsecdn.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.bsecdn.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bsecdn.ForeColor = System.Drawing.SystemColors.ControlText
        Me.bsecdn.ImageIndex = 11
        Me.bsecdn.ImageList = Me.uxToolBarImages
        Me.bsecdn.Location = New System.Drawing.Point(48, 80)
        Me.bsecdn.Name = "bsecdn"
        Me.bsecdn.Size = New System.Drawing.Size(32, 28)
        Me.bsecdn.TabIndex = 1
        Me.ToolTip1.SetToolTip(Me.bsecdn, "Move section down in display order.")
        Me.bsecdn.UseVisualStyleBackColor = False
        '
        'bsecup
        '
        Me.bsecup.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.bsecup.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bsecup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.bsecup.ImageIndex = 10
        Me.bsecup.ImageList = Me.uxToolBarImages
        Me.bsecup.Location = New System.Drawing.Point(48, 48)
        Me.bsecup.Name = "bsecup"
        Me.bsecup.Size = New System.Drawing.Size(32, 28)
        Me.bsecup.TabIndex = 0
        Me.ToolTip1.SetToolTip(Me.bsecup, "Move section up in display order.")
        Me.bsecup.UseVisualStyleBackColor = False
        '
        'Lselsec
        '
        Me.Lselsec.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Lselsec.Location = New System.Drawing.Point(96, 48)
        Me.Lselsec.Name = "Lselsec"
        Me.Lselsec.Size = New System.Drawing.Size(232, 82)
        Me.Lselsec.TabIndex = 2
        Me.ToolTip1.SetToolTip(Me.Lselsec, "List of all sections in template with period sections first followed by static se" & _
                "ctions.")
        '
        'uxTemplates
        '
        Me.uxTemplates.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxTemplates.Controls.Add(Me.lintsheet)
        Me.uxTemplates.Controls.Add(Me.ttemplatenew)
        Me.uxTemplates.Controls.Add(Me.ctemplate)
        Me.uxTemplates.Controls.Add(Me.Label1)
        Me.uxTemplates.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxTemplates.Location = New System.Drawing.Point(8, 8)
        Me.uxTemplates.Name = "uxTemplates"
        Me.uxTemplates.Size = New System.Drawing.Size(664, 80)
        Me.uxTemplates.TabIndex = 0
        Me.uxTemplates.TabStop = False
        Me.uxTemplates.Text = "Templates"
        '
        'lintsheet
        '
        Me.lintsheet.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.lintsheet.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lintsheet.Location = New System.Drawing.Point(16, 56)
        Me.lintsheet.Name = "lintsheet"
        Me.lintsheet.Size = New System.Drawing.Size(312, 16)
        Me.lintsheet.TabIndex = 2
        '
        'ttemplatenew
        '
        Me.ttemplatenew.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ttemplatenew.Location = New System.Drawing.Point(96, 24)
        Me.ttemplatenew.Name = "ttemplatenew"
        Me.ttemplatenew.Size = New System.Drawing.Size(168, 21)
        Me.ttemplatenew.TabIndex = 1
        Me.ToolTip1.SetToolTip(Me.ttemplatenew, "Add new template name here.")
        '
        'ctemplate
        '
        Me.ctemplate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ctemplate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ctemplate.ItemHeight = 13
        Me.ctemplate.Location = New System.Drawing.Point(96, 24)
        Me.ctemplate.Name = "ctemplate"
        Me.ctemplate.Size = New System.Drawing.Size(232, 21)
        Me.ctemplate.TabIndex = 52
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!)
        Me.Label1.Location = New System.Drawing.Point(16, 27)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(72, 16)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Template:"
        '
        'bc_am_in_context
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.AutoScroll = True
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(680, 550)
        Me.ControlBox = False
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "bc_am_in_context"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Panel1.ResumeLayout(False)
        Me.uxItems.ResumeLayout(False)
        Me.uxItemToolbarPanel.ResumeLayout(False)
        Me.uxItemToolbarPanel.PerformLayout()
        Me.uxSections.ResumeLayout(False)
        Me.uxSectionToolBarPanel.ResumeLayout(False)
        Me.uxSectionToolBarPanel.PerformLayout()
        Me.uxTemplates.ResumeLayout(False)
        Me.uxTemplates.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region
    Public Shared asheet As bc_om_insight_sheet
    Public osheet As bc_om_insight_sheet
    Public csheet As bc_om_insight_sheet
    Public selsec As Integer
    Public change As Boolean
    Public selected_keyword As String = ""
    Public Shared insight_items As bc_om_insight_items
    Public Shared filter_insight_items As New bc_om_insight_items
    Public from_edit As Boolean = False
    Public ofcs As bc_am_in_settings
    Public oret As bc_am_in_retrieval
    Public ocont As bc_am_in_assignment
    Public oexcel As Object
    Public oexcelapp As Object
    Public tk_main As Object
    Public prev_idx As Integer = -1
    Public Shared loaded As Boolean = False
    Public Sub load_start(Optional ByVal template_only As Boolean = False)
        Try
            Me.ctemplate.Visible = True
            Dim i As Integer
            prev_idx = -1
            Me.ttemplatenew.Clear()
            Me.ttemplatenew.Visible = False
            Me.Lselsec.Items.Clear()
            Me.litems.Items.Clear()
            asheet = New bc_om_insight_sheet(-99)
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                asheet.db_read()
            Else
                asheet.tmode = bc_cs_soap_base_class.tREAD
                If asheet.transmit_to_server_and_receive(asheet, True) = False Then
                    Exit Sub
                End If
            End If

            Me.ctemplate.Items.Clear()
            For i = 0 To asheet.logical_templates_names.Count - 1
                Me.ctemplate.Items.Add(asheet.logical_templates_names(i))
            Next


            REM load items
            insight_items = New bc_om_insight_items
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                insight_items.db_read()
            Else
                insight_items.tmode = bc_cs_soap_base_class.tREAD
                If insight_items.transmit_to_server_and_receive(insight_items, True) = False Then
                    Exit Sub
                End If
            End If

            Me.uxSections.Enabled = False
            Me.uxItems.Enabled = False



        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_in_context", "load_start", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
        End Try
    End Sub
    'Private Sub load_client_settings()
    '    Try
    '        REM firstly attempt to get file from server and copy locally
    '        Dim oinsightconfig As New bc_om_insight_config_files
    '        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
    '            oinsightconfig.db_read()
    '        Else
    '            oinsightconfig.tmode = oinsightconfig.tREAD
    '            If oinsightconfig.transmit_to_server_and_receive(oinsightconfig, True) = True Then
    '                Dim fs As New bc_cs_file_transfer_services
    '                If oinsightconfig.xml_file_exists = True Then
    '                    fs.write_bytestream_to_document(bc_cs_central_settings.local_template_path + "bc_am_insight_config.xml", oinsightconfig.xml_file, Nothing)
    '                End If
    '            End If
    '        End If
    '        Dim oinsight3 As New bc_am_insight_parameters(True)
    '        If oinsight3.load_failed = False Then
    '            bc_am_insight_formats
    '            REM set up dialogue with these settings
    '            ofcs.load_settings()
    '        End If
    '    Catch ex As Exception
    '        Dim oerr As New bc_cs_error_log("bc_am_in_context", "load_client_settings", bc_cs_error_codes.USER_DEFINED, ex.Message)
    '    Finally

    '    End Try

    'End Sub

    Private Sub bclose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Hide()
    End Sub
    Public Sub reset_template_controls()
        Try
            Dim i As Integer
            For i = 0 To 9
                Me.tk_main.set_button_disabled(i)
            Next
            Me.tk_main.set_button_enabled(0)
            Me.uxSections.Enabled = False
            Me.uxItems.Enabled = False
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_in_context", "reset_template_controls", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
        End Try

    End Sub
    Private Sub ctemplate_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ctemplate.SelectedIndexChanged
        REM load selected template into memory

        Dim i As Integer
        Try
            If Me.ctemplate.SelectedIndex = -1 Then
                prev_idx = -1
                tk_main.set_template("")
                Me.tk_main.disable_import()
                Exit Sub
            End If
            If prev_idx = Me.ctemplate.SelectedIndex And prev_idx <> -1 Then
                Exit Sub
            End If
            Me.tk_main.enable_import()
            Me.uxItemToolbar.Buttons(0).Enabled = False
            Me.uxItemToolbar.Buttons(1).Enabled = False
            Me.uxItemToolbar.Buttons(2).Enabled = False
            Me.uxSectionToolbar.Buttons(0).Enabled = True
            Me.uxSectionToolbar.Buttons(1).Enabled = True

            If change = True Then
                Dim omsg As New bc_cs_message("Blue Curve", "Changes have occurred to the previously selected template, continuing will discard these changes." & vbCrLf & "Do you wish to save the changes first?", bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
                If omsg.cancel_selected Then
                    Me.ctemplate.SelectedIndex = prev_idx
                    Exit Sub
                End If
            End If
            If Me.ctemplate.Visible = True Then
                tk_main.set_template(Me.ctemplate.Text)
            Else
                tk_main.set_template(Me.ttemplatenew.Text)
            End If
            prev_idx = Me.ctemplate.SelectedIndex
            If prev_idx = -1 Then
                Exit Sub
            End If
            Me.lintsheet.Text = ""

            Me.clear_change()
            Me.clear_change("Template")
            Me.uxItems.Enabled = False
            Me.uxSections.Enabled = False
            change = False
            Me.Cursor = Cursors.WaitCursor
            Me.Lselsec.Items.Clear()
            Me.litems.Items.Clear()
            Me.litems.Items.Clear()
            Me.tk_main.set_status("Downloading Template Configuration for: " + asheet.logical_templates_names(Me.ctemplate.SelectedIndex))

            osheet = New bc_om_insight_sheet(asheet.logical_templates_ids(Me.ctemplate.SelectedIndex))
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                osheet.db_read()
            Else
                osheet.tmode = bc_cs_soap_base_class.tREAD
                If osheet.transmit_to_server_and_receive(osheet, True) = False Then
                    Exit Sub
                End If
            End If
            Me.Lselsec.Items.Clear()
            REM load sections for template
            REM period
            load_sections()
            Me.lintsheet.Visible = True
            Me.lintsheet.Text = ""
            REM intermediate sheet
            For i = 0 To bc_am_in_context.insight_items.intermediate_sheets.insert_sheets.Count - 1
                If bc_am_in_context.insight_items.intermediate_sheets.insert_sheets(i).id = asheet.logical_templates_ids(Me.ctemplate.SelectedIndex) Then
                    Me.lintsheet.Text = "Intermediate sheet: " + bc_am_in_context.insight_items.intermediate_sheets.insert_sheets(i).file_name
                    Exit For
                End If
            Next
            set_template_selected()
            Me.uxSections.Enabled = True
            Me.tk_main.clear_status()
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_in_context_Load", "Load template", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub
    Private Sub set_template_selected()
        Me.tk_main.set_button_enabled(0)
        Me.tk_main.set_button_enabled(4)
        If Me.ctemplate.Visible = True Then
            Me.tk_main.set_button_enabled(5)
        End If
        Me.tk_main.set_button_enabled(0)
        Me.tk_main.set_button_enabled(1)
        Me.tk_main.set_button_enabled(2)
        REM if no inermediate sheet
        If Me.lintsheet.Text = "" Then
            Me.tk_main.set_button_enabled(7)
            Me.tk_main.set_button_disabled(8)
            Me.tk_main.set_button_disabled(9)
        Else
            Me.tk_main.set_button_disabled(7)
            Me.tk_main.set_button_enabled(8)
            Me.tk_main.set_button_enabled(9)
        End If

    End Sub


    Private Sub load_sections()
        Try
            Dim i As Integer
            Me.uxSectionToolbar.Buttons(2).Enabled = False
            Me.Lselsec.Items.Clear()
            For i = 0 To osheet.bc_om_insightsections.Count - 1
                Me.Lselsec.Items.Add(osheet.bc_om_insightsections(i).name)
            Next
            REM static
            For i = 0 To osheet.bc_om_insightsections_static.Count - 1
                Me.Lselsec.Items.Add(osheet.bc_om_insightsections_static(i).name + " (static)")
            Next
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_in_context", "load_sections", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

        End Try
    End Sub


    Private Sub Lselsec_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Lselsec.SelectedIndexChanged
        Try
            If Me.Lselsec.SelectedIndex = -1 Then
                Me.uxSectionToolbar.Buttons(2).Enabled = False
                Exit Sub
            End If
            Me.uxItemToolbar.Buttons(2).Enabled = False
            Me.uxItemToolbar.Buttons(1).Enabled = False
            Me.uxItemToolbar.Buttons(0).Enabled = True
            Me.uxSectionToolbar.Buttons(2).Enabled = True
            Me.bsecup.Enabled = True
            Me.bsecdn.Enabled = True
            If Lselsec.SelectedIndex = 0 Or Me.Lselsec.SelectedIndex = osheet.bc_om_insightsections.Count Then
                Me.bsecup.Enabled = False
            End If
            If Lselsec.SelectedIndex = (Lselsec.Items.Count - 1) Or Lselsec.SelectedIndex = osheet.bc_om_insightsections.Count - 1 Then
                Me.bsecdn.Enabled = False
            End If


            populate_sections()
            Me.uxItemToolbar.Buttons(0).Enabled = True

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_in_context", "Lselsec_SelectedIndexChanged", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

        End Try
    End Sub
    Private Sub populate_sections(Optional ByVal from_edit As Boolean = False)
        Dim idx As Integer

        Try
            Dim i, j As Integer
            Dim lvew As ListViewItem
            If from_edit = True Then
                idx = Me.litems.SelectedItems(0).Index
            End If
            Me.from_edit = False
            Me.bup.Enabled = False
            Me.selsec = Me.Lselsec.SelectedIndex
            Me.bdn.Enabled = False
            Me.uxItems.Enabled = True
            Me.uxItemToolbar.Buttons(0).Enabled = False

            Me.litems.Items.Clear()
            If Me.Lselsec.SelectedIndex < osheet.bc_om_insightsections.Count Then
                For i = 0 To osheet.bc_om_insightsections(Me.Lselsec.SelectedIndex).rows.count - 1
                    With osheet.bc_om_insightsections(Me.Lselsec.SelectedIndex).rows(i)
                        lvew = New ListViewItem(CStr(.label), 12)
                        If .scale_symbol <> "" Then
                            lvew.SubItems.Add(.scale_symbol)
                        Else
                            lvew.SubItems.Add("n/a")
                        End If
                        lvew.SubItems.Add(.scale_factor)
                        If .datatype = "" Then
                            lvew.SubItems.Add("not set")
                        Else
                            Select Case .datatype
                                Case 0
                                    lvew.SubItems.Add("Number")
                                Case 1
                                    lvew.SubItems.Add("Boolean")
                                Case 2
                                    lvew.SubItems.Add("String")
                                Case 3
                                    lvew.SubItems.Add("Date")
                                Case 6
                                    Dim found As Boolean
                                    found = False
                                    For j = 0 To bc_am_in_context.insight_items.contributor_ids.Count - 1
                                        If CStr(bc_am_in_context.insight_items.contributor_ids(j)) = CStr(.lookup_sql) Then
                                            lvew.SubItems.Add("Output (" + bc_am_in_context.insight_items.contributor_names(j) + ")")
                                            found = True
                                            Exit For
                                        End If
                                    Next
                                    If found = False Then
                                        lvew.SubItems.Add("Output")
                                    End If
                            End Select
                        End If
                        Select Case .submission_code
                            Case 0
                                REM all period is 0
                                lvew.SubItems.Add("period")
                                'Me.cstorage.SelectedIndex = 0
                                'Me.cstorage.Enabled = False
                        End Select
                        If .link_code = "" Then
                            lvew.SubItems.Add("none")
                        Else
                            lvew.SubItems.Add(.link_code)
                        End If
                        If .flexible_label_flag = 0 Then
                            lvew.SubItems.Add("no")
                        Else
                            lvew.SubItems.Add("yes")
                        End If


                        REM for period repeating count always zero
                        lvew.SubItems.Add("0")
                        'Me.Crepeat.SelectedIndex = 0
                        'Me.Crepeat.Enabled = False

                        REM lookup only valid for 7
                        If .lookup_sql = "" Or .datatype = 6 Then
                            lvew.SubItems.Add("n/a")
                        Else
                            If .lookup_sql = "" Then
                                lvew.SubItems.Add("none")
                            Else
                                lvew.SubItems.Add(.lookup_sql)
                            End If
                        End If
                        If .monetary = 0 Then
                            lvew.SubItems.Add("no")
                        Else
                            lvew.SubItems.Add("yes")
                        End If
                        If .price_related = 0 Then
                            lvew.SubItems.Add("no")
                        Else
                            lvew.SubItems.Add("yes")
                        End If
                        Me.litems.Items.Add(lvew)
                    End With
                Next
            Else
                For i = 0 To osheet.bc_om_insightsections_static(Me.Lselsec.SelectedIndex - osheet.bc_om_insightsections.Count).rows.count - 1
                    With osheet.bc_om_insightsections_static(Me.Lselsec.SelectedIndex - osheet.bc_om_insightsections.Count).rows(i)
                        lvew = New ListViewItem(CStr(.label), 12)
                        If .scale_symbol <> "" Then
                            lvew.SubItems.Add(.scale_symbol)
                        Else
                            lvew.SubItems.Add("n/a")
                        End If
                        lvew.SubItems.Add(.scale_factor)
                        If .datatype = "" Then
                            lvew.SubItems.Add("not set")
                        Else

                            Select Case .datatype
                                Case 0
                                    lvew.SubItems.Add("Number")
                                Case 1
                                    lvew.SubItems.Add("Boolean")
                                Case 2
                                    lvew.SubItems.Add("String")
                                Case 3
                                    lvew.SubItems.Add("Date")
                                Case 6
                                    Dim found As Boolean
                                    found = False
                                    For j = 0 To bc_am_in_context.insight_items.contributor_ids.Count - 1
                                        If CStr(bc_am_in_context.insight_items.contributor_ids(j)) = .lookup_sql Then
                                            lvew.SubItems.Add("Output (" + bc_am_in_context.insight_items.contributor_names(j) + ")")
                                            found = True
                                            Exit For
                                        End If
                                    Next
                                    If found = False Then
                                        lvew.SubItems.Add("Output")
                                    End If
                            End Select
                        End If


                        Select Case .submission_code
                            Case 1
                                lvew.SubItems.Add("value")
                            Case 2
                                lvew.SubItems.Add("time series")
                            Case 3
                                lvew.SubItems.Add("repeating")
                            Case 4
                                lvew.SubItems.Add("repeating time series")


                        End Select
                        If .link_code = "" Then
                            lvew.SubItems.Add("none")
                        Else
                            lvew.SubItems.Add(.link_code)
                        End If
                        If .flexible_label_flag = 0 Then
                            lvew.SubItems.Add("no")
                        Else
                            lvew.SubItems.Add("yes")
                        End If
                        REM for period repeating count always zero
                        If Not IsNumeric(.repeating_count) Then
                            lvew.SubItems.Add("0")
                        Else
                            lvew.SubItems.Add(.repeating_count)
                        End If
                        REM lookup only valid for 7
                        If .lookup_sql = "" Or .datatype = 6 Then
                            lvew.SubItems.Add("n/a")
                        Else
                            lvew.SubItems.Add(.lookup_sql)
                        End If
                        If .monetary = 0 Then
                            lvew.SubItems.Add("no")
                        Else
                            lvew.SubItems.Add("yes")
                        End If
                        If .price_related = 0 Then
                            lvew.SubItems.Add("no")
                        Else
                            lvew.SubItems.Add("yes")
                        End If
                        Me.litems.Items.Add(lvew)
                    End With
                Next
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_in_context_Load", "Load section", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            If from_edit = True Then
                Me.litems.Items(idx).Selected = True
            End If
            If Me.selsec <> -1 Then
                Me.uxItemToolbar.Buttons(0).Enabled = True
            End If
            'Me.clear_status()
            'Me.Cursor = Cursors.Default
        End Try

    End Sub

    Private Sub bclose_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            If change = True Then
                Dim omsg As New bc_cs_message("Blue Curve", "Changes have occured to previously selected template continuing will discard these changes Proceed without comiting changes first?", bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
                If omsg.cancel_selected Then
                    Exit Sub
                End If
            End If
            Me.Hide()
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_in_context_Load", "bclose_Click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
        End Try
    End Sub

    Private Sub litems_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles litems.SelectedIndexChanged
        Try
            If Me.litems.SelectedItems.Count = 0 Then
                Me.uxItemToolbar.Buttons(2).Enabled = False
                Me.uxItemToolbar.Buttons(1).Enabled = False
                Exit Sub
            End If
            Me.uxItemToolbar.Buttons(2).Enabled = True
            Me.uxItemToolbar.Buttons(1).Enabled = True
            Me.bup.Enabled = True
            Me.bdn.Enabled = True
            If Me.litems.SelectedItems(0).Index = 0 Then
                Me.bup.Enabled = False
            End If
            If Me.litems.SelectedItems(0).Index = Me.litems.Items.Count - 1 Then
                Me.bdn.Enabled = False
            End If
        Catch ex As Exception
        Finally
        End Try

    End Sub
    Private Sub load_item_attributes(ByVal mode As Boolean)
        Try
            Dim fitems As New bc_am_in_tk_item
            fitems.from_load = True
            Dim lvew As ListViewItem

            Dim i, j As Integer
            REM load edit drop downs
            fitems.csymbol.Items.Clear()
            fitems.csymbol.Items.Add("(m)")
            fitems.csymbol.Items.Add("(bn)")
            fitems.csymbol.Items.Add("(%)")
            fitems.csymbol.Items.Add("(x)")
            fitems.csymbol.Items.Add("(000s)")
            fitems.csymbol.Items.Add("(text)")
            fitems.csymbol.Items.Add("(date)")
            fitems.csymbol.Items.Add("(true/false)")
            fitems.cfactor.Items.Clear()
            fitems.cfactor.Items.Add("1")
            fitems.cfactor.Items.Add("0.01")
            fitems.cfactor.Items.Add("10")
            fitems.cfactor.Items.Add("100")
            fitems.cfactor.Items.Add("1000")
            fitems.cfactor.Items.Add("1000000")
            fitems.cfactor.Items.Add("1000000000")
            fitems.cdatatype.Items.Clear()
            fitems.cdatatype.Items.Add("Number")
            fitems.cdatatype.Items.Add("Boolean")
            fitems.cdatatype.Items.Add("String")
            fitems.cdatatype.Items.Add("Date")
            fitems.cdatatype.Items.Add("Output")
            fitems.cstorage.Items.Clear()
            fitems.edit_mode = mode
            fitems.cmon.Checked = False
            fitems.Cprice.Checked = False
            fitems.Crepeat.Items.Clear()
            For i = 0 To 20
                fitems.Crepeat.Items.Add(CStr(i))
            Next
            fitems.Crepeat.SelectedIndex = 0
            fitems.ccont.Items.Clear()
            For i = 0 To bc_am_in_context.insight_items.contributor_names.Count - 1
                fitems.ccont.Items.Add(bc_am_in_context.insight_items.contributor_names(i))
            Next

            If Me.selsec < osheet.bc_om_insightsections.Count Then
                fitems.cstorage.Items.Add("period")
                fitems.cstorage.SelectedIndex = 0
                fitems.cstorage.Enabled = False
                fitems.Crepeat.Enabled = False
                fitems.uxItems.Text = "Item Name:"
            Else
                fitems.cstorage.Items.Add("value")
                fitems.cstorage.Items.Add("time series")
                fitems.cstorage.Items.Add("repeating")
                fitems.cstorage.Items.Add("repeating time series")
            End If
            fitems.lval.Items.Clear()
            For i = 0 To osheet.validations_names.Count - 1
                lvew = New ListViewItem(CStr(osheet.validations_names(i)))
                lvew.SubItems.Add("")
                If Me.selsec < osheet.bc_om_insightsections.Count Then
                    If LCase(osheet.validations_names(i)) <> "look up" Then
                        fitems.lval.Items.Add(lvew)
                    End If
                Else
                    fitems.lval.Items.Add(lvew)
                End If
            Next
            If mode = True Then
                fitems.rnew.Visible = False
                fitems.rexist.Visible = False
                If Me.selsec < osheet.bc_om_insightsections.Count Then
                    With osheet.bc_om_insightsections(Me.selsec).rows(Me.litems.SelectedItems(0).Index)
                        fitems.uxItems.Text = "Edit item " + CStr(.label)

                        REM populate edit details
                        fitems.tlabel.Text = .label
                        fitems.orig_tx = .label
                        fitems.tlinkcode.Text = .link_code
                        fitems.orig_link = .link_code
                        If .monetary = 1 Then
                            fitems.cmon.Checked = True
                        Else
                            fitems.cmon.Checked = False
                        End If
                        If .price_related = 1 Then
                            fitems.Cprice.Checked = True
                        Else
                            fitems.Cprice.Checked = False
                        End If

                        If .flexible_label_flag = 1 Then
                            fitems.cflexible.Checked = True
                        Else
                            fitems.cflexible.Checked = False
                        End If

                        If .datatype < 6 Then
                            fitems.cdatatype.SelectedIndex = .datatype
                        Else
                            fitems.cdatatype.SelectedIndex = 4
                        End If
                        fitems.csymbol.Text = .scale_symbol

                        If .datatype <> 6 Then
                            fitems.tlookup.Text = .lookup_sql
                        Else
                            For i = 0 To bc_am_in_context.insight_items.contributor_ids.Count - 1
                                If IsNumeric(.lookup_Sql) = True Then
                                    If CInt(.lookup_sql) = bc_am_in_context.insight_items.contributor_ids(i) Then
                                        fitems.ccont.Text = bc_am_in_context.insight_items.contributor_names(i)
                                        Exit For
                                    End If
                                Else
                                    fitems.ccont.SelectedIndex = 0
                                    Exit For
                                End If
                            Next
                        End If
                        fitems.cfactor.Text = .scale_factor
                        fitems.Crepeat.Text = "0"
                        For j = 0 To osheet.validations_ids.Count - 1
                            For i = 0 To .validations.bc_om_cell_validation.count - 1
                                If osheet.validations_ids(j) = .validations.bc_om_cell_validation(i).validation_id Then
                                    fitems.lval.Items(j).Checked = True
                                    If osheet.validations_ids(j) = 7 Then
                                        fitems.lval.Items(j).SubItems(1) = Me.litems.SelectedItems(0).SubItems(8)
                                    End If
                                End If
                            Next
                        Next
                    End With

                Else
                    With osheet.bc_om_insightsections_static(Me.selsec - osheet.bc_om_insightsections.Count).rows(Me.litems.SelectedItems(0).Index)
                        fitems.uxItems.Text = "Edit item " + CStr(.label)
                        fitems.tlabel.Text = .label
                        fitems.orig_tx = .label
                        fitems.csymbol.Text = .scale_symbol
                        fitems.tlinkcode.Text = .link_code
                        fitems.orig_link = .link_code
                        If .monetary = 1 Then
                            fitems.cmon.Checked = True
                        Else
                            fitems.cmon.Checked = False
                        End If
                        If .price_related = 1 Then
                            fitems.Cprice.Checked = True
                        Else
                            fitems.Cprice.Checked = False
                        End If
                        If .flexible_label_flag = 1 Then
                            fitems.cflexible.Checked = True
                        Else
                            fitems.cflexible.Checked = False
                        End If
                        If .datatype < 6 Then
                            fitems.cdatatype.SelectedIndex = .datatype
                        Else
                            fitems.cdatatype.SelectedIndex = 4
                        End If
                        Select Case .submission_code
                            Case 1
                                fitems.cstorage.SelectedIndex = 0
                            Case 2
                                fitems.cstorage.SelectedIndex = 1
                            Case 3
                                fitems.cstorage.SelectedIndex = 2
                            Case 4
                                fitems.cstorage.SelectedIndex = 3

                        End Select
                        fitems.cfactor.Text = .scale_factor
                        fitems.Crepeat.Text = .repeating_count
                        fitems.csymbol.Text = .scale_symbol

                        If .datatype <> 6 Then
                            fitems.tlookup.Text = .lookup_sql
                        Else
                            For i = 0 To bc_am_in_context.insight_items.contributor_ids.Count - 1
                                If CInt(.lookup_sql) = bc_am_in_context.insight_items.contributor_ids(i) Then
                                    fitems.ccont.Text = bc_am_in_context.insight_items.contributor_names(i)
                                    Exit For
                                End If
                            Next
                        End If
                        For j = 0 To osheet.validations_ids.Count - 1
                            For i = 0 To .validations.bc_om_cell_validation.count - 1
                                If osheet.validations_ids(j) = .validations.bc_om_cell_validation(i).validation_id Then
                                    fitems.lval.Items(j).Checked = True
                                    If osheet.validations_ids(j) = 7 Then
                                        fitems.lval.Items(j).SubItems(1).Text = Me.litems.SelectedItems(0).SubItems(8).Text
                                    End If
                                End If
                            Next
                        Next
                    End With
                End If
            Else
                fitems.rnew.Visible = True
                fitems.rexist.Visible = True
                fitems.rnew.Checked = True
                fitems.fcontext = Me

            End If

            fitems.osheet = Me.osheet
            fitems.from_load = False
            fitems.ShowDialog()
            If fitems.cancel_selected = False Then
                set_item(mode, fitems)
                If mode = False Then
                    Me.uxItemToolbar.Buttons(1).Enabled = False
                    Me.uxItemToolbar.Buttons(2).Enabled = False
                End If
            End If


        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_in_context_Load", "load_item_attributes", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
        End Try

    End Sub
    Private Sub del_item()
        Try
            Dim omsg As New bc_cs_message("Blue Curve", "Are you sure you wish to remove: " + Me.litems.SelectedItems(0).Text + " from section?", bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
            If omsg.cancel_selected = False Then
                If Me.selsec < osheet.bc_om_insightsections.Count Then
                    osheet.bc_om_insightsections(Me.selsec).rows.removeat(Me.litems.SelectedItems(0).Index)
                    Me.set_change()
                Else
                    osheet.bc_om_insightsections_static(Me.selsec - osheet.bc_om_insightsections.Count).rows.removeat(Me.litems.SelectedItems(0).Index)
                    Me.set_change()
                End If
                Me.populate_sections()
                Me.uxItemToolbar.Buttons(1).Enabled = False
                Me.uxItemToolbar.Buttons(2).Enabled = False
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_in_context_Load", "Bdel_Click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
        End Try
    End Sub

    Private Sub bcalc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim ocalcs As New bc_am_calculations
        bc_am_calculation_values.TemplateId = asheet.logical_templates_ids(Me.ctemplate.SelectedIndex)
        ocalcs.ShowDialog()
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        discard()
    End Sub
    Public Function discard() As Boolean
        Try

            Dim omsg As bc_cs_message
            Dim filename As String
            Dim i As Integer
            discard = False


            REM if internediate sheet exists try and delete
            If Me.lintsheet.Text <> "" Then
                filename = ""
                Dim fs As New bc_cs_file_transfer_services
                For i = 0 To bc_am_in_context.insight_items.intermediate_sheets.insert_sheets.Count - 1
                    If Me.ctemplate.SelectedIndex > -1 And Me.ctemplate.Visible = True Then
                        If bc_am_in_context.insight_items.intermediate_sheets.insert_sheets(i).id = asheet.logical_templates_ids(Me.ctemplate.SelectedIndex) Then
                            filename = bc_am_in_context.insight_items.intermediate_sheets.insert_sheets(i).file_name
                            Exit For
                        End If
                    Else
                        If bc_am_in_context.insight_items.intermediate_sheets.insert_sheets(i).id = 0 Then
                            filename = bc_am_in_context.insight_items.intermediate_sheets.insert_sheets(i).file_name
                            Exit For
                        End If
                    End If
                Next
                If filename <> "" Then
                    If fs.check_document_exists(bc_cs_central_settings.local_template_path + filename, Nothing) = True Then
                        If fs.delete_file(bc_cs_central_settings.local_template_path + filename) = "in use" Then
                            omsg = New bc_cs_message("Blue Curve", "Intermediate sheet: " + filename + " is still open in Excel please close prior to discarding template definition", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                            Exit Function
                        End If
                    End If
                End If
            End If
            Me.lintsheet.Text = ""
            load_start()
            discard = True
            Me.uxItemToolbar.Buttons(0).Enabled = False
            Me.uxItemToolbar.Buttons(1).Enabled = False
            Me.uxItemToolbar.Buttons(2).Enabled = False
            Me.uxSectionToolbar.Buttons(0).Enabled = False
            Me.uxSectionToolbar.Buttons(1).Enabled = False
            Me.uxSectionToolbar.Buttons(2).Enabled = False
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_in_context_Load", "discard", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Function
    Private Sub clear_change()
        Try
            'Me.mup.Enabled = False
            'Me.mdn.Enabled = False
            Me.change = False
            Me.clear_change("Template")
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_in_context_Load", "clear_change", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub
    Private Sub copy_temp(ByVal tx As String)
        Dim omsg As bc_cs_message
        Try
            Dim i, j As Integer

            If Me.ttemplatenew.Visible = True Then
                omsg = New bc_cs_message("Blue Curve", "Copying: " + tx + " to " + Me.ttemplatenew.Text + " will remove exisiting definition for " + Me.ttemplatenew.Text + " are you sure?", bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
                Me.set_status("Downloading definition for " + tx + " and assiging to " + Me.ttemplatenew.Text)
            Else
                omsg = New bc_cs_message("Blue Curve", "Copying: " + tx + " to " + Me.ctemplate.Text + " will remove exisiting definition for " + Me.ctemplate.Text + " are you sure?", bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
                Me.set_status("Downloading definition for " + tx + " and assiging to " + Me.ctemplate.Text)
            End If
            Me.change = True
            If omsg.cancel_selected = True Then
                Exit Sub
            End If
            Me.Cursor = Cursors.WaitCursor
            Me.Lselsec.Items.Clear()
            Me.Lselsec.Refresh()
            Me.litems.Items.Clear()
            Me.litems.Refresh()
            For j = 0 To asheet.logical_templates_names.Count - 1
                If asheet.logical_templates_names(j) = tx Then
                    REM read definion for template and assign to osheet.
                    osheet = New bc_om_insight_sheet(asheet.logical_templates_ids(j))
                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        osheet.db_read()
                    Else
                        osheet.tmode = bc_cs_soap_base_class.tREAD
                        If osheet.transmit_to_server_and_receive(osheet, True) = False Then
                            Exit Sub
                        End If
                    End If
                    REM now assign new template id
                    If Me.ttemplatenew.Visible = True Then
                        osheet.logical_template_id = 0
                        osheet.logical_template_name = Me.ttemplatenew.Text
                    Else
                        osheet.logical_template_id = asheet.logical_templates_ids(Me.ctemplate.SelectedIndex)
                        osheet.logical_template_name = asheet.logical_templates_names(Me.ctemplate.SelectedIndex)
                    End If
                    REM load sections for template
                    REM period

                    For i = 0 To osheet.bc_om_insightsections.Count - 1
                        Me.Lselsec.Items.Add(osheet.bc_om_insightsections(i).name)
                    Next
                    REM static
                    For i = 0 To osheet.bc_om_insightsections_static.Count - 1
                        Me.Lselsec.Items.Add(osheet.bc_om_insightsections_static(i).name + " (static)")
                    Next
                    Me.uxSections.Enabled = True
                    Exit For
                End If
            Next
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_in_context_Load", "ccpytmp_SelectedIndexChanged", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            Me.Cursor = Cursors.Default
            Me.clear_status()
        End Try
    End Sub
    Private Function check_rows_in_sections() As String
        Dim i As Integer
        Dim ostr As String = ""
        REM check sections in template
        If osheet.bc_om_insightsections.Count = 0 And osheet.bc_om_insightsections_static.Count = 0 Then
            check_rows_in_sections = "no section"
            Exit Function
        End If
        REM check rows in section
        For i = 0 To osheet.bc_om_insightsections.Count - 1
            If osheet.bc_om_insightsections(i).rows.count = 0 Then
                If ostr = "" Then
                    ostr = osheet.bc_om_insightsections(i).name
                Else
                    ostr = ostr + ", " + osheet.bc_om_insightsections(i).name
                End If
            End If
        Next
        For i = 0 To osheet.bc_om_insightsections_static.Count - 1
            If osheet.bc_om_insightsections_static(i).rows.count = 0 Then
                If ostr = "" Then
                    ostr = osheet.bc_om_insightsections_static(i).name
                Else
                    ostr = ostr + ", " + osheet.bc_om_insightsections_static(i).name
                End If
            End If
        Next
        check_rows_in_sections = ostr
    End Function
    Private Sub Bupload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        upload()
    End Sub
    Public Function upload(Optional ByVal show_message As Boolean = True) As Boolean
        Try
            Dim omsg As bc_cs_message
            upload = True
            Dim filename As String = ""
            Dim i As Integer
            prev_idx = -1
            Dim ostr As String
            ostr = Me.check_rows_in_sections
            If ostr <> "" Then
                If ostr = "no section" Then
                    omsg = New bc_cs_message("Blue Curve", "Template has no sections and will not be uploaded do you wish to continue with general upload?", bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
                Else
                    omsg = New bc_cs_message("Blue Curve", "The followings sections will not be uploaded as they have no items: " + ostr + " do you wish to continue with general upload?", bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
                End If
                If omsg.cancel_selected = True Then
                    upload = False
                    Exit Function
                End If
                Me.Cursor = Cursors.WaitCursor
            End If

            If Me.ttemplatenew.Visible = True Then
                Me.set_status("Uploading template: " + Me.ttemplatenew.Text + " to system.")
            Else
                Me.set_status("Uploading template: " + Me.ctemplate.Text + " to system.")
            End If


            REM first check if format sheet is local and not open
            Dim osheet As New bc_om_intermediate_sheet
            Dim fs As New bc_cs_file_transfer_services

            If Me.lintsheet.Text <> "" Then
                filename = ""
                If Me.ctemplate.Visible = True Then
                    For i = 0 To bc_am_in_context.insight_items.intermediate_sheets.insert_sheets.Count - 1
                        If bc_am_in_context.insight_items.intermediate_sheets.insert_sheets(i).id = asheet.logical_templates_ids(Me.ctemplate.SelectedIndex) Then
                            filename = bc_am_in_context.insight_items.intermediate_sheets.insert_sheets(i).file_name
                            Exit For
                        End If
                    Next
                Else
                    For i = 0 To bc_am_in_context.insight_items.intermediate_sheets.insert_sheets.Count - 1
                        If bc_am_in_context.insight_items.intermediate_sheets.insert_sheets(i).id = 0 Then
                            filename = bc_am_in_context.insight_items.intermediate_sheets.insert_sheets(i).file_name
                            Exit For
                        End If
                    Next
                End If
            End If

            If filename <> "" Then
                If fs.check_document_exists(bc_cs_central_settings.local_template_path + filename, Nothing) = True Then
                    REM upload template to server
                    If Me.ctemplate.Visible = True Then
                        osheet.id = asheet.logical_templates_ids(Me.ctemplate.SelectedIndex)
                        osheet.name = asheet.logical_templates_names(Me.ctemplate.SelectedIndex)
                    Else
                        osheet.id = 0
                        osheet.name = Me.ttemplatenew.Text
                    End If
                    osheet.file_name = filename
                    If fs.in_use_by_another_process(bc_cs_central_settings.local_template_path + filename, Nothing) = True Then
                        omsg = New bc_cs_message("Blue Curve", "Intermediate sheet: " + filename + " is open in excel please save and close prior to template upload.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                        upload = False
                        Exit Function
                    End If
                    fs.write_document_to_bytestream(bc_cs_central_settings.local_template_path + filename, osheet.sheet, Nothing)
                    fs.delete_file(bc_cs_central_settings.local_template_path + filename)
                End If
            Else
                If Me.ctemplate.Visible = True Then
                    osheet.id = asheet.logical_templates_ids(Me.ctemplate.SelectedIndex)
                    osheet.name = "DELETE"
                Else
                    osheet.name = ""
                End If
            End If

            If osheet.name <> "" Then
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    osheet.db_write()
                Else
                    osheet.tmode = bc_cs_soap_base_class.tWRITE
                    osheet.no_send_back = True
                    If osheet.transmit_to_server_and_receive(osheet, True) = False Then
                        Exit Function
                    End If
                End If
            End If


            upload_config()
            REM upload templates
            load_start(True)
            Me.change = False
            Me.lintsheet.Text = ""
            Me.ctemplate.SelectedIndex = -1
            Me.ctemplate.Visible = True
            Me.ttemplatenew.Visible = False
            Me.uxItemToolbar.Buttons(0).Enabled = False
            Me.uxItemToolbar.Buttons(1).Enabled = False
            Me.uxItemToolbar.Buttons(2).Enabled = False
            Me.uxSectionToolbar.Buttons(0).Enabled = False
            Me.uxSectionToolbar.Buttons(1).Enabled = False
            Me.uxSectionToolbar.Buttons(2).Enabled = False
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_in_context_Load", "upload", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            Me.Cursor = Cursors.Default

        End Try

    End Function
    Public Sub export_all()
        REM load each template and then export
        Dim file_err As Boolean = False
        Dim sr As StreamWriter
        Dim fn As String = ""
        Try
            fn = bc_cs_central_settings.local_repos_path + "_" + Format(Now, "ddMMMyyyy_hhmmss") + "_insight_configuration.sql"
            sr = New StreamWriter(fn, False)
        Catch
            file_err = True
            Dim omsg As New bc_cs_message("Blue Curve", "Export file: " + fn + " is in use please close and try again.", bc_cs_message.MESSAGE)
            Exit Sub
        End Try

        Try
            Dim stemp As String
            stemp = "-----------------------------------------------------------"
            sr.WriteLine(stemp)
            stemp = "-- Blue Curve Insight Toolkit Automated Installation Script"
            sr.WriteLine(stemp)
            stemp = "-- Created: " + Format(Now, "dd-MMM-yyyy hh:mm")
            sr.WriteLine(stemp)
            stemp = "--"
            sr.WriteLine(stemp)
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                stemp = "-- Exported from database " + bc_cs_central_settings.dbname + " on server " + bc_cs_central_settings.servername
                sr.WriteLine(stemp)
            Else
                stemp = "-- Exported from " + bc_cs_central_settings.soap_server
                sr.WriteLine(stemp)
            End If
            stemp = "-----------------------------------------------------------"
            sr.WriteLine(stemp)
            For i = 0 To asheet.logical_templates_ids.Count - 1
                Me.set_status("Exporting template " + CStr(i + 1) + " of " + CStr(asheet.logical_templates_ids.Count))
                osheet = New bc_om_insight_sheet(asheet.logical_templates_ids(i))
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    osheet.db_read()
                Else
                    osheet.tmode = bc_cs_soap_base_class.tREAD
                    If osheet.transmit_to_server_and_receive(osheet, True) = False Then
                        Exit Sub
                    End If
                End If
                upload_config(True, True, sr, asheet.logical_templates_ids(i), asheet.logical_templates_names(i))
            Next
            REM now do template assigments
            Me.set_status("Exporting template assigments")
            stemp = "delete from insight_entity_class_submission"
            sr.WriteLine(stemp)

            For i = 0 To bc_am_in_context.insight_items.contributing_classes.Count - 1
                With bc_am_in_context.insight_items.contributing_classes(i)
                    REM SR - Column names included in the insert statement
                    stemp = "insert into insight_entity_class_submission (parent_entity_class_id,child_entity_class_id ,contributor_id ,granularity_class_id) values(" + CStr(.parent_class_id) + "," + CStr(.child_class_id) + "," + CStr(.schema_id) + "," + CStr(.granularity_class_id) + ")"
                    sr.WriteLine(stemp)
                    stemp = "delete from bcc_core_insight_temp_assign where class_id=" + CStr(.parent_class_id) + " and schema_id=" + CStr(.schema_id)
                    sr.WriteLine(stemp)
                    If .child_class_id > 0 Then
                        stemp = "delete from bcc_core_insight_temp_assign where class_id=" + CStr(.child_class_id) + " and schema_id=" + CStr(.schema_id)
                        sr.WriteLine(stemp)
                    End If
                    For j = 0 To .entity_template_assigments.Count - 1
                        REM SR - Column names included in the insert statement
                        stemp = "insert into bcc_core_insight_temp_assign (class_id,[schema_id],entity_id,template_id) values(" + CStr(.entity_template_assigments(j).class_id) + "," + CStr(.schema_id) + "," + CStr(.entity_template_assigments(j).entity_id) + "," + CStr(.entity_template_assigments(j).template_id) + ")"
                        sr.WriteLine(stemp)
                    Next

                End With
                sr.WriteLine("Go")
            Next
            Dim omsg As New bc_cs_message("Blue Curve", "Export file: " + fn + " Created", bc_cs_message.MESSAGE)

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_in_context", "export_all", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            If file_err = False Then
                sr.Close()
            End If
            Me.clear_status()
        End Try
    End Sub
    Public Sub upload_config(Optional ByVal export As Boolean = False, Optional ByVal multiple As Boolean = False, Optional ByVal sr As StreamWriter = Nothing, Optional ByVal template_id As Long = 0, Optional ByVal template_name As String = "")

        Try
            Dim oconfigdata As New bc_om_insight_template_config
            Dim i, j, k As Integer
            If multiple = True Then
                oconfigdata.logical_template_id = template_id
                oconfigdata.logical_template_name = template_name
            Else
                If Me.ttemplatenew.Visible = True Then
                    oconfigdata.logical_template_id = 0
                    oconfigdata.logical_template_name = Me.ttemplatenew.Text
                Else
                    oconfigdata.logical_template_id = asheet.logical_templates_ids(Me.ctemplate.SelectedIndex)
                    oconfigdata.logical_template_name = Me.ctemplate.Text
                End If
            End If
            oconfigdata.context_id = 1
            oconfigdata.rows.Clear()
            REM populate this from osheet object

            Dim orow As bc_om_insight_row_config
            For i = 0 To osheet.bc_om_insightsections.Count - 1
                For j = 0 To osheet.bc_om_insightsections(i).rows.count - 1
                    With osheet.bc_om_insightsections(i).rows(j)
                        orow = New bc_om_insight_row_config
                        orow.row_id = .row_id
                        orow.label = .label
                        orow.link_code = .link_code
                        orow.lookup_sql = .lookup_sql
                        orow.order = CStr(j)
                        orow.scale_factor = .scale_factor
                        orow.scale_symbol = .scale_symbol
                        orow.section_name = osheet.bc_om_insightsections(i).name
                        orow.submission_code = .submission_code
                        orow.data_type = .datatype
                        orow.flexible_label_flag = .flexible_label_flag
                        orow.repeating_count = .repeating_count
                        orow.static_flag = 0
                        orow.monetary = .monetary

                        orow.price_related = .price_related
                        For k = 0 To .validations.bc_om_cell_validation.count - 1
                            orow.validations.Add(.validations.bc_om_cell_validation(k).validation_id)
                        Next
                        oconfigdata.rows.Add(orow)
                    End With
                Next
            Next
            For i = 0 To osheet.bc_om_insightsections_static.Count - 1
                For j = 0 To osheet.bc_om_insightsections_static(i).rows.count - 1
                    With osheet.bc_om_insightsections_static(i).rows(j)
                        orow = New bc_om_insight_row_config
                        orow.row_id = .row_id
                        orow.label = .label
                        orow.link_code = .link_code
                        orow.lookup_sql = .lookup_sql
                        orow.order = CStr(j)
                        orow.scale_factor = .scale_factor
                        orow.scale_symbol = .scale_symbol
                        orow.section_name = osheet.bc_om_insightsections_static(i).name
                        orow.submission_code = .submission_code
                        orow.data_type = .datatype
                        orow.flexible_label_flag = .flexible_label_flag
                        orow.repeating_count = .repeating_count
                        orow.static_flag = 1
                        orow.monetary = .monetary
                        orow.price_related = .price_related

                        For k = 0 To .validations.bc_om_cell_validation.count - 1
                            orow.validations.Add(.validations.bc_om_cell_validation(k).validation_id)
                        Next
                        oconfigdata.rows.Add(orow)
                    End With
                Next
            Next
            If export = False Then
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    oconfigdata.db_read()
                Else
                    oconfigdata.tmode = bc_cs_soap_base_class.tREAD
                    oconfigdata.transmit_to_server_and_receive(oconfigdata, True)
                End If
            Else
                export_template(oconfigdata, multiple, sr)
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_in_context", "upload_config", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally

        End Try
    End Sub
    Private Sub export_template(ByVal oconfigdata As bc_om_insight_template_config, Optional ByVal multiple As Boolean = False, Optional ByVal sr As StreamWriter = Nothing)
        Dim stemp As String
        stemp = oconfigdata.write_data_to_xml(Nothing)
        stemp = "exec dbo.bcc_core_import_financial_template '" + oconfigdata.logical_template_name + "','" + stemp + "'"
        If multiple = False Then
            sr = New StreamWriter(bc_cs_central_settings.local_repos_path + oconfigdata.logical_template_name + ".sql", False)
        End If
        sr.WriteLine(stemp)
        sr.WriteLine("go")
        REM now do calcs
        If oconfigdata.logical_template_id Then
            Dim obc_calcs As New bc_om_calculations
            obc_calcs.intTemplateID = oconfigdata.logical_template_id
            bc_am_calculation_values.TemplateId = obc_calcs.intTemplateID
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                obc_calcs.db_read()
            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                obc_calcs.no_send_back = False
                obc_calcs.tmode = bc_cs_soap_base_class.tREAD
                obc_calcs.transmit_to_server_and_receive(obc_calcs, True)
            Else
                Exit Sub
            End If
            REM now write each calcs
            stemp = "delete from insight_calculations_tbl where template_id=" + CStr(oconfigdata.logical_template_id)
            sr.WriteLine(stemp)
            sr.WriteLine("go")
            For i = 0 To obc_calcs.calculations.Count - 1
                stemp = "exec dbo.bcc_core_calc_write " + CStr(obc_calcs.calculations(i).id) + "," + CStr(obc_calcs.calculations(i).result_row_id) + ",'" + CStr(obc_calcs.calculations(i).Name) + "','" + CStr(obc_calcs.calculations(i).Type) + "'," + CStr(oconfigdata.logical_template_id) + "," + CStr(obc_calcs.calculations(i).op1) + "," + CStr(obc_calcs.calculations(i).op2) + "," + CStr(obc_calcs.calculations(i).op3) + "," + CStr(obc_calcs.calculations(i).op4) + "," + CStr(obc_calcs.calculations(i).op5) + "," + CStr(obc_calcs.calculations(i).op6) + "," + CStr(obc_calcs.calculations(i).op7) + "," + CStr(obc_calcs.calculations(i).op8) + ",'" + obc_calcs.calculations(i).formula + "'," + CStr(obc_calcs.calculations(i).calc_when) + "," + CStr(obc_calcs.calculations(i).delete_flag) + "," + CStr(obc_calcs.calculations(i).num_years) + ",'" + obc_calcs.calculations(i).period_name + "'," + CStr(obc_calcs.calculations(i).monetary) + "," + CStr(obc_calcs.calculations(i).monetary2)
                sr.WriteLine(stemp)
            Next
        End If
        sr.WriteLine("go")
        If multiple = False Then
            sr.Close()
        End If
    End Sub
    Private Sub dtemplate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try

            Dim omsg As New bc_cs_message("Blue Curve", "Are you sure you want to remove template and definition for " + Me.ctemplate.Text, bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
            If omsg.cancel_selected = True Then
                Exit Sub
            Else
                Me.Cursor = Cursors.WaitCursor
                Me.set_status("Deleting template definition for: " + Me.ctemplate.Text)

                Dim osheetconfig As New bc_om_insight_template_config
                osheetconfig.logical_template_id = asheet.logical_templates_ids(Me.ctemplate.SelectedIndex)
                osheetconfig.logical_template_name = "DELETE"
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    osheetconfig.db_read()
                Else
                    osheetconfig.tmode = bc_cs_soap_base_class.tREAD
                End If
                load_start()
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_in_context_Load", "dtemplate_Click", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            Me.Cursor = Cursors.Default
            Me.clear_status()
        End Try
    End Sub


    Private Sub bup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bup.Click
        Try
            Dim trow As New bc_om_insight_row
            Dim idx As Integer
            idx = Me.litems.SelectedItems(0).Index - 1
            If Me.litems.SelectedItems(0).Index > 0 Then
                If Me.selsec < osheet.bc_om_insightsections.Count Then
                    trow = osheet.bc_om_insightsections(Me.selsec).rows(Me.litems.SelectedItems(0).Index - 1)
                    osheet.bc_om_insightsections(Me.selsec).rows.removeat(Me.litems.SelectedItems(0).Index - 1)
                    Dim k As New ArrayList
                    osheet.bc_om_insightsections(Me.selsec).rows.insert(Me.litems.SelectedItems(0).Index, trow)
                Else
                    trow = osheet.bc_om_insightsections_static(Me.selsec - osheet.bc_om_insightsections.Count).rows(Me.litems.SelectedItems(0).Index - 1)
                    osheet.bc_om_insightsections_static(Me.selsec - osheet.bc_om_insightsections.Count).rows.removeat(Me.litems.SelectedItems(0).Index - 1)
                    Dim k As New ArrayList
                    osheet.bc_om_insightsections_static(Me.selsec - osheet.bc_om_insightsections.Count).rows.insert(Me.litems.SelectedItems(0).Index, trow)
                End If
                populate_sections()
                Me.litems.Items(idx).Selected = True
                Me.set_change()
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_in_context_Load", "bup_Click", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            Me.Cursor = Cursors.Default

        End Try

    End Sub

    Private Sub bdn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bdn.Click
        Try
            Dim trow As New bc_om_insight_row
            Dim idx As Integer
            idx = Me.litems.SelectedItems(0).Index + 1
            If Me.litems.SelectedItems(0).Index < Me.litems.Items.Count - 1 Then
                If Me.selsec < osheet.bc_om_insightsections.Count Then
                    trow = osheet.bc_om_insightsections(Me.selsec).rows(Me.litems.SelectedItems(0).Index + 1)
                    osheet.bc_om_insightsections(Me.selsec).rows.removeat(Me.litems.SelectedItems(0).Index + 1)
                    Dim k As New ArrayList
                    osheet.bc_om_insightsections(Me.selsec).rows.insert(Me.litems.SelectedItems(0).Index, trow)
                Else
                    trow = osheet.bc_om_insightsections_static(Me.selsec - osheet.bc_om_insightsections.Count).rows(Me.litems.SelectedItems(0).Index + 1)
                    osheet.bc_om_insightsections_static(Me.selsec - osheet.bc_om_insightsections.Count).rows.removeat(Me.litems.SelectedItems(0).Index + 1)
                    Dim k As New ArrayList
                    osheet.bc_om_insightsections_static(Me.selsec - osheet.bc_om_insightsections.Count).rows.insert(Me.litems.SelectedItems(0).Index, trow)
                End If
                populate_sections()
                Me.litems.Items(idx).Selected = True
                Me.set_change()
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_in_context_Load", "bdn_Click", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            Me.Cursor = Cursors.Default

        End Try

    End Sub

    Private Sub set_item(ByVal mode As Boolean, ByVal fitem As bc_am_in_tk_item)
        Try
            Dim i, j As Integer
            Dim oval As bc_om_cell_validation

            j = 0
            Dim orow As New bc_om_insight_row
            orow.label = fitem.tlabel.Text
            orow.link_code = fitem.tlinkcode.Text
            If fitem.cmon.Checked = True Then
                orow.monetary = 1
            Else
                orow.monetary = 0
            End If
            If fitem.Cprice.Checked = True Then
                orow.price_related = 1
            Else
                orow.price_related = 0
            End If
            If fitem.cflexible.Checked = True Then
                orow.flexible_label_flag = 1
            Else
                orow.flexible_label_flag = 0
            End If

            If fitem.ccont.SelectedIndex = -1 Then
                orow.lookup_sql = fitem.tlookup.Text
            Else
                For i = 0 To bc_am_in_context.insight_items.contributor_names.Count - 1
                    If fitem.ccont.Text = bc_am_in_context.insight_items.contributor_names(i) Then
                        orow.lookup_sql = bc_am_in_context.insight_items.contributor_ids(i)
                        Exit For
                    End If
                Next
            End If
            orow.scale_factor = fitem.cfactor.Text
            orow.scale_symbol = fitem.csymbol.Text
            If fitem.cdatatype.SelectedIndex < 4 Then
                orow.datatype = fitem.cdatatype.SelectedIndex
            Else
                orow.datatype = 6
            End If
            orow.validations.bc_om_cell_validation.Clear()

            If Me.Lselsec.SelectedIndex < osheet.bc_om_insightsections.Count Then
                If mode = False Then
                    If fitem.rnew.Checked = True Then
                        orow.row_id = 0
                    Else
                        orow.row_id = bc_am_in_context.filter_insight_items.insight_items(fitem.Cexist.SelectedIndex).item_id
                        orow.label = fitem.Cexist.Text
                    End If
                Else
                    orow.row_id = osheet.bc_om_insightsections(Me.Lselsec.SelectedIndex).rows(Me.litems.SelectedItems(0).Index).row_id
                End If

                orow.static_flag = 0
                orow.submission_code = 0
                orow.repeating_count = 0
                For i = 0 To osheet.validations_ids.Count - 1
                    If i < fitem.lval.Items.Count Then
                        If fitem.lval.Items(i).Checked = True Then
                            oval = New bc_om_cell_validation
                            oval.validation_id = (osheet.validations_ids(i))
                            orow.validations.bc_om_cell_validation.Add(oval)
                        End If
                    End If
                Next
                If mode = True Then
                    osheet.bc_om_insightsections(Me.Lselsec.SelectedIndex).rows(Me.litems.SelectedItems(0).Index) = orow
                Else
                    osheet.bc_om_insightsections(Me.Lselsec.SelectedIndex).rows.add(orow)
                End If
            Else
                If mode = False Then
                    If fitem.rnew.Checked = True Then
                        orow.row_id = 0
                    Else
                        orow.row_id = bc_am_in_context.filter_insight_items.insight_items(fitem.Cexist.SelectedIndex).item_id
                        orow.label = fitem.Cexist.Text
                    End If
                Else
                    orow.row_id = osheet.bc_om_insightsections_static(Me.Lselsec.SelectedIndex - osheet.bc_om_insightsections.Count).rows(Me.litems.SelectedItems(0).Index).row_id
                End If
                orow.static_flag = 1
                orow.submission_code = fitem.cstorage.SelectedIndex + 1
                orow.repeating_count = fitem.Crepeat.Text

                For i = 0 To osheet.validations_ids.Count - 1
                    If fitem.lval.Items(i).Checked = True Then
                        oval = New bc_om_cell_validation
                        oval.validation_id = (osheet.validations_ids(i))
                        orow.validations.bc_om_cell_validation.Add(oval)
                    End If
                Next
                If mode = True Then
                    osheet.bc_om_insightsections_static(Me.Lselsec.SelectedIndex - osheet.bc_om_insightsections.Count).rows(Me.litems.SelectedItems(0).Index) = orow
                Else
                    osheet.bc_om_insightsections_static(Me.Lselsec.SelectedIndex - osheet.bc_om_insightsections.Count).rows.add(orow)
                End If
            End If
            Me.populate_sections(mode)
            Me.set_change()
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_in_context_Load", "set_item", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            Me.Cursor = Cursors.Default

        End Try

    End Sub




    Private Sub MenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        'If Me.luploadstatus.Text <> "" Then
        'omsg = New bc_cs_message("Blue Curve", Me.luploadstatus.Text + " - exit without first comitting these changes?", bc_cs_message.MESSAGE, True, False, "Yes", "No")
        'If omsg.cancel_selected Then
        'Exit Sub
        'End If
        'End If
        bc_am_in_context.loaded = False
        Me.Hide()
    End Sub



    Private Sub MenuItem3_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Cursor = Cursors.WaitCursor
        upload()
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub MenuItem5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Cursor = Cursors.WaitCursor
        discard()
        Me.Cursor = Cursors.Default
    End Sub
    Private Sub mtnew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub
    Public Sub del_tmp()
        Try
            Dim omsg As bc_cs_message
            Dim i As Integer
            If Me.ttemplatenew.Visible = False Then
                omsg = New bc_cs_message("Blue Curve", "Are you sure you want to remove template and definition for " + Me.ctemplate.Text, bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
            Else
                omsg = New bc_cs_message("Blue Curve", "Are you sure you want to remove template and definition for " + Me.ttemplatenew.Text, bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
            End If
            If omsg.cancel_selected = True Then
                Exit Sub
            Else
                Me.Cursor = Cursors.WaitCursor

                If Me.ttemplatenew.Visible = False Then
                    Me.set_status("Deleting template definition for: " + Me.ctemplate.Text)
                Else
                    Me.set_status("Deleting template definition for: " + Me.ttemplatenew.Text)
                End If

                If Me.ttemplatenew.Visible = False Then
                    Dim osheetconfig As New bc_om_insight_template_config
                    osheetconfig.logical_template_id = asheet.logical_templates_ids(Me.ctemplate.SelectedIndex)
                    osheetconfig.logical_template_name = "DELETE"
                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        osheetconfig.db_read()
                    Else
                        osheetconfig.tmode = bc_cs_soap_base_class.tREAD
                        osheetconfig.no_send_back = True
                        If osheetconfig.transmit_to_server_and_receive(osheetconfig, True) = False Then
                            Exit Sub
                        End If
                    End If
                    REM remove from template list
                    asheet.logical_templates_ids.RemoveAt(Me.ctemplate.SelectedIndex)
                    asheet.logical_templates_names.RemoveAt(Me.ctemplate.SelectedIndex)
                    Me.ctemplate.Items.Clear()
                    For i = 0 To asheet.logical_templates_names.Count - 1
                        Me.ctemplate.Items.Add(asheet.logical_templates_names(i))
                    Next
                Else
                    Me.ttemplatenew.Visible = False
                    Me.ctemplate.Visible = True
                End If
                Me.lintsheet.Text = ""
                clear_status()
                reset_template_controls()
                tk_main.set_template("")
                omsg = New bc_cs_message("Blue Curve", "Template successfully deleted", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)

                Me.Lselsec.Items.Clear()
                Me.litems.Items.Clear()

            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_in_context_Load", "mtdel", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub
    Private Sub new_sect()
        Try
            Dim found As Boolean
            Dim i, j As Integer
            Dim ofinput As New bc_am_in_tk_da_frm
            ofinput.Text = "Add New Section"
            ofinput.Height = 152
            ofinput.rsecnew.Visible = True
            ofinput.rsecexist.Visible = True
            ofinput.tinput.Visible = True
            ofinput.clist.Visible = False
            ofinput.rsecnew.Checked = True
            ofinput.Cstatic.Visible = True
            ofinput.Cstatic.Checked = False
            ofinput.Ltitle.Text = "Please enter or select new section name"
            ofinput.clist.Items.Clear()
            REM only add if not is use
            For i = 0 To bc_am_in_context.insight_items.sections.Count - 1
                found = False
                For j = 0 To osheet.bc_om_insightsections.Count - 1
                    If bc_am_in_context.insight_items.sections(i) = osheet.bc_om_insightsections(j).name Then
                        found = True
                        Exit For
                    End If
                Next
                If found = False Then
                    For j = 0 To osheet.bc_om_insightsections_static.Count - 1
                        If bc_am_in_context.insight_items.sections(i) = osheet.bc_om_insightsections_static(j).name Then
                            found = True
                            Exit For
                        End If
                    Next
                End If
                If found = False Then
                    ofinput.clist.Items.Add(bc_am_in_context.insight_items.sections(i))
                End If
            Next
            ofinput.ShowDialog()
            ofinput.rsecnew.Visible = False
            ofinput.rsecexist.Visible = False
            ofinput.Cstatic.Visible = False
            If ofinput.cancel_selected = True Then
                Exit Sub
            End If
            Me.uxItemToolbar.Buttons(0).Enabled = False
            If ofinput.rsecnew.Checked = False Then
                If Trim(ofinput.clist.Text) = "" Then
                    Exit Sub
                End If
                Dim nsection As New bc_om_insight_section
                nsection.name = ofinput.clist.Text
                If ofinput.Cstatic.Checked = False Then
                    osheet.bc_om_insightsections.Add(nsection)
                Else
                    osheet.bc_om_insightsections_static.Add(nsection)
                End If
                Me.load_sections()
            Else
                Dim nsection As New bc_om_insight_section
                REM section name uis new
                If Trim(ofinput.tinput.Text) = "" Then
                    Exit Sub
                End If
                nsection.name = ofinput.tinput.Text
                If nsection.name = "" Then
                    Exit Sub
                End If
                For i = 0 To bc_am_in_context.insight_items.sections.Count - 1
                    If UCase(bc_am_in_context.insight_items.sections(i)) = UCase(nsection.name) Then
                        Dim omsg As New bc_cs_message("Blue Curve", "Section already exists please use existing option.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                        Exit Sub
                    End If
                Next
                For i = 0 To Me.Lselsec.Items.Count - 1
                    If UCase(Me.Lselsec.Items(i)) = UCase(nsection.name) Or UCase(Me.Lselsec.Items(i)) = UCase(nsection.name) + " (STATIC)" Then
                        Dim omsg As New bc_cs_message("Blue Curve", "Section already exists in template.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                        Exit Sub
                    End If
                Next
                If ofinput.Cstatic.Checked = False Then
                    osheet.bc_om_insightsections.Add(nsection)
                Else
                    osheet.bc_om_insightsections_static.Add(nsection)
                End If
                Me.load_sections()

            End If
            If Me.Lselsec.SelectedIndex > -1 Then
                Me.Lselsec.SelectedIndex = -1
            End If
            Me.set_change()
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_in_context_Load", "bnewsec", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub
    Private Sub del_sec()
        Dim omsg As New bc_cs_message("Blue Curve", "Do you want to delete section: " + Me.Lselsec.Text, bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
        If omsg.cancel_selected = True Then
            Exit Sub
        End If
        Me.uxItemToolbar.Buttons(0).Enabled = False

        Dim i As Integer
        For i = 0 To osheet.bc_om_insightsections.Count - 1
            If osheet.bc_om_insightsections(i).name = Me.Lselsec.SelectedItem Then
                osheet.bc_om_insightsections.RemoveAt(i)
                Exit For
            End If
        Next
        For i = 0 To osheet.bc_om_insightsections_static.Count - 1
            If osheet.bc_om_insightsections_static(i).name + " (static)" = Me.Lselsec.SelectedItem Then
                osheet.bc_om_insightsections_static.RemoveAt(i)
                Exit For
            End If
        Next
        Me.uxSectionToolbar.Buttons(2).Enabled = False
        Me.litems.Items.Clear()
        Me.load_sections()
        Me.set_change()
    End Sub
    Public Sub load_sections_for_template(ByVal idx As Integer, ByVal ofinput As bc_am_in_tk_da_frm)
        Try
            Dim i As Integer
            ofinput.Clist2.Items.Clear()
            Me.Cursor = Cursors.WaitCursor
            csheet = New bc_om_insight_sheet(asheet.logical_templates_ids(idx))
            Me.set_status("retrieving template definition for: " + asheet.logical_templates_names(idx))

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                csheet.db_read()
            Else
                csheet.tmode = bc_cs_soap_base_class.tREAD
                If csheet.transmit_to_server_and_receive(csheet, True) = False Then
                    Exit Sub
                End If
            End If
            Me.clear_status()

            For i = 0 To csheet.bc_om_insightsections.Count - 1
                ofinput.Clist2.Items.Add(csheet.bc_om_insightsections(i).name)

            Next
            For i = 0 To csheet.bc_om_insightsections_static.Count - 1
                ofinput.Clist2.Items.Add(csheet.bc_om_insightsections_static(i).name + " (static)")
            Next
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_in_context", "load_section_for_template", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub


    Private Sub bsecup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bsecup.Click
        Dim tsec As New bc_om_insight_section
        Dim idx As Integer
        idx = Me.Lselsec.SelectedIndex
        If idx = -1 Then
            Exit Sub
        End If
        If Me.Lselsec.SelectedIndex < osheet.bc_om_insightsections.Count Then
            If idx = 0 Then
                Exit Sub
            End If
            tsec = osheet.bc_om_insightsections(Me.Lselsec.SelectedIndex - 1)
            osheet.bc_om_insightsections.RemoveAt(Me.Lselsec.SelectedIndex - 1)
            osheet.bc_om_insightsections.Insert(Me.Lselsec.SelectedIndex, tsec)
        Else
            If idx = osheet.bc_om_insightsections.Count Then
                Exit Sub
            End If
            tsec = osheet.bc_om_insightsections_static(Me.Lselsec.SelectedIndex - 1 - osheet.bc_om_insightsections.Count)
            osheet.bc_om_insightsections_static.RemoveAt(Me.Lselsec.SelectedIndex - 1 - osheet.bc_om_insightsections.Count)
            osheet.bc_om_insightsections_static.Insert(Me.Lselsec.SelectedIndex - osheet.bc_om_insightsections.Count, tsec)
        End If
        load_sections()
        Me.Lselsec.SelectedIndex = idx - 1
        Me.set_change()
    End Sub

    Private Sub bsecdn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bsecdn.Click
        Dim tsec As New bc_om_insight_section
        Dim idx As Integer
        idx = Me.Lselsec.SelectedIndex
        If idx = -1 Then
            Exit Sub
        End If
        If Me.Lselsec.SelectedIndex < osheet.bc_om_insightsections.Count Then
            If idx = osheet.bc_om_insightsections.Count - 1 Then
                Exit Sub
            End If
            tsec = osheet.bc_om_insightsections(Me.Lselsec.SelectedIndex + 1)
            osheet.bc_om_insightsections.RemoveAt(Me.Lselsec.SelectedIndex + 1)
            osheet.bc_om_insightsections.Insert(Me.Lselsec.SelectedIndex, tsec)
        Else
            If idx = osheet.bc_om_insightsections.Count + osheet.bc_om_insightsections_static.Count - 1 Then
                Exit Sub
            End If
            tsec = osheet.bc_om_insightsections_static(Me.Lselsec.SelectedIndex + 1 - osheet.bc_om_insightsections.Count)
            osheet.bc_om_insightsections_static.RemoveAt(Me.Lselsec.SelectedIndex + 1 - osheet.bc_om_insightsections.Count)
            osheet.bc_om_insightsections_static.Insert(Me.Lselsec.SelectedIndex - osheet.bc_om_insightsections.Count, tsec)
        End If
        load_sections()
        Me.Lselsec.SelectedIndex = idx + 1
        Me.set_change()
    End Sub

    Private Sub About_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim omsg As New bc_cs_message("Blue Curve", "", bc_cs_message.MESSAGE, False, True)
    End Sub

    Private Sub MenuItem6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.ofcs.Hide()
        Me.uxTemplates.Enabled = True
        Me.uxSections.Enabled = True
        Me.uxItems.Enabled = True
        Me.Panel1.Visible = True
    End Sub

    Private Sub MenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.uxTemplates.Enabled = False
        Me.uxSections.Enabled = False
        Me.uxItems.Enabled = False
        ocont.load_contributing_classes()

    End Sub
    Private Sub ccp_SelectedIndexChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.selected_keyword = "Parent Class"
    End Sub

    Private Function set_new_template(ByVal tx As String) As Boolean
        Dim i As Integer
        set_new_template = False
        If Trim(tx) = "" Then
            Exit Function
        End If

        'If change = True Then
        '    Dim omsg As New bc_cs_message("Blue Curve", "Changes have occured to previously selected template continuing will discard these changes Proceed without comiting changes first?", bc_cs_message.MESSAGE, True)
        '    If omsg.cancel_selected Then
        '        Exit Function
        '    End If
        'End If

        For i = 0 To asheet.logical_templates_names.Count - 1
            If tx.ToUpper = CStr(asheet.logical_templates_names(i).toupper) Then
                Dim omsg As New bc_cs_message("Blue Curve", "Template Name: " + tx + " already exists!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Function
            End If
        Next

        REM first check template doesnt already exist
        Me.Lselsec.Items.Clear()
        Me.litems.Items.Clear()
        Me.Lselsec.Enabled = True

        osheet = New bc_om_insight_sheet
        osheet.logical_template_id = 0
        Me.ttemplatenew.Text = tx
        osheet.logical_template_name = tx
        Me.ttemplatenew.Enabled = False
        Me.uxSections.Enabled = True
        Me.ctemplate.Visible = False
        Me.ttemplatenew.Visible = True
        Me.lintsheet.Text = ""
        Me.set_change()
        set_new_template = True

    End Function
    Private Sub set_change()

        Me.tk_main.set_change(0)
        Me.change = True
    End Sub

    Private Sub bcannew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.ctemplate.Visible = True
        Me.ttemplatenew.Visible = False
        Me.ttemplatenew.Text = ""
    End Sub
    Private Sub MenuItem6_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Panel1.Visible = False
        Me.uxTemplates.Enabled = False
        Me.uxSections.Enabled = False
        Me.uxItems.Enabled = False
        Me.oret.Hide()
        ofcs.Width = Me.Width - 15
        ofcs.Anchor = AnchorStyles.Top + AnchorStyles.Left + AnchorStyles.Right + AnchorStyles.Bottom
        ofcs.Height = Me.Height - 106
        ofcs.TopMost = True
        ofcs.Show()
    End Sub


    Private Sub Template_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.uxTemplates.Enabled = True
        If Me.ctemplate.SelectedIndex > -1 Or Me.ttemplatenew.Text <> "" Then
            Me.uxSections.Enabled = True
            If Me.Lselsec.SelectedIndex > -1 Then
                Me.uxItems.Enabled = True
            End If
        End If
        Me.Panel1.Visible = True
        Try
            Me.ofcs.Hide()
        Catch
        End Try
    End Sub

    Private Sub Contribution_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            Me.ofcs.Hide()
        Catch
        End Try
        Me.uxTemplates.Enabled = False
        Me.uxSections.Enabled = False
        Me.uxItems.Enabled = False
        Me.Panel1.Visible = False
        Me.uxTemplates.Enabled = False
        Me.uxSections.Enabled = False
        Me.uxItems.Enabled = False
        Me.oret.Hide()
        Me.ofcs.Hide()
        ocont.Width = Me.Width - 15
        ocont.Anchor = AnchorStyles.Top + AnchorStyles.Left + AnchorStyles.Right + AnchorStyles.Bottom
        ocont.Height = Me.Height - 106
        ocont.TopMost = True
        ocont.Show()

    End Sub

    Private Sub Panel3_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs)

    End Sub
    Public ptx As String
    Public Sub set_status(ByVal tx As String)
        Me.tk_main.set_status(tx)
    End Sub

    Public Sub clear_status()
        Me.tk_main.clear_status()
    End Sub

    Private Sub mup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Cursor = Cursors.WaitCursor
        upload()
        Me.Cursor = Cursors.Default
    End Sub
    Private Sub mdn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Cursor = Cursors.WaitCursor
        discard()
        'Me.Calculations.Enabled = False
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub MenuItem3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        upload_fcs()
    End Sub

    Public Sub upload_fcs(Optional ByVal show_message As Boolean = True)
        Me.Cursor = Cursors.WaitCursor
        Me.ofcs.create_xml_file(show_message)
        Me.Cursor = Cursors.Default
        clear_change("Client Settings")
    End Sub
    Private Sub MenuItem5_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Panel1.Visible = False
        Me.uxTemplates.Enabled = False
        Me.uxSections.Enabled = False
        Me.uxItems.Enabled = False
        Me.ofcs.Hide()
        oret.Width = Me.Width - 15
        oret.Anchor = AnchorStyles.Top + AnchorStyles.Left + AnchorStyles.Right + AnchorStyles.Bottom
        oret.Height = Me.Height - 106
        Dim lo As New System.Drawing.Point(1, 1)
        oret.Location = lo
        oret.TopMost = True
        oret.Show()
    End Sub



    Public Sub set_change(ByVal tx As String)
        Dim stx As String = ""
        'stx = Me.luploadstatus.Text
        If InStr(stx, tx) > 0 Then
            Exit Sub
        End If
        stx = stx.Replace("Pending Upload:", "")
        If stx = "" Then
            stx = "Pending Upload: " + tx
        Else
            stx = "Pending Upload: " + stx + "; " + tx
        End If

    End Sub
    Public Sub clear_change(ByVal tx As String)
        Exit Sub

        'stx = Me.luploadstatus.Text
        'If stx.Length > 0 Then
        '    stx = stx.Replace(tx + "; ", "")
        '    stx = stx.Replace(tx, "")
        '    REM remove spurioes ; at end
        '    If stx.Substring(stx.Length - 2) = "; " Then
        '        stx = stx.Substring(0, stx.Length - 2)
        '    End If
        '    If RTrim(stx.Length) < 20 Then
        '        'Me.luploadstatus.Text = ""
        '        'Me.mupall.Enabled = False
        '        'Me.mdiscall.Enabled = False
        '    Else
        '        'Me.luploadstatus.Text = stx
        '    End If
        'End If

    End Sub


    Private Sub MenuItem11_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim oinsight3 As New bc_am_insight_parameters(False)
        If oinsight3.load_failed = False Then
            Dim k As New bc_am_insight_formats()
            REM set up dialogue with these settings
            Me.oret.load_start()
        End If

        clear_change("Ad-hoc Data Retrieval")
    End Sub

    'Private Sub mcsdiscard_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Dim oinsight3 As New bc_am_insight_parameters(False)
    '    If oinsight3.load_failed = False Then
    '        bc_am_insight_formats
    '        REM set up dialogue with these settings
    '        Me.ofcs.load_settings()

    '    clear_change("Client Settings")
    'End Sub


    Private Sub madisc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Cursor = Cursors.WaitCursor
        Me.set_status("Discarding template assignment...")
        Me.Menu.MenuItems(2).MenuItems(0).Enabled = False
        Me.Menu.MenuItems(2).MenuItems(1).Enabled = False
        insight_items = New bc_om_insight_items
        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
            insight_items.db_read()
        Else
            insight_items.tmode = bc_cs_soap_base_class.tREAD
            If insight_items.transmit_to_server_and_receive(insight_items, True) = False Then
                Exit Sub
            End If
        End If
        ocont.load_contributing_classes()
        ocont.uxTemplateAssignment.Enabled = False
        Me.clear_status()
        clear_change("Assignment")
        Me.Cursor = Cursors.Default

    End Sub
    Public Sub load_existing_items(ByVal fitem As bc_am_in_tk_item)
        Try
            Dim i, j, k As Integer
            fitem.Cexist.Items.Clear()
            bc_am_in_context.filter_insight_items.insight_items.Clear()
            Dim found As Boolean = False
            For i = 0 To insight_items.insight_items.Count - 1
                If Me.Lselsec.SelectedIndex < osheet.bc_om_insightsections.Count Then
                    If insight_items.insight_items(i).row_flag = 1 Then
                        REM check doesnt already appear in any section
                        For j = 0 To osheet.bc_om_insightsections.Count - 1
                            For k = 0 To osheet.bc_om_insightsections(j).rows.count - 1
                                If osheet.bc_om_insightsections(j).rows(k).row_id = insight_items.insight_items(i).item_id Then
                                    found = True
                                    Exit For
                                End If
                                If found = True Then
                                    Exit For
                                End If
                            Next

                        Next
                        If found = False Then
                            bc_am_in_context.filter_insight_items.insight_items.Add(insight_items.insight_items(i))
                            fitem.Cexist.Items.Add(insight_items.insight_items(i).desc)
                        End If
                        found = False
                    End If
                Else
                    If insight_items.insight_items(i).row_flag = 0 Then
                        For j = 0 To osheet.bc_om_insightsections_static.Count - 1
                            For k = 0 To osheet.bc_om_insightsections_static(j).rows.count - 1
                                If osheet.bc_om_insightsections_static(j).rows(k).row_id = insight_items.insight_items(i).item_id Then
                                    found = True
                                    Exit For
                                End If
                                If found = True Then
                                    Exit For
                                End If
                            Next

                        Next
                        If found = False Then
                            bc_am_in_context.filter_insight_items.insight_items.Add(insight_items.insight_items(i))
                            fitem.Cexist.Items.Add(insight_items.insight_items(i).desc)
                        End If
                        found = False
                    End If
                End If
            Next
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_in_context_Load", "Rexist_CheckedChanged_1", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

            Me.Cursor = Cursors.Default
        End Try

    End Sub



    'Private Sub mupall_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Dim omsg As New bc_cs_message("Blue Curve", "Are you sure you wish to commit all changes?", bc_cs_message.MESSAGE, True, False, "Yes", "no")
    '    If omsg.cancel_selected = False Then
    '        Me.Cursor = Cursors.WaitCursor
    '        If Me.c Then
    '            upload(False)
    '            clear_change()
    '            clear_change("Template")
    '            Me.Calculations.Enabled = False
    '            Me.bpreview.Enabled = False
    '        End If
    '        If Me.maup.Enabled = True Then
    '            ocont.upload_cont(False)
    '        End If
    '        If Me.mcsupload.Enabled = True Then
    '            Me.upload_fcs(False)
    '        End If
    '        If Me.muploadret.Enabled = True Then
    '            Me.upload_ret(False)
    '        End If
    '        Me.mupall.Enabled = False
    '        Me.mdiscall.Enabled = False
    '        Me.Cursor = Cursors.Default
    '    End If
    'End Sub
    'Private Sub mdiscall_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mdiscall.Click
    '    Dim omsg As New bc_cs_message("Blue Curve", "Are you sure you wish to discard all changes and revert back to last saved settings?", bc_cs_message.MESSAGE, True, False, "Yes", "no")
    '    If omsg.cancel_selected = False Then
    '        Me.Cursor = Cursors.WaitCursor
    '        load_start()
    '        ocont.load_contributing_classes()
    '        ocont.Lcclass.SelectedItems(0).Selected = False
    '        ocont.passtemp.Visible = False
    '        Me.bpreview.Enabled = False
    '        Me.ofcs.load_settings()
    '        Me.oret.load_start()
    '        REM load current config settings if file exists
    '        load_client_settings()
    '        'Me.luploadstatus.Text = ""
    '        Me.mupall.Enabled = False
    '        Me.mdiscall.Enabled = False
    '        Me.Cursor = Cursors.Default
    '    End If
    'End Sub

    Private Sub mpreview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub
    Public Function preview() As Boolean
        Dim aexcel As bc_ao_in_excel = Nothing
        Dim xl As Object = Nothing

        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try
            preview = True
            REM previews current in memory settings
            Dim omsg As bc_cs_message
            Dim insheet As New bc_om_intermediate_sheet
            Dim fs As New bc_cs_file_transfer_services

            Dim i As Integer
            REM first check format sheet isnt open
            If bc_am_insight_formats.format_filename <> "" Then
                If fs.file_already_open(bc_cs_central_settings.local_template_path + bc_am_insight_formats.format_filename) Then
                    omsg = New bc_cs_message("Blue Curve", "Format sheet: " + bc_am_insight_formats.format_filename + " is open please close from Excel prior to preview", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    preview = False
                    Exit Function
                End If
            End If
            REM get intermediate file if doesnt exist locally and template has one
            If Me.lintsheet.Text <> "" Then
                If Me.ctemplate.SelectedIndex > -1 And Me.ctemplate.Visible = True Then
                    For i = 0 To bc_am_in_context.insight_items.intermediate_sheets.insert_sheets.Count - 1
                        If bc_am_in_context.insight_items.intermediate_sheets.insert_sheets(i).id = asheet.logical_templates_ids(Me.ctemplate.SelectedIndex) Then
                            insheet.file_name = bc_am_in_context.insight_items.intermediate_sheets.insert_sheets(i).file_name
                            insheet.name = bc_am_in_context.insight_items.intermediate_sheets.insert_sheets(i).name
                            Exit For
                        End If
                    Next
                Else
                    insheet.file_name = Me.lintsheet.Text.Substring(20, Me.lintsheet.Text.Length - 20)
                End If
                REM if intermediate sheet is open flag this and stop
                If fs.file_already_open(bc_cs_central_settings.local_template_path + insheet.file_name) = True Then
                    omsg = New bc_cs_message("Blue Curve", "Intermediate sheet: " + insheet.file_name + " is open please close from Excel prior to preview", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    preview = False
                    Exit Function
                End If
                REM if exists locally open this
                If fs.check_document_exists(bc_cs_central_settings.local_template_path + insheet.file_name) = False Or Me.tk_main.template_changed = False Then
                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        insheet.db_read()
                    Else
                        insheet.tmode = bc_cs_soap_base_class.tREAD
                        If insheet.transmit_to_server_and_receive(insheet, True) = False Then
                            Exit Function
                        End If
                    End If
                    fs.write_bytestream_to_document(bc_cs_central_settings.local_template_path + insheet.file_name, insheet.sheet, Nothing)
                End If
            End If

            Me.set_status("Generating preview in Excel...")
            Try
                xl = GetObject(, "excel.application")
            Catch
                Try
                    xl = CreateObject("excel.application")
                    xl.application.visible = False
                Catch
                    omsg = New bc_cs_message("Blue Curve", "Cannot invoke Excel", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Function
                End Try
            End Try
            If xl.application.workbooks.count = 0 Then
                xl.application.workbooks.add()
            End If
            aexcel = New bc_ao_in_excel(xl.application.workbooks(1))
            aexcel.screen_updating(True)
            aexcel.disable_application_alerts()

            save_settings_temp()
            If Me.ctemplate.SelectedIndex > -1 Then
                aexcel.delete_preview_sheet("Preview BCSHEET " + asheet.logical_templates_names(Me.ctemplate.SelectedIndex))
            End If

            If Me.lintsheet.Text <> "" Then
                REM insert preview sheet is exists
                aexcel.delete_int_sheet(insheet.name)
                If aexcel.insert_sheet(insheet.name, bc_cs_central_settings.local_template_path + insheet.file_name, True) = False Then

                End If
            End If

            REM test activeworkbook is still present
            If Me.ttemplatenew.Visible = True Then
                aexcel.build_workbook_preview(Me.ttemplatenew.Text, "new", osheet)
            Else
                aexcel.build_workbook_preview(Me.ctemplate.Text, asheet.logical_templates_ids(Me.ctemplate.SelectedIndex), osheet)
            End If
            REM now run linker as well
            aexcel.link_workbook(True)
            switch_back_Settings()
            omsg = New bc_cs_message("Blue Curve", "Preview Complete please check in Excel", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_in_context_Load", "preview", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

            Try
                If preview = True Then
                    xl.application.visible = True
                    aexcel.screen_updating(False)
                End If
            Catch ex As Exception
                Dim oerr As New bc_cs_error_log("bc_am_in_context_Load", "preview second", bc_cs_error_codes.USER_DEFINED, ex.Message)
            Finally
                System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            End Try
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            Me.clear_status()
        End Try
    End Function
    Private Sub save_settings_temp()
        Try
            REM if settings in memeory are not commited use this in previewer by temporalily
            REM switching over
            REM settings
            Dim fs As New bc_cs_file_transfer_services
            If Me.tk_main.settings_changed = True Or Me.tk_main.retrieval_changed = True Then
                REM copy over current config file
                If fs.check_document_exists(bc_cs_central_settings.local_template_path + "bc_am_insight_config.xml") = True Then
                    fs.file_copy(bc_cs_central_settings.local_template_path + "bc_am_insight_config.xml", bc_cs_central_settings.local_template_path + "server_backup_bc_am_insight_config.xml")
                End If
                REM now save current memory settings
                tk_main.ofcs.create_xml_file(False, False)
                tk_main.oret.create_xml_file(False, False)
                REM read these into memory
                Dim oinsight3 As New bc_am_insight_parameters(False)
                If oinsight3.load_failed = False Then
                    Dim k As New bc_am_insight_formats
                End If
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_in_context_Load", "save_settings_temp", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try
    End Sub
    Private Sub switch_back_Settings()
        Dim fs As New bc_cs_file_transfer_services
        If fs.check_document_exists(bc_cs_central_settings.local_template_path + "server_backup_bc_am_insight_config.xml") = True Then
            fs.file_copy(bc_cs_central_settings.local_template_path + "server_backup_bc_am_insight_config.xml", bc_cs_central_settings.local_template_path + "bc_am_insight_config.xml")
            fs.delete_file(bc_cs_central_settings.local_template_path + "server_backup_bc_am_insight_config.xml")
        End If
    End Sub



    Public Sub set_section(ByVal idx As Integer)
        If idx < csheet.bc_om_insightsections.Count Then
            osheet.bc_om_insightsections.Add(csheet.bc_om_insightsections(idx))
        Else
            osheet.bc_om_insightsections_static.Add(csheet.bc_om_insightsections_static(idx - csheet.bc_om_insightsections.Count))
        End If
        Me.load_sections()

    End Sub
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        del_tmp()
    End Sub
    Public Sub clone_template()
        Dim i As Integer
        Dim finput As New bc_am_in_tk_da_frm
        finput.Text = "Clone Template"
        finput.Height = 125
        finput.Ltitle.Text = "Select template to clone from"
        finput.clist.Items.Clear()
        For i = 0 To bc_am_in_context.asheet.logical_templates_ids.Count - 1
            finput.clist.Items.Add(asheet.logical_templates_names(i))
        Next
        finput.clist.Visible = True
        finput.ShowDialog()
        If finput.cancel_selected = True Then
            Exit Sub
        End If
        If finput.clist.SelectedIndex = -1 Then
            Exit Sub
        End If
        If finput.clist.Text = Me.ctemplate.Text And Me.ctemplate.Visible = True Then
            Dim omsg As New bc_cs_message("Blue Curve", "Template cannot be cloned from itself. If you want last saved copy please discard this copy", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
        Else
            Me.copy_temp(finput.clist.Text)
        End If

    End Sub
    Public Sub clone_section()
        Dim finput As New bc_am_in_tk_da_frm
        finput.fcontext = Me
        Dim i As Integer
        finput.Height = 152
        finput.Text = "Clone Section"
        finput.Ltitle.Text = "Select template and section to clone from"
        finput.L1.Text = "Template"
        finput.L2.Text = "Section"
        finput.L1.Visible = True
        finput.L2.Visible = True
        finput.clist.Visible = True
        finput.Clist2.Visible = True
        finput.clist.Items.Clear()
        finput.Clist2.Items.Clear()
        For i = 0 To asheet.logical_templates_names.Count - 1
            finput.clist.Items.Add(asheet.logical_templates_names(i))
        Next
        finput.ShowDialog()
        finput.L1.Visible = False
        finput.L2.Visible = False
        finput.clist.Visible = False
        finput.Clist2.Visible = False
        If finput.Clist2.SelectedIndex > -1 Then
            REM check section doesnt alreay appear
            For i = 0 To Me.Lselsec.Items.Count - 1
                If Me.Lselsec.Items(i) = finput.Clist2.Text Or Me.Lselsec.Items(i) = finput.Clist2.Text + " (static)" Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Section already exists in template", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub
                End If
            Next
            Me.set_section(finput.Clist2.SelectedIndex)
            Me.set_change()
        End If


    End Sub
    Public Sub new_template()
        Dim finput As New bc_am_in_tk_da_frm
        finput.Height = 125
        finput.Text = "New Template"
        finput.Ltitle.Text = "Enter new template name"
        finput.tinput.Visible = True
        finput.ShowDialog()
        If finput.cancel_selected = True Then
            Exit Sub
        End If
        tk_main.set_button_disabled(1)
        If set_new_template(finput.tinput.Text) = True Then
            tk_main.set_template_name("Selected template: " + finput.tinput.Text)
            set_template_selected()
            Me.tk_main.set_button_disabled(0)
        Else
            new_template()
        End If
        Me.uxItemToolbar.Buttons(0).Enabled = False
        Me.uxItemToolbar.Buttons(1).Enabled = False
        Me.uxItemToolbar.Buttons(2).Enabled = False
        Me.uxSectionToolbar.Buttons(0).Enabled = True
        Me.uxSectionToolbar.Buttons(1).Enabled = True
        Me.uxSectionToolbar.Buttons(2).Enabled = False
    End Sub
    Private Sub beditsheet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim oldCI As System.Globalization.CultureInfo = _
                               System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try
            Me.Cursor = Cursors.WaitCursor

            REM download sheet from  server
            Dim osheet As New bc_om_intermediate_sheet
            Dim fs As New bc_cs_file_transfer_services

            Dim xl As Object
            Dim i As Integer
            If Me.ctemplate.Visible = True Then
                For i = 0 To bc_am_in_context.insight_items.intermediate_sheets.insert_sheets.Count - 1
                    If bc_am_in_context.insight_items.intermediate_sheets.insert_sheets(i).id = asheet.logical_templates_ids(Me.ctemplate.SelectedIndex) Then
                        osheet.file_name = bc_am_in_context.insight_items.intermediate_sheets.insert_sheets(i).file_name
                        Exit For
                    End If
                Next
            Else
                For i = 0 To bc_am_in_context.insight_items.intermediate_sheets.insert_sheets.Count - 1
                    If bc_am_in_context.insight_items.intermediate_sheets.insert_sheets(i).id = 0 Then
                        osheet.file_name = bc_am_in_context.insight_items.intermediate_sheets.insert_sheets(i).file_name
                        Exit For
                    End If
                Next
            End If

            REM if exists locally open this
            If fs.check_document_exists(bc_cs_central_settings.local_template_path + osheet.file_name) = False Or Me.tk_main.template_changed = False Then
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    osheet.db_read()
                Else
                    osheet.tmode = bc_cs_soap_base_class.tREAD
                    If osheet.transmit_to_server_and_receive(osheet, True) = False Then
                        Exit Sub
                    End If
                End If
                fs.write_bytestream_to_document(bc_cs_central_settings.local_template_path + osheet.file_name, osheet.sheet, Nothing)
            End If
            REM save the file down locally

            REM open it 
            Dim omsg As bc_cs_message
            Try
                xl = GetObject(, "excel.application")
            Catch
                Try
                    xl = CreateObject("excel.application")
                Catch
                    omsg = New bc_cs_message("Blue Curve", "Cannot invoke Excel", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub
                End Try
            End Try
            If xl.application.workbooks.count = 0 Then
                xl.application.workbooks.add()
            End If
            Dim excel_app As New bc_ao_in_excel(xl.application)
            excel_app.open(bc_cs_central_settings.local_template_path + osheet.file_name)
            xl.visible = True
            Me.set_change()
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("beditsheet", "click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            Me.Cursor = Cursors.Default

        End Try

    End Sub
    Public Sub import_sheet()
        Dim fs As New bc_cs_file_transfer_services
        Dim i As Integer
        Dim odialog As New OpenFileDialog
        Dim fn As String
        Dim found As Boolean = False

        odialog.Title = "Select intermediate sheet"

        REM SW cope with office versions
        If bc_cs_central_settings.userOfficeStatus = 2 Then
            odialog.Filter = "Excel files (*.xls)|*.xls|(*.xlsx)|*.xlsx"
        Else
            odialog.Filter = "Excel files (*.xls)|*.xls"
        End If
        'odialog.Filter = "Excel files (*.xls)|*.xls"

        odialog.ShowDialog()
        If odialog.FileName <> "" Then
            fn = odialog.FileName.Substring(InStrRev(odialog.FileName, "\"))

            REM check file isnt alrady open
            If fs.file_already_open(bc_cs_central_settings.local_template_path + fn) = True Then
                Dim omsg As New bc_cs_message("Blue Curve", "File: " + bc_cs_central_settings.local_template_path + fn + " is open please close prior to importing!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Sub
            End If
            If fs.file_already_open(odialog.FileName) = True Then
                Dim omsg As New bc_cs_message("Blue Curve", "File: " + odialog.FileName + " is open please close prior to importing!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Sub
            End If
            Me.lintsheet.Text = "Intermediate sheet: " + fn
            REM copy over file
            fs.file_copy(odialog.FileName, bc_cs_central_settings.local_template_path + fn)

            For i = 0 To bc_am_in_context.insight_items.intermediate_sheets.insert_sheets.Count - 1
                If Me.ttemplatenew.Visible = False Then
                    If bc_am_in_context.insight_items.intermediate_sheets.insert_sheets(i).id = asheet.logical_templates_ids(Me.ctemplate.SelectedIndex) Then
                        bc_am_in_context.insight_items.intermediate_sheets.insert_sheets(i).file_name = fn
                        found = True
                        Exit For
                    End If
                End If
            Next
            If found = False Then
                Dim osheet As New bc_om_intermediate_sheet
                osheet.file_name = fn

                If Me.ttemplatenew.Visible = False Then
                    osheet.id = asheet.logical_templates_ids(Me.ctemplate.SelectedIndex)
                Else
                    osheet.id = 0
                End If
                bc_am_in_context.insight_items.intermediate_sheets.insert_sheets.Add(osheet)
            End If
            REM now set this template
            set_template_selected()
            Me.set_change()
        End If
    End Sub
    Public Sub edit_sheet()

        Dim oldCI As System.Globalization.CultureInfo = _
                               System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try
            Me.Cursor = Cursors.WaitCursor

            REM download sheet from  server
            Dim osheet As New bc_om_intermediate_sheet
            Dim fs As New bc_cs_file_transfer_services

            Dim xl As Object
            Dim i As Integer
            If Me.ctemplate.Visible = True Then
                For i = 0 To bc_am_in_context.insight_items.intermediate_sheets.insert_sheets.Count - 1
                    If bc_am_in_context.insight_items.intermediate_sheets.insert_sheets(i).id = asheet.logical_templates_ids(Me.ctemplate.SelectedIndex) Then
                        osheet.file_name = bc_am_in_context.insight_items.intermediate_sheets.insert_sheets(i).file_name
                        Exit For
                    End If
                Next
            Else
                For i = 0 To bc_am_in_context.insight_items.intermediate_sheets.insert_sheets.Count - 1
                    If bc_am_in_context.insight_items.intermediate_sheets.insert_sheets(i).id = 0 Then
                        osheet.file_name = bc_am_in_context.insight_items.intermediate_sheets.insert_sheets(i).file_name
                        Exit For
                    End If
                Next
            End If

            REM if exists locally open this
            If fs.check_document_exists(bc_cs_central_settings.local_template_path + osheet.file_name) = False Or Me.tk_main.template_changed = False Then
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    osheet.db_read()
                Else
                    osheet.tmode = bc_cs_soap_base_class.tREAD
                    If osheet.transmit_to_server_and_receive(osheet, True) = False Then
                        Exit Sub
                    End If
                End If
                fs.write_bytestream_to_document(bc_cs_central_settings.local_template_path + osheet.file_name, osheet.sheet, Nothing)
            End If
            REM save the file down locally

            REM open it 
            Dim omsg As bc_cs_message
            Try
                xl = GetObject(, "excel.application")
            Catch
                Try
                    xl = CreateObject("excel.application")
                Catch
                    omsg = New bc_cs_message("Blue Curve", "Cannot invoke Excel", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub
                End Try
            End Try
            If xl.application.workbooks.count = 0 Then
                xl.application.workbooks.add()
            End If
            Dim excel_app As New bc_ao_in_excel(xl.application)
            excel_app.open(bc_cs_central_settings.local_template_path + osheet.file_name)
            xl.visible = True
            set_template_selected()
            Me.set_change()
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("beditsheet", "click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            Me.Cursor = Cursors.Default

        End Try

    End Sub
    Public Sub delete_sheet()
        REM delete sheet
        Dim i As Integer
        Dim omsg As New bc_cs_message("Blue Curve", "Are you sure you want to delete intermediate sheet?", bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
        If omsg.cancel_selected = True Then
            Exit Sub
        End If
        For i = 0 To bc_am_in_context.insight_items.intermediate_sheets.insert_sheets.Count - 1
            If Me.ctemplate.Visible = True And Me.ctemplate.SelectedIndex > -1 Then
                If bc_am_in_context.insight_items.intermediate_sheets.insert_sheets(i).id = asheet.logical_templates_ids(Me.ctemplate.SelectedIndex) Then
                    bc_am_in_context.insight_items.intermediate_sheets.insert_sheets.RemoveAt(i)
                    Exit For
                End If
            Else
                If bc_am_in_context.insight_items.intermediate_sheets.insert_sheets(i).id = 0 Then
                    bc_am_in_context.insight_items.intermediate_sheets.insert_sheets.RemoveAt(i)
                    Exit For
                End If
            End If
        Next
        Me.lintsheet.Text = ""
        set_template_selected()
        Me.set_change()
    End Sub

    Private Sub bc_am_in_context_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim newindex As Integer
        With Me.uxSectionToolbar.Buttons()
            newindex = .Add("&New")
            .Item(newindex).ImageIndex = 0
            .Item(newindex).Enabled = False
            .Item(newindex).ToolTipText = "Assign A new section to template"
            newindex = .Add("&Clone")
            .Item(newindex).ImageIndex = 4
            .Item(newindex).Enabled = False
            .Item(newindex).ToolTipText = "Clone an existing section and assign to template"
            newindex = .Add("&Delete")
            .Item(newindex).ImageIndex = 3
            .Item(newindex).ToolTipText = "Delete section from template"
            .Item(newindex).Enabled = False
        End With
        With Me.uxItemToolbar.Buttons()
            newindex = .Add("&New")
            .Item(newindex).ImageIndex = 0
            .Item(newindex).Enabled = False
            .Item(newindex).ToolTipText = "Assign A new section to template"
            newindex = .Add("&Edit")
            .Item(newindex).ImageIndex = 7
            .Item(newindex).Enabled = False
            .Item(newindex).ToolTipText = "Edit an existing item"
            newindex = .Add("&Delete")
            .Item(newindex).ImageIndex = 3
            .Item(newindex).ToolTipText = "Delete section from template"
            .Item(newindex).Enabled = False
        End With

    End Sub
    Private Sub uxsectiontoolbar_ButtonClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles uxSectionToolbar.ButtonClick
        If e.Button.Text = "&Delete" Then
            Me.del_sec()
        ElseIf e.Button.Text = "&New" Then
            Me.new_sect()
        ElseIf e.Button.Text = "&Clone" Then
            Me.clone_section()
        End If
    End Sub

    Private Sub uxitemtoolbar_ButtonClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles uxItemToolbar.ButtonClick
        If e.Button.Text = "&Delete" Then
            Me.del_item()
        ElseIf e.Button.Text = "&New" Then
            Me.load_item_attributes(False)
        ElseIf e.Button.Text = "&Edit" Then
            Me.load_item_attributes(True)
        End If
    End Sub

    Private Sub litems_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles litems.DoubleClick
        If Not litems.SelectedItems Is Nothing AndAlso litems.SelectedItems.Count = 1 Then
            Me.load_item_attributes(True)
        End If
    End Sub

    Private Sub Panel1_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Panel1.Paint

    End Sub

    Private Sub uxItems_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxItems.Enter

    End Sub

    Private Sub uxTemplates_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxTemplates.Enter

    End Sub

    Private Sub ttemplatenew_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ttemplatenew.TextChanged

    End Sub
End Class
