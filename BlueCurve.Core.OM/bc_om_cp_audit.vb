Imports BlueCurve.Core.CS
<Serializable> Public Class bc_om_audit_links
    Inherits bc_cs_soap_base_class
    Public pref_type_id As Integer
    Public class_id As Long
    Public entity_id As Long
    Public schema_id As Long
    Public parent As Boolean
    Public user_id As Long
    Public links As New List(Of bc_om_audit_link)
    Public audit_type As EAUDIT_TYPE

    Public Enum EAUDIT_TYPE
        TAXONOMY = 0
        USERS_FOR_PREF = 1
        PREFS_FOR_USER = 2
    End Enum
    Public Overrides Sub process_object()
        Select Case tmode
            Case tREAD
                db_read()
        End Select
    End Sub
    Public Sub db_read()
        Try
            Dim db As New db_bc_om_audit_links
            Dim res As Object
            Dim l As bc_om_audit_link
            If audit_type = EAUDIT_TYPE.TAXONOMY Then
                res = db.get_links_taxonomy(entity_id, class_id, schema_id, parent, certificate)
            ElseIf audit_type = EAUDIT_TYPE.USERS_FOR_PREF Then
                res = db.get_links_aprefs(entity_id, pref_type_id, certificate)
            Else
                res = db.get_links_aprefs_for_user(user_id, pref_type_id, certificate)
            End If
            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    l = New bc_om_audit_link
                    l.name = res(0, i)
                    l.date_from = res(1, i)
                    l.date_to = res(2, i)
                    l.change_user = res(3, i)
                    l.inactive = res(4, i)
                    l.rating = res(5, i)
                    If audit_type = EAUDIT_TYPE.PREFS_FOR_USER Then
                        l.class_name = res(6, i)
                    End If
                    links.Add(l)
                Next
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_audit_links", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)

        End Try

    End Sub
    Private Class db_bc_om_audit_links
        Dim gdb As New bc_cs_db_services
        Public Function get_links_taxonomy(entity_id As Long, class_id As Long, schema_id As Long, parent As Boolean, ByRef cerrificate As bc_cs_security.certificate) As Object
            Dim sql As String
            sql = "exec dbo.bc_core_get_audit_entity_links " + CStr(class_id) + "," + CStr(entity_id) + "," + CStr(schema_id) + "," + CStr(parent)
            get_links_taxonomy = gdb.executesql(sql, cerrificate)
        End Function
        Public Function get_links_aprefs(entity_id As Long, pref_type_id As Long, ByRef cerrificate As bc_cs_security.certificate) As Object
            Dim sql As String
            sql = "exec dbo.bc_core_get_pref_audit_for_entity " + CStr(entity_id) + "," + CStr(pref_type_id)
            get_links_aprefs = gdb.executesql(sql, cerrificate)
        End Function
        Public Function get_links_aprefs_for_user(user_id As Long, pref_type_id As Long, ByRef cerrificate As bc_cs_security.certificate) As Object
            Dim sql As String
            sql = "exec dbo.bc_core_get_pref_audit_for_user  " + CStr(user_id) + "," + CStr(pref_type_id)
            get_links_aprefs_for_user = gdb.executesql(sql, cerrificate)
        End Function
    End Class
End Class
<Serializable> Public Class bc_om_audit_link
    Public name As String
    Public date_from As DateTime
    Public date_to As DateTime
    Public change_user As String
    Public inactive As Boolean
    Public rating As Integer
    Public class_name As String
End Class

<Serializable> Public Class bc_om_attribute_audit
    Inherits bc_cs_soap_base_class
    Public audit_values As New List(Of bc_om_attribute_audit_value)

    Public Enum EFINANCIAL_WORKLOW_STAGES
        DRAFT = 1
        PUBLISH = 8
    End Enum
    Public Enum EATTRIBUTE_TYPE
        ENTITY = 1
        USER_TABLE = 2
        USER_ATTRIBUTE = 3
        BUS_AREA = 4
        USER_ASSOCIATIONS = 5
        OTHER_ROLES = 6
    End Enum
    Public key_id As Long
    Public attribute_id As String
    Public stage_id As EFINANCIAL_WORKLOW_STAGES
    Public attribute_type As EATTRIBUTE_TYPE
    Public all As Boolean

    Public Overrides Sub process_object()
        Select Case tmode
            Case tREAD
                db_read()
        End Select
    End Sub
    Public Sub db_read()
        Try
            Dim db As New db_bc_om_attribute_audit
            Dim vres As Object
            vres = db.get_auditted_values(all, key_id, attribute_id, attribute_type, stage_id, certificate)
            Dim av As bc_om_attribute_audit_value
            If IsArray(vres) Then
                For i = 0 To UBound(vres, 2)
                    av = New bc_om_attribute_audit_value
                    av.attribute_name = vres(0, i)
                    av.user_name = vres(1, i)
                    av.value = vres(2, i)
                    av.date_from = vres(3, i)
                    av.date_to = vres(4, i)
                    audit_values.Add(av)
                Next
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_attribute_audit", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Private Class db_bc_om_attribute_audit
        Dim gdb As New bc_cs_db_services
        Public Function get_auditted_values(all As Boolean, key_id As Long, attribute_id As String, attribute_type As EATTRIBUTE_TYPE, stage_id As EFINANCIAL_WORKLOW_STAGES, ByRef certificate As bc_cs_security.certificate)
            Dim sql As String
            sql = "exec dbo.bc_core_get_auditted_attribute_values " + CStr(key_id) + ",'" + CStr(attribute_id) + "'," + CStr(attribute_type) + "," + CStr(stage_id) + "," + CStr(all)
            get_auditted_values = gdb.executesql(sql, certificate)
        End Function
    End Class
End Class
<Serializable> Public Class bc_om_attribute_audit_value
    Public attribute_name As String
    Public user_name As String
    Public value As String
    Public date_from As DateTime
    Public date_to As DateTime
End Class
<Serializable> Public Class bc_om_entity_user_audit
    Inherits bc_cs_soap_base_class
    Public items As New List(Of bc_om_entity_user_audit_item)
    Public key_id As Long
    Public key_type As EKEY_TYPE

    Public Enum EKEY_TYPE
        ENTITY = 1
        USER = 0
    End Enum
    Public Overrides Sub process_object()
        Select Case tmode
            Case tREAD
                db_read()
        End Select
    End Sub
    Public Sub db_read()
        Try
            Dim res As Object
            Dim item As bc_om_entity_user_audit_item
            Dim db As New db_bc_om_entity_user_audit
            res = db.get_audit_items(key_id, key_type, certificate)
            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    item = New bc_om_entity_user_audit_item
                    item.comment = res(0, i)
                    item.date_from = res(1, i)
                    item.date_to = res(2, i)
                    item.user = res(3, i)

                    items.Add(item)
                Next
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_entity_user_audit", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Private Class db_bc_om_entity_user_audit
        Dim gbc As New bc_cs_db_services
        Public Function get_audit_items(key_id As Long, key_type As EKEY_TYPE, ByRef certificate As bc_cs_security.certificate)
            Dim sql As String
            sql = "exec dbo.bc_core_get_entity_user_audit " + CStr(key_id) + "," + CStr(key_type)
            get_audit_items = gbc.executesql(sql, certificate)
        End Function
    End Class
End Class
<Serializable> Public Class bc_om_entity_user_audit_item
    Public comment As String
    Public date_from As DateTime
    Public date_to As DateTime
    Public user As String
End Class

