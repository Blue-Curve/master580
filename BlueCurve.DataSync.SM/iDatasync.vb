Public Interface iDatasync

    '''<summary>
    ''' Module:         BlueCurveLoader
    ''' Type:           lo
    ''' Desciption:     Interface for custom classes
    ''' Version:        1.0
    ''' Change history:
    ''' </summary>


    Function Process() As Boolean
    Sub Update()
    Property SyncId() As Integer
    Property SourceId() As String
    Property SourceTableName() As String
    Property SourceDBName() As String
    Property ChangeTypeCode() As Char
    Property WebService() As String
    Property ExternalMethodName() As String
    Property Processed() As DateTime
    Property ExceptionMessage() As String
    Property Retries() As Integer
    Property SyncStatus() As Integer

End Interface
