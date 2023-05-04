Imports BlueCurve.Core.CS
Imports System.IO

<Serializable()> Public Class bc_om_components_to_display
    Inherits bc_cs_soap_base_class
    Public doc_id As Long
    Public components As New List(Of bc_om_component_to_display)
    Public generate_template As Boolean = False

    Public Overrides Sub process_object()
        Select Case tmode
            Case bc_cs_soap_base_class.tWRITE
                db_write()
        End Select
    End Sub

    Public Sub db_write()
        Dim gdb As New bc_om_style_components_db
        Dim fs As New bc_cs_file_transfer_services
        Dim fn As String
        Dim ocomm As New bc_cs_activity_log("aaa", "bbb", bc_cs_message.MESSAGE, "llll", certificate)
        gdb.clear_display_info(doc_id, MyBase.certificate)
        ocomm = New bc_cs_activity_log("jj", "KKK", bc_cs_message.MESSAGE, "llll", certificate)

        Try
            System.IO.Directory.CreateDirectory(bc_cs_central_settings.central_repos_path + "components/" + CStr(doc_id))
        Catch ex As Exception
            Dim err As New bc_cs_error_log("bc_om_document_components", "db_write_no_tran", bc_cs_error_codes.USER_DEFINED, "Failed to create directory: " + bc_cs_central_settings.central_repos_path + "components/" + CStr(doc_id), certificate)
            Exit Sub
        End Try

        For i = 0 To components.Count - 1
            If IsNothing(components(i).bimage) = False Then
                fn = bc_cs_central_settings.central_repos_path + "components/" + CStr(doc_id) + "/" + components(i).image_name
                fs.write_bytestream_to_document(fn, components(i).bimage, certificate, True)
            End If
            gdb.update_component(components(i).component_id, components(i).ord, components(i).display_order, components(i).desc, components(i).image_name, MyBase.certificate)
        Next
        If generate_template = True Then

            Dim og As New bc_om_document
            og.id = doc_id
            og.html_generation()

        End If
    End Sub
End Class
<Serializable()> Public Class bc_om_component_to_display
    Public component_id As Long
    Public ord As Integer
    Public display_order As Integer
    Public desc As String
    Public image_name
    Public bimage As Byte()

End Class
<Serializable()> Public Class bc_om_user_defined_components
    Inherits bc_cs_soap_base_class
    Public user_id As Long
    Public date_from As Date
    Public date_to As Date
    Public public_flag As Boolean
    Public udcs As New ArrayList
    Public pub_type_id As Long
    Public Overrides Sub process_object()
        Select Case tmode
            Case bc_cs_soap_base_class.tREAD
                db_read()
        End Select
    End Sub
    Public Sub db_read()
        REM retrieve all UDCs that are private to user
        REM or public and  against a pub type in users bus area
        Try

            Dim vres As Object
            Dim ocomp As bc_om_user_defined_component
            udcs.Clear()
            Dim gdb As New bc_om_style_components_db
            vres = gdb.get_user_defined_comps(Me.pub_type_id, Me.user_id, Me.date_from, Me.date_to, MyBase.certificate)
            If IsArray(vres) Then
                For i = 0 To UBound(vres, 2)
                    ocomp = New bc_om_user_defined_component
                    ocomp.udc_id = vres(0, i)
                    ocomp.title = vres(1, i)
                    ocomp.rtf = vres(2, i)
                    ocomp.comp_date = vres(3, i)
                    ocomp.pub_type = vres(4, i)
                    ocomp.author = vres(5, i)
                    ocomp.entity = vres(6, i)
                    ocomp.author_id = vres(7, i)
                    ocomp.public_flag = vres(8, i)
                    ocomp.sdc = vres(9, i)
                    ocomp.for_class = vres(10, i)
                    udcs.Add(ocomp)
                Next
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_user_defined_components", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, MyBase.certificate)
        End Try
    End Sub
End Class

<Serializable()> Public Class bc_om_user_defined_component
    Inherits bc_cs_soap_base_class
    Public udc_id As Long = 0
    Public title As String
    Public comp_date As Date
    Public rtf As String
    Public public_flag As Boolean = False
    Public sdc As Integer = 0
    Public pub_type_id As Long
    Public entity_id As Long
    Public author_id As Long
    Public pub_type As String
    Public entity As String
    Public author As String
    Public language_id As Long
    Public duplicate_title As Boolean = False
    Public for_class As Long
    Public last_updated_from_doc_id As String
    Public write_mode As Integer = 1
    Public Const INSERT = 1
    Public Const DELETE = 2
    Public Overrides Sub process_object()
        Select Case tmode
            Case bc_cs_soap_base_class.tREAD
                db_read()
            Case bc_cs_soap_base_class.tWRITE
                db_write()
        End Select
    End Sub
    Public Sub db_read()
        Try
            Dim gdb As New bc_om_style_components_db
            Dim vres As Object
            vres = gdb.get_user_defined_component(Me.udc_id, MyBase.certificate)
            If IsArray(vres) Then
                If UBound(vres, 2) = 0 Then
                    Me.title = vres(0, 0)
                    Me.public_flag = vres(1, 0)
                    Me.author_id = vres(2, 0)
                Else
                    Me.udc_id = 0
                End If
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_user_defined_component", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, MyBase.certificate)
        End Try
    End Sub
    Public Sub db_write()
        Try
            Dim gdb As New bc_om_style_components_db
            Select Case write_mode
                Case INSERT
                    udc_id = gdb.insert_user_defined_component(Me.udc_id, Me.title, Me.rtf, Me.comp_date, Me.public_flag, Me.pub_type_id, Me.entity_id, Me.author_id, Me.last_updated_from_doc_id, MyBase.certificate)
                    If udc_id = -1 Then
                        Me.duplicate_title = True
                    End If
                Case DELETE
                    gdb.delete_udc(Me.udc_id, MyBase.certificate)
            End Select
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_user_defined_components", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message, MyBase.certificate)
        End Try
    End Sub

    Public Sub New()

    End Sub
