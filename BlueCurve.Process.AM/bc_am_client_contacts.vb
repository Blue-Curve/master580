Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports System.Windows.Forms
Public Class bc_am_client_contacts
    Implements Ibc_am_client_contacts

    Event cancel() Implements Ibc_am_client_contacts.cancel
    Event save(selected_clients As List(Of String)) Implements Ibc_am_client_contacts.save
    Event search(search_text As String, all As Boolean) Implements Ibc_am_client_contacts.search

    Dim _selected_clients As New List(Of String)

    Public Function load_view() Implements Ibc_am_client_contacts.load_view


    End Function

    Private Sub bcancel_Click(sender As Object, e As EventArgs) Handles bcancel.Click
        Me.Close()
    End Sub
    Private Sub run_search(all As Boolean)
        Me.uxrecipient.Nodes.Clear()
        Me.Refresh()
        Me.Cursor = Cursors.WaitCursor
        RaiseEvent search(Me.tsearch.Text, all)
        Me.Cursor = Cursors.Default


    End Sub
    Public Function load_search_results(search_results As List(Of bc_om_distribution.bc_om_client_contact)) Implements Ibc_am_client_contacts.load_search_results
        uxrecipient.BeginUnboundLoad()
        uxrecipient.Nodes.Clear()
        Dim found As Boolean
        Dim nc As Integer = 0
        For i = 0 To search_results.Count - 1
            found = False
            For j = 0 To _selected_clients.Count - 1
                If search_results(i).client_id = _selected_clients(j) Then
                    found = True
                    Exit For
                End If
            Next
            If found = False Then
                Dim tln As Object = Nothing
                uxrecipient.Nodes.Add()
                uxrecipient.Nodes(nc).SetValue(0, search_results(i).name)
                uxrecipient.Nodes(nc).SetValue(1, search_results(i).organisation)
                uxrecipient.Nodes(nc).SetValue(2, search_results(i).sent)

                uxrecipient.Nodes(nc).Tag = search_results(i).client_id
                nc = nc + 1
            End If
        Next
        uxrecipient.EndUnboundLoad()
    End Function
    Private Sub bsave_Click(sender As Object, e As EventArgs) Handles bsave.Click
        Me.Cursor = Cursors.WaitCursor


        RaiseEvent save(_selected_clients)
        Me.Cursor = Cursors.Default

        Me.Close()
    End Sub

    Private Sub ball_Click(sender As Object, e As EventArgs) Handles ball.Click
        run_search(True)

    End Sub

    Private Sub tsearch_EditValueChanged(sender As Object, e As EventArgs) Handles tsearch.EditValueChanged
        Me.Timer1.Start()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Me.Timer1.Stop()

        run_search(False)

    End Sub

    Private Sub uxrecipient_DoubleClick(sender As Object, e As EventArgs) Handles uxrecipient.DoubleClick
        _selected_clients.Add(uxrecipient.Selection(0).Tag)
        Dim tln As Object = Nothing
        uxsel.Nodes.Add()
        uxsel.Nodes(uxsel.Nodes.Count - 1).SetValue(0, uxrecipient.Selection(0).GetValue(0))
        uxsel.Nodes(uxsel.Nodes.Count - 1).SetValue(1, uxrecipient.Selection(0).GetValue(1))
        uxsel.Nodes(uxsel.Nodes.Count - 1).SetValue(2, uxrecipient.Selection(0).GetValue(2))

        uxsel.Nodes(uxsel.Nodes.Count - 1).Tag = uxrecipient.Selection(0).Tag
        uxrecipient.Nodes.Remove(uxrecipient.Selection(0))


    End Sub

    Private Sub uxsel_DoubleClick(sender As Object, e As EventArgs) Handles uxsel.DoubleClick

        For i = 0 To _selected_clients.Count - 1
            If CStr(_selected_clients(i)) = uxsel.Selection(0).Tag Then
                _selected_clients.RemoveAt(i)
                uxsel.Nodes.Remove(uxsel.Selection(0))
                Exit For
            End If
        Next


    End Sub

    Private Sub uxrecipient_FocusedNodeChanged(sender As Object, e As DevExpress.XtraTreeList.FocusedNodeChangedEventArgs) Handles uxrecipient.FocusedNodeChanged

    End Sub

    Private Sub ball_Click_1(sender As Object, e As EventArgs) Handles ball.Click

    End Sub
End Class
Public Class Cbc_am_client_contacts
    WithEvents _view As Ibc_am_client_contacts
    Dim _selected_clients As List(Of bc_om_distribution.bc_om_client_contact)
    Public bsave As Boolean = False
    Dim _doc_id As Long
    Public Sub New(doc_id As Long, view As Ibc_am_client_contacts)
        _view = view
        _doc_id = doc_id
    End Sub
    Public Function load_data() As Boolean
        load_data = True
    End Function
    Public Sub save(selected_clients As List(Of String)) Handles _view.save
        Dim scc As New bc_om_distribution.bc_om_selected_clients_for_doc
        scc.doc_id = _doc_id
        scc.selected_clients = selected_clients

        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
            scc.db_write()
        Else
            scc.tmode = bc_cs_soap_base_class.tWRITE
            If scc.transmit_to_server_and_receive(scc, True) = False Then
                Exit Sub
            End If
        End If
        bsave = True

    End Sub
    Private Sub run_search(search_text As String, all As Boolean) Handles _view.search
        Dim occ As New bc_om_distribution.bc_om_client_contacts
        occ.all = all
        occ.search_term = search_text
        occ.doc_id = _doc_id
        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
            occ.db_read()
        Else
            occ.tmode = bc_cs_soap_base_class.tREAD
            If occ.transmit_to_server_and_receive(occ, True) = False Then
                Exit Sub

            End If

        End If
        _view.load_search_results(occ.search_results)
    End Sub
End Class
Public Interface Ibc_am_client_contacts
    Event cancel()
    Event save(selected_clients As List(Of String))
    Event search(search_text As String, all As Boolean)
    Function load_view()
    Function load_search_results(search_results As List(Of bc_om_distribution.bc_om_client_contact))

End Interface