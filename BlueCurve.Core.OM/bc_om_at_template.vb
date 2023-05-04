Imports BlueCurve.Core.CS
Imports System.io
Imports System.Text
REM ===============================================
REM Bluecurve Limited 2005
REM Module:       Author Tool Template Object Model
REM Type:         Object Model
REM Description:  Properties, Property, 
REM               Templates, template, 
REM               Components, Component, 
REM               Sub Components, Sub Component
REM               Sub Component Parameters, Sub Component Parameter
REM Components, 
REM sub Components
REM Version:      1
REM Change history
REM ===============================================
<Serializable()> Public Class bc_om_email_components
    Inherits bc_cs_soap_base_class
    Public components As New List(Of bc_om_email_component)
    Public Overrides Sub process_object()
        Select Case tmode
            Case bc_cs_soap_base_class.tREAD
                db_read()
        End Select
    End Sub
    Public Sub db_read()
        Try
            Dim vres As Object
            Dim db As New db_email_templates
            Dim et As bc_om_email_component
            components.Clear()
            vres = db.get_all_email_components(certificate)
            If IsArray(vres) Then
                For i = 0 To UBound(vres, 2)
                    et = New bc_om_email_component
                    et.id = vres(0, i)
                    et.desc = vres(1, i)
                    et.sp_name = vres(2, i)
                    et.markup_name = vres(3, i)
                    components.Add(et)
                Next
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_email_components", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        End Try
    End Sub
End Class
<Serializable()> Public Class bc_om_email_component
    Inherits bc_cs_soap_base_class
    Public id As Long
    Public desc As String
    Public sp_name As String
    Public markup_name As String
    Public write_mode As EWRITE_MODE
    Public err_text As String

    Public Enum EWRITE_MODE
        DELETE = 1
        UPDATE = 2
        INSERT = 3
    End Enum

    Public Overrides Sub process_object()
        Select Case tmode
            Case bc_cs_soap_base_class.tWRITE
                db_write()
        End Select
    End Sub
    Public Sub db_write()
        Try
            Dim db As New db_email_templates
            Dim fs As New bc_cs_string_services(desc)
            desc = fs.delimit_apostrophies
            fs = New bc_cs_string_services(sp_name)
            sp_name = fs.delimit_apostrophies

            Select Case write_mode
                Case EWRITE_MODE.DELETE
                    db.delete_email_component(id, certificate)

                Case EWRITE_MODE.INSERT
                    db.add_email_component(desc, sp_name, certificate)


                Case EWRITE_MODE.UPDATE
                    db.update_email_component(id, desc, sp_name, certificate)



            End Select

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_email_components", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        End Try
    End Sub
End Class

<Serializable()> Public Class bc_om_email_templates
    Inherits bc_cs_soap_base_class
    Public email_templates As New List(Of bc_om_email_template)
    Public Overrides Sub process_object()
        Select Case tmode
            Case bc_cs_soap_base_class.tREAD
                db_read()

        End Select

    End Sub

    Public Sub db_read()
        Try
            Dim vres As Object
            Dim db As New db_email_templates
            Dim et As bc_om_email_template
            email_templates.Clear()
            vres = db.get_all_email_templates(certificate)
            If IsArray(vres) Then
                For i = 0 To UBound(vres, 2)
                    et = New bc_om_email_template
                    et.email_template_id = vres(0, i)
                    et.html_filename = vres(1, i)
                    email_templates.Add(et)
                Next
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_email_templates", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        End Try
    End Sub


End Class


