Imports System.Data
Imports System.Data.SqlClient
Imports System.Xml

'-------------------------------------------------------------------------------------------------------------
'Place custom classes in here.
'-------------------------------------------------------------------------------------------------------------

Public Class company_abg_financial
    Inherits bc_base_sync_class
    Implements iDatasync

    '''<summary>
    ''' Module:         BlueCurveLoader
    ''' Type:           lo
    ''' Desciption:     ABG company xml load
    ''' Version:        1.0
    ''' Change history:
    ''' </summary>


    Public Overrides Function Process() As Boolean

        Const serviceConfigXML As String = "ServiceConfig.xml"
        Const loadTableName As String = "company"

        'Web Service
        Dim objWebService As New WebService(Me.WebService)
        Dim objWebResults As Object
        Dim callWebparameters(0) As Object

        'Datbase
        Dim loadFieldCount As Integer = 0
        Dim cnn As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing
        Dim prm1 As SqlParameter

        Dim processProperty As String
        Dim webValueCall As String
        Dim subObject As String
        Dim paramValue As Object
        Dim xmlObject As Object
        Dim xmlreader As XmlTextReader = Nothing

        Dim xmlResults As Xml.XmlDocument


        Try

            'Check if anything should be done
            xmlreader = New XmlTextReader(AppDomain.CurrentDomain.BaseDirectory() & serviceConfigXML)
            Do While xmlreader.Read
                If xmlreader.Name = "field" And xmlreader.GetAttribute("table") = loadTableName Then
                    loadFieldCount += 1
                End If
            Loop

            If loadFieldCount > 0 Then

                'Setup connection to database
                cnn = New SqlConnection(My.Settings.ConnectionString)
                SqlConnection.ClearPool(cnn)
                cmd = New SqlCommand("bcc_abg_company_update", cnn)
                cmd.CommandType = CommandType.StoredProcedure

                'Set up required parameters 
                cmd.Parameters.Clear()
                prm1 = cmd.Parameters.AddWithValue("@actiontype", Me.ChangeTypeCode)
                prm1 = cmd.Parameters.AddWithValue("@sourceid", Me.SourceId)


                If Me.ChangeTypeCode <> "D" Then

                    'Get an object from the web service
                    callWebparameters(0) = Convert.ToInt32(Me.SourceId)
                    objWebResults = objWebService.GetTable(Me.SourceTableName, callWebparameters, Me.ExternalMethodName)

                    'Set up xmldocument
                    xmlResults = New Xml.XmlDocument
                    xmlResults.LoadXml(objWebResults.ToString)
                    Dim xnodRoot As XmlNode = xmlResults.DocumentElement

                    'Trap Web service error
                    If xnodRoot.Name = "Error" Then
                        Err.Raise(514, , "Web Service Error: " + "Calling " + Me.ExternalMethodName + ".")
                    End If

                    xmlreader = New XmlTextReader(AppDomain.CurrentDomain.BaseDirectory() & serviceConfigXML)

                    'Set up parameters using xml from config
                    Do While xmlreader.Read
                        If xmlreader.Name = "field" And xmlreader.GetAttribute("table") = loadTableName Then

                            processProperty = xmlreader.GetAttribute("property")
                            webValueCall = xmlreader.GetAttribute("paramname")
                            subObject = xmlreader.GetAttribute("subobject")

                            If subObject <> "" Then
                                Dim xnodSub As XmlNode = xnodRoot.SelectSingleNode(subObject)
                                xmlObject = xnodSub.SelectSingleNode(webValueCall)
                                If xmlObject Is Nothing Then
                                    paramValue = ""
                                Else
                                    paramValue = xnodSub(webValueCall).InnerText
                                End If
                            Else
                                xmlObject = xnodRoot.SelectSingleNode(webValueCall)
                                If xmlObject Is Nothing Then
                                    paramValue = ""
                                Else
                                    paramValue = xnodRoot(webValueCall).InnerText
                                End If
                            End If

                            'Select Case subObject
                            '    Case "Country"
                            '        Dim xnodSub As XmlNode = xnodRoot.SelectSingleNode("Country")
                            '        'paramValue = CallByName(objWebResults.country, webValueCall, CallType.Method)
                            '        paramValue = xnodSub(webValueCall).InnerText

                            '    Case "GICSSubIndustry"
                            '        paramValue = CallByName(objWebResults.gicssubindustry, webValueCall, CallType.Method)
                            '    Case Else
                            '        paramValue = xnodRoot(webValueCall).InnerText
                            '        'paramValue = CallByName(objWebResults, webValueCall, CallType.Method)

                            'End Select

                            prm1 = cmd.Parameters.AddWithValue("@" & processProperty, paramValue)

                        End If
                        xmlreader.Read()
                    Loop

                End If

                'Do the call
                cnn.Open()
                cmd.ExecuteReader()
                cnn.Close()

                'cmd.Dispose()
                'cnn.Dispose()

            End If

        Catch ex As Exception

            Throw ex

        Finally

            cnn.Close()
            cmd.Dispose()
            cnn.Dispose()

            xmlreader.Close()
            objWebService.CloseConnection()
        End Try




    End Function


End Class


Public Class country_abg_financial
    Inherits bc_base_sync_class
    Implements iDatasync

    '''<summary>
    ''' Module:         BlueCurveLoader
    ''' Type:           lo
    ''' Desciption:     ABG country xml load
    ''' Version:        1.0
    ''' Change history:
    ''' </summary>


    Public Overrides Function Process() As Boolean

        Const serviceConfigXML As String = "ServiceConfig.xml"
        Const loadTableName As String = "country"

        'Web Service
        Dim objWebService As New WebService(Me.WebService)
        Dim objWebResults As Object
        Dim xmlResults As Xml.XmlDocument
        Dim callWebparameters(0) As Object

        'Datbase
        Dim loadFieldCount As Integer = 0
        Dim cnn As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing
        Dim prm1 As SqlParameter
        Dim processProperty As String
        Dim webValueCall As String
        Dim subObject As String
        Dim paramValue As Object
        Dim xmlObject As Object
        Dim xmlreader As XmlTextReader = Nothing

        Try

            'Check if anything should be loaded
            xmlreader = New XmlTextReader(AppDomain.CurrentDomain.BaseDirectory() & serviceConfigXML)
            Do While xmlreader.Read
                If xmlreader.Name = "field" And xmlreader.GetAttribute("table") = loadTableName Then
                    loadFieldCount += 1
                End If
            Loop


            If loadFieldCount > 0 = True Then

                'Connect to database
                cnn = New SqlConnection(My.Settings.ConnectionString)
                SqlConnection.ClearPool(cnn)
                cmd = New SqlCommand("bcc_abg_country_update", cnn)
                cmd.CommandType = CommandType.StoredProcedure

                'Set required parameters 
                cmd.Parameters.Clear()
                prm1 = cmd.Parameters.AddWithValue("@actiontype", Me.ChangeTypeCode)
                prm1 = cmd.Parameters.AddWithValue("@sourceid", Me.SourceId)

                If Me.ChangeTypeCode <> "D" Then

                    'Get an object from the web service
                    callWebparameters(0) = Convert.ToInt32(Me.SourceId)
                    objWebResults = objWebService.GetTable(Me.SourceTableName, callWebparameters, Me.ExternalMethodName)

                    'Set up xmldocument
                    xmlResults = New Xml.XmlDocument
                    xmlResults.LoadXml(objWebResults.ToString)
                    Dim xnodRoot As XmlNode = xmlResults.DocumentElement

                    'Trap Web service error
                    If xnodRoot.Name = "Error" Then
                        Err.Raise(514, , "Web Service Error: " + "Calling " + Me.ExternalMethodName + ".")
                    End If

                    xmlreader = New XmlTextReader(AppDomain.CurrentDomain.BaseDirectory() & serviceConfigXML)

                    'Set up parameters list using xml from config
                    Do While xmlreader.Read
                        If xmlreader.Name = "field" And xmlreader.GetAttribute("table") = loadTableName Then

                            processProperty = xmlreader.GetAttribute("property")
                            webValueCall = xmlreader.GetAttribute("paramname")
                            subObject = xmlreader.GetAttribute("subobject")

                            If subObject <> "" Then
                                Dim xnodSub As XmlNode = xnodRoot.SelectSingleNode(subObject)
                                xmlObject = xnodSub.SelectSingleNode(webValueCall)
                                If xmlObject Is Nothing Then
                                    paramValue = ""
                                Else
                                    paramValue = xnodSub(webValueCall).InnerText
                                End If
                            Else
                                xmlObject = xnodRoot.SelectSingleNode(webValueCall)
                                If xmlObject Is Nothing Then
                                    paramValue = ""
                                Else
                                    paramValue = xnodRoot(webValueCall).InnerText
                                End If
                            End If

                            prm1 = cmd.Parameters.AddWithValue("@" & processProperty, paramValue)

                        End If
                        xmlreader.Read()
                    Loop

                End If

                'Do the call
                cnn.Open()
                cmd.ExecuteReader()
                cnn.Close()

                'cmd.Dispose()
                'cnn.Dispose()

            End If

        Catch ex As Exception

            Throw ex

        Finally

            cnn.Close()
            cmd.Dispose()
            cnn.Dispose()

            xmlreader.Close()
            objWebService.CloseConnection()

        End Try


    End Function
