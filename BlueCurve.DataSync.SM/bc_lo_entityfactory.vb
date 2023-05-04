Imports System.Data
Imports System.Data.SqlClient
Imports System.Xml

Public Class bc_base_sync_class
    Implements iDatasync

    '''<summary>
    ''' Module:         BlueCurveLoader
    ''' Type:           lo
    ''' Desciption:     Base Entity Clase.
    ''' (---- Do Not Change ----)
    ''' Version:        1.0
    ''' Change history:
    ''' </summary>

    Private _syncid As Integer
    Private _sourceid As String
    Private _sourceTableName As String
    Private _sourcedbName As String
    Private _changeTypeCode As Char
    Private _webService As String
    Private _externalMethodName As String
    Private _processed As Date
    Private _exceptionMessage As String
    Private _retries As Integer
    Private _syncStatus As Integer

    Public Property ChangeTypeCode() As Char Implements iDatasync.ChangeTypeCode
        Get
            ChangeTypeCode = _changeTypeCode
        End Get
        Set(ByVal value As Char)
            _changeTypeCode = value
        End Set
    End Property

    Public Property ExceptionMessage() As String Implements iDatasync.ExceptionMessage
        Get
            ExceptionMessage = _exceptionMessage
        End Get
        Set(ByVal value As String)
            _exceptionMessage = value
        End Set
    End Property

    Public Property ExternalMethodName() As String Implements iDatasync.ExternalMethodName
        Get
            ExternalMethodName = _externalMethodName
        End Get
        Set(ByVal value As String)
            _externalMethodName = value
        End Set
    End Property

    Public Overridable Function Process() As Boolean Implements iDatasync.Process

    End Function

    Public Property Processed() As Date Implements iDatasync.Processed
        Get
            Processed = _processed
        End Get
        Set(ByVal value As Date)
            _processed = value
        End Set
    End Property

    Public Property Retries() As Integer Implements iDatasync.Retries
        Get
            Retries = _retries
        End Get
        Set(ByVal value As Integer)
            _retries = value
        End Set
    End Property

    Public Property SyncStatus() As Integer Implements iDatasync.SyncStatus
        Get
            SyncStatus = _syncStatus
        End Get
        Set(ByVal value As Integer)
            _syncStatus = value
        End Set
    End Property

    Public Property SourceDBName() As String Implements iDatasync.SourceDBName
        Get
            SourceDBName = _sourcedbName
        End Get
        Set(ByVal value As String)
            _sourcedbName = value
        End Set
    End Property

    Public Property SourceId() As String Implements iDatasync.SourceId
        Get
            SourceId = _sourceid
        End Get
        Set(ByVal value As String)
            _sourceid = value
        End Set
    End Property

    Public Property SourceTableName() As String Implements iDatasync.SourceTableName
        Get
            SourceTableName = _sourceTableName
        End Get
        Set(ByVal value As String)
            _sourceTableName = value
        End Set
    End Property

    Public Property SyncId() As Integer Implements iDatasync.SyncId
        Get
            SyncId = _syncid
        End Get
        Set(ByVal value As Integer)
            _syncid = value
        End Set
    End Property


    Public Property WebService() As String Implements iDatasync.WebService
        Get
            WebService = _webService
        End Get
        Set(ByVal value As String)
            _webService = value

        End Set
    End Property


    Public Sub Update() Implements iDatasync.Update

        Try
            Dim cnn As New SqlConnection(My.Settings.ConnectionString)
            SqlConnection.ClearPool(cnn)
            Dim cmd As New SqlCommand("bcc_core_cp_update_sync_data", cnn)

            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.Clear()
            Dim prm1 As SqlParameter = cmd.Parameters.AddWithValue("@syncid", Me.SyncId)
            Dim prm2 As SqlParameter = cmd.Parameters.AddWithValue("@syncstatus", Me.SyncStatus)
            Dim prm3 As SqlParameter = cmd.Parameters.AddWithValue("@retries", Me.Retries)
            Dim prm4 As SqlParameter = cmd.Parameters.AddWithValue("@exceptionmessage", Me.ExceptionMessage)

            cnn.Open()
            cmd.ExecuteReader()

            cnn.Close()
            cmd.Dispose()
            cnn.Dispose()

        Catch ex As Exception
            Throw ex

        End Try

    End Sub

End Class



