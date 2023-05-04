Imports BlueCurve.Core.CS
<Serializable()> Public Class bc_om_non_entity_filter_attributes
    Inherits bc_cs_soap_base_class
    Public class_id As Long
    Public filter_attributes_types As New List(Of bc_om_filter_attribute_type)

    Public Overrides Sub process_object()
        Select Case tmode
            Case tREAD
                db_read()
        End Select
    End Sub
    Public Sub db_read()
        Try
            Dim ofa As New bc_om_filter_attribute_type
            filter_attributes_types.Clear()
            Dim res As Object
            Dim gdb As New bc_cs_db_services

            res = gdb.executesql("exec dbo.bc_core_get_filter_attributes_of_class " + CStr(class_id), certificate)

            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    ofa = New bc_om_filter_attribute_type
                    ofa.class_id = res(0, i)
                    ofa.attribute_id = res(1, i)
                    ofa.attribute_name = res(2, i)
                    ofa.use_in_submit = res(3, i)
                    ofa.display_name = res(4, i)
                    ofa.bus_area_id = res(5, i)
                    ofa.mine = res(6, i)
                    ofa.use_in_build = res(7, i)
                    ofa.certificate = certificate
                    ofa.db_read()
                    filter_attributes_types.Add(ofa)
                Next
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_non_entity_filter_attributes", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        End Try
    End Sub


End Class

<Serializable()> Public Class bc_om_dx_role
    Inherits bc_cs_soap_base_class
    Public Enum EROLEUPDTEMODE
        ADD = 1
        RENAME = 2
        INACTIVE = 3
        ACTIVE = 4
        DELETE = 5
    End Enum

    Public role_id As Long
    Public name As String = ""
    Public write_error_text As String
    Public updatemode As EROLEUPDTEMODE
    Public Overrides Sub process_object()
        Select Case tmode
            Case tWRITE
                db_write()
        End Select
    End Sub
    Public Sub db_write()
        Try
            Dim sqlparam As bc_cs_db_services.bc_cs_sql_parameter
            Dim sqlparams As New List(Of bc_cs_db_services.bc_cs_sql_parameter)
            Dim db As New bc_cs_db_services


            sqlparam = New bc_cs_db_services.bc_cs_sql_parameter
            sqlparam.name = "role_id"
            sqlparam.value = role_id
            sqlparams.Add(sqlparam)
            sqlparam = New bc_cs_db_services.bc_cs_sql_parameter
            sqlparam.name = "name"
            sqlparam.value = name
            sqlparams.Add(sqlparam)
            sqlparam = New bc_cs_db_services.bc_cs_sql_parameter
            sqlparam.name = "mode"
            sqlparam.value = updatemode
            sqlparams.Add(sqlparam)
            sqlparam = New bc_cs_db_services.bc_cs_sql_parameter
            sqlparam.name = "change_user_id"
            If bc_cs_central_settings.server_flag = 0 Then
                sqlparam.value = bc_cs_central_settings.logged_on_user_id
            Else
                sqlparam.value = certificate.user_id
            End If
            sqlparams.Add(sqlparam)
            Dim res As Object
            res = db.executesql_with_parameters("dbo.bc_core_cp_add_update_role", sqlparams, certificate)
            If IsArray(res) Then
                If UBound(res, 2) = 0 Then
                    write_error_text = res(0, 0)
                End If
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_dx_role", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)

        End Try
    End Sub
End Class

