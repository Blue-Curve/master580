Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports BlueCurve.Insight.AM

Imports System.Threading
Imports System.Windows.Forms

Public Class bc_am_distribution
    Implements Ibc_am_distribution
    Dim _doc_distribution As New bc_om_distribution
    Dim _poll_enabled As Boolean = False
    Dim _channel_id As Integer = 0
    Const POLL_INTERVAL = 3000
    Dim _pas As Thread
    Event add_recipient(channel_id As Integer) Implements Ibc_am_distribution.add_recipient
    Event rem_recipient(channel_id As Integer, recipient_id As String) Implements Ibc_am_distribution.rem_recipient
    Event resend_recipient(client_id As Long) Implements Ibc_am_distribution.resend_recipient
    Event resend_channel(channel_id As Long) Implements Ibc_am_distribution.resend_channel
    Event cancel_distribution() Implements Ibc_am_distribution.cancel_distribution
    Const cancel_buttom_xpos = 674




    
    Public Function load_view(doc_distribution As bc_om_distribution, title As String) As Boolean Implements Ibc_am_distribution.load_view
        Try
            load_view = False
            _doc_distribution = doc_distribution



            _pas = New Thread(Sub() poll())
            Me.Text = "Distribution - " + title
            _pas.Start()
            _doc_distribution = doc_distribution
            Select Case _doc_distribution.distribution_type
                Case bc_om_distribution.reach_distribution_type.Manual
                    Me.ltype.Text = "Manual"
                Case bc_om_distribution.reach_distribution_type.Automatic
                    Me.ltype.Text = "Automatic"
            End Select
            If _doc_distribution.distribution_date.Year <> 1 Then
                If _doc_distribution.distribution_date < Now.ToUniversalTime Then
                    Me.lddate.Text = "On Publish"
                Else
                    Me.lddate.Text = Format(_doc_distribution.distribution_date.ToLocalTime, "dd MMM yyyy HH:mm:ss")
                End If
            Else
                Me.lddate.Text = "not set"
            End If
                Me.uxdistribute.Visible = False

           

                update_view()
                set_up_tab(Nothing)
            load_view = True

         
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_distribution", "load_view", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Function
    Public Function set_view(doc_distribution As bc_om_distribution) As Boolean Implements Ibc_am_distribution.set_view
        Try

            _doc_distribution = doc_distribution


            update_view()

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_distribution", "set_view", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try
    End Function
    Private Delegate Sub RetrieveDocsDelegate(ByVal update_display As Boolean, ByVal always_refresh As Boolean, ByVal from_poll As Boolean)

    Public Function update_view() As Boolean
        Try

            REM if called from poll marshall to UI thread
            If Me.lstatus.InvokeRequired Then
                Dim ed As New RetrieveDocsDelegate(AddressOf update_view)
                Me.lstatus.Invoke(ed, New Object() {False, False, True})
                Exit Function
            End If



            Me.lpoll.Text = "last poll " + Format(Now, "dd-MMM-yyyy HH:mm:ss")

            Me.badd.Visible = False
            Me.bRemove.Visible = False
            Me.uxgenmaillist.Visible = False

            Me.bmerge.Visible = False
            Me.uxchannels.Enabled = True
            update_view = False
            Me.lstatus.Visible = True
            Me.lsuccess.Visible = False
            Me.lfail.Visible = False
            Me.lpoll.Visible = False
            Me.bdistcancel.Visible = False
            _poll_enabled = False
            Me.lstatus.Text = get_status(_doc_distribution.status)
            Me.uxdistribute.Visible = False
            Dim dp As System.Drawing.Point
            dp.X = cancel_buttom_xpos
            dp.Y = Me.bdistcancel.Location.Y
            Me.bdistcancel.Location = dp
            Me.uxtab.TabPages(1).Text = "Email Preview"
            Me.lpreview.Text = "Not Generated"
            Me.bresendchannel.Visible = False
            REM email preview
            For i = 0 To _doc_distribution.channels.Count - 1
                If _doc_distribution.channels(i).preview_status = 1 Or _doc_distribution.channels(i).preview_status = 2 Then
                    set_poll()
                    Exit For
                End If
            Next
            REM

            Select Case _doc_distribution.status
                Case bc_om_distribution.reach_status_codes.Preparing

                    Me.badd.Visible = True
                    Me.uxgenmaillist.Visible = True

                    Me.bmerge.Visible = True
                Case bc_om_distribution.reach_status_codes.Mail_List_Generating
                    set_poll()


                Case bc_om_distribution.reach_status_codes.Awaiting_Distribution_Time
                    Me.uxdistribute.Text = "Distribute Now"
                    set_poll()
                    Me.bdistcancel.Visible = True
                    Me.uxdistribute.Visible = True


                Case bc_om_distribution.reach_status_codes.Ready_To_Distribute
                    set_poll()
                    Me.bdistcancel.Visible = True
                    dp.X = Me.uxdistribute.Location.X
                    Me.bdistcancel.Location = dp


                Case bc_om_distribution.reach_status_codes.Distributing
                    set_poll()

                Case bc_om_distribution.reach_status_codes.Distributed
                    Me.lstatus.Visible = False
                    Me.lsuccess.Visible = True
                    Me.uxdistribute.Visible = True
                    Me.uxdistribute.Text = "Redistribute"
                    Me.uxtab.TabPages(1).Text = "Email View"
                    'Me.lpreview.Text = "Html Email Body"
                    Me.bresendchannel.Visible = True
                Case bc_om_distribution.reach_status_codes.Distribution_Failed
                    Me.lstatus.Visible = False
                    Me.lfail.Visible = True
                    Me.uxdistribute.Visible = True
                    Me.uxdistribute.Text = "Redistribute"
                    Me.bresendchannel.Visible = True
                Case bc_om_distribution.reach_status_codes.Ready_for_Mailing_List
                    set_poll()
                Case bc_om_distribution.reach_status_codes.Mail_List_Complete
                    Me.badd.Visible = True
                    Me.uxgenmaillist.Visible = True

                    Me.bmerge.Visible = True
                Case bc_om_distribution.reach_status_codes.Sending_Emails
                    set_poll()
                Case bc_om_distribution.reach_status_codes.Ready_To_Distribute_Manual
                    Me.badd.Visible = True
                    Me.uxgenmaillist.Visible = True

                    Me.bmerge.Visible = True
                    Me.uxdistribute.Text = "Distribute"
                    Me.uxdistribute.Visible = True
            End Select



            If _doc_distribution.stage_change_date.Year <> 1900 Then
                Me.lsdate.Text = Format(_doc_distribution.stage_change_date.ToLocalTime, "dd MMM yyyy HH:mm:ss")
            Else
                Me.lsdate.Text = "not set"
            End If

            load_channels()
            load_history()
            Me.uxtab.TabPages(1).PageEnabled = False
            Me.uxtab.TabPages(2).PageEnabled = False
            For i = 0 To _doc_distribution.channels.Count - 1
                If _doc_distribution.channels(i).channel_id = Me.uxchannels.Selection(0).Tag Then
                    If _doc_distribution.channels(i).targetted = True And _poll_enabled = False Then
                        Me.uxtab.TabPages(1).PageEnabled = True
                        Me.uxtab.TabPages(2).PageEnabled = True
                    End If

                    Exit For
                End If


            Next
            Try
                _channel_id = Me.uxchannels.Selection(0).Tag
            Catch
                _channel_id = 1
            End Try


            update_view = True
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_distribution", "load_view", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Function
    Private Sub set_poll()
        _poll_enabled = True
        Me.lpoll.Visible = True
        Me.uxtab.TabPages(1).PageEnabled = False
        Me.uxtab.TabPages(2).PageEnabled = False
        Me.uxchannels.Enabled = False

    End Sub
    Private Function load_history()
        uxhistory.BeginUnboundLoad()
        uxhistory.Nodes.Clear()



        For i = 0 To _doc_distribution.history.Count - 1
            Dim tln As Object = Nothing
            With _doc_distribution.history(i)
                uxhistory.Nodes.Add()
                uxhistory.Nodes(i).SetValue(0, Format(.da.ToLocalTime, "dd MMM yyyy HH:mm:ss"))
                uxhistory.Nodes(i).SetValue(1, .comment)
                uxhistory.Nodes(i).SetValue(2, .msg)
                uxhistory.Nodes(i).SetValue(3, .user)
            End With
        Next
        uxhistory.EndUnboundLoad()
    End Function
    Private Function get_status(status_id As bc_om_distribution.reach_status_codes) As String
        Select Case status_id
            Case bc_om_distribution.reach_status_codes.Awaiting_Distribution_Time
                get_status = "Await Distribution Time"
            Case bc_om_distribution.reach_status_codes.Distributed
                get_status = "Distributed"
            Case bc_om_distribution.reach_status_codes.Distributing
                get_status = "Distributing"
            Case bc_om_distribution.reach_status_codes.Distribution_Failed
                get_status = "Distribution Failed"
            Case bc_om_distribution.reach_status_codes.Mail_List_Complete
                get_status = "Mail List Complete"
            Case bc_om_distribution.reach_status_codes.Mail_List_Generating
                get_status = "Mail List Generating"
            Case bc_om_distribution.reach_status_codes.No_Distribution
                get_status = "No Distribution"
            Case bc_om_distribution.reach_status_codes.Preparing
                get_status = "Preparing"
            Case bc_om_distribution.reach_status_codes.Ready_for_Mailing_List
                get_status = "Ready for Mailing List"
            Case bc_om_distribution.reach_status_codes.Ready_To_Distribute
                get_status = "Ready to Distribute"
            Case bc_om_distribution.reach_status_codes.Ready_To_Distribute_Manual
                get_status = "Ready to Distribute Manual"
            Case bc_om_distribution.reach_status_codes.Sending_Emails
                get_status = "Sending Emails"
        End Select
    End Function
    Private Function load_channels()
        Try
            uxchannels.BeginUnboundLoad()
            uxchannels.ClearNodes()



            For i = 0 To _doc_distribution.channels.Count - 1
                Dim tln As Object = Nothing
                uxchannels.Nodes.Add()
                uxchannels.Nodes(i).SetValue(0, _doc_distribution.channels(i).name)
                uxchannels.Nodes(i).SetValue(1, get_status(_doc_distribution.channels(i).status))
                uxchannels.Nodes(i).StateImageIndex = -1

                If (_doc_distribution.channels(i).status = bc_om_distribution.reach_status_codes.Distributed) Then
                    uxchannels.Nodes(i).StateImageIndex = 1

                End If
                If _doc_distribution.channels(i).status = bc_om_distribution.reach_status_codes.Distribution_Failed Then
                    uxchannels.Nodes(i).StateImageIndex = 0
                End If
                If _doc_distribution.channels(i).status_change_date.Year > 1900 Then
                    uxchannels.Nodes(i).SetValue(2, Format(_doc_distribution.channels(i).status_change_date.ToLocalTime, "dd MMM yyyy HH:mm:ss"))
                End If
                If _doc_distribution.channels(i).list_date.Year > 1900 Then
                    uxchannels.Nodes(i).SetValue(3, Format(_doc_distribution.channels(i).list_date.ToLocalTime, "dd MMM yyyy HH:mm:ss"))
                End If
                uxchannels.Nodes(i).SetValue(4, _doc_distribution.channels(i).comment)
                uxchannels.Nodes(i).Tag = _doc_distribution.channels(i).channel_id
                If _doc_distribution.channels(i).targetted = 1 Then
                    If _doc_distribution.channels(i).status = bc_om_distribution.reach_status_codes.Distributed Or _doc_distribution.channels(i).preview_status = 1 Then

                    End If
                End If

            Next
            uxchannels.EndUnboundLoad()
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_distribution", "load_channles", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

        End Try
    End Function
    Private Function set_recipients(doc_distribution As bc_om_distribution, channel_id As Integer) As Boolean Implements Ibc_am_distribution.set_recipients


        Try
            _doc_distribution = doc_distribution
            uxrecipient.BeginUnboundLoad()
            uxrecipient.Nodes.Clear()
            For k = 0 To _doc_distribution.channels.Count - 1

                If _doc_distribution.channels(k).channel_id = channel_id Then
                    For i = 0 To _doc_distribution.channels(k).recipients.Count - 1

                        uxrecipient.Nodes.Add()
                        uxrecipient.Nodes(i).SetValue(0, _doc_distribution.channels(k).recipients(i).name)
                        uxrecipient.Nodes(i).SetValue(1, _doc_distribution.channels(k).recipients(i).organisation)
                        uxrecipient.Nodes(i).SetValue(2, _doc_distribution.channels(k).recipients(i).sent)
                        uxrecipient.Nodes(i).SetValue(3, _doc_distribution.channels(k).recipients(i).from)
                        If _doc_distribution.channels(k).recipients(i).add_mode = bc_om_distribution.LIST_ADD_MODE.MANUAL Then
                            uxrecipient.Nodes(i).SetValue(4, "Manual")
                            uxrecipient.Nodes(i).SetValue(5, Format(_doc_distribution.channels(k).recipients(i).list_add_date.ToLocalTime, "dd MMM yyyy HH:mm:ss"))
                        ElseIf _doc_distribution.channels(k).recipients(i).add_mode = bc_om_distribution.LIST_ADD_MODE.MERGE Then
                            uxrecipient.Nodes(i).SetValue(4, "Merged from: " + _doc_distribution.channels(k).recipients(i).merged_from_pub_type)
                            uxrecipient.Nodes(i).SetValue(5, Format(_doc_distribution.channels(k).recipients(i).list_add_date.ToLocalTime, "dd MMM yyyy HH:mm:ss"))
                        Else
                            uxrecipient.Nodes(i).SetValue(4, "Profile Match")
                            uxrecipient.Nodes(i).SetValue(5, Format(_doc_distribution.channels(k).list_date.ToLocalTime, "dd MMM yyyy HH:mm:ss"))

                        End If


                        'If _doc_distribution.channels(k).recipients(i).status_change_date = "1-1-1900" Or _doc_distribution.channels(k).recipients(i).status = 2 Then
                        '    uxrecipient.Nodes(i).SetValue(6, "Pending")
                        '    uxrecipient.Nodes(i).StateImageIndex = -1
                        'Else
                        '    If _doc_distribution.channels(k).recipients(i).status = 0 Then
                        '        uxrecipient.Nodes(i).SetValue(6, "Success")
                        '        uxrecipient.Nodes(i).StateImageIndex = 1

                        '    ElseIf _doc_distribution.channels(k).recipients(i).status = 1 Then
                        '        uxrecipient.Nodes(i).SetValue(6, "Failed")
                        '        uxrecipient.Nodes(i).StateImageIndex = 0
                        '    End If
                        '    uxrecipient.Nodes(i).SetValue(7, Format(_doc_distribution.channels(k).recipients(i).status_change_date.ToLocalTime, "dd MMM yyyy HH:mm:ss"))

                        'End If
                        uxrecipient.Nodes(i).SetValue(8, _doc_distribution.channels(k).recipients(i).comment)
                        uxrecipient.Nodes(i).Tag = _doc_distribution.channels(k).recipients(i).client_id
                    Next
                End If
                Exit For
            Next

            uxrecipient.EndUnboundLoad()


        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_distribution", "set_recipients", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.uxtab.TabPages(2).Text = "Mail List (" + CStr(uxrecipient.Nodes.Count) + " recipients"
        End Try
    End Function

    Private Sub poll()
        While True
            Thread.CurrentThread.Sleep(POLL_INTERVAL)

            If _poll_enabled = True Then

                RaiseEvent refresh_status()
            End If
        End While
    End Sub

    Private Sub bcancel_Click(sender As Object, e As EventArgs) Handles bcancel.Click

        Me.Close()


    End Sub

    Private Sub uxchannels_FocusedNodeChanged(sender As Object, e As DevExpress.XtraTreeList.FocusedNodeChangedEventArgs) Handles uxchannels.FocusedNodeChanged
        set_up_tab(e)
    End Sub

    Private Sub set_up_tab(e As DevExpress.XtraTreeList.FocusedNodeChangedEventArgs)

        Try
            _channel_id = CInt(e.Node.Tag)
        Catch
            _channel_id = _doc_distribution.channels(0).channel_id
        End Try


        Me.uxtab.TabPages(1).PageEnabled = False
        Me.uxtab.TabPages(2).PageEnabled = False
        Me.uxtab.TabPages(2).Text = "Mail List"
        For i = 0 To _doc_distribution.channels.Count - 1
            If _doc_distribution.channels(i).channel_id = _channel_id AndAlso _doc_distribution.channels(i).targetted = True Then
                Me.uxtab.TabPages(1).PageEnabled = True
                Me.uxtab.TabPages(2).PageEnabled = True
                Me.uxtab.SelectedTabPageIndex = 0
                If _doc_distribution.channels(i).channel_id = _channel_id AndAlso _doc_distribution.channels(i).list_generated = True Then

                    Me.uxgenmaillist.Text = "Regenerate"
                    If _doc_distribution.status = bc_om_distribution.reach_status_codes.Preparing Then
                        Me.uxgenmaillist.Visible = True

                        Me.bmerge.Visible = True
                    End If
                End If

                If _doc_distribution.channels(i).preview_date.Year > 1900 Then
                    Select Case _doc_distribution.channels(i).preview_status
                        Case 1
                            Me.lpreview.Text = "Preview pending generation: " + Format(_doc_distribution.channels(i).preview_date.ToLocalTime, "dd MMM yyyy HH:mm:ss")
                        Case 2
                            Me.lpreview.Text = "Preview being generated: " + Format(_doc_distribution.channels(i).preview_date.ToLocalTime, "dd MMM yyyy HH:mm:ss")
                        Case 3
                            Me.lpreview.Text = "Preview Generated: " + Format(_doc_distribution.channels(i).preview_date.ToLocalTime, "dd MMM yyyy HH:mm:ss")
                        Case 4
                            Me.lpreview.Text = "Preview Failed: " + Format(_doc_distribution.channels(i).preview_date.ToLocalTime, "dd MMM yyyy HH:mm:ss")
                    End Select
                Else
                    Me.lpreview.Text = "Preview not generated"
                End If

                Exit For

            ElseIf _doc_distribution.channels(i).channel_id = _channel_id AndAlso _doc_distribution.channels(i).generate_list = True Then
                Me.uxtab.TabPages(1).PageEnabled = True
                Me.uxtab.TabPages(2).PageEnabled = True
                Me.uxtab.SelectedTabPageIndex = 0
                Me.uxtab.TabPages(2).Text = "Mail List"
                If _doc_distribution.channels(i).channel_id = _channel_id AndAlso _doc_distribution.channels(i).list_generated = True Then
                    Me.uxgenmaillist.Text = "Regenerate"
                    Me.uxtab.TabPages(2).Text = "Mail List (" + CStr(uxrecipient.Nodes.Count) + " recipients)"
                    If _doc_distribution.status = bc_om_distribution.reach_status_codes.Preparing Then
                        Me.uxgenmaillist.Visible = True

                        Me.bmerge.Visible = True
                    End If
                End If

                Me.uxrecipient.Columns(3).Visible = False
                Me.uxrecipient.Columns(6).Visible = False
                Me.uxrecipient.Columns(7).Visible = False
                Me.uxrecipient.Columns(8).Visible = False



                'If _doc_distribution.channels(i).preview_date.Year > 1900 Then
                '    Select Case _doc_distribution.channels(i).preview_status
                '        Case 1
                '            Me.lpreview.Text = "Preview pending generation: " + Format(_doc_distribution.channels(i).preview_date.ToLocalTime, "dd MMM yyyy HH:mm:ss")
                '        Case 2
                '            Me.lpreview.Text = "Preview being generated: " + Format(_doc_distribution.channels(i).preview_date.ToLocalTime, "dd MMM yyyy HH:mm:ss")
                '        Case 3
                '            Me.lpreview.Text = "Preview Generated: " + Format(_doc_distribution.channels(i).preview_date.ToLocalTime, "dd MMM yyyy HH:mm:ss")
                '        Case 4
                '            Me.lpreview.Text = "Preview Failed: " + Format(_doc_distribution.channels(i).preview_date.ToLocalTime, "dd MMM yyyy HH:mm:ss")
                '    End Select
                'Else
                '    Me.lpreview.Text = "Preview not generated"
                'End If

                Exit For
            End If
        Next
    End Sub
    Public Event distribute(immediate As Boolean) Implements Ibc_am_distribution.distribute
    Public Event generate_mail_List(channel_id As Integer) Implements Ibc_am_distribution.generate_mail_List
    Public Event merge_mail_List() Implements Ibc_am_distribution.merge_mail_List
    Public Event generate_preview() Implements Ibc_am_distribution.generate_preview
    Public Event refresh_status() Implements Ibc_am_distribution.refresh_status
    Public Event load_recipients(channel_Id As Integer) Implements Ibc_am_distribution.load_recipients
    'Public Event load_history() Implements Ibc_am_distribution.load_history


    Private Sub bc_am_distribution_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If Not IsNothing(_pas) Then
            _pas.Abort()
        End If
    End Sub

    Private Sub uxdistribute_Click(sender As Object, e As EventArgs) Handles uxdistribute.Click

        Dim omsg As New bc_cs_message("Blue Curve", "Are you sure you wish to " + Me.uxdistribute.Text + " all channels.", bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
        If omsg.cancel_selected = False Then
            If Me.uxdistribute.Text = "Distribute" Then
                RaiseEvent distribute(False)
            Else
                RaiseEvent distribute(True)
            End If
        End If

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        RaiseEvent refresh_status()
    End Sub

    Private Sub uxrefreshmaillist_Click(sender As Object, e As EventArgs)
        Me.Cursor = Cursors.WaitCursor
        RaiseEvent load_recipients(_channel_id)
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub uxtab_Resize(sender As Object, e As EventArgs) Handles uxtab.Resize

    End Sub

    Private Sub XtraTabControl1_SelectedPageChanged(sender As Object, e As DevExpress.XtraTab.TabPageChangedEventArgs) Handles uxtab.SelectedPageChanged




        Me.Cursor = Cursors.WaitCursor
        Select Case e.Page.Name
            Case "History"
                'RaiseEvent refresh_status()
            Case "MailList"
                RaiseEvent load_recipients(_channel_id)
                For i = 0 To _doc_distribution.channels.Count - 1
                    If _doc_distribution.channels(i).channel_id = _channel_id Then
                        If _doc_distribution.channels(i).list_generated = False Then
                            'Me.badd.Visible = False
                            Me.uxgenmaillist.Text = "Generate"
                        End If
                        Exit For
                    End If

                Next
            Case "Preview"
                Me.hpreview.Visible = False
                'Me.uxpreview.Visible = True
                Me.pdate.Text = ""
                'Me.uxpreview.Text = "Generate"

                Me.htmlbody.Visible = False
                Me.hpreview.Visible = False

                For i = 0 To _doc_distribution.channels.Count - 1
                    If _doc_distribution.channels(i).channel_id = Me.uxchannels.Selection(0).Tag Then

                        If _doc_distribution.channels(i).email_body = "" Then
                            Me.hpreview.Visible = False
                            Me.htmlbody.Visible = False
                            Me.lpreview.Text = "Preview Not Generated"
                        Else
                            Me.lpreview.Text = ""
                            Me.hpreview.Visible = True
                            Me.hpreview.Properties.Caption = "Click to View Body of Email in a Browser"
                            Me.htmlbody.HtmlText = _doc_distribution.channels(i).email_body
                            Me.htmlbody.Visible = True
                        End If
                        Exit For
                    End If
                Next

        End Select
        Me.Cursor = Cursors.Default

    End Sub

    Private Sub uxgenmaillist_Click(sender As Object, e As EventArgs) Handles uxgenmaillist.Click


        If Me.uxgenmaillist.Text = "Regenerate" Then
            Dim omsg As New bc_cs_message("Blue Curve", "Are you sure you wish to regenerate the mail list? Any manual ammendments will be removed.", bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
            If omsg.cancel_selected = True Then
                Exit Sub

            End If
        End If
        Dim channel_id As Integer
        channel_id = Me.uxchannels.Selection.Item(0).Tag

        Me.uxrecipient.Nodes.Clear()
        Me.lstatus.Text = "Mail List Generating"
        Me.lsdate.Text = Format(Now, "dd MMM yyyy HH:mm:ss")

        Me.Refresh()
        Me.Cursor = Cursors.WaitCursor
        RaiseEvent generate_mail_List(_channel_id)
        _channel_id = channel_id
        For i = 0 To Me.uxchannels.Nodes.Count - 1
            If Me.uxchannels.Nodes(i).Tag = CStr(channel_id) Then
                Me.uxchannels.Nodes(i).Selected = True
                Me.uxtab.TabPages(1).PageEnabled = True
                Me.uxtab.TabPages(2).PageEnabled = True
                Me.uxtab.SelectedTabPageIndex = 2
                set_up_tab(Nothing)
                Me.uxtab.SelectedTabPageIndex = 2
                Exit For
            End If
        Next

        Me.Cursor = Cursors.Default
    End Sub

    Private Sub uxlist_Click(sender As Object, e As EventArgs)
        RaiseEvent generate_preview()
    End Sub

    Private Sub HyperLinkEdit1_OpenLink(sender As Object, e As DevExpress.XtraEditors.Controls.OpenLinkEventArgs) Handles hpreview.OpenLink
        For i = 0 To _doc_distribution.channels.Count - 1
            If _doc_distribution.channels(i).channel_id = Me.uxchannels.Selection(0).Tag Then
                System.Diagnostics.Process.Start(_doc_distribution.channels(i).email_url)
            End If
        Next


    End Sub

    Private Sub bAdd_Click(sender As Object, e As EventArgs) Handles badd.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            RaiseEvent add_recipient(_channel_id)

        Catch

        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    Private Sub bRemove_Click(sender As Object, e As EventArgs) Handles bRemove.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            RaiseEvent rem_recipient(_channel_id, Me.uxrecipient.Selection.Item(0).Tag)
        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    Private Sub bresend_Click(sender As Object, e As EventArgs)

        Dim omsg As New bc_cs_message("Blue Curve", "Are you sure you wish to resend email to " + Me.uxrecipient.Selection(0).GetValue(0), bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
        If omsg.cancel_selected = False Then
            RaiseEvent resend_recipient(Me.uxrecipient.Selection.Item(0).Tag)
        End If

    End Sub



    Private Sub uxrecipient_FocusedNodeChanged(sender As Object, e As DevExpress.XtraTreeList.FocusedNodeChangedEventArgs) Handles uxrecipient.FocusedNodeChanged
        Me.bRemove.Visible = False
        'Me.bresend.Visible = False
        If Me.uxrecipient.Selection.Count = 1 Then
            If _doc_distribution.status = bc_om_distribution.reach_status_codes.Ready_To_Distribute_Manual Or _doc_distribution.status = bc_om_distribution.reach_status_codes.Preparing Or _doc_distribution.status = bc_om_distribution.reach_status_codes.Mail_List_Complete Then
                Me.bRemove.Visible = True

                'ElseIf _doc_distribution.status = bc_om_distribution.reach_status_codes.Distributed Then
                '    Me.bresend.Visible = True
            End If
        End If
    End Sub

    Private Sub bhistory_Click(sender As Object, e As EventArgs)
        RaiseEvent refresh_status()
    End Sub

    Private Sub XtraTabPage3_Paint(sender As Object, e As PaintEventArgs) Handles Preview.Paint

    End Sub

    Private Sub bdistcancel_Click(sender As Object, e As EventArgs) Handles bdistcancel.Click
        RaiseEvent cancel_distribution()
    End Sub




    Private Sub Uxpreview_Click(sender As Object, e As EventArgs) Handles Uxpreview.Click
        RaiseEvent generate_preview()
    End Sub

    Private Sub bresendchannel_Click(sender As Object, e As EventArgs) Handles bresendchannel.Click
        Dim omsg As New bc_cs_message("Blue Curve", "Do you wish to resend channel: " + Me.uxchannels.Selection.Item(0).GetValue(0), bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
        If omsg.cancel_selected = False Then
            RaiseEvent resend_channel(Me.uxchannels.Selection.Item(0).Tag)

        End If
    End Sub

    Private Sub bc_am_distribution_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub uxtab_Click(sender As Object, e As EventArgs) Handles uxtab.Click

    End Sub

    'Private Sub cmerge_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmerge.SelectedIndexChanged
    '    Try
    '        If Me.cmerge.SelectedIndex = -1 Then
    '            Exit Sub
    '        End If
    '        Dim omsg As New bc_cs_message("Blue Curve", "Are you sure you wish to regenerate and merge the mail list from last " + Me.cmerge.Text + " into the current list", bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
    '        If omsg.cancel_selected = True Then
    '            Exit Sub
    '        End If

    '        Dim channel_id As Integer
    '        channel_id = Me.uxchannels.Selection.Item(0).Tag

    '        RaiseEvent merge_mail_List()

    '        For i = 0 To Me.uxchannels.Nodes.Count - 1
    '            If Me.uxchannels.Nodes(i).Tag = CStr(channel_id) Then
    '                Me.uxchannels.Nodes(i).Selected = True
    '                Me.uxtab.TabPages(1).PageEnabled = True
    '                Me.uxtab.TabPages(2).PageEnabled = True
    '                Me.uxtab.SelectedTabPageIndex = 2
    '                set_up_tab(Nothing)
    '                Me.uxtab.SelectedTabPageIndex = 2
    '                Exit For
    '            End If
    '        Next
    '    Catch

    '    Finally
    '        Me.cmerge.SelectedIndex = -1
    '        Me.Cursor = Cursors.Default
    '    End Try
    'End Sub

    Private Sub bmerge_Click(sender As Object, e As EventArgs) Handles bmerge.Click
        RaiseEvent merge_mail_List()
        Dim channel_id As Integer
        channel_id = Me.uxchannels.Selection.Item(0).Tag
        For i = 0 To Me.uxchannels.Nodes.Count - 1
            If Me.uxchannels.Nodes(i).Tag = CStr(channel_id) Then
                Me.uxchannels.Nodes(i).Selected = True
                Me.uxtab.TabPages(1).PageEnabled = True
                Me.uxtab.TabPages(2).PageEnabled = True
                Me.uxtab.SelectedTabPageIndex = 2
                set_up_tab(Nothing)
                Me.uxtab.SelectedTabPageIndex = 2
                Exit For
            End If
        Next
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        export_to_excel()
    End Sub
    Sub export_to_excel()
        Dim oxlApp As Object 'Excel.Application4.   
        Cursor = Cursors.WaitCursor


        Try

            If Me.uxrecipient.Nodes.Count = 0 Then
                Exit Sub
            End If


            Dim oxlOutWBook As Object 'Excel.Workbook5.     

            oxlApp = bc_ao_in_excel.CreateNewExcelInstance
            oxlOutWBook = oxlApp.Workbooks.Add
            Dim oexcel As New bc_ao_in_excel(oxlApp)


            Dim c As Integer


            Dim values(uxrecipient.Nodes.Count, uxrecipient.Columns.Count - 1) As String

            For i = 0 To uxrecipient.Nodes.Count - 1
                c = 1
                For j = 0 To uxrecipient.Columns.Count - 1
                    If i = 0 Then

                    End If

                    If uxrecipient.Columns(j).Visible = True Then
                        If i = 0 Then
                            values(i, c) = uxrecipient.Columns(j).Caption

                        End If
                        values(i + 1, c) = uxrecipient.Nodes(i).GetValue(j)
                        c = c + 1
                    End If
                Next

            Next

            oexcel.bc_array_excel_export(values, True, "Results")



            oxlApp.application.visible = True

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_distribution", "export_to_excel", bc_cs_error_codes.USER_DEFINED, "Failed to export to excel: " + ex.Message)
        Finally
            Try
                oxlApp.application.visible = True
            Catch

            End Try
           
            Cursor = Cursors.Default

            End Try
    End Sub
End Class
Public Class Cbc_am_distribution
    WithEvents _view As Ibc_am_distribution
    Dim _doc_distribution As New bc_om_distribution
    Dim f As Integer = 0
    Public Function load_data(view As Ibc_am_distribution, doc_id As Long, title As String) As Boolean
        load_data = False
        Try
            _view = view
            _doc_distribution.doc_id = doc_id
            load_distribution_data()
            If _doc_distribution.channels.Count = 0 Then
                Dim omsg As New bc_cs_message("Blue Curve", "No distribution channels configured for publication type.", False, False, False, "Yes", "No", True)
                Exit Function
            End If


            Return _view.load_view(_doc_distribution, title)
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Cbc_am_distribution", "load_data", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Function
    Private Function load_distribution_data()
        load_distribution_data = False
        Try

            _doc_distribution.operation = bc_om_distribution.DIST_OPERATION.LOAD_DATA
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                _doc_distribution.db_read()
            Else
                _doc_distribution.tmode = bc_cs_soap_base_class.tREAD
                If _doc_distribution.transmit_to_server_and_receive(_doc_distribution, True) = False Then
                    Exit Function
                End If
            End If

            load_distribution_data = True
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Cbc_am_distribution", "load_distribution_data", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Function
    Public Sub distribute(immediate As Boolean) Handles _view.distribute
        Try
            _doc_distribution.resend_channel_id = 0
            If immediate = False Then
                _doc_distribution.operation = bc_om_distribution.DIST_OPERATION.DISTRIBUTE
            Else
                _doc_distribution.operation = bc_om_distribution.DIST_OPERATION.DISTRIBUTE_NOW
            End If

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                _doc_distribution.db_read()
            Else
                _doc_distribution.tmode = bc_cs_soap_base_class.tREAD
                If _doc_distribution.transmit_to_server_and_receive(_doc_distribution, True) = False Then
                    Exit Sub
                End If
            End If
            refresh_view()

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Cbc_am_distribution", "distribute", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub

    Public Sub resend_chanel(channel_id As Integer) Handles _view.resend_channel
        Try

            _doc_distribution.operation = bc_om_distribution.DIST_OPERATION.DISTRIBUTE_NOW
            _doc_distribution.resend_channel_id = channel_id
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                _doc_distribution.db_read()
            Else
                _doc_distribution.tmode = bc_cs_soap_base_class.tREAD
                If _doc_distribution.transmit_to_server_and_receive(_doc_distribution, True) = False Then
                    Exit Sub
                End If
            End If

            refresh_view()

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Cbc_am_distribution", "resend_channel", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub

    Public Sub cancel_distribution() Handles _view.cancel_distribution

        Try
            _doc_distribution.operation = bc_om_distribution.DIST_OPERATION.CANCEL_DISTRIBUTION
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                _doc_distribution.db_read()
            Else
                _doc_distribution.tmode = bc_cs_soap_base_class.tREAD
                If _doc_distribution.transmit_to_server_and_receive(_doc_distribution, True) = False Then
                    Exit Sub
                End If
            End If
            refresh_view()

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Cbc_am_distribution", "distribute", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Private Sub resend_recipient(client_id As Long) Handles _view.resend_recipient
        Try
            Dim oclient As New bc_om_distribution.bc_om_client_contact
            oclient.client_id = client_id
            oclient.doc_Id = _doc_distribution.doc_id
            oclient.modify_mode = bc_om_distribution.bc_om_client_contact.CLIENT_CONTACT_MODIFY_MODE.RESEND
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                oclient.db_write()
            Else
                oclient.tmode = bc_cs_soap_base_class.tWRITE
                If oclient.transmit_to_server_and_receive(oclient, True) = False Then
                    Exit Sub
                End If
            End If
            refresh_view()
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Cbc_am_distribution", "resend_recipient", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Private Function refresh_view() Handles _view.refresh_status
        refresh_view = False
        Try

            load_distribution_data()
            _view.set_view(_doc_distribution)
            refresh_view = True
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Cbc_am_distribution", "refresh_view", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Function
    Private Function generate_mail_list(channel_id As Integer) Handles _view.generate_mail_List
        _doc_distribution.operation = bc_om_distribution.DIST_OPERATION.DISTRIBUTION_LIST

        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
            _doc_distribution.db_read()
        Else
            _doc_distribution.tmode = bc_cs_soap_base_class.tREAD
            If _doc_distribution.transmit_to_server_and_receive(_doc_distribution, True) = False Then
                Exit Function
            End If
        End If

        refresh_view()

    End Function
    Private Function merge_mail_list() As Boolean Handles _view.merge_mail_List


        merge_mail_list = False
        Dim fmml As New bc_dx_mail_list_generate
        Dim Cmml As New Cbc_dx_mail_list_generate
        If Cmml.load_data(fmml, _doc_distribution) = True Then

            fmml.ShowDialog()
        End If

        If Cmml.cancel = False Then
            refresh_view()
            merge_mail_list = True
        End If


    End Function

    Private Function rem_recipients(channel_id As Integer, recipient_id As String) Handles _view.rem_recipient
        Dim rec As New bc_om_distribution.bc_om_client_contact
        rec.client_id = recipient_id
        rec.doc_Id = _doc_distribution.doc_id
        rec.modify_mode = bc_om_distribution.bc_om_client_contact.CLIENT_CONTACT_MODIFY_MODE.REMOVE
        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
            rec.db_write()
        Else
            rec.tmode = bc_cs_soap_base_class.tWRITE
            If rec.transmit_to_server_and_receive(rec, True) = False Then
                Exit Function
            End If
        End If
        load_recipients(channel_id)
    End Function
    Private Function add_recipients(channel_id As Long) Handles _view.add_recipient
        Dim fcc As New bc_am_client_contacts
        Dim cc As New Cbc_am_client_contacts(_doc_distribution.doc_id, fcc)
        If cc.load_data = True Then
            fcc.ShowDialog()
            If cc.bsave = True Then
                load_recipients(channel_id)
            End If
        End If

    End Function
    Private Function load_recipients(channel_id As Integer) Handles _view.load_recipients

        Try
            _doc_distribution.operation = bc_om_distribution.DIST_OPERATION.LOAD_RECIPIENTS
            _doc_distribution.channel_id = channel_id

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                _doc_distribution.db_read()
            Else
                _doc_distribution.tmode = bc_cs_soap_base_class.tREAD
                If _doc_distribution.transmit_to_server_and_receive(_doc_distribution, True) = False Then
                    Exit Function
                End If
            End If
            _view.set_recipients(_doc_distribution, channel_id)
            load_recipients = True
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Cbc_am_distribution", "load_recipients", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Function

    Private Function generate_preview() Handles _view.generate_preview

        Try
            _doc_distribution.operation = bc_om_distribution.DIST_OPERATION.EMAIL_PREVIEW

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                _doc_distribution.db_read()
            Else
                _doc_distribution.tmode = bc_cs_soap_base_class.tREAD
                If _doc_distribution.transmit_to_server_and_receive(_doc_distribution, True) = False Then
                    Exit Function
                End If
            End If
            _view.set_view(_doc_distribution)
            generate_preview = True
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Cbc_am_distribution", "load_recipients", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Function

End Class
Public Interface Ibc_am_distribution
    Function load_view(doc_distribution As bc_om_distribution, title As String) As Boolean
    Function set_view(doc_distribution As bc_om_distribution) As Boolean
    Function set_recipients(doc_distribution As bc_om_distribution, channel_id As Integer) As Boolean
    Event generate_preview()
    Event generate_mail_List(channel_id As Integer)
    Event merge_mail_List()

    Event distribute(immediate As Boolean)
    Event refresh_status()
    Event load_recipients(channel_id As Integer)
    'Event load_history()
    Event add_recipient(channel_id As Integer)
    Event rem_recipient(channel_id As Integer, recipient_id As String)
    Event resend_recipient(client_id As Long)
    Event resend_channel(channel_id As Long)
    Event cancel_distribution()
End Interface
