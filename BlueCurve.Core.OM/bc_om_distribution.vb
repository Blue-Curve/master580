Imports BlueCurve.Core.CS
Imports System.Threading
<Serializable()> Public Class bc_om_distribution_channels_config
    Inherits bc_cs_soap_base_class
    Public channels As New List(Of bc_om_distribution_channel_config)

    Public Overrides Sub process_object()
        Select Case tmode
            Case bc_cs_soap_base_class.tREAD
                db_read()
            Case bc_cs_soap_base_class.tWRITE
                db_write()
        End Select
    End Sub
    Public Sub db_read()
        Try
            Dim gdb As New bc_cs_db_services
            Dim res As Object
            Dim c As bc_om_distribution_channel_config
            res = gdb.executesql("bc_core_get_all_channel_info", certificate)
            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    c = New bc_om_distribution_channel_config
                    c.id = res(0, i)
                    c.name = res(1, i)
                    c.generate_mail_list = res(2, i)
                    c.channel_group = res(3, i)
                    c.transfer_method = res(4, i)
                    c.files_sp = res(5, i)
                    c.uri = res(6, i)
                    c.username = res(7, i)
                    c.password = res(8, i)
                    c.fingerprint = res(9, i)
                    c.port = res(10, i)
                    c.dir = res(11, i)
                    c.compress = res(12, i)
                    c.in_use = res(13, i)

                    channels.Add(c)
                Next
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_distribution_channels_config", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try
    End Sub
    Public Sub db_write()
        Try
            Dim gdb As New bc_cs_db_services
            gdb.executesql("bc_core_delete_channels", certificate)
            Dim fs As bc_cs_string_services


            For i = 0 To channels.Count - 1
                fs = New bc_cs_string_services(channels(i).name)
                channels(i).name = fs.delimit_apostrophies
                fs = New bc_cs_string_services(channels(i).username)
                channels(i).username = fs.delimit_apostrophies
                fs = New bc_cs_string_services(channels(i).password)
                channels(i).password = fs.delimit_apostrophies
                fs = New bc_cs_string_services(channels(i).fingerprint)
                channels(i).fingerprint = fs.delimit_apostrophies
                fs = New bc_cs_string_services(channels(i).uri)
                channels(i).uri = fs.delimit_apostrophies
                fs = New bc_cs_string_services(channels(i).files_sp)
                channels(i).files_sp = fs.delimit_apostrophies
                gdb.executesql("bc_core_update_channel " + CStr(channels(i).id) + ",'" + CStr(channels(i).name) + "'," + CStr(channels(i).generate_mail_list) + "," + CStr(channels(i).files_sp) + "," + CStr(channels(i).channel_group) + "," + CStr(channels(i).transfer_method) + ",'" + CStr(channels(i).uri) + "','" + CStr(channels(i).username) + "','" + CStr(channels(i).password) + "','" + CStr(channels(i).fingerprint) + "'," + CStr(channels(i).port) + ",'" + CStr(channels(i).dir) + "'," + CStr(channels(i).compress), certificate)
            Next

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_distribution_channels_config", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub

End Class



<Serializable()> Public Class bc_om_distribution_channel_config

    Inherits bc_cs_soap_base_class
    Public id As Integer
    Public name As String
    Public generate_mail_list As Boolean
    Public channel_group As Integer
    Public transfer_method As bc_cs_net_send_channel.eTRANSFER_METHOD
    Public files_sp As String
    Public uri As String
    Public username As String
    Public password As String
    Public fingerprint As String
    Public port As Integer
    Public dir As String
    Public compress As Boolean
    Public in_use As Boolean = False

End Class

<Serializable()> Public Class bc_om_pub_type_attribute
    Inherits bc_cs_soap_base_class
    Public pub_type_id As Long
    Public att_val As bc_om_attribute_value


    Public Overrides Sub process_object()
        Select Case tmode
            Case tWRITE
                db_write()
        End Select
    End Sub

    Public Sub db_write()
        Dim db As New bc_cs_db_services
        Dim sql As String
        Dim fs As New bc_cs_string_services(att_val.value)
        att_val.value = fs.delimit_apostrophies()



        sql = "exec dbo.bc_core_set_pub_type_attribute_value " + CStr(pub_type_id) + "," + CStr(att_val.attribute_Id) + ",'" + att_val.value + "'"
        db.executesql(sql, certificate)
    End Sub
End Class




<Serializable()> Public Class bc_om_pub_type_attributes
    Inherits bc_cs_soap_base_class
    Public pt_id As Long
    Public attributes As New List(Of bc_om_attribute)
    Public no_lists As Boolean = False
    Public Overrides Sub process_object()
        Select Case tmode
            Case tREAD
                db_read()
        End Select
    End Sub
    

    Public Sub db_read()
        Dim db As New bc_cs_db_services
        Dim res As Object
        Dim lres As Object

        Dim att As bc_om_attribute


        REM attributes
        res = db.executesql("exec dbo.bc_core_get_pub_type_attributes " + CStr(pt_id), certificate)


        If IsArray(res) Then
            For i = 0 To UBound(res, 2)
                att = New bc_om_attribute
                att.attribute_id = res(0, i)
                att.name = res(1, i)
                att.is_lookup = res(2, i)
                att.type_id = res(3, i)
                att.length = res(4, i)
                att.lookup_sql = res(5, i)
                att.nullable = res(7, i)
                att.repeats = res(8, i)
                att.show_workflow = res(9, i)
                att.persmission = res(11, i)
                att.is_def = res(12, i)
                att.show_filter = res(13, i)

                If att.is_lookup = True And no_lists = False Then
                    lres = db.executesql(att.lookup_sql, certificate)
                    If IsArray(lres) Then
                        For j = 0 To UBound(lres, 2)
                            att.lookup_keys.Add(lres(0, j))
                            att.lookup_values.Add(lres(1, j))
                        Next
                    End If
                End If
                att.default_value = res(6, i)

                attributes.Add(att)


            Next
        End If
      
        REM values

    End Sub