End Class


Public Class user_abg_financial
    Inherits bc_base_sync_class
    Implements iDatasync

    '''<summary>
    ''' Module:         BlueCurveLoader
    ''' Type:           lo
    ''' Desciption:     ABG user load
    ''' Version:        1.0
    ''' Change history:
    ''' </summary>


    Public Overrides Function Process() As Boolean

        Const serviceConfigXML As String = "ServiceConfig.xml"
        Const loadTableName As String = "user"

        'Web Service
        Dim objWebService As New WebService(Me.WebService)
        Dim objWebResults As Object
        Dim callWebparameters(0) As Object

        'Datbase
        Dim loadFieldCount As Integer = 0
        Dim cnn As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing
        Dim prm1 As SqlParameter
        Dim processProperty As String
        Dim webValueCall As String
        Dim subObject As String
        Dim paramValue As Object
        Dim xmlObject As Object
        Dim xmlreader As XmlTextReader = Nothing

        Dim xmlResults As Xml.XmlDocument

        Try

            'Check if anything should be loaded
            xmlreader = New XmlTextReader(AppDomain.CurrentDomain.BaseDirectory() & serviceConfigXML)
            Do While xmlreader.Read
                If xmlreader.Name = "field" And xmlreader.GetAttribute("table") = loadTableName Then
                    loadFieldCount += 1
                End If
            Loop

            If loadFieldCount > 0 = True Then

                'Connect to database
                cnn = New SqlConnection(My.Settings.ConnectionString)
                SqlConnection.ClearPool(cnn)
                cmd = New SqlCommand("bcc_abg_user_update", cnn)
                cmd.CommandType = CommandType.StoredProcedure

                'Set required parameters
                cmd.Parameters.Clear()
                prm1 = cmd.Parameters.AddWithValue("@actiontype", Me.ChangeTypeCode)
                prm1 = cmd.Parameters.AddWithValue("@sourceid", Me.SourceId)

                If Me.ChangeTypeCode <> "D" Then

                    'Get an object from the web service
                    callWebparameters(0) = Me.SourceId
                    objWebResults = objWebService.GetTable(Me.SourceTableName, callWebparameters, Me.ExternalMethodName)

                    'Set up xmldocument
                    xmlResults = New Xml.XmlDocument
                    xmlResults.LoadXml(objWebResults.ToString)
                    Dim xnodRoot As XmlNode = xmlResults.DocumentElement

                    'Trap Web service error
                    If xnodRoot.Name = "Error" Then
                        Err.Raise(514, , "Web Service Error: " + "Calling " + Me.ExternalMethodName + ".")
                    End If

                    xmlreader = New XmlTextReader(AppDomain.CurrentDomain.BaseDirectory() & serviceConfigXML)


                    'Set up parameters list using xml from config
                    Do While xmlreader.Read
                        If xmlreader.Name = "field" And xmlreader.GetAttribute("table") = loadTableName Then

                            processProperty = xmlreader.GetAttribute("property")
                            webValueCall = xmlreader.GetAttribute("paramname")
                            subObject = xmlreader.GetAttribute("subobject")


                            If subObject <> "" Then
                                Dim xnodSub As XmlNode = xnodRoot.SelectSingleNode(subObject)
                                xmlObject = xnodSub.SelectSingleNode(webValueCall)
                                If xmlObject Is Nothing Then
                                    paramValue = ""
                                Else
                                    paramValue = xnodSub(webValueCall).InnerText
                                End If
                            Else
                                xmlObject = xnodRoot.SelectSingleNode(webValueCall)
                                If xmlObject Is Nothing Then
                                    paramValue = ""
                                Else
                                    paramValue = xnodRoot(webValueCall).InnerText
                                End If
                            End If


                            'Select Case subObject
                            '    Case "usertype"
                            '        paramValue = CallByName(objWebResults.usertype, webValueCall, CallType.Method)
                            '    Case "useroffice"
                            '        paramValue = CallByName(objWebResults.useroffice, webValueCall, CallType.Method)

                            '    Case Else
                            '        paramValue = CallByName(objWebResults, webValueCall, CallType.Method)

                            'End Select

                            prm1 = cmd.Parameters.AddWithValue("@" & processProperty, paramValue)

                        End If
                        xmlreader.Read()
                    Loop

                End If

                'Do the call
                cnn.Open()
                cmd.ExecuteReader()
                cnn.Close()

                'cmd.Dispose()
                'cnn.Dispose()

            End If

        Catch ex As Exception

            Throw ex

        Finally

            cnn.Close()
            cmd.Dispose()
            cnn.Dispose()

            xmlreader.Close()
            objWebService.CloseConnection()

        End Try


    End Function
End Class
Public Class companyanalyst_abg_financial
    Inherits bc_base_sync_class
    Implements iDatasync

    '''<summary>
    ''' Module:         BlueCurveLoader
    ''' Type:           lo
    ''' Desciption:     ABG anyalyst load
    ''' Version:        1.0
    ''' Change history:
    ''' </summary>


    Public Overrides Function Process() As Boolean

        Const serviceConfigXML As String = "ServiceConfig.xml"
        Const loadTableName As String = "companyanalyst"

        'Web Service
        Dim objWebService As New WebService(Me.WebService)
        Dim objWebResults As Object
        Dim callWebparameters(0) As Object

        'Datbase
        Dim loadFieldCount As Integer = 0
        Dim cnn As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing
        Dim prm1 As SqlParameter
        Dim processProperty As String
        Dim webValueCall As String
        Dim subObject As String
        Dim paramValue As Object
        Dim xmlObject As Object
        Dim xmlreader As XmlTextReader = Nothing

        Dim xmlResults As Xml.XmlDocument

        Dim AnalystNode As XmlNode

        Try

            'Check if anything should be loaded
            xmlreader = New XmlTextReader(AppDomain.CurrentDomain.BaseDirectory() & serviceConfigXML)
            Do While xmlreader.Read
                If xmlreader.Name = "field" And xmlreader.GetAttribute("table") = loadTableName Then
                    loadFieldCount += 1
                End If
            Loop


            'Get an object from the web service
            callWebparameters(0) = Convert.ToInt32(Me.SourceId)
            objWebResults = objWebService.GetTable(Me.SourceTableName, callWebparameters, Me.ExternalMethodName)

            'Set up xmldocument
            xmlResults = New Xml.XmlDocument
            xmlResults.LoadXml(objWebResults.ToString)
            Dim xnodRoot As XmlNode = xmlResults.DocumentElement

            'Trap Web service error
            If xnodRoot.Name = "Error" Then
                Err.Raise(514, , "Web Service Error: " + "Calling " + Me.ExternalMethodName + ".")
            End If

            For Each AnalystNode In xnodRoot

                If loadFieldCount > 0 = True Then

                    xmlreader = New XmlTextReader(AppDomain.CurrentDomain.BaseDirectory() & serviceConfigXML)

                    'Write Record to database
                    cnn = New SqlConnection(My.Settings.ConnectionString)
                    SqlConnection.ClearPool(cnn)
                    cmd = New SqlCommand("bcc_abg_companyanalyst_update", cnn)
                    cmd.CommandType = CommandType.StoredProcedure

                    'Set up parameters list using xml from config
                    cmd.Parameters.Clear()
                    prm1 = cmd.Parameters.AddWithValue("@actiontype", Me.ChangeTypeCode)
                    prm1 = cmd.Parameters.AddWithValue("@sourceid", Me.SourceId)

                    If Me.ExternalMethodName <> "D" Then

                        Do While xmlreader.Read
                            If xmlreader.Name = "field" And xmlreader.GetAttribute("table") = loadTableName Then

                                processProperty = xmlreader.GetAttribute("property")
                                webValueCall = xmlreader.GetAttribute("paramname")
                                subObject = xmlreader.GetAttribute("subobject")

                                If subObject <> "" Then
                                    Dim xnodSub As XmlNode = AnalystNode.SelectSingleNode(subObject)
                                    xmlObject = xnodSub.SelectSingleNode(webValueCall)
                                    If xmlObject Is Nothing Then
                                        paramValue = ""
                                    Else
                                        paramValue = xnodSub(webValueCall).InnerText
                                    End If
                                Else
                                    xmlObject = AnalystNode.SelectSingleNode(webValueCall)
                                    If xmlObject Is Nothing Then
                                        paramValue = ""
                                    Else
                                        paramValue = AnalystNode(webValueCall).InnerText
                                    End If
                                End If

                                'paramValue = CallByName(Analyst, webValueCall, CallType.Method)

                                prm1 = cmd.Parameters.AddWithValue("@" & processProperty, paramValue)

                            End If
                            xmlreader.Read()
                        Loop

                    End If

                    'Do the call
                    cnn.Open()
                    cmd.ExecuteReader()
                    cnn.Close()
                End If

                'cmd.Dispose()
                'cnn.Dispose()

            Next

        Catch ex As Exception

            Throw ex

        Finally

            cnn.Close()
            cmd.Dispose()
            cnn.Dispose()

            xmlreader.Close()
            objWebService.CloseConnection()

        End Try

    End Function
