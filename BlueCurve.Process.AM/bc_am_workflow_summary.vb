REM Imports System.Windows.Forms.RichTextBoxStreamType
Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports BlueCurve.Core.as
Imports System.Windows.forms
Imports system.Threading
Imports System.Drawing.Bitmap
Imports System.Collections
Imports System.Drawing

Public Class bc_am_workflow_summary
    Inherits System.Windows.Forms.Form

    Public Shared column As Integer
    Public ldoc As New bc_om_document
    Public workflow_enabled As Boolean
    Public sdoc As Boolean
    Public low_res As Boolean = False
    Public cs_enabled As Boolean = False

    'Constants that map to indicies in actionImages image list
    Private Const VIEWDOC_ENABLED As Integer = 4
    Private Const VIEWDOC_DISABLED As Integer = 5
    Private Const CHECKIN_ENABLED As Integer = 0
    Private Const CHECKIN_DISABLED As Integer = 1
    Private Const REVERT_ENABLED As Integer = 2
    Private Const REVERT_DISABLED As Integer = 3

    'Constants that map to indices in buttonImages image list
    Private Const CATEGORISE_BUTTON_ENABLED As Integer = 6
    Private Const CATEGORISE_BUTTON_DISABLED As Integer = 7
    Private Const CHECKOUT_BUTTON_ENABLED As Integer = 2
    Private Const CHECKOUT_BUTTON_DISABLED As Integer = 3
    Private Const VIEWDOC_BUTTON_ENABLED As Integer = 0
    Private Const VIEWDOC_BUTTON_DISABLED As Integer = 1
    Private Const CHECKIN_BUTTON_ENABLED As Integer = 4
    Private Const CHECKIN_BUTTON_DISABLED As Integer = 5

    'Constants that map to incidices in smallicons list
    Private Const CHECKED_IN As Integer = 0
    Private Const CHECKED_OUT As Integer = 1
    Private Const CHECKED_OUT_TO_ME As Integer = 2

    'Consts for two states of form
    Private Const FORM_WIDTH_HIDDEN As Integer = 54
    Friend WithEvents LinkTab As System.Windows.Forms.TabPage
    Friend WithEvents NoLinksLbl As System.Windows.Forms.Label
    Friend WithEvents ComplyLinks As System.Windows.Forms.GroupBox
    Friend WithEvents LinkLabel2 As System.Windows.Forms.LinkLabel
    Friend WithEvents ReachLinks As System.Windows.Forms.GroupBox
    Friend WithEvents LinkLabel1 As System.Windows.Forms.LinkLabel
    Friend WithEvents HistoryTab As System.Windows.Forms.TabPage
    Friend WithEvents AuditTrail As System.Windows.Forms.ListView
    Friend WithEvents da As System.Windows.Forms.ColumnHeader
    Friend WithEvents desc As System.Windows.Forms.ColumnHeader
    Friend WithEvents comment As System.Windows.Forms.ColumnHeader
    Friend WithEvents code As System.Windows.Forms.ColumnHeader
    Friend WithEvents RevisionsTab As System.Windows.Forms.TabPage
    Friend WithEvents RevertDocument As System.Windows.Forms.PictureBox
    Friend WithEvents ViewRevision As System.Windows.Forms.PictureBox
    Friend WithEvents lrevisions As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader7 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader8 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader9 As System.Windows.Forms.ColumnHeader
    Friend WithEvents SupportTab As System.Windows.Forms.TabPage
    Friend WithEvents Tsupptx As System.Windows.Forms.TextBox
    Friend WithEvents CheckInSupportDoc As System.Windows.Forms.PictureBox
    Friend WithEvents CheckOutSupportDoc As System.Windows.Forms.PictureBox
    Friend WithEvents ViewSupportDoc As System.Windows.Forms.PictureBox
    Friend WithEvents SupportingDocsList As System.Windows.Forms.ListView
    Friend WithEvents title As System.Windows.Forms.ColumnHeader
    Friend WithEvents id As System.Windows.Forms.ColumnHeader
    Friend WithEvents dat As System.Windows.Forms.ColumnHeader
    Friend WithEvents type As System.Windows.Forms.ColumnHeader
    Friend WithEvents checkedout As System.Windows.Forms.ColumnHeader
    Friend WithEvents AccessTab As System.Windows.Forms.TabPage
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents chkforcecheckin As System.Windows.Forms.CheckBox
    Friend WithEvents AccessRightsHelpText As System.Windows.Forms.Label
    Friend WithEvents LockedHelp As System.Windows.Forms.PictureBox
    Friend WithEvents WriteHelp As System.Windows.Forms.PictureBox
    Friend WithEvents MoveHelp As System.Windows.Forms.PictureBox
    Friend WithEvents ReadHelp As System.Windows.Forms.PictureBox
    Friend WithEvents ViewHelp As System.Windows.Forms.PictureBox
    Friend WithEvents LockedRights As System.Windows.Forms.CheckBox
    Friend WithEvents WriteRights As System.Windows.Forms.CheckBox
    Friend WithEvents MoveRights As System.Windows.Forms.CheckBox
    Friend WithEvents ReadRights As System.Windows.Forms.CheckBox
    Friend WithEvents ViewRights As System.Windows.Forms.CheckBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents CommentTab As System.Windows.Forms.TabPage
    Friend WithEvents CommentOtherBox As System.Windows.Forms.GroupBox
    Friend WithEvents lcomment As System.Windows.Forms.Label
    Friend WithEvents CommentWorkflowBox As System.Windows.Forms.GroupBox
    Friend WithEvents lworkflow As System.Windows.Forms.Label
    Friend WithEvents cbostages As System.Windows.Forms.ComboBox
    Friend WithEvents DocDetailsTabs As System.Windows.Forms.TabControl
    Friend WithEvents pscat As System.Windows.Forms.PictureBox
    Private Const FORM_WIDTH_DISPLAYED As Integer = 424

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()
        cs_enabled = False
        'Add any initialization after the InitializeComponent() call
        'Me.Width = FORM_WIDTH_HIDDEN
        Me.ViewDocAction.Visible = False
        Me.EditDocAction.Visible = False
        Me.CategoriseDocAction.Visible = False
        Me.EditButtonIcon.Visible = False
        Me.CheckInDocAction.Visible = False
        Me.EditButtonIcon.Visible = False
        Me.CheckOutSupportDoc.Visible = False
        Me.CheckInSupportDoc.Visible = False
        Me.ViewSupportDoc.Visible = False
        Me.RevertDocument.Visible = False
        Me.ViewRevision.Visible = False

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
    Friend WithEvents DocDetails As System.Windows.Forms.RichTextBox
    Friend WithEvents PictureBox5 As System.Windows.Forms.PictureBox
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents DocTitle As System.Windows.Forms.RichTextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ActionsBox As System.Windows.Forms.GroupBox
    Friend WithEvents DocDetailsBox As System.Windows.Forms.GroupBox
    Friend WithEvents WorkflowBox As System.Windows.Forms.GroupBox
    Friend WithEvents ViewDocAction As System.Windows.Forms.PictureBox
    Friend WithEvents buttonImages As System.Windows.Forms.ImageList
    Friend WithEvents EditButtonIcon As System.Windows.Forms.PictureBox
    Friend WithEvents EditDocAction As System.Windows.Forms.PictureBox
    Friend WithEvents buttonIcons As System.Windows.Forms.ImageList
    Friend WithEvents Workflow As System.Windows.Forms.TreeView
    Friend WithEvents CheckInDocAction As System.Windows.Forms.PictureBox
    Friend WithEvents CategoriseDocAction As System.Windows.Forms.PictureBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents CurrentStageColor As System.Windows.Forms.Label
    Friend WithEvents NextStageColor As System.Windows.Forms.Label
    Friend WithEvents smallicons As System.Windows.Forms.ImageList
    Friend WithEvents mimeIcons As System.Windows.Forms.ImageList
    Friend WithEvents actionImages As System.Windows.Forms.ImageList
    Friend WithEvents SummaryButtonImages As System.Windows.Forms.ImageList
    Friend WithEvents ToggleSummaryForm As System.Windows.Forms.PictureBox
    Friend WithEvents FormRoller As System.Windows.Forms.Timer
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents PinForm As System.Windows.Forms.PictureBox
    Friend WithEvents UnRollFormTimer As System.Windows.Forms.Timer
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents lregister As System.Windows.Forms.Label
    Friend WithEvents Ltopreg As System.Windows.Forms.Label
    Friend WithEvents pattach As System.Windows.Forms.PictureBox
    Friend WithEvents lattach As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_workflow_summary))
        Me.DocDetails = New System.Windows.Forms.RichTextBox
        Me.smallicons = New System.Windows.Forms.ImageList(Me.components)
        Me.mimeIcons = New System.Windows.Forms.ImageList(Me.components)
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader4 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader5 = New System.Windows.Forms.ColumnHeader
        Me.Workflow = New System.Windows.Forms.TreeView
        Me.CheckInDocAction = New System.Windows.Forms.PictureBox
        Me.EditDocAction = New System.Windows.Forms.PictureBox
        Me.ViewDocAction = New System.Windows.Forms.PictureBox
        Me.buttonImages = New System.Windows.Forms.ImageList(Me.components)
        Me.DocTitle = New System.Windows.Forms.RichTextBox
        Me.ActionsBox = New System.Windows.Forms.GroupBox
        Me.EditButtonIcon = New System.Windows.Forms.PictureBox
        Me.lattach = New System.Windows.Forms.Label
        Me.pattach = New System.Windows.Forms.PictureBox
        Me.CategoriseDocAction = New System.Windows.Forms.PictureBox
        Me.Ltopreg = New System.Windows.Forms.Label
        Me.DocDetailsBox = New System.Windows.Forms.GroupBox
        Me.WorkflowBox = New System.Windows.Forms.GroupBox
        Me.CurrentStageColor = New System.Windows.Forms.Label
        Me.NextStageColor = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.lregister = New System.Windows.Forms.Label
        Me.buttonIcons = New System.Windows.Forms.ImageList(Me.components)
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.ViewRevision = New System.Windows.Forms.PictureBox
        Me.RevertDocument = New System.Windows.Forms.PictureBox
        Me.ViewSupportDoc = New System.Windows.Forms.PictureBox
        Me.CheckOutSupportDoc = New System.Windows.Forms.PictureBox
        Me.CheckInSupportDoc = New System.Windows.Forms.PictureBox
        Me.pscat = New System.Windows.Forms.PictureBox
        Me.actionImages = New System.Windows.Forms.ImageList(Me.components)
        Me.PinForm = New System.Windows.Forms.PictureBox
        Me.SummaryButtonImages = New System.Windows.Forms.ImageList(Me.components)
        Me.ToggleSummaryForm = New System.Windows.Forms.PictureBox
        Me.FormRoller = New System.Windows.Forms.Timer(Me.components)
        Me.Label1 = New System.Windows.Forms.Label
        Me.UnRollFormTimer = New System.Windows.Forms.Timer(Me.components)
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.LinkTab = New System.Windows.Forms.TabPage
        Me.NoLinksLbl = New System.Windows.Forms.Label
        Me.ComplyLinks = New System.Windows.Forms.GroupBox
        Me.LinkLabel2 = New System.Windows.Forms.LinkLabel
        Me.ReachLinks = New System.Windows.Forms.GroupBox
        Me.LinkLabel1 = New System.Windows.Forms.LinkLabel
        Me.HistoryTab = New System.Windows.Forms.TabPage
        Me.AuditTrail = New System.Windows.Forms.ListView
        Me.da = New System.Windows.Forms.ColumnHeader
        Me.desc = New System.Windows.Forms.ColumnHeader
        Me.comment = New System.Windows.Forms.ColumnHeader
        Me.code = New System.Windows.Forms.ColumnHeader
        Me.RevisionsTab = New System.Windows.Forms.TabPage
        Me.lrevisions = New System.Windows.Forms.ListView
        Me.ColumnHeader6 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader7 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader8 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader9 = New System.Windows.Forms.ColumnHeader
        Me.SupportTab = New System.Windows.Forms.TabPage
        Me.Tsupptx = New System.Windows.Forms.TextBox
        Me.SupportingDocsList = New System.Windows.Forms.ListView
        Me.title = New System.Windows.Forms.ColumnHeader
        Me.id = New System.Windows.Forms.ColumnHeader
        Me.dat = New System.Windows.Forms.ColumnHeader
        Me.type = New System.Windows.Forms.ColumnHeader
        Me.checkedout = New System.Windows.Forms.ColumnHeader
        Me.AccessTab = New System.Windows.Forms.TabPage
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.chkforcecheckin = New System.Windows.Forms.CheckBox
        Me.AccessRightsHelpText = New System.Windows.Forms.Label
        Me.LockedHelp = New System.Windows.Forms.PictureBox
        Me.WriteHelp = New System.Windows.Forms.PictureBox
        Me.MoveHelp = New System.Windows.Forms.PictureBox
        Me.ReadHelp = New System.Windows.Forms.PictureBox
        Me.ViewHelp = New System.Windows.Forms.PictureBox
        Me.LockedRights = New System.Windows.Forms.CheckBox
        Me.WriteRights = New System.Windows.Forms.CheckBox
        Me.MoveRights = New System.Windows.Forms.CheckBox
        Me.ReadRights = New System.Windows.Forms.CheckBox
        Me.ViewRights = New System.Windows.Forms.CheckBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.CommentTab = New System.Windows.Forms.TabPage
        Me.CommentOtherBox = New System.Windows.Forms.GroupBox
        Me.lcomment = New System.Windows.Forms.Label
        Me.CommentWorkflowBox = New System.Windows.Forms.GroupBox
        Me.lworkflow = New System.Windows.Forms.Label
        Me.cbostages = New System.Windows.Forms.ComboBox
        Me.DocDetailsTabs = New System.Windows.Forms.TabControl
        CType(Me.CheckInDocAction, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.EditDocAction, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ViewDocAction, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ActionsBox.SuspendLayout()
        CType(Me.EditButtonIcon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pattach, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CategoriseDocAction, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.DocDetailsBox.SuspendLayout()
        Me.WorkflowBox.SuspendLayout()
        CType(Me.ViewRevision, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RevertDocument, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ViewSupportDoc, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CheckOutSupportDoc, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CheckInSupportDoc, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pscat, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PinForm, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ToggleSummaryForm, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LinkTab.SuspendLayout()
        Me.ComplyLinks.SuspendLayout()
        Me.ReachLinks.SuspendLayout()
        Me.HistoryTab.SuspendLayout()
        Me.RevisionsTab.SuspendLayout()
        Me.SupportTab.SuspendLayout()
        Me.AccessTab.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LockedHelp, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.WriteHelp, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MoveHelp, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ReadHelp, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ViewHelp, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.CommentTab.SuspendLayout()
        Me.CommentOtherBox.SuspendLayout()
        Me.CommentWorkflowBox.SuspendLayout()
        Me.DocDetailsTabs.SuspendLayout()
        Me.SuspendLayout()
        '
        'DocDetails
        '
        Me.DocDetails.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DocDetails.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.DocDetails.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.DocDetails.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DocDetails.Location = New System.Drawing.Point(8, 14)
        Me.DocDetails.Name = "DocDetails"
        Me.DocDetails.ReadOnly = True
        Me.DocDetails.Size = New System.Drawing.Size(368, 122)
        Me.DocDetails.TabIndex = 1
        Me.DocDetails.Text = "DocDetails"
        Me.DocDetails.WordWrap = False
        '
        'smallicons
        '
        Me.smallicons.ImageStream = CType(resources.GetObject("smallicons.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.smallicons.TransparentColor = System.Drawing.Color.Transparent
        Me.smallicons.Images.SetKeyName(0, "")
        Me.smallicons.Images.SetKeyName(1, "")
        Me.smallicons.Images.SetKeyName(2, "")
        '
        'mimeIcons
        '
        Me.mimeIcons.ImageStream = CType(resources.GetObject("mimeIcons.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.mimeIcons.TransparentColor = System.Drawing.Color.Transparent
        Me.mimeIcons.Images.SetKeyName(0, "")
        Me.mimeIcons.Images.SetKeyName(1, "")
        Me.mimeIcons.Images.SetKeyName(2, "")
        Me.mimeIcons.Images.SetKeyName(3, "")
        Me.mimeIcons.Images.SetKeyName(4, "")
        Me.mimeIcons.Images.SetKeyName(5, "")
        Me.mimeIcons.Images.SetKeyName(6, "")
        Me.mimeIcons.Images.SetKeyName(7, "")
        Me.mimeIcons.Images.SetKeyName(8, "")
        Me.mimeIcons.Images.SetKeyName(9, "")
        Me.mimeIcons.Images.SetKeyName(10, "")
        Me.mimeIcons.Images.SetKeyName(11, "")
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Title"
        Me.ColumnHeader1.Width = 135
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "id"
        Me.ColumnHeader2.Width = 0
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Date"
        Me.ColumnHeader3.Width = 77
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Type"
        Me.ColumnHeader4.Width = 45
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "Checked Out "
        '
        'Workflow
        '
        Me.Workflow.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Workflow.Indent = 25
        Me.Workflow.ItemHeight = 14
        Me.Workflow.Location = New System.Drawing.Point(8, 40)
        Me.Workflow.Name = "Workflow"
        Me.Workflow.Size = New System.Drawing.Size(366, 96)
        Me.Workflow.TabIndex = 16
        '
        'CheckInDocAction
        '
        Me.CheckInDocAction.Image = CType(resources.GetObject("CheckInDocAction.Image"), System.Drawing.Image)
        Me.CheckInDocAction.Location = New System.Drawing.Point(246, 16)
        Me.CheckInDocAction.Name = "CheckInDocAction"
        Me.CheckInDocAction.Size = New System.Drawing.Size(136, 26)
        Me.CheckInDocAction.TabIndex = 19
        Me.CheckInDocAction.TabStop = False
        Me.CheckInDocAction.Visible = False
        '
        'EditDocAction
        '
        Me.EditDocAction.Image = CType(resources.GetObject("EditDocAction.Image"), System.Drawing.Image)
        Me.EditDocAction.Location = New System.Drawing.Point(112, 16)
        Me.EditDocAction.Name = "EditDocAction"
        Me.EditDocAction.Size = New System.Drawing.Size(128, 25)
        Me.EditDocAction.TabIndex = 18
        Me.EditDocAction.TabStop = False
        Me.ToolTip1.SetToolTip(Me.EditDocAction, "Click here to Check out the document for editing. No other user will be able to e" & _
                "dit the document while you have it checked out.")
        '
        'ViewDocAction
        '
        Me.ViewDocAction.Image = CType(resources.GetObject("ViewDocAction.Image"), System.Drawing.Image)
        Me.ViewDocAction.Location = New System.Drawing.Point(8, 16)
        Me.ViewDocAction.Name = "ViewDocAction"
        Me.ViewDocAction.Size = New System.Drawing.Size(96, 25)
        Me.ViewDocAction.TabIndex = 17
        Me.ViewDocAction.TabStop = False
        Me.ToolTip1.SetToolTip(Me.ViewDocAction, "Click here to open the document in read-only mode")
        '
        'buttonImages
        '
        Me.buttonImages.ImageStream = CType(resources.GetObject("buttonImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.buttonImages.TransparentColor = System.Drawing.Color.Transparent
        Me.buttonImages.Images.SetKeyName(0, "")
        Me.buttonImages.Images.SetKeyName(1, "")
        Me.buttonImages.Images.SetKeyName(2, "")
        Me.buttonImages.Images.SetKeyName(3, "")
        Me.buttonImages.Images.SetKeyName(4, "")
        Me.buttonImages.Images.SetKeyName(5, "")
        Me.buttonImages.Images.SetKeyName(6, "")
        Me.buttonImages.Images.SetKeyName(7, "")
        '
        'DocTitle
        '
        Me.DocTitle.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DocTitle.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.DocTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.DocTitle.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DocTitle.ForeColor = System.Drawing.Color.Black
        Me.DocTitle.Location = New System.Drawing.Point(24, 0)
        Me.DocTitle.Name = "DocTitle"
        Me.DocTitle.ReadOnly = True
        Me.DocTitle.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Horizontal
        Me.DocTitle.Size = New System.Drawing.Size(384, 32)
        Me.DocTitle.TabIndex = 20
        Me.DocTitle.Text = "DocTitle"
        '
        'ActionsBox
        '
        Me.ActionsBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ActionsBox.Controls.Add(Me.EditButtonIcon)
        Me.ActionsBox.Controls.Add(Me.lattach)
        Me.ActionsBox.Controls.Add(Me.pattach)
        Me.ActionsBox.Controls.Add(Me.CategoriseDocAction)
        Me.ActionsBox.Controls.Add(Me.ViewDocAction)
        Me.ActionsBox.Controls.Add(Me.EditDocAction)
        Me.ActionsBox.Controls.Add(Me.CheckInDocAction)
        Me.ActionsBox.Controls.Add(Me.Ltopreg)
        Me.ActionsBox.Font = New System.Drawing.Font("Verdana", 7.25!)
        Me.ActionsBox.Location = New System.Drawing.Point(24, 32)
        Me.ActionsBox.Name = "ActionsBox"
        Me.ActionsBox.Size = New System.Drawing.Size(384, 48)
        Me.ActionsBox.TabIndex = 21
        Me.ActionsBox.TabStop = False
        Me.ActionsBox.Text = "Actions"
        '
        'EditButtonIcon
        '
        Me.EditButtonIcon.Image = CType(resources.GetObject("EditButtonIcon.Image"), System.Drawing.Image)
        Me.EditButtonIcon.Location = New System.Drawing.Point(114, 21)
        Me.EditButtonIcon.Name = "EditButtonIcon"
        Me.EditButtonIcon.Size = New System.Drawing.Size(18, 15)
        Me.EditButtonIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.EditButtonIcon.TabIndex = 20
        Me.EditButtonIcon.TabStop = False
        '
        'lattach
        '
        Me.lattach.BackColor = System.Drawing.Color.FromArgb(CType(CType(166, Byte), Integer), CType(CType(202, Byte), Integer), CType(CType(240, Byte), Integer))
        Me.lattach.Font = New System.Drawing.Font("Verdana", 11.0!)
        Me.lattach.Location = New System.Drawing.Point(36, 20)
        Me.lattach.Name = "lattach"
        Me.lattach.Size = New System.Drawing.Size(89, 16)
        Me.lattach.TabIndex = 24
        Me.lattach.Text = "Attach File"
        '
        'pattach
        '
        Me.pattach.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.pattach.Image = CType(resources.GetObject("pattach.Image"), System.Drawing.Image)
        Me.pattach.Location = New System.Drawing.Point(7, 14)
        Me.pattach.Name = "pattach"
        Me.pattach.Size = New System.Drawing.Size(129, 26)
        Me.pattach.TabIndex = 23
        Me.pattach.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pattach, "Click here to Check out the document for editing. No other user will be able to e" & _
                "dit the document while you have it checked out.")
        '
        'CategoriseDocAction
        '
        Me.CategoriseDocAction.Image = CType(resources.GetObject("CategoriseDocAction.Image"), System.Drawing.Image)
        Me.CategoriseDocAction.Location = New System.Drawing.Point(247, 16)
        Me.CategoriseDocAction.Name = "CategoriseDocAction"
        Me.CategoriseDocAction.Size = New System.Drawing.Size(128, 25)
        Me.CategoriseDocAction.TabIndex = 21
        Me.CategoriseDocAction.TabStop = False
        Me.ToolTip1.SetToolTip(Me.CategoriseDocAction, "Click here to change the entity classification of this document.")
        '
        'Ltopreg
        '
        Me.Ltopreg.Font = New System.Drawing.Font("Verdana", 8.0!)
        Me.Ltopreg.Location = New System.Drawing.Point(152, 24)
        Me.Ltopreg.Name = "Ltopreg"
        Me.Ltopreg.Size = New System.Drawing.Size(184, 16)
        Me.Ltopreg.TabIndex = 22
        Me.Ltopreg.Visible = False
        '
        'DocDetailsBox
        '
        Me.DocDetailsBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DocDetailsBox.Controls.Add(Me.DocDetails)
        Me.DocDetailsBox.Font = New System.Drawing.Font("Verdana", 7.25!)
        Me.DocDetailsBox.Location = New System.Drawing.Point(24, 79)
        Me.DocDetailsBox.Name = "DocDetailsBox"
        Me.DocDetailsBox.Size = New System.Drawing.Size(384, 160)
        Me.DocDetailsBox.TabIndex = 22
        Me.DocDetailsBox.TabStop = False
        Me.DocDetailsBox.Text = "Document details"
        '
        'WorkflowBox
        '
        Me.WorkflowBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.WorkflowBox.Controls.Add(Me.CurrentStageColor)
        Me.WorkflowBox.Controls.Add(Me.NextStageColor)
        Me.WorkflowBox.Controls.Add(Me.Label2)
        Me.WorkflowBox.Controls.Add(Me.Workflow)
        Me.WorkflowBox.Controls.Add(Me.lregister)
        Me.WorkflowBox.Font = New System.Drawing.Font("Verdana", 7.25!)
        Me.WorkflowBox.Location = New System.Drawing.Point(24, 239)
        Me.WorkflowBox.Name = "WorkflowBox"
        Me.WorkflowBox.Size = New System.Drawing.Size(382, 153)
        Me.WorkflowBox.TabIndex = 23
        Me.WorkflowBox.TabStop = False
        Me.WorkflowBox.Text = "Workflow"
        '
        'CurrentStageColor
        '
        Me.CurrentStageColor.BackColor = System.Drawing.Color.LightSteelBlue
        Me.CurrentStageColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.CurrentStageColor.Location = New System.Drawing.Point(8, 18)
        Me.CurrentStageColor.Name = "CurrentStageColor"
        Me.CurrentStageColor.Size = New System.Drawing.Size(12, 12)
        Me.CurrentStageColor.TabIndex = 19
        '
        'NextStageColor
        '
        Me.NextStageColor.BackColor = System.Drawing.Color.FromArgb(CType(CType(207, Byte), Integer), CType(CType(229, Byte), Integer), CType(CType(240, Byte), Integer))
        Me.NextStageColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.NextStageColor.Location = New System.Drawing.Point(173, 18)
        Me.NextStageColor.Name = "NextStageColor"
        Me.NextStageColor.Size = New System.Drawing.Size(12, 12)
        Me.NextStageColor.TabIndex = 18
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(8, 16)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(368, 16)
        Me.Label2.TabIndex = 17
        Me.Label2.Text = "     indicates the current state.            indicates possible next stages."
        '
        'lregister
        '
        Me.lregister.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lregister.Font = New System.Drawing.Font("Verdana", 8.0!)
        Me.lregister.Location = New System.Drawing.Point(8, 48)
        Me.lregister.Name = "lregister"
        Me.lregister.Size = New System.Drawing.Size(366, 48)
        Me.lregister.TabIndex = 29
        Me.lregister.Visible = False
        '
        'buttonIcons
        '
        Me.buttonIcons.ImageStream = CType(resources.GetObject("buttonIcons.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.buttonIcons.TransparentColor = System.Drawing.Color.Transparent
        Me.buttonIcons.Images.SetKeyName(0, "")
        Me.buttonIcons.Images.SetKeyName(1, "")
        '
        'ViewRevision
        '
        Me.ViewRevision.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ViewRevision.BackColor = System.Drawing.Color.Transparent
        Me.ViewRevision.Image = CType(resources.GetObject("ViewRevision.Image"), System.Drawing.Image)
        Me.ViewRevision.Location = New System.Drawing.Point(334, 18)
        Me.ViewRevision.Name = "ViewRevision"
        Me.ViewRevision.Size = New System.Drawing.Size(32, 32)
        Me.ViewRevision.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.ViewRevision.TabIndex = 1
        Me.ViewRevision.TabStop = False
        Me.ToolTip1.SetToolTip(Me.ViewRevision, "View this revision")
        '
        'RevertDocument
        '
        Me.RevertDocument.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RevertDocument.Image = CType(resources.GetObject("RevertDocument.Image"), System.Drawing.Image)
        Me.RevertDocument.Location = New System.Drawing.Point(332, 68)
        Me.RevertDocument.Name = "RevertDocument"
        Me.RevertDocument.Size = New System.Drawing.Size(32, 32)
        Me.RevertDocument.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.RevertDocument.TabIndex = 2
        Me.RevertDocument.TabStop = False
        Me.ToolTip1.SetToolTip(Me.RevertDocument, "Revert document to this revision")
        '
        'ViewSupportDoc
        '
        Me.ViewSupportDoc.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ViewSupportDoc.Image = CType(resources.GetObject("ViewSupportDoc.Image"), System.Drawing.Image)
        Me.ViewSupportDoc.Location = New System.Drawing.Point(334, 18)
        Me.ViewSupportDoc.Name = "ViewSupportDoc"
        Me.ViewSupportDoc.Size = New System.Drawing.Size(32, 32)
        Me.ViewSupportDoc.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.ViewSupportDoc.TabIndex = 12
        Me.ViewSupportDoc.TabStop = False
        Me.ToolTip1.SetToolTip(Me.ViewSupportDoc, "View support document")
        '
        'CheckOutSupportDoc
        '
        Me.CheckOutSupportDoc.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CheckOutSupportDoc.Image = CType(resources.GetObject("CheckOutSupportDoc.Image"), System.Drawing.Image)
        Me.CheckOutSupportDoc.Location = New System.Drawing.Point(334, 69)
        Me.CheckOutSupportDoc.Name = "CheckOutSupportDoc"
        Me.CheckOutSupportDoc.Size = New System.Drawing.Size(32, 32)
        Me.CheckOutSupportDoc.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.CheckOutSupportDoc.TabIndex = 13
        Me.CheckOutSupportDoc.TabStop = False
        Me.ToolTip1.SetToolTip(Me.CheckOutSupportDoc, "Check-out support document")
        '
        'CheckInSupportDoc
        '
        Me.CheckInSupportDoc.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CheckInSupportDoc.Image = CType(resources.GetObject("CheckInSupportDoc.Image"), System.Drawing.Image)
        Me.CheckInSupportDoc.Location = New System.Drawing.Point(336, 120)
        Me.CheckInSupportDoc.Name = "CheckInSupportDoc"
        Me.CheckInSupportDoc.Size = New System.Drawing.Size(32, 32)
        Me.CheckInSupportDoc.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.CheckInSupportDoc.TabIndex = 14
        Me.CheckInSupportDoc.TabStop = False
        Me.ToolTip1.SetToolTip(Me.CheckInSupportDoc, "Check-in support document")
        '
        'pscat
        '
        Me.pscat.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pscat.Image = CType(resources.GetObject("pscat.Image"), System.Drawing.Image)
        Me.pscat.Location = New System.Drawing.Point(334, 120)
        Me.pscat.Name = "pscat"
        Me.pscat.Size = New System.Drawing.Size(32, 32)
        Me.pscat.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pscat.TabIndex = 16
        Me.pscat.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pscat, "Categorise support document")
        Me.pscat.Visible = False
        '
        'actionImages
        '
        Me.actionImages.ImageStream = CType(resources.GetObject("actionImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.actionImages.TransparentColor = System.Drawing.Color.Transparent
        Me.actionImages.Images.SetKeyName(0, "")
        Me.actionImages.Images.SetKeyName(1, "")
        Me.actionImages.Images.SetKeyName(2, "")
        Me.actionImages.Images.SetKeyName(3, "")
        Me.actionImages.Images.SetKeyName(4, "")
        Me.actionImages.Images.SetKeyName(5, "")
        '
        'PinForm
        '
        Me.PinForm.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PinForm.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PinForm.Image = CType(resources.GetObject("PinForm.Image"), System.Drawing.Image)
        Me.PinForm.Location = New System.Drawing.Point(393, 6)
        Me.PinForm.Name = "PinForm"
        Me.PinForm.Size = New System.Drawing.Size(22, 22)
        Me.PinForm.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PinForm.TabIndex = 26
        Me.PinForm.TabStop = False
        Me.PinForm.Visible = False
        '
        'SummaryButtonImages
        '
        Me.SummaryButtonImages.ImageStream = CType(resources.GetObject("SummaryButtonImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.SummaryButtonImages.TransparentColor = System.Drawing.Color.Transparent
        Me.SummaryButtonImages.Images.SetKeyName(0, "")
        Me.SummaryButtonImages.Images.SetKeyName(1, "")
        '
        'ToggleSummaryForm
        '
        Me.ToggleSummaryForm.Image = CType(resources.GetObject("ToggleSummaryForm.Image"), System.Drawing.Image)
        Me.ToggleSummaryForm.Location = New System.Drawing.Point(-4, 2)
        Me.ToggleSummaryForm.Name = "ToggleSummaryForm"
        Me.ToggleSummaryForm.Size = New System.Drawing.Size(28, 232)
        Me.ToggleSummaryForm.TabIndex = 27
        Me.ToggleSummaryForm.TabStop = False
        '
        'FormRoller
        '
        Me.FormRoller.Interval = 1
        '
        'Label1
        '
        Me.Label1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.Location = New System.Drawing.Point(408, 192)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(8, 424)
        Me.Label1.TabIndex = 28
        '
        'UnRollFormTimer
        '
        Me.UnRollFormTimer.Interval = 3000
        '
        'ImageList1
        '
        Me.ImageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit
        Me.ImageList1.ImageSize = New System.Drawing.Size(16, 16)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        '
        'LinkTab
        '
        Me.LinkTab.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.LinkTab.Controls.Add(Me.NoLinksLbl)
        Me.LinkTab.Controls.Add(Me.ComplyLinks)
        Me.LinkTab.Controls.Add(Me.ReachLinks)
        Me.LinkTab.Location = New System.Drawing.Point(4, 21)
        Me.LinkTab.Name = "LinkTab"
        Me.LinkTab.Size = New System.Drawing.Size(376, 168)
        Me.LinkTab.TabIndex = 5
        Me.LinkTab.Text = "Links"
        Me.LinkTab.UseVisualStyleBackColor = True
        '
        'NoLinksLbl
        '
        Me.NoLinksLbl.Font = New System.Drawing.Font("Verdana", 7.25!)
        Me.NoLinksLbl.Location = New System.Drawing.Point(16, 80)
        Me.NoLinksLbl.Name = "NoLinksLbl"
        Me.NoLinksLbl.Size = New System.Drawing.Size(344, 16)
        Me.NoLinksLbl.TabIndex = 3
        Me.NoLinksLbl.Text = "This document has no Blue Curve links"
        '
        'ComplyLinks
        '
        Me.ComplyLinks.Controls.Add(Me.LinkLabel2)
        Me.ComplyLinks.Font = New System.Drawing.Font("Verdana", 7.25!)
        Me.ComplyLinks.Location = New System.Drawing.Point(16, 96)
        Me.ComplyLinks.Name = "ComplyLinks"
        Me.ComplyLinks.Size = New System.Drawing.Size(344, 56)
        Me.ComplyLinks.TabIndex = 2
        Me.ComplyLinks.TabStop = False
        Me.ComplyLinks.Text = "Link 2"
        Me.ComplyLinks.Visible = False
        '
        'LinkLabel2
        '
        Me.LinkLabel2.Location = New System.Drawing.Point(16, 16)
        Me.LinkLabel2.Name = "LinkLabel2"
        Me.LinkLabel2.Size = New System.Drawing.Size(280, 24)
        Me.LinkLabel2.TabIndex = 1
        Me.LinkLabel2.TabStop = True
        Me.LinkLabel2.Text = "LinkLabel2"
        '
        'ReachLinks
        '
        Me.ReachLinks.Controls.Add(Me.LinkLabel1)
        Me.ReachLinks.Font = New System.Drawing.Font("Verdana", 7.25!)
        Me.ReachLinks.Location = New System.Drawing.Point(16, 16)
        Me.ReachLinks.Name = "ReachLinks"
        Me.ReachLinks.Size = New System.Drawing.Size(344, 56)
        Me.ReachLinks.TabIndex = 0
        Me.ReachLinks.TabStop = False
        Me.ReachLinks.Text = "Link 1"
        Me.ReachLinks.Visible = False
        '
        'LinkLabel1
        '
        Me.LinkLabel1.Location = New System.Drawing.Point(16, 24)
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.Size = New System.Drawing.Size(280, 24)
        Me.LinkLabel1.TabIndex = 0
        Me.LinkLabel1.TabStop = True
        Me.LinkLabel1.Text = "LinkLabel1"
        '
        'HistoryTab
        '
        Me.HistoryTab.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.HistoryTab.Controls.Add(Me.AuditTrail)
        Me.HistoryTab.Location = New System.Drawing.Point(4, 21)
        Me.HistoryTab.Name = "HistoryTab"
        Me.HistoryTab.Size = New System.Drawing.Size(376, 168)
        Me.HistoryTab.TabIndex = 4
        Me.HistoryTab.Text = "History"
        Me.HistoryTab.UseVisualStyleBackColor = True
        '
        'AuditTrail
        '
        Me.AuditTrail.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AuditTrail.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.da, Me.desc, Me.comment, Me.code})
        Me.AuditTrail.Font = New System.Drawing.Font("Verdana", 7.25!)
        Me.AuditTrail.FullRowSelect = True
        Me.AuditTrail.Location = New System.Drawing.Point(0, 0)
        Me.AuditTrail.Name = "AuditTrail"
        Me.AuditTrail.Size = New System.Drawing.Size(376, 170)
        Me.AuditTrail.TabIndex = 8
        Me.AuditTrail.UseCompatibleStateImageBehavior = False
        Me.AuditTrail.View = System.Windows.Forms.View.Details
        '
        'da
        '
        Me.da.Text = "History Date"
        Me.da.Width = 100
        '
        'desc
        '
        Me.desc.Text = "Description"
        Me.desc.Width = 247
        '
        'comment
        '
        Me.comment.Text = "comment"
        Me.comment.Width = 75
        '
        'code
        '
        Me.code.Text = "code"
        Me.code.Width = 41
        '
        'RevisionsTab
        '
        Me.RevisionsTab.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.RevisionsTab.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.RevisionsTab.Controls.Add(Me.RevertDocument)
        Me.RevisionsTab.Controls.Add(Me.ViewRevision)
        Me.RevisionsTab.Controls.Add(Me.lrevisions)
        Me.RevisionsTab.Location = New System.Drawing.Point(4, 21)
        Me.RevisionsTab.Name = "RevisionsTab"
        Me.RevisionsTab.Size = New System.Drawing.Size(376, 168)
        Me.RevisionsTab.TabIndex = 3
        Me.RevisionsTab.Text = "Revisions"
        Me.RevisionsTab.UseVisualStyleBackColor = True
        '
        'lrevisions
        '
        Me.lrevisions.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lrevisions.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader6, Me.ColumnHeader7, Me.ColumnHeader8, Me.ColumnHeader9})
        Me.lrevisions.Font = New System.Drawing.Font("Verdana", 7.25!)
        Me.lrevisions.FullRowSelect = True
        Me.lrevisions.Location = New System.Drawing.Point(0, 0)
        Me.lrevisions.Name = "lrevisions"
        Me.lrevisions.Size = New System.Drawing.Size(318, 168)
        Me.lrevisions.SmallImageList = Me.smallicons
        Me.lrevisions.TabIndex = 0
        Me.lrevisions.UseCompatibleStateImageBehavior = False
        Me.lrevisions.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader6
        '
        Me.ColumnHeader6.Text = "Date"
        Me.ColumnHeader6.Width = 97
        '
        'ColumnHeader7
        '
        Me.ColumnHeader7.Text = "Stage"
        Me.ColumnHeader7.Width = 80
        '
        'ColumnHeader8
        '
        Me.ColumnHeader8.Text = "Filename"
        Me.ColumnHeader8.Width = 65
        '
        'ColumnHeader9
        '
        Me.ColumnHeader9.Text = "Taken By"
        '
        'SupportTab
        '
        Me.SupportTab.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.SupportTab.Controls.Add(Me.pscat)
        Me.SupportTab.Controls.Add(Me.Tsupptx)
        Me.SupportTab.Controls.Add(Me.CheckInSupportDoc)
        Me.SupportTab.Controls.Add(Me.CheckOutSupportDoc)
        Me.SupportTab.Controls.Add(Me.ViewSupportDoc)
        Me.SupportTab.Controls.Add(Me.SupportingDocsList)
        Me.SupportTab.Location = New System.Drawing.Point(4, 21)
        Me.SupportTab.Name = "SupportTab"
        Me.SupportTab.Size = New System.Drawing.Size(376, 168)
        Me.SupportTab.TabIndex = 0
        Me.SupportTab.Text = "Support Docs "
        Me.SupportTab.UseVisualStyleBackColor = True
        '
        'Tsupptx
        '
        Me.Tsupptx.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Tsupptx.Location = New System.Drawing.Point(346, 152)
        Me.Tsupptx.Multiline = True
        Me.Tsupptx.Name = "Tsupptx"
        Me.Tsupptx.ReadOnly = True
        Me.Tsupptx.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.Tsupptx.Size = New System.Drawing.Size(30, 33)
        Me.Tsupptx.TabIndex = 15
        Me.Tsupptx.Visible = False
        '
        'SupportingDocsList
        '
        Me.SupportingDocsList.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SupportingDocsList.BackColor = System.Drawing.SystemColors.Window
        Me.SupportingDocsList.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.title, Me.id, Me.dat, Me.type, Me.checkedout})
        Me.SupportingDocsList.Font = New System.Drawing.Font("Verdana", 7.25!)
        Me.SupportingDocsList.FullRowSelect = True
        Me.SupportingDocsList.Location = New System.Drawing.Point(0, 0)
        Me.SupportingDocsList.Name = "SupportingDocsList"
        Me.SupportingDocsList.ShowItemToolTips = True
        Me.SupportingDocsList.Size = New System.Drawing.Size(320, 137)
        Me.SupportingDocsList.SmallImageList = Me.smallicons
        Me.SupportingDocsList.TabIndex = 9
        Me.SupportingDocsList.UseCompatibleStateImageBehavior = False
        Me.SupportingDocsList.View = System.Windows.Forms.View.Details
        '
        'title
        '
        Me.title.Text = "Title"
        Me.title.Width = 135
        '
        'id
        '
        Me.id.Text = "id"
        Me.id.Width = 0
        '
        'dat
        '
        Me.dat.Text = "Date"
        Me.dat.Width = 77
        '
        'type
        '
        Me.type.Text = "Type"
        Me.type.Width = 45
        '
        'checkedout
        '
        Me.checkedout.Text = "Checked Out "
        '
        'AccessTab
        '
        Me.AccessTab.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.AccessTab.Controls.Add(Me.PictureBox1)
        Me.AccessTab.Controls.Add(Me.chkforcecheckin)
        Me.AccessTab.Controls.Add(Me.AccessRightsHelpText)
        Me.AccessTab.Controls.Add(Me.LockedHelp)
        Me.AccessTab.Controls.Add(Me.WriteHelp)
        Me.AccessTab.Controls.Add(Me.MoveHelp)
        Me.AccessTab.Controls.Add(Me.ReadHelp)
        Me.AccessTab.Controls.Add(Me.ViewHelp)
        Me.AccessTab.Controls.Add(Me.LockedRights)
        Me.AccessTab.Controls.Add(Me.WriteRights)
        Me.AccessTab.Controls.Add(Me.MoveRights)
        Me.AccessTab.Controls.Add(Me.ReadRights)
        Me.AccessTab.Controls.Add(Me.ViewRights)
        Me.AccessTab.Controls.Add(Me.Label3)
        Me.AccessTab.Location = New System.Drawing.Point(4, 21)
        Me.AccessTab.Name = "AccessTab"
        Me.AccessTab.Size = New System.Drawing.Size(376, 168)
        Me.AccessTab.TabIndex = 2
        Me.AccessTab.Text = "Access"
        Me.AccessTab.UseVisualStyleBackColor = True
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(130, 136)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(16, 16)
        Me.PictureBox1.TabIndex = 23
        Me.PictureBox1.TabStop = False
        '
        'chkforcecheckin
        '
        Me.chkforcecheckin.Enabled = False
        Me.chkforcecheckin.Font = New System.Drawing.Font("Verdana", 7.25!)
        Me.chkforcecheckin.Location = New System.Drawing.Point(24, 136)
        Me.chkforcecheckin.Name = "chkforcecheckin"
        Me.chkforcecheckin.Size = New System.Drawing.Size(122, 16)
        Me.chkforcecheckin.TabIndex = 22
        Me.chkforcecheckin.Text = "Force Check-in"
        '
        'AccessRightsHelpText
        '
        Me.AccessRightsHelpText.Font = New System.Drawing.Font("Verdana", 7.25!)
        Me.AccessRightsHelpText.Location = New System.Drawing.Point(167, 40)
        Me.AccessRightsHelpText.Name = "AccessRightsHelpText"
        Me.AccessRightsHelpText.Size = New System.Drawing.Size(200, 112)
        Me.AccessRightsHelpText.TabIndex = 21
        Me.AccessRightsHelpText.Text = "Click on each ? icon to view more information on each access right"
        '
        'LockedHelp
        '
        Me.LockedHelp.Image = CType(resources.GetObject("LockedHelp.Image"), System.Drawing.Image)
        Me.LockedHelp.Location = New System.Drawing.Point(130, 116)
        Me.LockedHelp.Name = "LockedHelp"
        Me.LockedHelp.Size = New System.Drawing.Size(16, 16)
        Me.LockedHelp.TabIndex = 20
        Me.LockedHelp.TabStop = False
        '
        'WriteHelp
        '
        Me.WriteHelp.Image = CType(resources.GetObject("WriteHelp.Image"), System.Drawing.Image)
        Me.WriteHelp.Location = New System.Drawing.Point(130, 97)
        Me.WriteHelp.Name = "WriteHelp"
        Me.WriteHelp.Size = New System.Drawing.Size(16, 16)
        Me.WriteHelp.TabIndex = 19
        Me.WriteHelp.TabStop = False
        '
        'MoveHelp
        '
        Me.MoveHelp.Image = CType(resources.GetObject("MoveHelp.Image"), System.Drawing.Image)
        Me.MoveHelp.Location = New System.Drawing.Point(130, 78)
        Me.MoveHelp.Name = "MoveHelp"
        Me.MoveHelp.Size = New System.Drawing.Size(16, 16)
        Me.MoveHelp.TabIndex = 18
        Me.MoveHelp.TabStop = False
        '
        'ReadHelp
        '
        Me.ReadHelp.Image = CType(resources.GetObject("ReadHelp.Image"), System.Drawing.Image)
        Me.ReadHelp.Location = New System.Drawing.Point(130, 59)
        Me.ReadHelp.Name = "ReadHelp"
        Me.ReadHelp.Size = New System.Drawing.Size(16, 16)
        Me.ReadHelp.TabIndex = 17
        Me.ReadHelp.TabStop = False
        '
        'ViewHelp
        '
        Me.ViewHelp.Image = CType(resources.GetObject("ViewHelp.Image"), System.Drawing.Image)
        Me.ViewHelp.Location = New System.Drawing.Point(130, 40)
        Me.ViewHelp.Name = "ViewHelp"
        Me.ViewHelp.Size = New System.Drawing.Size(16, 16)
        Me.ViewHelp.TabIndex = 16
        Me.ViewHelp.TabStop = False
        '
        'LockedRights
        '
        Me.LockedRights.Enabled = False
        Me.LockedRights.Font = New System.Drawing.Font("Verdana", 7.25!)
        Me.LockedRights.Location = New System.Drawing.Point(24, 116)
        Me.LockedRights.Name = "LockedRights"
        Me.LockedRights.Size = New System.Drawing.Size(80, 16)
        Me.LockedRights.TabIndex = 15
        Me.LockedRights.Text = "Locked"
        '
        'WriteRights
        '
        Me.WriteRights.Enabled = False
        Me.WriteRights.Font = New System.Drawing.Font("Verdana", 7.25!)
        Me.WriteRights.Location = New System.Drawing.Point(24, 97)
        Me.WriteRights.Name = "WriteRights"
        Me.WriteRights.Size = New System.Drawing.Size(72, 16)
        Me.WriteRights.TabIndex = 14
        Me.WriteRights.Text = "Write "
        '
        'MoveRights
        '
        Me.MoveRights.Enabled = False
        Me.MoveRights.Font = New System.Drawing.Font("Verdana", 7.25!)
        Me.MoveRights.Location = New System.Drawing.Point(24, 78)
        Me.MoveRights.Name = "MoveRights"
        Me.MoveRights.Size = New System.Drawing.Size(64, 16)
        Me.MoveRights.TabIndex = 13
        Me.MoveRights.Text = "Move "
        '
        'ReadRights
        '
        Me.ReadRights.Enabled = False
        Me.ReadRights.Font = New System.Drawing.Font("Verdana", 7.25!)
        Me.ReadRights.Location = New System.Drawing.Point(24, 59)
        Me.ReadRights.Name = "ReadRights"
        Me.ReadRights.Size = New System.Drawing.Size(64, 16)
        Me.ReadRights.TabIndex = 12
        Me.ReadRights.Text = "Read"
        '
        'ViewRights
        '
        Me.ViewRights.Checked = True
        Me.ViewRights.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ViewRights.Enabled = False
        Me.ViewRights.Font = New System.Drawing.Font("Verdana", 7.25!)
        Me.ViewRights.Location = New System.Drawing.Point(24, 40)
        Me.ViewRights.Name = "ViewRights"
        Me.ViewRights.Size = New System.Drawing.Size(64, 16)
        Me.ViewRights.TabIndex = 11
        Me.ViewRights.Text = "View"
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Verdana", 7.25!)
        Me.Label3.Location = New System.Drawing.Point(8, 16)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(344, 16)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "You have the following access rights for this document:"
        '
        'CommentTab
        '
        Me.CommentTab.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.CommentTab.Controls.Add(Me.CommentOtherBox)
        Me.CommentTab.Controls.Add(Me.CommentWorkflowBox)
        Me.CommentTab.ForeColor = System.Drawing.SystemColors.ActiveBorder
        Me.CommentTab.Location = New System.Drawing.Point(4, 21)
        Me.CommentTab.Name = "CommentTab"
        Me.CommentTab.Size = New System.Drawing.Size(376, 168)
        Me.CommentTab.TabIndex = 1
        Me.CommentTab.Text = "Workflow "
        Me.CommentTab.UseVisualStyleBackColor = True
        '
        'CommentOtherBox
        '
        Me.CommentOtherBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CommentOtherBox.Controls.Add(Me.lcomment)
        Me.CommentOtherBox.Font = New System.Drawing.Font("Verdana", 7.25!)
        Me.CommentOtherBox.ForeColor = System.Drawing.Color.Black
        Me.CommentOtherBox.Location = New System.Drawing.Point(8, 80)
        Me.CommentOtherBox.Name = "CommentOtherBox"
        Me.CommentOtherBox.Size = New System.Drawing.Size(360, 82)
        Me.CommentOtherBox.TabIndex = 4
        Me.CommentOtherBox.TabStop = False
        Me.CommentOtherBox.Text = "Other comments"
        '
        'lcomment
        '
        Me.lcomment.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lcomment.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.25!)
        Me.lcomment.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lcomment.Location = New System.Drawing.Point(8, 31)
        Me.lcomment.Name = "lcomment"
        Me.lcomment.Size = New System.Drawing.Size(349, 44)
        Me.lcomment.TabIndex = 2
        '
        'CommentWorkflowBox
        '
        Me.CommentWorkflowBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CommentWorkflowBox.Controls.Add(Me.lworkflow)
        Me.CommentWorkflowBox.Controls.Add(Me.cbostages)
        Me.CommentWorkflowBox.Font = New System.Drawing.Font("Verdana", 7.25!)
        Me.CommentWorkflowBox.ForeColor = System.Drawing.Color.Black
        Me.CommentWorkflowBox.Location = New System.Drawing.Point(8, 8)
        Me.CommentWorkflowBox.Name = "CommentWorkflowBox"
        Me.CommentWorkflowBox.Size = New System.Drawing.Size(360, 64)
        Me.CommentWorkflowBox.TabIndex = 3
        Me.CommentWorkflowBox.TabStop = False
        Me.CommentWorkflowBox.Text = "Stage details"
        '
        'lworkflow
        '
        Me.lworkflow.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.25!)
        Me.lworkflow.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lworkflow.Location = New System.Drawing.Point(8, 16)
        Me.lworkflow.Name = "lworkflow"
        Me.lworkflow.Size = New System.Drawing.Size(344, 40)
        Me.lworkflow.TabIndex = 0
        '
        'cbostages
        '
        Me.cbostages.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbostages.Location = New System.Drawing.Point(8, 25)
        Me.cbostages.Name = "cbostages"
        Me.cbostages.Size = New System.Drawing.Size(349, 20)
        Me.cbostages.TabIndex = 1
        '
        'DocDetailsTabs
        '
        Me.DocDetailsTabs.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DocDetailsTabs.Controls.Add(Me.CommentTab)
        Me.DocDetailsTabs.Controls.Add(Me.AccessTab)
        Me.DocDetailsTabs.Controls.Add(Me.SupportTab)
        Me.DocDetailsTabs.Controls.Add(Me.RevisionsTab)
        Me.DocDetailsTabs.Controls.Add(Me.HistoryTab)
        Me.DocDetailsTabs.Controls.Add(Me.LinkTab)
        Me.DocDetailsTabs.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed
        Me.DocDetailsTabs.Font = New System.Drawing.Font("Verdana", 7.25!)
        Me.DocDetailsTabs.Location = New System.Drawing.Point(24, 411)
        Me.DocDetailsTabs.Name = "DocDetailsTabs"
        Me.DocDetailsTabs.SelectedIndex = 0
        Me.DocDetailsTabs.Size = New System.Drawing.Size(384, 193)
        Me.DocDetailsTabs.TabIndex = 11
        Me.DocDetailsTabs.Visible = False
        '
        'bc_am_workflow_summary
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.AutoScroll = True
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(239, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(416, 616)
        Me.ControlBox = False
        Me.Controls.Add(Me.DocDetailsBox)
        Me.Controls.Add(Me.ActionsBox)
        Me.Controls.Add(Me.DocTitle)
        Me.Controls.Add(Me.ToggleSummaryForm)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.PinForm)
        Me.Controls.Add(Me.WorkflowBox)
        Me.Controls.Add(Me.DocDetailsTabs)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.MaximizeBox = False
        Me.Name = "bc_am_workflow_summary"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        CType(Me.CheckInDocAction, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.EditDocAction, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ViewDocAction, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ActionsBox.ResumeLayout(False)
        CType(Me.EditButtonIcon, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pattach, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CategoriseDocAction, System.ComponentModel.ISupportInitialize).EndInit()
        Me.DocDetailsBox.ResumeLayout(False)
        Me.WorkflowBox.ResumeLayout(False)
        CType(Me.ViewRevision, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RevertDocument, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ViewSupportDoc, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CheckOutSupportDoc, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CheckInSupportDoc, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pscat, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PinForm, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ToggleSummaryForm, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LinkTab.ResumeLayout(False)
        Me.ComplyLinks.ResumeLayout(False)
        Me.ReachLinks.ResumeLayout(False)
        Me.HistoryTab.ResumeLayout(False)
        Me.RevisionsTab.ResumeLayout(False)
        Me.SupportTab.ResumeLayout(False)
        Me.SupportTab.PerformLayout()
        Me.AccessTab.ResumeLayout(False)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LockedHelp, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.WriteHelp, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MoveHelp, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ReadHelp, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ViewHelp, System.ComponentModel.ISupportInitialize).EndInit()
        Me.CommentTab.ResumeLayout(False)
        Me.CommentOtherBox.ResumeLayout(False)
        Me.CommentWorkflowBox.ResumeLayout(False)
        Me.DocDetailsTabs.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub bc_am_workflow_summary_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Me.cbostages.Visible = False
            Dim intX As Integer = Screen.PrimaryScreen.Bounds.Width
            Dim intY As Integer = Screen.PrimaryScreen.Bounds.Height
            If intX = 1024 Then
                low_res = True
            End If
            no_selection()
            Me.ScrollControlIntoView(Me.DocTitle)
        Catch

        End Try
    End Sub
    Public Sub hide_all()
        Me.DocTitle.Visible = False
        Me.ActionsBox.Visible = False
        Me.DocDetailsTabs.Visible = False
        Me.DocDetailsTabs.Visible = False
        Me.DocDetailsBox.Visible = False
        Me.WorkflowBox.Visible = False
    End Sub
    Public Sub show_all()
        Me.DocTitle.Visible = True
        Me.ActionsBox.Visible = True
        Me.DocDetailsTabs.Visible = True
        Me.DocDetailsTabs.Visible = True
        Me.DocDetailsBox.Visible = True
        Me.WorkflowBox.Visible = True
    End Sub

    Public Sub no_selection()
        hide_all()
        'If low_res = True Then
        'Me.DocTitle.Rtf = "{\rtf1\ansi\ansicpg1252\deff0\deflang2057{\fonttbl{\f0\fswiss\fcharset0 Arial;}}{\*\generator Msftedit 5.41.15.1507;}\viewkind4\uc1\pard\f0\fs20 No selection\par\b\"
        Me.DocTitle.Text = "no selection"
        'Else
        'Me.DocTitle.Rtf = "{\rtf1\ansi\ansicpg1252\deff0\deflang2057{\fonttbl{\f0\fswiss\fcharset0 Arial;}}{\*\generator Msftedit 5.41.15.1507;}\viewkind4\uc1\pard\f0\fs28 No selection\par\b\"
        'End If
        Me.ViewDocAction.Visible = False
        Me.EditDocAction.Visible = False
        Me.CategoriseDocAction.Visible = False
        Me.EditButtonIcon.Visible = False
        Me.CheckInDocAction.Visible = False
        Me.EditButtonIcon.Visible = False
        Me.CheckOutSupportDoc.Visible = False
        Me.CheckInSupportDoc.Visible = False
        Me.ViewSupportDoc.Visible = False
        Me.pscat.Visible = False

        Me.RevertDocument.Visible = False
        Me.ViewRevision.Visible = False
        resetForm()
        Me.Width = FORM_WIDTH_HIDDEN
        Me.ScrollControlIntoView(Me.ToggleSummaryForm)
    End Sub

    Public Sub clear_doc()
        'If low_res = True Then
        'Me.DocTitle.Rtf = "{\rtf1\ansi\ansicpg1252\deff0\deflang2057{\fonttbl{\f0\fswiss\fcharset0 Verdana;}}{\*\generator Msftedit 5.41.15.1507;}\viewkind4\uc1\pard\f0\fs20 Loading...\par\b\"
        'Else
        'Me.DocTitle.Rtf = "{\rtf1\ansi\ansicpg1252\deff0\deflang2057{\fonttbl{\f0\fswiss\fcharset0 Verdana;}}{\*\generator Msftedit 5.41.15.1507;}\viewkind4\uc1\pard\f0\fs28 Loading...\par\b\"
        'End If
        Me.ViewDocAction.Visible = False
        Me.EditDocAction.Visible = False
        Me.CategoriseDocAction.Visible = False
        Me.EditButtonIcon.Visible = False
        Me.CheckInDocAction.Visible = False
        Me.EditButtonIcon.Visible = False
        Me.CheckOutSupportDoc.Visible = False
        Me.pscat.Visible = False
        Me.CheckInSupportDoc.Visible = False
        Me.ViewSupportDoc.Visible = False
        Me.RevertDocument.Visible = False
        Me.ViewRevision.Visible = False


        Me.SupportingDocsList.Width = DocDetailsTabs.Width




        resetForm()
    End Sub

    Public Sub setupForm(ByVal displayForm As Boolean, ByVal pinSummaryForm As Boolean)
        'If displayForm Then
        Me.Width = FORM_WIDTH_HIDDEN
        'Else
        '   Me.Width = FORM_WIDTH_HIDDEN
        'End If

        'If pinSummaryForm Then
        'formPinned = True
        'PinForm.BorderStyle = BorderStyle.Fixed3D
        'Else
        '    formPinned = False
        '    PinForm.BorderStyle = BorderStyle.None
        'End If
    End Sub

    Private Sub resetForm()
        Me.DocDetails.Clear()
        Me.AuditTrail.Items.Clear()
        Me.CategoriseDocAction.Enabled = False
        Me.lworkflow.Text = ""
        Me.lcomment.Text = ""
        Me.cbostages.Items.Clear()
        Me.cbostages.Visible = False
        Me.ViewDocAction.Image = buttonImages.Images(VIEWDOC_BUTTON_DISABLED)
        Me.EditDocAction.Image = buttonImages.Images(CHECKOUT_BUTTON_DISABLED)
        Dim omime As New bc_am_mime_types
        Me.EditButtonIcon.Image = mimeIcons.Images(1)
        Me.CategoriseDocAction.Image = buttonImages.Images(CATEGORISE_BUTTON_DISABLED)
        Me.SupportingDocsList.Items.Clear()
        Me.lrevisions.Items.Clear()
        Dim owf As New bc_am_workflow
        bc_am_workflow.cfrm.ImportMenu_SupportDoc.Enabled = False
        bc_am_workflow.cfrm.mimpreg.Enabled = False

        Me.CheckInDocAction.Visible = False

    End Sub

    'Setup form based on document selected in the bc_am_workflow_frm
    Public Sub load_doc()
        Dim slog As New bc_cs_activity_log("bc_am_workflow_summary", "load_doc", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            clear_doc()
            cs_enabled = False
            Me.CheckInDocAction.Location = New System.Drawing.Point(246, 16)
            Me.Cursor = Cursors.WaitCursor
            Dim owf As New bc_am_workflow
            bc_am_workflow.cfrm.ImportMenu_SupportDoc.Enabled = False
            bc_am_workflow.cfrm.mimpreg.Enabled = False

            REM establish if force check in can be shown
            bc_am_workflow.cfrm.madmin.Visible = False
            bc_am_workflow.cfrm.mforce.Enabled = False
            If ldoc.force_check_in = True Then
                bc_am_workflow.cfrm.madmin.Visible = True
            End If
            If ldoc.checked_out_user <> 0 Then
                bc_am_workflow.cfrm.mforce.Enabled = True
            End If
            Dim i As Integer
            Me.cbostages.Items.Clear()
            REM summary text
            Me.DocDetails.Rtf = get_rtf_text()
            ldoc.title = Trim(ldoc.title)

            Me.ViewSupportDoc.Visible = False
            Me.CheckInSupportDoc.Visible = False
            Me.CheckInSupportDoc.Visible = False

            REM history
            Me.AuditTrail.Items.Clear()
            Dim st As String
            For i = 0 To ldoc.history.Count - 1
                st = CStr(ldoc.history(i).da)
                Dim oitem As New ListViewItem(st)
                oitem.SubItems.Add(ldoc.history(i).desc)
                oitem.SubItems.Add(ldoc.history(i).comment)
                oitem.SubItems.Add(ldoc.history(i).code)
                AuditTrail.Items.Add(oitem)
            Next
            Me.ViewDocAction.Visible = False
            Me.EditDocAction.Visible = False
            Me.ViewSupportDoc.Visible = False
            load_sub_data()

            REM access rights
            If ldoc.read = True Then
                Me.ViewDocAction.Visible = True
                Me.ViewDocAction.Image = buttonImages.Images(VIEWDOC_BUTTON_ENABLED)
                Me.ViewDocAction.Visible = True
                Me.ViewSupportDoc.Visible = True
            End If
            Me.cbostages.Visible = False
            Me.CategoriseDocAction.Visible = False
            If ldoc.write = True Then
                Dim omime As New bc_am_mime_types

                If bc_am_workflow.cfrm.uxLocal.Checked Then
                    REM steve wooderson  08/07/2013 local docs
                    REM Set up for local document 

                    'Me.EditDocAction.Visible = True
                    'Me.EditDocAction.Image = buttonImages.Images(CHECKOUT_BUTTON_ENABLED)
                    'Me.EditButtonIcon.Visible = True
                    'Me.EditButtonIcon.Image = Me.mimeIcons.Images(CInt(omime.get_image_index_for_mime_type(ldoc.extension, True)))

                    REM enable import support document
                    'bc_am_workflow.cfrm.ImportMenu_SupportDoc.Enabled = True
                    'bc_am_workflow.cfrm.mreattach.Enabled = False
                    'If ldoc.extension = "" Then
                    '    bc_am_workflow.cfrm.mimpreg.Enabled = True
                    'Else
                    '    If ldoc.write = True And ldoc.checked_out_user = "0" Then
                    '        bc_am_workflow.cfrm.mreattach.Enabled = True
                    '    End If
                    'End If

                    Me.CheckInDocAction.Location = New System.Drawing.Point(112, 16)
                    Me.CheckInDocAction.Visible = True
                    Me.CheckInDocAction.Image = buttonImages.Images(CHECKIN_BUTTON_ENABLED)

                ElseIf ldoc.checked_out_user = 0 Then
                    'Checked in
                    Me.EditDocAction.Enabled = True
                    Me.EditDocAction.Visible = True
                    Me.EditDocAction.Image = buttonImages.Images(CHECKOUT_BUTTON_ENABLED)
                    Me.EditButtonIcon.Visible = True
                    Me.EditButtonIcon.Image = Me.mimeIcons.Images(CInt(omime.get_image_index_for_mime_type(ldoc.extension, True)))
                    REM enable import support document
                    bc_am_workflow.cfrm.ImportMenu_SupportDoc.Enabled = True
                    bc_am_workflow.cfrm.mreattach.Enabled = False
                    If ldoc.extension = "" Then
                        bc_am_workflow.cfrm.mimpreg.Enabled = True
                    Else
                        If ldoc.write = True And ldoc.checked_out_user = "0" Then
                            bc_am_workflow.cfrm.mreattach.Enabled = True
                        End If
                    End If

                    Me.CheckInDocAction.Visible = False
                    Me.CategoriseDocAction.Image = buttonImages.Images(CATEGORISE_BUTTON_ENABLED)
                    Me.CategoriseDocAction.Visible = True
                    Me.CategoriseDocAction.Enabled = True
                Else
                    If ldoc.checked_out_user = bc_cs_central_settings.logged_on_user_id Then
                        'Checked out to current user
                        Me.EditDocAction.Enabled = True
                        Me.EditDocAction.Visible = True
                        Me.EditButtonIcon.Image = Me.mimeIcons.Images(CInt(omime.get_image_index_for_mime_type(ldoc.extension, True)))
                        Me.EditButtonIcon.Visible = True
                        Me.CategoriseDocAction.Image = buttonImages.Images(CATEGORISE_BUTTON_ENABLED)
                        Me.CheckInDocAction.Visible = True
                        Me.CheckInDocAction.Image = buttonImages.Images(CHECKIN_BUTTON_ENABLED)
                        Me.CategoriseDocAction.Visible = False
                    Else
                        'Checked out to other user
                        Me.CheckInDocAction.Visible = False
                        Me.EditDocAction.Visible = False
                        Me.EditButtonIcon.Visible = False
                        Me.CategoriseDocAction.Image = buttonImages.Images(CATEGORISE_BUTTON_DISABLED)
                        Me.EditButtonIcon.Image = Me.mimeIcons.Images(CInt(omime.get_image_index_for_mime_type(ldoc.extension, False)))
                        Me.CategoriseDocAction.Visible = False
                    End If
                End If
            End If

                workflow_enabled = False
                CurrentStageColor.BackColor = bc_am_workflow.colors.workflow_current_stage_backcolor
                NextStageColor.BackColor = bc_am_workflow.colors.workflow_next_stage_backcolor

                If ldoc.move = True And ldoc.checked_out_user = "0" Then
                    REM if support docs checked out can't move
                    sdoc = False
                    For i = 0 To ldoc.support_documents.document.Count - 1
                        If ldoc.support_documents.document(i).checked_out_user <> "0" Then
                            sdoc = True
                            Me.lcomment.Text = "Support Document(s) checked out so can't change stage."
                            Exit For
                        End If
                    Next
                    If sdoc = False Then
                        workflow_enabled = True
                        REM if has move rights then allow workflow
                        REM Me.cbostages.Visible = True

                        Me.cbostages.Items.Clear()
                        REM workflow
                        Try
                            If ldoc.reject = True Then
                                Me.cbostages.Items.Add(ldoc.workflow_stages.stages(0).stage_name)
                                Me.cbostages.Items.Add(ldoc.reject_stage_name)
                                ldoc.workflow_stages.stages(1).stage_id = ldoc.reject_stage
                                ldoc.workflow_stages.stages(1).stage_name = ldoc.reject_stage_name
                                ldoc.workflow_stages.stages(1).action_ids.clear()
                                ldoc.workflow_stages.stages(1).num_approvers = 0
                            Else
                                For i = 0 To ldoc.workflow_stages.stages.Count - 1
                                    REM stage routes
                                    If ldoc.workflow_stages.stages(i).stage_type = "F" And ldoc.stage = 1 Then
                                        REM fast track
                                        Me.cbostages.Items.Add("Fast Track")
                                    Else
                                        Me.cbostages.Items.Add(ldoc.workflow_stages.stages(i).stage_name)
                                    End If
                                Next
                            End If
                        Catch
                            Me.cbostages.Items.Clear()
                            For i = 0 To ldoc.workflow_stages.stages.Count - 1
                                Me.cbostages.Items.Add(ldoc.workflow_stages.stages(i).stage_name)
                            Next
                        End Try
                        REM first stage is current stage
                        If ldoc.workflow_stages.stages.Count > 0 Then
                            Me.cbostages.SelectedIndex = 0
                        Else
                            Me.cbostages.Visible = False
                        End If
                    End If

                End If

                Me.DocDetailsTabs.Visible = True
                Dim od As System.Drawing.Size
                od.Height = 160
                od.Width = Me.SupportingDocsList.Size.Width
                Me.SupportingDocsList.Size = od
                populate_tree()
                Me.Workflow.CollapseAll()
                highlight_current()
                Me.ViewSupportDoc.Visible = False
                Me.CheckOutSupportDoc.Visible = False
                Me.pscat.Visible = False
                Me.CheckInSupportDoc.Visible = False
                Me.ViewRevision.Visible = False
                Me.RevertDocument.Visible = False
                Me.WorkflowBox.Visible = True
                Me.lregister.Visible = False
                Me.Ltopreg.Visible = False
                Me.lattach.Visible = False
                Me.Workflow.Visible = True
                Me.Label2.Visible = True
                Me.CurrentStageColor.Visible = True
                Me.NextStageColor.Visible = True
                Me.pattach.Visible = False
                REM if document has only been registered disable bits
                If ldoc.extension = "" Then
                    Me.lregister.Visible = True
                    Me.Ltopreg.Visible = True
                    Me.lattach.Visible = True
                    Me.pattach.Visible = True
                    Me.ViewDocAction.Visible = False
                    Me.EditDocAction.Visible = False
                    Me.EditButtonIcon.Visible = False
                End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_workflow_summary", "load_doc", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            If ldoc.title.Length < 40 Then
                Me.DocTitle.Text = ldoc.title
                'If Me.low_res = True Then
                'Me.DocTitle.Rtf = "{\rtf1\ansi\ansicpg1252\deff0\deflang2057{\fonttbl{\f0\fswiss\fcharset0 Arial;}}{\*\generator Msftedit 5.41.15.1507;}\viewkind4\uc1\pard\f0\fs20\b " + ldoc.title + "\fs20"
                'Else
                'Me.DocTitle.Rtf = "{\rtf1\ansi\ansicpg1252\deff0\deflang2057{\fonttbl{\f0\fswiss\fcharset0 Arial;}}{\*\generator Msftedit 5.41.15.1507;}\viewkind4\uc1\pard\f0\fs24\b " + ldoc.title + "\fs24"
                'End If
            Else
                Me.DocTitle.Text = ldoc.title
                'If Me.low_res = True Then
                'Me.DocTitle.Rtf = "{\rtf1\ansi\ansicpg1252\deff0\deflang2057{\fonttbl{\f0\fswiss\fcharset0 Arial;}}{\*\generator Msftedit 5.41.15.1507;}\viewkind4\uc1\pard\f0\fs16\b " + ldoc.title + "\fs16 \b0"
                'Else
                'Me.DocTitle.Rtf = "{\rtf1\ansi\ansicpg1252\deff0\deflang2057{\fonttbl{\f0\fswiss\fcharset0 Arial;}}{\*\generator Msftedit 5.41.15.1507;}\viewkind4\uc1\pard\f0\fs20\b " + ldoc.title + "\fs20 \b0"
                'End If
            End If
            Dim intX As Integer = Screen.PrimaryScreen.Bounds.Width
            Dim intY As Integer = Screen.PrimaryScreen.Bounds.Height
            show_all()
            If bc_am_workflow.snapsummary = True Then
                If intX <= 1024 Then
                    bc_am_workflow.ofrm.Width = 556
                Else
                    bc_am_workflow.ofrm.Width = 856
                End If
            End If

            Me.ScrollControlIntoView(Me.DocTitle)
            Me.Cursor = Cursors.Default
            slog = New bc_cs_activity_log("bc_am_workflow_summary", "load_doc", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try


    End Sub

    'Load the different document detail tabs with extended information on the document selected
    Private Sub load_sub_data()
        loadWorkflowTab()
        loadAccessTab()
        loadSupportDocsTab()
        loadRevisionsTab()
        loadLinksTab()
    End Sub

#Region " DocDetails tab loading code "
    'Loads Workflow tab with details on the current workflow stage for the document
    Private Sub loadWorkflowTab()
        Me.lworkflow.Text = ""
        Me.lcomment.Text = ""

        If bc_am_workflow.cfrm.uxCentral.Checked Then
            Me.lworkflow.Text = "Arrived: " + Format(ldoc.arrive, "dd-MMM-yyyy HH:mm")
            Me.lworkflow.Text = Me.lworkflow.Text + "; Acknowledged: " + Format(ldoc.acknowledged, "dd-MMM-yyyy HH:mm")
            If ldoc.checked_out_user <> "0" Then
                If ldoc.checked_out_user = bc_cs_central_settings.logged_on_user_id Then
                    Me.lcomment.Text = "Document is Checked out to me." + vbCrLf + "Cannot change workflow stage."
                ElseIf sdoc = False Then
                    Me.lcomment.Text = "Document is Checked out to another person. " + vbCrLf + "Cannot change workflow stage."
                Else
                    Me.lcomment.Text = "Support Document(s) is Checked out." + vbCrLf + "Cannot change workflow stage."
                End If
            Else
                If ldoc.main_note = "" And ldoc.move = True Then
                    ldoc.main_note = "Please change stage using workflow control above"
                End If
                If ldoc.move = False Then
                    ldoc.main_note = "You do not have permission to change the stage of this document"
                End If
                If ldoc.main_note <> "" Then
                    Me.lcomment.Text = "Comment: " + ldoc.main_note
                End If
            End If
        End If

    End Sub

    'Setup Access tab with rights based on user and document
    Private Sub loadAccessTab()

        If ldoc.read = True Then
            Me.ReadRights.Checked = True
        Else
            Me.ReadRights.Checked = False
        End If
        If ldoc.move = True Then
            Me.MoveRights.Checked = True
        Else
            Me.MoveRights.Checked = False
        End If
        If ldoc.write = True Then
            Me.WriteRights.Checked = True
        Else
            Me.WriteRights.Checked = False
        End If
        If ldoc.locked = True Then
            Me.LockedRights.Checked = True
        Else
            Me.LockedRights.Checked = False
        End If
        If ldoc.force_check_in = True Then
            Me.chkforcecheckin.Checked = True
        Else
            Me.chkforcecheckin.Checked = False
        End If
    End Sub

    'Load the Support Docs tab with any support documents associated with the master document
    Private Sub loadSupportDocsTab()
        Dim i As Integer
        Me.Tsupptx.Text = ""
        For i = 0 To ldoc.support_documents.document.Count - 1
            With ldoc.support_documents.document(i)
                Dim oitem As New ListViewItem(CStr(.title))
                oitem.ImageIndex = 0
                If .checked_out_user <> "0" Then
                    Me.SupportingDocsList.SmallImageList = Me.smallicons
                    If .checked_out_user = bc_cs_central_settings.logged_on_user_id Then
                        oitem.ImageIndex = 2
                    Else
                        oitem.ImageIndex = 1
                    End If
                Else
                    Dim omime As New bc_am_mime_types
                    oitem.ImageIndex = CHECKED_IN
                End If
                oitem.SubItems.Add(CStr(.id))
                oitem.SubItems.Add(CStr(.doc_date))
                oitem.SubItems.Add(.extension)
                Dim j As Integer
                If .checked_out_user <> "0" Then
                    If .checked_out_user = bc_cs_central_settings.logged_on_user_id Then
                        oitem.SubItems.Add("me")
                    Else
                        For j = 0 To bc_am_load_objects.obc_users.user.Count - 1
                            If .checked_out_user = CStr(bc_am_load_objects.obc_users.user(j).id) Then
                                oitem.SubItems.Add(bc_am_load_objects.obc_users.user(j).first_name + " " + bc_am_load_objects.obc_users.user(j).surname)
                                Exit For
                            End If
                        Next
                    End If
                Else
                    oitem.SubItems.Add("in")
                End If

                SupportingDocsList.Items.Add(oitem)
            End With
        Next
    End Sub

    'Load any earlier revisions of the selected master document
    Private Sub loadRevisionsTab()
        Me.lrevisions.Items.Clear()
        Dim st, fn, na, b, f As String
        Dim i As Integer
        Try
            For i = 0 To ldoc.history.Count - 1
                If ldoc.history(i).code = "R" Then
                    With ldoc.history(i)
                        Dim oitem As New ListViewItem(CStr(.da))
                        Dim s As String
                        f = InStr(.desc, "file:", CompareMethod.Text)
                        s = .desc
                        st = s.Substring(22, f - 23)
                        b = InStr(.desc, "by:", CompareMethod.Text)
                        fn = s.Substring(f + 5, b - (f + 6) - 1)
                        na = s.Substring(b + 2, Len(s) - (b + 2))
                        oitem.SubItems.Add(st)
                        oitem.SubItems.Add(fn)
                        oitem.SubItems.Add(na)
                        Me.lrevisions.Items.Add(oitem)
                    End With
                End If
            Next
        Catch

        End Try
    End Sub

    'Load Links tab with any links to other parts of Blue Curve application
    Private Sub loadLinksTab()
        REM if document has mailing list
        Me.NoLinksLbl.Visible = True
        Me.ReachLinks.Visible = False
        Me.ComplyLinks.Visible = False
        Me.LinkLabel1.Visible = False
        Me.LinkLabel2.Visible = False
        If ldoc.link1 = True Then
            Me.ReachLinks.Visible = True
            Me.NoLinksLbl.Visible = False
            Me.LinkLabel1.Visible = True
            Me.LinkLabel1.Text = ldoc.link1_text
        End If
        If ldoc.link2 = True Then
            Me.ComplyLinks.Visible = True
            Me.NoLinksLbl.Visible = False
            Me.LinkLabel2.Visible = True
            Me.LinkLabel2.Text = ldoc.link2_text
        End If
    End Sub
#End Region

    'Determine if the mime type is one that can be edited
    'Private Function can_edit() As Boolean

    '    Dim omime As New bc_am_mime_types
    '    Dim i As Integer
    '    can_edit = False
    '    If omime.can_edit(ldoc.extension) Then
    '        REM is document checked in or checked out to me
    '        If ldoc.checked_out_user = 0 Then
    '            can_edit = True
    '        Else
    '            If ldoc.checked_out_user = bc_cs_central_settings.logged_on_user_id Then
    '                can_edit = True
    '            End If
    '        End If
    '    End If
    'End Function

    'Create a description of the document as RTF text 
    'Display information:
    '   State, i.e. checked in or out and if out who to
    '   Summary
    '   Expiry date for stage
    '   Sub title
    '   Publication type
    '   Language
    '   Page count
    '   Authors
    '   Entity taxonomy
    Private Function get_rtf_text() As String
        Dim summary As String
        Dim sstr As String
        Dim stitle As String = ""
        Dim da_str As String
        Dim main_note As String
        Dim pt As String
        Dim ln As String = ""
        Dim authors As String = ""
        Dim cauthors As String = ""
        Dim entities As String = ""
        Dim sentities As String = ""
        Dim co As String = ""
        Dim i, j As Integer
        Dim scopy As String = ""
        Dim pg As String

        'Workflow stage and state
        main_note = "\b Stage: \b0 \tab\tab\tab\tab\tab " + ldoc.stage_name
        If ldoc.checked_out_user <> "0" Then
            If ldoc.checked_out_user = bc_cs_central_settings.logged_on_user_id Then
                co = " (Checked out to me) "
            Else
                For i = 0 To bc_am_load_objects.obc_users.user.Count - 1
                    If bc_am_load_objects.obc_users.user(i).id = ldoc.checked_out_user Then
                        co = " \par\b Checked out to: \b0 \tab\tab " + bc_am_load_objects.obc_users.user(i).first_name + " " + bc_am_load_objects.obc_users.user(i).surname
                        REM diable edit as cant edit if checked out elsewhere
                        Me.EditDocAction.Enabled = False
                    End If
                Next
            End If
        Else
            co = " (Checked in) "
        End If
        da_str = "\par\b Date: \b0  \tab\tab\tab\tab\tab " + CStr(ldoc.doc_date)
        If CStr(ldoc.stage_expire_date <> "9-9-9999") Then
            da_str = da_str + "\par\b Complete By: \b0 \tab\tab\tab " + CStr(ldoc.stage_expire_date)
        End If

        'Get summary & title info
        main_note = main_note + co
        If ldoc.summary <> "" Then
            summary = "\par\b Summary: \b0  \par " + ldoc.summary
        End If
        If ldoc.copy_doc_text <> "" Then
            scopy = "\fs20\par " + ldoc.copy_doc_text
        End If
        If ldoc.sub_title <> "" Then
            stitle = ldoc.sub_title
        End If
        If scopy <> "" Then
            stitle = scopy + stitle
        End If

        'Add publication type name
        pt = "\par\b Publication Type: \b0 \tab\tab " + ldoc.pub_type_name

        REM get language
        For i = 0 To bc_am_load_objects.obc_pub_types.languages.Count - 1
            If bc_am_load_objects.obc_pub_types.languages(i).id = ldoc.language_id Then
                ln = "\par\b Language:\b0 \tab\tab\tab\tab " + bc_am_load_objects.obc_pub_types.languages(i).Name
                Exit For
            End If
        Next

        'Get the page count
        pg = "\par\b Pages: \b0 \tab\tab\tab\tab\tab " + CStr(ldoc.pages)

        'Authors split by originating author and contributing authors
        For i = 0 To bc_am_load_objects.obc_users.user.Count - 1
            If bc_am_load_objects.obc_users.user(i).id = ldoc.originating_author Then
                cauthors = "\par\b Originating Author: \b0 \tab\tab " + bc_am_load_objects.obc_users.user(i).first_name + " " + bc_am_load_objects.obc_users.user(i).surname
                Exit For
            End If
        Next
        Dim first As Boolean = True
        For i = 0 To bc_am_load_objects.obc_users.user.Count - 1
            For j = 0 To ldoc.authors.Count - 1
                If bc_am_load_objects.obc_users.user(i).id = ldoc.authors(j).id And ldoc.authors(j).id <> ldoc.originating_author Then
                    If first = True Then
                        authors = authors + "\par\b Other Authors: \b0 \tab\tab\tab " + bc_am_load_objects.obc_users.user(i).first_name + " " + bc_am_load_objects.obc_users.user(i).surname
                        first = False
                    Else
                        authors = authors + "; " + bc_am_load_objects.obc_users.user(i).first_name + " " + bc_am_load_objects.obc_users.user(i).surname
                    End If
                    Exit For
                End If
            Next
        Next

        'Related entities
        If ldoc.entity_id <> 0 Then
            For i = 0 To bc_am_load_objects.obc_entities.entity.Count - 1
                If bc_am_load_objects.obc_entities.entity(i).id = ldoc.entity_id Then
                    entities = "\par\b Master Classification: \b0 \tab " + bc_am_load_objects.obc_entities.entity(i).name
                    Exit For
                End If
            Next
        End If
        first = True
        REM get class of master classficiation
        Dim master_class_id As Long
        For i = 0 To bc_am_load_objects.obc_entities.entity.Count - 1
            If bc_am_load_objects.obc_entities.entity(i).id = ldoc.entity_id Then
                master_class_id = bc_am_load_objects.obc_entities.entity(i).class_id
            End If
        Next
        For i = 0 To bc_am_load_objects.obc_entities.entity.Count - 1
            For j = 0 To ldoc.taxonomy.Count - 1
                If bc_am_load_objects.obc_entities.entity(i).id = ldoc.taxonomy(j).entity_id And bc_am_load_objects.obc_entities.entity(i).id <> ldoc.entity_id And bc_am_load_objects.obc_entities.entity(i).class_id = master_class_id Then
                    If first = True Then
                        sentities = "\par\b Other Lead Classification: \b0 \tab \tab " + Trim(bc_am_load_objects.obc_entities.entity(i).name)
                        first = False
                    Else
                        sentities = sentities + ";" + Trim(bc_am_load_objects.obc_entities.entity(i).name)
                    End If
                    Exit For
                End If
            Next

        Next

        'Now create the final rtf string
        If ldoc.sub_title <> "" Then
            If low_res = True Then
                sstr = "{\rtf1\ansi\ansicpg1252\deff0\deflang2057\deftab360{\fonttbl{\f0\fswiss\fcharset0 Verdana;}}{\*\generator Msftedit 5.41.15.1507;}\viewkind4\uc1\pard\f0\fs15\b " + "Sub Title:  \tab\tab\tab\tab " + stitle + " \fs15 \b0 \tab\tab"

            Else
                sstr = "{\rtf1\ansi\ansicpg1252\deff0\deflang2057\deftab360{\fonttbl{\f0\fswiss\fcharset0 Verdana;}}{\*\generator Msftedit 5.41.15.1507;}\viewkind4\uc1\pard\f0\fs20\b " + "Sub Title:  \tab\tab\tab\tab " + stitle + " \fs20 \b0 \tab\tab"
            End If
            sstr = sstr + "\par " + main_note
        Else
            If low_res = True Then
                sstr = "{\rtf1\ansi\ansicpg1252\deff0\deflang2057\deftab360{\fonttbl{\f0\fswiss\fcharset0 Verdana;}}{\*\generator Msftedit 5.41.15.1507;}\viewkind4\uc1\pard\f0\fs15 " + main_note + "\fs15"
            Else
                sstr = "{\rtf1\ansi\ansicpg1252\deff0\deflang2057\deftab360{\fonttbl{\f0\fswiss\fcharset0 Verdana;}}{\*\generator Msftedit 5.41.15.1507;}\viewkind4\uc1\pard\f0\fs20 " + main_note + "\fs20"
            End If
        End If
        sstr = sstr + pt + da_str + ln + pg + cauthors + authors + entities + sentities + other_taxonomy()
        other_taxonomy()
        'da_str + _
        'summary + _
        'pt + _
        'ln + _
        'pg + _
        'authors + _
        'entities
        get_rtf_text = sstr
    End Function
    Private Function other_taxonomy()
        Dim i, k, l, m As Integer
        Dim first As Boolean
        Dim str As String = ""
        Dim prev_class As String = ""
        REM classes
        For k = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
            If bc_am_load_objects.obc_pub_types.pubtype(k).id = ldoc.pub_type_id Then
                For m = 0 To ldoc.taxonomy.Count - 1
                    If prev_class = "" Or prev_class <> ldoc.taxonomy(m).class_Name Then
                        first = True
                    End If
                    If first = True Then
                        If Len(ldoc.taxonomy(m).class_name) >= 12 Then
                            str = str + "\par\b " + Trim(ldoc.taxonomy(m).class_Name) + " \b0\tab\tab\tab  " + ldoc.taxonomy(m).name
                        ElseIf Len(ldoc.taxonomy(m).class_name) > 6 Then
                            str = str + "\par\b " + Trim(ldoc.taxonomy(m).class_Name) + " \b0\tab\tab\tab\tab  " + ldoc.taxonomy(m).name
                        Else
                            str = str + "\par\b " + Trim(ldoc.taxonomy(m).class_Name) + " \b0\tab\tab\tab\tab\tab  " + ldoc.taxonomy(m).name
                        End If
                    Else
                        str = str + "; " + ldoc.taxonomy(m).name
                    End If
                    If ldoc.taxonomy(m).system_set = True Then
                        str = str + "*"
                    End If
                    first = False
                    prev_class = ldoc.taxonomy(m).class_Name
                Next
                Exit For
            End If
        Next
        REM lists
        For l = 0 To bc_am_load_objects.obc_pub_types.pubtype(k).lists.count - 1
            first = True
            For m = 0 To ldoc.list_items.list_items.Count - 1
                If bc_am_load_objects.obc_pub_types.pubtype(k).lists(l).id = ldoc.list_items.list_items(m).list_id Then
                    For i = 0 To bc_am_load_objects.obc_pub_types.pubtype(k).lists(l).items.count - 1
                        If bc_am_load_objects.obc_pub_types.pubtype(k).lists(l).items(i).item_id = ldoc.list_items.list_items(m).item_id Then
                            If first = True Then
                                If Len(bc_am_load_objects.obc_pub_types.pubtype(k).lists(l).name) >= 12 Then
                                    str = str + "\par\b " + Trim(bc_am_load_objects.obc_pub_types.pubtype(k).lists(l).name) + " \b0\tab\tab\tab  " + bc_am_load_objects.obc_pub_types.pubtype(k).lists(l).items(i).name
                                ElseIf Len(bc_am_load_objects.obc_pub_types.pubtype(k).lists(l).name) > 6 Then
                                    str = str + "\par\b " + Trim(bc_am_load_objects.obc_pub_types.pubtype(k).lists(l).name) + " \b0\tab\tab\tab\tab  " + bc_am_load_objects.obc_pub_types.pubtype(k).lists(l).items(i).name

                                Else
                                    str = str + "\par\b " + Trim(bc_am_load_objects.obc_pub_types.pubtype(k).lists(l).name) + " \b0\tab\tab\tab\tab\tab  " + bc_am_load_objects.obc_pub_types.pubtype(k).lists(l).items(i).name
                                End If
                            Else
                                str = str + "; " + bc_am_load_objects.obc_pub_types.pubtype(k).lists(l).items(i).name
                            End If
                            first = False
                            Exit For
                        End If
                    Next
                End If
            Next
        Next
        other_taxonomy = str
    End Function


    Private Sub bc_am_workflow_summary_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.SizeChanged

        Try
            bc_am_workflow.ofrm.Width = bc_am_workflow.cfrm.Width - Me.Width - 10
        Catch

        End Try

    End Sub

#Region " Button implementations "
    Private Sub EditDocAction_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles EditDocAction.MouseDown
        Try
            Me.Cursor = Cursors.WaitCursor
            animateButton(EditDocAction)
            Dim owf As New bc_am_workflow
            REM if imported document check out 

            REM SW cope with office versions
            If Len(ldoc.extension) > 5 Or (ldoc.extension <> ".doc" And ldoc.extension <> ".docx" And ldoc.extension <> ".ppt" And ldoc.extension <> ".pptx") Then
                owf.checkout_master_doc(ldoc)
            Else
                REM author tool document
                owf.open_doc(ldoc)
            End If

        Catch

        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub ViewDocAction_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ViewDocAction.MouseDown
        Try

            animateButton(ViewDocAction)


            Dim t As New Thread(AddressOf do_view)
            t.Start()



            Me.Refresh()
        Catch

        End Try
    End Sub

    Private Sub do_view()
        Dim owf As New bc_am_workflow
        If Len(ldoc.extension) > 5 Or (ldoc.extension <> ".doc" And ldoc.extension <> ".docx" And ldoc.extension <> ".ppt" And ldoc.extension <> ".pptx") Then
            owf.look_at_support_doc(ldoc.id, ldoc, True)
        Else
            owf.view_doc(ldoc)
        End If
    End Sub

    Private Sub ViewDocHistorical_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ViewSupportDoc.MouseDown
        animateButton(ViewSupportDoc)
        If Me.SupportingDocsList.SelectedItems.Count = 0 Then
            Exit Sub
        End If
        Dim id As Long
        id = Me.SupportingDocsList.SelectedItems(0).SubItems(1).Text()
        Dim owf As New bc_am_workflow
        owf.look_at_support_doc(id, ldoc, True)

    End Sub

    Private Sub ViewRevision_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ViewRevision.MouseDown
        Dim owf As New bc_am_workflow
        animateButton(ViewRevision)
        Try

            If Me.lrevisions.SelectedItems.Count = 0 Then
                Exit Sub
            End If
            Dim fn As String
            fn = Me.lrevisions.SelectedItems(0).SubItems(2).Text()
            bc_am_workflow.cfrm.cursor_wait()
            owf.view_revision(fn, Me.ldoc)
        Catch

        End Try
        bc_am_workflow.cfrm.cursor_default()

    End Sub

    Private Sub RevertDocument_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles RevertDocument.MouseDown
        Dim owf As New bc_am_workflow

        animateButton(RevertDocument)
        Try
            If Me.lrevisions.SelectedItems.Count = 0 Then
                Exit Sub
            End If
            Dim fn As String
            fn = Me.lrevisions.SelectedItems(0).SubItems(2).Text()
            bc_am_workflow.cfrm.cursor_wait()
            owf.revert(fn, Me.ldoc)

        Catch

        End Try
        bc_am_workflow.cfrm.cursor_default()

    End Sub

    Private Sub animateButton(ByVal button As PictureBox)
        button.BorderStyle = Windows.Forms.BorderStyle.Fixed3D
        'Add a pause so border change is visible to user
        Thread.Sleep(200)
        button.BorderStyle = Windows.Forms.BorderStyle.None
    End Sub

    Private Sub CheckInHistoricalDoc_Mousedown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles CheckInSupportDoc.MouseDown
        If Me.SupportingDocsList.SelectedItems.Count = 0 Then
            Exit Sub
        End If
        animateButton(CheckInSupportDoc)

        Dim owf As New bc_am_workflow
        Dim id As Long
        id = Me.SupportingDocsList.SelectedItems(0).SubItems(1).Text()
        owf.check_in(id, False, ldoc)
    End Sub

    Private Sub CheckInDocAction_Mousedown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles CheckInDocAction.MouseDown

        animateButton(CheckInDocAction)

        Dim owf As New bc_am_workflow
        owf.set_server_status("Requesting...")
        owf.check_in(ldoc.id, True, ldoc)
        owf.set_server_status("")

    End Sub

    Private Sub CheckOutHistoricalDoc_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles CheckOutSupportDoc.MouseDown
        If Me.SupportingDocsList.SelectedItems.Count = 0 Then
            Exit Sub
        End If
        animateButton(CheckOutSupportDoc)
        Dim id As Long
        id = Me.SupportingDocsList.SelectedItems(0).SubItems(1).Text()
        Dim owf As New bc_am_workflow
        owf.look_at_support_doc(id, ldoc, False)

    End Sub

    Private Sub CategoriseDocAction_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles CategoriseDocAction.MouseDown
        animateButton(CategoriseDocAction)
        Dim owf As New bc_am_workflow
        owf.edit_metadata(Me.ldoc)
    End Sub

    Private Sub ViewHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewHelp.Click
        Me.AccessRightsHelpText.Text = "You can view the document metadata, but not  the actual document."
    End Sub

    Private Sub ReadHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReadHelp.Click
        Me.AccessRightsHelpText.Text = "You can read the document itself, but cant edit it."
    End Sub

    Private Sub MoveHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MoveHelp.Click
        Me.AccessRightsHelpText.Text = "You can change the workflow stage of the document."
    End Sub

    Private Sub WriteHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WriteHelp.Click
        Me.AccessRightsHelpText.Text = "You can edit the document."
    End Sub

    Private Sub LockedHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LockedHelp.Click
        Me.AccessRightsHelpText.Text = "The document  if edited will have locked down components if implemented."
    End Sub
#End Region

#Region " Workflow tree view code"

    Public Sub highlight_current()
        Try
            REM find current in node tree
            Dim i As Integer
            check_node(Me.Workflow.Nodes, i)
            Dim ocomm As New bc_cs_activity_log("aa", "bb", bc_cs_activity_codes.COMMENTARY, CStr(i))

            Me.cs_enabled = False
            Me.Workflow.Nodes(1).ExpandAll()
            'Me.Workflow.SelectedNode = Me.Workflow.Nodes(i)
        Catch

        Finally

        End Try
    End Sub

    Public Function check_node(ByVal onode As Object, ByRef selnode As Integer) As Integer
        Try
            Dim i, j As Integer
            For i = 0 To onode.Count - 1
                If onode(i).Text = ldoc.stage_name Then
                    onode(i).forecolor = bc_am_workflow.colors.workflow_current_stage_forecolor
                    onode(i).backcolor = bc_am_workflow.colors.workflow_current_stage_backcolor
                    onode(i).expand()
                    Me.Workflow.SelectedNode = onode(i)
                End If

                With ldoc.pub_type_workflow
                    For j = 0 To .next_stages.Count - 1
                        If onode(i).Text = .next_stages(j) Then
                            onode(i).forecolor = bc_am_workflow.colors.workflow_next_stage_forecolor
                            onode(i).backcolor = bc_am_workflow.colors.workflow_next_stage_backcolor
                            onode(i).expand()
                        End If
                    Next
                End With
                check_node(onode(i).nodes, selnode)
            Next
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

    Public Sub populate_tree()
        Dim j As Integer
        Try
            Me.Workflow.Nodes.Add(ldoc.pub_type_workflow.nstage.current_stage_name)
            Me.Workflow.Nodes.Clear()
            'Me.Workflow.Nodes.Add("Workflow (Green current stage; Orange stage can move to)")
            'Me.Workflow.Nodes(0).ForeColor = System.Drawing.Color.Black
            With ldoc.pub_type_workflow.nstage
                Me.Workflow.Nodes.Add(.current_stage_name)
                Me.Workflow.Nodes(0).ForeColor = System.Drawing.Color.Gray
                If .current_stage_name = ldoc.stage_name Then
                    For j = 0 To .routes.Count - 1
                        .routes(j).next_stage = True
                    Next
                    Me.Workflow.Nodes(0).Expand()
                End If
                add_node(Me.Workflow.Nodes(0), .routes)
            End With

        Catch ex As Exception

        End Try
    End Sub

    Public Sub add_node(ByVal start_node As Object, ByVal lstage As ArrayList)
        Dim i As Integer
        Dim ncount As Integer
        ncount = 0
        Try
            For i = 0 To lstage.Count - 1

                If lstage(i).duplicate = False Then
                    start_node.nodes.Add(lstage(i).current_stage_name)
                    start_node.nodes(ncount).forecolor = System.Drawing.Color.Gray
                    add_node(start_node.nodes(ncount), lstage(i).routes)
                    ncount = ncount + 1
                End If
            Next
        Catch ex As Exception

        End Try
    End Sub

    Private Sub cbostages_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbostages.SelectedIndexChanged
        Dim owf As New bc_am_workflow
        Dim i As Integer
        Dim tpoll As Boolean
        Dim ta As Integer
        Dim na As Integer
        Try
            tpoll = bc_am_workflow.polling_enabled
            bc_am_workflow.polling_enabled = False

            If Me.cbostages.SelectedIndex > 0 Then
                If ldoc.workflow_stages.stages(Me.cbostages.SelectedIndex).num_approvers > 1 Then
                    Dim adoc As New bc_om_document
                    adoc.approval_only = True
                    adoc.approved_by = bc_cs_central_settings.logged_on_user_id
                    adoc.original_stage = ldoc.stage
                    adoc.stage = ldoc.workflow_stages.stages(Me.cbostages.SelectedIndex).stage_id
                    adoc.stage_name = ldoc.workflow_stages.stages(Me.cbostages.SelectedIndex).stage_name
                    adoc.id = ldoc.id
                    ta = ldoc.workflow_stages.stages(Me.cbostages.SelectedIndex).num_approvers
                    na = ldoc.workflow_stages.stages(Me.cbostages.SelectedIndex).approved_by.count
                    REM check current user hasnt already aspproved document

                    REM SIM MAY 2013

                    REM check if already approved
                    For i = 0 To ldoc.workflow_stages.stages(Me.cbostages.SelectedIndex).approved_by.count - 1
                        If CStr(bc_cs_central_settings.logged_on_user_id) = CStr(ldoc.workflow_stages.stages(Me.cbostages.SelectedIndex).approved_by(i)) Then
                            Dim omessage As New bc_cs_message("Blue Curve - Process", "You have already approved this document to this stage!" + CStr(ta - na) + " other approver(s) still required.", bc_cs_message.MESSAGE)
                            Me.cbostages.SelectedIndex = 0
                            Exit Sub
                        End If
                    Next
                    REM check if author can approve
                    Dim allow_approval As Boolean = False
                    Dim approver_role_found As Boolean = False
                    REM author can approve and no specific role
                    If ldoc.workflow_stages.stages(Me.cbostages.SelectedIndex).author_approval <> 1 And ldoc.workflow_stages.stages(Me.cbostages.SelectedIndex).approval_role = -1 Then
                        allow_approval = True
                    ElseIf ldoc.workflow_stages.stages(Me.cbostages.SelectedIndex).author_approval = 1 And ldoc.originating_author = bc_cs_central_settings.logged_on_user_id Then
                        REM author cannot approve his own document
                        Dim omessage As New bc_cs_message("Blue Curve - Process", "This user is the author, and is unable to approve this report." + CStr(ta - na) + " other approver(s) still required.", bc_cs_message.MESSAGE)
                        Me.cbostages.SelectedIndex = 0
                        Exit Sub
                        REM check if user is in approver role
                    ElseIf ldoc.workflow_stages.stages(Me.cbostages.SelectedIndex).approval_role <> -1 Then
                        For i = 0 To bc_am_load_objects.obc_prefs.secondary_roles.Count - 1
                            If bc_am_load_objects.obc_prefs.secondary_roles(i) = ldoc.workflow_stages.stages(Me.cbostages.SelectedIndex).approval_role Then
                                Dim omsg As New bc_cs_message("Blue Curve - Process", "The Supervisory Analyst attests to the review and approval of the substance of this research report", bc_cs_message.MESSAGE, True, False, "Approve", "Cancel", True)
                                If omsg.cancel_selected = False Then
                                    allow_approval = True
                                End If
                                approver_role_found = True
                                Exit For
                            End If
                        Next
                        If approver_role_found = False Then
                            Dim omessage As New bc_cs_message("Blue Curve - Process", "This user is not setup as an approver, and is unable to approve this report.", bc_cs_message.MESSAGE)
                            Me.cbostages.SelectedIndex = 0
                            Exit Sub
                        End If
                    End If


                    If allow_approval = True Then
                        If ldoc.workflow_stages.stages(Me.cbostages.SelectedIndex).num_approvers > ldoc.workflow_stages.stages(Me.cbostages.SelectedIndex).approved_by.count + 1 Then
                            Dim omessage As New bc_cs_message("Blue Curve - Process", CStr(ldoc.workflow_stages.stages(Me.cbostages.SelectedIndex).approved_by.count + 1) + " of " + CStr(ldoc.workflow_stages.stages(Me.cbostages.SelectedIndex).num_approvers) + " required approvers have approved this document so document can't change stage.", bc_cs_message.MESSAGE)
                            owf.approve_doc(adoc)
                            Exit Sub
                        End If
                    End If
                End If
                Dim ofrmcomment As New bc_am_workflow_params
                ofrmcomment.ldoc = ldoc
                ofrmcomment.ShowDialog()

                REM hold original stage
                ldoc.original_stage = ldoc.stage
                ldoc.original_stage_name = ldoc.stage_name
                ldoc.stage = ldoc.workflow_stages.stages(Me.cbostages.SelectedIndex).stage_id
                ldoc.stage_name = ldoc.workflow_stages.stages(Me.cbostages.SelectedIndex).stage_name
                ldoc.action_Ids.Clear()
                ldoc.action_Ids = ldoc.workflow_stages.stages(Me.cbostages.SelectedIndex).action_ids
                owf.new_stage = ldoc.stage
                owf.new_stage_name = ldoc.stage_name
                REM reject 
                If ldoc.reject = True Then
                    ldoc.reset_reject = True
                End If
                ldoc.set_reject = False
                If ldoc.workflow_stages.stages(Me.cbostages.SelectedIndex).stage_type = "R" Then
                    owf.new_stage = ""
                    owf.new_stage = "Creating"
                    ldoc.stage = 1
                    ldoc.stage_name = "Creating"
                    ldoc.reject_stage = ldoc.original_stage
                    ldoc.reject_stage_name = ldoc.original_stage_name
                    ldoc.set_reject = True
                    ldoc.reset_reject = False
                    owf.change_stage(False, 1, "Creating", ldoc)
                Else
                    ldoc.reset_reject = True
                    owf.change_stage(False, ldoc.workflow_stages.stages(Me.cbostages.SelectedIndex).stage_id, Me.cbostages.Text, ldoc)
                End If
            End If
        Catch

        Finally
            bc_am_workflow.polling_enabled = tpoll
        End Try
    End Sub

    Private Sub Workflow_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles Workflow.AfterSelect
        Try

            If Me.cs_enabled = False Then
                Exit Sub
            End If
            REM first check of node is change stage node
            Dim st As String
            Dim i, j As Integer
            Dim ta As Integer
            Dim na As Integer
            Dim tpoll As Boolean
            Dim owf As New bc_am_workflow
            If workflow_enabled = False Then
                Dim omessage As New bc_cs_message("Blue Curve - Process", "You cannot change stage of document please check comment", bc_cs_message.MESSAGE)
                Exit Sub
            End If
            st = Me.Workflow.SelectedNode.Text
            For i = 0 To ldoc.workflow_stages.stages.Count - 1
                If ldoc.workflow_stages.stages(i).stage_name = st And ldoc.stage_name <> st Then
                    Try
                        Dim smandatory_fields As String
                        Dim file_attach As Boolean = True
                        smandatory_fields = ldoc.check_mandatory_fields(bc_am_load_objects.obc_entities, bc_am_load_objects.obc_pub_types, file_attach)
                        If smandatory_fields <> "" Then
                            Dim omessage As New bc_cs_message("Blue Curve", smandatory_fields, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                            Exit Sub
                        End If
                        If file_attach = False Then
                            Dim omessage As New bc_cs_message("Blue Curve", "File must be attached!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                            Exit Sub
                        End If
                        tpoll = bc_am_workflow.polling_enabled
                        bc_am_workflow.polling_enabled = False
                        Dim adoc As New bc_om_document
                        adoc.stage_name = ldoc.workflow_stages.stages(i).stage_name
                        If ldoc.workflow_stages.stages(i).num_approvers > 0 Then
                            adoc.approval_only = True
                            adoc.approved_by = bc_cs_central_settings.logged_on_user_id
                            adoc.original_stage = ldoc.stage
                            adoc.stage = ldoc.workflow_stages.stages(i).stage_id
                            adoc.stage_name = ldoc.workflow_stages.stages(i).stage_name
                            adoc.id = ldoc.id
                            ta = ldoc.workflow_stages.stages(i).num_approvers
                            na = ldoc.workflow_stages.stages(i).approved_by.count


                            REM SIM MAY 2013

                            REM check if already approved
                            For j = 0 To ldoc.workflow_stages.stages(i).approved_by.count - 1
                                If CStr(bc_cs_central_settings.logged_on_user_id) = CStr(ldoc.workflow_stages.stages(i).approved_by(j)) Then
                                    Dim omessage As New bc_cs_message("Blue Curve - Process", "You have already approved this document to this stage!" + CStr(ta - na) + " other approver(s) still required.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                                    Me.cbostages.SelectedIndex = 0
                                    Exit Sub
                                End If
                            Next
                            REM check if author can approve
                            Dim allow_approval As Boolean = False
                            Dim approver_role_found As Boolean = False
                            REM author can approve and no specific role
                            If ldoc.workflow_stages.stages(i).author_approval <> 1 And ldoc.workflow_stages.stages(i).approval_role <= 0 Then
                                allow_approval = True
                            ElseIf ldoc.workflow_stages.stages(i).author_approval = 1 And ldoc.originating_author = bc_cs_central_settings.logged_on_user_id Then
                                REM author cannot approve his own document
                                Dim omessage As New bc_cs_message("Blue Curve - Process", "This user is the author, and is unable to approve this report. " + CStr(ta - na) + " other approver(s) still required.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                                Me.cbostages.SelectedIndex = 0
                                Exit Sub
                                REM check if user is in approver role
                            ElseIf ldoc.workflow_stages.stages(i).approval_role > 0 Then
                                For j = 0 To bc_am_load_objects.obc_prefs.secondary_roles.Count - 1
                                    If bc_am_load_objects.obc_prefs.secondary_roles(j) = ldoc.workflow_stages.stages(i).approval_role Then
                                        Dim omsg As New bc_cs_message("Blue Curve - Process", "You are authorized to approve this report do you wish to approve?", bc_cs_message.MESSAGE, True, False, "Approve", "Cancel", True)
                                        If omsg.cancel_selected = False Then
                                            allow_approval = True
                                        Else
                                            Exit Sub
                                        End If
                                        approver_role_found = True
                                        Exit For
                                    End If
                                Next
                                If approver_role_found = False Then
                                    Dim omessage As New bc_cs_message("Blue Curve - Process", "This user is not setup as an approver, and is unable to approve this report.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                                    Me.cbostages.SelectedIndex = 0
                                    Exit Sub
                                End If
                            End If


                            If allow_approval = True Then
                                If ldoc.workflow_stages.stages(i).num_approvers > ldoc.workflow_stages.stages(i).approved_by.count + 1 Then
                                    Dim omessage As New bc_cs_message("Blue Curve - Process", CStr(ldoc.workflow_stages.stages(i).approved_by.count + 1) + " of " + CStr(ldoc.workflow_stages.stages(i).num_approvers) + " required approvers have approved this document so document can't change stage.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                                    owf.approve_doc(adoc)
                                    Exit Sub
                                End If
                            End If

                        End If
                        Dim ofrmcomment As New bc_am_workflow_params
                        ofrmcomment.ldoc = ldoc
                        ofrmcomment.lcomment.Text = "Comment for users assigned to next stage: " + adoc.stage_name
                        ofrmcomment.ShowDialog()
                        If ofrmcomment.ok_selected = False Then
                            Exit Sub
                        End If

                        REM hold original stage
                        ldoc.original_stage = ldoc.stage
                        ldoc.original_stage_name = ldoc.stage_name
                        ldoc.stage = ldoc.workflow_stages.stages(i).stage_id
                        ldoc.stage_name = ldoc.workflow_stages.stages(i).stage_name
                        ldoc.action_Ids.Clear()
                        ldoc.action_Ids = ldoc.workflow_stages.stages(i).action_ids
                        owf.new_stage = ldoc.stage
                        owf.new_stage_name = ldoc.stage_name
                        REM reject 
                        If ldoc.reject = True Then
                            ldoc.reset_reject = True
                        End If
                        ldoc.set_reject = False
                        If ldoc.workflow_stages.stages(i).stage_type = "R" Then
                            owf.new_stage = ""
                            owf.new_stage = "Creating"
                            ldoc.stage = 1
                            ldoc.stage_name = "Creating"
                            ldoc.reject_stage = ldoc.original_stage
                            ldoc.reject_stage_name = ldoc.original_stage_name
                            ldoc.set_reject = True
                            ldoc.reset_reject = False
                            owf.change_stage(False, 1, "Creating", ldoc)
                        Else
                            ldoc.reset_reject = True
                            owf.change_stage(False, ldoc.workflow_stages.stages(i).stage_id, Me.cbostages.Text, ldoc)
                        End If
                    Catch

                    Finally
                        bc_am_workflow.polling_enabled = tpoll
                    End Try
                End If

            Next
        Catch

        Finally
            Me.ScrollControlIntoView(Me.ToggleSummaryForm)
        End Try

    End Sub

#End Region

#Region "DocDetails list control event handlers  "
    'Re-order the Audit trail based on user selecting a column
    Private Sub AuditTrail_ColumnClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles AuditTrail.ColumnClick
        Dim slog = New bc_cs_activity_log("bc_am_workflow_summary", "Audit_trail_ColumnClick", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            column = CInt(e.Column.ToString)

            Select Case CInt(e.Column.ToString)

                Case 1, 2, 3 ' title

                    AuditTrail.ListViewItemSorter() = New CompareByname

                    If CompareByname.toggle = False Then
                        CompareByname.toggle = True
                    Else
                        CompareByname.toggle = False
                    End If

                Case 0   ' date

                    AuditTrail.ListViewItemSorter() = New CompareBydate

                    If CompareBydate.toggle = False Then
                        CompareBydate.toggle = True
                    Else
                        CompareBydate.toggle = False
                    End If

            End Select

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_workflow_summary", "Audit_trail_ColumnClick", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            slog = New bc_cs_activity_log("bc_am_workflow_summary", "Audit_trail_ColumnClick", bc_cs_activity_codes.TRACE_EXIT, "")

        End Try
    End Sub

    'Set the images on the picture box buttons for the actions available on a selected support document
    Private Sub SupportingDocsList_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles SupportingDocsList.MouseUp

        Me.CheckOutSupportDoc.Enabled = False
        Me.CheckInSupportDoc.Enabled = False
        Me.pscat.Visible = False
        Try
            If ldoc.read = True Then
                Me.ViewSupportDoc.Image = actionImages.Images(VIEWDOC_ENABLED)
            Else
                Me.ViewSupportDoc.Image = actionImages.Images(VIEWDOC_DISABLED)
            End If
            Me.ViewSupportDoc.Visible = False
            Me.CheckOutSupportDoc.Visible = False
            Me.pscat.Visible = False
            Me.CheckInSupportDoc.Visible = False
            Me.ViewSupportDoc.Enabled = True
            Me.CheckOutSupportDoc.Enabled = True
            Me.CheckInSupportDoc.Enabled = True

            Dim omime As New bc_am_mime_types
            Dim selectedMimeType As String
            selectedMimeType = Me.SupportingDocsList.SelectedItems(0).SubItems(3).Text
            If ldoc.read = True Then
                Me.ViewSupportDoc.Visible = True
            End If
            If ldoc.write = True Then
                Me.CheckOutSupportDoc.Image = mimeIcons.Images(CInt(omime.get_image_index_for_mime_type(selectedMimeType, True)))
                Me.CheckOutSupportDoc.Visible = True
            Else
                Me.CheckOutSupportDoc.Image = mimeIcons.Images(CInt(omime.get_image_index_for_mime_type(selectedMimeType, False)))
                'Me.CheckOutSupportDoc.Enabled = False
            End If
            If Me.SupportingDocsList.SelectedItems(0).SubItems(4).Text() <> "me" And Me.SupportingDocsList.SelectedItems(0).SubItems(4).Text() <> "in" Then
                Me.CheckOutSupportDoc.Visible = False
            End If
            Me.CheckInSupportDoc.Visible = False

            REM only show check in if support doc is checked out to current user
            If Me.SupportingDocsList.SelectedItems(0).SubItems(4).Text() = "me" Then
                Me.CheckInSupportDoc.Visible = True
                'Me.CheckInSupportDoc.Image = actionImages.Images(CHECKIN_BUTTON_ENABLED)
            Else
                Me.CheckInSupportDoc.Visible = False
                Me.pscat.Visible = True

                'Me.CheckInSupportDoc.Enabled = False
                'Me.CheckInSupportDoc.Image = actionImages.Images(CHECKIN_BUTTON_DISABLED)
            End If
            Me.Tsupptx.Text = ldoc.support_documents.document(Me.SupportingDocsList.SelectedItems(0).Index).summary
            Me.SupportingDocsList.Width = DocDetailsTabs.Width - 60


        Catch ex As Exception

        End Try


    End Sub

    'Set the images on the picture box buttons for the actions available on a selected revision
    Private Sub Lrevisions_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lrevisions.MouseUp
        Dim st As String
        Dim i As Integer
        Try
            Me.ViewRevision.Enabled = True
            Me.RevertDocument.Enabled = True

            Me.ViewRevision.Visible = False
            Me.RevertDocument.Visible = False

            REM firstly can only view revisions if user has read permission on this stage            
            st = Me.lrevisions.SelectedItems(0).SubItems(1).Text()
            If ldoc.read = True Then
                REM can only view revisions of documents in user has read permission on the stage of the document
                For i = 0 To bc_am_load_objects.obc_prefs.stage_role_access_codes.Count - 1
                    If Trim(st) = Trim(bc_am_load_objects.obc_prefs.stage_role_access_codes(i).stage_name) Then
                        If UCase(bc_am_load_objects.obc_prefs.stage_role_access_codes(i).access_id) = "R" Then
                            Me.ViewRevision.Visible = True
                            Me.ViewRevision.Image = actionImages.Images(VIEWDOC_ENABLED)
                            Exit For
                        Else
                            Me.ViewRevision.Visible = False
                            'Me.ViewRevision.Image = actionImages.Images(VIEWDOC_DISABLED)
                        End If
                    End If
                Next

            End If
            REM can only revert if have write access to current stage and can only revert to document in same stage
            REM and master document has to be checked in 
            If ldoc.write = True And ldoc.checked_out_user = "0" Then
                If Trim(st) = Trim(ldoc.stage_name) Then
                    Me.RevertDocument.Visible = True
                    Me.RevertDocument.Image = actionImages.Images(REVERT_ENABLED)
                Else
                    Me.RevertDocument.Visible = False
                    'Me.RevertDocument.Image = actionImages.Images(REVERT_DISABLED)
                End If
            End If
        Catch

        End Try
    End Sub
#End Region

    Private Sub DocDetailsTabs_DrawItem(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DrawItemEventArgs) Handles DocDetailsTabs.DrawItem

        Dim g As Graphics = e.Graphics
        Dim tp As TabPage = DocDetailsTabs.TabPages(e.Index)
        Dim brBack As Brush, brFore As Brush
        Dim sf As New StringFormat
        Dim r As New RectangleF(e.Bounds.X, e.Bounds.Y + 2, e.Bounds.Width, e.Bounds.Height - 2)

        sf.Alignment = StringAlignment.Center

        Dim strTitle As String = tp.Text

        'If the current index is the Selected Index, change the color

        'this is the background color of the tabpage
        'you could make this a stndard color for the selected page
        brBack = New SolidBrush(tp.BackColor)
        'this is the background color of the tab page
        g.FillRectangle(brBack, e.Bounds)
        brFore = New SolidBrush(Color.Black)

        g.DrawString(strTitle, DocDetailsTabs.Font, brFore, r, sf)

    End Sub

#Region " Internal classes used to order the Audit trail "
    Class CompareByname
        Implements System.Collections.IComparer
        Public Shared toggle As Boolean = False

        Function compare(ByVal x As Object, ByVal y As Object) As Integer _
            Implements System.Collections.IComparer.Compare

            Dim item1 As ListViewItem = CType(x, ListViewItem)
            Dim item2 As ListViewItem = CType(y, ListViewItem)
            If toggle = True Then
                Return String.Compare(item1.SubItems(column).Text, item2.SubItems(column).Text)
            Else
                Return String.Compare(item2.SubItems(column).Text, item1.SubItems(column).Text)
            End If
        End Function
    End Class
    Class CompareBydate
        Implements System.Collections.IComparer
        Public Shared toggle As Boolean = False

        Function compare(ByVal x As Object, ByVal y As Object) As Integer _
            Implements System.Collections.IComparer.Compare

            Dim item1 As ListViewItem = CType(x, ListViewItem)
            Dim item2 As ListViewItem = CType(y, ListViewItem)
            If toggle = True Then
                Return Date.Compare(item1.SubItems(column).Text, item2.SubItems(column).Text)
            Else
                Return Date.Compare(item2.SubItems(column).Text, item1.SubItems(column).Text)
            End If
        End Function
    End Class
#End Region

#Region " Routines for hiding/displaying the form "
    Private rollForm As Boolean
    Private unrollForm As Boolean
    Public Shared formPinned As Boolean

    Private Sub ToggleSummaryForm_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles ToggleSummaryForm.MouseEnter
        If Not is_form_displayed() Then
            If Not unrollForm Then
                unrollForm = True
                FormRoller.Start()
            End If
        Else
            If Not rollForm Then
                rollForm = True
                FormRoller.Start()
            End If
        End If
    End Sub

    Public Function is_form_displayed() As Boolean
        is_form_displayed = True
        '        If formPinned Or Me.Width = FORM_WIDTH_DISPLAYED Then is_form_displayed = True
    End Function

    'Hides or displays the form
    Private Sub FormRoller_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles FormRoller.Tick
        'Const ROLL_SPEED As Integer = 10

        'If rollForm Then
        '    If Me.Width > FORM_WIDTH_HIDDEN Then
        '        Me.Width = Me.Width - ROLL_SPEED
        '        If Me.Width < FORM_WIDTH_HIDDEN Then
        '            Me.Width = FORM_WIDTH_HIDDEN
        '            FormRoller.Stop()
        '            rollForm = False
        '        End If
        '    End If
        'ElseIf unrollForm Then
        '    If Me.Width < FORM_WIDTH_DISPLAYED Then
        '        Me.Width = Me.Width + ROLL_SPEED
        '        If Me.Width > FORM_WIDTH_DISPLAYED Then
        '            Me.Width = FORM_WIDTH_DISPLAYED
        '            FormRoller.Stop()
        '            unrollForm = False
        '        End If
        '    End If
        'End If
    End Sub

    Private Sub PinForm_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PinForm.MouseUp
        If formPinned Then
            PinForm.BorderStyle = BorderStyle.None
            formPinned = False
        Else
            PinForm.BorderStyle = BorderStyle.Fixed3D
            formPinned = True
        End If
    End Sub

    Private Sub bc_am_workflow_summary_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.MouseLeave
        'If Not formPinned And Not rollForm And Not unrollForm Then
        'UnRollFormTimer.Start()
        'End If
    End Sub

    Private Sub bc_am_workflow_summary_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.MouseEnter
        'If Not formPinned And UnRollFormTimer.Enabled Then
        'UnRollFormTimer.Stop()
        'End If
    End Sub

    Private Sub UnRollFormTimer_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles UnRollFormTimer.Tick
        'UnRollFormTimer.Stop()
        'If formPinned Then Exit Sub

        'If Not is_form_displayed() Then
        'If Not unrollForm Then
        'unrollForm = True
        'FormRoller.Start()
        'End If
        'Else
        'If Not rollForm Then
        'rollForm = True
        'FormRoller.Start()
        'End If
        'End If

    End Sub
#End Region


    Private Sub ViewDocAction_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewDocAction.Click

    End Sub

    Private Sub Label1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label1.Click

    End Sub

    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        System.Diagnostics.Process.Start(ldoc.link1_url)
    End Sub
    Private Sub LinkLabel2_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        System.Diagnostics.Process.Start(ldoc.link2_url)
    End Sub
    Private Sub DocTitle_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DocTitle.TextChanged

    End Sub

    Private Sub PinForm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PinForm.Click

    End Sub

    Private Sub EditDocAction_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EditDocAction.Click

    End Sub

    'Private Sub Workflow_Mousehoover(ByVal sender As Object, ByVal e As System.EventArgs) Handles Workflow.MouseHover
    '    cs_enabled = True
    'End Sub
    'Private Sub Workflow_Mouseenter(ByVal sender As Object, ByVal e As System.EventArgs) Handles Workflow.MouseEnter
    '    cs_enabled = True
    'End Sub

    'Private Sub Workflow_Mouseleave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Workflow.MouseLeave
    '    cs_enabled = False
    'End Sub


    Private Sub Workflow_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Workflow.MouseMove
        cs_enabled = True
    End Sub

    Private Sub SupportingDocsList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SupportingDocsList.SelectedIndexChanged

    End Sub



    Private Sub Workflow_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles Workflow.MouseEnter
        highlight_current()


    End Sub

    Private Sub ToggleSummaryForm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToggleSummaryForm.Click

    End Sub

    Private Sub bc_am_workflow_summary_MaximizedBoundsChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.MaximizedBoundsChanged

    End Sub
    Private Sub Workflow_BeforeExpand(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewCancelEventArgs) Handles Workflow.BeforeExpand

    End Sub

    Private Sub lrevisions_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lrevisions.SelectedIndexChanged

    End Sub

    Private Sub DocDetails_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DocDetails.TextChanged

    End Sub

    Private Sub pattach_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pattach.Click
        Me.pattach.BorderStyle = BorderStyle.Fixed3D
        Dim owf As New bc_am_workflow
        owf.import_registered_document(ldoc)
        Me.pattach.BorderStyle = BorderStyle.None

    End Sub

    Private Sub lattach_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lattach.Click
        Me.pattach.BorderStyle = BorderStyle.Fixed3D
        Dim owf As New bc_am_workflow
        owf.import_registered_document(ldoc)
        Me.pattach.BorderStyle = BorderStyle.None

    End Sub
    Private Sub PictureBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox1.Click
        Me.AccessRightsHelpText.Text = "You can force check in the document, overriding someone elses check out."
    End Sub

    Private Sub pscat_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pscat.MouseDown
        Me.Cursor = Cursors.WaitCursor
        animateButton(pscat)
        Dim id As Long
        id = Me.SupportingDocsList.SelectedItems(0).SubItems(1).Text()
        Dim owf As New bc_am_workflow
        owf.change_support_doc_categorisation(id, ldoc)

        Me.Cursor = Cursors.Default

    End Sub

    Private Sub CheckOutSupportDoc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckOutSupportDoc.Click

    End Sub

    Private Sub pscat_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pscat.Click

    End Sub

    Private Sub ViewSupportDoc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewSupportDoc.Click

    End Sub

    Private Sub CategoriseDocAction_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CategoriseDocAction.Click

    End Sub
End Class
