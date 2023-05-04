Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports BlueCurve.Core.AS
Imports System.Windows.Forms

#Region "changes"
'Changes:
'Tracker                 Initials                   Date                      Synopsis
'FIL 6835                PR                         8/1/2014                  Added default support doc
#End Region

Public Class bc_am_pubtype
    Public view As bc_am_cp_pub_type
    Public container As bc_am_cp_container
    Private maintain_mode As Integer
    Public user_attributes As New ArrayList
    Public current_pubtype As New bc_om_pub_type
    Public from_new As Boolean
    Public no_action As Boolean = False
    Public hold_vals As New ArrayList
    Public opt As bc_om_pub_type_workflow


    'icons
    Private Const add_icon As Integer = 0
    Private Const edit_icon As Integer = 1
    Private Const delete_icon As Integer = 2

    Private Const deactivate_icon As Integer = 6

    Private Const class_icon As Integer = 0
    Private Const selected_icon As Integer = 1
    Private Const unlinked_icon As Integer = 2
    Private Const data_icon As Integer = 3
    Private Const inactive_icon As Integer = 4

    Private Const child_parent_icon = 0
    Private Const parent_child_icon = 1
    Private Const links_icon = 2
    Private Const attributes_icon = 3
    Private Const att_edited_icon = 5
    Private Const publication_icon = 8
    Private Const mandatory_icon As Integer = 9
    Private Const number_icon As Integer = 13
    Private Const date_icon As Integer = 14
    Private Const string_icon As Integer = 15
    Private Const boolean_icon As Integer = 16
    Private Const warning_icon As Integer = 17
    Private Const selected_data_icon As Integer = 18
    Private Const users_icon As Integer = 19
    Private Const user_icon As Integer = 20
    Private Const parent_icon As Integer = 21
    Private Const child_icon As Integer = 22
    Private Const push_publish_icon As Integer = 24
    Private Const folder_icon As Integer = 25
    Private Const new_icon As Integer = 26
    Private Const active_icon As Integer = 27

  
    Public Sub New(ByVal container As bc_am_cp_container, Optional ByVal view As bc_am_cp_pub_type = Nothing)

        If Not view Is Nothing Then
            view.controller = Me
            Me.view = view
        End If

        If Not container Is Nothing Then
            Me.container = container
        End If

    End Sub
    Public Sub load_all()
        view.lpubtypes.ContextMenuStrip = Nothing
        view.tpubtypes.ContextMenuStrip = Nothing
        view.bentchanges.Visible = False
        view.bentdiscard.Visible = False


        'If Me.mode > 0 Then
        If Me.container.mode > bc_am_cp_container.VIEW Then
            view.Lpubtypes.ContextMenuStrip = view.PtContextMenuStrip
            view.tpubtypes.ContextMenuStrip = view.maintainitemscsm
            view.bentchanges.Visible = True
            view.bentdiscard.Visible = True
        End If
        If Me.container.mode > bc_am_cp_container.EDIT_FULL Then
            view.DeleteEntityToolStripMenuItem.Visible = True
        End If
        load_pub_types_tree()
        load_pubtype_attributes()
        size_nicely()
        view.uxdistribution.Visible = False

        If container.oPubtypes.process_switches.distribute = True Then
            view.uxdistribution.Visible = True
        End If
    End Sub
    Public Sub load_distribution()

        Dim fd As New bc_am_cp_distribution
        Dim cs As New Cbc_am_cp_distribution(fd, current_pubtype.id, current_pubtype.name)
        If cs.load_data() = True Then
            fd.ShowDialog()
        End If
    End Sub
    Public Sub load_pub_types()
        Try
            set_menu()
            clear_pub_type()
            'If view.mode > 0 Then
            view.minactive.Enabled = False
            view.mchname.Enabled = False

            view.DeleteEntityToolStripMenuItem.Enabled = False
            'End If
            If view.tpubtypes.SelectedNode.Text = "All Publication Types" Then
                view.BlueCurve_TextSearch1.SearchText = ""
                load_pub_types("", 0)
                view.tpubtypes.Nodes(0).Expand()
                Exit Sub
            End If
            If view.tpubtypes.SelectedNode.Text = "Active" Then
                view.BlueCurve_TextSearch1.SearchText = ""
                load_pub_types("", 10)
                Exit Sub
            End If
            If view.tpubtypes.SelectedNode.Text = "Inactive" Then
                view.BlueCurve_TextSearch1.SearchText = ""
                load_pub_types("", 11)
                Exit Sub
            End If
            Dim tx As String
            Try
                If view.tpubtypes.SelectedNode.Parent.Text = "Publication Types By Business Area" Then
                    view.BlueCurve_TextSearch1.SearchText = ""
                    tx = view.tpubtypes.SelectedNode.Text
                    view.mactive.Image = view.tabimages.Images(deactivate_icon)
                    If InStr(tx, "(inactive") > 0 Then
                        tx = tx.Substring(0, tx.Length - 11)
                        view.mactive.Image = view.tabimages.Images(active_icon)
                    End If
                    load_pub_types(tx, 1)
                    Exit Sub
                End If
                If view.tpubtypes.SelectedNode.Parent.Text = "Publication Types By Language" Then
                    view.BlueCurve_TextSearch1.SearchText = ""
                    tx = view.tpubtypes.SelectedNode.Text
                    view.mactive.Image = view.tabimages.Images(deactivate_icon)
                    If InStr(tx, "(inactive") > 0 Then
                        tx = tx.Substring(0, tx.Length - 11)
                        view.mactive.Image = view.tabimages.Images(active_icon)
                    End If
                    load_pub_types(tx, 2)
                End If
                If view.tpubtypes.SelectedNode.Parent.Text = "Publication Types By Category" Then
                    view.BlueCurve_TextSearch1.SearchText = ""
                    load_pub_types(view.tpubtypes.SelectedNode.Text, 3)
                End If
                If view.tpubtypes.SelectedNode.Parent.Text = "Publication Types By Type" Then
                    view.BlueCurve_TextSearch1.SearchText = ""
                    load_pub_types(view.tpubtypes.SelectedNode.Text, 4)
                End If
            Catch

            End Try
            Try

                tx = view.tpubtypes.SelectedNode.Parent.Text
            Catch
                If view.tpubtypes.SelectedNode.Index <> 0 Then
                    view.Lpubtypes.Items.Clear()
                    load_pubtype_toolbar()

                End If

            End Try
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_pubtype", "load_pub_types", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try


    End Sub
    Public Sub set_menu()

        If container.mode = 0 Then
            view.tpubtypes.ContextMenuStrip = Nothing
            Exit Sub
        End If
        load_pubtype_toolbar()

        view.tpubtypes.ContextMenuStrip = Nothing
        view.ChangeNameToolStripMenuItem.Visible = True
        view.mactive.Visible = False
        view.DeleteToolStripMenuItem.Visible = True

        If view.mode < 2 Then
            view.DeleteToolStripMenuItem.Visible = False
        End If


        If view.tpubtypes.SelectedNode.Text = "Publication Types By Business Area" Then
            maintain_mode = 2
            view.ChangeNameToolStripMenuItem.Visible = False
            view.mactive.Visible = False

            view.DeleteToolStripMenuItem.Visible = False
            view.tpubtypes.ContextMenuStrip = view.maintainitemscsm
            Exit Sub
        End If
        If view.tpubtypes.SelectedNode.Text = "Publication Types By Language" Then
            maintain_mode = 3
            view.ChangeNameToolStripMenuItem.Visible = False
            view.mactive.Visible = False

            view.DeleteToolStripMenuItem.Visible = False
            view.tpubtypes.ContextMenuStrip = view.maintainitemscsm
            Exit Sub
        End If
        Try
            view.DeleteToolStripMenuItem.Visible = False
            If Me.container.mode = bc_am_cp_container.EDIT_FULL Then

                view.DeleteToolStripMenuItem.Visible = True
            End If
            If view.tpubtypes.SelectedNode.Parent.Text = "Publication Types By Business Area" Then
                maintain_mode = 2
                view.ChangeNameToolStripMenuItem.Text = "Change name of " + view.tpubtypes.SelectedNode.Text
                If InStr(view.tpubtypes.SelectedNode.Text, "(inactive)") > 0 Then
                    view.mactive.Text = "Make " + view.tpubtypes.SelectedNode.Text.Substring(0, view.tpubtypes.SelectedNode.Text.Length - 11) + " active"
                    view.DeleteToolStripMenuItem.Enabled = True
                    view.ChangeNameToolStripMenuItem.Enabled = False
                Else
                    view.mactive.Text = "Make " + view.tpubtypes.SelectedNode.Text + " inactive"
                    view.DeleteToolStripMenuItem.Enabled = False
                    view.ChangeNameToolStripMenuItem.Enabled = True
                End If
                view.mactive.Visible = True
                view.DeleteToolStripMenuItem.Text = "Delete " + view.tpubtypes.SelectedNode.Text
                view.tpubtypes.ContextMenuStrip = view.maintainitemscsm
                Exit Sub
            End If
            If view.tpubtypes.SelectedNode.Parent.Text = "Publication Types By Language" Then
                maintain_mode = 3
                view.ChangeNameToolStripMenuItem.Text = "Change name of " + view.tpubtypes.SelectedNode.Text
                If InStr(view.tpubtypes.SelectedNode.Text, "(inactive)") > 0 Then
                    view.mactive.Text = "Make " + view.tpubtypes.SelectedNode.Text.Substring(0, view.tpubtypes.SelectedNode.Text.Length - 11) + " active"
                    view.DeleteToolStripMenuItem.Enabled = True
                    view.ChangeNameToolStripMenuItem.Enabled = False
                Else
                    view.mactive.Text = "Make " + view.tpubtypes.SelectedNode.Text + " inactive"
                    view.DeleteToolStripMenuItem.Enabled = False
                    view.ChangeNameToolStripMenuItem.Enabled = True
                End If
                view.mactive.Visible = True
                view.DeleteToolStripMenuItem.Text = "Delete " + view.tpubtypes.SelectedNode.Text
                view.tpubtypes.ContextMenuStrip = view.maintainitemscsm
                Exit Sub
            End If
        Catch

        End Try

    End Sub
    Private Sub load_pub_types(ByVal tx As String, Optional ByVal mode As Integer = 0, Optional ByVal filter As String = "")
        Try
            view.Lpubtypes.Items.Clear()
            Dim found As Boolean = False
            view.Lpubtypes.Sorting = SortOrder.None
            For i = 0 To container.oPubtypes.pubtype.Count - 1
                Dim na As String
                na = container.oPubtypes.pubtype(i).name
                found = False
                If filter <> "" Then
                    If view.BlueCurve_TextSearch1.SearchText.Length <= na.Length Then
                        If InStr(UCase(na), UCase(view.BlueCurve_TextSearch1.SearchText)) > 0 Then
                            found = True
                        End If
                    End If
                Else

                    found = True
                End If

                If found = True Then

                    Select Case mode
                        Case 0
                            If container.oPubtypes.pubtype(i).inactive = False Then
                                view.Lpubtypes.Items.Add(container.oPubtypes.pubtype(i).name)
                                view.Lpubtypes.Items(view.Lpubtypes.Items.Count - 1).ImageIndex = publication_icon
                            Else
                                view.Lpubtypes.Items.Add(container.oPubtypes.pubtype(i).name + " (inactive)")
                                view.Lpubtypes.Items(view.Lpubtypes.Items.Count - 1).ImageIndex = deactivate_icon
                            End If
                        Case 10
                            If container.oPubtypes.pubtype(i).inactive = False Then
                                view.Lpubtypes.Items.Add(container.oPubtypes.pubtype(i).name)
                                view.Lpubtypes.Items(view.Lpubtypes.Items.Count - 1).ImageIndex = publication_icon

                            End If
                        Case 11
                            If container.oPubtypes.pubtype(i).inactive = True Then
                                view.Lpubtypes.Items.Add(container.oPubtypes.pubtype(i).name + " (inactive)")
                                view.Lpubtypes.Items(view.Lpubtypes.Items.Count - 1).ImageIndex = deactivate_icon

                            End If

                        Case 1
                            Dim ba_id As Long
                            For j = 0 To container.oAdmin.business_areas.Count - 1
                                If container.oAdmin.business_areas(j).description = tx Then
                                    ba_id = container.oAdmin.business_areas(j).id
                                    Exit For
                                End If
                            Next
                            If container.oPubtypes.pubtype(i).bus_area_id = ba_id Then
                                If container.oPubtypes.pubtype(i).inactive = False Then
                                    view.Lpubtypes.Items.Add(container.oPubtypes.pubtype(i).name)
                                    view.Lpubtypes.Items(view.Lpubtypes.Items.Count - 1).ImageIndex = publication_icon

                                Else
                                    view.Lpubtypes.Items.Add(container.oPubtypes.pubtype(i).name + " (inactive)")
                                    view.Lpubtypes.Items(view.Lpubtypes.Items.Count - 1).ImageIndex = deactivate_icon

                                End If
                            End If
                        Case 2
                            Dim l_id As Long
                            For j = 0 To container.oAdmin.languages.Count - 1
                                If container.oAdmin.languages(j).language_name = tx Then
                                    l_id = container.oAdmin.languages(j).language_id
                                    Exit For
                                End If
                            Next
                            If container.oPubtypes.pubtype(i).language = l_id Then
                                If container.oPubtypes.pubtype(i).inactive = False Then
                                    view.Lpubtypes.Items.Add(container.oPubtypes.pubtype(i).name)
                                    view.Lpubtypes.Items(view.Lpubtypes.Items.Count - 1).ImageIndex = publication_icon

                                Else
                                    view.Lpubtypes.Items.Add(container.oPubtypes.pubtype(i).name + " (inactive)")
                                    view.Lpubtypes.Items(view.Lpubtypes.Items.Count - 1).ImageIndex = deactivate_icon

                                End If
                            End If
                        Case 3
                            Dim ct_id As Long
                            If tx = "None" Then
                                ct_id = 0
                            Else
                                For j = 0 To container.oClasses.classes.Count - 1
                                    If container.oClasses.classes(j).class_name = tx Then
                                        ct_id = container.oClasses.classes(j).class_id
                                        Exit For
                                    End If
                                Next
                            End If


                            If container.oPubtypes.pubtype(i).child_category = ct_id Then
                                If container.oPubtypes.pubtype(i).inactive = False Then
                                    view.Lpubtypes.Items.Add(container.oPubtypes.pubtype(i).name)
                                    view.Lpubtypes.Items(view.Lpubtypes.Items.Count - 1).ImageIndex = publication_icon

                                Else
                                    view.Lpubtypes.Items.Add(container.oPubtypes.pubtype(i).name + " (inactive)")
                                    view.Lpubtypes.Items(view.Lpubtypes.Items.Count - 1).ImageIndex = deactivate_icon

                                End If
                            End If
                        Case 4
                           
                            Dim support_doc_only As Boolean
                            support_doc_only = True
                            If tx = "Master" Then
                                support_doc_only = False
                            End If
                            If container.oPubtypes.pubtype(i).support_doc_only = support_doc_only Then
                                If container.oPubtypes.pubtype(i).inactive = False Then
                                    view.Lpubtypes.Items.Add(container.oPubtypes.pubtype(i).name)
                                    view.Lpubtypes.Items(view.Lpubtypes.Items.Count - 1).ImageIndex = publication_icon

                                Else
                                    view.Lpubtypes.Items.Add(container.oPubtypes.pubtype(i).name + " (inactive)")
                                    view.Lpubtypes.Items(view.Lpubtypes.Items.Count - 1).ImageIndex = deactivate_icon

                                End If
                            End If
                    End Select

                End If
            Next
            view.Lpubtypes.Sorting = SortOrder.Ascending


            view.ppubtype.Enabled = False
            load_pubtype_toolbar()
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_pubtype", "load_pub_types", bc_cs_message.ERR, bc_cs_error_codes.USER_DEFINED)


        End Try

    End Sub
    Public Sub pt_mode(ByVal filter As String)
        Dim tx As String

        If view.tpubtypes.SelectedNode.Text = "All Publication Types" Then
            load_pub_types(view.tpubtypes.SelectedNode.Text, 0, filter)
        ElseIf view.tpubtypes.SelectedNode.Text = "Active" Then
            load_pub_types(view.tpubtypes.SelectedNode.Text, 10, filter)
        ElseIf view.tpubtypes.SelectedNode.Text = "Inactive" Then
            load_pub_types(view.tpubtypes.SelectedNode.Text, 11, filter)
        Else
            Try
                tx = view.tpubtypes.SelectedNode.Parent.Text
                If tx = "Publication Types By Business Area" Then
                    load_pub_types(view.tpubtypes.SelectedNode.Text, 1, filter)
                End If
                If tx = "Publication Types By Language" Then
                    load_pub_types(view.tpubtypes.SelectedNode.Text, 2, filter)
                End If
                If tx = "Publication Types By Category" Then
                    load_pub_types(view.tpubtypes.SelectedNode.Text, 3, filter)
                End If

            Catch ex As Exception
                view.lpubtypes.Items.Clear()
            End Try
        End If

    End Sub
    Public Sub discard_changes()
        Dim lpt As String
        Dim omsg As New bc_cs_message("Blue Curve", "Are you sure you wish to discard changes to " + view.Lpubtypes.SelectedItems(0).Text, bc_cs_message.MESSAGE, True, False, "Yes", "no", True)
        If omsg.cancel_selected = True Then
            Exit Sub
        End If
        REM remove any added languages or business areas
        If container.oAdmin.languages(container.oAdmin.languages.Count - 1).language_id = -1 Then
            container.oAdmin.languages.RemoveAt(container.oAdmin.languages.Count - 1)
        End If
        If container.oAdmin.business_areas(container.oAdmin.business_areas.Count - 1).id = -1 Then
            container.oAdmin.business_areas.RemoveAt(container.oAdmin.business_areas.Count - 1)
        End If

        lpt = view.Lpubtypes.SelectedItems(0).Text
        If from_new = True Then
            load_pub_types_tree()
            load_pub_types("All Publication Types", 0, "")
            view.tpubtypename.TabPages(0).Text = "no selection"
            view.tpubtypename.TabPages(0).ImageIndex = deactivate_icon
            view.DataGridView1.Rows.Clear()
            size_nicely()
        End If
        exit_edit_mode()
        For i = 0 To view.Lpubtypes.Items.Count - 1
            If view.Lpubtypes.Items(i).Text = lpt Then
                view.Lpubtypes.Items(i).Selected = False
                view.Lpubtypes.Items(i).Selected = True
                Exit For
            End If
        Next
        container.uxNavBar.Enabled = True
        container.uncomitted_data = False

    End Sub
    Public Sub clear_pub_type()
        For i = 0 To view.DataGridView1.Rows.Count - 1
            view.DataGridView1.Item(2, i).Value = ""
        Next
        view.tpubtypename.TabPages(0).ImageIndex = deactivate_icon
        view.tpubtypename.TabPages(0).Text = "no selection"
        view.ppubtype.Enabled = False
        view.DataGridView1.Enabled = False
        view.mchname.Enabled = False
        view.DeleteEntityToolStripMenuItem.Enabled = False
        view.minactive.Enabled = False
        view.workflow.Nodes.Clear()

    End Sub
    Private Sub load_pub_types_tree()
        view.tpubtypes.Nodes.Clear()
        view.tpubtypes.ImageList = view.tabimages
        view.tpubtypes.Nodes.Add("All Publication Types", "All Publication Types", publication_icon)
        view.tpubtypes.Nodes(0).Nodes.Add("Active", "Active", folder_icon)
        view.tpubtypes.Nodes(0).Nodes.Add("Inactive", "Inactive", folder_icon)
        view.tpubtypes.Nodes.Add("Publication Types By Business Area", "Publication Types By Business Area", publication_icon)
        For i = 0 To container.oAdmin.business_areas.Count - 1
            If container.oAdmin.business_areas(i).inactive = False Then
                view.tpubtypes.Nodes(1).Nodes.Add(container.oAdmin.business_areas(i).description, container.oAdmin.business_areas(i).description, folder_icon)
            Else
                view.tpubtypes.Nodes(1).Nodes.Add(container.oAdmin.business_areas(i).description + " (inactive)", container.oAdmin.business_areas(i).description + " (inactive)", deactivate_icon)
            End If
        Next
        view.tpubtypes.Nodes.Add("Publication Types By Language", "Publication Types By Language", publication_icon)
        For i = 0 To container.oAdmin.languages.Count - 1
            If container.oAdmin.languages(i).inactive = False Then
                view.tpubtypes.Nodes(2).Nodes.Add(container.oAdmin.languages(i).language_name, container.oAdmin.languages(i).language_name, folder_icon)
            Else
                view.tpubtypes.Nodes(2).Nodes.Add(container.oAdmin.languages(i).language_name + " (inactive)", container.oAdmin.languages(i).language_name + " (inactive)", deactivate_icon)
            End If
        Next
        view.tpubtypes.Nodes.Add("Publication Types By Category", "Publication Types By Category", publication_icon)
        view.tpubtypes.Nodes(3).Nodes.Add("None", "None", folder_icon)
        For i = 0 To container.oClasses.classes.Count - 1
            For j = 0 To container.oPubtypes.pubtype.Count - 1
                If container.oPubtypes.pubtype(j).child_category = container.oClasses.classes(i).class_id Then
                    view.tpubtypes.Nodes(3).Nodes.Add(container.oClasses.classes(i).class_name, container.oClasses.classes(i).class_name, folder_icon)
                    Exit For
                End If
            Next
        Next

        view.tpubtypes.Nodes.Add("Publication Types By Type", "Publication Types By Type", publication_icon)
        view.tpubtypes.Nodes(4).Nodes.Add("Master", "Master", folder_icon)
        view.tpubtypes.Nodes(4).Nodes.Add("Support Only", "Support Only", folder_icon)
        
        view.tpubtypes.Nodes(0).Expand()
        view.tpubtypes.SelectedNode = view.tpubtypes.Nodes(0)
        load_pubtype_toolbar()
    End Sub
    Public Sub add_new_language_menu()
        view.maintain_mode = 3
        Dim ofrm As New bc_am_cp_edit
        ofrm.ltitle.Text = "Add new language and assign to " + view.Lpubtypes.SelectedItems(0).Text
        ofrm.ShowDialog()
        If ofrm.cancel_selected = True Then
            Exit Sub
        End If
        If container.oAdmin.languages(container.oAdmin.languages.Count - 1).language_id = -1 Then
            container.oAdmin.languages.RemoveAt(container.oAdmin.languages.Count - 1)
        End If

        add_new(ofrm.Tentry.Text)
    End Sub
    Public Sub add_new_busarea_menu()
        view.maintain_mode = 2
        Dim ofrm As New bc_am_cp_edit
        ofrm.ltitle.Text = "Add new business area and assign to " + view.Lpubtypes.SelectedItems(0).Text
        ofrm.ShowDialog()
        If ofrm.cancel_selected = True Then
            Exit Sub
        End If
        If container.oAdmin.business_areas(container.oAdmin.business_areas.Count - 1).id = -1 Then
            container.oAdmin.business_areas.RemoveAt(container.oAdmin.business_areas.Count - 1)
        End If

        add_new(ofrm.Tentry.Text)
    End Sub
    Public Sub add_new(ByVal item As String)
        Try
            Dim omsg As bc_cs_message
            REM check item doesnt already exist
            Select Case view.maintain_mode
                Case 2
                    For i = 0 To container.oAdmin.business_areas.Count - 1
                        If Trim(UCase(container.oAdmin.business_areas(i).description)) = Trim(UCase(item)) Then
                            omsg = New bc_cs_message("Blue Curve", "Business Area " + item + " already exists!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                            Exit Sub
                        End If
                    Next
                    REM set in attributes
                    Dim obus_area As New bc_om_bus_area
                    obus_area.description = item
                    obus_area.id = -1
                    container.oAdmin.business_areas.Add(obus_area)
                    view.user_attributes_values(view.DataGridView1.SelectedCells(0).RowIndex).value = item
                    user_attributes(0).lookup_values.add(item)
                    Dim dtextbox As New DataGridViewTextBoxColumn
                    dtextbox.Width = view.DataGridView1.Columns(2).Width
                    dtextbox.MaxInputLength = 250
                    view.DataGridView1.Columns.RemoveAt(2)
                    dtextbox.Name = "Values"
                    view.DataGridView1.Columns.Insert(2, dtextbox)
                    For i = 0 To view.user_attributes_values.Count - 1
                        view.DataGridView1.Item(2, i).Value = view.user_attributes_values(i).value
                    Next
                    Me.edit_mode()
                Case 3
                    For i = 0 To container.oAdmin.languages.Count - 1
                        If Trim(UCase(container.oAdmin.languages(i).language_name)) = Trim(UCase(item)) Then
                            omsg = New bc_cs_message("Blue Curve", "Language " + item + " already exists!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                            Exit Sub
                        End If
                    Next
                    REM set in attributes
                    Dim olang As New bc_om_user_language
                    olang.language_name = item
                    olang.language_id = -1
                    container.oAdmin.languages.Add(olang)
                    view.user_attributes_values(view.DataGridView1.SelectedCells(0).RowIndex).value = item

                    user_attributes(1).lookup_values.add(item)
                    Dim dtextbox As New DataGridViewTextBoxColumn
                    dtextbox.Width = view.DataGridView1.Columns(2).Width
                    dtextbox.MaxInputLength = 250
                    view.DataGridView1.Columns.RemoveAt(2)
                    dtextbox.Name = "Values"
                    view.DataGridView1.Columns.Insert(2, dtextbox)
                    For i = 0 To view.user_attributes_values.Count - 1
                        view.DataGridView1.Item(2, i).Value = view.user_attributes_values(i).value
                    Next

                    Me.edit_mode()
            End Select

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("OkToolStripMenuItem2", "Click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

        End Try
    End Sub
    Private Sub load_pubtype_attributes()
        Try
            user_attributes.Clear()
            Dim oatt As bc_om_attribute
            oatt = New bc_om_attribute
            oatt.type_id = 1
            oatt.nullable = False
            oatt.is_lookup = True
            oatt.name = "Business Area"
            For i = 0 To container.oAdmin.business_areas.Count - 1
                If container.oAdmin.business_areas(i).inactive = False Then
                    oatt.lookup_values.Add(container.oAdmin.business_areas(i).description)
                End If
            Next
            user_attributes.Add(oatt)
            oatt = New bc_om_attribute
            oatt = New bc_om_attribute
            oatt.type_id = 1
            oatt.nullable = False
            oatt.is_lookup = True
            oatt.name = "Language"
            For i = 0 To container.oAdmin.languages.Count - 1
                If container.oAdmin.languages(i).inactive = False Then
                    oatt.lookup_values.Add(container.oAdmin.languages(i).language_name)
                End If
            Next
            user_attributes.Add(oatt)
            oatt = New bc_om_attribute
            oatt.type_id = 1
            oatt.nullable = False
            oatt.is_lookup = True
            oatt.name = "Category"
            oatt.lookup_values.Add("None")
            For i = 0 To container.oClasses.classes.Count - 1
                oatt.lookup_values.Add(container.oClasses.classes(i).Class_name)
            Next
            user_attributes.Add(oatt)
            oatt = New bc_om_attribute
            oatt = New bc_om_attribute
            oatt.type_id = 1
            oatt.nullable = True
            oatt.name = "Description"
            user_attributes.Add(oatt)


            oatt = New bc_om_attribute
            oatt.type_id = 3
            oatt.nullable = False
            ' oatt.is_lookup = True
            oatt.name = "Support Only"
           
            user_attributes.Add(oatt)


            REM FIL JIRA 6835
            oatt = New bc_om_attribute
            oatt.type_id = 1
            oatt.nullable = True
            oatt.is_lookup = True
            oatt.name = "Default Support Type"
            For i = 0 To container.oPubtypes.pubtype.Count - 1
                If container.oPubtypes.pubtype(i).support_doc_only = True Then
                    oatt.lookup_values.Add(container.oPubtypes.pubtype(i).name)
                End If
            Next
            user_attributes.Add(oatt)
            REM END JIRA
            REM FIL 5.3
            oatt = New bc_om_attribute
            oatt.type_id = 3
            oatt.nullable = True
            ' oatt.is_lookup = True
            oatt.name = "Warn Default"

            user_attributes.Add(oatt)

            oatt = New bc_om_attribute
            oatt.type_id = 1
            oatt.nullable = True
            oatt.name = "Folder"
            user_attributes.Add(oatt)

            REM ===

            view.DataGridView1.Rows.Clear()
            For i = 0 To user_attributes.Count - 1
                view.DataGridView1.Rows.Add()
                view.DataGridView1.Item(1, i).Value = user_attributes(i).name
                view.DataGridView1.Item(3, i).Value = New Drawing.Bitmap(1, 1)
                If user_attributes(i).nullable = False Then
                    view.DataGridView1.Item(0, i).Value = view.tabimages.Images(mandatory_icon)
                Else
                    view.DataGridView1.Item(0, i).Value = New Drawing.Bitmap(1, 1)
                End If
                Select Case i
                    Case 0, 1
                        view.DataGridView1.Item(3, i).Value = view.tabimages.Images(new_icon)
                End Select
            Next
            Me.size_nicely()
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_pubtype", "load_pubtype_attributes", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

        End Try
    End Sub
    Public Sub add_pub_type_menu()
        Dim ofrm As New bc_am_cp_edit
        ofrm.ltitle.Text = "Enter pub type name"
        ofrm.ShowDialog()
        If ofrm.cancel_selected = True Then
            Exit Sub
        End If
        add_pub_type(ofrm.Tentry.Text)
    End Sub
    Public Sub change_pub_type_menu()
        Dim ofrm As New bc_am_cp_edit
        ofrm.ltitle.Text = "Change name of " + view.Lpubtypes.SelectedItems(0).Text
        ofrm.ShowDialog()
        If ofrm.cancel_selected = True Then
            Exit Sub
        End If
        from_new = False
        save_changes(ofrm.Tentry.Text)
    End Sub
    Public Sub change_item_menu()
        Dim ofrm As New bc_am_cp_edit
        ofrm.ltitle.Text = "Change name of " + view.tpubtypes.SelectedNode.Text
        ofrm.ShowDialog()
        If ofrm.cancel_selected = True Then
            Exit Sub
        End If
        save_item_changes(ofrm.Tentry.Text)
    End Sub
    Public Sub delete_item_menu()
        Dim item As String
        item = view.tpubtypes.SelectedNode.Text
        item = Left(item, item.Length - 10)
        item = Trim(item)
        Dim omsg As New bc_cs_message("Blue Curve", "Are you sure you wish to delete " + item + "?", bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
        If omsg.cancel_selected = True Then
            Exit Sub
        End If
        delete_item(item)
    End Sub

    Public Sub reset_warnings()

        view.tpubtypename.TabPages(0).ImageIndex = -1

        For i = 0 To Me.user_attributes.Count - 1

            If Me.user_attributes(i).nullable = False Then
                view.DataGridView1.Item(0, i).Value = view.tabimages.Images(mandatory_icon)
            End If
            If view.user_attributes_values.Count > 0 Then
                If view.user_attributes_values(i).value_changed = True Then
                    view.DataGridView1.Item(0, i).Value = view.tabimages.Images(att_edited_icon)
                End If
            End If
        Next

    End Sub
    Private Function check_warnings() As Boolean
        REM check for mandatory fields
        reset_warnings()
        check_warnings = True
        view.warnings.Clear()

        For j = 0 To user_attributes.Count - 1
            If user_attributes(j).nullable = False Then
                If view.user_attributes_values(j).value = "" Then
                    Dim owarning As New warning
                    owarning.msg = view.tpubtypename.TabPages(0).Text + " requires : " + user_attributes(j).name + " to be entered"
                    owarning.attribute = j
                    owarning.attribute_tab = 0
                    owarning.entity_tab = -1
                    view.warnings.Add(owarning)
                    check_warnings = False
                End If
            End If
            If InStr(view.user_attributes_values(j).value, "(inactive)") > 0 Then
                Dim owarning As New warning
                owarning.msg = view.tpubtypename.TabPages(0).Text + " has inactive  " + user_attributes(j).name + " change to active one"
                owarning.attribute = j
                owarning.attribute_tab = 0
                owarning.entity_tab = -1
                view.warnings.Add(owarning)
                check_warnings = False
            End If
        Next

        view.load_warnings()


    End Function
    Private Sub load_pub_type()
        Try
            view.bentchanges.Enabled = False
            view.bentdiscard.Enabled = False
            current_pubtype = New bc_om_pub_type
            view.tpubtypename.TabPages(0).Text = view.Lpubtypes.SelectedItems(0).Text
            view.tpubtypename.ImageList = view.tabimages

            view.Cursor = Cursors.WaitCursor


            For i = 0 To container.oPubtypes.pubtype.Count - 1
                If InStr(view.Lpubtypes.SelectedItems(0).Text, "(inactive)") = 0 Then
                    view.tpubtypename.TabPages(0).ImageIndex = publication_icon
                    If container.oPubtypes.pubtype(i).name = view.Lpubtypes.SelectedItems(0).Text Then
                        current_pubtype = container.oPubtypes.pubtype(i)
                        Exit For
                    End If
                Else
                    view.tpubtypename.TabPages(0).ImageIndex = deactivate_icon
                    If container.oPubtypes.pubtype(i).name = view.Lpubtypes.SelectedItems(0).Text.Substring(0, view.Lpubtypes.SelectedItems(0).Text.Length - 11) Then
                        current_pubtype = container.oPubtypes.pubtype(i)
                        Exit For
                    End If
                End If

            Next
            load_pubtype_attributes()
            load_attribute_values()
            load_pubtype_toolbar()
            view.minactive.Image = view.tabimages.Images(deactivate_icon)
            If container.mode > 0 Then
                view.DataGridView1.Columns(3).Visible = True
            End If
            If InStr(view.Lpubtypes.SelectedItems(0).Text, "(inactive)") > 0 Then
                view.DataGridView1.Item(0, 1).Selected = True
                view.minactive.Image = view.tabimages.Images(active_icon)
                view.DataGridView1.Columns(3).Visible = False
            End If
            opt = New bc_om_pub_type_workflow
            opt.id = current_pubtype.id
            opt.bwithactions_and_approvers = True
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                opt.db_read()
            Else
                opt.tmode = bc_cs_soap_base_class.tREAD
                opt.transmit_to_server_and_receive(opt, True)
            End If

            view.load_all()
            size_nicely()
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_pubtype", "load_pub_type", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            view.Cursor = Cursors.Default
        End Try


    End Sub
    Private Sub load_attribute_values()
        view.user_attributes_values.Clear()
        REM bus area
        Dim oatt_val As New bc_om_attribute_value
        For i = 0 To container.oAdmin.business_areas.Count - 1
            If container.oAdmin.business_areas(i).id = current_pubtype.bus_area_id Then
                If container.oAdmin.business_areas(i).inactive = False Then
                    oatt_val.value = container.oAdmin.business_areas(i).description
                Else
                    oatt_val.value = container.oAdmin.business_areas(i).description + " (inactive)"
                End If
                Exit For
            End If

        Next
        view.user_attributes_values.Add(oatt_val)
        oatt_val = New bc_om_attribute_value
        For i = 0 To container.oAdmin.languages.Count - 1
            If container.oAdmin.languages(i).language_id = current_pubtype.language Then
                If container.oAdmin.languages(i).inactive = False Then
                    oatt_val.value = container.oAdmin.languages(i).language_name
                Else
                    oatt_val.value = container.oAdmin.languages(i).language_name + " (inactive)"
                End If
                Exit For
            End If
        Next
        view.user_attributes_values.Add(oatt_val)
        oatt_val = New bc_om_attribute_value
        If current_pubtype.child_category = 0 Then
            oatt_val.value = "None"
        Else
            For i = 0 To container.oClasses.classes.Count - 1
                If container.oClasses.classes(i).class_id = current_pubtype.child_category Then
                    oatt_val.value = container.oClasses.classes(i).class_name
                    Exit For
                End If
            Next
        End If
        view.user_attributes_values.Add(oatt_val)

        oatt_val = New bc_om_attribute_value
        oatt_val.value = current_pubtype.description
        view.user_attributes_values.Add(oatt_val)

      
        oatt_val = New bc_om_attribute_value
        If current_pubtype.support_doc_only = False Then
            oatt_val.value = "False"
        Else
            oatt_val.value = "True"
        End If
        view.user_attributes_values.Add(oatt_val)

        REM JIRA 8306 
        oatt_val = New bc_om_attribute_value
        For i = 0 To container.oPubtypes.pubtype.Count - 1
            If current_pubtype.default_support_pub_type = container.oPubtypes.pubtype(i).id Then
                oatt_val.value = container.oPubtypes.pubtype(i).name
                Exit For
            End If
        Next
        view.user_attributes_values.Add(oatt_val)
        REM END JIRA
        REM FIL 5.3
        oatt_val = New bc_om_attribute_value
        If current_pubtype.mandatory_default_support_doc = False Then
            oatt_val.value = "False"
        Else
            oatt_val.value = "True"
        End If
        view.user_attributes_values.Add(oatt_val)
        REM 
        oatt_val = New bc_om_attribute_value
        oatt_val.value = current_pubtype.folder_name
        view.user_attributes_values.Add(oatt_val)

        Dim dtextbox As New DataGridViewTextBoxColumn
        dtextbox.Width = view.DataGridView1.Columns(2).Width
        dtextbox.MaxInputLength = 250
        view.DataGridView1.Columns.RemoveAt(2)
        dtextbox.Name = "Values"
        view.DataGridView1.Columns.Insert(2, dtextbox)
        For i = 0 To view.user_attributes_values.Count - 1
            view.DataGridView1.Item(2, i).Value = view.user_attributes_values(i).value
        Next

    End Sub
    Public Sub select_pubtype()
        view.ppubtype.Visible = True
        from_new = False
        load_pub_type()
        'If view.mode > 0 Then
        view.minactive.Visible = True
        If current_pubtype.inactive = False Then
            view.minactive.Text = "Make " + current_pubtype.name + " inactive"
            view.mchname.Enabled = True
            view.mchname.Text = "Change Name of " + current_pubtype.name
            view.ppubtype.Enabled = True
            view.DataGridView1.Enabled = True
        Else
            view.minactive.Text = "Make " + current_pubtype.name + " active"
            view.mchname.Enabled = False
            view.mchname.Text = "Change Name of publication type"
            view.ppubtype.Enabled = False
            view.DataGridView1.Enabled = False
        End If
        view.minactive.Enabled = True
        view.DeleteEntityToolStripMenuItem.Visible = False
        If container.mode = 2 Then
            view.DeleteEntityToolStripMenuItem.Visible = True
            If current_pubtype.inactive = False Then
                view.DeleteEntityToolStripMenuItem.Enabled = False
                view.DeleteEntityToolStripMenuItem.Text = "Delete Publication Type"
            Else
                view.DeleteEntityToolStripMenuItem.Text = "Delete " + current_pubtype.name
                view.DeleteEntityToolStripMenuItem.Enabled = True
            End If
        End If
        'End If

    End Sub
    Public Sub delete_item()

        Try
            Dim omsg As bc_cs_message
            Dim item As String = ""
            Dim ntx As String = ""
            ntx = view.tpubtypes.SelectedNode.Text
            If InStr(ntx, "(inactive)") > 0 Then
                ntx = ntx.Substring(0, ntx.Length - 11)
            End If

            REM only allow if no users assigned
            Select Case view.maintain_mode


                Case 2
                    item = "Business Area"
                    For k = 0 To container.oAdmin.business_areas.Count - 1
                        If container.oAdmin.business_areas(k).description = ntx Then
                            For j = 0 To container.oPubtypes.pubtype.Count - 1
                                If container.oPubtypes.pubtype(j).bus_area_id = container.oAdmin.business_areas(k).id Then
                                    omsg = New bc_cs_message("Blue Curve", "Business area " + ntx + " has inactive publication types assigned cannot delete.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                                    Exit Sub
                                End If
                            Next
                        End If
                    Next
                    For k = 0 To container.oAdmin.business_areas.Count - 1
                        If container.oAdmin.business_areas(k).description = ntx Then
                            For j = 0 To container.oAdmin.user_bus_areas.Count - 1
                                If container.oAdmin.user_bus_areas(j).bus_area_id = container.oAdmin.business_areas(k).id Then
                                    omsg = New bc_cs_message("Blue Curve", "Business area " + ntx + " has inactive users assigned cannot delete.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                                    Exit Sub
                                End If
                            Next
                        End If
                    Next

                Case 3
                    item = "Language"
                    For j = 0 To container.oPubtypes.pubtype.Count - 1
                        For k = 0 To container.oAdmin.languages.Count - 1
                            If Trim(container.oAdmin.languages(k).language_name) = Trim(ntx) Then
                                If container.oPubtypes.pubtype(j).language = container.oAdmin.languages(k).language_id Then
                                    omsg = New bc_cs_message("Blue Curve", "Language " + Trim(ntx) + " has inactive publication types assigned cannot delete.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                                    Exit Sub
                                End If
                            End If
                        Next
                    Next
                    For j = 0 To container.oUsers.user.Count - 1
                        For k = 0 To container.oAdmin.languages.Count - 1
                            If Trim(container.oAdmin.languages(k).language_name) = Trim(ntx) Then
                                If container.oUsers.user(j).language_id = container.oAdmin.languages(k).language_id Then
                                    omsg = New bc_cs_message("Blue Curve", "Language  " + Trim(ntx) + " has inactive users assigned cannot delete.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                                    Exit Sub
                                End If
                            End If
                        Next
                    Next

            End Select
            omsg = New bc_cs_message("Blue Curve", "Are you sure you wish to delete " + Trim(ntx), bc_cs_message.MESSAGE, True, False, "Yes", "no", True)
            If omsg.cancel_selected = True Then
                Exit Sub
            End If

            Select Case view.maintain_mode
                Case 0

                Case 2
                    For i = 0 To container.oAdmin.business_areas.Count - 1
                        If container.oAdmin.business_areas(i).description = ntx Then
                            Dim business_areas As New bc_om_bus_area
                            business_areas.id = container.oAdmin.business_areas(i).id
                            business_areas.write_mode = bc_om_user_office.DELETE
                            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                                business_areas.db_write()
                            Else
                                business_areas.tmode = bc_om_user_role.tWRITE
                                If business_areas.transmit_to_server_and_receive(business_areas, True) = False Then
                                    Exit Sub
                                End If
                            End If
                            If business_areas.delete_error <> "" Then
                                omsg = New bc_cs_message("Blue Curve", item + " deletion failed database error: " + business_areas.delete_error, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                                Exit Sub
                            End If
                            item = item + " " + ntx
                            container.oAdmin.business_areas.RemoveAt(i)
                            Exit For
                        End If
                    Next
                Case 3
                    For i = 0 To container.oAdmin.languages.Count - 1
                        If Trim(container.oAdmin.languages(i).language_name) = Trim(ntx) Then
                            Dim language As New bc_om_user_language
                            language.language_id = container.oAdmin.languages(i).language_id
                            language.write_mode = bc_om_user_language.DELETE
                            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                                language.db_write()
                            Else
                                language.tmode = bc_om_user_role.tWRITE
                                If language.transmit_to_server_and_receive(language, True) = False Then
                                    Exit Sub
                                End If
                            End If
                            If language.delete_error <> "" Then
                                omsg = New bc_cs_message("Blue Curve", item + "  deletion failed database error: " + language.delete_error, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                                Exit Sub
                            End If
                            item = item + " " + ntx
                            container.oAdmin.languages.RemoveAt(i)
                            Exit For
                        End If
                    Next

            End Select
            load_pub_types_tree()
            load_pubtype_attributes()
            view.tpubtypes.Nodes(1).Expand()
            view.tpubtypes.Nodes(view.maintain_mode - 1).Expand()
            omsg = New bc_cs_message("Blue Curve", Trim(item) + " successfully deleted.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("DeleteToolStripMenuItem", "Click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
        End Try


    End Sub
    Public Sub add_pub_type(ByVal tx As String)

        REM check value is entered
        Dim omsg As bc_cs_message

        For i = 0 To container.oPubtypes.pubtype.Count - 1
            If UCase(container.oPubtypes.pubtype(i).name) = UCase(tx) Then
                omsg = New bc_cs_message("Blue Curve", "pub type " + tx + " already exists!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Sub
            End If
        Next
        current_pubtype = New bc_om_pub_type
        current_pubtype.name = tx
        view.Lpubtypes.Items.Add(tx)

        For i = 0 To view.Lpubtypes.Items.Count - 1
            If view.Lpubtypes.Items(i).Text = tx Then
                view.Lpubtypes.Items(i).ImageIndex = selected_icon
                view.Lpubtypes.Items(i).Selected = True
                Exit For
            End If
        Next

        'load_pubtype_attributes()
        'load_attribute_values()
        view.tpubtypename.TabPages(0).Text = tx
        view.ppubtype.Visible = True
        from_new = True
        'view.Lpubtypes.Items.Add(tx)
        'view.Lpubtypes.Items(view.Lpubtypes.Items.Count - 1).ImageIndex = 32
        'view.Lpubtypes.Items(view.Lpubtypes.Items.Count - 1).Selected = True
        edit_mode()

    End Sub
    Public Sub edit_mode()
        view.bentchanges.Enabled = True
        view.bentdiscard.Enabled = True
        view.Lpubtypes.Enabled = False
        view.tpubtypes.Enabled = False
        view.ppubtype.Enabled = True
        load_pubtype_toolbar()
        container.uxNavBar.Enabled = False
        container.uncomitted_data = True
    End Sub
    Private Sub exit_edit_mode()
        view.bentchanges.Enabled = False
        view.bentdiscard.Enabled = False
        view.Lpubtypes.Enabled = True
        view.tpubtypes.Enabled = True
        'view.lvwvalidate.Visible = False
        from_new = False
        load_pubtype_toolbar()
    End Sub

    Public Sub activate_pubtype()

        If InStr(view.Lpubtypes.SelectedItems(0).Text, "inactive") > 0 Then
            current_pubtype.inactive = 0
            current_pubtype.write_mode = bc_om_pub_type.SET_ACTIVE
        Else
            current_pubtype.inactive = 1
            current_pubtype.write_mode = bc_om_pub_type.SET_INACTIVE
        End If
        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
            current_pubtype.certificate.user_id = bc_cs_central_settings.logged_on_user_id
            current_pubtype.db_write()
        Else
            current_pubtype.tmode = bc_cs_soap_base_class.tWRITE
            If current_pubtype.transmit_to_server_and_receive(current_pubtype, True) = False Then
                Exit Sub
            End If
        End If
        For i = 0 To container.oPubtypes.pubtype.Count - 1
            If container.oPubtypes.pubtype(i).id = current_pubtype.id Then
                container.oPubtypes.pubtype(i) = current_pubtype
                Exit For
            End If
        Next
        Dim tx As String
        If current_pubtype.inactive = 0 Then
            tx = current_pubtype.name
            Dim omsg As New bc_cs_message("Blue Curve", tx + " has been reactivated please check all attributes to make sure they are active and valid otherwise publication type may not perform as expected.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
        Else
            tx = current_pubtype.name + " (inactive)"
        End If
        load_pub_types_tree()
        load_pub_types("All Publication Types", 0, "")

        For i = 0 To view.Lpubtypes.Items.Count - 1
            If view.Lpubtypes.Items(i).Text = tx Then
                view.Lpubtypes.Items(i).Selected = True
                Exit For
            End If
        Next
    End Sub
    Public Sub del_pubtype()

        Dim omsg As bc_cs_message
        omsg = New bc_cs_message("Blue Curve", "Are you sure you wish to delete " + current_pubtype.name + "?", bc_cs_message.MESSAGE, True, False, "Yes", "no", True)
        If omsg.cancel_selected = True Then
            Exit Sub
        End If

        current_pubtype.write_mode = bc_om_pub_type.DELETE
        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
            current_pubtype.db_write()
        Else
            current_pubtype.tmode = bc_om_pub_type.tWRITE
            If current_pubtype.transmit_to_server_and_receive(current_pubtype, True) = False Then
                Exit Sub
            End If
        End If
        If current_pubtype.delete_error <> "" Then
            omsg = New bc_cs_message("Blue Curve", "Failed to delete " + current_pubtype.name + current_pubtype.delete_error, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            Exit Sub
        End If
        REM take out from object
        For i = 0 To container.oPubtypes.pubtype.Count - 1
            If container.oPubtypes.pubtype(i).id = current_pubtype.id Then
                container.oPubtypes.pubtype.RemoveAt(i)
                Exit For
            End If
        Next
        load_pub_types_tree()
        load_pub_types("All Publication Types", 0, "")
        omsg = New bc_cs_message("Blue Curve", current_pubtype.name + " successfully deleted", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)

    End Sub
    Public Sub activate_items()
        Dim found As Boolean = False
        Dim item As String = ""
        If view.tpubtypes.SelectedNode.Parent.Text = "Publication Types By Business Area" Then
            view.maintain_mode = 2
        Else
            view.maintain_mode = 3
        End If
        If InStr(view.tpubtypes.SelectedNode.Text, "(inactive)") = 0 Then

            Select Case view.maintain_mode
                Case 2
                    For i = 0 To container.oPubtypes.pubtype.Count - 1
                        For j = 0 To container.oAdmin.business_areas.Count - 1
                            If container.oAdmin.business_areas(j).description = view.tpubtypes.SelectedNode.Text And container.oPubtypes.pubtype(i).inactive = False Then
                                If container.oPubtypes.pubtype(i).bus_area_id = container.oAdmin.business_areas(j).id Then
                                    item = "Cannot make business area " + view.tpubtypes.SelectedNode.Text + " inactive as active publication types are assigned to it!"
                                    found = True
                                    Exit For
                                End If
                                Exit For
                            End If
                        Next
                    Next

                    If found = False Then
                        For i = 0 To container.oUsers.user.Count - 1
                            For j = 0 To container.oAdmin.business_areas.Count - 1
                                If container.oAdmin.business_areas(j).description = view.tpubtypes.SelectedNode.Text And container.oUsers.user(i).inactive = False Then
                                    For k = 0 To container.oUsers.user(i).bus_areas.count - 1
                                        If container.oUsers.user(i).bus_areas(k) = container.oAdmin.business_areas(j).id Then
                                            item = "Cannot make business area " + view.tpubtypes.SelectedNode.Text + " inactive as active users are assigned to it!"
                                            found = True
                                            Exit For
                                        End If
                                    Next
                                    Exit For
                                End If
                            Next
                        Next
                    End If
                    REM for language and business area check users and pub types assigned.
                Case 3

                    For i = 0 To container.oPubtypes.pubtype.Count - 1
                        For j = 0 To container.oAdmin.languages.Count - 1
                            If container.oAdmin.languages(j).language_name = view.tpubtypes.SelectedNode.Text And container.oPubtypes.pubtype(i).inactive = False Then
                                If container.oPubtypes.pubtype(i).language = container.oAdmin.languages(j).language_id Then
                                    item = "Cannot make language " + view.tpubtypes.SelectedNode.Text + " inactive as active publication types are assigned to it!"
                                    found = True
                                    Exit For
                                End If
                                Exit For
                            End If
                        Next
                    Next
                    If found = False Then
                        For i = 0 To container.oUsers.user.Count - 1
                            For j = 0 To container.oAdmin.languages.Count - 1
                                If container.oAdmin.languages(j).language_name = view.tpubtypes.SelectedNode.Text And container.oUsers.user(i).inactive = False Then
                                    If container.oUsers.user(i).language_id = container.oAdmin.languages(j).language_id Then
                                        item = "Cannot make language " + view.tpubtypes.SelectedNode.Text + " inactive as active users are assigned to it!"
                                        found = True
                                        Exit For
                                    End If
                                    Exit For
                                End If
                            Next
                        Next
                    End If
            End Select
            If found = True Then
                Dim omsg As New bc_cs_message("Blue Curve", item, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            Else

                Select Case view.maintain_mode
                    Case 2
                        For i = 0 To container.oAdmin.business_areas.Count - 1
                            If container.oAdmin.business_areas(i).description = view.tpubtypes.SelectedNode.Text Then
                                container.oAdmin.business_areas(i).inactive = True
                                container.oAdmin.business_areas(i).write_mode = bc_om_user_language.SET_INACTIVE

                                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                                    container.oAdmin.business_areas(i).db_write()
                                Else
                                    container.oAdmin.business_areas(i).tmode = bc_om_user_role.tWRITE
                                    If container.oAdmin.business_areas(i).transmit_to_server_and_receive(container.oAdmin.business_areas(i), True) = False Then
                                        container.oAdmin.business_areas(i).inactive = False
                                        Exit Sub
                                    End If
                                End If
                            End If
                        Next
                    Case 3
                        For i = 0 To container.oAdmin.languages.Count - 1
                            If container.oAdmin.languages(i).language_name = view.tpubtypes.SelectedNode.Text Then
                                container.oAdmin.languages(i).inactive = True
                                container.oAdmin.languages(i).write_mode = bc_om_user_language.SET_INACTIVE

                                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                                    container.oAdmin.languages(i).db_write()
                                Else
                                    container.oAdmin.languages(i).tmode = bc_om_user_role.tWRITE
                                    If container.oAdmin.languages(i).transmit_to_server_and_receive(container.oAdmin.languages(i), True) = False Then
                                        container.oAdmin.languages(i).inactive = False
                                        Exit Sub
                                    End If
                                End If
                            End If
                        Next
                End Select
                load_pub_types_tree()
                view.tpubtypes.Nodes(view.maintain_mode - 1).Expand()

            End If
        Else
            Dim ntx As String
            ntx = view.tpubtypes.SelectedNode.Text
            If InStr(ntx, "(inactive)") > 0 Then
                ntx = ntx.Substring(0, ntx.Length - 11)
            End If
            Select Case view.maintain_mode
                Case 0

                Case 2
                    For i = 0 To container.oAdmin.business_areas.Count - 1
                        If container.oAdmin.business_areas(i).description = ntx Then
                            container.oAdmin.business_areas(i).inactive = False
                            container.oAdmin.business_areas(i).write_mode = bc_om_user_language.SET_ACTIVE

                            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                                container.oAdmin.business_areas(i).db_write()
                            Else
                                container.oAdmin.business_areas(i).tmode = bc_om_user_role.tWRITE
                                If container.oAdmin.business_areas(i).transmit_to_server_and_receive(container.oAdmin.business_areas(i), True) = False Then
                                    container.oAdmin.business_areas(i).inactive = True
                                    Exit Sub
                                End If
                            End If
                        End If
                    Next
                Case 3
                    For i = 0 To container.oAdmin.languages.Count - 1
                        If container.oAdmin.languages(i).language_name = ntx Then
                            container.oAdmin.languages(i).inactive = False
                            container.oAdmin.languages(i).write_mode = bc_om_user_language.SET_ACTIVE

                            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                                container.oAdmin.languages(i).db_write()
                            Else
                                container.oAdmin.languages(i).tmode = bc_om_user_role.tWRITE
                                If container.oAdmin.languages(i).transmit_to_server_and_receive(container.oAdmin.languages(i), True) = False Then
                                    container.oAdmin.languages(i).inactive = True
                                    Exit Sub
                                End If
                            End If
                        End If
                    Next
            End Select
            load_pub_types_tree()
            'load_pubtype_attributes()
            view.tpubtypes.Nodes(view.maintain_mode - 1).Expand()
        End If
    End Sub
    Public Sub save_item_changes(ByVal tx As String)
        Dim found As Boolean = False
        Dim item As String = ""
        If view.tpubtypes.SelectedNode.Parent.Text = "Publication Types By Business Area" Then
            view.maintain_mode = 2
        Else
            view.maintain_mode = 3
        End If

        Select Case view.maintain_mode
            Case 2
                For j = 0 To container.oAdmin.business_areas.Count - 1
                    If Trim(UCase(container.oAdmin.business_areas(j).description)) = Trim(UCase(tx)) Then
                        item = "Business area " + tx + " already exists!"
                        Dim omsg As New bc_cs_message("Blue Curve", item, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)

                        Exit Sub
                    End If
                Next

                REM for language and business area check users and pub types assigned.
            Case 3
                For j = 0 To container.oAdmin.languages.Count - 1
                    If Trim(UCase(container.oAdmin.languages(j).language_name) = Trim(UCase(tx))) Then
                        item = "Language " + tx + " already exists!"
                        Dim omsg As New bc_cs_message("Blue Curve", item, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                        Exit Sub
                    End If
                Next
        End Select

        Select Case view.maintain_mode
            Case 2
                For i = 0 To container.oAdmin.business_areas.Count - 1
                    If container.oAdmin.business_areas(i).description = view.tpubtypes.SelectedNode.Text Then
                        container.oAdmin.business_areas(i).write_mode = bc_om_user_language.UPDATE
                        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                            container.oAdmin.business_areas(i).db_write()
                        Else
                            container.oAdmin.business_areas(i).tmode = bc_om_user_role.tWRITE
                            If container.oAdmin.business_areas(i).transmit_to_server_and_receive(container.oAdmin.business_areas(i), True) = False Then
                                Exit Sub
                            End If
                        End If
                        container.oAdmin.business_areas(i).description = tx
                        Exit For
                    End If
                Next
            Case 3
                For i = 0 To container.oAdmin.languages.Count - 1
                    If container.oAdmin.languages(i).language_name = view.tpubtypes.SelectedNode.Text Then
                        container.oAdmin.languages(i).write_mode = bc_om_user_language.UPDATE
                        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                            container.oAdmin.languages(i).db_write()
                        Else
                            container.oAdmin.languages(i).tmode = bc_om_user_role.tWRITE
                            If container.oAdmin.languages(i).transmit_to_server_and_receive(container.oAdmin.languages(i), True) = False Then
                                Exit Sub
                            End If
                        End If
                        container.oAdmin.languages(i).language_name = tx
                        Exit For
                    End If
                Next
        End Select
        load_pub_types_tree()
        view.tpubtypes.Nodes(view.maintain_mode - 1).Expand()
    End Sub
    Public Sub delete_item(ByVal tx As String)
        Dim found As Boolean = False
        Dim item As String = ""
        If view.tpubtypes.SelectedNode.Parent.Text = "Publication Types By Business Area" Then
            view.maintain_mode = 2
        Else
            view.maintain_mode = 3
        End If


        Select Case view.maintain_mode
            Case 2
                For i = 0 To container.oAdmin.business_areas.Count - 1
                    If container.oAdmin.business_areas(i).description = tx Then
                        container.oAdmin.business_areas(i).write_mode = bc_om_user_language.DELETE
                        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                            container.oAdmin.business_areas(i).db_write()

                        Else
                            container.oAdmin.business_areas(i).tmode = bc_om_user_role.tWRITE
                            If container.oAdmin.business_areas(i).transmit_to_server_and_receive(container.oAdmin.business_areas(i), True) = False Then
                                Exit Sub
                            End If
                        End If
                        container.oAdmin.business_areas.RemoveAt(i)
                        Exit For
                    End If
                Next
            Case 3
                For i = 0 To container.oAdmin.languages.Count - 1
                    If container.oAdmin.languages(i).language_name = tx Then
                        container.oAdmin.languages(i).write_mode = bc_om_user_language.DELETE
                        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                            container.oAdmin.languages(i).db_write()
                        Else
                            container.oAdmin.languages(i).tmode = bc_om_user_role.tWRITE
                            If container.oAdmin.languages(i).transmit_to_server_and_receive(container.oAdmin.languages(i), True) = False Then
                                Exit Sub
                            End If

                        End If
                        container.oAdmin.languages.RemoveAt(i)
                        Exit For
                    End If
                Next
        End Select
        load_pub_types_tree()
        view.tpubtypes.Nodes(view.maintain_mode - 1).Expand()
    End Sub
    Public Sub size_nicely()
        'view.DataGridView1.Width = view.workflow.Width
        view.Lpubtypes.Columns(0).Width = view.Lpubtypes.Width - 5
        'If view.DataGridView1.Columns(3).Visible = True Then
        '    view.DataGridView1.Columns(1).Width = view.DataGridView1.Width * (4.5 / 10)
        '    view.DataGridView1.Columns(2).Width = view.DataGridView1.Width * (4.5 / 10)
        'Else
        '    view.DataGridView1.Columns(1).Width = view.DataGridView1.Width * (5 / 10)
        '    view.DataGridView1.Columns(2).Width = view.DataGridView1.Width * (5 / 10)
        'End If
    End Sub
    
    Public Sub commit_changes()
        Try
            view.Cursor = Cursors.WaitCursor
            save_changes(view.Lpubtypes.SelectedItems(0).Text)
          Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_pubtype", "commit_changes", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            container.uxNavBar.Enabled = True
            container.uncomitted_data = False
            view.Cursor = Cursors.Default
        End Try
    End Sub
    Public Sub save_changes(ByVal name As String)

        Try
            If check_warnings() = False Then
                Dim omsg As New bc_cs_message("Blue Curve", "Publication Type cannot be submitted as missing attributes/associations please check warnings", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                view.lvwvalidate.Visible = True
                Exit Sub
            End If
            current_pubtype.name = name
            REM is new language or business area is requried load these first
            With container.oAdmin.languages(container.oAdmin.languages.Count - 1)
                If .language_id = -1 Then
                    .write_mode = bc_om_user_language.INSERT
                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        .db_write()
                    Else
                        .tmode = bc_om_user_role.tWRITE
                        If .transmit_to_server_and_receive(container.oAdmin.languages(container.oAdmin.languages.Count - 1), True) = False Then
                            Exit Sub
                        End If
                    End If
                End If
            End With
            With container.oAdmin.business_areas(container.oAdmin.business_areas.Count - 1)
                If .id = -1 Then
                    .write_mode = bc_om_user_language.INSERT
                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        .db_write()
                    Else
                        .tmode = bc_om_user_role.tWRITE
                        If .transmit_to_server_and_receive(container.oAdmin.business_areas(container.oAdmin.business_areas.Count - 1), True) = False Then
                            Exit Sub
                        End If
                    End If


                End If
            End With

            REM write attribute values into object
            For i = 0 To container.oAdmin.business_areas.Count - 1
                If container.oAdmin.business_areas(i).description = view.user_attributes_values(0).value Then
                    current_pubtype.bus_area_id = container.oAdmin.business_areas(i).id
                    Exit For
                End If
            Next
            For i = 0 To container.oAdmin.languages.Count - 1
                If container.oAdmin.languages(i).language_name = view.user_attributes_values(1).value Then
                    current_pubtype.language = container.oAdmin.languages(i).language_id
                    Exit For
                End If
            Next
            For i = 0 To container.oClasses.classes.Count - 1
                If container.oClasses.classes(i).class_name = view.user_attributes_values(2).value Then
                    current_pubtype.child_category = container.oClasses.classes(i).class_id
                    Exit For
                End If
            Next
            current_pubtype.description = view.user_attributes_values(3).value
            current_pubtype.support_doc_only = False
            If view.user_attributes_values(4).value = "True" Then
                current_pubtype.support_doc_only = True
            End If
            REM JIRA 6825
            For i = 0 To container.oPubtypes.pubtype.Count - 1
                If container.oPubtypes.pubtype(i).name = view.user_attributes_values(5).value Then
                    current_pubtype.default_support_pub_type = container.oPubtypes.pubtype(i).id
                    Exit For
                End If
            Next
            REM END JIRA
            REM FIL 5.3
            current_pubtype.mandatory_default_support_doc = False
            If view.user_attributes_values(6).value = "True" Then
                current_pubtype.mandatory_default_support_doc = True
            End If
            current_pubtype.folder_name = view.user_attributes_values(7).value


            REM ==

            If from_new = False Then
                current_pubtype.write_mode = bc_om_pub_type.UPDATE_PT
            Else
                current_pubtype.write_mode = bc_om_pub_type.INSERT
            End If
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                current_pubtype.db_write()
            Else
                current_pubtype.tmode = bc_om_pub_type.tWRITE
                If current_pubtype.transmit_to_server_and_receive(current_pubtype, True) = False Then
                    Exit Sub
                End If
            End If
            REM update global  object 
            If from_new = False Then
                For i = 0 To container.oPubtypes.pubtype.Count - 1
                    If container.oPubtypes.pubtype(i).id = current_pubtype.id Then
                        container.oPubtypes.pubtype(i) = current_pubtype
                    End If
                Next
            Else
                container.oPubtypes.pubtype.Add(current_pubtype)
            End If
            If from_new = False Then
                Dim omsg As New bc_cs_message("Blue Curve", "Publication type: " + current_pubtype.name + " successfully updated!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            Else
                Dim omsg As New bc_cs_message("Blue Curve", "Publication type: " + current_pubtype.name + " successfully added!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            End If

            REM write down process
            opt.id = current_pubtype.id
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                opt.db_write()
            Else
                opt.tmode = bc_cs_soap_base_class.tWRITE
                If opt.transmit_to_server_and_receive(opt, True) = False Then
                    Exit Sub
                End If
            End If

            exit_edit_mode()
            from_new = False
            Dim tx As String
            tx = current_pubtype.name
            load_pub_types_tree()
            load_pub_types("All Publication Types", 0, "")
            For i = 0 To view.Lpubtypes.Items.Count - 1
                If view.Lpubtypes.Items(i).Text = tx Then
                    view.Lpubtypes.Items(i).Selected = True
                    Exit For
                End If
            Next
            REM read back  stages
            Dim ostages As New bc_om_workflow_stages
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                ostages.db_read()
            Else
                ostages.tmode = bc_cs_soap_base_class.tREAD
                ostages.transmit_to_server_and_receive(ostages, True)
            End If
            Me.container.oPubtypes.stages = ostages.stages

            container.set_sync = True

            container.oAdmin.sync_types.sync_types(1).sync_set = True

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bentchanges", "click", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try

    End Sub
    Public Sub attribute_details()
        view.mdettitle.Text = "Details for: " + view.tpubtypename.SelectedTab.Text
        view.mdetupdate.Text = "Last Updated: " + current_pubtype.comment
        view.mdetuser.Text = "By: Unknown"
        If Me.current_pubtype.user_id = CStr(bc_cs_central_settings.logged_on_user_id) Then
            view.mdetuser.Text = "By: me"
        Else
            For i = 0 To container.oUsers.user.Count - 1
                If container.oUsers.user(i).id = Me.current_pubtype.user_id Then
                    view.mdetuser.Text = "By: " + container.oUsers.user(i).first_name + " " + container.oUsers.user(i).surname
                    Exit For
                End If
            Next
        End If
        view.mdetpubupdate.Visible = False
        view.mdetpubuser.Visible = False
        view.show_attribute_details()

    End Sub
    Public Function set_data_type_Control() As Boolean
        Try
            Dim lus As New ArrayList
            Dim i As Integer
            set_data_type_Control = False
            If no_action = True Then
                Exit Function
            End If
            If view.DataGridView1.SelectedCells(0).ColumnIndex <> 2 Then
                Exit Function
            End If

            no_action = True

            REM temporary hold current values
            hold_vals.Clear()
            For i = 0 To view.DataGridView1.Rows.Count - 1
                hold_vals.Add(view.DataGridView1.Item(2, i).Value)
                If IsNothing(hold_vals(i)) Or InStr(hold_vals(i), "(inactive)") > 0 Then
                    hold_vals(i) = ""
                End If
            Next
            Dim lu As DataGridViewComboBoxCell
            Dim use_combo As Boolean = False
            Dim dtextbox As New DataGridViewTextBoxColumn
            Dim Dcombobox As New DataGridViewComboBoxColumn
            Dim row_id As Integer
            row_id = view.DataGridView1.SelectedCells(0).RowIndex

            Dim j As Integer
            REM 1 string 2 number 3 boolean 5 date
            'view.DataGridView1.Item(1, row_id).Selected = True
            REM build up array of all lookup values
            i = view.DataGridView1.SelectedCells(0).RowIndex
            For j = 0 To view.DataGridView1.Rows.Count - 1
                If user_attributes(j).is_lookup = True Then
                    lu = New DataGridViewComboBoxCell
                    lu.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing

                    For k = 0 To user_attributes(j).lookup_values.count - 1
                        lu.Items.Add(user_attributes(j).lookup_values(k))
                    Next
                    lu.Items.Add("")
                    lus.Add(lu)

                ElseIf user_attributes(j).type_id = 3 Then
                    lu = New DataGridViewComboBoxCell
                    lu.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing

                    lu.Items.Add("")
                    lu.Items.Add("True")
                    lu.Items.Add("False")
                    lus.Add(lu)

                Else
                    lu = New DataGridViewComboBoxCell
                    lu.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing

                    If IsNothing(hold_vals(j)) Then
                        lu.Items.Add("")
                    Else
                        lu.Items.Add(hold_vals(j))
                    End If
                    lus.Add(lu)

                End If
            Next
            use_combo = False
            If user_attributes(i).is_lookup = True Then
                use_combo = True
            ElseIf user_attributes(i).type_id = 3 Then
                use_combo = True
            End If
            REM 
            Dcombobox.Width = view.DataGridView1.Columns(2).Width

            dtextbox.Width = view.DataGridView1.Columns(2).Width

            dtextbox.MaxInputLength = 250
            view.DataGridView1.Columns.RemoveAt(2)
            If use_combo = True Then
                Dcombobox.Name = "Values"
                view.DataGridView1.Columns.Insert(2, Dcombobox)
            Else
                dtextbox.Name = "Values"
                view.DataGridView1.Columns.Insert(2, dtextbox)
            End If
            REM reset value
            no_action = True
            For i = 0 To view.DataGridView1.Rows.Count - 1
                If use_combo = True Then
                    view.DataGridView1.Item(2, i) = lus(i)
                End If
                If hold_vals(i) <> "" Then
                    view.DataGridView1.Item(2, i).Value = hold_vals(i)
                End If
            Next

            view.DataGridView1.Item(2, row_id).Selected = True
            no_action = False
            set_data_type_Control = True
        Catch ex As Exception
            MsgBox(ex.Message)

        End Try

    End Function
    Public Function validate_attribute_value(ByVal val) As Boolean

        no_action = True
        Try
            validate_attribute_value = True

            Try
                If view.DataGridView1.SelectedCells(0).ColumnIndex <> 2 Then
                    Exit Function
                End If
            Catch
                Exit Function
            End Try
            Dim failed As Boolean = False
            Dim i As Integer

            i = view.DataGridView1.SelectedCells(0).RowIndex

            If Trim(val) = "" Then

                Select Case user_attributes(i).type_id And user_attributes(i).is_lookup = False
                    Case 2
                        If IsNumeric(val) = False Then
                            Dim omsg As New bc_cs_message("Blue Cuvre", "Item must be numeric", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                            failed = True
                        End If
                    Case 5
                        If IsDate(val) = False Then
                            Dim omsg As New bc_cs_message("Blue Cuvre", "Item must be Date and Time", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                            failed = True
                        End If
                End Select

            End If

            If failed = True Then
                validate_attribute_value = False
            Else

                set_val(val)

                If view.lvwvalidate.Visible = True Then

                    check_warnings()
                End If
            End If
        Catch ex As Exception
            MsgBox("validate: " + ex.Message)
        Finally
            no_action = False
        End Try
    End Function
    Public Sub set_val(ByVal val As String, Optional ByVal ext As Boolean = False)
        Try
            If val <> view.user_attributes_values(view.DataGridView1.SelectedCells(0).RowIndex).value Then
                view.user_attributes_values(view.DataGridView1.SelectedCells(0).RowIndex).value = val
                If ext = True Then
                    view.DataGridView1.Item(2, view.DataGridView1.SelectedCells(0).RowIndex).Value = val
                End If
                view.user_attributes_values(view.DataGridView1.SelectedCells(0).RowIndex).value_changed = True
                view.DataGridView1.Item(0, view.DataGridView1.SelectedCells(0).RowIndex).Value = view.tabimages.Images(att_edited_icon)
                Me.edit_mode()

            End If
        Catch ex As Exception
            'MsgBox("here:" + ex.Message)


        End Try
    End Sub
    Public Sub load_pubtype_toolbar()
        Dim newitem As ToolStripItem
        Dim top_node As Boolean = False
        Dim unlinked_class As Boolean = False
        Me.container.uxGenericToolStrip.Items.Clear()
        If Me.container.mode = bc_am_cp_container.VIEW Then
            Exit Sub
        End If
        Me.container.uxGenericToolStrip.ImageList = Me.container.uxToolStripImageList
        With Me.container.uxGenericToolStrip.Items
            REM schema
            If view.bentchanges.Enabled = False Then
                newitem = .Add("Add PubType")
                newitem.ToolTipText = "Add new Publication Type"
                newitem.ImageIndex = add_icon
                newitem.TextImageRelation = TextImageRelation.ImageAboveText
                If view.Lpubtypes.SelectedItems.Count = 0 Then
                    newitem = .Add("Change PubType Name")
                    newitem.Enabled = False
                    newitem.ImageIndex = edit_icon
                    newitem.TextImageRelation = TextImageRelation.ImageAboveText
                    newitem.ToolTipText = "Change Publication Type name"
                    newitem = .Add("Deactivate PubType")
                    newitem.Enabled = False
                    newitem.ImageIndex = deactivate_icon
                    newitem.TextImageRelation = TextImageRelation.ImageAboveText
                    newitem.ToolTipText = "Deactivate Publication Type"
                    If Me.container.mode = bc_am_cp_container.EDIT_FULL Then
                        newitem = .Add("Delete PubType")
                        newitem.Enabled = False
                        newitem.ImageIndex = delete_icon
                        newitem.TextImageRelation = TextImageRelation.ImageAboveText
                        newitem.ToolTipText = "Delete Publication Type"
                    End If
                Else
                    If InStr(view.Lpubtypes.SelectedItems(0).Text, "(inactive)") = 0 Then
                        newitem = .Add("Change PubType Name")
                        newitem.ImageIndex = edit_icon
                        newitem.TextImageRelation = TextImageRelation.ImageAboveText
                        newitem.ToolTipText = "Change name of publication type " + view.Lpubtypes.SelectedItems(0).Text
                        newitem = .Add("Deactivate")
                        newitem.ImageIndex = deactivate_icon
                        newitem.TextImageRelation = TextImageRelation.ImageAboveText
                        newitem.ToolTipText = "Deactivate publication type " + view.Lpubtypes.SelectedItems(0).Text

                        If Me.container.mode = bc_am_cp_container.EDIT_FULL Then

                            newitem = .Add("Delete PubType")
                            newitem.Enabled = False
                            newitem.ImageIndex = delete_icon
                            newitem.TextImageRelation = TextImageRelation.ImageAboveText
                            newitem.ToolTipText = "Delete publication type"
                        End If
                    Else
                        newitem = .Add("Change PubType Name")
                        newitem.ImageIndex = edit_icon
                        newitem.TextImageRelation = TextImageRelation.ImageAboveText
                        newitem.ToolTipText = "Change name of publication type "
                        newitem.Enabled = False
                        newitem = .Add("Activate")
                        newitem.ImageIndex = selected_icon
                        newitem.TextImageRelation = TextImageRelation.ImageAboveText
                        newitem.ToolTipText = "Activate publication type " + Left(view.Lpubtypes.SelectedItems(0).Text, Me.view.Lpubtypes.SelectedItems(0).Text.Length - 10)
                        If Me.container.mode = bc_am_cp_container.EDIT_FULL Then

                            newitem = .Add("Delete PubType")
                            newitem.ImageIndex = delete_icon
                            newitem.TextImageRelation = TextImageRelation.ImageAboveText
                            newitem.ToolTipText = "Delete publication type " + view.Lpubtypes.SelectedItems(0).Text
                        End If
                        End If
                End If

                Dim separator As New ToolStripSeparator
                .Add(separator)

                newitem = .Add("Add Language")
                newitem.ImageIndex = add_icon
                newitem.TextImageRelation = TextImageRelation.ImageAboveText
                newitem.Enabled = False

                If view.DataGridView1.SelectedCells.Count = 1 And view.Lpubtypes.SelectedItems.Count > 0 Then
                    If view.DataGridView1.SelectedCells(0).RowIndex = 1 Then
                        newitem.Enabled = True
                        newitem.ToolTipText = "Add new language and assign to " + view.Lpubtypes.SelectedItems(0).Text

                    End If
                End If

                separator = New ToolStripSeparator
                .Add(separator)

                newitem = .Add("Add Bus Area")
                newitem.ImageIndex = add_icon
                newitem.TextImageRelation = TextImageRelation.ImageAboveText
                newitem.Enabled = False
                If view.DataGridView1.SelectedCells.Count = 1 Then
                    If view.DataGridView1.SelectedCells(0).RowIndex = 0 And view.Lpubtypes.SelectedItems.Count > 0 Then
                        newitem.Enabled = True
                        newitem.ToolTipText = "Add new business area and assign to " + view.Lpubtypes.SelectedItems(0).Text
                    End If
                End If
            Else
                newitem = .Add("Add PubType")
                newitem.Enabled = False
                newitem.TextImageRelation = TextImageRelation.ImageAboveText
                newitem.ToolTipText = "Add new Publication Type"
                newitem.ImageIndex = add_icon
                newitem = .Add("Change PubType Name")
                newitem.Enabled = False
                newitem.ImageIndex = edit_icon
                newitem.TextImageRelation = TextImageRelation.ImageAboveText
                newitem.ToolTipText = "Change Publication Type name"
                newitem = .Add("Deactivate")
                newitem.Enabled = False
                newitem.ImageIndex = deactivate_icon
                newitem.TextImageRelation = TextImageRelation.ImageAboveText
                newitem.ToolTipText = "Deactivate Publication Type"
                newitem = .Add("Delete PubType")
                newitem.Enabled = False
                newitem.ImageIndex = delete_icon
                newitem.ToolTipText = "Delete schema"

                Dim separator As New ToolStripSeparator
                .Add(separator)

                newitem = .Add("Add Language")
                newitem.ImageIndex = add_icon
                newitem.TextImageRelation = TextImageRelation.ImageAboveText
                newitem.Enabled = False
                If view.DataGridView1.SelectedCells.Count = 1 Then
                    If view.DataGridView1.SelectedCells(0).RowIndex = 1 Then
                        newitem.Enabled = True
                    End If
                End If

                separator = New ToolStripSeparator
                .Add(separator)

                newitem = .Add("Add Bus Area")
                newitem.Enabled = False
                newitem.ImageIndex = add_icon
                newitem.TextImageRelation = TextImageRelation.ImageAboveText
                If view.DataGridView1.SelectedCells.Count = 1 Then
                    If view.DataGridView1.SelectedCells(0).RowIndex = 0 Then
                        newitem.Enabled = True
                    End If
                End If
            End If
        End With
    End Sub
   


End Class





