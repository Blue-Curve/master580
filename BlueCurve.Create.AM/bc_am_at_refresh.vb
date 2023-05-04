Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports BlueCurve.Core.AS
Imports System.Threading
Imports System.Windows.Forms

REM ==========================================
REM Blue Curve Limited 2005
REM Module:       Refresh
REM Type:         Application Module
REM Description:  Refreshs a document
REM Version:      1
REM Change history
REM ==========================================
'Public Class bc_am_at_refresh
'    Dim ao_type As Integer
'    Dim ao_object As Object
'    Dim odoc As bc_ao_at_object
'    Public odocmetadata As New bc_om_document
'    Public success As Boolean
'    Public Sub New()
'        success = True
'    End Sub
'    Public Sub New(ByVal show_form As Boolean, ByVal ao_object As Object, ByVal ao_type As String)
'        Dim slog = New bc_cs_activity_log("bc_am_at_refresh", "new", bc_cs_activity_codes.TRACE_ENTRY, "")
'        Dim omessage As bc_cs_message
'        Dim name As String
'        Dim ocommentary As bc_cs_activity_log

'        Try
'            success = True
'            Dim bc_cs_central_settings As New bc_cs_central_settings(True)

'            REM instantiate correct AO object
'            If ao_type = bc_ao_at_object.WORD_DOC Then
'                odoc = New bc_ao_word(ao_object)
'            End If
'            If ao_type = bc_ao_at_object.POWERPOINT_DOC Then
'                odoc = New bc_ao_powerpoint(ao_object)
'            End If
'            REM get filename
'            name = odoc.get_name

'            REM SW cope with office versions
'            If Left(Right(name, 4), 1) = "." Then
'                name = Left(name, Len(name) - 4)
'            End If
'            If Left(Right(name, 5), 1) = "." Then
'                name = Left(name, Len(name) - 5)
'            End If


'            REM load XML metadata for file into document object
'            odocmetadata = odocmetadata.read_data_from_file(bc_cs_central_settings.local_repos_path + name + ".dat")
'            REM if form authentication assign values
'            'If bc_cs_central_settings.show_authentication_form = 1 Then
'            '    bc_cs_central_settings.user_name = odocmetadata.logged_on_user_name
'            '    bc_cs_central_settings.user_password = odocmetadata.logged_on_user_password
'            'End If
'            bc_cs_central_settings.selected_conn_method = odocmetadata.connection_method
'            bc_am_load_objects.obc_current_document = odocmetadata
'            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.LOCAL Then
'                ocommentary = New bc_cs_activity_log("bc_am_at_refresh", "new", bc_cs_activity_codes.COMMENTARY, "Cannot Refresh as System working Locally!")
'                omessage = New bc_cs_message("Blue Curve Create", "Cannot Refresh as System working Locally!", bc_cs_message.MESSAGE)
'            Else
'                bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve create", "Loading Refresh Components...", 30, True, True)
'                refresh()
'                bc_cs_central_settings.progress_bar.unload()
'            End If

'        Catch ex As Exception
'            Dim db_err As New bc_cs_error_log("bc_am_at_refresh", "new", bc_cs_error_codes.USER_DEFINED, ex.Message)
'        Finally
'            slog = New bc_cs_activity_log("bc_am_at_refresh", "new", bc_cs_activity_codes.TRACE_EXIT, "")
'        End Try
'    End Sub
'    Public Function refresh() As Boolean
'        Dim slog = New bc_cs_activity_log("bc_am_at_refresh", "refresh", bc_cs_activity_codes.TRACE_ENTRY, "")
'        Try
'            Dim orefresh As New bc_om_refresh_components
'            Dim ocommentary As bc_cs_activity_log
'            Dim omessage As bc_cs_message
'            refresh = True
'            bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve create", "Loading Refresh Components...", 30, True, True)
'            bc_cs_central_settings.progress_bar.increment("Loading Refresh Components...")
'            REM parse document to obtain collection of refresh components
'            orefresh = odoc.load_refresh_components(False)

'            REM send refresh request object to server and get object response populates
'            bc_cs_central_settings.progress_bar.increment("Retrieving Data For Components...")
'            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
'                ocommentary = New bc_cs_activity_log("bc_am_at_refresh", "new", bc_cs_activity_codes.COMMENTARY, "Refreshing via ADO!")
'                odoc.update_refresh_status_bar("Retrieving data via ADO...", 0, 0)
'                orefresh.db_read()
'                refresh = orefresh.success
'            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
'                ocommentary = New bc_cs_activity_log("bc_am_at_refresh", "new", bc_cs_activity_codes.COMMENTARY, "Refreshing via SOAP!")
'                odoc.update_refresh_status_bar("Retrieving data via SOAP...", 0, 0)
'                orefresh.packet_code = "refresh" + Format(Now, "ddmm")

'                orefresh.tmode = bc_cs_soap_base_class.tREAD
'                orefresh.transmit_to_server_and_receive(orefresh, True)
'            Else
'                ocommentary = New bc_cs_activity_log("bc_am_at_refresh", "new", bc_cs_activity_codes.COMMENTARY, "Connection Method not supported!")
'                omessage = New bc_cs_message("Blue Curve Create", "Connection Method not supported!", bc_cs_message.MESSAGE)
'            End If
'            odoc.populate_document(orefresh)


'        Catch ex As Exception
'            Dim db_err As New bc_cs_error_log("bc_am_at_refresh", "refresh", bc_cs_error_codes.USER_DEFINED, ex.Message)
'        Finally

'            slog = New bc_cs_activity_log("bc_am_at_refresh", "refresh", bc_cs_activity_codes.TRACE_EXIT, "")
'        End Try
'    End Function


'    Public Sub parse_document(ByVal ao_object As Object, ByVal ao_type As String)
'        Dim slog = New bc_cs_activity_log("bc_am_at_refresh", "new", bc_cs_activity_codes.TRACE_ENTRY, "")
'        Try
'            REM create new ao object

