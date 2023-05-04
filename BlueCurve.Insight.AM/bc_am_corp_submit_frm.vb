Imports System.Collections
Imports System.IO
Imports System.Windows.Forms
Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports BlueCurve.Core.AS


Public Class bc_am_corp_submit_frm

    Dim class_id As Long
    Dim bc_cs_central_settings As New bc_cs_central_settings(True)
    Dim corp_action As New bc_om_corp_action

    Public ActionId As Long
    Public AoExcel As bc_ao_in_excel

    Public Sub New(ByVal aoObject As Object)
        'Public Sub New(ByVal show_form As Boolean, ByVal ao_object As Object, ByVal ao_type As String, ByVal validate_only As Boolean)
        Dim slog = New bc_cs_activity_log("bc_am_corp_submit_frm", "new", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim ocommentary As New bc_cs_activity_log("bc_am_corp_submit_frm", "new", bc_cs_activity_codes.COMMENTARY, "Load corporate action submit form")

        Try

            AoExcel = New bc_ao_in_excel(aoObject)
            ActionId = AoExcel.GetActionId()
            corp_action.ActionId = ActionId

            If ActionId = 0 Then
                Dim omsg As New bc_cs_message("Blue Curve", "Can not find a corporate action.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Sub

            Else
                Me.InitializeComponent()
                'Me.ShowDialog()
            End If


        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_corp_submit_frm", "new", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try

    End Sub


    Private Sub bc_am_corp_action_frm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try

            If bc_cs_central_settings.connection_method = bc_cs_central_settings.ADO Then
                corp_action.db_read()
            ElseIf bc_cs_central_settings.connection_method = bc_cs_central_settings.SOAP Then
                corp_action.tmode = bc_cs_soap_base_class.tREAD
                If corp_action.transmit_to_server_and_receive(corp_action, True) = False Then
                    Exit Sub
                End If
            Else
                Dim omsg As New bc_cs_message("Blue Curve", "Cannot connect to " + bc_cs_central_settings.soap_server + " application cannot be launched.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                System.Windows.Forms.Application.Exit()
            End If

            uxStockBox.Text = corp_action.EntityDescription
            uxActionBox.Text = corp_action.EventDescriprion
            corp_action.CalcAdjustment = AoExcel.exceldoc.activesheet.cells(8, 2).value()
            uxFactorBox.Text = corp_action.CalcAdjustment

            If corp_action.Submitted = 1 Then
                Dim omsg As New bc_cs_message("Blue Curve Insight", "The adjustments for this action have already been submitted.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Me.Close()
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_corp_submit_frm", "bc_am_corp_action_frm_Load", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
        End Try

    End Sub

    'Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Me.Close()
    'End Sub

    'Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    '    If CheckAdjust.Checked = True Then

    '        SubmitData(corp_action)

    '        REM corp_action = Nothing
    '        Me.Close()
    '    Else

    '        Dim omsg As New bc_cs_message("Blue Curve", "Please confirm adjustments.", bc_cs_message.MESSAGE, False, False, "Yes", "", True)
    '    End If

    'End Sub

    Public Sub SubmitData(ByRef CorpAction As bc_om_corp_action)

        Try

            'Dim test As String
            'Write data to Excel
            'test = ao_excel.get_sheet_name()

            AoExcel.SubmitCorpAction(CorpAction)

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_corp_submit_frm", "SubmitData", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Sub

    Private Sub bok_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles bok.MouseDown
        Me.bok.Visible = False
    End Sub

    Private Sub bok_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles bok.MouseUp

        Dim CellCount As Long
        Dim i1 As Long
        Dim i2 As Long
        
        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try

            AoExcel.exceldoc.sheets(2).select()

            Me.bok.Visible = True
            Me.Refresh()

            REM check dates
            CellCount = 0
            For i1 = 0 To corp_action.Adjustments.eventactions.Count - 1
                If corp_action.Adjustments.eventactions(i1).datatype = "time series" Then
                    If corp_action.Adjustments.eventactions(i1).datecolumn <> 0 Then
                        CellCount = CellCount + 2
                    Else
                        CellCount = CellCount + 1
                    End If
                End If
            Next i1

            REM Data Validation
            CellCount = CellCount + 1
            For i1 = 0 To corp_action.Adjustments.eventactions.Count - 1
                If corp_action.Adjustments.eventactions(i1).datatype = "time series" Then
                    If corp_action.Adjustments.eventactions(i1).datecolumn <> 0 Then
                        CellCount = CellCount + 2
                    Else
                        CellCount = CellCount + 1
                    End If
                    i2 = 13
                    While CStr(AoExcel.exceldoc.activesheet.cells(i2, CellCount).value) <> ""
                        'ErrorCheck = IsError(AoExcel.exceldoc.activesheet.cells(i2, CellCount))
                        'If CStr(AoExcel.exceldoc.activesheet.cells(i2, CellCount).value) = "-2146826281" Or CStr(AoExcel.exceldoc.activesheet.cells(i2, CellCount).value) = "-2146826273" Then
                        If IsNumeric(AoExcel.exceldoc.activesheet.cells(i2, CellCount).value) = True Then
                            If AoExcel.exceldoc.activesheet.cells(i2, CellCount).value < 0 Then
                                Dim bc_cs_message As New bc_cs_message("Blue Curve Insight", "Can not submit data. Data Error in the " + CStr(AoExcel.exceldoc.activesheet.cells(12, CellCount).value) + " column.", bc_cs_message.MESSAGE)
                                Exit Sub
                            End If
                        End If
                        i2 = i2 + 1
                    End While
                End If

            Next i1


            If uxCheckAdjust.Checked = True Then

                Dim ocommentary As New bc_cs_activity_log("bc_am_corp_submit_frm", "bok_MouseUp", bc_cs_activity_codes.COMMENTARY, "Submitting corporate action")

                Me.Cursor = Cursors.WaitCursor

                SubmitData(corp_action)

                If corp_action.Submitted <> 1 Then
                    Exit Sub
                End If

                REM set the action as submitted
                AoExcel.exceldoc.sheets(2).select()
                corp_action.CalcNewShares = AoExcel.exceldoc.activesheet.cells(4, 11).value()
                corp_action.CalcCapIncreased = AoExcel.exceldoc.activesheet.cells(5, 11).value()
                corp_action.CalcStockPrice = AoExcel.exceldoc.activesheet.cells(6, 11).value()
                corp_action.CalcCashDividend = AoExcel.exceldoc.activesheet.cells(7, 11).value()
                corp_action.CalcAdjustment = AoExcel.exceldoc.activesheet.cells(8, 2).value()
                corp_action.CalcfvOldMcap = AoExcel.exceldoc.activesheet.cells(8, 8).value()
                corp_action.CalcfvAdjusted = AoExcel.exceldoc.activesheet.cells(8, 11).value()
                corp_action.CalcfvAdjustment = AoExcel.exceldoc.activesheet.cells(9, 2).value()

                If bc_cs_central_settings.connection_method = bc_cs_central_settings.ADO Then
                    corp_action.write_mode = 1
                    corp_action.db_write()
                ElseIf bc_cs_central_settings.connection_method = bc_cs_central_settings.SOAP Then
                    corp_action.write_mode = 1
                    corp_action.tmode = bc_cs_soap_base_class.tWRITE
                    If corp_action.transmit_to_server_and_receive(corp_action, True) = False Then
                        Exit Sub
                    End If
                Else
                    Exit Sub
                End If

                Me.Close()
            Else

                Dim omsg As New bc_cs_message("Blue Curve", "Please confirm adjustments.", bc_cs_message.MESSAGE, False, False, "Yes", "", True)
            End If



        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_corp_submit_frm", "bok_MouseUp", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
        End Try

    End Sub

    Private Sub bcancel_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles bcancel.MouseDown
        Me.bcancel.Visible = True
        Me.Hide()
    End Sub

    Private Sub bcancel_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles bcancel.MouseUp
        Dim ocommentary As New bc_cs_activity_log("bc_am_corp_submit_frm", "bcancel_MouseUp", bc_cs_activity_codes.COMMENTARY, "Cancel submit action")
        Me.Close()
    End Sub

    Private Sub bok_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bok.Click

    End Sub
End Class



