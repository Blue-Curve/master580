Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM

Public Class bc_am_entity_user_audit
    Implements Ibc_am_entity_user_audit

    Public Function load_view(items As bc_om_entity_user_audit, key_name As String) As Boolean Implements Ibc_am_entity_user_audit.load_view

        load_view = False
        Try
            Me.Text = "Audit for: " + key_name
            Me.uxaudit.BeginUnboundLoad()
            Me.uxaudit.Nodes.Clear()

            For i = 0 To items.items.Count - 1
                Me.uxaudit.Nodes.Add()
                Me.uxaudit.Nodes(i).SetValue(0, items.items(i).comment)
                Me.uxaudit.Nodes(i).SetValue(1, Format(items.items(i).date_from.ToLocalTime, "dd-MMM-yyyy HH:mm:ss"))
                Me.uxaudit.Nodes(i).SetValue(2, Format(items.items(i).date_to.ToLocalTime, "dd-MMM-yyyy HH:mm:ss"))
                Me.uxaudit.Nodes(i).SetValue(3, items.items(i).user)
            Next
            Me.uxaudit.EndUnboundLoad()
            load_view = True
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_entity_user_audit", "load_view", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try
    End Function
    Private Sub bclose_Click(sender As Object, e As EventArgs) Handles bclose.Click
        Me.Hide()
    End Sub

   
End Class
Public Class Cbc_am_entity_user_audit
    WithEvents _view As Ibc_am_entity_user_audit
   
    Public Function load_data(view As Ibc_am_entity_user_audit, key_id As Long, key_type As bc_om_entity_user_audit.EKEY_TYPE, key_name As String) As Boolean
        Dim items As New bc_om_entity_user_audit
        _view = view
        items.key_id = key_id
        items.key_type = key_type

        load_data = False
        Try
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                items.db_read()
            Else
                items.tmode = bc_cs_soap_base_class.tREAD
                If items.transmit_to_server_and_receive(items, True) = False Then
                    Exit Function
                End If
            End If
            If items.items.Count = 0 Then
                Dim omsg As New bc_cs_message("Blue Curve", "No audit information available", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Function
            End If

            load_data = _view.load_view(items, key_name)

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Cbc_am_entity_user_audit", "load_data", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Function

End Class
Public Interface Ibc_am_entity_user_audit
    Function load_view(items As bc_om_entity_user_audit, key_name As String) As Boolean
End Interface