End Class


<Serializable> Public Class bc_om_display_taxonomy
    Inherits bc_cs_soap_base_class
    Public doc As bc_om_document
    Public dstr As String
    Public read_mode As Integer = 0
    Public single_class_id As Long
    Public implicit_tags As New List(Of bc_om_entity)
    Public Overrides Sub process_object()
        Select Case tmode
            Case bc_cs_soap_base_class.tREAD
                Select Case read_mode
                    Case 0
                        db_read_all()
                    Case 1
                        db_read_single_class()
                End Select

        End Select
    End Sub
    Public Sub db_read_all()
        Try
            Dim db As New bc_cs_db_services
            Dim res As Object

            Dim xml As String
            xml = "<taxonomy>"

            For i = 0 To doc.authors.Count - 1
                xml = xml + "<author_id><id>" + CStr(doc.authors(i).id) + "</id></author_id>"
            Next

            For i = 0 To doc.taxonomy.Count - 1
                xml = xml + "<entity_id><id>" + CStr(doc.taxonomy(i).entity_id) + "</id></entity_id>"
            Next
            For i = 0 To doc.disclosures.Count - 1
                xml = xml + "<disc_id><id>" + CStr(doc.disclosures(i).entity_id) + "</id></disc_id>"
            Next

            xml = xml + "</taxonomy>"

            res = db.executesql("exec dbo.bc_core_get_display_tax_for_entity " + CStr(doc.pub_type_id) + ",'" + xml + "'", certificate)

            If IsArray(res) Then
                dstr = res(0, 0)
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_display_taxonomy", "db_read_all", bc_cs_activity_codes.COMMENTARY, ex.Message, certificate)
        End Try
    End Sub
    Public Sub db_read_single_class()
        Try
            Dim db As New bc_cs_db_services
            Dim res As Object
            implicit_tags.Clear()

            Dim xml As String
            xml = "<taxonomy>"

            For i = 0 To doc.authors.Count - 1
                xml = xml + "<author_id><id>" + CStr(doc.authors(i).id) + "</id></author_id>"
            Next

            For i = 0 To doc.taxonomy.Count - 1
                xml = xml + "<entity_id><id>" + CStr(doc.taxonomy(i).entity_id) + "</id></entity_id>"
            Next
            For i = 0 To doc.disclosures.Count - 1
                xml = xml + "<disc_id><id>" + CStr(doc.disclosures(i).entity_id) + "</id></disc_id>"
            Next

            xml = xml + "</taxonomy>"

            res = db.executesql("exec dbo.bc_core_get_display_tax_for_entity_class " + CStr(doc.pub_type_id) + ",'" + xml + "'," + CStr(single_class_id), certificate)
            Dim oent As bc_om_entity

            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    oent = New bc_om_entity
                    oent.name = res(0, i)
                    implicit_tags.Add(oent)

                Next
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_display_taxonomy", "db_read_single_class", bc_cs_activity_codes.COMMENTARY, ex.Message, certificate)
        End Try
    End Sub
End Class

