Public Class bc_am_dd_key
    Implements IComparable

    Private oValue As Object
    Private strName As String

    Property KeyValue() As Object
        Get
            Return oValue
        End Get
        Set(ByVal oValue As Object)
            Me.oValue = oValue
        End Set
    End Property

    Property KeyName() As String
        Get
            Return strName
        End Get
        Set(ByVal strName As String)
            Me.strName = strName
        End Set
    End Property

    Public Sub New(ByVal strName As String, ByVal oValue As Object)
        KeyName = strName
        KeyValue = oValue
    End Sub

    Public Sub New(ByVal strName As String, ByRef xn As Xml.XmlNode)
        KeyName = strName
        If Not xn Is Nothing AndAlso Not xn.Attributes(KeyName) Is Nothing Then
            KeyValue = xn.Attributes(KeyName).Value
        End If
    End Sub

    Public Function CompareTo(ByVal obj As Object) As Integer Implements System.IComparable.CompareTo
        If TypeOf obj Is bc_am_dd_key Then
            If CType(obj, bc_am_dd_key).KeyName > KeyName Then
                Return -1
            Else
                Return 1
            End If
        Else
            Return 0
        End If
    End Function

End Class
