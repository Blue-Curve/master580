Imports BlueCurve.Core.CS
Public Class bc_am_excel_data_load
    Public cancel_selected As Boolean = False
    Public ao_excel As Object
    Public templates As New bc_in_excel_io_templates
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click

    End Sub


    Private Sub bc_am_excel_data_load_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Me.Hide()
        Me.cancel_selected = True
    End Sub

    Private Sub Ltemplate_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Ltemplate.SelectedIndexChanged
        If Ltemplate.SelectedIndex > -1 Then
            Me.baction.Enabled = True
            If templates.templates(Me.Ltemplate.SelectedIndex).io = "input" Then
                Me.baction.Text = "Upload"
            Else
                Me.baction.Text = "Download"
            End If
        End If
    End Sub

    Private Sub baction_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles baction.Click
        Me.Hide()
    End Sub

    Private Sub bclose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bclose.Click
        Me.Hide()
    End Sub

    Private Sub bgoto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bgoto.Click
        Try
            If Me.Lvwarnings.SelectedItems.Count = 1 Then
                ao_excel.goto_cell(Me.Lvwarnings.SelectedItems(0).Text, CInt(Me.Lvwarnings.SelectedItems(0).SubItems(2).Text), CInt(Me.Lvwarnings.SelectedItems(0).SubItems(3).Text))
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("ba_am_exceldata_load", "bgoto_click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub

    Private Sub Lvwarnings_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Lvwarnings.SelectedIndexChanged
        Me.bgoto.Enabled = False
        If Me.Lvwarnings.SelectedItems.Count = 1 Then
            If IsNumeric(Me.Lvwarnings.SelectedItems(0).SubItems(2).Text) Then
                Me.bgoto.Enabled = True
            End If
        End If
    End Sub
End Class