Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports BlueCurve.Core.AS
Imports System.Windows.Forms
Imports System.Collections
Public Class bc_am_in_open_frm
    Inherits create.am.bc_am_at_wizard_generic

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()
        def_size = Me.Size
            col1_def_width.Width = Me.ListView1.Columns(0).Width
            col2_def_width.Width = Me.ListView1.Columns(1).Width
            col3_def_width.Width = Me.ListView1.Columns(2).Width
            col4_def_width.Width = Me.ListView1.Columns(3).Width
            col5_def_width.Width = Me.ListView1.Columns(4).Width
            col6_def_width.Width = Me.ListView1.Columns(5).Width
            col7_def_width.Width = Me.ListView1.Columns(6).Width
            col8_def_width.Width = Me.ListView1.Columns(7).Width
            col9_def_width.Width = Me.ListView1.Columns(8).Width
            read_user_settings(gsettings)
        
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
    Friend WithEvents ListView1 As System.Windows.Forms.ListView
    Friend WithEvents title As System.Windows.Forms.ColumnHeader
    Friend WithEvents DocDate As System.Windows.Forms.ColumnHeader
    Friend WithEvents checkedout As System.Windows.Forms.ColumnHeader
    Friend WithEvents PictureBox4 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox6 As System.Windows.Forms.PictureBox
    Friend WithEvents btncan As System.Windows.Forms.Button
    Friend WithEvents btnopen As System.Windows.Forms.Button
    Friend WithEvents Author As System.Windows.Forms.ColumnHeader
    Friend WithEvents class_id As System.Windows.Forms.ColumnHeader
    Friend WithEvents Lblstages As System.Windows.Forms.Label
    Friend WithEvents entity_name As System.Windows.Forms.ColumnHeader
    Friend WithEvents contributor_name As System.Windows.Forms.ColumnHeader
    Friend WithEvents entity As System.Windows.Forms.ColumnHeader
    Friend WithEvents cont_id As System.Windows.Forms.ColumnHeader
    Friend WithEvents PictureBox5 As System.Windows.Forms.PictureBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents chksettings As System.Windows.Forms.CheckBox
    Friend WithEvents PictureBox7 As System.Windows.Forms.PictureBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(bc_am_in_open_frm))
        Me.ListView1 = New System.Windows.Forms.ListView
        Me.DocDate = New System.Windows.Forms.ColumnHeader
        Me.entity_name = New System.Windows.Forms.ColumnHeader
        Me.Author = New System.Windows.Forms.ColumnHeader
        Me.title = New System.Windows.Forms.ColumnHeader
        Me.class_id = New System.Windows.Forms.ColumnHeader
        Me.contributor_name = New System.Windows.Forms.ColumnHeader
        Me.checkedout = New System.Windows.Forms.ColumnHeader
        Me.entity = New System.Windows.Forms.ColumnHeader
        Me.cont_id = New System.Windows.Forms.ColumnHeader
        Me.PictureBox4 = New System.Windows.Forms.PictureBox
        Me.PictureBox6 = New System.Windows.Forms.PictureBox
        Me.btncan = New System.Windows.Forms.Button
        Me.btnopen = New System.Windows.Forms.Button
        Me.Lblstages = New System.Windows.Forms.Label
        Me.PictureBox5 = New System.Windows.Forms.PictureBox
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.chksettings = New System.Windows.Forms.CheckBox
        Me.PictureBox7 = New System.Windows.Forms.PictureBox
        Me.SuspendLayout()
        '
        'ListView1
        '
        Me.ListView1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListView1.BackColor = System.Drawing.Color.White
        Me.ListView1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.DocDate, Me.entity_name, Me.Author, Me.title, Me.class_id, Me.contributor_name, Me.checkedout, Me.entity, Me.cont_id})
        Me.ListView1.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListView1.FullRowSelect = True
        Me.ListView1.GridLines = True
        Me.ListView1.HideSelection = False
        Me.ListView1.Location = New System.Drawing.Point(96, 112)
        Me.ListView1.MultiSelect = False
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(432, 176)
        Me.ListView1.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.ListView1.TabIndex = 26
        Me.ListView1.View = System.Windows.Forms.View.Details
        '
        'DocDate
        '
        Me.DocDate.Text = "Date"
        Me.DocDate.Width = 126
        '
        'entity_name
        '
        Me.entity_name.Text = "Entity"
        Me.entity_name.Width = 209
        '
        'Author
        '
        Me.Author.Text = "Author"
        Me.Author.Width = 69
        '
        'title
        '
        Me.title.Text = "Title"
        Me.title.Width = 0
        '
        'class_id
        '
        Me.class_id.Text = "Class"
        Me.class_id.Width = 74
        '
        'contributor_name
        '
        Me.contributor_name.Text = "Schema"
        Me.contributor_name.Width = 80
        '
        'checkedout
        '
        Me.checkedout.Text = "Checked Out"
        Me.checkedout.Width = 191
        '
        'entity
        '
        Me.entity.Text = "Entity Id"
        '
        'cont_id
        '
        Me.cont_id.Text = "contributor"
        '
        'PictureBox4
        '
        Me.PictureBox4.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox4.Image = CType(resources.GetObject("PictureBox4.Image"), System.Drawing.Image)
        Me.PictureBox4.Location = New System.Drawing.Point(16, 88)
        Me.PictureBox4.Name = "PictureBox4"
        Me.PictureBox4.Size = New System.Drawing.Size(72, 64)
        Me.PictureBox4.TabIndex = 52
        Me.PictureBox4.TabStop = False
        Me.ToolTip1.SetToolTip(Me.PictureBox4, "Show All Workbooks I Am Allowed To See")
        '
        'PictureBox6
        '
        Me.PictureBox6.Image = CType(resources.GetObject("PictureBox6.Image"), System.Drawing.Image)
        Me.PictureBox6.Location = New System.Drawing.Point(16, 216)
        Me.PictureBox6.Name = "PictureBox6"
        Me.PictureBox6.Size = New System.Drawing.Size(72, 62)
        Me.PictureBox6.TabIndex = 54
        Me.PictureBox6.TabStop = False
        Me.ToolTip1.SetToolTip(Me.PictureBox6, "Show Offline Workbooks")
        '
        'btncan
        '
        Me.btncan.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btncan.BackColor = System.Drawing.SystemColors.ControlLight
        Me.btncan.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btncan.Location = New System.Drawing.Point(456, 332)
        Me.btncan.Name = "btncan"
        Me.btncan.Size = New System.Drawing.Size(72, 24)
        Me.btncan.TabIndex = 74
        Me.btncan.Text = "&Cancel"
        Me.ToolTip1.SetToolTip(Me.btncan, "Close Form")
        '
        'btnopen
        '
        Me.btnopen.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnopen.BackColor = System.Drawing.SystemColors.ControlLight
        Me.btnopen.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnopen.Location = New System.Drawing.Point(456, 304)
        Me.btnopen.Name = "btnopen"
        Me.btnopen.Size = New System.Drawing.Size(72, 24)
        Me.btnopen.TabIndex = 73
        Me.btnopen.Text = "&Open"
        Me.ToolTip1.SetToolTip(Me.btnopen, "Open Selected Workbook")
        '
        'Lblstages
        '
        Me.Lblstages.BackColor = System.Drawing.Color.FromArgb(CType(235, Byte), CType(240, Byte), CType(249, Byte))
        Me.Lblstages.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Lblstages.Location = New System.Drawing.Point(96, 88)
        Me.Lblstages.Name = "Lblstages"
        Me.Lblstages.Size = New System.Drawing.Size(232, 16)
        Me.Lblstages.TabIndex = 75
        Me.Lblstages.Text = "Please select a workbook to open..."
        '
        'PictureBox5
        '
        Me.PictureBox5.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox5.Image = CType(resources.GetObject("PictureBox5.Image"), System.Drawing.Image)
        Me.PictureBox5.Location = New System.Drawing.Point(16, 152)
        Me.PictureBox5.Name = "PictureBox5"
        Me.PictureBox5.Size = New System.Drawing.Size(72, 62)
        Me.PictureBox5.TabIndex = 76
        Me.PictureBox5.TabStop = False
        Me.ToolTip1.SetToolTip(Me.PictureBox5, "Show WorkBooks That I Have A Preference For")
        '
        'ToolTip1
        '
        Me.ToolTip1.AutomaticDelay = 50
        '
        'chksettings
        '
        Me.chksettings.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chksettings.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chksettings.Location = New System.Drawing.Point(3, 351)
        Me.chksettings.Name = "chksettings"
        Me.chksettings.Size = New System.Drawing.Size(160, 16)
        Me.chksettings.TabIndex = 77
        Me.chksettings.Text = "Restore Default Form Settings"
        '
        'PictureBox7
        '
        Me.PictureBox7.Image = CType(resources.GetObject("PictureBox7.Image"), System.Drawing.Image)
        Me.PictureBox7.Location = New System.Drawing.Point(474, 0)
        Me.PictureBox7.Name = "PictureBox7"
        Me.PictureBox7.Size = New System.Drawing.Size(64, 64)
        Me.PictureBox7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox7.TabIndex = 78
        Me.PictureBox7.TabStop = False
        '
        'bc_am_in_open_frm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(538, 366)
        Me.Controls.Add(Me.PictureBox7)
        Me.Controls.Add(Me.chksettings)
        Me.Controls.Add(Me.PictureBox5)
        Me.Controls.Add(Me.Lblstages)
        Me.Controls.Add(Me.btncan)
        Me.Controls.Add(Me.btnopen)
        Me.Controls.Add(Me.PictureBox6)
        Me.Controls.Add(Me.PictureBox4)
        Me.Controls.Add(Me.ListView1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "bc_am_in_open_frm"
        Me.Text = "Form1"
        Me.Controls.SetChildIndex(Me.ListView1, 0)
        Me.Controls.SetChildIndex(Me.PictureBox4, 0)
        Me.Controls.SetChildIndex(Me.PictureBox6, 0)
        Me.Controls.SetChildIndex(Me.btnopen, 0)
        Me.Controls.SetChildIndex(Me.btncan, 0)
        Me.Controls.SetChildIndex(Me.Lblstages, 0)
        Me.Controls.SetChildIndex(Me.PictureBox5, 0)
        Me.Controls.SetChildIndex(Me.chksettings, 0)
        Me.Controls.SetChildIndex(Me.PictureBox7, 0)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public ok_selected As Boolean
    Public gsettings As New bc_om_insight_settings
    Public col1_def_width As System.Drawing.Size
    Public col2_def_width As System.Drawing.Size
    Public col3_def_width As System.Drawing.Size
    Public col4_def_width As System.Drawing.Size
    Public col5_def_width As System.Drawing.Size
    Public col6_def_width As System.Drawing.Size
    Public col7_def_width As System.Drawing.Size
    Public col8_def_width As System.Drawing.Size
    Public col9_def_width As System.Drawing.Size
    Public col10_def_width As System.Drawing.Size
    Public def_size As System.Drawing.Size
    Private Sub btncan_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btncan.Click
        save_settings(Me.gsettings)
        Me.Hide()
    End Sub
    Private Sub load_docs(ByVal network As Boolean, ByVal mine As Boolean)
        Dim slog As New bc_cs_activity_log("bc_am_in_submit", "load_docs", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            Dim i, k As Integer
            If bc_cs_central_settings.selected_conn_method <> bc_cs_central_settings.LOCAL And network = True Then
                Dim bc_am_in_open_workbook_list As New bc_am_in_open_workbook_list(True, mine)
            Else
                Dim bc_am_in_open_workbook_list As New bc_am_in_open_workbook_list(False, mine)
            End If
            ListView1.Items.Clear()
            Dim item As ListViewItem
            'ListView1.SmallImageList = ImageList1
            If Not IsNothing(bc_am_load_objects.obc_workbooks) Then
                For i = 0 To bc_am_load_objects.obc_workbooks.bc_om_insight_contribution_for_entity.Count - 1
                    With bc_am_load_objects.obc_workbooks.bc_om_insight_contribution_for_entity(i)
                        REM list view
                        item = New ListViewItem(CStr(.last_submission_date))
                        Dim class_name As String = ""
                        Dim found As Boolean
                        found = False
                        For k = 0 To bc_am_load_objects.obc_entities.entity.Count - 1
                            If bc_am_load_objects.obc_entities.entity(k).id = .lead_entity_id Then
                                item.SubItems.Add(CStr(bc_am_load_objects.obc_entities.entity(k).name))
                                found = True
                                class_name = bc_am_load_objects.obc_entities.entity(k).class_name
                            End If
                        Next
                        If found = False Then
                            item.SubItems.Add("Inactive")
                        End If
                        REM Author
                        found = False
                        For k = 0 To bc_am_load_objects.obc_users.user.Count - 1
                            If CStr(bc_am_load_objects.obc_users.user(k).id) = .author_id Then
                                item.SubItems.Add(bc_am_load_objects.obc_users.user(k).first_name + " " + bc_am_load_objects.obc_users.user(k).surname)
                                found = True
                                Exit For
                            End If
                        Next
                        If found = False Then
                            item.SubItems.Add("")
                        End If
                        item.SubItems.Add(.workbook_name)
                        item.SubItems.Add(class_name)

                        If bc_am_load_objects.obc_workbooks.bc_om_insight_contribution_for_entity(i).schema_name <> "" Then
                            item.SubItems.Add(bc_am_load_objects.obc_workbooks.bc_om_insight_contribution_for_entity(i).schema_name)
                        Else
                            item.SubItems.Add("")
                        End If

                        REM checked out user
                        found = False
                        If .checked_out_user_id = 0 Then
                            item.ImageIndex = 0
                            item.SubItems.Add("")
                        Else
                            For k = 0 To bc_am_load_objects.obc_users.user.Count - 1

                                If CStr(bc_am_load_objects.obc_users.user(k).id) = .checked_out_user_id Then
                                    item.SubItems.Add(bc_am_load_objects.obc_users.user(k).first_name + " " + bc_am_load_objects.obc_users.user(k).surname)
                                    found = True
                                    Exit For
                                End If
                            Next
                            If found = False Then
                                item.SubItems.Add("")
                            End If
                        End If
                        item.SubItems.Add(.lead_entity_id)
                        item.SubItems.Add(.contributor_id)
                        ListView1.Items.Add(item)
                    End With
                Next
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_load_objects", "load_docs", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            slog = New bc_cs_activity_log("bc_am_in_submit", "load_docs", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Private Sub bc_am_in_open_frm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.Text = "Blue Curve Insight"
        Me.btnopen.Enabled = False
        Me.ok_selected = False
        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.LOCAL Then
            PictureBox6.BorderStyle = BorderStyle.Fixed3D
            PictureBox4.BorderStyle = BorderStyle.None
            PictureBox4.Enabled = False
            PictureBox5.BorderStyle = BorderStyle.None
            PictureBox5.Enabled = False
            load_docs(False, False)
        Else
            PictureBox4.BorderStyle = BorderStyle.None
            PictureBox5.BorderStyle = BorderStyle.Fixed3D
            PictureBox6.BorderStyle = BorderStyle.None
            load_docs(True, True)
        End If
        Me.btnopen.Enabled = False
    End Sub
    Private Sub PictureBox4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox4.Click
        Dim otrace As New bc_cs_activity_log("bc_am_in_open_frm", "PictureBox4_Click", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Me.Cursor = Cursors.WaitCursor
            PictureBox4.BorderStyle = BorderStyle.Fixed3D
            PictureBox5.BorderStyle = BorderStyle.None
            PictureBox6.BorderStyle = BorderStyle.None
            load_docs(True, False)
            Me.btnopen.Enabled = False

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_open_frm", "PictureBox4_Click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.Cursor = Cursors.Default
            otrace = New bc_cs_activity_log("bc_am_in_open_frm", "PictureBox4_Click", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    Private Sub PictureBox6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox6.Click
        Dim otrace As New bc_cs_activity_log("bc_am_in_open_frm", "PictureBox4_Click", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Me.Cursor = Cursors.WaitCursor
            PictureBox4.BorderStyle = BorderStyle.None
            PictureBox5.BorderStyle = BorderStyle.None
            PictureBox6.BorderStyle = BorderStyle.Fixed3D
            load_docs(False, False)

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_open_frm", "PictureBox4_Click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.Cursor = Cursors.Default
            otrace = New bc_cs_activity_log("bc_am_in_open_frm", "PictureBox4_Click", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub



    Private Sub btnopen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnopen.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            save_settings(Me.gsettings)
            open_doc()
        Catch

        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub
    Public Sub open_doc()

        Dim otrace As New bc_cs_activity_log("bc_am_in_open_doc", "btnopen_Click", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve Insight", "Opening Workbook...", 10, False, True)
            bc_am_load_objects.obc_om_insight_contribution_for_entity = New bc_om_insight_contribution_for_entity
            With bc_am_load_objects.obc_om_insight_contribution_for_entity
                .lead_entity_id = CLng(ListView1.SelectedItems(0).SubItems(7).Text)
                '.lead_class_id = CLng(ListView1.SelectedItems(0).SubItems(2).Text)
                .contributor_id = CLng(ListView1.SelectedItems(0).SubItems(8).Text)
                If PictureBox6.BorderStyle = BorderStyle.Fixed3D Then
                    REM for local set as checked out
                    .checked_out_user_id = -1
                End If
            End With
            Me.Hide()
            ok_selected = True
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_open_doc", "btnopen_Click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            bc_cs_central_settings.progress_bar.unload()
            otrace = New bc_cs_activity_log("bc_am_in_open_doc", "btnopen_Click", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub


    Private Sub ListView1_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles ListView1.ColumnClick
        Dim slog = New bc_cs_activity_log("bc_am_in_open_frm", "get_entity_id_selected", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Select Case CInt(e.Column.ToString)
                Case 0
                    REM date
                    ListView1.ListViewItemSorter() = New CompareBydate
                    If CompareBydate.toggle = False Then
                        CompareBydate.toggle = True
                    Else
                        CompareBydate.toggle = False
                    End If
                Case 1
                    REM entity
                    ListView1.ListViewItemSorter() = New CompareByentity
                    If CompareByentity.toggle = False Then
                        CompareByentity.toggle = True
                    Else
                        CompareByentity.toggle = False
                    End If
                Case 2
                    REM author
                    ListView1.ListViewItemSorter() = New CompareByauthor

                    If CompareByauthor.toggle = False Then
                        CompareByauthor.toggle = True
                    Else
                        CompareByauthor.toggle = False
                    End If
                Case 3
                    REM title
                    ListView1.ListViewItemSorter() = New CompareByname
                    If CompareByname.toggle = False Then
                        CompareByname.toggle = True
                    Else
                        CompareByname.toggle = False
                    End If

                Case 4
                    REM class
                    ListView1.ListViewItemSorter() = New CompareByclass
                    If CompareByclass.toggle = False Then
                        CompareByclass.toggle = True
                    Else
                        CompareByauthor.toggle = False
                    End If
                Case 5
                    REM contribuor
                    ListView1.ListViewItemSorter() = New CompareByco
                    If CompareByco.toggle = False Then
                        CompareByco.toggle = True
                    Else
                        CompareBydate.toggle = False
                    End If

                Case 6
                    REM checked out
                    ListView1.ListViewItemSorter() = New CompareBytype
                    If CompareBytype.toggle = False Then
                        CompareBytype.toggle = True
                    Else
                        CompareBytype.toggle = False
                    End If
            End Select
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_open_frm", "columnclick", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_in_open_frm", "columnclick", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    Class CompareByname
        Implements System.Collections.IComparer
        Public Shared toggle As Boolean = False

        Function compare(ByVal x As Object, ByVal y As Object) As Integer _
        Implements System.Collections.IComparer.Compare

            Dim item1 As ListViewItem = CType(x, ListViewItem)
            Dim item2 As ListViewItem = CType(y, ListViewItem)
            If toggle = True Then
                Return String.Compare(item1.SubItems(3).Text, item2.SubItems(3).Text)
            Else
                Return String.Compare(item2.SubItems(3).Text, item1.SubItems(3).Text)
            End If
        End Function

        Public Sub New()

        End Sub
    End Class
    Class CompareByentity
        Implements System.Collections.IComparer
        Public Shared toggle As Boolean = False

        Function compare(ByVal x As Object, ByVal y As Object) As Integer _
        Implements System.Collections.IComparer.Compare

            Dim item1 As ListViewItem = CType(x, ListViewItem)
            Dim item2 As ListViewItem = CType(y, ListViewItem)
            If toggle = True Then
                Return String.Compare(item1.SubItems(1).Text, item2.SubItems(1).Text)
            Else
                Return String.Compare(item2.SubItems(1).Text, item1.SubItems(1).Text)
            End If
        End Function
    End Class
    Class CompareByclass
        Implements System.Collections.IComparer
        Public Shared toggle As Boolean = False

        Function compare(ByVal x As Object, ByVal y As Object) As Integer _
        Implements System.Collections.IComparer.Compare

            Dim item1 As ListViewItem = CType(x, ListViewItem)
            Dim item2 As ListViewItem = CType(y, ListViewItem)
            If toggle = True Then
                Return String.Compare(item1.SubItems(4).Text, item2.SubItems(4).Text)
            Else
                Return String.Compare(item2.SubItems(4).Text, item1.SubItems(4).Text)
            End If
        End Function
    End Class
    Class CompareBycont
        Implements System.Collections.IComparer
        Public Shared toggle As Boolean = False

        Function compare(ByVal x As Object, ByVal y As Object) As Integer _
        Implements System.Collections.IComparer.Compare

            Dim item1 As ListViewItem = CType(x, ListViewItem)
            Dim item2 As ListViewItem = CType(y, ListViewItem)
            If toggle = True Then
                Return String.Compare(item1.SubItems(5).Text, item2.SubItems(5).Text)
            Else
                Return String.Compare(item2.SubItems(5).Text, item1.SubItems(5).Text)
            End If
        End Function
    End Class
    Class CompareByauthor
        Implements System.Collections.IComparer
        Public Shared toggle As Boolean = False

        Function compare(ByVal x As Object, ByVal y As Object) As Integer _
        Implements System.Collections.IComparer.Compare

            Dim item1 As ListViewItem = CType(x, ListViewItem)
            Dim item2 As ListViewItem = CType(y, ListViewItem)
            If toggle = True Then
                Return String.Compare(item1.SubItems(2).Text, item2.SubItems(2).Text)
            Else
                Return String.Compare(item2.SubItems(2).Text, item1.SubItems(2).Text)
            End If
        End Function
    End Class
    Class CompareBydate
        Implements System.Collections.IComparer
        Public Shared toggle As Boolean = False

        Function compare(ByVal x As Object, ByVal y As Object) As Integer _
        Implements System.Collections.IComparer.Compare
            Try
                Dim item1 As ListViewItem = CType(x, ListViewItem)
                Dim item2 As ListViewItem = CType(y, ListViewItem)
                If toggle = True Then
                    Return Date.Compare(Date.Parse(item1.Text), Date.Parse(item2.Text))
                Else
                    Return Date.Compare(Date.Parse(item2.Text), Date.Parse(item1.Text))
                End If
            Catch

            End Try
        End Function

        Public Sub New()

        End Sub
    End Class

    Class CompareBytype
        Implements System.Collections.IComparer
        Public Shared toggle As Boolean = False

        Function compare(ByVal x As Object, ByVal y As Object) As Integer _
        Implements System.Collections.IComparer.Compare
            Dim item1 As ListViewItem = CType(x, ListViewItem)
            Dim item2 As ListViewItem = CType(y, ListViewItem)
            If toggle = True Then
                Return String.Compare(item1.SubItems(6).Text, item2.SubItems(6).Text)
            Else
                Return String.Compare(item2.SubItems(6).Text, item1.SubItems(6).Text)
            End If
        End Function
    End Class
    Class CompareBystage
        Implements System.Collections.IComparer
        Public Shared toggle As Boolean = False

        Function compare(ByVal x As Object, ByVal y As Object) As Integer _
        Implements System.Collections.IComparer.Compare
            Dim item1 As ListViewItem = CType(x, ListViewItem)
            Dim item2 As ListViewItem = CType(y, ListViewItem)
            If toggle = True Then
                Return String.Compare(item1.SubItems(4).Text, item2.SubItems(4).Text)
            Else
                Return String.Compare(item2.SubItems(4).Text, item1.SubItems(4).Text)
            End If
        End Function
    End Class
    Class CompareByco
        Implements System.Collections.IComparer
        Public Shared toggle As Boolean = False

        Function compare(ByVal x As Object, ByVal y As Object) As Integer _
        Implements System.Collections.IComparer.Compare
            Dim item1 As ListViewItem = CType(x, ListViewItem)
            Dim item2 As ListViewItem = CType(y, ListViewItem)
            If toggle = True Then
                Return String.Compare(item1.SubItems(5).Text, item2.SubItems(5).Text)
            Else
                Return String.Compare(item2.SubItems(5).Text, item1.SubItems(5).Text)
            End If
        End Function
    End Class

    Private Sub ListView1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView1.SelectedIndexChanged
        Me.btnopen.Enabled = True
    End Sub


    Private Sub PictureBox5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox5.Click
        Dim otrace As New bc_cs_activity_log("bc_am_in_open_frm", "PictureBox5_Click", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            PictureBox4.BorderStyle = BorderStyle.None
            PictureBox5.BorderStyle = BorderStyle.Fixed3D
            PictureBox6.BorderStyle = BorderStyle.None
            load_docs(True, True)
            Me.btnopen.Enabled = False

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_open_frm", "PictureBox5_Click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_am_in_open_frm", "PictureBox5_Click", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Private Sub ListView1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListView1.DoubleClick
        open_doc()
    End Sub
    Private Function save_settings(ByRef osettings As bc_om_insight_settings) As Boolean
        Dim fn As String = ""
        Try
            osettings.size = Me.Size
            osettings.col_1_width.Width = Me.ListView1.Columns(0).Width
            osettings.col_2_width.Width = Me.ListView1.Columns(1).Width
            osettings.col_3_width.Width = Me.ListView1.Columns(2).Width
            osettings.col_4_width.Width = Me.ListView1.Columns(3).Width
            osettings.col_5_width.Width = Me.ListView1.Columns(4).Width
            osettings.col_6_width.Width = Me.ListView1.Columns(5).Width
            osettings.col_7_width.Width = Me.ListView1.Columns(6).Width
            osettings.col_8_width.Width = Me.ListView1.Columns(7).Width
            osettings.col_9_width.Width = Me.ListView1.Columns(8).Width
            Dim fs As New bc_cs_file_transfer_services
            Dim bcs As New bc_cs_central_settings
            fn = bc_cs_central_settings.get_user_dir + "bc_am_insight_settings_" + CStr(bc_cs_central_settings.logged_on_user_id) + ".xml"
            osettings.write_data_to_file(fn)
        Catch ex As Exception
            Dim ocommentary As New bc_cs_activity_log("bc_am_wizard_main", "read_user_settings", bc_cs_activity_codes.COMMENTARY, "Failed to write user settings: " + fn)
        End Try

    End Function

    Private Sub bc_am_at_wizard_main_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        save_settings(Me.gsettings)
    End Sub
    Private Sub restore_factory_settings()
        REM remove config file
        Dim bcs As New bc_cs_central_settings
        Dim fs As New bc_cs_file_transfer_services
        Dim fn As String
        fn = bc_cs_central_settings.get_user_dir + "bc_am_insight_settings_" + CStr(bc_cs_central_settings.logged_on_user_id) + ".xml"
        If fs.check_document_exists(fn) Then
            fs.delete_file(fn)
        End If

        Me.Size = Me.def_size
        Me.ListView1.Columns(0).Width = col1_def_width.Width
        Me.ListView1.Columns(1).Width = col2_def_width.Width
        Me.ListView1.Columns(2).Width = col3_def_width.Width
        Me.ListView1.Columns(3).Width = col4_def_width.Width
        Me.ListView1.Columns(4).Width = col5_def_width.Width
        Me.ListView1.Columns(5).Width = col6_def_width.Width
        Me.ListView1.Columns(6).Width = col7_def_width.Width
        Me.ListView1.Columns(7).Width = col8_def_width.Width
        Me.ListView1.Columns(8).Width = col9_def_width.Width

        'load_open_controls()
    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chksettings.CheckedChanged
        If Me.chksettings.Checked = True Then
            restore_factory_settings()
            Me.chksettings.Checked = False
        End If
    End Sub
    Private Function read_user_settings(ByRef osettings As bc_om_insight_settings) As Boolean
        Dim fn As String = ""
        Try
            Dim fs As New bc_cs_file_transfer_services
            read_user_settings = False
            Dim bcs As New bc_cs_central_settings
            fn = bc_cs_central_settings.get_user_dir + "bc_am_insight_settings_" + CStr(bc_cs_central_settings.logged_on_user_id) + ".xml"
            If fs.check_document_exists(fn) Then
                osettings = osettings.read_data_from_file(fn)
                read_user_settings = True
                Me.Size = osettings.size
                Me.ListView1.Columns(0).Width = osettings.col_1_width.Width
                Me.ListView1.Columns(1).Width = osettings.col_2_width.Width
                Me.ListView1.Columns(2).Width = osettings.col_3_width.Width
                Me.ListView1.Columns(3).Width = osettings.col_4_width.Width
                Me.ListView1.Columns(4).Width = osettings.col_5_width.Width
                Me.ListView1.Columns(5).Width = osettings.col_6_width.Width
                Me.ListView1.Columns(6).Width = osettings.col_7_width.Width
                Me.ListView1.Columns(7).Width = osettings.col_8_width.Width
                Me.ListView1.Columns(8).Width = osettings.col_9_width.Width
            End If
        Catch
            Dim ocommentary As New bc_cs_activity_log("bc_am_wizard_main", "read_user_settings", bc_cs_activity_codes.COMMENTARY, "Failed to read user settings: " + fn)

        End Try
    End Function
End Class