<Serializable> Public Class bc_om_pt_dist_config
    Inherits bc_cs_soap_base_class

    Public pub_type_id As String
    Public all_channels As New List(Of bc_om_dist_channel)
    Public sel_channels As New List(Of Long)
    Public all_classes As New List(Of bc_om_entity_class)
    Public sel_classes As New List(Of bc_om_extended_taxonomy)
    Public email_templates As New List(Of bc_om_email_template)
    Public sel_body_email_template As Long
    Public sel_title_email_template As Long
    Public automatic As Boolean = True
    Public all_products As New List(Of bc_om_entity)
    Public sel_products As New List(Of Long)
    Public all_product_types As New List(Of bc_om_entity)
    Public sel_product_type As Long
    Public attributes As New bc_om_pub_type_attributes



    Public Overrides Sub process_object()
        Select Case tmode
            Case bc_cs_soap_base_class.tREAD
                db_read()
            Case bc_cs_soap_base_class.tWRITE
                db_write()
        End Select
    End Sub


    Public Sub db_read()
        Try
            Dim db As New bc_cs_db_services
            Dim sql As String
            Dim res As Object
            Dim oc As bc_om_dist_channel
            Dim pr As bc_om_entity

            sql = "exec dbo.bc_core_re_admin_get_product_types"
            res = db.executesql(sql, certificate)

            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    pr = New bc_om_entity
                    pr.id = res(0, i)
                    pr.name = res(1, i)
                    Me.all_product_types.Add(pr)
                Next
            End If

            sql = "exec dbo.bc_core_re_admin_get_sel_product_type " + CStr(pub_type_id)
            res = db.executesql(sql, certificate)
            If IsArray(res) Then
                If UBound(res, 2) = 0 Then
                    sel_product_type = res(0, 0)
                End If
            End If


            sql = "exec dbo.bc_core_re_admin_get_products"
            res = db.executesql(sql, certificate)

            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    pr = New bc_om_entity
                    pr.id = res(0, i)
                    pr.name = res(1, i)
                    Me.all_products.Add(pr)

                Next
            End If
            sql = "exec dbo.bc_core_re_admin_get_sel_products " + CStr(pub_type_id)
            res = db.executesql(sql, certificate)
            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    sel_products.Add(res(0, i))
                Next
            End If
            sql = "exec dbo.bc_core_re_admin_get_channels"
            res = db.executesql(sql, certificate)
            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    oc = New bc_om_dist_channel
                    oc.channel_id = res(0, i)
                    oc.channel_name = res(1, i)
                    Me.all_channels.Add(oc)
                Next
            End If
            sql = "exec dbo.bc_core_re_admin_get_channels_for_pt " + CStr(pub_type_id)
            res = db.executesql(sql, certificate)
            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    Me.sel_channels.Add(res(0, i))
                    Me.automatic = res(1, i)

                Next
            End If
            Dim et As bc_om_email_template
            sql = "exec dbo.bc_core_re_admin_get_email_templates "
            res = db.executesql(sql, certificate)
            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    et = New bc_om_email_template
                    et.email_template_id = res(0, i)
                    et.html_filename = res(1, i)
                    Me.email_templates.Add(et)
                Next
            End If
            sql = "exec dbo.bc_core_re_admin_get_email_template_for_pt " + CStr(pub_type_id)
            res = db.executesql(sql, certificate)
            If IsArray(res) Then
                If UBound(res, 2) = 0 Then
                    Me.sel_body_email_template = res(0, 0)
                End If
            End If
            sql = "exec dbo.bc_core_re_admin_get_taxonomy_classes " + CStr(pub_type_id)
            res = db.executesql(sql, certificate)
            Dim tx As bc_om_entity_class

            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    tx = New bc_om_entity_class
                    tx.class_id = res(0, i)
                    tx.class_name = res(1, i)
                    Me.all_classes.Add(tx)
                Next
            End If

            sql = "exec dbo.bc_core_re_admin_get_taxonomy_classes_for_pt " + CStr(pub_type_id)
            res = db.executesql(sql, certificate)
            Dim stx As bc_om_extended_taxonomy

            If IsArray(res) Then
                For i = 0 To UBound(res, 2)

                    stx = New bc_om_extended_taxonomy
                    stx.class_id = res(0, i)
                    stx.class_name = res(1, i)
                    stx.mandatory = res(2, i)
                    stx.max_num = res(3, i)
                    stx.dependent_class_id = res(4, i)
                    stx.has_default_value = res(5, i)

                    Me.sel_classes.Add(stx)
                Next
            End If
           

            attributes.certificate = certificate
            attributes.pt_id = pub_type_id
            attributes.db_read()



        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_pt_dist_config", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        End Try
    End Sub
    Public Sub db_write()
        Try
            Dim db As New bc_cs_db_services
            Dim sql As String
            sql = "exec dbo.bc_core_re_admin_re_pt_dist " + CStr(pub_type_id)
            db.executesql(sql, certificate)
            For i = 0 To sel_channels.Count - 1
                sql = "exec dbo.bc_core_re_admin_add_channel_for_pt " + CStr(pub_type_id) + "," + CStr(sel_channels(i)) + "," + CStr(automatic)
                db.executesql(sql, certificate)
            Next
            sql = "exec dbo.bc_core_re_admin_add_email_template " + CStr(pub_type_id) + "," + CStr(sel_body_email_template)
            db.executesql(sql, certificate)

            For i = 0 To sel_classes.Count - 1
                sql = "exec dbo.bc_core_re_admin_add_tax_for_pt " + CStr(i + 1) + "," + CStr(pub_type_id) + "," + CStr(sel_classes(i).class_id) + "," + CStr(sel_classes(i).mandatory) + "," + CStr(sel_classes(i).max_num) + "," + CStr(sel_classes(i).dependent_class_id) + "," + CStr(sel_classes(i).has_default_value)
                db.executesql(sql, certificate)
            Next
            For i = 0 To sel_products.Count - 1
                sql = "exec dbo.bc_core_re_admin_set_sel_product " + CStr(pub_type_id) + "," + CStr(sel_products(i))
                db.executesql(sql, certificate)
            Next
            sql = "exec dbo.bc_core_re_admin_set_sel_product_type " + CStr(pub_type_id) + "," + CStr(sel_product_type)
            db.executesql(sql, certificate)

            For i = 0 To attributes.attributes.Count - 1
                sql = "exec dbo.bc_core_set_pub_type_attribute_value " + CStr(pub_type_id) + "," + CStr(attributes.attributes(i).attribute_id) + ",'" + attributes.attributes(i).default_value + "'"
                db.executesql(sql, certificate)
            Next

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_pt_dist_config", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        End Try
    End Sub

    <Serializable> Public Class bc_om_dist_channel
        Public channel_id As Long
        Public channel_name As String
        Public automatic As Boolean
    End Class
    <Serializable> Public Class bc_om_extended_taxonomy
        Inherits bc_om_entity_class
        Public mandatory As Boolean = False
        Public max_num As Integer = 0
        Public has_default_value As Boolean = False
        Public dependent_class_id As Long = 0

    End Class
