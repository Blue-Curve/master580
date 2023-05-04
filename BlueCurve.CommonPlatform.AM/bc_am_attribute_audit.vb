Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM


Public Class bc_am_attribute_audit
    Implements Ibc_am_attribute_audit
    Event get_data(stage_id As bc_om_attribute_audit.EFINANCIAL_WORKLOW_STAGES) Implements Ibc_am_attribute_audit.get_data
    Dim loading As Boolean = False
    Public Function load_view(title As String, audit_values As bc_om_attribute_audit, show_workflow As Boolean, show_date_to As Boolean, stage As bc_om_attribute_audit.EFINANCIAL_WORKLOW_STAGES) As Boolean Implements Ibc_am_attribute_audit.load_view
        Try
            loading = True
            Me.Text = "History for " + title
            If stage = 1 Then
                Me.uxworkflow.SelectedIndex = 0
            Else
                Me.uxworkflow.SelectedIndex = 1
            End If
            If show_workflow = True Then
                Me.uxworkflow.Visible = True
            Else
                Me.uxworkflow.Visible = False
            End If
            If show_date_to = False Then
                Me.uxaudit.Columns(3).Visible = False
            End If
            load_view = load_audit(audit_values)
            loading = False
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_attribute_audit", "load_view", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Function
    Public Function load_audit(audit_values As bc_om_attribute_audit) Implements Ibc_am_attribute_audit.load_audit
        Try
            uxaudit.Nodes.Clear()
            uxaudit.BeginUnboundLoad()

            Dim nc As Integer = 0
            For i = 0 To audit_values.audit_values.Count - 1
                uxaudit.Nodes.Add()
                uxaudit.Nodes(i).SetValue(0, audit_values.audit_values(i).attribute_name)
                uxaudit.Nodes(i).SetValue(1, audit_values.audit_values(i).value)
                uxaudit.Nodes(i).SetValue(2, Format(audit_values.audit_values(i).date_from.ToLocalTime, "dd-MMM-yyyy HH:mm:ss"))
                If audit_values.audit_values(i).date_to.Year = "9999" Then
                    uxaudit.Nodes(i).SetValue(3, "Current")
                Else
                    uxaudit.Nodes(i).SetValue(3, Format(audit_values.audit_values(i).date_to.ToLocalTime, "dd-MMM-yyyy HH:mm:ss"))
                End If
                uxaudit.Nodes(i).SetValue(4, audit_values.audit_values(i).user_name)
            Next
            uxaudit.EndUnboundLoad()
            load_audit = True
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_attribute_audit", "load_audit", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Function
    Private Sub bcancel_Click(sender As Object, e As EventArgs) Handles bcancel.Click
        Me.Hide()
    End Sub

    Private Sub uxworkflow_SelectedIndexChanged(sender As Object, e As EventArgs) Handles uxworkflow.SelectedIndexChanged
        If Me.uxworkflow.SelectedIndex = -1 Or loading = True Then
            Exit Sub
        End If

        Me.Cursor = Windows.Forms.Cursors.WaitCursor

        If Me.uxworkflow.SelectedIndex = 0 Then
            RaiseEvent get_data(bc_om_attribute_audit.EFINANCIAL_WORKLOW_STAGES.DRAFT)
        Else
            RaiseEvent get_data(bc_om_attribute_audit.EFINANCIAL_WORKLOW_STAGES.PUBLISH)
        End If
        Me.Cursor = Windows.Forms.Cursors.Default

    End Sub

    Private Sub bc_am_attribute_audit_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class
