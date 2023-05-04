Imports System
Imports System.Data
Imports System.Data.SqlClient

Imports System.Web


Public Class bc_lo_loadfactory

    '''<summary>
    ''' Module:         BlueCurveLoader
    ''' Type:           lo
    ''' Desciption:     File Loader
    ''' Version:        1.0
    ''' Change history:
    ''' </summary>

    Private _synctype As Char
    Private _logtitle As String = " "

    Public Sub New(ByVal syncType As Char)

        'SyncType: S = Service M = Manual
        _synctype = syncType

        If syncType = "M" Then
            _logtitle = "Manual Synchronisation"
        Else
            _logtitle = "Service Synchronisation"
        End If
    End Sub


    Public Sub CheckAndLoad()

        Dim objsync As iDatasync
        'Dim objWebService As New WebService()
        'Dim objWebTable As Object

        Dim countOk As Integer
        Dim countErr As Integer
        Dim InnerMessage As String = ""

        Try
            Dim SyncCRecords As New bc_lo_syncRecords

            SyncCRecords.Populate()


            If SyncCRecords.Count > 0 Then
                'Record how many records selected
                _accLog = New ActivityLogger
                _accLog.WriteToActivityLog(SyncCRecords.Count.ToString & " Records selected for Synchronisation", _logtitle)
                _accLog = Nothing
            End If

            '--------
            'Dim objWebService As New WebService()
            'Dim objtable As Object
            'Dim _result As String
            'objtable = objWebService.GetTable("company", "182", "GetCompanyById")
            '_result = CallByName(objtable, "CompanyFullName", CallType.Method)
            '_result = objtable.CompanyFullName
            '--------

            'Connect to web service proxy

            'Do what needs doing
            For Each objsync In SyncCRecords

                Try

                    'Get an object from the web service
                    'objWebTable = objWebService.GetTable(objsync.SourceTableName, objsync.SourceId, objsync.ExternalMethodName)

                    objsync.Process()

                    objsync.SyncStatus = 3
                    countOk += 1
                    objsync.Update()


                Catch ex As Exception

                    'Check Retries. If more than the NoOfRetries flag as an error
                    countErr += 1
                    objsync.Retries += 1
                    If objsync.Retries >= My.Settings.NoOfRetries Then
                        objsync.SyncStatus = 99
                        objsync.ExceptionMessage = ex.Message

                        Dim db_err As New ErrorLogger

                        If Not ex.InnerException Is Nothing Then
                            InnerMessage = ex.InnerException.Message
                        End If
                        db_err.WriteToErrorLog(ex.Message, InnerMessage, ex.StackTrace.ToString, "sync error")
                        db_err = Nothing
                    Else
                        objsync.SyncStatus = 0
                    End If

                    objsync.Update()
                End Try

                System.Threading.Thread.Sleep(1000)

            Next

            If countOk > 0 Or countErr > 0 Then
                'Record what was done
                _accLog = New ActivityLogger
                _accLog.WriteToActivityLog(countOk.ToString & " Records Ok " & countErr.ToString & " Exceptions", _logtitle.ToString)
                _accLog = Nothing
            End If

            'Clear up
            SyncCRecords = Nothing
            objsync = Nothing


        Catch ex As Exception
            If Not ex.InnerException Is Nothing Then
                InnerMessage = ex.InnerException.Message
            End If
            Dim db_err As New ErrorLogger
            db_err.WriteToErrorLog(ex.Message, InnerMessage, ex.StackTrace.ToString, "sync error")
            db_err = Nothing
        End Try

    End Sub

    Public Sub Testing()

        Dim testlog As New EventLog

        If Not EventLog.SourceExists("BlueCurveLoader") Then
            EventLog.CreateEventSource("BlueCurveLoader", "Application")
        End If

        testlog.Source = "BlueCurveLoader"
        testlog.WriteEntry("Testing loadfactory OK")

    End Sub

End Class


Public Class WebService

    '''<summary>
    ''' Module:         BlueCurveLoader
    ''' Type:           lo
    ''' Desciption:     Web Service handler
    ''' Version:        1.0
    ''' Change history:
    ''' </summary>

    Private _webProxy As Object
    'PR this os now in the cs layer
    'Private _webProxy As bluecurve.Proxy.WS.bc_cs_dynamic_web_proxy
    Private _wsvcClass As Object
    'Private WebService As ServiceReference1.DataTransferSoapClient
    Private WebTable As Object

    Public Sub New()

        'WebService = New ServiceReference1.DataTransferSoapClient
        Dim result As Boolean
        Dim webServiceASMX As String

        Try

            webServiceASMX = "/" + My.Settings.DefaultWebServiceName + ".asmx"
            'PR this is now in cs layer
            '_webProxy = New bluecurve.Proxy.WS.bc_cs_dynamic_web_proxy
            _webProxy._url = My.Settings.WebServiceURL + webServiceASMX
            _webProxy._servicename = My.Settings.DefaultWebServiceName

            If My.Settings.MultiWebServices = True Then
                result = _webProxy.load_proxy(True, _wsvcClass)
            Else
                result = _webProxy.load_proxy()
            End If

        Catch ex As Exception

            Throw ex

        End Try

    End Sub


    Public Sub New(ByVal WebServiceName As String)

        'WebService = New ServiceReference1.DataTransferSoapClient
        Dim result As Boolean
        Dim webServiceASMX As String
        Dim connectName As String
        Dim positionASMX As Integer

        Try

            positionASMX = WebServiceName.IndexOf(".asmx")

            If positionASMX = -1 Then
                webServiceASMX = "/" + WebServiceName.ToLower + ".asmx"
                connectName = WebServiceName
            Else
                webServiceASMX = "/" + WebServiceName.ToLower
                connectName = Left(WebServiceName, 12)
            End If

            ' _webProxy = New bluecurve.Proxy.WS.bc_cs_dynamic_web_proxy

            _webProxy._url = My.Settings.WebServiceURL + webServiceASMX
            _webProxy._servicename = connectName

            If My.Settings.MultiWebServices = True Then
                result = _webProxy.load_proxy(True, _wsvcClass)
            Else
                result = _webProxy.load_proxy()
            End If

        Catch ex As Exception

            Throw ex

        End Try

    End Sub

    Public Function GetTable(ByVal table As String, ByVal webParameter As Object, ByVal webServiceCall As String) As Object

        Dim callParameter() As Object
        callParameter = webParameter
        Dim WebTable As Object

        Try

            'Call the web service to get an object back

            If My.Settings.MultiWebServices = True Then
                WebTable = Me._webProxy.call_web_method(webServiceCall, callParameter, _wsvcClass)
            Else
                WebTable = Me._webProxy.call_web_method(webServiceCall, callParameter)
            End If

            GetTable = WebTable

        Catch ex As Exception

            Throw ex

        End Try

    End Function


    Public Sub CloseConnection()

        Try

            If My.Settings.MultiWebServices = True Then
                _wsvcClass = Nothing
            End If

            _webProxy = Nothing

        Catch ex As Exception

            Throw ex

        End Try

    End Sub



End Class


