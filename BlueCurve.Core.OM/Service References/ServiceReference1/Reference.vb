﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.34209
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On

Imports System
Imports System.Runtime.Serialization

Namespace ServiceReference1
    
    <System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),  _
     System.Runtime.Serialization.DataContractAttribute(Name:="bc_core_component", [Namespace]:="http://schemas.datacontract.org/2004/07/bc_core_components_svc"),  _
     System.SerializableAttribute()>  _
    Partial Public Class bc_core_component
        Inherits Object
        Implements System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
        
        <System.NonSerializedAttribute()>  _
        Private extensionDataField As System.Runtime.Serialization.ExtensionDataObject
        
        <System.Runtime.Serialization.OptionalFieldAttribute()>  _
        Private contributor_idField As Long
        
        <System.Runtime.Serialization.OptionalFieldAttribute()>  _
        Private data_at_dateField As Date
        
        <System.Runtime.Serialization.OptionalFieldAttribute()>  _
        Private entity_idField As Long
        
        <System.Runtime.Serialization.OptionalFieldAttribute()>  _
        Private language_idField As Long
        
        <System.Runtime.Serialization.OptionalFieldAttribute()>  _
        Private pub_type_idField As Long
        
        <System.Runtime.Serialization.OptionalFieldAttribute()>  _
        Private stage_idField As Long
        
        <System.Runtime.Serialization.OptionalFieldAttribute()>  _
        Private typeField As Integer
        
        <System.Runtime.Serialization.OptionalFieldAttribute()>  _
        Private user_idField As Long
        
        <Global.System.ComponentModel.BrowsableAttribute(false)>  _
        Public Property ExtensionData() As System.Runtime.Serialization.ExtensionDataObject Implements System.Runtime.Serialization.IExtensibleDataObject.ExtensionData
            Get
                Return Me.extensionDataField
            End Get
            Set
                Me.extensionDataField = value
            End Set
        End Property
        
        <System.Runtime.Serialization.DataMemberAttribute()>  _
        Public Property contributor_id() As Long
            Get
                Return Me.contributor_idField
            End Get
            Set
                If (Me.contributor_idField.Equals(value) <> true) Then
                    Me.contributor_idField = value
                    Me.RaisePropertyChanged("contributor_id")
                End If
            End Set
        End Property
        
        <System.Runtime.Serialization.DataMemberAttribute()>  _
        Public Property data_at_date() As Date
            Get
                Return Me.data_at_dateField
            End Get
            Set
                If (Me.data_at_dateField.Equals(value) <> true) Then
                    Me.data_at_dateField = value
                    Me.RaisePropertyChanged("data_at_date")
                End If
            End Set
        End Property
        
        <System.Runtime.Serialization.DataMemberAttribute()>  _
        Public Property entity_id() As Long
            Get
                Return Me.entity_idField
            End Get
            Set
                If (Me.entity_idField.Equals(value) <> true) Then
                    Me.entity_idField = value
                    Me.RaisePropertyChanged("entity_id")
                End If
            End Set
        End Property
        
        <System.Runtime.Serialization.DataMemberAttribute()>  _
        Public Property language_id() As Long
            Get
                Return Me.language_idField
            End Get
            Set
                If (Me.language_idField.Equals(value) <> true) Then
                    Me.language_idField = value
                    Me.RaisePropertyChanged("language_id")
                End If
            End Set
        End Property
        
        <System.Runtime.Serialization.DataMemberAttribute()>  _
        Public Property pub_type_id() As Long
            Get
                Return Me.pub_type_idField
            End Get
            Set
                If (Me.pub_type_idField.Equals(value) <> true) Then
                    Me.pub_type_idField = value
                    Me.RaisePropertyChanged("pub_type_id")
                End If
            End Set
        End Property
        
        <System.Runtime.Serialization.DataMemberAttribute()>  _
        Public Property stage_id() As Long
            Get
                Return Me.stage_idField
            End Get
            Set
                If (Me.stage_idField.Equals(value) <> true) Then
                    Me.stage_idField = value
                    Me.RaisePropertyChanged("stage_id")
                End If
            End Set
        End Property
        
        <System.Runtime.Serialization.DataMemberAttribute()>  _
        Public Property type() As Integer
            Get
                Return Me.typeField
            End Get
            Set
                If (Me.typeField.Equals(value) <> true) Then
                    Me.typeField = value
                    Me.RaisePropertyChanged("type")
                End If
            End Set
        End Property
        
        <System.Runtime.Serialization.DataMemberAttribute()>  _
        Public Property user_id() As Long
            Get
                Return Me.user_idField
            End Get
            Set
                If (Me.user_idField.Equals(value) <> true) Then
                    Me.user_idField = value
                    Me.RaisePropertyChanged("user_id")
                End If
            End Set
        End Property
        
        Public Event PropertyChanged As System.ComponentModel.PropertyChangedEventHandler Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
        
        Protected Sub RaisePropertyChanged(ByVal propertyName As String)
            Dim propertyChanged As System.ComponentModel.PropertyChangedEventHandler = Me.PropertyChangedEvent
            If (Not (propertyChanged) Is Nothing) Then
                propertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs(propertyName))
            End If
        End Sub
    End Class
    
    <System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),  _
     System.Runtime.Serialization.DataContractAttribute(Name:="bc_core_component_parameters", [Namespace]:="http://schemas.datacontract.org/2004/07/bc_core_components_svc"),  _
     System.SerializableAttribute()>  _
    Partial Public Class bc_core_component_parameters
        Inherits Object
        Implements System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
        
        <System.NonSerializedAttribute()>  _
        Private extensionDataField As System.Runtime.Serialization.ExtensionDataObject
        
        <System.Runtime.Serialization.OptionalFieldAttribute()>  _
        Private lparametersField() As ServiceReference1.bc_core_component_parameter
        
        <Global.System.ComponentModel.BrowsableAttribute(false)>  _
        Public Property ExtensionData() As System.Runtime.Serialization.ExtensionDataObject Implements System.Runtime.Serialization.IExtensibleDataObject.ExtensionData
            Get
                Return Me.extensionDataField
            End Get
            Set
                Me.extensionDataField = value
            End Set
        End Property
        
        <System.Runtime.Serialization.DataMemberAttribute()>  _
        Public Property lparameters() As ServiceReference1.bc_core_component_parameter()
            Get
                Return Me.lparametersField
            End Get
            Set
                If (Object.ReferenceEquals(Me.lparametersField, value) <> true) Then
                    Me.lparametersField = value
                    Me.RaisePropertyChanged("lparameters")
                End If
            End Set
        End Property
        
        Public Event PropertyChanged As System.ComponentModel.PropertyChangedEventHandler Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
        
        Protected Sub RaisePropertyChanged(ByVal propertyName As String)
            Dim propertyChanged As System.ComponentModel.PropertyChangedEventHandler = Me.PropertyChangedEvent
            If (Not (propertyChanged) Is Nothing) Then
                propertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs(propertyName))
            End If
        End Sub
    End Class
    
    <System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),  _
     System.Runtime.Serialization.DataContractAttribute(Name:="bc_core_component_parameter", [Namespace]:="http://schemas.datacontract.org/2004/07/bc_core_components_svc"),  _
     System.SerializableAttribute()>  _
    Partial Public Class bc_core_component_parameter
        Inherits Object
        Implements System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
        
        <System.NonSerializedAttribute()>  _
        Private extensionDataField As System.Runtime.Serialization.ExtensionDataObject
        
        <System.Runtime.Serialization.OptionalFieldAttribute()>  _
        Private system_defiendField As ServiceReference1.SYSTEM_DEFINED
        
        <System.Runtime.Serialization.OptionalFieldAttribute()>  _
        Private valueField As String
        
        <Global.System.ComponentModel.BrowsableAttribute(false)>  _
        Public Property ExtensionData() As System.Runtime.Serialization.ExtensionDataObject Implements System.Runtime.Serialization.IExtensibleDataObject.ExtensionData
            Get
                Return Me.extensionDataField
            End Get
            Set
                Me.extensionDataField = value
            End Set
        End Property
        
        <System.Runtime.Serialization.DataMemberAttribute()>  _
        Public Property system_defiend() As ServiceReference1.SYSTEM_DEFINED
            Get
                Return Me.system_defiendField
            End Get
            Set
                If (Me.system_defiendField.Equals(value) <> true) Then
                    Me.system_defiendField = value
                    Me.RaisePropertyChanged("system_defiend")
                End If
            End Set
        End Property
        
        <System.Runtime.Serialization.DataMemberAttribute()>  _
        Public Property value() As String
            Get
                Return Me.valueField
            End Get
            Set
                If (Object.ReferenceEquals(Me.valueField, value) <> true) Then
                    Me.valueField = value
                    Me.RaisePropertyChanged("value")
                End If
            End Set
        End Property
        
        Public Event PropertyChanged As System.ComponentModel.PropertyChangedEventHandler Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
        
        Protected Sub RaisePropertyChanged(ByVal propertyName As String)
            Dim propertyChanged As System.ComponentModel.PropertyChangedEventHandler = Me.PropertyChangedEvent
            If (Not (propertyChanged) Is Nothing) Then
                propertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs(propertyName))
            End If
        End Sub
    End Class
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),  _
     System.Runtime.Serialization.DataContractAttribute(Name:="SYSTEM_DEFINED", [Namespace]:="http://schemas.datacontract.org/2004/07/bc_core_components_objects")>  _
    Public Enum SYSTEM_DEFINED As Integer
        
        <System.Runtime.Serialization.EnumMemberAttribute()>  _
        NONE = 0
        
        <System.Runtime.Serialization.EnumMemberAttribute()>  _
        [SET] = 1
        
        <System.Runtime.Serialization.EnumMemberAttribute()>  _
        RUNTIME = 2
    End Enum
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0"),  _
     System.ServiceModel.ServiceContractAttribute(ConfigurationName:="ServiceReference1.IComponentsService")>  _
    Public Interface IComponentsService
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/IComponentsService/TestService", ReplyAction:="http://tempuri.org/IComponentsService/TestServiceResponse")>  _
        Function TestService(ByVal value As Integer) As String
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/IComponentsService/TestComponent", ReplyAction:="http://tempuri.org/IComponentsService/TestComponentResponse")>  _
        Function TestComponent(ByVal component As ServiceReference1.bc_core_component) As String
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/IComponentsService/GetJsonForBCComponent", ReplyAction:="http://tempuri.org/IComponentsService/GetJsonForBCComponentResponse")>  _
        Function GetJsonForBCComponent(ByVal entity_id As Long, ByVal type As Integer, ByVal stage_id As Long, ByVal contributor_id As Long, ByVal user_id As Long, ByVal language_id As Long, ByVal data_at_date As Date, ByVal pub_type_id As Long, ByVal parameters As ServiceReference1.bc_core_component_parameters) As String
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/IComponentsService/GetJsonForBCComponentInTemplate", ReplyAction:="http://tempuri.org/IComponentsService/GetJsonForBCComponentInTemplateResponse")>  _
        Function GetJsonForBCComponentInTemplate(ByVal entity_id As Long, ByVal type As Integer, ByVal template_id As Long, ByVal comp_id As Long, ByVal stage_id As Long, ByVal contributor_id As Long, ByVal user_id As Long, ByVal language_id As Long, ByVal data_at_date As Date, ByVal pub_type_id As Long) As String
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/IComponentsService/GetJsonForBCComponentInDocument", ReplyAction:="http://tempuri.org/IComponentsService/GetJsonForBCComponentInDocumentResponse")>  _
        Function GetJsonForBCComponentInDocument(ByVal entity_id As Long, ByVal type As Integer, ByVal doc_id As Long, ByVal locator As String, ByVal stage_id As Long, ByVal contributor_id As Long, ByVal user_id As Long, ByVal language_id As Long, ByVal data_at_date As Date, ByVal pub_type_id As Long) As String
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/IComponentsService/GetJsonForDocumentCompostion", ReplyAction:="http://tempuri.org/IComponentsService/GetJsonForDocumentCompostionResponse")>  _
        Function GetJsonForDocumentCompostion(ByVal doc_id As Long, ByVal save_to_file As Boolean) As String
    End Interface
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")>  _
    Public Interface IComponentsServiceChannel
        Inherits ServiceReference1.IComponentsService, System.ServiceModel.IClientChannel
    End Interface
    
    <System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")>  _
    Partial Public Class ComponentsServiceClient
        Inherits System.ServiceModel.ClientBase(Of ServiceReference1.IComponentsService)
        Implements ServiceReference1.IComponentsService
        
        Public Sub New()
            MyBase.New
        End Sub
        
        Public Sub New(ByVal endpointConfigurationName As String)
            MyBase.New(endpointConfigurationName)
        End Sub
        
        Public Sub New(ByVal endpointConfigurationName As String, ByVal remoteAddress As String)
            MyBase.New(endpointConfigurationName, remoteAddress)
        End Sub
        
        Public Sub New(ByVal endpointConfigurationName As String, ByVal remoteAddress As System.ServiceModel.EndpointAddress)
            MyBase.New(endpointConfigurationName, remoteAddress)
        End Sub
        
        Public Sub New(ByVal binding As System.ServiceModel.Channels.Binding, ByVal remoteAddress As System.ServiceModel.EndpointAddress)
            MyBase.New(binding, remoteAddress)
        End Sub
        
        Public Function TestService(ByVal value As Integer) As String Implements ServiceReference1.IComponentsService.TestService
            Return MyBase.Channel.TestService(value)
        End Function
        
        Public Function TestComponent(ByVal component As ServiceReference1.bc_core_component) As String Implements ServiceReference1.IComponentsService.TestComponent
            Return MyBase.Channel.TestComponent(component)
        End Function
        
        Public Function GetJsonForBCComponent(ByVal entity_id As Long, ByVal type As Integer, ByVal stage_id As Long, ByVal contributor_id As Long, ByVal user_id As Long, ByVal language_id As Long, ByVal data_at_date As Date, ByVal pub_type_id As Long, ByVal parameters As ServiceReference1.bc_core_component_parameters) As String Implements ServiceReference1.IComponentsService.GetJsonForBCComponent
            Return MyBase.Channel.GetJsonForBCComponent(entity_id, type, stage_id, contributor_id, user_id, language_id, data_at_date, pub_type_id, parameters)
        End Function
        
        Public Function GetJsonForBCComponentInTemplate(ByVal entity_id As Long, ByVal type As Integer, ByVal template_id As Long, ByVal comp_id As Long, ByVal stage_id As Long, ByVal contributor_id As Long, ByVal user_id As Long, ByVal language_id As Long, ByVal data_at_date As Date, ByVal pub_type_id As Long) As String Implements ServiceReference1.IComponentsService.GetJsonForBCComponentInTemplate
            Return MyBase.Channel.GetJsonForBCComponentInTemplate(entity_id, type, template_id, comp_id, stage_id, contributor_id, user_id, language_id, data_at_date, pub_type_id)
        End Function
        
        Public Function GetJsonForBCComponentInDocument(ByVal entity_id As Long, ByVal type As Integer, ByVal doc_id As Long, ByVal locator As String, ByVal stage_id As Long, ByVal contributor_id As Long, ByVal user_id As Long, ByVal language_id As Long, ByVal data_at_date As Date, ByVal pub_type_id As Long) As String Implements ServiceReference1.IComponentsService.GetJsonForBCComponentInDocument
            Return MyBase.Channel.GetJsonForBCComponentInDocument(entity_id, type, doc_id, locator, stage_id, contributor_id, user_id, language_id, data_at_date, pub_type_id)
        End Function
        
        Public Function GetJsonForDocumentCompostion(ByVal doc_id As Long, ByVal save_to_file As Boolean) As String Implements ServiceReference1.IComponentsService.GetJsonForDocumentCompostion
            Return MyBase.Channel.GetJsonForDocumentCompostion(doc_id, save_to_file)
        End Function
    End Class
End Namespace
