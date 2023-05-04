Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports BlueCurve.Core.AS
Imports System.IO
Imports System.Windows.Forms
Imports System.Threading

Public Class bc_am_user_defined_components
    Public loading As Boolean = False
    Public ocomps As New bc_om_user_defined_components
    Public mode As Integer
    Public ao_object As bc_ao_at_object
    Public ldoc As New bc_om_document
    Public fnname As String
    Public user_name As String
    Public activedocument As Object
    Public selected_entity_id As Long
    Public obj_type As String
    Public show_form As Boolean = False
    Public Function load_data() As Boolean
        If mode = 1 Then
            load_data = set_insert_mode()
        Else
            load_data = set_preview()
        End If
    End Function
    Private Function set_insert_mode() As Boolean
        loading = True
        set_insert_mode = False
        If ao_object.check_for_sdc Then
            Dim omsg As New bc_cs_message("Blue Curve", "Selection contains a System Defined Component please move your insertion point", bc_cs_message.MESSAGE)
            Exit Function
        End If
        If ao_object.check_for_udc Then
            Dim omsg As New bc_cs_message("Blue Curve", "Selection already contains an exisiting User Defiend Component please move your insertion point", bc_cs_message.MESSAGE)
            Exit Function
        End If

        Me.Text = "Insert Component Into Document - Blue Curve"
        Dim tp As TimeSpan
        tp = New TimeSpan(28, 0, 0, 0, 0)
        Dim da As DateTime
        da = Now
        Me.Dfrom.Value = da.Subtract(tp)
        Me.Dto.Value = Now
        Me.Ltitle.Visible = False
        Me.Ttitle.Visible = False
        Me.breload.Text = "Delete"
        Me.breload.Visible = False
        Me.breload.Enabled = False
        Me.TabControl1.TabPages(1).Enabled = False
        Me.rprivate.Visible = True
        Me.rpublic.Visible = True
        Me.bsave.Enabled = False
        Me.bsave.Text = "Insert"
        Me.budcall.Checked = True
        loading = False
        load_comps(True)
        set_default_classify()
        set_insert_mode = True
    End Function
    Private Sub set_default_classify()
        REM set filters to pub type author and entity of document
        For i = 0 To Me.Cpubtype.Items.Count - 1
            If Me.Cpubtype.Items(i) = ldoc.pub_type_name Then
                Me.Cpubtype.Text = ldoc.pub_type_name
                Exit For
            End If
        Next
        For i = 0 To Me.Cauthor.Items.Count - 1
            If Me.Cauthor.Items(i) = Me.user_name Then
                Me.Cauthor.Text = Me.user_name
                Exit For
            End If
            If Me.Cauthor.Items(i) = "me" Then
                Me.Cauthor.Text = "me"
                Exit For
            End If
        Next
        For j = 0 To bc_am_load_objects.obc_entities.entity.Count - 1
            If bc_am_load_objects.obc_entities.entity(j).id = ldoc.entity_id Then
                For i = 0 To Me.Centity.Items.Count - 1
                    If Me.Centity.Items(i) = bc_am_load_objects.obc_entities.entity(j).name Then
                        Me.Centity.Text = bc_am_load_objects.obc_entities.entity(j).name
                        Exit For
                    End If
                Next
                Exit For
            End If

        Next
    End Sub
    
    Private Sub load_comps(ByVal from_db As Boolean)
        Me.Cursor = Cursors.WaitCursor
        Try
            Me.breload.Enabled = False
            Me.TabControl1.TabPages(1).Enabled = False
            Me.bsave.Enabled = False
            Me.tpreview.Rtf = ""
            Me.TabControl1.TabPages(1).Text = "Preview"
            If loading = True Then
                Exit Sub
            End If
            ocomps.pub_type_id = bc_am_load_objects.obc_current_document.pub_type_id
            ocomps.date_from = Me.Dfrom.Value
            ocomps.date_to = Me.Dto.Value
            ocomps.user_id = bc_cs_central_settings.logged_on_user_id
            Me.Lcomps.Items.Clear()
            If from_db = True Then
                If bc_cs_central_settings.connection_method = bc_cs_central_settings.ADO Then
                    ocomps.db_read()
                Else
                    ocomps.tmode = bc_cs_soap_base_class.tREAD
                    If ocomps.transmit_to_server_and_receive(ocomps, True) = False Then
                        Exit Sub
                    End If
                End If
                load_dynamic_lists()
            End If
            Dim lvw As ListViewItem

            For i = 0 To ocomps.udcs.Count - 1
                If ((Me.Cpubtype.Text = "All" Or Me.Cpubtype.Text = ocomps.udcs(i).pub_type) And (Me.Cauthor.Text = "All" Or Me.Cauthor.Text = ocomps.udcs(i).author) And (Me.Centity.Text = "All" Or Me.Centity.Text = ocomps.udcs(i).entity) And (Me.budcall.Checked = True Or (Me.rpublic.Checked = True And ocomps.udcs(i).public_flag = True) Or (Me.rprivate.Checked = True And ocomps.udcs(i).public_flag = False)) And (ocomps.udcs(i).sdc = 0 And Me.rsystem.Checked = False)) Or (ocomps.udcs(i).sdc > 0 And Me.Ruser.Checked = False) Then

                    lvw = New ListViewItem(CStr(ocomps.udcs(i).title))
                    If ocomps.udcs(i).sdc > 0 Then
                        Select Case ocomps.udcs(i).sdc
                            Case 1
                                lvw.SubItems.Add("System Text")
                            Case 2
                                lvw.SubItems.Add("System Table")
                            Case 3
                                lvw.SubItems.Add("System Chart")
                            Case 4, 5, 12
                                lvw.SubItems.Add("System Image")
                            Case 6
                                lvw.SubItems.Add("System Index")
                            Case 7
                                lvw.SubItems.Add("System File")
                            Case 8
                                lvw.SubItems.Add("System Edit Rtf")
                            Case 9
                                lvw.SubItems.Add("System Edit Text")
                            Case Else
                                lvw.SubItems.Add("System")
                        End Select
                        lvw.SubItems.Add("-")
                        lvw.SubItems.Add("-")
                    Else
                        If ocomps.udcs(i).author_id = bc_cs_central_settings.logged_on_user_id Then
                            lvw.SubItems.Add("User Edit")
                        Else
                            lvw.SubItems.Add("User Read")
                        End If
                        lvw.SubItems.Add(CStr(Format(ocomps.udcs(i).comp_date, "dd-MMM-yyyy HH:mm:ss")))
                        If ocomps.udcs(i).author_id = bc_cs_central_settings.logged_on_user_id Then
                            lvw.SubItems.Add("me")
                        Else
                            lvw.SubItems.Add(ocomps.udcs(i).author)
                        End If
                    End If
                    lvw.SubItems.Add(ocomps.udcs(i).pub_type)
                    lvw.SubItems.Add(ocomps.udcs(i).entity)
                    lvw.SubItems.Add(ocomps.udcs(i).udc_id)
                    Me.Lcomps.Items.Add(lvw)
                End If
            Next
            Me.Pselitem.Enabled = False
            load_entity_sel_list()

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_user_defined_components", "load_comps", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub
    Private Sub load_entity_sel_list()

        Dim prev_class_name As String = ""
        Dim selclass As String = ""
        Me.Cclasssel.Items.Clear()
        REM default to class id of document
        Me.Cclasssel.Items.Add("none")
        For i = 0 To bc_am_load_objects.obc_entities.entity.Count - 1
            With bc_am_load_objects.obc_entities.entity(i)
                If .class_name <> prev_class_name Then
                    Me.Cclasssel.Items.Add(.class_name)
                End If
                If ldoc.entity_id = .id Then
                    selclass = .class_name
                End If
                prev_class_name = .class_name
            End With
        Next
        If selclass = "" Or ldoc.entity_id = 0 Then
            REM set class relevant to component


            Me.Cclasssel.Text = "none"
        Else
            Me.Cclasssel.Text = selclass
        End If
    End Sub
    Private Function set_preview() As Boolean
        Try
            Me.Pselitem.Visible = False
            Me.Psave.Visible = True

            set_preview = False
            REM check not in an SDC
            If ao_object.check_for_sdc Then
                Dim omsg As New bc_cs_message("Blue Curve", "Selection contains a System Defined Component please re-select excluding this", bc_cs_message.MESSAGE)
                Exit Function
            End If
            If ao_object.check_for_udc Then
                Dim omsg As New bc_cs_message("Blue Curve", "Selection already contains an exisiting User Defiend Component please re-select excluding this", bc_cs_message.MESSAGE)
                Exit Function
            End If
            Dim rtf As String
            ao_object.ready = False
            rtf = ao_object.get_selection_in_rtf()

            Me.bsave.Text = "Save"
            Me.TabControl1.TabPages(0).Text = "Preview New Component"

            Me.Text = "Create User Defined Component - Blue Curve"
            Me.tpreview.Rtf = ""
            Me.Ttitle.Text = ""
            If Me.TabControl1.TabPages.Count > 1 Then
                Me.TabControl1.TabPages.RemoveAt(0)
            End If

            REM firstly see if inside a user dfined component already
            Me.bsave.Text = "Save"
            Me.TabControl1.TabPages(0).Text = "Preview New Component"
            Dim tx As String = ""
            Dim selected_udi_id As Long = 0
            selected_udi_id = ao_object.udi_get_selected_udi_id
            If selected_udi_id > 0 Then
                Me.bsave.Text = "Update"
                Me.Text = "Update User Defined Component - Blue Curve"
                Me.TabControl1.TabPages(0).Text = "Preview Existing"
                REM get title and domain for this component from database

                tx = "get this from db for udc_id"
                'rtf = ao_object.get_selection_in_rtf()
                If rtf = "" Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Couldnt retrieve component from document!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Function
                End If
                REM if public and not author then disable private button
                Dim oudi_comp As New bc_om_user_defined_component
                oudi_comp.udc_id = selected_udi_id
                REM put in soap bit later
                If bc_cs_central_settings.connection_method = bc_cs_central_settings.ADO Then
                    oudi_comp.db_read()
                Else
                    oudi_comp.tmode = bc_cs_soap_base_class.tREAD
                    If oudi_comp.transmit_to_server_and_receive(oudi_comp, True) = False Then
                        Exit Function
                    End If
                End If
                REM check if component is tsill registered in database
                If oudi_comp.udc_id = 0 Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Component no longer in database do you wish to insert as new?", bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
                    If omsg.cancel_selected = True Then
                        Exit Function
                    Else
                        REM select the bookmark and remove
                        ao_object.udi_delete_bookmark("rnet_udc_" + CStr(selected_udi_id))
                        REM mark as new
                        selected_udi_id = 0
                        Me.bsave.Text = "Save"
                        Me.TabControl1.TabPages(0).Text = "Preview New Component"
                    End If
                Else
                    REM if component is public and not author cannot edit
                    If oudi_comp.author_id <> bc_cs_central_settings.logged_on_user_id Then
                        Dim omsg As New bc_cs_message("Blue Curve", "Cannot modify this component as you are not the author of it.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                        Exit Function
                    End If
                    tx = oudi_comp.title
                    Me.rprivate.Checked = True
                    If oudi_comp.public_flag = True Then
                        Me.rpublic.Checked = True
                    End If
                End If
            End If
            If selected_udi_id = 0 Then

                tx = ao_object.udi_get_first_paragraph
                If Asc(tx.Substring(tx.Length - 1, 1)) = 13 Then
                    tx = tx.Substring(0, tx.Length - 1)
                End If

                If rtf = "" Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Nothing selected in document!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Function
                End If
                Me.rprivate.Checked = True
            End If
            Me.tpreview.Rtf = rtf
            Me.Ttitle.Text = tx
            Me.TabControl1.TabPages(0).Enabled = True
            Me.breload.Enabled = True

            set_preview = True

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_user_defined_components", "set_preview", bc_cs_error_codes.USER_DEFINED, ex.Message)
            Me.Hide()
        End Try
    End Function

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Me.Hide()
    End Sub
    Private Sub breload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles breload.Click
        If Me.breload.Text = "Reload" Then
            set_preview()
        ElseIf Me.breload.Text = "Delete" Then
            Dim omsg As New bc_cs_message("Blue Curve", "Are you sure you wish to delete component: " + Me.Lcomps.SelectedItems(0).SubItems(1).Text + " from system?", bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
            If omsg.cancel_selected = True Then
                Exit Sub
            Else
                Dim oudc As New bc_om_user_defined_component
                oudc.udc_id = CLng(Me.Lcomps.SelectedItems(0).SubItems(5).Text)
                oudc.write_mode = bc_om_user_defined_component.DELETE
                If bc_cs_central_settings.connection_method = bc_cs_central_settings.ADO Then
                    oudc.db_write()
                Else
                    If oudc.transmit_to_server_and_receive(oudc, True) = False Then
                        Exit Sub
                    End If

                End If


                load_comps(True)
            End If
        End If
    End Sub

    Private Sub tpreview_TextChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub cwrap_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cwrap.CheckedChanged
        If Cwrap.Checked = True Then
            Me.tpreview.WordWrap = True
        Else
            Me.tpreview.WordWrap = False
        End If
    End Sub

    Private Sub Ttitle_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Ttitle.TextChanged
        If Trim(Ttitle.Text) <> "" Then
            Me.bsave.Enabled = True
        End If
    End Sub

    Private Sub bsave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bsave.Click
        If Me.TabControl1.TabPages.Count = 1 Then
            Dim udc_id As Long = 0
            If Me.bsave.Text = "Update" Then
                udc_id = ao_object.udi_get_selected_udi_id

            End If
            If save_user_component(udc_id) = True Then
                Me.Hide()
            Else
                Me.Ttitle.Select()
            End If
        Else

            insert_component()
        End If
    End Sub
    Private Function save_user_component(ByVal udc_id As Long) As Boolean
        Try
            REM get metata from document pub type, entity author
            save_user_component = False
            Dim ocomp As New bc_om_user_defined_component
            ocomp.udc_id = udc_id
            ocomp.title = Me.Ttitle.Text
            If ocomp.title.Length > 250 Then
                ocomp.title = ocomp.title.Substring(0, 250)
            End If
            ocomp.rtf = Me.tpreview.Rtf
            ocomp.public_flag = Me.Rspublic.Checked
            ocomp.pub_type_id = ldoc.pub_type_id
            ocomp.author_id = bc_cs_central_settings.logged_on_user_id
            ocomp.entity_id = ldoc.entity_id

            If Not IsNumeric(ldoc.id) Then
                ocomp.last_updated_from_doc_id = 0
            Else
                ocomp.last_updated_from_doc_id = ldoc.id
            End If
            If bc_cs_central_settings.connection_method = bc_cs_central_settings.ADO Then
                ocomp.db_write()
            Else
                If ocomp.transmit_to_server_and_receive(ocomp, True) = False Then
                    Exit Function
                End If

            End If
            If ocomp.duplicate_title = True Then
                Dim omsg As New bc_cs_message("Blue Curve", "Title: " + ocomp.title + " already exists please re-enter", bc_cs_message.MESSAGE)

                Exit Function
            End If
            set_udi(ocomp, True)
            save_user_component = True
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_user_defined_components", "save_user_component", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Function

    Private Function set_udi(ByVal ocomp As bc_om_user_defined_component, ByVal editable As Boolean, Optional ByVal new_sel As Boolean = False) As String
        Dim locator As String
        locator = ""

        Try
            REM save down and retrieve new id
            REM set id around selection
            locator = ao_object.udi_set_locator(ocomp.udc_id, ldoc.entity_id, new_sel)
            REM now add it to document refresh
            Dim orefresh As New bc_om_refresh_component
            orefresh.locator = locator
            orefresh.entity_id = ldoc.entity_id
            orefresh.name = ocomp.title
            If editable = True Then
                orefresh.locator_description = "User Edit"
            Else
                orefresh.locator_description = "User Read"
            End If

            REM 100 is a udc
            orefresh.mode = 100
            'orefresh.mode_param1 = bc_am_load_objects.obc_templates.component_types.component_types(i).sp_name
            'orefresh.mode_param2 = bc_am_load_objects.obc_templates.component_types.component_types(i).insert_object
            orefresh.contributor_id = 1
            orefresh.refresh_type = 1
            orefresh.type = ocomp.udc_id
            orefresh.original_markup_colour_index = 0

            REM udc has no parameters
            'For j = 0 To bc_am_load_objects.obc_templates.component_types.component_types(i).parameters.bc_om_component_parameters.count - 1
            'orefresh.parameters.component_template_parameters.Add(bc_am_load_objects.obc_templates.component_types.component_types(i).parameters.bc_om_component_parameters(j))
            'Next
            If new_sel = True Then
                orefresh.author_id = ocomp.author_id
                orefresh.last_update_date = ocomp.comp_date
            Else
                orefresh.author_id = bc_cs_central_settings.logged_on_user_id
                orefresh.last_update_date = Now
            End If
            orefresh.last_refresh_date = Now
            ldoc.refresh_components.refresh_components.Add(orefresh)
       
            If new_sel = True Then
                ao_object.set_selection_from_rtf(locator, ocomp.rtf, ldoc.refresh_components)
            End If
            Dim sdc_colour_index As Long = 0
            Dim udc_colour_index As Long = 0
            Dim idx As String
            idx = ao_object.get_property_value("xrnet_sdc_colour_idx")
            If IsNumeric(idx) Then
                sdc_colour_index = idx
            End If
            idx = ao_object.get_property_value("xrnet_udc_colour_idx")
            If IsNumeric(idx) Then
                udc_colour_index = idx
            End If
            Dim bk_colour As Long
            With ldoc.refresh_components
                If .show_sdcs = True And sdc_colour_index <> 0 And editable = False Then
                    For j = 0 To .refresh_components.Count - 1
                        If .refresh_components(j).locator = locator Then
                            bk_colour = ao_object.get_background_color_for_locator(.refresh_components(j).locator)
                            If bk_colour <> sdc_colour_index And .refresh_components(j).original_markup_colour_index = 0 Then
                                .refresh_components(j).original_markup_colour_index = bk_colour
                                ao_object.set_background_colour_for_locator(.refresh_components(j).locator, sdc_colour_index, .refresh_components(j).mode)
                                ao_object.update_refresh_status_bar("highlight component ", CStr(j + 1), CStr(.refresh_components.Count))
                            End If
                            Exit For
                        End If
                    Next
                End If
                If .show_udcs = True And udc_colour_index <> 0 And editable = True Then
                    For j = 0 To .refresh_components.Count - 1
                        If .refresh_components(j).locator = locator Then
                            bk_colour = ao_object.get_background_color_for_locator(.refresh_components(j).locator)
                            If bk_colour <> udc_colour_index And .refresh_components(j).original_markup_colour_index = 0 Then
                                .refresh_components(j).original_markup_colour_index = bk_colour
                                ao_object.set_background_colour_for_locator(.refresh_components(j).locator, udc_colour_index, .refresh_components(j).mode)
                                ao_object.update_refresh_status_bar("highlight component ", CStr(j + 1), CStr(.refresh_components.Count))
                            End If
                            Exit For
                        End If
                    Next
                End If
            End With


            ldoc.write_data_to_file(bc_cs_central_settings.local_repos_path + Me.fnname + ".dat")
            ao_object.save()
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_user_defined_components", "set_udi", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            set_udi = locator
        End Try
    End Function
    Private Sub insert_component()
        Try
            REM firstly check it doesnt alreay exist
            REM is it a udc or an sdc



            If InStr(Lcomps.SelectedItems(0).SubItems(1).Text, "System") > 0 Then
                Dim locator As String = ""
                For i = 0 To Me.ocomps.udcs.Count - 1
                    If Me.ocomps.udcs(i).udc_id = CInt(Me.Lcomps.SelectedItems(0).SubItems(6).Text) Then
                        For j = 0 To bc_am_load_objects.obc_templates.component_types.component_types.Count - 1
                            If Me.ocomps.udcs(i).udc_id = bc_am_load_objects.obc_templates.component_types.component_types(j).component_id Then



                                REM fa user edit componet check uniue for entity
                                If bc_am_load_objects.obc_templates.component_types.component_types(j).mode = 8 Or bc_am_load_objects.obc_templates.component_types.component_types(j).mode = 9 Then
                                    If ao_object.check_sdc_in_document(Me.ocomps.udcs(i).udc_id, bc_am_load_objects.obc_templates.component_types.component_types(j).component_id, selected_entity_id) = True Then
                                        Dim omsg As New bc_cs_message("Blue Curve", "System edit component already exists in document for selected entity cant insert again!", bc_cs_message.MESSAGE)
                                        Exit Sub
                                    End If
                                End If

                                locator = ao_object.set_locator_for_component(Me.ocomps.udcs(i).udc_id, selected_entity_id, Me.ocomps.udcs(i).sdc, False)

                                If locator = "" Then
                                    Exit Sub
                                End If
                                Dim orefresh As New bc_om_refresh_component
                                orefresh.locator = locator
                                orefresh.entity_id = selected_entity_id
                                orefresh.name = bc_am_load_objects.obc_templates.component_types.component_types(j).component_name
                                orefresh.locator_description = bc_am_load_objects.obc_templates.component_types.component_types(j).component_name
                                orefresh.mode = bc_am_load_objects.obc_templates.component_types.component_types(j).mode
                                orefresh.mode_param1 = bc_am_load_objects.obc_templates.component_types.component_types(j).sp_name
                                orefresh.mode_param2 = bc_am_load_objects.obc_templates.component_types.component_types(j).insert_object
                                orefresh.contributor_id = bc_am_load_objects.obc_templates.component_types.component_types(j).contributor
                                orefresh.refresh_type = bc_am_load_objects.obc_templates.component_types.component_types(j).refresh_type()
                                orefresh.type = Me.ocomps.udcs(i).udc_id
                                For k = 0 To bc_am_load_objects.obc_templates.component_types.component_types(j).parameters.bc_om_component_parameters.count - 1
                                    orefresh.parameters.component_template_parameters.Add(bc_am_load_objects.obc_templates.component_types.component_types(j).parameters.bc_om_component_parameters(k))
                                Next
                                ldoc.refresh_components.refresh_components.Add(orefresh)
                                ldoc.write_data_to_file(bc_cs_central_settings.local_repos_path + Me.fnname + ".dat")
                                ao_object.save()


                                If obj_type = bc_ao_at_object.POWERPOINT_DOC Then
                                    Dim ocomprefresh As New bc_am_at_change_component_params(me.show_form, activedocument, "powerpoint")
                                Else
                                    Dim ocomprefresh As New bc_am_at_change_component_params(Me.show_form, activedocument, "word")
                                End If
                            End If
                        Next
                Exit For
            End If
                Next
            Else
            REM check it doesnt alreay exists TBD
            Dim bm As String
            For i = 0 To Me.ocomps.udcs.Count - 1
                If Me.ocomps.udcs(i).udc_id = CInt(Me.Lcomps.SelectedItems(0).SubItems(6).Text) Then
                    If ao_object.check_udc_in_document(Me.ocomps.udcs(i).udc_id) = True Then
                        Dim omsg As New bc_cs_message("Blue Curve", "User component already exists in document!", bc_cs_message.MESSAGE)
                        Exit Sub
                    End If
                    If Me.ocomps.udcs(i).author_id = bc_cs_central_settings.logged_on_user_id Then
                        bm = Me.set_udi(Me.ocomps.udcs(i), True, True)
                    Else
                        bm = Me.set_udi(Me.ocomps.udcs(i), False, True)
                    End If
                    If bm = "" Then
                        Dim omsg As New bc_cs_message("Blue Curve", "Failed to set component in documemt", bc_cs_message.MESSAGE)
                        Exit Sub
                    End If
                    Exit For
                End If
            Next
            End If

            Me.Hide()

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_user_defined_components", "insert_component", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

        End Try
    End Sub







    Private Sub TabControl1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabControl1.SelectedIndexChanged
        If Me.TabControl1.SelectedIndex = 1 Then
            Me.rprivate.Visible = False
            Me.rpublic.Visible = False
        Else
            Me.rprivate.Visible = True
            Me.rpublic.Visible = True
        End If
    End Sub

    Private Sub Panel2_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Panel2.Paint

    End Sub

    Private Sub Ttitle_TextChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Ttitle.TextChanged

    End Sub

    Private Sub RadioButton1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rprivate.CheckedChanged
        If Me.TabControl1.TabPages.Count > 1 Then
            If Me.rprivate.Checked = True Then
                load_comps(False)
            End If
        End If
    End Sub

    Private Sub Lcomps_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Lcomps.DoubleClick
        If Lcomps.SelectedItems.Count = 1 Then
             If InStr(Lcomps.SelectedItems(0).SubItems(1).Text, "System") > 0 Then
                Exit Sub
            End If
            Me.bsave.Enabled = True
            show_preview()
            Me.TabControl1.SelectedTab = Me.TabControl1.TabPages(1)

        End If
    End Sub
    Private Sub clear_preview()
        Me.TabControl1.TabPages(1).Enabled = False
        Me.bsave.Enabled = False
        Me.tpreview.Rtf = ""
        Me.TabControl1.TabPages(1).Text = "Preview"
        Me.TabControl1.SelectedTab = Me.TabControl1.TabPages(0)
    End Sub
    Private Sub Lcomps_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Lcomps.SelectedIndexChanged
        If Lcomps.SelectedItems.Count = 0 Then
            Exit Sub
        End If
        clear_preview()
        Me.Pselitem.Enabled = False

        If InStr(Lcomps.SelectedItems(0).SubItems(1).Text, "System") > 0 Then
            Me.TabControl1.TabPages(1).Enabled = False
            REM need to enable entity bit at some point
            Me.Pselitem.Enabled = True
            Me.bsave.Enabled = True
            REM if no entity based oroduct set class to class component
            If ldoc.entity_id = 0 Then
                REM if component is private or public and owned by user then enable delete
                For i = 0 To ocomps.udcs.Count - 1
                    If ocomps.udcs(i).udc_id = CLng(Me.Lcomps.SelectedItems(0).SubItems(6).Text) Then
                        For j = 0 To bc_am_load_objects.obc_entities.entity.Count - 1
                            With bc_am_load_objects.obc_entities.entity(j)
                                If .class_id = ocomps.udcs(i).for_class Then
                                    Me.Cclasssel.Text = .class_name
                                    Exit For
                                End If
                            End With
                        Next

                        Exit For
                    End If
                Next

            End If

            Exit Sub
        End If
        Me.TabControl1.TabPages(1).Enabled = True
        Me.breload.Enabled = False
        If Lcomps.SelectedItems.Count = 1 Then
            REM if component is private or public and owned by user then enable delete
            For i = 0 To ocomps.udcs.Count - 1
                If ocomps.udcs(i).udc_id = CLng(Me.Lcomps.SelectedItems(0).SubItems(6).Text) Then
                    If ocomps.udcs(i).author_id = bc_cs_central_settings.logged_on_user_id Then
                        Me.breload.Enabled = True
                    End If
                    Exit For
                End If
            Next
            Me.bsave.Enabled = True
            show_preview()
        End If
    End Sub
    Private Sub show_preview()
        For i = 0 To ocomps.udcs.Count - 1
            If ocomps.udcs(i).udc_id = CLng(Me.Lcomps.SelectedItems(0).SubItems(6).Text) Then
                Me.tpreview.Rtf = ocomps.udcs(i).rtf
                Me.TabControl1.TabPages(1).Enabled = True
                Me.bsave.Enabled = True
                Me.TabControl1.TabPages(1).Text = "Preview: " + Me.Lcomps.SelectedItems(0).Text

                Exit For
            End If
        Next
    End Sub
    Private Sub RadioButton2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rpublic.CheckedChanged
        If Me.TabControl1.TabPages.Count > 1 Then
            If Me.rpublic.Checked = True Then
                load_comps(False)
            End If
        End If
    End Sub

    Private Sub Dfrom_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Dfrom.ValueChanged

        load_comps(True)
    End Sub

    Private Sub Dto_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Dto.ValueChanged
        load_comps(True)
    End Sub
    Private Sub load_dynamic_lists()
        Try
            loading = True
            Dim op As String
            Dim oa As String
            Dim oe As String
            op = Me.Cpubtype.Text
            oa = Me.Cauthor.Text
            oe = Me.Centity.Text

            Me.Cpubtype.Items.Clear()
            Me.Cpubtype.Items.Add("All")
            Me.Centity.Items.Clear()
            Me.Centity.Items.Add("All")
            Me.Cauthor.Items.Clear()
            Me.Cauthor.Items.Add("All")
            Dim i, j As Integer
            Dim found As Boolean
            For i = 0 To ocomps.udcs.Count - 1
                found = False
                For j = 1 To Me.Cpubtype.Items.Count - 1
                    If CStr(Me.Cpubtype.Items(j)) = CStr(ocomps.udcs(i).pub_type) Then
                        found = True
                        Exit For
                    End If
                Next
                If found = False And ocomps.udcs(i).pub_type <> "-" Then
                    Me.Cpubtype.Items.Add(CStr(ocomps.udcs(i).pub_type))
                End If

                found = False
                For j = 1 To Me.Centity.Items.Count - 1
                    If CStr(Me.Centity.Items(j)) = CStr(ocomps.udcs(i).entity) Then
                        found = True
                        Exit For
                    End If
                Next
                If found = False And ocomps.udcs(i).entity <> "-" Then
                    Me.Centity.Items.Add(ocomps.udcs(i).entity)
                End If
                found = False
                For j = 1 To Me.Cauthor.Items.Count - 1
                    If UCase(ocomps.udcs(i).author) = UCase(Me.user_name) Then
                        ocomps.udcs(i).author = "me"
                    End If
                    If CStr(Me.Cauthor.Items(j)) = CStr(ocomps.udcs(i).author) Then
                        found = True
                        Exit For
                    End If
                Next

                If found = False And ocomps.udcs(i).author <> "-" Then
                    If UCase(ocomps.udcs(i).author) = UCase(Me.user_name) Then
                        Me.Cauthor.Items.Add("me")
                    Else
                        Me.Cauthor.Items.Add(ocomps.udcs(i).author)
                    End If
                End If

            Next


            Try
                Me.Cpubtype.Text = op
                If Me.Cpubtype.SelectedIndex = -1 Then
                    Me.Cpubtype.SelectedIndex = 0
                End If
            Catch ex As Exception
                Me.Cpubtype.SelectedIndex = 0
            End Try
            Try
                Me.Centity.Text = oe
                If Me.Centity.SelectedIndex = -1 Then
                    Me.Centity.SelectedIndex = 0
                End If
            Catch ex As Exception
                Me.Centity.SelectedIndex = 0
            End Try
            Try
                Me.Cauthor.Text = oa
                If Me.Cauthor.SelectedIndex = -1 Then
                    Me.Cauthor.SelectedIndex = 0
                End If
            Catch ex As Exception
                Me.Cauthor.SelectedIndex = 0

            End Try



        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_ins_adhoc_component", "load_dynamic_lists", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            loading = False
        End Try
    End Sub

    Private Sub Cauthor_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cauthor.SelectedIndexChanged
        load_comps(False)
    End Sub

    Private Sub Cpubtype_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cpubtype.SelectedIndexChanged
        load_comps(False)
    End Sub

    Private Sub Centity_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Centity.SelectedIndexChanged
        load_comps(False)
    End Sub

    Public no_column As Boolean
    Public Shared column As Integer
    Private Sub DocumentList_ColumnClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles Lcomps.ColumnClick

        Dim slog = New bc_cs_activity_log("bc_am_workflow_frm", "get_entity_id_selected", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try

            no_column = False

            column = CInt(e.Column.ToString)

            Select Case CInt(e.Column.ToString)

                Case 0, 1, 3, 4, 5

                    REM title

                    Lcomps.ListViewItemSorter() = New CompareByname

                    If CompareByname.toggle = False Then

                        CompareByname.toggle = True

                    Else

                        CompareByname.toggle = False

                    End If

                Case 2

                    REM date

                    Lcomps.ListViewItemSorter() = New CompareBydate

                    If CompareBydate.toggle = False Then

                        CompareBydate.toggle = True

                    Else

                        CompareBydate.toggle = False

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

    Private Sub bc_am_user_defined_components_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub budcall_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles budcall.CheckedChanged
        If Me.TabControl1.TabPages.Count > 1 Then
            If Me.budcall.Checked = True Then
                load_comps(False)
            End If
        End If
    End Sub


    Private Sub Cauthor_SelectedIndexChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cauthor.SelectedIndexChanged

    End Sub

    Private Sub Dfrom_ValueChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Dfrom.ValueChanged

    End Sub

    Private Sub rsystem_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rsystem.CheckedChanged
        If Me.rsystem.Checked = True Then
            Me.pfilter.Enabled = False
            load_comps(False)
        End If
    End Sub
    Private Sub ruser_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Ruser.CheckedChanged
        If Me.Ruser.Checked = True Then
            Me.pfilter.Enabled = True
            load_comps(False)
        End If
    End Sub

    Private Sub Rall_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Rall.CheckedChanged
        If Me.Rall.Checked = True Then
            Me.pfilter.Enabled = True
            load_comps(False)
        End If
    End Sub

    Private Sub Cclasssel_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cclasssel.SelectedIndexChanged
        If Me.Cclasssel.SelectedIndex = -1 Then
            Exit Sub
        End If
        Dim sel_entity As String = ""
        Me.Centitysel.Items.Clear()
        Me.Centitysel.Items.Add("none")
        If Me.Cclasssel.Text = "none" Then
            Me.Centitysel.Text = "none"
            Exit Sub
        End If
        For i = 0 To bc_am_load_objects.obc_entities.entity.Count - 1
            With bc_am_load_objects.obc_entities.entity(i)
                If .class_name = Me.Cclasssel.Text Then
                    Me.Centitysel.Items.Add(.name)
                    If .id = ldoc.entity_id Then
                        sel_entity = .name
                    End If
                End If
            End With
        Next
        If sel_entity = "" Then
            Me.selected_entity_id = 0
        Else
            Me.Centitysel.Text = sel_entity
        End If
    End Sub

    Private Sub Centitysel_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Centitysel.SelectedIndexChanged
        Me.selected_entity_id = 0
        If Me.Centitysel.SelectedIndex = -1 Then
            Exit Sub
        End If
        If Me.Centitysel.SelectedIndex = 0 Then
            Exit Sub
        End If
        For i = 0 To bc_am_load_objects.obc_entities.entity.Count - 1
            With bc_am_load_objects.obc_entities.entity(i)
                If .name = Me.Centitysel.Text And .class_name = Me.Cclasssel.Text Then
                    Me.selected_entity_id = .id
                    Exit Sub
                End If
            End With
        Next
    End Sub

    Private Sub Label2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label2.Click

    End Sub

    Private Sub Pselitem_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Pselitem.Paint

    End Sub
End Class

