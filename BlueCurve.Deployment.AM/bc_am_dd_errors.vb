Imports BlueCurve.Core.OM
Imports BlueCurve.Core.CS
Imports System.Windows.Forms

Public Class bc_am_dd_errors

    Friend boolOverride As Boolean = False

    Private Sub bcancel_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles bcancel.MouseUp
        Dim ocommentary As New bc_cs_activity_log(Me.Name, "bcancel_MouseUp", bc_cs_activity_codes.COMMENTARY, "Cancel Query action")
        Me.Close()
    End Sub

    Private Sub bok_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles bok.MouseDown
        Me.bok.Image = Me.bokdisabled.Image
    End Sub

    Private Sub bok_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles bok.MouseUp
        boolOverride = True
        Me.Close()
    End Sub
End Class