End Class

<Serializable()> Public Class bc_om_component_documents
    Inherits bc_cs_soap_base_class
    Public user_id As Long
    Public date_from As Date
    Public date_to As Date
    Public documents As New ArrayList

    Public Overrides Sub process_object()
        Select Case tmode
            Case bc_cs_soap_base_class.tREAD
                db_read()
        End Select

    End Sub
    Public Sub db_read()
        Try
            Dim gdb As New bc_om_style_components_db
            Dim vres As Object
            Dim odoc As bc_om_component_document
            documents.Clear()
            vres = gdb.get_document_list(user_id, Format(date_from, "dd-MMM-yyyy 00:00"), Format(date_to, "dd-MMM-yyyy 23:59"), MyBase.certificate)
            If IsArray(vres) Then
                For i = 0 To UBound(vres, 2)
                    odoc = New bc_om_component_document
                    odoc.doc_id = vres(0, i)
                    odoc.doc_date = vres(1, i)
                    odoc.title = vres(2, i)
                    odoc.pub_type_name = vres(3, i)
                    odoc.stage = vres(4, i)
                    odoc.author = vres(5, i)
                    odoc.entity = vres(6, i)
                    documents.Add(odoc)
                Next
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_component_documents", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, MyBase.certificate)
        End Try
    End Sub

End Class
<Serializable()> Public Class bc_om_component_document
    Public doc_id As Long
    Public doc_date As DateTime
    Public title As String
    Public pub_type_name As String
    Public entity As String
    Public author As String
    Public stage As String
    <NonSerialized()> Public search As Boolean = False

    Public Sub New()

    End Sub
End Class

<Serializable()> Public Class bc_om_component_search
    Inherits bc_cs_soap_base_class
    Public search_in_doc_ids As New ArrayList
    Public result_doc_ids As New ArrayList

    Public search_text As String
    Public Overrides Sub process_object()
        Select Case tmode
            Case bc_cs_soap_base_class.tREAD
                db_read()
        End Select

    End Sub
    Public Sub db_read()
        Try
            Dim gdb As New bc_om_style_components_db
            Dim vres As Object
            Dim dl As String = ""

            result_doc_ids.Clear()

            For i = 0 To search_in_doc_ids.Count - 1
                If i = 0 Then
                    dl = CStr(search_in_doc_ids(i))
                Else
                    dl = dl + ", " + CStr(search_in_doc_ids(i))
                End If
            Next

            vres = gdb.get_docs_that_contain_component(search_text, dl, MyBase.certificate)


            If IsArray(vres) Then
                For i = 0 To UBound(vres, 2)
                    result_doc_ids.Add(vres(0, i))
                Next
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_component_search", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, MyBase.certificate)
        End Try
    End Sub
End Class

