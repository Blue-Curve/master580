Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports BlueCurve.Core.AS
Public Class bc_am_style_componetize
    Public ocomponents As New bc_om_document_components
    Public err_txt As String = ""
    Public Sub insert_previous_document_context(ByVal ao_object As Object, ByVal ao_type As String)
        Dim otrace As New bc_cs_activity_log("bc_am_style_componetize", "insert_previous_document_context", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            If UCase(ao_type) = "WORD" Then
                Dim ao_obj As New bc_ao_word

            Else
                Dim omsg As New bc_cs_message("Blue Curve", "Object type: " + ao_type + " not supported for this fetaure.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Sub
            End If

            REM insert output at selection point
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_style_componetize", "insert_previous_document_context", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_am_style_componetize", "insert_previous_document_context", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    REM this is called from ax layer for performance
    Public Function componetize_from_ax(ByVal ao_object As Object, ByVal ao_type As String, ByRef ocomponents As bc_om_document_components, ByVal componetize_styles As bc_om_componetize_styles_for_template) As Boolean
        Dim otrace As New bc_cs_activity_log("bc_am_style_componetize", "componetize_from_ax(", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            Dim ao_obj As bc_ao_word
            If UCase(ao_type) = "WORD" Then
                ao_obj = New bc_ao_word(ao_object)
            Else
                Dim omsg As New bc_cs_message("Blue Curve", "Object type: " + ao_type + " not supported for this fetaure.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Function
            End If
            componetize_from_ax = ao_obj.componetize(ocomponents, componetize_styles)



        Catch ex As Exception
            componetize_from_ax = False
            Dim oerr As New bc_cs_error_log("bc_am_style_componetize", "componetize_from_ax", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_am_style_componetize", "componetize_from_ax", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function

    Public Function componetize(ByVal ao_object As Object, ByVal ao_type As String, ByVal ldoc_id As Long) As Boolean
        Dim otrace As New bc_cs_activity_log("bc_am_style_componetize", "componetize", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            componetize = False

            Dim ao_obj As bc_ao_word

            If UCase(ao_type) = "WORD" Then
                ao_obj = New bc_ao_word(ao_object)
            Else
                'ao_obj = New bc_ao_powerpoint
                Dim omsg As New bc_cs_message("Blue Curve", "Object type: " + ao_type + " not supported for this fetaure.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Function
            End If


            Dim componetize_styles As New bc_om_componetize_styles_for_template
            Dim template_id As String
            template_id = ao_obj.get_property_value("rnet_template_id")
            If Not IsNumeric(template_id) Then
                Dim ocomm As New bc_cs_activity_log("bc_am_style_componetize", "componetize", bc_cs_activity_codes.COMMENTARY, "Failed to get template id")
                Exit Function
            End If

            componetize_styles.template_id = template_id


            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                componetize_styles.db_read()
            Else
                componetize_styles.tmode = bc_cs_soap_base_class.tREAD
                If componetize_styles.transmit_to_server_and_receive(componetize_styles, True) = False Then
                    Exit Function

                End If
            End If
            ocomponents.doc_id = ldoc_id
         
            If componetize_styles.found = False Then
                otrace = New bc_cs_activity_log("bc_am_style_componetize", "componetize", bc_cs_activity_codes.TRACE_EXIT, "Template not set up for style componetisation: " + CStr(template_id))
                componetize = True
                Exit Function
            End If
            If componetize_styles.styles.Count = 0 And componetize_styles.all_styles <> True Then
                otrace = New bc_cs_activity_log("bc_am_style_componetize", "componetize", bc_cs_activity_codes.TRACE_EXIT, "N Header styles set up for template not set up for style componetisation: " + CStr(template_id))
                componetize = True
                Exit Function
            End If


            REM get the styles for componitisation need these from current document object
            ocomponents.doc_id = ldoc_id
            ocomponents.components.Clear()
            ocomponents.template_id = ao_obj.get_property_value("rnet_template_id")

            REM see if pass through is installed if running from process for perfomance
            If Diagnostics.Process.GetCurrentProcess.MainModule.ModuleName = "BlueCurve.Process.AP.exe" Then
                Dim ocomm As New bc_cs_activity_log("bc_am_style_componetize", "componetize", bc_cs_activity_codes.COMMENTARY, "Componetize called from process")
                If ao_obj.bc_ax_style_componetize(ocomponents, componetize_styles) = False Then
                    ocomm = New bc_cs_activity_log("bc_am_style_componetize", "componetize", bc_cs_activity_codes.COMMENTARY, "using built in componetize")
                    REM call ao layer to componetize
                    If ao_obj.componetize(ocomponents, componetize_styles) = True Then
                        componetize = True
                    End If
                Else
                    componetize = True
                End If
            Else
                Dim ocomm As New bc_cs_activity_log("bc_am_style_componetize", "componetize", bc_cs_activity_codes.COMMENTARY, "Componetize called from toolbar")
                REM call ao layer to componetize
                If ao_obj.componetize(ocomponents, componetize_styles) = True Then
                    componetize = True
                Else
                    err_txt = ao_obj.componetize_error_text
                End If
            End If


        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_style_componetize", "componetize", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_am_style_componetize", "componetize", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function
End Class
Public Class bc_am_html_componetize
    Public ocomponents As New bc_om_document_components
    Public err_txt As String = ""
    'Public Sub insert_previous_document_context(ByVal ao_object As Object, ByVal ao_type As String)
    '    Dim otrace As New bc_cs_activity_log("bc_am_style_componetize", "insert_previous_document_context", bc_cs_activity_codes.TRACE_ENTRY, "")
    '    Try
    '        If UCase(ao_type) = "WORD" Then
    '            Dim ao_obj As New bc_ao_word

    '        Else
    '            Dim omsg As New bc_cs_message("Blue Curve", "Object type: " + ao_type + " not supported for this fetaure.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
    '            Exit Sub
    '        End If

    '        REM insert output at selection point
    '    Catch ex As Exception
    '        Dim oerr As New bc_cs_error_log("bc_am_style_componetize", "insert_previous_document_context", bc_cs_error_codes.USER_DEFINED, ex.Message)
    '    Finally
    '        otrace = New bc_cs_activity_log("bc_am_style_componetize", "insert_previous_document_context", bc_cs_activity_codes.TRACE_EXIT, "")
    '    End Try
    'End Sub
    REM this is called from ax layer for performance
    Public Function componetize_from_ax(ByVal ao_object As Object, ByVal ao_type As String, ByRef ocomponents As bc_om_document_components, ByVal componetize_styles As bc_om_html_componetize_settings_for_template                            ) As Boolean
        Dim otrace As New bc_cs_activity_log("bc_am_html_componetize", "componetize_from_ax(", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            Dim ao_obj As bc_ao_word_html_componetize

            If UCase(ao_type) = "WORD" Then
                ao_obj = New bc_ao_word_html_componetize(ao_object)
            Else
                Dim omsg As New bc_cs_message("Blue Curve", "Object type: " + ao_type + " not supported for this fetaure.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Function
            End If
            componetize_from_ax = ao_obj.componetize(ocomponents, componetize_styles)



        Catch ex As Exception
            componetize_from_ax = False
            Dim oerr As New bc_cs_error_log("bc_am_html_componetize", "componetize_from_ax", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_am_html_componetize", "componetize_from_ax", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function

    Public Function componetize(ByVal ao_object As Object, ByVal ao_type As String, ByVal ldoc_id As Long) As Boolean
        Dim otrace As New bc_cs_activity_log("bc_am_html_componetize", "componetize", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            componetize = False

            Dim ao_obj As bc_ao_word_html_componetize

            If UCase(ao_type) = "WORD" Then
                ao_obj = New bc_ao_word_html_componetize(ao_object)
            Else
                'ao_obj = New bc_ao_powerpoint
                Dim omsg As New bc_cs_message("Blue Curve", "Object type: " + ao_type + " not supported for this fetaure.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Function
            End If


            Dim componetize_styles As New bc_om_html_componetize_settings_for_template

            Dim template_id As String
            template_id = ao_obj.get_property_value("rnet_template_id")
            If Not IsNumeric(template_id) Then
                Dim ocomm As New bc_cs_activity_log("bc_am_style_componetize", "componetize", bc_cs_activity_codes.COMMENTARY, "Failed to get template id using default")
                template_id = 0
            End If

            componetize_styles.template_id = template_id


            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                componetize_styles.db_read()
            Else
                componetize_styles.tmode = bc_cs_soap_base_class.tREAD
                If componetize_styles.transmit_to_server_and_receive(componetize_styles, True) = False Then
                    Exit Function

                End If
            End If
            ocomponents.doc_id = ldoc_id


            REM get the styles for componitisation need these from current document object
            ocomponents.doc_id = ldoc_id
            ocomponents.components.Clear()
            ocomponents.template_id = template_id

            REM see if pass through is installed if running from process for perfomance
            If Diagnostics.Process.GetCurrentProcess.MainModule.ModuleName = "BlueCurve.Process.AP.exe" Then
                Dim ocomm As New bc_cs_activity_log("bc_am_html_componetize", "componetize", bc_cs_activity_codes.COMMENTARY, "Componetize called from process")
                If ao_obj.bc_ax_html_componetize(ocomponents, componetize_styles) = False Then
                    ocomm = New bc_cs_activity_log("bc_am_html_componetize", "componetize", bc_cs_activity_codes.COMMENTARY, "using built in componetize")
                    REM call ao layer to componetize
                    If ao_obj.componetize(ocomponents, componetize_styles) = True Then
                        componetize = True
                    End If
                Else
                    componetize = True
                End If
            Else
                Dim ocomm As New bc_cs_activity_log("bc_am_html_componetize", "componetize", bc_cs_activity_codes.COMMENTARY, "Componetize called from toolbar")
                REM call ao layer to componetize
                If ao_obj.componetize(ocomponents, componetize_styles) = True Then
                    componetize = True
                Else
                    err_txt = ao_obj.componetize_error_text
                End If
            End If


        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_html_componetize", "componetize", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_am_html_componetize", "componetize", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function
End Class
REM user defined components July 2010
Public Class bc_am_udcs

    Dim ao_type As String
    Dim ao_object As Object
    Dim odoc As bc_ao_at_object
    Dim ldoc As New bc_om_document
    Dim ofs As New bc_cs_file_transfer_services
    Dim name As String
    REM FIL MAY 2013
    Dim show_form As Boolean
    Public user_name As String
    REM constructor determines whether to show the
    REM submit form or not the active object and the type
    Public Sub New(ByVal show_form As Boolean, ByVal ao_object As Object, ByVal ao_type As String, Optional ByVal brecreate_metadata As Boolean = False)
        Dim slog = New bc_cs_activity_log("bc_am_udcs", "new", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim fs As New bc_cs_file_transfer_services
        Dim Connection_method As String = ""
        Dim local_docs As New bc_om_documents
        Dim original_local_index As Integer
        Dim recreate_flag As Boolean = False
        Dim noerr As Boolean = True
        Dim originally_local As Boolean = False
        
        Me.ao_type = ao_type
        Try
            Dim omessage As bc_cs_message
            If ao_type <> bc_ao_at_object.WORD_DOC And ao_type <> bc_ao_at_object.POWERPOINT_DOC Then
                omessage = New bc_cs_message("Blue Curve create", "Application Object Type: " + ao_type + " not supported", bc_cs_message.MESSAGE)
                Exit Sub
            End If
            REM FIL MAY 2013
            Me.show_form = show_form
            Me.ao_object = ao_object
            REM instantiate correct AO object
            If ao_type = bc_ao_at_object.WORD_DOC Then
                odoc = New bc_ao_word(ao_object)
            End If
            If ao_type = bc_ao_at_object.POWERPOINT_DOC Then
                odoc = New bc_ao_powerpoint(ao_object)
            End If
            odoc.unlock_document()
            REM set local index
            original_local_index = -1
            REM get filename
            name = odoc.get_name
            If Left(name, 4) = "view" Then
                omessage = New bc_cs_message("Blue Curve create", "Document is view only cannot access user deined components", bc_cs_message.MESSAGE)
                Exit Sub
            End If
            name = odoc.get_doc_id

            If name = "" Or name = "NONE" Then
                name = ldoc.id
            End If
            If fs.check_document_exists_and_can_be_read(bc_cs_central_settings.local_repos_path + name + ".dat") = False Then
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.LOCAL Then
                    omessage = New bc_cs_message("Blue Curve create", "Cannot Submit Document as no network and document was not opened via Create.", bc_cs_message.MESSAGE)
                    Exit Sub
                End If
                REM xml metadata file doesnt exists so attempt to recreate
                Dim ordm As New bc_am_recreate_doc_metadata
                ordm.brecreate_from_pub_type = brecreate_metadata
                ordm.odoc = odoc

                If ordm.recreate_doc_metadata(name, odoc) = False Then
                    Exit Sub
                Else
                    ldoc = ldoc.read_data_from_file(bc_cs_central_settings.local_repos_path + name + ".dat")
                    recreate_flag = True
                End If
            Else
                REM load metadata
                ldoc = ldoc.read_data_from_file(bc_cs_central_settings.local_repos_path + name + ".dat")
                Dim orefresh As bc_om_refresh_components
                orefresh = odoc.load_refresh_components(False)

                consolidate_refresh_list(orefresh)
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_udcs", "new", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

        End Try
    End Sub
    Private Sub consolidate_refresh_list(ByVal orefresh As bc_om_refresh_components)
        Dim slog = New bc_cs_activity_log("bc_am_at_refresh", "consolidate_refresh_list", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim i, j As Integer
            For i = 0 To ldoc.refresh_components.refresh_components.Count - 1
                With ldoc.refresh_components.refresh_components(i)
                    For j = 0 To orefresh.refresh_components.Count - 1
                        .locator_description = ""
                        If .locator = orefresh.refresh_components(j).locator Then
                            .locator_description = orefresh.refresh_components(j).locator_description
                            Exit For
                        End If
                    Next
                End With
            Next
            REM components without a locator decription no longer exist in document so remove from 
            REM metadata
            i = 0
            While i < ldoc.refresh_components.refresh_components.Count
                With ldoc.refresh_components.refresh_components(i)
                    If .locator_description = "" Then
                        ldoc.refresh_components.refresh_components.RemoveAt(i)
                    Else
                        i = i + 1
                    End If
                End With
            End While


        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_at_refresh", "consolidate_refresh_list", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_at_refresh", "consolidate_refresh_list", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Public Sub create_component()
        Dim ofrm As New bc_am_user_defined_components
        ofrm.ao_object = odoc
        ofrm.ldoc = ldoc
        ofrm.mode = 0
        ofrm.Visible = False
        ofrm.fnname = name
        If ofrm.load_data = True Then
            Dim oapi As New API
            API.SetWindowPos(ofrm.Handle.ToInt32, API.HWND_TOPMOST, 10, 10, 1, 1, 1)
            ofrm.Show()
        End If
    End Sub
    Public Sub insert_component()
        Try
            Dim name As String

            name = odoc.get_doc_id
            
            If odoc.get_locator_for_display(True) <> "" Then
                Dim omsg As New bc_cs_message("Blue Curve", "You are already in a component please move to another insertion point!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Sub
            End If

            Dim omessage As bc_cs_message
            Dim fs As New bc_cs_file_transfer_services
            Dim recreate_flag As Boolean = True
            If fs.check_document_exists_and_can_be_read(bc_cs_central_settings.local_repos_path + name + ".dat") = False Then
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.LOCAL Then
                    omessage = New bc_cs_message("Blue Curve create", "Cannot Refresh Document as no network and document was not opened via Create.", bc_cs_message.MESSAGE)
                    Exit Sub
                End If
                REM xml metadata file doesnt exists so attempt to recreate
                Dim ordm As New bc_am_recreate_doc_metadata
                ordm.brecreate_from_pub_type = True
                ordm.odoc = odoc
                If ordm.recreate_doc_metadata(name, odoc) = False Then
                    recreate_flag = False
                    Exit Sub
                Else
                    recreate_flag = True
                End If
            Else
                REM load metadata
                bc_am_load_objects.obc_current_document = bc_am_load_objects.obc_current_document.read_data_from_file(bc_cs_central_settings.local_repos_path + name + ".dat")
            End If
            If (recreate_flag = False) Then
                omessage = New bc_cs_message("Blue Curve - create", "Document Metadata file does not exist!", bc_cs_message.MESSAGE)
                Exit Sub
            End If

            Dim ofrm As New bc_am_user_defined_components
            ofrm.obj_type = ao_type
            ofrm.ao_object = odoc
            ofrm.ldoc = ldoc
            ofrm.mode = 1
            ofrm.user_name = Me.user_name
            ofrm.fnname = name
            ofrm.activedocument = ao_object

            If ofrm.load_data = True Then
                If ofrm.Lcomps.Items.Count = 0 Then
                    Dim omsg As New bc_cs_message("Blue Curve", "No Components Configured to Insert", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub
                End If
                REM FIL MAY 2013
                REM make this modal now
                ofrm.show_form = Me.show_form

                ofrm.ShowDialog()

                'Dim oapi As New API
                'API.SetWindowPos(ofrm.Handle.ToInt32, API.HWND_TOPMOST, 10, 10, 1, 1, 1)
                'ofrm.Show()
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_udcs", "insert_component", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try
    End Sub
    Public Sub delete_component_from_document()
        Dim ofrm As New bc_am_user_defined_components
        ofrm.ao_object = odoc
        If odoc.udi_get_selected_udi_id > 0 Then
            odoc.udi_delete_bookmark_and_range("rnet_udc_" + CStr(odoc.udi_get_selected_udi_id))
        Else
            Dim omsg As New bc_cs_message("Blue Curve", "No user defined component selected!", bc_cs_message.MESSAGE, False, False, "Yes", "No", False)
        End If
    End Sub
    'Private Function load_data() As Boolean
    '    Dim slog = New bc_cs_activity_log("bc_am_at_submit", "load_data", bc_cs_activity_codes.TRACE_ENTRY, "")

    '    Try
    '        load_data = False
    '        Dim i As Integer
    '        Dim obc_objects As New bc_am_load_objects
    '        REM check connection
    '        REM should be getting this from document
    '        'bc_cs_central_settings.selected_conn_method = ldoc.connection_method
    '        REM users
    '        bc_am_load_objects.obc_users = bc_am_load_objects.obc_users.read_data_from_file(bc_cs_central_settings.local_template_path + bc_am_load_objects.USERS_FILENAME)
    '        REM set logged on user
    '        For i = 0 To bc_am_load_objects.obc_users.user.Count - 1
    '            If bc_am_load_objects.obc_users.user(i).os_user_name = bc_cs_central_settings.logged_on_user_name Then
    '                bc_cs_central_settings.logged_on_user_id = bc_am_load_objects.obc_users.user(i).id
    '                user_name = bc_am_load_objects.obc_users.user(i).first_name + " " + bc_am_load_objects.obc_users.user(i).surname
    '                Exit For
    '            End If
    '        Next
    '        REM entities
    '        bc_am_load_objects.obc_entities = bc_am_load_objects.obc_entities.read_data_from_file(bc_cs_central_settings.local_template_path + bc_am_load_objects.ENTITIES_FILENAME)
    '        REM pub types
    '        bc_am_load_objects.obc_templates = bc_am_load_objects.obc_templates.read_data_from_file(bc_cs_central_settings.local_template_path + bc_am_load_objects.TEMPLATES_FILENAME)
    '        REM preferances
    '        REM bc_am_load_objects.obc_prefs = bc_am_load_objects.obc_pub_types.read_data_from_file(bc_cs_central_settings.local_template_path + bc_am_load_objects.PREFS_FILENAME)

    '        load_data = True
    '    Catch ex As Exception
    '        Dim db_err As New bc_cs_error_log("bc_am_load_objects", "load_data", bc_cs_error_codes.USER_DEFINED, ex.Message)
    '    Finally
    '        slog = New bc_cs_activity_log("bc_am_at_submit", "load_data", bc_cs_activity_codes.TRACE_EXIT, "")
    '    End Try

    'End Function

End Class


