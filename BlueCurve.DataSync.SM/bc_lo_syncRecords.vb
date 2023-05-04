
Imports System
Imports System.Collections
Imports System.Data
Imports System.Data.SqlClient

Public Class bc_lo_syncRecords : Implements IEnumerable

    Private SyncCollection As New Collection()

    Public Function Count() As Integer
        Count = SyncCollection.Count
    End Function


    'Public Sub Populate_old()

    '    Dim syncRecords As Data.SqlClient.SqlDataReader

    '    Try

    '        'Get the list of Records to Process
    '        syncRecords = GetRecordsToSync()

    '        'Set the record collection

    '        Dim syncObject As iDatasync = Nothing

    '        While syncRecords.Read

    '            If My.Settings.Client = "ABG" Then

    '                Select Case syncRecords(syncRecords.GetOrdinal("sourcetablename"))
    '                    Case "company"
    '                        syncObject = New Company_abg_financial()
    '                    Case "country"
    '                        syncObject = New Country_abg_financial()
    '                    Case "user"
    '                        syncObject = New User_abg_financial()
    '                    Case "companyanalyst"
    '                        syncObject = New companyanalyst_abg_financial()
    '                    Case "usertype"
    '                        syncObject = New usertype_abg_financial()
    '                    Case "gicssector"
    '                        syncObject = New gicssector_abg_financial()
    '                    Case "pubtype"
    '                        syncObject = New pubtype_abg_financial()
    '                End Select
    '            End If

    '            syncObject.SyncId = syncRecords(syncRecords.GetOrdinal("syncid"))
    '            syncObject.SourceId = syncRecords(syncRecords.GetOrdinal("sourceid"))
    '            syncObject.SourceTableName = syncRecords(syncRecords.GetOrdinal("sourcetablename"))
    '            syncObject.SourceDBName = syncRecords(syncRecords.GetOrdinal("sourcedbname"))
    '            syncObject.ChangeTypeCode = syncRecords(syncRecords.GetOrdinal("changetypecode"))
    '            syncObject.ExternalMethodName = syncRecords(syncRecords.GetOrdinal("externalmethodname"))
    '            syncObject.Processed = syncRecords(syncRecords.GetOrdinal("processed"))
    '            syncObject.ExceptionMessage = syncRecords(syncRecords.GetOrdinal("exceptionmessage"))
    '            syncObject.Retries = syncRecords(syncRecords.GetOrdinal("retries"))
    '            syncObject.WebService = syncRecords(syncRecords.GetOrdinal("webservice"))
    '            syncObject.SyncStatus = syncRecords(syncRecords.GetOrdinal("syncstatus"))

    '            'Add to collection
    '            SyncCollection.Add(syncObject, syncRecords(syncRecords.GetOrdinal("syncid")))

    '            'Select Case SyncRecords(SyncRecords.GetOrdinal("sourcetablename"))
    '            '    Case "company"
    '            '        SyncCollection.Add(New Company_abg_financial(), SyncRecords(SyncRecords.GetOrdinal("syncid")))
    '            '    Case "country"
    '            '        SyncCollection.Add(New Country_abg_financial(), SyncRecords(SyncRecords.GetOrdinal("syncid")))
    '            'End Select


    '            'If SyncRecords(SyncRecords.GetOrdinal("sourcetablename")) = "company" Then
    '            '    SyncCollection.Add(New Company_abg_financial(), SyncRecords(SyncRecords.GetOrdinal("syncid")))
    '            'End If

    '        End While

    '        syncRecords.Close()
    '        syncRecords = Nothing


    '    Catch ex As Exception

    '        Throw ex
    '    End Try


    'End Sub


    Public Sub Populate()

        Dim syncRecords As Data.SqlClient.SqlDataReader = Nothing

        Dim cnn As New SqlConnection(My.Settings.ConnectionString)
        Dim cmd As New SqlCommand("bcc_core_cp_get_sync_data", cnn)

        Try

            'Get the list of Records to Process
            syncRecords = GetRecordsToSync(cnn, cmd)

            'Set the record collection
            Dim objectType As Type
            Dim syncObject As iDatasync = Nothing
            Dim xmlreader As Xml.XmlTextReader = Nothing
            Dim className As String

            While syncRecords.Read

                'Check for the classname
                className = ""
                xmlreader = New Xml.XmlTextReader(AppDomain.CurrentDomain.BaseDirectory() & "ServiceConfig.xml")
                Do While xmlreader.Read
                    If xmlreader.Name = "tablename" And xmlreader.GetAttribute("name") = syncRecords(syncRecords.GetOrdinal("sourcetablename")) Then
                        className = xmlreader.GetAttribute("classname")
                    End If
                Loop

                'Create an object of the corect type
                If My.Settings.Client = "ABG" Then
                    objectType = Type.GetType("BlueCurve.DataSync.SM." + className)
                    syncObject = Activator.CreateInstance(objectType)
                End If

                syncObject.SyncId = syncRecords(syncRecords.GetOrdinal("syncid"))
                syncObject.SourceId = syncRecords(syncRecords.GetOrdinal("sourceid"))
                syncObject.SourceTableName = syncRecords(syncRecords.GetOrdinal("sourcetablename"))
                syncObject.SourceDBName = syncRecords(syncRecords.GetOrdinal("sourcedbname"))
                syncObject.ChangeTypeCode = syncRecords(syncRecords.GetOrdinal("changetypecode"))
                syncObject.ExternalMethodName = syncRecords(syncRecords.GetOrdinal("externalmethodname"))
                syncObject.Processed = syncRecords(syncRecords.GetOrdinal("processed"))
                syncObject.ExceptionMessage = syncRecords(syncRecords.GetOrdinal("exceptionmessage"))
                syncObject.Retries = syncRecords(syncRecords.GetOrdinal("retries"))
                syncObject.WebService = syncRecords(syncRecords.GetOrdinal("webservice"))
                syncObject.SyncStatus = syncRecords(syncRecords.GetOrdinal("syncstatus"))

                'Add to collection
                SyncCollection.Add(syncObject, syncRecords(syncRecords.GetOrdinal("syncid")))

            End While

        Catch ex As Exception

            Throw ex

        Finally

            cnn.Close()
            cnn.Dispose()
            cmd.Dispose()

            syncRecords.Close()
            syncRecords = Nothing

        End Try


    End Sub



    Public Function GetRecordsToSync(ByRef cnn As SqlConnection, ByRef cmd As SqlCommand) As Data.SqlClient.SqlDataReader

        Try

            'Dim cnn As New SqlConnection(My.Settings.ConnectionString)
            'Dim cmd As New SqlCommand("bcc_core_cp_get_sync_data", cnn)

            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.Clear()
            Dim prm1 As SqlParameter = cmd.Parameters.AddWithValue("@batchsize", My.Settings.BatchSize)

            cnn.Open()

            Dim dr As SqlDataReader = cmd.ExecuteReader
            GetRecordsToSync = dr

            'cnn.Close()
            'cnn.Dispose()
            'cmd.Dispose()

        Catch ex As Exception
            Throw ex

        End Try

    End Function

    Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
        Return New SyncEnumerator(Me)
    End Function


    ReadOnly Property Item(ByVal Index As Integer) As Object

        Get
            Return SyncCollection(Index)
        End Get

    End Property

End Class


Public Class SyncEnumerator : Implements IEnumerator
    Dim parent As bc_lo_syncRecords
    Dim index As Integer

    Sub New(ByVal fr As bc_lo_syncRecords)
        Me.parent = fr
        index = 0
    End Sub

    Public ReadOnly Property Current() As Object Implements System.Collections.IEnumerator.Current
        Get
            Return parent.Item(index)

        End Get
    End Property

    Public Function MoveNext() As Boolean Implements System.Collections.IEnumerator.MoveNext

        index += 1
        If index > parent.Count Then
            Return False
        Else
            Return True
        End If

    End Function

    Public Sub Reset() Implements System.Collections.IEnumerator.Reset

    End Sub
End Class