End Class


Public Class gicssector_abg_financial
    Inherits bc_base_sync_class
    Implements iDatasync

    '''<summary>
    ''' Module:         BlueCurveLoader
    ''' Type:           lo
    ''' Desciption:     ABG gicssector load
    ''' Version:        1.0
    ''' Change history:
    ''' </summary>


    Public Overrides Function Process() As Boolean

        Const serviceConfigXML As String = "ServiceConfig.xml"
        Const loadTableName As String = "gicssector"

        'Web Service
        Dim objWebService As New WebService(Me.WebService)
        Dim objWebResults As Object
        Dim callWebparameters(0) As Object

        'Datbase
        Dim loadFieldCount As Integer = 0
        Dim cnn As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing
        Dim prm1 As SqlParameter
        Dim processProperty As String
        Dim webValueCall As String
        Dim subObject As String
        Dim paramValue As Object
        Dim xmlObject As Object
        Dim xmlreader As XmlTextReader = Nothing

        Dim xmlResults As Xml.XmlDocument

        Try

            'Check if anything should be loaded
            xmlreader = New XmlTextReader(AppDomain.CurrentDomain.BaseDirectory() & serviceConfigXML)
            Do While xmlreader.Read
                If xmlreader.Name = "field" And xmlreader.GetAttribute("table") = loadTableName Then
                    loadFieldCount += 1
                End If
            Loop

            If loadFieldCount > 0 = True Then

                'Write Record to database
                cnn = New SqlConnection(My.Settings.ConnectionString)
                SqlConnection.ClearPool(cnn)
                cmd = New SqlCommand("bcc_abg_gicssector_update", cnn)
                cmd.CommandType = CommandType.StoredProcedure

                cmd.Parameters.Clear()
                prm1 = cmd.Parameters.AddWithValue("@actiontype", Me.ChangeTypeCode)
                prm1 = cmd.Parameters.AddWithValue("@sourceid", Me.SourceId)

                If Me.ChangeTypeCode <> "D" Then

                    'Get an object from the web service
                    callWebparameters(0) = Me.SourceId
                    objWebResults = objWebService.GetTable(Me.SourceTableName, callWebparameters, Me.ExternalMethodName)

                    'Set up xmldocument
                    xmlResults = New Xml.XmlDocument
                    xmlResults.LoadXml(objWebResults.ToString)
                    Dim xnodRoot As XmlNode = xmlResults.DocumentElement

                    'Trap Web service error
                    If xnodRoot.Name = "Error" Then
                        Err.Raise(514, , "Web Service Error: " + "Calling " + Me.ExternalMethodName + ".")
                    End If

                    xmlreader = New XmlTextReader(AppDomain.CurrentDomain.BaseDirectory() & serviceConfigXML)

                    'Set up parameters list using xml from config
                    Do While xmlreader.Read
                        If xmlreader.Name = "field" And xmlreader.GetAttribute("table") = loadTableName Then

                            processProperty = xmlreader.GetAttribute("property")
                            webValueCall = xmlreader.GetAttribute("paramname")
                            subObject = xmlreader.GetAttribute("subobject")

                            If subObject <> "" Then
                                Dim xnodSub As XmlNode = xnodRoot.SelectSingleNode(subObject)
                                xmlObject = xnodSub.SelectSingleNode(webValueCall)
                                If xmlObject Is Nothing Then
                                    paramValue = ""
                                Else
                                    paramValue = xnodSub(webValueCall).InnerText
                                End If
                            Else
                                xmlObject = xnodRoot.SelectSingleNode(webValueCall)
                                If xmlObject Is Nothing Then
                                    paramValue = ""
                                Else
                                    paramValue = xnodRoot(webValueCall).InnerText
                                End If
                            End If

                            prm1 = cmd.Parameters.AddWithValue("@" & processProperty, paramValue)

                        End If
                        xmlreader.Read()
                    Loop

                End If
                'Do the call
                cnn.Open()
                cmd.ExecuteReader()
                cnn.Close()

                'cmd.Dispose()
                'cnn.Dispose()

            End If

        Catch ex As Exception

            Throw ex

        Finally

            cnn.Close()
            cmd.Dispose()
            cnn.Dispose()

            xmlreader.Close()
            objWebService.CloseConnection()

        End Try


    End Function
End Class


Public Class pubtype_abg_financial
    Inherits bc_base_sync_class
    Implements iDatasync

    '''<summary>
    ''' Module:         BlueCurveLoader
    ''' Type:           lo
    ''' Desciption:     ABG pubtype pusher
    ''' Version:        1.0
    ''' Change history:
    ''' </summary>


    Public Overrides Function Process() As Boolean

        Const serviceConfigXML As String = "ServiceConfig.xml"
        Const loadTableName As String = "pubtype"

        'Web Service
        Dim objWebService As New WebService(Me.WebService)
        REM Dim objWebResults As Object
        Dim callWebparameters(1) As Object

        'Database
        Dim loadFieldCount As Integer = 0
        Dim cnn As SqlConnection
        Dim cmd As SqlCommand
        Dim prm1 As SqlParameter
        Dim processProperty As String
        Dim tableField As String
        Dim subObject As String
        Dim xmlreader As XmlTextReader = Nothing
        Dim paramCount As Integer

        Try

            'Check if anything should be sent
            xmlreader = New XmlTextReader(AppDomain.CurrentDomain.BaseDirectory() & serviceConfigXML)
            Do While xmlreader.Read
                If xmlreader.Name = "field" And xmlreader.GetAttribute("table") = loadTableName Then
                    loadFieldCount += 1
                End If
            Loop

            If loadFieldCount > 0 Then

                'Get the data from database to send to ABG
                cnn = New SqlConnection(My.Settings.ConnectionString)
                cmd = New SqlCommand("bcc_core_cp_get_pubtype_data", cnn)
                cmd.CommandType = CommandType.StoredProcedure

                prm1 = cmd.Parameters.AddWithValue("@sourceid", Me.SourceId)

                cnn.Open()
                Dim dr As SqlDataReader = cmd.ExecuteReader

                'Build web server callparameters list
                callWebparameters(0) = Me.SourceId

                paramCount = 0
                dr.Read()
                xmlreader = New XmlTextReader(AppDomain.CurrentDomain.BaseDirectory() & serviceConfigXML)
                Do While xmlreader.Read
                    If xmlreader.Name = "field" And xmlreader.GetAttribute("table") = loadTableName Then

                        processProperty = xmlreader.GetAttribute("property")
                        tableField = xmlreader.GetAttribute("paramname")
                        subObject = xmlreader.GetAttribute("subobject")

                        paramCount = +1
                        ReDim Preserve callWebparameters(paramCount)
                        callWebparameters(paramCount) = dr.Item(dr.GetOrdinal(tableField))

                    End If
                Loop

                xmlreader.Close()

                cnn.Close()
                cnn.Dispose()
                cmd.Dispose()

                'Call the web service
                'objWebResults = objWebService.GetTable(Me.SourceTableName, callWebparameters, Me.ExternalMethodName)

                xmlreader.Close()

            End If

            objWebService.CloseConnection()

        Catch ex As Exception

            Throw ex

        End Try


    End Function
