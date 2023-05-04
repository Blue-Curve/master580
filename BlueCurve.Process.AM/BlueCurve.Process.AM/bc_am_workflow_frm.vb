Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports BlueCurve.create.am
Imports BlueCurve.Core.AS
Imports System.Windows.forms
Imports System.Threading
Imports System.Collections
Public Class bc_am_workflow_frm

    Inherits System.Windows.Forms.Form

    Public no_poll As Boolean = False

    Public bload As Boolean

    Public snew As Boolean

    Public bform_load As Boolean

    Public Shared column As Integer
    Public Shared SortToggle As Boolean = True

    Public Shared not_clear As Boolean

    Public no_column As Boolean

    'Constants for indicies in image list
    Private Const CHECKED_IN As Integer = 0
    Private Const CHECKED_OUT As Integer = 2
    Friend WithEvents checked_out_to As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Private Const CHECKED_OUT_TO_ME As Integer = 1


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

    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList




    Friend WithEvents Title As System.Windows.Forms.ColumnHeader
    Friend WithEvents doc_id As System.Windows.Forms.ColumnHeader
    Friend WithEvents da As System.Windows.Forms.ColumnHeader
    Friend WithEvents type As System.Windows.Forms.ColumnHeader
    Friend WithEvents Stage As System.Windows.Forms.ColumnHeader
    Friend WithEvents author As System.Windows.Forms.ColumnHeader
    Friend WithEvents entity As System.Windows.Forms.ColumnHeader
    Friend WithEvents expire_date As System.Windows.Forms.ColumnHeader
    Friend WithEvents bus_area As System.Windows.Forms.ColumnHeader
    Friend WithEvents ContextMenu1 As System.Windows.Forms.ContextMenu
    Friend WithEvents DocumentList As System.Windows.Forms.ListView
    Friend WithEvents ViewMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents CheckOutMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents CheckInMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents CategoriseMenuItem As System.Windows.Forms.MenuItem

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_workflow_frm))
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.Title = New System.Windows.Forms.ColumnHeader
        Me.doc_id = New System.Windows.Forms.ColumnHeader
        Me.da = New System.Windows.Forms.ColumnHeader
        Me.type = New System.Windows.Forms.ColumnHeader
        Me.Stage = New System.Windows.Forms.ColumnHeader
        Me.author = New System.Windows.Forms.ColumnHeader
        Me.entity = New System.Windows.Forms.ColumnHeader
        Me.expire_date = New System.Windows.Forms.ColumnHeader
        Me.bus_area = New System.Windows.Forms.ColumnHeader
        Me.DocumentList = New System.Windows.Forms.ListView
        Me.checked_out_to = New System.Windows.Forms.ColumnHeader
        Me.ContextMenu1 = New System.Windows.Forms.ContextMenu
        Me.ViewMenuItem = New System.Windows.Forms.MenuItem
        Me.CheckOutMenuItem = New System.Windows.Forms.MenuItem
        Me.CheckInMenuItem = New System.Windows.Forms.MenuItem
        Me.CategoriseMenuItem = New System.Windows.Forms.MenuItem
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.SuspendLayout()
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "")
        Me.ImageList1.Images.SetKeyName(1, "")
        Me.ImageList1.Images.SetKeyName(2, "")
        Me.ImageList1.Images.SetKeyName(3, "")
        '
        'Title
        '
        Me.Title.Text = "Title"
        Me.Title.Width = 201
        '
        'doc_id
        '
        Me.doc_id.Text = "Id"
        Me.doc_id.Width = 1
        '
        'da
        '
        Me.da.Text = "Date"
        Me.da.Width = 142
        '
        'type
        '
        Me.type.Text = "Pub Type"
        Me.type.Width = 114
        '
        'Stage
        '
        Me.Stage.Text = "Stage"
        Me.Stage.Width = 88
        '
        'author
        '
        Me.author.Text = "Author"
        Me.author.Width = 87
        '
        'entity
        '
        Me.entity.Text = "Entity"
        Me.entity.Width = 95
        '
        'expire_date
        '
        Me.expire_date.Text = "Completed By"
        Me.expire_date.Width = 115
        '
        'bus_area
        '
        Me.bus_area.Text = "Bus Area"
        Me.bus_area.Width = 81
        '
        'DocumentList
        '
        Me.DocumentList.AllowColumnReorder = True
        Me.DocumentList.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DocumentList.BackColor = System.Drawing.Color.White
        Me.DocumentList.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.Title, Me.doc_id, Me.da, Me.type, Me.Stage, Me.author, Me.checked_out_to, Me.entity, Me.expire_date, Me.bus_area, Me.ColumnHeader1})
        Me.DocumentList.ContextMenu = Me.ContextMenu1
        Me.DocumentList.Font = New System.Drawing.Font("Verdana", 7.25!)
        Me.DocumentList.FullRowSelect = True
        Me.DocumentList.GridLines = True
        Me.DocumentList.Location = New System.Drawing.Point(2, 3)
        Me.DocumentList.MultiSelect = False
        Me.DocumentList.Name = "DocumentList"
        Me.DocumentList.Size = New System.Drawing.Size(842, 567)
        Me.DocumentList.SmallImageList = Me.ImageList1
        Me.DocumentList.TabIndex = 9
        Me.DocumentList.UseCompatibleStateImageBehavior = False
        Me.DocumentList.View = System.Windows.Forms.View.Details
        '
        'checked_out_to
        '
        Me.checked_out_to.Text = "Checked Out To"
        Me.checked_out_to.Width = 115
        '
        'ContextMenu1
        '
        Me.ContextMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.ViewMenuItem, Me.CheckOutMenuItem, Me.CheckInMenuItem, Me.CategoriseMenuItem})
        '
        'ViewMenuItem
        '
        Me.ViewMenuItem.Index = 0
        Me.ViewMenuItem.Text = "View"
        '
        'CheckOutMenuItem
        '
        Me.CheckOutMenuItem.Index = 1
        Me.CheckOutMenuItem.Text = "Check out"
        '
        'CheckInMenuItem
        '
        Me.CheckInMenuItem.Index = 2
        Me.CheckInMenuItem.Text = "Check in"
        Me.CheckInMenuItem.Visible = False
        '
        'CategoriseMenuItem
        '
        Me.CategoriseMenuItem.Index = 3
        Me.CategoriseMenuItem.Text = "Change Categorisation"
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Language"
        '
        'bc_am_workflow_frm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(856, 582)
        Me.ControlBox = False
        Me.Controls.Add(Me.DocumentList)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "bc_am_workflow_frm"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds
        Me.ResumeLayout(False)

    End Sub