Public Class Cbc_am_attribute_audit
    WithEvents _view As Ibc_am_attribute_audit
    Dim _audit_values As New bc_om_attribute_audit
    Dim _title As String
    
    Public Function load_data(view As Ibc_am_attribute_audit, key_name As String, attribute_name As String, attribute_id As String, key_id As Long, attribute_type As bc_om_attribute_audit.EATTRIBUTE_TYPE, show_workflow As Boolean, all As Boolean) As Boolean
        Try
            load_data = False
            _view = view
            If all = False Then
                _title = key_name + ": " + attribute_name
            Else
                _title = key_name
            End If
            _audit_values.key_id = key_id
            _audit_values.attribute_id = attribute_id
            _audit_values.attribute_type = attribute_type
            _audit_values.all = all
            Dim show_date_to As Boolean = False
            If attribute_type = bc_om_attribute_audit.EATTRIBUTE_TYPE.BUS_AREA Or attribute_type = bc_om_attribute_audit.EATTRIBUTE_TYPE.USER_ASSOCIATIONS Or attribute_type = bc_om_attribute_audit.EATTRIBUTE_TYPE.OTHER_ROLES Then
                show_date_to = True
                _title = key_name + ": " + attribute_name
            Else
                show_date_to = False
                If all = False Then
                    _title = key_name + ": " + attribute_name
                Else
                    _title = key_name
                End If
            End If
            If get_data(bc_om_attribute_audit.EFINANCIAL_WORKLOW_STAGES.PUBLISH) = True Then
                If _audit_values.audit_values.Count > 0 Then
                    load_data = _view.load_view(_title, _audit_values, show_workflow, show_date_to, bc_om_attribute_audit.EFINANCIAL_WORKLOW_STAGES.PUBLISH)
                ElseIf attribute_type = bc_om_attribute_audit.EATTRIBUTE_TYPE.ENTITY Then
                    If get_data(bc_om_attribute_audit.EFINANCIAL_WORKLOW_STAGES.DRAFT) = True Then
                        If _audit_values.audit_values.Count > 0 Then
                            load_data = _view.load_view(_title, _audit_values, show_workflow, show_date_to, bc_om_attribute_audit.EFINANCIAL_WORKLOW_STAGES.DRAFT)
                        End If
                    ElseIf all = True Or attribute_type = bc_om_attribute_audit.EATTRIBUTE_TYPE.USER_ASSOCIATIONS Or attribute_type = bc_om_attribute_audit.EATTRIBUTE_TYPE.OTHER_ROLES Or attribute_type = bc_om_attribute_audit.EATTRIBUTE_TYPE.BUS_AREA Then
                        Dim omsg As New bc_cs_message("Blue Curve", "No audit information available", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    End If
                Else
                    Dim omsg As New bc_cs_message("Blue Curve", "No audit information available", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                End If
            ElseIf all = True Or attribute_type = bc_om_attribute_audit.EATTRIBUTE_TYPE.USER_ASSOCIATIONS Or attribute_type = bc_om_attribute_audit.EATTRIBUTE_TYPE.OTHER_ROLES Or attribute_type = bc_om_attribute_audit.EATTRIBUTE_TYPE.BUS_AREA Then
                Dim omsg As New bc_cs_message("Blue Curve", "No audit information available", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Cbc_am_attribute_audit", "load_data", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Function
    Private Function change_stage(stage_id As bc_om_attribute_audit.EFINANCIAL_WORKLOW_STAGES) As Boolean Handles _view.get_data
        If get_data(stage_id) = True Then
            _view.load_audit(_audit_values)
        End If
    End Function
    Private Function get_data(stage_id As bc_om_attribute_audit.EFINANCIAL_WORKLOW_STAGES)
        Try
            _audit_values.audit_values.Clear()
            _audit_values.stage_id = stage_id
            get_data = False
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                _audit_values.db_read()
            Else
                _audit_values.tmode = bc_cs_soap_base_class.tREAD
                If _audit_values.transmit_to_server_and_receive(_audit_values, True) = False Then
                    Exit Function
                End If
            End If
            get_data = True
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Cbc_am_attribute_audit", "get_data", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Function
End Class
Public Interface Ibc_am_attribute_audit
    Function load_view(title As String, audit_values As bc_om_attribute_audit, show_workflow As Boolean, show_date_to As Boolean, stage As bc_om_attribute_audit.EFINANCIAL_WORKLOW_STAGES) As Boolean
    Function load_audit(audit_values As bc_om_attribute_audit)
    Event get_data(stage_id As bc_om_attribute_audit.EFINANCIAL_WORKLOW_STAGES)
End Interface
