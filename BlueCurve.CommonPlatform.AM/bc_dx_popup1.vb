Imports BlueCurve.Core.CS
Public Class bc_dx_att_popup1
    Implements Ibc_dx_att_popup
    Event save(type As Integer, tval As String, dval As String) Implements Ibc_dx_att_popup.save
    Dim _type As Integer
    Dim _length As Integer
    Public Function load_view(type As Integer, length As Integer, tval As String, dval As DateTime) As Boolean Implements Ibc_dx_att_popup.load_view
        Try
            _type = type
            _length = length
            Me.uxmemo.Visible = False
            Me.uxdate.Visible = False
            If type = 1 Then
                Me.uxmemo.Visible = True
                Me.uxmemo.Text = tval
                Me.uxmemo.Properties.MaxLength = length
                set_len()
            ElseIf type = 5 Then
                Me.uxdate.Visible = True
                Me.uxdate.DateTime = dval
                Me.Height = 83

            End If
            Return True
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_dx_att_popup", "load_view", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Function
    Sub set_len()
        Me.llen.Text = CStr(uxmemo.Text.Length) + " of " + CStr(_length)
    End Sub
    Private Sub bsave_Click(sender As Object, e As EventArgs) Handles bsave.Click

        RaiseEvent save(_type, Me.uxmemo.Text, Me.uxdate.DateTime)
        Me.Hide()
    End Sub
    Private Sub bcancel_Click(sender As Object, e As EventArgs) Handles bcancel.Click
        Me.Hide()
    End Sub


    Private Sub uxmemo_EditValueChanged(sender As Object, e As EventArgs) Handles uxmemo.EditValueChanged
        set_len()
        Me.bsave.Enabled = True
    End Sub

    Private Sub uxdate_EditValueChanged(sender As Object, e As EventArgs) Handles uxdate.EditValueChanged
        Me.bsave.Enabled = True
    End Sub
End Class
Public Class Cbc_dx_att_popup
    WithEvents _view As Ibc_dx_att_popup
    Public bsave As Boolean = False
    Public _tval As String
    Public _dval As DateTime
    Public Function load_data(view As Ibc_dx_att_popup, type As Integer, length As Integer, tval As String, dval As DateTime)
        _view = view
        Return _view.load_view(type, length, tval, dval)
    End Function
    Sub save(type As Integer, tval As String, dval As DateTime) Handles _view.save

        bsave = True
        If type = 1 Then
            _tval = tval
        ElseIf type = 5 Then
            _dval = dval
        End If
    End Sub
End Class
Public Interface Ibc_dx_att_popup
    Function load_view(type As Integer, length As Integer, tval As String, dval As DateTime) As Boolean
    Event save(type As Integer, tval As String, dval As String)
End Interface