'        Catch ex As Exception
'            Dim db_err As New bc_cs_error_log("bc_am_at_refresh", "new", bc_cs_error_codes.USER_DEFINED, ex.Message)
'        Finally
'            slog = New bc_cs_activity_log("bc_am_at_refresh", "new", bc_cs_activity_codes.TRACE_EXIT, "")
'        End Try
'    End Sub
'End Class
REM class to enable ad-hoc sql calls from VBA
Public Class bc_am_sql
    Dim connection_method As String
    Dim obc_om_sql As bc_om_sql
    Public Sub New(ByVal sql As String, ByVal ao_object As Object, ByVal ao_type As String)
        Dim slog = New bc_cs_activity_log("bc_am_sql", "new", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim odoc As Object = Nothing
            Dim omessage As bc_cs_message
            'Dim name As String
            Dim odocmetadata As New bc_om_document
            obc_om_sql = New bc_om_sql(sql)
            REM get connection method from metadata file
            Dim bc_cs_central_settings As New bc_cs_central_settings(True)
            REM instantiate correct AO object
            If ao_type = bc_ao_at_object.WORD_DOC Then
                odoc = New bc_ao_word(ao_object)
            End If
            If ao_type = bc_ao_at_object.POWERPOINT_DOC Then
                omessage = New bc_cs_message("Blue Curve Create", "Not Yet Implemented!", bc_cs_message.MESSAGE)
            End If
            REM get filename
            'name = odoc.get_doc_id
            REM load XML metadata for file into document object
            'odocmetadata = odocmetadata.read_data_from_file(bc_cs_central_settings.local_repos_path + name + ".dat")
            connection_method = bc_cs_central_settings.connection_method

            'bc_cs_central_settings.selected_conn_method = connection_method
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_sql", "new", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_sql", "new", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    Public Function execute() As Object
        Dim slog = New bc_cs_activity_log("bc_am_sql", "execute", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            Dim ocommentary As bc_cs_activity_log
            If connection_method = bc_cs_central_settings.SOAP Then
                ocommentary = New bc_cs_activity_log("bc_am_at_refresh", "new", bc_cs_activity_codes.COMMENTARY, "Executing SQL via SOAP.")
                obc_om_sql.tmode = bc_cs_soap_base_class.tREAD
                obc_om_sql.transmit_to_server_and_receive(obc_om_sql, True)
                Return obc_om_sql.results
            ElseIf connection_method = bc_cs_central_settings.ADO Then
                ocommentary = New bc_cs_activity_log("bc_am_at_refresh", "new", bc_cs_activity_codes.COMMENTARY, "Executing SQL via ADO direct.")
                obc_om_sql.db_read()
                Return obc_om_sql.results
            ElseIf connection_method = bc_cs_central_settings.LOCAL Then
                ocommentary = New bc_cs_activity_log("bc_am_at_refresh", "new", bc_cs_activity_codes.COMMENTARY, "Cannot execute SQL as Connected Locally Only!")
                Return Nothing
            Else
                ocommentary = New bc_cs_activity_log("bc_am_at_refresh", "new", bc_cs_activity_codes.COMMENTARY, "Connection Method: " + connection_method + " not supported!")
                Return Nothing
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_sql", "execute", bc_cs_error_codes.USER_DEFINED, ex.Message)
            Return Nothing
        Finally
            slog = New bc_cs_activity_log("bc_am_sql", "execute", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function
End Class
'component refresh functionality
Public Class bc_am_at_change_component_params
    Dim ao_type As Integer
    Dim ao_object As Object
    Dim udc_mode As Boolean = False
    Dim odoc As bc_ao_at_object
    Public Sub New(ByVal show_form As Boolean, ByVal ao_object As Object, ByVal ao_type As String, Optional ByVal brecreate_metadata As Boolean = False, Optional ByVal bhide_refresh As Boolean = False, Optional ByRef locked As Boolean = False, Optional ByVal enable_cancel As Boolean = True, Optional ByVal enable_refresh As Boolean = True, Optional ByVal enable_highlight As Boolean = False, Optional ByVal disable_refresh As Boolean = False, Optional ByVal disable_contributer As Boolean = False, Optional ByVal enable_save As Boolean = True, Optional ByRef exit_code As Integer = 0)
        Dim slog = New bc_cs_activity_log("bc_am_at_change_component_params", "new", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim omessage As bc_cs_message
            Dim ocommentary As bc_cs_activity_log



            Dim name As String
            Dim locator As String
            'Dim bc_cs_central_settings As New bc_cs_central_settings(True)

            Dim extensionsize As Integer


            REM instantiate correct AO object
            If ao_type = bc_ao_at_object.WORD_DOC Then
                odoc = New bc_ao_word(ao_object)
            ElseIf ao_type = bc_ao_at_object.POWERPOINT_DOC Then
                odoc = New bc_ao_powerpoint(ao_object)
            Else
                omessage = New bc_cs_message("Blue Curve Create", "Not Yet Implemented!", bc_cs_message.MESSAGE)
                Exit Sub
            End If
            REM get filename
            name = odoc.get_name
            If Left(name, 4) = "view" Then
                omessage = New bc_cs_message("Blue Curve create", "Document is view only cannot Refresh", bc_cs_message.MESSAGE)
                Exit Sub
            End If
            Dim secondary_entity As Long
            secondary_entity = 0
            Try
                secondary_entity = odoc.get_property_value("rnet_secondary_entity_id")
            Catch

            End Try

            REM SW cope with office versions
            If Left(Right(name, 4), 1) = "." Then
                name = Left(name, Len(name) - 4)
            End If
            If Left(Right(name, 5), 1) = "." Then
                name = Left(name, Len(name) - 5)
            End If


            REM load XML metadata for file into document object
            Dim fs As New bc_cs_file_transfer_services

            name = odoc.get_doc_id
            Dim recreate_flag As Boolean = True
            If fs.check_document_exists_and_can_be_read(bc_cs_central_settings.local_repos_path + name + ".dat") = False Then
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.LOCAL Then
                    omessage = New bc_cs_message("Blue Curve create", "Cannot Refresh Document as no network and document was not opened via Create.", bc_cs_message.MESSAGE)
                    Exit Sub
                End If
                REM xml metadata file doesnt exists so attempt to recreate
                Dim ordm As New bc_am_recreate_doc_metadata
                ordm.brecreate_from_pub_type = brecreate_metadata
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
            bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve create", "Loading Component Parameters...", 30, True, True)
            bc_am_load_objects.obc_current_document = bc_am_load_objects.obc_current_document.read_data_from_file(bc_cs_central_settings.local_repos_path + name + ".dat")

            REM load entities
            'bc_am_load_objects.obc_entities = bc_am_load_objects.obc_entities.read_data_from_file(bc_cs_central_settings.local_template_path + bc_am_load_objects.ENTITIES_FILENAME)
            REM load pub types
            'bc_am_load_objects.obc_pub_types = bc_am_load_objects.obc_entities.read_data_from_file(bc_cs_central_settings.local_template_path + bc_am_load_objects.PUB_TYPES_FILENAME)
            REM if form authentication assign values
            'If bc_cs_central_settings.show_authentication_form = 1 Then
            '    bc_cs_central_settings.user_name = bc_am_load_objects.obc_current_document.logged_on_user_name
            '    bc_cs_central_settings.user_password = bc_am_load_objects.obc_current_document.logged_on_user_password
            'End If
            REM get locator
            locator = odoc.get_locator_for_display(True)
            If locator = "" Then
                bc_cs_central_settings.progress_bar.unload()
                omessage = New bc_cs_message("Blue Curve Create", "Component Not Selected.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Sub
            End If



            Dim row As Integer = 0
            Dim col As Integer = 0
            REM get last updated for each component from server
            Dim i As Integer
            For i = 0 To bc_am_load_objects.obc_current_document.refresh_components.refresh_components.Count - 1
                With bc_am_load_objects.obc_current_document.refresh_components.refresh_components(i)
                    .last_update_date = "9-9-9999"
                    .no_refresh = 1
                    .parent_entity_id = secondary_entity
                End With
            Next
            bc_am_load_objects.obc_current_document.refresh_components.language_id = bc_am_load_objects.obc_current_document.language_id
            bc_am_load_objects.obc_current_document.refresh_components.pub_type_id = bc_am_load_objects.obc_current_document.pub_type_id

            bc_am_load_objects.obc_current_document.refresh_components.doc_id = bc_am_load_objects.obc_current_document.id
            REM test take this out when finished
            For i = 0 To bc_am_load_objects.obc_current_document.refresh_components.refresh_components.Count - 1
                With bc_am_load_objects.obc_current_document.refresh_components.refresh_components(i)
                    If .locator = locator Then
                        .no_refresh = 0
                        bc_am_load_objects.obc_current_document.refresh_components.refresh_type = bc_am_load_objects.obc_current_document.refresh_components.refresh_components(i).refresh_type
                        If .mode = 2 Then
                            bc_am_load_objects.obc_current_document.refresh_components.oselection_parameters.value = odoc.get_selection_values(row, col)
                            bc_am_load_objects.obc_current_document.refresh_components.oselection_parameters.row = row
                            bc_am_load_objects.obc_current_document.refresh_components.oselection_parameters.col = col
                        ElseIf .mode = 8 Or .mode = 9 Or .mode = 100 Then

                            REM is UDC do something different
                            bc_am_load_objects.obc_current_document.refresh_components.single_select_mode = True
                            .single_Selected = True
                            If bc_am_load_objects.obc_current_document.connection_method = bc_cs_central_settings.ADO Then
                                bc_am_load_objects.obc_current_document.refresh_components.get_data_update_date()
                            ElseIf bc_am_load_objects.obc_current_document.connection_method = bc_cs_central_settings.SOAP Then
                                bc_am_load_objects.obc_current_document.refresh_components.tmode = bc_om_refresh_components.tREAD_UPDATE_DATES
                                bc_am_load_objects.obc_current_document.refresh_components.transmit_to_server_and_receive(bc_am_load_objects.obc_current_document.refresh_components, False)
                            End If
                            REM evaluate last update data
                            If .last_update_date <> "9-9-9999" And .last_refresh_date <> "9-9-9999" Then
                                If .last_update_date <= .last_refresh_date Then
                                    .no_refresh = 1
                                End If
                            End If
                            udc_mode = True
                        End If
                        .single_Selected = False
                        bc_am_load_objects.obc_current_document.refresh_components.single_select_mode = False
                        Exit For
                    End If
                End With
            Next
            bc_cs_central_settings.progress_bar.hide()
            If show_form = True Then
                REM DX upgrade
                Dim frefresh As New view_bc_dx_build_parameters
                Dim crefresh As New ctrll_bc_am_build_parameters(frefresh, bc_am_load_objects.obc_current_document, locator, enable_cancel, enable_refresh, disable_refresh, disable_contributer, enable_save)
                If crefresh.load_data = False Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Failed to Load Display", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub
                End If
                If crefresh.browser = True Then
                    Exit Sub
                End If
                frefresh.ShowDialog()
                REM if application is invisible this has been caled from a build so abort the document
                If crefresh.cancel_selected = True And odoc.check_if_visible = False Then
                    odoc.abort_doc()
                    If fs.check_document_exists(bc_cs_central_settings.local_repos_path + bc_am_load_objects.obc_current_document.filename + bc_am_load_objects.obc_current_document.extension) Then
                        fs.delete_file(bc_cs_central_settings.local_repos_path + bc_am_load_objects.obc_current_document.filename + bc_am_load_objects.obc_current_document.extension)
                    End If
                    If fs.check_document_exists(bc_cs_central_settings.local_repos_path + bc_am_load_objects.obc_current_document.filename + ".dat") Then
                        fs.delete_file(bc_cs_central_settings.local_repos_path + bc_am_load_objects.obc_current_document.filename + ".dat")
                    End If
                End If
                exit_code = 0
                If crefresh.cancel_selected = True Then
                    exit_code = 1
                ElseIf crefresh.refresh = False Then
                    exit_code = 2
                End If


                If crefresh.cancel_selected = True Or crefresh.refresh = False Then
                    Exit Sub
                End If
            End If

                'Dim orefreshfrm As New bc_am_component_refresh
                'orefreshfrm.ao_object = odoc
                'orefreshfrm.component_mode = True
                'orefreshfrm.slocator = locator
                'orefreshfrm.chighsdc.Visible = False
                'orefreshfrm.Rall.Visible = False
                'orefreshfrm.Redit.Visible = False

                'orefreshfrm.Button2.Enabled = enable_cancel
                'orefreshfrm.btnopen.Enabled = enable_refresh
                'REM FIL DEC 2013
                'orefreshfrm.Button1.Enabled = enable_save
                'If enable_refresh = False Then
                '    orefreshfrm.master_refresh_disabled = True
                'End If
                'orefreshfrm.disable_contributer = disable_contributer
                'orefreshfrm.disable_refresh = disable_refresh
                'If disable_contributer = True Then
                '    orefreshfrm.ComboBox1.Enabled = False
                'End If
                'If disable_refresh = True Then
                '    orefreshfrm.bgoto.Enabled = False
                'End If

                'orefreshfrm.bhighlight.Visible = enable_highlight


                'If bhide_refresh = True Then
                '    orefreshfrm.btnopen.Visible = False
                'End If
                'If udc_mode = True Then
                '    orefreshfrm.trtf.Visible = True
                '    orefreshfrm.lrtf.Text = "Preview of current stored: " + bc_am_load_objects.obc_current_document.refresh_components.refresh_components(i).name
                '    If bc_am_load_objects.obc_current_document.refresh_components.refresh_components(i).entity_id <> 0 And bc_am_load_objects.obc_current_document.refresh_components.refresh_components(i).entity_id <> bc_am_load_objects.obc_current_document.entity_id Then
                '        For j = 0 To bc_am_load_objects.obc_entities.entity.Count - 1
                '            If bc_am_load_objects.obc_entities.entity(j).id = bc_am_load_objects.obc_current_document.refresh_components.refresh_components(i).entity_id Then
                '                orefreshfrm.lrtf.Text = "Preview of current stored: " + bc_am_load_objects.obc_current_document.refresh_components.refresh_components(i).name + " [" + bc_am_load_objects.obc_entities.entity(j).name + "]"
                '                Exit For
                '            End If
                '        Next
                '    End If
                '    If bc_am_load_objects.obc_current_document.refresh_components.refresh_components(i).mode = 9 Then
                '        orefreshfrm.trtf.Text = bc_am_load_objects.obc_current_document.refresh_components.refresh_components(i).refresh_value(0, 0)
                '    Else
                '        Try
                '            orefreshfrm.trtf.Rtf = bc_am_load_objects.obc_current_document.refresh_components.refresh_components(i).refresh_value(0, 0)
                '        Catch
                '            orefreshfrm.trtf.Text = bc_am_load_objects.obc_current_document.refresh_components.refresh_components(i).refresh_value(0, 0)
                '        End Try
                '    End If
                '    Application.DoEvents()
                'Else
                '    orefreshfrm.trtf.Visible = False
                'End If
                REM ING August 2012
                'Dim oapi As New API
                'API.SetWindowPos(orefreshfrm.Handle.ToInt32, API.HWND_TOPMOST, 10, 10, 1, 1, 1)
                REM --------------------------
                'orefreshfrm.TopMost = True
                'orefreshfrm.ShowDialog()

                'If orefreshfrm.cancel_selected = False Then
                '    REM update property values
                '    odoc.update_parameter_values_to_document(bc_am_load_objects.obc_current_document.refresh_components, Nothing)
                'End If
                'If orefreshfrm.ok_selected = False Then
                '    bc_cs_central_settings.progress_bar.unload()
                '    Exit Sub
                'End If
                'Else
                For i = 0 To bc_am_load_objects.obc_current_document.refresh_components.refresh_components.Count - 1
                    bc_am_load_objects.obc_current_document.refresh_components.refresh_components(i).no_refresh = 1
                    If bc_am_load_objects.obc_current_document.refresh_components.refresh_components(i).locator = locator Then
                        bc_am_load_objects.obc_current_document.refresh_components.refresh_components(i).no_refresh = 0
                    End If
                Next
                'End If
                bc_am_load_objects.obc_current_document.refresh_components.component_documents = bc_am_load_objects.obc_current_document.composite_constiuents



                If bc_am_load_objects.obc_current_document.connection_method = bc_cs_central_settings.LOCAL Then
                    ocommentary = New bc_cs_activity_log("bc_am_at_refresh", "new", bc_cs_activity_codes.COMMENTARY, "Cannot Refresh as System working Locally!")
                    omessage = New bc_cs_message("Blue Curve Create", "Cannot Refresh as System working Locally!", bc_cs_message.MESSAGE)
                Else
                    REM send refresh request object to server and get object response populates
                    bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve create", "Retrieving Data For Components...", 30, True, True)
                    bc_cs_central_settings.progress_bar.increment("Retrieving Data For Component...")
                    Dim did As String = ""
                    If bc_am_load_objects.obc_current_document.id = 0 Then
                        bc_am_load_objects.obc_current_document.refresh_components.doc_id = bc_am_load_objects.obc_current_document.filename
                    Else
                        bc_am_load_objects.obc_current_document.refresh_components.doc_id = bc_am_load_objects.obc_current_document.id
                    End If
                    bc_am_load_objects.obc_current_document.refresh_components.pub_type_id = bc_am_load_objects.obc_current_document.pub_type_id

                    bc_am_load_objects.obc_current_document.refresh_components.indexs_only = False
                    bc_am_load_objects.obc_current_document.refresh_components.assoc_entities = bc_am_load_objects.obc_current_document.disclosures
                    If bc_am_load_objects.obc_current_document.connection_method = bc_cs_central_settings.ADO Then
                        ocommentary = New bc_cs_activity_log("bc_am_at_refresh", "new", bc_cs_activity_codes.COMMENTARY, "Refreshing via ADO!")
                        odoc.update_refresh_status_bar("Retrieving data via ADO...", 0, 0)
                        bc_am_load_objects.obc_current_document.refresh_components.language_id = bc_am_load_objects.obc_current_document.language_id
                        bc_am_load_objects.obc_current_document.refresh_components.pub_type_id = bc_am_load_objects.obc_current_document.pub_type_id
                        bc_am_load_objects.obc_current_document.refresh_components.db_read()
                    ElseIf bc_am_load_objects.obc_current_document.connection_method = bc_cs_central_settings.SOAP Then
                        ocommentary = New bc_cs_activity_log("bc_am_at_refresh", "new", bc_cs_activity_codes.COMMENTARY, "Refreshing via SOAP!")
                        odoc.update_refresh_status_bar("Retrieving data via SOAP...", 0, 0)
                        bc_am_load_objects.obc_current_document.refresh_components.tmode = bc_cs_soap_base_class.tREAD
                        bc_am_load_objects.obc_current_document.refresh_components.transmit_to_server_and_receive(bc_am_load_objects.obc_current_document.refresh_components, True)
                    Else
                        ocommentary = New bc_cs_activity_log("bc_am_at_refresh", "new", bc_cs_activity_codes.COMMENTARY, "Connection Method not supported!")
                        omessage = New bc_cs_message("Blue Curve Create", "Connection Method not supported!", bc_cs_message.MESSAGE)
                    End If
                    bc_am_load_objects.obc_current_document.disclosures = bc_am_load_objects.obc_current_document.refresh_components.assoc_entities
                    bc_cs_central_settings.progress_bar.increment("Refreshing Document Components...")
                    REM populate document 
                    odoc.populate_document(bc_am_load_objects.obc_current_document.refresh_components)
                    REM if highlighting turned on turn this on
                    Dim idx As String
                    Dim sdc_colour_index As Long = 0
                    Dim udc_colour_index As Long = 0
                    idx = odoc.get_property_value("xrnet_sdc_colour_idx")
                    If IsNumeric(idx) Then
                        sdc_colour_index = idx
                    End If
                    idx = odoc.get_property_value("xrnet_udc_colour_idx")
                    If IsNumeric(idx) Then
                        udc_colour_index = idx
                    End If
                    Dim bk_colour As Long
                    With bc_am_load_objects.obc_current_document.refresh_components
                        If .show_sdcs = True And sdc_colour_index <> 0 Then
                            For i = 0 To .refresh_components.Count - 1
                                If .refresh_components(i).locator = locator And .refresh_components(i).mode <> 8 And .refresh_components(i).mode <> 9 Then
                                    bk_colour = odoc.get_background_color_for_locator(.refresh_components(i).locator)
                                    If bk_colour <> udc_colour_index And .refresh_components(i).original_markup_colour_index = 0 Then
                                        .refresh_components(i).original_markup_colour_index = bk_colour
                                        odoc.set_background_colour_for_locator(.refresh_components(i).locator, sdc_colour_index, .refresh_components(i).mode)
                                        odoc.update_refresh_status_bar("highlight component ", CStr(i + 1), CStr(.refresh_components.Count))
                                    End If
                                    Exit For
                                End If
                            Next
                        End If
                        If .show_udcs = True And udc_colour_index <> 0 Then
                            For i = 0 To .refresh_components.Count - 1
                                If .refresh_components(i).locator = locator And (.refresh_components(i).mode = 100 Or .refresh_components(i).mode = 9 Or .refresh_components(i).mode = 10) Then
                                    bk_colour = odoc.get_background_color_for_locator(.refresh_components(i).locator)
                                    If bk_colour <> udc_colour_index And .refresh_components(i).original_markup_colour_index = 0 Then
                                        MsgBox(CStr(bk_colour))
                                        .refresh_components(i).original_markup_colour_index = bk_colour
                                        odoc.set_background_colour_for_locator(.refresh_components(i).locator, udc_colour_index, .refresh_components(i).mode)
                                        odoc.update_refresh_status_bar("highlight component ", CStr(i + 1), CStr(.refresh_components.Count))
                                    End If
                                    Exit For
                                End If
                            Next
                        End If


                    End With

                    odoc.remove_invalid_bookmarks(bc_am_load_objects.obc_current_document.refresh_components)


                    REM save down metadata
                    If bc_am_load_objects.obc_current_document.id > 0 Then
                        bc_am_load_objects.obc_current_document.write_data_to_file(bc_cs_central_settings.local_repos_path + CStr(bc_am_load_objects.obc_current_document.id) + ".dat")
                    Else

                        REM SW cope with office versions
                        extensionsize = 0
                        extensionsize = (Len(bc_am_load_objects.obc_current_document.filename) - (InStrRev(bc_am_load_objects.obc_current_document.filename, ".") - 1))

                        If Right(bc_am_load_objects.obc_current_document.filename, extensionsize) <> bc_am_load_objects.obc_current_document.extension Then
                            bc_am_load_objects.obc_current_document.write_data_to_file(bc_cs_central_settings.local_repos_path + bc_am_load_objects.obc_current_document.filename + ".dat")
                        Else
                            bc_am_load_objects.obc_current_document.write_data_to_file(bc_cs_central_settings.local_repos_path + Left(bc_am_load_objects.obc_current_document.filename, Len(bc_am_load_objects.obc_current_document.filename) - extensionsize) + ".dat")
                        End If
                    End If
                    bc_cs_central_settings.progress_bar.unload()
                End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_at_change_component_params", "new", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            locked = bc_am_load_objects.obc_current_document.locked
            slog = New bc_cs_activity_log("bc_am_at_change_component_params", "new", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    REM loads relevant data for submit into memory
    REM FIL MAY 2013
    'Private Function load_data() As Boolean
    '    Dim slog = New bc_cs_activity_log("bc_am_at_change_component_params", "load_data", bc_cs_activity_codes.TRACE_ENTRY, "")

    '    Try
    '        load_data = False
    '        Dim i As Integer
    '        'Dim obc_objects As New bc_am_load_objects
    '        REM check connection
    '        REM should be getting this from document
    '        bc_cs_central_settings.version = Application.ProductVersion
    '        'bc_cs_central_settings.selected_conn_method = bc_am_load_objects.obc_current_document.connection_method
    '        REM users
    '        bc_am_load_objects.obc_users = bc_am_load_objects.obc_users.read_data_from_file(bc_cs_central_settings.local_template_path + bc_am_load_objects.USERS_FILENAME)
    '        REM set logged on user
    '        For i = 0 To bc_am_load_objects.obc_users.user.Count - 1
    '            If bc_am_load_objects.obc_users.user(i).os_user_name = bc_cs_central_settings.logged_on_user_name Then
    '                bc_cs_central_settings.logged_on_user_id = bc_am_load_objects.obc_users.user(i).id
    '                Exit For
    '            End If
    '        Next
    '        REM entities
    '        'bc_am_load_objects.obc_entities = bc_am_load_objects.obc_entities.read_data_from_file(bc_cs_central_settings.local_template_path + bc_am_load_objects.ENTITIES_FILENAME)
    '        REM pub types
    '        'bc_am_load_objects.obc_pub_types = bc_am_load_objects.obc_pub_types.read_data_from_file(bc_cs_central_settings.local_template_path + bc_am_load_objects.PUB_TYPES_FILENAME)
    '        REM preferances
    '        'bc_am_load_objects.obc_prefs = bc_am_load_objects.obc_pub_types.read_data_from_file(bc_cs_central_settings.local_template_path + bc_am_load_objects.PREFS_FILENAME)

    '        load_data = True
    '    Catch ex As Exception
    '        Dim db_err As New bc_cs_error_log("bc_am_at_change_component_params", "load_data", bc_cs_error_codes.USER_DEFINED, ex.Message)
    '    Finally
    '        slog = New bc_cs_activity_log("bc_am_at_change_component_params", "load_data", bc_cs_activity_codes.TRACE_EXIT, "")
    '    End Try

    'End Function

End Class

'edit chart functionality
Public Class bc_am_at_edit_chart
    Dim ao_type As Integer
    Dim ao_object As Object
    Dim odoc As bc_ao_at_object
    Public Sub New(ByVal ao_object As Object, ByVal ao_type As String, Optional ByVal brecreate_metadata As Boolean = False)
        Dim slog = New bc_cs_activity_log("bc_am_at_edit_chart", "new", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim omessage As bc_cs_message
            Dim ocommentary As bc_cs_activity_log
            Dim name As String
            Dim locator As String
            Dim bc_cs_central_settings As New bc_cs_central_settings(True)
            Dim extensionsize As Integer


            REM instantiate correct AO object
            If ao_type = bc_ao_at_object.WORD_DOC Then
                odoc = New bc_ao_word(ao_object)
            ElseIf ao_type = bc_ao_at_object.POWERPOINT_DOC Then
                odoc = New bc_ao_powerpoint(ao_object)
            Else
                omessage = New bc_cs_message("Blue Curve Create", "Not Yet Implemented!", bc_cs_message.MESSAGE)
                Exit Sub
            End If
            REM get filename
            name = odoc.get_name
            If Left(name, 4) = "view" Then
                omessage = New bc_cs_message("Blue Curve create", "Document is view only cannot edit chart", bc_cs_message.MESSAGE)
                Exit Sub
            End If

            REM SW cope with office versions
            If Left(Right(name, 4), 1) = "." Then
                name = Left(name, Len(name) - 4)
            End If
            If Left(Right(name, 5), 1) = "." Then
                name = Left(name, Len(name) - 5)
            End If


            REM load XML metadata for file into document object
            Dim fs As New bc_cs_file_transfer_services
            name = odoc.get_doc_id
            Dim recreate_flag As Boolean = True
            If fs.check_document_exists_and_can_be_read(bc_cs_central_settings.local_repos_path + name + ".dat") = False Then
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.LOCAL Then
                    omessage = New bc_cs_message("Blue Curve create", "Cannot edit chart as no network and document was not opened via Create.", bc_cs_message.MESSAGE)
                    Exit Sub
                End If
                REM xml metadata file doesnt exists so attempt to recreate
                Dim ordm As New bc_am_recreate_doc_metadata
                ordm.brecreate_from_pub_type = brecreate_metadata
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
            bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve create", "Loading Chart Component...", 30, True, True)
            bc_am_load_objects.obc_current_document = bc_am_load_objects.obc_current_document.read_data_from_file(bc_cs_central_settings.local_repos_path + name + ".dat")
            REM load entities
            bc_am_load_objects.obc_entities = bc_am_load_objects.obc_entities.read_data_from_file(bc_cs_central_settings.local_template_path + bc_am_load_objects.ENTITIES_FILENAME)
            REM load pub types
            bc_am_load_objects.obc_pub_types = bc_am_load_objects.obc_entities.read_data_from_file(bc_cs_central_settings.local_template_path + bc_am_load_objects.PUB_TYPES_FILENAME)
            REM if form authentication assign values
            'If bc_cs_central_settings.show_authentication_form = 1 Then
            '    bc_cs_central_settings.user_name = bc_am_load_objects.obc_current_document.logged_on_user_name
            '    bc_cs_central_settings.user_password = bc_am_load_objects.obc_current_document.logged_on_user_password
            'End If
            REM get locator
            locator = odoc.get_locator_for_display(True)
            If locator = "" Then
                bc_cs_central_settings.progress_bar.unload()
                omessage = New bc_cs_message("Blue Curve Create", "Component selected is not a chart.", bc_cs_message.MESSAGE)
                Exit Sub
            End If

            Dim i As Integer

            For i = 0 To bc_am_load_objects.obc_current_document.refresh_components.refresh_components.Count - 1
                bc_am_load_objects.obc_current_document.refresh_components.refresh_components(i).no_refresh = 1
                If bc_am_load_objects.obc_current_document.refresh_components.refresh_components(i).locator = locator Then
                    If bc_am_load_objects.obc_current_document.refresh_components.refresh_components(i).mode <> 3 Then
                        bc_cs_central_settings.progress_bar.unload()
                        omessage = New bc_cs_message("Blue Curve Create", "Component is not of type chart!", bc_cs_message.MESSAGE)
                        Exit Sub
                    End If
                    bc_am_load_objects.obc_current_document.refresh_components.refresh_components(i).no_refresh = 0
                    REM ING JUNE 2012
                    REM make sure it is always enabled as you are editting
                    bc_am_load_objects.obc_current_document.refresh_components.refresh_components(i).disabled = 0
                End If
            Next

            If bc_am_load_objects.obc_current_document.connection_method = bc_cs_central_settings.LOCAL Then
                ocommentary = New bc_cs_activity_log("bc_am_at_refresh", "new", bc_cs_activity_codes.COMMENTARY, "Cannot Edit Chart as System working Locally!")
                omessage = New bc_cs_message("Blue Curve Create", "Cannot Refresh as System working Locally!", bc_cs_message.MESSAGE)
            Else
                REM send refresh request object to server and get object response populates
                bc_cs_central_settings.progress_bar.increment("Retrieving Data For Component...")
                Dim did As String = ""
                If bc_am_load_objects.obc_current_document.id = 0 Then
                    did = bc_am_load_objects.obc_current_document.filename
                Else
                    bc_am_load_objects.obc_current_document.refresh_components.doc_id = bc_am_load_objects.obc_current_document.id
                End If
                bc_am_load_objects.obc_current_document.refresh_components.indexs_only = False
                If bc_am_load_objects.obc_current_document.connection_method = bc_cs_central_settings.ADO Then
                    ocommentary = New bc_cs_activity_log("bc_am_at_refresh", "new", bc_cs_activity_codes.COMMENTARY, "Refreshing via ADO!")
                    odoc.update_refresh_status_bar("Retrieving data via ADO...", 0, 0)
                    bc_am_load_objects.obc_current_document.refresh_components.doc_id = did
                    bc_am_load_objects.obc_current_document.refresh_components.language_id = bc_am_load_objects.obc_current_document.language_id
                    bc_am_load_objects.obc_current_document.refresh_components.pub_type_id = bc_am_load_objects.obc_current_document.pub_type_id
                    bc_am_load_objects.obc_current_document.refresh_components.db_read()
                ElseIf bc_am_load_objects.obc_current_document.connection_method = bc_cs_central_settings.SOAP Then
                    ocommentary = New bc_cs_activity_log("bc_am_at_refresh", "new", bc_cs_activity_codes.COMMENTARY, "Refreshing via SOAP!")
                    odoc.update_refresh_status_bar("Retrieving data via SOAP...", 0, 0)
                    bc_am_load_objects.obc_current_document.refresh_components.tmode = bc_cs_soap_base_class.tREAD
                    bc_am_load_objects.obc_current_document.refresh_components.transmit_to_server_and_receive(bc_am_load_objects.obc_current_document.refresh_components, True)
                Else
                    ocommentary = New bc_cs_activity_log("bc_am_at_refresh", "new", bc_cs_activity_codes.COMMENTARY, "Connection Method not supported!")
                    omessage = New bc_cs_message("Blue Curve Create", "Connection Method not supported!", bc_cs_message.MESSAGE)
                End If
                bc_cs_central_settings.progress_bar.increment("Refreshing Document Components...")
                REM populate document 
                odoc.populate_document(bc_am_load_objects.obc_current_document.refresh_components, True, True)

                For i = 0 To bc_am_load_objects.obc_current_document.refresh_components.refresh_components.Count - 1
                    If bc_am_load_objects.obc_current_document.refresh_components.refresh_components(i).locator = locator Then
                        bc_am_load_objects.obc_current_document.refresh_components.refresh_components(i).disabled = 1
                        odoc.update_parameter_values_to_document(bc_am_load_objects.obc_current_document.refresh_components, bc_am_load_objects.obc_current_document)
                        Exit For
                    End If
                Next

                REM save down metadata
                If bc_am_load_objects.obc_current_document.id > 0 Then
                    bc_am_load_objects.obc_current_document.write_data_to_file(bc_cs_central_settings.local_repos_path + CStr(bc_am_load_objects.obc_current_document.id) + ".dat")
                Else

                    REM SW cope with office versions
                    extensionsize = 0
                    extensionsize = (Len(bc_am_load_objects.obc_current_document.filename) - (InStrRev(bc_am_load_objects.obc_current_document.filename, ".") - 1))

                    If Right(bc_am_load_objects.obc_current_document.filename, extensionsize) <> bc_am_load_objects.obc_current_document.extension Then
                        bc_am_load_objects.obc_current_document.write_data_to_file(bc_cs_central_settings.local_repos_path + bc_am_load_objects.obc_current_document.filename + ".dat")
                    Else
                        bc_am_load_objects.obc_current_document.write_data_to_file(bc_cs_central_settings.local_repos_path + Left(bc_am_load_objects.obc_current_document.filename, Len(bc_am_load_objects.obc_current_document.filename) - extensionsize) + ".dat")
                    End If
                End If
                bc_cs_central_settings.progress_bar.unload()
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_at_edit_chart", "new", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_at_edit_chart", "new", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    REM loads relevant data for submit into memory
    'Private Function load_data() As Boolean
    '    Dim slog = New bc_cs_activity_log("bc_am_at_edit_chart", "load_data", bc_cs_activity_codes.TRACE_ENTRY, "")

    '    Try
    '        load_data = False
    '        Dim i As Integer
    '        'Dim obc_objects As New bc_am_load_objects
    '        REM check connection
    '        REM should be getting this from document
    '        bc_cs_central_settings.version = Application.ProductVersion
    '        'bc_cs_central_settings.selected_conn_method = bc_am_load_objects.obc_current_document.connection_method
    '        REM users
    '        bc_am_load_objects.obc_users = bc_am_load_objects.obc_users.read_data_from_file(bc_cs_central_settings.local_template_path + bc_am_load_objects.USERS_FILENAME)
    '        REM set logged on user
    '        For i = 0 To bc_am_load_objects.obc_users.user.Count - 1
    '            If bc_am_load_objects.obc_users.user(i).os_user_name = bc_cs_central_settings.logged_on_user_name Then
    '                bc_cs_central_settings.logged_on_user_id = bc_am_load_objects.obc_users.user(i).id
    '                Exit For
    '            End If
    '        Next
    '        REM entities
    '        bc_am_load_objects.obc_entities = bc_am_load_objects.obc_entities.read_data_from_file(bc_cs_central_settings.local_template_path + bc_am_load_objects.ENTITIES_FILENAME)
    '        REM pub types
    '        bc_am_load_objects.obc_pub_types = bc_am_load_objects.obc_pub_types.read_data_from_file(bc_cs_central_settings.local_template_path + bc_am_load_objects.PUB_TYPES_FILENAME)
    '        REM preferances
    '        bc_am_load_objects.obc_prefs = bc_am_load_objects.obc_pub_types.read_data_from_file(bc_cs_central_settings.local_template_path + bc_am_load_objects.PREFS_FILENAME)

    '        load_data = True
    '    Catch ex As Exception
    '        Dim db_err As New bc_cs_error_log("bc_am_at_edit_chart", "load_data", bc_cs_error_codes.USER_DEFINED, ex.Message)
    '    Finally
    '        slog = New bc_cs_activity_log("bc_am_at_edit_chart", "load_data", bc_cs_activity_codes.TRACE_EXIT, "")
    '    End Try

    'End Function

End Class
REM FIL 5.3
Public Class bc_am_at_build_params
    Dim ao_type As Integer
    Dim ao_object As Object
    Dim odoc As bc_ao_at_object
    Public odocmetadata As New bc_om_document
    Public success As Boolean = True
    'Public cancel As Boolean = False
    Public Sub New(Optional ByVal show_form As Boolean = True, Optional ByVal ao_object As Object = Nothing, Optional ByVal ao_type As String = "word", Optional ByVal refresh_type As Integer = 1, Optional ByVal show_progress As Boolean = True, Optional ByVal brecreate_metadata As Boolean = False, Optional ByVal evaluate_last_update_date As Boolean = False, Optional ByRef locked As Boolean = False, Optional ByVal partial_refresh_sp As String = "")

        Dim slog = New bc_cs_activity_log("bc_am_at_build_params", "new", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim omessage As bc_cs_message
        Dim name As String

        Dim ocommentary As bc_cs_activity_log
        Dim orefresh As New bc_om_refresh_components

        Try
            Dim bc_cs_central_settings As New bc_cs_central_settings(True)
            'If load_data() = False Then
            '    omessage = New bc_cs_message("Blue Curve create", "System Failed To Load Contact System Administrator!", bc_cs_message.MESSAGE)
            '    Exit Sub
            'End If
            REM instantiate correct AO object
            If ao_type = bc_ao_at_object.WORD_DOC Then

                odoc = New bc_ao_word(ao_object)
            ElseIf ao_type = bc_ao_at_object.POWERPOINT_DOC Then
                odoc = New bc_ao_powerpoint(ao_object)
            Else
                omessage = New bc_cs_message("Blue Curve Create", "Not Yet Implemented!", bc_cs_message.MESSAGE)
                Exit Sub
            End If
            REM get filename
            name = odoc.get_name
            If Left(name, 4) = "view" Then
                omessage = New bc_cs_message("Blue Curve create", "Document is view only cannot Change Parameters", bc_cs_message.MESSAGE)
                Exit Sub
            End If

            REM SW cope with office versions
            If Left(Right(name, 4), 1) = "." Then
                name = Left(name, Len(name) - 4)
            End If
            If Left(Right(name, 5), 1) = "." Then
                name = Left(name, Len(name) - 5)
            End If


            name = odoc.get_doc_id
            Dim fs As New bc_cs_file_transfer_services
            Dim recreate_flag As Boolean = True
            If fs.check_document_exists_and_can_be_read(bc_cs_central_settings.local_repos_path + name + ".dat") = False Then
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.LOCAL Then
                    omessage = New bc_cs_message("Blue Curve create", "Cannot Refresh Document as no network and document was not opened via Create.", bc_cs_message.MESSAGE)
                    Exit Sub
                End If
                REM xml metadata file doesnt exists so attempt to recreate
                Dim ordm As New bc_am_recreate_doc_metadata
                ordm.brecreate_from_pub_type = brecreate_metadata
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
            odoc.unlock_document()
            REM load XML metadata for file into document object
            bc_am_load_objects.obc_current_document = bc_am_load_objects.obc_current_document.read_data_from_file(bc_cs_central_settings.local_repos_path + name + ".dat")
            'REM load entities
            'bc_am_load_objects.obc_entities = bc_am_load_objects.obc_entities.read_data_from_file(bc_cs_central_settings.local_template_path + bc_am_load_objects.ENTITIES_FILENAME)
            'REM load pub types
            bc_am_load_objects.obc_pub_types = bc_am_load_objects.obc_entities.read_data_from_file(bc_cs_central_settings.local_template_path + bc_am_load_objects.PUB_TYPES_FILENAME)
            'REM if form authentication assign values
            'If bc_cs_central_settings.show_authentication_form = 1 Then
            '    bc_cs_central_settings.user_name = bc_am_load_objects.obc_current_document.logged_on_user_name
            '    bc_cs_central_settings.user_password = bc_am_load_objects.obc_current_document.logged_on_user_password
            'End If
            If bc_am_load_objects.obc_current_document.connection_method = bc_cs_central_settings.LOCAL Then
                ocommentary = New bc_cs_activity_log("bc_am_at_refresh", "new", bc_cs_activity_codes.COMMENTARY, "Cannot Change Parameters  as System working Locally!")
                omessage = New bc_cs_message("Blue Curve Create", "Cannot Change Parameters as System working Locally!", bc_cs_message.MESSAGE)
            Else
                REM HERE
                'Dim vparam As New view_bc_am_build_parameters

                'Dim cparam As New ctrll_bc_am_build_parameters(vparam, bc_am_load_objects.obc_current_document)
                'If cparam.load_data = True Then
                '    vparam.ShowDialog()
                'End If

                REM dev express
                Dim cparam As ctrll_bc_am_build_parameters
                Dim dxvparam As New view_bc_dx_build_parameters
                cparam = New ctrll_bc_am_build_parameters(dxvparam, bc_am_load_objects.obc_current_document)
                If cparam.load_data = True Then
                    dxvparam.ShowDialog()
                End If

                If cparam.cancel_selected = True Then
                    odoc.abort_doc()
                    If fs.check_document_exists(bc_cs_central_settings.local_repos_path + bc_am_load_objects.obc_current_document.filename + bc_am_load_objects.obc_current_document.extension) Then
                        fs.delete_file(bc_cs_central_settings.local_repos_path + bc_am_load_objects.obc_current_document.filename + bc_am_load_objects.obc_current_document.extension)
                    End If
                    If fs.check_document_exists(bc_cs_central_settings.local_repos_path + bc_am_load_objects.obc_current_document.filename + ".dat") Then
                        fs.delete_file(bc_cs_central_settings.local_repos_path + bc_am_load_objects.obc_current_document.filename + ".dat")
                    End If
                End If
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_at_build_params", "new", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            locked = bc_am_load_objects.obc_current_document.locked
            slog = New bc_cs_activity_log("bc_am_at_build_params", "new", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
End Class
REM ================
Public Class bc_am_at_component_refresh
    Dim ao_type As Integer
    Dim ao_object As Object
    Public odoc As bc_ao_at_object
    Public odocmetadata As New bc_om_document
    Public success As Boolean = True
    Public err_text As String = ""
    REM direct refresh when ao object is already known
    Public Sub New(_odoc As bc_ao_at_object)
        odoc = _odoc
    End Sub
    Public Sub New(ByVal ao_object As Object, ByVal ao_type As String, ByVal evaluate_last_update_date As Boolean)
        Dim slog = New bc_cs_activity_log("bc_am_at_refresh", "new short", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim omessage As bc_cs_message
            success = True
            REM instantiate correct AO object
            If ao_type = bc_ao_at_object.WORD_DOC Then
                odoc = New bc_ao_word(ao_object)
            ElseIf ao_type = bc_ao_at_object.POWERPOINT_DOC Then
                odoc = New bc_ao_powerpoint(ao_object)
            Else
                omessage = New bc_cs_message("Blue Curve Create", "Not Yet Implemented!", bc_cs_message.MESSAGE)
                Exit Sub
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_at_refresh", "new short", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_at_refresh", "new short", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    REM refresh from api
    Public Sub New(Optional ByVal show_form As Boolean = True, Optional ByVal ao_object As Object = Nothing, Optional ByVal ao_type As String = "word", Optional ByVal refresh_type As Integer = 1, Optional ByVal show_progress As Boolean = True, Optional ByVal brecreate_metadata As Boolean = False, Optional ByVal evaluate_last_update_date As Boolean = False, Optional ByRef locked As Boolean = False, Optional ByVal partial_refresh_sp As String = "")
        Dim slog = New bc_cs_activity_log("bc_am_at_refresh", "new", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim omessage As bc_cs_message
        Dim name As String

        Dim ocommentary As bc_cs_activity_log
        Dim orefresh As New bc_om_refresh_components

        Try
        

            Dim bc_cs_central_settings As New bc_cs_central_settings(True)
            'If load_data() = False Then
            '    omessage = New bc_cs_message("Blue Curve create", "System Failed To Load Contact System Administrator!", bc_cs_message.MESSAGE)
            '    Exit Sub
            'End If
            REM instantiate correct AO object
            If ao_type = bc_ao_at_object.WORD_DOC Then

                odoc = New bc_ao_word(ao_object)
            ElseIf ao_type = bc_ao_at_object.POWERPOINT_DOC Then
                odoc = New bc_ao_powerpoint(ao_object)
            Else
                omessage = New bc_cs_message("Blue Curve Create", "Not Yet Implemented!", bc_cs_message.MESSAGE)
                Exit Sub
            End If
            REM get filename
            name = odoc.get_name
            If Left(name, 4) = "view" Then
                omessage = New bc_cs_message("Blue Curve create", "Document is view only cannot Refresh", bc_cs_message.MESSAGE)
                Exit Sub
            End If

            REM SW cope with office versions
            If Left(Right(name, 4), 1) = "." Then
                name = Left(name, Len(name) - 4)
            End If
            If Left(Right(name, 5), 1) = "." Then
                name = Left(name, Len(name) - 5)
            End If


            name = odoc.get_doc_id
            Dim fs As New bc_cs_file_transfer_services
            Dim recreate_flag As Boolean = True
            If fs.check_document_exists_and_can_be_read(bc_cs_central_settings.local_repos_path + name + ".dat") = False Then
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.LOCAL Then
                    omessage = New bc_cs_message("Blue Curve create", "Cannot Refresh Document as no network and document was not opened via Create.", bc_cs_message.MESSAGE)
                    Exit Sub
                End If
                REM xml metadata file doesnt exists so attempt to recreate
                Dim ordm As New bc_am_recreate_doc_metadata
                ordm.brecreate_from_pub_type = brecreate_metadata
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
            odoc.unlock_document()
            REM load XML metadata for file into document object
            bc_am_load_objects.obc_current_document = bc_am_load_objects.obc_current_document.read_data_from_file(bc_cs_central_settings.local_repos_path + name + ".dat")
            'REM load entities
            'bc_am_load_objects.obc_entities = bc_am_load_objects.obc_entities.read_data_from_file(bc_cs_central_settings.local_template_path + bc_am_load_objects.ENTITIES_FILENAME)
            'REM load pub types
            'bc_am_load_objects.obc_pub_types = bc_am_load_objects.obc_entities.read_data_from_file(bc_cs_central_settings.local_template_path + bc_am_load_objects.PUB_TYPES_FILENAME)
            'REM if form authentication assign values
            'If bc_cs_central_settings.show_authentication_form = 1 Then
            '    bc_cs_central_settings.user_name = bc_am_load_objects.obc_current_document.logged_on_user_name
            '    bc_cs_central_settings.user_password = bc_am_load_objects.obc_current_document.logged_on_user_password
            'End If
            If bc_am_load_objects.obc_current_document.connection_method = bc_cs_central_settings.LOCAL Then
                ocommentary = New bc_cs_activity_log("bc_am_at_refresh", "new", bc_cs_activity_codes.COMMENTARY, "Cannot Refresh as System working Locally!")
                omessage = New bc_cs_message("Blue Curve Create", "Cannot Refresh as System working Locally!", bc_cs_message.MESSAGE)
            Else
                If show_progress = True Then
                    bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve create", "Loading Refresh Components...", 30, True, True)
                    bc_cs_central_settings.progress_bar.increment("Loading Refresh Components...")
                End If
                refresh(show_form, refresh_type, show_progress, bc_am_load_objects.obc_current_document, evaluate_last_update_date, partial_refresh_sp)
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_at_refresh", "new", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            locked = bc_am_load_objects.obc_current_document.locked
            slog = New bc_cs_activity_log("bc_am_at_refresh", "new", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Public Sub refresh(ByVal show_form As Boolean, ByVal refresh_type As Integer, ByVal show_progress As Boolean, ByVal ldoc As bc_om_document, ByVal evaluate_last_update_date As Boolean, Optional ByVal partial_refresh_sp As String = "", Optional do_not_write_metadata As Boolean = False)
        Dim slog = New bc_cs_activity_log("bc_am_at_refresh", "refresh", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim oname As String = ""
        Try
          
            odoc.unlock_document()
            oname = odoc.get_activedocument
            Dim ocommentary As bc_cs_activity_log
            Dim orefresh As New bc_om_refresh_components
            Dim omessage As bc_cs_message
            Dim i As Integer
            Dim extensionsize As Integer

            REM secondary entity
            Dim secondary_entity As Long
            secondary_entity = 0
            Try
                secondary_entity = odoc.get_property_value("rnet_secondary_entity_id")
            Catch

            End Try

            REM parse document to obtain collection of refresh components
            orefresh = odoc.load_refresh_components(False)
            REM consolidate this with metadata list TBD
            consolidate_refresh_list(orefresh)
            REM get last updated for each component from server
            If ldoc.refresh_components.refresh_components.Count > 0 Then
                If ldoc.refresh_components.refresh_components(0).original_markup_colour_index <> 0 Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Please hide component markup before refreshing!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub
                End If
            End If
            For i = 0 To ldoc.refresh_components.refresh_components.Count - 1
                With ldoc.refresh_components.refresh_components(i)
                    .last_update_date = "9-9-9999"
                    .no_refresh = 0
                    .parent_entity_id = secondary_entity
                End With
            Next
            REM test take this out when finished
            evaluate_last_update_date = True


            If evaluate_last_update_date = True Then
                If ldoc.connection_method = bc_cs_central_settings.ADO Then
                    ldoc.refresh_components.get_data_update_date()
                ElseIf ldoc.connection_method = bc_cs_central_settings.SOAP Then
                    ldoc.refresh_components.tmode = bc_om_refresh_components.tREAD_UPDATE_DATES
                    REM PR this shouldnt be here ldoc.refresh_components.transmit_to_server_and_receive(ldoc.refresh_components, False)
                    If ldoc.refresh_components.transmit_to_server_and_receive(ldoc.refresh_components, True) = False Then
                        Exit Sub
                        success = False
                    End If
                End If
                evaluate_refresh_required(ldoc)
            Else
                ocommentary = New bc_cs_activity_log("bc_am_at_component_refresh", "refresh", bc_cs_activity_codes.COMMENTARY, "Evaluate last data update date supressed!")
            End If


            If show_form = True Then
                REM set current selection values

                ldoc.refresh_components.oselection_parameters.value = ""
                ldoc.refresh_components.oselection_parameters.row = 0
                ldoc.refresh_components.oselection_parameters.col = 0
                If show_progress = True Then
                    bc_cs_central_settings.progress_bar.unload()
                End If
                'Dim orefreshfrm As New bc_am_component_refresh
                'orefreshfrm.ao_object = odoc
                'orefreshfrm.ShowDialog()
                'DX
                Dim dxrefreshform As New bc_ax_adv_doc_refresh
                Dim crefreshform As New Cbc_ax_adv_doc_refresh(dxrefreshform, ldoc)

                dxrefreshform.ShowDialog()
                If crefreshform.cancel_selected Then
                    Exit Sub
                End If

                'If orefreshfrm.highlight_selected = True Then
                ' MsgBox(ldoc.refresh_components.refresh_components(orefreshfrm.ListView3.SelectedItems(0).Index).locator)
                ' odoc.highlight_locator(ldoc.refresh_components.refresh_components(orefreshfrm.ListView3.SelectedItems(0).Index).locator)
                'Exit Sub
                'End If
                If crefreshform.cancel_selected = True Then
                    Exit Sub
                End If
                REM update property values
                odoc.update_parameter_values_to_document(ldoc.refresh_components, ldoc)
                If crefreshform.refresh_selected = False Then
                    Exit Sub
                End If
                REM save down metadata
                'If orefreshfrm.highlight_selected = True Then
                '    If ldoc.id > 0 Then
                '        ldoc.write_data_to_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ".dat")
                '    Else
                '        REM SW cope with office versions
                '        extensionsize = 0
                '        extensionsize = (Len(ldoc.filename) - (InStrRev(ldoc.filename, ".") - 1))

                '        If Right(ldoc.filename, extensionsize) <> ldoc.extension Then
                '            ldoc.write_data_to_file(bc_cs_central_settings.local_repos_path + ldoc.filename + ".dat")
                '        Else
                '            ldoc.write_data_to_file(bc_cs_central_settings.local_repos_path + Left(ldoc.filename, Len(ldoc.filename) - extensionsize) + ".dat")
                '        End If
                '    End If
                '    odoc.save()
                'End If
                'If orefreshfrm.ok_selected = False Then
                '    Exit Sub
                'End If
                If show_form = True Then
                    bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve create", "Retrieving Data For Components...", 30, True, True)
                End If
            End If
            With ldoc
                If .refresh_components.show_sdcs = True Then
                    odoc.disable_screen_updating()
                    For i = 0 To .refresh_components.refresh_components.Count - 1
                        If InStr(.refresh_components.refresh_components(i).locator, "_at") > 0 Then
                            If .refresh_components.refresh_components(i).original_markup_colour_index <> 0 Then
                                odoc.set_background_colour_for_locator(.refresh_components.refresh_components(i).locator, .refresh_components.refresh_components(i).original_markup_colour_index, .refresh_components.refresh_components(i).mode)
                                .refresh_components.refresh_components(i).original_markup_colour_index = 0
                                odoc.update_refresh_status_bar("unhighlight component ", CStr(i + 1), CStr(.refresh_components.refresh_components.Count))
                            End If
                        End If
                    Next
                    .refresh_components.show_sdcs = False
                    odoc.enable_screen_updating()
                End If
                If .refresh_components.show_udcs = True Then
                    odoc.disable_screen_updating()
                    For i = 0 To .refresh_components.refresh_components.Count - 1
                        If InStr(.refresh_components.refresh_components(i).locator, "_udc") > 0 Then
                            If .refresh_components.refresh_components(i).original_markup_colour_index <> 0 Then
                                odoc.set_background_colour_for_locator(.refresh_components.refresh_components(i).locator, .refresh_components.refresh_components(i).original_markup_colour_index, .refresh_components.refresh_components(i).mode)
                                .refresh_components.refresh_components(i).original_markup_colour_index = 0
                                odoc.update_refresh_status_bar("unhighlight component ", CStr(i + 1), CStr(.refresh_components.refresh_components.Count))
                            End If
                        End If
                    Next
                    .refresh_components.show_udcs = False
                    odoc.enable_screen_updating()
                End If
            End With

            REM send refresh request object to server and get object response populates
            If show_progress = True Then
                bc_cs_central_settings.progress_bar.increment("Retrieving Data For Components...")
            End If
            Dim did As String
            did = ldoc.id
            If ldoc.id = 0 Then
                did = ldoc.filename
            End If
            ldoc.refresh_components.component_documents = ldoc.composite_constiuents

            ldoc.refresh_components.refresh_type = refresh_type
            ldoc.refresh_components.language_id = ldoc.language_id
            ldoc.refresh_components.doc_id = did
            ldoc.refresh_components.pub_type_id = ldoc.pub_type_id
            ldoc.refresh_components.indexs_only = False
            ldoc.refresh_components.assoc_entities = ldoc.disclosures
            Dim login_name As String
            login_name = UCase(bc_cs_central_settings.GetLoginName)

            REM FIL JUNE 2013 partial component refresh
            If partial_refresh_sp <> "" Then
                ldoc.refresh_components.partial_component_refresh = True
                ldoc.refresh_components.partial_component_sp_list = partial_refresh_sp
            End If


            If ldoc.connection_method = bc_cs_central_settings.ADO Then
                ocommentary = New bc_cs_activity_log("bc_am_at_refresh", "new", bc_cs_activity_codes.COMMENTARY, "Refreshing via ADO!")
                odoc.update_refresh_status_bar("Retrieving data via ADO...", 0, 0)
                ldoc.refresh_components.db_read()
            ElseIf ldoc.connection_method = bc_cs_central_settings.SOAP Then
                ocommentary = New bc_cs_activity_log("bc_am_at_refresh", "new", bc_cs_activity_codes.COMMENTARY, "Refreshing via SOAP!")
                odoc.update_refresh_status_bar("Retrieving data via SOAP...", 0, 0)
                ldoc.refresh_components.certificate = New bc_cs_security.certificate
                ldoc.refresh_components.certificate.user_id = bc_cs_central_settings.logged_on_user_id
                If ldoc.refresh_components.certificate.user_id = "0" Then
                    For i = 0 To bc_am_load_objects.obc_users.user.Count - 1
                        If bc_cs_central_settings.authentication_method = 0 Then
                            If UCase(bc_am_load_objects.obc_users.user(i).os_user_name) = login_name Then
                                ldoc.refresh_components.certificate.user_id = bc_am_load_objects.obc_users.user(i).id
                                Exit For
                            End If
                        Else
                            If bc_am_load_objects.obc_users.user(i).id = bc_cs_central_settings.logged_on_user_id Then
                                ldoc.refresh_components.certificate.user_id = bc_am_load_objects.obc_users.user(i).id
                                Exit For
                            End If
                        End If

                    Next
                End If
                ldoc.refresh_components.tmode = bc_cs_soap_base_class.tREAD
                ldoc.refresh_components.success = False
                ldoc.refresh_components.transmit_to_server_and_receive(ldoc.refresh_components, True)

            Else
                ocommentary = New bc_cs_activity_log("bc_am_at_refresh", "new", bc_cs_activity_codes.COMMENTARY, "Connection Method not supported!")
                omessage = New bc_cs_message("Blue Curve Create", "Connection Method not supported!", bc_cs_message.MESSAGE)
            End If
            ldoc.refresh_components.partial_component_refresh = False
            ldoc.refresh_components.partial_component_sp_list = ""
            If show_progress = True Then
                bc_cs_central_settings.progress_bar.increment("Refreshing Document Components...")
            End If
            If ldoc.refresh_components.success = False Then
                Me.success = False
                Exit Sub
            End If
            If ldoc.refresh_components.calculating = True Then
                omessage = New bc_cs_message("Blue Curve", "Calculations are still in progress please refresh again later", bc_cs_message.MESSAGE)
                Exit Sub
            End If
            ldoc.disclosures = ldoc.refresh_components.assoc_entities
            REM if any type 1000 dynamic components produced please assign to refresh object
            assign_dynamic_components(ldoc.refresh_components, odoc)

            REM populate document 
            odoc.populate_document(ldoc.refresh_components, show_progress)

            Me.success = ldoc.refresh_components.success
            REM now do indexs
            REM indexs

            If any_index_components(ldoc) = True Then
                If show_form = True Then
                    bc_cs_central_settings.progress_bar.increment("Retrieving Index Information...")
                End If
                ldoc.refresh_components.indexs_only = True
                odoc.load_index_components(ldoc.refresh_components.index_components)
                REM request index info from server
                If ldoc.connection_method = bc_cs_central_settings.ADO Then
                    ocommentary = New bc_cs_activity_log("bc_am_at_refresh", "new", bc_cs_activity_codes.COMMENTARY, "Refreshing Indexs via ADO!")
                    odoc.update_refresh_status_bar("Retrieving data via ADO...", 0, 0)
                    ldoc.refresh_components.db_read()
                ElseIf ldoc.connection_method = bc_cs_central_settings.SOAP Then
                    ocommentary = New bc_cs_activity_log("bc_am_at_refresh", "new", bc_cs_activity_codes.COMMENTARY, "Refreshing Indexs via SOAP!")
                    odoc.update_refresh_status_bar("Retrieving data via SOAP...", 0, 0)
                    ldoc.refresh_components.tmode = bc_cs_soap_base_class.tREAD
                    ldoc.refresh_components.transmit_to_server_and_receive(ldoc.refresh_components, True)
                Else
                    ocommentary = New bc_cs_activity_log("bc_am_at_refresh", "new", bc_cs_activity_codes.COMMENTARY, "Connection Method not supported!")
                    omessage = New bc_cs_message("Blue Curve Create", "Connection Method not supported!", bc_cs_message.MESSAGE)
                End If
                If show_progress = True Then
                    bc_cs_central_settings.progress_bar.increment("Refreshing indexes...")
                End If
                odoc.populate_document(ldoc.refresh_components, show_progress)
            Else
                ocommentary = New bc_cs_activity_log("bc_am_at_refresh", "refresh", bc_cs_activity_codes.COMMENTARY, "No Index components in document so index generation suppressed.")
            End If
            odoc.remove_invalid_bookmarks(ldoc.refresh_components)

            If do_not_write_metadata = False Then
                REM save down metadata
                If ldoc.id > 0 Then
                    ldoc.write_data_to_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ".dat")
                Else

                    REM SW cope with office versions
                    extensionsize = 0
                    extensionsize = (Len(ldoc.filename) - (InStrRev(ldoc.filename, ".") - 1))

                    If Right(ldoc.filename, extensionsize) <> ldoc.extension Then
                        ldoc.write_data_to_file(bc_cs_central_settings.local_repos_path + ldoc.filename + ".dat")
                    Else
                        ldoc.write_data_to_file(bc_cs_central_settings.local_repos_path + Left(ldoc.filename, Len(ldoc.filename) - extensionsize) + ".dat")
                    End If
                End If
            End If


        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_at_refresh", "refresh", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            'If ldoc.locked = True Then
            '    odoc.lock_document()
            'Else
            '    odoc.unlock_document()
            'End If
            odoc.set_activedocument(oname)
            If show_progress = True Then
                Try
                    bc_cs_central_settings.progress_bar.unload()
                Catch
                End Try
            End If

            slog = New bc_cs_activity_log("bc_am_at_refresh", "new", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Private Sub assign_dynamic_components(ByRef orefresh As bc_om_refresh_components, ByVal odoc As bc_ao_at_object)
        Dim slog = New bc_cs_activity_log("bc_am_at_refresh", "assign_dynamic_components", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            For i = 0 To orefresh.refresh_components.Count - 1
                If orefresh.refresh_components(i).mode = 1000 Then

                    For j = 0 To orefresh.refresh_components(i).new_components.Count - 1
                        If j = 0 Then
                            If odoc.set_selection(orefresh.refresh_components(i).locator) = False Then
                                Dim ocomm As New bc_cs_activity_log("bc_am_at_component_refresh", "assign_dynamic_components", bc_cs_activity_codes.COMMENTARY, "Cant find bookmark: " + orefresh.refresh_components(i).locator)
                                Exit For
                            End If
                        Else
                            If odoc.set_next_selection(orefresh.refresh_components(j - 1).locator) = False Then
                                Dim ocomm As New bc_cs_activity_log("bc_am_at_component_refresh", "assign_dynamic_components", bc_cs_activity_codes.COMMENTARY, "Cant find bookmark: " + orefresh.refresh_components(j - 1).locator)
                                Exit For
                            End If


                        End If
                        REM goto bookmark
                        With orefresh.refresh_components(i).new_components(j)
                            .locator = odoc.set_locator_for_component(.type, .entity_id, .mode, True)
                            REM through a page break
                        End With
                        orefresh.refresh_components.Add(orefresh.refresh_components(i).new_components(j))
                    Next
                End If
                If orefresh.refresh_components(i).mode = 1001 Then

                    For j = 0 To orefresh.refresh_components(i).new_components.Count - 1
                        If j = 0 Then
                            If odoc.set_selection(orefresh.refresh_components(i).locator) = False Then
                                Dim ocomm As New bc_cs_activity_log("bc_am_at_component_refresh", "assign_dynamic_components", bc_cs_activity_codes.COMMENTARY, "Cant find bookmark: " + orefresh.refresh_components(i).locator)
                                Exit For
                            End If
                        Else
                            If odoc.set_next_selection(orefresh.refresh_components(j - 1).locator, False) = False Then
                                Dim ocomm As New bc_cs_activity_log("bc_am_at_component_refresh", "assign_dynamic_components", bc_cs_activity_codes.COMMENTARY, "Cant find bookmark: " + orefresh.refresh_components(j - 1).locator)
                                Exit For
                            End If
                        End If
                        REM goto bookmark
                        With orefresh.refresh_components(i).new_components(j)
                            .locator = odoc.set_locator_for_component(.type, .entity_id, .mode, True)
                            REM through a page break
                        End With
                        orefresh.refresh_components.Add(orefresh.refresh_components(i).new_components(j))
                    Next
                End If
            Next

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_at_refresh", "assign_dynamic_components", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_at_refresh", "assign_dynamic_components", bc_cs_activity_codes.TRACE_ENTRY, "")
        End Try
    End Sub

    Private Function any_index_components(ByVal ldoc As bc_om_document) As Boolean
        Dim i As Integer
        any_index_components = False
        For i = 0 To ldoc.refresh_components.refresh_components.Count - 1
            If ldoc.refresh_components.refresh_components(i).mode = 6 Or ldoc.refresh_components.refresh_components(i).mode = 26 Then
                any_index_components = True
                Exit Function
            End If
        Next
    End Function

    Private Sub consolidate_refresh_list(ByVal orefresh As bc_om_refresh_components)
        Dim slog = New bc_cs_activity_log("bc_am_at_refresh", "consolidate_refresh_list", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim i, j As Integer
            For i = 0 To bc_am_load_objects.obc_current_document.refresh_components.refresh_components.Count - 1
                With bc_am_load_objects.obc_current_document.refresh_components.refresh_components(i)
                    .locator_description = ""
                    For j = 0 To orefresh.refresh_components.Count - 1
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
            While i < bc_am_load_objects.obc_current_document.refresh_components.refresh_components.Count
                With bc_am_load_objects.obc_current_document.refresh_components.refresh_components(i)
                    If .locator_description = "" Then
                        bc_am_load_objects.obc_current_document.refresh_components.refresh_components.RemoveAt(i)
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

    Private Sub evaluate_refresh_required(ByRef ldoc As bc_om_document)
        Dim slog = New bc_cs_activity_log("bc_am_at_refresh", " evaluate_refresh_required", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim i As Integer

            For i = 0 To ldoc.refresh_components.refresh_components.Count - 1
                With ldoc.refresh_components.refresh_components(i)
                    .no_refresh = 0
                    If .last_update_date <> "9-9-9999" And .last_refresh_date <> "9-9-9999" Then
                        If .last_update_date <= .last_refresh_date Then
                            .no_refresh = 1
                        End If
                    End If
                End With
            Next

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_at_refresh", " evaluate_refresh_required(", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_at_refresh", "evaluate_refresh_required(", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
    REM loads relevant data for submit into memory
    'Private Function load_data() As Boolean
    '    Dim slog = New bc_cs_activity_log("bc_am_at_refresh", "load_data", bc_cs_activity_codes.TRACE_ENTRY, "")

    '    Try
    '        load_data = False
    '        Dim i As Integer
    '        Dim obc_objects As New bc_am_load_objects
    '        REM check connection
    '        REM should be getting this from document
    '        bc_cs_central_settings.version = Application.ProductVersion
    '        'bc_cs_central_settings.selected_conn_method = bc_am_load_objects.obc_current_document.connection_method
    '        REM users
    '        bc_am_load_objects.obc_users = bc_am_load_objects.obc_users.read_data_from_file(bc_cs_central_settings.local_template_path + bc_am_load_objects.USERS_FILENAME)
    '        REM set logged on user
    '        For i = 0 To bc_am_load_objects.obc_users.user.Count - 1
    '            If bc_am_load_objects.obc_users.user(i).os_user_name = bc_cs_central_settings.logged_on_user_name Then
    '                bc_cs_central_settings.logged_on_user_id = bc_am_load_objects.obc_users.user(i).id
    '                Exit For
    '            End If
    '        Next
    '        REM entities
    '        bc_am_load_objects.obc_entities = bc_am_load_objects.obc_entities.read_data_from_file(bc_cs_central_settings.local_template_path + bc_am_load_objects.ENTITIES_FILENAME)
    '        REM pub types
    '        bc_am_load_objects.obc_pub_types = bc_am_load_objects.obc_pub_types.read_data_from_file(bc_cs_central_settings.local_template_path + bc_am_load_objects.PUB_TYPES_FILENAME)
    '        REM preferances
    '        bc_am_load_objects.obc_prefs = bc_am_load_objects.obc_pub_types.read_data_from_file(bc_cs_central_settings.local_template_path + bc_am_load_objects.PREFS_FILENAME)

    '        load_data = True
    '    Catch ex As Exception
    '        Dim db_err As New bc_cs_error_log("bc_am_load_objects", "load_data", bc_cs_error_codes.USER_DEFINED, ex.Message)
    '    Finally
    '        slog = New bc_cs_activity_log("bc_am_at_refresh", "load_data", bc_cs_activity_codes.TRACE_EXIT, "")
    '    End Try

    'End Function
End Class
Public Class bc_am_edit_text_components
    Dim ao_type As Integer
    Dim ao_object As Object
    Public Shared odoc As bc_ao_word
    Public odocmetadata As New bc_om_document
    REM direct refresh when ao object is already known
    Public Sub New(ByVal ao_object As Object, ByVal ao_type As String)
        Dim slog = New bc_cs_activity_log("bc_am_edit_text_components", "new", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            REM load
            odoc = New bc_ao_word(ao_object)
            Dim omessage As bc_cs_message
            Dim name As String
            name = odoc.get_name
            If Left(name, 4) = "view" Then
                omessage = New bc_cs_message("Blue Curve create", "Document is view only edit text", bc_cs_message.MESSAGE)
                Exit Sub
            End If

            REM SW cope with office versions
            If Left(Right(name, 4), 1) = "." Then
                name = Left(name, Len(name) - 4)
            End If
            If Left(Right(name, 5), 1) = "." Then
                name = Left(name, Len(name) - 5)
            End If


            name = odoc.get_doc_id
            Dim fs As New bc_cs_file_transfer_services
            Dim recreate_flag As Boolean = True
            If fs.check_document_exists_and_can_be_read(bc_cs_central_settings.local_repos_path + name + ".dat") = False Then
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.LOCAL Then
                    omessage = New bc_cs_message("Blue Curve create", "Cannot Submit Document as no network and document was not opened via Create.", bc_cs_message.MESSAGE)
                    Exit Sub
                End If
                REM xml metadata file doesnt exists so attempt to recreate
                Dim ordm As New bc_am_recreate_doc_metadata
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
            odoc.unlock_document()

            REM read in existing text components
            Dim i As Integer

            For i = 0 To bc_am_load_objects.obc_current_document.refresh_components.refresh_components.Count - 1
                With bc_am_load_objects.obc_current_document.refresh_components.refresh_components(i)
                    If .mode = 8 Or .mode = 9 Then
                        .value = odoc.get_value_for_locator(.locator)
                    End If
                End With
            Next
            Dim t As New Thread(AddressOf dothetask)
            t.Start()
            REM being up interface
            'Dim owcf As New bc_am_text_components
            'owcf.ShowDialog()

            REM write down new components to document
            'For i = 0 To bc_am_load_objects.obc_current_document.refresh_components.refresh_components.Count - 1
            'With bc_am_load_objects.obc_current_document.refresh_components.refresh_components(i)
            'If .mode = 8 Or .mode = 9 Then
            REM odoc.set_value_for_locator(.locator, .value)
            'End If
            'End With
            'Next
            'odoc.save()
            'If bc_am_load_objects.obc_current_document.id <> 0 Then
            'bc_am_load_objects.obc_current_document.write_xml_to_file(bc_cs_central_settings.local_repos_path + CStr(bc_am_load_objects.obc_current_document.id) + ".dat")
            'Else
            'bc_am_load_objects.obc_current_document.write_xml_to_file(bc_cs_central_settings.local_repos_path + CStr(bc_am_load_objects.obc_current_document.filename) + ".dat")
            'End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_edit_text_components", "new", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            If bc_am_load_objects.obc_current_document.locked = True Then
                odoc.lock_document()
            Else
                odoc.unlock_document()
            End If
            slog = New bc_cs_activity_log("bc_am_edit_text_components", "new", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Public Sub dothetask()
        REM being up interface
        Dim owcf As New bc_am_text_components
        owcf.ShowDialog()
        If bc_am_load_objects.obc_current_document.locked = True Then
            odoc.lock_document()
        Else
            odoc.unlock_document()
        End If
    End Sub
End Class
Public Class bc_am_insert_components
    Public odoc As Object
    Public Sub New(ByVal show_form As Boolean, ByVal ao_object As Object, ByVal ao_type As String, ByVal component_id As Long)
        Dim slog = New bc_cs_activity_log("bc_am_insert_components", "new", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            Dim omessage As bc_cs_message
            Dim name As String
            Dim ldoc As New bc_om_document
            Dim recreate_flag As Boolean

            If ao_type <> bc_ao_at_object.WORD_DOC And ao_type <> bc_ao_at_object.POWERPOINT_DOC Then
                omessage = New bc_cs_message("Blue Curve create", "Application Object Type: " + ao_type + " not supported", bc_cs_message.MESSAGE)
                Exit Sub
            End If
            'If load_data() = False Then
            '    omessage = New bc_cs_message("Blue Curve create", "System Failed To Load Contact System Administrator!", bc_cs_message.MESSAGE)
            'Else
            REM instantiate correct AO object

            If ao_type = bc_ao_at_object.WORD_DOC Then
                odoc = New bc_ao_word(ao_object)
            End If
            If ao_type = bc_ao_at_object.POWERPOINT_DOC Then
                odoc = New bc_ao_powerpoint(ao_object)
            End If
            odoc.unlock_document()
            REM set local index
            REM get filename
            name = odoc.get_name
            If Left(name, 4) = "view" Then
                omessage = New bc_cs_message("Blue Curve create", "Document is view only cannot Submit", bc_cs_message.MESSAGE)
                Exit Sub
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
                    Exit Sub
                End If
                REM xml metadata file doesnt exists so attempt to recreate
                Dim ordm As New bc_am_recreate_doc_metadata
                If ordm.recreate_doc_metadata(name, odoc) = False Then
                    Exit Sub
                Else
                    ldoc = ldoc.read_data_from_file(bc_cs_central_settings.local_repos_path + name + ".dat")
                    recreate_flag = True
                End If
            Else
                REM load metadata
                ldoc = ldoc.read_data_from_file(bc_cs_central_settings.local_repos_path + name + ".dat")
            End If
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
                    orefresh.entity_id = ldoc.entity_id
                    orefresh.name = bc_am_load_objects.obc_templates.component_types.component_types(i).component_name
                    orefresh.locator_description = bc_am_load_objects.obc_templates.component_types.component_types(i).component_name
                    orefresh.mode = bc_am_load_objects.obc_templates.component_types.component_types(i).mode
                    orefresh.mode_param1 = bc_am_load_objects.obc_templates.component_types.component_types(i).sp_name
                    orefresh.mode_param2 = bc_am_load_objects.obc_templates.component_types.component_types(i).insert_object
                    orefresh.contributor_id = bc_am_load_objects.obc_templates.component_types.component_types(i).contributor
                    orefresh.refresh_type = bc_am_load_objects.obc_templates.component_types.component_types(i).refresh_type()
                    orefresh.type = component_id
                    For j = 0 To bc_am_load_objects.obc_templates.component_types.component_types(i).parameters.bc_om_component_parameters.count - 1
                        orefresh.parameters.component_template_parameters.Add(bc_am_load_objects.obc_templates.component_types.component_types(i).parameters.bc_om_component_parameters(j))
                    Next
                    ldoc.refresh_components.pub_type_id = ldoc.pub_type_id
                    ldoc.refresh_components.language_id = ldoc.language_id

                    orefresh.locator = odoc.set_locator_for_component(component_id, ldoc.entity_id, orefresh.mode)
                    ldoc.refresh_components.refresh_components.Add(orefresh)
                    ldoc.write_data_to_file(bc_cs_central_settings.local_repos_path + name + ".dat")
                    Dim ocomprefresh As New bc_am_at_change_component_params(show_form, ao_object, ao_type)
                End If
            Next
            If found = False Then
                omessage = New bc_cs_message("Blue Curve", "Component Id: " + CStr(component_id) + " doesnt exist in system.", bc_cs_message.MESSAGE)
                Exit Sub
            End If
            'End If

            'End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_insert_components", "new", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_insert_components", "new", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    'Private Function load_data() As Boolean
    '    Dim slog = New bc_cs_activity_log("bc_am_insert_components", "load_data", bc_cs_activity_codes.TRACE_ENTRY, "")

    '    Try
    '        load_data = False

    '        Dim obc_objects As New bc_am_load_objects
    '        REM templates
    '        bc_am_load_objects.obc_templates = bc_am_load_objects.obc_pub_types.read_data_from_file(bc_cs_central_settings.local_template_path + bc_am_load_objects.TEMPLATES_FILENAME)
    '        load_data = True
    '    Catch ex As Exception
    '        Dim db_err As New bc_cs_error_log("bc_am_insert_components", "load_data", bc_cs_error_codes.USER_DEFINED, ex.Message)
    '    Finally
    '        slog = New bc_cs_activity_log("bc_am_insert_components", "load_data", bc_cs_activity_codes.TRACE_EXIT, "")
    '    End Try

    'End Function

End Class

Public Class bc_am_recreate_doc_metadata
    Public brecreate_from_pub_type As Boolean = False
    Public odoc As bc_ao_at_object
    Public Sub New()

    End Sub
    Public Function recreate_doc_metadata(ByVal name As String, ByVal odoc As Object) As Boolean

        Dim slog = New bc_cs_activity_log("bc_am_recreate_doc_metadata", "recreate_doc_metadata", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim valid_user As Boolean = False
        Dim checked_out_user As String
        Dim msg As String = ""
        Dim i As Integer
        Dim tid As String = ""
        Try
            recreate_doc_metadata = False
            'If recreate_doc_security() = False Then
            'Exit Function
            'End If
            Dim ocommentary As bc_cs_activity_log
            Dim bc_cs_central_settings As New bc_cs_central_settings(False)
            REM need to check connection
            Dim soap_connection As Boolean = True
            If bc_cs_central_settings.connection_method = bc_cs_central_settings.SOAP Then
                Dim lsoapconn As New bc_cs_test_soap_server_connection
                If lsoapconn.transmit_to_server_and_receive(lsoapconn, False) = False Then
                    If lsoapconn.transmission_state = 3 Then
                        soap_connection = False
                    End If
                End If
            End If
            If bc_cs_central_settings.check_connection(bc_cs_central_settings.connection_method, soap_connection) = True Then
                bc_am_load_objects.obc_current_document = New bc_om_document
                REM set logged on user from lead analyst property
                bc_cs_central_settings.logged_on_user_id = odoc.get_property_value("rnet_analyst_id")
                If IsNumeric(name) Then
                    REM document has been submitted so reqeust metadata from server
                    REM based on last submitted metadata
                    tid = name
                    bc_am_load_objects.obc_current_document.id = name
                    bc_am_load_objects.obc_current_document.bwith_document = False
                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        ocommentary = New bc_cs_activity_log("bc_am_at_submit", "recreate_doc_metadata", bc_cs_activity_codes.COMMENTARY, "Recreating Metadata file via ADO.")
                        Dim gbc As New bc_cs_db_services
                        bc_am_load_objects.obc_current_document.db_read_for_create()
                    ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                        ocommentary = New bc_cs_activity_log("bc_am_at_submit", "recreate_doc_metadata", bc_cs_activity_codes.COMMENTARY, "Recreating Metadata file via SOAP.")
                        bc_am_load_objects.obc_current_document.tmode = bc_cs_soap_base_class.tREAD
                        bc_am_load_objects.obc_current_document.read_mode = bc_om_document.READ_FOR_CREATE

                        bc_am_load_objects.obc_current_document.transmit_to_server_and_receive(bc_am_load_objects.obc_current_document, True)
                    End If
                    'PR JAN 2016
                    bc_am_load_objects.obc_current_document.bwith_document = True
                    'END
                    REM check if user has document checked out
                    If bc_am_load_objects.obc_current_document.checked_out_user = 0 Then
                        msg = "Document is checked in or View Only Cannot Submit/Refresh! Please open from System."
                    Else
                        If (bc_cs_central_settings.show_authentication_form = 0 Or bc_cs_central_settings.show_authentication_form = 2) Then
                            If bc_am_load_objects.obc_current_document.checked_out_user = bc_cs_central_settings.logged_on_user_id Then
                                valid_user = True
                            End If
                        Else
                            For i = 0 To bc_am_load_objects.obc_users.user.Count - 1
                                With bc_am_load_objects.obc_users.user(i)
                                    If UCase(.user_name) = UCase(bc_cs_central_settings.user_name) Then
                                        If .id = bc_am_load_objects.obc_current_document.checked_out_user Then
                                            valid_user = True
                                            Exit For
                                        End If
                                    End If
                                End With
                            Next
                        End If
                        For i = 0 To bc_am_load_objects.obc_users.user.Count - 1
                            With bc_am_load_objects.obc_users.user(i)
                                If .id = bc_am_load_objects.obc_current_document.checked_out_user And bc_cs_central_settings.logged_on_user_id <> .id Then
                                    checked_out_user = .user_name
                                    msg = "Document is checked out by: " + checked_out_user + " cannot Submit!"
                                    Exit For
                                End If
                            End With
                        Next
                    End If
                    If valid_user = False Then
                        Dim omessage As New bc_cs_message("Blue Curve create", msg, bc_cs_message.MESSAGE)
                        Exit Function
                    End If
                Else
                    ocommentary = New bc_cs_activity_log("bc_am_at_submit", "recreate_doc_metadata", bc_cs_activity_codes.COMMENTARY, "Recreating Metadata from Document as never been submitted.")


                    recreate_doc_metadata = recreate_non_submitted_document(name, odoc.get_extension)

                    Exit Function
                End If

                bc_am_load_objects.obc_current_document.local_index = -1

                bc_am_load_objects.obc_current_document.connection_method = bc_cs_central_settings.selected_conn_method
                'If bc_cs_central_settings.show_authentication_form = 1 Then
                '    bc_am_load_objects.obc_current_document.logged_on_user_name = bc_cs_central_settings.user_name
                '    bc_am_load_objects.obc_current_document.logged_on_user_password = bc_cs_central_settings.user_password
                'End If

                bc_am_load_objects.obc_current_document.id = tid
                bc_am_load_objects.obc_current_document.write_data_to_file(bc_cs_central_settings.local_repos_path + name + ".dat")

                recreate_doc_metadata = True
                bc_cs_central_settings.progress_bar.unload()
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_recreate_doc_metadata", "recreate_doc_metadata", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            REM ING JUNE 2012
            If IsNumeric(tid) Then
                bc_am_load_objects.obc_current_document.id = tid
            End If
            slog = New bc_cs_activity_log("bc_am_recreate_doc_metadata", "recreate_doc_metadata", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
    Private Function recreate_non_submitted_document(ByVal name As String, extension As String) As Boolean
        REM only do this on a document that has a non submitted OM where the Om has changed
        REM dont do it if OM has changed
        Dim slog As New bc_cs_activity_log("bc_am_recreate_doc_metadata", "recreate_non_submitted_document", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            Dim fs As New bc_cs_file_transfer_services
            recreate_non_submitted_document = False
            If fs.check_document_exists_and_can_be_read(bc_cs_central_settings.local_repos_path + name + ".dat") = False Then
                If fs.updated_om = True Then
                    Dim omessage As New bc_cs_message("Blue Curve Create", "Document was created before system upgrade and never submitted and so cannot be submitted now. Please recreate the document again and copy and paste from this one.", bc_cs_message.MESSAGE)
                Else
                    If Me.brecreate_from_pub_type = False Then
                        Dim omessage As New bc_cs_message("Blue Curve Create", "Document has already been submitted, this is a locally saved copy please open system copy and remove this one.", bc_cs_message.MESSAGE)
                    Else
                        REM load system
                        Dim bc_cs_central_settings As New bc_cs_central_settings(True)
                        Dim bc_am_load As New bc_am_load("Create")
                        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.LOCAL Or bc_am_load.bdataloaded = False Then
                            Dim omsg As New bc_cs_message("Blue Curve", "Cannot work on document as you are not an authenticated system user or system system is of line!", bc_cs_message.MESSAGE)
                            Exit Function
                        End If
                        If recreate_from_pub_type(extension) = False Then
                            Dim omessage As New bc_cs_message("Blue Curve Create", "Failed to recrate document metadata on unsubmitted document.", bc_cs_message.MESSAGE)
                        Else
                            recreate_non_submitted_document = True
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_recreate_doc_metadata", "recreate_non_submitted_document", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

            slog = New bc_cs_activity_log("bc_am_recreate_doc_metadata", "recreate_non_submitted_document", bc_cs_activity_codes.TRACE_EXIT, "")


        End Try
    End Function
    Private Sub set_doc_metadata(ByVal pt As Long, ByRef ndoc As bc_om_document)
        Dim i As Integer

        REM get language id from pub type
        For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
            If bc_am_load_objects.obc_pub_types.pubtype(i).id = pt Then
                ndoc.refresh_components.language_id = ndoc.language_id
                ndoc.pub_type_name = bc_am_load_objects.obc_pub_types.pubtype(i).name
                ndoc.pub_type_id = bc_am_load_objects.obc_pub_types.pubtype(i).id
                ndoc.bus_area = bc_am_load_objects.obc_pub_types.pubtype(i).bus_area_Id
                ndoc.refresh_components.workflow_state = bc_am_load_objects.obc_pub_types.pubtype(i).default_financial_workflow_stage
                ndoc.refresh_components.accounting_standard = 1
                ndoc.stage = bc_am_load_objects.obc_pub_types.pubtype(i).workflow.stages(0).stage_id
                ndoc.stage_name = bc_am_load_objects.obc_pub_types.pubtype(i).workflow.stages(0).stage_name
                ndoc.workflow_stages = bc_am_load_objects.obc_pub_types.pubtype(i).workflow
                ndoc.language_id = bc_am_load_objects.obc_pub_types.pubtype(i).language
                Exit For
            End If
        Next
    End Sub
    Private Function recreate_from_pub_type(extension) As Boolean
        Dim slog As New bc_cs_activity_log("bc_am_recreate_doc_metadata", "recreate_from_pub_type", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim i, j, l As Integer
            bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve", "Recreating document metadata", 0, False, True)
            bc_cs_central_settings.progress_bar.increment("Recreating document metadata")
            recreate_from_pub_type = False
            REM this is called when the apu has specified that if the document has 
            REM never been submitted and their is no metadata file then to go ahead and recrate the
            REM the metadata file from scratch
            Dim ndoc As New bc_om_document
            ndoc.filename = odoc.get_doc_id
            ndoc.id = 0
            ndoc.pub_type_id = odoc.get_property_value("rnet_pub_type_id")
            ndoc.entity_id = odoc.get_property_value("rnet_entity_id")
            Dim otaxonomy As New bc_om_taxonomy
            otaxonomy.entity_id = ndoc.entity_id
            ndoc.taxonomy.Add(otaxonomy)
            ndoc.extension = extension
            Dim ouser As New bc_om_user
            ouser.id = odoc.get_property_value("rnet_analyst_id")
            ndoc.authors.Add(ouser)
            ndoc.originating_author = odoc.get_property_value("rnet_analyst_id")
            ndoc.refresh_components.language_id = 1
            ndoc.master_flag = True
            ndoc.doc_date = Now
            ndoc.connection_method = bc_cs_central_settings.selected_conn_method

            REM assign workflow stages from first stage
            REM assign components based on template
            ndoc.refresh_components = New bc_om_refresh_components
            ndoc.refresh_components = odoc.load_refresh_components(False)
            set_doc_metadata(ndoc.pub_type_id, ndoc)
            REM iterate populating the parameters for the component
            bc_am_load_objects.obc_templates = bc_am_load_objects.obc_templates.read_data_from_file(bc_cs_central_settings.local_template_path + bc_am_load_objects.TEMPLATES_FILENAME)
            bc_cs_central_settings.progress_bar.increment("Recreating document metadata")

            For i = 0 To ndoc.refresh_components.refresh_components.Count - 1
                For j = 0 To bc_am_load_objects.obc_templates.component_types.component_types.Count - 1
                    If ndoc.refresh_components.refresh_components(i).type = bc_am_load_objects.obc_templates.component_types.component_types(j).component_id Then
                        REM recreate refresh components
                        With bc_am_load_objects.obc_templates.component_types.component_types(j)
                            ndoc.refresh_components.refresh_components(i).name = .component_name
                            ndoc.refresh_components.refresh_components(i).contributor_id = 1
                            ndoc.refresh_components.refresh_components(i).mode = .mode
                            ndoc.refresh_components.refresh_components(i).mode_param1 = .sp_name
                            ndoc.refresh_components.refresh_components(i).mode_param2 = .insert_object
                            ndoc.refresh_components.refresh_components(i).last_refresh_date = "9-9-9999"
                            ndoc.refresh_components.refresh_components(i).refresh_type = .refresh_type
                            REM now do parameters
                            For l = 0 To .parameters.bc_om_component_parameters.count - 1
                                REM if type 7 update real time list
                                If .parameters.bc_om_component_parameters(l).datatype = 7 Then
                                    populate_lookup_List(ndoc.entity_id, .parameters.bc_om_component_parameters(l))
                                End If
                                Dim np As New bc_om_component_parameter
                                np.class_id = .parameters.bc_om_component_parameters(l).class_id
                                np.datatype = .parameters.bc_om_component_parameters(l).datatype
                                np.default_value = .parameters.bc_om_component_parameters(l).default_value
                                np.default_value_id = .parameters.bc_om_component_parameters(l).default_value_id
                                np.lookup_sql = .parameters.bc_om_component_parameters(l).lookup_sql
                                np.lookup_values = .parameters.bc_om_component_parameters(l).lookup_values
                                np.lookup_values_ids = .parameters.bc_om_component_parameters(l).lookup_values_ids
                                np.name = .parameters.bc_om_component_parameters(l).name
                                np.order = .parameters.bc_om_component_parameters(l).order
                                np.system_defined = .parameters.bc_om_component_parameters(l).system_defined
                                np.original_system_defined = .parameters.bc_om_component_parameters(l).original_system_defined

                                ndoc.refresh_components.refresh_components(i).parameters.component_template_parameters.Add(np)
                            Next
                        End With

                        Exit For
                    End If
                Next
            Next
            odoc.update_parameter_values_from_document(ndoc.refresh_components, ndoc)

            For i = 0 To ndoc.refresh_components.refresh_components.Count - 1
                For l = 0 To ndoc.refresh_components.refresh_components(i).parameters.component_template_parameters.count - 1
                    With ndoc.refresh_components.refresh_components(i).parameters.component_template_parameters(l)
                        If .datatype = 9 Then
                            populate_lookup_List(ndoc.entity_id, ndoc.refresh_components.refresh_components(i).parameters.component_template_parameters(l), ndoc.filename, ndoc.refresh_components.refresh_components(i).parameters.component_template_parameters(0).default_value_id, 9)
                        End If
                    End With
                Next

            Next

            ndoc.write_data_to_file(bc_cs_central_settings.local_repos_path + ndoc.filename + ".dat")
            bc_am_load_objects.obc_current_document = ndoc
            recreate_from_pub_type = True
            bc_cs_central_settings.progress_bar.increment("Recreating document metadata")

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_recreate_doc_metadata", "recreate_from_pub_type", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            bc_cs_central_settings.progress_bar.unload()
            slog = New bc_cs_activity_log("bc_am_recreate_doc_metadata", "recreate_from_pub_type", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function
    Private Sub populate_lookup_List(ByVal entity_id As Long, ByRef comp As bc_om_component_parameter, Optional ByVal doc_id As String = "", Optional ByVal context_id As String = "", Optional ByVal type_id As Integer = 0)
        Dim oparam_values As New bc_om_parameter_lookup
        oparam_values.lookup_sql = comp.lookup_sql
        oparam_values.entity_id = entity_id
        oparam_values.type_id = type_id
        oparam_values.doc_id = doc_id
        If IsNumeric(context_id) Then
            oparam_values.context_id = context_id
        End If
        oparam_values.doc_id = doc_id
        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
            oparam_values.db_read()
        Else
            oparam_values.tmode = bc_cs_soap_base_class.tREAD
            oparam_values.transmit_to_server_and_receive(oparam_values, True)
        End If
        comp.lookup_values.Clear()
        comp.lookup_values_ids.Clear()
        comp.lookup_values = oparam_values.lookup_vals
        comp.lookup_values_ids = oparam_values.lookup_vals_ids
        Try
            comp.context_items = oparam_values.context_items
        Catch
            MsgBox("failed")
        End Try
    End Sub


    Public Function recreate_doc_security() As Boolean
        Dim slog = New bc_cs_activity_log("bc_am_recreate_doc_metadata", "recreate_doc_security", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim msg As String = ""
        Try
            Dim ocommentary As bc_cs_activity_log
            Dim i As Integer
            recreate_doc_security = False
            REM OS Authentication
            If (bc_cs_central_settings.show_authentication_form = 1 Or bc_cs_central_settings.show_authentication_form = 3) Then
                REM prompt for user credentials
                Dim bc_cs_logon As New bc_cs_logon_form
                bc_cs_logon.ShowDialog()
                If bc_cs_logon.cancel = True Then
                    recreate_doc_security = False
                    Exit Function
                End If
                bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve create", "Authenticating User", 10, False, True)
                bc_cs_central_settings.progress_bar.increment("Authenticating User...")

                REM check if this matches user id
                msg = "invalid user"
                For i = 0 To bc_am_load_objects.obc_users.user.Count - 1
                    With bc_am_load_objects.obc_users.user(i)
                        If UCase(.user_name) = UCase(bc_cs_central_settings.user_name) Then
                            msg = "invalid password"
                            If UCase(.password) = UCase(bc_cs_central_settings.user_password) Then
                                recreate_doc_security = True
                                msg = ""
                            End If
                            Exit For
                        End If
                    End With
                Next
            Else
                recreate_doc_security = True
            End If
            If recreate_doc_security() = False Then
                bc_cs_central_settings.progress_bar.unload()
                ocommentary = New bc_cs_activity_log("bc_am_at_submit", "recreate_doc_security", bc_cs_activity_codes.COMMENTARY, "Authentication Failed: " + msg)
                Dim omessage As New bc_cs_message("Blue Curve create", "Document cannot be submitted: " + msg, bc_cs_message.MESSAGE)
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_recreate_doc_metadata", "recreate_doc_security", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

            slog = New bc_cs_activity_log("bc_am_recreate_doc_metadata", "recreate_doc_security", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function
End Class