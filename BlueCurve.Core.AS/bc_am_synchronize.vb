Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports System.Windows.Forms
REM ==========================================
REM Blue Curve Limited 2005
REM Module:       System Client Side Synchronization
REM Type:         Application Module
REM Description:  Takes local working data from database to local area
REM               Read from file
REM Version:      1.0
REM Change history
REM ==========================================
Public Class bc_am_synchronize
    Sub diff_gram_sync_entities()
        Try
            Dim fn As String
            fn = bc_cs_central_settings.local_template_path + bc_am_load_objects.ENTITIES_FILENAME

            Dim nent As New bc_om_entities
            Dim fs As New bc_cs_file_transfer_services
            If fs.check_document_exists_and_can_be_read(fn) Then
                bc_am_load_objects.obc_entities = bc_am_load_objects.obc_entities.read_data_from_file(fn)
                nent.last_synced_date = bc_am_load_objects.obc_entities.last_synced_date
                nent.diffgram_mode = True
            Else
                REM never been synced
                nent.diffgram_mode = False
            End If

            nent.from_sync = True


            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                nent.db_read()
            Else
                nent.tmode = bc_cs_soap_base_class.tREAD
                If nent.transmit_to_server_and_receive(nent, True) = False Then
                    Exit Sub
                End If
            End If
            nent.from_sync = False
            If nent.diffgram_mode = False Then
                bc_am_load_objects.obc_entities = nent
                bc_am_load_objects.obc_entities.last_synced_date = Now.ToUniversalTime
                bc_am_load_objects.obc_entities.write_data_to_file(fn)
                Exit Sub
            End If

            If nent.entity.Count = 0 Then
                REM all up to date
                Dim ocommm As New bc_cs_activity_log("bc_am_synchronize", "diff_gram_sync_entities", bc_cs_activity_codes.COMMENTARY, "Entities are up to date")
                bc_am_load_objects.obc_entities.filter_attributes_types = nent.filter_attributes_types
                bc_am_load_objects.obc_entities.disclosure_entities = nent.disclosure_entities
                bc_am_load_objects.obc_entities.write_data_to_file(fn)
                Exit Sub
            End If

            Dim ocomm As New bc_cs_activity_log("bc_am_synchronize", "diff_gram_sync_entities", bc_cs_activity_codes.COMMENTARY, CStr(nent.entity.Count) + " entity changes")
            REM remove entities in returned list
            Dim i As Integer
            While (i < bc_am_load_objects.obc_entities.entity.Count)
                For j = 0 To nent.entity.Count - 1
                    If bc_am_load_objects.obc_entities.entity(i).id = nent.entity(j).id Then
                        bc_am_load_objects.obc_entities.entity.RemoveAt(i)
                        i = i - 1
                        Exit For
                    End If
                Next
                i = i + 1
            End While
            REM add new ones that arnt inactive or deleted
            For j = 0 To nent.entity.Count - 1
                If (nent.entity(j).inactive = 0 And nent.entity(j).deleted = 0) Then
                    bc_am_load_objects.obc_entities.entity.Add(nent.entity(j))
                End If
            Next
            REM reapply sort class_id, name
            bc_am_load_objects.obc_entities.entity = bc_am_load_objects.obc_entities.entity.OrderBy(Function(c As bc_om_entity) c.class_id).ThenBy(Function(c As bc_om_entity) c.name).ToList()
            bc_am_load_objects.obc_entities.last_synced_date = Now.ToUniversalTime
            bc_am_load_objects.obc_entities.write_data_to_file(fn)
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_synchronize", "diff_gram_sync_entities", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try


    End Sub
    Public Function synchronize()
        synchronize = 1
    End Function
    REM FIL JUNE 2012
    Public Sub partial_sync(ByVal user As bc_om_user)
        Dim otrace As New bc_cs_activity_log("bc_am_at_synchronize", "partial_sync", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim ocommentary As bc_cs_activity_log
            Dim otemplates As bc_om_templates = Nothing
            For i = 0 To user.sync_settings.Count - 1
                If user.sync_settings(i) = 1 Then
                    bc_cs_central_settings.progress_bar.increment("Synchronizing Templates...")
                    Application.DoEvents()
                    sync_files(otemplates)

                ElseIf user.sync_settings(i) = 2 Then
                    bc_cs_central_settings.progress_bar.increment("Synchronizing Publications...")
                    Application.DoEvents()
                    sync_publications(otemplates)
                ElseIf user.sync_settings(i) = 3 Then
                    bc_cs_central_settings.progress_bar.increment("Synchronizing Entities...")
                    Application.DoEvents()

                    sync_entities()
                End If
            Next
            REM sync user preferance always
            bc_cs_central_settings.progress_bar.increment("Loading Preference Data...")
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                bc_am_load_objects.obc_prefs.tmode = bc_cs_soap_base_class.tREAD
                bc_am_load_objects.obc_prefs.transmit_to_server_and_receive(bc_am_load_objects.obc_prefs, True)
                ocommentary = New bc_cs_activity_log("bc_am_load", "new", bc_cs_activity_codes.COMMENTARY, "Loading User Preference Data via SOAP")
            Else
                bc_am_load_objects.obc_prefs.db_read()
            End If
            bc_am_load_objects.obc_prefs.write_data_to_file(bc_cs_central_settings.local_template_path + bc_am_load_objects.PREFS_FILENAME)
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_at_synchronize", "partial_sync", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_am_at_synchronize", "partial_sync", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    Public Sub sync_files(ByRef otemplates As bc_om_templates)
        Dim otrace As New bc_cs_activity_log("bc_am_at_synchronize", "sync_files", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim ocommentary As bc_cs_activity_log
            Dim strfn As String
            otemplates = New bc_om_templates

            If bc_cs_central_settings.selected_conn_method <> "ado" Then
                ocommentary = New bc_cs_activity_log("bc_am_at_synchronize", "new", bc_cs_activity_codes.COMMENTARY, "Loading Templates via SOAP")
                otemplates.tmode = bc_cs_soap_base_class.tREAD
                otemplates.transmit_to_server_and_receive(otemplates, True)

            Else
                REM templates
                ocommentary = New bc_cs_activity_log("bc_am_at_synchronize", "new", bc_cs_activity_codes.COMMENTARY, "Loading Templates from Database")
                otemplates.db_read()
            End If

            strfn = bc_cs_central_settings.local_template_path + bc_am_load_objects.PUB_TYPES_FILENAME

            strfn = bc_cs_central_settings.local_template_path + bc_am_load_objects.TEMPLATES_FILENAME

            Dim fs As New bc_cs_file_transfer_services
            ocommentary = New bc_cs_activity_log("bc_am_at_synchronize", "new", bc_cs_activity_codes.COMMENTARY, "Synchronising Files")
            Dim files_failed As Boolean = False
            Dim path As String = ""
            For i = 0 To otemplates.template.Count - 1
                Try
                    If otemplates.template(i).bytedoc.length > 0 AndAlso bc_cs_central_settings.local_template_path <> bc_cs_central_settings.central_template_path Then
                        REM FIL 5.5
                        Select Case otemplates.template(i).location_type
                            Case bc_om_template.destination_location_type.TEMPLATES
                                path = bc_cs_central_settings.local_template_path
                            Case bc_om_template.destination_location_type.WORD_STARUP
                                path = get_application_startup("word")
                            Case bc_om_template.destination_location_type.EXCEL_STARUP
                                path = get_application_startup("excel")
                            Case bc_om_template.destination_location_type.PATH
                                path = otemplates.template(i).path
                            Case Else
                                path = bc_cs_central_settings.local_template_path
                        End Select
                        If fs.write_bytestream_to_document_readonly(path + otemplates.template(i).filename, otemplates.template(i).bytedoc, Nothing, False) = False Then
                            files_failed = True
                        End If
                        REM ====
                        otemplates.template(i).bytedoc = Nothing
                    End If
                Catch

                End Try
            Next
            If files_failed = True Then
                set_pending_files_sync()
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_at_synchronize", "sync_files", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_am_at_synchronize", "sync_files", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    Private Function get_application_startup(ByVal application As String) As String
        Dim stem As String
        stem = Replace(bc_cs_central_settings.get_user_dir(), "\Bluecurve", "")
        If application = "word" Then
            get_application_startup = stem + "\Microsoft\word\startup\"
        ElseIf application = "excel" Then
            get_application_startup = stem + "\Microsoft\excel\xlstart\"
        Else
            get_application_startup = bc_cs_central_settings.local_template_path
        End If


    End Function

    Public Sub sync_publications(Optional ByVal otemplates As bc_om_templates = Nothing)

        Dim otrace As New bc_cs_activity_log("bc_am_at_synchronize", "sync_publications", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim ocommentary As bc_cs_activity_log
            Dim strfn As String
            Dim ierr As Integer
            Dim opub_types As New bc_om_pub_types
            Dim oprops As New bc_om_doc_properties


            If bc_cs_central_settings.selected_conn_method <> "ado" Then

                ocommentary = New bc_cs_activity_log("bc_am_at_synchronize", "new", bc_cs_activity_codes.COMMENTARY, "Loading Publication Types from via SOAP")
                opub_types.tmode = bc_cs_soap_base_class.tREAD
                opub_types.transmit_to_server_and_receive(opub_types, True)
                ocommentary = New bc_cs_activity_log("bc_am_at_synchronize", "new", bc_cs_activity_codes.COMMENTARY, "Loading Properties via SOAP")
                oprops.tmode = bc_cs_soap_base_class.tREAD
                oprops.transmit_to_server_and_receive(oprops, True)
                If IsNothing(otemplates) Then
                    otemplates = New bc_om_templates
                    otemplates.without_files = True
                    ocommentary = New bc_cs_activity_log("bc_am_at_synchronize", "new", bc_cs_activity_codes.COMMENTARY, "Loading Templates via SOAP")
                    otemplates.tmode = bc_cs_soap_base_class.tREAD
                    otemplates.transmit_to_server_and_receive(otemplates, True)
                End If

            Else
                REM read in directly from database
                REM pub types
                ocommentary = New bc_cs_activity_log("bc_am_at_synchronize", "new", bc_cs_activity_codes.COMMENTARY, "Loading Publication Types from Database")
                opub_types.db_read()
                REM properties
                ocommentary = New bc_cs_activity_log("bc_am_at_synchronize", "new", bc_cs_activity_codes.COMMENTARY, "Loading Properties from Database")
                oprops.db_read()
                REM templates
                If IsNothing(otemplates) Then
                    otemplates = New bc_om_templates
                    otemplates.without_files = True
                    ocommentary = New bc_cs_activity_log("bc_am_at_synchronize", "new", bc_cs_activity_codes.COMMENTARY, "Loading Templates from Database")
                    otemplates.db_read()
                End If
            End If

            strfn = bc_cs_central_settings.local_template_path + bc_am_load_objects.PUB_TYPES_FILENAME
            ocommentary = New bc_cs_activity_log("bc_am_at_synchronize", "new", bc_cs_activity_codes.COMMENTARY, "Writing Publication Types")
            ierr = opub_types.write_data_to_file(strfn)
            REM copy memory copy to global data area
            bc_am_load_objects.obc_pub_types = opub_types

            strfn = bc_cs_central_settings.local_template_path + bc_am_load_objects.TEMPLATES_FILENAME

            ocommentary = New bc_cs_activity_log("bc_am_at_synchronize", "new", bc_cs_activity_codes.COMMENTARY, "Writing Templates")
            REM copy memory copy to global data area
            bc_am_load_objects.obc_templates = otemplates

            ierr = otemplates.write_data_to_file(strfn)

            strfn = bc_cs_central_settings.local_template_path + bc_am_load_objects.PROPERTIES_FILENAME
            ocommentary = New bc_cs_activity_log("bc_am_at_synchronize", "new", bc_cs_activity_codes.COMMENTARY, "Writing Properties")
            ierr = oprops.write_data_to_file(strfn)
            REM copy memory copy to global data area
            bc_am_load_objects.obc_doc_properties = oprops

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_at_synchronize", "sync_publications", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_am_at_synchronize", "sync_publications", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Public Sub sync_entities()
        Dim otrace As New bc_cs_activity_log("bc_am_at_synchronize", "sync_entities", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim ocommentary As bc_cs_activity_log
            Dim strfn As String
            Dim ierr As Integer
            Dim oentities As New bc_om_entities
            Dim oentity_links As New bc_om_entity_links
            If bc_cs_central_settings.selected_conn_method <> "ado" Then
                ocommentary = New bc_cs_activity_log("bc_am_at_synchronize", "new", bc_cs_activity_codes.COMMENTARY, "Loading Entities via SOAP")
                'oentities.tmode = bc_cs_soap_base_class.tREAD
                'oentities.transmit_to_server_and_receive(oentities, True)
                diff_gram_sync_entities()

                ocommentary = New bc_cs_activity_log("bc_am_at_synchronize", "new", bc_cs_activity_codes.COMMENTARY, "Loading Entity Links via SOAP")
                oentity_links.tmode = bc_cs_soap_base_class.tREAD
                oentity_links.transmit_to_server_and_receive(oentity_links, True)
            Else
                REM entities
                'ocommentary = New bc_cs_activity_log("bc_am_at_synchronize", "new", bc_cs_activity_codes.COMMENTARY, "Loading Entities from Database")
                'oentities.db_read()
                diff_gram_sync_entities()
                REM entity links
                ocommentary = New bc_cs_activity_log("bc_am_at_synchronize", "new", bc_cs_activity_codes.COMMENTARY, "Loading Entity Links from Database")
                oentity_links.db_read()
            End If

            'strfn = bc_cs_central_settings.local_template_path + bc_am_load_objects.ENTITIES_FILENAME
            'ocommentary = New bc_cs_activity_log("bc_am_at_synchronize", "new", bc_cs_activity_codes.COMMENTARY, "Writing Entities")
            'ierr = oentities.write_data_to_file(strfn)
            'REM copy memory copy to global data area
            'bc_am_load_objects.obc_entities = oentities

            strfn = bc_cs_central_settings.local_template_path + bc_am_load_objects.ENTITY_LINKS_FILENAME
            ocommentary = New bc_cs_activity_log("bc_am_at_synchronize", "new", bc_cs_activity_codes.COMMENTARY, "Writing Entity Links")
            ierr = oentity_links.write_data_to_file(strfn)


        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_at_synchronize", "sync_entities", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_am_at_synchronize", "sync_entities", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Public Sub New()

    End Sub
    Public Sub full_sync()
        Dim otrace As New bc_cs_activity_log("bc_am_at_synchronize", "full_sync", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            Dim strfn As String
            Dim ierr As Integer
            Dim ocommentary As bc_cs_activity_log
            Dim i As Integer

            Dim opub_types As New bc_om_pub_types
            Dim oprops As New bc_om_doc_properties
            Dim otemplates As New bc_om_templates
            Dim oentities As New bc_om_entities
            Dim oentity_links As New bc_om_entity_links
            Dim oprefs As New bc_om_user_prefs
            Dim obc_om_insight_submission_entity_links As New bc_om_insight_submission_entity_links

            ocommentary = New bc_cs_activity_log("bc_am_at_synchronize", "new", bc_cs_activity_codes.COMMENTARY, "Starting Synchronize.")
            bc_cs_central_settings.progress_bar.increment("Loading Synchronize Data...")
            If bc_cs_central_settings.selected_conn_method <> "ado" Then
                REM rem read in via SOAP
                ocommentary = New bc_cs_activity_log("bc_am_at_synchronize", "new", bc_cs_activity_codes.COMMENTARY, "Loading Publication Types from via SOAP")
                opub_types.tmode = bc_cs_soap_base_class.tREAD
                opub_types.transmit_to_server_and_receive(opub_types, True)
                ocommentary = New bc_cs_activity_log("bc_am_at_synchronize", "new", bc_cs_activity_codes.COMMENTARY, "Loading Properties via SOAP")
                oprops.tmode = bc_cs_soap_base_class.tREAD
                oprops.transmit_to_server_and_receive(oprops, True)
                ocommentary = New bc_cs_activity_log("bc_am_at_synchronize", "new", bc_cs_activity_codes.COMMENTARY, "Loading Templates via SOAP")
                otemplates.tmode = bc_cs_soap_base_class.tREAD
                otemplates.transmit_to_server_and_receive(otemplates, True)
                ocommentary = New bc_cs_activity_log("bc_am_at_synchronize", "new", bc_cs_activity_codes.COMMENTARY, "Loading Entities via SOAP")
                'oentities.tmode = bc_cs_soap_base_class.tREAD
                'oentities.transmit_to_server_and_receive(oentities, True)
                diff_gram_sync_entities()

                ocommentary = New bc_cs_activity_log("bc_am_at_synchronize", "new", bc_cs_activity_codes.COMMENTARY, "Loading Entity Links via SOAP")
                oentity_links.tmode = bc_cs_soap_base_class.tREAD
                oentity_links.transmit_to_server_and_receive(oentity_links, True)
                ocommentary = New bc_cs_activity_log("bc_am_at_synchronize", "new", bc_cs_activity_codes.COMMENTARY, "Loading Prefs via SOAP")
                oprefs.tmode = bc_cs_soap_base_class.tREAD
                oprefs.transmit_to_server_and_receive(oprefs, True)
                '    ocommentary = New bc_cs_activity_log("bc_am_in_synchronize", "new", bc_cs_activity_codes.COMMENTARY, "Loading Insight Links via SOAP")
                '    obc_om_insight_submission_entity_links.tmode = bc_cs_soap_base_class.tREAD
                '    obc_om_insight_submission_entity_links.transmit_to_server_and_receive(obc_om_insight_submission_entity_links, True)
            Else
                REM read in directly from database
                REM pub types
                ocommentary = New bc_cs_activity_log("bc_am_at_synchronize", "new", bc_cs_activity_codes.COMMENTARY, "Loading Publication Types from Database")
                opub_types.db_read()
                REM properties
                ocommentary = New bc_cs_activity_log("bc_am_at_synchronize", "new", bc_cs_activity_codes.COMMENTARY, "Loading Properties from Database")
                oprops.db_read()
                REM templates
                ocommentary = New bc_cs_activity_log("bc_am_at_synchronize", "new", bc_cs_activity_codes.COMMENTARY, "Loading Templates from Database")
                otemplates.db_read()
                REM entities
                ocommentary = New bc_cs_activity_log("bc_am_at_synchronize", "new", bc_cs_activity_codes.COMMENTARY, "Loading Entities from Database")
                REM oentities.db_read()
                diff_gram_sync_entities()
                REM entity links
                ocommentary = New bc_cs_activity_log("bc_am_at_synchronize", "new", bc_cs_activity_codes.COMMENTARY, "Loading Entity Links from Database")
                oentity_links.db_read()
                REM prefs
                ocommentary = New bc_cs_activity_log("bc_am_at_synchronize", "new", bc_cs_activity_codes.COMMENTARY, "Loading Prefs from Database")
                oprefs.db_read()
                REM insight links
                'ocommentary = New bc_cs_activity_log("bc_am_in_synchronize", "new", bc_cs_activity_codes.COMMENTARY, "Loading Insight Links from Database")
                'obc_om_insight_submission_entity_links.db_read()
                'ocommentary = New bc_cs_activity_log("bc_am_in_synchronize", "new", bc_cs_activity_codes.COMMENTARY, "Load Complete from Database")
            End If
            REM inistatiate class to hold all object data
            Dim obcobjects As New bc_am_load_objects
            bc_cs_central_settings.progress_bar.increment("Saving Synchronize Data...")
            strfn = bc_cs_central_settings.local_template_path + bc_am_load_objects.PUB_TYPES_FILENAME
            ocommentary = New bc_cs_activity_log("bc_am_at_synchronize", "new", bc_cs_activity_codes.COMMENTARY, "Writing Publication Types")
            ierr = opub_types.write_data_to_file(strfn)
            REM copy memory copy to global data area
            bc_am_load_objects.obc_pub_types = opub_types

            strfn = bc_cs_central_settings.local_template_path + bc_am_load_objects.TEMPLATES_FILENAME
            ocommentary = New bc_cs_activity_log("bc_am_at_synchronize", "new", bc_cs_activity_codes.COMMENTARY, "Writing Templates")
            REM copy memory copy to global data area
            bc_am_load_objects.obc_templates = otemplates
            REM write down files
            i = 0
            Dim fs As New bc_cs_file_transfer_services
            ocommentary = New bc_cs_activity_log("bc_am_at_synchronize", "new", bc_cs_activity_codes.COMMENTARY, "Synchronising Files")
            Dim files_failed As Boolean = False
            Dim path As String = ""
            For i = 0 To otemplates.template.Count - 1
                Try
                    If otemplates.template(i).bytedoc.length > 0 AndAlso bc_cs_central_settings.local_template_path <> bc_cs_central_settings.central_template_path Then
                        REM FIL 5.5
                        Select Case otemplates.template(i).location_type
                            Case bc_om_template.destination_location_type.TEMPLATES
                                path = bc_cs_central_settings.local_template_path
                            Case bc_om_template.destination_location_type.WORD_STARUP
                                path = get_application_startup("word")
                            Case bc_om_template.destination_location_type.EXCEL_STARUP
                                path = get_application_startup("excel")
                            Case bc_om_template.destination_location_type.PATH
                                path = otemplates.template(i).path
                            Case Else
                                path = bc_cs_central_settings.local_template_path
                        End Select


                        If fs.write_bytestream_to_document_readonly(path + otemplates.template(i).filename, otemplates.template(i).bytedoc, Nothing, False) = False Then
                            files_failed = True
                        End If
                        REM ====
                        otemplates.template(i).bytedoc = Nothing
                    End If
                Catch

                End Try
            Next

            If files_failed = True Then

                set_pending_files_sync()
            End If

            ierr = otemplates.write_data_to_file(strfn)

            strfn = bc_cs_central_settings.local_template_path + bc_am_load_objects.PROPERTIES_FILENAME
            ocommentary = New bc_cs_activity_log("bc_am_at_synchronize", "new", bc_cs_activity_codes.COMMENTARY, "Writing Properties")
            ierr = oprops.write_data_to_file(strfn)
            REM copy memory copy to global data area
            bc_am_load_objects.obc_doc_properties = oprops

            'strfn = bc_cs_central_settings.local_template_path + bc_am_load_objects.ENTITIES_FILENAME
            'ocommentary = New bc_cs_activity_log("bc_am_at_synchronize", "new", bc_cs_activity_codes.COMMENTARY, "Writing Entities")
            'ierr = oentities.write_data_to_file(strfn)
            REM copy memory copy to global data area
            'bc_am_load_objects.obc_entities = oentities

            strfn = bc_cs_central_settings.local_template_path + bc_am_load_objects.ENTITY_LINKS_FILENAME
            ocommentary = New bc_cs_activity_log("bc_am_at_synchronize", "new", bc_cs_activity_codes.COMMENTARY, "Writing Entity Links")
            ierr = oentity_links.write_data_to_file(strfn)
            REM copy memory copy to global data area
            bc_am_load_objects.obc_entity_links = oentity_links

            strfn = bc_cs_central_settings.local_template_path + bc_am_load_objects.PREFS_FILENAME
            ocommentary = New bc_cs_activity_log("bc_am_at_synchronize", "new", bc_cs_activity_codes.COMMENTARY, "Writing Prefs")
            ierr = oprefs.write_data_to_file(strfn)
            REM copy memory copy to global data area
            bc_am_load_objects.obc_prefs = oprefs

            REM insight
            'If Not IsNothing(obc_om_insight_submission_entity_links) Then
            'strfn = bc_cs_central_settings.local_template_path + bc_am_load_objects.INSIGHT_LINKS_FILENAME
            'ocommentary = New bc_cs_activity_log("bc_am_at_synchronize", "new", bc_cs_activity_codes.COMMENTARY, "Writing Insight Links")
            'ierr = obc_om_insight_submission_entity_links.write_data_to_file(strfn)
            'REM copy memory copy to global data area
            'bc_am_load_objects.obc_om_insight_submission_entity_links = obc_om_insight_submission_entity_links
            ''End If

            ocommentary = New bc_cs_activity_log("bc_am_at_synchronize", "new", bc_cs_activity_codes.COMMENTARY, "Synchronize Complete.")

            If ierr = bc_cs_error_codes.RETURN_ERROR Then
                Err.Raise(bc_cs_error_codes.RETURN_ERROR, , "Error Writing Templats to XML file")
            End If

            REM check if local documents OM have changed and need resetting
            If fs.check_document_exists_and_can_be_read(bc_cs_central_settings.local_repos_path + "bc_local_documents.dat") = False Then
                If fs.updated_om = True Then
                    bc_cs_central_settings.progress_bar.change_caption("Realigning local documents")
                    Dim local_docs As New bc_om_documents
                    Dim oset_local_docs As New bc_am_get_checked_out_docs_from_server
                    oset_local_docs.set_from_server(local_docs)
                    local_docs.write_data_to_file(bc_cs_central_settings.local_repos_path + "bc_local_documents.dat")
                End If
            End If
            REM if local workbooks OM has changed
            If fs.check_document_exists_and_can_be_read(bc_cs_central_settings.local_repos_path + "bc_local_workbooks.dat") = False Then
                If fs.updated_om = True Then
                    bc_cs_central_settings.progress_bar.change_caption("Realigning local workbooks")
                    Dim local_workbooks As New bc_om_insight_contribution_for_entities
                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        local_workbooks.db_read_checked_out_to_me()
                    ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                        local_workbooks.tmode = bc_cs_soap_base_class.tREAD_ALL_CHECKED_OUT
                        local_workbooks.transmit_to_server_and_receive(local_workbooks.tmode, True)
                    Else
                        ocommentary = New bc_cs_activity_log("bc_am_at_open_doc", "update_local_docs", bc_cs_activity_codes.COMMENTARY, "Object model has changed but system is offline so local documents wont be updated")
                        Exit Sub
                    End If
                    local_workbooks.write_data_to_file(bc_cs_central_settings.local_repos_path + "bc_local_workbooks.dat")
                End If
            End If
            REM reread config file incase any have been synced across
            REM ING JUNE 2012
            Dim bcs As New bc_cs_central_settings(True)
            REM ----------------------
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_at_synchronize", "full_sync", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_am_at_synchronize", "full_sync", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Private Sub set_pending_files_sync()
        Try
            Dim omsg As New bc_cs_message("Blue Curve", "File synchorinze failed. Please close Blue Curve, Word and Excel and restart the application.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            Dim ocomm As New bc_cs_activity_log("Blue Curve", "sync_files", bc_cs_activity_codes.COMMENTARY, "File synchorinze failed. Please close Blue Curve, Word and Excel and restart the application.")

            Dim osync As New bc_om_set_partial_sync
            osync.type = 1
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                osync.db_write()
            Else
                osync.tmode = bc_cs_soap_base_class.tWRITE
                osync.transmit_to_server_and_receive(osync, True)
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_at_synchronize", "set_pending_files_sync(", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
End Class
Public Class bc_am_get_checked_out_docs_from_server
    Public Sub New()

    End Sub
    Public Sub set_from_server(ByRef docs As bc_om_documents)
        Dim otrace As New bc_cs_activity_log("bc_am_set_local_docs_from_server", "set_from_server", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim ocommentary As bc_cs_activity_log
            docs.document.Clear()
            REM recreate from server
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                docs.read_all_checked_out_documents()
            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                docs.tmode = bc_cs_soap_base_class.tREAD_ALL_CHECKED_OUT
                docs.transmit_to_server_and_receive(docs, True)
            Else
                ocommentary = New bc_cs_activity_log("bc_am_at_open_doc", "update_local_docs", bc_cs_activity_codes.COMMENTARY, "Object model has changed but system is offline so local documents wont be updated")
                Exit Sub
            End If
            REM check now if document actually exists of not pull the record
            Dim j As Integer
            Dim fn As String
            Dim fs As New bc_cs_file_transfer_services
            j = 0
            While j < docs.document.Count
                fn = bc_cs_central_settings.local_repos_path + CStr(docs.document(j).id) + docs.document(j).extension
                If fs.check_document_exists(fn, Nothing) = False Then
                    docs.document.RemoveAt(j)
                Else
                    REM get xml file updated for document
                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        docs.document(j).db_read()
                    ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                        docs.document(j).tmode = docs.document(j).tREAD
                        docs.document(j).transmit_to_server_and_receive(docs.document(j), True)
                    End If
                    docs.document(j).write_xml_to_file(bc_cs_central_settings.local_repos_path + CStr(docs.document(j).id) + ".dat")
                    j = j + 1
                End If
            End While
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_set_local_docs_from_server", "set_from_server", bc_cs_error_codes.USER_DEFINED, Err.Description)
        Finally
            otrace = New bc_cs_activity_log("bc_am_set_local_docs_from_server", "set_from_server", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
End Class

