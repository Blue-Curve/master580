Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM

Public Class bc_am_dx_view_attestations
    Implements Ibc_am_dx_view_attestations


    Dim _attestations As New bc_om_attestations_for_doc
    Event get_attestation_answers(attestation_id As Long) Implements Ibc_am_dx_view_attestations.get_attestation_answers
   


    Public Function load_view(attestations As bc_om_attestations_for_doc, title As String) Implements Ibc_am_dx_view_attestations.load_view
        load_view = False
        Try
            Me.Text = "View Attestations for " + title
            _attestations = attestations

            uxatt.Nodes.Clear()
            uxatt.BeginUnboundLoad()


            For i = 0 To _attestations.attestations.Count - 1
                With _attestations.attestations(i)
                    uxatt.Nodes.Add(.id)
                    uxatt.Nodes(i).Tag = .id
                    uxatt.Nodes(i).SetValue(0, Format(.submitted_date.ToLocalTime, "dd MMM yyyy HH:mm:ss"))
                    uxatt.Nodes(i).SetValue(1, .user_name)
                    uxatt.Nodes(i).SetValue(2, .questionnaire)
                    uxatt.Nodes(i).SetValue(3, .group)
                    If .pass = 0 Then
                        uxatt.Nodes(i).StateImageIndex = -1
                    Else
                        uxatt.Nodes(i).StateImageIndex = 1
                    End If
                End With
            Next
            uxatt.EndUnboundLoad()

            load_view = True
            If Me.uxatt.Nodes.Count > 0 Then
                show_answers()
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_dx_view_attestations", "load_view", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Function

    Private Sub bcancel_Click(sender As Object, e As EventArgs) Handles bcancel.Click
        Me.Hide()
    End Sub

    Private Sub show_answers()
        If Me.uxatt.Selection.Count = 0 Then
            Exit Sub
        End If
        If (IsNumeric(Me.uxatt.Selection(0).Tag)) = False Then
            Exit Sub
        End If
        Me.Cursor = Windows.Forms.Cursors.WaitCursor

        RaiseEvent get_attestation_answers(CLng(Me.uxatt.Selection(0).Tag))

        Me.Cursor = Windows.Forms.Cursors.Default
    End Sub
  
    Private Sub uxatt_FocusedNodeChanged(sender As Object, e As DevExpress.XtraTreeList.FocusedNodeChangedEventArgs) Handles uxatt.FocusedNodeChanged
        show_answers()
    End Sub
    Public Function load_answers(aanswers As bc_om_attestation_answers) Implements Ibc_am_dx_view_attestations.load_answers
        Try
            Dim oa As bc_am_attestation_answer
            While Me.panel1.Controls.Count <> 0
                Me.panel1.Controls.RemoveAt(0)
            End While
            Dim dp As System.Drawing.Point
            dp.X = 10
            dp.Y = 10

            For i = 0 To aanswers.responses.Count - 1
                With aanswers.responses(i)
                    oa = New bc_am_attestation_answer
                    oa.Width = Me.panel1.Width - 50

                    'oa.Anchor = Windows.Forms.AnchorStyles.Left & Windows.Forms.AnchorStyles.Right
                    oa.Location = dp
                    oa.uxq.Text = CStr(i + 1) + ") " + .text
                    oa.la.Text = .option_selected_text
                    'If .pass = False Then
                    '    oa.ppass.Visible = False
                    '    oa.pfail.Visible = True
                    'Else
                    '    oa.ppass.Visible = True
                    '    oa.pfail.Visible = False
                    'End If
                    Me.panel1.Controls.Add(oa)
                    dp.Y = dp.Y + oa.Height + 10
                End With
            Next

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_dx_view_attestations", "load_ansers", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try


    End Function



End Class
Public Class Cbc_am_dx_view_attestations
    WithEvents _view As Ibc_am_dx_view_attestations
    Dim _doc_id As Long
    Dim _attestations As New bc_om_attestations_for_doc

    Public Function load_data(view As Ibc_am_dx_view_attestations, doc_id As Long, title As String) As String
        load_data = False
        Try
            _view = view
            _doc_id = doc_id
            If load_attestations() = False Then
                Exit Function
            End If
            If _attestations.attestations.Count = 0 Then
                Exit Function
            End If
            load_data = _view.load_view(_attestations, title)
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Cbc_am_dx_view_attestations", "load_attestations", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Function
    Private Function load_attestations() As Boolean
        load_attestations = False
        Try
            _attestations.doc_id = _doc_id
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                _attestations.db_read()
            Else
                _attestations.tmode = bc_cs_soap_base_class.tREAD
                If _attestations.transmit_to_server_and_receive(_attestations, True) = False Then
                    Exit Function
                End If
            End If
            load_attestations = True
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Cbc_am_dx_view_attestations", "load_attestations", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Function
    Private Sub get_attestations_answers(att_id As Long) Handles _view.get_attestation_answers
        Try

            Dim aanswers As New bc_om_attestation_answers
            aanswers.att_id = att_id

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                aanswers.db_read()
            Else
                aanswers.tmode = bc_cs_soap_base_class.tREAD
                If aanswers.transmit_to_server_and_receive(aanswers, True) = False Then
                    Exit Sub
                End If
            End If
            _view.load_answers(aanswers)

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Cbc_am_dx_view_attestations", "get_attestations_answers", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
End Class
Public Interface Ibc_am_dx_view_attestations
    Function load_view(attestations As bc_om_attestations_for_doc, title As String)
    Event get_attestation_answers(attestation_id As Long)
    Function load_answers(aanswers As bc_om_attestation_answers)
End Interface
