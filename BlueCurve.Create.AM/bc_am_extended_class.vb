Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports System.Windows.Forms


Public Class bc_am_extended_class
    Implements Iview_bc_am_extended_class
    Event save_data(ByVal oec As bc_om_extended_classification, ByVal tx As String) Implements Iview_bc_am_extended_class.save_data

    Dim _doc As bc_om_document


    Public Sub load_data(ByVal doc As bc_om_document) Implements Iview_bc_am_extended_class.load_data
        Try
            _doc = doc

            Me.timportant.Text = _doc.urgent_text
            For i = 0 To _doc.oec.Lists.Count - 1
                Me.TabControl1.TabPages.Add(_doc.oec.Lists(i).name)
                If i = 0 Then
                    load_list(0)
                End If
            Next
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_extended_class", "load_data", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Private Sub load_list(ByVal i As Integer)
        Try
            Me.litems.Items.Clear()
            Dim lvew As ListViewItem
            For i = 0 To _doc.oec.Lists(i).items.Count - 1
                lvew = New ListViewItem(CStr(_doc.oec.Lists(Me.TabControl1.SelectedIndex).items(i).value))
                If _doc.oec.Lists(Me.TabControl1.SelectedIndex).items(i).selected = True Then
                    lvew.Checked = True
                End If
                Me.litems.Items.Add(lvew)

            Next
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_extended_class", "load_list", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Private Sub TabControl1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabControl1.SelectedIndexChanged
        Try
            load_list(Me.TabControl1.SelectedIndex)
           
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_extended_class", "TabControl1_SelectedIndexChanged", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub

    Private Sub bcancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Hide()
    End Sub
    Private Sub btnsubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnsubmit.Click
        RaiseEvent save_data(_doc.oec, Me.timportant.Text)
        Me.Hide()
    End Sub
    Private Sub check_ok()
        Me.btnsubmit.Enabled = False
        Dim item_found As Boolean = False
        If Trim(Me.timportant.Text) <> "" Then
            For i = 0 To _doc.oec.Lists.Count - 1
                For j = 0 To _doc.oec.Lists(i).items.Count - 1
                    item_found = True
                    If _doc.oec.Lists(i).items(j).selected = True Then
                        Me.btnsubmit.Enabled = True
                        Exit Sub
                    End If
                Next
            Next
            If item_found = False Then
                Me.btnsubmit.Enabled = True
            End If
        End If

    End Sub

    Private Sub litems_ItemCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs) Handles litems.ItemCheck
        Try
           

            If e.CurrentValue = 0 Then
                _doc.oec.Lists(Me.TabControl1.SelectedIndex).items(e.Index).selected = True
            Else
                _doc.oec.Lists(Me.TabControl1.SelectedIndex).items(e.Index).selected = False
            End If
            check_ok()

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("cttrl_bc_am_extended_class", "litems_ItemCheck", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub

    Private Sub timportant_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles timportant.TextChanged
        check_ok()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Hide()
    End Sub
End Class
Public Class cttrl_bc_am_extended_class
    Dim _doc As bc_om_document

    WithEvents _view As Iview_bc_am_extended_class

    Public Sub New(ByVal view As Iview_bc_am_extended_class, ByVal doc As bc_om_document)
        _view = view
        _doc = doc

    End Sub
    Public Sub load_data()
        Try
            If _doc.oec.Lists.Count = 0 Then
                Dim oec As New bc_om_extended_classification
                _doc.oec.entity_id = _doc.entity_id
                _doc.oec.pub_type_id = _doc.pub_type_id
                REM 5.3 refix
                If _doc.id = 0 Then
                    _doc.oec.doc_id = _doc.filename
                Else
                    _doc.oec.doc_id = _doc.id
                End If


                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    _doc.oec.db_read()
                Else
                    _doc.oec.tmode = bc_cs_soap_base_class.tREAD
                    If _doc.oec.transmit_to_server_and_receive(_doc.oec, True) = False Then
                        Exit Sub
                    End If
                End If
            End If
            _view.load_data(_doc)

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("cttrl_bc_am_extended_class", "load_data", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Public Sub save_data(ByVal oec As bc_om_extended_classification, ByVal tx As String) Handles _view.save_data
        Try
            _doc.urgent_text = tx
            _doc.oec = oec

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("cttrl_bc_am_extended_class", "save_data", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
End Class
Public Interface Iview_bc_am_extended_class
    Sub load_data(ByVal doc As bc_om_document)
    Event save_data(ByVal oec As bc_om_extended_classification, ByVal tx As String)
End Interface

