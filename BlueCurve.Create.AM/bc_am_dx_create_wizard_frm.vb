Imports DevExpress.XtraWizard
Imports DevExpress.XtraEditors
Imports BlueCurve.Core.CS
Imports BlueCurve.Core.AS
Imports BlueCurve.Create.AM
Imports BlueCurve.Core.OM
Imports DevExpress.XtraTreeList

Public Class bc_dx_am_create_wizard_frm

    Private Const className = "bc_dx_am_create_wizard_frms"

    Public build_mode As Boolean = False

    Dim oopen_doc As bc_am_at_open_doc
    'Dim obcload As New bc_am_load("Create with Offline Working")
    Dim pubTypeId As Long
    Dim entityId As Long
    Dim selectedEntity1Id As Long
    Dim selectedEntity2Id As Long

    'Public is_composite As Boolean
    Public prev_child_category As Long
    Public sub_entity_class As Long = 0

    Private SearchUserEntitiesOnly = False
    Private SearchUserEntitiesOnly2 = False
    'Public primary_entities As New List(Of bc_om_entity)
    'Public secondary_entities As New List(Of bc_om_entity)
    'Public entity_mode As Boolean = 0
    Dim gclass As Long
    Dim guseronly As Boolean

    Public sel_pub_type_Id As Long = 0
    Public sel_entity_Id1 As Long = 0
    Public sel_entity_id2 As Long = 0
    Public is_composite As Boolean = False
    Public is_clone As Boolean = False
    Public gfrom_desktop As Boolean = False
    
    REM search
    Public searchItems As New List(Of bc_as_cat_taxononmy_item)
    Public Shared wtaxonomies As List(Of wtaxonomy)
    Class wtaxonomy
        Public pclass As Long
        Public pall_list As New List(Of bc_om_entity)
        Public ppref_list As New List(Of bc_om_entity)
        Public class_name As String
        Public filter_attribute_id As Long = 0
        Public filter_attribute_name As String
    End Class
    Public Sub New()

        DevExpress.Skins.SkinManager.EnableFormSkins()
        DevExpress.UserSkins.BonusSkins.Register()
        DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle("Blue")
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
    Public Sub bc_am_create_wizard_frm_load()

        Dim moduleName As String = "Load"
        Dim slog As New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_ENTRY, "")

        Try

            'If obcload.bcancelselected = True Then
            '    Exit Sub
            'End If

            If build_mode = True Then
                Me.WizardControl1.SelectedPage = WizardPublicationPage
            End If
            set_up_taxonomies()
            Me.uxDocOptions.SelectedIndex = 0

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.LOCAL Then
                Me.lname.Text = ""
                Me.lrole.Text = "Offline"
            Else
                For i = 0 To bc_am_load_objects.obc_users.user.Count - 1
                    With bc_am_load_objects.obc_users.user(i)
                        If bc_cs_central_settings.logged_on_user_id = .id Then
                            Me.lname.Text = .first_name + " " + .surname
                            Me.lrole.Text = .role
                            Exit For
                        End If
                    End With
                Next
            End If
            display()

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log(className, moduleName, bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_EXIT, "")
        End Try


    End Sub
    Sub set_up_taxonomies()
        Try
            Me.Cursor = Windows.Forms.Cursors.WaitCursor
            If Not IsNothing(wtaxonomies) Then
                Exit Sub
            End If
            wtaxonomies = New List(Of wtaxonomy)
            Dim wtax As wtaxonomy

            Dim found As Boolean
            For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                If bc_am_load_objects.obc_pub_types.pubtype(i).support_doc_only = False And bc_am_load_objects.obc_pub_types.pubtype(i).show_in_wizard = True Then
                    REM check if pub type in user business area
                    For m = 0 To bc_am_load_objects.obc_prefs.bus_areas.Count - 1
                        If bc_am_load_objects.obc_prefs.bus_areas(m) = bc_am_load_objects.obc_pub_types.pubtype(i).bus_area_id Then
                            If bc_am_load_objects.obc_pub_types.pubtype(i).child_category <> 0 Then
                                found = False
                                For v = 0 To wtaxonomies.Count - 1
                                    If wtaxonomies(v).pclass = bc_am_load_objects.obc_pub_types.pubtype(i).child_category Then
                                        found = True
                                        Exit For
                                    End If
                                Next
                                If found = False Then
                                    wtax = New wtaxonomy
                                    wtax.pclass = bc_am_load_objects.obc_pub_types.pubtype(i).child_category
                                    If bc_cs_central_settings.selected_conn_method <> bc_cs_central_settings.LOCAL Then
                                        For j = 0 To bc_am_load_objects.obc_entities.filter_attributes_types.Count - 1
                                            If bc_am_load_objects.obc_entities.filter_attributes_types(j).use_in_build = True AndAlso bc_am_load_objects.obc_entities.filter_attributes_types(j).class_id = wtax.pclass Then

                                                wtax.filter_attribute_id = bc_am_load_objects.obc_entities.filter_attributes_types(j).attribute_id
                                                wtax.filter_attribute_name = bc_am_load_objects.obc_entities.filter_attributes_types(j).attribute_name

                                                Exit For
                                            End If
                                        Next

                                    End If
                                    wtaxonomies.Add(wtax)
                                End If
                            End If
                            If bc_am_load_objects.obc_pub_types.pubtype(i).sub_entity_class <> 0 Then
                                found = False
                                For v = 0 To wtaxonomies.Count - 1
                                    If wtaxonomies(v).pclass = bc_am_load_objects.obc_pub_types.pubtype(i).sub_entity_class Then
                                        found = True
                                        Exit For
                                    End If
                                Next
                                If found = False Then
                                    wtax = New wtaxonomy
                                    wtax.pclass = bc_am_load_objects.obc_pub_types.pubtype(i).sub_entity_class
                                    If bc_cs_central_settings.selected_conn_method <> bc_cs_central_settings.LOCAL Then
                                        For j = 0 To bc_am_load_objects.obc_entities.filter_attributes_types.Count - 1
                                            If bc_am_load_objects.obc_entities.filter_attributes_types(j).use_in_build = True AndAlso bc_am_load_objects.obc_entities.filter_attributes_types(j).class_id = wtax.pclass Then
                                                wtax.filter_attribute_id = bc_am_load_objects.obc_entities.filter_attributes_types(j).attribute_id
                                                wtax.filter_attribute_name = bc_am_load_objects.obc_entities.filter_attributes_types(j).attribute_name
                                                Exit For
                                            End If
                                        Next

                                    End If
                                    wtaxonomies.Add(wtax)
                                End If
                            End If
                            Exit For
                        End If
                    Next
                End If
            Next

            For i = 0 To bc_am_load_objects.obc_entities.entity.Count - 1
                For l = 0 To wtaxonomies.Count - 1
                    If bc_am_load_objects.obc_entities.entity(i).class_id = wtaxonomies(l).pclass Then
                        wtaxonomies(l).class_name = bc_am_load_objects.obc_entities.entity(i).class_name
                        If wtaxonomies(l).filter_attribute_id = 0 Then
                            wtaxonomies(l).pall_list.Add(bc_am_load_objects.obc_entities.entity(i))
                        End If
                    End If
                Next
            Next
            REM filter attributes
            For l = 0 To wtaxonomies.Count - 1
                If wtaxonomies(l).filter_attribute_id > 0 Then
                    wtaxonomies(l).class_name = wtaxonomies(l).class_name
                    Dim ofatt = New bc_om_filter_attribute
                    ofatt.attribute_id = wtaxonomies(l).filter_attribute_id
                    ofatt.class_id = wtaxonomies(l).pclass

                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        ofatt.db_read()
                    Else
                        ofatt.tmode = bc_cs_soap_base_class.tREAD
                        If ofatt.transmit_to_server_and_receive(ofatt, True) = False Then
                            Exit Sub
                        End If
                    End If

                    Dim oti As bc_om_entity
                    Dim loti = New List(Of bc_om_entity)
                    For m = 0 To ofatt.results.Count - 1
                        oti = New bc_om_entity
                        oti.id = ofatt.results(m)
                        loti.Add(oti)
                    Next


                    Dim fss = From t In bc_am_load_objects.obc_entities.entity
                              Join p In loti On p.id Equals t.id
                             Select t
                    wtaxonomies(l).pall_list = fss.ToList


                    fss = From t In bc_am_load_objects.obc_entities.entity
                              Join p In loti On p.id Equals t.id
                              Join q In bc_am_load_objects.obc_prefs.pref On q.entity_id Equals t.id
                              Select t
                    wtaxonomies(l).ppref_list = fss.ToList
                End If
            Next


            Dim oent As bc_om_entity
            For i = 0 To bc_am_load_objects.obc_prefs.pref.Count - 1
                For l = 0 To wtaxonomies.Count - 1
                    If wtaxonomies(l).filter_attribute_id = 0 AndAlso bc_am_load_objects.obc_prefs.pref(i).class_id = wtaxonomies(l).pclass Then
                        With bc_am_load_objects.obc_prefs.pref(i)
                            oent = New bc_om_entity
                            oent.id = .entity_id
                            oent.name = .entity_name
                            oent.class_id = .class_id
                            oent.class_name = .class_name
                            wtaxonomies(l).ppref_list.Add(oent)
                            Exit For
                        End With
                    End If
                Next
            Next

            'END



        Catch ex As Exception
            MsgBox(ex.Message)

        Finally
            Me.Cursor = Windows.Forms.Cursors.Default
        End Try

    End Sub

    Public Sub display()

        Dim moduleName As String = " display"
        Dim slog As New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Me.TopMost = True
            Me.TopLevel = True
            Me.WizardControl1.Focus()
            Me.ShowDialog()


        Catch ex As Exception
            Dim db_err As New bc_cs_error_log(className, moduleName, bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Private Sub WizardControl1_CancelClick(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles WizardControl1.CancelClick
        sel_pub_type_Id = 0
    End Sub

    Private Sub WizardControl1_CustomizeCommandButtons(ByVal sender As Object, ByVal e As DevExpress.XtraWizard.CustomizeCommandButtonsEventArgs) Handles WizardControl1.CustomizeCommandButtons

        WizardControl1.Controls.Item(0).Enabled = True
        If e.Page.Name = "WelcomeWizardPage1" Then
            WizardControl1.Controls.Item(0).Enabled = False
        End If
        If e.Page.Name = "WizardPublicationPage" Then
            If build_mode = True Then
                WizardControl1.Controls.Item(0).Enabled = False REM next
            End If
        End If

        If e.Page.Name = "WizardEquityPage1" Then
            If sub_entity_class = 0 Then
                WizardControl1.Controls.Item(1).Visible = False REM next
                WizardControl1.Controls.Item(2).Visible = True REM Finnish
            Else
                WizardControl1.Controls.Item(1).Visible = True REM next
                WizardControl1.Controls.Item(2).Visible = False REM Finnish
            End If
        End If

        If e.Page.Name = "WizardEquityPage2" Then
            WizardControl1.Controls.Item(1).Visible = False REM next
            WizardControl1.Controls.Item(2).Visible = True REM Finnish
        End If

    End Sub


    Private Sub WizardControl1_SelectedPageChanged(ByVal sender As Object, ByVal e As DevExpress.XtraWizard.WizardPageChangedEventArgs) Handles WizardControl1.SelectedPageChanged

        If e.Page.Name = "WelcomeWizardPage1" Then
            Me.chkclone.Checked = False
            Me.chkclone.Visible = False

        End If
        If e.Page.Name = "WizardPublicationPage" Then
            selectedEntity1Id = 0
            selectedEntity2Id = 0
            Me.chkclone.Checked = False
            Me.chkclone.Visible = False

            load_pubtypes()

        End If

        If e.Page.Name = "WizardEquityPage1" Then
            gclass = prev_child_category
            guseronly = SearchUserEntitiesOnly

            If selectedEntity1Id = 0 Then
                Select Case True
                    Case bc_cs_central_settings.show_all_entities
                        uxTnToggle.Visible = True
                    Case Not bc_cs_central_settings.show_all_entities
                        uxTnToggle.Visible = False
                End Select

                Select Case True
                    Case bc_cs_central_settings.my_entities_default
                        SearchUserEntitiesOnly = True
                        uxTnToggle.Text = "All Subjects"
                        guseronly = True
                    Case Not bc_cs_central_settings.my_entities_default
                        SearchUserEntitiesOnly = False
                        uxTnToggle.Text = "My Subjects"
                        guseronly = False
                End Select

                'gclass = prev_child_category
                'guseronly = SearchUserEntitiesOnly
                Me.tsearch.Text = ""

                WizardControl1.Controls.Item(2).Enabled = False
                load_entities1(uxEntity1, prev_child_category, selectedEntity1Id, SearchUserEntitiesOnly, 0)
                'WizardControl1.Controls.Item(2).Enabled = False


            End If
        End If

        If e.Page.Name = "WizardEquityPage2" Then
            gclass = sub_entity_class
            guseronly = SearchUserEntitiesOnly2
            If selectedEntity2Id = 0 Then

                Select Case True
                    Case bc_cs_central_settings.show_all_entities
                        uxTnToggle2.Visible = True
                    Case Not bc_cs_central_settings.show_all_entities
                        uxTnToggle2.Visible = False
                End Select

                Select Case True
                    Case bc_cs_central_settings.my_entities_default
                        SearchUserEntitiesOnly2 = True
                        uxTnToggle2.Text = "All Subjects"
                    Case Not bc_cs_central_settings.my_entities_default
                        SearchUserEntitiesOnly2 = False
                        uxTnToggle2.Text = "My Subjects"
                End Select

                'gclass = sub_entity_class
                'guseronly = SearchUserEntitiesOnly2

                Me.tsearch2.Text = ""
                WizardControl1.Controls.Item(2).Enabled = False
                load_entities2(uxEntity2, sub_entity_class, selectedEntity2Id, SearchUserEntitiesOnly2, 1)
            End If

        End If

    End Sub

    Private Sub WizardControl1_SelectedPageChanging(ByVal sender As Object, ByVal e As DevExpress.XtraWizard.WizardPageChangingEventArgs) Handles WizardControl1.SelectedPageChanging
        Try
            searchtimer.Stop()
            searchtimer2.Stop()

            REM Page control
            If e.PrevPage.Name = "WelcomeWizardPage1" And e.Direction = Direction.Forward Then
                If uxDocOptions.SelectedIndex = 0 Then
                    e.Page = WizardPublicationPage
                End If
                If Me.build_mode = False Then
                    If Me.uxDocOptions.Properties.Items(Me.uxDocOptions.SelectedIndex).Value = 1 Then
                        Me.Cursor = Windows.Forms.Cursors.WaitCursor


                        Dim fdxopen As New bc_dx_doc_open
                        Dim cdxopen As New Cbc_dx_doc_Open(fdxopen)
                        If Cbc_dx_doc_Open.gdopen_date_from = "1-1-1900" Then
                            Cbc_dx_doc_Open.gdopen_date_from = DateAdd(DateInterval.Day, -7, Now)
                        End If
                        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.LOCAL Then
                            cdxopen.load_data(True, True, False)
                        Else
                            cdxopen.load_data(False, True, False)
                        End If
                        cdxopen.load_view()
                        fdxopen.TopLevel = True
                        Me.Cursor = Windows.Forms.Cursors.Default
                        Me.WindowState = Windows.Forms.FormWindowState.Minimized
                        fdxopen.ShowDialog()
                        If cdxopen.doc_opened = True Then
                            Me.WindowState = Windows.Forms.FormWindowState.Minimized

                        Else
                            Me.WindowState = Windows.Forms.FormWindowState.Normal
                        End If

                        Me.uxDocOptions.SelectedIndex = 0
                        e.Page = WelcomeWizardPage1
                        Exit Sub
                    End If
                End If
            End If

            If e.PrevPage.Name = "WizardPublicationPage" And e.Direction = Direction.Forward Then
                If Me.uxPubtypes.Visible = True Then
                    getPublicationDetails()
                End If

                If prev_child_category <> 0 Then
                    e.Page = WizardEquityPage1
                End If
            End If

            If e.PrevPage.Name = "WizardEquityPage1" And e.Direction = Direction.Forward Then

                getEntityDetails(uxEntity1, prev_child_category, selectedEntity1Id)

                e.Page = WizardEquityPage2
                WizardControl1.Controls.Item(1).Visible = False REM next
                WizardControl1.Controls.Item(2).Visible = True REM Finnish

                uxEntity1.Select()

            End If

            If e.PrevPage.Name = "WizardEquityPage2" And e.Direction = Direction.Backward Then
                getEntityDetails(uxEntity2, sub_entity_class, selectedEntity2Id)
                e.Page = WizardEquityPage1

                uxEntity2.Select()

            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("WizardControl1", "SelectedPageChanging", bc_cs_error_codes.USER_DEFINED, ex.Message)


        End Try
    End Sub

    Public Sub load_pubtypes()

        Dim moduleName As String = " load_pubtypes"
        Dim slog As New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim pubtypename As String
        Dim listitem As Long
        Dim selectedPubTypeId As Long
        Dim folders As New List(Of String)
        Me.npubtypes.Nodes.Clear()

        Dim found = True

        Try
            selectedPubTypeId = pubTypeId

            If Not IsNothing(bc_am_load_objects.obc_pub_types) Then
                Me.uxPubtypes.Items.Clear()
                For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                    If bc_am_load_objects.obc_pub_types.pubtype(i).support_doc_only = False And bc_am_load_objects.obc_pub_types.pubtype(i).show_in_wizard = True Then
                        REM check if pub type in user business area
                        For m = 0 To bc_am_load_objects.obc_prefs.bus_areas.Count - 1
                            If bc_am_load_objects.obc_prefs.bus_areas(m) = bc_am_load_objects.obc_pub_types.pubtype(i).bus_area_id Then
                                pubtypename = bc_am_load_objects.obc_pub_types.pubtype(i).name().ToString
                                Me.uxPubtypes.Items.Add(pubtypename)
                                found = False

                                If bc_am_load_objects.obc_pub_types.pubtype(i).folder_name <> "" Then
                                    For k = 0 To folders.Count - 1
                                        If folders(k) = bc_am_load_objects.obc_pub_types.pubtype(i).folder_name Then
                                            found = True
                                            Exit For
                                        End If
                                    Next
                                    If found = False Then
                                        folders.Add(bc_am_load_objects.obc_pub_types.pubtype(i).folder_name)

                                    End If
                                End If


                                If bc_am_load_objects.obc_pub_types.pubtype(i).id = selectedPubTypeId Then
                                    listitem = uxPubtypes.Items.Count - 1
                                End If

                                Exit For
                            End If
                        Next
                    End If
                Next
                Me.chkclone.Visible = False
                If listitem <> 0 Then
                    Me.uxPubtypes.SetSelected(listitem, True)
                    uxSelectedpubtype.Text = uxPubtypes.SelectedValue
                    For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                        If bc_am_load_objects.obc_pub_types.pubtype(i).name = uxSelectedpubtype.Text Then
                            If bc_am_load_objects.obc_pub_types.pubtype(i).is_clonable = True Then
                                Me.chkclone.Visible = True
                            End If
                            Exit For
                        End If
                    Next
                End If

            End If
            Me.npubtypes.Visible = False

            If folders.Count > 0 Then
                Me.uxPubtypes.SelectedIndex = -1
                Me.uxPubtypes.Visible = False
                Me.npubtypes.Visible = True

                folders.Sort()
                REM new node tree
                Me.npubtypes.BeginUpdate()
                Dim tln As Nodes.TreeListNode = Nothing
                Dim n As DevExpress.XtraTreeList.Nodes.TreeListNode
                Dim yesfolder As New List(Of String)

                Me.npubtypes.Nodes.Clear()
                For i = 0 To folders.Count - 1
                    n = npubtypes.AppendNode(New Object() {folders(i)}, tln)
                    n.StateImageIndex = 0
                    n.Tag = 0
                    For j = 0 To uxPubtypes.Items.Count - 1
                        For k = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                            If bc_am_load_objects.obc_pub_types.pubtype(k).name = uxPubtypes.Items(j) AndAlso bc_am_load_objects.obc_pub_types.pubtype(k).folder_name = folders(i) Then
                                n.Nodes.Add(New Object() {bc_am_load_objects.obc_pub_types.pubtype(k).name})
                                n.Nodes(n.Nodes.Count - 1).Tag = bc_am_load_objects.obc_pub_types.pubtype(k).id
                                n.Nodes(n.Nodes.Count - 1).StateImageIndex = 1
                                yesfolder.Add(bc_am_load_objects.obc_pub_types.pubtype(k).name())
                                Exit For
                            End If
                        Next
                    Next
                Next
                REM now add ones under General is no folder
                If yesfolder.Count < Me.uxPubtypes.Items.Count Then
                    n = npubtypes.AppendNode(New Object() {"General"}, tln)
                    n.StateImageIndex = 0
                    n.Tag = 0
                    For j = 0 To uxPubtypes.Items.Count - 1
                        For k = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                            If bc_am_load_objects.obc_pub_types.pubtype(k).name = uxPubtypes.Items(j) AndAlso bc_am_load_objects.obc_pub_types.pubtype(k).folder_name = "" Then
                                n.Nodes.Add(New Object() {bc_am_load_objects.obc_pub_types.pubtype(k).name})
                                n.Nodes(n.Nodes.Count - 1).Tag = bc_am_load_objects.obc_pub_types.pubtype(k).id
                                n.Nodes(n.Nodes.Count - 1).StateImageIndex = 1
                                Exit For
                            End If
                        Next
                    Next
                End If
                Me.npubtypes.EndUpdate()



            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log(className, moduleName, bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

            slog = New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub


    Public Sub load_entities1(ByRef lcontrole As DevExpress.XtraEditors.ListBoxControl, ByVal entityClass As Integer, Optional ByVal currentEntityId As Long = 0, Optional ByVal UserEntitiesOnly As Boolean = False, Optional ByVal secondary_entity As Integer = 0)

        Dim moduleName As String = "load_entities"
        Dim slog As New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim entity_list As New List(Of bc_om_entity)
        Dim user_entity_list As New List(Of bc_om_entity)
        Me.Cursor = Windows.Forms.Cursors.WaitCursor


        Try
            lfiltername1.Text = ""

            If UserEntitiesOnly = True Then
                For i = 0 To wtaxonomies.Count - 1
                    If wtaxonomies(i).pclass = entityClass Then
                        gclass = wtaxonomies(i).pclass
                        entity_list = wtaxonomies(i).ppref_list
                        WizardEquityPage1.Text = wtaxonomies(i).class_name
                        WizardEquityPage1.DescriptionText = "Please select " + wtaxonomies(i).class_name
                        If wtaxonomies(i).filter_attribute_id > 0 Then
                            lfiltername1.Text = "filtered by: " + wtaxonomies(i).filter_attribute_name
                        End If


                        Exit For
                    End If
                Next
            Else
                For i = 0 To wtaxonomies.Count - 1
                    If wtaxonomies(i).pclass = entityClass Then
                        WizardEquityPage1.Text = wtaxonomies(i).class_name
                        WizardEquityPage1.DescriptionText = "Please select " + wtaxonomies(i).class_name
                        entity_list = wtaxonomies(i).pall_list
                        If wtaxonomies(i).filter_attribute_id > 0 Then
                            lfiltername1.Text = "filtered by: " + wtaxonomies(i).filter_attribute_name
                        End If
                        Exit For
                    End If
                Next
            End If

            display_data_to_control(lcontrole, entity_list, entityClass, currentEntityId)


        Catch ex As Exception
            Dim db_err As New bc_cs_error_log(className, moduleName, bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.Cursor = Windows.Forms.Cursors.Default

            slog = New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    Public Sub load_entities2(ByRef lcontrole As DevExpress.XtraEditors.ListBoxControl, ByVal entityClass As Integer, Optional ByVal currentEntityId As Long = 0, Optional ByVal UserEntitiesOnly As Boolean = False, Optional ByVal secondary_entity As Integer = 0)

        Dim moduleName As String = "load_entities"
        Dim slog As New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim entity_list As New List(Of bc_om_entity)
        Dim user_entity_list As New List(Of bc_om_entity)
        Me.Cursor = Windows.Forms.Cursors.WaitCursor


        Try
            lfiltername2.Text = ""
            If UserEntitiesOnly = True Then
                For i = 0 To wtaxonomies.Count - 1
                    If wtaxonomies(i).pclass = entityClass Then
                        gclass = wtaxonomies(i).pclass
                        entity_list = wtaxonomies(i).ppref_list
                        WizardEquityPage2.Text = wtaxonomies(i).class_name
                        WizardEquityPage2.DescriptionText = "Please select " + wtaxonomies(i).class_name
                        If wtaxonomies(i).filter_attribute_id > 0 Then
                            lfiltername2.Text = "filtered by: " + wtaxonomies(i).filter_attribute_name
                        End If
                        Exit For
                    End If
                Next
            Else
                For i = 0 To wtaxonomies.Count - 1
                    If wtaxonomies(i).pclass = entityClass Then
                        WizardEquityPage2.Text = wtaxonomies(i).class_name
                        WizardEquityPage2.DescriptionText = "Please select " + wtaxonomies(i).class_name
                        entity_list = wtaxonomies(i).pall_list
                        If wtaxonomies(i).filter_attribute_id > 0 Then
                            lfiltername2.Text = "filtered by: " + wtaxonomies(i).filter_attribute_name
                        End If
                        Exit For
                    End If
                Next
            End If

            display_data_to_control(lcontrole, entity_list, entityClass, currentEntityId)


        Catch ex As Exception
            Dim db_err As New bc_cs_error_log(className, moduleName, bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.Cursor = Windows.Forms.Cursors.Default

            slog = New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Private Sub display_data_to_control(ByRef lcontrole As DevExpress.XtraEditors.ListBoxControl, ByVal entity_list As List(Of bc_om_entity), ByVal entityClass As Integer, Optional ByVal currentEntityId As Long = 0)

        Dim listitem As Long
        lcontrole.BeginUpdate()

        REM add items to the control
        If Not IsNothing(entity_list) Then
            lcontrole.Items.Clear()
            listitem = 0

            For i = 0 To entity_list.Count - 1
                lcontrole.Items.Add(entity_list(i).name)
            Next

            If entity_list.Count > 0 Then
                WizardControl1.Controls.Item(2).Enabled = True
            End If
        End If
        lcontrole.EndUpdate()
    End Sub

    REM XXX
    Private Sub npubtypes_FocusedNodeChanged(sender As Object, e As FocusedNodeChangedEventArgs) Handles npubtypes.FocusedNodeChanged
        Try
            Dim selectedvalue As String

            pubTypeId = 0
            selectedvalue = e.Node.Tag

            Me.chkclone.Visible = False
            Me.chkclone.Visible = False
            Me.WizardControl1.Controls.Item(2).Enabled = False
            Me.WizardControl1.Controls.Item(1).Enabled = False
            If selectedvalue = 0 Then
                Exit Sub
            End If

            For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                If selectedvalue = bc_am_load_objects.obc_pub_types.pubtype(i).id Then
                    pubTypeId = bc_am_load_objects.obc_pub_types.pubtype(i).id

                    is_composite = bc_am_load_objects.obc_pub_types.pubtype(i).composite
                    prev_child_category = bc_am_load_objects.obc_pub_types.pubtype(i).child_category
                    sub_entity_class = bc_am_load_objects.obc_pub_types.pubtype(i).sub_entity_class
                    Exit For
                End If
            Next

            uxSelectedpubtype.Text = e.Node.GetValue(0)

            'Me.chkclone.Visible = False
            Me.chkclone.CheckState = Windows.Forms.CheckState.Unchecked

            If (npubtypes.Visible = True) Then

                For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                    If bc_am_load_objects.obc_pub_types.pubtype(i).name = npubtypes.FocusedNode.GetValue(0) AndAlso bc_am_load_objects.obc_pub_types.pubtype(i).is_clonable = True Then
                        Me.chkclone.Visible = True
                    End If
                Next
            Else

                For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                    If bc_am_load_objects.obc_pub_types.pubtype(i).name = uxSelectedpubtype.Text AndAlso bc_am_load_objects.obc_pub_types.pubtype(i).is_clonable = True Then
                        Me.chkclone.Visible = True
                    End If
                Next
            End If
            'WizardControl1.Controls.Item(2).Enabled = False
            If prev_child_category = 0 Then
                WizardControl1.Controls.Item(1).Visible = False REM next
                WizardControl1.Controls.Item(2).Visible = True REM Finnish
                WizardControl1.Controls.Item(2).Enabled = True

            Else
                WizardControl1.Controls.Item(1).Visible = True REM next
                WizardControl1.Controls.Item(1).Enabled = True
                WizardControl1.Controls.Item(2).Visible = False REM Finnish
            End If
        Catch ex As Exception

        Finally

        End Try

    End Sub

    Private Sub uxPubtypes_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxPubtypes.SelectedIndexChanged

        getPublicationDetails()

        uxSelectedpubtype.Text = uxPubtypes.SelectedValue
        Me.chkclone.Visible = False
        Me.chkclone.CheckState = Windows.Forms.CheckState.Unchecked

        For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
            If bc_am_load_objects.obc_pub_types.pubtype(i).name = uxSelectedpubtype.Text AndAlso bc_am_load_objects.obc_pub_types.pubtype(i).is_clonable = True Then
                Me.chkclone.Visible = True
            End If
        Next
        WizardControl1.Controls.Item(2).Enabled = False
        If prev_child_category = 0 Then
            WizardControl1.Controls.Item(1).Visible = False REM next
            WizardControl1.Controls.Item(2).Visible = True REM Finnish
            WizardControl1.Controls.Item(2).Enabled = True

        Else
            WizardControl1.Controls.Item(1).Visible = True REM next
            WizardControl1.Controls.Item(2).Visible = False REM Finnish
        End If

    End Sub

    Private Sub getPublicationDetails()

        Dim selectedvalue As String

        pubTypeId = 0
        selectedvalue = uxPubtypes.SelectedValue

        For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
            If selectedvalue = bc_am_load_objects.obc_pub_types.pubtype(i).name().ToString() Then
                pubTypeId = bc_am_load_objects.obc_pub_types.pubtype(i).id
                is_composite = bc_am_load_objects.obc_pub_types.pubtype(i).composite
                prev_child_category = bc_am_load_objects.obc_pub_types.pubtype(i).child_category
                sub_entity_class = bc_am_load_objects.obc_pub_types.pubtype(i).sub_entity_class
                Exit For
            End If
        Next
    End Sub

    Private Sub getEntityDetails(ByRef lcontrole As DevExpress.XtraEditors.ListBoxControl, ByVal entityClass As Integer, ByRef SEntitiyId As Long)

        Dim selectedvalue As String

        SEntitiyId = 0
        selectedvalue = lcontrole.SelectedValue
        For i = 0 To bc_am_load_objects.obc_entities.entity.Count - 1

            If bc_am_load_objects.obc_entities.entity(i).class_id = entityClass Then
                If selectedvalue = bc_am_load_objects.obc_entities.entity(i).name Then
                    SEntitiyId = bc_am_load_objects.obc_entities.entity(i).id
                    Exit For
                End If
            End If
        Next
    End Sub

    Private Sub uxEntity1_KeyDown(sender As Object, e As Windows.Forms.KeyEventArgs) Handles uxEntity1.KeyDown
        If listboxtimer.Enabled = False Then
            Me.uxEntity1.IncrementalSearch = False
            Me.uxEntity1.IncrementalSearch = True
        End If
    End Sub

    Private Sub uxEntity1_KeyUp(sender As Object, e As Windows.Forms.KeyEventArgs) Handles uxEntity1.KeyUp
        listboxtimer.Start()
    End Sub

    Private Sub uxEntity1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxEntity1.SelectedIndexChanged
        If uxEntity1.SelectedIndex >= 0 Then
            WizardControl1.Controls.Item(2).Enabled = True
        End If
        getEntityDetails(uxEntity1, prev_child_category, selectedEntity1Id)


        'If sub_entity_class = 0 Then
        '    WizardControl1.Controls.Item(1).Visible = False REM next
        '    WizardControl1.Controls.Item(2).Visible = True REM Finnish
        'Else
        '    WizardControl1.Controls.Item(1).Visible = True REM next
        '    WizardControl1.Controls.Item(2).Visible = False REM Finnish
        'End If

    End Sub

    Private Sub uxEntity2_KeyDown(sender As Object, e As Windows.Forms.KeyEventArgs) Handles uxEntity2.KeyDown
        If listboxtimer.Enabled = False Then
            Me.uxEntity2.IncrementalSearch = False
            Me.uxEntity2.IncrementalSearch = True
        End If
    End Sub

    Private Sub uxEntity2_KeyUp(sender As Object, e As Windows.Forms.KeyEventArgs) Handles uxEntity2.KeyUp
        listboxtimer.Start()
    End Sub


    Private Sub uxEntity2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxEntity2.SelectedIndexChanged
        If uxEntity2.SelectedIndex >= 0 Then
            WizardControl1.Controls.Item(2).Enabled = True
        End If
        getEntityDetails(uxEntity2, sub_entity_class, selectedEntity2Id)

    End Sub

    Private Sub uxTnToggle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxTnToggle.Click
        Me.tsearch.Text = ""
        If SearchUserEntitiesOnly = False Then
            SearchUserEntitiesOnly = True
            uxTnToggle.Text = "All Subjects"
        Else
            SearchUserEntitiesOnly = False
            uxTnToggle.Text = "My Subjects"
        End If
        guseronly = SearchUserEntitiesOnly
        selectedEntity1Id = 0
        load_entities1(uxEntity1, prev_child_category, selectedEntity1Id, SearchUserEntitiesOnly)
        uxEntity1.Select()

    End Sub


    Private Sub uxTnToggle2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxTnToggle2.Click
        Me.tsearch2.Text = ""

        If SearchUserEntitiesOnly2 = False Then
            SearchUserEntitiesOnly2 = True
            uxTnToggle2.Text = "All Subjects"
        Else
            SearchUserEntitiesOnly2 = False
            uxTnToggle2.Text = "My Subjects"
        End If
        guseronly = SearchUserEntitiesOnly2
        selectedEntity2Id = 0
        load_entities2(uxEntity2, sub_entity_class, selectedEntity2Id, SearchUserEntitiesOnly2)
        uxEntity2.Select()

    End Sub

    Private Sub tsearch_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsearch.EditValueChanged
        searchtimer.Stop()
        searchtimer.Start()
    End Sub

    Private Sub searchtimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles searchtimer.Tick
        searchtimer.Stop()
        runsearch1(tsearch.Text, uxEntity1, selectedEntity1Id)
    End Sub

    Private Sub runsearch1(ByVal tx As String, ByRef lcontrole As DevExpress.XtraEditors.ListBoxControl, Optional ByVal currentEntityId As Long = 0)

        Dim moduleName As String = " runsearch"
        Dim slog As New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            Me.Cursor = Windows.Forms.Cursors.WaitCursor

            Dim i, j As Long
            If tx = "" Then
                load_entities1(lcontrole, gclass, 0, guseronly, 0)
                Exit Sub
            End If

            Dim found_entities As New List(Of Long)
            Dim display_entities As New bc_om_entities
            Dim entity_list As New List(Of bc_om_entity)


            If tx <> "" Then
                REM real time extended search
                Dim search_results As New bc_om_real_time_search
                search_results.class_id = gclass
                search_results.search_text = tx
                search_results.mine = guseronly
                search_results.inactive = False
                search_results.filter_attribute_id = 0
                search_results.results_as_ids = True


                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    search_results.db_read()
                ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                    search_results.tmode = bc_cs_soap_base_class.tREAD
                    search_results.transmit_to_server_and_receive(search_results, False)
                End If
                For i = 0 To search_results.resultsids.Count - 1
                    found_entities.Add(search_results.resultsids(i))
                Next

                REM end real time extended search

                'For i = 0 To bc_am_load_objects.obc_entities.search_attributes.search_values.Count - 1
                '    If InStr(UCase(bc_am_load_objects.obc_entities.search_attributes.search_values(i).value), UCase(tx)) > 0 Then
                '        found_entities.Add(bc_am_load_objects.obc_entities.search_attributes.search_values(i).entity_id)
                '    End If
                'Next
            End If
            lcontrole.BeginUpdate()
            lcontrole.Items.Clear()
            If guseronly = True Then
                For i = 0 To wtaxonomies.Count - 1
                    If wtaxonomies(i).pclass = gclass Then
                        entity_list = wtaxonomies(i).ppref_list
                        Exit For
                    End If
                Next
            Else
                For i = 0 To wtaxonomies.Count - 1
                    If wtaxonomies(i).pclass = gclass Then
                        entity_list = wtaxonomies(i).pall_list
                        Exit For
                    End If
                Next
            End If


            Dim found As Boolean = False
            For i = 0 To entity_list.Count - 1
                found = False
                If InStr(UCase(entity_list(i).name), UCase(tx)) > 0 Then
                    found = True
                End If
                If found = False Then
                    For j = 0 To found_entities.Count - 1
                        If entity_list(i).id = found_entities(j) Then
                            found = True
                            Exit For
                        End If
                    Next
                End If
                If found = True Then
                    lcontrole.Items.Add(entity_list(i).name)
                End If
            Next

            lcontrole.EndUpdate()

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log(className, moduleName, bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.Cursor = Windows.Forms.Cursors.Default
        End Try
    End Sub
    Private Sub runsearch2(ByVal tx As String, ByRef lcontrole As DevExpress.XtraEditors.ListBoxControl, Optional ByVal currentEntityId As Long = 0)

        Dim moduleName As String = " runsearch"
        Dim slog As New bc_cs_activity_log(className, moduleName, bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            Me.Cursor = Windows.Forms.Cursors.WaitCursor

            Dim i, j As Long
            If tx = "" Then
                load_entities2(lcontrole, gclass, 0, guseronly, 1)
                Exit Sub
            End If

            Dim found_entities As New List(Of Long)
            Dim display_entities As New bc_om_entities
            Dim entity_list As New List(Of bc_om_entity)


            If tx <> "" Then
                REM real time extended search
                Dim search_results As New bc_om_real_time_search
                search_results.class_id = gclass
                search_results.search_text = tx
                search_results.mine = guseronly
                search_results.inactive = False
                search_results.filter_attribute_id = 0
                search_results.results_as_ids = True
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    search_results.db_read()
                ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                    search_results.tmode = bc_cs_soap_base_class.tREAD
                    search_results.transmit_to_server_and_receive(search_results, False)
                End If
                For i = 0 To search_results.resultsids.Count - 1
                    found_entities.Add(search_results.resultsids(i))
                Next

                REM end real time extended search
                'For i = 0 To bc_am_load_objects.obc_entities.search_attributes.search_values.Count - 1
                '    If InStr(UCase(bc_am_load_objects.obc_entities.search_attributes.search_values(i).value), UCase(tx)) > 0 Then
                '        found_entities.Add(bc_am_load_objects.obc_entities.search_attributes.search_values(i).entity_id)
                '    End If
                'Next
            End If
            lcontrole.BeginUpdate()
            lcontrole.Items.Clear()
            If guseronly = True Then
                For i = 0 To wtaxonomies.Count - 1
                    If wtaxonomies(i).pclass = gclass Then
                        entity_list = wtaxonomies(i).ppref_list
                        Exit For
                    End If
                Next
            Else
                For i = 0 To wtaxonomies.Count - 1
                    If wtaxonomies(i).pclass = gclass Then
                        entity_list = wtaxonomies(i).pall_list
                        Exit For
                    End If
                Next
            End If


            Dim found As Boolean = False
            For i = 0 To entity_list.Count - 1
                found = False
                If InStr(UCase(entity_list(i).name), UCase(tx)) > 0 Then
                    found = True
                End If
                If found = False Then
                    For j = 0 To found_entities.Count - 1
                        If entity_list(i).id = found_entities(j) Then
                            found = True
                            Exit For
                        End If
                    Next
                End If
                If found = True Then
                    lcontrole.Items.Add(entity_list(i).name)
                End If
            Next

            lcontrole.EndUpdate()

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log(className, moduleName, bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.Cursor = Windows.Forms.Cursors.Default
        End Try
    End Sub
    'Public Sub buildSearchList(ByVal searchList As List(Of bc_as_cat_taxononmy_item), ByVal source_entity_list As List(Of bc_om_entity), ByVal classid As Long)
    '    Dim item As bc_as_cat_taxononmy_item = Nothing
    '    Dim found As Boolean = False

    '    REM load user taxonomy
    '    searchList.Clear()
    '    For i = 0 To source_entity_list.entity.Count - 1
    '        If source_entity_list.entity.Item(i).class_id = classid Then
    '            item = New bc_as_cat_taxononmy_item(source_entity_list.entity.Item(i).name, source_entity_list.entity.Item(i).id, False)
    '            searchList.Add(item)
    '        End If
    '    Next

    'End Sub

    Private Sub tsearch2_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsearch2.EditValueChanged
        searchtimer2.Stop()
        searchtimer2.Start()
    End Sub

    Private Sub WizardControl1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WizardControl1.Click

    End Sub

    Private Sub searchtimer2_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles searchtimer2.Tick
        searchtimer2.Stop()
        runsearch2(tsearch2.Text, uxEntity2, selectedEntity2Id)
    End Sub

    Private Sub WizardEquityPage1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WizardEquityPage1.Click

    End Sub

    Private Sub WizardControl1_FinishClick(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles WizardControl1.FinishClick

        doBuildDocument(pubTypeId, selectedEntity1Id, selectedEntity2Id, e)

        REM()
    End Sub

    Public Sub doBuildDocument(ByVal pubType As Long, ByVal entity1 As Long, ByVal entity2 As Long, ByVal e As System.ComponentModel.CancelEventArgs)

        REM ------------------
        REM Go make a document
        REM ------------------

        sel_pub_type_Id = pubType
        sel_entity_Id1 = entity1
        sel_entity_id2 = entity2

        REM check if a composite
        is_composite = False
        For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
            If bc_am_load_objects.obc_pub_types.pubtype(i).id = sel_pub_type_Id AndAlso bc_am_load_objects.obc_pub_types.pubtype(i).composite = True Then
                is_composite = True
                Exit For
            End If
        Next
        is_clone = False
        If Me.chkclone.Visible = True AndAlso Me.chkclone.CheckState = Windows.Forms.CheckState.Checked Then
            is_clone = True
        End If

        If gfrom_desktop = True Then
            If is_clone = True Then
                Dim bic As New bc_am_clone_document
                Me.WindowState = Windows.Forms.FormWindowState.Minimized
                If bic.invoke_clone(sel_pub_type_Id, sel_entity_Id1, sel_entity_id2) = False Then
                    Me.chkclone.CheckState = Windows.Forms.CheckState.Unchecked
                    Me.WizardControl1.SelectedPage = WelcomeWizardPage1
                    Me.WindowState = Windows.Forms.FormWindowState.Normal
                Else
                    Me.WizardControl1.SelectedPage = WelcomeWizardPage1
                    Me.chkclone.CheckState = Windows.Forms.CheckState.Unchecked
                    Me.chkclone.Visible = False
                End If
            ElseIf is_composite = True Then
                Dim vcb As New bc_dx_adv_composite_build
                Dim ccb As New Cbc_dx_adv_composite_build(vcb)
                Me.WindowState = Windows.Forms.FormWindowState.Minimized
                If ccb.load_data(sel_pub_type_Id, sel_entity_Id1) = False Then
                    Me.WizardControl1.SelectedPage = WelcomeWizardPage1
                    Me.WindowState = Windows.Forms.FormWindowState.Normal
                Else
                    vcb.ShowDialog()
                    Me.WizardControl1.SelectedPage = WelcomeWizardPage1
                    If ccb.build_selected = False Then
                        Me.WindowState = Windows.Forms.FormWindowState.Normal
                    End If
                End If
            Else
                bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve create", "Initializing Build...", 20, True, True)

                Dim oproductselection As New bc_am_product_selection
                Me.WindowState = Windows.Forms.FormWindowState.Minimized
                Me.WizardControl1.SelectedPage = WelcomeWizardPage1
                If oproductselection.launch_product(sel_pub_type_Id, sel_entity_Id1, Nothing, sel_entity_id2) = bc_cs_error_codes.RETURN_ERROR Then
                    Me.WindowState = Windows.Forms.FormWindowState.Normal
                End If
            End If
        Else
            Me.Hide()
        End If

        e.Cancel = True


    End Sub


    Private Sub listboxtimer_Tick(sender As Object, e As EventArgs) Handles listboxtimer.Tick
        listboxtimer.Stop()
    End Sub

   
End Class

Public Class bc_as_cat_taxononmy_item
    Public display_name As String
    Public id As Long
    Public selected As Boolean
    Public searched As Boolean
    Public search_names As New List(Of String)
    Public search_name As String
    Public Sub New(ByVal display_name As String, ByVal id As Long, ByVal selected As Boolean)
        Me.display_name = display_name
        Me.id = id
        Me.selected = selected
    End Sub
End Class
