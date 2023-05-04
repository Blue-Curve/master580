Imports System.Windows.Forms
Imports System.Threading
Imports System.Diagnostics
Imports bluecurve.create.am
Imports BlueCurve.Core.AS
Imports BlueCurve.Core.CS


Public Class bc_am_workflow_alert
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
    Friend WithEvents lalert As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_workflow_alert))
        Me.lalert = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'lalert
        '
        Me.lalert.BackColor = System.Drawing.Color.White
        Me.lalert.Location = New System.Drawing.Point(16, 16)
        Me.lalert.Name = "lalert"
        Me.lalert.Size = New System.Drawing.Size(264, 56)
        Me.lalert.TabIndex = 0
        '
        'bc_am_workflow_alert
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(296, 88)
        Me.Controls.Add(Me.lalert)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(700, 660)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "bc_am_workflow_alert"
        Me.Opacity = 0.9
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Blue Curve - alert"
        Me.TopMost = True
        Me.ResumeLayout(False)

    End Sub

#End Region
    Public from_create As Boolean = False
    Public from_service As Boolean = False
    Public application As String = "Create"
    Public doc_id As Long
    Public process_mode As Long
    Public Shared openfrm As bc_am_at_wizard_main
    Private Sub bc_am_workflow_alert_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim r, b As Integer

        r = Screen.PrimaryScreen.WorkingArea.Right()
        b = Screen.PrimaryScreen.WorkingArea.Bottom()
        Me.StartPosition = Windows.Forms.FormStartPosition.Manual
        Dim d As New Drawing.Point(r - Me.Width, b - Me.Height)
        Me.Location = d

    End Sub
    Private Sub lalert_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lalert.DoubleClick
        Me.Cursor = Cursors.WaitCursor
        Try
            If Me.from_service = True Then
                If Me.application = "Create" Then
                    bc_cs_central_settings.create_client_poll_enabled = True
                    REM if application no open launch
                    Dim othread As New Thread(AddressOf launch_create)
                    othread.Start()
                    Me.Hide()
                    Exit Sub
                ElseIf Me.application = "Process" Then
                    Me.Hide()
                    bc_cs_central_settings.process_client_poll_enabled = True
                    Dim othread As New Thread(AddressOf launch_process)
                    othread.Start()
                    Me.Hide()
                    Exit Sub
                Else
                    Exit Sub
                End If
            End If
            Dim owf As New bc_am_workflow
            If from_create = True Then
                openfrm.from_alert = True
                Me.Hide()
                openfrm.WindowState = FormWindowState.Normal
                REM bring it into forground
                Dim oapi As New API
                openfrm.StartPosition = FormStartPosition.CenterScreen
                openfrm.lxpos = openfrm.Location.X
                openfrm.lypos = openfrm.Location.Y

                API.SetWindowPos(openfrm.Handle.ToInt32, API.HWND_TOPMOST, openfrm.lxpos, openfrm.lypos, 1, 1, 1)
                bc_am_at_wizard_generic.mode = 10
                openfrm.load_open_controls()
                openfrm.load_docs(False, True, True, False)
                openfrm.from_alert = True
                bc_am_at_wizard_generic.mode = 11
                REM if single document highlight it
                Dim i As Integer
                If Me.doc_id <> 0 Then
                    For i = 0 To openfrm.ListView1.Items.Count - 1
                        If openfrm.ListView1.Items(i).SubItems(6).Text = CStr(doc_id) Then
                            openfrm.ListView1.Items(i).Selected = True
                            openfrm.ListView1.Items(i).EnsureVisible()
                            Exit For
                        End If
                    Next
                End If
                Exit Sub
            End If
            Me.Hide()
            bc_am_workflow.unread_mode = False
            bc_am_workflow.mode = 0
            If bc_am_workflow.alert_mode <> 0 Then
                bc_am_workflow.mode = bc_am_workflow.alert_mode
            End If
            If bc_am_workflow.alert_doc_id <> -1 Then
                bc_am_workflow.selected_doc_id = bc_am_workflow.alert_doc_id
            Else
                bc_am_workflow.selected_doc_id = -1
            End If

            bc_am_workflow.from_service = False
            bc_am_workflow.mode = bc_am_workflow.alert_mode
            owf.retrieve_docs(False, False)
            bc_am_workflow.cfrm.WindowState = Windows.Forms.FormWindowState.Maximized
        Catch ex As Exception

        Finally
            Me.Cursor = Cursors.Default
            Me.Close()
        End Try
    End Sub
    Private Sub launch_create()

        Dim obcload As New bc_am_load("Create", False)
        bc_am_at_wizard_generic.mode = 10
        openfrm = New bc_am_at_wizard_main
        openfrm.load_start()
        openfrm.load_open_controls()
        openfrm.load_docs(False, True, True, False)
        openfrm.from_alert = True
        bc_am_at_wizard_generic.mode = 11
        REM if single document highlight it
        Dim i As Integer
        If Me.doc_id <> 0 Then
            For i = 0 To openfrm.ListView1.Items.Count - 1
                If openfrm.ListView1.Items(i).SubItems(6).Text = CStr(doc_id) Then
                    openfrm.ListView1.Items(i).Selected = True
                    openfrm.ListView1.Items(i).EnsureVisible()
                    Exit For
                End If
            Next
        End If

        openfrm.ShowDialog()
    End Sub
    Private Sub launch_process()
        Try
            Dim obcload As New bc_am_load("Create", False)
            Dim owf As New bc_am_workflow
            bc_am_workflow.from_service = True

            bc_am_workflow.unread_mode = False
            If Me.doc_id <> 0 Then
                bc_am_workflow.selected_doc_id = Me.doc_id
            Else
                bc_am_workflow.selected_doc_id = -1
            End If
            bc_am_workflow.mode = Me.process_mode
            owf.load()
            bc_am_workflow.cfrm.WindowState = FormWindowState.Maximized


        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    
End Class
