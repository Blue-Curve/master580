
Imports System
Imports System.Exception
Imports System.IO
Imports System.Diagnostics
Imports System.Collections
Imports System.Data
Imports System.Data.SqlClient
Imports System.Text
Imports System.Collections.Generic
Imports System.Xml
Imports System.Globalization.CultureInfo

'==========================================
' Bluecurve Limited 2011
' Module:         bc_data_loader
' Type:           AP
' Desciption:     Data Loader
' Public Methods: RemotePriceLoad
'                 
' Version:        1.0
' Change history:
'
'==========================================
Public Class bc_data_loader

    Public ActivityLog As ActivityLogger
    Public OutputFileName As String

    Public SettingConnectionString As String
    Public SettingRemoteSource As String
    Public SettingClientName As String
    Public SettingOutFilePath As String
    Public SettingTickerAttribute As String
    Public SettingTickerAttribute2 As String


    Public Sub New()

        ReadConfig()

    End Sub

    Public Function RemotePriceLoad() As Boolean

        Try
            Dim remoteTickers As bc_tickers = Nothing

            'Dim remoteCaller As New bc_yahoo_loader
            Dim remoteLoader As iLoader = Nothing
            Dim classType As Type
            Dim className As String = ""
            Dim clientName As String
            Dim remoteSource As String
            Dim succsess As Boolean = False

            'Get settings
            RemotePriceLoad = False

            'clientName = My.Settings.ClientName
            'remoteSource = My.Settings.RemoteSource
            clientName = SettingClientName
            remoteSource = SettingRemoteSource

            OutputFileName = NewFileName()

            'Create the correct object
            If clientName = "Drexel" And remoteSource = "Yahoo" Then
                className = "bc_yahoo_loader"
            End If

            If clientName = "WB" And remoteSource = "Yahoo" Then
                className = "bc_wb_yahoo_loader"
            End If

            If clientName = "UNLU" And remoteSource = "MATRIKS" Then
                className = "bc_unlu_xml_loader"
            End If


            classType = Type.GetType("BlueCurve.DataLoader.SM." + className)
            remoteLoader = Activator.CreateInstance(classType)
            remoteLoader.OutputFile = OutputFileName

            Dim urlCall As String
            Dim stockTicker As String = ""
            Dim returnText As String = "test"

            ActivityLog = New ActivityLogger
            ActivityLog.WriteToActivityLog("Price load started", "Data Loader")
            ActivityLog = Nothing

            If remoteSource = "MATRIKS" Then

                'Get Prices
                stockTicker = "Prices"

                'Build the url call to get the data
                urlCall = remoteLoader.BuildDataCall(stockTicker)
                ActivityLog = New ActivityLogger
                ActivityLog.WriteToActivityLog("Created url " & urlCall, "Data Loader")
                ActivityLog = Nothing

                'Get the data from the remote source
                returnText = GetRemoteData(urlCall, stockTicker)


                'Get ticker names
                remoteTickers = New bc_tickers
                remoteTickers.LoadTickers(SettingConnectionString, SettingTickerAttribute)
                ActivityLog = New ActivityLogger
                ActivityLog.WriteToActivityLog("Tickers loaded", "Data Loader")
                ActivityLog = Nothing

                For i = 1 To remoteTickers.Count
                    'Build the new files
                    remoteLoader.AppendData(returnText, remoteTickers.TickerCollection(i).Ticker, stockTicker)
                    ActivityLog = New ActivityLogger
                    ActivityLog.WriteToActivityLog("Output Created for " & stockTicker, "Data Loader")
                    ActivityLog = Nothing
                Next


                'Get FX Rates
                stockTicker = "FX Rates"

                'Build the url call to get the data
                urlCall = remoteLoader.BuildDataCall(stockTicker)
                ActivityLog = New ActivityLogger
                ActivityLog.WriteToActivityLog("Created url " & urlCall, "Data Loader")
                ActivityLog = Nothing

                'Get the data from the remote source
                returnText = GetRemoteData(urlCall, stockTicker)

                'Get ticker names
                remoteTickers = New bc_tickers
                remoteTickers.LoadTickers(SettingConnectionString, SettingTickerAttribute2)
                ActivityLog = New ActivityLogger
                ActivityLog.WriteToActivityLog("FX Tickers loaded", "Data Loader")
                ActivityLog = Nothing

                For i = 1 To remoteTickers.Count
                    'Build the new files
                    remoteLoader.AppendData(returnText, remoteTickers.TickerCollection(i).Ticker, stockTicker)
                    ActivityLog = New ActivityLogger
                    ActivityLog.WriteToActivityLog("Output Created for " & stockTicker, "Data Loader")
                    ActivityLog = Nothing
                Next

                'Get Index
                stockTicker = "Index"

                'Build the url call to get the data
                urlCall = remoteLoader.BuildDataCall(stockTicker)
                ActivityLog = New ActivityLogger
                ActivityLog.WriteToActivityLog("Created url " & urlCall, "Data Loader")
                ActivityLog = Nothing

                'Get the data from the remote source
                returnText = GetRemoteData(urlCall, stockTicker)
                'Get ticker names
                remoteTickers = New bc_tickers
                remoteTickers.LoadTickers(SettingConnectionString, SettingTickerAttribute2)
                ActivityLog = New ActivityLogger
                ActivityLog.WriteToActivityLog("Index Tickers loaded", "Data Loader")
                ActivityLog = Nothing

                For i = 1 To remoteTickers.Count
                    'Build the new files
                    remoteLoader.AppendData(returnText, remoteTickers.TickerCollection(i).Ticker, stockTicker)
                    ActivityLog = New ActivityLogger
                    ActivityLog.WriteToActivityLog("Output Created for " & stockTicker, "Data Loader")
                    ActivityLog = Nothing
                Next

            End If

            If remoteSource = "Yahoo" Then

                'Get ticker names
                remoteTickers = New bc_tickers
                remoteTickers.LoadTickers(SettingConnectionString, SettingTickerAttribute)
                ActivityLog = New ActivityLogger
                ActivityLog.WriteToActivityLog("Tickers loaded", "Data Loader")
                ActivityLog = Nothing

                'Process tickers to build output file
                For i = 1 To remoteTickers.Count
                    stockTicker = remoteTickers.TickerCollection(i).ticker

                    If Len(stockTicker) > 0 Then

                        'Build the url call to get the data
                        urlCall = remoteLoader.BuildDataCall(stockTicker)
                        ActivityLog = New ActivityLogger
                        ActivityLog.WriteToActivityLog("Created url " & urlCall, "Data Loader")
                        ActivityLog = Nothing

                        'Get the data from the remote source
                        returnText = GetRemoteData(urlCall, stockTicker)

                        'Build the new files
                        remoteLoader.AppendData(returnText, remoteTickers.TickerCollection(i).name, " ")
                        ActivityLog = New ActivityLogger
                        ActivityLog.WriteToActivityLog("Output Created for " & stockTicker, "Data Loader")
                        ActivityLog = Nothing

                    End If
                Next
            End If

            'REM Write output files
            succsess = remoteLoader.WriteFiles(SettingConnectionString, SettingOutFilePath)
            If succsess = True Then
                ActivityLog = New ActivityLogger
                ActivityLog.WriteToActivityLog("Output files writen to " & SettingOutFilePath, "Data Loader")
                ActivityLog = Nothing
            End If

            ActivityLog = New ActivityLogger
            ActivityLog.WriteToActivityLog("Price load ended", "Data Loader")
            ActivityLog = Nothing

            remoteLoader = Nothing
            RemotePriceLoad = True

        Catch ex As Exception

            Dim InnerMessage As String = ""
            If Not ex.InnerException Is Nothing Then
                InnerMessage = ex.InnerException.Message
            End If

            Dim db_err As New ErrorLogger
            db_err.WriteToErrorLog(ex.Message, InnerMessage, ex.StackTrace.ToString, "load error")
            db_err = Nothing
        End Try

    End Function

    Function GetRemoteData(ByVal url As String, ByVal stockTicker As String) As String

        Dim ResponseText As String = ""

        Try

            ActivityLog = New ActivityLogger
            ActivityLog.WriteToActivityLog("Starting to download data for " & stockTicker, "GetRemoteData")
            ActivityLog = Nothing

            Dim objHTTP As Object = Nothing

            'instantiage WinHttp instance
            'On Error Resume Next
            Try
                objHTTP = CreateObject("WinHTTP.WinHTTPrequest.5")
            Catch ex As Exception
                If Err.Number <> 0 Then
                    objHTTP = CreateObject("WinHTTP.WinHTTPrequest.5.1")
                End If
            End Try

            Dim success As Boolean
            success = False
            Dim retryCounter As Integer
            retryCounter = 0

            Dim maxNumberOfRetries As Integer
            maxNumberOfRetries = 5

            GetRemoteData = ""

            Do While success = False

                'Initialize an HTTP request.
                objHTTP.Open("GET", url, False)

                'Send the HTTP request.
                objHTTP.Send()

                ' get the HTTP response code
                Dim httpRequestStatus As Integer
                httpRequestStatus = objHTTP.Status
                If httpRequestStatus = 200 Then
                    success = True
                Else

                    ActivityLog = New ActivityLogger
                    ActivityLog.WriteToActivityLog("Making HTTP GET call to: " & url & " caused result code: " & httpRequestStatus, "GetRemoteData")
                    ActivityLog = Nothing

                    retryCounter = retryCounter + 1
                    If retryCounter > maxNumberOfRetries Then
                        Err.Raise(10001, "GetRemoteData ", "Unable to download data for " & stockTicker)
                        Exit Function
                    End If
                End If
            Loop

            ' Display the response text.
            ResponseText = objHTTP.ResponseText
            'MsgBox(ResponseText)
            'ActivityLog = New ActivityLogger
            'ActivityLog.WriteToActivityLog(ResponseText, "GetRemoteData")
            'ActivityLog = Nothing


            ActivityLog = New ActivityLogger
            ActivityLog.WriteToActivityLog("Downloaded data for " & stockTicker, "GetRemoteData")
            ActivityLog = Nothing

        Catch ex As Exception
            Dim InnerMessage As String = ""
            If Not ex.InnerException Is Nothing Then
                InnerMessage = ex.InnerException.Message
            End If

            Dim db_err As New ErrorLogger
            db_err.WriteToErrorLog(ex.Message, InnerMessage, ex.StackTrace.ToString, "load error")
            db_err = Nothing
        Finally
            GetRemoteData = ResponseText
        End Try

    End Function

    Public Function NewFileName() As String

        Dim vDate As Date
        vDate = Date.Now

        NewFileName = "In" & vDate.Year.ToString() & ((vDate - DateTime.Parse("01/01/" + vDate.Year.ToString())).Days + 1).ToString("000")
        NewFileName = NewFileName & vDate.Hour.ToString & vDate.Minute.ToString & vDate.Second.ToString

    End Function


    Public Sub ReadConfig()

        'Set the record collection
        ActivityLog = New ActivityLogger
        ActivityLog.WriteToActivityLog("Reading config", "GetRemoteData")
        ActivityLog = Nothing

        Dim xmlreader As Xml.XmlTextReader = Nothing
        'Check for the classname
        xmlreader = New Xml.XmlTextReader("C:\bluecurve\" & "BlueCurve.DataLoader.xml")
        Do While xmlreader.Read
            If xmlreader.Name = "setting" And xmlreader.GetAttribute("name") = "connectionstring" Then
                SettingConnectionString = xmlreader.GetAttribute("value")
            End If
            If xmlreader.Name = "setting" And xmlreader.GetAttribute("name") = "remotesource" Then
                SettingRemoteSource = xmlreader.GetAttribute("value")
            End If
            If xmlreader.Name = "setting" And xmlreader.GetAttribute("name") = "clientname" Then
                SettingClientName = xmlreader.GetAttribute("value")
            End If
            If xmlreader.Name = "setting" And xmlreader.GetAttribute("name") = "outfilepath" Then
                SettingOutFilePath = xmlreader.GetAttribute("value")
            End If
            If xmlreader.Name = "setting" And xmlreader.GetAttribute("name") = "tickerattribute" Then
                SettingTickerAttribute = xmlreader.GetAttribute("value")
            End If
            If xmlreader.Name = "setting" And xmlreader.GetAttribute("name") = "tickerattribute2" Then
                SettingTickerAttribute2 = xmlreader.GetAttribute("value")
            End If
        Loop
    End Sub

End Class

Friend Class bc_tickers
    Public TickerCollection As New Collection()

    Public Function Count() As Integer
        Count = TickerCollection.Count
    End Function

    Public Sub LoadTickers(ByVal ConnectionString As String, ByVal tickerAttribute As String)

        Dim TickerRecords As Data.SqlClient.SqlDataReader = Nothing
        Dim cnn As New SqlConnection(ConnectionString)
        Dim cmd As New SqlCommand("bcc_core_bp_get_tickers", cnn)

        Try

            'Get list of tickers
            TickerRecords = GetTickerRecords(cnn, cmd, tickerAttribute)

            'Set the record collection
            Dim tickerObject As bc_ticker = Nothing
            Dim xmlreader As Xml.XmlTextReader = Nothing

            While TickerRecords.Read

                tickerObject = New bc_ticker
                tickerObject.id = TickerRecords(TickerRecords.GetOrdinal("id"))
                tickerObject.Ticker = TickerRecords(TickerRecords.GetOrdinal("ticker"))
                tickerObject.Name = TickerRecords(TickerRecords.GetOrdinal("name"))

                'Add to collection
                TickerCollection.Add(tickerObject)

            End While

        Catch ex As Exception

            Throw ex

        Finally

            cnn.Close()
            cnn.Dispose()
            cmd.Dispose()

            If Not TickerRecords Is Nothing Then
                TickerRecords.Close()
                TickerRecords = Nothing
            End If

        End Try

    End Sub


    Friend Function GetTickerRecords(ByRef cnn As SqlConnection, ByRef cmd As SqlCommand, ByRef tickerAttribute As String) As Data.SqlClient.SqlDataReader

        Try

            Dim prm1 As SqlParameter

            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Clear()
            prm1 = cmd.Parameters.AddWithValue("@contributorid", 1)
            prm1 = cmd.Parameters.AddWithValue("@accountingid", 1)
            prm1 = cmd.Parameters.AddWithValue("@attributename", tickerAttribute)

            cnn.Open()
            Dim dr As SqlDataReader = cmd.ExecuteReader
            GetTickerRecords = dr

        Catch ex As Exception
            Throw ex

        End Try

    End Function

End Class


<Serializable()> Friend Class bc_ticker

    Public id As Long
    Public Exchange As String
    Public Ticker As String
    Public TickerFeedCode As String
    Public Name As String

    Public Overrides Function ToString() As String
        Return Ticker
    End Function

    Public Sub New()

    End Sub

End Class


Friend Class bc_insight_jobs

    Public Sub CreateJob(ByVal jobId As String, ByVal connectionString As String)

        Dim cnn As New SqlConnection(connectionString)
        Dim cmd As New SqlCommand("bcc_core_bp_write_job", cnn)

        Try

            Dim prm1 As SqlParameter

            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Clear()
            prm1 = cmd.Parameters.AddWithValue("@Jobid", jobId)

            cnn.Open()
            cmd.ExecuteReader()
            cnn.Close()

        Catch ex As Exception

            Throw ex

        Finally

            cnn.Close()
            cnn.Dispose()
            cmd.Dispose()

        End Try

    End Sub

End Class



Public Interface iLoader

    '''<summary>
    ''' Module:         BlueCurveLoader
    ''' Type:           lo
    ''' Desciption:     Interface for custom classes
    ''' Version:        1.0
    ''' Change history:
    ''' </summary>

    Property OutputFile() As String

    Function BuildDataCall(ByVal loadticker As String) As String
    Sub AppendData(ByVal sourcedata As String, ByVal entityname As String, ByVal filetype As String)
    Function WriteFiles(ByVal connectionString As String, ByVal outFilePath As String) As Boolean

End Interface

Public Class bc_yahoo_loader
    Implements iLoader

    Private outputFileName As String
    Private rowCount As Long = 0

    Private outputFileNameOpen As String
    Private outputFileNameHigh As String
    Private outputFileNameLow As String
    Private outputFileNameClose As String
    Private outputFileNameVolume As String
    Private outputFileNameAdj As String

    Private outputDataOpen As New StringBuilder()
    Private outputDataHigh As New StringBuilder()
    Private outputDataLow As New StringBuilder()
    Private outputDataClose As New StringBuilder()
    Private outputDataVolume As New StringBuilder()
    Private outputDataAdj As New StringBuilder()

    Public Property OutputFile() As String Implements iLoader.OutputFile
        Get
            OutputFile = outputFileName
        End Get
        Set(ByVal value As String)
            outputFileName = value

            outputFileNameOpen = outputFileName + "01"
            outputFileNameHigh = outputFileName + "02"
            outputFileNameLow = outputFileName + "03"
            outputFileNameClose = outputFileName + "04"
            outputFileNameVolume = outputFileName + "05"
            outputFileNameAdj = outputFileName + "06"

        End Set
    End Property

    Public Sub New()


    End Sub

    Public Function BuildDataCall(ByVal loadticker As String) As String Implements iLoader.BuildDataCall


        Dim yahoocall As String = "http://ichart.yahoo.com/table.csv?s="
        Dim dateToday As Date
        Dim date3years As Date

        Dim dateDay As Integer
        Dim dateMonth As Integer
        Dim dateYear As Integer

        Try

            dateToday = Date.Now
            date3years = DateAdd(DateInterval.Year, -3, dateToday)

            REM Add Ticker
            yahoocall = yahoocall + loadticker

            REM Add start date
            dateDay = Day(dateToday)
            dateMonth = Month(dateToday) - 1
            dateYear = Year(dateToday)

            yahoocall = yahoocall + "&d=" + Trim(dateMonth)
            yahoocall = yahoocall + "&e=" + Trim(dateDay)
            yahoocall = yahoocall + "&f=" + Trim(dateYear)

            REM Add end date
            dateDay = Day(date3years)
            dateMonth = Month(date3years) - 1
            dateYear = Year(date3years)

            yahoocall = yahoocall + "&a=" + Trim(dateMonth)
            yahoocall = yahoocall + "&b=" + Trim(dateDay)
            yahoocall = yahoocall + "&c=" + Trim(dateYear)

            REM Add period
            yahoocall = yahoocall + "&g=d"

            REM Add end bit
            yahoocall = yahoocall + "&ignore=.csv"

        Catch ex As Exception

            Throw ex

        Finally

            BuildDataCall = yahoocall

        End Try
    End Function


    Public Sub AppendData(ByVal sourcedata As String, ByVal entityname As String, ByVal filetype As String) Implements iLoader.AppendData

        'Build output data

        Dim priceArry() As String
        Dim outputDataBuilder As New StringBuilder()
        Dim priceLine As String
        Dim strReader As New StringReader(sourcedata)
        Dim dateTo As Date
        Dim dateFrom As Date
        Dim lineCount As Long = 0

        Try

            priceLine = strReader.ReadLine()
            While True
                priceLine = strReader.ReadLine()
                If priceLine Is Nothing Then
                    Exit While
                Else
                    priceArry = priceLine.Split(",")

                    lineCount += 1

                    'REM As of 01/07/2012 Yahoo is giving duplicate row for current date.
                    'Rem As of 09/08/2012 yahoo put feed back how it was.
                    'REM skip the first row
                    'If lineCount = 1 Then
                    '    Continue While
                    'End If
                    'rowCount += 1
                    'dateFrom = Date.Parse(priceArry(0))
                    'If lineCount = 2 Then
                    '    dateTo = Date.Parse("9999/09/09")
                    'Else
                    '    dateTo = DateAdd(DateInterval.Day, 1, dateFrom)
                    'End If

                    rowCount += 1

                    dateFrom = Date.Parse(priceArry(0))
                    If lineCount = 1 Then
                        dateTo = Date.Parse("9999/09/09")
                    Else
                        dateTo = DateAdd(DateInterval.Day, 1, dateFrom)
                    End If


                    'Open
                    outputDataOpen.Append(outputFileNameOpen)
                    outputDataOpen.Append(";")
                    outputDataOpen.Append(rowCount.ToString)
                    outputDataOpen.Append(";T;")
                    outputDataOpen.Append("[" & entityname & "]@[Instrument];")
                    outputDataOpen.Append("attribute.price_open;")
                    outputDataOpen.Append(Format(dateFrom, "dd/MM/yyyy"))
                    outputDataOpen.Append(";")
                    outputDataOpen.Append(Format(dateTo, "dd/MM/yyyy"))
                    outputDataOpen.Append(";;;;Publish;1;")
                    outputDataOpen.Append(priceArry(1).ToString)
                    outputDataOpen.Append(";;")
                    outputDataOpen.AppendLine()

                    'High
                    outputDataHigh.Append(outputFileNameHigh)
                    outputDataHigh.Append(";")
                    outputDataHigh.Append(rowCount.ToString)
                    outputDataHigh.Append(";T;")
                    outputDataHigh.Append("[" & entityname & "]@[Instrument];")
                    outputDataHigh.Append("attribute.price_high;")
                    outputDataHigh.Append(Format(dateFrom, "dd/MM/yyyy"))
                    outputDataHigh.Append(";")
                    outputDataHigh.Append(Format(dateTo, "dd/MM/yyyy"))
                    outputDataHigh.Append(";;;;Publish;1;")
                    outputDataHigh.Append(priceArry(2).ToString)
                    outputDataHigh.Append(";;")
                    outputDataHigh.AppendLine()

                    'Low
                    outputDataLow.Append(outputFileNameLow)
                    outputDataLow.Append(";")
                    outputDataLow.Append(rowCount.ToString)
                    outputDataLow.Append(";T;")
                    outputDataLow.Append("[" & entityname & "]@[Instrument];")
                    outputDataLow.Append("attribute.price_low;")
                    outputDataLow.Append(Format(dateFrom, "dd/MM/yyyy"))
                    outputDataLow.Append(";")
                    outputDataLow.Append(Format(dateTo, "dd/MM/yyyy"))
                    outputDataLow.Append(";;;;Publish;1;")
                    outputDataLow.Append(priceArry(3).ToString)
                    outputDataLow.Append(";;")
                    outputDataLow.AppendLine()

                    'Close
                    outputDataClose.Append(outputFileNameClose)
                    outputDataClose.Append(";")
                    outputDataClose.Append(rowCount.ToString)
                    outputDataClose.Append(";T;")
                    outputDataClose.Append("[" & entityname & "]@[Instrument];")
                    outputDataClose.Append("attribute.price_close;")
                    outputDataClose.Append(Format(dateFrom, "dd/MM/yyyy"))
                    outputDataClose.Append(";")
                    outputDataClose.Append(Format(dateTo, "dd/MM/yyyy"))
                    outputDataClose.Append(";;;;Publish;1;")
                    outputDataClose.Append(priceArry(4).ToString)
                    outputDataClose.Append(";;")
                    outputDataClose.AppendLine()

                    'Volume
                    outputDataVolume.Append(outputFileNameVolume)
                    outputDataVolume.Append(";")
                    outputDataVolume.Append(rowCount.ToString)
                    outputDataVolume.Append(";T;")
                    outputDataVolume.Append("[" & entityname & "]@[Instrument];")
                    outputDataVolume.Append("attribute.volume_daily;")
                    outputDataVolume.Append(Format(dateFrom, "dd/MM/yyyy"))
                    outputDataVolume.Append(";")
                    outputDataVolume.Append(Format(dateTo, "dd/MM/yyyy"))
                    outputDataVolume.Append(";;;;Publish;1;")
                    outputDataVolume.Append(priceArry(5).ToString)
                    outputDataVolume.Append(";;")
                    outputDataVolume.AppendLine()

                    'Adjusted
                    outputDataAdj.Append(outputFileNameAdj)
                    outputDataAdj.Append(";")
                    outputDataAdj.Append(rowCount.ToString)
                    outputDataAdj.Append(";T;")
                    outputDataAdj.Append("[" & entityname & "]@[Instrument];")
                    outputDataAdj.Append("attribute.core.price;")
                    outputDataAdj.Append(Format(dateFrom, "dd/MM/yyyy"))
                    outputDataAdj.Append(";")
                    outputDataAdj.Append(Format(dateTo, "dd/MM/yyyy"))
                    outputDataAdj.Append(";;;;Publish;1;")
                    outputDataAdj.Append(priceArry(6).ToString)
                    outputDataAdj.Append(";;")
                    outputDataAdj.AppendLine()

                End If
            End While

        Catch ex As Exception
            Throw ex
        End Try

    End Sub


    Public Function WriteFiles(ByVal connectionString As String, ByVal outFilePath As String) As Boolean Implements iLoader.WriteFiles

        REM Write output files
        WriteFiles = False

        Dim InsightJobs As bc_insight_jobs
        InsightJobs = New bc_insight_jobs

        Try

            REM write open
            Using outfile As New StreamWriter(outFilePath & outputFileNameOpen & ".txt")
                outfile.Write(Me.outputDataOpen.ToString)
            End Using
            InsightJobs.CreateJob(outputFileNameOpen, connectionString)

            REM write High
            Using outfile As New StreamWriter(outFilePath & outputFileNameHigh & ".txt")
                outfile.Write(Me.outputDataHigh.ToString)
                outfile.Close()
            End Using
            InsightJobs.CreateJob(outputFileNameHigh, connectionString)

            REM write Low
            Using outfile As New StreamWriter(outFilePath & outputFileNameLow & ".txt")
                outfile.Write(Me.outputDataLow.ToString)
                outfile.Close()
            End Using
            InsightJobs.CreateJob(outputFileNameLow, connectionString)

            REM write Close
            Using outfile As New StreamWriter(outFilePath & outputFileNameClose & ".txt")
                outfile.Write(Me.outputDataClose.ToString)
                outfile.Close()
            End Using
            InsightJobs.CreateJob(outputFileNameClose, connectionString)

            REM write volume
            Using outfile As New StreamWriter(outFilePath & outputFileNameVolume & ".txt")
                outfile.Write(Me.outputDataVolume.ToString)
                outfile.Close()
            End Using
            InsightJobs.CreateJob(outputFileNameVolume, connectionString)

            REM write adj
            Using outfile As New StreamWriter(outFilePath & outputFileNameAdj & ".txt")
                outfile.Write(Me.outputDataAdj.ToString)
                outfile.Close()
            End Using
            InsightJobs.CreateJob(outputFileNameAdj, connectionString)

            InsightJobs = Nothing

        Catch ex As Exception
            Throw ex
        Finally
            WriteFiles = True

        End Try
    End Function


End Class


Public Class bc_wb_yahoo_loader
    Implements iLoader

    Private outputFileName As String
    Private rowCount As Long = 0

    Private outputFileNameOpen As String
    Private outputFileNameHigh As String
    Private outputFileNameLow As String
    Private outputFileNameClose As String
    Private outputFileNameVolume As String
    Private outputFileNameAdj As String

    Private outputDataOpen As New StringBuilder()
    Private outputDataHigh As New StringBuilder()
    Private outputDataLow As New StringBuilder()
    Private outputDataClose As New StringBuilder()
    Private outputDataVolume As New StringBuilder()
    Private outputDataAdj As New StringBuilder()

    Public Property OutputFile() As String Implements iLoader.OutputFile
        Get
            OutputFile = outputFileName
        End Get
        Set(ByVal value As String)
            outputFileName = value

            outputFileNameOpen = outputFileName + "01"
            outputFileNameHigh = outputFileName + "02"
            outputFileNameLow = outputFileName + "03"
            outputFileNameClose = outputFileName + "04"
            outputFileNameVolume = outputFileName + "05"
            outputFileNameAdj = outputFileName + "06"

        End Set
    End Property

    Public Sub New()


    End Sub

    Public Function BuildDataCall(ByVal loadticker As String) As String Implements iLoader.BuildDataCall


        Dim yahoocall As String = "http://ichart.yahoo.com/table.csv?s="
        Dim dateToday As Date
        Dim date3years As Date

        Dim dateDay As Integer
        Dim dateMonth As Integer
        Dim dateYear As Integer

        Try

            dateToday = Date.Now
            date3years = DateAdd(DateInterval.Year, -3, dateToday)

            REM Add Ticker
            yahoocall = yahoocall + loadticker

            REM Add start date
            dateDay = Day(dateToday)
            dateMonth = Month(dateToday) - 1
            dateYear = Year(dateToday)

            yahoocall = yahoocall + "&d=" + Trim(dateMonth)
            yahoocall = yahoocall + "&e=" + Trim(dateDay)
            yahoocall = yahoocall + "&f=" + Trim(dateYear)

            REM Add end date
            dateDay = Day(date3years)
            dateMonth = Month(date3years) - 1
            dateYear = Year(date3years)

            yahoocall = yahoocall + "&a=" + Trim(dateMonth)
            yahoocall = yahoocall + "&b=" + Trim(dateDay)
            yahoocall = yahoocall + "&c=" + Trim(dateYear)

            REM Add period
            yahoocall = yahoocall + "&g=d"

            REM Add end bit
            yahoocall = yahoocall + "&ignore=.csv"

        Catch ex As Exception

            Throw ex

        Finally

            BuildDataCall = yahoocall

        End Try
    End Function


    Public Sub AppendData(ByVal sourcedata As String, ByVal entityname As String, ByVal filetype As String) Implements iLoader.AppendData

        'Build output data

        Dim priceArry() As String
        Dim outputDataBuilder As New StringBuilder()
        Dim priceLine As String
        Dim strReader As New StringReader(sourcedata)
        Dim dateTo As Date
        Dim dateFrom As Date
        Dim lineCount As Long = 0

        Try

            priceLine = strReader.ReadLine()
            While True
                priceLine = strReader.ReadLine()
                If priceLine Is Nothing Then
                    Exit While
                Else
                    priceArry = priceLine.Split(",")

                    lineCount += 1

                    'REM As of 01/07/2012 Yahoo is giving duplicate row for current date.
                    'Rem As of 09/08/2012 yahoo put feed back how it was.
                    'REM skip the first row
                    'If lineCount = 1 Then
                    '    Continue While
                    'End If
                    'rowCount += 1
                    'dateFrom = Date.Parse(priceArry(0))
                    'If lineCount = 2 Then
                    '    dateTo = Date.Parse("9999/09/09")
                    'Else
                    '    dateTo = DateAdd(DateInterval.Day, 1, dateFrom)
                    'End If

                    rowCount += 1

                    dateFrom = Date.Parse(priceArry(0))
                    If lineCount = 1 Then
                        dateTo = Date.Parse("9999/09/09")
                    Else
                        dateTo = DateAdd(DateInterval.Day, 1, dateFrom)
                    End If


                    'Open
                    outputDataOpen.Append(outputFileNameOpen)
                    outputDataOpen.Append(";")
                    outputDataOpen.Append(rowCount.ToString)
                    outputDataOpen.Append(";T;")
                    outputDataOpen.Append("[" & entityname & "]@[Instrument];")
                    outputDataOpen.Append("attribute.price_open;")
                    outputDataOpen.Append(Format(dateFrom, "dd/MM/yyyy"))
                    outputDataOpen.Append(";")
                    outputDataOpen.Append(Format(dateTo, "dd/MM/yyyy"))
                    outputDataOpen.Append(";;;;Publish;1;")
                    outputDataOpen.Append(priceArry(1).ToString)
                    outputDataOpen.Append(";;")
                    outputDataOpen.AppendLine()

                    'High
                    outputDataHigh.Append(outputFileNameHigh)
                    outputDataHigh.Append(";")
                    outputDataHigh.Append(rowCount.ToString)
                    outputDataHigh.Append(";T;")
                    outputDataHigh.Append("[" & entityname & "]@[Instrument];")
                    outputDataHigh.Append("attribute.price_high;")
                    outputDataHigh.Append(Format(dateFrom, "dd/MM/yyyy"))
                    outputDataHigh.Append(";")
                    outputDataHigh.Append(Format(dateTo, "dd/MM/yyyy"))
                    outputDataHigh.Append(";;;;Publish;1;")
                    outputDataHigh.Append(priceArry(2).ToString)
                    outputDataHigh.Append(";;")
                    outputDataHigh.AppendLine()

                    'Low
                    outputDataLow.Append(outputFileNameLow)
                    outputDataLow.Append(";")
                    outputDataLow.Append(rowCount.ToString)
                    outputDataLow.Append(";T;")
                    outputDataLow.Append("[" & entityname & "]@[Instrument];")
                    outputDataLow.Append("attribute.price_low;")
                    outputDataLow.Append(Format(dateFrom, "dd/MM/yyyy"))
                    outputDataLow.Append(";")
                    outputDataLow.Append(Format(dateTo, "dd/MM/yyyy"))
                    outputDataLow.Append(";;;;Publish;1;")
                    outputDataLow.Append(priceArry(3).ToString)
                    outputDataLow.Append(";;")
                    outputDataLow.AppendLine()

                    'Close
                    outputDataClose.Append(outputFileNameClose)
                    outputDataClose.Append(";")
                    outputDataClose.Append(rowCount.ToString)
                    outputDataClose.Append(";T;")
                    outputDataClose.Append("[" & entityname & "]@[Instrument];")
                    outputDataClose.Append("attribute.price_close;")
                    outputDataClose.Append(Format(dateFrom, "dd/MM/yyyy"))
                    outputDataClose.Append(";")
                    outputDataClose.Append(Format(dateTo, "dd/MM/yyyy"))
                    outputDataClose.Append(";;;;Publish;1;")
                    outputDataClose.Append(priceArry(4).ToString)
                    outputDataClose.Append(";;")
                    outputDataClose.AppendLine()

                    'Volume
                    outputDataVolume.Append(outputFileNameVolume)
                    outputDataVolume.Append(";")
                    outputDataVolume.Append(rowCount.ToString)
                    outputDataVolume.Append(";T;")
                    outputDataVolume.Append("[" & entityname & "]@[Instrument];")
                    outputDataVolume.Append("Average Daily Volume;")
                    outputDataVolume.Append(Format(dateFrom, "dd/MM/yyyy"))
                    outputDataVolume.Append(";")
                    outputDataVolume.Append(Format(dateTo, "dd/MM/yyyy"))
                    outputDataVolume.Append(";;;;Publish;1;")
                    outputDataVolume.Append(priceArry(5).ToString)
                    outputDataVolume.Append(";;")
                    outputDataVolume.AppendLine()

                    'Adjusted
                    outputDataAdj.Append(outputFileNameAdj)
                    outputDataAdj.Append(";")
                    outputDataAdj.Append(rowCount.ToString)
                    outputDataAdj.Append(";T;")
                    outputDataAdj.Append("[" & entityname & "]@[Instrument];")
                    outputDataAdj.Append("attribute.core.price;")
                    outputDataAdj.Append(Format(dateFrom, "dd/MM/yyyy"))
                    outputDataAdj.Append(";")
                    outputDataAdj.Append(Format(dateTo, "dd/MM/yyyy"))
                    outputDataAdj.Append(";;;;Publish;1;")
                    outputDataAdj.Append(priceArry(6).ToString)
                    outputDataAdj.Append(";;")
                    outputDataAdj.AppendLine()

                End If
            End While

        Catch ex As Exception
            Throw ex
        End Try

    End Sub


    Public Function WriteFiles(ByVal connectionString As String, ByVal outFilePath As String) As Boolean Implements iLoader.WriteFiles

        REM Write output files
        WriteFiles = False

        Dim InsightJobs As bc_insight_jobs
        InsightJobs = New bc_insight_jobs

        Try

            REM write open
            Using outfile As New StreamWriter(outFilePath & outputFileNameOpen & ".txt")
                outfile.Write(Me.outputDataOpen.ToString)
            End Using
            InsightJobs.CreateJob(outputFileNameOpen, connectionString)

            REM write High
            Using outfile As New StreamWriter(outFilePath & outputFileNameHigh & ".txt")
                outfile.Write(Me.outputDataHigh.ToString)
                outfile.Close()
            End Using
            InsightJobs.CreateJob(outputFileNameHigh, connectionString)

            REM write Low
            Using outfile As New StreamWriter(outFilePath & outputFileNameLow & ".txt")
                outfile.Write(Me.outputDataLow.ToString)
                outfile.Close()
            End Using
            InsightJobs.CreateJob(outputFileNameLow, connectionString)

            REM write Close
            Using outfile As New StreamWriter(outFilePath & outputFileNameClose & ".txt")
                outfile.Write(Me.outputDataClose.ToString)
                outfile.Close()
            End Using
            InsightJobs.CreateJob(outputFileNameClose, connectionString)

            'REM write volume
            'Using outfile As New StreamWriter(outFilePath & outputFileNameVolume & ".txt")
            '    outfile.Write(Me.outputDataVolume.ToString)
            '    outfile.Close()
            'End Using
            'InsightJobs.CreateJob(outputFileNameVolume, connectionString)

            REM write adj
            Using outfile As New StreamWriter(outFilePath & outputFileNameAdj & ".txt")
                outfile.Write(Me.outputDataAdj.ToString)
                outfile.Close()
            End Using
            InsightJobs.CreateJob(outputFileNameAdj, connectionString)

            InsightJobs = Nothing

        Catch ex As Exception
            Throw ex
        Finally
            WriteFiles = True

        End Try
    End Function


End Class

Public Class bc_unlu_xml_loader
    Implements iLoader

    Private outputFileName As String
    Private rowCount As Long = 0

    Private outputFileNameOpen As String
    Private outputFileNameHigh As String
    Private outputFileNameLow As String
    Private outputFileNameClose As String
    Private outputFileNameVolume As String
    Private outputFileNameFXRate As String
    Private outputFileNameIndex As String

    Private outputDataOpen As New StringBuilder()
    Private outputDataHigh As New StringBuilder()
    Private outputDataLow As New StringBuilder()
    Private outputDataClose As New StringBuilder()
    Private outputDataVolume As New StringBuilder()
    Private outputDataFXRate As New StringBuilder()
    Private outputDataIndex As New StringBuilder()


    Public Property OutputFile() As String Implements iLoader.OutputFile
        Get
            OutputFile = outputFileName
        End Get
        Set(ByVal value As String)
            outputFileName = value

            outputFileNameOpen = outputFileName + "01"
            outputFileNameHigh = outputFileName + "02"
            outputFileNameLow = outputFileName + "03"
            outputFileNameClose = outputFileName + "04"
            outputFileNameVolume = outputFileName + "05"
            outputFileNameFXRate = outputFileName + "06"
            outputFileNameIndex = outputFileName + "07"

        End Set
    End Property

    Public Sub New()


    End Sub

    Public Function BuildDataCall(ByVal loadticker As String) As String Implements iLoader.BuildDataCall

        Dim servicecall As String = ""

        Try

            If loadticker = "Prices" Then
                servicecall = "http://web.matriksdata.com/FinanceDataCenter/DataXML/DelayDBMatriks.aspx"
                servicecall = servicecall + "?PiyasaKod=4&AllData=true"
            End If

            If loadticker = "FX Rates" Then
                servicecall = "http://web.matriksdata.com/FinanceDataCenter/DataXML/OnlineDBMatriks.aspx?PiyasaKod=3&Sembol=(USDTRY,EURTRY)"
            End If

            If loadticker = "Index" Then
                servicecall = "http://web.matriksdata.com/FinanceDataCenter/DataXML/DelayDBMatriks.aspx?PiyasaKod=27&SEMBOL=(XU100)"
            End If

        Catch ex As Exception

            Throw ex

        Finally

            BuildDataCall = servicecall

        End Try
    End Function


    Public Sub AppendData(ByVal sourcedata As String, ByVal entityname As String, ByVal filetype As String) Implements iLoader.AppendData

        'Build output data

        Dim sonprice As String
        Dim tarihFrom As String
        Dim hacmiVolume As String
        Dim outputDataBuilder As New StringBuilder()
        Dim strReader As New StringReader(sourcedata)
        Dim dateTo As Date
        Dim dateFrom As Date
        Dim lineCount As Long = 0

        Try

            Dim xmlDoc As New XmlDocument, xmlSelectedNode As XmlNode
            xmlDoc.LoadXml(sourcedata)
            Dim root As XmlElement = xmlDoc.DocumentElement
            Dim MyCultureInfo As Globalization.CultureInfo = New Globalization.CultureInfo("en-GB")


            If filetype = "Prices" Then

                xmlSelectedNode = root.SelectSingleNode("data[sembol=""" & entityname & """]")

                If Not xmlSelectedNode Is Nothing Then

                    rowCount += 1

                    sonprice = xmlSelectedNode.SelectSingleNode("son").InnerText
                    sonprice = Replace(sonprice, ",", ".")

                    tarihFrom = xmlSelectedNode.SelectSingleNode("tarih").InnerText
                    dateFrom = Date.Parse(tarihFrom, MyCultureInfo)
                    dateTo = Date.Parse("9999/09/09")

                    hacmiVolume = xmlSelectedNode.SelectSingleNode("hacimlot").InnerText
                    hacmiVolume = Replace(hacmiVolume, ",", ".")


                    'Close
                    outputDataClose.Append(outputFileNameClose)
                    outputDataClose.Append(";")
                    outputDataClose.Append(rowCount.ToString)
                    outputDataClose.Append(";T;")
                    outputDataClose.Append(entityname & ";")
                    outputDataClose.Append("attribute.core.price;")
                    outputDataClose.Append(Format(dateFrom, "dd/MM/yyyy"))
                    outputDataClose.Append(";")
                    outputDataClose.Append(Format(dateTo, "dd/MM/yyyy"))
                    outputDataClose.Append(";;;;Publish;1;")
                    outputDataClose.Append(sonprice)
                    outputDataClose.Append(";;;")
                    outputDataClose.AppendLine()

                    'volume
                    outputDataVolume.Append(outputFileNameVolume)
                    outputDataVolume.Append(";")
                    outputDataVolume.Append(rowCount.ToString)
                    outputDataVolume.Append(";T;")
                    outputDataVolume.Append(entityname & ";")
                    outputDataVolume.Append("attribute.cp.Volume_Traded;")
                    outputDataVolume.Append(Format(dateFrom, "dd/MM/yyyy"))
                    outputDataVolume.Append(";")
                    outputDataVolume.Append(Format(dateTo, "dd/MM/yyyy"))
                    outputDataVolume.Append(";;;;Publish;1;")
                    outputDataVolume.Append(hacmiVolume)
                    outputDataVolume.Append(";;;")
                    outputDataVolume.AppendLine()

                End If

            End If


            If filetype = "FX Rates" Then
                xmlSelectedNode = root.SelectSingleNode("data[sembol=""" & entityname & """]")
                If Not xmlSelectedNode Is Nothing Then

                    rowCount += 1

                    sonprice = xmlSelectedNode.SelectSingleNode("son").InnerText
                    sonprice = Replace(sonprice, ",", ".")

                    tarihFrom = xmlSelectedNode.SelectSingleNode("tarih").InnerText
                    dateFrom = Date.Parse(tarihFrom, MyCultureInfo)
                    dateTo = Date.Parse("9999/09/09")

                    'FX Rate
                    outputDataFXRate.Append(outputFileNameFXRate)
                    outputDataFXRate.Append(";")
                    outputDataFXRate.Append(rowCount.ToString)
                    outputDataFXRate.Append(";T;")
                    outputDataFXRate.Append(entityname & ";")
                    outputDataFXRate.Append("attribute.core.exch_rate;")
                    outputDataFXRate.Append(Format(dateFrom, "dd/MM/yyyy"))
                    outputDataFXRate.Append(";")
                    outputDataFXRate.Append(Format(dateTo, "dd/MM/yyyy"))
                    outputDataFXRate.Append(";;;;Publish;1;")
                    outputDataFXRate.Append(sonprice)
                    outputDataFXRate.Append(";;;")
                    outputDataFXRate.AppendLine()

                End If

            End If

            If filetype = "Index" Then
                xmlSelectedNode = root.SelectSingleNode("data[sembol=""" & entityname & """]")
                If Not xmlSelectedNode Is Nothing Then

                    rowCount += 1

                    sonprice = xmlSelectedNode.SelectSingleNode("son").InnerText
                    sonprice = Replace(sonprice, ",", ".")

                    tarihFrom = xmlSelectedNode.SelectSingleNode("tarih").InnerText
                    dateFrom = Date.Parse(tarihFrom, MyCultureInfo)
                    dateTo = Date.Parse("9999/09/09")

                    'Index
                    outputDataIndex.Append(outputFileNameIndex)
                    outputDataIndex.Append(";")
                    outputDataIndex.Append(rowCount.ToString)
                    outputDataIndex.Append(";T;")
                    outputDataIndex.Append(entityname & ";")
                    outputDataIndex.Append("attribute.core.price;")
                    outputDataIndex.Append(Format(dateFrom, "dd/MM/yyyy"))
                    outputDataIndex.Append(";")
                    outputDataIndex.Append(Format(dateTo, "dd/MM/yyyy"))
                    outputDataIndex.Append(";;;;Publish;1;")
                    outputDataIndex.Append(sonprice)
                    outputDataIndex.Append(";;;")
                    outputDataIndex.AppendLine()
                End If
            End If

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function WriteFiles(ByVal connectionString As String, ByVal outFilePath As String) As Boolean Implements iLoader.WriteFiles

        REM Write output files
        WriteFiles = False

        Dim InsightJobs As bc_insight_jobs
        InsightJobs = New bc_insight_jobs

        Try

            REM write Close
            Using outfile As New StreamWriter(outFilePath & outputFileNameClose & ".txt")
                outfile.Write(Me.outputDataClose.ToString)
                outfile.Close()
            End Using
            InsightJobs.CreateJob(outputFileNameClose, connectionString)

            REM write volume
            Using outfile As New StreamWriter(outFilePath & outputFileNameVolume & ".txt")
                outfile.Write(Me.outputDataVolume.ToString)
                outfile.Close()
            End Using
            InsightJobs.CreateJob(outputFileNameVolume, connectionString)

            REM write FX rate
            Using outfile As New StreamWriter(outFilePath & outputFileNameFXRate & ".txt")
                outfile.Write(Me.outputDataFXRate.ToString)
                outfile.Close()
            End Using
            InsightJobs.CreateJob(outputFileNameFXRate, connectionString)

            REM write Index
            Using outfile As New StreamWriter(outFilePath & outputFileNameIndex & ".txt")
                outfile.Write(Me.outputDataIndex.ToString)
                outfile.Close()
            End Using
            InsightJobs.CreateJob(outputFileNameIndex, connectionString)
            InsightJobs = Nothing

        Catch ex As Exception
            Throw ex
        Finally
            WriteFiles = True

        End Try
    End Function


End Class