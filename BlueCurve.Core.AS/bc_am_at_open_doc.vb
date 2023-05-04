Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports BlueCurve.Core.AS
Imports System.Threading



REM ==========================================
REM Blue Curve Limited 2005
REM Module:       AT open Document
REM Type:         AM
REM Desciption:   Class to Open Documents
REM Methods:  New
REM Version:      1
REM Change history
REM ==========================================
Public Class bc_am_at_open_doc
    Public onetwork_docs As bc_om_documents
    Private Const WORD_EXT = ".doc"
    Private Const POWERPOINT_EXT = ".ppt"
    Private Const WORD2007_EXT = ".docx"
    Private Const WORD2007_MACRO_EXT = ".docm"
    Private Const POWERPOINT2007_EXT = ".pptx"

    Public odocument As Object
    Public odoc As Object
    Public authentication_failed As Boolean
    Public recent_change As Boolean = False


    Public Sub New()

    End Sub
   

    REM constructor copies documents user can open into shared memory
    Public Sub New(ByVal blocaldoc As Boolean, ByVal all As Boolean, ByVal date_from As Date, ByVal date_to As Date, ByVal show_publish As Boolean, Optional ByVal show_progress As Boolean = True, Optional ByVal not_from_service As Boolean = True)
        Dim otrace As New bc_cs_activity_log("bc_am_at_open_doc", "new", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim ocommentary As bc_cs_activity_log
        Try
            Dim omessage As bc_cs_message
            authentication_failed = False
            If blocaldoc = True Then
                ocommentary = New bc_cs_activity_log("bc_am_at_open_doc", "new", bc_cs_activity_codes.COMMENTARY, "Local Document List Selected.")
                REM check storage method
                If bc_cs_central_settings.file_storage_method_local = bc_cs_central_settings.XML_ALL Then
                    ocommentary = New bc_cs_activity_log("bc_am_at_open_doc", "new", bc_cs_activity_codes.COMMENTARY, "Local Storage method XML_ALL selected.")
                    onetwork_docs = New bc_om_documents(True)
                    If onetwork_docs.check_data_file_exists(bc_cs_central_settings.local_repos_path + "bc_local_documents.dat") = True Then
                        onetwork_docs = onetwork_docs.read_data_from_file(bc_cs_central_settings.local_repos_path + "bc_local_documents.dat")
                    End If
                Else
                    REM implement native and xml_unit later
                    ocommentary = New bc_cs_activity_log("bc_am_at_open_doc", "new", bc_cs_activity_codes.COMMENTARY, "Storage Method: " + bc_cs_central_settings.file_storage_method_local + " not implemented for Local!")
                    omessage = New bc_cs_message("Blue Curve create", "Storage Method: " + bc_cs_central_settings.file_storage_method_local + " not Implemented!", bc_cs_message.MESSAGE)
                End If
            Else
                Dim opf As bc_cs_progress = Nothing
                If show_progress = True Then
                    opf = New bc_cs_progress("Blue Curve create", "Retrieving Network Document List", 6, False, True)
                End If
                onetwork_docs = New bc_om_documents(all)
                onetwork_docs.date_from = date_from
                onetwork_docs.date_to = date_to
                onetwork_docs.show_publish = show_publish
                REM read in network documents
                REM based on connection method
                If bc_cs_central_settings.selected_conn_method = "ado" Then
                    ocommentary = New bc_cs_activity_log("bc_am_at_open_doc", "new", bc_cs_activity_codes.COMMENTARY, "Attempting to get Network Document list via ado")
                    Dim gdbc As New bc_cs_db_services(False)
                    If show_progress = True Then
                        opf.increment("Retrieving Network Document List")
                    End If
                    onetwork_docs.db_read()
                ElseIf bc_cs_central_settings.selected_conn_method = "soap" Then
                    If show_progress = True Then
                        opf.increment("Retrieving Network Document List")
                    End If
                    ocommentary = New bc_cs_activity_log("bc_am_at_open_doc", "new", bc_cs_activity_codes.COMMENTARY, "Attempting to get Network Document list via soap")
                    onetwork_docs.packet_code = "createpoll"
                    onetwork_docs.tmode = bc_cs_soap_base_class.tREAD

                    If onetwork_docs.transmit_to_server_and_receive(onetwork_docs, not_from_service) = False Then
                        If not_from_service = True Then
                            omessage = New bc_cs_message("Blue Curve create", "Network Error Cannot Retrieve Network Documents!", bc_cs_message.MESSAGE)
                        Else
                            authentication_failed = True
                        End If
                        If show_progress = True Then
                            opf.hide()
                        End If
                        Exit Sub
                    End If
                Else
                    omessage = New bc_cs_message("Blue Curve create", "System is working locally cannot Open Documents!", bc_cs_message.MESSAGE)
                End If
                If show_progress = True Then
                    opf.increment("Consolidating Local Document List...")
                End If
                update_local_docs(onetwork_docs)
                If show_progress = True Then
                    opf.unload()
                End If
            End If
        Catch ex As Exception
            If not_from_service = True Then
                Dim db_err As New bc_cs_error_log("bc_am_at_open_doc", "new", bc_cs_error_codes.USER_DEFINED, ex.Message)
            End If
        Finally
            otrace = New bc_cs_activity_log("bc_am_at_open_doc", "new", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    Public Function open(ByVal mode As String, ByRef ldoc As bc_om_document, Optional ByVal visible As Boolean = True, Optional ByVal new_instance As Boolean = False, Optional imp As Boolean = False) As Boolean

        Dim otrace As New bc_cs_activity_log("bc_am_at_open_doc", "open", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim ocommentary As bc_cs_activity_log
        Dim omessage As bc_cs_message
        Dim strfilename As String = ""
        Dim tid As Long
        Try
            open = False
            REM open document based upon mode, storage method and extension
            REM local documents
            Dim fs As New bc_cs_file_transfer_services
            tid = ldoc.id

            If mode = True Then
                If bc_cs_central_settings.file_storage_method_local = bc_cs_central_settings.XML_ALL Then
                    REM extract document
                    REM If Right(ldoc.filename, 4) = ldoc.extension Then
                    REM ldoc.filename = Left(ldoc.filename, Len(ldoc.filename) - 4)
                    REM End If
                    REM strfilename = bc_cs_central_settings.local_repos_path + ldoc.filename + ldoc.extension
                    strfilename = bc_cs_central_settings.local_repos_path + ldoc.filename
                    REM if file exists leave if it doesnt extract from local docs
                    REM PR change May 2007 file is always native never in document object
                    If ldoc.id = 0 Then
                        ldoc.filename = Replace(ldoc.filename, ldoc.extension, "")
                        If Not fs.check_document_exists(bc_cs_central_settings.local_repos_path + ldoc.filename + ldoc.extension) Then
                            omessage = New bc_cs_message("Blue Curve - create", "Document not found: " + bc_cs_central_settings.local_repos_path + ldoc.filename, bc_cs_message.MESSAGE)
                            Exit Function
                            'fs.write_bytestream_to_document(bc_cs_central_settings.local_repos_path + ldoc.filename + ldoc.extension, ldoc.byteDoc)
                        End If
                    Else
                        If Not fs.check_document_exists(bc_cs_central_settings.local_repos_path + ldoc.filename) Then
                            omessage = New bc_cs_message("Blue Curve - create", "Document not found: " + bc_cs_central_settings.local_repos_path + ldoc.filename, bc_cs_message.MESSAGE)
                            Exit Function
                            'fs.write_bytestream_to_document(bc_cs_central_settings.local_repos_path + ldoc.filename + ldoc.extension, ldoc.byteDoc)
                        End If
                        If fs.check_document_exists(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ".dat") Then
                            ldoc = ldoc.read_data_from_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ".dat")
                        End If
                    End If
                    REM update connection mode
                    ldoc.connection_method = bc_cs_central_settings.selected_conn_method
                    'PR sept 2011
                    ldoc.bwith_document = True


                    open = True
                Else
                    ocommentary = New bc_cs_activity_log("bc_am_at_open_doc", "open", bc_cs_activity_codes.COMMENTARY, "Storage Method: " + bc_cs_central_settings.file_storage_method_local + " not implemented for local Documents!")
                End If
            Else

                REM Network Documents
                ldoc.btake_revision = True
                ldoc.bcheck_out = True
                ldoc.bwith_document = True
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    Dim gbc As New bc_cs_db_services(False)
                    ldoc.db_check_out_create_doc()
                    ldoc.local_index = -1
                    ldoc.connection_method = bc_cs_central_settings.selected_conn_method
                    If ldoc.doc_not_found = True Then
                        omessage = New bc_cs_message("Blue Curve - create", "Error With document on server please try again or contact system administrator", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)

                        Exit Function
                    End If
                    open = True
                Else
                    REM request document xml document from server
                    If new_instance = True Then
                        ldoc.packet_code = "asyncopen" + Format(Now, "mmss")
                    Else
                        ldoc.packet_code = "create"
                    End If

                    ocommentary = New bc_cs_activity_log("bc_am_at_open_doc", "open", bc_cs_activity_codes.COMMENTARY, "File Being extracted via soap")
                    ldoc.tmode = bc_cs_soap_base_class.tREAD
                    ldoc.read_mode = bc_om_document.CHECK_OUT_CREATE_DOC
                    If ldoc.transmit_to_server_and_receive(ldoc, True) = False Then
                        Exit Function
                    End If
                    If ldoc.doc_not_found = True Then
                        omessage = New bc_cs_message("Blue Curve - create", "Error With document on server please try again or contact system administrator", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                        recent_change = True
                        Exit Function
                    End If
                    open = True
                    ldoc.local_index = -1
                    ldoc.connection_method = bc_cs_central_settings.selected_conn_method
                    REM write down metadata
                    REM write down metadata
                    'If bc_cs_central_settings.show_authentication_form = 1 Then
                    '    ldoc.logged_on_user_name = bc_cs_central_settings.user_name
                    '    ldoc.logged_on_user_password = bc_cs_central_settings.user_password
                    'End If
                End If
                strfilename = bc_cs_central_settings.local_repos_path + CStr(ldoc.filename)
                REM update connection mode
                ldoc.connection_method = bc_cs_central_settings.selected_conn_method
                If ldoc.id = 0 Then
                    omessage = New bc_cs_message("Blue Curve", "Document has just become checked out to another user cannot check out", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    ldoc.id = tid
                    open = True
                    recent_change = True
                    Exit Function
                End If
                If ldoc.id = -1 Then
                    omessage = New bc_cs_message("Blue Curve", "Document has just had stage changed  cannot check out", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    ldoc.id = tid
                    open = True
                    recent_change = True
                    Exit Function
                End If
            End If

            REM extract document
            If mode = False Then
                fs.write_bytestream_to_document(bc_cs_central_settings.local_repos_path + CStr(ldoc.filename), ldoc.byteDoc, Nothing)
            End If



            Dim strmimetype As String
            strmimetype = ldoc.extension
            ocommentary = New bc_cs_activity_log("bc_am_at_open_doc", "open", bc_cs_activity_codes.COMMENTARY, "Filename Selected: " + strfilename)
            REM AO parts
            If ldoc.extension = WORD_EXT Or ldoc.extension = "[imp]" + WORD_EXT Then
                REM instantiate correct ao object
                odoc = New bc_ao_word
            ElseIf ldoc.extension = WORD2007_EXT Or ldoc.extension = "[imp]" + WORD2007_EXT Or ldoc.extension = "[imp]" + WORD2007_MACRO_EXT Then
                REM instantiate correct ao object
                odoc = New bc_ao_word
            ElseIf ldoc.extension = POWERPOINT_EXT Then
                odoc = New bc_ao_powerpoint
            ElseIf ldoc.extension = POWERPOINT2007_EXT Then
                odoc = New bc_ao_powerpoint
            Else
                REM raise error file type not supported
                omessage = New bc_cs_message("Blue Curve create", "File Type: " + strmimetype + " not supported.", bc_cs_message.MESSAGE)
                Exit Function
            End If
            If visible = False Then
                odoc.invisible()
            End If

            If ldoc.id = 0 Then
                strfilename = Replace(strfilename, ldoc.extension, "")
                odoc.open(strfilename + ldoc.extension, visible, new_instance)
            Else
                odoc.open(strfilename, visible, new_instance)
            End If

            REM FIL JAN 2016 if thsi errors retry 10 times 
            REM then abort open force check in the document and tell the user
            Dim retry As Integer = 1
            Dim dsuccess As Boolean = False
            While retry < 10
                Try

                    odoc.caption(ldoc.title)

                    REM copy activedocument object back 
                    If ldoc.extension = WORD_EXT Or ldoc.extension = "[imp]" + WORD_EXT Then
                        ldoc.docobject = odoc.worddocument
                    ElseIf ldoc.extension = WORD2007_EXT Or ldoc.extension = "[imp]" + WORD2007_EXT Or ldoc.extension = "[imp]" + WORD2007_MACRO_EXT Then
                        ldoc.docobject = odoc.worddocument
                    Else
                        ldoc.docobject = odoc.powerpointpresentation
                    End If

                    Dim intanalystid As Integer = 0
                    Dim strAnalystid As String
                    strAnalystid = odoc.get_property_value("rnet_analyst_id")
                    If strAnalystid <> "" And strAnalystid <> "Error" Then
                        intanalystid = CInt(strAnalystid)
                    Else
                        intanalystid = 0
                    End If

                    dsuccess = True
                    Exit While

                Catch ex As Exception
                    retry = retry + 1
                    Thread.Sleep(100)
                    Dim ocomm As New bc_cs_activity_log("bc_am_at_open_doc", "open", bc_cs_activity_codes.COMMENTARY, "Problem opening doc retrying: " + CStr(retry + 1) + ": " + ex.Message)
                End Try
            End While
            If dsuccess = False Then
                REM force check document back in
                ldoc.mode = bc_om_document.F
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    ldoc.db_write(Nothing)
                Else
                    ldoc.tmode = bc_cs_soap_base_class.tWRITE
                    If ldoc.transmit_to_server_and_receive(ldoc, True) = False Then
                        Dim omsgg As New bc_cs_message("bc_am_at_open_doc", "There is a problem opening the document please retry", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                        odoc.close()
                        Exit Function
                    End If
                End If
                Dim omsg As New bc_cs_message("bc_am_at_open_doc", "There is a problem opening the document please retry", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                odoc.close()
                Exit Function
            End If
            If mode = True Then
                If ldoc.extension = WORD2007_EXT Then
                    ldoc.write_data_to_file(bc_cs_central_settings.local_repos_path + Left(ldoc.filename, Len(ldoc.filename) - 5) + ".dat")
                Else
                    ldoc.write_data_to_file(bc_cs_central_settings.local_repos_path + Left(ldoc.filename, Len(ldoc.filename) - 4) + ".dat")
                End If
            Else
                ldoc.write_data_to_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ".dat")
            End If

            ocommentary = New bc_cs_activity_log("bc_am_at_open_doc", "open", bc_cs_activity_codes.COMMENTARY, "document has never been network submitted so using filename")
            If ldoc.id = 0 Then
                ldoc.filename = Replace(ldoc.filename, ldoc.extension, "")
                odoc.set_doc_id(ldoc.filename)
            Else
                odoc.set_doc_id(ldoc.id)
            End If
            'set pub type
            odoc.set_parameter("rnet_pub_type_id", ldoc.pub_type_id)
            'end fix
            REM set user role
            REM set analyst id as well
            odoc.set_parameter("rnet_analyst_id", bc_cs_central_settings.logged_on_user_id)


            odoc.set_parameter("rnet_stage_id", ldoc.stage)


            Dim role_name As String
            Dim i As Integer
            For i = 0 To bc_am_load_objects.obc_users.user.Count - 1
                If bc_am_load_objects.obc_users.user(i).id = bc_cs_central_settings.logged_on_user_id Then
                    role_name = bc_am_load_objects.obc_users.user(i).role
                    odoc.set_parameter("rnet_user_role_name", role_name.Trim)
                    Exit For
                End If
            Next
            If mode = False And imp = False Then
                write_to_local_store(ldoc)
            End If
            REM safety measure incase file is deleted

            REM if document is locked then lock
            If ldoc.locked = True And visible = True Then
                odoc.lock_document()
            Else
                odoc.unlock_document()


            End If
            REM call vba call in
            odoc.after_open()
            REM write current record to local document store

        Catch ex As Exception
            open = True
            Dim db_err As New bc_cs_error_log("bc_am_at_open_doc", "open", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            tid = ldoc.id = tid

            otrace = New bc_cs_activity_log("bc_am_at_open_doc", "open", bc_cs_activity_codes.TRACE_EXIT, "")

        End Try

    End Function

    Public Function open_read_only(ByVal ldoc As bc_om_document) As Integer
        Dim otrace As New bc_cs_activity_log("bc_am_at_open_doc", "open_read_only", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim ocommentary As bc_cs_activity_log
        Dim omessage As bc_cs_message
        Dim strfilename As String = ""

        Try
            REM open document based upon mode, storage method and extension
            ldoc.bwith_document = True

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                ldoc.db_read_for_create()

            Else
                ldoc.tmode = bc_cs_soap_base_class.tREAD
                ldoc.read_mode = bc_om_document.READ_FOR_CREATE
                If ldoc.transmit_to_server_and_receive(ldoc, False) = False Then
                    omessage = New bc_cs_message("Blue Curve - create", "Error With document on server please try again or contact system administrator", bc_cs_message.MESSAGE)

                    Exit Function
                End If
            End If
            If ldoc.doc_not_found = True Then
                omessage = New bc_cs_message("Blue Curve - create", "Error With document on server please try again or contact system administrator", bc_cs_message.MESSAGE)
                Exit Function
            End If
            REM Network Documents
            'ldoc.btake_revision = False
            'ldoc.bcheck_out = False
            'ldoc.bwith_document = True
            'Dim tdoc_id As Long
            'tdoc_id = ldoc.id
            'If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
            '    Dim gbc As New bc_cs_db_services(False)
            '    ldoc.db_read()
            '    ldoc.local_index = -1
            '    ldoc.connection_method = bc_cs_central_settings.selected_conn_method
            '    If ldoc.doc_not_found = True Then
            '        omessage = New bc_cs_message("Blue Curve - create", "Error With document on server please try again or contact system administrator", bc_cs_message.MESSAGE)
            '        Exit Function
            '    End If
            'Else
            '    REM request document xml document from server
            '    ocommentary = New bc_cs_activity_log("bc_am_at_open_doc", "open", bc_cs_activity_codes.COMMENTARY, "File Being extracted via soap")
            '    ldoc.tmode = bc_cs_soap_base_class.tREAD
            '    If ldoc.transmit_to_server_and_receive(ldoc, True) = False Then
            '        Exit Function
            '    End If

            '    If ldoc.doc_not_found = True Then
            '        omessage = New bc_cs_message("Blue Curve - create", "Error With document on server please try again or contact system administrator", bc_cs_message.MESSAGE)
            '        Exit Function
            '    End If
            '    ldoc.local_index = -1
            '    ldoc.connection_method = bc_cs_central_settings.selected_conn_method

            'End If
            'ldoc.id = tdoc_id
            REM update connection mode
            ldoc.connection_method = bc_cs_central_settings.selected_conn_method
            Dim fs As New bc_cs_file_transfer_services
            Dim view_only_filename As String
            view_only_filename = save_view_only_file(ldoc) + ldoc.extension
            fs.write_bytestream_to_document(bc_cs_central_settings.local_repos_path + view_only_filename, ldoc.byteDoc, Nothing)
            Dim strmimetype As String
            strmimetype = ldoc.extension
            ocommentary = New bc_cs_activity_log("bc_am_at_open_doc", "open_read_inly", bc_cs_activity_codes.COMMENTARY, "Filename Selected: " + strfilename)
            REM AO parts
            If ldoc.extension = WORD_EXT Then
                REM instantiate correct ao object
                odoc = New bc_ao_word
            ElseIf ldoc.extension = WORD2007_EXT Then
                REM instantiate correct ao object
                odoc = New bc_ao_word
            ElseIf ldoc.extension = POWERPOINT_EXT Then
                odoc = New bc_ao_powerpoint
            ElseIf ldoc.extension = POWERPOINT2007_EXT Then
                odoc = New bc_ao_powerpoint
            Else
                REM raise error file type not supported
                omessage = New bc_cs_message("Blue Curve create", "File Type: " + strmimetype + " not supported.", bc_cs_message.MESSAGE)
                Exit Function
            End If
            odoc.open(bc_cs_central_settings.local_repos_path + view_only_filename, True)

            REM set caption
            odoc.caption(view_only_filename)
            REM copy activedocument object back 
            If ldoc.extension = WORD_EXT Then
                ldoc.docobject = odoc.worddocument
            ElseIf ldoc.extension = WORD2007_EXT Then
                ldoc.docobject = odoc.worddocument
            Else
                ldoc.docobject = odoc.powerpointpresentation
            End If
            'Rama Comments: set analyst id as zero in case if there is no analyst existing
            Dim intanalystid As Integer = 0
            Dim strAnalystid As String
            strAnalystid = odoc.get_property_value("rnet_analyst_id")
            If strAnalystid <> "" And strAnalystid <> "Error" Then
                intanalystid = CInt(strAnalystid)
            Else
                intanalystid = 0
            End If


            'PR fix 2-8-20056
            'if document has never been network submitted use filename
            ocommentary = New bc_cs_activity_log("bc_am_at_open_doc", "open", bc_cs_activity_codes.COMMENTARY, "document has never been network submitted so using filename")
            odoc.set_doc_id(ldoc.id)
            'set pub type
            odoc.set_parameter("rnet_pub_type_id", ldoc.pub_type_id)
            'end fix
            REM set user role
            REM set analyst id as well
            odoc.set_parameter("rnet_analyst_id", bc_cs_central_settings.logged_on_user_id)
            odoc.set_parameter("rnet_stage_id", ldoc.stage)

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_at_open_doc", "open_read_only", bc_cs_error_codes.USER_DEFINED, Err.Description)

        Finally
            otrace = New bc_cs_activity_log("bc_am_at_open_doc", "open_read_only", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function
    Public Function save_view_only_file(ByVal ldoc As bc_om_document) As String
        Dim slog As New bc_cs_activity_log("bc_am_at_open_doc", "save_view_only_file", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim fs As New bc_cs_file_transfer_services
            Dim fn As String
            Dim i As Integer = 0
            Dim found As Boolean
            save_view_only_file = ""
            fn = "view" + CStr(i) + ldoc.extension
            found = True
            While found = True
                i = i + 1
                fn = "view" + CStr(i) + ldoc.extension
                REM tidy up other files
                fs.delete_file(bc_cs_central_settings.local_repos_path + fn, Nothing, False)
                If fs.check_document_exists(bc_cs_central_settings.local_repos_path + fn) = False Then
                    fs.write_bytestream_to_document(bc_cs_central_settings.local_repos_path + fn, ldoc.byteDoc, Nothing)
                    found = False
                End If
            End While
            save_view_only_file = "view" + CStr(i)
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_at_open_doc", "save_view_only_file", bc_cs_error_codes.USER_DEFINED, ex.Message)
            save_view_only_file = ""
        Finally
            slog = New bc_cs_activity_log("bc_am_at_open_doc", "save_view_only_file", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function
    Public Sub close()

        odoc.close()
    End Sub
    Private Sub write_to_local_store(ByVal ldoc As bc_om_document)
        Dim otrace As New bc_cs_activity_log("bc_am_at_open_doc", "write_to_local_store", bc_cs_activity_codes.TRACE_ENTRY, "")
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
            Dim db_err As New bc_cs_error_log("bc_am_at_open_doc", "write_to_local_store", bc_cs_error_codes.USER_DEFINED, Err.Description)
        Finally
            otrace = New bc_cs_activity_log("bc_am_at_open_doc", "write_to_local_store", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    Public Sub update_local_docs(ByVal network_doc_list As bc_om_documents)
        Dim otrace As New bc_cs_activity_log("bc_am_at_open_doc", "update_local_docs", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim ocommentary As bc_cs_activity_log
            Dim fs As New bc_cs_file_transfer_services
            Dim local_docs As New bc_om_documents
            Dim docs_exist As Boolean = False
            Dim i, j As Integer
            If fs.check_document_exists_and_can_be_read(bc_cs_central_settings.local_repos_path + "bc_local_documents.dat") Then
                local_docs = local_docs.read_data_from_file(bc_cs_central_settings.local_repos_path + "bc_local_documents.dat")
                docs_exist = True
                REM delete this document
                For j = 0 To network_doc_list.document.Count - 1
                    For i = 0 To local_docs.document.Count - 1
                        REM remove local record if document exists that is checked in 
                        If local_docs.document(i).id = network_doc_list.document(j).id And network_doc_list.document(j).checked_out_user = "0" Then
                            ocommentary = New bc_cs_activity_log("bc_am_at_open_doc", "update_local_docs", bc_cs_activity_codes.COMMENTARY, "Removing Checked in Document: " + local_docs.document(i).title + " from local store.")
                            local_docs.document.RemoveAt(i)
                            Exit For
                        End If
                    Next
                Next
            End If
            If docs_exist = True Then
                REM save back down
                local_docs.write_data_to_file(bc_cs_central_settings.local_repos_path + "bc_local_documents.dat")
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_at_open_doc", "update_local_docs", bc_cs_error_codes.USER_DEFINED, Err.Description)
        Finally

            otrace = New bc_cs_activity_log("bc_am_at_open_doc", "update_local_docs", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
End Class
