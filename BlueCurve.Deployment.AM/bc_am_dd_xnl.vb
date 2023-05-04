Imports System.Xml

Friend Class bc_am_dd_xnl
    Inherits XmlNodeList

    Dim alContents As ArrayList

    Public Overrides ReadOnly Property Count() As Integer
        Get
            Return alContents.Count
        End Get
    End Property

    Public Overrides Function GetEnumerator() As System.Collections.IEnumerator
        Return alContents.GetEnumerator()
    End Function

    Public Overrides Function Item(ByVal index As Integer) As System.Xml.XmlNode
        Return alContents.Item(index)
    End Function

    Public Sub Add(ByVal xn As XmlNode)
        alContents.Add(xn)
    End Sub

    Public Sub New()
        MyBase.New()
        alContents = New ArrayList        
    End Sub


End Class