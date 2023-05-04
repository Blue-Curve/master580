Imports BlueCurve.Core.CS
Imports System.IO
<Serializable()> Public Class bc_om_custom_entity_list
    Inherits bc_cs_soap_base_class
    Public pub_type_id As Long
    Public class_id As Long
    Public entity_list As New List(Of bc_om_entity)
    Public Overrides Sub process_object()

        Select Case tmode
            Case tREAD
                db_read()
        End Select
    End Sub
    Public Sub db_read()
        Try
            Dim res As Object
            Dim gdb As New bc_cs_db_services

            Dim user_id As Long
            If bc_cs_central_settings.server_flag = 1 Then
                user_id = certificate.user_id
            Else
                user_id = bc_cs_central_settings.logged_on_user_id


            End If
            res = gdb.executesql("exec dbo.bc_custom_entity_list " + CStr(user_id) + "," + CStr(pub_type_id) + "," + CStr(class_id), certificate)
            Dim ooent As bc_om_entity
            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    ooent = New bc_om_entity
                    ooent.id = res(0, i)
                    ooent.name = res(1, i)
                    ooent.class_id = res(2, i)
                    ooent.inactive = 0
                    'ooent.show_mode = bc_am_load_objects.obc_prefs.pref(i).show_mode
                    entity_list.Add(ooent)
                Next
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_custom_entity_list", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
End Class

<Serializable()> Public Class bc_om_filter_attribute
    Inherits bc_cs_soap_base_class
    Public class_id As Long
    Property attribute_id As Object
    Public results As New List(Of Long)

    REM real time filteres entities of class for attrbute
    Public Overrides Sub process_object()
        Select Case tmode
            Case tREAD
                db_read()
        End Select
    End Sub
    Public Sub db_read()
        Try
            Dim db_entity As New bc_om_entity_db
            Dim res As Object
            res = db_entity.read_entity_filter_attributes(class_id, attribute_id, certificate)
            results.Clear()

            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    results.Add(res(0, i))
                Next
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_filter_attribute", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
End Class

<Serializable()> Public Class bc_om_filter_attribute_type
    Inherits bc_cs_soap_base_class
    <Serializable()> Public Class bc_om_filter_lu
        Public id As String
        Public name As String
    End Class
    Public class_id As Long
    Public attribute_id As Long
    Public attribute_name As String
    Public use_in_submit As Boolean
    Public use_in_build As Boolean
    Public use_in_insight As Boolean
    Public display_name As String
    Public bus_area_id As Integer
    Public mine As Boolean
    Public filter_lookup As New List(Of bc_om_filter_lu)

    Public Sub db_read()
        Try
            Dim db As New bc_cs_db_services
            Dim res As Object
            Dim fi As bc_om_filter_lu
            res = db.executesql("exec dbo.bc_core_filter_lookups " + CStr(attribute_id) + "," + CStr(class_id), certificate)
            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    fi = New bc_om_filter_lu
                    fi.id = res(0, i)
                    fi.name = res(1, i)
                    If fi.name = "" Then
                        fi.name = fi.id
                    End If
                    filter_lookup.Add(fi)
                Next
            End If


        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_filter_attribute_type", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        End Try
        

    End Sub

End Class

<Serializable()> Public Class bc_om_class_link_attributes
    Inherits bc_cs_soap_base_class
    Public parent_class_Id As Long
    Public child_class_id As Long
    Public schema_id As Long
    Public attributes As New List(Of bc_om_link_attribute)
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
            Dim ola As bc_om_link_attribute
            Me.attributes.Clear()
            sql = "exec dbo.bc_core_get_class_link_attributes"
            res = db.executesql(sql, certificate)
            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    If res(0, i) = parent_class_Id And res(1, i) = child_class_id Then
                        ola = New bc_om_link_attribute
                        ola.attribute_id = res(2, i)
                        ola.attribute_name = res(3, i)
                        Me.attributes.Add(ola)
                    End If
                Next
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_class_link_attributes", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        End Try
    End Sub
End Class
<Serializable()> Public Class bc_om_link_attribute
    Public attribute_id As Long
    Public attribute_name As String
End Class
<Serializable()> Public Class bc_om_search_attribute_for_entity
    Public entity_Id As Long
    Public class_id As Long
    Public entity_name As String
    Public attribute_Id As Long
    Public attribute_name As String
    Public value As String

    Public Sub New()

    End Sub