<Serializable()> Public Class bc_om_email_template
    Inherits bc_cs_soap_base_class
    Public email_template_id As Long
    Public html_filename As String
    Public clone_html_filename As String
    Public write_mode As EWRITE_MODE
    Public err_text As String
    Public file As Byte()
    Public Enum EWRITE_MODE
        DELETE = 1
        UPDATE = 2
        INSERT = 3
        CLONE = 4
        DOWNLOAD = 5
        UPLOAD = 6
    End Enum
    Public Overrides Sub process_object()
        Select Case tmode
            
            Case bc_cs_soap_base_class.tWRITE
                db_write()
        End Select
    End Sub

    Public Sub db_write()
        Try
            err_text = ""
            Dim db As New db_email_templates
            Dim vres As Object
            Select Case write_mode
                Case EWRITE_MODE.DELETE
                    Dim fs As New bc_cs_file_transfer_services
                    If fs.check_document_exists(bc_cs_central_settings.central_template_path + "email_templates\" + Me.html_filename, certificate) Then
                        fs.delete_file(bc_cs_central_settings.central_template_path + "email_templates\" + Me.html_filename, certificate, False)

                        vres = db.delete_email_template(email_template_id, certificate)
                        If IsArray(vres) Then
                            If UBound(vres, 2) = 0 Then
                                err_text = vres(0, 0)
                            End If
                        End If
                    Else
                        err_text = bc_cs_central_settings.central_template_path + "email_templates\" + Me.html_filename + " not found on server."
                    End If
                Case EWRITE_MODE.UPDATE
                    db.update_email_template(email_template_id, html_filename, certificate)
                Case EWRITE_MODE.INSERT
                    Dim fs As New bc_cs_file_transfer_services
                    If fs.write_bytestream_to_document(bc_cs_central_settings.central_template_path + "email_templates\" + Me.html_filename, Me.file, certificate, False) = False Then
                        err_text = "file: " + bc_cs_central_settings.central_template_path + "email_templates\" + Me.html_filename + " could not be written on server"
                        Exit Sub
                    End If
                    db.insert_email_template(html_filename, certificate)
                Case EWRITE_MODE.CLONE
                    Dim fs As New bc_cs_file_transfer_services
                    If fs.check_document_exists(bc_cs_central_settings.central_template_path + "email_templates\" + Me.html_filename, certificate) = False Then
                        err_text = "file: " + bc_cs_central_settings.central_template_path + "email_templates\" + Me.html_filename + " could not be found on server"
                        Exit Sub
                    End If
                    fs.file_copy(bc_cs_central_settings.central_template_path + "email_templates\" + Me.html_filename, bc_cs_central_settings.central_template_path + "email_templates\" + Me.clone_html_filename, True)
                    db.insert_email_template(clone_html_filename, certificate)
                Case EWRITE_MODE.UPLOAD
                    Dim fs As New bc_cs_file_transfer_services
                    If fs.write_bytestream_to_document(bc_cs_central_settings.central_template_path + "email_templates\" + Me.html_filename, Me.file, certificate, False) = False Then
                        err_text = "file: " + bc_cs_central_settings.central_template_path + "email_templates\" + Me.html_filename + " could not be written on server"
                    End If
                Case EWRITE_MODE.DOWNLOAD
                    Dim fs As New bc_cs_file_transfer_services
                    If fs.write_document_to_bytestream(bc_cs_central_settings.central_template_path + "email_templates\" + Me.html_filename, Me.file, certificate, False) = False Then
                        err_text = "file: " + bc_cs_central_settings.central_template_path + "email_templates\" + Me.html_filename + " not found on server"
                    End If

                Case Else
                    err_text = "Invalid Write Mode"
            End Select

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_email_template", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        End Try
    End Sub
End Class

<Serializable()> Public Class bc_om_email_template_for_pub_type
    Inherits bc_cs_soap_base_class
    Public pub_type_id As Long
    Public email_template_id As Long
    Public write_mode As EWRITE_MODE
    Public err_text As String
    Public Enum EWRITE_MODE
        DELETE = 1
        UPDATE = 2
    End Enum
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
            Dim db As New db_email_templates
            Dim vres As Object
            vres = db.get_email_template_for_pub_type(pub_type_id,certificate)
            If IsArray(vres) Then
                If UBound(vres, 2) = 0 Then
                    email_template_id = vres(0, 0)
                End If

            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_email_template_for_pub_type", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        End Try
    End Sub
    Public Sub db_write()
        Try
            Dim db As New db_email_templates


            Select Case write_mode
                Case EWRITE_MODE.DELETE
                    db.delete_email_template_for_pub_type(pub_type_id, certificate)
                   
                Case EWRITE_MODE.UPDATE
                    db.update_email_template_for_pub_type(pub_type_id, email_template_id, certificate)
               
                Case Else
                    err_text = "Invalid Write Mode"
            End Select

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_email_template_for_oub_type", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        End Try
    End Sub
End Class

Class db_email_templates
    Dim gdb As New bc_cs_db_services
    Public Sub delete_email_component(id As Long, ByRef certificate As bc_cs_security.certificate)
        gdb.executesql("exec dbo.bc_core_delete_email_component " + CStr(id), certificate)
    End Sub
    Public Sub update_email_component(id As Long, desc As String, sp_name As String, ByRef certificate As bc_cs_security.certificate)
        gdb.executesql("exec dbo.bc_core_update_email_component " + CStr(id) + ",'" + desc + "','" + sp_name + "'", certificate)
    End Sub
    Public Sub add_email_component(desc As String, sp_name As String, ByRef certificate As bc_cs_security.certificate)
        gdb.executesql("exec dbo.bc_core_add_email_component '" + desc + "','" + sp_name + "'", certificate)
    End Sub

    Public Function get_all_email_components(ByRef certificate As bc_cs_security.certificate)
        get_all_email_components = gdb.executesql("exec dbo.bc_core_get_email_components", certificate)
    End Function
    Public Function get_email_template_filename(template_id As Long, ByRef certificate As bc_cs_security.certificate)
        get_email_template_filename = gdb.executesql("exec dbo.bc_core_bp_get_email_template_for_id " + CStr(template_id), certificate)
    End Function
    Public Function get_email_template_for_pub_type(pub_type_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        get_email_template_for_pub_type = gdb.executesql("exec dbo.bc_core_bp_get_email_template_for_pt " + CStr(pub_type_id), certificate)
    End Function
    Public Sub delete_email_template_for_pub_type(pub_type_id As Long, ByRef certificate As bc_cs_security.certificate)
        gdb.executesql("exec dbo.bc_core_bp_delete_email_template_for_pt " + CStr(pub_type_id), certificate)
    End Sub
    Public Sub update_email_template_for_pub_type(pub_type_id As Long, template_id As Long, ByRef certificate As bc_cs_security.certificate)
        gdb.executesql("exec dbo.bc_core_bp_update_email_template_for_pt " + CStr(pub_type_id) + "," + CStr(template_id), certificate)
    End Sub
    Public Function get_all_email_templates(ByRef certificate As bc_cs_security.certificate) As Object
        get_all_email_templates = gdb.executesql("exec dbo.bc_core_bp_get_all_email_templates", certificate)
    End Function
    Public Function delete_email_template(id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        delete_email_template = gdb.executesql("exec dbo.bc_core_bp_delete_email_template " + CStr(id), certificate)
    End Function
    Public Sub update_email_template(id As Long, html_filename As String, ByRef certificate As bc_cs_security.certificate)
        Dim fs As New bc_cs_string_services(html_filename)
        html_filename = fs.delimit_apostrophies()
        gdb.executesql("exec dbo.bc_core_bp_update_email_template " + CStr(id) + ",'" + html_filename + "'", certificate)
    End Sub
    Public Sub insert_email_template(html_filename As String, ByRef certificate As bc_cs_security.certificate)
        Dim fs As New bc_cs_string_services(html_filename)
        html_filename = fs.delimit_apostrophies()
        gdb.executesql("exec dbo.bc_core_bp_insert_email_template '" + html_filename + "'", certificate)
    End Sub
End Class
<Serializable()> Public Class bc_om_doc_properties
    Inherits bc_cs_soap_base_class
    Public doc_property As New ArrayList
    Public Sub New()

    End Sub
    Public Sub add(ByVal strname As String, ByVal itype As Integer)
        Dim doc_pro As New bc_om_doc_property(strname, itype)
        doc_pro.certificate = MyBase.certificate
        doc_property.Add(doc_pro)
    End Sub
    Public Overrides Sub process_object()
        Dim otrace As New bc_cs_activity_log("bc_om_doc_properties", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            REM this is always specific to object
            If Me.tmode = bc_cs_soap_base_class.tREAD Then
                db_read()
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_doc_properties", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_doc_properties", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub
    Public Sub db_read()
        Dim otrace As New bc_cs_activity_log("bc_doc_properties", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Dim i As Integer
        Dim oproperty As bc_om_doc_property
        Dim db_template As New bc_at_template_db
        Dim vproperties As Object
        vproperties = db_template.read_all_properties(MyBase.certificate)
        If IsArray(vproperties) Then
            For i = 0 To UBound(vproperties, 2)
                oproperty = New bc_om_doc_property(vproperties(0, i), vproperties(1, i))
                oproperty.certificate = MyBase.certificate
                doc_property.Add(oproperty)
            Next
        End If

        otrace = New bc_cs_activity_log("bc_doc_properties", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
    End Sub
    'Public Overloads Overrides Function call_web_service() As String
    '    Dim otrace As New bc_cs_activity_log("bc_om_doc_properties", "call_web_service", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
    '    If IsNumeric(bc_cs_central_settings.timeout) Then
    '        webservice.Timeout = CLng(bc_cs_central_settings.timeout)
    '        Dim ocommentary As New bc_cs_activity_log("bc_om_web services", "call_web_service", bc_cs_activity_codes.COMMENTARY, "Setting Web Service Timeout to: " + bc_cs_central_settings.timeout)
    '    End If

    '   call_web_service = webservice.LoadProperties()
    '   otrace = New bc_cs_activity_log("bc_om_doc_properties", "call_web_service", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
    'End Function
End Class
<Serializable()> Public Class bc_om_doc_property
    Inherits bc_cs_soap_base_class

    Public name As String
    Public type As Integer
    Public Sub New()

    End Sub
    Public Sub New(ByVal strname As String, ByVal itype As Integer)
        name = strname
        type = itype
    End Sub
End Class

<Serializable()> Public Class bc_om_templates
    Inherits bc_cs_soap_base_class
    Public template As New ArrayList
    Public component_types As New bc_om_component_types
    REM FIL JUN 2012
    Public without_files As Boolean = False
    Public Sub New()

    End Sub
    Public Function add(ByVal strlogical_name As String, ByVal strfilename As String, Optional ByVal templateID As Integer = -1) As Integer
        Dim ctemplate As New bc_om_template(templateID, strlogical_name, strfilename)
        add = template.Add(ctemplate)
    End Function
    Public Sub Remove(ByVal index As Integer)
        template.RemoveAt(index)
    End Sub
    Public Overrides Sub process_object()
        Dim otrace As New bc_cs_activity_log("bc_om_templates", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            REM this is always specific to object
            If Me.tmode = bc_cs_soap_base_class.tREAD Then
                db_read()
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_templates", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_templates", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub
    REM reads entire template suite into memory overwrites what is 
    REM already there
    Public Sub db_read()
        Dim otrace As New bc_cs_activity_log("bc_om_templates", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try
            Dim i As Integer
            Dim vtemplates As Object
            Dim otemplate As bc_om_template
            Dim db_template As New bc_at_template_db
            Dim fs As bc_cs_file_transfer_services
            template.Clear()

            vtemplates = db_template.read_all_templates(MyBase.certificate)
            If IsArray(vtemplates) Then
                For i = 0 To UBound(vtemplates, 2)
                    otemplate = New bc_om_template
                    otemplate.certificate = certificate
                    REM copy file into byte
                    fs = New bc_cs_file_transfer_services
                    If Right(CStr(vtemplates(2, i)), 4) <> "html" Then
                        If without_files = False And vtemplates(3, i) < 5 Then
                            fs.write_document_to_bytestream(bc_cs_central_settings.central_template_path + CStr(vtemplates(2, i)), otemplate.bytedoc, MyBase.certificate)
                        End If
                    End If
                    REM SB 5.5
                    otemplate.location_type = vtemplates(3, i)
                    otemplate.path = vtemplates(4, i)
                    template.Add(otemplate)
                    template(i).db_read(vtemplates(0, i), MyBase.certificate)
                Next
            End If
            component_types.certificate = certificate
            component_types.db_read()
            db_template = Nothing
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_templates", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_templates", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub

    'Public Overloads Overrides Function call_web_service() As String
    '    Dim otrace As New bc_cs_activity_log("bc_om_templates", "call_web_service", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
    '    If IsNumeric(bc_cs_central_settings.timeout) Then
    '        webservice.Timeout = CLng(bc_cs_central_settings.timeout)
    '        Dim ocommentary As New bc_cs_activity_log("bc_om_web services", "call_web_service", bc_cs_activity_codes.COMMENTARY, "Setting Web Service Timeout to: " + bc_cs_central_settings.timeout)
    '    End If

    '   call_web_service = webservice.LoadTemplates()
    '   otrace = New bc_cs_activity_log("bc_om_templates", "call_web_service", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
    'End Function
End Class

<Serializable()> Public Class bc_om_template
    Inherits bc_cs_soap_base_class
    REM DB FIL 5.5
    Enum destination_location_type
        TEMPLATES = 1
        WORD_STARUP = 2
        EXCEL_STARUP = 3
        PATH = 4
    End Enum

    Public name As String
    Public filename As String
    Public id As Integer
    Public components As New bc_om_components
    Public bytedoc As Byte()
    Public delete As Boolean
    Public location_type As destination_location_type = destination_location_type.TEMPLATES
    Public path As String
    Public updatePhysicalFileOnly As Boolean = False ' soap only
    Public Sub New(ByVal templateId As Integer, ByVal strlogical_name As String, ByVal strfilename As String)
        id = templateId
        name = strlogical_name
        filename = strfilename
    End Sub
    Public Sub New()

    End Sub
    Public Sub update(ByVal strlogical_name As String, ByVal strfilename As String)
        name = strlogical_name
        filename = strfilename
    End Sub

    Public Sub db_read(ByVal id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim db_template As New bc_at_template_db
        Dim vtemplate As Object
        MyBase.certificate = certificate
        vtemplate = db_template.read_template(id, Me, certificate)

        REM now read components
        components.db_read(id, certificate)
    End Sub

    Public Sub db_write()

        Dim otrace As New bc_cs_activity_log("bc_om_template", "db_write", bc_cs_activity_codes.TRACE_ENTRY, "", MyBase.certificate)

        Try
            Dim db_template As New bc_at_template_db
            MyBase.certificate = certificate
            If Me.delete Then
                db_template.delete_template(Me.id, certificate)
            Else
                If updatePhysicalFileOnly Then
                    Dim fs As New bc_cs_file_transfer_services
                    If File.Exists(String.Concat(bc_cs_central_settings.central_template_path, filename)) Then
                        My.Computer.FileSystem.RenameFile(String.Concat(bc_cs_central_settings.central_template_path, filename), _
                                        String.Concat(Left(filename, InStr(filename, ".") - 1) + "_", Date.Now.ToString("yyyyMMddHHmmss"), Right(filename, Len(filename) - (InStrRev(filename, ".") - 1))))
                    End If
                    fs.write_bytestream_to_document(bc_cs_central_settings.central_template_path + Me.filename, Me.bytedoc, MyBase.certificate)
                Else
                    Me.id = db_template.write_template(Me.name, Me.filename, MyBase.certificate)
                End If
            End If


        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_template", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message, MyBase.certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_template", "db_write", bc_cs_activity_codes.TRACE_EXIT, "", MyBase.certificate)
        End Try

    End Sub

    Public Overrides Sub process_object()
        Dim otrace As New bc_cs_activity_log("bc_om_templates", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            REM this is always specific to object
            If Me.tmode = bc_cs_soap_base_class.tREAD Then
                db_read(Me.id, Me.certificate)
            End If
            If Me.tmode = bc_cs_soap_base_class.tWRITE Then
                db_write()
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_templates", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_templates", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub

    Public Overrides Function ToString() As String
        Return name
    End Function

    Public Sub WriteDocumentToByteStream()

        Dim otrace As New bc_cs_activity_log("bc_om_template", "WriteDocumentToByteStream", bc_cs_activity_codes.TRACE_ENTRY, "", MyBase.certificate)

        Try

            Dim fs = New bc_cs_file_transfer_services
            fs.write_document_to_bytestream(bc_cs_central_settings.local_template_path + filename, Me.bytedoc, MyBase.certificate)

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_template", "WriteDocumentToByteStream", bc_cs_error_codes.USER_DEFINED, ex.Message, MyBase.certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_template", "WriteDocumentToByteStream", bc_cs_activity_codes.TRACE_EXIT, "", MyBase.certificate)
        End Try

    End Sub
End Class

<Serializable()> Public Class bc_om_components
    Inherits bc_cs_soap_base_class

    Public component As New ArrayList
    REM propogate template id 
    Public Sub New()
    End Sub

    Public Sub db_read(ByVal id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim i As Integer
        Dim vcomponents As Object
        Dim ocomponent As bc_om_component
        Dim db_template As New bc_at_template_db
        MyBase.certificate = certificate
        vcomponents = db_template.read_all_components(id, MyBase.certificate)
        If IsArray(vcomponents) Then
            For i = 0 To UBound(vcomponents, 2)
                ocomponent = New bc_om_component(vcomponents(2, i), vcomponents(3, i), vcomponents(4, i), vcomponents(1, i), vcomponents(0, i), certificate)
                component.Add(ocomponent)
                ocomponent.db_read(vcomponents(0, i), certificate)
            Next
        End If

        db_template = Nothing
    End Sub
End Class
<Serializable()> Public Class bc_om_component
    Inherits bc_cs_soap_base_class


    Public autotext_name As String
    Public bookmark_name As String
    Public r1c1_ref As String
    Public type As Integer
    Public sub_components As bc_om_sub_components
    <NonSerialized()> Private comp_id As Long
    Public Sub New()

    End Sub
    Public Sub New(ByVal strbookmarkname As String, ByVal strautotextname As String, ByVal strcellref As String, ByVal itype As Integer, ByVal comp_id As Long, ByRef certificate As bc_cs_security.certificate)
        MyBase.certificate = certificate
        autotext_name = strautotextname
        bookmark_name = strbookmarkname
        r1c1_ref = strcellref
        type = itype
        Me.comp_id = comp_id
        sub_components = New bc_om_sub_components(comp_id)
    End Sub
    Public Sub update(ByVal strautotextname As String, ByVal strbookmarkname As String, ByVal strcellref As String, ByVal itype As String)
        autotext_name = strautotextname
        bookmark_name = strbookmarkname
        r1c1_ref = strcellref
        type = itype
    End Sub
    Public Sub db_write(ByVal id As Integer, ByRef certificate As bc_cs_security.certificate)
        Dim db_template As New bc_at_template_db
        db_template.write_component(id, type, autotext_name, bookmark_name, r1c1_ref, certificate)
    End Sub
    Public Sub db_read(ByVal id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim db_template As New bc_at_template_db
        MyBase.certificate = certificate

        REM now read components
        sub_components.db_read(id, certificate)
    End Sub
End Class
<Serializable()> Public Class bc_om_sub_components
    Inherits bc_cs_soap_base_class

    Public sub_component As New ArrayList
    <NonSerialized()> Private comp_id As Long
    Public Sub New(ByVal comp_id As Long)
        Me.comp_id = comp_id
    End Sub

    Public Sub db_read(ByVal id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim i As Integer
        Dim vsub_components As Object
        Dim osub_component As bc_om_sub_component
        Dim db_template As New bc_at_template_db
        MyBase.certificate = certificate
        vsub_components = db_template.read_all_sub_components(id, certificate)
        If IsArray(vsub_components) Then
            For i = 0 To UBound(vsub_components, 2)
                'osub_component = New bc_om_sub_component(vsub_components(4, i), vsub_components(5, i), vsub_components(3, i), vsub_components(2, i), vsub_components(1, i), comp_id, vsub_components(6, i), vsub_components(7, i), vsub_components(8, i), vsub_components(9, i), vsub_components(16, i), vsub_components(10, i), vsub_components(11, i), vsub_components(12, i), vsub_components(13, i), vsub_components(14, i), vsub_components(15, i), certificate)
                osub_component = New bc_om_sub_component(vsub_components(0, i), vsub_components(4, i), vsub_components(5, i), vsub_components(3, i), vsub_components(2, i), vsub_components(1, i), comp_id, vsub_components(6, i), vsub_components(7, i), vsub_components(8, i), vsub_components(9, i), vsub_components(16, i), vsub_components(10, i), vsub_components(11, i), vsub_components(12, i), vsub_components(13, i), vsub_components(14, i), vsub_components(15, i), certificate)
                osub_component.certificate = MyBase.certificate
                sub_component.Add(osub_component)
            Next
        End If

        db_template = Nothing
    End Sub
End Class
<Serializable()> Public Class bc_om_sub_component
    Inherits bc_cs_soap_base_class

    Public name As String
    Public row As Integer
    Public col As Integer
    Public value As String
    Public style As String
    Public type As Integer
    Public mode As Integer
    Public refresh_type As Integer
    Public subcompid As Integer
    REM 1 single cell
    REM 2 standard display table
    REM 3 standard display chart
    REM 4 Image file
    REM 5 Image database
    REM 6 RTF
    Public mode_param1
    Public mode_param2
    Public mode_param3
    Public contributor_id As Long
    Public parameters As New bc_om_component_template_parameters
    Public web_service_name As String
    Public external_id As Long
    Public format_file As String
    Public cache_level As Integer = 0
    Public new_components As New ArrayList
    <NonSerialized()> Private comp_id As Long
    Public Sub New()

    End Sub
    Public Sub New(ByVal comp_id As Long)

    End Sub
    Public Sub New(ByVal isubcompid As Long, ByVal irow As Long, ByVal icol As Long, ByVal sstyle As String, ByVal svalue As String, ByVal itype As Long, ByVal comp_id As Long, ByVal component_name As String, ByVal mode As Long, ByVal param1 As String, ByVal param2 As String, ByVal param3 As String, ByVal contributor_id As Long, ByVal refresh_type As Long, ByVal web_service_name As String, ByVal format_file As String, ByVal external_id As Long, ByVal cache_level As Integer, ByRef certificate As bc_cs_security.certificate)
        'Public Sub New(ByVal ByVal irow As Long, ByVal icol As Long, ByVal sstyle As String, ByVal svalue As String, ByVal itype As Long, ByVal comp_id As Long, ByVal component_name As String, ByVal mode As Long, ByVal param1 As String, ByVal param2 As String, ByVal param3 As String, ByVal contributor_id As Long, ByVal refresh_type As Long, ByVal web_service_name As String, ByVal format_file As String, ByVal external_id As Long, ByVal cache_level As Integer, ByRef certificate As bc_cs_security.certificate)
        Dim otrace As New bc_cs_activity_log("bc_om_sub_component", "new", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            MyBase.certificate = certificate

            subcompid = isubcompid 'SK added
            row = irow
            col = icol
            style = sstyle
            value = svalue
            type = itype
            Me.contributor_id = contributor_id
            Me.name = component_name
            Me.comp_id = comp_id
            Me.mode = mode
            Me.mode_param1 = param1
            Me.mode_param2 = param2
            Me.mode_param3 = param3
            Me.refresh_type = refresh_type
            Me.web_service_name = web_service_name
            Me.format_file = format_file
            Me.cache_level = cache_level
            Me.external_id = external_id
            parameters = New bc_om_component_template_parameters(comp_id, type, row, col, certificate)

            parameters.db_read(certificate)

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_sub_component", "get_value_for_component", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_om_sub_component", "new", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Sub
    Public Sub update(ByVal irow, ByVal icol, ByVal sstyle, ByVal value, ByVal itype)
        row = irow
        col = icol
        style = sstyle
        value = value
        style = itype
    End Sub

    Public Sub db_read(ByVal id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim db_template As New bc_at_template_db
        db_template.read_sub_component(id, Me, certificate)
    End Sub
End Class
<Serializable()> Public Class bc_om_component_types
    Inherits bc_cs_soap_base_class
    Public component_types As New ArrayList
    Public addable_types As New ArrayList
    Public refresh_types As New ArrayList
    Public external_component_types As New ArrayList
    Public Function add(ByVal componentName As String, ByVal type As Integer, ByVal spName As String, _
                    ByVal typeName As String, ByVal refreshType As Integer, ByVal webServiceName As String, _
                    ByVal externalID As Long, ByVal formatFile As String, ByVal cache_level As Integer, _
                    Optional ByVal componentID As Integer = -1) As Integer
        Dim compType As New bc_om_component_type(componentID, componentName, type, spName, typeName, _
                                                    refreshType, webServiceName, externalID, formatFile, cache_level)
        add = component_types.Add(compType)
    End Function
    Public Sub Remove(ByVal index As Integer)
        component_types.RemoveAt(index)
    End Sub
    Public Sub db_read()
        Dim otrace As New bc_cs_activity_log("bc_om_component_types", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Dim vcomps As Object
            Dim i As Integer

            component_types.Clear()
            Dim db_template As New bc_at_template_db

            vcomps = db_template.read_all_components_types(certificate)
            If IsArray(vcomps) Then
                For i = 0 To UBound(vcomps, 2)
                    Dim ocomp As New bc_om_component_type
                    ocomp.certificate = certificate
                    ocomp.parameters.certificate = certificate
                    ocomp.component_id = vcomps(0, i)
                    ocomp.component_name = vcomps(1, i)
                    ocomp.mode = vcomps(2, i)
                    ocomp.sp_name = vcomps(3, i)
                    ocomp.insert_object = vcomps(4, i)
                    ocomp.contributor = vcomps(5, i)
                    ocomp.refresh_type = vcomps(6, i)
                    ocomp.web_service_name = vcomps(7, i)
                    ocomp.format_file = vcomps(8, i)
                    ocomp.external_id = vcomps(9, i)
                    ocomp.cache_level = vcomps(10, i)
                    ocomp.parameters.component_id = ocomp.component_id
                    ocomp.parameters.db_read()
                    component_types.Add(ocomp)
                Next
            End If

            addable_types.Clear()
            vcomps = db_template.read_all_addable_types(certificate)
            If IsArray(vcomps) Then
                If UBound(vcomps, 2) >= 0 Then
                    If CStr(vcomps(0, 0)) = "Error" Then
                        Dim ocommentary As New bc_cs_activity_log("bc_om_component_types", "db_read", bc_cs_activity_codes.COMMENTARY, "Warning BluePrint datastructures not installed.", certificate)
                    Else
                        For i = 0 To UBound(vcomps, 2)
                            Dim otype As New bc_om_component_addable_type
                            otype.certificate = certificate
                            otype.type_id = vcomps(0, i)
                            otype.type_name = vcomps(1, i)
                            otype.type_description = vcomps(2, i)
                            addable_types.Add(otype)
                        Next
                    End If
                End If
            End If

            refresh_types.Clear()
            vcomps = db_template.read_all_refresh_types(certificate)
            If IsArray(vcomps) Then
                If UBound(vcomps, 2) >= 0 Then
                    If CStr(vcomps(0, 0)) = "Error" Then
                        Dim ocommentary As New bc_cs_activity_log("bc_om_component_types", "db_read", bc_cs_activity_codes.COMMENTARY, "Warning refresh types not installed.", certificate)
                    Else
                        For i = 0 To UBound(vcomps, 2)
                            Dim orefreshtype As New bc_om_component_addable_type.bc_om_component_refresh_type
                            orefreshtype.certificate = certificate
                            orefreshtype.id = vcomps(0, i)
                            orefreshtype.name = vcomps(1, i)
                            orefreshtype.description = vcomps(2, i)
                            refresh_types.Add(orefreshtype)
                        Next
                    End If
                End If
            End If

            external_component_types.Clear()
            vcomps = db_template.read_all_external_component_types(certificate)
            If IsArray(vcomps) Then
                If UBound(vcomps, 2) >= 0 Then
                    If CStr(vcomps(0, 0)) = "Error" Then
                        Dim ocommentary As New bc_cs_activity_log("bc_om_component_types", "db_read", bc_cs_activity_codes.COMMENTARY, "Warning external components not installed.", certificate)
                    Else
                        For i = 0 To UBound(vcomps, 2)
                            Dim oextcomptype As New bc_om_component_addable_type.bc_om_component_external_component_type
                            oextcomptype.certificate = certificate
                            oextcomptype.id = vcomps(0, i)
                            oextcomptype.name = vcomps(1, i)
                            external_component_types.Add(oextcomptype)
                        Next
                    End If
                End If
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_component_types", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_om_component_types", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Sub

    Public Overrides Sub process_object()

        Dim otrace As New bc_cs_activity_log("bc_om_component_type", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            REM this is always specific to object
            If Me.tmode = bc_cs_soap_base_class.tREAD Then
                db_read()
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_component_type", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_component_type", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Sub

    Public Sub New()

    End Sub
End Class
<Serializable()> Public Class bc_om_component_type
    Inherits bc_cs_soap_base_class
    Public component_id As Long
    Public component_name As String
    Public mode As Integer
    Public sp_name As String
    Public insert_object As String
    Public contributor As Long
    Public refresh_type As Integer
    Public parameters As New bc_om_component_parameters
    Public delete As Boolean
    Public web_service_name As String
    Public external_id As Long
    Public format_file As String
    Public cache_level As Integer = 0

    Public Sub New()
        Me.parameters.certificate = MyBase.certificate

    End Sub

    Public Sub New(ByVal componentID As Integer, ByVal componentName As String, ByVal type As Integer, ByVal spName As String, _
                    ByVal typeName As String, ByVal refreshType As Integer, ByVal webServiceName As String, ByVal externalID As Long, _
                    ByVal formatFile As String, ByVal cache_level As Integer)


        Me.parameters.certificate = MyBase.certificate
        Me.component_id = componentID
        Me.component_name = componentName
        Me.mode = type
        Me.sp_name = spName
        Me.insert_object = typeName
        Me.refresh_type = refreshType
        Me.web_service_name = webServiceName
        Me.external_id = externalID
        Me.format_file = formatFile
        Me.cache_level = cache_level

    End Sub

    Public Sub db_write()

        Dim otrace As New bc_cs_activity_log("bc_om_component_type", "db_write", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try
            Dim db_template As New bc_at_template_db

            If Me.component_id = -1 Then
                Me.component_id = db_template.write_component_type(Me.component_name, Me.mode, Me.sp_name, Me.insert_object, _
                                                                    Me.refresh_type, Me.web_service_name, Me.external_id, _
                                                                    Me.format_file, Me.cache_level, MyBase.certificate)
            ElseIf Me.delete Then
                db_template.delete_component_type(Me.component_id, MyBase.certificate)
            Else
                db_template.update_component_type(Me.component_id, Me.component_name, Me.mode, Me.sp_name, _
                                                                    Me.insert_object, Me.refresh_type, Me.web_service_name, Me.external_id, _
                                                                    Me.format_file, Me.cache_level, MyBase.certificate)
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_component_type", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_component_type", "db_write", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Sub

    Public Overrides Sub process_object()

        Dim otrace As New bc_cs_activity_log("bc_om_component_type", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            REM this is always specific to object
            If Me.tmode = bc_cs_soap_base_class.tWRITE Then
                db_write()
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_component_type", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_component_type", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Sub
End Class
REM ===============================================================
REM class for database ineraction for bc_at_template object model
Public Class bc_at_template_db
    Private gbc_db As New bc_cs_db_services
    REM write a tempate record and retuns Id
    Public Sub New()

    End Sub
    Public Function delete_template(ByVal id As Integer, ByRef certificate As bc_cs_security.certificate) As Boolean

        Dim otrace As New bc_cs_activity_log("bc_at_template_db", "read_template", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Dim sql As String
        Dim vid As Object

        sql = String.Concat("exec bcc_core_bp_delete_template ", id.ToString)
        vid = gbc_db.executesql(sql, certificate)

        delete_template = True

        otrace = New bc_cs_activity_log("bc_at_template_db", "read_template", bc_cs_activity_codes.TRACE_EXIT, "", certificate)

    End Function
    Public Function read_all_properties(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String

        sql = "select at_doc_prop_name, at_doc_prop_datatype from at_document_properties order by at_doc_prop_name asc"
        read_all_properties = gbc_db.executesql(sql, certificate)

    End Function
    Public Function read_all_templates(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select template_id, template_name, template_filename, coalesce(template_type,1), coalesce(template_note,'')  from at_template order by template_name asc"
        read_all_templates = gbc_db.executesql(sql, certificate)

    End Function
    Public Function read_template(ByVal id As Long, ByRef obc_template As bc_om_template, ByRef certificate As bc_cs_security.certificate) As Integer
        Dim otrace As New bc_cs_activity_log("bc_at_template_db", "read_template", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Dim sql As String
        Dim vid As Object

        sql = String.Concat("select template_id,template_name,template_filename from at_template where template_id=", id)
        vid = gbc_db.executesql(sql, certificate)

        If IsArray(vid) Then
            obc_template.id = vid(0, 0)
            obc_template.name = vid(1, 0)
            obc_template.filename = vid(2, 0)
            read_template = vid(0, 0)
        End If

        otrace = New bc_cs_activity_log("bc_at_template_db", "read_template", bc_cs_activity_codes.TRACE_EXIT, "", certificate)

    End Function
    Public Function read_all_components(ByVal id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String

        sql = "select at_component_id,at_component_type, at_component_bookmark_name, at_component_autotext_name,at_component_cellref_bookmark from at_template_components where at_component_template_id=" + CStr(id)
        read_all_components = gbc_db.executesql(sql, certificate)

    End Function
    Public Function read_component(ByVal id As Long, ByRef obc_component As bc_om_component, ByRef certificate As bc_cs_security.certificate) As Integer
        Dim otrace As New bc_cs_activity_log("bc_at_template_db", "read_component", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Dim sql As String
        Dim vid As Object

        sql = "select at_component_type, at_component_bookmark_name, at_component_autotext_name,at_component_cellref_bookmark from at_template_components where at_component_id=" + CStr(id)
        vid = gbc_db.executesql(sql, certificate)

        If IsArray(vid) Then
            obc_component.type = vid(0, 0)
            obc_component.bookmark_name = vid(1, 0)
            obc_component.autotext_name = vid(2, 0)
            obc_component.r1c1_ref = vid(3, 0)
        End If
        read_component = id

        otrace = New bc_cs_activity_log("bc_at_template_db", "read_component", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
    End Function
    Public Function read_all_sub_components(ByVal id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String

        sql = "select at_sub_component_id, at_sub_component_type, at_sub_component_value,at_sub_component_style, at_sub_component_xpos,at_sub_component_ypos,at_component_description, at_type_addable,coalesce(at_sp_name,''),coalesce(chart_name,''), coalesce(at_template_sub_components.contributor_id,1), refresh_type, coalesce(web_service_name,''), coalesce(format_file,''), coalesce(external_id,0),coalesce(caching_level,0),isnull(attribute_id,'0') from at_template_sub_components, at_component_types where at_component_type_id=at_sub_component_type and at_component_id=" + CStr(id) + " order by at_sub_component_xpos asc"
        read_all_sub_components = gbc_db.executesql(sql, certificate)

    End Function
    Public Function read_sub_component(ByVal id As Long, ByRef obc_sub_component As bc_om_sub_component, ByRef certificate As bc_cs_security.certificate) As Integer
        Dim otrace As New bc_cs_activity_log("bc_at_template_db", "read_sub_components", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Dim sql As String
        Dim vid As Object

        sql = "select at_sub_component_type, at_sub_component_value,at_sub_component_style, at_sub_component_xpos,at_sub_component_ypos, at_type_addable, coalesce(at_sp_name,''), coalesce(chart_name,'') from at_template_sub_components where at_sub_component_id=" + CStr(id)
        vid = gbc_db.executesql(sql, certificate)

        If IsArray(vid) Then
            obc_sub_component.type = vid(0, 0)
            obc_sub_component.value = vid(1, 0)
            obc_sub_component.style = vid(2, 0)
            obc_sub_component.row = vid(3, 0)
            obc_sub_component.col = vid(4, 0)
        End If
        read_sub_component = id

        otrace = New bc_cs_activity_log("bc_at_template_db", "read_sub_components", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
    End Function
    Public Function read_component_parameters(ByVal comp_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim otrace As New bc_cs_activity_log("bc_at_template_db", "read_component_parameters", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Dim sql As String

        sql = "select comp_id, display_id, ord,value,system_defined,row,col,coalesce(contributor_id,1) from standard_display_template_parameters where comp_id='" + CStr(comp_id) + "'"
        read_component_parameters = gbc_db.executesql(sql, certificate)
        otrace = New bc_cs_activity_log("bc_at_template_db", "read_component_parameters", bc_cs_activity_codes.TRACE_EXIT, "", certificate)

    End Function
    Public Function write_template(ByVal strname As String, ByVal strfilename As String, ByRef certificate As bc_cs_security.certificate) As Integer
        Dim otrace As New bc_cs_activity_log("bc_at_template_db", "write_template", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Dim sql As String
        Dim vid As Object

        write_template = -1

        sql = "select max(template_id) from at_template where template_filename = '" + strfilename.Replace("'", "''") + "'"
        vid = gbc_db.executesql(sql, certificate)
        If IsArray(vid) Then
            If Not IsDBNull(vid(0, 0)) Then
                'template already registered
                write_template = 0
                Exit Function
            End If
        End If

        sql = "insert into at_template(template_name,template_filename) values('" + strname.Replace("'", "''") + "','" + strfilename.Replace("'", "''") + "')"
        gbc_db.executesql(sql, certificate)
        sql = "select max(template_id) from at_template"
        vid = gbc_db.executesql(sql, certificate)

        If IsArray(vid) Then
            write_template = vid(0, 0)
        End If

        otrace = New bc_cs_activity_log("bc_at_template_db", "write_template", bc_cs_activity_codes.TRACE_EXIT, "", certificate)

    End Function
    Public Function write_component(ByVal template_id As Long, ByVal icomponenttype As Integer, ByVal strautotext As String, ByVal strbookmark As String, ByVal strcellref As String, ByRef certificate As bc_cs_security.certificate) As Integer
        Dim otrace As New bc_cs_activity_log("bc_at_template_db", "write_component", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Dim sql As String
        Dim vid As Object
        Dim id As Long

        sql = "select max(at_component_id) from at_template_components"
        vid = gbc_db.executesql(sql, certificate)
        id = 1
        If IsArray(vid) Then
            id = vid(0, 0) + 1
        End If
        sql = "insert into at_template_components(at_component_id, at_component_template_id, at_component_type,at_component_bookmark_name,at_component_autotext_name,at_component_cellref_bookmark) values(" + CStr(id) + "," + CStr(template_id) + "," + CStr(icomponenttype) + ",'" + strbookmark + "','" + strautotext + "','" + strcellref + "')"
        gbc_db.executesql(sql, certificate)
        write_component = id

        otrace = New bc_cs_activity_log("bc_at_template_db", "write_component", bc_cs_activity_codes.TRACE_EXIT, "", certificate)

    End Function

    Public Function write_sub_component(ByVal icomponentid As Long, ByVal icomponenttype As Long, ByVal irow As Integer, ByVal icol As Integer, ByVal strvalue As String, ByVal strstyle As String, ByRef certificate As bc_cs_security.certificate) As Integer
        Dim otrace As New bc_cs_activity_log("bc_at_template_db", "write_sub_component", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Dim sql As String
        Dim vid As Object
        Dim id As Long

        sql = "select max(at_sub_component_id) from at_template_sub_components"
        vid = gbc_db.executesql(sql, certificate)
        id = 1
        If IsArray(vid) Then
            id = vid(0, 0) + 1
        End If
        sql = "insert into at_template_sub_components(at_sub_component_id, at_component_id, at_sub_component_type,at_sub_component_value,at_sub_component_style,at_sub_component_xpos, at_sub_component_ypos) values(" + CStr(id) + "," + CStr(icomponentid) + "," + CStr(icomponenttype) + ",'" + strvalue + "','" + strstyle + "'," + CStr(irow) + "," + CStr(icol) + ")"
        gbc_db.executesql(sql, certificate)
        write_sub_component = id

        otrace = New bc_cs_activity_log("bc_at_template_db", "write_sub_component", bc_cs_activity_codes.TRACE_EXIT, "", certificate)

    End Function
    Public Function read_all_components_types(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select at_component_type_id,at_component_description,at_type_addable,coalesce(at_sp_name,''),coalesce(chart_name,''),contributor_id,refresh_type,coalesce(web_service_name,''), coalesce(format_file,''), coalesce(external_id,0),coalesce(caching_level,0) from at_component_types order by at_component_description asc"
        read_all_components_types = gbc_db.executesql(sql, certificate)
    End Function

    Public Function read_all_addable_types(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "exec bcc_core_bp_get_addable_component_types"
        read_all_addable_types = gbc_db.executesql_show_no_error(sql)
    End Function

    Public Function read_all_refresh_types(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "exec bcc_core_bp_get_component_refresh_types"
        read_all_refresh_types = gbc_db.executesql_show_no_error(sql)
    End Function

    Public Function read_all_external_component_types(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "exec bcc_core_bp_get_external_components"
        read_all_external_component_types = gbc_db.executesql_show_no_error(sql)
    End Function

    Public Function write_component_type(ByVal description As String, ByVal type As Integer, _
                                             ByVal spName As String, ByVal typeName As String, _
                                             ByVal refreshType As Integer, ByVal webserviceName As String, _
                                             ByVal externalID As Long, ByVal formatFile As String, ByVal cache_level As Integer, _
                                             ByRef certificate As bc_cs_security.certificate) As Integer

        Dim otrace As New bc_cs_activity_log("bc_at_template_db", "write_component_type", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try

            write_component_type = -1

            Dim sql As New StringBuilder
            With sql
                .Append("exec bcc_core_bp_insert_component_type ")
                .Append("'")
                .Append(description.Replace("'", "''"))
                .Append("',")
                .Append(type)
                .Append(",")
                .Append("'")
                .Append(spName.Replace("'", "''"))
                .Append("','")
                .Append(typeName.Replace("'", "''"))
                .Append("',")
                .Append(refreshType)
                .Append(",'")
                .Append(webserviceName.Replace("'", "''"))
                .Append("',")
                .Append(externalID)
                .Append(",'")
                .Append(formatFile.Replace("'", "''"))
                .Append("',")
                .Append(cache_level)
            End With

            Dim id As Object

            id = gbc_db.executesql(sql.ToString, certificate)

            If IsArray(id) Then
                write_component_type = id(0, 0)
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_at_template_db", "write_component_type", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_at_template_db", "write_component_type", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Function

    Public Sub delete_component_type(ByVal component_id As Integer, ByRef certificate As bc_cs_security.certificate)

        Dim otrace As New bc_cs_activity_log("bc_at_template_db", "delete_component_type", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Dim sql As New StringBuilder
            With sql
                .Append("exec bcc_core_bp_delete_component_type ")
                .Append(component_id)
            End With

            gbc_db.executesql(sql.ToString, certificate)

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_at_template_db", "delete_component_type", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_at_template_db", "delete_component_type", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Sub

    Public Sub update_component_type(ByVal component_id As Integer, ByVal description As String, ByVal type As Integer, _
                                            ByVal spName As String, ByVal typeName As String, _
                                            ByVal refreshType As Integer, ByVal webServiceName As String, _
                                            ByVal externalID As Long, ByVal formatFile As String, ByVal cache_level As Integer, _
                                            ByRef certificate As bc_cs_security.certificate)


        Dim otrace As New bc_cs_activity_log("bc_at_template_db", "update_component_type", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Dim sql As New StringBuilder
            With sql
                .Append("exec bcc_core_bp_update_component_type ")
                .Append(component_id)
                .Append(",'")
                .Append(description.Replace("'", "''"))
                .Append("',")
                .Append("'")
                .Append(spName.Replace("'", "''"))
                .Append("','")
                .Append(typeName.Replace("'", "''"))
                .Append("',")
                .Append(refreshType)
                .Append(",'")
                .Append(webServiceName.Replace("'", "''"))
                .Append("',")
                .Append(externalID)
                .Append(",'")
                .Append(formatFile.Replace("'", "''"))
                .Append("',")
                .Append(cache_level)
            End With

            gbc_db.executesql(sql.ToString, certificate)

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_at_template_db", "update_component_type", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_at_template_db", "update_component_type", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Sub

End Class

<Serializable()> Public Class bc_om_component_addable_type
    Inherits bc_cs_soap_base_class
    Public type_id As Long
    Public type_name As String
    Public type_description As String

    Public Sub New()

    End Sub

    Public Overrides Function ToString() As String
        Return Me.type_name
    End Function

    <Serializable()> Public Class bc_om_component_refresh_type
        Inherits bc_cs_soap_base_class
        Public id As Integer
        Public name As String
        Public description As String

        Public Sub New()

        End Sub

        Public Overrides Function ToString() As String
            Return Me.name
        End Function

    End Class

    <Serializable()> Public Class bc_om_component_external_component_type
        Inherits bc_cs_soap_base_class
        Public id As Long
        Public name As String

        Public Sub New()

        End Sub

        Public Overrides Function ToString() As String
            Return Me.name
        End Function

    End Class

End Class
