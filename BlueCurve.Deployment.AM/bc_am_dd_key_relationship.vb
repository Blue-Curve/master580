Public Class bc_am_dd_key_relationship

    Dim alParents As New ArrayList
    Dim alChildren As New ArrayList
    Dim strText As String

    Public Sub New(ByVal strText As String)
        Me.strText = strText
    End Sub

    ReadOnly Property Text() As String
        Get
            Return strText
        End Get
    End Property

    Public Function GetDepth() As Integer
        If alParents.Count = 0 Then
            Return 0
        End If
        Dim intDepth As Integer = 0
        For Each badkr As bc_am_dd_key_relationship In alParents
            intDepth = Math.Max(intDepth, badkr.GetDepth())
        Next
        Return intDepth + 1
    End Function

    Friend Sub AddParent(ByVal badkr As bc_am_dd_key_relationship)
        If Not alParents.Contains(badkr) AndAlso Not alChildren.Contains(badkr) AndAlso Not Me Is badkr Then
            alParents.Add(badkr)
            badkr.AddChild(Me)
        End If
    End Sub

    Friend Sub AddChild(ByVal badkr As bc_am_dd_key_relationship)
        If Not alChildren.Contains(badkr) AndAlso Not alParents.Contains(badkr) AndAlso Not Me Is badkr Then
            alChildren.Add(badkr)
            badkr.AddParent(Me)
        End If
    End Sub

End Class
