Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports BlueCurve.Core.AS


Imports System.Windows.Forms

''' <summary>
''' Module: Common Platform
''' Type: AM
''' Description: Common Platform Shell 
''' Version: 5.0
''' Change History:
''' </summary>

Public Class bc_am_cp_container

    Public mode As Integer
    Public set_sync As Boolean = False
    Public Const VIEW As Integer = 0
    Public Const EDIT_LITE As Integer = 1
    Public Const EDIT_FULL As Integer = 2
    Public Const TAXONOMY As Integer = 1
    Public Const USERS As Integer = 2
    Public Const PUBTYPES As Integer = 3
    Public Const TRANSLATE As Integer = 4
    Public Const MONITOR As Integer = 5

    Public sConn As String
    Public sUser As String

    Public oClasses As New bc_om_entity_classes
    Public oEntities As New bc_om_entities
    Public oAdmin As New bc_om_user_admin
    Public oPubtypes As New bc_om_pub_types
    Public oUsers As New bc_om_users
    Public oTranslate As bc_om_translation_item
    Public uncomitted_data As Boolean = False
    Private cUser As Object
    Private cTaxonomy As bc_am_taxonomy
    Private cPubTypes As bc_am_pubtype
    Private cUsers As bc_am_users
    Private cTranslate As bc_am_translation
    Private cMonitor As bc_am_monitor


    Public smsgshown As Boolean = False
    Private selected_form As Integer = 1

    Private Const data_icon As Integer = 4
    Private Const structure_icon As Integer = 5

    Private Sub uxExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxExit.Click
        If check_uncomitted_data() = False Then
            Application.Exit()
        End If
    End Sub
    REM FIL JUNE 2013
    Private Function check_uncomitted_data() As Boolean
        Dim tx As String = ""
        check_uncomitted_data = False
        If uncomitted_data = True Then
            check_uncomitted_data = True
            Dim omsg As New bc_cs_message("Blue Curve", "You have uncommitted data please commit or discard  prior to terminating application", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
        ElseIf Me.oAdmin.sync_types.sync_types.Count = 0 And set_sync = True And smsgshown = False Then
            REM FIL JUNE 2012
            Dim omsg As New bc_cs_message("Blue Curve", "Data has changed do you wish to set sync for all users?", bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
            If omsg.cancel_selected = False Then
                set_sync_flag_on_server()
            End If
            smsgshown = True
        ElseIf Me.oAdmin.sync_types.sync_types.Count > 0 And smsgshown = False Then
            For i = 0 To Me.oAdmin.sync_types.sync_types.Count - 1
                If Me.oAdmin.sync_types.sync_types(i).sync_set = True Then
                    If tx = "" Then
                        tx = Me.oAdmin.sync_types.sync_types(i).name
                    Else
                        tx = tx + "; " + Me.oAdmin.sync_types.sync_types(i).name
                    End If
                End If
            Next
            If tx <> "" Then
                Dim omsg As New bc_cs_message("Blue Curve", "Data has changed for " + tx + " set sync in these areas for all users?", bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
                If omsg.cancel_selected = False Then
                    set_partial_sync_flag_on_server()
                End If
                smsgshown = True
            End If

        End If
    End Function
    Private Sub set_sync_flag_on_server()
        Dim osync As New bc_om_set_sync

        If bc_cs_central_settings.connection_method = bc_cs_central_settings.ADO Then
            osync.db_write()
        Else
            osync.tmode = bc_cs_soap_base_class.tWRITE
            If osync.transmit_to_server_and_receive(osync, False) = False Then
                Dim omsg As New bc_cs_message("Blue Curve", "Failed to set user synchronisation flag.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            End If
        End If
    End Sub
    Private Sub set_partial_sync_flag_on_server()


        If bc_cs_central_settings.connection_method = bc_cs_central_settings.ADO Then
            Me.oAdmin.sync_types.db_write()
        Else
            Me.oAdmin.sync_types.tmode = bc_cs_soap_base_class.tWRITE
            If Me.oAdmin.sync_types.transmit_to_server_and_receive(Me.oAdmin.sync_types, False) = False Then
                Dim omsg As New bc_cs_message("Blue Curve", "Failed to set user synchronisation flag.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            End If
        End If
    End Sub

    Private Sub bc_am_cp_container_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If check_uncomitted_data() = True Then
            e.Cancel = True
        End If
    End Sub

    Private Sub bc_am_cp_container_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim log = New bc_cs_activity_log("bc_am_cp_container", "bc_am_cp_container_Load", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve", "Loading Common Platform", 20, False, True)            'Me.Visible = False

            If logon() = True Then
                cUser = New bc_am_cp_users
                With cUser
                    .sconn = sConn
                    .suser = sUser
                End With

                bc_cs_central_settings.progress_bar.increment("Initialising...")

                uxUser.Text = sUser
                uxConnection.Text = sConn

                cTaxonomy = New bc_am_taxonomy(Me, New bc_am_cp_main)
                cPubTypes = New bc_am_pubtype(Me, New bc_am_cp_pub_type)
                cUsers = New bc_am_users(Me, New bc_am_cp_users)
                cTranslate = New bc_am_translation(Me, New bc_am_cp_translation)
                cMonitor = New bc_am_monitor(Me, New bc_am_cp_monitor)



                Me.cTaxonomy.from_class_list = False
                bc_cs_central_settings.progress_bar.hide()
                uxNavBar.Items(0).Selected = True

                If Me.oAdmin.sync_types.sync_types.Count = 0 Then
                    Me.SetSynchronizeToolStripMenuItem.Visible = True
                    Me.SetSynchroinzeAllToolStripMenuItem.Visible = False
                    Me.SetSynchronizeFilesToolStripMenuItem.Visible = False
                    Me.SetSynchronizeOPublicatiosToolStripMenuItem.Visible = False
                    Me.SetSynchronizeEntitiesToolStripMenuItem.Visible = False
                Else
                    Me.SetSynchronizeToolStripMenuItem.Visible = False
                    Me.SetSynchroinzeAllToolStripMenuItem.Visible = True
                    Me.SetSynchronizeFilesToolStripMenuItem.Visible = True
                    Me.SetSynchronizeOPublicatiosToolStripMenuItem.Visible = True
                    Me.SetSynchronizeEntitiesToolStripMenuItem.Visible = True
                End If




            Else
                bc_cs_central_settings.progress_bar.hide()
                Application.Exit()
            End If


        Catch ex As Exception
            Dim omsg As New bc_cs_message("Blue Curve", "System load error: " + ex.Message, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
        Finally
            log = New bc_cs_activity_log("bc_am_cp_container", "bc_am_cp_container_Load", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Private Function logon() As Boolean

        Dim log = New bc_cs_activity_log("bc_am_cp_container", "logon", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            Dim user_found As Boolean = False
            Dim uname As String
            Dim bcs As New bc_cs_central_settings(True)

            logon = False

            bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.connection_method
            oUsers.inactive = True
            oUsers.read_partial_sync = True
            If bc_cs_central_settings.connection_method = bc_cs_central_settings.ADO Then
                Me.sConn = "Connected to " + bc_cs_central_settings.servername + " via ADO"
                If oUsers.test_connection = False Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Cannot connect to " + bc_cs_central_settings.servername + ":" + bc_cs_central_settings.dbname + " application cannot be launched.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Function
                Else
                    oUsers.db_read()
                    REM if no users returned then create system account
                    If oUsers.user.Count = 0 Then
                        create_system_user()
                    End If
                    logon = True
                End If
            ElseIf bc_cs_central_settings.connection_method = bc_cs_central_settings.SOAP Then
                Me.sConn = "Connected to " + bc_cs_central_settings.soap_server + " via Webservices"
                oUsers.tmode = bc_om_users.tREAD
                If oUsers.transmit_to_server_and_receive(oUsers, True) = False Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Cannot connect to " + bc_cs_central_settings.soap_server + " application cannot be launched.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Function
                Else
                    logon = True
                End If
            Else
                Dim omsg As New bc_cs_message("Blue Curve", "Invalid connection method" + bc_cs_central_settings.connection_method + " application cannot be launched.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Me.Cursor = Windows.Forms.Cursors.Default
                System.Windows.Forms.Application.Exit()
                Exit Function
            End If
            oUsers.read_partial_sync = False

            REM Steve Wooderson 21/07/2011 If logon form in use match user on user_name
            If (bc_cs_central_settings.show_authentication_form = 1 Or bc_cs_central_settings.show_authentication_form = 3) Then
                uname = bc_cs_central_settings.user_name
                For i = 0 To oUsers.user.Count - 1
                    If LCase(oUsers.user(i).user_name) = LCase(uname) Then
                        bc_cs_central_settings.logged_on_user_id = oUsers.user(i).id
                        Me.sUser = "User " + oUsers.user(i).first_name + " " + oUsers.user(i).surname + " Role: " + oUsers.user(i).role
                        user_found = True
                        Exit For
                    End If
                Next
            Else
                uname = bc_cs_central_settings.GetLoginName
                For i = 0 To oUsers.user.Count - 1
                    If LCase(oUsers.user(i).os_user_name) = LCase(uname) Then
                        bc_cs_central_settings.logged_on_user_id = oUsers.user(i).id
                        Me.sUser = "User " + oUsers.user(i).first_name + " " + oUsers.user(i).surname + " Role: " + oUsers.user(i).role
                        user_found = True
                        Exit For
                    End If
                Next
            End If

            bc_cs_central_settings.progress_bar.increment("Loading Data...")

            'If user_found = False Then
            '    Dim omsg As New bc_cs_message("Blue Curve", "User authentication failed. Application cannot be launched.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            '    logon = False
            'Else
            logon = False
            REM now load rest of common data
            oClasses.classes.Clear()
            oEntities.entity.Clear()
            Me.oPubtypes.pubtype.Clear()
            oEntities.get_inactive = False

            'oEntities.search_attributes.search_class = -1

            REM read in data
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                oClasses.get_inactive = True
                oClasses.db_read()
                oEntities.get_inactive = True
                oEntities.db_read()
                oAdmin.db_read()
                oPubtypes.get_inactive = True
                oPubtypes.db_read()
                logon = True
            Else
                oClasses.get_inactive = True
                oClasses.tmode = bc_om_entity_classes.tREAD
                If oClasses.transmit_to_server_and_receive(oClasses, True) = True Then
                    oEntities.get_inactive = True
                    oEntities.tmode = bc_om_entities.tREAD
                    If oEntities.transmit_to_server_and_receive(oEntities, True) = True Then
                        oAdmin.tmode = bc_om_entities.tREAD
                        If oAdmin.transmit_to_server_and_receive(oAdmin, True) = True Then
                            oPubtypes.get_inactive = True
                            oPubtypes.tmode = bc_om_pub_types.tREAD
                            If oPubtypes.transmit_to_server_and_receive(oPubtypes, True) = True Then
                                logon = True
                            End If
                        End If
                    End If

                End If
            End If
            'End If
            REM FIL FEB 2012
            REM remove classes that are not custom
            Dim ccount As Integer = 0
            While ccount < oClasses.classes.Count
                If oClasses.classes(ccount).class_type_id <> 1 And oClasses.classes(ccount).class_type_id <> 4 Then
                    oClasses.classes.RemoveAt(ccount)
                    ccount = ccount - 1
                End If
                ccount = ccount + 1
            End While
            REM remove schemas that are core
            ccount = 0
            While ccount < oClasses.schemas.Count()
                If oClasses.schemas(ccount).core <> 0 Then
                    oClasses.schemas.RemoveAt(ccount)
                    ccount = ccount - 1
                End If
                ccount = ccount + 1
            End While

            'load the search attributes that were synced
            bc_am_load_objects.obc_entities = bc_am_load_objects.obc_entities.read_data_from_file(bc_cs_central_settings.local_template_path + bc_am_load_objects.ENTITIES_FILENAME)
            'oEntities.search_attributes.search_values = bc_am_load_objects.obc_entities.search_attributes.search_values

            bc_am_load_objects.obc_entities = oEntities

            For i = 0 To oUsers.user.Count - 1
                If ((bc_cs_central_settings.show_authentication_form = 0 Or bc_cs_central_settings.show_authentication_form = 2) And LCase(oUsers.user(i).os_user_name) = LCase(uname)) _
                  Or ((bc_cs_central_settings.show_authentication_form = 1 Or bc_cs_central_settings.show_authentication_form = 3) And LCase(oUsers.user(i).user_name) = LCase(uname)) Then
                    For j = 0 To Me.oAdmin.roles.Count - 1
                        If Me.oAdmin.roles(j).id = Me.oUsers.user(i).role_id Then
                            If Me.oAdmin.roles(j).level = 1 Then
                                Me.mode = EDIT_LITE
                            ElseIf Me.oAdmin.roles(j).level = 2 Then
                                Me.mode = EDIT_FULL
                            Else
                                Me.mode = VIEW
                            End If
                            REM now set visibity of application for role
                            Me.uxTaxonomy.Visible = False
                            Me.uxUsers.Visible = False
                            Me.uxPubTypes.Visible = False
                            Me.uxNavBar.Items.Clear()
                            Dim lvw As ListViewItem
                            Me.uxTranslate.Visible = False

                            REM if no application at least show users
                            If Me.oAdmin.roles(j).apps.count = 0 Then
                                Me.uxUsers.Visible = True
                                selected_form = USERS
                                lvw = New ListViewItem("Users", 1)
                                Me.uxNavBar.Items.Add(lvw)
                            Else
                                selected_form = Me.oAdmin.roles(j).apps(0)
                                For k = 0 To Me.oAdmin.roles(j).apps.count - 1
                                    Select Case Me.oAdmin.roles(j).apps(k)
                                        Case TAXONOMY
                                            Me.uxTaxonomy.Visible = True
                                            lvw = New ListViewItem("Taxonomy", 0)
                                            Me.uxNavBar.Items.Add(lvw)
                                        Case USERS
                                            Me.uxUsers.Visible = True
                                            lvw = New ListViewItem("Users", 1)
                                            Me.uxNavBar.Items.Add(lvw)
                                        Case PUBTYPES
                                            Me.uxPubTypes.Visible = True
                                            lvw = New ListViewItem("Publication Types", 2)
                                            Me.uxNavBar.Items.Add(lvw)
                                        Case TRANSLATE
                                            Me.uxTranslate.Visible = True
                                            lvw = New ListViewItem("Translation", 3)
                                            Me.uxNavBar.Items.Add(lvw)
                                        Case MONITOR
                                            Me.uxNavBar.Visible = True
                                            lvw = New ListViewItem("Monitoring", 4)
                                            Me.uxNavBar.Items.Add(lvw)
                                    End Select
                                Next

                                'For l = 0 To Me.MainMenuStrip.Items.Count - 1
                                '    If MainMenuStrip.Items(l).Visible = False Then
                                '        Me.uxNavBar.Items(l).Remove()
                                '    End If
                                'Next
                            End If
                            Exit For
                        End If
                    Next
                End If
            Next
            set_role_level_access()
        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_cp_container", "logon", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_cp_container", "logon", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function
    Private Sub set_role_level_access()
        Select Case Me.mode
            Case bc_am_cp_container.VIEW
                Me.uxStructure.Visible = False
            Case bc_am_cp_container.EDIT_LITE
                Me.uxStructure.Visible = False


        End Select
    End Sub
    Public Sub create_system_user()

        Dim log = New bc_cs_activity_log("bc_am_cp_container", "create_system_user", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try

            Dim ouser As New bc_om_user
            bc_cs_central_settings.logged_on_user_id = 0
            ouser.first_name = "System"
            ouser.surname = "Admin"
            ouser.user_name = "System Admin"
            ouser.write_mode = bc_om_user.INSERT
            ouser.os_user_name = bc_cs_central_settings.GetLoginName

            REM create new role give it max edit rights
            Dim orole As New bc_om_user_role
            orole.description = "System Admin"
            orole.level = 2
            orole.write_mode = bc_om_user_role.INSERT
            orole.db_write()
            ouser.role_id = orole.id
            ouser.role = "System Admin"

            Dim olanguage As New bc_om_user_language
            olanguage.language_name = "English"
            olanguage.write_mode = bc_om_user_language.INSERT
            olanguage.db_write()
            ouser.language_id = olanguage.language_id

            Dim office As New bc_om_user_office
            office.description = "System Admin"
            office.db_write()
            ouser.office_id = office.id

            ouser.db_write()
            bc_cs_central_settings.logged_on_user_id = ouser.id
            Me.oUsers.user.Add(ouser)

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_cp_container", "create_system_user", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_cp_container", "create_system_user", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    Private Sub load_forms()

        Dim log = New bc_cs_activity_log("bc_am_cp_container", "load_forms", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try

            Me.Cursor = Windows.Forms.Cursors.WaitCursor

            ExitSignOutToolStripMenuItem.Visible = False
            If bc_cs_central_settings.show_authentication_form = 1 Or bc_cs_central_settings.override_username_password = True Then
                ExitSignOutToolStripMenuItem.Visible = True
            End If


            Dim app As String = ""

            cUsers.view.Hide()
            cPubTypes.view.Hide()
            cTaxonomy.view.Hide()
            cTranslate.view.Hide()
            cMonitor.view.Hide()
            Me.uxViewToolStrip.Visible = False

            Select Case selected_form
                Case USERS
                    app = "Users"
                    With cUser
                        cUsers.view.TopLevel = False
                        cUsers.view.Parent = Me.uxMainPanel
                        cUsers.view.Dock = DockStyle.Fill
                        cUsers.load_all()
                        cUsers.view.Show()
                    End With
                Case TAXONOMY
                    app = "Taxonomy"
                    With cTaxonomy
                        cTaxonomy.view.TopLevel = False
                        cTaxonomy.view.Parent = Me.uxMainPanel
                        cTaxonomy.view.Dock = DockStyle.Fill
                        cTaxonomy.load_all()
                        cTaxonomy.view.Show()
                        cTaxonomy.size_nicely()
                        Me.cTaxonomy.show_data_toolbar()
                        Me.cTaxonomy.from_class_list = True
                        load_top_tb(0)
                        Me.uxViewToolStrip.Visible = True
                    End With
                Case PUBTYPES
                    app = "Publication Types"
                    With cPubTypes
                        cPubTypes.view.TopLevel = False
                        cPubTypes.view.Parent = Me.uxMainPanel
                        cPubTypes.view.Dock = DockStyle.Fill
                        cPubTypes.load_all()
                        cPubTypes.view.Show()
                    End With
                Case TRANSLATE
                    app = "Translation"
                    With cTranslate
                        cTranslate.view.TopLevel = False
                        cTranslate.view.Parent = Me.uxMainPanel
                        cTranslate.view.Dock = DockStyle.Fill
                        cTranslate.load_all()
                        cTranslate.view.Show()
                    End With
                Case MONITOR
                    app = "Monitoring"
                    With cMonitor
                        cMonitor.view.TopLevel = False
                        cMonitor.view.Parent = Me.uxMainPanel
                        cMonitor.view.Dock = DockStyle.Fill
                        cMonitor.load_all()
                        Dim ctmonitor As New Cbc_am_cp_monitor(cMonitor.view)
                        If ctmonitor.load_data = False Then

                        Else
                            cMonitor.view.Show()
                        End If

                    End With



            End Select

            Me.Text = "Common Platform - Blue Curve"
            Me.SchedulerToolStripMenuItem.Visible = False
            Select Case Me.mode
                Case VIEW
                    Me.Text = String.Concat(Me.Text, " ", "(View Only)")
                Case EDIT_LITE
                    Me.Text = String.Concat(Me.Text, " ", "(Edit Lite)")
                Case EDIT_FULL
                    Me.Text = String.Concat(Me.Text, " ", "(Advanced Edit)")
                    Me.SchedulerToolStripMenuItem.Visible = True
            End Select

            Me.Text = Me.Text + " " + app

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_cp_container", "load_forms", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_cp_container", "load_forms", bc_cs_activity_codes.TRACE_EXIT, "")
            Me.Cursor = Windows.Forms.Cursors.Default
        End Try

    End Sub

    Private Sub uxAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxAbout.Click

        Dim about As New bc_am_cp_about
        about.uxProduct.Text = Application.ProductName
        about.uxVersion.Text = Application.ProductVersion
        about.ShowDialog()
    End Sub

    Private Sub uxUsers_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxUsers.Click

        For i = 0 To Me.uxNavBar.Items.Count - 1
            If Me.uxNavBar.Items(i).Text = "Users" Then
                Me.uxNavBar.Items(i).Selected = True
                Exit For
            End If
        Next
    End Sub


    Private Sub uxTaxonomy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxTaxonomy.Click

        For i = 0 To Me.uxNavBar.Items.Count - 1
            If Me.uxNavBar.Items(i).Text = "Taxonomy" Then
                Me.uxNavBar.Items(i).Selected = True
                Exit For
            End If
        Next

    End Sub

    Private Sub uxPubTypes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxPubTypes.Click

        For i = 0 To Me.uxNavBar.Items.Count - 1
            If Me.uxNavBar.Items(i).Text = "Publication Types" Then
                Me.uxNavBar.Items(i).Selected = True
                Exit For
            End If
        Next

    End Sub

    Private Sub uxNavBar_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxNavBar.SelectedIndexChanged
        Dim log = New bc_cs_activity_log("bc_am_cp_container", "uxNavBar_SelectedIndexChanged", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Me.uxViewToolStrip.Visible = False
            If uxNavBar.SelectedItems.Count = 0 Then
                Exit Sub
            End If
            If uxNavBar.SelectedItems(0).Text = "Taxonomy" Then
                load_top_tb(0)
                Me.uxViewToolStrip.Visible = True
                Me.uxStructure.Select()
                selected_form = TAXONOMY
                load_forms()
            ElseIf uxNavBar.SelectedItems(0).Text = "Users" Then
                Me.uxGenericToolStrip.Items.Clear()
                selected_form = USERS
                load_forms()
            ElseIf uxNavBar.SelectedItems(0).Text = "Publication Types" Then
                Me.uxGenericToolStrip.Items.Clear()
                Me.uxGenericToolStrip.Items.Clear()
                selected_form = PUBTYPES
                load_forms()
            ElseIf uxNavBar.SelectedItems(0).Text = "Translation" Then
                Me.uxGenericToolStrip.Items.Clear()
                selected_form = TRANSLATE
                load_forms()
            ElseIf uxNavBar.SelectedItems(0).Text = "Monitoring" Then
                Me.uxGenericToolStrip.Items.Clear()
                selected_form = MONITOR
                load_forms()
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_cp_container", "uxNavBar_SelectedIndexChanged", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_cp_container", "uxNavBar_SelectedIndexChanged", bc_cs_activity_codes.TRACE_EXIT, "")
            Me.Cursor = Windows.Forms.Cursors.Default
        End Try

    End Sub
    Private Sub load_top_tb(ByVal mode As Integer)
        Dim newitem As ToolStripButton

        Me.uxViewToolStrip.ImageList = uxToolStripImageList

        With Me.uxViewToolStrip.Items
            .Clear()
            newitem = .Add("Data")
            If mode = 0 Then
                newitem.Checked = True
            End If
            newitem.ImageIndex = data_icon
            If Me.mode = bc_am_cp_container.EDIT_FULL Then
                newitem = .Add("Structure")
                If mode = 1 Then
                    newitem.Checked = True
                End If
                newitem.ImageIndex = structure_icon
            End If

        End With
    End Sub

    Private Sub uxData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxData.Click
        Me.cTaxonomy.show_data_toolbar()
    End Sub
    REM ING JUNE 2012
    Private Sub bc_am_cp_container_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        Try
            If Not Me.cTaxonomy Is Nothing Then
                Me.cTaxonomy.view.lvwvalidate.Columns(0).Width = Me.cTaxonomy.view.lvwvalidate.Width - 22
                Me.cTaxonomy.view.lvwatt.Columns(0).Width = Me.cTaxonomy.view.lvwatt.Width - 170
                Me.cTaxonomy.view.Lentities.Columns(0).Width = Me.cTaxonomy.view.Lentities.Width - 22
                Me.cTaxonomy.data_grid_size()
            End If

            If Not Me.cPubTypes Is Nothing Then
                Me.cPubTypes.view.lvwvalidate.Columns(0).Width = Me.cPubTypes.view.lvwvalidate.Width - 22
                Me.cPubTypes.size_nicely()
            End If

            If Not Me.cUsers Is Nothing Then
                Me.cUsers.view.lvwvalidate.Columns(0).Width = Me.cPubTypes.view.lvwvalidate.Width - 22
                Me.cUsers.size_nicely()
            End If

            ' stop resizing when a certain size is reached
            If Me.Width < 1024 Then
                Me.Width = 1024
                Exit Sub
            End If

            If Me.Height < 750 Then
                Me.Height = 750
                Exit Sub
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub uxGenericToolStrip_ItemClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles uxGenericToolStrip.ItemClicked
        Me.cTaxonomy.view.Cursor = Cursors.WaitCursor
        Me.Cursor = Cursors.WaitCursor


        Select Case Trim(e.ClickedItem.Text)
            Case "&Add Schema", "Add Schema"
                Me.cTaxonomy.add_schema_menu()
            Case "&Edit Schema", "Edit Schema"
                Me.cTaxonomy.change_schema_menu()
            Case "&Deactivate Schema", "Deactivate Schema"
                Me.cTaxonomy.activate_schema(Me.cTaxonomy.view.cschema.Text)
            Case "&Activate Schema", "Activate Schema"
                Me.cTaxonomy.activate_schema(Me.cTaxonomy.view.cschema.Text)
            Case "&Delete Schema", "Delete Schema"
                Me.cTaxonomy.delete_schema(Me.cTaxonomy.view.cschema.Text)
            Case "Add Class"
                Me.cTaxonomy.add_class_menu()
            Case "Edit Class"
                Me.cTaxonomy.rename_class_menu()
            Case "Deactivate Class", "Activate Class"
                Me.cTaxonomy.activate_class(Me.cTaxonomy.view.ttaxonomy.SelectedNode.Text)
            Case "Delete Class"
                Me.cTaxonomy.delete_class(Me.cTaxonomy.view.ttaxonomy.SelectedNode.Text)
            Case "Add Class Link", "Add Class Link"
                Me.cTaxonomy.add_link_menu()
            Case "Remove Class Link", "Remove Class Link"
                Me.cTaxonomy.remove_class_link(Me.cTaxonomy.view.ttaxonomy.SelectedNode.Text)
            Case ("No.of Links")
                Me.cTaxonomy.set_number()
            Case "Add Entity"
                Me.cTaxonomy.add_entity_menu()
            Case "Change Name"
                Me.cTaxonomy.change_entity_menu()
            Case "Activate", "Deactivate"
                If Me.cTaxonomy.view.Visible = True Then
                    Me.cTaxonomy.activate_entity_menu()
                ElseIf Me.cPubTypes.view.Visible = True Then
                    Me.cPubTypes.activate_pubtype()
                ElseIf Me.cUsers.view.Visible = True Then
                    Me.cUsers.active()
                End If
            Case "Delete"
                Me.cTaxonomy.delete_entity_menu()
            Case "Association"
                If Me.cTaxonomy.view.Visible = True Then
                    Me.cTaxonomy.load_entity_assign(Me.cTaxonomy.view.tentities.SelectedNode.Text)
                Else
                    REM FIL JULY 2012
                    If Me.cUsers.view.tentities.SelectedNode.Text = "Business Areas" Then
                        Me.cUsers.assign_bus_area()
                    Else
                        Me.cUsers.assign_entity()
                    End If
                End If
            Case "Remove Association"
                Me.cTaxonomy.remove_association()
            Case "Navigate"
                Me.cTaxonomy.navigate()
            Case "Attribute Details"
                Me.cTaxonomy.attribute_details(False)
            Case "Commit Changes"
                If Me.cTaxonomy.view.Visible = True Then
                    Me.cTaxonomy.commit_changes()
                ElseIf Me.cPubTypes.view.Visible = True Then
                    Me.cPubTypes.commit_changes()
                Else
                    Me.cUsers.commit_changes()
                End If
            Case "Discard Changes"
                If Me.cTaxonomy.view.Visible = True Then
                    Me.cTaxonomy.discard_changes()
                ElseIf Me.cPubTypes.view.Visible = True Then
                    Me.cPubTypes.discard_changes()
                Else
                    Me.cUsers.discard_change()
                End If
            Case "Add PubType"
                Me.cPubTypes.add_pub_type_menu()
            Case "Change PubType Name"
                Me.cPubTypes.change_pub_type_menu()
            Case "Delete PubType"
                Me.cPubTypes.del_pubtype()
            Case "Add Language"
                If Me.cPubTypes.view.Visible = True Then
                    Me.cPubTypes.add_new_language_menu()
                Else
                    Me.cUsers.add_new_language_menu()
                End If
            Case "Add Role"
                Me.cUsers.add_new_role_menu()
            Case "Add Office"
                Me.cUsers.add_new_office_menu()
            Case "Add Bus Area"
                Me.cPubTypes.add_new_busarea_menu()
            Case "Add User"
                Me.cUsers.add_user_menu()
            Case "Delete User"
                Me.cUsers.delete_user()
            Case "Association Class"
                Me.cUsers.assign_new_class()
            Case "Display Attributes"
                Me.cUsers.assign_display_attributes()


        End Select
        Me.cTaxonomy.view.Cursor = Cursors.Default
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub uxViewToolStrip_ItemClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles uxViewToolStrip.ItemClicked
        Select Case e.ClickedItem.Text
            Case "Structure"
                Me.cTaxonomy.from_class_list = False
                Me.cTaxonomy.show_structure_toolbar()
                load_top_tb(1)

            Case "Data"
                Me.cTaxonomy.show_data_toolbar()
                Me.cTaxonomy.from_class_list = True
                load_top_tb(0)
        End Select
    End Sub


    Private Sub SetSynchronizeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SetSynchronizeToolStripMenuItem.Click
        set_sync_flag_on_server()
        REM maerk all users as pending sync
        REM ING JUNE 2012
        For i = 0 To oUsers.user.Count - 1
            oUsers.user(i).sync_level = 1
        Next
        Me.cUsers.reset_user_list()

        REM --------------
    End Sub

    Private Sub uxMainPanel_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles uxMainPanel.Paint

    End Sub

    Private Sub SetSynchroinzeAllToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SetSynchroinzeAllToolStripMenuItem.Click
        For i = 0 To Me.oAdmin.sync_types.sync_types.Count - 1
            Me.oAdmin.sync_types.sync_types(i).sync_set = True
            Me.set_partial_sync_flag_on_server()
        Next
        clear_sync_flag()
    End Sub
    Private Sub clear_sync_flag()
        For i = 0 To Me.oAdmin.sync_types.sync_types.Count - 1
            Me.oAdmin.sync_types.sync_types(i).sync_set = False

        Next
    End Sub

    Private Sub SetSynchronizeFilesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SetSynchronizeFilesToolStripMenuItem.Click
        Me.oAdmin.sync_types.sync_types(0).sync_set = True
        Me.oAdmin.sync_types.sync_types(1).sync_set = False
        Me.oAdmin.sync_types.sync_types(2).sync_set = False
        Me.set_partial_sync_flag_on_server()
        Me.oAdmin.sync_types.sync_types(0).sync_set = False
    End Sub

    Private Sub SetSynchronizeOPublicatiosToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SetSynchronizeOPublicatiosToolStripMenuItem.Click
        Me.oAdmin.sync_types.sync_types(0).sync_set = False
        Me.oAdmin.sync_types.sync_types(1).sync_set = True
        Me.oAdmin.sync_types.sync_types(2).sync_set = False
        Me.set_partial_sync_flag_on_server()
        Me.oAdmin.sync_types.sync_types(1).sync_set = False
    End Sub

    Private Sub SetSynchronizeEntitiesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SetSynchronizeEntitiesToolStripMenuItem.Click
        Me.oAdmin.sync_types.sync_types(0).sync_set = False
        Me.oAdmin.sync_types.sync_types(1).sync_set = False
        Me.oAdmin.sync_types.sync_types(2).sync_set = True
        Me.set_partial_sync_flag_on_server()
        Me.oAdmin.sync_types.sync_types(2).sync_set = False
    End Sub

    Private Sub uxMainMenu_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles uxMainMenu.ItemClicked

    End Sub

    Private Sub ShowToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ShowToolStripMenuItem.Click
        Dim fs As New bc_dx_task_schedule
        Dim cfs As New Cbc_dx_task_schedule
        If cfs.load_data(fs) = True Then
            fs.ShowDialog()
        End If

    End Sub

    Private Sub SchedulerToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SchedulerToolStripMenuItem.Click

    End Sub

    Private Sub ExitSignOutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitSignOutToolStripMenuItem.Click
        If check_uncomitted_data() = False Then
            Dim cl As New Cbc_dx_full_logon()
            cl.remove_credentials_file()
            Application.Exit()
        End If
    End Sub
End Class