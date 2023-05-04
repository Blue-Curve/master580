Imports System.Drawing
Imports System.Windows.forms
Imports system.Diagnostics
Imports BlueCurve.Core.CS



Public Class bc_am_workflow_settings
    Inherits System.Windows.Forms.Form

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
    Friend WithEvents pinterval As System.Windows.Forms.Label
    Friend WithEvents SettingsTabs As System.Windows.Forms.TabControl
    Friend WithEvents PollingTab As System.Windows.Forms.TabPage
    Friend WithEvents RefreshTab As System.Windows.Forms.TabPage
    Friend WithEvents ColoursTab As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents ColorDialog1 As System.Windows.Forms.ColorDialog
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents BCLogoPic As System.Windows.Forms.PictureBox
    Friend WithEvents RestorePollingDefaults As System.Windows.Forms.Button
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents NextWkfStageBackColor As System.Windows.Forms.Button
    Friend WithEvents CurrentWkfStageBackColor As System.Windows.Forms.Button
    Friend WithEvents ExpiredBackColor As System.Windows.Forms.Button
    Friend WithEvents SearchBackColor As System.Windows.Forms.Button
    Friend WithEvents StageChangedBackColor As System.Windows.Forms.Button
    Friend WithEvents UrgentBackColor As System.Windows.Forms.Button
    Friend WithEvents UnviewedBackColor As System.Windows.Forms.Button
    Friend WithEvents ViewedBackColor As System.Windows.Forms.Button
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents ViewedForeColor As System.Windows.Forms.Button
    Friend WithEvents UnviewedForeColor As System.Windows.Forms.Button
    Friend WithEvents UrgentForeColor As System.Windows.Forms.Button
    Friend WithEvents StageChangedForeColor As System.Windows.Forms.Button
    Friend WithEvents SearchResultForeColor As System.Windows.Forms.Button
    Friend WithEvents ExpiredForeColor As System.Windows.Forms.Button
    Friend WithEvents CurrentWkfStageForeColor As System.Windows.Forms.Button
    Friend WithEvents NextWkfStageForeColor As System.Windows.Forms.Button
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents SaveButton As System.Windows.Forms.Button
    Friend WithEvents PollingEnabled As System.Windows.Forms.CheckBox
    Friend WithEvents PollingSettings As System.Windows.Forms.GroupBox
    Friend WithEvents cbeep As System.Windows.Forms.CheckBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents pscreen As System.Windows.Forms.CheckBox
    Friend WithEvents palerter As System.Windows.Forms.CheckBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents IntervalSettingHelp As System.Windows.Forms.PictureBox
    Friend WithEvents UrgentSettingHelp As System.Windows.Forms.PictureBox
    Friend WithEvents ScreenSettingHelp As System.Windows.Forms.PictureBox
    Friend WithEvents AlerterSettingHelp As System.Windows.Forms.PictureBox
    Friend WithEvents AlerterSettings As System.Windows.Forms.GroupBox
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents RefreshSettings As System.Windows.Forms.GroupBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents Label27 As System.Windows.Forms.Label
    Friend WithEvents AutoRefresh As System.Windows.Forms.CheckBox
    Friend WithEvents MinRefreshTime As System.Windows.Forms.ListBox
    Friend WithEvents Cancel As System.Windows.Forms.Button
    Friend WithEvents Interval As System.Windows.Forms.ComboBox
    Friend WithEvents UrgentDays As System.Windows.Forms.ComboBox
    Friend WithEvents FadeTime As System.Windows.Forms.ComboBox
    Friend WithEvents InactivitySettings As System.Windows.Forms.GroupBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents RefreshAfterActivityTime As System.Windows.Forms.ListBox
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents cfilterheight As System.Windows.Forms.ComboBox
    Friend WithEvents Label28 As System.Windows.Forms.Label
    Friend WithEvents Csnap As System.Windows.Forms.CheckBox
    Friend WithEvents Label29 As System.Windows.Forms.Label
    Friend WithEvents Label31 As System.Windows.Forms.Label
    Friend WithEvents Cdayasback As System.Windows.Forms.ComboBox
    Friend WithEvents Label30 As System.Windows.Forms.Label
    Friend WithEvents actIntaval As System.Windows.Forms.Label
    Friend WithEvents InactInterval As System.Windows.Forms.ComboBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_workflow_settings))
        Me.SettingsTabs = New System.Windows.Forms.TabControl
        Me.PollingTab = New System.Windows.Forms.TabPage
        Me.Label25 = New System.Windows.Forms.Label
        Me.PollingSettings = New System.Windows.Forms.GroupBox
        Me.InactInterval = New System.Windows.Forms.ComboBox
        Me.actIntaval = New System.Windows.Forms.Label
        Me.UrgentDays = New System.Windows.Forms.ComboBox
        Me.Interval = New System.Windows.Forms.ComboBox
        Me.AlerterSettingHelp = New System.Windows.Forms.PictureBox
        Me.ScreenSettingHelp = New System.Windows.Forms.PictureBox
        Me.UrgentSettingHelp = New System.Windows.Forms.PictureBox
        Me.IntervalSettingHelp = New System.Windows.Forms.PictureBox
        Me.AlerterSettings = New System.Windows.Forms.GroupBox
        Me.FadeTime = New System.Windows.Forms.ComboBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.cbeep = New System.Windows.Forms.CheckBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.pscreen = New System.Windows.Forms.CheckBox
        Me.palerter = New System.Windows.Forms.CheckBox
        Me.pinterval = New System.Windows.Forms.Label
        Me.PollingEnabled = New System.Windows.Forms.CheckBox
        Me.RefreshTab = New System.Windows.Forms.TabPage
        Me.InactivitySettings = New System.Windows.Forms.GroupBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.RefreshAfterActivityTime = New System.Windows.Forms.ListBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label26 = New System.Windows.Forms.Label
        Me.RefreshSettings = New System.Windows.Forms.GroupBox
        Me.MinRefreshTime = New System.Windows.Forms.ListBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.AutoRefresh = New System.Windows.Forms.CheckBox
        Me.ColoursTab = New System.Windows.Forms.TabPage
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.Label24 = New System.Windows.Forms.Label
        Me.Label23 = New System.Windows.Forms.Label
        Me.NextWkfStageForeColor = New System.Windows.Forms.Button
        Me.CurrentWkfStageForeColor = New System.Windows.Forms.Button
        Me.NextWkfStageBackColor = New System.Windows.Forms.Button
        Me.Label10 = New System.Windows.Forms.Label
        Me.CurrentWkfStageBackColor = New System.Windows.Forms.Button
        Me.Label9 = New System.Windows.Forms.Label
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.ExpiredForeColor = New System.Windows.Forms.Button
        Me.SearchResultForeColor = New System.Windows.Forms.Button
        Me.StageChangedForeColor = New System.Windows.Forms.Button
        Me.UrgentForeColor = New System.Windows.Forms.Button
        Me.UnviewedForeColor = New System.Windows.Forms.Button
        Me.ViewedForeColor = New System.Windows.Forms.Button
        Me.Label22 = New System.Windows.Forms.Label
        Me.Label21 = New System.Windows.Forms.Label
        Me.Label20 = New System.Windows.Forms.Label
        Me.Label19 = New System.Windows.Forms.Label
        Me.Label18 = New System.Windows.Forms.Label
        Me.Label17 = New System.Windows.Forms.Label
        Me.ExpiredBackColor = New System.Windows.Forms.Button
        Me.SearchBackColor = New System.Windows.Forms.Button
        Me.StageChangedBackColor = New System.Windows.Forms.Button
        Me.UrgentBackColor = New System.Windows.Forms.Button
        Me.UnviewedBackColor = New System.Windows.Forms.Button
        Me.ViewedBackColor = New System.Windows.Forms.Button
        Me.Label16 = New System.Windows.Forms.Label
        Me.Label15 = New System.Windows.Forms.Label
        Me.Label14 = New System.Windows.Forms.Label
        Me.Label13 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.Label31 = New System.Windows.Forms.Label
        Me.Cdayasback = New System.Windows.Forms.ComboBox
        Me.Label30 = New System.Windows.Forms.Label
        Me.Csnap = New System.Windows.Forms.CheckBox
        Me.Label29 = New System.Windows.Forms.Label
        Me.cfilterheight = New System.Windows.Forms.ComboBox
        Me.Label28 = New System.Windows.Forms.Label
        Me.RestorePollingDefaults = New System.Windows.Forms.Button
        Me.SaveButton = New System.Windows.Forms.Button
        Me.ColorDialog1 = New System.Windows.Forms.ColorDialog
        Me.BCLogoPic = New System.Windows.Forms.PictureBox
        Me.Cancel = New System.Windows.Forms.Button
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Label27 = New System.Windows.Forms.Label
        Me.SettingsTabs.SuspendLayout()
        Me.PollingTab.SuspendLayout()
        Me.PollingSettings.SuspendLayout()
        CType(Me.AlerterSettingHelp, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ScreenSettingHelp, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.UrgentSettingHelp, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.IntervalSettingHelp, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.AlerterSettings.SuspendLayout()
        Me.RefreshTab.SuspendLayout()
        Me.InactivitySettings.SuspendLayout()
        Me.RefreshSettings.SuspendLayout()
        Me.ColoursTab.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        CType(Me.BCLogoPic, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'SettingsTabs
        '
        Me.SettingsTabs.Controls.Add(Me.PollingTab)
        Me.SettingsTabs.Controls.Add(Me.RefreshTab)
        Me.SettingsTabs.Controls.Add(Me.ColoursTab)
        Me.SettingsTabs.Controls.Add(Me.TabPage1)
        Me.SettingsTabs.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed
        Me.SettingsTabs.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SettingsTabs.Location = New System.Drawing.Point(8, 8)
        Me.SettingsTabs.Name = "SettingsTabs"
        Me.SettingsTabs.SelectedIndex = 0
        Me.SettingsTabs.Size = New System.Drawing.Size(400, 285)
        Me.SettingsTabs.TabIndex = 0
        '
        'PollingTab
        '
        Me.PollingTab.BackColor = System.Drawing.Color.White
        Me.PollingTab.Controls.Add(Me.Label25)
        Me.PollingTab.Controls.Add(Me.PollingSettings)
        Me.PollingTab.Controls.Add(Me.PollingEnabled)
        Me.PollingTab.Location = New System.Drawing.Point(4, 22)
        Me.PollingTab.Name = "PollingTab"
        Me.PollingTab.Size = New System.Drawing.Size(392, 259)
        Me.PollingTab.TabIndex = 0
        Me.PollingTab.Text = "Polling"
        Me.PollingTab.UseVisualStyleBackColor = True
        '
        'Label25
        '
        Me.Label25.Location = New System.Drawing.Point(128, 12)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(256, 32)
        Me.Label25.TabIndex = 29
        Me.Label25.Text = "Polling determines "
        '
        'PollingSettings
        '
        Me.PollingSettings.BackColor = System.Drawing.Color.White
        Me.PollingSettings.Controls.Add(Me.InactInterval)
        Me.PollingSettings.Controls.Add(Me.actIntaval)
        Me.PollingSettings.Controls.Add(Me.UrgentDays)
        Me.PollingSettings.Controls.Add(Me.Interval)
        Me.PollingSettings.Controls.Add(Me.AlerterSettingHelp)
        Me.PollingSettings.Controls.Add(Me.ScreenSettingHelp)
        Me.PollingSettings.Controls.Add(Me.UrgentSettingHelp)
        Me.PollingSettings.Controls.Add(Me.IntervalSettingHelp)
        Me.PollingSettings.Controls.Add(Me.AlerterSettings)
        Me.PollingSettings.Controls.Add(Me.Label2)
        Me.PollingSettings.Controls.Add(Me.Label1)
        Me.PollingSettings.Controls.Add(Me.pscreen)
        Me.PollingSettings.Controls.Add(Me.palerter)
        Me.PollingSettings.Controls.Add(Me.pinterval)
        Me.PollingSettings.Location = New System.Drawing.Point(8, 56)
        Me.PollingSettings.Name = "PollingSettings"
        Me.PollingSettings.Size = New System.Drawing.Size(376, 200)
        Me.PollingSettings.TabIndex = 28
        Me.PollingSettings.TabStop = False
        Me.PollingSettings.Text = "Polling settings"
        '
        'InactInterval
        '
        Me.InactInterval.Location = New System.Drawing.Point(216, 93)
        Me.InactInterval.Name = "InactInterval"
        Me.InactInterval.Size = New System.Drawing.Size(65, 21)
        Me.InactInterval.TabIndex = 28
        Me.InactInterval.Text = "ComboBox1"
        '
        'actIntaval
        '
        Me.actIntaval.Location = New System.Drawing.Point(110, 96)
        Me.actIntaval.Name = "actIntaval"
        Me.actIntaval.Size = New System.Drawing.Size(114, 16)
        Me.actIntaval.TabIndex = 42
        Me.actIntaval.Text = "Inactive Interval"
        '
        'UrgentDays
        '
        Me.UrgentDays.Location = New System.Drawing.Point(112, 64)
        Me.UrgentDays.Name = "UrgentDays"
        Me.UrgentDays.Size = New System.Drawing.Size(56, 21)
        Me.UrgentDays.TabIndex = 41
        Me.UrgentDays.Text = "ComboBox1"
        '
        'Interval
        '
        Me.Interval.Location = New System.Drawing.Point(112, 32)
        Me.Interval.Name = "Interval"
        Me.Interval.Size = New System.Drawing.Size(56, 21)
        Me.Interval.TabIndex = 40
        Me.Interval.Text = "ComboBox1"
        '
        'AlerterSettingHelp
        '
        Me.AlerterSettingHelp.Image = CType(resources.GetObject("AlerterSettingHelp.Image"), System.Drawing.Image)
        Me.AlerterSettingHelp.Location = New System.Drawing.Point(8, 128)
        Me.AlerterSettingHelp.Name = "AlerterSettingHelp"
        Me.AlerterSettingHelp.Size = New System.Drawing.Size(16, 16)
        Me.AlerterSettingHelp.TabIndex = 39
        Me.AlerterSettingHelp.TabStop = False
        Me.ToolTip1.SetToolTip(Me.AlerterSettingHelp, "Turns on the alerter so that user can be alerted to new document or document stat" & _
                "e changes")
        '
        'ScreenSettingHelp
        '
        Me.ScreenSettingHelp.Image = CType(resources.GetObject("ScreenSettingHelp.Image"), System.Drawing.Image)
        Me.ScreenSettingHelp.Location = New System.Drawing.Point(8, 96)
        Me.ScreenSettingHelp.Name = "ScreenSettingHelp"
        Me.ScreenSettingHelp.Size = New System.Drawing.Size(16, 16)
        Me.ScreenSettingHelp.TabIndex = 38
        Me.ScreenSettingHelp.TabStop = False
        Me.ToolTip1.SetToolTip(Me.ScreenSettingHelp, "Allow the screen to be automtically updated at the poll interval")
        '
        'UrgentSettingHelp
        '
        Me.UrgentSettingHelp.Image = CType(resources.GetObject("UrgentSettingHelp.Image"), System.Drawing.Image)
        Me.UrgentSettingHelp.Location = New System.Drawing.Point(8, 64)
        Me.UrgentSettingHelp.Name = "UrgentSettingHelp"
        Me.UrgentSettingHelp.Size = New System.Drawing.Size(16, 16)
        Me.UrgentSettingHelp.TabIndex = 37
        Me.UrgentSettingHelp.TabStop = False
        Me.ToolTip1.SetToolTip(Me.UrgentSettingHelp, "Documents with a completed date within this number of days are marked as urgent")
        '
        'IntervalSettingHelp
        '
        Me.IntervalSettingHelp.Image = CType(resources.GetObject("IntervalSettingHelp.Image"), System.Drawing.Image)
        Me.IntervalSettingHelp.Location = New System.Drawing.Point(8, 32)
        Me.IntervalSettingHelp.Name = "IntervalSettingHelp"
        Me.IntervalSettingHelp.Size = New System.Drawing.Size(16, 16)
        Me.IntervalSettingHelp.TabIndex = 36
        Me.IntervalSettingHelp.TabStop = False
        Me.ToolTip1.SetToolTip(Me.IntervalSettingHelp, "The interval at which the server is polled for updates from the client")
        '
        'AlerterSettings
        '
        Me.AlerterSettings.Controls.Add(Me.FadeTime)
        Me.AlerterSettings.Controls.Add(Me.Label4)
        Me.AlerterSettings.Controls.Add(Me.Label3)
        Me.AlerterSettings.Controls.Add(Me.cbeep)
        Me.AlerterSettings.Location = New System.Drawing.Point(120, 128)
        Me.AlerterSettings.Name = "AlerterSettings"
        Me.AlerterSettings.Size = New System.Drawing.Size(240, 64)
        Me.AlerterSettings.TabIndex = 35
        Me.AlerterSettings.TabStop = False
        Me.AlerterSettings.Text = "Alerter settings"
        '
        'FadeTime
        '
        Me.FadeTime.Location = New System.Drawing.Point(96, 40)
        Me.FadeTime.Name = "FadeTime"
        Me.FadeTime.Size = New System.Drawing.Size(48, 21)
        Me.FadeTime.TabIndex = 38
        Me.FadeTime.Text = "ComboBox1"
        Me.ToolTip1.SetToolTip(Me.FadeTime, "Time for alerter to appear on screen")
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(144, 40)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(64, 16)
        Me.Label4.TabIndex = 37
        Me.Label4.Text = "seconds"
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(16, 40)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(72, 16)
        Me.Label3.TabIndex = 35
        Me.Label3.Text = "Fade in for:"
        '
        'cbeep
        '
        Me.cbeep.CausesValidation = False
        Me.cbeep.Location = New System.Drawing.Point(16, 16)
        Me.cbeep.Name = "cbeep"
        Me.cbeep.Size = New System.Drawing.Size(64, 16)
        Me.cbeep.TabIndex = 31
        Me.cbeep.Text = "Beep"
        Me.ToolTip1.SetToolTip(Me.cbeep, "Maket the Alerter Beep")
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(168, 64)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(104, 16)
        Me.Label2.TabIndex = 29
        Me.Label2.Text = "days of expiry"
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(32, 64)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(88, 16)
        Me.Label1.TabIndex = 28
        Me.Label1.Text = "Urgent within"
        '
        'pscreen
        '
        Me.pscreen.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.pscreen.Location = New System.Drawing.Point(32, 96)
        Me.pscreen.Name = "pscreen"
        Me.pscreen.Size = New System.Drawing.Size(72, 16)
        Me.pscreen.TabIndex = 27
        Me.pscreen.Text = "Screen"
        '
        'palerter
        '
        Me.palerter.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.palerter.Location = New System.Drawing.Point(32, 128)
        Me.palerter.Name = "palerter"
        Me.palerter.Size = New System.Drawing.Size(72, 16)
        Me.palerter.TabIndex = 26
        Me.palerter.Text = "Alerter"
        '
        'pinterval
        '
        Me.pinterval.Location = New System.Drawing.Point(32, 32)
        Me.pinterval.Name = "pinterval"
        Me.pinterval.Size = New System.Drawing.Size(50, 16)
        Me.pinterval.TabIndex = 18
        Me.pinterval.Text = "Interval"
        '
        'PollingEnabled
        '
        Me.PollingEnabled.Location = New System.Drawing.Point(8, 8)
        Me.PollingEnabled.Name = "PollingEnabled"
        Me.PollingEnabled.Size = New System.Drawing.Size(120, 24)
        Me.PollingEnabled.TabIndex = 27
        Me.PollingEnabled.Text = "Polling Enabled -"
        '
        'RefreshTab
        '
        Me.RefreshTab.BackColor = System.Drawing.Color.White
        Me.RefreshTab.Controls.Add(Me.InactivitySettings)
        Me.RefreshTab.Controls.Add(Me.Label26)
        Me.RefreshTab.Controls.Add(Me.RefreshSettings)
        Me.RefreshTab.Controls.Add(Me.AutoRefresh)
        Me.RefreshTab.Location = New System.Drawing.Point(4, 22)
        Me.RefreshTab.Name = "RefreshTab"
        Me.RefreshTab.Size = New System.Drawing.Size(392, 259)
        Me.RefreshTab.TabIndex = 1
        Me.RefreshTab.Text = "Refresh"
        Me.RefreshTab.UseVisualStyleBackColor = True
        '
        'InactivitySettings
        '
        Me.InactivitySettings.BackColor = System.Drawing.Color.WhiteSmoke
        Me.InactivitySettings.Controls.Add(Me.Label8)
        Me.InactivitySettings.Controls.Add(Me.RefreshAfterActivityTime)
        Me.InactivitySettings.Controls.Add(Me.Label7)
        Me.InactivitySettings.Location = New System.Drawing.Point(8, 144)
        Me.InactivitySettings.Name = "InactivitySettings"
        Me.InactivitySettings.Size = New System.Drawing.Size(376, 96)
        Me.InactivitySettings.TabIndex = 10
        Me.InactivitySettings.TabStop = False
        Me.InactivitySettings.Visible = False
        '
        'Label8
        '
        Me.Label8.Location = New System.Drawing.Point(72, 49)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(176, 16)
        Me.Label8.TabIndex = 15
        Me.Label8.Text = "seconds of screen inactivity"
        '
        'RefreshAfterActivityTime
        '
        Me.RefreshAfterActivityTime.BackColor = System.Drawing.Color.WhiteSmoke
        Me.RefreshAfterActivityTime.Location = New System.Drawing.Point(16, 41)
        Me.RefreshAfterActivityTime.Name = "RefreshAfterActivityTime"
        Me.RefreshAfterActivityTime.Size = New System.Drawing.Size(48, 30)
        Me.RefreshAfterActivityTime.TabIndex = 14
        '
        'Label7
        '
        Me.Label7.Location = New System.Drawing.Point(16, 24)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(232, 16)
        Me.Label7.TabIndex = 13
        Me.Label7.Text = "Refresh and Turn on Screen Poll after"
        '
        'Label26
        '
        Me.Label26.Location = New System.Drawing.Point(112, 12)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(272, 24)
        Me.Label26.TabIndex = 9
        Me.Label26.Text = "Auto refresh sets ..."
        '
        'RefreshSettings
        '
        Me.RefreshSettings.Controls.Add(Me.MinRefreshTime)
        Me.RefreshSettings.Controls.Add(Me.Label6)
        Me.RefreshSettings.Controls.Add(Me.Label5)
        Me.RefreshSettings.Location = New System.Drawing.Point(8, 48)
        Me.RefreshSettings.Name = "RefreshSettings"
        Me.RefreshSettings.Size = New System.Drawing.Size(376, 88)
        Me.RefreshSettings.TabIndex = 8
        Me.RefreshSettings.TabStop = False
        Me.RefreshSettings.Text = "Auto Refresh settings"
        '
        'MinRefreshTime
        '
        Me.MinRefreshTime.Location = New System.Drawing.Point(16, 45)
        Me.MinRefreshTime.Name = "MinRefreshTime"
        Me.MinRefreshTime.Size = New System.Drawing.Size(48, 30)
        Me.MinRefreshTime.TabIndex = 9
        Me.ToolTip1.SetToolTip(Me.MinRefreshTime, "Interval when screen will be automtically refreshed when the cursor enters it.")
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(72, 53)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(184, 16)
        Me.Label6.TabIndex = 8
        Me.Label6.Text = "seconds of previous refresh"
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(16, 24)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(152, 16)
        Me.Label5.TabIndex = 7
        Me.Label5.Text = "Do not Refresh within"
        '
        'AutoRefresh
        '
        Me.AutoRefresh.Checked = True
        Me.AutoRefresh.CheckState = System.Windows.Forms.CheckState.Checked
        Me.AutoRefresh.Location = New System.Drawing.Point(8, 8)
        Me.AutoRefresh.Name = "AutoRefresh"
        Me.AutoRefresh.Size = New System.Drawing.Size(112, 24)
        Me.AutoRefresh.TabIndex = 0
        Me.AutoRefresh.Text = "Auto Refresh -"
        Me.ToolTip1.SetToolTip(Me.AutoRefresh, "Refreshs Document list automtically on cursor entry")
        '
        'ColoursTab
        '
        Me.ColoursTab.BackColor = System.Drawing.Color.White
        Me.ColoursTab.Controls.Add(Me.GroupBox2)
        Me.ColoursTab.Controls.Add(Me.GroupBox1)
        Me.ColoursTab.Location = New System.Drawing.Point(4, 22)
        Me.ColoursTab.Name = "ColoursTab"
        Me.ColoursTab.Size = New System.Drawing.Size(392, 259)
        Me.ColoursTab.TabIndex = 2
        Me.ColoursTab.Text = "Colours"
        Me.ColoursTab.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Label24)
        Me.GroupBox2.Controls.Add(Me.Label23)
        Me.GroupBox2.Controls.Add(Me.NextWkfStageForeColor)
        Me.GroupBox2.Controls.Add(Me.CurrentWkfStageForeColor)
        Me.GroupBox2.Controls.Add(Me.NextWkfStageBackColor)
        Me.GroupBox2.Controls.Add(Me.Label10)
        Me.GroupBox2.Controls.Add(Me.CurrentWkfStageBackColor)
        Me.GroupBox2.Controls.Add(Me.Label9)
        Me.GroupBox2.Location = New System.Drawing.Point(8, 192)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(376, 64)
        Me.GroupBox2.TabIndex = 1
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Workflow"
        '
        'Label24
        '
        Me.Label24.Location = New System.Drawing.Point(200, 16)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(128, 16)
        Me.Label24.TabIndex = 23
        Me.Label24.Text = "Current stage text:"
        '
        'Label23
        '
        Me.Label23.Location = New System.Drawing.Point(200, 40)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(136, 16)
        Me.Label23.TabIndex = 22
        Me.Label23.Text = "Available stages text:"
        '
        'NextWkfStageForeColor
        '
        Me.NextWkfStageForeColor.BackColor = System.Drawing.Color.Black
        Me.NextWkfStageForeColor.Location = New System.Drawing.Point(336, 40)
        Me.NextWkfStageForeColor.Name = "NextWkfStageForeColor"
        Me.NextWkfStageForeColor.Size = New System.Drawing.Size(24, 16)
        Me.NextWkfStageForeColor.TabIndex = 21
        Me.NextWkfStageForeColor.UseVisualStyleBackColor = False
        '
        'CurrentWkfStageForeColor
        '
        Me.CurrentWkfStageForeColor.BackColor = System.Drawing.Color.White
        Me.CurrentWkfStageForeColor.Location = New System.Drawing.Point(336, 16)
        Me.CurrentWkfStageForeColor.Name = "CurrentWkfStageForeColor"
        Me.CurrentWkfStageForeColor.Size = New System.Drawing.Size(24, 16)
        Me.CurrentWkfStageForeColor.TabIndex = 20
        Me.CurrentWkfStageForeColor.UseVisualStyleBackColor = False
        '
        'NextWkfStageBackColor
        '
        Me.NextWkfStageBackColor.BackColor = System.Drawing.Color.FromArgb(CType(CType(207, Byte), Integer), CType(CType(229, Byte), Integer), CType(CType(240, Byte), Integer))
        Me.NextWkfStageBackColor.Location = New System.Drawing.Point(160, 40)
        Me.NextWkfStageBackColor.Name = "NextWkfStageBackColor"
        Me.NextWkfStageBackColor.Size = New System.Drawing.Size(24, 16)
        Me.NextWkfStageBackColor.TabIndex = 3
        Me.NextWkfStageBackColor.UseVisualStyleBackColor = False
        '
        'Label10
        '
        Me.Label10.Location = New System.Drawing.Point(8, 40)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(136, 16)
        Me.Label10.TabIndex = 2
        Me.Label10.Text = "Available stages back:"
        '
        'CurrentWkfStageBackColor
        '
        Me.CurrentWkfStageBackColor.BackColor = System.Drawing.Color.LightSteelBlue
        Me.CurrentWkfStageBackColor.Location = New System.Drawing.Point(160, 16)
        Me.CurrentWkfStageBackColor.Name = "CurrentWkfStageBackColor"
        Me.CurrentWkfStageBackColor.Size = New System.Drawing.Size(24, 16)
        Me.CurrentWkfStageBackColor.TabIndex = 1
        Me.CurrentWkfStageBackColor.UseVisualStyleBackColor = False
        '
        'Label9
        '
        Me.Label9.Location = New System.Drawing.Point(8, 16)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(128, 16)
        Me.Label9.TabIndex = 0
        Me.Label9.Text = "Current stage back:"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.ExpiredForeColor)
        Me.GroupBox1.Controls.Add(Me.SearchResultForeColor)
        Me.GroupBox1.Controls.Add(Me.StageChangedForeColor)
        Me.GroupBox1.Controls.Add(Me.UrgentForeColor)
        Me.GroupBox1.Controls.Add(Me.UnviewedForeColor)
        Me.GroupBox1.Controls.Add(Me.ViewedForeColor)
        Me.GroupBox1.Controls.Add(Me.Label22)
        Me.GroupBox1.Controls.Add(Me.Label21)
        Me.GroupBox1.Controls.Add(Me.Label20)
        Me.GroupBox1.Controls.Add(Me.Label19)
        Me.GroupBox1.Controls.Add(Me.Label18)
        Me.GroupBox1.Controls.Add(Me.Label17)
        Me.GroupBox1.Controls.Add(Me.ExpiredBackColor)
        Me.GroupBox1.Controls.Add(Me.SearchBackColor)
        Me.GroupBox1.Controls.Add(Me.StageChangedBackColor)
        Me.GroupBox1.Controls.Add(Me.UrgentBackColor)
        Me.GroupBox1.Controls.Add(Me.UnviewedBackColor)
        Me.GroupBox1.Controls.Add(Me.ViewedBackColor)
        Me.GroupBox1.Controls.Add(Me.Label16)
        Me.GroupBox1.Controls.Add(Me.Label15)
        Me.GroupBox1.Controls.Add(Me.Label14)
        Me.GroupBox1.Controls.Add(Me.Label13)
        Me.GroupBox1.Controls.Add(Me.Label12)
        Me.GroupBox1.Controls.Add(Me.Label11)
        Me.GroupBox1.Location = New System.Drawing.Point(8, 8)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(376, 168)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Document list"
        '
        'ExpiredForeColor
        '
        Me.ExpiredForeColor.BackColor = System.Drawing.Color.Black
        Me.ExpiredForeColor.Location = New System.Drawing.Point(336, 136)
        Me.ExpiredForeColor.Name = "ExpiredForeColor"
        Me.ExpiredForeColor.Size = New System.Drawing.Size(24, 16)
        Me.ExpiredForeColor.TabIndex = 24
        Me.ExpiredForeColor.UseVisualStyleBackColor = False
        '
        'SearchResultForeColor
        '
        Me.SearchResultForeColor.BackColor = System.Drawing.Color.Black
        Me.SearchResultForeColor.Location = New System.Drawing.Point(336, 112)
        Me.SearchResultForeColor.Name = "SearchResultForeColor"
        Me.SearchResultForeColor.Size = New System.Drawing.Size(24, 16)
        Me.SearchResultForeColor.TabIndex = 23
        Me.SearchResultForeColor.UseVisualStyleBackColor = False
        '
        'StageChangedForeColor
        '
        Me.StageChangedForeColor.BackColor = System.Drawing.Color.White
        Me.StageChangedForeColor.Location = New System.Drawing.Point(336, 88)
        Me.StageChangedForeColor.Name = "StageChangedForeColor"
        Me.StageChangedForeColor.Size = New System.Drawing.Size(24, 16)
        Me.StageChangedForeColor.TabIndex = 22
        Me.StageChangedForeColor.UseVisualStyleBackColor = False
        '
        'UrgentForeColor
        '
        Me.UrgentForeColor.BackColor = System.Drawing.Color.White
        Me.UrgentForeColor.Location = New System.Drawing.Point(336, 64)
        Me.UrgentForeColor.Name = "UrgentForeColor"
        Me.UrgentForeColor.Size = New System.Drawing.Size(24, 16)
        Me.UrgentForeColor.TabIndex = 21
        Me.UrgentForeColor.UseVisualStyleBackColor = False
        '
        'UnviewedForeColor
        '
        Me.UnviewedForeColor.BackColor = System.Drawing.Color.Black
        Me.UnviewedForeColor.Location = New System.Drawing.Point(336, 40)
        Me.UnviewedForeColor.Name = "UnviewedForeColor"
        Me.UnviewedForeColor.Size = New System.Drawing.Size(24, 16)
        Me.UnviewedForeColor.TabIndex = 20
        Me.UnviewedForeColor.UseVisualStyleBackColor = False
        '
        'ViewedForeColor
        '
        Me.ViewedForeColor.BackColor = System.Drawing.Color.Black
        Me.ViewedForeColor.Location = New System.Drawing.Point(336, 16)
        Me.ViewedForeColor.Name = "ViewedForeColor"
        Me.ViewedForeColor.Size = New System.Drawing.Size(24, 16)
        Me.ViewedForeColor.TabIndex = 19
        Me.ViewedForeColor.UseVisualStyleBackColor = False
        '
        'Label22
        '
        Me.Label22.Location = New System.Drawing.Point(200, 136)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(112, 16)
        Me.Label22.TabIndex = 18
        Me.Label22.Text = "Expired text:"
        '
        'Label21
        '
        Me.Label21.Location = New System.Drawing.Point(200, 112)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(112, 16)
        Me.Label21.TabIndex = 17
        Me.Label21.Text = "Search result text:"
        '
        'Label20
        '
        Me.Label20.Location = New System.Drawing.Point(200, 64)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(120, 16)
        Me.Label20.TabIndex = 16
        Me.Label20.Text = "Urgent text:"
        '
        'Label19
        '
        Me.Label19.Location = New System.Drawing.Point(200, 40)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(112, 16)
        Me.Label19.TabIndex = 15
        Me.Label19.Text = "Unviewed text:"
        '
        'Label18
        '
        Me.Label18.Location = New System.Drawing.Point(200, 16)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(112, 16)
        Me.Label18.TabIndex = 14
        Me.Label18.Text = "Selected text:"
        '
        'Label17
        '
        Me.Label17.Location = New System.Drawing.Point(200, 88)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(152, 16)
        Me.Label17.TabIndex = 13
        Me.Label17.Text = "Stage changed text:"
        '
        'ExpiredBackColor
        '
        Me.ExpiredBackColor.BackColor = System.Drawing.Color.PowderBlue
        Me.ExpiredBackColor.Location = New System.Drawing.Point(160, 136)
        Me.ExpiredBackColor.Name = "ExpiredBackColor"
        Me.ExpiredBackColor.Size = New System.Drawing.Size(24, 16)
        Me.ExpiredBackColor.TabIndex = 12
        Me.ExpiredBackColor.UseVisualStyleBackColor = False
        '
        'SearchBackColor
        '
        Me.SearchBackColor.BackColor = System.Drawing.Color.Thistle
        Me.SearchBackColor.Location = New System.Drawing.Point(160, 112)
        Me.SearchBackColor.Name = "SearchBackColor"
        Me.SearchBackColor.Size = New System.Drawing.Size(24, 16)
        Me.SearchBackColor.TabIndex = 11
        Me.SearchBackColor.UseVisualStyleBackColor = False
        '
        'StageChangedBackColor
        '
        Me.StageChangedBackColor.BackColor = System.Drawing.Color.SteelBlue
        Me.StageChangedBackColor.Location = New System.Drawing.Point(160, 88)
        Me.StageChangedBackColor.Name = "StageChangedBackColor"
        Me.StageChangedBackColor.Size = New System.Drawing.Size(24, 16)
        Me.StageChangedBackColor.TabIndex = 10
        Me.StageChangedBackColor.UseVisualStyleBackColor = False
        '
        'UrgentBackColor
        '
        Me.UrgentBackColor.BackColor = System.Drawing.Color.Blue
        Me.UrgentBackColor.Location = New System.Drawing.Point(160, 64)
        Me.UrgentBackColor.Name = "UrgentBackColor"
        Me.UrgentBackColor.Size = New System.Drawing.Size(24, 16)
        Me.UrgentBackColor.TabIndex = 9
        Me.UrgentBackColor.UseVisualStyleBackColor = False
        '
        'UnviewedBackColor
        '
        Me.UnviewedBackColor.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.UnviewedBackColor.Location = New System.Drawing.Point(160, 40)
        Me.UnviewedBackColor.Name = "UnviewedBackColor"
        Me.UnviewedBackColor.Size = New System.Drawing.Size(24, 16)
        Me.UnviewedBackColor.TabIndex = 8
        Me.UnviewedBackColor.UseVisualStyleBackColor = False
        '
        'ViewedBackColor
        '
        Me.ViewedBackColor.BackColor = System.Drawing.Color.White
        Me.ViewedBackColor.Location = New System.Drawing.Point(160, 16)
        Me.ViewedBackColor.Name = "ViewedBackColor"
        Me.ViewedBackColor.Size = New System.Drawing.Size(24, 16)
        Me.ViewedBackColor.TabIndex = 7
        Me.ToolTip1.SetToolTip(Me.ViewedBackColor, "Click here to change the background colour of documents that have previously been" & _
                " viewed in Process")
        Me.ViewedBackColor.UseVisualStyleBackColor = False
        '
        'Label16
        '
        Me.Label16.Location = New System.Drawing.Point(16, 88)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(152, 16)
        Me.Label16.TabIndex = 6
        Me.Label16.Text = "Stage changed back:"
        '
        'Label15
        '
        Me.Label15.Location = New System.Drawing.Point(16, 136)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(112, 16)
        Me.Label15.TabIndex = 5
        Me.Label15.Text = "Expired back:"
        '
        'Label14
        '
        Me.Label14.Location = New System.Drawing.Point(16, 64)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(120, 16)
        Me.Label14.TabIndex = 4
        Me.Label14.Text = "Urgent back:"
        '
        'Label13
        '
        Me.Label13.Location = New System.Drawing.Point(16, 112)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(112, 16)
        Me.Label13.TabIndex = 2
        Me.Label13.Text = "Search result back:"
        '
        'Label12
        '
        Me.Label12.Location = New System.Drawing.Point(16, 40)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(112, 16)
        Me.Label12.TabIndex = 1
        Me.Label12.Text = "Unviewed back:"
        '
        'Label11
        '
        Me.Label11.Location = New System.Drawing.Point(16, 16)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(112, 16)
        Me.Label11.TabIndex = 0
        Me.Label11.Text = "Selected back:"
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.Label31)
        Me.TabPage1.Controls.Add(Me.Cdayasback)
        Me.TabPage1.Controls.Add(Me.Label30)
        Me.TabPage1.Controls.Add(Me.Csnap)
        Me.TabPage1.Controls.Add(Me.Label29)
        Me.TabPage1.Controls.Add(Me.cfilterheight)
        Me.TabPage1.Controls.Add(Me.Label28)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(392, 259)
        Me.TabPage1.TabIndex = 3
        Me.TabPage1.Text = "Filters"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'Label31
        '
        Me.Label31.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label31.Location = New System.Drawing.Point(246, 105)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(174, 17)
        Me.Label31.TabIndex = 14
        Me.Label31.Text = "Days Back"
        '
        'Cdayasback
        '
        Me.Cdayasback.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Cdayasback.FormattingEnabled = True
        Me.Cdayasback.Location = New System.Drawing.Point(164, 105)
        Me.Cdayasback.Name = "Cdayasback"
        Me.Cdayasback.Size = New System.Drawing.Size(64, 21)
        Me.Cdayasback.TabIndex = 13
        '
        'Label30
        '
        Me.Label30.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label30.Location = New System.Drawing.Point(6, 105)
        Me.Label30.Name = "Label30"
        Me.Label30.Size = New System.Drawing.Size(174, 17)
        Me.Label30.TabIndex = 12
        Me.Label30.Text = "Default Date from "
        '
        'Csnap
        '
        Me.Csnap.AutoSize = True
        Me.Csnap.Location = New System.Drawing.Point(164, 55)
        Me.Csnap.Name = "Csnap"
        Me.Csnap.Size = New System.Drawing.Size(15, 14)
        Me.Csnap.TabIndex = 11
        Me.Csnap.UseVisualStyleBackColor = True
        '
        'Label29
        '
        Me.Label29.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label29.Location = New System.Drawing.Point(6, 55)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(152, 16)
        Me.Label29.TabIndex = 10
        Me.Label29.Text = "Snap Summary Panel"
        '
        'cfilterheight
        '
        Me.cfilterheight.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cfilterheight.FormattingEnabled = True
        Me.cfilterheight.Location = New System.Drawing.Point(164, 12)
        Me.cfilterheight.Name = "cfilterheight"
        Me.cfilterheight.Size = New System.Drawing.Size(64, 21)
        Me.cfilterheight.TabIndex = 9
        '
        'Label28
        '
        Me.Label28.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label28.Location = New System.Drawing.Point(6, 15)
        Me.Label28.Name = "Label28"
        Me.Label28.Size = New System.Drawing.Size(152, 16)
        Me.Label28.TabIndex = 8
        Me.Label28.Text = "Height of Filters Panel"
        '
        'RestorePollingDefaults
        '
        Me.RestorePollingDefaults.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.RestorePollingDefaults.Location = New System.Drawing.Point(296, 299)
        Me.RestorePollingDefaults.Name = "RestorePollingDefaults"
        Me.RestorePollingDefaults.Size = New System.Drawing.Size(112, 32)
        Me.RestorePollingDefaults.TabIndex = 26
        Me.RestorePollingDefaults.Text = "Restore Defaults"
        Me.ToolTip1.SetToolTip(Me.RestorePollingDefaults, "Restore the default settings")
        Me.RestorePollingDefaults.UseVisualStyleBackColor = False
        '
        'SaveButton
        '
        Me.SaveButton.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.SaveButton.Location = New System.Drawing.Point(336, 339)
        Me.SaveButton.Name = "SaveButton"
        Me.SaveButton.Size = New System.Drawing.Size(72, 32)
        Me.SaveButton.TabIndex = 1
        Me.SaveButton.Text = "Save"
        Me.SaveButton.UseVisualStyleBackColor = False
        '
        'BCLogoPic
        '
        Me.BCLogoPic.Image = CType(resources.GetObject("BCLogoPic.Image"), System.Drawing.Image)
        Me.BCLogoPic.Location = New System.Drawing.Point(8, 299)
        Me.BCLogoPic.Name = "BCLogoPic"
        Me.BCLogoPic.Size = New System.Drawing.Size(184, 32)
        Me.BCLogoPic.TabIndex = 25
        Me.BCLogoPic.TabStop = False
        '
        'Cancel
        '
        Me.Cancel.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.Cancel.Location = New System.Drawing.Point(256, 339)
        Me.Cancel.Name = "Cancel"
        Me.Cancel.Size = New System.Drawing.Size(72, 32)
        Me.Cancel.TabIndex = 26
        Me.Cancel.Text = "Cancel"
        Me.Cancel.UseVisualStyleBackColor = False
        '
        'Label27
        '
        Me.Label27.Location = New System.Drawing.Point(5, 349)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(296, 24)
        Me.Label27.TabIndex = 27
        Me.Label27.Text = "© Blue Curve Limited. All rights reserved."
        '
        'bc_am_workflow_settings
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(418, 373)
        Me.ControlBox = False
        Me.Controls.Add(Me.Cancel)
        Me.Controls.Add(Me.BCLogoPic)
        Me.Controls.Add(Me.SaveButton)
        Me.Controls.Add(Me.RestorePollingDefaults)
        Me.Controls.Add(Me.SettingsTabs)
        Me.Controls.Add(Me.Label27)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "bc_am_workflow_settings"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Blue Curve - Process Settings"
        Me.SettingsTabs.ResumeLayout(False)
        Me.PollingTab.ResumeLayout(False)
        Me.PollingSettings.ResumeLayout(False)
        CType(Me.AlerterSettingHelp, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ScreenSettingHelp, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.UrgentSettingHelp, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.IntervalSettingHelp, System.ComponentModel.ISupportInitialize).EndInit()
        Me.AlerterSettings.ResumeLayout(False)
        Me.RefreshTab.ResumeLayout(False)
        Me.InactivitySettings.ResumeLayout(False)
        Me.RefreshSettings.ResumeLayout(False)
        Me.ColoursTab.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        CType(Me.BCLogoPic, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region
    Public first As Boolean

    'Update the shared bc_am_workflow with the new settings
    'However, they will not be persisted until the Process application is changed
    Private Sub SaveButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveButton.Click
        Dim owf As New bc_am_workflow

        'Polling settings
        bc_am_workflow.polling_enabled = Me.PollingEnabled.Checked
        If bc_am_workflow.polling_enabled Then
            If Me.pscreen.Checked = True Then
                bc_am_workflow.screen_poll = True
                bc_am_workflow.screen_update_enabled = True
            Else
                bc_am_workflow.screen_poll = False
            End If

            'Convert value to milleseconds
            bc_am_workflow.polling_interval = CLng(Me.Interval.SelectedItem) * 1000
            bc_am_workflow.pre_expire_alert_notify = CInt(UrgentDays.SelectedItem)
            bc_am_workflow.user_inactive_interval = CLng(Me.InactInterval.SelectedItem)

            If Me.palerter.Checked Then
                bc_am_workflow.alerter_enabled = True
                If Me.cbeep.Checked = True Then
                    bc_am_workflow.beep_enabled = True
                Else
                    bc_am_workflow.beep_enabled = False
                End If

                bc_am_workflow.fade_interval = CInt(FadeTime.SelectedItem)
            Else
                bc_am_workflow.alerter_enabled = False
            End If

        End If

        'Refresh settings
        bc_am_workflow.auto_refresh = Me.AutoRefresh.Checked
        If bc_am_workflow.auto_refresh Then
            bc_am_workflow.screen_refresh_interval = CInt(Me.MinRefreshTime.SelectedItem)
            bc_am_workflow.screen_inactive_interval = CInt(Me.RefreshAfterActivityTime.SelectedItem)
        End If

        'update status at bottom of main screen
        owf.set_polling_status()

        'Colors
        Dim newColors As New ProcessColors
        With newColors
            .doc_list_expired_backcolor = ExpiredBackColor.BackColor
            .doc_list_expired_forecolor = ExpiredForeColor.BackColor
            .doc_list_read_backcolor = ViewedBackColor.BackColor
            .doc_list_read_forecolor = ViewedForeColor.BackColor
            .doc_list_search_backcolor = SearchBackColor.BackColor
            .doc_list_search_forecolor = SearchResultForeColor.BackColor
            .doc_list_stage_changed_backcolor = StageChangedBackColor.BackColor
            .doc_list_stage_changed_forecolor = StageChangedForeColor.BackColor
            .doc_list_unread_backcolor = UnviewedBackColor.BackColor
            .doc_list_unread_forecolor = UnviewedForeColor.BackColor
            .doc_list_urgent_backcolor = UrgentBackColor.BackColor
            .doc_list_urgent_forecolor = UrgentForeColor.BackColor
            .workflow_current_stage_backcolor = CurrentWkfStageBackColor.BackColor
            .workflow_current_stage_forecolor = CurrentWkfStageForeColor.BackColor
            .workflow_next_stage_backcolor = NextWkfStageBackColor.BackColor
            .workflow_next_stage_forecolor = NextWkfStageForeColor.BackColor
        End With
        owf.setColors(newColors)
        bc_am_workflow.cfrm.FilterPanel.Height = CInt(Me.cfilterheight.Text)
        bc_am_workflow.filterpanelheight = CDbl(Me.cfilterheight.Text)
        bc_am_workflow.snapsummary = Me.Csnap.Checked

        bc_am_workflow.write_user_settings_fo_file(True)



        Me.Close()
    End Sub

    Private Sub bc_am_workflow_settings_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'Load Polling tab's controls
        Me.PollingEnabled.Checked = bc_am_workflow.polling_enabled
        PollingEnabled_CheckedChanged(Me.PollingEnabled, New EventArgs)

        Me.pscreen.Checked = bc_am_workflow.screen_poll
        Me.palerter.Checked = bc_am_workflow.alerter_enabled
        palerter_CheckedChanged(Me.palerter, New EventArgs)



        Me.cbeep.Checked = bc_am_workflow.beep_enabled

        Dim i As Integer
        Me.Interval.Items.Clear()
        Interval.Items.Add(5)
        Interval.Items.Add(10)
        Interval.Items.Add(15)
        Interval.Items.Add(20)
        Interval.Items.Add(30)
        Interval.Items.Add(60)
        Interval.Items.Add(300)
        For i = 0 To Interval.Items.Count - 1
            'Value is in milleseconds so convert to seconds
            If CStr(Interval.Items(i)) = CStr(bc_am_workflow.polling_interval / 1000) Then
                Me.Interval.SelectedIndex = i
                Exit For
            End If
        Next

        Me.InactInterval.Items.Clear()
        InactInterval.Items.Add(1)
        InactInterval.Items.Add(5)
        InactInterval.Items.Add(10)
        InactInterval.Items.Add(30)
        InactInterval.Items.Add(60)
        For i = 0 To InactInterval.Items.Count - 1
            If CStr(InactInterval.Items(i)) = CStr(bc_am_workflow.user_inactive_interval) Then
                Me.InactInterval.SelectedIndex = i
                Exit For
            End If
        Next

        Me.UrgentDays.Items.Clear()

        For i = 1 To 30
            Me.UrgentDays.Items.Add(CStr(i))
            If i = bc_am_workflow.pre_expire_alert_notify Then
                Me.UrgentDays.SelectedIndex = i - 1
                Exit For
            End If
        Next

        Me.FadeTime.Items.Clear()
        Me.FadeTime.Items.Add(5)
        Me.FadeTime.Items.Add(10)
        Me.FadeTime.Items.Add(15)
        Me.FadeTime.Items.Add(20)
        Me.FadeTime.Items.Add(30)
        Me.FadeTime.Items.Add(60)
        For i = 0 To FadeTime.Items.Count - 1
            If FadeTime.Items(i) = bc_am_workflow.fade_interval Then
                Me.FadeTime.SelectedIndex = i
                Exit For
            End If
        Next


        'Setup the Refresh tab's controls
        Me.AutoRefresh.Checked = bc_am_workflow.auto_refresh
        AutoRefresh_CheckedChanged(Me.AutoRefresh, New EventArgs)

        Me.MinRefreshTime.Items.Clear()
        Me.MinRefreshTime.Items.Add("1")
        Me.MinRefreshTime.Items.Add("5")
        Me.MinRefreshTime.Items.Add("10")
        Me.MinRefreshTime.Items.Add("20")
        Me.MinRefreshTime.Items.Add("30")
        Me.RefreshAfterActivityTime.Items.Clear()
        Me.RefreshAfterActivityTime.Items.Add("10")
        Me.RefreshAfterActivityTime.Items.Add("30")
        Me.RefreshAfterActivityTime.Items.Add("60")
        Me.RefreshAfterActivityTime.Items.Add("300")
        Me.RefreshAfterActivityTime.Items.Add("1800")


        For i = 0 To 4
            If CStr(bc_am_workflow.screen_refresh_interval) = Me.MinRefreshTime.Items(i) Then
                Me.MinRefreshTime.SelectedIndex = i
                Exit For
            End If
        Next
        For i = 0 To 4
            If CStr(bc_am_workflow.screen_inactive_interval) = Me.RefreshAfterActivityTime.Items(i) Then
                Me.RefreshAfterActivityTime.SelectedIndex = i
                Exit For
            End If
        Next

        'Load the Colors tab's controls
        ExpiredBackColor.BackColor = bc_am_workflow.colors.doc_list_expired_backcolor
        ExpiredForeColor.BackColor = bc_am_workflow.colors.doc_list_expired_forecolor
        ViewedBackColor.BackColor = bc_am_workflow.colors.doc_list_read_backcolor
        ViewedForeColor.BackColor = bc_am_workflow.colors.doc_list_read_forecolor
        SearchBackColor.BackColor = bc_am_workflow.colors.doc_list_search_backcolor
        SearchResultForeColor.BackColor = bc_am_workflow.colors.doc_list_search_forecolor
        StageChangedBackColor.BackColor = bc_am_workflow.colors.doc_list_stage_changed_backcolor
        StageChangedForeColor.BackColor = bc_am_workflow.colors.doc_list_stage_changed_forecolor
        UnviewedBackColor.BackColor = bc_am_workflow.colors.doc_list_unread_backcolor
        UnviewedForeColor.BackColor = bc_am_workflow.colors.doc_list_unread_forecolor
        UrgentBackColor.BackColor = bc_am_workflow.colors.doc_list_urgent_backcolor
        UrgentForeColor.BackColor = bc_am_workflow.colors.doc_list_urgent_forecolor
        CurrentWkfStageBackColor.BackColor = bc_am_workflow.colors.workflow_current_stage_backcolor
        CurrentWkfStageForeColor.BackColor = bc_am_workflow.colors.workflow_current_stage_forecolor
        NextWkfStageBackColor.BackColor = bc_am_workflow.colors.workflow_next_stage_backcolor
        NextWkfStageForeColor.BackColor = bc_am_workflow.colors.workflow_next_stage_forecolor

        REM if service poller working override local poller
        If System.Diagnostics.Process.GetProcessesByName("BlueCurveService").GetLength(0) > 0 Then
            If bc_cs_central_settings.service_poll_enabled = 1 Then
                Me.PollingEnabled.Checked = True
                Me.palerter.Checked = True

            Else
                Me.PollingEnabled.Checked = False
                Me.palerter.Checked = False
            End If
            Me.PollingEnabled.Enabled = False
            Me.palerter.Enabled = False
            Me.FadeTime.Enabled = False
            Me.Interval.Enabled = False
            Me.Interval.Text = bc_cs_central_settings.poll_interval
            Me.FadeTime.Text = bc_cs_central_settings.alert_interval
        End If
        For i = 138 To 600
            Me.cfilterheight.Items.Add(CStr(i))
        Next
        Me.cfilterheight.Text = CStr(bc_am_workflow.filterpanelheight)
        Me.Cdayasback.Items.Clear()
        For i = 1 To 28
            Me.Cdayasback.Items.Add(CStr(i))

        Next
        Me.Cdayasback.SelectedIndex = bc_am_workflow.days_back - 1
        Me.Csnap.Checked = bc_am_workflow.snapsummary
    End Sub
    Private Sub PollingEnabled_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PollingEnabled.CheckedChanged
        If bc_am_workflow.ofrm.bload = True Then
            'CAB - why?
            Exit Sub
        End If
        If PollingEnabled.Checked = True Then
            REM bc_am_workflow.polling_enabled = "TRUE"
            Me.PollingSettings.Enabled = True
            Me.PollingSettings.BackColor = System.Drawing.Color.White
            If Me.palerter.Checked Then
                Me.AlerterSettings.BackColor = System.Drawing.Color.White
            Else
                Me.AlerterSettings.BackColor = System.Drawing.Color.WhiteSmoke
            End If
        Else
            Me.pscreen.Checked = False
            Me.palerter.Checked = False
            REM bc_am_workflow.polling_enabled = "FALSE"
            Me.PollingSettings.Enabled = False
            Me.PollingSettings.BackColor = System.Drawing.Color.WhiteSmoke
            Me.AlerterSettings.BackColor = System.Drawing.Color.WhiteSmoke
        End If
    End Sub

    'Deprecated method 
    Private Sub S_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim i As Integer
        bc_am_workflow.ofrm.bload = True
        bc_am_workflow.screen_update_enabled = False

        Me.palerter.Checked = False
        Me.pscreen.Checked = False
        If UCase(bc_am_workflow.alerter_enabled) = "TRUE" Then
            Me.palerter.Checked = True
        End If
        If UCase(bc_am_workflow.screen_poll) = "TRUE" Then
            Me.pscreen.Checked = True
        End If
        If UCase(bc_am_workflow.polling_enabled) = "TRUE" Then
            'Me.penabled.Checked = True
        Else
            'Me.denabled.Checked = True
        End If
        Me.Interval.Items.Clear()
        For i = 1 To 300
            If i Mod 5 = 0 Then
                Me.Interval.Items.Add(i)
                If i * 1000 = CInt(bc_am_workflow.polling_interval) Then
                    Me.Interval.SelectedIndex = (i / 5) - 1
                End If
            End If
        Next
        Me.UrgentDays.Items.Clear()
        For i = 1 To 31
            Me.UrgentDays.Items.Add(i)
            If i = bc_am_workflow.pre_expire_alert_notify Then
                Me.UrgentDays.SelectedIndex = i - 1
            End If
        Next

        If bc_am_workflow.polling_enabled = "FALSE" Then
            Me.palerter.Enabled = False
            Me.pscreen.Enabled = False
            Me.Interval.Enabled = False
        End If
        bc_am_workflow.ofrm.bload = False


    End Sub

    Private Sub AutoRefresh_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoRefresh.CheckedChanged
        If first = False Then
            first = True
            Exit Sub
        End If

        If Me.AutoRefresh.Checked = False Then
            Me.RefreshSettings.BackColor = System.Drawing.Color.WhiteSmoke
            RefreshSettings.Enabled = False
            Me.InactivitySettings.Enabled = True
            Me.InactivitySettings.BackColor = System.Drawing.Color.White
        Else
            Me.RefreshSettings.BackColor = System.Drawing.Color.White
            RefreshSettings.Enabled = True
            Me.InactivitySettings.Enabled = False
            Me.InactivitySettings.BackColor = System.Drawing.Color.WhiteSmoke
        End If

    End Sub

    Private Sub palerter_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles palerter.CheckedChanged
        If bc_am_workflow.ofrm.bload = True Then
            'Disable alerter- CAB why?
            palerter.Checked = False
            Exit Sub
        End If

        If palerter.Checked Then
            AlerterSettings.BackColor = System.Drawing.Color.White
            AlerterSettings.Enabled = True
        Else
            AlerterSettings.BackColor = System.Drawing.Color.WhiteSmoke
            AlerterSettings.Enabled = False
        End If
    End Sub

    'Custom DrawItem method so that we can set the tab colors
    Private Sub SettingsTabs_DrawItem(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DrawItemEventArgs) Handles SettingsTabs.DrawItem

        Dim g As Graphics = e.Graphics
        Dim tp As TabPage = SettingsTabs.TabPages(e.Index)
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

        'If DocDetailsTabs.SelectedIndex = e.Index Then
        brFore = New SolidBrush(Color.Black)
        'Else
        '    brFore = New SolidBrush(Color.WhiteSmoke)
        'End If

        g.DrawString(strTitle, SettingsTabs.Font, brFore, r, sf)


    End Sub

    Private Sub ViewedBackColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewedBackColor.Click
        Dim dialog As New ColorDialog
        dialog.Color = bc_am_workflow.colors.doc_list_read_backcolor
        dialog.ShowDialog()
        ViewedBackColor.BackColor = dialog.Color
    End Sub

    Private Sub ViewedForeColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewedForeColor.Click
        Dim dialog As New ColorDialog
        dialog.Color = bc_am_workflow.colors.doc_list_read_forecolor
        dialog.ShowDialog()
        ViewedForeColor.BackColor = dialog.Color
    End Sub

    Private Sub UnviewedBackColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UnviewedBackColor.Click
        Dim dialog As New ColorDialog
        dialog.Color = bc_am_workflow.colors.doc_list_unread_backcolor
        dialog.ShowDialog()
        UnviewedBackColor.BackColor = dialog.Color
    End Sub

    Private Sub UnviewedForeColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UnviewedForeColor.Click
        Dim dialog As New ColorDialog
        dialog.Color = bc_am_workflow.colors.doc_list_unread_forecolor
        dialog.ShowDialog()
        UnviewedForeColor.BackColor = dialog.Color
    End Sub

    Private Sub UrgentBackColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UrgentBackColor.Click
        Dim dialog As New ColorDialog
        dialog.Color = bc_am_workflow.colors.doc_list_urgent_backcolor
        dialog.ShowDialog()
        UrgentBackColor.BackColor = dialog.Color
    End Sub

    Private Sub UrgentForeColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UrgentForeColor.Click
        Dim dialog As New ColorDialog
        dialog.Color = bc_am_workflow.colors.doc_list_urgent_forecolor
        dialog.ShowDialog()
        UrgentForeColor.BackColor = dialog.Color
    End Sub

    Private Sub StageChangedBackColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StageChangedBackColor.Click
        Dim dialog As New ColorDialog
        dialog.Color = bc_am_workflow.colors.doc_list_stage_changed_backcolor
        dialog.ShowDialog()
        StageChangedBackColor.BackColor = dialog.Color
    End Sub

    Private Sub StageChangedForeColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StageChangedForeColor.Click
        Dim dialog As New ColorDialog
        dialog.Color = bc_am_workflow.colors.doc_list_stage_changed_forecolor
        dialog.ShowDialog()
        StageChangedForeColor.BackColor = dialog.Color
    End Sub

    Private Sub SearchBackColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SearchBackColor.Click
        Dim dialog As New ColorDialog
        dialog.Color = bc_am_workflow.colors.doc_list_search_backcolor
        dialog.ShowDialog()
        SearchBackColor.BackColor = dialog.Color
    End Sub

    Private Sub SearchResultForeColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SearchResultForeColor.Click
        Dim dialog As New ColorDialog
        dialog.Color = bc_am_workflow.colors.doc_list_search_forecolor
        dialog.ShowDialog()
        SearchResultForeColor.BackColor = dialog.Color
    End Sub

    Private Sub ExpiredBackColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExpiredBackColor.Click
        Dim dialog As New ColorDialog
        dialog.Color = bc_am_workflow.colors.doc_list_expired_backcolor
        dialog.ShowDialog()
        ExpiredBackColor.BackColor = dialog.Color
    End Sub

    Private Sub ExpiredForeColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExpiredForeColor.Click
        Dim dialog As New ColorDialog
        dialog.Color = bc_am_workflow.colors.doc_list_expired_forecolor
        dialog.ShowDialog()
        ExpiredForeColor.BackColor = dialog.Color
    End Sub

    Private Sub CurrentWkfStageBackColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CurrentWkfStageBackColor.Click
        Dim dialog As New ColorDialog
        dialog.Color = bc_am_workflow.colors.workflow_current_stage_backcolor
        dialog.ShowDialog()
        CurrentWkfStageBackColor.BackColor = dialog.Color
    End Sub

    Private Sub CurrentWkfStageForeColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CurrentWkfStageForeColor.Click
        Dim dialog As New ColorDialog
        dialog.Color = bc_am_workflow.colors.workflow_current_stage_forecolor
        dialog.ShowDialog()
        CurrentWkfStageForeColor.BackColor = dialog.Color
    End Sub

    Private Sub NextWkfStageBackColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NextWkfStageBackColor.Click
        Dim dialog As New ColorDialog
        dialog.Color = bc_am_workflow.colors.workflow_next_stage_backcolor
        dialog.ShowDialog()
        NextWkfStageBackColor.BackColor = dialog.Color
    End Sub

    Private Sub NextWkfStageForeColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NextWkfStageForeColor.Click
        Dim dialog As New ColorDialog
        dialog.Color = bc_am_workflow.colors.workflow_next_stage_forecolor
        dialog.ShowDialog()
        NextWkfStageForeColor.BackColor = dialog.Color
    End Sub

    Private Sub CancelButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        Me.Close()
    End Sub

    Private Sub RestorePollingDefaults_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RestorePollingDefaults.Click
        Dim defaults As New bc_am_workflow
        defaults.loadDefaultSettings()

        If bc_am_workflow.auto_refresh Then
            Me.AutoRefresh.Checked = True
            Me.RefreshSettings.Enabled = True
            For i = 0 To 4
                If CStr(bc_am_workflow.screen_refresh_interval) = Me.MinRefreshTime.Items(i) Then
                    Me.MinRefreshTime.SelectedIndex = i
                    Exit For
                End If
            Next
            For i = 0 To 4
                If CStr(bc_am_workflow.screen_inactive_interval) = Me.RefreshAfterActivityTime.Items(i) Then
                    Me.RefreshAfterActivityTime.SelectedIndex = i
                    Exit For
                End If
            Next
            For i = 0 To 5
                If CStr(bc_am_workflow.user_inactive_interval) = Me.InactInterval.Items(i) Then
                    Me.InactInterval.SelectedIndex = i
                    Exit For
                End If
            Next

        Else
            Me.AutoRefresh.Checked = False
            Me.RefreshSettings.Enabled = False
        End If

        Me.PollingEnabled.Checked = bc_am_workflow.polling_enabled
        If Me.PollingEnabled.Checked Then Me.PollingSettings.Enabled = True

        Me.pscreen.Checked = bc_am_workflow.screen_poll
        Me.palerter.Checked = bc_am_workflow.alerter_enabled
        If Me.palerter.Checked Then
            Me.cbeep.Checked = bc_am_workflow.beep_enabled
            Me.AlerterSettings.Enabled = True
        End If
        For i = 0 To bc_am_workflow.ofrm.DocumentList.Columns.Count - 1
            bc_am_workflow.ofrm.DocumentList.Columns(i).DisplayIndex = i
        Next
        bc_am_workflow.ofrm.DocumentList.Columns(0).Width = 201
        bc_am_workflow.ofrm.DocumentList.Columns(1).Width = 1
        bc_am_workflow.ofrm.DocumentList.Columns(2).Width = 142
        bc_am_workflow.ofrm.DocumentList.Columns(3).Width = 114
        bc_am_workflow.ofrm.DocumentList.Columns(4).Width = 88
        bc_am_workflow.ofrm.DocumentList.Columns(5).Width = 87
        bc_am_workflow.ofrm.DocumentList.Columns(6).Width = 115
        bc_am_workflow.ofrm.DocumentList.Columns(7).Width = 93
        bc_am_workflow.ofrm.DocumentList.Columns(8).Width = 115
        bc_am_workflow.ofrm.DocumentList.Columns(9).Width = 81
        Dim pColors As New ProcessColors
        pColors.resetDefaultColors()
        With pColors
            ExpiredBackColor.BackColor = .doc_list_expired_backcolor
            ExpiredForeColor.BackColor = .doc_list_expired_forecolor
            ViewedBackColor.BackColor = .doc_list_read_backcolor
            ViewedForeColor.BackColor = .doc_list_read_forecolor
            SearchBackColor.BackColor = .doc_list_search_backcolor
            SearchResultForeColor.BackColor = .doc_list_search_forecolor
            StageChangedBackColor.BackColor = .doc_list_stage_changed_backcolor
            StageChangedForeColor.BackColor = .doc_list_stage_changed_forecolor
            UnviewedBackColor.BackColor = .doc_list_unread_backcolor
            UnviewedForeColor.BackColor = .doc_list_unread_forecolor
            UrgentBackColor.BackColor = .doc_list_urgent_backcolor
            UrgentForeColor.BackColor = .doc_list_urgent_forecolor
            CurrentWkfStageBackColor.BackColor = .workflow_current_stage_backcolor
            CurrentWkfStageForeColor.BackColor = .workflow_current_stage_forecolor
            NextWkfStageBackColor.BackColor = .workflow_next_stage_backcolor
            NextWkfStageForeColor.BackColor = .workflow_next_stage_forecolor
        End With
        Dim owf As New bc_am_workflow
        owf.setColors(pColors)
        Me.Hide()
    End Sub

    Private Sub RefreshColorDefaults_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim pColors As New ProcessColors
        With pColors
            ExpiredBackColor.BackColor = .doc_list_expired_backcolor
            ExpiredForeColor.BackColor = .doc_list_expired_forecolor
            ViewedBackColor.BackColor = .doc_list_read_backcolor
            ViewedForeColor.BackColor = .doc_list_read_forecolor
            SearchBackColor.BackColor = .doc_list_search_backcolor
            SearchResultForeColor.BackColor = .doc_list_search_forecolor
            StageChangedBackColor.BackColor = .doc_list_stage_changed_backcolor
            StageChangedForeColor.BackColor = .doc_list_stage_changed_forecolor
            UnviewedBackColor.BackColor = .doc_list_unread_backcolor
            UnviewedForeColor.BackColor = .doc_list_unread_forecolor
            UrgentBackColor.BackColor = .doc_list_urgent_backcolor
            UrgentForeColor.BackColor = .doc_list_urgent_forecolor
            CurrentWkfStageBackColor.BackColor = .workflow_current_stage_backcolor
            CurrentWkfStageForeColor.BackColor = .workflow_current_stage_forecolor
            NextWkfStageBackColor.BackColor = .workflow_next_stage_backcolor
            NextWkfStageForeColor.BackColor = .workflow_next_stage_forecolor
        End With
    End Sub

    Private Sub RestoreRefreshDefaults_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim defaults As New bc_am_workflow, i As Integer
        defaults.loadDefaultSettings()

        If bc_am_workflow.auto_refresh Then
            Me.AutoRefresh.Checked = True
            Me.RefreshSettings.Enabled = True
            For i = 0 To 4
                If CStr(bc_am_workflow.screen_refresh_interval) = Me.MinRefreshTime.Items(i) Then
                    Me.MinRefreshTime.SelectedIndex = i
                    Exit For
                End If
            Next
            For i = 0 To 4
                If CStr(bc_am_workflow.screen_inactive_interval) = Me.RefreshAfterActivityTime.Items(i) Then
                    Me.RefreshAfterActivityTime.SelectedIndex = i
                    Exit For
                End If
            Next
            For i = 0 To 5
                If CStr(bc_am_workflow.user_inactive_interval) = Me.InactInterval.Items(i) Then
                    Me.InactInterval.SelectedIndex = i
                    Exit For
                End If
            Next
        Else
            Me.AutoRefresh.Checked = False
            Me.RefreshSettings.Enabled = False
        End If

    End Sub

    Private Sub Label27_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label27.Click

    End Sub

    Private Sub Csnap_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Csnap.CheckedChanged

    End Sub

    Private Sub Cdayasback_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cdayasback.SelectedIndexChanged
        bc_am_workflow.days_back = Me.Cdayasback.SelectedIndex + 1
    End Sub
End Class

