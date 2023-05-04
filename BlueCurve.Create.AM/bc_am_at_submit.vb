Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports BlueCurve.Core.AS
Imports System.Collections
Imports System.Windows.Forms
Imports System.Runtime.InteropServices
REM ==========================================
REM Blue Curve Limited 2005
REM Module:        Submit
REM Type:         Application Module
REM Description:  Submits a Document
REM Version:      1
REM Change history
REM ==========================================
REM constructor determines whether to show the
REM submit form or not
Public Class bc_am_show_sdc_udc
    Dim ao_type As Integer
    Dim ao_object As Object
    Dim odoc As bc_ao_at_object
    Dim ldoc As New bc_om_document
    Dim show_udc As Boolean
    Dim show_sdc As Boolean
    Dim ofs As New bc_cs_file_transfer_services
    REM constructor determines whether to show the
    REM submit form or not the active object and the type
    Public Sub New(ByVal show_form As Boolean, ByVal ao_object As Object, ByVal ao_type As String, ByVal show_mode As Boolean, ByVal show_sdc As Boolean, ByVal show_udc As Boolean, Optional ByVal brecreate_metadata As Boolean = False, Optional ByVal sdc_colour_index As Long = 0, Optional ByVal udc_colour_index As Long = 0)
        Dim slog = New bc_cs_activity_log("bc_am_show_sdc_udc", "new", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim name As String
        Dim fs As New bc_cs_file_transfer_services
        Dim Connection_method As String = ""
        Dim local_docs As New bc_om_documents
        Dim original_local_index As Integer
        Dim new_name As String
        Dim recreate_flag As Boolean = False
        Dim noerr As Boolean = True
        Dim iname As String
        Dim originally_local As Boolean = False


        Try
            bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve create", "Initializing ...", 4, False, True)
            If show_mode = True Then
                bc_cs_central_settings.progress_bar.increment("Highlighting Components...")
            Else
                bc_cs_central_settings.progress_bar.increment("Hiding Component Highlighting...")
            End If

            Dim omessage As bc_cs_message
            If ao_type <> bc_ao_at_object.WORD_DOC And ao_type <> bc_ao_at_object.POWERPOINT_DOC Then
                omessage = New bc_cs_message("Blue Curve create", "Application Object Type: " + ao_type + " not supported", bc_cs_message.MESSAGE)
            Else
                If load_data() = False Then
                    omessage = New bc_cs_message("Blue Curve create", "System Failed To Load Contact System Administrator!", bc_cs_message.MESSAGE)
                Else
                    REM instantiate correct AO object
                    If ao_type = bc_ao_at_object.WORD_DOC Then
                        odoc = New bc_ao_word(ao_object)
                    End If
                    If ao_type = bc_ao_at_object.POWERPOINT_DOC Then
                        odoc = New bc_ao_powerpoint(ao_object)
                    End If
                    Me.show_sdc = show_sdc
                    Me.show_udc = show_udc
                    odoc.unlock_document()
                    REM set local index
                    original_local_index = -1
                    REM get filename
                    name = odoc.get_name
                    If Left(name, 4) = "view" Then
                        omessage = New bc_cs_message("Blue Curve create", "Document is view only cannot Submit", bc_cs_message.MESSAGE)
                        Exit Sub
                    End If
                    name = odoc.get_doc_id
                    iname = odoc.get_doc_id
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
                    End If

                    new_name = odoc.get_name
                    name = odoc.get_name

                    REM SW cope with office versions
                    If Left(Right(name, 4), 1) = "." Then
                        name = Left(name, Len(name) - 4)
                    End If
                    If Left(Right(name, 5), 1) = "." Then
                        name = Left(name, Len(name) - 5)
                    End If

                    REM load XML metadata for file into document object
                    Connection_method = ldoc.connection_method
                    bc_cs_central_settings.selected_conn_method = Connection_method
                    Dim bk_colour As String
                    odoc.disable_screen_updating()

                    If show_mode = True Then
                        For i = 0 To ldoc.refresh_components.refresh_components.Count - 1
                            bk_colour = odoc.get_background_color_for_locator(ldoc.refresh_components.refresh_components(i).locator)
                            If bk_colour <> sdc_colour_index And ldoc.refresh_components.refresh_components(i).original_markup_colour_index = 0 Then
                                ldoc.refresh_components.refresh_components(i).original_markup_colour_index = bk_colour
                                odoc.set_background_colour_for_locator(ldoc.refresh_components.refresh_components(i).locator, sdc_colour_index, ldoc.refresh_components.refresh_components(i).mode)
                            End If
                        Next
                    Else
                        For i = 0 To ldoc.refresh_components.refresh_components.Count - 1
                            If ldoc.refresh_components.refresh_components(i).original_markup_colour_index <> 0 Then
                                odoc.set_background_colour_for_locator(ldoc.refresh_components.refresh_components(i).locator, ldoc.refresh_components.refresh_components(i).original_markup_colour_index, ldoc.refresh_components.refresh_components(i).mode)
                                ldoc.refresh_components.refresh_components(i).original_markup_colour_index = 0
                            End If
                        Next
                    End If
                    ldoc.write_data_to_file(bc_cs_central_settings.local_repos_path + name + ".dat")
                End If
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_show_sdc_udc", "new", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            odoc.enable_screen_updating()
            slog = New bc_cs_activity_log("bc_am_show_sdc_udc", "new", bc_cs_activity_codes.TRACE_EXIT, "")
            bc_cs_central_settings.progress_bar.unload()
        End Try
    End Sub
  
    Private Function load_data() As Boolean
        Dim slog = New bc_cs_activity_log("bc_am_show_sdc_udc", "load_data", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            load_data = False
            Dim i As Integer
            Dim obc_objects As New bc_am_load_objects
            REM check connection
            REM should be getting this from document
            bc_cs_central_settings.version = Application.ProductVersion
            'bc_cs_central_settings.selected_conn_method = ldoc.connection_method
            REM users
            bc_am_load_objects.obc_users = bc_am_load_objects.obc_users.read_data_from_file(bc_cs_central_settings.local_template_path + bc_am_load_objects.USERS_FILENAME)
            REM set logged on user
            For i = 0 To bc_am_load_objects.obc_users.user.Count - 1
                If bc_am_load_objects.obc_users.user(i).os_user_name = bc_cs_central_settings.logged_on_user_name Then
                    bc_cs_central_settings.logged_on_user_id = bc_am_load_objects.obc_users.user(i).id
                    Exit For
                End If
            Next
            REM entities
            bc_am_load_objects.obc_templates = bc_am_load_objects.obc_entities.read_data_from_file(bc_cs_central_settings.local_template_path + bc_am_load_objects.TEMPLATES_FILENAME)
            load_data = True
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_show_sdc_udc", "load_data", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_show_sdc_udc", "load_data", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

End Class
Public Class bc_am_at_submit
    Dim ao_type As Integer
    Dim ao_object As Object
    Dim odoc As bc_ao_at_object
    Dim ldoc As New bc_om_document
    Dim ofs As New bc_cs_file_transfer_services
    Public server_side_events_failed As Boolean = False

    Public Sub New()

    End Sub
    Public Sub quick_submit(ByVal mode As Integer, ByVal ao_object As Object, ByVal ao_type As String, Optional ByVal brecreate_metadata As Boolean = False)
        Dim slog = New bc_cs_activity_log("bc_am_at_submit", "quick_submit", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim name As String
        Dim fs As New bc_cs_file_transfer_services
        Dim ocommentary As bc_cs_activity_log
        Dim local_docs As New bc_om_documents
        Dim original_local_index As Integer
        Dim recreate_flag As Boolean = False
        Dim noerr As Boolean = True
        Dim originally_local As Boolean = False
        Dim originally_saved_file_name As String = ""

        REM new stuff
        Dim doc_id As String
        Dim extension As String
        Dim metadata_file As String
        Dim namewithoutext As String
        Dim sfilename As String
        Dim sourcefilename As String
        Dim fullsourcefilename As String
        Dim submission_successful As Boolean = False
        Dim tldoc As New bc_om_document
        Dim bcheckout As Boolean = False
        Dim bopen As Boolean = False
        Try

            Dim omessage As bc_cs_message
            Dim orig_doc_id As String = "0"
            submission_successful = False
            REM instantiate correct AO object
            If ao_type = bc_ao_at_object.WORD_DOC Then
                odoc = New bc_ao_word(ao_object)
            ElseIf ao_type = bc_ao_at_object.POWERPOINT_DOC Then
                odoc = New bc_ao_powerpoint(ao_object)
            Else
                Dim omsg As New bc_cs_message("Blue Cuvre", "Mime type not supported:" + ao_type, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Sub
            End If
            REM unlock document if locked always store unloced
            odoc.unlock_document()


            REM set local index
            original_local_index = -1
            REM get properties
            name = odoc.get_name
            doc_id = odoc.get_doc_id
            orig_doc_id = doc_id
            metadata_file = CStr(doc_id) + ".dat"

            extension = "." + Right(name, Len(name) - InStrRev(name, "."))
            namewithoutext = Replace(name, extension, "")
            sfilename = CStr(doc_id) + extension

            If Left(name, 4) = "view" Then
                omessage = New bc_cs_message("Blue Curve create", "Document is view only cannot Submit", bc_cs_message.MESSAGE)
                Exit Sub
            End If


            If fs.check_document_exists_and_can_be_read(bc_cs_central_settings.local_repos_path + metadata_file) = False Then
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.LOCAL Then
                    omessage = New bc_cs_message("Blue Curve create", "Cannot Submit Document as no network and document was not opened via Create.", bc_cs_message.MESSAGE)
                    Exit Sub
                End If
                REM xml metadata file doesnt exists so attempt to recreate
                Dim ordm As New bc_am_recreate_doc_metadata
                ordm.brecreate_from_pub_type = brecreate_metadata
                ordm.odoc = odoc

                If ordm.recreate_doc_metadata(namewithoutext, odoc) = False Then
                    Exit Sub
                Else
                    ldoc = ldoc.read_data_from_file(bc_cs_central_settings.local_repos_path + metadata_file)
                    recreate_flag = True
                End If
            Else
                REM load metadata
                ldoc = ldoc.read_data_from_file(bc_cs_central_settings.local_repos_path + metadata_file)
            End If

           

            REM store this copy of metadata in case or rollback
            tldoc = tldoc.read_data_from_file(bc_cs_central_settings.local_repos_path + metadata_file)

            If ldoc.refresh_components.refresh_components.Count > 0 Then
                If ldoc.refresh_components.refresh_components(0).original_markup_colour_index <> 0 Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Please hide component markup before submitting!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub
                End If
            End If

            REM if markup up is on turn off
            hide_markup()


            REM indicate this document is always the master
            ldoc.master_flag = True
            REM read in to metadata object onformation from document
            REM Number of Pages
            REM EFG JULY 2013
            Dim pages As Integer = 0
            pages = odoc.number_of_pages
            If pages > 0 Then
                ldoc.pages = pages
            End If
            REM Title if property has no value maintain metadata value
            Dim ti, st, su As String
            ti = odoc.get_document_title

            If Trim(ti) <> "" Then
                ldoc.title = ti
            Else
                If Trim(ldoc.title) = "" Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Document has no title please use full submit to enter title", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    REM FIL  DEC 2015 
                    Exit Sub
                End If

            End If
                st = odoc.get_document_subtitle
                If st <> "" Then
                    ldoc.sub_title = st
                End If
                su = odoc.get_document_summary
                If su <> "" And su <> " " Then
                    ldoc.summary = su
                End If
                Dim blocal_submit As Boolean = False

                bcheckout = False
                bopen = False

                If mode = 1 Then
                    bcheckout = True
                ElseIf mode = 2 Then
                    bcheckout = True
                    bopen = True
                End If

                bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve create", "Initializing Save...", 4, False, True)
            Dim tx As String
            sourcefilename = odoc.get_name
            fullsourcefilename = odoc.get_fullname

            If sourcefilename <> sfilename Then
                'odoc.save()
                tx = odoc.save_return_error
                If tx <> "" Then
                    Dim omsg As New bc_cs_message("Blue Curve", "There is a problem saving the local copy of the document please correct the issue and try again: " + tx, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub
                End If
            End If
            'odoc.saveas(doc_id)

            tx = odoc.saveas_return_error(CStr(doc_id))
            If tx <> "" Then
                Dim omsg As New bc_cs_message("Blue Curve", "There is a problem save as  of the local copy  of the document please correct the issue and try again: " + tx, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Sub
            End If

                ldoc.write_data_to_file(bc_cs_central_settings.local_repos_path + metadata_file)

                ldoc.byteDoc = Nothing
                REM stuff document into metadata bytestream
                If IsNothing(ldoc.byteDoc) = True Then
                    Dim ofs As New bc_cs_file_transfer_services
                    fs.write_document_to_bytestream_doc_open(bc_cs_central_settings.local_repos_path + sfilename, ldoc.byteDoc, Nothing)
                End If

                bc_cs_central_settings.progress_bar.increment("Saving Document to System...")

                Dim ldoc_found As Boolean = False
                Dim i As Integer
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    Dim dbc As New bc_cs_db_services(False)
                    ocommentary = New bc_cs_activity_log("bc_am_at_submit", "new", bc_cs_activity_codes.COMMENTARY, "Submitting Document via ADO.")
                    ldoc.db_write(Nothing)
                    If ldoc.doc_write_success = True Then
                        submission_successful = True
                    End If
                    REM SOAP connectivity
                ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then

                    REM FIL AUG 2013 take out lookup lists to make packet smaller
                    For i = 0 To ldoc.refresh_components.refresh_components.Count - 1
                        For j = 0 To ldoc.refresh_components.refresh_components(i).parameters.component_template_parameters.count - 1
                            ldoc.refresh_components.refresh_components(i).parameters.component_template_parameters(j).lookup_values.clear()
                            ldoc.refresh_components.refresh_components(i).parameters.component_template_parameters(j).lookup_values_ids.clear()
                        Next
                    Next
                    REM ==========================================================
                    ocommentary = New bc_cs_activity_log("bc_am_at_submit", "new", bc_cs_activity_codes.COMMENTARY, "Submitting Document via SOAP.")

                    REM send the document object to the server
                    ldoc.packet_code = "submit" + CStr(Format(Now, "hhmm"))
                    ldoc.tmode = bc_cs_soap_base_class.tWRITE
                    If ldoc.transmit_to_server_and_receive(ldoc, True) = False Then
                        submission_successful = False
                    Else
                        If ldoc.doc_write_success = True Then
                            submission_successful = True
                        End If
                    End If
                End If

                If ldoc.doc_write_error_text <> "" Or submission_successful = False Then
                    odoc.close(False, True)
                    Dim omsg As New bc_cs_message("Blue Curve", "Document Submission Failed, document will be reopened. Please try again or contact system administrator", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Dim ocomm As New bc_cs_activity_log("bc_am_at_submit", "New", bc_cs_activity_codes.COMMENTARY, "Document Submission Failed: " + ldoc.doc_write_error_text)
                    REM if events have fired restore original document and metadata
                If IsNothing(ldoc.preeventbyteDoc) = False Then
                    fs.write_bytestream_to_document(fullsourcefilename, ldoc.preeventbyteDoc, Nothing)
                    ldoc.preeventbyteDoc = Nothing
                    tldoc.write_data_to_file(bc_cs_central_settings.local_repos_path + metadata_file)
                End If
                    odoc.visible()
                    odoc.open(fullsourcefilename, True, True)
                    Exit Sub
                Else
                    ofs.delete_file(bc_cs_central_settings.local_repos_path + metadata_file)
                    If bopen = False Then
                        odoc.close(False, True)
                        If bcheckout = False Then
                            ofs.delete_file(bc_cs_central_settings.local_repos_path + sfilename)
                        End If
                    End If
                    If sourcefilename <> sfilename Then
                        If ofs.check_document_exists(bc_cs_central_settings.local_repos_path + sourcefilename) = True Then
                            ofs.delete_file(bc_cs_central_settings.local_repos_path + sourcefilename)
                        End If
                    End If
                    If metadata_file <> CStr(ldoc.id) + ".dat" And bcheckout = False Then
                        REM if a register took place delete spurious dat file
                        If ofs.check_document_exists(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ".dat") Then
                            ofs.delete_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ".dat")
                        End If
                    End If

                    If ldoc.server_side_events_failed <> "" Then
                        Dim omsg As New bc_cs_message("Blue Curve", ldoc.server_side_events_failed, bc_cs_message.MESSAGE)
                    End If

                    bc_cs_central_settings.progress_bar.increment("Housekeeping...")
                    REM network submission successful sp remove from local document store
                    If bc_cs_central_settings.selected_conn_method <> bc_cs_central_settings.LOCAL And bcheckout = False Then
                        REM delete local record if exists
                        ocommentary = New bc_cs_activity_log("bc_am_at_submit", "new", bc_cs_activity_codes.COMMENTARY, "Deleting Local Document as saved to Server: ")
                        REM read in local documents list
                        If fs.check_document_exists(bc_cs_central_settings.local_repos_path + "bc_local_documents.dat") Then
                            local_docs = local_docs.read_data_from_file(bc_cs_central_settings.local_repos_path + "bc_local_documents.dat")
                            REM delete this document
                            For i = 0 To local_docs.document.Count - 1
                                If local_docs.document(i).id = ldoc.id And ldoc.id <> 0 Then
                                    local_docs.document.RemoveAt(i)
                                    Exit For
                                End If
                                If local_docs.document(i).filename = ldoc.filename Then
                                    local_docs.document.RemoveAt(i)
                                    Exit For
                                End If
                            Next
                            REM save back down
                            local_docs.write_data_to_file(bc_cs_central_settings.local_repos_path + "bc_local_documents.dat")
                        End If
                    End If
                End If
                REM if submission is successful see if an open or check out was requested
                If bcheckout = True Then
                    Dim breopen As Boolean = True
                    bc_cs_central_settings.progress_bar.increment("Checking Document Back Out...")
                    REM call create open
                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        ldoc.db_check_out_create_doc()
                    Else
                        ldoc.tmode = bc_cs_soap_base_class.tREAD
                        ldoc.read_mode = bc_om_document.CHECK_OUT_CREATE_DOC
                        If ldoc.transmit_to_server_and_receive(ldoc, True) = False Then
                            breopen = False
                        End If
                    End If


                    ldoc.connection_method = bc_cs_central_settings.selected_conn_method
                    ldoc.write_data_to_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ".dat")
                    If Not IsNumeric(orig_doc_id) Then
                        odoc.saveas(CStr(ldoc.id))
                        fs.delete_file(bc_cs_central_settings.local_repos_path + orig_doc_id + ldoc.extension)
                    REM save to local doc store
                    write_to_local_store(ldoc)
                    REM change title
                    odoc.caption(ldoc.title)

                End If
                REM change title
                odoc.caption(ldoc.title)

                    If breopen = False Then
                        If bopen = False Then
                            Dim omsg As New bc_cs_message("Blue Curve", "Document couldnt be checked back out: " + ldoc.doc_write_error_text, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                        Else
                            Dim omsg As New bc_cs_message("Blue Curve", "Document couldnt be re-opened. " + ldoc.doc_write_error_text, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                        End If
                        Exit Sub
                    End If
                    If bopen = True Then
                        odoc.set_doc_id(ldoc.id)
                        odoc.visible()
                    End If
                End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_at_submit", "quick_submit", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            bc_cs_central_settings.progress_bar.unload()
            odoc.enable_screen_updating()
            If ao_type <> bc_ao_at_object.POWERPOINT_DOC Then
                odoc.visible()
            End If
            slog = New bc_cs_activity_log("bc_am_at_submit", "quick_submit", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    Private Sub write_to_local_store(ByVal ldoc As bc_om_document)
        Dim slog As New bc_cs_activity_log("bc_am_at_submit", "write_to_local_store", bc_cs_activity_codes.TRACE_EXIT, "")

        Try
            Dim olocalstore As New bc_om_documents
            Dim count As Integer

            If olocalstore.check_data_file_exists(bc_cs_central_settings.local_repos_path + "bc_local_documents.dat") Then
                olocalstore = olocalstore.read_data_from_file(bc_cs_central_settings.local_repos_path + "bc_local_documents.dat")
                count = olocalstore.document.Count
                ldoc.local_index = count
            Else
                ldoc.local_index = 0
            End If
            REM  clear out document object
            ldoc.byteDoc = Nothing
            olocalstore.document.Add(ldoc)
            olocalstore.write_data_to_file(bc_cs_central_settings.local_repos_path + "bc_local_documents.dat")
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_at_submit", "write_to_local_store", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_at_submit", "quick_submit", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    Public Sub New(ByVal ao_object As Object, ByVal ao_type As String, Optional ByVal brecreate_metadata As Boolean = False, Optional ByVal scannedList(,) As String = Nothing)
        Dim slog = New bc_cs_activity_log("bc_am_at_submit", "new", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim name As String
        Dim fs As New bc_cs_file_transfer_services
        Dim ocommentary As bc_cs_activity_log
        Dim local_docs As New bc_om_documents
        Dim original_local_index As Integer
        Dim recreate_flag As Boolean = False
        Dim noerr As Boolean = True
        Dim originally_local As Boolean = False
        Dim originally_saved_file_name As String = ""

        REM new stuff
        Dim doc_id As String
        Dim extension As String
        Dim metadata_file As String
        Dim orig_metadata_file As String
        Dim namewithoutext As String
        Dim sfilename As String
        Dim sourcefilename As String
        Dim fullsourcefilename As String
        Dim submission_successful As Boolean = False
        Dim tldoc As New bc_om_document
        Dim bcheckout As Boolean = False
        Dim bopen As Boolean = False
        Try

            Dim omessage As bc_cs_message
            Dim orig_doc_id As String = "0"
            submission_successful = False
            REM instantiate correct AO object
            If ao_type = bc_ao_at_object.WORD_DOC Then
                odoc = New bc_ao_word(ao_object)
            ElseIf ao_type = bc_ao_at_object.POWERPOINT_DOC Then
                odoc = New bc_ao_powerpoint(ao_object)
            Else
                Dim omsg As New bc_cs_message("Blue Cuvre", "Mime type not supported:" + ao_type, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Sub
            End If
            REM unlock document if locked always store unloced
            odoc.unlock_document()


            REM set local index
            original_local_index = -1
            REM get properties
            name = odoc.get_name
            doc_id = odoc.get_doc_id
            orig_doc_id = doc_id
            metadata_file = CStr(doc_id) + ".dat"
            orig_metadata_file = ""
            If IsNumeric(doc_id) = 0 Then
                 orig_metadata_file = metadata_file
            End If



            extension = "." + Right(name, Len(name) - InStrRev(name, "."))
            namewithoutext = Replace(name, extension, "")
            sfilename = CStr(doc_id) + extension

           

          
            If Left(name, 4) = "view" Then
                omessage = New bc_cs_message("Blue Curve create", "Document is view only cannot Submit", bc_cs_message.MESSAGE)
                Exit Sub
            End If


            If fs.check_document_exists_and_can_be_read(bc_cs_central_settings.local_repos_path + metadata_file) = False Then
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.LOCAL Then
                    omessage = New bc_cs_message("Blue Curve create", "Cannot Submit Document as no network and document was not opened via Create.", bc_cs_message.MESSAGE)
                    Exit Sub
                End If
                REM xml metadata file doesnt exists so attempt to recreate
                Dim ordm As New bc_am_recreate_doc_metadata
                ordm.brecreate_from_pub_type = brecreate_metadata
                ordm.odoc = odoc

                If ordm.recreate_doc_metadata(namewithoutext, odoc) = False Then
                    Exit Sub
                Else
                    ldoc = ldoc.read_data_from_file(bc_cs_central_settings.local_repos_path + metadata_file)
                    recreate_flag = True
                End If
            Else
                REM load metadata
                ldoc = ldoc.read_data_from_file(bc_cs_central_settings.local_repos_path + metadata_file)
            End If

            REM store this copy of metadata in case or rollback
            tldoc = tldoc.read_data_from_file(bc_cs_central_settings.local_repos_path + metadata_file)

            If ldoc.refresh_components.refresh_components.Count > 0 Then
                If ldoc.refresh_components.refresh_components(0).original_markup_colour_index <> 0 Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Please hide component markup before submitting!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub
                End If
            End If

            REM if markup up is on turn off
            hide_markup()


            REM indicate this document is always the master
            ldoc.master_flag = True
            REM read in to metadata object onformation from document
            REM Number of Pages
            REM EFG JULY 2013
            Dim pages As Integer = 0
            pages = odoc.number_of_pages
            If pages > 0 Then
                ldoc.pages = pages
            End If
            REM Title if property has no value maintain metadata value
            Dim ti, st, su As String
            ti = odoc.get_document_title
            If ti <> "" Then
                ldoc.title = ti
            End If
            st = odoc.get_document_subtitle
            If st <> "" Then
                ldoc.sub_title = st
            End If
            su = odoc.get_document_summary
            If su <> "" And su <> " " Then
                ldoc.summary = su
            End If
            Dim blocal_submit As Boolean = False
            'If show_form = True Then

            REM DEV EXPRESS
            Dim fdxsubmit As New bc_dx_as_categorize
            fdxsubmit.ao_object = odoc


            fdxsubmit.document = ldoc
            fdxsubmit.create_mode = True
            fdxsubmit.show_stage_change = True
            fdxsubmit.show_local_submit = True

            fdxsubmit.enable_pub_types = False
            fdxsubmit.enable_lead_entity = False

            Dim pts As New List(Of String)
            pts.Add(ldoc.pub_type_name)
            fdxsubmit.set_pub_types = pts


            fdxsubmit.ShowDialog()


            If fdxsubmit.ok_selected = False Then
                ocommentary = New bc_cs_activity_log("bc_am_at_submit", "new", bc_cs_activity_codes.COMMENTARY, "Cancel Selected on Submit form.")
                If ldoc.locked = True Then
                    odoc.lock_document()
                Else
                    odoc.unlock_document()
                End If
                Exit Sub
            End If

            REM SEPT 2015
            Dim instateactions As New List(Of bc_om_workflow_action)
            If fdxsubmit.instate = True AndAlso fdxsubmit.chkinstateactions.CheckedItems.Count > 0 Then
                ldoc.action_Ids.Clear()
                For g = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                    If bc_am_load_objects.obc_pub_types.pubtype(g).id = ldoc.pub_type_id Then
                        If bc_am_load_objects.obc_pub_types.pubtype(g).in_stage_actions.count > 0 Then
                            For j = 0 To bc_am_load_objects.obc_pub_types.pubtype(g).in_stage_actions.count - 1
                                If bc_am_load_objects.obc_pub_types.pubtype(g).in_stage_actions(j).stage_Id = ldoc.stage Then
                                    For k = 0 To bc_am_load_objects.obc_pub_types.pubtype(g).in_stage_actions(j).actions.count - 1
                                        instateactions.Add(bc_am_load_objects.obc_pub_types.pubtype(g).in_stage_actions(j).actions(k))
                                    Next
                                End If
                            Next
                        End If
                        Exit For
                    End If
                Next
                For k = 0 To fdxsubmit.chkinstateactions.CheckedItems.Count - 1
                    For j = 0 To instateactions.Count - 1
                        If fdxsubmit.chkinstateactions.CheckedItems(k) = instateactions(j).display_name Then
                            ldoc.action_Ids.Add(instateactions(j).id)
                        End If
                    Next
                Next
            End If
            REM ==============================


            If fdxsubmit.rcentral.SelectedIndex = 1 Then
                blocal_submit = True
            Else
                blocal_submit = False
            End If


            bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve create", "Initializing Save...", 4, False, True)

            sourcefilename = odoc.get_name
            fullsourcefilename = odoc.get_fullname
            Dim tx As String

            If sourcefilename <> sfilename Then
                tx = odoc.save_return_error
                If tx <> "" Then
                    Dim omsg As New bc_cs_message("Blue Curve", "There is a problem saving the local copy of the document please correct the issue and try again: " + tx, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub
                End If
            End If
            tx = odoc.saveas_return_error(doc_id)

            If tx <> "" Then
                Dim omsg As New bc_cs_message("Blue Curve", "There is a problem with ""save as"" of the local copy of the document please correct the issue and try again: " + tx, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Sub
            End If
            ldoc.write_data_to_file(bc_cs_central_settings.local_repos_path + metadata_file)

            Dim no_register As Boolean
            no_register = False
            REM  multiple approver
            Dim no_workflow As Boolean = False
            check_multiple_approver(no_workflow)

            If no_workflow = True Then
                ldoc.stage = ldoc.original_stage
                ldoc.stage_name = ldoc.original_stage_name
                ldoc.original_stage = 0
            End If

            REM ---------------------------------------------
            REM Client Side Event Handling
            REM ---------------------------------------------
            REM PR pending events

            'If ldoc.action_Ids.Count > 0 And from_async_handler = False Then
            '    ldoc.pending_mode = bc_om_document.PD
            '    no_workflow = True

            'End If

            ldoc.byteDoc = Nothing
            If blocal_submit = False And no_workflow = False Then
                If ldoc.action_Ids.Count > 0 Then

                    REM if document has never been network submiited register first here
                    If ldoc.id = 0 And ldoc.action_Ids(0) <> 0 Then
                        originally_local = True
                        ocommentary = New bc_cs_activity_log("bc_am_at_submit", "new", bc_cs_activity_codes.COMMENTARY, "Registring Document as actions required.")
                        ldoc.bimport_support_only = False
                        ldoc.register_only = True
                        ldoc.byteDoc = Nothing
                        bc_cs_central_settings.progress_bar.increment("Registering Document ...")
                        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                            ocommentary = New bc_cs_activity_log("bc_am_at_submit", "new", bc_cs_activity_codes.COMMENTARY, "Registering Document via ADO.")
                            ldoc.db_write(Nothing)


                        ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then

                            ocommentary = New bc_cs_activity_log("bc_am_at_submit", "new", bc_cs_activity_codes.COMMENTARY, "Registering Document via SOAP.")
                            ldoc.tmode = bc_cs_soap_base_class.tWRITE
                            If ldoc.transmit_to_server_and_receive(ldoc, True) = False Then
                                no_register = True
                            End If
                        End If
                        If ldoc.doc_write_error_text <> "" Or no_register = True Then
                            Dim omsg As New bc_cs_message("Blue Curve", "Document registration failed please try again or contact system administator", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                            Dim ocomm As New bc_cs_activity_log("bc_am_at_submit", "New", bc_cs_activity_codes.COMMENTARY, "Document Registration Failed: " + ldoc.doc_write_error_text)
                            Exit Sub
                        End If

                        REM PR March 2012 set this here so multiple doc events can work properly
                        odoc.set_doc_id(ldoc.id)

                        REM reset register flag
                        ldoc.register_only = False
                        ldoc.write_data_to_file(bc_cs_central_settings.local_repos_path + metadata_file)

                    End If
                    REM if document could not be registered dont run actions
                    If no_register = False Then
                        REM make document invisible
                        odoc.invisible()
                        REM make a copy of the original document in case submission or events fail
                        fs.file_copy(bc_cs_central_settings.local_repos_path + sfilename, bc_cs_central_settings.local_repos_path + "tmp" + extension)
                        fs.write_document_to_bytestream(bc_cs_central_settings.local_repos_path + "tmp" + extension, ldoc.preeventbyteDoc, Nothing)
                        fs.delete_file(bc_cs_central_settings.local_repos_path + "tmp" + extension)

                        Dim weh As New bc_am_workflow_events_handler(odoc.get_object, ldoc, True)
                        weh.from_create = True
                        weh.run_events(ldoc)

                        If weh.success = True Then
                            ocommentary = New bc_cs_activity_log("bc_am_at_submit", "new", bc_cs_activity_codes.COMMENTARY, "All transitional Events succeeded so stage change approved.")
                            REM save document as updates may of occured
                            If ldoc.id > 0 And originally_local = False Then
                                'odoc.saveas(CStr(ldoc.id))
                                tx = odoc.saveas_return_error(CStr(ldoc.id))
                                If tx <> "" Then
                                    Dim omsg As New bc_cs_message("Blue Curve", "There is a problem save as after events  of the local copy  of the document please correct the issue and try again: " + tx, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                                    Exit Sub
                                End If

                            Else
                                odoc.set_doc_id(ldoc.id)
                                'odoc.saveas(namewithoutext)
                                tx = odoc.saveas_return_error(CStr(namewithoutext))
                                If tx <> "" Then
                                    Dim omsg As New bc_cs_message("Blue Curve", "There is a problem save as after events  of the local copy  of the document please correct the issue and try again: " + tx, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                                    Exit Sub
                                End If
                            End If
                            attach_support_documents(ldoc)
                            REM core June 2014 redo number of pages as event may of changed the page count
                            ldoc.pages = odoc.number_of_pages
                            REM =======
                        Else
                            REM revert back to current stage
                            ldoc.stage = ldoc.original_stage
                            ldoc.stage_name = ldoc.original_stage_name
                            REM FIL 5.6.3

                            If weh.suppress_failure_message = False Then
                                Dim etx As String
                                If fdxsubmit.instate = True Then
                                    etx = "In State Events Failed." + vbCrLf
                                Else
                                    etx = "Transitional Events Failed so document will not change Stage." + vbCrLf
                                End If
                                If ldoc.history.Count > 0 Then
                                    etx = etx + ldoc.history(ldoc.history.Count - 1).comment
                                End If
                                omessage = New bc_cs_message("Blue Curve - Workflow", etx, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                            End If
                                If weh.keep_open_on_failure = True Then
                                    If orig_metadata_file <> "" AndAlso fs.check_document_exists(bc_cs_central_settings.local_repos_path + metadata_file) Then
                                        fs.delete_file(bc_cs_central_settings.local_repos_path + metadata_file)
                                    End If
                                    ldoc.write_data_to_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ".dat")
                                    If weh.refresh = True Then
                                        bc_cs_central_settings.progress_bar.change_caption("Refreshing Document...")
                                        Dim obc_refresh As New bc_am_at_component_refresh(False, ao_object, "word", 1, False)

                                        ldoc = ldoc.read_data_from_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ".dat")
                                    End If

                                    Exit Sub
                                End If
                                REM end FIL 5.6.3
                                ldoc.byteDoc = ldoc.preeventbyteDoc
                            End If
                    End If
                End If

            End If

            REM stuff document into metadata bytestream

            If IsNothing(ldoc.byteDoc) = True Then
                Dim ofs As New bc_cs_file_transfer_services
                fs.write_document_to_bytestream_doc_open(bc_cs_central_settings.local_repos_path + sfilename, ldoc.byteDoc, Nothing)
            End If

            bc_cs_central_settings.progress_bar.increment("Saving Document to System...")

            Dim ldoc_found As Boolean = False
            Dim i As Integer
            If blocal_submit = True Then
                odoc.close(False, True)
                ocommentary = New bc_cs_activity_log("bc_am_at_submit", "new", bc_cs_activity_codes.COMMENTARY, "Submitting Document via Local.")

                If fs.check_document_exists(bc_cs_central_settings.local_repos_path + "bc_local_documents.dat") Then
                    local_docs = local_docs.read_data_from_file(bc_cs_central_settings.local_repos_path + "bc_local_documents.dat")
                    REM see if document already exists
                    For i = 0 To local_docs.document.Count - 1
                        If local_docs.document(i).id = ldoc.id And ldoc.id <> 0 Then
                            ldoc_found = True
                            Exit For
                        End If
                        If local_docs.document(i).filename = ldoc.filename Or local_docs.document(i).filename = ldoc.filename + ldoc.extension Then
                            ldoc_found = True
                            Exit For
                        End If
                    Next
                End If

                If InStr(ldoc.filename, ldoc.extension) = 0 Then
                    ldoc.filename = ldoc.filename + ldoc.extension
                End If


                ldoc.docobject = Nothing


                If ldoc_found = True Then
                    local_docs.document(i) = ldoc
                Else
                    local_docs.document.Add(ldoc)
                End If
                local_docs.write_data_to_file(bc_cs_central_settings.local_repos_path + "bc_local_documents.dat")
                Exit Sub
            Else

                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    Dim dbc As New bc_cs_db_services(False)
                    ocommentary = New bc_cs_activity_log("bc_am_at_submit", "new", bc_cs_activity_codes.COMMENTARY, "Submitting Document via ADO.")
                    ldoc.db_write(Nothing)
                    If ldoc.doc_write_success = True Then
                        submission_successful = True
                    End If
                    REM SOAP connectivity
                ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then

                    REM FIL AUG 2013 take out lookup lists to make packet smaller
                    For i = 0 To ldoc.refresh_components.refresh_components.Count - 1
                        For j = 0 To ldoc.refresh_components.refresh_components(i).parameters.component_template_parameters.count - 1
                            ldoc.refresh_components.refresh_components(i).parameters.component_template_parameters(j).lookup_values.clear()
                            ldoc.refresh_components.refresh_components(i).parameters.component_template_parameters(j).lookup_values_ids.clear()
                        Next
                    Next
                    REM ==========================================================
                    ocommentary = New bc_cs_activity_log("bc_am_at_submit", "new", bc_cs_activity_codes.COMMENTARY, "Submitting Document via SOAP.")

                    REM send the document object to the server
                    ldoc.packet_code = "submit" + CStr(Format(Now, "hhmm"))
                    ldoc.tmode = bc_cs_soap_base_class.tWRITE


                    If ldoc.transmit_to_server_and_receive(ldoc, True) = False Then
                        submission_successful = False
                    Else
                        If ldoc.doc_write_success = True Then
                            submission_successful = True
                        End If
                    End If
                End If
            End If

            If ldoc.doc_write_error_text <> "" Or submission_successful = False Then
                odoc.close(False, True)
                Dim omsg As New bc_cs_message("Blue Curve", "Document Submission Failed, document will be reopened. Please try again or contact system administrator", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Dim ocomm As New bc_cs_activity_log("bc_am_at_submit", "New", bc_cs_activity_codes.COMMENTARY, "Document Submission Failed: " + ldoc.doc_write_error_text)
                REM if events have fired restore original document and metadata
                If IsNothing(ldoc.preeventbyteDoc) = False Then
                    fs.write_bytestream_to_document(fullsourcefilename, ldoc.preeventbyteDoc, Nothing)
                    ldoc.preeventbyteDoc = Nothing
                    tldoc.write_data_to_file(bc_cs_central_settings.local_repos_path + metadata_file)
                End If
                odoc.visible()
                odoc.open(fullsourcefilename, True, True)
                Exit Sub
            Else
                ofs.delete_file(bc_cs_central_settings.local_repos_path + metadata_file)
                If bopen = False Then
                    odoc.close(False, True)
                    If bcheckout = False Then
                        ofs.delete_file(bc_cs_central_settings.local_repos_path + sfilename)
                    End If
                End If
                If sourcefilename <> sfilename Then
                    If ofs.check_document_exists(bc_cs_central_settings.local_repos_path + sourcefilename) = True Then
                        ofs.delete_file(bc_cs_central_settings.local_repos_path + sourcefilename)
                    End If
                End If
                If metadata_file <> CStr(ldoc.id) + ".dat" And bcheckout = False Then
                    REM if a register took place delete spurious dat file
                    If ofs.check_document_exists(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ".dat") Then
                        ofs.delete_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ".dat")
                    End If
                End If

                If ldoc.server_side_events_failed <> "" Then
                    Dim omsg As New bc_cs_message("Blue Curve", ldoc.server_side_events_failed, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                End If

                bc_cs_central_settings.progress_bar.increment("Housekeeping...")
                REM network submission successful sp remove from local document store
                If bc_cs_central_settings.selected_conn_method <> bc_cs_central_settings.LOCAL And bcheckout = False Then
                    REM delete local record if exists
                    ocommentary = New bc_cs_activity_log("bc_am_at_submit", "new", bc_cs_activity_codes.COMMENTARY, "Deleting Local Document as saved to Server: ")
                    REM read in local documents list
                    If fs.check_document_exists(bc_cs_central_settings.local_repos_path + "bc_local_documents.dat") Then
                        local_docs = local_docs.read_data_from_file(bc_cs_central_settings.local_repos_path + "bc_local_documents.dat")
                        REM delete this document

                        For i = 0 To local_docs.document.Count - 1
                            If local_docs.document(i).id = ldoc.id And ldoc.id <> 0 Then
                                local_docs.document.RemoveAt(i)
                                Exit For
                            End If
                            If local_docs.document(i).filename = ldoc.filename Or local_docs.document(i).filename = ldoc.filename + ldoc.extension Then
                                local_docs.document.RemoveAt(i)
                                Exit For
                            End If
                        Next
                        REM save back down
                        local_docs.write_data_to_file(bc_cs_central_settings.local_repos_path + "bc_local_documents.dat")
                    End If
                End If
            End If
            REM if submission is successful see if an open or check out was requested
            If bcheckout = True Then
                Dim breopen As Boolean = True
                bc_cs_central_settings.progress_bar.increment("Checking Document Back Out...")
                REM call create open
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    ldoc.db_check_out_create_doc()
                Else
                    ldoc.tmode = bc_cs_soap_base_class.tREAD
                    ldoc.read_mode = bc_om_document.CHECK_OUT_CREATE_DOC
                    If ldoc.transmit_to_server_and_receive(ldoc, True) = False Then
                        breopen = False
                    End If
                End If


                ldoc.connection_method = bc_cs_central_settings.selected_conn_method
                ldoc.write_data_to_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ".dat")
                If Not IsNumeric(orig_doc_id) Then
                    odoc.saveas(CStr(ldoc.id))
                    fs.delete_file(bc_cs_central_settings.local_repos_path + orig_doc_id + ldoc.extension)

                End If

                If breopen = False Then
                    If bopen = False Then
                        Dim omsg As New bc_cs_message("Blue Curve", "Document couldnt be checked back out: " + ldoc.doc_write_error_text, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Else
                        Dim omsg As New bc_cs_message("Blue Curve", "Document couldnt be re-opened. " + ldoc.doc_write_error_text, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    End If
                    Exit Sub
                End If
                If bopen = True Then
                    odoc.set_doc_id(ldoc.id)
                    odoc.visible()
                End If
            End If

            If fdxsubmit.instate = True AndAlso fdxsubmit.chkinstateactions.CheckedItems.Count > 0 Then
                Dim omsg As New bc_cs_message("Blue Curve", "Do you wish to see the preview now?", bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
                If omsg.cancel_selected = False Then

                    Dim opreview As New bc_om_get_preview_url
                    opreview.doc_id = ldoc.id
                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        opreview.db_read()
                    Else
                        opreview.tmode = bc_cs_soap_base_class.tREAD
                        If opreview.transmit_to_server_and_receive(opreview, True) = True Then
                            Try
                                System.Diagnostics.Process.Start(opreview.url)
                            Catch
                                omsg = New bc_cs_message("Blue Curve", "Failed to invoke preview : " + opreview.url, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                            End Try


                        End If
                    End If
                End If
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_at_submit", "new", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            bc_cs_central_settings.progress_bar.unload()
            odoc.enable_screen_updating()
            If ao_type <> bc_ao_at_object.POWERPOINT_DOC Then
                odoc.visible()
            End If
            slog = New bc_cs_activity_log("bc_am_at_submit", "new", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    
    Private Function hide_markup() As Boolean
        Try
            hide_markup = True
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
        Catch ex As Exception
            hide_markup = False
            Dim oerr As New bc_cs_error_log("bc_am_at_submit", "hide_markup", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Function
    Public Sub check_multiple_approver(ByRef no_workflow As Boolean)
        If ldoc.original_stage_name <> ldoc.stage_name And ldoc.original_stage_name <> "" Then
            Dim j As Integer
            Dim ta, na As String
            Dim omessage As New bc_cs_message
            For i = 0 To ldoc.workflow_stages.stages.Count - 1
                If ldoc.workflow_stages.stages(i).stage_name = ldoc.stage_name Then
                    If ldoc.workflow_stages.stages(i).num_approvers > 0 Then
                        ta = ldoc.workflow_stages.stages(i).num_approvers
                        na = ldoc.workflow_stages.stages(i).approved_by.count
                        REM check current user hasn't already approved document
                        For j = 0 To ldoc.workflow_stages.stages(i).approved_by.count - 1
                            If CStr(bc_cs_central_settings.logged_on_user_id) = CStr(ldoc.workflow_stages.stages(i).approved_by(j)) Then
                                omessage = New bc_cs_message("Blue Curve - Process", "You have already approved this document to this stage. " + CStr(ta - na) + " other approver(s) still required.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                                ldoc.set_approve = False
                                no_workflow = True
                                Exit Sub
                            End If
                        Next
                        If no_workflow = False Then

                            Dim approver_role_found As Boolean = False
                            REM author can approve and no specific role


                            If ldoc.workflow_stages.stages(i).author_approval = 1 And ldoc.originating_author = bc_cs_central_settings.logged_on_user_id Then
                                REM author cannot approve his own document
                                omessage = New bc_cs_message("Blue Curve - Process", "This user is the author, and is unable to approve this report. " + CStr(ta - na) + " other approver(s) still required.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                                no_workflow = True
                                Exit Sub
                                REM check if user is in approver role
                            ElseIf ldoc.workflow_stages.stages(i).approval_role > 0 Then
                                For j = 0 To bc_am_load_objects.obc_prefs.secondary_roles.Count - 1
                                    If bc_am_load_objects.obc_prefs.secondary_roles(j) = ldoc.workflow_stages.stages(i).approval_role Then
                                        Dim omsg As New bc_cs_message("Blue Curve - Process", "You are authorized to approve this report do you wish to approve?", bc_cs_message.MESSAGE, True, False, "Approve", "Cancel", True)
                                        If omsg.cancel_selected = True Then
                                            no_workflow = True
                                            Exit Sub
                                        End If
                                        approver_role_found = True
                                        Exit For
                                    End If
                                Next
                                If approver_role_found = False Then
                                    omessage = New bc_cs_message("Blue Curve - Process", "This user is not setup as an approver, and is unable to approve this report.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                                    no_workflow = True
                                    Exit Sub
                                End If
                            End If
                            If ldoc.workflow_stages.stages(i).num_approvers > ldoc.workflow_stages.stages(i).approved_by.count + 1 Then
                                omessage = New bc_cs_message("Blue Curve - Process", "Document cannot change stage as: " + CStr(ta - na - 1) + " additional approver(s) are required", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                                ldoc.set_approve = True
                                ldoc.approved_by = bc_cs_central_settings.logged_on_user_id
                                ldoc.approve_stage = ldoc.workflow_stages.stages(i).stage_id
                                ldoc.approve_stage_name = ldoc.workflow_stages.stages(i).stage_name
                                no_workflow = True
                                Exit Sub
                            End If
                        End If
                    End If
                    Exit For
                End If
            Next
        End If
    End Sub
  
    Public Sub attach_support_documents(ByVal filename As String)
        Dim slog = New bc_cs_activity_log("bc_am_at_submit", "attach_support_documents", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim mime_types As New ArrayList
            Dim support_doc As bc_om_document

            Dim support_docs As New bc_om_documents
            Dim i As Integer
            Dim extensionsize As Integer

            REM remove all support docs that are not new
            While i < ldoc.support_documents.Count
                If ldoc.support_documents(i).support_doc_state <> 2 Then
                    ldoc.support_documents.RemoveAt(i)
                Else
                    i = i + 1
                End If
            End While
            mime_types.Add(".pdf")
            mime_types.Add("_first_call.xml")

            REM scan for all files of mime type and add suffix until cant find anymore
            Dim fs As New bc_cs_file_transfer_services
            Dim fn As String = ""
            Try

                REM SW cope with office versions
                extensionsize = 0
                extensionsize = (Len(ldoc.extension) - (InStrRev(ldoc.extension, ".") - 1))

                If Right(ldoc.filename, extensionsize) = Right(ldoc.extension, extensionsize) Then
                    fn = Left(ldoc.filename, Len(ldoc.filename) - extensionsize)
                Else
                    fn = ldoc.filename
                End If
            Catch

            End Try

            For i = 0 To mime_types.Count - 1
                If fs.check_document_exists(bc_cs_central_settings.local_repos_path + fn + mime_types(i)) = True Then
                    support_doc = New bc_om_document
                    support_doc.id = 0
                    support_doc.master_flag = False
                    support_doc.pub_type_id = ldoc.pub_type_id
                    support_doc.pub_type_name = ldoc.pub_type_name
                    support_doc.originating_author = ldoc.originating_author
                    support_doc.bus_area = ldoc.bus_area
                    support_doc.checked_out_user = 0
                    support_doc.doc_date = ldoc.doc_date.ToUniversalTime
                    support_doc.entity_id = ldoc.entity_id
                    support_doc.originating_author = ldoc.originating_author
                    support_doc.title = ldoc.title + CStr(mime_types(i))
                    support_doc.extension = mime_types(i)
                    fs.write_document_to_bytestream(bc_cs_central_settings.local_repos_path + fn + mime_types(i), support_doc.byteDoc, Nothing)


                    support_doc.filename = filename + mime_types(i)
                    ldoc.support_documents.Add(support_doc)
                    REM delete locla copy
                    fs.delete_file(bc_cs_central_settings.local_repos_path + fn + mime_types(i))
                End If
            Next

        Catch ex As Exception

            Dim db_err As New bc_cs_error_log("bc_am_at_submit", "attach_support_documents", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

            slog = New bc_cs_activity_log("bc_am_at_submit", "attach_support_documents", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub

    REM loads relevant data for submit into memory
    Private Function load_data() As Boolean
        Dim slog = New bc_cs_activity_log("bc_am_at_submit", "load_data", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            load_data = False
            Dim i As Integer
            Dim obc_objects As New bc_am_load_objects
            REM check connection
            REM should be getting this from document
            bc_cs_central_settings.version = Application.ProductVersion
            'bc_cs_central_settings.selected_conn_method = ldoc.connection_method
            REM users
            bc_am_load_objects.obc_users = bc_am_load_objects.obc_users.read_data_from_file(bc_cs_central_settings.local_template_path + bc_am_load_objects.USERS_FILENAME)
            REM set logged on user
            For i = 0 To bc_am_load_objects.obc_users.user.Count - 1
                If bc_am_load_objects.obc_users.user(i).os_user_name = bc_cs_central_settings.logged_on_user_name Then
                    bc_cs_central_settings.logged_on_user_id = bc_am_load_objects.obc_users.user(i).id
                    Exit For
                End If
            Next
            REM entities
            bc_am_load_objects.obc_entities = bc_am_load_objects.obc_entities.read_data_from_file(bc_cs_central_settings.local_template_path + bc_am_load_objects.ENTITIES_FILENAME)
            REM pub types
            bc_am_load_objects.obc_pub_types = bc_am_load_objects.obc_pub_types.read_data_from_file(bc_cs_central_settings.local_template_path + bc_am_load_objects.PUB_TYPES_FILENAME)
            REM preferances
            bc_am_load_objects.obc_prefs = bc_am_load_objects.obc_pub_types.read_data_from_file(bc_cs_central_settings.local_template_path + bc_am_load_objects.PREFS_FILENAME)

            load_data = True
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_load_objects", "load_data", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            slog = New bc_cs_activity_log("bc_am_at_submit", "load_data", bc_cs_activity_codes.TRACE_EXIT, "")

        End Try

    End Function

    Public Sub attach_support_documents(ByRef ldoc As bc_om_document)
        Dim slog = New bc_cs_activity_log("bc_am_at_submit", "attach_support_documents", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim filename As String
            Dim support_doc As bc_om_document
            Dim mime_types As New ArrayList
            Dim ext As String
            Dim fs As New bc_cs_file_transfer_services
            Dim i As Integer
            'support_documents.Clear()
            ext = ldoc.extension
            ext = Replace(ext, "[imp]", "")
            filename = Replace(ldoc.filename, ext, "")
            mime_types.Add(".pdf")

            For i = 0 To mime_types.Count - 1
                If fs.check_document_exists(bc_cs_central_settings.local_repos_path + filename + mime_types(i)) = True Then
                    support_doc = New bc_om_document
                    support_doc.id = 0
                    support_doc.from_event = True
                    support_doc.master_flag = False
                    support_doc.pub_type_id = ldoc.pub_type_id
                    support_doc.pub_type_name = ldoc.pub_type_name
                    support_doc.originating_author = ldoc.originating_author
                    support_doc.bus_area = ldoc.bus_area
                    support_doc.checked_out_user = 0
                    support_doc.doc_date = ldoc.doc_date
                    support_doc.entity_id = ldoc.entity_id
                    support_doc.originating_author = ldoc.originating_author
                    support_doc.title = RTrim(ldoc.title) + CStr(mime_types(i))
                    support_doc.extension = mime_types(i)
                    fs.write_document_to_bytestream(bc_cs_central_settings.local_repos_path + filename + mime_types(i), support_doc.byteDoc, Nothing)
                    support_doc.filename = filename + mime_types(i)
                    ldoc.support_documents.Add(support_doc)
                    REM delete locla copy
                    fs.delete_file(bc_cs_central_settings.local_repos_path + filename + mime_types(i))
                End If
            Next



            'Dim mime_types As New ArrayList
            'Dim support_doc As bc_om_document

            'Dim support_docs As New bc_om_documents
            'Dim i As Integer
            'Dim extensionsize As Integer

            'REM remove all support docs that are not new
            'While i < support_documents.Count
            '    If support_documents(i).support_doc_state <> 2 Then
            '        support_documents.RemoveAt(i)
            '    Else
            '        i = i + 1
            '    End If
            'End While
            'mime_types.Add(".pdf")

            'REM scan for all files of mime type and add suffix until cant find anymore
            'Dim fs As New bc_cs_file_transfer_services
            'Dim fn As String = ""
            'Try

            '    REM SW cope with office versions
            '    extensionsize = 0
            '    extensionsize = (Len(ldoc.extension) - (InStrRev(ldoc.extension, ".") - 1))

            '    If Right(ldoc.filename, extensionsize) = Right(ldoc.extension, extensionsize) Then
            '        fn = Left(ldoc.filename, Len(ldoc.filename) - extensionsize)
            '    Else
            '        fn = ldoc.filename
            '    End If

            'Catch

            'End Try

            'For i = 0 To mime_types.Count - 1
            '    If fs.check_document_exists(bc_cs_central_settings.local_repos_path + fn + mime_types(i)) = True Then
            '        support_doc = New bc_om_document
            '        support_doc.id = 0
            '        support_doc.master_flag = False
            '        support_doc.pub_type_id = ldoc.pub_type_id
            '        support_doc.pub_type_name = ldoc.pub_type_name
            '        support_doc.originating_author = ldoc.originating_author
            '        support_doc.bus_area = ldoc.bus_area
            '        support_doc.checked_out_user = 0
            '        support_doc.doc_date = ldoc.doc_date
            '        support_doc.entity_id = ldoc.entity_id
            '        support_doc.originating_author = ldoc.originating_author
            '        support_doc.title = ldoc.title + CStr(mime_types(i))
            '        support_doc.extension = mime_types(i)
            '        fs.write_document_to_bytestream(bc_cs_central_settings.local_repos_path + fn + mime_types(i), support_doc.byteDoc, Nothing)
            '        support_doc.filename = filename + mime_types(i)
            '        support_documents.Add(support_doc)
            '        REM delete locla copy
            '        fs.delete_file(bc_cs_central_settings.local_repos_path + fn + mime_types(i))
            '    End If
            'Next

        Catch ex As Exception

            Dim db_err As New bc_cs_error_log("bc_am_at_submit", "attach_support_documents", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

            slog = New bc_cs_activity_log("bc_am_at_submit", "attach_support_documents", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub

    REM loads relevant data for submit into memory
    'Private Function load_data() As Boolean
    '    Dim slog = New bc_cs_activity_log("bc_am_at_submit", "load_data", bc_cs_activity_codes.TRACE_ENTRY, "")

    '    Try
    '        load_data = False
    '        Dim i As Integer
    '        Dim obc_objects As New bc_am_load_objects
    '        REM check connection
    '        REM should be getting this from document
    '        bc_cs_central_settings.version = Application.ProductVersion
    '        'bc_cs_central_settings.selected_conn_method = ldoc.connection_method
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
    '        slog = New bc_cs_activity_log("bc_am_at_submit", "load_data", bc_cs_activity_codes.TRACE_EXIT, "")
    '    End Try

    'End Function
    Public Sub submit_after_stage_change(ByRef ldoc As bc_om_document, ByVal ao_object As Object, ByVal ao_type As String, Optional ByVal keep_invisible As Boolean = False, Optional ByVal with_progress As Boolean = True, Optional ByVal from_async_handler As Boolean = False)
        Dim slog = New bc_cs_activity_log("bc_am_at_submit_after_stage_change", "new", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim fs As New bc_cs_file_transfer_services
        Dim Connection_method As String = ""
        Dim local_docs As New bc_om_documents
        Dim recreate_flag As Boolean = False
        Dim noerr As Boolean = True
        Dim originally_local As Boolean = False

        Try
            REM instantiate correct AO object
            If ao_type = bc_ao_at_object.WORD_DOC Then
                odoc = New bc_ao_word(ao_object)
            End If
            If ao_type = bc_ao_at_object.POWERPOINT_DOC Then
                odoc = New bc_ao_powerpoint(ao_object)
            End If
            If ao_type = bc_ao_at_object.EXCEL_DOC Then

                odoc = New bc_ao_excel(ao_object)
            End If
            odoc.unlock_document()

            REM Number of Pages
            ldoc.pages = odoc.number_of_pages


            If with_progress = True Then
                bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve create", "Initializing Save...", 4, False, True)
            End If

            odoc.save()
            REM ---------------------------------------------




            odoc.close(True)
            REM stuff document into metadata bytestream
            fs.write_document_to_bytestream(bc_cs_central_settings.local_repos_path + ldoc.filename, ldoc.byteDoc, Nothing)

            attach_support_documents(ldoc)

            ldoc.revision_from_process = True
            ldoc.from_stage_change = True


            If with_progress = True Then
                bc_cs_central_settings.progress_bar.increment("Saving Document to System...")
            End If
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                ldoc.db_write(Nothing)
            Else
                ldoc.packet_code = "submit" + Format(Now, "mmss")

                ldoc.tmode = bc_cs_soap_base_class.tWRITE
                If ldoc.transmit_to_server_and_receive(ldoc, False) = False Then
                    ldoc.doc_write_error_text = "Failed to Save via Soap. Please check log file"
                End If
            End If
            If ldoc.doc_write_error_text <> "" Then
                ldoc.server_side_events_failed = "Stage Change Failed as submission failed Document Will Stay in Current Stage: " + ldoc.doc_write_error_text
            End If

            REM delete file and metadata is sucessful
            If fs.check_document_exists(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ".dat") = True Then
                fs.delete_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ".dat")
            End If
            If fs.check_document_exists(bc_cs_central_settings.local_repos_path + ldoc.filename) = True Then
                fs.delete_file(bc_cs_central_settings.local_repos_path + ldoc.filename)
            End If



        Catch ex As Exception
            If with_progress = True Then
                Dim db_err As New bc_cs_error_log("bc_am_at_submit_after_state_change", "new", bc_cs_error_codes.USER_DEFINED, ex.Message)
            Else
                ldoc.doc_write_error_text = "bc_am_at_submit_after_state_change: new " + ex.Message
            End If
        Finally
            If with_progress = True Then
                Try
                    bc_cs_central_settings.progress_bar.unload()
                    If IsNothing(odoc) = False Then
                        odoc.enable_screen_updating()
                        If keep_invisible = False Then
                            If ao_type <> bc_ao_at_object.POWERPOINT_DOC Then
                                odoc.visible()
                                Marshal.ReleaseComObject(odoc.docobject)
                            End If
                        End If
                    End If
                Catch
                End Try
            End If
            slog = New bc_cs_activity_log("bc_am_at_submit_after_state_change", "new", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
End Class