#End Region



    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim slog As New bc_cs_activity_log("bc_am_workflow_frm", "form_load", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            DocumentList.SmallImageList = ImageList1
            bform_load = True

            bform_load = False

            bc_am_workflow.snew = True

            

        Catch ex As Exception
            MsgBox(ex.Message)


        Finally

            slog = New bc_cs_activity_log("bc_am_workflow_frm", "form_load", bc_cs_activity_codes.TRACE_EXIT, "")

        End Try

    End Sub

    Private Sub Form1_Quit(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Closed

        Try

            workflow_exit()

        Catch



        End Try

    End Sub

    Private Sub workflow_exit()

        Try

            bc_am_workflow.write_user_settings_fo_file()

            Me.Close()

        Catch ex As Exception



        End Try

    End Sub
    Private Function in_pub_type_filter(ByVal pub_type_id As String) As Boolean
        Dim i As Integer
        in_pub_type_filter = False
        For i = 0 To bc_am_workflow.spubtype_id.Count - 1
            If bc_am_workflow.spubtype_id(i) = pub_type_id Then
                in_pub_type_filter = True
                Exit Function
            End If
        Next
    End Function
    Private Function in_entity_filter(ByVal entity_id As String) As Boolean
        Dim i As Integer
        in_entity_filter = False
        For i = 0 To bc_am_workflow.sentity_id.Count - 1
            If bc_am_workflow.sentity_id(i) = entity_id Then
                in_entity_filter = True
                Exit Function
            End If
        Next
    End Function
    Private Function in_stage_filter(ByVal stage As String) As Boolean
        Dim i As Integer
        in_stage_filter = False
        For i = 0 To bc_am_workflow.sstage.Count - 1
            If bc_am_workflow.sstage(i) = stage Then
                in_stage_filter = True
                Exit Function
            End If
        Next
    End Function
    Private Function in_author_filter(ByVal author_id As String) As Boolean
        Dim i As Integer
        in_author_filter = False
        For i = 0 To bc_am_workflow.sauthor_id.Count - 1
            If bc_am_workflow.sauthor_id(i) = author_id Then
                in_author_filter = True
                Exit Function
            End If
        Next
    End Function
    Public Function load_data(ByVal mode As Integer, Optional ByVal autorefresh As Boolean = False) As Boolean

        Dim slog As New bc_cs_activity_log("bc_am_workflow_frm", "load_data", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim efound As Boolean
        Try
            Dim exclude As Boolean
            'Me.DocumentList.Visible = False
            bc_am_workflow.loading = True
            If bc_am_workflow.processing = True Or (bc_am_workflow.auto_refresh = False And autorefresh = False) Then
                Exit Function

            End If

            Dim i, k, j As Integer

            Me.DocumentList.OwnerDraw = True

            Me.DocumentList.BeginUpdate()
            Me.DocumentList.Items.Clear()

            bc_am_workflow.sdocs.Clear()
            REM reset filters
            bc_am_workflow.lpub_types.Clear()
            bc_am_workflow.lentity.Clear()
            bc_am_workflow.lauthor.Clear()
            bc_am_workflow.lbusarea.Clear()
            bc_am_workflow.lstages.Clear()
            Dim ico As Integer
            ico = -1
            Dim dlist As New ArrayList
            'dlist.Clear()

            k = 0

            For i = 0 To bc_am_workflow.docs.document.Count - 1
                REM collate filter lists
                efound = False

                bc_am_workflow.lpub_types.Add(bc_am_workflow.docs.document(i).pub_type_id)
                If bc_am_workflow.docs.document(i).entity_id > 0 Then
                    bc_am_workflow.lentity.Add(bc_am_workflow.docs.document(i).entity_id)
                End If
                bc_am_workflow.lauthor.Add(bc_am_workflow.docs.document(i).originating_author)
                bc_am_workflow.lbusarea.Add(bc_am_workflow.docs.document(i).bus_area)
                bc_am_workflow.lpub_types.Add(bc_am_workflow.docs.document(i).pub_type_id)
                bc_am_workflow.lstages.Add(bc_am_workflow.docs.document(i).stage_name)
                If (mode = 0 Or (mode = 1 And bc_am_workflow.docs.document(i).unread = True) Or (mode = 3 And bc_am_workflow.docs.document(i).stage_expire_date < Now) Or (mode = 2 And (Now.AddDays(bc_am_workflow.pre_expire_alert_notify) > bc_am_workflow.docs.document(i).stage_expire_date) And Now < bc_am_workflow.docs.document(i).stage_expire_date) Or (mode = 4 And bc_am_workflow.docs.document(i).stage_change_flag = True)) And ((bc_am_workflow.matches_only = True And bc_am_workflow.docs.document(i).search_flag = True) Or bc_am_workflow.matches_only = False) Then
                    exclude = False
                    If bc_am_workflow.spubtype_id.Count > 0 Then
                        If in_pub_type_filter(bc_am_workflow.docs.document(i).pub_type_id) = False Then

                            exclude = True
                        End If
                    End If
                    If bc_am_workflow.sentity_id.Count > 0 Then
                        If in_entity_filter(bc_am_workflow.docs.document(i).entity_id) = False Then

                            exclude = True
                        End If
                    End If
                    If bc_am_workflow.sstage.Count > 0 Then
                        If in_stage_filter(bc_am_workflow.docs.document(i).stage_name) = False Then

                            exclude = True
                        End If
                    End If
                    If bc_am_workflow.sauthor_id.Count > 0 Then
                        If in_author_filter(bc_am_workflow.docs.document(i).originating_author) = False Then

                            exclude = True
                        End If
                    End If

                    'If bc_am_workflow.fdatefrom <> "9-9-9999" And bc_am_workflow.docs.document(i).doc_date < bc_am_workflow.fdatefrom Then
                    'exclude = True
                    'End If
                    'If bc_am_workflow.fdateto <> "9-9-9999" And bc_am_workflow.docs.document(i).doc_date > bc_am_workflow.fdateto Then
                    'exclude = True
                    'End If


                    If exclude = False Then
                        ico = ico + 1
                        bc_am_workflow.sdocs.Add(bc_am_workflow.docs.document(i).id)

                        REM now load list view

                        Dim oitem As New ListViewItem(CStr(bc_am_workflow.docs.document(i).title))
                        REM PR


                        oitem.ImageIndex = 0
                        If bc_am_workflow.docs.document(i).checked_out_user <> "0" Then
                            If bc_am_workflow.docs.document(i).checked_out_user = bc_cs_central_settings.logged_on_user_id Then
                                oitem.ImageIndex = 1
                            Else
                                oitem.ImageIndex = 2
                            End If
                        End If
                        REM registered only
                        If bc_am_workflow.docs.document(i).extension = "" Then
                            oitem.ImageIndex = 3
                        End If

                        oitem.SubItems.Add(CStr(bc_am_workflow.docs.document(i).id))
                        oitem.SubItems.Add(CStr(bc_am_workflow.docs.document(i).doc_date))
                        Dim ptfound As Boolean = False
                        For j = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                            If bc_am_load_objects.obc_pub_types.pubtype(j).id = bc_am_workflow.docs.document(i).pub_type_id Then
                                oitem.SubItems.Add(bc_am_load_objects.obc_pub_types.pubtype(j).name)
                                ptfound = True
                                Exit For
                            End If
                        Next
                        If ptfound = False Then
                            oitem.SubItems.Add("unknown")
                        End If
                        oitem.SubItems.Add(CStr(bc_am_workflow.docs.document(i).stage_name))
                        REM originating author and checked out user
                        Dim fauthor As Boolean
                        fauthor = False
                        Dim fcheckedout As Boolean
                        fcheckedout = False
                        For j = 0 To bc_am_load_objects.obc_users.user.Count - 1
                            If bc_am_load_objects.obc_users.user(j).id = bc_am_workflow.docs.document(i).originating_author Then
                                oitem.SubItems.Add(bc_am_load_objects.obc_users.user(j).first_name + " " + bc_am_load_objects.obc_users.user(j).surname)
                                fauthor = True
                                Exit For
                            End If
                        Next
                        If fauthor = False Then
                            oitem.SubItems.Add("inactive")
                        End If
                        Dim cuser As Boolean
                        cuser = False

                        For j = 0 To bc_am_load_objects.obc_users.user.Count - 1
                            If bc_am_workflow.docs.document(i).checked_out_user = 0 Then
                                oitem.SubItems.Add("Checked In")
                                cuser = True
                                fcheckedout = True
                            ElseIf bc_am_load_objects.obc_users.user(j).id = bc_am_workflow.docs.document(i).checked_out_user Then
                                oitem.SubItems.Add(bc_am_load_objects.obc_users.user(j).first_name + " " + bc_am_load_objects.obc_users.user(j).surname)
                                cuser = True
                                fcheckedout = True
                            End If
                            If fcheckedout = True Then
                                Exit For
                            End If

                        Next

                        If cuser = False Then
                            oitem.SubItems.Add("unknown user")
                        End If

                        If fauthor = False Then
                            'oitem.SubItems.Add("none")
                            If fcheckedout = False Then
                                'oitem.SubItems.Add("unknown")
                            End If
                        End If
                        If bc_am_workflow.docs.document(i).entity_id = 0 Then
                            oitem.SubItems.Add("None")
                            efound = True
                        Else
                            For j = 0 To bc_am_load_objects.obc_entities.entity.Count - 1

                                If bc_am_workflow.docs.document(i).entity_id = bc_am_load_objects.obc_entities.entity(j).id Then

                                    oitem.SubItems.Add(bc_am_load_objects.obc_entities.entity(j).name)
                                    efound = True

                                    Exit For

                                End If

                            Next

                        End If
                        If efound = False Then
                            oitem.SubItems.Add("Inactive")
                        End If
                        efound = True
                        oitem.SubItems.Add(CStr(bc_am_workflow.docs.document(i).stage_expire_date))
                        oitem.SubItems.Add(bc_am_workflow.docs.document(i).bus_area)

                        Dim lang As String
                        Dim lfound As Boolean
                        lfound = False
                        For k = 0 To bc_am_load_objects.obc_pub_types.languages.Count - 1
                            If bc_am_load_objects.obc_pub_types.languages(k).id = bc_am_workflow.docs.document(i).language_id Then
                                lang = bc_am_load_objects.obc_pub_types.languages(k).name
                                lfound = True
                                oitem.SubItems.Add(lang)
                                Exit For
                            End If
                        Next
                        If lfound = False Then
                            oitem.SubItems.Add("")
                        End If

                        If bc_am_workflow.docs.document(i).unread = True Then
                            oitem.BackColor = bc_am_workflow.colors.doc_list_unread_backcolor
                            oitem.ForeColor = bc_am_workflow.colors.doc_list_unread_forecolor
                        End If


                        If bc_am_workflow.docs.document(i).urgent_flag = True Then
                            bc_am_workflow.docs.document(i).expire_flag = False
                            oitem.BackColor = bc_am_workflow.colors.doc_list_urgent_backcolor
                            oitem.ForeColor = bc_am_workflow.colors.doc_list_urgent_forecolor
                        End If
                        If bc_am_workflow.docs.document(i).expire_flag = True Then
                            bc_am_workflow.docs.document(i).urgent_flag = False
                            oitem.BackColor = bc_am_workflow.colors.doc_list_expired_backcolor
                            oitem.ForeColor = bc_am_workflow.colors.doc_list_expired_forecolor
                        End If
                        If bc_am_workflow.docs.document(i).stage_change_flag = True Then
                            oitem.BackColor = bc_am_workflow.colors.doc_list_stage_changed_backcolor
                            oitem.ForeColor = bc_am_workflow.colors.doc_list_stage_changed_forecolor
                        End If
                        If bc_am_workflow.docs.document(i).search_flag = True Then
                            oitem.BackColor = bc_am_workflow.colors.doc_list_search_backcolor
                            oitem.ForeColor = bc_am_workflow.colors.doc_list_search_forecolor
                        End If

                        If bc_am_workflow.unread_mode = False Then
                            dlist.Add(oitem)
                            'Me.DocumentList.Items.Add(oitem)
                        Else
                            If bc_am_workflow.docs.document(i).unread = True Then
                                dlist.Add(oitem)
                                'Me.DocumentList.Items.Add(oitem)
                            End If
                        End If
                    End If
                End If
            Next

            For i = 0 To dlist.Count - 1
                Me.DocumentList.Items.Add(dlist(i))
            Next
            For j = ico + 1 To Me.DocumentList.Items.Count - 1
                Me.DocumentList.Items(j).Text = ""
                Me.DocumentList.Items(j).SubItems(1).Text = ""
                Me.DocumentList.Items(j).SubItems(2).Text = ""
                Me.DocumentList.Items(j).SubItems(3).Text = ""
                Me.DocumentList.Items(j).SubItems(4).Text = ""
                Me.DocumentList.Items(j).SubItems(5).Text = ""
                Me.DocumentList.Items(j).SubItems(6).Text = ""
                Me.DocumentList.Items(j).SubItems(7).Text = ""
                Me.DocumentList.Items(j).SubItems(8).Text = ""
                Me.DocumentList.Items(j).SubItems(9).Text = ""
                Me.DocumentList.Items(j).SubItems(10).Text = ""
                Me.DocumentList.Items(j).ForeColor = Drawing.Color.Black
                Me.DocumentList.Items(j).BackColor = Drawing.Color.White
                Me.DocumentList.Items(j).ImageIndex = -1
            Next


            'Me.DocumentList.Items.Clear()
            'For i = 0 To dlist.Count - 1
            ' Me.DocumentList.Items.Add(dlist(i))
            'Next
            'Me.DocumentList.Refresh()

            REM hightlight previously selected document
            DocumentList.SelectedItems.Clear()
            If bc_am_workflow.selected_doc_id <> 0 Then

                If highlight_selected_document() = True Then

                    REM redisplay summary

                    For i = 0 To bc_am_workflow.docs.document.Count - 1

                        If bc_am_workflow.docs.document(i).id = bc_am_workflow.selected_doc_id Then
                            Dim oobc_Workflow As New bc_am_workflow
                            If bc_am_workflow.from_service = False Then
                                oobc_Workflow.load_doc(bc_am_workflow.docs.document(i))
                            End If
                            Exit For
                        End If

                    Next
                Else
                    bc_am_workflow.selected_doc_id = 0

                    REM blank summary

                    bc_am_workflow.cfrm.ofrmsummary.no_selection()

                End If

            End If
            REM load filters
            Dim owf As New bc_am_workflow
            bc_am_workflow.cfrm.new_load_filters()
            'Me.DocumentList.Visible = True
            bc_am_workflow.loading = False

        Catch ex As Exception
            Dim err As New bc_cs_error_log("bc_am_workflow_frm", "load_data", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            Me.DocumentList.EndUpdate()
            Me.DocumentList.OwnerDraw = False
            slog = New bc_cs_activity_log("bc_am_workflow_frm", "load_data", bc_cs_activity_codes.TRACE_EXIT, "")

        End Try

    End Function







    Private Sub TreeView1_MouseLeave1(ByVal sender As System.Object, ByVal e As System.EventArgs)

        bc_am_workflow.screen_update_enabled = True

        Dim owf As New bc_am_workflow

        owf.set_polling_status()



    End Sub



    Private Sub MenuExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Me.Close()

    End Sub



    Private Sub MenuItem3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)



    End Sub

    Private Sub Workflow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)



    End Sub

    REM New document

    Private Sub MenuItem4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        REM invoke Author Tool

    End Sub

    Private Sub DocumentList_Layout(ByVal sender As Object, ByVal e As System.Windows.Forms.LayoutEventArgs) Handles DocumentList.Layout

    End Sub


    Private Sub DocumentList_MouseLeave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DocumentList.MouseLeave

        If bc_am_workflow.snew = True Or bc_am_workflow.loading = True Then
            Exit Sub
        End If

        bc_am_workflow.screen_update_enabled = True
        Dim owf As New bc_am_workflow
        owf.set_polling_status()
        not_clear = False

    End Sub
    Private Sub DocumentList_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles DocumentList.MouseUp

        'If e.Button = MouseButtons.Right Then Exit Sub
        Try

            Dim oobc_workflow As New bc_am_workflow
            Me.Cursor = Cursors.WaitCursor
            Dim i, j As Integer
            For i = 0 To Me.DocumentList.Items.Count - 1

                If Me.DocumentList.Items(i).SubItems(1).Text = bc_am_workflow.selected_doc_id Then

                    DocumentList.Items(i).BackColor = System.Drawing.Color.White
                    For j = 0 To bc_am_workflow.docs.document.Count - 1
                        If bc_am_workflow.docs.document(j).id = bc_am_workflow.selected_doc_id Then
                            DocumentList.Items(i).BackColor = System.Drawing.Color.White
                            DocumentList.Items(i).ForeColor = System.Drawing.Color.Black
                            If bc_am_workflow.docs.document(j).urgent_flag = True Then
                                DocumentList.Items(i).BackColor = bc_am_workflow.colors.doc_list_urgent_backcolor
                                DocumentList.Items(i).ForeColor = bc_am_workflow.colors.doc_list_urgent_forecolor
                            End If
                            If bc_am_workflow.docs.document(j).expire_flag = True Then
                                DocumentList.Items(i).BackColor = bc_am_workflow.colors.doc_list_expired_backcolor
                                DocumentList.Items(i).ForeColor = bc_am_workflow.colors.doc_list_expired_forecolor
                            End If
                            If bc_am_workflow.docs.document(j).stage_change_flag = True Then
                                DocumentList.Items(i).BackColor = bc_am_workflow.colors.doc_list_stage_changed_backcolor
                                DocumentList.Items(i).ForeColor = bc_am_workflow.colors.doc_list_stage_changed_forecolor
                            End If
                            If bc_am_workflow.docs.document(j).search_flag = True Then
                                DocumentList.Items(i).BackColor = bc_am_workflow.colors.doc_list_search_backcolor
                                DocumentList.Items(i).ForeColor = bc_am_workflow.colors.doc_list_search_forecolor
                            End If


                            Exit For
                        End If
                    Next

                    Me.DocumentList.Refresh()

                    Exit For

                End If

            Next

            For i = 0 To bc_am_workflow.docs.document.Count - 1
                If CStr(bc_am_workflow.docs.document(i).id) = Me.DocumentList.SelectedItems(0).SubItems(1).Text Then

                    If bc_am_workflow.docs.document(i).unread = True Then
                        bc_am_workflow.docs.document(i).unread = False
                        bc_am_workflow.docs.document(i).acknowledged = Now
                    End If
                    If bc_am_workflow.docs.document(i).stage_change_flag = True Then
                        bc_am_workflow.docs.document(i).stage_change_flag = False
                        bc_am_workflow.docs.document(i).acknowledged = Now
                    End If
                    Dim tdoc As New bc_om_document
                    tdoc.id = Me.DocumentList.SelectedItems(0).SubItems(1).Text

                    tdoc.stage = bc_am_workflow.docs.document(i).stage
                    tdoc.checked_out_user = bc_am_workflow.docs.document(i).checked_out_user
                    tdoc.master_flag = True
                    tdoc.arrive = bc_am_workflow.docs.document(i).arrive
                    tdoc.acknowledged = bc_am_workflow.docs.document(i).acknowledged
                    'bc_am_load_objects.obc_current_document = tdoc
                    oobc_workflow.load_doc(tdoc)
                    bc_am_workflow.selected_doc_id = Me.DocumentList.SelectedItems(0).SubItems(1).Text
                    highlight_selected_document()

                    Exit For

                End If

            Next

        Catch ex As Exception

        Finally
            bc_am_workflow.from_service = False
            Me.Cursor = Cursors.Default

        End Try

    End Sub

    Public Function highlight_selected_document() As Boolean

        Dim i As Integer




        Dim found As Boolean = False
        REM deselect exisiting
        For i = 0 To Me.DocumentList.Items.Count - 1

            If Me.DocumentList.Items(i).SubItems(1).Text = bc_am_workflow.selected_doc_id Then

                DocumentList.Items(i).BackColor = bc_am_workflow.colors.doc_list_read_backcolor
                DocumentList.Items(i).ForeColor = bc_am_workflow.colors.doc_list_read_forecolor

                Me.DocumentList.Refresh()

                found = True

                Exit For

            End If

        Next

        highlight_selected_document = found

    End Function

    Private Sub DocumentList_ColumnClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles DocumentList.ColumnClick

        Dim slog = New bc_cs_activity_log("bc_am_workflow_frm", "get_entity_id_selected", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try

            no_column = False

            column = CInt(e.Column.ToString)

            Select Case CInt(e.Column.ToString)

                Case 0, 1, 3, 4, 5, 6, 7, 9, 10

                    REM title

                    DocumentList.ListViewItemSorter() = New CompareByname

                    If CompareByname.toggle = False Then

                        CompareByname.toggle = True
                        SortToggle = False

                    Else

                        CompareByname.toggle = False
                        SortToggle = True

                    End If

                Case 2, 8

                    REM date

                    DocumentList.ListViewItemSorter() = New CompareBydate

                    If CompareBydate.toggle = False Then

                        CompareBydate.toggle = True
                        SortToggle = False

                    Else

                        CompareBydate.toggle = False
                        SortToggle = True

                    End If

            End Select

        Catch ex As Exception

            Dim db_err As New bc_cs_error_log("bc_am_workflow_frm", "columnclick", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally

            slog = New bc_cs_activity_log("bc_am_workflow_frm", "columnclick", bc_cs_activity_codes.TRACE_EXIT, "")

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
                Return String.Compare(item1.SubItems(column).Text, item2.SubItems(column).Text)
            Else
                Return String.Compare(item2.SubItems(column).Text, item1.SubItems(column).Text)
            End If
        End Function

        Public Sub New()

        End Sub
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
                    Return Date.Compare(item1.SubItems(column).Text, item2.SubItems(column).Text)
                Else
                    Return Date.Compare(item2.SubItems(column).Text, item1.SubItems(column).Text)
                End If
            Catch

            End Try
        End Function
    End Class
    Private Sub bc_am_workflow_frm_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.SizeChanged


        Try

            bc_am_workflow.cfrm.ofrmsummary.Width = bc_am_workflow.cfrm.Width - Me.Width - 10
            Me.DocumentList.Width = Me.Width - 20
        Catch



        End Try

    End Sub

    Private Sub load_list()

        bc_am_workflow.user_inactive_last = Now()

        bc_am_workflow.screen_update_enabled = False

        Dim owf As New bc_am_workflow
        owf.set_polling_status()

        If bc_am_workflow.snew = True Or not_clear = True Then
            Exit Sub
        End If
        not_clear = True
        bc_am_workflow.screen_update_enabled = False

        bc_am_workflow.from_screen = True
        owf.retrieve_docs(False)

        apply_sort()

        'If no_column = False Then

        '    Select Case column



        '        Case 0, 1, 3, 4, 5, 6, 8, 9, 10

        '            REM title

        '            'If CompareByname.toggle = False Then

        '            '    CompareByname.toggle = True

        '            'Else

        '            '    CompareByname.toggle = False

        '            'End If
        '            CompareByname.toggle = SortToggle



        '            DocumentList.ListViewItemSorter() = New CompareByname



        '        Case 2, 7

        '            REM date

        '            'If CompareBydate.toggle = False Then

        '            '    CompareBydate.toggle = True

        '            'Else

        '            '    CompareBydate.toggle = False

        '            'End If
        '            CompareByname.toggle = SortToggle


        '            DocumentList.ListViewItemSorter() = New CompareBydate



        '    End Select

        'End If

        'no_column = True
    End Sub

    Public Sub apply_sort()

        If no_column = False Then

            Select Case column
                Case 0, 1, 3, 4, 5, 6, 7, 9, 10
                    CompareByname.toggle = SortToggle
                    DocumentList.ListViewItemSorter() = New CompareByname
                    CompareByname.toggle = Not SortToggle
                Case 2, 8
                    CompareBydate.toggle = SortToggle
                    DocumentList.ListViewItemSorter() = New CompareBydate
                    CompareBydate.toggle = Not SortToggle
            End Select

        End If

    End Sub



    Private Sub DocumentList_MouseHover(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DocumentList.MouseHover
        load_list()
    End Sub

    Private Sub bc_am_workflow_frm_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.MouseLeave
        not_clear = False
    End Sub


    Public last_move_date As Date

    Private Sub ContextMenu1_Popup(ByVal sender As Object, ByVal e As System.EventArgs) Handles ContextMenu1.Popup
        Try
            CheckInMenuItem.Visible = False
            CheckOutMenuItem.Visible = False
            CategoriseMenuItem.Visible = False
            ViewMenuItem.Visible = False
            'Determine state of currently selected document in the list
            Dim owf As New bc_am_workflow, selected_doc As bc_om_document
            If Not IsNumeric(Me.DocumentList.SelectedItems(0).SubItems(1).Text) Then
                Exit Sub
            End If
            selected_doc = owf.getDocForId(Me.DocumentList.SelectedItems(0).SubItems(1).Text)
            If bc_am_workflow.curr_doc.extension = "" Then
                CategoriseMenuItem.Visible = True
            Else

                If bc_am_workflow.curr_doc.read = True Then
                    ViewMenuItem.Visible = True
                End If
                If bc_am_workflow.curr_doc.write = True Then
                    If selected_doc.checked_out_user = 0 Then
                        CheckOutMenuItem.Visible = True
                        CategoriseMenuItem.Visible = True
                    End If
                    If bc_am_workflow.curr_doc.checked_out_user = bc_cs_central_settings.logged_on_user_id Then
                        CheckInMenuItem.Visible = True
                    End If
                End If
            End If
        Catch

        Finally

        End Try
    End Sub

    Private Sub CheckOutMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckOutMenuItem.Click
        Dim owf As New bc_am_workflow, selected_doc As bc_om_document
        selected_doc = owf.getDocForId(Me.DocumentList.SelectedItems(0).SubItems(1).Text)
        REM if imported document check out
        REM SW cope with office versions
        If Len(selected_doc.extension) > 5 Or (selected_doc.extension <> ".doc" And selected_doc.extension <> ".docx" And selected_doc.extension <> ".ppt" And selected_doc.extension <> ".pptx") Then
            owf.checkout_master_doc(selected_doc)
        Else
            REM author tool document
            owf.open_doc(selected_doc)
        End If
    End Sub

    Private Sub CheckInMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckInMenuItem.Click
        Dim owf As New bc_am_workflow, selected_doc As bc_om_document
        selected_doc = owf.getDocForId(Me.DocumentList.SelectedItems(0).SubItems(1).Text)
        owf.check_in(selected_doc.id, True, bc_am_workflow.cfrm.ofrmsummary.ldoc)
    End Sub

    Private Sub ViewMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ViewMenuItem.Click
        Dim t As New Thread(AddressOf do_view)
        t.Start()
    End Sub

    Private Sub CategoriseMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CategoriseMenuItem.Click
        Dim owf As New bc_am_workflow, selected_doc As bc_om_document

        selected_doc = owf.getDocForId(Me.DocumentList.SelectedItems(0).SubItems(1).Text)

        owf.edit_metadata(bc_am_workflow.curr_doc)

    End Sub

    Private Delegate Sub ShowFormDelegate()

    Private Sub do_view()
        Dim owf As New bc_am_workflow, selected_doc As bc_om_document

        REM steve wooderson 17/02/2011
        If Me.InvokeRequired Then

            Dim delegate1 As New ShowFormDelegate(AddressOf do_view)
            Me.Invoke(delegate1)

        Else
            selected_doc = owf.getDocForId(Me.DocumentList.SelectedItems(0).SubItems(1).Text)
            owf.look_at_support_doc(selected_doc.id, selected_doc, True)
        End If

    End Sub



    Private Sub DocumentList_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles DocumentList.KeyUp

        If e.KeyValue = 116 Then
            Dim rstate As Boolean
            rstate = bc_am_workflow.auto_refresh
            bc_am_workflow.auto_refresh = True
            Dim owf As New bc_am_workflow
            owf.retrieve_docs(False)
            bc_am_workflow.auto_refresh = rstate
        End If

    End Sub
    Private Sub DocumentList_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles DocumentList.MouseMove
        If bc_am_workflow.polling_process = False Then
            bc_am_workflow.user_inactive_last = Now()
            If bc_am_workflow.screen_update_enabled = True Then
                bc_am_workflow.screen_update_enabled = False
                Dim owf As New bc_am_workflow
                owf.set_polling_status()
            End If
        End If

    End Sub
End Class

