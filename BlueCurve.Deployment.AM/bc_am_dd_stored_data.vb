Imports System.Xml
Public Class bc_am_dd_stored_data

    Private strQuery, strTableName As String
    Private xdResults As XmlDocument
    Private xnlResults As XmlNodeList
    Private intIndex As Integer

    ReadOnly Property TableName() As String
        Get
            Return strTableName
        End Get
    End Property

    Property Index() As Integer
        Get
            Return intIndex
        End Get
        Set(ByVal intIndex As Integer)
            Me.intIndex = intIndex
        End Set
    End Property

    ReadOnly Property Nodes() As XmlNodeList
        Get
            Return xnlResults
        End Get
    End Property

    ReadOnly Property Xml() As XmlDocument
        Get
            Return xdResults
        End Get
    End Property

    ReadOnly Property Query() As Object
        Get
            Return strQuery
        End Get
    End Property

    'End properties

    Public Sub New(ByVal strQuery As String, ByVal strTableName As String, ByVal xdResults As XmlDocument)
        Me.strQuery = strQuery
        Me.strTableName = strTableName
        Me.xdResults = xdResults
        If xdResults Is Nothing Then
            xnlResults = New bc_am_dd_xnl
        Else
            xnlResults = xdResults.GetElementsByTagName(strTableName)
        End If
    End Sub

End Class
Public Class bc_am_dd_named_list
    Inherits SortedList

    Dim strName As String

    Property Name() As String
        Get
            Return strName
        End Get
        Set(ByVal strName As String)
            Me.strName = strName
        End Set
    End Property

    Public Sub New(ByVal strName As String)
        MyBase.New()
        Me.strName = strName
    End Sub

End Class