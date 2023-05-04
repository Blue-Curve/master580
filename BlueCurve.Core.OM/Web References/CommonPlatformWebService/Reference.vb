﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:2.0.50727.5420
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On

Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Xml.Serialization

'
'This source code was auto-generated by Microsoft.VSDesigner, Version 2.0.50727.5420.
'
Namespace CommonPlatformWebService
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.5420"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Web.Services.WebServiceBindingAttribute(Name:="CMDSoapSoapBinding", [Namespace]:="http://zoidberg:8080/BlueCurveRMS/services/CMDSoap")>  _
    Partial Public Class CMDSoapService
        Inherits System.Web.Services.Protocols.SoapHttpClientProtocol
        
        Private conflictsOperationCompleted As System.Threading.SendOrPostCallback
        
        Private disclosuresOperationCompleted As System.Threading.SendOrPostCallback
        
        Private pricechartOperationCompleted As System.Threading.SendOrPostCallback
        
        Private listDocumentsOperationCompleted As System.Threading.SendOrPostCallback
        
        Private useDefaultCredentialsSetExplicitly As Boolean
        
        '''<remarks/>
        Public Sub New()
            MyBase.New
            Me.Url = Global.BlueCurve.Core.OM.My.MySettings.Default.BlueCurve_Core_OM_CommonPlatformWebService_CMDSoapService
            If (Me.IsLocalFileSystemWebService(Me.Url) = true) Then
                Me.UseDefaultCredentials = true
                Me.useDefaultCredentialsSetExplicitly = false
            Else
                Me.useDefaultCredentialsSetExplicitly = true
            End If
        End Sub
        
        Public Shadows Property Url() As String
            Get
                Return MyBase.Url
            End Get
            Set
                If (((Me.IsLocalFileSystemWebService(MyBase.Url) = true)  _
                            AndAlso (Me.useDefaultCredentialsSetExplicitly = false))  _
                            AndAlso (Me.IsLocalFileSystemWebService(value) = false)) Then
                    MyBase.UseDefaultCredentials = false
                End If
                MyBase.Url = value
            End Set
        End Property
        
        Public Shadows Property UseDefaultCredentials() As Boolean
            Get
                Return MyBase.UseDefaultCredentials
            End Get
            Set
                MyBase.UseDefaultCredentials = value
                Me.useDefaultCredentialsSetExplicitly = true
            End Set
        End Property
        
        '''<remarks/>
        Public Event conflictsCompleted As conflictsCompletedEventHandler
        
        '''<remarks/>
        Public Event disclosuresCompleted As disclosuresCompletedEventHandler
        
        '''<remarks/>
        Public Event pricechartCompleted As pricechartCompletedEventHandler
        
        '''<remarks/>
        Public Event listDocumentsCompleted As listDocumentsCompletedEventHandler
        
        '''<remarks/>
        <System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace:="http://soap.rms.bluecurve.net", ResponseNamespace:="http://zoidberg:8080/BlueCurveRMS/services/CMDSoap")>  _
        Public Function conflicts(ByVal documentId As String) As <System.Xml.Serialization.SoapElementAttribute("conflictsReturn")> String
            Dim results() As Object = Me.Invoke("conflicts", New Object() {documentId})
            Return CType(results(0),String)
        End Function
        
        '''<remarks/>
        Public Overloads Sub conflictsAsync(ByVal documentId As String)
            Me.conflictsAsync(documentId, Nothing)
        End Sub
        
        '''<remarks/>
        Public Overloads Sub conflictsAsync(ByVal documentId As String, ByVal userState As Object)
            If (Me.conflictsOperationCompleted Is Nothing) Then
                Me.conflictsOperationCompleted = AddressOf Me.OnconflictsOperationCompleted
            End If
            Me.InvokeAsync("conflicts", New Object() {documentId}, Me.conflictsOperationCompleted, userState)
        End Sub
        
        Private Sub OnconflictsOperationCompleted(ByVal arg As Object)
            If (Not (Me.conflictsCompletedEvent) Is Nothing) Then
                Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg,System.Web.Services.Protocols.InvokeCompletedEventArgs)
                RaiseEvent conflictsCompleted(Me, New conflictsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
            End If
        End Sub
        
        '''<remarks/>
        <System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace:="http://soap.rms.bluecurve.net", ResponseNamespace:="http://zoidberg:8080/BlueCurveRMS/services/CMDSoap")>  _
        Public Function disclosures(ByVal documentId As String) As <System.Xml.Serialization.SoapElementAttribute("disclosuresReturn")> String
            Dim results() As Object = Me.Invoke("disclosures", New Object() {documentId})
            Return CType(results(0),String)
        End Function
        
        '''<remarks/>
        Public Overloads Sub disclosuresAsync(ByVal documentId As String)
            Me.disclosuresAsync(documentId, Nothing)
        End Sub
        
        '''<remarks/>
        Public Overloads Sub disclosuresAsync(ByVal documentId As String, ByVal userState As Object)
            If (Me.disclosuresOperationCompleted Is Nothing) Then
                Me.disclosuresOperationCompleted = AddressOf Me.OndisclosuresOperationCompleted
            End If
            Me.InvokeAsync("disclosures", New Object() {documentId}, Me.disclosuresOperationCompleted, userState)
        End Sub
        
        Private Sub OndisclosuresOperationCompleted(ByVal arg As Object)
            If (Not (Me.disclosuresCompletedEvent) Is Nothing) Then
                Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg,System.Web.Services.Protocols.InvokeCompletedEventArgs)
                RaiseEvent disclosuresCompleted(Me, New disclosuresCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
            End If
        End Sub
        
        '''<remarks/>
        <System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace:="http://soap.rms.bluecurve.net", ResponseNamespace:="http://zoidberg:8080/BlueCurveRMS/services/CMDSoap")>  _
        Public Function pricechart(ByVal documentId As String, ByVal stockId As String) As <System.Xml.Serialization.SoapElementAttribute("pricechartReturn")> String
            Dim results() As Object = Me.Invoke("pricechart", New Object() {documentId, stockId})
            Return CType(results(0),String)
        End Function
        
        '''<remarks/>
        Public Overloads Sub pricechartAsync(ByVal documentId As String, ByVal stockId As String)
            Me.pricechartAsync(documentId, stockId, Nothing)
        End Sub
        
        '''<remarks/>
        Public Overloads Sub pricechartAsync(ByVal documentId As String, ByVal stockId As String, ByVal userState As Object)
            If (Me.pricechartOperationCompleted Is Nothing) Then
                Me.pricechartOperationCompleted = AddressOf Me.OnpricechartOperationCompleted
            End If
            Me.InvokeAsync("pricechart", New Object() {documentId, stockId}, Me.pricechartOperationCompleted, userState)
        End Sub
        
        Private Sub OnpricechartOperationCompleted(ByVal arg As Object)
            If (Not (Me.pricechartCompletedEvent) Is Nothing) Then
                Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg,System.Web.Services.Protocols.InvokeCompletedEventArgs)
                RaiseEvent pricechartCompleted(Me, New pricechartCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
            End If
        End Sub
        
        '''<remarks/>
        <System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace:="http://soap.rms.bluecurve.net", ResponseNamespace:="http://zoidberg:8080/BlueCurveRMS/services/CMDSoap")>  _
        Public Function listDocuments() As <System.Xml.Serialization.SoapElementAttribute("listDocumentsReturn")> String
            Dim results() As Object = Me.Invoke("listDocuments", New Object(-1) {})
            Return CType(results(0),String)
        End Function
        
        '''<remarks/>
        Public Overloads Sub listDocumentsAsync()
            Me.listDocumentsAsync(Nothing)
        End Sub
        
        '''<remarks/>
        Public Overloads Sub listDocumentsAsync(ByVal userState As Object)
            If (Me.listDocumentsOperationCompleted Is Nothing) Then
                Me.listDocumentsOperationCompleted = AddressOf Me.OnlistDocumentsOperationCompleted
            End If
            Me.InvokeAsync("listDocuments", New Object(-1) {}, Me.listDocumentsOperationCompleted, userState)
        End Sub
        
        Private Sub OnlistDocumentsOperationCompleted(ByVal arg As Object)
            If (Not (Me.listDocumentsCompletedEvent) Is Nothing) Then
                Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg,System.Web.Services.Protocols.InvokeCompletedEventArgs)
                RaiseEvent listDocumentsCompleted(Me, New listDocumentsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
            End If
        End Sub
        
        '''<remarks/>
        Public Shadows Sub CancelAsync(ByVal userState As Object)
            MyBase.CancelAsync(userState)
        End Sub
        
        Private Function IsLocalFileSystemWebService(ByVal url As String) As Boolean
            If ((url Is Nothing)  _
                        OrElse (url Is String.Empty)) Then
                Return false
            End If
            Dim wsUri As System.Uri = New System.Uri(url)
            If ((wsUri.Port >= 1024)  _
                        AndAlso (String.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) = 0)) Then
                Return true
            End If
            Return false
        End Function
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.5420")>  _
    Public Delegate Sub conflictsCompletedEventHandler(ByVal sender As Object, ByVal e As conflictsCompletedEventArgs)
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.5420"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class conflictsCompletedEventArgs
        Inherits System.ComponentModel.AsyncCompletedEventArgs
        
        Private results() As Object
        
        Friend Sub New(ByVal results() As Object, ByVal exception As System.Exception, ByVal cancelled As Boolean, ByVal userState As Object)
            MyBase.New(exception, cancelled, userState)
            Me.results = results
        End Sub
        
        '''<remarks/>
        Public ReadOnly Property Result() As String
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(0),String)
            End Get
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.5420")>  _
    Public Delegate Sub disclosuresCompletedEventHandler(ByVal sender As Object, ByVal e As disclosuresCompletedEventArgs)
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.5420"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class disclosuresCompletedEventArgs
        Inherits System.ComponentModel.AsyncCompletedEventArgs
        
        Private results() As Object
        
        Friend Sub New(ByVal results() As Object, ByVal exception As System.Exception, ByVal cancelled As Boolean, ByVal userState As Object)
            MyBase.New(exception, cancelled, userState)
            Me.results = results
        End Sub
        
        '''<remarks/>
        Public ReadOnly Property Result() As String
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(0),String)
            End Get
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.5420")>  _
    Public Delegate Sub pricechartCompletedEventHandler(ByVal sender As Object, ByVal e As pricechartCompletedEventArgs)
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.5420"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class pricechartCompletedEventArgs
        Inherits System.ComponentModel.AsyncCompletedEventArgs
        
        Private results() As Object
        
        Friend Sub New(ByVal results() As Object, ByVal exception As System.Exception, ByVal cancelled As Boolean, ByVal userState As Object)
            MyBase.New(exception, cancelled, userState)
            Me.results = results
        End Sub
        
        '''<remarks/>
        Public ReadOnly Property Result() As String
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(0),String)
            End Get
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.5420")>  _
    Public Delegate Sub listDocumentsCompletedEventHandler(ByVal sender As Object, ByVal e As listDocumentsCompletedEventArgs)
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.5420"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class listDocumentsCompletedEventArgs
        Inherits System.ComponentModel.AsyncCompletedEventArgs
        
        Private results() As Object
        
        Friend Sub New(ByVal results() As Object, ByVal exception As System.Exception, ByVal cancelled As Boolean, ByVal userState As Object)
            MyBase.New(exception, cancelled, userState)
            Me.results = results
        End Sub
        
        '''<remarks/>
        Public ReadOnly Property Result() As String
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(0),String)
            End Get
        End Property
    End Class
End Namespace