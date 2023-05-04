<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bc_am_cp_monitor
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_cp_monitor))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle11 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle12 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle13 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle14 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle15 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle16 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.uxClients = New System.Windows.Forms.ComboBox()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.uxClear = New System.Windows.Forms.Button()
        Me.uxRefresh = New System.Windows.Forms.Button()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.uxTJobs = New System.Windows.Forms.TreeView()
        Me.repimages = New System.Windows.Forms.ImageList(Me.components)
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer()
        Me.SplitContainer3 = New System.Windows.Forms.SplitContainer()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.uxGridJob = New System.Windows.Forms.DataGridView()
        Me.checkname = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.jobdescription = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.uxStatGrid = New System.Windows.Forms.DataGridView()
        Me.sritemname = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.lastrundate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.errorfound = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.lasterrordate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.lasterrorid = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.lastrestart = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.restartresult = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.restartcount = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.uxErrorGrid = New System.Windows.Forms.DataGridView()
        Me.errorid = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.errordate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.severity = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.srserrortext = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.MonitorTabPage = New System.Windows.Forms.TabPage()
        Me.SplitContainer4 = New System.Windows.Forms.SplitContainer()
        Me.SplitContainer5 = New System.Windows.Forms.SplitContainer()
        Me.uxLJobs = New System.Windows.Forms.ListView()
        Me.srsJobContextMenuStrip = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AddJobItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.uxJobsGrid = New System.Windows.Forms.DataGridView()
        Me.attribute = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.value = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.uxSheduleGrid = New System.Windows.Forms.DataGridView()
        Me.JobNameS = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.clientname = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ItemName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.scheduletime = New BlueCurve.CommonPlatform.AM.TimeColumn()
        Me.Frequency = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.monitorstarttime = New BlueCurve.CommonPlatform.AM.TimeColumn()
        Me.monitorendtime = New BlueCurve.CommonPlatform.AM.TimeColumn()
        Me.monitormonday = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.monitortuesday = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.monitorwednesday = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.monitorthursday = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.monitorfriday = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.monitorsaturday = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.monitorsunday = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.active = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.errorfrequency = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.schedid = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.restartattemps = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.restartfrequency = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.srsContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AddItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeleteItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EditItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.SchedReset = New System.Windows.Forms.Button()
        Me.SchedSave = New System.Windows.Forms.Button()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.ClientReset = New System.Windows.Forms.Button()
        Me.ClientSave = New System.Windows.Forms.Button()
        Me.SplitContainer6 = New System.Windows.Forms.SplitContainer()
        Me.uxClientTree = New System.Windows.Forms.TreeView()
        Me.srsClientContextMenuStrip = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.Addclient = New System.Windows.Forms.ToolStripMenuItem()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.uxClientDetails = New System.Windows.Forms.DataGridView()
        Me.cattribute = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.cvalue = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.tabemail = New System.Windows.Forms.TabPage()
        Me.SplitContainer7 = New System.Windows.Forms.SplitContainer()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.uxEmailTree = New System.Windows.Forms.TreeView()
        Me.Emailreset = New System.Windows.Forms.Button()
        Me.EmailSave = New System.Windows.Forms.Button()
        Me.uxPrefDetails = New System.Windows.Forms.DataGridView()
        Me.filterid = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.prefid = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.pclientemailid = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.pclient = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.preftype = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.pdescription = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.value1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.value2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.srsEmailContextMenuStrip = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.InsertAddress = New System.Windows.Forms.ToolStripMenuItem()
        Me.InertPref = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeleteAddress = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeletePref = New System.Windows.Forms.ToolStripMenuItem()
        Me.uxEmailDetails = New System.Windows.Forms.DataGridView()
        Me.emailid = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eclient = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ename = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.edescription = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eaddress = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.eonerror = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.eonstatuscheck = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn7 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn8 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn9 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn10 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn11 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn12 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn13 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn14 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn15 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn16 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn17 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn18 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TimeColumn1 = New BlueCurve.CommonPlatform.AM.TimeColumn()
        Me.DataGridViewTextBoxColumn19 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TimeColumn2 = New BlueCurve.CommonPlatform.AM.TimeColumn()
        Me.TimeColumn3 = New BlueCurve.CommonPlatform.AM.TimeColumn()
        Me.DataGridViewTextBoxColumn20 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn21 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn22 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn23 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn24 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn25 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn26 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn27 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn28 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn29 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn30 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn31 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn32 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn33 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn34 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn35 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn36 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn37 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn38 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TabControl1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.Panel2.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        CType(Me.SplitContainer3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer3.Panel1.SuspendLayout()
        Me.SplitContainer3.Panel2.SuspendLayout()
        Me.SplitContainer3.SuspendLayout()
        CType(Me.uxGridJob, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxStatGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxErrorGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MonitorTabPage.SuspendLayout()
        CType(Me.SplitContainer4, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer4.Panel1.SuspendLayout()
        Me.SplitContainer4.Panel2.SuspendLayout()
        Me.SplitContainer4.SuspendLayout()
        CType(Me.SplitContainer5, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer5.Panel1.SuspendLayout()
        Me.SplitContainer5.Panel2.SuspendLayout()
        Me.SplitContainer5.SuspendLayout()
        Me.srsJobContextMenuStrip.SuspendLayout()
        CType(Me.uxJobsGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uxSheduleGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.srsContextMenuStrip1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        CType(Me.SplitContainer6, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer6.Panel1.SuspendLayout()
        Me.SplitContainer6.Panel2.SuspendLayout()
        Me.SplitContainer6.SuspendLayout()
        Me.srsClientContextMenuStrip.SuspendLayout()
        CType(Me.uxClientDetails, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabemail.SuspendLayout()
        CType(Me.SplitContainer7, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer7.Panel1.SuspendLayout()
        Me.SplitContainer7.Panel2.SuspendLayout()
        Me.SplitContainer7.SuspendLayout()
        CType(Me.uxPrefDetails, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.srsEmailContextMenuStrip.SuspendLayout()
        CType(Me.uxEmailDetails, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(0, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(45, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Client:"
        '
        'uxClients
        '
        Me.uxClients.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.uxClients.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxClients.FormattingEnabled = True
        Me.uxClients.Location = New System.Drawing.Point(51, 6)
        Me.uxClients.Name = "uxClients"
        Me.uxClients.Size = New System.Drawing.Size(270, 21)
        Me.uxClients.TabIndex = 3
        '
        'TabControl1
        '
        Me.TabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Controls.Add(Me.MonitorTabPage)
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.tabemail)
        Me.TabControl1.Location = New System.Drawing.Point(3, 33)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(878, 422)
        Me.TabControl1.TabIndex = 5
        '
        'TabPage2
        '
        Me.TabPage2.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.TabPage2.Controls.Add(Me.uxClear)
        Me.TabPage2.Controls.Add(Me.uxRefresh)
        Me.TabPage2.Controls.Add(Me.SplitContainer1)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(870, 396)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Reporting"
        '
        'uxClear
        '
        Me.uxClear.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxClear.Enabled = False
        Me.uxClear.Location = New System.Drawing.Point(748, 371)
        Me.uxClear.Name = "uxClear"
        Me.uxClear.Size = New System.Drawing.Size(55, 23)
        Me.uxClear.TabIndex = 7
        Me.uxClear.Text = "Clear "
        Me.uxClear.UseVisualStyleBackColor = True
        '
        'uxRefresh
        '
        Me.uxRefresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxRefresh.Location = New System.Drawing.Point(809, 370)
        Me.uxRefresh.Name = "uxRefresh"
        Me.uxRefresh.Size = New System.Drawing.Size(55, 23)
        Me.uxRefresh.TabIndex = 1
        Me.uxRefresh.Text = "Refresh"
        Me.uxRefresh.UseVisualStyleBackColor = True
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainer1.Location = New System.Drawing.Point(3, 3)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.uxTJobs)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.SplitContainer2)
        Me.SplitContainer1.Size = New System.Drawing.Size(864, 362)
        Me.SplitContainer1.SplitterDistance = 170
        Me.SplitContainer1.TabIndex = 0
        '
        'uxTJobs
        '
        Me.uxTJobs.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxTJobs.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxTJobs.ImageIndex = 0
        Me.uxTJobs.ImageList = Me.repimages
        Me.uxTJobs.Location = New System.Drawing.Point(0, 0)
        Me.uxTJobs.Name = "uxTJobs"
        Me.uxTJobs.SelectedImageIndex = 0
        Me.uxTJobs.Size = New System.Drawing.Size(170, 362)
        Me.uxTJobs.TabIndex = 0
        '
        'repimages
        '
        Me.repimages.ImageStream = CType(resources.GetObject("repimages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.repimages.TransparentColor = System.Drawing.Color.Transparent
        Me.repimages.Images.SetKeyName(0, "Links")
        Me.repimages.Images.SetKeyName(1, "Attributes")
        Me.repimages.Images.SetKeyName(2, "publication")
        Me.repimages.Images.SetKeyName(3, "number")
        Me.repimages.Images.SetKeyName(4, "date")
        Me.repimages.Images.SetKeyName(5, "string")
        Me.repimages.Images.SetKeyName(6, "data")
        Me.repimages.Images.SetKeyName(7, "users")
        Me.repimages.Images.SetKeyName(8, "user")
        Me.repimages.Images.SetKeyName(9, "parent")
        Me.repimages.Images.SetKeyName(10, "child")
        Me.repimages.Images.SetKeyName(11, "selected")
        Me.repimages.Images.SetKeyName(12, "Folder")
        Me.repimages.Images.SetKeyName(13, "New")
        Me.repimages.Images.SetKeyName(14, "Tasks.ico")
        Me.repimages.Images.SetKeyName(15, "clock.ico")
        Me.repimages.Images.SetKeyName(16, "ProgressWarn.ico")
        '
        'SplitContainer2
        '
        Me.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer2.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer2.Name = "SplitContainer2"
        Me.SplitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer2.Panel1
        '
        Me.SplitContainer2.Panel1.AccessibleDescription = ""
        Me.SplitContainer2.Panel1.Controls.Add(Me.SplitContainer3)
        '
        'SplitContainer2.Panel2
        '
        Me.SplitContainer2.Panel2.Controls.Add(Me.Label6)
        Me.SplitContainer2.Panel2.Controls.Add(Me.uxErrorGrid)
        Me.SplitContainer2.Size = New System.Drawing.Size(690, 362)
        Me.SplitContainer2.SplitterDistance = 201
        Me.SplitContainer2.TabIndex = 0
        '
        'SplitContainer3
        '
        Me.SplitContainer3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer3.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer3.Name = "SplitContainer3"
        Me.SplitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer3.Panel1
        '
        Me.SplitContainer3.Panel1.Controls.Add(Me.Label2)
        Me.SplitContainer3.Panel1.Controls.Add(Me.uxGridJob)
        '
        'SplitContainer3.Panel2
        '
        Me.SplitContainer3.Panel2.Controls.Add(Me.Label5)
        Me.SplitContainer3.Panel2.Controls.Add(Me.uxStatGrid)
        Me.SplitContainer3.Size = New System.Drawing.Size(690, 201)
        Me.SplitContainer3.SplitterDistance = 106
        Me.SplitContainer3.TabIndex = 0
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(3, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(69, 13)
        Me.Label2.TabIndex = 72
        Me.Label2.Text = "Job Details"
        '
        'uxGridJob
        '
        Me.uxGridJob.AllowUserToAddRows = False
        Me.uxGridJob.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxGridJob.BackgroundColor = System.Drawing.SystemColors.Window
        Me.uxGridJob.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.uxGridJob.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.uxGridJob.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.checkname, Me.jobdescription})
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.uxGridJob.DefaultCellStyle = DataGridViewCellStyle1
        Me.uxGridJob.Location = New System.Drawing.Point(0, 18)
        Me.uxGridJob.MultiSelect = False
        Me.uxGridJob.Name = "uxGridJob"
        Me.uxGridJob.RowHeadersVisible = False
        Me.uxGridJob.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.uxGridJob.Size = New System.Drawing.Size(690, 90)
        Me.uxGridJob.TabIndex = 5
        '
        'checkname
        '
        Me.checkname.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.checkname.HeaderText = "Job Name"
        Me.checkname.Name = "checkname"
        '
        'jobdescription
        '
        Me.jobdescription.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.jobdescription.HeaderText = "Description"
        Me.jobdescription.Name = "jobdescription"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(3, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(58, 13)
        Me.Label5.TabIndex = 73
        Me.Label5.Text = "Statistics"
        '
        'uxStatGrid
        '
        Me.uxStatGrid.AllowUserToAddRows = False
        Me.uxStatGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxStatGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.uxStatGrid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.uxStatGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.uxStatGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.sritemname, Me.lastrundate, Me.errorfound, Me.lasterrordate, Me.lasterrorid, Me.lastrestart, Me.restartresult, Me.restartcount})
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.uxStatGrid.DefaultCellStyle = DataGridViewCellStyle2
        Me.uxStatGrid.Location = New System.Drawing.Point(0, 18)
        Me.uxStatGrid.MultiSelect = False
        Me.uxStatGrid.Name = "uxStatGrid"
        Me.uxStatGrid.RowHeadersVisible = False
        Me.uxStatGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.uxStatGrid.Size = New System.Drawing.Size(690, 76)
        Me.uxStatGrid.TabIndex = 5
        '
        'sritemname
        '
        Me.sritemname.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.sritemname.HeaderText = "Task Name"
        Me.sritemname.Name = "sritemname"
        '
        'lastrundate
        '
        Me.lastrundate.HeaderText = "Last Run Date"
        Me.lastrundate.Name = "lastrundate"
        Me.lastrundate.Width = 150
        '
        'errorfound
        '
        Me.errorfound.HeaderText = "Open Errors"
        Me.errorfound.Name = "errorfound"
        Me.errorfound.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.errorfound.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        '
        'lasterrordate
        '
        Me.lasterrordate.HeaderText = "Last Date Error Found"
        Me.lasterrordate.Name = "lasterrordate"
        Me.lasterrordate.Width = 150
        '
        'lasterrorid
        '
        Me.lasterrorid.HeaderText = "Last Error ID"
        Me.lasterrorid.Name = "lasterrorid"
        '
        'lastrestart
        '
        Me.lastrestart.HeaderText = "Last Restart Attempt"
        Me.lastrestart.Name = "lastrestart"
        Me.lastrestart.Width = 150
        '
        'restartresult
        '
        Me.restartresult.HeaderText = "Restart Result"
        Me.restartresult.Name = "restartresult"
        '
        'restartcount
        '
        Me.restartcount.HeaderText = "Restart Trys"
        Me.restartcount.Name = "restartcount"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(3, 1)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(42, 13)
        Me.Label6.TabIndex = 74
        Me.Label6.Text = "Errors"
        '
        'uxErrorGrid
        '
        Me.uxErrorGrid.AllowUserToAddRows = False
        Me.uxErrorGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxErrorGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader
        Me.uxErrorGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.uxErrorGrid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.uxErrorGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.uxErrorGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.errorid, Me.errordate, Me.severity, Me.srserrortext})
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.uxErrorGrid.DefaultCellStyle = DataGridViewCellStyle3
        Me.uxErrorGrid.Location = New System.Drawing.Point(0, 21)
        Me.uxErrorGrid.Name = "uxErrorGrid"
        Me.uxErrorGrid.RowHeadersVisible = False
        Me.uxErrorGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.uxErrorGrid.Size = New System.Drawing.Size(690, 136)
        Me.uxErrorGrid.TabIndex = 2
        '
        'errorid
        '
        Me.errorid.HeaderText = "Error ID"
        Me.errorid.Name = "errorid"
        Me.errorid.Width = 68
        '
        'errordate
        '
        Me.errordate.HeaderText = "Error Date"
        Me.errordate.MinimumWidth = 200
        Me.errordate.Name = "errordate"
        Me.errordate.Width = 200
        '
        'severity
        '
        Me.severity.HeaderText = "Severity"
        Me.severity.Name = "severity"
        Me.severity.Width = 70
        '
        'srserrortext
        '
        Me.srserrortext.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.srserrortext.HeaderText = "Error"
        Me.srserrortext.MinimumWidth = 2000
        Me.srserrortext.Name = "srserrortext"
        '
        'MonitorTabPage
        '
        Me.MonitorTabPage.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.MonitorTabPage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.MonitorTabPage.Controls.Add(Me.SplitContainer4)
        Me.MonitorTabPage.Location = New System.Drawing.Point(4, 22)
        Me.MonitorTabPage.Name = "MonitorTabPage"
        Me.MonitorTabPage.Padding = New System.Windows.Forms.Padding(3)
        Me.MonitorTabPage.Size = New System.Drawing.Size(870, 396)
        Me.MonitorTabPage.TabIndex = 0
        Me.MonitorTabPage.Text = "Schedule"
        '
        'SplitContainer4
        '
        Me.SplitContainer4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.SplitContainer4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer4.Location = New System.Drawing.Point(3, 3)
        Me.SplitContainer4.Name = "SplitContainer4"
        Me.SplitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer4.Panel1
        '
        Me.SplitContainer4.Panel1.Controls.Add(Me.SplitContainer5)
        '
        'SplitContainer4.Panel2
        '
        Me.SplitContainer4.Panel2.Controls.Add(Me.uxSheduleGrid)
        Me.SplitContainer4.Panel2.Controls.Add(Me.Label4)
        Me.SplitContainer4.Panel2.Controls.Add(Me.SchedReset)
        Me.SplitContainer4.Panel2.Controls.Add(Me.SchedSave)
        Me.SplitContainer4.Size = New System.Drawing.Size(862, 388)
        Me.SplitContainer4.SplitterDistance = 153
        Me.SplitContainer4.TabIndex = 0
        '
        'SplitContainer5
        '
        Me.SplitContainer5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.SplitContainer5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer5.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer5.Name = "SplitContainer5"
        '
        'SplitContainer5.Panel1
        '
        Me.SplitContainer5.Panel1.Controls.Add(Me.uxLJobs)
        '
        'SplitContainer5.Panel2
        '
        Me.SplitContainer5.Panel2.Controls.Add(Me.uxJobsGrid)
        Me.SplitContainer5.Panel2.Controls.Add(Me.Label3)
        Me.SplitContainer5.Size = New System.Drawing.Size(860, 151)
        Me.SplitContainer5.SplitterDistance = 287
        Me.SplitContainer5.TabIndex = 0
        '
        'uxLJobs
        '
        Me.uxLJobs.Alignment = System.Windows.Forms.ListViewAlignment.Left
        Me.uxLJobs.AutoArrange = False
        Me.uxLJobs.ContextMenuStrip = Me.srsJobContextMenuStrip
        Me.uxLJobs.Dock = System.Windows.Forms.DockStyle.Fill
        Me.uxLJobs.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxLJobs.FullRowSelect = True
        Me.uxLJobs.GridLines = True
        Me.uxLJobs.HideSelection = False
        Me.uxLJobs.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.uxLJobs.LabelWrap = False
        Me.uxLJobs.Location = New System.Drawing.Point(0, 0)
        Me.uxLJobs.MultiSelect = False
        Me.uxLJobs.Name = "uxLJobs"
        Me.uxLJobs.Size = New System.Drawing.Size(283, 147)
        Me.uxLJobs.TabIndex = 0
        Me.uxLJobs.UseCompatibleStateImageBehavior = False
        Me.uxLJobs.View = System.Windows.Forms.View.SmallIcon
        '
        'srsJobContextMenuStrip
        '
        Me.srsJobContextMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddJobItem})
        Me.srsJobContextMenuStrip.Name = "srsJobContextMenuStrip"
        Me.srsJobContextMenuStrip.Size = New System.Drawing.Size(165, 26)
        '
        'AddJobItem
        '
        Me.AddJobItem.Image = CType(resources.GetObject("AddJobItem.Image"), System.Drawing.Image)
        Me.AddJobItem.Name = "AddJobItem"
        Me.AddJobItem.Size = New System.Drawing.Size(164, 22)
        Me.AddJobItem.Text = "Add new SRS Job"
        '
        'uxJobsGrid
        '
        Me.uxJobsGrid.AllowUserToAddRows = False
        Me.uxJobsGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxJobsGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.uxJobsGrid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.uxJobsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.uxJobsGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.attribute, Me.value})
        Me.uxJobsGrid.ContextMenuStrip = Me.srsJobContextMenuStrip
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.uxJobsGrid.DefaultCellStyle = DataGridViewCellStyle4
        Me.uxJobsGrid.Location = New System.Drawing.Point(3, 16)
        Me.uxJobsGrid.MultiSelect = False
        Me.uxJobsGrid.Name = "uxJobsGrid"
        Me.uxJobsGrid.RowHeadersVisible = False
        Me.uxJobsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.uxJobsGrid.Size = New System.Drawing.Size(561, 138)
        Me.uxJobsGrid.TabIndex = 70
        '
        'attribute
        '
        Me.attribute.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.attribute.HeaderText = "Attribute"
        Me.attribute.Name = "attribute"
        '
        'value
        '
        Me.value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.value.HeaderText = "Value"
        Me.value.Name = "value"
        '
        'Label3
        '
        Me.Label3.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(3, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(69, 13)
        Me.Label3.TabIndex = 69
        Me.Label3.Text = "Job Details"
        '
        'uxSheduleGrid
        '
        Me.uxSheduleGrid.AllowUserToAddRows = False
        Me.uxSheduleGrid.AllowUserToOrderColumns = True
        Me.uxSheduleGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxSheduleGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.uxSheduleGrid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.uxSheduleGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.uxSheduleGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.JobNameS, Me.clientname, Me.ItemName, Me.scheduletime, Me.Frequency, Me.monitorstarttime, Me.monitorendtime, Me.monitormonday, Me.monitortuesday, Me.monitorwednesday, Me.monitorthursday, Me.monitorfriday, Me.monitorsaturday, Me.monitorsunday, Me.active, Me.errorfrequency, Me.schedid, Me.restartattemps, Me.restartfrequency})
        Me.uxSheduleGrid.ContextMenuStrip = Me.srsContextMenuStrip1
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle9.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.uxSheduleGrid.DefaultCellStyle = DataGridViewCellStyle9
        Me.uxSheduleGrid.Location = New System.Drawing.Point(0, 27)
        Me.uxSheduleGrid.MultiSelect = False
        Me.uxSheduleGrid.Name = "uxSheduleGrid"
        Me.uxSheduleGrid.RowHeadersVisible = False
        Me.uxSheduleGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.uxSheduleGrid.ShowRowErrors = False
        Me.uxSheduleGrid.Size = New System.Drawing.Size(858, 159)
        Me.uxSheduleGrid.TabIndex = 72
        '
        'JobNameS
        '
        Me.JobNameS.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        DataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.JobNameS.DefaultCellStyle = DataGridViewCellStyle5
        Me.JobNameS.HeaderText = "Job Name"
        Me.JobNameS.MinimumWidth = 150
        Me.JobNameS.Name = "JobNameS"
        Me.JobNameS.ReadOnly = True
        Me.JobNameS.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        '
        'clientname
        '
        Me.clientname.HeaderText = "Client"
        Me.clientname.Name = "clientname"
        Me.clientname.ReadOnly = True
        Me.clientname.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        '
        'ItemName
        '
        Me.ItemName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.ItemName.HeaderText = "Task Name"
        Me.ItemName.MinimumWidth = 150
        Me.ItemName.Name = "ItemName"
        '
        'scheduletime
        '
        Me.scheduletime.HeaderText = "One Off Time"
        Me.scheduletime.Name = "scheduletime"
        '
        'Frequency
        '
        DataGridViewCellStyle6.Format = "N0"
        DataGridViewCellStyle6.NullValue = Nothing
        Me.Frequency.DefaultCellStyle = DataGridViewCellStyle6
        Me.Frequency.HeaderText = "Frequency (minutes)"
        Me.Frequency.Name = "Frequency"
        '
        'monitorstarttime
        '
        Me.monitorstarttime.HeaderText = "Start Time"
        Me.monitorstarttime.Name = "monitorstarttime"
        '
        'monitorendtime
        '
        Me.monitorendtime.HeaderText = "End Time"
        Me.monitorendtime.Name = "monitorendtime"
        '
        'monitormonday
        '
        Me.monitormonday.HeaderText = "Monday"
        Me.monitormonday.Name = "monitormonday"
        Me.monitormonday.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.monitormonday.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        '
        'monitortuesday
        '
        Me.monitortuesday.HeaderText = "Tuesday"
        Me.monitortuesday.Name = "monitortuesday"
        Me.monitortuesday.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.monitortuesday.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        '
        'monitorwednesday
        '
        Me.monitorwednesday.HeaderText = "Wedensday"
        Me.monitorwednesday.Name = "monitorwednesday"
        Me.monitorwednesday.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.monitorwednesday.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        '
        'monitorthursday
        '
        Me.monitorthursday.HeaderText = "Thursday"
        Me.monitorthursday.Name = "monitorthursday"
        Me.monitorthursday.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.monitorthursday.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        '
        'monitorfriday
        '
        Me.monitorfriday.HeaderText = "Friday"
        Me.monitorfriday.Name = "monitorfriday"
        Me.monitorfriday.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.monitorfriday.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        '
        'monitorsaturday
        '
        Me.monitorsaturday.HeaderText = "Saturday"
        Me.monitorsaturday.Name = "monitorsaturday"
        Me.monitorsaturday.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.monitorsaturday.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        '
        'monitorsunday
        '
        Me.monitorsunday.HeaderText = "Sunday"
        Me.monitorsunday.Name = "monitorsunday"
        Me.monitorsunday.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.monitorsunday.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        '
        'active
        '
        Me.active.HeaderText = "Active"
        Me.active.Name = "active"
        Me.active.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.active.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.active.Width = 150
        '
        'errorfrequency
        '
        Me.errorfrequency.HeaderText = "Error Frequency (minutes)"
        Me.errorfrequency.Name = "errorfrequency"
        '
        'schedid
        '
        Me.schedid.HeaderText = "schedid"
        Me.schedid.Name = "schedid"
        Me.schedid.ReadOnly = True
        Me.schedid.Visible = False
        '
        'restartattemps
        '
        DataGridViewCellStyle7.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle7.Format = "N0"
        DataGridViewCellStyle7.NullValue = Nothing
        Me.restartattemps.DefaultCellStyle = DataGridViewCellStyle7
        Me.restartattemps.HeaderText = "Restart Attempts"
        Me.restartattemps.Name = "restartattemps"
        '
        'restartfrequency
        '
        DataGridViewCellStyle8.Format = "N0"
        DataGridViewCellStyle8.NullValue = Nothing
        Me.restartfrequency.DefaultCellStyle = DataGridViewCellStyle8
        Me.restartfrequency.HeaderText = "Restart Frequency"
        Me.restartfrequency.Name = "restartfrequency"
        '
        'srsContextMenuStrip1
        '
        Me.srsContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddItem, Me.DeleteItem, Me.EditItem})
        Me.srsContextMenuStrip1.Name = "srsContextMenuStrip1"
        Me.srsContextMenuStrip1.Size = New System.Drawing.Size(227, 70)
        '
        'AddItem
        '
        Me.AddItem.Image = CType(resources.GetObject("AddItem.Image"), System.Drawing.Image)
        Me.AddItem.Name = "AddItem"
        Me.AddItem.Size = New System.Drawing.Size(226, 22)
        Me.AddItem.Text = "Add new item to the shedule"
        '
        'DeleteItem
        '
        Me.DeleteItem.Image = CType(resources.GetObject("DeleteItem.Image"), System.Drawing.Image)
        Me.DeleteItem.Name = "DeleteItem"
        Me.DeleteItem.Size = New System.Drawing.Size(226, 22)
        Me.DeleteItem.Text = "Remove item from shedule"
        '
        'EditItem
        '
        Me.EditItem.Enabled = False
        Me.EditItem.Name = "EditItem"
        Me.EditItem.Size = New System.Drawing.Size(226, 22)
        Me.EditItem.Text = "Edit"
        Me.EditItem.Visible = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(3, 11)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(59, 13)
        Me.Label4.TabIndex = 71
        Me.Label4.Text = "Schedule"
        '
        'SchedReset
        '
        Me.SchedReset.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SchedReset.CausesValidation = False
        Me.SchedReset.Enabled = False
        Me.SchedReset.Image = CType(resources.GetObject("SchedReset.Image"), System.Drawing.Image)
        Me.SchedReset.Location = New System.Drawing.Point(829, 202)
        Me.SchedReset.Name = "SchedReset"
        Me.SchedReset.Size = New System.Drawing.Size(28, 24)
        Me.SchedReset.TabIndex = 70
        Me.SchedReset.UseVisualStyleBackColor = True
        '
        'SchedSave
        '
        Me.SchedSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SchedSave.Enabled = False
        Me.SchedSave.Image = CType(resources.GetObject("SchedSave.Image"), System.Drawing.Image)
        Me.SchedSave.Location = New System.Drawing.Point(795, 202)
        Me.SchedSave.Name = "SchedSave"
        Me.SchedSave.Size = New System.Drawing.Size(28, 24)
        Me.SchedSave.TabIndex = 69
        Me.SchedSave.UseVisualStyleBackColor = True
        '
        'TabPage1
        '
        Me.TabPage1.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.TabPage1.Controls.Add(Me.ClientReset)
        Me.TabPage1.Controls.Add(Me.ClientSave)
        Me.TabPage1.Controls.Add(Me.SplitContainer6)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(870, 396)
        Me.TabPage1.TabIndex = 2
        Me.TabPage1.Text = "Client Details"
        '
        'ClientReset
        '
        Me.ClientReset.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ClientReset.CausesValidation = False
        Me.ClientReset.Enabled = False
        Me.ClientReset.Image = CType(resources.GetObject("ClientReset.Image"), System.Drawing.Image)
        Me.ClientReset.Location = New System.Drawing.Point(836, 366)
        Me.ClientReset.Name = "ClientReset"
        Me.ClientReset.Size = New System.Drawing.Size(28, 24)
        Me.ClientReset.TabIndex = 72
        Me.ClientReset.UseVisualStyleBackColor = True
        '
        'ClientSave
        '
        Me.ClientSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ClientSave.Enabled = False
        Me.ClientSave.Image = CType(resources.GetObject("ClientSave.Image"), System.Drawing.Image)
        Me.ClientSave.Location = New System.Drawing.Point(802, 366)
        Me.ClientSave.Name = "ClientSave"
        Me.ClientSave.Size = New System.Drawing.Size(28, 24)
        Me.ClientSave.TabIndex = 71
        Me.ClientSave.UseVisualStyleBackColor = True
        '
        'SplitContainer6
        '
        Me.SplitContainer6.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainer6.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer6.Name = "SplitContainer6"
        '
        'SplitContainer6.Panel1
        '
        Me.SplitContainer6.Panel1.Controls.Add(Me.uxClientTree)
        Me.SplitContainer6.Panel1.Controls.Add(Me.Label7)
        '
        'SplitContainer6.Panel2
        '
        Me.SplitContainer6.Panel2.Controls.Add(Me.uxClientDetails)
        Me.SplitContainer6.Size = New System.Drawing.Size(871, 360)
        Me.SplitContainer6.SplitterDistance = 255
        Me.SplitContainer6.TabIndex = 0
        '
        'uxClientTree
        '
        Me.uxClientTree.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxClientTree.ContextMenuStrip = Me.srsClientContextMenuStrip
        Me.uxClientTree.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxClientTree.ImageIndex = 0
        Me.uxClientTree.ImageList = Me.repimages
        Me.uxClientTree.Location = New System.Drawing.Point(3, 21)
        Me.uxClientTree.Name = "uxClientTree"
        Me.uxClientTree.SelectedImageIndex = 0
        Me.uxClientTree.Size = New System.Drawing.Size(252, 339)
        Me.uxClientTree.TabIndex = 73
        '
        'srsClientContextMenuStrip
        '
        Me.srsClientContextMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.Addclient})
        Me.srsClientContextMenuStrip.Name = "srsClientContextMenuStrip"
        Me.srsClientContextMenuStrip.Size = New System.Drawing.Size(156, 26)
        '
        'Addclient
        '
        Me.Addclient.Image = CType(resources.GetObject("Addclient.Image"), System.Drawing.Image)
        Me.Addclient.Name = "Addclient"
        Me.Addclient.Size = New System.Drawing.Size(155, 22)
        Me.Addclient.Text = "Add new Client"
        '
        'Label7
        '
        Me.Label7.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(3, 5)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(83, 13)
        Me.Label7.TabIndex = 70
        Me.Label7.Text = "Client Details"
        '
        'uxClientDetails
        '
        Me.uxClientDetails.AllowUserToAddRows = False
        Me.uxClientDetails.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxClientDetails.BackgroundColor = System.Drawing.SystemColors.Window
        Me.uxClientDetails.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.uxClientDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.uxClientDetails.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.cattribute, Me.cvalue})
        Me.uxClientDetails.ContextMenuStrip = Me.srsClientContextMenuStrip
        DataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle10.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.uxClientDetails.DefaultCellStyle = DataGridViewCellStyle10
        Me.uxClientDetails.Location = New System.Drawing.Point(3, 21)
        Me.uxClientDetails.Name = "uxClientDetails"
        Me.uxClientDetails.RowHeadersVisible = False
        Me.uxClientDetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.uxClientDetails.Size = New System.Drawing.Size(606, 339)
        Me.uxClientDetails.TabIndex = 71
        '
        'cattribute
        '
        Me.cattribute.HeaderText = "Attribute"
        Me.cattribute.Name = "cattribute"
        '
        'cvalue
        '
        Me.cvalue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.cvalue.HeaderText = "Value"
        Me.cvalue.Name = "cvalue"
        '
        'tabemail
        '
        Me.tabemail.Controls.Add(Me.SplitContainer7)
        Me.tabemail.Location = New System.Drawing.Point(4, 22)
        Me.tabemail.Name = "tabemail"
        Me.tabemail.Size = New System.Drawing.Size(870, 396)
        Me.tabemail.TabIndex = 3
        Me.tabemail.Text = "Email Details"
        Me.tabemail.UseVisualStyleBackColor = True
        '
        'SplitContainer7
        '
        Me.SplitContainer7.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.SplitContainer7.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer7.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer7.Name = "SplitContainer7"
        '
        'SplitContainer7.Panel1
        '
        Me.SplitContainer7.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.SplitContainer7.Panel1.Controls.Add(Me.Label9)
        Me.SplitContainer7.Panel1.Controls.Add(Me.uxEmailTree)
        '
        'SplitContainer7.Panel2
        '
        Me.SplitContainer7.Panel2.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.SplitContainer7.Panel2.Controls.Add(Me.Emailreset)
        Me.SplitContainer7.Panel2.Controls.Add(Me.EmailSave)
        Me.SplitContainer7.Panel2.Controls.Add(Me.uxPrefDetails)
        Me.SplitContainer7.Panel2.Controls.Add(Me.uxEmailDetails)
        Me.SplitContainer7.Size = New System.Drawing.Size(870, 396)
        Me.SplitContainer7.SplitterDistance = 253
        Me.SplitContainer7.TabIndex = 0
        '
        'Label9
        '
        Me.Label9.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(3, 2)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(38, 13)
        Me.Label9.TabIndex = 73
        Me.Label9.Text = "Email"
        '
        'uxEmailTree
        '
        Me.uxEmailTree.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxEmailTree.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxEmailTree.ImageIndex = 0
        Me.uxEmailTree.ImageList = Me.repimages
        Me.uxEmailTree.Location = New System.Drawing.Point(2, 21)
        Me.uxEmailTree.Name = "uxEmailTree"
        Me.uxEmailTree.SelectedImageIndex = 0
        Me.uxEmailTree.Size = New System.Drawing.Size(250, 339)
        Me.uxEmailTree.TabIndex = 72
        '
        'Emailreset
        '
        Me.Emailreset.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Emailreset.CausesValidation = False
        Me.Emailreset.Enabled = False
        Me.Emailreset.Image = CType(resources.GetObject("Emailreset.Image"), System.Drawing.Image)
        Me.Emailreset.Location = New System.Drawing.Point(577, 365)
        Me.Emailreset.Name = "Emailreset"
        Me.Emailreset.Size = New System.Drawing.Size(28, 24)
        Me.Emailreset.TabIndex = 74
        Me.Emailreset.UseVisualStyleBackColor = True
        '
        'EmailSave
        '
        Me.EmailSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.EmailSave.Enabled = False
        Me.EmailSave.Image = CType(resources.GetObject("EmailSave.Image"), System.Drawing.Image)
        Me.EmailSave.Location = New System.Drawing.Point(543, 365)
        Me.EmailSave.Name = "EmailSave"
        Me.EmailSave.Size = New System.Drawing.Size(28, 24)
        Me.EmailSave.TabIndex = 73
        Me.EmailSave.UseVisualStyleBackColor = True
        '
        'uxPrefDetails
        '
        Me.uxPrefDetails.AllowUserToAddRows = False
        Me.uxPrefDetails.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxPrefDetails.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.uxPrefDetails.BackgroundColor = System.Drawing.SystemColors.Window
        Me.uxPrefDetails.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.uxPrefDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.uxPrefDetails.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.filterid, Me.prefid, Me.pclientemailid, Me.pclient, Me.preftype, Me.pdescription, Me.value1, Me.value2})
        Me.uxPrefDetails.ContextMenuStrip = Me.srsEmailContextMenuStrip
        DataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle11.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.uxPrefDetails.DefaultCellStyle = DataGridViewCellStyle11
        Me.uxPrefDetails.Location = New System.Drawing.Point(3, 195)
        Me.uxPrefDetails.MultiSelect = False
        Me.uxPrefDetails.Name = "uxPrefDetails"
        Me.uxPrefDetails.RowHeadersVisible = False
        Me.uxPrefDetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.uxPrefDetails.Size = New System.Drawing.Size(607, 56)
        Me.uxPrefDetails.TabIndex = 2
        '
        'filterid
        '
        Me.filterid.HeaderText = "Filter id"
        Me.filterid.Name = "filterid"
        Me.filterid.Visible = False
        Me.filterid.Width = 46
        '
        'prefid
        '
        Me.prefid.HeaderText = "Pref id"
        Me.prefid.Name = "prefid"
        Me.prefid.Visible = False
        Me.prefid.Width = 43
        '
        'pclientemailid
        '
        Me.pclientemailid.HeaderText = "clientemailid"
        Me.pclientemailid.Name = "pclientemailid"
        Me.pclientemailid.Visible = False
        Me.pclientemailid.Width = 70
        '
        'pclient
        '
        Me.pclient.HeaderText = "Client"
        Me.pclient.Name = "pclient"
        Me.pclient.ReadOnly = True
        Me.pclient.Width = 58
        '
        'preftype
        '
        Me.preftype.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.preftype.FillWeight = 150.0!
        Me.preftype.HeaderText = "Pref. Type"
        Me.preftype.MinimumWidth = 150
        Me.preftype.Name = "preftype"
        Me.preftype.ReadOnly = True
        Me.preftype.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        '
        'pdescription
        '
        Me.pdescription.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.pdescription.HeaderText = "Description"
        Me.pdescription.MinimumWidth = 100
        Me.pdescription.Name = "pdescription"
        Me.pdescription.ReadOnly = True
        '
        'value1
        '
        Me.value1.HeaderText = "Value 1"
        Me.value1.Name = "value1"
        Me.value1.ReadOnly = True
        Me.value1.Width = 68
        '
        'value2
        '
        Me.value2.HeaderText = "Value 2"
        Me.value2.Name = "value2"
        Me.value2.ReadOnly = True
        Me.value2.Width = 68
        '
        'srsEmailContextMenuStrip
        '
        Me.srsEmailContextMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.InsertAddress, Me.InertPref, Me.DeleteAddress, Me.DeletePref})
        Me.srsEmailContextMenuStrip.Name = "srsEmailContextMenuStrip"
        Me.srsEmailContextMenuStrip.Size = New System.Drawing.Size(207, 92)
        '
        'InsertAddress
        '
        Me.InsertAddress.Image = CType(resources.GetObject("InsertAddress.Image"), System.Drawing.Image)
        Me.InsertAddress.Name = "InsertAddress"
        Me.InsertAddress.Size = New System.Drawing.Size(206, 22)
        Me.InsertAddress.Text = "Add new Email Address"
        '
        'InertPref
        '
        Me.InertPref.Image = CType(resources.GetObject("InertPref.Image"), System.Drawing.Image)
        Me.InertPref.Name = "InertPref"
        Me.InertPref.Size = New System.Drawing.Size(206, 22)
        Me.InertPref.Text = "Add new User Preference"
        '
        'DeleteAddress
        '
        Me.DeleteAddress.Image = CType(resources.GetObject("DeleteAddress.Image"), System.Drawing.Image)
        Me.DeleteAddress.Name = "DeleteAddress"
        Me.DeleteAddress.Size = New System.Drawing.Size(206, 22)
        Me.DeleteAddress.Text = "Delete Email Address"
        '
        'DeletePref
        '
        Me.DeletePref.Image = CType(resources.GetObject("DeletePref.Image"), System.Drawing.Image)
        Me.DeletePref.Name = "DeletePref"
        Me.DeletePref.Size = New System.Drawing.Size(206, 22)
        Me.DeletePref.Text = "Delete User Preference"
        '
        'uxEmailDetails
        '
        Me.uxEmailDetails.AllowUserToAddRows = False
        Me.uxEmailDetails.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxEmailDetails.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.uxEmailDetails.BackgroundColor = System.Drawing.SystemColors.Window
        Me.uxEmailDetails.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.uxEmailDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.uxEmailDetails.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.emailid, Me.eclient, Me.ename, Me.edescription, Me.eaddress, Me.eonerror, Me.eonstatuscheck})
        Me.uxEmailDetails.ContextMenuStrip = Me.srsEmailContextMenuStrip
        Me.uxEmailDetails.Cursor = System.Windows.Forms.Cursors.Default
        DataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle12.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.uxEmailDetails.DefaultCellStyle = DataGridViewCellStyle12
        Me.uxEmailDetails.Location = New System.Drawing.Point(3, 21)
        Me.uxEmailDetails.MultiSelect = False
        Me.uxEmailDetails.Name = "uxEmailDetails"
        Me.uxEmailDetails.RowHeadersVisible = False
        Me.uxEmailDetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.uxEmailDetails.Size = New System.Drawing.Size(607, 44)
        Me.uxEmailDetails.TabIndex = 1
        '
        'emailid
        '
        Me.emailid.HeaderText = "email id"
        Me.emailid.Name = "emailid"
        Me.emailid.ReadOnly = True
        Me.emailid.Visible = False
        Me.emailid.Width = 48
        '
        'eclient
        '
        Me.eclient.HeaderText = "Client"
        Me.eclient.Name = "eclient"
        Me.eclient.ReadOnly = True
        Me.eclient.Width = 58
        '
        'ename
        '
        Me.ename.HeaderText = "Name"
        Me.ename.Name = "ename"
        Me.ename.Width = 60
        '
        'edescription
        '
        Me.edescription.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.edescription.HeaderText = "Description"
        Me.edescription.MinimumWidth = 100
        Me.edescription.Name = "edescription"
        '
        'eaddress
        '
        Me.eaddress.FillWeight = 150.0!
        Me.eaddress.HeaderText = "Email Address"
        Me.eaddress.MinimumWidth = 150
        Me.eaddress.Name = "eaddress"
        Me.eaddress.Width = 150
        '
        'eonerror
        '
        Me.eonerror.HeaderText = "On Error"
        Me.eonerror.Name = "eonerror"
        Me.eonerror.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.eonerror.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.eonerror.Width = 71
        '
        'eonstatuscheck
        '
        Me.eonstatuscheck.HeaderText = "On Status"
        Me.eonstatuscheck.Name = "eonstatuscheck"
        Me.eonstatuscheck.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.eonstatuscheck.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.eonstatuscheck.Width = 79
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.DataGridViewTextBoxColumn1.HeaderText = "Job Name"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.DataGridViewTextBoxColumn2.HeaderText = "Description"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.DataGridViewTextBoxColumn3.HeaderText = "Task Name"
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        '
        'DataGridViewTextBoxColumn4
        '
        Me.DataGridViewTextBoxColumn4.HeaderText = "Last Run Date"
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.Width = 150
        '
        'DataGridViewTextBoxColumn5
        '
        Me.DataGridViewTextBoxColumn5.HeaderText = "Last Date Error Found"
        Me.DataGridViewTextBoxColumn5.Name = "DataGridViewTextBoxColumn5"
        Me.DataGridViewTextBoxColumn5.Width = 150
        '
        'DataGridViewTextBoxColumn6
        '
        Me.DataGridViewTextBoxColumn6.HeaderText = "Last Error ID"
        Me.DataGridViewTextBoxColumn6.Name = "DataGridViewTextBoxColumn6"
        '
        'DataGridViewTextBoxColumn7
        '
        Me.DataGridViewTextBoxColumn7.HeaderText = "Last Restart Attempt"
        Me.DataGridViewTextBoxColumn7.Name = "DataGridViewTextBoxColumn7"
        Me.DataGridViewTextBoxColumn7.Width = 150
        '
        'DataGridViewTextBoxColumn8
        '
        Me.DataGridViewTextBoxColumn8.HeaderText = "Restart Result"
        Me.DataGridViewTextBoxColumn8.Name = "DataGridViewTextBoxColumn8"
        '
        'DataGridViewTextBoxColumn9
        '
        Me.DataGridViewTextBoxColumn9.HeaderText = "Restart Trys"
        Me.DataGridViewTextBoxColumn9.Name = "DataGridViewTextBoxColumn9"
        '
        'DataGridViewTextBoxColumn10
        '
        Me.DataGridViewTextBoxColumn10.HeaderText = "Error ID"
        Me.DataGridViewTextBoxColumn10.Name = "DataGridViewTextBoxColumn10"
        Me.DataGridViewTextBoxColumn10.Width = 68
        '
        'DataGridViewTextBoxColumn11
        '
        Me.DataGridViewTextBoxColumn11.HeaderText = "Error Date"
        Me.DataGridViewTextBoxColumn11.MinimumWidth = 125
        Me.DataGridViewTextBoxColumn11.Name = "DataGridViewTextBoxColumn11"
        Me.DataGridViewTextBoxColumn11.Width = 125
        '
        'DataGridViewTextBoxColumn12
        '
        Me.DataGridViewTextBoxColumn12.HeaderText = "Severity"
        Me.DataGridViewTextBoxColumn12.Name = "DataGridViewTextBoxColumn12"
        Me.DataGridViewTextBoxColumn12.Width = 70
        '
        'DataGridViewTextBoxColumn13
        '
        Me.DataGridViewTextBoxColumn13.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.DataGridViewTextBoxColumn13.HeaderText = "Error"
        Me.DataGridViewTextBoxColumn13.MinimumWidth = 2000
        Me.DataGridViewTextBoxColumn13.Name = "DataGridViewTextBoxColumn13"
        '
        'DataGridViewTextBoxColumn14
        '
        Me.DataGridViewTextBoxColumn14.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.DataGridViewTextBoxColumn14.HeaderText = "Attribute"
        Me.DataGridViewTextBoxColumn14.Name = "DataGridViewTextBoxColumn14"
        '
        'DataGridViewTextBoxColumn15
        '
        Me.DataGridViewTextBoxColumn15.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.DataGridViewTextBoxColumn15.HeaderText = "Value"
        Me.DataGridViewTextBoxColumn15.Name = "DataGridViewTextBoxColumn15"
        '
        'DataGridViewTextBoxColumn16
        '
        Me.DataGridViewTextBoxColumn16.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        DataGridViewCellStyle13.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.DataGridViewTextBoxColumn16.DefaultCellStyle = DataGridViewCellStyle13
        Me.DataGridViewTextBoxColumn16.HeaderText = "Job Name"
        Me.DataGridViewTextBoxColumn16.MinimumWidth = 150
        Me.DataGridViewTextBoxColumn16.Name = "DataGridViewTextBoxColumn16"
        Me.DataGridViewTextBoxColumn16.ReadOnly = True
        Me.DataGridViewTextBoxColumn16.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        '
        'DataGridViewTextBoxColumn17
        '
        Me.DataGridViewTextBoxColumn17.HeaderText = "Client"
        Me.DataGridViewTextBoxColumn17.Name = "DataGridViewTextBoxColumn17"
        Me.DataGridViewTextBoxColumn17.ReadOnly = True
        Me.DataGridViewTextBoxColumn17.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        '
        'DataGridViewTextBoxColumn18
        '
        Me.DataGridViewTextBoxColumn18.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.DataGridViewTextBoxColumn18.HeaderText = "Task Name"
        Me.DataGridViewTextBoxColumn18.MinimumWidth = 150
        Me.DataGridViewTextBoxColumn18.Name = "DataGridViewTextBoxColumn18"
        '
        'TimeColumn1
        '
        Me.TimeColumn1.HeaderText = "One Off Time"
        Me.TimeColumn1.Name = "TimeColumn1"
        '
        'DataGridViewTextBoxColumn19
        '
        DataGridViewCellStyle14.Format = "N0"
        DataGridViewCellStyle14.NullValue = Nothing
        Me.DataGridViewTextBoxColumn19.DefaultCellStyle = DataGridViewCellStyle14
        Me.DataGridViewTextBoxColumn19.HeaderText = "Frequency (minutes)"
        Me.DataGridViewTextBoxColumn19.Name = "DataGridViewTextBoxColumn19"
        '
        'TimeColumn2
        '
        Me.TimeColumn2.HeaderText = "Start Time"
        Me.TimeColumn2.Name = "TimeColumn2"
        '
        'TimeColumn3
        '
        Me.TimeColumn3.HeaderText = "End Time"
        Me.TimeColumn3.Name = "TimeColumn3"
        '
        'DataGridViewTextBoxColumn20
        '
        Me.DataGridViewTextBoxColumn20.HeaderText = "Error Frequency (minutes)"
        Me.DataGridViewTextBoxColumn20.Name = "DataGridViewTextBoxColumn20"
        '
        'DataGridViewTextBoxColumn21
        '
        Me.DataGridViewTextBoxColumn21.HeaderText = "schedid"
        Me.DataGridViewTextBoxColumn21.Name = "DataGridViewTextBoxColumn21"
        Me.DataGridViewTextBoxColumn21.ReadOnly = True
        Me.DataGridViewTextBoxColumn21.Visible = False
        '
        'DataGridViewTextBoxColumn22
        '
        DataGridViewCellStyle15.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle15.Format = "N0"
        DataGridViewCellStyle15.NullValue = Nothing
        Me.DataGridViewTextBoxColumn22.DefaultCellStyle = DataGridViewCellStyle15
        Me.DataGridViewTextBoxColumn22.HeaderText = "Restart Attempts"
        Me.DataGridViewTextBoxColumn22.Name = "DataGridViewTextBoxColumn22"
        '
        'DataGridViewTextBoxColumn23
        '
        DataGridViewCellStyle16.Format = "N0"
        DataGridViewCellStyle16.NullValue = Nothing
        Me.DataGridViewTextBoxColumn23.DefaultCellStyle = DataGridViewCellStyle16
        Me.DataGridViewTextBoxColumn23.HeaderText = "Restart Frequency"
        Me.DataGridViewTextBoxColumn23.Name = "DataGridViewTextBoxColumn23"
        '
        'DataGridViewTextBoxColumn24
        '
        Me.DataGridViewTextBoxColumn24.HeaderText = "Attribute"
        Me.DataGridViewTextBoxColumn24.Name = "DataGridViewTextBoxColumn24"
        '
        'DataGridViewTextBoxColumn25
        '
        Me.DataGridViewTextBoxColumn25.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.DataGridViewTextBoxColumn25.HeaderText = "Value"
        Me.DataGridViewTextBoxColumn25.Name = "DataGridViewTextBoxColumn25"
        '
        'DataGridViewTextBoxColumn26
        '
        Me.DataGridViewTextBoxColumn26.HeaderText = "Filter id"
        Me.DataGridViewTextBoxColumn26.Name = "DataGridViewTextBoxColumn26"
        Me.DataGridViewTextBoxColumn26.Visible = False
        Me.DataGridViewTextBoxColumn26.Width = 46
        '
        'DataGridViewTextBoxColumn27
        '
        Me.DataGridViewTextBoxColumn27.HeaderText = "Pref id"
        Me.DataGridViewTextBoxColumn27.Name = "DataGridViewTextBoxColumn27"
        Me.DataGridViewTextBoxColumn27.Visible = False
        Me.DataGridViewTextBoxColumn27.Width = 43
        '
        'DataGridViewTextBoxColumn28
        '
        Me.DataGridViewTextBoxColumn28.HeaderText = "clientemailid"
        Me.DataGridViewTextBoxColumn28.Name = "DataGridViewTextBoxColumn28"
        Me.DataGridViewTextBoxColumn28.Visible = False
        Me.DataGridViewTextBoxColumn28.Width = 70
        '
        'DataGridViewTextBoxColumn29
        '
        Me.DataGridViewTextBoxColumn29.HeaderText = "Client"
        Me.DataGridViewTextBoxColumn29.Name = "DataGridViewTextBoxColumn29"
        Me.DataGridViewTextBoxColumn29.ReadOnly = True
        Me.DataGridViewTextBoxColumn29.Width = 58
        '
        'DataGridViewTextBoxColumn30
        '
        Me.DataGridViewTextBoxColumn30.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.DataGridViewTextBoxColumn30.FillWeight = 150.0!
        Me.DataGridViewTextBoxColumn30.HeaderText = "Pref. Type"
        Me.DataGridViewTextBoxColumn30.MinimumWidth = 150
        Me.DataGridViewTextBoxColumn30.Name = "DataGridViewTextBoxColumn30"
        Me.DataGridViewTextBoxColumn30.ReadOnly = True
        Me.DataGridViewTextBoxColumn30.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        '
        'DataGridViewTextBoxColumn31
        '
        Me.DataGridViewTextBoxColumn31.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.DataGridViewTextBoxColumn31.HeaderText = "Description"
        Me.DataGridViewTextBoxColumn31.MinimumWidth = 100
        Me.DataGridViewTextBoxColumn31.Name = "DataGridViewTextBoxColumn31"
        Me.DataGridViewTextBoxColumn31.ReadOnly = True
        '
        'DataGridViewTextBoxColumn32
        '
        Me.DataGridViewTextBoxColumn32.HeaderText = "Value 1"
        Me.DataGridViewTextBoxColumn32.Name = "DataGridViewTextBoxColumn32"
        Me.DataGridViewTextBoxColumn32.ReadOnly = True
        Me.DataGridViewTextBoxColumn32.Width = 68
        '
        'DataGridViewTextBoxColumn33
        '
        Me.DataGridViewTextBoxColumn33.HeaderText = "Value 2"
        Me.DataGridViewTextBoxColumn33.Name = "DataGridViewTextBoxColumn33"
        Me.DataGridViewTextBoxColumn33.ReadOnly = True
        Me.DataGridViewTextBoxColumn33.Width = 68
        '
        'DataGridViewTextBoxColumn34
        '
        Me.DataGridViewTextBoxColumn34.HeaderText = "email id"
        Me.DataGridViewTextBoxColumn34.Name = "DataGridViewTextBoxColumn34"
        Me.DataGridViewTextBoxColumn34.ReadOnly = True
        Me.DataGridViewTextBoxColumn34.Visible = False
        Me.DataGridViewTextBoxColumn34.Width = 48
        '
        'DataGridViewTextBoxColumn35
        '
        Me.DataGridViewTextBoxColumn35.HeaderText = "Client"
        Me.DataGridViewTextBoxColumn35.Name = "DataGridViewTextBoxColumn35"
        Me.DataGridViewTextBoxColumn35.ReadOnly = True
        Me.DataGridViewTextBoxColumn35.Width = 58
        '
        'DataGridViewTextBoxColumn36
        '
        Me.DataGridViewTextBoxColumn36.HeaderText = "Name"
        Me.DataGridViewTextBoxColumn36.Name = "DataGridViewTextBoxColumn36"
        Me.DataGridViewTextBoxColumn36.ReadOnly = True
        Me.DataGridViewTextBoxColumn36.Width = 60
        '
        'DataGridViewTextBoxColumn37
        '
        Me.DataGridViewTextBoxColumn37.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.DataGridViewTextBoxColumn37.HeaderText = "Description"
        Me.DataGridViewTextBoxColumn37.MinimumWidth = 100
        Me.DataGridViewTextBoxColumn37.Name = "DataGridViewTextBoxColumn37"
        '
        'DataGridViewTextBoxColumn38
        '
        Me.DataGridViewTextBoxColumn38.FillWeight = 150.0!
        Me.DataGridViewTextBoxColumn38.HeaderText = "Email Address"
        Me.DataGridViewTextBoxColumn38.MinimumWidth = 150
        Me.DataGridViewTextBoxColumn38.Name = "DataGridViewTextBoxColumn38"
        Me.DataGridViewTextBoxColumn38.Width = 150
        '
        'bc_am_cp_monitor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(885, 458)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.uxClients)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "bc_am_cp_monitor"
        Me.Text = "Form1"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage2.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel2.ResumeLayout(False)
        Me.SplitContainer2.Panel2.PerformLayout()
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer2.ResumeLayout(False)
        Me.SplitContainer3.Panel1.ResumeLayout(False)
        Me.SplitContainer3.Panel1.PerformLayout()
        Me.SplitContainer3.Panel2.ResumeLayout(False)
        Me.SplitContainer3.Panel2.PerformLayout()
        CType(Me.SplitContainer3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer3.ResumeLayout(False)
        CType(Me.uxGridJob, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxStatGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxErrorGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MonitorTabPage.ResumeLayout(False)
        Me.SplitContainer4.Panel1.ResumeLayout(False)
        Me.SplitContainer4.Panel2.ResumeLayout(False)
        Me.SplitContainer4.Panel2.PerformLayout()
        CType(Me.SplitContainer4, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer4.ResumeLayout(False)
        Me.SplitContainer5.Panel1.ResumeLayout(False)
        Me.SplitContainer5.Panel2.ResumeLayout(False)
        Me.SplitContainer5.Panel2.PerformLayout()
        CType(Me.SplitContainer5, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer5.ResumeLayout(False)
        Me.srsJobContextMenuStrip.ResumeLayout(False)
        CType(Me.uxJobsGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uxSheduleGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.srsContextMenuStrip1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.SplitContainer6.Panel1.ResumeLayout(False)
        Me.SplitContainer6.Panel1.PerformLayout()
        Me.SplitContainer6.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer6, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer6.ResumeLayout(False)
        Me.srsClientContextMenuStrip.ResumeLayout(False)
        CType(Me.uxClientDetails, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabemail.ResumeLayout(False)
        Me.SplitContainer7.Panel1.ResumeLayout(False)
        Me.SplitContainer7.Panel1.PerformLayout()
        Me.SplitContainer7.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer7, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer7.ResumeLayout(False)
        CType(Me.uxPrefDetails, System.ComponentModel.ISupportInitialize).EndInit()
        Me.srsEmailContextMenuStrip.ResumeLayout(False)
        CType(Me.uxEmailDetails, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents uxClients As System.Windows.Forms.ComboBox
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents uxClear As System.Windows.Forms.Button
    Friend WithEvents uxRefresh As System.Windows.Forms.Button
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents uxTJobs As System.Windows.Forms.TreeView
    Friend WithEvents SplitContainer2 As System.Windows.Forms.SplitContainer
    Friend WithEvents SplitContainer3 As System.Windows.Forms.SplitContainer
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Protected Friend WithEvents uxGridJob As System.Windows.Forms.DataGridView
    Friend WithEvents checkname As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents jobdescription As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Protected Friend WithEvents uxStatGrid As System.Windows.Forms.DataGridView
    Friend WithEvents sritemname As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents lastrundate As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents errorfound As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents lasterrordate As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents lasterrorid As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents lastrestart As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents restartresult As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents restartcount As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents uxErrorGrid As System.Windows.Forms.DataGridView
    Friend WithEvents MonitorTabPage As System.Windows.Forms.TabPage
    Friend WithEvents SplitContainer4 As System.Windows.Forms.SplitContainer
    Friend WithEvents SplitContainer5 As System.Windows.Forms.SplitContainer
    Friend WithEvents uxLJobs As System.Windows.Forms.ListView
    Friend WithEvents uxJobsGrid As System.Windows.Forms.DataGridView
    Friend WithEvents attribute As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents value As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Protected Friend WithEvents uxSheduleGrid As System.Windows.Forms.DataGridView
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents SchedReset As System.Windows.Forms.Button
    Friend WithEvents SchedSave As System.Windows.Forms.Button
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents ClientReset As System.Windows.Forms.Button
    Friend WithEvents ClientSave As System.Windows.Forms.Button
    Friend WithEvents SplitContainer6 As System.Windows.Forms.SplitContainer
    Friend WithEvents uxClientTree As System.Windows.Forms.TreeView
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents uxClientDetails As System.Windows.Forms.DataGridView
    Friend WithEvents cattribute As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents cvalue As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents tabemail As System.Windows.Forms.TabPage
    Friend WithEvents SplitContainer7 As System.Windows.Forms.SplitContainer
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents uxEmailTree As System.Windows.Forms.TreeView
    Friend WithEvents Emailreset As System.Windows.Forms.Button
    Friend WithEvents EmailSave As System.Windows.Forms.Button
    Friend WithEvents uxPrefDetails As System.Windows.Forms.DataGridView
    Friend WithEvents filterid As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents prefid As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents pclientemailid As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents pclient As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents preftype As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents pdescription As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents value1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents value2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Public WithEvents uxEmailDetails As System.Windows.Forms.DataGridView
    Friend WithEvents repimages As System.Windows.Forms.ImageList
    Friend WithEvents srsContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents AddItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EditItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents srsJobContextMenuStrip As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents AddJobItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents srsClientContextMenuStrip As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents Addclient As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents srsEmailContextMenuStrip As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents InsertAddress As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents InertPref As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents JobNameS As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents clientname As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ItemName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents scheduletime As BlueCurve.CommonPlatform.AM.TimeColumn
    Friend WithEvents Frequency As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents monitorstarttime As BlueCurve.CommonPlatform.AM.TimeColumn
    Friend WithEvents monitorendtime As BlueCurve.CommonPlatform.AM.TimeColumn
    Friend WithEvents monitormonday As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents monitortuesday As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents monitorwednesday As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents monitorthursday As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents monitorfriday As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents monitorsaturday As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents monitorsunday As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents active As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents errorfrequency As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents schedid As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents restartattemps As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents restartfrequency As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn5 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn6 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn7 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn8 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn9 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn10 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn11 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn12 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn13 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn14 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn15 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn16 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn17 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn18 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TimeColumn1 As BlueCurve.CommonPlatform.AM.TimeColumn
    Friend WithEvents DataGridViewTextBoxColumn19 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TimeColumn2 As BlueCurve.CommonPlatform.AM.TimeColumn
    Friend WithEvents TimeColumn3 As BlueCurve.CommonPlatform.AM.TimeColumn
    Friend WithEvents DataGridViewTextBoxColumn20 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn21 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn22 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn23 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn24 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn25 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn26 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn27 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn28 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn29 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn30 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn31 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn32 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn33 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn34 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn35 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn36 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn37 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn38 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents emailid As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents eclient As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ename As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents edescription As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents eaddress As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents eonerror As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents eonstatuscheck As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents DeleteAddress As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DeleteItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents errorid As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents errordate As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents severity As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents srserrortext As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DeletePref As System.Windows.Forms.ToolStripMenuItem
End Class
