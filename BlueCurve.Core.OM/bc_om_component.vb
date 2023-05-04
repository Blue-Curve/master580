Imports BlueCurve.Core.CS
Imports System.Collections
Imports System.IO
Imports System.Data.OleDb
Imports System.Data.sqlclient
Imports System.Data
'this is created by rama boya
'this is the container class holding components collectiont
<Serializable()> Public Class ComponentContainer
    Inherits bc_cs_soap_base_class
    Private objCompCollection As bc_am_components
    Private dtFrom As Date
    Private dtTo As Date
    Private intDocId As String
    Private intEntityId As String
    Private blnFinalResult As Boolean = True
    Public Property DocumentId() As String
        Get
            Return intDocId
        End Get
        Set(ByVal Value As String)
            intDocId = Value
        End Set
    End Property
    Public Property EntityId() As String
        Get
            Return intEntityId
        End Get
        Set(ByVal Value As String)
            intEntityId = Value
        End Set
    End Property

    Public Property Components() As bc_am_components
        Get
            Return objCompCollection
        End Get
        Set(ByVal Value As bc_am_components)
            objCompCollection = Value
        End Set
    End Property
    Public Property DateFrom() As Date
        Get
            Return dtFrom
        End Get
        Set(ByVal Value As Date)
            dtFrom = Value
        End Set
    End Property
    Public Property DateTo() As Date
        Get
            Return dtTo
        End Get
        Set(ByVal Value As Date)
            dtTo = Value
        End Set
    End Property
    'THIS property is set to false when there is an error 
    Public Property Result() As Boolean
        Get
            Return blnFinalResult
        End Get
        Set(ByVal Value As Boolean)
            blnFinalResult = Value
        End Set
    End Property

    Public Sub New()
        objCompCollection = New bc_am_components
    End Sub
    Public Overrides Sub process_object()
        Dim otrace As New bc_cs_activity_log("ComponentContainer", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            REM this is always specific to object
            If Me.tmode = bc_cs_soap_base_class.tWRITE Then
                db_write(MyBase.certificate)
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("ComponentContainer", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("ComponentContainer", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub

    Public Shadows Function db_write(ByRef certificate As bc_cs_security.certificate) As Boolean
        Dim ObjCompDAL As New bc_components_dal
        Dim ObjComponent As bc_am_component
        Dim blnResult As Boolean
        'set the datefrom,dateto and document id values od DAL object
        ObjCompDAL.DateFrom = Me.DateFrom
        ObjCompDAL.DateTo = Me.DateTo
        ObjCompDAL.DocumentId = Me.DocumentId
        ObjCompDAL.EntityId = Me.EntityId
        If ComponentiserConstants.LogActivityFile Then Dim otrace As New bc_cs_activity_log("bc_components_dal", "db_write", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            For Each ObjComponent In Me.Components
                'Set the component type property value to know whether it is a Text, Image or Table.
                Select Case ObjComponent.ComponentType
                    Case bc_am_component.CompType.Image
                        ObjCompDAL.ComponentTypeValue = "Image"
                    Case bc_am_component.CompType.Text
                        ObjCompDAL.ComponentTypeValue = "Text"
                        'Case bc_am_component.CompType.Table
                        '    ObjCompDAL.ComponentTypeValue = "Table"
                        '    REM PR sept 2009
                    Case bc_am_component.CompType.NewTable
                        ObjCompDAL.ComponentTypeValue = "NewTable"
                End Select
                'set the show error message flag.
                If CStr(ObjComponent.ComponentType) And CStr(ObjComponent.ComponentType) <> CStr(bc_am_component.CompType.NewTable) Then
                    'If CStr(ObjComponent.ComponentType) <> CStr(bc_am_component.CompType.Table) And CStr(ObjComponent.ComponentType) <> CStr(bc_am_component.CompType.NewTable) Then
                    blnResult = ObjCompDAL.Insert_component(ObjComponent, certificate)
                ElseIf CStr(ObjComponent.ComponentType) = CStr(bc_am_component.CompType.NewTable) Then
                    REM PR sept 2009
                    blnResult = ObjCompDAL.Insert_new_table_component(ObjComponent, certificate)
                    'Else
                    '    blnResult = ObjCompDAL.Insert_table_component(ObjComponent, certificate)
                End If
                If Not blnResult Then
                    Me.Result = False
                End If
            Next
        Catch ex As Exception
            Me.Result = False
            If ComponentiserConstants.ShowErrorMessage Then
                Dim db_err As New bc_cs_error_log("ComponentContainer", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            Else
                Dim otrace2 As New bc_cs_activity_log("ComponentContainer", "db_write", bc_cs_activity_codes.COMMENTARY, ex.Message, certificate)
            End If
        Finally
            db_write = Me.Result
            If ComponentiserConstants.LogActivityFile Then Dim otrace1 As New bc_cs_activity_log("bc_components_dal", "db_write", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Function
    'Public Shadows Function write_xml_via_soap_client_request() As String
    '    Dim s, os As String
    '    Dim bc_cs_accountinfoheader As New localhost.bc_cs_accountinfoheader
    '    Try
    '        webservice = New localhost.Service1
    '        webservice.Url = bc_cs_central_settings.soap_server

    '        bc_cs_accountinfoheader.authentication_method = bc_cs_central_settings.show_authentication_form
    '        If bc_cs_central_settings.show_authentication_form = 0 Then
    '            bc_cs_accountinfoheader.os_logon_name = bc_cs_central_settings.logged_on_user_name
    '        Else
    '            bc_cs_accountinfoheader.user_name = bc_cs_central_settings.user_name
    '            bc_cs_accountinfoheader.password = bc_cs_central_settings.user_password
    '        End If
    '        webservice.bc_cs_accountinfoheaderValue = bc_cs_accountinfoheader

    '        set_proxy_settings()

    '        REM call object model specific web service
    '        os = write_xml_to_string()
    '        s = call_web_service(os)
    '        If Left(s, 25) = "Webservice Authentication" Then
    '            If ComponentiserConstants.LogActivityFile Then Dim otrace As New bc_cs_activity_log("bc_om_document", "write_xml_via_soap_client_request", bc_cs_activity_codes.COMMENTARY, "Authentication Failed for Web Service UploadComponents! User:" + bc_cs_accountinfoheader.os_logon_name)
    '            Dim omessage As New bc_cs_message("Blue Curve Create", "Authentication Failed for Web Service LoadUsers! User:" + bc_cs_accountinfoheader.os_logon_name, bc_cs_message.MESSAGE)
    '            write_xml_via_soap_client_request = "failed"
    '        End If
    '        If s = "failed" Then
    '            If ComponentiserConstants.LogActivityFile Then Dim otrace1 As New bc_cs_activity_log("bc_om_document", "write_xml_via_soap_client_request", bc_cs_activity_codes.COMMENTARY, "UploadComponents Failed couldnt complete SOAP request! User:" + bc_cs_accountinfoheader.os_logon_name)
    '            write_xml_via_soap_client_request = "failed"
    '        End If
    '    Catch ex As Exception
    '        Me.Result = False
    '        If ComponentiserConstants.ShowErrorMessage Then
    '            Dim db_err As New bc_cs_error_log("bc_am_component", "write_xml_via_soap_client_request", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
    '        Else
    '            Dim otrace2 As New bc_cs_activity_log("bc_am_component", "write_xml_via_soap_client_request", bc_cs_activity_codes.COMMENTARY, ex.Message, certificate)
    '        End If
    '    Finally
    '        If ComponentiserConstants.LogActivityFile Then Dim otrace3 As New bc_cs_activity_log("bc_am_component", "write_xml_via_soap_client_request", bc_cs_activity_codes.COMMENTARY, "Authentication Failed for Web Service UploadComponents! User:" + bc_cs_accountinfoheader.os_logon_name)
    '    End Try
    'End Function
    'Public Overloads Overrides Function call_web_service(ByVal s As String) As String
    '    If ComponentiserConstants.LogActivityFile Then Dim otrace As New bc_cs_activity_log("bc_om_component", "call_web_service", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
    '    Try
    '        call_web_service = webservice.UploadComponents(s)
    '    Catch ex As Exception
    '        Me.Result = False
    '        If ComponentiserConstants.ShowErrorMessage Then
    '            Dim db_err As New bc_cs_error_log("bc_am_component", "call_web_service", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
    '        Else
    '            Dim otrace2 As New bc_cs_activity_log("bc_am_component", "call_web_service", bc_cs_activity_codes.COMMENTARY, ex.Message, certificate)
    '        End If
    '    Finally
    '        If ComponentiserConstants.LogActivityFile Then Dim otrace1 As New bc_cs_activity_log("bc_om_component", "call_web_service", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
    '    End Try
    'End Function
End Class
'This is the actual components collection class
<Serializable()> Public Class bc_am_components
    Inherits CollectionBase

    Public Sub New() 'Default constructor
        MyBase.New()
    End Sub

    Public Sub Add(ByVal objComponent As bc_am_component)
        MyBase.InnerList.Add(objComponent)
    End Sub
    Public Sub Remove(ByVal objComponent As bc_am_component)
        MyBase.InnerList.Remove(objComponent)
    End Sub

    Default Public Property Item(ByVal index As Integer) As bc_am_component
        Get
            Return MyBase.InnerList(index)
        End Get
        Set(ByVal Value As bc_am_component)
            MyBase.InnerList(index) = Value
        End Set
    End Property
End Class
'This is the object model of component
<Serializable()> Public Class bc_am_component
    Inherits bc_cs_soap_base_class
    REM conponent id
    Private cid As Long
    Private contributor_id As Long
    REM text based document locator
    Private locatorid As String
    Private strStyle As String = ""
    Private strCompType As String
    REM data set are values single cell component
    Private compvalue As String
    Public bytImgage As Byte()
    Public image_name As String
    Private inttableRowCount As Integer
    Private inttableColumnCount As Integer
    'Private structuretable As DataSet
    Public newstructuretable As New ArrayList
    Private enumCompType As CompType
    Private intOrderNo As Integer
    Private intPageNo As Integer
    Private intEntityId As String = ""
    
    Enum CompType
        None
        Text
        Image
        'Table
        NewTable
    End Enum

    Public Property ComponentType() As Integer
        Get
            Return enumCompType
        End Get
        Set(ByVal Value As Integer)
            enumCompType = Value
        End Set
    End Property
    Public Property Id() As Long
        Get
            Return cid
        End Get
        Set(ByVal Value As Long)
            cid = Value
        End Set
    End Property
    Public Property Locator() As String
        Get
            Return locatorid
        End Get
        Set(ByVal Value As String)
            locatorid = Value
        End Set
    End Property
    Public Property Style() As String
        Get
            Return strStyle
        End Get
        Set(ByVal Value As String)
            strStyle = Value
        End Set
    End Property
    Public Property Value() As String
        Get
            Return compvalue
        End Get
        Set(ByVal Value As String)
            compvalue = Value
        End Set
    End Property

    Public Property Image() As Byte()
        Get
            Return bytImgage
        End Get
        Set(ByVal Value As Byte())
            bytImgage = Value
        End Set
    End Property

    Public Property TableRowCount() As Integer
        Get
            Return inttableRowCount
        End Get
        Set(ByVal Value As Integer)
            inttableRowCount = Value
        End Set
    End Property
    Public Property OrderNo() As Integer
        Get
            Return intOrderNo
        End Get
        Set(ByVal Value As Integer)
            intOrderNo = Value
        End Set
    End Property
    Public Property PageNo() As Integer
        Get
            Return intPageNo
        End Get
        Set(ByVal Value As Integer)
            intPageNo = Value
        End Set
    End Property

    Public Property TableColumnCount() As Integer
        Get
            Return inttableColumnCount
        End Get
        Set(ByVal Value As Integer)
            inttableColumnCount = Value
        End Set
    End Property
    'Public Property TableData() As DataSet
    '    Get
    '        Return structuretable
    '    End Get
    '    Set(ByVal Value As DataSet)
    '        structuretable = Value
    '    End Set
    'End Property
    Public Property EntityId() As String
        Get
            Return intEntityId
        End Get
        Set(ByVal Value As String)
            intEntityId = Value
        End Set
    End Property

    'Public Sub New()
    '    structuretable = New DataSet
    'End Sub
End Class
REM Database interaction class
Public Class bc_components_dal
    Private gbc_db As bc_cs_db_services
    Private strcomptype As String
    Private intDocId As String
    Private intentityId As String
    Private dtFrom As Date
    Private blnFinalResult As Boolean = True
    Private dtTo As Date
    Public Property ComponentTypeValue() As String
        Get
            Return strcomptype
        End Get
        Set(ByVal Value As String)
            strcomptype = Value
        End Set
    End Property
    Public Property DocumentId() As String
        Get
            Return intDocId
        End Get
        Set(ByVal Value As String)
            intDocId = Value
        End Set
    End Property
    Public Property EntityId() As String
        Get
            Return intentityId
        End Get
        Set(ByVal Value As String)
            intentityId = Value
        End Set
    End Property
    Public Property DateFrom() As Date
        Get
            Return dtFrom
        End Get
        Set(ByVal Value As Date)
            dtFrom = Value
        End Set
    End Property
    Public Property DateTo() As Date
        Get
            Return dtTo
        End Get
        Set(ByVal Value As Date)
            dtTo = Value
        End Set
    End Property
    'THIS property is set to false when there is an error 
    Public Property Result() As Boolean
        Get
            Return blnFinalResult
        End Get
        Set(ByVal Value As Boolean)
            blnFinalResult = Value
        End Set
    End Property

    Public Sub New()
        gbc_db = New bc_cs_db_services(False)
    End Sub
    Public Function Insert_component(ByRef ObjComp As bc_am_component, ByRef certificate As bc_cs_security.certificate) As Boolean
        If ComponentiserConstants.LogActivityFile Then Dim otrace As New bc_cs_activity_log("bc_components_dal", "Insert_component for - " & ObjComp.Locator, bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Dim fs As New bc_cs_file_transfer_services
        Try
            'Remove invalid characters for undefined text only and dont apply this if the rich text flag conversion is on.
            Me.executesp_for_component("dbo ", ObjComp, 1, certificate)
            ''If the object is of type image then write the file in server and delete it .
            If Me.ComponentTypeValue = "Image" Then
                If ComponentiserConstants.LogActivityFile Then Dim otrace As New bc_cs_activity_log("bc_components_dal", "just before converting bytestream into image" & ObjComp.Locator, bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
                fs.write_bytestream_to_document(bc_cs_central_settings.central_repos_path + ObjComp.Value, ObjComp.Image, Nothing)
                If ComponentiserConstants.LogActivityFile Then Dim otrace As New bc_cs_activity_log("bc_components_dal", "after converting into image and  deleting" & ObjComp.Locator, bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
            End If
        Catch ex As Exception
            Me.Result = False
            If ComponentiserConstants.ShowErrorMessage Then
                Dim db_err As New bc_cs_error_log("bc_components_dal", "Insert_component at -----" & ObjComp.Locator, bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            Else
                Dim otrace2 As New bc_cs_activity_log("bc_components_dal", "Insert_component", bc_cs_activity_codes.COMMENTARY, ex.Message, certificate)
            End If
        Finally
            Insert_component = Me.Result
            If ComponentiserConstants.LogActivityFile Then Dim otrace1 As New bc_cs_activity_log("bc_components_dal", "Insert_component -" & ObjComp.Locator, bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Function
    'Public Function Insert_table_component(ByRef ObjComp As bc_am_component, ByRef certificate As bc_cs_security.certificate) As Boolean
    '    Dim sql As String
    '    Dim atttypetableid As Object
    '    Dim atttypeimageid As Object
    '    Dim attid As Integer
    '    Dim fs As New bc_cs_file_transfer_services
    '    If ComponentiserConstants.LogActivityFile Then Dim otrace As New bc_cs_activity_log("bc_components_dal", "Insert_table_component -" & ObjComp.Locator, bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
    '    Try
    '        'The following query will bring attribute id for a particular component type table
    '        sql = " select attribute_id from attribute_tbl where [name]  =" & "'" & "Component " & Me.ComponentTypeValue & "'"
    '        atttypetableid = gbc_db.executesql(sql, certificate)
    '        atttypetableid = CInt(atttypetableid(0, 0))
    '        'The following query will bring attribute id for a particular component type Image
    '        sql = " select attribute_id from attribute_tbl where [name]  =" & "'" & "Component " & "Image" & "'"
    '        atttypeimageid = gbc_db.executesql(sql, certificate)
    '        atttypeimageid = CInt(atttypeimageid(0, 0))
    '        'Please note that master table has attribuite type as table only always.
    '        Me.Insert_Table_Comp_in_Master(atttypetableid, ObjComp, certificate)
    '        Dim oTable As New DataTable
    '        Dim oRows As DataRow
    '        Dim strValue As String = ""
    '        Dim strFileName As String = ""
    '        oTable = ObjComp.TableData.Tables(0)
    '        For Each oRows In oTable.Rows
    '            strFileName = oRows(2)
    '            strFileName = strFileName.Trim
    '            If strFileName <> "" AndAlso strFileName.Length > 4 AndAlso _
    '                     strFileName.Substring((strFileName.Length - 4), 4) = ".bmp" Then
    '                attid = atttypeimageid
    '                strValue = strFileName
    '                If ComponentiserConstants.LogActivityFile Then Dim otrace As New bc_cs_activity_log("bc_components_dal", "just before converting bytestream into image" & ObjComp.Locator, bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
    '                '' if the table cell is an image then write the document to central server and delete
    '                fs.write_bytestream_to_document(bc_cs_central_settings.central_repos_path + strValue, ObjComp.Image, Nothing)
    '                If ComponentiserConstants.LogActivityFile Then Dim otrace As New bc_cs_activity_log("bc_components_dal", "converted into image and deleted" & ObjComp.Locator, bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
    '            Else
    '                attid = atttypetableid
    '                strValue = oRows(2)
    '            End If
    '            Me.executesp_for_table_component("dbo.bcc_core_insert_document_table_component", ObjComp.Locator, oRows(0), oRows(1), strValue, oRows(4), attid, 1, certificate)
    '        Next
    '    Catch ex As Exception
    '        Me.Result = False
    '        If ComponentiserConstants.ShowErrorMessage Then
    '            Dim db_err As New bc_cs_error_log("bc_components_dal", "Insert_table_component at -----" & ObjComp.Locator, bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
    '        Else
    '            Dim otrace2 As New bc_cs_activity_log("bc_components_dal", "Insert_table_component", bc_cs_activity_codes.COMMENTARY, ex.Message, certificate)
    '        End If
    '    Finally
    '        Insert_table_component = Me.Result
    '        If ComponentiserConstants.LogActivityFile Then Dim otrace1 As New bc_cs_activity_log("bc_components_dal", "Insert_table_component -" & ObjComp.Locator, bc_cs_activity_codes.TRACE_EXIT, "", certificate)
    '    End Try
    'End Function
    'PR comment sept 2009
    Public Function Insert_new_table_component(ByRef ObjComp As bc_am_component, ByRef certificate As bc_cs_security.certificate) As Boolean
        Dim sql As String
        Dim atttypetableid As Object
        Dim atttypeimageid As Object
        Dim attid As Integer
        Dim fs As New bc_cs_file_transfer_services
        If ComponentiserConstants.LogActivityFile Then Dim otrace As New bc_cs_activity_log("bc_components_dal", "Insert_new_table_component -" & ObjComp.Locator, bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            'The following query will bring attribute id for a particular component type table
            sql = " select attribute_id from attribute_tbl where [name]  =" & "'" & "Component Table" & "'"
            atttypetableid = gbc_db.executesql(sql, certificate)
            atttypetableid = CInt(atttypetableid(0, 0))
            'The following query will bring attribute id for a particular component type Image
            sql = " select attribute_id from attribute_tbl where [name]  =" & "'" & "Component " & "Image" & "'"
            atttypeimageid = gbc_db.executesql(sql, certificate)
            atttypeimageid = CInt(atttypeimageid(0, 0))
            'Please note that master table has attribuite type as table only always.
            Me.Insert_Table_Comp_in_Master(atttypetableid, ObjComp, certificate)
            REM PR comment sept 2009 change from here 
            Dim strValue As String = ""
            Dim strFileName As String = ""

            Dim i As Integer
            For i = 0 To ObjComp.newstructuretable.Count - 1
                strFileName = ObjComp.newstructuretable(i).value()
                strFileName = strFileName.Trim
                If strFileName <> "" AndAlso strFileName.Length > 4 AndAlso _
                       strFileName.Substring((strFileName.Length - 4), 4) = ".bmp" Then
                    attid = atttypeimageid
                    strValue = strFileName
                    If ComponentiserConstants.LogActivityFile Then Dim otrace As New bc_cs_activity_log("bc_components_dal", "just before converting bytestream into image" & ObjComp.Locator, bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
                    '' if the table cell is an image then write the document to central server and delete
                    fs.write_bytestream_to_document(bc_cs_central_settings.central_repos_path + strValue, ObjComp.Image, Nothing)
                    If ComponentiserConstants.LogActivityFile Then Dim otrace As New bc_cs_activity_log("bc_components_dal", "converted into image and deleted" & ObjComp.Locator, bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
                Else
                    attid = atttypetableid
                    strValue = ObjComp.newstructuretable(i).value
                End If
                Me.executesp_for_table_component("dbo.bcc_core_insert_document_table_component", ObjComp.Locator, ObjComp.newstructuretable(i).RowValue, ObjComp.newstructuretable(i).ColumnValue, strValue, ObjComp.newstructuretable(i).Style, attid, 1, certificate)
            Next

            REM PR comment sept 2009 change from here 
            'Dim oTable As New DataTable
            'Dim oRows As DataRow
            ''Dim strValue As String = ""
            ''Dim strFileName As String = ""
            'oTable = ObjComp.TableData.Tables(0)
            'For Each oRows In oTable.Rows
            '    strFileName = oRows(2)
            '    strFileName = strFileName.Trim
            '    If strFileName <> "" AndAlso strFileName.Length > 4 AndAlso _
            '             strFileName.Substring((strFileName.Length - 4), 4) = ".bmp" Then
            '        attid = atttypeimageid
            '        strValue = strFileName
            '        If ComponentiserConstants.LogActivityFile Then Dim otrace As New bc_cs_activity_log("bc_components_dal", "just before converting bytestream into image" & ObjComp.Locator, bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
            '        '' if the table cell is an image then write the document to central server and delete
            '        fs.write_bytestream_to_document(bc_cs_central_settings.central_repos_path + strValue, ObjComp.Image, Nothing)
            '        If ComponentiserConstants.LogActivityFile Then Dim otrace As New bc_cs_activity_log("bc_components_dal", "converted into image and deleted" & ObjComp.Locator, bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
            '    Else
            '        attid = atttypetableid
            '        strValue = oRows(2)
            '    End If
            '    Me.executesp_for_table_component("dbo.bcc_core_insert_document_table_component", ObjComp.Locator, oRows(0), oRows(1), strValue, oRows(4), attid, 1, certificate)
            'Next
        Catch ex As Exception
            Me.Result = False
            If ComponentiserConstants.ShowErrorMessage Then
                Dim db_err As New bc_cs_error_log("bc_components_dal", "Insert_table_component at -----" & ObjComp.Locator, bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            Else
                Dim otrace2 As New bc_cs_activity_log("bc_components_dal", "Insert_new_table_component", bc_cs_activity_codes.COMMENTARY, ex.Message, certificate)
            End If
        Finally
            Insert_new_table_component = Me.Result
            If ComponentiserConstants.LogActivityFile Then Dim otrace1 As New bc_cs_activity_log("bc_components_dal", "Insert_new_table_component -" & ObjComp.Locator, bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Function
    Public Sub Insert_Table_Comp_in_Master(ByVal atttypeid As Integer, ByRef ObjComp As bc_am_component, ByRef certificate As bc_cs_security.certificate)
        
        If ComponentiserConstants.LogActivityFile Then Dim otrace As New bc_cs_activity_log("bc_components_dal", "Insert_Table_Comp_in_Master -" & ObjComp.Locator, bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Me.executesp_for_master_table_component("dbo.bcc_core_insert_master_table_component", ObjComp, atttypeid, 1, certificate)
        Catch ex As Exception
            Me.Result = False
            If ComponentiserConstants.ShowErrorMessage Then
                Dim db_err As New bc_cs_error_log("bc_components_dal", "Insert_Table_Comp_in_Master at -----" & ObjComp.Locator, bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            Else
                Dim otrace2 As New bc_cs_activity_log("bc_components_dal", "Insert_Table_Comp_in_Master", bc_cs_activity_codes.COMMENTARY, ex.Message, certificate)
            End If
        Finally
            If ComponentiserConstants.LogActivityFile Then Dim otrace1 As New bc_cs_activity_log("bc_components_dal", "Insert_Table_Comp_in_Master -" & ObjComp.Locator, bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub
    Public Sub executesp_for_component(ByVal sp_name As String, _
                        ByRef ObjComp As bc_am_component, ByVal lang_id As Integer, ByRef certificate As bc_cs_security.certificate)
        If ComponentiserConstants.LogActivityFile Then Dim otrace As New bc_cs_activity_log("bc_components_dal", "executesp_for_component", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Dim strValue As String
        Dim intPageno As Integer = 0
        Dim strEntityId As String
        If ObjComp.EntityId = "" Then
            strEntityId = Me.EntityId
        Else
            strEntityId = ObjComp.EntityId
        End If
        If ObjComp.Value = "" Then
            strValue = ""
        Else
            strValue = ObjComp.Value
        End If
        Dim sqlcn As New SqlConnection
        sqlcn.ConnectionString = gbc_db.GetConnectionString(certificate)

        Try
            '  Dim ocommentary = New bc_cs_activity_log("bc_components_dal", "executesp_for_component", bc_cs_activity_codes.COMMENTARY, "Attempting to execute stored procedure: " + CStr(sp_name))
            sqlcn.Open()
            Dim cmd As New SqlCommand(sp_name, sqlcn)
            cmd.CommandType = Data.CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@doc_id", Me.DocumentId)
            cmd.Parameters.AddWithValue("@entity_id", strEntityId)
            cmd.Parameters.AddWithValue("@component_id", ObjComp.Id)
            cmd.Parameters.AddWithValue("@value", strValue.Trim)
            cmd.Parameters.AddWithValue("@locator", ObjComp.Locator)
            cmd.Parameters.AddWithValue("@comp_type", Me.ComponentTypeValue)
            cmd.Parameters.AddWithValue("@orderno", ObjComp.OrderNo)
            cmd.Parameters.AddWithValue("@pageno", intPageno)
            cmd.Parameters.AddWithValue("@Date_From", Me.DateFrom)
            cmd.Parameters.AddWithValue("@Date_To", Me.DateTo)
            cmd.Parameters.AddWithValue("@Style", ObjComp.Style)
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            Me.Result = False
            If ComponentiserConstants.ShowErrorMessage Then
                Dim db_err As New bc_cs_error_log("bc_components_dal", "executesp_for_component", bc_cs_error_codes.DB_ERR, ex.Message, certificate)
            Else
                Dim otrace2 As New bc_cs_activity_log("bc_components_dal", "executesp_for_component", bc_cs_activity_codes.COMMENTARY, ex.Message, certificate)
            End If
        Finally
            If ComponentiserConstants.LogActivityFile Then Dim otrace1 As New bc_cs_activity_log("bc_components_dal", "executesp_for_component", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
            If Not (sqlcn Is Nothing) Then
                sqlcn.Close()
            End If
        End Try
    End Sub
    Public Sub executesp_for_table_component(ByVal sp_name As String, ByVal Locator As String, ByVal Rowno As Integer, ByVal Colno As Integer, _
            ByVal value As String, ByVal Style As String, ByVal attributeid As Integer, ByVal lang_id As Integer, ByRef certificate As bc_cs_security.certificate)
        If ComponentiserConstants.LogActivityFile Then Dim otrace As New bc_cs_activity_log("bc_components_dal", "executesp_for_table_component", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Dim strValue As String
        If value = "" Then
            strValue = ""
        Else
            strValue = value
        End If
        Dim sqlcn As New SqlConnection
        sqlcn.ConnectionString = gbc_db.GetConnectionString(certificate)
        Try
            '   Dim ocommentary = New bc_cs_activity_log("bc_am_component", "executesp_for_table_component", bc_cs_activity_codes.COMMENTARY, "Attempting to execute stored procedure: " + CStr(sp_name) + " for entity_id:" + Locator + " for entity_id:" + CStr(Rowno) + " for entity_id:" + CStr(Colno))
            sqlcn.Open()
            Dim cmd As New SqlCommand(sp_name, sqlcn)
            cmd.CommandType = Data.CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@doc_id", Me.DocumentId)
            cmd.Parameters.AddWithValue("@locator", Locator)
            cmd.Parameters.AddWithValue("@attributeid", attributeid)
            cmd.Parameters.AddWithValue("@row_no", Rowno)
            cmd.Parameters.AddWithValue("@col_no", Colno)
            cmd.Parameters.AddWithValue("@value", strValue.Trim)
            cmd.Parameters.AddWithValue("@style", Style.Trim)
            cmd.Parameters.AddWithValue("@Date_From", Me.DateFrom)
            cmd.Parameters.AddWithValue("@Date_To", Me.DateTo)
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            Me.Result = False
            If ComponentiserConstants.ShowErrorMessage Then
                Dim db_err As New bc_cs_error_log("bc_components_dal", "executesp_for_table_component", bc_cs_error_codes.DB_ERR, ex.Message, certificate)
            Else
                Dim otrace2 As New bc_cs_activity_log("bc_components_dal", "executesp_for_table_component", bc_cs_activity_codes.COMMENTARY, ex.Message, certificate)
            End If
        Finally
            If ComponentiserConstants.LogActivityFile Then Dim otrace1 As New bc_cs_activity_log("bc_components_dal", "executesp_for_table_component", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
            If Not (sqlcn Is Nothing) Then
                sqlcn.Close()
            End If
        End Try
    End Sub
    Public Sub executesp_for_master_table_component(ByVal sp_name As String, ByRef ObjComp As bc_am_component, _
          ByVal atttypeid As Integer, ByVal lang_id As Integer, ByRef certificate As bc_cs_security.certificate)
        If ComponentiserConstants.LogActivityFile Then Dim otrace As New bc_cs_activity_log("bc_components_dal", "executesp_for_master_table_component", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Dim intPageno As Integer = 0
        Dim sqlcn As New SqlConnection
        Dim strEntityId As String
        If ObjComp.EntityId = "" Then
            strEntityId = Me.EntityId
        Else
            strEntityId = ObjComp.EntityId
        End If
        sqlcn.ConnectionString = gbc_db.GetConnectionString(certificate)
        Try
            '  Dim ocommentary = New bc_cs_activity_log("bc_components_dal", "executesp_for_master_table_component", bc_cs_activity_codes.COMMENTARY, "Attempting to execute stored procedure: " + CStr(sp_name))
            sqlcn.Open()
            Dim cmd As New SqlCommand(sp_name, sqlcn)
            cmd.CommandType = Data.CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@doc_id", Me.DocumentId)
            cmd.Parameters.AddWithValue("@locator", ObjComp.Locator)
            cmd.Parameters.AddWithValue("@attributeid", atttypeid)
            cmd.Parameters.AddWithValue("@component_id", "")
            cmd.Parameters.AddWithValue("@comp_type", Me.ComponentTypeValue)
            cmd.Parameters.AddWithValue("@entity_id", strEntityId)
            cmd.Parameters.AddWithValue("@value", "")
            cmd.Parameters.AddWithValue("@Date_From", Me.DateFrom)
            cmd.Parameters.AddWithValue("@Date_To", Me.DateTo)
            cmd.Parameters.AddWithValue("@OrderNo", ObjComp.OrderNo)
            cmd.Parameters.AddWithValue("@PageNo", intPageno)
            cmd.Parameters.AddWithValue("@Style", ObjComp.Style)
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            Me.Result = False
            If ComponentiserConstants.ShowErrorMessage Then
                Dim db_err As New bc_cs_error_log("bc_components_dal", "executesp_for_master_table_component", bc_cs_error_codes.DB_ERR, ex.Message, certificate)
            Else
                Dim otrace2 As New bc_cs_activity_log("bc_components_dal", "executesp_for_master_table_component", bc_cs_activity_codes.COMMENTARY, ex.Message, certificate)
            End If
        Finally
            If ComponentiserConstants.LogActivityFile Then Dim otrace1 As New bc_cs_activity_log("bc_components_dal", "executesp_for_master_table_component", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
            If Not (sqlcn Is Nothing) Then
                sqlcn.Close()
            End If
        End Try
    End Sub

End Class
REM PR remoting fix sept 2009
<Serializable()> Public Class bc_om_component_table_cell
    Public RowValue As Integer
    Public ColumnValue As Integer
    Public Value As Object
    Public Imagebytes As Object
    Public Style As String

    Public Sub New()

    End Sub
End Class
