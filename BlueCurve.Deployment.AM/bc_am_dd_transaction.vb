Imports System.Xml

Public Class bc_am_dd_transaction
    Implements IComparable

    Friend intDepth As Integer = 0
    Friend strType As String = ""
    Friend xn As XmlNode

    Public Sub New(ByVal xn As XmlNode)
        If Not xn.Attributes("depth") Is Nothing Then
            intDepth = xn.Attributes("depth").Value
        End If
        strType = xn.Attributes("type").Value.ToLower
        Me.xn = xn
    End Sub

    Public Function CompareTo(ByVal obj As Object) As Integer Implements System.IComparable.CompareTo
        If TypeOf obj Is bc_am_dd_transaction Then
            Dim badt As bc_am_dd_transaction = obj
            If strType = "delete" OrElse badt.strType = "delete" Then

                If strType = "delete" AndAlso badt.strType = "delete" Then
                    If intDepth < badt.intDepth Then
                        Return 1
                    ElseIf intDepth = badt.intDepth Then
                        Return 0
                    Else
                        Return -1
                    End If
                Else
                    If strType = "delete" Then
                        Return -1
                    Else
                        Return 1
                    End If
                End If

            Else
                If intDepth > badt.intDepth Then
                    Return 1
                ElseIf intDepth = badt.intDepth Then
                    Return 0
                Else
                    Return -1
                End If
            End If
        Else
            Return 0
        End If
    End Function
End Class
