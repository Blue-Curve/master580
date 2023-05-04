Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports System.Windows.Forms


Public Class Vbc_am_behalf_author
    Implements Ibc_am_behalf_author
    Event save(ByVal nas As String) Implements Ibc_am_behalf_author.save
    Public Sub load_data(ByVal users As ArrayList) Implements Ibc_am_behalf_author.load_data
        For i = 0 To users.Count - 1
            Me.ComboBox1.Items.Add(users(i))
        Next
        If users.Count > 0 Then
            Me.ComboBox1.SelectedIndex = 0
        End If

    End Sub
    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        Me.btnsubmit.Enabled = False
        If Me.ComboBox1.SelectedIndex > -1 Then
            Me.btnsubmit.Enabled = True
        End If
    End Sub
    Private Sub btnsubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnsubmit.Click
        Me.Hide()
        RaiseEvent save(Me.ComboBox1.Text)

    End Sub
End Class
Public Class Cbc_am_behalf_author
   
    WithEvents _view As Ibc_am_behalf_author
    Dim _behalf_of As Integer
    Dim _doc_id As String
    Public originator As Long
    Dim sec As New bc_om_doc_security
    Public Sub New(ByVal doc_id As String, ByVal pub_type_id As Long, ByVal entity_id As Long, ByVal view As Ibc_am_behalf_author)
        sec.pub_type_id = pub_type_id
        sec.entity_id = entity_id
        _doc_id = doc_id
        _view = view
        originator = bc_cs_central_settings.logged_on_user_id
    End Sub
    Public Function check_behalf_selection_required() As Boolean
        Try
            check_behalf_selection_required = False
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.LOCAL Then
                Exit Function
            End If

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                sec.db_read()
            Else
                sec.tmode = bc_cs_soap_base_class.tREAD
                If sec.transmit_to_server_and_receive(sec, True) = False Then

                    Exit Function
                End If
            End If
            If sec.approval_type = 1 Then
                Exit Function
            ElseIf sec.approval_type = 2 Then
                If sec.proxy_user_ids.Count = 0 Then
                    Exit Function
                ElseIf sec.proxy_user_ids.Count = 1 Then
                    originator = sec.proxy_user_ids(0)
                    save_originator()
                Else
                    check_behalf_selection_required = True
                    _view.load_data(sec.proxy_user_names)
                End If
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Cbc_am_behalf_author", "check_behalf_required", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try

    End Function
    Public Function save(ByVal na As String) Handles _view.save
        Try
            For i = 0 To sec.proxy_user_names.Count - 1
                If sec.proxy_user_names(i) = na Then
                    originator = sec.proxy_user_ids(i)
                    save_originator()
                    Exit For
                End If

            Next
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Cbc_am_behalf_author", "save", bc_cs_error_codes.USER_DEFINED, ex.Message)
            save = False
        Finally
            save = True

        End Try
     
    End Function
    Sub save_originator()
        Try
            Dim sql As New bc_om_sql("exec dbo.bc_core_set_originator_for_doc '" + CStr(_doc_id) + "'," + CStr(originator))
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                sql.db_read()

            Else
                sql.tmode = bc_cs_soap_base_class.tREAD
                If sql.transmit_to_server_and_receive(sql, True) = False Then
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Cbc_am_behalf_author", "save_originator", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try
        
    End Sub
End Class
Public Interface Ibc_am_behalf_author
    Sub load_data(ByVal users As ArrayList)
    Event save(ByVal na As String)
End Interface


