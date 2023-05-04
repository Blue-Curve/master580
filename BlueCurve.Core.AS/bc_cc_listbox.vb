Imports System.Windows.Forms

Public Class bc_cc_listbox
    Inherits ListBox

    Private dtLastPress As DateTime = DateTime.Now
    Private strKeys As String

    Private Const WM_KEYDOWN As Integer = &H100
    Private Const WM_KEYUP As Integer = &H101
    Private Const WM_CHAR As Integer = &H102

    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)

        Select Case m.Msg
            Case WM_CHAR
                If DateTime.Now.Subtract(dtLastPress).Ticks / TimeSpan.TicksPerMillisecond < 300 Then
                    strKeys = strKeys & Chr(m.WParam)
                Else
                    strKeys = Chr(m.WParam)
                End If
                dtLastPress = DateTime.Now
                For Each oItem As Object In Items
                    If oItem.ToString.ToLower.IndexOf(strKeys.ToLower) = 0 Then
                        SelectedItem = oItem
                        Exit Sub
                    End If
                Next
            Case Else
                MyBase.WndProc(m)
        End Select

    End Sub

End Class
