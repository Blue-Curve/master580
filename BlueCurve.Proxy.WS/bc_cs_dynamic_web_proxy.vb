'Imports System
'Imports System.Collections.Generic
'Imports System.Text
'Imports System.Reflection
'Imports System.CodeDom
'Imports System.CodeDom.Compiler
'Imports System.Security.Permissions
'Imports System.Web.Services.Description
'Public Class bc_cs_dynamic_web_proxy
'    Public _url As String
'    Public _servicename As String
'    Public _err_text As String
'    Private wsvcClass As Object
'    Public Shared in_progress As Boolean = False

'    Public Function load_proxy(Optional ByVal from_external As Boolean = False, Optional ByRef ext_wsvcClass As Object = Nothing) As Boolean
'        Try
'            Dim i As Integer
'            load_proxy = True
'            REM check if proxy is loaded
'            For i = 0 To bc_compiled_proxies.proxies.Count - 1
'                If bc_compiled_proxies.proxies(i).url = _url And bc_compiled_proxies.proxies(i).service_name = _servicename Then
'                    wsvcClass = bc_compiled_proxies.proxies(i).wsvcClass
'                    Exit Function
'                End If
'            Next

'            Dim webServiceAsmxUrl As String
'            webServiceAsmxUrl = _url
'            Dim client As New System.Net.WebClient()
'            'Connect To the web service 
'            Dim stream As System.IO.Stream
'            stream = client.OpenRead(webServiceAsmxUrl + "?wsdl")

'            ' Now read the WSDL file describing a service. 
'            Dim description As ServiceDescription
'            description = ServiceDescription.Read(stream)



'            '///// LOAD THE DOM ///////// 
'            ' Initialize a service description importer. 
'            Dim importer As New ServiceDescriptionImporter()
'            importer.ProtocolName = "SOAP" ' // Use SOAP 1.2. 
'            importer.AddServiceDescription(description, "", "")

'            '// Generate a proxy client. 
'            importer.Style = ServiceDescriptionImportStyle.Client

'            '// Generate properties to represent primitive values. 
'            importer.CodeGenerationOptions = System.Xml.Serialization.CodeGenerationOptions.GenerateProperties

'            '// Initialize a Code-DOM tree into which we will import the service. 
'            Dim nmspace As New CodeNamespace()
'            Dim unit1 As New CodeCompileUnit()
'            unit1.Namespaces.Add(nmspace)

'            'Import the service into the Code-DOM tree. This creates proxy code that uses the service. 
'            Dim warning As New ServiceDescriptionImportWarnings
'            warning = importer.Import(nmspace, unit1)



'            If (warning = 0) Then ' If zero then we are good to go 
'                '// Generate the proxy code 
'                Dim provider1 As CodeDomProvider
'                provider1 = CodeDomProvider.CreateProvider("CSHARP")
'                '// Compile the assembly proxy with the appropriate references 
'                Dim assemblyReferences(5) As String
'                assemblyReferences(0) = "System.dll"
'                assemblyReferences(1) = "System.Web.dll"
'                assemblyReferences(2) = "System.Web.Services.dll"
'                assemblyReferences(3) = "System.Xml.dll"
'                assemblyReferences(4) = "System.Data.dll"

'                Dim parms As CompilerParameters
'                parms = New CompilerParameters(assemblyReferences)

'                Dim results As CompilerResults
'                results = provider1.CompileAssemblyFromDom(parms, unit1)

'                '// Check For Errors 
'                'If (results.Errors.Count > 0) Then

'                '    For Each oops In results.Errors

'                '        Debug.WriteLine(oops.ErrorText)

'                '    Next

'                'End If
'                If from_external = False Then
'                    wsvcClass = results.CompiledAssembly.CreateInstance(_servicename)

'                    If IsNothing(wsvcClass) Then
'                        load_proxy = False
'                        _err_text = "couldnt compile proxy check service name: " + _servicename + " is correct"
'                        Exit Function
'                    End If
'                Else
'                    ext_wsvcClass = results.CompiledAssembly.CreateInstance(_servicename)
'                End If
'                REM add to cache
'                Dim oproxy As New bc_compiled_proxy
'                oproxy.url = _url
'                oproxy.service_name = _servicename
'                oproxy.wsvcClass = wsvcClass
'                bc_compiled_proxies.proxies.Add(oproxy)

'            End If
'        Catch ex As Exception
'            load_proxy = False
'            _err_text = ex.Message
'        End Try
'    End Function
'    Public Function call_web_method(ByVal method As String, ByVal args() As Object) As Object
'        Try
'            Me._err_text = ""
'            Dim mi As Object
'            REM keep it single concurrency as used shared memory
'            'While in_progress = True

'            'End While


'            in_progress = True
'            mi = wsvcClass.GetType().GetMethod(method)
'            call_web_method = mi.Invoke(wsvcClass, args)
'            in_progress = False
'        Catch ex As Exception
'            Me._err_text = ex.Message
'            call_web_method = Nothing
'        Finally
'            in_progress = False
'        End Try
'    End Function
'    Public Function call_web_method(ByVal method As String, ByVal args() As Object, ByVal ext_wsvcClass As Object) As Object
'        Try
'            Me._err_text = ""
'            Dim mi As Object

'            in_progress = True
'            mi = ext_wsvcClass.GetType().GetMethod(method)
'            call_web_method = mi.Invoke(ext_wsvcClass, args)
'            in_progress = False
'        Catch ex As Exception
'            Me._err_text = ex.Message
'            call_web_method = Nothing
'        Finally
'            in_progress = False
'        End Try
'    End Function
'    Private Class bc_compiled_proxies
'        Public Shared proxies As New ArrayList
'    End Class
'    Private Class bc_compiled_proxy
'        Public url As String
'        Public service_name As String
'        Public wsvcClass As Object
'    End Class
'End Class







