Imports BlueCurve.Core.CS
Imports System.IO
Imports System.Xml.Serialization
Imports System.Web.Script.Serialization
Imports System.Text
<Serializable> Public Class bc_om_get_support_template_info
    Inherits bc_cs_soap_base_class
    Public doc_id As String
    Public title As String
    Public template As String
    Public Overrides Sub process_object()
        If tmode = bc_cs_soap_base_class.tREAD Then
            db_read()
        End If
    End Sub
    Public Sub db_read()
        Try
            Dim db As New bc_cs_db_services
            Dim res As Object
            Dim user_id As Long
            If bc_cs_central_settings.server_flag = 0 Then
                user_id = bc_cs_central_settings.logged_on_user_id
            Else
                user_id = certificate.user_id

            End If
            res = db.executesql("exec dbo.bc_core_get_support_doc_template " + CStr(doc_id) + "," + CStr(user_id), certificate)
            If IsArray(res) Then
                If UBound(res, 2) = 0 Then
                    title = res(0, 0)
                    template = res(1, 0)
                End If
            End If


        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_get_support_template_info", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)

        End Try
    End Sub
End Class
    <Serializable> Public Class bc_om_step_attribute
        Public date_from As DateTime
        Public date_to As DateTime
        Public value As String
    End Class
    '<Serializable> Public Class bc_om_step_attributes
    '    Inherits bc_cs_soap_base_class
    '    Public entity_id As Long
    '    Public attribute_id As Long
    '    Public stage_id As Long = 8
    '    Public contributor_id As Long = 1
    '    Public steps As New List(Of bc_om_step_attribute)

    '    Public Overrides Sub process_object()
    '        If tmode = bc_cs_soap_base_class.tREAD Then
    '            db_read()
    '        ElseIf tmode = bc_cs_soap_base_class.tWRITE Then
    '            db_write()
    '        End If
    '    End Sub
    '    Public Sub db_read()
    '        Try
    '            Dim res As Object
    '            Dim db As New bc_cs_db_services
    '            Dim sa As bc_om_step_attribute
    '            res = db.executesql("exec dbo.bc_core_cp_get_step_data " + CStr(entity_id) + "," + CStr(attribute_id) + "," + CStr(stage_id) + "," + CStr(contributor_id), certificate)
    '            If IsArray(res) Then
    '                For i = 0 To UBound(res, 2)
    '                    sa = New bc_om_step_attribute
    '                    sa.value = res(0, i)
    '                    sa.date_from = res(1, i)
    '                    sa.date_to = res(2, i)
    '                    steps.Add(sa)
    '                Next
    '            End If

    '        Catch ex As Exception
    '            Dim oerr As New bc_cs_error_log("bc_om_step_attributes", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
    '        End Try
    '    End Sub
    '    Public Sub db_write()
    '        Try
    '            Dim db As New bc_cs_db_services
    '            Dim param As New bc_cs_db_services.bc_cs_sql_parameter
    '            Dim params As New List(Of bc_cs_db_services.bc_cs_sql_parameter)

    '            db.executesql("exec dbo.bc_core_cp_delete_step_data " + CStr(entity_id) + "," + CStr(attribute_id) + "," + CStr(stage_id) + "," + CStr(contributor_id), certificate)

    '            For i = 0 To steps.Count - 1
    '                param = New bc_cs_db_services.bc_cs_sql_parameter
    '                param.name = "entity_id"
    '                param.value = entity_id
    '                params.Add(param)
    '                param = New bc_cs_db_services.bc_cs_sql_parameter
    '                param.name = "attribute_id"
    '                param.value = attribute_id
    '                params.Add(param)
    '                param = New bc_cs_db_services.bc_cs_sql_parameter
    '                param.name = "stage_id"
    '                param.value = stage_id
    '                params.Add(param)
    '                param = New bc_cs_db_services.bc_cs_sql_parameter
    '                param.name = "contributor_id"
    '                param.value = contributor_id
    '                params.Add(param)
    '                param = New bc_cs_db_services.bc_cs_sql_parameter
    '                param.name = "value"
    '                param.value = steps(i).value
    '                params.Add(param)
    '                param = New bc_cs_db_services.bc_cs_sql_parameter
    '                param.name = "date_from"
    '                param.value = steps(i).date_from
    '                params.Add(param)
    '                param = New bc_cs_db_services.bc_cs_sql_parameter
    '                param.name = "date_from"
    '                param.value = steps(i).date_from
    '                params.Add(param)
    '            Next
    '            If params.Count > 0 Then
    '                db.executesql_with_parameters("exec dbo.bc_core_cp_insert_step_data", params, certificate)
    '            End If

    '        Catch ex As Exception
    '            Dim oerr As New bc_cs_error_log("bc_om_step_attributes", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
    '        End Try
    '    End Sub
    'End Class
    <Serializable> Public Class bc_om_doc_audit_record

        Public area As String
        Public desc As String
        Public user As String
        Public udate As DateTime
        Public stage As String
    End Class
    <Serializable> Public Class bc_om_doc_audit
        Inherits bc_cs_soap_base_class
        Public doc_id As Long
        Public audit As New List(Of bc_om_doc_audit_record)
        Public Overrides Sub process_object()
            If tmode = bc_cs_soap_base_class.tREAD Then
                db_read()
            End If
        End Sub
        Public Sub db_read()
            Try
                Dim res As Object
                Dim db As New bc_cs_db_services
                Dim ar As bc_om_doc_audit_record

                res = db.executesql("exec dbo.bc_core_doc_audit_info " + CStr(doc_id), certificate)
                If IsArray(res) Then
                    For i = 0 To UBound(res, 2)
                        ar = New bc_om_doc_audit_record
                        ar.area = res(0, i)
                        ar.desc = res(1, i)
                        ar.user = res(3, i)
                        ar.udate = res(4, i)
                        ar.stage = res(5, i)
                        audit.Add(ar)
                    Next
                End If


            Catch ex As Exception
                Dim oerr As New bc_cs_error_log("bc_om_doc_audit", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)

            End Try
        End Sub
    End Class




    <Serializable> Public Class bc_om_next_stage_users
        Inherits bc_cs_soap_base_class
        Public doc_id As Long
        Public pub_type_id As Long
        Public next_stage As Integer
        Public users As New List(Of bc_om_user)
        Public Overrides Sub process_object()
            If tmode = bc_cs_soap_base_class.tREAD Then
                db_read()
            End If
        End Sub
        Public Sub db_read()
            Try
                Dim gdb As New bc_cs_db_services
                Dim res As Object
                users.Clear()
                Dim user As New bc_om_user
                res = gdb.executesql("exec dbo.bc_core_get_stage_users " + CStr(doc_id) + "," + CStr(pub_type_id) + "," + CStr(next_stage), certificate)
                If IsArray(res) Then
                    For i = 0 To UBound(res, 2)
                        user = New bc_om_user
                        user.id = res(0, i)
                        user.user_name = res(1, i)
                        users.Add(user)
                    Next
                End If



            Catch ex As Exception

            End Try
        End Sub
    End Class


    <Serializable> Public Class bc_om_dependent_taxonomy
        Inherits bc_cs_soap_base_class
        Public target_class_id As Long
        Public dependent_entities As New List(Of Long)
        Public results As New List(Of bc_as_cat_taxonomy.bc_as_cat_taxononmy_item)
        Public Overrides Sub process_object()
            If tmode = bc_cs_soap_base_class.tREAD Then
                db_read()
            End If
        End Sub
        Public Sub db_read()
            Try
                results.Clear()
                Dim bc As bc_as_cat_taxonomy.bc_as_cat_taxononmy_item
                Dim db As New bc_cs_db_services
                Dim res As Object
                Dim sql As String
                Dim found As Boolean
                For i = 0 To dependent_entities.Count - 1
                    sql = "exec dbo.bc_custom_get_dependent_entities " + CStr(target_class_id) + "," + CStr(dependent_entities(i))
                    res = db.executesql(sql, certificate)
                    If IsArray(res) Then
                        For j = 0 To UBound(res, 2)
                            found = False
                            bc = New bc_as_cat_taxonomy.bc_as_cat_taxononmy_item(res(1, j), res(0, j), False)
                            REM if not duplicate add to list
                            For m = 0 To results.Count - 1
                                If results(m).id = bc.id Then
                                    found = True
                                    Exit For
                                End If
                            Next

                            If found = False Then
                                results.Add(bc)
                            End If
                        Next
                    End If
                Next


                Dim fss = From t In results Select t Order By t.display_name

                results = fss.ToList

            Catch ex As Exception
                Dim oerr As New bc_cs_error_log("bc_om_dependent_taxonomy", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            End Try
        End Sub
    End Class


    <Serializable> Public Class bc_om_real_time_search
        Inherits bc_cs_soap_base_class
        Public class_id As Long
        Public search_text As String
        Public filter_attribute_id As Long = 0
        Public mine As Boolean = False
        Public inactive As Boolean = False
        Public results As New List(Of bc_om_entity)
        Public resultsids As New List(Of Long)
        Public results_as_ids = True

        Public Overrides Sub process_object()
            Select Case tmode
                Case tREAD
                    db_read()
            End Select
        End Sub
        Public Sub db_read()
            Try
                results.Clear()
                resultsids.Clear()

                Dim fs As New bc_cs_string_services(search_text)
                search_text = fs.delimit_apostrophies()

                Dim oparams As New List(Of bc_cs_db_services.bc_cs_sql_parameter)
                Dim oparam As New bc_cs_db_services.bc_cs_sql_parameter

                oparam.name = "class_id"
                oparam.value = class_id
                oparams.Add(oparam)

                oparam = New bc_cs_db_services.bc_cs_sql_parameter
                oparam.name = "filter_attribute_id"
                oparam.value = filter_attribute_id
                oparams.Add(oparam)

                oparam = New bc_cs_db_services.bc_cs_sql_parameter
                oparam.name = "mine"
                oparam.value = mine
                oparams.Add(oparam)

                oparam = New bc_cs_db_services.bc_cs_sql_parameter
                oparam.name = "inactive"
                oparam.value = inactive
                oparams.Add(oparam)


                oparam = New bc_cs_db_services.bc_cs_sql_parameter
                oparam.name = "user_id"
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    oparam.value = bc_cs_central_settings.logged_on_user_id
                Else
                    oparam.value = certificate.user_id
                End If
                oparam.value = inactive
                oparams.Add(oparam)

                oparam = New bc_cs_db_services.bc_cs_sql_parameter
                oparam.name = "search_text"
                oparam.value = search_text
                oparams.Add(oparam)

                Dim db As New bc_cs_db_services
                Dim res As Object
                Dim oent As bc_om_entity

                res = db.executesql_with_parameters("dbo.bc_core_real_time_search", oparams, certificate)

                If IsArray(res) Then
                    For i = 0 To UBound(res, 2)
                        If results_as_ids = True Then
                            resultsids.Add(res(0, i))
                        Else
                            oent = New bc_om_entity
                            oent.id = res(0, i)
                            oent.name = res(1, i)
                            oent.class_id = res(2, i)
                            oent.class_name = res(3, i)
                            oent.inactive = res(4, i)

                            results.Add(oent)
                        End If
                    Next
                End If




            Catch ex As Exception
                Dim db_err As New bc_cs_error_log("bc_om_real_time_search", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)


            End Try
        End Sub
    End Class




    <Serializable> Public Class bc_om_copy_disclosures
        Inherits bc_cs_soap_base_class
        Public doc_id As Long
        Public class_id As Long

        Public Overrides Sub process_object()


            Select Case tmode
                Case tREAD
                    db_read()
            End Select
        End Sub
        Public Sub db_read()
            Dim gdb As New bc_cs_db_services
            Dim sql As String
            Dim res As Object
            sql = "exec dbo.bc_get_disclosure_class " + CStr(doc_id)
            res = gdb.executesql(sql, certificate)
            If IsArray(res) Then
                class_id = res(0, 0)
            End If


        End Sub

    End Class

    <Serializable> Public Class bc_om_get_guid_for_html_product
        Inherits bc_cs_soap_base_class
        Public entity_id As Long
        Public pub_type_id As Long
        Public doc_id As Long
        Public user_id As Long
        Public sguid As String
        Public Overrides Sub process_object()


            Select Case tmode
                Case tREAD
                    db_read()
            End Select
        End Sub
        Public Sub db_read()
            Try
                Dim db As New bc_cs_db_services
                Dim res As Object
                res = db.executesql("exec dbo.bc_core_web_products_set_guid " + CStr(entity_id) + "," + CStr(pub_type_id) + "," + CStr(user_id) + "," + CStr(doc_id), certificate)
                If IsArray(res) Then
                    sguid = res(0, 0)
                End If
            Catch ex As Exception
                Dim oerr As New bc_cs_error_log("bc_om_get_guid_for_html_product", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message)

            End Try
        End Sub
    End Class

    <Serializable> Public Class bc_om_get_preview_url
        Inherits bc_cs_soap_base_class
        Public doc_id As Long
        Public url As String
        Public Overrides Sub process_object()
            Select Case tmode
                Case tREAD
                    db_read()
            End Select
        End Sub
        Public Sub db_read()
            Try
                Dim db As New bc_cs_db_services
                Dim sql As String
                Dim res As Object
                sql = "exec dbo.bc_core_instate_get_preview_url " + CStr(doc_id)
                res = db.executesql(sql, certificate)
                If IsArray(res) Then
                    If UBound(res, 2) = 0 Then
                        url = res(0, 0)

                    End If
                End If
            Catch ex As Exception
                Dim oerr As New bc_cs_error_log("bc_om_get_preview_url", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            End Try
        End Sub
    End Class
    <Serializable> Public Class bc_om_set_attribute_values
        Inherits bc_cs_soap_base_class
        Public attributes As New List(Of bc_om_set_attribute_value)
        Public doc_id As Long

        Public Overrides Sub process_object()
            Select Case tmode
                Case tWRITE
                    db_write()
            End Select

        End Sub
        Public Sub db_write()
            Try
                Dim db As New db_bc_om_set_attribute_values
                For i = 0 To attributes.Count - 1
                    With attributes(i)
                        db.update_attribute_value(doc_id, .entity_id, .attribute_id, .submission_code, .value, .meth, .risk, certificate)
                    End With
                Next
            Catch ex As Exception
                Dim oerr As New bc_cs_error_log("bc_om_set_attribute_values", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message)
            End Try
        End Sub
        Private Class db_bc_om_set_attribute_values
            Dim gdb As New bc_cs_db_services
            Public Sub update_attribute_value(doc_id As Long, entity_id As Long, attribute_id As Long, submission_code As bc_om_attributes_for_doc.ATT_SUB_CODE, value As String, meth As String, risk As String, ByRef certificate As bc_cs_security.certificate)
                Dim sql As String
                Dim fs As New bc_cs_string_services(value)
                value = fs.delimit_apostrophies
                fs = New bc_cs_string_services(meth)
                meth = fs.delimit_apostrophies
                fs = New bc_cs_string_services(risk)
                risk = fs.delimit_apostrophies


                If bc_cs_central_settings.server_flag = 0 Then
                    sql = "exec dbo.bc_core_ent_att_update_value " + CStr(doc_id) + "," + CStr(entity_id) + "," + CStr(attribute_id) + "," + CStr(submission_code) + ",'" + value + "','" + meth + "','" + risk + "'," + CStr(bc_cs_central_settings.logged_on_user_id)
                Else
                    sql = "exec dbo.bc_core_ent_att_update_value " + CStr(doc_id) + "," + CStr(entity_id) + "," + CStr(attribute_id) + "," + CStr(submission_code) + ",'" + value + "','" + meth + "','" + risk + "'," + CStr(certificate.user_id)
                End If
                gdb.executesql(sql, certificate)
            End Sub
        End Class
    End Class
    <Serializable> Public Class bc_om_set_attribute_value
        Public entity_id As Long
        Public attribute_id As Long
        Public submission_code As bc_om_attributes_for_doc.ATT_SUB_CODE
        Public value As String
        Public meth As String
        Public risk As String
        Public Sub New()

        End Sub
    End Class
    <Serializable> Public Class bc_om_attributes_for_doc
        Inherits bc_cs_soap_base_class

        Public doc As bc_om_document
        Public parent_entities As New List(Of bc_om_parent_entity)
        Public lookup_lists As New List(Of bc_om_attribute)
        Public save_msg As String = ""
        Public rec_classes As New List(Of bc_om_rec_classes)


        <Serializable> Public Class bc_om_parent_entity
            Inherits bc_om_entity_base_class
            Public child_entities As New List(Of bc_om_child_entity)
        End Class
        <Serializable> Public Class bc_om_child_entity
            Inherits bc_om_entity_base_class
        End Class
        <Serializable> Public Class bc_om_rec_classes
            Public rec_name As String
            Public class_name As String
            Public display As Boolean

        End Class
        <Serializable> Public Class bc_om_entity_attribute_value
            Public attribute_id As Long
            Public attribute_name As String
            Public value As String
            Public date_changed As Date
            Public user_changed As String
            Public data_type As ATT_TYPE
            Public submission_code As ATT_SUB_CODE
            Public is_lookup As Boolean
            Public lookup_sql As String
            Public mandatory As Boolean
            Public auditted As Boolean
            Public changed As Boolean = False
            Public published_value As String
            Public published_date_changed As Date
            Public published_user_changed As String
            Public workflowed As Boolean
            Public dval_str As String
            Public pval_str As String
            Public meth As String
            Public risk As String
            Public pmeth As String
            Public prisk As String
        End Class
        Public Enum ATT_SUB_CODE
            VALUE = 1
            TIMESERIES = 2
        End Enum
        Public Enum ATT_TYPE
            TEXT = 1
            NUMBER = 2
            BOOL = 3
        End Enum

        <Serializable> Public MustInherit Class bc_om_entity_base_class
            Public attributes As New List(Of bc_om_entity_attribute_value)
            Public entity As New bc_om_taxonomy
            Public attributes_in_user As New List(Of bc_om_attribute)
            Public extended_name1 As String
            Public extended_name2 As String
            Public extended_name3 As String
            Public extended_name4 As String

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
                Dim db As New db_bc_om_attributes_for_doc
                Dim cres As Object
                Dim pe As bc_om_parent_entity
                Dim ce As bc_om_child_entity

                REM go thorugh each entity in the classification
                For i = 0 To doc.taxonomy.Count - 1
                    pe = New bc_om_parent_entity
                    pe.entity = doc.taxonomy(i)

                    cres = db.get_extended_entity_name(doc.taxonomy(i).entity_id, certificate)
                    If IsArray(cres) Then
                        pe.extended_name4 = cres(0, 0)

                    End If
                    get_attributes_for_entity(doc.id, pe, certificate)

                    cres = db.get_child_entities(doc.taxonomy(i).entity_id, certificate)
                    If IsArray(cres) Then
                        For j = 0 To UBound(cres, 2)
                            ce = New bc_om_child_entity
                            ce.entity.entity_id = cres(0, j)
                            ce.entity.name = cres(1, j)
                            ce.entity.class_id = cres(2, j)
                            ce.entity.class_name = cres(3, j)
                            ce.extended_name1 = cres(4, j)
                            ce.extended_name2 = cres(5, j)
                            ce.extended_name3 = cres(6, j)

                            get_attributes_for_entity(doc.id, ce, certificate)
                            If ce.attributes.Count > 0 Then
                                pe.child_entities.Add(ce)
                            End If
                        Next
                    End If
                    If pe.attributes.Count > 0 Then
                        parent_entities.Add(pe)
                    End If
                Next
                REM remove entities of classes that have no  attributes


                REM now go throgh unique attributes that have lookups and get lookup list
                Dim found As Boolean = False
                Dim la As bc_om_attribute

                For i = 0 To parent_entities.Count - 1

                    For j = 0 To parent_entities(i).attributes.Count - 1
                        If parent_entities(i).attributes(j).is_lookup = True Then
                            found = False
                            For k = 0 To lookup_lists.Count - 1
                                If lookup_lists(k).attribute_id = parent_entities(i).attributes(j).attribute_id Then
                                    found = True
                                    Exit For
                                End If
                            Next

                            If found = False Then
                                la = New bc_om_attribute
                                la.attribute_id = parent_entities(i).attributes(j).attribute_id
                                la.lookup_sql = parent_entities(i).attributes(j).lookup_sql
                                la.db_read()
                                Me.lookup_lists.Add(la)
                            End If
                        End If
                    Next
                    For m = 0 To parent_entities(i).child_entities.Count - 1
                        With parent_entities(i).child_entities(m)
                            For j = 0 To .attributes.Count - 1
                                If .attributes(j).is_lookup = True Then
                                    found = False
                                    For k = 0 To lookup_lists.Count - 1
                                        If lookup_lists(k).attribute_id = .attributes(j).attribute_id Then
                                            found = True
                                            Exit For
                                        End If
                                    Next

                                    If found = False Then
                                        la = New bc_om_attribute
                                        la.attribute_id = .attributes(j).attribute_id
                                        la.lookup_sql = .attributes(j).lookup_sql
                                        la.db_read()
                                        Me.lookup_lists.Add(la)
                                    End If
                                End If
                            Next
                        End With
                    Next
                Next
                Dim mres
                mres = db.get_save_msg(certificate)
                If IsArray(mres) Then
                    If UBound(mres, 2) > -1 Then
                        save_msg = mres(0, 0)
                    End If
                End If

                REM map rec to claases
                Dim xres
                Dim rc As New bc_om_rec_classes

                xres = db.get_rec_classes(certificate)
                If IsArray(xres) Then
                    For i = 0 To UBound(xres, 2)
                        rc = New bc_om_rec_classes
                        rc.rec_name = xres(0, i)
                        rc.class_name = xres(1, i)
                        rc.display = xres(2, i)
                        Me.rec_classes.Add(rc)
                    Next

                End If

            Catch ex As Exception
                Dim oerr As New bc_cs_error_log("bc_om_attributes_for_doc", "db_readd", bc_cs_error_codes.USER_DEFINED, ex.Message)
            End Try
        End Sub
        Private Function get_attributes_for_entity(doc_id As Long, ByRef entity As bc_om_entity_base_class, ByRef certificate As bc_cs_security.certificate) As Boolean
            get_attributes_for_entity = False
            Try
                Dim db As New db_bc_om_attributes_for_doc
                Dim ares As Object
                Dim results As New List(Of bc_om_entity_attribute_value)
                Dim at As bc_om_entity_attribute_value
                ares = db.get_attributes_for_entity(doc_id, entity.entity.entity_id, certificate)

                If IsArray(ares) Then
                    For i = 0 To UBound(ares, 2)
                        at = New bc_om_entity_attribute_value
                        at.attribute_id = ares(0, i)
                        at.attribute_name = ares(1, i)
                        at.data_type = ares(2, i)
                        at.is_lookup = ares(3, i)
                        at.submission_code = ares(4, i)
                        at.auditted = ares(5, i)
                        at.mandatory = ares(6, i)
                        at.lookup_sql = ares(7, i)
                        at.workflowed = ares(11, i)
                        'If at.workflowed = True Then
                        at.published_value = ares(8, i)
                        If IsDate(ares(9, i)) Then
                            at.published_date_changed = ares(9, i)
                        Else
                            at.published_date_changed = "1-1-1900"
                        End If
                        at.published_user_changed = ares(10, i)
                        at.value = ares(12, i)
                        If IsDate(ares(13, i)) Then
                            at.date_changed = ares(13, i)
                        Else
                            at.date_changed = "1-1-1900"
                        End If
                        at.user_changed = ares(14, i)
                        'Else
                        'at.value = ares(8, i)
                        'If IsDate(ares(9, i)) Then
                        '    at.date_changed = ares(9, i)
                        'Else
                        '    at.date_changed = "1-1-1900"

                        'End If
                        'at.user_changed = ares(10, i)
                        'End If
                        at.dval_str = ares(15, i)

                        at.pval_str = ares(16, i)
                        at.meth = ares(17, i)
                        at.risk = ares(18, i)
                        at.pmeth = ares(19, i)
                        at.prisk = ares(20, i)
                        entity.attributes.Add(at)
                    Next
                End If
                get_attributes_for_entity = True
            Catch ex As Exception
                Dim oerr As New bc_cs_error_log("bc_om_attributes_for_doc", " get_attributes_for_entity", bc_cs_error_codes.USER_DEFINED, ex.Message)

            End Try
        End Function
        Public Sub db_write()
            Try

            Catch ex As Exception
                Dim oerr As New bc_cs_error_log("bc_om_attributes_for_doc", "load_data", bc_cs_error_codes.USER_DEFINED, ex.Message)
            End Try
        End Sub
        Private Class db_bc_om_attributes_for_doc
            Dim gdb As New bc_cs_db_services
            Public Function get_extended_entity_name(entity_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
                Dim sql As String
                sql = "exec dbo.bc_core_att_get_extended_entity_name " + CStr(entity_id)
                get_extended_entity_name = gdb.executesql(sql, certificate)
            End Function
            Public Function get_child_entities(entity_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
                Dim sql As String
                sql = "exec dbo.bc_core_att_get_child_entities " + CStr(entity_id)
                get_child_entities = gdb.executesql(sql, certificate)
            End Function
            Public Function get_attributes_for_entity(doc_id As Long, entity_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
                Dim sql As String
                sql = "exec dbo.bc_core_att_edit_attributes_for_entity " + CStr(entity_id) + "," + CStr(doc_id)
                get_attributes_for_entity = gdb.executesql(sql, certificate)

            End Function
            Public Function get_rec_classes(ByRef certificate As bc_cs_security.certificate) As Object
                Dim sql As String
                sql = "exec dbo.bc_core_get_rec_classes"
                get_rec_classes = gdb.executesql(sql, certificate)

            End Function

            Public Function get_save_msg(ByRef certificate As bc_cs_security.certificate) As Object
                Dim sql As String
                sql = "exec dbo.bc_core_att_get_save_msg"
                get_save_msg = gdb.executesql(sql, certificate)

            End Function
        End Class
    End Class
    <Serializable> Public Class bc_om_stop_documemt
        Inherits bc_cs_soap_base_class
        Public doc_id As Long
        Public mode As DOC_STOP_MODE

        Public Enum DOC_STOP_MODE
            CANCEL = 0
            REJECT = 1
        End Enum
        Public Overrides Sub process_object()
            Select Case tmode
                Case tREAD
                    db_read()
            End Select
        End Sub
        Public Sub db_read()
            Dim db As New db_bc_om_stop_documemt
            db.stop_document(doc_id, mode, certificate)
        End Sub
        Class db_bc_om_stop_documemt
            Dim gdb As New bc_cs_db_services
            Public Function stop_document(doc_id As Long, mode As DOC_STOP_MODE, ByRef certificate As bc_cs_security.certificate) As Object
                Dim sql As String
                If bc_cs_central_settings.server_flag = 0 Then
                    sql = "exec dbo.bc_core_stop_document " + CStr(doc_id) + "," + CStr(bc_cs_central_settings.logged_on_user_id) + "," + CStr(mode)
                Else
                    sql = "exec dbo.bc_core_stop_document " + CStr(doc_id) + "," + CStr(certificate.user_id) + "," + CStr(mode)
                End If
                stop_document = gdb.executesql(sql, certificate)
            End Function

        End Class
    End Class
    <Serializable> Public Class bc_om_attestations_for_doc
        Inherits bc_cs_soap_base_class
        Public doc_id As Long
        Public attestations As New List(Of bc_om_attestation_config)
        Public Overrides Sub process_object()
            Select Case tmode
                Case tREAD
                    db_read()
            End Select
        End Sub
        Public Sub db_read()
            Try
                attestations.Clear()
                Dim db As New db_bc_om_attestations_for_doc
                Dim vres As Object
                vres = db.get_attestations_for_doc(doc_id, certificate)
                If IsArray(vres) Then
                    For i = 0 To UBound(vres, 2)
                        Dim at As New bc_om_attestation_config
                        at.id = vres(0, i)
                        at.submitted_date = vres(1, i)
                        at.pass = vres(2, i)
                        at.questionnaire = vres(3, i)
                        at.group = vres(4, i)
                        at.user_name = vres(5, i)
                        attestations.Add(at)
                    Next
                End If


            Catch ex As Exception
                Dim db_err As New bc_cs_error_log("bc_om_attestations_for_doc", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            End Try
        End Sub
        Class db_bc_om_attestations_for_doc
            Dim gdb As New bc_cs_db_services
            Friend Function get_attestations_for_doc(doc_id As Long, ByRef certificiate As bc_cs_security.certificate)
                Dim sql As String
                sql = "exec dbo.bc_core_get_attestations_for_doc " + CStr(doc_id)
                get_attestations_for_doc = gdb.executesql(sql, certificiate)
            End Function
        End Class
    End Class
    <Serializable> Public Class bc_om_attestation_answers
        Inherits bc_cs_soap_base_class
        Public att_id As Long
        Public responses As New List(Of bc_om_attestation_config.bc_om_attestation_question)
        Public Overrides Sub process_object()
            Select Case tmode
                Case tREAD
                    db_read()
            End Select
        End Sub
        Public Sub db_read()
            Try
                Dim db As New db_bc_om_attestation_answers
                Dim res As Object
                Dim a As bc_om_attestation_config.bc_om_attestation_question

                res = db.get_responses(att_id, certificate)
                If IsArray(res) Then
                    For i = 0 To UBound(res, 2)
                        a = New bc_om_attestation_config.bc_om_attestation_question
                        a.text = res(0, i)
                        a.option_selected_text = res(1, i)
                        a.pass = res(2, i)
                        responses.Add(a)
                    Next
                End If
            Catch ex As Exception
                Dim db_err As New bc_cs_error_log("bc_om_attestation_answers", "db_db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)

            End Try
        End Sub
        Private Class db_bc_om_attestation_answers
            Dim gdb As New bc_cs_db_services
            Public Function get_responses(id As Long, ByRef certificate As bc_cs_security.certificate)
                Dim sql As String
                sql = "exec dbo.bc_core_get_assetation_reponses " + CStr(id)
                get_responses = gdb.executesql(sql, certificate)
            End Function
        End Class
    End Class
    <Serializable> Public Class bc_om_attestation_config
        Inherits bc_cs_soap_base_class
        Public user_id As Long
        Public doc_id As Long
        Public id As Long
        Public group As String
        Public questionnaire As String
        Public group_id As String
        Public questions As New List(Of bc_om_attestation_question)
        Public message As String
        Public pass As Boolean
        Public edit_mode As Boolean
        Public submitted_date As DateTime
        Public user_name As String
    Public behalf_of_user_id As Integer
    Public behalf_of_user_date As Date
    Public behalf_of_user_choices As New List(Of bc_om_user)
        Public Overrides Sub process_object()
            Select Case tmode
                Case tREAD
                    db_read()
                Case tWRITE
                    db_write()
            End Select
        End Sub
        Public Sub db_write()
            Try
                Dim gdb As New db_bc_om_attestation_config
                Dim res As Object
                Dim att_id As Long
            res = gdb.write_attestation_response(Me.doc_id, Me.user_id, Me.group_id, Me.id, pass, Me.behalf_of_user_id, behalf_of_user_date, certificate)
                If IsArray(res) Then
                    If UBound(res, 2) = 0 Then
                        att_id = res(0, 0)
                        For i = 0 To Me.questions.Count - 1
                            gdb.write_attestation_answer(att_id, i + 1, Me.questions(i).text, Me.questions(i).option_selected_text, Me.questions(i).pass, certificate)
                        Next
                    End If
                End If
            Catch ex As Exception
                Dim db_err As New bc_cs_error_log("bc_om_attestation_config", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)

            End Try
        End Sub
        Public Sub db_read()
            Try
                Dim gdb As New db_bc_om_attestation_config
                Dim res As Object

                res = gdb.get_attestation_header(doc_id, user_id, certificate)
                If IsArray(res) Then
                    For i = 0 To UBound(res, 2)
                        id = res(0, 0)
                        group = res(1, 0)
                        questionnaire = res(2, 0)
                        group_id = res(3, 0)
                        edit_mode = res(4, 0)
                        message = res(5, 0)
                        behalf_of_user_id = res(6, 0)
                    behalf_of_user_date = res(7, 0)

                    Next
                End If

                If message <> "" Then
                    Exit Sub
                End If

                res = gdb.get_attestation_qs(id, user_id, doc_id, certificate)
                Dim ores As Object
                Dim q As bc_om_attestation_config.bc_om_attestation_question
                Dim o As bc_om_attestation_config.bc_om_attestation_option

                For i = 0 To UBound(res, 2)
                    q = New bc_om_attestation_config.bc_om_attestation_question
                    q.qid = res(0, i)
                    q.text = res(1, i)
                    q.default_option = res(2, i)

                    ores = gdb.get_attestation_options_for_q(q.qid, certificate)
                    If IsArray(ores) Then
                        For j = 0 To UBound(ores, 2)
                            o = New bc_om_attestation_config.bc_om_attestation_option
                            o.text = ores(0, j)
                            o.pass = ores(1, j)
                            q.options.Add(o)
                        Next
                    End If

                    questions.Add(q)
            Next
            res = gdb.get_attestation_behalf_of_choices(doc_id, certificate)
            Dim bhu As bc_om_user

            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    bhu = New bc_om_user
                    bhu.id = res(0, i)
                    bhu.first_name = res(1, i)
                    bhu.surname = res(2, i)
                    behalf_of_user_choices.Add(bhu)
                Next
            End If

            Catch ex As Exception
                Dim db_err As New bc_cs_error_log("bc_om_attestation_config", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)

            End Try
        End Sub
        Private Class db_bc_om_attestation_config
            Dim gdb As New bc_cs_db_services
        Friend Function get_attestation_behalf_of_choices(doc_id As Long, ByRef certificate As bc_cs_security.certificate)
            Dim sql As String
            sql = "exec dbo.bc_core_get_attestation_behalf_of_choices " + CStr(doc_id)
            get_attestation_behalf_of_choices = gdb.executesql(sql, certificate)
        End Function
        Friend Function get_attestation_header(doc_id As Long, user_id As Long, ByRef certificate As bc_cs_security.certificate)
            Dim sql As String
            sql = "exec dbo.bc_core_get_attestation " + CStr(doc_id) + "," + CStr(user_id)
            get_attestation_header = gdb.executesql(sql, certificate)
        End Function
            Friend Function get_attestation_qs(id As Long, user_id As Long, doc_Id As Long, ByRef certificate As bc_cs_security.certificate)
                Dim sql As String
                sql = "exec dbo.bc_core_get_attestation_qs " + CStr(id) + "," + CStr(user_id) + "," + CStr(doc_Id)
                get_attestation_qs = gdb.executesql(sql, certificate)
            End Function
            Friend Function get_attestation_options_for_q(qid As Long, ByRef certificate As bc_cs_security.certificate)
                Dim sql As String
                sql = "exec dbo.bc_core_get_attestation_options_for_q " + CStr(qid)
                get_attestation_options_for_q = gdb.executesql(sql, certificate)
            End Function
        Friend Function write_attestation_response(doc_id As Long, user_id As Long, group_id As Long, questionnaire_id As Long, pass As Boolean, behalf_of_user_id As Long, behalf_of_user_date As Date, ByRef certificate As bc_cs_security.certificate) As Object
            Dim sqlparams As New List(Of bc_cs_db_services.bc_cs_sql_parameter)
            Dim sqlparam As bc_cs_db_services.bc_cs_sql_parameter
            sqlparam = New bc_cs_db_services.bc_cs_sql_parameter
            sqlparam.name = "doc_id"
            sqlparam.value = doc_id
            sqlparams.Add(sqlparam)
            sqlparam = New bc_cs_db_services.bc_cs_sql_parameter
            sqlparam.name = "user_id"
            sqlparam.value = user_id
            sqlparams.Add(sqlparam)
            sqlparam = New bc_cs_db_services.bc_cs_sql_parameter
            sqlparam.name = "questionnaire_id"
            sqlparam.value = questionnaire_id
            sqlparams.Add(sqlparam)
            sqlparam = New bc_cs_db_services.bc_cs_sql_parameter
            sqlparam.name = "group"
            sqlparam.value = group_id
            sqlparams.Add(sqlparam)
            sqlparam = New bc_cs_db_services.bc_cs_sql_parameter
            sqlparam.name = "pass"
            sqlparam.value = pass
            sqlparams.Add(sqlparam)
            sqlparam = New bc_cs_db_services.bc_cs_sql_parameter
            sqlparam.name = "behalf_of"
            sqlparam.value = behalf_of_user_id
            sqlparams.Add(sqlparam)
            sqlparam = New bc_cs_db_services.bc_cs_sql_parameter
            sqlparam.name = "behalf_of_date"
            sqlparam.value = behalf_of_user_date
            sqlparams.Add(sqlparam)

            write_attestation_response = gdb.executesql_with_parameters("dbo.bc_core_write_attestations_for_doc", sqlparams, certificate)
        End Function
            Friend Sub write_attestation_answer(att_id As Long, ord As Integer, question As String, answer As String, pass As Boolean, ByRef certificate As bc_cs_security.certificate)
                Dim sql As String
                Dim fs As New bc_cs_string_services(question)
                question = fs.delimit_apostrophies
                fs = New bc_cs_string_services(answer)
                answer = fs.delimit_apostrophies

                sql = "exec dbo.bc_core_write_attestation_answer " + CStr(att_id) + "," + CStr(ord) + ",'" + question + "','" + answer + "'," + CStr(pass)
                gdb.executesql(sql, certificate)
            End Sub
        End Class


        <Serializable> Public Class bc_om_attestation_question
            Public qid As Integer
            Public text As String
            Public options As New List(Of bc_om_attestation_option)
            Public default_option As String
            Public chosen_option As Integer
            Public option_selected_text
            Public pass As Boolean
        End Class

        <Serializable> Public Class bc_om_attestation_option
            Public text As String
            Public pass As Boolean = True
        End Class

    End Class
    <Serializable> Public Class bc_as_cat_taxonomy
        Public Sub New(ByVal name As String, ByVal type As Integer, ByVal class_id As Long, mandatory As Boolean, max_number As Integer)
            Me.name = name
            Me.type = type
            Me.Class_id = class_id
            Me.mandatory = mandatory
            Me.max_number = max_number
        End Sub
        Public name As String
        Public type As Integer REM 0 author 1 class 2 disclosures
        Public Class_id As Long
        Public mandatory As Boolean
        Public max_number As Integer
        Public num_set As Integer
        Public dependent_class As New List(Of Long)
        Public items As New List(Of bc_as_cat_taxononmy_item)

        'f
        'Public selected_items As New List(Of bc_as_cat_taxononmy_sel_item)
        <Serializable> Public Class bc_as_cat_taxononmy_item
            Public display_name As String
            Public id As Long
            Public order As Integer
            Public selected As Boolean
            Public searched As Boolean
            Public search_names As New List(Of String)
            Public search_name As String
            Public scanned As Boolean = False
            Public mine As Boolean = False

            Public Sub New(ByVal display_name As String, ByVal id As Long, ByVal selected As Boolean)
                Me.display_name = display_name
                Me.id = id
                Me.selected = selected
            End Sub
        End Class
        Public Class bc_as_cat_taxononmy_sel_item
            Public display_name As String
            Public id As Long
        End Class
    End Class
    <Serializable()> Public Class bc_om_doc_select_params
        Inherits bc_cs_soap_base_class

        Public doc As bc_om_document
        Public pub_type_id As Long
        Public sel_class As New List(Of bc_om_entity_class)
        Public prop_class As New List(Of bc_om_entity_class)
        Public def_prop_entity As New bc_om_entity
        Public def_docs As New List(Of bc_om_document)
        Public analytic_mode As Boolean = False
        Public set_default_pub_type As Boolean = False
        Public set_default_author As Boolean = False
        Public default_range As Integer = -9
        Public is_default_in_days As Boolean = False
        Public Overrides Sub process_object()
            Select Case tmode
                Case bc_cs_soap_base_class.tREAD
                    db_read()
            End Select
        End Sub
        Public Sub db_read()
            Try
                Dim sql As String
                Dim res As Object
                Dim gdb As New bc_cs_db_services
                Dim c As bc_om_entity_class

                sel_class.Clear()
                sql = "exec dbo.bc_core_linked_docs_display_default"
                res = gdb.executesql(sql, certificate)
                If IsArray(res) Then
                    set_default_pub_type = res(0, 0)
                    set_default_author = res(1, 0)
                    default_range = res(2, 0)
                    is_default_in_days = res(3, 0)
                End If


                sql = "exec dbo.bc_core_linked_docs_sel_class "
                res = gdb.executesql(sql, certificate)
                If IsArray(res) Then
                    For i = 0 To UBound(res, 2)
                        c = New bc_om_entity_class
                        c.class_id = res(0, i)
                        c.class_name = res(1, i)
                        sel_class.Add(c)
                    Next
                End If
                prop_class.Clear()
                sql = "exec dbo.bc_core_linked_docs_prop_class "
                res = gdb.executesql(sql, certificate)
                If IsArray(res) Then
                    For i = 0 To UBound(res, 2)
                        c = New bc_om_entity_class
                        c.class_id = res(0, i)
                        c.class_name = res(1, i)
                        prop_class.Add(c)
                    Next
                End If
                sql = "exec dbo.bc_core_get_def_prop_entity " + CStr(doc.entity_id)
                res = gdb.executesql(sql, certificate)
                If IsArray(res) Then
                    If UBound(res, 2) = 0 Then
                        def_prop_entity = New bc_om_entity
                        def_prop_entity.id = res(0, 0)
                        def_prop_entity.name = res(1, 0)
                        def_prop_entity.class_id = res(2, 0)
                        def_prop_entity.class_name = res(3, 0)
                    End If
                End If
                If doc.id = 0 Then
                    If bc_cs_central_settings.server_flag = 0 Then
                        If analytic_mode = True Then
                            sql = "exec dbo.bc_core_def_docs_analytic " + CStr(doc.entity_id) + "," + CStr(bc_cs_central_settings.logged_on_user_id) + "," + CStr(pub_type_id)

                        Else
                            sql = "exec dbo.bc_core_def_docs " + CStr(doc.entity_id) + "," + CStr(bc_cs_central_settings.logged_on_user_id) + "," + CStr(pub_type_id)
                        End If
                    Else
                        If analytic_mode = True Then
                            sql = "exec dbo.bc_core_def_docs_analytic " + CStr(doc.entity_id) + "," + CStr(certificate.user_id) + "," + CStr(pub_type_id)

                        Else
                            sql = "exec dbo.bc_core_def_docs " + CStr(doc.entity_id) + "," + CStr(certificate.user_id) + "," + CStr(pub_type_id)
                        End If
                    End If
                    Dim vres As Object
                    vres = gdb.executesql(sql, certificate)
                    Dim ddoc As bc_om_document
                    If IsArray(vres) Then
                        For i = 0 To UBound(vres, 2)
                            ddoc = New bc_om_document
                            ddoc.id = vres(0, i)
                            ddoc.title = vres(1, i)
                            ddoc.doc_date = vres(2, i)
                            ddoc.original_extension = vres(3, i)
                            ddoc.filename = vres(4, i)
                            ddoc.pub_type_name = vres(5, i)
                            ddoc.extension = vres(7, i)
                            def_docs.Add(ddoc)
                        Next
                    End If
                End If
            Catch ex As Exception
                Dim db_err As New bc_cs_error_log("bc_om_doc_select_params", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)

            End Try

        End Sub
    End Class

    <Serializable()> Public Class bc_om_doc_search_params
        Inherits bc_cs_soap_base_class

        Public date_from As Date
        Public date_to As Date
        Public entity_id As Long
        Public author_id As Long
        Public type_id As Long
        Public prop_entity_id As Long
        Public search_results As New List(Of bc_om_document)
        Public analytic_mode As Boolean = False
        Public Overrides Sub process_object()
            Select Case tmode
                Case bc_cs_soap_base_class.tREAD
                    db_read()
            End Select
        End Sub

        Public Sub db_read()
            Try
                date_from = Left(date_from, InStr(date_from, " ")) + " 00:00"
                date_to = Left(date_to, InStr(date_to, " ")) + " 23:59"
                search_results.Clear()
                Dim sdate_from, sdate_to As bc_om_date
                Dim xsdate_from, xsdate_to As String
                sdate_from = New bc_om_date(date_from)
                sdate_to = New bc_om_date(date_to)
                xsdate_from = sdate_from.write_data_to_xml(Nothing)
                xsdate_to = sdate_to.write_data_to_xml(Nothing)

                Dim gdb As New bc_cs_db_services

                Dim sql As String
                If bc_cs_central_settings.server_flag = 0 Then
                    If analytic_mode = True Then
                        sql = "exec dbo.bc_core_analytic_document_search '" + xsdate_from + "','" + xsdate_to + "'," + CStr(entity_id) + "," + CStr(author_id) + "," + CStr(type_id) + "," + CStr(prop_entity_id) + "," + CStr(bc_cs_central_settings.logged_on_user_id)

                    Else
                        sql = "exec dbo.bc_core_linked_document_search '" + xsdate_from + "','" + xsdate_to + "'," + CStr(entity_id) + "," + CStr(author_id) + "," + CStr(type_id) + "," + CStr(prop_entity_id) + "," + CStr(bc_cs_central_settings.logged_on_user_id)
                    End If
                Else
                    If analytic_mode = True Then
                        sql = "exec dbo.bc_core_analytic_document_search '" + xsdate_from + "','" + xsdate_to + "'," + CStr(entity_id) + "," + CStr(author_id) + "," + CStr(type_id) + "," + CStr(prop_entity_id) + "," + CStr(certificate.user_id)
                    Else
                        sql = "exec dbo.bc_core_linked_document_search '" + xsdate_from + "','" + xsdate_to + "'," + CStr(entity_id) + "," + CStr(author_id) + "," + CStr(type_id) + "," + CStr(prop_entity_id) + "," + CStr(certificate.user_id)
                    End If
                End If
                Dim vres As Object
                vres = gdb.executesql(sql, certificate)
                Dim doc As bc_om_document
                If IsArray(vres) Then
                    For i = 0 To UBound(vres, 2)
                        doc = New bc_om_document
                        doc.id = vres(0, i)
                        doc.title = vres(1, i)
                        doc.doc_date = vres(2, i)
                        doc.original_extension = vres(3, i)
                        doc.filename = vres(4, i)
                        doc.pub_type_name = vres(5, i)
                        doc.extension = vres(7, i)
                        search_results.Add(doc)
                    Next
                End If


            Catch ex As Exception
                Dim db_err As New bc_cs_error_log("bc_om_doc_search_params", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)

            End Try
        End Sub

    End Class
    <Serializable()> Public Class bc_om_html_preview
        Inherits bc_cs_soap_base_class
        Public doc_Id As Long
        Public html As Byte()
        Public success As Boolean = False

        Public Overrides Sub process_object()
            Dim otrace As New bc_cs_activity_log("bc_om_html_preview", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
            Try
                If Me.tmode = bc_cs_soap_base_class.tREAD Then
                    db_read()
                End If
            Catch ex As Exception
                Dim db_err As New bc_cs_error_log("bc_om_html_preview", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            Finally
                otrace = New bc_cs_activity_log("bc_om_html_preview", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
            End Try
        End Sub
        Public Sub db_read()
            Dim fn As String
            fn = bc_cs_central_settings.central_repos_path + "\html previews\" + CStr(doc_Id) + ".html"
            Dim fs As New bc_cs_file_transfer_services
            If fs.check_document_exists(fn) Then
                If fs.write_document_to_bytestream(fn, html, certificate, False) = True Then
                    success = True
                End If
            End If
        End Sub
    End Class
    <Serializable()> Public Class bc_om_doc_security
        Inherits bc_cs_soap_base_class
        Public schema_id As Long
        Public entity_id As Long
        Public pub_type_id As Long
        Public approval_type As Integer
        REM 0 none
        REM 1 normal
        REM 2 proxy
        Public proxy_user_ids As New ArrayList
        Public proxy_user_names As New ArrayList

        Public Overrides Sub process_object()
            Dim otrace As New bc_cs_activity_log("bc_om_doc_security", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
            Try
                If Me.tmode = bc_cs_soap_base_class.tREAD Then
                    db_read()
                End If
            Catch ex As Exception
                Dim db_err As New bc_cs_error_log("bc_om_doc_security", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            Finally
                otrace = New bc_cs_activity_log("bc_om_doc_securitys", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
            End Try
        End Sub

        Public Sub New()
            Me.approval_type = 0
            Me.schema_id = 1
        End Sub

        Public Sub db_read()
            Dim vres As Object
            Dim gdb As New bc_om_create_security_db
            vres = gdb.get_submission_security(Me.schema_id, Me.pub_type_id, Me.entity_id, MyBase.certificate)
            proxy_user_ids.Clear()
            proxy_user_names.Clear()
            If IsArray(vres) Then
                For i = 0 To UBound(vres, 2)
                    Me.approval_type = vres(0, i)
                    If Me.approval_type = 0 Then
                        REM access denied
                        Exit Sub
                    ElseIf Me.approval_type = 1 Then
                        REM user has primary preferacne so can submit
                        Exit Sub
                    ElseIf Me.approval_type = 2 Then
                        REM user can submit on behalf of list below
                        proxy_user_ids.Add(vres(1, i))
                        proxy_user_names.Add(vres(2, i))
                    Else
                        Me.approval_type = 0
                    End If
                Next
            End If

        End Sub
        Private Class bc_om_create_security_db
            Private gbc_db As New bc_cs_db_services
            Public Function get_submission_security(ByVal schema_id As Long, ByVal pub_type_id As Long, ByVal entity_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
                Try
                    Dim user_id As Long
                    If bc_cs_central_settings.server_flag = 0 Then
                        user_id = bc_cs_central_settings.logged_on_user_id
                    Else
                        user_id = certificate.user_id
                    End If
                    Dim sql As String
                    sql = "exec dbo.bc_core_create_sub_sec " + CStr(schema_id) + "," + CStr(pub_type_id) + "," + CStr(entity_id) + "," + CStr(user_id)
                    get_submission_security = gbc_db.executesql(sql, certificate)

                Catch ex As Exception
                    Dim oerr As New bc_cs_error_log("bc_om_create_security_db", "get_submission_security", bc_cs_error_codes.USER_DEFINED, ex.Message)
                    get_submission_security = Nothing
                End Try

            End Function

            Public Sub New()

            End Sub
        End Class
    End Class
    REM =======
    <Serializable()> Public Class bc_om_date
        Inherits bc_cs_soap_base_class
        Public da As DateTime
        Public Sub New()

        End Sub
        Public Sub New(ByVal da As DateTime)
            Me.da = da
        End Sub
        Public Overrides Function write_data_to_xml(ByRef certifiate As bc_cs_security.certificate) As String
            Dim otrace As New bc_cs_activity_log("bc_om_date", "write_data_to_xml", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
            Dim cbc_data_services As New bc_cs_data_services
            Dim ocommentary As New bc_cs_activity_log("bc_om_document_write", "write_data_to_xml", bc_cs_activity_codes.COMMENTARY, "Attempting to write XML string")

            write_data_to_xml = soap_serialize_object_to_xml(Me, Me.certificate)

            otrace = New bc_cs_activity_log("bc_om_date", "write_data_to_xml", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Function
        Private Function soap_serialize_object_to_xml(ByVal o As Object, ByRef certificate As bc_cs_security.certificate, Optional ByVal packet_code As String = "") As String

            Dim otrace As New bc_cs_activity_log("bc_om_document_write", "soap_serialize_object_to_xml", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

            Dim bc_cs_central_settings As New bc_cs_central_settings
            Dim by() As Byte

            Try
                Using ms As New MemoryStream

                    Dim extratype(0) As Type

                    extratype(0) = GetType(bc_om_user)
                    'extratype(1) = GetType(bc_om_taxonomy)
                    'extratype(2) = GetType(bc_om_history)
                    'extratype(3) = GetType(bc_om_comp_cons)

                    Dim sf As New XmlSerializer(o.GetType, extratype)

                    sf.Serialize(ms, o)

                    by = ms.ToArray()

                    ms.Close()

                    'xml serialization does not always encode apostrophe's.  This causes an issue when passing to SQL Server
                    soap_serialize_object_to_xml = Replace(System.Text.Encoding.UTF8.GetString(by), "'", "&#39;")

                End Using

            Catch ex As Exception
                Dim db_err As New bc_cs_error_log("bc_om_document_write", "soap_serialize_object_to_xml", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
                soap_serialize_object_to_xml = ""
            Finally
                otrace = New bc_cs_activity_log("bc_om_document_write", "soap_serialize_object_to_xml", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
            End Try

        End Function

    End Class
    <Serializable()> Public Class bc_om_documents
        Inherits bc_cs_soap_base_class
        Public document As New ArrayList
        Public all As Boolean
        Public date_from As Date
        Public date_to As Date
        Public workflow_mode As Boolean
        Public show_publish As Boolean
        Public publish_only As Boolean
        Public filter_items As New List(Of bc_om_document_filter_item)
        <Serializable()> Public Class bc_om_document_filter_item
            Public type_name As String
            Public item As String


        End Class
        Public Sub New()
            workflow_mode = False
            show_publish = False
        End Sub
        Public Sub New(ByVal ball As Boolean)
            all = ball
            workflow_mode = False
            show_publish = False
        End Sub
        REM used when instantiated on server so client logged on user can be supplied
        Public Sub New(ByVal ball As Boolean, ByVal logged_on_user As Long, ByVal date_from As Date, ByVal date_to As Date)
            all = ball
            Me.date_from = date_from
            Me.date_to = date_to
            show_publish = False
        End Sub
        Public Overrides Sub process_object()
            Dim otrace As New bc_cs_activity_log("bc_om_documents", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
            Try
                REM this is always specific to object
                If Me.tmode = bc_cs_soap_base_class.tREAD Then
                    db_read()
                End If
                If Me.tmode = bc_cs_soap_base_class.tREAD_ALL_CHECKED_OUT Then
                    read_all_checked_out_documents()
                End If
            Catch ex As Exception
                Dim db_err As New bc_cs_error_log("bc_om_documents", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            Finally
                otrace = New bc_cs_activity_log("bc_om_documents", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
            End Try
        End Sub
        Public Sub read_all_checked_out_documents()
            Dim otrace As New bc_cs_activity_log("bc_om_documents", "read_all_checked_out_documents", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
            Try
                Dim odocument As bc_om_document
                Dim db_document As New bc_om_document_db
                Dim vdocuments As Object

                Dim i As Integer
                Dim vtitle As Object
                Me.document.Clear()
                'If workflow_mode = True Then
                'Me.show_publish = True
                'End If
                vdocuments = db_document.read_checked_out_documents(all, workflow_mode, date_from, date_to, certificate)
                If IsArray(vdocuments) Then
                    For i = 0 To UBound(vdocuments, 2)
                        odocument = New bc_om_document(vdocuments(4, i), vdocuments(15, i), vdocuments(16, i), vdocuments(0, i), vdocuments(1, i), vdocuments(14, i), vdocuments(7, i), vdocuments(13, i), 1, False, vdocuments(2, i), vdocuments(9, i), vdocuments(10, i), True, vdocuments(8, i), vdocuments(11, i), vdocuments(12, i), 1, 0, vdocuments(6, i), vdocuments(19, i), vdocuments(24, i), vdocuments(20, i))
                        odocument.extension = vdocuments(22, i)
                        odocument.certificate = MyBase.certificate
                        REM now get originating author and set
                        If vdocuments(15, i) = 0 Then
                            Dim oorigauthor As Object
                            oorigauthor = db_document.read_originating_author(vdocuments(4, i), MyBase.certificate)
                            If IsArray(oorigauthor) Then
                                Try
                                    odocument.originating_author = oorigauthor(0, 0)
                                Catch
                                    Dim ocommentary As New bc_cs_activity_log("bc_om_documents", "db_read", bc_cs_activity_codes.COMMENTARY, "No originating author for document:" + CStr(vdocuments(4, i)), certificate)
                                End Try
                            End If
                        End If
                        REM if document is a translated copy get copy doc and ammend to title
                        If CStr(vdocuments(21, i)) <> "0" And CStr(vdocuments(21, i)) <> "" Then
                            vtitle = db_document.get_title_for_id(CStr(vdocuments(21, i)), MyBase.certificate)
                            If IsArray(vtitle) Then
                                If UBound(vtitle, 2) >= 0 Then
                                    odocument.title = odocument.title + " (copied from: " + CStr(vtitle(0, 0)) + ")"
                                End If
                            End If
                        End If
                        document.Add(odocument)
                    Next
                End If
                REM now take out redundant data
                For i = 0 To document.Count - 1
                    With document(i)
                        .action_Ids.clear()
                        .authors.clear()
                        .byteDoc = Nothing
                        .composite_constiuents.Clear()
                        .history.clear()
                        .pub_type_workflow.next_stages.Clear()
                        .refresh_components.refresh_components.clear()
                        .support_documents.clear()
                        .workflow_stages.stages.clear()
                    End With
                Next

            Catch ex As Exception
                Dim db_err As New bc_cs_error_log("bc_om_documents", "read_all_checked_out_documents", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            Finally
                otrace = New bc_cs_activity_log("bc_om_documents", "read_all_checked_out_documents", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
            End Try

        End Sub
        REM this operator checks if doc_ids and stages and title match only
        Public Shared Operator =(ByVal op1 As bc_om_documents, ByVal op2 As bc_om_documents) As Boolean
            If op1.document.Count <> op2.document.Count Then
                Return False
            End If

            Dim found As Boolean = False
            For i = 0 To op1.document.Count - 1
                found = False
                For j = 0 To op2.document.Count - 1
                    If op1.document(i).stage_name = op2.document(i).stage_name And op1.document(i).entity_id = op2.document(i).entity_id And op1.document(i).id = op2.document(i).id And op1.document(i).stage = op2.document(i).stage And op1.document(i).title = op2.document(i).title And op1.document(i).checked_out_user = op2.document(i).checked_out_user And op1.document(i).doc_date.Equals(op2.document(i).doc_date) = True And op1.document(i).stage_expire_date = op2.document(i).stage_expire_date And op1.document(i).distribution_status = op2.document(i).distribution_status And op1.document(i).attestation_pass = op2.document(i).attestation_pass And op1.document(i).num_attestations = op2.document(i).num_attestations And op1.document(i).attribute_change = op2.document(i).attribute_change Then
                        found = True
                        Exit For
                    End If
                Next
                If found = False Then
                    Return False
                End If
            Next
            Return True
        End Operator
        Public Shared Operator <>(ByVal op1 As bc_om_documents, ByVal op2 As bc_om_documents) As Boolean
            If op1.document.Count <> op2.document.Count Then
                Return True
            End If

            Dim found As Boolean = False
            For i = 0 To op1.document.Count - 1
                found = False
                For j = 0 To op2.document.Count - 1
                    If op1.document(i).stage_name = op2.document(i).stage_name And op1.document(i).entity_id = op2.document(i).entity_id And op1.document(i).id = op2.document(i).id And op1.document(i).stage = op2.document(i).stage And op1.document(i).title = op2.document(i).title And op1.document(i).checked_out_user = op2.document(i).checked_out_user And op1.document(i).doc_date.Equals(op2.document(i).doc_date) = True And op1.document(i).stage_expire_date = op2.document(i).stage_expire_date And op1.document(i).distribution_status = op2.document(i).distribution_status And op1.document(i).attestation_pass = op2.document(i).attestation_pass And op1.document(i).num_attestations = op2.document(i).num_attestations And op1.document(i).attribute_change = op2.document(i).attribute_change Then
                        found = True
                        Exit For
                    End If
                Next
                If found = False Then
                    Return True
                End If
            Next
            Return False
        End Operator

        Public Sub db_read()
            Dim otrace As New bc_cs_activity_log("bc_om_documents", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
            Try
                Dim i As Integer
                Dim odocument As bc_om_document
                Dim db_document As New bc_om_document_db
                Dim vdocuments As Object
                Dim vtitle As Object
                Me.document.Clear()

                If date_from = "9-9-9999" Then
                    date_from = "1-1-1900"
                Else
                    date_from = Left(date_from, InStr(date_from, " ")) + " 00:00"
                End If

                If InStr(date_to, " ") <> 0 Then
                    date_to = Left(date_to, InStr(date_to, " ")) + " 23:59"
                End If

                Dim sdate_from, sdate_to As bc_om_date
                Dim xsdate_from, xsdate_to As String
                sdate_from = New bc_om_date(date_from)
                sdate_to = New bc_om_date(date_to)
                xsdate_from = sdate_from.write_data_to_xml(certificate)
                xsdate_to = sdate_to.write_data_to_xml(certificate)


                vdocuments = db_document.read_all_documents(all, workflow_mode, xsdate_from, xsdate_to, show_publish, publish_only, certificate)

                If IsArray(vdocuments) Then
                    For i = 0 To UBound(vdocuments, 2)

                        odocument = New bc_om_document(vdocuments(4, i), vdocuments(15, i), vdocuments(16, i), vdocuments(0, i), vdocuments(1, i), vdocuments(14, i), vdocuments(7, i), vdocuments(13, i), vdocuments(23, i), False, vdocuments(2, i), vdocuments(9, i), vdocuments(10, i), True, vdocuments(8, i), vdocuments(11, i), vdocuments(12, i), 1, 0, vdocuments(6, i), vdocuments(19, i), vdocuments(20, i))
                        odocument.extension = vdocuments(22, i)
                        odocument.certificate = MyBase.certificate
                        REM if document is a translated copy get copy doc and ammend to title
                        'If CStr(vdocuments(21, i)) <> "0" And CStr(vdocuments(21, i)) <> "" Then
                        '    vtitle = db_document.get_title_for_id(CStr(vdocuments(21, i)), MyBase.certificate)
                        '    If IsArray(vtitle) Then
                        '        If UBound(vtitle, 2) >= 0 Then
                        '            odocument.title = odocument.title + " (copied from: " + CStr(vtitle(0, 0)) + ")"
                        '        End If
                        '    End If
                        'End If
                        odocument.lead_entity_name = CStr(vdocuments(24, i))
                        odocument.originator_name = CStr(vdocuments(25, i))
                        odocument.checked_out_user_name = CStr(vdocuments(26, i))
                        odocument.language_name = CStr(vdocuments(27, i))
                        Try
                            odocument.distribution_status = CStr(vdocuments(28, i))
                        Catch

                        End Try

                        Try
                            odocument.num_attestations = vdocuments(29, i)
                            odocument.attestation_pass = vdocuments(30, i)
                            odocument.fast_track = vdocuments(31, i)
                            odocument.attribute_change = vdocuments(32, i)

                        Catch

                        End Try
                        Try
                            odocument.assign_to_name = vdocuments(33, i)


                        Catch ex As Exception

                        End Try
                        document.Add(odocument)
                    Next
                End If

                REM get filter data
                Dim vfilter As Object
                Dim ofi As bc_om_document_filter_item
                Me.filter_items.Clear()

                vfilter = db_document.read_filter_items(all, workflow_mode, xsdate_from, xsdate_to, show_publish, publish_only, certificate)
                If IsArray(vfilter) Then
                    For i = 0 To UBound(vfilter, 2)
                        ofi = New bc_om_document_filter_item
                        ofi.type_name = vfilter(0, i)
                        ofi.item = vfilter(1, i)
                        Me.filter_items.Add(ofi)
                    Next
                End If
            Catch ex As Exception
                Dim db_err As New bc_cs_error_log("bc_om_documents", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            Finally
                otrace = New bc_cs_activity_log("bc_om_documents", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
            End Try
        End Sub
    End Class
    <Serializable()> Public Class bc_om_document
        Inherits bc_cs_soap_base_class

        Public id As Long
        Public entity_id As Long
        Public pub_type_id As Long
        Public title As String
        Public sub_title As String
        Public summary As String
        Public doc_date As Date
        Public pages As Integer
        Public language_id As Integer
        Public connection_method As String
        Public filename As String
        Public pub_type_name As String
        Public checked_out_user As String
        Public extension As String
        Public local_index As Integer
        Public stage As String
        Public stage_name As String
        Public originating_author As Long
        Public bus_area As String
        Public master_flag As Boolean
        Public master_container_id As Long
        Public brevision_mode As Integer
        Public revision_filename As String
        Public bimport_support_only As Boolean
        Public reject As Boolean
        Public reject_from_stage As Long
        Public new_doc As Boolean
        Public distribution_status As String
        Public attribute_change As String
        Public html_template As String
        Public assigned_user As Long
        Public assign_to_name As String
        REM actually document object
        <NonSerialized()> Public docobject As Object

        REM workflow stage routes
        Public workflow_stages As New bc_om_workflow


        REM authors
        Public authors As New List(Of bc_om_user)

        REM taxonomy
        Public taxonomy As New List(Of bc_om_taxonomy)



        REM defined Componnets
        Public refresh_components As New bc_om_refresh_components
        Public fast_track As Integer = 0

        REM disclosures
        Public disclosures As New ArrayList
        REM Byte Stream Representation of the actual document
        Public byteDoc As Byte()
        Public preeventbyteDoc As Byte()

        REM support documents
        Public support_documents As New List(Of bc_om_document)

        REM actions
        Public original_stage As Long
        Public original_stage_name As String
        Public action_Ids As New ArrayList
        REM history
        Public history As New List(Of bc_om_history)
        Public lead_entity_name As String
        Public originator_name As String
        Public checked_out_user_name As String

        REM logging information
        'Public logged_on_user_name As String = bc_cs_central_settings.user_name
        'Public logged_on_user_password As String = bc_cs_central_settings.user_password
        REM additional stuff for workflow
        Public main_note As String
        Public new_flag As Boolean
        Public urgent_flag As Boolean
        Public expire_flag As Boolean
        Public stage_change_flag As Boolean
        Public register_only As Boolean
        Public stage_expire_date As Date
        Public unread As Boolean
        Public doc_not_found As Boolean
        Public bwith_document As Boolean
        Public bcheck_out As Boolean
        Public btake_revision As Boolean
        Public write As Boolean
        Public read As Boolean
        Public move As Boolean
        Public locked As Boolean
        Public force_check_in As Boolean
    Public allow_disclosures As Boolean = True
    Public allow_dist_channels As Boolean = True
        Public arrive As Date
        Public acknowledged As Date
        Public master_translated_doc As String
        Public sub_translated_docs As String
        Public translated_from_doc As Long
        Public copy_doc_text As String
        Public teaser_text As String
        Public translate_flag As Boolean
        'Public approval_only As Boolean
        Public approved_by As Long
        Public qa As Boolean
        Public composite_constiuents As New ArrayList
        Public receive_translation As Boolean
        Public uploaded_translated_components As Boolean
        Public reset_reject As Boolean
        Public set_reject As Boolean
        Public reject_stage As Long
        Public reject_stage_name As String
        Public user_role As String
        Public pub_type_workflow As New bc_om_pub_type_workflow
        Public publish_flag As Boolean
        Public revision_from_process As Boolean = False
        Public support_doc_state As Integer
        Public last_revision_filename As String
        <NonSerialized()> Public search_flag As Boolean
        REM new extensions to object model not in PC
        'Public link1 As Boolean
        'Public link2 As Boolean
        'Public link1_text As String
        'Public link2_text As String
        'Public link1_url As String
        'Public link2_url As String
        Public links As New List(Of bc_om_document_link)

        Public generate_mailing_list As Boolean
        Public isStagingDocument As Boolean
        Public ParentDocumentId As String
        Public isdeleted As Boolean
        Public isnew As Boolean
        Public mandatory_fields As New bc_om_mandatory_fields
        Public entity_classify_filters As New bc_om_entity_classify_filters
        Public server_side_events As New bc_om_server_side_events
        Public server_side_events_failed As String
        Public list_items As New bc_om_list_items
        Public mailing_list_installed As Boolean = False
        Public register_template As Boolean = False
        Public do_not_show_taxonomy As Boolean = False
        Public auto_generate_taxonomy As Boolean = True
        Public original_extension As String
        'Public do_force_check_in As Boolean = False
        Public registered_file_name As String
        Public set_approve As Boolean = False
        Public approve_stage As String
        Public approve_stage_name As String
        Public local_filename As String
        Public secondary_entity_id As Long

        REM Public has_mailing_list As Boolean
        REM Public mail_list_url As String
        Public track_flag As Boolean
        Public pending_mode As Integer = 0
        Public comments As New bc_om_comments
        Public stage_change_comment As String
        Public read_mode As Integer = 0
        Public create_date As DateTime
        Public update_date As DateTime

        Public from_event As Boolean = False
        Public support_doc_display_only As Boolean = False

        Public language_name As String
        Public component_componetize As Boolean = False

        Public alternate_process_summary_display As String

        Public Const NORMAL = 0
        Public Const CHECK_DOCUMENT_STATE = 1000
        Public Const APPROVE_DOC_ONLY = 1001
        Public Const GET_REVISION = 1002
        Public Const CHECK_OUT_NON_CREATE_DOC = 1003
        Public Const GET_PHYSICAL_DOC_VIEW_ONLY = 1004
        Public Const READ_FOR_PROCESS = 1005
        Public Const REVERT = 1006
        Public Const CHECK_OUT_CREATE_DOC = 1007
        Public Const READ_FOR_CREATE = 1008
        Public Const CHECK_OUT_SUPPORT_DOC = 1009
        Public Const READ_FOR_CATEGORIZE = 1010
        Public Const TAKE_REVISION_BEFORE_REATTACH = 1010
        Public Const CHECK_OUT_HTML_PRODUCT = 1011
        Public Const CHECKED_IN = -4
        Public bwith_history As Boolean = False
        Public bwith_comments As Boolean = False
        Public bwith_links As Boolean = False
        REM FIL DEC 2012
        Public marked_for_delete As Boolean = False
        Public metadata_changed As Boolean = False
        REM FIL 5.3
        Public behalf_of_author_id As Long
        Public quick_submit_invoked As Boolean = False
        Public urgent_text As String
        Public oec As New bc_om_extended_classification

        REM reach
        Public html_preview_exists As Boolean = False
        Public linked_docs As New List(Of bc_om_document)
        Public analytic_docs As New List(Of bc_om_document)
        Public tables_on_Demand As Boolean = False

        Public num_attestations As Integer = 0
        Public attestation_pass As Boolean = False
        Public completed_date As DateTime
        Public disemination_date As DateTime

        Public summary_rtf As String
        Public summary_html As String

        Public support_doc_non_open_doc_event As Boolean = False
        Public custom_title As String
        Public custom_title2 As String
        Public disclosure_file As String
        Public disclosure_file_title As String
    Public last_disclosure_filename As String

    Public distribution_channels As New List(Of bc_om_distribution_channel)

        <Serializable()> Public Class bc_om_document_link
            Public display_name As String
            Public url As String
        End Class

        <Serializable()> Public Class bc_om_server_side_events
            Public events As New ArrayList
            Public success As Boolean
            Public Sub New()
                success = False
            End Sub
        End Class
        <Serializable()> Public Class bc_om_server_side_event
            Public sql As String
            Public name As String
            Public return_text As String
            Public routing_stage As Long

            Public Sub New()

            End Sub
        End Class

        Public Overrides Sub process_object()
            Dim otrace As New bc_cs_activity_log("bc_om_document", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
            Try
                REM this is always specific to object
                If Me.tmode = bc_cs_soap_base_class.tREAD Then
                    Select Case Me.read_mode
                        REM evaluates if a document has become checked out or stage changed if screen hanst been updated
                        Case CHECK_DOCUMENT_STATE
                            db_check_document_state()
                            REM document approval
                        Case APPROVE_DOC_ONLY
                            db_approve_doc_only()
                            REM get a revision of a document
                        Case GET_REVISION
                            db_get_revision()
                            REM revert to a previous document version
                        Case REVERT
                            db_revert()
                            REM get the physical file onlt
                        Case GET_PHYSICAL_DOC_VIEW_ONLY
                            db_read_physical_doc_view_only()
                        Case READ_FOR_CREATE
                            REM used by create document read with or wothout the file
                            db_read_for_create()
                        Case READ_FOR_PROCESS
                            REM reads all metadata required by process
                            db_read_for_process()
                        Case CHECK_OUT_CREATE_DOC
                            REM checks out a create document
                            db_check_out_create_doc()
                        Case CHECK_OUT_NON_CREATE_DOC
                            REM checks out a non create document
                            db_check_out_non_create_doc()
                        Case CHECK_OUT_SUPPORT_DOC
                            REM checks out a support document
                            Me.db_check_out_support_doc()
                        Case READ_FOR_CATEGORIZE
                            Me.db_read_for_categorize()
                        Case TAKE_REVISION_BEFORE_REATTACH
                            Me.take_revision()

                        Case CHECK_OUT_HTML_PRODUCT
                            Me.db_check_out_html_product()


                    End Select
                    Me.read_mode = 0
                End If
                If Me.tmode = bc_cs_soap_base_class.tWRITE Then
                    REM DEBUG for fil to store dat file
                    If bc_cs_central_settings.save_doc_metadata = 1 Then
                        If Me.id <> 0 Then
                            write_data_to_file(bc_cs_central_settings.doc_metadata_path + "\" + CStr(Me.id) + "_" + CStr(Me.stage) + "_" + Replace(Format(Now, "hh:mm:ss"), ":", "") + ".dat")
                        End If
                    End If
                    REM ===========================================
                    db_write(MyBase.certificate)

                End If
            Catch ex As Exception
                Dim db_err As New bc_cs_error_log("bc_om_document", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            Finally
                otrace = New bc_cs_activity_log("bc_om_document", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
            End Try
        End Sub
        Public Sub New()
            Me.stage_expire_date = System.DateTime.Parse("9-9-9999")
            Me.arrive = System.DateTime.Parse("1-1-1900")
            Me.acknowledged = System.DateTime.Parse("1-1-1900")
            Me.unread = False
            Me.new_flag = False
            Me.bwith_document = True
            Me.bcheck_out = True
            Me.btake_revision = True
            Me.search_flag = False
            Me.track_flag = False
            Me.write = False
            Me.read = False
            Me.move = False
            Me.register_only = False
            Me.doc_not_found = False
            Me.brevision_mode = 0
            Me.bimport_support_only = False
            Me.translate_flag = False
            'Me.approval_only = False
            Me.qa = False
            Me.receive_translation = False
            Me.uploaded_translated_components = False
            Me.reject = False
            Me.reset_reject = False
            Me.set_reject = False
            Me.publish_flag = False
            Me.packet_code = "create"
            'Me.link1 = False
            'Me.link2 = False
            Me.generate_mailing_list = False
            REM set this to 0 for new
            Me.support_doc_state = 0
            Me.new_doc = False

            translated_from_doc = 0

        End Sub
        Public Sub get_default_disclosure_access()
            Dim gdb As New bc_om_document_db
            gdb.get_access_rights(1, certificate)
            Dim vaccess As Object
            vaccess = gdb.get_access_rights(1, certificate)
            If IsArray(vaccess) Then
                For i = 0 To UBound(vaccess, 2)
                    If vaccess(0, i) = "D" Then
                        Me.allow_disclosures = True
                End If
                If vaccess(0, i) = "C" Then
                    Me.allow_dist_channels = True
                End If
                Next
            End If

        End Sub
        Public Sub New(ByVal iid As Long, ByVal lentity_id As Long, ByVal lauthor_id As Long, ByVal lpub_type_id As Long, ByVal strtitle As String, ByVal strsummary As String, ByVal ddoc_date As Date, ByVal ipages As Integer, ByVal ilanguage As Integer, ByVal strconnection_method As String, ByVal strfilename As String, ByVal strpub_type_name As String, ByVal lchecked_out_user As Long, ByVal def As Boolean, ByVal stage As String, ByVal sub_title As String, ByVal bus_area As String, ByVal master_flag As Boolean, ByVal container_id As Long, ByVal stage_name As String, ByVal target_date As String, ByVal main_note As String, Optional ByVal default_disclosure As Long = 0)
            Me.id = iid
            Me.entity_id = lentity_id
            Me.pub_type_id = lpub_type_id
            Me.title = strtitle
            Me.summary = strsummary
            Me.doc_date = ddoc_date
            Me.pages = ipages
            Me.connection_method = strconnection_method
            Me.language_id = ilanguage
            Me.filename = strfilename
            Me.pub_type_name = strpub_type_name
            Me.checked_out_user = lchecked_out_user
            Me.bus_area = bus_area
            Me.sub_title = sub_title
            Me.register_only = False
            local_index = -1
            Me.stage = stage
            Me.unread = False
            Me.stage_expire_date = target_date
            Me.main_note = main_note
            Me.expire_flag = False
            Me.urgent_flag = False
            Me.master_flag = master_flag
            Me.master_container_id = container_id
            Me.stage_name = stage_name
            Me.originating_author = lauthor_id
            Me.new_flag = False
            Me.bwith_document = True
            Me.bcheck_out = True
            Me.btake_revision = True
            Me.search_flag = False
            Me.track_flag = False
            Me.write = False
            Me.read = False
            Me.move = False
            Me.doc_not_found = False
            Me.brevision_mode = 0
            translated_from_doc = 0
            Me.bimport_support_only = False
            Me.translate_flag = False
            'Me.approval_only = False
            Me.qa = False
            Me.receive_translation = False
            Me.uploaded_translated_components = False
            Me.reject = False
            Me.reset_reject = False
            Me.set_reject = False
            Me.publish_flag = False
            Me.new_doc = False


            REM set this to 0 for new
            Me.support_doc_state = 0

            Me.packet_code = "create"
            If def = False Then
                Dim default_author As New bc_om_user
                default_author.certificate = MyBase.certificate
                default_author.id = lauthor_id
                authors.Add(default_author)
                Dim default_entity As New bc_om_taxonomy
                default_entity.entity_id = lentity_id
                taxonomy.Add(default_entity)
            End If
            If default_disclosure <> 0 Then
                Dim otaxonomy = New bc_om_taxonomy(default_disclosure)
                disclosures.Add(otaxonomy)
            End If
            'Me.link1 = False
            'Me.link2 = False
            Me.generate_mailing_list = False

            REM assigned loged on user if form authentication is used
            If bc_cs_central_settings.show_authentication_form = 1 Then


            End If
        End Sub
        Public Sub db_check_document_state()
            Dim otrace As New bc_cs_activity_log("bc_om_document", "db_check_document_state", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
            Try
                REM check to see if document on client matches taht on server
                REM i.e. checked in and at same stage
                REM used if someone else has chekced it out ir moved it and client nor refreshed
                REM -1 incorrect stage
                REM 0 checked out
                Dim db_document As New bc_om_document_db

                Dim res As Object
                res = db_document.db_read_check_document_state(Me.id, Me.stage, MyBase.certificate)
                If IsArray(res) Then
                    Me.id = res(0, 0)
                End If
            Catch ex As Exception
                Dim db_err As New bc_cs_error_log("bc_om_document", "db_check_document_state", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            Finally
                otrace = New bc_cs_activity_log("bc_om_document", "db_check_document_state", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
            End Try
        End Sub

        Public Sub db_approve_doc_only()
            Dim otrace As New bc_cs_activity_log("bc_om_document", "db_approve_doc", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
            Try

                Dim tid As Long
                tid = id
                db_check_document_state()
                If Me.id <= 0 And Me.id <> CHECKED_IN Then
                    Exit Sub
                End If
                id = tid
                Dim db_document As New bc_om_document_trans_db
                db_document.new_approve_doc(Nothing, Me.id, Me.stage, Me.approve_stage, certificate)

            Catch ex As Exception
                Dim db_err As New bc_cs_error_log("bc_om_document", "db_approve_doc", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            Finally
                otrace = New bc_cs_activity_log("bc_om_document", "db_approve_doc", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
            End Try
        End Sub

        Public Sub db_get_revision()
            Dim otrace As New bc_cs_activity_log("bc_om_document", "db_get_revision", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
            Try
                REM read revision copy only
                Dim fs As New bc_cs_file_transfer_services
                If fs.write_document_to_bytestream(bc_cs_central_settings.central_repos_path + Me.revision_filename, Me.byteDoc, MyBase.certificate, False) = False Then
                    Me.doc_not_found = True
                End If
                Me.doc_not_found = False
            Catch ex As Exception
                Dim db_err As New bc_cs_error_log("bc_om_document", "db_get_revision", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            Finally
                otrace = New bc_cs_activity_log("bc_om_document", "db_get_revision", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
            End Try
        End Sub

        Public Sub db_revert()
            Dim otrace As New bc_cs_activity_log("bc_om_document", "db_take_revision", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
            Try

                Dim tid As Long
                tid = id
                db_check_document_state()
                If Me.id <= 0 And Me.id <> CHECKED_IN Then
                    Exit Sub
                End If
                id = tid
                REM document revert is document hasnt just become checked out or edit
                REM check document to revert to exists on server
                Dim fs As New bc_cs_file_transfer_services
                If fs.check_document_exists(bc_cs_central_settings.central_repos_path + Me.revision_filename) = False Then
                    doc_not_found = True
                Else
                    REM take revision of master
                    If Me.take_revision = True Then
                        REM copy reverted copy over master
                        If Me.revert_file(Me.revision_filename) = False Then
                            Me.id = -2
                        End If
                    Else
                        Me.id = -3
                    End If
                End If

            Catch ex As Exception
                Dim db_err As New bc_cs_error_log("bc_om_document", "db_take_revision", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            Finally
                otrace = New bc_cs_activity_log("bc_om_document", "db_take_revision", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
            End Try
        End Sub
        Public Sub db_check_out_html_product()
            Dim otrace As New bc_cs_activity_log("bc_om_document", "xdb_check_out_html_product", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
            Try
                Dim tid As Long
                tid = id
                db_check_document_state()
                If Me.id <= 0 And Me.id <> CHECKED_IN Then
                    Exit Sub
                End If
                id = tid

                Dim db_document As New bc_om_document_db
                Me.checked_out_user = db_document.check_out_document(id, Me.stage, MyBase.certificate)
                Dim res As Object
                res = db_document.get_html_product(id, MyBase.certificate)
                If IsArray(res) Then
                    If UBound(res, 2) = 0 Then
                        Me.html_template = res(0, 0)
                    End If

                End If
            Catch ex As Exception
                Dim db_err As New bc_cs_error_log("bc_om_document", "db_check_out_html_product", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            Finally
                otrace = New bc_cs_activity_log("bc_om_document", "db_check_out_html_product", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
            End Try
        End Sub
        Public Sub db_check_out_non_create_doc()
            Dim otrace As New bc_cs_activity_log("bc_om_document", "db_check_out_non_create_doc", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
            Try
                Dim tid As Long
                tid = id
                db_check_document_state()
                If Me.id <= 0 And Me.id <> CHECKED_IN Then
                    Exit Sub
                End If
                id = tid
                Dim fs As New bc_cs_file_transfer_services
                Me.doc_not_found = False

                If fs.write_document_to_bytestream(bc_cs_central_settings.central_repos_path + Me.filename, Me.byteDoc, MyBase.certificate, True) = False Then
                    Me.doc_not_found = True
                    Exit Sub
                End If
                Me.take_revision()

                Dim db_document As New bc_om_document_db
                Me.checked_out_user = db_document.check_out_document(id, Me.stage, MyBase.certificate)


            Catch ex As Exception
                Dim db_err As New bc_cs_error_log("bc_om_document", "db_check_out_non_create_doc", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            Finally
                otrace = New bc_cs_activity_log("bc_om_document", "db_check_out_non_create_doc", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
            End Try
        End Sub
        Public Sub db_check_out_support_doc()
            Dim otrace As New bc_cs_activity_log("bc_om_document", "db_check_out_support_doc", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
            Try
                Dim tid As Long
                tid = id
                db_check_document_state()
                If Me.id <= 0 And Me.id <> CHECKED_IN Then
                    Exit Sub
                End If
                Dim fs As New bc_cs_file_transfer_services
                Me.doc_not_found = False

                If fs.write_document_to_bytestream(bc_cs_central_settings.central_repos_path + Me.filename, Me.byteDoc, MyBase.certificate, True) = False Then
                    Me.doc_not_found = True
                    Exit Sub
                End If
                id = tid

                Dim db_document As New bc_om_document_db
                Me.checked_out_user = db_document.check_out_document(id, Me.stage, MyBase.certificate, True, Me.title)


            Catch ex As Exception
                Dim db_err As New bc_cs_error_log("bc_om_document", "db_check_out_support_doc", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            Finally
                otrace = New bc_cs_activity_log("bc_om_document", "db_check_out_support_doc", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
            End Try
        End Sub

        Public Sub db_read_physical_doc_view_only()
            Dim otrace As New bc_cs_activity_log("bc_om_document", "db_read_physical_doc_view_only", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
            Try

                Dim fs As New bc_cs_file_transfer_services
                Me.doc_not_found = False
                If fs.write_document_to_bytestream(bc_cs_central_settings.central_repos_path + Me.filename, Me.byteDoc, MyBase.certificate, True) = False Then
                    Me.doc_not_found = True
                End If
            Catch ex As Exception
                Dim db_err As New bc_cs_error_log("bc_om_document", "db_read_physical_doc_view_only", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            Finally
                otrace = New bc_cs_activity_log("bc_om_document", "db_read_physical_doc_view_only", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
            End Try
        End Sub
        REM read used when selecting a document in process
        Public Sub db_read_for_process()
            Dim otrace As New bc_cs_activity_log("bc_om_document", "db_read_for_process", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
            Try
                Dim db_document As New bc_om_document_db
                Dim vresults As Object
                Dim otaxonomy As bc_om_taxonomy
                Dim oauthor As bc_om_user
                Dim i As Integer
                Dim vhistory As Object
                Dim vaccess As Object
                Dim ohistory As bc_om_history





                REM  document
                vresults = db_document.read_document(id, MyBase.certificate)

                If IsArray(vresults) Then
                    Me.pub_type_id = vresults(1, 0)
                    Me.title = vresults(2, 0)
                    Me.summary = vresults(3, 0)
                    Me.doc_date = vresults(4, 0)
                    Me.pages = vresults(5, 0)
                    Me.language_id = vresults(6, 0)
                    Me.filename = vresults(8, 0)
                    Me.checked_out_user = vresults(9, 0)
                    Me.extension = vresults(10, 0)
                    Me.pub_type_name = vresults(11, 0)
                    Me.stage = vresults(14, 0)
                    Me.sub_title = vresults(13, 0)
                    Me.stage_name = vresults(12, 0)
                    Me.bus_area = vresults(15, 0)
                    Me.master_container_id = vresults(18, 0)
                    Me.refresh_components.workflow_state = vresults(16, 0)
                    Me.refresh_components.accounting_standard = vresults(17, 0)
                    Me.refresh_components.data_at_date = CStr(vresults(19, 0))
                    Me.stage_expire_date = CDate(vresults(20, 0))
                    Me.originating_author = vresults(22, 0)

                    Me.main_note = vresults(23, 0)
                    Me.entity_id = CLng(vresults(24, 0))
                    Me.teaser_text = vresults(25, 0)
                    Me.update_date = vresults(26, 0)
                    Me.create_date = vresults(27, 0)
                    Me.urgent_flag = vresults(29, 0)
                    Me.urgent_text = vresults(30, 0)
                    Me.behalf_of_author_id = vresults(31, 0)
                    Try
                        Me.completed_date = vresults(32, 0)
                        Me.disemination_date = vresults(33, 0)

                    Catch ex As Exception

                    End Try
                    Try
                        Me.summary_html = vresults(34, 0)
                        Me.custom_title = vresults(35, 0)
                        Me.custom_title2 = vresults(36, 0)
                    Catch ex As Exception

                    End Try
                End If


                REM most recent comment
                comments.comments.Clear()
                comments = New bc_om_comments
                comments.doc_id = Me.id
                comments.span = 1
                comments.db_read()
                REM read history
                history.Clear()
                vhistory = db_document.get_history(Me.id, MyBase.certificate)
                If IsArray(vhistory) Then
                    For i = 0 To UBound(vhistory, 2)
                        ohistory = New bc_om_history
                        ohistory.da = vhistory(0, i)
                        ohistory.desc = vhistory(1, i)
                        ohistory.stage = vhistory(2, i)
                        ohistory.user = vhistory(3, i)
                        ohistory.code = vhistory(4, i)
                        history.Add(ohistory)
                    Next
                End If

                REM links
                Me.links.Clear()
                Dim vlink As Object
                Dim olink As bc_om_document_link
                vlink = db_document.has_link(Me.id, 1, MyBase.certificate)
                If IsArray(vlink) Then
                    For i = 0 To UBound(vlink, 2)
                        olink = New bc_om_document_link
                        olink.url = vlink(0, i)
                        olink.display_name = vlink(1, i)
                        Me.links.Add(olink)
                    Next
                End If

                REM access rights
                vaccess = db_document.get_access_rights(Me.stage, MyBase.certificate)
                Me.force_check_in = False
            Dim bdisc As Boolean = False
            Dim bdc As Boolean = False
                If IsArray(vaccess) Then
                    For i = 0 To UBound(vaccess, 2)
                        If vaccess(0, i) = "M" Then
                            Me.move = True
                        End If
                        If vaccess(0, i) = "R" Then
                            Me.read = True
                        End If
                        If vaccess(0, i) = "W" Then
                            Me.write = True
                        End If
                        If vaccess(0, i) = "L" Then
                            Me.locked = True
                        End If
                        If vaccess(0, i) = "F" Then
                            Me.force_check_in = True
                        End If
                        Me.allow_disclosures = False
                        If vaccess(0, i) = "D" Then
                            bdisc = True
                    End If
                    Me.allow_dist_channels = False
                    If vaccess(0, i) = "C" Then
                        bdc = True
                    End If

                    Next
                End If

                If bdisc = False Then
                    Me.allow_disclosures = False
                Else
                    Me.allow_disclosures = True
            End If
            If bdc = False Then
                Me.allow_dist_channels = False
            Else
                Me.allow_dist_channels = True
            End If

                REM workflow
                Dim bc_om_stage As New bc_om_workflow_stage

                bc_om_stage.certificate = MyBase.certificate
                bc_om_stage.stage_id = Me.stage
                bc_om_stage.stage_name = Me.stage_name

                Me.workflow_stages.stages.Clear()

                Me.workflow_stages.stages.Add(bc_om_stage)

                Me.workflow_stages.db_read(Me.master_container_id, Me.stage, False, Me.pub_type_id, Me.id, MyBase.certificate)


                REM now also get totally reading for pub type
                pub_type_workflow = New bc_om_pub_type_workflow
                pub_type_workflow.certificate = MyBase.certificate
                pub_type_workflow.id = Me.pub_type_id
                pub_type_workflow.current_stage = Me.stage

                pub_type_workflow.load()

                '==========================================================================================
                'REM check if distribution list generator is installed
                'REM turn this stuff of for now as not sure if it is used
                ''Me.mailing_list_installed = db_document.is_generate_dist_list_installed(MyBase.certificate)
                'REM manadatory fields
                'mandatory_fields = New bc_om_mandatory_fields(Me.id)
                'mandatory_fields.db_read(MyBase.certificate)
                'REM entity classify filter
                'REM I am not sure what this does but I will leave in for now
                'entity_classify_filters = New bc_om_entity_classify_filters(Me.id)
                'entity_classify_filters.db_read(MyBase.certificate)

                'REM evaluate wether to show taxonomy or not
                'Dim user_db As New bc_om_user_db
                'Me.do_not_show_taxonomy = user_db.do_not_display_taxonomy(certificate.user_id, MyBase.certificate)
                '==========================================================================================


                REM read in taxonomy
                taxonomy.Clear()
                vresults = db_document.read_taxonomy_for_document(id, MyBase.certificate)
                If IsArray(vresults) Then
                    For i = 0 To UBound(vresults, 2)
                        otaxonomy = New bc_om_taxonomy(vresults(0, i))
                        otaxonomy.class_id = vresults(1, i)
                        otaxonomy.name = vresults(2, i)
                        otaxonomy.class_name = vresults(3, i)
                        taxonomy.Add(otaxonomy)

                    Next
                End If


                REM read in authors
                authors.Clear()
                vresults = db_document.read_authors_for_document(id, MyBase.certificate)
                If IsArray(vresults) Then
                    For i = 0 To UBound(vresults, 2)
                        oauthor = New bc_om_user(vresults(0, i), "", "", "", 0, "", "", "", MyBase.certificate)
                        authors.Add(oauthor)
                    Next
                End If
                REM read in disclsoures
                disclosures.Clear()
                vresults = db_document.read_disclosures_for_document(id, MyBase.certificate)
                If IsArray(vresults) Then
                    For i = 0 To UBound(vresults, 2)
                        otaxonomy = New bc_om_taxonomy(vresults(0, i))
                        disclosures.Add(otaxonomy)
                    Next
                End If

                REM support documents
                Dim vdocs As Object
                Me.support_documents.Clear()
                vdocs = db_document.get_docs_for_container(Me.master_container_id, certificate)
                Me.support_documents.Clear()
                If IsArray(vdocs) Then
                    For i = 0 To UBound(vdocs, 2)
                        REM for each support read in metadata for it
                        If vdocs(0, i) <> Me.id Then
                            Dim osupport As New bc_om_document
                            osupport.certificate = Me.certificate
                            osupport.id = vdocs(0, i)
                            osupport.stage = Me.stage
                            osupport.db_read_support_doc(False)
                            Me.support_documents.Add(osupport)
                        End If
                    Next
                End If


                REM translated docs
                REM copies document has been translated to
                Dim vtrans As Object
                Me.sub_translated_docs = ""
                'vtrans = db_document.get_master_translated_doc_text(Me.id, MyBase.certificate)
                'If IsArray(vtrans) Then
                '    If UBound(vtrans, 2) >= 0 Then
                '        For i = 0 To UBound(vtrans, 2)
                '            If i > 0 Then
                '                Me.sub_translated_docs = Me.sub_translated_docs + "; "
                '            End If
                '            Me.sub_translated_docs = Me.sub_translated_docs + CStr(vtrans(0, i))
                '        Next
                '    End If
                'End If
                REM doc tranlated from
                Me.master_translated_doc = ""
                'Dim vtransm As Object
                'vtransm = db_document.get_sub_doc_text(Me.id, MyBase.certificate)
                'If IsArray(vtransm) Then
                '    If UBound(vtransm, 2) >= 0 Then
                '        Me.master_translated_doc = vtransm(0, 0)
                '        Me.translated_from_doc = CLng(vtransm(1, 0))
                '        Me.title = Me.title
                '        Me.copy_doc_text = "(Copied from: " + CStr(vtransm(0, 0)) + " type: " + Trim(CStr(vtransm(4, 0))) + ")"
                '    End If
                'End If


                Dim fn As String
                fn = bc_cs_central_settings.central_repos_path + "\html previews\" + CStr(id) + ".html"
                Dim fs As New bc_cs_file_transfer_services
                html_preview_exists = False
                If fs.check_document_exists(fn) Then
                    html_preview_exists = True
                End If
                Dim vres As Object
                Dim ld As bc_om_document
                linked_docs.Clear()
                vres = db_document.read_linked_documents(Me.id, MyBase.certificate)
                If IsArray(vres) Then
                    For i = 0 To UBound(vres, 2)
                        ld = New bc_om_document
                        ld.id = vres(0, i)
                        ld.title = vres(1, i)
                        ld.doc_date = vres(2, i)
                        ld.original_extension = vres(3, i)
                        ld.filename = vres(4, i)
                        ld.pub_type_name = vres(5, i)
                        ld.extension = vres(6, i)
                        linked_docs.Add(ld)
                    Next
                End If
                analytic_docs.Clear()
                vres = db_document.read_analytic_documents(Me.id, MyBase.certificate)
                If IsArray(vres) Then
                    For i = 0 To UBound(vres, 2)
                        ld = New bc_om_document
                        ld.id = vres(0, i)
                        ld.title = vres(1, i)
                        ld.doc_date = vres(2, i)
                        ld.original_extension = vres(3, i)
                        ld.filename = vres(4, i)
                        ld.pub_type_name = vres(5, i)
                        ld.extension = vres(6, i)
                        analytic_docs.Add(ld)
                    Next
                End If
                vres = db_document.read_table_on_demand(Me.id, MyBase.certificate)
                If IsArray(vres) Then
                    If UBound(vres, 2) = 0 Then
                        Me.tables_on_Demand = vres(0, 0)
                    End If
                End If

                REM disclosure file
                Dim vdf As Object
                vdf = db_document.read_disclosure_file_name(Me.pub_type_id, Me.id, MyBase.certificate)
                If IsArray(vdf) Then
                    If UBound(vdf, 2) = 0 Then
                        disclosure_file = vdf(0, 0)
                        REM SEPT 2018
                        disclosure_file_title = vdf(1, 0)
                        last_disclosure_filename = vdf(2, 0)

                    End If
                End If

                Dim asres As Object
                asres = db_document.read_alt_process_summary_text(Me.id, MyBase.certificate)
                If IsArray(asres) Then
                    If UBound(asres, 2) = 0 Then
                        alternate_process_summary_display = asres(0, 0)

                    End If
            End If
            Dim vch As Object
            Dim ch As bc_om_distribution_channel
            vch = db_document.read_channels_for_documents(Me.id, MyBase.certificate)
            If IsArray(vch) Then
                For i = 0 To UBound(vch, 2)
                    ch = New bc_om_distribution_channel
                    ch.channel_id = vch(0, i)
                    ch.name = vch(1, i)
                    ch.type = vch(2, i)
                    Me.distribution_channels.Add(ch)
                Next
            End If

            Catch ex As Exception
                Dim db_err As New bc_cs_error_log("bc_om_document", "db_read_for_process", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            Finally

                otrace = New bc_cs_activity_log("bc_om_document", "db_read_for_process", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
            End Try
        End Sub
        Public Sub db_read_support_doc(Optional ByVal full_read As Boolean = True)
            Dim otrace As New bc_cs_activity_log("bc_om_document", "db_read_support_doc", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
            Try
                Dim db_document As New bc_om_document_db
                Dim vresults As Object
                Dim otaxonomy As bc_om_taxonomy
                Dim oauthor As bc_om_user
                Dim i As Integer

                REM  document
                vresults = db_document.read_document(id, MyBase.certificate)

                If IsArray(vresults) Then
                    Me.pub_type_id = vresults(1, 0)
                    Me.title = vresults(2, 0)
                    Me.summary = vresults(3, 0)
                    Me.doc_date = vresults(4, 0)
                    Me.pages = vresults(5, 0)
                    Me.language_id = vresults(6, 0)
                    Me.filename = vresults(8, 0)
                    Me.checked_out_user = vresults(9, 0)
                    Me.extension = vresults(10, 0)
                    Me.pub_type_name = vresults(11, 0)
                    Me.stage = vresults(14, 0)
                    Me.sub_title = vresults(13, 0)
                    Me.stage_name = vresults(12, 0)
                    Me.bus_area = vresults(15, 0)
                    Me.master_container_id = vresults(18, 0)
                    Me.refresh_components.workflow_state = vresults(16, 0)
                    Me.refresh_components.accounting_standard = vresults(17, 0)
                    Me.refresh_components.data_at_date = CStr(vresults(19, 0))
                    Me.stage_expire_date = CDate(vresults(20, 0))
                    Me.originating_author = vresults(22, 0)
                    Me.main_note = vresults(23, 0)
                    Me.entity_id = CLng(vresults(24, 0))
                    Me.teaser_text = vresults(25, 0)
                    Me.update_date = vresults(26, 0)
                    Me.create_date = vresults(27, 0)
                    Me.urgent_flag = vresults(29, 0)
                    Me.urgent_text = vresults(30, 0)
                    Me.behalf_of_author_id = vresults(31, 0)
                    Try
                        Me.summary_html = vresults(34, 0)
                        Me.custom_title = vresults(35, 0)
                        Me.custom_title2 = vresults(36, 0)
                    Catch ex As Exception

                    End Try
                End If

                If full_read = False Then
                    Exit Sub
                End If
                REM read in taxonomy
                taxonomy.Clear()
                vresults = db_document.read_taxonomy_for_document(id, MyBase.certificate)
                If IsArray(vresults) Then
                    For i = 0 To UBound(vresults, 2)
                        otaxonomy = New bc_om_taxonomy(vresults(0, i))
                        otaxonomy.class_id = vresults(1, i)
                        otaxonomy.name = vresults(2, i)
                        otaxonomy.class_name = vresults(3, i)
                        taxonomy.Add(otaxonomy)
                    Next
                End If


                REM read in authors
                authors.Clear()
                vresults = db_document.read_authors_for_document(id, MyBase.certificate)
                If IsArray(vresults) Then
                    For i = 0 To UBound(vresults, 2)
                        oauthor = New bc_om_user(vresults(0, i), "", "", "", 0, "", "", "", MyBase.certificate)
                        authors.Add(oauthor)
                    Next
                End If
                disclosures.Clear()
                vresults = db_document.read_disclosures_for_document(id, MyBase.certificate)
                If IsArray(vresults) Then
                    For i = 0 To UBound(vresults, 2)
                        otaxonomy = New bc_om_taxonomy(vresults(0, i))
                        disclosures.Add(otaxonomy)
                    Next
                End If

            Catch ex As Exception
                Dim db_err As New bc_cs_error_log("bc_om_document", "db_read_support_doc", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            Finally
                otrace = New bc_cs_activity_log("bc_om_document", "db_read_support_doc", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
            End Try
        End Sub

        Private Function revert_file(ByVal fn As String) As String
            REM takes a revision of current document
            Dim otrace As New bc_cs_activity_log("bc_om_document", "c", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
            Try
                Dim fs As New bc_cs_file_transfer_services
                Dim ocommentary As bc_cs_activity_log
                Dim tx As String
                Dim gdbc As New bc_om_document_db
                Dim newfn As String
                Dim newext As String


                newfn = Left(fn, InStr(fn, "_") - 1)
                newext = Right(fn, Len(fn) - InStrRev(fn, ".") + 1)
                If InStr(fn, "[imp]") > 0 Then
                    newext = "[imp]" + newext
                End If
                newfn = newfn + newext

                revert_file = False
                REM keep trying until find a slot
                REM check server copy exists of revert file
                If fs.check_document_exists(bc_cs_central_settings.central_repos_path + Me.filename, MyBase.certificate) = False Then
                    ocommentary = New bc_cs_activity_log("bc_om_document", "revert", bc_cs_activity_codes.COMMENTARY, "Revision file: " + fn + " does not exist on server.", certificate)
                    Exit Function
                End If

                fs.file_copy(bc_cs_central_settings.central_repos_path + fn, bc_cs_central_settings.central_repos_path + newfn)

                REM update extension
                Dim db_document As New bc_om_document_db
                db_document.update_filename(Me.id, newext, newfn, False, MyBase.certificate)
                tx = "Document: " + Me.filename + " Reverted to file: " + fn + " and renamed to " + newfn
                gdbc.write_history(Me.id, tx, "RF", Me.stage, Me.stage, MyBase.certificate)
                revert_file = True
            Catch ex As Exception
                Dim db_err As New bc_cs_error_log("bc_om_document", "revert", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
                revert_file = False
            Finally
                otrace = New bc_cs_activity_log("bc_om_document", "revert", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
            End Try
        End Function
        'Public Sub xdb_read()
        '    Dim otrace As New bc_cs_activity_log("bc_om_document", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        '    Try
        '        Dim db_document As New bc_om_document_db
        '        Dim vresults As Object
        '        Dim otaxonomy As bc_om_taxonomy
        '        Dim oauthor As bc_om_user
        '        Dim i As Integer
        '        Dim vaccess As Object
        '        Dim original_check_out_user As String = ""
        '        Dim original_stage_Id As Long
        '        Dim input_checked_out_user As String
        '        Dim fs As New bc_cs_file_transfer_services


        '        vresults = db_document.read_document(id, MyBase.certificate)

        '        If IsArray(vresults) Then
        '            Me.pub_type_id = vresults(1, 0)
        '            Me.title = vresults(2, 0)
        '            Me.summary = vresults(3, 0)
        '            Me.doc_date = vresults(4, 0)
        '            Me.pages = vresults(5, 0)
        '            Me.connection_method = bc_cs_central_settings.LOCAL
        '            Me.language_id = vresults(6, 0)
        '            Me.filename = vresults(8, 0)
        '            Me.checked_out_user = vresults(9, 0)
        '            Me.extension = vresults(10, 0)
        '            Me.pub_type_name = vresults(11, 0)
        '            Me.stage = vresults(14, 0)
        '            Me.sub_title = vresults(13, 0)
        '            Me.stage_name = vresults(12, 0)
        '            Me.bus_area = vresults(15, 0)
        '            Me.master_container_id = vresults(18, 0)
        '            Me.refresh_components.workflow_state = vresults(16, 0)
        '            Me.refresh_components.accounting_standard = vresults(17, 0)
        '            Me.refresh_components.data_at_date = CStr(vresults(19, 0))
        '            Me.stage_expire_date = CDate(vresults(20, 0))
        '            Me.originating_author = vresults(22, 0)

        '            Me.main_note = vresults(23, 0)
        '            Me.entity_id = CInt(vresults(24, 0))
        '            Me.teaser_text = vresults(25, 0)
        '            Me.update_date = vresults(26, 0)
        '            Me.create_date = vresults(27, 0)
        '        End If

        '        REM read most recent comment
        '        If master_flag = True Then

        '            REM access rights
        '            vaccess = db_document.get_access_rights(Me.stage, MyBase.certificate)
        '            Me.force_check_in = False
        '            If IsArray(vaccess) Then
        '                For i = 0 To UBound(vaccess, 2)
        '                    If vaccess(0, i) = "M" Then
        '                        Me.move = True
        '                    End If
        '                    If vaccess(0, i) = "R" Then
        '                        Me.read = True
        '                    End If
        '                    If vaccess(0, i) = "W" Then
        '                        Me.write = True
        '                    End If
        '                    If vaccess(0, i) = "L" Then
        '                        Me.locked = True
        '                    End If
        '                    If vaccess(0, i) = "E" Then
        '                        Me.force_check_in = True
        '                    End If
        '                Next
        '            End If

        '            Dim bc_om_stage As New bc_om_workflow_stage
        '            bc_om_stage.certificate = MyBase.certificate
        '            bc_om_stage.stage_id = Me.stage
        '            bc_om_stage.stage_name = Me.stage_name
        '            Me.workflow_stages.stages.Clear()
        '            Me.workflow_stages.stages.Add(bc_om_stage)
        '            Me.workflow_stages.db_read(Me.master_container_id, Me.stage, False, Me.pub_type_id, Me.id, MyBase.certificate)
        '            REM now also get totally reading for pub type
        '            pub_type_workflow = New bc_om_pub_type_workflow
        '            pub_type_workflow.certificate = MyBase.certificate
        '            pub_type_workflow.id = Me.pub_type_id
        '            pub_type_workflow.current_stage = Me.stage
        '            pub_type_workflow.load()
        '            '==========================================================================================
        '            'REM check if distribution list generator is installed
        '            'REM turn this stuff of for now as not sure if it is used
        '            ''Me.mailing_list_installed = db_document.is_generate_dist_list_installed(MyBase.certificate)
        '            'REM manadatory fields
        '            'mandatory_fields = New bc_om_mandatory_fields(Me.id)
        '            'mandatory_fields.db_read(MyBase.certificate)
        '            'REM entity classify filter
        '            'REM I am not sure what this does but I will leave in for now
        '            'entity_classify_filters = New bc_om_entity_classify_filters(Me.id)
        '            'entity_classify_filters.db_read(MyBase.certificate)

        '            'REM evaluate wether to show taxonomy or not
        '            'Dim user_db As New bc_om_user_db
        '            'Me.do_not_show_taxonomy = user_db.do_not_display_taxonomy(certificate.user_id, MyBase.certificate)
        '            '==========================================================================================

        '        End If



        '        REM check out document
        '        original_stage_Id = Me.stage
        '        input_checked_out_user = Me.checked_out_user

        '        REM read in taxonomy
        '        taxonomy.Clear()
        '        vresults = db_document.read_taxonomy_for_document(id, MyBase.certificate)
        '        If IsArray(vresults) Then
        '            For i = 0 To UBound(vresults, 2)
        '                otaxonomy = New bc_om_taxonomy(vresults(0, i))
        '                otaxonomy.class_id = vresults(1, i)
        '                otaxonomy.name = vresults(2, i)
        '                otaxonomy.class_name = vresults(3, i)

        '            Next
        '        End If


        '        REM read in authors
        '        authors.Clear()
        '        vresults = db_document.read_authors_for_document(id, MyBase.certificate)
        '        If IsArray(vresults) Then
        '            For i = 0 To UBound(vresults, 2)

        '                oauthor = New bc_om_user(vresults(0, i), "", "", "", 0, "", "", "", MyBase.certificate)
        '                authors.Add(oauthor)
        '            Next
        '        End If
        '        REM read in disclsoures
        '        disclosures.Clear()
        '        vresults = db_document.read_disclosures_for_document(id, MyBase.certificate)
        '        If IsArray(vresults) Then
        '            For i = 0 To UBound(vresults, 2)
        '                otaxonomy = New bc_om_taxonomy(vresults(0, i))
        '                disclosures.Add(otaxonomy)
        '            Next
        '        End If


        '        REM write document to byte stream
        '        If bwith_document = True Then
        '            REM only needed from create now
        '            Me.refresh_components.refresh_components.Clear()
        '            Me.refresh_components.db_read_metadata(Me.id, MyBase.certificate)
        '            Me.doc_not_found = False

        '            If fs.write_document_to_bytestream(bc_cs_central_settings.central_repos_path + Me.filename, Me.byteDoc, MyBase.certificate, True) = False Then
        '                Me.doc_not_found = True
        '            End If

        '            Me.support_documents.document.Clear()

        '            If Me.doc_not_found = False Then
        '                If bcheck_out = True Then
        '                    Dim support_text As String = ""

        '                    If Me.master_flag = False Then
        '                        support_text = Me.title
        '                    End If
        '                    original_check_out_user = Me.checked_out_user
        '                    REM can only check out if not alrady checked out
        '                    If original_stage_Id <> Me.stage Then
        '                        Me.id = -1
        '                        Exit Sub
        '                    End If
        '                    'If Me.checked_out_user = "0" Or Me.checked_out_user = "" Then
        '                    Me.checked_out_user = db_document.check_out_document(id, Me.stage, MyBase.certificate)

        '                End If
        '                If btake_revision = True Then
        '                    REM take revision of file only for master docs
        '                    If Me.master_flag = True Then
        '                        Me.take_revision()
        '                    End If
        '                End If
        '            End If


        '            'vdocs = db_document.get_docs_for_container(Me.master_container_id, certificate)
        '            'Me.support_documents.document.Clear()
        '            'If IsArray(vdocs) Then
        '            '    For i = 0 To UBound(vdocs, 2)
        '            '        REM for each support read in metadata for it
        '            '        If vdocs(0, i) <> Me.id Then
        '            '            Dim osupport As New bc_om_document
        '            '            osupport.certificate = Me.certificate
        '            '            osupport.id = vdocs(0, i)
        '            '            osupport.stage = Me.stage
        '            '            osupport.bwith_document = False
        '            '            osupport.btake_revision = False
        '            '            osupport.bcheck_out = False
        '            '            osupport.db_read()
        '            '            osupport.support_doc_state = 0
        '            '            Me.support_documents.document.Add(osupport)
        '            '        End If
        '            '    Next
        '            'End If
        '        Else
        '            If Me.master_flag = True Then
        '                REM support documents
        '                Dim vdocs As Object
        '                Me.support_documents.document.Clear()
        '                vdocs = db_document.get_docs_for_container(Me.master_container_id, certificate)
        '                Me.support_documents.document.Clear()
        '                If IsArray(vdocs) Then
        '                    For i = 0 To UBound(vdocs, 2)
        '                        REM for each support read in metadata for it
        '                        If vdocs(0, i) <> Me.id Then
        '                            Dim osupport As New bc_om_document
        '                            osupport.certificate = Me.certificate
        '                            osupport.id = vdocs(0, i)
        '                            osupport.stage = Me.stage
        '                            osupport.bwith_document = False
        '                            osupport.btake_revision = False
        '                            osupport.bcheck_out = False
        '                            osupport.db_read()
        '                            Me.support_documents.document.Add(osupport)
        '                        End If
        '                    Next
        '                End If
        '            End If
        '        End If


        '        REM translated docs
        '        REM copies document has been translated to
        '        Dim vtrans As Object
        '        Me.sub_translated_docs = ""
        '        vtrans = db_document.get_master_translated_doc_text(Me.id, MyBase.certificate)
        '        If IsArray(vtrans) Then
        '            If UBound(vtrans, 2) >= 0 Then
        '                For i = 0 To UBound(vtrans, 2)
        '                    If i > 0 Then
        '                        Me.sub_translated_docs = Me.sub_translated_docs + "; "
        '                    End If
        '                    Me.sub_translated_docs = Me.sub_translated_docs + CStr(vtrans(0, i))
        '                Next
        '            End If
        '        End If
        '        REM doc tranlated from
        '        Me.master_translated_doc = ""
        '        Dim vtransm As Object
        '        vtransm = db_document.get_sub_doc_text(Me.id, MyBase.certificate)
        '        If IsArray(vtransm) Then
        '            If UBound(vtransm, 2) >= 0 Then
        '                Me.master_translated_doc = vtransm(0, 0)
        '                Me.translated_from_doc = CLng(vtransm(1, 0))
        '                Me.title = Me.title
        '                Me.copy_doc_text = "(Copied from: " + CStr(vtransm(0, 0)) + " type: " + Trim(CStr(vtransm(4, 0))) + ")"
        '            End If
        '        End If
        '        REM handling chnages if screen is no longer up to date
        '        REM if document is not is stage requested document is in flag this
        '        If original_stage_Id <> Me.stage Then
        '            Me.id = -1
        '            Exit Sub
        '        End If
        '        REM if a check out is requested but doc is already checked out flag this
        '        If Me.bcheck_out = True And original_check_out_user <> "0" And original_check_out_user <> input_checked_out_user And original_check_out_user <> "" Then
        '            Me.id = 0
        '            Exit Sub
        '        Else
        '            If Me.checked_out_user <> "0" And Me.bcheck_out = False And original_check_out_user <> input_checked_out_user Then
        '                Me.id = 0
        '                Exit Sub
        '            End If
        '        End If


        '    Catch ex As Exception
        '        Dim db_err As New bc_cs_error_log("bc_om_document", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        '    Finally
        '        otrace = New bc_cs_activity_log("bc_om_document", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        '    End Try
        'End Sub
        Private Sub clear_down_data_not_needed()
            Me.support_documents.Clear()
            Me.history.Clear()
            Me.links.Clear()
            Me.comments.comments.Clear()
            Me.byteDoc = Nothing
        End Sub
        Public Sub db_check_out_create_doc()
            Dim otrace As New bc_cs_activity_log("bc_om_document", "db_check_out_create_doc", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
            Try
                clear_down_data_not_needed()
                Dim db_document As New bc_om_document_db
                Dim vresults As Object
                Dim otaxonomy As bc_om_taxonomy
                Dim oauthor As bc_om_user
                Dim i As Integer

                Dim original_check_out_user As String = ""
                Dim original_stage_Id As Long
                Dim input_checked_out_user As String
                Dim fs As New bc_cs_file_transfer_services

                REM check out document
                original_stage_Id = Me.stage
                input_checked_out_user = Me.checked_out_user


                vresults = db_document.read_document(id, MyBase.certificate)

                If IsArray(vresults) Then
                    Me.pub_type_id = vresults(1, 0)
                    Me.title = vresults(2, 0)
                    Me.summary = vresults(3, 0)
                    Me.doc_date = vresults(4, 0)
                    Me.pages = vresults(5, 0)
                    Me.connection_method = bc_cs_central_settings.LOCAL
                    Me.language_id = vresults(6, 0)
                    Me.filename = vresults(8, 0)
                    Me.checked_out_user = vresults(9, 0)
                    Me.extension = vresults(10, 0)
                    Me.pub_type_name = vresults(11, 0)
                    Me.stage = vresults(14, 0)
                    Me.sub_title = vresults(13, 0)
                    Me.stage_name = vresults(12, 0)
                    Me.bus_area = vresults(15, 0)
                    Me.master_container_id = vresults(18, 0)
                    Me.refresh_components.workflow_state = vresults(16, 0)
                    Me.refresh_components.accounting_standard = vresults(17, 0)
                    Me.refresh_components.data_at_date = CStr(vresults(19, 0))
                    Me.stage_expire_date = CDate(vresults(20, 0))
                    Me.originating_author = vresults(22, 0)

                    Me.main_note = vresults(23, 0)
                    Me.entity_id = CLng(vresults(24, 0))
                    Me.teaser_text = vresults(25, 0)
                    Me.update_date = vresults(26, 0)
                    Me.create_date = vresults(27, 0)
                    Me.urgent_flag = vresults(29, 0)
                    Me.urgent_text = vresults(30, 0)
                    Me.behalf_of_author_id = vresults(31, 0)
                    Try
                        Me.summary_html = vresults(34, 0)
                        Me.custom_title = vresults(35, 0)
                        Me.custom_title2 = vresults(36, 0)
                    Catch ex As Exception

                    End Try
                End If


                REM handling chnages if screen is no longer up to date
                REM if document is not is stage requested document is in flag this
                If original_stage_Id <> Me.stage Then
                    Me.id = -1
                    Exit Sub
                End If
                REM if a check out is requested but doc is already checked out flag this
                If Me.checked_out_user <> "0" Then
                    Me.id = 0
                    Exit Sub
                End If

                Dim bc_om_stage As New bc_om_workflow_stage
                bc_om_stage.certificate = MyBase.certificate
                bc_om_stage.stage_id = Me.stage
                bc_om_stage.stage_name = Me.stage_name
                Me.workflow_stages.stages.Clear()
                Me.workflow_stages.stages.Add(bc_om_stage)
                Me.workflow_stages.db_read(Me.master_container_id, Me.stage, False, Me.pub_type_id, Me.id, MyBase.certificate)
                REM now also get totally reading for pub type
                pub_type_workflow = New bc_om_pub_type_workflow
                pub_type_workflow.certificate = MyBase.certificate
                pub_type_workflow.id = Me.pub_type_id
                pub_type_workflow.current_stage = Me.stage
                pub_type_workflow.load()



                REM read in taxonomy
                taxonomy.Clear()
                vresults = db_document.read_taxonomy_for_document(id, MyBase.certificate)
                If IsArray(vresults) Then
                    For i = 0 To UBound(vresults, 2)
                        otaxonomy = New bc_om_taxonomy(vresults(0, i))
                        otaxonomy.class_id = vresults(1, i)
                        otaxonomy.name = vresults(2, i)
                        otaxonomy.class_name = vresults(3, i)
                        taxonomy.Add(otaxonomy)
                    Next
                End If


                REM read in authors
                authors.Clear()
                vresults = db_document.read_authors_for_document(id, MyBase.certificate)
                If IsArray(vresults) Then
                    For i = 0 To UBound(vresults, 2)

                        oauthor = New bc_om_user(vresults(0, i), "", "", "", 0, "", "", "", MyBase.certificate)
                        authors.Add(oauthor)
                    Next
                End If
                REM read in disclsoures
                disclosures.Clear()
                vresults = db_document.read_disclosures_for_document(id, MyBase.certificate)
                If IsArray(vresults) Then
                    For i = 0 To UBound(vresults, 2)
                        otaxonomy = New bc_om_taxonomy(vresults(0, i))
                        disclosures.Add(otaxonomy)
                    Next
                End If

                REM only needed from create now
                Me.refresh_components.refresh_components.Clear()
                Me.refresh_components.db_read_metadata(Me.id, MyBase.certificate)
                Me.doc_not_found = False

                If fs.write_document_to_bytestream(bc_cs_central_settings.central_repos_path + Me.filename, Me.byteDoc, MyBase.certificate, False) = False Then
                    Me.doc_not_found = True
                    Exit Sub
                End If

                REM read support documents for display to create submit screen only
                vresults = db_document.read_support_documents_for_display(Me.id, MyBase.certificate)
                Dim sd As bc_om_document
                If IsArray(vresults) Then
                    For i = 0 To UBound(vresults, 2)
                        sd = New bc_om_document
                        sd.id = vresults(0, i)
                        sd.db_read_support_doc(True)
                        sd.filename = vresults(1, i)
                        sd.support_doc_display_only = True
                        Me.support_documents.Add(sd)
                        'sd.title = vresults(0, i)
                        'sd.pub_type_name = vresults(1, i)
                        'sd.extension = vresults(2, i)
                        'sd.filename = vresults(3, i)
                        'sd.support_doc_display_only = True
                        'Me.support_documents.document.Add(sd)
                    Next
                End If


                If Me.doc_not_found = False Then
                    original_check_out_user = Me.checked_out_user
                    REM can only check out if not alrady checked out
                    If original_stage_Id <> Me.stage Then
                        Me.id = -1
                        Exit Sub
                    End If

                    Me.checked_out_user = db_document.check_out_document(id, Me.stage, MyBase.certificate)

                    REM take revision of file only for master docs
                    Me.take_revision()
                End If

                Dim vres As Object
                Dim ld As bc_om_document
                linked_docs.Clear()
                vres = db_document.read_linked_documents(Me.id, MyBase.certificate)
                If IsArray(vres) Then
                    For i = 0 To UBound(vres, 2)
                        ld = New bc_om_document
                        ld.id = vres(0, i)
                        ld.title = vres(1, i)
                        ld.doc_date = vres(2, i)
                        ld.original_extension = vres(3, i)
                        ld.filename = vres(4, i)
                        ld.pub_type_name = vres(5, i)
                        ld.extension = vres(6, i)
                        linked_docs.Add(ld)
                    Next
                End If
                analytic_docs.Clear()
                vres = db_document.read_analytic_documents(Me.id, MyBase.certificate)
                If IsArray(vres) Then
                    For i = 0 To UBound(vres, 2)
                        ld = New bc_om_document
                        ld.id = vres(0, i)
                        ld.title = vres(1, i)
                        ld.doc_date = vres(2, i)
                        ld.original_extension = vres(3, i)
                        ld.filename = vres(4, i)
                        ld.pub_type_name = vres(5, i)
                        ld.extension = vres(6, i)
                        analytic_docs.Add(ld)
                    Next
                End If
                Dim vaccess As Object
                vaccess = db_document.get_access_rights(Me.stage, MyBase.certificate)
            Me.allow_disclosures = False
            Me.allow_dist_channels = False

                If IsArray(vaccess) Then
                    For i = 0 To UBound(vaccess, 2)
                        If vaccess(0, i) = "D" Then
                            Me.allow_disclosures = True
                    End If
                    If vaccess(0, i) = "C" Then
                        Me.allow_dist_channels = True
                    End If
                    Next
                End If
            Dim vch As Object
            Dim ch As bc_om_distribution_channel
            vch = db_document.read_channels_for_documents(Me.id, MyBase.certificate)
            If IsArray(vch) Then
                For i = 0 To UBound(vch, 2)
                    ch = New bc_om_distribution_channel
                    ch.channel_id = vch(0, i)
                    ch.name = vch(1, i)
                    ch.type = vch(2, i)
                    Me.distribution_channels.Add(ch)
                Next
            End If

            Catch ex As Exception
                Dim db_err As New bc_cs_error_log("bc_om_document", "db_check_out_create_doc", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            Finally
                otrace = New bc_cs_activity_log("bc_om_document", "db_check_out_create_doc", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
            End Try
        End Sub
        Public Sub db_read_for_create()
            Dim otrace As New bc_cs_activity_log("bc_om_document", "db_read_for_create", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
            Try
                Dim db_document As New bc_om_document_db
                Dim vresults As Object
                Dim otaxonomy As bc_om_taxonomy
                Dim oauthor As bc_om_user
                Dim i As Integer
                Dim original_check_out_user As String = ""
                Dim fs As New bc_cs_file_transfer_services


                vresults = db_document.read_document(id, MyBase.certificate)

                If IsArray(vresults) Then
                    Me.pub_type_id = vresults(1, 0)
                    Me.title = vresults(2, 0)
                    Me.summary = vresults(3, 0)
                    Me.doc_date = vresults(4, 0)
                    Me.pages = vresults(5, 0)
                    Me.connection_method = bc_cs_central_settings.LOCAL
                    Me.language_id = vresults(6, 0)
                    Me.filename = vresults(8, 0)
                    Me.checked_out_user = vresults(9, 0)
                    Me.extension = vresults(10, 0)
                    Me.pub_type_name = vresults(11, 0)
                    Me.stage = vresults(14, 0)
                    Me.sub_title = vresults(13, 0)
                    Me.stage_name = vresults(12, 0)
                    Me.bus_area = vresults(15, 0)
                    Me.master_container_id = vresults(18, 0)
                    Me.refresh_components.workflow_state = vresults(16, 0)
                    Me.refresh_components.accounting_standard = vresults(17, 0)
                    Me.refresh_components.data_at_date = CStr(vresults(19, 0))
                    Me.stage_expire_date = CDate(vresults(20, 0))
                    Me.originating_author = vresults(22, 0)

                    Me.main_note = vresults(23, 0)
                    Me.entity_id = CLng(vresults(24, 0))
                    Me.teaser_text = vresults(25, 0)
                    Me.update_date = vresults(26, 0)
                    Me.create_date = vresults(27, 0)
                    Me.urgent_flag = vresults(29, 0)
                    Me.urgent_text = vresults(30, 0)
                    Me.behalf_of_author_id = vresults(31, 0)
                    Try
                        Me.summary_html = vresults(34, 0)
                        Me.custom_title = vresults(35, 0)
                        Me.custom_title2 = vresults(36, 0)
                    Catch ex As Exception

                    End Try
                End If
                Dim bc_om_stage As New bc_om_workflow_stage
                bc_om_stage.certificate = MyBase.certificate
                bc_om_stage.stage_id = Me.stage
                bc_om_stage.stage_name = Me.stage_name
                Me.workflow_stages.stages.Clear()
                Me.workflow_stages.stages.Add(bc_om_stage)
                Me.workflow_stages.db_read(Me.master_container_id, Me.stage, False, Me.pub_type_id, Me.id, MyBase.certificate)
                REM now also get totally reading for pub type
                pub_type_workflow = New bc_om_pub_type_workflow
                pub_type_workflow.certificate = MyBase.certificate
                pub_type_workflow.id = Me.pub_type_id
                pub_type_workflow.current_stage = Me.stage
                pub_type_workflow.load()



                REM read in taxonomy
                taxonomy.Clear()
                vresults = db_document.read_taxonomy_for_document(id, MyBase.certificate)
                If IsArray(vresults) Then
                    For i = 0 To UBound(vresults, 2)
                        otaxonomy = New bc_om_taxonomy(vresults(0, i))
                        otaxonomy.class_id = vresults(1, i)
                        otaxonomy.name = vresults(2, i)
                        otaxonomy.class_name = vresults(3, i)
                        taxonomy.Add(otaxonomy)
                    Next
                End If


                REM read in authors
                authors.Clear()
                vresults = db_document.read_authors_for_document(id, MyBase.certificate)
                If IsArray(vresults) Then
                    For i = 0 To UBound(vresults, 2)

                        oauthor = New bc_om_user(vresults(0, i), "", "", "", 0, "", "", "", MyBase.certificate)
                        authors.Add(oauthor)
                    Next
                End If
                REM read in disclsoures
                disclosures.Clear()
                vresults = db_document.read_disclosures_for_document(id, MyBase.certificate)
                If IsArray(vresults) Then
                    For i = 0 To UBound(vresults, 2)
                        otaxonomy = New bc_om_taxonomy(vresults(0, i))
                        disclosures.Add(otaxonomy)
                    Next
                End If


                Me.refresh_components.refresh_components.Clear()
                Me.refresh_components.db_read_metadata(Me.id, MyBase.certificate)


                REM support documents

                Me.support_documents.Clear()
                Dim vdocs As Object
                Me.support_documents.Clear()
                vdocs = db_document.get_docs_for_container(Me.master_container_id, certificate)
                Me.support_documents.Clear()
                If IsArray(vdocs) Then
                    For i = 0 To UBound(vdocs, 2)
                        REM for each support read in metadata for it
                        If vdocs(0, i) <> Me.id Then
                            Dim osupport As New bc_om_document
                            osupport.certificate = Me.certificate
                            osupport.id = vdocs(0, i)
                            osupport.stage = Me.stage
                            osupport.db_read_support_doc(False)
                            osupport.support_doc_display_only = True
                            Me.support_documents.Add(osupport)
                        End If
                    Next
                End If


                REM write document to byte stream
                If bwith_document = True Then
                    REM only needed from create now
                    Me.doc_not_found = False

                    If fs.write_document_to_bytestream(bc_cs_central_settings.central_repos_path + Me.filename, Me.byteDoc, MyBase.certificate, True) = False Then
                        Me.doc_not_found = True
                    End If

                    Me.support_documents.Clear()
                End If
                Dim vres As Object
                Dim ld As bc_om_document
                linked_docs.Clear()
                vres = db_document.read_linked_documents(Me.id, MyBase.certificate)
                If IsArray(vres) Then
                    For i = 0 To UBound(vres, 2)
                        ld = New bc_om_document
                        ld.id = vres(0, i)
                        ld.title = vres(1, i)
                        ld.doc_date = vres(2, i)
                        ld.original_extension = vres(3, i)
                        ld.filename = vres(4, i)
                        ld.pub_type_name = vres(5, i)
                        ld.extension = vres(6, i)
                        linked_docs.Add(ld)
                    Next
                End If
                analytic_docs.Clear()
                vres = db_document.read_analytic_documents(Me.id, MyBase.certificate)
                If IsArray(vres) Then
                    For i = 0 To UBound(vres, 2)
                        ld = New bc_om_document
                        ld.id = vres(0, i)
                        ld.title = vres(1, i)
                        ld.doc_date = vres(2, i)
                        ld.original_extension = vres(3, i)
                        ld.filename = vres(4, i)
                        ld.pub_type_name = vres(5, i)
                        ld.extension = vres(6, i)
                        analytic_docs.Add(ld)
                    Next
                End If
                Dim vaccess As Object
                vaccess = db_document.get_access_rights(Me.stage, MyBase.certificate)
            Me.allow_disclosures = False
            Me.allow_dist_channels = False

                If IsArray(vaccess) Then
                    For i = 0 To UBound(vaccess, 2)
                        If vaccess(0, i) = "D" Then
                            Me.allow_disclosures = True
                    End If
                    If vaccess(0, i) = "C" Then
                        Me.allow_dist_channels = True
                    End If
                    Next
            End If
            Dim vch As Object
            Dim ch As bc_om_distribution_channel
            vch = db_document.read_channels_for_documents(Me.id, MyBase.certificate)
            If IsArray(vch) Then
                For i = 0 To UBound(vch, 2)
                    ch = New bc_om_distribution_channel
                    ch.channel_id = vch(0, i)
                    ch.name = vch(1, i)
                    ch.type = vch(2, i)
                    Me.distribution_channels.Add(ch)
                Next
            End If
            Catch ex As Exception
                Dim db_err As New bc_cs_error_log("bc_om_document", "db_read_for_create", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            Finally
                REM PR JAN 2016 added as recreate metadata is setting bwith_document to false
                REM so submission then doesnt send document
                bwith_document = True

                otrace = New bc_cs_activity_log("bc_om_document", "db_read_for_create", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
            End Try
        End Sub
        Public Sub db_read_for_filedetails(db As Object)
            Dim otrace As New bc_cs_activity_log("bc_om_document", "db_read_for_filedetails", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
            Try
                Dim db_document As New bc_om_document_db
                Dim vresults As Object

                vresults = db_document.read_document_trans(db, id, MyBase.certificate)

                If IsArray(vresults) Then
                    Me.pub_type_id = vresults(1, 0)
                    Me.title = vresults(2, 0)
                    Me.summary = vresults(3, 0)
                    Me.doc_date = vresults(4, 0)
                    Me.pages = vresults(5, 0)
                    Me.connection_method = bc_cs_central_settings.LOCAL
                    Me.language_id = vresults(6, 0)
                    Me.filename = vresults(8, 0)
                    Me.checked_out_user = vresults(9, 0)
                    Me.extension = vresults(10, 0)
                    Me.pub_type_name = vresults(11, 0)
                    Me.stage = vresults(14, 0)
                    Me.sub_title = vresults(13, 0)
                    Me.stage_name = vresults(12, 0)
                    Me.bus_area = vresults(15, 0)
                    Me.master_container_id = vresults(18, 0)
                    Me.refresh_components.workflow_state = vresults(16, 0)
                    Me.refresh_components.accounting_standard = vresults(17, 0)
                    Me.refresh_components.data_at_date = CStr(vresults(19, 0))
                    Me.stage_expire_date = CDate(vresults(20, 0))
                    Me.originating_author = vresults(22, 0)

                    Me.main_note = vresults(23, 0)
                    Me.entity_id = CLng(vresults(24, 0))
                    Me.teaser_text = vresults(25, 0)
                    Me.update_date = vresults(26, 0)
                    Me.create_date = vresults(27, 0)
                    Me.urgent_flag = vresults(29, 0)
                    Me.urgent_text = vresults(30, 0)
                    Me.behalf_of_author_id = vresults(31, 0)
                    Try
                        Me.summary_html = vresults(34, 0)
                        Me.custom_title = vresults(35, 0)
                        Me.custom_title2 = vresults(36, 0)
                    Catch ex As Exception

                    End Try
                End If



            Catch ex As Exception
                Dim db_err As New bc_cs_error_log("bc_om_document", "db_read_for_filedetailse", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            Finally
                otrace = New bc_cs_activity_log("bc_om_document", "db_read_for_filedetailse", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
            End Try
        End Sub
        Public Sub db_read_for_categorize()
            Dim otrace As New bc_cs_activity_log("bc_om_document", "db_read_for_categorize", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
            Try
                Dim db_document As New bc_om_document_db
                Dim vresults As Object
                Dim otaxonomy As bc_om_taxonomy
                Dim oauthor As bc_om_user
                Dim i As Integer
                Dim original_check_out_user As String = ""
                Dim fs As New bc_cs_file_transfer_services
                Dim tid As Long
                tid = id

                db_check_document_state()
                If Me.id <= 0 And Me.id <> CHECKED_IN Then
                    Exit Sub
                End If
                id = tid
                vresults = db_document.read_document(id, MyBase.certificate)

                If IsArray(vresults) Then
                    Me.pub_type_id = vresults(1, 0)
                    Me.title = vresults(2, 0)
                    Me.summary = vresults(3, 0)
                    Me.doc_date = vresults(4, 0)
                    Me.pages = vresults(5, 0)
                    Me.connection_method = bc_cs_central_settings.LOCAL
                    Me.language_id = vresults(6, 0)
                    Me.filename = vresults(8, 0)
                    Me.checked_out_user = vresults(9, 0)
                    Me.extension = vresults(10, 0)
                    Me.pub_type_name = vresults(11, 0)
                    Me.stage = vresults(14, 0)
                    Me.sub_title = vresults(13, 0)
                    Me.stage_name = vresults(12, 0)
                    Me.bus_area = vresults(15, 0)
                    Me.master_container_id = vresults(18, 0)
                    Me.refresh_components.workflow_state = vresults(16, 0)
                    Me.refresh_components.accounting_standard = vresults(17, 0)
                    Me.refresh_components.data_at_date = CStr(vresults(19, 0))
                    Me.stage_expire_date = CDate(vresults(20, 0))
                    Me.originating_author = vresults(22, 0)

                    Me.main_note = vresults(23, 0)
                    Me.entity_id = CLng(vresults(24, 0))
                    Me.teaser_text = vresults(25, 0)
                    Me.update_date = vresults(26, 0)
                    Me.create_date = vresults(27, 0)
                    Me.urgent_flag = vresults(29, 0)
                    Me.urgent_text = vresults(30, 0)
                    Me.behalf_of_author_id = vresults(31, 0)
                    Try
                        Me.summary_html = vresults(34, 0)
                        Me.custom_title = vresults(35, 0)
                        Me.custom_title2 = vresults(36, 0)
                    Catch ex As Exception

                    End Try
                End If
                Dim bc_om_stage As New bc_om_workflow_stage
                bc_om_stage.certificate = MyBase.certificate
                bc_om_stage.stage_id = Me.stage
                bc_om_stage.stage_name = Me.stage_name
                Me.workflow_stages.stages.Clear()
                Me.workflow_stages.stages.Add(bc_om_stage)
                Me.workflow_stages.db_read(Me.master_container_id, Me.stage, False, Me.pub_type_id, Me.id, MyBase.certificate)
                REM now also get totally reading for pub type
                pub_type_workflow = New bc_om_pub_type_workflow
                pub_type_workflow.certificate = MyBase.certificate
                pub_type_workflow.id = Me.pub_type_id
                pub_type_workflow.current_stage = Me.stage
                pub_type_workflow.load()



                REM read in taxonomy
                taxonomy.Clear()
                vresults = db_document.read_taxonomy_for_document(id, MyBase.certificate)
                If IsArray(vresults) Then
                    For i = 0 To UBound(vresults, 2)
                        otaxonomy = New bc_om_taxonomy(vresults(0, i))
                        otaxonomy.class_id = vresults(1, i)
                        otaxonomy.name = vresults(2, i)
                        otaxonomy.class_name = vresults(3, i)
                        taxonomy.Add(otaxonomy)
                    Next
                End If


                REM read in authors
                authors.Clear()
                vresults = db_document.read_authors_for_document(id, MyBase.certificate)
                If IsArray(vresults) Then
                    For i = 0 To UBound(vresults, 2)

                        oauthor = New bc_om_user(vresults(0, i), "", "", "", 0, "", "", "", MyBase.certificate)
                        authors.Add(oauthor)
                    Next
                End If
                REM read in disclsoures
                disclosures.Clear()
                vresults = db_document.read_disclosures_for_document(id, MyBase.certificate)
                If IsArray(vresults) Then
                    For i = 0 To UBound(vresults, 2)
                        otaxonomy = New bc_om_taxonomy(vresults(0, i))
                        disclosures.Add(otaxonomy)
                    Next
                End If

                Dim vres As Object
                Dim ld As bc_om_document
                linked_docs.Clear()
                vres = db_document.read_linked_documents(Me.id, MyBase.certificate)
                If IsArray(vres) Then
                    For i = 0 To UBound(vres, 2)
                        ld = New bc_om_document
                        ld.id = vres(0, i)
                        ld.title = vres(1, i)
                        ld.doc_date = vres(2, i)
                        ld.original_extension = vres(3, i)
                        ld.filename = vres(4, i)
                        ld.pub_type_name = vres(5, i)
                        ld.extension = vres(6, i)
                        linked_docs.Add(ld)
                    Next
                End If
                analytic_docs.Clear()
                vres = db_document.read_analytic_documents(Me.id, MyBase.certificate)
                If IsArray(vres) Then
                    For i = 0 To UBound(vres, 2)
                        ld = New bc_om_document
                        ld.id = vres(0, i)
                        ld.title = vres(1, i)
                        ld.doc_date = vres(2, i)
                        ld.original_extension = vres(3, i)
                        ld.filename = vres(4, i)
                        ld.pub_type_name = vres(5, i)
                        ld.extension = vres(6, i)
                        analytic_docs.Add(ld)
                    Next
                End If
                Dim vaccess As Object
                vaccess = db_document.get_access_rights(Me.stage, MyBase.certificate)
            Me.allow_disclosures = False
            Me.allow_dist_channels = False
                If IsArray(vaccess) Then
                    For i = 0 To UBound(vaccess, 2)
                        If vaccess(0, i) = "D" Then
                            Me.allow_disclosures = True
                    End If
                    If vaccess(0, i) = "C" Then
                        Me.allow_dist_channels = True
                    End If
                    Next
                End If

                REM disclosure file
                Dim vdf As Object
                vdf = db_document.read_disclosure_file_name(Me.pub_type_id, Me.id, MyBase.certificate)
                If IsArray(vdf) Then
                    If UBound(vdf, 2) = 0 Then
                        disclosure_file = vdf(0, 0)
                        REM SEPT 2018
                        disclosure_file_title = vdf(1, 0)
                        last_disclosure_filename = vdf(2, 0)

                    End If
            End If
            REM distribution files
            Dim vch As Object
            Dim ch As bc_om_distribution_channel

            vch = db_document.read_channels_for_documents(Me.id, MyBase.certificate)
            If IsArray(vch) Then
                For i = 0 To UBound(vch, 2)
                    ch = New bc_om_distribution_channel
                    ch.channel_id = vch(0, i)
                    ch.name = vch(1, i)
                    ch.type = vch(2, i)
                    Me.distribution_channels.Add(ch)
                Next
            End If



        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_document", "db_read_for_categorize", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_document", "db_read_for_categorize", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
        End Sub
        Public Function check_mandatory_fields(ByVal entities As bc_om_entities, ByVal pub_types As bc_om_pub_types, ByRef file_attach As Boolean) As String
            Dim otrace As New bc_cs_activity_log("bc_om_document", "check_mandatory_fields", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
            Try
                Dim i, j, k As Integer
                Dim ostr As New ArrayList
                Dim found As Boolean
                Dim class_name As String
                ostr.Clear()
                file_attach = True
                REM check fixed items first
                For i = 0 To Me.mandatory_fields.fields.Count - 1
                    If Me.mandatory_fields.fields(i).type_id = 0 Then
                        class_name = ""
                        found = False
                        For j = 0 To entities.entity.Count - 1
                            If entities.entity(j).class_id = Me.mandatory_fields.fields(i).class_id Then
                                class_name = entities.entity(j).class_name
                            End If
                            REM check to see document is calssified by at least one entity
                            REM of mandatory class
                            For k = 0 To Me.taxonomy.Count - 1
                                If Me.taxonomy(k).entity_id = entities.entity(j).id And entities.entity(j).class_id = Me.mandatory_fields.fields(i).class_id Then
                                    found = True
                                    Exit For
                                End If
                            Next
                            If found = True Then
                                Exit For
                            End If
                        Next
                        If found = False Then
                            ostr.Add(class_name)
                        End If
                    End If
                    If Me.mandatory_fields.fields(i).type_id = 1 Then
                        Select Case Me.mandatory_fields.fields(i).item_id
                            Case 1
                                If Me.sub_title = "" Or Me.sub_title = " " Then
                                    ostr.Add("Sub Title")
                                End If
                            Case 2
                                If Me.summary = "" Then
                                    ostr.Add("Summary")
                                End If
                            Case 3
                                If Me.pages = 0 Then
                                    ostr.Add("Number Of Pages")
                                End If
                            Case 4
                                If Me.language_id = -1 Then
                                    ostr.Add("Language")
                                End If
                            Case 5
                                If Me.extension = "" Then
                                    file_attach = False
                                End If
                        End Select
                    End If
                    If Me.mandatory_fields.fields(i).type_id = 2 Then
                        class_name = ""
                        found = False
                        For j = 0 To Me.list_items.list_items.Count - 1
                            If Me.list_items.list_items(j).list_id = Me.mandatory_fields.fields(i).class_id Then
                                found = True
                                Exit For
                            End If
                        Next
                        Dim m As Integer
                        If found = False Then
                            For j = 0 To pub_types.pubtype.Count - 1
                                If pub_types.pubtype(j).id = Me.pub_type_id Then
                                    For m = 0 To pub_types.pubtype(j).lists.count - 1
                                        If pub_types.pubtype(j).lists(m).id = Me.mandatory_fields.fields(i).class_id Then
                                            ostr.Add(pub_types.pubtype(j).lists(m).name)
                                            Exit For
                                        End If
                                    Next
                                End If
                            Next
                        End If
                    End If
                Next
                REM check entity class
                If ostr.Count > 0 Then
                    check_mandatory_fields = "The following fields are mandatory: "
                    For i = 0 To ostr.Count - 1
                        check_mandatory_fields = check_mandatory_fields + " " + ostr(i) + ";"
                    Next
                Else
                    check_mandatory_fields = ""
                End If

            Catch ex As Exception
                Dim db_err As New bc_cs_error_log("bc_om_document", "check_mandatory_fields", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
                check_mandatory_fields = ""
            Finally
                otrace = New bc_cs_activity_log("bc_om_document", "check_mandatory_fields", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
            End Try
        End Function
        Public Function take_revision(Optional ByRef db As bc_cs_db_services = Nothing, Optional ByVal ext As String = "") As Boolean
            REM takes a revision of current document
            Dim otrace As New bc_cs_activity_log("bc_om_document", "take_revision", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
            Try
                Me.last_revision_filename = ""
                Dim fs As New bc_cs_file_transfer_services
                Dim ocommentary As bc_cs_activity_log
                Dim fn As String = ""
                Dim i As Integer

                i = 0
                If ext = "" Then
                    fn = CStr(Me.id) + "_" + CStr(Me.stage) + "_" + CStr(i) + Me.extension
                Else
                    fn = CStr(Me.id) + "_" + CStr(Me.stage) + "_" + CStr(i) + ext
                End If
                REM keep trying until find a slot
                While fs.check_document_exists(bc_cs_central_settings.central_repos_path + fn, MyBase.certificate) = True
                    i = i + 1
                    If ext = "" Then
                        fn = CStr(Me.id) + "_" + CStr(Me.stage) + "_" + CStr(i) + Me.extension
                    Else
                        fn = CStr(Me.id) + "_" + CStr(Me.stage) + "_" + CStr(i) + ext
                    End If
                End While
                REM make file copy
                ocommentary = New bc_cs_activity_log("bc_om_document", "take_revision", bc_cs_activity_codes.COMMENTARY, "Taking Revision file: " + fn, certificate)

                If fs.file_copy(bc_cs_central_settings.central_repos_path + Me.filename, bc_cs_central_settings.central_repos_path + fn, True) <> 0 Then
                    Me.doc_write_success = False
                    Me.doc_write_error_text = "Failed to take revision of file: " + bc_cs_central_settings.central_repos_path + Me.filename + " to " + bc_cs_central_settings.central_repos_path + fn
                    Exit Function
                End If

                Dim tx As String
                tx = "Revision taken stage: " + CStr(Me.stage_name) + " file: " + fn

                If IsNothing(db) Then
                    Dim gdbc As New bc_om_document_db
                    gdbc.write_history(Me.id, tx, "RT", Me.stage, Me.stage, certificate)
                Else
                    Dim gdbc As New bc_om_document_trans_db
                    gdbc.write_history(db, Me.id, tx, "RT", Me.stage, Me.stage, certificate)

                End If
                take_revision = True
                Me.last_revision_filename = fn
            Catch ex As Exception
                Dim db_err As New bc_cs_error_log("bc_om_document", "take_revision", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            Finally
                otrace = New bc_cs_activity_log("bc_om_document", "take_revision", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
            End Try
        End Function




        <Serializable()> Public Class bc_om_comp_cons
            Public doc_id As Long
            Public ord As Integer

        End Class
        Class bc_om_linked_document
            Public doc_id As Long

            Public Sub New()

            End Sub
        End Class
        Class bc_om_analytic_document
            Public doc_id As Long

            Public Sub New()

            End Sub
        End Class
        <Serializable()> Public Class bc_om_document_write
            Inherits bc_cs_soap_base_class
            Public selected_items As New List(Of bc_om_selected_list_item)
            Public write_selected_items As Boolean
            Public id As Long
            Public entity_id As Long
            Public pub_type_id As Long
            Public title As String
            Public sub_title As String
            Public summary As String
            Public doc_date As Date
            Public pages As Integer
            Public language_id As Integer
            Public filename As String
            Public checked_out_user As String
            Public extension As String
            Public stage As String
            Public stage_name As String
            Public originating_author As Long
            Public bus_area As String
            Public master_flag As Boolean
            Public master_container_id As Long
            'Public brevision_mode As Integer
            'Public revision_filename As String
            Public bimport_support_only As Boolean
            'Public reject As Boolean
            'Public reject_from_stage As Long
            Public new_doc As Boolean
            Public financial_workflow_stage As Long
            REM actually document object
            '<NonSerialized()> Public docobject As Object

            REM workflow stage routes
            'Public workflow_stages As New bc_om_workflow


            REM authors
            Public authors As New ArrayList


            REM taxonomy
            Public taxonomy As New ArrayList

            REM defined Componnets
            Public refresh_components As New ArrayList

            Public linked_documents As New List(Of bc_om_linked_document)

        Public analytic_documents As New List(Of bc_om_analytic_document)

        Public distribution_channels As New List(Of bc_om_distribution_channel)


            REM disclosures
            Public disclosures As New ArrayList
            REM Byte Stream Representation of the actual document
            'Public byteDoc As Byte()

            REM support documents
            'Public support_documents As New bc_om_documents

            REM actions
            Public original_stage As Long
            Public original_stage_name As String
            Public action_Ids As New ArrayList
            REM history
            Public history As New ArrayList
            Public refresh_component As New ArrayList
            Public component_parameters As New ArrayList
            Public component_parameters_List_vals As New ArrayList

            Public main_note As String
            Public register_only As Boolean
            Public stage_expire_date As Date
            Public master_translated_doc As String
            Public sub_translated_docs As String
            Public translated_from_doc As Long
            Public copy_doc_text As String
            Public teaser_text As String
            Public translate_flag As Boolean
            Public approval_only As Boolean
            Public approved_by As Long

            Public composite_constiuents As New ArrayList
            'Public receive_translation As Boolean
            'Public uploaded_translated_components As Boolean

            Public revision_from_process As Boolean = False
            Public support_doc_state As Integer
            Public last_revision_filename As String
            <NonSerialized()> Public search_flag As Boolean
            REM new extensions to object model not in PC

            'Public generate_mailing_list As Boolean
            'Public isStagingDocument As Boolean
            'Public ParentDocumentId As String
            'Public isdeleted As Boolean
            'Public isnew As Boolean
            'Public mandatory_fields As New bc_om_mandatory_fields
            'Public entity_classify_filters As New bc_om_entity_classify_filters
            'Public server_side_events As New bc_om_server_side_events
            'Public server_side_events_failed As String
            'Public list_items As New bc_om_list_items
            'Public mailing_list_installed As Boolean = False
            Public register_template As Boolean = False
            'Public do_not_show_taxonomy As Boolean = False
            'Public auto_generate_taxonomy As Boolean = True
            Public original_extension As String
            Public do_force_check_in As Boolean = False
            Public registered_file_name As String
            Public set_approve As Boolean = False
            Public approve_stage As String
            Public approve_stage_name As String

            Public secondary_entity_id As Long
            Public stage_to As Long
            Public data_at_date As Date
            Public workflow_state As Long
            Public stage_change_comment As String
            REM Public has_mailing_list As Boolean
            REM Public mail_list_url As String
            'Public track_flag As Boolean
            'Public comments As New bc_om_comments
            Public urgent_flag As Boolean
            Public behalf_of_author_id As Long
            Public urgent_text As String

            Public rtf_text As String
            Public html_text As String
            Public custom_title As String
            Public custom_title2 As String
            Public Sub New()

            End Sub

            Public Overrides Function write_data_to_xml(ByRef certifiate As bc_cs_security.certificate) As String
                Dim otrace As New bc_cs_activity_log("bc_om_document_write", "write_data_to_xml", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
                Dim cbc_data_services As New bc_cs_data_services
                Dim ocommentary As New bc_cs_activity_log("bc_om_document_write", "write_data_to_xml", bc_cs_activity_codes.COMMENTARY, "Attempting to write XML string")

                write_data_to_xml = soap_serialize_object_to_xml(Me, Me.certificate)

                otrace = New bc_cs_activity_log("bc_om_document_write", "write_data_to_xml", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
            End Function
            Private Function soap_serialize_object_to_xml(ByVal o As Object, ByRef certificate As bc_cs_security.certificate, Optional ByVal packet_code As String = "") As String

                Dim otrace As New bc_cs_activity_log("bc_om_document_write", "soap_serialize_object_to_xml", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

                Dim bc_cs_central_settings As New bc_cs_central_settings
                Dim by() As Byte

                Try
                    Using ms As New MemoryStream

                        Dim extratype(6) As Type

                        extratype(0) = GetType(bc_om_user)
                        extratype(1) = GetType(bc_om_taxonomy)
                        extratype(2) = GetType(bc_om_history)
                        extratype(3) = GetType(bc_om_comp_cons)
                        extratype(4) = GetType(bc_om_refresh_component)
                        extratype(5) = GetType(component_parameter_for_write_down)
                        extratype(6) = GetType(component_parameter_list_val_for_write_down)

                        Dim sf As New XmlSerializer(o.GetType, extratype)

                        sf.Serialize(ms, o)

                        by = ms.ToArray()

                        ms.Close()

                        'xml serialization does not always encode apostrophe's.  This causes an issue when passing to SQL Server
                        soap_serialize_object_to_xml = Replace(System.Text.Encoding.UTF8.GetString(by), "'", "&#39;")

                    End Using

                Catch ex As Exception
                    Dim db_err As New bc_cs_error_log("bc_om_document_write", "soap_serialize_object_to_xml", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
                    soap_serialize_object_to_xml = ""
                Finally
                    otrace = New bc_cs_activity_log("bc_om_document_write", "soap_serialize_object_to_xml", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
                End Try

            End Function

            Protected Overrides Sub Finalize()
                MyBase.Finalize()
            End Sub
        End Class
        REM force check in
        Public Const F = 1
        REM register only
        Public Const R = 2
        REM stage change only
        Public Const SC = 3
        REM metadata only
        Public Const MO = 4
        REM document with stage change
        Public Const DS = 5
        REM document no stage change
        Public Const DN = 6
        REM reattach master
        Public Const RM = 7
        REM register document
        Public Const RD = 8
        REM new support
        Public Const SN = 9
        REM check in support
        Public Const SI = 10
        REM pending stage change with document
        Public Const PD = 11
        REM pending stage change only
        Public Const PS = 12

        REM write the history only
        Public Const HO = 13

        REM Steve Wooderson 24/10/2013 Delete support doc
        REM delete document
        Public Const DD = 14

        Public Const SUBMIT_STAGE_CHANGE_FAILURE = 14
        Public write_history As Boolean = False
        Public mode As Integer
        Public doc_write_success As Boolean
        Public doc_write_error_text As String
        Public from_stage_change As Boolean = False
        Public new_html_doc As Boolean = False
        Public style_components As bc_om_document_components
        REM userd by ajax only for flaging checking in and out for html docs

        Public Overrides Function transmit_to_server_and_receive(ByRef obj As Object, show_errors As Boolean) As Boolean

            If bc_cs_central_settings.use_rest_unboxed_post = False Then
                transmit_to_server_and_receive = MyBase.transmit_to_server_and_receive(obj, show_errors)
                Exit Function
            End If

            REM specific object via rest
            transmit_to_server_and_receive = False
            Try
                Dim ocomm As New bc_cs_activity_log("bc_om_document", "transmit_to_server_and_receive", bc_cs_activity_codes.COMMENTARY, "unboxed rest post start")
                obj.certificate.user_id = bc_cs_central_settings.logged_on_user_id
                docobject = Nothing

                Dim JsonSerializer = New JavaScriptSerializer
                JsonSerializer.MaxJsonLength = Int32.MaxValue
                Dim json As String
                Try
                    json = JsonSerializer.Serialize(obj)
                Catch ex As Exception
                    If show_errors = True Then
                        Dim oerr As New bc_cs_error_log("bc_om_document", "transmit_to_server_and_receive", bc_cs_error_codes.USER_DEFINED, "Error Sending Via Rest Post json serialization failed: " + ex.Message)
                    End If
                    Exit Function
                End Try

                json = "{ ""doc"": " + json + "}"

                ocomm = New bc_cs_activity_log("bc_cs_document unboxed", "transmit_to_server_and_receive", bc_cs_activity_codes.COMMENTARY, "Json size: " + CStr(json.Length), certificate)

                Dim rp As New bc_cs_ns_json_post(bc_cs_central_settings.soap_server + "Rest_document_object_tranmission", json)

                rp.send(certificate)
                If rp.err_text = "" Then
                    rp.response_text = Left(rp.response_text, Len(rp.response_text) - 1)
                    rp.response_text = Right(rp.response_text, Len(rp.response_text) - 5)

                    Dim ms As New MemoryStream(Encoding.Unicode.GetBytes(rp.response_text))
                    Dim serializer As New System.Runtime.Serialization.Json.DataContractJsonSerializer(obj.[GetType]())
                    obj = DirectCast(serializer.ReadObject(ms), bc_om_document)
                    If obj.certificate.server_errors.Count > 0 Then
                        obj.transmission_state = 1
                        If show_errors = True Then
                            Dim oerr As New bc_cs_error_log("bc_om_document", "transmit_to_server_and_receive", bc_cs_error_codes.USER_DEFINED, obj.certificate.server_errors(0))
                            Exit Function
                        End If

                    End If
                    ms.Close()
                    ms.Dispose()
                    transmit_to_server_and_receive = True
                Else
                    obj.transmission_state = 3
                    If show_errors = True Then
                        Dim oerr As New bc_cs_error_log("bc_om_document", "transmit_to_server_and_receive", bc_cs_error_codes.USER_DEFINED, "Error Sending Via Rest Post: " + rp.err_text)
                    End If
                    Exit Function
                End If

            Catch ex As Exception
                Dim oerr As New bc_cs_error_log("bc_om_document", "transmit_to_server_and_receive", bc_cs_error_codes.USER_DEFINED, ex.Message)

            Finally
                Dim ocomm As New bc_cs_activity_log("bc_om_document", "transmit_to_server_and_receive", bc_cs_activity_codes.COMMENTARY, "unboxed rest post end")

            End Try



        End Function




        Public Function flag_check_inout(checkin As Boolean) As String
            Try
                Dim db_document As New bc_om_document_trans_db
                db_document.check_in_out(Me.id, checkin, certificate)
                If certificate.server_errors.Count > 0 Then
                    flag_check_inout = certificate.server_errors(0)
                Else
                    flag_check_inout = ""
                End If
            Catch ex As Exception
                flag_check_inout = "error: " + ex.Message

            End Try

        End Function
        Public Function db_write(ByRef certificate As bc_cs_security.certificate) As Long
            Dim otrace As New bc_cs_activity_log("bc_om_document", "db_write_trans", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
            Dim html_email_error As Boolean = False
            Dim db As New bc_cs_db_services
            Dim db_document As New bc_om_document_trans_db

            Try
                doc_write_error_text = ""
                Dim run_events As Boolean = True

                REM flag if document is not checked out to you 
                REM or checked in or in the same stage


                REM flags depict write actions required
                Dim fwrite_metadata As Boolean = False
                Dim fwrite_doc As Boolean = False
                Dim fcheckin_doc As Boolean = False
                Dim fstage_change As Boolean = False

                REM checks document is still in correct stage to procees
                Dim tid As Long
                Dim tstage As Long
                tstage = stage
                If original_stage <> 0 Then
                    stage = original_stage
                End If

                tid = id
                db_check_document_state()


                REM force check in
                If mode = F Then
                    If Me.id = -4 Then
                        Me.doc_write_success = False
                        Me.doc_write_error_text = "Another user has just checked in document."
                    Else
                        db_document.force_check_in(tid, MyBase.certificate)
                        db_write = True
                        Me.id = tid
                        Me.doc_write_success = True
                    End If
                    Exit Function
                End If

                If Me.id <= 0 And Me.id <> CHECKED_IN Then
                    Me.doc_write_success = False
                    Select Case Me.id
                        Case 0
                            Me.doc_write_error_text = "Another user has just checked out the document."
                        Case -1
                            Me.doc_write_error_text = "Another user has just change the stage of the document."
                    End Select
                    Exit Function
                End If
                REM delete support document
                If mode = DD Then
                    db_document.remove_support_doc(tid, MyBase.certificate, Nothing)
                    db_write = True
                    Me.doc_write_success = True
                    Exit Function
                End If

                id = tid
                stage = tstage

                db.open_conn(certificate)
                db.success = True



                mode = 0
                If Me.id = 0 And filename <> "" Then
                    Me.local_filename = filename
                End If
                REM register only
                REM only called on a stage change on first submit that has client events
                REM if running in client synchronous mode
                If Me.register_only Then
                    If support_doc_non_open_doc_event = False Then
                        fwrite_metadata = True
                        run_events = False
                        mode = R
                    Else
                        REM support document attached on stage change eventbut doc not opened
                        fwrite_doc = False
                        fwrite_metadata = True
                        run_events = True
                        fcheckin_doc = True
                        mode = DS
                    End If
                End If

                If Me.register_template = True Then
                    history.Clear()
                    If filename <> "" Then
                        REM attach document
                        REM just requires a new extension and a new file written
                        Dim ext As String
                        If Not IsNothing(revision_filename) Then
                            ext = Right(revision_filename, Len(revision_filename) - InStrRev(revision_filename, ".") + 1)
                            If InStr(revision_filename, "[imp]") > 0 Then
                                ext = "[imp]" + ext
                            End If
                            If take_revision(db, ext) = False Then
                                Exit Function
                            End If
                        End If
                        mode = RM
                        filename = db_document.New_update_filename(db, id, extension, certificate)
                        fwrite_doc = True
                    Else
                        mode = RD
                        history.Clear()
                        fwrite_metadata = True
                    End If
                Else
                    If IsNumeric(Me.id) And Me.id > 0 And register_only = True And Me.support_documents.Count = 0 Then
                        REM document already exists checked in so stage change only required
                        run_events = True
                        fwrite_metadata = False
                        'bstage_change_only = True
                        fstage_change = True
                        fcheckin_doc = True
                        mode = SC
                    ElseIf mode <> R Then
                        If Me.bwith_document = True Then
                            fwrite_metadata = True
                            fwrite_doc = True
                            fcheckin_doc = True

                            If Me.original_stage <> Me.stage And Me.original_stage <> 0 Then
                                mode = DS
                            Else
                                mode = DN
                            End If
                        Else
                            If support_doc_non_open_doc_event = False Then
                                fwrite_doc = False
                                fwrite_metadata = True
                                fcheckin_doc = False
                                mode = MO
                                REM if support docs then this is just importing a new support doc
                                If Me.support_documents.Count And new_html_doc = False Then

                                    mode = SN
                                    fwrite_metadata = False
                                End If
                            End If
                        End If
                    End If
                End If


                If fwrite_metadata = True Or write_history = True Then
                    write_metadata(db, Me, True, Me.stage, db_document)
                End If
                If db.success = False Or doc_write_error_text <> "" Then
                    Exit Function
                End If


                REM approval
                If Me.set_approve = True Then
                    db_document.new_approve_doc(db, Me.id, Me.stage, Me.approve_stage, certificate)
                End If
                If db.success = False Then
                    Exit Function
                End If


                REM deal with support docs
                If mode <> R Or new_html_doc = True Then
                    For i = 0 To Me.support_documents.Count - 1
                        If Me.support_documents(i).support_doc_display_only = False Then
                            If write_metadata(db, Me.support_documents(i), False, Me.stage, db_document) = True Then
                                If write_physical_doc(db, Me.support_documents(i), db_document) = False Then
                                    db.success = False
                                    Exit Function
                                End If
                            Else
                                db.success = False
                                Exit Function
                            End If
                        ElseIf Me.support_documents(i).marked_for_delete = True Then
                            db_document.remove_support_doc(Me.support_documents(i).id, certificate, db)
                        ElseIf Me.support_documents(i).metadata_changed = True Then
                            If write_metadata(db, Me.support_documents(i), False, Me.stage, db_document) = False Then
                                db.success = False
                                Exit Function
                            End If
                        End If
                    Next
                End If
                REM only write document if no errors
                If fwrite_doc = True And mode <> R Then
                    If write_physical_doc(db, Me, db_document) = False Then
                        db.success = False
                        Exit Function
                    End If
                End If


                db_document.tmp_write_comment(db, Me.id, stage_change_comment, Me.assigned_user, certificate)


                If db.success = False Then
                    Exit Function
                End If
                REM all good so commit
                db_write = True
                db.close_conn(True, certificate)
                Dim ocomm As New bc_cs_activity_log("bc_om_document", "db_write_trans", bc_cs_activity_codes.COMMENTARY, "Submission Successful for doc:" + CStr(id), certificate)

                If fwrite_metadata = True Then

                    If html_generation() = False Then
                        html_email_error = True
                        Exit Function
                    End If
                End If

                REM server side events
                If (Me.master_flag = True And (mode = DS Or mode = SC) And pending_mode <> PS And pending_mode <> PD) Or Not IsNothing(style_components) Then
                    REM is stage change only then check document out 
                    REM so no one else can check the document out  whilst the actions execute
                    If mode = SC Then
                        db_document.check_out_document(Me.id, certificate)
                    End If
                    REM now run custom server side events
                    Dim ocss_events As New bc_om_wf_custom_events

                    If (Me.original_stage = 0) Then
                        ocss_events.instateactions = True
                    End If

                    ocss_events.certificate = MyBase.certificate
                    ocss_events.entity_id = Me.entity_id
                    ocss_events.doc_id = Me.id
                    ocss_events.pub_type_id = Me.pub_type_id
                    ocss_events.container_id = Me.master_container_id
                    ocss_events.stage_from = original_stage
                    ocss_events.stage_to = Me.stage
                    ocss_events.stage_name_from = Me.original_stage_name
                    ocss_events.stage_name_to = Me.stage_name
                    ocss_events.last_revision_filename = Me.last_revision_filename
                    ocss_events.current_filename = Me.filename
                    ocss_events.revision_from_process = Me.revision_from_process
                    ocss_events.cdoc = Me
                    If Me.component_componetize = True Then
                        ocss_events.refresh_components = Me.refresh_components
                        ocss_events.refresh_components.certificate = certificate
                    End If
                    If Not IsNothing(Me.style_components) Then
                        ocss_events.style_components = Me.style_components
                        ocss_events.style_components.doc_id = Me.id
                        ocss_events.style_components.certificate = certificate
                    End If

                    Me.byteDoc = Nothing


                    If ocss_events.run_events_trans = False Then

                        If original_stage = stage Then
                            Me.server_side_events_failed = "Inter State Event failed Execution: " + vbCrLf + ocss_events.err_text
                        Else
                            Me.server_side_events_failed = "Server Side Event failed document wont change stage: " + vbCrLf + ocss_events.err_text
                        End If
                        Me.no_send_back = False
                        'events_failed = True
                        Me.stage = Me.original_stage
                        If mode = DS Then
                            mode = DN
                        Else
                            mode = 0
                        End If
                        REM see if event was interstatial

                        REM if support docs were bought int with from client stage change events now
                        REM remove this
                        For i = 0 To Me.support_documents.Count - 1
                            If Me.support_documents(i).from_event = True Then
                                If db_document.remove_support_doc(Me.support_documents(i).id, certificate, Nothing) = True Then
                                    Dim fs As New bc_cs_file_transfer_services
                                    If fs.check_document_exists(bc_cs_central_settings.central_repos_path + Me.support_documents(i).filename) Then
                                        fs.delete_file(bc_cs_central_settings.central_repos_path + Me.support_documents(i).filename)
                                    End If
                                End If
                            End If

                        Next
                    ElseIf ocss_events.output_text <> "" Then
                        Me.no_send_back = False
                        Me.server_side_events_failed = ocss_events.output_text
                        If ocss_events.override_stage_id <> 0 Then
                            Me.stage = ocss_events.override_stage_id
                        End If


                    End If

                End If


                If Me.local_filename <> "" Then
                    Me.filename = Me.local_filename
                End If
                REM final update
                If pending_mode <> 0 Then
                    Me.mode = pending_mode
                    pending_mode = 0
                End If
                Dim da As New bc_om_date(stage_expire_date)

                db_document.update_and_audit(id, Me.stage, mode, "", stage_change_comment, da.write_data_to_xml(certificate), certificate)
                Me.doc_write_success = True

            Catch ex As Exception
                Dim err As New bc_cs_error_log("bc_om_document", "db_write_trans", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
                db.success = False

            Finally
                If html_email_error = False Then
                    If mode <> F And mode <> DD Then
                        If db.success = False Or doc_write_error_text <> "" Then
                            Me.doc_write_success = False

                            db.close_conn(False, certificate)
                            If from_stage_change = True Then
                                db_document.update_and_audit(id, Me.stage, SUBMIT_STAGE_CHANGE_FAILURE, doc_write_error_text, "", "", certificate)
                            End If
                            Dim ocomm As New bc_cs_activity_log("bc_om_document", "db_write_trans", bc_cs_activity_codes.COMMENTARY, "Submission failed for doc rollbacked:" + CStr(id), certificate)
                        End If
                    End If
                Else
                    Dim da As New bc_om_date(stage_expire_date)
                    If (original_stage = stage Or original_stage = 0) Then
                        db_document.update_and_audit(id, Me.stage, mode, "", stage_change_comment, da.write_data_to_xml(certificate), certificate)
                    Else
                        db_document.update_and_audit(id, Me.stage, SUBMIT_STAGE_CHANGE_FAILURE, ": Failed to generate HTML", "", "", certificate)
                    End If
                End If
                mode = 0
                If no_send_back = False Then
                    Me.docobject = Nothing
                    'Me.support_documents.Clear()
                End If
                otrace = New bc_cs_activity_log("bc_om_document", "db_write_trans", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
            End Try
        End Function
        Function html_generation() As Boolean

            REM HTML GENERATION
            Dim htmldb As New bc_cs_db_services
            Dim htmlsuccess As Boolean = False
            Dim htmlerror As String = ""
            Try

                Dim res As Object
                htmldb.open_conn(certificate)
                htmldb.success = True

                res = htmldb.executesql_trans("exec dbo.bc_core_get_email_template_id " + CStr(id), certificate, True)
                If htmldb.success = False Then
                    Dim ocomm = New bc_cs_activity_log("bc_om_document", "db_write", bc_cs_activity_codes.COMMENTARY, "Failed to find sp: bc_core_get_email_template_id", certificate)
                    htmldb.close_conn(True, certificate)
                    htmlsuccess = True
                Else
                    If IsArray(res) AndAlso UBound(res, 2) = 0 Then
                        Dim email_template_id As Integer
                        email_template_id = res(0, 0)
                        If email_template_id = 0 Then
                            Dim ocomm = New bc_cs_activity_log("bc_om_document", "db_write", bc_cs_activity_codes.COMMENTARY, "No Email Template for Product", certificate)
                            htmldb.close_conn(True, certificate)
                            htmlsuccess = True
                        Else
                            If bc_cs_central_settings.server_flag = 0 Then
                                certificate = New bc_cs_security.certificate
                            End If
                            Dim et As New bc_cs_email_preview_services(certificate, False)
                            Dim ocomm = New bc_cs_activity_log("bc_om_document", "db_write", bc_cs_activity_codes.COMMENTARY, "aaa", certificate)

                            If et.create_preview(0, email_template_id, id, 0, 0, htmldb) = False Then
                                ocomm = New bc_cs_activity_log("bc_om_document", "db_write", bc_cs_activity_codes.COMMENTARY, "bbb", certificate)
                                htmlerror = "failed to generate html file: " + et.err_txt
                            Else
                                ocomm = New bc_cs_activity_log("bc_om_document", "db_write", bc_cs_activity_codes.COMMENTARY, "ccc", certificate)
                                htmlsuccess = True
                            End If
                        End If
                    Else
                        htmlerror = "failed to generate html file no template_id configured."
                    End If
                End If

            Catch ex As Exception
                htmlerror = "Exception: " + ex.Message
            Finally
                If htmlsuccess = True Then
                    htmldb.close_conn(True, certificate)
                Else
                    Me.doc_write_error_text = "Inter State Event failed generate html : " + htmlerror
                    Me.doc_write_success = False
                    htmldb.close_conn(False, certificate)
                End If
                html_generation = htmlsuccess


            End Try

        End Function


        REM SW cope with office versions
        'extensionsize = 0
        'extensionsize = (Len(Me.extension) - (InStrRev(Me.extension, ".") - 1))


        'REM get current stage
        'If Me.new_doc = True Then
        '    current_stage = 1
        'End If
        'If Me.id <> 0 Then
        '    container_id = db_document.get_container_id_for_document(Me.id, MyBase.certificate)
        '    If Me.new_doc = False Then
        '        current_stage = db_document.get_current_stage(CStr(container_id(0, 0)), MyBase.certificate)
        '    End If
        'Else
        '    Me.new_doc = True
        'End If

        'bstage_change_only = False
        'run_events = True
        'If register_only = True Then
        '    run_events = False
        'End If
        'If IsNumeric(Me.id) And Me.id > 0 And register_only = True And Me.support_documents.document.Count = 0 Then
        '    REM document already exists checked in so stage change only required
        '    run_events = True
        '    bstage_change_only = True
        '    Dim ocommentary As New bc_cs_activity_log("bc_om_document", "db_write", bc_cs_activity_codes.COMMENTARY, "Document stage change only")
        'End If`

        'bnew = False
        'If Me.id = 0 Then
        '    bnew = True
        'End If
        'Dim new_filename As String = ""

        'If bstage_change_only = True Then
        '    'db_document.set_container_stage(

        '    Exit Function
        'End If


        REM register only
        'If register_only = True Then
        '    Exit Function
        'End If

        'REM now write document
        'If bwith_document = True Then
        '    If Me.filename = "" Then
        '        If Left(Me.extension, 5) = "[imp]" Then
        '            Me.filename = CStr(Me.id) + Right(Me.extension, extensionsize)
        '        End If
        '    End If
        '    ofs.write_bytestream_to_document(bc_cs_central_settings.central_repos_path + Me.filename, Me.byteDoc, MyBase.certificate)
        '    filedetails = New FileInfo(bc_cs_central_settings.central_repos_path + Me.filename)
        '    filesize = CStr(filedetails.Length)
        '    db_document.update_filesize(Me.id, filesize, certificate)
        'End If



        'Me.id = db_document.write_document(id, pub_type_id, title, summary, doc_date, pages, language_id, extension, originating_author, Me.sub_title, Me.stage, Me.master_container_id, Me.master_flag, Me.stage_name, Me.master_flag, Me.filename, Me.entity_id, Me.refresh_components.workflow_state, Me.refresh_components.accounting_standard, Me.refresh_components.data_at_date, Me.history, Me.main_note, Me.stage_expire_date, Me.bus_area, Me.translated_from_doc, Me.teaser_text, new_filename, Me.bimport_support_only, certificate)
        'If Me.register_only = True And bstage_change_only = False Then
        '    Me.registered_file_name = new_filename
        'End If
        'If bnew = True Then
        '    local_file_name = filename
        '    filename = CStr(Me.id) + Right(Me.extension, extensionsize)
        'End If

        'If Me.master_flag = True Then
        '    If Me.composite_constiuents.Count > 0 Then
        '        db_document.delete_constituents(Me.id, MyBase.certificate)
        '    End If
        '    For i = 0 To Me.composite_constiuents.Count - 1
        '        db_document.write_constituents(Me.id, Me.composite_constiuents(i), i, MyBase.certificate)
        '    Next
        '    'If Me.publish_flag = True Then
        '    '    db_document.publish(Me.id, MyBase.certificate)
        '    'End If
        'End If

        'If Me.set_approve = True Then
        '    If bc_cs_central_settings.server_flag = 1 Then
        '        db_document.approve_doc(Me.id, Me.stage, Me.approve_stage, certificate.user_id, certificate)
        '    Else
        '        db_document.approve_doc(Me.id, Me.stage, Me.approve_stage, bc_cs_central_settings.logged_on_user_id, certificate)
        '    End If
        '    db_document.write_history(CLng(container_id(0, 0)), "Approved to stage:" + Me.approve_stage_name, "Approval", "A", MyBase.certificate)
        'End If


        'container_id = db_document.get_container_id_for_document(Me.id, MyBase.certificate)
        REM document record successfully written or updated
        REM master document
        'If Me.master_flag = True Then

        'If Me.id > 0 Then
        '    REM remove existing authors
        '    'db_document.delete_Authors(Me.id, MyBase.certificate)
        '    'For i = 0 To authors.Count - 1

        '    '    db_document.write_authors_for_document(Me.id, authors(i).id, MyBase.certificate)
        '    'Next
        '    'REM taxonomy
        '    'db_document.reset_entities_for_document(Me.id, MyBase.certificate)
        '    'For i = 0 To taxonomy.Count - 1
        '    '    db_document.write_taxonomy_for_document(Me.id, taxonomy(i).entity_id, MyBase.certificate)
        '    'Next
        '    'REM write disclosures
        '    'db_document.reset_disclosures_for_document(Me.id, MyBase.certificate)

        '    'db_document.write_tmp_disclosures(Me.id, Me.filename, MyBase.certificate)
        '    'For i = 0 To disclosures.Count - 1
        '    '    db_document.write_disclsoures_for_document(Me.id, disclosures(i).entity_id, Me.filename, MyBase.certificate)
        '    'Next
        '    REM list items
        '    db_document.clear_list_items(Me.id, MyBase.certificate)
        '    For i = 0 To Me.list_items.list_items.Count - 1
        '        db_document.insert_list_item(Me.id, Me.list_items.list_items(i).list_id, Me.list_items.list_items(i).item_id, MyBase.certificate)
        '    Next
        '    If generate_mailing_list = True Then
        '        If Me.auto_generate_taxonomy = False Then
        '            db_document.set_no_auto_taxonomy(Me.id, MyBase.certificate)
        '        End If
        '        db_document.regenerate_distribution_list(CStr(Me.id), MyBase.certificate)
        '    End If
        'End If
        REM defined components and properties
        'refresh_components.db_write(Me.id, Me.stage_name, certificate)
        REM if document has just been translated then create file for trnaslation


        REM Support documents for the master
        'If support_documents.document.Count > 0 Then
        '    REM get container id from master
        '    'container_id = db_document.get_container_id_for_document(Me.id, MyBase.certificate)
        '    For i = 0 To support_documents.document.Count - 1
        '        support_documents.document(i).doc_date = Now
        '        support_documents.document(i).id = 0
        '        support_documents.document(i).master_flag = False
        '        'support_documents.document(i).master_container_id = container_id(0, 0)
        '        Dim fn = support_documents.document(i).filename
        '        Dim k = InStr(support_documents.document(i).filename, "_", CompareMethod.Text)
        '        If k > 0 Then
        '            fn = Right(fn, Len(fn) - k + 1)
        '            fn = CStr(Me.id) + fn
        '            support_documents.document(i).filename = fn
        '        Else
        '            support_documents.document(i).filename = CStr(Me.id) + support_documents.document(i).extension
        '        End If
        '        support_documents.document(i).db_write(Me.certificate)
        '    Next
        'End If
        'End If


        'If master_flag = True And Me.register_template = True And Me.extension <> "" Then
        '    db_document.update_filename(Me.id, Me.extension, False, MyBase.certificate)

        '    If Not IsNumeric(Left(filename, Len(filename) - extensionsize)) Then
        '        ofs.write_bytestream_to_document(bc_cs_central_settings.central_repos_path + CStr(Me.id) + Right(Me.extension, extensionsize), Me.byteDoc, MyBase.certificate)
        '        filedetails = New FileInfo(bc_cs_central_settings.central_repos_path + CStr(Me.id) + Right(Me.extension, extensionsize))
        '    Else
        '        ofs.write_bytestream_to_document(bc_cs_central_settings.central_repos_path + Me.filename, Me.byteDoc, MyBase.certificate)
        '        filedetails = New FileInfo(bc_cs_central_settings.central_repos_path + Me.filename)
        '    End If
        '    filesize = CStr(filedetails.Length)
        '    db_document.update_filesize(Me.id, filesize, MyBase.certificate)

        'End If
        'If register_only = False And bwith_document = True And Me.register_template = False Then
        '    If Left(Me.extension, 5) = "[imp]" Then
        '        If Me.filename = "" Then
        '            ofs.write_bytestream_to_document(bc_cs_central_settings.central_repos_path + CStr(Me.id) + Right(Me.extension, extensionsize), Me.byteDoc, MyBase.certificate)
        '            filedetails = New FileInfo(bc_cs_central_settings.central_repos_path + CStr(Me.id) + Right(Me.extension, extensionsize))
        '        Else
        '            ofs.write_bytestream_to_document(bc_cs_central_settings.central_repos_path + Me.filename, Me.byteDoc, MyBase.certificate)
        '            filedetails = New FileInfo(bc_cs_central_settings.central_repos_path + Me.filename)
        '        End If
        '        filesize = CStr(filedetails.Length)
        '        db_document.update_filesize(Me.id, filesize, certificate)
        '    Else
        '        If Me.register_template = False Then
        '            If Me.registered_file_name <> "" Then
        '                ofs.write_bytestream_to_document(bc_cs_central_settings.central_repos_path + registered_file_name, Me.byteDoc, MyBase.certificate)
        '                filedetails = New FileInfo(bc_cs_central_settings.central_repos_path + registered_file_name)
        '                Me.registered_file_name = ""
        '            Else
        '                'If Not IsNumeric(Left(filename, Len(filename) - 4)) Then
        '                'ofs.write_bytestream_to_document(bc_cs_central_settings.central_repos_path + CStr(Me.id) + Right(Me.extension, 4), Me.byteDoc, MyBase.certificate)
        '                'Else
        '                ofs.write_bytestream_to_document(bc_cs_central_settings.central_repos_path + Me.filename, Me.byteDoc, MyBase.certificate)
        '                filedetails = New FileInfo(bc_cs_central_settings.central_repos_path + Me.filename)
        '            End If
        '            filesize = CStr(filedetails.Length)
        '            db_document.update_filesize(Me.id, filesize, certificate)
        '        End If

        '        If Me.master_flag = False And Me.extension = ".pdf" Then
        '            'ofs.write_bytestream_to_document(bc_cs_central_settings.central_repos_path + Me.filename, Me.byteDoc, MyBase.certificate)
        '            REM 23-6-2008 overrite original pdf if it exists with this one
        '            Dim vpdf As Object
        '            Dim ipdf As Long
        '            vpdf = db_document.get_original_pdf(Me.master_container_id, MyBase.certificate)
        '            ipdf = 0
        '            If IsArray(vpdf) Then
        '                If UBound(vpdf, 2) >= 0 Then
        '                    Try
        '                        For i = 0 To UBound(vpdf, 2)
        '                            ipdf = CLng(vpdf(0, i))
        '                            If ipdf > 0 And ipdf <> Me.id Then
        '                                ofs.write_bytestream_to_document(bc_cs_central_settings.central_repos_path + CStr(ipdf) + ".pdf", Me.byteDoc, MyBase.certificate)
        '                            End If
        '                        Next
        '                    Catch

        '                    End Try
        '                End If
        '            End If
        '        End If
        '    End If
        'Else
        '    If Me.register_template = False Then
        '        If Me.bimport_support_only = False Then
        '            Dim ocommentary As New bc_cs_activity_log("bc_om_document", "db_write", bc_cs_activity_codes.COMMENTARY, "Document only being registered so leave checked out", certificate)
        '            'db_document.write_history(Me.master_container_id, "AA: Document has been unscheduled checked out: register only:" + CStr(register_only) + "; with document: " + CStr(bwith_document), "Error", "E", MyBase.certificate)
        '            If bc_cs_central_settings.server_flag = 1 Then
        '                db_document.check_out_document(Me.id, CLng(container_id(0, 0)), originating_author, Me.certificate, "", False)
        '            Else
        '                db_document.check_out_document(Me.id, CLng(container_id(0, 0)), originating_author, Nothing, "", False)
        '            End If
        '        End If
        '    End If
        'End If
        REM composite constituents



        'Catch ex As Exception
        '    Dim db_err As New bc_cs_error_log("bc_om_document", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        '    eerr = True
        'Finally
        'If bnew = True Then
        '    Me.filename = local_file_name
        'End If
        'REM if error leave document checked out so client can still have local copy access
        'If bc_cs_central_settings.server_flag = 1 Then
        '    If certificate.error_state = True Then
        '        eerr = True
        '        REM
        '        db_document.write_history(Me.master_container_id, "BB: Document has been unscheduled checked out", "Error", "E", MyBase.certificate)
        '        Dim ocommentary As New bc_cs_activity_log("bc_om_document", "db_write", bc_cs_activity_codes.COMMENTARY, "Document submission failed so document will  be left checked out", certificate)
        '        db_document.check_out_document(Me.id, container_id(0, 0), Me.checked_out_user, certificate, "", False)
        '    End If
        'End If
        'REM now run server side events if these fail revert container stage
        'REM only run if no errors
        'If Me.master_container_id = 0 Then
        '    container_id = db_document.get_container_id_for_document(Me.id, certificate)
        '    Me.master_container_id = container_id(0, 0)
        'End If
        'Dim result_tx As String = ""
        'Dim events_failed As Boolean = False

        'Try
        '    If Me.master_flag = True And eerr = False And (run_events = True) Then
        '        REM now run custom server side events
        '        Dim ocss_events As New bc_om_wf_custom_events
        '        ocss_events.certificate = MyBase.certificate
        '        ocss_events.entity_id = Me.entity_id
        '        ocss_events.doc_id = Me.id
        '        ocss_events.pub_type_id = Me.pub_type_id
        '        ocss_events.container_id = Me.master_container_id
        '        ocss_events.stage_from = current_stage
        '        ocss_events.stage_to = Me.stage
        '        ocss_events.stage_name_to = Me.stage_name
        '        ocss_events.last_revision_filename = Me.last_revision_filename
        '        ocss_events.current_filename = Me.filename
        '        ocss_events.revision_from_process = Me.revision_from_process
        '        ocss_events.cdoc = Me
        '        Me.byteDoc = Nothing
        '        Me.no_send_back = True
        '        If ocss_events.run_events = False Then
        '            Me.no_send_back = False
        '            events_failed = True
        '            Me.server_side_events_failed = "Server Side Event failed document wont change stage: " + ocss_events.err_text
        '        ElseIf ocss_events.output_text <> "" Then
        '            Me.no_send_back = False
        '            Me.server_side_events_failed = ocss_events.output_text
        '        End If
        '    End If
        '    REM if stage change allowed then empty approvers table
        '    'If Me.original_stage <> 0 And events_failed = False Then
        '    '    db_document.delete_approvers_for_doc(Me.id, MyBase.certificate)
        '    '    REM write stage change comments
        '    '    For i = 0 To Me.comments.comments.Count - 1
        '    '        Me.comments.comments(i).doc_id = Me.id
        '    '        Me.comments.comments(i).stage_id = Me.stage
        '    '        Me.comments.comments(i).db_write()
        '    '    Next
        '    'End If

        'Catch ex As Exception
        '    Dim db_err As New bc_cs_error_log("bc_om_document", "db_write1", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        'Finally
        '    'If bc_cs_central_settings.server_flag = 1 Then
        '    '    If MyBase.certificate.error_state = True Then
        '    '        db_document.set_container_stage(container_id(0, 0), current_stage, MyBase.certificate)
        '    '        db_document.write_history(Me.master_container_id, "Server error container will stay in current stage.", "Error", "E", MyBase.certificate)
        '    '        Dim comm As New bc_cs_activity_log("bc_om_wf_custom_events", "run_events", bc_cs_activity_codes.COMMENTARY, "Server Side Custom Events failed Container will stay in current stage", MyBase.certificate)
        '    '    End If
        '    'End If
        'End Try
        'otrace = New bc_cs_activity_log("bc_om_document", "db_write_trans", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        'End Try
        'End Function

        <Serializable()> Public Class bc_om_selected_list_item
            Public list_id As Integer
            Public key As Long
        End Class

        Public Sub tmp_write_comment(ByRef db As bc_cs_db_services, doc_id As Long, comment As String, assigned_user As Long, ByVal gdb As bc_om_document_trans_db)
            Dim otrace As New bc_cs_activity_log("bc_om_document", "tmp_write_comment", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
            Try
                Dim st As New bc_cs_string_services(comment)
                comment = st.delimit_apostrophies

                gdb.tmp_write_comment(db, doc_id, comment, assigned_user, certificate)
            Catch ex As Exception
                Me.doc_write_error_text = "bc_om_document:tmp_write_comment" + ex.Message
                Dim comm As New bc_cs_activity_log("bc_om_document", "tmp_write_comment", bc_cs_activity_codes.COMMENTARY, ex.Message, certificate)
            Finally
                otrace = New bc_cs_activity_log("bc_om_document", "tmp_write_comment", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
            End Try

        End Sub
        Public Function write_metadata(ByRef db As bc_cs_db_services, ByRef doc As bc_om_document, ByVal is_master As Boolean, ByVal stage_to As Long, ByVal gdb As bc_om_document_trans_db) As Boolean
            Dim otrace As New bc_cs_activity_log("bc_om_document", "write_metadata", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
            Try
                write_metadata = False
                REM now set up object for xml write
                Dim dw As New bc_om_document_write
                dw.id = doc.id
                dw.title = doc.title
                dw.sub_title = doc.sub_title
                dw.summary = doc.summary
                dw.rtf_text = doc.summary_rtf
                dw.html_text = doc.summary_html
                dw.custom_title = doc.custom_title
                dw.custom_title2 = doc.custom_title2
                dw.main_note = doc.main_note
                dw.teaser_text = doc.teaser_text
                dw.doc_date = doc.doc_date
                dw.pages = doc.pages
                dw.language_id = doc.language_id
                dw.entity_id = doc.entity_id
                dw.originating_author = doc.originating_author
                dw.filename = doc.filename
                dw.extension = doc.extension
                dw.pub_type_id = doc.pub_type_id
                dw.bus_area = doc.bus_area
                dw.stage = doc.stage
                dw.translated_from_doc = doc.translated_from_doc
                dw.master_container_id = Me.master_container_id

                dw.stage_expire_date = doc.stage_expire_date.ToUniversalTime

                dw.data_at_date = doc.refresh_components.data_at_date
                dw.workflow_state = doc.refresh_components.workflow_state
                dw.financial_workflow_stage = doc.refresh_components.workflow_state
                For i = 0 To doc.authors.Count - 1
                    dw.authors.Add(doc.authors(i))
                Next
                For i = 0 To doc.taxonomy.Count - 1
                    dw.taxonomy.Add(doc.taxonomy(i))
                Next

                dw.disclosures = doc.disclosures
                dw.stage_change_comment = doc.stage_change_comment
                dw.stage_to = stage_to
                dw.urgent_flag = doc.urgent_flag
                dw.urgent_text = doc.urgent_text
                dw.behalf_of_author_id = doc.behalf_of_author_id

                Dim si As bc_om_selected_list_item

                dw.write_selected_items = False

                For i = 0 To doc.oec.Lists.Count - 1

                    dw.write_selected_items = True
                    For j = 0 To doc.oec.Lists(i).items.Count - 1
                        If doc.oec.Lists(i).items(j).selected = True Then
                            si = New bc_om_selected_list_item
                            si.list_id = doc.oec.Lists(i).id
                            si.key = doc.oec.Lists(i).items(j).key
                            dw.selected_items.Add(si)
                        End If
                    Next
                Next

                If is_master = True Then

                    'dw.authors = doc.authors
                    'dw.taxonomy = doc.taxonomy
                    Dim ld As bc_om_linked_document

                    For i = 0 To doc.linked_docs.Count - 1
                        ld = New bc_om_linked_document
                        ld.doc_id = doc.linked_docs(i).id
                        dw.linked_documents.Add(ld)
                    Next

                Dim dc As bc_om_distribution_channel
                For i = 0 To doc.distribution_channels.Count - 1
                    dc = New bc_om_distribution_channel
                    dc.channel_id = doc.distribution_channels(i).channel_id
                    dw.distribution_channels.Add(dc)

                Next

                    Dim ad As bc_om_analytic_document

                    For i = 0 To doc.analytic_docs.Count - 1
                        ad = New bc_om_analytic_document
                        ad.doc_id = doc.analytic_docs(i).id
                        dw.analytic_documents.Add(ad)
                    Next

                    Dim occ As bc_om_comp_cons

                    For i = 0 To Me.composite_constiuents.Count - 1
                        occ = New bc_om_comp_cons
                        occ.doc_id = Me.composite_constiuents(i)
                        occ.ord = i
                        dw.composite_constiuents.Add(occ)
                    Next

                    Dim oh As bc_om_history
                    For i = 0 To doc.history.Count - 1
                        oh = New bc_om_history
                        oh.comment = doc.history(i).comment
                        dw.history.Add(oh)
                    Next
                    Dim rc As bc_om_refresh_component
                    Dim cp As component_parameter_for_write_down
                    Dim lp As component_parameter_list_val_for_write_down

                    For i = 0 To doc.refresh_components.refresh_components.Count - 1
                        rc = New bc_om_refresh_component
                        rc.locator = doc.refresh_components.refresh_components(i).locator
                        rc.last_refresh_date = doc.refresh_components.refresh_components(i).last_refresh_date

                        rc.last_update_date = doc.refresh_components.refresh_components(i).last_update_date
                        rc.type = doc.refresh_components.refresh_components(i).type
                        rc.contributor_id = doc.refresh_components.refresh_components(i).contributor_id
                        rc.sub_doc_id = doc.refresh_components.refresh_components(i).sub_doc_id
                        rc.disabled = doc.refresh_components.refresh_components(i).disabled
                        rc.entity_id = doc.refresh_components.refresh_components(i).entity_id


                        dw.refresh_components.Add(rc)

                        For j = 0 To doc.refresh_components.refresh_components(i).parameters.component_template_parameters.Count - 1
                            cp = New component_parameter_for_write_down
                            cp.system_defined = doc.refresh_components.refresh_components(i).parameters.component_template_parameters(j).system_defined
                            cp.default_value = doc.refresh_components.refresh_components(i).parameters.component_template_parameters(j).default_value_id
                            If doc.refresh_components.refresh_components(i).parameters.component_template_parameters(j).datatype = 0 Or doc.refresh_components.refresh_components(i).parameters.component_template_parameters(j).datatype = 2 Then
                                If doc.refresh_components.refresh_components(i).parameters.component_template_parameters(j).default_value_id = "" Then
                                    cp.default_value = doc.refresh_components.refresh_components(i).parameters.component_template_parameters(j).default_value
                                End If
                            End If
                            cp.order = j
                            cp.locator = rc.locator
                            cp.type_id = rc.type
                            dw.component_parameters.Add(cp)

                            For k = 0 To doc.refresh_components.refresh_components(i).parameters.component_template_parameters(j).list_key_ids.Count - 1
                                lp = New component_parameter_list_val_for_write_down
                                lp.locator = rc.locator
                                lp.type_id = rc.type
                                lp.order = j
                                lp.sel_ord = k
                                lp.val = doc.refresh_components.refresh_components(i).parameters.component_template_parameters(j).list_key_ids(k)
                                dw.component_parameters_List_vals.Add(lp)
                            Next

                        Next
                    Next

                    If doc.id = 0 Then
                        dw.filename = ""
                    End If
                End If



                REM write document but always leave it checked out at this point
                Dim vres As Object = Nothing
                vres = gdb.write_document_via_xml(db, dw.write_data_to_xml(certificate), is_master, certificate)
                If db.success = False Then
                    Me.doc_write_error_text = vres(0, 0)
                    Exit Function
                End If

                doc.id = vres(0, 0)
                doc.filename = vres(1, 0)
                doc.master_container_id = vres(2, 0)


                write_metadata = True
            Catch ex As Exception
                Me.doc_write_error_text = "bc_om_document:write metadata" + ex.Message
                Dim comm As New bc_cs_activity_log("bc_om_document", "write_metadata", bc_cs_activity_codes.COMMENTARY, ex.Message, certificate)
            Finally
                otrace = New bc_cs_activity_log("bc_om_document", "write_metadata", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
            End Try

        End Function
        <Serializable()> Public Class component_parameter_for_write_down
            Public locator As String
            Public system_defined As Integer
            Public default_value As String
            Public order As Integer
            Public type_id As Long

        End Class
        <Serializable()> Public Class component_parameter_list_val_for_write_down
            Public locator As String
            Public order As Integer
            Public type_id As Long
            Public sel_ord As String
            Public val As Long
        End Class



        Public Function write_physical_doc(ByRef db As bc_cs_db_services, ByVal doc As bc_om_document, ByVal gdb As bc_om_document_trans_db) As Boolean
            Dim otrace As New bc_cs_activity_log("bc_om_document", "write_physical_doc", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
            Try
                write_physical_doc = False
                Dim ofs As New bc_cs_file_transfer_services
                Dim filedetails As FileInfo
                Dim filesize As String
                filedetails = Nothing
                filesize = 0

                If ofs.write_bytestream_to_document(bc_cs_central_settings.central_repos_path + doc.filename, doc.byteDoc, MyBase.certificate, False) = False Then
                    doc_write_error_text = ofs.err_text
                Else
                    filedetails = New FileInfo(bc_cs_central_settings.central_repos_path + doc.filename)
                    filesize = CStr(filedetails.Length)
                    gdb.update_filesize(db, doc.id, filesize, certificate)
                    write_physical_doc = True
                End If

            Catch ex As Exception
                'Dim db_err As New bc_cs_error_log("bc_om_document", "run_physical_doc", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
                doc_write_error_text = "bc_om_document:write_physical_doc " + ex.Message
            Finally
                otrace = New bc_cs_activity_log("bc_om_document", "write_physical_doc", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
            End Try

        End Function





        'Private Function run_server_side_events(ByVal container_id As Long, ByVal events As bc_om_server_side_events, ByRef result_tx As String, ByVal certificate As bc_cs_security.certificate) As Boolean
        '    Dim otrace As New bc_cs_activity_log("bc_om_document", "run_server_side_events", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        '    Try
        '        Dim i As Integer
        '        Dim ocommentary As bc_cs_activity_log
        '        Dim gbc As New bc_om_document_db
        '        Dim event_success As Boolean = True
        '        run_server_side_events = True
        '        For i = 0 To events.events.Count - 1
        '            ocommentary = New bc_cs_activity_log("bc_om_document", " run_server_side_events", bc_cs_activity_codes.COMMENTARY, "Attempting to run server side Event: " + events.events(i).name, MyBase.certificate)
        '            event_success = gbc.run_server_side_event_sql(events.events(i).sql, result_tx, certificate)
        '            If event_success = True Then
        '                ocommentary = New bc_cs_activity_log("bc_om_document", " run_server_side_events", bc_cs_activity_codes.COMMENTARY, "Server side Event: " + events.events(i).name + " succeeded", MyBase.certificate)

        '                gbc.write_history(container_id, "Server side Event: " + events.events(i).name + " succeeded", "Server", "S", certificate)
        '                If result_tx <> "" Then
        '                    event_success = False
        '                    run_server_side_events = False
        '                End If
        '            Else
        '                run_server_side_events = False
        '                ocommentary = New bc_cs_activity_log("bc_om_document", " run_server_side_events", bc_cs_activity_codes.COMMENTARY, "Server side Event: " + events.events(i).name + " failed document wont change stage and furthur events surpress", MyBase.certificate)
        '                gbc.write_history(container_id, "Server side Event: " + events.events(i).name + " failed document wont change stage and furthur events surpress", "Server", "S", certificate)
        '                Exit For
        '            End If
        '        Next

        '    Catch ex As Exception
        '        Dim db_err As New bc_cs_error_log("bc_om_document", "run_server_side_events", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        '    Finally
        '        otrace = New bc_cs_activity_log("bc_om_document", "run_server_side_events", bc_cs_activity_codes.TRACE_EXIT, "", certificate)

        '    End Try
        'End Function
        Public Function upload_translation_components() As Boolean
            Dim otrace As New bc_cs_activity_log("bc_om_document", "upload_translation_components", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
            Try

                REM attempt to find file
                upload_translation_components = False
                Dim fn As String
                Dim i As Integer
                Dim db_document As New bc_om_document_db
                Dim vtran As Object
                Dim ocommentary As bc_cs_activity_log
                vtran = db_document.get_translation_file(CStr(Me.translated_from_doc), MyBase.certificate)
                If IsArray(vtran) Then
                    fn = bc_cs_central_settings.central_repos_path + CStr(vtran(0, 0)) + ".txt"
                    Dim fs As New bc_cs_file_transfer_services
                    If fs.check_document_exists(fn) Then
                        Dim ocomps As New bc_om_translation_components
                        ocomps = ocomps.read_data_from_file(fn)
                        REM see if language id has been upload
                        ocommentary = New bc_cs_activity_log("ccc111", CStr(ocomps.components.Count), bc_cs_activity_codes.COMMENTARY, CStr(i), MyBase.certificate)
                        If ocomps.language_id = Me.language_id Then
                            REM now write value to db
                            For i = 0 To ocomps.components.Count - 1
                                db_document.upload_translated_component(Me.id, ocomps.components(i).type_id, Me.entity_id, ocomps.components(i).destination, ocomps.components(i).locator, MyBase.certificate)
                            Next
                            upload_translation_components = True
                        End If
                    End If
                End If

            Catch ex As Exception
                Dim db_err As New bc_cs_error_log("bc_om_document", "upload_translation_components", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            Finally
                otrace = New bc_cs_activity_log("bc_om_document", "upload_translation_components", bc_cs_activity_codes.TRACE_EXIT, "", certificate)

            End Try
        End Function
        Public Sub attach_translations(ByVal fn As String)
            Dim sdoc As New bc_om_document
            Dim fs As New bc_cs_file_transfer_services
            sdoc.id = 0
            sdoc.master_flag = False
            sdoc.pub_type_id = Me.pub_type_id
            sdoc.pub_type_name = Me.pub_type_name
            sdoc.originating_author = Me.originating_author
            sdoc.bus_area = Me.bus_area
            sdoc.checked_out_user = 0
            sdoc.doc_date = Now
            sdoc.entity_id = Me.entity_id
            sdoc.originating_author = Me.originating_author
            sdoc.title = "Translation Components"
            sdoc.extension = ".txt"
            sdoc.register_only = False
            sdoc.filename = fn
            sdoc.bwith_document = True
            fs.write_document_to_bytestream(fn, sdoc.byteDoc, Nothing, False)
            Me.support_documents.Add(sdoc)
        End Sub
        REM prepares data to be sent back from a soap request
        REM used on server only This routine is shadowed as it is non standard
        REM from the base class
        'Public Shadows Function write_xml_to_soap_server_reponse(ByVal doc As String) As String
        '    Dim cs As New bc_cs_central_settings(True)
        '    Dim otrace As New bc_cs_activity_log("bc_om_document", "write_xml_to_soap_server_reponse", bc_cs_activity_codes.TRACE_ENTRY, "", MyBase.certificate)

        '    Try
        '        REM initialse database
        '        Dim db As New bc_cs_db_services(False)
        '        Dim odoc As New bc_om_document
        '        odoc.certificate = MyBase.certificate

        '        Dim oxml As New bc_cs_xml_services
        '        REM decrypt
        '        Dim osec As New bc_cs_security
        '        doc = osec.decrypt_xml(doc, certificate)
        '        odoc = oxml.soap_deserialize_string_to_object(doc, Me.certificate)
        '        REM product response and return
        '        'Me.id = CLng(doc_id)
        '        odoc.db_read()
        '        REM write object to string
        '        write_xml_to_soap_server_reponse = osec.encrypt_xml(odoc.write_xml_to_string, certificate)
        '    Catch ex As Exception
        '        Dim db_err As New bc_cs_error_log("bc_om_document", "write_xml_to_soap_server_reponse", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        '    Finally
        '        otrace = New bc_cs_activity_log("bc_om_document", "write_xml_to_soap_server_reponse", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        '    End Try

        'End Function
        REM web service for download of a document
        'Public Overloads Overrides Function call_web_service() As String
        '    Dim otrace As New bc_cs_activity_log("bc_om_document", "call_web_service", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        '    Dim ocommentary As New bc_cs_activity_log("bc_om_document", "call_web_service", bc_cs_activity_codes.COMMENTARY, "Attempting to call Web Service LoadDocument.")
        '    If IsNumeric(bc_cs_central_settings.timeout) Then
        '        webservice.Timeout = CLng(bc_cs_central_settings.timeout)
        '        ocommentary = New bc_cs_activity_log("bc_om_web services", "call_web_service", bc_cs_activity_codes.COMMENTARY, "Setting Web Service Timeout to: " + bc_cs_central_settings.timeout)
        '    End If

        '    call_web_service = webservice.LoadDocument(Me.id)
        '    otrace = New bc_cs_activity_log("bc_om_document", "call_web_service", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        'End Function
        'Public Overloads Overrides Function call_web_service(ByVal s As String) As String
        '    Dim otrace As New bc_cs_activity_log("bc_om_document", "call_web_servic(s)", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        '    Dim ocommentary As New bc_cs_activity_log("bc_om_document", "call_web_service", bc_cs_activity_codes.COMMENTARY, "Attempting to call Web Service LoadDocument.")
        '    If IsNumeric(bc_cs_central_settings.timeout) Then
        '        webservice.Timeout = CLng(bc_cs_central_settings.timeout)
        '        ocommentary = New bc_cs_activity_log("bc_om_web services", "call_web_service", bc_cs_activity_codes.COMMENTARY, "Setting Web Service Timeout to: " + bc_cs_central_settings.timeout)
        '    End If

        '    call_web_service = webservice.LoadDocument(s)
        '    otrace = New bc_cs_activity_log("bc_om_document", "call_web_service(s)", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        'End Function
        REM web service for upload of document
        'Public Function call_web_service_uploaddocument(ByVal s As String) As String
        '    Dim otrace As New bc_cs_activity_log("bc_om_document", "call_web_service_uploaddocument", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        '    Try
        '        Dim os As String
        '        If IsNumeric(bc_cs_central_settings.timeout) Then
        '            webservice.Timeout = CLng(bc_cs_central_settings.timeout)
        '            Dim ocommentary As New bc_cs_activity_log("bc_om_web services", "call_web_service", bc_cs_activity_codes.COMMENTARY, "Setting Web Service Timeout to: " + bc_cs_central_settings.timeout)
        '        End If

        '        os = webservice.UploadDocument(s)
        '        Return os
        '    Catch ex As Exception
        '        Dim db_err As New bc_cs_error_log("bc_om_document", "call_web_service_uploaddocument", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        '        Return "failed"
        '    Finally
        '        otrace = New bc_cs_activity_log("bc_om_document", "call_web_service_uploaddocument", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        '    End Try
        'End Function
        'Public Shadows Function write_xml_via_soap_client_request() As String
        '    Dim otrace As New bc_cs_activity_log("bc_om_document", "write_xml_via_soap_client_request", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        '    Try
        '        Dim ocommentary As bc_cs_activity_log

        '        Dim s, os As String
        '        Dim bc_cs_accountinfoheader As New localhost.bc_cs_accountinfoheader
        '        webservice = New localhost.Service1
        '        webservice.Url = bc_cs_central_settings.soap_server

        '        bc_cs_accountinfoheader.authentication_method = bc_cs_central_settings.show_authentication_form
        '        If bc_cs_central_settings.show_authentication_form = 0 Then
        '            bc_cs_accountinfoheader.os_logon_name = bc_cs_central_settings.logged_on_user_name
        '        Else
        '            bc_cs_accountinfoheader.user_name = bc_cs_central_settings.user_name
        '            bc_cs_accountinfoheader.password = bc_cs_central_settings.user_password
        '        End If
        '        webservice.bc_cs_accountinfoheaderValue = bc_cs_accountinfoheader

        '        set_proxy_settings()

        '        REM call object model specific web service
        '        os = write_xml_to_string()
        '        s = call_web_service_uploaddocument(os)
        '        If Left(s, 25) = "Webservice Authentication" Then
        '            ocommentary = New bc_cs_activity_log("bc_om_document", "write_xml_via_soap_client_request", bc_cs_activity_codes.COMMENTARY, "Authentication Failed for Web Service UploadDocument! User:" + bc_cs_accountinfoheader.os_logon_name)
        '            Dim omessage As New bc_cs_message("Blue Curve Create", "Authentication Failed for Web Service LoadUsers! User:" + bc_cs_accountinfoheader.os_logon_name, bc_cs_message.MESSAGE)
        '            write_xml_via_soap_client_request = "failed"
        '        End If
        '        If s = "failed" Then
        '            ocommentary = New bc_cs_activity_log("bc_om_document", "write_xml_via_soap_client_request", bc_cs_activity_codes.COMMENTARY, "Document Save Failed couldnt complete SOAP request! User:" + bc_cs_accountinfoheader.os_logon_name)
        '            write_xml_via_soap_client_request = "failed"
        '        End If
        '    Catch ex As Exception
        '        Dim db_err As New bc_cs_error_log("bc_om_document", "write_xml_via_soap_client_request", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        '    Finally
        '        otrace = New bc_cs_activity_log("bc_om_document", "write_xml_via_soap_client_request", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        '    End Try

        'End Function
        'Public Overloads Overrides Sub db_read(ByVal id As Long, ByRef certificate As bc_cs_security.certificate)

        'End Sub

        'Public Overloads Overrides Sub db_write(ByVal id As Integer, ByRef certificate As cs.bc_cs_security.certificate)

        'End Sub
    End Class

    <Serializable()> Public Class bc_om_translation_components
        Inherits bc_cs_soap_base_class
        Public doc_id As Long
        Public language_id As Long
        Public components As New ArrayList
        Public Sub New()

        End Sub
    End Class
    <Serializable()> Public Class bc_om_translation_component
        Inherits bc_cs_soap_base_class
        Public locator As String
        Public type_id As Long
        Public source As String
        Public destination As String
        Public max_length As String
        Public Sub New()
            Me.source = ""
            Me.destination = ""
        End Sub
    End Class

    REM taxonomy class
    <Serializable()> Public Class bc_om_taxonomy
        Public entity_id As Long
        Public class_id As Long
        Public name As String
        Public class_name As String
        Public system_set As Integer = 0
        Public Sub New()

        End Sub
        Public Sub New(ByVal lentity_id As Long)
            entity_id = lentity_id
        End Sub
    End Class
    REM taxonomy class
    <Serializable()> Public Class bc_om_history
        Public da As DateTime
        Public desc As String
        Public comment As String
        Public code As String
        Public user As String
        Public stage As String

        Public Sub New()

        End Sub
    End Class
    <Serializable()> Public Class bc_om_mandatory_fields
        Public doc_id As Long
        Public fields As New ArrayList
        Public Sub New(ByVal doc_id As Long)
            Me.doc_id = doc_id
        End Sub
        Public Sub New()

        End Sub
        Public Sub db_read(ByRef certificate As bc_cs_security.certificate)
            Dim otrace As New bc_cs_activity_log("bc_om_mandatory_fields", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
            Try
                Dim vfields As Object
                Dim i As Integer
                Dim lfield As bc_om_mandatory_field
                fields.Clear()
                Dim obc_doc As New bc_om_document_db
                vfields = obc_doc.get_mandatory_fields(doc_id, certificate)
                If IsArray(vfields) Then
                    If CStr(vfields(0, 0)) = "Error" Then
                        Dim ocomm As New bc_cs_activity_log("bc_om_entity_classify_filters", "db_read", bc_cs_activity_codes.COMMENTARY, "Warning: Mandatory Fields Option not installed", certificate)
                        Exit Sub
                    End If
                    For i = 0 To UBound(vfields, 2)
                        lfield = New bc_om_mandatory_field
                        lfield.type_id = CLng(vfields(0, i))
                        lfield.class_id = vfields(1, i)
                        lfield.item_id = vfields(2, i)
                        lfield.stage_id = vfields(3, i)
                        Me.fields.Add(lfield)
                    Next
                End If
            Catch ex As Exception
                Dim db_err As New bc_cs_error_log("bc_om_mandatory_fields", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            Finally
                otrace = New bc_cs_activity_log("bc_om_mandatory_fields", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
            End Try
        End Sub
    End Class
    <Serializable()> Public Class bc_om_mandatory_field
        Public type_id As Long
        Public class_id As Long
        Public item_id As Long
        Public stage_id As Long

        Public Sub New()

        End Sub
    End Class
    <Serializable()> Public Class bc_om_entity_classify_filters
        Public doc_id As Long
        Public filters As New ArrayList
        Public Sub New(ByVal doc_id As Long)
            Me.doc_id = doc_id
        End Sub
        Public Sub New()

        End Sub
        Public Sub db_read(ByRef certificate As bc_cs_security.certificate)
            Dim otrace As New bc_cs_activity_log("bc_om_entity_classify_filters", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
            Try
                Dim vfields As Object
                Dim i As Integer
                Dim lfield As bc_om_entity_classify_filter
                filters.Clear()
                Dim obc_doc As New bc_om_document_db
                vfields = obc_doc.get_entity_classify_filters(doc_id, certificate)
                If IsArray(vfields) Then
                    If CStr(vfields(0, 0)) = "Error" Then
                        Dim ocomm As New bc_cs_activity_log("bc_om_entity_classify_filters", "db_read", bc_cs_activity_codes.COMMENTARY, "Warning: Entity Classify Filter not installed", certificate)

                        Exit Sub
                    End If
                    For i = 0 To UBound(vfields, 2)
                        lfield = New bc_om_entity_classify_filter
                        lfield.class_id = CLng(vfields(0, i))
                        lfield.entity_id = CLng(vfields(1, i))
                        Me.filters.Add(lfield)
                    Next
                End If
            Catch ex As Exception
                Dim db_err As New bc_cs_error_log("bc_om_entity_classify_filters", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            Finally
                otrace = New bc_cs_activity_log("bc_om_entity_classify_filters", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
            End Try
        End Sub
    End Class
    <Serializable()> Public Class bc_om_entity_classify_filter
        Public class_id As Long
        Public entity_id As Long

        Public Sub New()

        End Sub
    End Class
    REM =========================================================================
    REM Database interaction layer
    REM =========================================================================
    Public Class bc_om_document_trans_db
        Private gbc_db As New bc_cs_db_services
        REM routines in transaction
        Public Sub force_check_in(ByVal doc_id As Long, ByVal certifcate As bc_cs_security.certificate)
            Dim sql As String
            If bc_cs_central_settings.server_flag = 0 Then
                certifcate.user_id = bc_cs_central_settings.logged_on_user_id
            End If
            sql = "exec dbo.bc_core_force_check_in " + CStr(doc_id) + "," + certifcate.user_id
            gbc_db.executesql(sql, certifcate)
        End Sub
        Public Sub check_out_document(ByVal id As Long, ByRef certificate As bc_cs_security.certificate)
            Dim sql As String
            Dim user_id As Long
            If bc_cs_central_settings.server_flag = 0 Then
                user_id = bc_cs_central_settings.logged_on_user_id
            Else
                user_id = certificate.user_id
            End If
            sql = "exec dbo.bc_core_checkout_document " + CStr(id) + "," + CStr(user_id)
            gbc_db.executesql(sql, certificate)
        End Sub
        Public Function New_update_filename(ByRef db As bc_cs_db_services, ByVal doc_id As Long, ByVal ext As String, ByRef certificate As bc_cs_security.certificate) As String
            Dim sql As String
            Dim fn As Object
            sql = "exec dbo.bc_core_update_filename " + CStr(doc_id) + ",'" + ext + "'"
            fn = db.executesql_trans(sql, certificate, True)
            New_update_filename = fn(0, 0)

        End Function
        Public Sub new_approve_doc(ByRef db As bc_cs_db_services, ByVal doc_id As Long, ByVal stage_from As Long, ByVal stage_to As String, ByRef certificate As bc_cs_security.certificate)
            Dim sql As String
            Dim user_id As Long
            ' REM SR - Column names included in the following insert statement
            If bc_cs_central_settings.server_flag = 0 Then
                user_id = bc_cs_central_settings.logged_on_user_id
            Else
                user_id = certificate.user_id
            End If

            sql = "exec dbo.bc_core_set_approver_for_doc " + CStr(doc_id) + "," + CStr(user_id) + "," + CStr(stage_from) + "," + CStr(stage_to)

            If IsNothing(db) Then
                gbc_db.executesql(sql, certificate)
            Else
                db.executesql_trans(sql, certificate)
            End If
        End Sub
        Public Sub tmp_write_comment(db, doc_Id, comment, assigned_user, certificate)
            Dim fs As New bc_cs_string_services(comment)
            comment = fs.delimit_apostrophies()

            Dim sql As String
            sql = "exec dbo.bc_core_tmp_write_comment " + CStr(doc_Id) + ",N'" + comment + "'," + CStr(assigned_user)
            db.executesql_trans(sql, certificate, True)
        End Sub
        Public Function write_document_via_xml(ByRef db As bc_cs_db_services, ByVal xml As String, ByVal is_master As Boolean, ByRef certificate As bc_cs_security.certificate) As Object
            Dim sql As String

            If bc_cs_central_settings.server_flag = 0 Then
                certificate.user_id = bc_cs_central_settings.logged_on_user_id
            End If

            sql = "exec dbo.bc_core_write_document N'" + xml + "'," + CStr(is_master) + "," + CStr(certificate.user_id)
            write_document_via_xml = db.executesql_trans(sql, certificate, True)


        End Function
        Public Sub update_filesize(ByRef db As bc_cs_db_services, ByVal doc_id As Long, ByVal filesize As String, ByRef certificate As bc_cs_security.certificate)
            Dim sql As String
            sql = "update document_table set doc_size_kb='" + filesize + "' where doc_id = " + CStr(doc_id)
            db.executesql_trans(sql, certificate)
        End Sub
        REM routines not in transaction
        Public Sub update_and_audit(ByVal doc_id As Long, ByVal stage_id As Long, ByVal code As Integer, ByVal tx As String, ByVal stage_change_comment As String, ByVal stage_expire_date As String, ByRef certificate As bc_cs_security.certificate)
            Dim sql As String
            Dim str As New bc_cs_string_services(tx)
            tx = str.delimit_apostrophies
            str = New bc_cs_string_services(stage_change_comment)
            stage_change_comment = str.delimit_apostrophies()

            If bc_cs_central_settings.server_flag = 0 Then
                certificate = New bc_cs_security.certificate
                certificate.user_id = bc_cs_central_settings.logged_on_user_id
            End If
            sql = "dbo.bc_core_update_and_audit " + CStr(doc_id) + "," + CStr(stage_id) + "," + CStr(code) + ",'" + tx + "',N'" + stage_change_comment + "','" + stage_expire_date + "'," + CStr(certificate.user_id)
            gbc_db.executesql(sql, certificate)
        End Sub
        Public Sub write_history(ByRef db As bc_cs_db_services, ByVal doc_id As Long, ByVal tx As String, ByVal code As String, ByVal stage_from As Long, ByVal stage_to As Long, Optional ByRef certificate As bc_cs_security.certificate = Nothing)
            Dim sql As String
            Dim s As New bc_cs_string_services(tx)
            tx = s.delimit_apostrophies

            If bc_cs_central_settings.server_flag = 0 Then
                certificate.user_id = bc_cs_central_settings.logged_on_user_id
            End If
            sql = "bc_core_write_history " + CStr(doc_id) + ",'" + tx + "'," + CStr(certificate.user_id) + ",'" + CStr(code) + "'," + CStr(stage_from) + "," + CStr(stage_to)
            db.executesql_trans(sql, certificate)
        End Sub
        Public Function check_in_out(doc_id As Long, check_in As Boolean, ByRef certificate As bc_cs_security.certificate) As String
            Dim sql As String
            sql = "exec dbo.bc_core_flag_check_in_out " + CStr(doc_id) + "," + CStr(check_in) + "," + CStr(certificate.user_id)
            gbc_db.executesql(sql, certificate)
        End Function

        Public Function remove_support_doc(ByVal doc_id As Long, ByRef certificate As bc_cs_security.certificate, ByRef db As bc_cs_db_services) As Boolean
            Dim sql As String

            remove_support_doc = False
            Try
                If bc_cs_central_settings.server_flag = 0 Then
                    certificate = New bc_cs_security.certificate
                    certificate.user_id = bc_cs_central_settings.logged_on_user_id
                End If

                sql = "exec dbo.bc_core_delete_support_doc " + CStr(doc_id) + "," + CStr(certificate.user_id)
                If Not IsNothing(db) Then
                    db.executesql_trans(sql, certificate)
                Else
                    gbc_db.executesql(sql, certificate)
                End If
                remove_support_doc = True
            Catch ex As Exception
                Dim oerr As New bc_cs_error_log("bc_om_document_trans_db", "remove_support_doc", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)

            End Try
        End Function
    End Class


    Public Class bc_om_document_db
        Private gbc_db As New bc_cs_db_services
        Public Function delete_regular_report(id As Integer, ByRef certificate As bc_cs_security.certificate)
            Dim sql As String
            sql = "exec dbo.bc_core_delete_regular_report " + CStr(id)
            delete_regular_report = gbc_db.executesql(sql, certificate)
        End Function
        Public Function add_regular_report(id As Integer, bglobal As Boolean, name As String, title As String, sub_title As String, summary As String, pub_type_id As Long, custom_title As String, summary_html As String, create_user_id As Long, custom_title2 As String, ByRef certificate As bc_cs_security.certificate) As Object
            Try
                Dim sql As String
                Dim user_id As Long
                If bc_cs_central_settings.server_flag = 0 Then
                    user_id = bc_cs_central_settings.logged_on_user_id
                Else
                    user_id = certificate.user_id
                End If
                Dim iglobal As Integer = 0
                If bglobal = True Then
                    iglobal = 1
                End If
                Dim fs As New bc_cs_string_services(title)
                title = fs.delimit_apostrophies
                fs = New bc_cs_string_services(sub_title)
                sub_title = fs.delimit_apostrophies
                fs = New bc_cs_string_services(summary)
                summary = fs.delimit_apostrophies
                fs = New bc_cs_string_services(name)
                name = fs.delimit_apostrophies
                fs = New bc_cs_string_services(custom_title)
                custom_title = fs.delimit_apostrophies
                fs = New bc_cs_string_services(summary_html)
                summary_html = fs.delimit_apostrophies
                fs = New bc_cs_string_services(custom_title2)
                custom_title2 = fs.delimit_apostrophies

                sql = "exec dbo.bc_core_add_regular_report " + CStr(id) + ",'" + name + "'," + CStr(pub_type_id) + ",'" + title + "','" + sub_title + "','" + summary + "'," + CStr(iglobal) + "," + CStr(user_id) + ",'" + custom_title + "','" + summary_html + "'," + CStr(create_user_id) + ",'" + custom_title2 + "'"


                add_regular_report = gbc_db.executesql(sql, certificate)
            Catch ex As Exception
                Dim oerr As New bc_cs_error_log("bc_om_document_db", "add_regular_report", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)

            End Try
        End Function
        Public Sub add_entity_for_regular_report(id As Integer, entity_id As Long, principal As Boolean, ByRef certificate As bc_cs_security.certificate)
            Dim sql As String
            sql = "exec dbo.bc_core_add_entity_for_regular_report " + CStr(id) + "," + CStr(entity_id) + "," + CStr(principal)
            gbc_db.executesql(sql, certificate)
        End Sub
        Public Sub add_user_for_regular_report(id As Integer, user_id As Long, ByRef certificate As bc_cs_security.certificate)
            Dim sql As String
            sql = "exec dbo.bc_core_add_user_for_regular_report " + CStr(id) + "," + CStr(user_id)
            gbc_db.executesql(sql, certificate)
        End Sub
        Public Sub add_disclosure_for_regular_report(id As Integer, entity_id As Long, ByRef certificate As bc_cs_security.certificate)
            Dim sql As String
            sql = "exec dbo.bc_core_add_disclosure_for_regular_report " + CStr(id) + "," + CStr(entity_id)
            gbc_db.executesql(sql, certificate)
        End Sub

        Public Function read_table_on_demand(ByVal id As Long, ByRef certificate As bc_cs_security.certificate)
            Dim sql As String
            sql = "exec dbo.bc_core_get_table_on_demand " + CStr(id)
            read_table_on_demand = gbc_db.executesql(sql, certificate)
        End Function
    Public Function read_channels_for_documents(ByVal id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "exec dbo.bc_core_get_dist_channels_for_doc " + CStr(id)
        read_channels_for_documents = gbc_db.executesql(sql, certificate)
    End Function
    Public Function read_linked_documents(ByVal id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "exec dbo.bc_core_get_linked_documents " + CStr(id)
        read_linked_documents = gbc_db.executesql(sql, certificate)
    End Function
    Public Function read_analytic_documents(ByVal id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "exec dbo.bc_core_get_analytic_documents " + CStr(id)
        read_analytic_documents = gbc_db.executesql(sql, certificate)
    End Function
    Public Function get_comments_for_doc(ByVal doc_id As Long, ByVal span As Integer, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "exec dbo.bc_core_get_document_comments " + CStr(doc_id) + "," + CStr(span)
        get_comments_for_doc = gbc_db.executesql(sql, certificate)
    End Function
    Public Sub write_comment_for_doc(ByVal doc_id As Long, ByVal comment As String, ByRef certificate As bc_cs_security.certificate)
        Dim fs As New bc_cs_string_services(comment)
        comment = fs.delimit_apostrophies()
        If bc_cs_central_settings.server_flag = 0 Then
            certificate.user_id = bc_cs_central_settings.logged_on_user_id
        End If

        Dim sql As String
        sql = "exec dbo.bc_core_write_doc_comment " + CStr(certificate.user_id) + "," + CStr(doc_id) + ",N'" + comment + "'"
        gbc_db.executesql(sql, certificate)
    End Sub

    Public Function read_checked_out_documents(ByVal all As Boolean, ByVal workflow_mode As Boolean, ByVal date_from As Date, ByVal date_to As Date, ByRef certificate As bc_cs_security.certificate) As Object
        Dim otrace As New bc_cs_activity_log("bc_om_document_db", "read_checked_out_documents", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Dim luserid As Long
        Dim sql As String
        REM reads all documents user can see
        Try
            If bc_cs_central_settings.server_flag = 0 Then
                luserid = bc_cs_central_settings.logged_on_user_id
            Else
                luserid = certificate.user_id
            End If
            REM get all documents checked out to this user
            sql = "select doc_pub_type_id,doc_title,doc_object,doc_date,document_table.doc_id, doc_publish_flag, stage_name,doc_date,container_tbl.stage_id, pub_type_name, document_table.user_id, doc_sub_title, bus_area_name, doc_number_pages, doc_text, coalesce(lead_entity_id,0), coalesce(originator_id,0), coalesce(financial_workflow_stage,0), coalesce(accounting_standard,0), container_tbl.target_date, container_tbl.note,  coalesce(doc_pre_render_filename,''), coalesce(doc_extension,'') from " & _
                          "pub_type_table, container_tbl, document_table, person_bus_area_tbl, user_table, stage_role_access_tbl, stage_tbl, bus_area_table where " & _
                          "bus_area_table.bus_area_id=doc_bus_area_id and " & _
                          "person_bus_area_tbl.bus_area_id=doc_bus_Area_id and " & _
                          "document_table.container_id=container_tbl.container_id and " & _
                          "user_table.role_id = stage_role_access_tbl.role_id and access_id = 'W' and " & _
                          "stage_role_Access_tbl.stage_id = container_tbl.stage_id and " & _
                          "stage_tbl.stage_id = container_tbl.stage_id and document_table.if_publish=1 and " & _
                          "(doc_extension = '.doc' or doc_extension = '.docx' or doc_extension= '.ppt'or doc_extension= '.pptx')  and " & _
                          "person_bus_area_tbl.user_id='" & luserid & "' and user_table.user_Id='" & luserid & "' and " & _
                          "pub_type_table.pub_type_id = document_table.doc_pub_type_id and  document_table.user_id =" & CStr(luserid) & _
                          "order by doc_title asc, document_table.doc_date desc"

            read_checked_out_documents = gbc_db.executesql(sql, certificate)

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_document_db", "read_checked_out_documents", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            read_checked_out_documents = Nothing
        Finally
            otrace = New bc_cs_activity_log("bc_om_document_db", "read_checked_out_documents", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
            REM reads all documents user can see
        End Try

    End Function
    Public Function read_filter_items(ByVal all As Boolean, ByVal workflow_mode As Boolean, ByVal xsdate_from As String, ByVal xsdate_to As String, ByVal show_publish As Boolean, publish_only As Boolean, ByRef certificate As bc_cs_security.certificate) As Object
        Dim otrace As New bc_cs_activity_log("bc_om_document_db", "read_filter_items", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Dim sql As String
            Dim luserid As String
            read_filter_items = Nothing
            If bc_cs_central_settings.server_flag = 0 Then
                luserid = bc_cs_central_settings.logged_on_user_id
            Else
                luserid = certificate.user_id
            End If
            If workflow_mode = True Then
                REM workflow allows everything
                sql = "bc_core_doc_filter_items " & workflow_mode & "," & CStr(luserid) + "," & all & ",'" & xsdate_from & "','" & xsdate_to & "','" & CStr(show_publish) & "','" & CStr(publish_only) & "'"
                read_filter_items = gbc_db.executesql(sql, certificate)
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_document_db", "read_filter_items", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            Return Nothing
        Finally
            otrace = New bc_cs_activity_log("bc_om_document_db", "read_filter_items", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Function
    Public Function read_all_documents(ByVal all As Boolean, ByVal workflow_mode As Boolean, ByVal xsdate_from As String, ByVal xsdate_to As String, ByVal show_publish As Boolean, ByVal publish_only As Boolean, ByRef certificate As bc_cs_security.certificate) As Object
        Dim otrace As New bc_cs_activity_log("bc_om_document_db", "read_all_documents", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Dim sql As String
            Dim ocommentary As bc_cs_activity_log




            read_all_documents = Nothing
            Dim luserid As String
            ocommentary = New bc_cs_activity_log("bc _om_document_db", "read_all_documents", bc_cs_activity_codes.COMMENTARY, "Mode: " + CStr(workflow_mode), certificate)
            If bc_cs_central_settings.server_flag = 0 Then
                luserid = bc_cs_central_settings.logged_on_user_id
            Else
                luserid = certificate.user_id
            End If


            If workflow_mode = True Then
                REM workflow allows everything
                sql = "bcc_core_open_doc true, " + CStr(luserid) + "," & all & ",'" & xsdate_from & "','" & xsdate_to & "','" & show_publish & "','" & CStr(publish_only) + "'"

                Dim res As Object
                res = gbc_db.executesql(sql, certificate)
                If IsArray(res) Then
                    If UBound(res, 2) = -1 Then
                        Dim ocomm As New bc_cs_activity_log("bc_om_document_db", "read_all_documents", bc_cs_activity_codes.COMMENTARY, "Document Open SP returned no items (" + sql + ")", certificate)
                        Return Nothing
                    ElseIf IsNumeric(res(0, 0)) Then
                        read_all_documents = res
                        Exit Function
                    End If
                Else
                    REM no docs returned which is valid response
                    Return Nothing
                End If

            Else
                REM PR Jan 2009 allows for SP based solution as well
                REM if no SP users default
                sql = "bcc_core_open_doc false, " + CStr(luserid) + "," & all & ",'" & xsdate_from & "','" & xsdate_to & "','" & show_publish & "'"
                Dim ocom As New bc_cs_activity_log("bc_om_document_db", "read_all_documents", bc_cs_activity_codes.COMMENTARY, "Open SQL: " + sql, certificate)
                Dim res As Object
                res = gbc_db.executesql(sql, certificate)
                If IsArray(res) Then
                    If UBound(res, 2) = -1 Then
                        Dim ocomm As New bc_cs_activity_log("bc_om_document_db", "read_all_documents", bc_cs_activity_codes.COMMENTARY, "Document Open SP returned no items (" + sql + ")", certificate)
                        Return Nothing
                    ElseIf IsNumeric(res(0, 0)) Then
                        read_all_documents = res
                        Exit Function
                    End If
                Else
                    REM no docs returned which is valid response
                    Return Nothing
                End If


            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_document_db", "read_all_documents", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            Return Nothing

        Finally
            otrace = New bc_cs_activity_log("bc_om_document_db", "read_all_documents", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Function
    Public Function read_document(ByVal doc_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String


        sql = "dbo.bc_core_read_document " + CStr(doc_id)
        read_document = gbc_db.executesql(sql, certificate)

    End Function
    Public Function read_document_trans(db As Object, ByVal doc_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String


        sql = "dbo.bc_core_read_document " + CStr(doc_id)
        read_document_trans = db.executesql_trans(sql, certificate)

    End Function

    Public Function read_originating_author(ByVal doc_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select analyst_id from doc_analyst_table where doc_id=" + CStr(doc_id) + " and principal_analyst=1"
        read_originating_author = gbc_db.executesql(sql, certificate)
    End Function
    Public Function read_teaser_text(ByVal doc_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select coalesce(teaser_text,'') from document_table where doc_id=" + CStr(doc_id) + " "
        read_teaser_text = gbc_db.executesql_show_no_error(sql)
    End Function

    Public Function read_taxonomy_for_document(ByVal doc_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select e.entity_id, e.class_id, e.name, c.class_name  from doc_entity_tbl d, entity_tbl e, entity_class_tbl c where doc_id=" + CStr(doc_id) + " and d.entity_id=e.entity_id and c.class_id=e.class_id and coalesce(e.inactive,0)=0 and coalesce(e.deleted,0)=0 order by c.class_id,d.ranking asc"
        read_taxonomy_for_document = gbc_db.executesql(sql, certificate)
    End Function
    Public Function read_disclosures_for_document(ByVal doc_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select e.entity_id from assoc_doc_entity_tbl a " + _
              " inner join entity_tbl e on e.entity_id=a.entity_Id and coalesce(e.inactive,0)=0 and coalesce(e.deleted,0)=0" + _
              " where a.doc_id=" + CStr(doc_id)
        read_disclosures_for_document = gbc_db.executesql(sql, certificate)
    End Function
    Public Function read_authors_for_document(ByVal doc_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String

        sql = "exec dbo.bc_core_analysts_in_doc " + CStr(doc_id)
        read_authors_for_document = gbc_db.executesql(sql, certificate)
    End Function
    Public Sub update_filename(ByVal doc_id As Long, ByVal ext As String, ByVal fn As String, ByVal ext_only As Boolean, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String

        REM SW cope with office versions
        If ext_only = False Then
            sql = "update document_table set  doc_filename ='" + fn + "', doc_filename_internal='" + fn + "', doc_object='" + fn + "', doc_extension='" + CStr(ext) + "' where doc_id = " + CStr(doc_id)
        Else
            sql = "update document_table set doc_extension='" + CStr(ext) + "' where doc_id = " + CStr(doc_id)
        End If
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Sub update_filesize(ByVal doc_id As Long, ByVal filesize As String, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "update document_table set doc_size_kb='" + filesize + "' where doc_id = " + CStr(doc_id)
        gbc_db.executesql(sql, certificate)
    End Sub
    'Public Function is_generate_dist_list_installed(ByRef certificate As bc_cs_security.certificate) As Boolean
    '    Dim otrace As New bc_cs_activity_log("bc_om_document_db", "is_generate_dist_list_installed", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
    '    Try
    '        REM regenerate distribution list
    '        Dim sql As String
    '        Dim ocommentary As bc_cs_activity_log
    '        sql = "sp_helptext bcc_core_wf_regen_list"
    '        Dim res As Object
    '        res = gbc_db.executesql_show_no_error(sql)
    '        is_generate_dist_list_installed = True
    '        If IsArray(res) Then
    '            If res(0, 0) = "Error" Then
    '                is_generate_dist_list_installed = False
    '                ocommentary = New bc_cs_activity_log("bc_om_document_db", "write_document", bc_cs_activity_codes.COMMENTARY, "Regenerate Distribution List not installed:  dbo.bcc_core_wf_regen_list", certificate)
    '            End If
    '        End If
    '    Catch ex As Exception
    '        Dim db_err As New bc_cs_error_log("bc_om_document_db", "is_generate_dist_list_installed", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
    '    Finally
    '        otrace = New bc_cs_activity_log("bc_om_document_db", "is_generate_dist_list_installed", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
    '    End Try
    'End Function
    'Public Sub regenerate_distribution_list(ByVal doc_id As String, ByRef certificate As bc_cs_security.certificate)
    '    Dim otrace As New bc_cs_activity_log("bc_om_document_db", "regenerate_distribution_list", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
    '    Try
    '        REM regenerate distribution list
    '        Dim sql As String
    '        Dim ocommentary As bc_cs_activity_log
    '        sql = "exec dbo.bcc_core_wf_regen_list '" + CStr(doc_id) + "'"
    '        Dim res As Object
    '        res = gbc_db.executesql_show_no_error(sql)
    '        If IsArray(res) Then
    '            If res(0, 0) = "Error" Then
    '                ocommentary = New bc_cs_activity_log("bc_om_document_db", "write_document", bc_cs_activity_codes.COMMENTARY, "Regenerate Distribution List not installed:  dbo.bcc_core_wf_regen_list", certificate)
    '            End If
    '        End If
    '    Catch ex As Exception
    '        Dim db_err As New bc_cs_error_log("bc_om_document_db", "regenerate_distribution_list", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
    '    Finally
    '        otrace = New bc_cs_activity_log("bc_om_document_db", "regenerate_distribution_list", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
    '    End Try
    'End Sub
    'Public Function ReplaceSpecialChars(ByVal s As String) As String
    '    Dim strValid As String = s.Trim
    '    Dim i As Integer
    '    For i = 1 To 144
    '        If Len(strValid) = 0 Then
    '            Return ""
    '        End If
    '        If (i < 32 Or i > 125) Then
    '            strValid = Replace(strValid, Chr(i).ToString, "", 1, , CompareMethod.Binary)
    '        End If
    '    Next
    '    For i = 145 To 255
    '        If Len(strValid) = 0 Then
    '            Return ""
    '        End If
    '        If (Not (i > 144 And i < 149)) AndAlso (i <> 150 And i <> 153 And i <> 169 And i <> 174) Then
    '            strValid = Replace(strValid, Chr(i).ToString, " ", 1, , CompareMethod.Binary)
    '        End If
    '    Next
    '    ReplaceSpecialChars = strValid
    'End Function
    'Public Sub insert_46_revert(ByVal stage_id, ByVal doc_id, ByVal doc_ext, ByVal doc_sequence)
    '    Dim sql As String
    '    Dim res As Object
    '    sql = "select container_id from document_table where doc_id=" + CStr(doc_id)
    '    res = gbc_db.executesql_show_no_error(sql)
    '    If IsArray(res) Then
    '        sql = "insert into  wf_revert_documents(container_id, stage_id, doc_id, doc_ext,doc_object,if_publish,doc_sequence) values ('" + CStr(res(0, 0)) + "'," + CStr(stage_id) + ",'" + CStr(doc_id) + "','.doc','" + CStr(doc_id) + ".doc',1," + CStr(doc_sequence) + ")"
    '        gbc_db.executesql_show_no_error(sql)
    '    End If
    'End Sub

    'Public Sub log_metadata_changed_only(ByVal doc_id As Long, ByRef certificate As bc_cs_security.certificate)
    '    Dim sql As String
    '    If bc_cs_central_settings.server_flag = 0 Then
    '        certificate = New bc_cs_security.certificate

    '        certificate.user_id = bc_cs_central_settings.logged_on_user_id
    '    End If
    '    sql = "dbo.bc_core_log_mc_changed " + CStr(doc_id) + "," + CStr(certificate.user_id)
    '    gbc_db.executesql(sql, certificate)
    'End Sub

    'Public Sub delete_constituents(ByVal mdoc_id As Long, ByRef certificate As bc_cs_security.certificate)
    '    Dim sql As String
    '    sql = "delete from doc_comp_constituents_tbl  where master_doc_id=" + CStr(mdoc_id)
    '    gbc_db.executesql(sql, certificate)
    'End Sub
    'Public Sub write_constituents(ByVal mdoc_id As Long, ByVal cdoc_id As Long, ByVal ord As Integer, ByVal certificate As bc_cs_security.certificate)
    '    Dim sql As String
    '    REM SR - Column names included in the insert statement
    '    sql = "insert into doc_comp_constituents_tbl (master_doc_id, sub_doc_id, ord ) values(" + CStr(mdoc_id) + "," + CStr(cdoc_id) + "," + CStr(ord) + ")"
    '    gbc_db.executesql(sql, certificate)
    'End Sub
    Public Sub upload_translated_component(ByVal doc_id As Long, ByVal type_id As Long, ByVal entity_id As Long, ByVal value As String, ByVal locator As String, ByVal certificate As bc_cs_security.certificate)
        Dim otrace As New bc_cs_activity_log("bc_om_document_db", "upload_translated_component", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Dim sql As String
            Dim ocom As bc_cs_activity_log
            ocom = New bc_cs_activity_log("fff", "fff", bc_cs_activity_codes.COMMENTARY, "1", certificate)

            sql = "delete from doc_comp_tbl where doc_id=" + CStr(doc_id) + " and at_component_type_id=" + CStr(type_id)
            ocom = New bc_cs_activity_log("fff", "fff", bc_cs_activity_codes.COMMENTARY, "2", certificate)
            gbc_db.executesql(sql, certificate)
            ocom = New bc_cs_activity_log("fff", "fff", bc_cs_activity_codes.COMMENTARY, "3", certificate)
            Dim st As New bc_cs_string_services(value)
            value = st.delimit_apostrophies()
            ocom = New bc_cs_activity_log("fff", "fff", bc_cs_activity_codes.COMMENTARY, "4", certificate)
            sql = "exec dbo.bcc_core_insert_document_component " + CStr(doc_id) + "," + CStr(entity_id) + "," + CStr(type_id) + ",'" + value + "','" + CStr(locator) + "',1,1"
            ocom = New bc_cs_activity_log("fff", "fff", bc_cs_activity_codes.COMMENTARY, "5", certificate)

            gbc_db.executesql(sql, certificate)
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_documents_db", "upload_translated_component", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_document_db", "upload_translated_component", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub

    Public Function get_history(ByVal doc_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "exec dbo.bc_core_get_document_history " + CStr(doc_id)
        get_history = gbc_db.executesql(sql, certificate)
    End Function
    'Public Function write_taxonomy_for_document(ByVal doc_id As Long, ByVal entity_id As Long, ByRef certificate As bc_cs_security.certificate) As Boolean
    '    Dim sql As String
    '    If entity_id = 0 Then
    '        Exit Function
    '    End If
    '    REM delete existing record if it exists
    '    sql = "delete from doc_entity_tbl where doc_id=" + CStr(doc_id) + " And entity_id = " + CStr(entity_id)
    '    gbc_db.executesql(sql, certificate)
    '    sql = "insert into doc_entity_tbl(doc_id,entity_id) values(" + CStr(doc_id) + "," + CStr(entity_id) + ")"
    '    gbc_db.executesql(sql, certificate)
    'End Function
    Public Function has_link(ByVal doc_id As Long, ByVal link_id As Integer, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "exec dbo.bc_core_get_document_links " + CStr(doc_id) + "," + CStr(link_id)
        has_link = gbc_db.executesql(sql, certificate)
    End Function
    'Public Sub write_tmp_disclosures(ByVal doc_id As Long, ByVal tmp_doc_id As String, ByRef certificate As bc_cs_security.certificate)
    '    Dim sql As String
    '    REM copy over tmp ones
    '    sql = "exec dbo.bcc_core_copy_tmp_disclosures '" + tmp_doc_id + "'," + CStr(doc_id)
    '    gbc_db.executesql_show_no_error(sql)
    'End Sub
    'Public Function get_user_name_for_id(ByVal id As Long, ByVal certificate As bc_cs_security.certificate) As Object
    '    Dim sql As String
    '    sql = "select first_name + ' ' + surname from user_table where user_id =" + CStr(id)
    '    get_user_name_for_id = gbc_db.executesql(sql, certificate)
    'End Function
    Public Sub write_history(ByVal doc_id As Long, ByVal tx As String, ByVal code As String, ByVal stage_from As Long, ByVal stage_to As Long, Optional ByRef certificate As bc_cs_security.certificate = Nothing)
        Dim sql As String
        Dim s As New bc_cs_string_services(tx)
        tx = s.delimit_apostrophies

        If bc_cs_central_settings.server_flag = 0 Then
            certificate.user_id = bc_cs_central_settings.logged_on_user_id
        End If
        sql = "bc_core_write_history " + CStr(doc_id) + ",'" + tx + "'," + CStr(certificate.user_id) + ",'" + CStr(code) + "'," + CStr(stage_from) + "," + CStr(stage_to)
        gbc_db.executesql(sql, certificate)
    End Sub

    Public Function get_regular_report_entities(ByVal id As Long, ByVal certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        'sql = "select entity_id, primary_entity from regular_report_entity_table where regular_report_id=" + CStr(id)
        sql = "exec dbo.bc_core_get_regular_report_entities " + CStr(id)
        get_regular_report_entities = gbc_db.executesql(sql, certificate)
    End Function
    Public Function get_regular_reports(ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        If bc_cs_central_settings.server_flag = 0 Then
            sql = "exec dbo.bc_core_get_regular_reports " + CStr(bc_cs_central_settings.logged_on_user_id)
        Else
            sql = "exec dbo.bc_core_get_regular_reports " + certificate.user_id
        End If
        get_regular_reports = gbc_db.executesql(sql, certificate)

    End Function
    Public Function allow_disclosure_tab(ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        If bc_cs_central_settings.server_flag = 0 Then
            sql = "exec dbo.bc_core_get_allow_disclosure_tab " + CStr(bc_cs_central_settings.logged_on_user_id)
        Else
            sql = "exec dbo.bc_core_get_allow_disclosure_tab " + certificate.user_id
        End If
        allow_disclosure_tab = gbc_db.executesql(sql, certificate)
    End Function
    'Public Function get_regular_report(ByVal id As Long, ByRef certificate As bc_cs_security.certificate)
    '    Dim sql As String
    '      sql = "select regular_report_name, doc_title,doc_sub_title,doc_summary,pub_type_id,language_id from regular_report_table where regular_report_id=" + CStr(id)
    '    get_regular_report = gbc_db.executesql(sql, certificate)
    'End Function
    Public Function get_regular_report_disclosures(ByVal id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "exec dbo.bc_core_get_regular_report_disclosures " + CStr(id)
        get_regular_report_disclosures = gbc_db.executesql(sql, certificate)
    End Function
    Public Function get_regular_report_authors(ByVal id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "exec dbo.bc_core_get_regular_report_authors " + CStr(id)
        get_regular_report_authors = gbc_db.executesql(sql, certificate)
        '//sql = "select user_id, principal from regular_report_user_table where regular_report_id=" + CStr(id) + " order by ord asc"

    End Function
    Public Sub check_in_document(ByVal doc_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "update document_table set user_id=0 where doc_id=" + CStr(doc_id)
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Sub set_container_stage(ByVal container_id As Long, ByVal stage_from_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "update container_tbl set stage_id=" + CStr(stage_from_id) + " where container_id=" + CStr(container_id)
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Function get_html_product(id As Long, ByRef certificate As bc_cs_security.certificate)
        get_html_product = gbc_db.executesql("exec dbo.bc_core_get_html_product " + CStr(id), certificate)
    End Function
    Public Function check_out_document(ByVal doc_id As Long, ByVal stage_id As Long, ByRef certificate As bc_cs_security.certificate, Optional ByVal support_doc As Boolean = False, Optional ByVal sfn As String = "") As String
        Dim sql As String
        If bc_cs_central_settings.server_flag = 0 Then
            certificate.user_id = bc_cs_central_settings.logged_on_user_id
        End If
        check_out_document = certificate.user_id
        Dim ocommentary As New bc_cs_activity_log("bc_om_document", "check_out_document", bc_cs_activity_codes.COMMENTARY, "Checking Document out to: " + CStr(certificate.user_id), certificate)
        sql = "exec dbo.bc_core_check_out_doc " + CStr(certificate.user_id) + "," + CStr(doc_id)
        gbc_db.executesql(sql, certificate)
        If support_doc = True Then
            write_history(doc_id, "Support Doc: " + sfn + " - Checked Out", "SO", stage_id, stage_id, certificate)
        Else
            write_history(doc_id, "Checked Out", "CO", stage_id, stage_id, certificate)
        End If
    End Function
    Public Function run_server_side_event_sql(ByVal sql As String, ByRef result_tx As String, ByRef certificate As bc_cs_security.certificate) As Boolean
        Dim res As Object
        Dim ocommentary As New bc_cs_activity_log("bc_om_document_db", "run_server_side_event_sql", bc_cs_activity_codes.COMMENTARY, "Attempting to run SQL: " + sql, certificate)
        run_server_side_event_sql = True
        res = gbc_db.executesql_show_no_error(sql)
        If IsArray(res) Then
            If res(0, 0) = "Error" Then
                ocommentary = New bc_cs_activity_log("bc_om_document_db", "run_server_side_event_sql", bc_cs_activity_codes.COMMENTARY, "Error running SQL: " + sql, certificate)
                run_server_side_event_sql = False
            Else
                result_tx = res(0, 0)
            End If
        End If
    End Function
    Public Function get_current_stage(ByVal container_id As Long, ByVal certificate As bc_cs_security.certificate) As Long
        Dim sql As String
        Dim res As Object
        sql = "select stage_id from container_tbl where container_id=" + CStr(container_id)
        res = gbc_db.executesql(sql, certificate)
        If IsArray(res) Then
            get_current_stage = CLng(res(0, 0))
        End If
    End Function
    Public Sub revert_stage(ByVal container_id As Long, ByVal stage As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "update container_tbl set stage_id=" + CStr(stage) + " where container_id=" + CStr(container_id)
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Sub set_no_auto_taxonomy(ByVal doc_id As Long, ByVal certificate As bc_cs_security.certificate)
        Dim sql As String
        'REM SR - table bcc_core_first_doc_sub not exists (redundant table)
        sql = "insert into bcc_core_first_doc_sub values(" + CStr(doc_id) + ")"
        gbc_db.executesql_show_no_error(sql)
    End Sub
    Public Sub bc_core_set_stage(ByVal doc_id As Long, ByVal stage_to As Long, ByVal user_id As Long, ByVal certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "exec dbo.bc_core_set_stage " + CStr(doc_id) + "," + CStr(stage_to) + "," + CStr(user_id)
        gbc_db.executesql(sql, certificate)
    End Sub


    'Public Sub approve_doc(ByVal doc_id As Long, ByVal stage_from As Long, ByVal stage_to As String, ByVal user_id As Long, ByRef certificate As bc_cs_security.certificate)
    '    Dim sql As String
    '    SSSS()


    '    ' REM SR - Column names included in the following insert statement
    '    sql = "insert into doc_approvers_for_stage (doc_id, [user_id], stage_from ,stage_to) values(" + CStr(doc_id) + "," + CStr(user_id) + "," + CStr(stage_from) + "," + CStr(stage_to) + ")"
    '    gbc_db.executesql(sql, certificate)
    'End Sub
    Public Function db_read_check_document_state(ByVal id As Long, ByVal stage_id As Long, ByVal certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        If bc_cs_central_settings.server_flag = 0 Then
            certificate.user_id = bc_cs_central_settings.logged_on_user_id

        End If
        sql = "exec dbo.bc_core_check_document_state " + CStr(id) + "," + CStr(stage_id) + "," + certificate.user_id

        db_read_check_document_state = gbc_db.executesql(sql, certificate)
    End Function



    Public Sub delete_approvers_for_doc(ByVal doc_Id As Long, ByVal certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "delete from doc_approvers_for_stage where doc_id=" + CStr(doc_Id)
        gbc_db.executesql(sql, certificate)
    End Sub

    Public Function get_docs_for_container(ByVal container_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "select doc_id from document_table where container_id=" + CStr(container_id) + " and coalesce(deleted,0)=0 order by doc_date desc"
        get_docs_for_container = gbc_db.executesql(sql, certificate)
    End Function
    Public Function read_support_documents_for_display(ByVal id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "exec dbo.bc_core_support_docs_for_display " + CStr(id)
        read_support_documents_for_display = gbc_db.executesql(sql, certificate)
    End Function


    Public Function get_container_id_for_document(ByVal doc_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select container_id from document_table where doc_id=" + CStr(doc_id)
        get_container_id_for_document = gbc_db.executesql(sql, certificate)
    End Function
    Public Function get_title_for_id(ByVal id As String, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select doc_title from document_table where doc_id='" + CStr(id) + "'"
        get_title_for_id = gbc_db.executesql(sql, certificate)
    End Function
    Public Function get_translation_file(ByVal doc_id As String, ByVal certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select e.doc_id from document_table d,document_table e, container_tbl l where d.container_id = l.container_id And e.container_id = l.container_id And d.doc_id = " + CStr(doc_id) + " and e.doc_title = 'Translation Components'"
        get_translation_file = gbc_db.executesql(sql, certificate)
    End Function
    Public Function get_rep_list_items(ByVal rep_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "exec dbo.bcc_core_cat_get_lists_for_reg_rep " + CStr(rep_id)
        get_rep_list_items = gbc_db.executesql(sql, certificate)
    End Function
    Public Function taxonomy_item_system_set(ByVal entity_id As Long, ByVal doc_id As Long, ByRef certificate As bc_cs_security.certificate) As Boolean
        Dim sql As String
        Dim res As Object
        taxonomy_item_system_set = False
        sql = "select count(*) from doc_entity_system_set_tbl where doc_id=" + CStr(doc_id) + " and entity_id=" + CStr(entity_id)
        res = gbc_db.executesql_show_no_error(sql)
        If IsArray(res) Then
            If IsNumeric(res(0, 0)) Then
                If CInt(res(0, 0)) > 0 Then
                    taxonomy_item_system_set = True
                End If
            End If
        End If
    End Function
    Public Function get_list_items(ByVal doc_id As String, ByVal certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "exec dbo.bcc_core_cat_get_lists_for_doc " + CStr(doc_id)
        get_list_items = gbc_db.executesql_show_no_error(sql)
    End Function
    Public Function read_disclosure_file_name(pub_type_id As Long, doc_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "exec dbo.bc_core_get_disclosure_filename " + CStr(pub_type_id) + "," + CStr(doc_id)
        read_disclosure_file_name = gbc_db.executesql(sql, certificate)
    End Function
    Public Function read_alt_process_summary_text(doc_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "exec dbo.bc_core_get_alt_process_summary_text " + CStr(doc_id)
        read_alt_process_summary_text = gbc_db.executesql(sql, certificate)
    End Function

    Public Function get_access_rights(ByVal stage_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        Dim luserid As String
        If bc_cs_central_settings.server_flag = 0 Then
            luserid = bc_cs_central_settings.logged_on_user_id
        Else
            luserid = certificate.user_id
        End If
        sql = "exec dbo.bc_core_get_access_right_for_doc " + CStr(stage_id) + "," + CStr(luserid)

        get_access_rights = gbc_db.executesql(sql, certificate)
    End Function
    Public Function get_master_translated_doc_text(ByVal doc_id As String, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select doc_title, doc_id from document_table where doc_pre_render_filename='" + CStr(doc_id) + "'"
        get_master_translated_doc_text = gbc_db.executesql(sql, certificate)
    End Function
    Public Function get_sub_doc_text(ByVal doc_id As String, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select e.doc_title, e.doc_Id ,d.doc_title, d.doc_id, p.pub_type_name from document_table d, document_table e, pub_type_table p where d.doc_id='" + CStr(doc_id) + "'" + _
        " and isnumeric(d.doc_pre_render_filename)=1  and e.doc_id=d.doc_pre_render_filename and p.pub_type_id=e.doc_pub_type_id"
        get_sub_doc_text = gbc_db.executesql(sql, certificate)
    End Function

    Public Sub publish(ByVal master_doc_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "dbo.bcc_core_wf_set_publish " + CStr(master_doc_id)
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Function get_role_id(ByVal user_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select role_id from user_table where user_id=" + CStr(user_id)
        get_role_id = gbc_db.executesql(sql, certificate)
    End Function
    Public Sub New()

    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
    Public Function get_original_pdf(ByVal container_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select doc_id from document_table d, container_tbl c where d.container_id=c.container_id and (d.doc_extension = '.pdf' or if_publish=1) and c.container_id=" + CStr(container_id) + " order by doc_date asc"
        get_original_pdf = gbc_db.executesql(sql, certificate)
    End Function
    Public Function get_mandatory_fields(ByVal doc_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "exec dbo.bcc_core_mandatory_fields " + CStr(doc_id)
        get_mandatory_fields = gbc_db.executesql_show_no_error(sql)
    End Function
    Public Function get_entity_classify_filters(ByVal doc_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "exec dbo.bcc_core_entity_filters " + CStr(doc_id)
        get_entity_classify_filters = gbc_db.executesql_show_no_error(sql)
    End Function


End Class
    REM used to hold users workflow settings
    <Serializable()> Public Class bc_om_list_items
        Public list_items As New ArrayList

        Public Sub New()

        End Sub
    End Class
    <Serializable()> Public Class bc_om_list_item
        Public list_id As Long
        Public item_id As String
    End Class
    <Serializable()> Public Class bc_om_regular_reports
        Inherits bc_cs_soap_base_class
        Public regular_reports As New List(Of bc_om_regular_report)
        Public allow_disclosure_tab As Boolean = True

        Public Overrides Sub process_object()
            Select Case tmode
                Case tREAD
                    db_read()
            End Select
        End Sub


        REM read all regular reports
        Public Sub db_read()
            Try
                Dim vres As Object
                Dim oregrep As bc_om_regular_report
                Dim i As Integer
                Dim gdb As New bc_om_document_db
                vres = gdb.get_regular_reports(MyBase.certificate)
                If IsArray(vres) Then
                    For i = 0 To UBound(vres, 2)
                        oregrep = New bc_om_regular_report
                        oregrep.certificate = MyBase.certificate
                        oregrep.id = vres(0, i)
                        oregrep.name = CStr(vres(1, i))
                        oregrep.doc.title = CStr(vres(2, i))
                        oregrep.doc.sub_title = CStr(vres(3, i))
                        oregrep.doc.summary = CStr(vres(4, i))
                        oregrep.doc.pub_type_id = CStr(vres(5, i))
                        oregrep.doc.language_id = CStr(vres(6, i))
                        oregrep.bglobal = vres(7, i)
                        oregrep.create_user_id = vres(8, i)
                        oregrep.create_user_name = vres(9, i)
                        oregrep.doc.custom_title = vres(10, i)
                        oregrep.doc.summary_html = vres(11, i)
                        oregrep.update_user_id = vres(12, i)
                        oregrep.update_user_name = vres(13, i)
                    oregrep.doc.custom_title2 = vres(14, i)


                    oregrep.db_read()


                        regular_reports.Add(oregrep)
                    Next
                End If

                vres = gdb.allow_disclosure_tab(MyBase.certificate)
                If IsArray(vres) Then
                    If UBound(vres, 2) = 0 Then
                        allow_disclosure_tab = vres(0, 0)

                    End If
                End If


            Catch ex As Exception
                Dim oerr As New bc_cs_error_log("bc_om_regular_reports", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, MyBase.certificate)

            End Try
        End Sub
    End Class
    <Serializable()> Public Class bc_om_regular_report
        Inherits bc_cs_soap_base_class
        Public id As Long
        Public name As String
        Public bglobal As Boolean
        Public update_user_id As Long
        Public update_user_name As String
        Public create_user_id As Long
        Public create_user_name As String
        Public write_mode As EWRITE_MODE

        Public Enum EWRITE_MODE
            DELETE = 0
            UPDATE = 1
        End Enum

        Public doc As New bc_om_document
        Public Overrides Sub process_object()
            Dim gdb As New bc_om_document_db

            db_write()
        End Sub
        Public Sub db_write()
            Dim gdb As New bc_om_document_db
            Select Case write_mode
                Case EWRITE_MODE.DELETE
                    gdb.delete_regular_report(Me.id, certificate)

                Case EWRITE_MODE.UPDATE
                    update_regular_report()

            End Select
        End Sub
        Public Sub update_regular_report()
            Try
                Dim gdb As New bc_om_document_db
                Dim res As Object
                If Me.id <> 0 Then
                    res = gdb.delete_regular_report(Me.id, certificate)
                    If IsArray(res) Then
                        If UBound(res, 2) = 0 Then
                            Me.create_user_id = res(0, 0)
                        End If

                    End If
                End If


                res = gdb.add_regular_report(Me.id, Me.bglobal, Me.name, Me.doc.title, Me.doc.sub_title, Me.doc.summary, Me.doc.pub_type_id, Me.doc.custom_title, Me.doc.summary_html, Me.create_user_id, Me.doc.custom_title2, certificate)
                If IsArray(res) Then
                    If UBound(res, 2) = 0 Then
                        Me.id = res(0, 0)
                    End If
                End If

                If Me.doc.entity_id <> 0 Then
                    gdb.add_entity_for_regular_report(Me.id, Me.doc.entity_id, True, certificate)
                End If
                For i = 0 To Me.doc.taxonomy.Count - 1
                    gdb.add_entity_for_regular_report(Me.id, Me.doc.taxonomy(i).entity_id, False, certificate)
                Next
                For i = 0 To Me.doc.authors.Count - 1
                    gdb.add_user_for_regular_report(Me.id, Me.doc.authors(i).id, certificate)
                Next
                For i = 0 To Me.doc.disclosures.Count - 1
                    gdb.add_disclosure_for_regular_report(Me.id, Me.doc.disclosures(i).entity_id, certificate)
                Next
            Catch ex As Exception
                Dim oerr As New bc_cs_error_log("bc_om_regular_report", "update_regular_report", bc_cs_error_codes.USER_DEFINED, ex.Message, MyBase.certificate)

            End Try
        End Sub

        Public Sub db_read()
            Dim vres As Object
            Dim i As Integer
            Dim gdb As New bc_om_document_db
            Dim otax As bc_om_taxonomy
            'vres = gdb.get_regular_report(Me.id, MyBase.certificate)
            'If IsArray(vres) Then
            '    Me.name = CStr(vres(0, 0))
            '    doc.title = CStr(vres(1, 0))
            '    doc.sub_title = CStr(vres(2, 0))
            '    doc.summary = CStr(vres(3, 0))
            '    doc.pub_type_id = CStr(vres(4, 0))
            '    doc.language_id = CStr(vres(5, 0))
            '    Me.bglobal = vres(6, 0)
            '    Me.user_id = vres(7, 0)
            '    Me.user_name = vres(8, 0)
            'End If
            vres = gdb.get_regular_report_entities(Me.id, MyBase.certificate)
            If IsArray(vres) Then
                For i = 0 To UBound(vres, 2)
                    If CBool(vres(1, i)) = True Then
                        doc.entity_id = vres(0, i)
                    End If
                    otax = New bc_om_taxonomy(CLng(vres(0, i)))
                    otax.name = vres(2, i)
                    otax.class_id = vres(3, i)
                    otax.class_name = vres(4, i)
                    doc.taxonomy.Add(otax)
                Next
            End If
            Dim olist As bc_om_list_item
            vres = gdb.get_rep_list_items(Me.id, MyBase.certificate)
            If IsArray(vres) Then
                For i = 0 To UBound(vres, 2)
                    olist = New bc_om_list_item
                    olist.list_id = vres(0, i)
                    olist.item_id = vres(1, i)
                    doc.list_items.list_items.Add(olist)
                Next
            End If
            REM authors
            vres = gdb.get_regular_report_authors(Me.id, MyBase.certificate)
            Dim oauthor As bc_om_user
            doc.authors.Clear()
            If IsArray(vres) Then
                For i = 0 To UBound(vres, 2)
                    oauthor = New bc_om_user(vres(0, i), "", "", "", 0, "", "", "", MyBase.certificate)
                    doc.authors.Add(oauthor)
                Next
            End If
            REM disclosures
            vres = gdb.get_regular_report_disclosures(Me.id, MyBase.certificate)
            Dim odisc As bc_om_taxonomy
            doc.disclosures.Clear()
            If IsArray(vres) Then
                For i = 0 To UBound(vres, 2)
                    odisc = New bc_om_taxonomy(CLng(vres(0, i)))
                    doc.disclosures.Add(odisc)
                Next
            End If
        End Sub
    End Class

    <Serializable()> Public Class bc_om_workflow_settings
        Inherits bc_cs_soap_base_class
        REM polling interval
        Public polling_interval As Long
        REM polling enabled switch
        Public polling_enabled As Boolean
        Public screen_update As Boolean
        Public alerter_enabled As Boolean
        REM alert nefore expire in days
        Public pre_expire_alert_notify As Integer
        REM array of docs that are still marked as new
        Public ndocs As New bc_om_documents
        Public mode As Integer
        Public unread_mode As Boolean
        Public fade_interval As Integer
        Public beep_enabled As Boolean
        Public auto_refresh As Boolean
        Public screen_refresh_interval As Integer
        Public screen_inactive_interval As Integer
        Public sentity As New ArrayList
        Public sauthor As New ArrayList
        Public spubtype As New ArrayList
        Public sstage As New ArrayList
        Public sbus As New ArrayList
        Public fdatefrom As New Date
        Public fdateto As New Date
        Public turned_on As Boolean
        Public colors As New Hashtable
        Public days_back As Integer
        Public columnorder As New ArrayList
        Public columnwidth As New ArrayList
        Public filterpanelheight As Integer
        Public snapmode As Boolean = True
        Public refreshsizechanged As Boolean = False
        Public automaticdoclistupdate As Boolean = False
        'TODO Implement saved settings for interface    
        'Public display_filters As Boolean
        'Public display_summary As Boolean
        'Public pin_summary As Boolean
        Public user_inactive_interval As Long

        Public Sub New()
            fdatefrom = "9-9-9999"
            fdateto = "9-9-9999"
        End Sub
    End Class

    <Serializable()> Public Class bc_om_comments
        Inherits bc_cs_soap_base_class
        Public Const MOST_RECENT = -1
        Public Const ALL = -2
        Public comments As New List(Of bc_om_comment)

        Public doc_id As Long
        Public span As Integer
        Public Overrides Sub process_object()

            Select Case tmode
                Case tREAD
                    db_read()

            End Select

        End Sub
        Public Sub db_read()
            Try
                Dim gdb As New bc_om_document_db
                Dim vres As Object = Nothing

                Dim oc As bc_om_comment
                comments.Clear()
                vres = gdb.get_comments_for_doc(doc_id, span, certificate)
                If IsArray(vres) Then
                    For i = 0 To UBound(vres, 2)
                        oc = New bc_om_comment
                        oc.user_name = vres(0, i)
                        oc.comment = vres(1, i)
                        oc.stage_name = vres(2, i)
                        oc.da = vres(3, i)
                        comments.Add(oc)
                    Next
                End If
            Catch ex As Exception
                Dim oerr As New bc_cs_error_log("bc_om_comments", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, MyBase.certificate)

            End Try
        End Sub

        Public Sub New()

        End Sub
    End Class
    <Serializable()> Public Class bc_om_comment
        Inherits bc_cs_soap_base_class
        Public user_id As Long
        Public doc_id As Long
        Public user_name As String
        Public comment As String
        Public stage_id As Long
        Public stage_name As String
        Public da As Date
        Public Overrides Sub process_object()

            Select Case tmode
                Case tWRITE
                    db_write()
            End Select

        End Sub


        Public Sub db_write()
            Dim gdb As New bc_om_document_db
            gdb.write_comment_for_doc(doc_id, comment, certificate)
        End Sub

        Public Sub New()

        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
        End Sub
    End Class
    <Serializable()> Public Class bc_om_pending_document
        Inherits bc_cs_soap_base_class

        Public document As New bc_om_document
        Public pending_document As Boolean = False
        Public stage_id_from As Long
        Public stage_id_to As Long
        Public stage_id_to_name As String
        Public failed As Boolean = False
        Public system_errors As New ArrayList

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
                pending_document = False
                REM check for top 1 pending document
                Dim gdb As New bc_om_pending_document_db
                Dim vres As Object
                vres = gdb.get_next_pending_document(MyBase.certificate)
                If IsArray(vres) Then
                    If UBound(vres, 2) = 0 Then
                        pending_document = True
                        Me.stage_id_to = vres(1, 0)
                        Me.stage_id_to_name = vres(2, 0)
                        Me.stage_id_from = vres(3, 0)
                        document.id = vres(0, 0)
                        document.bwith_document = False
                        REM TBD 
                        'document.db_read()
                        document.id = vres(0, 0)
                    End If
                End If
            Catch ex As Exception
                Dim oerr As New bc_cs_error_log("bc_om_pending_document", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, MyBase.certificate)

            End Try
        End Sub
        Public Sub db_write()
            Try
                Dim gdb As New bc_om_pending_document_db
                For i = 0 To system_errors.Count - 1
                    gdb.set_async_stage_change_error(document.id, system_errors(i), Me.stage_id_from, Me.stage_id_to, MyBase.certificate)
                Next
                gdb.set_pending_document_complete(document.id, failed, MyBase.certificate)

            Catch ex As Exception
                Dim oerr As New bc_cs_error_log("bc_om_pending_document", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message, MyBase.certificate)
            End Try
        End Sub
        Private Class bc_om_pending_document_db
            Private gbc_db As New bc_cs_db_services
            Public Sub set_async_stage_change_error(ByVal id As Long, ByVal tx As String, ByVal stage_from As Long, ByVal stage_to As Long, ByRef certificate As bc_cs_security.certificate)
                Dim sql As String
                Dim fs As New bc_cs_string_services(tx)
                tx = fs.delimit_apostrophies
                If bc_cs_central_settings.server_flag = 0 Then
                    certificate.user_id = bc_cs_central_settings.logged_on_user_id

                End If
                sql = "exec dbo.bc_core_write_history " + CStr(id) + ",'System Error: " + tx + "'," + CStr(certificate.user_id) + ",'ER'," + CStr(stage_from) + "," + CStr(stage_to)
                gbc_db.executesql(sql, certificate)
            End Sub

            Public Function get_next_pending_document(ByRef certificate As bc_cs_security.certificate)
                Dim sql As String
                sql = "exec dbo.bc_core_get_next_pending_document"
                get_next_pending_document = gbc_db.executesql(sql, certificate)
            End Function
            Public Sub set_pending_document_complete(ByVal doc_id As Long, ByVal failed As Boolean, ByRef certificate As bc_cs_security.certificate)
                Dim sql As String
                sql = "exec dbo.bc_core_pending_document_complete " + CStr(doc_id) + "," + CStr(failed)
                gbc_db.executesql(sql, certificate)
            End Sub
        End Class
    End Class
    <Serializable()> Public Class bc_om_check_async_failures
        Inherits bc_cs_soap_base_class
        Public failed_docs As New ArrayList

        Public Overrides Sub process_object()
            Select Case tmode
                Case bc_cs_soap_base_class.tREAD
                    db_read()
            End Select
        End Sub
        Public Sub db_read()
            Try
                Dim gdb As New bc_om_check_async_failures_db
                Dim res As Object
                res = gdb.get_failures(MyBase.certificate)
                failed_docs.Clear()
                If IsArray(res) Then
                    For i = 0 To UBound(res, 2)
                        failed_docs.Add(res(0, i))
                    Next
                End If

            Catch ex As Exception
                Dim oerr As New bc_cs_error_log("bc_om_check_async_failures", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, MyBase.certificate)
            End Try
        End Sub
        Private Class bc_om_check_async_failures_db
            Private gbc_db As New bc_cs_db_services
            Public Function get_failures(ByRef certicate As bc_cs_security.certificate) As Object
                Dim sql As String
                If bc_cs_central_settings.server_flag = 0 Then
                    certicate.user_id = bc_cs_central_settings.logged_on_user_id
                End If
                sql = "exec dbo.bc_core_get_async_doc_failures " + CStr(certicate.user_id)
                get_failures = gbc_db.executesql(sql, certicate)
            End Function
        End Class
    End Class







