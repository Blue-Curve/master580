Public Class SystemLoadfailedException
    Inherits System.ApplicationException

    Overrides ReadOnly Property Message() As String
        Get
            Return "System Failed To Load. Contact System Administrator!"
        End Get
    End Property

End Class