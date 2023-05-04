Imports BlueCurve.Core.CS
Imports System.io
Imports System.xml
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Net
Imports System.Net.WebProxy
Imports System.ServiceModel
Imports System.ServiceModel.Channels
Imports System.Net.NetworkInformation
Imports System.Security.Cryptography.X509Certificates

Imports System.Xml.Serialization
Imports System.Web.Script.Serialization
Imports System.Text





REM =========================================================
REM == New Generic base class so only one web service is used
REM =========================================================
REM ===============================================
REM == Test connection class 
<Serializable()> Public Class bc_cs_test_soap_server_connection
    Inherits bc_cs_soap_base_class
    Public test_receive As String
    Public Sub New()

    End Sub
    Public Overrides Sub process_object()
        Dim otrace As New bc_cs_activity_log("bc_om_test_soap_server_connection", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            REM this is always specific to object
            Me.test_receive = "RECEIVED"
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_test_soap_server_connection", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_test_soap_server_connection", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub
End Class
<Serializable()> Public Class bc_cs_soap_base_class
    Public Const tREAD = 1
    Public Const tWRITE = 2
    Public Const tREAD_ALL_CHECKED_OUT = 3
    Public Const tHASHPASSWORD = 10
    Public certificate As New bc_cs_security.certificate
    Public packet_code As String
    Public tmode As Integer
    Public transmission_state As Integer
    Public no_send_back As Boolean
    Public async As Boolean = False
    <NonSerialized()> Private packets As cs_objects_packets
    <NonSerialized()> Private received_packets As cs_objects_packets
    REM write object to database
    '<NonSerialized()> Protected webservice As BlueCurveWebService.BlueCurveWS
    Public Sub New()
        no_send_back = False
    End Sub
    Public Shared wcfclient As ServiceReference1.BlueCurveWSClient = Nothing
   
    REM

    Public Shared Function load_wcf_proxy()
        'Dim otrace As New bc_cs_activity_log("bc_cs_soap_base_class", "transmit_to_server_and_receive", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            'bc_cs_central_settings.webconfig_maxRequestLength = 2147483647
            'bc_cs_central_settings.webconfig_maxRequestLength = 3000000
            'Define the endpoint address'
            Dim endpointStr = bc_cs_central_settings.soap_server
            Dim endpointadd As New EndpointAddress(endpointStr)

            If bc_cs_central_settings.wcf_binding <> "binary" Then
                REM basic binding



                If bc_cs_central_settings.wcf_binding <> "https" Then
                    Dim binding As New BasicHttpBinding()

                    binding.Name = "BCBinding"
                    binding.CloseTimeout = TimeSpan.FromMilliseconds(bc_cs_central_settings.timeout)
                    binding.OpenTimeout = TimeSpan.FromMilliseconds(bc_cs_central_settings.timeout)
                    binding.ReceiveTimeout = TimeSpan.FromMilliseconds(bc_cs_central_settings.timeout * 10)
                    binding.SendTimeout = TimeSpan.FromMilliseconds(bc_cs_central_settings.timeout)

                    binding.AllowCookies = False
                    binding.BypassProxyOnLocal = False
                    binding.HostNameComparisonMode = HostNameComparisonMode.StrongWildcard
                    binding.MaxBufferSize = 2147483647
                    binding.MaxReceivedMessageSize = 2147483647
                    binding.MaxBufferPoolSize = 2147483647
                    binding.MessageEncoding = WSMessageEncoding.Text
                    binding.TextEncoding = System.Text.Encoding.UTF8
                    binding.TransferMode = TransferMode.Buffered
                    binding.UseDefaultWebProxy = True

                    binding.ReaderQuotas.MaxDepth = 32
                    binding.ReaderQuotas.MaxStringContentLength = 1000000000
                    binding.ReaderQuotas.MaxArrayLength = 1000000000
                    binding.ReaderQuotas.MaxBytesPerRead = 4096
                    binding.ReaderQuotas.MaxNameTableCharCount = 16384

                    binding.Security.Mode = BasicHttpSecurityMode.None
                    binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None
                    binding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None
                    binding.Security.Transport.Realm = ""
                    binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName
                    'binding.Security.Message.AlgorithmSuite = Security.SecurityAlgorithmSuite.Default
                    wcfclient = New ServiceReference1.BlueCurveWSClient(binding, endpointadd)
                Else

                    '                    <service name="WCFWSHttps.Service1" behaviorConfiguration="WCFWSHttps.Service1Behavior">
                    '<!-- Service Endpoints -->
                    '<endpoint address="https://localhost/WCFWSHttps/Service1.svc" binding="wsHttpBinding" 
                    '   bindingConfiguration="TransportSecurity" contract="WCFWSHttps.IService1"/>
                    '<endpoint address="mex" binding="mexHttpsBinding" contract="IMetadataExchange"/>
                    '</service>

                    Dim binding As New BasicHttpsBinding()

                    binding.Name = "TransportSecurity"
                    binding.CloseTimeout = TimeSpan.FromMilliseconds(bc_cs_central_settings.timeout)
                    binding.OpenTimeout = TimeSpan.FromMilliseconds(bc_cs_central_settings.timeout)
                    binding.ReceiveTimeout = TimeSpan.FromMilliseconds(bc_cs_central_settings.timeout * 10)
                    binding.SendTimeout = TimeSpan.FromMilliseconds(bc_cs_central_settings.timeout)

                    binding.AllowCookies = False
                    binding.BypassProxyOnLocal = False
                    binding.HostNameComparisonMode = HostNameComparisonMode.StrongWildcard
                    binding.MaxBufferSize = 2147483647
                    binding.MaxReceivedMessageSize = 2147483647
                    binding.MaxBufferPoolSize = 2147483647
                    binding.MessageEncoding = WSMessageEncoding.Text
                    binding.TextEncoding = System.Text.Encoding.UTF8
                    binding.TransferMode = TransferMode.Buffered
                    binding.UseDefaultWebProxy = True

                    binding.ReaderQuotas.MaxDepth = 32
                    binding.ReaderQuotas.MaxStringContentLength = 1000000000
                    binding.ReaderQuotas.MaxArrayLength = 1000000000
                    binding.ReaderQuotas.MaxBytesPerRead = 4096
                    binding.ReaderQuotas.MaxNameTableCharCount = 16384

                    binding.Security.Mode = BasicHttpsSecurityMode.Transport


                    binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None
                    binding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None

                    binding.Security.Transport.Realm = ""


                    wcfclient = New ServiceReference1.BlueCurveWSClient(binding, endpointadd)

                End If
            Else
                REM binary over http
                Dim bbinding = New CustomBinding()

                bbinding.Name = "Custom_BCBinding"


                bbinding.CloseTimeout = TimeSpan.FromMilliseconds(bc_cs_central_settings.timeout)
                bbinding.OpenTimeout = TimeSpan.FromMilliseconds(bc_cs_central_settings.timeout)
                bbinding.ReceiveTimeout = TimeSpan.FromMilliseconds(bc_cs_central_settings.timeout * 10)
                bbinding.SendTimeout = TimeSpan.FromMilliseconds(bc_cs_central_settings.timeout)

                Dim ht As New HttpTransportBindingElement
                'ht.AuthenticationScheme = AuthenticationSchemes.Negotiate

                ht.MaxBufferPoolSize = 2147483647
                ht.MaxBufferSize = 2147483647
                ht.MaxReceivedMessageSize = 2147483647


                Dim bi As New BinaryMessageEncodingBindingElement
                bi.ReaderQuotas.MaxDepth = 128
                bi.ReaderQuotas.MaxStringContentLength = 2147483647
                bi.ReaderQuotas.MaxArrayLength = 2147483647
                bi.ReaderQuotas.MaxBytesPerRead = 2147483647
                bi.ReaderQuotas.MaxNameTableCharCount = 2147483647

                bbinding.Elements.Add(bi)
                bbinding.Elements.Add(ht)

                bbinding.Name = "Custom_BCBinding"

                wcfclient = New ServiceReference1.BlueCurveWSClient(bbinding, endpointadd)

            End If

            load_wcf_proxy = True
        Catch ex As Exception
            load_wcf_proxy = False
            Dim db_err As New bc_cs_error_log("bc_cs_soap_base_class", "load_wcf_proxy", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try
    End Function

    Public Overridable Function transmit_to_server_and_receive(ByRef oobject As Object, ByVal show_errors As Boolean) As Boolean
        Dim otrace As New bc_cs_activity_log("bc_cs_soap_base_class", "transmit_to_server_and_receive", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try

            If bc_cs_central_settings.use_rest_post = True Then
                transmit_to_server_and_receive = transmit_to_server_and_receive_rest(oobject, show_errors)
                Exit Function
            End If


            If IsNothing(wcfclient) Then
                If load_wcf_proxy() = False Then
                    transmit_to_server_and_receive = False
                    Exit Function
                End If
            End If



            REM compress object
            Dim compressed_body As String
            Dim bcs As New bc_cs_security
            Dim return_packet As New cs_object_packet
            Dim omessage As New bc_cs_message
            If bc_cs_central_settings.selected_autenticated_method = 3 Then
                bc_cs_central_settings.show_authentication_form = 3
            End If
            If bc_cs_central_settings.show_authentication_form = 2 And bc_cs_central_settings.override_username_password = False Then
                REM steve temp
                If bcs.connect_to_active_directory(certificate) = False Then
                    Dim ocomm As New bc_cs_activity_log("bc_cs_soap_base_class", "transmit_to_server_and_receive", bc_cs_activity_codes.COMMENTARY, "Network Error Cant Connect to Server using Active Directory.")
                    'omessage = New bc_cs_message("Blue Curve", "Network Error Cant Connect to Server using Active Directory.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Me.transmission_state = 2
                    transmit_to_server_and_receive = False
                    Exit Function
                End If
            End If
            populate_certificate()

            compressed_body = bcs.compress_xml(Me.write_data_to_string(Nothing, Me.packet_code), Nothing)
            write_compressed_string_to_packets(compressed_body, Nothing)


            If Me.transmit_packets(return_packet) = False And show_errors = True Then
                If return_packet.transmission_state = 1 Then
                    If bc_cs_central_settings.suppress_server_error = 0 Then
                        omessage = New bc_cs_message("Blue Curve", "Server Error Contact System Administrator", bc_cs_message.MESSAGE)
                    End If
                End If
                If return_packet.transmission_state = 2 Then
                    omessage = New bc_cs_message("Blue Curve", "Server Authentication Failed for User", bc_cs_message.MESSAGE)
                End If
                If return_packet.transmission_state = 3 Then
                    omessage = New bc_cs_message("Blue Curve", "Network Error Cant Connect to Server", bc_cs_message.MESSAGE)
                End If
            End If
            Me.transmission_state = return_packet.transmission_state
            transmit_to_server_and_receive = False

            'clear the password
            return_packet.certificate.password = ""
            return_packet.certificate.authentication_token = ""
            If return_packet.transmission_state = 0 Then
                If return_packet.no_send_back = False Then
                    REM extract object and decompress
                    oobject = read_data_from_string(bcs.decompress_xml(return_packet.received_object, certificate), certificate, Me.packet_code)
                    ' clear the password
                    oobject.certificate.password = ""
                    oobject.certificate.authentication_token = ""
                End If
                transmit_to_server_and_receive = True

            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_cs_soap_base_class", "transmit_to_server_and_receive", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            ' clear the password
            Me.certificate.password = ""
            Me.certificate.authentication_token = ""
            otrace = New bc_cs_activity_log("bc_cs_soap_base_class", "transmit_to_server_and_receive", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Function


    Protected Function transmit_to_server_and_receive_rest(ByRef oobject As Object, ByVal show_errors As Boolean) As Boolean
        Dim otrace As New bc_cs_activity_log("bc_cs_soap_base_class", "transmit_to_server_and_receive_rest", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try



            transmit_to_server_and_receive_rest = False
            Dim bcs As New bc_cs_security
            Dim return_packet As New cs_object_packet
            Dim omessage As New bc_cs_message
            If bc_cs_central_settings.selected_autenticated_method = 3 Then
                bc_cs_central_settings.show_authentication_form = 3
            End If
            If bc_cs_central_settings.show_authentication_form = 2 And bc_cs_central_settings.override_username_password = False Then
                REM steve temp
                If bcs.connect_to_active_directory(certificate) = False Then
                    Dim ocommm As New bc_cs_activity_log("bc_cs_soap_base_class", "transmit_to_server_and_receive", bc_cs_activity_codes.COMMENTARY, "Network Error Cant Connect to Server using Active Directory.")

                    'omessage = New bc_cs_message("Blue Curve", "Network Error Cant Connect to Server using Active Directory.", bc_cs_message.MESSAGE)
                    Me.transmission_state = 2
                    transmit_to_server_and_receive_rest = False
                    Exit Function
                End If
            End If
            populate_certificate()

            Dim send_packet As New cs_object_packet
            send_packet.use_rest_compression = bc_cs_central_settings.use_rest_compression

            send_packet.packet_code = packet_code
            send_packet.no_send_back = no_send_back
            If bc_cs_central_settings.use_rest_compression = True Then
                send_packet.sent_object = bcs.compress_xml(Me.write_data_to_string(Nothing, Me.packet_code), certificate)
            Else
                send_packet.sent_object = Me.write_data_to_string(Nothing, Me.packet_code)
            End If
            send_packet.certificate = certificate

            Dim JsonSerializer = New JavaScriptSerializer
            JsonSerializer.MaxJsonLength = Int32.MaxValue
            Dim json As String
            Try
                json = JsonSerializer.Serialize(send_packet)
            Catch ex As Exception
                Dim db_err As New bc_cs_error_log("bc_cs_soap_base_class", "transmit_to_server_and_receive_rest json serialisation failed", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
                Exit Function
            End Try
            json = "{ ""cs_object_packet"": " + json + "}"

            Dim ocomm As New bc_cs_activity_log("bc_cs_soap_base_class", "transmit_to_server_and_receive_rest", bc_cs_activity_codes.COMMENTARY, "Json size: " + CStr(json.Length), certificate)


            Dim rp As New bc_cs_ns_json_post(bc_cs_central_settings.soap_server + "Rest_generic_object_tranmission", json)

            rp.send(certificate)
            If rp.err_text = "" Then

                rp.response_text = Left(rp.response_text, Len(rp.response_text) - 1)
                rp.response_text = Right(rp.response_text, Len(rp.response_text) - 5)

                Dim ms As New MemoryStream(Encoding.Unicode.GetBytes(rp.response_text))
                Dim serializer As New System.Runtime.Serialization.Json.DataContractJsonSerializer(return_packet.[GetType]())
                return_packet = DirectCast(serializer.ReadObject(ms), cs_object_packet)
                ms.Close()
                ms.Dispose()
                If return_packet.transmission_state = 1 Then

                    For i = 0 To return_packet.server_errors.Count - 1
                        Dim oxcomm As New bc_cs_activity_log("bc_cs_soap_base_class", "transmit_to_server_and_receive_rest", bc_cs_activity_codes.COMMENTARY, return_packet.server_errors(i))
                    Next

                    If bc_cs_central_settings.suppress_server_error = 0 And show_errors = True Then
                        omessage = New bc_cs_message("Blue Curve", "Server Error Contact System Administrator", bc_cs_message.MESSAGE)
                    Else
                        transmission_state = 1
                    End If
                    Exit Function
                End If
                If return_packet.transmission_state = 2 And show_errors = True Then
                    omessage = New bc_cs_message("Blue Curve", "Server Authentication Failed for User", bc_cs_message.MESSAGE)
                    Exit Function
                Else
                    transmission_state = 2
                End If

                If return_packet.transmission_state = 0 Then

                    If return_packet.no_send_back = False Then
                        REM extract object and decompress
                        If bc_cs_central_settings.use_rest_compression = True Then
                            oobject = read_data_from_string(bcs.decompress_xml(return_packet.received_object, certificate), certificate, Me.packet_code)
                        Else
                            oobject = read_data_from_string(return_packet.received_object, certificate, Me.packet_code)
                        End If
                        ' clear the password
                        oobject.certificate.password = ""
                        oobject.certificate.authentication_token = ""
                    End If

                    'If bc_cs_central_settings.show_authentication_form = 2 Or bc_cs_central_settings.show_authentication_form = 3 Then
                    '    bc_cs_central_settings.ad_excel_token = return_packet.certificate.ad_excel_token
                    '    return_packet.certificate.ad_excel_token = ""
                    'End If
                    transmit_to_server_and_receive_rest = True
                    Exit Function
                End If



            Else
                return_packet.transmission_state = 3
                Me.transmission_state = 3
                If show_errors = True Then
                    Dim oerr As New bc_cs_error_log("bc_cs_soap_base_class", "transmit_to_server_and_receive_rest", bc_cs_error_codes.USER_DEFINED, "Error Sending Via Rest Post: " + rp.err_text)
                End If
                Exit Function
            End If



        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_cs_soap_base_class", "transmit_to_server_and_receive_rest", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            ' clear the password

            Me.certificate.password = ""
            Me.certificate.authentication_token = ""
            otrace = New bc_cs_activity_log("bc_cs_soap_base_class", "transmit_to_server_and_receive_rest", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Function


    Protected Overridable Sub populate_certificate()
        Dim otrace As New bc_cs_activity_log("bc_cs_soap_base_class", "populate_certificate", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try

            Me.certificate.authentication_mode = bc_cs_central_settings.show_authentication_form
            Me.certificate.override_username_password = bc_cs_central_settings.override_username_password

            If bc_cs_central_settings.override_username_password = True Then
                Me.certificate.name = bc_cs_central_settings.user_name
                Me.certificate.password = bc_cs_central_settings.user_password
                Me.certificate.user_id = "Authenticating: " + bc_cs_central_settings.user_name
            ElseIf Me.certificate.authentication_mode = 0 Then
                Me.certificate.os_name = bc_cs_central_settings.GetLoginName
                Me.certificate.user_id = "Authenticating: " + Me.certificate.os_name
            ElseIf Me.certificate.authentication_mode = 1 Then
                Me.certificate.name = bc_cs_central_settings.user_name
                Me.certificate.password = bc_cs_central_settings.user_password
                Me.certificate.user_id = "Authenticating: " + bc_cs_central_settings.user_name
            ElseIf Me.certificate.authentication_mode = 2 Then
                Me.certificate.os_name = bc_cs_central_settings.GetLoginName
                Me.certificate.user_id = "AD Authenticating: " + Me.certificate.os_name
            End If
            Try
                Dim nics() As NetworkInterface = NetworkInterface.GetAllNetworkInterfaces()

                For Each adapter In nics
                    Select Case adapter.NetworkInterfaceType
                        'Exclude Tunnels, Loopbacks and PPP
                        Case NetworkInterfaceType.Tunnel, NetworkInterfaceType.Loopback, NetworkInterfaceType.Ppp
                        Case Else
                            If Not adapter.GetPhysicalAddress.ToString = String.Empty And Not adapter.GetPhysicalAddress.ToString = "00000000000000E0" Then
                                Me.certificate.client_mac_address = adapter.GetPhysicalAddress.ToString
                                Exit For ' Got a mac so exit for
                            End If

                    End Select
                Next adapter
            Catch
                Me.certificate.client_mac_address = "cant find mac address"
            End Try

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_cs_soap_base_class", "populate_certificate", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_cs_soap_base_class", "populate_certificate", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub
    Private Sub write_compressed_string_to_packets(ByVal s As String, ByRef certificate As bc_cs_security.certificate)
        Dim otrace As New bc_cs_activity_log("bc_cs_soap_base_class", "write_object_to_packets", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            REM serialize to string max 2M limit per packet
            'Const packetsize As Long = 100000000
            Dim oxml As New bc_cs_data_services
            Dim bend As Boolean = False
            Dim start As Integer = 0
            Dim packet_count As Long = 0
            Dim str As String

            Dim packet As cs_object_packet
            Dim ocommentary As bc_cs_activity_log
            Dim fn As String

            Dim fstream As StreamWriter

            packets = New cs_objects_packets(1)

            If Right(bc_cs_central_settings.soap_server, 3) = "svc" Then
                ocommentary = New bc_cs_activity_log("bc_cs_soap_base_class", "write_object_to_packets WCF", bc_cs_activity_codes.COMMENTARY, "Compressed object total size: " + CStr(s.Length), certificate)
                REM chop up
                While bend = False
                    str = ""
                    Try
                        str = s.Substring(start, bc_cs_central_settings.webconfig_maxRequestLength)
                    Catch
                        str = s.Substring(start)
                    End Try
                    packet = New cs_object_packet(packet_count, Nothing, 0)
                    packet.sent_object = str
                    packet.certificate = certificate
                    packets.opackets.Add(packet)
                    packet_count = packet_count + 1
                    start = start + bc_cs_central_settings.webconfig_maxRequestLength
                    If str.Length < bc_cs_central_settings.webconfig_maxRequestLength Then
                        bend = True
                        Exit While
                    End If
                End While
                packets.number_of_packets = packet_count
                ocommentary = New bc_cs_activity_log("bc_cs_soap_base_class", "read_xml_from_file", bc_cs_activity_codes.COMMENTARY, "Creating: " + CStr(packet_count) + " Packets of size: " + CStr(bc_cs_central_settings.webconfig_maxRequestLength) + " each.")

            Else
                ocommentary = New bc_cs_activity_log("bc_cs_soap_base_class", "write_object_to_packets", bc_cs_activity_codes.COMMENTARY, "Compressed object total size: " + CStr(s.Length), certificate)
                REM chop up
                While bend = False
                    str = ""
                    Try
                        str = s.Substring(start, bc_cs_central_settings.webconfig_maxRequestLength)
                    Catch
                        str = s.Substring(start)
                    End Try
                    Dim kk As System.DateTime
                    kk = System.DateTime.Now
                    fn = "\packet" + CStr(kk.Minute) + CStr(kk.Second) + CStr(kk.Millisecond) + ".txt"
                    'REM write string to file
                    fstream = New StreamWriter(bc_cs_central_settings.get_user_dir + fn, FileMode.Create)
                    fstream.Write(str)
                    fstream.Close()
                    Dim fs As New bc_cs_file_transfer_services
                    Dim obyte As Byte() = Nothing
                    fs.write_document_to_bytestream(bc_cs_central_settings.get_user_dir + fn, obyte, Me.certificate)
                    packet = New cs_object_packet(packet_count, obyte, 0)
                    If Len(str) < bc_cs_central_settings.webconfig_maxRequestLength Then
                        bend = True
                    End If
                    fs.delete_file(bc_cs_central_settings.get_user_dir + fn)
                    packet.certificate = certificate
                    packets.opackets.Add(packet)
                    packet_count = packet_count + 1
                    start = start + bc_cs_central_settings.webconfig_maxRequestLength
                    If str.Length < bc_cs_central_settings.webconfig_maxRequestLength Then
                        bend = True
                        Exit While
                    End If
                End While
                packets.number_of_packets = packet_count
                ocommentary = New bc_cs_activity_log("bc_cs_soap_base_class", "read_xml_from_file", bc_cs_activity_codes.COMMENTARY, "Creating: " + CStr(packet_count) + " Packets of size: " + CStr(bc_cs_central_settings.webconfig_maxRequestLength) + " each.")

            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_cs_soap_base_class", "write_object_to_packets", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)

        Finally
            otrace = New bc_cs_activity_log("bc_cs_soap_base_class", "write_object_to_packets", bc_cs_activity_codes.TRACE_EXIT, "", certificate)

        End Try
    End Sub

    'Private Sub write_compressed_string_to_packets(ByVal s As String, ByRef certificate As bc_cs_security.certificate)
    '    Dim otrace As New bc_cs_activity_log("bc_cs_soap_base_class", "write_object_to_packets", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
    '    Try
    '        REM serialize to string max 2M limit per packet
    '        'Const packetsize As Long = 100000000
    '        Dim oxml As New bc_cs_data_services
    '        Dim bend As Boolean = False
    '        Dim start As Integer = 0
    '        Dim packet_count As Long = 0
    '        Dim str As String

    '        Dim packet As cs_object_packet
    '        Dim ocommentary As bc_cs_activity_log
    '        Dim fn As String

    '        Dim fstream As StreamWriter

    '        packets = New cs_objects_packets(1)



    '        ocommentary = New bc_cs_activity_log("bc_cs_soap_base_class", "write_object_to_packets", bc_cs_activity_codes.COMMENTARY, "Compressed object total size: " + CStr(s.Length), certificate)
    '        REM chop up
    '        While bend = False
    '            str = ""
    '            Try
    '                str = s.Substring(start, bc_cs_central_settings.webconfig_maxRequestLength)
    '            Catch
    '                str = s.Substring(start)
    '            End Try
    '            Dim kk As System.DateTime
    '            kk = System.DateTime.Now
    '            fn = "\packet" + CStr(kk.Minute) + CStr(kk.Second) + CStr(kk.Millisecond) + ".txt"
    '            'REM write string to file
    '            fstream = New StreamWriter(bc_cs_central_settings.get_user_dir + fn, FileMode.Create)
    '            fstream.Write(str)
    '            fstream.Close()
    '            Dim fs As New bc_cs_file_transfer_services
    '            Dim obyte As Byte() = Nothing
    '            fs.write_document_to_bytestream(bc_cs_central_settings.get_user_dir + fn, obyte, Me.certificate)
    '            packet = New cs_object_packet(packet_count, obyte, 0)
    '            If Len(str) < bc_cs_central_settings.webconfig_maxRequestLength Then
    '                bend = True
    '            End If
    '            fs.delete_file(bc_cs_central_settings.get_user_dir + fn)
    '            packet.certificate = certificate
    '            packets.opackets.Add(packet)
    '            packet_count = packet_count + 1
    '            start = start + bc_cs_central_settings.webconfig_maxRequestLength
    '            If str.Length < bc_cs_central_settings.webconfig_maxRequestLength Then
    '                bend = True
    '                Exit While
    '            End If
    '        End While
    '        packets.number_of_packets = packet_count
    '        ocommentary = New bc_cs_activity_log("bc_cs_soap_base_class", "read_xml_from_file", bc_cs_activity_codes.COMMENTARY, "Creating: " + CStr(packet_count) + " Packets of size: " + CStr(bc_cs_central_settings.webconfig_maxRequestLength) + " each.")


    '    Catch ex As Exception
    '        Dim db_err As New bc_cs_error_log("bc_cs_soap_base_class", "write_object_to_packets", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)

    '    Finally
    '        otrace = New bc_cs_activity_log("bc_cs_soap_base_class", "write_object_to_packets", bc_cs_activity_codes.TRACE_EXIT, "", certificate)

    '    End Try
    'End Sub
    Public Function read_data_from_string_show_no_error(ByVal strdata As String, ByRef certifiate As bc_cs_security.certificate, Optional ByVal packet_code As String = "") As Object
        Dim otrace As New bc_cs_activity_log("bc_cs_soap_base_class", "read_data_from_string_show_no_error", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Dim cbc_data_services As New bc_cs_data_services
            Dim cbs As New bc_cs_security
            read_data_from_string_show_no_error = cbc_data_services.soap_deserialize_string_to_object(strdata, Me.certificate, packet_code)
        Catch ex As Exception
            read_data_from_string_show_no_error = Nothing
        Finally
            otrace = New bc_cs_activity_log("bc_cs_soap_base_class", "read_data_from_string_show_no_error", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Function
    Public Function read_data_from_string(ByVal strdata As String, ByRef certifiate As bc_cs_security.certificate, Optional ByVal packet_code As String = "") As Object
        Dim otrace As New bc_cs_activity_log("bc_cs_soap_base_class", "read_data_from_string", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Dim cbc_data_services As New bc_cs_data_services
            Dim cbs As New bc_cs_security
            read_data_from_string = cbc_data_services.soap_deserialize_string_to_object(strdata, Me.certificate, packet_code)
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_cs_soap_base_class", "read_data_from_string", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            read_data_from_string = ""
        Finally
            otrace = New bc_cs_activity_log("bc_cs_soap_base_class", "read_data_from_string", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Function
    Public Function write_data_to_string(ByRef certifiate As bc_cs_security.certificate, Optional ByVal packet_code As String = "") As String
        Dim otrace As New bc_cs_activity_log("bc_cs_soap_base_class", "write_data_to_string", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Dim cbc_data_services As New bc_cs_data_services
            cbc_data_services.o = Me
            write_data_to_string = cbc_data_services.soap_serialize_object_to_string(Me.certificate, packet_code)

            'Dim JsonSerializer As New JavaScriptSerializer
            'JsonSerializer.MaxJsonLength = Int32.MaxValue
            'write_data_to_string = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(JsonSerializer.Serialize(Me)))
            'MsgBox(write_data_to_string)

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_cs_soap_base_class", "write_data_to_string", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            write_data_to_string = ""
        Finally
            otrace = New bc_cs_activity_log("bc_cs_soap_base_class", "write_data_to_string", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Function
    Private Function transmit_packets(ByRef opacket As cs_object_packet) As Boolean
        Dim otrace As New bc_cs_activity_log("bc_cs_soap_base_class", "transmit_packets", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Dim i As Integer
            Dim ocommentary As bc_cs_activity_log
            transmit_packets = False

            For i = 0 To packets.number_of_packets - 1
                If Me.async = True And i = packets.number_of_packets - 1 Then
                    ocommentary = New bc_cs_activity_log("bc_cs_soap_base_class", "transmit_packets_async", bc_cs_activity_codes.COMMENTARY, "Transmitting last packet asyncronously.")
                    REM only last packet can be sent asyncronously
                    If transmit_packet(i, opacket, True) = False Then
                        ocommentary = New bc_cs_activity_log("bc_cs_soap_base_class", "transmit_packets_async", bc_cs_activity_codes.COMMENTARY, "Cant Tranmit client error.")
                        Exit For
                    End If
                Else
                    If transmit_packet(i, opacket, False) = False Then
                        ocommentary = New bc_cs_activity_log("bc_cs_soap_base_class", "transmit_packets", bc_cs_activity_codes.COMMENTARY, "Cant Tranmit client error.")
                        Exit For
                    End If
                End If
                If opacket.transmission_state = 1 Then
                    ocommentary = New bc_cs_activity_log("bc_cs_soap_base_class", "transmit_packets", bc_cs_activity_codes.COMMENTARY, "Packet: " + CStr(i + 1) + " failed to transmit or general server error")
                    Exit For
                End If
                If opacket.transmission_state = 2 Then
                    ocommentary = New bc_cs_activity_log("bc_cs_soap_base_class", "transmit_packets", bc_cs_activity_codes.COMMENTARY, "Authentication failed.")
                    Exit For
                End If
                If opacket.transmission_state = 3 Then
                    ocommentary = New bc_cs_activity_log("bc_cs_soap_base_class", "transmit_packets", bc_cs_activity_codes.COMMENTARY, "Network Connection failed.")
                    Exit For
                End If
                If opacket.transmission_state = 20 Then
                    ocommentary = New bc_cs_activity_log("bc_cs_soap_base_class", "transmit_packets", bc_cs_activity_codes.COMMENTARY, "Failed to Call Asyncronous Web Service.")
                    Exit For
                End If

            Next
            For i = 0 To opacket.server_errors.Count - 1
                ocommentary = New bc_cs_activity_log("bc_cs_soap_base_class", "transmit_packets", bc_cs_activity_codes.COMMENTARY, opacket.server_errors(i))
            Next
            If opacket.transmission_state = 0 Then
                transmit_packets = True
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_cs_soap_base_class", "transmit_packets", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_cs_soap_base_class", "transmit_packets", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Function
    'Private Function web_service_transmit_compressed_packet(ByVal s As String) As String
    '    REM if this error create a new return packet and flag network failure
    '    Dim otrace As New bc_cs_activity_log("bc_cs_soap_base_class", "web_service_transmit_compressed_packet", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
    '    Dim ocommentary As bc_cs_activity_log

    '    Try
    '        REM web service will eventually go here
    '        'Dim bc_cs_accountinfoheader As New localhost.bc_cs_accountinfoheader

    '        web_service_transmit_compressed_packet = ""

    '        webservice = New BlueCurveWebService.BlueCurveWS
    '        webservice.Url = bc_cs_central_settings.soap_server

    '        set_proxy_settings()

    '        REM call object model specific web service
    '        If IsNumeric(bc_cs_central_settings.timeout) Then
    '            webservice.Timeout = CLng(bc_cs_central_settings.timeout)
    '            ocommentary = New bc_cs_activity_log("bc_cs_web services", "call_web_service", bc_cs_activity_codes.COMMENTARY, "Setting Web Service Timeout to: " + bc_cs_central_settings.timeout)
    '        End If
    '        Dim retry As Integer
    '        Dim transmission_error As Boolean
    '        Dim i As Integer
    '        Dim err_txt As String = ""
    '        retry = bc_cs_central_settings.retry
    '        transmission_error = False
    '        ocommentary = New bc_cs_activity_log("bc_cs_web services", "call_web_service", bc_cs_activity_codes.COMMENTARY, "Retry set to: " + CStr(retry))
    '        For i = 0 To retry - 1
    '            Try
    '                web_service_transmit_compressed_packet = webservice.generic_object_transmission(s)
    '                Exit For
    '            Catch ex As Exception
    '                ocommentary = New bc_cs_activity_log("bc_cs_web services", "call_web_service", bc_cs_activity_codes.COMMENTARY, "Retrying connection:" + CStr(i + 1) + " Of " + CStr(retry))
    '                If i = retry - 1 Then
    '                    err_txt = ex.Message
    '                    ocommentary = New bc_cs_activity_log("bc_cs_web services", "call_web_service", bc_cs_activity_codes.COMMENTARY, "Maximun Retries exceeded")
    '                    transmission_error = True
    '                End If
    '            End Try
    '        Next

    '        If transmission_error = True Then
    '            Dim npacket As New cs_object_packet
    '            If err_txt <> "The underlying connection was closed: The connection was closed unexpectedly." Then
    '                npacket.transmission_state = 3
    '                npacket.server_errors.Add("Network Error: " + err_txt)
    '            Else
    '                ocommentary = New bc_cs_activity_log("bc_cs_web services", "call_web_service", bc_cs_activity_codes.COMMENTARY, "The underlying connection was closed: The connection was closed unexpectedly.")
    '            End If
    '            Dim ixml As New bc_cs_data_services
    '            npacket.no_send_back = True
    '            ixml.o = npacket
    '            web_service_transmit_compressed_packet = ixml.soap_serialize_object_to_string(Nothing)

    '        End If
    '    Finally
    '        otrace = New bc_cs_activity_log("bc_cs_soap_base_class", "web_service_transmit_compressed_packet", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
    '    End Try
    'End Function
    Private Function wcf_web_service_transmit_compressed_packet(tpacket As ServiceReference1.cs_object_packet) As ServiceReference1.cs_object_packet
        REM if this error create a new return packet and flag network failure
        Dim otrace As New bc_cs_activity_log("bc_cs_soap_base_class", "wcf_web_service_transmit_compressed_packet", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Dim ocommentary As bc_cs_activity_log
        wcf_web_service_transmit_compressed_packet = Nothing
        Try
            Dim retry As Integer
            Dim transmission_error As Boolean
            Dim i As Integer
            Dim err_txt As String = ""
            retry = bc_cs_central_settings.retry
            transmission_error = False
            ocommentary = New bc_cs_activity_log("bc_cs_web services", "call_web_service", bc_cs_activity_codes.COMMENTARY, "")

            Try
                wcf_web_service_transmit_compressed_packet = wcfclient.generic_object_transmission_wcf(tpacket)
            Catch ex As Exception
                err_txt = ex.Message
                otrace = New bc_cs_activity_log("bc_cs_soap_base_class", "wcf_web_service_transmit_compressed_packet", bc_cs_activity_codes.COMMENTARY, ex.Message, certificate)

                transmission_error = True
            End Try


            If transmission_error = True Then
                Dim npacket As New ServiceReference1.cs_object_packet
                If err_txt <> "The underlying connection was closed: The connection was closed unexpectedly." Then
                    npacket.transmission_state = 3
                    'npacket.server_errors.Add("Network Error: " + err_txt)
                Else
                    ocommentary = New bc_cs_activity_log("bc_cs_web services", "call_web_service", bc_cs_activity_codes.COMMENTARY, "The underlying connection was closed: The connection was closed unexpectedly.")
                End If
                Dim ixml As New bc_cs_data_services
                npacket.no_send_back = True

                wcf_web_service_transmit_compressed_packet = npacket

            End If
        Finally
            otrace = New bc_cs_activity_log("bc_cs_soap_base_class", "wcf_web_service_transmit_compressed_packet", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Function


    'Private Function web_service_transmit_compressed_packet_async(ByVal s As String) As String
    '    REM if this error create a new return packet and flag network failure
    '    Dim otrace As New bc_cs_activity_log("bc_cs_soap_base_class", "web_service_transmit_compressed_packet_async", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
    '    Dim ocommentary As bc_cs_activity_log

    '    Try
    '        REM web service will eventually go here
    '        'Dim bc_cs_accountinfoheader As New localhost.bc_cs_accountinfoheader

    '        web_service_transmit_compressed_packet_async = ""

    '        webservice = New BlueCurveWebService.BlueCurveWS
    '        webservice.Url = bc_cs_central_settings.soap_server

    '        set_proxy_settings()

    '        REM call object model specific web service
    '        If IsNumeric(bc_cs_central_settings.timeout) Then
    '            webservice.Timeout = CLng(bc_cs_central_settings.timeout)
    '            ocommentary = New bc_cs_activity_log("bc_cs_web services", "call_web_service_async", bc_cs_activity_codes.COMMENTARY, "Setting Web Service Timeout to: " + bc_cs_central_settings.timeout)
    '        End If
    '        Dim retry As Integer
    '        Dim transmission_error As Boolean
    '        Dim i As Integer
    '        Dim err_txt As String = ""
    '        retry = bc_cs_central_settings.retry
    '        transmission_error = False
    '        ocommentary = New bc_cs_activity_log("bc_cs_web services", "call_web_service_async", bc_cs_activity_codes.COMMENTARY, "Retry set to: " + CStr(retry))
    '        For i = 0 To retry - 1
    '            Try
    '                webservice.generic_object_transmissionAsync(s)
    '                Exit For
    '            Catch ex As Exception
    '                ocommentary = New bc_cs_activity_log("bc_cs_web services", "call_web_service_async", bc_cs_activity_codes.COMMENTARY, "Retrying connection:" + CStr(i + 1) + " Of " + CStr(retry))
    '                If i = retry - 1 Then
    '                    err_txt = ex.Message
    '                    ocommentary = New bc_cs_activity_log("bc_cs_web services", "call_web_service_async", bc_cs_activity_codes.COMMENTARY, "Maximun Retries exceeded")
    '                    transmission_error = True
    '                End If
    '            End Try
    '        Next
    '        Dim npacket As New cs_object_packet
    '        npacket.no_send_back = True
    '        npacket.transmission_state = 0
    '        If transmission_error = True Then
    '            If err_txt <> "The underlying connection was closed: The connection was closed unexpectedly." Then
    '                npacket.transmission_state = 3
    '                npacket.server_errors.Add("Network Error: " + err_txt)
    '            Else
    '                ocommentary = New bc_cs_activity_log("bc_cs_web services", "call_web_service", bc_cs_activity_codes.COMMENTARY, "The underlying connection was closed: The connection was closed unexpectedly.")
    '            End If
    '        End If
    '        Dim ixml As New bc_cs_data_services
    '        npacket.no_send_back = True
    '        ixml.o = npacket
    '        web_service_transmit_compressed_packet_async = ixml.soap_serialize_object_to_string(Nothing)

    '    Finally
    '        otrace = New bc_cs_activity_log("bc_cs_soap_base_class", "web_service_transmit_compressed_packet_async", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
    '    End Try
    'End Function




    'Private Function web_service_transmit_compressed_packet_async(ByVal s As String) As Boolean
    '    REM if this error create a new return packet and flag network failure
    '    Dim otrace As New bc_cs_activity_log("bc_cs_soap_base_class", "web_service_transmit_compressed_packet_async", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
    '    Dim ocommentary As bc_cs_activity_log

    '    Try
    '        web_service_transmit_compressed_packet_async = True
    '        webservice = New BlueCurveWebService.BlueCurveWS
    '        webservice.Url = bc_cs_central_settings.soap_server
    '        Dim err_txt As String
    '        set_proxy_settings()
    '        Dim transmission_error As Boolean = False
    '        REM call object model specific web service
    '        If IsNumeric(bc_cs_central_settings.timeout) Then
    '            webservice.Timeout = CLng(bc_cs_central_settings.timeout)
    '            ocommentary = New bc_cs_activity_log("bc_cs_web services", "web_service_transmit_compressed_packet_async", bc_cs_activity_codes.COMMENTARY, "Setting Web Service Timeout to: " + bc_cs_central_settings.timeout)
    '        End If

    '        Try
    '            webservice.generic_object_transmissionAsync(s)

    '        Catch ex As Exception
    '            err_txt = ex.Message
    '            transmission_error = True
    '        End Try

    '    Finally
    '        otrace = New bc_cs_activity_log("bc_cs_soap_base_class", "web_service_transmit_compressed_packet_async", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
    '    End Try
    'End Function
    'Private Function transmit_packet(ByVal packet_number As Long, ByRef opacket As cs_object_packet, ByVal async_mode As Boolean) As Boolean
    '    Dim otrace As New bc_cs_activity_log("bc_cs_soap_base_class", "transmit_packet", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
    '    Try

    '        Dim cbc_data_services As bc_cs_data_services
    '        Dim ocommentary As bc_cs_activity_log
    '        Dim s As String
    '        transmit_packet = True
    '        REM transmits single packet
    '        Dim tpacket As New cs_object_packet(packet_number, Me.packets.opackets(packet_number).packet, Me.packets.number_of_packets)
    '        tpacket.certificate = Me.certificate
    '        tpacket.packet_code = Me.packet_code
    '        tpacket.transmission_state = 1
    '        ocommentary = New bc_cs_activity_log("bc_cs_soap_base_class", "read_xml_from_file", bc_cs_activity_codes.COMMENTARY, "Transmitting packet: " + CStr(packet_number + 1) + " of: " + CStr(Me.packets.number_of_packets) + " ;packet code: " + Me.packet_code)
    '        REM serialize to string
    '        cbc_data_services = New bc_cs_data_services
    '        cbc_data_services.o = tpacket
    '        s = cbc_data_services.soap_serialize_object_to_string(Me.certificate)
    '        REM call web service to transmit
    '        If async_mode = True Then
    '            s = web_service_transmit_compressed_packet_async(s)
    '        Else
    '            s = web_service_transmit_compressed_packet(s)
    '        End If

    '        REM decompress response and return as an object packet
    '        REM deserialoze string back and read error code
    '        cbc_data_services = New bc_cs_data_services
    '        Dim bcs As New bc_cs_security
    '        Dim lpacket As New cs_object_packet
    '        lpacket = cbc_data_services.soap_deserialize_string_to_object(s, Me.certificate, Me.packet_code)
    '        opacket = lpacket

    '    Catch ex As Exception
    '        transmit_packet = False
    '        Dim db_err As New bc_cs_error_log("bc_cs_soap_base_class", "transmit_packet", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
    '    Finally
    '        otrace = New bc_cs_activity_log("bc_cs_soap_base_class", "transmit_packet", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
    '    End Try

    'End Function
    Private Function transmit_packet(ByVal packet_number As Long, ByRef opacket As cs_object_packet, ByVal async_mode As Boolean) As Boolean
        Dim otrace As New bc_cs_activity_log("bc_cs_soap_base_class", "transmit_packet", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try

            Dim ocommentary As bc_cs_activity_log

            transmit_packet = True
            REM transmits single packet
            Dim tpacket As New cs_object_packet(packet_number, Me.packets.opackets(packet_number).packet, Me.packets.number_of_packets)
            tpacket.certificate = Me.certificate
            tpacket.packet_code = Me.packet_code
            tpacket.transmission_state = 1


            ocommentary = New bc_cs_activity_log("bc_cs_soap_base_class", "read_xml_from_file", bc_cs_activity_codes.COMMENTARY, "Transmitting packet wcf: " + CStr(packet_number + 1) + " of: " + CStr(Me.packets.number_of_packets) + " ;packet code: " + Me.packet_code)

            Dim wcftpacket As New ServiceReference1.cs_object_packet
            wcftpacket.packet_number = packet_number
            wcftpacket.sent_object = Me.packets.opackets(packet_number).sent_object
            wcftpacket.number_of_packets = Me.packets.number_of_packets
            wcftpacket.certificate = New ServiceReference1.bc_cs_securitycertificate
            wcftpacket.certificate.os_name = certificate.os_name
            wcftpacket.certificate.name = certificate.name
            wcftpacket.certificate.password = certificate.password
            wcftpacket.certificate.authentication_mode = certificate.authentication_mode
            wcftpacket.certificate.client_mac_address = certificate.client_mac_address
            wcftpacket.certificate.authentication_token = certificate.authentication_token
            wcftpacket.certificate.override_username_password = certificate.override_username_password


            wcftpacket.packet_code = Me.packet_code
            wcftpacket.transmission_state = 1


            Dim rwcftpacket As New ServiceReference1.cs_object_packet

            rwcftpacket = wcf_web_service_transmit_compressed_packet(wcftpacket)

            opacket.transmission_state = rwcftpacket.transmission_state
            If opacket.transmission_state = 0 Then
                opacket.no_send_back = rwcftpacket.no_send_back
                If opacket.no_send_back = False Then
                    opacket.received_object = rwcftpacket.received_object
                End If

            End If
            If Not IsNothing(rwcftpacket) Then
                If Not IsNothing(rwcftpacket.server_errors) Then
                    For i = 0 To rwcftpacket.server_errors.Count - 1
                        opacket.server_errors.Add(rwcftpacket.server_errors(i))
                    Next
                End If
            End If






        Catch ex As Exception
            transmit_packet = False
            Dim db_err As New bc_cs_error_log("bc_cs_soap_base_class", "transmit_packet", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_cs_soap_base_class", "transmit_packet", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Function
    Public Overridable Sub process_object()
        Dim ocommentary As New bc_cs_activity_log("bc_cs_soap_base_class", "process_object", bc_cs_activity_codes.COMMENTARY, "Object has no process_object" + certificate.os_name, certificate)
    End Sub
    Protected Function authenticate_user() As Boolean
        Dim otrace As New bc_cs_activity_log("bc_cs_soap_base_class", "autheticate_user", bc_cs_activity_codes.TRACE_ENTRY, certificate.user_id, certificate)
        Dim bcs As New bc_cs_security
        Dim ocommentary As bc_cs_activity_log

        authenticate_user = True

        REM has username password override been set
        If certificate.override_username_password Then
            ocommentary = New bc_cs_activity_log("bc_cs_soap_base_class", "authenticate_user", bc_cs_activity_codes.COMMENTARY, "UserName Password Override Set", certificate)
            certificate.authentication_mode = 1

        Else
            REM if server is strict AD then invalidate client mode
            If bc_cs_central_settings.show_authentication_form = 2 And certificate.authentication_mode <> 2 Then
                authenticate_user = False
                ocommentary = New bc_cs_activity_log("bc_cs_soap_base_class", "authenticate_user", bc_cs_activity_codes.COMMENTARY, "Authentication failed for user:" + certificate.os_name + " Server is set to AD mode but Client isnt. <prompt>2</prompt>", certificate)
                REM make a random id
                certificate.user_id = "authentication_failed"
                Exit Function
            ElseIf bc_cs_central_settings.show_authentication_form <> 2 And certificate.authentication_mode = 2 Then
                authenticate_user = False
                ocommentary = New bc_cs_activity_log("bc_cs_soap_base_class", "authenticate_user", bc_cs_activity_codes.COMMENTARY, "Authentication failed for user:" + certificate.os_name + " Client is set to AD mode but Server isnt. <prompt>2</prompt>", certificate)
                REM make a random id
                certificate.user_id = "authentication_failed"
                Exit Function
            End If
        End If


        If bcs.authenticate_user(certificate.authentication_mode, certificate.os_name, certificate.name, certificate.password, certificate) = "0" Then
            authenticate_user = False
            ocommentary = New bc_cs_activity_log("bc_cs_soap_base_class", "authenticate_user", bc_cs_activity_codes.COMMENTARY, "Authentication failed for user:" + certificate.os_name, certificate)
            REM make a random id
            certificate.user_id = "authentication_failed"
        Else
            ocommentary = New bc_cs_activity_log("bc_cs_soap_base_class", "authenticate_user", bc_cs_activity_codes.COMMENTARY, "Authentication passed for user:" + certificate.os_name + " user id: " + CStr(certificate.user_id), certificate)
            authenticate_user = True

        End If
        otrace = New bc_cs_activity_log("bc_cs_soap_base_class", "autheticate_user", bc_cs_activity_codes.TRACE_EXIT, "", certificate)

    End Function
    'Public Function receive_packet(ByVal s As String) As String
    '    certificate.user_id = "receiving..."
    '    Dim otrace As New bc_cs_activity_log("bc_cs_soap_base_class", "receive_packet", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
    '    Dim return_packet As New cs_object_packet
    '    Dim new_object As Object

    '    Try
    '        REM deserialize packet
    '        Dim tpacket As New cs_object_packet
    '        Dim ocommentary As bc_cs_activity_log
    '        Dim i As Integer
    '        Dim complete_string As String = ""
    '        Dim id As String
    '        Dim cbc_data_services As New bc_cs_data_services
    '        REM authenticate here
    '        tpacket = cbc_data_services.soap_deserialize_string_to_object(s, certificate, Me.packet_code)
    '        tpacket.certificate.error_state = False
    '        Me.certificate = tpacket.certificate








    '        Me.certificate.server_errors.Clear()
    '        If authenticate_user() = True Then
    '            id = tpacket.packet_code + "_" + CStr(certificate.user_id)
    '            Dim fs As New bc_cs_file_transfer_services
    '            fs.write_bytestream_to_document("c:\bluecurve\packet" + id + "_" + CStr(tpacket.packet_number + 1) + ".txt", tpacket.packet, Me.certificate)
    '            ocommentary = New bc_cs_activity_log("bc_cs_om_base_class", "receive_packet", bc_cs_activity_codes.COMMENTARY, "Receiving packet: " + CStr(tpacket.packet_number + 1) + " of: " + CStr(tpacket.number_of_packets) + " Logged on user_id:" + certificate.user_id + "filename: packet" + id, certificate)
    '            REM check if last packet if so  rebuild object from packets
    '            If tpacket.packet_number + 1 = tpacket.number_of_packets Then
    '                REM AD FIX oct 2015
    '                If certificate.authentication_mode = 2 Then
    '                    Dim gbc_db As New bc_cs_db_services
    '                    Dim sql As String
    '                    sql = "delete from bc_core_active_directory where token='" + certificate.authentication_token.ToString + "'"
    '                    gbc_db.executesql(sql, certificate)
    '                End If
    '                REM ===
    '                ocommentary = New bc_cs_activity_log("bc_cs_om_base_class", "receive_packet", bc_cs_activity_codes.COMMENTARY, "All Packets received attempting to reconstruct object filename: packet" + id, certificate)
    '                REM reconstitute

    '                For i = 1 To tpacket.number_of_packets
    '                    Dim sfs As New StreamReader("c:\bluecurve\packet" + id + "_" + CStr(i) + ".txt")
    '                    complete_string = complete_string + sfs.ReadToEnd()
    '                    sfs.Close()
    '                Next
    '                REM house keep files

    '                For i = 1 To tpacket.number_of_packets
    '                    fs.delete_file("c:\bluecurve\packet" + id + "_" + CStr(i) + ".txt")
    '                Next

    '                REM uncompress input and derialize into correct polymorphic object
    '                Dim ocomp As New bc_cs_security
    '                new_object = read_data_from_string(ocomp.decompress_xml(complete_string, Me.certificate), Me.certificate, Me.packet_code)
    '                new_object.certificate = Me.certificate

    '                If certificate.error_state = True Then
    '                    return_packet.transmission_state = 1
    '                Else
    '                    If bc_cs_central_settings.log_user_session = True Then
    '                        Dim bcs As New bc_cs_security
    '                        bcs.log_user_session("", certificate)
    '                    End If
    '                    new_object.process_object()
    '                    If bc_cs_central_settings.show_authentication_form = 2 Then
    '                        return_packet.certificate.ad_excel_token = Me.certificate.ad_excel_token
    '                    End If
    '                    REM call polymorphic function to start object specific processing
    '                    If new_object.no_send_back = False Then
    '                        return_packet.no_send_back = False
    '                        If new_object.certificate.error_state = True Then
    '                            return_packet.transmission_state = 1
    '                        End If
    '                        Dim bcs As New bc_cs_security
    '                        return_packet.received_object = bcs.compress_xml(new_object.write_data_to_string(Me.certificate, Me.packet_code), Me.certificate)
    '                        If certificate.error_state = True Then
    '                            return_packet.transmission_state = 1
    '                        End If
    '                    Else
    '                        If certificate.error_state = True Then
    '                            return_packet.transmission_state = 1
    '                        End If
    '                        return_packet.no_send_back = True
    '                    End If
    '                End If
    '            End If
    '        Else
    '            REM authentication error
    '            return_packet.transmission_state = 2

    '        End If
    '        REM serialize to string
    '        return_packet.server_errors = certificate.server_errors

    '        cbc_data_services = New bc_cs_data_services
    '        cbc_data_services.o = return_packet
    '        receive_packet = cbc_data_services.soap_serialize_object_to_string(Me.certificate)

    '    Catch ex As Exception
    '        Dim db_err As New bc_cs_error_log("bc_cs_soap_base_class", "receive_packet", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
    '        return_packet.transmission_state = 1
    '        receive_packet = ""
    '    Finally
    '        otrace = New bc_cs_activity_log("bc_cs_soap_base_class", "receive_packet", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
    '    End Try

    'End Function
    'Private Sub set_proxy_settings()

    '    Dim otrace As New bc_cs_activity_log("bc_cs_om_base_class", "set_proxy_services", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

    '    Try
    '        Select Case bc_cs_central_settings.proxy
    '            Case "IE", ""
    '                Try
    '                    Dim jj As System.Net.IWebProxy
    '                    jj = WebRequest.DefaultWebProxy
    '                    Dim ocommentary As New bc_cs_activity_log("bc_cs_xml_services", "set_proxy_services", bc_cs_activity_codes.COMMENTARY, "IE using Proxy address: " + jj.GetProxy(New Uri(bc_cs_central_settings.soap_server)).ToString, certificate)
    '                    If bc_cs_central_settings.authenticate Then
    '                        jj.Credentials = CredentialCache.DefaultCredentials
    '                    End If
    '                    webservice.Proxy = jj
    '                Catch ex As Exception
    '                    Dim ocommentary As New bc_cs_activity_log("bc_cs_xml_services", "set_proxy_services", bc_cs_activity_codes.COMMENTARY, "IE settings not using Proxy: " + ex.Message)
    '                End Try
    '            Case "AUTOPROXY" 'SUPPORT REMOVED
    '                Dim aProxy As Object = Nothing
    '                'Dim aProxy As New WebProxy(bc_cs_central_settings.soap_server)
    '                Dim proxyServerAddresses() As String
    '                Dim proxyServerAddress As String
    '                Dim jj As New System.Net.WebProxy

    '                proxyServerAddress = aProxy.Address.ToString
    '                proxyServerAddresses = Split(proxyServerAddress, ";")

    '                Dim ocommentary As New bc_cs_activity_log("bc_cs_xml_services", "testsoapserviceconnection", bc_cs_activity_codes.COMMENTARY, "GetProxyForURL: " + proxyServerAddress, certificate)

    '                Dim uri As System.Uri
    '                Dim i As Integer
    '                For i = 0 To UBound(proxyServerAddresses)
    '                    Try
    '                        If Left(proxyServerAddresses(i), 7) = "http://" Then
    '                            uri = New System.Uri(proxyServerAddresses(i))
    '                        Else
    '                            uri = New System.Uri("http://" & proxyServerAddresses(i))
    '                        End If
    '                        jj.Address = uri
    '                        ocommentary = New bc_cs_activity_log("bc_cs_xml_services", "testsoapserviceconnection", bc_cs_activity_codes.COMMENTARY, "Using Proxy address: " + CStr(jj.Address.AbsoluteUri), certificate)
    '                        If bc_cs_central_settings.authenticate Then
    '                            jj.Credentials = CredentialCache.DefaultCredentials
    '                        End If
    '                        webservice.Proxy = jj
    '                        'webservice.CheckConnection()
    '                        Exit For
    '                    Catch ex As Exception
    '                        If i = UBound(proxyServerAddresses) Then
    '                            Throw New Exception(ex.Message, ex.InnerException)
    '                        End If
    '                    End Try
    '                Next i
    '            Case "AUTOPROXYPAC" 'SUPPORT REMOVED
    '                Dim aProxy As Object = Nothing
    '                'Dim aProxy As New WebProxy(bc_cs_central_settings.soap_server) ' , bc_cs_central_settings.pac_file)
    '                Dim proxyServerAddresses() As String
    '                Dim proxyServerAddress As String
    '                Dim jj As New System.Net.WebProxy

    '                proxyServerAddress = aProxy.Address.ToString
    '                proxyServerAddresses = Split(proxyServerAddress, ";")

    '                Dim ocommentary As New bc_cs_activity_log("bc_cs_xml_services", "testsoapserviceconnection", bc_cs_activity_codes.COMMENTARY, "GetProxyForURL: " + proxyServerAddress, certificate)

    '                Dim uri As System.Uri
    '                Dim i As Integer
    '                For i = 0 To UBound(proxyServerAddresses)
    '                    Try
    '                        If Left(proxyServerAddresses(i), 7) = "http://" Then
    '                            uri = New System.Uri(proxyServerAddresses(i))
    '                        Else
    '                            uri = New System.Uri("http://" & proxyServerAddresses(i))
    '                        End If
    '                        jj.Address = uri
    '                        ocommentary = New bc_cs_activity_log("bc_cs_xml_services", "testsoapserviceconnection", bc_cs_activity_codes.COMMENTARY, "Using Proxy address: " + CStr(jj.Address.AbsoluteUri), certificate)
    '                        If bc_cs_central_settings.authenticate Then
    '                            jj.Credentials = CredentialCache.DefaultCredentials
    '                        End If
    '                        webservice.Proxy = jj
    '                        'webservice.CheckConnection()
    '                        Exit For
    '                    Catch ex As Exception
    '                        If i = UBound(proxyServerAddresses) Then
    '                            Throw New Exception(ex.Message, ex.InnerException)
    '                        End If
    '                    End Try
    '                Next i
    '            Case Else
    '                Dim ocommentary As New bc_cs_activity_log("bc_cs_xml_services", "set_proxy_services", bc_cs_activity_codes.COMMENTARY, "Using Config File Proxy address: " + CStr(bc_cs_central_settings.proxy))
    '                Dim jj As New System.Net.WebProxy

    '                Dim uri As New System.Uri(bc_cs_central_settings.proxy)

    '                jj.Address = uri
    '                If bc_cs_central_settings.authenticate Then
    '                    jj.Credentials = CredentialCache.DefaultCredentials
    '                End If
    '                webservice.Proxy = jj
    '        End Select

    '    Catch ex As Exception
    '        Dim db_err As New bc_cs_error_log("bc_cs_om_base_class", "set_proxy_services", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
    '    Finally
    '        otrace = New bc_cs_activity_log("bc_cs_om_base_class", "set_proxy_services", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
    '    End Try
    'End Sub
    Public Function write_data_to_file(ByVal strfilename As String) As Integer
        Dim otrace As New bc_cs_activity_log("bc_cs_soap_base_class", "write_data_to_file", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Dim cbc_data_services As New bc_cs_data_services
        Dim ocommentary As New bc_cs_activity_log("bc_cs_soap_base_class", "write_data_to_file", bc_cs_activity_codes.COMMENTARY, "Attempting to write XMl file:" + strfilename)
        cbc_data_services.o = Me
        cbc_data_services.soap_serialize_object_to_file(strfilename, Me.certificate)

        otrace = New bc_cs_activity_log("bc_cs_soap_base_class", "write_data_to_file", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
    End Function
    REM read object from XML file
    Public Function read_data_from_file(ByVal strfilename As String, Optional ByRef certificate As bc_cs_security.certificate = Nothing, Optional ByVal show_error As Boolean = True) As Object
        Dim otrace As New bc_cs_activity_log("bc_cs_soap_base_class", "read_data_from_file", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Dim cbc_data_services As New bc_cs_data_services
        Dim ocommentary As New bc_cs_activity_log("bc_cs_soap_base_class", "read_data_from_file", bc_cs_activity_codes.COMMENTARY, "Attempting to read XMl file:" + strfilename, certificate)

        read_data_from_file = cbc_data_services.soap_deserialize_file_to_object(strfilename, Me.certificate, show_error)

        otrace = New bc_cs_activity_log("bc_cs_soap_base_class", "read_data_from_file", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
    End Function
    Public Function check_data_file_exists(ByVal strfilename As String) As Boolean
        Dim otrace As New bc_cs_activity_log("bc_cs_om_base_class", "check_data_file_exists", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Dim cbc_data_services As New bc_cs_data_services

        check_data_file_exists = False
        check_data_file_exists = cbc_data_services.soap_check_deserialize_file_to_object(strfilename, Me.certificate)

        otrace = New bc_cs_activity_log("bc_cs_om_base_class", "check_data_file_exists", bc_cs_activity_codes.TRACE_EXIT, "", certificate)

    End Function

    Private Class cs_objects_packets
        Public number_of_packets As Integer
        Public opackets As New ArrayList
        Public Sub New(ByVal number_of_packets As Integer)
            Me.number_of_packets = number_of_packets
        End Sub
    End Class
    <Serializable()> Public Class cs_object_packet
        Public packet_number As Integer
        Public packet_code As String
        Public number_of_packets As Integer
        Public packet As Byte()
        Public transmission_state As Integer = 0
        Public server_errors As New ArrayList
        Public sent_object As String
        Public received_object As String
        Public no_send_back As Boolean
        Public certificate As New bc_cs_security.certificate
        Public use_rest_compression As Boolean = False
        Public Sub New(ByVal packet_number As Integer, ByVal packet As Byte(), ByVal number_of_packets As Integer)
            Me.packet_number = packet_number
            Me.packet = packet
            Me.number_of_packets = number_of_packets
        End Sub
        Public Sub New()

        End Sub
    End Class

    Public Overridable Function write_data_to_xml(ByRef certifiate As bc_cs_security.certificate) As String
        Dim otrace As New bc_cs_activity_log("bc_om_soap_base_class", "write_data_to_xml", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Dim cbc_data_services As New bc_cs_data_services
        Dim ocommentary As New bc_cs_activity_log("bc_om_soap_base_class", "write_data_to_xml", bc_cs_activity_codes.COMMENTARY, "Attempting to write XML string")
        cbc_data_services.o = Me
        write_data_to_xml = cbc_data_services.soap_serialize_object_to_xml(Me.certificate)

        otrace = New bc_cs_activity_log("bc_om_soap_base_class", "write_data_to_xml", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
    End Function

End Class


