Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports BlueCurve.Core.AS
Imports BlueCurve.Create.AM

Public Class bc_am_dx_insert_component
    Implements Ibc_am_dx_insert_component
    Event insert(component_id As Long, entity_id As Long) Implements Ibc_am_dx_insert_component.insert

    Dim _component_id As Long
    Dim _entity_id As Long
    Dim _class_id As Long
    Dim _component_name As String
    Dim _ocomps As bc_om_user_defined_components
    Dim _classes As List(Of bc_om_entity_class)

    Public Function load_view(component_Id As Long, class_id As Long, entity_Id As Long, component_name As String, class_name As String, entity_name As String, ocomps As bc_om_user_defined_components, classes As List(Of bc_om_entity_class), enable_class As Boolean, enable_entity As Boolean) Implements Ibc_am_dx_insert_component.load_view
        Try
            load_view = False
            _component_id = component_Id
            _entity_id = entity_Id
            _class_id = class_id
            _component_name = component_name
            _ocomps = ocomps
            _classes = classes

            If _component_id <> 0 Then
                REM class is that of component
                Me.uxcomp.Enabled = False
                For i = 0 To _ocomps.udcs.Count - 1
                    If ocomps.udcs(i).udc_id = component_Id Then
                        Me.uxcomp.Properties.Items.Add(_ocomps.udcs(i).title)
                        Me.uxcomp.SelectedIndex = 0
                        Me.uxcomp.Enabled = False

                        If _ocomps.udcs(i).for_class = 0 Then
                            Me.uxclass.Properties.Items.Add("None")
                        Else
                            For j = 0 To _classes.Count - 1

                                If (_classes(j).class_id = ocomps.udcs(i).for_class) Then
                                    Me.uxclass.Properties.Items.Add(classes(j).class_name)
                                    _class_id = _classes(j).class_id
                                    Me.lclass.Text = classes(j).class_name
                                    Me.uxclass.SelectedIndex = 0
                                    Exit For
                                End If
                            Next
                        End If
                        Me.uxclass.Enabled = False
                        Exit For
                    End If
                Next
            Else
                load_comps()
                Dim idx As Integer

                Me.uxclass.Properties.Items.Add("None")
                For i = 0 To _classes.Count - 1
                    Me.uxclass.Properties.Items.Add(_classes(i).class_name)
                    If (_classes(i).class_name = class_name) Then
                        idx = i
                    End If
                Next

                REM if component passed in set this to class component is for
                If component_Id <> 0 Then

                End If

                If class_id <> 0 Then
                    Me.uxclass.SelectedIndex = idx + 1
                End If



                If enable_class = False Then
                    Me.uxclass.Enabled = False
                End If
                If enable_entity = False And enable_class = False Then
                    Me.ListBoxControl1.Enabled = False
                    Me.psearch.Visible = False
                    Me.tsearch.Visible = False
                End If
            End If
            Me.RadioGroup1.SelectedIndex = 1


            load_view = True
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Cbc_am_dx_insert_component", "load_view", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Function
    Private Sub load_comps()
        Try
            Me.uxcomp.Properties.Items.Clear()


            For i = 0 To _ocomps.udcs.Count - 1
                If _class_id = 0 Or _class_id = _ocomps.udcs(i).for_class Then
                    Me.uxcomp.Properties.Items.Add(_ocomps.udcs(i).title)
                End If
            Next

            If _component_id <> 0 Then
                Me.uxcomp.Enabled = False
                Me.uxcomp.Text = _component_name
            ElseIf Me.uxcomp.Properties.Items.Count = 1 Then
                Me.uxcomp.Enabled = False
                Me.uxcomp.SelectedIndex = 0
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Cbc_am_dx_insert_component", "load_comps", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Private Sub bcancel_Click(sender As Object, e As EventArgs) Handles bcancel.Click
        Me.Hide()
    End Sub

    Private Sub bok_Click(sender As Object, e As EventArgs) Handles uxinsert.Click
        Cursor = Windows.Forms.Cursors.WaitCursor
        Try


            For i = 0 To _ocomps.udcs.Count - 1
                If _ocomps.udcs(i).title = Me.uxcomp.Text Then
                    _component_id = _ocomps.udcs(i).udc_id
                    Exit For
                End If
            Next
            _entity_id = 0

            If _class_id <> 0 Then
                For i = 0 To bc_am_load_objects.obc_entities.entity.Count - 1
                    If bc_am_load_objects.obc_entities.entity(i).class_Id = _class_id AndAlso bc_am_load_objects.obc_entities.entity(i).name = Me.ListBoxControl1.Text Then
                        _entity_id = bc_am_load_objects.obc_entities.entity(i).id
                        Exit For
                    End If
                Next
            End If

            RaiseEvent insert(_component_id, _entity_id)
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Cbc_am_dx_insert_component", "bok_Click", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            Cursor = Windows.Forms.Cursors.WaitCursor
            Me.Hide()
        End Try
    End Sub

    Private Sub uxclass_SelectedIndexChanged(sender As Object, e As EventArgs) Handles uxclass.SelectedIndexChanged
        Try
            If Me.uxclass.Properties.Items.Count = 1 Then
                Me.tsearch.Enabled = True
                Me.psearch.Enabled = True
                Me.ListBoxControl1.Enabled = True
                load_entities()
            Else

                Me.tsearch.Enabled = False
                Me.psearch.Enabled = False
                Me.ListBoxControl1.Enabled = False

                If Me.uxclass.SelectedIndex > -1 Then
                    If Me.uxclass.SelectedIndex > 0 Then
                        _class_id = _classes(Me.uxclass.SelectedIndex).class_id - 1
                        Me.tsearch.Enabled = True
                        Me.psearch.Enabled = True
                        Me.ListBoxControl1.Enabled = True
                    Else
                        _class_id = 0
                    End If
                    load_comps()
                    load_entities()
                End If
            End If
            check_complete()

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Cbc_am_dx_insert_component", "uxclass_SelectedIndexChanged", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Private Sub load_entities()
        Try
            'Me.uxentity.beginUpdate()
            Me.ListBoxControl1.Items.Clear()
            Me.ListBoxControl1.Text = ""

            If _class_id = 0 Then
                Exit Sub
            End If
            If Me.tsearch.Text <> "" Then
                Me.tsearch.Text = ""
            Else
                run_search()
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Cbc_am_dx_insert_component", "load_entities", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            'Me.uxentity.EndUpdate()
        End Try
    End Sub

    Private Sub uxcomp_SelectedIndexChanged(sender As Object, e As EventArgs) Handles uxcomp.SelectedIndexChanged
        check_complete()

    End Sub
    Sub check_complete()
        Me.uxinsert.Enabled = False
        If uxcomp.SelectedIndex > -1 And uxclass.SelectedIndex > -1 And ListBoxControl1.SelectedIndex > -1 Then
            Me.uxinsert.Enabled = True
        End If
    End Sub


    Private Sub tsearch_EditValueChanged(sender As Object, e As EventArgs) Handles tsearch.TextChanged

        lsearchtimer.Stop()
        lsearchtimer.Start()
    End Sub
    Private Sub run_search() Handles lsearchtimer.Tick

        'Me.uxentity.BeginUpdate()
        lsearchtimer.Stop()
        Cursor = Windows.Forms.Cursors.WaitCursor
        Dim found_entities As New List(Of String)
        Dim found_entity_ids As New List(Of Long)
        Dim pref_entities As New List(Of String)
        Dim sel_entity As String = ""
        Try

            REM extended search terms
            REM needs to be real time now

            If Trim(Me.tsearch.Text) <> "" Then
                Dim search_results As New bc_om_real_time_search
                search_results.class_id = _class_id
                search_results.search_text = Me.tsearch.Text
                search_results.mine = False
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
            End If
            'If Trim(Me.tsearch.Text) <> "" Then
            '    For i = 0 To bc_am_load_objects.obc_entities.search_attributes.search_values.Count - 1
            '        If bc_am_load_objects.obc_entities.search_attributes.search_values(i).class_id = _class_id AndAlso InStr(UCase(bc_am_load_objects.obc_entities.search_attributes.search_values(i).value), UCase(Me.tsearch.Text)) > 0 Then
            '            found_entity_ids.Add(bc_am_load_objects.obc_entities.search_attributes.search_values(i).entity_id)
            '        End If
            '    Next
            'End If
            REM needs to be real time now
            For i = 0 To bc_am_load_objects.obc_entities.entity.Count - 1
                If bc_am_load_objects.obc_entities.entity(i).class_id = _class_id Then
                    For j = 0 To bc_am_load_objects.obc_prefs.pref.Count - 1
                        If bc_am_load_objects.obc_prefs.pref(j).entity_id = bc_am_load_objects.obc_entities.entity(i).id Then
                            pref_entities.Add(bc_am_load_objects.obc_entities.entity(i).name)
                            Exit For
                        End If
                    Next

                    If _entity_id <> 0 AndAlso _entity_id = bc_am_load_objects.obc_entities.entity(i).id Then
                        sel_entity = bc_am_load_objects.obc_entities.entity(i).name
                    End If
                    If Trim(Me.tsearch.Text) = "" Then
                        found_entities.Add(bc_am_load_objects.obc_entities.entity(i).name)
                    Else
                        If InStr(UCase(bc_am_load_objects.obc_entities.entity(i).name), UCase(Me.tsearch.Text)) > 0 Then
                            found_entities.Add(bc_am_load_objects.obc_entities.entity(i).name)
                        Else
                            For j = 0 To found_entity_ids.Count - 1
                                If found_entity_ids(j) = bc_am_load_objects.obc_entities.entity(i).id Then
                                    found_entities.Add(bc_am_load_objects.obc_entities.entity(i).name)
                                    Exit For
                                End If
                            Next
                        End If
                    End If
                End If
            Next





            Dim idx As Integer = -1

            Me.ListBoxControl1.Items.Clear()

            If Me.RadioGroup1.SelectedIndex = 0 Then
                For i = 0 To found_entities.Count - 1

                    Me.ListBoxControl1.Items.Add(found_entities(i))

                    If found_entities(i) = sel_entity Then
                        idx = i
                    End If
                Next
            Else
                Dim ecount As Integer = 0

                For i = 0 To found_entities.Count - 1
                    For j = 0 To pref_entities.Count - 1
                        If pref_entities(j) = found_entities(i) Then

                            Me.ListBoxControl1.Items.Add(found_entities(i))
                            If found_entities(i) = sel_entity Then
                                idx = ecount
                            End If
                            ecount = ecount + 1
                            Exit For
                        End If
                    Next
                Next
            End If

            If idx > -1 Then
                Me.ListBoxControl1.SelectedIndex = idx
            Else
                Me.ListBoxControl1.Text = ""
                Me.ListBoxControl1.SelectedIndex = -1
            End If


        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Cbc_am_dx_insert_component", "run_search", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            check_complete()
            Cursor = Windows.Forms.Cursors.Default
            'Me.uxentity.EndUpdate()
        End Try

    End Sub

    

   

    

    Private Sub bc_am_dx_insert_component_Load(sender As Object, e As EventArgs) Handles MyBase.Load
       


    End Sub

    Private Sub RadioGroup1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RadioGroup1.SelectedIndexChanged
        run_search()
    End Sub

    Private Sub tsearch_EditValueChanged_1(sender As Object, e As EventArgs) Handles tsearch.EditValueChanged

    End Sub

    Private Sub uxentity_SelectedIndexChanged(sender As Object, e As EventArgs)
        check_complete()
    End Sub

    Private Sub ListBoxControl1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBoxControl1.SelectedIndexChanged
        check_complete()
    End Sub
