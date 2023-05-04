Imports BlueCurve.Core.CS

Public Class bc_dx_cell_validation
    Implements Ibc_dx_cell_validation


    Event highlight_cell(sheet_name As String, row As Integer, column As Integer) Implements Ibc_dx_cell_validation.highlight_cell

    Public Sub New()

        DevExpress.Skins.SkinManager.EnableFormSkins()
        DevExpress.UserSkins.BonusSkins.Register()
        DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle("Blue")
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub


    Public Function load_view(warningstype As ArrayList, warningssheet As ArrayList, warningstx As ArrayList, warningsrow As ArrayList, warningscol As ArrayList, warningsaddress As ArrayList) As Boolean Implements Ibc_dx_cell_validation.load_view
        load_view = False
        Try
            Me.uxres.BeginUpdate()
            For i = 0 To warningstype.Count - 1
                Me.uxres.Nodes.Add("")
                Me.uxres.Nodes(Me.uxres.Nodes.Count - 1).SetValue(0, warningssheet(i))
                Me.uxres.Nodes(Me.uxres.Nodes.Count - 1).SetValue(1, warningstype(i))
                Me.uxres.Nodes(Me.uxres.Nodes.Count - 1).SetValue(2, warningstx(i))
                If warningstype(i) = "Data" Or warningstype(i) = "Warning" Then
                    Me.Text = "Insight Submission Warnings"
                    Me.uxres.Nodes(Me.uxres.Nodes.Count - 1).SetValue(4, warningsrow(i))
                    Me.uxres.Nodes(Me.uxres.Nodes.Count - 1).SetValue(5, warningscol(i))
                    Me.uxres.Nodes(Me.uxres.Nodes.Count - 1).SetValue(3, warningsaddress(i))
                    Me.uxres.Nodes(Me.uxres.Nodes.Count - 1).StateImageIndex = 0

                End If
            Next

            load_view = True
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_dx_cell_validation", "load_view", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.uxres.EndUpdate()

        End Try
    End Function



    Private Sub bclose_Click(sender As Object, e As EventArgs) Handles bclose.Click
        Me.Hide()
    End Sub

    Private Sub bok_Click(sender As Object, e As EventArgs) Handles bgoto.Click

        If Me.uxres.Selection.Count = 0 Then
            Exit Sub
        End If

        RaiseEvent highlight_cell(Me.uxres.Selection(0).GetValue(0), Me.uxres.Selection(0).GetValue(4), Me.uxres.Selection(0).GetValue(5))
    End Sub

   

    Private Sub uxres_FocusedNodeChanged(sender As Object, e As DevExpress.XtraTreeList.FocusedNodeChangedEventArgs) Handles uxres.FocusedNodeChanged
        Me.bgoto.Enabled = False
        If Me.uxres.Selection.Count = 0 Then
            Exit Sub
        End If
        Me.bgoto.Enabled = True
    End Sub
End Class
Public Class cbc_dx_cell_validation
    WithEvents _view As Ibc_dx_cell_validation

    Dim _oexcel As bc_ao_in_excel
    Public Function load_data(view As Ibc_dx_cell_validation, oexcel As bc_ao_in_excel, warningstype As ArrayList, warningssheet As ArrayList, warningstx As ArrayList, warningsrow As ArrayList, warningscol As ArrayList, warningsaddress As ArrayList)
        load_data = False
        Try
            _view = view
            _oexcel = oexcel


            load_data = _view.load_view(warningstype, warningssheet, warningstx, warningsrow, warningscol, warningsaddress)

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("cbc_dx_cell_validation", "load_data", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try
    End Function
    Public Sub highlight_cell(sheet_name As String, row As Integer, column As Integer) Handles _view.highlight_cell

       
        Try
            _oexcel.highlight_cell(sheet_name, row, column, "", True)
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("cbc_dx_cell_validation", "highlight_cell", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
End Class
Public Interface Ibc_dx_cell_validation
    Function load_view(warningstype As ArrayList, warningssheet As ArrayList, warningstx As ArrayList, warningsrow As ArrayList, warningscol As ArrayList, warningsaddress As ArrayList) As Boolean
    Event highlight_cell(sheet_name As String, row As Integer, column As Integer)

End Interface