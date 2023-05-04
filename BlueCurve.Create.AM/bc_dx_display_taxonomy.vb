Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Public Class bc_dx_display_taxonomy
    Implements Ibc_dx_display_taxonomy



    Private Sub bcancel_Click(sender As Object, e As EventArgs) Handles bcancel.Click
        Me.Hide()
    End Sub
 
    Public Function load_view(dstr As String, title As String) As Object Implements Ibc_dx_display_taxonomy.load_view
        load_view = False
        Try
            Me.rexp.HtmlText = dstr
            Me.Text = "Taxonomy for: " + title

            load_view = True
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_dx_display_taxonomy", "load_view", bc_cs_activity_codes.COMMENTARY, ex.Message)
        End Try
    End Function
End Class



Public Class Cbc_dx_display_taxonomy
    WithEvents _view As Ibc_dx_display_taxonomy
    Dim _doc As bc_om_document
    Public Sub New(view As Ibc_dx_display_taxonomy, doc As bc_om_document)
        _view = view
        _doc = doc
    End Sub
    Public Function load_data() As Boolean
        Try
            load_data = False
            Dim odt As New bc_om_display_taxonomy
            odt.doc = _doc
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                odt.db_read_all()

            Else
                odt.tmode = bc_cs_soap_base_class.tREAD
                If odt.transmit_to_server_and_receive(odt, True) = False Then
                    Exit Function
                End If
            End If
            _view.load_view(odt.dstr, _doc.title)

            load_data = True
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Cbc_dx_display_taxonomy", "load_data", bc_cs_activity_codes.COMMENTARY, ex.Message)
        End Try
    End Function
End Class



Public Interface Ibc_dx_display_taxonomy
    Function load_view(dstr As String, title As String)
End Interface
