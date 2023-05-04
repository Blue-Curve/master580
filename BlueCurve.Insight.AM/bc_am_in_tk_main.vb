Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports BlueCurve.Core.AS
Imports System.Windows.Forms
Public Class bc_am_in_tk_main
    Inherits System.Windows.Forms.Form
    'Public Shared asheet As bc_om_insight_sheet
    'Public osheet As bc_om_insight_sheet
    'Public csheet As bc_om_insight_sheet
    Public selsec As Integer
    Public change As Boolean
    Public ptx As String
    Public selected_keyword As String = ""
    Public Shared insight_items As bc_om_insight_items
    Public filter_insight_items As New bc_om_insight_items
    Public from_edit As Boolean = False
    Public ofcontext As bc_am_in_context
    Public ofcs As bc_am_in_settings
    Public oret As bc_am_in_retrieval
    Public ocont As bc_am_in_assignment
    Public ouniverse As bc_am_in_universe_builder
    Public oexcel As Object
    Public oexcelapp As Object
    Public template_changed As Boolean = False
    Public settings_changed As Boolean = False
    Public config_changed As Boolean = False
    Public retrieval_changed As Boolean = False
    Public pb As bc_cs_progress
    Friend WithEvents bexport As System.Windows.Forms.MenuItem
    Friend WithEvents bexpall As System.Windows.Forms.MenuItem
    Public Shared loaded As Boolean = False

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()
        DevExpress.Skins.SkinManager.EnableFormSkins()
        DevExpress.UserSkins.BonusSkins.Register()
        DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle("Blue")
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
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents lname As System.Windows.Forms.Label
    Friend WithEvents lconn As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox3 As System.Windows.Forms.PictureBox
    Friend WithEvents blogo As System.Windows.Forms.PictureBox
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents MainMenu1 As System.Windows.Forms.MainMenu
    Friend WithEvents MenuItem1 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem2 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem3 As System.Windows.Forms.MenuItem
    Friend WithEvents uxNavBarImages As System.Windows.Forms.ImageList
    Friend WithEvents uxToolBarImages As System.Windows.Forms.ImageList
    Friend WithEvents uxGenericListImages As System.Windows.Forms.ImageList
    Friend WithEvents ListView1 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents uxToolBarMain As System.Windows.Forms.ToolBar
    Friend WithEvents MenuItem4 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem7 As System.Windows.Forms.MenuItem
    Friend WithEvents mupall As System.Windows.Forms.MenuItem
    Friend WithEvents mdiscall As System.Windows.Forms.MenuItem
    Friend WithEvents Lstatus As System.Windows.Forms.Label
    Friend WithEvents lseltmp As System.Windows.Forms.Label
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents pmain As System.Windows.Forms.Panel
    Friend WithEvents lrole As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim ListViewItem1 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem("Templates", 0)
        Dim ListViewItem2 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem("Contribution & Assign", 2)
        Dim ListViewItem3 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem("General Settings", 4)
        Dim ListViewItem4 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem("Link Options", 3)
        Dim ListViewItem5 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem("Data Retrieval", 1)
        Dim ListViewItem6 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem("Universe Builder", 5)
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_in_tk_main))
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.lrole = New System.Windows.Forms.Label()
        Me.lname = New System.Windows.Forms.Label()
        Me.lconn = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.MainMenu1 = New System.Windows.Forms.MainMenu()
        Me.MenuItem1 = New System.Windows.Forms.MenuItem()
        Me.mupall = New System.Windows.Forms.MenuItem()
        Me.mdiscall = New System.Windows.Forms.MenuItem()
        Me.bexport = New System.Windows.Forms.MenuItem()
        Me.bexpall = New System.Windows.Forms.MenuItem()
        Me.MenuItem7 = New System.Windows.Forms.MenuItem()
        Me.MenuItem2 = New System.Windows.Forms.MenuItem()
        Me.MenuItem3 = New System.Windows.Forms.MenuItem()
        Me.MenuItem4 = New System.Windows.Forms.MenuItem()
        Me.uxNavBarImages = New System.Windows.Forms.ImageList()
        Me.uxToolBarImages = New System.Windows.Forms.ImageList()
        Me.uxGenericListImages = New System.Windows.Forms.ImageList()
        Me.ListView1 = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.uxToolBarMain = New System.Windows.Forms.ToolBar()
        Me.Lstatus = New System.Windows.Forms.Label()
        Me.lseltmp = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip()
        Me.pmain = New System.Windows.Forms.Panel()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.blogo = New System.Windows.Forms.PictureBox()
        Me.PictureBox3 = New System.Windows.Forms.PictureBox()
        Me.Panel3.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.blogo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel3
        '
        Me.Panel3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel3.Controls.Add(Me.lrole)
        Me.Panel3.Controls.Add(Me.lname)
        Me.Panel3.Controls.Add(Me.lconn)
        Me.Panel3.Controls.Add(Me.PictureBox1)
        Me.Panel3.Location = New System.Drawing.Point(0, 498)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(936, 63)
        Me.Panel3.TabIndex = 41
        '
        'lrole
        '
        Me.lrole.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lrole.Location = New System.Drawing.Point(631, 8)
        Me.lrole.Name = "lrole"
        Me.lrole.Size = New System.Drawing.Size(288, 16)
        Me.lrole.TabIndex = 43
        Me.lrole.Text = "Role"
        Me.lrole.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lname
        '
        Me.lname.Font = New System.Drawing.Font("Verdana", 8.25!)
        Me.lname.Location = New System.Drawing.Point(288, 8)
        Me.lname.Name = "lname"
        Me.lname.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lname.Size = New System.Drawing.Size(288, 16)
        Me.lname.TabIndex = 42
        Me.lname.Text = "User"
        Me.ToolTip1.SetToolTip(Me.lname, "logged on user")
        '
        'lconn
        '
        Me.lconn.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lconn.Font = New System.Drawing.Font("Verdana", 8.25!)
        Me.lconn.Location = New System.Drawing.Point(300, 36)
        Me.lconn.Name = "lconn"
        Me.lconn.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lconn.Size = New System.Drawing.Size(622, 16)
        Me.lconn.TabIndex = 39
        Me.lconn.Text = "Connected"
        Me.lconn.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.ToolTip1.SetToolTip(Me.lconn, "Connection method to server")
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.White
        Me.Panel2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Panel2.Location = New System.Drawing.Point(300, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(232, 64)
        Me.Panel2.TabIndex = 44
        '
        'MainMenu1
        '
        Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem1, Me.MenuItem3})
        '
        'MenuItem1
        '
        Me.MenuItem1.Index = 0
        Me.MenuItem1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mupall, Me.mdiscall, Me.bexport, Me.bexpall, Me.MenuItem7, Me.MenuItem2})
        Me.MenuItem1.Text = "&File"
        '
        'mupall
        '
        Me.mupall.Index = 0
        Me.mupall.Text = "&Upload Changes"
        '
        'mdiscall
        '
        Me.mdiscall.Index = 1
        Me.mdiscall.Text = "&Discard Changes"
        '
        'bexport
        '
        Me.bexport.Enabled = False
        Me.bexport.Index = 2
        Me.bexport.Text = "Export Template"
        '
        'bexpall
        '
        Me.bexpall.Index = 3
        Me.bexpall.Text = "Export All"
        '
        'MenuItem7
        '
        Me.MenuItem7.Index = 4
        Me.MenuItem7.Text = "-"
        '
        'MenuItem2
        '
        Me.MenuItem2.Index = 5
        Me.MenuItem2.Text = "&Exit"
        '
        'MenuItem3
        '
        Me.MenuItem3.Index = 1
        Me.MenuItem3.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem4})
        Me.MenuItem3.Text = "&Help"
        '
        'MenuItem4
        '
        Me.MenuItem4.Index = 0
        Me.MenuItem4.Text = "&About"
        '
        'uxNavBarImages
        '
        Me.uxNavBarImages.ImageStream = CType(resources.GetObject("uxNavBarImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.uxNavBarImages.TransparentColor = System.Drawing.Color.Transparent
        Me.uxNavBarImages.Images.SetKeyName(0, "")
        Me.uxNavBarImages.Images.SetKeyName(1, "")
        Me.uxNavBarImages.Images.SetKeyName(2, "")
        Me.uxNavBarImages.Images.SetKeyName(3, "")
        Me.uxNavBarImages.Images.SetKeyName(4, "")
        Me.uxNavBarImages.Images.SetKeyName(5, "Settings.png")
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
        Me.uxToolBarImages.Images.SetKeyName(13, "none.ico")
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
        Me.uxGenericListImages.Images.SetKeyName(5, "Web.png")
        '
        'ListView1
        '
        Me.ListView1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ListView1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1})
        Me.ListView1.HideSelection = False
        Me.ListView1.Items.AddRange(New System.Windows.Forms.ListViewItem() {ListViewItem1, ListViewItem2, ListViewItem3, ListViewItem4, ListViewItem5, ListViewItem6})
        Me.ListView1.LargeImageList = Me.uxNavBarImages
        Me.ListView1.Location = New System.Drawing.Point(0, 104)
        Me.ListView1.MultiSelect = False
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(82, 394)
        Me.ListView1.TabIndex = 46
        Me.ToolTip1.SetToolTip(Me.ListView1, "Please select area of interest.")
        Me.ListView1.UseCompatibleStateImageBehavior = False
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Templates"
        '
        'uxToolBarMain
        '
        Me.uxToolBarMain.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxToolBarMain.Appearance = System.Windows.Forms.ToolBarAppearance.Flat
        Me.uxToolBarMain.AutoSize = False
        Me.uxToolBarMain.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.uxToolBarMain.ButtonSize = New System.Drawing.Size(16, 16)
        Me.uxToolBarMain.Dock = System.Windows.Forms.DockStyle.None
        Me.uxToolBarMain.DropDownArrows = True
        Me.uxToolBarMain.ImageList = Me.uxToolBarImages
        Me.uxToolBarMain.Location = New System.Drawing.Point(0, 62)
        Me.uxToolBarMain.Name = "uxToolBarMain"
        Me.uxToolBarMain.ShowToolTips = True
        Me.uxToolBarMain.Size = New System.Drawing.Size(958, 42)
        Me.uxToolBarMain.TabIndex = 47
        Me.uxToolBarMain.Wrappable = False
        '
        'Lstatus
        '
        Me.Lstatus.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Lstatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Lstatus.Location = New System.Drawing.Point(293, 40)
        Me.Lstatus.Name = "Lstatus"
        Me.Lstatus.Size = New System.Drawing.Size(555, 16)
        Me.Lstatus.TabIndex = 48
        Me.Lstatus.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.ToolTip1.SetToolTip(Me.Lstatus, "Status ")
        '
        'lseltmp
        '
        Me.lseltmp.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lseltmp.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lseltmp.Location = New System.Drawing.Point(293, 14)
        Me.lseltmp.Name = "lseltmp"
        Me.lseltmp.Size = New System.Drawing.Size(555, 16)
        Me.lseltmp.TabIndex = 51
        Me.lseltmp.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.ToolTip1.SetToolTip(Me.lseltmp, "Currenly selected template")
        '
        'pmain
        '
        Me.pmain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pmain.AutoScroll = True
        Me.pmain.BackColor = System.Drawing.Color.White
        Me.pmain.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pmain.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pmain.Location = New System.Drawing.Point(84, 104)
        Me.pmain.Name = "pmain"
        Me.pmain.Size = New System.Drawing.Size(851, 393)
        Me.pmain.TabIndex = 45
        '
        'PictureBox1
        '
        Me.PictureBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(3, 17)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(175, 40)
        Me.PictureBox1.TabIndex = 38
        Me.PictureBox1.TabStop = False
        '
        'blogo
        '
        Me.blogo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.blogo.Image = CType(resources.GetObject("blogo.Image"), System.Drawing.Image)
        Me.blogo.Location = New System.Drawing.Point(872, 1)
        Me.blogo.Name = "blogo"
        Me.blogo.Size = New System.Drawing.Size(64, 64)
        Me.blogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.blogo.TabIndex = 43
        Me.blogo.TabStop = False
        '
        'PictureBox3
        '
        Me.PictureBox3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PictureBox3.Image = CType(resources.GetObject("PictureBox3.Image"), System.Drawing.Image)
        Me.PictureBox3.Location = New System.Drawing.Point(0, 0)
        Me.PictureBox3.Name = "PictureBox3"
        Me.PictureBox3.Size = New System.Drawing.Size(936, 64)
        Me.PictureBox3.TabIndex = 42
        Me.PictureBox3.TabStop = False
        '
        'bc_am_in_tk_main
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 14)
        Me.AutoScroll = True
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(936, 560)
        Me.Controls.Add(Me.ListView1)
        Me.Controls.Add(Me.uxToolBarMain)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.lseltmp)
        Me.Controls.Add(Me.Lstatus)
        Me.Controls.Add(Me.blogo)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.PictureBox3)
        Me.Controls.Add(Me.pmain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Menu = Me.MainMenu1
        Me.Name = "bc_am_in_tk_main"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Insight Toolkit - Blue Curve"
        Me.Panel3.ResumeLayout(False)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.blogo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub bc_am_in_tk_main_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        pb = New bc_cs_progress("Blue Curve", "Initialising Insight Toolkit...", 0, False, True)

        Try

            load_begin()
            Me.mupall.Enabled = False
            Me.mdiscall.Enabled = False
            Me.TopLevel = True
            bc_am_in_tk_main.loaded = True
            ofcontext = New bc_am_in_context
            ofcontext.tk_main = Me

            ofcontext.load_start()

            pb.increment("Initialising Insight Toolkit...")
            ofcs = New bc_am_in_settings

            ofcs.tk_main = Me
            load_client_settings()
            ofcontext.TopLevel = False
            ofcontext.Parent = Me.pmain
            ofcontext.Width = Me.pmain.Width
            ofcontext.Height = Me.pmain.Height
            ofcontext.AutoScroll = True
            ofcontext.Dock = DockStyle.Fill

            ofcs.TopLevel = False
            ofcs.Parent = Me.pmain
            ofcs.Width = Me.pmain.Width
            ofcs.Height = Me.pmain.Height
            ofcs.Dock = DockStyle.Fill

            ocont = New bc_am_in_assignment
            ocont.tk_main = Me
            ocont.TopLevel = False
            ocont.Parent = Me.pmain
            ocont.Width = Me.pmain.Width
            ocont.Height = Me.pmain.Height
            ocont.AutoScroll = True
            ocont.load_contributing_classes()
            ocont.Dock = DockStyle.Fill

            oret = New bc_am_in_retrieval
            oret.tk_main = Me
            oret.TopLevel = False
            oret.Parent = Me.pmain
            oret.Width = Me.pmain.Width
            oret.Height = Me.pmain.Height
            oret.AutoScroll = True
            oret.Dock = DockStyle.Fill

            ouniverse = New bc_am_in_universe_builder
            ouniverse.tk_main = Me
            ouniverse.TopLevel = False
            ouniverse.Parent = Me.pmain
            ouniverse.Width = Me.pmain.Width
            ouniverse.Height = Me.pmain.Height
            ouniverse.Location = New System.Drawing.Point(0, 0)
            ouniverse.AutoScroll = True
            ouniverse.Dock = DockStyle.Fill

            Dim classes As New bc_om_entity_classes

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                classes.db_read()
            Else
                classes.tmode = bc_cs_soap_base_class.tREAD
                If classes.transmit_to_server_and_receive(classes, True) = False Then
                    Exit Sub
                End If
            End If


            Dim aggregations_installed As Boolean = False
            For i = 0 To classes.classes.Count - 1
                If classes.classes(i).class_type_Id = 2 Then
                    aggregations_installed = True
                    Exit For
                End If
            Next
            If aggregations_installed = False Then
                Me.ListView1.Items.RemoveAt(5)
            End If


            pb.increment("Initialising Insight Toolkit...")
            Me.ListView1.Items(0).Selected = True

            ofcontext.Show()

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_in_tk_main", "load", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            pb.unload()
        End Try

    End Sub
    Public Sub set_template(ByVal na As String)
        If na <> "" Then
            Me.lseltmp.Text = "Selected template: " + na
        Else
            Me.lseltmp.Text = ""
        End If
    End Sub

    Private Sub load_begin()
        Try
            bc_am_in_tk_main.loaded = True
            Dim i As Integer
            For i = 0 To bc_am_load_objects.obc_users.user.Count - 1
                With bc_am_load_objects.obc_users.user(i)
                    If .id = bc_cs_central_settings.logged_on_user_id Then
                        Me.lname.Text = "User: " + .first_name + " " + .surname '+ "   Role: " + .role
                        Me.lrole.Text = "Role: " + .role
                        Exit For
                    End If
                End With
            Next
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                Me.lconn.Text = "Connected via ADO to: " + bc_cs_central_settings.servername
            Else
                Me.lconn.Text = "Connected via Webservices to: " + bc_cs_central_settings.soap_server
            End If
            REM load current config settings if file exists





        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_in_tk_main", "load_begin", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
        End Try

    End Sub
    Private Sub load_client_settings()
        Try
            REM firstly attempt to get file from server and copy locally
            Dim oinsightconfig As New bc_om_insight_config_files
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                oinsightconfig.db_read()
            Else
                oinsightconfig.tmode = bc_cs_soap_base_class.tREAD
                If oinsightconfig.transmit_to_server_and_receive(oinsightconfig, True) = True Then
                    Dim fs As New bc_cs_file_transfer_services
                    If oinsightconfig.xml_file_exists = True Then
                        fs.write_bytestream_to_document_readonly(bc_cs_central_settings.local_template_path + "bc_am_insight_config.xml", oinsightconfig.xml_file, Nothing)
                    End If
                End If
            End If
            Dim oinsight3 As New bc_am_insight_parameters(False)
            If oinsight3.load_failed = False Then
                Dim kk As New bc_am_insight_formats
                REM set up dialogue with these settings
                ofcs.load_settings()
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_in_context", "load_client_settings", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

        End Try

    End Sub
    Private Sub MenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem2.Click
        If Me.mupall.Enabled = True Then
            Dim omsg As New bc_cs_message("Blue Curve", "You have uncomitted data are you sure you wish to exit?", bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
            If omsg.cancel_selected = True Then
                Exit Sub
            End If
            Me.mupall.Enabled = False
        End If
        Me.Hide()
    End Sub
    Public Sub set_status(ByVal tx As String, Optional ByVal with_progress As Boolean = True)
        ptx = ""
        If Lstatus.Text.Length > 8 Then
            If Lstatus.Text.Substring(0, 7) = "Pending" Then
                ptx = Me.Lstatus.Text
            End If
        End If
        Me.Lstatus.Text = tx
        Me.Refresh()
        Me.Lstatus.Refresh()
        Try
            bc_cs_central_settings.progress_bar.hide()
        Catch

        End Try
        If with_progress = True Then
            bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve", tx, 0, False, True)
            bc_cs_central_settings.progress_bar.increment(tx)

        End If

    End Sub
    Public Sub clear_status()
        Me.Lstatus.Text = ptx
        Me.Lstatus.Refresh()
        Try
            bc_cs_central_settings.progress_bar.hide()
        Catch

        End Try
    End Sub
    Public Sub clear_all_change()

        template_changed = False
        settings_changed = False
        config_changed = False
        retrieval_changed = False
        Me.mdiscall.Enabled = False
        Me.mupall.Enabled = False
        Me.bexpall.Enabled = True
        Me.ptx = ""
        Me.ofcontext.change = False
        clear_status()
    End Sub
    Public Sub set_change(ByVal type As Integer)
        Try
            Me.bexpall.Enabled = False
            Select Case type
                Case 0
                    template_changed = True
                Case 1
                    settings_changed = True
                Case 2
                    config_changed = True
                Case 3
                    retrieval_changed = True
            End Select
            Me.mdiscall.Enabled = True
            Me.mupall.Enabled = True

            Dim tx As String
            tx = "Pending upload:"
            If template_changed = True Then
                tx = tx + " template"
            End If
            If settings_changed = True Then
                tx = tx + " settings"
            End If
            If config_changed = True Then
                tx = tx + " assignment"
            End If
            If retrieval_changed = True Then
                tx = tx + " retrieval"
            End If
            set_status(tx, False)
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_in_tk_main", "set_change", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Private Sub load_calc_lib()
        Dim fcl As New bc_am_in_calc_lib
        Dim cs As New List(Of bc_om_contributor)
        Dim c As bc_om_contributor

        For i = 0 To bc_am_in_context.insight_items.contributor_ids.Count - 1
            c = New bc_om_contributor(bc_am_in_context.insight_items.contributor_ids(i), bc_am_in_context.insight_items.contributor_names(i))
            cs.Add(c)
        Next
        Dim ccl As New Cbc_am_in_calc_lib(fcl, cs)
        If ccl.load_data = True Then
            fcl.ShowDialog()
            If Not IsNothing(ccl.calc_lib) Then
                bc_am_in_context.insight_items.libray_calcs.libray_calculations.Clear()
                bc_am_in_context.lib_list.Clear()
                For i = 0 To ccl.calc_lib.libray_calculations.Count - 1
                    bc_am_in_context.insight_items.libray_calcs.libray_calculations.Add(ccl.calc_lib.libray_calculations(i))
                    bc_am_in_context.lib_list.Add(ccl.calc_lib.libray_calculations(i))
                Next
            End If
        End If
    End Sub
    Private Sub uxToolBarMain_ButtonClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles uxToolBarMain.ButtonClick

        Try
            Me.Cursor = Cursors.WaitCursor

            Select Case Me.ListView1.SelectedItems(0).Index
                Case 0
                    If e.Button.Text = "&Preview" Then
                        Me.ofcontext.preview()
                        Me.WindowState = FormWindowState.Minimized
                    ElseIf e.Button.Text = "&Calculations" Then
                        Me.load_calcs()
                    ElseIf e.Button.Text = "&Calc Library" Then
                        Me.load_calc_lib()
                    ElseIf e.Button.Text = "&New" Then
                        If Me.template_changed Then
                            Dim omsg As New bc_cs_message("Blue Curve", "Changes have occured to the currently selected template continuing will discard these changes Proceed without comiting changes first?", bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
                            If omsg.cancel_selected Then
                                Exit Sub
                            End If
                        End If
                        Me.ofcontext.new_template()
                    ElseIf e.Button.Text = "&Clone" Then
                        Me.ofcontext.clone_template()
                    ElseIf e.Button.Text = "&Delete" Then
                        Me.ofcontext.del_tmp()
                    ElseIf e.Button.Text = "&Import Sheet" Then
                        Me.ofcontext.import_sheet()
                    ElseIf e.Button.Text = "&Edit Sheet" Then
                        Me.ofcontext.edit_sheet()
                    ElseIf e.Button.Text = "&Remove Sheet" Then
                        Me.ofcontext.delete_sheet()
                    End If

                Case 1
                    If e.Button.Text = "&New" Then
                        load_ass()
                    ElseIf e.Button.Text = "&Delete" Then
                        del_ass()
                    ElseIf e.Button.Text = "&Assign" Then
                        ass_temp()
                    ElseIf e.Button.Text = "&Assign All" Then
                        ass_temp(True)
                    End If
                Case 2
                    If e.Button.Text = "&Preview" Then
                        Me.ofcontext.preview()
                    ElseIf e.Button.Text = "&General Options" Then
                        'Me.ofcs.uxGeneral.Height = 360
                        Me.ofcs.uxGeneral.Visible = True
                        Me.ofcs.uxLinkOps.Visible = False
                        Me.ofcs.uxLinkCodes.Visible = False
                        Me.ofcs.uxMapCodes.Visible = False
                    ElseIf e.Button.Text = "&Link Options" Then
                        Me.ofcs.uxGeneral.Visible = False
                        Me.ofcs.uxAggAudit.Visible = False
                        Me.ofcs.uxLinkOps.Location = Me.ofcs.uxGeneral.Location
                        'Me.ofcs.uxLinkOps.Height = 160
                        Me.ofcs.uxLinkOps.Visible = True
                        Me.ofcs.uxLinkCodes.Visible = True
                        Dim dp As System.Drawing.Point
                        dp.X = Me.ofcs.uxLinkOps.Location.X
                        dp.Y = 172
                        Me.ofcs.uxLinkCodes.Location = dp
                        'Me.ofcs.uxLinkCodes.Height = 160
                        dp.Y = 172 + 165
                        Me.ofcs.uxMapCodes.Location = dp
                        Me.ofcs.uxMapCodes.Visible = True
                        'Me.ofcs.uxMapCodes.Height = 128
                    ElseIf e.Button.Text = "&Create format" Then
                        create_format_sheet()
                    ElseIf e.Button.Text = "&Import format" Then
                        Me.ofcs.import_sheet()
                    ElseIf e.Button.Text = "&Edit format" Then
                        Me.ofcs.edit_sheet()
                    ElseIf e.Button.Text = "&Delete format" Then
                        Me.ofcs.delete_sheet()
                    ElseIf e.Button.Text = "&Role Security" Then
                        role_security()
                    End If
                Case 3
                    If e.Button.Text = "&Preview" Then
                        Me.ofcontext.preview()
                        Me.WindowState = FormWindowState.Minimized
                    End If
                Case 4
                    If e.Button.Text = "&Preview" Then
                        Me.ofcontext.preview()
                        Me.WindowState = FormWindowState.Minimized
                    ElseIf e.Button.Text = "&New" Then
                        ret_item(True)
                    ElseIf e.Button.Text = "&Edit" Then
                        ret_item(False)
                    ElseIf e.Button.Text = "&Delete" Then
                        del_ret_item()
                    End If
                Case 5
                    If e.Button.Text = "&New" Then
                        ouniverse.newClick()
                    ElseIf e.Button.Text = "&Save" Then
                        ouniverse.saveClick()
                    ElseIf e.Button.Text = "&Cancel" Then
                        ouniverse.cancelClick()
                    ElseIf e.Button.Text = "Activate" Then
                        ouniverse.activateClick(True)
                    ElseIf e.Button.Text = "Deactivate" Then
                        ouniverse.activateClick(False)
                    ElseIf e.Button.Text = "&Delete" Then
                        ouniverse.deleteClick()
                    ElseIf e.Button.Text = "&Calculations" Then
                        Me.load_calcs()
                    ElseIf e.Button.Text = "&Calc Library" Then
                        Me.load_calc_lib()
                    ElseIf e.Button.Text = "&Exclusions" Then
                        Dim fexcl As New bc_am_agg_exclusions
                        Dim cexcl As New bc_am_agg_exclusions_controller(fexcl)
                        If cexcl.load_exclusions_for_universe(ouniverse.boeAggregation.id, ouniverse.lvAggregations.SelectedItems(0).Text, ouniverse.classes, ouniverse.entities) = False Then
                            Exit Sub
                        End If
                        fexcl.ShowDialog()

                    ElseIf e.Button.Text = "&Hierarchy" Then
                        Dim fhier As New bc_am_agg_hier
                        Dim cexcl As New Cbc_am_agg_hier(fhier)
                        If cexcl.load_hier_for_universe(ouniverse.boeAggregation.id, ouniverse.lvAggregations.SelectedItems(0).Text, ouniverse.classes, ouniverse.entities, ouniverse.cbTargetClass.Text, ouniverse.cbtargetclass2.Text, ouniverse.cbSourceClass.Text) = False Then
                            Exit Sub
                        End If
                        fhier.ShowDialog()


                    End If
            End Select
        Catch ex As Exception
            Dim s As String = ""
        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub
    Public Sub ret_item(ByVal newitem As Boolean)
        Dim fret As New bc_am_in_tk_retrieval
        Dim i As Integer
        fret.from_edit = False
        fret.oret = oret
        If newitem = False Then
            fret.from_edit = True
        End If

        fret.ShowDialog()

        If fret.cancel_selected = True Then
            Exit Sub
        End If
        Dim lvw As New ListViewItem(fret.Citem.Text)
        If fret.rsystem.Checked = True Then
            lvw.SubItems.Add("System Sheets")
        Else
            lvw.SubItems.Add(fret.tsheet.Text)
        End If
        lvw.SubItems.Add(fret.Crow.Text)
        lvw.SubItems.Add(fret.Ccol.Text)
        lvw.SubItems.Add(fret.sscale.Text)
        lvw.SubItems.Add(fret.Cstorage.Text)
        lvw.SubItems.Add(fret.cdimension.Text)
        If fret.Corder.SelectedIndex = -1 Then
            lvw.SubItems.Add("0")
        Else
            lvw.SubItems.Add(fret.Corder.Text)
        End If
        If newitem = True Then
            Me.oret.litems.Items.Add(lvw)
        Else
            i = Me.oret.litems.SelectedItems(0).Index
            Me.oret.litems.Items.RemoveAt(i)
            Me.oret.litems.Items.Insert(i, lvw)
        End If
        set_change(3)
        If newitem = True Then
            Me.set_button_disabled(3)
            Me.set_button_disabled(4)
        End If
    End Sub
    Private Sub del_ret_item()
        Dim omsg As New bc_cs_message("Blue Curve", "Are you sure you wish to delete item " + Me.oret.litems.SelectedItems(0).Text, bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
        If omsg.cancel_selected = True Then
            Exit Sub
        End If

        Me.oret.litems.Items.RemoveAt(Me.oret.litems.SelectedItems(0).Index)
        set_change(3)
        Me.set_button_disabled(3)
        Me.set_button_disabled(4)
    End Sub
    Private Sub role_security()
        Dim fsec As New bc_am_in_tk_user_sec
        fsec.Ltitle.Text = "Select allowed data submission for role " + Me.ofcs.Lsec.SelectedItems(0).Text
        fsec.Chkdraft.Checked = False
        fsec.chkpublish.Checked = False
        fsec.rdraft.Enabled = False
        fsec.rpublish.Enabled = False

        If Me.ofcs.Lsec.SelectedItems(0).SubItems(1).Text = "Yes" Then
            fsec.Chkdraft.Checked = True
        End If
        If Me.ofcs.Lsec.SelectedItems(0).SubItems(2).Text = "Yes" Then
            fsec.chkpublish.Checked = True
        End If
        If Me.ofcs.Lsec.SelectedItems(0).SubItems(3).Text = "Draft" Then
            If fsec.Chkdraft.Checked = True Then
                fsec.rdraft.Enabled = True
                fsec.rdraft.Checked = True
            End If
        End If
        If Me.ofcs.Lsec.SelectedItems(0).SubItems(3).Text = "Publish" Then
            If fsec.chkpublish.Checked = True Then
                fsec.rpublish.Enabled = True
                fsec.rpublish.Checked = True
            End If
        End If
        fsec.bapply.Enabled = False
        fsec.ShowDialog()
        If fsec.cancel_selected = True Then
            Exit Sub
        End If
        Me.ofcs.Lsec.SelectedItems(0).SubItems(1).Text = "No"
        Me.ofcs.Lsec.SelectedItems(0).SubItems(2).Text = "No"
        Me.ofcs.Lsec.SelectedItems(0).SubItems(3).Text = "Draft"
        If fsec.Chkdraft.Checked Then
            Me.ofcs.Lsec.SelectedItems(0).SubItems(1).Text = "Yes"
        End If
        If fsec.chkpublish.Checked Then
            Me.ofcs.Lsec.SelectedItems(0).SubItems(2).Text = "Yes"
        End If
        If fsec.rpublish.Checked Then
            Me.ofcs.Lsec.SelectedItems(0).SubItems(3).Text = "Publish"
        End If
        set_change(1)
    End Sub
    Public Sub get_ass()
        REM FIL 5.6 read back on demand
        Dim oas As New bc_om_entity_template_assigments
        oas.schema_Id = bc_am_in_context.insight_items.contributing_classes(ocont.Lcclass.SelectedItems(0).Index).schema_id
        oas.granularity_class_id = bc_am_in_context.insight_items.contributing_classes(ocont.Lcclass.SelectedItems(0).Index).granularity_class_id
        oas.entity_id = bc_am_in_context.insight_items.contributing_classes(ocont.Lcclass.SelectedItems(0).Index).entity_template_assigments(ocont.lasstmp.SelectedItems(0).Index).entity_Id

        If ocont.tasstmp.SelectedIndex = 0 Then
            oas.class_id = bc_am_in_context.insight_items.contributing_classes(ocont.Lcclass.SelectedItems(0).Index).parent_class_id
        Else
            oas.class_id = bc_am_in_context.insight_items.contributing_classes(ocont.Lcclass.SelectedItems(0).Index).child_class_id
        End If

        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
            oas.db_read()
        Else
            oas.tmode = bc_cs_soap_base_class.tREAD
            If oas.transmit_to_server_and_receive(oas, True) = False Then
                Exit Sub
            End If
        End If
        bc_am_in_context.insight_items.contributing_classes(ocont.Lcclass.SelectedItems(0).Index).entity_template_assigments(ocont.lasstmp.SelectedItems(0).Index).template_Id = oas.template_id
        If oas.template_id = 0 Then
            ocont.lasstmp.SelectedItems(0).SubItems(1).Text = "not set"
        Else
            For i = 0 To bc_am_in_context.asheet.logical_templates_ids.Count - 1
                If bc_am_in_context.asheet.logical_templates_ids(i) = oas.template_id Then
                    ocont.lasstmp.SelectedItems(0).SubItems(1).Text = bc_am_in_context.asheet.logical_templates_names(i)
                    Exit For
                End If
            Next
        End If
    End Sub
    Private Sub ass_temp(Optional ByVal all As Boolean = False)
        Try
            Dim i As Integer
            Dim co As Integer
            Dim ofinput As New bc_am_in_tk_da_frm
            ofinput.Text = "Assign Template"
            ofinput.Height = 125
            ofinput.tinput.Visible = False
            ofinput.clist.Visible = True
            ofinput.L1.Visible = True
            ofinput.L1.Text = "template"
            ofinput.clist.Items.Clear()
            For i = 0 To bc_am_in_context.asheet.logical_templates_ids.Count - 1
                ofinput.clist.Items.Add(bc_am_in_context.asheet.logical_templates_names(i))
            Next
            If all = True Then
                ofinput.Ltitle.Text = "Select template to assign to all"
            Else
                ofinput.Ltitle.Text = "Select template to assign"
            End If
            If all = False Then
                If ocont.lasstmp.SelectedItems(0).SubItems(1).Text <> "not set" Then
                    ofinput.clist.Text = ocont.lasstmp.SelectedItems(0).SubItems(1).Text
                End If
            End If
            ofinput.ShowDialog()
            If ofinput.cancel_selected = True Then
                Exit Sub
            End If
            REM parent class
            If ofinput.clist.SelectedIndex = -1 Then
                Exit Sub
            End If
            REM FIL 5.6 write on demand
            Dim oas As New bc_om_entity_template_assigments
            oas.schema_Id = bc_am_in_context.insight_items.contributing_classes(ocont.Lcclass.SelectedItems(0).Index).schema_id
            oas.granularity_class_id = bc_am_in_context.insight_items.contributing_classes(ocont.Lcclass.SelectedItems(0).Index).granularity_class_id
            If ocont.tasstmp.SelectedIndex = 0 Then
                oas.class_id = bc_am_in_context.insight_items.contributing_classes(ocont.Lcclass.SelectedItems(0).Index).parent_class_id
            Else
                oas.class_id = bc_am_in_context.insight_items.contributing_classes(ocont.Lcclass.SelectedItems(0).Index).child_class_id
            End If
            If all = False Then
                oas.entity_id = bc_am_in_context.insight_items.contributing_classes(ocont.Lcclass.SelectedItems(0).Index).entity_template_assigments(ocont.lasstmp.SelectedItems(0).Index).entity_Id
            Else
                oas.entity_id = -1
            End If
            oas.template_id = bc_am_in_context.asheet.logical_templates_ids(ofinput.clist.SelectedIndex)

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                oas.db_write()
            Else
                oas.tmode = bc_cs_soap_base_class.tWRITE
                If oas.transmit_to_server_and_receive(oas, True) = False Then
                    Exit Sub
                End If
            End If

            If all = False Then
                ocont.lasstmp.SelectedItems(0).SubItems(1).Text = bc_am_in_context.asheet.logical_templates_names(ofinput.clist.SelectedIndex)

                If ocont.tasstmp.SelectedIndex = 0 Then
                    bc_am_in_context.insight_items.contributing_classes(ocont.Lcclass.SelectedItems(0).Index).entity_template_assigments(ocont.lasstmp.SelectedItems(0).Index).template_id = bc_am_in_context.asheet.logical_templates_ids(ofinput.clist.SelectedIndex)
                Else
                    co = bc_am_in_context.insight_items.contributing_classes(ocont.Lcclass.SelectedItems(0).Index).entity_template_assigments.count
                    co = co / 2
                    oas.class_id = bc_am_in_context.insight_items.contributing_classes(ocont.Lcclass.SelectedItems(0).Index).child_class_id
                    bc_am_in_context.insight_items.contributing_classes(ocont.Lcclass.SelectedItems(0).Index).entity_template_assigments(co + ocont.lasstmp.SelectedItems(0).Index).template_id = bc_am_in_context.asheet.logical_templates_ids(ofinput.clist.SelectedIndex)
                End If
                Me.set_button_disabled(3)
            Else
                Me.Cursor = Cursors.WaitCursor
                ocont.lasstmp.BeginUpdate()

                If ocont.tasstmp.SelectedIndex = 0 Then
                    co = bc_am_in_context.insight_items.contributing_classes(ocont.Lcclass.SelectedItems(0).Index).entity_template_assigments.count
                    If ocont.tasstmp.TabPages.Count > 1 Then
                        co = co / 2
                    End If
                    For i = 0 To co - 1
                        bc_am_in_context.insight_items.contributing_classes(ocont.Lcclass.SelectedItems(0).Index).entity_template_assigments(i).template_id = bc_am_in_context.asheet.logical_templates_ids(ofinput.clist.SelectedIndex)
                        ocont.lasstmp.Items(i).SubItems(1).Text = bc_am_in_context.asheet.logical_templates_names(ofinput.clist.SelectedIndex)
                    Next
                Else
                    co = bc_am_in_context.insight_items.contributing_classes(ocont.Lcclass.SelectedItems(0).Index).entity_template_assigments.count
                    co = co / 2
                    For i = 0 To co - 1
                        bc_am_in_context.insight_items.contributing_classes(ocont.Lcclass.SelectedItems(0).Index).entity_template_assigments(co + i).template_id = bc_am_in_context.asheet.logical_templates_ids(ofinput.clist.SelectedIndex)
                        ocont.lasstmp.Items(i).SubItems(1).Text = bc_am_in_context.asheet.logical_templates_names(ofinput.clist.SelectedIndex)
                    Next
                End If
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_in_tk_main", "ass_temp", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            ocont.lasstmp.EndUpdate()
            Me.Cursor = Cursors.Default

        End Try
    End Sub
    Private Sub load_ass()
        Try
            Dim i, j As Integer
            Dim ofass As New bc_am_in_tk_cont
            ofass.ccp.Items.Clear()
            ofass.ccc.Items.Clear()
            ofass.ccs.Items.Clear()
            For i = 0 To bc_am_in_context.insight_items.class_names.Count - 1
                ofass.ccp.Items.Add(bc_am_in_context.insight_items.class_names(i))
            Next

            For i = 0 To bc_am_in_context.insight_items.schema_names.Count - 1
                ofass.ccs.Items.Add(bc_am_in_context.insight_items.schema_names(i))
            Next

            ofass.Cprime.Items.Add("None")
            ofass.Csec.Items.Add("None")
            For i = 0 To bc_am_in_context.insight_items.pref_types.Count - 1
                ofass.Cprime.Items.Add(bc_am_in_context.insight_items.pref_types(i).name)
                ofass.Csec.Items.Add(bc_am_in_context.insight_items.pref_types(i).name)
            Next
            ofass.Cprime.SelectedIndex = 0
            ofass.Csec.SelectedIndex = 0
            ofass.Csec.Enabled = False

            ofass.ShowDialog()
            If ofass.cancel_selected = True Then
                Exit Sub
            End If

            set_status("Uploading Contributing Class...")
            Me.Cursor = Cursors.WaitCursor
            Dim occ As New bc_om_contributing_class
            occ.schema_id = bc_am_in_context.insight_items.schema_ids(ofass.ccs.SelectedIndex)
            occ.parent_class_id = bc_am_in_context.insight_items.class_ids(ofass.ccp.SelectedIndex)
            If ofass.ccc.SelectedIndex = -1 Then
                occ.child_class_id = 0
            ElseIf ofass.ccc.Text = "None" Then
                occ.child_class_id = 0
            Else
                For j = 0 To bc_am_in_context.insight_items.class_names.Count - 1
                    If bc_am_in_context.insight_items.class_names(j) = ofass.ccc.Text Then
                        occ.child_class_id = bc_am_in_context.insight_items.class_ids(j)
                    End If
                Next
            End If
            occ.granularity_class_id = 0
            For j = 0 To bc_am_in_context.insight_items.class_names.Count - 1
                If bc_am_in_context.insight_items.class_names(j) = ofass.cgran.Text Then
                    occ.granularity_class_id = bc_am_in_context.insight_items.class_ids(j)
                End If
            Next

            occ.primary_apref_type = 0
            occ.secondary_apref_type = 0
            If ofass.Cprime.SelectedIndex > 0 Then
                occ.primary_apref_type = bc_am_in_context.insight_items.pref_types(ofass.Cprime.SelectedIndex - 1).id
            End If
            If ofass.Csec.SelectedIndex > 0 Then
                occ.secondary_apref_type = bc_am_in_context.insight_items.pref_types(ofass.Csec.SelectedIndex - 1).id
            End If

            For j = 0 To bc_am_in_context.insight_items.contributing_classes.Count - 1
                If bc_am_in_context.insight_items.contributing_classes(j).parent_class_id = occ.parent_class_id And bc_am_in_context.insight_items.contributing_classes(j).schema_id = occ.schema_id Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Definition already exists for this parent class/schema combination", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub
                End If
            Next

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                occ.db_write()
                occ.db_read()
            Else
                occ.tmode = bc_cs_soap_base_class.tWRITE
                occ.no_send_back = True
                occ.transmit_to_server_and_receive(occ, True)
                occ.no_send_back = False
                occ.tmode = bc_cs_soap_base_class.tREAD
                occ.transmit_to_server_and_receive(occ, True)
            End If
            bc_am_in_context.insight_items.contributing_classes.Add(occ)

            REM load list
            ocont.load_contributing_classes()
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_in_tk_main", "load_ass", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            clear_status()
            Me.Cursor = Cursors.Default

        End Try
    End Sub


    Private Sub load_calcs()
        Me.set_status("Downloading calculations...")
        Dim ocalcs As New bc_am_calculations
        bc_am_calculation_values.link_attributes.attributes.Clear()
        If ListView1.SelectedIndices(0) = 5 Then
            bc_am_calculation_values.AggregationId = ouniverse.boeAggregation.id
            If bc_am_calculation_values.source_class_id <> 0 And bc_am_calculation_values.target_class_id Then
                bc_am_calculation_values.link_attributes.parent_class_Id = bc_am_calculation_values.target_class_id
                bc_am_calculation_values.link_attributes.child_class_id = bc_am_calculation_values.source_class_id
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    bc_am_calculation_values.link_attributes.db_read()
                Else
                    bc_am_calculation_values.link_attributes.tmode = bc_cs_soap_base_class.tREAD
                    If bc_am_calculation_values.link_attributes.transmit_to_server_and_receive(bc_am_calculation_values.link_attributes, True) = False Then
                        Exit Sub
                    End If
                End If
            End If
        Else
            bc_am_calculation_values.link_attributes.parent_class_Id = 0
            bc_am_calculation_values.link_attributes.child_class_id = 0
            bc_am_calculation_values.TemplateId = CType(bc_am_in_context.asheet.logical_templates_ids(Me.ofcontext.ctemplate.SelectedIndex), Integer)
        End If
        ocalcs.Top = True
        ocalcs.cbCopy.Visible = True
        ocalcs.Label1.Visible = True
        Me.clear_status()





        ocalcs.ShowDialog()
        'If ocalcs.item_added = True Then
        '    Me.set_status("Updating new items...")
        '    Me.Refresh()
        '    Me.Cursor = Cursors.WaitCursor
        '    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
        '        bc_am_in_context.insight_items.db_read()
        '    Else
        '        bc_am_in_context.insight_items.tmode = bc_cs_soap_base_class.tREAD
        '        bc_am_in_context.insight_items.items_only = True
        '        If bc_am_in_context.insight_items.transmit_to_server_and_receive(bc_am_in_context.insight_items, True) = False Then
        '            Exit Sub
        '        End If
        '    End If
        '    Me.Cursor = Cursors.Default
        'End If
        Me.clear_status()
    End Sub

    Private Sub addUniverseBuilderToolbarButtons()
        Dim newindex As Integer
        With Me.uxToolBarMain.Buttons
            newindex = .Add("&New")
            .Item(newindex).ImageIndex = 0
            .Item(newindex).ToolTipText = "Create new aggregation universe"
            If Me.ofcontext.ttemplatenew.Visible = True Then
                .Item(newindex).Enabled = False
            End If
            ouniverse.btnNew = .Item(newindex)
            newindex = .Add("&Save")
            .Item(newindex).ImageIndex = 4
            .Item(newindex).ToolTipText = "Save aggregation universe"
            If Me.ofcontext.ctemplate.SelectedIndex = -1 Then
                .Item(newindex).Enabled = False
            End If
            ouniverse.btnSave = .Item(newindex)
            newindex = .Add("&Cancel")
            .Item(newindex).ImageIndex = 3
            .Item(newindex).ToolTipText = "Cancel changes"
            If Me.ofcontext.ctemplate.SelectedIndex = -1 Then
                .Item(newindex).Enabled = False
            End If
            ouniverse.btnCancel = .Item(newindex)

            newindex = .Add("")
            .Item(newindex).Style = ToolBarButtonStyle.Separator
            newindex = .Add("Deactivate")
            .Item(newindex).ToolTipText = "Make Universe Active/Inactive"""
            .Item(newindex).ImageIndex = 13
            If Me.ofcontext.ctemplate.SelectedIndex = -1 Then
                .Item(newindex).Enabled = False
            End If
            ouniverse.btnactive = .Item(newindex)


            newindex = .Add("&Delete")
            .Item(newindex).ImageIndex = 3
            .Item(newindex).ToolTipText = "Delete Universe"
            If Me.ofcontext.ctemplate.SelectedIndex = -1 Then
                .Item(newindex).Enabled = False
            End If
            ouniverse.btnDelete = .Item(newindex)

            newindex = .Add("")
            .Item(newindex).Style = ToolBarButtonStyle.Separator
            newindex = .Add("&Calculations")
            .Item(newindex).ImageIndex = 11
            .Item(newindex).ToolTipText = "Calculations for selected template"
            If Me.ofcontext.ctemplate.SelectedIndex = -1 Then
                .Item(newindex).Enabled = False
            End If
            ouniverse.btnCalculations = .Item(newindex)

            REM 5.6
            newindex = .Add("")
            .Item(newindex).Style = ToolBarButtonStyle.Separator
            newindex = .Add("&Calc Library")
            .Item(newindex).ImageIndex = 11
            .Item(newindex).ToolTipText = "Calculations Library"
            .Item(newindex).Enabled = True
            ouniverse.btncalclib = .Item(newindex)


            REM FIL 5.2
            newindex = .Add("")
            .Item(newindex).Style = ToolBarButtonStyle.Separator
            newindex = .Add("&Exclusions")
            .Item(newindex).ImageIndex = 3
            .Item(newindex).ToolTipText = "Exclusions For Selected Universe"
            If Me.ofcontext.ctemplate.SelectedIndex = -1 Then
                .Item(newindex).Enabled = False
            End If
            ouniverse.btnexclusions = .Item(newindex)
            REM FIL 5.5
            newindex = .Add("")
            .Item(newindex).Style = ToolBarButtonStyle.Separator
            newindex = .Add("&Hierarchy")
            .Item(newindex).ImageIndex = 12
            .Item(newindex).ToolTipText = "Hierarchy For Selected Universe"
            If Me.ofcontext.ctemplate.SelectedIndex = -1 Then
                .Item(newindex).Enabled = False
            End If
            ouniverse.bthhier = .Item(newindex)


        End With
    End Sub

    Private Sub ListView1_SelectedIndexChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView1.SelectedIndexChanged
        Dim newindex As Integer

        If Me.ListView1.SelectedItems.Count > 0 Then
            ofcontext.Hide()
            ofcs.Hide()
            ocont.Hide()
            oret.Hide()
            Me.uxToolBarMain.Buttons.Clear()
            Select Case Me.ListView1.SelectedItems(0).Index
                Case 0
                    ofcontext.Show()
                    With Me.uxToolBarMain.Buttons
                        newindex = .Add("&New")
                        .Item(newindex).ImageIndex = 0
                        .Item(newindex).ToolTipText = "Create new financial template"
                        If Me.ofcontext.ttemplatenew.Visible = True Then
                            .Item(newindex).Enabled = False
                        End If
                        newindex = .Add("&Clone")
                        .Item(newindex).ImageIndex = 4
                        .Item(newindex).ToolTipText = "Clone selected financial template from another template"
                        If Me.ofcontext.ctemplate.SelectedIndex = -1 Then
                            .Item(newindex).Enabled = False
                        End If
                        newindex = .Add("&Delete")
                        .Item(newindex).ImageIndex = 3
                        .Item(newindex).ToolTipText = "Delete selected financial template"
                        If Me.ofcontext.ctemplate.SelectedIndex = -1 Then
                            .Item(newindex).Enabled = False
                        End If
                        newindex = .Add("")
                        .Item(newindex).Style = ToolBarButtonStyle.Separator
                        newindex = .Add("&Preview")
                        .Item(newindex).ImageIndex = 10
                        If Me.ofcontext.ctemplate.SelectedIndex = -1 Then
                            .Item(newindex).Enabled = False
                        End If
                        .Item(newindex).ToolTipText = "Preview selected template in excel"
                        newindex = .Add("&Calculations")
                        .Item(newindex).ImageIndex = 11
                        .Item(newindex).ToolTipText = "Calculations for selected template"
                        If Me.ofcontext.ctemplate.SelectedIndex = -1 Then
                            .Item(newindex).Enabled = False
                        End If
                        REM 5.6
                        newindex = .Add("")
                        .Item(newindex).Style = ToolBarButtonStyle.Separator
                        newindex = .Add("&Calc Library")
                        .Item(newindex).ImageIndex = 11
                        .Item(newindex).ToolTipText = "Calculations Library"
                        .Item(newindex).Enabled = True



                        newindex = .Add("")
                        .Item(newindex).Style = ToolBarButtonStyle.Separator
                        newindex = .Add("&Import Sheet")
                        .Item(newindex).ImageIndex = 6
                        .Item(newindex).ToolTipText = "Import an excel intermediate data entry sheet for the financial template."
                        If Me.ofcontext.ctemplate.SelectedIndex = -1 Then
                            .Item(newindex).Enabled = False
                        End If
                        newindex = .Add("&Edit Sheet")
                        .Item(newindex).ImageIndex = 7
                        .Item(newindex).ToolTipText = "Edit intermediate sheet in Excel."
                        If Me.ofcontext.ctemplate.SelectedIndex = -1 Or Me.ofcontext.lintsheet.Text = "" Then
                            .Item(newindex).Enabled = False
                        End If
                        newindex = .Add("&Remove Sheet")
                        .Item(newindex).ImageIndex = 3
                        .Item(newindex).ToolTipText = "Delete intermediate sheet from use in financial template."
                        If Me.ofcontext.ctemplate.SelectedIndex = -1 Or Me.ofcontext.lintsheet.Text = "" Then
                            .Item(newindex).Enabled = False
                        End If

                    End With
                Case 1
                    With Me.uxToolBarMain.Buttons
                        newindex = .Add("&New")
                        .Item(newindex).ImageIndex = 4
                        .Item(newindex).ToolTipText = "Add new contributing class"
                        newindex = .Add("&Delete")
                        .Item(newindex).ImageIndex = 3
                        .Item(newindex).ToolTipText = "Delete contributing class"
                        If Me.ofcontext.ctemplate.SelectedIndex = -1 Then
                            .Item(newindex).Enabled = False
                        End If
                        newindex = .Add("")
                        .Item(newindex).Style = ToolBarButtonStyle.Separator
                        newindex = .Add("&Assign")
                        .Item(newindex).ImageIndex = 6
                        .Item(newindex).ToolTipText = "Assign template to selected entity"
                        .Item(newindex).Enabled = False
                        newindex = .Add("&Assign All")
                        .Item(newindex).ImageIndex = 1
                        .Item(newindex).ToolTipText = "Assign default template to all entities"
                        .Item(newindex).Enabled = False
                    End With
                    set_button_disabled(1)
                    ocont.Lcclass.SelectedItems.Clear()
                    ocont.lasstmp.Items.Clear()
                    ocont.uxTemplateAssignment.Enabled = False
                    ocont.Show()
                Case 2
                    With Me.uxToolBarMain.Buttons
                        newindex = .Add("&Create format")
                        .Item(newindex).ImageIndex = 10
                        .Item(newindex).ToolTipText = "Create a new Excel format sheet"
                        newindex = .Add("&Edit format")
                        .Item(newindex).ImageIndex = 7
                        .Item(newindex).ToolTipText = "Edit existing  Excel format sheet"
                        If ofcs.tformatsheet.Text = "" Then
                            .Item(newindex).Enabled = False
                        End If
                        newindex = .Add("&Import format")
                        .Item(newindex).ImageIndex = 6
                        .Item(newindex).ToolTipText = "Import an existing Excel format sheet"
                        newindex = .Add("&Delete format")
                        .Item(newindex).ImageIndex = 3
                        .Item(newindex).ToolTipText = "Delete existing  Excel format sheet"
                        If ofcs.tformatsheet.Text = "" Then
                            .Item(newindex).Enabled = False
                        End If
                        newindex = .Add("")
                        .Item(newindex).Style = ToolBarButtonStyle.Separator
                        newindex = .Add("&Role Security")
                        .Item(newindex).ImageIndex = 12
                        .Item(newindex).ToolTipText = "Manages whether a user role can submit data to draft and/or publish"
                        .Item(newindex).Enabled = False
                        newindex = .Add("")
                        .Item(newindex).Style = ToolBarButtonStyle.Separator
                        newindex = .Add("&Preview")
                        .Item(newindex).ImageIndex = 10
                        If Me.ofcontext.ctemplate.SelectedIndex = -1 Then
                            .Item(newindex).Enabled = False
                        End If
                        Me.ofcs.Lsec.SelectedItems.Clear()

                        Me.ofcs.uxGeneral.Height = 300
                        Me.ofcs.uxGeneral.Visible = True

                        Me.ofcs.uxAggAudit.Height = 185
                        Dim dp As System.Drawing.Point
                        dp.X = Me.ofcs.uxGeneral.Location.X
                        dp.Y = 310
                        Me.ofcs.uxAggAudit.Location = dp
                        Me.ofcs.uxAggAudit.Visible = True

                        Me.ofcs.uxLinkCodes.Visible = False
                        Me.ofcs.uxLinkOps.Visible = False
                        Me.ofcs.uxMapCodes.Visible = False

                    End With


                    ofcs.Show()
                Case 3
                    With Me.uxToolBarMain.Buttons
                        newindex = .Add("  &Preview")
                        .Item(newindex).ImageIndex = 10
                        If Me.ofcontext.ctemplate.SelectedIndex = -1 Then
                            .Item(newindex).Enabled = False
                        End If
                        Me.ofcs.uxGeneral.Visible = False
                        Me.ofcs.uxAggAudit.Visible = False

                        Me.ofcs.uxLinkOps.Location = Me.ofcs.uxGeneral.Location
                        'Me.ofcs.uxLinkOps.Height = 160
                        Me.ofcs.uxLinkOps.Visible = True
                        Me.ofcs.uxLinkCodes.Visible = True
                        Dim dp As System.Drawing.Point
                        dp.X = Me.ofcs.uxLinkOps.Location.X
                        dp.Y = 185
                        Me.ofcs.uxLinkCodes.Location = dp
                        'Me.ofcs.uxLinkCodes.Height = 160
                        dp.Y = 185 + 165
                        Me.ofcs.uxMapCodes.Location = dp
                        Me.ofcs.uxMapCodes.Visible = True
                        'Me.ofcs.uxMapCodes.Height = 128
                        Me.ofcs.uxGeneral.Visible = False
                        Me.ofcs.uxLinkCodes.Visible = True
                        Me.ofcs.uxLinkOps.Visible = True
                        Me.ofcs.uxMapCodes.Visible = True
                    End With
                    ofcs.Show()
                Case 4
                    With Me.uxToolBarMain.Buttons
                        newindex = .Add("&New")
                        .Item(newindex).ImageIndex = 4
                        .Item(newindex).ToolTipText = "Add new item to retrieve into excel sheet"
                        newindex = .Add("&Edit")
                        .Item(newindex).ImageIndex = 7
                        .Item(newindex).ToolTipText = "Edit item to retrieve into excel sheet"
                        .Item(newindex).Enabled = False
                        newindex = .Add("&Delete")
                        .Item(newindex).ImageIndex = 3
                        .Item(newindex).ToolTipText = "remove item from retrival into excel"
                        .Item(newindex).Enabled = False
                        newindex = .Add("")
                        .Item(newindex).Style = ToolBarButtonStyle.Separator
                        newindex = .Add("&Preview")
                        .Item(newindex).ImageIndex = 10
                        If Me.ofcontext.ctemplate.SelectedIndex = -1 Then
                            .Item(newindex).Enabled = False
                        End If
                    End With
                    Me.oret.litems.SelectedItems.Clear()
                    Me.oret.Show()
                Case 5
                    Me.lseltmp.Text = ""
                    addUniverseBuilderToolbarButtons()
                    Me.ouniverse.Show()
            End Select
        End If
    End Sub
    Private Sub mupall_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mupall.Click
        Try
            Dim omsg As New bc_cs_message("Blue Curve", "Are you sure you want to upload changes", bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
            If omsg.cancel_selected = True Then
                Exit Sub
            End If
            Me.Cursor = Cursors.WaitCursor
            Me.set_status("Uploading data to system...")
            If Me.template_changed = True Then
                If Me.ofcontext.upload = False Then
                    Me.clear_status()
                    Exit Sub
                End If
                Me.set_status("Uploading template...")
            End If
            'Me.Cursor = Cursors.WaitCursor
            'If Me.config_changed = True Then
            '    Me.set_status("Uploading assignment...")
            '    Me.ocont.upload_cont()
            'End If
            If Me.settings_changed = True Then
                If upload_user_security() = False Then
                    Exit Sub
                End If
                Me.set_status("Uploading settings...")
                Me.ofcs.create_xml_file(False)
            End If
            If Me.retrieval_changed = True Then
                Me.set_status("Uploading retrieval items...")
                Me.oret.create_xml_file(False)
            End If

            Me.ListView1.Items(0).Selected = True
            Me.ofcontext.reset_template_controls()
            Me.ofcontext.ctemplate.SelectedIndex = -1
            Me.bexport.Enabled = False
            omsg = New bc_cs_message("Blue Curve", "Upload Complete!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            clear_all_change()
            Me.lseltmp.Text = ""
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("mupall", "click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub
    Private Function upload_user_security() As Boolean
        Dim i As Integer
        upload_user_security = True
        REM read settings back into object
        Dim ousersec As bc_om_insight_user_access
        bc_am_in_context.insight_items.user_security.Clear()
        For i = 0 To Me.ofcs.Lsec.Items.Count - 1
            If Me.ofcs.Lsec.Items(i).SubItems(1).Text = "Yes" Then
                ousersec = New bc_om_insight_user_access
                ousersec.draft_default = False
                ousersec.role_name = Me.ofcs.Lsec.Items(i).Text
                ousersec.stage_name = "Draft"
                If Me.ofcs.Lsec.Items(i).SubItems(3).Text = "Draft" Then
                    ousersec.draft_default = True
                End If
                bc_am_in_context.insight_items.user_security.Add(ousersec)
            End If
            If Me.ofcs.Lsec.Items(i).SubItems(2).Text = "Yes" Then
                ousersec = New bc_om_insight_user_access
                ousersec.draft_default = True
                ousersec.role_name = Me.ofcs.Lsec.Items(i).Text
                ousersec.stage_name = "Publish"
                If Me.ofcs.Lsec.Items(i).SubItems(3).Text = "Publish" Then
                    ousersec.draft_default = False
                End If
                bc_am_in_context.insight_items.user_security.Add(ousersec)
            End If
        Next

        bc_am_in_context.insight_items.write_mode = 1
        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
            bc_am_in_context.insight_items.db_write()
        Else

            'bc_am_in_context.insight_items.tmode = bc_cs_soap_base_class.tWRITE
            bc_am_in_context.insight_items.tmode = bc_cs_soap_base_class.tWRITE

            bc_am_in_context.insight_items.no_send_back = True
            If bc_am_in_context.insight_items.transmit_to_server_and_receive(bc_am_in_context.insight_items, True) = False Then
                upload_user_security = False
            End If
        End If
    End Function
    Private Sub mdiscall_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mdiscall.Click
        Try
            Dim omsg As New bc_cs_message("Blue Curve", "Are you sure you want to discard changes", bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
            If omsg.cancel_selected = True Then
                Exit Sub
            End If
            Me.Cursor = Cursors.WaitCursor
            Me.set_status("Rolling back changes...")
            load_client_settings()
            If Me.template_changed = True Or Me.config_changed = True Then
                If Me.ofcontext.discard = False Then
                    Me.clear_status()
                    Exit Sub
                End If
            End If
            If Me.config_changed = True Then
                ocont.load_contributing_classes()
            End If
            If Me.settings_changed = True Then
                ofcs.load_settings()
                ofcs.load_user_security()
            End If
            If Me.retrieval_changed = True Then
                oret.load_start()
            End If
            Me.Cursor = Cursors.Default
            Me.ListView1.Items(0).Selected = True
            Me.ofcontext.reset_template_controls()
            omsg = New bc_cs_message("Blue Curve", "Discard Complete!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            clear_all_change()
            Me.bexport.Enabled = False
            Me.ofcontext.ctemplate.SelectedIndex = -1
            Me.lseltmp.Text = ""
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_in_tk_main", "mdiscall", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub
    Private Sub bc_am_in_tk_main_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        If Me.mupall.Enabled = True Then
            Dim omsg As New bc_cs_message("Blue Curve", "You have uncomitted data are you sure you wish to exit?", bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
            If omsg.cancel_selected = True Then
                e.Cancel = True
                Exit Sub
            End If
        End If
    End Sub
    Public Sub set_button_enabled(ByVal idx As Integer)
        Me.uxToolBarMain.Buttons(idx).Enabled = True
    End Sub
    Public Sub set_button_disabled(ByVal idx As Integer)
        Me.uxToolBarMain.Buttons(idx).Enabled = False
    End Sub

    Private Sub del_ass()
        Try
            If ocont.Lcclass.SelectedItems(0).Index > -1 Then
                Dim omsg As New bc_cs_message("Blue Curve", "Are you sure you wish to delete contributing entity: " + CStr(ocont.Lcclass.SelectedItems(0).Text) + " and its assigned templates", bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
                If omsg.cancel_selected = True Then
                    Exit Sub
                End If
                Me.Cursor = Cursors.WaitCursor
                Me.set_status("Deleting contributing class " + CStr(ocont.Lcclass.SelectedItems(0).Text))
                bc_am_in_context.insight_items.contributing_classes(ocont.Lcclass.SelectedItems(0).Index).write_mode = bc_om_contributing_class.EWRITE_MODE.DELETE
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    bc_am_in_context.insight_items.contributing_classes(ocont.Lcclass.SelectedItems(0).Index).db_write()
                Else
                    bc_am_in_context.insight_items.contributing_classes(ocont.Lcclass.SelectedItems(0).Index).tmode = bc_cs_soap_base_class.tWRITE
                    bc_am_in_context.insight_items.contributing_classes(ocont.Lcclass.SelectedItems(0).Index).no_send_back = True
                    If bc_am_in_context.insight_items.contributing_classes(ocont.Lcclass.SelectedItems(0).Index).transmit_to_server_and_receive(bc_am_in_context.insight_items, True) = False Then
                        Exit Sub
                    End If
                End If
                bc_am_in_context.insight_items.contributing_classes.RemoveAt(ocont.Lcclass.SelectedItems(0).Index)
                Me.set_button_disabled(1)
                Me.set_button_disabled(3)
                Me.set_button_disabled(4)
                'If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                '    bc_am_in_context.insight_items.db_write()
                'Else
                '    bc_am_in_context.insight_items.tmode = bc_cs_soap_base_class.tWRITE
                '    bc_am_in_context.insight_items.no_send_back = True
                '    If bc_am_in_context.insight_items.transmit_to_server_and_receive(bc_am_in_context.insight_items, True) = False Then
                '        Exit Sub
                '    End If
                'End If
                REM load list
                ocont.load_contributing_classes()
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_in_tk_main", "del_ass", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            Me.clear_status()
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Public Sub create_format_sheet()
        Dim fn As String = "bc_insight_format_default.xls"
        Dim omsg As bc_cs_message

        Dim fs As New bc_cs_file_transfer_services
        If fs.file_already_open(bc_cs_central_settings.local_template_path + fn) Then
            omsg = New bc_cs_message("Blue Curve", "Cannot create format sheet as file: " + bc_cs_central_settings.local_template_path + fn + " is open please close and try again", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            Exit Sub
        End If
        If Me.ofcs.tformatsheet.Text <> "Format Sheet: none" Then
            omsg = New bc_cs_message("Blue Curve", "Creating format sheet will overwrite reference to existing " + Me.ofcs.tformatsheet.Text + " proceed?", bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
            If omsg.cancel_selected = True Then
                Exit Sub
            End If
        End If

        Dim oformat As New bc_am_in_tk_format
        oformat.ShowDialog()
        If oformat.cancel_selected = True Then
            Exit Sub
        End If
        Me.Cursor = Cursors.WaitCursor
        Me.set_status("Generating format sheet in excel")

        Dim xl As Object = Nothing
        Dim excel_new As Boolean
        Try
            Me.Cursor = Cursors.WaitCursor

            REM previews current in memory settings
            Try
                xl = GetObject(, "excel.application")
                excel_new = False
            Catch
                Try
                    excel_new = True
                    xl = bc_ao_in_excel.CreateNewExcelInstance
                    xl.application.visible = False
                Catch
                    omsg = New bc_cs_message("Blue Curve", "Cannot invoke Excel", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub
                End Try
            End Try
            If xl.application.workbooks.count = 0 Then
                xl.application.workbooks.add()
            End If
            Dim excel_app As New bc_ao_in_excel(xl.application)
            Dim contcol As New bc_ao_in_excel.ao_rgb
            Dim flexcol As New bc_ao_in_excel.ao_rgb
            Dim editcol As New bc_ao_in_excel.ao_rgb
            Dim headcol As New bc_ao_in_excel.ao_rgb
            Dim title_name_font As New bc_ao_in_excel.ao_font
            Dim title_value_font As New bc_ao_in_excel.ao_font
            Dim section_font As New bc_ao_in_excel.ao_font
            Dim row_font As New bc_ao_in_excel.ao_font
            Dim flexible_font As New bc_ao_in_excel.ao_font
            Dim value_font As New bc_ao_in_excel.ao_font
            Dim head_font As New bc_ao_in_excel.ao_font
            contcol.red = oformat.bcontcol.BackColor.R
            contcol.green = oformat.bcontcol.BackColor.G
            contcol.blue = oformat.bcontcol.BackColor.B
            editcol.red = oformat.beditcol.BackColor.R
            editcol.green = oformat.beditcol.BackColor.G
            editcol.blue = oformat.beditcol.BackColor.B
            headcol.red = oformat.bheadcol.BackColor.R
            headcol.green = oformat.bheadcol.BackColor.G
            headcol.blue = oformat.bheadcol.BackColor.B
            flexcol.red = oformat.bflex.ForeColor.R
            flexcol.green = oformat.bflex.ForeColor.G
            flexcol.blue = oformat.bflex.ForeColor.B

            REM title name
            title_name_font.name = oformat.bheadname.Font.Name
            title_name_font.size = oformat.bheadname.Font.Size
            title_name_font.italic = oformat.bheadname.Font.Italic
            title_name_font.bold = oformat.bheadname.Font.Bold
            title_name_font.colour = contcol

            REM title value
            title_value_font.name = oformat.bheadvalue.Font.Name
            title_value_font.size = oformat.bheadvalue.Font.Size
            title_value_font.italic = oformat.bheadvalue.Font.Italic
            title_value_font.bold = oformat.bheadvalue.Font.Bold
            title_value_font.colour = contcol

            REM section_name
            section_font.name = oformat.bsection.Font.Name
            section_font.size = oformat.bsection.Font.Size
            section_font.italic = oformat.bsection.Font.Italic
            section_font.bold = oformat.bsection.Font.Bold
            section_font.colour = contcol

            REM row_name
            row_font.name = oformat.brow.Font.Name
            row_font.size = oformat.brow.Font.Size
            row_font.italic = oformat.brow.Font.Italic
            row_font.bold = oformat.brow.Font.Bold
            row_font.colour = contcol

            REM flexible
            flexible_font.name = oformat.brow.Font.Name
            flexible_font.size = oformat.brow.Font.Size
            flexible_font.italic = oformat.brow.Font.Italic
            flexible_font.bold = oformat.brow.Font.Bold
            flexible_font.colour = flexcol

            REM header font
            head_font.name = oformat.bheadfont.Font.Name
            head_font.size = oformat.bheadfont.Font.Size
            head_font.italic = oformat.bheadfont.Font.Italic
            head_font.bold = oformat.bheadfont.Font.Bold

            REM value font
            value_font.name = oformat.beditfont.Font.Name
            value_font.size = oformat.beditfont.Font.Size
            value_font.italic = oformat.beditfont.Font.Italic
            value_font.bold = oformat.beditfont.Font.Bold
            If excel_app.create_format_sheet(fn, editcol, contcol, headcol, title_name_font, title_value_font, section_font, row_font, flexible_font, head_font, value_font) = True Then
                Me.ofcs.tformatsheet.Text = "Format Sheet: bc_insight_format_default.xls"
                Me.ofcs.template = "bc_insight_format_default.xls"
                Me.set_change(1)
            End If
            omsg = New bc_cs_message("Blue Curve", "Format sheet: " + "bc_insight_format_default.xls created.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            Me.set_button_enabled(1)
            Me.set_button_enabled(3)

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_in_tk_main", "create_format_sheet", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            If excel_new = True Then
                xl.application.quit()
            End If
            Me.clear_status()
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    Private Sub MenuItem4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem4.Click
        Dim fabout As New bc_am_in_tk_about
        fabout.ShowDialog()
    End Sub
    Public Sub set_template_name(ByVal tx As String)
        Me.lseltmp.Text = tx
    End Sub
    Private Sub pmain_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles pmain.Paint

    End Sub
    Public Sub enable_import()
        Me.bexport.Enabled = True
    End Sub
    Public Sub disable_import()
        Me.bexport.Enabled = False
    End Sub
    Private Sub Panel3_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Panel3.Paint

    End Sub

    Private Sub PictureBox3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox3.Click

    End Sub

    Private Sub bc_am_in_tk_main_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Resize

        ' stop resizing when a certain size is reached
        If Me.Width < 997 Then
            Me.Width = 997
            Exit Sub
        End If

        If Me.Height < 720 Then
            Me.Height = 720
            Exit Sub
        End If

    End Sub

    Private Sub bexport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bexport.Click
        Me.Cursor = Cursors.WaitCursor
        Me.set_status("Exporting Template " + Me.ofcontext.ctemplate.Text + "...")
        Me.ofcontext.upload_config(True)
        Me.clear_status()
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub bexpall_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bexpall.Click
        Me.Cursor = Cursors.WaitCursor
        Me.ofcontext.export_all()
        Me.clear_status()
        Me.Cursor = Cursors.Default
    End Sub
End Class