End Class
<Serializable> Public Class bc_om_distribution_for_pub_type
    Inherits bc_cs_soap_base_class
    Public pub_type_id As Long
    Public channels As New List(Of bc_om_distribution.bc_om_doc_distribution_channel)
    Public sel_channels As New List(Of bc_om_distribution.bc_om_doc_distribution_channel)
    Public email_templates As New List(Of bc_om_distribution_email_template)
    Public automatic As Boolean
    <Serializable> Public Class bc_om_distribution_email_template
        Inherits bc_cs_soap_base_class
        Public id As Integer
        Public filename As String
        Public title_template As Boolean
    End Class
    Public Overrides Sub process_object()
        Select Case tmode
            Case tREAD
                db_read()
            Case tWRITE
                db_write()
        End Select
    End Sub
    Public Sub db_read()
        Try
            Dim res As Object
            Dim db As New db_bc_om_distribution_for_pub_type
            Dim c As bc_om_distribution.bc_om_doc_distribution_channel
            Dim t As bc_om_distribution_email_template
            res = db.get_all_channels(certificate)
            Me.channels.Clear()
            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    c = New bc_om_distribution.bc_om_doc_distribution_channel
                    c.channel_id = res(0, i)
                    c.name = res(1, i)
                    c.targetted = res(2, i)
                    Me.channels.Add(c)
                Next
            End If
            res = db.get_all_email_templates(certificate)
            Me.email_templates.Clear()
            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    t = New bc_om_distribution_email_template
                    t.id = res(0, i)
                    t.filename = res(1, i)
                    Me.email_templates.Add(t)
                Next
            End If
            res = db.get_channels_for_pt(pub_type_id, certificate)

            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    c = New bc_om_distribution.bc_om_doc_distribution_channel
                    c.channel_id = res(0, i)
                    c.title_template = res(1, i)
                    c.body_template = res(2, i)
                    Me.automatic = res(3, i)
                    Me.sel_channels.Add(c)
                Next
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_distribution_for_pub_type", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Sub
    Public Sub db_write()
        Try
            Dim db As New db_bc_om_distribution_for_pub_type
            db.rem_channels_for_pt(pub_type_id, certificate)
            For i = 0 To sel_channels.Count - 1
                db.add_channel_for_pt(pub_type_id, sel_channels(i).channel_id, automatic, certificate)
                If sel_channels(i).title_template <> 0 And sel_channels(i).body_template Then
                    db.add_email_template_for_pub_type(pub_type_id, sel_channels(i).title_template, sel_channels(i).body_template, certificate)
                End If
            Next

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_distribution_for_pub_type", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Sub
    Class db_bc_om_distribution_for_pub_type
        Dim gdb As New bc_cs_db_services

        Public Function get_all_channels(ByRef certficate As bc_cs_security.certificate)
            Dim sql As String
            sql = "exec dbo.bc_core_re_get_all_distribution_channels"
            get_all_channels = gdb.executesql(sql, certficate)
        End Function
        Public Function get_all_email_templates(ByRef certficate As bc_cs_security.certificate)
            Dim sql As String
            sql = "exec dbo.bc_core_re_get_all_email_templates"
            get_all_email_templates = gdb.executesql(sql, certficate)
        End Function
        Public Function get_channels_for_pt(pub_type_id As Long, ByRef certficate As bc_cs_security.certificate)
            Dim sql As String
            sql = "exec dbo.bc_core_re_get_channels_for_pt " + CStr(pub_type_id)
            get_channels_for_pt = gdb.executesql(sql, certficate)
        End Function
        Public Sub rem_channels_for_pt(pub_type_id As Long, ByRef certficate As bc_cs_security.certificate)
            Dim sql As String
            sql = "exec dbo.bc_core_re_rem_channels_for_pt " + CStr(pub_type_id)
            gdb.executesql(sql, certficate)
        End Sub
        Public Sub add_channel_for_pt(pub_type_id As Long, channel_id As Integer, automatic As Boolean, ByRef certficate As bc_cs_security.certificate)
            Dim sql As String
            sql = "exec dbo.bc_core_re_add_channel_for_pt " + CStr(pub_type_id) + "," + CStr(channel_id) + "," + CStr(automatic)
            gdb.executesql(sql, certficate)
        End Sub
        Public Sub add_email_template_for_pub_type(pub_type_id As Long, title_template_id As Integer, body_template_id As Integer, ByRef certficate As bc_cs_security.certificate)
            Dim sql As String
            sql = "exec dbo.bc_core_re_add_email_template_for_pub_type " + CStr(pub_type_id) + "," + CStr(title_template_id) + "," + CStr(body_template_id)
            gdb.executesql(sql, certficate)
        End Sub
    End Class