End Class

Public Class usertype_abg_financial
    Inherits bc_base_sync_class
    Implements iDatasync

    '''<summary>
    ''' Module:         BlueCurveLoader
    ''' Type:           lo
    ''' Desciption:     ABG UserType load
    ''' Version:        1.0
    ''' Change history:
    ''' </summary>


    Public Overrides Function Process() As Boolean

        Const serviceConfigXML As String = "ServiceConfig.xml"
        Const loadTableName As String = "usertype"

        'Web Service
        Dim objWebService As New WebService(Me.WebService)
        Dim objWebResults As Object
        Dim callWebparameters(0) As Object

        'Datbase
        Dim loadFieldCount As Integer = 0
        Dim cnn As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing
        Dim prm1 As SqlParameter
        Dim processProperty As String
        Dim webValueCall As String
        Dim subObject As String
        Dim paramValue As Object
        Dim xmlObject As Object
        Dim xmlreader As XmlTextReader = Nothing

        Dim xmlResults As Xml.XmlDocument

        Try

            'Check if anything should be loaded
            xmlreader = New XmlTextReader(AppDomain.CurrentDomain.BaseDirectory() & serviceConfigXML)
            Do While xmlreader.Read
                If xmlreader.Name = "field" And xmlreader.GetAttribute("table") = loadTableName Then
                    loadFieldCount += 1
                End If
            Loop

            If loadFieldCount > 0 = True Then

                'Write Record to database
                cnn = New SqlConnection(My.Settings.ConnectionString)
                SqlConnection.ClearPool(cnn)
                cmd = New SqlCommand("bcc_abg_usertype_update", cnn)
                cmd.CommandType = CommandType.StoredProcedure

                cmd.Parameters.Clear()
                prm1 = cmd.Parameters.AddWithValue("@actiontype", Me.ChangeTypeCode)
                prm1 = cmd.Parameters.AddWithValue("@sourceid", Me.SourceId)

                If Me.ChangeTypeCode <> "D" Then

                    'Get an object from the web service
                    callWebparameters(0) = Convert.ToInt32(Me.SourceId)
                    objWebResults = objWebService.GetTable(Me.SourceTableName, callWebparameters, Me.ExternalMethodName)

                    'Set up xmldocument
                    xmlResults = New Xml.XmlDocument
                    xmlResults.LoadXml(objWebResults.ToString)
                    Dim xnodRoot As XmlNode = xmlResults.DocumentElement

                    'Trap Web service error
                    If xnodRoot.Name = "Error" Then
                        Err.Raise(514, , "Web Service Error: " + "Calling " + Me.ExternalMethodName + ".")
                    End If

                    xmlreader = New XmlTextReader(AppDomain.CurrentDomain.BaseDirectory() & serviceConfigXML)

                    'Set up parameters list using xml from config

                    Do While xmlreader.Read
                        If xmlreader.Name = "field" And xmlreader.GetAttribute("table") = loadTableName Then

                            processProperty = xmlreader.GetAttribute("property")
                            webValueCall = xmlreader.GetAttribute("paramname")
                            subObject = xmlreader.GetAttribute("subobject")


                            If subObject <> "" Then
                                Dim xnodSub As XmlNode = xnodRoot.SelectSingleNode(subObject)
                                xmlObject = xnodSub.SelectSingleNode(webValueCall)
                                If xmlObject Is Nothing Then
                                    paramValue = ""
                                Else
                                    paramValue = xnodSub(webValueCall).InnerText
                                End If
                            Else
                                xmlObject = xnodRoot.SelectSingleNode(webValueCall)
                                If xmlObject Is Nothing Then
                                    paramValue = ""
                                Else
                                    paramValue = xnodRoot(webValueCall).InnerText
                                End If
                            End If

                            prm1 = cmd.Parameters.AddWithValue("@" & processProperty, paramValue)

                        End If
                        xmlreader.Read()
                    Loop

                End If

                'Do the call
                cnn.Open()
                cmd.ExecuteReader()
                cnn.Close()

                'cmd.Dispose()
                'cnn.Dispose()

            End If

        Catch ex As Exception

            Throw ex

        Finally

            cnn.Close()
            cmd.Dispose()
            cnn.Dispose()

            xmlreader.Close()
            objWebService.CloseConnection()

        End Try


    End Function
End Class


Public Class extcomponent_abg_financial
    Inherits bc_base_sync_class
    Implements iDatasync

    '''<summary>
    ''' Module:         BlueCurveLoader
    ''' Type:           lo
    ''' Desciption:     ABG component xml load
    ''' Version:        1.0
    ''' Change history:
    ''' </summary>


    Public Overrides Function Process() As Boolean

        Const serviceConfigXML As String = "ServiceConfig.xml"
        Const loadTableName As String = "extcomponent"

        'Web Service
        Dim objWebService As New WebService(Me.WebService)
        Dim objWebResults As Object
        Dim xmlResults As Xml.XmlDocument
        Dim callWebparameters(0) As Object

        'Datbase
        Dim loadFieldCount As Integer = 0
        Dim cnn As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing
        Dim prm1 As SqlParameter
        Dim processProperty As String
        Dim webValueCall As String
        Dim subObject As String
        Dim paramValue As Object
        Dim xmlObject As Object
        Dim xmlreader As XmlTextReader = Nothing

        Dim InfoNode As XmlNode


        Dim InfoChild As XmlNode

        Try

            'Check if anything should be loaded
            xmlreader = New XmlTextReader(AppDomain.CurrentDomain.BaseDirectory() & serviceConfigXML)
            Do While xmlreader.Read
                If xmlreader.Name = "field" And xmlreader.GetAttribute("table") = loadTableName Then
                    loadFieldCount += 1
                End If
            Loop

            'Get an object from the web service
            callWebparameters(0) = Convert.ToInt32(Me.SourceId)
            objWebResults = objWebService.GetTable(Me.SourceTableName, callWebparameters, Me.ExternalMethodName)

            'Set up xmldocument
            xmlResults = New Xml.XmlDocument
            xmlResults.LoadXml(objWebResults.ToString)
            Dim xnodRoot As XmlNode = xmlResults.DocumentElement

            'Trap Web service error
            If xnodRoot.Name = "Error" Then
                Err.Raise(514, , "Web Service Error: " + "Calling " + Me.ExternalMethodName + ".")
            End If

            For Each InfoNode In xnodRoot

                If loadFieldCount > 0 = True Then

                    'Connect to database
                    cnn = New SqlConnection(My.Settings.ConnectionString)
                    SqlConnection.ClearPool(cnn)
                    cmd = New SqlCommand("bcc_abg_extcomponent_update", cnn)
                    cmd.CommandType = CommandType.StoredProcedure

                    'Set required parameters 
                    cmd.Parameters.Clear()
                    prm1 = cmd.Parameters.AddWithValue("@actiontype", Me.ChangeTypeCode)
                    prm1 = cmd.Parameters.AddWithValue("@sourceid", Me.SourceId)

                    If Me.ChangeTypeCode <> "D" Then

                        xmlreader = New XmlTextReader(AppDomain.CurrentDomain.BaseDirectory() & serviceConfigXML)

                        'Set up parameters list using xml from config
                        Do While xmlreader.Read
                            If xmlreader.Name = "field" And xmlreader.GetAttribute("table") = loadTableName Then

                                processProperty = xmlreader.GetAttribute("property")
                                webValueCall = xmlreader.GetAttribute("paramname")
                                subObject = xmlreader.GetAttribute("subobject")

                                If subObject <> "" Then
                                    Dim xnodSub As XmlNode = InfoNode.SelectSingleNode(subObject)
                                    xmlObject = xnodSub.SelectSingleNode(webValueCall)
                                    If xmlObject Is Nothing Then
                                        paramValue = ""
                                    Else
                                        paramValue = InfoNode(webValueCall).InnerText
                                    End If
                                Else
                                    xmlObject = InfoNode.SelectSingleNode(webValueCall)
                                    If xmlObject Is Nothing Then
                                        paramValue = ""
                                    Else
                                        paramValue = InfoNode(webValueCall).InnerText
                                    End If
                                End If

                                prm1 = cmd.Parameters.AddWithValue("@" & processProperty, paramValue)

                            End If
                            xmlreader.Read()
                        Loop


                    End If

                    'Do the call
                    cnn.Open()
                    cmd.ExecuteReader()
                    cnn.Close()

                End If


                '---------------------------------------
                'Check for child records and record them 
                '---------------------------------------
                Dim xnodChild As XmlNode = InfoNode.SelectSingleNode("InfoGroupChildren")

                If xnodChild IsNot Nothing Then
                    For Each InfoChild In xnodChild

                        'Connect to database
                        cnn = New SqlConnection(My.Settings.ConnectionString)
                        SqlConnection.ClearPool(cnn)
                        cmd = New SqlCommand("bcc_abg_extcomponent_update", cnn)
                        cmd.CommandType = CommandType.StoredProcedure

                        'Set required parameters 
                        cmd.Parameters.Clear()
                        prm1 = cmd.Parameters.AddWithValue("@actiontype", Me.ChangeTypeCode)
                        prm1 = cmd.Parameters.AddWithValue("@sourceid", Me.SourceId)

                        If Me.ChangeTypeCode <> "D" Then

                            xmlreader = New XmlTextReader(AppDomain.CurrentDomain.BaseDirectory() & serviceConfigXML)

                            'Set up parameters list using xml from config
                            Do While xmlreader.Read
                                If xmlreader.Name = "field" And xmlreader.GetAttribute("table") = loadTableName Then

                                    processProperty = xmlreader.GetAttribute("property")
                                    webValueCall = xmlreader.GetAttribute("paramname")
                                    subObject = xmlreader.GetAttribute("subobject")

                                    If subObject <> "" Then
                                        Dim xnodSub As XmlNode = InfoChild.SelectSingleNode(subObject)
                                        xmlObject = xnodSub.SelectSingleNode(webValueCall)
                                        If xmlObject Is Nothing Then
                                            paramValue = ""
                                        Else
                                            paramValue = InfoChild(webValueCall).InnerText
                                        End If
                                    Else
                                        xmlObject = InfoChild.SelectSingleNode(webValueCall)
                                        If xmlObject Is Nothing Then
                                            paramValue = ""
                                        Else
                                            paramValue = InfoChild(webValueCall).InnerText
                                        End If
                                    End If

                                    prm1 = cmd.Parameters.AddWithValue("@" & processProperty, paramValue)

                                End If
                                xmlreader.Read()
                            Loop


                        End If

                        'Do the call
                        cnn.Open()
                        cmd.ExecuteReader()
                        cnn.Close()

                    Next
                End If

            Next




        Catch ex As Exception

            Throw ex

        Finally

            cnn.Close()
            cmd.Dispose()
            cnn.Dispose()

            xmlreader.Close()
            objWebService.CloseConnection()

        End Try


    End Function
