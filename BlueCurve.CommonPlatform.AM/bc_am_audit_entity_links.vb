Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Public Class bc_am_audit_entity_links
    Implements Ibc_am_audit_entity_links

    Public Function load_view(links As bc_om_audit_links, class_name As String, entity_name As String, schema_name As String, audit_type As bc_om_audit_links.EAUDIT_TYPE) Implements Ibc_am_audit_entity_links.load_view
        load_view = False
        Try
            If audit_type = bc_om_audit_links.EAUDIT_TYPE.USERS_FOR_PREF Then
                Me.uxaudit.Columns(0).Caption = "User"
                Me.Text = "Audit for " + entity_name + " linked to  " + class_name
                Me.uxaudit.Columns(4).Caption = "user currently active"
            ElseIf audit_type = bc_om_audit_links.EAUDIT_TYPE.TAXONOMY Then
                Me.uxaudit.Columns(0).Caption = class_name
                Me.Text = "Audit for " + entity_name + " linked to  " + class_name + " for schema " + schema_name
                Me.uxaudit.Columns(4).Caption = class_name + " currently active"
            Else
                Me.uxaudit.Columns(0).Caption = "Entity"
                Me.uxaudit.Columns(6).Visible = True
                Me.Text = "Audit for " + entity_name + " preference type  " + class_name
            End If

            Me.uxaudit.Nodes.Clear()
            Me.uxaudit.BeginUnboundLoad()

            For i = 0 To links.links.Count - 1

                uxaudit.Nodes.Add()
                uxaudit.Nodes(i).SetValue(0, links.links(i).name)
                uxaudit.Nodes(i).SetValue(1, Format(links.links(i).date_from.ToLocalTime, "dd-MMM-yyyy HH:mm:ss"))
                If Year(links.links(i).date_to) = "9999" Then
                    uxaudit.Nodes(i).SetValue(2, "Current")
                Else
                    uxaudit.Nodes(i).SetValue(2, Format(links.links(i).date_to.ToLocalTime, "dd-MMM-yyyy HH:mm:ss"))
                End If
                uxaudit.Nodes(i).SetValue(3, links.links(i).change_user)
                If links.links(i).inactive = True Then
                    uxaudit.Nodes(i).SetValue(4, "Inactive")
                Else
                    uxaudit.Nodes(i).SetValue(4, "Active")
                End If
                uxaudit.Nodes(i).SetValue(5, links.links(i).rating)
                uxaudit.Nodes(i).SetValue(6, links.links(i).class_name)
            Next

            Me.uxaudit.EndUnboundLoad()

            load_view = True
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_audit_entity_links", "load_view", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try
    End Function




    Private Sub bclose_Click(sender As Object, e As EventArgs) Handles bclose.Click
        Me.Hide()
    End Sub

   
End Class
Public Class Cbc_am_audit_entity_links
    WithEvents _view As Ibc_am_audit_entity_links
    REM taxonomy
    Public Function load_data(view As Ibc_am_audit_entity_links, entity_id As Long, class_id As Long, schema_id As Long, parent As Boolean, class_name As String, entity_name As String, schema_name As String, pref_type_id As Integer, user_id As Long, audit_type As bc_om_audit_links.EAUDIT_TYPE) As Boolean
        load_data = False
        Try
            _view = view
            Dim links As New bc_om_audit_links
            links.class_id = class_id
            links.entity_id = entity_id
            links.schema_id = schema_id
            links.parent = parent
            links.pref_type_id = pref_type_id
            links.user_id = user_id
            links.audit_type = audit_type
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                links.db_read()
            Else
                links.tmode = bc_cs_soap_base_class.tREAD
                If links.transmit_to_server_and_receive(links, True) = False Then
                    Exit Function
                End If

            End If
            If links.links.Count = 0 Then
                Dim omsg As New bc_cs_message("Blue Curve", "No audit information available", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)

                Exit Function

            End If

            load_data = _view.load_view(links, class_name, entity_name, schema_name, audit_type)

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log(" Cbc_am_audit_entity_links", "load_data taxonomy", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try

    End Function
End Class
Public Interface Ibc_am_audit_entity_links
    Function load_view(links As bc_om_audit_links, class_name As String, entity_name As String, schema_name As String, audit_type As bc_om_audit_links.EAUDIT_TYPE)
End Interface