<Serializable()> Public Class bc_om_document_components
    Inherits bc_cs_soap_base_class
    Public doc_id As String
    Public bwrite_to_json_file As Boolean = True
    Public success As Boolean = True
    Public components As New List(Of bc_om_style_component)
    Public template_id As Long
    Public err_text As String = ""
    Public table_charts_only As Boolean
    Public byteDoc As Byte()
    Public table_on_demand As Boolean = False

    Public Sub New()
    End Sub
    Public Overrides Sub process_object()
        Select Case tmode
            Case bc_cs_soap_base_class.tREAD
                db_read()
                'Case bc_cs_soap_base_class.tWRITE
                '    db_write_no_tran()

        End Select

    End Sub
    Public Sub db_read()
        Try
            Dim gdb As New bc_om_style_components_db
            Dim comps, pcomps As Object
            Dim comp As bc_om_style_component
            Dim pcomp As bc_om_style_component_paragraph
            Dim tod As Object

            tod = gdb.get_table_on_demand(Me.doc_id, MyBase.certificate)
            If IsArray(tod) Then
                If UBound(tod, 2) = 0 Then
                    table_on_demand = tod(0, 0)
                End If
            End If


            comps = gdb.get_components_for_document(Me.doc_id, MyBase.certificate)

            If IsArray(comps) Then
                For i = 0 To UBound(comps, 2)
                    comp = New bc_om_style_component
                    comp.component_id = comps(0, i)
                    comp.text = comps(1, i)
                    comp.ortf = comps(2, i)
                    comp.style = comps(3, i)
                    comp.page_no = comps(4, i)
                    comp.bimage = comps(5, i)
                    comp.btable = comps(6, i)
                    comp.text_box_id = comps(7, i)
                    comp.display_order = comps(8, i)
                    comp.description = comps(9, i)
                    REM if table now read table
                    REM to see if there is  chart in it

                    If comp.btable = True Then
                        comp.chart_in_table = gdb.chart_in_table(comp.component_id, 0, MyBase.certificate)
                    End If
                    pcomps = gdb.get_paragraphs_for_component(comp.component_id, MyBase.certificate)
                    If IsArray(comps) Then
                        For j = 0 To UBound(pcomps, 2)
                            pcomp = New bc_om_style_component_paragraph
                            pcomp.ord = pcomps(0, j)
                            pcomp.text = pcomps(1, j)
                            pcomp.ortf = pcomps(2, j)
                            pcomp.style = pcomps(3, j)
                            pcomp.page_no = pcomps(4, j)
                            pcomp.bimage = pcomps(5, j)
                            pcomp.btable = pcomps(6, j)
                            pcomp.display_order = pcomps(7, j)
                            pcomp.description = pcomps(8, j)
                            pcomp.table_locator = pcomps(9, j)

                            ' If Me.table_charts_only = False Or (Me.table_charts_only = True And (pcomp.bimage = True Or pcomp.btable = True)) Then
                            If pcomp.btable = True Then
                                pcomp.chart_in_table = gdb.chart_in_table(comp.component_id, pcomp.ord, MyBase.certificate)
                            End If
                            comp.paragraphs.Add(pcomp)
                            ' End If
                        Next
                    End If
                    Me.components.Add(comp)
                Next
                REM now read document
                REM JL omnly get document if components exist
                Dim fs As New bc_cs_file_transfer_services
                If UBound(comps, 2) >= 0 And table_on_demand = True Then
                    fs.write_document_to_bytestream(bc_cs_central_settings.central_repos_path + CStr(doc_id) + ".docx", Me.byteDoc, MyBase.certificate)
                End If
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_document_components", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, MyBase.certificate)
        End Try
    End Sub
    Public Function db_write(ByRef db As bc_cs_db_services, ByRef commit_files As ArrayList)
        Try
            db_write = False
            Dim gdb As New bc_om_style_components_db
            Dim comp_id, ord As Long
            Dim fname As String = ""
            Dim commit_file As bc_om_files_to_commit = Nothing
            Dim path As String = bc_cs_central_settings.central_repos_path + "\components\" + CStr(doc_id)
            gdb.seal_existing_data(db, Me.doc_id, certificate)
            If gdb.err_text <> "" Then
                err_text = gdb.err_text
                Exit Function
            End If
            gdb.set_template_for_document(db, Me.doc_id, Me.template_id, certificate)
            If gdb.err_text <> "" Then
                err_text = gdb.err_text
                Exit Function
            End If
            REM table
            For i = 0 To components.Count - 1
                With components(i)
                    comp_id = gdb.write_component_for_doc(db, Me.doc_id, .text, .rtf, .compressed_html, .style, .page_no, .btable, .bimage, .text_box_id, .is_html, .html, .css, .width, .location, MyBase.certificate)
                    If gdb.err_text <> "" Then
                        err_text = gdb.err_text
                        Exit Function
                    End If
                    REM table
                    If .btable = True Then
                        For k = 0 To .table_cells.Count - 1
                            gdb.write_component_paragraph_cell(db, Me.doc_id, comp_id, 0, .table_cells(k).row, .table_cells(k).col, .table_cells(k).paragraph, .table_cells(k).bimage, .table_cells(k).text, .table_cells(k).style, certificate)
                            If gdb.err_text <> "" Then
                                err_text = gdb.err_text
                                Exit Function
                            End If
                            REM 
                            fname = CStr(Me.doc_id) + "_" + CStr(comp_id) + "_" + CStr(ord) + ".png"
                            'Dim fs As New bc_cs_file_transfer_services
                            If .table_cells(k).bimage = True Then

                                fname = CStr(Me.doc_id) + "_" + CStr(comp_id) + "_0_" + CStr(.table_cells(k).row) + "_" + CStr(.table_cells(k).col) + "_" + CStr(.table_cells(k).paragraph) + ".png"
                                commit_file = New bc_om_files_to_commit
                                commit_file.mode = bc_om_files_to_commit.IMAGE
                                commit_file.target_dir = path
                                commit_file.target_file = fname

                                commit_file.bimage = .table_cells(k).image
                                commit_files.Add(commit_file)

                                gdb.write_image_to_db(db, Me.doc_id, comp_id, 0, .table_cells(k).row, .table_cells(k).col, .table_cells(k).paragraph, .table_cells(k).image, certificate)


                                'System.IO.Directory.CreateDirectory(path)
                                'If fs.write_bytestream_to_document(path + "\" + fname, .table_cells(k).image, certificate, False) = False Then
                                '    Dim ocomm As New bc_cs_activity_log("bc_om_documents_components", "db_write", bc_cs_activity_codes.COMMENTARY, "Failed to write component image" + bc_cs_central_settings.central_repos_path + "component_images" + "\" + fname, certificate)
                                'End If
                            End If
                        Next
                    End If
                    REM chart
                    If .bimage = True Then
                        fname = CStr(Me.doc_id) + "_" + CStr(comp_id) + ".png"
                        commit_file = New bc_om_files_to_commit
                        commit_file.mode = bc_om_files_to_commit.IMAGE
                        commit_file.target_dir = path
                        commit_file.target_file = fname
                        commit_file.bimage = .image
                        commit_files.Add(commit_file)
                        gdb.write_image_to_db(db, Me.doc_id, comp_id, 0, 0, 0, 0, .image, certificate)

                    End If
                    If comp_id > 0 Then
                        REM now do paragraphs
                        For j = 0 To .paragraphs.Count - 1
                            ord = gdb.write_component_paragraph(db, comp_id, .paragraphs(j).text, .paragraphs(j).rtf, .paragraphs(j).compressed_html, .paragraphs(j).style, .paragraphs(j).page_no, .paragraphs(j).bimage, .paragraphs(j).btable, .paragraphs(j).table_locator, MyBase.certificate)
                            If gdb.err_text <> "" Then
                                err_text = gdb.err_text
                                Exit Function
                            End If
                            REM table
                            REM table
                            If .paragraphs(j).btable = True Then
                                For k = 0 To .paragraphs(j).table_cells.Count - 1
                                    gdb.write_component_paragraph_cell(db, Me.doc_id, comp_id, ord, .paragraphs(j).table_cells(k).row, .paragraphs(j).table_cells(k).col, .paragraphs(j).table_cells(k).paragraph, .paragraphs(j).table_cells(k).bimage, .paragraphs(j).table_cells(k).text, .paragraphs(j).table_cells(k).style, certificate)
                                    If gdb.err_text <> "" Then
                                        err_text = gdb.err_text
                                        Exit Function
                                    End If
                                    REM 
                                    fname = CStr(Me.doc_id) + "_" + CStr(comp_id) + "_" + CStr(ord) + ".png"
                                    If .paragraphs(j).table_cells(k).bimage = True Then
                                        fname = CStr(Me.doc_id) + "_" + CStr(comp_id) + "_" + CStr(ord) + "_" + CStr(.paragraphs(j).table_cells(k).row) + "_" + CStr(.paragraphs(j).table_cells(k).col) + "_" + CStr(.paragraphs(j).table_cells(k).paragraph) + ".png"
                                        commit_file = New bc_om_files_to_commit
                                        commit_file.mode = bc_om_files_to_commit.IMAGE
                                        commit_file.target_dir = path
                                        commit_file.target_file = fname
                                        commit_file.bimage = .paragraphs(j).table_cells(k).image
                                        commit_files.Add(commit_file)

                                    End If
                                Next
                                REM chart
                            ElseIf .paragraphs(j).bimage = True Then
                                fname = CStr(Me.doc_id) + "_" + CStr(comp_id) + "_" + CStr(ord) + ".png"
                                commit_file = New bc_om_files_to_commit
                                commit_file.mode = bc_om_files_to_commit.IMAGE
                                commit_file.target_dir = path
                                commit_file.target_file = fname
                                commit_file.bimage = .paragraphs(j).image
                                commit_files.Add(commit_file)
                            End If
                        Next
                    End If
                End With
            Next
            Dim user_id As Long
            If bc_cs_central_settings.server_flag = 0 Then
                user_id = bc_cs_central_settings.logged_on_user_id
            Else
                user_id = certificate.user_id
            End If
            Dim res As Object
            res = gdb.call_out_after_componetize_write(Me.doc_id, user_id, db, certificate)
            If gdb.err_text <> "" Then
                err_text = gdb.err_text
                Exit Function
            End If
            REM see if email preview required
            If IsArray(res) AndAlso UBound(res, 2) = 0 Then
                Dim email_template_id As Integer
                email_template_id = res(0, 0)
                If email_template_id > 0 Then
                    Dim et As New bc_cs_email_preview_services(certificate)
                    If et.create_preview(0, email_template_id, doc_id, 0, 0, db) = False Then
                        success = False
                        err_text = "failed to generate html file: " + et.err_txt
                    End If
                End If
            End If




        Catch ex As Exception
            success = False
            err_text = "bc_om_document_components:db_write" + ex.Message
        End Try
    End Function
    REM older code bases pre 5.5
    'Class files_to_commit
    '    Public Const CONTROL_FILE = 1
    '    Public Const FILE_COPY = 2
    '    Public Const DELETE = 3
    '    Public Const IMAGE = 4
    '    Public mode As Integer
    '    Public contents As String
    '    Public target_file As String
    '    Public source_file As String
    '    Public target_dir As String
    '    Public bimage As Byte()

    '    Public Sub New()

    '    End Sub
    'End Class
  
