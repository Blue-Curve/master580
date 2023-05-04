Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports BlueCurve.Core.AS
Imports BlueCurve.Create.AM
Public Class bc_am_dx_regular_reports
    Implements Ibc_am_dx_regular_reports
    Event create(doc As bc_om_document) Implements Ibc_am_dx_regular_reports.create
    Event delete(report_id As Integer) Implements Ibc_am_dx_regular_reports.delete
    Event edit(report As bc_om_regular_report) Implements Ibc_am_dx_regular_reports.edit
    Event setup(bglobal As Boolean) Implements Ibc_am_dx_regular_reports.setup
    Dim _reports As bc_om_regular_reports
    Dim _badmin As Boolean
    Public Function load_view(reports As bc_om_regular_reports, bglobal As Boolean, badmin As Boolean) Implements Ibc_am_dx_regular_reports.load_view

        _reports = reports
        _badmin = badmin
        Me.bdelete.Enabled = False
        Me.bedit.Enabled = False
        If bglobal = False And Me.rtype.SelectedIndex <> 1 Then
            Me.rtype.SelectedIndex = 1
        ElseIf bglobal = True And Me.rtype.SelectedIndex <> 0 Then
            Me.rtype.SelectedIndex = 0
        Else
            load_reports()
        End If

    End Function
    Private Sub load_reports()
        Try
            uxreports.Nodes.Clear()
            uxreports.BeginUnboundLoad()
            Dim nc As Integer = 0

            For i = 0 To _reports.regular_reports.Count - 1
                Dim tln As Object = Nothing
                With _reports.regular_reports(i)
                    If Me.rtype.SelectedIndex = 1 Then
                        If .bglobal = 0 Then
                            uxreports.Nodes.Add()
                            uxreports.Nodes(nc).SetValue(0, .name)
                            For j = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                                If bc_am_load_objects.obc_pub_types.pubtype(j).id = .doc.pub_type_id Then
                                    uxreports.Nodes(nc).SetValue(1, bc_am_load_objects.obc_pub_types.pubtype(j).name)
                                    Exit For
                                End If
                            Next
                            uxreports.Nodes(nc).SetValue(2, .create_user_name)
                            uxreports.Nodes(nc).SetValue(3, .update_user_name)
                            uxreports.Nodes(nc).SetValue(4, get_author_list(.doc.authors))
                            uxreports.Nodes(nc).Tag = .id
                            nc = nc + 1
                        End If
                    Else
                        If .bglobal = True Then
                            uxreports.Nodes.Add()
                            uxreports.Nodes(nc).SetValue(0, .name)
                            For j = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                                If bc_am_load_objects.obc_pub_types.pubtype(j).id = .doc.pub_type_id Then
                                    uxreports.Nodes(nc).SetValue(1, bc_am_load_objects.obc_pub_types.pubtype(j).name)
                                    Exit For
                                End If
                            Next
                            uxreports.Nodes(nc).SetValue(2, .create_user_name)
                            uxreports.Nodes(nc).SetValue(3, .update_user_name)
                            uxreports.Nodes(nc).SetValue(4, get_author_list(.doc.authors))
                            uxreports.Nodes(nc).Tag = .id
                            nc = nc + 1
                        End If
                    End If
                End With
            Next
            uxreports.EndUnboundLoad()
            Me.bcontinue.Enabled = False
            If uxreports.Selection.Count = 1 Then
                set_buttons(uxreports.Selection(0).Tag)

            End If


        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_dx_regular_reports", "load_reports", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try
    End Sub
    Function get_author_list(authors As List(Of bc_om_user)) As String
        Dim astr As String = ""
        For i = 0 To authors.Count - 1
            For j = 0 To bc_am_load_objects.obc_users.user.Count - 1
                If authors(i).id = bc_am_load_objects.obc_users.user(j).id Then
                    If astr <> "" Then
                        astr = astr + "; "
                    End If
                    If bc_am_load_objects.obc_pub_types.process_switches.surname_first = True Then
                        astr = astr + bc_am_load_objects.obc_users.user(j).surname + ", " + bc_am_load_objects.obc_users.user(j).first_name
                    Else
                        astr = astr + bc_am_load_objects.obc_users.user(j).first_name + " " + bc_am_load_objects.obc_users.user(j).surname
                    End If
                    Exit For
                End If
            Next
        Next
        get_author_list = astr
    End Function

    Private Sub bcancel_Click(sender As Object, e As EventArgs) Handles bcancel.Click
        Me.Hide()
    End Sub

    Private Sub bcontinue_Click(sender As Object, e As EventArgs) Handles bcontinue.Click
        For i = 0 To _reports.regular_reports.Count - 1
            If _reports.regular_reports(i).id = Me.uxreports.Selection(0).Tag Then
                RaiseEvent create(_reports.regular_reports(i).doc)
                Me.Hide()
                Exit For
            End If
        Next
    End Sub

    Private Sub rtype_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rtype.SelectedIndexChanged
        load_reports()
        Me.bnew.Visible = True
        Me.bedit.Visible = True
        Me.bdelete.Visible = True
        If Me.rtype.SelectedIndex = 0 And _badmin = False Then
            Me.bnew.Visible = False
            Me.bedit.Visible = False
            Me.bdelete.Visible = False

        End If

    End Sub

    Private Sub bnew_Click(sender As Object, e As EventArgs) Handles bnew.Click
        If rtype.SelectedIndex = 0 Then
            RaiseEvent setup(True)
        Else
            RaiseEvent setup(False)
        End If

    End Sub
    Private Sub set_buttons(report_id As Integer)
        Try
            Me.bdelete.Enabled = False
            Me.bedit.Enabled = False
            Me.bcontinue.Enabled = True



            'For i = 0 To _reports.regular_reports.Count - 1
            '    If _reports.regular_reports(i).id = report_id AndAlso _reports.regular_reports(i).user_id = bc_cs_central_settings.logged_on_user_id Then
            '        Me.bdelete.Enabled = True
            '        Me.bedit.Enabled = True

            '        Exit For
            '    End If


            'Next

            For i = 0 To _reports.regular_reports.Count - 1
                If _reports.regular_reports(i).id = report_id Then
                    Me.bdelete.Enabled = True
                    Me.bedit.Enabled = True

                    Exit For
                End If


            Next


        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("uxreports", "set_buttons", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try
    End Sub
    Private Sub uxreports_FocusedNodeChanged(sender As Object, e As DevExpress.XtraTreeList.FocusedNodeChangedEventArgs) Handles uxreports.FocusedNodeChanged
        Try


            set_buttons(e.Node.Tag)



        Catch ex As Exception

        End Try
    End Sub

    Private Sub bdelete_Click(sender As Object, e As EventArgs) Handles bdelete.Click
        Dim omsg As New bc_cs_message("Blue Curve", "Are you sure you wish to delete regular report: " + Me.uxreports.Selection(0).GetValue(0), bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
        If omsg.cancel_selected = False Then
            RaiseEvent delete(Me.uxreports.Selection(0).Tag)
        End If
    End Sub

    Private Sub bedit_Click(sender As Object, e As EventArgs) Handles bedit.Click
        For i = 0 To _reports.regular_reports.Count - 1
            If _reports.regular_reports(i).id = Me.uxreports.Selection(0).Tag Then
                RaiseEvent edit(_reports.regular_reports(i))
                Exit For
            End If
        Next

    End Sub
End Class
Public Class Cbc_am_dx_regular_reports
    WithEvents _view As Ibc_am_dx_regular_reports
    Public selected_doc As bc_om_document = Nothing
    Dim _badmin As Boolean
    Dim allow_disclosure_tab
    Public Function load_data(view As Ibc_am_dx_regular_reports, badmin As Boolean) As Boolean
        load_data = False
        Try
            _view = view
            _badmin = badmin

            load_data_from_system()

            load_data = True
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_dx_regular_reports", "load_data", bc_cs_error_codes.USER_DEFINED, ex.Message)


        End Try
    End Function
    Private Function load_data_from_system() As bc_om_regular_reports
        Try
            Dim rr As New bc_om_regular_reports
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                rr.db_read()
            Else
                rr.tmode = bc_cs_soap_base_class.tREAD
                If rr.transmit_to_server_and_receive(rr, True) = False Then
                    Exit Function
                End If
            End If
            allow_disclosure_tab = rr.allow_disclosure_tab

            Dim i As Integer = 0
            Dim pfound As Boolean = False
            While i < rr.regular_reports.Count
                pfound = False
                For j = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                    If bc_am_load_objects.obc_pub_types.pubtype(j).id = rr.regular_reports(i).doc.pub_type_id Then
                        For k = 0 To bc_am_load_objects.obc_prefs.bus_areas.Count - 1
                            If bc_am_load_objects.obc_prefs.bus_areas(k) = bc_am_load_objects.obc_pub_types.pubtype(j).bus_area_id Then
                                pfound = True
                                Exit For
                            End If
                        Next
                        If pfound = True Then
                            Exit For
                        End If
                    End If
                Next

                If pfound = False Then
                    rr.regular_reports.RemoveAt(i)
                    i = i - 1

                End If
                i = i + 1
            End While

            load_data_from_system = rr
            If bc_am_load_objects.obc_pub_types.process_switches.regular_report_admin_only = False Then
                _badmin = True
            End If

            If bc_am_load_objects.obc_pub_types.process_switches.regular_report_default_global = True Then
                _view.load_view(rr, True, _badmin)
            Else
                _view.load_view(rr, False, _badmin)
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_dx_regular_reports", "load_data_from_system", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try
    End Function
    Private Sub create(doc As bc_om_document) Handles _view.create
        doc.allow_disclosures = allow_disclosure_tab
        selected_doc = doc
    End Sub
    Private Sub delete_doc(report_id As Integer) Handles _view.delete
        Try
            Dim orep As New bc_om_regular_report
            orep.id = report_id

            orep.write_mode = bc_om_regular_report.EWRITE_MODE.DELETE
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                orep.db_write()
            Else
                orep.tmode = bc_cs_soap_base_class.tWRITE
                If orep.transmit_to_server_and_receive(orep, True) = False Then
                    Exit Sub
                End If
            End If
            _view.load_view(load_data_from_system, True, _badmin)
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_dx_regular_reports", "delete_doc", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try
    End Sub
    Private Sub setup(bglobal As Boolean) Handles _view.setup
        Try
            REM ===================================
            Dim sdoc As New bc_om_document
            Dim osubmit As New bc_dx_as_categorize
            osubmit.show_stage_change = False
            osubmit.show_local_submit = False
            osubmit.enable_pub_types = True
            osubmit.enable_lead_entity = True

            sdoc.allow_disclosures = allow_disclosure_tab
            sdoc.allow_dist_channels = False


            sdoc.pub_type_id = 0
            sdoc.id = 0
            sdoc.stage = 1
            sdoc.originating_author = bc_cs_central_settings.logged_on_user_id
            Dim ouser As New bc_om_user
            ouser.id = sdoc.originating_author
            sdoc.authors.Add(ouser)
            osubmit._doc = sdoc
            Dim pts As New List(Of String)
            For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                If bc_am_load_objects.obc_pub_types.pubtype(i).support_doc_only = False Then
                    pts.Add(bc_am_load_objects.obc_pub_types.pubtype(i).name)
                End If
            Next
            osubmit.set_pub_types = pts

            osubmit.caption = "Blue Curve Process - Create Regular Report Template"
            osubmit.ok_button_caption = "Create"
            osubmit.file = False
            osubmit.regular_report_mode = True

            osubmit.ShowDialog()
            If Not osubmit.ok_selected Then
                Exit Sub
            End If
            Dim orep As New bc_om_regular_report
            orep.write_mode = bc_om_regular_report.EWRITE_MODE.UPDATE
            orep.doc = sdoc
            orep.name = osubmit.regular_report_title
            orep.bglobal = bglobal

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                orep.db_write()
            Else
                orep.tmode = bc_cs_soap_base_class.tWRITE
                If orep.transmit_to_server_and_receive(orep, True) = False Then
                    Exit Sub
                End If
            End If
            Dim omsg As New bc_cs_message("Blue Curve", "Regular Report: " + orep.name + " Created.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            _view.load_view(load_data_from_system, orep.bglobal, _badmin)
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_dx_regular_reports", "setup", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try
    End Sub
    Private Sub edit(rep As bc_om_regular_report) Handles _view.edit
        Try
            REM ===================================
            'Dim sdoc As New bc_om_document
            Dim oname As String
            oname = rep.name
            Dim osubmit As New bc_dx_as_categorize
            osubmit.show_stage_change = False
            osubmit.show_local_submit = False
            osubmit.enable_pub_types = True
            osubmit.enable_lead_entity = True
            rep.doc.allow_disclosures = allow_disclosure_tab
            osubmit._doc = rep.doc



            Dim pts As New List(Of String)

            For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                If bc_am_load_objects.obc_pub_types.pubtype(i).id = rep.doc.pub_type_id Then
                    pts.Add(bc_am_load_objects.obc_pub_types.pubtype(i).name)
                    Exit For
                End If
            Next
            osubmit.set_pub_types = pts
            osubmit.regular_report_title = rep.name
            osubmit.regular_report_bglobal = rep.bglobal

            osubmit.caption = "Blue Curve Process - Update Regular Report Template"
            osubmit.ok_button_caption = "Update"
            osubmit.file = False
            osubmit.regular_report_mode = True

            osubmit.ShowDialog()
            If Not osubmit.ok_selected Then
                Exit Sub
            End If

            rep.write_mode = bc_om_regular_report.EWRITE_MODE.UPDATE
            rep.name = osubmit.regular_report_title
            rep.bglobal = osubmit.regular_report_bglobal

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                rep.db_write()
            Else
                rep.tmode = bc_cs_soap_base_class.tWRITE
                If rep.transmit_to_server_and_receive(rep, True) = False Then
                    Exit Sub
                End If
            End If
            Dim omsg As New bc_cs_message("Blue Curve", "Regular Report: " + oname + " Updated.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            _view.load_view(load_data_from_system, rep.bglobal, _badmin)
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_dx_regular_reports", "edit", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try
    End Sub
End Class
Public Interface Ibc_am_dx_regular_reports
    Function load_view(reports As bc_om_regular_reports, bglobal As Boolean, badmin As Boolean)
    Event create(doc As bc_om_document)
    Event delete(report_id As Integer)
    Event edit(report As bc_om_regular_report)
    Event setup(bglobal As Boolean)
End Interface
