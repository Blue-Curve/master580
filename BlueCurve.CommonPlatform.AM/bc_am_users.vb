Imports BlueCurve.Core.OM
Imports BlueCurve.Core.CS
Imports System.Windows.Forms
Imports System.Collections

Public Class bc_am_users
    Public view As bc_am_cp_users
    Public container As bc_am_cp_container
    Private maintain_mode As Integer
    Public user_attributes As New ArrayList
    Public from_new As Boolean
    Public no_action As Boolean = False
    Public hold_vals As New ArrayList
    Public current_user As New bc_om_user
    Public new_user As Boolean = False
    Private setting_text_box As Boolean = False

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
    Private Const view_icon As Integer = 28
    Private Const applications_icon As Integer = 29
    Private Const advanced_icon As Integer = 30
    Private Const translation_icon As Integer = 33

    Public using_loading As Boolean
    'Password Encryption
    Private pwd As Char = "*"
    Public Sub set_default_attribute_values()
        Try
            For i = 0 To view.DataGridView1.Rows.Count - 1
                For j = 0 To container.oClasses.attribute_pool.Count - 1
                    If container.oClasses.attribute_pool(j).name = view.DataGridView1.Item(1, i).Value Then
                        If Me.container.oClasses.attribute_pool(j).is_def = True Then
                            Dim oa As New bc_om_attribute
                            oa.attribute_id = Me.container.oClasses.attribute_pool(j).attribute_id
                            oa.read_default_value = True
                            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                                oa.db_read_default_value()
                            Else
                                oa.tmode = bc_cs_soap_base_class.tREAD
                                If oa.transmit_to_server_and_receive(oa, True) = False Then
                                    Exit Sub
                                End If
                            End If
                            'user_attributes(view.DataGridView1.SelectedCells(0).RowIndex).value = oa.default_value
                            'user_attributes(view.DataGridView1.SelectedCells(0).RowIndex).value_changed = True
                            user_attributes(i).value = oa.default_value
                            user_attributes(i).value_changed = True
                            view.DataGridView1.Item(3, i).Value = oa.default_value
                            Exit Sub
                        End If
                    End If
                Next
            Next

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_users", "set_default_attribute_values", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub


    Public Sub New(ByVal container As bc_am_cp_container, Optional ByVal view As bc_am_cp_users = Nothing)
        If Not view Is Nothing Then
            view.controller = Me
            Me.view = view
        End If
        If Not container Is Nothing Then
            Me.container = container
        End If
        size_nicely()
    End Sub
    Public Sub size_nicely()
        Try
            view.Lusers.Columns(0).Width = view.Lusers.Width - 5
            data_grid_size()
        Catch

        End Try
    End Sub
    Public Sub load_all()
        view.puserup.Visible = False
        view.puserdn.Visible = False

        If Me.container.mode = bc_am_cp_container.VIEW Then
            view.tentities.ContextMenuStrip = Nothing
            view.tusers.ContextMenuStrip = Nothing
            view.dpartialsync.Columns(1).ReadOnly = True
            view.DataGridView1.Columns(3).ReadOnly = True
            view.DataGridView1.Columns(0).Visible = False
            view.DataGridView1.Columns(2).Visible = False
            view.Lusers.ContextMenuStrip = Nothing
            view.bentchanges.Visible = False
            view.bentdiscard.Visible = False
        End If
        If Me.container.mode > bc_am_cp_container.VIEW Then
            view.bentchanges.Visible = True
            view.bentdiscard.Visible = True
        End If
        If Me.container.mode > bc_am_cp_container.EDIT_FULL Then
            view.DeleteEntityToolStripMenuItem.Visible = True
        End If
        load_users_tree()
        load_user_attributes()
        view.Lusers.ContextMenuStrip = Nothing
        view.DeleteEntityToolStripMenuItem.Visible = False

        If Me.container.mode > 0 Then
            view.Lusers.ContextMenuStrip = view.UserContextMenuStrip
            If Me.container.mode > 1 Then
                REM delete user level 3 only
                view.DeleteEntityToolStripMenuItem.Visible = True
            End If
        End If
        Me.size_nicely()

    End Sub
    Private Sub load_users_tree()
        Try
            Dim ffound As Boolean = False
            Dim mfound As Boolean = False
            Dim rfound As Boolean = False
            Dim vfound As Boolean = False
            Dim wfound As Boolean = False
            Dim lfound As Boolean = False
            REM JIRA 4988
            Dim afound As Boolean = False

            Dim dfound As Boolean = False
            Dim efound As Boolean = False
            view.tusers.Nodes.Clear()
            view.tusers.Nodes.Add("All Users", "All Users", users_icon)
            view.tusers.Nodes(0).Nodes.Add("Active", "Active", folder_icon)
            view.tusers.Nodes(0).Nodes.Add("Inactive", "Inactive", folder_icon)
            view.tusers.Nodes(0).Nodes.Add("Synced Users", "Synced Users", folder_icon)
            view.tusers.Nodes(0).Nodes.Add("Unsynced Users", "Unsynced Users", folder_icon)

            If container.oAdmin.sync_types.sync_types.Count > 0 Then
                view.tusers.Nodes(0).Nodes.Add("Partially Synced Users", "Partially Synced Users", folder_icon)
            End If
            view.tusers.Nodes.Add("Users by Security Role", "Users by Security Role", users_icon)
            For i = 0 To Me.container.oAdmin.roles.Count - 1
                If Me.container.oAdmin.roles(i).inactive = 0 Then
                    view.tusers.Nodes(1).Nodes.Add(Me.container.oAdmin.roles(i).description, Me.container.oAdmin.roles(i).description, folder_icon)
                Else
                    view.tusers.Nodes(1).Nodes.Add(Me.container.oAdmin.roles(i).description + " (inactive)", Me.container.oAdmin.roles(i).description + " (inactive)", deactivate_icon)
                End If
                Select Case Me.container.oAdmin.roles(i).level
                    Case 0
                        view.tusers.Nodes(1).Nodes(i).Nodes.Add("Access Level: View Only", "Access Level: View Only", view_icon)
                    Case 1
                        view.tusers.Nodes(1).Nodes(i).Nodes.Add("Access Level: Edit Lite", "Access Level: Edit Lite", att_edited_icon)
                    Case 2
                        view.tusers.Nodes(1).Nodes(i).Nodes.Add("Access Level: Advanced Edit", "Access Level: Advanced Edit", advanced_icon)
                    Case Else
                        view.tusers.Nodes(1).Nodes(i).Nodes.Add("Access Level: View Only", "Access Level: View Only", view_icon)

                End Select
                view.tusers.Nodes(1).Nodes(i).Nodes.Add("Admin Applications...", "Admin Applications", applications_icon)
                Dim apps_found(3, 1) As Boolean
                For k = 0 To 2
                    apps_found(k, 0) = False
                Next

                For j = 0 To Me.container.oAdmin.roles(i).apps.count - 1
                    Select Case Me.container.oAdmin.roles(i).apps(j)
                        Case 1
                            apps_found(0, 0) = True
                        Case 2
                            apps_found(1, 0) = True
                        Case 3
                            apps_found(2, 0) = True
                        Case 4
                            apps_found(3, 0) = True
                    End Select
                Next
                If apps_found(0, 0) = True Then
                    view.tusers.Nodes(1).Nodes(i).Nodes(1).Nodes.Add("Taxonomy", "Taxonomy", parent_icon)
                Else
                    view.tusers.Nodes(1).Nodes(i).Nodes(1).Nodes.Add("Taxonomy", "Taxonomy", deactivate_icon)
                End If
                If apps_found(1, 0) = True Then
                    view.tusers.Nodes(1).Nodes(i).Nodes(1).Nodes.Add("Users", "Users", users_icon)
                Else
                    view.tusers.Nodes(1).Nodes(i).Nodes(1).Nodes.Add("Users", "Users", deactivate_icon)
                End If
                If apps_found(2, 0) = True Then
                    view.tusers.Nodes(1).Nodes(i).Nodes(1).Nodes.Add("Publication Types", "Publication Types", publication_icon)
                Else
                    view.tusers.Nodes(1).Nodes(i).Nodes(1).Nodes.Add("Publication Types", "Publication Types", deactivate_icon)
                End If
                If apps_found(3, 0) = True Then
                    view.tusers.Nodes(1).Nodes(i).Nodes(1).Nodes.Add("Translation", "Translation", translation_icon)
                Else
                    view.tusers.Nodes(1).Nodes(i).Nodes(1).Nodes.Add("Translation", "Translation", deactivate_icon)
                End If


                view.tusers.Nodes(1).Nodes(i).Nodes.Add("Stage Access...", "Stage Access", applications_icon)
                Dim tx As String
                For j = 0 To Me.container.oPubtypes.stages.Count - 1
                    ffound = False
                    mfound = False
                    rfound = False
                    vfound = False
                    wfound = False
                    lfound = False
                    REM JIRA 4988
                    afound = False
                    dfound = False
                    efound = False
                    tx = " ["
                    For m = 0 To Me.container.oAdmin.roles(i).stage_role_access.Count - 1
                        If Me.container.oAdmin.roles(i).stage_role_access(m).stage_name = Me.container.oPubtypes.stages(j) Then

                            Select Case Trim(Me.container.oAdmin.roles(i).stage_role_access(m).access_id)
                                Case "F"
                                    ffound = True
                                    tx = tx + "F"
                                Case "M"
                                    mfound = True
                                    tx = tx + "M"
                                Case "R"
                                    rfound = True
                                    tx = tx + "R"
                                Case "V"
                                    vfound = True
                                    tx = tx + "V"
                                Case "W"
                                    wfound = True
                                    tx = tx + "W"
                                Case "L"
                                    lfound = True
                                    tx = tx + "L"
                                    REM JIRA 4988
                                Case "A"
                                    afound = True
                                    tx = tx + "A"
                                Case "D"
                                    dfound = True
                                    tx = tx + "D"
                                Case "E"
                                    efound = True
                                    tx = tx + "E"
                            End Select
                        End If
                    Next
                    tx = tx + "]"
                    view.tusers.Nodes(1).Nodes(i).Nodes(2).Nodes.Add(Me.container.oPubtypes.stages(j), Me.container.oPubtypes.stages(j) + tx, 31)
                    If ffound = True Then
                        view.tusers.Nodes(1).Nodes(i).Nodes(2).Nodes(j).Nodes.Add("Force Check In", "Force Check In", 20)
                    Else
                        view.tusers.Nodes(1).Nodes(i).Nodes(2).Nodes(j).Nodes.Add("Force Check In", "Force Check In", 6)
                    End If
                            If mfound = True Then
                        view.tusers.Nodes(1).Nodes(i).Nodes(2).Nodes(j).Nodes.Add("Move", "Move", 20)
                            Else
                        view.tusers.Nodes(1).Nodes(i).Nodes(2).Nodes(j).Nodes.Add("Move", "Move", 6)
                            End If
                            If rfound = True Then
                        view.tusers.Nodes(1).Nodes(i).Nodes(2).Nodes(j).Nodes.Add("Read", "Read", 20)
                            Else
                        view.tusers.Nodes(1).Nodes(i).Nodes(2).Nodes(j).Nodes.Add("Read", "Read", 6)
                            End If
                            If vfound = True Then
                        view.tusers.Nodes(1).Nodes(i).Nodes(2).Nodes(j).Nodes.Add("View", "View", 20)
                            Else
                        view.tusers.Nodes(1).Nodes(i).Nodes(2).Nodes(j).Nodes.Add("View", "View", 6)
                            End If
                            If wfound = True Then
                        view.tusers.Nodes(1).Nodes(i).Nodes(2).Nodes(j).Nodes.Add("Write", "Write", 20)
                            Else
                        view.tusers.Nodes(1).Nodes(i).Nodes(2).Nodes(j).Nodes.Add("Write", "Write", 6)
                    End If
                    If lfound = True Then
                        view.tusers.Nodes(1).Nodes(i).Nodes(2).Nodes(j).Nodes.Add("Locked", "Locked", 20)
                    Else
                        view.tusers.Nodes(1).Nodes(i).Nodes(2).Nodes(j).Nodes.Add("Locked", "Locked", 6)
                    End If
                    If afound = True Then
                        view.tusers.Nodes(1).Nodes(i).Nodes(2).Nodes(j).Nodes.Add("Author Only", "Author Only", 20)
                    Else
                        view.tusers.Nodes(1).Nodes(i).Nodes(2).Nodes(j).Nodes.Add("Author Only", "Author Only", 6)
                    End If
                    If dfound = True Then
                        view.tusers.Nodes(1).Nodes(i).Nodes(2).Nodes(j).Nodes.Add("Disclosures", "Disclosures", 20)
                    Else
                        view.tusers.Nodes(1).Nodes(i).Nodes(2).Nodes(j).Nodes.Add("Disclosures", "Disclosures", 6)
                    End If
                    If efound = True Then
                        view.tusers.Nodes(1).Nodes(i).Nodes(2).Nodes(j).Nodes.Add("Email Into Stage", "Email Into Stage", 20)
                    Else
                        view.tusers.Nodes(1).Nodes(i).Nodes(2).Nodes(j).Nodes.Add("Email Into Stage", "Email Into Stage", 6)
                    End If
                       
                Next

            Next
            view.tusers.Nodes.Add("Users by Office", "Users by Office", users_icon)
            For i = 0 To Me.container.oAdmin.offices.Count - 1
                If Me.container.oAdmin.offices(i).inactive = 0 Then
                    view.tusers.Nodes(2).Nodes.Add(Me.container.oAdmin.offices(i).description, Me.container.oAdmin.offices(i).description, folder_icon)
                Else
                    view.tusers.Nodes(2).Nodes.Add(Me.container.oAdmin.offices(i).description + " (inactive)", Me.container.oAdmin.offices(i).description + " (inactive)", deactivate_icon)
                End If
            Next
            view.tusers.Nodes.Add("Users by Business Area", "Users by Business Area", users_icon)
            For i = 0 To Me.container.oAdmin.business_areas.Count - 1
                If Me.container.oAdmin.business_areas(i).inactive = 0 Then
                    view.tusers.Nodes(3).Nodes.Add(Me.container.oAdmin.business_areas(i).description, Me.container.oAdmin.business_areas(i).description, folder_icon)
                Else
                    view.tusers.Nodes(3).Nodes.Add(Me.container.oAdmin.business_areas(i).description + " (inactive)", Me.container.oAdmin.business_areas(i).description + " (inactive)", deactivate_icon)
                End If
            Next
            view.tusers.Nodes.Add("Users by Language", "Users by Language", users_icon)
            For i = 0 To Me.container.oAdmin.languages.Count - 1
                If Me.container.oAdmin.languages(i).inactive = 0 Then
                    view.tusers.Nodes(4).Nodes.Add(Me.container.oAdmin.languages(i).language_name, Me.container.oAdmin.languages(i).language_name, folder_icon)
                Else
                    view.tusers.Nodes(4).Nodes.Add(Me.container.oAdmin.languages(i).language_name + " (inactive)", Me.container.oAdmin.languages(i).language_name + " (inactive)", deactivate_icon)
                End If
            Next

            view.tusers.Nodes.Add("User Preference Types", "User Preference Types", users_icon)
            For i = 0 To Me.container.oUsers.pref_types.Count - 1
                view.tusers.Nodes(5).Nodes.Add(Me.container.oUsers.pref_types(i).name, Me.container.oUsers.pref_types(i).name, folder_icon)
            Next


            view.tusers.Nodes(0).Expand()
            view.tusers.SelectedNode = view.tusers.Nodes(0)



        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_users", "load_users_tree", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try
    End Sub
    Public Sub del_pref_type()
        For i = 0 To Me.container.oUsers.pref_types.Count - 1
            If Me.container.oUsers.pref_types(i).name = view.tusers.SelectedNode.Text Then
                Me.container.oUsers.pref_types(i).write_mode = bc_om_users.bc_om_preference_type.DELETE
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    Me.container.oUsers.pref_types(i).db_Write()
                Else
                    Me.container.oUsers.pref_types(i).tmode = bc_cs_soap_base_class.tWRITE
                    If Me.container.oUsers.pref_types(i).transmit_to_server_and_receive(Me.container.oUsers.pref_types(i), True) = False Then
                        Exit Sub
                    End If
                End If
                If Me.container.oUsers.pref_types(i).delete_err <> "success" Then
                    Dim oerr As New bc_cs_message("Blue Curve", "Cannot delete preference type: " + Me.container.oUsers.pref_types(i).delete_err, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub
                End If
                Me.container.oUsers.pref_types.RemoveAt(i)
                Me.load_users_tree()
                view.tusers.SelectedNode = view.tusers.Nodes(5)
                view.tusers.SelectedNode.Expand()
                Exit For

            End If

        Next
    End Sub

    Public Sub add_pref_type()
        Dim fedit As New bc_am_cp_edit
        fedit.ltitle.Text = "Enter preference type"
        fedit.ShowDialog()
        If fedit.cancel_selected = False Then

            For i = 0 To container.oUsers.pref_types.Count - 1
                If UCase(container.oUsers.pref_types(i).name) = UCase(fedit.Tentry.Text) Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Preference type already exists: " + fedit.Tentry.Text, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub
                End If

            Next
            Dim opref As New bc_om_users.bc_om_preference_type
            opref.name = fedit.Tentry.Text
            opref.write_mode = bc_om_users.bc_om_preference_type.ADD

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                opref.db_write()
                container.oUsers.db_read()

            Else
                REM PR AUG7
                opref.tmode = bc_cs_soap_base_class.tWRITE
                If opref.transmit_to_server_and_receive(opref, True) = False Then
                    Exit Sub
                End If
                container.oUsers.tmode = bc_cs_soap_base_class.tREAD
                If container.oUsers.transmit_to_server_and_receive(container.oUsers, True) = False Then
                    Exit Sub
                End If
            End If
            load_users_tree()
            view.tusers.SelectedNode = view.tusers.Nodes(5)
            view.tusers.SelectedNode.ExpandAll()

        End If
    End Sub

    Public Sub rename_pref_type()
        Dim idx As Integer
        Dim fedit As New bc_am_cp_edit

        For i = 0 To container.oUsers.pref_types.Count - 1
            If container.oUsers.pref_types(i).name = view.tusers.SelectedNode.Text Then
                fedit.ltitle.Text = "Change " + container.oUsers.pref_types(i).name
                idx = i
                Exit For
            End If
        Next

        fedit.ShowDialog()
        If fedit.cancel_selected = False Then

            For i = 0 To container.oUsers.pref_types.Count - 1
                If UCase(container.oUsers.pref_types(i).name) = UCase(fedit.Tentry.Text) Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Preference type already exists: " + fedit.Tentry.Text, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub
                End If

            Next
            Dim opref As New bc_om_users.bc_om_preference_type
            opref.id = container.oUsers.pref_types(idx).id
            opref.name = fedit.Tentry.Text
            opref.write_mode = bc_om_users.bc_om_preference_type.RENAME


            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                opref.db_write()

            Else
                opref.tmode = bc_cs_soap_base_class.tWRITE

                If opref.transmit_to_server_and_receive(opref, True) = False Then
                    Exit Sub
                End If

            End If
            container.oUsers.pref_types(idx).name = opref.name
            load_users_tree()
            view.tusers.SelectedNode = view.tusers.Nodes(5)
            view.tusers.SelectedNode.ExpandAll()

        End If
    End Sub
    Public Sub load_node_tree()
        Dim otrace As New bc_cs_activity_log("bc_am_users", "load_node_tree", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try

            Dim first As Boolean
            Dim ent_list As New ArrayList
            Dim icon_list As New ArrayList
            Dim ord_list As New ArrayList
            view.tusers.ContextMenuStrip = Nothing
            view.tentities.Nodes.Clear()
            view.tentities.Nodes.Add("Business Areas", "Business Areas", folder_icon)
            For i = 0 To Me.current_user.bus_areas.Count - 1
                For j = 0 To Me.container.oAdmin.business_areas.Count - 1
                    If Me.current_user.bus_areas(i) = Me.container.oAdmin.business_areas(j).id Then
                        If Me.container.oAdmin.business_areas(j).inactive = False Then
                            view.tentities.Nodes(0).Nodes.Add(Me.container.oAdmin.business_areas(j).description, Me.container.oAdmin.business_areas(j).description, links_icon)
                        Else
                            view.tentities.Nodes(0).Nodes.Add(Me.container.oAdmin.business_areas(j).description + " (inactive)", Me.container.oAdmin.business_areas(j).description + " (inactive)", deactivate_icon)
                        End If
                    End If
                Next
            Next

            REM SIM MAY 2013

         
            view.tentities.Nodes.Add("User Associations", "User Associations", folder_icon)
            Dim uname As String = ""
            For i = 0 To Me.current_user.associated_users.Count - 1
                For j = 0 To Me.container.oUsers.user.Count - 1
                    If Me.current_user.associated_users(i) = Me.container.oUsers.user(j).id Then
                        If container.oPubtypes.process_switches.surname_first = True Then
                            uname = Me.container.oUsers.user(j).surname + ", " + Me.container.oUsers.user(j).first_name
                        Else
                            uname = Me.container.oUsers.user(j).first_name + " " + Me.container.oUsers.user(j).surname
                        End If
                        If Me.container.oUsers.user(j).inactive = False Then
                            view.tentities.Nodes(1).Nodes.Add(uname, uname, links_icon)
                        Else
                            view.tentities.Nodes(1).Nodes.Add(uname + " (inactive)", uname + " (inactive)", deactivate_icon)
                        End If
                    End If
                Next
            Next

            REM SIM MAY 2013

            view.tentities.Nodes.Add("Other Roles", "Other Roles", folder_icon)
            For i = 0 To Me.current_user.other_roles.Count - 1

                For j = 0 To Me.container.oAdmin.roles.Count - 1
                    If Me.current_user.other_roles(i) = Me.container.oAdmin.roles(j).id Then
                        view.tentities.Nodes(2).Nodes.Add(Me.container.oAdmin.roles(j).description, Me.container.oAdmin.roles(j).description, links_icon)
                    End If
                Next
            Next
            

            REM more than one preferance type
            Dim n, nn As TreeNode
            n = Nothing
            Dim type_count As Integer
            type_count = view.tentities.Nodes.Count
            For pp = 0 To Me.container.oUsers.pref_types.Count - 1
                n = view.tentities.Nodes.Add(Me.container.oUsers.pref_types(pp).name, Me.container.oUsers.pref_types(pp).name, links_icon)
                n.Tag = Me.container.oUsers.pref_types(pp).id
            Next


            Dim pclass As String = ""
            Dim pentity As String = ""

            REM FIL 5.5 make sure the list is sorted
            If Me.current_user.sort = True Then
                sort_current_user_prefs(Me.current_user.prefs)
            Else
                Me.current_user.sort = True
            End If
            Dim prev_pref_type As String = ""
            Dim new_type As Boolean = True
            type_count = 2
            For i = 0 To Me.current_user.prefs.Count - 1
                new_type = False

                If prev_pref_type = "" Or prev_pref_type <> Me.current_user.prefs(i).pref_name Then
                    type_count = 2 + Me.current_user.prefs(i).pref_type
                    new_type = True
                End If
                If pclass = "" Or pclass <> Me.current_user.prefs(i).class_name Or new_type = True Then
                    n = view.tentities.Nodes(type_count).Nodes.Add(Me.current_user.prefs(i).class_name, Me.current_user.prefs(i).class_name, folder_icon)
                End If
                If Me.current_user.prefs(i).inactive = 0 Then
                    nn = n.Nodes.Add(Me.current_user.prefs(i).entity_name + " (" + CStr(Me.current_user.prefs(i).rating) + ")", Me.current_user.prefs(i).entity_name, links_icon)
                Else
                    nn = n.Nodes.Add(Me.current_user.prefs(i).entity_name + " (" + CStr(Me.current_user.prefs(i).rating) + ")", Me.current_user.prefs(i).entity_name + " (inactive)", deactivate_icon)
                End If
                nn.Tag = Me.current_user.prefs(i).entity_id

                For j = 0 To Me.current_user.prefs(i).users.Count - 1
                    If container.oPubtypes.process_switches.surname_first = True Then
                        If Me.current_user.prefs(i).users(j).inactive = True Then
                            nn.Nodes.Add(CStr(j + 1) + ". " + Me.current_user.prefs(i).users(j).surname + ", " + Me.current_user.prefs(i).users(j).surname, CStr(j + 1) + ". " + Me.current_user.prefs(i).users(j).surname + ", " + Me.current_user.prefs(i).users(j).first_name, deactivate_icon)

                        Else
                            If Trim(Me.current_user.prefs(i).users(j).surname + ", " + Me.current_user.prefs(i).users(j).first_name) = view.Lusers.SelectedItems(0).Text Then
                                nn.Nodes.Add(CStr(j + 1) + ". " + Me.current_user.prefs(i).users(j).surname + ", " + Me.current_user.prefs(i).users(j).first_name, CStr(j + 1) + ". " + Me.current_user.prefs(i).users(j).surname + ", " + Me.current_user.prefs(i).users(j).first_name, user_icon)
                            Else
                                nn.Nodes.Add(CStr(j + 1) + ". " + Me.current_user.prefs(i).users(j).surname + ", " + Me.current_user.prefs(i).users(j).first_name, CStr(j + 1) + ". " + Me.current_user.prefs(i).users(j).surname + ", " + Me.current_user.prefs(i).users(j).first_name, links_icon)
                            End If
                        End If
                    Else
                        If Me.current_user.prefs(i).users(j).inactive = True Then
                            nn.Nodes.Add(CStr(j + 1) + ". " + Me.current_user.prefs(i).users(j).first_name + " " + Me.current_user.prefs(i).users(j).surname, CStr(j + 1) + ". " + Me.current_user.prefs(i).users(j).first_name + " " + Me.current_user.prefs(i).users(j).surname, deactivate_icon)

                        Else
                            If Trim(Me.current_user.prefs(i).users(j).first_name + " " + Me.current_user.prefs(i).users(j).surname) = view.Lusers.SelectedItems(0).Text Then
                                nn.Nodes.Add(CStr(j + 1) + ". " + Me.current_user.prefs(i).users(j).first_name + " " + Me.current_user.prefs(i).users(j).surname, CStr(j + 1) + ". " + Me.current_user.prefs(i).users(j).first_name + " " + Me.current_user.prefs(i).users(j).surname, user_icon)
                            Else
                                nn.Nodes.Add(CStr(j + 1) + ". " + Me.current_user.prefs(i).users(j).first_name + " " + Me.current_user.prefs(i).users(j).surname, CStr(j + 1) + ". " + Me.current_user.prefs(i).users(j).first_name + " " + Me.current_user.prefs(i).users(j).surname, links_icon)
                            End If
                        End If
                    End If
                    nn.Nodes(j).Tag = Me.current_user.prefs(i).users(j).id

                Next


                pclass = Me.current_user.prefs(i).class_name
                prev_pref_type = Me.current_user.prefs(i).pref_name
            Next

            'End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_users", "load_node_tree", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            otrace = New bc_cs_activity_log("bc_am_users", "load_node_tree", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
        'view.tentities.ExpandAll()
    End Sub
    Private Sub data_error()
        MsgBox("yep")
    End Sub
    Private Sub sort_current_user_prefs(ByRef prefs As List(Of bc_om_user_pref))
        Try
            Dim t As New bc_om_user_pref
            Dim u As New bc_om_user
            REM preferance type
            For i = 0 To prefs.Count - 1
                For j = i + 1 To prefs.Count - 1
                    If prefs(i).pref_name > prefs(j).pref_name Then
                        t = prefs(i)
                        prefs(i) = prefs(j)
                        prefs(j) = t
                    End If
                Next
            Next
            REM class name
            For i = 0 To prefs.Count - 1
                For j = i + 1 To prefs.Count - 1
                    If prefs(i).class_name > prefs(j).class_name And prefs(i).pref_name = prefs(j).pref_name Then
                        t = prefs(i)
                        prefs(i) = prefs(j)
                        prefs(j) = t
                    End If
                Next
            Next
            REM entity name name
            For i = 0 To prefs.Count - 1
                For j = i + 1 To prefs.Count - 1
                    If prefs(i).entity_name > prefs(j).entity_name And prefs(i).class_name = prefs(j).class_name And prefs(i).pref_name = prefs(j).pref_name Then
                        t = prefs(i)
                        prefs(i) = prefs(j)
                        prefs(j) = t
                    End If
                Next
            Next

            REM user rating 
            For k = 0 To prefs.Count - 1
                For i = 0 To prefs(k).users.Count - 1
                    For j = i + 1 To prefs(k).users.Count - 1
                        If prefs(k).users(i).rating > prefs(k).users(j).rating Then
                            u = prefs(k).users(i)
                            prefs(k).users(i) = prefs(k).users(j)
                            prefs(k).users(j) = u
                        End If
                    Next

                Next


            Next

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_users", "sort_current_user_prefs", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub

    Private Sub load_user_attributes()

        Try


            user_attributes.Clear()

            view.DataGridView1.Rows.Clear()

            Dim oatt As bc_om_attribute_value
            For i = 0 To Me.container.oUsers.user_display_attributes.user_display_attributes.Count - 1
                If Me.container.oUsers.user_display_attributes.user_display_attributes(i).attribute_id = 0 Then
                    For j = 0 To Me.container.oUsers.user_system_attributes.Count - 1
                        If Me.container.oUsers.user_system_attributes(j).field_name = Me.container.oUsers.user_display_attributes.user_display_attributes(i).name Then
                            oatt = New bc_om_attribute_value
                            oatt.attribute_Id = 0
                            oatt.value = ""
                            oatt.field_name = Me.container.oUsers.user_system_attributes(j).field_name
                            oatt.nullable = True
                            oatt.name = Me.container.oUsers.user_system_attributes(j).display_name
                            oatt.add_new = Me.container.oUsers.user_system_attributes(j).add_new
                            If Me.container.oUsers.user_system_attributes(j).mandatory = True Then
                                oatt.nullable = False
                            End If
                            'If Me.container.oUsers.user_system_attributes(j).lookup_vals.Count > 0 Then
                            '    oatt.is_lookup = True
                            '    For k = 0 To Me.container.oUsers.user_system_attributes(j).lookup_vals.Count - 1
                            '        oatt.lookup_values.Add(Me.container.oUsers.user_system_attributes(j).lookup_vals(k))
                            '    Next
                            'End If

                            view.DataGridView1.Rows.Add()
                            view.DataGridView1.Item(1, view.DataGridView1.Rows.Count - 1).Value = oatt.name
                            If oatt.nullable = True Then
                                view.DataGridView1.Item(0, view.DataGridView1.Rows.Count - 1).Value = New Drawing.Bitmap(1, 1)
                            Else
                                view.DataGridView1.Item(0, view.DataGridView1.Rows.Count - 1).Value = view.tabimages.Images(mandatory_icon)
                            End If
                            Select Case Me.container.oUsers.user_system_attributes(j).type_id
                                Case 1
                                    view.DataGridView1.Item(2, view.DataGridView1.Rows.Count - 1).Value = view.tabimages.Images(string_icon)
                                Case 2
                                    view.DataGridView1.Item(2, view.DataGridView1.Rows.Count - 1).Value = view.tabimages.Images(number_icon)
                                Case 3
                                    view.DataGridView1.Item(2, view.DataGridView1.Rows.Count - 1).Value = view.tabimages.Images(boolean_icon)
                                Case 5
                                    view.DataGridView1.Item(2, view.DataGridView1.Rows.Count - 1).Value = view.tabimages.Images(date_icon)
                                Case Else
                                    view.DataGridView1.Item(2, view.DataGridView1.Rows.Count - 1).Value = view.tabimages.Images(string_icon)

                            End Select
                            If oatt.add_new = "" Then
                                view.DataGridView1.Item(4, view.DataGridView1.Rows.Count - 1).Value = New Drawing.Bitmap(1, 1)
                            Else
                                view.DataGridView1.Item(4, view.DataGridView1.Rows.Count - 1).Value = view.tabimages.Images(new_icon)
                            End If
                            Me.user_attributes.Add(oatt)

                            Exit For
                        End If
                    Next
                Else
                    REM dynamic attribute
                    For j = 0 To Me.container.oClasses.attribute_pool.Count - 1
                        If Me.container.oClasses.attribute_pool(j).attribute_id = Me.container.oUsers.user_display_attributes.user_display_attributes(i).attribute_id Then
                            oatt = New bc_om_attribute_value
                            oatt.attribute_Id = Me.container.oClasses.attribute_pool(j).attribute_id
                            oatt.nullable = True
                            oatt.name = Me.container.oClasses.attribute_pool(j).name
                            oatt.add_new = ""
                            oatt.submission_code = Me.container.oClasses.attribute_pool(j).submission_code

                            If Me.container.oClasses.attribute_pool(j).nullable = False Then
                                oatt.nullable = False
                            End If
                            'If Me.container.oUsers.user_system_attributes(j).lookup_vals.Count > 0 Then
                            '    oatt.is_lookup = True
                            '    For k = 0 To Me.container.oUsers.user_system_attributes(j).lookup_vals.Count - 1
                            '        oatt.lookup_values.Add(Me.container.oUsers.user_system_attributes(j).lookup_vals(k))
                            '    Next
                            'End If

                            view.DataGridView1.Rows.Add()
                            view.DataGridView1.Item(1, view.DataGridView1.Rows.Count - 1).Value = oatt.name
                            If oatt.nullable = True Then
                                view.DataGridView1.Item(0, view.DataGridView1.Rows.Count - 1).Value = New Drawing.Bitmap(1, 1)
                            Else
                                view.DataGridView1.Item(0, view.DataGridView1.Rows.Count - 1).Value = view.tabimages.Images(mandatory_icon)
                            End If
                            Select Case Me.container.oClasses.attribute_pool(j).type_id
                                Case 1
                                    view.DataGridView1.Item(2, view.DataGridView1.Rows.Count - 1).Value = view.tabimages.Images(string_icon)
                                Case 2
                                    view.DataGridView1.Item(2, view.DataGridView1.Rows.Count - 1).Value = view.tabimages.Images(number_icon)
                                Case 3
                                    view.DataGridView1.Item(2, view.DataGridView1.Rows.Count - 1).Value = view.tabimages.Images(boolean_icon)
                                Case 5
                                    view.DataGridView1.Item(2, view.DataGridView1.Rows.Count - 1).Value = view.tabimages.Images(date_icon)
                                Case Else
                                    view.DataGridView1.Item(2, view.DataGridView1.Rows.Count - 1).Value = view.tabimages.Images(string_icon)


                            End Select
                            If oatt.add_new = "" Then
                                view.DataGridView1.Item(4, view.DataGridView1.Rows.Count - 1).Value = New Drawing.Bitmap(1, 1)
                            Else
                                view.DataGridView1.Item(4, view.DataGridView1.Rows.Count - 1).Value = view.tabimages.Images(new_icon)
                            End If
                            Me.user_attributes.Add(oatt)
                            If Me.container.oClasses.attribute_pool(j).is_def = True Then
                                view.DataGridView1.Item(4, view.DataGridView1.Rows.Count - 1).Value = view.tabimages.Images(34)
                            End If
                            Exit For
                        End If
                    Next

                    view.DataGridView1.Visible = True


                End If
            Next





            REM FIL JUN 2013
            REM set up pending sync control

            Dim lu As DataGridViewComboBoxCell


            'AddHandler CType(lu, DataGridViewComboBoxCell).ErrorText, AddressOf data_error
            If view.dpartialsync.Rows.Count = 0 Then


                view.dpartialsync.Rows.Add()
                view.dpartialsync.Rows.Add()
                view.dpartialsync.Rows.Add()


                view.dpartialsync.Item(0, 0).Value = "Templates"
                view.dpartialsync.Item(0, 1).Value = "Publications"
                view.dpartialsync.Item(0, 2).Value = "Entities"


                lu = New DataGridViewComboBoxCell
                lu.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing
                lu.Items.Add("True")
                lu.Items.Add("False")

                view.dpartialsync.Item(1, 0) = lu

                lu = New DataGridViewComboBoxCell
                lu.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing
                lu.Items.Add("True")
                lu.Items.Add("False")

                view.dpartialsync.Item(1, 1) = lu

                lu = New DataGridViewComboBoxCell
                lu.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing
                lu.Items.Add("True")
                lu.Items.Add("False")

                view.dpartialsync.Item(1, 2) = lu
                view.dpartialsync.Item(1, 0).Value = "False"
                view.dpartialsync.Item(1, 1).Value = "False"
                view.dpartialsync.Item(1, 2).Value = "False"
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_users", "load_user_attributes", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
        End Try

    End Sub
    Private Sub clear_user_details()
        view.puser.Enabled = False
        view.Ttitle.TabPages(0).ImageIndex = deactivate_icon
        view.Ttitle.TabPages(0).Text = "no selection"
        view.tentities.Nodes.Clear()

        For i = 0 To view.DataGridView1.Rows.Count - 1
            view.DataGridView1.Item(3, i).Value = ""

            'password 
            view.DataGridView1.Item(3, i).Tag = ""

        Next
        view.DataGridView1.Enabled = False
        view.Pnouser.Visible = True
        view.puserpic.Visible = False
        'view.centities.Checked = False
        'view.Cpublications.Checked = False
        'view.cphysical_templates.Checked = False


        view.Refresh()
    End Sub
    Private Sub load_users(ByVal mode As Integer, ByVal inactive As Boolean, Optional ByVal filter As String = "")
        Try
            view.Cursor = Cursors.WaitCursor


            clear_user_details()
            view.Lusers.Items.Clear()
            Dim na As String
            Dim found As Boolean
            Dim ntx As String = ""
            view.minactive.Enabled = False
            view.DeleteEntityToolStripMenuItem.Enabled = False
            Try
                ntx = view.tusers.SelectedNode.Text
                If InStr(ntx, "(inactive)") > 0 Then
                    ntx = ntx.Substring(0, ntx.Length - 11)
                End If
            Catch

            End Try

            REM FIL JUNE 2013
            If mode = 10 Or mode = 11 Or mode = 12 Then
                container.oUsers.resetsync = False
                container.oUsers.read_partial_sync = False
                If container.oAdmin.sync_types.sync_types.Count > 0 Then
                    container.oUsers.read_partial_sync = True
                End If


                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    container.oUsers.db_read()
                Else
                    container.oUsers.tmode = bc_cs_soap_base_class.tREAD
                    If container.oUsers.transmit_to_server_and_receive(container.oUsers, True) = False Then
                        Exit Sub
                    End If
                End If
                container.oUsers.read_partial_sync = False
            End If

            For i = 0 To container.oUsers.user.Count - 1
                If container.oPubtypes.process_switches.surname_first = True Then

                    na = Trim(container.oUsers.user(i).surname) + ", " + Trim(container.oUsers.user(i).first_name)
                Else

                    na = Trim(container.oUsers.user(i).first_name) + " " + Trim(container.oUsers.user(i).surname)
                End If
                found = False
                If filter <> "" Then
                    If view.tfilter.SearchText.Length <= na.Length Then
                        If InStr(UCase(na), UCase(view.tfilter.SearchText)) > 0 Then
                            found = True
                        End If
                    End If
                Else
                    found = True
                End If
                If found = True Then
                    If mode = 0 Then
                        If container.oUsers.user(i).inactive = False Then
                            view.Lusers.Items.Add(na, user_icon)
                        Else
                            view.Lusers.Items.Add(na + " (inactive)", deactivate_icon)
                        End If
                    ElseIf mode = 1 Then
                        If container.oUsers.user(i).inactive = inactive Then
                            If inactive = False Then
                                view.Lusers.Items.Add(na, user_icon)
                            Else
                                view.Lusers.Items.Add(na + " (inactive)", deactivate_icon)
                            End If
                        End If
                    ElseIf mode = 2 Then
                        REM by role
                        For j = 0 To container.oAdmin.roles.Count - 1
                            If container.oUsers.user(i).role = ntx Then
                                If container.oUsers.user(i).inactive = False Then
                                    view.Lusers.Items.Add(na, user_icon)
                                Else
                                    view.Lusers.Items.Add(na + " (inactive)", deactivate_icon)
                                End If
                                Exit For
                            End If
                        Next
                    ElseIf mode = 3 Then
                        REM by office
                        For j = 0 To container.oAdmin.offices.Count - 1
                            If container.oAdmin.offices(j).description = ntx Then
                                If container.oUsers.user(i).office_id = container.oAdmin.offices(j).id Then
                                    If container.oUsers.user(i).inactive = False Then
                                        view.Lusers.Items.Add(na, user_icon)
                                    Else
                                        view.Lusers.Items.Add(na + " (inactive)", deactivate_icon)
                                    End If
                                End If
                                Exit For
                            End If
                        Next
                    ElseIf mode = 4 Then
                        For j = 0 To container.oAdmin.business_areas.Count - 1
                            If container.oAdmin.business_areas(j).description = ntx Then
                                For k = 0 To container.oAdmin.user_bus_areas.Count - 1
                                    If container.oAdmin.user_bus_areas(k).bus_area_id = container.oAdmin.business_areas(j).id Then
                                        If container.oUsers.user(i).id = container.oAdmin.user_bus_areas(k).user_id Then
                                            If container.oUsers.user(i).inactive = False Then
                                                view.Lusers.Items.Add(na, user_icon)
                                            Else
                                                view.Lusers.Items.Add(na + " (inactive)", deactivate_icon)
                                            End If
                                            Exit For
                                        End If
                                    End If
                                Next
                                Exit For
                            End If
                        Next
                    ElseIf mode = 5 Then
                        REM by role
                        For j = 0 To container.oAdmin.languages.Count - 1
                            If container.oAdmin.languages(j).language_name = ntx Then
                                If container.oUsers.user(i).language_id = container.oAdmin.languages(j).language_id Then
                                    If container.oUsers.user(i).inactive = False Then
                                        view.Lusers.Items.Add(na, user_icon)
                                    Else
                                        view.Lusers.Items.Add(na + " (inactive)", deactivate_icon)
                                    End If
                                    Exit For
                                End If
                            End If
                        Next
                    ElseIf mode = 10 Then

                        If container.oAdmin.sync_types.sync_types.Count = 0 Then
                            If container.oUsers.user(i).sync_level = 0 Then
                                If container.oUsers.user(i).inactive = False Then
                                    view.Lusers.Items.Add(na, user_icon)
                                Else
                                    view.Lusers.Items.Add(na + " (inactive)", deactivate_icon)
                                End If
                            End If
                        Else
                            If container.oUsers.user(i).sync_settings.count = 0 Then
                                If container.oUsers.user(i).inactive = False Then
                                    view.Lusers.Items.Add(na, user_icon)
                                Else
                                    view.Lusers.Items.Add(na + " (inactive)", deactivate_icon)
                                End If
                            End If

                        End If
                    ElseIf mode = 11 Then
                        If container.oAdmin.sync_types.sync_types.Count = 0 Then
                            If container.oUsers.user(i).sync_level > 0 Then
                                If container.oUsers.user(i).inactive = False Then
                                    view.Lusers.Items.Add(na, user_icon)
                                Else
                                    view.Lusers.Items.Add(na + " (inactive)", deactivate_icon)
                                End If
                            End If
                        Else
                            If container.oUsers.user(i).sync_settings.count = 3 Then
                                If container.oUsers.user(i).inactive = False Then
                                    view.Lusers.Items.Add(na, user_icon)
                                Else
                                    view.Lusers.Items.Add(na + " (inactive)", deactivate_icon)
                                End If
                            End If

                        End If
                    ElseIf mode = 12 Then
                        If container.oUsers.user(i).sync_settings.count > 0 And container.oUsers.user(i).sync_settings.count < 3 Then
                            If container.oUsers.user(i).inactive = False Then
                                view.Lusers.Items.Add(na, user_icon)
                            Else
                                view.Lusers.Items.Add(na + " (inactive)", deactivate_icon)
                            End If
                        End If

                    End If


                End If
            Next
            load_user_toolbar()
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_cp_users", "Load_users", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            view.Cursor = Cursors.Default
        End Try
    End Sub
    Public Sub user_mode(ByVal filter As String)
        Dim tx As String

        If view.tusers.SelectedNode.Text = "All Users" Then
            load_users(0, False, filter)
        ElseIf view.tusers.SelectedNode.Text = "Active" Then
            load_users(1, False, filter)
        ElseIf view.tusers.SelectedNode.Text = "Synced Users" Then
            load_users(10, False, filter)
        ElseIf view.tusers.SelectedNode.Text = "Unsynced Users" Then
            load_users(11, False, filter)
            REM FIL JUN 2013
        ElseIf view.tusers.SelectedNode.Text = "Partially Synced Users" Then
            load_users(12, False, filter)
        ElseIf view.tusers.SelectedNode.Text = "Inactive" Then
            load_users(1, True, filter)
        Else
            Try
                tx = view.tusers.SelectedNode.Parent.Text
                If tx = "Users by Security Role" Then
                    load_users(2, 0, filter)
                End If
                If tx = "Users by Office" Then
                    load_users(3, 0, filter)
                End If
                If tx = "Users by Business Area" Then
                    load_users(4, 0, filter)
                End If
                If tx = "Users by Language" Then
                    load_users(5, 0, filter)
                End If
            Catch ex As Exception
                view.Lusers.Items.Clear()
            End Try
        End If

    End Sub
    Public Sub commit_changes()

        Dim ousers As New bc_om_users
        Dim key As String

        Try
            If check_warnings() = False Then
                Dim omsg As New bc_cs_message("Blue Curve", "User cannot be submitted as missing attributes/associations please check warnings", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Sub
            End If

            Dim new_role As Boolean = False
            view.Cursor = Cursors.WaitCursor
            Dim i, l As Integer
            REM insert any new language, role or office added
            With container.oAdmin.languages(container.oAdmin.languages.Count - 1)
                If .language_id = -1 Then
                    .write_mode = bc_om_user_language.INSERT
                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        .db_write()
                    Else
                        .tmode = bc_om_user_language.tWRITE
                        If .transmit_to_server_and_receive(container.oAdmin.languages(container.oAdmin.languages.Count - 1), True) = False Then
                            Exit Sub
                        End If
                    End If
                    key = container.oAdmin.languages(container.oAdmin.languages.Count - 1).language_id
                    align_key_for_value("language_id", key)
                End If
            End With
            REM SIM MAY 2013
            Dim other_role_found As Boolean = False
            For i = 0 To container.oAdmin.roles.Count - 1
                With container.oAdmin.roles(i)
                    If .id <= 0 Then
                        Dim j As Long
                        other_role_found = False
                        For j = 0 To current_user.other_roles.Count - 1
                            If current_user.other_roles(j) = .id Then
                                other_role_found = True
                                Exit For
                            End If
                        Next

                        .write_mode = bc_om_user_role.INSERT
                        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                            .db_write()
                        Else
                            .tmode = bc_om_user_role.tWRITE
                            If .transmit_to_server_and_receive(container.oAdmin.roles(container.oAdmin.roles.Count - 1), True) = False Then
                                Exit Sub
                            End If
                        End If
                        If other_role_found = True Then
                            current_user.other_roles(j) = container.oAdmin.roles(container.oAdmin.roles.Count - 1).id
                            new_role = True
                        Else
                            REM update user lookup values 
                            key = container.oAdmin.roles(i).id.ToString
                            align_key_for_value("role_id", key)
                        End If
                        new_role = True
                    End If
                End With
            Next

            With container.oAdmin.offices(container.oAdmin.offices.Count - 1)
                If .id = -1 Then
                    .write_mode = bc_om_user_office.INSERT
                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        .db_write()
                    Else
                        .tmode = bc_om_user_office.tWRITE
                        If .transmit_to_server_and_receive(container.oAdmin.offices(container.oAdmin.offices.Count - 1), True) = False Then
                            Exit Sub
                        End If
                    End If
                    REM update user lookup values 
                    key = container.oAdmin.offices(container.oAdmin.offices.Count - 1).id.ToString
                    align_key_for_value("office_id", key)
                End If


            End With
            current_user.attribute_values.Clear()
            For i = 0 To Me.user_attributes.Count - 1
                current_user.attribute_values.Add(Me.user_attributes(i))
            Next

            REM assign attributes over to current user
            For j = 0 To user_attributes.Count - 1
                If user_attributes(j).attribute_id = 0 Then
                    If user_attributes(j).field_name = "first_name" Then
                        current_user.first_name = user_attributes(j).value
                    End If
                    If user_attributes(j).field_name = "surname" Then
                        current_user.surname = user_attributes(j).value
                    End If
                    If user_attributes(j).field_name = "password" Then
                        If user_attributes(j).value <> "**********" Then
                            Me.current_user.password = user_attributes(j).value

                            REM ousers.HashPassword(Me.current_user.password)
                            Dim osecurity As New bc_cs_security
                            Dim certificate As New bc_cs_security.certificate
                            Me.current_user.password = osecurity.HashPassword(Me.current_user.id, Me.current_user.password, certificate)

                            user_attributes(j).value = Me.current_user.password
                        End If
                    End If
                End If
            Next



            Me.current_user.sync_settings.Clear()



            If view.dpartialsync.Item(1, 0).Value = "True" Then
                Me.current_user.sync_settings.Add(1)
            End If
            If view.dpartialsync.Item(1, 1).Value = "True" Then
                Me.current_user.sync_settings.Add(2)
            End If
            If view.dpartialsync.Item(1, 2).Value = "True" Then
                Me.current_user.sync_settings.Add(3)
            End If

            If new_user = True Then
                Me.current_user.sync_level = 1
                Me.current_user.write_mode = bc_om_user.INSERT
            Else
                Me.current_user.write_mode = bc_om_user.UPDATE
            End If
            REM write new bus areas first
            For j = 0 To container.oAdmin.business_areas.Count - 1
                If container.oAdmin.business_areas(j).id < 0 Then
                    REM now assign system id to nes bus areas
                    For i = 0 To Me.current_user.bus_areas.Count - 1
                        If Me.current_user.bus_areas(i) = container.oAdmin.business_areas(j).id Then
                            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                                container.oAdmin.business_areas(j).db_write()
                            Else
                                container.oAdmin.business_areas(j).tmode = bc_om_user.tWRITE
                                If container.oAdmin.business_areas(j).transmit_to_server_and_receive(container.oAdmin.business_areas(j), True) = False Then
                                    Exit Sub
                                End If
                            End If
                            Me.current_user.bus_areas(i) = container.oAdmin.business_areas(j).id
                            Exit For
                        End If
                    Next
                End If
            Next
            Me.current_user.attribute_values.Clear()
            For i = 0 To Me.user_attributes.Count - 1
                Me.current_user.attribute_values.Add(Me.user_attributes(i))
            Next
            Me.current_user.submit_prefs.Clear()

            REM preferances
            Dim sp As bc_om_submit_preferanes_for_type
            Dim esp As bc_om_submit_preferanes_for_type.bc_om_submit_preferanes_for_entity
            For i = 3 To view.tentities.Nodes.Count - 1
                sp = New bc_om_submit_preferanes_for_type
                sp.pref_type_id = view.tentities.Nodes(i).Tag
               
                For j = 0 To view.tentities.Nodes(i).Nodes.Count - 1
                    For k = 0 To view.tentities.Nodes(i).Nodes(j).Nodes.Count - 1
                        esp = New bc_om_submit_preferanes_for_type.bc_om_submit_preferanes_for_entity
                        esp.entity_id = view.tentities.Nodes(i).Nodes(j).Nodes(k).Tag
                        For m = 0 To view.tentities.Nodes(i).Nodes(j).Nodes(k).Nodes.Count - 1
                            esp.users.Add(view.tentities.Nodes(i).Nodes(j).Nodes(k).Nodes(m).Tag)
                        Next
                        sp.entities.Add(esp)
                    Next
                Next
                Me.current_user.submit_prefs.Add(sp)
            Next


            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                Me.current_user.db_write()
            Else
                Me.current_user.tmode = bc_om_user.tWRITE
                If Me.current_user.transmit_to_server_and_receive(Me.current_user, True) = False Then
                    Exit Sub
                End If
            End If
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                Me.current_user.db_read()
            Else
                Me.current_user.tmode = bc_om_user.tREAD
                If Me.current_user.transmit_to_server_and_receive(Me.current_user, True) = False Then
                    Exit Sub
                End If
            End If

            REM copy over to user list
            For i = 0 To container.oUsers.user.Count - 1
                If container.oUsers.user(i).id = Me.current_user.id Then

                    container.oUsers.user(i) = Me.current_user
                    Exit For
                End If

            Next

            REM assign over bus areas for filter
            l = 0
            Try
                For i = 0 To container.oAdmin.user_bus_areas.Count - 1
                    If container.oAdmin.user_bus_areas(l).user_id = Me.current_user.id Then
                        container.oAdmin.user_bus_areas.RemoveAt(l)
                    Else
                        l = l + 1
                    End If
                Next
            Catch

            End Try
            REM now add new ones
            Dim ouser_busarea As bc_om_user_bus_area
            For i = 0 To Me.current_user.bus_areas.Count - 1
                REM if new then first eneter intio system
                ouser_busarea = New bc_om_user_bus_area
                ouser_busarea.user_id = Me.current_user.id
                ouser_busarea.bus_area_id = Me.current_user.bus_areas(i)
                container.oAdmin.user_bus_areas.Add(ouser_busarea)
            Next
            REM copy over to apreflist
            'i = 0
            'While i < container.oAdmin.user_prefs.Count
            '    If container.oAdmin.user_prefs(i).user_id = Me.current_user.id Then
            '        container.oAdmin.user_prefs.RemoveAt(i)
            '    Else
            '        i = i + 1
            '    End If
            'End While
            'For i = 0 To Me.current_user.prefs.Count - 1
            '    container.oAdmin.user_prefs.Add(Me.current_user.prefs(i))
            'Next

            REM now reload aprefs
            'Dim lprefs As New bc_om_all_prefs
            'If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
            '    lprefs.db_read()
            'Else
            '    lprefs.tmode = bc_om_user.tREAD
            '    If lprefs.transmit_to_server_and_receive(lprefs, True) = False Then
            '        Exit Sub
            '    End If
            'End If

            'container.oAdmin.user_prefs = lprefs.user_prefs


            view.bentchanges.Enabled = False
            view.bentdiscard.Enabled = False
            view.pusers.Enabled = True
            view.tusers.Enabled = True
            container.uxNavBar.Enabled = True
            Me.current_user.comment = Format(Now, "hh:mm:ss")
            Me.current_user.change_user = "me"
            REM this not implemented in db yet so set as unknown
            current_user.change_user = "unknown"
            If new_user = True Then
                container.oUsers.user.Add(Me.current_user)
                load_users_tree()
                load_users(0, False, "")
                new_user = False
                If container.oPubtypes.process_switches.surname_first = True Then
                    highlight_user(current_user.surname + ", " + current_user.first_name)
                Else
                    highlight_user(current_user.first_name + " " + current_user.surname)
                End If

                If new_role = False Then
                    Dim omsg As New bc_cs_message("Blue Curve", "User " + current_user.first_name + " " + current_user.surname + " Added.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Else
                    Dim omsg As New bc_cs_message("Blue Curve", "User " + current_user.first_name + " " + current_user.surname + " Added. You also added a new role so please set role access levels in navigator.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                End If
            Else
                load_users_tree()
                load_users(0, False, "")
                If container.oPubtypes.process_switches.surname_first = True Then
                    highlight_user(current_user.surname + ", " + current_user.first_name)
                Else
                    highlight_user(current_user.first_name + " " + current_user.surname)
                End If
                If new_role = False Then
                    Dim omsg As New bc_cs_message("Blue Curve", "User " + current_user.first_name + " " + current_user.surname + " Updated.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Else
                    Dim omsg As New bc_cs_message("Blue Curve", "User " + current_user.first_name + " " + current_user.surname + " Updated. You also added a new role so please set role access levels in navigator.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                End If
            End If
                container.uncomitted_data = False
                'container.set_sync = True





        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_users", "commit_changes", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            view.Cursor = Cursors.Default
        End Try
    End Sub
    Private Sub align_key_for_value(ByVal field_name As String, ByVal key As String)
        For i = 0 To Me.container.oUsers.user_system_attributes.Count - 1
            If Me.container.oUsers.user_system_attributes(i).field_name = field_name Then
                Me.container.oUsers.user_system_attributes(i).lookup_vals_key(Me.container.oUsers.user_system_attributes(i).lookup_vals.Count - 1) = key
                Exit For
            End If
        Next
        For i = 0 To Me.user_attributes.Count - 1
            If Me.user_attributes(i).field_name = field_name Then
                Me.user_attributes(i).value = key
            End If

        Next
    End Sub
    Public Sub data_grid_size()
        If view.DataGridView1.Columns(0).Visible = True Then
            'view.DataGridView1.Columns(0).Width = view.DataGridView1.Width * (0.5 / 10)
            view.DataGridView1.Columns(1).Width = view.DataGridView1.Width * (3.5 / 10)
            'view.DataGridView1.Columns(2).Width = view.DataGridView1.Width * (0.5 / 10)
            view.DataGridView1.Columns(3).Width = view.DataGridView1.Width * (3.5 / 10)
            'view.DataGridView1.Columns(4).Width = view.DataGridView1.Width * (1 / 10)
            'view.DataGridView1.Columns(5).Width = view.DataGridView1.Width * (2.8 / 10) - 42
        Else
            view.DataGridView1.Columns(1).Width = view.DataGridView1.Width * (1 / 3)
            view.DataGridView1.Columns(3).Width = view.DataGridView1.Width * (1 / 3)
            'view.DataGridView1.Columns(5).Width = view.DataGridView1.Width * (1 / 3) - 42
        End If
    End Sub

    Public Sub load_user()
        Try

            If Me.new_user = True Then
                Exit Sub
            End If
            Me.clear_user_details()
            view.tentities.ContextMenuStrip = Nothing
            If container.mode > 0 Then
                view.tentities.ContextMenuStrip = view.assignentity
            End If

            reset_warnings()
            Dim dtextbox As New DataGridViewTextBoxColumn
            dtextbox.Width = view.DataGridView1.Columns(3).Width
            dtextbox.MaxInputLength = 250
            view.DataGridView1.Columns.RemoveAt(3)
            dtextbox.Name = "Values"
            view.DataGridView1.Columns.Insert(3, dtextbox)
            If container.mode = 0 Or InStr(view.Lusers.SelectedItems(0).Text, "(inactive)") > 0 Then
                view.DataGridView1.Columns(3).ReadOnly = True
                view.tentities.ContextMenuStrip = Nothing
                view.DataGridView1.Columns(4).Visible = False
            Else
                view.DataGridView1.Columns(4).Visible = True

            End If

            current_user = New bc_om_user
            view.new_user = False
            Dim user_name As String = ""
            REM locate selected user
            If container.oPubtypes.process_switches.surname_first = True Then

                For i = 0 To container.oUsers.user.Count - 1
                    user_name = Trim(container.oUsers.user(i).surname) + ", " + Trim(container.oUsers.user(i).first_name)
                    If Trim(UCase(user_name)) = Trim(UCase(view.Lusers.SelectedItems(0).Text)) Or Trim(UCase(user_name)) + " (INACTIVE)" = Trim(UCase(view.Lusers.SelectedItems(0).Text)) Then
                        current_user = container.oUsers.user(i)
                        Exit For
                    End If
                Next
            Else
                For i = 0 To container.oUsers.user.Count - 1
                    user_name = Trim(container.oUsers.user(i).first_name) + " " + Trim(container.oUsers.user(i).surname)
                    If Trim(UCase(user_name)) = Trim(UCase(view.Lusers.SelectedItems(0).Text)) Or Trim(UCase(user_name)) + " (INACTIVE)" = Trim(UCase(view.Lusers.SelectedItems(0).Text)) Then
                        current_user = container.oUsers.user(i)
                        Exit For
                    End If
                Next
            End If
            current_user.picture_change = False


            If container.mode < 2 Then
                view.DeleteEntityToolStripMenuItem.Visible = False
            End If
            view.minactive.Enabled = True
            If Me.current_user.inactive = True Then
                view.minactive.Text = "Make " + user_name + " active"
                view.minactive.Image = view.tabimages.Images(active_icon)
                view.DeleteEntityToolStripMenuItem.Enabled = True
            Else
                view.minactive.Text = "Make " + user_name + " inactive"
                view.minactive.Image = view.tabimages.Images(deactivate_icon)
                view.DeleteEntityToolStripMenuItem.Enabled = False
            End If
            view.DeleteEntityToolStripMenuItem.Text = "Delete " + user_name
            view.pendingsync.Visible = False
            REM FIL JUNE 2013
            view.mpe.Visible = False
            view.mps.Visible = False
            view.mpt.Visible = False
            view.cpendingsync.Visible = False
            If Me.container.oAdmin.sync_types.sync_types.Count = 0 Then
                If Me.current_user.sync_level = 0 And Me.current_user.inactive = False Then
                    view.pendingsync.Visible = True
                    view.pendingsync.Text = "Set " + user_name + " as pending a synchronize"
                End If
                If Me.current_user.sync_level = 1 And Me.current_user.inactive = False Then
                    view.pendingsync.Visible = True
                    view.pendingsync.Text = "Clear " + user_name + " from pending a synchronize"
                End If
            Else
                If Me.current_user.sync_settings.Count = 0 Then
                    view.pendingsync.Visible = True
                    view.pendingsync.Text = "Set " + user_name + " as pending a full synchronize"
                    view.mpe.Text = "Set " + user_name + " as pending an entity synchronize"
                    view.mpe.Visible = True
                    view.mps.Text = "Set " + user_name + " as pending a publication synchronize"
                    view.mps.Visible = True
                    view.mpt.Text = "Set " + user_name + " as pending a template synchronize"
                    view.mpt.Visible = True
                ElseIf Me.current_user.sync_settings.Count = 3 Then
                    view.cpendingsync.Visible = True
                    view.cpendingsync.Text = "Clear " + user_name + " from pending a synchronize"
                Else
                    view.pendingsync.Visible = True
                    view.pendingsync.Text = "Set " + user_name + " as pending a full synchronize"
                    view.cpendingsync.Visible = True
                    view.cpendingsync.Text = "Clear " + user_name + " from pending a synchronize"
                    Dim tfound As Boolean = False
                    Dim pfound As Boolean = False
                    Dim efound As Boolean = False
                    For i = 0 To Me.current_user.sync_settings.Count - 1
                        If Me.current_user.sync_settings(i) = 1 Then
                            tfound = True
                        End If
                        If Me.current_user.sync_settings(i) = 2 Then
                            pfound = True
                        End If
                        If Me.current_user.sync_settings(i) = 3 Then
                            efound = True
                        End If
                    Next
                    If tfound = False Then
                        view.mpt.Text = "Set " + user_name + " as pending a template synchronize"
                        view.mpt.Visible = True
                    End If
                    If pfound = False Then
                        view.mps.Text = "Set " + user_name + " as pending a publication synchronize"
                        view.mps.Visible = True
                    End If
                    If efound = False Then
                        view.mpe.Text = "Set " + user_name + " as pending a entity synchronize"
                        view.mpe.Visible = True
                    End If
                End If


            End If
            REM ------------------------------------
            current_user.attribute_values.Clear()
            For i = 0 To Me.user_attributes.Count - 1
                current_user.attribute_values.Add(Me.user_attributes(i))
            Next


            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                current_user.db_read()
            Else
                current_user.tmode = bc_om_user.tREAD
                If current_user.transmit_to_server_and_receive(current_user, True) = False Then
                    Exit Sub
                End If
            End If
            REM now reload aprefs
            'Dim lprefs As New bc_om_all_prefs
            'If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
            '    lprefs.db_read()
            'Else
            '    lprefs.tmode = bc_om_user.tREAD
            '    If lprefs.transmit_to_server_and_receive(lprefs, True) = False Then
            '        Exit Sub
            '    End If
            'End If

            'container.oAdmin.user_prefs = lprefs.user_prefs
            Me.user_attributes.Clear()

            For i = 0 To current_user.attribute_values.Count - 1
                Me.user_attributes.Add(current_user.attribute_values(i))
            Next



            load_attribute_values()
            load_node_tree()

            view.bentchanges.Enabled = False
            view.bentdiscard.Enabled = False
            REM picture
            view.puserpic.Visible = False
            view.Pnouser.Visible = True
            'Me.DataGridView1.Height = Me.tentities.Height - 50
            Try
                If Not (current_user.picture Is Nothing) Then
                    load_thumbnail()

                Else
                    If container.mode = 0 Or current_user.inactive = True Then
                        view.Pnouser.Visible = False
                    End If
                End If
            Catch

            End Try
            current_user.picture = Nothing

            If view.DataGridView1.RowCount > 1 Then
                view.DataGridView1.Item(1, 1).Selected = True
            End If

            view.Ttitle.TabPages(0).Text = view.Lusers.SelectedItems(0).Text
            If current_user.inactive = True Then
                view.Ttitle.TabPages(0).ImageIndex = deactivate_icon
            Else
                view.Ttitle.TabPages(0).ImageIndex = user_icon
            End If
            load_user_toolbar()
            view.lvwvalidate.Items.Clear()

            If current_user.inactive = True Then
                view.tentities.ContextMenuStrip = Nothing
            End If
            data_grid_size()
            view.puser.Enabled = True
            view.DataGridView1.Enabled = True

            REM FIL JUN 2013
            using_loading = True

            view.dpartialsync.Item(1, 0).Value = "False"
            view.dpartialsync.Item(1, 1).Value = "False"
            view.dpartialsync.Item(1, 2).Value = "False"


            For i = 0 To Me.current_user.sync_settings.Count - 1
                If Me.current_user.sync_settings(i) = 1 Then
                    view.dpartialsync.Item(1, 0).Value = "True"
                ElseIf Me.current_user.sync_settings(i) = 2 Then
                    view.dpartialsync(1, 1).Value = "True"
                ElseIf Me.current_user.sync_settings(i) = 3 Then
                    view.dpartialsync(1, 2).Value = "True"
                End If
            Next
            using_loading = False

            If view.Lusers.SelectedItems.Count = 0 Then
                view.dpartialsync.Item(1, 0).Value = "False"
                view.dpartialsync.Item(1, 1).Value = "False"
                view.dpartialsync.Item(1, 2).Value = "False"

            End If
            'view.psync.Visible = False
            'view.DataGridView1.Visible = True

            'view.tusername.TabPages(0).Select()


        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_cp_users", "load_user", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try
    End Sub
    Private Sub load_thumbnail()

        Dim ms As System.IO.MemoryStream = New System.IO.MemoryStream(current_user.picture)
        view.puserpic.Image = System.Drawing.Image.FromStream(ms)

        view.puserpic.Visible = True
        view.Pnouser.Visible = False
    End Sub
    REM FIL JUly 2012
    Public Sub set_assign_menu()
        Dim ptx, tx As String
        view.IncreaseRatingToolStripMenuItem.Visible = False
        view.DecreaseRatingToolStripMenuItem.Visible = False
        view.massignentity.Visible = False
        view.AddAssociatedClassToolStripMenuItem.Visible = False

        If container.oUsers.pref_types.Count <= 0 Then
            Try
                tx = view.tentities.SelectedNode.Parent.Text
            Catch ex As Exception
                tx = view.tentities.SelectedNode.Text
                view.massignentity.Visible = True
                view.AddAssociatedClassToolStripMenuItem.Visible = True
                view.massignentity.Text = "Assignment of " + tx + " to " + current_user.first_name + " " + current_user.surname
            End Try
            If view.tentities.SelectedNode.ImageIndex = user_icon Then
                If view.tentities.SelectedNode.Index < view.tentities.SelectedNode.Parent.Nodes.Count - 1 Then
                    view.DecreaseRatingToolStripMenuItem.Visible = True
                End If
                If view.tentities.SelectedNode.Index > 0 Then
                    view.IncreaseRatingToolStripMenuItem.Visible = True
                End If
                view.IncreaseRatingToolStripMenuItem.Text = "Increase Rating of " + view.Lusers.SelectedItems(0).Text + " in " + view.tentities.SelectedNode.Parent.Text.Substring(0, view.tentities.SelectedNode.Parent.Text.Length - 3)
                view.DecreaseRatingToolStripMenuItem.Text = "Decrease Rating of " + view.Lusers.SelectedItems(0).Text + " in " + view.tentities.SelectedNode.Parent.Text.Substring(0, view.tentities.SelectedNode.Parent.Text.Length - 3)

            End If
        Else
            view.AddAssociatedClassToolStripMenuItem.Visible = False
            view.massignentity.Visible = False
            If view.tentities.SelectedNode.Text = "Business Areas" Then
                view.massignentity.Visible = True
                view.massignentity.Text = "Assignment of Business Areas to " + current_user.first_name + " " + current_user.surname
            End If
            If view.tentities.SelectedNode.Text = "User Associations" Then
                view.massignentity.Visible = True
                view.massignentity.Text = "Assignment of User Associations to " + current_user.first_name + " " + current_user.surname
            End If

            REM SIm MAY 2013
            If view.tentities.SelectedNode.Text = "Other Roles" Then
                view.massignentity.Visible = True
                view.massignentity.Text = "Assignment of Other Roles to " + current_user.first_name + " " + current_user.surname
            End If
            REM ===

            For m = 0 To container.oUsers.pref_types.Count - 1
                If container.oUsers.pref_types(m).name = view.tentities.SelectedNode.Text Then
                    view.AddAssociatedClassToolStripMenuItem.Visible = True
                    view.AddAssociatedClassToolStripMenuItem.Text = "Add new associated class and entities for " + view.tentities.SelectedNode.Text
                    Exit For
                End If
            Next
            Try
                ptx = view.tentities.SelectedNode.Parent.Text
                tx = view.tentities.SelectedNode.Text
                For m = 0 To container.oUsers.pref_types.Count - 1
                    If container.oUsers.pref_types(m).name = ptx Then
                        view.massignentity.Visible = True
                        view.massignentity.Text = "Assignment of " + tx + " to " + current_user.first_name + " " + current_user.surname + " for " + ptx
                        Exit For
                    End If
                Next

            Catch

            End Try
            If view.tentities.SelectedNode.ImageIndex = user_icon Then
                If view.tentities.SelectedNode.Index < view.tentities.SelectedNode.Parent.Nodes.Count - 1 Then
                    view.DecreaseRatingToolStripMenuItem.Visible = True
                End If
                If view.tentities.SelectedNode.Index > 0 Then
                    view.IncreaseRatingToolStripMenuItem.Visible = True
                End If
                view.IncreaseRatingToolStripMenuItem.Text = "Increase Rating of " + view.Lusers.SelectedItems(0).Text + " in " + view.tentities.SelectedNode.Parent.Text.Substring(0, view.tentities.SelectedNode.Parent.Text.Length - 3)
                view.DecreaseRatingToolStripMenuItem.Text = "Decrease Rating of " + view.Lusers.SelectedItems(0).Text + " in " + view.tentities.SelectedNode.Parent.Text.Substring(0, view.tentities.SelectedNode.Parent.Text.Length - 3)

            End If

        End If
        load_user_toolbar()

    End Sub

    Private Sub highlight_user(ByVal user As String)
        For i = 0 To view.Lusers.Items.Count - 1
            If view.Lusers.Items(i).Text = user Then
                view.Lusers.Items(i).Selected = True
                Exit For
            End If
        Next
    End Sub
    Public Sub discard_change()
        Try
            Dim omsg As bc_cs_message
            If Me.new_user = True Then
                omsg = New bc_cs_message("Blue Curve", "Are you sure you wish to discard " + view.Lusers.SelectedItems(0).Text, bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
            Else
                omsg = New bc_cs_message("Blue Curve", "Are you sure you wish to discard changes made to " + view.Lusers.SelectedItems(0).Text, bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
            End If
            If omsg.cancel_selected = True Then
                Exit Sub
            End If
            REM remove tmp assigned values
            Dim i As Integer
            While i < container.oAdmin.business_areas.Count - 1
                If container.oAdmin.business_areas(i).id < 0 Then
                    container.oAdmin.business_areas.RemoveAt(i)
                Else
                    i = i + 1
                End If
            End While
            If container.oAdmin.languages(container.oAdmin.languages.Count - 1).language_id = -1 Then
                container.oAdmin.languages.RemoveAt(container.oAdmin.languages.Count - 1)
            End If
            If container.oAdmin.roles(container.oAdmin.roles.Count - 1).id = -1 Then
                container.oAdmin.roles.RemoveAt(container.oAdmin.roles.Count - 1)
            End If
            If container.oAdmin.offices(container.oAdmin.offices.Count - 1).id = -1 Then
                container.oAdmin.offices.RemoveAt(container.oAdmin.offices.Count - 1)
            End If
            Dim luser As String
            view.bentchanges.Enabled = False
            view.bentdiscard.Enabled = False
            view.pusers.Enabled = True
            view.tusers.Enabled = True
            luser = view.Lusers.SelectedItems(0).Text
            If Me.new_user = False Then
                view.Lusers.SelectedItems.Clear()
                If container.oPubtypes.process_switches.surname_first = True Then
                    highlight_user(current_user.surname + ", " + current_user.first_name)
                Else
                    highlight_user(current_user.first_name + " " + current_user.surname)
                End If
            Else
                view.Lusers.SelectedItems(0).Remove()
                Me.clear_user_details()
            End If
            container.uxNavBar.Enabled = True
            Me.new_user = False
            Me.load_user_toolbar()
            container.uncomitted_data = False
            reset_warnings()
            view.lvwvalidate.Items.Clear()

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_users", "discard_changes", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Public Sub reset_warnings()

        view.tusername.TabPages(0).ImageIndex = attributes_icon
        view.Tuserass.TabPages(0).ImageIndex = links_icon
        view.Ttitle.TabPages(0).ImageIndex = user_icon
        For i = 0 To Me.user_attributes.Count - 1
            If Me.user_attributes(i).nullable = False Then
                view.DataGridView1.Item(0, i).Value = view.tabimages.Images(mandatory_icon)
            End If
            If Me.user_attributes.Count > 0 Then
                If Me.user_attributes(i).value_changed = True Then
                    view.DataGridView1.Item(0, i).Value = view.tabimages.Images(att_edited_icon)
                End If
            End If
        Next

    End Sub

    Public Sub load_attribute_values()




        REM set up first name and surname for new user
        For j = 0 To user_attributes.Count - 1
            If user_attributes(j).attribute_id = 0 Then
                If user_attributes(j).field_name = "first_name" Then
                    user_attributes(j).value = current_user.first_name
                End If
                If user_attributes(j).field_name = "surname" Then
                    user_attributes(j).value = current_user.surname
                End If
            End If
        Next


        REM attribute values
        For j = 0 To user_attributes.Count - 1
            If user_attributes(j).attribute_id = 0 Then
                For i = 0 To container.oUsers.user_system_attributes.Count - 1
                    If container.oUsers.user_system_attributes(i).display_name = user_attributes(j).name Then
                        For l = 0 To container.oUsers.user_system_attributes(i).lookup_vals_key.Count - 1
                            If container.oUsers.user_system_attributes(i).lookup_vals_key(l) = user_attributes(j).value Then
                                If container.oUsers.user_system_attributes(i).lookup_vals_inactive(l) = True Then
                                    user_attributes(j).value = container.oUsers.user_system_attributes(i).lookup_vals(l) + " (inactive)"
                                Else
                                    user_attributes(j).value = container.oUsers.user_system_attributes(i).lookup_vals(l)
                                End If
                                Exit For
                            End If
                        Next
                        If user_attributes(j).field_name = "password" Then
                            user_attributes(j).value = "**********"
                        End If
                        Exit For
                    End If
                Next
            Else
                For i = 0 To container.oClasses.attribute_pool.Count - 1
                    If container.oClasses.attribute_pool(i).name = view.DataGridView1.Item(1, j).Value Then
                        If container.oClasses.attribute_pool(i).lookup_keys.count > 0 Then
                            REM find key value for value
                            For l = 0 To container.oClasses.attribute_pool(i).lookup_values.count - 1
                                If RTrim(LTrim(container.oClasses.attribute_pool(i).lookup_keys(l))) = user_attributes(j).value Then
                                    user_attributes(j).value = container.oClasses.attribute_pool(i).lookup_values(l)
                                    Exit For
                                End If
                            Next
                            Exit For
                        End If
                    End If
                Next


            End If
            view.DataGridView1.Item(3, j).Value = user_attributes(j).value


            If user_attributes(j).value_changed = True Then
                view.DataGridView1.Item(0, j).Value = view.tabimages.Images(att_edited_icon)
            ElseIf user_attributes(j).nullable = False Then
                view.DataGridView1.Item(0, j).Value = view.tabimages.Images(mandatory_icon)
            Else
                view.DataGridView1.Item(0, j).Value = New Drawing.Bitmap(1, 1)
            End If
        Next


    End Sub
    Public Sub load_user_toolbar()

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
                newitem = .Add("Add User")
                newitem.ToolTipText = "Add new User"
                newitem.TextImageRelation = TextImageRelation.ImageAboveText
                newitem.ImageIndex = add_icon
                If view.Lusers.SelectedItems.Count = 0 Then
                    newitem = .Add("Deactivate")
                    newitem.Enabled = False
                    newitem.ImageIndex = deactivate_icon
                    newitem.ToolTipText = "Deactive user"
                    newitem.TextImageRelation = TextImageRelation.ImageAboveText
                    If Me.container.mode = bc_am_cp_container.EDIT_FULL Then
                        newitem = .Add("Delete User")
                        newitem.Enabled = False
                        newitem.ImageIndex = delete_icon
                        newitem.ToolTipText = "Delete user"
                        newitem.TextImageRelation = TextImageRelation.ImageAboveText
                    End If
                Else
                    If InStr(view.Lusers.SelectedItems(0).Text, "(inactive)") = 0 Then
                        newitem = .Add("Deactivate")
                        newitem.ImageIndex = deactivate_icon
                        newitem.ToolTipText = "Deactivate user " + view.Lusers.SelectedItems(0).Text
                        newitem.TextImageRelation = TextImageRelation.ImageAboveText

                        If Me.container.mode = bc_am_cp_container.EDIT_FULL Then
                            newitem = .Add("Delete User")
                            newitem.Enabled = False
                            newitem.ImageIndex = delete_icon
                            newitem.ToolTipText = "Delete User"
                            newitem.TextImageRelation = TextImageRelation.ImageAboveText
                        End If
                    Else
                        newitem = .Add("Change User Name")
                        newitem.ImageIndex = edit_icon
                        newitem.ToolTipText = "Change name of User"
                        newitem.TextImageRelation = TextImageRelation.ImageAboveText
                        newitem.Enabled = False
                        newitem = .Add("Activate")
                        newitem.ImageIndex = selected_icon
                        newitem.ToolTipText = "Activate User " + Left(view.Lusers.SelectedItems(0).Text, Me.view.Lusers.SelectedItems(0).Text.Length - 10)
                        newitem.TextImageRelation = TextImageRelation.ImageAboveText
                        If Me.container.mode = bc_am_cp_container.EDIT_FULL Then

                            newitem = .Add("Delete User")
                            newitem.ImageIndex = delete_icon
                            newitem.ToolTipText = "Delete User " + view.Lusers.SelectedItems(0).Text
                            newitem.TextImageRelation = TextImageRelation.ImageAboveText
                        End If
                    End If
                End If

                Dim separator As New ToolStripSeparator
                .Add(separator)

                newitem = .Add("Association Class")
                newitem.ImageIndex = 7
                newitem.Enabled = False
                newitem.ToolTipText = "Add new associated class and entities"
                newitem.TextImageRelation = TextImageRelation.ImageAboveText


                Dim i As Integer
                Dim stx As String
                If container.oUsers.pref_types.Count > 1 Then
                    Try
                        stx = view.tentities.SelectedNode.Text

                        For i = 0 To container.oUsers.pref_types.Count - 1
                            If container.oUsers.pref_types(i).name = stx Then
                                If view.Lusers.SelectedItems.Count > 0 Then
                                    If InStr(view.Lusers.SelectedItems(0).Text, "(inactive)") = 0 Then
                                        newitem.Enabled = True
                                        newitem.ToolTipText = "Add new associated class and entities to " + view.Lusers.SelectedItems(0).Text + " for " + stx
                                        newitem.TextImageRelation = TextImageRelation.ImageAboveText
                                    End If
                                End If
                            End If
                        Next
                    Catch
                    End Try
                Else
                    If view.Lusers.SelectedItems.Count > 0 Then
                        If InStr(view.Lusers.SelectedItems(0).Text, "(inactive)") = 0 Then
                            newitem.Enabled = True
                            newitem.ToolTipText = "Add new associated class and entities to " + view.Lusers.SelectedItems(0).Text
                            newitem.TextImageRelation = TextImageRelation.ImageAboveText
                        End If
                    End If
                End If

                newitem = .Add("Association")
                newitem.ImageIndex = 7
                newitem.Enabled = False
                newitem.TextImageRelation = TextImageRelation.ImageAboveText
                Try
                    If container.oUsers.pref_types.Count > 1 And view.tentities.SelectedNode.Text <> "Business Areas" Then
                        Try
                            stx = view.tentities.SelectedNode.Parent.Text
                            For i = 0 To container.oUsers.pref_types.Count - 1
                                If container.oUsers.pref_types(i).name = stx Then
                                    If view.Lusers.SelectedItems.Count > 0 Then
                                        If InStr(view.Lusers.SelectedItems(0).Text, "(inactive)") = 0 Then
                                            newitem.Enabled = True
                                            newitem.ToolTipText = "Assign " + view.tentities.SelectedNode.Text + " to " + view.Lusers.SelectedItems(0).Text + " for " + stx
                                            newitem.TextImageRelation = TextImageRelation.ImageAboveText
                                        End If
                                    End If
                                End If
                            Next

                        Catch ex As Exception

                        End Try

                    Else
                        Dim tx As String
                        Try
                            tx = view.tentities.SelectedNode.Parent.Text
                        Catch ex As Exception
                            Try
                                tx = view.tentities.SelectedNode.Text
                                newitem.Enabled = True
                                newitem.ToolTipText = "Assign " + tx + " to " + view.Lusers.SelectedItems(0).Text
                            Catch


                            End Try
                        End Try
                    End If
                Catch

                End Try

                separator = New ToolStripSeparator
                .Add(separator)

                newitem = .Add("Add Language")
                newitem.ImageIndex = add_icon
                newitem.TextImageRelation = TextImageRelation.ImageAboveText
                newitem.Enabled = False
                If view.Lusers.SelectedItems.Count = 1 Then
                    If view.DataGridView1.SelectedCells.Count = 1 Then
                        If view.DataGridView1.Rows(view.DataGridView1.SelectedCells(0).RowIndex).Cells(1).Value = "Language" Then
                            newitem.Enabled = True
                            If view.Lusers.SelectedItems.Count > 0 Then
                                newitem.ToolTipText = "Add new language and assign to " + view.Lusers.SelectedItems(0).Text
                                newitem.TextImageRelation = TextImageRelation.ImageAboveText
                            End If
                        End If
                    End If
                End If
                newitem = .Add("Add Role")
                newitem.ImageIndex = add_icon
                newitem.Enabled = False
                newitem.TextImageRelation = TextImageRelation.ImageAboveText
                If view.Lusers.SelectedItems.Count = 1 Then

                    If view.DataGridView1.SelectedCells.Count = 1 Then
                        If view.DataGridView1.Rows(view.DataGridView1.SelectedCells(0).RowIndex).Cells(1).Value = "Role" Then
                            newitem.Enabled = True
                            If view.Lusers.SelectedItems.Count > 0 Then
                                newitem.ToolTipText = "Add new role and assign to " + view.Lusers.SelectedItems(0).Text
                                newitem.TextImageRelation = TextImageRelation.ImageAboveText
                            End If
                        End If
                    End If
                End If
                newitem = .Add("Add Office")
                newitem.ImageIndex = add_icon
                newitem.Enabled = False
                newitem.TextImageRelation = TextImageRelation.ImageAboveText
                If view.Lusers.SelectedItems.Count = 1 Then
                    If view.DataGridView1.SelectedCells.Count = 1 Then
                        If view.DataGridView1.Rows(view.DataGridView1.SelectedCells(0).RowIndex).Cells(1).Value = "Office" Then
                            newitem.Enabled = True
                            If view.Lusers.SelectedItems.Count > 0 Then
                                newitem.ToolTipText = "Add new office and assign to " + view.Lusers.SelectedItems(0).Text
                                newitem.TextImageRelation = TextImageRelation.ImageAboveText
                            End If
                        End If
                    End If
                End If
                separator = New ToolStripSeparator
                .Add(separator)
                newitem = .Add("Display Attributes")
                newitem.Enabled = True
                newitem.ImageIndex = add_icon
                newitem.TextImageRelation = TextImageRelation.ImageAboveText
            Else
                newitem = .Add("Add User")
                newitem.Enabled = False
                newitem.ToolTipText = "Add new Add User"
                newitem.TextImageRelation = TextImageRelation.ImageAboveText
                newitem.ImageIndex = add_icon
                newitem = .Add("Deactivate")
                newitem.Enabled = False
                newitem.ImageIndex = deactivate_icon
                newitem.ToolTipText = "Deactive Add User"
                newitem.TextImageRelation = TextImageRelation.ImageAboveText
                newitem = .Add("Delete Add User")
                newitem.Enabled = False
                newitem.ImageIndex = delete_icon
                newitem.ToolTipText = "Delete Add User"
                newitem.TextImageRelation = TextImageRelation.ImageAboveText

                Dim separator As New ToolStripSeparator
                .Add(separator)

                newitem = .Add("Association Class")
                newitem.ImageIndex = 7
                newitem.Enabled = False
                newitem.ToolTipText = "Add new associated class and entities"
                newitem.TextImageRelation = TextImageRelation.ImageAboveText


                Dim i As Integer
                Dim stx As String
                If container.oUsers.pref_types.Count > 1 Then
                    Try
                        stx = view.tentities.SelectedNode.Text
                        For i = 0 To container.oUsers.pref_types.Count - 1
                            If container.oUsers.pref_types(i).name = stx Then
                                If view.Lusers.SelectedItems.Count > 0 Then
                                    If InStr(view.Lusers.SelectedItems(0).Text, "(inactive)") = 0 Then
                                        newitem.Enabled = True
                                        newitem.ToolTipText = "Add new associated class and entities to " + view.Lusers.SelectedItems(0).Text + " for " + stx
                                        newitem.TextImageRelation = TextImageRelation.ImageAboveText
                                    End If
                                End If
                            End If
                        Next
                    Catch

                    End Try
                Else
                    If view.Lusers.SelectedItems.Count > 0 Then
                        If InStr(view.Lusers.SelectedItems(0).Text, "(inactive)") = 0 Then
                            newitem.Enabled = True
                            newitem.ToolTipText = "Add new associated class and entities to " + view.Lusers.SelectedItems(0).Text
                            newitem.TextImageRelation = TextImageRelation.ImageAboveText
                        End If
                    End If
                End If

                newitem = .Add("Association")
                newitem.ImageIndex = 7
                newitem.Enabled = False
                newitem.TextImageRelation = TextImageRelation.ImageAboveText
                Try
                    If container.oUsers.pref_types.Count > 1 And view.tentities.SelectedNode.Text <> "Business Areas" Then
                        Try
                            stx = view.tentities.SelectedNode.Parent.Text
                            For i = 0 To container.oUsers.pref_types.Count - 1
                                If container.oUsers.pref_types(i).name = stx Then
                                    If view.Lusers.SelectedItems.Count > 0 Then
                                        If InStr(view.Lusers.SelectedItems(0).Text, "(inactive)") = 0 Then
                                            newitem.Enabled = True
                                            newitem.ToolTipText = "Assign " + view.tentities.SelectedNode.Text + " to " + view.Lusers.SelectedItems(0).Text + " for " + stx
                                            newitem.TextImageRelation = TextImageRelation.ImageAboveText
                                        End If
                                    End If
                                End If
                            Next

                        Catch ex As Exception

                        End Try

                    Else
                        Dim tx As String
                        Try
                            tx = view.tentities.SelectedNode.Parent.Text
                        Catch ex As Exception
                            Try
                                tx = view.tentities.SelectedNode.Text
                                newitem.Enabled = True
                                newitem.ToolTipText = "Assign " + tx + " to " + view.Lusers.SelectedItems(0).Text
                            Catch


                            End Try
                        End Try
                    End If
                Catch

                End Try
                newitem = .Add("")
                newitem.Enabled = False
                newitem = .Add("Add Language")
                newitem.ImageIndex = add_icon
                newitem.TextImageRelation = TextImageRelation.ImageAboveText
                newitem.Enabled = False
                If view.DataGridView1.SelectedCells.Count = 1 Then
                    If view.DataGridView1.SelectedCells(0).RowIndex = 5 Then
                        newitem.Enabled = True
                    End If
                End If
                newitem = .Add("Add Role")
                newitem.Enabled = False
                newitem.ImageIndex = add_icon
                newitem.TextImageRelation = TextImageRelation.ImageAboveText
                If view.DataGridView1.SelectedCells.Count = 1 Then
                    If view.DataGridView1.SelectedCells(0).RowIndex = 3 Then
                        newitem.Enabled = True
                    End If
                End If
                newitem = .Add("Add Office")
                newitem.Enabled = False
                newitem.ImageIndex = add_icon
                newitem.TextImageRelation = TextImageRelation.ImageAboveText
                If view.DataGridView1.SelectedCells.Count = 1 Then
                    If view.DataGridView1.SelectedCells(0).RowIndex = 9 Then
                        newitem.Enabled = True
                    End If
                End If
                If Me.container.mode = bc_am_cp_container.EDIT_FULL Then
                    separator = New ToolStripSeparator
                    .Add(separator)
                    newitem = .Add("Display Attributes")
                    newitem.Enabled = False
                    newitem.ImageIndex = add_icon
                    newitem.TextImageRelation = TextImageRelation.ImageAboveText

                End If


            End If
        End With
    End Sub
    Public Sub add_user_menu()
        Dim ofrm As New bc_am_cp_edit
        ofrm.ltitle.Text = "Enter user name"
        ofrm.ShowDialog()
        If ofrm.cancel_selected = True Then
            Exit Sub
        End If
        add_user(ofrm.Tentry.Text)
    End Sub
    Public Sub change_user_menu()
        Dim ofrm As New bc_am_cp_edit
        ofrm.ltitle.Text = "Change name of " + view.Lusers.SelectedItems(0).Text
        ofrm.ShowDialog()
        If ofrm.cancel_selected = True Then
            Exit Sub
        End If
        'change_user(ofrm.Tentry.Text)
    End Sub
    Private Sub add_user(ByVal tx As String)
        Try
            REM first check user name is entered and unique
            Dim uname As String
            For i = 0 To container.oUsers.user.Count - 1
                uname = container.oUsers.user(i).first_name + " " + container.oUsers.user(i).surname
                If UCase(Trim(uname)) = UCase(Trim(tx)) Then
                    Dim omsg As New bc_cs_message("Blue Curve", "user name " + uname + " already exists", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub
                End If
            Next
            REM now create new user
            view.Lusers.SelectedItems.Clear()
            view.minactive.Enabled = False
            view.DeleteEntityToolStripMenuItem.Enabled = False

            If Not IsNothing(current_user.first_name) Then
                load_user_attributes()
            End If

            current_user = New bc_om_user
            current_user.prefs.Clear()
            current_user.bus_areas.Clear()
            current_user.sync_level = 1
            uname = tx
            If InStr(uname, " ") > 0 Then
                current_user.first_name = uname.Substring(0, InStr(uname, " ") - 1)
                current_user.surname = uname.Substring(Len(current_user.first_name) + 1, Len(uname) - Len(current_user.first_name) - 1)
            Else
                current_user.first_name = uname
            End If
            view.puserpic.Visible = False
            view.Pnouser.Visible = True
            view.tentities.Nodes.Clear()

            load_attribute_values()
            load_node_tree()
            new_user = True

            view.Ttitle.TabPages(0).ImageIndex = user_icon
            view.Ttitle.TabPages(0).Text = current_user.first_name + " " + current_user.surname
            If container.oPubtypes.process_switches.surname_first = True Then
                view.Lusers.Items.Add(current_user.surname + ", " + current_user.first_name, 0)
                highlight_user(current_user.surname + ", " + current_user.first_name)
            Else
                view.Lusers.Items.Add(current_user.first_name + " " + current_user.surname, 0)
                highlight_user(current_user.first_name + " " + current_user.surname)
            End If

            'view.Lusers.Items.Add(current_user.first_name + " " + current_user.surname, 0)
            'Me.highlight_user(current_user.first_name + " " + current_user.surname)
            view.tentities.ContextMenuStrip = view.assignentity
            set_default_attribute_values()
            set_change()
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_users", "add_user(k", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
        End Try

    End Sub

    Public Sub set_change()
        If view.Lusers.SelectedItems.Count = 0 Then
            Exit Sub
        End If
        If Me.container.mode = bc_am_cp_container.VIEW Then
            Exit Sub
        End If
        If using_loading = True Then
            Exit Sub
        End If
        view.puser.Enabled = True
        view.bentchanges.Enabled = True
        view.bentdiscard.Enabled = True
        view.pusers.Enabled = False
        view.tusers.Enabled = False
        container.uxNavBar.Enabled = False
        Me.load_user_toolbar()
        container.uncomitted_data = True
        view.DataGridView1.Enabled = True
        view.baudit.Enabled = False
        view.buseraudit.Enabled = False
    End Sub
    Public Sub active()
        Try
            REM locate selected user
            If InStr(view.Lusers.SelectedItems(0).Text, "inactive") > 0 Then
                current_user.inactive = 0
                current_user.write_mode = bc_om_user.SET_ACTIVE
            Else
                REM cant make current logged on user inactivbe
                If current_user.id = bc_cs_central_settings.logged_on_user_id Then
                    Dim omsg As New bc_cs_message("Blue Curve", "You cannot make yourself inactive!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub
                End If
                current_user.inactive = 1
                current_user.write_mode = bc_om_user.SET_INACTIVE
            End If

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                current_user.db_write()
            Else
                current_user.tmode = bc_om_user.tWRITE
                If current_user.transmit_to_server_and_receive(current_user, True) = False Then
                    Exit Sub
                End If
            End If
            For i = 0 To container.oUsers.user.Count - 1
                If container.oUsers.user(i).id = current_user.id Then
                    container.oUsers.user(i) = current_user
                    Exit For
                End If
            Next
            load_users_tree()
            load_users(0, False, "")
            If container.oPubtypes.process_switches.surname_first = True Then
                If current_user.inactive = False Then
                    highlight_user(current_user.surname + ", " + current_user.first_name)
                    Dim omsg As New bc_cs_message("Blue Curve", view.Lusers.SelectedItems(0).Text + " has been reactivated please check all attributes to make sure they are active and valid otherwise user may not perform as expected.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Else
                    highlight_user(current_user.surname + ", " + current_user.first_name + " (inactive)")
                End If
            Else
                If current_user.inactive = False Then

                    If container.oPubtypes.process_switches.surname_first = True Then
                        highlight_user(current_user.surname + ", " + current_user.first_name)
                    Else
                        highlight_user(current_user.first_name + " " + current_user.surname)
                    End If

                    Dim omsg As New bc_cs_message("Blue Curve", view.Lusers.SelectedItems(0).Text + " has been reactivated please check all attributes to make sure they are active and valid otherwise user may not perform as expected.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Else

                    If container.oPubtypes.process_switches.surname_first = True Then
                        highlight_user(current_user.surname + ", " + current_user.first_name + " (inactive)")
                    Else
                        highlight_user(current_user.first_name + " " + current_user.surname + " (inactive)")
                    End If
                End If
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_users", "active", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Sub
    REM FIL JUN 2013
    Public Sub set_sync(ByVal mode As Integer)
        Try
            If container.oAdmin.sync_types.sync_types.Count = 0 Then
                If current_user.sync_level = 0 Then
                    current_user.write_mode = bc_om_user.SET_SYNC
                    current_user.sync_level = 1

                Else
                    current_user.write_mode = bc_om_user.CLEAR_SYNC
                    current_user.sync_level = 0
                End If

            Else
                current_user.write_mode = bc_om_user.WRITE_PARTIAL_SYNC
                Select Case mode
                    Case 1
                        current_user.sync_settings.Clear()
                        current_user.sync_settings.Add(1)
                        current_user.sync_settings.Add(2)
                        current_user.sync_settings.Add(3)

                    Case 2
                        current_user.sync_settings.Clear()
                    Case 3
                        current_user.sync_settings.Add(1)
                    Case 4
                        current_user.sync_settings.Add(2)
                    Case 5
                        current_user.sync_settings.Add(3)
                End Select
            End If
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                current_user.db_write()
            Else
                current_user.tmode = bc_om_user.tWRITE
                If current_user.transmit_to_server_and_receive(current_user, True) = False Then
                    Exit Sub
                End If
            End If
            For i = 0 To container.oUsers.user.Count - 1
                If container.oUsers.user(i).id = current_user.id Then
                    container.oUsers.user(i) = current_user
                    Exit For
                End If
            Next
            Dim tx As String
            Dim user As String
            user = view.Lusers.SelectedItems(0).Text


            tx = view.tusers.SelectedNode.Text
            load_users_tree()
            load_users(0, False, "")

            If tx = "Synced Users" Then
                view.tusers.SelectedNode = view.tusers.Nodes(0).Nodes(2)
            ElseIf tx = "Unsynced Users" Then
                view.tusers.SelectedNode = view.tusers.Nodes(0).Nodes(3)
            ElseIf tx = "Partially Synced Users" Then
                view.tusers.SelectedNode = view.tusers.Nodes(0).Nodes(4)
            Else
                highlight_user(user)
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_users", "set_sync", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Sub
    REM -------------------------------------------

    Public Sub delete_user()
        Try
            Dim ouser As String

            ouser = view.controller.current_user.first_name + " " + view.controller.current_user.surname
            Dim omsg As New bc_cs_message("Blue Curve", "Are you sure you wish to delete " + ouser, bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
            If omsg.cancel_selected Then
                Exit Sub
            End If
            view.Cursor = Cursors.WaitCursor
            view.controller.current_user.write_mode = bc_om_user.DELETE
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                view.controller.current_user.db_write()
            Else
                view.controller.current_user.tmode = bc_om_user.tWRITE
                If view.controller.current_user.transmit_to_server_and_receive(view.controller.current_user, True) = False Then
                    omsg = New bc_cs_message("Blue Curve", "Failed to remove " + ouser, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub
                End If
            End If
            If view.controller.current_user.delete_error <> "" Then
                omsg = New bc_cs_message("Blue Curve", "Failed to remove " + ouser + " database error: " + view.controller.current_user.delete_error, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            Else
                For i = 0 To container.oUsers.user.Count - 1
                    If container.oUsers.user(i).id = current_user.id Then
                        container.oUsers.user.RemoveAt(i)
                        Exit For
                    End If
                Next
                load_users(0, False, "")
                view.Cursor = Cursors.Default
                omsg = New bc_cs_message("Blue Curve", ouser + " suceessfully deleted", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_users", "delete_user", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            view.Cursor = Cursors.Default
        End Try
    End Sub
    Public Function check_warnings() As Boolean
        Try
            REM check for mandatory fields
            reset_warnings()
            check_warnings = True
            view.warnings.Clear()
            For j = 0 To Me.user_attributes.Count - 1
                If Me.user_attributes(j).nullable = False Then
                    If Me.user_attributes(j).value = "" Then
                        Dim owarning As New warning
                        owarning.msg = view.Ttitle.TabPages(0).Text + " requires : " + Me.user_attributes(j).name + " to be entered"
                        owarning.attribute = j
                        owarning.attribute_tab = 0
                        owarning.entity_tab = -1
                        view.warnings.Add(owarning)
                        check_warnings = False
                    End If
                End If

                If Me.user_attributes(j).value <> "" Then
                    For k = 0 To container.oClasses.attribute_pool.Count - 1
                        If container.oClasses.attribute_pool(k).attribute_id = Me.user_attributes(j).attribute_id Then
                            If container.oClasses.attribute_pool(k).is_lookup = False Then
                                Select Case container.oClasses.attribute_pool(k).type_id

                                    Case 2
                                        If IsNumeric(Me.user_attributes(j).value) = False Then
                                            Dim owarning As New warning
                                            owarning.msg = Me.user_attributes(j).name + " requires  " + container.oClasses.attribute_pool(k).name + " to be numeric"
                                            owarning.attribute = j
                                            owarning.attribute_tab = 0
                                            owarning.entity_tab = -1
                                            check_warnings = False

                                            view.warnings.Add(owarning)
                                        End If

                                    Case 5
                                        If IsDate(Me.user_attributes(j).value) = False Then
                                            Dim owarning As New warning
                                            owarning.msg = Me.user_attributes(j).name + " requires  " + container.oClasses.attribute_pool(k).name + " to be date"
                                            owarning.attribute = j
                                            owarning.attribute_tab = 0
                                            owarning.entity_tab = -1
                                            check_warnings = False
                                            view.warnings.Add(owarning)
                                        End If
                                End Select
                            End If
                            Exit For
                        End If
                    Next
                End If




                REM check if anything is inactive
                If InStr(Me.user_attributes(j).value, "(inactive)") > 0 Then
                    Dim owarning As New warning
                    owarning.msg = view.Ttitle.TabPages(0).Text + " has inactive  " + Me.user_attributes(j).name + " change to active one"
                    owarning.attribute = j
                    owarning.attribute_tab = 0
                    owarning.entity_tab = -1
                    view.warnings.Add(owarning)
                    check_warnings = False
                End If
            Next
            Dim na As String = ""
            Dim fnatt As Integer = -1
            Dim snatt As Integer = -1
            For i = 0 To Me.user_attributes.Count - 1
                If Me.user_attributes(i).field_name = "first_name" Then
                    na = Me.user_attributes(i).value + " "
                    fnatt = i
                    Exit For
                End If
            Next
            For i = 0 To Me.user_attributes.Count - 1
                If Me.user_attributes(i).field_name = "surname" Then
                    na = na + Me.user_attributes(i).value
                    snatt = i
                    Exit For
                End If
            Next
            Dim osname As String = ""
            Dim oschanged As Boolean = False
            Dim osatt As Integer = -1
            For i = 0 To Me.user_attributes.Count - 1
                If Me.user_attributes(i).field_name = "os_user_name" Then
                    osname = Me.user_attributes(i).value
                    oschanged = Me.user_attributes(i).value_changed
                    osatt = i
                    Exit For
                End If
            Next
            Dim wsname As String = ""
            Dim wsatt As Integer = -1
            For i = 0 To Me.user_attributes.Count - 1
                If Me.user_attributes(i).field_name = "web_user_name" Then
                    wsname = Me.user_attributes(i).value
                    wsatt = i
                    Exit For
                End If
            Next

            For i = 0 To container.oUsers.user.Count - 1
                If container.oUsers.user(i).id <> current_user.id And fnatt > -1 And snatt > -1 Then
                    If na = container.oUsers.user(i).first_name + " " + container.oUsers.user(i).surname Then
                        Dim owarning As New warning
                        owarning.msg = "user first name combination already exists for another user"
                        owarning.attribute = fnatt
                        owarning.attribute_tab = 0
                        owarning.entity_tab = -1
                        view.warnings.Add(owarning)
                        owarning = New warning
                        owarning.msg = "user surname combination already exists for another user"
                        owarning.attribute = snatt
                        owarning.attribute_tab = 0
                        owarning.entity_tab = -1
                        view.warnings.Add(owarning)
                        check_warnings = False
                    End If
                    REM ING August 2012
                    If osatt > -1 And UCase(container.oUsers.user(i).os_user_name) = UCase(osname) And container.oUsers.user(i).os_user_name <> "" Then
                        Dim owarning As New warning
                        owarning.msg = "Logon name is already is used by " + container.oUsers.user(i).first_name + " " + container.oUsers.user(i).surname
                        owarning.attribute = osatt
                        owarning.attribute_tab = 0
                        owarning.entity_tab = -1
                        view.warnings.Add(owarning)
                        check_warnings = False
                    End If
                    REM user name is unqiue
                    If wsatt > -1 And UCase(container.oUsers.user(i).user_name) = UCase(wsname) And container.oUsers.user(i).user_name <> "" Then
                        Dim owarning As New warning
                        owarning.msg = "web user name is already is used by " + container.oUsers.user(i).first_name + " " + container.oUsers.user(i).surname
                        owarning.attribute = wsatt
                        owarning.attribute_tab = 0
                        owarning.entity_tab = -1
                        view.warnings.Add(owarning)
                        check_warnings = False
                    End If
                End If
            Next

            If current_user.id = bc_cs_central_settings.logged_on_user_id And osatt > -1 Then
                If oschanged = True Then
                    Me.user_attributes(osatt).value = bc_cs_central_settings.logged_on_user_name
                    view.DataGridView1.Item(3, osatt).Value = bc_cs_central_settings.logged_on_user_name
                    Me.user_attributes(osatt).value_changed = False
                    Dim owarning As New warning
                    owarning.msg = "logon name cant be changed for logged on user so reset."
                    owarning.attribute = osatt
                    owarning.attribute_tab = 0
                    owarning.entity_tab = -1
                    view.warnings.Add(owarning)
                    check_warnings = False
                End If
            End If

            REM check at least one business area has been set and it is active
            If current_user.bus_areas.Count = 0 Then
                Dim owarning As New warning
                owarning.msg = view.Ttitle.TabPages(0).Text + " requires assignment to at least 1 business area"
                owarning.attribute = -1
                owarning.attribute_tab = 0
                owarning.entity_tab = 0
                view.warnings.Add(owarning)
                check_warnings = False
            Else
                REM make sure at least oe business area is active
                Dim active As Boolean = False
                For i = 0 To current_user.bus_areas.Count - 1
                    For k = 0 To container.oAdmin.business_areas.Count - 1
                        If current_user.bus_areas(i) = container.oAdmin.business_areas(k).id Then
                            If container.oAdmin.business_areas(k).inactive = False Then
                                active = True
                            End If
                            Exit For
                        End If
                    Next
                Next
                If active = False Then
                    Dim owarning As New warning
                    owarning.msg = view.Ttitle.TabPages(0).Text + " requires assugnment to at least 1 active business area"
                    owarning.attribute = -1
                    owarning.attribute_tab = 0
                    owarning.entity_tab = 0
                    view.warnings.Add(owarning)
                    check_warnings = False
                End If
            End If
            view.lvwvalidate.Items.Clear()
            If view.warnings.Count > 0 Then


                view.lvwvalidate.Columns(0).Text = CStr(view.warnings.Count) + " Warnings"

                REM --------------
                Dim lwarn As ListViewItem
                For i = 0 To view.warnings.Count - 1
                    lwarn = New ListViewItem(CStr(view.warnings(i).msg))
                    view.Ttitle.TabPages(0).ImageIndex = warning_icon
                    If view.warnings(i).entity_tab > -1 Then
                        lwarn.ImageIndex = warning_icon
                        view.Tuserass.TabPages(0).ImageIndex = warning_icon
                        view.tentities.Nodes(0).ImageIndex = warning_icon
                    Else
                        lwarn.ImageIndex = warning_icon
                        view.tusername.TabPages(0).ImageIndex = warning_icon
                        view.DataGridView1.Item(0, view.warnings(i).attribute).Value = view.tabimages.Images(warning_icon)
                    End If
                    view.lvwvalidate.Items.Add(lwarn)
                Next
            Else
                REM ING JUNE 2012
                view.lvwvalidate.Columns(0).Text = "Warnings"
                REM -------------
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_users", "check_warnings", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try
    End Function
    Public Function validate_attribute_value(ByVal val As Object) As Boolean

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
            Dim oerr As New bc_cs_error_log("bc_am_users", "validate_attribute_value", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            no_action = False
        End Try
    End Function
    'Public Sub set_val(ByVal val As String, Optional ByVal ext As Boolean = False)
    '    If val <> view.user_attributes_values(view.DataGridView1.SelectedCells(0).RowIndex).value Then
    '        view.user_attributes_values(view.DataGridView1.SelectedCells(0).RowIndex).value = val
    '        view.user_attributes_values(view.DataGridView1.SelectedCells(0).RowIndex).value_changed = True
    '        view.DataGridView1.Item(0, view.DataGridView1.SelectedCells(0).RowIndex).Value = view.tabimages.Images(att_edited_icon)
    '        If ext = True Then
    '            view.DataGridView1.Item(3, view.DataGridView1.SelectedCells(0).RowIndex).Value = val
    '        End If
    '        set_change()
    '    End If
    'End Sub
    'Public Function set_data_type_Control() As Boolean
    '    Try
    '        Dim lus As New ArrayList
    '        Dim i As Integer
    '        set_data_type_Control = False
    '        If no_action = True Then
    '            Exit Function
    '        End If
    '        If view.DataGridView1.SelectedCells(0).ColumnIndex <> 2 Then
    '            Exit Function
    '        End If

    '        no_action = True

    '        REM temporary hold current values
    '        hold_vals.Clear()
    '        For i = 0 To view.DataGridView1.Rows.Count - 1
    '            hold_vals.Add(view.DataGridView1.Item(2, i).Value)
    '            If IsNothing(hold_vals(i)) Or InStr(hold_vals(i), "(inactive)") > 0 Then
    '                hold_vals(i) = ""
    '            End If
    '        Next
    '        Dim lu As DataGridViewComboBoxCell
    '        Dim use_combo As Boolean = False
    '        Dim dtextbox As New DataGridViewTextBoxColumn
    '        Dim Dcombobox As New DataGridViewComboBoxColumn
    '        Dim row_id As Integer
    '        row_id = view.DataGridView1.SelectedCells(0).RowIndex

    '        Dim j As Integer
    '        REM 1 string 2 number 3 boolean 5 date
    '        'view.DataGridView1.Item(1, row_id).Selected = True
    '        REM build up array of all lookup values
    '        i = view.DataGridView1.SelectedCells(0).RowIndex
    '        For j = 0 To view.DataGridView1.Rows.Count - 1
    '            If user_attributes(j).is_lookup = True Then
    '                lu = New DataGridViewComboBoxCell
    '                lu.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing

    '                For k = 0 To user_attributes(j).lookup_values.count - 1
    '                    lu.Items.Add(user_attributes(j).lookup_values(k))
    '                Next
    '                lu.Items.Add("")
    '                lus.Add(lu)

    '            ElseIf user_attributes(j).type_id = 3 Then
    '                lu = New DataGridViewComboBoxCell
    '                lu.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing

    '                lu.Items.Add("")
    '                lu.Items.Add("True")
    '                lu.Items.Add("False")
    '                lus.Add(lu)

    '            Else
    '                lu = New DataGridViewComboBoxCell
    '                lu.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing

    '                If IsNothing(hold_vals(j)) Then
    '                    lu.Items.Add("")
    '                Else
    '                    lu.Items.Add(hold_vals(j))
    '                End If
    '                lus.Add(lu)

    '            End If
    '        Next
    '        use_combo = False
    '        If user_attributes(i).is_lookup = True Then
    '            use_combo = True
    '        ElseIf user_attributes(i).type_id = 3 Then
    '            use_combo = True
    '        End If
    '        REM 
    '        Dcombobox.Width = view.DataGridView1.Columns(2).Width

    '        dtextbox.Width = view.DataGridView1.Columns(2).Width
    '        If view.DataGridView1.SelectedCells(0).RowIndex = 15 Then
    '            dtextbox.MaxInputLength = 10000
    '        Else
    '            dtextbox.MaxInputLength = 250
    '        End If


    '        view.DataGridView1.Columns.RemoveAt(2)
    '        If use_combo = True Then
    '            Dcombobox.Name = "Values"
    '            view.DataGridView1.Columns.Insert(2, Dcombobox)
    '        Else
    '            dtextbox.Name = "Values"
    '            view.DataGridView1.Columns.Insert(2, dtextbox)
    '        End If
    '        REM reset value
    '        no_action = True
    '        For i = 0 To view.DataGridView1.Rows.Count - 1
    '            If use_combo = True Then
    '                view.DataGridView1.Item(3, i) = lus(i)
    '            End If
    '            If hold_vals(i) <> "" Then
    '                view.DataGridView1.Item(3, i).Value = hold_vals(i)
    '            End If
    '        Next

    '        view.DataGridView1.Item(3, row_id).Selected = True
    '        no_action = False
    '        set_data_type_Control = True
    '        load_user_toolbar()

    '    Catch ex As Exception
    '        Dim oerr As New bc_cs_error_log("bc_am_users", "set_data_type_control", bc_cs_error_codes.USER_DEFINED, ex.Message)

    '    End Try

    'End Function
    REM SIM MAY 2013
    Public Sub assign_other_roles()
        Try
            Dim fedit As New bc_am_cp_assign
            REM SIM MAY2
            fedit.num = 0
            REM ====
            fedit.llfilter.Visible = False
            fedit.tlfilter.Visible = False
            fedit.ltitle.Text = "Assign other role(s) to " + Me.current_user.first_name + " " + Me.current_user.surname
            Dim found As Boolean = False
            Dim i As Integer

            For i = 0 To Me.container.oAdmin.roles.Count - 1
                found = False
                For j = 0 To current_user.other_roles.Count - 1
                    If current_user.other_roles(j) = Me.container.oAdmin.roles(i).id Then
                        If Me.container.oAdmin.roles(i).inactive = False Then
                            fedit.lselparent.Items.Add(Me.container.oAdmin.roles(i).description)
                        Else
                            fedit.lselparent.Items.Add(Me.container.oAdmin.roles(i).description + " (inactive)")
                        End If
                        found = True
                        Exit For
                    End If
                Next
                If found = False Then
                    If Me.container.oAdmin.roles(i).inactive = False Then
                        fedit.lallparent.Items.Add(Me.container.oAdmin.roles(i).description)
                    End If
                End If
            Next
            If (fedit.lselparent.Items.Count >= fedit.num) Or (fedit.lselparent.Items.Count > 0 And fedit.num = -1) Then
                fedit.Bok.Enabled = True
            End If
            fedit.ShowDialog()
            If fedit.cancel_selected = True Then
                Exit Sub
            End If
            current_user.other_roles.Clear()
            REM copy over new ones
            Dim oba As bc_om_user_role
            i = 0
            While i < Me.container.oAdmin.roles.Count
                If Me.container.oAdmin.roles(i).id < 0 Then
                    Me.container.oAdmin.roles.RemoveAt(i)
                Else
                    i = i + 1
                End If
            End While
            For i = 0 To fedit.new_entities.Count - 1
                oba = New bc_om_user_role
                oba.id = (i * -1) - 1
                oba.description = fedit.new_entities(i)
                container.oAdmin.roles.Add(oba)
            Next

            For i = 0 To container.oAdmin.roles.Count - 1
                found = False
                For j = 0 To fedit.lselparent.Items.Count - 1
                    If fedit.lselparent.Items(j) = container.oAdmin.roles(i).description Or fedit.lselparent.Items(j) + " (inactive)" = container.oAdmin.roles(i).description Then
                        current_user.other_roles.Add(container.oAdmin.roles(i).id)
                        found = True
                    End If
                    If found = True Then
                        Exit For
                    End If
                Next
            Next

            Me.load_node_tree()
            Me.set_change()
            Me.check_warnings()


        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_users", "assign_other_roles", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try



    End Sub
    REM ====
    Public Sub assign_bus_area()
        Try
            Dim fedit As New bc_am_cp_assign
            fedit.num = -1
            fedit.llfilter.Visible = False
            fedit.tlfilter.Visible = False
            fedit.Ltitle.Text = "Assign business area(s) to " + Me.current_user.first_name + " " + Me.current_user.surname
            Dim found As Boolean = False
            Dim i As Integer
            For i = 0 To container.oAdmin.business_areas.Count - 1
                found = False
                For j = 0 To current_user.bus_areas.Count - 1
                    If current_user.bus_areas(j) = container.oAdmin.business_areas(i).id Then
                        If container.oAdmin.business_areas(i).inactive = False Then
                            fedit.lselparent.Items.Add(container.oAdmin.business_areas(i).description)
                        Else
                            fedit.lselparent.Items.Add(container.oAdmin.business_areas(i).description + " (inactive)")
                        End If
                        found = True
                        Exit For
                    End If
                Next
                If found = False Then
                    If container.oAdmin.business_areas(i).inactive = False Then
                        fedit.lallparent.Items.Add(container.oAdmin.business_areas(i).description)
                    End If
                End If
            Next
            If (fedit.lselparent.Items.Count >= fedit.num) Or (fedit.lselparent.Items.Count > 0 And fedit.num = -1) Then
                fedit.Bok.Enabled = True
            End If
            fedit.ShowDialog()
            If fedit.cancel_selected = True Then
                Exit Sub
            End If
            current_user.bus_areas.Clear()
            REM copy over new ones
            Dim oba As bc_om_bus_area
            i = 0
            While i < container.oAdmin.business_areas.Count
                If container.oAdmin.business_areas(i).id < 0 Then
                    container.oAdmin.business_areas.RemoveAt(i)
                Else
                    i = i + 1
                End If
            End While
            For i = 0 To fedit.new_entities.Count - 1
                oba = New bc_om_bus_area
                oba.id = (i * -1) - 1
                oba.description = fedit.new_entities(i)
                container.oAdmin.business_areas.Add(oba)
            Next

            For i = 0 To container.oAdmin.business_areas.Count - 1
                found = False
                For j = 0 To fedit.lselparent.Items.Count - 1
                    If fedit.lselparent.Items(j) = container.oAdmin.business_areas(i).description Or fedit.lselparent.Items(j) + " (inactive)" = container.oAdmin.business_areas(i).description Then
                        current_user.bus_areas.Add(container.oAdmin.business_areas(i).id)
                        found = True
                    End If
                    If found = True Then
                        Exit For
                    End If
                Next
            Next

            Me.load_node_tree()
            Me.set_change()
            Me.check_warnings()


        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_users", "assign_bus_area", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try



    End Sub
    Public Sub assign_user_associations()
        Try

            Dim fedit As New bc_am_cp_assign
            fedit.num = 0
            fedit.llfilter.Visible = False
            fedit.tlfilter.Visible = False
            fedit.ltitle.Text = "Assign user association(s) to " + Me.current_user.first_name + " " + Me.current_user.surname
            Dim found As Boolean = False
            Dim i As Integer
            For i = 0 To container.oUsers.user.Count - 1
                found = False
                For j = 0 To current_user.associated_users.Count - 1
                    If current_user.associated_users(j) = container.oUsers.user(i).id Then
                        If container.oPubtypes.process_switches.surname_first = True Then
                            If container.oUsers.user(i).inactive = False Then
                                fedit.lselparent.Items.Add(container.oUsers.user(i).surname + ", " + container.oUsers.user(i).first_name)
                            Else
                                fedit.lselparent.Items.Add(container.oUsers.user(i).surname + ", " + container.oUsers.user(i).first_name + " (inactive)")
                            End If
                        Else
                            If container.oUsers.user(i).inactive = False Then
                                fedit.lselparent.Items.Add(container.oUsers.user(i).first_name + " " + container.oUsers.user(i).surname)
                            Else
                                fedit.lselparent.Items.Add(container.oUsers.user(i).first_name + " " + container.oUsers.user(i).surname + " (inactive)")
                            End If
                        End If
                        found = True
                        Exit For
                    End If
                Next
                If found = False Then
                    If container.oUsers.user(i).inactive = False And current_user.id <> container.oUsers.user(i).id Then
                        If container.oPubtypes.process_switches.surname_first = True Then
                            fedit.lallparent.Items.Add(container.oUsers.user(i).surname + ", " + container.oUsers.user(i).first_name)
                        Else
                            fedit.lallparent.Items.Add(container.oUsers.user(i).first_name + " " + container.oUsers.user(i).surname)
                        End If

                    End If
                    End If
            Next
            If (fedit.lselparent.Items.Count >= fedit.num) Or (fedit.lselparent.Items.Count > 0 And fedit.num = -1) Then
                fedit.Bok.Enabled = True
            End If
            fedit.Label2.Visible = False
            fedit.Clist.Visible = False
            fedit.Tnew.Visible = False
            fedit.lallparent.Height = fedit.lselparent.Height
            Dim dp As System.Drawing.Point
            dp.X = fedit.lallparent.Location.X
            dp.Y = fedit.lselparent.Location.Y
            fedit.lallparent.Location = dp

            fedit.ShowDialog()
            If fedit.cancel_selected = True Then
                Exit Sub
            End If
            current_user.associated_users.Clear()
            REM copy over new ones
            'Dim oba As bc_om_bus_area
            'i = 0
            'While i < container.oAdmin.business_areas.Count
            '    If container.oAdmin.business_areas(i).id < 0 Then
            '        container.oAdmin.business_areas.RemoveAt(i)
            '    Else
            '        i = i + 1
            '    End If
            'End While
            'For i = 0 To fedit.new_entities.Count - 1
            '    oba = New bc_om_bus_area
            '    oba.id = (i * -1) - 1
            '    oba.description = fedit.new_entities(i)
            '    container.oAdmin.business_areas.Add(oba)
            'Next

            For i = 0 To container.oUsers.user.Count - 1
                found = False
                For j = 0 To fedit.lselparent.Items.Count - 1
                    If container.oPubtypes.process_switches.surname_first = True Then
                        If fedit.lselparent.Items(j) = container.oUsers.user(i).surname + ", " + container.oUsers.user(i).first_name Or fedit.lselparent.Items(j) + " (inactive)" = container.oUsers.user(i).first_name + " " + container.oUsers.user(i).surname Then
                            current_user.associated_users.Add(container.oUsers.user(i).id)
                            found = True
                        End If
                    Else
                        If fedit.lselparent.Items(j) = container.oUsers.user(i).first_name + " " + container.oUsers.user(i).surname Or fedit.lselparent.Items(j) + " (inactive)" = container.oUsers.user(i).first_name + " " + container.oUsers.user(i).surname Then
                            current_user.associated_users.Add(container.oUsers.user(i).id)
                            found = True
                        End If
                    End If
                    If found = True Then
                        Exit For
                    End If
                Next
            Next

            Me.load_node_tree()
            Me.set_change()
            Me.check_warnings()


        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_users", "assign_user_associations", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try



    End Sub
    REM FIL July 2012
    Public Sub assign_entity()
        Try
            Dim fedit As New bc_am_cp_assign
            Dim lclass As String
            Dim class_id As Long
            Dim orig_prefs As New ArrayList
            lclass = view.tentities.SelectedNode.Text
            fedit.num = 0
            fedit.fcontainer = container

            Dim dp As New System.Drawing.Point
            dp.X = fedit.lallparent.Location.X
            dp.Y = fedit.lselparent.Location.Y
            fedit.lallparent.Height = fedit.lselparent.Height - 25
            fedit.lallparent.Location = dp
            fedit.Tnew.Visible = False
            fedit.Label1.Visible = False
            fedit.Label2.Visible = False
            fedit.filter_class = lclass

            Dim tx As String = ""
            Dim pref_id As Long


            Dim i As Integer
            'If Me.container.oUsers.pref_types.Count > 1 Then
            tx = view.tentities.SelectedNode.Parent.Text
            For i = 0 To Me.container.oUsers.pref_types.Count - 1
                If Me.container.oUsers.pref_types(i).name = tx Then
                    pref_id = Me.container.oUsers.pref_types(i).id
                    Exit For
                End If
            Next
            fedit.ltitle.Text = "Assign " + lclass + "(s) to " + Me.current_user.first_name + " " + Me.current_user.surname + " for " + tx
            'Else
            'If Me.container.oUsers.pref_types.Count = 1 Then
            '    pref_id = Me.container.oUsers.pref_types(0).id
            'Else
            '    pref_id = 1
            'End If
            'fedit.ltitle.Text = "Assign " + lclass + "(s) to " + Me.current_user.first_name + " " + Me.current_user.surname
            'End If

            Dim found As Boolean = False

            For i = 0 To container.oEntities.entity.Count - 1

                found = False
                If container.oEntities.entity(i).class_name = lclass Then
                    class_id = container.oEntities.entity(i).class_id
                    For j = 0 To current_user.prefs.Count - 1
                        If current_user.prefs(j).entity_id = container.oEntities.entity(i).id And current_user.prefs(j).pref_type = pref_id Then
                            If container.oEntities.entity(i).inactive = False Then
                                fedit.lselparent.Items.Add(container.oEntities.entity(i).name)
                            Else
                                fedit.lselparent.Items.Add(container.oEntities.entity(i).name + " (inactive)")
                            End If
                            found = True
                            orig_prefs.Add(current_user.prefs(j))
                            Exit For
                        End If
                    Next
                    If found = False Then
                        If container.oEntities.entity(i).inactive = False Then
                            fedit.lallparent.Items.Add(container.oEntities.entity(i).name)
                        End If
                    End If
                End If
            Next
            If (fedit.lselparent.Items.Count >= fedit.num) Or (fedit.lselparent.Items.Count > 0 And fedit.num = -1) Then
                fedit.Bok.Enabled = True
            End If

            fedit.tlfilter.Visible = False
            fedit.BlueCurve_TextSearch1.SearchClass = class_id

            fedit.BlueCurve_TextSearch1.Visible = True
            fedit.BlueCurve_TextSearch1.Enabled = True
            fedit.BlueCurve_TextSearch1.AttributeControlBuild(False)
            fedit.BlueCurve_TextSearch1.SearchUserEntitiesOnly = False
            fedit.BlueCurve_TextSearch1.SearchBuildEntitiesOnly = False
            fedit.BlueCurve_TextSearch1.showinactive = False
            fedit.BlueCurve_TextSearch1.ExcludeControl = "lselparent"

            fedit.ShowDialog()
            If fedit.cancel_selected = True Then
                Exit Sub
            End If
            REM clear out current prefs for this class
            i = 0
            Dim tprefs As New List(Of bc_om_user_pref)

            REM prefs prior to change
            Dim prev_entities As New List(Of Long)
            Dim rem_entities As New List(Of Long)
            Dim sel_entities As New List(Of Long)
            Dim new_entities As New List(Of Long)
            For i = 0 To current_user.prefs.Count - 1
                If current_user.prefs(i).class_id = class_id And current_user.prefs(i).pref_type = pref_id Then
                    prev_entities.Add(current_user.prefs(i).entity_id)
                End If
            Next
            For i = 0 To container.oEntities.entity.Count - 1
                If container.oEntities.entity(i).class_name = lclass Then
                    For j = 0 To fedit.lselparent.Items.Count - 1
                        If fedit.lselparent.Items(j) = container.oEntities.entity(i).name Then
                            sel_entities.Add(container.oEntities.entity(i).id)
                        End If
                    Next
                End If
            Next

            For i = 0 To prev_entities.Count - 1
                found = False
                For j = 0 To sel_entities.Count - 1
                    If prev_entities(i) = sel_entities(j) Then
                        found = True
                        Exit For
                    End If
                Next
                If found = False Then
                    rem_entities.Add(prev_entities(i))
                End If
            Next

            For i = 0 To sel_entities.Count - 1
                found = False
                For j = 0 To prev_entities.Count - 1
                    If prev_entities(j) = sel_entities(i) Then
                        found = True
                        Exit For
                    End If
                Next
                If found = False Then
                    new_entities.Add(sel_entities(i))
                End If
            Next

            REM remove deselected prefs
            For i = 0 To rem_entities.Count - 1
                For j = 0 To current_user.prefs.Count - 1
                    If current_user.prefs(j).pref_type = pref_id AndAlso current_user.prefs(j).entity_id = rem_entities(i) Then
                        current_user.prefs.RemoveAt(j)
                        Exit For
                    End If
                Next
            Next

            REM add new entities
            For i = 0 To new_entities.Count - 1
                add_new_pref(new_entities(i), pref_id, tx, lclass)
            Next


            Me.load_node_tree()
            Me.set_change()
            Me.check_warnings()


        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_users", "assign_entity", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try

    End Sub

    Private Sub add_new_pref(entity_id As Long, pref_id As Integer, pref_name As String, class_name As String)
        Try
            Dim opref As bc_om_user_pref
            For i = 0 To container.oEntities.entity.Count - 1
                If container.oEntities.entity(i).id = entity_id Then

                    opref = New bc_om_user_pref
                    opref.class_id = container.oEntities.entity(i).class_id
                    opref.entity_id = container.oEntities.entity(i).id
                    opref.entity_name = container.oEntities.entity(i).name
                    opref.class_name = class_name
                    opref.user_name = view.Lusers.SelectedItems(0).Text
                    opref.user_id = current_user.id
                    opref.rating = 1
                    opref.pref_type = pref_id
                    REM FIL 5.5
                    opref.pref_name = pref_name
                    set_users_for_prefs(current_user, opref)

                End If
            Next

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_users", "add_new_pref", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try



    End Sub

    Public Sub assign_new_class()
        Try
            Dim fedit As New bc_am_cp_assign
            fedit.Label2.Text = "Choose Class"
            fedit.Clist.Visible = True
            fedit.Tnew.Visible = False
            fedit.tlfilter.Enabled = False
            fedit.num = -1
            fedit.Bok.Enabled = False
            fedit.Label1.Visible = False
            fedit.fcontainer = container
            Dim i As Integer
            Dim tx As String
            Dim pref_id As Long
            Dim pref_tx As String = ""
            'If Me.container.oUsers.pref_types.Count > 1 Then

            tx = view.tentities.SelectedNode.Text

            fedit.ltitle.Text = "Assign new associated class and entities to " + Me.current_user.first_name + " " + Me.current_user.surname + " for " + tx
            For i = 0 To Me.container.oUsers.pref_types.Count - 1
                If Me.container.oUsers.pref_types(i).name = tx Then
                    pref_id = Me.container.oUsers.pref_types(i).id
                    pref_tx = tx
                    Exit For
                End If
            Next
            'Else
            'If Me.container.oUsers.pref_types.Count = 1 Then
            '    pref_id = Me.container.oUsers.pref_types(0).id
            'Else
            '    pref_id = 1
            'End If
            'fedit.ltitle.Text = "Assign new associated class and entities to " + Me.current_user.first_name + " " + Me.current_user.surname
            'End If
            Dim found As Boolean = False

            REM populate list box with unassigned classes
            For i = 0 To container.oClasses.classes.Count - 1
                found = False
                For j = 0 To current_user.prefs.Count - 1
                    If current_user.prefs(j).class_id = container.oClasses.classes(i).class_id And current_user.prefs(j).pref_type = pref_id Then
                        found = True
                        Exit For
                    End If
                Next
                If found = False Then
                    fedit.Clist.Items.Add(container.oClasses.classes(i).class_name)
                End If
            Next

            If fedit.num = -1 Then
                If fedit.lselparent.Items.Count > 0 Then
                    fedit.Bok.Enabled = True
                End If
            ElseIf (fedit.lselparent.Items.Count >= fedit.num) Then
                fedit.Bok.Enabled = True
            End If
            fedit.ShowDialog()
            If fedit.cancel_selected = True Then
                Exit Sub
            End If
            REM clear out current prefs for this class

            Dim opref As bc_om_user_pref
            Dim lclass As String
            lclass = fedit.Clist.Text
            For i = 0 To container.oEntities.entity.Count - 1
                found = False
                If container.oEntities.entity(i).class_name = lclass Then
                    For j = 0 To fedit.lselparent.Items.Count - 1
                        If fedit.lselparent.Items(j) = container.oEntities.entity(i).name Then
                            add_new_pref(container.oEntities.entity(i).id, pref_id, tx, lclass)

                            Exit For
                        End If
                    Next
                End If
            Next

            Me.load_node_tree()
            Me.set_change()
            Me.check_warnings()

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_users", "assign_new_class", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try

    End Sub
    Private Sub set_users_for_prefs(ByRef current_user As bc_om_user, opref As bc_om_user_pref)
        Dim puser As bc_om_user
        opref.users.Clear()

        REM get next rating for and all users for this entity
        Dim prat As New bc_om_get_pref_rating
        prat.pref_type_id = opref.pref_type
        prat.entity_id = opref.entity_id
        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
            prat.db_read()

        Else
            prat.tmode = bc_cs_soap_base_class.tREAD
            If prat.transmit_to_server_and_receive(prat, True) = False Then
                Exit Sub
            End If
        End If
        Dim user_already_assigned As Boolean = False
        For k = 0 To prat.users.Count - 1

            puser = New bc_om_user
            puser.id = prat.users(k).id
            puser.first_name = prat.users(k).first_name + " " + prat.users(k).surname
            puser.surname = ""
            puser.inactive = prat.users(k).inactive
            puser.rating = k + 1
            opref.users.Add(puser)
            If puser.id = current_user.id Then
                user_already_assigned = True
            End If
        Next
        REM if user was removed and readded dont include it 
        If user_already_assigned = False Then
            opref.rating = prat.users.Count + 1
            puser = New bc_om_user
            puser.id = current_user.id
            puser.first_name = opref.user_name
            puser.surname = ""
            puser.rating = opref.rating
            puser.inactive = 0
            opref.users.Add(puser)
        End If
        current_user.prefs.Add(opref)
    End Sub
    Public Sub add_new_language_menu()
        view.maintain_mode = 3
        Dim ofrm As New bc_am_cp_edit
        ofrm.ltitle.Text = "Add new language and assign to " + view.Lusers.SelectedItems(0).Text
        ofrm.ShowDialog()
        If ofrm.cancel_selected = True Then
            Exit Sub
        End If
        If container.oAdmin.languages(container.oAdmin.languages.Count - 1).language_id = -1 Then
            container.oAdmin.languages.RemoveAt(container.oAdmin.languages.Count - 1)
        End If

        add_new(ofrm.Tentry.Text)
    End Sub
    Public Sub add_new_office_menu()
        view.maintain_mode = 1
        Dim ofrm As New bc_am_cp_edit
        ofrm.ltitle.Text = "Add new office and assign to " + view.Lusers.SelectedItems(0).Text
        ofrm.ShowDialog()
        If ofrm.cancel_selected = True Then
            Exit Sub
        End If

        If container.oAdmin.offices(container.oAdmin.offices.Count - 1).id = -1 Then
            container.oAdmin.offices.RemoveAt(container.oAdmin.offices.Count - 1)
        End If
        add_new(ofrm.Tentry.Text)
    End Sub
    Public Sub add_new_role_menu()
        view.maintain_mode = 0
        Dim ofrm As New bc_am_cp_edit
        ofrm.ltitle.Text = "Add new role and assign to " + view.Lusers.SelectedItems(0).Text
        ofrm.ShowDialog()
        If ofrm.cancel_selected = True Then
            Exit Sub
        End If
        If container.oAdmin.roles(container.oAdmin.roles.Count - 1).id = 0 Then
            container.oAdmin.roles.RemoveAt(container.oAdmin.roles.Count - 1)
        End If
        add_new(ofrm.Tentry.Text)
    End Sub
    Public Sub add_new(ByVal item As String)
        Try
            Dim omsg As bc_cs_message
            REM check item doesnt already exist
            Select Case view.maintain_mode
                Case 0
                    For i = 0 To container.oAdmin.roles.Count - 1
                        If Trim(UCase(container.oAdmin.roles(i).description)) = Trim(UCase(item)) Then
                            omsg = New bc_cs_message("Blue Curve", "Role " + item + " already exists!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                            Exit Sub
                        End If
                    Next
                    REM set in attributes
                    Dim orole As New bc_om_user_role
                    orole.description = item
                    orole.id = 0
                    container.oAdmin.roles.Add(orole)
                    user_attributes(view.DataGridView1.SelectedCells(0).RowIndex).value = item
                    For i = 0 To container.oUsers.user_system_attributes.Count - 1
                        If container.oUsers.user_system_attributes(i).field_name = user_attributes(view.DataGridView1.SelectedCells(0).RowIndex).field_name Then
                            container.oUsers.user_system_attributes(i).lookup_vals.Add(item)
                            container.oUsers.user_system_attributes(i).lookup_vals_key.Add(0)
                            container.oUsers.user_system_attributes(i).lookup_vals_inactive.Add(False)
                            Exit For
                        End If

                    Next

                    Dim dtextbox As New DataGridViewTextBoxColumn
                    dtextbox.Width = view.DataGridView1.Columns(3).Width
                    dtextbox.MaxInputLength = 250
                    view.DataGridView1.Columns.RemoveAt(3)
                    dtextbox.Name = "Values"
                    view.DataGridView1.Columns.Insert(3, dtextbox)
                    user_attributes(view.DataGridView1.SelectedCells(0).RowIndex).value_changed = True
                    For i = 0 To user_attributes.Count - 1
                        view.DataGridView1.Item(3, i).Value = user_attributes(i).value
                    Next
                    view.DataGridView1.Item(0, view.DataGridView1.SelectedCells(0).RowIndex).Value = view.tabimages.Images(att_edited_icon)

                    Me.set_change()
                Case 1
                    For i = 0 To container.oAdmin.offices.Count - 1
                        If Trim(UCase(container.oAdmin.offices(i).description)) = Trim(UCase(item)) Then
                            omsg = New bc_cs_message("Blue Curve", "Office " + item + " already exists!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                            Exit Sub
                        End If
                    Next
                    REM set in attributes
                    Dim ooffice As New bc_om_user_office
                    ooffice.description = item
                    ooffice.id = -1
                    container.oAdmin.offices.Add(ooffice)
                    user_attributes(view.DataGridView1.SelectedCells(0).RowIndex).value = item
                    For i = 0 To container.oUsers.user_system_attributes.Count - 1
                        If container.oUsers.user_system_attributes(i).field_name = user_attributes(view.DataGridView1.SelectedCells(0).RowIndex).field_name Then
                            container.oUsers.user_system_attributes(i).lookup_vals.Add(item)
                            container.oUsers.user_system_attributes(i).lookup_vals_key.Add(-1)
                            container.oUsers.user_system_attributes(i).lookup_vals_inactive.Add(False)
                            Exit For
                        End If

                    Next

                    Dim dtextbox As New DataGridViewTextBoxColumn
                    dtextbox.Width = view.DataGridView1.Columns(3).Width
                    dtextbox.MaxInputLength = 250
                    view.DataGridView1.Columns.RemoveAt(3)
                    dtextbox.Name = "Values"
                    view.DataGridView1.Columns.Insert(3, dtextbox)
                    user_attributes(view.DataGridView1.SelectedCells(0).RowIndex).value_changed = True
                    For i = 0 To user_attributes.Count - 1
                        view.DataGridView1.Item(3, i).Value = user_attributes(i).value
                    Next
                    view.DataGridView1.Item(0, view.DataGridView1.SelectedCells(0).RowIndex).Value = view.tabimages.Images(att_edited_icon)

                    Me.set_change()
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
                    user_attributes(view.DataGridView1.SelectedCells(0).RowIndex).value = item
                    For i = 0 To container.oUsers.user_system_attributes.Count - 1
                        If container.oUsers.user_system_attributes(i).field_name = user_attributes(view.DataGridView1.SelectedCells(0).RowIndex).field_name Then
                            container.oUsers.user_system_attributes(i).lookup_vals.Add(item)
                            container.oUsers.user_system_attributes(i).lookup_vals_key.Add(-1)
                            container.oUsers.user_system_attributes(i).lookup_vals_inactive.Add(False)
                            Exit For
                        End If

                    Next

                    Dim dtextbox As New DataGridViewTextBoxColumn
                    dtextbox.Width = view.DataGridView1.Columns(3).Width
                    dtextbox.MaxInputLength = 250
                    view.DataGridView1.Columns.RemoveAt(3)
                    dtextbox.Name = "Values"
                    view.DataGridView1.Columns.Insert(3, dtextbox)
                    user_attributes(view.DataGridView1.SelectedCells(0).RowIndex).value_changed = True
                    For i = 0 To user_attributes.Count - 1
                        view.DataGridView1.Item(3, i).Value = user_attributes(i).value
                    Next
                    view.DataGridView1.Item(0, view.DataGridView1.SelectedCells(0).RowIndex).Value = view.tabimages.Images(att_edited_icon)

                    Me.set_change()

            End Select

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_users", "add_new", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

        End Try
    End Sub
    Public Sub show_user_audit()
        Dim fs As New bc_am_entity_user_audit
        Dim cfs As New Cbc_am_entity_user_audit
        If cfs.load_data(fs, current_user.id, bc_om_entity_user_audit.EKEY_TYPE.USER, current_user.first_name + " " + current_user.surname) = True Then
            fs.ShowDialog()
        End If
    End Sub

    Public Sub show_audit(idx As Integer, pref_type_name As String)
        Dim fs As New bc_am_audit_entity_links

        Dim cs As New Cbc_am_audit_entity_links
        If cs.load_data(fs, 0, 0, 0, 0, pref_type_name, current_user.first_name + " " + current_user.surname, "", idx - 2, current_user.id, bc_om_audit_links.EAUDIT_TYPE.PREFS_FOR_USER) = True Then
            fs.ShowDialog()
        End If
    End Sub

    Public Sub show_audit_other_areas(idx As Integer, type_name As String)
        Dim fs As New bc_am_attribute_audit
        Dim cfs As New Cbc_am_attribute_audit
        Dim etype As New bc_om_attribute_audit.EATTRIBUTE_TYPE
        Dim attribute_name As String
        If type_name <> "Business Areas" And type_name <> "User Associations" And type_name <> "Other Roles" Then
            Exit Sub
        End If

        If idx = 0 Then
            etype = bc_om_attribute_audit.EATTRIBUTE_TYPE.BUS_AREA
            attribute_name = "Business Area"
        ElseIf idx = 1 Then
            etype = bc_om_attribute_audit.EATTRIBUTE_TYPE.USER_ASSOCIATIONS
            attribute_name = "User Associations"
        Else
            etype = bc_om_attribute_audit.EATTRIBUTE_TYPE.OTHER_ROLES
            attribute_name = "Other Roles"
        End If
        If cfs.load_data(fs, current_user.first_name + " " + current_user.surname, attribute_name, 0, current_user.id, etype, False, False) = True Then
            fs.ShowDialog()
        End If
    End Sub


    Public Sub load_user_pic()
        If container.mode = 0 Or current_user.inactive = True Then
            Exit Sub
        End If
        Try
            Dim fp As New bc_am_picture_attribute
            Dim cp As New Cbc_am_picture_attribue
            Dim cimage As System.Drawing.Image = Nothing
            If view.puserpic.Visible = True Then
                cimage = view.puserpic.Image
            End If
            If cp.load_data(current_user, fp, cimage) = True Then
                fp.ShowDialog()
                If cp.changed = True Then
                    current_user.picture_change = True
                    set_change()
                    fp = Nothing
                    If current_user.picture_extension <> "" Then
                        load_thumbnail()
                    Else
                        view.Pnouser.Visible = True
                        view.puserpic.Visible = False

                    End If

                End If
            End If
            Exit Sub
            'Dim ofrmfile As New OpenFileDialog
            'ofrmfile.Filter = "bitmap (*.bmp)|*.bmp|JPEG Compressed Image (*.jpg)|*.jpg|GIF files (*.gif)|*.gif"
            'ofrmfile.ShowDialog()
            'If ofrmfile.FileName = "" Then
            '    Exit Sub
            'End If
            'Dim fs As New bc_cs_file_transfer_services
            'fs.write_document_to_bytestream(ofrmfile.FileName, current_user.picture, Nothing)
            'view.Pnouser.Visible = False
            'view.puserpic.ImageLocation = ofrmfile.FileName
            'view.puserpic.Visible = True
            'set_change()

            Dim ofrm As New bc_am_cp_edit(current_user)
            ofrm.Text = "User Image"
            ofrm.Size = New System.Drawing.Size(500, 300)
            ofrm.ltitle.Text = ""
            ofrm.centry.Visible = False
            ofrm.centry.Sorted = False
            ofrm.Tentry.Visible = False
            If view.Pnouser.Visible = False Then
                ofrm.userpic.Image = view.puserpic.Image
            Else
                ofrm.userpic.Image = ofrm.defaultpic.Image
            End If
            ofrm.userpic.Visible = True
            ofrm.uxAssignPicture.Visible = True
            ofrm.uxRemovePicture.Visible = True

            ofrm.ShowDialog()

            If ofrm.cancel_selected = True Then
                Exit Sub
            End If
            view.Pnouser.Visible = False
            view.puserpic.Image = ofrm.userpic.Image
            view.puserpic.ImageLocation = ofrm.userpic.ImageLocation
            view.puserpic.Visible = True
            set_change()

        Catch ex As Exception

        End Try

    End Sub
    Private Function set_maintain_mode() As String

        Dim tx As String
        Try
            tx = view.tusers.SelectedNode.Parent.Text
            set_maintain_mode = ""
            If tx = "Users by Security Role" Then
                Me.maintain_mode = 0
                set_maintain_mode = "Role"
            ElseIf tx = "Users by Office" Then
                Me.maintain_mode = 1
                set_maintain_mode = "Office"
            ElseIf tx = "Users by Business Area" Then
                Me.maintain_mode = 2
                set_maintain_mode = "Business Areas"
            ElseIf tx = "Users by Language" Then
                Me.maintain_mode = 3
                set_maintain_mode = "Language"

            End If
        Catch
            set_maintain_mode = ""

        End Try
    End Function

    Public Sub change_item_menu()

        Dim tx As String
        tx = set_maintain_mode()
        Dim ofrm As New bc_am_cp_edit
        ofrm.ltitle.Text = "Change name of " + tx + ": " + view.tusers.SelectedNode.Text
        ofrm.ShowDialog()
        If ofrm.cancel_selected = True Then
            Exit Sub
        End If
        view.Cursor = Cursors.WaitCursor
        change_item(ofrm.Tentry.Text, tx)
        view.Cursor = Cursors.Default

    End Sub
    Public Sub item_active()
        Dim found As Boolean = False
        Dim item As String = ""
        Try
            Dim tx As String
            tx = set_maintain_mode()
            If InStr(view.tusers.SelectedNode.Text, "(inactive)") = 0 Then
                Select Case Me.maintain_mode
                    Case 0
                        For i = 0 To container.oUsers.user.Count - 1
                            If container.oUsers.user(i).role = view.tusers.SelectedNode.Text And container.oUsers.user(i).inactive = False Then
                                item = "Cannot make role " + view.tusers.SelectedNode.Text + " inactive as active users are assigned to it!"
                                found = True
                                Exit For
                            End If
                        Next
                    Case 1
                        For i = 0 To container.oUsers.user.Count - 1
                            For j = 0 To container.oAdmin.offices.Count - 1
                                If container.oAdmin.offices(j).description = view.tusers.SelectedNode.Text And container.oUsers.user(i).inactive = False Then
                                    If container.oUsers.user(i).office_id = container.oAdmin.offices(j).id Then
                                        item = "Cannot make office " + view.tusers.SelectedNode.Text + " inactive as active users are assigned to it!"
                                        found = True
                                        Exit For
                                    End If
                                    Exit For
                                End If
                            Next
                        Next
                    Case 2
                        For j = 0 To container.oAdmin.business_areas.Count - 1
                            If container.oAdmin.business_areas(j).description = view.tusers.SelectedNode.Text Then
                                For i = 0 To container.oUsers.user.Count - 1
                                    If container.oUsers.user(i).inactive = False Then
                                        For k = 0 To container.oAdmin.user_bus_areas.Count - 1
                                            If container.oAdmin.user_bus_areas(k).bus_area_id = container.oAdmin.business_areas(j).id And container.oAdmin.user_bus_areas(k).user_id = container.oUsers.user(i).id Then
                                                item = "Cannot make business area " + view.tusers.SelectedNode.Text + " inactive as active users are assigned to it!"
                                                found = True
                                                Exit For
                                            End If
                                        Next
                                        If found = True Then
                                            Exit For
                                        End If
                                    End If
                                Next
                                Exit For
                            End If
                        Next

                        If found = False Then
                            For i = 0 To container.oPubtypes.pubtype.Count - 1
                                For j = 0 To container.oAdmin.business_areas.Count - 1
                                    If container.oAdmin.business_areas(j).description = view.tusers.SelectedNode.Text And container.oPubtypes.pubtype(i).inactive = False Then
                                        If container.oPubtypes.pubtype(i).bus_area_id = container.oAdmin.business_areas(j).id Then
                                            item = "Cannot make business area " + view.tusers.SelectedNode.Text + " inactive as active publication types are assigned to it!"
                                            found = True
                                            Exit For
                                        End If
                                        Exit For
                                    End If
                                Next
                            Next
                        End If
                        REM for language and business area check users and pub types assigned.
                    Case 3
                        For i = 0 To container.oUsers.user.Count - 1
                            For j = 0 To container.oAdmin.languages.Count - 1
                                If container.oAdmin.languages(j).language_name = view.tusers.SelectedNode.Text And container.oUsers.user(i).inactive = False Then
                                    If container.oUsers.user(i).language_id = container.oAdmin.languages(j).language_id Then
                                        item = "Cannot make language " + view.tusers.SelectedNode.Text + " inactive as active users are assigned to it!"
                                        found = True
                                        Exit For
                                    End If
                                    Exit For
                                End If
                            Next
                        Next
                        If found = False Then
                            For i = 0 To container.oPubtypes.pubtype.Count - 1
                                For j = 0 To container.oAdmin.languages.Count - 1
                                    If container.oAdmin.languages(j).language_name = view.tusers.SelectedNode.Text And container.oPubtypes.pubtype(i).inactive = False Then
                                        If container.oPubtypes.pubtype(i).language = container.oAdmin.languages(j).language_id Then
                                            item = "Cannot make language " + view.tusers.SelectedNode.Text + " inactive as active publication types are assigned to it!"
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
                    Select Case Me.maintain_mode
                        Case 0
                            For i = 0 To container.oAdmin.roles.Count - 1
                                If container.oAdmin.roles(i).description = view.tusers.SelectedNode.Text Then
                                    container.oAdmin.roles(i).inactive = True
                                    container.oAdmin.roles(i).write_mode = bc_om_user_role.SET_INACTIVE

                                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                                        container.oAdmin.roles(i).db_write()
                                    Else
                                        container.oAdmin.roles(i).tmode = bc_om_user_role.tWRITE
                                        If container.oAdmin.roles(i).transmit_to_server_and_receive(container.oAdmin.roles(i), True) = False Then
                                            container.oAdmin.roles(i).inactive = False
                                            Exit Sub
                                        End If
                                    End If
                                    Exit For
                                End If
                            Next
                        Case 1
                            For i = 0 To container.oAdmin.offices.Count - 1
                                If container.oAdmin.offices(i).description = view.tusers.SelectedNode.Text Then
                                    container.oAdmin.offices(i).inactive = True
                                    container.oAdmin.offices(i).write_mode = bc_om_user_office.SET_INACTIVE

                                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                                        container.oAdmin.offices(i).db_write()
                                    Else
                                        container.oAdmin.offices(i).tmode = bc_om_user_role.tWRITE
                                        If container.oAdmin.offices(i).transmit_to_server_and_receive(container.oAdmin.offices(i), True) = False Then
                                            container.oAdmin.offices(i).inactive = False
                                            Exit Sub
                                        End If
                                    End If
                                End If
                            Next
                        Case 2
                            For i = 0 To container.oAdmin.business_areas.Count - 1
                                If container.oAdmin.business_areas(i).description = view.tusers.SelectedNode.Text Then
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
                                If container.oAdmin.languages(i).language_name = view.tusers.SelectedNode.Text Then
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
                    Dim nitem As String
                    nitem = view.tusers.SelectedNode.Text
                    load_users_tree()
                    load_user_attributes()
                    view.tusers.Nodes(Me.maintain_mode + 1).Expand()

                    For i = 0 To view.tusers.Nodes(1).Nodes.Count - 1
                        If view.tusers.Nodes(1).Nodes(i).Text = nitem Or view.tusers.Nodes(1).Nodes(i).Text = nitem + " (inactive)" Then
                            view.tusers.SelectedNode = view.tusers.Nodes(1).Nodes(i)
                            Exit For
                        End If
                    Next

                End If
            Else
                Dim ntx As String
                ntx = view.tusers.SelectedNode.Text
                If InStr(ntx, "(inactive)") > 0 Then
                    ntx = ntx.Substring(0, ntx.Length - 11)
                End If
                Select Case Me.maintain_mode
                    Case 0
                        For i = 0 To container.oAdmin.roles.Count - 1
                            If container.oAdmin.roles(i).description = ntx Then
                                container.oAdmin.roles(i).inactive = False
                                container.oAdmin.roles(i).write_mode = bc_om_user_role.SET_ACTIVE

                                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                                    container.oAdmin.roles(i).db_write()
                                Else
                                    container.oAdmin.roles(i).tmode = bc_om_user_role.tWRITE
                                    If container.oAdmin.roles(i).transmit_to_server_and_receive(container.oAdmin.roles(i), True) = False Then
                                        container.oAdmin.roles(i).inactive = True
                                        Exit Sub
                                    End If
                                End If
                                Exit For
                            End If
                        Next
                    Case 1
                        For i = 0 To container.oAdmin.offices.Count - 1
                            If container.oAdmin.offices(i).description = ntx Then
                                container.oAdmin.offices(i).inactive = False
                                container.oAdmin.offices(i).write_mode = bc_om_user_office.SET_ACTIVE

                                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                                    container.oAdmin.offices(i).db_write()
                                Else
                                    container.oAdmin.offices(i).tmode = bc_om_user_role.tWRITE
                                    If container.oAdmin.offices(i).transmit_to_server_and_receive(container.oAdmin.offices(i), True) = False Then
                                        container.oAdmin.offices(i).inactive = True
                                        Exit Sub
                                    End If
                                End If
                                Exit For
                            End If
                        Next
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
                Dim nitem As String
                nitem = view.tusers.SelectedNode.Text
                load_users_tree()
                load_user_attributes()
                view.tusers.Nodes(Me.maintain_mode + 1).Expand()

                For i = 0 To view.tusers.Nodes(1).Nodes.Count - 1
                    If view.tusers.Nodes(1).Nodes(i).Text = nitem Or view.tusers.Nodes(1).Nodes(i).Text + " (inactive)" = nitem Then
                        view.tusers.SelectedNode = view.tusers.Nodes(1).Nodes(i)
                        Exit For
                    End If
                Next
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_users", "item_active", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Private Sub change_item(ByVal tx As String, ByVal item As String)
        Try
            Dim omsg As bc_cs_message
            REM check item doesnt already exist
            Select Case maintain_mode
                Case 0
                    For i = 0 To container.oAdmin.roles.Count - 1
                        If UCase(Trim(container.oAdmin.roles(i).description)) = UCase(Trim(tx)) Then
                            omsg = New bc_cs_message("Blue Curve", item + ": " + tx + " already exists!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                            Exit Sub
                        End If
                    Next

                    '    If logged_on_role() Then
                    '        Exit Sub
                    '    End If
                    'Case 10
                    '    If logged_on_role() = True Then
                    '        Exit Sub
                    '    End If

                Case 1
                    For i = 0 To container.oAdmin.offices.Count - 1
                        If Trim(UCase(container.oAdmin.offices(i).description)) = Trim(UCase(tx)) Then
                            omsg = New bc_cs_message("Blue Curve", Trim(item) + ": " + tx + " already exists!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                            Exit Sub
                        End If
                    Next

                Case 2
                    For i = 0 To container.oAdmin.business_areas.Count - 1
                        If Trim(UCase(container.oAdmin.business_areas(i).description)) = Trim(UCase(tx)) Then
                            omsg = New bc_cs_message("Blue Curve", item + ": " + tx + " already exists!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                            Exit Sub
                        End If
                    Next
                Case 3
                    For i = 0 To container.oAdmin.languages.Count - 1
                        If Trim(UCase(container.oAdmin.languages(i).language_name)) = Trim(UCase(tx)) Then
                            omsg = New bc_cs_message("Blue Curve", item + ": " + tx + " already exists!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                            Exit Sub
                        End If
                    Next
            End Select


            'REM item entered
            'If Me.maintain_mode <> 10 Then
            '    If Me.ttchitem.Text = "" Then
            '        omsg = New bc_cs_message("Blue Curve", item + " must be entered!", bc_cs_message.MESSAGE,false,false,"Yes","No",true)
            '        Exit Sub
            '    End If
            'End If
            REM check item doesnt already exist

            Select Case Me.maintain_mode
                Case 0

                    For i = 0 To container.oAdmin.roles.Count - 1
                        If Trim(UCase(container.oAdmin.roles(i).description)) = Trim(UCase(view.tusers.SelectedNode.Text)) Then
                            With container.oAdmin.roles(i)
                                .write_mode = bc_om_user_role.UPDATE
                                If InStr(view.tusers.SelectedNode.Text, "Access Level") = 0 Then
                                    .description = tx
                                    'Else
                                    '    orole.description = container.oadmin.roles(i).description
                                    '    orole.level = Me.cboaccess.SelectedIndex
                                    'End If
                                    'For j = 0 To container.oadmin.roles(i).apps.count - 1
                                    '    orole.apps.Add(container.oadmin.roles(i).apps(j))
                                    'Next
                                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                                        .db_write()
                                    Else
                                        .tmode = bc_om_user_role.tWRITE
                                        If .transmit_to_server_and_receive(container.oAdmin.roles(i), True) = False Then
                                            Exit Sub
                                        End If
                                    End If
                                    REM update users as they use name not FK
                                    For j = 0 To container.oUsers.user.Count - 1
                                        If container.oUsers.user(j).role_id = .id Then
                                            container.oUsers.user(j).role = .description
                                        End If
                                    Next
                                End If
                            End With
                            Exit For
                        End If
                    Next
                Case 1

                    For i = 0 To container.oAdmin.offices.Count - 1
                        If Trim(UCase(container.oAdmin.offices(i).description)) = Trim(UCase(view.tusers.SelectedNode.Text)) Then
                            With container.oAdmin.offices(i)
                                .write_mode = bc_om_user_office.UPDATE
                                .description = Trim(tx)
                                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                                    .db_write()
                                Else
                                    .tmode = bc_om_user_role.tWRITE
                                    If .transmit_to_server_and_receive(container.oAdmin.offices(i), True) = False Then
                                        Exit Sub
                                    End If
                                End If
                            End With
                            Exit For
                        End If
                    Next

                Case 2
                    For i = 0 To container.oAdmin.business_areas.Count - 1
                        If Trim(UCase(container.oAdmin.business_areas(i).description)) = Trim(UCase(view.tusers.SelectedNode.Text)) Then
                            With container.oAdmin.business_areas(i)
                                .write_mode = bc_om_bus_area.UPDATE
                                .description = Trim(tx)
                                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                                    .db_write()
                                Else
                                    .tmode = bc_om_user_role.tWRITE
                                    If .transmit_to_server_and_receive(container.oAdmin.business_areas(i), True) = False Then
                                        Exit Sub
                                    End If
                                End If
                            End With
                            Exit For
                        End If
                    Next

                Case 3

                    For i = 0 To container.oAdmin.languages.Count - 1
                        If Trim(UCase(container.oAdmin.languages(i).language_name)) = Trim(UCase(view.tusers.SelectedNode.Text)) Then
                            With container.oAdmin.languages(i)
                                .write_mode = bc_om_user_language.UPDATE
                                .language_name = Trim(tx)
                                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                                    .db_write()
                                Else
                                    .tmode = bc_om_user_role.tWRITE
                                    If .transmit_to_server_and_receive(container.oAdmin.languages(i), True) = False Then
                                        Exit Sub
                                    End If
                                End If
                            End With
                            Exit For
                        End If
                    Next
                    'Case 10
                    '    Dim orole As New bc_om_user_role

                    '    For i = 0 To container.oadmin.roles.Count - 1
                    '        If container.oadmin.roles(i).description = view.tusers.SelectedNode.Parent.Text Then

                    '            orole.id = container.oadmin.roles(i).id
                    '            orole.level = container.oadmin.roles(i).level
                    '            orole.description = container.oadmin.roles(i).description
                    '            For j = 0 To container.oadmin.roles(i).apps.count - 1
                    '                orole.apps.Add(container.oadmin.roles(i).apps(j))
                    '            Next
                    '            REM now add application
                    '            orole.write_mode = bc_om_user_role.UPDATE
                    '            If Me.cboaccess.Text = "Taxonomy" Then
                    '                orole.apps.Add(1)
                    '            ElseIf Me.cboaccess.Text = "Users" Then
                    '                orole.apps.Add(2)
                    '            ElseIf Me.cboaccess.Text = "Publication Types" Then
                    '                orole.apps.Add(3)
                    '            ElseIf Me.cboaccess.Text = "Process" Then
                    '                orole.apps.Add(4)
                    '            Else
                    '                Exit Sub
                    '            End If

                    '            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    '                orole.db_write()
                    '            Else
                    '                orole.tmode = bc_om_user_role.tWRITE
                    '                If orole.transmit_to_server_and_receive(orole, True) = False Then
                    '                    Exit Sub
                    '                End If
                    '            End If

                    '            container.oadmin.roles(i).apps.add(orole.apps(orole.apps.Count - 1))

                    '            Exit For
                    '        End If
                    '    Next
            End Select
            Dim node_id As Integer
            node_id = view.tusers.SelectedNode.Index

            Dim role As String
            role = view.tusers.SelectedNode.Parent.Text

            load_users_tree()
            load_user_attributes()
            If Me.maintain_mode <> 10 Then
                view.tusers.Nodes(Me.maintain_mode + 1).Expand()
            Else
                view.tusers.Nodes(1).Expand()
                view.tusers.Nodes(1).Nodes(node_id).Expand()
                view.tusers.Nodes(1).Nodes(node_id).Nodes(1).Expand()
            End If


            For i = 0 To view.tusers.Nodes(1).Nodes.Count - 1
                If view.tusers.Nodes(1).Nodes(i).Text = tx Then
                    view.tusers.SelectedNode = view.tusers.Nodes(1).Nodes(i)
                    Exit For
                End If
            Next
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("OkToolStripMenuItem2", "Click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
        End Try
    End Sub
    Public Sub delete_item()
        Try
            Dim tx As String
            tx = set_maintain_mode()
            Dim omsg As bc_cs_message
            Dim item As String = ""
            Dim ntx As String
            ntx = view.tusers.SelectedNode.Text
            If InStr(ntx, "(inactive)") > 0 Then
                ntx = ntx.Substring(0, ntx.Length - 11)
            End If

            REM only allow if no users assigned
            Select Case Me.maintain_mode
                Case 0
                    item = "Role"
                    For j = 0 To container.oUsers.user.Count - 1
                        If container.oUsers.user(j).role = ntx Then
                            omsg = New bc_cs_message("Blue Curve", "Role " + ntx + " has inactive users assigned cannot delete.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                            Exit Sub
                        End If
                    Next
                Case 1
                    item = "Office"
                    For j = 0 To container.oUsers.user.Count - 1
                        For k = 0 To container.oAdmin.offices.Count - 1
                            If Trim(container.oAdmin.offices(k).description) = Trim(ntx) Then
                                If container.oUsers.user(j).office_id = container.oAdmin.offices(k).id Then
                                    omsg = New bc_cs_message("Blue Curve", "Office " + Trim(ntx) + " has inactive users assigned cannot delete.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                                    Exit Sub
                                End If
                            End If
                        Next
                    Next

                Case 2
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


                Case 3
                    item = "Language"
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


                Case 11
                    item = "Application admin"
                    'If Me.logged_on_role = True Then
                    '    Exit Sub
                    'End If
            End Select
            If Me.maintain_mode <> 11 Then
                omsg = New bc_cs_message("Blue Curve", "Are you sure you wish to delete " + item + ": " + Trim(ntx), bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
            Else
                omsg = New bc_cs_message("Blue Curve", "Are you sure you wish to remove " + item + ": " + view.tusers.SelectedNode.Text + " from role", bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
            End If
            If omsg.cancel_selected = True Then
                Exit Sub
            End If

            Select Case Me.maintain_mode
                Case 0
                    For i = 0 To container.oAdmin.roles.Count - 1
                        If container.oAdmin.roles(i).description = ntx Then
                            Dim orole As New bc_om_user_role
                            orole.id = container.oAdmin.roles(i).id
                            orole.write_mode = bc_om_user_role.DELETE
                            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                                orole.db_write()
                            Else
                                orole.tmode = bc_om_user_role.tWRITE
                                If orole.transmit_to_server_and_receive(orole, True) = False Then
                                    Exit Sub
                                End If
                            End If
                            If orole.delete_error <> "" Then
                                'omsg = New bc_cs_message("Blue Curve", item + " " + Me.ttchitem.Text + " deletion failed database error: " + orole.delete_error, bc_cs_message.MESSAGE,false,false,"Yes","No",true)
                                Exit Sub
                            End If
                            item = item + " " + ntx
                            container.oAdmin.roles.RemoveAt(i)
                            Exit For
                        End If
                    Next
                Case 1
                    For i = 0 To container.oAdmin.offices.Count - 1
                        If Trim(container.oAdmin.offices(i).description) = Trim(ntx) Then
                            Dim office As New bc_om_user_office
                            office.id = container.oAdmin.offices(i).id
                            office.write_mode = bc_om_user_office.DELETE
                            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                                office.db_write()
                            Else
                                office.tmode = bc_om_user_role.tWRITE
                                If office.transmit_to_server_and_receive(office, True) = False Then
                                    Exit Sub
                                End If
                            End If
                            If office.delete_error <> "" Then
                                'omsg = New bc_cs_message("Blue Curve", item + " " + Trim(Me.ttchitem.Text) + " deletion failed database error: " + office.delete_error, bc_cs_message.MESSAGE,false,false,"Yes","No",true)
                                Exit Sub
                            End If
                            item = item + " " + ntx
                            container.oAdmin.offices.RemoveAt(i)
                            Exit For
                        End If
                    Next
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
                                'omsg = New bc_cs_message("Blue Curve", item + " " + Me.ttchitem.Text + " deletion failed database error: " + business_areas.delete_error, bc_cs_message.MESSAGE,false,false,"Yes","No",true)
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
                                'omsg = New bc_cs_message("Blue Curve", item + " " + Trim(Me.ttchitem.Text) + " deletion failed database error: " + language.delete_error, bc_cs_message.MESSAGE,false,false,"Yes","No",true)
                                Exit Sub
                            End If
                            item = item + " " + ntx
                            container.oAdmin.languages.RemoveAt(i)
                            Exit For
                        End If
                    Next
                Case 11
                    For i = 0 To container.oAdmin.roles.Count - 1
                        If container.oAdmin.roles(i).description = view.tusers.SelectedNode.Parent.Parent.Text Then
                            REM remove application
                            Dim appid As Integer
                            If view.tusers.SelectedNode.Text = "Taxonomy" Then
                                appid = 1
                            ElseIf view.tusers.SelectedNode.Text = "Users" Then
                                appid = 2
                            ElseIf view.tusers.SelectedNode.Text = "Publication Types" Then
                                appid = 3
                            ElseIf view.tusers.SelectedNode.Text = "Process" Then
                                appid = 4
                            Else
                                Exit Sub
                            End If
                            For j = 0 To container.oAdmin.roles(i).apps.count - 1
                                If container.oAdmin.roles(i).apps(j) = appid Then
                                    container.oAdmin.roles(i).apps.removeat(j)
                                    Exit For
                                End If
                            Next
                            container.oAdmin.roles(i).write_mode = bc_om_user_role.UPDATE
                            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                                container.oAdmin.roles(i).db_write()
                            Else
                                container.oAdmin.roles(i).tmode = bc_om_user_role.tWRITE
                                If container.oAdmin.roles(i).transmit_to_server_and_receive(container.oAdmin.roles(i), True) = False Then
                                    Exit Sub
                                End If
                            End If

                            item = item + " " + view.tusers.SelectedNode.Text

                            Exit For
                        End If
                    Next
            End Select
            load_users_tree()
            load_user_attributes()
            Dim node_id As String
            node_id = view.tusers.SelectedNode.Index
            If Me.maintain_mode <> 11 Then
                view.tusers.Nodes(Me.maintain_mode + 1).Expand()
            Else
                view.tusers.Nodes(1).Expand()
                view.tusers.Nodes(1).Nodes(node_id).Expand()
                view.tusers.Nodes(1).Nodes(node_id).Nodes(1).Expand()
            End If
            omsg = New bc_cs_message("Blue Curve", Trim(item) + " successfully deleted.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("DeleteToolStripMenuItem", "Click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
        End Try


    End Sub
    Public Sub assign_access_menu()
        REM flag up own role
        If is_logged_on_user_role(view.tusers.SelectedNode.Parent.Text) Then
            Dim omsg As New bc_cs_message("Blue Curve", "You are currently role: " + view.tusers.SelectedNode.Parent.Text + " changing access will take affect next time you logon are you sure you want to continue?", bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
            If omsg.cancel_selected = True Then
                Exit Sub
            End If
        End If
        Dim ofrm As New bc_am_cp_edit
        ofrm.ltitle.Text = "Change Access Level of Role: " + view.tusers.SelectedNode.Parent.Text
        ofrm.centry.Visible = True
        ofrm.centry.Sorted = False
        ofrm.Tentry.Visible = False
        ofrm.centry.Items.Add("View Only")
        ofrm.centry.Items.Add("Edit Lite")
        ofrm.centry.Items.Add("Advanced Edit")
        ofrm.centry.Text = Right(view.tusers.SelectedNode.Text, view.tusers.SelectedNode.Text.Length - 14)
        ofrm.ShowDialog()
        If ofrm.cancel_selected = True Then
            Exit Sub
        End If
        For i = 0 To container.oAdmin.roles.Count - 1
            If Trim(UCase(container.oAdmin.roles(i).description)) = Trim(UCase(view.tusers.SelectedNode.Parent.Text)) Then
                With container.oAdmin.roles(i)
                    .write_mode = bc_om_user_role.UPDATE
                    .level = ofrm.centry.SelectedIndex
                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        .db_write()
                    Else
                        .tmode = bc_om_user_role.tWRITE
                        If .transmit_to_server_and_receive(container.oAdmin.roles(i), True) = False Then
                            Exit Sub
                        End If
                    End If


                End With
                Exit For
            End If
        Next
        Dim role As String
        role = view.tusers.SelectedNode.Parent.Text

        load_users_tree()
        load_user_attributes()

        For i = 0 To view.tusers.Nodes(1).Nodes.Count - 1
            If view.tusers.Nodes(1).Nodes(i).Text = role Then
                view.tusers.SelectedNode = view.tusers.Nodes(1).Nodes(i)
                view.tusers.SelectedNode.Expand()
                Exit For
            End If
        Next

    End Sub
    Public Sub remove_app()
        If is_logged_on_user_role(view.tusers.SelectedNode.Parent.Parent.Text) Then
            Dim omsg As New bc_cs_message("Blue Curve", "You are currently role: " + view.tusers.SelectedNode.Parent.Parent.Text + " removing applications will take affect next time you logon are you sure you want to continue?", bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
            If omsg.cancel_selected = True Then
                Exit Sub
            End If
        End If
        For i = 0 To container.oAdmin.roles.Count - 1
            If Trim(UCase(container.oAdmin.roles(i).description)) = Trim(UCase(view.tusers.SelectedNode.Parent.Parent.Text)) Then
                With container.oAdmin.roles(i)
                    .write_mode = bc_om_user_role.UPDATE
                    Dim app As String
                    app = view.tusers.SelectedNode.Text
                    For j = 0 To .apps.count - 1
                        If app = "Taxonomy" Then
                            If .apps(j) = 1 Then
                                .apps.removeat(j)
                                Exit For
                            End If
                        ElseIf app = "Users" Then
                            If .apps(j) = 2 Then
                                .apps.removeat(j)
                                Exit For
                            End If
                        ElseIf app = "Publication Types" Then
                            If .apps(j) = 3 Then
                                .apps.removeat(j)
                                Exit For
                            End If
                        ElseIf app = "Translation" Then
                            If .apps(j) = 4 Then
                                .apps.removeat(j)
                                Exit For
                            End If
                        End If
                    Next
                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        .db_write()
                    Else
                        .tmode = bc_om_user_role.tWRITE
                        If .transmit_to_server_and_receive(container.oAdmin.roles(i), True) = False Then
                            Exit Sub
                        End If
                    End If


                End With
                Exit For
            End If
        Next
        Dim role As String
        role = view.tusers.SelectedNode.Parent.Parent.Text
        load_users_tree()
        load_user_attributes()

        For i = 0 To view.tusers.Nodes(1).Nodes.Count - 1
            If view.tusers.Nodes(1).Nodes(i).Text = role Then
                view.tusers.SelectedNode = view.tusers.Nodes(1).Nodes(i)
                view.tusers.SelectedNode = view.tusers.Nodes(1).Nodes(i).Nodes(1)
                view.tusers.SelectedNode.Expand()
                Exit For
            End If
        Next
    End Sub
    Public Sub add_stage_role_access()
        Try
            Dim stage As String
            Dim role As String
            Dim access As String
            access = view.tusers.SelectedNode.Text
            access = UCase(access.Substring(0, 1))
            role = view.tusers.SelectedNode.Parent.Parent.Parent.Text
            stage = view.tusers.SelectedNode.Parent.Text
            stage = Trim(stage.Substring(0, InStr(stage, "[") - 1))
            Dim sra As New bc_om_stage_role_access
            For i = 0 To Me.container.oAdmin.roles.Count - 1
                If Me.container.oAdmin.roles(i).description = role Then
                    sra.role_id = Me.container.oAdmin.roles(i).id
                    sra.stage_name = stage
                    sra.access_id = access
                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        sra.del_flag = False
                        sra.db_write()
                    Else
                        sra.tmode = bc_cs_soap_base_class.tWRITE
                        If sra.transmit_to_server_and_receive(sra, True) = False Then
                            Exit Sub
                        End If

                    End If
                    REM update memory object


                    Me.container.oAdmin.roles(i).stage_role_access.add(sra)

                    Me.view.tusers.SelectedNode.ImageIndex = 20
                    set_access_title()
                    Exit For
                End If
            Next
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_users", "add_stage_role_access", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try
    End Sub
    Public Sub set_access_title()
        Dim stage As String
        Me.view.tusers.SelectedNode = Me.view.tusers.SelectedNode.Parent

        stage = Me.view.tusers.SelectedNode.Text
        stage = Trim(stage.Substring(0, InStr(stage, "[") - 1))
        stage = stage + " ["

        If Me.view.tusers.SelectedNode.Nodes(0).ImageIndex = 20 Then
            stage = stage + "F"
        End If
        If Me.view.tusers.SelectedNode.Nodes(1).ImageIndex = 20 Then
            stage = stage + "M"
        End If
        If Me.view.tusers.SelectedNode.Nodes(2).ImageIndex = 20 Then
            stage = stage + "R"
        End If
        If Me.view.tusers.SelectedNode.Nodes(3).ImageIndex = 20 Then
            stage = stage + "V"
        End If
        If Me.view.tusers.SelectedNode.Nodes(4).ImageIndex = 20 Then
            stage = stage + "W"
        End If
        If Me.view.tusers.SelectedNode.Nodes(5).ImageIndex = 20 Then
            stage = stage + "D"
        End If
        stage = stage + "]"
        Me.view.tusers.SelectedNode.Text = stage

    End Sub
    Public Sub rem_stage_role_access()
        Try
            Dim stage As String
            Dim role As String
            Dim access As String
            access = view.tusers.SelectedNode.Text
            access = UCase(access.Substring(0, 1))
            role = view.tusers.SelectedNode.Parent.Parent.Parent.Text
            stage = view.tusers.SelectedNode.Parent.Text
            stage = Trim(stage.Substring(0, InStr(stage, "[") - 1))
            Dim sra As New bc_om_stage_role_access
            For i = 0 To Me.container.oAdmin.roles.Count - 1
                If Me.container.oAdmin.roles(i).description = role Then
                    sra.role_id = Me.container.oAdmin.roles(i).id
                    sra.stage_name = stage
                    sra.access_id = access
                    sra.del_flag = True
                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        sra.db_write()
                    Else
                        sra.tmode = bc_cs_soap_base_class.tWRITE
                        If sra.transmit_to_server_and_receive(sra, True) = False Then
                            Exit Sub
                        End If
                    End If
                    REM update memory object
                    For j = 0 To Me.container.oAdmin.roles(i).stage_role_access.count - 1
                        With Me.container.oAdmin.roles(i).stage_role_access(j)
                            If Trim(.access_id) = sra.access_id And .stage_name = sra.stage_name Then
                                Me.container.oAdmin.roles(i).stage_role_access.removeat(j)
                                Exit For
                            End If
                        End With
                    Next

                    Me.view.tusers.SelectedNode.ImageIndex = 6
                    set_access_title()
                    Exit For
                End If
            Next
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_users", "rem_stage_role_access", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try
    End Sub
    Public Sub add_app()
        For i = 0 To container.oAdmin.roles.Count - 1
            If Trim(UCase(container.oAdmin.roles(i).description)) = Trim(UCase(view.tusers.SelectedNode.Parent.Parent.Text)) Then
                With container.oAdmin.roles(i)
                    .write_mode = bc_om_user_role.UPDATE
                    Dim app As String
                    app = view.tusers.SelectedNode.Text
                    If app = "Taxonomy" Then
                        .apps.add(1)
                    ElseIf app = "Users" Then
                        .apps.add(2)
                    ElseIf app = "Publication Types" Then
                        .apps.add(3)
                    ElseIf app = "Translation" Then
                        .apps.add(4)
                    End If

                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        .db_write()
                    Else
                        .tmode = bc_om_user_role.tWRITE
                        If .transmit_to_server_and_receive(container.oAdmin.roles(i), True) = False Then
                            Exit Sub
                        End If
                    End If


                End With
                Exit For
            End If
        Next
        Dim role As String
        role = view.tusers.SelectedNode.Parent.Parent.Text
        load_users_tree()
        load_user_attributes()

        For i = 0 To view.tusers.Nodes(1).Nodes.Count - 1
            If view.tusers.Nodes(1).Nodes(i).Text = role Then
                view.tusers.SelectedNode = view.tusers.Nodes(1).Nodes(i)
                view.tusers.SelectedNode = view.tusers.Nodes(1).Nodes(i).Nodes(1)
                view.tusers.SelectedNode.Expand()

                Exit For
            End If
        Next
    End Sub
    Private Function is_logged_on_user_role(ByVal role As String) As Boolean
        is_logged_on_user_role = False
        For i = 0 To container.oUsers.user.Count - 1
            If container.oUsers.user(i).id = bc_cs_central_settings.logged_on_user_id Then
                If role = container.oUsers.user(i).role Then
                    is_logged_on_user_role = True
                End If
                Exit For
            End If
        Next
    End Function
    Public Sub increase_rating(ByVal inc As Boolean)
        Dim oco As Integer = 0
        Dim entity_id As Long
        Dim soid As String = ""
        Dim slid As String = ""
        Dim lco As Integer = 0
        Dim co As Integer = 0
        Dim rating As Integer

        Dim pti As Long

        pti = view.tentities.SelectedNode.Parent.Parent.Parent.Tag
        entity_id = view.tentities.SelectedNode.Parent.Tag

        REM now change user prefs rating
        For j = 0 To current_user.prefs.Count - 1
            If current_user.prefs(j).entity_id = entity_id And current_user.prefs(j).pref_type = pti Then
                If inc = True Then
                    For i = 0 To current_user.prefs(j).users.Count - 1
                        If current_user.prefs(j).users(i).id = current_user.id Then
                            rating = current_user.prefs(j).users(i).rating
                            current_user.prefs(j).users(i).rating = current_user.prefs(j).users(i - 1).rating
                            current_user.prefs(j).users(i - 1).rating = rating
                            current_user.prefs(j).users(i - 1).rating_changed = True
                            Exit For
                        End If
                    Next
                    current_user.prefs(j).rating = current_user.prefs(j).rating - 1
                Else
                    For i = 0 To current_user.prefs(j).users.Count - 1
                        If current_user.prefs(j).users(i).id = current_user.id Then
                            rating = current_user.prefs(j).users(i).rating
                            current_user.prefs(j).users(i).rating = current_user.prefs(j).users(i + 1).rating
                            current_user.prefs(j).users(i + 1).rating = rating
                            current_user.prefs(j).users(i + 1).rating_changed = True
                            Exit For
                        End If
                    Next
                    current_user.prefs(j).rating = current_user.prefs(j).rating + 1
                End If
                Exit For
            End If
        Next
        Dim nc, ne, nf As Integer
        nf = view.tentities.SelectedNode.Parent.Parent.Parent.Index
        nc = view.tentities.SelectedNode.Parent.Parent.Index
        ne = view.tentities.SelectedNode.Parent.Index
        load_node_tree()

        view.tentities.CollapseAll()
        view.tentities.Nodes(nf).Expand()
        view.tentities.Nodes(nf).Nodes(nc).Nodes(ne).Expand()
        view.tentities.SelectedNode = view.tentities.Nodes(nf).Nodes(nc).Nodes(ne)



        set_change()
    End Sub
    REM FIL July 2012
    'Private Sub write_pref_list()
    '    Dim wpl As New bc_om_all_prefs
    '    Dim entity_id As Long = 0
    '    current_user.set_user_prefs.delete_entity_pref_list.Clear()
    '    current_user.set_user_prefs.prefs.Clear()
    '    REM copy over prefs for entities used by current user from global list
    '    For m = 0 To Me.container.oUsers.pref_types.Count - 1
    '        entity_id = 0
    '        For i = 0 To container.oAdmin.user_prefs.Count - 1
    '            If Me.container.oUsers.pref_types(m).id = container.oAdmin.user_prefs(i).pref_type Then
    '                For j = 0 To current_user.prefs.Count - 1
    '                    If Me.container.oUsers.pref_types(m).id = current_user.prefs(j).pref_type Then
    '                        If container.oAdmin.user_prefs(i).entity_id = current_user.prefs(j).entity_id Then
    '                            If entity_id <> current_user.prefs(j).entity_id Then
    '                                current_user.set_user_prefs.delete_entity_pref_list.Add(current_user.prefs(j))
    '                            End If
    '                            current_user.set_user_prefs.prefs.Add(container.oAdmin.user_prefs(i))
    '                            entity_id = current_user.prefs(j).entity_id
    '                        End If
    '                    End If
    '                Next
    '            End If
    '        Next
    '    Next
    'End Sub
    REM ING JUNE 2012
    Public Sub reset_user_list()
        Try
            view.tusers.SelectedNode = view.tusers.Nodes(0)
        Catch

        End Try
    End Sub
    Public Function set_data_type_Control() As Boolean
        Try
            Dim length As Integer
            Dim disable_col As Boolean = False
            If setting_text_box = True Then
                Exit Function

            End If
            Dim lus As New ArrayList
            set_data_type_Control = False
            If no_action = True Then
                Exit Function
            End If


            If view.DataGridView1.ColumnCount <> 6 Then
                Exit Function
            End If
            REM temporary hold current values
            hold_vals.Clear()
            For i = 0 To view.DataGridView1.Rows.Count - 1
                hold_vals.Add(view.DataGridView1.Item(3, i).Value)
                If IsNothing(hold_vals(i)) Then
                    hold_vals(i) = ""
                End If
            Next
            Dim lu As DataGridViewComboBoxCell

            view.DataGridView1.Columns(3).ReadOnly = False



            Dim use_combo As Boolean = False
            Dim dtextbox As New DataGridViewTextBoxColumn
            dtextbox.SortMode = DataGridViewColumnSortMode.NotSortable

            Dim Dcombobox As New DataGridViewComboBoxColumn
            Dim row_id As Integer
            row_id = view.DataGridView1.SelectedCells(0).RowIndex

            Dim j As Integer
            REM 1 string 2 number 3 boolean 5 date

            'view.DataGridView1.Item(1, row_id).Selected = True
            Application.DoEvents()

            REM build up array of all lookup values
            Dim found As Boolean = False
            For i = 0 To view.DataGridView1.Rows.Count - 1
                found = False
                REM user table
                For j = 0 To container.oUsers.user_system_attributes.Count - 1
                    If container.oUsers.user_system_attributes(j).display_name = view.DataGridView1.Item(1, i).Value Then
                        found = True
                        If container.oUsers.user_system_attributes(j).lookup_vals.Count > 0 Then

                            lu = New DataGridViewComboBoxCell
                            lu.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing
                            If i = row_id Then
                                For k = 0 To container.oUsers.user_system_attributes(j).lookup_vals.Count - 1
                                    If container.oUsers.user_system_attributes(j).lookup_vals_inactive(k) = False Then
                                        lu.Items.Add(container.oUsers.user_system_attributes(j).lookup_vals(k))
                                    End If
                                Next
                            End If
                            lu.Items.Add("")
                            lus.Add(lu)



                        ElseIf container.oUsers.user_system_attributes(j).type_id = 3 Then
                            lu = New DataGridViewComboBoxCell
                            lu.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing

                            lu.Items.Add("")
                            lu.Items.Add("True")
                            lu.Items.Add("False")
                            If hold_vals(i) <> "True" And hold_vals(i) <> "False" Then
                                hold_vals(i) = ""
                            End If
                            lus.Add(lu)
                        Else
                            lu = New DataGridViewComboBoxCell
                            lu.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing

                            If IsNothing(hold_vals(i)) Then
                                lu.Items.Add("")
                            Else
                                lu.Items.Add(hold_vals(i))
                            End If
                            lus.Add(lu)

                        End If
                        Exit For
                    End If
                Next
                REM attributes
                If found = True Then
                    Continue For
                End If

                For j = 0 To container.oClasses.attribute_pool.Count - 1
                    If container.oClasses.attribute_pool(j).name = view.DataGridView1.Item(1, i).Value Then
                        If container.oClasses.attribute_pool(j).is_lookup = True Then


                            lu = New DataGridViewComboBoxCell
                            lu.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing
                            'If i = row_id Then
                            lu.Items.Add("")
                            For k = 0 To container.oClasses.attribute_pool(j).lookup_values.count - 1
                                lu.Items.Add(container.oClasses.attribute_pool(j).lookup_values(k))
                            Next
                            'End If
                            lus.Add(lu)


                        ElseIf container.oClasses.attribute_pool(j).type_id = 3 Then
                            lu = New DataGridViewComboBoxCell
                            lu.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing

                            lu.Items.Add("")
                            lu.Items.Add("True")
                            lu.Items.Add("False")
                            If hold_vals(i) <> "True" And hold_vals(i) <> "False" Then
                                hold_vals(i) = ""
                            End If
                            lus.Add(lu)
                        Else
                            lu = New DataGridViewComboBoxCell
                            lu.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing

                            If IsNothing(hold_vals(i)) Then
                                lu.Items.Add("")
                            Else
                                lu.Items.Add(hold_vals(i))
                            End If
                            lus.Add(lu)

                        End If


                      

                        Exit For
                    End If
                Next
            Next

            Dim beditbox As Boolean = False

            For i = 0 To container.oUsers.user_system_attributes.Count - 1
                If container.oUsers.user_system_attributes(i).display_name = view.DataGridView1.Item(1, view.DataGridView1.SelectedCells(0).RowIndex).Value Then
                    length = container.oUsers.user_system_attributes(i).length

                    If container.oUsers.user_system_attributes(i).lookup_vals.Count > 0 Then
                        use_combo = True
                    ElseIf setting_text_box = False And container.oUsers.user_system_attributes(i).type_id = 1 And container.oUsers.user_system_attributes(i).popup = True Then
                        Dim fedit As New bc_am_text_edit
                        beditbox = True
                        fedit.Text = "Edit: " + container.oUsers.user_system_attributes(i).display_name
                        fedit.maxlength = container.oUsers.user_system_attributes(i).length
                        fedit.ttextentry.MaxLength = container.oUsers.user_system_attributes(i).length
                        fedit.ttextentry.Text = view.DataGridView1.Item(3, view.DataGridView1.SelectedCells(0).RowIndex).Value
                        fedit.Llength.Text = "Maximum length: " + CStr(container.oUsers.user_system_attributes(i).length)
                        setting_text_box = True
                        'fedit.Initializing = False

                        fedit.ShowDialog()
                        If fedit.ok_selected = True Then
                            setting_text_box = True
                            validate_attribute_value(fedit.ttextentry.Text, True)
                        End If
                        view.DataGridView1.Item(0, row_id).Selected = True
                        setting_text_box = False
                    ElseIf container.oUsers.user_system_attributes(i).type_id = 3 Then
                        use_combo = True
                    End If
                    Exit For
                End If
            Next


            For i = 0 To container.oClasses.attribute_pool.Count - 1
                If container.oClasses.attribute_pool(i).name = view.DataGridView1.Item(1, view.DataGridView1.SelectedCells(0).RowIndex).Value Then
                    length = container.oClasses.attribute_pool(i).length
                    If container.oClasses.attribute_pool(i).is_def = True Then
                        disable_col = True
                    End If
                    If container.oClasses.attribute_pool(i).is_lookup = True Then
                        use_combo = True
                    ElseIf setting_text_box = False And container.oClasses.attribute_pool(i).type_id = 1 And container.oClasses.attribute_pool(i).repeats = 1 Then
                        Dim fedit As New bc_am_text_edit
                        beditbox = True
                        fedit.Text = "Edit: " + container.oClasses.attribute_pool(i).name
                        fedit.maxlength = container.oClasses.attribute_pool(i).length
                        fedit.ttextentry.MaxLength = container.oClasses.attribute_pool(i).length
                        fedit.ttextentry.Text = view.DataGridView1.Item(3, view.DataGridView1.SelectedCells(0).RowIndex).Value
                        fedit.Llength.Text = "Maximum length: " + CStr(container.oClasses.attribute_pool(i).length)
                        setting_text_box = True
                        'fedit.Initializing = False
                        fedit.ShowDialog()
                        If fedit.ok_selected = True Then
                            setting_text_box = True
                            validate_attribute_value(fedit.ttextentry.Text, True)
                        End If
                        view.DataGridView1.Item(0, row_id).Selected = True
                        setting_text_box = False
                    ElseIf container.oClasses.attribute_pool(i).type_id = 3 Then
                        use_combo = True
                    End If
                    Exit For
                End If
            Next
            REM 
            Dcombobox.Width = view.DataGridView1.Columns(3).Width

            dtextbox.Width = view.DataGridView1.Columns(3).Width

            If beditbox = False Then
                view.DataGridView1.Columns.RemoveAt(3)
                If use_combo = True Then
                    Dcombobox.Name = "Values"
                    view.DataGridView1.Columns.Insert(3, Dcombobox)
                Else
                    dtextbox.Name = "Values"
                    dtextbox.MaxInputLength = length
                    view.DataGridView1.Columns.Insert(3, dtextbox)
                End If
                REM reset value
                no_action = True
                For i = 0 To view.DataGridView1.Rows.Count - 1
                    If use_combo = True Then
                        view.DataGridView1.Item(3, i) = lus(i)
                    End If
                    If hold_vals(i) <> "" Then
                        view.DataGridView1.Item(3, i).Value = hold_vals(i)
                    End If
                Next
                view.DataGridView1.Item(3, row_id).Selected = True
            End If
            If disable_col = True Then
                view.DataGridView1.Columns(3).ReadOnly = True
            End If
            no_action = False
            set_data_type_Control = True
        Catch ex As Exception
            Dim ocomm As New bc_cs_activity_log("bc_am_taxonomy", "set_data_type_comtrol", bc_cs_activity_codes.COMMENTARY, "Error: " + ex.Message)

        End Try

    End Function

    Public Function validate_attribute_value(ByVal val As String, Optional ByVal tb As Boolean = False) As Boolean
        no_action = True
        Try
            validate_attribute_value = True
            'If Trim(val) = "" Then
            '    Exit Function
            'End If
            Try
                If view.DataGridView1.SelectedCells(0).ColumnIndex <> 3 And tb = False Then
                    Exit Function
                End If
            Catch
                Exit Function
            End Try
            set_val(val, tb)

        Catch ex As Exception
            'MsgBox(ex.Message)
        Finally
            no_action = False
        End Try
    End Function
    REM ING JUNE 2012
    Public gext As Boolean = False
    Public Sub set_val(ByVal val As String, Optional ByVal ext As Boolean = False)
        Try
            If gext = True Then
                gext = False
                Exit Sub
            End If
            gext = ext
            If val <> user_attributes(view.DataGridView1.SelectedCells(0).RowIndex).value Then
                REM see if item is a key value pair

                For i = 0 To container.oUsers.user_system_attributes.Count - 1
                    If container.oUsers.user_system_attributes(i).display_name = view.DataGridView1.Item(1, view.DataGridView1.SelectedCells(0).RowIndex).Value Then
                        If container.oUsers.user_system_attributes(i).lookup_vals_key.Count > 0 Then
                            REM find key value for value
                            For j = 0 To container.oUsers.user_system_attributes(i).lookup_vals.Count - 1
                                If RTrim(LTrim(container.oUsers.user_system_attributes(i).lookup_vals(j))) = RTrim(LTrim(val)) Then
                                    val = container.oUsers.user_system_attributes(i).lookup_vals_key(j)
                                End If
                            Next
                        End If
                        Exit For
                    End If
                Next

                For i = 0 To container.oClasses.attribute_pool.Count - 1
                    If container.oClasses.attribute_pool(i).name = view.DataGridView1.Item(1, view.DataGridView1.SelectedCells(0).RowIndex).Value Then
                        If container.oClasses.attribute_pool(i).lookup_keys.count > 0 Then
                            REM find key value for value
                            For j = 0 To container.oClasses.attribute_pool(i).lookup_values.count - 1
                                If RTrim(LTrim(container.oClasses.attribute_pool(i).lookup_values(j))) = RTrim(LTrim(val)) Then
                                    val = container.oClasses.attribute_pool(i).lookup_keys(j)
                                End If
                            Next
                        End If
                        Exit For
                    End If

                Next


                user_attributes(view.DataGridView1.SelectedCells(0).RowIndex).value = val
                user_attributes(view.DataGridView1.SelectedCells(0).RowIndex).value_changed = True
                view.DataGridView1.Item(0, view.DataGridView1.SelectedCells(0).RowIndex).Value = view.tabimages.Images(att_edited_icon)
                If ext = True Then
                    gext = True
                    view.DataGridView1.Item(3, view.DataGridView1.SelectedCells(0).RowIndex).Value = val
                Else
                    gext = False
                End If


                Me.set_change()


            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_taxonomy", "set_val", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Public Sub attribute_details(all As Boolean)
        Try
            If view.DataGridView1.SelectedCells.Count = 0 Then
                Exit Sub

            End If
            attribute_history(all)
            Exit Sub
            view.mdettitle.Text = "Details for: " + view.DataGridView1.Item(1, view.DataGridView1.SelectedCells(0).RowIndex).Value

            view.mdetupdate.Text = "Last Updated: " + Me.user_attributes(view.DataGridView1.SelectedCells(0).RowIndex).last_update_comment
            view.mdetuser.Text = "By: " + Me.user_attributes(view.DataGridView1.SelectedCells(0).RowIndex).last_update_user
            view.mdetpubupdate.Visible = False
            view.mdetpubuser.Visible = False
            view.attributedetails.Show(Cursor.Position.X, Cursor.Position.Y)
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("DataGridView1", "DoubleClick", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            view.Cursor = Cursors.Default
        End Try
    End Sub
    Public Sub attribute_history(all As Boolean)
        Try
            If view.DataGridView1.SelectedCells.Count = 0 Then
                Exit Sub

            End If
            Dim fa As New bc_am_attribute_audit
            Dim ca As New Cbc_am_attribute_audit
            Dim aname, kname, aid As String
            Dim kid As Long
            Dim atype As bc_om_attribute_audit.EATTRIBUTE_TYPE
            aname = view.DataGridView1.Item(1, view.DataGridView1.SelectedCells(0).RowIndex).Value
            kid = current_user.id
            kname = current_user.first_name + " " + current_user.surname


            For i = 0 To Me.user_attributes.Count - 1
                If Me.user_attributes(i).name = aname Then
                    If Me.user_attributes(i).attribute_id = 0 Then
                        aid = Me.user_attributes(i).field_name
                        atype = bc_om_attribute_audit.EATTRIBUTE_TYPE.USER_TABLE
                    Else
                        aid = Me.user_attributes(i).attribute_id
                        atype = bc_om_attribute_audit.EATTRIBUTE_TYPE.USER_ATTRIBUTE
                    End If
                    Exit For
                End If
            Next

            If ca.load_data(fa, kname, aname, aid, kid, atype, False, all) = True Then
                fa.ShowDialog()
            ElseIf all = False Then
                view.mdettitle.Text = "Details for: " + view.DataGridView1.Item(1, view.DataGridView1.SelectedCells(0).RowIndex).Value
                Dim da As DateTime
                Dim lda As String
                Try
                    da = Me.user_attributes(view.DataGridView1.SelectedCells(0).RowIndex).last_update_comment
                    da = da.ToLocalTime
                    lda = "Last Updated: " + Format(da, "dd-MMM-yyyy HH:mm:ss")
                Catch
                    lda = "Last Updated: " + Me.user_attributes(view.DataGridView1.SelectedCells(0).RowIndex).last_update_comment
                End Try
                view.mdetupdate.Text = lda
                view.mdetuser.Text = "By: " + Me.user_attributes(view.DataGridView1.SelectedCells(0).RowIndex).last_update_user
                view.mdetpubupdate.Visible = False
                view.mdetpubuser.Visible = False
                view.attributedetails.Show(Cursor.Position.X, Cursor.Position.Y)
            End If
                Catch ex As Exception
                    Dim oerr As New bc_cs_error_log("DataGridView1", "DoubleClick", bc_cs_error_codes.USER_DEFINED, ex.Message)
                Finally
                    view.Cursor = Cursors.Default
                End Try
    End Sub
    Public Sub add_new(ByVal assign_form As bc_am_cp_assign)

        Try

            Dim fedit As New bc_am_cp_edit
            fedit.height = 518
            fedit.ltitle.Text = "Add New Attribute to User Display"
            fedit.Cworkflow.Enabled = False
            fedit.Caudit.Enabled = True
            fedit.Cdt.Items.Add("String")
            fedit.Cdt.Items.Add("Number")
            fedit.Cdt.Items.Add("Boolean")
            fedit.Cdt.Items.Add("Date")
            fedit.Cdt.SelectedIndex = 0
            fedit.Tentry.Visible = False
            fedit.centry.Visible = False
            fedit.uxDetails.Visible = True
            'fedit.Height = fedit.Height + fedit.uxDetails.Height
            fedit.ShowDialog()
            If fedit.cancel_selected = True Then
                Exit Sub
            Else
                view.Cursor = Cursors.WaitCursor
                If add_new_attribute(fedit.Tattname.Text, fedit.Cdt.SelectedIndex, fedit.Caudit.Checked, fedit.cmandatory.Checked, fedit.Cworkflow.Checked, fedit.clookup.Checked, fedit.Tsql.Text, fedit.Tlength.Text, fedit.cpopup.Checked, fedit.chkdef.Checked, fedit.defsql.Text) = True Then
                    REM now assign
                    assign_form.lselparent.Items.Add(fedit.Tattname.Text)
                    assign_form.Bok.Enabled = True
                End If
                view.Cursor = Cursors.Default
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_users", "add_new", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            view.Cursor = Cursors.Default
        End Try
    End Sub
    Private Function add_new_attribute(ByVal name As String, ByVal dt As Integer, ByVal audit As Boolean, ByVal mandatory As Boolean, ByVal workflow As Boolean, ByVal lookup As Boolean, ByVal sql As String, ByVal length As String, ByVal popup As Boolean, def As Boolean, def_sql As String) As Boolean
        Try
            add_new_attribute = False
            If name = "" Then
                Dim omsg As New bc_cs_message("Blue Curve", "attribtute name must be entered.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Function
            End If
            REM check attribute name is not already in use
            For i = 0 To Me.container.oClasses.attribute_pool.Count - 1
                If Trim(UCase(Me.container.oClasses.attribute_pool(i).name)) = Trim(UCase(name)) Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Attribute: " + name + " already exists please enter again!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Function
                End If
            Next
            REM check it is not a user table value as well

            For i = 0 To Me.container.oUsers.user_system_attributes.Count - 1
                If Trim(UCase(Me.container.oUsers.user_system_attributes(i).display_name)) = Trim(UCase(name)) Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Attribute: " + name + " already exists please enter again!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Function
                End If
            Next

            Dim oatt As New bc_om_attribute
            oatt.repeats = 0

            oatt.name = name
            Select Case dt
                Case 0
                    oatt.type_id = 1
                    If IsNumeric(length) = True Then
                        oatt.length = CInt(length)
                    Else
                        oatt.length = 255
                    End If
                    If popup = True Then
                        oatt.repeats = 1
                    End If
                Case 1
                    oatt.type_id = 2
                Case 2
                    oatt.type_id = 3
                Case 3
                    oatt.type_id = 5
            End Select
            If audit = True Then
                oatt.submission_code = 2
            Else
                oatt.submission_code = 1
            End If
            oatt.nullable = True
            If mandatory = True Then
                oatt.nullable = False
            End If
            oatt.show_workflow = False
            If workflow = True Then
                oatt.show_workflow = 1
            End If
            oatt.is_lookup = False
            If lookup = True Then
                oatt.is_lookup = True
                oatt.lookup_sql = sql
            End If
            oatt.def_sql = False
            If def = True Then
                oatt.is_def = def
                oatt.def_sql = def_sql
            End If

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                oatt.db_write()
            Else
                oatt.tmode = bc_cs_soap_base_class.tWRITE
                If oatt.transmit_to_server_and_receive(oatt, True) = False Then
                    Exit Function
                End If

            End If
            REM if lookup reload attributes to get lookup
            If oatt.is_lookup Then
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    Me.container.oClasses.db_read()
                Else
                    Me.container.oClasses.tmode = bc_cs_soap_base_class.tREAD
                    If Me.container.oClasses.transmit_to_server_and_receive(Me.container.oClasses, True) = False Then
                        Exit Function
                    End If
                End If
            Else
                REM now assign to pool if not read back
                Me.container.oClasses.attribute_pool.Add(oatt)
            End If

            add_new_attribute = True

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_users", "add_new_attribute", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

        End Try

    End Function

    Public Sub assign_display_attributes()
        REM view.Lusers.SelectedItems.Clear()
        Dim fassign As New bc_am_cp_assign
        fassign.ltitle.Text = "Assign Attributes to Display"
        fassign.bup.Visible = True
        fassign.bdn.Visible = True
        fassign.tlfilter.Visible = False
        fassign.Clist.Visible = False
        fassign.llfilter.Visible = False
        fassign.Tnew.Visible = False
        fassign.bnew.Visible = True
        fassign.bedit.Visible = True
        fassign.Label2.Visible = False
        fassign.fcontainer = Me

        fassign.lallparent.Height = fassign.lselparent.Height
        Dim dp As System.Drawing.Point
        dp.X = fassign.lallparent.Location.X
        dp.Y = fassign.lselparent.Location.Y
        fassign.lallparent.Location = dp

        fassign.Label1.Visible = False
        fassign.lallparent.Sorted = True

        Dim found As Boolean

        REM user table
        For j = 0 To Me.user_attributes.Count - 1
            fassign.lselparent.Items.Add(Me.user_attributes(j).name)
        Next

        For i = 0 To Me.container.oUsers.user_system_attributes.Count - 1
            found = False

            For j = 0 To Me.user_attributes.Count - 1
                If Me.container.oUsers.user_system_attributes(i).display_name = Me.user_attributes(j).name Then
                    found = True
                    Exit For
                End If
            Next
            If found = False Then
                fassign.lallparent.Items.Add(Me.container.oUsers.user_system_attributes(i).display_name)
            End If
        Next

        REM attribute table
        For i = 0 To Me.container.oClasses.attribute_pool.Count - 1
            found = False

            For j = 0 To Me.user_attributes.Count - 1
                If Me.container.oClasses.attribute_pool(i).name = Me.user_attributes(j).name Then
                    found = True
                    Exit For
                End If
            Next
            If found = False Then
                fassign.lallparent.Items.Add(Me.container.oClasses.attribute_pool(i).name)
            End If
        Next
        fassign.ShowDialog()

        If fassign.cancel_selected = True Then
            Exit Sub
        End If

        save_user_display_attributes(fassign)
    End Sub

    Private Sub save_user_display_attributes(ByVal fassign As bc_am_cp_assign)

        Me.container.oUsers.user_display_attributes.user_display_attributes.Clear()

        Dim nuaa As New bc_om_users.bc_om_user_display_attributes
        Dim nua As bc_om_users.bc_om_user_display_attribute
        For i = 0 To fassign.lselparent.Items.Count - 1


            nua = New bc_om_users.bc_om_user_display_attribute
            nua.name = fassign.lselparent.Items(i)
            Me.container.oUsers.user_display_attributes.user_display_attributes.Add(nua)

        Next

        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
            Me.container.oUsers.user_display_attributes.db_write()
            Me.container.oUsers.user_display_attributes.db_read()
        Else
            Me.container.oUsers.user_display_attributes.tmode = bc_cs_soap_base_class.tWRITE
            If Me.container.oUsers.user_display_attributes.transmit_to_server_and_receive(Me.container.oUsers.user_display_attributes, True) = False Then
                Exit Sub
            End If
            Me.container.oUsers.user_display_attributes.tmode = bc_cs_soap_base_class.tREAD
            If Me.container.oUsers.user_display_attributes.transmit_to_server_and_receive(Me.container.oUsers.user_display_attributes, True) = False Then
                Exit Sub
            End If

        End If

        REM view.Lusers.SelectedItems.Clear()
        view.Cursor = Cursors.WaitCursor
        load_user_attributes()
        If view.Lusers.SelectedItems.Count <> 0 Then
            load_user()
        End If
        view.Cursor = Cursors.Default

    End Sub
    Public Sub modify_attribute_menu(ByVal assign_form As bc_am_cp_assign)
        Try
            REM check attribute is an attribute
            Dim found As Boolean = False
            Dim attribute_id As Long
            Dim oname As String
            For i = 0 To Me.container.oClasses.attribute_pool.Count - 1
                If Me.container.oClasses.attribute_pool(i).name = assign_form.lselparent.Text Then
                    attribute_id = Me.container.oClasses.attribute_pool(i).attribute_id
                    found = True

                End If
            Next
            If found = False Then
                Dim omsg As New bc_cs_message("Blue Curve", "Attribute: " + assign_form.lselparent.Text + " is a user table attribute cannot edit!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Sub
                Exit Sub
            End If

            Dim fedit As New bc_am_cp_edit
            fedit.height = 518
            fedit.ltitle.Text = "Modify Attribute " + assign_form.lselparent.Text
            fedit.Cworkflow.Enabled = False
            fedit.Caudit.Enabled = True
            fedit.Tentry.Visible = False
            fedit.centry.Visible = False
            fedit.uxDetails.Visible = True
            For i = 0 To Me.container.oClasses.attribute_pool.Count - 1
                If Me.container.oClasses.attribute_pool(i).name = assign_form.lselparent.Text Then
                    fedit.Tattname.Text = Me.container.oClasses.attribute_pool(i).name
                    If Me.container.oClasses.attribute_pool(i).show_workflow = 1 Then
                        fedit.Cworkflow.Checked = True
                    End If
                    If Me.container.oClasses.attribute_pool(i).nullable = 0 Then
                        fedit.cmandatory.Checked = True
                    End If
                    If Me.container.oClasses.attribute_pool(i).submission_code = 2 Then
                        fedit.Caudit.Checked = True
                    End If

                    If Me.container.oClasses.attribute_pool(i).is_lookup = True Then
                        fedit.clookup.Checked = True
                        fedit.Tsql.Text = Me.container.oClasses.attribute_pool(i).lookup_Sql
                    End If
                    If Me.container.oClasses.attribute_pool(i).is_def = True Then
                        fedit.chkdef.Checked = True
                        fedit.defsql.Text = Me.container.oClasses.attribute_pool(i).def_sql
                        fedit.defsql.Enabled = True
                    End If

                    fedit.Cdt.Items.Add("String")
                    fedit.Cdt.Items.Add("Number")
                    fedit.Cdt.Items.Add("Boolean")
                    fedit.Cdt.Items.Add("Date")
                    Select Case Me.container.oClasses.attribute_pool(i).type_id
                        Case 1
                            fedit.Cdt.SelectedIndex = 0
                            fedit.Tlength.Text = Me.container.oClasses.attribute_pool(i).length
                            If Me.container.oClasses.attribute_pool(i).repeats = 1 Then
                                fedit.cpopup.Checked = True
                            End If
                        Case 2
                            fedit.Cdt.SelectedIndex = 1
                        Case 3
                            fedit.Cdt.SelectedIndex = 2
                        Case 5
                            fedit.Cdt.SelectedIndex = 3
                    End Select
                    Exit For
                End If
            Next

            'fedit.Height = fedit.Height + fedit.uxDetails.Height

            fedit.ShowDialog()
            If fedit.cancel_selected = True Then
                Exit Sub
            Else
                view.Cursor = Cursors.WaitCursor

                If modify_attribute(attribute_id, fedit.Tattname.Text, fedit.Cdt.SelectedIndex, fedit.Caudit.Checked, fedit.cmandatory.Checked, fedit.Cworkflow.Checked, fedit.clookup.Checked, fedit.Tsql.Text, fedit.Tlength.Text, fedit.cpopup.Checked, fedit.chkdef.Checked, fedit.defsql.Text) = True Then
                    Dim labs As New ArrayList
                    oname = assign_form.lselparent.Text

                    If fedit.Tattname.Text <> oname Then
                        For i = 0 To assign_form.lselparent.Items.Count - 1
                            labs.Add(assign_form.lselparent.Items(i))
                        Next
                        assign_form.lselparent.Items.Clear()
                        For i = 0 To labs.Count - 1
                            If labs(i) = oname Then
                                assign_form.lselparent.Items.Add(fedit.Tattname.Text)
                            Else
                                assign_form.lselparent.Items.Add(labs(i))
                            End If
                        Next

                        save_user_display_attributes(assign_form)
                        assign_form.lselparent.Text = fedit.Tattname.Text
                    End If
                End If
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_users", "modify_attribute", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            view.Cursor = Cursors.Default
        End Try
    End Sub


    REM ING JUNE 2012
    Public Function modify_attribute(ByVal attribute_id As Long, ByVal name As String, ByVal dt As Integer, ByVal audit As Boolean, ByVal mandatory As Boolean, ByVal workflow As Boolean, ByVal lookup As Boolean, ByVal sql As String, ByVal length As String, ByVal popup As Boolean, def As Boolean, def_sql As String) As Boolean
        Try

            Dim oatt As New bc_om_attribute
            REM get attribute id
            oatt.name = name
            oatt.attribute_id = attribute_id
            modify_attribute = False

            If name = "" Then
                Dim omsg As New bc_cs_message("Blue Curve", "attribtute name must be entered.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Function
            End If
            REM now check name isnt if an existing attribute that isnt the selected one
            For i = 0 To Me.container.oClasses.attribute_pool.Count - 1
                If Trim(UCase(Me.container.oClasses.attribute_pool(i).name)) = Trim(UCase(name)) And Trim(UCase(Me.container.oClasses.attribute_pool(i).name)) <> Trim(UCase(oatt.name)) Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Attribute name: " + name + " is in use by another attribute!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Function
                End If
            Next

            Select Case dt
                Case 0
                    oatt.type_id = 1
                    If IsNumeric(length) = True Then
                        oatt.length = CInt(length)
                    Else
                        oatt.length = 255
                    End If
                    If popup = True Then
                        oatt.repeats = 1
                    End If
                Case 1
                    oatt.type_id = 2
                Case 2
                    oatt.type_id = 3
                Case 3
                    oatt.type_id = 5
            End Select
            If audit = True Then
                oatt.submission_code = 2
            Else
                oatt.submission_code = 1
            End If
            oatt.nullable = True
            If mandatory Then
                oatt.nullable = False
            End If
            oatt.show_workflow = False
            If workflow = True Then
                oatt.show_workflow = 1
            End If
            oatt.is_lookup = False
            If lookup = True Then
                oatt.is_lookup = True
                oatt.lookup_sql = sql
            End If
            If def = True Then
                oatt.is_def = True
                oatt.def_sql = def_sql
            End If
            oatt.write_mode = bc_om_attribute.UPDATE
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                oatt.db_write()
            Else
                oatt.tmode = bc_cs_soap_base_class.tWRITE
                If oatt.transmit_to_server_and_receive(oatt, True) = False Then
                    Exit Function
                End If
            End If
            REM if lookup reload attributes to get lookup
            If oatt.is_lookup Then
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    Me.container.oClasses.db_read()
                Else
                    Me.container.oClasses.tmode = bc_cs_soap_base_class.tREAD
                    If Me.container.oClasses.transmit_to_server_and_receive(Me.container.oClasses, True) = False Then
                        Exit Function
                    End If
                End If

            Else
                REM now assign to pool if not read back
                For i = 0 To Me.container.oClasses.attribute_pool.Count - 1
                    If Me.container.oClasses.attribute_pool(i).attribute_id = oatt.attribute_id Then
                        Me.container.oClasses.attribute_pool(i) = oatt
                        Exit For
                    End If
                Next
            End If
            modify_attribute = True

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_users", "modify_attribute", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Function
End Class