End Class
<Serializable()> Public MustInherit Class bc_om_style_component_base_class
    Public text As String = ""
    Public rtf As Byte() = Nothing
    Public ortf As String
    Public compressed_html As Byte() = Nothing
    Public style As String

    REM Public header As Boolean = False
    Public page_no As String

    Public btable As Boolean = False
    Public table_cells As New List(Of bc_om_style_component_table_cell)
    Public bimage As Boolean = False
    Public image As Byte()
    Public display_order As Integer
    Public description As String
    Public chart_in_table As Boolean = False
    Public table_locator As String
    Public is_html As Boolean = False
    Public html As String = ""
    Public css As String = ""
    Public width As String = ""
    Public location As String = ""

    Public Sub New()

    End Sub
End Class

<Serializable()> Public Class bc_om_style_component
    Inherits bc_om_style_component_base_class

    Public paragraphs As New List(Of bc_om_style_component_paragraph)
    Public component_id As Integer
    Public text_box_id As Integer = 0
    Public Sub New()

    End Sub
End Class
<Serializable()> Public Class bc_om_style_component_paragraph
    Inherits bc_om_style_component_base_class
    Public ord As Integer
    Public Sub New()

    End Sub
End Class
'<Serializable()> Public Class bc_om_style_component_table
'    Public component_id As Integer
'    Public order As Integer
'    Public row As Integer
'    Public col As Integer
'    Public text As String
'    Public style As String
'    Public image As Boolean = False
'    Public image_filename As String
'    Public Sub New()

'    End Sub
'End Class
<Serializable()> Public Class bc_om_style_component_table_cell
    Public component_id As Integer
    Public order As Integer
    Public row As Integer
    Public col As Integer
    Public paragraph As Integer
    Public text As String
    Public style As String
    Public bimage As Boolean = False
    Public image As Byte()

    Public Sub New()

    End Sub
End Class
<Serializable()> Public Class bc_om_html_componetize_settings_for_template
    Inherits bc_cs_soap_base_class
    Public template_id As Long
   
    Public s_table_as_html_body As Boolean = True
    Public s_text_as_html_body As Boolean = False
    Public s_table_as_html_text_boxes As Boolean = True
    Public s_text_as_html_text_boxes As Boolean = False

    Public s_search_main_body As Boolean = True
    Public s_search_text_boxes As Boolean = False
    Public s_text As Boolean = True
    Public s_tables As Boolean = True
    Public s_images As Boolean = True

    Public s_pages_up_to As Integer = 0
    Public s_images_inline As Boolean = False

    Public Overrides Sub process_object()
        Select Case tmode
            Case bc_cs_soap_base_class.tREAD
                db_read()
            Case bc_cs_soap_base_class.tWRITE
                db_write()
        End Select

    End Sub

    Public Sub db_read()
        Try
            Dim gdb As New bc_om_style_components_db
            Dim vres

            vres = gdb.get_settings(template_id, MyBase.certificate)
            If IsArray(vres) Then
                If UBound(vres, 2) = 0 Then
                    s_search_main_body = vres(0, 0)
                    s_search_text_boxes = vres(1, 0)
                    If s_search_main_body = True Then
                        s_table_as_html_body = vres(2, 0)
                        s_text_as_html_body = vres(3, 0)
                    End If
                    If s_search_text_boxes = True Then
                        s_table_as_html_text_boxes = vres(4, 0)
                        s_text_as_html_text_boxes = vres(5, 0)
                    End If
                   
                    s_text = vres(6, 0)
                    s_tables = vres(7, 0)
                    s_images = vres(8, 0)
                    s_pages_up_to = vres(9, 0)
                    s_images_inline = vres(10, 0)
                End If
            End If
          
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_html_componetize_settings_for_template", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Public Sub db_write()
        REM Blue printz bit

    End Sub
