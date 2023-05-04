Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Threading
Imports System.Net
Imports System.Text
Imports System.IO
Imports System.Security.Cryptography.X509Certificates

Public Class bc_cs_ns_json_post
    Dim _uri As String
    Dim _json_data As String
    Public err_text As String
    Public response_text As String

    Public Sub New(uri As String, json_data As String)
        _uri = uri
        _json_data = json_data
    End Sub
    Public Function send(certificate As bc_cs_security.certificate) As Boolean
        Try
            send = False
            Dim request As HttpWebRequest
            Dim response As HttpWebResponse = Nothing
            Dim reader As StreamReader
            Dim address As Uri
            Dim byteData() As Byte
            Dim postStream As Stream = Nothing

            address = New Uri(_uri)

            ' Create the web request  
            request = DirectCast(WebRequest.Create(address), HttpWebRequest)
            request.Timeout = bc_cs_central_settings.timeout

            request.AutomaticDecompression = DecompressionMethods.GZip
            request.SendChunked = True


            ' Set type to POST  
            request.Method = "POST"
            request.ContentType = "application/json"
            Try
                ' Create a byte array of the data we want to send  
                byteData = UTF8Encoding.UTF8.GetBytes(_json_data.ToString())
            Catch ex As Exception
                err_text = "json via rest post byte data creation: " + ex.Message
                Return False
            End Try
            ' Set the content length in the request headers  
            Try
                request.ContentLength = byteData.Length
            Catch ex As Exception
                err_text = "json via rest post byte data length: " + ex.Message
                Return False
            End Try

            ' Write data  
            Try
                postStream = request.GetRequestStream()
                postStream.Write(byteData, 0, byteData.Length)

            Catch ex As Exception
                err_text = "json via rest post failed1: " + ex.Message
                Return False
            Finally
                If Not postStream Is Nothing Then postStream.Close()
            End Try

            Try
                ' Get response  

                response = DirectCast(request.GetResponse(), HttpWebResponse)

                ' Get the response stream into a reader  
                reader = New StreamReader(response.GetResponseStream())


                response_text = reader.ReadToEnd
                Return True


            Catch ex As Exception
                'err_text = "json via rest post failed2: " + ex.Message + ":" + ex.InnerException.Message

                err_text = "json via rest post failed2: " + ex.Message
                Return False

            Finally
                If Not response Is Nothing Then response.Close()
            End Try
        Catch ex As Exception
            err_text = "json via rest post failed end: " + ex.Message
        End Try
    End Function
End Class

Public Class bc_cs_ns_soap_post
    Dim _uri As String
    Dim _soap_data As String
    Dim _password As String
    Dim _cert_location As String
    Dim _soapaction As String

    Public err_text As String
    Public response_text As String

    Public Sub New(uri As String, soap_data As String, cert_location As String, password As String, soapaction As String)
        _uri = uri
        _soap_data = soap_data
        _password = password
        _cert_location = cert_location
        _soapaction = soapaction
    End Sub
    Public Function send(certificate As bc_cs_security.certificate) As Boolean
        Try
            send = False
            Dim request As HttpWebRequest
            Dim response As HttpWebResponse = Nothing
            Dim reader As StreamReader
            Dim address As Uri
            Dim SoapByte() As Byte
            Dim postStream As Stream = Nothing
            Dim x509 As Object

            Try
                address = New Uri(_uri)

            Catch ex As Exception
                err_text = "Soap Uri Error: " + ex.Message + " " + _uri
                Return False
            End Try

            Try

                'Settings
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls Or SecurityProtocolType.Ssl3 Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls12
                ServicePointManager.Expect100Continue = True

                'Create the web request  
                SoapByte = System.Text.Encoding.UTF8.GetBytes(_soap_data)
                request = DirectCast(WebRequest.Create(address), HttpWebRequest)

                request.Headers.Add("SOAPAction", _soapaction)
                'request.Headers.Add("SOAPAction", "http://ipreo.com/ResearchImport/2015/07/IResearchImportService/ImportResearchRixml")

                request.Timeout = bc_cs_central_settings.timeout

                'set Cirtificate
                x509 = New X509Certificate2(_cert_location, _password)
                'x509 = New X509Certificate2("C:\bluecurve\workflow@ipreoresearch.com-Symantec-ipreo2018.pfx", "ipreoresearchcert!")
                request.ClientCertificates.Add(x509)

                request.ContentLength = _soap_data.Length
                request.ContentType = "text/xml;charset=utf-8"
                request.Accept = "gzip, deflate"
                request.Method = "POST"

            Catch ex As Exception
                err_text = "soap request error: " + ex.Message + "[" + _uri + "]"
                Return False
            End Try


            ' Write data  
            Try
                'postStream = request.GetRequestStream()
                'postStream.Write(SoapByte, 0, SoapByte.Length)

                Dim stmw As StreamWriter
                postStream = request.GetRequestStream()
                stmw = New StreamWriter(postStream)
                stmw.Write(_soap_data)
                stmw.Close()


            Catch ex As Exception
                err_text = "Soap post failed: " + ex.Message
                Return False
            Finally
                If Not postStream Is Nothing Then postStream.Close()
            End Try

            Try
                ' Get response  
                response = DirectCast(request.GetResponse(), HttpWebResponse)
                reader = New StreamReader(response.GetResponseStream())
                response_text = reader.ReadToEnd
                Return True

            Catch ex As Exception
                err_text = "Soap response failed: " + ex.Message
                Return False

            Finally
                If Not response Is Nothing Then response.Close()
            End Try
        Catch ex As Exception
            err_text = "Soap post failed end: " + ex.Message
        End Try
    End Function
End Class