End Class
<Serializable> Public Class bc_om_distribution
    Inherits bc_cs_soap_base_class
    Public doc_id As Long
    Public distribution_date As DateTime
    Public stage_change_date As DateTime
    Public distribution_type As reach_distribution_type
    Public status As reach_status_codes
    Public channels As List(Of bc_om_doc_distribution_channel) = New List(Of bc_om_doc_distribution_channel)
    Public channel_id As Integer
    Public resend_channel_id As Integer
    Public history As New List(Of bc_om_distribution_history)
    Public pub_types As New List(Of bc_om_pub_type)
    'Public merge_pub_type_id As Long
    Public merge_from_doc As bc_om_document
    Public mail_list_classes As New List(Of bc_om_entity_class)
    Public Enum reach_status_codes
        No_Distribution = 0
        Preparing = 1
        Mail_List_Generating = 3
        Awaiting_Distribution_Time = 4
        Ready_To_Distribute = 5
        Distributing = 6
        Distributed = 7
        Distribution_Failed = 8
        Ready_for_Mailing_List = 9
        Mail_List_Complete = 12
        Sending_Emails = 13
        Ready_To_Distribute_Manual = 14
    End Enum
    Public Enum reach_distribution_type
        Automatic = 1
        Manual = 0
    End Enum
    <Serializable> Public Class bc_om_distribution_history
        Public user As String
        Public da As DateTime
        Public comment As String
        Public msg As String
    End Class
    <Serializable> Public Class bc_om_doc_distribution_channel
        Public channel_id As Integer
        Public name As String
        Public targetted As Boolean
        Public list_generated As Boolean
        Public list_date As DateTime
        Public status_change_date As DateTime
        Public comment As String
        Public status As reach_status_codes
        Public preview_status As Integer
        Public preview_date As DateTime
        Public links As List(Of bc_om_preview_link) = New List(Of bc_om_preview_link)
        Public recipients As List(Of bc_om_client_contact) = New List(Of bc_om_client_contact)
        Public email_url As String
        Public email_title As String
        Public email_body As String
        Public title_template As Integer
        Public body_template As Integer
        Public title_template_name As Integer
        Public body_template_name As Integer
        Public generate_list As Boolean = False
    End Class
    <Serializable> Public Class bc_om_preview_link

        Public display_name As String
        Public url As String
    End Class
    <Serializable> Public Class bc_om_selected_clients_for_doc
        Inherits bc_cs_soap_base_class
        Public doc_id As Long
        Public selected_clients As New List(Of String)
        Public Overrides Sub process_object()
            Select Case tmode
                Case bc_cs_soap_base_class.tWRITE
                    db_write()
            End Select
        End Sub
        Public Sub db_write()
            Try
                Dim gdb As New bc_cs_db_services
                Dim sql As String
                Dim user_id As Long
                If bc_cs_central_settings.server_flag = 0 Then
                    user_id = bc_cs_central_settings.logged_on_user_id
                Else
                    user_id = certificate.user_id
                End If
                For i = 0 To selected_clients.Count - 1
                    sql = "exec dbo.bc_core_re_add_doc_recipient " + CStr(doc_id) + ",'" + CStr(selected_clients(i)) + "'," + CStr(user_id)
                    gdb.executesql(sql, certificate)
                Next
            Catch ex As Exception
                Dim oerr As New bc_cs_error_log("bc_om_selected_clients_for_doc", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message)
            End Try
        End Sub
    End Class
    <Serializable> Public Class bc_om_client_contacts
        Inherits bc_cs_soap_base_class
        Public search_results As List(Of bc_om_client_contact)
        Public all As Boolean = False
        Public search_term As String
        Public doc_id As Long
        Public Overrides Sub process_object()
            Select Case tmode
                Case bc_cs_soap_base_class.tREAD
                    db_read()
            End Select
        End Sub
        Public Sub db_read()
            Try
                Dim gdb As New bc_cs_db_services
                Dim fs As New bc_cs_string_services(search_term)
                search_term = fs.delimit_apostrophies
                Dim res As Object
                res = gdb.executesql("exec dbo.bc_core_get_all_client_contacts " + CStr(doc_id) + "," + CStr(all) + ",'" + search_term + "'", certificate)
                search_results = New List(Of bc_om_client_contact)
                Dim cc As bc_om_client_contact
                For i = 0 To UBound(res, 2)
                    cc = New bc_om_client_contact
                    cc.client_id = res(0, i)
                    cc.name = res(1, i)
                    cc.organisation = res(2, i)
                    cc.sent = res(3, i)
                    search_results.Add(cc)
                Next

            Catch ex As Exception
                Dim oerr As New bc_cs_error_log("bc_om_client_contacts", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message)
            End Try
        End Sub
    End Class

    <Serializable> Public Class bc_om_client_contact
        Inherits bc_cs_soap_base_class
        Public client_id As String
        Public name As String
        Public organisation As String
        Public sent As String
        Public from As String
        Public status As String
        Public status_change_date As DateTime
        Public list_add_date As DateTime
        Public comment As String
        Public add_mode As LIST_ADD_MODE
        Public doc_Id As Long
        Public merged_from_pub_type As String
        Public modify_mode As CLIENT_CONTACT_MODIFY_MODE
        Public Overrides Sub process_object()
            Select Case tmode
                Case bc_cs_soap_base_class.tWRITE
                    db_write()
            End Select
        End Sub
        Public Sub db_write()
            Dim db As New db_client_contact
            Dim user_id As Long
            If bc_cs_central_settings.server_flag = 0 Then
                certificate = New bc_cs_security.certificate
                certificate.user_id = bc_cs_central_settings.logged_on_user_id
            Else
                user_id = certificate.user_id
            End If

            Select Case modify_mode
                Case CLIENT_CONTACT_MODIFY_MODE.RESEND
                    db.resend_client_contact(doc_Id, client_id, certificate)
                Case CLIENT_CONTACT_MODIFY_MODE.REMOVE
                    db.rem_client_contact(doc_Id, client_id, certificate)
            End Select
        End Sub
        Public Enum CLIENT_CONTACT_MODIFY_MODE
            ADD = 0
            REMOVE = 1
            RESEND = 2
        End Enum
        Class db_client_contact
            Dim gdb As New bc_cs_db_services
            'Friend Sub add_client_contact(doc_id As Long, client_id As Long, ByRef certificate As bc_cs_security.certificate)
            '    Dim sql As String
            '    sql = "exec dbo.bc_core_add_recipient " + CStr(doc_id) + "," + CStr(client_id)
            '    gdb.executesql(sql, certificate)
            'End Sub
            Friend Sub rem_client_contact(doc_id As Long, client_id As String, ByRef certificate As bc_cs_security.certificate)
                Dim sql As String
                sql = "exec dbo.bc_core_rem_recipient " + CStr(doc_id) + ",'" + CStr(client_id) + "'," + CStr(certificate.user_id)

                gdb.executesql(sql, certificate)
            End Sub
            Friend Sub resend_client_contact(doc_id As Long, client_id As String, ByRef certificate As bc_cs_security.certificate)
                Dim sql As String
                sql = "exec dbo.bc_core_re_resend_recipient " + CStr(doc_id) + ",'" + CStr(client_id) + "'," + CStr(certificate.user_id)

                gdb.executesql(sql, certificate)
            End Sub
        End Class
    End Class
    Public entity_Id As Long
    Public operation As DIST_OPERATION
    Public Enum LIST_ADD_MODE
        PROFILE_MATCH = 0
        MANUAL = 1
        MERGE = 2
    End Enum
    Public Enum DIST_OPERATION
        LOAD_DATA = 0
        EMAIL_PREVIEW = 1
        DISTRIBUTION_LIST = 2
        DISTRIBUTE = 3
        LOAD_RECIPIENTS = 4
        LOAD_MAIL_LIST = 5
        CANCEL_DISTRIBUTION = 6
        DISTRIBUTE_NOW = 7
        MERGE_LIST = 8
        MAIL_LIST_CLASSES = 9
    End Enum

    Public Overrides Sub process_object()
        Select Case tmode
            Case tREAD
                db_read()
        End Select
    End Sub
    Public Sub db_read()

        Select Case operation
            Case DIST_OPERATION.LOAD_DATA
                load_data()
            Case DIST_OPERATION.LOAD_RECIPIENTS
                load_recipients()
            Case DIST_OPERATION.EMAIL_PREVIEW
                set_generate_emaiL_preview()
            Case DIST_OPERATION.DISTRIBUTION_LIST
                set_generate_distribution_list()
            Case DIST_OPERATION.DISTRIBUTE
                set_distribute(0)
            Case DIST_OPERATION.DISTRIBUTE_NOW
                set_distribute(1, resend_channel_id)
            Case DIST_OPERATION.CANCEL_DISTRIBUTION
                cancel_distribute()
            Case DIST_OPERATION.MERGE_LIST
                merge_list()
            Case DIST_OPERATION.MAIL_LIST_CLASSES
                get_mail_list_classes()
        End Select
    End Sub
    Sub get_mail_list_classes()
        Try
            mail_list_classes.Clear()
            Dim db As New bc_om_distribution_db
            Dim res As Object
            res = db.db_get_mail_list_classes(certificate)
            Dim mclass As bc_om_entity_class
            For i = 0 To UBound(res, 2)
                mclass = New bc_om_entity_class
                mclass.class_id = res(0, i)
                mclass.class_name = res(1, i)
                mail_list_classes.Add(mclass)
            Next
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_distribution", "get_mail_list_classes", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Public Sub load_recipients()
        Try
            Dim db As New bc_om_distribution_db
            Dim res As Object
            res = db.get_recipient_list(doc_id, channel_id, certificate)
            Dim c As bc_om_distribution.bc_om_client_contact
            For i = 0 To Me.channels.Count - 1
                If Me.channels(i).channel_id = channel_id Then
                    Me.channels(i).recipients.Clear()
                    If IsArray(res) Then
                        For j = 0 To UBound(res, 2)
                            c = New bc_om_distribution.bc_om_client_contact
                            c.client_id = res(0, j)
                            c.name = res(1, j)
                            c.organisation = res(2, j)
                            c.sent = res(3, j)
                            c.from = res(4, j)
                            c.status = res(5, j)
                            c.status_change_date = res(6, j)
                            c.comment = res(7, j)
                            c.add_mode = res(8, j)
                            c.list_add_date = res(9, j)
                            c.merged_from_pub_type = res(10, j)


                            Me.channels(i).recipients.Add(c)
                        Next
                    End If
                    Exit For
                End If
            Next

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_distribution", "load_recipients", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Sub
    Public Sub load_data()
        Dim db As New bc_om_distribution_db
        REM get base data for distribution
        '          select coalesce(ds.distribution_status,1),coalesce(ds.status_change_date,'1-1-1900'),[automatic], doc_date,p.channel_id,c.name,c.targeted,
        'coalesce(cs.distribution_status,1), coalesce(cs.status_change_date,'1-1-1900'), coalesce(cs.err_msg,''), 
        'coalesce(distribution_list_generated,0), coalesce(distribution_list_generated_date,'1-1-1900'),
        'coalesce(preview_generated_date,'1-1-1900')
        '   from document_table d
        'inner join bc_core_pub_type_distribution_channel p on p.pub_type_id=d.doc_pub_type_id
        'inner join bc_core_distribution_channels c on c.channel_id=p.channel_id
        'left outer join bc_core_reach_doc_status ds on ds.doc_id=d.doc_Id
        'left outer join bc_core_reach_channel_status_for_doc cs on cs.doc_id=d.doc_id and cs.channel_id=c.channel_id
        'where d.doc_Id=@doc_id

        Try
            Dim res As Object
            res = db.get_distribution_details(doc_id, certificate)
            channels = New List(Of bc_om_distribution.bc_om_doc_distribution_channel)
            Dim c As bc_om_distribution.bc_om_doc_distribution_channel
            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    c = New bc_om_distribution.bc_om_doc_distribution_channel
                    If i = 0 Then
                        Me.status = res(0, 0)
                        Me.stage_change_date = res(1, 0)
                        If res(2, 0) = True Then
                            Me.distribution_type = reach_distribution_type.Automatic
                        Else
                            Me.distribution_type = reach_distribution_type.Manual
                        End If
                        Me.distribution_date = res(3, 0)
                    End If
                    c.channel_id = res(4, i)
                    c.name = res(5, i)
                    c.targetted = res(6, i)
                    c.status = res(7, i)

                    c.status_change_date = res(8, i)
                    c.comment = res(9, i)
                    If c.targetted = True Then
                        c.list_generated = res(10, i)
                        c.list_date = res(11, i)
                        c.preview_status = res(12, i)
                        c.preview_date = res(13, i)
                        c.email_url = res(14, i)
                        c.email_title = res(15, i)
                        c.email_body = res(16, i)
                    End If
                    c.generate_list = res(17, i)
                    If c.generate_list = True Then
                        c.list_generated = res(10, i)
                        c.list_date = res(11, i)
                        c.preview_status = res(12, i)
                        c.preview_date = res(13, i)
                        c.email_url = res(14, i)
                        c.email_title = res(15, i)
                        Dim fs As New bc_cs_file_transfer_services

                        If fs.check_document_exists(bc_cs_central_settings.central_repos_path + "email_previews\" + CStr(doc_id) + ".html", certificate) Then

                            c.email_body = fs.write_document_to_string(bc_cs_central_settings.central_repos_path + "email_previews\" + CStr(doc_id) + ".html")
                        Else

                            c.email_body = ""
                        End If

                    End If
                    Me.channels.Add(c)
                Next
            End If
            REM history
            Me.history.Clear()
            res = db.get_distribution_history(doc_id, certificate)

            Dim h As bc_om_distribution_history
            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    h = New bc_om_distribution_history
                    h.user = res(0, i)
                    h.da = res(1, i)
                    h.comment = res(2, i)
                    h.msg = res(3, i)
                    Me.history.Add(h)
                Next
            End If


            res = db.get_pub_types(certificate)
            Dim opt As bc_om_pub_type
            Me.pub_types.Clear()
            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    opt = New bc_om_pub_type
                    opt.id = res(0, i)
                    opt.name = res(1, i)
                    Me.pub_types.Add(opt)

                Next
            End If


        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_distribution", "load_data", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Sub
    'Public Sub check_status()
    '    Dim db As New bc_om_distribution_db
    '    Dim poll As Boolean = True
    '    REM load overall status and channel status
    '    Dim res As Object
    '    res = db.get_distribution_status(doc_id, certificate)
    '    If UBound(res, 2) = 0 Then
    '        Me.status = res(0, 0)
    '        Me.stage_change_date = res(1, 0)
    '    Else
    '        Me.status = reach_status_codes.Preparing
    '        Me.stage_change_date = "1-1-0001"
    '    End If
    '    REM channel status

    '    REM history


    'End Sub
    Public Sub set_generate_emaiL_preview()
        Try
            Dim db As New bc_om_distribution_db
            db.set_generate_emaiL_preview(doc_id, entity_Id, certificate)

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_distribution", "set_generate_emaiL_preview", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Sub
    Public Sub set_generate_distribution_list()
        Try
            Dim db As New bc_om_distribution_db
            db.set_generate_distribution_list(doc_id, certificate)

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_distribution", "set_generate_distribution_list", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Sub
    Public Sub merge_list()
        Try
            Dim db As New bc_om_distribution_db
            REM write document record
            db.add_merge_doc(merge_from_doc.id, merge_from_doc.pub_type_id, certificate)

            For i = 0 To merge_from_doc.authors.Count - 1
                db.add_merge_doc_author(merge_from_doc.id, merge_from_doc.authors(i).id, certificate)
            Next
            For i = 0 To merge_from_doc.taxonomy.Count - 1
                db.add_merge_doc_entity(merge_from_doc.id, merge_from_doc.taxonomy(i).entity_id, certificate)
            Next

            REM do merge
            db.merge_distribution_list(doc_id, merge_from_doc.id, certificate)

            REM remove doc
            db.remove_merge_doc(merge_from_doc.id, certificate)
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_distribution", "merge_list", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Sub
    Public Sub set_distribute(immediate As Integer, Optional resend_channel_id As Integer = 0)
        Try
            Dim db As New bc_om_distribution_db
            db.set_distribute(doc_id, immediate, resend_channel_id, certificate)

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_distribution", "set_distribute", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Sub
    Public Sub cancel_distribute()
        Try
            Dim db As New bc_om_distribution_db
            db.cancel_distribute(doc_id, certificate)

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_distribution", "cancel_distribute", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Sub
    Class bc_om_distribution_db
        Dim gdb As New bc_cs_db_services
        Public Function db_get_mail_list_classes(ByRef certificate As bc_cs_security.certificate) As Object
            db_get_mail_list_classes = gdb.executesql("exec dbo.bc_custom_get_mail_list_classes ", certificate)

        End Function

        Public Function get_distribution_history(doc_id As Long, ByRef certificate As bc_cs_security.certificate)
            get_distribution_history = gdb.executesql("exec dbo.bc_core_re_get_doc_history " + CStr(doc_id), certificate)

        End Function
        Public Function get_pub_types(ByRef certificate As bc_cs_security.certificate)
            Dim sql As String
            sql = "exec dbo.bc_core_re_admin_pub_types"
            get_pub_types = gdb.executesql(sql, certificate)
        End Function
        Sub set_generate_emaiL_preview(doc_Id As Long, entity_id As Long, ByRef certificate As bc_cs_security.certificate)
            Dim sql As String
            If bc_cs_central_settings.server_flag = 0 Then
                sql = "exec dbo.bc_core_re_request_email_preview " + CStr(doc_Id) + "," + CStr(bc_cs_central_settings.logged_on_user_id)

            Else
                sql = "exec dbo.bc_core_re_request_email_preview " + CStr(doc_Id) + "," + CStr(certificate.user_id)
            End If
            gdb.executesql(sql, certificate)
        End Sub
        Sub set_generate_distribution_list(doc_Id As Long, ByRef certificate As bc_cs_security.certificate)
            Dim sql As String
            ' sql = "exec dbo.bc_core_set_generate_distribution_list " + CStr(doc_Id)
            If bc_cs_central_settings.server_flag = 0 Then
                sql = "exec dbo.bc_core_re_generate_recipients_for_doc " + CStr(doc_Id) + "," + CStr(bc_cs_central_settings.logged_on_user_id)
            Else
                sql = "exec dbo.bc_core_re_generate_recipients_for_doc " + CStr(doc_Id) + "," + CStr(certificate.user_id)
            End If
            gdb.executesql(sql, certificate)
        End Sub
        Sub add_merge_doc(doc_id As Long, pub_type_id As Long, ByRef certificate As bc_cs_security.certificate)
            gdb.executesql("exec dbo.bc_core_add_merge_doc " + CStr(doc_id) + "," + CStr(pub_type_id), certificate)
        End Sub
        Sub add_merge_doc_author(doc_id As Long, user_id As Long, ByRef certificate As bc_cs_security.certificate)
            gdb.executesql("exec dbo.bc_core_add_merge_doc_author " + CStr(doc_id) + "," + CStr(user_id), certificate)

        End Sub
        Sub add_merge_doc_entity(doc_id As Long, entity_id As Long, ByRef certificate As bc_cs_security.certificate)
            gdb.executesql("exec dbo.bc_core_add_merge_doc_entity " + CStr(doc_id) + "," + CStr(entity_id), certificate)

        End Sub
        Sub remove_merge_doc(doc_id As Long, ByRef certificate As bc_cs_security.certificate)
            gdb.executesql("exec dbo.bc_core_remove_merge_doc " + CStr(doc_id), certificate)
        End Sub
        Sub merge_distribution_list(doc_id As Long, merge_doc_id As Long, ByRef certificate As bc_cs_security.certificate)
            Dim sql As String
            ' sql = "exec dbo.bc_core_set_generate_distribution_list " + CStr(doc_Id)
            If bc_cs_central_settings.server_flag = 0 Then
                sql = "exec dbo.bc_core_re_merge_recipient_list " + CStr(doc_id) + "," + CStr(merge_doc_id) + "," + CStr(bc_cs_central_settings.logged_on_user_id)
            Else
                sql = "exec dbo.bc_core_re_merge_recipient_list " + CStr(doc_id) + "," + CStr(merge_doc_id) + "," + CStr(certificate.user_id)
            End If
            gdb.executesql(sql, certificate)

        End Sub

        Sub set_distribute(doc_Id As Long, immediate As Integer, resend_channel_id As Integer, ByRef certificate As bc_cs_security.certificate)
            Dim sql As String
            If bc_cs_central_settings.server_flag = 0 Then
                sql = "exec dbo.bc_core_set_distribute " + CStr(doc_Id) + "," + CStr(bc_cs_central_settings.logged_on_user_id) + "," + CStr(immediate) + ",0," + CStr(resend_channel_id)
            Else
                sql = "exec dbo.bc_core_set_distribute " + CStr(doc_Id) + "," + CStr(certificate.user_id) + "," + CStr(immediate) + ",0," + CStr(resend_channel_id)
            End If

            gdb.executesql(sql, certificate)
        End Sub
        Sub cancel_distribute(doc_Id As Long, ByRef certificate As bc_cs_security.certificate)
            Dim sql As String
            If bc_cs_central_settings.server_flag = 0 Then
                sql = "exec dbo.bc_core_cancel_distribute " + CStr(doc_Id) + "," + CStr(bc_cs_central_settings.logged_on_user_id)
            Else
                sql = "exec dbo.bc_core_cancel_distribute " + CStr(doc_Id) + "," + CStr(certificate.user_id)
            End If
            gdb.executesql(sql, certificate)
        End Sub
        Function get_distribution_status(doc_Id As Long, ByRef certificate As bc_cs_security.certificate)
            Dim sql As String
            sql = "exec dbo.bc_core_get_distribution_status " + CStr(doc_Id)
            get_distribution_status = gdb.executesql(sql, certificate)
        End Function
        Function get_distribution_details(doc_Id As Long, ByRef certificate As bc_cs_security.certificate)
            Dim sql As String
            sql = "exec dbo.bc_core_get_distribution_details " + CStr(doc_Id)
            get_distribution_details = gdb.executesql(sql, certificate)
        End Function
        Function get_recipient_list(doc_id As Long, channel_id As Integer, ByRef certificate As bc_cs_security.certificate)
            Dim sql As String
            sql = "exec dbo.bc_core_get_recipients " + CStr(doc_id) + "," + CStr(channel_id)
            get_recipient_list = gdb.executesql(sql, certificate)
        End Function
        Sub add_recipient(doc_id As Long, channel_id As Integer, client_id As Long, ByRef certificate As bc_cs_security.certificate)
            Dim sql As String
            sql = "exec dbo.bc_core_add_recipient " + CStr(doc_id) + "," + CStr(channel_id) + "," + CStr(client_id)
            gdb.executesql(sql, certificate)
        End Sub
        Sub remrecipient(doc_id As Long, channel_id As Integer, client_id As Long, ByRef certificate As bc_cs_security.certificate)
            Dim sql As String
            sql = "exec dbo.bc_core_rem_recipient " + CStr(doc_id) + "," + CStr(channel_id) + "," + CStr(client_id)
            gdb.executesql(sql, certificate)
        End Sub
    End Class
End Class