End Class


Public Class currency_abg_financial
    Inherits bc_base_sync_class
    Implements iDatasync

    '''<summary>
    ''' Module:         BlueCurveLoader
    ''' Type:           lo
    ''' Desciption:     ABG currency xml load
    ''' Version:        1.0
    ''' Change history:
    ''' </summary>


    Public Overrides Function Process() As Boolean

        Const serviceConfigXML As String = "ServiceConfig.xml"
        Const loadTableName As String = "currency"

        'Web Service
        Dim objWebService As New WebService(Me.WebService)
        Dim objWebResults As Object
        Dim callWebparameters(0) As Object

        'Datbase
        Dim loadFieldCount As Integer = 0
        Dim cnn As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing
        Dim prm1 As SqlParameter

        Dim processProperty As String
        Dim webValueCall As String
        Dim subObject As String
        Dim paramValue As Object
        Dim xmlObject As Object
        Dim xmlreader As XmlTextReader = Nothing

        Dim xmlResults As Xml.XmlDocument

        Try

            'Check if anything should be done
            xmlreader = New XmlTextReader(AppDomain.CurrentDomain.BaseDirectory() & serviceConfigXML)
            Do While xmlreader.Read
                If xmlreader.Name = "field" And xmlreader.GetAttribute("table") = loadTableName Then
                    loadFieldCount += 1
                End If
            Loop

            If loadFieldCount > 0 Then

                'Setup connection to database
                cnn = New SqlConnection(My.Settings.ConnectionString)
                SqlConnection.ClearPool(cnn)
                cmd = New SqlCommand("bcc_abg_currency_update", cnn)
                cmd.CommandType = CommandType.StoredProcedure

                'Set up required parameters 
                cmd.Parameters.Clear()
                prm1 = cmd.Parameters.AddWithValue("@actiontype", Me.ChangeTypeCode)
                prm1 = cmd.Parameters.AddWithValue("@sourceid", Me.SourceId)


                If Me.ChangeTypeCode <> "D" Then

                    'Get an object from the web service
                    callWebparameters(0) = Convert.ToInt32(Me.SourceId)
                    objWebResults = objWebService.GetTable(Me.SourceTableName, callWebparameters, Me.ExternalMethodName)

                    'Set up xmldocument
                    xmlResults = New Xml.XmlDocument
                    xmlResults.LoadXml(objWebResults.ToString)
                    Dim xnodRoot As XmlNode = xmlResults.DocumentElement

                    'Trap Web service error
                    If xnodRoot.Name = "Error" Then
                        Err.Raise(514, , "Web Service Error: " + "Calling " + Me.ExternalMethodName + ".")
                    End If

                    xmlreader = New XmlTextReader(AppDomain.CurrentDomain.BaseDirectory() & serviceConfigXML)


                    'Set up parameters using xml from config
                    Do While xmlreader.Read
                        If xmlreader.Name = "field" And xmlreader.GetAttribute("table") = loadTableName Then

                            processProperty = xmlreader.GetAttribute("property")
                            webValueCall = xmlreader.GetAttribute("paramname")
                            subObject = xmlreader.GetAttribute("subobject")

                            If subObject <> "" Then
                                Dim xnodSub As XmlNode = xnodRoot.SelectSingleNode(subObject)
                                xmlObject = xnodSub.SelectSingleNode(webValueCall)
                                If xmlObject Is Nothing Then
                                    paramValue = ""
                                Else
                                    paramValue = xnodSub(webValueCall).InnerText
                                End If
                            Else
                                xmlObject = xnodRoot.SelectSingleNode(webValueCall)
                                If xmlObject Is Nothing Then
                                    paramValue = ""
                                Else
                                    paramValue = xnodRoot(webValueCall).InnerText
                                End If
                            End If

                            prm1 = cmd.Parameters.AddWithValue("@" & processProperty, paramValue)

                        End If
                        xmlreader.Read()
                    Loop

                End If

                'Do the call
                cnn.Open()
                cmd.ExecuteReader()
                cnn.Close()

            End If

        Catch ex As Exception

            Throw ex

        Finally

            cnn.Close()
            cmd.Dispose()
            cnn.Dispose()

            xmlreader.Close()
            objWebService.CloseConnection()
        End Try


    End Function


End Class


Public Class office_abg_financial
    Inherits bc_base_sync_class
    Implements iDatasync

    '''<summary>
    ''' Module:         BlueCurveLoader
    ''' Type:           lo
    ''' Desciption:     ABG office xml load
    ''' Version:        1.0
    ''' Change history:
    ''' </summary>


    Public Overrides Function Process() As Boolean

        Const serviceConfigXML As String = "ServiceConfig.xml"
        Const loadTableName As String = "office"

        'Web Service
        Dim objWebService As New WebService(Me.WebService)
        Dim objWebResults As Object
        Dim callWebparameters(0) As Object

        'Datbase
        Dim loadFieldCount As Integer = 0
        Dim cnn As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing
        Dim prm1 As SqlParameter

        Dim processProperty As String
        Dim webValueCall As String
        Dim subObject As String
        Dim paramValue As Object
        Dim xmlObject As Object
        Dim xmlreader As XmlTextReader = Nothing

        Dim xmlResults As Xml.XmlDocument

        Try

            'Check if anything should be done
            xmlreader = New XmlTextReader(AppDomain.CurrentDomain.BaseDirectory() & serviceConfigXML)
            Do While xmlreader.Read
                If xmlreader.Name = "field" And xmlreader.GetAttribute("table") = loadTableName Then
                    loadFieldCount += 1
                End If
            Loop

            If loadFieldCount > 0 Then

                'Setup connection to database
                cnn = New SqlConnection(My.Settings.ConnectionString)
                SqlConnection.ClearPool(cnn)
                cmd = New SqlCommand("bcc_abg_office_update", cnn)
                cmd.CommandType = CommandType.StoredProcedure

                'Set up required parameters 
                cmd.Parameters.Clear()
                prm1 = cmd.Parameters.AddWithValue("@actiontype", Me.ChangeTypeCode)
                prm1 = cmd.Parameters.AddWithValue("@sourceid", Me.SourceId)


                If Me.ChangeTypeCode <> "D" Then

                    'Get an object from the web service
                    callWebparameters(0) = Convert.ToInt32(Me.SourceId)
                    objWebResults = objWebService.GetTable(Me.SourceTableName, callWebparameters, Me.ExternalMethodName)

                    'Set up xmldocument
                    xmlResults = New Xml.XmlDocument
                    xmlResults.LoadXml(objWebResults.ToString)
                    Dim xnodRoot As XmlNode = xmlResults.DocumentElement

                    'Trap Web service error
                    If xnodRoot.Name = "Error" Then
                        Err.Raise(514, , "Web Service Error: " + "Calling " + Me.ExternalMethodName + ".")
                    End If

                    xmlreader = New XmlTextReader(AppDomain.CurrentDomain.BaseDirectory() & serviceConfigXML)


                    'Set up parameters using xml from config
                    Do While xmlreader.Read
                        If xmlreader.Name = "field" And xmlreader.GetAttribute("table") = loadTableName Then

                            processProperty = xmlreader.GetAttribute("property")
                            webValueCall = xmlreader.GetAttribute("paramname")
                            subObject = xmlreader.GetAttribute("subobject")

                            If subObject <> "" Then
                                Dim xnodSub As XmlNode = xnodRoot.SelectSingleNode(subObject)
                                xmlObject = xnodSub.SelectSingleNode(webValueCall)
                                If xmlObject Is Nothing Then
                                    paramValue = ""
                                Else
                                    paramValue = xnodSub(webValueCall).InnerText
                                End If
                            Else
                                xmlObject = xnodRoot.SelectSingleNode(webValueCall)
                                If xmlObject Is Nothing Then
                                    paramValue = ""
                                Else
                                    paramValue = xnodRoot(webValueCall).InnerText
                                End If
                            End If

                            prm1 = cmd.Parameters.AddWithValue("@" & processProperty, paramValue)

                        End If
                        xmlreader.Read()
                    Loop

                End If

                'Do the call
                cnn.Open()
                cmd.ExecuteReader()
                cnn.Close()

            End If

        Catch ex As Exception

            Throw ex

        Finally

            cnn.Close()
            cmd.Dispose()
            cnn.Dispose()

            xmlreader.Close()
            objWebService.CloseConnection()
        End Try

    End Function


