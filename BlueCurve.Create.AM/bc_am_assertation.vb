Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM




Public Class bc_am_attestation
    Implements Ibc_am_attestation
    Dim _config As bc_om_attestation_config
    Event save(config As bc_om_attestation_config) Implements Ibc_am_attestation.save
    Event cancel() Implements Ibc_am_attestation.cancel
    Public qcontrols As New List(Of bc_am_assertation_q)
    WithEvents qo As bc_am_assertation_q

    Public Function load_view(config As bc_om_attestation_config, title As String) Implements Ibc_am_attestation.load_view
        Try
            _config = config
            Me.Text = "Attestation for: " + title
            Me.lgroup.Text = config.group
            Me.lq.Text = config.questionnaire
            Dim item As DevExpress.XtraEditors.Controls.RadioGroupItem
            If _config.edit_mode = True Then
                Me.ledit.Text = "You have already Attested this document. Please edit your answers below."
                Me.btnsubmit.Text = "Update"
            End If

               Dim dp As System.Drawing.Point
            dp.X = 10
            dp.Y = 10

            Dim pcount As Integer = 0
            For i = 0 To _config.questions.Count - 1
                With _config.questions(i)
                    qo = New bc_am_assertation_q
                    qo.BorderStyle = Windows.Forms.BorderStyle.None


                    qo.Anchor = Windows.Forms.AnchorStyles.Top + Windows.Forms.AnchorStyles.Left + Windows.Forms.AnchorStyles.Right
                    qo.Width = Me.Width

                    qo.Parent = Me.panel1
                    qo.BringToFront()

                    qcontrols.Add(qo)
                    qo.Location = dp

                    AddHandler qo.uxo.SelectedIndexChanged, AddressOf optionchanged_handler



                    Me.panel1.Controls.Add(qo)
                    qo.uxo.Tag = CStr(i)
                    qo.uxq.Text = CStr(i + 1) + ") " + .text
                    qo.pfail.Visible = False
                    qo.ppass.Visible = False
                    For j = 0 To .options.Count - 1
                        item = New DevExpress.XtraEditors.Controls.RadioGroupItem(j, CStr(.options(j).text))
                        qo.uxo.Properties.Items.Add(item)
                        If .options(j).text = .default_option Then
                            qo.uxo.SelectedIndex = j
                            If .options(j).pass = True Then
                                'qo.ppass.Visible = True
                                pcount = pcount + 1
                            Else
                                qo.ppass.Visible = False

                            End If
                        End If
                    Next



                    dp.Y = dp.Y + qo.Height + 10
                End With
            Next
            If pcount = _config.questions.Count Then
                'Me.ppass.Visible = True
                'Me.pfail.Visible = True
            End If

            load_view = True
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_attestation", "load_view", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try

    End Function
    Private Sub optionchanged_handler(ByVal sender As DevExpress.XtraEditors.RadioGroup, ByVal e As System.EventArgs)
        If sender.SelectedIndex = -1 Then
            Exit Sub
        End If

        If _config.questions(CInt(sender.Tag)).options(sender.SelectedIndex).pass = True Then
            'qcontrols(CInt(sender.Tag)).pfail.Visible = False
            ''qcontrols(CInt(sender.Tag)).ppass.Visible = True

        Else
            'qcontrols(CInt(sender.Tag)).pfail.Visible = True
            'qcontrols(CInt(sender.Tag)).ppass.Visible = False
        End If
        Me.pfail.Visible = False
        Me.ppass.Visible = False
        Dim pcount As Integer = 0
        For i = 0 To qcontrols.Count - 1
            If qcontrols(i).ppass.Visible = False Then
                'Me.pfail.Visible = True
                Exit Sub
            ElseIf qcontrols(i).ppass.Visible = True Then
                pcount = pcount + 1
            End If
        Next
        'If pcount = qcontrols.Count Then
        '    Me.ppass.Visible = True
        'Else
        '    Me.pfail.Visible = True
        'End If

    End Sub
    Private Sub btnsubmit_Click(sender As Object, e As EventArgs) Handles btnsubmit.Click
        Try
            For i = 0 To _config.questions.Count - 1
                If qcontrols(i).uxo.SelectedIndex = -1 Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Not all questions have been answered.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub
                End If
            Next
            Dim pcount As Integer = 0
            For i = 0 To _config.questions.Count - 1
                _config.questions(i).option_selected_text = _config.questions(i).options(qcontrols(i).uxo.SelectedIndex).text
                If qcontrols(i).ppass.Visible = True Then
                    _config.questions(i).pass = True
                    pcount = pcount + 1
                Else
                    _config.questions(i).pass = False
                End If
            Next
            If pcount = _config.questions.Count Then
                _config.pass = True
            Else
                _config.pass = False

            End If

            Me.Cursor = Windows.Forms.Cursors.WaitCursor
            RaiseEvent save(_config)
            Me.Cursor = Windows.Forms.Cursors.Default


            Me.Hide()
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_attestation", "submit", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try

    End Sub
    Private Sub bc_am_assertation_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub bcancel_Click(sender As Object, e As EventArgs) Handles bcancel.Click
        RaiseEvent cancel()
        Me.Hide()

    End Sub

    
End Class
Public Class Cbc_am_attestation
    WithEvents _view As Ibc_am_attestation
    Public _config As New bc_om_attestation_config
    Public err_text As String
    Public _cancel As Boolean = False
    Public Function load_data(doc_id As Long, user_id As Long, title As String, view As Ibc_am_attestation)
        load_data = False
        _view = view
        _config.doc_id = doc_id
        _config.user_id = bc_cs_central_settings.logged_on_user_id

        Try

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                _config.db_read()
            Else
                _config.tmode = bc_cs_soap_base_class.tREAD
                If _config.transmit_to_server_and_receive(_config, True) = False Then
                    Exit Function
                End If
            End If
            If _config.message <> "" Then
                Dim omsg As New bc_cs_message("Blue Curve", _config.message, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Return False
            End If

        Catch ex As Exception
            err_text = "Failed to get data for assertions: " + ex.Message
            Exit Function
        End Try
        Return _view.load_view(_config, title)

    End Function
    Public Function save(config As bc_om_attestation_config) Handles _view.save
        _config = config
        _cancel = False
        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
            _config.db_write()
        Else
            _config.tmode = bc_cs_soap_base_class.tWRITE


            If _config.transmit_to_server_and_receive(_config, True) = False Then
                Exit Function
            End If
        End If
    End Function
    Public Function cancel() Handles _view.cancel
        _cancel = True
    End Function
End Class
Public Interface Ibc_am_attestation
    Function load_view(config As bc_om_attestation_config, title As String)
    Event save(config As bc_om_attestation_config)
    Event cancel()
End Interface