End Class
<Serializable()> Public Class bc_om_componetize_styles_for_template
    Inherits bc_cs_soap_base_class
    Public template_id As Long
    Public search_headers As Boolean = True
    Public search_text_boxes As Boolean = True
    Public rtf As Boolean = False
    Public html As Boolean = False
    Public tables As Boolean = False
    Public images As Boolean = False
    Public header_styles_only As Boolean = False
    Public all_styles As Boolean = False
    Public styles As New ArrayList
    Public preview_rtf As Boolean = False
    Public found As Boolean = False
    Public table_as_image As Boolean = False
    Public table_on_demand As Boolean = False
    Public Overrides Sub process_object()
        Select Case tmode
            Case bc_cs_soap_base_class.tREAD
                db_read()
            Case bc_cs_soap_base_class.tWRITE
                db_write()
        End Select

    End Sub

    Public Sub db_read()
        Try
            Dim gdb As New bc_om_style_components_db
            Dim vres, bres As Object
            Dim ostyle As bc_om_componetize_style
            vres = gdb.get_parse_areas(template_id, MyBase.certificate)
            If IsArray(vres) Then
                If UBound(vres, 2) = 0 Then
                    search_headers = vres(0, 0)
                    search_text_boxes = vres(1, 0)
                    rtf = vres(2, 0)
                    html = vres(3, 0)
                    tables = vres(4, 0)
                    images = vres(5, 0)
                    header_styles_only = vres(6, 0)
                    all_styles = vres(7, 0)
                    preview_rtf = vres(8, 0)
                    table_as_image = vres(9, 0)
                    table_on_demand = vres(10, 0)
                    found = True
                End If
            End If
            If all_styles = False Then
                styles.Clear()
                vres = gdb.get_header_style_for_template(template_id, MyBase.certificate)
                If IsArray(vres) Then
                    For i = 0 To UBound(vres, 2)
                        ostyle = New bc_om_componetize_style
                        ostyle.header_style = vres(0, i)
                        bres = gdb.get_body_style_for_header(template_id, ostyle.header_style, MyBase.certificate)
                        If IsArray(bres) Then
                            For j = 0 To UBound(bres, 2)
                                ostyle.body_style.Add(bres(0, j))
                            Next
                        End If
                        styles.Add(ostyle)
                    Next
                End If
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_componetize_styles_for_template", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Public Sub db_write()
        REM Blue printz bit

    End Sub
End Class
<Serializable()> Public Class bc_om_componetize_style
    Public header_style As String
    REM rem if true and in table then only takes the cell not entire table
    Public cell_only As Boolean = False
    Public body_style As New ArrayList