End Class

Public Class account_abg_financial
    Inherits bc_base_sync_class
    Implements iDatasync

    '''<summary>
    ''' Module:         BlueCurveLoader
    ''' Type:           lo
    ''' Desciption:     ABG office xml load
    ''' Version:        1.0
    ''' Change history:
    ''' </summary>


    Public Overrides Function Process() As Boolean

        Const serviceConfigXML As String = "ServiceConfig.xml"
        Const loadTableName As String = "account"

        'Web Service
        Dim objWebService As New WebService(Me.WebService)
        Dim objWebResults As Object
        Dim callWebparameters(0) As Object

        'Datbase
        Dim loadFieldCount As Integer = 0
        Dim cnn As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing
        Dim prm1 As SqlParameter

        Dim processProperty As String
        Dim webValueCall As String
        Dim subObject As String
        Dim paramValue As Object
        Dim xmlObject As Object
        Dim xmlreader As XmlTextReader = Nothing

        Dim xmlResults As Xml.XmlDocument

        Try

            'Check if anything should be done
            xmlreader = New XmlTextReader(AppDomain.CurrentDomain.BaseDirectory() & serviceConfigXML)
            Do While xmlreader.Read
                If xmlreader.Name = "field" And xmlreader.GetAttribute("table") = loadTableName Then
                    loadFieldCount += 1
                End If
            Loop

            If loadFieldCount > 0 Then

                'Setup connection to database
                cnn = New SqlConnection(My.Settings.ConnectionString)
                SqlConnection.ClearPool(cnn)
                cmd = New SqlCommand("bcc_abg_account_update", cnn)
                cmd.CommandType = CommandType.StoredProcedure

                'Set up required parameters 
                cmd.Parameters.Clear()
                prm1 = cmd.Parameters.AddWithValue("@actiontype", Me.ChangeTypeCode)
                prm1 = cmd.Parameters.AddWithValue("@sourceid", Me.SourceId)


                If Me.ChangeTypeCode <> "D" Then

                    'Get an object from the web service
                    callWebparameters(0) = Convert.ToInt32(Me.SourceId)
                    objWebResults = objWebService.GetTable(Me.SourceTableName, callWebparameters, Me.ExternalMethodName)

                    'Set up xmldocument
                    xmlResults = New Xml.XmlDocument
                    xmlResults.LoadXml(objWebResults.ToString)
                    Dim xnodRoot As XmlNode = xmlResults.DocumentElement

                    'Trap Web service error
                    If xnodRoot.Name = "Error" Then
                        Err.Raise(514, , "Web Service Error: " + "Calling " + Me.ExternalMethodName + ".")
                    End If

                    xmlreader = New XmlTextReader(AppDomain.CurrentDomain.BaseDirectory() & serviceConfigXML)


                    'Set up parameters using xml from config
                    Do While xmlreader.Read
                        If xmlreader.Name = "field" And xmlreader.GetAttribute("table") = loadTableName Then

                            processProperty = xmlreader.GetAttribute("property")
                            webValueCall = xmlreader.GetAttribute("paramname")
                            subObject = xmlreader.GetAttribute("subobject")

                            If subObject <> "" Then
                                Dim xnodSub As XmlNode = xnodRoot.SelectSingleNode(subObject)
                                xmlObject = xnodSub.SelectSingleNode(webValueCall)
                                If xmlObject Is Nothing Then
                                    paramValue = ""
                                Else
                                    paramValue = xnodSub(webValueCall).InnerText
                                End If
                            Else
                                xmlObject = xnodRoot.SelectSingleNode(webValueCall)
                                If xmlObject Is Nothing Then
                                    paramValue = ""
                                Else
                                    paramValue = xnodRoot(webValueCall).InnerText
                                End If
                            End If

                            prm1 = cmd.Parameters.AddWithValue("@" & processProperty, paramValue)

                        End If
                        xmlreader.Read()
                    Loop

                End If

                'Do the call
                cnn.Open()
                cmd.ExecuteReader()
                cnn.Close()

            End If

        Catch ex As Exception

            Throw ex

        Finally

            cnn.Close()
            cmd.Dispose()
            cnn.Dispose()

            xmlreader.Close()
            objWebService.CloseConnection()
        End Try

    End Function


End Class