﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.42000
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
     System.Runtime.Serialization.DataContractAttribute(Name:="cs_object_packet", [Namespace]:="http://schemas.datacontract.org/2004/07/bluecurve.core.wcf.ws"),  _
     System.SerializableAttribute()>  _
    Partial Public Class cs_object_packet
        Inherits Object
        Implements System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
        
        <System.NonSerializedAttribute()>  _
        Private extensionDataField As System.Runtime.Serialization.ExtensionDataObject
        
        <System.Runtime.Serialization.OptionalFieldAttribute()>  _
        Private certificateField As ServiceReference1.bc_cs_securitycertificate
        
        <System.Runtime.Serialization.OptionalFieldAttribute()>  _
        Private no_send_backField As Boolean
        
        <System.Runtime.Serialization.OptionalFieldAttribute()>  _
        Private number_of_packetsField As Integer
        
        <System.Runtime.Serialization.OptionalFieldAttribute()>  _
        Private packet_codeField As String
        
        <System.Runtime.Serialization.OptionalFieldAttribute()>  _
        Private packet_numberField As Integer
        
        <System.Runtime.Serialization.OptionalFieldAttribute()>  _
        Private received_objectField As String
        
        <System.Runtime.Serialization.OptionalFieldAttribute()>  _
        Private sent_objectField As String
        
        <System.Runtime.Serialization.OptionalFieldAttribute()>  _
        Private server_errorsField() As String
        
        <System.Runtime.Serialization.OptionalFieldAttribute()>  _
        Private transmission_stateField As Integer
        
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
        Public Property certificate() As ServiceReference1.bc_cs_securitycertificate
            Get
                Return Me.certificateField
            End Get
            Set
                If (Object.ReferenceEquals(Me.certificateField, value) <> true) Then
                    Me.certificateField = value
                    Me.RaisePropertyChanged("certificate")
                End If
            End Set
        End Property
        
        <System.Runtime.Serialization.DataMemberAttribute()>  _
        Public Property no_send_back() As Boolean
            Get
                Return Me.no_send_backField
            End Get
            Set
                If (Me.no_send_backField.Equals(value) <> true) Then
                    Me.no_send_backField = value
                    Me.RaisePropertyChanged("no_send_back")
                End If
            End Set
        End Property
        
        <System.Runtime.Serialization.DataMemberAttribute()>  _
        Public Property number_of_packets() As Integer
            Get
                Return Me.number_of_packetsField
            End Get
            Set
                If (Me.number_of_packetsField.Equals(value) <> true) Then
                    Me.number_of_packetsField = value
                    Me.RaisePropertyChanged("number_of_packets")
                End If
            End Set
        End Property
        
        <System.Runtime.Serialization.DataMemberAttribute()>  _
        Public Property packet_code() As String
            Get
                Return Me.packet_codeField
            End Get
            Set
                If (Object.ReferenceEquals(Me.packet_codeField, value) <> true) Then
                    Me.packet_codeField = value
                    Me.RaisePropertyChanged("packet_code")
                End If
            End Set
        End Property
        
        <System.Runtime.Serialization.DataMemberAttribute()>  _
        Public Property packet_number() As Integer
            Get
                Return Me.packet_numberField
            End Get
            Set
                If (Me.packet_numberField.Equals(value) <> true) Then
                    Me.packet_numberField = value
                    Me.RaisePropertyChanged("packet_number")
                End If
            End Set
        End Property
        
        <System.Runtime.Serialization.DataMemberAttribute()>  _
        Public Property received_object() As String
            Get
                Return Me.received_objectField
            End Get
            Set
                If (Object.ReferenceEquals(Me.received_objectField, value) <> true) Then
                    Me.received_objectField = value
                    Me.RaisePropertyChanged("received_object")
                End If
            End Set
        End Property
        
        <System.Runtime.Serialization.DataMemberAttribute()>  _
        Public Property sent_object() As String
            Get
                Return Me.sent_objectField
            End Get
            Set
                If (Object.ReferenceEquals(Me.sent_objectField, value) <> true) Then
                    Me.sent_objectField = value
                    Me.RaisePropertyChanged("sent_object")
                End If
            End Set
        End Property
        
        <System.Runtime.Serialization.DataMemberAttribute()>  _
        Public Property server_errors() As String()
            Get
                Return Me.server_errorsField
            End Get
            Set
                If (Object.ReferenceEquals(Me.server_errorsField, value) <> true) Then
                    Me.server_errorsField = value
                    Me.RaisePropertyChanged("server_errors")
                End If
            End Set
        End Property
        
        <System.Runtime.Serialization.DataMemberAttribute()>  _
        Public Property transmission_state() As Integer
            Get
                Return Me.transmission_stateField
            End Get
            Set
                If (Me.transmission_stateField.Equals(value) <> true) Then
                    Me.transmission_stateField = value
                    Me.RaisePropertyChanged("transmission_state")
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
     System.Runtime.Serialization.DataContractAttribute(Name:="bc_cs_security.certificate", [Namespace]:="http://schemas.datacontract.org/2004/07/BlueCurve.Core.CS"),  _
     System.SerializableAttribute(),  _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(Object())),  _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(String())),  _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(ServiceReference1.bc_cs_logon)),  _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(ServiceReference1.bc_cs_soap_base_class)),  _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(ServiceReference1.cs_object_packet))>  _
    Partial Public Class bc_cs_securitycertificate
        Inherits Object
        Implements System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
        
        <System.NonSerializedAttribute()>  _
        Private extensionDataField As System.Runtime.Serialization.ExtensionDataObject
        
        Private authentication_modeField As Integer
        
        Private authentication_tokenField As String
        
        Private client_mac_addressField As String
        
        Private error_stateField As Boolean
        
        Private nameField As String
        
        Private os_nameField As String
        
        Private override_username_passwordField As Boolean
        
        Private passwordField As String
        
        Private server_errorsField() As Object
        
        Private user_idField As String
        
        <Global.System.ComponentModel.BrowsableAttribute(false)>  _
        Public Property ExtensionData() As System.Runtime.Serialization.ExtensionDataObject Implements System.Runtime.Serialization.IExtensibleDataObject.ExtensionData
            Get
                Return Me.extensionDataField
            End Get
            Set
                Me.extensionDataField = value
            End Set
        End Property
        
        <System.Runtime.Serialization.DataMemberAttribute(IsRequired:=true)>  _
        Public Property authentication_mode() As Integer
            Get
                Return Me.authentication_modeField
            End Get
            Set
                If (Me.authentication_modeField.Equals(value) <> true) Then
                    Me.authentication_modeField = value
                    Me.RaisePropertyChanged("authentication_mode")
                End If
            End Set
        End Property
        
        <System.Runtime.Serialization.DataMemberAttribute(IsRequired:=true)>  _
        Public Property authentication_token() As String
            Get
                Return Me.authentication_tokenField
            End Get
            Set
                If (Object.ReferenceEquals(Me.authentication_tokenField, value) <> true) Then
                    Me.authentication_tokenField = value
                    Me.RaisePropertyChanged("authentication_token")
                End If
            End Set
        End Property
        
        <System.Runtime.Serialization.DataMemberAttribute(IsRequired:=true)>  _
        Public Property client_mac_address() As String
            Get
                Return Me.client_mac_addressField
            End Get
            Set
                If (Object.ReferenceEquals(Me.client_mac_addressField, value) <> true) Then
                    Me.client_mac_addressField = value
                    Me.RaisePropertyChanged("client_mac_address")
                End If
            End Set
        End Property
        
        <System.Runtime.Serialization.DataMemberAttribute(IsRequired:=true)>  _
        Public Property error_state() As Boolean
            Get
                Return Me.error_stateField
            End Get
            Set
                If (Me.error_stateField.Equals(value) <> true) Then
                    Me.error_stateField = value
                    Me.RaisePropertyChanged("error_state")
                End If
            End Set
        End Property
        
        <System.Runtime.Serialization.DataMemberAttribute(IsRequired:=true)>  _
        Public Property name() As String
            Get
                Return Me.nameField
            End Get
            Set
                If (Object.ReferenceEquals(Me.nameField, value) <> true) Then
                    Me.nameField = value
                    Me.RaisePropertyChanged("name")
                End If
            End Set
        End Property
        
        <System.Runtime.Serialization.DataMemberAttribute(IsRequired:=true)>  _
        Public Property os_name() As String
            Get
                Return Me.os_nameField
            End Get
            Set
                If (Object.ReferenceEquals(Me.os_nameField, value) <> true) Then
                    Me.os_nameField = value
                    Me.RaisePropertyChanged("os_name")
                End If
            End Set
        End Property
        
        <System.Runtime.Serialization.DataMemberAttribute(IsRequired:=true)>  _
        Public Property override_username_password() As Boolean
            Get
                Return Me.override_username_passwordField
            End Get
            Set
                If (Me.override_username_passwordField.Equals(value) <> true) Then
                    Me.override_username_passwordField = value
                    Me.RaisePropertyChanged("override_username_password")
                End If
            End Set
        End Property
        
        <System.Runtime.Serialization.DataMemberAttribute(IsRequired:=true)>  _
        Public Property password() As String
            Get
                Return Me.passwordField
            End Get
            Set
                If (Object.ReferenceEquals(Me.passwordField, value) <> true) Then
                    Me.passwordField = value
                    Me.RaisePropertyChanged("password")
                End If
            End Set
        End Property
        
        <System.Runtime.Serialization.DataMemberAttribute(IsRequired:=true)>  _
        Public Property server_errors() As Object()
            Get
                Return Me.server_errorsField
            End Get
            Set
                If (Object.ReferenceEquals(Me.server_errorsField, value) <> true) Then
                    Me.server_errorsField = value
                    Me.RaisePropertyChanged("server_errors")
                End If
            End Set
        End Property
        
        <System.Runtime.Serialization.DataMemberAttribute(IsRequired:=true)>  _
        Public Property user_id() As String
            Get
                Return Me.user_idField
            End Get
            Set
                If (Object.ReferenceEquals(Me.user_idField, value) <> true) Then
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
     System.Runtime.Serialization.DataContractAttribute(Name:="bc_cs_logon", [Namespace]:="http://schemas.datacontract.org/2004/07/BlueCurve.Core.CS"),  _
     System.SerializableAttribute()>  _
    Partial Public Class bc_cs_logon
        Inherits ServiceReference1.bc_cs_soap_base_class
        
        Private modeField As Integer
        
        Private role_idField As Long
        
        Private role_nameField As String
        
        Private user_emailField As String
        
        Private user_idField As Long
        
        Private user_nameField As String
        
        <System.Runtime.Serialization.DataMemberAttribute(IsRequired:=true)>  _
        Public Property mode() As Integer
            Get
                Return Me.modeField
            End Get
            Set
                If (Me.modeField.Equals(value) <> true) Then
                    Me.modeField = value
                    Me.RaisePropertyChanged("mode")
                End If
            End Set
        End Property
        
        <System.Runtime.Serialization.DataMemberAttribute(IsRequired:=true)>  _
        Public Property role_id() As Long
            Get
                Return Me.role_idField
            End Get
            Set
                If (Me.role_idField.Equals(value) <> true) Then
                    Me.role_idField = value
                    Me.RaisePropertyChanged("role_id")
                End If
            End Set
        End Property
        
        <System.Runtime.Serialization.DataMemberAttribute(IsRequired:=true)>  _
        Public Property role_name() As String
            Get
                Return Me.role_nameField
            End Get
            Set
                If (Object.ReferenceEquals(Me.role_nameField, value) <> true) Then
                    Me.role_nameField = value
                    Me.RaisePropertyChanged("role_name")
                End If
            End Set
        End Property
        
        <System.Runtime.Serialization.DataMemberAttribute(IsRequired:=true)>  _
        Public Property user_email() As String
            Get
                Return Me.user_emailField
            End Get
            Set
                If (Object.ReferenceEquals(Me.user_emailField, value) <> true) Then
                    Me.user_emailField = value
                    Me.RaisePropertyChanged("user_email")
                End If
            End Set
        End Property
        
        <System.Runtime.Serialization.DataMemberAttribute(IsRequired:=true)>  _
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
        
        <System.Runtime.Serialization.DataMemberAttribute(IsRequired:=true)>  _
        Public Property user_name() As String
            Get
                Return Me.user_nameField
            End Get
            Set
                If (Object.ReferenceEquals(Me.user_nameField, value) <> true) Then
                    Me.user_nameField = value
                    Me.RaisePropertyChanged("user_name")
                End If
            End Set
        End Property
    End Class
    
    <System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),  _
     System.Runtime.Serialization.DataContractAttribute(Name:="bc_cs_soap_base_class", [Namespace]:="http://schemas.datacontract.org/2004/07/BlueCurve.Core.CS"),  _
     System.SerializableAttribute(),  _
     System.Runtime.Serialization.KnownTypeAttribute(GetType(ServiceReference1.bc_cs_logon))>  _
    Partial Public Class bc_cs_soap_base_class
        Inherits Object
        Implements System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
        
        <System.NonSerializedAttribute()>  _
        Private extensionDataField As System.Runtime.Serialization.ExtensionDataObject
        
        Private asyncField As Boolean
        
        Private certificateField As ServiceReference1.bc_cs_securitycertificate
        
        Private no_send_backField As Boolean
        
        Private packet_codeField As String
        
        Private tmodeField As Integer
        
        Private transmission_stateField As Integer
        
        <Global.System.ComponentModel.BrowsableAttribute(false)>  _
        Public Property ExtensionData() As System.Runtime.Serialization.ExtensionDataObject Implements System.Runtime.Serialization.IExtensibleDataObject.ExtensionData
            Get
                Return Me.extensionDataField
            End Get
            Set
                Me.extensionDataField = value
            End Set
        End Property
        
        <System.Runtime.Serialization.DataMemberAttribute(IsRequired:=true)>  _
        Public Property async() As Boolean
            Get
                Return Me.asyncField
            End Get
            Set
                If (Me.asyncField.Equals(value) <> true) Then
                    Me.asyncField = value
                    Me.RaisePropertyChanged("async")
                End If
            End Set
        End Property
        
        <System.Runtime.Serialization.DataMemberAttribute(IsRequired:=true)>  _
        Public Property certificate() As ServiceReference1.bc_cs_securitycertificate
            Get
                Return Me.certificateField
            End Get
            Set
                If (Object.ReferenceEquals(Me.certificateField, value) <> true) Then
                    Me.certificateField = value
                    Me.RaisePropertyChanged("certificate")
                End If
            End Set
        End Property
        
        <System.Runtime.Serialization.DataMemberAttribute(IsRequired:=true)>  _
        Public Property no_send_back() As Boolean
            Get
                Return Me.no_send_backField
            End Get
            Set
                If (Me.no_send_backField.Equals(value) <> true) Then
                    Me.no_send_backField = value
                    Me.RaisePropertyChanged("no_send_back")
                End If
            End Set
        End Property
        
        <System.Runtime.Serialization.DataMemberAttribute(IsRequired:=true)>  _
        Public Property packet_code() As String
            Get
                Return Me.packet_codeField
            End Get
            Set
                If (Object.ReferenceEquals(Me.packet_codeField, value) <> true) Then
                    Me.packet_codeField = value
                    Me.RaisePropertyChanged("packet_code")
                End If
            End Set
        End Property
        
        <System.Runtime.Serialization.DataMemberAttribute(IsRequired:=true)>  _
        Public Property tmode() As Integer
            Get
                Return Me.tmodeField
            End Get
            Set
                If (Me.tmodeField.Equals(value) <> true) Then
                    Me.tmodeField = value
                    Me.RaisePropertyChanged("tmode")
                End If
            End Set
        End Property
        
        <System.Runtime.Serialization.DataMemberAttribute(IsRequired:=true)>  _
        Public Property transmission_state() As Integer
            Get
                Return Me.transmission_stateField
            End Get
            Set
                If (Me.transmission_stateField.Equals(value) <> true) Then
                    Me.transmission_stateField = value
                    Me.RaisePropertyChanged("transmission_state")
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
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0"),  _
     System.ServiceModel.ServiceContractAttribute(ConfigurationName:="ServiceReference1.BlueCurveWS")>  _
    Public Interface BlueCurveWS
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/BlueCurveWS/generic_object_transmission_wcf", ReplyAction:="http://tempuri.org/BlueCurveWS/generic_object_transmission_wcfResponse")>  _
        Function generic_object_transmission_wcf(ByVal value As ServiceReference1.cs_object_packet) As ServiceReference1.cs_object_packet
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/BlueCurveWS/generic_object_transmission_wcf", ReplyAction:="http://tempuri.org/BlueCurveWS/generic_object_transmission_wcfResponse")>  _
        Function generic_object_transmission_wcfAsync(ByVal value As ServiceReference1.cs_object_packet) As System.Threading.Tasks.Task(Of ServiceReference1.cs_object_packet)
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/BlueCurveWS/password_management", ReplyAction:="http://tempuri.org/BlueCurveWS/password_managementResponse")>  _
        Function password_management(ByVal ingot As ServiceReference1.bc_cs_logon) As String
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/BlueCurveWS/password_management", ReplyAction:="http://tempuri.org/BlueCurveWS/password_managementResponse")>  _
        Function password_managementAsync(ByVal ingot As ServiceReference1.bc_cs_logon) As System.Threading.Tasks.Task(Of String)
    End Interface
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")>  _
    Public Interface BlueCurveWSChannel
        Inherits ServiceReference1.BlueCurveWS, System.ServiceModel.IClientChannel
    End Interface
    
    <System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")>  _
    Partial Public Class BlueCurveWSClient
        Inherits System.ServiceModel.ClientBase(Of ServiceReference1.BlueCurveWS)
        Implements ServiceReference1.BlueCurveWS
        
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
        
        Public Function generic_object_transmission_wcf(ByVal value As ServiceReference1.cs_object_packet) As ServiceReference1.cs_object_packet Implements ServiceReference1.BlueCurveWS.generic_object_transmission_wcf
            Return MyBase.Channel.generic_object_transmission_wcf(value)
        End Function
        
        Public Function generic_object_transmission_wcfAsync(ByVal value As ServiceReference1.cs_object_packet) As System.Threading.Tasks.Task(Of ServiceReference1.cs_object_packet) Implements ServiceReference1.BlueCurveWS.generic_object_transmission_wcfAsync
            Return MyBase.Channel.generic_object_transmission_wcfAsync(value)
        End Function
        
        Public Function password_management(ByVal ingot As ServiceReference1.bc_cs_logon) As String Implements ServiceReference1.BlueCurveWS.password_management
            Return MyBase.Channel.password_management(ingot)
        End Function
        
        Public Function password_managementAsync(ByVal ingot As ServiceReference1.bc_cs_logon) As System.Threading.Tasks.Task(Of String) Implements ServiceReference1.BlueCurveWS.password_managementAsync
            Return MyBase.Channel.password_managementAsync(ingot)
        End Function
    End Class
End Namespace
