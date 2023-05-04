Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Data
Imports System.Data.SqlClient

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://bluecurve.core.datasyncnotify/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class BlueCurve_DataSyncNotifyWS
    Inherits System.Web.Services.WebService

    <WebMethod()> _
    Public Function TriggerSync(ByVal sourceId As String, ByVal sourceTableName As String, ByVal sourceDBName As String, ByVal changeTypeCode As String, ByVal externalMethodName As String, ByVal webService As String) As String

        Dim connectionString As String = Nothing

        Try

            'Dim cnn As New SqlConnection(My.Settings.ConnectionString)
            Dim connections As ConnectionStringSettingsCollection = ConfigurationManager.ConnectionStrings
            If connections.Count <> 0 Then
                For Each connection As ConnectionStringSettings In connections
                    If connection.Name = "BlueCurve.DataSyncMotify.My.MySettings.ConnectionString" Then
                        connectionString = connection.ConnectionString
                    End If
                Next
            End If

            Dim cnn As New SqlConnection(connectionString)
            SqlConnection.ClearPool(cnn)
            Dim cmd As New SqlCommand("bcc_core_cp_insert_sync_data", cnn)

            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.Clear()
            Dim prm1 As SqlParameter = cmd.Parameters.AddWithValue("@sourceid", sourceId)
            Dim prm2 As SqlParameter = cmd.Parameters.AddWithValue("@sourcetablename", sourceTableName)
            Dim prm3 As SqlParameter = cmd.Parameters.AddWithValue("@sourcedbname", sourceDBName)
            Dim prm4 As SqlParameter = cmd.Parameters.AddWithValue("@changetypecode", changeTypeCode)
            Dim prm5 As SqlParameter = cmd.Parameters.AddWithValue("@externalmethodname", externalMethodName)
            Dim prm6 As SqlParameter = cmd.Parameters.AddWithValue("@webservice", webService)

            cnn.Open()
            cmd.ExecuteReader()

            cnn.Close()
            cmd.Dispose()
            cnn.Dispose()

            Return "OK"

        Catch ex As Exception
            Return ex.Message
        End Try


    End Function


    <WebMethod()> _
   Public Function SyncAllRecordsByType(ByVal recordType As String) As String

        Dim connectionString As String = Nothing

        Try

            Dim storedProc As String = "bcc_abg_sync_allrecordsbyType"

            'Dim cnn As New SqlConnection(My.Settings.ConnectionString)
            Dim connections As ConnectionStringSettingsCollection = ConfigurationManager.ConnectionStrings
            If connections.Count <> 0 Then
                For Each connection As ConnectionStringSettings In connections
                    If connection.Name = "BlueCurve.DataSyncMotify.My.MySettings.ConnectionString" Then
                        connectionString = connection.ConnectionString
                    End If
                Next
            End If

            Dim cnn As New SqlConnection(connectionString)
            SqlConnection.ClearPool(cnn)
            Dim cmd As New SqlCommand(storedProc, cnn)

            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Clear()
            Dim prm1 As SqlParameter = cmd.Parameters.AddWithValue("@recordtype", recordType)

            cnn.Open()
            cmd.ExecuteReader()

            cnn.Close()
            cmd.Dispose()
            cnn.Dispose()

            Return "OK"

        Catch ex As Exception
            Return ex.Message
        End Try


    End Function


End Class