End Class
Friend Class bc_om_style_components_db
    Private gbc_db As New bc_cs_db_services(False)
    Public err_text As String
    Public Function chart_in_table(ByVal component_id As Integer, ByVal ord As Integer, ByRef certificate As bc_cs_security.certificate) As Boolean
        Dim res As Object
        Dim sql As String
        chart_in_table = False
        sql = "exec  dbo.bc_core_sc_chart_in_table " + CStr(component_id) + "," + CStr(ord)
        res = gbc_db.executesql(sql, certificate)
        If IsArray(res) Then
            If res(0, 0) = 1 Then
                chart_in_table = True
            End If
        End If
    End Function

    Public Function call_out_after_componetize_write(ByVal doc_id As Long, ByVal user_id As Long, ByRef db As bc_cs_db_services, ByRef certificate As bc_cs_security.certificate)
        Dim res As Object
        Dim sql As String
        sql = "exec  dbo.bc_core_sc_after_write_down " + CStr(doc_id) + "," + CStr(user_id)
        If IsNothing(db) Then
            res = gbc_db.executesql(sql, certificate)
        Else
            res = db.executesql_trans(sql, certificate, True)
            If db.success = False Then
                err_text = CStr(res(0, 0))
                Exit Function
            End If
        End If
        Return res
    End Function
    Public Function write_component_for_doc(ByRef db As bc_cs_db_services, ByVal doc_id As String, ByVal title As String, ByVal compressed_rtf As Object, ByVal compressed_html As Object, ByVal style As String, ByVal page_no As String, ByVal table As Boolean, ByVal image As Boolean, ByVal text_box_id As Long, is_html As Boolean, html As String, css As String, width As String, location As String, ByRef certificate As bc_cs_security.certificate) As Long
        Dim sql As String
        Dim res As Object
        Dim comp_id As Long
        Try
            Dim str As bc_cs_string_services
            Dim srtf As String = ""
            Dim shtml As String = ""
            If Not IsNothing(compressed_rtf) Then
                Dim bcs As New bc_cs_security
                srtf = bcs.decompress_xml_winzip(compressed_rtf, certificate)
            End If
            If Not IsNothing(compressed_html) Then
                Dim bcs As New bc_cs_security
                shtml = bcs.decompress_xml_winzip(compressed_html, certificate)
            End If


            str = New bc_cs_string_services(title)
            title = str.delimit_apostrophies
            str = New bc_cs_string_services(srtf)
            srtf = str.delimit_apostrophies
            str = New bc_cs_string_services(shtml)
            shtml = str.delimit_apostrophies
            str = New bc_cs_string_services(style)
            style = str.delimit_apostrophies
            str = New bc_cs_string_services(page_no)
            page_no = str.delimit_apostrophies
            str = New bc_cs_string_services(html)
            html = str.delimit_apostrophies
            str = New bc_cs_string_services(css)
            css = str.delimit_apostrophies
            If is_html = False Then
                sql = "exec  dbo.bc_core_sc_insert_component_value " + CStr(doc_id) + ",N'" + title + "',N'" + srtf + "',N'" + shtml + "','" + style + "'," + page_no + "," + CStr(image) + "," + CStr(table) + "," + CStr(text_box_id) + ",0,'','','" + width + "','" + location + "'"
            Else
                sql = "exec  dbo.bc_core_sc_insert_component_value " + CStr(doc_id) + ",N'" + title + "',N'" + srtf + "',N'" + shtml + "','" + style + "'," + page_no + "," + CStr(image) + "," + CStr(table) + "," + CStr(text_box_id) + ",1,N'" + html + "',N'" + css + "','" + width + "','" + location + "'"
            End If
            If IsNothing(db) Then
                res = gbc_db.executesql(sql, certificate)
            Else
                res = db.executesql_trans(sql, certificate, True)
                If db.success = False Then
                    err_text = CStr(res(0, 0))
                    Exit Function
                End If
            End If



            comp_id = res(0, 0)
            write_component_for_doc = comp_id
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log(" bc_om_style_components_db", "write_component_for_doc", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)

        End Try
    End Function
    Friend Sub seal_existing_data(ByRef db As bc_cs_db_services, ByVal doc_id As String, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        Dim res As Object
        sql = "exec dbo.bc_core_sc_deal_existing_components " + CStr(doc_id)
        If IsNothing(db) Then
            res = gbc_db.executesql(sql, certificate)
        Else
            res = db.executesql_trans(sql, certificate, True)
            If db.success = False Then
                err_text = CStr(res(0, 0))
                Exit Sub
            End If
        End If
    End Sub
    Friend Sub set_template_for_document(ByRef db As bc_cs_db_services, ByVal doc_id As String, ByRef template_id As String, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        Dim res As Object
        sql = "exec dbo.bc_core_sc_set_template_for_doc " + CStr(doc_id) + "," + CStr(template_id)
        If IsNothing(db) Then
            res = gbc_db.executesql(sql, certificate)
        Else
            res = db.executesql_trans(sql, certificate, True)
            If db.success = False Then
                err_text = CStr(res(0, 0))
                Exit Sub
            End If
        End If
    End Sub

    Friend Function write_component_paragraph(ByRef db As bc_cs_db_services, ByVal component_id As String, ByVal text As String, ByVal compressed_rtf As Object, ByVal compressed_html As Object, ByVal style As String, ByVal page_no As String, ByVal image As Boolean, ByVal table As Boolean, ByVal table_locator As String, ByRef certificate As bc_cs_security.certificate)
        Try
            Dim sql As String
            Dim ord As Integer

            Dim srtf As String = ""
            Dim shtml As String = ""
            If Not IsNothing(compressed_rtf) Then
                Dim bcs As New bc_cs_security
                srtf = bcs.decompress_xml_winzip(compressed_rtf, certificate)
            End If
            If Not IsNothing(compressed_html) Then
                Dim bcs As New bc_cs_security
                shtml = bcs.decompress_xml_winzip(compressed_html, certificate)
            End If

            Dim str As bc_cs_string_services
            str = New bc_cs_string_services(text)
            text = str.delimit_apostrophies
            str = New bc_cs_string_services(srtf)
            srtf = str.delimit_apostrophies
            str = New bc_cs_string_services(shtml)
            shtml = str.delimit_apostrophies
            str = New bc_cs_string_services(style)
            style = str.delimit_apostrophies
            str = New bc_cs_string_services(page_no)
            page_no = str.delimit_apostrophies

            sql = "exec dbo.bc_core_sc_insert_component_value_paragraph " + CStr(component_id) + ",N'" + text + "',N'" + srtf + "',N'" + shtml + "','" + style + "'," + CStr(page_no) + "," + CStr(image) + "," + CStr(table) + ",'" + table_locator + "'"

            Dim res As Object
            If IsNothing(db) Then
                res = gbc_db.executesql(sql, certificate)
            Else
                res = db.executesql_trans(sql, certificate, True)
                If db.success = False Then
                    err_text = CStr(res(0, 0))
                    Exit Function
                End If
            End If
            write_component_paragraph = res(0, 0)

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_style_components_db", "write_component_paragraph", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)

        End Try
    End Function
    Friend Sub write_image_to_db(ByRef db As bc_cs_db_services, ByVal doc_id As Long, ByVal component_id As Long, ByVal ord As Integer, ByVal row As Integer, ByVal col As Integer, ByVal paragraph As Integer, ByVal image As Byte(), ByRef certificate As bc_cs_security.certificate)
        Try
            REM turn off for now
            Exit Sub
            Dim sql As String
            Dim res As Object
            Dim fs As New bc_cs_file_transfer_services
            Dim fimage As String

            fimage = Convert.ToBase64String(image)

            sql = "exec dbo.bc_core_style_component_add_image " + CStr(doc_id) + "," + CStr(component_id) + "," + CStr(ord) + "," + CStr(row) + "," + CStr(col) + "," + CStr(paragraph) + ",'" + CStr(fimage) + "'"
            If IsNothing(db) Then
                res = gbc_db.executesql(sql, certificate)
            Else
                res = db.executesql_trans(sql, certificate, True)
                If db.success = False Then
                    err_text = CStr(res(0, 0))
                    Exit Sub
                End If
            End If
        Catch ex As Exception

            Dim oerr As New bc_cs_error_log("bc_om_style_components_db", "write_component_paragraph_cell", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        End Try

    End Sub

    Friend Sub write_component_paragraph_cell(ByRef db As bc_cs_db_services, ByVal doc_id As Long, ByVal component_id As Long, ByVal ord As Integer, ByVal row As Integer, ByVal col As Integer, ByVal paragraph As Integer, ByVal image As Boolean, ByVal text As String, ByVal style As String, ByRef certificate As bc_cs_security.certificate)
        Try
            Dim sql As String
            Dim fs As New bc_cs_string_services(text)
            Dim res As Object
            text = fs.delimit_apostrophies
            fs = New bc_cs_string_services(style)
            style = fs.delimit_apostrophies

            sql = "exec dbo.bc_core_style_component_add_table_cell " + CStr(doc_id) + "," + CStr(component_id) + "," + CStr(ord) + "," + CStr(row) + "," + CStr(col) + "," + CStr(paragraph) + "," + CStr(image) + ",N'" + text + "','" + style + "'"
            If IsNothing(db) Then
                res = gbc_db.executesql(sql, certificate)
            Else
                res = db.executesql_trans(sql, certificate, True)
                If db.success = False Then
                    err_text = CStr(res(0, 0))
                    Exit Sub
                End If
            End If
        Catch ex As Exception

            Dim oerr As New bc_cs_error_log("bc_om_style_components_db", "write_component_paragraph_cell", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        End Try

    End Sub
    Friend Function get_user_defined_component(ByVal udc_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Try
            Dim sql As String
            sql = "select title, public_flag, author_id from  bc_core_user_def_comp where udc_id=" + CStr(udc_id)
            get_user_defined_component = gbc_db.executesql(sql, certificate)
        Catch ex As Exception
            get_user_defined_component = Nothing
            Dim oerr As New bc_cs_error_log("bc_om_style_components_db", " get_user_defined_component", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        End Try
    End Function
    Friend Function insert_user_defined_component(ByVal udc_id As Long, ByVal title As String, ByVal rtf As String, ByVal update_date As Date, ByVal public_flag As Boolean, ByVal pub_type_id As Long, ByVal entity_id As Long, ByVal author_id As Long, ByVal doc_id As Long, ByRef certificate As bc_cs_security.certificate) As Long
        Try
            Dim sql As String
            Dim vres As Object
            Dim ostr As New bc_cs_string_services(title)
            title = ostr.delimit_apostrophies
            ostr = New bc_cs_string_services(rtf)
            rtf = ostr.delimit_apostrophies
            REM firstly check title is unique
            sql = "select count(*) from bc_core_user_def_comp where title='" + title + "' and (author_id=" + CStr(author_id) + " or public_flag=1)"
            vres = gbc_db.executesql(sql, certificate)
            If IsArray(vres) Then
                If UBound(vres, 2) = 0 Then
                    If CInt(vres(0, 0)) > 0 Then
                        insert_user_defined_component = -1
                        Exit Function
                    End If
                End If
            End If

            Dim gdb As New bc_om_refresh_components_db
            REM 0 means new
            Dim ipf As Integer
            ipf = 0
            If public_flag = True Then
                ipf = 1
            End If
            udc_id = gdb.update_udc("", 0, rtf, doc_id, title, ipf, pub_type_id, author_id, entity_id, certificate)
            insert_user_defined_component = udc_id

            'update_udc(ByVal type As Long, ByVal value As String, ByVal doc_id As Long, ByRef certificate As bc_cs_security.certificate) As Integer


            'If udc_id = 0 Then
            '    sql = "select coalesce(max(udc_id),0) + 1 from bc_core_user_def_comp"
            '    vres = gbc_db.executesql(sql, certificate)
            '    udc_id = vres(0, 0)
            'Else
            '    sql = "delete from bc_core_user_def_comp where udc_id=" + CStr(udc_id)
            '    vres = gbc_db.executesql(sql, certificate)
            'End If
            'Dim ipf As Integer
            'ipf = 0
            'If public_flag = True Then
            '    ipf = 1
            'End If
            'sql = "update bc_core_user_def_comp set [current]=0 where udc_id=" + CStr(udc_id)
            'gbc_db.executesql(sql, certificate)
            'sql = "insert into bc_core_user_def_comp values(" + CStr(udc_id) + ",'" + title + "','" + rtf + "',getdate(),'" + CStr(CInt(ipf)) + "'," + CStr(pub_type_id) + "," + CStr(entity_id) + "," + CStr(author_id) + ",'" + CStr(doc_id) + "',1)"
            'gbc_db.executesql(sql, certificate)
            'insert_user_defined_component = udc_id
        Catch ex As Exception
            insert_user_defined_component = 0
            Dim oerr As New bc_cs_error_log("bc_om_style_components_db", "insert_user_defined_component", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        End Try
    End Function
    Friend Function get_document_list(ByVal user_id As Long, ByVal date_from As String, ByVal date_to As String, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "exec dbo.bc_core_component_document_list " + CStr(user_id) + ",'" + date_from + "','" + date_to + "'"
        get_document_list = gbc_db.executesql(sql, certificate)
    End Function
    Friend Sub clear_display_info(ByVal doc_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "exec dbo.bc_core_clear_display_info " + CStr(doc_id)
        gbc_db.executesql(sql, certificate)
    End Sub
    Friend Sub update_component(ByVal component_id As Long, ByVal ord As Integer, ByVal display_order As Integer, ByVal desc As String, ByVal image_name As String, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        Dim fs As New bc_cs_string_services(desc)
        desc = fs.delimit_apostrophies
        sql = "exec dbo.bc_core_update_component " + CStr(component_id) + "," + CStr(ord) + "," + CStr(display_order) + ",'" + desc + "','" + image_name + "'"

        gbc_db.executesql(sql, certificate)
    End Sub
    Friend Function get_components_for_document(ByVal doc_id As String, ByRef certificate As bc_cs_security.certificate) As Object
        get_components_for_document = Nothing
        Try
            Dim sql As String
            sql = "exec dbo.bc_core_get_components_for_document " + CStr(doc_id)
            get_components_for_document = gbc_db.executesql(sql, certificate)
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_style_components_db", "get_components_for_document", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)

        End Try
    End Function
    Friend Function get_table_on_demand(ByVal doc_id As String, ByRef certificate As bc_cs_security.certificate) As Object
        get_table_on_demand = Nothing
        Try
            Dim sql As String
            sql = "exec dbo.bc_core_get_table_on_demand " + CStr(doc_id)
            get_table_on_demand = gbc_db.executesql(sql, certificate)
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_style_components_db", " get_table_on_demand", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)

        End Try
    End Function
    Friend Function get_paragraphs_for_component(ByVal component_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        get_paragraphs_for_component = Nothing
        Try
            Dim sql As String
            sql = "exec dbo.bc_core_get_paragraphs_for_component " + CStr(component_id)
            get_paragraphs_for_component = gbc_db.executesql(sql, certificate)
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_style_components_db", "get_paragraphs_for_component", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        End Try
    End Function
    Public Function get_docs_that_contain_component(ByVal search_str As String, ByVal doc_ids As String, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String


        search_str = "%" + search_str + "%"
        sql = "select distinct t.doc_id from bc_core_style_component_value t " + _
               "left outer join bc_core_style_component_value_paragraph p on p.component_id=t.component_id and  p.date_to = '9-9-9999' " + _
               " where t.doc_id in (" + doc_ids + ") and t.date_to = '9-9-9999'  and " + _
               " (t.title like '%" + search_str + "'  or coalesce(p.text,'') like '" + search_str + "')"


        get_docs_that_contain_component = gbc_db.executesql(sql, certificate)

    End Function
    Public Sub delete_udc(ByVal udc_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "delete from bc_core_user_def_comp where udc_id=" + CStr(udc_id)
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Function get_componetizer_server_settings_for_template(ByVal template_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "select coalesce(save_to_json_file,0) from bc_core_comp_parse_areas  where template_id = " + CStr(template_id)
        get_componetizer_server_settings_for_template = gbc_db.executesql(sql, certificate)
    End Function
    Public Function get_header_style_for_template(ByVal template_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "select header_style from bc_core_compentize_header_styles_for_template where template_id = " + CStr(template_id)
        get_header_style_for_template = gbc_db.executesql(sql, certificate)
    End Function
    Public Function get_body_style_for_header(ByVal template_id As Long, ByVal header_style As String, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        Dim ostr As New bc_cs_string_services(header_style)
        header_style = ostr.delimit_apostrophies
        sql = "select body_style from bc_core_compentize_body_styles_for_template where template_id=" + CStr(template_id) + " and header_style='" + header_style + "'"
        get_body_style_for_header = gbc_db.executesql(sql, certificate)
    End Function
    Public Function get_parse_areas(ByVal template_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select parse_headers, parse_text_boxes, rtf, html,tables,images,coalesce(header_styles_only,0),coalesce(all_styles,0),coalesce(preview_rtf,0), coalesce(table_as_image,0) , coalesce(table_on_demand,0) from bc_core_comp_parse_areas where template_id=" + CStr(template_id)
        get_parse_areas = gbc_db.executesql(sql, certificate)
    End Function
    Public Function get_settings(ByVal template_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "exec dbo.bc_core_get_html_comp_settings " + CStr(template_id)
        get_settings = gbc_db.executesql(sql, certificate)
    End Function
    Public Function get_pages_to_componetize(ByVal template_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "exec dbo.bc_core_get_html_comp_pages_to_comp " + CStr(template_id)
        get_pages_to_componetize = gbc_db.executesql(sql, certificate)
    End Function
    Public Function get_user_defined_comps(ByVal pub_type_id As Long, ByVal author_id As Long, ByVal date_from As Date, ByVal date_to As Date, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select udc_id, u.title as title,rtf,update_date, coalesce(p.pub_type_name,'unknown'),ltrim(rtrim(coalesce(us.first_name,'unknown'))) + ' ' + ltrim(rtrim(coalesce(us.surname,'user'))),coalesce(e.name,'unknown'), author_id,public_flag,0,0 from  bc_core_user_def_comp u  " + _
                 " left outer join pub_type_table  p on p.pub_type_id = u.pub_type_id " + _
                 " left outer join entity_tbl  e on e.entity_id = u.entity_id  " + _
                 " left outer join user_table  us on u.author_id = us.user_id  " + _
                 " where (u.author_id =" + CStr(author_id) + " or public_flag =1) And update_date >='" + Format(date_from, "dd-MMM-yyyy 00:00") + "' And update_date <='" + Format(date_to, "dd-MMM-yyyy 23:59") + "' and [current] =1 and doc_id <> 0" + _
                 " union " + _
                 " select at_component_type_id, at_component_description as title,'-','9-9-9999' ,'-','-','-',0,0,at_type_addable,coalesce(for_class,0) " + _
                 " from at_component_types a " + _
                 " inner join bc_core_insert_comps_for_pub_types b on a.at_component_type_id=b.comp_id and b.pub_type_Id=" + CStr(pub_type_id) + _
                 " order by title asc "

        get_user_defined_comps = gbc_db.executesql(sql, certificate)
    End Function
End Class