End Class
Public Class Cbc_am_dx_insert_component
    WithEvents _view As Ibc_am_dx_insert_component
    Private _ldoc As New bc_om_document
    Private _ao_type As String
    Private _ao_object As Object
    Private _class_id As Long
    Private _classes As New List(Of bc_om_entity_class)
    Private _enable_class As Boolean
    Private _enable_entity As Boolean
    Private odoc As bc_ao_at_object = Nothing
    Private name As String


    Public Sub New(view As bc_am_dx_insert_component, ao_type As String, ao_object As Object, enable_class As Boolean, enable_entity As Boolean)
        _ao_object = ao_object
        _ao_type = ao_type
        _view = view
        _enable_class = enable_class
        _enable_entity = enable_entity
    End Sub

    Public Function load(Optional component_id As Long = 0) As Boolean
        Try
            If load_metadata() = False Then
                Exit Function
            End If
            load = False
            Dim component_name As String = ""
            Dim entity_name As String = ""
            Dim class_name As String = ""

            Dim ocomps As New bc_om_user_defined_components
            ocomps.pub_type_id = _ldoc.pub_type_id
            ocomps.user_id = bc_cs_central_settings.logged_on_user_id
            ocomps.date_from = Now
            ocomps.date_to = Now


            If bc_cs_central_settings.connection_method = bc_cs_central_settings.ADO Then
                ocomps.db_read()
            Else
                ocomps.tmode = bc_cs_soap_base_class.tREAD
                If ocomps.transmit_to_server_and_receive(ocomps, True) = False Then
                    Exit Function
                End If
            End If

            If ocomps.udcs.Count = 0 Then
                Dim omsg As New bc_cs_message("Blue Curve", "No Insert Components Configured for Publication Type", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Function
            End If
            component_name = ""
            If component_id <> 0 Then
                For i = 0 To ocomps.udcs.Count - 1
                    If ocomps.udcs(i).udc_id = component_id Then
                        component_name = ocomps.udcs(i).title
                        Exit For
                    End If
                Next
                If component_name = "" Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Components " + CStr(component_id) + " Not Configured for Publication Type", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Function
                End If

            End If
            _class_id = 0
            Dim pclass_id As Long = 0
            Dim oclass As bc_om_entity_class

            For i = 0 To bc_am_load_objects.obc_entities.entity.Count - 1
                If pclass_id = 0 Or pclass_id <> bc_am_load_objects.obc_entities.entity(i).class_id Then
                    oclass = New bc_om_entity_class
                    oclass.class_id = bc_am_load_objects.obc_entities.entity(i).class_id
                    oclass.class_name = bc_am_load_objects.obc_entities.entity(i).class_name
                    _classes.Add(oclass)
                End If
                If _ldoc.entity_id <> 0 AndAlso bc_am_load_objects.obc_entities.entity(i).id = _ldoc.entity_id Then
                    _class_id = bc_am_load_objects.obc_entities.entity(i).class_Id
                    class_name = bc_am_load_objects.obc_entities.entity(i).class_name
                End If
                pclass_id = bc_am_load_objects.obc_entities.entity(i).class_id
            Next
            _view.load_view(component_id, _class_id, _ldoc.entity_id, component_name, class_name, entity_name, ocomps, _classes, _enable_class, _enable_entity)

            load = True

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Cbc_am_dx_insert_component", "load", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Function
    Private Function load_metadata() As Boolean
        Try
            load_metadata = False

            Dim omessage As bc_cs_message

            Dim recreate_flag As Boolean

            If _ao_type <> bc_ao_at_object.WORD_DOC And _ao_type <> bc_ao_at_object.POWERPOINT_DOC Then
                omessage = New bc_cs_message("Blue Curve create", "Application Object Type: " + _ao_type + " not supported", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Function
            End If


            If _ao_type = bc_ao_at_object.WORD_DOC Then
                odoc = New bc_ao_word(_ao_object)
            End If
            If _ao_type = bc_ao_at_object.POWERPOINT_DOC Then
                odoc = New bc_ao_powerpoint(_ao_object)
            End If
            odoc.unlock_document()
            REM set local index
            REM get filename
            name = odoc.get_name
            If Left(name, 4) = "view" Then
                omessage = New bc_cs_message("Blue Curve create", "Document is view only cannot Submit", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Function
            End If
            Dim fs As New bc_cs_file_transfer_services
            name = odoc.get_doc_id()
            If name = "" Then
                name = odoc.get_name

                REM SW cope with office versions
                If Left(Right(name, 4), 1) = "." Then
                    name = Left(name, Len(name) - 4)
                End If
                If Left(Right(name, 5), 1) = "." Then
                    name = Left(name, Len(name) - 5)
                End If


            End If

            If fs.check_document_exists_and_can_be_read(bc_cs_central_settings.local_repos_path + name + ".dat") = False Then
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.LOCAL Then
                    omessage = New bc_cs_message("Blue Curve create", "Cannot Submit Document as no network and document was not opened via Create.", bc_cs_message.MESSAGE)
                    Exit Function
                End If
                REM xml metadata file doesnt exists so attempt to recreate
                Dim ordm As New bc_am_recreate_doc_metadata
                If ordm.recreate_doc_metadata(name, odoc) = False Then
                    Exit Function
                Else
                    _ldoc = _ldoc.read_data_from_file(bc_cs_central_settings.local_repos_path + name + ".dat")
                    recreate_flag = True
                End If
            Else
                REM load metadata
                _ldoc = _ldoc.read_data_from_file(bc_cs_central_settings.local_repos_path + name + ".dat")
            End If

            load_metadata = True
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Cbc_am_dx_insert_component", "load_metadata", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try


    End Function
    Sub insert(component_id As Long, entity_id As Long) Handles _view.insert
        Try
            Dim locator As String = ""
            REM now create a refresh component and assign to document
            Dim i, j As Integer
            Dim found As Boolean
            found = False
            For i = 0 To bc_am_load_objects.obc_templates.component_types.component_types.Count - 1
                If component_id = bc_am_load_objects.obc_templates.component_types.component_types(i).component_id Then
                    found = True
                    Dim orefresh As New bc_om_refresh_component
                    'orefresh.locator = locator
                    orefresh.entity_id = entity_id
                    orefresh.name = bc_am_load_objects.obc_templates.component_types.component_types(i).component_name
                    orefresh.locator_description = bc_am_load_objects.obc_templates.component_types.component_types(i).component_name
                    orefresh.mode = bc_am_load_objects.obc_templates.component_types.component_types(i).mode
                    orefresh.mode_param1 = bc_am_load_objects.obc_templates.component_types.component_types(i).sp_name
                    orefresh.mode_param2 = bc_am_load_objects.obc_templates.component_types.component_types(i).insert_object
                    orefresh.contributor_id = bc_am_load_objects.obc_templates.component_types.component_types(i).contributor
                    orefresh.refresh_type = bc_am_load_objects.obc_templates.component_types.component_types(i).refresh_type()
                    orefresh.web_service_name = bc_am_load_objects.obc_templates.component_types.component_types(i).web_service_name
                    orefresh.external_id = bc_am_load_objects.obc_templates.component_types.component_types(i).external_id
                    orefresh.format_file = bc_am_load_objects.obc_templates.component_types.component_types(i).format_file

                    orefresh.type = component_id
                    For j = 0 To bc_am_load_objects.obc_templates.component_types.component_types(i).parameters.bc_om_component_parameters.count - 1
                        orefresh.parameters.component_template_parameters.Add(bc_am_load_objects.obc_templates.component_types.component_types(i).parameters.bc_om_component_parameters(j))
                    Next
                    _ldoc.refresh_components.pub_type_id = _ldoc.pub_type_id
                    _ldoc.refresh_components.language_id = _ldoc.language_id
                    orefresh.locator = odoc.set_locator_for_component(component_id, _ldoc.entity_id, orefresh.mode)
                    If orefresh.locator <> "" Then
                        _ldoc.refresh_components.refresh_components.Add(orefresh)
                        _ldoc.write_data_to_file(bc_cs_central_settings.local_repos_path + name + ".dat")
                        Dim ocomprefresh As New bc_am_at_change_component_params(False, _ao_object, _ao_type)
                    End If
                End If
            Next
            If found = False Then
                Dim omessage As New bc_cs_message("Blue Curve", "Component Id: " + CStr(component_id) + " doesnt exist in system.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Sub
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Cbc_am_dx_insert_component", "insert", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub

End Class
Public Interface Ibc_am_dx_insert_component
    Event insert(component_id As Long, entity_id As Long)
    Function load_view(component_Id As Long, class_id As Long, entity_Id As Long, component_name As String, class_name As String, entity_name As String, ocomps As bc_om_user_defined_components, classes As List(Of bc_om_entity_class), enable_class As Boolean, enable_entity As Boolean)
End Interface