<Serializable()> Public Class bc_om_dx_stage_role_access
    Inherits bc_cs_soap_base_class
    Public role_id As Long = 0

    Public stage_roles As New List(Of bc_om_dx_stage_role)
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

            Dim osr As bc_om_dx_stage_role

            res = db.executesql("exec dbo.bc_core_get_stage_role " + CStr(role_id), certificate)
            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    osr = New bc_om_dx_stage_role
                    osr.stage_id = res(0, i)
                    osr.access = res(1, i)
                    stage_roles.Add(osr)
                Next
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_stage_role_access", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        End Try
    End Sub
    <Serializable()> Public Class bc_om_dx_stage_role
        Inherits bc_cs_soap_base_class
        Public role_id As Long
        Public add As Boolean = True
        Public stage_id As Long
        Public access As String

        Public Overrides Sub process_object()
            Select Case tmode
                Case tWRITE
                    db_write()
            End Select
        End Sub
        Public Sub db_write()
            Try
                Dim db As New bc_cs_db_services
                Dim sql As String
                sql = "exec dbo.bc_core_set_stage_role " + CStr(role_id) + "," + CStr(add) + "," + CStr(stage_id) + ",'" + access + "'"
                db.executesql(sql, certificate)

            Catch ex As Exception
                Dim oerr As New bc_cs_error_log("bc_om_dx_stage_role", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            End Try
        End Sub
    End Class
End Class

<Serializable()> Public Class bc_om_dx_cp_apps_for_role
    Inherits bc_cs_soap_base_class
    Public role_id As Long = 0
    Public user_mode As Boolean = False
   
    Public apps As New List(Of bc_om_dx_cp_app_for_role)
    Public Enum ECP_ROLES
        ROLES = 3
        PUBTYPES = 4
        USERS = 5
        ENTITIES = 6
        cSTRUCTURE = 2
        SCHEDULER = 1
    End Enum

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
            Dim user_id As Long
            Dim osr As bc_om_dx_cp_app_for_role
            If user_mode = True Then
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    user_id = bc_cs_central_settings.logged_on_user_id
                Else
                    user_id = certificate.user_id
                End If


            End If
            res = db.executesql("exec dbo.bc_core_get_get_apps_access_for_role " + CStr(role_id) + "," + CStr(user_id), certificate)
            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    osr = New bc_om_dx_cp_app_for_role
                    osr.app_id = res(0, i)
                    osr.app_name = res(1, i)
                    osr.access_level = res(2, i)
                    osr.custom = res(3, i)



                    apps.Add(osr)
                Next
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_dx_cp_apps_for_role", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        End Try
    End Sub


    <Serializable()> Public Class bc_om_dx_cp_app_for_role
        Inherits bc_cs_soap_base_class
        Public app_id As ECP_ROLES
        Public app_name As String
        Public access_level As Integer
        Public custom As Boolean = False
        Public role_id As Long

        Public Overrides Sub process_object()
            Select Case tmode
                Case tWRITE
                    db_write()
            End Select
        End Sub
        Public Sub db_write()
            Try
                Dim db As New bc_cs_db_services
                Dim sql As String
                sql = "exec dbo.bc_core_set_app_role_access " + CStr(role_id) + "," + CStr(app_id) + "," + CStr(access_level)
                db.executesql(sql, certificate)

            Catch ex As Exception
                Dim oerr As New bc_cs_error_log("bc_om_dx_cp_app_for_role", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            End Try
        End Sub
    End Class
End Class

<Serializable()> Public Class bc_om_stages
    Inherits bc_cs_soap_base_class
    Public stages As New List(Of bc_om_entity)
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

            Dim ostage As bc_om_entity

            res = db.executesql("exec dbo.bc_core_get_stages", certificate)
            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    ostage = New bc_om_entity
                    ostage.id = res(0, i)
                    ostage.name = res(1, i)
                    stages.Add(ostage)
                Next
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_stages", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        End Try
    End Sub
End Class

<Serializable()> Public Class bc_om_user_attribute
    Inherits bc_cs_soap_base_class
    Public user_id As Long
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
        Dim cuser As Long
        If bc_cs_central_settings.server_flag = 0 Then
            cuser = bc_cs_central_settings.logged_on_user_id
        Else
            cuser = certificate.user_id
        End If

        sql = "exec dbo.bc_core_set_user_attribute_value " + CStr(user_id) + "," + CStr(att_val.attribute_Id) + ",'" + att_val.value + "'," + CStr(cuser)
        db.executesql(sql, certificate)
    End Sub
End Class


<Serializable()> Public Class bc_om_user_attributes
    Inherits bc_cs_soap_base_class
    Public user_id As Long = 0
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
        res = db.executesql("exec dbo.bc_core_get_user_attributes " + CStr(user_id), certificate)


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
                att.persmission = res(11, i)
                att.is_def = res(12, i)
                att.show_filter = res(13, i)

                attributes.Add(att)


            Next
        End If

        REM values

    End Sub
End Class



<Serializable()> Public Class bc_om_dx_cp_entity_class_attribute
    Inherits bc_cs_soap_base_class
    Public class_id As Long
    Public attribute_id As Long
    Public mode As Emode
    Public Enum Emode
        REMOVE = 1
        MOVE_UP = 2
        MOVE_DOWN = 3
        ASSIGN = 4
        SET_READ_ONLY = 5
        CLEAR_READ_ONLY = 6
        SET_FILTER = 7
        CLEAR_FILTER = 8
    End Enum
    Public Overrides Sub process_object()
        Select Case tmode
            Case tWRITE
                db_write()
        End Select
    End Sub
    Public Sub db_write()

        Try
            Dim gdb As New bc_cs_db_services
            gdb.executesql("exec dbo.bc_core_cp_change_class_attributes " + CStr(class_id) + "," + CStr(attribute_id) + "," + CStr(mode), certificate)

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_dx_cp_entity_class_attribute", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
End Class
<Serializable()> Public Class bc_om_dx_cp_entity_class
    Inherits bc_cs_soap_base_class
    Public class_id As Long
    Public class_name As String
    Public inactive As Boolean
    Public schema_id As Long
    Public parent_classes As New List(Of bc_om_dx_cp_entity_class)
    Public child_classes As New List(Of bc_om_dx_cp_entity_class)
    Public p_c As Integer
    Public c_p As Integer


    Public Sub db_read()
        Try
            Dim db As New bc_cs_db_services
            Dim res As Object
            Dim oclass As bc_om_dx_cp_entity_class
            res = db.executesql("exec dbo.bc_core_cp_get_linked_child_classes " + CStr(schema_id) + "," + CStr(class_id), certificate)
            IsArray(res)
            For i = 0 To UBound(res, 2)
                oclass = New bc_om_dx_cp_entity_class
                oclass.class_id = res(0, i)
                oclass.class_name = res(1, i)
                oclass.schema_id = schema_id
                child_classes.Add(oclass)
            Next
            res = db.executesql("exec dbo.bc_core_cp_get_linked_parent_classes " + CStr(schema_id) + "," + CStr(class_id), certificate)
            IsArray(res)
            For i = 0 To UBound(res, 2)
                oclass = New bc_om_dx_cp_entity_class
                oclass.class_id = res(0, i)
                oclass.class_name = res(1, i)
                oclass.schema_id = schema_id
                parent_classes.Add(oclass)
            Next

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_dx_cp_entity_classe", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
End Class

<Serializable()> Public Class bc_om_dx_cp_entity_classes
    Inherits bc_cs_soap_base_class
    Public classes As New List(Of bc_om_dx_cp_entity_class)
    Public schema_id As Long
    Public get_inactive
    Public Overrides Sub process_object()
        Select Case tmode
            Case tREAD
                db_read()
        End Select
    End Sub
    Public Sub db_read()
        Try
            REM read classes
            Dim vres As Object
            Dim oclass As bc_om_dx_cp_entity_class
            Dim i As Integer
            Dim gdb As New bc_om_entity_db
            vres = gdb.read_all_classes(MyBase.certificate, get_inactive)
            classes.Clear()
            If IsArray(vres) Then
                For i = 0 To UBound(vres, 2)
                    oclass = New bc_om_dx_cp_entity_class
                    oclass.class_id = vres(0, i)
                    oclass.class_name = vres(1, i)
                    oclass.inactive = vres(2, i)
                    oclass.certificate = MyBase.certificate
                    oclass.schema_id = schema_id
                    oclass.db_read()
                    classes.Add(oclass)
                Next
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_dx_cp_entity_classes", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub

End Class

<Serializable()> Public Class bc_om_dx_ext_ax_params
    Inherits bc_cs_soap_base_class
    Public pub_type_id As Long
    Public class_id As Long
    Public mandatory As Boolean
    Public max_number As Long
    Public dependent_on_class As Long

    Public Overrides Sub process_object()

        Select Case Me.tmode
            Case tWRITE

                db_write()
        End Select
    End Sub
    Public Sub db_write()

        Try
            Dim gdb As New bc_cs_db_services
            gdb.executesql("exec dbo.bc_core_cp_add_tax_params " + CStr(pub_type_id) + "," + CStr(class_id) + "," + CStr(mandatory) + "," + CStr(max_number) + "," + CStr(dependent_on_class), certificate)

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_dx_ext_ax_params", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub

End Class

<Serializable()> Public Class bc_om_dx_process_config
    Inherits bc_cs_soap_base_class
    Public actions As New List(Of bc_om_entity)
    Public stages As New List(Of bc_om_entity)
    Public pub_types As New bc_om_pub_types
    Public roles As New List(Of bc_om_entity)

    Public Overrides Sub process_object()

        Select Case Me.tmode
            Case tREAD
                db_read()
        End Select
    End Sub
    Public Sub db_read()

        Try

            pub_types.certificate = certificate
            pub_types.get_inactive = False
            pub_types.pt_only = True

            pub_types.db_read()

            Dim db As New bc_cs_db_services
            Dim res As Object
            res = db.executesql("exec dbo.bc_core_cp_get_stages", certificate)
            Dim stage As bc_om_entity
            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    stage = New bc_om_entity
                    stage.id = res(0, i)
                    stage.name = res(1, i)
                    stages.Add(stage)
                Next
            End If

            res = db.executesql("exec dbo.bc_core_cp_get_actions", certificate)
            Dim action As bc_om_entity
            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    action = New bc_om_entity
                    action.id = res(0, i)
                    action.name = res(1, i)
                    actions.Add(action)
                Next
            End If


            res = db.executesql("exec dbo.bc_core_cp_get_roles", certificate)
            Dim role As bc_om_entity
            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    role = New bc_om_entity
                    role.id = res(0, i)
                    role.name = res(1, i)
                    roles.Add(role)
                Next
            End If


        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_dx_process_config", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
End Class
<Serializable()> Public Class bc_om_dx_pub_type_link_items
    Inherits bc_cs_soap_base_class
    Public link_type As EFIXEDENTITYCLASSES
    Public items As New List(Of bc_om_entity)
    Public Overrides Sub process_object()

        Select Case Me.tmode
            Case tREAD
                db_read()
        End Select
    End Sub
    Public Sub db_read()

        Try
            Dim db As New bc_cs_db_services
            Dim res As Object

            res = db.executesql("exec dbo.bc_core_cp_get_pub_type_link_items " + CStr(link_type), certificate)
            If IsArray(res) Then
                Dim ent As bc_om_entity
                For i = 0 To UBound(res, 2)
                    ent = New bc_om_entity
                    ent.id = res(0, i)
                    ent.name = res(1, i)

                    items.Add(ent)
                Next
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_dx_pub_type_links_item", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
End Class


<Serializable()> Public Class bc_om_dx_pub_type_links
    Inherits bc_cs_soap_base_class
    Public links As New List(Of bc_om_entity)
    Public pub_type_id As Long
    Public link_type As EFIXEDENTITYCLASSES

    Public Overrides Sub process_object()
        Select Case Me.tmode
            Case tREAD
                db_read()
            Case tWRITE
                db_write()
        End Select
    End Sub
    Public Sub db_read()

        Try
            Dim db As New bc_cs_db_services
            Dim res As Object
            Dim ent As bc_om_entity

            res = db.executesql("exec dbo.bc_core_cp_get_pub_type_links " + CStr(pub_type_id) + "," + CStr(link_type), certificate)
            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    ent = New bc_om_entity
                    ent.id = res(0, i)
                    ent.name = res(1, i)
                    If (link_type = EFIXEDENTITYCLASSES.CLASSIFY) Then
                        ent.inactive = res(2, i)
                        ent.class_id = res(3, i)
                    End If
                    links.Add(ent)
                Next
            End If





        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_dx_pub_type_links", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)

        End Try
    End Sub
    Public Sub db_write()

        Try
            Dim db As New bc_cs_db_services
            db.executesql("exec dbo.bc_core_cp_del_pub_type_link " + CStr(pub_type_id) + "," + CStr(link_type), certificate)
            For i = 0 To links.Count - 1
                db.executesql("exec dbo.bc_core_cp_add_pub_type_link " + CStr(pub_type_id) + "," + CStr(link_type) + "," + CStr(links(i).id), certificate)
            Next

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_dx_pub_type_links", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
End Class


<Serializable()> Public Class bc_om_dx_cp_pub_type
    Inherits bc_cs_soap_base_class
    Public id As Long
    Public name As String
    Public config As New bc_om_pt_dist_config
    Public inactive As Boolean
    Public write_mode As Integer
    Public Const INSERT = 0
    Public Const UPDATE_PT = 1
    Public Const DELETE = 2
    Public Const SET_ACTIVE = 3
    Public Const SET_INACTIVE = 4
    Public Const INSERT_AND_SET_DEFAULT_ATTRIBUTE = 5
    Public values_only As Boolean = False

    Public Overrides Sub process_object()
        Select Case Me.tmode
            Case tREAD
                db_read()
            Case tWRITE
                db_write()

        End Select
    End Sub
    Public Sub db_read()

        Try


            Dim gdb As New bc_cs_db_services
            Dim vres As Object
            vres = gdb.executesql("select pub_type_name from pub_type_table where pub_type_id=" + CStr(id), certificate)
            'If IsArray(vres) Then
            '    If UBound(vres, 2) = 1 Then
            name = vres(0, 0)
            'End If
            'End If
            config.pub_type_id = id
            config.certificate = certificate

            config.pub_type_id = id
            config.certificate = certificate
            config.db_read()


        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_dx_cp_pub_type", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Sub

    Public Sub db_write()
        Dim otrace As New bc_cs_activity_log("bc_om_dx_cp_pub_type", "db_write", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Dim gdb As New bc_om_at_pub_type_db




            Select Case Me.write_mode
                REM JIRA 6825
                Case INSERT
                    Me.id = gdb.insert_pub_type_base(Me.name, MyBase.certificate)
                    '    Me.comment = Format("hh:MM:ss", Now)
                Case INSERT_AND_SET_DEFAULT_ATTRIBUTE
                    Me.id = gdb.insert_pub_type_base(Me.name, MyBase.certificate)

                    gdb.set_default_attribute_values(Me.id, MyBase.certificate)

                Case UPDATE_PT
                    gdb.update_pubtype_name(Me.id, Me.name, MyBase.certificate)

                Case DELETE
                    gdb.delete_pub_type(Me.id, MyBase.certificate)
                Case SET_ACTIVE
                    gdb.set_inactive(Me.id, False, MyBase.certificate)
                Case SET_INACTIVE
                    gdb.set_inactive(Me.id, True, MyBase.certificate)
            End Select


        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_dx_cp_pub_type", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message, MyBase.certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_dx_cp_pub_type", "db_write", bc_cs_activity_codes.TRACE_EXIT, "", certificate)

        End Try

    End Sub


End Class


<Serializable()> Public Class bc_om_cp_authenticate_user
    Inherits bc_cs_soap_base_class
    Enum EAUTHENTICATION_METHOD
        OS_LOGON = 0
        USER_NAME_PASSWORD = 1
        AD = 2
    End Enum


    Public user As New bc_om_user
    Public logged_on_user_id As Long = 0
    Public authentication_method As EAUTHENTICATION_METHOD
    Public password_encryted As Boolean = False

    Public Overrides Sub process_object()
        'Select Case tmode
        '    Case tREAD
        'End Select
    End Sub
    Public Sub db_read()
        Try
            Dim gdb As New bc_cs_db_services
            Dim res As Object

            REM may need to encrypt password here


            res = gdb.executesql("exec dbo.bc_core_cp_authenticate_user " + CStr(authentication_method) + ",'" + CStr(user.os_user_name) + "','" + CStr(user.user_name) + "','" + CStr(user.password) + "'", certificate)
            If IsArray(res) Then
                If UBound(res(0, 0)) = 1 Then
                    logged_on_user_id = res(0, 0)
                End If
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_cp_authenticate_users", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub

End Class
<Serializable()> Public Class bc_om_cp_pref_types
    Inherits bc_cs_soap_base_class
    Public pref_type_id As Integer
    Public pref_type_name As String



End Class
Public Enum EFIXEDENTITYCLASSES
    ENTITY = 1
    USER = 0
    BUS_AREA = -1
    ROLE = -2
    LANGUAGE = -3
    OFFICE = -4
    PREF = -5
    PUB_TYPE = -6
    ASSOC_USER = -7
    CHANNEL = -8
    CLASSIFY = -9
    eMODULE = -10
End Enum

<Serializable()> Public Class bc_om_cp_user_links
    Inherits bc_cs_soap_base_class

    Public user_id As Long
    Public area_id As EFIXEDENTITYCLASSES
    Public pref_type_id As Long = 0
    Public area_entities As New List(Of bc_om_entity)

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
            Dim gdb As New bc_cs_db_services
            Dim res As Object
            Dim en As bc_om_entity
            res = gdb.executesql("exec dbo.bc_core_cp_get_user_links " + CStr(user_id) + "," + CStr(area_id) + "," + CStr(pref_type_id), certificate)

            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    en = New bc_om_entity
                    en.id = res(0, i)
                    en.name = res(1, i)
                    If area_id <> EFIXEDENTITYCLASSES.PREF Then
                        en.class_id = area_id
                    Else
                        en.class_id = res(2, i)
                        en.class_name = res(3, i)
                    End If
                    en.inactive = res(4, i)

                    area_entities.Add(en)
                Next
            End If


        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_cp_user_links", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        End Try
    End Sub


    Public Sub db_write()
        Try
            'Dim gdb As New bc_cs_db_services
            Dim udb As New bc_om_user_db
            Select Case area_id
                Case EFIXEDENTITYCLASSES.BUS_AREA
                    udb.delete_bus_area_for_user(user_id, certificate)
                    For i = 0 To area_entities.Count - 1
                        udb.add_bus_area_for_user(user_id, area_entities(i).id, certificate)
                    Next


                Case EFIXEDENTITYCLASSES.ASSOC_USER


                    udb.delete_associated_users(user_id, certificate)
                    For i = 0 To area_entities.Count - 1
                        udb.write_associated_user(user_id, area_entities(i).id, certificate)
                    Next

                Case EFIXEDENTITYCLASSES.ROLE
                    udb.delete_other_roles(user_id, certificate)
                    For i = 0 To area_entities.Count - 1
                        udb.write_other_role(user_id, area_entities(i).id, certificate)
                    Next

                Case EFIXEDENTITYCLASSES.PREF

            End Select

            'If submit_class_id = 0 Then
            '    REM users
            '    gdb.executesql("exec dbo.bc_core_cp_delete_user_prefs " + CStr(pref_type_id) + "," + CStr(entity_id) + "," + user_id, certificate)
            '    For i = 0 To linked_entities.Count - 1
            '        gdb.executesql("exec dbo.bc_core_cp_add_user_pref " + CStr(pref_type_id) + "," + CStr(entity_id) + "," + CStr(linked_entities(i).id) + "," + CStr(i + 1) + "," + user_id, certificate)
            '    Next
            'Else

            '    gdb.executesql("exec dbo.bc_core_cp_delete_links " + CStr(schema_id) + "," + CStr(entity_id) + "," + CStr(submit_class_id) + "," + CStr(parent) + "," + user_id, certificate)
            '    If (parent = True) Then
            '        For i = 0 To linked_entities.Count - 1
            '            gdb.executesql("exec dbo.bc_core_cp_add_link " + CStr(schema_id) + "," + CStr(entity_id) + "," + CStr(linked_entities(i).id) + "," + CStr(parent) + "," + CStr(i + 1) + "," + user_id, certificate)
            '        Next
            '    Else
            '        For i = 0 To linked_entities.Count - 1
            '            gdb.executesql("exec dbo.bc_core_cp_add_link " + CStr(schema_id) + "," + CStr(linked_entities(i).id) + "," + CStr(entity_id) + "," + CStr(parent) + "," + CStr(i + 1) + "," + user_id, certificate)
            '        Next
            '    End If
            'End If
            'gdb.executesql("exec dbo.bc_core_set_audit_entity_links " + CStr(entity_id) + "," + CStr(schema_id), certificate)

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_cp_associations", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
End Class
<Serializable()> Public Class bc_om_cp_entity_links
    Inherits bc_cs_soap_base_class

    Public entity_id As Long
    Public schema_id As Long = 0
    Public pref_type_id As Long = 0
    Public child_classes As New List(Of bc_om_entity_class)
    Public parent_classes As New List(Of bc_om_entity_class)
    Public linked_entities As New List(Of bc_om_entity)
    Public pref_users As New List(Of bc_om_user)
    Public submit_class_id As Long
    Public parent As Boolean


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
            Dim gdb As New bc_cs_db_services
            Dim res As Object
            Dim cl As bc_om_entity_class
            Dim en As bc_om_entity
            Dim us As bc_om_user
            If schema_id > 0 Then

                res = gdb.executesql("exec dbo.bc_core_cp_get_class_links " + CStr(entity_id) + "," + CStr(schema_id), certificate)

                If IsArray(res) Then
                    For i = 0 To UBound(res, 2)
                        cl = New bc_om_entity_class
                        cl.class_id = res(0, i)
                        cl.class_name = res(1, i)
                        cl.class_type_Id = 0
                        If res(2, i) = 0 Then
                            cl.class_type_Id = res(3, i)
                            child_classes.Add(cl)
                        Else
                            cl.class_type_Id = res(4, i)
                            parent_classes.Add(cl)
                        End If
                    Next
                End If
                res = gdb.executesql("exec dbo.bc_core_cp_get_entity_links " + CStr(entity_id) + "," + CStr(schema_id), certificate)

                If IsArray(res) Then
                    For i = 0 To UBound(res, 2)
                        en = New bc_om_entity
                        en.class_id = res(0, i)
                        en.id = res(1, i)
                        en.name = res(2, i)
                        en.inactive = res(3, i)
                        linked_entities.Add(en)

                    Next
                End If
            Else
                res = gdb.executesql("exec dbo.bc_core_cp_get_prefs " + CStr(entity_id) + "," + CStr(pref_type_id), certificate)

                If IsArray(res) Then
                    For i = 0 To UBound(res, 2)
                        us = New bc_om_user
                        us.id = res(0, i)
                        us.user_name = res(1, i)
                        us.inactive = res(2, i)
                        pref_users.Add(us)

                    Next
                End If
            End If


        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_schemas", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        End Try
    End Sub


    Public Sub db_write()
        Try
            Dim gdb As New bc_cs_db_services
            Dim user_id As String
            If bc_cs_central_settings.server_flag = 0 Then
                user_id = bc_cs_central_settings.logged_on_user_id
            Else
                user_id = certificate.user_id
            End If
            If submit_class_id = 0 Then
                REM users
                gdb.executesql("exec dbo.bc_core_cp_delete_user_prefs " + CStr(pref_type_id) + "," + CStr(entity_id) + "," + user_id, certificate)
                For i = 0 To linked_entities.Count - 1
                    gdb.executesql("exec dbo.bc_core_cp_add_user_pref " + CStr(pref_type_id) + "," + CStr(entity_id) + "," + CStr(linked_entities(i).id) + "," + CStr(i + 1) + "," + user_id, certificate)
                Next
            Else

                gdb.executesql("exec dbo.bc_core_cp_delete_links " + CStr(schema_id) + "," + CStr(entity_id) + "," + CStr(submit_class_id) + "," + CStr(parent) + "," + user_id, certificate)
                If (parent = True) Then
                    For i = 0 To linked_entities.Count - 1
                        gdb.executesql("exec dbo.bc_core_cp_add_link " + CStr(schema_id) + "," + CStr(entity_id) + "," + CStr(linked_entities(i).id) + "," + CStr(parent) + "," + CStr(i + 1) + "," + user_id, certificate)
                    Next
                Else
                    For i = 0 To linked_entities.Count - 1
                        gdb.executesql("exec dbo.bc_core_cp_add_link " + CStr(schema_id) + "," + CStr(linked_entities(i).id) + "," + CStr(entity_id) + "," + CStr(parent) + "," + CStr(i + 1) + "," + user_id, certificate)
                    Next
                End If
            End If
            gdb.executesql("exec dbo.bc_core_set_audit_entity_links " + CStr(entity_id) + "," + CStr(schema_id), certificate)

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_cp_associations", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        End Try
    End Sub
End Class

<Serializable()> Public Class bc_om_schemas
    Inherits bc_cs_soap_base_class

    Public schemas As New List(Of bc_om_schema)
    Public pref_types As New List(Of bc_om_cp_pref_types)
    Public Overrides Sub process_object()
        Select Case tmode
            Case tREAD
                db_read()

        End Select
    End Sub
    Public Sub db_read()
        Try
            Dim gdb As New bc_cs_db_services
            Dim res As Object
            res = gdb.executesql("exec dbo.bc_core_cp_get_all_schemas", certificate)
            Dim sch As New bc_om_schema
            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    sch = New bc_om_schema
                    sch.schema_id = res(0, i)
                    sch.schema_name = res(1, i)
                    schemas.Add(sch)
                Next
            End If
            res = gdb.executesql("exec dbo.bc_core_cp_get_pref_types ", certificate)
            Dim pt As New bc_om_cp_pref_types
            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    pt = New bc_om_cp_pref_types
                    pt.pref_type_id = res(0, i)
                    pt.pref_type_name = res(1, i)
                    pref_types.Add(pt)
                Next
            Else
                pt = New bc_om_cp_pref_types
                pt.pref_type_id = 1
                pt.pref_type_name = "Users"
                pref_types.Add(pt)
            End If


        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_schemas", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        End Try
    End Sub
End Class


<Serializable()> Public Class bc_om_all_attributes
    Inherits bc_cs_soap_base_class
    Public attributes As New List(Of bc_om_attribute)
    Public write_mode As Integer
    Public delete_error_text As String
    Public user_attrubutes As Boolean = False
    Public pub_type_attributes As Boolean = False
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
            Dim db As New bc_cs_db_services
            Dim params As New List(Of bc_cs_db_services.bc_cs_sql_parameter)
            Dim param As bc_cs_db_services.bc_cs_sql_parameter


            If write_mode = 1 Then
                REM delete
                Dim res As Object
                res = db.executesql("exec dbo.bc_core_cp_delete_attribute " + attributes(0).attribute_id.ToString, certificate)
                If IsArray(res) Then
                    If UBound(res, 2) = 0 Then
                        delete_error_text = res(0, 0)
                    End If
                End If
            Else
                For i = 0 To attributes.Count - 1
                    params = New List(Of bc_cs_db_services.bc_cs_sql_parameter)


                    param = New bc_cs_db_services.bc_cs_sql_parameter
                    param.name = "attribute_id"
                    param.value = attributes(i).attribute_id
                    params.Add(param)

                    param = New bc_cs_db_services.bc_cs_sql_parameter
                    param.name = "name"
                    param.value = attributes(i).name
                    params.Add(param)

                    param = New bc_cs_db_services.bc_cs_sql_parameter
                    param.name = "type_id"
                    param.value = attributes(i).type_id
                    params.Add(param)

                    param = New bc_cs_db_services.bc_cs_sql_parameter
                    param.name = "lookup"
                    param.value = attributes(i).is_lookup

                    params.Add(param)

                    param = New bc_cs_db_services.bc_cs_sql_parameter
                    param.name = "lookup_sql"
                    param.value = attributes(i).lookup_sql

                    params.Add(param)

                    param = New bc_cs_db_services.bc_cs_sql_parameter
                    param.name = "submission_code"
                    param.value = attributes(i).submission_code

                    params.Add(param)

                    param = New bc_cs_db_services.bc_cs_sql_parameter
                    param.name = "workflow"
                    param.value = attributes(i).show_workflow

                    params.Add(param)

                    param = New bc_cs_db_services.bc_cs_sql_parameter
                    param.name = "nullable"
                    param.value = attributes(i).nullable

                    params.Add(param)

                    param = New bc_cs_db_services.bc_cs_sql_parameter
                    param.name = "def"
                    param.value = attributes(i).is_def

                    params.Add(param)

                    param = New bc_cs_db_services.bc_cs_sql_parameter
                    param.name = "def_sql"
                    param.value = attributes(i).def_sql

                    params.Add(param)


                    param = New bc_cs_db_services.bc_cs_sql_parameter
                    param.name = "length"
                    param.value = attributes(i).length

                    params.Add(param)

                    param = New bc_cs_db_services.bc_cs_sql_parameter
                    param.name = "repeats"
                    param.value = attributes(i).repeats


                    params.Add(param)

                    param = New bc_cs_db_services.bc_cs_sql_parameter
                    param.name = "user_id"
                    If (bc_cs_central_settings.server_flag = 0) Then
                        param.value = bc_cs_central_settings.logged_on_user_id
                    Else
                        param.value = certificate.user_id
                    End If

                    params.Add(param)

                    db.executesql_with_parameters("dbo.bc_core_cp_insert_update_attribute", params, certificate)
                Next
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_get_all_attributes", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        End Try
    End Sub

    Public Sub db_read()
        Try
            Dim gdb As New bc_cs_db_services
            Dim res As Object
            res = gdb.executesql("exec dbo.bc_core_get_attributes_of_class 0, " + CStr(user_attrubutes) + "," + CStr(pub_type_attributes), certificate)
            Dim att As New bc_om_attribute
            attributes = New List(Of bc_om_attribute)
            Dim lres As Object
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
                    att.is_def = res(10, i)

                    att.def_sql = res(11, i)
                    att.persmission = res(12, i)
                    att.submission_code = res(13, i)

                    If att.is_lookup = True Then
                        'lres = gdb.executesql(att.lookup_sql, certificate)
                        'If IsArray(lres) Then
                        '    For j = 0 To UBound(lres, 2)
                        '        att.lookup_keys.Add(lres(0, j))
                        '        att.lookup_values.Add(lres(1, j))
                        '    Next
                        'End If
                    End If
                    att.default_value = res(6, i)

                    attributes.Add(att)

                Next
            End If


        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_get_all_attributes", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        End Try
    End Sub

End Class
<Serializable()> Public Class bc_om_class_attributes
    Inherits bc_cs_soap_base_class
    Public attributes As New List(Of bc_om_attribute)
    Public class_id As Long
    Public no_lists As Boolean = False
    Public Overrides Sub process_object()
        Select Case tmode
            Case tREAD
                db_read()
        End Select
    End Sub
    Public Sub db_read()
        Try
            Dim gdb As New bc_cs_db_services
            Dim res As Object


            res = gdb.executesql("exec dbo.bc_core_get_attributes_of_class " + CStr(class_id), certificate)
            Dim att As New bc_om_attribute
            attributes = New List(Of bc_om_attribute)
            Dim lres As Object
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
                    att.is_def = res(10, i)

                    att.def_sql = res(11, i)
                    att.persmission = res(12, i)
                    att.submission_code = res(13, i)
                    att.show_filter = res(14, i)

                    If att.is_lookup = True And no_lists = False Then
                        Dim err_tx As String = ""
                        lres = gdb.executesql_return_error_no_tran(att.lookup_sql, certificate, err_tx)
                        If (err_tx <> "") Then
                            att.lookup_keys.Add(0)
                            att.lookup_values.Add("err: " + err_tx)
                        Else
                            If IsArray(lres) Then
                                For j = 0 To UBound(lres, 2)
                                    att.lookup_keys.Add(lres(0, j))
                                    att.lookup_values.Add(lres(1, j))
                                Next
                            End If
                        End If
                    End If
                    att.default_value = res(6, i)

                    attributes.Add(att)

                Next
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_class_attributes", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        End Try
    End Sub

End Class
<Serializable()> Public Class bc_om_dx_filter_attribute_values
    Inherits bc_cs_soap_base_class
    Public class_id As Long
    Public attribute_id As Long
    Public attribute_value As String
    Public results As New List(Of Long)
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
            res = db.executesql("exec dbo.bc_core_co_run_filter " + CStr(class_id) + "," + CStr(attribute_id) + ",'" + CStr(attribute_value) + "'", certificate)
            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    results.Add(res(0, i))
                Next
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_dx_filter_attribute_values", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        End Try
    End Sub
End Class

<Serializable()> Public Class bc_om_entities_of_a_class
    Inherits bc_cs_soap_base_class
    Public entities As New List(Of bc_om_entity)
    Public class_id As Long
    Public inactive As Boolean
    'Public attributes As New List(Of bc_om_attribute)

    Public filter_attributes_types As New List(Of bc_om_filter_attribute_type)
    Public Overrides Sub process_object()
        Select Case tmode
            Case tREAD
                db_read()
        End Select
    End Sub
    Public Sub db_read()
        Try
            Dim gdb As New bc_cs_db_services
            Dim res As Object
            res = gdb.executesql("exec dbo.bc_core_get_entities_of_a_class " + CStr(class_id) + "," + CStr(inactive), certificate)
            Dim entity As New bc_om_entity
            entities.Clear()

            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    entity = New bc_om_entity
                    entity.id = res(0, i)
                    entity.name = res(1, i)
                    entity.inactive = res(2, i)
                    entities.Add(entity)
                Next
            End If


            REM filter attributes
            res = gdb.executesql("exec dbo.bc_core_get_filter_attributes_of_class " + CStr(class_id), certificate)

            Dim ofa As New bc_om_filter_attribute_type
            filter_attributes_types.Clear()

            If IsArray(res) Then
                For i = 0 To UBound(res, 2)

                    ofa = New bc_om_filter_attribute_type
                    ofa.class_id = res(0, i)
                    ofa.attribute_id = res(1, i)
                    ofa.attribute_name = res(2, i)
                    ofa.use_in_submit = res(3, i)
                    ofa.display_name = res(4, i)
                    ofa.bus_area_id = res(5, i)
                    ofa.mine = res(6, i)
                    ofa.use_in_build = res(7, i)
                    ofa.certificate = certificate


                    ofa.db_read()
                    filter_attributes_types.Add(ofa)
                Next
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_entities_of_a_class", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        End Try
    End Sub


End Class
