Imports System.io
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary
Imports system.security.Principal
Imports System.Security.Permissions
Imports System.Runtime.Serialization.Formatter
Imports System.Threading
Imports System.Net
Imports System.Xml.Serialization


Imports System.Collections.Generic
Imports System.Text
Imports System.Reflection
Imports System.CodeDom
Imports System.CodeDom.Compiler
Imports System.Web.Services.Description





REM ==========================================
REM Bluecurve Limited 2005
REM Module:       Xml Services
REM Description:  XML/SOAP Common Routines
REM Type:         CS
REM               Serialize object to file
REM               Deserialize a file to an object                  
REM Version:      1
REM Change history
REM ==========================================
Public Class bc_cs_data_services
    Public xsd As String
    Public o As Object

    REM serializes an object to a file
    Public Function soap_serialize_object_to_file(ByVal strfilename As String, ByRef certificate As bc_cs_security.certificate) As Integer

        Dim otrace As New bc_cs_activity_log("bc_cs_data_services", "soap_serialize_object_to_file", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try
            Dim oerr As Boolean = True
            Dim fs As FileStream = Nothing

            While oerr = True
                Try
                    Thread.Sleep(200)
                    'Open a filestream for output
                    fs = New FileStream(strfilename, FileMode.Create)
                    oerr = False
                Catch

                End Try
            End While
            'create a Binary formatter for this stream
            Dim sf As New BinaryFormatter()
            'Serialize the object to this stream
            sf.Serialize(fs, o)

            fs.Close()

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_cs_data_services", "soap_serialize_file_to_object", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_cs_data_services", "soap_serialize_file_yo_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Function

    Public Function soap_check_deserialize_file_to_object(ByVal strfilename As String, ByRef certificate As bc_cs_security.certificate, Optional ByVal show_error As Boolean = True) As Boolean
        Dim otrace As New bc_cs_activity_log("bc_cs_data_services", "soap_check_deserialize_file_to_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Dim fs As FileStream = Nothing
        Try
            Dim ocommentary As New bc_cs_activity_log("bc_cs_data_services", "soap_check_deserialize_file_to_object", bc_cs_activity_codes.COMMENTARY, "Attempting to Deserialize from file: " + strfilename, certificate)
            soap_check_deserialize_file_to_object = True
            'Open a filestream for output
            Dim oerr As Boolean = True
            Dim ofs As New bc_cs_file_transfer_services
            If ofs.check_document_exists(strfilename, certificate) = False Then
                soap_check_deserialize_file_to_object = False
                Exit Function
            End If
            While oerr = True
                Try
                    Thread.Sleep(200)
                    fs = New FileStream(strfilename, FileMode.Open)
                    oerr = False
                Catch ex As Exception
                    Dim ocomm As New bc_cs_activity_log("bc_cs_data_services", "soap_check_deserialize_file_to_object", bc_cs_activity_codes.COMMENTARY, ex.Message, certificate)
                End Try
            End While
            'create a Binary formatter for this stream
            Dim sf As New BinaryFormatter()
            'deSerialize the stream to the objectd
            Dim o As Object
            Try
                o = sf.Deserialize(fs)
                fs.Close()
            Catch ex As Exception
                Dim ocomm As New bc_cs_activity_log("bc_cs_data_services", "soap_check_deserialize_file_to_object", bc_cs_activity_codes.COMMENTARY, ex.Message, certificate)
                soap_check_deserialize_file_to_object = False
                fs.Close()
            End Try

        Catch ex As Exception
            soap_check_deserialize_file_to_object = False
            Dim ocomm As New bc_cs_activity_log("bc_cs_data_services", "soap_check_deserialize_file_to_object", bc_cs_activity_codes.COMMENTARY, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_cs_data_services", "soap_check_deserialize_file_to_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Function
    Public Function soap_deserialize_file_to_object(ByVal strfilename As String, ByRef certificate As bc_cs_security.certificate, Optional ByVal show_error As Boolean = True) As Object
        Dim otrace As New bc_cs_activity_log("bc_cs_data_services", "soap_deserialize_file_to_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Dim fs As FileStream = Nothing
        Try
            Dim ocommentary As New bc_cs_activity_log("bc_cs_data_services", "soap_deserialize_file_to_object", bc_cs_activity_codes.COMMENTARY, "Attempting to Deserialize from file: " + strfilename, certificate)
            'Open a filestream for output
            Dim oerr As Boolean = True
            Dim ofs As New bc_cs_file_transfer_services
            If ofs.check_document_exists(strfilename, certificate) = False Then
                Return Nothing
            End If
            While oerr = True
                Try
                    Thread.Sleep(200)

                    fs = New FileStream(strfilename, FileMode.Open)
                    oerr = False
                Catch

                End Try
            End While
            'create a Binary formatter for this stream
            Dim sf As New BinaryFormatter()
            'deSerialize the stream to the object
            soap_deserialize_file_to_object = sf.Deserialize(fs)
            'close the stream
        Catch ex As Exception
            If show_error = True Then
                Dim db_err As New bc_cs_error_log("bc_cs_data_services", "soap_deserialize_file_to_object", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            Else
                Dim ocommentary As New bc_cs_activity_log("bc_cs_data_services", "soap_deserialize_file_to_object", bc_cs_activity_codes.COMMENTARY, "error: " + ex.Message, certificate)
            End If
            soap_deserialize_file_to_object = Nothing
        Finally
            fs.Close()
            otrace = New bc_cs_activity_log("bc_cs_data_services", "soap_deserialize_file_to_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Function
    Public Function soap_serialize_object_to_string(ByRef certificate As bc_cs_security.certificate, Optional ByVal packet_code As String = "") As String

        Dim otrace As New bc_cs_activity_log("bc_cs_data_services", "soap_serialize_object_to_string", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Dim bc_cs_central_settings As New bc_cs_central_settings
        Dim by() As Byte

        Try
            Using ms As New MemoryStream

                Dim sf As New BinaryFormatter()

                sf.Serialize(ms, o)

                by = ms.ToArray()

                ms.Close()

                soap_serialize_object_to_string = Convert.ToBase64String(by)

            End Using

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_cs_data_services", "soap_serialize_object_to_string", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            soap_serialize_object_to_string = ""
        Finally
            otrace = New bc_cs_activity_log("bc_cs_data_services", "soap_serialize_object_to_string", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Function
   
    Public Function soap_deserialize_string_to_object(ByVal strdata As String, ByRef certificate As bc_cs_security.certificate, Optional ByVal packet_code As String = "") As Object

        Dim otrace As New bc_cs_activity_log("bc_cs_xml_Services", "soap_deserialize_string_to_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

    

        Dim bc_cs_central_settings As New bc_cs_central_settings
        Dim by() As Byte

        Try

            by = Convert.FromBase64String(strdata)
         
            strdata = Nothing
            Using ms = New MemoryStream(by)
                by = Nothing
                'create a Binary formatter for thi          s stream
                Dim sf As New BinaryFormatter()
                'deSerialize the stream to the object
             
                soap_deserialize_string_to_object = sf.Deserialize(ms)
                'close the stream
                ms.Close()
                ms.Dispose()

            End Using
          Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_cs_data_services", "soap_deserialize_string_to_object", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            soap_deserialize_string_to_object = Nothing
        Finally
            otrace = New bc_cs_activity_log("bc_cs_data_services", "soap_deserialize_string_to_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Function

    Public Function soap_serialize_object_to_xml(ByRef certificate As bc_cs_security.certificate, Optional ByVal packet_code As String = "") As String

        Dim otrace As New bc_cs_activity_log("bc_cs_data_services", "soap_serialize_object_to_xml", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Dim bc_cs_central_settings As New bc_cs_central_settings
        Dim by() As Byte

        Try
            Using ms As New MemoryStream

                Dim sf As New XmlSerializer(o.GetType)

                sf.Serialize(ms, o)

                by = ms.ToArray()

                ms.Close()

                'xml serialization does not always encode apostrophe's.  This causes an issue when passing to SQL Server
                soap_serialize_object_to_xml = Replace(System.Text.Encoding.UTF8.GetString(by), "'", "&#39;")

            End Using

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_cs_data_services", "soap_serialize_object_to_xml", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            soap_serialize_object_to_xml = ""
        Finally
            otrace = New bc_cs_activity_log("bc_cs_data_services", "soap_serialize_object_to_xml", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Function

 
End Class

Public Class bc_cs_third_party_web_service
    'Public url As String
    'Public service_name As String
    'Public namespace_name As String
    'Public assembly_name As String
    Public err As Boolean
    Public error_text As String
    Public result_xml As String
    Public result_byte_array As Byte()
    Shared ass As System.Reflection.Assembly
    Private Shared t As Type


    REM this gets called only once per instance
    'Private Function load_assembly(ByRef certificate As bc_cs_security.certificate) As Boolean
    '    Try
    '        Dim ocomm As bc_cs_activity_log
    '        load_assembly = False
    '        If IsNothing(ass) Then
    '            ocomm = New bc_cs_activity_log("bc_cs_third_party_web_service", "load_assembly", bc_cs_activity_codes.COMMENTARY, "Attempting to load assembly: " + bc_cs_central_settings.external_web_proxy_fullname, certificate)
    '            ass = Reflection.Assembly.Load(bc_cs_central_settings.external_web_proxy_fullname)
    '            load_assembly = True
    '            ocomm = New bc_cs_activity_log("bc_cs_third_party_web_service", "load_assembly", bc_cs_activity_codes.COMMENTARY, "Load Successful for: " + bc_cs_central_settings.external_web_proxy_fullname, certificate)
    '        Else
    '            load_assembly = True
    '            ocomm = New bc_cs_activity_log("bc_cs_third_party_web_service", "load_assembly", bc_cs_activity_codes.COMMENTARY, "Assembly already loaded: " + bc_cs_central_settings.external_web_proxy_fullname, certificate)
    '        End If

    '    Catch ex As Exception
    '        error_text = "failed to load assembly: " + bc_cs_central_settings.external_web_proxy_fullname + ": " + ex.Message
    '    End Try
    'End Function
   

    Public Function call_web_service_dynamic(ByVal webmethod As String, ByVal service_name As String, ByVal external_id As Long, ByVal entity_id As Long, ByVal pub_type_id As Long, ByVal acc_id As Long, ByVal stage_id As Long, ByVal language_id As Long, ByVal data_at_date As Date, ByVal doc_id As String, ByVal row As Long, ByVal col As Long, ByVal contributor_id As Long, ByVal parameters As Object, ByVal rollback_mode As Boolean, ByRef certificate As bc_cs_security.certificate, Optional ByVal params As String = "", Optional ByRef db As bc_cs_db_services = Nothing) As Object
        Dim otrace As New bc_cs_activity_log("bc_cs_third_party_web_service", "call_web_service_dynamic", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Dim ocomm As bc_cs_activity_log
        'Dim oTObject As Object
        Dim fullname As String = ""
        Dim url As String = ""
        Dim post As Boolean = False
        call_web_service_dynamic = Nothing

        'Try
        '    If load_assembly(certificate) = False Then
        '        err = True
        '        call_web_service_dynamic = Nothing
        '        Exit Function
        '    End If
        '    err = False
        '    fullname = bc_cs_central_settings.external_web_proxy + ".bc_cs_dynamic_web_proxy, " + bc_cs_central_settings.external_web_proxy_fullname

        '    If IsNothing(t) Then
        '        ocomm = New bc_cs_activity_log("bc_cs_third_party_web_service", "load_assembly", bc_cs_activity_codes.COMMENTARY, "Attempting to invoke: " + bc_cs_central_settings.external_web_proxy + ".bc_cs_dynamic_web_proxy", certificate)
        '        Try
        '            t = ass.GetType(bc_cs_central_settings.external_web_proxy + ".bc_cs_dynamic_web_proxy")
        '        Catch ex As Exception
        '            err = True
        '            error_text = "assembly not loaded " + fullname + ": " + ex.Message
        '            call_web_service_dynamic = Nothing
        '            Exit Function
        '        End Try
        '    Else
        '        ocomm = New bc_cs_activity_log("bc_cs_third_party_web_service", "call_web_service_dynamic", bc_cs_activity_codes.COMMENTARY, bc_cs_central_settings.external_web_proxy + ".bc_cs_dynamic_web_proxy already invoked", certificate)
        '    End If

        '    Try
        '        oTObject = Activator.CreateInstance(t)
        '    Catch ex As Exception
        '        error_text = "couldnt instantiate " + bc_cs_central_settings.external_web_proxy + ".bc_cs_dynamic_web_proxy: " + ex.Message
        '        err = True
        '        call_web_service_dynamic = Nothing
        '        Exit Function
        '    End Try
        '    REM later on we will get these from method table
        'Dim gdb As New bc_cs_web_method_parameters_db
        'Dim res As Object
        'res = gdb.get_service_name_and_url(webmethod, certificate)
        'If IsArray(res) Then
        '    Try
        '        service_name = res(0, 0)
        '        If res(0, 1) = "Default url set in config file" Then
        '            url = bc_cs_central_settings.external_web_serivce_url
        '        Else
        '            url = res(1, 0)
        '        End If
        '    Catch ex As Exception
        '        error_text = "error getting service name and url for webmethod: " + webmethod + ": " + ex.Message
        '        err = True
        '        Exit Function
        '    End Try
        'Else
        '        error_text = "no service name or url set up for webmethod: " + webmethod
        '        err = True
        '        Exit Function
        'End If
        Try
            Dim oTObject As bc_cs_dynamic_web_proxy = Nothing

            If Len(service_name) <> 6 Then
                If UCase(Left(service_name, 6)) = "[POST]" Then
                    service_name = Right(service_name, Len(service_name) - 6)
                    post = True
                    ocomm = New bc_cs_activity_log("bc_cs_third_party_web_service", "load_assembly", bc_cs_activity_codes.COMMENTARY, "http post protocol selected for service:" + service_name, certificate)
                    url = bc_cs_central_settings.external_web_serivce_url + service_name
                    'If Len(url) > 4 Then
                    'If Right(url, 4) <> "asmx" Then
                    'url = url + ".asmx"
                    'End If
                    'End If
                End If
            End If
            If post = False Then
                url = bc_cs_central_settings.external_web_serivce_url + service_name
                If Len(url) > 4 Then
                    If Right(url, 4) <> "asmx" Then
                        url = url + ".asmx"
                    End If
                End If
            End If
            If post = False Then
                oTObject = New bc_cs_dynamic_web_proxy
                url = bc_cs_central_settings.external_web_serivce_url + service_name
                If Len(url) > 4 Then
                    If Right(url, 4) <> "asmx" Then
                        url = url + ".asmx"
                    End If
                End If

                ocomm = New bc_cs_activity_log("bc_cs_third_party_web_service 5.6.0", "call_web_service_dynamic 5.6.0", bc_cs_activity_codes.COMMENTARY, "URL: " + url + " , Service Name: " + service_name, certificate)


                oTObject._url = url
                oTObject._servicename = service_name
                REM now compile proxy if not already loaded
                Try
                    If oTObject.load_proxy(False, Nothing, certificate) = False Then
                        error_text = "failed to compile proxy: " + oTObject._err_text + " for service name: " + service_name
                        err = True
                        call_web_service_dynamic = Nothing
                        Exit Function
                    End If
                Catch ex As Exception
                    error_text = "couldnt call load_proxy in bc_cs_dynamic_proxy_class: " + ex.Message
                    err = True
                    call_web_service_dynamic = Nothing
                    Exit Function
                End Try
            End If
            REM now get parameters for web service
            Dim web_service_params As New bc_cs_web_method_parameters
            web_service_params.web_method_name = webmethod
            web_service_params.db_read(certificate)
            ocomm = New bc_cs_activity_log("bc_cs_third_party_webservices", "call_wb_service_dynamic", bc_cs_activity_codes.COMMENTARY, "attempting to call webmethod:" + webmethod + " with " + CStr(web_service_params.web_method_params.Count) + " parameters.", certificate)
            Dim args(web_service_params.web_method_params.Count - 1) As Object
            Dim isbytes As Boolean = False
            If web_service_params.web_method_params.Count > 0 Then
                error_text = set_arguments(external_id, entity_id, pub_type_id, contributor_id, stage_id, acc_id, language_id, data_at_date, doc_id, parameters, web_service_params, args, rollback_mode, isbytes, certificate, params, db)
                If error_text <> "" Then
                    err = True
                    Exit Function
                End If
            End If
            Dim i As Integer
            For i = 0 To bc_cs_central_settings.retry - 1
                If post = True Then
                    ocomm = New bc_cs_activity_log("bc_cs_third_party_webservices", "call_wb_service_dynamic", bc_cs_activity_codes.COMMENTARY, "starting call to webservice using http POST  try: " + CStr(i + 1), certificate)
                    call_web_service_dynamic = send_via_http_post(url + "/" + webmethod, args, isbytes, certificate)
                    ocomm = New bc_cs_activity_log("bc_cs_third_party_webservices", "call_wb_service_dynamic", bc_cs_activity_codes.COMMENTARY, "completed call to webservice", certificate)
                    If error_text <> "" Then
                        err = True
                        If i = bc_cs_central_settings.retry - 1 Then
                            Exit For
                        Else
                            REM wait for timeout before trying again
                            ocomm = New bc_cs_activity_log("bc_cs_third_party_webservices", "call_wb_service_dynamic", bc_cs_activity_codes.COMMENTARY, "waiting for retry:" + bc_cs_central_settings.timeout + "seconds", certificate)
                            error_text = ""
                            Thread.Sleep(bc_cs_central_settings.timeout * 1000)
                        End If
                    Else
                        error_text = ""
                        err = False
                        Exit For
                    End If
                Else
                    ocomm = New bc_cs_activity_log("bc_cs_third_party_webservices", "call_wb_service_dynamic", bc_cs_activity_codes.COMMENTARY, "starting call to webmethod:" + webmethod + " try: " + CStr(i + 1), certificate)
                    call_web_service_dynamic = oTObject.call_web_method(webmethod, args)
                    ocomm = New bc_cs_activity_log("bc_cs_third_party_webservices", "call_wb_service_dynamic", bc_cs_activity_codes.COMMENTARY, "completed call to webmethod:" + webmethod, certificate)
                    If oTObject._err_text <> "" Then
                        error_text = "webmethod call error: " + oTObject._err_text
                        err = True
                        If i = bc_cs_central_settings.retry - 1 Then
                            Exit For
                        Else
                            REM wait for timeout before trying again
                            ocomm = New bc_cs_activity_log("bc_cs_third_party_webservices", "call_wb_service_dynamic", bc_cs_activity_codes.COMMENTARY, "waiting for retry:" + bc_cs_central_settings.timeout + "seconds", certificate)
                            Thread.Sleep(bc_cs_central_settings.timeout * 1000)
                        End If
                    Else
                        error_text = ""
                        err = False
                        Exit For
                    End If
                End If
            Next
        Catch ex As Exception
            error_text = "could not invoke bc_cs_dynamic_web_proxy" + ex.Message
            err = True
            call_web_service_dynamic = Nothing
        Finally
            otrace = New bc_cs_activity_log("bc_cs_third_party_web_service", "call_web_service_dynamic", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Function
    Private Function set_arguments(ByVal external_id As Long, ByVal entity_id As Long, ByVal pub_type_id As Long, ByVal contributor_id As Long, ByVal stage_Id As Long, ByVal acc_id As Long, ByVal language_id As Long, ByVal data_at_date As Date, ByVal doc_id As String, ByVal parameters As Object, ByVal web_service_params As bc_cs_web_method_parameters, ByRef args() As Object, ByVal rollback_mode As Boolean, ByRef isbytes As Boolean, ByRef certificate As bc_cs_security.certificate, ByVal params As String, ByRef db As bc_cs_db_services) As String
        Dim otrace As New bc_cs_activity_log("bc_cs_third_party_web_service", "set_arguments", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Dim i, j As Integer
        Try
            Dim largs(web_service_params.web_method_params.Count - 1) As Object
            Dim ocomm As bc_cs_activity_log
            isbytes = False
            set_arguments = ""
            For i = 0 To web_service_params.web_method_params.Count - 1
                With web_service_params.web_method_params(i)
                    REM custom, 1 exetrnal_id
                    Select Case .system_type_id

                        Case 0
                            REM if custom value exists and not system definedassign that
                            REM else assign default value
                            largs(i) = .default_value
                            REM see if BC parameter is here and not assigned use this instead
                            If IsNothing(parameters) = False Then
                                For j = 0 To parameters.component_template_parameters.count - 1
                                    If parameters.component_template_parameters(j).name = .name Then
                                        If parameters.component_template_parameters(j).system_defined = 0 Then

                                            largs(i) = parameters.component_template_parameters(j).default_value
                                        End If
                                        Exit For
                                    End If
                                Next
                            End If

                        Case 1
                            REM external_id
                            largs(i) = external_id
                        Case 2
                            REM entity_id
                            largs(i) = entity_id
                        Case 3
                            REM pub_type_id
                            largs(i) = pub_type_id
                        Case 4
                            REM contributor_id
                            largs(i) = contributor_id
                        Case 5
                            REM stage_id
                            If .data_type = 3 Then
                                If stage_Id = 1 Then
                                    largs(i) = False
                                Else
                                    largs(i) = True
                                End If
                            Else
                                largs(i) = stage_Id
                            End If
                        Case 6
                            REM acc_id
                            largs(i) = acc_id
                        Case 7
                            REM language_id
                            largs(i) = language_id
                        Case 8
                            REM data_at_date
                            largs(i) = data_at_date
                        Case 9
                            REM doc_id
                            largs(i) = doc_id

                        Case 10
                            largs(i) = certificate.user_id
                        Case 11
                            largs(i) = certificate.os_name
                        Case 12
                            largs(i) = certificate.name
                        Case 13
                            largs(i) = rollback_mode

                        Case 100, 101, 102, 103
                            REM sep 2010 XML string
                            REM 100 compressed, external config file passed into SP
                            REM 101 uncompressed, external config file passed into SP
                            REM 102 compressed, no external config file passed into SP
                            REM 103 uncompressed, no external config file passed into SP

                            REM compresses xml output
                            REM default value is an sp
                            Dim gdb As New bc_cs_db_services
                            Dim vres As Object
                            Dim sp As String
                            REM see if a config file is available
                            Dim fs As New bc_cs_file_transfer_services
                            Dim fn As String
                            Dim fstr As String

                            If .system_type_id = 100 Or .system_type_id = 101 Then
                                fn = bc_cs_central_settings.central_template_path + .default_value + "_config.xml"
                                If fs.check_document_exists(fn, certificate) = False Then
                                    set_arguments = "Config file: " + fn + " does not exist"
                                    Exit Function
                                Else
                                    fstr = fs.write_document_to_string(fn)
                                End If
                                sp = "exec dbo." + .default_value + " " + CStr(doc_id) + ",'" + fstr + "'"
                            Else
                                REM FIL April 2013
                                If params = "" Then
                                    sp = "exec dbo." + .default_value + "'" + CStr(doc_id) + "'"
                                Else
                                    Dim ss As New bc_cs_string_services(params)
                                    params = ss.delimit_apostrophies
                                    sp = "exec dbo." + .default_value + "'" + CStr(doc_id) + "','" + params + "'"
                                End If
                            End If
                            ocomm = New bc_cs_activity_log("bc_cs_third_party_web_service", "set_arguments", bc_cs_activity_codes.COMMENTARY, "attempting to execute sp: " + sp, certificate)
                            If IsNothing(db) = True Then
                                vres = gdb.executesql_show_no_error(sp)
                            Else
                                vres = db.executesql_trans(sp, certificate, True)
                                If db.success = False Then
                                    set_arguments = "Error executing SP: " + .default_value + ":" + Right(vres(0, 0), Len(vres(0, 0)) - 5)
                                    Exit Function
                                End If
                            End If
                            Dim ostr As String = ""
                            If IsArray(vres) Then
                                If Left(vres(0, 0), 5) = "Error" Then
                                    set_arguments = "Error executing SP: " + .default_value + ":" + Right(vres(0, 0), Len(vres(0, 0)) - 5)
                                    Exit Function
                                Else
                                    largs(i) = vres(0, 0)
                                End If
                            Else
                                set_arguments = "Error executing SP" + .default_value
                            End If
                            Dim err_tx As String
                            err_tx = parse_for_documents(CStr(largs(i)), ostr, certificate)
                            largs(i) = ostr
                            If err_tx <> "" Then
                                set_arguments = err_tx
                                Exit Function
                            End If
                            Dim bcs As New bc_cs_security
                            REM type id 100 is compress
                            If .system_type_id = 100 Or .system_type_id = 102 Then
                                isbytes = True
                                largs(i) = bcs.compress_xml_winzip(CStr(largs(i)), certificate)
                            End If
                            If err_tx <> "" Then
                                set_arguments = err_tx
                                Exit Function
                            End If

                    End Select

                    Select Case .data_type
                        Case 0
                            ocomm = New bc_cs_activity_log("bc_cs_third_party_webservices", "set_arguments", bc_cs_activity_codes.COMMENTARY, "Setting parameter: " + CStr(.name) + ", data type string with value: " + CStr(largs(i)), certificate)
                            args(i) = CStr(largs(i))

                        Case 1
                            ocomm = New bc_cs_activity_log("bc_cs_third_party_webservices", "set_arguments", bc_cs_activity_codes.COMMENTARY, "Setting parameter: " + CStr(.name) + ", data type date with value: " + CStr(largs(i)), certificate)
                            args(i) = CDate(largs(i))
                        Case 2
                            ocomm = New bc_cs_activity_log("bc_cs_third_party_webservices", "set_arguments", bc_cs_activity_codes.COMMENTARY, "Setting parameter: " + CStr(.name) + ", data type integer with value: " + CStr(largs(i)), certificate)
                            args(i) = CInt(largs(i))
                        Case 3
                            ocomm = New bc_cs_activity_log("bc_cs_third_party_webservices", "set_arguments", bc_cs_activity_codes.COMMENTARY, "Setting parameter: " + CStr(.name) + ", data type boolean with value: " + CStr(largs(i)), certificate)
                            args(i) = CBool(largs(i))
                        Case 4
                            ocomm = New bc_cs_activity_log("bc_cs_third_party_webservices", "set_arguments", bc_cs_activity_codes.COMMENTARY, "Setting parameter: " + CStr(.name) + ", data type byte array", certificate)
                            args(i) = (largs(i))
                    End Select

                End With
            Next
        Catch ex As Exception
            set_arguments = "error with setting web method parameter: " + web_service_params.web_method_params(i).name + ": " + ex.Message
        Finally
            otrace = New bc_cs_activity_log("bc_cs_third_party_web_service", "set_arguments", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Function
    Private Function send_via_http_post(ByVal address As String, ByVal args() As Object, ByVal inbytes As Boolean, ByRef certificate As bc_cs_security.certificate) As Object
        Dim otrace As New bc_cs_activity_log("bc_cs_third_party_web_service", "send_via_http_post", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try

            send_via_http_post = Nothing
            Dim ocomm As New bc_cs_activity_log("bc_cs_third_party_webservices", "CHECKURL", bc_cs_activity_codes.COMMENTARY, "Attemping to send via http post to address:" + address, certificate)
            error_text = ""
            Dim request As HttpWebRequest = WebRequest.Create(address)
            request.Method = "POST"
            request.ContentType = "application/xml"
            'request.ContentType = "application/soap+xml; charset=utf-8"
            'request.ContentType = "application/soap+xml"
            Dim bytedata As Byte()
            If inbytes = False Then
                bytedata = UTF8Encoding.UTF8.GetBytes(CStr(args(0)))
            Else
                bytedata = args(0)
            End If
            REM REMOVE THIS
            Dim fs As New FileStream("c:\bluecurve\cdp.zip", FileMode.Create)
            fs.Write(bytedata, 0, bytedata.Length)
            fs.Close()


            REM
            request.ContentLength = bytedata.Length
            Dim poststream As Stream = request.GetRequestStream
            ocomm = New bc_cs_activity_log("bc_cs_third_party_webservices", "send_via_http_post", bc_cs_activity_codes.COMMENTARY, "http post body size:" + CStr(bytedata.Length), certificate)
            poststream.Write(bytedata, 0, bytedata.Length)
            Dim response As HttpWebResponse = request.GetResponse
            Dim reader As New StreamReader(response.GetResponseStream)
            Dim post_response As String
            post_response = reader.ReadToEnd
            REM extract bidy part of reponse
            ocomm = New bc_cs_activity_log("bc_cs_third_party_web_service", "send_via_http_post", bc_cs_activity_codes.COMMENTARY, "RAW Post Response:" + post_response, certificate)
            post_response = post_response.Replace("<?xml version=""1.0"" encoding=""utf-8""?>", "")
            post_response = post_response.Replace("<?xml version=""1.0"" encoding=""utf-16""?>", "")
            post_response = post_response.Replace("</string>", "")
            ocomm = New bc_cs_activity_log("bc_cs_third_party_web_service", "send_via_http_post", bc_cs_activity_codes.COMMENTARY, "Post Response1:" + post_response, certificate)
            If InStr(post_response, "http://tempuri.org/") > 0 Then
                post_response = Right(post_response, Len(post_response) - InStr(post_response, ">"))
            End If
            ocomm = New bc_cs_activity_log("bc_cs_third_party_web_service", "send_via_http_post", bc_cs_activity_codes.COMMENTARY, "Post Response:" + post_response, certificate)
            send_via_http_post = post_response

        Catch ex As Exception
            send_via_http_post = Nothing
            error_text = "failed to send via http post: " + ex.Message
        Finally
            otrace = New bc_cs_activity_log("bc_cs_third_party_web_service", "send_via_http_post", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Function
    Public Function parse_for_documents(ByVal sxml As String, ByRef oxml As String, ByRef certificate As bc_cs_security.certificate) As String
        Try
            Dim fn As String
            Dim fs As New bc_cs_file_transfer_services
            Dim doc_byte As Byte() = Nothing
            Dim str As String
            Dim xml As String
            parse_for_documents = ""
            xml = sxml
            REM ABG June 2012
            oxml = sxml
            While InStr(xml, "BC_IMPORT_DOC") > 0
                xml = Right(xml, Len(xml) - (InStr(xml, "BC_IMPORT_DOC") + 13))
                fn = Left(xml, InStr(xml, "}") - 1)
                REM now write this file to a bytestream
                If fs.check_document_exists(bc_cs_central_settings.central_repos_path + fn) Then
                    fs.write_document_to_bytestream(bc_cs_central_settings.central_repos_path + fn, doc_byte, certificate)
                Else
                    parse_for_documents = "couldnt find document " + bc_cs_central_settings.central_repos_path + fn
                    Exit Function
                End If
                str = Convert.ToBase64String(doc_byte)
                sxml = sxml.Replace("BC_IMPORT_DOC{" + fn + "}", str)
                oxml = sxml
            End While
        Catch ex As Exception
            parse_for_documents = "error parsing for documents + " + ex.Message

        End Try

    End Function

    REM to be implemented later
    <Serializable()> Public Class bc_cs_web_methods
        Inherits bc_cs_soap_base_class
        Public methods As New ArrayList
        Public Overrides Sub process_object()
            Select Case tmode
                Case bc_cs_soap_base_class.tREAD
                    db_read()
            End Select
        End Sub
        Public Sub db_read()
            Dim i As Integer
            Dim gdb As New bc_cs_web_method_parameters_db
            Dim vres As Object
            Dim omethod As bc_cs_web_method
            methods.Clear()
            vres = gdb.read_web_methods(MyBase.certificate)
            If IsArray(vres) Then
                For i = 0 To UBound(vres, 2)
                    omethod = New bc_cs_web_method
                    omethod.web_method_name = vres(0, i)
                    omethod.servicename = vres(1, i)
                    omethod.url = vres(2, i)
                    omethod.certificate = MyBase.certificate
                    omethod.db_read()
                    Me.methods.Add(omethod)
                Next
            End If

        End Sub
    End Class
    <Serializable()> Public Class bc_cs_web_method
        Inherits bc_cs_soap_base_class
        Public web_method_name As String
        Public original_name As String
        Public url As String
        Public servicename As String
        Public write_mode As Integer = 0
        Public parameters As New bc_cs_web_method_parameters
        Public Overrides Sub process_object()
            Select Case tmode
                Case bc_cs_soap_base_class.tREAD
                    db_read()

                Case bc_cs_soap_base_class.tWRITE
                    db_write()
            End Select
        End Sub
        Public Sub db_read()
            parameters.web_method_name = Me.web_method_name
            parameters.db_read(MyBase.certificate)
        End Sub
        Public Sub db_write()
            Dim gdb As New bc_cs_web_method_parameters_db
            Select Case write_mode

                Case 0
                    gdb.insert_web_method(Me.web_method_name, Me.servicename, Me.url, MyBase.certificate)
                Case 1
                    gdb.update_web_method(original_name, web_method_name, servicename, url, MyBase.certificate)
                Case 2
                    gdb.delete_web_method(web_method_name, MyBase.certificate)

            End Select

        End Sub
    End Class
    <Serializable()> Public Class bc_cs_web_method_parameters
        Inherits bc_cs_soap_base_class

        Public web_method_params As New ArrayList
        Public web_method_name As String
        Public Overrides Sub process_object()
            Select Case tmode
                Case bc_cs_soap_base_class.tREAD
                    db_read(MyBase.certificate)

                Case bc_cs_soap_base_class.tWRITE
                    db_write()
            End Select
        End Sub
        Public Sub db_read(ByRef certificate As bc_cs_security.certificate)
            Dim otrace As New bc_cs_activity_log("bc_cs_third_party_web_service", "call_web_service", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
            Try
                Dim vres As Object
                Dim oparam As bc_cs_web_method_parameter
                Dim i As Integer
                Dim gdb As New bc_cs_web_method_parameters_db
                vres = gdb.read_web_params_for_method(web_method_name, certificate)
                web_method_params.Clear()
                If IsArray(vres) Then
                    For i = 0 To UBound(vres, 2)
                        oparam = New bc_cs_web_method_parameter
                        oparam.name = vres(0, i)
                        oparam.system_type_id = vres(1, i)
                        oparam.default_value = vres(2, i)
                        oparam.data_type = vres(3, i)
                        web_method_params.Add(oparam)
                    Next
                End If

            Catch ex As Exception
                Dim oerr As New bc_cs_error_log("bc_cs_web_method_parameters", "db_readf", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            Finally
                otrace = New bc_cs_activity_log("bc_cs_third_party_web_service", "call_web_service", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
            End Try
        End Sub
        Public Sub db_write()
            Dim i As Integer
            Dim gdb As New bc_cs_web_method_parameters_db
            gdb.delete_method_params(Me.web_method_name, MyBase.certificate)
            For i = 0 To web_method_params.Count - 1
                web_method_params(i).db_write(i, Me.web_method_name)
            Next
        End Sub
    End Class
    <Serializable()> Public Class bc_cs_web_method_parameter
        Inherits bc_cs_soap_base_class

        Public name As String
        Public system_type_id As Integer
        Public default_value As String
        Public data_type As Integer
        Public write_mode As Integer


        Public Sub New()

        End Sub
        Public Sub db_write(ByVal ord As Integer, ByVal web_method_name As String)
            Dim gdb As New bc_cs_web_method_parameters_db
            gdb.add_parameter(name, web_method_name, ord, system_type_id, data_type, default_value, MyBase.certificate)
        End Sub
    End Class
    Public Class bc_cs_web_method_parameters_db
        Private gbc_db As New bc_cs_db_services(False)
        Public Sub delete_method_param(ByVal ord As Integer, ByVal method_name As String, ByRef certificate As bc_cs_security.certificate)
            Dim sql As String
            sql = "delete from  bc_core_web_method_parameters where web_method_name='" + method_name + "' and ord=" + CStr(ord)
            gbc_db.executesql(sql, certificate)
        End Sub
        Public Sub add_parameter(ByVal name As String, ByVal method_name As String, ByVal ord As Integer, ByVal system_type_id As Integer, ByVal data_type_id As Integer, ByVal default_value As String, ByRef certificate As bc_cs_security.certificate)
            Dim sql As String
            Dim fs As New bc_cs_string_services(default_value)
            default_value = fs.delimit_apostrophies
            REM SR - Column names included in the Insert statement
            sql = "insert into  bc_core_web_method_parameters (web_method_name,name,ord,system_type_id,default_value,data_type) values ('" + method_name + "','" + name + "'," + CStr(ord) + "," + CStr(system_type_id) + ",'" + default_value + "'," + CStr(data_type_id) + ")"
            gbc_db.executesql(sql, certificate)
        End Sub
        Public Sub delete_method_params(ByVal method_name As String, ByRef certificate As bc_cs_security.certificate)
            Dim sql As String
            sql = "delete from  bc_core_web_method_parameters where web_method_name='" + method_name + "'"
            gbc_db.executesql(sql, certificate)
        End Sub
        Public Function get_service_name_and_url(ByVal method_name As String, ByRef certificate As bc_cs_security.certificate) As Object
            Dim sql As String
            sql = "select service_name,url from bc_core_external_web_methods where method_name='" + method_name + "'"
            get_service_name_and_url = gbc_db.executesql(sql, certificate)
        End Function
        Public Function read_web_methods(ByRef certificate As bc_cs_security.certificate) As Object
            Dim sql As String
            sql = "select method_name,service_name,url from bc_core_external_web_methods order by method_name asc"
            read_web_methods = gbc_db.executesql(sql, certificate)
        End Function
        Public Function read_web_params_for_method(ByVal web_method_name As String, ByRef certificate As bc_cs_security.certificate)
            Dim sql As String
            sql = "select name,system_type_id,coalesce(default_value,''),data_type from dbo.bc_core_web_method_parameters where web_method_name= '" + CStr(web_method_name) + "' order by ord asc"
            read_web_params_for_method = gbc_db.executesql(sql, certificate)
        End Function
        Public Sub insert_web_method(ByVal web_method_name As String, ByVal service_name As String, ByVal url As String, ByRef certificate As bc_cs_security.certificate)
            Dim sql As String
            REM SR - "bc_core_external_web_methods" is a redundant table
            sql = "insert into bc_core_external_web_methods values('" + web_method_name + "','" + service_name + "','" + url + "')"
            gbc_db.executesql(sql, certificate)
        End Sub
        Public Sub delete_web_method(ByVal web_method_name As String, ByRef certificate As bc_cs_security.certificate)
            Dim sql As String
            sql = "delete from  bc_core_web_method_parameters where web_method_name='" + web_method_name + "'"
            gbc_db.executesql(sql, certificate)
            sql = "delete from  bc_core_external_web_methods where method_name='" + web_method_name + "'"
            gbc_db.executesql(sql, certificate)
        End Sub
        Public Sub update_web_method(ByVal original_name As String, ByVal web_method_name As String, ByVal service_name As String, ByVal url As String, ByRef certificate As bc_cs_security.certificate)
            Dim sql As String
            sql = "update bc_core_web_method_parameters set web_method_name='" + web_method_name + "' where web_method_name='" + original_name + "'"
            gbc_db.executesql(sql, certificate)
            sql = "update bc_core_external_web_methods set method_name ='" + web_method_name + "', service_name='" + service_name + "', url='" + url + "' where method_name='" + original_name + "'"
            gbc_db.executesql(sql, certificate)
        End Sub
    End Class
End Class
Public Class bc_cs_external_assemblies
    Public Shared loaded_external_assemblies As New ArrayList
    Public err_text As String
    Public success As Boolean = True
    Public Function load_assembly() As bc_cs_external_assembly
        Try
            Dim i As Integer
            Dim ocomm As bc_cs_activity_log
            Dim loaded As Boolean = False
            Dim ows As bc_cs_external_assembly = Nothing
            REM only load if not already loaded

            For i = 0 To loaded_external_assemblies.Count - 1
                If loaded_external_assemblies(i).namespace_name = bc_cs_central_settings.custom_process_events_assembly_namespace Then
                    loaded = True
                    ows = loaded_external_assemblies(i)
                    Exit For
                End If
            Next
            If loaded = False Then
                ocomm = New bc_cs_activity_log("bc_cs_external_assemblies", "load_assembly", bc_cs_activity_codes.COMMENTARY, "Loading external assembly: " + bc_cs_central_settings.custom_process_events_assembly_namespace)
                ows = New bc_cs_external_assembly(bc_cs_central_settings.custom_process_events_assembly_fullname, bc_cs_central_settings.custom_process_events_assembly_namespace)
                If ows.err = True Then
                    Dim oerr As New bc_cs_error_log("bc_cs_external_assemblies", "load_assembly", bc_cs_error_codes.USER_DEFINED, ows.err_txt)
                    Me.err_text = ows.err_txt
                    success = False
                    load_assembly = Nothing
                    Exit Function
                Else
                    loaded_external_assemblies.Add(ows)
                End If
            End If
            load_assembly = ows
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_cs_external_assemblies", "load_assembly", bc_cs_error_codes.USER_DEFINED, ex.Message)
            load_assembly = Nothing
        End Try
    End Function

End Class
Public Class bc_cs_external_assembly
    Public namespace_name As String
    Public assembly_name As String
    Public err As Boolean = False
    Public err_txt As String = ""
    Private ass As System.Reflection.Assembly

    Public Sub New(ByVal assembly_name As String, ByVal namespace_name As String)
        Try
            err = False
            Me.namespace_name = namespace_name
            Me.assembly_name = assembly_name

            If load_assembly() = False Then
                err = True
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_cs_external_assembly", "new", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Private Function load_assembly() As Boolean
        Try
            load_assembly = False
            Dim ocomm As New bc_cs_activity_log("bc_cs_third_party_web_service", "load_assembly", bc_cs_activity_codes.COMMENTARY, "Attempting to load assembly: " + assembly_name)
            ass = Reflection.Assembly.Load(assembly_name)
            load_assembly = True
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_cs_external_assembly", "load_assembly", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Function
    Public Function run_action(ByVal class_name As String, ByVal action_name As String, ByRef obj As Object, ByRef doc_open As Boolean, ByRef current_document As Object) As Boolean
        Dim ocomm As New bc_cs_activity_log("bc_cs_external_assembly", "run_action", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            Dim fullname As String
            run_action = False
            REM bind to run method 
            err = False
            fullname = namespace_name + "." + class_name + ", " + assembly_name
            ocomm = New bc_cs_activity_log("bc_cs_external_assembly", "run_action", bc_cs_activity_codes.COMMENTARY, "Attempting to bind to: " + namespace_name + "." + class_name)

            Dim t As Type = ass.GetType(namespace_name + "." + class_name)
            Dim oTObject As Object = Activator.CreateInstance(t)
            REM need to set parameters here as well
            REM mandatory attributes
            REM custom attributes
            'Dim custom_event As New pr_custom_event
            oTObject.action_name = action_name
            oTObject.bc_document = current_document
            oTObject.initialise(obj)
            If oTObject.event_complete = False Then
                doc_open = True
                run_action = oTObject.run
            Else
                run_action = True
            End If
            REM copy over custom history
            Dim k As Integer
            For k = 0 To oTObject.history_list.Count - 1
                current_document.history.Add(oTObject.history_list(k))
            Next
        Catch ex As Exception
            run_action = False
            Dim oerr As New bc_cs_error_log("bc_cs_external_assembly", "run_action", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            ocomm = New bc_cs_activity_log("bc_cs_external_assembly", "run_action", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
End Class


Public Class bc_cs_dynamic_web_proxy
    Public _url As String
    Public _servicename As String
    Public _err_text As String
    Private wsvcClass As Object
    Public Shared in_progress As Boolean = False
    Public Function load_proxy(Optional ByVal from_external As Boolean = False, Optional ByRef ext_wsvcClass As Object = Nothing, Optional ByRef certificate As bc_cs_security.certificate = Nothing) As Boolean
        Dim ocomm As New bc_cs_activity_log("bc_cs_dynamic_web_proxy 5.6.0", "load_proxy", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try

            Dim i As Integer
            load_proxy = True
            REM check if proxy is loaded
            For i = 0 To bc_compiled_proxies.proxies.Count - 1
                If bc_compiled_proxies.proxies(i).url = _url And bc_compiled_proxies.proxies(i).service_name = _servicename Then
                    wsvcClass = bc_compiled_proxies.proxies(i).wsvcClass
                    ocomm = New bc_cs_activity_log("bc_cs_dynamic_web_proxy", "load_proxy", bc_cs_activity_codes.COMMENTARY, "Proxy taken from Cache: " + _url + " : " + _servicename, certificate)
                    Exit Function
                End If
            Next

            Dim webServiceAsmxUrl As String
            webServiceAsmxUrl = _url
            Dim client As New System.Net.WebClient()
            'Connect To the web service 

            ocomm = New bc_cs_activity_log("bc_cs_dynamic_web_proxy", "load_proxy", bc_cs_activity_codes.COMMENTARY, "URL: " + _url, certificate)


            Dim stream As System.IO.Stream

            stream = client.OpenRead(webServiceAsmxUrl + "?wsdl")

            ocomm = New bc_cs_activity_log("bc_cs_dynamic_web_proxy", "load_proxy", bc_cs_activity_codes.COMMENTARY, "Stream Open: " + webServiceAsmxUrl + "?wsdl", certificate)


            ' Now read the WSDL file describing a service. 
            Dim description As ServiceDescription
            description = ServiceDescription.Read(stream)

            ocomm = New bc_cs_activity_log("bc_cs_dynamic_web_proxy", "load_proxy", bc_cs_activity_codes.COMMENTARY, "Description read", certificate)


            '///// LOAD THE DOM ///////// 
            ' Initialize a service description importer. 
            Dim importer As New ServiceDescriptionImporter()
            importer.ProtocolName = "SOAP" ' // Use SOAP 1.2. 
            importer.AddServiceDescription(description, "", "")

            '// Generate a proxy client. 
            importer.Style = ServiceDescriptionImportStyle.Client

            '// Generate properties to represent primitive values. 
            importer.CodeGenerationOptions = System.Xml.Serialization.CodeGenerationOptions.GenerateProperties

            ocomm = New bc_cs_activity_log("bc_cs_dynamic_web_proxy", "load_proxy", bc_cs_activity_codes.COMMENTARY, "importer", certificate)


            '// Initialize a Code-DOM tree into which we will import the service. 
            Dim nmspace As New CodeNamespace()
            Dim unit1 As New CodeCompileUnit()
            unit1.Namespaces.Add(nmspace)

            ocomm = New bc_cs_activity_log("bc_cs_dynamic_web_proxy", "load_proxy", bc_cs_activity_codes.COMMENTARY, "namespace", certificate)


            'Import the service into the Code-DOM tree. This creates proxy code that uses the service. 
            Dim warning As New ServiceDescriptionImportWarnings
            warning = importer.Import(nmspace, unit1)

            ocomm = New bc_cs_activity_log("bc_cs_dynamic_web_proxy", "load_proxy", bc_cs_activity_codes.COMMENTARY, "warning", certificate)


            If (warning = 0) Then ' If zero then we are good to go 
                ocomm = New bc_cs_activity_log("bc_cs_dynamic_web_proxy", "load_proxy", bc_cs_activity_codes.COMMENTARY, "no warning", certificate)

                '// Generate the proxy code 
                Dim provider1 As CodeDomProvider



                provider1 = CodeDomProvider.CreateProvider("CSHARP")
                '// Compile the assembly proxy with the appropriate references 
                Dim assemblyReferences(5) As String
                assemblyReferences(0) = "System.dll"
                assemblyReferences(1) = "System.Web.dll"
                assemblyReferences(2) = "System.Web.Services.dll"
                assemblyReferences(3) = "System.XML.dll"
                assemblyReferences(4) = "System.Data.dll"

                Dim parms As CompilerParameters
                parms = New CompilerParameters(assemblyReferences)

                ocomm = New bc_cs_activity_log("bc_cs_dynamic_web_proxy", "load_proxy", bc_cs_activity_codes.COMMENTARY, "parms", certificate)


                Dim results As CompilerResults
                Try
                   
                    results = provider1.CompileAssemblyFromDom(parms, unit1)
                Catch ex As Exception
                    ocomm = New bc_cs_activity_log("bc_cs_dynamic_web_proxy", "load_proxy", bc_cs_activity_codes.COMMENTARY, "method 1 failed try without assemblies 4.5", certificate)
                    parms = New CompilerParameters()
                    Try
                        results = provider1.CompileAssemblyFromDom(parms, unit1)
                    Catch exx As Exception
                        ocomm = New bc_cs_activity_log("bc_cs_dynamic_web_proxy", "load_proxy", bc_cs_activity_codes.COMMENTARY, exx.StackTrace, certificate)
                        _err_text = " implicit method failed"
                        load_proxy = False
                        Exit Function
                    End Try
                End Try

                ocomm = New bc_cs_activity_log("bc_cs_dynamic_web_proxy", "load_proxy", bc_cs_activity_codes.COMMENTARY, "results", certificate)


                '// Check For Errors 
                'If (results.Errors.Count > 0) Then

                '    For Each oops In results.Errors

                '        Debug.WriteLine(oops.ErrorText)

                '    Next

                'End If
                If from_external = False Then
                    wsvcClass = results.CompiledAssembly.CreateInstance(_servicename)
                    ocomm = New bc_cs_activity_log("bc_cs_dynamic_web_proxy", "load_proxy", bc_cs_activity_codes.COMMENTARY, "external false", certificate)

                Else
                    ext_wsvcClass = results.CompiledAssembly.CreateInstance(_servicename)
                    ocomm = New bc_cs_activity_log("bc_cs_dynamic_web_proxy", "load_proxy", bc_cs_activity_codes.COMMENTARY, "external true", certificate)

                End If
                REM add to cache
                Dim oproxy As New bc_compiled_proxy
                oproxy.url = _url
                oproxy.service_name = _servicename
                oproxy.wsvcClass = wsvcClass
                bc_compiled_proxies.proxies.Add(oproxy)
            Else
                load_proxy = False
                _err_text = warning.ToString
            End If
        Catch ex As Exception
            load_proxy = False
            _err_text = ex.Message
        Finally
            ocomm = New bc_cs_activity_log("bc_cs_dynamic_web_proxy", "load_proxy", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Function
    Public Function call_web_method(ByVal method As String, ByVal args() As Object) As Object
        Try
            Me._err_text = ""
            Dim mi As Object
            REM keep it single concurrency as used shared memory
            'While in_progress = True

            'End While
            in_progress = True
            mi = wsvcClass.GetType().GetMethod(method)
            call_web_method = mi.Invoke(wsvcClass, args)
            in_progress = False
        Catch ex As Exception
            Me._err_text = ex.Message
            call_web_method = Nothing
        Finally
            in_progress = False
        End Try
    End Function
    Public Function call_web_method(ByVal method As String, ByVal args() As Object, ByVal ext_wsvcClass As Object) As Object
        Try
            Me._err_text = ""
            Dim mi As Object

            in_progress = True
            mi = ext_wsvcClass.GetType().GetMethod(method)
            call_web_method = mi.Invoke(ext_wsvcClass, args)
            in_progress = False
        Catch ex As Exception
            Me._err_text = ex.Message
            call_web_method = Nothing
        Finally
            in_progress = False
        End Try
    End Function
    Private Class bc_compiled_proxies
        Public Shared proxies As New ArrayList
    End Class
    Private Class bc_compiled_proxy
        Public url As String
        Public service_name As String
        Public wsvcClass As Object
    End Class

    
End Class