End Class
<Serializable()> Public Class bc_om_search_attribute_for_entities
    Inherits bc_cs_soap_base_class
    Public search_values As New ArrayList
    Public search_attributes As String
    Public search_class As Long

    Public Overrides Sub process_object()
        Select Case tmode
            Case tREAD
                db_read()
        End Select
    End Sub

    Public Sub db_read()

        Dim vres As Object
        Dim i As Integer
        Dim oent As bc_om_search_attribute_for_entity
        Dim SearchAttribute As bc_om_attribute_value = Nothing

        search_values.Clear()

        vres = Me.read_all_Attribute_entities(Me.search_class, MyBase.certificate)
        If IsArray(vres) Then
            For i = 0 To UBound(vres, 2)
                oent = New bc_om_search_attribute_for_entity
                oent.class_id = vres(0, i)
                oent.entity_Id = vres(1, i)
                oent.attribute_Id = vres(2, i)
                oent.attribute_name = vres(3, i)
                oent.value = vres(4, i)
                oent.entity_name = vres(5, i)
                Me.search_values.Add(oent)
            Next
        End If

    End Sub

    Public Function read_all_Attribute_entities(ByVal search_class As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim gbc_db As New bc_cs_db_services
        Dim sql As String

        sql = "exec dbo.bc_core_get_search_entities " + CStr(search_class)
        read_all_Attribute_entities = gbc_db.executesql(sql, certificate)

    End Function

End Class


<Serializable()> Public Class bc_om_search_attributes

    Inherits bc_cs_soap_base_class
    Public search_attributes As New ArrayList

    Public Overrides Sub process_object()
        Select Case tmode
            Case tREAD
                db_read()
        End Select
    End Sub

    Public Sub db_read()

        Dim vres As Object
        Dim i As Integer
        Dim oattribute As bc_om_search_attribute
        Me.search_attributes.Clear()

        vres = Me.read_all_search_attributes(MyBase.certificate)
        If IsArray(vres) Then
            For i = 0 To UBound(vres, 2)
                oattribute = New bc_om_search_attribute
                oattribute.class_id = vres(0, i)
                oattribute.attribute_Id = vres(1, i)
                Me.search_attributes.Add(oattribute)
            Next
        End If
    End Sub

    Public Function read_all_search_attributes(ByRef certificate As bc_cs_security.certificate) As Object
        Dim gbc_db As New bc_cs_db_services
        Dim sql As String

        sql = " select class_id as class_id ,attribute_id as attribute_id from bc_core_search_attributes_by_class_type"

        read_all_search_attributes = gbc_db.executesql(sql, certificate)
    End Function

End Class
<Serializable()> Public Class bc_om_search_attribute
    Public class_id As Long
    Public attribute_Id As Long
End Class




REM ===============================================
REM Bluecurve Limited 2005
REM Module:       Create Entity Object Model
REM Type:         Object Model
REM Description:  Users
REM               User
REM Version:      1
REM Change history
REM ===============================================
<Serializable()> Public Class bc_om_entities
    Inherits bc_cs_soap_base_class
    Public entity As New List(Of bc_om_entity)
    Public alternate_entity_list As bc_om_entities
    Public disclosure_entities As New ArrayList
    Public disclosure_tab_name As String
    Public get_inactive As Boolean = False
    'Public search_attributes As New bc_om_search_attribute_for_entities
    Public do_search_attributes As Boolean = True
    Public filter_attributes As New List(Of bc_om_entity)
    Public filter_attributes_types As New List(Of bc_om_filter_attribute_type)
    Public last_synced_date As Date = DateAdd(DateInterval.Year, -100, Now.ToUniversalTime)
    Public diffgram_mode As Boolean = False
    Public from_sync As Boolean = False

    Public Overrides Sub process_object()
        Dim otrace As New bc_cs_activity_log("bc_om_entities", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            REM this is always specific to object
            If Me.tmode = bc_cs_soap_base_class.tREAD Then
                db_read()
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_entities", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_entities", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub
    Public Sub db_read()
        Dim otrace As New bc_cs_activity_log("bc_om_entities", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Dim i As Integer
            Dim oentity As bc_om_entity
            Dim vres As Object
            Dim db_entity As New bc_om_entity_db
            Dim ventity As Object
            entity.Clear()
            disclosure_entities.Clear()
            If diffgram_mode = True Then
                ventity = db_entity.read_all_entities_diffgram_mode(last_synced_date, MyBase.certificate)
                If IsArray(ventity) Then
                    For i = 0 To UBound(ventity, 2)
                        oentity = New bc_om_entity(ventity(0, i), ventity(1, i), ventity(2, i), ventity(3, i))
                        oentity.inactive = ventity(4, i)
                        oentity.deleted = ventity(5, i)
                        oentity.certificate = MyBase.certificate
                        entity.Add(oentity)
                    Next
                End If
            Else
                ventity = db_entity.read_all_entities(MyBase.certificate, get_inactive)
                If IsArray(ventity) Then
                    For i = 0 To UBound(ventity, 2)
                        oentity = New bc_om_entity(ventity(0, i), ventity(1, i), ventity(2, i), ventity(3, i))
                        oentity.inactive = ventity(4, i)
                        oentity.comment = ventity(5, i)

                        oentity.certificate = MyBase.certificate
                        entity.Add(oentity)
                    Next
                End If
            End If
            If from_sync = True Then

                ventity = db_entity.read_all_disclosure_entities


                If IsArray(ventity) Then
                    If UBound(ventity, 2) > 0 Then
                        If IsNumeric(ventity(0, 0)) Then
                            For i = 0 To UBound(ventity, 2)
                                oentity = New bc_om_entity(ventity(0, i), ventity(1, i), ventity(2, i), ventity(3, i))
                                oentity.certificate = MyBase.certificate
                                disclosure_entities.Add(oentity)
                            Next
                        End If
                        Me.disclosure_tab_name = "Disclosures"

                        vres = db_entity.read_disclosure_tab_name()
                        If IsArray(vres) Then
                            If UBound(vres, 2) > 0 Then
                                If vres(0, 0) <> "Error" Then
                                    Me.disclosure_tab_name = vres(0, 0)
                                End If
                            End If
                        End If


                    End If
                End If

                REM alternate sp driven entity list
                ventity = db_entity.alternate_read_all_entities(MyBase.certificate)
                If IsArray(ventity) Then
                    Me.alternate_entity_list = New bc_om_entities
                    For i = 0 To UBound(ventity, 2)
                        oentity = New bc_om_entity(ventity(0, i), ventity(1, i), ventity(2, i), ventity(3, i))
                        oentity.certificate = MyBase.certificate
                        Me.alternate_entity_list.entity.Add(oentity)
                    Next
                End If

                'search_attributes.tmode = bc_cs_soap_base_class.
                REM JUNE 2017 now done on demand to keep dat file size down.
                'If do_search_attributes = True Then

                '    otrace = New bc_cs_activity_log("bc_om_entities", "db_read", bc_cs_activity_codes.COMMENTARY, "aaaastarting search attributes read", certificate)
                '    search_attributes.db_read()
                '    otrace = New bc_cs_activity_log("bc_om_entities", "db_read", bc_cs_activity_codes.COMMENTARY, "finished search attributes read", certificate)

                'End If
            End If

            Dim res As Object
            res = db_entity.read_filter_attributes(certificate)
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
                   
                    filter_attributes_types.Add(ofa)
                Next
            End If

            If from_sync = False Then



                Dim ofs As bc_om_entity
                res = db_entity.read_entity_filter_attributes(0, 0, certificate)
                filter_attributes.Clear()

                If IsArray(res) Then
                    For i = 0 To UBound(res, 2)
                        ofs = New bc_om_entity
                        ofs.id = res(0, i)
                        ofs.class_id = res(1, i)
                        ofs.name = res(2, i)
                        ofs.class_name = res(3, i)
                        ofs.inactive = res(4, i)
                        ofs.attribute_id = res(5, i)

                        filter_attributes.Add(ofs)
                    Next
                End If


            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_entities", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            otrace = New bc_cs_activity_log("bc_om_entities", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub
    REM gets entity class for entity
    Public Function get_entity_class_for_entity(ByVal entity_id As Long) As Long
        Dim otrace As New bc_cs_activity_log("bc_om_entities", "get_entity_class_for_entity", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try
            Dim i As Integer
            get_entity_class_for_entity = 0
            For i = 0 To entity.Count - 1
                If entity(i).id = entity_id Then
                    get_entity_class_for_entity = entity(i).class_id
                    Exit Function
                End If
            Next
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_entities.webservices", "get_entity_class_for_entity", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_entities", "get_entity_class_for_entity", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Function
    'Public Overloads Overrides Function call_web_service() As String
    '    Dim otrace As New bc_cs_activity_log("bc_om_entities", "call_web_service", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
    '   If IsNumeric(bc_cs_central_settings.timeout) Then
    '      webservice.Timeout = CLng(bc_cs_central_settings.timeout)
    '     Dim ocommentary As New bc_cs_activity_log("bc_om_web services", "call_web_service", bc_cs_activity_codes.COMMENTARY, "Setting Web Service Timeout to: " + bc_cs_central_settings.timeout)
    'End If

    'call_web_service = webservice.LoadEntities
    'otrace = New bc_cs_activity_log("bc_om_doc_entities", "call_web_service", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
    'End Function
    'Public Overrides Function write_xml_via_soap_client_request() As Object

    'End Function

    Public Sub New()

    End Sub
End Class
REM ===============================================


<Serializable()> Public Class bc_om_linked_entity
    Inherits bc_om_entity
    Public child_class_id As Long
    Public parent_class_id As Long
    Public entity_link_id As Long
    Public rating As Integer = 0
    Public linked_attribute_values As New ArrayList
    Public Overrides Sub db_read()
        Dim i As Integer

        REM read all attribues of link
        REM firstly get attrbutes relevant to link
        Dim vres As Object
        Dim oatt As bc_om_attribute_value

        Dim gdb As New bc_om_entity_db
        vres = gdb.read_attributes_info_class_link(parent_class_id, child_class_id, schema_id, MyBase.certificate)
        REM if isarray(vres) then
        If IsArray(vres) Then
            For i = 0 To UBound(vres, 2)
                REM submission code
                oatt = New bc_om_attribute_value
                oatt.attribute_Id = vres(0, i)
                oatt.submission_code = vres(1, i)
                oatt.nullable = vres(2, i)
                oatt.show_workflow = vres(3, i)
                REM now offset submission to linked equivalent
                oatt.submission_code = oatt.submission_code + 4
                oatt.entity_link_id = entity_link_id
                oatt.certificate = MyBase.certificate
                oatt.db_read()
                linked_attribute_values.Add(oatt)
            Next
        End If
    End Sub

    Public Sub New()

    End Sub
End Class
<Serializable()> Public Class bc_om_entity
    Inherits bc_cs_soap_base_class
    Implements IComparable
    Public id As Long
    Public class_id As Long
    Public name As String
    Public class_name As String
    Public show_mode As Integer
    Public show_text As String
    Public write_mode As Integer
    Public schema_id As Long
    Public parent_entities As New ArrayList
    Public child_entities As New ArrayList
    Public attribute_values As New ArrayList
    Public delete_error As String
    Public deleted As Boolean
    Public user_prefs As New ArrayList
    Public boolLoadAggregationPreview As Boolean = False
    Public brunnow As Boolean = False
    Public oPreview As Object
    REM ===================
    Public inactive As Boolean = False
    Public comment As String
    Public attribute_id As Long
    Public Const INSERT = 0
    Public Const UPDATE = 1
    Public Const ATTRIBUTES = 2
    Public Const DELETE = 3
    Public Const SET_ACTIVE = 4
    Public Const SET_INACTIVE = 5
    Public Const ONLY_ATTRIBUTES = 6
    Public Const INSERT_AND_SET_DEFAULT_ATTRIBUTES = 7
    Public attributes_only As Boolean = False
    Public deactivate_fail_text As String = ""



    Public Overrides Sub process_object()
        Try
            Select Case tmode
                Case tREAD
                    db_read()
                Case tWRITE
                    db_write()
            End Select
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_entity", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try
    End Sub
    Public Sub New()

    End Sub
    Public Sub New(ByVal lid As Long, ByVal lclass_id As Long, ByVal strname As String, ByVal class_name As String)
        Me.id = lid
        Me.class_id = lclass_id
        Me.name = strname
        Me.class_name = class_name
        Me.show_mode = 0
        Me.show_text = ""
    End Sub





    Public Overridable Sub db_read()
        Try
            Dim gdb As New bc_om_entity_db

            If boolLoadAggregationPreview = True Then
                oPreview = gdb.get_aggregation_preview(id, MyBase.certificate)
                REM do this asynchronously later on
                REM gdb.get_aggregation_preview_async(Me, id, MyBase.certificate)
                Exit Sub
            End If

            If brunnow = True Then
                gdb.run_now(id, MyBase.certificate)
                Exit Sub
            End If

            Dim vres As Object
            Dim i As Integer
            If attributes_only = False Then


                Dim oent As bc_om_linked_entity
                parent_entities.Clear()
                REM parent entities

                vres = gdb.get_parent_entities_for_entity(Me.id, Me.schema_id, MyBase.certificate)
                If IsArray(vres) Then
                    For i = 0 To UBound(vres, 2)
                        oent = New bc_om_linked_entity
                        oent.id = vres(0, i)
                        oent.class_id = vres(1, i)
                        oent.entity_link_id = vres(2, i)
                        oent.child_class_id = Me.class_id
                        oent.parent_class_id = oent.class_id
                        oent.schema_id = Me.schema_id
                        oent.certificate = MyBase.certificate
                        oent.name = vres(3, i)
                        oent.class_name = vres(4, i)
                        oent.inactive = vres(5, i)
                        oent.rating = vres(6, i)
                        REM at moment dont update inked attrbutes so take this out
                        oent.db_read()
                        parent_entities.Add(oent)
                    Next
                End If
                REM child entities
                child_entities.Clear()
                vres = gdb.get_child_entities_for_entity(Me.id, Me.schema_id, MyBase.certificate)
                If IsArray(vres) Then

                    For i = 0 To UBound(vres, 2)
                        oent = New bc_om_linked_entity
                        oent.id = vres(0, i)
                        oent.class_id = vres(1, i)
                        oent.entity_link_id = vres(2, i)
                        oent.child_class_id = oent.class_id
                        oent.parent_class_id = Me.class_id
                        oent.schema_id = Me.schema_id
                        oent.name = vres(3, i)
                        oent.class_name = vres(4, i)
                        oent.inactive = vres(5, i)
                        oent.rating = vres(6, i)
                        oent.certificate = MyBase.certificate

                        oent.db_read()
                        child_entities.Add(oent)
                    Next
                End If
            End If

            For i = 0 To attribute_values.Count - 1
                attribute_values(i).entity_id = Me.id
                attribute_values(i).certificate = MyBase.certificate
                attribute_values(i).value_changed = False
                attribute_values(i).db_read()
            Next
            REM preferances
            If attributes_only = False Then


                Me.user_prefs.Clear()
                REM FIL July 2012 changes to pref list
                Dim user_pref As bc_om_user_pref = Nothing

                vres = gdb.get_user_prefs(Me.id, MyBase.certificate)
                If IsArray(vres) Then
                    For i = 0 To UBound(vres, 2)
                        user_pref = New bc_om_user_pref
                        user_pref.entity_id = Me.id
                        user_pref.user_id = vres(0, i)
                        user_pref.pref_type = vres(1, i)
                        user_pref.pref_name = vres(2, i)
                        Me.user_prefs.Add(user_pref)
                    Next
                End If
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_entity", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try


    End Sub
    Public Overridable Sub db_write()
        Try
            Dim gdb As New bc_om_entity_db
            Dim i As Integer
            Dim vres As Object
            Dim tname As String
            tname = Me.name
            Dim str As New bc_cs_string_services(Me.name)
            name = str.delimit_apostrophies
            Select Case write_mode
                Case INSERT
                    REM new entity
                    vres = gdb.add_entity(Me.name, Me.class_id, MyBase.certificate)
                    If IsArray(vres) Then
                        Me.id = vres(0, 0)
                    End If
                    gdb.audit_entity_action(Me.id, "Created " + Me.name, MyBase.certificate)
                Case INSERT_AND_SET_DEFAULT_ATTRIBUTES
                    REM new entity
                    vres = gdb.add_entity(Me.name, Me.class_id, MyBase.certificate)
                    If IsArray(vres) Then
                        Me.id = vres(0, 0)
                    End If
                    gdb.audit_entity_action(Me.id, "Created " + Me.name, MyBase.certificate)
                    gdb.set_default_attribute_values(Me.id, MyBase.certificate)
                Case UPDATE
                    REM change name
                    gdb.change_entity_name(Me.id, Me.name, MyBase.certificate)
                    gdb.audit_entity_action(Me.id, "Name changed to " + name, MyBase.certificate)
                Case ONLY_ATTRIBUTES
                    REM attributes
                    For i = 0 To Me.attribute_values.Count - 1
                        Me.attribute_values(i).certificate = MyBase.certificate
                        Me.attribute_values(i).db_write()
                    Next
                Case ATTRIBUTES

                    REM attributes
                    For i = 0 To Me.attribute_values.Count - 1
                        Me.attribute_values(i).certificate = MyBase.certificate
                        Me.attribute_values(i).db_write()
                    Next

                    REM links
                    REM linked attributes and ratings
                    Dim rating As Integer
                    rating = 1
                    Dim prev_class As Long = 0

                    For i = 0 To Me.parent_entities.Count - 1
                        If prev_class <> Me.parent_entities(i).class_id Then
                            rating = 1
                        End If
                        gdb.update_rating(True, Me.id, Me.parent_entities(i).Id, rating, Me.parent_entities(i).schema_id, MyBase.certificate)
                        rating = rating + 1
                        prev_class = Me.parent_entities(i).class_id
                        For j = 0 To Me.parent_entities(i).linked_attribute_values.count - 1
                            Me.parent_entities(i).linked_attribute_values(j).certificate = MyBase.certificate
                            Me.parent_entities(i).linked_attribute_values(j).db_write()
                        Next
                    Next
                    rating = 1
                    prev_class = 0
                    For i = 0 To Me.child_entities.Count - 1
                        If prev_class <> Me.child_entities(i).class_id Then
                            rating = 1
                        End If
                        gdb.update_rating(False, Me.id, Me.child_entities(i).Id, rating, Me.child_entities(i).schema_id, MyBase.certificate)
                        rating = rating + 1
                        prev_class = Me.child_entities(i).class_id
                        For j = 0 To Me.child_entities(i).linked_attribute_values.count - 1
                            Me.child_entities(i).linked_attribute_values(j).certificate = MyBase.certificate
                            Me.child_entities(i).linked_attribute_values(j).db_write()
                        Next
                    Next
                    REM user prefs
                    Dim audit_id As Long
                    Dim ares As Object
                    ares = gdb.get_audit_id(certificate)
                    If IsArray(ares) Then
                        If UBound(ares, 2) = 0 Then
                            audit_id = ares(0, 0)
                        End If
                    End If


                    gdb.delete_user_prefs(audit_id, Me.id, MyBase.certificate)
                    For i = 0 To Me.user_prefs.Count - 1
                        gdb.add_user_pref(audit_id, Me.id, Me.user_prefs(i), i + 1, MyBase.certificate)
                    Next


                    gdb.audit_entity_links(Me.id, Me.schema_id, MyBase.certificate)

                Case DELETE
                    delete_error = gdb.delete_entity(Me.id, MyBase.certificate)
                    gdb.audit_entity_action(Me.id, "Set Deleted", MyBase.certificate)
                    deleted = True
                Case SET_ACTIVE
                    gdb.change_entity_state(Me.id, True, MyBase.certificate)
                    gdb.audit_entity_action(Me.id, "Set Active", MyBase.certificate)
                Case SET_INACTIVE
                    deactivate_fail_text = gdb.change_entity_state(Me.id, False, MyBase.certificate)
                    If deactivate_fail_text = "" Then
                        gdb.audit_entity_action(Me.id, "Set Inactive", MyBase.certificate)
                    End If
            End Select
            Me.name = tname
            'Dim ugdb As New bc_om_user_db
            'ugdb.set_sync(MyBase.certificate)
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_entity", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try
    End Sub
    Public Overrides Function toString() As String
        Return name
    End Function
    Function CompareTo(ByVal obj As Object) As Integer Implements IComparable.CompareTo
        Return String.Compare(name, obj.ToString)
    End Function
    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class
<Serializable()> Public Class bc_om_attribute_value
    Inherits bc_cs_soap_base_class
    Public entity_id As Long
    Public attribute_Id As Long
    Public show_workflow As Boolean = False
    Public submission_code As Integer
    Public value As String
    Public published_value As String
    Public original_published_value As String
    Public nullable As Boolean = False
    Public publish_draft_value As Boolean = False
    Public value_changed As Boolean = False
    Public entity_link_id As Long
    Public last_update_comment As String
    Public last_update_user As String
    Public last_update_published_comment As String
    Public last_update_published_user As String
    Public name As String
    Public add_new As String
    Public field_name As String
    Public user_id As Long = 0
    Public steps As List(Of bc_om_step_attribute)


    <NonSerialized()> Dim gdb As New bc_om_entity_db

    Public Overrides Sub process_object()
        Try
            Select Case tmode
                Case tREAD
                    db_read()
                Case tWRITE
                    db_write()
            End Select
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_attribute_value", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try

    End Sub

    REM FIL JUNE 2013
    Public Sub db_read()
        Dim gdb As New bc_om_entity_db
        Dim vres As Object
        If user_id <> 0 Then
            value = ""
            If Me.attribute_Id = 0 Then
                REM user table
                vres = gdb.read_user_table_value_for_user(user_id, field_name, certificate)
            Else
                REM configured user attribute
                vres = gdb.read_attribute_value_for_user(user_id, attribute_Id, certificate)
            End If
            If IsArray(vres) Then
                If UBound(vres, 2) > -1 Then
                    value = vres(0, 0)
                    last_update_comment = vres(1, 0)
                    last_update_user = vres(2, 0)
                End If
            End If
        Else
            vres = gdb.read_attribute_value(entity_id, attribute_Id, submission_code, entity_link_id, certificate)
            value = ""
            published_value = ""
            original_published_value = ""
            last_update_comment = ""
            last_update_user = ""
            last_update_published_comment = ""
            last_update_published_user = ""

            'step atribute
            If submission_code = 10 Then
                Dim sa As bc_om_step_attribute
                steps = New List(Of bc_om_step_attribute)
                If IsArray(vres) Then
                    For i = 0 To UBound(vres, 2)

                        sa = New bc_om_step_attribute
                        sa.value = vres(0, i)
                        sa.date_from = vres(1, i)
                        sa.date_to = vres(2, i)
                        If Now.ToUniversalTime >= sa.date_from And Now.ToUniversalTime < sa.date_to Then
                            value = vres(0, i)
                        End If
                        steps.Add(sa)
                    Next
                End If

            Else
                If IsArray(vres) Then
                    If UBound(vres, 2) > -1 Then
                        value = vres(0, 0)
                        last_update_comment = vres(1, 0)
                        last_update_user = vres(2, 0)
                        If show_workflow = True And UBound(vres, 2) > 0 Then
                            published_value = vres(0, 1)
                            original_published_value = vres(0, 1)
                            last_update_published_comment = vres(1, 1)
                            last_update_published_user = vres(2, 1)
                        End If
                        REM step


                    End If
                End If
            End If
        End If
    End Sub
    Public Sub db_write()
        If Me.value_changed = True Then
            Dim gdb As New bc_om_entity_db

            If submission_code = 10 Then
                Dim db As New bc_cs_db_services
                Dim param As bc_cs_db_services.bc_cs_sql_parameter
                Dim params As New List(Of bc_cs_db_services.bc_cs_sql_parameter)
                Dim user_id As Long
                If bc_cs_central_settings.server_flag = 0 Then
                    user_id = bc_cs_central_settings.logged_on_user_id
                Else
                    user_id = certificate.user_id

                End If
                db.executesql("exec dbo.bc_core_cp_delete_step_data " + CStr(entity_id) + "," + CStr(attribute_Id), certificate)

                For i = 0 To steps.Count - 1
                    params = New List(Of bc_cs_db_services.bc_cs_sql_parameter)

                    param = New bc_cs_db_services.bc_cs_sql_parameter
                    param.name = "entity_id"
                    param.value = entity_id
                    params.Add(param)
                    param = New bc_cs_db_services.bc_cs_sql_parameter
                    param.name = "attribute_id"
                    param.value = attribute_Id
                    params.Add(param)
                    param = New bc_cs_db_services.bc_cs_sql_parameter
                    param.name = "value"
                    param.value = steps(i).value
                    params.Add(param)
                    param = New bc_cs_db_services.bc_cs_sql_parameter
                    param.name = "date_from"
                    param.value = steps(i).date_from
                    params.Add(param)
                    param = New bc_cs_db_services.bc_cs_sql_parameter
                    param.name = "date_to"
                    param.value = steps(i).date_to
                    params.Add(param)
                    param = New bc_cs_db_services.bc_cs_sql_parameter
                    param.name = "user_id"
                    param.value = user_id
                    params.Add(param)

                    db.executesql_with_parameters("bc_core_cp_insert_step_data", params, certificate)
                Next
                Me.value_changed = False
            End If

            Dim tname As String
            tname = Me.value
            Dim str As New bc_cs_string_services(Me.value)
            Me.value = str.delimit_apostrophies()

            If Me.user_id <> 0 Then
                If Me.attribute_Id = 0 Then
                    Me.last_update_comment = gdb.write_user_table_value(Me.user_id, Me.field_name, value, MyBase.certificate)
                Else
                    Me.last_update_comment = gdb.write_user_attribute_value(Me.user_id, Me.attribute_Id, value, Me.submission_code, MyBase.certificate)
                End If
                Me.last_update_user = "me"
            Else

                If Me.show_workflow = False Then
                    Me.last_update_comment = gdb.write_attribute_value(Me.entity_id, Me.submission_code, Me.attribute_Id, True, value, Me.entity_link_id, MyBase.certificate)
                    Me.last_update_user = "me"
                Else
                    Me.last_update_comment = gdb.write_attribute_value(Me.entity_id, Me.submission_code, Me.attribute_Id, False, value, Me.entity_link_id, MyBase.certificate)
                    Me.last_update_user = "me"
                    If Me.publish_draft_value = True And Me.show_workflow = True Then
                        Me.last_update_published_comment = gdb.write_attribute_value(Me.entity_id, Me.submission_code, Me.attribute_Id, True, value, Me.entity_link_id, MyBase.certificate)
                        Me.publish_draft_value = False
                        Me.last_update_published_user = "me"
                    End If
                End If
            End If
            Me.value = tname
        End If
        Me.value_changed = False

    End Sub

    Public Sub New()

    End Sub
End Class
REM ===============================================
REM Bluecurve Limited 2005
REM Module:       Create Entity Links Object Model
REM Type:         Object Model
REM Description:  Users
REM               User
REM Version:      1
REM Change history
REM ===============================================
<Serializable()> Public Class bc_om_entity_links
    Inherits bc_cs_soap_base_class
    Public entity_link As New ArrayList

    Public Overrides Sub process_object()
        Dim otrace As New bc_cs_activity_log("bc_om_entity_links", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            REM this is always specific to object
            If Me.tmode = bc_cs_soap_base_class.tREAD Then
                db_read()
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_entity_links", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_entity_links", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub
    Public Sub db_read()
        Dim otrace As New bc_cs_activity_log("bc_om_entities", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Dim i As Integer
        Dim olink As bc_om_entity_link
        Dim db_link As New bc_om_entity_link_db
        Dim vlinks
        vlinks = db_link.read_all_entity_links(MyBase.certificate)
        If IsArray(vlinks) Then
            For i = 0 To UBound(vlinks, 2)
                olink = New bc_om_entity_link(vlinks(0, i), vlinks(1, i), vlinks(2, i))

                olink.certificate = MyBase.certificate
                entity_link.Add(olink)
            Next
        End If
        otrace = New bc_cs_activity_log("bc_om_entities", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
    End Sub
    REM entity link manipulation functions
    Public Function get_entity_id_for_class_hierarchy(ByVal entity_id As Long, ByVal class_id As Long, ByVal oentities As bc_om_entities, Optional schema_id As Integer = 0, Optional first As Boolean = False)
        Dim otrace As New bc_cs_activity_log("bc_om_entity_links", "get_entity_id_for_class_hierarchy", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Dim i As Integer
        Dim ret_entity_id As Long
        Try
            If bc_cs_central_settings.selected_conn_method <> bc_cs_central_settings.LOCAL Then
                REM 5.8 do this on db as faster

                ret_entity_id = 0
                If schema_id = 0 Then
                    schema_id = 1
                End If
                Dim sql As String
                Dim cclass_id As String
                cclass_id = oentities.get_entity_class_for_entity(entity_id)
                If first = True Then
                    sql = "bcc_core_v5_excel_function '<excel_function><type>5005</type><class_id>" + CStr(cclass_id) + "</class_id><entity_id>" + CStr(entity_id) + "</entity_id><ass_class_id>" + CStr(class_id) + "</ass_class_id><schema_id>" + CStr(schema_id) + "</schema_id><priority>all</priority><dimensions>entity_id</dimensions><attributename></attributename></excel_function>'," + CStr(bc_cs_central_settings.logged_on_user_id)
                    Dim osql As New bc_cs_sql
                    osql.sql = sql
                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        osql.db_read()
                        Try
                            ret_entity_id = osql.results(0, 0)

                        Catch

                        End Try
                    Else
                        osql.tmode = bc_cs_soap_base_class.tREAD
                        If osql.transmit_to_server_and_receive(osql, True) = True Then
                            Try
                                ret_entity_id = osql.results(0, 0)
                            Catch
                            End Try
                        End If
                    End If
                End If
                If ret_entity_id > 0 Then
                    get_entity_id_for_class_hierarchy = ret_entity_id
                    Dim ocomm As New bc_cs_activity_log("bc_om_entity_links", "get_entity_id_for_class_hierarcy", bc_cs_activity_codes.COMMENTARY, "Parent matched from database: " + CStr(ret_entity_id))
                    Exit Function
                End If
            End If
            get_entity_id_for_class_hierarchy = 0

            REM get parent for entity until match is found for class
            For i = 0 To entity_link.Count - 1
                With entity_link(i)
                    If schema_id = 0 Then
                        If .child_entity_id = entity_id Then
                            REM check if parent matches class
                            If oentities.get_entity_class_for_entity(.parent_entity_id) = class_id Then
                                get_entity_id_for_class_hierarchy = .parent_entity_id
                                Exit Function
                            Else

                                REM propogate parent
                                ret_entity_id = get_entity_id_for_class_hierarchy(.parent_entity_id, class_id, oentities, .schema_id)
                                If ret_entity_id > 0 Then
                                    get_entity_id_for_class_hierarchy = ret_entity_id
                                    Exit Function
                                End If
                            End If
                        End If
                    Else
                        If .child_entity_id = entity_id And .schema_id = schema_id Then
                            REM check if parent matches class
                            If oentities.get_entity_class_for_entity(.parent_entity_id) = class_id Then
                                get_entity_id_for_class_hierarchy = .parent_entity_id
                                Exit Function
                            Else
                                REM propogate parent
                                ret_entity_id = get_entity_id_for_class_hierarchy(.parent_entity_id, class_id, oentities, .schema_id)
                                If ret_entity_id > 0 Then
                                    get_entity_id_for_class_hierarchy = ret_entity_id
                                    Exit Function
                                End If
                            End If
                        End If

                    End If
                End With
            Next

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_entity_links.webservices", "get_entity_id_for_class_hierarchy", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            get_entity_id_for_class_hierarchy = Nothing
        Finally
            otrace = New bc_cs_activity_log("bc_om_entity_links", "get_entity_id_for_class_hierarchy", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Function
    'Public Overloads Overrides Function call_web_service() As String
    '    Dim otrace As New bc_cs_activity_log("bc_om_entity_links", "call_web_service", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
    '    If IsNumeric(bc_cs_central_settings.timeout) Then
    '        webservice.Timeout = CLng(bc_cs_central_settings.timeout)
    '        Dim ocommentary As New bc_cs_activity_log("bc_om_web services", "call_web_service", bc_cs_activity_codes.COMMENTARY, "Setting Web Service Timeout to: " + bc_cs_central_settings.timeout)
    '    End If

    '   call_web_service = webservice.LoadEntityLinks
    '   otrace = New bc_cs_activity_log("bc_om_entity_links", "call_web_service", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
    'End Function

    Public Sub New()

    End Sub
End Class
<Serializable()> Public Class bc_om_entity_link
    Inherits bc_cs_soap_base_class
    Public child_entity_id As Long
    Public parent_entity_id As Long
    Public schema_id As Long
    Public parent_class_id As Long
    Public child_class_id As Long
    Public link_id As Long
    Public attributes As New ArrayList
    Public write_mode As Long
    Public rating As Integer = 1
    Public child_parent_rating As Integer
    Public parent_child_rating As Integer
    Public Const INSERT = 0
    Public Const DELETE = 1

    Public Sub New()

    End Sub
    Public Overrides Sub process_object()
        Select Case tmode
            Case tWRITE
                db_write()
        End Select
    End Sub
    Public Sub New(ByVal lchild_entity_id As Long, ByVal lparent_entity_id As Long, ByVal lschema_id As Long)
        child_entity_id = lchild_entity_id
        parent_entity_id = lparent_entity_id
        schema_id = lschema_id

    End Sub
    Public Sub db_write()
        Try
            Dim gdb As New bc_om_entity_db
            Dim vres As Object
            Dim oatt As bc_om_attribute_value
            If Me.write_mode = INSERT Then
                vres = gdb.add_entity_link(Me.parent_entity_id, Me.child_entity_id, Me.schema_id, Me.child_parent_rating, Me.parent_child_rating, MyBase.certificate)
                REM now get attributes for relationship
                If IsArray(vres) Then
                    If UBound(vres, 2) > -1 Then
                        Me.link_id = vres(0, 0)
                        REM now get attrbutes
                        vres = gdb.read_attributes_info_class_link(Me.parent_class_id, Me.child_class_id, Me.schema_id, MyBase.certificate)
                        For i = 0 To UBound(vres, 2)
                            oatt = New bc_om_attribute_value
                            oatt.attribute_Id = vres(0, i)
                            oatt.submission_code = vres(1, i)
                            oatt.nullable = vres(2, i)
                            oatt.show_workflow = vres(3, i)
                            oatt.submission_code = oatt.submission_code + 4
                            oatt.entity_link_id = Me.link_id
                            Me.attributes.Add(oatt)
                        Next
                    End If
                End If
            Else
                vres = gdb.delete_entity_link(Me.parent_entity_id, Me.child_entity_id, Me.schema_id, MyBase.certificate)
                If IsArray(vres) Then
                    If UBound(vres, 2) > -1 Then
                        Me.child_parent_rating = vres(0, 0)
                        Me.parent_child_rating = vres(1, 0)
                    End If
                End If
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_entity_link", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message, MyBase.certificate)
        End Try
    End Sub
End Class
<Serializable()> Public Class bc_om_schema
    Inherits bc_cs_soap_base_class
    Public schema_id As Long
    Public schema_name As String
    Public write_mode As Integer = 0
    Public delete_error As String
    Public inactive As Boolean = False
    REM FIL FEB 2013
    Public core As Integer = 0

    Public Const INSERT = 0
    Public Const UPDATE = 1
    Public Const DELETE = 2
    Public Const SET_ACTIVE = 3
    Public Const SET_INACTIVE = 4
    Public Overrides Sub process_object()
        Try
            Select Case Me.tmode
                Case bc_cs_soap_base_class.tWRITE
                    db_write()
            End Select
        Catch ex As Exception
            Dim oerr As New bc_cs_activity_log("bc_om_schema", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Public Sub db_write()
        Try
            Dim gdb As New bc_om_entity_db
            Dim tname As String
            tname = Me.schema_name
            Dim str As New bc_cs_string_services(Me.schema_name)
            Dim vres As Object
            Me.schema_name = str.delimit_apostrophies
            Select Case Me.write_mode
                Case 0
                    vres = gdb.add_schema(Me.schema_name, MyBase.certificate)
                    If IsArray(vres) Then
                        Me.schema_id = vres(0, 0)
                    End If
                Case 1
                    gdb.update_schema_name(Me.schema_id, Me.schema_name, MyBase.certificate)
                Case 2
                    delete_error = gdb.delete_schema(Me.schema_id, MyBase.certificate)
                Case SET_ACTIVE
                    gdb.set_schema_inactive(Me.schema_id, False, MyBase.certificate)
                Case SET_INACTIVE
                    gdb.set_schema_inactive(Me.schema_id, True, MyBase.certificate)

            End Select
            Me.schema_name = tname

        Catch ex As Exception
            Dim oerr As New bc_cs_activity_log("bc_om_schema", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
End Class
<Serializable()> Public Class bc_om_entity_classes
    Inherits bc_cs_soap_base_class
    Public classes As New ArrayList
    Public attribute_pool As New ArrayList
    Public schemas As New ArrayList
    Public get_inactive As Boolean = False
    Public no_attributes As Boolean = False
    Public class_only As Boolean = False

    Public Overrides Sub process_object()
        Select Case tmode
            Case tREAD
                db_read()
        End Select
    End Sub
    Public Sub db_read()
        REM read classes
        Dim vres As Object
        Dim oclass As bc_om_entity_class
        Dim oatt As bc_om_attribute
        Dim osch As bc_om_schema
        Dim i As Integer
        Dim gdb As New bc_om_entity_db
        vres = gdb.read_all_classes(MyBase.certificate, get_inactive)
        classes.Clear()
        If IsArray(vres) Then
            For i = 0 To UBound(vres, 2)
                oclass = New bc_om_entity_class
                oclass.class_id = vres(0, i)
                oclass.class_name = vres(1, i)
                oclass.inactive = vres(2, i)
                oclass.certificate = MyBase.certificate
                oclass.class_type_Id = vres(3, i)
                oclass.list_order = vres(4, i)
                oclass.entry_description = vres(5, i)

                If class_only = False Then
                    If oclass.class_type_Id = 1 Then
                        oclass.db_read(no_attributes)
                    Else
                        oclass.db_read(False)
                    End If
                End If
                classes.Add(oclass)
            Next
        End If
        If class_only = True Then
            Exit Sub
        End If
        REM read attributes
        Me.attribute_pool.Clear()
        vres = gdb.read_attributes(MyBase.certificate)
        If IsArray(vres) Then
            For i = 0 To UBound(vres, 2)
                oatt = New bc_om_attribute
                oatt.name = vres(0, i)
                oatt.attribute_id = vres(1, i)
                oatt.type_id = vres(2, i)

                oatt.is_lookup = vres(3, i)
                oatt.min_val = vres(4, i)
                oatt.max_val = vres(5, i)
                oatt.length = vres(6, i)
                oatt.nullable = vres(7, i)
                oatt.lookup_sql = vres(8, i)
                oatt.repeats = vres(10, i)
                oatt.show_workflow = vres(11, i)
                oatt.audited = vres(12, i)
                oatt.default_value = vres(13, i)
                oatt.submission_code = vres(14, i)
                If oatt.is_lookup = True Then
                    oatt.certificate = MyBase.certificate


                    If oatt.lookup_sql <> "" Then
                        oatt.db_read()
                    End If
                End If
                oatt.is_def = vres(15, i)
                oatt.def_sql = vres(16, i)

                Me.attribute_pool.Add(oatt)
            Next
        End If
        REM schemas
        Me.schemas.Clear()
        vres = gdb.read_schemas(MyBase.certificate)
        If IsArray(vres) Then
            For i = 0 To UBound(vres, 2)
                osch = New bc_om_schema
                osch.schema_id = vres(0, i)
                osch.schema_name = vres(1, i)
                osch.inactive = vres(2, i)
                osch.core = vres(3, i)
                Me.schemas.Add(osch)
            Next
        End If
    End Sub
End Class
<Serializable()> Public Class bc_om_entity_class
    Inherits bc_cs_soap_base_class
    Public class_id As Long
    Public class_name As String
    Public child_links As New ArrayList
    Public parent_links As New ArrayList
    Public attributes As New ArrayList
    Public write_mode As Integer
    Public delete_error As String
    Public inactive As Boolean = False

    REM FIL Feb 2013
    Public class_type_Id As Long
    Public list_order As Integer
    Public entry_description As String

    Public Const INSERT = 0
    Public Const UPDATE = 1
    Public Const DELETE = 2
    Public Const SET_ACTIVE = 3
    Public Const SET_INACTIVE = 4
    Public Overrides Sub process_object()
        Try
            Select Case tmode
                Case tREAD
                    db_read()
                Case tWRITE
                    db_write()
            End Select
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_entity_class", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Public Sub db_write()
        Try
            Dim gdb As New bc_om_entity_db
            Dim str As New bc_cs_string_services(Me.class_name)
            Dim vres As Object
            Dim tname As String
            tname = Me.class_name

            Me.class_name = str.delimit_apostrophies
            str = New bc_cs_string_services(entry_description)
            entry_description = str.delimit_apostrophies
            Select Case Me.write_mode

                Case INSERT
                    vres = gdb.add_entity_class(class_name, entry_description, MyBase.certificate)
                    If IsArray(vres) Then
                        Me.class_id = vres(0, 0)
                    End If
                Case UPDATE
                    gdb.change_entity_class(class_id, class_name, entry_description, MyBase.certificate)
                Case DELETE
                    delete_error = gdb.delete_entity_class(class_id, MyBase.certificate)
                Case SET_ACTIVE
                    gdb.set_class_inactive(class_id, False, MyBase.certificate)
                Case SET_INACTIVE
                    gdb.set_class_inactive(class_id, True, MyBase.certificate)
            End Select
            Me.class_name = tname
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_entity_class", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message)


        End Try

    End Sub
    Public Sub db_read(Optional ByVal no_attributes As Boolean = False)
        Try
            Dim vres As Object
            Dim olink As bc_om_class_link_info
            Dim catt As bc_om_class_attribute
            Dim i As Integer
            Dim gdb As New bc_om_entity_db
            REM parent links
            parent_links.Clear()
            child_links.Clear()
            vres = gdb.read_parent_class_links_for_class(Me.class_id, MyBase.certificate)
            If IsArray(vres) Then
                For i = 0 To UBound(vres, 2)
                    olink = New bc_om_class_link_info
                    olink.certificate = MyBase.certificate
                    olink.schema_id = vres(1, i)
                    olink.parent_class_id = vres(0, i)
                    olink.child_class_id = Me.class_id
                    olink.mandatory_cp = vres(2, i)
                    If no_attributes = False Then
                        olink.db_read()
                    End If
                    parent_links.Add(olink)
                Next
            End If
            REM child links
            vres = gdb.read_child_class_links_for_class(Me.class_id, MyBase.certificate)
            If IsArray(vres) Then
                For i = 0 To UBound(vres, 2)
                    olink = New bc_om_class_link_info
                    olink.certificate = MyBase.certificate
                    olink.schema_id = vres(1, i)
                    olink.child_class_id = vres(0, i)
                    olink.parent_class_id = Me.class_id
                    olink.mandatory_pc = vres(2, i)
                    If no_attributes = False Then
                        olink.db_read()
                    End If
                    child_links.Add(olink)
                Next
            End If
            If no_attributes = False Then
                REM class attributes
                vres = gdb.read_attributes_for_class(Me.class_id, MyBase.certificate)
                If IsArray(vres) Then
                    For i = 0 To UBound(vres, 2)
                        catt = New bc_om_class_attribute
                        catt.certificate = MyBase.certificate
                        catt.attribute_id = vres(0, i)
                        catt.order = vres(1, i)
                        catt.permission = vres(2, i)
                        catt.attribute_name = vres(3, i)
                        Me.attributes.Add(catt)
                    Next
                End If
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_entity_class", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Public Overrides Function ToString() As String
        Return class_name
    End Function

    Public Sub New()

    End Sub
End Class
<Serializable()> Public Class bc_om_class_link_info
    Inherits bc_cs_soap_base_class
    Public schema_id As Long
    Public parent_class_id As Long
    Public child_class_id As Long
    Public mandatory_pc As Integer
    Public mandatory_cp As Integer
    Public linked_attributes As New ArrayList
    Public write_mode As Integer
    Public Const INSERT = 0
    Public Const DELETE = 1
    Public Const UPDATE_PC = 2
    Public Const UPDATE_CP = 3
    Public Sub New()

    End Sub
    Public Overrides Sub process_object()
        Try
            Select Case Me.tmode
                Case tWRITE
                    Me.db_write()

            End Select
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_class_link_info", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try
    End Sub
    Public Sub db_read()
        Dim vres As Object
        Dim i As Integer
        Dim catt As New bc_om_class_attribute
        Dim gdb As New bc_om_entity_db
        linked_attributes.Clear()
        vres = gdb.read_attributes_for_class_link(Me.parent_class_id, Me.child_class_id, Me.schema_id, MyBase.certificate)
        If IsArray(vres) Then
            For i = 0 To UBound(vres, 2)
                catt = New bc_om_class_attribute
                catt.attribute_id = vres(0, i)
                catt.order = vres(1, i)
                catt.permission = vres(2, i)
                linked_attributes.Add(catt)
            Next
        End If


    End Sub
    Public Sub db_write()
        Try
            Dim gdb As New bc_om_entity_db

            Select Case Me.write_mode
                Case INSERT
                    gdb.write_class_link(Me.schema_id, Me.parent_class_id, Me.child_class_id, MyBase.certificate)

                Case DELETE
                    gdb.delete_class_link(Me.schema_id, Me.parent_class_id, Me.child_class_id, MyBase.certificate)
                Case UPDATE_PC
                    gdb.update_entity_number(Me.schema_id, Me.parent_class_id, Me.child_class_id, Me.mandatory_pc, True, MyBase.certificate)
                Case UPDATE_CP
                    gdb.update_entity_number(Me.schema_id, Me.parent_class_id, Me.child_class_id, Me.mandatory_cp, False, MyBase.certificate)
            End Select
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_class_link_info", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
End Class
<Serializable()> Public Class bc_om_class_link_attribute
    Inherits bc_om_class_attribute
    Public schema_id As Long
    Public parent_class_id As Long
    Public Overrides Sub process_object()
        Select Case tmode
            Case tWRITE
                db_write()
        End Select
    End Sub
    Public Overrides Sub db_write()

        Dim gdb As New bc_om_entity_db



        gdb.delete_class_link_attribute(class_id, attribute_id, parent_class_id, schema_id, MyBase.certificate)
        If write_mode = INSERT_ATTRIBUTE Then
            gdb.add_class_link_attribute(class_id, attribute_id, order, permission, parent_class_id, schema_id, MyBase.certificate)
        End If
        


    End Sub
End Class

<Serializable()> Public Class bc_om_class_attribute
    Inherits bc_cs_soap_base_class
    Public attribute_id As Long
    Public order As Integer
    Public permission As Integer
    Public attribute_name As String
    Public class_id As Long
    Public write_mode As Integer

    Public Const INSERT_ATTRIBUTE = 0
    Public Const DELETE_ATTRIBUTE = 1
    Public Overrides Sub process_object()
        Try
            Select Case tmode
                Case tWRITE
                    db_write()
            End Select
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_class_attribute", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Public Overridable Sub db_write()
        Try
            Dim gdb As New bc_om_entity_db
            If class_id > 0 Then
                gdb.delete_class_attribute(class_id, attribute_id, MyBase.certificate)
                If write_mode = INSERT_ATTRIBUTE Then
                    gdb.add_class_attribute(class_id, attribute_id, order, permission, MyBase.certificate)
                End If
            Else
                REM user pub type attributes
                gdb.delete_upt_attribute(class_id, attribute_id, MyBase.certificate)
                If write_mode = INSERT_ATTRIBUTE Then
                    gdb.add_upt_attribute(class_id, attribute_id, order, permission, MyBase.certificate)
                End If
            End If


        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_class_attribute", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
End Class
REM ING JUNE 2012
<Serializable()> Public Class bc_om_attribute
    Inherits bc_cs_soap_base_class
    Public name As String
    Public attribute_id As Long
    Public type_id As Long
    Public is_lookup As Boolean
    Public min_val As Long
    Public max_val As Long
    Public length As Integer
    Public nullable As Integer
    Public lookup_sql As String
    Public multiple As Integer
    Public repeats As Integer
    Public show_workflow As Integer
    Public audited As Integer
    Public default_value As String
    Public submission_code As Integer
    Public lookup_values As New ArrayList
    Public lookup_keys As New ArrayList
    Public write_mode As Integer = 0
    Public delete_error As String
    Public is_def As Boolean = False
    Public def_sql As String
    Public read_default_value As Boolean
    Public persmission As Integer
    Public show_filter As Boolean = False
    Public Const INSERT = 0
    Public Const UPDATE = 1
    Public Const DELETE = 2
    Public Const GET_DEFAULT_VALUE = 3
    REM ING JUNE 2012

    Public Sub New()

    End Sub
    Public Overrides Sub process_object()
        Try
            Select Case Me.tmode
                Case tREAD
                    If read_default_value = True Then
                        db_read_default_value()
                    Else
                        db_read()
                    End If
                Case tWRITE
                    db_write()
            End Select
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_attribute", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Public Sub db_read_default_value()
        Try
            Dim gdb As New bc_om_entity_db
            Dim res As Object
            Me.default_value = ""
            res = gdb.get_default_value_for_attribute(Me.attribute_id, MyBase.certificate)
            If IsArray(res) Then
                If UBound(res, 2) = 0 Then
                    Me.default_value = res(0, 0)
                End If
            End If


        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_attribute", "db_read_default_value", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try
    End Sub
    Public Sub db_read()
        Try
            Dim vres As Object
            Dim i As Integer
            Dim gdb As New bc_om_entity_db
            Me.lookup_values.Clear()
            Me.lookup_keys.Clear()
            vres = gdb.get_lookup_values(Me.lookup_sql, MyBase.certificate)
            If IsArray(vres) Then
                For i = 0 To UBound(vres, 2)
                    Me.lookup_values.Add(CStr(vres(0, i)))
                    If UBound(vres, 1) = 2 Then
                        Me.lookup_keys.Add(CStr(vres(1, i)))
                    End If

                Next
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_attribute", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Public Sub db_write()
        Try
            Dim gdb As New bc_om_entity_db
            Dim vres As Object
            Dim tname As String
            Dim tlu As String
            tname = name
            tlu = lookup_sql
            Dim fs As New bc_cs_string_services(name)
            name = fs.delimit_apostrophies
            fs = New bc_cs_string_services(lookup_sql)
            Me.lookup_sql = fs.delimit_apostrophies

            Select Case write_mode
                Case INSERT
                    vres = gdb.add_attribute(name, type_id, is_lookup, nullable, lookup_sql, repeats, show_workflow, audited, submission_code, Me.multiple, Me.repeats, length, is_def, def_sql, MyBase.certificate)
                    Me.attribute_id = 0
                    If IsArray(vres) Then
                        Me.attribute_id = vres(0, 0)
                    End If
                Case UPDATE
                    gdb.modify_attribute(Me.attribute_id, name, type_id, is_lookup, nullable, lookup_sql, repeats, show_workflow, audited, submission_code, Me.multiple, Me.repeats, length, is_def, def_sql, MyBase.certificate)
                Case DELETE
                    delete_error = gdb.delete_attribute(Me.attribute_id, MyBase.certificate)
            End Select
            name = tname
            lookup_sql = tlu
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_attribute", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
End Class

REM =========================================================================
REM Database interaction layer
REM =========================================================================
Public Class bc_om_entity_db
    REM Private gbc_db As New bc_cs_db_services
    Public gbc_db As New bc_cs_db_services
    Public processing As Boolean = False
    Public results As Object
    REM reads all pub type in database 

    REM FIL JUNE 2013
    Public Function read_entity_filter_attributes(class_id As Long, attribute_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        Dim user_id As Long
        If bc_cs_central_settings.server_flag = 1 Then
            user_id = bc_cs_central_settings.logged_on_user_id
        Else
            user_id = certificate.user_id
        End If
        sql = "exec dbo.bc_core_entity_attribute_filter_values " + CStr(class_id) + "," + CStr(attribute_id) + "," + CStr(user_id)
        read_entity_filter_attributes = gbc_db.executesql(sql, certificate)
    End Function
    Public Function read_filter_attributes(ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        Dim user_id As Long
        If bc_cs_central_settings.server_flag = 0 Then
            user_id = bc_cs_central_settings.logged_on_user_id
        Else
            user_id = certificate.user_id
        End If
        sql = "exec dbo.bc_core_entity_attribute_filter_attributes " + CStr(user_id)
        read_filter_attributes = gbc_db.executesql(sql, certificate)
    End Function


    Public Function get_default_value_for_attribute(attribute_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String

        sql = "exec dbo.bc_core_get_def_value_for_attribute " + CStr(attribute_id) + ""
        get_default_value_for_attribute = gbc_db.executesql(sql, certificate)
    End Function
    Public Function read_user_table_value_for_user(ByVal user_id As Long, ByVal field_name As String, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "exec dbo.bc_core_get_user_table_value_for_user " + CStr(user_id) + ",'" + field_name + "'"
        read_user_table_value_for_user = gbc_db.executesql(sql, certificate)
    End Function
    Public Function read_attribute_value_for_user(ByVal user_id As Long, ByVal attribute_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "exec dbo.bc_core_get_attribute_value_for_user " + CStr(user_id) + "," + CStr(attribute_id)
        read_attribute_value_for_user = gbc_db.executesql(sql, certificate)
    End Function


    Public Function read_all_classes(ByRef certificate As bc_cs_security.certificate, ByVal get_inactive As Boolean) As Object
        Dim sql As String
        REM 5.7.1.2 move to SP APR 2017

        If get_inactive = True Then
            'sql = "select class_id, class_name,coalesce(inactive,0),coalesce(class_type_id,1),coalesce(list_order,0) from entity_class_tbl where coalesce(deleted,'0') = 0 order by class_name asc"
            sql = "exec dbo.bc_core_get_entity_classes 1"
        Else
            'sql = "select class_id, class_name,coalesce(inactive,0),coalesce(class_type_id,1),coalesce(list_order,0) from entity_class_tbl where coalesce(deleted,'0') = 0 and  coalesce(inactive,'0') = 0 order by class_name asc"
            sql = "exec dbo.bc_core_get_entity_classes 0"
        End If
        read_all_classes = gbc_db.executesql(sql, certificate)
    End Function
    Public Function read_child_class_links_for_class(ByVal class_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select child_class_id, class_link_tbl.schema_id,coalesce(mandatory_num_pc,0) from class_link_tbl inner join schema_tbl on class_link_tbl.schema_id=schema_tbl.schema_id and coalesce(schema_tbl.deleted,0)=0 where parent_class_id=" + CStr(class_id)
        read_child_class_links_for_class = gbc_db.executesql(sql, certificate)
    End Function
    Public Function read_parent_class_links_for_class(ByVal class_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select parent_class_id, class_link_tbl.schema_id,coalesce(mandatory_num_cp,0)  from class_link_tbl inner join schema_tbl on class_link_tbl.schema_id=schema_tbl.schema_id and coalesce(schema_tbl.deleted,0)=0  where child_class_id = " + CStr(class_id)
        read_parent_class_links_for_class = gbc_db.executesql(sql, certificate)
    End Function
    Public Function read_attributes(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select label_value,attribute_id,type_id,is_lookup,coalesce(min_val,0),coalesce(max_val,0),coalesce(length,0),nullable,coalesce(lookup_sql,''),coalesce(multiple,0), coalesce(no_of_repeats,0),coalesce(show_workflow,0),coalesce(audited,0),coalesce(default_value,0),coalesce(submission_code,0), coalesce(is_def,0), coalesce(def_sql,'')  from attribute_tbl a, label_value_tbl l where a.label_code=l.label_code and language_code = 'en' and a.attribute_id not in (select distinct row_id from row_context_tbl where context_id=1) and coalesce(a.deleted,0)=0 order by label_value asc"
        read_attributes = gbc_db.executesql(sql, certificate)
    End Function
    Public Function read_attributes_for_class(ByVal class_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select a.attribute_id,attribute_order, permissions,a.name from class_attribute_tbl c, attribute_tbl a where class_id=" + CStr(class_id) + " and a.attribute_id=c.attribute_id and coalesce(a.deleted,0)=0 order by attribute_order asc"
        read_attributes_for_class = gbc_db.executesql(sql, certificate)
    End Function
    Public Function read_attributes_for_class_link(ByVal parent_class_id As Long, ByVal child_class_id As Long, ByVal schema_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select attribute_id,attribute_order, permissions from class_link_attribute_tbl where parent_class_id=" + CStr(parent_class_id) + " and child_class_id=" + CStr(child_class_id) + "  and schema_id=" + CStr(schema_id) + " order by attribute_order asc"
        read_attributes_for_class_link = gbc_db.executesql(sql, certificate)
    End Function
    Public Function read_attributes_info_class_link(ByVal parent_class_id As Long, ByVal child_class_id As Long, ByVal schema_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select a.attribute_id,a.submission_code,a.nullable,a.show_workflow from class_link_attribute_tbl l, attribute_tbl a where parent_class_id=" + CStr(parent_class_id) + " and child_class_id=" + CStr(child_class_id) + "   and schema_id=" + CStr(schema_id) + " and a.attribute_id=l.attribute_id order by attribute_order asc"
        read_attributes_info_class_link = gbc_db.executesql(sql, certificate)
    End Function
    'Public Function alternate_read_all_entities(ByRef certificate As bc_cs_security.certificate) As Object
    '    Dim sql As String
    '    sql = "exec dbo.bc_core_alternate_entity_list"
    '    alternate_read_all_entities = gbc_db.executesql(sql, certificate)
    'End Function
    Public Function alternate_read_all_entities(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        If bc_cs_central_settings.server_flag = 0 Then
            sql = "exec dbo.bc_core_alternate_entity_list " + CStr(bc_cs_central_settings.logged_on_user_id)
        Else
            sql = "exec dbo.bc_core_alternate_entity_list " + CStr(certificate.user_id)
        End If
        Dim res As Object
        res = gbc_db.executesql_show_no_error(sql)
        If IsArray(res) Then
            If CStr(res(0, 0)) <> "Error" Then
                alternate_read_all_entities = res
                Exit Function
            End If
        End If
        sql = "exec dbo.bc_core_alternate_entity_list"
        alternate_read_all_entities = gbc_db.executesql(sql, certificate)

    End Function
    Public Function read_all_entities_diffgram_mode(last_sync_date As DateTime, ByRef certificate As bc_cs_security.certificate) As Object
        Dim oparams As New List(Of bc_cs_db_services.bc_cs_sql_parameter)
        Dim oparam As New bc_cs_db_services.bc_cs_sql_parameter
        oparam.name = "last_sync_date "
        oparam.value = last_sync_date
        oparams.Add(oparam)
        read_all_entities_diffgram_mode = gbc_db.executesql_with_parameters("bc_core_get_entities_diffgram", oparams, certificate)
    End Function
    Public Function read_all_entities(ByRef certificate As bc_cs_security.certificate, Optional ByVal get_inactive As Boolean = False) As Object
        Dim sql As String

        '5.7.0.2 AAM apr 2017 move to sp
        If get_inactive = False Then
            sql = "exec dbo.bc_core_get_entities 0"
            'sql = " select e.entity_id, e.class_id, e.name, c.class_name,coalesce(e.inactive,0),coalesce(e.comment,'')  from entity_tbl e, entity_class_tbl c " _
            '               & " where c.class_id=e.class_id  and coalesce(e.deleted,0) = 0  and coalesce(e.inactive,0)=0 and coalesce(c.inactive,0)=0" _
            '               & " order by e.class_id, name asc "
        Else
            sql = "exec dbo.bc_core_get_entities 1"
            'sql = " select e.entity_id, e.class_id, e.name, c.class_name,coalesce(e.inactive,0),coalesce(e.comment,'')  from entity_tbl e, entity_class_tbl c " _
            '               & " where c.class_id=e.class_id  and coalesce(e.deleted,0) = 0" _
            '               & " order by e.class_id, name asc "
        End If
        'sql = "select entity_id, e.class_id, name, class_name  from entity_tbl e, entity_class_tbl c where c.class_id=e.class_id order by e.class_id, name asc"
        read_all_entities = gbc_db.executesql(sql, certificate)
    End Function
    Public Function read_all_disclosure_entities() As Object
        Dim sql As String
        'the following query has been modified to get active codes only
        sql = " select e.entity_id, e.class_id, e.name, c.class_name  from disclosure_entities e, entity_class_tbl c " _
                       & " where c.class_id=e.class_id  " _
                       & "   and e.entity_id not in (select entity_id from entity_attribute_value_tbl " _
                                             & " where attribute_id=(select attribute_id from attribute_tbl " _
                                             & " where label_code='attribute.entity.inactive') " _
                                             & " and attrib_value = '1') " _
                       & " and coalesce(e.inactive,0)=0 and coalesce(e.deleted,0)=0 order by e.class_id, name asc "
        'sql = "select entity_id, e.class_id, name, class_name  from entity_tbl e, entity_class_tbl c where c.class_id=e.class_id order by e.class_id, name asc"
        read_all_disclosure_entities = gbc_db.executesql_show_no_error(sql)
    End Function
    Public Function read_restrictions_for_entity(ByVal entity_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "exec dbo.bcc_core_res_build_wizard " + CStr(entity_id)
        read_restrictions_for_entity = gbc_db.executesql_show_no_error(sql)
    End Function
    REM FIL JUNE 2013
    Public Function write_user_attribute_value(ByVal user_id As Long, ByVal attribute_id As Long, ByVal value As String, ByVal submission_code As Integer, ByRef certificate As bc_cs_security.certificate) As String
        Dim sql As String
        Dim res As Object
        Dim suser_id As String
        If bc_cs_central_settings.server_flag = 1 Then
            suser_id = certificate.user_id
        Else
            suser_id = bc_cs_central_settings.logged_on_user_id
        End If
        sql = "exec dbo.bc_core_cp_write_user_attribute_value " + CStr(user_id) + "," + CStr(attribute_id) + ",'" + CStr(value) + "'," + CStr(suser_id) + "," + CStr(submission_code)
        res = gbc_db.executesql(sql, certificate)
        If IsArray(res) Then
            write_user_attribute_value = res(0, 0)
        Else
            write_user_attribute_value = ""
        End If
    End Function
    Public Function write_user_table_value(ByVal user_id As Long, ByVal field_name As String, ByVal value As String, ByRef certificate As bc_cs_security.certificate) As String
        Dim sql As String
        Dim res As Object
        Dim suser_id As String
        If bc_cs_central_settings.server_flag = 1 Then
            suser_id = certificate.user_id
        Else
            suser_id = bc_cs_central_settings.logged_on_user_id
        End If
        sql = "exec dbo.bc_core_cp_write_user_table_value " + CStr(user_id) + ",'" + CStr(field_name) + "','" + CStr(value) + "'," + CStr(suser_id)
        res = gbc_db.executesql(sql, certificate)
        If IsArray(res) Then
            write_user_table_value = res(0, 0)
        Else
            write_user_table_value = ""
        End If
    End Function

    Public Function write_attribute_value(ByVal entity_id As Long, ByVal submission_code As Long, ByVal attribute_id As Long, ByVal publish As Boolean, ByVal value As String, ByVal link_id As Long, ByRef certificate As bc_cs_security.certificate) As String
        Dim sql As String
        Dim res As Object
        Dim user_id As String
        If bc_cs_central_settings.server_flag = 1 Then
            user_id = certificate.user_id
        Else
            user_id = bc_cs_central_settings.logged_on_user_id
        End If
        sql = "exec dbo.bcc_core_cp_write_attribute_value " + CStr(entity_id) + "," + CStr(submission_code) + "," + CStr(attribute_id) + "," + CStr(publish) + ",'" + CStr(value) + "'," + CStr(link_id) + "," + CStr(user_id)
        res = gbc_db.executesql(sql, certificate)
        If IsArray(res) Then
            write_attribute_value = res(0, 0)
        Else
            write_attribute_value = ""
        End If
    End Function
    Public Sub change_entity_name(ByVal entity_id As Long, ByVal name As String, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        Dim user_id As String
        If bc_cs_central_settings.server_flag = 1 Then
            user_id = certificate.user_id
        Else
            user_id = bc_cs_central_settings.logged_on_user_id
        End If
        sql = "update entity_tbl set last_update_date=getutcdate(), name ='" + CStr(name) + "',comment=convert(varchar(20),getdate()), user_id=" + CStr(user_id) + " where entity_id = " + CStr(entity_id)
        gbc_db.executesql(sql, certificate)
    End Sub

    Public Sub New()

    End Sub
    Public Sub audit_entity_action(entity_id As Long, comment As String, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        Dim sr As New bc_cs_string_services(comment)
        comment = sr.delimit_apostrophies
        Dim user_id As Long
        If bc_cs_central_settings.server_flag = 0 Then
            user_id = bc_cs_central_settings.logged_on_user_id
        Else
            user_id = certificate.user_id
        End If
        sql = "exec dbo.bc_core_add_entity_user_audit " + CStr(entity_id) + ",1,'" + comment + "'," + CStr(user_id)
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Function set_default_attribute_values(entity_id As String, ByRef certificate As bc_cs_security.certificate)

        Dim user_id As Long
        If bc_cs_central_settings.server_flag = 0 Then
            user_id = bc_cs_central_settings.logged_on_user_id
        Else
            user_id = certificate.user_id
        End If
        gbc_db.executesql("exec dbo.bc_core_cp_set_def_att_values " + entity_id + "," + CStr(user_id), certificate)
    End Function
    Public Function add_entity(ByVal entity_name As String, ByVal class_id As String, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        If bc_cs_central_settings.server_flag = 1 Then
            sql = "insert into entity_tbl (entity_id, class_id, name, inactive, comment, deleted, user_id) select coalesce(max(entity_id),0) + 1 ," + CStr(class_id) + ", '" + entity_name + "',0,convert(varchar(20),getdate()),0," + CStr(certificate.user_id) + " from entity_tbl"
        Else
            sql = "insert into entity_tbl (entity_id, class_id, name, inactive, comment, deleted, user_id) select coalesce(max(entity_id),0) + 1 ," + CStr(class_id) + ", '" + entity_name + "',0,convert(varchar(20),getdate()),0," + CStr(bc_cs_central_settings.logged_on_user_id) + " from entity_tbl"
        End If
        gbc_db.executesql(sql, certificate)

        sql = "select max(entity_id) from entity_tbl"
        add_entity = gbc_db.executesql(sql, certificate)
    End Function
    Public Function read_schemas(ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "select schema_id,schema_name,coalesce(inactive,0),coalesce(core,0) from schema_tbl where coalesce(deleted,0)=0 order by schema_name asc"
        read_schemas = gbc_db.executesql(sql, certificate)
    End Function
    Public Sub audit_entity_links(entity_id As Long, schema_id As Long, ByRef certificate As bc_cs_security.certificate)
        Try
            Dim sql As String
            If bc_cs_central_settings.server_flag = 0 Then
                sql = "exec dbo.bc_core_set_audit_entity_links " + CStr(entity_id) + "," + CStr(schema_id)
            Else
                sql = "exec dbo.bc_core_set_audit_entity_links " + CStr(entity_id) + "," + CStr(schema_id)
            End If

            gbc_db.executesql(sql, certificate)
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_entity", "audit_entity_links", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try
    End Sub
    Public Sub update_rating(ByVal parent As Boolean, ByVal entity_id As Long, ByVal ass_entity_id As Long, ByVal rating As Integer, ByVal schema_id As Long, ByRef certificate As bc_cs_security.certificate)
        Try
            Dim sql As String
            If bc_cs_central_settings.server_flag = 0 Then
                sql = "exec dbo.bc_core_update_rating " + CStr(parent) + "," + CStr(rating) + "," + CStr(schema_id) + "," + CStr(ass_entity_id) + "," + CStr(entity_id) + "," + CStr(bc_cs_central_settings.logged_on_user_id)
            Else
                sql = "exec dbo.bc_core_update_rating " + CStr(parent) + "," + CStr(rating) + "," + CStr(schema_id) + "," + CStr(ass_entity_id) + "," + CStr(entity_id) + "," + CStr(certificate.user_id)
            End If

            gbc_db.executesql(sql, certificate)
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_entity", "update_rating", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try
    End Sub


    Public Function add_entity_link(ByVal parent_entity_id As Long, ByVal child_entity_id As Long, ByVal schema_id As Long, ByVal child_parent_rating As Integer, ByVal parent_child_rating As Integer, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        Dim user_id As Long
        REM if link already exists leave
        If bc_cs_central_settings.server_flag = 0 Then
            user_id = bc_cs_central_settings.logged_on_user_id
        Else
            user_id = certificate.user_id
        End If
        sql = "exec dbo.bcc_core_cp_add_entity_link " + CStr(schema_id) + "," + CStr(parent_entity_id) + "," + CStr(child_entity_id) + "," + CStr(child_parent_rating) + "," + CStr(parent_child_rating) + "," + CStr(user_id)
        add_entity_link = gbc_db.executesql(sql, certificate)
    End Function
    Public Sub delete_class_attribute(ByVal class_id As Long, ByVal attribute_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "delete from class_attribute_tbl where class_id=" + CStr(class_id) + " and attribute_id=" + CStr(attribute_id)
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Sub delete_upt_attribute(ByVal class_id As Long, ByVal attribute_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "exec dbo.bc_core_cp_delete_upt_attribute " + CStr(class_id) + " , " + CStr(attribute_id)
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Sub delete_class_link_attribute(ByVal class_id As Long, ByVal attribute_id As Long, ByVal parent_class_id As Long, ByVal schema_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "delete from class_link_attribute_tbl where child_class_id=" + CStr(class_id) + " and attribute_id=" + CStr(attribute_id) + " and parent_class_id=" + CStr(parent_class_id) + " and schema_id=" + CStr(schema_id)
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Sub set_schema_inactive(ByVal id As Long, ByVal inactive As Boolean, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        Dim user_id As String
        If bc_cs_central_settings.server_flag = 1 Then
            user_id = certificate.user_id
        Else
            user_id = bc_cs_central_settings.logged_on_user_id
        End If
        If inactive = False Then
            sql = "update schema_tbl set inactive=0, comment='set active ' + convert(varchar(20),getdate()),user_id=" + CStr(user_id) + " where schema_id=" + CStr(id)
        Else
            sql = "update schema_tbl set inactive=1, comment='set inactive ' + convert(varchar(20),getdate()),user_id=" + CStr(user_id) + " where schema_id=" + CStr(id)
        End If
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Sub set_class_inactive(ByVal id As Long, ByVal inactive As Boolean, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        Dim user_id As String
        If bc_cs_central_settings.server_flag = 1 Then
            user_id = certificate.user_id
        Else
            user_id = bc_cs_central_settings.logged_on_user_id
        End If
        If inactive = False Then
            sql = "update entity_class_tbl set inactive=0, comment='set active ' + convert(varchar(20),getdate()),user_id=" + CStr(user_id) + " where class_id=" + CStr(id)
        Else
            sql = "update entity_class_tbl set inactive=1, comment='set inactive ' + convert(varchar(20),getdate()),user_id=" + CStr(user_id) + " where class_id=" + CStr(id)
        End If
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Sub add_class_attribute(ByVal class_id As Long, ByVal attribute_id As Long, ByVal order As Long, ByVal permission As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "insert into class_attribute_tbl (class_id, attribute_id, attribute_order, permissions) values(" + CStr(class_id) + "," + CStr(attribute_id) + "," + CStr(order) + "," + CStr(permission) + ")"
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Sub add_upt_attribute(ByVal class_id As Long, ByVal attribute_id As Long, ByVal order As Long, ByVal permission As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "exec dbo.bc_core_cp_insert_upt_attribute " + CStr(class_id) + "," + CStr(attribute_id) + "," + CStr(order) + "," + CStr(permission)
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Sub add_class_link_attribute(ByVal class_id As Long, ByVal attribute_id As Long, ByVal order As Long, ByVal permission As Long, ByVal parent_class_id As Long, ByVal schema_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "insert into class_link_attribute_tbl (parent_class_id, child_class_id, schema_id, attribute_id, attribute_order, permissions) values(" + CStr(parent_class_id) + "," + CStr(class_id) + "," + CStr(schema_id) + "," + CStr(attribute_id) + "," + CStr(order) + "," + CStr(permission) + ")"
        gbc_db.executesql(sql, certificate)
    End Sub

    REM ING JUNE 2012
    Public Function add_attribute(ByVal name As String, ByVal type_id As Long, ByVal is_lookup As Boolean, ByVal nullable As Boolean, ByVal lookup_sql As String, ByVal repeats As Long, ByVal show_workflow As Boolean, ByVal audited As Boolean, ByVal submission_code As Long, ByVal multiple As Boolean, ByVal no_of_repeats As Long, ByVal length As Integer, is_def As Boolean, def_sql As String, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "exec dbo.bcc_core_cp_add_attribute '" + name + "'," + CStr(type_id) + "," + CStr(is_lookup) + "," + CStr(nullable) + ",'" + lookup_sql + "'," + CStr(multiple) + "," + CStr(no_of_repeats) + "," + CStr(show_workflow) + "," + CStr(audited) + "," + CStr(submission_code) + "," + CStr(length) + "," + CStr(is_def) + ",'" + CStr(def_sql) + "'"

        add_attribute = gbc_db.executesql(sql, certificate)
    End Function
    REM ----
    Public Sub update_entity_number(ByVal schema_id As Long, ByVal parent_class_id As Long, ByVal child_class_id As Long, ByVal num As Integer, ByVal pc As Boolean, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        If pc = True Then
            sql = "update class_link_tbl set mandatory_num_pc=" + CStr(num) + " where schema_id=" + CStr(schema_id) + " and parent_class_id=" + CStr(parent_class_id) + " and child_class_id=" + CStr(child_class_id)
        Else
            sql = "update class_link_tbl set mandatory_num_cp=" + CStr(num) + " where schema_id=" + CStr(schema_id) + " and parent_class_id=" + CStr(parent_class_id) + " and child_class_id=" + CStr(child_class_id)
        End If
        gbc_db.executesql(sql, certificate)
    End Sub
    REM ING JUNE 2012
    Public Sub modify_attribute(ByVal attribute_id As Long, ByVal name As String, ByVal type_id As Long, ByVal is_lookup As Boolean, ByVal nullable As Boolean, ByVal lookup_sql As String, ByVal repeats As Long, ByVal show_workflow As Boolean, ByVal audited As Boolean, ByVal submission_code As Long, ByVal multiple As Boolean, ByVal no_of_repeats As Long, ByVal length As Integer, is_def As Boolean, def_sql As String, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "exec dbo.bcc_core_cp_modify_attribute " + CStr(attribute_id) + ",'" + name + "'," + CStr(type_id) + "," + CStr(is_lookup) + "," + CStr(nullable) + ",'" + lookup_sql + "'," + CStr(multiple) + "," + CStr(no_of_repeats) + "," + CStr(show_workflow) + "," + CStr(audited) + "," + CStr(submission_code) + "," + CStr(length) + "," + CStr(is_def) + ",'" + CStr(def_sql) + "'"
        gbc_db.executesql(sql, certificate)
    End Sub
    REM ----
    Public Sub write_class_link(ByVal schema_id As Long, ByVal parent_class_id As Long, ByVal child_class_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "insert into class_link_tbl (schema_id, parent_class_id, child_class_id, mandatory_num_pc, mandatory_num_cp) values(" + CStr(schema_id) + "," + CStr(parent_class_id) + "," + CStr(child_class_id) + ",0,0)"
        gbc_db.executesql(sql, certificate)
    End Sub

    Public Sub delete_class_link(ByVal schema_id As Long, ByVal parent_class_id As Long, ByVal child_class_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "delete from  class_link_tbl where schema_id=" + CStr(schema_id) + " and parent_class_id=" + CStr(parent_class_id) + " and child_class_id=" + CStr(child_class_id)
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Sub change_entity_class(ByVal class_id As String, ByVal class_name As String, entry_description As String, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        Dim user_id As String
        If bc_cs_central_settings.server_flag = 1 Then
            user_id = certificate.user_id
        Else
            user_id = bc_cs_central_settings.logged_on_user_id
        End If
        sql = "update entity_class_tbl set class_name='" + class_name + "', comment=convert(varchar(20),getdate()), user_id=" + CStr(user_id) + ", entry_description='" + entry_description + "' where class_id=" + CStr(class_id)
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Function read_disclosure_tab_name() As Object
        Dim sql As String

        sql = "select tab_name from bc_core_disclosure_tab_name"
        read_disclosure_tab_name = gbc_db.executesql_show_no_error(sql)
    End Function
    Public Function add_entity_class(ByVal class_name As String, entry_description As String, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        Dim user_id As String
        If bc_cs_central_settings.server_flag = 1 Then
            user_id = certificate.user_id
        Else
            user_id = bc_cs_central_settings.logged_on_user_id
        End If
        sql = "insert into entity_class_tbl(class_id,class_name,inactive,deleted,comment,user_id,entry_description) select coalesce(max(class_id),0) + 1 ,'" + CStr(class_name) + "',0,0,convert(varchar(20),getdate())," + CStr(user_id) + ",'" + entry_description + "' from entity_class_tbl"
        gbc_db.executesql(sql, certificate)
        sql = "select max(class_id) from entity_class_tbl"
        add_entity_class = gbc_db.executesql(sql, certificate)
    End Function

    Public Function delete_entity_link(ByVal parent_entity_id As Long, ByVal child_entity_id As Long, ByVal schema_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "exec dbo.bcc_core_cp_delete_entity_link " + CStr(parent_entity_id) + "," + CStr(child_entity_id) + "," + CStr(schema_id)
        delete_entity_link = gbc_db.executesql(sql, certificate)
    End Function
    Public Function delete_schema(ByVal schema_id As Long, ByRef certificate As bc_cs_security.certificate) As String
        Dim sql As String
        If bc_cs_central_settings.server_flag = 0 Then
            certificate.user_id = bc_cs_central_settings.logged_on_user_id

        End If
        sql = "exec dbo.bcc_core_cp_generic_delete 'schema'," + CStr(schema_id) + "," + CStr(certificate.user_id)
        delete_schema = gbc_db.test_sql(sql, certificate)
    End Function
    Public Function change_entity_state(ByVal entity_id As Long, ByVal active As Boolean, ByRef certificate As bc_cs_security.certificate) As String
        Try
            Dim sql As String
            sql = ""
            Dim user_id As Long
            If active = False Then
                Dim tx As String = ""
                sql = "exec dbo.bc_core_check_can_deactivate " + CStr(entity_id)
                Dim res As Object
                res = gbc_db.executesql(sql, certificate)
                If IsArray(res) Then
                    If UBound(res, 2) = 0 Then
                        tx = res(0, 0)
                    End If
                End If
                If tx <> "" Then
                    change_entity_state = tx
                    Exit Function
                End If
            End If

            If bc_cs_central_settings.server_flag = 1 Then
                user_id = certificate.user_id
            Else
                user_id = bc_cs_central_settings.logged_on_user_id
            End If
            If active = False Then
                sql = "update entity_tbl set last_update_date=getutcdate(), inactive=1, comment='set inactive ' + convert(varchar(40),getdate()),user_id=" + CStr(user_id) + " where entity_id=" + CStr(entity_id)
            Else
                sql = "update entity_tbl set last_update_date=getutcdate(), inactive=0, comment='set active ' + convert(varchar(40),getdate()),user_id=" + CStr(user_id) + " where entity_id=" + CStr(entity_id)
            End If
            gbc_db.executesql(sql, certificate)
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_entity_db", "change_entity_state", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        End Try
    End Function
    Public Function get_audit_id(ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "exec dbo.bc_core_cp_get_audit_id"
        get_audit_id = gbc_db.executesql(sql, certificate)
    End Function
    Public Sub delete_user_prefs(audit_id As Long, ByVal entity_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        Dim user_id As Long
        If bc_cs_central_settings.server_flag = 0 Then
            user_id = bc_cs_central_settings.logged_on_user_id
        Else
            user_id = certificate.user_id
        End If
        sql = "exec dbo.bc_core_delete_user_prefs " + CStr(audit_id) + "," + CStr(entity_id)
        'sql = "delete from apref_entity_tbl where entity_id=" + CStr(entity_id)
        gbc_db.executesql(sql, certificate)
    End Sub
    REM PR AUG7
    Public Sub add_user_pref(audit_id As Long, ByVal entity_id As Long, ByVal opref As bc_om_user_pref, ByVal rating As Integer, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        Dim user_id As Long
        If bc_cs_central_settings.server_flag = 0 Then
            user_id = bc_cs_central_settings.logged_on_user_id
        Else
            user_id = certificate.user_id
        End If
        sql = "exec dbo.bc_core_add_user_pref " + CStr(audit_id) + "," + CStr(opref.user_id) + "," + CStr(entity_id) + "," + CStr(opref.rating) + "," + CStr(opref.pref_type) + "," + CStr(user_id)
        REM sql = "insert into apref_entity_tbl (user_id,entity_id,rating,date_from,preference_type_id) values(" + CStr(opref.user_id) + "," + CStr(entity_id) + "," + CStr(opref.rating) + ",getutcdate()," + CStr(opref.pref_type) + ")"
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Function delete_attribute(ByVal attribute_id As Long, ByRef certificate As bc_cs_security.certificate) As String
        Dim sql As String
        Dim user_id As String
        If bc_cs_central_settings.server_flag = 1 Then
            user_id = certificate.user_id
        Else
            user_id = bc_cs_central_settings.logged_on_user_id
        End If
        sql = "exec dbo.bcc_core_cp_generic_delete 'attribute'," + CStr(attribute_id) + "," + CStr(user_id)
        delete_attribute = gbc_db.test_sql(sql, certificate)
    End Function
    Public Function delete_entity(ByVal entity_id As Long, ByRef certificate As bc_cs_security.certificate) As String
        Dim sql As String
        Dim user_id As String
        If bc_cs_central_settings.server_flag = 1 Then
            user_id = certificate.user_id
        Else
            user_id = bc_cs_central_settings.logged_on_user_id
        End If
        sql = "exec dbo.bcc_core_cp_generic_delete 'entity'," + CStr(entity_id) + "," + CStr(user_id)
        delete_entity = gbc_db.test_sql(sql, certificate)
    End Function
    Public Function delete_entity_class(ByVal class_id As Long, ByRef certificate As bc_cs_security.certificate) As String
        Dim sql As String
        Dim user_id As String
        If bc_cs_central_settings.server_flag = 1 Then
            user_id = certificate.user_id
        Else
            user_id = bc_cs_central_settings.logged_on_user_id
        End If
        sql = "exec dbo.bcc_core_cp_generic_delete 'class'," + CStr(class_id) + "," + CStr(user_id)
        delete_entity_class = gbc_db.test_sql(sql, certificate)
    End Function


    Public Function read_attribute_value(ByVal entity_id As Long, ByVal attribute_Id As Long, ByVal submission_code As Long, ByVal link_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "exec dbo.bcc_core_cp_read_attribute_value " + CStr(entity_id) + "," + CStr(attribute_Id) + "," + CStr(submission_code) + "," + CStr(link_id)
        read_attribute_value = gbc_db.executesql(sql, certificate)
    End Function
    Public Function add_schema(ByVal schema_name As String, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        Dim user_id As String
        If bc_cs_central_settings.server_flag = 1 Then
            user_id = certificate.user_id
        Else
            user_id = bc_cs_central_settings.logged_on_user_id
        End If
        sql = "insert into schema_tbl (schema_id, schema_name, internal, inactive, deleted, comment, user_id) select coalesce(max(schema_id),0)+1,'" + schema_name + "',0,0,0,convert(varchar(20),getdate())," + CStr(user_id) + " from schema_tbl"
        gbc_db.executesql(sql, certificate)
        sql = "select max(schema_id) from schema_tbl"
        add_schema = gbc_db.executesql(sql, certificate)
    End Function
    Public Sub update_schema_name(ByVal schema_id As Long, ByVal schema_name As String, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        Dim user_id As String
        If bc_cs_central_settings.server_flag = 1 Then
            user_id = certificate.user_id
        Else
            user_id = bc_cs_central_settings.logged_on_user_id
        End If
        sql = "update schema_tbl set schema_name='" + schema_name + "', comment=convert(varchar(20),getdate()), user_id=" + CStr(user_id) + " where schema_id=" + CStr(schema_id)
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Function get_lookup_values(ByVal sql As String, ByRef certificate As bc_cs_security.certificate)
        get_lookup_values = gbc_db.executesql_show_no_error(sql)
    End Function

    Public Function get_parent_entities_for_entity(ByVal entity_id As Long, ByVal schema_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "select e.entity_id, e.class_id, l.entity_link_id,e.name,c.class_name,coalesce(e.inactive,0), coalesce(l.child_parent_rating,1) from entity_tbl e " + _
              " inner join entity_link_tbl l on l.parent_entity_id=e.entity_id and l.schema_id=" + CStr(schema_id) + _
              " inner join entity_class_tbl c on c.class_id=e.class_id" + _
              " where l.child_entity_id = " + CStr(entity_id) + " and coalesce(e.deleted,0)=0" + _
              " and coalesce(e.deleted,0)=0 order by class_name asc,parent_child_rating asc,name asc"
        'sql = "select e.entity_id, e.class_id, l.entity_link_id,e.name,c.class_name from entity_link_tbl l, entity_tbl e where child_entity_id=" + CStr(entity_id) + " and l.parent_entity_id=e.entity_id and schema_id=" + CStr(schema_id) + " and coalesce(e.deleted,0)=0"
        get_parent_entities_for_entity = gbc_db.executesql(sql, certificate)
    End Function
    Public Function get_child_entities_for_entity(ByVal entity_id As Long, ByVal schema_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "select e.entity_id, e.class_id, l.entity_link_id,e.name,c.class_name,coalesce(e.inactive,0), coalesce(l.parent_child_rating,1) from entity_tbl e " + _
              " inner join entity_link_tbl l on l.child_entity_id=e.entity_id and l.schema_id=" + CStr(schema_id) + _
              " inner join entity_class_tbl c on c.class_id=e.class_id" + _
              " where l.parent_entity_id = " + CStr(entity_id) + " and coalesce(e.deleted,0)=0" + _
              " and  coalesce(e.deleted,0)=0 order by class_name, child_parent_rating asc, name asc"

        'sql = "select e.entity_id, e.class_id, l.entity_link_id from entity_link_tbl l, entity_tbl e where parent_entity_id=" + CStr(entity_id) + " and l.child_entity_id=e.entity_id and schema_id=" + CStr(schema_id) + " and coalesce(e.deleted,0)=0"
        get_child_entities_for_entity = gbc_db.executesql(sql, certificate)
    End Function
    Public Function get_user_prefs(ByVal entity_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        REM FIL July 2012
        sql = "select user_id,a.preference_type_id,p.preference_type_name  from apref_entity_tbl a " + _
              " inner join bc_core_cp_preference_types p on p.preference_type_id =a.preference_type_id" + _
              " where a.entity_id=" + CStr(entity_id) + " order by a.preference_type_id,rating asc"

        get_user_prefs = gbc_db.executesql(sql, certificate)
    End Function




    'Public Sub get_aggregation_preview_async(ByRef obj As bc_cs_soap_base_class, ByVal aggregationId As Long, ByRef certificate As bc_cs_security.certificate)
    '    Dim sql As String
    '    sql = "bc_core_aggregate_universe " & aggregationId & ",1"
    '    REM get_aggregation_preview = 
    '    REM gbc_db.executesql(sql, certificate)
    '    gbc_db.executesql_async(sql, certificate)
    'End Sub
    Public Function get_aggregation_preview(ByVal aggregationId As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        Dim vres As Object
        sql = "bc_core_aggregate_universe " & aggregationId & ",1"
        vres = gbc_db.executesql_show_no_error(sql)

        If IsArray(vres) Then
            If CStr(vres(0, 0)) = "Error" Then
                If InStr(CStr(vres(1, 0)), "Timeout expired") > 0 Then
                    vres(0, 0) = "Unable to preview due to volume of calculations and size of universe." + vbCr + "Please use the run now command and view the results via the Extract Tools."
                Else
                    vres(0, 0) = "There was a database error executing this Aggregation Universe: " + CStr(vres(1, 0))
                End If
            End If
        End If
        get_aggregation_preview = vres

    End Function
    Public Sub run_now(ByVal aggregationId As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "bc_core_aggregate_universe " & aggregationId & ""
        REM if SOAP mode run thsi asynchronouslt as as web call is asynchronous
        REM if ado mode run sp asynchronously
        If bc_cs_central_settings.server_flag = 1 Then
            gbc_db.executesql_extended_timeout(sql, certificate)
        Else
            'gbc_db.executesql_async_no_callback(sql, certificate)
            gbc_db.executesql_async(sql, certificate)
        End If
    End Sub
End Class
Public Class bc_om_entity_link_db
    Private gbc_db As New bc_cs_db_services
    REM reads all pub type in database
    Public Function read_all_entity_links(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select child_entity_id, parent_entity_id,schema_id from entity_link_tbl order by child_entity_id asc"
        read_all_entity_links = gbc_db.executesql(sql, certificate)
    End Function

End Class

