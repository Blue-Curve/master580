
Imports BlueCurve.Core.OM
Imports BlueCurve.Core.CS
Imports BlueCurve.Core.AS
Imports System.Windows.Forms
Imports System.Threading

Public Class bc_am_corp_build

    Public Sub New(ByVal showForm As Boolean, ByVal aoObject As Object, ByVal aoType As String, ByVal validateOnly As Boolean)
        Dim slog = New bc_cs_activity_log("bc_am_corp_build", "new", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim ao_excel As bc_ao_in_excel
        Dim omessage As bc_cs_message
        Dim workNumber As String
        Dim workMoney As String
        Dim workRatio As String
        Dim workExdate As Date = Nothing

        Dim workFromdate As Date = Nothing
        Dim workTodate As Date = Nothing
        Dim workDeleteClass As Integer = -99
        Dim workDeleteClassText As String = ""
        Dim deletedRowCount As Long
        Dim workEntity As bc_om_entity = Nothing

        Try

            If IsNothing(aoObject) Then
                omessage = New bc_cs_message("Blue Curve insight", "Workbook must be Open", bc_cs_message.MESSAGE)
                Exit Sub
            End If

            Dim frmcorpbuild As New bc_am_corp_action_frm(aoObject)

            If showForm = True Then

                frmcorpbuild.ShowDialog()

                If frmcorpbuild.BuildAction = True Then

                    workNumber = frmcorpbuild.uxNumber.Text
                    workMoney = frmcorpbuild.uxMoney.Text
                    workRatio = frmcorpbuild.uxRatioB.Text
                    If workNumber = "" Then
                        workNumber = "0"
                    End If
                    If workMoney = "" Then
                        workMoney = "0"
                    End If
                    If workRatio = "" Then
                        workRatio = "0"
                    End If
                    workExdate = frmcorpbuild.uxExDate.Value

                    If frmcorpbuild.SelectedAction.EventType = "DELETE" Then

                        '--------
                        'Data Deletion
                        '--------
                        workFromdate = frmcorpbuild.uxDateFrom.Value
                        workTodate = frmcorpbuild.uxDateTo.Value
                        workDeleteClass = frmcorpbuild.DeletionClass
                        workDeleteClassText = frmcorpbuild.uxCfType2.Text

                        If workFromdate.Date = workTodate.Date Then
                            omessage = New bc_cs_message("Blue Curve insight", workDeleteClassText + " data will be deleted for the " + workFromdate.ToShortDateString + ". Do you wish to continue?", bc_cs_message.MESSAGE, True)
                        Else
                            omessage = New bc_cs_message("Blue Curve insight", workDeleteClassText + " data will be deleted between the " + workFromdate.ToShortDateString + " and " + workTodate.ToShortDateString + ". Do you wish to continue?", bc_cs_message.MESSAGE, True)
                        End If
                        If omessage.cancel_selected = False Then

                            deletedRowCount = 0
                            aoObject.Application.Cursor = 2


                            For Each workEntity In frmcorpbuild.SelectedEntityList

                                Dim corp_action As New bc_om_corp_action(frmcorpbuild.PrimaryFocus, workEntity.id, frmcorpbuild.SelectedAction.Id, CType(workNumber, Long), workExdate.Date, CType(workMoney, Double), workRatio)
                                corp_action.EntityDescription = workEntity.name
                                corp_action.EventDescriprion = frmcorpbuild.SelectedAction.Description
                                corp_action.EventType = frmcorpbuild.SelectedAction.EventType
                                corp_action.DeleteClassId = workDeleteClass
                                corp_action.DeleteFrom = workFromdate.Date
                                corp_action.DeleteTo = workTodate.Date


                                REM Write action detail
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
                                    REM put a msessage invalid connection
                                    Exit Sub
                                End If

                                REM get details of the action
                                If bc_cs_central_settings.connection_method = bc_cs_central_settings.ADO Then
                                    corp_action.db_read()
                                ElseIf bc_cs_central_settings.connection_method = bc_cs_central_settings.SOAP Then
                                    corp_action.tmode = bc_cs_soap_base_class.tREAD
                                    If corp_action.transmit_to_server_and_receive(corp_action, True) = False Then
                                        Exit Sub
                                    End If
                                Else
                                    REM put a msessage invalid connection
                                    Exit Sub
                                End If

                                If bc_cs_central_settings.connection_method = bc_cs_central_settings.ADO Then
                                    corp_action.write_mode = 2
                                    corp_action.db_write()
                                    deletedRowCount = deletedRowCount + CLng(corp_action.RowsDeleted)
                                ElseIf bc_cs_central_settings.connection_method = bc_cs_central_settings.SOAP Then
                                    corp_action.write_mode = 2
                                    corp_action.tmode = bc_cs_soap_base_class.tWRITE
                                    If corp_action.transmit_to_server_and_receive(corp_action, True) = False Then
                                        Exit Sub
                                    End If
                                    deletedRowCount = deletedRowCount + CLng(corp_action.RowsDeleted)
                                Else
                                    REM put a msessage invalid connection
                                    Exit Sub
                                End If
                            Next
                            omessage = New bc_cs_message("Blue Curve insight", "Deletion Successful. " + LTrim(Str(deletedRowCount)) + " Records Deleted.", bc_cs_message.MESSAGE)
                        End If

                        aoObject.Application.Cursor = -4143
                    Else

                        '--------
                        'Corp Action Adjustment and All Historical data excel sheets 
                        '--------

                        workEntity = frmcorpbuild.SelectedEntityList.Item(0)

                        Dim corp_action As New bc_om_corp_action(frmcorpbuild.PrimaryFocus, workEntity.id, frmcorpbuild.SelectedAction.Id, CType(workNumber, Long), workExdate.Date, CType(workMoney, Double), workRatio)
                        corp_action.EntityDescription = frmcorpbuild.SelectedEntity.name
                        corp_action.EventDescriprion = frmcorpbuild.SelectedAction.Description
                        corp_action.EventType = frmcorpbuild.SelectedAction.EventType

                        REM Write action detail
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
                            REM put a msessage invalid connection
                            Exit Sub
                        End If

                        REM get details of the action
                        If bc_cs_central_settings.connection_method = bc_cs_central_settings.ADO Then
                            corp_action.db_read()
                        ElseIf bc_cs_central_settings.connection_method = bc_cs_central_settings.SOAP Then
                            corp_action.tmode = bc_cs_soap_base_class.tREAD
                            If corp_action.transmit_to_server_and_receive(corp_action, True) = False Then
                                Exit Sub
                            End If
                        Else
                            REM put a msessage invalid connection
                            Exit Sub
                        End If

                        REM Build corparate action and 'All historical data' excel sheets 
                        ao_excel = New bc_ao_in_excel(aoObject)
                        ao_excel.BuildCorpAction(corp_action)

                    End If
                End If
            End If

            frmcorpbuild.Dispose()

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_corp_build", "new", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Sub

End Class

Public Class bc_am_corp_submit

    Public Sub New(ByVal showForm As Boolean, ByVal aoObject As Object, ByVal aoType As String, ByVal validateOnly As Boolean)
        Dim slog = New bc_cs_activity_log("bc_am_corp_submit", "new", bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim omessage As bc_cs_message

        Try

            If IsNothing(aoObject) Then
                omessage = New bc_cs_message("Blue Curve insight", "Workbook must be Open", bc_cs_message.MESSAGE)
                Exit Sub
            End If

            Dim frmcorpbuild As New bc_am_corp_submit_frm(aoObject)

            If frmcorpbuild.ActionId = 0 Then
                frmcorpbuild.Dispose()
                Exit Sub
            End If

            If showForm = True Then
                frmcorpbuild.ShowDialog()
            End If

            frmcorpbuild.Dispose()

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_corp_submit", "new", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Sub

End Class