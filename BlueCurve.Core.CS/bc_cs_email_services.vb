Imports System.Net.Mail
Imports System.Collections
Imports System.ComponentModel
Imports System.Threading
Imports System.IO
Imports System.ServiceModel
'Imports System.ServiceModel.Channel
Imports System.Net.NetworkInformation

Public Class bc_cs_reach_distribute


    Public Sub New(lcerticiate As bc_cs_security.certificate)
        Dim certificate As New bc_cs_security.certificate
        Try
            REM cache email templates

            certificate = lcerticiate
            Dim ct As New bc_cs_email_templates_cache
            If ct.load_templates(lcerticiate) = False Then
                Dim err As New bc_cs_error_log("bc_cs_reach_distribute", "new", bc_cs_error_codes.USER_DEFINED, "Failed to cache all templates", certificate)
            End If

        Catch ex As Exception
            Dim err As New bc_cs_error_log("bc_cs_reach_distribute", "new", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            certificate.server_errors.Clear()
        End Try
    End Sub

    Public Sub service_poll()
        Dim certificate As New bc_cs_security.certificate
        certificate.user_id = "BlueCurveDistributionServices"
        Try

            Dim db As New bc_cs_db_services
            Dim sql As String
            Dim eres As Object

            sql = "exec dbo.bc_core_get_queued_reach_docs"
            eres = db.executesql(sql, certificate)

            Dim pas As Thread
            If IsArray(eres) Then
                For i = 0 To UBound(eres, 2)

                    Dim j As Integer
                    j = i

                    pas = New Thread(Sub() Me.dothetask(eres(0, j)))
                    pas.Start()
                Next
            End If
        Catch ex As Exception
            Dim err As New bc_cs_error_log("bc_cs_reach_distribute", "service_poll", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        End Try
    End Sub

    Sub dothetask(doc_id As Long)
        Dim certificate As New bc_cs_security.certificate
        certificate.user_id = "BlueCurveDistributionServices"
        Dim db As New bc_cs_db_services
        Dim err_msg As String = ""
        Dim success As Boolean = False
        Dim sql As String
        Dim overall_success As Boolean = True
        Dim overall_err_msg As String = ""

        Try
            Dim vres As Object
            Dim entity_id As Long = 0
            Dim header_email_template_Id As Long = 0
            Dim body_email_template_Id As Long = 0
            Dim channel_id As Integer = 0
            Dim email_header As String = ""
            Dim email_body As String = ""

            Dim targeted As Boolean = False
            Dim generate_email As Boolean = False
            Dim mail_send As Boolean = False
            Dim save_email As Boolean = False
            Dim generate_mail_list As Boolean = False

            sql = "exec dbo.bc_core_get_distribution_channels_for_doc " + CStr(doc_id)
            vres = db.executesql(sql, certificate)

            If IsArray(vres) Then
                For i = 0 To UBound(vres, 2)
                    success = True
                    err_msg = ""
                    channel_id = vres(0, i)
                    entity_id = vres(1, i)
                    header_email_template_Id = vres(2, i)
                    body_email_template_Id = vres(3, i)
                    targeted = vres(4, 0)
                    generate_email = vres(5, i)
                    mail_send = vres(6, i)
                    save_email = vres(7, i)
                    generate_mail_list = vres(8, i)


                    'If generate_email = True Then 
                    '    REM email generation
                    '    err_msg = generate_email_header_and_body(body_email_template_Id, header_email_template_Id, doc_id, entity_id, channel_id, save_email, email_header, email_body)
                    '    certificate.server_errors.Clear()
                    '    If err_msg <> "" Then
                    '        overall_success = False
                    '        success = False
                    '    End If
                    '    REM distribution list generation
                    '    REM send the email
                    '    If mail_send = True And success = True Then
                    '        Dim fs As New bc_cs_string_services(email_header)
                    '        email_header = fs.delimit_apostrophies
                    '        fs = New bc_cs_string_services(email_body)
                    '        email_body = fs.delimit_apostrophies
                    '        sql = "exec dbo.bc_core_send_email_from_reach " + CStr(doc_id) + ",'" + email_header + "','" + email_body + "'" + "," + CStr(generate_email)
                    '        certificate.server_errors.Clear()
                    '        db.executesql(sql, certificate)
                    '        If certificate.server_errors.Count > 0 Then
                    '            success = False
                    '            err_msg = certificate.server_errors(0)
                    '        End If
                    '        certificate.server_errors.Clear()
                    '    End If
                    'ElseIf targeted = False Then
                    If targeted = False Then
                        REM Net Send

                        'db.executesql_extended_timeout(sql, certificate)
                        If generate_mail_list = True Then
                            REM TBD sp call to do this an donly if mail list not already done
                            sql = "exec dbo.bc_core_re_generate_recipients_for_doc " + CStr(doc_id) + ",0,1,1"
                            db.executesql_extended_timeout(sql, certificate)
                        End If
                        REM audit last distribution list
                        sql = "exec dbo.bc_core_re_audit_dist_list " + CStr(doc_id) + "," + CStr(channel_id)

                        Dim ons As New bc_cs_net_send_channel(doc_id, channel_id)
                        err_msg = ons.send_net_send_channel(certificate)

                        If err_msg <> "" Then
                            success = False
                        End If
                    End If
                    If success = True Then
                        sql = "exec dbo.bc_core_set_channel_distribution_status " + CStr(doc_id) + "," + CStr(channel_id) + ",7,''"
                    Else
                        overall_success = False
                        Dim fs As New bc_cs_string_services(err_msg)
                        sql = "exec dbo.bc_core_set_channel_distribution_status " + CStr(doc_id) + "," + CStr(channel_id) + ",8,'" + fs.delimit_apostrophies + "'"
                    End If
                    db.executesql(sql, certificate)
                Next
            End If

        Catch ex As Exception
            Dim err As New bc_cs_error_log("bc_cs_reach_distribute", "dothetask", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            overall_success = False
            overall_err_msg = ex.Message


        Finally
            If overall_success = True Then
                sql = "exec dbo.bc_core_distribution_complete " + CStr(doc_id) + ",0,''"
            Else
                sql = "exec dbo.bc_core_distribution_complete " + CStr(doc_id) + ",1,'Overall Distribution failed check individual channels'"
            End If
            db.executesql(sql, certificate)
        End Try
    End Sub
    

    'Private Function generate_email_header_and_body(body_email_template_Id As Integer, header_email_template_Id As Integer, doc_id As Long, entity_id As Long, channel_id As Integer, save_email As Boolean, ByRef email_header As String, ByRef email_body As String) As String
    '    Try
    '        Dim etc As bc_cs_email_template
    '        Dim fn As String
    '        If body_email_template_Id <> 0 And header_email_template_Id <> 0 Then
    '            REM html email generation
    '            etc = New bc_cs_email_template(body_email_template_Id, doc_id, entity_id, 0, 0, 0, 0, certificate, Nothing)
    '            If etc.execute_template() = True Then
    '                If save_email = True Then
    '                    fn = bc_cs_central_settings.central_repos_path + "\email_previews\" + CStr(doc_id) + ".html"
    '                    Try
    '                        Dim fs As New StreamWriter(fn, False)
    '                        fs.WriteLine(etc.content)
    '                        fs.Close()
    '                    Catch ex As Exception
    '                        generate_email_header_and_body = "Channel " + CStr(channel_id) + "Email Template Couldnt write file: " + fn + ": "
    '                        Exit Function
    '                    End Try
    '                End If
    '                email_body = etc.content
    '                etc = New bc_cs_email_template(header_email_template_Id, doc_id, entity_id, 0, 0, 0, 0, certificate, Nothing)
    '                If etc.execute_template() = True Then
    '                    email_header = etc.content
    '                Else
    '                    generate_email_header_and_body = "Channel " + CStr(channel_id) + ": Failed to generate email template for header: " + etc.err_text
    '                    Exit Function
    '                End If
    '            Else
    '                generate_email_header_and_body = "Channel " + CStr(channel_id) + ": Failed to generate email template for body: " + etc.err_text
    '                Exit Function
    '            End If
    '        Else
    '            generate_email_header_and_body = "Channel " + CStr(channel_id) + ": Failed to generate email template no body or header template defined"
    '            Exit Function
    '        End If

    '        REM now send emails


    '    Catch ex As Exception
    '        Dim err As New bc_cs_error_log("bc_cs_reach_distribute", "dothetask", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
    '        generate_email_header_and_body = ex.Message

    '    End Try
    'End Function
End Class
Public Class bc_cs_email_preview_services
    Dim certiciate As New bc_cs_security.certificate
    Public Sub New(ByRef lcerticiate As bc_cs_security.certificate, Optional cache As Boolean = True)
        Try
            If cache = False Then
                Exit Sub
            End If
            REM cache email templates
            Me.certiciate = lcerticiate
            Dim ct As New bc_cs_email_templates_cache
            If ct.load_templates(lcerticiate) = False Then
                Dim err As New bc_cs_error_log("bc_cs_email_preview_services", "new", bc_cs_error_codes.USER_DEFINED, "Failed to cache all templates", certiciate)
            End If

        Catch ex As Exception
            Dim err As New bc_cs_error_log("bc_cs_email_preview_services", "new", bc_cs_error_codes.USER_DEFINED, ex.Message, certiciate)
        Finally
            certiciate.server_errors.Clear()
            certiciate.error_state = False
        End Try
    End Sub

    Public Sub service_poll()
        Try
            Dim db As New bc_cs_db_services
            Dim sql As String
            Dim eres As Object

            sql = "exec dbo.bc_core_get_queued_email_previews"
            eres = db.executesql(sql, certiciate)

            Dim pas As Thread
            If IsArray(eres) Then
                For i = 0 To UBound(eres, 2)
                    Dim j As Integer
                    j = i
                    pas = New Thread(Sub() Me.dothetask(eres(0, j), eres(1, j), eres(2, j), eres(3, j), eres(4, j)))
                    pas.Start()
                Next
            End If
        Catch ex As Exception
            Dim err As New bc_cs_error_log("bc_cs_email_preview_services", "service_poll", bc_cs_error_codes.USER_DEFINED, ex.Message, certiciate)
        End Try
    End Sub

    Sub dothetask(email_preview_id As Long, header_template_id As Long, body_template_Id As Long, doc_Id As Long, entity_Id As Long)
        Dim db As New bc_cs_db_services
        Try
            REM see if body and or title is autogenerated
            Dim etc As bc_cs_email_template
            Dim elog As New bc_cs_email_preview_log
            Dim header As String
            etc = New bc_cs_email_template(header_template_id, doc_Id, entity_Id, 0, 0, 0, 0, certiciate, Nothing)
            If etc.execute_template() = False Then
                elog = New bc_cs_email_preview_log
                elog.log_error(email_preview_id, body_template_Id, entity_Id, doc_Id, "Email Template Failed to Generate" + etc.err_text, certiciate)
                Exit Sub
            Else
                header = etc.content
            End If
            etc = New bc_cs_email_template(body_template_Id, doc_Id, entity_Id, 0, 0, 0, 0, certiciate, Nothing)
            If etc.execute_template() = False Then
                elog = New bc_cs_email_preview_log
                elog.log_error(email_preview_id, body_template_Id, entity_Id, doc_Id, "Email Template Failed to Generate" + etc.err_text, certiciate)
                Exit Sub
            Else
                REM save to file system
                Dim fn As String
                fn = bc_cs_central_settings.central_repos_path + "\email_previews\" + CStr(doc_Id) + ".html"
                Try

                    Dim fs As New StreamWriter(fn, False, System.Text.Encoding.UTF8)
                    fs.WriteLine(etc.content)
                    fs.Close()
                    REM store in db
                    Dim ds As New bc_cs_string_services(etc.content)
                    Dim hs As New bc_cs_string_services(header)
                    header = hs.delimit_apostrophies
                    db.executesql("exec dbo.bc_core_add_email_preview_for_doc " + CStr(doc_Id) + ",'" + header + "','" + ds.delimit_apostrophies + "'", certiciate)

                Catch ex As Exception
                    elog.log_error(email_preview_id, body_template_Id, entity_Id, doc_Id, "Email Template Couldnt write file: " + fn + ": " + ex.Message, certiciate)
                    Exit Sub
                End Try
                elog.log_success(email_preview_id, body_template_Id, entity_Id, doc_Id, certiciate)
            End If

        Catch ex As Exception
            Dim err As New bc_cs_error_log("bc_cs_email_preview_services", "dothetask", bc_cs_error_codes.USER_DEFINED, ex.Message, certiciate)
        Finally

        End Try
    End Sub
    REM when called not from the service
    Public err_txt As String
    Public Function create_preview(email_preview_id As Long, template_id As Long, doc_Id As Long, entity_Id As Long, stage_id As Long, ByRef gdb As bc_cs_db_services) As Boolean
        Dim db As New bc_cs_db_services

        Try
            REM see if body and or title is autogenerated
            Dim etc As bc_cs_email_template
            Dim elog As New bc_cs_email_preview_log
            etc = New bc_cs_email_template(template_id, entity_Id, doc_Id, 0, stage_id, 0, 0, certiciate, gdb)
            If etc.execute_template() = False Then
                err_txt = etc.err_text
                Exit Function
            Else
                REM save to file system

                Dim fn As String
                fn = bc_cs_central_settings.central_repos_path + "\email_previews\" + CStr(doc_Id) + ".html"
                Try
                    Dim fs As New StreamWriter(fn, False, System.Text.Encoding.UTF8)
                    fs.WriteLine(etc.content)
                    fs.Close()
                Catch ex As Exception
                    err_txt = "Email Template Couldnt write file: " + fn + ": " + ex.Message
                    Exit Function
                End Try
                create_preview = True
                If bc_cs_central_settings.repos_html_db = True Then
                    Dim dr As New bc_cs_db_repos
                    If dr.write_html_preview_to_db(bc_cs_central_settings.central_repos_path + "\email_previews\" + CStr(doc_Id) + ".html", CStr(doc_Id) + ".html", doc_Id, certiciate) = False Then
                        err_txt = "Email Template Couldnt write file to db repos"
                        Exit Function
                    End If
                    Dim fnt As New bc_cs_file_transfer_services
                    fnt.delete_file(bc_cs_central_settings.central_repos_path + "\email_previews\" + CStr(doc_Id) + ".html")

                    create_preview = True

                End If
            End If
        Catch ex As Exception
            err_txt = ex.Message
        Finally

        End Try
    End Function
    Public Class bc_cs_email_preview_log
        Dim db As New bc_cs_db_services


        Public Sub log_error(email_Preview_id As Long, template_id As Long, entity_Id As Long, doc_id As Long, msg As String, ByRef certificate As bc_cs_security.certificate)
            Try
                Dim fs As New bc_cs_string_services(msg)
                msg = fs.delimit_apostrophies()
                Dim sql As String
                sql = "exec dbo.bc_core_log_email_preview_error " + CStr(email_Preview_id) + "," + CStr(template_id) + "," + CStr(entity_Id) + "," + CStr(doc_id) + ",'" + msg + "'"
                db.executesql(sql, certificate)
            Catch ex As Exception
                Dim err As New bc_cs_error_log("bc_cs_email_preview_log", "log_error", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)

            End Try
        End Sub
        Public Sub log_success(email_preview_id As Long, template_id As Long, entity_Id As Long, doc_id As Long, ByRef certificate As bc_cs_security.certificate)
            Try
                Dim sql As String
                sql = "exec dbo.bc_core_log_email_preview_success " + CStr(email_preview_id) + "," + CStr(template_id) + "," + CStr(entity_Id) + "," + CStr(doc_id)
                db.executesql(sql, certificate)
            Catch ex As Exception
                Dim err As New bc_cs_error_log("bc_cs_email_preview_log", "log_error", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)

            End Try
        End Sub

    End Class
End Class

Public Class bc_cs_email_services

    'Shared Smtp_Server As SmtpClient
    Dim certificate As New bc_cs_security.certificate
    Dim thread_count As Integer = 0
    Public Sub New()

    End Sub
    Public Sub New(lcerticiate As bc_cs_security.certificate)
        Try
            REM cache email templates
            Me.certificate = lcerticiate
            Dim ct As New bc_cs_email_templates_cache
            If ct.load_templates(lcerticiate) = False Then
                Dim err As New bc_cs_error_log("bc_cs_email_services", "new", bc_cs_error_codes.USER_DEFINED, "Failed to cache all templates", certificate)
            End If

        Catch ex As Exception
            Dim err As New bc_cs_error_log("bc_cs_email_services", "new", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            certificate.server_errors.Clear()
        End Try
    End Sub

    Public Sub service_poll()
        Try
            Dim db As New bc_cs_db_services
            Dim sql As String
            Dim eres As Object

            sql = "exec dbo.bc_core_get_queued_emails"
            eres = db.executesql(sql, certificate)

            Dim pas As Thread
            If IsArray(eres) Then
                For i = 0 To UBound(eres, 2)
                    thread_count = thread_count + 1
                    Dim ocomm As New bc_cs_activity_log("bc_cs_email_services", "service_poll", bc_cs_activity_codes.COMMENTARY, "Active Threads: " + CStr(thread_count) + " Max: " + CStr(bc_cs_central_settings.email_service_maximum_concurrent_threads), certificate)
                    REM dont allow excessive threads
                    While thread_count > bc_cs_central_settings.email_service_maximum_concurrent_threads
                        Thread.Sleep(100)
                    End While
                    Dim j As Integer
                    j = i
                    pas = New Thread(Sub() Me.dothetask(eres(0, j), eres(3, j), eres(1, j), eres(2, j), eres(4, j), eres(5, j), eres(6, j), eres(7, j), eres(8, j), eres(9, j), eres(10, j), eres(11, j)))
                    'pas = New Thread(Sub() Me.run_email_under_IIS(eres(0, j), eres(3, j), eres(1, j), eres(2, j), eres(4, j), eres(5, j), eres(6, j), eres(7, j), eres(8, j), eres(9, j), eres(10, j), eres(11, j)))
                    pas.Start()
                Next
            End If
        Catch ex As Exception
            Dim err As New bc_cs_error_log("bc_cs_email_services", "service_poll", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        End Try
    End Sub

    Public Sub run_email_under_IIS(id As Long, use_bcc As Boolean, title As String, body As String, title_template_Id As Integer, body_template_id As Integer, doc_Id As Long, entity_Id As Long, user_id As Long, stage_id As Long, digest1 As Long, digest2 As Long)
        Dim ocomm As New bc_cs_activity_log("bc_cs_email_services", "run_email_under_IIS", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)


        Try

            Dim ma As New bc_cs_max_address_services()
            Dim s As New BlueCurve.Core.CS.ServiceReference2.BCIISServicesClient
            
            s.Endpoint.Binding.SendTimeout = TimeSpan.FromMilliseconds(9999999)
            Dim ea As New EndpointAddress("http://localhost/BCIISServices/BCIISServices.svc")
            s.Endpoint.Address = ea
            Dim err As String = ""

            ocomm = New bc_cs_activity_log("bc_cs_calcs", "run_email_under_iis", bc_cs_activity_codes.COMMENTARY.ToString(), "Start invoking under IIS email: " + id.ToString(), certificate)
            Dim err_tx As String
            err_tx = s.SendEmail(id, use_bcc, title, body, title_template_Id, body_template_id, doc_Id, entity_Id, user_id, stage_id, digest1, digest2)
            If err_tx <> "" Then
                Dim terr As New bc_cs_error_log("bc_cs_email_services", "xrun_email_under_IIS", bc_cs_error_codes.USER_DEFINED, err_tx, certificate)

            End If

        Catch ex As Exception
            Dim err As New bc_cs_error_log("bc_cs_email_services", "run_email_under_IIS", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            thread_count = thread_count - 1
            ocomm = New bc_cs_activity_log("bc_cs_email_services", "run_email_under_IIS", bc_cs_activity_codes.TRACE_EXIT, "", certificate)

        End Try
    End Sub

    REM get any attachments
    Sub dothetask(id As Long, use_bcc As Boolean, title As String, body As String, title_template_Id As Integer, body_template_id As Integer, doc_Id As Long, entity_Id As Long, user_id As Long, stage_id As Long, digest1 As Long, digest2 As Long)
        Dim ocomm As New bc_cs_activity_log("bc_cs_email_services", "dothetask", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Dim db As New bc_cs_db_services
        Dim sql As String
        Dim ares As Object
        Dim rres As Object
        Dim recipients As List(Of String)
        Dim replyto As List(Of String)
        Dim preply_to As String = ""
        Dim pfrom_address As String = ""
        Dim attachments As List(Of System.Net.Mail.Attachment) = Nothing
        Dim attachment As System.Net.Mail.Attachment
        Dim attachment_failed As Boolean
        Dim sattachment As String
        Dim elog As bc_cs_email_log
        Dim pdisplay_name As String = ""


        Try
            REM see if body and or title is autogenerated
            Dim etc As bc_cs_email_template
            If title_template_Id > 0 Then
                etc = New bc_cs_email_template(title_template_Id, entity_Id, doc_Id, user_id, stage_id, digest1, digest2, certificate, Nothing)
                If etc.execute_template() = False Then
                    elog = New bc_cs_email_log
                    elog.log_error("", id, "", "Title Template Failed: " + CStr(title_template_Id) + ":" + etc.err_text, certificate)
                    Exit Sub
                Else
                    title = etc.content
                End If
            End If

            If body_template_id > 0 Then
                etc = New bc_cs_email_template(body_template_id, entity_Id, doc_Id, user_id, stage_id, digest1, digest2, certificate, Nothing)
                If etc.execute_template() = False Then
                    elog = New bc_cs_email_log
                    elog.log_error("", id, "", "Body Template Failed: " + CStr(body_template_id) + ":" + etc.err_text, certificate)
                    Exit Sub
                Else
                    body = etc.content
                End If
            End If

            attachment_failed = False
            sql = "exec dbo.bc_core_get_email_attachments " + CStr(id)
            ares = db.executesql(sql, certificate)
            If IsArray(ares) Then
                attachments = New List(Of System.Net.Mail.Attachment)
                For a = 0 To UBound(ares, 2)
                    Try
                        Dim replace_file_name As String
                        Dim content_from_sp As String
                        sattachment = ares(0, a)
                        replace_file_name = ares(1, a)
                        content_from_sp = ares(2, a)

                        sattachment = Replace(sattachment, "<repos>", bc_cs_central_settings.central_repos_path)
                        Dim fs As bc_cs_file_transfer_services


                        If content_from_sp <> "" Then
                            REM execute SP to get data
                            sql = "exec " + content_from_sp + " " + CStr(doc_Id)
                            rres = db.executesql(sql, certificate)
                            If IsArray(rres) Then
                                If UBound(rres, 2) = 0 Then
                                    fs = New bc_cs_file_transfer_services
                                    Dim sw As New StreamWriter(bc_cs_central_settings.central_repos_path + "\email_attachments\" + CStr(id) + replace_file_name)
                                    sw.WriteLine(rres(0, 0))
                                    sw.Close()
                                    sw.Dispose()
                                    fs.file_copy(bc_cs_central_settings.central_repos_path + "\email_attachments\" + CStr(id) + replace_file_name, bc_cs_central_settings.central_repos_path + "\email_attachments\" + replace_file_name)

                                    attachment = New System.Net.Mail.Attachment(bc_cs_central_settings.central_repos_path + "\email_attachments\" + replace_file_name)
                                    fs.delete_file(bc_cs_central_settings.central_repos_path + "\email_attachments\" + CStr(id) + replace_file_name)
                                    fs.delete_file(bc_cs_central_settings.central_repos_path + "\email_attachments\" + replace_file_name)

                                Else
                                    elog = New bc_cs_email_log
                                    elog.log_error("", id, "", "Attachment failed no results from SP: " + content_from_sp, certificate)
                                    attachment_failed = True
                                    Exit Sub
                                End If
                            Else
                                elog = New bc_cs_email_log
                                elog.log_error("", id, "", "Attachment failed no results from SP: " + content_from_sp, certificate)
                                attachment_failed = True
                                Exit Sub
                            End If

                        ElseIf replace_file_name <> "" Then
                            fs = New bc_cs_file_transfer_services
                            fs.file_copy(sattachment, bc_cs_central_settings.central_repos_path + "\email_attachments\" + replace_file_name)
                            attachment = New System.Net.Mail.Attachment(bc_cs_central_settings.central_repos_path + "\email_attachments\" + replace_file_name)
                            fs.delete_file(bc_cs_central_settings.central_repos_path + "\email_attachments\" + replace_file_name)
                        Else
                            attachment = New System.Net.Mail.Attachment(sattachment)
                        End If

                        attachments.Add(attachment)
                        attachment = Nothing

                    Catch ex As Exception
                        elog = New bc_cs_email_log
                        elog.log_error("", id, "", "Attachment failed " + ex.Message, certificate)
                        attachment_failed = True
                        Exit Sub
                    End Try
                Next
            End If

            recipients = New List(Of String)
            replyto = New List(Of String)
            REM get recipients for each email
            sql = "exec dbo.bc_core_get_email_recipients " + CStr(id)
            rres = db.executesql(sql, certificate)
            If IsArray(rres) Then
                For j = 0 To UBound(rres, 2)
                    REM email per from address
                    If pfrom_address <> "" And pfrom_address <> rres(1, j) And recipients.Count > 0 Then
                        replyto.Clear()
                        replyto.Add(preply_to)
                        send_email(id, pfrom_address, recipients, replyto, title, body, True, attachments, use_bcc, 0, pdisplay_name)

                        recipients.Clear()
                        replyto.Clear()
                    End If
                    recipients.Add(rres(0, j))
                    pfrom_address = rres(1, j)
                    preply_to = rres(2, j)
                    pdisplay_name = rres(3, j)

                Next
                If recipients.Count > 0 Then
                    replyto.Clear()
                    replyto.Add(preply_to)
                    send_email(id, pfrom_address, recipients, replyto, title, body, True, attachments, use_bcc, 0, pdisplay_name)
                End If
            End If



        Catch ex As Exception
            Dim err As New bc_cs_error_log("bc_cs_email_services", "dothetask", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            thread_count = thread_count - 1
            ocomm = New bc_cs_activity_log("bc_cs_email_services", "dothetask", bc_cs_activity_codes.TRACE_EXIT, "", certificate)

        End Try
    End Sub

    Public Function send_email_direct(sender As String, recipients As List(Of String), replyto As List(Of String), subject As String, body As String, ishtml As Boolean, attachments As List(Of System.Net.Mail.Attachment), use_bcc As Boolean, Optional displayname As String = "") As Boolean
        Dim smtp_Server As SmtpClient

        Try
            send_email_direct = False

            Dim e_mail As New MailMessage()
            If bc_cs_central_settings.smtp_gateways.Count = 0 Then

                Exit Function
            Else
                smtp_Server = New SmtpClient
                smtp_Server.UseDefaultCredentials = bc_cs_central_settings.smtp_gateways(0)._userdefaultcredentials
                smtp_Server.Credentials = New Net.NetworkCredential(bc_cs_central_settings.smtp_gateways(0)._username, bc_cs_central_settings.smtp_gateways(0)._password)
                smtp_Server.Port = bc_cs_central_settings.smtp_gateways(0)._port
                'Smtp_Server.Timeout = bc_cs_central_settings.smtp_gateways(i)._timeout
                smtp_Server.EnableSsl = bc_cs_central_settings.smtp_gateways(0)._enable_ssl
                smtp_Server.Host = bc_cs_central_settings.smtp_gateways(0)._host
                smtp_Server.DeliveryMethod = SmtpDeliveryMethod.Network
            End If



            If displayname = "" Then
                e_mail.From = New MailAddress(sender)
            Else
                e_mail.From = New MailAddress(sender, displayname)
            End If

            For j = 0 To recipients.Count - 1
                If use_bcc = False Or recipients.Count = 1 Then
                    e_mail.To.Add(recipients(j))
                Else
                    e_mail.Bcc.Add(recipients(j))
                End If
            Next

            If Not IsNothing(attachments) Then
                For j = 0 To attachments.Count - 1
                    e_mail.Attachments.Add(attachments(j))
                Next
                For j = 0 To replyto.Count - 1
                    e_mail.ReplyToList.Add(replyto(j))
                Next
            End If

            e_mail.Subject = subject
            e_mail.IsBodyHtml = ishtml
            e_mail.Body = body

            smtp_Server.Send(e_mail)


            send_email_direct = True


            e_mail.Attachments.Dispose()

        Catch ex As Exception
            send_email_direct = False
        End Try
    End Function


    Public Function send_email(email_id As Long, sender As String, recipients As List(Of String), replyto As List(Of String), subject As String, body As String, ishtml As Boolean, attachments As List(Of System.Net.Mail.Attachment), use_bcc As Boolean, Optional doc_id As Long = 0, Optional displayname As String = "") As Boolean
        Dim smtp_Server As SmtpClient
        Dim ocomm As New bc_cs_activity_log("bc_cs_email_services", "send_email", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            send_email = False

            Dim e_mail As New MailMessage()
            If bc_cs_central_settings.smtp_gateways.Count = 0 Then
                send_email = False
                Dim elog As New bc_cs_email_log
                elog.log_error("no gateway", email_id, "", "bc_cs_email_services:send_email: no gateway loaded", certificate)
                Exit Function
            Else
                smtp_Server = New SmtpClient
                smtp_Server.UseDefaultCredentials = bc_cs_central_settings.smtp_gateways(0)._userdefaultcredentials
                If bc_cs_central_settings.smtp_gateways(0)._username <> "" Then
                    ocomm = New bc_cs_activity_log("bc_cs_email_services", "send_email", bc_cs_activity_codes.TRACE_EXIT, "set credentials", certificate)
                    smtp_Server.Credentials = New Net.NetworkCredential(bc_cs_central_settings.smtp_gateways(0)._username, bc_cs_central_settings.smtp_gateways(0)._password)
                End If
                If bc_cs_central_settings.smtp_gateways(0)._port <> 0 Then
                    ocomm = New bc_cs_activity_log("bc_cs_email_services", "send_email", bc_cs_activity_codes.TRACE_EXIT, "set port", certificate)
                    smtp_Server.Port = bc_cs_central_settings.smtp_gateways(0)._port
                Else
                    ocomm = New bc_cs_activity_log("bc_cs_email_services", "send_email", bc_cs_activity_codes.TRACE_EXIT, "using default port", certificate)
                End If
                'Smtp_Server.Timeout = bc_cs_central_settings.smtp_gateways(i)._timeout
                smtp_Server.EnableSsl = bc_cs_central_settings.smtp_gateways(0)._enable_ssl
                smtp_Server.Host = bc_cs_central_settings.smtp_gateways(0)._host
                smtp_Server.DeliveryMethod = SmtpDeliveryMethod.Network
            End If



            If displayname = "" Then
                e_mail.From = New MailAddress(sender)
            Else
                e_mail.From = New MailAddress(sender, displayname)
            End If

            For j = 0 To recipients.Count - 1
                If use_bcc = False Or recipients.Count = 1 Then
                    e_mail.To.Add(recipients(j))
                Else
                    e_mail.Bcc.Add(recipients(j))
                End If
            Next

            If Not IsNothing(attachments) Then
                For j = 0 To attachments.Count - 1
                    e_mail.Attachments.Add(attachments(j))
                Next
                For j = 0 To replyto.Count - 1
                    e_mail.ReplyToList.Add(replyto(j))
                Next
            End If

            e_mail.Subject = subject
            e_mail.IsBodyHtml = ishtml
            e_mail.Body = body



            If bc_cs_central_settings.smtp_gateways(0)._async = False Then
                If send(smtp_Server, email_id, smtp_Server.Host, e_mail, certificate) = True Then
                    send_email = True
                End If
            Else
                If send_async(smtp_Server, email_id, smtp_Server.Host, e_mail, certificate) = True Then
                    send_email = True
                End If
            End If
            e_mail.Attachments.Dispose()
        Catch ex As Exception
            'Dim err As New bc_cs_error_log("bc_cs_email_services", "send_email", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            Dim elog As New bc_cs_email_log
            elog.log_error(smtp_Server.Host, email_id, "", "bc_cs_email_services:send_email" + ex.Message, certificate)
        Finally
            ocomm = New bc_cs_activity_log("bc_cs_email_services", "send_email", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Function
    Private Function send(Smtp_Server As SmtpClient, email_id As Long, gateway As String, e_mail As MailMessage, ByRef certificate As bc_cs_security.certificate, Optional doc_Id As Long = 0) As Boolean
        Dim ocomm As New bc_cs_activity_log("bc_cs_email_services", "send", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)


        Dim elog As New bc_cs_email_log
        Try
            Smtp_Server.Send(e_mail)
            elog.log_success(email_id, e_mail.From.Address, certificate)



            send = True
        Catch ex As Exception
            elog.log_error(gateway, email_id, e_mail.From.Address, ex.Message, certificate)
            send = False
        Finally
            ocomm = New bc_cs_activity_log("bc_cs_email_services", "send", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Function
    Private Class async_params
        Public email_id As Long
        Public gateway As String
        Public from_address As String
        Public certificate As bc_cs_security.certificate
    End Class
    Private Function send_async(Smtp_Server As SmtpClient, email_id As Long, gateway As String, e_mail As MailMessage, ByRef certificate As bc_cs_security.certificate, Optional doc_Id As Long = 0) As Boolean
        Dim elog As New bc_cs_email_log
        Try
            AddHandler Smtp_Server.SendCompleted, AddressOf SendCompletedCallback
            Dim token As New async_params
            token.email_id = email_id
            token.gateway = gateway
            token.from_address = e_mail.From.Address
            token.certificate = certificate
            Smtp_Server.SendAsync(e_mail, token)
            async_email_id = email_id
            async_from_address = e_mail.From.Address
            send_async = True
        Catch ex As Exception
            elog.log_error(gateway, email_id, e_mail.From.Address, "err: " + ex.Message, certificate)
            send_async = False
        End Try
    End Function
    Public async_email_id As Integer
    Public async_from_address As String
    Private Sub SendCompletedCallback(ByVal sender As Object, ByVal e As AsyncCompletedEventArgs)
        Dim elog As New bc_cs_email_log

        Try

            'Dim certificate As New bc_cs_security.certificate

            'certificate = e.UserState.certificate

            If e.Cancelled Then
                elog.log_error(e.UserState.gateway, e.UserState.email_id, e.UserState.from_address, "send cancelled", certificate)
            End If
            If e.Error IsNot Nothing Then
                elog.log_error(e.UserState.gateway, e.UserState.email_id, e.UserState.from_address, e.Error.Message + ":" + e.Error.InnerException.ToString, certificate)
            Else
                elog.log_success(e.UserState.email_id, e.UserState.from_address, certificate)
            End If

        Catch ex As Exception
            elog.log_error("", async_email_id, async_from_address, "Aync callback error: " + ex.Message, certificate)
        End Try

    End Sub
    Public Class bc_cs_smtp_gateway
        Public _host As String
        Public _enable_ssl As Boolean
        Public _username As String
        Public _password As String
        Public _port As Integer
        Public _userdefaultcredentials As Boolean
        Public _timeout As Integer
        Public _delivery_method As Integer = SmtpDeliveryMethod.Network
        Public _async As Boolean
    End Class
    Public Class bc_cs_email_log
        Dim db As New bc_cs_db_services


        Public Sub log_error(gateway As String, email_id As Long, from_address As String, msg As String, ByRef certificate As bc_cs_security.certificate)
            Try
                Dim fs As New bc_cs_string_services(msg)
                msg = fs.delimit_apostrophies()
                Dim sql As String
                sql = "exec dbo.bc_core_log_email_error '" + CStr(gateway) + "'," + CStr(email_id) + ",'" + CStr(from_address) + "','" + msg + "'"
                db.executesql(sql, certificate)
            Catch ex As Exception
                Dim err As New bc_cs_error_log("bc_cs_email_log", "log_error", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)

            End Try
        End Sub
        Public Sub log_success(email_id As Long, from_address As String, ByRef certificate As bc_cs_security.certificate)
            Try
                Dim sql As String
                sql = "exec dbo.bc_core_log_email_success " + CStr(email_id) + ",'" + CStr(from_address) + "'"
                db.executesql(sql, certificate)
            Catch ex As Exception
                Dim err As New bc_cs_error_log("bc_cs_email_log", "log_error", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)

            End Try
        End Sub

    End Class
End Class
Public Class bc_cs_email_templates_cache
    Public Shared cached_email_templates As New List(Of bc_cs_email_template_content)
    Public Class bc_cs_email_template_content
        Public id As Integer
        Public filename As String
        Public template_content As String
    End Class
    Public Function load_templates(ByRef certificate As bc_cs_security.certificate) As Boolean
        Try
            load_templates = False
            Dim db As New bc_cs_email_template.db_bc_cs_email_template
            Dim res As Object
            Dim et As bc_cs_email_template_content
            Dim fn As String
            Dim fs As New bc_cs_file_transfer_services
            Dim sw As StreamReader
            If cached_email_templates.Count > 0 Then
                load_templates = True
                Exit Function
            End If
            res = db.get_email_templates(certificate)
            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    et = New bc_cs_email_template_content
                    et.id = res(0, i)
                    et.filename = res(1, i)

                    REM read the template in
                    fn = bc_cs_central_settings.central_template_path + "email_templates\" + et.filename
                    If fs.check_document_exists(fn, certificate) = False Then
                        Dim err As New bc_cs_error_log("bc_cs_email_template_content", "load_templates", bc_cs_error_codes.USER_DEFINED, "cant find email template: " + fn, certificate)
                    Else
                        sw = New StreamReader(fn)
                        et.template_content = sw.ReadToEnd
                        cached_email_templates.Add(et)
                        sw.Close()
                    End If
                Next
            End If
            load_templates = True
        Catch ex As Exception
            Dim err As New bc_cs_error_log("bc_cs_email_templates_cache", "load_templates", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        End Try
    End Function
End Class
Public Class bc_cs_email_template
    Dim _template_Id As Integer
    Dim _entity_Id As Long
    Dim _doc_id As Long
    Dim _user_id As Long
    Dim _stage_id As Long
    Dim _digest1 As Long
    Dim _digest2 As Long
    Dim _db As bc_cs_db_services

    Public err_text As String
    Dim _certificate As bc_cs_security.certificate
    Public content As String

    Enum COMPONENT_TYPE
        EMAIL = 1
        STANDARD = 2
    End Enum
    Public Sub New(ByRef certificate As bc_cs_security.certificate)
        _certificate = certificate
    End Sub
    Public Sub New(template_Id As Integer, entity_Id As Long, doc_id As Long, user_i As Long, stage_Id As Long, digest1 As Long, digest2 As Long, ByRef certificate As bc_cs_security.certificate, ByRef db As bc_cs_db_services)
        _template_Id = template_Id
        _entity_Id = entity_Id
        _doc_id = doc_id
        _user_id = _user_id
        _stage_id = stage_Id
        _digest1 = digest1
        _digest2 = digest2
        _certificate = certificate
        _db = db
    End Sub
    Public Function execute_template(Optional save_to_file As Boolean = False) As Boolean
        execute_template = False
        Try
            Dim db As New db_bc_cs_email_template
            Dim res As Object
            Dim template_name As String
            Dim str As String
            REM check if template is in cahce
            For i = 0 To bc_cs_email_templates_cache.cached_email_templates.Count - 1
                If bc_cs_email_templates_cache.cached_email_templates(i).id = _template_Id Then
                    Dim ocomm As New bc_cs_activity_log("bc_cs_emaiL_template", "execute_template", bc_cs_activity_codes.COMMENTARY, "Template Id: " + CStr(_template_Id) + " found in cache", _certificate)
                    execute_template = parse_template(bc_cs_email_templates_cache.cached_email_templates(i).template_content)
                    Exit Function
                End If
            Next
            res = db.get_template_name(_template_Id, _certificate)
            If IsArray(res) AndAlso UBound(res, 2) = 0 Then

                template_name = res(0, 0)
                If template_name = "" Then
                    err_text = "cant get template name for template id blank " + CStr(_template_Id)
                    execute_template = False
                    Exit Function
                End If
            Else
                err_text = "cant get template name for template id nothing " + CStr(_template_Id)
                execute_template = False
                Exit Function
            End If
                REM read the template in
                Dim fn As String
                fn = bc_cs_central_settings.central_template_path + "email_templates\" + template_name

                Dim fs As New bc_cs_file_transfer_services
                If fs.check_document_exists(fn, _certificate) = False Then
                    err_text = "cant find email template: " + fn
                    execute_template = False
                    Exit Function
                End If


                Dim sw As New StreamReader(fn)
                str = sw.ReadToEnd
                sw.Close()
                REM now ass to cache
                Dim et As New bc_cs_email_templates_cache.bc_cs_email_template_content
                et.id = _template_Id
                et.template_content = str
                bc_cs_email_templates_cache.cached_email_templates.Add(et)
            execute_template = parse_template(str)

        Catch ex As Exception
            err_text = ex.Message
            execute_template = False
        Finally
        End Try
    End Function
    Function parse_template(str As String)
        parse_template = False
        Try
            REM look for BC markup
            Dim s, e As Integer
            Dim comp_Id As String
            Dim component_value As String
            s = InStr(str, "{BCCOMPEMAIL")
            While s > 0
                e = InStr(s + 12, str, "}")
                comp_Id = str.Substring(s + 11, e - (s + 12))
                If Not IsNumeric(comp_Id) Then
                    err_text = "parse_template: invalid component id: " + comp_Id
                    Exit Function
                End If
                component_value = get_value_for_component(CInt(comp_Id), COMPONENT_TYPE.EMAIL, _db)
                If err_text <> "" Then
                    Exit Function
                End If
                str = str.Replace("{BCCOMPEMAIL" + comp_Id + "}", component_value)
                s = InStr(str, "{BCCOMPEMAIL")
            End While
            content = str
            parse_template = True
        Catch ex As Exception
            err_text = "parse_template " + ex.Message
        Finally
        End Try
    End Function
    REM called from ajax service for e docs
    Public Function get_value_for_component(doc_id As Long, comp_id As Integer) As String
        Try
            get_value_for_component = ""
            _doc_id = doc_id
           
            'Dim gdb As New bc_cs_db_services
            'gdb.open_conn(_certificate)
            get_value_for_component = get_value_for_component(comp_id, COMPONENT_TYPE.EMAIL, Nothing)
            'gdb.close_conn(True, _certificate)
        Catch ex As Exception
            err_text = "get_value_for_component " + ex.Message
            get_value_for_component = ""
        End Try
    End Function
    Function get_value_for_component(comp_id As Integer, type As Integer, ByRef gdb As bc_cs_db_services) As String
        Dim sp_name As String
        Dim db As New db_bc_cs_email_template
       
        Dim res As Object
        REM email specirfict components
        REM late omn use ordianry components ans maybe wcf data layer with json returned and parse
        Try
            get_value_for_component = ""
            Select Case type
                Case COMPONENT_TYPE.EMAIL
                    res = db.get_sp_for_email_component(comp_id, _certificate)
                  
                    If IsArray(res) Then
                        Try
                            sp_name = CStr(res(0, 0))
                            _certificate.server_errors.Clear()
                            err_text = ""
                            res = db.execute_email_component(sp_name, _entity_Id, _doc_id, _user_id, _stage_id, _digest1, _digest2, err_text, _certificate, gdb)
                            If err_text <> "" Then
                                err_text = "get_value_for_component failed to execute sp: " + err_text
                                get_value_for_component = ""
                                _certificate.server_errors.Clear()
                                Exit Function
                            End If
                            If IsArray(res) Then
                                Try
                                    get_value_for_component = res(0, 0)
                                Catch
                                    get_value_for_component = ""
                                    Exit Function
                                End Try
                            Else
                                get_value_for_component = ""
                                Exit Function
                            End If


                        Catch ex As Exception
                            err_text = "get_value_for_component cant get sp for component: " + CStr(comp_id)
                        End Try
                    Else
                        err_text = "get_value_for_component cant get sp for component: " + CStr(comp_id)
                    End If

                Case COMPONENT_TYPE.STANDARD
                    REM TBD
            End Select

        Catch ex As Exception
            err_text = "get_value_for_component " + ex.Message
            get_value_for_component = ""
        Finally
        End Try
    End Function


    Public Class db_bc_cs_email_template
        Dim db As New bc_cs_db_services
        Public Function get_email_templates(ByRef certificate As bc_cs_security.certificate)
            Dim sql As String
            sql = "exec dbo.bc_core_get_email_templates"
            get_email_templates = db.executesql(sql, certificate)
        End Function
        Public Function get_template_name(id As Integer, ByRef certificate As bc_cs_security.certificate)
            Dim sql As String
            sql = "exec dbo.bc_core_get_email_template " + CStr(id)
            get_template_name = db.executesql(sql, certificate)
        End Function
        Public Function get_sp_for_email_component(id As Integer, ByRef certificate As bc_cs_security.certificate)
            Dim sql As String
            sql = "exec dbo.bc_core_get_email_component " + CStr(id)
            get_sp_for_email_component = db.executesql(sql, certificate)
        End Function
        Public Function execute_email_component(sp_name As String, entity_Id As Long, doc_Id As Long, user_id As Long, stage_id As Long, digest1 As Long, digest2 As Long, ByRef err_tx As String, ByRef certificate As bc_cs_security.certificate, ByRef gdb As bc_cs_db_services)
            Dim sql As String
            sql = "exec dbo." + sp_name + " " + CStr(entity_Id) + "," + CStr(doc_Id) + "," + CStr(user_id) + "," + CStr(stage_id) + "," + CStr(digest1) + "," + CStr(digest2)
            If IsNothing(gdb) Then
                execute_email_component = db.executesql_return_error(sql, certificate, err_tx, Nothing)
            Else

                execute_email_component = gdb.executesql_return_error(sql, certificate, err_tx, gdb)
            End If
        End Function
    End Class

End Class
Public Class bc_cs_distribution_list

    Dim certificate As New bc_cs_security.certificate
    Dim thread_count As Integer = 0
    Public Sub New(lcerticiate As bc_cs_security.certificate)
        Try
            REM cache email templates
            Me.certificate = lcerticiate

        Catch ex As Exception
            Dim err As New bc_cs_error_log("bc_cs_distribution_list", "new", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            certificate.server_errors.Clear()
        End Try
    End Sub

      Public Sub service_poll()
        Try
            Dim db As New bc_cs_db_services
            Dim sql As String
            Dim eres As Object

            sql = "exec dbo.bc_core_reach_get_pending_distribution_lists"
            eres = db.executesql(sql, certificate)

            Dim pas As Thread
            If IsArray(eres) Then
                For i = 0 To UBound(eres, 2)
                    Dim j As Integer
                    j = i
                    pas = New Thread(Sub() Me.dothetask(eres(0, j)))
                    pas.Start()
                Next
            End If
        Catch ex As Exception
            Dim err As New bc_cs_error_log("bc_cs_distribution_list", "service_poll", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        End Try
    End Sub

    REM get any attachments
    Sub dothetask(id As Long)
        Dim db As New bc_cs_db_services
        Dim sql As String

       
        Try
            sql = "exec dbo.bc_core_re_generate_recipients_for_doc  " + CStr(id)
            db.executesql(sql, certificate)

        Catch ex As Exception
            Dim err As New bc_cs_error_log("bc_cs_distribution_list", "dothetask", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
        End Try
    End Sub
End Class





